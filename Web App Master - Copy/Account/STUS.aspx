<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="STUS.aspx.cs" Inherits="Web_App_Master.Account.STUS" %>
<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:PlaceHolder runat="server" ID="MessageHolder" Visible="false">
                        <p class="fg-red text-bold" >
                            <asp:Literal runat="server" ID="Notification" />
                        </p>
                    </asp:PlaceHolder>
    <div class="row" id="StusSettings">
        <asp:FileUpload ID="LibraryUploader" runat="server" Width="250px" />
        <asp:Button ID="UploadLibrary" ClientIDMode="Static" CssClass="fg-white" runat="server" Text="Upload Asset File" OnClick="UploadLibrary_Click" />
    </div>
    <div class="row">
        <asp:Button ID="SendSQL" ClientIDMode="Static" runat="server" Text="Asset Library to SQL" OnClick="SendSQL_Click" />
    
    </div>
    <div class="row">
        <asp:Button ID="PullSQL" ClientIDMode="Static" runat="server" Text="SQL to Asset Library" OnClick="PullSQL_Click" />
    
    </div>
    <div class="row">
        <asp:Button ID="DeleteSQL" ClientIDMode="Static" runat="server" Text="Erase SQL DataBase" OnClick="DeleteSQL_Click" />
    
    </div>
</asp:Content>
