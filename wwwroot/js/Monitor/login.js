
var connection = new signalR.HubConnectionBuilder().withUrl("/accountHub").build();

history.pushState(null, null, location.href); 
window.onpopstate = function(event) { 
	history.go(1); 
};

var loginIdKey = 'login_id';
var loginId;

function setCookie(key, value, exp){
    var date = new Date();
    date.setTime(date.getTime() + exp * 24 * 60 * 60 * 1000);
    document.cookie = key + '=' + value + ';expires=' + date.toUTCString() + ';path=/';
}
var deleteCookie = function(key){
    document.cookie = key + '=; expires=Thu, 01 Jan 1999 00:00:10 GMT;';
}

function login()
{
    loginId = document.getElementById("id").value;
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
    else{
        console.log("LoginIdKey:" + loginIdKey + " LoginId:" + loginId);
        // deleteCookie(loginIdKey);
        setCookie(loginIdKey, loginId, 100);
    }
});
