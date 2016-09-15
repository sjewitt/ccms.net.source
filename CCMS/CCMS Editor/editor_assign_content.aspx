<%@ Page Language="C#" MasterPageFile="~/editor.Master" AutoEventWireup="true" CodeBehind="editor_assign_content.aspx.cs" Inherits="ccms.editor_assign_content" Title="Assign content" %>

<asp:Content ID="header1" ContentPlaceHolderID="onloadJavascripts" runat="server">
<script type="text/javascript">
    window.onload = function ()
    {
        if (window.opener != null) window.opener.location.reload();
    }
    </script>
</asp:Content>

<asp:Content ID="content1" ContentPlaceHolderID="EditActionPlaceholder" runat="server">

    <div>Choose content item to insert:</div>
    <asp:DropDownList ID="sel_content" runat="server" 
        onselectedindexchanged="sel_content_SelectedIndexChanged" 
        CausesValidation="True" AutoPostBack="True">
    </asp:DropDownList>
    <h2>Details:</h2>
    <div class="pure-g">
        <div class="pure-u-1-6">
            Status:
        </div>
        <div class="pure-u-5-6">
            <asp:Label ID="lbl_status" runat="server"></asp:Label>
        </div>
    </div>
    <div class="pure-g">
        <div class="pure-u-1-6">Title: </div>
        <div class="pure-u-5-6"><asp:Label ID="lbl_contentTitle" runat="server" Text="[title]"></asp:Label> (id=<asp:Label ID="lbl_id" runat="server" Text="0"></asp:Label>)</div>
    </div>

    <div class="pure-g">
        <div class="pure-u-1-6">Version count: </div>
        <div class="pure-u-5-6"><asp:Label ID="lbl_versionCount" runat="server" Text="0"></asp:Label></div>
    </div>

    <div class="pure-g">
        <div class="pure-u-1-6">Current active content:</div>
        
        <div class="pure-u-5-6">
            <div id="content_preview">
                <blockquote>
                    <asp:Label ID="lbl_contentBody" runat="server" Text="[content]"></asp:Label>
                </blockquote>
            </div>
        </div>
    </div>
     


        <div class="pure-g strip">
            <div class="pure-u">
              <asp:LinkButton ID="LinkButton1" runat="server" class="pure-button" 
                    onclick="LinkButton1_Click">Assign selected</asp:LinkButton>  
            </div>
            
      </div>

</asp:Content>
