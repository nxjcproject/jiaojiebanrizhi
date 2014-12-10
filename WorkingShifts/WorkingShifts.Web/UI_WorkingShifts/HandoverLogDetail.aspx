<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HandoverLogDetail.aspx.cs" Inherits="WorkingShifts.Web.UI_WorkingShifts.HandoverLogDetail" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>交接班记录详情</title>
    <link rel="stylesheet" type="text/css" href="/lib/ealib/themes/gray/easyui.css"/>
	<link rel="stylesheet" type="text/css" href="/lib/ealib/themes/icon.css"/>
    <link rel="stylesheet" type="text/css" href="/lib/extlib/themes/syExtIcon.css"/>
    <link rel="stylesheet" type="text/css" href="/lib/extlib/themes/syExtCss.css"/>

	<script type="text/javascript" src="/lib/ealib/jquery.min.js" charset="utf-8"></script>
	<script type="text/javascript" src="/lib/ealib/jquery.easyui.min.js" charset="utf-8"></script>
    <script type="text/javascript" src="/lib/ealib/easyui-lang-zh_CN.js" charset="utf-8"></script>

    <script type="text/javascript" src="/lib/ealib/extend/jquery.PrintArea.js" charset="utf-8"></script> 
    <script type="text/javascript" src="/lib/ealib/extend/jquery.jqprint.js" charset="utf-8"></script>

    <script type="text/javascript" src="/js/common/PrintFile.js" charset="utf-8"></script> 
