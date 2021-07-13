'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'CommonMasterPageSales.master.vb
'──────────────────────────────────
'機能： マスターページ(iPod)
'補足： 
'作成： 2014/08/05 TMEJ 小澤 NextSTEPサービス 作業進捗管理に向けたシステム構想検討
'更新： 
'──────────────────────────────────
Imports Toyota.eCRB.SystemFrameworks.Web.Controls
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Globalization

Partial Class Master_CommonMasterPageSmall
    Inherits System.Web.UI.MasterPage

#Region "定数"

    ''' <summary>
    ''' マスターページ文言取得ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_MSTPG_DISPLAYID As String = "MASTERMAIN_SVR"

    ''' <summary>
    ''' マスターページ文言取得ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_MSTPG_FOOTERDISPLAYID As String = "MASTERFOOTER_SVR"

    ''' <summary>
    ''' クルクル対応の待ち時間設定値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_REFRESH_TIMER_TIME As String = "REFRESH_TIMER_TIME"

#End Region

#Region "Page_Load"
    ''' <summary>
    ''' マスターページのページロードを処理。
    ''' </summary>
    ''' <param name="sender">イベントの発生元。</param>
    ''' <param name="e">イベントに固有のデータ。</param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '初回読み込みチェック
        If Not Page.IsPostBack Then
            '初回読み込みの場合

            'マスターページの画面文言を設定します。
            Me._SetControlWord()

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

#End Region

#Region "_SetControlWord"
    ''' <summary>
    ''' 各コントロールへ文言を設定します。
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub _SetControlWord()
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'ログアウト
        CType(Me.FindControl("MstPG_Logout"), CustomHyperLink).Text = WebWordUtility.GetWord(C_MSTPG_DISPLAYID, 12)

        'タイムアウト時のメッセージ
        CType(FindControl("MstPG_RefreshTimerMessage1"), HiddenField).Value = WebWordUtility.GetWord(C_MSTPG_DISPLAYID, 21)

        ' メッセージ・設定値を取得
        Dim sysEnv As New SystemEnvSetting
        'リフレッシュタイマー
        CType(FindControl("MstPG_RefreshTimerTime"), HiddenField).Value = sysEnv.GetSystemEnvSetting(C_REFRESH_TIMER_TIME).PARAMVALUE
        sysEnv = Nothing

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
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
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'ログインユーザー情報取得
        Dim staff As StaffContext = StaffContext.Current()

        'ログイン状態を「4：ログアウト」に更新
        staff.UpdatePresence("4", "0")

        FormsAuthentication.SignOut()

        Response.Redirect(EnvironmentSetting.LoginUrl)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

#End Region

End Class
