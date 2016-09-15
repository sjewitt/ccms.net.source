/*
GLOBAL EDITOR JAVASCRIPTS:
*/

//do something with this re resizing if wondow.name=="popup"...
//alert(window.resizeTo(100,200));

//function popup(url,width,height){
function popup(url){
  alert("popping up");
  var width = 720;
  var height = 580;
  var win = window.open(url,'popup','width=' + width + ',height=' + height + ',toolbar=0,resizable=1,scrollbars=1');
  win.resizeTo(width,height);
  win.focus();
}

/*
ADMIN JAVASCRIPTS
*/
//USERS:
function validateFields(){
  var ok = true;
  var fields = ['login','password','fullname','email'];
  for(var a=0;a<fields.length;a++){
    if(document.user[fields[a]].value == ""){
      ok = false;
    }
  }
  return ok;  
}

function setPerms(){
  var chkbxArray = document.user.permission;
  var perms = 0;
  for(var a=0;a<chkbxArray.length;a++){
    if(chkbxArray[a].checked){
      perms = perms + parseInt(chkbxArray[a].value);
    }
  }
  document.user.permissions.value=perms;
}

function doUserAction(action){
  if(validateFields()){
    if(confirm("You are about to " + action + " this User. Proceed?")){
      document.user.submit();
    }
  }
  else{
    alert("All user property fields must be filled.");
  }
}

//CONTENT TYPES:
function deleteCT(){
  if(confirm("You are about to delete this Content Type. Proceed?")){
    document.deletect.submit();
  }
}
  
function doCTAction(action){
  if(document.contenttype.name.value.toString() != ""){
    if(confirm("You are about to " + action + " this Content Type. Proceed?")){
      document.contenttype.submit();
    }
  }
  else{
    alert("The 'name' field cannot be empty.");
  }
}

/*
FOR 'managepages':
*/
//Open the selected viewtree ref in the parent window: 
function reloadOpener(newloc){
  window.opener.location = newloc;
}

/*
define tab/panel div ID mapping.
This also defines other proprties such as 
iteation length to capture the URL tab index
parameter.
[note - functions with dynamic ASP replacements are inline]
*/

/*
General tab manipulation functions:
*/

var elemList = [
  {tab:'tab1',panel:'panel1'},
  {tab:'tab2',panel:'panel2'},
  {tab:'tab3',panel:'panel3'},
  {tab:'tab4',panel:'panel4'}
  /*{tab:'tab5',panel:'panel5'}*/
  ];

function showPanel(panelId){
  var panel;
  var tab;
  for(var a=0;a<elemList.length;a++){
    panel   = document.getElementById(elemList[a].panel);
    tab     = document.getElementById(elemList[a].tab);
    panel.style.display       = "none";
    tab.style.color           = "#eee";
    tab.style.backgroundColor = "#ccc";
    if(elemList[a].panel == panelId){
      panel.style.display = "block";
      tab.style.color = "purple";
      tab.style.backgroundColor = "#999";
    }
    var out = "";
    var count = 0;
    for(p in panel.style){
      out += (p+"="+panel.style[p]+"\t");
      if(count>=10){
        count=0;
        out+="\n";
      }
    }
  }
}

//tab 1:
function setAction(action){
  document.forms["vt"].action.value = action;
  //and submit it...
}
function checkVTForm(frm){
  alert(frm.action.value);
  return false;
}


//from manageviewtree - jump to appropriate page:
function doAction(nodeId){
   try{
    //alert(nodeId);
    //retrieve the action from the selected item. The node ID is passed in as an argument:
    var action = document.viewtreeactions.getaction.options[document.viewtreeactions.getaction.selectedIndex].value;
    var alertStr = "";
  
  
    switch(action){
  
      case "reorder":
        alertStr = "Re-order child pages of viewtree node " + nodeId;
        if(confirm(alertStr)){
          window.location = "/ccms_asp/editor/siblingorder.asp?nodeid=" + nodeId;  
        }
      break;
    
      case "layout":
        alertStr = "Manage layouts";
        if(confirm(alertStr)){
          window.location = "/ccms_asp/editor/assignlayouts.asp?nodeid=" + nodeId;
        }
      break;
    
      case "pageprops":
        alertStr = "Manage properties of the page that is referenced by this viewtree node";
        if(confirm(alertStr)){
          window.location = "/ccms_asp/editor/pageproperties.asp?nodeid=" + nodeId;  
        }
      break;
    
      //info only
      case "nodeprops":
        alertStr = "Properties of this viewtree node (" + nodeId + "):\n\tReferring page: \n\t";
        alert(alertStr);
      break;
    
      case "movenode":
        alertStr = "Move this node (" + nodeId + ") to a new viewtree location";
        if(confirm(alertStr)){
          window.location = "/ccms_asp/editor/movenode.asp?nodeid=" + nodeId;   
        }
      break;
    
      default:
        alertStr = "Please select an action to proceed.";
        alert(alertStr);
      break;
    }
  }
  catch(e){
    alert(e.message);
  }
}

function submitMoveNode(){
  try{
    if(confirm("Move selected viewtree item to this location?")){
      document.movenode.submit();
    }
  }  
  catch(e){
    alert(e.message);
  }
}

/*
submit template actions:
*/
function layoutAction(action)
{
  try
  {
    action = parseInt(action);
    var layout = document.layoutaction.layout[document.layoutaction.layout.selectedIndex].value;
    var msg = "";
    document.layoutaction.doaction.value=action;
    switch(action)
    {
      case 1:
        msg = "Edit source HTML (edit code in HTML form, update file)";
      break;
      case 2:
        msg = "Set visibility to editor (set new DB flag, modify code accordingly)";
      break;
      case 3:
        msg = "Delete this template (delete file)";
      break;
      case 4:
        msg = "Add a new template (create file from POSTed source)";
      break;
      default:
    }
    //alert(action + ", " + layout);
    if(confirm(msg))
    {
      document.layoutaction.submit();
      window.resizeTo(600,500);
    }
  }
  catch(e)
  {
    alert(e.message);
  }
}