let search = (e) => {
    e.preventDefault();

    let value = $("#search-box").val();

    $("#review-table-body").load("/Ratings/Search?nameToSearch=" + value);
}

document.getElementById("search").addEventListener("submit", e => search(e));
document.getElementById("search-box").addEventListener("keyup", e => search(e));

