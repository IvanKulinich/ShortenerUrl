// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$("#submit").click(function (ev) {
    $.ajax({
        type: "POST",
        url: "/",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            "Url": $("#urlshort").val()
        }),

        success: function (response) {
            location.reload();
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
});