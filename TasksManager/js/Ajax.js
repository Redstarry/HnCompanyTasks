var data = null;
//当前页
var currentPage = null;
//一页的数量
var itemsPerPage = null;
//总页数
var totalPages = 1;
//总数量
var totalItems = null;
var temp = "";
var Hours = new Array();
var minutes = new Array();
for (let index = 0; index < 24; index++) {
	Hours[index] = index;
}
for (let index = 0; index < 60; index++) {
	minutes[index] = index;
}
var app;

function timer() {
    window.setInterval(function() {
        var sj = Math.round(new Date() / 1000);
        var expsj = window.localStorage["expDate"];
        if(sj >= expsj)
        {
            $(window).attr("location", "../Login.html");
        }
    },1000);
}
timer();
// 请求全部数据
// (function() {
// 	$.ajax({
// 		async: false,
// 		type: "get",
// 		url: "http://192.168.1.47:5000/api/Tasks",
// 		contentType: "application/json;charset=utf-8",
// 		headers:{"Authorization":"Bearer " + window.localStorage["Token"]},
// 		success: function(response) {
// 			data = response["response"];
// 			currentPage = data["currentPage"];
// 			itemsPerPage = data["itemsPerPage"];
// 			totalPages = data["totalPages"];
// 			if(totalPages == 0) totalPages = 1;
// 			totalItems = data["totalItems"];
// 			// window.localStorage.setItem("data",response["response"]);
// 		}
		
// 	});
// })();

