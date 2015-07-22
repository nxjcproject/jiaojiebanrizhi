var DcsWarningLoger = function () {
    var that = this;

    var HTML_ID = '#dcsWarningLoger';

    that.EditIndex = undefined;
    that.Load = function (organizationId, shift) {
        that.OrganizationId = organizationId;
        that.shift = shift;


        _init();
    }
    // 初始化
    function _init() {
        that.shift.attachOnSelectedChanged(_shiftChanged);
        getWarningLog();
    }

    // 班次改变时，重新加载
    function _shiftChanged() {
        getWarningLog();
    }

    that.EndEditing = function () {
        if (that.EditIndex == undefined) { return true }
        $(HTML_ID).datagrid('endEdit', that.EditIndex);
        that.EditIndex = undefined;
        return true;
    }

    that.OnClickRow = function (index) {
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

    that.Accept = function () {
        if (that.EndEditing()) {
            $(HTML_ID).datagrid('acceptChanges');
        }
    }

    that.Reject = function () {
        $(HTML_ID).datagrid('rejectChanges');
        that.EditIndex = undefined;
    }

    // 生成报警信息grid
    function getWarningLog() {
        var queryUrl = 'HandoverLoger.aspx/GetDCSWarningLogWithDataGridFormat';
        //var dataToSend = '{organizationId: "' + organizationId + '"}';
        var dataToSend = '{organizationId: "' + that.OrganizationId + '",startTime:"' + that.shift.getSelected().startTime + '",endTime:"' + that.shift.getSelected().endTime + '"}';
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
        $(HTML_ID).datagrid({
            data: json
        });
    }

    //_init();

}