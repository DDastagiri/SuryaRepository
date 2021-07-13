'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3070204.ascx.vb
'─────────────────────────────────────
'機能： 見積書・契約書印刷
'補足： 
'作成： 2012/11/25 TCS 坪根
'更新： 2013/03/08 TCS 山田 【A.STEP2】新車タブレット受付画面の管理指標変更対応
'更新： 2013/07/11 TCS 坪根  GL0895対応
'更新： 2013/07/11 TCS 河原  Aカード情報相互連携開発
'更新： 2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展
'─────────────────────────────────────

Imports System.Text
Imports System.Globalization

Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.Estimate.Quotation.BizLogic
Imports Toyota.eCRB.Estimate.Quotation.DataAccess

Partial Class Pages_SC3070204
    Inherits System.Web.UI.UserControl
    Implements ICallbackEventHandler

#Region "定数"

    ''' <summary>
    ''' メッセージID (ID:901)
    ''' </summary>
    ''' <remarks>
    ''' タブレット基盤との連携に失敗しました。
    ''' </remarks>
    Private Const MESSAGE_ID_901 As Integer = 901

    ''' <summary>
    ''' メッセージID (ID:902)
    ''' </summary>
    ''' <remarks>
    ''' 見積書の印刷に失敗しました。
    ''' </remarks>
    Private Const MESSAGE_ID_902 As Integer = 902

    ''' <summary>
    ''' メッセージID (ID:903)
    ''' </summary>
    ''' <remarks>
    ''' 契約書の印刷に失敗しました。
    ''' </remarks>
    Private Const MESSAGE_ID_903 As Integer = 903

    ''' <summary>
    ''' メッセージID (ID:904)
    ''' </summary>
    ''' <remarks>
    ''' 受注Ｎｏを発行しますが、よろしいですか。
    ''' </remarks>
    Private Const MESSAGE_ID_904 As Integer = 904

    ''' <summary>
    ''' メッセージID (ID:905)
    ''' </summary>
    ''' <remarks>
    ''' 受注Ｎｏをキャンセルしますが、よろしいですか。＊ＴＡＣＴでのキャンセルが必要です。
    ''' </remarks>
    Private Const MESSAGE_ID_905 As Integer = 905

    '2013/03/08 TCS 山田 【A.STEP2】新車タブレット受付画面の管理指標変更対応 START
    ''' <summary>
    ''' メッセージID (ID:906)
    ''' </summary>
    ''' <remarks>
    ''' 注文書の印刷に失敗しました。
    ''' </remarks>
    Private Const MESSAGE_ID_906 As Integer = 906
    '2013/03/08 TCS 山田 【A.STEP2】新車タブレット受付画面の管理指標変更対応 END

    ''' <summary>
    ''' 印刷モード　見積書
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PRINT_MODE_ESTIMATION As String = "1"

    '2013/03/08 TCS 山田 【A.STEP2】新車タブレット受付画面の管理指標変更対応 START
    ''' <summary>
    ''' 印刷モード　注文書
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PRINT_MODE_ORDER As String = "3"
    '2013/03/08 TCS 山田 【A.STEP2】新車タブレット受付画面の管理指標変更対応 END

    ''' <summary>
    ''' 印刷モード　契約書
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PRINT_MODE_CONTRACT As String = "2"

    ''' <summary>
    ''' テーブル名　印刷情報(基本)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TBL_PRINTINFO_BASIC = "SC3070204PrintInfoBasic"

    ''' <summary>
    ''' 列名　契約書印刷フラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CLM_CONTRACT_PRINTFLG = "CONTPRINTFLG"

    ''' <summary>
    ''' 列名　契約状況フラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CLM_CONTRACT_STATUSFLG = "CONTRACTFLG"

    ''' <summary>
    ''' 列名　FollowupBox 連番
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CLM_FOLLOWUPBOX_SEQNO = "FLLWUPBOX_SEQNO"

#End Region

#Region "コールバック"

    Private callBackArgument As CallBackArgumentClass
    Private _callbackResult As String

#Region "列挙値"

    Private Enum ResultCode
        Success = 0
        Failure = -999
    End Enum

