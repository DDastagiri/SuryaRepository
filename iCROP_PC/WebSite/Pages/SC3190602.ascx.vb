'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3190602.ascx.vb
'──────────────────────────────────
'機能： BO部品入力
'補足： 
'作成： 2014/08/28 TMEJ t.mizumoto
'──────────────────────────────────

Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.PartsManagement.BoMonitor.BizLogic
Imports Toyota.eCRB.PartsManagement.BoMonitor.BizLogic.SC3190602BusinessLogic
Imports Toyota.eCRB.PartsManagement.BoMonitor.DataAccess
Imports System.Globalization
Imports System.Web.Script.Serialization

''' <summary>
''' BO部品入力
''' </summary>
''' <remarks></remarks>
Partial Class Pages_SC3190602_Control
    Inherits System.Web.UI.UserControl

#Region "定数"

#Region "その他"

    ''' <summary>
    ''' 機能ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const AppId As String = "SC3190602"

#End Region

#Region "DB関連"

    ''' <summary>
    ''' 車両預かりフラグ:顧客
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VclPartakeFlgCust As String = "0"

    ''' <summary>
    ''' 車両預かりフラグ:販売店
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VclPartakeFlgDlr As String = "1"

#End Region

#Region "文言ID"

    ''' <summary>
    ''' 文言：タイトル
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdTitle As Integer = 1

    ''' <summary>
    ''' 文言：P/O No.
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdPoNum As Integer = 2

    ''' <summary>
    ''' 文言：R/O No.
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdRoNum As Integer = 3

    ''' <summary>
    ''' 文言：{0}.作業
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdJobName As Integer = 4

    ''' <summary>
    ''' 文言：-
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdDelete As Integer = 5

    ''' <summary>
    ''' 文言：部品名
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdPartsName As Integer = 6

    ''' <summary>
    ''' 文言：部品コード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdPartsCd As Integer = 7

    ''' <summary>
    ''' 文言：数量
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdPartsAmount As Integer = 8

    ''' <summary>
    ''' 文言：注文日
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdOrdDate As Integer = 9

    ''' <summary>
    ''' 文言：到着予定日
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdArrivalScheDate As Integer = 10

    ''' <summary>
    ''' 文言：チェック
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdCheck As Integer = 11

    ''' <summary>
    ''' 文言：+
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdAdd As Integer = 12

    ''' <summary>
    ''' 文言：車両預かりフラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdVclPartakeFlg As Integer = 13

    ''' <summary>
    ''' 文言：お客様約束日
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdCstAppointmentDate As Integer = 14

    ''' <summary>
    ''' 文言：登録
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdRegistration As Integer = 15

    ''' <summary>
    ''' 文言：車両預かりフラグ:DLR
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdVclPartakeFlgDlr As Integer = 16

    ''' <summary>
    ''' 文言：車両預かりフラグ:Cust
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdVclPartakeFlgCust As Integer = 17

    ''' <summary>
    ''' 文言：DBタイムアウトエラー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdDbTimeOut As Integer = 900

    ''' <summary>
    ''' 文言：登録時確認メッセージ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdRegisterConfirm As Integer = 901

    ''' <summary>
    ''' 文言：必須入力確認メッセージ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdCompulsoryInput As Integer = 902

#End Region

#Region "メッセージID"

    ''' <summary>
    ''' 正常
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MessageIdSuccess As Integer = 0

    ''' <summary>
    ''' DBタイムアウト
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MessageIdDbTimeOut As Integer = 900

#End Region

#End Region

