module Edsger.Types

open Microsoft.Boogie
open EdsgerFSharp.Utils

type IEDBase = interface end
type IEDMixedCmd = interface
                       inherit IEDBase end
type IEDBigBlock =
    interface
        inherit IEDBase
        end

type EDOpcode =
    | Add | Sub | Mul | Div | Mod | RealDiv | FloatDiv | Pow
    | Eq | Neq | Gt | Ge | Lt | Le | And | Or | Imp | Iff | Subtype
    
type EDAbsy =
    | EDAbsyContracts
    | EDAssignLhsW of EDAssignLhs
    | EDBlock
    | EDCmdW of EDCmd
    | EDDeclarationW of EDDeclaration
    | EDEnsures
    | EDExprW of EDExpr
    | EDProgramW of EDProgram
    | EDQKeyValue
    | EDRequires
    | EDTransferCmdW of EDTransferCmd
    | EDTrigger
    | EDTypeW of EDType
    | EDTypedIdent
    interface IEDBase
    
and EDProgram =
    | EDProgram of TopLevelDeclarations: EDDeclaration list
    interface IEDBase
    
and EDDeclaration =
    | EDProcedure of string
    | EDImplementation of bigBlocks: IEDBigBlock list    
    | EDFunction of string
    interface IEDBase
    
and EDBigBlockB =
    | EDBigBlockB of simpleCmds: EDCmd list * ec: EDStructuredCmd option * tc: EDTransferCmd option
    interface IEDBase
    interface IEDBigBlock

and EDBigBlockFlat =  //Edsger specific
    | EDBigBlockFlat of cmds: IEDMixedCmd list
    interface IEDBase
    interface IEDBigBlock
    
and EDCmd =
    | EDAssignCmd of string
    | EDCmdContracts
    | EDCommentCmd of string
    | EDHavocCmd
    | EDPredicateCmdW of EDPredicateCmd 
    | EDRE
    | EDStateCmdW of EDStateCmd
    | EDSugaredCmd
    | EDYieldCmd
    interface IEDBase
    interface IEDMixedCmd
    
   
and EDStructuredCmd =
    | EDBreakCmd
    | EDIfCmd
    | EDStructuredCmdContracts
    | EDWhileCmd of Invariants: EDPredicateCmd list * Guard: EDExpr * Body: IEDBigBlock list
    | EDComposition of cmds:IEDMixedCmd list//Edsger Specific
    interface IEDBase
    interface IEDMixedCmd
    
and EDAssignLhs =
    | EDAssignLhsContracts
    | EDMapAssignLhs
    | EDSimpleAssignLhs
    interface IEDBase
    
and EDPredicateCmd =
    | EDAssertCmdW of EDAssertCmd
    | EDAssumeCmd of expr: EDExpr
    interface IEDBase
    
and EDAssertCmd =
    | EDAssertBasicCmd of expr: EDExpr
    | EDAssertEnsuresCmd of expr: EDExpr
    | EDAssertRequiresCmd of expr: EDExpr
    | EDLoopInitAssertCmd of expr: EDExpr
    | EDLoopInvMaintainedAssertCmd of expr: EDExpr
    interface IEDBase
    
and EDStateCmd =
    | EDCallCmd
    | EDParCallCmd
    | EDSugaredCmdContract
    interface IEDBase
    
and EDExpr =
    | EDBinderExprW of EDBinderExpr
    | EDBvConcatExpr of string
    | EDBvExtractExpr of string
    | EDCodeExpr of string
    | EDExprContracts of string
    | EDIdentifierExpr of string
    | EDLetExpr of string
    | EDLiteralExpr of string
    | EDNAryExpr of func: EDApplicable * args: EDExpr list 
    | EDOldExpr of string
    interface IEDBase
    
and EDBinderExpr =
    | EDLambdaExpr of string
    | EDQuantifierExprW of EDQuantifierExpr
    interface IEDBase
    
and EDQuantifierExpr =
    | EDExistsExpr of string
    | EDForallExpr of string
    interface IEDBase
    
and EDTransferCmd =
    | EDGotoCmd
    | EDReturnCmd
    | EDReturnExprCmd
    | EDTransferCmdContracts
    interface IEDBase
    interface IEDMixedCmd
    
    
