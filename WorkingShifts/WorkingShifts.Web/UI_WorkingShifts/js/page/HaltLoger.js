var HaltLoger = function (organizationId, shift) {
    var that = this;

    var HTML_ID = '#haltLoger';

    that.OrganizationId = organizationId;
    that.Shift = shift;
    that.EditIndex = undefined;

    // 初始化
    function _init() {
        that.Shift.attachOnSelectedChanged(_shiftChanged);

        $(document).ready(function () {
            getHaltLog();
        });
    }

    // 班次改变时，重新加载
    function _shiftChanged() {
        getHaltLog();
    }

    that.EndEditing = function () {
        if (that.EditIndex == undefined) { return true }
        if ($(HTML_ID).datagrid('validateRow', that.EditIndex)) {
            var ed = $(HTML_ID).datagrid('getEditor', { index: that.EditIndex, field: 'Reason' });
            var reasonId = $(ed.target).combobox('getValue');
            var reasonText = $(ed.target).combobox('getText');
            $(HTML_ID).datagrid('getRows')[that.EditIndex]['ReasonID'] = reasonId;
            $(HTML_ID).datagrid('getRows')[that.EditIndex]['ReasonText'] = reasonText;
            $(HTML_ID).datagrid('endEdit', that.EditIndex);
            that.EditIndex = undefined;
            return true;
        } else {
            return false;
        }
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
        if (hlEndEditing()) {
            $(HTML_ID).datagrid('acceptChanges');
        }
    }

    that.Reject = function () {
        $(HTML_ID).datagrid('rejectChanges');
        that.EditIndex = undefined;
    }

    // 获取停机原因选择
    function getHaltLog() {
        var queryUrl = 'HandoverLoger.aspx/GetMachineHaltLogWithDataGridFormat';
        var dataToSend = '{organizationId: "' + that.OrganizationId + '",startTime:"' + that.Shift.getShiftFullStartTime() + '",endTime:"' + that.Shift.getShiftFullEndTime() + '"}';

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
        $(HTML_ID).datagrid({
            data: json
        });
    }

    that.Validate = function () {
        // 检验停机原因
        var haltLogs = $(HTML_ID).datagrid('getData');
        for (var i = 0; i < haltLogs.total; i++) {
            if (haltLogs.rows[i].ReasonID.length != 7) {
                $.messager.alert('提示', '请为停机时间： ' + haltLogs.rows[i].HaltTime + ' 的记录选择明确的停机原因', 'info');
                return false;
            }
        }

        return true;
    }

    _init();
}