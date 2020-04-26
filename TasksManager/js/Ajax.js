var data = null;
//当前页
var currentPage = null;
//一页的数量
var itemsPerPage = null;
//总页数
var totalPages = 2;
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
var app;
// var GetData = function(response) {
// 	data = response["response"];
// 	currentPage = data["currentPage"];
// 	itemsPerPage = data["itemsPerPage"];
// 	totalPages = data["totalPages"];
// 	if(totalPages == 0) totalPages = 1;
// 	totalItems = data["totalItems"];
// };
// 请求全部数据
(function() {
	$.ajax({
		async: false,
		type: "get",
		url: "http://192.168.1.47:5000/api/Tasks",
		contentType: "application/json;charset=utf-8",
		success: function(response) {
			data = response["response"];
			currentPage = data["currentPage"];
			itemsPerPage = data["itemsPerPage"];
			totalPages = data["totalPages"];
			if(totalPages == 0) totalPages = 1;
			totalItems = data["totalItems"];
			// window.localStorage.setItem("data",response["response"]);
		}
		
	});
})();
console.log(window.localStorage.getItem("data"))
// GetData();

// 用Vue更新数据
app = new Vue({
	el: "#app",
	data: {
		showData: data["items"],
		Number1: parseInt(totalPages),
		HoursData: Hours,
		Minutes: minutes,
		IsShow: false,
		MaskTitle:"",
		sendUrl:"",
		sendType:"",
		MaskTaskName: '',
		MaskTaskType: '',
		MaskIntervalD: '',
		MaskIntervalH: '',
		MaskIntervalM: '',
		MaskIntervalS: '',
		MaskBuinessType: '',
		MaskDes: '',
		Maskval:"",
		MaskIsDisable:false,
		SelectId:"",
		SelectTaskName:"",
		SelectTaskStatus:"",
		SelectTaskType:"",
		SelectCreateStartTime:"",
		SelectCreateEndTime:"",
		SelectExStartTime:"",
		SelectExEndTime:""
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
			this.MaskIntervalD="";
			this.MaskIntervalH="";
			this.MaskIntervalM="";
			this.MaskIntervalS="";
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
		SelectBtn: function(params) {
			$.ajax({
				async:false,
				type: "post",
				url: "http://192.168.1.47:5000/api/Tasks",
				data: JSON.stringify({
					"Task_Name":this.SelectTaskName,
					"Task_TaskType":this.SelectTaskType,
					"Task_ExecuteReuslt":this.SelectTaskStatus,
					"CreatTimeStart":this.SelectCreateStartTime,
					"CreatTimeEnd":this.SelectCreateEndTime,
					"TaskPresetTimeStart":this.SelectExStartTime,
					"TaskPresetTimeEnd":this.SelectExEndTime
				}),
				contentType: "application/json;charset=utf-8",
				success: function (response) {
					data = response["response"];
					currentPage = data["currentPage"];
					itemsPerPage = data["itemsPerPage"];
					totalPages = data["totalPages"];
					totalItems = data["totalItems"];
				}
			});
		},
		sendAddAjax: function() {
			var NewExDate = this.Maskval.split("T").join(" ");
			console.log(NewExDate);
			var Interval = this.MaskIntervalD + ":" + this.MaskIntervalH + ":" + this.MaskIntervalM + ":" + this.MaskIntervalS;
			console.log(this.MaskTaskName)
			$.ajax({
				async: false,
				type: app.sendType,
				url: app.sendUrl,
				contentType: "application/json;charset=utf-8",
				data: JSON.stringify({
					"Task_Name": this.MaskTaskName,
					"Task_TaskType": this.MaskTaskType,
					"Task_BusinessType": this.MaskBuinessType,
					"Task_PresetTime": NewExDate,
					"Task_Interval": Interval,
					"Task_Describe": this.MaskDes
				}),
				success: function(response) {
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
					app.IsShow = false;
				}
			});
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
	var id = $(this).parent().parent().find(".id").html()
	var index = $(".active").index();
	DeleteAjax(id);
	var url = "http://192.168.1.47:5000/api/Tasks?pageNumber=" + index + "&PageSize=" + itemsPerPage;
	GetAjax(url);
	app.showData = data["items"];
	app.Number1 = parseInt(data["totalPages"]);

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
		success: function(response) {
			// data = response["response"];
			// currentPage = data["currentPage"];
			// itemsPerPage = data["itemsPerPage"];
			// totalPages = data["totalPages"];
			// totalItems = data["totalItems"];
		}
	});
}
