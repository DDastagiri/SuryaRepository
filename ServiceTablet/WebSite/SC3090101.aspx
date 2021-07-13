<%@ Import Namespace="Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic" %>
<%@ Page Title="" Language="VB" MasterPageFile="~/Master/NoHeaderMasterPageSmall.Master" AutoEventWireup="false" CodeFile="SC3010101.aspx.vb" Inherits="Pages_SC3010101" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
  <link rel="Stylesheet" href="<%=WebResource.GetUrl("Styles/SC3090101/SC3090101.css")%>" type="text/css" media="screen,print" />
  <script type="text/javascript" src="<%=WebResource.GetUrl("Scripts/SC3010101/SC3010101.js")%>"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">
  <div id="baseBox">
    <asp:Image ID="img_back" runat="server" ImageUrl="~/Styles/Images/SC3090101/backimgSmall.png" />
    <div id="mainLogin">

      <!-- ここからコンテンツ -->
      <div id="contents">
        
        <!-- 入力 -->
        <asp:Panel ID="login" runat="server">
          <div id="loginId">
            <icrop:CustomTextBox ID="id" runat="server" PlaceHolderWordNo="1" type="text" onkeyup="checkInput();" MaxLength="26"/>
          </div>
          <div id="loginPw">
            <icrop:CustomTextBox ID="password" runat="server" PlaceHolderWordNo="2" type="password" onkeyup="checkInput();" MaxLength="10"/>
          </div>
        </asp:Panel>

        <!-- Loading -->
        <asp:Panel ID="loading" runat="server" CssClass="loading" style="display:none;">
          <table border="0" width="100%">
            <tr>
              <td align="center">
                <table border="0">
                <colgroup>
                  <col style="width:25px;" />
                  <col />
                </colgroup>
                <tbody>
                  <tr>
                    <td>
                      <div class="loadingVertical">
                        <div class="loadingIcn">
                          <img src="Styles/Images/SC3010101/animeicn.png" width="42" height="44" alt="" />
                        </div>
                      </div>
                    </td>
                    <td>
                      <div class="loadingVertical">
                        <div class="loadingChar">
                          <icrop:CustomLabel runat="server" id="lblLoginIn" TextWordNo="5" />
                        </div>
                      </div>
                    </td>
                  </tr>
                </tbody>
                </table>
              </td>
            </tr>
          </table>
        </asp:Panel>

        <!-- ボタン -->
        <div id="loginBtn" class="mousUp">
          <div class="loginChar">
            <icrop:CustomLabel runat="server" ID="lblLogin" TextWordNo="3" />
          </div>
        </div>
        <div id="loginDown" style="display:none;">         
          <div class="loginChar">
            <asp:Button runat="server" UseSubmitBehavior="true" CssClass="mousDown" ID="logOnBtn02" OnClientClick="login()"/>
          </div>
        </div>

      </div>
      <!-- ここかまでコンテンツ -->

      <div id="divValue">
        <asp:HiddenField runat="server" ID="hdnMac" />
        <asp:HiddenField runat="server" ID="hdnUploadFlg" />
      </div>

      <!-- 接続エラー -->
      <asp:Panel runat="server" ID="pnlError" Visible="false">
        <table width="100%">
          <tr>
            <td align="center">
              <table>
                <tr>
                  <td align="left" style="word-break:break-all;">
                    <icrop:CustomLabel runat="server" ID="clError"/>
                  </td>
                  <td align="left">
                    <div class="loginChar">
                      <asp:Button runat="server" ID="btnRefresh" CssClass="mousDown" />
                    </div>
                  </td>
                </tr>
              </table>
            </td>
          </tr>
        </table>
      </asp:Panel>
      
    </div>
  </div>
  
<asp:HiddenField ID="loginPG_RefreshTimerTime" runat="server" />
<asp:HiddenField ID="loginPG_RefreshTimerMessage1" runat="server" />

<asp:Button ID="refreshButton" runat="server" style="display:none" />
<asp:Button ID="OpenAMAuthButton" runat="server" UseSubmitBehavior="false" style="display:none" />
<asp:Button ID="autoSubmitButton" runat="server" UseSubmitBehavior="false" style="display:none" />
<asp:Button ID="errRedirectButton" runat="server" UseSubmitBehavior="false" style="display:none" />

</asp:Content>

