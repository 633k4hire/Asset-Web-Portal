<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="av_list_template.ascx.cs" Inherits="Web_App_Master.Account.Templates.av_list_template" %>

      <div id="av_dynamic_column" class="col-md-12" onclick="NotifyCustom('<%# Eval("ItemName")%>','<%# Eval("AssetNumber")%>','wrench'); BarcodeScanned('<%# Eval("AssetNumber")%>');">
    <div class=" av-lv rounded bg-metro-dark shadow" >

        <div class=" av-lv-thumb"><img id="av_list_img" src="<%# Eval("FirstImage")%>" style=" height:100%;"></img></div>
        <div class="av-lv-seperator"></div>    
                        <div class="av-lv-content">
                            <strong><%# Eval("ItemName")%> - <%# Eval("Description")%>
                            </strong>
       </div>

         <span class="av-lv-badge bg-red fg-white shadow"><strong><%# Eval("AssetNumber")%></strong></span>
         

    </div>
          </div>


