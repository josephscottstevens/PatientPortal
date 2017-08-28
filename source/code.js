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
    //var dataNames = t.columns.map(t => t.getAttribute("name"));
    var show = true;
    dataValues.forEach(function (dv, idxDv) {
      if (dv.includes(filterValues[idxDv]) == false) {
        show = false;
      }
    });
    t.filterShow = show;
  });
  ShowFirst100();
}

function ShowFirst100() {
  var i = 0;
  data.forEach(function (t, idx) {
    var row = document.getElementById(t.rowId);
    if (t.filterShow && i < 100) {
      row.style.display = "";
      row.style.gridRow = i+2;
      i++;
    } else {
      row.style.display = "none";
      row.style.gridRow = 1000;
    }
  });
}

var sortOrder = 0;
var sortCol = "";
function SortByNumber(a, b) {
  var x = a.columns[sortCol].innerHTML;
  var y = b.columns[sortCol].innerHTML;
  return (x - y) * sortOrder;
}

function SortByString(a, b) {
  var x = a.columns.filter(t => t.getAttribute("name") == sortCol)[0].innerHTML.toLowerCase();
  var y = b.columns.filter(t => t.getAttribute("name") == sortCol)[0].innerHTML.toLowerCase();
  return (x < y ? -1 : x > y ? 1 : 0) * sortOrder;
}

function clearSort() {
  document.querySelectorAll(".sortBtn").forEach(t => t.src = "content\\noArrow.png");
}

function sortMe(e, sortFunc) {
  if (e.target.nodeName === "INPUT") return;
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
  data.sort(sortFunc);
  ShowFirst100();
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

var data;

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
  websocket = new WebSocket("ws://"+window.location.host+"/websocket");
  websocket.onmessage = function(evt) {
    //location.reload();
  };
}
window.addEventListener("load", init, false);