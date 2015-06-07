<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DispatchingLog_SubmitStatus.aspx.cs" Inherits="WorkingShifts.Web.UI_DispatchingSummary.DispatchingLog_SubmitStatus" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>调度日志提交状态</title>
    <link rel="stylesheet" type="text/css" href="/lib/ealib/themes/gray/easyui.css" />
    <link rel="stylesheet" type="text/css" href="/lib/ealib/themes/icon.css" />
    <link rel="stylesheet" type="text/css" href="/lib/extlib/themes/syExtIcon.css" />
    <link rel="stylesheet" type="text/css" href="/lib/extlib/themes/syExtCss.css" />


    <link rel="stylesheet" type="text/css" href="css/page/DispatchingLog.css" />

    <script type="text/javascript" src="/lib/ealib/jquery.min.js" charset="utf-8"></script>
    <script type="text/javascript" src="/js/common/jquery.utility.js"></script>
    <script type="text/javascript" src="/lib/ealib/jquery.easyui.min.js" charset="utf-8"></script>
    <script type="text/javascript" src="/lib/ealib/easyui-lang-zh_CN.js" charset="utf-8"></script>

    <script type="text/javascript" src="js/page/DispatchingLog_SubmitStatus.js"></script>
</head>
<body>
    <div class="easyui-layout" data-options="fit:true,border:false">
        <div data-options="region:'center',border:false,collapsible:false" style="padding:10px;">
            <table>
                <tr>
                    <td id ="Line0101" class ="DispatchingLogStatusPic"></td>
                    <td class ="DispatchingLogHorizontalBlank"></td>
                    <td id ="Line0102" class ="DispatchingLogStatusPic"></td>
                    <td class ="DispatchingLogHorizontalBlank"></td>
                    <td id ="Line0103" class ="DispatchingLogStatusPic"></td>
                    <td class ="DispatchingLogHorizontalBlank"></td>
                    <td id ="Line0104" class ="DispatchingLogStatusPic"></td>
                    <td class ="DispatchingLogHorizontalBlank"></td>
                    <td id ="Line0105" class ="DispatchingLogStatusPic"></td>
                </tr>
                <tr>
                    <td class ="DispatchingLogverticalBlank"></td>
                    <td></td>
                    <td class ="DispatchingLogverticalBlank"></td>
                    <td></td>
                    <td class ="DispatchingLogverticalBlank"></td>
                    <td></td>
                    <td class ="DispatchingLogverticalBlank"></td>
                    <td></td>
                    <td class ="DispatchingLogverticalBlank"></td>
                </tr>
                <tr>
                    <td id ="Line0201" class ="DispatchingLogStatusPic"></td>
                    <td class ="DispatchingLogHorizontalBlank"></td>
                    <td id ="Line0202" class ="DispatchingLogStatusPic"></td>
                    <td class ="DispatchingLogHorizontalBlank"></td>
                    <td id ="Line0203" class ="DispatchingLogStatusPic"></td>
                    <td class ="DispatchingLogHorizontalBlank"></td>
                    <td id ="Line0204" class ="DispatchingLogStatusPic"></td>
                    <td class ="DispatchingLogHorizontalBlank"></td>
                    <td id ="Line0205" class ="DispatchingLogStatusPic"></td>
                </tr>
            </table>
        </div>
    </div>
    <form id="form1" runat="server"></form>
</body>
</html>
