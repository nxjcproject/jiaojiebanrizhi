<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DispatchingLog_Branch.aspx.cs" Inherits="WorkingShifts.Web.UI_DispatchingSummary.DispatchingLog_Branch" %>

<%@ Register Src="~/UI_WebUserControls/OrganizationSelector/OrganisationTree.ascx" TagPrefix="uc1" TagName="OrganisationTree" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>调度日志</title>
    <link rel="stylesheet" type="text/css" href="/lib/ealib/themes/gray/easyui.css" />
    <link rel="stylesheet" type="text/css" href="/lib/ealib/themes/icon.css" />
    <link rel="stylesheet" type="text/css" href="/lib/extlib/themes/syExtIcon.css" />
    <link rel="stylesheet" type="text/css" href="/lib/extlib/themes/syExtCss.css" />


    <link rel="stylesheet" type="text/css" href="/UI_DispatchingSummary/css/page/DispatchingLog.css" />

    <script type="text/javascript" src="/lib/ealib/jquery.min.js" charset="utf-8"></script>
    <script type="text/javascript" src="/js/common/jquery.utility.js"></script>
    <script type="text/javascript" src="/lib/ealib/jquery.easyui.min.js" charset="utf-8"></script>
    <script type="text/javascript" src="/lib/ealib/easyui-lang-zh_CN.js" charset="utf-8"></script>

    <script type="text/javascript" src="js/page/DispatchingLog_Branch.js"></script>

