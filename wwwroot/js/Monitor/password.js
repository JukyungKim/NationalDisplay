
var connection = new signalR.HubConnectionBuilder().withUrl("/accountHub").build();


function test2()
{
    alert(document.getElementById("pw1").value + "버튼을 클릭하셨습니다.");   
}

setInterval(() => {
    logout();
}, 1000 * 60 * 10);

function logout()
{
    window.location.href = "home/login";
}

function checkPassword()
{
    changePassword();



    // var pw = $("#password").val();

    // pw = document.getElementById("pw1").value;
    // var num = pw.search(/[0-9]/g);
    // var eng = pw.search(/[a-z]/ig);
    // var spe = pw.search(/[`~!@@#$%^&*|₩₩₩'₩";:₩/?]/gi);
    
   
    // if(pw.length < 10 || pw.length > 20){
    //  alert("10자리 ~ 20자리 이내로 입력해주세요.");
    //  return false;
    // }else if(pw.search(/\s/) != -1){
    //  alert("비밀번호는 공백 없이 입력해주세요.");
    //  return false;
    // }else if( (num < 0 && eng < 0) || (eng < 0 && spe < 0) || (spe < 0 && num < 0) ){
    //  alert("영문,숫자, 특수문자 중 2가지 이상을 혼합하여 입력해주세요.");
    //  return false;
    // }else {
    //    console.log("통과");	 
    // }
}

function changePassword()
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
    pw = document.getElementById("pw1").value;
    pw2 = document.getElementById("pw2").value;
    var reg = /^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{10,}$/;

    if(result === 0){
        alert("비밀 번호가 틀렸습니다.");   
    }
    else if(false === reg.test(pw)) {
        alert('비밀번호는 10자 이상이어야 하며, 숫자/대문자/소문자/특수문자를 모두 포함해야 합니다.');
        }
    else if(pw !== pw2){
        alert('비밀번호가 같지 않습니다.');
    }
    else {
        console.log("통과");
    }
});



