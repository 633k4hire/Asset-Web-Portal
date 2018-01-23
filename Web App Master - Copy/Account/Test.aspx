<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Test.aspx.cs" Inherits="Web_App_Master.Account.Test" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../Scripts/jquery-1.10.2.min.js"></script>
    <script type="text/javascript">
        function postit(ss){
        $.ajax({
            type: 'POST',
            url: 'Test.aspx/BarcodeSearch',
                data: "{num:" + ss + "}",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: parsesearch_complete,
                error: ajaxError

        });
        }
        function parsesearch_complete(result) {
            alert(result.d);

        }
        function ajaxError(result) {
            alert(result.d);
        }
    </script>
    <script type="text/javascript">
    document.currentBarcodeSequence = "";
    document.lastKeypress = new Date();
    var monitorBarcodes = function(e){
        //sequenceLimitMs should be set as low as possible to prevent capture of human keyed numbers.
        //200 allow testing without a barcode scanner, you could try a value of 50 with a scanner.
        var sequenceLimitMs = 200;
        var now = new Date();
        var elapsed = now - document.lastKeypress;
        document.lastKeypress = now;
        if(e.charCode >= 48 && e.charCode <= 57){
            //pressed key is a number
            if(elapsed < sequenceLimitMs || document.currentBarcodeSequence === ""){
                //event is part of a barcode sequence
                document.currentBarcodeSequence += (e.charCode - 48);

                if(document.currentBarcodeSequence.length > 1){
                    clearTimeout(document.printBarcodeTimeout);
                    document.printBarcodeTimeout = setTimeout("setBarcode()", sequenceLimitMs+10);
                }
            } else {
                document.currentBarcodeSequence = "" + (e.charCode - 48);
                clearTimeout(document.printBarcodeTimeout);
            }
        } else {
            document.currentBarcodeSequence = "";
            clearTimeout(document.printBarcodeTimeout);
        }
    }
    var setBarcode = function(){
        var barcodeInput = document.getElementById("barcode");
        barcodeInput.value = document.currentBarcodeSequence;

        postit(document.currentBarcodeSequence);
    }

    window.onkeypress = monitorBarcodes;
</script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:TextBox ID="barcode" runat="server" ClientIDMode="Static" ></asp:TextBox>
            <input id="other" type="text" />
        </div>
    </form>
</body>
</html>
