<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="admin_usermanager.aspx.cs" Inherits="CCMS_editor.admin_usermanager" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        Users<asp:DropDownList ID="usermanager_userlist" runat="server" AutoPostBack="True"
            OnSelectedIndexChanged="usermanager_userlist_SelectedIndexChanged">
        </asp:DropDownList><br />
        <asp:Label ID="lbl_adduser" runat="server" Text="Add new user"></asp:Label>
        <asp:Label ID="lbl_edituser" runat="server" Text="Edit existing user" Visible="False"></asp:Label><br />
        Login<asp:TextBox ID="user_login" runat="server"></asp:TextBox><br />
        Password<asp:TextBox ID="user_password" runat="server" OnTextChanged="TextBox2_TextChanged"></asp:TextBox><br />
        Full Name<asp:TextBox ID="user_fullname" runat="server"></asp:TextBox><br />
        Email<asp:TextBox ID="user_email" runat="server"></asp:TextBox><br />
        Active<asp:CheckBox ID="chk_user_isactive" runat="server" OnCheckedChanged="user_isactive_CheckedChanged"
            Text=" " /><br />
        perms<asp:TextBox ID="user_perms" runat="server"></asp:TextBox><br />
        [chkarray]
        <asp:CheckBoxList ID="user_perms_checkbox" runat="server">
        </asp:CheckBoxList><br />
        <asp:Panel ID="pnl_chkboxes" runat="server">
        </asp:Panel>
        <br />
        <asp:Button ID="btn_adduser" runat="server" Text="Add new user" />
        <asp:Button ID="btn_updateuser" runat="server" Text="Update user" Visible="False" OnClick="btn_updateuser_Click" /><br />
        <br />
        <asp:Label ID="Label1" runat="server" Height="151px" Text="Label" Width="266px"></asp:Label><br />
        <br />
    
    </div>
    </form>
</body>
</html>
