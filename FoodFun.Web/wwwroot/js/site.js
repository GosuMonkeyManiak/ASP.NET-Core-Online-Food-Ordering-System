$('#imageInput').focusout(() => {
    $('#image').attr('src', $('#imageInput').val());
});

$(document).ready(() => {
    $('#image').attr('src', $('#imageInput').val());
});
