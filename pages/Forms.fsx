#r "../SuaveHtml.dll"
#load "../dataAccess.fsx"

open SuaveHtml
open DataAccess 

let data = 
  DataAccess.getUsers
  |> Seq.where (fun t -> t.HomePhone.IsSome)
  |> Seq.map (
    fun t -> 
      [t.NameComputed; t.HomePhone; t.HomeCity; t.HomeState; t.HomeZip]
      |> List.map stringToNode
      )
  |> reduceSequence 200

let Home = 
  div ["id", "Grid"] [
    script [ "type", "text/javascript"; "src", "source/code.js" ] []
    table ["id", "Table1"] [
      thead [] [
         tr [] [
          th [] (text "FullName")
          th [] (text "Phone")
          th [] (text "HomeCity")
          th [] (text "HomeState")
          th [] (text "HomeZip")
        ]
      ]
      tbody [] data
    ]
  ]