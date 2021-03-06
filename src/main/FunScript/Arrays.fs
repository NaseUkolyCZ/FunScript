﻿module internal FunScript.Arrays

open AST
open System.Collections.Generic
open Microsoft.FSharp.Quotations

let private creation =
   CompilerComponent.create <| fun (|Split|) _ returnStrategy ->
      function
      | Patterns.NewArray(arrayType, exprs) ->
         let decls, refs = Reflection.getDeclarationAndReferences (|Split|) exprs
         [ yield! decls |> Seq.concat 
           yield returnStrategy.Return <| Array refs
         ]
      | _ -> []

let private getIndex = 
   CompilerComponent.binary 
      <@ fun (xs:_ []) i -> xs.[i] @> 
      (fun array index -> IndexGet(array, index))

let private setIndex = 
   CompilerComponent.ternaryStatement
      <@ fun (xs:_ []) i v -> xs.[i] <- v @> 
      (fun array index value -> Assign(IndexGet(array, index), value))

let private getIndexBoxed = 
   CompilerComponent.binary 
      <@ fun (xs:System.Array) (i : int) -> xs.GetValue i @> 
      (fun array index -> IndexGet(array, index))

let private setIndexBoxed = 
   CompilerComponent.ternaryStatement
      <@ fun (xs:System.Array) (v : obj) (i : int) -> xs.SetValue(v, i) @> 
      (fun array value index -> Assign(IndexGet(array, index), value))

[<JS>]
module Extensions =
   let getEnumeratorBoxed(xs : System.Array) =
      (unbox<obj []> xs |> Array.toSeq).GetEnumerator()

let components = 
   [ 
      [
         creation
         getIndex
         setIndex
         ExpressionReplacer.createUnsafe <@ fun (xs:_ IList) i -> xs.[i] @> <@ fun (xs:_[]) i -> xs.[i] @>
         ExpressionReplacer.createUnsafe <@ fun (xs:_ IList) i v -> xs.[i] <- v @> <@ fun (xs:_[]) i v -> xs.[i] <- v @>
         ExpressionReplacer.createUnsafe <@ fun (xs:_ IList) -> xs.Count @> <@ fun (xs:_[]) -> xs.Length @>
         ExpressionReplacer.create <@ fun (xs:_ []) -> xs.Length @> <@ Core.Array.BoxedLength @>
         ExpressionReplacer.createUnsafe <@ Array.toList @> <@ Core.List.OfArray @>
         ExpressionReplacer.createUnsafe <@ Array.ofList @> <@ Core.List.ToArray @>
         ExpressionReplacer.createUnsafe 
            <@ fun (t, s:int) -> System.Array.CreateInstance(t, s) @> 
            <@ fun (t, s:int) -> Core.Array.CreateInstance(t, s) @>
         ExpressionReplacer.createUnsafe <@ fun (xs : System.Array) -> xs.GetEnumerator() @> <@ Extensions.getEnumeratorBoxed @>
         getIndexBoxed
         setIndexBoxed
         ExpressionReplacer.create <@ Array.toSeq @> <@ Core.Seq.OfArray @>
         ExpressionReplacer.create <@ Array.ofSeq @> <@ Core.Seq.ToArray @>
      ]
      ExpressionReplacer.createModuleMapping 
            "FSharp.Core" "Microsoft.FSharp.Collections.ArrayModule"
            "FunScript" "FunScript.Core.Array"
   ] |> List.concat