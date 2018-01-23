<%@ Page Title="PDF" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PdfViewer.aspx.cs" Inherits="Web_App_Master.Account.PdfViewer" %>
<asp:Content ID="PdfViewer" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript">

                $(document).ready(function () {
                   
                    
                    function calcIframeDivHeight() {
                        var newHeight = 0;
                        var myViewHeight = jQuery(".main-content").height();
                        newHeight = myViewHeight - 160;
                        $("#IframeDiv").height(newHeight);
                        $("#MainContent_ReportFrame").height(newHeight);
                        $("#MainContent_ReportFrame").width($("#IframeDiv").width());     
                    }

                    // run on page load    
                    calcIframeDivHeight();

                    // run on window resize event
                    $(window).resize(function () {
                        calcIframeDivHeight();
                    });

                });

            </script>
         <div class="row">
            <div class="col-md-12">
                <!--    <span class="awp-save-btn bg-green fg-white shadow  mif-ani-flash " onclick="printpackingslip()"><i title="Print"  style="font-size:2em; vertical-align:top" class="mif-printer av-hand-cursor fg-white shadow-metro-black"></i></span>--> 
                <div class="awp_box rounded bg-metro-dark shadow">
                    <div class="awp_box_title bg-metro-dark">
                       <span class="fg-white shadow-metro-black"><asp:Literal runat="server" ID="PdfTitle" ></asp:Literal></span>
                    </div>
                    <div class="awp_box_content bg-metro-light">
                         <div id="ReceivingHidden" style="display:none"><asp:Literal ID="ReceivingLink" runat="server"></asp:Literal> </div>
                        <div id="IframeDiv" style="min-height:100px;height:300px;">
                         <iframe frameborder="0" style="position:absolute; min-height:100px;" id="ReportFrame" runat="server" src="#" ></iframe>
                         </div>
                    </div>
                </div>
            </div>  
        </div>

</asp:Content>
