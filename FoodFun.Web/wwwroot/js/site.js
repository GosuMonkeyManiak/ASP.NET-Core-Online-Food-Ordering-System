$('#imageInput').focusout(() => {
    $('#image').attr('src', $('#imageInput').val());
});

$(document).ready(() => {
    $('#image').attr('src', $('#imageInput').val());
});

var menu_btn = document.querySelector("#menu-btn");
var sidebar = document.querySelector("#sidebar");
var container = document.querySelector(".my-container");

menu_btn.addEventListener("click", () => {
    sidebar.classList.toggle("active-nav");
    container.classList.toggle("active-cont");
})