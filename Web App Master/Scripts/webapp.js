document.ASSET = null;
document.currentAsset = "0000";
document.currentBarcodeSequence = "";
document.lastKeypress = new Date();
var Monitor = function (e) {
    //technically a logger but we will not store all strokes...just capture superfast input and escape key

    //sequenceLimitMs should be set as low as possible to prevent capture of human keyed numbers.
    //200 allow testing without a barcode scanner, you could try a value of 50 with a scanner.
    var sequenceLimitMs = 50;
    var now = new Date();
    var elapsed = now - document.lastKeypress;
    document.lastKeypress = now;
    //capture escape
   
    lastkey = e.keyCode;
    var keysArray = getNumberArray(keys);
    //CTRL_ENTER IS HOOKED
    if (keysArray.toString() == "13,17") {
        try {
            var url = document.location.href;
            try {
                url = url.substr(url.lastIndexOf('/') + 1);
            } catch (er) {
                url = url.substr(url.lastIndexOf('/'));
            }
            if (url.startsWith("CheckIn")) {
                var hr = $("#Finalize").attr('href');
                window.location.href = hr;
            }
            if (url.startsWith("Login")) {
                $("LoginBtn").click();
            }
            if (url.startsWith("OutCart")) {
                var newpath = document.location.href;
                newpath = newpath.replace(url, "");
                newpath = newpath + "CheckOut.aspx";
                window.location.href = newpath;
            }
        } catch (err) { }
        keys = [];
    }
    setTimeout(TimedKeyUp, 1000);

    //ENTER IS HOOKED
    if (e.charCode == 13)
    {
        var barcodeHasFocus = $('#BarcodeSearchBox').is(':focus');
        if (barcodeHasFocus == true) {
            BarcodeScanned($('#BarcodeSearchBox').val());
            return false;
        } 

        var url = document.location.href;
        url = url.substr(url.lastIndexOf('/') + 1);
       
        
        if (url.startsWith("CheckOut")) {
            var hr = $("#Finalize").attr('href');
            window.location.href = hr;
        }

        if (url.startsWith("CheckIn")) {
            var hr = $("#Finalize").attr('href');
            window.location.href = hr;
        }
        if (url.startsWith("AssetView"))
        {
            
        }
        if (url.startsWith("Login"))
        {
            $("LoginBtn").click();
        }
        return false;
    }
    //EESCAPE IS HOOKED
    if (e.charCode == 27) {
        var url = document.location.href;
        url = url.substr(url.lastIndexOf('/') + 1);
        if (url.startsWith("Checkout")) {
           
            //window.location.href = "/Account/AssetView.aspx";
        }
        hideMetroCharm('#assets');
        hideMetroCharm('#settings-charm');
        hideMetroCharm('#assetview-charm');
        HideLoader();
       
    }
    //only 0-9 e.charCode >= 48 && e.charCode <= 57
    var charStr = String.fromCharCode(e.charCode);
    var tt = (e.charCode - 48);
    if (/[a-z0-9A-Z]/i.test(charStr)) {
        //pressed key is a number
        if (elapsed < sequenceLimitMs || document.currentBarcodeSequence === "") {
            //event is part of a barcode sequence
            var tmp = String.fromCharCode((e.charCode - 48));
            if (/[a-zA-Z]/i.test(charStr))
            {
                document.currentBarcodeSequence += charStr;
            } else {
                document.currentBarcodeSequence += charStr;

            }
            if (document.currentBarcodeSequence.length > 1) {
                clearTimeout(document.printBarcodeTimeout);
                document.printBarcodeTimeout = setTimeout("setBarcode()", sequenceLimitMs + 10);
            }
        } else {
            if (/[a-zA-Z]/i.test(charStr)) {
                document.currentBarcodeSequence = "" + charStr;
            } else {
                document.currentBarcodeSequence = "" + charStr;
            }
            clearTimeout(document.printBarcodeTimeout);
        }
    } else {
        document.currentBarcodeSequence = "";
        clearTimeout(document.printBarcodeTimeout);
    }
    var res1 = document.currentBarcodeSequence;
    var res2 = 0;
}
var setBarcode = function () {
    var res1 = document.currentBarcodeSequence;
    var barcodeInput = document.getElementById("BarcodeSearchBox");
    if (barcodeInput != null) //user not logged in yet
    {
        barcodeInput.value = document.currentBarcodeSequence;
        //change icon to searching id="barcodeIcon" class="glyphicon glyphicon-barcode"
        var NAME = document.getElementById("barcodeIcon");
        if (NAME != null) {
            NAME.className = "glyphicon glyphicon-refresh normal-right-spinner";   // Set other class name
        }
        //fire off ajax call for auto search here
        document.currentAsset = document.currentBarcodeSequence;
        BarcodeScanned(document.currentBarcodeSequence);
    } else {
        alert("Please login to use Asset Scanner");
    }

}

