<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AssetView.aspx.cs" Inherits="Web_App_Master.Account.AssetView" %>
<%@ MasterType VirtualPath="~/Site.Master" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!--menu-->
    <div class="app-bar" style="margin-top:0px !important; left:0px!important; position:fixed !important; z-index:25000;">
    
    <ul class="app-bar-menu">
        <li><h1>Asset View</h1> </li>
        <li>
            <a href="#" class="dropdown-toggle">View</a>
            <ul class="d-menu" data-role="dropdown">
                <li><a href="#" onclick="ChangeAssetView('list')">List</a></li>
                <li><a href="#" onclick="ChangeAssetView('detail')">Detail</a></li>
                <li><a href="#" onclick="ChangeAssetView('smtile')">Small Tile</a></li>
                <li><a href="#" onclick="ChangeAssetView('mdtile')">Medium Tile</a></li>
                <li><a href="#" onclick="ChangeAssetView('lgtile')">large Tile</a></li>
            </ul>
        </li>
       
       
        
        
    </ul>
   
</div>
<!--updatepanel-->
                    <asp:UpdatePanel runat="server" ID="CheckoutUpdatePanel" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>                
               <!--REPEATER-->
    <div style=" padding-top:50px !important">
    <asp:Repeater ID="AssetViewRepeater" runat="server" EnableViewState="false" OnItemDataBound="AssetViewRepeater_ItemDataBound">
        <HeaderTemplate> </HeaderTemplate>
        <ItemTemplate></ItemTemplate>
        <FooterTemplate> </FooterTemplate>
    </asp:Repeater>
        </div>
            </ContentTemplate>
            <Triggers>
               <asp:AsyncPostBackTrigger ControlID="avChangeView" EventName="Click" />
            </Triggers>
         </asp:UpdatePanel>


  
    <div id="maincontentTiles" style="height:100%;width:100%;min-height:100%;min-width:100%;background-color:#2d89ef;">
        </div>
<asp:TextBox runat="server" ID ="avSelectedView" ClientIDMode="Static" style="display:none"></asp:TextBox>
<asp:Button ID="avChangeView" runat="server" Text="CLICK ME" OnClick="ChangeView_Click" style="display:none" ClientIDMode="Static"/>

    
    

    </asp:Content>
