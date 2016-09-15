<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ORIGeditor_assign_content.aspx.cs" Inherits="ccms.editor_assign_content" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script src="Scripts/editscripts.js" type="text/javascript"></script>
    <link rel="Stylesheet" type="text/css" href="styles/editstyles.css" />
    <script type="text/javascript">
        window.onload = function () {
            if(window.opener!=null) window.opener.location.reload();
        }
    </script>
    <title></title>
</head>
<body style="height: 35px">
    <form id="form1" runat="server">
    <div>
    
        Choose content item to insert:</div>
    <asp:DropDownList ID="sel_content" runat="server" 
        onselectedindexchanged="sel_content_SelectedIndexChanged" 
        CausesValidation="True" AutoPostBack="True">
    </asp:DropDownList>
    <p>Details:</p>
    Status:
    <asp:Label ID="lbl_status" runat="server"></asp:Label>
    <br />
    Title: <asp:Label ID="lbl_contentTitle" runat="server" Text="[title]"></asp:Label> (id=<asp:Label ID="lbl_id" runat="server" Text="0"></asp:Label>)<br />
    Version count: <asp:Label ID="lbl_versionCount" runat="server" Text="0"></asp:Label><br />
    Current active content:
    <div id="content_preview"><asp:Label ID="lbl_contentBody" runat="server" Text="[content]"></asp:Label></div>
    
     <asp:Button ID="Button1" runat="server" onclick="Button1_Click" 
        Text="Assign selected" />
    
     </form>
</body>
</html>
