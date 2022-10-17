
var loginIdKey = 'login_id';
var loginId;

window.onload = function(){
    loginId = getCookie(loginIdKey);
    console.log('loginId : ' + loginId);
}

var getCookie = function(name){
    var value = document.cookie.match('(^|;) ?' + name + '=([^;]*)(;|$)');
    return value? value[2] : null;
  }


setInterval(() => {
    logout();
}, 1000 * 60 * 10);

function logout()
{
    window.location.href = "/home/login";
}