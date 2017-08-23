#r "../SuaveHtml.dll"
#load "../dataAccess.fsx"

open SuaveHtml
open DataAccess 

// Public
type Name = string
type SortMode = NumberSort | StringSort
type FilterMode = Enabled | Disabled
type ColumnInfo = Name * SortMode * FilterMode

// Private
type Id = string
type ColumnNumber = int
type Column = Id * ColumnNumber * ColumnInfo
let str t = defaultArg t ""
// For Testing
let x:ColumnInfo = "bob", NumberSort, Enabled
let a, b, c = x
let getColName (colInfo:ColumnInfo) =
  let a, b, c = colInfo
  a

let getId colInfo = 
  let zz:string = getColName colInfo
  let zzz:Id = zz.Replace(" ", "")
  zzz
// END

let columnInfoList:ColumnInfo list = 
  [
    "Name",       StringSort, Enabled
    "Home Phone", StringSort, Enabled
    "Home City",  StringSort, Enabled
    "Home State", StringSort, Enabled
    "Home Zip",   StringSort, Enabled
  ]

let columnList:Column list =
  columnInfoList
  |> List.indexed
  |> List.map (fun (i, col) -> getId col, i + 1, col)

  
let reduceSequence count seqList = 
    seqList
    |> Seq.map (fun t -> tr [] t)
    |> Seq.take count
    |> Seq.toList

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

let data = 
  DataAccess.getUsers
  |> Seq.where (fun t -> t.HomePhone.IsSome)
  |> Seq.map (fun t -> 
      [
        "Name", str t.HomePhone
        "HomePhone", str t.NameComputed
        "HomeCity", str t.HomeCity
        "HomeState", str t.HomeState
        "HomeZip", str t.HomeZip
      ] 
      |> columnBuilder)
   |> Seq.toList

let grid =
  data
  |> Seq.indexed
  |> Seq.map (fun (i:int, lst: Node list) -> 
    let colStyle = "grid-row: " + (string (i + 1))
    let attrPart = ["class", "row"; "style", colStyle]
    let attr =
      if i = 0 then 
        attrPart
      else 
        attrPart @ ["data-row", (string i)]
    div attr lst)
  |> Seq.toList  

let Home = 
  div ["class", "Grid"] ((headerRow columns) @ grid)