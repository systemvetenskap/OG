function ValidateCheckBoxList(sender, args) {
    var checkBoxList = document.getElementById("<%=chkFruits.ClientID %>");
    var checkboxes = checkBoxList.getElementsByTagName("input");
    var isValid = false;
    for (var i = 0; i < checkboxes.length; i++) {
        if (checkboxes[i].checked) {
            isValid = true;
            break;
        }
    }
    args.IsValid = isValid;
}
function userValid(elementID, minium) {    
    elementID = "bodyn_" + elementID; //Var tvungen att fixa element namnet
    var atLeast = parseInt(minium);
    var CHK = document.getElementById(elementID);
    var checkbox = CHK.getElementsByTagName("input");
    var counter = 0;
    for (var i = 0; i < checkbox.length; i++) {

        if (checkbox[i].checked) {
            counter++;
        }
    }    
    if (atLeast != counter) {
        alert("Välj " + atLeast + " svar");
        return false;
    }
    return true;
}