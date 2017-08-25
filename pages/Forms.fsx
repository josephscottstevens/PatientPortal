#r "../SuaveHtml.dll"
#load "../dataAccess.fsx"

open SuaveHtml
open DataAccess 
open System

// Public
type Name = string
type SortMode = SortNone | SortByNumber | SortByString
type FilterMode = FilterNone | FilterByString
type DataValue = string
type ColumnInfo = Name * SortMode * FilterMode * DataValue
type Id = string
type ColumnNumber = int
type Column = Id * ColumnNumber * ColumnInfo
let str t = defaultArg t ""

let getId colInfo = 
  let (colName:string), _, _, _ = colInfo
  let idNoSpaces:Id = colName.Replace(" ", "")
  idNoSpaces
let getColumns (columnInfoSeq: ColumnInfo seq) =
  columnInfoSeq
  |> Seq.indexed
  |> Seq.map (fun (i, (col)) -> getId col, i + 1, col):Column seq
// END

// Begin user column declaration
let gridData = 
  DataAccess.getUsers
  |> Seq.where (fun t -> t.HomePhone.IsSome)
  |> Seq.where (fun t -> t.HomeCity.IsSome)
  //|> Seq.take 10
  |> Seq.map (fun t -> 
      [
        "Pre",        SortNone    , FilterNone    , ""  
        "Name",       SortByString, FilterByString, str t.NameComputed
        "Home Phone", SortByString, FilterByString, str t.HomePhone
        "Home City",  SortByString, FilterByString, str t.HomeCity
        "Home State", SortByString, FilterByString, str t.HomeState
        "Home Zip",   SortByString, FilterByString, str t.HomeZip
      ]
      |> getColumns)
// END

let grid =
  gridData
  |> Seq.indexed
  |> Seq.map(fun (rowNum, columnSeq) ->
    let nodes =
      columnSeq
      |> Seq.indexed
      |> Seq.map (fun (colNum, t) -> 
          let id, num, (name, _, _, dataValue) = t
          let style = "grid-row: 1; grid-column: " + (colNum+1).ToString()
          if rowNum = 0 then
            let classValue = id + " " + id + "Header"
            div ["class", classValue; "style", style; "id", id; "onclick", "sortMe(event, SortByString)"] (text name)
          else
            let classValue = id + " " + id + "Col"
            div ["class", classValue; "style", style] (text dataValue))
      |> Seq.toList        
    let rowStyle = "grid-row: " + (rowNum + 1).ToString()
    if rowNum = 0 then
      div ["class", "row"; "style", rowStyle] nodes
    else
      div ["class", "row"; "data-row", rowNum.ToString(); "style", rowStyle] nodes)
  |> Seq.toList

let Home = 
  div ["class", "wrapper"] grid
  //div ["class", "Grid"] ((headerRow columns) @ grid)