var EnergyConsumptionAlarmLoger = function (organizationId, shift) {
    var that = this;

    var HTML_ID = '#ecAlarmLoger';

    that.OrganizationId = organizationId;
    that.Shift = shift;
    that.EditIndex = undefined;

    // 初始化
    function _init() {
        that.Shift.attachOnSelectedChanged(_shiftChanged);
    }

    // 班次改变时，重新加载
    function _shiftChanged() {
        getEnergyConsumptionAlarmLog();
    }

    that.EndEditing = function () {
        if (that.EditIndex == undefined) { return true }
        $(HTML_ID).datagrid('endEdit', that.EditIndex);
        that.EditIndex = undefined;
        return true;
    }

    that.OnClickRow = function(index) {
        if (that.EditIndex != index) {
            if (that.EndEditing()) {
                $(HTML_ID).datagrid('selectRow', index)
                        .datagrid('beginEdit', index);
                that.EditIndex = index;
            } else {
                $(HTML_ID).datagrid('selectRow', that.EditIndex);
            }
        }
    }

    that.Accept = function() {
        if (that.EndEditing()) {
            $(HTML_ID).datagrid('acceptChanges');
        }
    }

    that.Reject = function() {
        $(HTML_ID).datagrid('rejectChanges');
        that.EditIndex = undefined;
    }

    // 生成能耗报警信息
    function getEnergyConsumptionAlarmLog() {
        var queryUrl = 'HandoverLoger.aspx/GetEnergyConsumptionAlarmLogWithDataGridFormat';
        var dataToSend = '{organizationId: "' + that.OrganizationId + '",startTime:"' + shift.getSelected().startTime + '",endTime:"' + shift.getSelected().endTime + '"}';
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
        $(HTML_ID).datagrid({
            data: json
        });
    }

    _init();

    $(document).ready(function () {
        getEnergyConsumptionAlarmLog();
    });
}