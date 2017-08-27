#r "../SuaveHtml.dll"
#load "../dataAccess.fsx"

open SuaveHtml
open DataAccess 
open System

// Public
type Name = string
type DragMode = DragNone | Draggable
type SortMode = SortNone | SortByNumber | SortByString
type FilterMode = FilterNone | FilterByString
type ColumnInfo = Name * SortMode * FilterMode * DragMode * Node
type Id = string
type ColumnNumber = int
type Column = Id * ColumnNumber * ColumnInfo

let str t = 
  Text ((defaultArg t "").Trim())

let getId colInfo = 
  let (colName:string), _, _, _, _ = colInfo
  let idNoSpaces:Id = colName.Replace(" ", "")
  idNoSpaces
let getColumns (columnInfoSeq: ColumnInfo seq) =
  columnInfoSeq
  |> Seq.indexed
  |> Seq.map (fun (i, col) -> getId col, i + 1, col):Column seq
// END

// Begin user column declaration

let toggleNode = 
  img ["class", "toggleBtn"; "src", "content\\rightArrow.png"; "onclick", "toggleMe(event)"]

let gridData = 
  DataAccess.getUsers
  |> Seq.where (fun t -> t.HomePhone.IsSome)
  |> Seq.where (fun t -> t.HomeCity.IsSome)
  //|> Seq.take 10
  |> Seq.map (fun t -> 
      [
        "Pre",        SortNone    , FilterNone    , DragNone,  toggleNode
        "Name",       SortByString, FilterByString, Draggable, str t.NameComputed
        "Home Phone", SortByString, FilterByString, Draggable, str t.HomePhone
        "Home City",  SortByString, FilterByString, Draggable, str t.HomeCity
        "Home State", SortByString, FilterByString, Draggable, str t.HomeState
        "Home Zip",   SortByString, FilterByString, Draggable, str t.HomeZip
        "Detail Row", SortNone    , FilterNone    , DragNone , str t.Email
      ]
      |> getColumns)
// END
let getSortAttr sort =
  if sort = SortNone then
    "", ""
  else
    "onclick", "sortMe(event, SortByString)"

let getDrag drag =
  if drag = DragNone then
    ["", ""]
  else
    ["ondragstart", "drag(event)"; "ondrop", "drop(event)"; "ondragover", "allowDrop(event)"; "draggable", "true"]

let getFilter filter =
  if filter = FilterNone then
    Text ""
  else
    input ["type", "text"; "onkeyup", "filterMe(this)"]

let grid =
  gridData
  |> Seq.indexed
  |> Seq.map(fun (rowNum, columnSeq) ->
    let nodes =
      columnSeq
      |> Seq.indexed
      |> Seq.map (fun (colNum, t) -> 
          let id, num, (name, sortMode, filterMode, dragMode, dataValue) = t
          let style = "grid-row: 1; grid-column: " + string(colNum + 1)
          if rowNum = 0 then
            let classValue = id + " " + id + "Header" + " Header"
            let sortImage = img ["class", "sortBtn"; "src", "content\\noArrow.png";]
            div ["class", classValue; "style", style; "id", id; getSortAttr sortMode; ] [Text name; sortImage; getFilter filterMode]
          else
            if name = "Detail Row" then
              div ["class", "detailRow"; "style", "display: none"] [dataValue]
            else
              let classValue = id + " " + id + "Col"
              div ["class", classValue; "style", style] [dataValue])
      |> Seq.toList        
    let rowStyle = "grid-row: " + string(rowNum + 1)
    let showOnly =
      if rowNum < 100 then
        ""
      else
        ";display: none;"
    if rowNum = 0 then
      div ["class", "row"; "style", rowStyle;] nodes
    else
      div ["class", "row"; "id", "row" + string rowNum; "style", rowStyle+showOnly] nodes)
  |> Seq.toList

let Home = 
  div ["class", "wrapper"] grid