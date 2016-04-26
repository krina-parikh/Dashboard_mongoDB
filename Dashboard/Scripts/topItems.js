
function topItem(flag) {
    alert(flag);
    var lnk = "";
    
    if (flag == "False") {
        lnk = "Home/withOutCategory";
    }
    else {
        lnk = "Home/withCategory";
    }
    $.ajax({
        url: lnk,
        type: "get",
        contentType: 'application/json',
        context: 'table',
        success: function (data) {
            display(data);
        },
        error: function (data) { alert("error"); }
    });
}


function display(data) {
    alert("Display method");
    if (data.length <= 0) {
        jdata = '<tr></tr>' + '<tr><td></td></tr>' + '<tr><td> No Data </td></tr>';
        $('#topGrid').append(jdata);
    }
    else {
        $('#topGrid').empty();
        var jdata = '<tr><td>' + "CATEGORY" + '</td><td>' + "ITEM" + '</td><td>' + "Total Quantity" + '</td><td>' + "SALES" + '</td></tr>';
        $('#topGrid').append(jdata);
        $.each(data, function (key, value) {
            jdata = '<tr><td>' + value.CATEGORYNAME + '</td><td> ' + value.ITEMNAME + '</td><td> ' + value.TOTALQuantity + '</td><td> ' + value.SALEs + '</td></tr>';
            $('#topGrid').append(jdata);
        });
    }
}