#r "SuaveHtml.dll"
open SuaveHtml

type Name = string
type DragMode = DragNone | Draggable
type SortMode = SortNone | SortByNumber | SortByString
type FilterMode = FilterNone | FilterByString
type ColumnInfo = Name * SortMode * FilterMode * DragMode * Node
type Id = string
type ColumnNumber = int
type Column = Id * ColumnNumber * ColumnInfo

let i = tag "i"

let getId colInfo = 
  let (colName:string), _, _, _, _ = colInfo
  let idNoSpaces:Id = colName.Replace(" ", "")
  idNoSpaces
  
let getColumns (columnInfoSeq: ColumnInfo seq) =
  columnInfoSeq
  |> Seq.indexed
  |> Seq.map (fun (i, col) -> getId col, i + 1, col):Column seq
  
let getSortAttr sort =
  match sort with
  | SortNone -> "", ""
  | SortByString
  | SortByNumber -> "onclick", "sortMe(event, " + string sort + ")"

let getDrag drag =
  match drag with
  | DragNone -> ["", ""]
  | Draggable -> ["ondragstart", "drag(event)"; "ondrop", "drop(event)"; "ondragover", "allowDrop(event)"; "draggable", "true"]

let getFilter filter colId =
  match filter with 
  | FilterNone -> Text ""
  | FilterByString -> input ["type", "text"; "onkeyup", "filterMe(this)"; "id", colId + "Input"; "class", "filterInput"]

let cellStyle colNumber = "grid-row: 1; grid-column: " + string(colNumber + 1)
let rowStyle rowNumber = "grid-row: " + string(rowNumber + 1)

let headerRow headerData =
  let headerColumns =
    headerData
    |> Seq.indexed
    |> Seq.map  (fun (colNum, t)-> 
      let id, num, (name, sortMode, filterMode, dragMode, dataValue) = t
      let classValue = id + " " + id + "Header" + " Header"
      let sortImage = img ["class", "sortBtn"; "src", "content\\noArrow.png";]
      if name = "Detail Row" then
        Text ""
      else
        div (["class", classValue; "style", cellStyle colNum; "id", id; getSortAttr sortMode; ] @ getDrag dragMode ) [Text name; sortImage; getFilter filterMode id])
    |> Seq.toList
  div ["class", "headerRow"; "style", rowStyle 0] headerColumns
  
let footerRow rowsPerPage = 
  let footerStyle = "grid-row: " + string (rowsPerPage+2) + "; grid-column: 1 / -1;"
  let pagination =
    [1..10]
    |> List.map (fun i ->
      let pagingClass = 
        if i = 1 then
          "pagingItem pagingItemActive"
        else
          "pagingItem"
      a "#" ["id", "page" + (string i); "class", pagingClass; "onclick", "GoToPage('" + string(i - 1) + "')"] (text (string i)))

  let paginationHeader =
    [
      i ["id", "pageFirst"; "class", "pagingControl pagingItemDisabled fa fa-fast-backward"; "onclick", "GoToPage('pageFirst')"][]
      i ["id", "pagePrevious"; "class", "pagingControl pagingItemDisabled fa fa-backward"; "onclick", "GoToPage('pagePrevious')"][]
      i ["id", "pageBlockPrevious"; "class", "pagingControl fa fa-ellipsis-h"; "style", "display: none"; "onclick", "GoToPage('pageBlockPrevious')"][]
    ]
  let paginationFooter =
    [
      i ["id", "pageBlockNext"; "class", "pagingControl fa fa-ellipsis-h"; "onclick", "GoToPage('pageBlockNext')"][]
      i ["id", "pageNext"; "class", "pagingControl fa fa-forward"; "onclick", "GoToPage('pageNext')"][]
      i ["id", "pageLast"; "class", "pagingControl fa fa-fast-forward"; "onclick", "GoToPage('pageLast')"][]
    ]
  div ["id", "footer"; "class", "footerRow"; "style", footerStyle]
    (paginationHeader @ pagination @ paginationFooter)

let grid gridData rowsPerPage =
  let gridComputed = 
    gridData
      |> Seq.indexed
      |> Seq.map(fun (rowNum, columnSeq) ->
        let nodes =
          columnSeq
          |> Seq.indexed
          |> Seq.map (fun (colNum, t) -> 
              let id, num, (name, sortMode, filterMode, dragMode, dataValue) = t
              
              if name = "Detail Row" then
                div ["class", "detailRow"; "style", "display: none"] [dataValue]
              else
                let classValue = id + " " + id + "Col Col"
                div ["class", classValue; "name", id; "style", cellStyle colNum] [dataValue])
          |> Seq.toList        
        
        let rowStyleShow =
          if rowNum < rowsPerPage then
            ""
          else
            (rowStyle rowNum) + ";display: none;"
        div ["class", "row"; "id", string rowNum; "style", rowStyleShow] nodes)
      |> Seq.toList
  [headerRow (Seq.head gridData)] @ gridComputed @ ([footerRow 10])