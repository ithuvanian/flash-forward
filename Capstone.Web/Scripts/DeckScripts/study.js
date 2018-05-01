
var flipped = false;


function flipCard() {
    $('.cardFront').addClass('hiddenSide');
    $('.cardBack').removeClass('hiddenSide');
    $('.flip').addClass('hiddenNav');
    $('.markFrame').removeClass('hiddenNav');
}


function nextCard() {
    $('.cardBack').addClass('hiddenSide');
    $('.cardFront').removeClass('hiddenSide');
    $('.markFrame').addClass('hiddenNav');
    $('.flip').removeClass('hiddenNav');

    var thisCard = $('.activeCard');
    var nextCard = thisCard.next();

    thisCard.removeClass('activeCard').addClass('oldCard');
    nextCard.removeClass('newCard').addClass('activeCard');
}

$(document).ready(function () {

    $('.resultsFrame').hide();
    
    var cardCount = parseInt($('#cardCount').data('name'));
    $('#possibleScore').text(cardCount);

    var totalCorrect = 0;
    $('.totalCorrect').text(totalCorrect);
    var cardsViewed = 0;
    $('.totalViewed').text(cardsViewed);


    $('.studyCard').first().removeClass('newCard').addClass('activeCard');

    $('.markRight').click(function () {
        totalCorrect++;
        $('.totalCorrect').text(totalCorrect);
    });

    $('.finish').click(function () {
        $('.studyFrame').hide();
        $('#finalScore').text(totalCorrect);
        $('#possibleScore').text(cardsViewed);
        $('.resultsFrame').show();
    });

    $('.studyNav').click(function () {

        console.log(cardsViewed);

        if (flipped == false) {
            flipCard();
            flipped = true;

        } else {
            cardsViewed++;
            $('.totalViewed').text(cardsViewed);

            if (cardsViewed == cardCount) {
                $('.studyFrame').hide();
                $('#finalScore').text(totalCorrect);
                $('.resultsFrame').show();
            }
            else {
                nextCard();
                flipped = false;
            }
        }
    });

});