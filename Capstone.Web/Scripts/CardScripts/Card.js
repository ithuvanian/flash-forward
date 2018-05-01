
$(document).ready(function () {

    $('.remove_tag, .inactive_tag').click(function () {
        $(this).submit();
    });

    $('.edit_fields_action').on('focusout', function () {
        $(this).submit();
    });

    var tagCount = parseInt($('#tagCount').data('name'));


});