'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3070207BusinessLogic.vb
'─────────────────────────────────────
'機能： 注文承認
'補足： 
'作成： 2013/12/10 TCS 山口  Aカード情報相互連携開発
'更新： 2017/05/11 TCS 河原  TR-SLT-TMT-20161020-001
'更新： 2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 
'─────────────────────────────────────

Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.Estimate.Quotation.DataAccess
Imports Toyota.eCRB.Tool.Notify.Api.BizLogic
Imports Toyota.eCRB.Tool.Notify.Api.DataAccess
Imports Toyota.eCRB.CommonUtility.BizLogic
Imports System.Globalization
Imports System.Reflection.MethodBase
Imports Toyota.eCRB.iCROP.BizLogic.IC3802801



Public Class SC3070207BusinessLogic
    Inherits BaseBusinessComponent

#Region "定数"
    ''' <summary>
    ''' ログ出力メッセージ1
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ERROR_MSG1 As String = "Tact Error : ReturnId = "

    ''' <summary>
    ''' ログ出力メッセージ2
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ERROR_MSG2 As String = "NoticeRequestI/F Error : ReturnId = "

    ''' <summary>
    ''' 活動結果登録連携可否フラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const UseDmsActivityLink As String = "USE_DMS_ACTIVITY_LINK"

    ''' <summary>
    ''' 活動結果登録連携可否フラグ 1: SA01連携を使用する
    ''' </summary>
    ''' <remarks></remarks>
    Private Const UseDmsActivityLinkUse As String = "1"

    ''' <summary>
    ''' 契約状況フラグ　契約済み
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONTRACTFLG_CONTRACT As String = "1"

    ''' <summary>
    ''' 支払区分　現金
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PAYMENTMETHOD_MONEY As String = "1"

    ''' <summary>
    ''' 支払区分　ローン
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PAYMENTMETHOD_LOAN As String = "2"

    ''' <summary>
    ''' SA04 送信タイプ設定名
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SYSENVKEY_SA04_SEND_XML As String = "DMS_SA04_SEND_XML"
    ''' <summary>
    ''' SA04 送信タイプ設定値（XML使用時）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SA04_SEND_XML As String = "1"

    ''' <summary>
    ''' 文言
    ''' </summary>
    ''' <remarks></remarks>
    Public Const MsgId901 As Integer = 70901 '承認依頼がキャンセルされています。
    Public Const MsgId902 As Integer = 70902 '承認に失敗しました。
    Public Const MsgId903 As Integer = 70903 '否認に失敗しました。
    Public Const MsgId904 As Integer = 70904 'マネージャーコメントは256桁以内で入力してください。
    Public Const MsgId905 As Integer = 70905 'マネージャーコメントは禁則文字以外で入力してください。
    Public Const MsgId906 As Integer = 70906 '注文承認依頼を承認しました。
    Public Const MsgId907 As Integer = 70907 '注文承認依頼を否認しました。

    ''' <summary>
    ''' ステータス
    ''' </summary>
    ''' <remarks></remarks>
    Private Enum RequestTypeEnum
        Approval
        Denial
    End Enum
#End Region

#Region "メンバ変数"

    ''' <summary>
    ''' メッセージID
    ''' </summary>
    ''' <remarks></remarks>
    Private _msgId As Integer = 0

#End Region

#Region "プロパティ"
    ''' <summary>
    ''' 商談情報の対象を保持
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Property SalesTempFlg As SC3070207TableAdapter.SalesTemp

    ''' <summary>
    ''' メッセージID
    ''' </summary>
    ''' <value>メッセージID</value>
    ''' <returns></returns>
    ''' <remarks>0の場合は正常、それ以外の場合エラー</remarks>
    Public ReadOnly Property MsgId() As Integer
        Get
            Return Me._msgId
        End Get
    End Property
#End Region

