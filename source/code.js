var rowsPerPage;
var data;
var currentPage = 1;

function allowDrop(ev) {
  ev.preventDefault();
}

function drag(ev) {
  ev.dataTransfer.setData("text", ev.currentTarget.id);
}

function drop(ev) {
  ev.preventDefault();
  var data = ev.dataTransfer.getData("text");
  var sourceDiv = document.getElementById(data);
  var sourceCol = sourceDiv.style.gridColumnStart;
  var sourceClass = "." + sourceDiv.id;
  var targetDiv = ev.currentTarget;
  var targetCol = targetDiv.style.gridColumnStart;
  var targetClass = "." + targetDiv.id 

  document.querySelectorAll(sourceClass).forEach(t => t.style.gridColumnStart = targetCol);
  document.querySelectorAll(targetClass).forEach(t => t.style.gridColumnStart = sourceCol);
}

function filterMe(e) {
  var targetClass = "." + e.parentNode.id + "Col";
  var i = 0;
  var filterText = e.value.toLowerCase();
  var filterValues = [...document.querySelectorAll(".filterInput")].map(fv => fv.value.toLowerCase());
  data.forEach(function (t, idx) {
    var dataValues = t.columns.map(t => t.innerHTML.toLowerCase());
    var show = true;
    dataValues.forEach(function (dv, idxDv) {
      if (dv.includes(filterValues[idxDv]) == false) {
        show = false;
      }
    });
    t.filterShow = show;
  });
  ShowRowsByRowCount();
}

function GoToPage(page) {
  var nextPage;
  if (parseInt(page)) {
    nextPage = parseInt(page);
  } else {
    switch (page) {
      case 'pageFirst': 
        nextPage = 1;
        break;
      case 'pagePrevious': 
        nextPage = currentPage - 1;
        break;
      case 'pageBlockPrevious': 
        nextPage = (Math.floor((currentPage - rowsPerPage + 1)/10)*10);
        break;
      case 'pageBlockNext': 
        nextPage = (Math.floor((currentPage + rowsPerPage + 1)/10)*10);
        break;
      case 'pageNext': 
        nextPage = currentPage + 1;
        break;
      case 'pageLast': 
        nextPage = Math.floor(data.filter( t => t.filterShow).length / rowsPerPage)-1;
        break;
    }
  }
  UpdatePagingFooter(nextPage);
  ShowRowsByRowCount();
}

function UpdatePagingFooter(nextPage) {
  document.getElementById("page" + currentPage).classList.remove("pagingItemActive");
  document.getElementById("page" + nextPage).classList.add("pagingItemActive");
  var currentPageBlock = (Math.floor((currentPage / 10))+1)*10;
  var nextPageBlock = (Math.floor((nextPage / 10))+1)*10;
  if (currentPageBlock !== nextPageBlock) {
    if (currentPageBlock > nextPageBlock) {
      [currentPageBlock, nextPageBlock] = [nextPageBlock, currentPageBlock];
      nextPageBlock -= 10;
      currentPageBlock -= 9;
    } 
    document.querySelectorAll(".pagingItem").forEach(t => t.classList.add("pagingHide"));
    for (var i = currentPageBlock; i < nextPageBlock; i++) {
      document.getElementById("page" + i).classList.remove("pagingHide");
    }
  }
  currentPage = nextPage;
  var renderablePages = data.filter( t => t.filterShow).length;
  if (currentPage <= rowsPerPage) {
    document.getElementById("pageFirst").classList.add("pagingItemDisabled");
    document.getElementById("pagePrevious").classList.add("pagingItemDisabled");
    document.getElementById("pageBlockPrevious").classList.add("pagingHide");
  } else {
    document.getElementById("pageFirst").classList.remove("pagingItemDisabled");
    document.getElementById("pagePrevious").classList.remove("pagingItemDisabled");
    document.getElementById("pageBlockPrevious").classList.remove("pagingHide");
  }

  if (currentPage >= renderablePages - rowsPerPage) {
    document.getElementById("pageBlockNext").classList.add("pagingHide");
    document.getElementById("pageNext").classList.add("pagingItemDisabled");
    document.getElementById("pageBlockPrevious").classList.add("pagingItemDisabled");
  } else {
    document.getElementById("pageBlockNext").classList.remove("pagingHide");
    document.getElementById("pageNext").classList.remove("pagingItemDisabled");
    document.getElementById("pageBlockPrevious").classList.remove("pagingItemDisabled");
  }
}

