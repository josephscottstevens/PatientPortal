#r "../SuaveHtml.dll"
#load "../dataAccess.fsx"
#load "../Grid.fsx"

open SuaveHtml
open DataAccess 
open Grid

let str t = 
  Text ((defaultArg t "").Trim())

let toggleNode = 
  img ["class", "toggleBtn"; "src", "content\\rightArrow.png"; "onclick", "toggleMe(event)"]

let gridData = 
  DataAccess.getUsers
  |> Seq.where (fun t -> t.HomePhone.IsSome)
  |> Seq.where (fun t -> t.HomeCity.IsSome)
  |> Seq.map (fun t -> 
      [
        "Pre",        SortNone    , FilterNone    , DragNone,  toggleNode
        "Name",       SortByString, FilterByString, Draggable, str t.NameComputed
        "Gender",     SortByString, FilterByString, Draggable, str t.Gender
        "Home Phone", SortByString, FilterByString, Draggable, str t.HomePhone
        "Home City",  SortByString, FilterByString, Draggable, str t.HomeCity
        "Home State", SortByString, FilterByString, Draggable, str t.HomeState
        "Home Zip",   SortByString, FilterByString, Draggable, str t.HomeZip
        "Detail Row", SortNone    , FilterNone    , DragNone , str t.Email
      ]
      |> getColumns)

let Home = 
  div ["class", "wrapper"] [grid gridData 10]