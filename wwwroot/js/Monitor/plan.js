// "use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/sensorHub").build();
var sensorItems = document.getElementsByClassName("myButton");
var sensorIndex = 0;
var sensorArray = new Array(20);
var selectedSensorIndex = 0;

window.onload = function(){
    init();
}
setInterval(() => {
    logout();
}, 1000 * 60 * 10);

function logout()
{
    window.location.href = "home/login";
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

    selectedSensorIndex = sensorIndex;

    if(sensorArray[selectedSensorIndex].id != null) {    
        document.getElementById("smoke").setAttribute('stroke-dasharray', sensorArray[selectedSensorIndex].smoke + "," + "100");
        document.getElementById("temp").setAttribute('stroke-dasharray', sensorArray[selectedSensorIndex].temp + "," + "100");
        document.getElementById("gas").setAttribute('stroke-dasharray', sensorArray[selectedSensorIndex].gas + "," + "100");

        document.getElementById("smoke_value").textContent = sensorArray[selectedSensorIndex].smoke + "%";
        document.getElementById("temp_value").textContent = sensorArray[selectedSensorIndex].temp + "ºC";
        document.getElementById("gas_value").textContent = sensorArray[selectedSensorIndex].gas + "%";
    }
    else{
        document.getElementById("smoke").setAttribute('stroke-dasharray', sensorArray[selectedSensorIndex].smoke + "," + "100");
        document.getElementById("temp").setAttribute('stroke-dasharray', sensorArray[selectedSensorIndex].temp + "," + "100");
        document.getElementById("gas").setAttribute('stroke-dasharray', sensorArray[selectedSensorIndex].gas + "," + "100");

        document.getElementById("smoke_value").textContent = 0 + "%";
        document.getElementById("temp_value").textContent = 0 + "ºC";
        document.getElementById("gas_value").textContent = 0 + "%";
    }
    // document.getElementById("smoke").setAttribute('stroke-dasharray', sensorArray[sensorIndex].smoke + "," + "100");
    // document.getElementById("temp").setAttribute('stroke-dashasrray', sensorArray[sensorIndex].temp + "," + "100");
    // document.getElementById("gas").setAttribute('stroke-dasharray', sensorArray[sensorIndex].gas + "," + "100");

    // document.getElementById("smoke_value").textContent = sensorArray[sensorIndex].smoke + "%";
    // document.getElementById("temp_value").textContent = sensorArray[sensorIndex].temp + "ºC";
    // document.getElementById("gas_value").textContent = sensorArray[sensorIndex].gas + "%";
}

function requestSensorData(){
    var user = "u";
    var message = "m";
    connection.invoke("SendSensorData", user, message).catch(function (err) {
        return console.error(err.toString());
    });
}

