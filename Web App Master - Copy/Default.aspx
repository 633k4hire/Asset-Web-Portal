<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Web_App_Master._Default" %>

<%@ Register Src="~/Account/Templates/av_list_template.ascx" TagPrefix="uc1" TagName="av_list_template" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
        

    <div class="jumbotron">
        <h1>Asset Portal</h1>
        <p class="lead">Welcome to the Web Asset Portal</p>
    </div>

   <div class="row">
      <div id="av_dynamic_column" class="col-md-12">
    <div class=" av-lv rounded bg-metro-dark shadow" >

        <div class=" av-lv-thumb"><img id="av_list_img" src="/Account/Images/00010.JPG" style=" height:100%;"></img></div>
        <div class="av-lv-seperator"></div>    
                        <div class="av-lv-content">
                            <strong><input type="text" id="av_title" value="Super dooper Leverler For DST Machine" readonly style="width:100%; border:0px solid black; background-color:transparent;" />
                            </strong>
       </div>

         <span class="av-lv-badge bg-red fg-white shadow"><strong>0000</strong></span>
         

    </div>
          </div>
</div>

</asp:Content>
