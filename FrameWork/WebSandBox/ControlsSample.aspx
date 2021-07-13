<%@ Page Title="" Language="VB" MasterPageFile="~/Master/NoHeaderMasterPage.Master" AutoEventWireup="false" CodeFile="ControlsSample.aspx.vb" Inherits="Pages_ControlsSample" %>

<asp:Content ID="Header1" ContentPlaceHolderID="head" runat="server">
<style type="text/css">
    table { border:1px solid black; }
    th, td { border:1px solid black; padding:5px; }
</style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="content" Runat="Server">
    カスタムコントロール　ギャラリー
    <table>
    <tbody>
        <tr>
            <th style="width:10%">CustomLabel</th>
            <td style="width:35%">
                <icrop:CustomLabel ID="CustomLabel1" runat="server" Width="100px" UseEllipsis="true">100pxを超えるデータは省略表示されます。</icrop:CustomLabel>
            </td>
            <th style="width:10%">CustomTextBox</th>              
            <td style="width:35%">
                <icrop:CustomTextBox ID="CustomTextBox1" runat="server" Width="200" PlaceHolderWordNo="2" />
            </td>
        </tr>
        <tr>
            <th>CustomButton</th>
            <td>
                 <icrop:CustomButton ID="CustomButton1" runat="server" TextWordNo="1" IconUrl="~/Styles/Images/Icon.png" BadgeCount="0" CausesPostBack="true" />
            </td>
            <th>CustomHyperLink</th>
            <td>
                <icrop:CustomHyperLink ID="CustomHyperLink1" runat="server" TextWordNo="1" IconUrl="~/Styles/Images/Icon.png" BadgeCount="0" CausesPostBack="true" />
            </td>
        </tr>
        <tr>
            <th>SegmentedButton</th>
            <td>
                <icrop:SegmentedButton ID="SegmentedButton1" runat="server" >
                    <asp:ListItem Text="Button1" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Button1" Value="2"></asp:ListItem>
                    <asp:ListItem Text="Button1" Value="3"></asp:ListItem>
                    <asp:ListItem Text="Button1" Value="4"></asp:ListItem>
                </icrop:SegmentedButton>
            </td>
            <th>&nbsp;</th>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <th>ItemSelector</th>
            <td>
                <icrop:ItemSelector ID="ItemSelector" runat="server" Width="100" Height="30">
                    <asp:ListItem Text="aaa" Value="1"></asp:ListItem>
                    <asp:ListItem Text="bbb" Value="2"></asp:ListItem>
                    <asp:ListItem Text="acccaa" Value="3" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="adddaa" Value="4"></asp:ListItem>
                    <asp:ListItem Text="aaeeea" Value="5"></asp:ListItem>
                </icrop:ItemSelector>
           </td>
            <th>MultiItemSelector</th>
            <td>
                <icrop:MultiItemSelector ID="MultiItemSelector1" runat="server" Width="200" Height="30" TextWordNo="1" IconUrl="">
                    <asp:ListItem Text="aaa" Value="1"></asp:ListItem>
                    <asp:ListItem Text="bbb" Value="2"></asp:ListItem>
                    <asp:ListItem Text="acccaa" Value="3" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="adddaa" Value="4"></asp:ListItem>
                    <asp:ListItem Text="aaeeea" Value="5"></asp:ListItem>
                </icrop:MultiItemSelector>
            </td>
        </tr>
        <tr>
            <th>SwitchButton</th>
            <td>
                <icrop:SwitchButton ID="switchButton" style="display:inline-block;" runat="server" OnTextWordNo="1" OffTextWordNo="2" Width="200" Height="50"　/>
            </td>
            <th></th>
            <td>
            </td>
        </tr>
        <tr>
            <th>CheckButton(push status)</th>
            <td>
                <icrop:CheckButton ID="CheckButton2" runat="server" Text="OK?" Width="200" Height="50" DisplayStyle="PushStatus" />
            </td>
            <th>DateTimeSelector (date)</th>
            <td>
                <icrop:DateTimeSelector ID="DateTimeSelector1" runat="server" PlaceHolderWordNo="99" Format="Date" />
            </td>
        </tr>
        <tr>
            <th>DateTimeSelector (time)</th>
            <td>
                <icrop:DateTimeSelector ID="DateTimeSelector2" runat="server" PlaceHolderWordNo="99" Format="Time" />
            </td>
            <th>DateTimeSelector (datetime)</th>
            <td>
                <icrop:DateTimeSelector ID="DateTimeSelector3" runat="server" PlaceHolderWordNo="99" Format="DateTime" />
            </td>
        </tr>
        <tr>
            <th>PopOver</th>
            <td>
                <asp:Button ID="popupTrigger" runat="server" Text="PopOver" />
                <icrop:PopOver ID="PopOver1" runat="server" TriggerClientID="popupTrigger" Width="200px" Height="200px">
                    <asp:TextBox ID="InnerText" runat="server" ></asp:TextBox>
                    <asp:Button ID="InnerButton" runat="server" Text="Click" />
                </icrop:PopOver>
            </td>
            <th>PopOverForm</th>
            <td>
                <asp:Button ID="buttonX" runat="server" Text="PopOverForm" />
                <icrop:PopOverForm ID="popOverForm1" runat="server" TriggerClientID="buttonX" HeaderTextWordNo="1" Width="300px" Height="300px">
                </icrop:PopOverForm>
                <script type="text/javascript">
                    $(function () {
                        $("#popOverForm1").PopOverForm({
                            render: function (pop, index, args, container) {
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
                            close: function (pop, result) {
                                console.log('close:' + result);
                                return true;
                            }
                        });
                    });
                </script>
            </td>
        </tr>
        </tbody>
    </table>

</asp:Content>