function saveSensorPosition(id, x, y){
    connection.invoke("SaveSensorPosition", id, x, y).catch(function (err){
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
    document.getElementById("id" + index).parentElement.style.cssText = `
    box-shadow: 3px 4px 0px 0px #035fb4;
	background:linear-gradient(to bottom, #3c7cbd 5%, #0966c4 100%);
	background-color:#2782dd;
	border:1px solid #3586d8;
	text-shadow:0px 1px 0px #528ecc;
    `;

    document.getElementById("id" + index).textContent = `Sensor ID  ${sensor[0]}`;
    document.getElementById("smoke" + index).textContent = `연기:${sensor[1]}`;
    document.getElementById("temp" + index).textContent = `온도:${sensor[2]}`;
    document.getElementById("gas" + index).textContent = `가스:${sensor[3]}`;
    
    sensorArray[index].id = sensor[0];
    sensorArray[index].smoke = sensor[1];
    sensorArray[index].temp = sensor[2];
    sensorArray[index].gas = sensor[3];
    
    console.log("sensor data " + index + " : " + sensorArray[index].smoke + " " + sensorArray[index].temp + " " + sensorArray[index].gas);
    
    var id = `${sensor[0]}`;
    if(document.getElementById(id) == null){
        var tag = document.getElementById("region_plan_id");
        var newTag = document.createElement('div');
        newTag.setAttribute('id', id);
        newTag.setAttribute('class', 'draggable');
        newTag.innerHTML = `Sensor ID : ${sensor[0]}`;
        newTag.style.cssText = `
            position: absolute;
            left: ${sensor[4]}px;
            top: ${sensor[5]}px;
            `;
        newTag.addEventListener('mouseup', (event) => {
            saveSensorPosition(Number(sensor[0]), Number(newTag.getBoundingClientRect().left), Number(newTag.getBoundingClientRect().top));
            console.log("mouse up event:" + event.clientX + " " + event.clientY);
        })
        tag.appendChild(newTag);

        dragobject.initialize();
        $(function(){
            $(".draggable").draggable();
        });
        // dragobject.move(300, 300);
    }

    console.log("sensor array length : " + sensorArray.length);
    if(sensorArray[selectedSensorIndex].id != null) {    
        document.getElementById("smoke").setAttribute('stroke-dasharray', sensorArray[selectedSensorIndex].smoke + "," + "100");
        document.getElementById("temp").setAttribute('stroke-dasharray', sensorArray[selectedSensorIndex].temp + "," + "100");
        document.getElementById("gas").setAttribute('stroke-dasharray', sensorArray[selectedSensorIndex].gas + "," + "100");

        document.getElementById("smoke_value").textContent = sensorArray[selectedSensorIndex].smoke + "%";
        document.getElementById("temp_value").textContent = sensorArray[selectedSensorIndex].temp + "ºC";
        document.getElementById("gas_value").textContent = sensorArray[selectedSensorIndex].gas + "%";
    }
    else{
        document.getElementById("smoke").setAttribute('stroke-dasharray', sensorArray[selectedSensorIndex].smoke + "," + "100");
        document.getElementById("temp").setAttribute('stroke-dasharray', sensorArray[selectedSensorIndex].temp + "," + "100");
        document.getElementById("gas").setAttribute('stroke-dasharray', sensorArray[selectedSensorIndex].gas + "," + "100");

        document.getElementById("smoke_value").textContent = 0 + "%";
        document.getElementById("temp_value").textContent = 0 + "ºC";
        document.getElementById("gas_value").textContent = 0 + "%";
    }

    // var li = document.createElement("li");
    // document.getElementById("messagesList").appendChild(li);
    // document.getElementById("temp" + message).textContent = `온도:${user}`;// `${user} says ${message}`;

    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    // li.textContent = `${user} says ${message}`;
});


var dragobject = {
    z: 0, x: 0, y: 0, offsetx: null, offsety: null, targetobj: null, dragapproved: 0,
    initialize: function () {
        document.onmousedown = this.drag
        document.onmouseup = function () { this.dragapproved = 0 }
    },
    drag: function (e) {
        var evtobj = window.event ? window.event : e
        this.targetobj = window.event ? event.srcElement : e.target
        if (this.targetobj.className == "drag") {
            this.dragapproved = 1
            if (isNaN(parseInt(this.targetobj.style.left))) { this.targetobj.style.left = 0 }
            if (isNaN(parseInt(this.targetobj.style.top))) { this.targetobj.style.top = 0 }
            this.offsetx = parseInt(this.targetobj.style.left)
            this.offsety = parseInt(this.targetobj.style.top)
            this.x = evtobj.clientX
            this.y = evtobj.clientY
            if (evtobj.preventDefault)
                evtobj.preventDefault()
            document.onmousemove = dragobject.moveit
        }
    },
    moveit: function (e) {
        var evtobj = window.event ? window.event : e
        if (this.dragapproved == 1) {
            this.targetobj.style.left = this.offsetx + evtobj.clientX - this.x + "px"
            this.targetobj.style.top = this.offsety + evtobj.clientY - this.y + "px"
            
            return false
        }   
    },
    move: function (x, y) {
        this.targetobj.style.left = x + "px"
        this.targetobj.style.top = y + "px"
    }
}

dragobject.initialize()

$(function(){
    $(".draggable").draggable();
});
