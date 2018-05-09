// Learn more about F# at http://fsharp.org

open System

type T = StaticProperty.Provided.MyType<"Hello">

[<EntryPoint>]
let main argv =
    printfn "%s" (T().SayHello ((+) 1))
    0 // return an integer exit code
