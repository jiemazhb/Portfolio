function searchFun() {
    var $text = $("#searchStr-box").val();
    if ($text.trim().length != 0) {
        return true;
    }
    else {
        return false;
    }
}
