<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HandoverLog.aspx.cs" Inherits="WorkingShifts.Web.UI_WorkingShifts.HandoverLog" %>
<%@ Register Src="~/UI_WebUserControls/OrganizationSelector/OrganisationTree.ascx" TagPrefix="uc1" TagName="OrganisationTree" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>交接班日志查询</title>
    <link rel="stylesheet" type="text/css" href="/lib/ealib/themes/gray/easyui.css"/>
	<link rel="stylesheet" type="text/css" href="/lib/ealib/themes/icon.css"/>
    <link rel="stylesheet" type="text/css" href="/lib/extlib/themes/syExtIcon.css"/>
    <link rel="stylesheet" type="text/css" href="/lib/extlib/themes/syExtCss.css"/>

	<script type="text/javascript" src="/lib/ealib/jquery.min.js" charset="utf-8"></script>
    <script type="text/javascript" src="/js/common/jquery.utility.js"></script>
	<script type="text/javascript" src="/lib/ealib/jquery.easyui.min.js" charset="utf-8"></script>
    <script type="text/javascript" src="/lib/ealib/easyui-lang-zh_CN.js" charset="utf-8"></script>
</head>
<body>
    <div class="easyui-layout" data-options="fit:true,border:false" style="padding: 5px;">
        <!-- 左侧目录树开始 -->
        <div data-options="region:'west',border:false,collapsible:false" style="width: 230px;">
            <uc1:OrganisationTree runat="server" ID="OrganisationTree" />
        </div>
        <!-- 左侧目录树结束 -->
        <!-- 中央区域开始 -->
        <div data-options="region:'center',border:false,collapsible:false" style="padding-left:10px;">
            <!-- 工具栏开始 -->
            <div id="toolbar_HandoverLog" style="display: none;">
                <table>
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td>当前分厂：</td>
                                    <td><input id="organizationName" class="easyui-textbox" readonly="readonly" style="width:100px" /></td>
                                    <td style="width:10px;"></td>
                                    <td>开始：</td>
                                    <td><input id="startTime" class="easyui-datebox" style="width:100px" /></td>
                                    <td style="width:10px;"></td>
                                    <td>结束：</td>
                                    <td><input id="endTime" class="easyui-datebox" style="width:100px" /></td>
                                    <td style="width:10px;"></td>
                                    <td><a href="#" class="easyui-linkbutton" data-options="iconCls:'icon-search'" onclick="getShiftLogs()">查询</a></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
            <!-- 工具栏结束 -->
            <table id="dgHandoverLog" class="easyui-datagrid" data-options="idField:'WorkingTeamShiftLogID',toolbar:'#toolbar_HandoverLog',rownumbers:true,singleSelect:true,pagination:true,pageSize:10"" title="" style="width:100%;height:100%">
		        <thead>
			        <tr>
                        <th data-options="field:'OperateColumn',formatter:formatOperateColumn,width:60">操作</th>
                        <th data-options="field:'WorkingTeamShiftLogID',hidden:true">交接班日志ID</th>
                        <th data-options="field:'ShiftDate',width:150">交接班时间</th>
				        <th data-options="field:'Shifts',width:60">班次</th>
                        <th data-options="field:'WorkingTeam',width:60">班组</th>
				        <th data-options="field:'PerformToObjectives',width:200">本班生产计划完成情况</th>
                        <th data-options="field:'ProblemsAndSettlements',width:200">本班出现的问题及处理情况</th>
                        <th data-options="field:'EquipmentSituation',width:200">本班设备运行情况</th>
                        <th data-options="field:'AdvicesToNextShift',width:200">下班工作重点及建议</th>
			        </tr>
		        </thead>
            </table>
        </div>
        <!-- 中央区域结束 -->
    </div>
    <form id="form1" runat="server"></form>
    <script type="text/javascript">

        function formatOperateColumn(val, row) {
            return '<a href="HandoverLogDetail.aspx?organizationId=' + organizationId.trim() + '&workingTeamShiftLogId=' + row.WorkingTeamShiftLogID + '">查看</a>';
        }

        // 分厂ID变量
        var organizationId = '';

        // 目录树选择事件
        function onOrganisationTreeClick(node) {
            // 仅分厂级目录有效
            if (node.id.length != 5) {
                $.messager.alert('说明', '请选择分厂级别的节点。', 'info');
                return;
            }

            // 更新当前分厂名
            $('#organizationName').textbox('setText', node.text);
            organizationId = node.OrganizationId;

            // 获取职工信息
            loadStaffInfo();
        }

        // 获取交接班日志记录
        function getShiftLogs() {
            var startTime = $('#startTime').datetimebox('getValue');
            var endTime = $('#endTime').datetimebox('getValue');
            var queryUrl = 'HandoverLog.aspx/GetWorkingTeamShiftLogsWithDataGridFormat';
            var dataToSend = '{organizationId: "' + organizationId + '",startTime: "' + startTime + '",endTime: "' + endTime + '"}';

            $.ajax({
                type: "POST",
                url: queryUrl,
                data: dataToSend,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    initializeHandoverLog(jQuery.parseJSON(msg.d));
                }
            });
        }

        function initializeHandoverLog(json) {
            $('#dgHandoverLog').datagrid({
                data: json
            });
        }

        $(document).ready(function () {
            //设置默认为十天的时间
            var startDate = new Date();
            var endDate = new Date();
            startDate.setDate(startDate.getDate() - 10);
            endDate.setDate(endDate.getDate() + 1);
            var startDateStr = startDate.getFullYear().toString() + '-' + (startDate.getMonth() + 1).toString() + '-' + startDate.getDate();
            var endDateStr = endDate.getFullYear().toString() + '-' + (endDate.getMonth() + 1).toString() + '-' + endDate.getDate();
            $('#startTime').datebox('setValue', startDateStr);
            $('#endTime').datebox('setValue', endDateStr);
        });
    </script>
</body>
</html>