//Event Hookups
window.onkeypress = Monitor;
var keys = [];
var lastkey;
window.addEventListener("keydown",
    function (e) {
        keys[e.keyCode] = e.keyCode;
        lastkey = e.keyCode;
        var keysArray = getNumberArray(keys);
        
        setTimeout(TimedKeyUp, 1000);
    },
    false);
window.addEventListener('keyup',
    function (e) {
        keys[e.keyCode] = false;        
    },
    false);

//Document Ready
$(document).ready(function () {

    // define our variables
    var fullHeightMinusHeader, sideScrollHeight = 0;

    // create function to calculate ideal height values
    function calcHeights() {
        try {
            // set height of main columns
            fullHeightMinusHeader = jQuery(window).height() - jQuery("#MasterMenu").outerHeight();
            jQuery(".main-content, .sidebar-one").height(fullHeightMinusHeader);
            // set height of sidebar scroll content
            jQuery(".settings-scroll").height(fullHeightMinusHeader - 53);
            sideScrollHeight = (fullHeightMinusHeader / 2) - 6;
            jQuery(".side-scroll").height(sideScrollHeight);
        } catch (err) { }
        try { SetAssetViewHeight(); } catch (er) { }
        
    } // end calcHeights function

    // run on page load    
    calcHeights();

    // run on window resize event
    $(window).resize(function () {
        try { calcHeights(); } catch (er) { } 
    });
    HideLoader();

});

