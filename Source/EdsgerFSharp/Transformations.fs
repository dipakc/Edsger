module EdsgerFSharp.Transformations

open Edsger.Types
open EdsgerFSharp.Utils
open Microsoft.Boogie
open Microsoft.Boogie
open NUnit.Framework


let rec deconstruct (term: IEDBase): IEDBase list =
    match term with
    | :? EDAbsy as absy -> deconAbsy absy 
    | :? EDProgram as prog  -> deconProgram prog 
    | :? EDDeclaration as declaration -> deconDeclaration declaration
    | :? EDBigBlockB as bigBlock -> deconBigBlock bigBlock
    | :? EDBigBlockFlat as bigBlockFlat -> deconBigBlockFlat bigBlockFlat
    | :? EDCmd as cmd -> deconCmd cmd 
    | :? EDStructuredCmd as structuredCmd -> deconStructuredCmd structuredCmd 
    | :? EDAssignLhs as assignLhs -> deconAssignLhs assignLhs 
    | :? EDPredicateCmd as predicateCmd -> deconPredicateCmd predicateCmd 
    | :? EDAssertCmd as assertCmd -> deconAssertCmd assertCmd 
    | :? EDStateCmd as stateCmd -> deconStateCmd stateCmd 
    | :? EDExpr as expr -> deconExpr expr 
    | :? EDBinderExpr as binderExpr -> deconBinderExpr binderExpr 
    | :? EDQuantifierExpr as quantifierExpr -> deconQuantifierExpr quantifierExpr 
    | :? EDTransferCmd as transferCmd -> deconTransferCmd transferCmd 
    | :? EDType as tpe -> deconType tpe 
    | :? EDConstrainedProxy as constraintedProxy -> deconConstraintedProxy constraintedProxy
    | _ -> Unk


and deconAbsy (term: EDAbsy): IEDBase list =
    match term with
    | EDAbsyContracts -> []
    | EDAssignLhsW(assignLhs) -> [assignLhs] //deconAssignLhs assignLhs
    | EDBlock -> []
    | EDCmdW(cmd) -> [cmd] //deconCmd cmd
    | EDDeclarationW(decl) -> [decl]//deconDeclaration decl
    | EDEnsures -> []
    | EDExprW(expr) -> [expr]//deconExpr expr
    | EDProgramW(program) -> [program] //deconProgram program
    | EDQKeyValue -> []
    | EDRequires -> []
    | EDTransferCmdW(transferCmd) -> [transferCmd] //deconTransferCmd transferCmd
    | EDTrigger -> []
    | EDTypeW(tpe) -> [tpe]//deconType tpe
    | EDTypedIdent -> []
and deconProgram (prog): IEDBase list =
    match prog with 
    | EDProgram(topLevelDeclarations) ->
        topLevelDeclarations
        |> List.map (fun x -> x :> IEDBase)
and deconDeclaration (decl): IEDBase list =
    match decl with 
    | EDProcedure(repr) -> []
    | EDImplementation(bigBlocks) ->
        bigBlocks |> List.map (fun x -> x :> IEDBase)
    | EDFunction(repr) -> []

and deconBigBlock (bigBlock: IEDBigBlock): IEDBase list =
    match bigBlock with
    | :? EDBigBlockB as bb -> deconBigBlockB(bb)
    | :? EDBigBlockFlat as bf -> deconBigBlockFlat(bf)
    | _ -> Unk
    
    
and deconBigBlockB (bigBlock: EDBigBlockB): IEDBase list =
    match bigBlock with
    | EDBigBlockB(simpleCmds, ecopt, tcopt) ->
        let scs =
            simpleCmds
            |> List.map (fun x -> x :> IEDBase)
        
        let ecs =
            match ecopt with
            | Some(ec) -> [ec :> IEDBase]
            | None -> []
        
        let tcs =
            match tcopt with
            | Some(tc) -> [tc :> IEDBase]
            | None -> []
        
        scs @ ecs @ tcs    
    
and deconBigBlockFlat (bigBlockFlat: EDBigBlockFlat): IEDBase list =
    match bigBlockFlat with
    | EDBigBlockFlat(cmds) ->        
        cmds
        |> List.map (fun x -> x :> IEDBase)
        
and  deconCmd cmd =
    match cmd with
    | EDAssignCmd(_) -> []
    | EDCmdContracts -> []
    | EDCommentCmd -> []
    | EDHavocCmd -> []
    | EDPredicateCmdW(predicateCmd) -> [predicateCmd]//deconPredicateCmd(predicateCmd)
    | EDRE -> []
    | EDStateCmdW(stateCmd) -> [stateCmd]//deconStateCmd(stateCmd)
    | EDSugaredCmd -> []
    | EDYieldCmd -> []

