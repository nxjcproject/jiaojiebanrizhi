var Stocktaking = function () {
    var that = this;

    that.EditIndex = undefined;

    var _originalStocktakingInfoLoaded = false;
    that.Load = function (organizationId, shift) {
        that.OrganizationId = organizationId;
        that.shift = shift;
        _init
    }
    // 初始化
    function _init() {
        that.shift.attachOnSelectedChanged(_shiftChanged);
    }

    // 班次改变时，重新加载
    function _shiftChanged() {
        if (_originalStocktakingInfoLoaded) {
            $('#dgStocktaking').datagrid({ data: [] });
            _originalStocktakingInfoLoaded = false;
        }
    }

    function _setOriginalStocktakingInfoLoaded(value) {
        _originalStocktakingInfoLoaded = value;
    }

    function _loadOriginalStocktakingInfo() {

        var currentTime = new Date();
        var shiftTime = that.shift.getSelected();
        var getCurrentShiftData = undefined;

        currentTime = currentTime.getHours() + ':' + currentTime.getMinutes();
        // 当前的时间 在 所选的班次的时间段内时，说明需要获取当前的盘库信息，否则系统一律按照上一班处理。
        if (currentTime >= shiftTime.startTime && currentTime < shiftTime.endTime)
            getCurrentShiftData = true;
        else
            getCurrentShiftData = false;

        var queryUrl = 'HandoverLoger.aspx/GetOriginalStocktakingInfoWithDataGridFormat';
        var dataToSend = '{organizationId: "' + that.OrganizationId + '", getCurrentShiftData: "' + getCurrentShiftData + '"}';

        $.ajax({
            type: "POST",
            url: queryUrl,
            data: dataToSend,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                $('#dgStocktaking').datagrid({
                    data: jQuery.parseJSON(msg.d)
                });

                _setOriginalStocktakingInfoLoaded(true);
            }
        });
    }

    that.EndEditing = function () {
        if (that.EditIndex == undefined) { return true }
        if ($('#dgStocktaking').datagrid('validateRow', this.EditIndex)) {
            // 结束编辑
            $('#dgStocktaking').datagrid('endEdit', this.EditIndex);
            that.EditIndex = undefined;
            return true;
        } else {
            return false;
        }
    }

    that.OnClickRow = function (index) {
        if (that.EditIndex != index) {
            if (that.EndEditing()) {
                $('#dgStocktaking').datagrid('selectRow', index)
                        .datagrid('beginEdit', index);
                that.EditIndex = index;
            } else {
                $('#dgStocktaking').datagrid('selectRow', that.EditIndex);
            }
        }
    }

    that.Refresh = function () {
        _loadOriginalStocktakingInfo();
    }

    that.Accept = function () {
        if (that.EndEditing()) {
            $('#dgStocktaking').datagrid('acceptChanges');
        }
    }

    that.Reject = function () {
        $('#dgStocktaking').datagrid('rejectChanges');
        that.EditIndex = undefined;
    }

    that.OpenDlgStocktaking = function() {
        $('#dlgStocktaking').dialog('open');

        if (_originalStocktakingInfoLoaded == false) {
            _loadOriginalStocktakingInfo();
        }
    }

    //_init();
};