#Region "見積情報取得IF用の定数"
    ''' <summary>
    ''' 契約情報連携IFのパス
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TactOrderPath As String = "TACT_ORDER_PATH"

    ''' <summary>
    ''' 実行モード　見積情報取得用
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ESTIMATION_MODE_ALL As Integer = 0

    ''' <summary>
    ''' 変換モード　見積情報取得用
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CHANGE_MODE_NOT_TCV As Integer = 0

    ''' <summary>
    ''' Dictinay　key 終了コード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DIC_KEY_ID As String = "ID"

    ''' <summary>
    ''' Dictinay　key メッセージ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DIC_KEY_MSG As String = "MSG"

    ''' <summary>
    ''' Dictinay　key 契約書NO
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DIC_KEY_NO As String = "NO"
#End Region

#Region "通知依頼IF用の定数"
    ''' <summary>
    ''' 通知 000000:成功
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ResultIdSuccess As String = "000000"

    ''' <summary>
    ''' 依頼種別（契約承認）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NOTICE_IF_ORDER_APPROVAL As String = "08"

    ''' <summary>
    ''' ステータス（承認）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NOTICE_IF_APPROVAL As String = "4"

    ''' <summary>
    ''' ステータス（否認）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NOTICE_IF_DENIAL As String = "5"

    ''' <summary>
    ''' (カテゴリータイプ)  : Popup
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NOTICE_IF_PUSHCATEGORY_POPUP As String = "1"

    ''' <summary>
    ''' (表示位置) : header
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NOTICE_IF_POSITION_HEADER As String = "1"

    ''' <summary>
    ''' (表示時間) 
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NOTICE_IF_TIME As Long = 3

    ''' <summary>
    ''' (表示タイプ) : Text()
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NOTICE_IF_DISPLAY_TYPE_TEXT As String = "1"

    ''' <summary>
    ''' (色) : 薄い黄色
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NOTICE_IF_COLOR As String = "1"

    ''' <summary>
    ''' (表示時関数)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NOTICE_IF_DISPLAY_FUNCTION As String = "icropScript.ui.setNotice()"

    ''' <summary>
    ''' (アクション時関数)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NOTICE_IF_ACTION_FUNCTION As String = "icropScript.ui.openNoticeDialog()"
#End Region

