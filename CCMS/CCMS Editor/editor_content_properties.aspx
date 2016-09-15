<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="editor_content_properties.aspx.cs" Inherits="ccms.editor_content_properties1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Edit Content Properties</title>

    <!-- common edtor javascript functions -->
    <script type="text/javascript" src="scripts/common_functions.js"></script>

    <!-- date picker CSS -->
    <link rel='stylesheet' type='text/css' href='styles/datepicker/calendar.css' />

    <!-- datepicker javascripts -->
    <script src="scripts/datepicker/calendar.js" type="text/javascript"></script> 
    <script type="text/javascript">
        window.onload = function () 
        {
            //reload parent:
            if (window.opener != null) window.opener.location.reload();

            //date picker
            calendar.set("txt_validFrom");
            calendar.set("txt_validTo");
        }
        function clearField(elemId)
        {

            var elem = document.getElementById(elemId);
            alert(elem.value);
            elem.value = ""; 
        }

    </script>


</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table>
        <tr>
            <td>Name</td>
            <td class="style1">
                <asp:TextBox ID="txt_name" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>Description</td>
            <td class="style1">
                <asp:TextBox ID="txt_description" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>Content Type</td>
            <td class="style1">
                <asp:DropDownList ID="sel_contentType" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>Valid From</td>
            <td class="style1">
                <asp:TextBox ID="txt_validFrom" runat="server"></asp:TextBox>[<a href="#" onclick="clearField('txt_validFrom');return false;">Clear</a>]
            </td>
        </tr>
        <tr>
            <td>Valid Till</td>
            <td class="style1">
                <asp:TextBox ID="txt_validTo" runat="server"></asp:TextBox>[<a href="#" onclick="clearField('txt_validTo');return false;">Clear</a>]
            </td>
        </tr>

    </table>
    </div>
    <asp:Button ID="btn_update" runat="server" onclick="btn_update_Click" 
        Text="Update" />
    </form>
</body>
</html>
