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

    <script type="text/javascript" src="/UI_WorkingShifts/js/page/HandoverLoger.js" charset="utf-8"></script>
    <script type="text/javascript" src="/UI_WorkingShifts/js/page/ShiftsInfo.js" charset="utf-8"></script>
    <script type="text/javascript" src="/UI_WorkingShifts/js/page/WorkingTeam.js" charset="utf-8"></script>
    <!-- 盘库信息 -->
    <script type="text/javascript" src="/UI_WorkingShifts/js/page/Stocktaking.js" charset="utf-8"></script>
    <!-- 操作员信息 -->
    <script type="text/javascript" src="/UI_WorkingShifts/js/page/OperatorLoger.js" charset="utf-8"></script>
    <script type="text/javascript" src="/UI_WorkingShifts/js/page/HaltLoger.js" charset="utf-8"></script>
    <script type="text/javascript" src="/UI_WorkingShifts/js/page/DcsWarningLoger.js" charset="utf-8"></script>
    <script type="text/javascript" src="/UI_WorkingShifts/js/page/EnergyConsumptionAlarmLoger.js" charset="utf-8"></script>
</head>
<body>
    
    <script>

    </script>
	<div id="wrapper" class="easyui-panel" style="width:100%;height:auto;padding:2px;">
        <div class="easyui-panel" style="padding:5px;width:100%;">
            <a href="javascript:void(0)" class="easyui-linkbutton easyui-tooltip tooltip-f" data-options="plain:true,iconCls:'icon-ok'" title="提交后不可修改，请谨慎操作。" onclick="submit()">提交</a> | 
            <a href="javascript:void(0)" class="easyui-linkbutton easyui-tooltip tooltip-f" data-options="plain:true,iconCls:'icon-filter'" title="填写盘库信息。可不填写，默认为系统累计。" onclick="stocktaking.OpenDlgStocktaking()">盘库信息</a> | 
            <a href="javascript:void(0)" class="easyui-linkbutton easyui-tooltip tooltip-f" data-options="plain:true,iconCls:'ext-icon-group'" title="填写操作员信息。可不填写，默认为上次编排，如有变动请修改。" onclick="operatorLoger.OpenDlgOperator()">操作员信息</a>
        </div>
	    <div id="p" class="easyui-panel" title="交接班记录" style="width:100%;height:auto;padding:10px;">
            <div>
                班次：
                <input id="shifts" class="easyui-combobox" data-options="editable:false,panelHeight:'auto'" style="width:130px;" />
                班组：
                <input id="workingTeam" class="easyui-combobox" data-options="editable:false,panelHeight:'auto'" style="width:80px;" />
                负责人：
                <input id="chargeMan" class="easyui-combobox" data-options="valueField:'StaffID',textField:'Combined',panelHeight:'auto',data:logerData.getStaffInfo()" style="width:180px" />
            </div>
	    </div>
        <div class="easyui-panel" style="width:100%;height:auto;padding:10px;">
            <div>
                <!--停机记录DataGrid-->
	            <table id="haltLoger" class="easyui-datagrid" title="停机原因" style="width:100%;height:auto"
			            data-options="
				            iconCls: 'icon-edit',
				            singleSelect: true,
				            onClickRow: haltLoger.OnClickRow
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
								            data:logerData.getMachineHaltReason(),
                                            onClick: changeHaltReason
							            }
						            }">停机原因</th>
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
				            singleSelect: true,
				            onClickRow: dcsWarningLoger.OnClickRow
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
            </div>
            <div style="margin-top: 20px;">
                <!--能耗报警记录DataGrid-->
	            <table id="ecAlarmLoger" class="easyui-datagrid" title="能耗报警记录" style="width:100%;height:auto"
			            data-options="
				            iconCls: 'icon-edit',
				            singleSelect: true,
				            onClickRow: energyConsumptionAlarmLoger.OnClickRow
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
            <a href="javascript:void(0)" class="easyui-linkbutton easyui-tooltip tooltip-f" data-options="plain:true,iconCls:'icon-ok'" title="提交后不可修改，请谨慎操作。" onclick="submit()">提交</a> | 
            <a href="javascript:void(0)" class="easyui-linkbutton easyui-tooltip tooltip-f" data-options="plain:true,iconCls:'icon-filter'" title="填写盘库信息。可不填写，默认为系统累计。" onclick="stocktaking.OpenDlgStocktaking()">盘库信息</a> | 
            <a href="javascript:void(0)" class="easyui-linkbutton easyui-tooltip tooltip-f" data-options="plain:true,iconCls:'ext-icon-group'" title="填写操作员信息。默认为上次编排，如有变动请修改。" onclick="operatorLoger.OpenDlgOperator()">操作员信息</a>
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
			    data-options="
				    singleSelect: true,
				    onClickRow: operatorLoger.OnClickRow
			    ">
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
				    singleSelect: true,
				    onClickRow: stocktaking.OnClickRow,
                    toolbar: '#tbdgStocktaking'
			    ">
		    <thead>
			    <tr>
                    <th data-options="field:'OrganizationID',hidden:true">OrganizationID</th>
                    <th data-options="field:'VariableId',hidden:true">VariableId</th>
                    <th data-options="field:'OrganizationName',width:120">生产线</th>
				    <th data-options="field:'Name',width:120">物料名称</th>
                    <th data-options="field:'Unit',width:40">单位</th>
				    <th data-options="field:'Data',width:120">系统值</th>
                    <th data-options="field:'DataValue',width:120,editor:{type:'numberbox',options:{min:0,precision:4}}">修正值</th>
                    <th data-options="field:'Remark',width:120,editor:{type:'text'}">备注</th>
			    </tr>
		    </thead>
	    </table>
        <div id="tbdgStocktaking" style="height:auto">
            <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-reload',plain:true" onclick="stocktaking.Refresh()">刷新</a>
		    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-save',plain:true" onclick="stocktaking.Accept()">应用</a>
		    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-undo',plain:true" onclick="stocktaking.Reject()">取消</a>
	    </div>
    </div>
    <!-- 盘库信息对话框结束 -->
	<script type="text/javascript">

	    var validateFunctions = [];

	    var logerData = new LogerData();
	    var shiftsInfo = new ShiftsInfo(logerData.getOrganizationId());
	    var workingTeam = new WorkingTeam(logerData.getOrganizationId());
	    var stocktaking = new Stocktaking(logerData.getOrganizationId(), shiftsInfo);
	    var operatorLoger = new OperatorLoger(logerData.getOrganizationId(), workingTeam);
	    var haltLoger = new HaltLoger(logerData.getOrganizationId(), shiftsInfo);
	    var dcsWarningLoger = new DcsWarningLoger(logerData.getOrganizationId(), shiftsInfo);
	    var energyConsumptionAlarmLoger = new EnergyConsumptionAlarmLoger(logerData.getOrganizationId(), shiftsInfo);

        // 挂载验证函数
	    addValidateFunction(workingTeam.Validate);
	    addValidateFunction(haltLoger.Validate);

	    // 添加验证函数
	    function addValidateFunction(handler) {
	        validateFunctions.push(handler);
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

        // 检验录入是否完整
	    function Validate() {

	        // 检验负责人
	        if ($('#chargeMan').combobox('getText') == "") {
	            $.messager.alert('提示', '请选择负责人。', 'info');
	            return false;
	        }

	        if (validateFunctions.length > 0) {
	            for (var i = 0; i < validateFunctions.length; i++) {
	                if (validateFunctions[i]())
	                    continue;
	                else
	                    return false;
	            }
	        }
	    }

	    // 提交
	    function submit() {

	        // 检验
	        if (Validate() == false)
	            return;

	        $.messager.confirm('确认', '确认提交交接班日志？', function (r) {
	            if (r) {
	                var time = "\"time\":\"" + shiftsInfo.getShiftFullStartTime() + "\"";
	                var shifts = "\"shifts\":\"" + shiftsInfo.getSelectedText() + "\"";
	                var team = "\"workingTeam\":\"" + $('#workingTeam').combobox('getValue') + "\"";
	                var chargeMan = "\"chargeMan\":\"" + $('#chargeMan').combobox('getValue') + "\"";

	                var operators = "\"operators\":" + (JSON.stringify($('#operatorSelector').datagrid('getData')));
	                var haltLogs = "\"haltLogs\":" + (JSON.stringify($('#haltLoger').datagrid('getData')));
	                var dcsWarningLogs = "\"dcsWarningLogs\":" + (JSON.stringify($('#dcsWarningLoger').datagrid('getData')));
	                var ecAlarmLogs = "\"energyConsumptionAlarmLogs\":" + (JSON.stringify($('#ecAlarmLoger').datagrid('getData')));
	                var stocktakingInfos = "\"stocktakingInfos\":" + (JSON.stringify($('#dgStocktaking').datagrid('getData')));

	                var performToObjectives = "\"performToObjectives\":\"" + $('#performToObjectives').val() + "\"";
	                var problemsAndSettlements = "\"problemsAndSettlements\":\"" + $('#problemsAndSettlements').val() + "\"";
	                var equipmentSituation = "\"equipmentSituation\":\"" + $('#equipmentSituation').val() + "\"";
	                var advicesToNextShift = "\"advicesToNextShift\":\"" + $('#advicesToNextShift').val() + "\"";

	                var loger = '{' + time + ',' + shifts + ',' + team + ',' + chargeMan + ',' + operators + ',' + haltLogs + ',' + dcsWarningLogs + ',' + ecAlarmLogs + ',' + stocktakingInfos + ','
                        performToObjectives + ',' + problemsAndSettlements + ',' + equipmentSituation + ',' + advicesToNextShift + '}';

	                var queryUrl = 'HandoverLoger.aspx/CreateWorkingTeamShiftLog';
	                var dataToSend = '{organizationId:"' + logerData.getOrganizationId() + '",json:\'' + loger + '\'}';

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
	                    },
	                    error: function (msg) {
	                        $.messager.alert('提示', '日志创建失败，错误原因：' + jQuery.parseJSON(msg.responseText).Message, 'error');
	                    }
	                });
	            }
	        });
	    }
	</script>
    <form id="form1" runat="server"></form>
</body>
</html>
