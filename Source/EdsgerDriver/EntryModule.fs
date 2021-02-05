module Edsger.EntryModule

open EdsgerWrapper
open EdsgerDriver.Utils
open Edsger.Types
open Edsger.ProgramImporter
open EdsgerDriver.Transformations
open Argu
open System
open Microsoft.Boogie
open NUnit.Framework

let GetEDProgram (fileName: string) =
    let fileNames = ResizeArray<string>()

    fileNames.Add(fileName)

    // ExecutionEngine.printer = new ConsolePrinter();

    CommandLineOptions.Install(new CommandLineOptions())

    let program =
        ExecutionEngine.ParseBoogieProgram(fileNames, false)

    ToEDProgram program


let ConjunctAsInv (fileName) =

    let p1 = GetEDProgram fileName

    let _, p2 =
        TopDownOnce (Util.lift IntroWhileRule) p1

    let p2 = p2 :?> EDProgram


    printfn "%s" (StringOfProgram 0 p2)

let ReplaceTermWithVar (fileName) =
    let p1 = GetEDProgram fileName

    ()

let IntroIf (fileName) = Unk


let Synthesize (fileName, synthGrammarFile) =
    let args =
        [| "/env:0"
           "/printVerifiedProceduresCount:0"
           //"/errorTrace:0"
           "/nologo"
           //"/print:-"
           //"/print:./tmp/boogie_log.txt"
           "/proverOpt:SOLVER=CVC4"
           //"/proverOpt:PROVER_PATH=<cvc4 path>"
           //"/proverLog:./Source/EdsgerTests/TestOutput/@PREFIX@_@PROC@.sygus"
           "/synth"
           //"/synthGrammar:./Test/grammar2.txt"
           "/synthGrammar:" + synthGrammarFile
           fileName |]

    OnlyBoogie.Main(args) |> ignore
    ()

let Verify (fileName) =

    let args =
        [| "/env:0"
           "/printVerifiedProceduresCount:0"
           //"/errorTrace:0"
           "/nologo"
           //"/print:-"
           //"/print:./tmp/boogie_log.txt"
           "/proverOpt:SOLVER=CVC4"
           //"/proverOpt:PROVER_PATH=<cvc4 path>"
           //"/proverLog:./Source/EdsgerTests/TestOutput/@PREFIX@_@PROC@.sygus"
           fileName |]

    OnlyBoogie.Main(args) |> ignore
    ()

type CliArguments =
    | File of filepath: string
    | Tactic of tactic_name: string
    | Synth_Grammar of synth_grammar_file: string

    interface IArgParserTemplate with
        member s.Usage =
            match s with
            | File _ -> "Specify the file name"
            | Tactic _ -> "Tactic name"
            | Synth_Grammar _ -> "Specify synth grammar file"

[<EntryPoint>]
let main argv =

    let errorHandler = ProcessExiter(colorizer = function | ErrorCode.HelpText -> None | _ -> Some ConsoleColor.Red)

    let parser = ArgumentParser.Create<CliArguments>(programName = "EdsgerDriver", errorHandler = errorHandler)

    let results = parser.Parse(argv)
    //let all = results.GetAllResults()

    let filepath = (results.GetResults File).Head
    let tactic_name = (results.GetResults Tactic).Head


    match tactic_name with
    | "ConjunctAsInv" -> ConjunctAsInv(filepath)
    | "IntroIf" -> IntroIf(filepath)
    | "Synthesize" ->
        let synth_grammar_file = (results.GetResults Synth_Grammar).Head
        Synthesize(filepath, synth_grammar_file)
    | "Verify" -> Verify(filepath)
    | "ReplaceTermWithVar" -> ReplaceTermWithVar(filepath)
    | _ -> failwith ("unknown tactic: " + tactic_name)

    0
