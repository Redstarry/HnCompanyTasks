
(function initPage(){
	$(".preLi").next().addClass("active");
	$(".next").parent().addClass("disabled");
	
	
})();
// 时间间隔的选项初始化
function Loops(Number){
	const Loop = Array();
	for(i = 0; i<Number;i++)
	{
		Loop[i] = i;
	}
	return Loop;
}
function GetCurrentDate(){
	const year = new Date().getFullYear();
	const month = addZone(new Date().getMonth() + 1);
	const Day = addZone(new Date().getDate());
	const Hour = addZone(new Date().getHours());
	const Min = addZone(new Date().getMinutes());
	const Sconds = addZone(new Date().getSeconds());
	return year + "-" + month + "-" + Day + "T" + Hour + ":" + Min + ":" + Sconds;
}
function addZone(DateNumber){
	var Interval = DateNumber
	if(DateNumber < 10)
	{
		Interval = "0" + DateNumber;
	}
	return Interval;
}
var app = new Vue({
	el:"#app",
	data:{
		showData:"", //查询的数据
		// CurrentPage:当前页;TotalPage:全部的页数;TotalItems:数据的总数量;ItemsPerPage:一页的数量
		PageInfo:{"CurrentPage":1,"TotalPage":1,"TotalItems":0,"ItemsPerPage":5},
		IsShow: false, //是否显示 弹窗
		HoursData: Loops(24), //时间间隔的天数 双向绑定
		Minutes: Loops(60), //时间间隔的分和秒 双向绑定
		DataMask:[
			{"url":"http://192.168.1.47:5000/api/Tasks/task", "Type":"post"}, //添加任务
			{"url":"http://192.168.1.47:5000/api/Tasks/", "Type":"put"}, //更新任务
			{"url":"http://192.168.1.47:5000/api/Tasks/","Type":"get"} //获取全部数据
		],
		title:"",
		MaskTaskName: '', //双向绑定
		MaskTaskType: '', //双向绑定
		MaskIntervalD: 0, //双向绑定
		MaskIntervalH: 0, //双向绑定
		MaskIntervalM: 0, //双向绑定
		MaskIntervalS: 0, //双向绑定
		MaskBuinessType: '',
		MaskDes: '',
		Maskval:"",
		MaskIsDisable:false,
		SelectId:"",
		SelectTaskName:"",
		SelectTaskStatus:"2",
		SelectTaskType:"all",
		SelectCreateStartTime:GetCurrentDate(),
		SelectCreateEndTime:GetCurrentDate(),
		SelectExStartTime:GetCurrentDate(),
		SelectExEndTime:GetCurrentDate(),
		startDate:0,
		OpenOrClose:false
	},
	methods:{
		// 封装的Ajax请求；url:请求地址; sendType:请求的类型; sendData:请求的数据.
		sendAjax1:function(url, sendType, sendData){
			
			var a = new Promise((resolve, reject)=>{
				console.log(sendType)
				$.ajax({
					url:url,
					type:sendType,
					contentType:"application/json;charset=utf-8",
					data:sendData,
					headers:{"Authorization":"Bearer " + window.localStorage["Token"]},
					success:function(response)
					{
						resolve(response);
					},
					error:function(xhr)
					{
						reject(xhr);
					}
				});
			});
			a.then(
				value=>{
					if(value["status"] == 1)
					{
						var responseData = value["response"];
						this.showData = responseData["items"];
						this.PageInfo["CurrentPage"] = responseData["currentPage"];
						this.PageInfo["TotalPage"] = responseData["totalPages"];
						this.PageInfo["TotalItems"] = responseData["totalItems"];
						this.PageInfo["ItemsPerPage"] = responseData["itemsPerPage"];
					}
					else
					{
						alert(value["message"]);
					}
				},
				reason=>{
					alert(reason.statusText);
				}
			);
			
		},
		// 查询按钮
		SelectBtn:function(){
			const RequestData = JSON.stringify({
				"Task_Name":this.SelectTaskName,
				"Task_TaskType":this.SelectTaskType,
				"Task_ExecuteReuslt":this.SelectTaskStatus,
				"CreatTimeStart":this.CalendarDateAnalysis(this.SelectCreateStartTime), 
				"CreatTimeEnd":this.CalendarDateAnalysis(this.SelectCreateEndTime),
				"TaskPresetTimeStart":this.CalendarDateAnalysis(this.SelectExStartTime),
				"TaskPresetTimeEnd":this.CalendarDateAnalysis(this.SelectExEndTime)
			});
			var NowDate = new Date().getTime();
			if(NowDate - this.startDate > 2000)
			{
				this.sendAjax1(this.DataMask[2]["url"], "post",RequestData);
				this.startDate = NowDate;
			}
			
		},
		// 新增按钮
		AddBtn:function(){
			this.IsShow = !this.IsShow;
			this.title = "新增任务";
			this.MaskTaskName = "";
			this.MaskBuinessType = "";
			this.MaskDes = "";
			this.MaskTaskType = "TimedTask";
			this.MaskIntervalD = 0;
			this.MaskIntervalH = 0;
			this.MaskIntervalM = 0;
			this.MaskIntervalS = 0;
			this.Maskval = GetCurrentDate();
			this.MaskIsDisable = false;
			this.OpenOrClose = (this.MaskTaskType == "OneOff") ? true:false;
		},
		// 新增和更新任务 中的 "确认按钮"
		SureBtn:function(e){
			var Interval = this.MaskIntervalD + ":" + this.MaskIntervalH + ":" + this.MaskIntervalM + ":" + this.MaskIntervalS;
			if(this.MaskTaskType == "OneOff")
			{
				Interval = "";
			}
			var AssembleData = JSON.stringify({
				"Task_Name":this.MaskTaskName,
				"Task_TaskType":this.MaskTaskType,
				"Task_BusinessType":this.MaskBuinessType,
				"Task_PresetTime":this.CalendarDateAnalysis(this.Maskval),
				"Task_Interval": Interval,
				"Task_Describe":this.MaskDes
			});
			switch($(e.target).parent().parent().parent().find("h2").html())
			{
				case "更新任务" :
				this.sendAjax1(this.DataMask[1]["url"], this.DataMask[1]["Type"], AssembleData);
				break;
				case "新增任务" :
				this.sendAjax1(this.DataMask[0]["url"], this.DataMask[0]["Type"], AssembleData);
				break;
			}
			this.IsShow = !this.IsShow;
		},
		// 新增和更新任务 中的 "取消按钮"
		MaskCancel:function(){
			this.IsShow = !this.IsShow;
		},
		// 更新按钮
		UpdateBtn:function(e){
			this.IsShow = !this.IsShow;
			const btn = e.target;
			this.title = "更新任务";

			this.MaskTaskName = $(btn).parent().parent().find(".name").html();
			this.MaskBuinessType = $(btn).parent().parent().find(".BusinessType").html();
			this.MaskDes = $(btn).parent().parent().find(".Describe").html();
			this.MaskTaskType = $(btn).parent().parent().find(".taskType").html();
			this.Maskval = this.SystemDateAnalysis($(btn).parent().parent().find(".PreseTime").html());
			const Interval = $(btn).parent().parent().find(".Interval").html();
			this.MaskIntervalD = Interval.split(":")[0];
			this.MaskIntervalH = Interval.split(":")[1];
			this.MaskIntervalM = Interval.split(":")[2];
			this.MaskIntervalS = Interval.split(":")[3];
			this.MaskIsDisable = true;
			this.OpenOrClose = (this.MaskTaskType == "OneOff") ? true:false;
			
			
		},
		// 把 2020-04-26 11:54:22 转换成2020-04-26T11:54:22
		SystemDateAnalysis:function(oldDate){
			return oldDate.split(" ").join("T");
		},
		// 把 2020-04-26T11:54:22 转换成2020-04-26 11:54:22
		CalendarDateAnalysis:function(calendarDate){
			return calendarDate.split("T").join(" ");
		},
		//  选择业务类型，打开和关闭 时间间隔
		OpenOrCloseInterval:function(){
			this.MaskIsDisable = false;
			this.OpenOrClose = (this.MaskTaskType == "OneOff") ? true:false;
		},
		RemoveBtn: function(){
			var flag = confirm("确定要删除这条数据吗?")
			if(flag)
			{
				const btn = e.target; 
				const id = $(btn).parent().parent().find(".id").html();
				const index = $(".active").index();
				const url = "http://192.168.1.47:5000/api/Tasks/" + id;
				this.sendAjax1(url,"delete", null);
			}
		},
		NextBtn:function(){
			var activeIndex = $(".active").index();
			if (activeIndex == this.PageInfo["TotalPage"] - 1) {
				$(".next").parent().addClass("disabled");
			}
			if (this.PageInfo["CurrentPage"] != this.PageInfo["TotalPage"]) {
				activeIndex += 1;
				var url = "http://192.168.1.47:5000/api/Tasks?pageNumber=" + activeIndex + "&PageSize=" + this.PageInfo["ItemsPerPage"];
				this.sendAjax1(url,"get",null);
				$(".page li").eq(this.PageInfo["CurrentPage"]).addClass("active");
				$(".page li").eq(this.PageInfo["CurrentPage"] - 1).removeClass("active");
				$(".prev").parent().removeClass("disabled");
			}
		},
		PrevBtn:function(){
			var index = $(".page .active").index();
			if (index == 2) {
				$(".prev").parent().addClass("disabled");
			}
			if (index > 1) {
				$(".page li").eq(index - 1).addClass("active");
				$(".page li").eq(index).removeClass("active");
				$(".next").parent().removeClass("disabled");
				var url = "http://192.168.1.47:5000/api/Tasks?pageNumber=" + (index - 1) + "&PageSize=" + this.PageInfo["ItemsPerPage"];
				this.sendAjax1(url,"get",null);
			
			}
		},
		NumberBtn:function(){
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
			if (indeclick == this.PageInfo["TotalPage"]) {
				$(".next").parent().addClass("disabled");
			} else {
				$(".next").parent().removeClass("disabled");
			}
			$(this).addClass("active");
			
			$(".page li").eq(index).removeClass("active");
			var url = "http://192.168.1.47:5000/api/Tasks?pageNumber=" + indeclick + "&PageSize=" + this.PageInfo["ItemsPerPage"];
			this.sendAjax1(url,"get",null);
			
		}
	},
});