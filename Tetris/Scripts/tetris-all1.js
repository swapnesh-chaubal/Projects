/* @class Block
The basic building block in the game. Each shape is made of these blocks and the board also has a matrix of them
*/
function Block(size) {
    this.size = size;
    this.color = "White";
    this.isActive = true;
    this.x = 0;
    this.y = 0;
}

/* @class ShapeRotation
Class to store a rotation for the shape
*/
function ShapeRotation() {
    this.rotation = [];
}

/* @class Block
The basic building block in the game. Each shape is made of these blocks and the board also has a matrix of them
*/
function Shape() {
    this.rotationCount = 0;
    this.x = 0;
    this.y = 0;
    this.color = "red";
    this.rotations = new Array(4);

    this.rotate = new function rotate() { }
}
/* @class Shape _T */
function Shape_T() {
   // this.rotations = new Array(4)
    this.rotations[0] = [[0, 0, 0], [0, 1, 0], [1, 1, 1]];
    this.rotations[1] = [[1, 0, 0], [1, 1, 0], [1, 0, 0]];
    this.rotations[2] = [[1, 1, 1], [0, 1, 0], [0, 0, 0]];
    this.rotations[3] = [[0, 0, 1], [0, 1, 1], [0, 0, 1]];

    this.color = "DarkMagenta";
    this.rotationCount = this.rotations.length;
}

Shape_T.prototype = new Shape();

function Shape_I() {

    this.rotations[0] = [[0, 0, 1], [0, 0, 1], [0, 0, 1]];
    this.rotations[1] = [[0, 0, 0], [0, 0, 0], [1, 1, 1]];
    this.rotations[2] = [[0, 0, 1], [0, 0, 1], [0, 0, 1]];
    this.rotations[3] = [[0, 0, 0], [0, 0, 0], [1, 1, 1]];

    this.color = "red";
    this.rotationCount = this.rotations.length;
}

Shape_I.prototype = new Shape();

/*
* @Class Shape J
*/
function Shape_J() {

    this.rotations[0] = [[1, 1, 1], [0, 0, 1], [0, 0, 0]];
    this.rotations[1] = [[0, 0, 1], [0, 0, 1], [0, 1, 1]];
    this.rotations[2] = [[0, 0, 0], [1, 0, 0], [1, 1, 1]];
    this.rotations[3] = [[0, 1, 1], [0, 0, 1], [0, 0, 1]];

    this.color = "blue";
    this.rotationCount = this.rotations.length;
}

Shape_J.prototype = new Shape();

/*
* @Class Shape L
*/
function Shape_L() {

    this.rotations[0] = [[1, 1, 1], [1, 0, 0], [0, 0, 0]];
    this.rotations[1] = [[0, 1, 1], [0, 0, 1], [0, 0, 1]];
    this.rotations[2] = [[0, 0, 0], [0, 0, 1], [1, 1, 1]];
    this.rotations[3] = [[1, 1, 0], [1, 0, 0], [1, 0, 0]];

    this.color = "orange";
    this.rotationCount = this.rotations.length;
}

Shape_L.prototype = new Shape();

/*
* @Class Shape O
*/
function Shape_O() {

    this.rotations[0] = [[0, 0, 0], [0, 1, 1], [0, 1, 1]];
    this.rotations[1] = [[0, 0, 0], [0, 1, 1], [0, 1, 1]];
    this.rotations[2] = [[0, 0, 0], [0, 1, 1], [0, 1, 1]];
    this.rotations[3] = [[0, 0, 0], [0, 1, 1], [0, 1, 1]];

    this.color = "yellow";
    this.rotationCount = this.rotations.length;
}

Shape_O.prototype = new Shape();

/*
* @Class Shape Z
*/
function Shape_Z() {

    this.rotations[0] = [[0, 0, 0], [0, 1, 1], [1, 1, 0]];
    //this.rotations[0] = [[1, 1, 1], [1, 1, 1], [1, 1, 1]];
    this.rotations[1] = [[1, 0, 0], [1, 1, 0], [0, 1, 0]];
    this.rotations[2] = [[0, 0, 0], [0, 1, 1], [1, 1, 0]];
    this.rotations[3] = [[1, 0, 0], [1, 1, 0], [0, 1, 0]];

    this.color = "green";
    this.rotationCount = this.rotations.length;
}

