module EdsgerFSharp.Utils

let Unk<'T> : 'T = failwith "Not implemented yet"

let indentifym (indent: int) (x: string) =
    x.Split '\n'    
    |> Array.map (fun x -> (String.replicate indent "    ") + x)
    |> Array.fold (fun x y -> x + "\n" + y) ""
    
let indentify (indent: int) (x: string) =
    (String.replicate indent "    ") + x