and EDType =
    | EDBasicType
    | EDBvType
    | EDCtorType
    | EDFloatType
    | EDMapType
    | EDTypeContracts
    | EDTypeProxy
    | EDConstraintedProxyW of EDConstrainedProxy 
    | EDTypeSynonymAnnortation
    | EDTypeVariable
    | EDUnresolvedTypeIdentifier
    interface IEDBase
    
and EDConstrainedProxy =
    | EDBvTypeProxy
    | EDMapTypeProxy
    interface IEDBase

and EDApplicable =
    | EDArithmeticCoercion of string
    | EDBinaryOperator of Op: EDOpcode * FunctionName: string * ArgumentCount: int  
    | EDFunctionCall of string
    | EDIfThenElse of string
    | EDMapSelect of string
    | EDMapStore of string
    | EDTypeCoercion of string
    | EDUnaryOperator of string
    interface IEDBase
    

let StringOfOpcode(Op: EDOpcode): string =
    match Op with
    | Add -> "+"
    | Sub -> "-"    
    | Mul -> "*"
    | Div -> Unk
    | Mod -> Unk
    | RealDiv -> Unk
    | FloatDiv -> Unk
    | Pow -> Unk   
    | Eq -> "=="
    | Neq -> "!="    
    | Gt -> ">"
    | Ge -> ">="
    | Lt -> "<"
    | Le -> "<="
    | And -> "&&"
    | Or -> "||"
    | Imp -> "==>"
    | Iff -> "<==>"
    | Subtype -> Unk

    
let rec StringOfProgram (indent: int) (prog: EDProgram): string =
    match prog with
    | EDProgram decls ->
        decls
        |> List.map (StringOfDeclaration indent) 
        |> List.fold (+) ""
        
and StringOfDeclaration (indent: int) (decl: EDDeclaration): string =
    match decl with
    | EDProcedure(repr) ->
        ""
        //TODO: Do not print the procedure for now.
        //indentifym indent repr
    | EDImplementation(bigBlocks) ->
        
        let blockstr =
            bigBlocks
            |> List.map (StringOfBigBlock indent)
            |> List.fold (+) ""
        
        indentify indent ("implementation\n")
        + indentify indent "{\n"
        + blockstr
        + indentify indent "}"
        
    | EDFunction(repr) ->
        indentifym indent repr       

and StringOfBigBlock (indent: int) (bigBlock: IEDBigBlock): string =
    match bigBlock with
    | :? EDBigBlockB as bb -> StringOfBigBlockB indent bb
    | :? EDBigBlockFlat as bf -> StringOfBigBlockFlat indent bf
    | _ -> Unk
    
and StringOfBigBlockB (indent: int) (bigBlock: EDBigBlockB): string =
    match bigBlock with
    | EDBigBlockB(simpleCmds, ec, tc) ->
        
        let v1: string =
            simpleCmds
            |> List.map (StringOfCmd indent)
            |> List.fold (+) ""
        
        let v2: string =
            match ec with
            | Some(ecv) -> StringOfStructuredCmd indent ecv
            | None -> ""            
        let v3 =
            match tc with
            | Some(tcv) -> StringOfTransferCmd indent tcv
            | None -> ""
        
        v1 + v2 + v3
        
and StringOfBigBlockFlat (indent: int) (bigBlockFlat: EDBigBlockFlat): string =
    match bigBlockFlat with
    | EDBigBlockFlat(cmds) ->
        let blockRepr = 
            cmds
            |> List.map ( fun x ->
                match x with
                | :? EDCmd as cmd -> StringOfCmd (indent + 1) cmd
                | :? EDStructuredCmd as sc-> StringOfStructuredCmd (indent + 1) sc
                | :? EDTransferCmd as tc  -> StringOfTransferCmd (indent + 1) tc
                | _ -> Unk
                )
            |> List.fold (+) ""
        //indentify indent "BEGIN_BLOCK\n"
        blockRepr
        //indentify indent "END_BLOCK"

and StringOfMixedCmd(indent: int) (cmd: IEDMixedCmd): string =
    match cmd with
    | :? EDCmd as cmd -> StringOfCmd indent cmd
    | :? EDStructuredCmd as sc -> StringOfStructuredCmd indent sc
    | :? EDTransferCmd as tc -> StringOfTransferCmd indent tc
    | _ -> Unk
        
        
