
// var connection = new signalR.HubConnectionBuilder().withUrl("/accountHub").build();
var connection = new signalR.HubConnectionBuilder().withUrl("/accountHub").build();

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

function createSubAccount()
{
    id = document.getElementById("sub_account_id").value;
    pw = document.getElementById("sub_account_password").value;

    if(!checkId(id)){
        return;
    }

    var passReg = /^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{10,}$/;
    if(false === passReg.test(pw)) {
        alert('비밀번호는 10자 이상이어야 하며, 숫자/대문자/소문자/특수문자를 모두 포함해야 합니다.');
        return;
    }

    connection.invoke("CreateSubAccount", id, pw).catch(function (err){
        return console.error(err.toString());
    });
}

connection.start().then(function(){
}).catch(function(err){
    return console.error(err.toString());
})

connection.on("SubAccountError", function(result){
    id = document.getElementById("sub_account_id").value;
    pw = document.getElementById("sub_account_password").value;
    // var reg = /^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{10,}$/;

    // if(result === 0){
    //     alert("비밀 번호가 틀렸습니다.");   
    // }
    // else if(false === reg.test(pw)) {
    //     alert('비밀번호는 10자 이상이어야 하며, 숫자/대문자/소문자/특수문자를 모두 포함해야 합니다.');
    //     }
    // else if(pw !== pw2){
    //     alert('비밀번호가 같지 않습니다.');
    // }
    // else {
    //     console.log("통과");
    // }
    if(result === true){
        alert("ID가 존재합니다.");
    }
    setInterval(() => {
        window.location.reload();
    }, 1000);
});

function checkId(id){
    if(!existData(id))
        return false;

    var idRegExp = /^[a-zA-z0-9]{4,12}$/; //아이디 유효성 검사
    
    if(!idRegExp.test(id)){
        alert("ID는 영문,한글,숫자 4~12자리로 입력해야 합니다.");
        return false;
    }
    
    return true;
}

function existData(value){
    if(value == ""){
        alert("ID를 입력해주세요");
        return false;
    }
    return true;
}



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