#Region "イベント定義"

    ''' <summary>
    ''' フォームロード時のイベント
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Logger.Info("SC3190602_Page_Load_Start")

        ' PostBack時、初期表示処理は行わない。
        If Me.IsPostBack Then
            Logger.Info("SC3190602_Page_Load_End PostBack")
            Return
        End If

        ' 文言設定
        Me.SetWord()

        Logger.Info("SC3190602_Page_Load_End")

    End Sub

    ''' <summary>
    ''' スピンアイコン表示時の初期化処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub LoadSpinButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SC3190602_LoadSpinButton.Click

        Logger.Info("SC3190602_LoadSpinButton_Click_Start")

        Dim boId As Decimal = Decimal.Parse(Me.SC3190602_BoId.Value)

        ' 部品情報一覧取得
        Dim businessLogic As SC3190602BusinessLogic = New SC3190602BusinessLogic
        Dim boInfoDataSet As SC3190602BoInfoDataSet = businessLogic.GetBoInfo(boId)

        ' データバインド
        Me.JobRepeater.DataSource = boInfoDataSet
        Me.JobRepeater.DataBind()

        ' 表示加工処理
        ProcessingPartsInfoList(boInfoDataSet.JobInfo.Rows(0))

        ' 表示完了処理
        JavaScriptUtility.RegisterStartupFunctionCallScript(Me.Page, "initDisplaySC3190602", "startup")

        Logger.Info("SC3190602_LoadSpinButton_Click_End")
    End Sub

    ''' <summary>
    ''' 登録ボタン押下処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub RegisterButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SC3190602_RegisterButton.Click

        Logger.Info("SC3190602_RegisterButton_Start")

        ' 入力内容
        Dim serializer As New JavaScriptSerializer
        Dim boInfo As Dictionary(Of String, Object) = serializer.Deserialize(Of Dictionary(Of String, Object))(Me.SC3190602_Input.Value)

        Dim msgId = MessageIdSuccess
        Dim businessLogic As SC3190602BusinessLogic = New SC3190602BusinessLogic

        ' 入力チェック処理
        msgId = businessLogic.CheckInputValue(boInfo)
        If msgId <> MessageIdSuccess Then
            ' 入力チェックにてエラー発生時
            JavaScriptUtility.RegisterStartupFunctionCallScript( _
                Me.Page, "showMessageBoxSC3190602", "startup", WebWordUtility.GetWord(AppId, msgId))

            Logger.Info("SC3190602_RegisterButton_End InputValue FormatError MessageId[" & msgId & "]")
            Return
        End If

        ' DB登録処理
        'スタッフ情報を取得
        Logger.Info("SC3190602_RegisterButton_001 " & "Call_Start StaffContext.Current")
        Dim staffInfo As StaffContext = StaffContext.Current
        Logger.Info("SC3190602_RegisterButton_001 " & "Call_End   StaffContext.Current IsNull[" & IsNothing(staffInfo) & "]")

        ' 本日日付取得
        Logger.Info("SC3190602_RegisterButton_002 " & "Call_Start DateTimeFunc.Now PramValue[" & staffInfo.DlrCD & "]")
        Dim nowDate As Date = DateTimeFunc.Now(staffInfo.DlrCD)
        Logger.Info("SC3190602_RegisterButton_002 " & "Call_End   DateTimeFunc.Now RetValue[" & nowDate.ToString & "]")

        msgId = businessLogic.RegisterBoInfo(staffInfo.DlrCD, staffInfo.BrnCD, nowDate, staffInfo.Account, boInfo)

        ' 下記の２パターン以外はシステムエラー
        If msgId = MessageIdSuccess Then
            ' 正常終了時
            ' 登録完了処理
            JavaScriptUtility.RegisterStartupFunctionCallScript(Me.Page, "registerCompleteSC3190602", "startup")
        Else
            ' DBTimeOutエラー時
            JavaScriptUtility.RegisterStartupFunctionCallScript( _
                Me.Page, "showMessageBoxSC3190602", "startup", WebWordUtility.GetWord(AppId, msgId))
        End If

        Logger.Info("SC3190602_RegisterButton_End")

    End Sub

#End Region

#Region "非公開メソッド"

