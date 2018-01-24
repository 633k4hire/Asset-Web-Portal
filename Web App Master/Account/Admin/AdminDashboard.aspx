<%@ Page Title="Admin Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" ValidateRequest="false" EnableEventValidation="false" CodeBehind="AdminDashboard.aspx.cs" Inherits="Web_App_Master.Account.Admin.AdminDashboard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="app-bar" style="margin-top:-1px !important; left:0px!important; position:fixed !important; z-index:88000;">
    
    <ul class="app-bar-menu">
        <li><h1>Administration</h1> </li>
        <li>
            <asp:LinkButton ID ="UsersAndRolesBtn" OnClick="UsersAndRolesBtn_Click" ToolTip="User Administration" ClientIDMode="Static" runat="server"><span class="mif-cog mif-3x"></span></asp:LinkButton>
        </li>
        <li>
            <asp:LinkButton ID ="AssetAdminBtn" OnClick="AssetAdminBtn_Click" ToolTip="Library Options" ClientIDMode="Static" runat="server"><span class="mif-books mif-3x"></span></asp:LinkButton>
        </li>
        <li>
            <asp:LinkButton ID ="AssetPeopleManager" OnClick="AssetPeopleManager_Click" ToolTip="Personnel" ClientIDMode="Static" runat="server"><span class="mif-users mif-3x"></span></asp:LinkButton>
        </li>
        <li>
            <asp:LinkButton ID ="CustomerManager" OnClick="CustomerManager_Click" ToolTip="Customers" ClientIDMode="Static" runat="server"><span class="mif-contacts-mail mif-3x"></span></asp:LinkButton>
        </li>
       <li>
            <asp:LinkButton ID ="NotificationManagerBtn" OnClick="NotificationManagerBtn_Click" ToolTip="Notification Manager" ClientIDMode="Static" runat="server"><span class="mif-bell mif-3x"></span></asp:LinkButton>
        </li>
        <li>

        </li>
       
        
        
    </ul>
        <asp:PlaceHolder ID ="ApplyChangesButton" runat="server" Visible="true">
   <ul class="app-bar-menu place-right">
            <li title="Test Mode">
                <span>
                
            <label class="awp-switch">
                <input title="Test Mode" id="TestModeSwitch"  type="checkbox" checked="checked" onchange="TestModeChanged()">
                <span class="check"></span>
            </label>                   
            &nbsp;&nbsp;</span></li>
    </ul>
            </asp:PlaceHolder>