if(data != null)
{
	temp = data["items"];
}
function initDate1() {
	var DateTime = new Date();
	var Month = DateTime.getMonth() + 1;
	if(Month < 10) Month = "0" + Month;
	var Day = DateTime.getDate();
	if(Day < 10) Day = "0" + Day;
	return DateTime.getFullYear() + "-" + Month + "-" + Day +"T00:00:00";
}
// 用Vue更新数据
app = new Vue({
	el: "#app",
	data: {
		IsShow: false,
		showData: temp,
		Number1: parseInt(totalPages),
		HoursData: Hours,
		Minutes: minutes,
		MaskTitle:"",
		sendUrl:"",
		sendType:"",
		MaskTaskName: '',
		MaskTaskType: '',
		MaskIntervalD: 0,
		MaskIntervalH: 0,
		MaskIntervalM: 0,
		MaskIntervalS: 0,
		MaskBuinessType: '',
		MaskDes: '',
		Maskval:"",
		MaskIsDisable:false,
		SelectId:"",
		SelectTaskName:"",
		SelectTaskStatus:"2",
		SelectTaskType:"all",
		SelectCreateStartTime:initDate1(),
		SelectCreateEndTime:initDate1(),
		SelectExStartTime:initDate1(),
		SelectExEndTime:initDate1(),
		startDate:0
	},
	methods: {
		MaskCancel: function(params) {
			this.IsShow = !this.IsShow;
			this.MaskIsDisable = false;
			this.sendUrl = "";
			this.MaskTitle="";
			this.sendType="";
			this.MaskTaskName="";
			this.MaskTaskType="";
			this.MaskIntervalD=0;
			this.MaskIntervalH=0;
			this.MaskIntervalM=0;
			this.MaskIntervalS=0;
			this.MaskBuinessType="";
			this.MaskDes = "";
			this.Maskval = "";
		},
		AddBtn: function(params) {
			this.MaskTitle = "添加任务";
			this.sendType = "post";
			this.sendUrl = "http://192.168.1.47:5000/api/Tasks/task";
			
			this.IsShow = !this.IsShow;
			var CurrentDateLong = new Date();
			var month = CurrentDateLong.getMonth() + 1;
			if( month < 10) month = "0" + month;
			var CurDate = CurrentDateLong.getDate();
			if( CurDate < 10) CurDate = "0" + CurDate;
			var hoursTime = CurrentDateLong.getHours();
			if( hoursTime < 10) hoursTime = "0" + hoursTime;
			var min = CurrentDateLong.getMinutes();
			if( min < 10) min = "0" + min;
			var second = CurrentDateLong.getSeconds();
			if( second < 10) second = "0" + second;
			var CurrentDate = CurrentDateLong.getFullYear() + "-" + month  + "-" + CurDate + "T" + hoursTime + ":" + min + ":" + second;
			this.Maskval = CurrentDate;
		},
		SelectBtn: function() {
			
			var NowDate = new Date().getTime();
			if(NowDate - this.startDate > 2000)
			{
				$.ajax({
					async:false,
					type: "post",
					url: "http://192.168.1.47:5000/api/Tasks",
					headers:{"Authorization":"Bearer " + window.localStorage["Token"]},
					data: JSON.stringify({
						"Task_Name":this.SelectTaskName,
						"Task_TaskType":this.SelectTaskType,
						"Task_ExecuteReuslt":this.SelectTaskStatus,
						"CreatTimeStart":this.changeDate(this.SelectCreateStartTime), 
						"CreatTimeEnd":this.changeDate(this.SelectCreateEndTime),
						"TaskPresetTimeStart":this.changeDate(this.SelectExStartTime),
						"TaskPresetTimeEnd":this.changeDate(this.SelectExEndTime)
					}),
					contentType: "application/json;charset=utf-8",
					success: function (response) {
						data = response["response"];
						app.showData = data["items"];
						currentPage = data["currentPage"];
						itemsPerPage = data["itemsPerPage"];
						totalPages = data["totalPages"];
						totalItems = data["totalItems"];
					}
				});
				this.startDate = NowDate;
				console.log(this.startDate)
			}
			
		},
		sendAddAjax: function() {
			var NewExDate = this.Maskval.split("T").join(" ");
			console.log(NewExDate);
			var Interval = "";
			if(this.MaskIntervalD!="" || this.MaskIntervalH!="" || this.MaskIntervalM !="" || this.MaskIntervalS != "")
			{
				Interval = this.MaskIntervalD + ":" + this.MaskIntervalH + ":" + this.MaskIntervalM + ":" + this.MaskIntervalS;
			}
			console.log(Interval)
			$.ajax({
				async: false,
				type: app.sendType,
				url: app.sendUrl,
				contentType: "application/json;charset=utf-8",
				headers:{"Authorization":"Bearer " + window.localStorage["Token"]},
				data: JSON.stringify({
					"Task_Name": this.MaskTaskName,
					"Task_TaskType": this.MaskTaskType,
					"Task_BusinessType": this.MaskBuinessType,
					"Task_PresetTime": NewExDate,
					"Task_Interval": Interval,
					"Task_Describe": this.MaskDes
				}),
				success: function(response) {
					if(response["status"] == 0)
					{
						alert(response["message"])
						return;
					}
					app.MaskTaskName = '';
					app.MaskTaskType = '';
					app.MaskBuinessType = "";
					app.MaskDes = "";
					data = response["response"];
					currentPage = data["currentPage"];
					itemsPerPage = data["itemsPerPage"];
					totalPages = data["totalPages"];
					totalItems = data["totalItems"];
					app.showData = data["items"];
					app.Number1 = parseInt(data["totalPages"]);
					app.IsShow = !app.IsShow;
				}
			});
		},
		changeDate:function (oldDate) {
			return oldDate.split("T").join(" ");
		}
	},
	watch: {
		"showData": function(NewData) {
			this.showData = NewData;

		}
	}
});
$(".preLi").next().addClass("active");
// 点击下一页事件
$(".next").click(function(e) {

	var activeIndex = $(".active").index();
	if (activeIndex == totalPages - 1) {
		$(".next").parent().addClass("disabled");
	}
	if (currentPage != totalPages) {
		activeIndex += 1;
		var url = "http://192.168.1.47:5000/api/Tasks?pageNumber=" + activeIndex + "&PageSize=" + itemsPerPage;
		GetAjax(url);
		app.showData = data["items"];
		$(".page li").eq(currentPage).addClass("active");
		$(".page li").eq(currentPage - 1).removeClass("active");
		$(".prev").parent().removeClass("disabled");
	}
});
// 点击上一页事件
$(".prev").click(function(e) {
	var index = $(".page .active").index();
	if (index == 2) {
		$(".prev").parent().addClass("disabled");
	}
	if (index > 1) {
		$(".page li").eq(index - 1).addClass("active");
		$(".page li").eq(index).removeClass("active");
		$(".next").parent().removeClass("disabled");
		var url = "http://192.168.1.47:5000/api/Tasks?pageNumber=" + (index - 1) + "&PageSize=" + itemsPerPage;
		GetAjax(url);
		app.showData = data["items"];

	}
});
// 页码点击事件
$(".page .pageNum").click(function(e) {
	var index = $(".page .active").index();
	var indeclick = $(this).index();
	if (index == indeclick) {
		return;
	}
	if (indeclick != 1) {
		$(".prev").parent().removeClass("disabled");
	} else {
		$(".prev").parent().addClass("disabled");
	}
	if (indeclick == totalPages) {
		$(".next").parent().addClass("disabled");
	} else {
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
	var flag = confirm("确定要删除这条数据吗？");
	if(flag)
	{
		var id = $(this).parent().parent().find(".id").html()
		var index = $(".active").index();
		DeleteAjax(id);
		var url = "http://192.168.1.47:5000/api/Tasks?pageNumber=" + index + "&PageSize=" + itemsPerPage;
		GetAjax(url);
		app.showData = data["items"];
		app.Number1 = parseInt(data["totalPages"]);
	}
	

}
// 更新数据的事件
function updateData(params) {
	var id = $(this).parent().parent().find(".id").html();
	var Interval = $(this).parent().parent().find(".Interval").html();
	var IntervalSplit = Interval.split(":");
	var PreseTime = $(this).parent().parent().find(".PreseTime").html();
	var ResultPreseTime = PreseTime.split(" ").join("T");
	console.log(ResultPreseTime)
	app.IsShow = true;
	app.MaskTitle = "更新任务";
	app.sendUrl = "http://192.168.1.47:5000/api/Tasks/" + id;
	app.sendType = "put";
	app.MaskTaskName = $(this).parent().parent().find(".name").html();
	// app.MaskTaskType = $(this).parent().parent().find(".taskType").html();
	app.MaskIntervalD = IntervalSplit[0];
	app.MaskIntervalH = IntervalSplit[1];
	app.MaskIntervalM = IntervalSplit[2];
	app.MaskIntervalS = IntervalSplit[3];
	app.MaskBuinessType = $(this).parent().parent().find(".BusinessType").html();
	app.MaskDes = $(this).parent().parent().find(".Describe").html();
	app.Maskval = ResultPreseTime;
	app.MaskIsDisable = true;
	console.log(app.sendUrl);
}
// get请求Ajax
function GetAjax(url) {
	$.ajax({
		async: false,
		type: "get",
		url: url,
		contentType: "application/json;charset=utf-8",
		headers:{"Authorization":"Bearer " + window.localStorage["Token"]},
		success: function(response) {
			data = response["response"];
			currentPage = data["currentPage"];
			itemsPerPage = data["itemsPerPage"];
			totalPages = data["totalPages"];
			totalItems = data["totalItems"];
		}
	});
}
// Delete 请求Ajax
function DeleteAjax(id) {
	$.ajax({
		async: false,
		type: "delete",
		url: "http://192.168.1.47:5000/api/Tasks/" + id,
		contentType: "application/json;charset=utf-8",
		headers:{"Authorization":"Bearer " + window.localStorage["Token"]},
		success: function(response) {
			// data = response["response"];
			// currentPage = data["currentPage"];
			// itemsPerPage = data["itemsPerPage"];
			// totalPages = data["totalPages"];
			// totalItems = data["totalItems"];
		}
	});
}