and deconStructuredCmd scmd =
    match scmd with
    | EDBreakCmd -> []
    | EDIfCmd -> []
    | EDStructuredCmdContracts -> []
    | EDWhileCmd(invariants, guard, body) ->
        let invList =
            invariants
            |> List.map (fun x -> x :> IEDBase)

        let bodyList =
            body
            |> List.map (fun x -> x :> IEDBase)
        
        invList @ [(guard :> IEDBase)] @ bodyList
    | EDComposition(cmds) ->
        cmds
        |> List.map (fun x -> x :> IEDBase) 
        
        
and deconAssignLhs assignLhs =
    match assignLhs with
    | EDAssignLhsContracts -> []
    | EDMapAssignLhs -> []
    | EDSimpleAssignLhs -> []

    
and deconPredicateCmd predicateCmd =
    match predicateCmd with 
    | EDAssertCmdW(assertCmd) -> [assertCmd]//deconAssertCmd assertCmd
    | EDAssumeCmd(expr) -> [expr] //deconExpr expr
    
and deconAssertCmd assertCmd =
    match assertCmd with
    | EDAssertBasicCmd(expr) -> [expr]
    | EDAssertEnsuresCmd(expr) -> [expr]
    | EDAssertRequiresCmd(expr) -> [expr]
    | EDLoopInitAssertCmd(expr) -> [expr]
    | EDLoopInvMaintainedAssertCmd(expr) -> [expr]
    
and deconStateCmd stateCmd =
    match stateCmd with
    | EDCallCmd -> []
    | EDParCallCmd -> []
    | EDSugaredCmdContract -> []
    
and deconExpr expr =
    match expr with
    | EDBinderExprW(binderExpr) -> [binderExpr]//deconBinderExpr binderExpr
    | EDBvConcatExpr(repr) -> []
    | EDBvExtractExpr(repr) -> []
    | EDCodeExpr(repr) -> []
    | EDExprContracts(repr) -> []
    | EDIdentifierExpr(repr) -> []
    | EDLetExpr(repr) -> []
    | EDLiteralExpr(repr) -> []
    | EDNAryExpr(func, args) ->
        [func :> IEDBase] @ (args |> List.map (fun x -> x :> IEDBase))         
    | EDOldExpr(repr) -> []
    
and deconBinderExpr bexpr =
    match bexpr with 
    | EDLambdaExpr(repr) -> []
    | EDQuantifierExprW(quantifierExpr) -> [quantifierExpr]//deconQuantifierExpr quantifierExpr
    
and deconQuantifierExpr qexpr =
    match qexpr with 
    | EDExistsExpr(repr) -> []
    | EDForallExpr(repr) -> []
    
and deconTransferCmd tcmd =
    match tcmd with
    | EDGotoCmd -> []
    | EDReturnCmd -> []
    | EDReturnExprCmd -> []
    | EDTransferCmdContracts -> []
    
    
and deconType tpe =
    match tpe with 
    | EDBasicType -> []
    | EDBvType -> []
    | EDCtorType -> []
    | EDFloatType -> []
    | EDMapType -> []
    | EDTypeContracts -> []
    | EDTypeProxy -> []
    | EDConstraintedProxyW(constraintedProxy) -> [constraintedProxy]//deconConstraintedProxy constraintedProxy
    | EDTypeSynonymAnnortation -> []
    | EDTypeVariable -> []
    | EDUnresolvedTypeIdentifier -> []
    
and deconConstraintedProxy cproxy =
    match cproxy with
    | EDBvTypeProxy -> []
    | EDMapTypeProxy -> []


let rec construct (term: IEDBase) (childs: IEDBase list): IEDBase =
    match term with
    | :? EDAbsy as absy ->
        (conAbsy absy childs) :> IEDBase
    | :? EDProgram as prog  ->
        (conProgram prog childs) :> IEDBase
    | :? EDDeclaration as declaration ->
        (conDeclaration declaration childs) :> IEDBase
    | :? EDBigBlockB as bigBlockB ->
        (conBigBlockB bigBlockB childs) :> IEDBase        
    | :? EDBigBlockFlat as bigBlockFlat ->
        (conBigBlockFlat bigBlockFlat childs) :> IEDBase        
    | :? EDCmd as cmd ->
        (conCmd cmd childs) :> IEDBase
    | :? EDStructuredCmd as structuredCmd ->
        (conStructuredCmd structuredCmd childs) :> IEDBase
    | :? EDAssignLhs as assignLhs ->
        (conAssignLhs assignLhs childs) :> IEDBase
    | :? EDPredicateCmd as predicateCmd ->
        (conPredicateCmd predicateCmd childs) :> IEDBase
    | :? EDAssertCmd as assertCmd ->
        (conAssertCmd assertCmd childs) :> IEDBase 
    | :? EDStateCmd as stateCmd ->
        (conStateCmd stateCmd childs) :> IEDBase
    | :? EDExpr as expr ->
        (conExpr expr childs) :> IEDBase
    | :? EDBinderExpr as binderExpr ->
        (conBinderExpr binderExpr childs) :> IEDBase  
    | :? EDQuantifierExpr as quantifierExpr ->
        (conQuantifierExpr quantifierExpr childs) :> IEDBase  
    | :? EDTransferCmd as transferCmd ->
        (conTransferCmd transferCmd childs) :> IEDBase
    | :? EDType as tpe ->
        (conType tpe childs) :> IEDBase
    | :? EDConstrainedProxy as constraintedProxy ->
        (conConstrainedProxy constraintedProxy childs) :> IEDBase 
    | :? EDApplicable as applicable ->
        (conApplicable applicable childs) :> IEDBase
    | _ -> Unk

