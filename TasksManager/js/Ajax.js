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
			totalItems = data["totalItems"];
		}
	});
})();

// 用Vue更新数据
var app = new Vue({
	el: "#app",
	data: {
		showData: data["items"],
		Number1: parseInt(totalPages),
		HoursData: Hours,
		Minutes: minutes,
		IsShow: false,
		TaskName: '',
		TaskType: '',
		IntervalD: '',
		IntervalH: '',
		IntervalM: '',
		IntervalS: '',
		BuinessType: '',
		Des: ''
	},
	methods: {
		MaskCancel: function(params) {
			this.IsShow = !this.IsShow;
		},
		AddBtn: function(params) {
			this.IsShow = !this.IsShow;
		},
		sendAddAjax: function() {
			var ExDate = $(".ExDate").val().split("-","T").join(" ");
			var NewExDate = $(".ExDate").val().split("T");
			// 拼接日期
			var year = NewExDate[0].split("-");
			var joinyear = year[0] + "年" + year[1] + "月" + year[2] + "日";
			// 拼接时间
			var CurrentSeconds = new Date().getSeconds();
			if(CurrentSeconds < 10)
			{
				CurrentSeconds = "0" + CurrentSeconds;
			}
			var SplitTime = NewExDate[1] + ":" + CurrentSeconds;
			//拼接日期和时间
			var NewDateArry = new Array()
			NewDateArry[0] = joinyear;
			NewDateArry[1] = SplitTime;
			var NewDate = NewDateArry.join(" ");
			
			var Interval = this.IntervalD + ":" + this.IntervalH + ":" + this.IntervalM + ":" + this.IntervalS;

			$.ajax({
				async: false,
				type: "post",
				url: "http://192.168.1.47:5000/api/Tasks/task",
				contentType: "application/json;charset=utf-8",
				data: JSON.stringify({
					"Task_Name": this.TaskName,
					"Task_TaskType": this.TaskType,
					"Task_BusinessType": this.BuinessType,
					"Task_PresetTime": NewDate,
					"Task_Interval": Interval,
					"Task_Describe": this.Des
				}),
				success: function(response) {
					app.TaskName = '';
					app.TaskType = '';
					app.BuinessType = "";
					app.Des = "";
					data = response["response"];
					currentPage = data["currentPage"];
					itemsPerPage = data["itemsPerPage"];
					totalPages = data["totalPages"];
					totalItems = data["totalItems"];
					app.showData = data["items"];
					app.Number1 = parseInt(data["totalPages"]);
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
			console.log(data);
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
