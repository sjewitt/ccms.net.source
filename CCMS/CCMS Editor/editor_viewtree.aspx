<%@ Page Language="C#" masterpagefile="~/editor.Master" AutoEventWireup="true" CodeBehind="editor_viewtree.aspx.cs" Inherits="ccms.editor_viewtree" %>

<asp:Content ID="header1" ContentPlaceHolderID="onloadJavascripts" runat="server">
<script language="javascript" type="text/javascript" src="Scripts/dom_managenav.aspx"></script>
</asp:Content>
<asp:Content ID="content1" ContentPlaceHolderID="EditActionPlaceholder" runat="server">
            
            <div id="tree">
            <%=this.editUtils.treeListHTML %>
            </div>

</asp:Content>
