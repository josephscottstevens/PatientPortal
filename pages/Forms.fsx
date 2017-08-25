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
        "Name",       SortByString, FilterByString, str t.HomePhone
        "Home Phone", SortByString, FilterByString, str t.NameComputed
        "Home City",  SortByString, FilterByString, str t.HomeCity
        "Home State", SortByString, FilterByString, str t.HomeState
        "Home Zip",   SortByString, FilterByString, str t.HomeZip
      ]
      |> getColumns)
// END
let makeHeaderCol ((i:int), (colName:string)) = 
  let colNoSp = colName.Replace(" ", "")
  let className = colNoSp + " " + colNoSp + "Header"
  let dataCol = string (i + 1)
  let style = "grid-row: 1; grid-column: " + (string (i + 1))
  let node =
    if i = 0 then
      []
    else
      (text colName)
  div ["class", className; "data-col", dataCol; "style", style; "id", colNoSp] node
let headerRow (cols: string list) =
  let rows = 
    (["Pre"] @ cols)
    |> List.indexed
    |> List.map makeHeaderCol
  [div ["class", "row"; "style", "grid-row: 1;"] rows]

let columnBuilder (lst:list<string * string>) =
  let columns = ["Pre", ">"] @ lst
  columns
  |> List.indexed 
  |> List.map (fun (i, (name, value)) -> 
      let clsName = name + " " + name + "Col"
      let column = string (i + 1)
      let style = "grid-row: 1; grid-column: " + column
      div ["class", clsName; "data-col", column; "style", style] (text value)
      )

// let grid =
//   data
//   |> Seq.indexed
//   |> Seq.map (fun (i:int, lst: Node list) -> 
//     let colStyle = "grid-row: " + (string (i + 1))
//     let attrPart = ["class", "row"; "style", colStyle]
//     let attr =
//       if i = 0 then 
//         attrPart
//       else 
//         attrPart @ ["data-row", (string i)]
//     div attr lst)
//   |> Seq.toList  

let Home = 
  div [] (text "testing4")
  //div ["class", "Grid"] ((headerRow columns) @ grid)