#Region "表示加工処理"

    ''' <summary>
    ''' 部品情報一覧(表示加工処理)
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ProcessingPartsInfoList(ByVal tagetDataRow As SC3190602BoInfoDataSet.JobInfoRow)

        ' B/O ID
        Me.SC3190602_BoId.Value = tagetDataRow.BO_ID

        ' P/O NO
        Me.SC3190602_PoNumHD.Value = Server.HtmlEncode(tagetDataRow.PO_NUM)

        ' R/O NO
        Me.SC3190602_RoNumHD.Value = Server.HtmlEncode(tagetDataRow.RO_NUM)

        ' 車両ステータス
        If String.Equals(VclPartakeFlgCust, tagetDataRow.VCL_PARTAKE_FLG) Then
            Me.SC3190602_VclPartakeFlg.SelectedIndex = 1
        ElseIf String.Equals(VclPartakeFlgDlr, tagetDataRow.VCL_PARTAKE_FLG) Then
            Me.SC3190602_VclPartakeFlg.SelectedIndex = 2
        Else
            Me.SC3190602_VclPartakeFlg.SelectedIndex = 0
        End If

        ' お客様約束日
        If Not String.IsNullOrEmpty(tagetDataRow.CST_APPOINTMENT_DATE) Then
            Me.SC3190602_CstAppDateHD.Value = tagetDataRow.CST_APPOINTMENT_DATE
        Else
            Me.SC3190602_CstAppDateHD.Value = String.Empty
        End If

        ' 作業分ループ
        For repJobListIndex = 0 To Me.JobRepeater.Items.Count - 1

            ' 作業名ラベル
            CType(Me.JobRepeater.Items(repJobListIndex).FindControl("SC3190602_JobNameWord"), Label).Text = _
                Server.HtmlEncode(String.Format(System.Globalization.CultureInfo.CurrentCulture _
                                                    , WebWordUtility.GetWord(AppId, WordIdJobName) _
                                                    , repJobListIndex + 1))
            ' 部品リストラベル
            CType(Me.JobRepeater.Items(repJobListIndex).FindControl("SC3190602_PartsNameWord"), Label).Text = _
                Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordIdPartsName))
            CType(Me.JobRepeater.Items(repJobListIndex).FindControl("SC3190602_PartsCdWord"), Label).Text = _
                Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordIdPartsCd))
            CType(Me.JobRepeater.Items(repJobListIndex).FindControl("SC3190602_PartsAmountWord"), Label).Text = _
                Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordIdPartsAmount))
            CType(Me.JobRepeater.Items(repJobListIndex).FindControl("SC3190602_OrdDateWord"), Label).Text = _
                Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordIdOrdDate))
            CType(Me.JobRepeater.Items(repJobListIndex).FindControl("SC3190602_ArrivalScheDateWord"), Label).Text = _
                Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordIdArrivalScheDate))
            CType(Me.JobRepeater.Items(repJobListIndex).FindControl("SC3190602_CheckWord"), Label).Text = _
                Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordIdCheck))
        Next

    End Sub

#End Region

#Region "文言の設定"

    ''' <summary>
    ''' 文言をセットする
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetWord()

        Logger.Info("SC3190602_SetWord_Start")

        Me.SC3190602_Title.Text = Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordIdTitle))
        Me.SC3190602_PoNumWord.Text = Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordIdPoNum))
        Me.SC3190602_RoNumWord.Text = Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordIdRoNum))
        Me.SC3190602_VclPartakeFlgWord.Text = Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordIdVclPartakeFlg))
        Me.SC3190602_VclPartakeFlg.Items(1).Text = WebWordUtility.GetWord(AppId, WordIdVclPartakeFlgCust)
        Me.SC3190602_VclPartakeFlg.Items(2).Text = WebWordUtility.GetWord(AppId, WordIdVclPartakeFlgDlr)
        Me.SC3190602_CstAppointmentDateWord.Text = Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordIdCstAppointmentDate))
        Me.SC3190602_Registration.Text = Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordIdRegistration))
        Me.SC3190602_RegisterWordHD.Value = Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordIdRegisterConfirm))
        Me.SC3190602_CompulsoryInputWordHD.Value = Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordIdCompulsoryInput))

        ' 隠しHTML
        Me.SC3190602_JobNameWordHD.Value = Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordIdJobName))
        Me.SC3190602_PartsNameWordHD.Text = Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordIdPartsName))
        Me.SC3190602_PartsCdWordHD.Text = Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordIdPartsCd))
        Me.SC3190602_PartsAmountWordHD.Text = Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordIdPartsAmount))
        Me.SC3190602_OrdDateWordHD.Text = Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordIdOrdDate))
        Me.SC3190602_ArrivalScheDateWordHD.Text = Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordIdArrivalScheDate))
        Me.SC3190602_CheckWordHD.Text = Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordIdCheck))

        Logger.Info("SC3190602_SetWord_End")
    End Sub

#End Region

#End Region

End Class
