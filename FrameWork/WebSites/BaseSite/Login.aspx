<%@ Import Namespace="Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic" %>
<%@ Page Title="" Language="VB" MasterPageFile="~/Master/NoHeaderMasterPage.master" AutoEventWireup="false" CodeFile="Login.aspx.vb" Inherits="Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
  <link rel="Stylesheet" href="<%=WebResource.GetUrl("Styles/SC3010101/my.css") %>" type="text/css" media="screen,print" />
  <script type="text/javascript" src="<%=WebResource.GetUrl("Scripts/SC3010101/SC3010101.js?20120329124500")%>"></script>
  
  <script type="text/javascript">
  <!--
      /**
      * 認証後の処理.
      * 
      * @param {String} id ユーザアカウント
      * @return {-} -
      * 
      * @example 
      *  1.リロード
      *
      */
      function movePage(id) {
          //PushServer登録

          //来店実績_ログイン更新後ページ遷移
          reloadPage();
      }
      -->
  </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">
  <div id="baseBox">
    <div id="mainLogin">

      <!-- ここからコンテンツ -->
      <div id="contents">
        
        <!-- 入力 -->
        <div id="login">
          <div id="loginId">
            <icrop:CustomTextBox ID="id" runat="server" PlaceHolderWordNo="1" type="text" onkeyup="checkInput();" MaxLength="26"/>
          </div>
          <div id="loginPw">
            <icrop:CustomTextBox ID="password" runat="server" PlaceHolderWordNo="2" type="password" onkeyup="checkInput();" MaxLength="10"/>
          </div>
        </div>

        <!-- Loading -->
        <div id="Loading" class="loading" style="display:none;">
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
                          <img src="Styles/Images/SC3010101/animeicn.png" width="21" height="22" alt="" />
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
        </div>

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
        <asp:HiddenField runat="server" ID="hdnMac" Value="ST-UB-4L-OG-IN-ID"/>
        <asp:HiddenField runat="server" ID="hdnUploadFlg"/>
      </div>

      <!-- 接続エラー -->
      <asp:Panel runat="server" ID="pnlError" Visible="false" Width="100%">
        <table width="100%">
          <tr>
            <td align="center">
              <table>
                <tr>
                  <td align="left" style="word-break:break-all;">
                    <icrop:CustomLabel runat="server" ID="clError" Font-Size="Small"/>
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

</asp:Content>

