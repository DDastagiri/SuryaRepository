<%@ Page Title="" Language="VB" MasterPageFile="~/Master/NoHeaderMasterPage.Master" AutoEventWireup="false" CodeFile="ControlsSample.aspx.vb" Inherits="Pages_ControlsSample" %>

<asp:Content ID="Header1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
    table { border:1px solid black; }
    th, td { border:1px solid black; padding:5px; }
</style>
<script type="text/javascript">
    $(function () {
        $("body").unbind("touchmove.icropScript");
    });

    var callback = {
        doCallback: function (method, argument, callbackFunction) {
            this.method = method;
            this.argument = argument;
            this.packedArgument = method + "," + argument;
            this.endCallback = callbackFunction;
            this.beginCallback();
        }
    };
</script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="content" Runat="Server">
    <div style="position:relative; top:20px; left:20px;">
    <h2>カスタムコントロール　ギャラリー (サーバーサイドコントロール）</h2>
    <table>
    <tbody>
        <tr>
            <th style="width:10%">CustomLabel</th>
            <td style="width:35%">
                <div>
                    基本パターン：<icrop:CustomLabel ID="customLabel1" runat="server" TextWordNo="1"></icrop:CustomLabel>
                </div>
                <div>
                    省略表示させる：<icrop:CustomLabel ID="customLabel2" runat="server" TextWordNo="1" Width="100px" UseEllipsis="true"></icrop:CustomLabel>
                </div>
            </td>
            <th style="width:10%">CustomTextBox</th>              
            <td style="width:35%">
                <div>
                    基本パターン：<icrop:CustomTextBox ID="customTextBox1" runat="server" OnClientClear="customTextBox1_clear" Width="200" MaxLength="10" PlaceHolderWordNo="3" />
                    <script type="text/javascript">
                        function customTextBox1_clear() {
                            alert('cleared! (' + this + ')');
                        }
                    </script>
                 </div>
                <div>
                    非活性：<icrop:CustomTextBox ID="customTextBox2" runat="server" Width="200" MaxLength="10" PlaceHolderWordNo="3" Enabled="false" />
                </div>
            </td>
        </tr>
        <tr>
            <th>CustomButton</th>
            <td>
                <div>
                    アイコン＋テキスト：<icrop:CustomButton ID="customButton1" runat="server" TextWordNo="2" IconUrl="~/Styles/Images/button01.png" Width="80" Height="70" OnClientClick="return confirm('PostBackしますか?');"  />
                </div>
                <div>
                    押す度にバッジカウントが増える：<icrop:CustomButton ID="customButton2" runat="server" TextWordNo="2" IconUrl="~/Styles/Images/button01.png" Width="80" Height="70" />
                </div>
            </td>
            <th>CustomHyperLink</th>
            <td>
                <div>
                    アイコン＋テキスト：<icrop:CustomHyperLink ID="customHyperLink1" runat="server" TextWordNo="2" IconUrl="~/Styles/Images/button01.png" Width="80" Height="70" />
                </div>
                <div>
                    テキストのみ：<icrop:CustomHyperLink ID="customHyperLink2" runat="server" TextWordNo="2" Width="200" />
                </div>
            </td>
        </tr>
        <tr>
            <th>SegmentedButton</th>
            <td>
                <div>基本パターン：
                    <icrop:SegmentedButton ID="segmentedButton1" runat="server" ></icrop:SegmentedButton>
                </div>
                <div>AutoPostBack：
                    <icrop:SegmentedButton ID="segmentedButton2" runat="server" AutoPostBack="true" ></icrop:SegmentedButton>               
                </div>
            </td>
            <th>SwitchButton</th>
            <td>
                <div>基本パターン：
                    <icrop:SwitchButton ID="switchButton1" runat="server" OnTextWordNo="6" OffTextWordNo="7" Checked="true" />
                </div>
                <div>AutoPostBack：
                    <icrop:SwitchButton ID="switchButton2" runat="server" OnTextWordNo="6" OffTextWordNo="7" Checked="false" AutoPostBack="true"　/>
                </div>
            </td>
        </tr>
        <tr>
            <th>ItemSelector</th>
            <td>
                <icrop:ItemSelector ID="ItemSelector" runat="server" Width="100" Height="30" PlaceHolderTextWordNo="8" HeaderTextWordNo="10" >
                    <asp:ListItem Text="項目１" Value="1"></asp:ListItem>
                    <asp:ListItem Text="項目２" Value="2"></asp:ListItem>
                    <asp:ListItem Text="項目３" Value="3"></asp:ListItem>
                    <asp:ListItem Text="項目４" Value="4"></asp:ListItem>
                    <asp:ListItem Text="項目５" Value="5"></asp:ListItem>
                </icrop:ItemSelector>
           </td>
            <th>MultiItemSelector</th>
            <td>
                <icrop:MultiItemSelector ID="MultiItemSelector1" runat="server" Width="200" Height="30" PlaceHolderTextWordNo="9" HeaderTextWordNo="10">
                    <asp:ListItem Text="項目１" Value="1" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="項目２" Value="2" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="項目３" Value="3"></asp:ListItem>
                    <asp:ListItem Text="項目４" Value="4"></asp:ListItem>
                    <asp:ListItem Text="項目５" Value="5"></asp:ListItem>
               </icrop:MultiItemSelector>
            </td>
        </tr>
        <tr>
            <th>Check</th>
            <td>
                <div>
                    CheckButton：<icrop:CheckButton ID="checkButton1" runat="server" TextWordNo="2" OffIconUrl="~/Styles/Images/button01.png" OnIconUrl="~/Styles/Images/button01_on.png" Width="80" Height="70" />
                </div>
                <div>
                    CheckMark：<icrop:CheckMark ID="checkMark1" runat="server" TextWordNo="3" Position="Right" />
                </div>
            </td>
            <th>DateTimeSelector (date)</th>
            <td>
                <icrop:DateTimeSelector ID="DateTimeSelector1" runat="server" PlaceHolderWordNo="2" Format="Date" />
            </td>
        </tr>
        <tr>
            <th>DateTimeSelector (time)</th>
            <td>
                <icrop:DateTimeSelector ID="DateTimeSelector2" runat="server" CssClass="abcde" PlaceHolderWordNo="2" Format="Time" />
            </td>
            <th>DateTimeSelector (datetime)</th>
            <td>
                <icrop:DateTimeSelector ID="DateTimeSelector3" runat="server" PlaceHolderWordNo="2" Format="DateTime" />
            </td>
        </tr>
        <tr>
            <th>PopOver</th>
            <td>
                <button id="popupTrigger" type="button">PopOver</button>
                <icrop:PopOver ID="popOver1" runat="server" TriggerClientID="popupTrigger" HeaderTextWordNo="2" Width="200px" Height="200px" HeaderStyle="ClientId" HeaderClientId="popOver1Header">
                    <div>
                        <asp:TextBox ID="zipCode" ClientIDMode="Static" runat="server" ></asp:TextBox>
                        <button id="getAddressButton" type="button">住所検索</button>
                    </div>
                    <div>
                        <asp:TextBox ID="address" ClientIDMode="Static" runat="server" Rows="4" ></asp:TextBox>
                    </div>
                </icrop:PopOver>
                <div id="popOver1Header" style="display:none">
                    <table>
                        <tr>
                            <td><asp:Button ID="popOver1HeaderButton1" runat="server" Text="○○" /></td>
                            <td><asp:Button ID="popOver1HeaderButton2" runat="server" Text="△△" /></td>
                            <td><asp:Button ID="popOver1HeaderButton3" runat="server" Text="■■" /></td>
                            <td><asp:Button ID="popOver1HeaderButton4" runat="server" Text="××" /></td>
                        </tr>
                    </table>
                </div>
                 <script type="text/javascript">
                     $(function () {
                         $("#getAddressButton").click(function () {
                             callback.doCallback("GetAddress", $("#zipCode").val(), function (result, context) {
                                 $('#address').val(result);
                             });
                         });
                     });
                </script>
           </td>

            <th>PopOverForm</th>
            <td>
                <asp:Button ID="popOverButton1" runat="server" Text="PopOverForm" />
                <icrop:PopOverForm ID="popOverForm1" runat="server" TriggerClientID="popOverButton1" PageCapacity="4" HeaderTextWordNo="2" Width="300px" Height="300px" OnClientRender="popOverForm1_render" onClientClose="popOverForm1_close">
                </icrop:PopOverForm>
                <script type="text/javascript">
                    function popOverForm1_render(pop, index, args, container) {
                        container.empty().append("<span>Progress..</span>");
                        setTimeout(function () {
                            pop.callbackServer({ message: 'Hello from client' }, function (result) {
                                //サーバーからのレスポンスを受けて画面を生成
                                var button = $("<div>Page" + index + "<button class='nextButton' type='button'>" + result.response + "</button></div>");
                                button.children("button").click(function (e) {
                                    if (index != 4) {
                                        pop.pushPage({});
                                    } else {
                                        pop.closePopOver(192);
                                    }
                                });
                                container.empty().append(button);
                            });
                        }, 1000);
                    }
                    function popOverForm1_close(pop, result) {
                        //ポストバックさせたい場合のみ、trueを返す
                        return true;
                    }
                </script>
             </td>
        </tr>
        <tr>
            <th>OpenDialog</th>
            <td>
                <asp:Button ID="dialogButton" runat="server" Text="OpenDialog" />
            </td>

            <th>PopOverForm (No Callback)</th>
            <td>
                <asp:Button ID="popOverButton2" runat="server" Text="PopOverForm" />
                <icrop:PopOverForm ID="popOverForm2" runat="server" TriggerClientID="popOverButton2" PageCapacity="2" HeaderTextWordNo="2" Width="300px" Height="300px" OnClientRender="popOverForm2_render" onClientClose="popOverForm2_close"></icrop:PopOverForm>
                <asp:Panel ID="popOverForm2_1" runat="server" style="display:none">
                    <asp:Repeater ID="popOverForm2_1_repeater" runat="server" >
                    <HeaderTemplate><table></HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td><icrop:CustomButton ID="leftCustomButton" ClientIDMode="AutoID" CssClass="popOverForm2_1_buttons" runat="server" CausesPostBack="false" /></td>
                            <td><icrop:CustomButton ID="rightCustomButton" ClientIDMode="AutoID" CssClass="popOverForm2_1_buttons" runat="server" CausesPostBack="false" /></td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate></table></FooterTemplate>
                    </asp:Repeater>
                </asp:Panel>
                <asp:Panel ID="popOverForm2_2" runat="server" style="display:none">
                    <asp:TextBox ID="detailTextBox" runat="server" Rows="3" Text="abcde"></asp:TextBox>
                </asp:Panel>
               <script type="text/javascript">
                   function popOverForm2_render(pop, index, args, container) {                      
                        var page;
                        if (index == 0) {
                            page = container.children("#popOverForm2_1");
                            if (page.size() == 0) {
                                page = $("#popOverForm2_1").css("display", "block");
                                container.empty().append(page);
                                page.find(".popOverForm2_1_buttons").click(function (e) {
                                    if ($(this).attr("data-HasDetail") == "true") {
                                        pop.pushPage({ itemId: $(this).attr("data-ItemId") });
                                    } else {
                                        pop.closePopOver($(this).attr("data-ItemId"));
                                    }
                                });
                            }
                        } else if (index == 1) {
                            page = container.children("#popOverForm2_2");
                            if (page.size() == 0) {
                                page = $("#popOverForm2_2").css("display", "block");
                                container.empty().append(page);
                                $("#detailTextBox").click(function (e) {
                                    pop.closePopOver($(this).val());
                                });
                            }
                            page.find("#detailTextBox").val("ItemId:" + args.itemId);
                        }
                            
                    }

                    function popOverForm2_close(pop, result) {
                        //ポストバックさせたい場合のみ、trueを返す
                        alert(result);
                        return false;
                    }
                </script>
            </td>
        </tr>
        <tr>
            <th>CustomRepeater</th>
            <td>
                <div id="customRepeater1TotalCount" style="text-align:right;"></div>
                <icrop:CustomRepeater ID="customRepeater1" runat="server" OnClientRender="customRepeater1_Render" OnClientLoadCallbackResponse="customRepeater1_LoadCallbackResponse" Width="300" Height="200" RewindPagerLabelWordNo="11" ForwardPagerLabelWordNo="12" PageRows="50" CurrentPage="1" MaxCacheRows="100" />
                <div>Criteria:<input type="text" id="customRepeater1Criteria" /><button type="button" id="customRepeater1ReloadButton">Reload</button></div>
                <script type="text/javascript">
                    function customRepeater1_Render(row, view) {
                        view.append("<div> " + row.name + " </div>");
                    }
                    function customRepeater1_LoadCallbackResponse(result) {
                        $("#customRepeater1TotalCount").text(result.totalCount + "件");
                    }

                    $(function () {
                        $("#customRepeater1ReloadButton").click(function (e) {
                            $("#customRepeater1").CustomRepeater("reload", $("#customRepeater1Criteria").val());
                        });
                    });
                </script>                    
            </td>

               <th>PopOverForm (Customized Header Button)</th>
            <td>
                <asp:Button ID="popOverForm3Trigger" runat="server" Text="PopOverForm" />
                <icrop:PopOverForm ID="popOverForm3" runat="server" TriggerClientID="popOverForm3Trigger" PageCapacity="4" HeaderTextWordNo="2" Width="300px" Height="300px" OnClientRender="popOverForm3_render" OnClientClose="popOverForm3_close" OnClientOpen="popOverForm3_open">
                </icrop:PopOverForm>
                <script type="text/javascript">
                    var popOverForm1_largePopup = true;
                    var popOverForm1_hideCounter = 1;

                    function popOverForm3_open(pop) {
                        popOverForm1_hideCounter += 1;
                        if ((popOverForm1_hideCounter % 3) == 0) {
                            return false;
                        } else {
                            if (popOverForm1_largePopup) {
                                pop.resize(300, 300);
                                popOverForm1_largePopup = false;
                            } else {
                                pop.resize(200, 200);
                                popOverForm1_largePopup = true;
                            }
                            return true;
                        }
                        
                    }

                    function popOverForm3_render(pop, index, args, container, header) {
                        container.empty().append("<span>Progress..</span>");
                        setTimeout(function () {
                            pop.callbackServer({ message: 'Hello from client' }, function (result) {
                                //サーバーからのレスポンスを受けて画面を生成
                                var button = $("<div>Page" + index + "<button class='nextButton' type='button'>" + result.response + "</button></div>");
                                if (index != 3) {
                                    button.children("button").click(function (e) {
                                        pop.pushPage({});
                                    });
                                    if (index == 0) {
                                        var rightDiv = header.find(".icrop-PopOverForm-header-right"),
                                        commitButton = $("<button type='button'>決定1</button>");

                                        commitButton
                                        .click(function (e) {
                                            pop.closePopOver(191);
                                        })
                                        .appendTo(rightDiv);
                                    } else if (index == 2) {
                                        var rightDiv = header.find(".icrop-PopOverForm-header-right"),
                                        commitButton = $("<button type='button'>決定2</button>");

                                        commitButton
                                        .click(function (e) {
                                            pop.closePopOver(191);
                                        })
                                        .appendTo(rightDiv);
                                   }
                                } else {
                                    button.children("button").click(function (e) {
                                        pop.closePopOver(192);
                                    });
                                }
                                container.empty().append(button);
                            });
                        }, 1000);
                    }
                    function popOverForm3_close(pop, result) {
                        //ポストバックさせたい場合のみ、trueを返す
                        return true;
                    }
                </script>
             </td>
        </tr>

        <tr>
            <th>NumericBox</th>
            <td>
                <div style="color:Red;">
                    <icrop:NumericBox ID="numericBox1" runat="server" MaxDigits="4" Width="50px" Height="30px"
                     AutoPostBack="False" CssClass="style1" AcceptDecimalPoint="True" CompletionLabelWordNo="13" CancelLabelWordNo="14" ></icrop:NumericBox>台
                </div>
            </td>
            <th></th>
            <td></td>
        </tr>
 
        </tbody>
    </table>
</div>
</asp:Content>