function TimedKeyUp() {
    keys[lastkey] = false;

}
function getNumberArray(arr) {
    var newArr = new Array();
    for (var i = 0; i < arr.length; i++) {
        if (typeof arr[i] == "number") {
            newArr[newArr.length] = arr[i];
        }
    }
    return newArr;
}
function csvToArray(text) {
    let p = '', row = [''], ret = [row], i = 0, r = 0, s = !0, l;
    for (l in text) {
        l = text[l];
        if ('"' === l) {
            if (s && l === p) row[i] += l;
            s = !s;
        } else if (',' === l && s) l = row[++i] = '';
        else if ('\n' === l && s) {
            if ('\r' === p) row[i] = row[i].slice(0, -1);
            row = ret[++r] = [l = '']; i = 0;
        } else row[i] += l;
        p = l;
    }
    return ret;
};
function TestModeChanged()
{    
    try {
        if ($('#TestModeSwitch').prop('checked')) {
            $.ajax({
                type: 'POST',
                url: '/Account/AssetController.aspx/SetTestMode',
                data: "{ischecked:'True'}",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: NotifyCustom('Changed Test Mode', 'Success', 'alert'),
                error: NotifyCustom('Changed Test Mode', 'Failure', 'alert')
            });
            return false;
        } else {
            $.ajax({
                type: 'POST',
                url: '/Account/AssetController.aspx/SetTestMode',
                data: "{ischecked:'False'}",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: NotifyCustom('Changed Test Mode', 'Success', 'alert'),
                error: NotifyCustom('Changed Test Mode', 'Failure', 'alert')
            });
            return false;
        }
    } catch (err) { NotifyCustom('Changed Test Mode', 'Failure', 'alert');}
}
function BarcodeScanned(num, isHistory, date) {
    try {
        //disable scan if on iframe pages
        var url = document.location.href;
        url = url.substr(url.lastIndexOf('/') + 1);
        if (url.startsWith("Checkout") || url.startsWith("CheckIn") || url.startsWith("PdfViewer")) {
            var NAME = document.getElementById("barcodeIcon");
            if (NAME !== null) {
                NAME.className = "glyphicon glyphicon-barcode";   // Set other class name
            }
            return false;
        }

        if (isHistory == "True") {
            $.ajax({
                type: 'POST',
                url: '/Account/AssetController.aspx/GetHistory',
                data: "{'num':'" + num + "','date':'" + date + "'}",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: AssetSuccess,
                error: AssetFailure

            });
            JumpToTab('AssetTab');
            return false;
        } else {
            $.ajax({
                type: 'POST',
                url: '/Account/AssetController.aspx/GetAsset',
                data: "{'num':'" + num + "'}",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: AssetSuccess,
                error: AssetFailure

            });           
        }
    } catch (err) {
        var aa = err;
    }

    
   
};
function printdiv(printpage) {
    var headstr = "<html><head><title></title></head><body>";
    var footstr = "</body>";

    var nnewstr = $('#' + printpage).html();
    var oldstr = document.body.innerHTML;
    document.body.innerHTML = headstr + nnewstr + footstr;
    window.pri
    window.print();
    document.body.innerHTML = oldstr;
    return false;
}
function AssetSuccess(msg) {
    //alert(msg.d.AssetNumber);
    var NAME = document.getElementById("barcodeIcon");
    if (NAME !== null) {
        NAME.className = "glyphicon glyphicon-barcode";   // Set other class name
    }
    document.ASSET = msg.d;
    var tmp = $("#BarcodeCheckBox").prop('checked');
    if (tmp === true)
    {
        if (msg.d.IsOut)
        {
            AjaxAddCheckin(msg.d.AssetNumber, true);
        } else {           
            AjaxAddCheckout(msg.d.AssetNumber, true);
        }
       
    } else {
        SetAutoCheck('false');
         LoadAsset(msg.d);
    }
   
    //Go To Asset Tab
    
    return false;
};
function HideCalUploader()
{
    $("#UploadAssetCert").hide();
}
function SetAutoCheck(autocheck)
{
    $.ajax({
        type: 'POST',
        url: '/Account/AssetController.aspx/SetAutoCheck',
        data: "{'autocheck':'" + autocheck+"'}",
        contentType: 'application/json; charset=utf-8',
        dataType: 'json'  
    });

}
function Shake(id)
{
    var iconspan = $("#" + id);    
    iconspan.addClass("mif-ani-flash");
    return false;
}
function Quiet(id)
{
    var iconspan = $("#" + id);    
    if (iconspan.hasClass("mif-ani-flash")) {
        iconspan.removeClass("mif-ani-flash");
        
    }
    return false;
}
function LoadAsset(asset) {
    try {
       
        try {
            BindAssetHistory();
            BindAssetCalibration();
            HideAllFrames();
            //$("#IsAvUp").html("true");
        } catch (erx1) { }
        var a = $("#av_imgidx").val("0");
        var a2 = $("#av_imgs").val(asset.Images);
        var a3 = csvToArray(asset.Images);
        // a.forEach(function (entry) {  });
        //fill in charm
        document.AssetImageList = a;
        document.CurrentAssetImageIdx = 0;
        var imglink = "/Account/Images/" + a3[0][0];
        var a4 =  $("#avSlideShow").attr("src", imglink);
        var a5 =  $("#av_AssetName").val(asset.AssetName);
        var a6 = $("#AssetViewHeaderLabel").html(asset.AssetName);
        var a7 = $("#AssetViewWindowLabel").html(asset.AssetNumber);
        var a8 = $("#av_AssetNumber").val(asset.AssetNumber);
        var a9 = $("#av_ShipTo").val(asset.ShipTo);
        var a11 = $("#av_ServiceOrder").val(asset.OrderNumber);
        var a12 =  $("#av_DateShipped").val(asset.DateShippedString);
        var a13 =  $("#av_ServiceEngineer").val(asset.ServiceEngineer);
        var a14 =  $("#av_PersonShipping").val(asset.PersonShipping);
        var a15 = $("#av_DateRecieved").val(asset.DateRecievedString);
        var a16 = $("#av_Weight").val(asset.weight);
        var a17 = $("#av_Description").val(asset.Description);
        try {
           $("#AssetReceivingReportFrame").attr("src", asset.ReturnReport);
           $("#AssetShippingReportFrame").attr("src", asset.UpsLabel);
           $("#AssetPackingReportFrame").attr("src", asset.PackingSlip);
            //$("#AssetReceivingReportFrame").attr("src", "/Account/Receiving/d4eb709d-beec-40f1-9634-07180121f2c8.pdf");
           // $("#AssetShippingReportFrame").attr("src", "/Account/Receiving/d4eb709d-beec-40f1-9634-07180121f2c8.pdf");
            //$("#AssetPackingReportFrame").attr("src", "/Account/Receiving/d4eb709d-beec-40f1-9634-07180121f2c8.pdf");
        } catch (erx) { }


        if (asset.IsOut) {
            $("#InOutBtn").attr("onclick", "AjaxAddCheckin('" + asset.AssetNumber + "')");
            var header = $("#AssetHeader");
            if (header.hasClass("bg-metro-dark")) {
                header.removeClass("bg-metro-dark");
                header.addClass("bg-red");
            }
            if (header.hasClass("bg-gray")) {
                header.removeClass("bg-gray");
                header.addClass("bg-red");
            }
            if (header.hasClass("bg-red")) {
                header.removeClass("bg-red");
                header.addClass("bg-red");
            }
        }
        else {
            $("#InOutBtn").attr("onclick", "AjaxAddCheckout('" + asset.AssetNumber + "')");
            var header = $("#AssetHeader");
            if (header.hasClass("bg-red")) {
                header.removeClass("bg-red");
                header.addClass("bg-metro-dark");
            } 
            if (header.hasClass("bg-gray")) {
                header.removeClass("bg-gray");
                header.addClass("bg-metro-dark");
            }
            if (header.hasClass("bg-metro-dark")) {
                header.removeClass("bg-metro-dark");
                header.addClass("bg-metro-dark");
            }
        }  
        if (asset.IsHistoryItem) {            
            var header = $("#AssetHeader");
            if (header.hasClass("bg-metro-dark")) {
                header.removeClass("bg-metro-dark");
                header.addClass("bg-gray");
            } 
            if (header.hasClass("bg-red")) {
                header.removeClass("bg-red");
                header.addClass("bg-gray");
            } 
            if (header.hasClass("bg-gray")) {
                header.removeClass("bg-gray");
                header.addClass("bg-gray");
            } 


        }
        if (asset.IsDamaged)
        {            
            $("#av-Damaged").prop('checked', true); 
        } else {
            $("#av-Damaged").prop('checked', false);
        }
        $("#av-Damaged").attr("onclick", "AssetIsDamaged('" + asset.AssetNumber + "')");

        if (asset.OnHold) {
            $("#av-OnHold").prop('checked', true);
        } else {
            $("#av-OnHold").prop('checked', false);
        }
        $("#av-OnHold").attr("onclick", "AssetOnHold('" + asset.AssetNumber + "')");

        if (asset.IsCalibrated) {
            $("#av-CalibratedTool").prop('checked', true);
        } else {
            $("#av-CalibratedTool").prop('checked', false);
        }
        $("#av-CalibratedTool").attr("onclick", "AssetIsCalibrated('" + asset.AssetNumber + "')");
       
        showMetroCharm('#assetview-charm');
        
    } catch (err) { }
}
function ResizeAssetReport() {
    try {

        //ShowDiv('AssetPackingReportDiv');
    //ShowDiv('AssetShippingReportDiv');
   // ShowDiv('AssetReceivingReportDiv');
    $("#AssetPackingReportFrame").height($("#AssetPackingReportDiv").height());
    $("#AssetPackingReportFrame").width($("#AssetPackingReportDiv").width());
    $("#AssetShippingReportFrame").height($("#AssetShippingReportDiv").height());
    $("#AssetShippingReportFrame").width($("#AssetShippingReportDiv").width());
    $("#AssetReceivingReportFrame").height($("#AssetReceivingReportDiv").height());
    $("#AssetReceivingReportFrame").width($("#AssetReceivingReportDiv").width()); } catch (er) { }
    

    return false;
}
function SetAssetViewHeight() {
    try { var newHeight = 0;
    var myViewHeight = jQuery(".main-content").height();
    newHeight = myViewHeight - 178;
    var a = $("#AssetImageDiv").height();
    $("#AssetImageDiv").height(newHeight);
    $("#AssetCalibrationDiv").height(newHeight-25);
    $("#AssetPackingReportDiv").height(newHeight);
    $("#AssetShippingReportDiv").height(newHeight);
    $("#AssetReceivingReportDiv").height(newHeight);
    $("#AssetHistoryDiv").height(newHeight); } catch (er) { }
   
    return false;
}
function HideAllFrames()
{
    try {
        HideDiv('AssetPackingReportDiv');
        HideDiv('AssetShippingReportDiv');
        HideDiv('AssetReceivingReportDiv');
    } catch (er) { }
}
function ShowAssetFrames() {
    try {
        ShowDiv('AssetPackingReportDiv');
        ShowDiv('AssetShippingReportDiv');
        ShowDiv('AssetReceivingReportDiv');
    } catch (er) { }
}
function BindAssetHistory() {
    var btn = document.getElementById("HistoryBinderBtn");
    btn.click();
}
function BindAssetCalibration() {
    var btn = document.getElementById("CalibrationBinderBtn");
    btn.click();
}
function JumpToTab(dest)
{
    //$("#AssetTab").css("opacity", 0);
    //$("#ImageTab").css("opacity", 0)
    //$("#ReportTab").css("opacity", 0);
    //$("#CalibrationTab").css("opacity", 0);
    //$("#HistoryTab").css("opacity", 0);
   // $("#" + dest).css("opacity", 1);
}
function AjaxAddCheckout(num, autocheck) {
    if (autocheck == null)
    {
        autocheck = false;
    }
    $.ajax({
        type: 'POST',
        url: '/Account/AssetController.aspx/AddCheckoutItem',
        data: "{'num':'" + num + "','autocheck':'" + autocheck+"'}",
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: AddCheckOutItem,
        error: AssetFailure

    });
    event.stopPropagation();
   
};
function AjaxAddCheckin(num, autocheck) {
    if (autocheck == null) {
        autocheck = false;
    }
    $.ajax({
        type: 'POST',
        url: '/Account/AssetController.aspx/AddCheckinItem',
        data: "{'num':'" + num + "','autocheck':'" + autocheck + "'}",
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: AddCheckInItem,
        error: AssetFailure

    });
    event.stopPropagation();
};
function AddCheckInItem(msg) {
    try {
        if (msg.d == null) return;
    
        if (msg.d.IsOut == false) return false;        
        CheckInPanelUpdate();
        hideMetroCharm('#settings-charm');
        hideMetroCharm('#assetview-charm');
        showMetroCharm('#assets');

        setTimeout(hideCheckInOut, 1750);
        //update checkin panel
    } catch (err) { }
}
function AddCheckOutItem(msg) {
    try {
        if (msg.d == null) return false;

        if (msg.d.IsOut == true) {
            return false
        };
        CheckOutPanelUpdate();
        hideMetroCharm('#settings-charm');
        hideMetroCharm('#assetview-charm');
        showMetroCharm('#assets');

        
        setTimeout(hideCheckInOut, 1420);
        //AjaxBindInOutBoxes();
        
    } catch (err) { }
    return false;
}
function CheckOutPanelUpdate() {

    var btn = document.getElementById("UpdateAllCarts");
    btn.click();

}
function UpdateAllPanels() {

    var btn = document.getElementById("UpdateAllCarts");
    btn.click();

}
function CheckInPanelUpdate() {

    var btn = document.getElementById("UpdateAllCarts");
    btn.click();

}
function ChangeAssetView(view) {

    $("#avSelectedView").val(view);
    var btn = document.getElementById("avChangeView");
    btn.click();

}
function SetSearchType(t)
{
    $("#avSelectedSearch").val(t);
}
function ChangeAssetViewListType(type)
{
    var lv = $("#lv1");
    if (lv != null)
    {
        try {
            lv.removeClass("default");
        } catch (er) { }
        try {
            lv.removeClass("list-type-icons");
        } catch (er) { }
        try {
            lv.removeClass("list-type-tiles");
        } catch (er) { }
        try {
            lv.removeClass("list-type-listing");
        } catch (er) { }
        try {
            lv.addClass(type);
        } catch (err) { }
    }
}
function hideCheckInOut()
{
    hideMetroCharm('#assets');
}
function NextAssetImg()
{
    try {
        var imgs = $("#av_imgs").val();
        var a = csvToArray(imgs);
        var idx = $("#av_imgidx").val();
        var newidx = parseInt(idx) + 1;
        if (newidx >= a.length)
        {
             return;
        }
       
        var lnk = a[0][newidx];
        var imglink = "/Account/Images/" + lnk ;
        $("#avSlideShow").attr("src", imglink);
        //set index
        $("#av_imgidx").val(newidx);

    } catch (err) { PrevAssetImg(); }
}
function PrevAssetImg() {
    try {
        var imgs = $("#av_imgs").val();
        var a = csvToArray(imgs);
        var idx = $("#av_imgidx").val();
        var newidx = parseInt(idx) - 1;
        if (newidx < 0) {
            return;
        }
        var lnk = a[0][newidx];
        var imglink = "/Account/Images/" + lnk;
        $("#avSlideShow").attr("src", imglink);
        //set index
        $("#av_imgidx").val(newidx);

    } catch (err) { NextAssetImg(); }
   
}
function AssetFailure(msg) {
    var t = "ff";
};
function DisplayAsset(msg)
{
    var NAME = document.getElementById("barcodeIcon");
    if (NAME !== null) {
        NAME = "glyphicon glyphicon-barcode";   // Set other class name
    }

}
function addNotification(asset, out) {
    var previousContent = $("#ActiveNoticeContainer").html();
    if (previousContent !== null) {
        if (out)
        {//out
            var newContent = "<div id='" + asset.AssetNumber + "' class='row bg-red'>" +
                "<h1>" + asset.AssetName + "</h1>" +
                "</div>";
            $("#ActiveNoticeContainer").html(previousContent + newContent);
        } else
        {//in
            var newContent = "<div id='" + asset.AssetNumber + "' class='row bg-metro'>" +
                "<h1>" + asset.AssetName+"</h1>"+
                "</div>";
            $("#ActiveNoticeContainer").html(previousContent + newContent);
        }
        
    } else
    {

    }
}
function NotifyClick() {
    
    $.Notify({
        caption: 'Notify title',
        content: 'Notify content',
        icon: "<i class='glyphicon glyphicon-wrench'></i>"
    });
};
function NotifyCustom(cap, cont, ico) {
    $.Notify({
        caption: cap,
        content: cont,
        icon: "<i class='glyphicon glyphicon-" + ico + "'></i>"
    });
};
function openModalDiv(divname) {
    try {
        $('#' + divname).dialog({
            draggable: true,
            resizable: true,
            show: 'Transfer',
            hide: 'Transfer',
            width: 320,
            autoOpen: false,
            minHeight: 10,
            minwidth: 10});
        $('#' + divname).dialog('open');
        $('#' + divname).parent().appendTo($("form:first"));
    } catch (err) { }
    return false;
}
function closeModalDiv(divname) {
    try {
        $('#' + divname).dialog('close');
    } catch (err) { }
    return false;
}
function ShowLoader() {
    $("#FullScreenLoader").show();
}
function HideLoader() {
    $("#FullScreenLoader").hide();
}
function ShowDiv(divname) {
    try {
        $('#' + divname).show();
    } catch (err) { }
}
function HideDiv(divname) {
    try {
        $('#' + divname).hide();
    } catch (err) { }
}
function UpdateAsset(asset) {
    $.ajax({
        type: 'POST',
        url: '/Account/AssetController.aspx/UpdateAsset',
        data:  JSON.stringify(asset),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (result) {
            alert("Success Update Asset");
        },
        error: function (result) {
            alert("Error Update Asset");
        }   
    });
}
function AssetIsDamaged(num) {
    $.ajax({
        type: 'POST',
        url: '/Account/AssetController.aspx/GetAsset',
        data: "{'num':'" + num + "'}",
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: IsDamagedResponse
    });
}
function AssetOnHold(num)
{
    $.ajax({
        type: 'POST',
        url: '/Account/AssetController.aspx/GetAsset',
        data: "{'num':'" + num + "'}",
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: OnHoldResponse
    });
}
function AssetIsCalibrated(num) {
    $.ajax({
        type: 'POST',
        url: '/Account/AssetController.aspx/GetAsset',
        data: "{num:" + num + "}",
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: IsCalibratedResponse
    });
}
function OnHoldResponse(msg)
{
    try {
        var tmp = $("#av-OnHold").prop('checked');
        msg.d.OnHold = $.parseJSON(tmp);
        $.ajax({
            type: 'POST',
            url: '/Account/AssetController.aspx/AssetOnHold',
            data: "{'num':'" + $("#av_AssetNumber").val() + "','b':'"+msg.d.OnHold+"'}",
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (result) {
                alert("Success Update Asset");
            },
            error: function (result) {
                alert("Error Update Asset");
            }
        });
    } catch (err) {
        alert(err);}
    return false;
}
function IsDamagedResponse(msg) {
    try {
        var tmp =$("#av-OnHold").prop('checked');
        msg.d.IsDamaged = $.parseJSON(tmp); 
        $.ajax({
            type: 'POST',
            url: '/Account/AssetController.aspx/AssetIsDamaged',
            data: "{'num':'" + $("#av_AssetNumber").val() + "','b':" + msg.d.IsDamaged + "'}",
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (result) {
                alert("Success Update Asset");
            },
            error: function (result) {
                alert("Error Update Asset");
            }
        });
    } catch (err) { }
    return false;
}
function IsCalibratedResponse(msg) {
    try {
        var tmp =$("#av-CalibratedTool").prop('checked');
        msg.d.IsCalibrated = $.parseJSON(tmp); 
        $.ajax({
            type: 'POST',
            url: '/Account/AssetController.aspx/AssetIsCalibrated',
            data: "{'num':'" + $("#av_AssetNumber").val() + "','b':" + msg.d.IsCalibrated + "'}",
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (result) {
                alert("Success Update Asset");
            },
            error: function (result) {
                alert("Error Update Asset");
            }
        });
    } catch (err) { }
    return false;
}
function SetAvView()
{
    var isUp = $("#IsAvUp").html();

}

//THEME
function ChangeTheme()
{
    var a1 = $('.bg-metro')
    $.each(a1, function (index, value) {
        $(value).removeClass("bg-metro")
        $(value).addClass("bg-red")
    });
    var a = $('.fg-white')
    $.each(a, function (index, value) {
        $(value).removeClass("fg-white")
        $(value).addClass("fg-red")
    });
    var b = $('.bg-metro-dark')
    $.each(b, function (index, value) {
        $(value).css("background-color", "red")
    });
    var c = $('.bg-metro-light')
    $.each(c, function (index, value) {
        $(value).css("background-color", "red")
    });
  
}