Shape_Z.prototype = new Shape();

/*
* @Class Shape S
*/
function Shape_S() {

    this.rotations[0] = [[0, 0, 0], [1, 1, 0], [0, 1, 1]];
    this.rotations[1] = [[0, 0, 1], [0, 1, 1], [0, 1, 0]];
    this.rotations[2] = [[0, 0, 0], [1, 1, 0], [0, 1, 1]];
    this.rotations[3] = [[0, 0, 1], [0, 1, 1], [0, 1, 0]];

    this.color = "aqua";
    this.rotationCount = this.rotations.length;
}

Shape_S.prototype = new Shape();

/*
* @Class Board
*/
function Board(size, width, height, canvas) {
    
    this.width = width / size;
    this.height = height / size;
    this.blockSize = size;
    this.canvas = canvas;
    board = new Array(width / size);

    createBoard(width / size, height / size, size);

    function createBoard(width, height, size) {
        for (i = 0; i < width; i++) {
            board[i] = new Array(height);
            for (j = 0; j < height; j++) {
                
                var block = new Block(size);
                block.x = i * size;
                block.y = j * size;
                board[i][j] = block;                
            }
        }
    }

    this.isCollision = function (shape) {
        //var rotation = shape.rotations[currentRotation % shape.rotationCount];        
        //for (i = 0; i < 3; i++) {
        //    for (j = 0; j < 3; j++) {
        //        this.canvas.strokeStyle = shape.color;
        //        this.canvas.fillStyle = shape.color;
        //        if (rotation[i][j] == 1) {                    
        //            this.canvas.fillRect((shape.x + i) * size, (shape.y + j) * size, size, size);
        //        }
        //    }
    }
    this.setMenu = function () {
        // T
        board[1][3].color = "Red";
        board[2][3].color = "Red";
        board[3][3].color = "Red";

        board[2][4].color = "Red";
        board[2][5].color = "Red";
        board[2][6].color = "Red";

        // E
        board[5][3].color = "Red";
        board[6][3].color = "Red";
        board[7][3].color = "Red";

        board[5][4].color = "Red";
        board[5][5].color = "Red";

        board[5][6].color = "Red";
        board[6][6].color = "Red";
        board[7][6].color = "Red";

        // T
        board[9][3].color = "Red";
        board[10][3].color = "Red";
        board[11][3].color = "Red";

        board[10][4].color = "Red";
        board[10][5].color = "Red";
        board[10][6].color = "Red";

        // R
        board[13][3].color = "Red";
        board[14][3].color = "Red";
        board[15][3].color = "Red";

        // I
        board[17][3].color = "Red";
        board[18][3].color = "Red";
        board[19][3].color = "Red";

        // S
        board[21][3].color = "Red";
        board[22][3].color = "Red";
        board[23][3].color = "Red";

        //board[4][3].color = "Red";
        //board[5][3].color = "Red";
        //board[6][3].color = "Red";
        ////board[7][3].color = "Red";

        //board[4][4].color = "Red";
        //board[5][4].color = "Red";
        //board[6][4].color = "Red";

        //board[5][5].color = "Red";
        //board[5][6].color = "Red";

    };

    this.drawMenu = function () {
        for (i = 0; i < width / size; i++) {
            for (j = 0; j < height / size; j++) {
                var block = board[i][j];
                if (block.color != "White") {
                    this.canvas.strokeStyle = "black";
                    this.canvas.strokeRect(block.x, block.y, size, size);
                    this.canvas.fillStyle = block.color;
                    this.canvas.fillRect(block.x, block.y, size, size);
                }
            }
        }
    };

    this.isLeftCollision = function (shape, size, currentRotation) {
        var rotation = shape.rotations[currentRotation % shape.rotationCount];        
        for (i = 0; i < 3; i++) {
            for (j = 0; j < 3; j++) {
                if (rotation[i][j] == 1) { 
                    if((shape.x + i) <= 0)
                        return true;
                }
            }
        }

        return false;
    }

    this.isRightCollision = function (shape, size, currentRotation) {
        var rotation = shape.rotations[currentRotation % shape.rotationCount];
        for (i = 0; i < 3; i++) {
            for (j = 0; j < 3; j++) {
                if (rotation[i][j] == 1) {
                    if ((shape.x + i) >= this.width - 1)
                        return true;
                }
            }
        }

        return false;
    }

    this.isVerticalCollision = function (shape, size, currentRotation) {
        var rotation = shape.rotations[currentRotation % shape.rotationCount];
        for (i = 0; i < 3; i++) {
            for (j = 0; j < 3; j++) {
                if (rotation[i][j] == 1) {
                    if ((shape.y + j) >= this.height - 1)
                        return true;
                    if (shape.x + i < this.width) {
                        if (!board[shape.x + i][shape.y + j + 1].isActive)
                            return true;
                    }
                }
            }
        }

        return false;
    }

    this.greyOutShape = function (shape, size, currentRotation) {
        var rotation = shape.rotations[currentRotation % shape.rotationCount];
        for (i = 0; i < 3; i++) {
            for (j = 0; j < 3; j++) {
                if (rotation[i][j] == 1) {
                    board[shape.x + i][shape.y + j].color = shape.color;
                    board[shape.x + i][shape.y + j].isActive = false;
                }
            }
        }
    }

    this.isRowFormed = function (level) {
        
        var rowsFormed = 0;
        var lowestRow = 0;

        for (j = 0; j < this.height; j++) {        
            var activeCount = 0;
            for (i = 0; i < this.width; i++) {
                if (!board[i][j].isActive) {
                    activeCount += 1;
                }
            }

            if(activeCount == this.width)
            {
                rowsFormed += 1;
                lowestRow = j;
                for (i = 0; i < this.width; i++) {
                    board[i][j].isActive = true;
                    board[i][j].color = "White";
                }                
            }
        }

        if (rowsFormed > 0) {
            for (j = lowestRow - 1; j >= 0; j--) {
                for (i = 0; i < this.width; i++) {
                    if (!board[i][j].isActive) {
                        board[i][j].isActive = true;
                        var temp = board[i][j].color;
                        board[i][j].color = "White";
                        board[i][j + rowsFormed].color = temp;
                        board[i][j + rowsFormed].isActive = false;
                    }
                }
            }

            score += rowsFormed * 10 * level;
        }
    };

    this.isGameOver = function (shape, size, currentRotation) {
        var rotation = shape.rotations[currentRotation % shape.rotationCount];
        for (i = 0; i < 3; i++) {
            for (j = 0; j < 3; j++) {
                if (rotation[i][j] == 1) {
                    if ((shape.y + j) >= this.height - 1)
                        return true;
                    if (shape.x + i < this.width) {
                        if (!board[shape.x + i][shape.y + j + 1].isActive)
                            return true;
                    }
                }
            }
        }

        return false;
    };

    this.draw = function (size) {
        for (i = 0; i < width/ size; i++) {
            for (j = 0; j < height/ size; j++) {
                var block = board[i][j];
                if (block.color != "White") {
                    this.canvas.strokeStyle = "black";
                    this.canvas.strokeRect(block.x, block.y, size, size);
                    this.canvas.fillStyle = block.color;
                    this.canvas.fillRect(block.x, block.y, size, size);
                }
            }
        }
    };

    this.adjustShapeBoundaries = function (shape, size, currentRotation) {
        var rotation = shape.rotations[currentRotation % shape.rotationCount];        
        for (i = 0; i < 3; i++) {
            for (j = 0; j < 3; j++) {
                this.canvas.strokeStyle = shape.color;
                this.canvas.fillStyle = shape.color;

                if (rotation[i][j] == 1) {
                    if (shape.x + i < 0) {
                        shape.x = -i;
                    }
                    if (shape.x + i >= this.width) {
                        shape.x = this.width - i - 1;
                    }
                }
            }
        }

    }

    this.drawShape = function (shape, size, currentRotation) {
        var rotation = shape.rotations[currentRotation % shape.rotationCount];        
        for (i = 0; i < 3; i++) {
            for (j = 0; j < 3; j++) {
                this.canvas.strokeStyle = "black";
                this.canvas.fillStyle = shape.color;

                if (rotation[i][j] == 1) {
                    if (shape.x + i < 0) {
                        shape.x = -i;
                    }


                    this.canvas.fillRect((shape.x + i) * size, (shape.y + j) * size, size, size);
                    this.canvas.strokeRect((shape.x + i) * size, (shape.y + j) * size, size, size);
                }
            }
        }

    };
}

