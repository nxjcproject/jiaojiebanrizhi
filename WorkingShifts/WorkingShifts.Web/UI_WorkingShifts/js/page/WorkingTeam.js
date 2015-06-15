var WorkingTeam = function (organizationId) {
    var that = this;

    var HTML_ID = '#workingTeam';

    that.OrganizationId = organizationId;

    // 班组信息
    var _workingTeams = undefined;

    // 初始化
    function _init() {
        _initWorkingTeam();

        $(document).ready(function () {
            _initWorkingTeamSelector();
        });
    }

    that.getWorkingTeams = function () {
        return _workingTeams;
    }

    function _setWorkingTeams(value) {
        _workingTeams = value;
    }

    // 获取选中的班组
    that.getSelected = function () {
        var selected = $(HTML_ID).combobox('getValue');
        return selected;
    }

    // 选择变化时，触发事件
    function selectedChanged(rec) {
        if (that.onSelectedChanged) {
            for (var i = 0; i < that.onSelectedChanged.length; i++) {
                that.onSelectedChanged[i]();
            }
        }
    }

    // 挂载事件
    that.attachOnSelectedChanged = function (handler) {
        if (!that.onSelectedChanged) { that.onSelectedChanged = []; }
        that.onSelectedChanged.push(handler);
    }

    // 初始化班组下拉列表数据源
    function _initWorkingTeam() {
        var queryUrl = 'HandoverLoger.aspx/GetWorkingTeamWithComboboxFormat';
        var dataToSend = '{organizationId: "' + that.OrganizationId + '"}';

        $.ajax({
            type: "POST",
            url: queryUrl,
            data: dataToSend,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (msg) {
                _setWorkingTeams(jQuery.parseJSON(msg.d));
            }
        });
    }

    // 初始化班组下拉选择框
    function _initWorkingTeamSelector() {
        $(HTML_ID).combobox({
            valueField: 'Name',
            textField: 'Name',
            onSelect: selectedChanged,
            data: _workingTeams
        });
    }

    that.Validate = function () {
        // 检验班组
        if ($('#workingTeam').combobox('getText') == "") {
            $.messager.alert('提示', '请选择当前班组。', 'info');
            return false;
        }
        return true;
    }

    _init();
}