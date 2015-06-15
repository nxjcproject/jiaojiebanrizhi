var OperatorLoger = function (organizationId, workingTeam) {
    var that = this;

    var HTML_ID = '#operatorSelector';

    that.OrganizationId = organizationId;
    that.WorkingTeam = workingTeam;
    that.EditIndex = undefined;

    var _datagridColumns = undefined;

    // 初始化
    function _init() {
        that.WorkingTeam.attachOnSelectedChanged(_workingTeamChanged);
        getDataGridColumns();

        $(document).ready(function () {
            getLastOperatorsLog();
        });
    }

    // 班次改变时，重新加载
    function _workingTeamChanged() {
        getLastOperatorsLog();
    }

    that.EndEditing = function () {
        if (that.EditIndex == undefined) { return true }
        if ($(HTML_ID).datagrid('validateRow', that.EditIndex)) {
            // 获取操作员选择编辑器中的工段列
            var cols = $(HTML_ID).datagrid('getColumnFields');
            // 除去第1列是DCS名称，遍历工段列，每个工段有两列
            for (var i = 2; i < cols.length; i += 2) {
                // cols[i]: StaffName_xxx
                // cols[i+1]: StaffID_xxx
                var ed = $(HTML_ID).datagrid('getEditor', { index: that.EditIndex, field: cols[i + 1] });
                var staffId = $(ed.target).combobox('getValue');
                var staffName = $(ed.target).combobox('getText');
                $(HTML_ID).datagrid('getRows')[that.EditIndex][cols[i + 1]] = staffId;
                $(HTML_ID).datagrid('getRows')[that.EditIndex][cols[i]] = staffName;
            }
            // 结束编辑
            $(HTML_ID).datagrid('endEdit', that.EditIndex);
            that.EditIndex = undefined;
            return true;
        } else {
            return false;
        }
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

    that.OpenDlgOperator = function () {
        if (that.WorkingTeam.getSelected() == "") {
            $.messager.alert('提示', '请先选择当前班组。', 'info');
            return;
        }
        $('#dlgOperator').dialog('open');
    }

    // 获取操作员DATAGRID列
    function getDataGridColumns() {
        var queryUrl = 'HandoverLoger.aspx/GetWorkingSectionsWithDataColumnFormat';
        var dataToSend = '{organizationId: "' + that.OrganizationId + '"}';

        $.ajax({
            type: "POST",
            url: queryUrl,
            data: dataToSend,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                _datagridColumns = msg.d;
                initializeColumns();
            }
        });
    }

    // 初始化操作员DATAGRID列
    function initializeColumns() {
        $(HTML_ID).datagrid({
            columns: eval(_datagridColumns)
        });
    }

    // 获取操作员记录
    function getLastOperatorsLog() {
        var queryUrl = 'HandoverLoger.aspx/GetLastOperatorsLog';
        var dataToSend = '{organizationId: "' + that.OrganizationId + '",workingTeam:"' + that.WorkingTeam.getSelected() + '"}';

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

    // 初始化操作员选择器
    function initializeStaffSelector(json) {
        // 初始化时，编辑INDEX需要重置
        that.EditIndex = undefined;
        // 清空数据
        $(HTML_ID).datagrid({
            data: []
        });
        $(HTML_ID).datagrid({
            data: json
        });
    }

    _init();
}