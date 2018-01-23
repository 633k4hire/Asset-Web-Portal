<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="av_sm_tile_template.ascx.cs" Inherits="Web_App_Master.Account.Templates.av_sm_tile_template" %>

<div class="tile tile-small-y bg-metro-light" data-role="tile"  onclick="NotifyCustom('<%# Eval("ItemName")%>','<%# Eval("AssetNumber")%>','wrench'); BarcodeScanned('<%# Eval("AssetNumber")%>');">
                <div class="tile-content bg-metro-light" >
                    <div class="image-container bg-metro-light" style="margin-top:25px">
                        <div class="frame">
                            <img src="<%# Eval("FirstImage")%>">
                        </div>
                        <div class="image-overlay op-green">
                            <div style="vertical-align:top; text-align:left"><%# Eval("Description")%></div>
                                
                        </div>
                    </div>
                    <span class="tile-badge fg-white bg-darkRed"><strong><%# Eval("AssetNumber")%></strong></span>
                    <div class="tile-label fg-white shadow-metro-black"><strong><%# Eval("ItemName")%></strong></div>
                </div>
            </div>     