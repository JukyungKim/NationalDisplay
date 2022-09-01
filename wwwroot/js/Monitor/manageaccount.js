
// var connection = new signalR.HubConnectionBuilder().withUrl("/accountHub").build();

window.onpopstate = function(event){
     if(event){
         console.log("뒤로가기");
         var link = "https://localhost:5001/home/main";
         location.href = link;
         location.replace(link);
         window.open(link);
     }
 }

setInterval(() => {
    logout();
}, 1000 * 60 * 10);

function checkSensorId()
{
    // console.log("센서 id 추가 " + document.getElementById("sensor_id1").value);
    // sensor_id = document.getElementById("sensor_id1").value;
    // connection.invoke("CheckSensorId", sensor_id).catch(function (err){
    //     return console.error(err.toString());
    // });
}

// connection.start().then(function(){

// }).catch(function(err){
//     return console.error(err.toString());
// })

// connection.on("SensorId", function(result){
//     // pw = document.getElementById("pw1").value;
//     if(result === true){
//         alert("센서 ID가 중복됩니다.");
//     }
// });


function logout()
{
    window.location.href = "home/login";
}

function show() {
    document.querySelector(".background").className = "background btn_add_sensor";
}

function close() {
    document.querySelector(".background").className = "background";
}
function hello() {
    alert('Hello world3');
}

function open() {
    document.querySelector(".modal").classList.remove("hidden");
}

function close2(){
    document.querySelector(".modal").classList.add("hidden");
}

window.onload = function () {
    // alert('1');
    // document.getElementById('hw2').addEventListener('click', hello);

    document.querySelector(".btn_add_sensor").addEventListener('click', show);
    document.querySelector(".close").addEventListener('click', close);
    document.querySelector(".cancel").addEventListener('click', close);

    // document.querySelector(".openBtn").addEventListener("click", open);
    // document.querySelector(".closeBtn").addEventListener("click", close2);
    // document.querySelector(".bg").addEventListener("click", close2);
}













