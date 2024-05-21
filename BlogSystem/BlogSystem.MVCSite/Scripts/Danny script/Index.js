$(function () {
    //获得top 5的文章,并调用createButton动态创建列表
    $.ajax({
        url: "/Article/GetTopFiveArticle",
        success: function (data) {

            $.each(data, function (index, val) {
                var $item = createButton(index, val);
                $("#topFive-container").append($item);
            })
        }
    })
    //动态创建li
    function createButton(index, val) {
        var $newList = "<li>" + (index + 1) + "<a id =" + val.Id + ">" + val.Title + "</a></li>";
        return $newList;
    }
    //动态监听<a>的点击并将返回结果显示出来
    $("#topFive-container").delegate("a", "click", function () {
        var articleId = $(this).attr("id");
        $.ajax({
            url: "/Article/GetOneArticleOfTopFive/?articleId=" + articleId,
            success: function (data) {              
                var createTime = ChangeDateFormat(data.CreateTime);
                $("#article-Title").text(data.Title);
                $("#index-content").html(data.Content);   //用Html自动就解码了。不要用text
                $("#title-category").text("【" + data.CategoryName + "】");
                $("#title-dataTime").text(createTime);
            },
            error: function () {
                alert("无法获得该文章");
            }
        })
    })
    //修改ajax返回的日期格式
    function ChangeDateFormat(cellval) {
        var date = new Date(parseInt(cellval.replace("/Date(", "").replace(")/", ""), 10));
        var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
        var currentDate = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
        var hours = date.getHours() < 10 ? "0" + date.getHours() : date.getHours();
        var min = date.getMinutes() < 10 ? "0" + date.getMinutes() : date.getMinutes();
        var sec = date.getSeconds() < 10 ? "0" + date.getSeconds() : date.getSeconds();
        var format = hours > 12 ? "PM" : "AM";
        return month + "/" + currentDate + "/" + date.getFullYear() + " " + hours + ":" + min + ":" + sec + " " + format;
    }
})