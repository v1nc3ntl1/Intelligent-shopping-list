$(Document).ready(function() {
    $("#EnteredBrand").attr('disabled', true);
});

function BrandTextBoxOnMouseClick() {
    $("#EnteredBrand").removeAttr('disabled');
    $("#SelectedBrand").attr('disabled', true);
    $("#Brand").val('');
}

function BrandDropdownOnMouseClick() {
    $("#SelectedBrand").removeAttr('disabled');
    $("#EnteredBrand").attr('disabled', true);
    $("#Brand").val('');
}

function onBlur(node) {
    var nodeOnClicked = $(node);
    $("#Brand").val(nodeOnClicked.val());
}