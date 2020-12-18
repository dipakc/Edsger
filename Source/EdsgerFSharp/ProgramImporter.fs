module Edsger.ProgramImporter

open System
open System.Reflection.Emit
open Edsger.Types
open Microsoft.Boogie

open EdsgerFSharp.Utils

let ToEdOpcode(opCode: BinaryOperator.Opcode): EDOpcode =
    match opCode with
    | BinaryOperator.Opcode.Add -> Add 
    | BinaryOperator.Opcode.Sub -> Sub 
    | BinaryOperator.Opcode.Mul -> Mul 
    | BinaryOperator.Opcode.Div -> Div 
    | BinaryOperator.Opcode.Mod -> Mod 
    | BinaryOperator.Opcode.RealDiv -> RealDiv 
    | BinaryOperator.Opcode.FloatDiv -> FloatDiv 
    | BinaryOperator.Opcode.Pow -> Pow 
    | BinaryOperator.Opcode.Eq -> Eq 
    | BinaryOperator.Opcode.Neq -> Neq 
    | BinaryOperator.Opcode.Gt -> Gt 
    | BinaryOperator.Opcode.Ge -> Ge 
    | BinaryOperator.Opcode.Lt -> Lt 
    | BinaryOperator.Opcode.Le -> Le 
    | BinaryOperator.Opcode.And -> And 
    | BinaryOperator.Opcode.Or -> Or 
    | BinaryOperator.Opcode.Imp -> Imp 
    | BinaryOperator.Opcode.Iff -> Iff 
    | BinaryOperator.Opcode.Subtype -> Subtype
    | _ -> Unk
    
    
let rec ToEDCmd (cmd: Cmd): EDCmd =
    match cmd with
    | :? AssignCmd as asgn -> EDAssignCmd(asgn.ToString())
    | :? PredicateCmd as predCmd -> EDPredicateCmdW(ToEDPredicateCmd(predCmd))        
    | _ -> Unk

and ToEDPredicateCmd (predCmd: PredicateCmd): EDPredicateCmd =
    match predCmd with
    | :? AssertCmd as assertCmd ->
        match assertCmd with
        | :? AssertEnsuresCmd as a ->
            EDAssertCmdW(EDAssertEnsuresCmd(ToEDExpr(a.Expr)))
        | :? AssertRequiresCmd as a ->
            EDAssertCmdW(EDAssertRequiresCmd(ToEDExpr(a.Expr)))
        | :? LoopInitAssertCmd as a ->
            EDAssertCmdW(EDLoopInitAssertCmd(ToEDExpr(a.Expr)))
        | :? LoopInvMaintainedAssertCmd as a ->
            EDAssertCmdW(EDLoopInvMaintainedAssertCmd(ToEDExpr(a.Expr)))
        | _ as a ->
            EDAssertCmdW(EDAssertBasicCmd(ToEDExpr(a.Expr)))
    | :? AssumeCmd as assumeCmd ->
        EDAssumeCmd(ToEDExpr(assumeCmd.Expr))
    | _ -> Unk

and ToEDExpr (expr: Expr): EDExpr =
    match expr with
    | :? BinderExpr as be -> EDBinderExprW(ToEDBinderExpr(be))
    | :? BvConcatExpr -> EDBvConcatExpr(expr.ToString())
    | :? BvExtractExpr -> EDBvExtractExpr(expr.ToString())
    | :? CodeExpr -> EDCodeExpr(expr.ToString())
    | :? ExprContracts -> EDExprContracts(expr.ToString())
    | :? IdentifierExpr -> EDIdentifierExpr(expr.ToString())
    | :? LetExpr -> EDLetExpr(expr.ToString())
    | :? LiteralExpr -> EDLiteralExpr(expr.ToString())
    | :? NAryExpr as naryExpr ->
        let func: EDApplicable = ToEDApplicable(naryExpr.Fun)

        let args: EDExpr list =
            naryExpr.Args |> List.ofSeq |> List.map ToEDExpr

        EDNAryExpr(func, args)
    | :? OldExpr -> EDOldExpr(expr.ToString())
    | _ -> Unk

