// "use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/sensorHub").build();
var sensorItems = document.getElementsByClassName("myButton");
var sensorIndex = 0;
var sensorArray = new Array(20);

window.onload = function(){
    init();
}

function init() {
    // alert("안녕~ 세계야!!");

    // for(var i = 0; i < sensorItems.length; i++){
    //     var item = sensorItems[i];
    //     item.addEventListener('click', function(){
    //         var v = document.getElementById(sensorIndex.toString());
    //         console.log(v.id);
    //     })
    //     sensorIndex++;
    // }

    for(var i = 0; i < 20; i++){
        sensorArray[i] = new structSensor();
    }
}

function structSensor(){
    var id = 0;
    var smoke = 0;
    var temp = 0;
    var gas = 0;
}

function onClickSensorItem(sensorIndex){
    console.log(sensorIndex);

    // document.getElementById("smoke").setAttribute('stroke-dasharray', "10, 100");
    // document.getElementById("temp").setAttribute('stroke-dasharray', "10, 100");
    // document.getElementById("gas").setAttribute('stroke-dasharray', "10, 100");

    document.getElementById("smoke").setAttribute('stroke-dasharray', sensorArray[sensorIndex].smoke + "," + "100");
    document.getElementById("temp").setAttribute('stroke-dasharray', sensorArray[sensorIndex].temp + "," + "100");
    document.getElementById("gas").setAttribute('stroke-dasharray', sensorArray[sensorIndex].gas + "," + "100");

    document.getElementById("smoke_value").textContent = sensorArray[sensorIndex].smoke + "%";
    document.getElementById("temp_value").textContent = sensorArray[sensorIndex].temp + "ºC";
    document.getElementById("gas_value").textContent = sensorArray[sensorIndex].gas + "%";
}

function requestSensorData(){
    var user = "u";
    var message = "m";
    connection.invoke("SendSensorData", user, message).catch(function (err) {
        return console.error(err.toString());
    });
}

setInterval(() => { 
    // console.log("Request sensor data");
    requestSensorData(); 
}, 2000);

connection.start().then(function () {
    // document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

connection.on("ReceiveSensorData", function (sensor, index) {
    document.getElementById("id" + index).textContent = `Sensor ID  ${sensor[0]}`;
    document.getElementById("smoke" + index).textContent = `연기:${sensor[1]}`;
    document.getElementById("temp" + index).textContent = `온도:${sensor[2]}`;
    document.getElementById("gas" + index).textContent = `가스:${sensor[3]}`;
    
    sensorArray[index].id = sensor[0];
    sensorArray[index].smoke = sensor[1];
    sensorArray[index].temp = sensor[2];
    sensorArray[index].gas = sensor[3];

    
    console.log("sensor data " + index + " : " + sensorArray[index].smoke + " " + sensorArray[index].temp + " " + sensorArray[index].gas);
    
    // var li = document.createElement("li");
    // document.getElementById("messagesList").appendChild(li);
    // document.getElementById("temp" + message).textContent = `온도:${user}`;// `${user} says ${message}`;

    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    // li.textContent = `${user} says ${message}`;
});

