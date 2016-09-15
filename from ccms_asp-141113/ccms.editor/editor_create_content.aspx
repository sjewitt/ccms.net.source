<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="editor_create_content.aspx.cs" Inherits="CCMS_editor._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Create Content</title>
    <!-- common edtor javascript functions -->
    <script type="text/javascript" src="scripts/common_functions.js"></script>
    
    <!-- datepicker javascripts -->
    <script type="text/javascript" src="datepicker/calendar.js"></script>    
    
    <!-- inline javascripts -->
    <script language="javascript" type="text/javascript">
    // <!CDATA[
    //.NET
    function Text1_onclick(){}

    //Set calendar fields:
    window.onload = function()
    {
        //date picker
        calendar.set("newcontent_validfrom");
        calendar.set("newcontent_validtill");
    }
    // ]]>
    </script>

    <!-- CSS for date picker -->
    <link rel="stylesheet" type="text/css" href="datepicker/calendar.css" />

    <!-- CSS for edit form -->
    <link rel="Stylesheet" type="text/css" href="styles/editstyles.css" />
    
</head>
<body>
    <form id="form1" runat="server">
    <div id="container">
        <h1>
            Create new content</h1>
        <table style="width: 228px; height: 123px;">
            <tr>
                <td style="width: 238px">
                    Name*</td>
                <td style="width: 949px">
        <asp:TextBox ID="newcontent_name" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="width: 238px">
                    Description</td>
                <td style="width: 949px">
        <asp:TextBox ID="newcontent_description" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="width: 238px">
                    Content Type</td>
                <td style="width: 949px">
        <asp:DropDownList ID="dropdown_contentType" runat="server">
        </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="width: 238px">
                    Valid From</td>
                <td style="width: 949px">
        <asp:TextBox ID="newcontent_validfrom" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="width: 238px">
                    Valid Till</td>
                <td style="width: 949px">
        <asp:TextBox ID="newcontent_validtill" runat="server"></asp:TextBox></td>
            </tr>
        </table>
        &nbsp;<asp:Button ID="newcontent_submit" runat="server" Text="Create new Content" OnClick="newcontent_submit_Click" Width="154px" />
        <br />
        <asp:Label ID="createcontent_statusmsg" runat="server" Height="23px" Width="152px"></asp:Label>&nbsp;<br />
        <asp:HyperLink ID="createcontent_editnew" runat="server" NavigateUrl="~/editor_edit_content.aspx" Visible="False">Edit new content</asp:HyperLink></div>
    </form>
</body>
</html>
