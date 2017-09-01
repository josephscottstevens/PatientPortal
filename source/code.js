var rowsPerPage;
var data;
var pagingData;
var currentPage = 0;
var currentPageBlock = 0;
const pageBlockSize = 10;

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
  document.querySelectorAll(".pagingItemActive").forEach(t => t.classList.remove("pagingItemActive"));
  if (parseInt(page)) {
    currentPage = parseInt(page);
  } else {
    switch (page) {
      case 'pageFirst': 
        currentPage = 0;
        currentPageBlock = 0;
        break;
      case 'pagePrevious': 
        currentPage -= 1;
        if (currentPage < 0) {
          currentPage = pageBlockSize - 1;
          currentPageBlock--;
        }
        break;
      case 'pageBlockPrevious': 
        currentPage = pageBlockSize - 1;
        currentPageBlock -= 1;
        break;
      case 'pageBlockNext': 
        currentPage = 0;
        currentPageBlock += 1;
        break;
      case 'pageNext': 
        currentPage += 1;
        if (currentPage >= pageBlockSize) {
          currentPage = 0;
          currentPageBlock++;
        }
        break;
      case 'pageLast': 
        currentPage = 0;
        currentPageBlock = pagingData.length - 1;
        break;
    }
  }
  document.getElementById("page" + (currentPage + 1)).classList.add("pagingItemActive");
  UpdatePagingFooter();
  ShowRowsByRowCount();
}

function UpdatePagingFooter() {
  document.querySelectorAll(".pagingItem").forEach(t => t.classList.add("pagingHide"));
  //pagingData[currentPageBlock].forEach(t => document.getElementById("page" + t).classList.remove("pagingHide"));
  for (var i = 0; i < pageBlockSize; i++) {
    if (pagingData[currentPageBlock][i]) {
      
      
      document.getElementById("page" + (i+1)).innerHTML = pagingData[currentPageBlock][i];
      document.getElementById("page" + (i+1)).style.display = "inline-block";
    } else {
      document.getElementById("page" + (i+1)).style.display = "none";
    }
  }

  if (currentPageBlock == 0) {
    document.getElementById("pageFirst").classList.add("pagingItemDisabled");
    document.getElementById("pagePrevious").classList.add("pagingItemDisabled");
    document.getElementById("pageBlockPrevious").classList.add("pagingHide");
  } else {
    document.getElementById("pageFirst").classList.remove("pagingItemDisabled");
    document.getElementById("pagePrevious").classList.remove("pagingItemDisabled");
    document.getElementById("pageBlockPrevious").classList.remove("pagingHide");
  }

  if (pagingData[currentPageBlock].length < pageBlockSize) {
    document.getElementById("pageBlockNext").style.display = "none";
    document.getElementById("pageNext").classList.add("pagingItemDisabled");
    document.getElementById("pageLast").classList.add("pagingItemDisabled");
  } else {
    document.getElementById("pageBlockNext").style.display = "inline-block";
    document.getElementById("pageNext").classList.remove("pagingItemDisabled");
    document.getElementById("pageLast").classList.remove("pagingItemDisabled");
  }
}

function ShowRowsByRowCount() {
  var visibleRows = [...document.querySelectorAll(".row")].filter(t => t.style.display != "none");
  var visibleData = data.filter( t => t.filterShow);
  
  var begin = (currentPageBlock * pageBlockSize) + currentPage;
  var end = begin + pageBlockSize;
  var rowsToShow = visibleData.slice(begin, end);
  visibleRows.forEach(t => t.style.display = "none");

  rowsToShow.forEach(function (t, idx) {
    var row = document.getElementById(t.rowId);
    row.style.display = "";
    row.style.gridRow = idx+2;  
  });
  document.getElementById("footer").style.gridRowStart = pageBlockSize + 2;
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
var createGroupedArray = function(arr, chunkSize) {
    var groups = [], i;
    for (i = 0; i < arr.length; i += chunkSize) {
        groups.push(arr.slice(i, i + chunkSize));
    }
    return groups;
}

function calcPagingData() {
  var allPages = 
    data.filter( t => t.filterShow)
    .filter(t => t.filterShow)
    .map(t => t.rowId);
  pagingData = createGroupedArray(allPages, pageBlockSize);
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
  rowsPerPage = [...document.querySelectorAll(".row")].filter(t => t.style.display !== "none").length;

  calcPagingData();

  websocket = new WebSocket("ws://"+window.location.host+"/websocket");
  websocket.onmessage = function(evt) {
    location.reload();
  };
}
window.addEventListener("load", init, false);