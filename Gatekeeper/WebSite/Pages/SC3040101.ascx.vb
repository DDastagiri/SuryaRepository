Imports Toyota.eCRB.Tool.Message.BizLogic
Imports Toyota.eCRB.Tool.Message.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Globalization

Partial Class Pages_SC3040101
    Inherits System.Web.UI.UserControl

#Region "定数"
    ''' <summary>
    ''' 機能ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_SYSTEM As String = "SC3040101"

    ''' <summary>
    ''' メッセージ入力エリアの制限文字数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_FIGURE_MESSAGE_TEXT As Integer = 128
#End Region

#Region "表示期限"
    '当日+7日
    Dim today As Date = Date.Today
    Dim targetdate As Date = today.AddDays(7)
#End Region

#Region "初期処理"

    ''' <summary>
    ''' (Pageイベント)Page_Load
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            '初期処理なし
        End If

        Me.TitleRequiredFieldValidator.ErrorMessage = WebWordUtility.GetWord("SC3040101", 901)
        Me.MessagesRequiredFieldValidator.ErrorMessage = WebWordUtility.GetWord("SC3040101", 902)

        titleCustomLabel.Text = WebWordUtility.GetWord("SC3040101", 1)
        displayPeriodCustomLabel.Text = WebWordUtility.GetWord("SC3040101", 4)
        postButton.Text = WebWordUtility.GetWord("SC3040101", 5)
        postCustomLabel.Text = WebWordUtility.GetWord("SC3040101", 5)
        cancelButton.Text = WebWordUtility.GetWord("SC3040101", 6)

        displayPeriodHidden.Value = String.Format(CultureInfo.CurrentCulture, "{0:yyyy-MM-dd}", targetdate)
        errMsg1Hidden.Value = WebWordUtility.GetWord("SC3040101", 901)
        errMsg2Hidden.Value = WebWordUtility.GetWord("SC3040101", 902)
        errMsg3Hidden.Value = WebWordUtility.GetWord("SC3040101", 903)
        serverProcessFlgHidden.Value = ""

        'placeholder
        messageCustomTitleTextBox.Attributes("placeholder") = WebWordUtility.GetWord("SC3040101", 2)
        messagesCustomTextBox.Attributes("placeholder") = WebWordUtility.GetWord("SC3040101", 3)

    End Sub

#End Region

#Region "投稿ボタン押下処理"

    ''' <summary>
    ''' 投稿ボタン押下処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks>本メソッドでは、以下のメソッドも利用しているため、例外情報に関しては、下記、メソッドを参照してください。</remarks>
    ''' <seealso>InsertPost</seealso>
    Protected Sub PostButton_Click(sender As Object, e As System.EventArgs) Handles postButton.Click

        '入力値検証
        If ValidateSC3040101() Then
            Using dt As New SC3040101DataSet.SC3040101MessageInfoDataTable
                Dim dr As SC3040101DataSet.SC3040101MessageInfoRow = CType(dt.NewRow(), SC3040101DataSet.SC3040101MessageInfoRow)

                dr.TITLE = messageCustomTitleTextBox.Text
                dr.MESSAGE = messagesCustomTextBox.Text
                dr.TIMELIMIT = displayPeriodCustomDateTimeSelector.Value

                dt.Rows.Add(dr)

                Dim bizLogic As New SC3040101BusinessLogic
                If bizLogic.InsertPost(dt) Then
                    messageCustomTitleTextBox.Text = ""
                    messagesCustomTextBox.Text = ""
                    displayPeriodCustomDateTimeSelector.Value = Nothing
                End If
            End Using
        Else
            postButton.Attributes("data-errorflg") = "yes"
            displayPerioderrHidden.Value = displayPeriodCustomDateTimeSelector.Value
        End If
    End Sub

#End Region

#Region "入力チェック"

    ''' <summary>
    ''' 入力値検証
    ''' </summary>
    ''' <returns>正常:True/異常:False</returns>
    ''' <remarks></remarks>
    Private Function ValidateSC3040101() As Boolean
        'タイトルの必須入力チェック
        If Not Me.TitleRequiredFieldValidator.IsValid Then
            ShowMessageBox2(901)
            Return False
        End If

        'メッセージの必須入力チェック
        If Not Me.MessagesRequiredFieldValidator.IsValid Then
            ShowMessageBox2(902)
            Return False
        End If

        '表示期限の必須入力チェック
        If IsNothing(displayPeriodCustomDateTimeSelector.Value) Then
            ShowMessageBox2(903)
            Return False
        End If

        'タイトルの禁則文字チェック
        If Not Validation.IsValidString(messageCustomTitleTextBox.Text) Then
            ShowMessageBox2(904)
            Return False
        End If

        'メッセージの禁則文字チェック
        If Not Validation.IsValidString(messagesCustomTextBox.Text) Then
            ShowMessageBox2(905)
            Return False
        End If

        'メッセージの入力文字数チェック
        If messagesCustomTextBox.Text.Length > C_FIGURE_MESSAGE_TEXT Then
            ShowMessageBox2(906)
            Return False
        End If
        Return True
    End Function

#End Region

#Region "ポップアップ処理"

    ''' <summary>
    ''' ユーザコントロール内でのメッセージ表示
    ''' </summary>
    ''' <param name="msg">表示用メッセージ</param>
    ''' <remarks></remarks>
    Private Sub ShowMessageBox2(ByVal msg As String)
        Dim sb As New StringBuilder
        sb.Append("<script>")
        sb.Append("  icropScript.ShowMessageBox(" & msg & ", """ & WebWordUtility.GetWord(C_SYSTEM, msg) & ""","""");")
        sb.Append("</script>")
        Dim cs As ClientScriptManager = Page.ClientScript
        cs.RegisterStartupScript(Me.GetType(), "icropScript.ShowMessageBox", sb.ToString())
    End Sub

#End Region

End Class
