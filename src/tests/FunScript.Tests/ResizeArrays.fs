﻿[<NUnit.Framework.TestFixture>] 
module FunScript.Tests.ResizeArrays

open NUnit.Framework

[<Test>]
let ``ResizeArray zero creation works``() =
   check 
      <@@ 
         let li = ResizeArray<float>()
         true
      @@>

[<Test>]
let ``ResizeArray zero creation with size works``() =
   check 
      <@@ 
         let li = ResizeArray<string>(5)
         true
      @@>

[<Test>]
let ``ResizeArray creation with seq works``() =
   check 
      <@@ 
         let li = ResizeArray<_>(seq{1. .. 5.})
         li.[2]
      @@>

[<Test>]
let ``ResizeArray folding works``() =
   check 
      <@@ 
         ResizeArray<_>(seq{1. .. 5.})
         |>Seq.fold (fun acc item -> acc + item) 0.
      @@>

[<Test>]
let ``ResizeArray.Count works``() =
   check 
      <@@ 
         let li = ResizeArray<_>(seq{1. .. 5.})
         li.Count |> float
      @@>

[<Test>]
let ``ResizeArray indexer getter works``() =
   check 
      <@@ 
         let li = ResizeArray<_>(seq{1. .. 5.})
         li.[1]
      @@>

[<Test>]
let ``ResizeArray indexer setter works``() =
   check 
      <@@ 
         let li = ResizeArray<_>(seq{1. .. 5.})
         li.[3] <- 10.
         li.[3]
      @@>

[<Test>]
let ``ResizeArray.Clear works``() =
   check 
      <@@ 
         let li = ResizeArray<_>(seq{1. .. 5.})
         li.Clear()
         li.Count |> float
      @@>

[<Test>]
let ``ResizeArray.Add works``() =
   check 
      <@@ 
         let li = ResizeArray<_>(seq{1. .. 5.})
         li.Add(6.)
         li.Count |> float
      @@>

[<Test>]
let ``ResizeArray.AddRange works``() =
   check 
      <@@ 
         let li = ResizeArray<_>(seq{1. .. 5.})
         li.Count |> float
      @@>

[<TestCase("ab"); TestCase("cd")>]
let ``ResizeArray.Contains works``(str:string) =
   check  
      <@@
         let li = ResizeArray<_>()
         li.Add("ab")
         li.Add("ch")
         li.Contains(str)
      @@>

[<TestCase("ab"); TestCase("cd")>]
let ``ResizeArray.IndexOf works``(str:string) =
   check  
      <@@
         let li = ResizeArray<_>()
         li.Add("ch")
         li.Add("ab")
         li.IndexOf(str) |> float
      @@>

[<TestCase("ab"); TestCase("cd")>]
let ``ResizeArray.Remove works``(str:string) =
   check  
      <@@
         let li = ResizeArray<_>()
         li.Add("ab")
         li.Add("ch")
         li.Remove(str)
      @@>

[<Test>]
let ``ResizeArray.RemoveAt works``() =
   check 
      <@@ 
         let li = ResizeArray<_>(seq{1. .. 5.})
         li.RemoveAt(2)
         li.[2]
      @@>

[<Test>]
let ``ResizeArray.Insert works``() =
   check 
      <@@ 
         let li = ResizeArray<_>(seq{1. .. 5.})
         li.Insert(2, 8.)
         li.[2]
      @@>

[<Test>]
let ``ResizeArray.ReverseInPlace works``() =
   check 
      <@@ 
         let li = ResizeArray<_>(seq{1. .. 5.})
         li.Reverse()
         li.[2]
      @@>

[<Test>]
let ``ResizeArray.SortInPlace works``() =
   check 
      <@@ 
         let li = ResizeArray<_>(["Ana";"Pedro";"Lucía";"Paco"])
         li.Sort()
         li.[2]
      @@>

[<Test>]
let ``ResizeArray.SortInPlaceWith works``() =
   check 
      <@@ 
         let li = ResizeArray<_>([|3.;6.;5.;4.;8.|])
         li.Sort(fun x y -> if x > y then -1 elif x < y then 1 else 0)
         li.Sort()
         li.[3]
      @@>


