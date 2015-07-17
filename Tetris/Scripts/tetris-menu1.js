function Menu() {
    canvasHeight = 400;
    canvasWidth = 320;
    this.image1 = new Image();
    this.image1.src = "Tetris1.png";

    this.image2 = new Image();
    this.image2.src = "Tetris2.png";

    this.image3 = new Image();
    this.image3.src = "Tetris3.png";

    this.image4 = new Image();
    this.image4.src = "Tetris4.png";

    imageArray = new Array(4);
    imageArray[0] = this.image1;
    imageArray[1] = this.image2;
    imageArray[2] = this.image3;
    imageArray[3] = this.image4;
    index = 0;

    styleArray = new Array(4);
    styleArray[0] = "blue";
    styleArray[1] = "red";
    styleArray[2] = "green";
    styleArray[3] = "yellow";
    index = 0;

    var level = new Image();
    level.src = "Level.png";

    var newGame = new Image();
    newGame.src = "NewGame.png";

    var o = new Image();
    o.src = "o.png";

    var options = new Image();
    options.src = "Options.png";

    selectArray = new Array(2);
    selectArray[0] = "white";
    selectArray[1] = "red";
    selectionIndex = 0;

    background = new Image();
    background.src = "background.png";
    var oY = 0;
   function drawMenu() {
       var y = 70;
       
       var ctx = $('#gameCanvas')[0].getContext('2d');
       $('#gameCanvas').attr('width', canvasWidth);
       $('#gameCanvas').attr('height', canvasHeight);
       ctx.drawImage(background, 0, 0);
       ctx.fillStyle = styleArray[index++ % 4];
       //canvas.font = "bold 78px SketchFlow Print";
       ctx.font = "bold 78px Stencil";
       ctx.fillText("Tetris", 20, y);

       y += 75;
       ctx.fillStyle = selectArray[selectionIndex];
       ctx.font = "bold 48px SketchFlow Print";
       ctx.fillText("New Game", 20, y);

       y += 75;
       ctx.fillStyle = selectArray[(selectionIndex + 1) % 3];
       ctx.font = "bold 48px SketchFlow Print";
       ctx.fillText("Controls", 20, y);

       y += 75;
       ctx.fillStyle = selectArray[(selectionIndex + 2) % 3];
       ctx.font = "bold 48px SketchFlow Print";
       ctx.fillText("About", 20, y);
   };

   var selection = "";
    $("#gameCanvas")
    // Add tab index to ensure the canvas retains focus
    .attr("tabindex", "0")
    .focus()
    // Mouse down override to prevent default browser controls from appearing
    .mousedown(function () { $(this).focus(); return false; })
    .keydown(function () {
        switch (event.keyCode) {
            case 38: // up key press
                oY -= 1;
                break;
            case 40: // down key press
                oY += 1;
                break;
            case 13: // enter key press
                switch ((oY % 3)) {
                    case 0: // New Game
                        selection = "NewGame";
                        break;
                    case 1: // Level
                        selection = "Controls";
                        break;
                    case 2: // Options
                        selection = "About";
                        break;
                    default:
                        break;
                }
                break;
            default:
                break; return false;
        }
    });


    this.show1 = function () {
        var interval = setInterval(function () {
            
            drawMenu();
            if (selection != "") {
                clearInterval(interval);

                var game = new Game(3);
                game.startGame();
            }
        }, 1000 / 2);

    };
}