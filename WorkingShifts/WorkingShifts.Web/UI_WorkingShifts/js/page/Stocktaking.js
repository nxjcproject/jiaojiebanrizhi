var Stocktaking = {
    OriginalStocktakingInfoLoaded: false,
    EditIndex: undefined
};

function OpenDlgStocktaking() {
    $('#dlgStocktaking').dialog('open');

    if (Stocktaking.OriginalStocktakingInfoLoaded == false) {
        LoadOriginalStocktakingInfo();
    }
}

function LoadOriginalStocktakingInfo() {
    var queryUrl = 'HandoverLoger.aspx/GetOriginalStocktakingInfoWithDataGridFormat';
    var dataToSend = '{organizationId: "' + organizationId + '"}';

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

            Stocktaking.OriginalStocktakingInfoLoaded = true;
        }
    });
}

function stEndEditing() {
    if (Stocktaking.EditIndex == undefined) { return true }
    if ($('#dgStocktaking').datagrid('validateRow', Stocktaking.EditIndex)) {
        // 结束编辑
        $('#dgStocktaking').datagrid('endEdit', Stocktaking.EditIndex);
        Stocktaking.EditIndex = undefined;
        return true;
    } else {
        return false;
    }
}

function stOnClickRow(index) {
    if (Stocktaking.EditIndex != index) {
        if (stEndEditing()) {
            $('#dgStocktaking').datagrid('selectRow', index)
                    .datagrid('beginEdit', index);
            Stocktaking.EditIndex = index;
        } else {
            $('#dgStocktaking').datagrid('selectRow', Stocktaking.EditIndex);
        }
    }
}

function stAccept() {
    if (stEndEditing()) {
        $('#dgStocktaking').datagrid('acceptChanges');
    }
}
function stReject() {
    $('#dgStocktaking').datagrid('rejectChanges');
    Stocktaking.EditIndex = undefined;
}