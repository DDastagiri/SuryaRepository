''' <summary>
''' メインメニューのマスターページです。
''' </summary>
''' <remarks></remarks>
Partial Class Pages_SC3010201
    Inherits System.Web.UI.MasterPage

    ''' <summary>
    ''' 画面ID
    ''' </summary>
    Protected Const APPID As String = "SC3010201"

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
            '３件以下の場合は、拡大ボタン非表示
            MessageBigSizeLink.Visible = False
        Else
            '３件以上は表示
            MessageBigSizeLink.Visible = True
        End If

        If Me.MessageListView.Controls(0).FindControl("messageNotFoundLiteral") IsNot Nothing Then
            CType(Me.MessageListView.Controls(0).FindControl("messageNotFoundLiteral"), Literal).Text _
                = WebWordUtility.GetWord(APPID, 8)
        End If

    End Sub

    ''' <summary>
    ''' ＲＳＳリストのデータバインドイベント
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベント引数</param>
    ''' <remarks></remarks>
    Protected Sub RssListView_DataBound(sender As Object, e As System.EventArgs) Handles RssListView.DataBound

        If RssListView.Items.Count <= DEFAULT_RSSCOUNT Then
            '３件以下の場合は、拡大ボタン非表示
            RssBigSizeLink.Visible = False
        Else
            '３件以上は表示
            RssBigSizeLink.Visible = True
        End If

        If Me.RssListView.Controls(0).FindControl("rssNotFoundLiteral") IsNot Nothing Then
            CType(Me.RssListView.Controls(0).FindControl("rssNotFoundLiteral"), Literal).Text _
                = WebWordUtility.GetWord(APPID, 9)
        End If

    End Sub

    ''' <summary>
    ''' 文言の設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetWord()

        leftContentTitleLabel.Text = WebWordUtility.GetWord(APPID, 3)
        messageTitleLabel.Text = WebWordUtility.GetWord(APPID, 2)
        newsTitleLabel.Text = WebWordUtility.GetWord(APPID, 4)
        MessageNText1.Text = WebWordUtility.GetWord(APPID, 5)
        MessageNText2.Text = WebWordUtility.GetWord(APPID, 6)
        NewsNText1.Text = WebWordUtility.GetWord(APPID, 5)
        NewsNText2.Text = WebWordUtility.GetWord(APPID, 6)
    End Sub


End Class

