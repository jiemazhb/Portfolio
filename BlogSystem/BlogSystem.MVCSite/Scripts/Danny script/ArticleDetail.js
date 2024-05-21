
function btnGoodHandler(btn) {
    var articleId = $("#current-articleId").val().trim();
        $.ajax({
            url: "/Article/GoodCount/" + articleId,
            type: "post",
            success: function (data) {
                if (data.result == "error") {
                    alert("登录之后才能点赞~！");
                }
                else {
                    if ($("#like").text() == data) {
                        alert("不能重复点赞！");
                    } else {
                        $("#like").text(data);
                    }
                }
            },
            error: function (e) {
                alert("点赞失败，请稍后再试..");
            }
        })
}

function btnBadHandler(btn) {
    var articleId = $("#current-articleId").val().trim();
        $.ajax({
            url: "/Article/BadCount/?articleid=" + articleId,
            type: "post",
            success: function (data) {
                if (data.result == "error") {
                    alert("登录之后才能踩~！");
                }
                else {
                    if ($("#dislike").text() == data) {
                        alert("每人只能踩一脚！");
                    } else {
                        $("#dislike").text(data);
                    }
                }
            },
            error: function (e) {
                alert("踩失败，请稍后再试..");
            }
        })
}

function addComment() {
    var articleId = $("#current-articleId").val().trim();
        $.ajax({
            url: "/Article/AddComment/",
            type: "post",
            data: {
                Id: articleId,
                Content: $("#commentContent").val()
            }
        }).done(function (param) {
            if (param.result == "error") {
                alert("登录后才能发表评论~！");
            } else if (param.result == "ok") {
                location.reload();
            }
        }).fail(function () {
            alert("评论失败，请稍后再试...");
        })
}
