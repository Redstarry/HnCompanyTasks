var data = null;
//当前页
var currentPage = null;
//一页的数量
var itemsPerPage = null;
//总页数
var totalPages = null;
//总数量
var totalItems = null;
var Hours = new Array();
var minutes = new Array();
for (let index = 0; index < 24; index++) {
    Hours[index] = index;
}
for (let index = 0; index < 60; index++) {
    minutes[index] = index;
}
// 请求全部数据
(function(){
    $.ajax({
        async:false,
        type: "get",
        url: "http://192.168.1.47:5000/api/Tasks",
        contextType: "application/json;charset=utf-8",
        success: function (response) {
            data = response["response"];
            currentPage = data["currentPage"];
            itemsPerPage = data["itemsPerPage"];
            totalPages = data["totalPages"];
            totalItems = data["totalItems"];
        }
    });
})();

// 用Vue更新数据
var app = new Vue({
    el:"#app",
    data:{
        showData:data["items"],
        Number1:parseInt(totalPages),
        HoursData: Hours,
        Minutes:minutes,
        IsShow:false,
        CurDate : null
    },
    methods:{
        MaskCancel:function(params) {
            this.IsShow = !this.IsShow;
        },
        AddBtn:function(params) {
            this.IsShow = !this.IsShow;
            var CurrentDate = new Date();
            if(this.IsShow == true)
            {
                this.CurDate = CurrentDate.getFullYear() + "-" + CurrentDate.getMonth() + "-" + CurrentDate.getDay() + "T" + CurrentDate.getHours() + ":" + CurrentDate.getMinutes() + ":" + CurrentDate.getSeconds();
            }
        },
        GetDate:function(params) {
            var CurrentDate = new Date();
            if(this.IsShow == true)
            {
                this.CurDate = CurrentDate.getDate();
            }
        }
    },
    watch:{
        "showData":function(NewData) {
            this.showData = NewData;
            
        }
    }
});
$(".preLi").next().addClass("active");
// 点击下一页事件
$(".next").click(function (e) { 

    var activeIndex = $(".active").index();
    if(activeIndex == totalPages - 1)
    {
        $(".next").parent().addClass("disabled");
    }
    if(currentPage != totalPages)
    {
        activeIndex += 1;
        var url = "http://192.168.1.47:5000/api/Tasks?pageNumber=" + activeIndex + "&PageSize=" + itemsPerPage;
        GetAjax(url);
        app.showData = data["items"];
        $(".page li").eq(currentPage).addClass("active");
        $(".page li").eq(currentPage-1).removeClass("active");
        $(".prev").parent().removeClass("disabled");
    }
});
// 点击上一页事件
$(".prev").click(function (e) { 
    var index = $(".page .active").index();
    if(index == 2)
    {
        $(".prev").parent().addClass("disabled");
    }
    if(index > 1)
    {
        $(".page li").eq(index-1).addClass("active");
        $(".page li").eq(index).removeClass("active");
        $(".next").parent().removeClass("disabled");
        var url = "http://192.168.1.47:5000/api/Tasks?pageNumber=" + (index-1) + "&PageSize=" + itemsPerPage;
        GetAjax(url);
        app.showData = data["items"];
        
    }
});
// 页码点击事件
$(".page .pageNum").click(function (e) { 
    var index = $(".page .active").index();
    var indeclick = $(this).index();
    if(index == indeclick)
    {
        return;
    }
    if(indeclick != 1)
    {
        $(".prev").parent().removeClass("disabled");
    }
    else
    {
        $(".prev").parent().addClass("disabled");
    }
    if(indeclick == totalPages)
    {
        $(".next").parent().addClass("disabled");
    }
    else
    {
        $(".next").parent().removeClass("disabled");
    }
    $(this).addClass("active");

    $(".page li").eq(index).removeClass("active");
    var url = "http://192.168.1.47:5000/api/Tasks?pageNumber=" + indeclick + "&PageSize=" + itemsPerPage;
    GetAjax(url);
    app.showData = data["items"];
    
});

// 删除数据的事件
function removeData(params) {
    var id = $(this).parent().parent().find(".id").html()
    var index = $(".active").index();
    DeleteAjax(id);
    var url = "http://192.168.1.47:5000/api/Tasks?pageNumber=" + index + "&PageSize=" + itemsPerPage;
    GetAjax(url);
    app.showData = data["items"];

}
// get请求Ajax
function GetAjax(url) {
    $.ajax({
        async:false,
        type: "get",
        url: url,
        contentType: "application/json;charset=utf-8",
        success: function (response) {
            data = response["response"];
            currentPage = data["currentPage"];
            itemsPerPage = data["itemsPerPage"];
            totalPages = data["totalPages"];
            totalItems = data["totalItems"];
            console.log(data);
        }
    });
}
// Delete 请求Ajax
function DeleteAjax(id) {
    $.ajax({
        async:false,
        type: "delete",
        url: "http://192.168.1.47:5000/api/Tasks/" + id,
        contentType: "application/json;charset=utf-8",
        success: function (response) {
            // data = response["response"];
            // currentPage = data["currentPage"];
            // itemsPerPage = data["itemsPerPage"];
            // totalPages = data["totalPages"];
            // totalItems = data["totalItems"];
        }
    });
}