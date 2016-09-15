<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="editor_remove_content.aspx.cs" Inherits="ccms.editor_remove_content" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<script type="text/javascript">
    window.onload = function () {
        //refresh the parent window:
        window.opener.location.reload();
    }
</script>
    <title>Remove Content</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
    <p>
        Do you want to remove content &#39;&#39; from this page?</p>
    <p>
        <asp:Label ID="lbl_result" runat="server"></asp:Label>
    </p>
    <p>
        <asp:Button ID="Button1" runat="server" Text="Remove" onclick="Button1_Click" />
        &nbsp;
        <asp:Button ID="Button2" runat="server" Text="Cancel" />
    </p>
    </form>
    <p>
        &nbsp;</p>
</body>
</html>
