// Learn more about F# at http://fsharp.net
// See the 'F# Tutorial' project for more help.

open System.Diagnostics
open System
open System.Threading

let exWrapper f arg1=
    let sw = new System.Diagnostics.Stopwatch();
    sw.Start();
    try
        let result = f arg1;
        result;
    with
    //| _ -> printfn "oops, something went wrong"
    | _ -> failwith "something bad happened out there";
    sw.Stop();
    printfn "The time elapsed is : %A" sw.ElapsedMilliseconds;

let printer arg=
    printfn "%A \n" arg;
    //1/0 |> ignore
    //System.Console.ReadLine();

[<EntryPoint>]
let main argv = 
    exWrapper printer argv
    exWrapper (fun x-> printfn "%d" x) 2;
    exWrapper (fun y-> printfn "%s" y) "hello world";
    System.Console.ReadKey() |> ignore;
    0 // return an integer exit code

