var startingTime;
var inte;
function CallHandler(stime) {
    startingTime = stime;
    inte = setInterval(upda, 200);

}
function upda() {
    $.ajax({
        url: "webbtesttime.ashx",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: { 'time': startingTime , 'type' : "tim"},
        responseType: "json",
        success: OnComplete,
        error: OnFail
    });
}
function OnComplete(result) {
    if (result.status == "Good")
    {
        document.getElementById("timeLeft").innerHTML = result.timeLeft;
        document.getElementById("timeLeft").style.background = "white";
    }
    else if (result.status == "Bad") {
        document.getElementById("timeLeft").innerHTML = result.timeLeft;
        document.getElementById("timeLeft").style.border = "1px solid red";
    }
    else
    {
        alert("Du drog över tiden på provet och blev automatiskt underkänd.");
        document.getElementById("bodyn_btnNext").value = "Lämna in";
        document.getElementById("bodyn_btnNext").innerHTML = "Lämna in";
        window.location.replace("webbtest.aspx");
        document.getElementById("timeLeft").innerHTML = result.timeLeft;

    }
}
function OnFail(result) {
    inte = null;
    //alert('Request Failed');
}