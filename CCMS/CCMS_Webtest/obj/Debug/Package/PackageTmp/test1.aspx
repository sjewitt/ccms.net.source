<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="test1.aspx.cs" Inherits="ccms.test1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="Label1" runat="server" Height="59px" Text="Label1" Width="125px"></asp:Label><br />
        <br />
        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/login.aspx">Test</asp:HyperLink><br />
        <br />
        <asp:HyperLink ID="logoutlink" runat="server">logout</asp:HyperLink>&nbsp;</div>
    </form>
</body>
</html>
