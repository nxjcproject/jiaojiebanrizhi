// 班次信息
var ShiftsInfo = function () {
    var that = this;
    // 分厂组织机构ID
    var _organizationId;

    // 班次信息
    var _shifts = undefined;

    // 打开交接班登记界面的时间
    var _logginTime = undefined;
    that.Load = function (organizationId) {
        _organizationId = organizationId;
        _init();
    }
    // 初始化
    function _init() {
        _logginTime = new Date();
        _initShifts();


    }

    // 班次信息访问器
    that.getShiftsInfo = function () {
        return _shifts;
    }

    // 班次起始完整时间访问器
    that.getShiftFullStartTime = function () {
        var time = new Date();
        var shiftType = $('#shifts').combobox('getText').substring(0, 3);
        var shiftValue = that.getSelected();
        
        if (shiftType == "上一班" && shiftValue.startTime > shiftValue.endTime) {
            time.setDate(time.getDate() - 1);
        }

        return _getyyyymmdd(time) + " " + shiftValue.startTime;
    }

    // 班次结束完整时间访问器
    that.getShiftFullEndTime = function () {
        var time = new Date();
        var shiftValue = that.getSelected();

        return _getyyyymmdd(time) + " " + shiftValue.endTime;
    }

    // 获取选中的班次
    that.getSelected = function () {
        var selected = $('#shifts').combobox('getValue');
        return _shifts[selected];
    }

    // 获取选中的班次文字
    that.getSelectedText = function () {
        return $('#shifts').combobox('getValue');
    }

    // 班次信息设置器（私有）
    function _setShifts(value) {
        _shifts = value;
        // 测试用
        // _shifts.甲班.startTime = '23:00';
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

    // 初始化班次信息
    function _initShifts() {
        var queryUrl = 'HandoverLoger.aspx/GetShiftsInfo';
        var dataToSend = '{organizationId: "' + _organizationId + '"}';

        $.ajax({
            type: "POST",
            url: queryUrl,
            data: dataToSend,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (msg) {
                _setShifts(jQuery.parseJSON(msg.d));
                _initShiftsSelector();
                $(that).trigger("shiftsInfoLoadComplate");
            }
        });
    }

    // 初始化班次下拉选择框
    function _initShiftsSelector() {
        var currentShift = _getCurrentShift();
        var lastShift = _getLastShift();

        $('#shifts').combobox({
            valueField: 'value',
            textField: 'text',
            onSelect: selectedChanged,
            data: [{
                value: lastShift,
                text: '上一班（' + _getDescription(lastShift) + '）'
            }, {
                value: currentShift,
                text: '当前班（' + _getDescription(currentShift) + '）',
                "selected": true
            }]
        });
    }

    // 根据打开界面时间获取当前班组
    function _getCurrentShift() {
        var currentTime = _getHHMM(_logginTime);

        if (currentTime >= _shifts.甲班.startTime && currentTime < _shifts.甲班.endTime)
            return '甲班';
        else if (currentTime >= _shifts.乙班.startTime && currentTime < _shifts.乙班.endTime)
            return '乙班';
        else if (currentTime >= _shifts.丙班.startTime && currentTime < _shifts.丙班.endTime)
            return '丙班';
    }

    // 根据打开界面时间获取上一班组
    function _getLastShift() {
        var currentShift = _getCurrentShift();

        switch (currentShift) {
            case '甲班':
                return '丙班';
            case '乙班':
                return '甲班';
            case '丙班':
                return '乙班';
        }
    }

    // 根据甲方需求，甲乙丙分别对应夜中白
    function _getDescription(shiftName) {
        switch (shiftName) {
            case '甲班':
                return '夜班';
            case '乙班':
                return '白班';
            case '丙班':
                return '中班';
            default:
                return shiftName;
        }
    }

    // 返回 hh:mm
    function _getHHMM(time) {
        var h = time.getHours();
        var m = time.getMinutes();

        return (h < 10 ? ('0' + h) : h) + '' + (m < 10 ? ('0' + m) : m);
    }

    // 返回 yyyy-MM-dd
    function _getyyyymmdd(time) {
        var y = time.getFullYear();
        var m = time.getMonth() + 1;
        var d = time.getDate();

        return y + '-' + (m < 10 ? ('0' + m) : m) + '-' + (d < 10 ? ('0' + d) : d);
    }

    //_init();
}