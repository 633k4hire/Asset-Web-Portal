<%@ Page Title="Starrag" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Web_App_Master._Default" %>

<%@ Register Src="~/Account/Templates/av_list_template.ascx" TagPrefix="uc1" TagName="av_list_template" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
        <div class="bg"></div>
    
    <div class="jumbotron">
        <h1><span onclick="ShowAssetModal();">Asset Web Portal</span></h1>
        <p class="lead"  style="z-index:400;"><b> Asset Tracking and Deployment System</b></p>
    </div>
    
   
    
<!-- <script>
        $(function(){
            var checks = $("input:radio");
            var lv = $("#lv1");
            checks.on('change', function(){
                if ($(this).is(":checked")) {
                    lv.removeClass(function (index, css) {
                        return (css.match(/(^|\s)list-type-\S+/g) || []).join(' ');
                    }).addClass($(this).val());
                }
            });
        });
    </script> <div class="row">
    <div class="col-md-12">

        <div class="awp_box rounded bg-metro-dark shadow">
        <span class="awp-bar ">
             <span class="awp-bar-btn bg-green fg-white shadow"><i onclick="javascript:alert('hello')" title="Add To Task List" class="mif-previous av-hand-cursor"></i></span>
             <span class="awp-bar-btn bg-green fg-white shadow"><i onclick="javascript:alert('hello')" title="Add To Task List" class="mif-next av-hand-cursor"></i></span>
       </span>
            <div class="awp_box_title bg-metro-dark">
               <span class="fg-white shadow-metro-black">Title</span>
            </div>
            <div class="awp_box_content bg-metro-light" style="text-align:left !important;">
                <asp:Button Text="Test" runat="server" ID ="tester" OnClick="tester_Click" />
            </div>
        </div>
    </div>
    </div>
   <div class="col-md-12">
        <span class="awp-save-btn bg-red fg-white shadow"><i onclick="javascript:alert('hello')" title="Add To Task List"  style="font-size:1em;" class="glyphicon glyphicon-floppy-disk av-hand-cursor"></i></span>
        <div class="awp_box rounded bg-metro-dark shadow">
            <div class="awp_box_title bg-metro-dark">
               <span class="fg-white shadow-metro-black">Title</span>
            </div>
            <div class="awp_box_content bg-metro-light" style="text-align:left !important;">
                                <div class="example" data-text="HAHAHA">
            <div class="set-border padding10 no-padding-top no-padding-bottom" style="vertical-align:middle;">
                <label class="input-control radio small-check">
                    <input type="radio" name="r1" value="default" checked="">
                    <span class="check"></span>
                    <span class="caption">Default</span>
                </label>
                <label class="input-control radio small-check">
                    <input type="radio" name="r1" value="list-type-icons">
                    <span class="check"></span>
                    <span class="caption">Icons</span>
                </label>
                <label class="input-control radio small-check">
                    <input type="radio" name="r1" value="list-type-tiles">
                    <span class="check"></span>
                    <span class="caption">Tiles</span>
                </label>
                <label class="input-control radio small-check">
                    <input type="radio" name="r1" value="list-type-listing">
                    <span class="check"></span>
                    <span class="caption">Listing</span>
                </label>
            </div>
            <br>
            <div class="listview set-border padding10 default" data-role="listview" id="lv1" data-on-list-click="alert('You select ' + list.find('.list-title').text())">
                <div class="list">
                    <img src="Images/folder-videos.png" class="list-icon">
                    <span class="list-title">Video</span>
                </div>
                <div class="list">
                    <img src="Images/folder-documents.png" class="list-icon">
                    <span class="list-title">Documents</span>
                </div>
                <div class="list">
                    <img src="Images/folder-downloads.png" class="list-icon">
                    <span class="list-title">Downloads</span>
                </div>
                <div class="list active">
                    <img src="Images/folder-images.png" class="list-icon">
                    <span class="list-title">Images</span>
                </div>
                <div class="list">
                    <img src="Images/folder-music.png" class="list-icon">
                    <span class="list-title">Music</span>
                </div>
            </div>
        </div>

            </div>
        </div>
    </div>
</div>
    -->

</asp:Content>
