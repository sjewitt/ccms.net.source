/// <reference path="jquery-1.10.2-vsdoc.js" />

$(function () {
    //get edit mask
    //console.log("generating login mask:");
    //detect query string:
    if (checkLoginQuerystringState() === "showLoginMask") {
        //build login DOM:
        var JQBuildTarget = $("#loginmask");
        JQBuildTarget.empty();

        var DOMBlurb = document.createElement("span");
        DOMBlurb.setAttribute("id", "loginDialogIntro");
        DOMBlurb.appendChild(document.createTextNode("Enter credentials:"))

        var DOMName = document.createElement("input");
        var DOMPwd = document.createElement("input");

        DOMName.setAttribute("type", "text");
        DOMName.setAttribute("id", "Username");
        DOMName.setAttribute("class", "login");

        DOMPwd.setAttribute("type", "password");
        DOMPwd.setAttribute("id", "Password");
        DOMPwd.setAttribute("class", "login");

        var btn = document.createElement("input");
        btn.setAttribute("type", "button");
        btn.setAttribute("id", "ccms_login_button");
        btn.setAttribute("value", "Login");
        $(btn).click(function () {
            /*
            TODO: rather than AJAX call, just submit as per LOGIN.ASPX and handle cookies like that 
            */
            //alert("clicked");

            //get data from fields:
            var data = new Object();
            data.Username = $("#Username").val();
            data.Password = $("#Password").val();

            $.post(window.location, data, function (data) { }, "application/json");
            //console.log(JSON.stringify(data));
            //            //do AJAX call here:
            //            var url = "/LoginService.svc/login";
            //            $.ajax({
            //                url: url,
            //                data: JSON.stringify(data),
            //                type: "POST",
            //                contentType: "application/json",
            //                success: function (data) {
            //                    alert(JSON.stringify(data));
            //                    if (data.Success) {
            //                        $("#loginDialogIntro").html("login success!. Load cookie...");
            //                    }
            //                    else {
            //                        $("#loginDialogIntro").html("login failed!. Please try again");
            //                    }
            //                    alert(" called OK");
            //                },
            //                error: function () {
            //                   
            //                    alert("call error");
            //                }

            //            });
        });

        var table = document.createElement("table");
        var row1 = document.createElement("tr");
        var row2 = document.createElement("tr");
        var row3 = document.createElement("tr");
        var cell1_1 = document.createElement("td");
        var cell1_2 = document.createElement("td");
        var cell2_1 = document.createElement("td");
        var cell2_2 = document.createElement("td");
        var cell3_1 = document.createElement("td");
        var cell3_2 = document.createElement("td");

        cell1_1.appendChild(document.createTextNode("User:"));
        cell2_1.appendChild(document.createTextNode("Password:"));

        cell1_2.appendChild(DOMName);
        cell2_2.appendChild(DOMPwd);

        cell3_2.appendChild(btn);

        row1.appendChild(cell1_1);
        row1.appendChild(cell1_2);

        row2.appendChild(cell2_1);
        row2.appendChild(cell2_2);

        row3.appendChild(cell3_1);
        row3.appendChild(cell3_2);

        table.appendChild(row1);
        table.appendChild(row2);
        table.appendChild(row3);

        JQBuildTarget[0].appendChild(DOMBlurb);
        JQBuildTarget[0].appendChild(table);

        $("#loginmask").dialog({
            title: "Please log in to CCMS:",
            modal: true
        });
    }

})

/*
*/
function checkLoginQuerystringState() {
    var loc = window.location;
    //console.log("location: "+loc)
    var _out = null;
    if (loc.toString().split("?").length === 2) {
        //console.log("Params: "+loc.toString().split("?"))
        //we have params:
        var params = loc.toString().split("?")[1].split("&");
        for (var a = 0; a < params.length; a++) {
            if (params[a].split("=")[0] === "loginmode" && params[a].split("=")[1] === "edit") {
                _out = "showLoginMask";
                //console.log("returning: " + _out);
            }
        }
    }
    return _out;  
}

/*
Return a DOM tree of the form. If isError, state login failed:
*/
function buildLoginForm(isError) {
    
}