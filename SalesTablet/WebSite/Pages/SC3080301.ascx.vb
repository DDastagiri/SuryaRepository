'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3080301.ascx.vb
'─────────────────────────────────────
'機能： 査定依頼
'補足： 
'作成： 2012/01/05 TCS 鈴木(恭)
'更新： 2012/03/09 TCS 鈴木(恭) 【SALES_2】コールバック時の文字列のエンコード処理追加
'更新： 2012/04/13 TCS 鈴木(恭) HTMLエンコード対応
'更新： 2013/01/24 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発
'更新： 2013/06/30 TCS 趙   【A STEP2】i-CROP新DB適応に向けた機能開発（既存流用）
'更新： 2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 
'─────────────────────────────────────

Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.CustomerInfo.AssesmentRequest.BizLogic
Imports Toyota.eCRB.CustomerInfo.AssesmentRequest.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Web.Controls
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.iCROP.BizLogic.Common
Imports Toyota.eCRB.CustomerInfo.AssesmentRequest.DataAccess.SC3080301TableAdapter
Imports System.Web.Services
Imports System.Globalization
Imports Toyota.eCRB.CustomerInfo.Details.BizLogic

'2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 START

''' <summary>
''' SC3080301(査定依頼)
''' Webページのプレゼンテーション層
''' </summary>
''' <remarks>査定依頼</remarks>
Partial Class Pages_SC3080301
    Inherits System.Web.UI.UserControl
    Implements ICallbackEventHandler

#Region "定数"
    ''' <summary>
    ''' 査定依頼画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const Str_Dispid_Assessment As String = "SC3080301"

    ''' <summary>
    ''' カーチェックシート画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const Str_Dispid_Carchecksheet As String = "SC3060101"

    ''' <summary>
    ''' 在席状態用分類コード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const Presence_Category_Standby As String = "1"
    Private Const Presence_Category_On As String = "2"
    Private Const Presence_Detail_Zero As String = "0"
    Private Const Presence_Detail_One As String = "1"

    ''' <summary>
    ''' スタッフの在席状態(0:査定依頼不可、1:在席中で査定依頼可能)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const Account_Status_Off As String = "0"
    Private Const Account_Status_On As String = "1"

    ''' <summary>
    ''' 画面表示状態(1:査定済みの画面、2:査定依頼中の画面)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const Display_Flg_Assessment As String = "1"
    Private Const Display_Flg_Cancel As String = "2"

    ''' <summary>
    ''' I/F結果ID DBタイムアウト
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IfDbTimeOut As String = "006000"

    ''' <summary>
    ''' メッセージID　通知IFエラー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ErrMessageIdNoticeIf As Integer = 9001

    ''' <summary>
    ''' 査定金額用フォーマット
    ''' </summary>
    ''' <remarks></remarks>
    Private Const AssessmentPriceFormat As String = "#,##0"

    ''' <summary>
    ''' セッションキー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_FLLSTRCD As String = "SearchKey.FLLWUPBOX_STRCD"              '店舗コード(Follow-up活動のキー)
    Private Const SESSION_KEY_FLLSEQNO As String = "SearchKey.FOLLOW_UP_BOX"                'Follow-up 活動連番(Follow-up活動のキー)
    Private Const SESSION_KEY_ORIGINALID As String = "SearchKey.CRCUSTID"                   '顧客コード (自社客=自社客連番 / 未取引客=未取引客ユーザーID)
    Private Const SESSION_KEY_CUSTFLG As String = "SearchKey.CSTKIND"                       '顧客種別 (1：自社客 2：未取引客)
    Private Const SESSION_KEY_CUSTCLASS As String = "SearchKey.CUSTOMERCLASS"               '顧客分類 (1：所有者、2：使用者、3：その他)
    Private Const SESSION_KEY_SALESSTAFFCD As String = "SearchKey.SALESSTAFFCD"             '担当セールススタッフコード
    Private Const SESSION_KEY_VCLID As String = "SearchKey.VCLID"                           '車両ID
    Private Const SESSION_KEY_CUSTNAME As String = "SearchKey.NAME"                         '顧客名称 (顧客名＋敬称)
    Private Const SESSION_KEY_ASSESSMENTNO As String = "SearchKey.ASSESSMENTNO"             '査定No
    Private Const SESSION_KEY_NOTICEREQID As String = "SearchKey.REQUESTID"                 '査定依頼ID
    '2013/01/24 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START
    Private Const SESSION_KEY_SALESBKGNO As String = "SearchKey.ORDER_NO"
    '2013/01/24 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END

#End Region

#Region "コールバック"

    Private callbackResultSC3080301 As String

    ''' <summary>
    ''' コールバック用文字列を返す
    ''' </summary>
    ''' <remarks></remarks>
    Public Function GetCallbackResult() As String Implements System.Web.UI.ICallbackEventHandler.GetCallbackResult

        Logger.Info(String.Format(CultureInfo.InvariantCulture, Str_Dispid_Assessment & System.Reflection.MethodBase.GetCurrentMethod.Name & "_Start"))

        Logger.Info(String.Format(CultureInfo.InvariantCulture, Str_Dispid_Assessment & System.Reflection.MethodBase.GetCurrentMethod.Name & "_End[callbackResultSC3080301:{0}]", Me.callbackResultSC3080301))

        Return Me.callbackResultSC3080301

    End Function

    Private Const OKText As String = "1"

    Private Const ErrorText As String = "999"

    '査定画面の初期表示
    Private Const MethodAssessmentLoad As String = "AssessmentLoad"

    '依頼ボタン押下
    Private Const MethodRegisterButton As String = "AssessmentRegister"

    'キャンセルボタン押下
    Private Const MethodCancelButton As String = "AssessmentCancel"

    ''' <summary>
    ''' コールバックイベントハンドリング
    ''' </summary>
    ''' <remarks></remarks>
    ''' <History>
    '''  2012/04/13 TCS 鈴木(恭) HTMLエンコード対応
    ''' </History>
    Public Sub RaiseCallbackEvent(eventArgument As String) Implements System.Web.UI.ICallbackEventHandler.RaiseCallbackEvent

        Logger.Info(String.Format(CultureInfo.InvariantCulture, Str_Dispid_Assessment & System.Reflection.MethodBase.GetCurrentMethod.Name & "_Start[eventArgument:{0}]", eventArgument))

        Try

            Dim tokens As String() = eventArgument.Split(New Char() {","c})
            Dim method As String = tokens(0)
            Dim registAssessmentNo As Long
            Dim registNoticeReqId As Long
            Dim registRetention As String
            Dim registVin As String
            Dim registSeqno As Long
            Dim resultString As String = String.Empty

            Select Case method
                Case MethodAssessmentLoad
                    '査定画面を作成する
                    resultString = AssessmentStartPopUpWindow()

                    Me.callbackResultSC3080301 = HttpUtility.HtmlEncode(resultString)

                    resultString = SC3080301BusinessLogic.SuccessIfZero
                Case MethodRegisterButton
                    '依頼ボタン押下処理
                    '2012/04/13 TCS 鈴木(恭) HTMLエンコード対応 START
                    '2012/03/09 TCS 鈴木(恭) 【SALES_2】コールバック時の文字列のエンコード処理追加 START
                    If Not String.IsNullOrEmpty(Trim(HttpUtility.UrlDecode(tokens(1)))) Then
                        registAssessmentNo = CLng(HttpUtility.UrlDecode(tokens(1)))
                    End If
                    If Not String.IsNullOrEmpty(Trim(HttpUtility.UrlDecode(tokens(2)))) Then
                        registNoticeReqId = CLng(HttpUtility.UrlDecode(tokens(2)))
                    End If
                    registRetention = HttpUtility.UrlDecode(tokens(3))
                    registVin = HttpUtility.UrlDecode(tokens(4))
                    If Not String.IsNullOrEmpty(Trim(HttpUtility.UrlDecode(tokens(5)))) Then
                        registSeqno = CLng(HttpUtility.UrlDecode(tokens(5)))
                    End If
                    '2012/03/09 TCS 鈴木(恭) 【SALES_2】コールバック時の文字列のエンコード処理追加 END
                    '2012/04/13 TCS 鈴木(恭) HTMLエンコード対応 END

                    resultString = AssessmentRegisterInfo(registAssessmentNo, registNoticeReqId, registRetention, registVin, registSeqno)

                Case MethodCancelButton
                    'キャンセルボタン押下処理
                    '2012/04/13 TCS 鈴木(恭) HTMLエンコード対応 START
                    '2012/03/09 TCS 鈴木(恭) 【SALES_2】コールバック時の文字列のエンコード処理追加 START
                    registAssessmentNo = CLng(HttpUtility.UrlDecode(tokens(1)))
                    registNoticeReqId = CLng(HttpUtility.UrlDecode(tokens(2)))
                    registRetention = HttpUtility.UrlDecode(tokens(3))
                    registVin = HttpUtility.UrlDecode(tokens(4))
                    If Not String.IsNullOrEmpty(Trim(HttpUtility.UrlDecode(tokens(5)))) Then
                        registSeqno = CLng(HttpUtility.UrlDecode(tokens(5)))
                    End If
                    '2012/03/09 TCS 鈴木(恭) 【SALES_2】コールバック時の文字列のエンコード処理追加 END
                    '2012/04/13 TCS 鈴木(恭) HTMLエンコード対応 END

                    resultString = AssessmentCancelInfo(registAssessmentNo, registNoticeReqId, registRetention, registVin, registSeqno)

            End Select

            If resultString = IfDbTimeOut Then
                Logger.Info(String.Format(CultureInfo.InvariantCulture, Str_Dispid_Assessment & System.Reflection.MethodBase.GetCurrentMethod.Name & resultString))
                Me.callbackResultSC3080301 = ErrorText + "," + WebWordUtility.GetWord(Str_Dispid_Assessment, 902)
            ElseIf resultString <> SC3080301BusinessLogic.SuccessIfZero Then
                Logger.Info(String.Format(CultureInfo.InvariantCulture, Str_Dispid_Assessment & System.Reflection.MethodBase.GetCurrentMethod.Name & resultString))
                Me.callbackResultSC3080301 = ErrorText + "," + resultString
            End If
        Catch ex As Exception
            '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 START
            Logger.Error(String.Format(CultureInfo.InvariantCulture, Str_Dispid_Assessment & System.Reflection.MethodBase.GetCurrentMethod.Name & ex.Message))
            '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 END
            Me.callbackResultSC3080301 = ErrorText + "," + ex.Message
        End Try

        Logger.Info(String.Format(CultureInfo.InvariantCulture, Str_Dispid_Assessment & System.Reflection.MethodBase.GetCurrentMethod.Name & "_End"))

    End Sub
