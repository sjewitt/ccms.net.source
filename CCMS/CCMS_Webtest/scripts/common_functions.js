
//common functions

//pop up an editor window
function popup(url) {
    var width = 720;
    var height = 580;
    var win = window.open(url, 'popup', 'width=' + width + ',height=' + height + ',toolbar=0,resizable=1,scrollbars=1');
    win.resizeTo(width, height);
    win.focus();
}
