/// <reference path="../jquery-3.1.1.js" />


$(window).scroll(function () {
    sessionStorage.scrollTop = $(this).scrollTop();
});


$(document).ready(function () {

    if (sessionStorage.scrollTop != "undefined") {
        $(window).scrollTop(sessionStorage.scrollTop);
    }

    $('.add_tag_action').click(function () {
        $(this).submit();
    });

    $('.edit_fields_action').on('focusout', function () {
        $(this).submit();
    });

    $('.remove_tag').click(function () {
        $(this).closest($('.remove_tag_action')).submit();
    });

    $('.remove_card').click(function () {
        $(this).closest($('.remove_card_action')).submit();
    });


    function removeTagFromDeck(deckID, tagName) {

        var args = {};
        args.DeckID = deckID;
        args.TagName = tagName;

        $.ajax({
            type: "POST",
            url: "~/Deck/RemoveDeckTag",
            contentType: "application/json; charset=utf-8",
            data: args,
            dataType: "json",
            success: function (msg) {
                // Something afterwards here

            }
        });

    }
});
