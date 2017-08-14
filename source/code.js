$(function() {
	$("#Grid").ejGrid({
		dataSource: ej.DataManager($("#Table1")),
    toolbarSettings : { showToolbar : true, toolbarItems : ["search"] },
    allowPaging : true,
		allowSorting : true,
    allowSearching : true,
		columns: ["FullName", "Phone", "HomeCity", "HomeState", "HomeZip"]
  });
});