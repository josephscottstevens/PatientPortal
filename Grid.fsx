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

let footerRow rowsPerPage totalRows = 
  let footerStyle = "grid-row: " + string (rowsPerPage+2) + "; grid-column: 1 / -1;"
  let last = int (float totalRows / float rowsPerPage)

  let pagination =
    [1..last]
    |> List.map (fun i ->
      let pagingClass = 
        if i <= rowsPerPage then
          if i = 1 then
            "pagingItem pagingItemActive"
          else
            "pagingItem"
        else
          "pagingItem pagingHide"
      a "#" ["id", "page" + (string i); "class", pagingClass; "onclick", "GoToPage('" + string i + "')"] (text (string i)))

  let paginationHeader =
    [
      a "#" ["id", "pageFirst"; "class", "pagingControl pagingItemDisabled"; "onclick", "GoToPage('pageFirst')"] (text "|<")
      a "#" ["id", "pagePrevious"; "class", "pagingControl pagingItemDisabled"; "onclick", "GoToPage('pagePrevious')"] (text "<")
      a "#" ["id", "pageBlockPrevious"; "class", "pagingControl pagingHide"; "onclick", "GoToPage('pageBlockPrevious')"] (text "...")
    ]
  let paginationFooter =
    [
      a "#" ["id", "pageBlockNext"; "class", "pagingControl"; "onclick", "GoToPage('pageBlockNext')"] (text "...")
      a "#" ["id", "pageNext"; "class", "pagingControl"; "onclick", "GoToPage('pageNext')"] (text ">")
      a "#" ["id", "pageLast"; "class", "pagingControl"; "onclick", "GoToPage('pageLast')"] (text ">|")
    ]
  div ["id", "footer"; "class", "footerRow"; "style", footerStyle; "data-rows-per-page", string rowsPerPage]
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
              let style = "grid-row: 1; grid-column: " + string(colNum + 1)
              if rowNum = 0 then
                let classValue = id + " " + id + "Header" + " Header"
                let sortImage = img ["class", "sortBtn"; "src", "content\\noArrow.png";]
                if name = "Detail Row" then
                  Text ""
                else
                  div (["class", classValue; "style", style; "id", id; getSortAttr sortMode; ] @ getDrag dragMode ) [Text name; sortImage; getFilter filterMode id]
              else
                if name = "Detail Row" then
                  div ["class", "detailRow"; "style", "display: none"] [dataValue]
                else
                  let classValue = id + " " + id + "Col"
                  div ["class", classValue; "name", id; "style", style] [dataValue])
          |> Seq.toList        
        let rowStyle = "grid-row: " + string(rowNum + 1)
        let showOnly =
          if rowNum < rowsPerPage then
            ""
          else
            ";display: none;"
        if rowNum = 0 then
          div ["class", "headerRow"; "style", rowStyle;] nodes
        else
          div ["class", "row"; "id", string rowNum; "style", rowStyle+showOnly] nodes)
      |> Seq.toList
  gridComputed @ [footerRow 9 (Seq.length gridData)]