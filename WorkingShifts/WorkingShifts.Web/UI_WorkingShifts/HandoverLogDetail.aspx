﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HandoverLogDetail.aspx.cs" Inherits="WorkingShifts.Web.UI_WorkingShifts.HandoverLogDetail" %>


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
    <script type="text/javascript" src="/js/common/jquery.utility.js"></script>
</head>
<body>
	<div id="wrapper" class="easyui-panel" style="width:100%;height:auto;padding:2px;">
        <div class="easyui-panel" style="padding:5px;width:100%;">
            <a href="javascript:void(0)" class="easyui-linkbutton" data-options="plain:true,iconCls:'icon-back'" onclick="javascript:history.go(-1)" >返回</a> | 
            <a href="javascript:void(0)" class="easyui-linkbutton" data-options="plain:true,iconCls:'icon-filter'" onclick="$('#dlgStocktaking').dialog('open')">盘库信息</a> | 
            <a href="javascript:void(0)" class="easyui-linkbutton" data-options="plain:true,iconCls:'ext-icon-group'" onclick="$('#dlgOperator').dialog('open')">操作员信息</a>
        </div>
	    <div id="p" class="easyui-panel" title="交接班记录" style="width:100%;height:auto;padding:10px;">
            <div>
                时间：
                <input id="time" class="easyui-textbox" readonly="true" style="width:120px;" />
                班次：
                <select id="shifts" class="easyui-combobox" data-options="editable: false" name="state" readonly="true" style="width:80px;">
		            <option value="A">甲班</option>
		            <option value="B">乙班</option>
		            <option value="C">丙班</option>
                </select>
                班组：
                <input id="workingTeam" class="easyui-combobox" data-options="valueField:'Name',textField:'Name',editable:false" readonly="true" name="state" style="width:80px;" />
                负责人：
                <input id="chargeMan" class="easyui-combobox" data-options="valueField:'StaffID',textField:'Combined'" readonly="true" style="width:180px" />
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
                            <th data-options="field:'Remarks',width:300">备注</th>
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
				            <th data-options="field:'Name',width:100">工序名称</th>
                            <th data-options="field:'EnergyConsumptionType',width:100">报警类型</th>
                            <th data-options="field:'StandardValue',width:100">标准值</th>
                            <th data-options="field:'ActualValue',width:100">实际值</th>
                            <th data-options="field:'Superscale',width:100">超标（%）</th>
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
    <!-- 操作员信息对话框开始 -->
    <div id="dlgOperator" class="easyui-dialog" title="操作员信息" style="width:1000px;height:400px;" 
        data-options="
            iconCls:'ext-icon-group',
            modal:true,
            closed:true,
        	buttons: [{
		        text:'确认',
		        iconCls:'icon-ok',
		        handler:function(){
			        $('#dlgOperator').dialog('close');
		        }
	        }]
        ">
        <!--操作员DataGrid-->
	    <table id="operatorSelector" class="easyui-datagrid" style="width:100%;height:100%"
			    data-options="singleSelect: true">
	    </table>
    </div>
    <!-- 操作员信息对话框结束 -->
    <!-- 盘库信息对话框开始 -->
    <div id="dlgStocktaking" class="easyui-dialog" title="盘库信息" style="width:700px;height:400px;" 
        data-options="
            iconCls:'icon-filter',
            modal:true,
            closed:true,
        	buttons: [{
		        text:'确认',
		        iconCls:'icon-ok',
		        handler:function(){
			        $('#dlgStocktaking').dialog('close');
		        }
	        }]
        ">
        <!--盘库信息DataGrid-->
	    <table id="dgStocktaking" class="easyui-datagrid" style="width:100%;height:100%;"
			    data-options="
				    iconCls: 'icon-edit',
				    singleSelect: true
			    ">
		    <thead>
			    <tr>
                    <th data-options="field:'OrganizationName',width:120">生产线</th>
				    <th data-options="field:'Name',width:120">物料名称</th>
                    <th data-options="field:'Unit',width:40">单位</th>
				    <th data-options="field:'Data',width:120">系统值</th>
                    <th data-options="field:'DataValue',width:120,styler:DataStyler">修正值</th>
                    <th data-options="field:'Remark',width:120">备注</th>
			    </tr>
		    </thead>
	    </table>
    </div>
    <!-- 盘库信息对话框结束 -->
	<script type="text/javascript">

	    var organizationId = $.getUrlParam('organizationId'); //'C41B1F47-A48A-495F-A890-0AABB2F3BFF7'; //测试用
	    var workingTeamShiftLogId = $.getUrlParam('workingTeamShiftLogId'); //'7740FE1A-3E6C-4C25-89FF-D08C6EE3E995'; //测试用

	    $(document).ready(function () {
	        // 获取交接班日志
	        getWorkingShiftLog();
	        // 获取操作员记录列
	        getDataGridColumns();
	        // 获取操作员记录
	        getOperatorsLog();
	        // 获取停机记录
	        getHaltLog();
	        // 获取报警记录
	        getWarningLog();
	        // 获取能耗报警记录
	        getEnergyConsumptionAlarmLog();
	        // 获取盘库信息
	        getStockingLog();
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

	    // 获取操作员DATAGRID列
	    function getDataGridColumns() {
	        var queryUrl = 'HandoverLogDetail.aspx/GetWorkingSectionsWithDataColumnFormat';
	        var dataToSend = '{organizationId: "' + organizationId + '"}';

	        $.ajax({
	            type: "POST",
	            url: queryUrl,
	            data: dataToSend,
	            contentType: "application/json; charset=utf-8",
	            dataType: "json",
	            success: function (msg) {
	                initializeColumns(msg.d);
	            }
	        });
	    }

	    // 初始化操作员DATAGRID列
	    function initializeColumns(json) {
	        $('#operatorSelector').datagrid({
	            columns: eval(json)
	        });
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

	    // 获取盘库信息
	    function getStockingLog() {
	        var queryUrl = 'HandoverLogDetail.aspx/GetStocktakingLogWithDataGridFormat';
	        var dataToSend = '{workingTeamShiftLogId: "' + workingTeamShiftLogId + '"}';

	        $.ajax({
	            type: "POST",
	            url: queryUrl,
	            data: dataToSend,
	            contentType: "application/json; charset=utf-8",
	            dataType: "json",
	            success: function (msg) {
	                $('#dgStocktaking').datagrid({
	                    data: jQuery.parseJSON(msg.d)
	                });
	            }
	        });
	    }

	    function DataStyler(value, row, index) {
	        if (value != row.Data) {
	            return 'background-color:#00FFFF;';
	        }
	    }
	</script>
    <form id="form1" runat="server"></form>
</body>
</html>
