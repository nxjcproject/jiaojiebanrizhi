var LogerData = function () {
    var that = this;

    var HTML_ID = '#haltLoger';
    // 分厂组织机构ID
    var _organizationId = undefined;
    // 员工信息
    var _staffInfo = undefined;
    // 停机原因
    var _haltReasons = undefined;

    that.getOrganizationId = function () {
        return _organizationId;
    }

    that.getStaffInfo = function () {
        return _staffInfo;
    }

    that.getMachineHaltReason = function () {
        return _haltReasons;
    }

    function _setOrganizationId(value) {
        _organizationId = value;
    }

    function _setStaffInfo(value) {
        _staffInfo = value;
    }

    function _setHaltReasons(value) {
        _haltReasons = value;
    }

    // 初始化组织机构ID
    function _initOrganizationId() {
        var queryUrl = 'HandoverLoger.aspx/GetAppSettingValue';

        $.ajax({
            type: "POST",
            url: queryUrl,
            data: '',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (msg) {
                _setOrganizationId(msg.d);
            }
        });
    }

    // 初始化员工信息
    function _initStaffInfo() {
        var queryUrl = 'HandoverLoger.aspx/GetStaffInfoWithComboboxFormat';
        var dataToSend = '{organizationId: "' + _organizationId + '"}';

        $.ajax({
            type: "POST",
            url: queryUrl,
            data: dataToSend,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (msg) {
                _setStaffInfo(jQuery.parseJSON(msg.d));
            }
        });
    }

    // 初始化停机原因
    function _initMachineHaltReasons() {
        var queryUrl = 'HandoverLoger.aspx/GetMachineHaltReasonsWithCombotreeFormat';
        var dataToSend = '';

        $.ajax({
            type: "POST",
            url: queryUrl,
            data: dataToSend,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                _setHaltReasons(jQuery.parseJSON(msg.d));
                $(that).trigger("LogerDataLoadComplate");
            }
        });
    }

    // 注意！此处的ajax必须为串行！
    _initOrganizationId();
    _initStaffInfo();
    _initMachineHaltReasons();
}