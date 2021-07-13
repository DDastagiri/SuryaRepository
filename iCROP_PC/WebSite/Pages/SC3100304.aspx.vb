'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3100304.aspx.vb
'──────────────────────────────────
'機能： ウェルカムボード
'補足： 
'作成： 2013/3/14 TMEJ t.shimamura
'更新： 2013/04/16 TMEJ m.asano     ウェルカムボード仕様変更対応 $01
'更新： 2013/04/24 SKFC y.kushiro   Pushログインタイミング変更 
'─────────────────────────────────
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic

Partial Class Pages_SC3100304
    Inherits BasePage

#Region "定数"
    ''' <summary>
    ''' 文言ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ReceptionistId As String = "SC3100304"

    ''' <summary>
    ''' デバッグフラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DebugFlag As Boolean = False
#End Region

#Region "イベント処理"

#Region "ページロード"

    ''' <summary>
    ''' ページロード時の処理です。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Logger.Info("Page_Load_Start Param[" & sender.ToString & "," & e.ToString & "]")

        ' 店舗名称を取得
        Dim context As StaffContext = StaffContext.Current

        BranchName02.Text = Server.HtmlEncode(context.BrnName)

        Dim branchEnvSet As New BranchEnvSetting

        ' 文言取得
        WelcomeMessageFooter.Text = Server.HtmlEncode(WebWordUtility.GetWord(ReceptionistId, 1))
        branchEnvSet = Nothing

        If DebugFlag Then
            DebugArea.Visible = True
        End If

        Logger.Info("Page_Load_End")
    End Sub

#End Region

#End Region

End Class