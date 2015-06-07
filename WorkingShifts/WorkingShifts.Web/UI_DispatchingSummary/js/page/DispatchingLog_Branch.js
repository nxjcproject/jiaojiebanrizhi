$(document).ready(function () {
    $('#Panel_DispatchingLog').panel({
        onResize: function (width, height) {
            $('#TextArea_DispatchingLog').css('width', width-8);
        }
    });
});
function SaveDispatchingLog() {
    alert('保存日志');
}