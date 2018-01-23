<%@ Page Title="Out Cart" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OutCart.aspx.cs" Inherits="Web_App_Master.Account.Checkout" %>
<asp:Content ID="OutCartContent" ContentPlaceHolderID="MainContent" runat="server">
        <div class="app-bar  shadow-bottom" style="margin-top:0px !important; left:0px!important; position:fixed !important; z-index:25000;">
    
    <ul class="app-bar-menu ">
        <li><h1>Out Cart</h1> </li>
    </ul>
<ul class="app-bar-menu place-right">
            <li>
            <asp:LinkButton runat="server" OnClick="ContinueToCheckOutBtn_Click"  ID="ContinueToCheckOutBtn" ClientIDMode="Static" style="vertical-align:bottom; text-decoration:none"><strong>Continue To Shipping&nbsp&nbsp</strong><i style="vertical-align:top" class="glyphicon glyphicon-arrow-right md-glyph"></i></asp:LinkButton>
            
        </li>
    </ul>
   </div>


    <div class="bg-metro" style=" padding-top:50px !important"></div>
            <div class="row">
                     <div class="col-md-3">
                         <div class="awp_box rounded bg-metro-dark shadow">           
                            <div class="awp_box_title bg-metro-dark">
                               <span class="fg-white shadow-metro-black">Ship To</span></div>
                            <div class="awp_box_content bg-metro-light" style="height:75px !important; vertical-align:middle;">   
                                    <asp:DropDownList Width="100%" ClientIDMode="Static" ID='checkout_ShipTo' AppendDataBoundItems="true" runat="server"  CssClass="form-control">
                                        <asp:ListItem Text="--Select One--" Value="" /> 
                                    </asp:DropDownList>
                            </div>
                        </div>
                     </div>
                     <div class="col-md-3">
                         <div class="awp_box rounded bg-metro-dark shadow">           
                            <div class="awp_box_title bg-metro-dark">
                               <span class="fg-white shadow-metro-black">Service Engineer</span></div>
                            <div class="awp_box_content bg-metro-light" style="height:75px !important; vertical-align:middle;">
                                    <asp:DropDownList Width="100%" ClientIDMode="Static" ID='checkout_ServiceEngineer' AppendDataBoundItems="true" runat="server"   CssClass=" form-control">
                                        <asp:ListItem Text="--Select One--" Value="" /> 
                                    </asp:DropDownList>
                            </div>
                        </div>
                     </div>
                     
                     <div class="col-md-3">
                         <div class="awp_box rounded bg-metro-dark shadow">           
                            <div class="awp_box_title bg-metro-dark">
                               <span class="fg-white shadow-metro-black">Shipping Person</span></div>
                            <div class="awp_box_content bg-metro-light" style="height:75px !important; vertical-align:middle;">
                                    <asp:DropDownList Width="100%" ClientIDMode="Static" ID='checkout_ShippingPerson' AppendDataBoundItems="true" runat="server"  CssClass=" form-control">
                                        <asp:ListItem Text="--Select One--" Value="" /> 
                                    </asp:DropDownList>
                            </div>
                        </div>
                     </div>
                     
                     <div class="col-md-3">
                         <div class="awp_box rounded bg-metro-dark shadow">           
                            <div class="awp_box_title bg-metro-dark">
                               <span class="fg-white shadow-metro-black">Order Number</span></div>
                            <div class="awp_box_content bg-metro-light" style="height:75px !important; vertical-align:middle;">
                                <div class="">                                    
                                    <input runat="server" id="checkout_OrderNumber" type="text" class="form-control" placeholder="Order Number" />
                                </div>
                            </div>
                        </div>
                     </div>
                     
                 </div>

    <div class="row">
        <div class="col-md-12">
                         <div class="awp_box rounded bg-metro-dark shadow">           
                            <div class="awp_box_title bg-metro-dark">
                               <span class="fg-white shadow-metro-black">Items:</span></div>
                            <div class="awp_box_content bg-metro-light"">
                                <div class="row">
                                    <asp:UpdatePanel runat="server" ID="OutCartUpdatePanel" ClientIDMode="Static" ChildrenAsTriggers="true" UpdateMode="Conditional">
                                        <ContentTemplate>   
                                            <asp:Repeater runat="server" ID ="FinalCheckoutRepeater" ClientIDMode="Static" OnItemCommand="FinalCheckoutRepeater_ItemCommand" >                    
                                                <ItemTemplate>              
                                                  <div id="av_dynamic_column" class="col-md-12" >
                                                         <span class="av-lv-linkbadge"><asp:LinkButton ID="RemoveFromCheckOutButton" CommandName="Delete" CommandArgument='<%# Eval("AssetNumber")%>' runat="server" > <i title="Remove from Check Out"  style="font-size:1.5em; cursor:pointer" class="glyphicon glyphicon-remove c-red"></i></asp:LinkButton></span>
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

        
    
</asp:Content>