#End Region

#Region "コールバック(Client → Server)"

    ''' <summary>
    ''' コールバック用文字列を返す
    ''' </summary>
    ''' <remarks></remarks>
    Public Function GetCallbackResult() As String Implements System.Web.UI.ICallbackEventHandler.GetCallbackResult

        Return _callbackResult

    End Function

    ''' <summary>
    ''' コールバック処理
    ''' </summary>
    ''' <param name="eventArgument">コールバックの引数</param>
    ''' <remarks>JavaScriptから呼ばれる</remarks>
    Public Sub RaiseCallbackEvent(ByVal eventArgument As String) Implements System.Web.UI.ICallbackEventHandler.RaiseCallbackEvent
        Dim callBackResult As New CallBackResultClass                               'コールバック返り値
        Dim serializer = New System.Web.Script.Serialization.JavaScriptSerializer

        Try
            callBackArgument = serializer.Deserialize(Of CallBackArgumentClass)(eventArgument)

            'コールバック要求を出したメソッド名を判別
            Select Case callBackArgument.Method
                Case "Initialize"       '初期表示
                    '印刷XML作成処理
                    callBackResult = Me.CreatePrintXML(callBackArgument)
                Case "UpdateEstimatePrintDate"          '見積印刷日更新
                    '見積印刷日更新
                    callBackResult = Me.UpdateEstimatePrintDate(callBackArgument)

                    '2013/07/11 TCS 坪根 GL0895対応 START
                    '正常メッセージを出力
                    Me.WriteErrorLog(callBackArgument)
                    '2013/07/11 TCS 坪根 GL0895対応 END
                    '2013/03/08 TCS 山田 【A.STEP2】新車タブレット受付画面の管理指標変更対応 START
                Case "UpdateContractPrintFlg", "OrderUpdateContractPrintFlg"   '契約書印刷フラグ更新
                    '2013/03/08 TCS 山田 【A.STEP2】新車タブレット受付画面の管理指標変更対応 END
                    '契約書印刷フラグ更新
                    callBackResult = Me.UpdateContractPrintFlg(callBackArgument)
                Case "DecideContractInfo"               '契約情報更新(確定時)
                    '契約情報更新(確定時)
                    callBackResult = Me.UpdateContractInfoByDecide(callBackArgument)
                Case "CancelContractInfo"               '契約情報更新(キャンセル時)
                    '契約情報更新(キャンセル時)
                    callBackResult = Me.UpdateContractInfoByCancel(callBackArgument)
                    '2013/07/11 TCS 坪根 GL0895対応 START
                Case "SuccessContractPrint", "SuccessOrderPrint"                '正常処理
                    '正常メッセージを出力
                    Me.WriteErrorLog(callBackArgument)
                    '2013/07/11 TCS 坪根 GL0895対応 END
                Case "ErrorProc"                        'エラー処理 ※クライアント側で発生時
                    'エラー処理
                    Me.WriteErrorLog(callBackArgument)

                    'クライアントでエラー表示する為、再度エラー情報設定
                    callBackResult.ResultCode = callBackArgument.ShowDialogErrorId
                    callBackResult.Message = callBackArgument.ShowDialogErrorMessage
            End Select
        Catch ex As OracleExceptionEx
            callBackResult.ResultCode = ResultCode.Failure
            callBackResult.Message = Me.GetHtmlEncodeValue(ex.Message)

            Logger.Error("エラー(OracleExceptionEx):" & ex.ToString)
        Catch ex As Exception
            callBackResult.ResultCode = ResultCode.Failure
            callBackResult.Message = Me.GetHtmlEncodeValue(ex.Message)

            Logger.Error("エラー(Exception):" & ex.ToString)
        End Try

        'JavaScript側で呼び出したメソッド名を設定
        callBackResult.Caller = callBackArgument.Method

        _callbackResult = serializer.Serialize(callBackResult)

    End Sub

#End Region

#Region "コールバック用内部クラス"
    Private Class CallBackArgumentClass

        '見積管理ID
        Private _estimateid As Long
        Public Property Estimateid() As Long
            Get
                Return Me._estimateid
            End Get
            Set(ByVal value As Long)
                Me._estimateid = value
            End Set
        End Property

        '支払区分
        Private _PaymentKbn As String
        Public Property PaymentKbn() As String
            Get
                Return Me._PaymentKbn
            End Get
            Set(ByVal value As String)
                Me._PaymentKbn = value
            End Set
        End Property

        'コールバックメソッド名
        Private _method As String
        Public Property Method() As String
            Get
                Return Me._method
            End Get
            Set(ByVal value As String)
                Me._method = value
            End Set
        End Property

        'エラーダイアログに表示するエラーID
        Private _showDialogErrorId As Integer
        Public Property ShowDialogErrorId() As Integer
            Get
                Return Me._showDialogErrorId
            End Get
            Set(ByVal value As Integer)
                Me._showDialogErrorId = value
            End Set
        End Property

        'エラーダイアログに表示するエラーメッセージ
        Private _showDialogErrorMessage As String
        Public Property ShowDialogErrorMessage() As String
            Get
                Return Me._showDialogErrorMessage
            End Get
            Set(ByVal value As String)
                Me._showDialogErrorMessage = value
            End Set
        End Property

        'エラーログに出力する内容
        Private _errorLogValue As String
        Public Property ErrorLogValue() As String
            Get
                Return Me._errorLogValue
            End Get
            Set(ByVal value As String)
                Me._errorLogValue = value
            End Set
        End Property

    End Class

    Private Class CallBackResultClass
        '呼び出し元メソッド(JavaScript側)
        Private _caller As String
        Public Property Caller() As String
            Get
                Return _caller
            End Get
            Set(ByVal value As String)
                _caller = value
            End Set
        End Property

        '戻り値
        Private _resultCode As Long
        Public Property ResultCode() As Long
            Get
                Return _resultCode
            End Get
            Set(ByVal value As Long)
                _resultCode = value
            End Set
        End Property

        'メッセージ
        Private _message As String
        Public Property Message() As String
            Get
                Return _message
            End Get
            Set(ByVal value As String)
                _message = value
            End Set
        End Property

        '見積書の印刷情報
        Private _printEstimateInfo As String
        Public Property PrintEstimateInfo() As String
            Get
                Return _printEstimateInfo
            End Get
            Set(ByVal value As String)
                _printEstimateInfo = value
            End Set
        End Property

        '2013/03/08 TCS 山田 【A.STEP2】新車タブレット受付画面の管理指標変更対応 START
        '注文書の印刷情報
        Private _PrintOrderInfo As String
        Public Property PrintOrderInfo() As String
            Get
                Return _PrintOrderInfo
            End Get
            Set(ByVal value As String)
                _PrintOrderInfo = value
            End Set
        End Property
        '2013/03/08 TCS 山田 【A.STEP2】新車タブレット受付画面の管理指標変更対応 END

        '契約書の印刷情報
        Private _printContractInfo As String
        Public Property PrintContractInfo() As String
            Get
                Return _printContractInfo
            End Get
            Set(ByVal value As String)
                _printContractInfo = value
            End Set
        End Property

        '契約書印刷フラグ
        Private _contractPrintFlg As String
        Public Property ContractPrintFlg() As String
            Get
                Return _contractPrintFlg
            End Get
            Set(ByVal value As String)
                _contractPrintFlg = value
            End Set
        End Property

        '契約状況フラグ
        Private _contractStatusFlg As String
        Public Property ContractStatusFlg() As String
            Get
                Return _contractStatusFlg
            End Get
            Set(ByVal value As String)
                _contractStatusFlg = value
            End Set
        End Property

        'Follow-up Box内連番
        Private _fllwupBoxSeqNo As String
        Public Property FllwupBoxSeqNo() As String
            Get
                Return _fllwupBoxSeqNo
            End Get
            Set(ByVal value As String)
                _fllwupBoxSeqNo = value
            End Set
        End Property
    End Class

#End Region

#End Region

#Region "イベント処理"

    ''' <summary>
    ''' ロードの処理を実施します。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        'コールバック作成
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), _
        "Callback", _
        String.Format(CultureInfo.InvariantCulture, _
          "sc3070204Script.callBack.beginCallback = function () {{ {0}; }};", _
          Page.ClientScript.GetCallbackEventReference(Me, _
         "sc3070204Script.callBack.packedArgument", _
         "sc3070204Script.callBack.endCallback", _
         "", _
         False)), _
          True)

        'メッセージ設定
        Me.SetMessage()
    End Sub

#End Region

#Region "Privateメソッド"

    ''' <summary>
    ''' 印刷XML作成処理
    ''' </summary>
    ''' <param name="callBackArgument">コールバックした引数</param>
    ''' <returns>コールバックする値</returns>
    ''' <remarks></remarks>
    Private Function CreatePrintXML(ByVal callBackArgument As CallBackArgumentClass) As CallBackResultClass

        Dim callBackResult As New CallBackResultClass                   'コールバック返り値
        Dim sc3070204Biz As New SC3070204BusinessLogic
        Dim sc3070204DocDt As SC3070204DataSet
        Dim msgId As Integer = ResultCode.Success

        '印刷情報取得
        sc3070204DocDt = sc3070204Biz.GetDataPrintInfo(callBackArgument.Estimateid, _
                                                       callBackArgument.PaymentKbn)
        '処理結果取得
        msgId = sc3070204Biz.MsgId
        If msgId = ResultCode.Success Then
            '印刷情報(基本)
            Dim printDataBasicRow As SC3070204DataSet.SC3070204PrintInfoBasicRow = Nothing
            printDataBasicRow = CType(sc3070204DocDt.Tables(TBL_PRINTINFO_BASIC).Rows(0),  _
                                      SC3070204DataSet.SC3070204PrintInfoBasicRow)

            '契約書印刷フラグを取得
            callBackResult.ContractPrintFlg = printDataBasicRow.CONTRACT_PRINTFLG
            '契約状況フラグを取得
            callBackResult.ContractStatusFlg = printDataBasicRow.CONTRACT_STATUS_FLG
            'Follow-up Box内連番を取得
            callBackResult.FllwupBoxSeqNo = printDataBasicRow.FLLWUPBOX_SEQNO

            printDataBasicRow = Nothing

            '見積書印刷情報のXml作成
            Dim strPrintEstimateInfo As String = sc3070204Biz.GetXmlPrintInfo(sc3070204DocDt, _
                                                                         PRINT_MODE_ESTIMATION)
            callBackResult.PrintEstimateInfo = strPrintEstimateInfo

            '2013/03/08 TCS 山田 【A.STEP2】新車タブレット受付画面の管理指標変更対応 START
            '注文書印刷情報のXml作成
            Dim strPrintOrderInfo As String = sc3070204Biz.GetXmlPrintInfo(sc3070204DocDt, _
                                                                         PRINT_MODE_ORDER)
            callBackResult.PrintOrderInfo = strPrintOrderInfo
            '2013/03/08 TCS 山田 【A.STEP2】新車タブレット受付画面の管理指標変更対応 END

            '契約書印刷情報のXml作成
            Dim strPrintContractInfo As String = sc3070204Biz.GetXmlPrintInfo(sc3070204DocDt, _
                                                                              PRINT_MODE_CONTRACT)
            callBackResult.PrintContractInfo = strPrintContractInfo

            callBackResult.ResultCode = ResultCode.Success
            callBackResult.Message = String.Empty
        Else
            callBackResult.ResultCode = msgId
            callBackResult.Message = Me.GetHtmlEncodeValue(WebWordUtility.GetWord("SC3070204", msgId))
            callBackResult.PrintEstimateInfo = String.Empty
            '2013/03/08 TCS 山田 【A.STEP2】新車タブレット受付画面の管理指標変更対応 START
            callBackResult.PrintOrderInfo = String.Empty
            '2013/03/08 TCS 山田 【A.STEP2】新車タブレット受付画面の管理指標変更対応 END
            callBackResult.PrintContractInfo = String.Empty
            callBackResult.ContractPrintFlg = String.Empty
            callBackResult.ContractStatusFlg = String.Empty
            callBackResult.FllwupBoxSeqNo = String.Empty
        End If
        sc3070204Biz = Nothing
        sc3070204DocDt = Nothing

        Return callBackResult

    End Function

    ''' <summary>
    ''' 見積印刷日更新
    ''' </summary>
    ''' <param name="callBackArgument">コールバックした引数</param>
    ''' <returns>コールバックする値</returns>
    ''' <remarks></remarks>
    Private Function UpdateEstimatePrintDate(ByVal callBackArgument As CallBackArgumentClass) As CallBackResultClass

        Dim callBackResult As New CallBackResultClass                   'コールバック返り値
        Dim sc3070204Biz As New SC3070204BusinessLogic
        Dim blnRes As Boolean = True        '処理結果

        '見積印刷日更新
        blnRes = sc3070204Biz.UpdateEstimatePrintDate(callBackArgument.Estimateid)
        If blnRes Then
            callBackResult.ResultCode = ResultCode.Success
            callBackResult.Message = String.Empty
        Else
            Dim msgId As Integer = sc3070204Biz.MsgId
            callBackResult.ResultCode = msgId
            callBackResult.Message = Me.GetHtmlEncodeValue(WebWordUtility.GetWord("SC3070204", msgId))
        End If
        sc3070204Biz = Nothing

        Return callBackResult

    End Function

    ''' <summary>
    ''' 契約書印刷フラグ更新
    ''' </summary>
    ''' <param name="callBackArgument">コールバックした引数</param>
    ''' <returns>コールバックする値</returns>
    ''' <remarks></remarks>
    Private Function UpdateContractPrintFlg(ByVal callBackArgument As CallBackArgumentClass) As CallBackResultClass

        Dim callBackResult As New CallBackResultClass                   'コールバック返り値
        Dim sc3070204Biz As New SC3070204BusinessLogic
        Dim blnRes As Boolean = True        '処理結果

        '2013/03/08 TCS 山田 【A.STEP2】新車タブレット受付画面の管理指標変更対応 START
        '契約書印刷フラグ更新
        blnRes = sc3070204Biz.UpdateContractPrintFlg(callBackArgument.Estimateid, callBackArgument.Method)
        '2013/03/08 TCS 山田 【A.STEP2】新車タブレット受付画面の管理指標変更対応 END
        If blnRes Then
            callBackResult.ResultCode = ResultCode.Success
            callBackResult.Message = String.Empty
        Else
            Dim msgId As Integer = sc3070204Biz.MsgId
            callBackResult.ResultCode = msgId
            callBackResult.Message = Me.GetHtmlEncodeValue(WebWordUtility.GetWord("SC3070204", msgId))
        End If
        sc3070204Biz = Nothing

        Return callBackResult

    End Function

    ''' <summary>
    ''' 契約情報更新(確定時)
    ''' </summary>
    ''' <param name="callBackArgument">コールバックした引数</param>
    ''' <returns>コールバックする値</returns>
    ''' <remarks></remarks>
    Private Function UpdateContractInfoByDecide(ByVal callBackArgument As CallBackArgumentClass) As CallBackResultClass

        Dim callBackResult As New CallBackResultClass                   'コールバック返り値
        Dim sc3070204Biz As New SC3070204BusinessLogic
        Dim blnRes As Boolean = True        '処理結果

        '契約情報更新(確定時)
        blnRes = sc3070204Biz.UpdateContractInfoByDecide(callBackArgument.Estimateid, _
                                                         callBackArgument.PaymentKbn)
        If blnRes Then
            callBackResult.ResultCode = ResultCode.Success
            callBackResult.Message = String.Empty
        Else
            Dim msgId As Integer = sc3070204Biz.MsgId
            callBackResult.ResultCode = msgId
            callBackResult.Message = Me.GetHtmlEncodeValue(WebWordUtility.GetWord("SC3070204", msgId))
        End If
        sc3070204Biz = Nothing

        Return callBackResult

    End Function

    ''' <summary>
    ''' 契約情報更新(キャンセル時)
    ''' </summary>
    ''' <param name="callBackArgument">コールバックした引数</param>
    ''' <returns>コールバックする値</returns>
    ''' <remarks></remarks>
    Private Function UpdateContractInfoByCancel(ByVal callBackArgument As CallBackArgumentClass) As CallBackResultClass

        Dim callBackResult As New CallBackResultClass                   'コールバック返り値
        Dim sc3070204Biz As New SC3070204BusinessLogic
        Dim blnRes As Boolean = True        '処理結果

        '契約情報更新(キャンセル時)
        blnRes = sc3070204Biz.UpdateContractInfoByCancel(callBackArgument.Estimateid)
        If blnRes Then
            callBackResult.ResultCode = ResultCode.Success
            callBackResult.Message = String.Empty
        Else
            Dim msgId As Integer = sc3070204Biz.MsgId
            callBackResult.ResultCode = msgId
            callBackResult.Message = Me.GetHtmlEncodeValue(WebWordUtility.GetWord("SC3070204", msgId))
        End If
        sc3070204Biz = Nothing

        Return callBackResult

    End Function

    ''' <summary>
    ''' エラーログ出力
    ''' </summary>
    ''' <param name="callBackArgument">コールバックした引数</param>
    ''' <remarks></remarks>
    Private Sub WriteErrorLog(ByVal callBackArgument As CallBackArgumentClass)
        '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 START
        'エラー以外でも使用しているためInfoに格下げ
        'エラーログに出力
        Logger.Info(callBackArgument.ErrorLogValue)
        '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 END
    End Sub

    ''' <summary>
    ''' メッセージを設定
    ''' </summary>
    ''' <remarks>クライアントで使用する文言・メッセージを設定</remarks>
    Private Sub SetMessage()
        Me.SC3070204HeaderTitle.Text = WebWordUtility.GetWord("SC3070204", 1)
        Me.EstimatePrintButtonLabel.Text = WebWordUtility.GetWord("SC3070204", 2)
        '2013/03/08 TCS 山田 【A.STEP2】新車タブレット受付画面の管理指標変更対応 START
        Me.OrderPrintButtonLabel.Text = WebWordUtility.GetWord("SC3070204", 7)
        '2013/03/08 TCS 山田 【A.STEP2】新車タブレット受付画面の管理指標変更対応 END
        Me.ContractPrintButtonLabel.Text = WebWordUtility.GetWord("SC3070204", 3)
        Me.SC3070204PopUpCancelButtonLabel.Text = WebWordUtility.GetWord("SC3070204", 6)

        Me.HdnMessage901.Value = Me.GetHtmlEncodeValue(WebWordUtility.GetWord("SC3070204", MESSAGE_ID_901))
        Me.HdnMessage902.Value = Me.GetHtmlEncodeValue(WebWordUtility.GetWord("SC3070204", MESSAGE_ID_902))
        Me.HdnMessage903.Value = Me.GetHtmlEncodeValue(WebWordUtility.GetWord("SC3070204", MESSAGE_ID_903))
        Me.HdnMessage904.Value = Me.GetHtmlEncodeValue(WebWordUtility.GetWord("SC3070204", MESSAGE_ID_904))
        Me.HdnMessage905.Value = Me.GetHtmlEncodeValue(WebWordUtility.GetWord("SC3070204", MESSAGE_ID_905))
        '2013/03/08 TCS 山田 【A.STEP2】新車タブレット受付画面の管理指標変更対応 START
        Me.HdnMessage906.Value = Me.GetHtmlEncodeValue(WebWordUtility.GetWord("SC3070204", MESSAGE_ID_906))
        '2013/03/08 TCS 山田 【A.STEP2】新車タブレット受付画面の管理指標変更対応 END
    End Sub

    ''' <summary>
    ''' HTMLエンコードをかけて返す
    ''' </summary>
    ''' <param name="strValue">HTMLエンコードする値</param>
    ''' <returns>HTMLエンコードした値</returns>
    ''' <remarks></remarks>
    Private Function GetHtmlEncodeValue(ByVal strValue As String) As String
        Dim returnValue As String = String.Empty

        'HTMLエンコードをかける
        returnValue = HttpUtility.JavaScriptStringEncode(strValue)

        Return returnValue
    End Function

#End Region

End Class