</div>
    
    <div style="padding-top:50px"></div>
    <!--Multiview-->
     <asp:PlaceHolder ID ="MessagePlaceHolder" ClientIDMode="Static" runat="server" Visible="false">
        <div class="row">
                <div class="col-md-12">
                    <div class="awp_box rounded bg-metro-dark shadow">
                        <div class="awp_box_title bg-metro-dark">
                           <span class="fg-white shadow-metro-black"><span class="mif-warning mif-ani-flash mif-ani-slow"></span></span>
                        </div>
                        <div class="awp_box_content bg-red fg-white shadow-metro-black">
                            <asp:Literal ID="ErrorMsg" runat="server"></asp:Literal>
                       </div>
                    </div>
                </div>
            </div>

    </asp:PlaceHolder>
   <asp:MultiView ID="AdminMultiView" ActiveViewIndex="0" runat="server" EnableViewState="true" ViewStateMode="Enabled">

        <asp:View ID="RolesAndUsersAdminView"  runat="server">

             <div class="row">
         <div class="col-md-12">
       
         </div>
    </div>
            <!--row end-->
            <div class="row">
                 <div class="col-md-8">
                    <div class="awp_box rounded bg-metro-dark shadow">
           
                    <div class="awp_box_title bg-metro-dark">
                       <span class="fg-white shadow-metro-black">Roles</span>
                    </div>
                    <div class="awp_box_content bg-metro-light">
                        <asp:UpdatePanel ChildrenAsTriggers="true" ID="RolesUpdatePanel" ClientIDMode="Static" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                                <asp:Repeater ID="RolesAndUsersRepeater" OnItemDataBound="RolesAndUsersRepeater_ItemDataBound" ClientIDMode="Static" runat="server" ViewStateMode="Enabled" EnableViewState="true">
                            <HeaderTemplate>
                               <div class="panel-group" id="rolesAccordion">

                            </HeaderTemplate>
                            <ItemTemplate>
                              <div class="panel panel-default">
                                  <div class="panel-heading">
                                    <h4 class="panel-title text-left">
                                      <a data-toggle="collapse" data-parent="#rolesAccordion" href='#collapse<%# Container.ItemIndex + 1%>'>
                                          <strong><%# Eval("RoleName")%></strong>
                                      </a>
                                    </h4>
                                  </div>
                                  <div id='collapse<%# Container.ItemIndex + 1%>' class="panel-collapse collapse">
                                    <div class="panel-body text-left">
                                        <!--Repeaters for users of this role-->
                                        <asp:Repeater DataSource='<%# Eval("RoleUsers")%>' runat="server">
                                            <ItemTemplate>
                                                <div class="row bg-metro">
                                           
                                                        <div class="col-sm-12  text-left" style="width:auto !important">
                                                            <asp:Button ToolTip="Delete User From Role" ID="DeleteFromRole" CssClass="btn btn-primary btn-sm" runat="server" Text="X" Font-Bold="true" CommandName='<%#Eval("UserId")%>' CommandArgument='<%#Eval("RoleId") %>' OnCommand="DeleteFromRole_Command" />
                                                    
                                                            <asp:DropDownList ClientIDMode="Static" ID='RoleDropDown' AppendDataBoundItems="true" runat="server" DataSource='<%#GetRoleNames() %>'  CssClass="dropdown-button">
                                                                <asp:ListItem Text="--Select One--" Value="" /> 
                                                            </asp:DropDownList>
                                                            <asp:Button ToolTip="Copy User To ROle" ID="CopyRole" CssClass="btn btn-primary btn-sm" runat="server" Text="Copy" Font-Bold="true" CommandName='<%#Eval("UserId")%>' CommandArgument='<%#Eval("RoleId") %>' OnCommand="CopyRole_Command" />
                                                            <asp:Button ToolTip="Change User Role" ID="ChangeRole" CssClass="btn btn-primary btn-sm" runat="server" Text="Change" Font-Bold="true" CommandName='<%#Eval("UserId")%>' CommandArgument='<%#Eval("RoleId") %>' OnCommand="ChangeRole_Command" />
                                                        <strong><%# Eval("UserName")%></strong> 
                                                        </div>                                                
                                              
                                                 
                                            
                                               </div>
                                     
                                            </ItemTemplate>
                                        </asp:Repeater>

                                    </div>
                                  </div>
                              </div>

                            </ItemTemplate>
                            <FooterTemplate>
                                </div>
                            </FooterTemplate>
                        </asp:Repeater>

                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                 </div>
                   <div class="col-md-4">
                <div class="awp_box rounded bg-metro-dark shadow">
           
                    <div class="awp_box_title bg-metro-dark">
                       <span class="fg-white shadow-metro-black">Users</span>
                    </div>
                    <div class="awp_box_content bg-metro-light">
                        <asp:UpdatePanel ID="UserUpdatePanel" ChildrenAsTriggers="true" ClientIDMode="Static" UpdateMode="Conditional" runat="server">
                            <ContentTemplate>
                               <asp:Repeater ID ="UserRepeater" ClientIDMode="Static" runat="server">
                                   <ItemTemplate>
                                       <div class="row  bg-metro-dark">
                                           
                                           <div class="col-sm-12">
                                              <asp:Button ToolTip="Delete User" ID="DeleteUser" CssClass="btn btn-primary btn-sm" runat="server" Text="X" Font-Bold="true" CommandName='<%#Eval("UserId")%>' CommandArgument='<%#Eval("RoleId") %>' OnCommand="DeleteUser_Command" />
                                               <strong><%# Eval("UserName")%></strong>
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
                        
            <!--row end-->   
        </asp:View>
        <asp:View ID="AssetAdminView" runat="server">
            <div class="awp-side-stuck">
        <ul class="t-menu compact bg-metro-dark">
                            <li><a href="#LibraryOptionsBox" title="Library Options"><span class="glyphicon glyphicon-th md-glyph"></span></a></li>
                            <li><a href="#StandardMsgBox" title="Standard Messages"><span class="glyphicon glyphicon-list-alt md-glyph"></span></a></li>
                            <li><a href="#UPSbox" title="UPS Account"><span class="glyphicon glyphicon-globe md-glyph"></span></a></li>
           </ul>    
    </div>
            <div id="LibraryOptionsBox" class="row">
                <div class =" col-md-12">
                    <div class="awp_box rounded bg-metro-dark shadow">
                        <div class="awp_box_title bg-metro-dark">
                             <span class="fg-white shadow-metro-black">Library Options</span>
                        </div>
                        <div class="awp_box_content bg-metro-light">
                             <div class="row">
                                       <asp:PlaceHolder runat="server" ID="MessageHolder" Visible="false">
                                           <div class="col-md-12"><p class="fg-red text-bold" >
                                                <asp:Literal runat="server" ID="Notification" />
                                            </p></div>                        
                                        </asp:PlaceHolder>
                                        </div>
                             <div class="row">
                                        <div class="col-md-3">
                                            <div class="awp_box rounded bg-metro-dark shadow">
           
                                                <div class="awp_box_title bg-metro-dark">
                                                   <span class="fg-white shadow-metro-black">Upload Library</span>
                                                </div>
                                                <div class="awp_box_content bg-metro-light">
                                                          <asp:FileUpload ID="LibraryUploader" runat="server" Width="250px" />
                                                         <asp:Button  OnClientClick="ShowLoader()"  ID="UploadLibrary" ClientIDMode="Static" CssClass="" runat="server" Text="Upload Asset File" OnClick="UploadLibrary_Click" />
                                                </div>
                                            </div>
                                       </div>
                                       <div class="col-md-3">
                                            <div class="awp_box rounded bg-metro-dark shadow">
           
                                                <div class="awp_box_title bg-metro-dark">
                                                   <span class="fg-white shadow-metro-black">Upload Notices</span>
                                                </div>
                                                <div class="awp_box_content bg-metro-light">
                                                    <asp:FileUpload ID="NoticeUploader" runat="server" Width="250px" />
                                                    <asp:Button OnClientClick="ShowLoader()"  ID="UploadNotices" ClientIDMode="Static" CssClass="" runat="server" Text="Upload Notices File" OnClick="UploadNotices_Click" />
                                                </div>
                                            </div>
                                       </div>
                                       <div class="col-md-3">
                                            <div class="awp_box rounded bg-metro-dark shadow">
           
                                                <div class="awp_box_title bg-metro-dark">
                                                   <span class="fg-white shadow-metro-black">Upload History</span>
                                                </div>
                                                <div class="awp_box_content bg-metro-light">
                                                    <asp:FileUpload ID="HistoryUploader" runat="server" Width="250px" />
                                                    <asp:Button OnClientClick="ShowLoader()"  ID="UploadHistory" ClientIDMode="Static" CssClass="" runat="server" Text="Upload History File" OnClick="UploadHistory_Click" />
                                                </div>
                                            </div>
                                       </div>
                                       <div class="col-md-3">
                                            <div class="awp_box rounded bg-metro-dark shadow">
           
                                                <div class="awp_box_title bg-metro-dark">
                                                   <span class="fg-white shadow-metro-black">Test Mode</span>
                                                </div>
                                                <div class="awp_box_content bg-metro-light">
                                                    
                                                    
                                                &nbsp;&nbsp;&nbsp;</div>
                                            </div>
                                       </div>
                                           </div>
                             <div class="row">
                                        <div class="col-md-12">
                                            <div class="awp_box rounded bg-metro-dark shadow">
           
                                                <div class="awp_box_title bg-metro-dark">
                                                   <span class="fg-white shadow-metro-black">SQL Options</span>
                                                </div>
                                                <div class="awp_box_content bg-metro-light">
                                                    <asp:Button OnClientClick="ShowLoader()" ID="SendSQL" CssClass="button" ClientIDMode="Static" runat="server" Text="Asset Library to SQL" OnClick="SendSQL_Click" />
                                                    <asp:Button OnClientClick="ShowLoader()"  ID="SendSettingsSQL" CssClass="button"  ClientIDMode="Static" runat="server" Text="Settings To SQL" OnClick="SendSettingsSQL_Click" />
                                                    <asp:Button OnClientClick="ShowLoader()"  ID="PullSettings" CssClass="button"  ClientIDMode="Static" runat="server" Text="SQL To Settings" OnClick="PullSettings_Click" />
                                                    <asp:Button OnClientClick="ShowLoader()"  ID="PullSQL" CssClass="button"  ClientIDMode="Static" runat="server" Text="SQL to Asset Library" OnClick="PullSQL_Click" />
                                                    <asp:Button OnClientClick="ShowLoader()"  ID="DeleteSQL" CssClass="button"  ClientIDMode="Static" runat="server" Text="Erase SQL DataBase" OnClick="DeleteSQL_Click" />
                                                    <asp:Button OnClientClick="ShowLoader()"  ID="DeleteSettingsSQL" CssClass="button"  ClientIDMode="Static" runat="server" Text="Erase Settings SQL" OnClick="DeleteSettingsSQL_Click" />
                                                </div>
                                            </div>
                                       </div>
                                       </div>
                                          
                        </div>
                    </div>
                </div>
            </div>
        
                <div id="StandardMsgBox"  class="row">
                 <div class="col-md-12">
                    <div class="awp_box rounded bg-metro-dark shadow">
           
                    <div class="awp_box_title bg-metro-dark">
                       <span class="fg-white shadow-metro-black">Standard Messages</span>
                    </div>
                    <div class="awp_box_content bg-metro-light">
                       <div class="col-md-12">
                            <span class="awp-save-btn bg-red fg-white shadow">
                                <asp:LinkButton runat="server" ID="SaveCheckOutMsgBtn" CssClass="fg-white" ClientIDMode="Static" OnClick="SaveCheckOutMsgBtn_Click">
                                <i title="Save"  style="font-size:1em;" class="glyphicon glyphicon-floppy-disk av-hand-cursor"></i></asp:LinkButton>
                            </span>
                        <div class="awp_box rounded bg-metro-dark shadow">           
                            <div class="awp_box_title bg-metro-dark">
                               <span class="fg-white shadow-metro-black">Check Out Message</span>
                            </div>
                            <div class="awp_box_content bg-metro-light">
                                <div class=" input-control textarea av-input"><textarea id="checkoutmsgbox" runat="server" ></textarea></div>
                            </div>
                        </div>
                    </div>
                       <div class="col-md-12">
                            <span class="awp-save-btn bg-red fg-white shadow">
                                <asp:LinkButton runat="server" ID="SaveCheckInMsgBtn" CssClass="fg-white" ClientIDMode="Static" OnClick="SaveCheckInMsgBtn_Click">
                                <i title="Save"  style="font-size:1em;" class="glyphicon glyphicon-floppy-disk av-hand-cursor"></i></asp:LinkButton>
                            </span>
                        <div class="awp_box rounded bg-metro-dark shadow">           
                            <div class="awp_box_title bg-metro-dark">
                               <span class="fg-white shadow-metro-black">Check In Message</span>
                            </div>
                            <div class="awp_box_content bg-metro-light">
                                <div class=" input-control textarea av-input"><textarea id="checkinmsgbox" runat="server" ></textarea></div>
                            </div>
                        </div>
                    </div>
                       <div class="col-md-12">
                            <span class="awp-save-btn bg-red fg-white shadow">
                                <asp:LinkButton runat="server" ID="SaveNoticMsgBtn" CssClass="fg-white" ClientIDMode="Static" OnClick="SaveNoticMsgBtn_Click">
                                <i title="Save"  style="font-size:1em;" class="glyphicon glyphicon-floppy-disk av-hand-cursor"></i></asp:LinkButton>
                            </span>
                        <div class="awp_box rounded bg-metro-dark shadow">           
                            <div class="awp_box_title bg-metro-dark">
                               <span class="fg-white shadow-metro-black">Notification Message</span>
                            </div>
                            <div class="awp_box_content bg-metro-light">
                                <div class=" input-control textarea av-input"><textarea id="notificationmsgbox" runat="server" ></textarea></div>
                            </div>
                        </div>
                    </div>                        
                       <div class="col-md-12">
                            <span class="awp-save-btn bg-red fg-white shadow">
                                <asp:LinkButton runat="server" ID="SaveShipperMsgBtn" CssClass="fg-white" ClientIDMode="Static" OnClick="SaveShipperMsgBtn_Click">
                                <i title="Save"  style="font-size:1em;" class="glyphicon glyphicon-floppy-disk av-hand-cursor"></i></asp:LinkButton>
                            </span>
                        <div class="awp_box rounded bg-metro-dark shadow">           
                            <div class="awp_box_title bg-metro-dark">
                               <span class="fg-white shadow-metro-black">Shipper Message</span>
                            </div>
                            <div class="awp_box_content bg-metro-light">
                                <div class=" input-control textarea av-input"><textarea id="shipmsgbox" runat="server" ></textarea></div>
                            </div>
                        </div>
                    </div>
                 </div>
                </div>

                 </div>

            </div>
                <div id="UPSbox"  class="row">
                 <div class="col-md-12">
                    <!--BUTTONS-->
                    <span class="awp-save-btn bg-red fg-white shadow">
                        <asp:LinkButton runat="server" ID="SaveUpsAcctBtn" CssClass="fg-white" ClientIDMode="Static" OnClick="SaveUpsAcctBtn_Click">
                        <i title="Save"  style="font-size:1em;" class="glyphicon glyphicon-floppy-disk av-hand-cursor"></i></asp:LinkButton>
                    </span>
                    <!--BOX-->
                     <div class="awp_box rounded bg-metro-dark shadow">           
                    <div class="awp_box_title bg-metro-dark">
                       <span class="fg-white shadow-metro-black">UPS Account</span>
                    </div>
                    <div class="awp_box_content bg-metro-light">
                        <div class="row">
                            <div class="col-md-3">
                                <div class="awp_box rounded bg-metro-dark shadow">           
                                    <div class="awp_box_title bg-metro-dark">
                                       <span class="fg-white shadow-metro-black">Access License Number</span>
                                    </div>
                                    <div class="awp_box_content bg-metro-light">
                                        <div class=" input-control text ">                                    
                                             <input runat="server" id="ups_aln" type="text" class="av-input" placeholder="Access License #..." />
                                        </div>
                                    </div>
                               </div>
                           </div>
                            <div class="col-md-3">
                                <div class="awp_box rounded bg-metro-dark shadow">           
                                    <div class="awp_box_title bg-metro-dark">
                                       <span class="fg-white shadow-metro-black">User ID</span>
                                    </div>
                                    <div class="awp_box_content bg-metro-light">
                                        <div class=" input-control text ">                                    
                                             <input runat="server" id="ups_userid" type="text" class="av-input" placeholder="User ID..." />
                                        </div>
                                    </div>
                               </div>
                           </div>
                            <div class="col-md-3">
                                <div class="awp_box rounded bg-metro-dark shadow">           
                                    <div class="awp_box_title bg-metro-dark">
                                       <span class="fg-white shadow-metro-black">Password</span>
                                    </div>
                                    <div class="awp_box_content bg-metro-light">
                                        <div class=" input-control text ">                                    
                                             <input runat="server" id="ups_pwd" type="password" class="av-input" placeholder="Password..." />
                                        </div>
                                    </div>
                               </div>
                           </div>
                            <div class="col-md-3">
                                <div class="awp_box rounded bg-metro-dark shadow">           
                                    <div class="awp_box_title bg-metro-dark">
                                       <span class="fg-white shadow-metro-black">Shipper Number</span>
                                    </div>
                                    <div class="awp_box_content bg-metro-light">
                                        <div class=" input-control text ">                                    
                                             <input runat="server" id="ups_shippernumber" type="text" class="av-input" placeholder="Shipper #" />
                                        </div>
                                    </div>
                               </div>
                           </div>


                        </div>
                    </div>
                </div>
                 </div>

            </div>


        

                

        </asp:View>
        <asp:View ID ="PersonnelView" runat="server">                 

            <div id="ShippingPersonBox"  class="row">
                    <div class =" col-md-12">
                        <div class="awp_box rounded bg-metro-dark shadow">           
                            <div class="awp_box_title bg-metro-dark">
                               <span class="fg-white shadow-metro-black">Shipping Personel</span>
                            </div>
                            <div class="awp_box_content bg-metro-light">                                
                                        <asp:Repeater ID="ShippingPersonRepeater" ClientIDMode="Static" runat="server">
                                            <ItemTemplate>
                                                <div class="row bg-metro">                                           
                                                        <div class="col-sm-12" style="width:auto !important; text-align:left !important">
                                                            <asp:Button ToolTip="Delete" ID="DeleteShippingPersonBtn" CssClass="btn btn-primary btn-sm" runat="server" Text="X" Font-Bold="true" CommandName='<%#Eval("Email")%>'  OnCommand="DeleteShippingPersonBtn_Command" />
                                                            <strong><%# Eval("Name")%>-<%# Eval("Email")%></strong></div>  
                                               </div>
                                     
                                            </ItemTemplate>
                                        </asp:Repeater>
                         
                            </div>
                        </div>

                    </div>
                                </div>
            <div id="EngineerBox"  class="row">
                     <div class =" col-md-12">
                        <div class="awp_box rounded bg-metro-dark shadow">           
                            <div class="awp_box_title bg-metro-dark">
                               <span class="fg-white shadow-metro-black">Service Engineers</span>
                            </div>
                            <div class="awp_box_content bg-metro-light">
                                <asp:Repeater ID="EngineerRepeater" runat="server">
                                            <ItemTemplate>
                                                <div class="row bg-metro">                                           
                                                        <div class="col-sm-12" style="width:auto !important; text-align:left !important">
                                                            <asp:Button ToolTip="Delete" CommandName="DeleteEngineer" ID="DeleteEngineer" CssClass="btn btn-primary btn-sm" runat="server" Text="X" Font-Bold="true" CommandArgument='<%# Eval("Name") %>' OnCommand="DeleteEngineer_Command" />
                                                            <strong><%# Eval("Name")%>-<%# Eval("Email")%></strong></div>  
                                               </div>
                                     
                                            </ItemTemplate>
                                        </asp:Repeater>
                            </div>
                        </div>

                    </div>
                </div>
       </asp:View>
        <asp:View ID ="CustomerView" runat="server">
          
                <div id="CustomersBox"  class="row">
                        <div class =" col-md-12">
                        <div class="awp_box rounded bg-metro-dark shadow"> 
                            <span class="awp-bar " style="padding-bottom:-10px!important;">
                                 <span class="awp-bar-btn bg-green fg-white shadow"><i onclick="" title="Add To Task List" class="mif-previous av-hand-cursor"></i></span>
                                 <span class="awp-bar-btn bg-green fg-white shadow"><i onclick="" title="Add To Task List" class="mif-next av-hand-cursor"></i></span>
                                 <span class="awp-bar-btn bg-green fg-white shadow"><i onclick="" title="Add To Task List" class="mif-next av-hand-cursor"></i></span>
                                 <span class="awp-bar-btn bg-green fg-white shadow"><i onclick="" title="Add To Task List" class="mif-next av-hand-cursor"></i></span>
                            </span>                          
                            <div class="awp_box_title bg-metro-dark">
                               <span class="fg-white shadow-metro-black">Customers</span>
                            </div>
                            <div class="awp_box_content bg-metro-light">
                                        <asp:Repeater ID="CustomerRepeater" ClientIDMode="Static" runat="server" OnItemCommand="CustomerRepeater_ItemCommand">
                                            <ItemTemplate>
                                                <div class="panel-group">
                                                  <div class="panel panel-default">
                                                    <div class="panel-heading">
                                                      <h4 class="panel-title">
                                                        <a data-toggle="collapse" href='#collapse<%# Container.ItemIndex + 1%>'><%# Eval("CompanyName")%></a>
                                                      </h4>
                                                    </div>
                                                    <div id='collapse<%# Container.ItemIndex + 1%>' class="panel-collapse collapse">
                                                      <div class="panel-body">
                                                          <div class="row">
                                                            <div class="col-md-4">
                                                            <address>
                                                              <strong><%# Eval("CompanyName") %></strong><br>
                                                              <%# Eval("Address") %><br>
                                                                <%# Eval("Address2") %><br>
                                                                <%# Eval("Address3") %><br>
                                                              <%# Eval("City") %>, <%# Eval("State") %> <%# Eval("Postal") %><br>
                                                                <%# Eval("Country") %><br>
                                                              <abbr title="Phone">P:</abbr> <%# Eval("Phone") %>
                                                            </address>
                                                            <address>
                                                              <strong><%# Eval("Attn") %></strong><br>
                                                              <a href='mailto:<%# Eval("Email") %>'><%# Eval("Email") %></a>
                                                            </address>
                                                            </div>
                                                          </div>
                                                      </div>
                                                      <div class="panel-footer">
                                                            <asp:Button CommandName="DeleteCustomer" ToolTip="Delete" ID="DeleteEngineer" CssClass="btn btn-primary btn-sm" runat="server" Text="X" Font-Bold="true" CommandArgument='<%# Eval("CompanyName") +"|"+ Eval("Address") %>'/>                                                         
                                                      </div>
                                                    </div>
                                                  </div>
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                            </div>
                        </div>

                    </div>
            </div>

       </asp:View>
        <asp:View ID ="NotificationsView" runat="server">
                <div id="NotificationsBox"  class="row">
                    <div class="col-md-12">
                         <div class="awp_box rounded bg-metro-dark shadow">
           
                            <div class="awp_box_title bg-metro-dark">
                               <span class="fg-white shadow-metro-black">Pending Notifications</span>
                            </div>
                            <div class="awp_box_content bg-metro-light">
                                <!--Repeaters for 30 Day Notices-->
                                        <asp:Repeater ID="Notice30DayRepeater" ClientIDMode="Static" runat="server">
                                            <ItemTemplate>
                                                <div class="row bg-metro">                                           
                                                        <div class="col-sm-12" style="width:auto !important; text-align:left">
                                                            <asp:Button ToolTip="Delete Notice" ID="DeleteNotice30DayBtn" CssClass="btn btn-primary btn-sm" runat="server" Text="X" Font-Bold="true" CommandName='<%#Eval("NoticeControlNumber")%>' CommandArgument='<%#Eval("GUID")%>'  OnCommand="DeleteNotice30DayBtn_Command" />
                                                            <asp:Button ToolTip="Send Notice" ID="SendNotice30DayBtn" CssClass="btn btn-primary btn-sm" runat="server" Text="Send" Font-Bold="true" CommandName='<%#Eval("NoticeControlNumber")%>' CommandArgument='<%#Eval("GUID")%>' OnCommand="SendNotice30DayBtn_Command" />
                                                        <strong><%# Eval("NoticeControlNumber")%></strong> &nbsp-&nbsp Scheduled in&nbsp<strong><%# Eval("DaysUntil").ToString()%>&nbsp Days</strong> &nbsp-&nbsp 
                                                            <strong><%# Eval("Name")%></strong>
                                                        </div>  
                                                        
                                               </div>
                                     
                                            </ItemTemplate>
                                        </asp:Repeater>
                            </div>
                         </div>
                       </div>       
                     
                </div>

       </asp:View>
    </asp:MultiView>

</asp:Content>
