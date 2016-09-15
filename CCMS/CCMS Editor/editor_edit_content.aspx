<%@ Page 
    Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="editor_edit_content.aspx.cs" 
    Inherits="ccms.edit_content" 
    validateRequest="false"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Edit Content</title>

  <script type="text/javascript" src="tinymce4.0/tinymce.min.js"></script>
  <script type="text/javascript" src="Scripts/jquery-1.10.2.js"></script>
  <script type="text/javascript" src="Scripts/jquery.base64.js"></script>

  <script type="text/javascript">
  /// <reference path="jquery-1.10.2-vsdoc.js" />
  //handler for version options:
  function manageVersion(frm) {
      if (frm.state.value == "delete") {
          if (confirm("You are about to delete a version.\nAre you sure?")) {
              frm.submit();
          }
      }
      else {
          frm.submit();
      }
  }

  function editVersion(versionId) 
  {
      try {
      /** BUG HERE!!! **/
          if (confirm("do you want to discard current changes?")) {
            document.location = "editor_edit_content.aspx?contentid=<%=Request.QueryString["contentid"]%>&instanceid=" + versionId;
          }
      }
      catch (e) {
        alert(e);
      }
  }
  window.onload = function()
  {
    window.opener.location.reload();
  }
  
  
  </script>

</head>
<body>
    <form id="frm_editor" runat="server" name="frm_editor">
  
 <script type="text/javascript">
     tinymce.init(
    {
        selector: "textarea",
        plugins: "link",

        link_list: [
        <%=this.getLinkList()%>  
        ]
    }
  );
  </script>
  <div style="border:1px solid #bbb;">
  Select instance:
      <asp:DropDownList ID="sel_instance" runat="server" onchange="editVersion(this.value)"
          onselectedindexchanged="DropDownList1_SelectedIndexChanged">
      </asp:DropDownList>
&nbsp;&nbsp; Currently selected: 
  </div>
    <div style="border:none">
        <br />
        <asp:TextBox ID="txt_content" runat="server" Height="107px" Width="100%" 
            TextMode="MultiLine" Rows="30"></asp:TextBox>

        <br />
        <br />
        Save changes to current instance&nbsp;
        <asp:Button ID="btn_submitText" runat="server" Text="OK" onclick="btn_submitText_Click" />
        
        <br />
        Set current instance to
        <asp:DropDownList ID="sel_state" runat="server">
        </asp:DropDownList>
&nbsp;<asp:Button ID="btn_setState" runat="server" onclick="btn_setState_Click" 
            Text="OK" />
        <br />
        Create new instance based on current instance 
        <asp:Button ID="btn_newInstance" runat="server" onclick="btn_newInstance_Click" 
            Text="OK" />
        
        <br />
        <br />
        [<a href="#" onclick="window.close(); return false;">Close</a>]</div>
    
    </form>
</body>
</html>
