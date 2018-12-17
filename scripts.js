$(document).ready(function() {
    var url = "https://api.nasa.gov/planetary/apod?api_key=Nope"
    var masterBlaster = $("#masterBlaster");
    var tempPctScrolled = window.pageYOffset
    var ged = $(".ged")
    var darthFader = $(".darthFader")
    var cloner = $("button.clone")

    $.get(url, function(data) {
        $("p").text(data.title) 
    })

    //BUTTONS
    darthFader.click(function(){
       $(this).siblings(ged).not("button").fadeOut()
    })

    cloner.click(function(){
        $(this).parent().clone(true).addClass("chip").insertAfter($(this).parent())
        ged = $(".ged")
        darthFader = $(".darthFader")
        cloner = $("button.clone")
    })

    masterBlaster.click(function(){
        ged.fadeIn()
        $.get(url, function(data) {
            ged.attr("src",data.hdurl)
        })
/*
        $.get(url, function(data) {
            ged.eq(Math.floor((Math.random() * 3))).attr("src",data.hdurl)
        })
*/
    })

    $("#order66").click(function(){
        $(".chip").remove()
    })


    //SCROLL
    function amountscrolled(){
        var winheight = $(window).height()
        var docheight = $(document).height()
        var scrollTop = $(window).scrollTop()
        var trackLength = docheight - winheight
        var pctScrolled = Math.floor(scrollTop/trackLength * 100)
        $("img#bar").css("width",pctScrolled + "%");
        //$("progress").val(pctScrolled);
        if (tempPctScrolled < pctScrolled) {
            $("#menu").finish().css("top","-20px")
        } else if (tempPctScrolled > pctScrolled) {
            $("#menu").finish().css("top","0px")

        }
        tempPctScrolled = pctScrolled
    }
     
    $(window).on("scroll", function(){
        amountscrolled()
    })


    //SNAKE
    var direction
    var canvas = document.getElementById("snakeCanvas")
    var ctx = canvas.getContext("2d")
    var x = canvas.width/2-50/2
    var y = canvas.height/2
    var xfood
    var yfood

    do {
        xfood = Math.floor((Math.random() * canvas.width-10))
        yfood = Math.floor((Math.random() * canvas.height-10))    
    } while (Touching());

    function CalculateFood() {
        while (Touching()) {
            xfood = Math.floor((Math.random() * canvas.width-10))
            yfood = Math.floor((Math.random() * canvas.height-10))
        } 
    }

    function Touching () {
        var touching = false

        if ((x > xfood-50 && x < xfood+20) && (y > yfood-50 && y < yfood+20)) {
            touching = true
        }

        return touching

    }

    $(document).keydown(function(e) {
        switch(e.which) {
            case 65: // left
                direction = (direction != 3 ? 1 : 3)
            break;
            case 87: // up
            direction = (direction != 4 ? 2 : 4)
            break;
            case 68: // right
            direction = (direction != 1 ? 3 : 1)
            break;
            case 83: // down
            direction = (direction != 2 ? 4 : 2)
            break;
            default: return; // exit this handler for other keys
        }
    });

    var speed = 10
    setInterval(function(){
        switch(direction) {
            case 1:
                x -= speed
            break;
            case 2:
                y -= speed
            break;
            case 3:
                x += speed
            break;
            case 4:
                y += speed
            break;
            default:
        }
        x = (x < 0-40 ? 390 : x)
        x = (x > 390 ? 0-40 : x)
        y = (y < 0-40 ? 390 : y)
        y = (y > 390 ? 0-40 : y)

        CalculateFood()
        ctx.clearRect(0, 0, canvas.width, canvas.height);
        ctx.fillStyle = "#0000FF"
        ctx.fillRect(x,y,50,50)
        ctx.fillStyle = "#FF0000"
        ctx.fillRect(xfood,yfood,20,20)

    }, 100);

    if (x==1) {
        
    }

})