namespace TpLoop

open ProviderImplementation
open ProviderImplementation.ProvidedTypes
open Microsoft.FSharp.Core.CompilerServices
open Microsoft.FSharp.Quotations
open System.IO
open System.Reflection

[<TypeProvider>]
type BasicProvider (cfg : TypeProviderConfig) as this =
    inherit TypeProviderForNamespaces (cfg)

    let ns = "TpLoop.Provided"
    let asm = Assembly.GetExecutingAssembly()

    let createTypes () =
        let myType = ProvidedTypeDefinition(asm, ns, "MyType", Some typeof<obj>)
        let genTy = typedefof<list<_>>
        // The next line stalls compilation of TpLoop.Tests; comment it out and it builds
        let fails = genTy.MakeGenericType(myType).FullName
        // The next lines always succeed
        let succeeds1 = myType.FullName
        let succeeds2 = genTy.FullName
        let succeeds3 = genTy.MakeGenericType(typeof<int>).FullName
        [myType]

    do
        this.AddNamespace(ns, createTypes())

[<assembly:TypeProviderAssembly>]
do ()
