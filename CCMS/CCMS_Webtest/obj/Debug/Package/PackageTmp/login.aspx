<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="CCMS.login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body style="height: 213px">
    <form id="form1" runat="server">


    <asp:Panel ID="pnl_loginform" runat="server" Height="86px" Width="317px">
        <asp:Label ID="lbl_loggedIn" runat="server" Text="Logged in..." Visible="False"></asp:Label>
        <br />
        Username:<asp:TextBox ID="txtUsername" runat="server" Width="150px"></asp:TextBox><br />
        Password:<asp:TextBox ID="txtPassword" runat="server" TextMode="Password" Width="148px"></asp:TextBox><br />
        <asp:Button ID="btnLogin" runat="server" onclick="btnLogin_Click" Text="Login" />
    </asp:Panel>
    <asp:Panel ID="pnl_loggedIn" runat="server" Height="69px" Width="318px">
        Click to
        <asp:LinkButton ID="lnkBtn_logOut" runat="server">Log Out</asp:LinkButton>
    </asp:Panel>
    <br />
    <br />
    DEBUG:
        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/ccms.aspx">HyperLink</asp:HyperLink><br />
    <asp:Label ID="lblDebug" runat="server" Text="Label"></asp:Label>
    <br />
    <br />
    </form>
</body>
</html>
