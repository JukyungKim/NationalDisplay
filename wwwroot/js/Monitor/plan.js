// "use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

window.onload = function(){
    init();
}

function init() {
    alert("안녕~ 세계야!!");
}

function requestSensorData(){
    var user = "u";
    var message = "m";
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
}

setInterval(() => { 
    console.log("Request sensor data");
    requestSensorData(); 
}, 2000);

connection.start().then(function () {
    // document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

connection.on("ReceiveMessage", function (user, message) {
    // var li = document.createElement("li");
    // document.getElementById("messagesList").appendChild(li);
    document.getElementById("temp0").textContent = `온도:${user}`;// `${user} says ${message}`;

    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    // li.textContent = `${user} says ${message}`;
});

