<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="editor_create_page.aspx.cs" Inherits="ccms.editor_create_page" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:Panel ID="pnl_form" runat="server">
<table>
    <tr>
      <td>Name</td>
      <td>
          <asp:TextBox ID="TextBox1" runat="server">NewPage</asp:TextBox>
        </td>
    </tr>
    <tr>
      <td>Page Title</td>
      <td>
          <asp:TextBox ID="TextBox2" runat="server">PageTitle</asp:TextBox>
        </td>
    </tr>
    <tr>
      <td>Link text</td>
      <td>
          <asp:TextBox ID="TextBox3" runat="server">LinkText</asp:TextBox>
        </td>
    </tr>
    <tr>
      <td>Description</td>
      <td>
          <asp:TextBox ID="TextBox4" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
      <td>Keywords</td>
      <td>
          <asp:TextBox ID="TextBox5" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
      <td></td>
      <td>
          <asp:Button ID="btn_createPage" runat="server" Text="Create" />
        </td>
    </tr>
  </table>
</asp:Panel>
    </div>

    
    <asp:Panel ID="pnl_done" runat="server" Height="80px" Visible="False">
        [visible when page created]<br /> Add to viewtree<br /> Edit
        <br />
    </asp:Panel>
    <asp:Panel 
            ID="pnl_notLoggedIn" runat="server" Height="70px">
        [Visible when not logged in]</asp:Panel>

    
    </form>
</body>
</html>