#End Region

#Region "イベント"

    ''' <summary>
    ''' ロード時の処理を実施します。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Logger.Info(String.Format(CultureInfo.InvariantCulture, Str_Dispid_Assessment & System.Reflection.MethodBase.GetCurrentMethod.Name & "_Start[sender:{0}][e:{1}]", sender.ToString, e.ToString))

        If Not Me.IsPostBack Then

            '文言情報を取得
            SetWordValue()

        End If

        'コールバック作成
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), _
        "Callback", _
        String.Format(CultureInfo.InvariantCulture, _
          "callbackSC3080301.beginCallback = function () {{ {0}; }};", _
          Page.ClientScript.GetCallbackEventReference(Me, _
         "callbackSC3080301.packedArgument", _
         "callbackSC3080301.endCallback", _
         "", _
         False)), _
          True)

        Logger.Info(String.Format(CultureInfo.InvariantCulture, Str_Dispid_Assessment & System.Reflection.MethodBase.GetCurrentMethod.Name & "_End[sender:{0}][e:{1}]", sender.ToString, e.ToString))

    End Sub
#End Region

#Region "メソッド"

    ''' <summary>
    ''' 画面初期表示イベント
    ''' </summary>
    ''' <remarks></remarks>
    ''' <History>
    '''  2012/04/13 TCS 鈴木(恭) HTMLエンコード対応
    ''' </History>
    Private Function AssessmentStartPopUpWindow() As String

        Logger.Info(String.Format(CultureInfo.InvariantCulture, Str_Dispid_Assessment & System.Reflection.MethodBase.GetCurrentMethod.Name & "_Start"))

        '前画面からセッション情報を取得
        Dim bizLogicSC3080301 As SC3080301BusinessLogic
        bizLogicSC3080301 = New SC3080301BusinessLogic

        Using getSessionDataTable = New SC3080301DataSet.SC3080301SessionDataTable
            Dim sessionDataRow As SC3080301DataSet.SC3080301SessionRow

            sessionDataRow = getSessionDataTable.NewSC3080301SessionRow
            SetSessionValue(sessionDataRow)                              'セッション値の取得
            getSessionDataTable.Rows.Add(sessionDataRow)                 '追加する

            '端末ID取得
            Using getTerminalDataTable = New SC3080301DataSet.SC3080301UcarTerminalDataTable
                Dim terminalList As SC3080301DataSet.SC3080301UcarTerminalDataTable = _
                    bizLogicSC3080301.GetTerminalList(sessionDataRow.DLRCD, sessionDataRow.STRCD)
                If terminalList IsNot Nothing AndAlso Not terminalList.Rows.Count = 0 Then
                    '保有車両リスト取得
                    Using getVehicleDataTable = New SC3080301DataSet.SC3080301VehicleDataTable
                        Dim vehicleList As SC3080301DataSet.SC3080301VehicleDataTable = _
                            bizLogicSC3080301.GetVehicleList(getSessionDataTable)
                        Using getOtherDataTable = New SC3080301DataSet.SC3080301OtherVehicleDataTable
                            Dim otherDataRow As SC3080301DataSet.SC3080301OtherVehicleRow

                            otherDataRow = getOtherDataTable.NewSC3080301OtherVehicleRow
                            SetOtherValue(getSessionDataTable, otherDataRow)            'セッション値の取得
                            getOtherDataTable.Rows.Add(otherDataRow)                    '追加する
                            '査定情報取得
                            Using getAssessmentDataTable = New SC3080301DataSet.SC3080301UcarAssessmentDataTable
                                Dim assessmentList As SC3080301DataSet.SC3080301UcarAssessmentDataTable = _
                                    bizLogicSC3080301.GetAssessmentList(getSessionDataTable)

                                If assessmentList IsNot Nothing AndAlso Not assessmentList.Rows.Count = 0 Then
                                    '車両の査定情報を反映
                                    SetVehicleAssessmentValues(vehicleList, assessmentList, sessionDataRow.DLRCD)
                                    'その他車両の査定情報を反映
                                    SetOtherVehicleAsssessmentValues(getOtherDataTable, assessmentList, sessionDataRow.DLRCD)
                                End If

                                '初期画面に反映
                                Me.GetDisplayValues(getSessionDataTable, vehicleList, getOtherDataTable)
                            End Using
                        End Using
                        '保有車両一覧に反映
                        Repeater1.DataSource = vehicleList
                        Repeater1.DataBind()
                    End Using
                Else
                    Me.AssessmentErrorPanel.Style.Item("display") = "block"
                    Me.RequestStatusPanel.Style.Item("display") = "none"
                    Me.EndStatusPanel.Style.Item("display") = "none"
                End If
                '2012/04/13 TCS 鈴木(恭) HTMLエンコード対応 START
                Me.SelectAccountStatusHidden.Value = HttpUtility.HtmlEncode(sessionDataRow.ACCOUNTSTATUS)
                '2012/04/13 TCS 鈴木(恭) HTMLエンコード対応 END
            End Using

            '上記で作成した画面のHTMLを返す		
            Using sw As New System.IO.StringWriter(CultureInfo.CurrentCulture)
                Dim writer As HtmlTextWriter = New HtmlTextWriter(sw)
                Me.RenderControl(writer)
                Return sw.GetStringBuilder().ToString
            End Using

        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, Str_Dispid_Assessment & System.Reflection.MethodBase.GetCurrentMethod.Name & "_End"))

    End Function

    ''' <summary>
    ''' 画面文言設定イベント
    ''' </summary>
    ''' <remarks></remarks>
    ''' <History>
    '''  2012/04/13 TCS 鈴木(恭) HTMLエンコード対応
    ''' </History>
    Private Sub SetWordValue()

        Logger.Info(String.Format(CultureInfo.InvariantCulture, Str_Dispid_Assessment & System.Reflection.MethodBase.GetCurrentMethod.Name & "_Start"))

        '2012/04/13 TCS 鈴木(恭) HTMLエンコード対応 START
        Me.AssessmentCarPopupMakerTitle.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(Str_Dispid_Assessment, 1))
        Me.AssessmentCarPopupCancelLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(Str_Dispid_Assessment, 2))
        Me.AssessmentCancleLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(Str_Dispid_Assessment, 2))
        Me.AssessmentCancleDummyLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(Str_Dispid_Assessment, 2))
        Me.AssessmentRequestLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(Str_Dispid_Assessment, 3))
        Me.AssessmentRequestDummyLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(Str_Dispid_Assessment, 3))
        Me.AssessmentCarPopupMakerBkLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(Str_Dispid_Assessment, 4))
        Me.AssessmentCarPopupModelTitle.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(Str_Dispid_Assessment, 5))
        Me.OtherRequestLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(Str_Dispid_Assessment, 6))
        Me.OtherAssessmentLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(Str_Dispid_Assessment, 6))
        Me.RequestIraiLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(Str_Dispid_Assessment, 7))
        Me.ErrorLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(Str_Dispid_Assessment, 901))
        Me.CustomLabelOtherCar.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(Str_Dispid_Assessment, 6))
        '2012/04/13 TCS 鈴木(恭) HTMLエンコード対応 END

        Logger.Info(String.Format(CultureInfo.InvariantCulture, Str_Dispid_Assessment & System.Reflection.MethodBase.GetCurrentMethod.Name & "_End"))

    End Sub

    ''' <summary>
    ''' 特定のHiddenの値を初期化する
    ''' </summary>
    ''' <remarks></remarks>
    ''' <History>
    '''  2012/04/13 TCS 鈴木(恭) HTMLエンコード対応
    ''' </History>
    Private Sub InitHiddenField()

        Logger.Info(String.Format(CultureInfo.InvariantCulture, Str_Dispid_Assessment & System.Reflection.MethodBase.GetCurrentMethod.Name & "_Start"))

        Me.SelectAssSeqnoHidden.Value = String.Empty
        Me.SelectAssVinHidden.Value = String.Empty
        Me.SelectInspectionDateHidden.Value = String.Empty
        Me.SelectApprisalPriceHidden.Value = String.Empty
        Me.SelectAssessmentNoHidden.Value = String.Empty
        Me.SelectNoticeReqIdHidden.Value = String.Empty
        Me.SelectStatusHidden.Value = String.Empty
        '2012/04/13 TCS 鈴木(恭) HTMLエンコード対応 START
        Me.SelectOtherAssessmentNoHidden.Value = String.Empty
        Me.SelectOtherNoticeReqIdHidden.Value = String.Empty
        Me.SelectOtherDateHidden.Value = String.Empty
        Me.SelectOtherPriceHidden.Value = String.Empty
        Me.SelectOtherStatusHidden.Value = String.Empty
        Me.SelectOtherUpdateDateHidden.Value = String.Empty
        Me.SelectRetentionHidden.Value = String.Empty
        '2012/04/13 TCS 鈴木(恭) HTMLエンコード対応 END

        Logger.Info(String.Format(CultureInfo.InvariantCulture, Str_Dispid_Assessment & System.Reflection.MethodBase.GetCurrentMethod.Name & "_End"))

    End Sub

    ''' <summary>
    ''' セッションの値をDataRowにセットする。
    ''' </summary>
    ''' <param name="sessionDataRow">顧客情報DataRow</param>
    ''' <remarks></remarks>
    Private Sub SetSessionValue(ByVal sessionDataRow As SC3080301DataSet.SC3080301SessionRow)

        Logger.Info(String.Format(CultureInfo.InvariantCulture, Str_Dispid_Assessment & System.Reflection.MethodBase.GetCurrentMethod.Name & _
                                  "_Start[sessionDataRow:{0}]", sessionDataRow.Table.Rows.Count))

        'ログインユーザー情報取得用
        Dim context As StaffContext = StaffContext.Current

        'セッション情報のセット
        '顧客種別 (1：自社客、2：未取引客)
        sessionDataRow.CUSTSEGMENT = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CUSTFLG, False), String)

        '顧客分類 (1：所有者、2：使用者、3：その他)
        sessionDataRow.CUSTCLASS = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CUSTCLASS, False), String)

        '自社客連番/未取引客ユーザID
        sessionDataRow.ORIGINALID = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_ORIGINALID, False), String)

        '顧客名称
        sessionDataRow.CUSTOMERNAME = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CUSTNAME, False), String)

        '販売店コード
        sessionDataRow.DLRCD = context.DlrCD

        '店舗コード
        sessionDataRow.STRCD = context.BrnCD

        'Follow-up Box販売店コード
        sessionDataRow.FLLWUPBOX_DLRCD = context.DlrCD

        'Follow-up Box店舗コード
        sessionDataRow.FLLWUPBOX_STRCD = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_FLLSTRCD, False), String)

        'Follow-up Box内連番
        sessionDataRow.FLLWUPBOX_SEQNO = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_FLLSEQNO, False), String)

        'スタッフ店舗コード
        sessionDataRow.STRCDSTAFF = context.BrnCD

        'スタッフコード
        sessionDataRow.STAFFCD = context.Account

        'スタッフ名
        sessionDataRow.STAFFNAME = context.UserName

        'スタッフのステータス
        If (context.PresenceCategory = Presence_Category_On And context.PresenceDetail = Presence_Detail_Zero) Or _
            (context.PresenceCategory = Presence_Category_Standby And context.PresenceDetail = Presence_Detail_One) Then
            sessionDataRow.ACCOUNTSTATUS = Account_Status_On
        Else
            sessionDataRow.ACCOUNTSTATUS = Account_Status_Off
        End If

        '2013/01/24 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START
        Dim salesbkgno As String = Nothing
        If ContainsKey(ScreenPos.Current, SESSION_KEY_SALESBKGNO) Then
            salesbkgno = GetValue(ScreenPos.Current, SESSION_KEY_SALESBKGNO, False)
        End If
        If Not String.IsNullOrEmpty(salesbkgno) Then
            sessionDataRow.ACCOUNTSTATUS = Account_Status_Off
        End If
        '2013/01/24 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END

        '更新アカウント
        sessionDataRow.UPDATEACCOUNT = context.Account

        '機能ID
        sessionDataRow.DISPID = Str_Dispid_Assessment

        Logger.Info(String.Format(CultureInfo.InvariantCulture, Str_Dispid_Assessment & System.Reflection.MethodBase.GetCurrentMethod.Name & "_End"))

    End Sub

    ''' <summary>
    ''' その他車両の値をDataRowにセットする。
    ''' </summary>
    ''' <param name="getSessionDataTable">顧客情報DataRow</param>
    ''' <param name="otherDataRow">顧客情報DataRow</param>
    ''' <remarks></remarks>
    Private Sub SetOtherValue(ByVal getSessionDataTable As SC3080301DataSet.SC3080301SessionDataTable, _
                              ByVal otherDataRow As SC3080301DataSet.SC3080301OtherVehicleRow)

        Logger.Info(String.Format(CultureInfo.InvariantCulture, Str_Dispid_Assessment & System.Reflection.MethodBase.GetCurrentMethod.Name & _
                                  "_Start[getSessionDataTable:{0}][otherDataRow:{1}]", getSessionDataTable.Rows.Count, otherDataRow.Table.Rows.Count))

        Dim sessionDataRow As SC3080301DataSet.SC3080301SessionRow = getSessionDataTable.Item(0)

        '顧客ID
        otherDataRow.ORIGINALID = sessionDataRow.ORIGINALID
        '文言（その他車両）
        otherDataRow.CARNAME = WebWordUtility.GetWord(Str_Dispid_Assessment, 6)
        '文言（その他車両）
        otherDataRow.RETENTION = SC3080301BusinessLogic.RetentionOther

        Logger.Info(String.Format(CultureInfo.InvariantCulture, Str_Dispid_Assessment & System.Reflection.MethodBase.GetCurrentMethod.Name & "_End"))

    End Sub

    ''' <summary>
    ''' 画面の値を取得する
    ''' </summary>
    ''' <param name="getSessionDataTable">セッション情報DataTable</param>
    ''' <param name="getVehicleDataTable">車両情報DataTable</param>
    ''' <param name="getOtherVehicleDataTable">車両情報DataTable</param>
    ''' <remarks></remarks>
    ''' <History>
    '''  2012/04/13 TCS 鈴木(恭) HTMLエンコード対応
    ''' </History>
    Private Sub GetDisplayValues(ByVal getSessionDataTable As SC3080301DataSet.SC3080301SessionDataTable, _
                                   ByVal getVehicleDataTable As SC3080301DataSet.SC3080301VehicleDataTable, _
                                   ByVal getOtherVehicleDataTable As SC3080301DataSet.SC3080301OtherVehicleDataTable)

        Logger.Info(String.Format(CultureInfo.InvariantCulture, Str_Dispid_Assessment & System.Reflection.MethodBase.GetCurrentMethod.Name & _
                                  "_Start[getSessionDataTable:{0}][getVehicleDataTable:{1}][getOtherVehicleDataTable:{2}]", _
                                  getSessionDataTable.Rows.Count, getVehicleDataTable.Rows.Count, getOtherVehicleDataTable.Rows.Count))

        Dim sessionDataRow As SC3080301DataSet.SC3080301SessionRow = getSessionDataTable.Item(0)
        Dim otherVehicleDataRow As SC3080301DataSet.SC3080301OtherVehicleRow = getOtherVehicleDataTable.Item(0)

        'Hidden値初期化
        InitHiddenField()

        '値を設定する
        If Not otherVehicleDataRow.IsSTATUSNull AndAlso _
            (otherVehicleDataRow.STATUS = SC3080301BusinessLogic.RequestStatus Or _
             otherVehicleDataRow.STATUS = SC3080301BusinessLogic.RequestReceiveStatus) Then

            'その他車両のセット
            SetDisplayOtherValues(sessionDataRow.DLRCD, sessionDataRow.ACCOUNTSTATUS, otherVehicleDataRow)
        Else
            'その他用フラグ
            Dim displayOtherFlg As String = String.Empty

            If Not otherVehicleDataRow.IsSTATUSNull AndAlso _
                          otherVehicleDataRow.STATUS = SC3080301BusinessLogic.EndStatus Then
                'その他車両のセット
                SetDisplayOtherValues(sessionDataRow.DLRCD, sessionDataRow.ACCOUNTSTATUS, otherVehicleDataRow)
                displayOtherFlg = Display_Flg_Assessment
            ElseIf getVehicleDataTable.Rows.Count = 0 Then
                '2012/04/13 TCS 鈴木(恭) HTMLエンコード対応 START
                Me.SelectRetentionHidden.Value = HttpUtility.HtmlEncode(otherVehicleDataRow.RETENTION)                           '保有
                Me.SelectCarnameHidden.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(Str_Dispid_Assessment, 6))          'その他車両
                '2012/04/13 TCS 鈴木(恭) HTMLエンコード対応 END
                Me.RequestStatusPanel.Style.Item("display") = "none"
                Me.EndStatusPanel.Style.Item("display") = "block"
                Me.AssessmentResultPanel.Style.Item("display") = "none"
                '2013/01/24 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START
                Dim salesbkgno As String = Nothing
                'GetValue(ScreenPos.Current, SESSION_KEY_SALESBKGNO, True)

                If ContainsKey(ScreenPos.Current, SESSION_KEY_SALESBKGNO) Then
                    salesbkgno = GetValue(ScreenPos.Current, SESSION_KEY_SALESBKGNO, False)
                End If
                If sessionDataRow.ACCOUNTSTATUS = Account_Status_On AndAlso String.IsNullOrEmpty(salesbkgno) Then
                    Me.AssessmentEnableButtonPanel.Style.Item("display") = "block"
                    Me.AssessmentDisableButtonPanel.Style.Item("display") = "none"
                Else
                    Me.AssessmentEnableButtonPanel.Style.Item("display") = "none"
                    Me.AssessmentDisableButtonPanel.Style.Item("display") = "block"
                End If
                '2013/01/24 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END
                Me.OtherAssessmentLabel.Style.Item("display") = "block"
                Me.AssessmentRegLabel.Style.Item("display") = "none"
                Me.AssessmentCarLabel.Style.Item("display") = "none"
            End If

            '1件目用フラグ
            Dim displayFirstFlg As String = String.Empty
            '初期表示用フラグ
            Dim displayFlg As String = String.Empty

            '検索結果のセット
            '2012/04/13 TCS 鈴木(恭) HTMLエンコード対応 START
            For Each dt In getVehicleDataTable
                If (Not dt.IsSTATUSNull AndAlso _
                    (dt.STATUS = SC3080301BusinessLogic.RequestStatus Or _
                     dt.STATUS = SC3080301BusinessLogic.RequestReceiveStatus)) Or _
                            (String.IsNullOrEmpty(displayFlg) And Not dt.IsSTATUSNull AndAlso _
                             dt.STATUS = SC3080301BusinessLogic.EndStatus) Then
                    Me.SelectRetentionHidden.Value = HttpUtility.HtmlEncode(SC3080301BusinessLogic.RetentionCustomer)    '保有
                    Me.SelectCarnoHidden.Value = HttpUtility.HtmlEncode(dt.CARNO)                                        '登録No
                    Me.SelectCarnameHidden.Value = HttpUtility.HtmlEncode(dt.CARNAME)                                    'メーカー名/車種名
                    Me.SelectAssVinHidden.Value = HttpUtility.HtmlEncode(dt.VIN)                                         'VIN
                    Me.SelectAssessmentNoHidden.Value = HttpUtility.HtmlEncode(dt.ASSESSMENTNO)                          '査定No
                    Me.SelectNoticeReqIdHidden.Value = HttpUtility.HtmlEncode(dt.NOTICEREQID)                            '通知依頼ID
                    Me.SelectStatusHidden.Value = HttpUtility.HtmlEncode(dt.STATUS)                                      'ステータス
                    Me.SelectAssSeqnoHidden.Value = HttpUtility.HtmlEncode(dt.SEQNO)                                     'SEQNO
                    If dt.STATUS = SC3080301BusinessLogic.RequestStatus Or _
                        dt.STATUS = SC3080301BusinessLogic.RequestReceiveStatus Then
                        Me.RequestTimeLabel.Text = HttpUtility.HtmlEncode(DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, dt.UPDATEDATE, sessionDataRow.DLRCD))    '作成日
                    ElseIf dt.STATUS = SC3080301BusinessLogic.EndStatus Then
                        Me.AssessmentDateLabel.Text = HttpUtility.HtmlEncode(dt.INSPECTIONDATE)                          '検査日
                        Me.AssessmentPriceLabel.Text = HttpUtility.HtmlEncode(dt.APPRISAL_PRICE)                         '提示価格
                    End If
                    If dt.STATUS = SC3080301BusinessLogic.RequestStatus Or _
                        dt.STATUS = SC3080301BusinessLogic.RequestReceiveStatus Then
                        displayFlg = Display_Flg_Cancel
                    ElseIf dt.STATUS = SC3080301BusinessLogic.EndStatus Then
                        displayFlg = Display_Flg_Assessment
                    End If
                ElseIf String.IsNullOrEmpty(displayFirstFlg) Then
                    Me.SelectRetentionHidden.Value = HttpUtility.HtmlEncode(SC3080301BusinessLogic.RetentionCustomer)    '保有
                    Me.SelectCarnoHidden.Value = HttpUtility.HtmlEncode(dt.CARNO)                                        '登録No
                    Me.SelectCarnameHidden.Value = HttpUtility.HtmlEncode(dt.CARNAME)                                    'メーカー名/車種名
                    Me.SelectAssVinHidden.Value = HttpUtility.HtmlEncode(dt.VIN)                                         'VIN
                    Me.SelectAssSeqnoHidden.Value = HttpUtility.HtmlEncode(dt.SEQNO)                                     'SEQNO
                End If
                displayFirstFlg = Display_Flg_Assessment
            Next
            '2012/04/13 TCS 鈴木(恭) HTMLエンコード対応 END
            '保有車両のセット
            SetDisplayRetentionValues(displayFlg, displayFirstFlg, sessionDataRow.DLRCD, sessionDataRow.ACCOUNTSTATUS, displayOtherFlg, otherVehicleDataRow)
        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture, Str_Dispid_Assessment & System.Reflection.MethodBase.GetCurrentMethod.Name & "_End"))

    End Sub

    ''' <summary>
    ''' 画面のその他車両値を設定する
    ''' </summary>
    ''' <param name="sessionDlrcd">販売店コード</param>
    ''' <param name="accountStatus">アカウントステータス</param>
    ''' <param name="otherVehicleDataRow">その他車両情報DataRow</param>
    ''' <remarks></remarks>
    ''' <History>
    '''  2012/04/13 TCS 鈴木(恭) HTMLエンコード対応
    ''' </History>
    Private Sub SetDisplayOtherValues(ByVal sessionDlrcd As String, ByVal accountStatus As String, ByVal otherVehicleDataRow As SC3080301DataSet.SC3080301OtherVehicleRow)

        Logger.Info(String.Format(CultureInfo.InvariantCulture, Str_Dispid_Assessment & System.Reflection.MethodBase.GetCurrentMethod.Name & _
                                  "_Start[accountStatus:{0}][otherVehicleDataRow:{1}]", accountStatus, otherVehicleDataRow.Table.Rows.Count))

        '2012/04/13 TCS 鈴木(恭) HTMLエンコード対応 START
        Me.SelectRetentionHidden.Value = HttpUtility.HtmlEncode(otherVehicleDataRow.RETENTION)                           '保有
        Me.SelectCarnameHidden.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(Str_Dispid_Assessment, 6))          'その他車両
        Me.SelectAssessmentNoHidden.Value = HttpUtility.HtmlEncode(otherVehicleDataRow.ASSESSMENTNO)                     '査定No
        Me.SelectNoticeReqIdHidden.Value = HttpUtility.HtmlEncode(otherVehicleDataRow.NOTICEREQID)                       '通知依頼ID
        Me.SelectStatusHidden.Value = HttpUtility.HtmlEncode(otherVehicleDataRow.STATUS)                                 'ステータス
        Me.SelectOtherAssessmentNoHidden.Value = HttpUtility.HtmlEncode(otherVehicleDataRow.ASSESSMENTNO)                'その他査定No
        Me.SelectOtherNoticeReqIdHidden.Value = HttpUtility.HtmlEncode(otherVehicleDataRow.NOTICEREQID)                  'その他通知依頼ID
        Me.SelectOtherStatusHidden.Value = HttpUtility.HtmlEncode(otherVehicleDataRow.STATUS)                            'その他ステータス

        If otherVehicleDataRow.STATUS = SC3080301BusinessLogic.RequestStatus Or _
            otherVehicleDataRow.STATUS = SC3080301BusinessLogic.RequestReceiveStatus Then
            Me.SelectOtherUpdateDateHidden.Value = HttpUtility.HtmlEncode(otherVehicleDataRow.UPDATEDATE)                      'その他作成日
            Me.RequestTimeLabel.Text = HttpUtility.HtmlEncode(DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, otherVehicleDataRow.UPDATEDATE, sessionDlrcd))     '作成日
            Me.RequestStatusPanel.Style.Item("display") = "block"
            Me.AssessmentDisableCancelButtonPanel.Style.Item("display") = "none"
            Me.EndStatusPanel.Style.Item("display") = "none"
            Me.OtherRequestLabel.Style.Item("display") = "block"
            Me.RequestRegLabel.Style.Item("display") = "none"
            Me.RequestCarLabel.Style.Item("display") = "none"
        ElseIf otherVehicleDataRow.STATUS = SC3080301BusinessLogic.EndStatus Then
            Me.SelectInspectionDateHidden.Value = HttpUtility.HtmlEncode(otherVehicleDataRow.INSPECTIONDATE)                   '検査日
            Me.SelectApprisalPriceHidden.Value = HttpUtility.HtmlEncode(otherVehicleDataRow.APPRISAL_PRICE)                    '提示価格
            Me.SelectOtherDateHidden.Value = HttpUtility.HtmlEncode(otherVehicleDataRow.INSPECTIONDATE)                        'その他検査日
            Me.SelectOtherPriceHidden.Value = HttpUtility.HtmlEncode(otherVehicleDataRow.APPRISAL_PRICE)                       'その他提示価格
            Me.AssessmentDateLabel.Text = HttpUtility.HtmlEncode(otherVehicleDataRow.INSPECTIONDATE)                           '表示用検査日
            Me.AssessmentPriceLabel.Text = HttpUtility.HtmlEncode(otherVehicleDataRow.APPRISAL_PRICE)                          '表示用提示価格
            Me.RequestStatusPanel.Style.Item("display") = "none"
            Me.EndStatusPanel.Style.Item("display") = "block"
            Me.AssessmentResultPanel.Style.Item("display") = "block"
            '2013/01/24 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START
            Dim salesbkgno As String = Nothing
            If ContainsKey(ScreenPos.Current, SESSION_KEY_SALESBKGNO) Then
                salesbkgno = GetValue(ScreenPos.Current, SESSION_KEY_SALESBKGNO, False)
            End If
            If accountStatus = Account_Status_On AndAlso String.IsNullOrEmpty(salesbkgno) Then
                Me.AssessmentEnableButtonPanel.Style.Item("display") = "block"
                Me.AssessmentDisableButtonPanel.Style.Item("display") = "none"
            Else
                Me.AssessmentEnableButtonPanel.Style.Item("display") = "none"
                Me.AssessmentDisableButtonPanel.Style.Item("display") = "block"
            End If
            '2013/01/24 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END
            Me.OtherAssessmentLabel.Style.Item("display") = "block"
            Me.AssessmentRegLabel.Style.Item("display") = "none"
            Me.AssessmentCarLabel.Style.Item("display") = "none"
        End If
        '2012/04/13 TCS 鈴木(恭) HTMLエンコード対応 END

        Logger.Info(String.Format(CultureInfo.InvariantCulture, Str_Dispid_Assessment & System.Reflection.MethodBase.GetCurrentMethod.Name & "_End"))

    End Sub


    ''' <summary>
    ''' 画面の保有車両値を設定する
    ''' </summary>
    ''' <param name="displayFlg">初期表示用フラグ</param>
    ''' <param name="displayFirstFlg">1件目フラグ</param>
    ''' <param name="sessionDlrcd">販売店コード</param>
    ''' <param name="accountStatus">アカウントステータス</param>
    ''' <param name="displayOtherFlg">その他車両フラグ</param>
    ''' <param name="otherVehicleDataRow">その他車両情報DataRow</param>
    ''' <remarks></remarks>
    Private Sub SetDisplayRetentionValues(ByVal displayFlg As String, ByVal displayFirstFlg As String, ByVal sessionDlrcd As String, _
                                          ByVal accountStatus As String, ByVal displayOtherFlg As String, _
                                           ByVal otherVehicleDataRow As SC3080301DataSet.SC3080301OtherVehicleRow)

        Logger.Info(String.Format(CultureInfo.InvariantCulture, Str_Dispid_Assessment & System.Reflection.MethodBase.GetCurrentMethod.Name & _
                                  "_Start[displayFlg:{0}][displayFirstFlg:{1}][accountStatus:{2}][displayOtherFlg:{3}][otherVehicleDataRow:{4}]", _
                                  displayFlg, displayFirstFlg, accountStatus, displayOtherFlg, otherVehicleDataRow.Table.Rows.Count))

        If displayFlg = Display_Flg_Cancel Then
            Me.RequestRegLabel.Text = Me.SelectCarnoHidden.Value
            Me.RequestCarLabel.Text = Me.SelectCarnameHidden.Value
            Me.RequestStatusPanel.Style.Item("display") = "block"
            Me.AssessmentDisableCancelButtonPanel.Style.Item("display") = "none"
            Me.EndStatusPanel.Style.Item("display") = "none"
            Me.OtherRequestLabel.Style.Item("display") = "none"
            Me.RequestRegLabel.Style.Item("display") = "block"
            Me.RequestCarLabel.Style.Item("display") = "block"
        ElseIf displayFirstFlg = Display_Flg_Assessment Then
            If displayFlg = Display_Flg_Assessment Then
                Me.AssessmentRegLabel.Text = Me.SelectCarnoHidden.Value
                Me.AssessmentCarLabel.Text = Me.SelectCarnameHidden.Value
                Me.RequestStatusPanel.Style.Item("display") = "none"
                Me.EndStatusPanel.Style.Item("display") = "block"
                '2013/01/24 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START
                Dim salesbkgno As String = Nothing
                If ContainsKey(ScreenPos.Current, SESSION_KEY_SALESBKGNO) Then
                    salesbkgno = GetValue(ScreenPos.Current, SESSION_KEY_SALESBKGNO, False)
                End If
                If accountStatus = Account_Status_On AndAlso String.IsNullOrEmpty(salesbkgno) Then
                    Me.AssessmentEnableButtonPanel.Style.Item("display") = "block"
                    Me.AssessmentDisableButtonPanel.Style.Item("display") = "none"
                Else
                    Me.AssessmentEnableButtonPanel.Style.Item("display") = "none"
                    Me.AssessmentDisableButtonPanel.Style.Item("display") = "block"
                End If
                '2013/01/24 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END
                Me.OtherAssessmentLabel.Style.Item("display") = "none"
                Me.AssessmentRegLabel.Style.Item("display") = "block"
                Me.AssessmentCarLabel.Style.Item("display") = "block"
                If displayFlg = Display_Flg_Assessment Then
                    Me.AssessmentResultPanel.Style.Item("display") = "block"
                Else
                    Me.AssessmentResultPanel.Style.Item("display") = "none"
                End If
            ElseIf displayOtherFlg = Display_Flg_Assessment Then
                'その他車両のセット
                SetDisplayOtherValues(sessionDlrcd, accountStatus, otherVehicleDataRow)
            Else
                Me.AssessmentRegLabel.Text = Me.SelectCarnoHidden.Value
                Me.AssessmentCarLabel.Text = Me.SelectCarnameHidden.Value
                Me.RequestStatusPanel.Style.Item("display") = "none"
                Me.EndStatusPanel.Style.Item("display") = "block"
                '2013/01/24 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START
                Dim salesbkgno As String = Nothing
                If ContainsKey(ScreenPos.Current, SESSION_KEY_SALESBKGNO) Then
                    salesbkgno = GetValue(ScreenPos.Current, SESSION_KEY_SALESBKGNO, False)
                End If
                If accountStatus = Account_Status_On AndAlso String.IsNullOrEmpty(salesbkgno) Then
                    Me.AssessmentEnableButtonPanel.Style.Item("display") = "block"
                    Me.AssessmentDisableButtonPanel.Style.Item("display") = "none"
                Else
                    Me.AssessmentEnableButtonPanel.Style.Item("display") = "none"
                    Me.AssessmentDisableButtonPanel.Style.Item("display") = "block"
                End If
                '2013/01/24 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END
                Me.OtherAssessmentLabel.Style.Item("display") = "none"
                Me.AssessmentRegLabel.Style.Item("display") = "block"
                Me.AssessmentCarLabel.Style.Item("display") = "block"
                If displayFlg = Display_Flg_Assessment Then
                    Me.AssessmentResultPanel.Style.Item("display") = "block"
                Else
                    Me.AssessmentResultPanel.Style.Item("display") = "none"
                End If
            End If
        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture, Str_Dispid_Assessment & System.Reflection.MethodBase.GetCurrentMethod.Name & "_End"))

    End Sub

    ''' <summary>
    ''' 査定の値を設定する
    ''' </summary>
    ''' <param name="getVehicleDataTable">車両情報DataTable</param>
    ''' <param name="sessionDlrcd">販売店コード</param>
    ''' <remarks></remarks>
    Private Sub SetVehicleAssessmentValues(ByVal getVehicleDataTable As SC3080301DataSet.SC3080301VehicleDataTable, _
                                           ByVal getAssessmentDataTable As SC3080301DataSet.SC3080301UcarAssessmentDataTable, _
                                           ByVal sessionDlrcd As String)

        Logger.Info(String.Format(CultureInfo.InvariantCulture, Str_Dispid_Assessment & System.Reflection.MethodBase.GetCurrentMethod.Name & _
                                  "_Start[getVehicleDataTable:{0}][getAssessmentDataTable:{1}]", getVehicleDataTable.Rows.Count, getAssessmentDataTable.Rows.Count))

        For Each dtVehicle In getVehicleDataTable
            For Each dtAssessment In getAssessmentDataTable
                '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
                'If (dtVehicle.ORIGINALID = dtAssessment.CRCUSTID And _
                '         Not dtVehicle.IsVINNull AndAlso _
                '         Not dtAssessment.IsORGCSTVCL_VINNull AndAlso _
                '         dtVehicle.VIN = dtAssessment.ORGCSTVCL_VIN And _
                '         dtAssessment.CSTKIND = SC3080301BusinessLogic.OrgCustFlg) Then
                If (dtVehicle.ORIGINALID = Trim(dtAssessment.CRCUSTID) And _
                         Not dtVehicle.IsVINNull AndAlso _
                         Not dtAssessment.IsORGCSTVCL_VINNull AndAlso _
                         dtVehicle.VIN = dtAssessment.ORGCSTVCL_VIN And _
                         dtAssessment.CSTKIND = SC3080301BusinessLogic.OrgCustFlg) Then
                '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
                    dtVehicle.ASSESSMENTNO = dtAssessment.ASSESSMENTNO
                    dtVehicle.NOTICEREQID = dtAssessment.NOTICEREQID
                    If Not dtAssessment.IsINSPECTIONDATENull Then
                        dtVehicle.INSPECTIONDATE = DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, dtAssessment.INSPECTIONDATE, sessionDlrcd)
                    End If
                    If Not dtAssessment.IsAPPRISAL_PRICENull Then
                        dtVehicle.APPRISAL_PRICE = Format(dtAssessment.APPRISAL_PRICE, AssessmentPriceFormat)
                    End If
                    dtVehicle.UPDATEDATE = dtAssessment.UPDATEDATE
                    dtVehicle.STATUS = dtAssessment.STATUS
                '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
                'ElseIf (dtVehicle.ORIGINALID = dtAssessment.CRCUSTID And _
                '         Not dtVehicle.IsSEQNONull AndAlso _
                '         Not dtAssessment.IsNEWCSTVCL_SEQNONull AndAlso _
                '         dtVehicle.SEQNO = dtAssessment.NEWCSTVCL_SEQNO And _
                '         dtAssessment.CSTKIND = SC3080301BusinessLogic.NewCustFlg) Then
                ElseIf (dtVehicle.ORIGINALID = Trim(dtAssessment.CRCUSTID) And _
                         Not dtVehicle.IsSEQNONull AndAlso _
                         Not dtAssessment.IsNEWCSTVCL_SEQNONull AndAlso _
                         dtVehicle.SEQNO = dtAssessment.NEWCSTVCL_SEQNO And _
                         dtAssessment.CSTKIND = SC3080301BusinessLogic.NewCustFlg) Then
                '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
                    dtVehicle.ASSESSMENTNO = dtAssessment.ASSESSMENTNO
                    dtVehicle.NOTICEREQID = dtAssessment.NOTICEREQID
                    If Not dtAssessment.IsINSPECTIONDATENull Then
                        dtVehicle.INSPECTIONDATE = DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, dtAssessment.INSPECTIONDATE, sessionDlrcd)
                    End If
                    If Not dtAssessment.IsAPPRISAL_PRICENull Then
                        dtVehicle.APPRISAL_PRICE = Format(dtAssessment.APPRISAL_PRICE, AssessmentPriceFormat)
                    End If
                    dtVehicle.UPDATEDATE = dtAssessment.UPDATEDATE
                    dtVehicle.STATUS = dtAssessment.STATUS
                End If
            Next
        Next

        Logger.Info(String.Format(CultureInfo.InvariantCulture, Str_Dispid_Assessment & System.Reflection.MethodBase.GetCurrentMethod.Name & "_End"))

    End Sub

    ''' <summary>
    ''' その他車両の値を設定する
    ''' </summary>
    ''' <param name="getOtherVehicleDataTable">顧客情報DataTable</param>
    ''' <param name="getAssessmentDataTable">顧客情報DataTable</param>
    ''' <param name="sessionDlrcd">販売店コード</param>
    ''' <remarks></remarks>
    Private Sub SetOtherVehicleAsssessmentValues(ByVal getOtherVehicleDataTable As SC3080301DataSet.SC3080301OtherVehicleDataTable, _
                                                 ByVal getAssessmentDataTable As SC3080301DataSet.SC3080301UcarAssessmentDataTable, _
                                                 ByVal sessionDlrcd As String)

        Logger.Info(String.Format(CultureInfo.InvariantCulture, Str_Dispid_Assessment & System.Reflection.MethodBase.GetCurrentMethod.Name & _
                                  "_Start[getOtherVehicleDataTable:{0}][getAssessmentDataTable:{1}]", getOtherVehicleDataTable.Rows.Count, getAssessmentDataTable.Rows.Count))

        Dim otherDataRow As SC3080301DataSet.SC3080301OtherVehicleRow = getOtherVehicleDataTable.Item(0)

        '値を設定する
        For Each dtAssessment In getAssessmentDataTable
            If dtAssessment.RETENTION = SC3080301BusinessLogic.RetentionOther Then
                'otherDataRow.RETENTION = dtAssessment.RETENTION
                otherDataRow.ASSESSMENTNO = dtAssessment.ASSESSMENTNO
                otherDataRow.NOTICEREQID = dtAssessment.NOTICEREQID
                If Not dtAssessment.IsINSPECTIONDATENull Then
                    otherDataRow.INSPECTIONDATE = DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, dtAssessment.INSPECTIONDATE, sessionDlrcd)
                End If
                If Not dtAssessment.IsAPPRISAL_PRICENull Then
                    otherDataRow.APPRISAL_PRICE = Format(dtAssessment.APPRISAL_PRICE, AssessmentPriceFormat)
                End If
                otherDataRow.UPDATEDATE = dtAssessment.UPDATEDATE
                otherDataRow.STATUS = dtAssessment.STATUS
            End If
        Next

        Logger.Info(String.Format(CultureInfo.InvariantCulture, Str_Dispid_Assessment & System.Reflection.MethodBase.GetCurrentMethod.Name & "_End"))

    End Sub

    ''' <summary>
    ''' 査定依頼登録イベント
    ''' </summary>
    ''' <remarks></remarks>
    Private Function AssessmentRegisterInfo(ByVal registAssessmentNo As Long, ByVal registNoticeReqId As Long, _
                                            ByVal registRetention As String, ByVal registVin As String, ByVal registSeqno As Long) As String

        Logger.Info(String.Format(CultureInfo.InvariantCulture, Str_Dispid_Assessment & System.Reflection.MethodBase.GetCurrentMethod.Name & _
                                  "_Start[registAssessmentNo:{0}][registNoticeReqId:{1}][registRetention:{2}][registVin:{3}][registSeqno:{4}]", _
                                  registAssessmentNo, registNoticeReqId, registRetention, registVin, registSeqno))

        ' 査定ボタン押下処理	
        ' 引数定義
        Dim selectSeqno As Long
        Dim selectAssessmentNo As Long
        Dim selectNoticeReqId As Long
        ' 引数取得
        If registAssessmentNo > SC3080301BusinessLogic.CountCheckZero Then
            selectAssessmentNo = registAssessmentNo
        End If
        If registNoticeReqId > SC3080301BusinessLogic.CountCheckZero Then
            selectNoticeReqId = registNoticeReqId
        End If

        Dim bizLogic As New SC3080301BusinessLogic
        Dim GetRegAssessment As String = SC3080301BusinessLogic.SuccessIfZero
        Dim bizLogicSC3080301 As SC3080301BusinessLogic
        bizLogicSC3080301 = New SC3080301BusinessLogic

        Using getSessionDataTable = New SC3080301DataSet.SC3080301SessionDataTable
            Dim sessionDataRow As SC3080301DataSet.SC3080301SessionRow

            sessionDataRow = getSessionDataTable.NewSC3080301SessionRow
            SetSessionValue(sessionDataRow)                              'セッション値の取得
            getSessionDataTable.Rows.Add(sessionDataRow)                 '追加する

            '来店情報取得
            Using getVisitSalesDataTable = New SC3080301DataSet.SC3080301VisitSalesDataTable
                Dim visitSalesList As SC3080301DataSet.SC3080301VisitSalesDataTable = _
                    bizLogicSC3080301.GetVisitSalesList(getSessionDataTable)
                'テーブルNo
                If visitSalesList IsNot Nothing AndAlso Not visitSalesList.Rows.Count = SC3080301BusinessLogic.CountCheckZero Then
                    Dim visitSalesRow As SC3080301DataSet.SC3080301VisitSalesRow = visitSalesList.Item(0)
                    If Not visitSalesRow.IsSALESTABLENONull Then
                        sessionDataRow.SALESTABLENO = visitSalesRow.SALESTABLENO
                    End If
                End If
            End Using

            If sessionDataRow.CUSTSEGMENT = SC3080301BusinessLogic.NewCustFlg And registRetention = SC3080301BusinessLogic.RetentionCustomer Then
                selectSeqno = registSeqno
            End If

            '端末ID取得
            Using getTerminalDataTable = New SC3080301DataSet.SC3080301UcarTerminalDataTable
                Dim ucarTerminalDataTable As SC3080301DataSet.SC3080301UcarTerminalDataTable = _
                    bizLogicSC3080301.GetTerminalList(sessionDataRow.DLRCD, sessionDataRow.STRCD)

                GetRegAssessment = bizLogic.RegistUcarAssessment(SC3080301BusinessLogic.RequestStatus, registRetention, registVin, selectSeqno, _
                                                                 selectAssessmentNo, selectNoticeReqId, sessionDataRow, ucarTerminalDataTable)
                
                If Not IsNothing(GetRegAssessment) Then
                    If GetRegAssessment.Equals(IfDbTimeOut) Then
                        'IFの結果がDBタイムアウトの場合
                        'ShowMessageBox(ErrMessageIdNoticeIf)
                        Return GetRegAssessment
                    End If
                End If
            End Using
        End Using

        Return GetRegAssessment

        Logger.Info(String.Format(CultureInfo.InvariantCulture, Str_Dispid_Assessment & System.Reflection.MethodBase.GetCurrentMethod.Name & "_End"))

    End Function

    ''' <summary>
    ''' 査定依頼キャンセルイベント
    ''' </summary>
    ''' <remarks></remarks>
    Private Function AssessmentCancelInfo(ByVal registAssessmentNo As Long, ByVal registNoticeReqId As Long, ByVal registRetention As String, _
                                          ByVal registVin As String, ByVal registSeqno As Long) As String

        Logger.Info(String.Format(CultureInfo.InvariantCulture, Str_Dispid_Assessment & System.Reflection.MethodBase.GetCurrentMethod.Name & _
                                  "_Start[registAssessmentNo:{0}][registNoticeReqId:{1}][registRetention:{2}][registVin:{3}][registSeqno:{4}]", _
                                  registAssessmentNo, registNoticeReqId, registRetention, registVin, registSeqno))

        ' キャンセルボタン押下処理	
        ' 引数定義
        Dim selectSeqno As Long
        Dim selectAssessmentNo As Long
        Dim selectNoticeReqId As Long
        ' 引数取得
        If registAssessmentNo > SC3080301BusinessLogic.CountCheckZero Then
            selectAssessmentNo = registAssessmentNo
        End If
        If registNoticeReqId > SC3080301BusinessLogic.CountCheckZero Then
            selectNoticeReqId = registNoticeReqId
        End If

        Dim bizLogic As New SC3080301BusinessLogic
        Dim GetRegAssessment As String = SC3080301BusinessLogic.SuccessIfZero
        Dim bizLogicSC3080301 As SC3080301BusinessLogic
        bizLogicSC3080301 = New SC3080301BusinessLogic

        Using getSessionDataTable = New SC3080301DataSet.SC3080301SessionDataTable
            Dim sessionDataRow As SC3080301DataSet.SC3080301SessionRow

            sessionDataRow = getSessionDataTable.NewSC3080301SessionRow
            SetSessionValue(sessionDataRow)                              'セッション値の取得
            getSessionDataTable.Rows.Add(sessionDataRow)                 '追加する

            '来店情報取得
            Using getVisitSalesDataTable = New SC3080301DataSet.SC3080301VisitSalesDataTable
                Dim visitSalesList As SC3080301DataSet.SC3080301VisitSalesDataTable = _
                    bizLogicSC3080301.GetVisitSalesList(getSessionDataTable)
                'テーブルNo
                If visitSalesList IsNot Nothing AndAlso Not visitSalesList.Rows.Count = SC3080301BusinessLogic.CountCheckZero Then
                    Dim visitSalesRow As SC3080301DataSet.SC3080301VisitSalesRow = visitSalesList.Item(0)
                    If Not visitSalesRow.IsSALESTABLENONull Then
                        sessionDataRow.SALESTABLENO = visitSalesRow.SALESTABLENO
                    End If
                End If
            End Using

            If sessionDataRow.CUSTSEGMENT = SC3080301BusinessLogic.NewCustFlg And registRetention = SC3080301BusinessLogic.RetentionCustomer Then
                selectSeqno = registSeqno
            End If

            '端末ID取得
            Using getTerminalDataTable = New SC3080301DataSet.SC3080301UcarTerminalDataTable
                Dim ucarTerminalDataTable As SC3080301DataSet.SC3080301UcarTerminalDataTable = _
                    bizLogicSC3080301.GetTerminalList(sessionDataRow.DLRCD, sessionDataRow.STRCD)
                GetRegAssessment = bizLogic.RegistUcarAssessment(SC3080301BusinessLogic.CancelStatus, registRetention, registVin, selectSeqno, _
                                                                 selectAssessmentNo, selectNoticeReqId, sessionDataRow, ucarTerminalDataTable)

                If Not IsNothing(GetRegAssessment) Then
                    If GetRegAssessment.Equals(IfDbTimeOut) Then
                        'IFの結果がDBタイムアウトの場合
                        'ShowMessageBox(ErrMessageIdNoticeIf)
                        Return GetRegAssessment
                    End If
                End If
            End Using
        End Using

        Return GetRegAssessment

        Logger.Info(String.Format(CultureInfo.InvariantCulture, Str_Dispid_Assessment & System.Reflection.MethodBase.GetCurrentMethod.Name & "_End"))

    End Function

    ''' <summary>
    ''' カーチェックシート画面遷移イベント
    ''' </summary>
    ''' <remarks></remarks>
    ''' <History>
    '''  2012/04/13 TCS 鈴木(恭) HTMLエンコード対応
    ''' </History>
    Private Sub linkCarCheckSheetButtonDummy_Click(sender As Object, e As System.EventArgs) Handles linkCarCheckSheetButtonDummy.Click

        Logger.Info(String.Format(CultureInfo.InvariantCulture, Str_Dispid_Assessment & System.Reflection.MethodBase.GetCurrentMethod.Name & "_Start[sender:{0}][e:{1}]", sender.ToString, e.ToString))

        'セッション情報格納
        '2012/04/13 TCS 鈴木(恭) HTMLエンコード対応 START
        SetValue(ScreenPos.Next, SESSION_KEY_NOTICEREQID, HttpUtility.HtmlDecode(Me.SelectNoticeReqIdHidden.Value))          '査定依頼ID
        SetValue(ScreenPos.Next, SESSION_KEY_ASSESSMENTNO, HttpUtility.HtmlDecode(Me.SelectAssessmentNoHidden.Value))        '査定No

        If Me.SelectRetentionHidden.Value = SC3080301BusinessLogic.RetentionCustomer Then
            If DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CUSTFLG, False), String) = SC3080301BusinessLogic.OrgCustFlg Then
                SetValue(ScreenPos.Next, SESSION_KEY_VCLID, HttpUtility.HtmlDecode(Me.SelectAssVinHidden.Value))                                                         '車両ID(自社客VIN)
            ElseIf DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CUSTFLG, False), String) = SC3080301BusinessLogic.NewCustFlg Then
                SetValue(ScreenPos.Next, SESSION_KEY_VCLID, HttpUtility.HtmlDecode(Me.SelectAssSeqnoHidden.Value))                                                       '車両ID(未取引客Seqno)
            End If
        End If
        '2012/04/13 TCS 鈴木(恭) HTMLエンコード対応 END
        SetValue(ScreenPos.Next, SESSION_KEY_FLLSTRCD, DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_FLLSTRCD, False), String))             '店舗コード(Follow-up活動のキー)
        SetValue(ScreenPos.Next, SESSION_KEY_FLLSEQNO, DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_FLLSEQNO, False), String))             'Follow-up 活動連番(Follow-up活動のキー)
        SetValue(ScreenPos.Next, SESSION_KEY_ORIGINALID, DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_ORIGINALID, False), String))         '顧客コード (自社客=自社客連番 / 未取引客=未取引客ユーザーID)
        SetValue(ScreenPos.Next, SESSION_KEY_CUSTFLG, DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CUSTFLG, False), String))               '顧客種別 (1：自社客 2：未取引客)
        SetValue(ScreenPos.Next, SESSION_KEY_CUSTCLASS, DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CUSTCLASS, False), String))           '顧客分類 (1：所有者、2：使用者、3：その他)
        SetValue(ScreenPos.Next, SESSION_KEY_SALESSTAFFCD, DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_SALESSTAFFCD, False), String))     '担当セールススタッフコード

        '画面遷移
        CType(Me.Page, BasePage).RedirectNextScreen(Str_Dispid_Carchecksheet)

        Logger.Info(String.Format(CultureInfo.InvariantCulture, Str_Dispid_Assessment & System.Reflection.MethodBase.GetCurrentMethod.Name & "_End[sender:{0}][e:{1}]", sender.ToString, e.ToString))

    End Sub

