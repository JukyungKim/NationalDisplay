
var connection = new signalR.HubConnectionBuilder().withUrl("/accountHub").build();

history.pushState(null, null, location.href); 
window.onpopstate = function(event) { 
	history.go(1); 
};

function login()
{
    pw = document.getElementById("pw").value;
    connection.invoke("LoginResult", pw).catch(function (err){
        return console.error(err.toString());
    });
}

connection.start().then(function(){

}).catch(function(err){
    return console.error(err.toString());
})

connection.on("LoginError", function(result){
    // pw = document.getElementById("pw1").value;
    console.log("Login error hub");
    if(result === 0){
        alert("계정 정보가 정확하지 않습니다.");   
    }
});
