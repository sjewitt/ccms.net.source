<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ORIGeditor_viewtree.aspx.cs" Inherits="ccms.editor_viewtree" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script language="javascript" type="text/javascript" src="Scripts/dom_managenav.aspx"></script>
    <title>Manage Viewtree</title>
</head>
<body>
    <div id="header">

    </div>
    <div id="main"><!-- the ASP.NET form and code --> 
        <form id="form1" runat="server">
        <div>
    
            <br />
            <asp:Panel ID="tree" runat="server">
            <%=this.editUtils.treeListHTML %>


            </asp:Panel>
        </div>
        </form>
    </div>
    <div id="footer">

    </div>
</body>
</html>