#End Region

#Region " ページクラス処理のバイパス処理 "
    '2013/01/24 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START
    Private Function GetPageInterface() As ICustomerDetailControl

        Logger.Info(String.Format(CultureInfo.InvariantCulture, Str_Dispid_Assessment & System.Reflection.MethodBase.GetCurrentMethod.Name & "_StartEnd[Return:{0}]", CType(Me.Page, ICommonSessionControl).ToString))

        Return CType(Me.Page, ICustomerDetailControl)
    End Function

    Private Sub SetValue(pos As ScreenPos, key As String, value As Object)

        Logger.Info(String.Format(CultureInfo.InvariantCulture, Str_Dispid_Assessment & System.Reflection.MethodBase.GetCurrentMethod.Name & _
                                  "_StartEnd[pos:{0}][key:{1}][value:{2}]", pos.ToString, key, value.ToString))

        GetPageInterface().SetValueBypass(pos, key, value)
    End Sub

    Private Function GetValue(pos As ScreenPos, key As String, removeFlg As Boolean) As Object

        Logger.Info(String.Format(CultureInfo.InvariantCulture, Str_Dispid_Assessment & System.Reflection.MethodBase.GetCurrentMethod.Name & _
                                  "_StartEnd[pos:{0}][key:{1}][removeFlg:{2}][Return:{3}]", _
                                  pos.ToString, key, removeFlg.ToString, GetPageInterface().GetValueBypass(pos, key, removeFlg).ToString))

        Return GetPageInterface().GetValueBypass(pos, key, removeFlg)
    End Function

    Private Function ContainsKey(pos As Toyota.eCRB.SystemFrameworks.Web.ScreenPos, key As String) As Boolean
        Return GetPageInterface().ContainsKeyBypass(pos, key)
    End Function
    '2013/01/24 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END
#End Region
End Class
