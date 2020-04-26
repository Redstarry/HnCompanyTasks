
$(".login").click(function (e) { 
    var user = $("#user").val();
    var pwd = $("#pwd").val();
    if(user == "" && pwd == "")
    {
        alert("账号或密码错误");
        return;
    }
    var data = login(user,pwd);

});

function login(user, pwd) {
    var data = $.ajax({
        type: "Post",
        url: "http://192.168.1.47:5000/api/Tasks/login",
        data: JSON.stringify({
            "UserName":user,
            "Pwd":pwd
        }),
        contentType:"application/json;charset=utf-8",
        success: function (response) 
        {
            localStorage.setItem("Token" , response["response"]["jwttoken"]);
            localStorage.setItem("expDate",response["response"]["overdue"]);
            console.log(response)
            if(response["status"] == 1)
            {
                $(window).attr("location", "./html/index.html");
            }
            else
            {
                alert("账号或密码错误");
            }
        }
    });
}