function ShowRowsByRowCount() {
  var visibleRows = [...document.querySelectorAll(".row")].filter(t => t.style.display != "none");
  var visibleData = data.filter( t => t.filterShow);
  
  var begin = (currentPage - 1 ) * rowsPerPage;
  var end = begin + rowsPerPage;
  var rowsToShow = visibleData.slice(begin, end);
  visibleRows.forEach(t => t.style.display = "none");

  rowsToShow.forEach(function (t, idx) {
    var row = document.getElementById(t.rowId);
    row.style.display = "";
    row.style.gridRow = idx+2;  
  });
  document.getElementById("footer").style.gridRowStart = rowsPerPage * currentPage + 2;
}

function SortByNumber(a, b) {
  var x = a.columns[sortCol].innerHTML;
  var y = b.columns[sortCol].innerHTML;
  return (x - y) * sortOrder;
}

function SortByString(sortCol, sortOrder) {
  return function(a, b) {
    var x = a.columns.filter(t => t.classList.contains(sortCol))[0].innerHTML.toLowerCase();
    var y = b.columns.filter(t => t.classList.contains(sortCol))[0].innerHTML.toLowerCase();
    return (x < y ? -1 : x > y ? 1 : 0) * sortOrder;
  };
}

function clearSort() {
  document.querySelectorAll(".sortBtn").forEach(t => t.src = "content\\noArrow.png");
}

function sortMe(e, sortFunc) {
  if (e.target.nodeName === "INPUT") return; // Prevent sorting grid if target is input
  var targetClass = "." + e.currentTarget.id + "Col";
  var toggleBtn = e.currentTarget.querySelector(".sortBtn");
  sortCol = e.target.id;
  if (toggleBtn.src.includes("noArrow.png")) {
    clearSort();
    toggleBtn.src = "content\\downArrow.png";
    sortOrder = 1;
  } else if (toggleBtn.src.includes("downArrow.png")) {
    clearSort();
    toggleBtn.src = "content\\upArrow.png";
    sortOrder = -1;
  } else {
    clearSort();
    toggleBtn.src = "content\\noArrow.png";
    sortOrder = 0;
  }
  data.sort(sortFunc(e.currentTarget.id, sortOrder));
  ShowRowsByRowCount();
}

function toggleMe(e) {
  var siblings = e.currentTarget.parentNode.parentNode.children;
  var detailRow = Array.prototype.filter.call(siblings, function(child){
    return child.className == "detailRow";
  })[0];
  if (e.currentTarget.src.includes("rightArrow.png")) {
    e.target.src = "content\\downArrow.png";
    detailRow.style.display = "";
  } else {
    e.currentTarget.src = "content\\rightArrow.png";
    detailRow.style.display = "none";
  }
}

function init()
{
  data = [];
  document.querySelectorAll(".row").forEach(function(t) {
    var cols = [...t.children];
    cols.pop();     // Remove Pre Column
    cols.shift(); // Remove Detail Column
    data.push({rowId:t.id, filterShow:true, displayOrder:t.id, columns:cols});
  });
  data.pop(); // Remove header row
  rowsPerPage = parseInt(document.getElementById("footer").getAttribute("data-rows-per-page"))
  websocket = new WebSocket("ws://"+window.location.host+"/websocket");
  websocket.onmessage = function(evt) {
    location.reload();
  };
}
window.addEventListener("load", init, false);