</head>
<body>
	<div id="wrapper" class="easyui-panel" style="width:100%;height:auto;padding:2px;">
	    <div id="p" class="easyui-panel" title="交接班记录" style="width:100%;height:auto;padding:10px;">
            <div style="float:left;">
                时间：
                <input id="time" class="easyui-textbox" readonly="true" style="width:180px;" />
                班次：
                <select id="shifts" class="easyui-combobox" data-options="editable: false" name="state" readonly="true" style="width:80px;">
		            <option value="A">甲班</option>
		            <option value="B">乙班</option>
		            <option value="C">丙班</option>
                </select>
            </div>
            <div style="float:right;">
                当前班组：
                <input id="workingTeam" class="easyui-combobox" data-options="valueField:'Name',textField:'Name',editable:false" readonly="true" name="state" style="width:180px;" />
                负责人：
                <input id="chargeMan" class="easyui-combobox" data-options="valueField:'StaffID',textField:'Combined'" readonly="true" style="width:180px" />
            </div>
            <div style="margin-top: 40px;">
                <!--操作员DataGrid-->
	            <table id="operatorSelector" class="easyui-datagrid" title="操作员选择" style="width:100%;height:auto"
			            data-options="
				            iconCls: 'icon-edit',
				            singleSelect: true
			            ">
		            <thead>
			            <tr>
                            <th data-options="field:'OrganizationID',hidden:true">DCSID</th>
                            <th data-options="field:'Name',width:100"></th>
                            <th data-options="field:'StaffName_SHSPS',hidden:true"></th>
                            <th data-options="field:'StaffID_SHSPS',width:180">石灰石破碎</th>
                            <th data-options="field:'StaffName_MFZB',hidden:true"></th>
                            <th data-options="field:'StaffID_MFZB',width:180">煤粉制备</th>
                            <th data-options="field:'StaffName_SHENGLIAOZB',hidden:true"></th>
                            <th data-options="field:'StaffID_SHENGLIAOZB',width:180">生料制备</th>
                            <th data-options="field:'StaffName_SHULIAOZB',hidden:true"></th>
                            <th data-options="field:'StaffID_SHULIAOZB',width:180">熟料制备</th>
                            <th data-options="field:'StaffName_SHUINIZB',hidden:true"></th>
                            <th data-options="field:'StaffID_SHUINIZB',width:180">水泥粉磨</th>
                            <th data-options="field:'StaffName_FZZB',hidden:true"></th>
                            <th data-options="field:'StaffID_FZZB',width:180">辅助生产</th>
			            </tr>
		            </thead>
	            </table>
            </div>
	    </div>
        <div class="easyui-panel" style="width:100%;height:auto;padding:10px;">
            <div>
                <!--停机记录DataGrid-->
	            <table id="haltLoger" class="easyui-datagrid" title="停机原因" style="width:100%;height:auto"
			            data-options="
				            iconCls: 'icon-edit',
				            singleSelect: true
			            ">
		            <thead>
			            <tr>
                            <th data-options="field:'MachineHaltLogID',hidden:true"></th>
				            <th data-options="field:'HaltTime',width:120">停机时间</th>
                            <th data-options="field:'ReasonID',hidden:true"></th>
                            <th data-options="field:'Label',width:120">设备点号</th>
                            <th data-options="field:'EquipmentName',width:120">设备名称</th>
				            <th data-options="field:'ReasonText',width:300">停机原因</th>
                            <th data-options="field:'Remarks',width:300,editor:{type:'textbox'}">备注</th>
			            </tr>
		            </thead>
	            </table>
            </div>
            <div style="margin-top: 20px;">
                <!--dcs报警记录DataGrid-->
	            <table id="dcsWarningLoger" class="easyui-datagrid" title="DCS报警记录" style="width:100%;height:auto"
			            data-options="
				            iconCls: 'icon-edit',
				            singleSelect: true
			            ">
		            <thead>
			            <tr>
                            <th data-options="field:'DCSWarningLogID',hidden:true">DCSWarningLogID</th>
				            <th data-options="field:'StartingTime',width:120">报警开始时间</th>
                            <th data-options="field:'EndingTime',width:120">报警结束时间</th>
                            <th data-options="field:'Label',width:120">报警点号</th>
				            <th data-options="field:'Message',width:120">报警原因</th>
                            <th data-options="field:'HandleInformation',width:300">处理情况</th>
                            <th data-options="field:'Remarks',width:300">备注</th>
			            </tr>
		            </thead>
	            </table>
            </div>
            <div style="margin-top: 20px;">
                <!--能耗报警记录DataGrid-->
	            <table id="ecAlarmLoger" class="easyui-datagrid" title="能耗报警记录" style="width:100%;height:auto"
			            data-options="
				            iconCls: 'icon-edit',
				            singleSelect: true
			            ">
		            <thead>
			            <tr>
                            <th data-options="field:'EnergyConsumptionAlarmLogID',hidden:true">EnergyConsumptionAlarmLogID</th>
				            <th data-options="field:'StartTime',width:120">报警时间</th>
                            <th data-options="field:'TimeSpan',width:220">参数超标时间段</th>
				            <th data-options="field:'Name',width:150">工序名称</th>
                            <th data-options="field:'StandardValue',width:120">标准值</th>
                            <th data-options="field:'ActualValue',width:120">实际值</th>
                            <th data-options="field:'Superscale',width:120">超标（%）</th>
                            <th data-options="field:'Reason',width:300">原因分析</th>
			            </tr>
		            </thead>
	            </table>
            </div>
        </div>
        <div class="easyui-panel" style="width:100%;height:auto;padding:10px;">
            <table style="width:100%;">
                <tr>
                    <td style="width:80px">本班生产计划完成情况</td>
                    <td>
                        <textarea id="performToObjectives" style="width:95%;height:50px;" readonly="readonly"></textarea></td>
                    <td style="width:80px">本班出现的问题及处理情况</td>
                    <td>
                        <textarea id="problemsAndSettlements" style="width:95%;height:50px;" readonly="readonly"></textarea></td>
                </tr>
                <tr>
                    <td style="width:80px">本班设备运行情况</td>
                    <td>
                        <textarea id="equipmentSituation" style="width:95%;height:50px;" readonly="readonly"></textarea></td>
                    <td style="width:80px">下班工作重点及建议</td>
                    <td>
                        <textarea id="advicesToNextShift" style="width:95%;height:50px;" readonly="readonly"></textarea></td>
                </tr>
            </table>
        </div>
	</div>
	<script type="text/javascript">

	    var organizationId = 'C41B1F47-A48A-495F-A890-0AABB2F3BFF7';
	    var workingTeamShiftLogId = '7740FE1A-3E6C-4C25-89FF-D08C6EE3E995';

	    $(document).ready(function () {
	        // 获取交接班日志
	        getWorkingShiftLog();
	        // 获取操作员记录
	        getOperatorsLog();
	        // 获取停机记录
	        getHaltLog();
	        // 获取报警记录
	        getWarningLog();
	        // 获取能耗报警记录
	        getEnergyConsumptionAlarmLog();
	    });

        // 获取交接班日志信息
	    function getWorkingShiftLog() {
	        var queryUrl = 'HandoverLogDetail.aspx/GetWorkingTeamShiftLog';
	        var dataToSend = '{workingTeamShiftLogId: "' + workingTeamShiftLogId + '"}';

	        $.ajax({
	            type: "POST",
	            url: queryUrl,
	            data: dataToSend,
	            contentType: "application/json; charset=utf-8",
	            dataType: "json",
	            success: function (msg) {
	                 updateWorkingTeamShiftLog(jQuery.parseJSON(msg.d));
	            }
	        });
	    }

        // 更新交接班界面
	    function updateWorkingTeamShiftLog(json) {
	        $('#time').textbox('setText',json.ShiftDate);
	        $('#shifts').combobox('setText', json.Shifts);
	        $('#workingTeam').combobox('setText', json.WorkingTeam);
	        $('#chargeMan').combobox('setText', json.ChargeManID + "  " + json.ChargeManName);
	        $('#performToObjectives').val(json.PerformToObjectives);
	        $('#problemsAndSettlements').val(json.ProblemsAndSettlements);
	        $('#equipmentSituation').val(json.EquipmentSituation);
	        $('#advicesToNextShift').val(json.AdvicesToNextShift);
	    }


	    // 获取操作员记录
	    function getOperatorsLog() {
	        var queryUrl = 'HandoverLogDetail.aspx/GetOperatorsLog';
	        var dataToSend = '{workingTeamShiftLogId: "' + workingTeamShiftLogId + '"}';

	        $.ajax({
	            type: "POST",
	            url: queryUrl,
	            data: dataToSend,
	            contentType: "application/json; charset=utf-8",
	            dataType: "json",
	            success: function (msg) {
	                initializeStaffSelector(jQuery.parseJSON(msg.d));
	            }
	        });
	    }

	    function initializeStaffSelector(json) {
	        $('#operatorSelector').datagrid({
	            data: json
	        });
	    }

	    // 获取停机原因
	    function getHaltLog() {
	        var queryUrl = 'HandoverLogDetail.aspx/GetMachineHaltLogWithDataGridFormat';
	        var dataToSend = '{organizationId: "' + organizationId + '", workingTeamShiftLogId: "' + workingTeamShiftLogId + '"}';

	        $.ajax({
	            type: "POST",
	            url: queryUrl,
	            data: dataToSend,
	            contentType: "application/json; charset=utf-8",
	            dataType: "json",
	            success: function (msg) {
	                initializeHaltLoger(jQuery.parseJSON(msg.d));
	            }
	        });
	    }

	    function initializeHaltLoger(json) {
	        $('#haltLoger').datagrid({
	            data: json
	        });
	    }

	    // 生成报警信息grid
	    function getWarningLog() {
	        var queryUrl = 'HandoverLogDetail.aspx/GetDCSWarningLogWithDataGridFormat';
	        var dataToSend = '{organizationId: "' + organizationId + '", workingTeamShiftLogId: "' + workingTeamShiftLogId + '"}';

	        $.ajax({
	            type: "POST",
	            url: queryUrl,
	            data: dataToSend,
	            contentType: "application/json; charset=utf-8",
	            dataType: "json",
	            success: function (msg) {
	                initializeWarningLoger(jQuery.parseJSON(msg.d));
	            }
	        });
	    }

	    function initializeWarningLoger(json) {
	        $('#dcsWarningLoger').datagrid({
	            data: json
	        });
	    }

	    // 生成能耗报警信息
	    function getEnergyConsumptionAlarmLog() {
	        var queryUrl = 'HandoverLogDetail.aspx/GetEnergyConsumptionAlarmLogWithDataGridFormat';
	        var dataToSend = '{organizationId: "' + organizationId + '", workingTeamShiftLogId: "' + workingTeamShiftLogId + '"}';

	        $.ajax({
	            type: "POST",
	            url: queryUrl,
	            data: dataToSend,
	            contentType: "application/json; charset=utf-8",
	            dataType: "json",
	            success: function (msg) {
	                initializeEnergyConsumptionAlarmLoger(jQuery.parseJSON(msg.d));
	            }
	        });
	    }

	    function initializeEnergyConsumptionAlarmLoger(json) {
	        $('#ecAlarmLoger').datagrid({
	            data: json
	        });
	    }

	    // 取消选中
	    function UnselectAll() {
	        $('#operatorSelector').datagrid('unselectAll');
	        $('#haltLoger').datagrid('unselectAll');
	        $('#dcsWarningLoger').datagrid('unselectAll');
	        $('#ecAlarmLoger').datagrid('unselectAll');
	    }

	</script>
    <form id="form1" runat="server"></form>
</body>
</html>
