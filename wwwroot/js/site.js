// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function uploadFile() {
    alert("test1")
    var blobFile;
    try {
       // $('#filechooser')
        var file = $('#filechooser');
        file.css("border", "3px solid red");
        alert(file.type);
        blobFile = file.files[0];
    } catch (e) {
        alert(e);
    }
    
    var formData = new FormData();
    formData.append("fileToUpload", blobFile);
    alert("test2")
    $.ajax({
        url: "/",
        type: "POST",
        data: "this was sent from js.",
        processData: false,
        contentType: false,
        success: function (response) {
            alert("succ");
        },
        error: function (jqXHR, textStatus, errorMessage) {
            alert("тще succ");
            console.log(errorMessage); // Optional
        }
    });
}