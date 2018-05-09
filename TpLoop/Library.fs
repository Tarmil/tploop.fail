namespace TpLoop

open ProviderImplementation
open ProviderImplementation.ProvidedTypes
open Microsoft.FSharp.Core.CompilerServices
open Microsoft.FSharp.Quotations
open System.Reflection

[<TypeProvider>]
type BasicProvider (config : TypeProviderConfig) as this =
    inherit TypeProviderForNamespaces (config)

    let ns = "StaticProperty.Provided"
    let asm = Assembly.GetExecutingAssembly()

    let createTypes () =
        let paramType = ProvidedTypeDefinition(asm, ns, "MyType", None)
        paramType.DefineStaticParameters([ProvidedStaticParameter("x", typeof<string>)], fun typename pars ->
            let param = pars.[0] :?> string
            let myType = ProvidedTypeDefinition(asm, ns, typename, Some typeof<obj>)
            let myCtor = ProvidedConstructor([], fun _ -> <@@ box 0 @@>)
            myType.AddMember(myCtor)
            let myParam = ProvidedParameter("who", typeof<Expr<int -> int>>, IsReflectedDefinition = true)
            let myMeth =
                ProvidedMethod("SayHello", [myParam], typeof<string>,
                    invokeCode = fun args -> <@@ sprintf "%s %A" param (%%args.[1] : Expr<int -> int>) @@>)
            myMeth.AddDefinitionLocation(1, 2, "test.fs")
            myMeth.AddXmlDoc("This is just a stupid test")
            myType.AddMember(myMeth)
            myType
        )
        [paramType]

    do
        this.AddNamespace(ns, createTypes())

[<assembly:TypeProviderAssembly>]
do ()
