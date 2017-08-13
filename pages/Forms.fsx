#load "suaveHtml.fsx"
#load "../dataAccess.fsx"

open SuaveHtml
open DataAccess

let data = 
  DataAccess.getUsers
  |> Seq.where (fun t -> t.NameComputed.IsSome && t.NameComputed.Value <> "")
  |> Seq.map (
    fun t-> 
      [ t.NameComputed; t.HomePhone; t.HomeCity; t.HomeState; t.HomeZip; ]
      |> Seq.map (stringToNode [])
      |> Seq.toList
      )
  |> reduceSequence [] 25

let Home = 
  table ["id", "example"; "class", "display"] [
    thead [] [
       tr [] [
        th [] (text "Name")
        th [] (text "Phone")
        th [] (text "HomeCity")
        th [] (text "HomeState")
        th [] (text "HomeZip")
      ]
    ]
    tbody [] data
  ]