and conAbsy (term: EDAbsy) (childs: IEDBase list): EDAbsy =
    match term with
    | EDAbsyContracts -> term 
    | EDAssignLhsW(assignLhs) ->
        EDAssignLhsW(childs.Head :?> EDAssignLhs)
    | EDBlock -> term
    | EDCmdW(cmd) ->
        EDCmdW(childs.Head :?> EDCmd)
    | EDDeclarationW(decl) -> EDDeclarationW(childs.Head :?> EDDeclaration)
    | EDEnsures -> term
    | EDExprW(expr) -> EDExprW(childs.Head :?> EDExpr)
    | EDProgramW(program) -> EDProgramW(childs.Head :?> EDProgram)
    | EDQKeyValue -> term
    | EDRequires -> term
    | EDTransferCmdW(transferCmd) -> EDTransferCmdW(childs.Head :?> EDTransferCmd)
    | EDTrigger -> term
    | EDTypeW(tpe) -> EDTypeW(childs.Head :?> EDType)
    | EDTypedIdent -> term

and conProgram (term: EDProgram) (childs: IEDBase list): EDProgram =
    EDProgram(childs |> List.map (fun x -> x :?> EDDeclaration))
    
and conDeclaration (term: EDDeclaration) (childs: IEDBase list): EDDeclaration =
    match term with
    | EDProcedure(_) -> term
    | EDImplementation(_) ->
        EDImplementation(childs |> List.map (fun x -> x :?> IEDBigBlock))         
    | EDFunction(_) -> term
    
and conBigBlockB (term: EDBigBlockB) (childs: IEDBase list): EDBigBlockB =
    let simpleCmds =
        childs
        |> List.filter (fun x -> x :? EDCmd)
        |> List.map (fun x -> x :?> EDCmd)
        
    let ec =
        childs
        |> List.filter (fun x -> x :? EDStructuredCmd)
        |> List.tryHead
        |> Option.map (fun x -> x :?> EDStructuredCmd)
        
    let tc: EDTransferCmd option =
        childs
        |> List.filter (fun x -> x :? EDTransferCmd)
        |> List.tryHead
        |> Option.map (fun x -> x :?> EDTransferCmd)
    
    EDBigBlockB(simpleCmds, ec, tc)

and conBigBlockFlat (term: EDBigBlockFlat) (childs: IEDBase list): EDBigBlockFlat =
    
    let cmds: IEDMixedCmd list =
        childs
        |> List.filter (fun x -> x :? IEDMixedCmd)
        |> List.map (fun x -> x :?> IEDMixedCmd)
    
    EDBigBlockFlat(cmds)
    
and conCmd (term: EDCmd) (childs: IEDBase list): EDCmd =
    match term with
    | EDAssignCmd(repr) -> term
    | EDCmdContracts -> term
    | EDCommentCmd -> term
    | EDHavocCmd -> term
    | EDPredicateCmdW(edPredicateCmd) ->
        EDPredicateCmdW(childs.Head :?> EDPredicateCmd)        
    | EDRE -> term
    | EDStateCmdW(edStateCmd) ->
        EDStateCmdW(childs.Head :?> EDStateCmd)        
    | EDSugaredCmd -> term
    | EDYieldCmd -> term
    
and conStructuredCmd (structuredCmd: EDStructuredCmd) (childs: IEDBase list) : EDStructuredCmd =
    Unk
    
and conAssignLhs (assignLhs: EDAssignLhs) (childs: IEDBase list) : EDAssignLhs =
    Unk
    
and conPredicateCmd (predicateCmd: EDPredicateCmd) (childs: IEDBase list) : EDPredicateCmd =
    Unk
    
