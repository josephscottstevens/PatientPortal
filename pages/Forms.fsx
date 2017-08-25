#r "../SuaveHtml.dll"
#load "../dataAccess.fsx"

open SuaveHtml
open DataAccess 

// Public
type Name = string
type SortMode = NumberSort | StringSort
type FilterMode = Enabled | Disabled
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

let getColumnList (columnInfoList: ColumnInfo list) =
  columnInfoList
  |> List.indexed
  |> List.map (fun (i, (col)) -> getId col, i + 1, col):Column list
// END

// Begin user column declaration
let columns = 
  DataAccess.getUsers
  |> Seq.where (fun t -> t.HomePhone.IsSome)
  |> Seq.map (fun t -> 
      [
        "Name",       StringSort, Enabled, str t.HomePhone
        "Home Phone", StringSort, Enabled, str t.NameComputed
        "Home City",  StringSort, Enabled, str t.HomeCity
        "Home State", StringSort, Enabled, str t.HomeState
        "Home Zip",   StringSort, Enabled, str t.HomeZip
      ]
      |> getColumnList)
// END
Seq.head columns
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