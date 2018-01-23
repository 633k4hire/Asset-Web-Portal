<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Ribbon.ascx.cs" Inherits="Web_App_Master.Account.Ribbon" %>
 <!--Toolbar-->
    <div data-role="charm" data-position="left" id="file-menu-charm" class="charm left-side" style="opacity: 1; display: block; right: 0px; left: auto; margin-top: 50px; bottom: 0px; min-width:100%; width:100%;"><h1 class="text-light">Menu</h1><span class="charm-closer"></span></div>

            <div class="main-menu">
                <div class="fluent-menu" data-role="fluentmenu" >
                <ul class="tabs-holder">
                    <li class="special"  onclick="toggleMetroCharm('#file-menu-charm')"><a href="#">File</a></li>
                    <li class=""><a href="#tab_home">Home</a></li>
                    <li class=""><a href="#tab_mailings">Mailing</a></li>
                    <li class="active"><a href="#tab_folder">Folder</a></li>
                    <li><a href="#tab_view">View</a></li>
                </ul>

                <div class="tabs-content">
                    <div class="tab-panel" id="tab_home" style="display: none;">
                        <div class="tab-panel-group">
                            <div class="tab-group-content">
                                <button class="fluent-big-button">
                                    <span class="icon mif-envelop"></span>
                                    Create<br>message
                                </button>
                                <div class="tab-content-segment">
                                    <button class="fluent-big-button dropdown-toggle">
                                        <span class="icon mif-file-picture"></span>
                                        <span class="label">Create<br>element</span>
                                    </button>
                                    <ul class="d-menu" data-role="dropdown">
                                        <li><a href="#">Message</a></li>
                                        <li><a href="#">Event</a></li>
                                        <li><a href="#">Meeting</a></li>
                                        <li><a href="#">Contact</a></li>
                                    </ul>
                                </div>
                                <div class="tab-content-segment">
                                    <button class="fluent-big-button">
                                        <span class="mif-cancel"></span>
                                        <span class="label">Delete</span>
                                    </button>
                                </div>
                            </div>
                            <div class="tab-group-caption">Clipboard</div>
                        </div>
                        <div class="tab-panel-group">
                            <div class="tab-group-content">
                                <div class="tab-content-segment">
                                    <button class="fluent-button"><span class="mif-loop"></span>Replay</button>
                                    <button class="fluent-button"><span class="mif-infinite"></span>Replay all</button>
                                    <button class="fluent-button"><span class="mif-loop2"></span>Forward</button>
                                </div>
                                <div class="tab-content-segment">
                                    <button class="fluent-tool-button"><img src="images/Notebook-Save.png"></button>
                                    <button class="fluent-tool-button"><img src="images/Folder-Rename.png"></button>
                                    <button class="fluent-tool-button"><img src="images/Calendar-Next.png"></button>
                                </div>
                            </div>
                            <div class="tab-group-caption">Reply</div>
                        </div>
                        <div class="tab-panel-group">
                            <div class="tab-group-content">
                                <div class="input-control text">
                                    <input type="text">
                                    <button class="button"><span class="mif-search"></span></button>
                                </div>
                                <button class="fluent-button"><span class="icon-book on-left"></span>Address Book</button>
                                <div class="tab-content-segment">
                                    <button class="fluent-button dropdown-toggle">
                                        <span class="mif-filter on-left"></span>
                                        <span class="label">Mail Filters</span>
                                    </button>
                                    <ul class="d-menu" data-role="dropdown">
                                        <li><a href="#">Unread messages</a></li>
                                        <li><a href="#">Has attachments</a></li>
                                        <li class="divider"></li>
                                        <li><a href="#">Important</a></li>
                                        <li><a href="#">Broken</a></li>
                                    </ul>
                                </div>
                            </div>
                            <div class="tab-group-caption">Search</div>
                        </div>
                    </div>

                    <div class="tab-panel" id="tab_mailings" style="display: none;">
                        <div class="tab-panel-group">
                            <div class="tab-group-content">
                                <button class="fluent-big-button">
                                    <span class="icon mif-envelop"></span>
                                    Create<br>message
                                </button>
                                <div class="tab-content-segment">
                                    <button class="fluent-big-button dropdown-toggle">
                                        <span class="icon mif-file-picture"></span>
                                        <span class="label">Create<br>element</span>
                                    </button>
                                    <ul class="d-menu" data-role="dropdown">
                                        <li><a href="#">Message</a></li>
                                        <li><a href="#">Event</a></li>
                                        <li><a href="#">Meeting</a></li>
                                        <li><a href="#">Contact</a></li>
                                    </ul>
                                </div>
                                <div class="tab-content-segment">
                                    <button class="fluent-big-button">
                                        <span class="mif-cancel"></span>
                                        <span class="label">Delete</span>
                                    </button>
                                </div>
                            </div>
                            <div class="tab-group-caption">Clipboard</div>
                        </div>
                    </div>

                    <div class="tab-panel" id="tab_folder" style="display: block;">
                        <div class="tab-panel-group">
                            <div class="tab-group-content">
                                <div class="tab-content-segment">
                                    <button class="fluent-button"><span class="mif-loop"></span>Replay</button>
                                    <button class="fluent-button"><span class="mif-infinite"></span>Replay all</button>
                                    <button class="fluent-button"><span class="mif-loop2"></span>Forward</button>
                                </div>
                                <div class="tab-content-segment">
                                    <button class="fluent-tool-button"><img src="images/Notebook-Save.png"></button>
                                    <button class="fluent-tool-button"><img src="images/Folder-Rename.png"></button>
                                    <button class="fluent-tool-button"><img src="images/Calendar-Next.png"></button>
                                </div>
                            </div>
                            <div class="tab-group-caption">Reply</div>
                        </div>
                    </div>

                    <div class="tab-panel" id="tab_view" style="display: none;">
                        <div class="tab-panel-group">
                            <div class="tab-group-content">
                                <button class="fluent-big-button">
                                    <span class="icon mif-envelop"></span>
                                    Create<br>message
                                </button>
                                <div class="tab-content-segment">
                                    <button class="fluent-big-button dropdown-toggle">
                                        <span class="icon mif-file-picture"></span>
                                        <span class="label">Create<br>element</span>
                                    </button>
                                    <ul class="d-menu" data-role="dropdown">
                                        <li><a href="#">Message</a></li>
                                        <li><a href="#">Event</a></li>
                                        <li><a href="#">Meeting</a></li>
                                        <li><a href="#">Contact</a></li>
                                    </ul>
                                </div>
                                <div class="tab-content-segment">
                                    <button class="fluent-big-button">
                                        <span class="mif-cancel"></span>
                                        <span class="label">Delete</span>
                                    </button>
                                </div>
                            </div>
                            <div class="tab-group-caption">Clipboard</div>
                        </div>
                    </div>
                </div>
            </div>
            </div>
