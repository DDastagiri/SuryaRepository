'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'CommonMasterPageSales.master.vb
'──────────────────────────────────
'機能： マスターページ(iPod)
'補足： 
'作成： yyyy/MM/dd KN  x.xxxxxx
'更新： 2012/05/23 KN  浅野 クルクル対応 $01
'更新： 2012/06/04 KN  浅野 ログアウト時にステータスが更新されない $02
'──────────────────────────────────
Imports Toyota.eCRB.SystemFrameworks.Web.Controls
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic

Partial Class Master_CommonMasterPageSales
    Inherits System.Web.UI.MasterPage

#Region "定数"
    Private Const C_MSTPG_DISPLAYID As String = "MASTERPAGEMAIN"                ''マスターページ文言取得ID

    ' $01 クルクル対応 START
    Private Const C_REFRESH_TIMER_TIME As String = "REFRESH_TIMER_TIME"         ''クルクル対応の待ち時間設定値
    ' $01 クルクル対応 END

#End Region

#Region "Page_Load"
    ''' <summary>
    ''' マスターページのページロードを処理。
    ''' </summary>
    ''' <param name="sender">イベントの発生元。</param>
    ''' <param name="e">イベントに固有のデータ。</param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            ''マスターページの画面文言を設定します。
            Me._SetControlWord()

        End If

        'タイトルの設定
        _SetTitile()

    End Sub
#End Region

#Region "_SetTitile"
    ''' <summary>
    ''' タイトルの設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub _SetTitile()

        '画面タイトル
        CType(Me.FindControl("MstPG_TitleLabel"), Label).Text = WebWordUtility.GetTitle
        CType(Me.FindControl("MstPG_WindowTitle"), Literal).Text = WebWordUtility.GetTitle

    End Sub
#End Region

#Region "_SetControlWord"
    ''' <summary>
    ''' 各コントロールへ文言を設定します。
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub _SetControlWord()

        'ログアウト
        CType(Me.FindControl("MstPG_Logout"), CustomHyperLink).Text = WebWordUtility.GetWord(C_MSTPG_DISPLAYID, 12)

        ' $01 クルクル対応 START
        ' メッセージ・設定値を取得
        Dim sysEnv As New SystemEnvSetting
        CType(FindControl("MstPG_RefreshTimerTime"), HiddenField).Value = sysEnv.GetSystemEnvSetting(C_REFRESH_TIMER_TIME).PARAMVALUE
        CType(FindControl("MstPG_RefreshTimerMessage1"), HiddenField).Value = WebWordUtility.GetWord(C_MSTPG_DISPLAYID, 21)
        sysEnv = Nothing
        ' $01 クルクル対応 END
    End Sub
#End Region

#Region "LogoutButton_Click"
    ''' <summary>
    ''' ログアウト処理を行います。
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub LogoutButton_Click(ByVal sender As Object, ByVal e As EventArgs)

        '$02 START ログアウト時にステータスが更新されない
        Dim staff As StaffContext = StaffContext.Current()
        staff.UpdatePresence("4", "0")
        '$02 END   ログアウト時にステータスが更新されない

        FormsAuthentication.SignOut()

        Response.Redirect(EnvironmentSetting.LoginUrl)

    End Sub
#End Region

End Class
