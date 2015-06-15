$(Document).ready(function () {
    $("#EnteredTag").attr('disabled', true);
});

function TagTextBoxOnMouseClick() {
    $("#EnteredTag").removeAttr('disabled');
    $("#SelectedTag").attr('disabled', true);
    $("#Tag").val('');
}

function TagDropdownOnMouseClick() {
    $("#SelectedTag").removeAttr('disabled');
    $("#EnteredTag").attr('disabled', true);
    $("#Tag").val('');
}

function onBlur(node) {
    var nodeOnClicked = $(node);
    $("#Tag").val(nodeOnClicked.val());
}