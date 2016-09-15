<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="editor_create_content.aspx.cs" Inherits="ccms.create_content" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">

    <title>Create New Content</title>
    <!-- common edtor javascript functions -->
    <script type="text/javascript" src="scripts/common_functions.js"></script>
    
    <!-- datepicker javascripts -->
    <script src="scripts/datepicker/calendar.js" type="text/javascript"></script>    
    
    <!-- inline javascripts -->
    <script language="javascript" type="text/javascript">
    // <!CDATA[
    //.NET
    //function Text1_onclick(){}

    //Set calendar fields:
    window.onload = function() {
        //reload parent:
        window.opener.location.reload();

        //date picker
        calendar.set("newcontent_validfrom");
        calendar.set("newcontent_validtill");
    }
    // ]]>
    </script>

    <!-- CSS for date picker -->
    <link rel="stylesheet" type="text/css" href="styles/datepicker/calendar.css" />

    <!-- CSS for edit form -->
    <link rel="Stylesheet" type="text/css" href="styles/editstyles.css" />
    
</head>
<body>
    <form id="form1" runat="server">
    <div id="container">
        <h1>Create new content...</h1>
        <asp:Panel ID="pnl_createContentForm" runat="server" Visible="False">
            <table style="width: 228px; height: 123px;">
                <tr>
                    <td style="width: 238px">Name*</td>
                    <td style="width: 949px"><asp:TextBox ID="newcontent_name" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td style="width: 238px">Description</td>
                    <td style="width: 949px"><asp:TextBox ID="newcontent_description" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td style="width: 238px">Content Type</td>
                    <td style="width: 949px"><asp:DropDownList ID="dropdown_contentType" runat="server"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td style="width: 238px">Valid From</td>
                    <td style="width: 949px"><asp:TextBox ID="newcontent_validfrom" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td style="width: 238px">Valid Till</td>
                    <td style="width: 949px"><asp:TextBox ID="newcontent_validtill" runat="server" ontextchanged="newcontent_validtill_TextChanged"></asp:TextBox></td>
                </tr>
            </table>
        <asp:Button ID="newcontent_submit" runat="server" Text="Create new Content" OnClick="newcontent_submit_Click" Width="154px" />
        <br />
        <asp:Label ID="createcontent_statusmsg" runat="server" Height="23px" Width="152px"></asp:Label>&nbsp;<br />
        <asp:HyperLink ID="createcontent_editnew" runat="server" NavigateUrl="~/ccms.editor/editor_edit_content.aspx" Visible="False">Edit new content</asp:HyperLink>
        </asp:Panel>
        <asp:Panel ID="pnl_notLoggedIn" runat="server">
        NOT LOGGED IN
        </asp:Panel>
    </div>
    </form>
</body>
</html>
