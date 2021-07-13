'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3010201.ascx.vb
'─────────────────────────────────────
'機能： メインメニュー
'補足： 
'作成： 2011/12/01 TCS 寺本
'更新： 2012/04/26 TCS 藤井 【SALES_2】HtmlEncode対応
'更新： 2013/12/19 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発
'更新： 2019/03/14 NSK 山田 18PRJ02750-00_(トライ店システム評価)サービス業務における次世代オペレーション実現の為の性能対策
'─────────────────────────────────────

Imports Toyota.eCRB.Common.MainMenu.BizLogic
Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks.Core
'2019/03/14 NSK 山田 18PRJ02750-00_(トライ店システム評価)サービス業務における次世代オペレーション実現の為の性能対策 START
Imports Toyota.eCRB.Common.MainMenu.DataAccess.SC3010201DataSet
'2019/03/14 NSK 山田 18PRJ02750-00_(トライ店システム評価)サービス業務における次世代オペレーション実現の為の性能対策 END

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
    Private Const DEFAULT_MESSAGECOUNT As Integer = 3
    ''' <summary>
    ''' 初期状態のRSSの表示件数
    ''' </summary>
    Private Const DEFAULT_RSSCOUNT As Integer = 3


    ''' <summary>
    ''' ページロード時の処理です。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        If Not Me.IsPostBack Then
            SetWord()
        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' 事項事項リストのデータバインドイベント
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベント引数</param>
    ''' <remarks></remarks>
    Protected Sub MessageListView_DataBound(sender As Object, e As System.EventArgs) Handles MessageListView.DataBound
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '重要事項の件数確認
        If MessageListView.Items.Count <= DEFAULT_MESSAGECOUNT Then
            '３件以下の場合は、拡大ボタン非表示
            MessageBigSizeLink.Visible = False

        Else
            '３件以上は表示
            MessageBigSizeLink.Visible = True

        End If

        If Me.MessageListView.Controls(0).FindControl("messageNotFoundLiteral") IsNot Nothing Then
            CType(Me.MessageListView.Controls(0).FindControl("messageNotFoundLiteral"), Literal).Text _
                = WebWordUtility.GetWord(AppId, 8)

        End If

        ' 2012/01/23 TCS 相田 【SALES_1B】 START
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
        ' 2012/01/23 TCS 相田 【SALES_1B】 END

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' ＲＳＳリストのデータバインドイベント
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベント引数</param>
    ''' <remarks></remarks>
    Protected Sub RssListView_DataBound(sender As Object, e As System.EventArgs) Handles RssListView.DataBound
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'RSSの件数チェック
        If RssListView.Items.Count <= DEFAULT_RSSCOUNT Then
            '３件以下の場合は、拡大ボタン非表示
            RssBigSizeLink.Visible = False

        Else
            '３件以上は表示
            RssBigSizeLink.Visible = True

        End If

        If Me.RssListView.Controls(0).FindControl("rssNotFoundLiteral") IsNot Nothing Then
            CType(Me.RssListView.Controls(0).FindControl("rssNotFoundLiteral"), Literal).Text _
                = WebWordUtility.GetWord(AppId, 9)

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' 文言の設定
    ''' </summary>
    ''' <remarks></remarks>
    ''' <History>
    ''' 2012/04/26 TCS 藤井 【SALES_2】HtmlEncode対応
    ''' </History>
    Private Sub SetWord()
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        ' 2012/04/26 TCS 藤井 【SALES_2】HtmlEncode対応 Modify Start
        leftContentTitleLabel.Text = Server.HtmlEncode(WebWordUtility.GetWord(AppId, 3))
        messageTitleLabel.Text = Server.HtmlEncode(WebWordUtility.GetWord(AppId, 2))
        newsTitleLabel.Text = Server.HtmlEncode(WebWordUtility.GetWord(AppId, 4))
        MessageNText1.Text = Server.HtmlEncode(WebWordUtility.GetWord(AppId, 5))
        MessageNText2.Text = Server.HtmlEncode(WebWordUtility.GetWord(AppId, 6))
        NewsNText1.Text = Server.HtmlEncode(WebWordUtility.GetWord(AppId, 5))
        NewsNText2.Text = Server.HtmlEncode(WebWordUtility.GetWord(AppId, 6))
        ' 2012/04/26 TCS 藤井 【SALES_2】HtmlEncode対応 Modify End

        ' 2012/01/23 TCS 相田 【SALES_1B】 START
        accountHiddenField.Value = StaffContext.Current.Account
        ' 2012/01/23 TCS 相田 【SALES_1B】 END

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' 重要事項更新ボタンクリック処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベント引数</param>
    ''' <remarks></remarks>
    Protected Sub MessageListViewUpdateButton_Click(sender As Object, e As System.EventArgs) Handles MessageListViewUpdateButton.Click
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '最新化
        MessageListViewUpdate()

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' RSS更新ボタンクリック処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベント引数</param>
    ''' <remarks></remarks>
    Protected Sub RssListViewUpdateButton_Click(sender As Object, e As System.EventArgs) Handles RssListViewUpdateButton.Click
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '再検索
        '2019/03/14 NSK 山田 18PRJ02750-00_(トライ店システム評価)サービス業務における次世代オペレーション実現の為の性能対策 START
        Dim bisLogic As New SC3010201BusinessLogic
        Dim rssData As SC3010201RssDataTable
        rssData = bisLogic.ReadRssInfo()
        RssListView.DataSource = rssData
        '2019/03/14 NSK 山田 18PRJ02750-00_(トライ店システム評価)サービス業務における次世代オペレーション実現の為の性能対策 END
        RssListView.DataBind()

        '別パネルの更新
        RssListViewUpPanel.Update()

        'クライアント側スクリプト設定
        JavaScriptUtility.RegisterStartupFunctionCallScript(Me.Page, "endRefreshRss", "endRefreshRss")

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ' 2012/01/23 TCS 相田 【SALES_1B】 START
    ''' <summary>
    ''' 削除ボタンクリック処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベント引数</param>
    ''' <remarks></remarks>
    Protected Sub DeleteButtonHidden_Click(sender As Object, e As System.EventArgs) Handles DeleteButtonHidden.Click
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim messageNo As Long = Long.Parse(createAccountHiddenField.Value, CultureInfo.InvariantCulture)

        '削除処理
        Dim bizClass As New SC3010201BusinessLogic
        bizClass.DeleteMessageInfo(messageNo)

        '最新化
        MessageListViewUpdate()
        createAccountHiddenField.Value = ""

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub
    ' 2012/01/23 TCS 相田 【SALES_1B】 END

    ''' <summary>
    ''' 重要事項項目の最新化
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub MessageListViewUpdate()
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '再検索
        '2019/03/14 NSK 山田 18PRJ02750-00_(トライ店システム評価)サービス業務における次世代オペレーション実現の為の性能対策 START
        Dim bisLogic As New SC3010201BusinessLogic
        Dim msgData As SC3010201MessageDataTable
        msgData = bisLogic.ReadMessageInfo()
        MessageListView.DataSource = msgData
        '2019/03/14 NSK 山田 18PRJ02750-00_(トライ店システム評価)サービス業務における次世代オペレーション実現の為の性能対策 END
        MessageListView.DataBind()

        '別パネルの更新
        MessageListViewUpPanel.Update()

        'クライアント側スクリプト設定
        JavaScriptUtility.RegisterStartupFunctionCallScript(Me.Page, "endRefreshMessage", "endRefreshMessage")

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    '2013/12/19 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
    ''' <summary>
    ''' 再描画ボタン処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ReloadButton_Click(sender As Object, e As System.EventArgs) Handles ReloadButton.Click

        '重要事項エリアの再描画
        '重要事項の再検索
        '2019/03/14 NSK 山田 18PRJ02750-00_(トライ店システム評価)サービス業務における次世代オペレーション実現の為の性能対策 START
        Dim bisLogic As New SC3010201BusinessLogic
        Dim msgData As SC3010201MessageDataTable
        msgData = bisLogic.ReadMessageInfo()
        MessageListView.DataSource = msgData
        '2019/03/14 NSK 山田 18PRJ02750-00_(トライ店システム評価)サービス業務における次世代オペレーション実現の為の性能対策 END
        MessageListView.DataBind()

        '重要事項エリアの更新
        MessageListViewUpPanel.Update()

        '重要事項エリアの拡大縮小ボタン設定
        MessageListButtonUpPanel.Update()

        '重要事項エリアのクライアント側スクリプト設定
        JavaScriptUtility.RegisterStartupFunctionCallScript(Me.Page, "endRefreshMessage", "endRefreshMessage")

        'RSSエリアの再描画
        'RSSの再検索
        '2019/03/14 NSK 山田 18PRJ02750-00_(トライ店システム評価)サービス業務における次世代オペレーション実現の為の性能対策 START
        Dim rssData As SC3010201RssDataTable
        rssData = bisLogic.ReadRssInfo()
        RssListView.DataSource = rssData
        '2019/03/14 NSK 山田 18PRJ02750-00_(トライ店システム評価)サービス業務における次世代オペレーション実現の為の性能対策 END
        RssListView.DataBind()

        'RSSエリアの更新
        RssListViewUpPanel.Update()

        'RSSエリアの拡大縮小ボタン設定
        RssListButtonUpPanel.Update()

        'RSSエリアのクライアント側スクリプト設定
        JavaScriptUtility.RegisterStartupFunctionCallScript(Me.Page, "endRefreshRss", "endRefreshRss")

        '2019/03/14 NSK 山田 18PRJ02750-00_(トライ店システム評価)サービス業務における次世代オペレーション実現の為の性能対策 START
        'クルクル非表示設定
        'JavaScriptUtility.RegisterStartupFunctionCallScript(Me.Page, "WhatsNewActiveOff", "WhatsNewActiveOff")

        'ダッシュボード表示設定
        JavaScriptUtility.RegisterStartupFunctionCallScript(Me.Page, "endReadData", "endReadData")
        '2019/03/14 NSK 山田 18PRJ02750-00_(トライ店システム評価)サービス業務における次世代オペレーション実現の為の性能対策 END

    End Sub
    '2013/12/19 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

End Class

