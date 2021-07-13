<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ControlTest.aspx.vb" Inherits="ControlTest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no"/>
    <title>ControlTest</title>
    <%'jQuery (インテリセンスを有効にするため、直打ちで相対パスを記述) Page_Load時に各要求毎の相対パスに動的に書き換える %>
    <%'デザイナ上で下記コントロール値を変更すると、壊れてしまうので、変更時は、直接ソースエディタで変更して下さい。 %>
    <asp:Literal ID="jQueryScriptBlock" runat="server">
        <script type="text/javascript" src="Scripts/jquery-1.4.4.js"></script>
        <script type="text/javascript" src="Scripts/jquery-ui-1.8.16.custom.min.js"></script>
        <script type="text/javascript" src="Scripts/jquery.flickable.js"></script>
        <script type="text/javascript" src="Scripts/jquery.json-2.3.js"></script>
        <script type="text/javascript" src="Scripts/jquery.popover.js"></script>
        <script type="text/javascript" src="Scripts/jquery.PopOverForm.js"></script>
        <script type="text/javascript" src="Scripts/jquery.CustomGridView.js"></script>
    </asp:Literal>
    <link rel="Stylesheet" href="Styles/ControlStyle.css" />
    <link rel="Stylesheet" href="Styles/jquery.popover.css" />
</head>
<body>
    <form id="form1" runat="server" style="position:relative; padding:100px;">
    
        <asp:Button ID="buttonX" runat="server" Text="PopOverForm" />
        <icrop:PopOverForm ID="popOverForm1" runat="server" TriggerClientID="buttonX" HeaderTextWordNo="1" Width="300px" Height="300px">
        </icrop:PopOverForm>
        <script type="text/javascript">
            $(function () {
                $("#popOverForm1").PopOverForm({
                    renderEvent: function (pop, index, args, container) {
                        console.log('render:' + index);
                        pop.callbackServer({ message: 'Hello from client' }, function (result) {
                            var button = $("<div>Page" + index + "<button class='nextButton' type='button'>Next</button></div>");
                            button.children("button").click(function (e) {
                                if (index != 4) {
                                    pop.pushPage({});
                                } else {
                                    pop.closePopOver(192);
                                }
                            });
                            container.empty().append(button);
                        }, 2000);
                    },
                    closeEvent: function (pop, result) {
                        console.log('close:' + result);
                        return true;
                    }
                });
            });
        </script>
    </form>
</body>
</html>