</head>
<body>
    <div class="easyui-layout" data-options="fit:true,border:false">
        <div data-options="region:'west',border:true,collapsible:false" style="width: 230px;">
            <!-- 左侧目录树开始 -->
            <uc1:OrganisationTree runat="server" ID="OrganisationTree" />
        </div>
        <div data-options="region:'center',border:false,collapsible:false" style="padding-left: 5px;">
            <!-- 中间层交接班记录 -->
            <div class="easyui-layout" data-options="fit:true,border:false">
                <div data-options="region:'center',border:true,collapsible:false" title="交接班记录">
                    <fieldset>
                        <legend>甲班</legend>
                        <table class="table">
                            <tr>
                                <td class="WorkingShift_Text">一、本班运行
                                1、本班生产运行良好，产品质量合格。<br />
                                    2、水泥磨主电机加油<br />
                                    3、更换热电偶<br />
                                    4、卫生已打扫<br />
                                    二、接班工作
                                1、2#磨明天检修，今晚不启动。
                                2、3#水泥仓今晚放空。
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <fieldset>
                        <legend>乙班</legend>
                        <table class="table">
                            <tr>
                                <td class="WorkingShift_Text">一、本班运行
                                1、本班生产运行良好，产品质量合格。<br />
                                    2、水泥磨主电机加油<br />
                                    3、更换2#皮带机拉绳开关<br />
                                    4、卫生已打扫<br />
                                    5、15:34，2#生料磨停机一次，原因是轴承震动过大<br />
                                    二、接班工作<br />
                                    1、2#生料磨磨明天检修，今晚不启动。<br />
                                    2、今晚预报有雨，做好防汛准备。<br />

                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <fieldset>
                        <legend>丙班</legend>
                        <table class="table">
                            <tr>
                                <td class="WorkingShift_Text">一、本班运行
                                1、本班生产运行良好，产品质量合格。<br />
                                    2、水泥磨主电机加油<br />
                                    3、更换热电偶<br />
                                    4、卫生已打扫<br />
                                    二、接班工作<br />
                                    1、2#磨停机检修。<br />
                                    2、4#熟料仓满，需要倒运。<br />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
                <div data-options="region:'south',border:false,collapsible:false" style="padding-top: 5px; height: 300px;">
                    <div id="Panel_DispatchingLog" class="easyui-panel" data-options="fit:true,border:true,tools:'#Tools_DispatchingLog'" title="调度记录">
                        <textarea id="TextArea_DispatchingLog" cols="20" name="S1" rows="2" style="width: 600px; height: 260px;"></textarea>
                    </div>
                    <div id="Tools_DispatchingLog">
                        <a href="javascript:void(0)" class="icon-save" onclick="javascript:SaveDispatchingLog();"></a>
                    </div>
                </div>
            </div>
        </div>
        <div data-options="region:'east',border:false,collapsible:false" style="width: 550px; padding-left: 5px;">
            <!-- 右侧层统计信息 -->
            <div class="easyui-layout" data-options="fit:true,border:false">
                <div data-options="region:'north',border:true,collapsible:false" style="height: 400px;" title="分公司产线能耗">
                    <fieldset>
                        <legend>青铜峡</legend>
                        <table class="table">
                            <tr>
                                <td class="ComprehensiveInformation_Value">熟料产量: 234吨
                                </td>
                                <td class="ComprehensiveInformation_ValueL">计划完成情况: 超10吨
                                </td>
                                <td class="ComprehensiveInformation_Value">耗煤量: 63吨
                                </td>
                            </tr>
                            <tr>
                                <td class="ComprehensiveInformation_Value">计划完成情况: 超7吨
                                </td>
                                <td class="ComprehensiveInformation_ValueL">水泥产量: 211吨
                                </td>
                                <td class="ComprehensiveInformation_Value">计划完成情况: 超10吨
                                </td>
                            </tr>
                            <tr>
                                <td class="ComprehensiveInformation_Value">用电量: 132千瓦时
                                </td>
                                <td class="ComprehensiveInformation_ValueL">计划完成情况: 欠21千瓦时
                                </td>
                                <td class="ComprehensiveInformation_Value">能耗报警次数: 21次
                                </td>
                            </tr>
                            <tr>
                                <td class="ComprehensiveInformation_Value">主机停机次数: 10次
                                </td>
                                <td class="ComprehensiveInformation_ValueL"></td>
                                <td class="ComprehensiveInformation_Value"></td>
                            </tr>
                        </table>
                    </fieldset>
                    <fieldset>
                        <legend>二分厂</legend>
                        <table class="table">
                            <tr>
                                <td class="ComprehensiveInformation_Value">熟料产量: 234吨
                                </td>
                                <td class="ComprehensiveInformation_ValueL">计划完成情况: 超10吨
                                </td>
                                <td class="ComprehensiveInformation_Value">耗煤量: 63吨
                                </td>
                            </tr>
                            <tr>
                                <td class="ComprehensiveInformation_Value">计划完成情况: 超7吨
                                </td>
                                <td class="ComprehensiveInformation_ValueL">水泥产量: 211吨
                                </td>
                                <td class="ComprehensiveInformation_Value">计划完成情况: 超10吨
                                </td>
                            </tr>
                            <tr>
                                <td class="ComprehensiveInformation_Value">用电量: 132千瓦时
                                </td>
                                <td class="ComprehensiveInformation_ValueL">计划完成情况: 欠21千瓦时
                                </td>
                                <td class="ComprehensiveInformation_Value">能耗报警次数: 21次
                                </td>
                            </tr>
                            <tr>
                                <td class="ComprehensiveInformation_Value">主机停机次数: 10次
                                </td>
                                <td class="ComprehensiveInformation_ValueL"></td>
                                <td class="ComprehensiveInformation_Value"></td>
                            </tr>
                        </table>
                    </fieldset>
                    <fieldset>
                        <legend>太阳山分厂</legend>
                        <table class="table">
                            <tr>
                                <td class="ComprehensiveInformation_Value">熟料产量: 234吨
                                </td>
                                <td class="ComprehensiveInformation_ValueL">计划完成情况: 超10吨
                                </td>
                                <td class="ComprehensiveInformation_Value">耗煤量: 63吨
                                </td>
                            </tr>
                            <tr>
                                <td class="ComprehensiveInformation_Value">计划完成情况: 超7吨
                                </td>
                                <td class="ComprehensiveInformation_ValueL">水泥产量: 211吨
                                </td>
                                <td class="ComprehensiveInformation_Value">计划完成情况: 超10吨
                                </td>
                            </tr>
                            <tr>
                                <td class="ComprehensiveInformation_Value">用电量: 132千瓦时
                                </td>
                                <td class="ComprehensiveInformation_ValueL">计划完成情况: 欠21千瓦时
                                </td>
                                <td class="ComprehensiveInformation_Value">能耗报警次数: 21次
                                </td>
                            </tr>
                            <tr>
                                <td class="ComprehensiveInformation_Value">主机停机次数: 10次
                                </td>
                                <td class="ComprehensiveInformation_ValueL"></td>
                                <td class="ComprehensiveInformation_Value"></td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
                <div data-options="region:'center',border:false,collapsible:false" style="padding-top: 5px;">
                    <div class="easyui-panel" data-options="fit:true,border:true" title="产线能耗实绩">
                    </div>
                </div>
            </div>
        </div>
    </div>

    <form id="form1" runat="server"></form>
</body>
</html>
