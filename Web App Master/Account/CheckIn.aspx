<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CheckIn.aspx.cs" Inherits="Web_App_Master.Account.CheckIn" %>
<asp:Content ID="CheckInContent" ContentPlaceHolderID="MainContent" runat="server">
           <div class="app-bar" style="margin-top:0px !important; left:0px!important; position:fixed !important; z-index:25000;">
    
    <ul class="app-bar-menu">
        <li><h1>Check In</h1> </li>
        <li>
                <asp:LinkButton ID ="CheckInViewBtn" OnClick="CheckInViewBtn_Click" ToolTip="Check In" ClientIDMode="Static" runat="server"><span id="CheckInIcon" runat="server" class="glyphicon glyphicon-cloud-upload md-glyph"></span></asp:LinkButton>
        </li>
        <li>
                <asp:LinkButton ID ="ReportViewBtn" OnClick="ReportViewBtn_Click" ToolTip="Reports" ClientIDMode="Static" runat="server"><span id="ReportIcon" runat="server" class="mif-file-text mif-3x"></span></asp:LinkButton>
        </li>
    </ul>
<ul class="app-bar-menu place-right">
            <li>
            <asp:LinkButton runat="server" OnClick="Finalize_Click"  ID="Finalize" ClientIDMode="Static" style="vertical-align:top; text-decoration:none"><i style="vertical-align:top" class="glyphicon glyphicon-floppy-disk md-glyph"></i><strong>Finalize</strong></asp:LinkButton>
            </li>
            
    </ul>
   </div>


    <div class="bg-metro" style=" padding-top:50px !important"></div>
    <asp:MultiView ID="CheckInMultiView" ActiveViewIndex="0" runat="server">
        <asp:View ID="CheckInView" runat="server">
                <div class="row">
                   <div class="col-md-12">
                         <div class="awp_box rounded bg-metro-dark shadow">           
                            <div class="awp_box_title bg-metro-dark">
                               <span class="fg-white shadow-metro-black">Items:</span></div>
                            <div class="awp_box_content bg-metro-light"">
                                <div class="row">
                                    <asp:UpdatePanel runat="server" ID="CheckInPageUpdatePanel" ClientIDMode="Static" ChildrenAsTriggers="true" UpdateMode="Conditional">
                                        <ContentTemplate>   
                                            <asp:Repeater runat="server" ID ="FCIrepeater" ClientIDMode="Static" OnItemCommand="FinalCheckInRepeater_ItemCommand" >                    
                                                <ItemTemplate>              
                                                  <div id="av_dynamic_column" class="col-md-12" >
                                                         <span class="av-lv-linkbadge"><asp:LinkButton ID="RemoveFromCheckInButton" CommandName="Delete" CommandArgument='<%# Eval("AssetNumber")%>' runat="server" > <i title="Remove from Check In"  style="font-size:1.5em; cursor:pointer" class="glyphicon glyphicon-remove c-red"></i></asp:LinkButton></span>
                                                              <div class=" av-lv rounded bg-metro-dark shadow"  onclick="BarcodeScanned('<%# Eval("AssetNumber")%>');">
       
                                                            <div class=" av-lv-thumb"><img id="av_list_img" src="<%# Eval("FirstImage")%>" style=" height:100%;"></img></div>
                                                            <div class="av-lv-seperator"></div>
                                                            <div class="av-lv-content fg-white shadow-metro-black">
                                                                <strong><%# Eval("AssetName")%> - <%# Eval("Description")%></strong>                                                                                
                                                            </div>           
                                                             <span class="edge-badge bg-red fg-white shadow"><strong><%# Eval("AssetNumber")%></strong></span>
                                                        </div>
                                                      </div>
                                                </ItemTemplate>                                 
                                        </asp:Repeater>
                                        </ContentTemplate>
                                       
                                    </asp:UpdatePanel>
                                  
                                </div>
                            </div>
                        </div>
                     </div>

              </div>

        </asp:View>
        <asp:View ID="ReportView" runat="server">
             <script type="text/javascript">

                $(document).ready(function () {
                   
                    
                    function calcIframeDivHeight() {
                        try {
                            var newHeight = 0;
                        var myViewHeight = jQuery(".main-content").height();
                        newHeight = myViewHeight - 160;
                        $("#IframeDiv").height(newHeight);
                        $("#MainContent_ReportFrame").height(newHeight);
                        $("#MainContent_ReportFrame").width($("#IframeDiv").width());
                        } catch (er) { }
                            
                    }

                    // run on page load   
                    try {calcIframeDivHeight(); } catch (er) { }
                    
                    
                    // run on window resize event
                    $(window).resize(function () {
                        try {calcIframeDivHeight(); } catch (er) { } 
                    });

                });

            </script>
            <div class="row">
            <div class="col-md-12">
                <!--    <span class="awp-save-btn bg-green fg-white shadow  mif-ani-flash " onclick="printpackingslip()"><i title="Print"  style="font-size:2em; vertical-align:top" class="mif-printer av-hand-cursor fg-white shadow-metro-black"></i></span>--> 
                <div class="awp_box rounded bg-metro-dark shadow">
                    <div class="awp_box_title bg-metro-dark">
                       <span class="fg-white shadow-metro-black">Receiving Report:</span>
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
        </asp:View>
    </asp:MultiView>

        
</asp:Content>
