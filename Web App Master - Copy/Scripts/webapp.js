'use strict';

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



function AssetSuccess(msg) {
    //alert(msg.d.AssetNumber);
    var NAME = document.getElementById("barcodeIcon");
    if (NAME !== null) {
        NAME.className = "glyphicon glyphicon-barcode";   // Set other class name
    }
    document.ASSET = msg.d;
    LoadAsset(msg.d);
     
    
};
function LoadAsset(asset)
{
    try {
        //images
        $("#av_imgidx").val("0");
        $("#av_imgs").val(asset.Images);
        var a = csvToArray(asset.Images);
        // a.forEach(function (entry) {  });
        //fill in charm
        document.AssetImageList = a;
        document.CurrentAssetImageIdx = 0;
        var imglink = "/Account/Images/" + a[0][0];
        $("#avSlideShow").attr("src", imglink);
        $("#av-AssetName").val(asset.ItemName);
        $("#av-AssetNumber").val(asset.AssetNumber);
        $("#av-ShipTo").val(asset.Shipto);
        $("#av-ServiceOrder").val(asset.ServiceOrder);
        $("#av-DateShipped").val(asset.DateShippedString);
        $("#av-ServiceEngineer").val(asset.ServiceEngineer);
        $("#av-PersonShipping").val(asset.PersonShipping);
        $("#av-DateRecieved").val(asset.DateRecievedString);
        $("#av-Weight").val(asset.weight);
        $("#av-Description").val(asset.Description);
        if (asset.IsOut) {
            $("#InOutBtn").attr("onclick", "AjaxAddCheckin('" + asset.AssetNumber + "')");

        }
        else {
            $("#InOutBtn").attr("onclick", "AjaxAddCheckout('" + asset.AssetNumber + "')");
        }


        showMetroCharm('#assetview-charm');
        var assetnum = document.currentAsset;
    } catch (err) { alert(err); }
}
function AjaxBindInOutBoxes()
{
    $.ajax({
        type: 'POST',
        url: '/Account/AssetController.aspx/BindCheckInOutBoxes',
        data: "{}",
        contentType: 'application/json; charset=utf-8',
        dataType: 'json'
    });
}
function AjaxAddCheckout(num) {
    $.ajax({
        type: 'POST',
        url: '/Account/AssetController.aspx/AddCheckoutItem',
        data: "{num:" + num + "}",
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: AddCheckOutItem,
        error: AssetFailure

    });
};
function AjaxAddCheckin(num) {
    $.ajax({
        type: 'POST',
        url: '/Account/AssetController.aspx/AddCheckinItem',
        data: "{num:" + num + "}",
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: AddCheckInItem,
        error: AssetFailure

    });
};
//after succesfull checkin async postback
function AddCheckInItem(msg) {
    try {
        if (msg.d == null) return;
       // var assetnumber = msg.d.AssetNumber;
        //var assetname = msg.d.ItemName;
        //var previousContent = $("#ActiveCheckinContainer").html();
        //if (previousContent !== null) {

//            var newContent = "<div id='" + assetnumber + "' class='bg-metro'>" +
  //              "<h1>" + assetname + "</h1>" +
    //            "</div>";
      //      $("#ActiveCheckinContainer").html(previousContent + newContent);
            
       // }
        NotifyCustom(msg.d.AssetNumber, 'Added To CheckIn', 'cloud-upload');

        hideMetroCharm('#settings-charm');
        hideMetroCharm('#assetview-charm');
        showMetroCharm('#assets');

        setTimeout(hideCheckInOut, 3000);
        //update checkin panel
    } catch (err) { }
}
//after succesfull checkout async postback
function AddCheckOutItem(msg) {
    try {
        if (msg.d == null) return;
        /**
         * 
         
        var assetnumber = msg.d.AssetNumber;
        var assetname = msg.d.ItemName;
        var previousContent = $("#ActiveCheckoutContainer").html();
        if (previousContent !== null) {

            var newContent = "<div id='" + assetnumber + "' class='bg-red'>" +
                "<h1>" + assetname + "</h1>" +
                "</div>";
            $("#ActiveCheckoutContainer").html(previousContent + newContent);
        }
*/
        NotifyCustom(msg.d.AssetNumber, 'Added To CheckOut', 'cloud-download');
        CheckOutPanelUpdate();
        hideMetroCharm('#settings-charm');
        hideMetroCharm('#assetview-charm');
        showMetroCharm('#assets');

        
        setTimeout(hideCheckInOut, 3000);
        //AjaxBindInOutBoxes();
        
    } catch (err) { }
}
function CheckOutPanelUpdate() {

    var btn = document.getElementById("button33");
    btn.click();

}
function ChangeAssetView(view) {

    $("#avSelectedView").val(view);
    var btn = document.getElementById("avChangeView");
    btn.click();

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
                "<h1>" + asset.ItemName + "</h1>" +
                "</div>";
            $("#ActiveNoticeContainer").html(previousContent + newContent);
        } else
        {//in
            var newContent = "<div id='" + asset.AssetNumber + "' class='row bg-metro'>" +
                "<h1>" + asset.ItemName+"</h1>"+
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