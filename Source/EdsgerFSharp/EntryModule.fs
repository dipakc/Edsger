   
module Edsger.EntryModule
open EdsgerDriver
open EdsgerFSharp.Utils
open Edsger.Types
open Edsger.ProgramImporter
open EdsgerFSharp.Transformations

open System
open Microsoft.Boogie

let GetEDProgram (fileName: string) =
    let fileNames = ResizeArray<string>()

    fileNames.Add(fileName)
    
    // ExecutionEngine.printer = new ConsolePrinter();

    CommandLineOptions.Install(new CommandLineOptions());
    let program = ExecutionEngine.ParseBoogieProgram(fileNames, false)
    
    ToEDProgram program
    

let ConjunctAsInv (fileName) = 
    
    let p1 = GetEDProgram fileName
    
    let _, p2 = TopDownOnce (Util.lift IntroWhileRule) p1
    
    let p2 = p2 :?> EDProgram 
    
    
    printfn "%s" (StringOfProgram 0 p2)


let IntroIf (fileName) =
    Unk

(*
    /errorTrace:0
    /nologo
    /print:-
    /proverOpt:SOLVER=CVC4
    /proverOpt:PROVER_PATH=/Users/dipakc/progs/cvc4_1_8/cvc4_wrapper.sh
    /proverLog:/Users/dipakc/SourceRepos/Edsger/Source/EdsgerTests/TestOutput/@PREFIX@_@PROC@.sygus
    /synth
    /synthGrammar:/Users/dipakc/SourceRepos/Boogie/Source/Synthesis/Test/grammar.txt
    /Users/dipakc/SourceRepos/Edsger/Source/EdsgerTests/TestData/DivMod1.bpl
*)    

let Synthesize (fileName) =
    let args =
        [|
          "/env:0"
          "/printVerifiedProceduresCount:0"
          //"/errorTrace:0"
          "/nologo"
          //"/print:-"
          "/print:/Users/dipakc/tmp/boogie_log.txt"
          "/proverOpt:SOLVER=CVC4"
          "/proverOpt:PROVER_PATH=/Users/dipakc/progs/cvc4_1_8/cvc4_wrapper.sh"
          "/proverLog:/Users/dipakc/SourceRepos/Edsger/Source/EdsgerTests/TestOutput/@PREFIX@_@PROC@.sygus"
          "/synth"
          "/synthGrammar:/Users/dipakc/SourceRepos/Boogie/Source/Synthesis/Test/grammar.txt"
          fileName        
        |]
        
    OnlyBoogie.Main(args) |> ignore
    ()

let Verify (fileName) =
        
    let args =
        [|
          "/env:0"
          "/printVerifiedProceduresCount:0"
          //"/errorTrace:0"
          "/nologo"
          //"/print:-"
          "/print:/Users/dipakc/tmp/boogie_log.txt"
          "/proverOpt:SOLVER=CVC4"
          "/proverOpt:PROVER_PATH=/Users/dipakc/progs/cvc4_1_8/cvc4_wrapper.sh"
          "/proverLog:/Users/dipakc/SourceRepos/Edsger/Source/EdsgerTests/TestOutput/@PREFIX@_@PROC@.sygus"
          //"/synth"
          //"/synthGrammar:/Users/dipakc/SourceRepos/Boogie/Source/Synthesis/Test/grammar.txt"
          fileName
          |]
        
    OnlyBoogie.Main(args) |> ignore
    ()


[<EntryPoint>]
let main argv =
    //printfn "Hello World from F#!"
    //printfn "%A" argv
    
    let fileName = argv.[1]
    let tactic = argv.[3]
    
    match tactic with
    | "ConjunctAsInv" -> ConjunctAsInv (fileName)
    | "IntroIf" -> IntroIf (fileName)
    | "Synthesize" -> Synthesize (fileName)
    | "Verify" -> Verify (fileName)
    | _ -> failwith ("unknown tactic: " + tactic)
        
    
    let args: string [] = [|
        "/errorTrace:0" 
        "/nologo" 
        "/print:-" 
        "/proverOpt:SOLVER=CVC4"
        "/proverOpt:PROVER_PATH=/Users/dipakc/progs/cvc4_1_8/cvc4_wrapper.sh" 
        "/proverLog:/Users/dipakc/SourceRepos/Edsger/Source/EdsgerTests/TestOutput/@PREFIX@_@PROC@.smt" 
        "/logPrefix:CSharpToFSharp" 
        "/trace" 
        "/Users/dipakc/SourceRepos/Edsger/Source/EdsgerTests/TestData/CSharpToFSharp.bpl"             
    |]
    // let result = OnlyBoogie.Main(args)
    let fileNames = ResizeArray<string>()
    fileNames.Add("/Users/dipakc/SourceRepos/Edsger/Source/EdsgerTests/TestData/CSharpToFSharp.bpl")

//    ExecutionEngine.printer = new ConsolePrinter();

    CommandLineOptions.Install(new CommandLineOptions());

    let program = ExecutionEngine.ParseBoogieProgram(fileNames, false)
    
    0 // return an integer exit code
