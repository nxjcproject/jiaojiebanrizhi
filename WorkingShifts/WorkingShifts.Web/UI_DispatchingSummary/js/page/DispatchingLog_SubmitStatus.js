var MaxColumnsCount = 5;
var MaxCellsCount = 10;
$(document).ready(function () {
    //SetDispatchingStatusPic('');
    //SetDispatchingText('');

    var m_CurrentDate = new Date();
    var m_Year = m_CurrentDate.getFullYear();    //获取完整的年份(4位,1970-????)
    var m_Month = m_CurrentDate.getMonth();       //获取当前月份(0-11,0代表1月)
    var m_Day = m_CurrentDate.getDate();        //获取当前日(1-31)
    var m_MonthString = m_Month >= 9 ? m_Month + 1 : '0' + (m_Month + 1);
    var m_DayString = m_Day >= 10 ? m_Day : '0' + m_Day;
    var m_DateString = m_Year + '-' + m_MonthString + '-' + m_DayString;
    LoadEnergyAlarmData(m_DateString);
});
function SetDispatchingStatusPic(myData) {
    var m_RowIndex = 0;
    var m_ColumnIndex = 0;
    var m_RowIndexString = "";
    var m_ColumnIndexString = "";
    for (var i = 0; i < myData.length; i++) {
        m_RowIndex = Math.floor(i / MaxColumnsCount) + 1;
        m_ColumnIndex = (i % MaxColumnsCount) + 1;
        m_RowIndexString = m_RowIndex.toString();
        m_RowIndexString = m_RowIndexString.length > 1 ? m_RowIndexString : "0" + m_RowIndexString;
        m_ColumnIndexString = m_ColumnIndex.toString();
        m_ColumnIndexString = m_ColumnIndexString.length > 1 ? m_ColumnIndexString : "0" + m_ColumnIndexString;
        if (myData[i].SubmitStatus == '1') {
            $('#Line' + m_RowIndexString + m_ColumnIndexString).css('background-image', 'url(images/page/DispatchingCylinder_blue.png)');
        }
        else {
            $('#Line' + m_RowIndexString + m_ColumnIndexString).css('background-image', 'url(images/page/DispatchingCylinder_gray.png)');
        }
    }
}
function SetDispatchingText(myData) {
    for (var i = 0; i < myData.length; i++) {
        m_RowIndex = Math.floor(i / MaxColumnsCount) + 1;
        m_ColumnIndex = (i % MaxColumnsCount) + 1;
        m_RowIndexString = m_RowIndex.toString();
        m_RowIndexString = m_RowIndexString.length > 1 ? m_RowIndexString : "0" + m_RowIndexString;
        m_ColumnIndexString = m_ColumnIndex.toString();
        m_ColumnIndexString = m_ColumnIndexString.length > 1 ? m_ColumnIndexString : "0" + m_ColumnIndexString;
        $('#Line' + m_RowIndexString + m_ColumnIndexString).text(myData[i].Name);
    }
}

function LoadEnergyAlarmData(myDate) {
    $.ajax({
        type: "POST",
        url: "DispatchingLog_SubmitStatus.aspx/GetDispatchingSubmitStatus",
        data: "{myDate:'" + myDate + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var m_MsgData = jQuery.parseJSON(msg.d);
            if (m_MsgData['rows']) {
                SetDispatchingStatusPic(m_MsgData['rows']);
                SetDispatchingText(m_MsgData['rows']);
            }
        }
    });
}