function CallHandler()
{
  /*  $.ajax({
        url: "webbtesttime.ashx",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: { 'time': '10:00:00' },
        responseType: "json",
        success: OnComplete,
        error: OnFail
    }); */
    alert("JEJ");
}
function OnComplete(result) {
    alert([result.timeLeft]);
}
function OnFail(result) {
    alert('Request Failed');
}