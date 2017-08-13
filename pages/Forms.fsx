#load "suaveHtml.fsx"
#load "../dataAccess.fsx"

open SuaveHtml
open DataAccess

let stringAsNode (str: string option) =
  let t = defaultArg str ""
  td [] (text t)
  
let dataTemp = 
  DataAccess.getUsers
  |> Seq.where (fun t -> t.NameComputed.IsSome && t.NameComputed.Value <> "")
  |> Seq.map (
    fun t-> 
      [ t.NameComputed; t.HomePhone; t.HomeCity; t.HomeState; t.HomeZip; ] 
      |> Seq.map stringAsNode
      |> Seq.toList
      )
  |> Seq.map (fun t -> tr [] t)
  |> Seq.take 25
  |> Seq.toList

let data = 
  DataAccess.getUsers 
  |> Seq.where (fun t -> t.NameComputed.IsSome && t.NameComputed.Value <> "")
  |> Seq.map (fun t-> t.NameComputed, t.HomePhone, t.HomeCity, t.HomeState, t.HomeZip)
let rowList = 
  data
  |> Seq.map (fun (a, b, c, d, e) -> stringAsNode a, stringAsNode b, stringAsNode c, stringAsNode d, stringAsNode e)
  |> Seq.map (fun (a, b, c, d, e) -> tr [] [a; b; c; d; e])
  |> Seq.take 25
  |> Seq.toList

let Home = 
  table ["id", "example"; "class", "display"; "cellspacing", "0"; "width", "100%"] [
    thead [] [
       tr [] [
        th [] (text "Name")
        th [] (text "Phone")
        th [] (text "HomeCity")
        th [] (text "HomeState")
        th [] (text "HomeZip")
      ]
    ]
    tbody [] dataTemp
  ]