function CreateItem_ListNameDropdownClick(div) {
    var id = $(div).attr('id');
    $("#" + id + " :input").removeAttr('disabled');
    $("#" + id + " :radio").prop('checked', true);
    $("#divShoppingListTextbox :input").attr('disabled', true);
    $("#divShoppingListTextbox :radio").prop('checked', false);
    $("#ListName").val('');
}

function CreateItem_ListNameTextboxClick(div) {
    var id = $(div).attr('id');
    $("#" + id + " :input").removeAttr('disabled');
    $("#" + id + " :radio").prop('checked', true);
    $("#divShoppingListDropdown :input").attr('disabled', true);
    $("#divShoppingListDropdown :radio").prop('checked', false);
    $("#ListName").val('');

}

function CreateItem_ListNameOnLeave() {
    if ($("#divShoppingListDropdown :input").attr('disabled')) {
        $("#ListName").val($("#EnteredListName").val());
    } else {
        $("#ListName").val($("#SelectedListName").val());
    }
}

function CreateItem_TagDropdownClick(div) {
    var id = $(div).attr('id');
    $("#" + id + " :input").removeAttr('disabled');
    $("#" + id + " :radio").prop('checked', true);
    $("#divTagsTextbox :input").attr('disabled', true);
    $("#divTagsTextbox :radio").prop('checked', false);
    $("#Tag").val('');
}

function CreateItem_TagTextboxClick(div) {
    var id = $(div).attr('id');
    $("#" + id + " :input").removeAttr('disabled');
    $("#" + id + " :radio").prop('checked', true);
    $("#divTagsDropdown :input").attr('disabled', true);
    $("#divTagsDropdown :radio").prop('checked', false);
    $("#Tag").val('');
}
function CreateItem_TagOnBlur() {
    if ($("#divTagsDropdown :input").attr('disabled')) {
        $("#Tag").val($("#EnteredTag").val());
    } else {
        $("#Tag").val($("#SelectedTag").val());
    }
}

$(document).ready(function() {
    $("#ListName").val('');
    $("#divShoppingListTextbox :input").attr('disabled', true);
    $("#Tag").val('');
    $("#divTagsTextbox :input").attr('disabled', true);

    $("#divShoppingListDropdown :radio").attr('checked', 'checked');
    $("#divTagsDropdown :radio").attr('checked', 'checked');
});
