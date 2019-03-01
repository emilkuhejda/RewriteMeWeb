$(document).ready(function () {
    $("#dataTable").DataTable({ searching: false });

    var sidebar = $(".sidebar.navbar-nav");
    var wrapperHeight = $("#wrapper").height();

    if (sidebar.height() < wrapperHeight) {
        sidebar.height(wrapperHeight);
    }
});