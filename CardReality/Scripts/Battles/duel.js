$(document).ready(function () {
  function updateTime(turnTime) {
        var now = new Date();
        leftSeconds = (turnTime - now) / 1000;
        if (leftSeconds <= 0) {
            battle.server.timeExceeded(battleId);
        }
    };

    updateTime(turnDate);

    setInterval(function () {
        $("#seconds").text(parseInt(--leftSeconds));
        if (leftSeconds <= 0) {
            battle.server.timeExceeded(battleId);
        }
    }, 1000);


    $(".card").click(function () {
        var id = Number($(this).data("id"));
        battle.server.setCard(battleId, id);
    });


    $(".select-card").click(function () {
        selectedCard = Number($(this).data("id"));
        console.log(selectedCard);
    });

    $(".foreign-card").click(function () {
        console.log(selectedCard);
        if (selectedCard == null) {
            return false;
        }

        var thisId = Number($(this).data("id"));
        console.log(thisId);
        if (!thisId) {
            return false;
        }

        battle.server.attackCard(battleId, selectedCard, thisId);
    });

    $("#direct-attack").click(function () {
        if (selectedCard == null) {
            return false;
        }

        battle.server.attackPlayer(battleId, selectedCard);
    });

    battle.client.notifyLifePoints = function (lifePoints) {
        $("#attacker-life-points").text(lifePoints.Attacker);
        $("#defender-life-points").text(lifePoints.Defender);
    };


    $("#end-turn").click(function () {
        battle.server.endTurn(battleId);
    });

    battle.client.changeTurn = function (playerOnTurnId) {
        $("#attacker-on-turn").toggle();
        $("#defender-on-turn").toggle();
        $("#end-turn").toggle();
        turnDate = new Date();
        turnDate.setSeconds(turnDate.getSeconds() + 60);
        updateTime(turnDate);
    };

    battle.client.pushCard = function (card) {
        $(".hand").append("<div class='card' data=id='" + card.Id + "'><a href='#'>" + card.Name + "</a></div>");
    };

    battle.client.removeCard = function (card) {
        $(".card").filter(function () { return $(this).data("id") == card.Id }).first().remove();
    };

    battle.client.battleEnded = function (winner) {
        alert(winner.Name + " has won the duel");
    };

    battle.client.setCard = function (field) {
        $(".card-place").filter(function () {
            return $(this).data("rowid")
                == field.Row && $(this).data("colid") == field.Col;
        })
           .first()
           .html("<a href='#' class='select-card' data-id='" + field.CardId + "'>" + field.CardName + "</a>");
    };

    $.connection.hub.start();
});