and StringOfCmd (indent: int) (cmd: EDCmd): string =
    match cmd with
    | EDAssignCmd(value) -> value
    | EDCmdContracts -> Unk
    | EDCommentCmd(comment) -> indentify indent ("//" + comment + "\n")
    | EDHavocCmd -> Unk
    | EDPredicateCmdW(predicateCmd) -> StringOfPredicateCmd indent predicateCmd 
    | EDRE -> Unk
    | EDStateCmdW(stateCmd) -> StringOfStateCmd indent stateCmd
    | EDSugaredCmd -> Unk
    | EDYieldCmd -> Unk

and StringOfStructuredCmd (indent: int) (sc: EDStructuredCmd): string =
    match sc with
    | EDComposition(cmds)->
        cmds
        |> List.map (StringOfMixedCmd indent)
        |> List.fold (+) ""
        
    | EDBreakCmd -> Unk
    | EDIfCmd -> Unk
    | EDStructuredCmdContracts -> Unk
    //| EDWhileCmd(Invariants: EDPredicateCmd list * Guard: EDExpr * Body: EDBigBlockFlat list
    |  EDWhileCmd(invs, grd, body) ->
        let IND = String.replicate indent "    "
        
        let invrep =
            invs
            |> List.map (StringOfPredicateCmd indent)
            //|> List.map (fun x -> x)
            |> String.concat ""
        
        let invrep = 
            [for inv in invs do
                match inv with
                | EDAssertCmdW(EDAssertBasicCmd(expr)) ->
                    let exprRep = (StringOfExpr 0 expr)
                    yield (sprintf "%sinvariant %s;\n" IND exprRep)
                | _ -> Unk
            ]
            |> String.concat ""                
        
        let grdrep = StringOfExpr 0 grd
        
        let bodyrep = 
            body
            |> List.map (StringOfBigBlock indent)
            |> String.concat ""
        
                
        sprintf "%swhile (%s)\n" IND grdrep
        + invrep
        + sprintf "%s{\n" IND
        + sprintf "%s" bodyrep
        + sprintf "%s}\n" IND
                
and StringOfTransferCmd (indent: int) (tc: EDTransferCmd): string =
    match tc with
    | EDGotoCmd -> Unk
    | EDReturnCmd -> Unk
    | EDReturnExprCmd -> Unk 
    | EDTransferCmdContracts -> Unk

and StringOfPredicateCmd (indent: int) (pc: EDPredicateCmd): string =
    match pc with 
    | EDAssertCmdW(assertCmd) ->
        StringOfAssertCmd indent assertCmd 
    
    | EDAssumeCmd(edExpr) ->
        indentify indent (sprintf "%s%s%s" "assume " (StringOfExpr 0 edExpr)  ";\n")

and StringOfStateCmd (indent: int) (sc: EDStateCmd): string =
    match sc with
    | EDCallCmd -> Unk
    | EDParCallCmd -> Unk
    | EDSugaredCmdContract -> Unk

and StringOfExpr (indent: int) (expr: EDExpr): string =
    match expr with 
    | EDBinderExprW(binderExpr: EDBinderExpr) -> Unk        
    | EDBvConcatExpr(repr) -> Unk
    | EDBvExtractExpr(repr) -> Unk
    | EDCodeExpr(repr) -> Unk 
    | EDExprContracts(repr) -> Unk 
    | EDIdentifierExpr(repr) -> repr
    | EDLetExpr(repr) -> Unk
    | EDLiteralExpr(repr) -> repr 
    | EDNAryExpr(func: EDApplicable,  args: EDExpr list) ->
        match func with
        | EDBinaryOperator(op: EDOpcode, funcname: string, argcount: int) ->
            
            String.replicate indent " "
            + (StringOfExpr 0 args.Head)
            //+ " " + StringOfOpcode(op) + " "
            + " " + funcname + " "
            + (StringOfExpr 0 args.Tail.Head)
        | _ -> Unk
        
    | EDOldExpr(repr) -> Unk 

and StringOfAssertCmd (indent: int) (assertCmd: EDAssertCmd): string =
    match assertCmd with
    | EDAssertBasicCmd(expr) -> StringOfExpr indent expr
    | EDAssertEnsuresCmd(expr) -> StringOfExpr indent expr
    | EDAssertRequiresCmd(expr) -> StringOfExpr indent expr
    | EDLoopInitAssertCmd(expr) -> StringOfExpr indent expr
    | EDLoopInvMaintainedAssertCmd(expr) -> StringOfExpr indent expr
    