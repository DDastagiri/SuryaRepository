'------------------------------------------------------------------------------
'SC3180205.aspx.vb
'------------------------------------------------------------------------------
'機能： 承認者選択
'補足： 
'作成： 2014/01/21 TMEJ小澤	初版作成
'更新： 
'------------------------------------------------------------------------------
Option Strict On
Option Explicit On

Imports Toyota.eCRB.SystemFrameworks.Web.Controls
Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Globalization
Imports Toyota.eCRB.AddRepair.AddRepairConfirm.BizLogic
Imports Toyota.eCRB.AddRepair.AddRepairConfirm.DataAccess.SC3180205DataSet

Public Class SC3180205
    Inherits BasePage

#Region "定数"

    ''' <summary>
    ''' 画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const APPLICATION_ID As String = "SC3180205"

    ''' <summary>
    ''' ログイン状態(1：ログイン)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PRESENCECATEGORY_LONIN As String = "1"
    ''' <summary>
    ''' ログイン状態(4：ログオフ)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PRESENCECATEGORY_LONOFF As String = "4"
    ''' <summary>
    ''' ログイン状態(3：待機中)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PRESENCECATEGORY_STANBY As String = "3"

    ''' <summary>
    ''' 文言ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Enum WordId
        ''' <summary>なし</summary>
        id000 = 0
        ''' <summary>データベースとの接続でタイムアウトが発生しました。再度処理を行ってください。</summary>
        id901 = 901
        ''' <summary>データベースエラーが発生しました。お手数ですがメインメニューへボタンをクリックしてください。</summary>
        id902 = 902
    End Enum

#End Region

#Region "初期処理"

    ''' <summary>
    ''' 初期処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim staffInfo As StaffContext = StaffContext.Current

        '初回読み込み時
        If Not IsPostBack Then
            '文言取得
            'Me.ApprovalListTitle.Text = WebWordUtility.GetWord(APPLICATION_ID, 1)
            'Me.CancelButton.Text = WebWordUtility.GetWord(APPLICATION_ID, 2)
            'Me.RegisterButton.Text = WebWordUtility.GetWord(APPLICATION_ID, 3)

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

#End Region

#Region "フッター制御"

    ''' <summary>
    ''' フッター制御
    ''' </summary>
    ''' <param name="commonMaster">マスターページ</param>
    ''' <param name="category">カテゴリ</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Public Overrides Function DeclareCommonMasterFooter(ByVal commonMaster As CommonMasterPage, _
                                                        ByRef category As FooterMenuCategory) As Integer()
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
        Return New Integer() {}

    End Function

#End Region

#Region "イベント"

    ''' <summary>
    ''' 初期表示用
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Protected Sub MainAreaReload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles MainAreaReload.Click
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'ログインユーザー情報取得
        Dim staffInfo As StaffContext = StaffContext.Current

        Using biz As New SC3180205BusinessLogic
            Try
                'ユーザー情報取得
                Dim dtUserInfo As SC3180205UserInfoDataTable = biz.GetUserInfo(staffInfo.DlrCD, _
                                                                               staffInfo.BrnCD)
                '取得情報チェック
                If dtUserInfo IsNot Nothing AndAlso 0 < dtUserInfo.Count Then
                    'データが取得できた場合

                    'アカウント情報をバインドする
                    Me.AccountAreaRepeater.DataSource = dtUserInfo
                    Me.AccountAreaRepeater.DataBind()

                    For i = 0 To Me.AccountAreaRepeater.Items.Count - 1
                        '画面定義取得
                        Dim accountArea As Control = Me.AccountAreaRepeater.Items(i)

                        'ROW取得
                        Dim drUserInfo As SC3180205UserInfoRow = _
                            CType(dtUserInfo.Rows(i), SC3180205UserInfoRow)

                        'アカウントとログイン状態を保持する
                        CType(accountArea.FindControl("AccountRecord"), HtmlControl).Attributes("name") = _
                            String.Concat(drUserInfo.ACCOUNT, ",", drUserInfo.PRESENCECATEGORY)

                        'ユーザー名
                        CType(accountArea.FindControl("AccountName"), CustomLabel).Text = drUserInfo.USERNAME

                        'ログイン状態の画像
                        Dim imageIcon As String = String.Empty
                        'ログイン状態チェック
                        If PRESENCECATEGORY_LONOFF.Equals(drUserInfo.PRESENCECATEGORY) Then
                            'ログインしていない場合
                            imageIcon = "nsc413OffIcn.png"

                        ElseIf PRESENCECATEGORY_STANBY.Equals(drUserInfo.PRESENCECATEGORY) Then
                            '待機中の場合
                            imageIcon = "nsc413CautionIcn.png"

                        ElseIf PRESENCECATEGORY_LONIN.Equals(drUserInfo.PRESENCECATEGORY) Then
                            'ログイン中の場合
                            imageIcon = "nsc413OnIcn.png"

                        End If

                        CType(accountArea.FindControl("PresenceImage"), HtmlControl).Attributes("src") = _
                            String.Concat("../Styles/Images/SC3180205/", imageIcon)

                    Next

                End If

                'エリア更新
                Me.MainAreaPanel.Update()

            Catch ex As OracleExceptionEx When ex.Number = 1013
                'ORACLEのタイムアウト処理
                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                                         , "{0}.{1} DB TIMEOUT:{2}" _
                                         , Me.GetType.ToString _
                                         , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                         , ex.Message))
                'DBタイムアウトのメッセージ表示
                Me.ShowMessageBox(WordId.id901)

            End Try

        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

#End Region

End Class