function Game(level) {
    /*canvasHeight = 800;
    canvasWidth = 400;*/

    canvasHeight = 400;
    canvasWidth = 240;
    score = 0;
    var scoreBoxHeight = 40;

    FPS = 30;
    var canvas = $('#gameCanvas')[0].getContext('2d');
    $('#gameCanvas').attr('width', canvasWidth);
    $('#gameCanvas').attr('height', canvasHeight);
    size = 20;
    var board = new Board(size, canvasWidth, canvasHeight, canvas);
    var shapeT = new Shape_T();
    var shapeI = new Shape_I();
    var shapeJ = new Shape_J();
    var shapeL = new Shape_L();
    var shapeS = new Shape_S();
    var shapeZ = new Shape_Z();

    var shapeList = new Array(6);
    shapeList[0] = shapeT;
    shapeList[1] = shapeI;
    shapeList[2] = shapeJ;
    shapeList[3] = shapeZ;
    shapeList[4] = shapeL;
    shapeList[5] = shapeS;

    Shape.currentRotation = 0;
    var canvasElement = document.getElementById("gameCanvas");

    // event handlers
    canvasElement.addEventListener("click", onClick, false);
    var currentShape = new Shape();
    currentShape = getNextShape();

    background = new Image();
    background.src = "background.png";

    drawCanvas = function () {
        canvas.fillStyle = "white";
        canvas.fillRect(0, 0, canvasWidth, canvasHeight);
        canvas.strokeRect(0, 0, canvasWidth, canvasHeight);
        canvas.drawImage(background, 0, 0);
        board.draw(size);
        canvas.fillStyle = "black";
        canvas.fillRect(0, scoreBoxHeight, canvasWidth, 1);
        canvas.font = "24px serif";
        canvas.strokeText("Score :" + score, 10, 30);
        canvas.strokeText("Level :" + level, canvasWidth - 100, 30);
        board.drawShape(currentShape, size, Shape.currentRotation);       
    }
    
    $("#gameCanvas")
    // Add tab index to ensure the canvas retains focus
    .attr("tabindex", "0")
    .focus()
    // Mouse down override to prevent default browser controls from appearing
    .mousedown(function () { $(this).focus(); return false; })
    .keydown(function () {
        switch (event.keyCode) {
            case 37: // left key press
                if (!board.isLeftCollision(currentShape, size, Shape.currentRotation))
                    currentShape.x -= 1;
                break;
            case 38: // up key press (rotation)
                Shape.currentRotation += 1;
                break;
            case 39: // right key press
                if (!board.isRightCollision(currentShape, size, Shape.currentRotation))
                    currentShape.x += 1;
                break;
            case 40: // down key press
                if (!board.isVerticalCollision(currentShape, size, Shape.currentRotation))
                    currentShape.y += 1;
                break;
            default:
                break; return false;
        }
    });

    function onClick(event) {
        //Shape.currentRotation += 1;
    }

    function draw() {
        drawCanvas();
    };

    function update() {
        board.adjustShapeBoundaries(currentShape, size, Shape.currentRotation);
        if (!board.isVerticalCollision(currentShape, size, Shape.currentRotation)) {
            currentShape.y += 1;
            board.isRowFormed(level);

            //if (board.isGameOver()) {
            //    clearInterval(gameInterval);
            //}
        }
        else {
            board.greyOutShape(currentShape, size, Shape.currentRotation);
            //alert(score);
            currentShape = getNextShape();
            currentShape.x = 0;
            currentShape.y = 0;
        }



    };

    function getNextShape() {
        var random = Math.floor((Math.random() * shapeList.length - 1) + 1);
        shapeList[random].x = (canvasWidth / size / 2 )- 1;
        return shapeList[random];
    };

    this.startGame = function () {
        gameInterval = setInterval(function () {            
            update();
            draw();
        }, 1000 / level + 2);
        
    };
}
