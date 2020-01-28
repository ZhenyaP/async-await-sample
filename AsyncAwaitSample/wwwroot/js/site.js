// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.

$(function () {
    $("#helloWorldAsyncBtn").click(function () {
        $.get({
            url: "/api/Home/HelloWorld",
            success: function (data) {
                $("#helloWorldResult").html(data);
            }
        });
    });
});
