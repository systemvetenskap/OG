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
function timeToEnd(starttime, duration, proc) //Starttiden och sedan hur långt, till sist procenten om personen har ett handikapp och behöver längre tid
{
    //var stTim = new Date.parse(starttime);
    //var durAt = parseInt(duration); //Är i millisekunder
    //var start = stTim.getMilliseconds();
    //stTim.getTime();

}
function wantToCont()
{
    if (confirm("Är du säker på att du vill lämna in testet? \n det går inte att ångra!")) {
        return true; //Om man vill lämna in det får man välja att skicka in en true
    }
    else {
        return false; //Vill maninte lämmna in det retunera false;
    }

}
