function allowDrop(ev) {
  ev.preventDefault();
}

function drag(ev) {
  ev.dataTransfer.setData("text", ev.target.id);
}

function drop(ev) {
  ev.preventDefault();
  var data = ev.dataTransfer.getData("text");
  var sourceDiv = document.getElementById(data);
  var sourceCol = sourceDiv.style.gridColumnStart;
  var sourceClass = "." + sourceDiv.id;
  var targetDiv = ev.target;
  var targetCol = targetDiv.style.gridColumnStart;
  // Bit of a gotcha here, if you give input an ID, this will break;
  var targetClass = (targetDiv.id != "") ? "." + targetDiv.id : "." + targetDiv.parentNode.id;

  document.querySelectorAll(sourceClass).forEach(t => t.style.gridColumnStart = targetCol);
  document.querySelectorAll(targetClass).forEach(t => t.style.gridColumnStart = sourceCol);
}

function filterMe(e) {
  var targetClass = "." + e.parentNode.id + "Col";
  var i = 0;
  var filterText = e.value.toLowerCase();
  document.querySelectorAll(targetClass).forEach(function(t) {
    if (t.innerHTML.toLowerCase().includes(filterText) && i < 100) {
      t.parentNode.style.display = "";
      i++;
    } else {
      t.parentNode.style.display = "none";
    }
  });
}

var sortOrder = 0;
var sortCol = "";
function SortByNumber(a, b) {
  return (a.value - b.value) * sortOrder;
}

function SortByString(a, b) {
  var x = a.columns[sortCol].innerHTML.toLowerCase();
  var y = b.columns[sortCol].innerHTML.toLowerCase();
  return (x < y ? -1 : x > y ? 1 : 0) * sortOrder;
}

function clearSort() {
  document.querySelectorAll(".sortBtn").forEach(t => t.src = "content\\noArrow.png");
}

function sortMe(e, sortFunc) {
  if (e.target.nodeName === "INPUT") return;
  var targetClass = "." + e.target.id + "Col";
  var toggleBtn = e.target.querySelector(".sortBtn");
  sortCol = e.target.id;
  if (toggleBtn.src.includes("noArrow.png")) {
    clearSort();
    toggleBtn.src = "content\\downArrow.png";
    sortOrder = 1;
  } else if (toggleBtn.src.includes("downArrow.png")) {
    clearSort();
    toggleBtn.src = "content\\upArrow.png";
    sortOrder = -1;
  } else if (toggleBtn.src.includes("upArrow.png")) {
    clearSort();
    toggleBtn.src = "content\\noArrow.png";
    sortOrder = 0;
  }
  var sortedData = data.sort(sortFunc);
  var element;
  sortedData.forEach(function (val, idx) {
    element = document.getElementById(val.rowId);
    if (idx < 100) {
      element.style.display = "";
    } else {
      element.style.display = "none";
    }
    element.style.gridRow = idx+2;
  });
}

function toggleMe(e) {
  var siblings = e.target.parentNode.parentNode.children;
  var detailRow = Array.prototype.filter.call(siblings, function(child){
    return child.className == "detailRow";
  })[0];
  if (e.target.src.includes("rightArrow.png")) {
    e.target.src = "content\\downArrow.png";
    detailRow.style.display = "";
  } else {
    e.target.src = "content\\rightArrow.png";
    detailRow.style.display = "none";
  }
}

var data;

function init()
{
  data = [];
  document.querySelectorAll(".row").forEach(function(t) {
    data.push({rowId:t.id, filterShow:true, displayOrder:t.id, columns:t.children});
  });
  websocket = new WebSocket("ws://"+window.location.host+"/websocket");
  websocket.onmessage = function(evt) {
    //location.reload();
  };
}
window.addEventListener("load", init, false);