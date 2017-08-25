#r "../SuaveHtml.dll"
#load "../dataAccess.fsx"

open SuaveHtml
open DataAccess 

// Public
type Name = string
type SortMode = SortNone | NumberSort | SortByString
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
          let style = "grid-row: 1; grid-column: " + (string (colNum+1))
          if rowNum = 0 then
            let classValue = id + " " + id + "Header"
            div ["class", classValue; "style", style; "id", id] (text name)
          else
            let classValue = id + " " + id + "Col"
            div ["class", classValue; "style", style] (text dataValue))
      |> Seq.toList        
    if rowNum = 0 then
      div ["class", "row"] nodes
    else
      div ["class", "row"; "data-row", string rowNum; "style", string (rowNum + 1)] nodes)
  |> Seq.toList

let Home = 
  div [] grid
  //div ["class", "Grid"] ((headerRow columns) @ grid)