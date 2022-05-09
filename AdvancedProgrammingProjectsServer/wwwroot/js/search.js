$(function () {
    $("#search").submit(e => {
        e.preventDefault();

        let value = $("#search-box").val();

        $("#review-table-body").load("/Ratings/Search?nameToSearch=" + value);
    });
});