$(document).ready(function() {
    
    url = "ws://localhost:1337";
    w = new WebSocket(url);
    w.onopen = function() {
        console.log("Open socket on" + url);
        
        w.send("Hello server")
    }

    w.onmessage = function(e) {
        console.log(e.data.toString())
    }
    w.onclose = function(e) {
        log("closed");
    }
    w.onerror = function(e) {
        log("error");
    }

    window.onload = function() {
        document.getElementById("login").onclick = function() {
            console.log($("#username").val());
            w.send(document.getElementById("username").value + "," + document.getElementById("password").value);
        }
    }


    //t = tcpclient('127.0.0.1', 1337, 'Timeout', 30)
/*
    function WebSocketTest() {
        
        // Let us open a web socket
        var ws = new WebSocket("ws://127.0.0.1:1337/echo");
            
        ws.onopen = function() {
            
            // Web Socket is connected, send data using send()
            ws.send("Message to send");
            alert("Message is sent...");
        };
        
        ws.onmessage = function (evt) { 
            var received_msg = evt.data;
            $("p").text(received_msg)
            alert("Message is received...");
        };
        
        ws.onclose = function() { 
            
            // websocket is closed.
            alert("Connection is closed..."); 
        }
    }

    WebSocketTest() 
*/
})