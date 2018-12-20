$(document).ready(function() {
    
    url = "ws://localhost:1337";
    w = new WebSocket(url);
    w.onopen = function() {
        console.log("Open socket on" + url);
        
    }

    w.onmessage = function(e) {
        $("button").text(e);
        if (e.data=="Luk") {
            close();
        }
    }
    w.onclose = function(e) {
        console.log("closed");
        //close();
    }
    w.onerror = function(e) {
        //console.log("error");
    }

    document.getElementById("login").onclick = function() {
        if ($("#username").val().toLowerCase().indexOf(',') == -1 && $("#password").val().toLowerCase().indexOf(',') == -1) {
            w.send($("#username").val() + "," + $("#password").val());
        } else {
            console.log("Fejl");
            $("#response").text("You can't have comma in username or password");
        }
    }

    window.onload = function() {

    }


})