and conAssertCmd (assertCmd: EDAssertCmd) (childs: IEDBase list) : EDAssertCmd =
    Unk
     
and conStateCmd (stateCmd: EDStateCmd) (childs: IEDBase list) : EDStateCmd =
    Unk
    
and conExpr (expr: EDExpr) (childs: IEDBase list) : EDExpr =
    Unk
    
and conBinderExpr (binderExpr: EDBinderExpr) (childs: IEDBase list) : EDBinderExpr =
    Unk
      
and conQuantifierExpr (quantifierExpr: EDQuantifierExpr) (childs: IEDBase list) : EDQuantifierExpr =
    Unk
      
and conTransferCmd (transferCmd: EDTransferCmd) (childs: IEDBase list) : EDTransferCmd =
    Unk
    
and conType (tpe: EDType) (childs: IEDBase list) : EDType =
    Unk
    
and conConstrainedProxy (constraintedProxy: EDConstrainedProxy) (childs: IEDBase list) : EDConstrainedProxy =
    Unk
     
and conApplicable (prog: EDApplicable) (childs: IEDBase list) : EDApplicable =
    Unk
    



let rec TopDownOnce (rule: IEDBase -> IEDBase option) (term: IEDBase)=
    
    let mutable replaced = false    
    let childs = deconstruct term
    
    let newchilds =
        [ for c in childs do
            if replaced then
                yield c
            else
                let newcOpt = rule c
                match newcOpt with
                | Some(newc) ->
                    replaced <- true
                    yield newc
                | None ->
                    let replaced', newc' = TopDownOnce rule c
                    if replaced' then
                        replaced <- true
                        
                    yield newc'
        ]

    replaced, if replaced then construct term newchilds else term


module Util =
    let lift<'T when 'T :> IEDBase> (transf) (term: IEDBase)  =
        match term with
        | :? 'T as sc ->
            (transf sc) |> Option.map (fun x -> x :> IEDBase)
        | _ -> None

let getNegOp(bop) =
    match bop with
    | EDBinaryOperator(Eq, _, _) -> EDBinaryOperator(Neq, StringOfOpcode(Neq), 2)
    | EDBinaryOperator(Neq, _, _) -> EDBinaryOperator(Eq, StringOfOpcode(Eq), 2)
    | EDBinaryOperator(Gt, _, _) -> EDBinaryOperator(Le, StringOfOpcode(Le), 2)
    | EDBinaryOperator(Ge, _, _) -> EDBinaryOperator(Lt, StringOfOpcode(Lt), 2)
    | EDBinaryOperator(Lt, _, _) -> EDBinaryOperator(Ge, StringOfOpcode(Ge), 2)
    | EDBinaryOperator(Le, _, _) -> EDBinaryOperator(Gt, StringOfOpcode(Gt), 2)
    | _ -> Unk
    
let IntroWhileRule(prog: EDCmd): EDStructuredCmd option =
    match prog with
    | EDPredicateCmdW(EDAssumeCmd(EDNAryExpr(EDBinaryOperator(op, "&&", 2), args))) ->
        let invExpr = args.Head
        let inv = EDAssertCmdW(EDLoopInvMaintainedAssertCmd(invExpr))
        let ngrd: EDExpr = args.Tail.Head        
        let grd =
                match ngrd with
                | EDNAryExpr(op, args) -> EDNAryExpr(getNegOp(op), args)
                | _ -> Unk
        
        let body: IEDBigBlock list = [EDBigBlockFlat([EDPredicateCmdW(EDAssumeCmd(invExpr))])]
        
        Some(EDComposition([
            EDPredicateCmdW(EDAssumeCmd(invExpr))
            EDWhileCmd([inv], grd, body)
        ]))
    | _ -> None



[<Test>]
let SimpleRewrite() =
    let b = EDBigBlockB([ EDAssignCmd("hello"); EDHavocCmd],
               Some(EDIfCmd),
               Some(EDGotoCmd))
    
    let rule1 (term: IEDBase) =
        match term with
        | :? EDStructuredCmd as sc ->
            match sc with
            | EDIfCmd -> Some(EDBreakCmd :> IEDBase)
            | _ -> None
        | _ -> None
    
    let transf2(term: EDStructuredCmd): EDStructuredCmd option =
        match term with
        | EDIfCmd -> Some(EDBreakCmd)
        | _ -> None
            
    let rule2 (term: IEDBase) =
        match term with
        | :? EDStructuredCmd as sc ->
            (transf2 sc) |> Option.map (fun x -> x :> IEDBase)
        | _ -> None
                      
    let rule2' = Util.lift transf2
    
    let b2 = TopDownOnce rule2' b
    
    printf "done"
    
    ()
    
    
    