#Region "Public"

    ''' <summary>
    ''' 承認処理
    ''' </summary>
    ''' <param name="parameter">パラメータDataTable</param>
    ''' <returns>処理結果 正常終了はTrue</returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Function InsertApproval(ByVal parameter As SC3070207DataSet.SC3070207ParameterDataTable) As Boolean
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_Start", GetCurrentMethod.Name), True)

        Dim drParameter As SC3070207DataSet.SC3070207ParameterRow
        drParameter = CType(parameter.Rows(0), SC3070207DataSet.SC3070207ParameterRow)


        Try
            '処理結果
            Dim result As Integer = 0

            '見積情報ロック取得
            Dim dtEstimateinfoLock As SC3070207DataSet.SC3070207EstimateinfoLockDataTable = _
                            SC3070207TableAdapter.GetEstimateinfoLock(drParameter.ESTIMATEID)
            If dtEstimateinfoLock.Rows.Count = 0 Then
                Me.Rollback = True
                Me._msgId = MsgId902
                '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 START
                Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name))
                '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 END                
                Return False
            End If

            For Each drEstimateinfoLock In dtEstimateinfoLock
                '承認依頼がキャンセルされている場合、処理終了
                If SC3070207TableAdapter.StatusApprovalRequest.Equals(drEstimateinfoLock.CONTRACT_APPROVAL_STATUS) = False Then
                    Me._msgId = MsgId901
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name), True)
                    Return False
                Else
                    '商談ID保持
                    drParameter.SALES_ID = drEstimateinfoLock.FLLWUPBOX_SEQNO
                    '通知先スタッフ保持
                    drParameter.TOACCOUNT = drEstimateinfoLock.CONTRACT_APPROVAL_REQUESTSTAFF
                End If
            Next

            '2017/05/11 TCS 河原  TR-SLT-TMT-20161020-001 START
            '未存在希望車種登録処理
            Dim resultSeq As Long = ActivityInfoBusinessLogic.InsertNotRegSelectedSeries(drParameter.ESTIMATEID, _
                                                                                         drParameter.ACCOUNT, _
                                                                                         drParameter.SALES_ID)
            If resultSeq < 0 Then
                Me.Rollback = True
                Me._msgId = MsgId902
                '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 START
                Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name))
                '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 END
                Return False
            End If
            '2017/05/11 TCS 河原  TR-SLT-TMT-20161020-001 END

            'A-Card番号取得
            Dim aCardNum As String = GetACardNum(drParameter.SALES_ID)

            '活動結果登録連携可否フラグを取得する
            Dim useDmsActivityLinkFlg As String = GetSystemEnvSetting(UseDmsActivityLink)
            If UseDmsActivityLinkUse.Equals(useDmsActivityLinkFlg) And String.IsNullOrEmpty(aCardNum) Then
                '活動結果登録連携可否フラグが「1: SA01連携を使用する」かつ
                'A-Card番号が未取得の場合

                '商談情報or商談一時情報をロック
                Dim dtSalesDataTable As SC3070207DataSet.SC3070207SalesDataTable = _
                    SC3070207TableAdapter.GetSales(drParameter.SALES_ID, Me.SalesTempFlg, True)
                If dtSalesDataTable.Rows.Count = 0 Then
                    Me.Rollback = True
                    Me._msgId = MsgId902
                    '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 START
                    Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name))
                    '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 END
                    Return False
                End If

                '活動連携(SA01)処理
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "SA01_Start (SalesID:{0})", drParameter.SALES_ID), True)
                Dim sa01 As New IC3802801BusinessLogic
                Dim sa01Result As Boolean = sa01.Main(drParameter.SALES_ID)
                If (sa01Result) Then
                    aCardNum = sa01.Main_ACardNo()
                End If
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "SA01_End (A-CardNum:{0})", aCardNum), True)

                'A-Card番号更新
                result = SC3070207TableAdapter.UpdateACardNo(drParameter.SALES_ID, _
                                                             aCardNum, _
                                                             drParameter.ACCOUNT, _
                                                             Me.SalesTempFlg)
                If result = 0 Then
                    Me.Rollback = True
                    Me._msgId = MsgId902
                    '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 START
                    Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name))
                    '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 END
                    Return False
                End If
            End If

            '注文連携(SA04)処理
            '支払方法区分、頭金支払方法区分取得
            Dim dtPaymentInfo As SC3070207DataSet.SC3070207PaymentInfoDataTable = _
                    SC3070207TableAdapter.GetPaymentInfo(drParameter.ESTIMATEID)
            Dim drPaymentInfo As SC3070207DataSet.SC3070207PaymentInfoRow = Nothing
            If dtPaymentInfo.Rows.Count() > 0 Then
                drPaymentInfo = CType(dtPaymentInfo.Rows(0), SC3070207DataSet.SC3070207PaymentInfoRow)
                drParameter.PAYMENTMETHOD = drPaymentInfo.PAYMENTMETHOD
            End If

            '見積情報取得
            Dim bizLogicIC3070201 As New IC3070201BusinessLogic
            Dim dsIC3070201 As IC3070201DataSet
            '見積情報取得
            dsIC3070201 = bizLogicIC3070201.GetEstimationInfo(drParameter.ESTIMATEID, ESTIMATION_MODE_ALL, CHANGE_MODE_NOT_TCV)
            '販売店システム設定取得
            Dim dlrEnv As New DealerEnvSetting
            Dim dlrEnvRow As DlrEnvSettingDataSet.DLRENVSETTINGRow = dlrEnv.GetEnvSetting("XXXXX", TactOrderPath)
            'システム環境設定取得
            Dim sysEnv As New SystemEnvSetting
            Dim useXmlSend As Boolean = SA04_SEND_XML.Equals(sysEnv.GetSystemEnvSetting(SYSENVKEY_SA04_SEND_XML).PARAMVALUE)
            sysEnv = Nothing

            'TACT連携
            Dim webClient As New SC3070207WebClient
            Dim resultTact As Dictionary(Of String, String) = webClient.RequestHttp(aCardNum, _
                                                                                    drPaymentInfo, _
                                                                                    dsIC3070201, _
                                                                                    dlrEnvRow, _
                                                                                    StaffContext.Current, _
                                                                                    useXmlSend)
            '契約書No
            Dim constractNo As String = String.Empty
            '処理結果判定
            If resultTact.ContainsKey(DIC_KEY_ID) Then
                If Not "0".Equals(resultTact.Item(DIC_KEY_ID)) Then
                    Me.Rollback = True
                    Me._msgId = MsgId902
                    Logger.Error(ERROR_MSG1 & resultTact.Item(DIC_KEY_ID))
                    '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 START
                    Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name))
                    '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 END
                    Return False
                Else
                    If resultTact.ContainsKey(DIC_KEY_NO) Then
                        '契約書No設定
                        constractNo = resultTact.Item(DIC_KEY_NO)
                    End If
                End If
            Else
                Me.Rollback = True
                Me._msgId = MsgId902
                '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 START
                Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name))
                '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 END
                Return False
            End If

            '見積情報更新
            result = SC3070207TableAdapter.UpdateEstimateInfo(drParameter.ESTIMATEID, _
                                                              SC3070207TableAdapter.StatusApproval, _
                                                              drParameter.ACCOUNT, _
                                                              drParameter.DLR_CD, _
                                                              constractNo, _
                                                              CONTRACTFLG_CONTRACT)
            If result = 0 Then
                Me.Rollback = True
                Me._msgId = MsgId902
                '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 START
                Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name))
                '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 END
                Return False
            End If

            '2015/03/13 TCS 鈴木【TMT課題対応(#72 セールスタブレットの価格相談履歴表示)】ADD START
            'マネージャ回答登録
            Dim chkFlg As Boolean = SC3070207TableAdapter.RegistAnswer(drParameter.ESTIMATEID, _
                                                                       GetContractApprovalSequence(drParameter.ESTIMATEID), _
                                                                       drParameter.ACCOUNT, _
                                                                       drParameter.DISPLAYCONTENTS,
                                                                       SC3070207TableAdapter.StatusApproval)

            '更新失敗の場合、ロールバックし処理を終了する
            If chkFlg = False Then
                Me.Rollback = True
                '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 START
                Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name))
                '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 END
                Return False
            End If
            '2015/03/13 TCS 鈴木【TMT課題対応(#72 セールスタブレットの価格相談履歴表示)】ADD END

            '見積作成画面上で選択されていない支払方法を設定
            Dim payment As String = String.Empty
            If PAYMENTMETHOD_MONEY.Equals(drParameter.PAYMENTMETHOD) Then
                payment = PAYMENTMETHOD_LOAN
            Else
                payment = PAYMENTMETHOD_MONEY
            End If

            '見積支払情報削除
            result = SC3070207TableAdapter.DeleteEstPaymentinfo(drParameter.ESTIMATEID, _
                                                                payment, _
                                                                drParameter.ACCOUNT)
            If result = 0 Then
                Me.Rollback = True
                Me._msgId = MsgId902
                '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 START
                Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name))
                '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 END
                Return False
            End If


            '承認依頼スタッフが自身の場合、通知を行わない
            If drParameter.NOTICEREQID <> 0 Then
                Dim returnXmlNotice As XmlCommon

                '通知用データ取得
                drParameter = GetNoticeRequest(drParameter)

                '承認結果通知処理
                returnXmlNotice = NoticeRequest(drParameter, RequestTypeEnum.Approval)

                If ResultIdSuccess.Equals(returnXmlNotice.ResultId) = False Then
                    '通知が失敗した場合
                    Me.Rollback = True
                    Me._msgId = MsgId902
                    Logger.Error(ERROR_MSG2 & returnXmlNotice.ResultId)
                    '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 START
                    Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name))
                    '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 END
                    Return False
                End If
            End If

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name), True)

            '正常終了
            Return True

        Catch ex As Exception

            Toyota.eCRB.SystemFrameworks.Core.Logger.Info(ex.Message)
            Toyota.eCRB.SystemFrameworks.Core.Logger.Error("Error Log", ex)

            Me.Rollback = True
            Me._msgId = MsgId902
            '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 START
            Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name))
            '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 END

            Return False

        End Try

    End Function

    ''' <summary>
    ''' 否認処理
    ''' </summary>
    ''' <param name="parameter">パラメータDataTable</param>
    ''' <returns>処理結果 正常終了はTrue</returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Function InsertDenial(ByVal parameter As SC3070207DataSet.SC3070207ParameterDataTable) As Boolean
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_Start", GetCurrentMethod.Name), True)

        Dim drParameter As SC3070207DataSet.SC3070207ParameterRow
        drParameter = CType(parameter.Rows(0), SC3070207DataSet.SC3070207ParameterRow)

        Try
            '処理結果
            Dim result As Integer = 0

            '見積情報ロック取得
            Dim dtEstimateinfoLock As SC3070207DataSet.SC3070207EstimateinfoLockDataTable = _
                            SC3070207TableAdapter.GetEstimateinfoLock(drParameter.ESTIMATEID)
            If dtEstimateinfoLock.Rows.Count = 0 Then
                Me.Rollback = True
                Me._msgId = MsgId903
                '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 START
                Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name))
                '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 END                
                Return False
            End If

            For Each drEstimateinfoLock In dtEstimateinfoLock
                '承認依頼がキャンセルされている場合、処理終了
                If SC3070207TableAdapter.StatusApprovalRequest.Equals(drEstimateinfoLock.CONTRACT_APPROVAL_STATUS) = False Then
                    Me._msgId = MsgId901
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name), True)
                    Return False
                Else
                    '通知先スタッフ保持
                    drParameter.TOACCOUNT = drEstimateinfoLock.CONTRACT_APPROVAL_REQUESTSTAFF
                End If
            Next

            '見積情報更新
            result = SC3070207TableAdapter.UpdateEstimateInfo(drParameter.ESTIMATEID, _
                                                              SC3070207TableAdapter.StatusDenial, _
                                                              drParameter.ACCOUNT)
            If result = 0 Then
                Me.Rollback = True
                Me._msgId = MsgId903
                '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 START
                Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name))
                '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 END
                Return False
            End If

            '2015/03/13 TCS 鈴木【TMT課題対応(#72 セールスタブレットの価格相談履歴表示)】ADD START
            'マネージャ回答登録
            Dim chkFlg As Boolean = SC3070207TableAdapter.RegistAnswer(drParameter.ESTIMATEID, _
                                                                       GetContractApprovalSequence(drParameter.ESTIMATEID), _
                                                                       drParameter.ACCOUNT, _
                                                                       drParameter.DISPLAYCONTENTS,
                                                                       SC3070207TableAdapter.StatusDenial)

            '更新失敗の場合、ロールバックし処理を終了する
            If chkFlg = False Then
                Me.Rollback = True
                '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 START
                Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name))
                '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 END
                Return False
            End If
            '2015/03/13 TCS 鈴木【TMT課題対応(#72 セールスタブレットの価格相談履歴表示)】ADD END

            '承認依頼スタッフが自身の場合、通知を行わない
            If drParameter.NOTICEREQID <> 0 Then
                Dim returnXmlNotice As XmlCommon

                '通知用データ取得
                drParameter = GetNoticeRequest(drParameter)

                '否認結果通知処理
                returnXmlNotice = NoticeRequest(drParameter, RequestTypeEnum.Denial)

                If ResultIdSuccess.Equals(returnXmlNotice.ResultId) = False Then
                    '通知が失敗した場合
                    Me.Rollback = True
                    Me._msgId = MsgId903
                    Logger.Error(ERROR_MSG2 & returnXmlNotice.ResultId)
                    '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 START
                    Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name))
                    '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 END
                    Return False
                End If
            End If

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name), True)

            '正常終了
            Return True

        Catch ex As Exception

            Toyota.eCRB.SystemFrameworks.Core.Logger.Info(ex.Message)
            Toyota.eCRB.SystemFrameworks.Core.Logger.Error("Error Log", ex)

            Me.Rollback = True
            Me._msgId = MsgId903
            '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 START
            Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name))
            '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 END

            Return False

        End Try
    End Function

    '2015/03/16 TCS 鈴木【TMT課題対応(#72 セールスタブレットの価格相談履歴表示)】ADD START
    ''' <summary>
    ''' 注文承認情報の依頼連番を取得
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <returns>注文承認情報の依頼連番</returns>
    ''' <remarks></remarks>
    Public Function GetContractApprovalSequence(ByVal estimateId As Long) As Decimal
        Dim dtEstimateinfoLock As SC3070207DataSet.SC3070207EstContractApprovalDataTable = SC3070207TableAdapter.GetContractApprovalSequence(estimateId)
        Return dtEstimateinfoLock(0).SEQNO
    End Function
    '2015/03/16 TCS 鈴木【TMT課題対応(#72 セールスタブレットの価格相談履歴表示)】ADD END
#End Region

#Region "Private"
    ''' <summary>
    ''' A-Card番号を取得する
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <returns>A-Card番号</returns>
    ''' <remarks></remarks>
    Private Function GetACardNum(ByVal salesId As Decimal) As String
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_Start", GetCurrentMethod.Name), True)

        Dim aCardNum As String = String.Empty

        '商談情報よりA-Card番号取得
        Me.SalesTempFlg = SC3070207TableAdapter.SalesTemp.Sales
        Dim dtSales As SC3070207DataSet.SC3070207SalesDataTable = _
            SC3070207TableAdapter.GetSales(salesId, Me.SalesTempFlg)

        If dtSales.Rows.Count = 0 Then
            '商談一時情報よりA-Card番号取得
            Me.SalesTempFlg = SC3070207TableAdapter.SalesTemp.SalesHis
            dtSales = SC3070207TableAdapter.GetSales(salesId, Me.SalesTempFlg)
        End If

        If dtSales.Rows.Count = 0 Then
            '商談一時情報よりA-Card番号取得
            Me.SalesTempFlg = SC3070207TableAdapter.SalesTemp.SalesTemp
            dtSales = SC3070207TableAdapter.GetSales(salesId, Me.SalesTempFlg)
        End If

        If dtSales.Rows.Count > 0 Then
            Dim drSales As SC3070207DataSet.SC3070207SalesRow
            drSales = CType(dtSales.Rows(0), SC3070207DataSet.SC3070207SalesRow)
            If drSales.IsACARD_NUMNull = False Then
                aCardNum = drSales.ACARD_NUM.Trim()
            End If
        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name), True)
        Return aCardNum
    End Function


    ''' <summary>
    ''' システム設定を取得する
    ''' </summary>
    ''' <param name="key">キー</param>
    ''' <returns>設定値</returns>
    ''' <remarks></remarks>
    Private Function GetSystemEnvSetting(ByVal key As String) As String
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_Start", GetCurrentMethod.Name), True)

        Dim value As String = String.Empty

        Dim sysEnv As New SystemEnvSetting
        Dim sysEnvRow As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow = _
            sysEnv.GetSystemEnvSetting(key)

        If sysEnvRow Is Nothing = False Then
            value = sysEnvRow.PARAMVALUE
        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name), True)
        Return value
    End Function

    ''' <summary>
    ''' 通知登録IF用データ取得
    ''' </summary>
    ''' <param name="parameter">パラメータDataRow</param>
    ''' <returns>パラメータDataRow</returns>
    ''' <remarks></remarks>
    Private Function GetNoticeRequest(ByVal parameter As SC3070207DataSet.SC3070207ParameterRow) As SC3070207DataSet.SC3070207ParameterRow
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_Start", GetCurrentMethod.Name), True)

        '初期値設定
        parameter.TOACCOUNTNAME = String.Empty
        parameter.CST_ID = String.Empty
        parameter.CST_NAME = String.Empty
        parameter.CST_TYPE = String.Empty
        parameter.CST_VCL_TYPE = String.Empty
        parameter.SLS_PIC_STF_CD = String.Empty
        parameter.VehicleSequenceNumber = String.Empty
        parameter.FLLWUPBOXSTRCD = String.Empty
        parameter.FLLWUPBOX_SEQNO = 0

        '通知先スタッフ名取得
        Dim dtUsersInfo As SC3070207DataSet.SC3070207UsersInfoDataTable = _
            SC3070207TableAdapter.GetUsersInfo(parameter.TOACCOUNT)
        '通知先スタッフ名保持
        For Each drUsersInfo In dtUsersInfo
            If drUsersInfo.IsUSERNAMENull = False Then
                parameter.TOACCOUNTNAME = drUsersInfo.USERNAME
            End If
        Next

        '通知依頼情報取得
        Dim dtNoticeRequest As SC3070207DataSet.SC3070207NoticeRequestInfoDataTable = _
            SC3070207TableAdapter.GetNoticeRequestInfo(parameter.NOTICEREQID)
        '通知依頼情報保持
        For Each drNoticeRequest In dtNoticeRequest
            If drNoticeRequest.IsCRCUSTIDNull = False Then
                parameter.CST_ID = drNoticeRequest.CRCUSTID
            End If
            If drNoticeRequest.IsCUSTOMNAMENull = False Then
                parameter.CST_NAME = drNoticeRequest.CUSTOMNAME
            End If
            If drNoticeRequest.IsCSTKINDNull = False Then
                parameter.CST_TYPE = drNoticeRequest.CSTKIND
            End If
            If drNoticeRequest.IsCUSTOMERCLASSNull = False Then
                parameter.CST_VCL_TYPE = drNoticeRequest.CUSTOMERCLASS
            End If
            If drNoticeRequest.IsSALESSTAFFCDNull = False Then
                parameter.SLS_PIC_STF_CD = drNoticeRequest.SALESSTAFFCD
            End If
            If drNoticeRequest.IsVCLIDNull = False Then
                parameter.VehicleSequenceNumber = drNoticeRequest.VCLID
            End If
            If drNoticeRequest.IsFLLWUPBOXSTRCDNull = False Then
                parameter.FLLWUPBOXSTRCD = drNoticeRequest.FLLWUPBOXSTRCD
            End If
            If drNoticeRequest.IsFLLWUPBOXNull = False Then
                parameter.FLLWUPBOX_SEQNO = drNoticeRequest.FLLWUPBOX
            End If
        Next

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name), True)
        Return parameter
    End Function

    ''' <summary>
    ''' 通知登録IF呼び出し
    ''' </summary>
    ''' <param name="parameter">パラメータDataRow</param>
    ''' <param name="stats">承認or否認</param>
    ''' <returns>XmlCommon</returns>
    ''' <remarks></remarks>
    Private Function NoticeRequest(ByVal parameter As SC3070207DataSet.SC3070207ParameterRow, _
                                   ByVal stats As RequestTypeEnum) As XmlCommon
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_Start", GetCurrentMethod.Name), True)

        Dim noticeData As XmlNoticeData = Nothing
        Dim account As XmlAccount = Nothing
        Dim requestNotice As XmlRequestNotice = Nothing
        Dim pushInfo As XmlPushInfo = Nothing
        Try
            noticeData = New XmlNoticeData
            'headにデータを格納
            noticeData.TransmissionDate = DateTimeFunc.Now(parameter.DLR_CD)

            '相談先情報をセット（セールスマネージャー）
            account = New XmlAccount
            account.ToAccount = parameter.TOACCOUNT
            account.ToAccountName = parameter.TOACCOUNTNAME

            '相談者情報（スタッフ情報）をセット
            requestNotice = New XmlRequestNotice
            requestNotice.DealerCode = parameter.DLR_CD
            requestNotice.StoreCode = parameter.BRN_CD
            requestNotice.RequestClass = NOTICE_IF_ORDER_APPROVAL
            If stats = RequestTypeEnum.Approval Then
                requestNotice.Status = NOTICE_IF_APPROVAL
            Else
                requestNotice.Status = NOTICE_IF_DENIAL
            End If
            requestNotice.RequestId = parameter.NOTICEREQID
            requestNotice.RequestClassId = parameter.ESTIMATEID
            requestNotice.FromAccount = parameter.ACCOUNT
            requestNotice.FromAccountName = parameter.ACCOUNTNAME
            requestNotice.CustomId = parameter.CST_ID
            requestNotice.CustomName = parameter.CST_NAME
            requestNotice.CustomerClass = parameter.CST_VCL_TYPE
            requestNotice.CustomerKind = parameter.CST_TYPE
            requestNotice.SalesStaffCode = parameter.SLS_PIC_STF_CD
            requestNotice.VehicleSequenceNumber = parameter.VehicleSequenceNumber
            requestNotice.FollowUpBoxStoreCode = parameter.FLLWUPBOXSTRCD
            If parameter.IsFLLWUPBOX_SEQNONull = False Then
                requestNotice.FollowUpBoxNumber = parameter.FLLWUPBOX_SEQNO
            End If

            '通知方法をセット
            pushInfo = New XmlPushInfo
            pushInfo.PushCategory = NOTICE_IF_PUSHCATEGORY_POPUP
            pushInfo.PositionType = NOTICE_IF_POSITION_HEADER
            pushInfo.Time = NOTICE_IF_TIME
            pushInfo.DisplayType = NOTICE_IF_DISPLAY_TYPE_TEXT
            If String.IsNullOrWhiteSpace(parameter.DISPLAYCONTENTS) Then
                If stats = RequestTypeEnum.Approval Then
                    pushInfo.DisplayContents = WebWordUtility.GetWord("SC3070201", MsgId906)
                Else
                    pushInfo.DisplayContents = WebWordUtility.GetWord("SC3070201", MsgId907)
                End If
            Else
                pushInfo.DisplayContents = parameter.DISPLAYCONTENTS
            End If
            pushInfo.Color = NOTICE_IF_COLOR
            pushInfo.DisplayFunction = NOTICE_IF_DISPLAY_FUNCTION
            pushInfo.ActionFunction = NOTICE_IF_ACTION_FUNCTION

            '格納したデータを親クラスに格納
            noticeData.AccountList.Add(account)
            noticeData.RequestNotice = requestNotice
            noticeData.PushInfo = pushInfo

            'ロジックを呼ぶ
            Using noticeRequestIF As New IC3040801BusinessLogic

                'i-CROPへ送信
                Dim response As XmlCommon = noticeRequestIF.NoticeDisplay(noticeData, ConstCode.NoticeDisposal.Peculiar)

                Return response
            End Using
        Finally
            If pushInfo IsNot Nothing Then
                pushInfo.Dispose()
            End If

            If requestNotice IsNot Nothing Then
                requestNotice.Dispose()
            End If

            If account IsNot Nothing Then
                account.Dispose()
            End If

            If noticeData IsNot Nothing Then
                noticeData.Dispose()
            End If

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name), True)

        End Try
    End Function
#End Region

End Class
