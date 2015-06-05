<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HandoverLoger.aspx.cs" Inherits="WorkingShifts.Web.UI_WorkingShifts.HandoverLoger" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>交接班记录</title>
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
    
    <script>
        //首先获得分厂的组织机构ID
        var organizationId = 'zc_nxjc_byc_byf';
        var queryUrl = 'HandoverLoger.aspx/GetAppSettingValue';
        $.ajax({
            type: "POST",
            url: queryUrl,
            data: '',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                organizationId = msg.d;
                getStaffInfo();
            }
        });
    </script>
	<div id="wrapper" class="easyui-panel" style="width:100%;height:auto;padding:2px;">
        <div class="easyui-panel" style="padding:5px;width:100%;">
            <a href="javascript:void(0)" class="easyui-linkbutton easyui-tooltip tooltip-f" data-options="plain:true,iconCls:'icon-ok'" title="提交后不可修改，请谨慎操作。" onclick="submit()">提交</a>
        </div>
	    <div id="p" class="easyui-panel" title="交接班记录" style="width:100%;height:auto;padding:10px;">
            <div style="float:left;">
                时间：
                <input id="time" class="easyui-datetimespinner" value="1111/11/11"  data-options="selections:[[11,13],[14,16]]" style="width:180px;" />
                班次：
                <select id="shifts" class="easyui-combobox" data-options="editable: false" name="state" style="width:80px;">
		            <option value="A">甲班</option>
		            <option value="B">乙班</option>
		            <option value="C">丙班</option>
                </select>
                <a href="#" class="easyui-linkbutton" iconCls="icon-reload" plain="true" onclick="RefreshFun();">刷新</a>
            </div>
            <div style="float:right;">
                当前班组：
                <input id="workingTeam" class="easyui-combobox" data-options="valueField:'Name',textField:'Name',editable:false" name="state" style="width:180px;" />
                负责人：
                <input id="chargeMan" class="easyui-combobox" data-options="valueField:'StaffID',textField:'Combined'" style="width:180px" />
            </div>
            <div style="margin-top: 40px;">
                <!--操作员DataGrid-->
	            <table id="operatorSelector" class="easyui-datagrid" title="操作员选择" style="width:100%;height:auto"
			            data-options="
				            iconCls: 'icon-edit',
				            singleSelect: true,
				            onClickRow: osOnClickRow,
                            toolbar: '#tbOperatorSelector'
			            ">
		            <thead>
			            <tr>
                            <th data-options="field:'OrganizationID',hidden:true">DCSID</th>
                            <th data-options="field:'Name',width:100"></th>
                            <th data-options="field:'StaffName_SHSPS',hidden:true"></th>
                            <th data-options="field:'StaffID_SHSPS',width:180,
						            formatter:function(value,row){
							            return row.StaffName_SHSPS;
						            },
						            editor:{
							            type:'combobox',
							            options:{
								            valueField:'StaffID',
								            textField:'Combined',
                                            data: getStaffInfo()
							            }
						            }">石灰石破碎</th>
                            <th data-options="field:'StaffName_MFZB',hidden:true"></th>
                            <th data-options="field:'StaffID_MFZB',width:180,
						            formatter:function(value,row){
							            return row.StaffName_MFZB;
						            },
						            editor:{
							            type:'combobox',
							            options:{
								            valueField:'StaffID',
								            textField:'Combined',
                                            data: getStaffInfo()
							            }
						            }">煤粉制备</th>
                            <th data-options="field:'StaffName_SHENGLIAOZB',hidden:true"></th>
                            <th data-options="field:'StaffID_SHENGLIAOZB',width:180,
						            formatter:function(value,row){
							            return row.StaffName_SHENGLIAOZB;
						            },
						            editor:{
							            type:'combobox',
							            options:{
								            valueField:'StaffID',
								            textField:'Combined',
                                            data: getStaffInfo()
							            }
						            }">生料制备</th>
                            <th data-options="field:'StaffName_SHULIAOZB',hidden:true"></th>
                            <th data-options="field:'StaffID_SHULIAOZB',width:180,
						            formatter:function(value,row){
							            return row.StaffName_SHULIAOZB;
						            },
						            editor:{
							            type:'combobox',
							            options:{
								            valueField:'StaffID',
								            textField:'Combined',
                                            data: getStaffInfo()
							            }
						            }">熟料制备</th>
                            <th data-options="field:'StaffName_SHUINIZB',hidden:true"></th>
                            <th data-options="field:'StaffID_SHUINIZB',width:180,
						            formatter:function(value,row){
							            return row.StaffName_SHUINIZB;
						            },
						            editor:{
							            type:'combobox',
							            options:{
								            valueField:'StaffID',
								            textField:'Combined',
                                            data: getStaffInfo()
							            }
						            }">水泥粉磨</th>
                            <th data-options="field:'StaffName_FZZB',hidden:true"></th>
                            <th data-options="field:'StaffID_FZZB',width:180,
						            formatter:function(value,row){
							            return row.StaffName_FZZB;
						            },
						            editor:{
							            type:'combobox',
							            options:{
								            valueField:'StaffID',
								            textField:'Combined',
                                            data: getStaffInfo()
							            }
						            }">辅助生产</th>
			            </tr>
		            </thead>
	            </table>
	            <div id="tbOperatorSelector" style="height:auto">
		            <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-save',plain:true" onclick="osAccept()">应用</a>
		            <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-undo',plain:true" onclick="osReject()">取消</a>
	            </div>
            </div>
	    </div>
        <div class="easyui-panel" style="width:100%;height:auto;padding:10px;">
            <div>
                <!--停机记录DataGrid-->
	            <table id="haltLoger" class="easyui-datagrid" title="停机原因" style="width:100%;height:auto"
			            data-options="
				            iconCls: 'icon-edit',
				            singleSelect: true,
				            onClickRow: hlOnClickRow,
                            toolbar: '#tbHaltLoger'
			            ">
		            <thead>
			            <tr>
                            <th data-options="field:'MachineHaltLogID',hidden:true"></th>
				            <th data-options="field:'HaltTime',width:120">停机时间</th>
                            <th data-options="field:'ReasonID',hidden:true"></th>
                            <th data-options="field:'ReasonText',hidden:true"></th>
                            <th data-options="field:'Label',width:120">设备点号</th>
                            <th data-options="field:'EquipmentName',width:120">设备名称</th>
				            <th data-options="field:'Reason',width:300,
                                    formatter:function(value,row){
							            return row.ReasonText;
						            },
						            editor:{
							            type:'combotree',
							            options:{
								            valueField:'MachineHaltReasonID',
								            textField:'ReasonText',
								            data:getMachineHaltReasons(),
                                            onClick: changeHaltReason
							            }
						            }">停机原因</th>
                            <th data-options="field:'Remarks',width:300,editor:{type:'textbox'}">备注</th>
			            </tr>
		            </thead>
	            </table>
	            <div id="tbHaltLoger" style="height:auto">
		            <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-save',plain:true" onclick="hlAccept()">应用</a>
		            <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-undo',plain:true" onclick="hlReject()">取消</a>
	            </div>
            </div>
            <div style="margin-top: 20px;">
                <!--dcs报警记录DataGrid-->
	            <table id="dcsWarningLoger" class="easyui-datagrid" title="DCS报警记录" style="width:100%;height:auto"
			            data-options="
				            iconCls: 'icon-edit',
				            singleSelect: true,
				            onClickRow: dcswlOnClickRow,
                            toolbar: '#tbdcsWarningLoger'
			            ">
		            <thead>
			            <tr>
                            <th data-options="field:'DCSWarningLogID',hidden:true">DCSWarningLogID</th>
				            <th data-options="field:'StartingTime',width:120">报警开始时间</th>
                            <th data-options="field:'EndingTime',width:120">报警结束时间</th>
                            <th data-options="field:'Label',width:120">报警点号</th>
				            <th data-options="field:'Message',width:120">报警原因</th>
                            <th data-options="field:'HandleInformation',width:300,editor:{type:'textbox'}">处理情况</th>
                            <th data-options="field:'Remarks',width:300,editor:{type:'textbox'}">备注</th>
			            </tr>
		            </thead>
	            </table>
	            <div id="tbdcsWarningLoger" style="height:auto">
		            <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-save',plain:true" onclick="dcswlAccept()">应用</a>
		            <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-undo',plain:true" onclick="dcswlReject()">取消</a>
	            </div>
            </div>
            <div style="margin-top: 20px;">
                <!--能耗报警记录DataGrid-->
	            <table id="ecAlarmLoger" class="easyui-datagrid" title="能耗报警记录" style="width:100%;height:auto"
			            data-options="
				            iconCls: 'icon-edit',
				            singleSelect: true,
				            onClickRow: ecalOnClickRow,
                            toolbar: '#tbecAlarmLoger'
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
                            <th data-options="field:'Reason',width:300,editor:{type:'textbox'}">原因分析</th>
			            </tr>
		            </thead>
	            </table>
	            <div id="tbecAlarmLoger" style="height:auto">
		            <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-save',plain:true" onclick="ecalAccept()">应用</a>
		            <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-undo',plain:true" onclick="ecalReject()">取消</a>
	            </div>
            </div>
        </div>
        <div class="easyui-panel" style="width:100%;height:auto;padding:10px;">
            <table style="width:100%;">
                <tr>
                    <td style="width:80px">本班生产计划完成情况</td>
                    <td>
                        <textarea id="performToObjectives" style="width:95%;height:50px;"></textarea></td>
                    <td style="width:80px">本班出现的问题及处理情况</td>
                    <td>
                        <textarea id="problemsAndSettlements" style="width:95%;height:50px;"></textarea></td>
                </tr>
                <tr>
                    <td style="width:80px">本班设备运行情况</td>
                    <td>
                        <textarea id="equipmentSituation" style="width:95%;height:50px;"></textarea></td>
                    <td style="width:80px">下班工作重点及建议</td>
                    <td>
                        <textarea id="advicesToNextShift" style="width:95%;height:50px;"></textarea></td>
                </tr>
            </table>
        </div>
        <div class="easyui-panel" style="padding:5px;width:100%;">
            <a href="javascript:void(0)" class="easyui-linkbutton easyui-tooltip tooltip-f" data-options="plain:true,iconCls:'icon-ok'" title="提交后不可修改，请谨慎操作。" onclick="submit()">提交</a>
        </div>
	</div>
	<script type="text/javascript">

	    
	    var staffInfo;//
	    var shiftTimeInfo;//时间班的起止时间信息
	    var machineHaltReasons;

	    $(document).ready(function () {
	        //初始化
	        init();
	    });
        
	    function init() {
	        osEditIndex = undefined;//初始化为undefined	              
	        // 初始化班组下拉列表
	        initializeWorkingTeam();
	        // 初始职工人员列表
	        initializeStaffInfo();
	        // 初始负责人下拉列表
	        initializeChargManCombobox();
	        //获取停机原因信息
	        // getMachineHaltReasons();
	        // 获取DCS系统信息
	        getDCSSystem();
	        //获取班次时间信息
	        getShiftTime();
	        // 获取停机记录
	        getHaltLog();
	        // 获取报警记录
	        getWarningLog();
	        // 获取能耗报警记录
	        getEnergyConsumptionAlarmLog();
	    }
	    //刷新
	    function RefreshFun() {
	        init();
	    }
	    function getFactoryOrganizationID() {
	        var queryUrl = 'HandoverLoger.aspx/GetAppSettingValue';
	        $.ajax({
	            type: "POST",
	            url: queryUrl,
	            data: '',
	            contentType: "application/json; charset=utf-8",
	            dataType: "json",
	            success: function (msg) {
	                organizationId = msg.d;
	                initDelay();
	            }
	        });
	    }
	    // 初始化班组下拉列表
	    function initializeWorkingTeam() {
	        var queryUrl = 'HandoverLoger.aspx/GetWorkingTeamWithComboboxFormat';
	        var dataToSend = '{organizationId: "' + organizationId + '"}';

	        $.ajax({
	            type: "POST",
	            url: queryUrl,
	            data: dataToSend,
	            contentType: "application/json; charset=utf-8",
	            dataType: "json",
	            success: function (msg) {
	                initializeWorkingTeamCombobox(jQuery.parseJSON(msg.d));
	            }
	        });
	    }

	    function initializeWorkingTeamCombobox(json) {
	        $('#workingTeam').combobox({ data: json, onSelect: changeChargeMan });
	    }

        // 获取员工信息
	    function getStaffInfo() {
	        if (staffInfo == null || staffInfo == '[]') {
	            var queryUrl = 'HandoverLoger.aspx/GetStaffInfoWithComboboxFormat';
	            var dataToSend = '{organizationId: "' + organizationId + '"}';

	            $.ajax({
	                type: "POST",
	                url: queryUrl,
	                data: dataToSend,
	                contentType: "application/json; charset=utf-8",
	                dataType: "json",
	                async: false,
	                success: function (msg) {
	                    staffInfo = jQuery.parseJSON(msg.d);
	                }
	            });
	        }

	        return staffInfo;
	    }	   
        //获取班次时间
	    function getShiftTime() {
	        var shift = $("#shifts").combobox('getText');
	        var queryUrl = 'HandoverLoger.aspx/GetShiftTimeInfo';
	        var dataToSend = '{organizationId: "' + organizationId +'",shift:"'+shift+ '"}';
	        $.ajax({
	            type: "POST",
	            url: queryUrl,
	            data: dataToSend,
	            contentType: "application/json; charset=utf-8",
	            dataType: "json",
	            async: false,
	            success: function (msg) {
	                shiftTimeInfo = jQuery.parseJSON(msg.d);
	            }
	        });
	    }
	    // 初始职工人员列表
	    function initializeStaffInfo() {
	        getStaffInfo();
	    }

        // 获取停机原因
	    function getMachineHaltReasons() {
	        if (machineHaltReasons == null) {
	            var queryUrl = 'HandoverLoger.aspx/GetMachineHaltReasonsWithCombotreeFormat';
	            var dataToSend = '';

	            $.ajax({
	                type: "POST",
	                url: queryUrl,
	                data: dataToSend,
	                contentType: "application/json; charset=utf-8",
	                dataType: "json",
	                async: false,
	                success: function (msg) {
	                    machineHaltReasons = jQuery.parseJSON(msg.d);
	                }
	            });
	        }

	        return machineHaltReasons;
	    }

	    // 初始负责人下拉列表
	    function initializeChargManCombobox() {
	        $('#chargeMan').combobox({ data: staffInfo });
	    }

	    // 更新负责人
	    function changeChargeMan() {
	        var workingTeamName = $('#workingTeam').combobox('getText');
	        getChargeManByWorkingTeam(workingTeamName);
	    }
        
        // 更新停机原因
	    function changeHaltReason(node) {
	        if (node.id.length != 7) {
	            $.messager.alert('提示', '停机原因需要明确到第三层', 'info');
	        }
	    }

        // 按班组获取负责人
	    function getChargeManByWorkingTeam(workingTeamName) {
	        var queryUrl = 'HandoverLoger.aspx/GetChargeManByWorkingTeamNameWithComboboxFormat';
	        var dataToSend = '{workingTeamName: "' + workingTeamName + '"}';

	        $.ajax({
	            type: "POST",
	            url: queryUrl,
	            data: dataToSend,
	            contentType: "application/json; charset=utf-8",
	            dataType: "json",
	            success: function (msg) {
	                setChargeManTextbox(jQuery.parseJSON(msg.d));
	            }
	        });
	    }

	    // 设置班组负责人文本框
	    function setChargeManTextbox(json) {
	        $('#chargeMan').combobox('setText', json.ID + "  " + json.Name);
	        $('#chargeMan').combobox('setValue', json.ID);
	    }

	    // 生成操作员选择grid
	    function getDCSSystem() {
	        var queryUrl = 'HandoverLoger.aspx/GetDCSSystemWithDataGridFormat';
	        var dataToSend = '{organizationId: "' + organizationId + '"}';

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

	    // 操作员选择
	    // os: operator selector
	    var osEditIndex = undefined;
	    function osEndEditing() {
	        if (osEditIndex == undefined) { return true }
	        if ($('#operatorSelector').datagrid('validateRow', osEditIndex)) {
	            // 获取操作员选择编辑器中的工段列
	            var cols = $('#operatorSelector').datagrid('getColumnFields');
                // 除去第1列是DCS名称，遍历工段列，每个工段有两列
	            for (var i = 2; i < cols.length; i += 2) {
	                // cols[i]: StaffName_xxx
                    // cols[i+1]: StaffID_xxx
	                var ed = $('#operatorSelector').datagrid('getEditor', { index: osEditIndex, field: cols[i + 1] });
	                var staffId = $(ed.target).combobox('getValue');
	                var staffName = $(ed.target).combobox('getText');
	                $('#operatorSelector').datagrid('getRows')[osEditIndex][cols[i + 1]] = staffId;
	                $('#operatorSelector').datagrid('getRows')[osEditIndex][cols[i]] = staffName;
	            }
                // 结束编辑
	            $('#operatorSelector').datagrid('endEdit', osEditIndex);
	            osEditIndex = undefined;
	            return true;
	        } else {
	            return false;
	        }
	    }
	    function osOnClickRow(index) {
	        if (osEditIndex != index) {
	            if (osEndEditing()) {
	                AcceptAll();
	                $('#operatorSelector').datagrid('selectRow', index)
							.datagrid('beginEdit', index);
	                osEditIndex = index;
	            } else {
	                $('#operatorSelector').datagrid('selectRow', osEditIndex);
	            }
	        }
	    }
	    function osAccept() {
	        if (osEndEditing()) {
	            $('#operatorSelector').datagrid('acceptChanges');
	        }
	    }
	    function osReject() {
	        $('#operatorSelector').datagrid('rejectChanges');
	        osEditIndex = undefined;
	    }

	    // 停机原因选择
	    function getHaltLog() {
	        var queryUrl = 'HandoverLoger.aspx/GetMachineHaltLogWithDataGridFormat';
	        var dataToSend = '{organizationId: "' + organizationId +'",startTime:"'+shiftTimeInfo.startTime+'",endTime:"'+shiftTimeInfo.endTime+ '"}';

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

	    // 停机原因选择
	    // hl: halt log

	    var hlEditIndex = undefined;
	    function hlEndEditing() {
	        if (hlEditIndex == undefined) { return true }
	        if ($('#haltLoger').datagrid('validateRow', hlEditIndex)) {
	            var ed = $('#haltLoger').datagrid('getEditor', { index: hlEditIndex, field: 'Reason' });
	            var reasonId = $(ed.target).combobox('getValue');
	            var reasonText = $(ed.target).combobox('getText');
	            $('#haltLoger').datagrid('getRows')[hlEditIndex]['ReasonID'] = reasonId;
	            $('#haltLoger').datagrid('getRows')[hlEditIndex]['ReasonText'] = reasonText;
	            $('#haltLoger').datagrid('endEdit', hlEditIndex);
	            hlEditIndex = undefined;
	            return true;
	        } else {
	            return false;
	        }
	        return true;
	    }
	    function hlOnClickRow(index) {
	        if (hlEditIndex != index) {
	            if (hlEndEditing()) {
	                AcceptAll();
	                $('#haltLoger').datagrid('selectRow', index)
							.datagrid('beginEdit', index);
	                hlEditIndex = index;
	            } else {
	                $('#haltLoger').datagrid('selectRow', hlEditIndex);
	            }
	        }
	    }
	    function hlAccept() {
	        if (hlEndEditing()) {
	            $('#haltLoger').datagrid('acceptChanges');
	        }
	    }
	    function hlReject() {
	        $('#haltLoger').datagrid('rejectChanges');
	        hlEditIndex = undefined;
	    }

	    // 生成报警信息grid
	    function getWarningLog() {
	        var queryUrl = 'HandoverLoger.aspx/GetDCSWarningLogWithDataGridFormat';
	        //var dataToSend = '{organizationId: "' + organizationId + '"}';
	        var dataToSend = '{organizationId: "' + organizationId + '",startTime:"' + shiftTimeInfo.startTime + '",endTime:"' + shiftTimeInfo.endTime + '"}';
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

	    // 报警原因选择
	    // dcswl: dcs warning log

	    var dcswlEditIndex = undefined;
	    function dcswlEndEditing() {
	        if (dcswlEditIndex == undefined) { return true }
	        $('#dcsWarningLoger').datagrid('endEdit', dcswlEditIndex);
	        dcswlEditIndex = undefined;
	        return true;
	    }
	    function dcswlOnClickRow(index) {
	        AcceptAll();
	        if (dcswlEditIndex != index) {
	            if (dcswlEndEditing()) {
	                $('#dcsWarningLoger').datagrid('selectRow', index)
							.datagrid('beginEdit', index);
	                dcswlEditIndex = index;
	            } else {
	                $('#dcsWarningLoger').datagrid('selectRow', dcswlEditIndex);
	            }
	        }
	    }
	    function dcswlAccept() {
	        if (dcswlEndEditing()) {
	            $('#dcsWarningLoger').datagrid('acceptChanges');
	        }
	    }
	    function dcswlReject() {
	        $('#dcsWarningLoger').datagrid('rejectChanges');
	        dcswlEditIndex = undefined;
	    }


	    // 生成能耗报警信息
	    function getEnergyConsumptionAlarmLog() {
	        var queryUrl = 'HandoverLoger.aspx/GetEnergyConsumptionAlarmLogWithDataGridFormat';
	        //var dataToSend = '{organizationId: "' + organizationId + '"}';
	        var dataToSend = '{organizationId: "' + organizationId + '",startTime:"' + shiftTimeInfo.startTime + '",endTime:"' + shiftTimeInfo.endTime + '"}';
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

	    // 能耗报警编辑
	    // ecal: energy consumption alarm log

	    var ecalEditIndex = undefined;
	    function ecalEndEditing() {
	        if (ecalEditIndex == undefined) { return true }
	        $('#ecAlarmLoger').datagrid('endEdit', ecalEditIndex);
	        ecalEditIndex = undefined;
	        return true;
	    }
	    function ecalOnClickRow(index) {
	        AcceptAll();
	        if (ecalEditIndex != index) {
	            if (ecalEndEditing()) {
	                $('#ecAlarmLoger').datagrid('selectRow', index)
							.datagrid('beginEdit', index);
	                ecalEditIndex = index;
	            } else {
	                $('#ecAlarmLoger').datagrid('selectRow', ecalEditIndex);
	            }
	        }
	    }
	    function ecalAccept() {
	        if (ecalEndEditing()) {
	            $('#ecAlarmLoger').datagrid('acceptChanges');
	        }
	    }
	    function ecalReject() {
	        $('#ecAlarmLoger').datagrid('rejectChanges');
	        ecalEditIndex = undefined;
	    }

	    // 取消选中
	    function UnselectAll() {
	        $('#operatorSelector').datagrid('unselectAll');
	        $('#haltLoger').datagrid('unselectAll');
	        $('#dcsWarningLoger').datagrid('unselectAll');
	        $('#ecAlarmLoger').datagrid('unselectAll');
	    }

	    // 应用变化
	    function AcceptAll() {
	        osAccept();
	        hlAccept();
	        dcswlAccept();
	        ecalAccept();

	        UnselectAll();
	    }

	    function Validate() {
	        // 检验班组
	        if ($('#workingTeam').combobox('getText') == "") {
	            $.messager.alert('错误', '请选择当前班组', 'error');
	            return false;
	        }

	        // 检验负责人
	        if ($('#chargeMan').combobox('getText') == "") {
	            $.messager.alert('错误', '请选择负责人', 'error');
	            return false;
	        }

	        // 检验停机原因
	        var haltLogs = $('#haltLoger').datagrid('getData');
	        for (var i = 0; i < haltLogs.total; i++) {
	            if (haltLogs.rows[i].ReasonID.length != 7) {
	                $.messager.alert('错误', '请为停机时间： ' + haltLogs.rows[i].HaltTime + ' 的记录选择明确的停机原因', 'error');
	                return false;
	            }
	        }
	    }

	    // 提交
	    function submit() {

	        AcceptAll();
	        if (Validate() == false)
	            return;

	        $.messager.confirm('确认', '确认提交交接班日志？', function (r) {
	            if (r) {
	                var time = "\"time\":\"" + $('#time').datetimespinner('getValue') + "\"";
	                var shifts = "\"shifts\":\"" + $('#shifts').combobox('getText') + "\"";
	                var team = "\"workingTeam\":\"" + $('#workingTeam').combobox('getValue') + "\"";
	                var chargeMan = "\"chargeMan\":\"" + $('#chargeMan').combobox('getValue') + "\"";

	                var operators = "\"operators\":" + (JSON.stringify($('#operatorSelector').datagrid('getData')));
	                var haltLogs = "\"haltLogs\":" + (JSON.stringify($('#haltLoger').datagrid('getData')));
	                var dcsWarningLogs = "\"dcsWarningLogs\":" + (JSON.stringify($('#dcsWarningLoger').datagrid('getData')));
	                var ecAlarmLogs = "\"energyConsumptionAlarmLogs\":" + (JSON.stringify($('#ecAlarmLoger').datagrid('getData')));

	                var performToObjectives = "\"performToObjectives\":\"" + $('#performToObjectives').val() + "\"";
	                var problemsAndSettlements = "\"problemsAndSettlements\":\"" + $('#problemsAndSettlements').val() + "\"";
	                var equipmentSituation = "\"equipmentSituation\":\"" + $('#equipmentSituation').val() + "\"";
	                var advicesToNextShift = "\"advicesToNextShift\":\"" + $('#advicesToNextShift').val() + "\"";

	                var loger = '{' + time + ',' + shifts + ',' + team + ',' + chargeMan + ',' + operators + ',' + haltLogs + ',' + dcsWarningLogs + ',' + ecAlarmLogs + ',' +
                        performToObjectives + ',' + problemsAndSettlements + ',' + equipmentSituation + ',' + advicesToNextShift + '}';

	                var queryUrl = 'HandoverLoger.aspx/CreateWorkingTeamShiftLog';
	                var dataToSend = '{organizationId:"' + organizationId + '",json:\'' + loger + '\'}';

	                $.ajax({
	                    type: "POST",
	                    url: queryUrl,
	                    data: dataToSend,
	                    contentType: "application/json; charset=utf-8",
	                    dataType: "json",
	                    success: function (msg) {
	                        if (msg.d == "success") {
	                            $.messager.alert('提示', '日志创建成功', 'info', function (r) {
	                                window.location.href = 'HandoverLoger.aspx';
	                            });
	                        }
	                    }
	                });
	            }
	        });
	    }
	</script>
    <form id="form1" runat="server"></form>
</body>
</html>
