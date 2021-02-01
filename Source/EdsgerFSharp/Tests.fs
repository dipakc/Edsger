module EdsgerFSharp.Tests

open Edsger.Types
open Edsger.ProgramImporter
open EdsgerFSharp.Transformations

open Microsoft.Boogie
open NUnit.Framework

[<Test>]
let CallBoogie() =
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
    let program = ExecutionEngine.ParseBoogieProgram(fileNames, false)
    Assert.AreEqual(4, 2+2)        
    
[<Test>]
let BoogieToEdsger()=
    let fileNames = ResizeArray<string>()
    //fileNames.Add("/Users/dipakc/SourceRepos/Edsger/Source/EdsgerTests/TestData/CSharpToFSharp.bpl")
    //fileNames.Add("/Users/dipakc/SourceRepos/Edsger/Source/EdsgerTests/TestData/DivMod1.bpl")
    //fileNames.Add("/Users/dipakc/SourceRepos/Edsger/Source/EdsgerTests/TestData/ParseTest.bpl")
    fileNames.Add("/Users/dipakc/SourceRepos/Edsger/Source/EdsgerTests/TestData/DivModDeriv01.bpl")
    
    // ExecutionEngine.printer = new ConsolePrinter();

    CommandLineOptions.Install(new CommandLineOptions());
    let program = ExecutionEngine.ParseBoogieProgram(fileNames, false)
    
    let edProgram = ToEDProgram program
    printfn "%s" (StringOfProgram 0 edProgram)
    Assert.AreEqual(2, 2)
    
    
[<Test>]
let IntroWhileTest()=
    let fileNames = ResizeArray<string>()

    fileNames.Add("/Users/dipakc/SourceRepos/Edsger/Source/EdsgerTests/TestData/DivModDeriv01.bpl")
    
    // ExecutionEngine.printer = new ConsolePrinter();

    CommandLineOptions.Install(new CommandLineOptions());
    let program = ExecutionEngine.ParseBoogieProgram(fileNames, false)
    
    let p1 = ToEDProgram program
    printfn "%s" (StringOfProgram 0 p1)
    //--------
    let _, p2 = TopDownOnce (Util.lift IntroWhileRule) p1
    let p2 = p2 :?> EDProgram 
    //-------
    
    printfn "%s" (StringOfProgram 0 p2)
    Assert.AreEqual(2, 2)
    
    
    