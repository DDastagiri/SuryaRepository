'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3010201.ascx.vb
'─────────────────────────────────────
'機能： メインメニュー
'補足： 
'作成： 2011/12/01 TCS 寺本
'更新： 2014/02/26 TCS 河原
'─────────────────────────────────────

Imports Toyota.eCRB.Common.MainMenu.BizLogic
Imports System.Globalization

''' <summary>
''' メインメニューのマスターページです。
''' </summary>
''' <remarks></remarks>
Partial Class Pages_SC3010201
    Inherits System.Web.UI.MasterPage

    ''' <summary>
    ''' 画面ID
    ''' </summary>
    Protected Const AppId As String = "SC3010201"

    ''' <summary>
    ''' 初期状態の重要事項の表示件数
    ''' </summary>
    ''' 
    Private Const DEFAULT_MESSAGECOUNT As Integer = 2
    ''' <summary>
    ''' 初期状態のRSSの表示件数
    ''' </summary>
    Private Const DEFAULT_RSSCOUNT As Integer = 2


    ''' <summary>
    ''' ページロード時の処理です。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        If Not Me.IsPostBack Then
            SetWord()
        End If

    End Sub


    ''' <summary>
    ''' 事項事項リストのデータバインドイベント
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベント引数</param>
    ''' <remarks></remarks>
    Protected Sub MessageListView_DataBound(sender As Object, e As System.EventArgs) Handles MessageListView.DataBound

        If MessageListView.Items.Count <= DEFAULT_MESSAGECOUNT Then
            '２件以下の場合は、拡大ボタン非表示
            MessageBigSizeLink.Visible = False
        Else
            '２件以上は表示
            MessageBigSizeLink.Visible = True
        End If

        If Me.MessageListView.Controls(0).FindControl("messageNotFoundLiteral") IsNot Nothing Then
            CType(Me.MessageListView.Controls(0).FindControl("messageNotFoundLiteral"), Literal).Text _
                = WebWordUtility.GetWord(AppId, 8)
        End If


        'イメージ切替
        Dim literal As String = String.Empty
        Dim account As String = StaffContext.Current.Account
        For Each ListItem In MessageListView.Items
            If ListItem.FindControl("createAccountLiteral") IsNot Nothing Then
                'アカウント比較
                literal = CType(ListItem.FindControl("createAccountLiteral"), Literal).Text
                If literal.Equals(account) Then
                    CType(ListItem.FindControl("ImageForNotCreateAccount"), Image).Visible = False
                Else
                    CType(ListItem.FindControl("ImageForCreateAccount"), Image).Visible = False
                End If
            End If
        Next


    End Sub


    ''' <summary>
    ''' ＲＳＳリストのデータバインドイベント
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベント引数</param>
    ''' <remarks></remarks>
    Protected Sub RssListView_DataBound(sender As Object, e As System.EventArgs) Handles RssListView.DataBound

        If RssListView.Items.Count <= DEFAULT_RSSCOUNT Then
            '２件以下の場合は、拡大ボタン非表示
            RssBigSizeLink.Visible = False
        Else
            '２件以上は表示
            RssBigSizeLink.Visible = True
        End If

        If Me.RssListView.Controls(0).FindControl("rssNotFoundLiteral") IsNot Nothing Then
            CType(Me.RssListView.Controls(0).FindControl("rssNotFoundLiteral"), Literal).Text _
                = WebWordUtility.GetWord(AppId, 9)
        End If

    End Sub


    ''' <summary>
    ''' 文言の設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetWord()
        leftContentTitleLabel.Text = Server.HtmlEncode(WebWordUtility.GetWord(AppId, 3))
        newsTitleLabel.Text = Server.HtmlEncode(WebWordUtility.GetWord(AppId, 4))
        MessageNText1.Text = Server.HtmlEncode(WebWordUtility.GetWord(AppId, 5))
        MessageNText2.Text = Server.HtmlEncode(WebWordUtility.GetWord(AppId, 6))
        NewsNText1.Text = Server.HtmlEncode(WebWordUtility.GetWord(AppId, 5))
        NewsNText2.Text = Server.HtmlEncode(WebWordUtility.GetWord(AppId, 6))

        accountHiddenField.Value = StaffContext.Current.Account

    End Sub


    ''' <summary>
    ''' 重要事項更新ボタンクリック処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベント引数</param>
    ''' <remarks></remarks>
    Protected Sub MessageListViewUpdateButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles MessageListViewUpdateButton.Click
        '最新化
        MessageListViewUpdate()
    End Sub


    ''' <summary>
    ''' RSS更新ボタンクリック処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベント引数</param>
    ''' <remarks></remarks>
    Protected Sub RssListViewUpdateButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RssListViewUpdateButton.Click
        '再検索
        RssListView.DataBind()
        '別パネルの更新
        RssListViewUpPanel.Update()
        'クライアント側スクリプト設定
        JavaScriptUtility.RegisterStartupFunctionCallScript(Me.Page, "endRefreshRss", "endRefreshRss")
    End Sub


    ''' <summary>
    ''' 削除ボタンクリック処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベント引数</param>
    ''' <remarks></remarks>
    Protected Sub DeleteButtonHidden_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles DeleteButtonHidden.Click
        Dim messageNo As Long = Long.Parse(createAccountHiddenField.Value, CultureInfo.InvariantCulture)
        '削除処理
        Dim bizClass As New SC3010201BusinessLogic
        bizClass.DeleteMessageInfo(messageNo)
        '最新化
        MessageListViewUpdate()
        createAccountHiddenField.Value = ""
    End Sub


    ''' <summary>
    ''' 重要事項項目の最新化
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub MessageListViewUpdate()
        '再検索
        MessageListView.DataBind()
        '別パネルの更新
        MessageListViewUpPanel.Update()
        'クライアント側スクリプト設定
        JavaScriptUtility.RegisterStartupFunctionCallScript(Me.Page, "endRefreshMessage", "endRefreshMessage")
    End Sub


End Class