and ToEDApplicable (applicable: IAppliable): EDApplicable =
    match applicable with
    | :? ArithmeticCoercion as x -> EDArithmeticCoercion(x.ToString())
    | :? BinaryOperator as binOp ->
        let Op: EDOpcode = ToEdOpcode(binOp.Op)
        let FunctionName: string = binOp.FunctionName
        let ArgumentCount: int = binOp.ArgumentCount
        EDBinaryOperator(Op, FunctionName, ArgumentCount)
    | :? FunctionCall as x -> EDFunctionCall(x.ToString())
    | :? IfThenElse as x -> EDIfThenElse(x.ToString())
    | :? MapSelect as x -> EDMapSelect(x.ToString())
    | :? MapStore as x -> EDMapStore(x.ToString())
    | :? TypeCoercion as x -> EDTypeCoercion(x.ToString())
    | :? UnaryOperator as x -> EDUnaryOperator(x.ToString())
    | _ -> Unk


and ToEDBinderExpr (be: BinderExpr): EDBinderExpr =
    match be with
    | :? LambdaExpr -> EDLambdaExpr(be.ToString())
    | :? QuantifierExpr as qe ->
        match qe with
        | :? ExistsExpr as existsExpr -> EDQuantifierExprW(EDExistsExpr(existsExpr.ToString()))
        | :? ForallExpr as forallExpr -> EDQuantifierExprW(EDForallExpr(forallExpr.ToString()))
        | _ -> Unk
    | _ -> Unk


and ToEDStructuredCmd (structuredCmd: StructuredCmd): EDStructuredCmd =
    match structuredCmd with
    | :? WhileCmd as whilecmd ->

        let Invariants: EDPredicateCmd list =
            whilecmd.Invariants
            |> Seq.toList
            |> List.map ToEDPredicateCmd

        let Guard: EDExpr = ToEDExpr whilecmd.Guard

        //let Body: EDBigBlockB list =
        //    whilecmd.Body.BigBlocks
        //    |> Seq.toList
        //    |> List.map ToEDBigBlockB

        let Body: IEDBigBlock list =
            whilecmd.Body.BigBlocks
            |> Seq.toList
            |> List.map ToEDBigBlockFlat
            |> List.map (fun x -> x :> IEDBigBlock)

        EDWhileCmd(Invariants, Guard, Body)
    | _ -> Unk

and ToEDTransferCmd (transferCmd: TransferCmd): EDTransferCmd = Unk

and ToEDBigBlockB (bigBlock: BigBlock): EDBigBlockB =

    let simpleCmds: EDCmd list =
        bigBlock.simpleCmds
        |> List.ofSeq
        |> List.map ToEDCmd

    let ec: EDStructuredCmd option =
        match box bigBlock.ec with
        | null -> None
        | _ -> Some(ToEDStructuredCmd(bigBlock.ec))

    let tc: EDTransferCmd option =
        match box bigBlock.tc with
        | null -> None
        | _ -> Some(ToEDTransferCmd(bigBlock.tc))

    EDBigBlockB(simpleCmds, ec, tc)

and ToEDBigBlockFlat (bigBlock: BigBlock): EDBigBlockFlat =

    let simpleCmds: EDCmd list =
        bigBlock.simpleCmds
        |> List.ofSeq
        |> List.map ToEDCmd

    let ec: IEDMixedCmd list =
        match box bigBlock.ec with
        | null -> []
        | _ -> [ToEDStructuredCmd(bigBlock.ec)]

    let tc: IEDMixedCmd list =
        match box bigBlock.tc with
        | null -> []
        | _ -> [ToEDTransferCmd(bigBlock.tc)]

    let simpleCmds = simpleCmds |> List.map (fun x -> x :> IEDMixedCmd)            

    EDBigBlockFlat(simpleCmds @ ec @ tc)

and ToEDDecl (decl: Declaration): EDDeclaration =
    match decl with
    | :? Procedure as proc -> EDProcedure(proc.ToString())
    | :? Implementation as impl ->
        impl.StructuredStmts.BigBlocks
        |> Seq.toList
        |> List.map ToEDBigBlockFlat
        |> List.map (fun x -> x :> IEDBigBlock)
        |> EDImplementation
    | :? Function as f -> EDFunction(f.ToString())
    | _ -> Unk


and ToEDProgram (prog: Program): EDProgram =
    EDProgram
        (prog.TopLevelDeclarations
         |> Seq.toList
         |> List.map ToEDDecl)
