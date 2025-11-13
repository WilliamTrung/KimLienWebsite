// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function onSelectPrimaryPicture() {
    var selected = document.getElementById("primary_pic");
    var show = document.getElementById("show_selected");
    let src = selected.options[selected.selectedIndex].value;
    console.log(src);
    show.src = src;
}
function previewImages() {
    refreshPreviewImages();
    var preview = document.querySelector('#preview');

    if (this.files) {
        [].forEach.call(this.files, readAndPreview);
    }
    
    function readAndPreview(file) {

        // File type validator based on the extension 
        if (!/\.(jpe?g|png|gif)$/i.test(file.name)) {
            return alert(file.name + " is not an image");
        }

        var reader = new FileReader();

        reader.addEventListener("load", function () {
            var image = new Image();
            /*image.src = "http://localhost/image.jpg?" + new Date().getTime();*/
            image.style = "height: auto; max-height: 300px; width:auto";
            image.title = file.name;
            image.src = this.result;
            image.setAttribute("class", "preview-image");
            preview.appendChild(image);
        });

        reader.readAsDataURL(file);
    }
    function refreshPreviewImages() {
        var images = document.getElementsByClassName("preview-image");
        while (images.length > 0) {
            images[0].remove();
        }
    }
}
document.querySelector('#img-selector').addEventListener("change", previewImages);

function onCategoryExpand(id) {
    var click = document.getElementById(id);
    $(".rotate").toggleClass("down");
    //var display = click.style.display;
    let classId = '.' + id;
    $(classId).toggleClass("show");
    //click.toggleClass("visible");
    //if (display == 'none') {
    //    click.style.display = 'block';
    //    click.style.visibility = 'visible';
    //} else {
    //    click.style.display = 'none';
    //    click.style.visibility = 'hidden';
    //}
}