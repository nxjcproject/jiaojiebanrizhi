<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DispatchingLog_Headquarters.aspx.cs" Inherits="WorkingShifts.Web.UI_DispatchingSummary.DispatchingLog" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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
</head>
<body>
    <div class="easyui-layout" data-options="fit:true,border:false">
        <!-- 左侧目录树开始 -->
        <div data-options="region:'north',border:false,collapsible:false" style="height: 400px;">
            <div id="TagItemsTabs" class="easyui-tabs" data-options="fit:true, tabPosition:'left'">
                <div title="青铜峡" style="padding: 5px;">
                    <div class="easyui-layout" data-options="fit:true,border:false">
                        <div data-options="region:'west',border:true,collapsible:false" style="width: 500px; padding: 10px;" title="能耗信息汇总">
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
                        <div data-options="region:'center',border:false,collapsible:false" style="padding-left: 5px;">
                            <div class="easyui-panel" data-options="fit:true,border:true" title="调度记录" style="font-size: 11pt; padding: 5px;">
                                一、生产运行<br />
                                1、昨天白天到今天夜班，生产运行良好，产品质量合格。<br />
                                2、<br />
                                二、能耗评价<br />
                                1、昨天白天到今天夜班，水泥磨电耗超标主要原因是，主电机过负荷跳闸，初步分析原因堵料或者轴承出现问题。<br />
                                三、设备保障<br />
                                四、安全环保<br />
                                五、其它<br />
                            </div>
                        </div>
                    </div>
                </div>
                <div title="中宁" style="padding: 5px;">
                    <div class="easyui-layout" data-options="fit:true,border:false">
                        <div data-options="region:'west',border:true,collapsible:false" style="width: 500px;" title="能耗信息汇总">
                        </div>
                        <div data-options="region:'center',border:false,collapsible:false" style="padding-left: 5px;">
                            <div class="easyui-panel" data-options="fit:true,border:true" title="调度记录">
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!-- 中央区域开始 -->
        <div data-options="region:'center',border:false,collapsible:false">
            <div class="easyui-layout" data-options="fit:true,border:false">
                <div data-options="region:'west',border:true,collapsible:false" style="width: 500px;" title="分公司产线能耗">
                </div>
                <div data-options="region:'center',border:false,collapsible:false" style="padding-left: 5px;">
                    <div class="easyui-panel" data-options="fit:true,border:true" title="分公司能耗实绩">
                    </div>
                </div>
            </div>
        </div>
    </div>
    <form id="form1" runat="server"></form>
</body>
</html>
