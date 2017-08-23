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
  document.querySelectorAll(targetClass).forEach(function(t) {
    if (t.innerText.toLowerCase().includes(e.value.toLowerCase())) {
      t.parentNode.style.display = "";
    } else {
      t.parentNode.style.display = "none";
    }
  });
}

function compareNumbers(a, b) {
  return a.value - b.value;
}

function sortMe(e) {
  var targetClass = "." + e.target.id + "Col";
  var data = [];
  document.querySelectorAll(targetClass).forEach(function(t) {
    data.push({value:t.innerText, rowNum:t.parentNode.getAttribute("data-row")});
  });
  var sortedData = data.sort(compareNumbers);
  sortedData.forEach(function (val, idx) {
    var rowSelect = "[data-row = '" + val.rowNum + "']";
    document.querySelector(rowSelect).style.gridRow = idx+2;
  });
}

function toggleMe(e) {
  var siblings = e.target.parentNode.parentNode.children;
  var detailRow = Array.prototype.filter.call(siblings, function(child){
    return child.className == "detailRow";
  })[0];
  if (e.target.src.includes("rightArrow.png")) {
    e.target.src = "downArrow.png";
    detailRow.style.display = "";
  } else {
    e.target.src = "rightArrow.png";
    detailRow.style.display = "none";
  }
}