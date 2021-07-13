'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3070208.ascx.vb
'─────────────────────────────────────
'機能： 注文承認依頼
'補足： 
'作成： 2013/11/29 TCS 山口  Aカード情報相互連携開発
'更新： 2014/05/28 TCS 安田 受注時説明機能開発（受注後工程スケジュール）
'更新： 2015/03/17 TCS 鈴木  次世代e-CRB 価格相談履歴参照機能開発
'─────────────────────────────────────

Option Explicit On

Imports System.Globalization
Imports System.Reflection.MethodBase
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.Estimate.Quotation.BizLogic
Imports Toyota.eCRB.Estimate.Quotation.DataAccess.SC3070208DataSet
Imports Toyota.eCRB.iCROP.BizLogic.SC3070201
Imports Toyota.eCRB.Estimate.Quotation.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Web.Controls


Partial Class Pages_SC3070208
    Inherits System.Web.UI.UserControl
    Implements ICallbackEventHandler

#Region "メンバ変数"
    '見積作成画面からの引継ぎ情報
    Private callBackArgument As CallBackArgumentClass
#End Region

#Region "プロパティ"
    Public Property TriggerClientId() As String
        Get
            Return Me.SC3070208_PopOverForm.Attributes("data-TriggerClientID")
        End Get
        Set(ByVal value As String)
            Me.SC3070208_PopOverForm.Attributes("data-TriggerClientID") = value
        End Set
    End Property
#End Region

#Region "定数／列挙値"

    ''' <summary>
    ''' 注文承認を表示中フラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SeatchKeyApprovalActive As String = "ApprovalActive"

    ''' <summary>
    ''' ログインステータス:スタンバイ
    ''' </summary>
    ''' <remarks></remarks>
    Private STANDBY As String = "1"

    ''' <summary>
    ''' ログインステータス:商談中
    ''' </summary>
    ''' <remarks></remarks>
    Private NEGOTIATION As String = "2"

    ''' <summary>
    ''' ログインステータス:退席中
    ''' </summary>
    ''' <remarks></remarks>
    Private LEAVING As String = "3"

    ''' <summary>
    ''' ログインステータス:オフライン
    ''' </summary>
    ''' <remarks></remarks>
    Private OFFLINE As String = "4"

    ''' <summary>
    ''' 処理結果コード
    ''' </summary>
    ''' <remarks></remarks>
    Private Enum ResultCode
        Success = 0     '成功：メッセージ表示なし
        Info = 100      '成功：メッセージ表示あり
        Failure = -999  '失敗：メッセージ表示あり
        DBTimeOut = -9  '失敗：メッセージ表示あり
        CheckError = -1 '失敗：メッセージ表示あり
    End Enum

#End Region

#Region "コールバック"
    Private _callbackResult As String

    ''' <summary>
    ''' コールバック用文字列を返す
    ''' </summary>
    ''' <remarks></remarks>
    Public Function GetCallbackResult() As String Implements System.Web.UI.ICallbackEventHandler.GetCallbackResult
        Return _callbackResult
    End Function

    Public Sub RaiseCallbackEvent(ByVal eventArgument As String) Implements System.Web.UI.ICallbackEventHandler.RaiseCallbackEvent
        Dim resultString As String = String.Empty
        Dim messageid As String = String.Empty
        Dim callBackResult As New CallBackResultClass
        Dim serializer = New System.Web.Script.Serialization.JavaScriptSerializer

        Dim params As SC3070208DataSet.SC3070208ParameterDataTable = Nothing
        Dim bizLogicSC3070208 As New SC3070208BusinessLogic()
        Try
            callBackArgument = New CallBackArgumentClass
            callBackArgument = serializer.Deserialize(Of CallBackArgumentClass)(eventArgument)
            callBackResult = New CallBackResultClass

            callBackResult.Caller = callBackArgument.Method
            callBackResult.ResultCode = ResultCode.Success
            callBackResult.Message = String.Empty

            Select Case callBackArgument.Method
                Case "CreateWindow"
                    '価格相談画面を作成する
                    callBackResult.Contents = CreateWindow(callBackArgument.Estimateid)

                Case "InsertInfo"
                    '依頼ボタン押下処理
                    Dim parameters As SC3070208DataSet.SC3070208ParameterDataTable = SetParameters(callBackArgument)

                    '入力チェック
                    Dim checkMessage As String = CheckApprovalInput(parameters)
                    If checkMessage <> "" Then
                        callBackResult.ResultCode = ResultCode.CheckError
                        callBackResult.Message = checkMessage
                    Else
                        If Not bizLogicSC3070208.InsertContractApproval(parameters) Then
                            callBackResult.ResultCode = ResultCode.CheckError
                        End If
                    End If

                Case "CancelInfo"
                    'キャンセルボタン押下処理
                    If Not bizLogicSC3070208.CancelContractApproval(callBackArgument.Estimateid, callBackArgument.NoticeRequestid, SetParameters(callBackArgument)) Then
                        callBackResult.ResultCode = ResultCode.CheckError
                    End If

                    '注文承認を非表示にする
                    Me.RemoveValueBypass(ScreenPos.Current, SeatchKeyApprovalActive)
            End Select

            If (callBackResult.Message = String.Empty) Then
                'メッセージ設定
                If String.IsNullOrEmpty(bizLogicSC3070208.Msg) Then
                    '置換文字列無し
                    callBackResult.Message = WebWordUtility.GetWord(SC3070208TableAdapter.ProgramId, bizLogicSC3070208.MsgId)
                Else
                    '置換文字列あり
                    callBackResult.Message = String.Format(CultureInfo.InvariantCulture, WebWordUtility.GetWord(SC3070208TableAdapter.ProgramId, bizLogicSC3070208.MsgId), bizLogicSC3070208.Msg)
                End If

            End If

            '更新： 2014/05/28 TCS 安田 受注時説明機能開発（受注後工程スケジュール） START
            If (callBackResult.ResultCode = ResultCode.Success AndAlso bizLogicSC3070208.MsgOutFlg) Then
                callBackResult.ResultCode = ResultCode.Info
                If (bizLogicSC3070208.MsgId = SC3070208BusinessLogic.MsgId942) Then
                    'MsgId942(契約条件の変更)
                    callBackResult.Info = "1"
                Else
                    'MsgId943(前提条件の変更) Or MsgId944(受注後工程未実施)
                    callBackResult.Info = ""
                End If
            End If
            '更新： 2014/05/28 TCS 安田 受注時説明機能開発（受注後工程スケジュール） END

        Catch ex As OracleExceptionEx
            If ex.ErrorCode = 111 Then
                callBackResult.ResultCode = ResultCode.DBTimeOut
                callBackResult.Message = WebWordUtility.GetWord("SC3070208", 901)
            Else
                callBackResult.ResultCode = ResultCode.Failure
                callBackResult.Message = ex.Message
                Logger.Error(ResultCode.Failure, ex)
            End If
        Catch ex As Exception
            callBackResult.ResultCode = ResultCode.Failure
            callBackResult.Message = ex.Message
            Logger.Error(ResultCode.Failure, ex)
        End Try
        Logger.Info(String.Format(CultureInfo.CurrentCulture, "Caller=[{0}] ResultCode=[{1}] Message=[{2}] Contents=[{3}]", callBackResult.Caller, callBackResult.ResultCode, callBackResult.Message, callBackResult.Contents))
        _callbackResult = serializer.Serialize(callBackResult)

    End Sub


#Region "コールバック用内部クラス"
    Private Class CallBackArgumentClass

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

        '希望値引き額
        Private _requestPrice As Nullable(Of Double)
        Public Property RequestPrice() As Nullable(Of Double)
            Get
                Return Me._requestPrice
            End Get
            Set(ByVal value As Nullable(Of Double))
                Me._requestPrice = value
            End Set
        End Property

        '2015/03/06 TCS 鈴木【TMT課題対応(#72 セールスタブレットの価格相談履歴表示)】ADD START
        'スタッフ入力メモ
        Private _requestStaffMemo As String
        Public Property RequestStaffMemo() As String
            Get
                Return Me._requestStaffMemo
            End Get
            Set(ByVal value As String)
                Me._requestStaffMemo = value
            End Set
        End Property
        '2015/03/06 TCS 鈴木【TMT課題対応(#72 セールスタブレットの価格相談履歴表示)】ADD END

        '顧客ID
        Private _customerid As String
        Public Property Customerid() As String
            Get
                Return Me._customerid
            End Get
            Set(ByVal value As String)
                Me._customerid = value
            End Set
        End Property

        '顧客名
        Private _customerName As String
        Public Property CustomerName() As String
            Get
                Return Me._customerName
            End Get
            Set(ByVal value As String)
                Me._customerName = value
            End Set
        End Property

        '顧客分類
        Private _customerClass As String
        Public Property CustomerClass() As String
            Get
                Return Me._customerClass
            End Get
            Set(ByVal value As String)
                Me._customerClass = value
            End Set
        End Property

        '顧客種別
        Private _customerKind As String
        Public Property CustomerKind() As String
            Get
                Return Me._customerKind
            End Get
            Set(ByVal value As String)
                Me._customerKind = value
            End Set
        End Property

        'フォローアップボックス店舗
        Private _followUpBoxStoreCode As String
        Public Property FollowUpBoxStoreCode() As String
            Get
                Return Me._followUpBoxStoreCode
            End Get
            Set(ByVal value As String)
                Me._followUpBoxStoreCode = value
            End Set
        End Property

        '2013/06/30 TCS 葛西 2013/10対応版　既存流用 START
        'フォローアップボックス連番
        Private _followUpBoxNumber As Nullable(Of Decimal)
        Public Property FollowUpBoxNumber() As Nullable(Of Decimal)
            Get
                Return Me._followUpBoxNumber
            End Get
            Set(ByVal value As Nullable(Of Decimal))
                Me._followUpBoxNumber = value
            End Set
        End Property
        '2013/06/30 TCS 葛西 2013/10対応版　既存流用 END

        '車両シーケンス№
        Private _vehicleSequenceNumber As String
        Public Property VehicleSequenceNumber() As String
            Get
                Return _vehicleSequenceNumber
            End Get
            Set(ByVal value As String)
                _vehicleSequenceNumber = value
            End Set
        End Property

        '顧客担当セールススタッフコード
        Private _salesStaffCode As String
        Public Property SalesStaffCode() As String
            Get
                Return _salesStaffCode
            End Get
            Set(ByVal value As String)
                _salesStaffCode = value
            End Set
        End Property

        'シリーズコード
        Private _seriesCode As String
        Public Property SeriesCode() As String
            Get
                Return _seriesCode
            End Get
            Set(ByVal value As String)
                _seriesCode = value
            End Set
        End Property

        'シリーズ名
        Private _seriesName As String
        Public Property SeriesName() As String
            Get
                Return _seriesName
            End Get
            Set(ByVal value As String)
                _seriesName = value
            End Set
        End Property

        'モデルコード
        Private _modelCode As String
        Public Property ModelCode() As String
            Get
                Return _modelCode
            End Get
            Set(ByVal value As String)
                _modelCode = value
            End Set
        End Property

        'モデル名
        Private _modelName As String
        Public Property ModelName() As String
            Get
                Return _modelName
            End Get
            Set(ByVal value As String)
                _modelName = value
            End Set
        End Property

        'マネージャーアカウント
        Private _managerAccount As String
        Public Property ManagerAccount() As String
            Get
                Return _managerAccount
            End Get
            Set(ByVal value As String)
                _managerAccount = value
            End Set
        End Property

        'マネージャー名
        Private _managerName As String
        Public Property ManagerName() As String
            Get
                Return _managerName
            End Get
            Set(ByVal value As String)
                _managerName = value
            End Set
        End Property

        '値引き理由コード
        Private _reasonsid As Nullable(Of Long)
        Public Property Reasonid() As Nullable(Of Long)
            Get
                Return _reasonsid
            End Get
            Set(ByVal value As Nullable(Of Long))
                _reasonsid = value
            End Set
        End Property

        '通知依頼ID
        Private _noticeRequestid As Nullable(Of Long)
        Public Property NoticeRequestid() As Nullable(Of Long)
            Get
                Return _noticeRequestid
            End Get
            Set(ByVal value As Nullable(Of Long))
                _noticeRequestid = value
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
                _message = HttpUtility.HtmlEncode(value)
            End Set
        End Property

        'HTMLコンテンツ
        Private _contents As String
        Public Property Contents() As String
            Get
                Return _contents
            End Get
            Set(ByVal value As String)
                _contents = HttpUtility.HtmlEncode(value)
            End Set
        End Property

        '付加情報(ResultCode = Info(100)の場合のみ使用)
        Private _info As String
        Public Property Info() As String
            Get
                Return _info
            End Get
            Set(ByVal value As String)
                _info = HttpUtility.HtmlEncode(value)
            End Set
        End Property
    End Class
#End Region

#End Region



#Region "イベント"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            'NONE
        End If
        'コールバック作成
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), _
        "Callback", _
        String.Format(CultureInfo.InvariantCulture, _
          "sc3070208Script.callBack.beginCallback = function () {{ {0}; }};", _
          Page.ClientScript.GetCallbackEventReference(Me, _
         "sc3070208Script.callBack.packedArgument", _
         "sc3070208Script.callBack.endCallback", _
         "", _
         False)), _
          True)

    End Sub

    Protected Sub SalesManagerRepeater_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles SC3070208_ApprovalStaffRepeater.ItemDataBound
        If e.Item.ItemType = ListItemType.Item _
         OrElse e.Item.ItemType = ListItemType.AlternatingItem Then

            Dim view As Data.DataView = DirectCast(e.Item.DataItem.DataView, Data.DataView)
            Dim row As SC3070208ApprovalStaffListRow = DirectCast(e.Item.DataItem.row, SC3070208ApprovalStaffListRow)
            Dim onlineStatusIconArea As HtmlGenericControl = DirectCast(e.Item.FindControl("SC3070208_OnlineStatusIconArea"), HtmlGenericControl)
            Dim salesMangerRow As HtmlGenericControl = DirectCast(e.Item.FindControl("SC3070208_ApprovalStaffRow"), HtmlGenericControl)
            Dim staffNameLabel As CustomLabel = DirectCast(e.Item.FindControl("SC3070208_ApprovalStaffNameLabel"), CustomLabel)


            salesMangerRow.Attributes("Class") = String.Empty
            AddCssClass(onlineStatusIconArea, "ncv51OnOffIcn")

            'オンライン・オフライン設定
            If StaffContext.Current.Account.Equals(row.ACCOUNT) Then
                '自分自身
                staffNameLabel.Text = WebWordUtility.GetWord(SC3070208TableAdapter.ProgramId, 3)
                AddCssClass(salesMangerRow, "Online")
            Else
                '自分自身以外
                Select Case row.PRESENCECATEGORY
                    Case STANDBY, NEGOTIATION, LEAVING
                        'スタンバイ、商談中、退席中はオンライン
                        AddCssClass(salesMangerRow, "Online")
                        AddCssClass(onlineStatusIconArea, "ncv51OnIcn")
                    Case Else
                        AddCssClass(salesMangerRow, "Offline")
                        AddCssClass(onlineStatusIconArea, "ncv51OffIcn")
                End Select
            End If

            If row.ACCOUNT.Equals(SC3070208_SelectedManagerAccount.Value) Then
                AddCssClass(salesMangerRow, "Check")
            End If

            If view.Count - 1 = e.Item.ItemIndex Then
                'リスト最終行の罫線の設定
                AddCssClass(salesMangerRow, "ListEnd")
            End If
        End If
    End Sub

#End Region

#Region "プライベートメソッド"

    Private Sub Initialize()
        '価格相談画面を初期化する
        Me.SC3070208_SelectedSalesMangerName_Display.Text = String.Empty
        Me.SC3070208_SelectedSalesMangerName.Value = String.Empty
        Me.SC3070208_SelectedManagerAccount.Value = String.Empty
        Me.SC3070208_SelectedManagerOnlineStatus.Value = String.Empty
        '2015/03/06 TCS 鈴木【TMT課題対応(#72 セールスタブレットの価格相談履歴表示)】ADD START
        Me.SC3070208_StaffMemo.Value = String.Empty
        '2015/03/06 TCS 鈴木【TMT課題対応(#72 セールスタブレットの価格相談履歴表示)】ADD END
    End Sub


    ''' <summary>
    ''' パラメータ設定
    ''' </summary>
    ''' <param name="arguments">パラメータ</param>
    ''' <returns>ParameterDataTable</returns>
    ''' <remarks></remarks>
    Private Function SetParameters(ByVal arguments As CallBackArgumentClass) As SC3070208DataSet.SC3070208ParameterDataTable
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_Start", GetCurrentMethod.Name), True)

        Using dtParameter As New SC3070208DataSet.SC3070208ParameterDataTable
            Dim drParameter As SC3070208DataSet.SC3070208ParameterRow
            drParameter = dtParameter.NewSC3070208ParameterRow
            '見積管理ID
            drParameter.ESTIMATEID = arguments.Estimateid
            '販売店コード
            drParameter.DLR_CD = StaffContext.Current.DlrCD
            '店舗コード
            drParameter.BRN_CD = StaffContext.Current.BrnCD
            'ログインアカウント
            drParameter.ACCOUNT = StaffContext.Current.Account
            'ログインアカウント名
            drParameter.ACCOUNTNAME = StaffContext.Current.UserName
            '送信先アカウント
            drParameter.TOACCOUNT = arguments.ManagerAccount
            '送信先アカウント名
            drParameter.TOACCOUNTNAME = arguments.ManagerName
            '顧客ID
            drParameter.CST_ID = arguments.Customerid
            '顧客名
            drParameter.CST_NAME = arguments.CustomerName
            '顧客種別
            drParameter.CST_TYPE = arguments.CustomerKind
            '顧客車両区分
            drParameter.CST_VCL_TYPE = arguments.CustomerClass
            'セールス担当スタッフコード
            drParameter.SLS_PIC_STF_CD = arguments.SalesStaffCode
            '車両シーケンスNo
            drParameter.VehicleSequenceNumber = arguments.VehicleSequenceNumber
            'Follow-Up Box店舗コード
            drParameter.FLLWUPBOXSTRCD = arguments.FollowUpBoxStoreCode
            'Follow-Up Box
            If arguments.FollowUpBoxNumber.HasValue Then
                drParameter.FLLWUPBOX_SEQNO = arguments.FollowUpBoxNumber.Value
            Else
                drParameter.FLLWUPBOX_SEQNO = 0
            End If
            '2015/03/17 TCS 鈴木【TMT課題対応(#72 セールスタブレットの価格相談履歴表示)】ADD START
            'スタッフ入力コメント
            drParameter.STAFFMEMO = arguments.RequestStaffMemo
            '2015/03/17 TCS 鈴木【TMT課題対応(#72 セールスタブレットの価格相談履歴表示)】ADD END

            dtParameter.Rows.Add(drParameter)

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name), True)

            Return dtParameter
        End Using
    End Function

    Private Function CreateWindow(ByVal estimateId As Long) As String
        '価格相談画面初期化
        Initialize()

        Dim bizLogic As New SC3070208BusinessLogic()

        '注文承認依頼取得
        Dim dtContractApproval As SC3070208DataSet.SC3070208ContractApprovalDataTable = bizLogic.GetContractApproval(estimateId)
        For Each drContractApproval In dtContractApproval
            'ステータスにより分岐
            If SC3070208TableAdapter.StatusApprovalRequest.Equals(drContractApproval.CONTRACT_APPROVAL_STATUS) Then
                '承認依頼中
                Me.SC3070208_IsUnderRequest.Value = Boolean.TrueString

                Me.SC3070208_RequestDate.Text = HttpUtility.HtmlEncode(DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, _
                                                                                                      drContractApproval.CONTRACT_APPROVAL_REQUESTDATE, _
                                                                                                      StaffContext.Current.DlrCD))
                If StaffContext.Current.Account = drContractApproval.CONTRACT_APPROVAL_STAFF Then
                    '自分
                    Me.SC3070208_SelectedSalesMangerName_Display.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(SC3070208TableAdapter.ProgramId, 3))
                Else
                    '自分以外
                    Me.SC3070208_SelectedSalesMangerName_Display.Text = HttpUtility.HtmlEncode(drContractApproval.USERNAME)
                End If
                Me.SC3070208_SelectedSalesMangerName.Value = drContractApproval.USERNAME
                Me.SC3070208_SelectedManagerAccount.Value = drContractApproval.CONTRACT_APPROVAL_STAFF
                Me.SC3070208_NoticeRequestid.Value = drContractApproval.NOTICEREQID

                'マネージャー欄、コメント欄の非活性スタイル
                AddCssClass(Me.SC3070208_SelectedSalesMangerNameAreaBox, "disabled")
                '2015/03/10 TCS 鈴木【TMT課題対応(#72 セールスタブレットの価格相談履歴表示)】ADD START
                AddCssClass(Me.SC3070208_StaffMemoArea, "disabled")
                If drContractApproval.IsSTAFFMEMONull() Then
                    Me.SC3070208_StaffMemo.Value = ""
                Else
                    Me.SC3070208_StaffMemo.Value = drContractApproval.STAFFMEMO
                End If
                '2015/03/10 TCS 鈴木【TMT課題対応(#72 セールスタブレットの価格相談履歴表示)】ADD END

                Me.SC3070208_UnderRequest.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(SC3070208TableAdapter.ProgramId, 6))
                Me.SC3070208_CancelButtonLiteral.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(SC3070208TableAdapter.ProgramId, 2))
                Me.SC3070208_RequestButton.Visible = False
                Me.SC3070208_IsExistManager.Value = Boolean.TrueString
            Else
                '承認依頼中以外
                Me.SC3070208_IsUnderRequest.Value = Boolean.FalseString

                '注文承認スタッフ一覧取得
                Dim dtApprovalStaffList As SC3070208DataSet.SC3070208ApprovalStaffListDataTable = bizLogic.GetStaffList()

                '注文承認スタッフ一覧作成
                Me.SC3070208_ApprovalStaffRepeater.DataSource = dtApprovalStaffList
                Me.SC3070208_ApprovalStaffRepeater.DataBind()

                '画面制御
                If 0 < dtApprovalStaffList.Count Then
                    'セールスマネージャーリストの先頭をデフォルト値として選択状態にする
                    If StaffContext.Current.Account = dtApprovalStaffList(0).ACCOUNT Then
                        '自分
                        Me.SC3070208_SelectedSalesMangerName_Display.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(SC3070208TableAdapter.ProgramId, 3))
                    Else
                        '自分以外
                        Me.SC3070208_SelectedSalesMangerName_Display.Text = HttpUtility.HtmlEncode(dtApprovalStaffList(0).USERNAME)
                    End If
                    Me.SC3070208_SelectedSalesMangerName.Value = dtApprovalStaffList(0).USERNAME
                    Me.SC3070208_SelectedManagerAccount.Value = dtApprovalStaffList(0).ACCOUNT
                    Me.SC3070208_SelectedManagerOnlineStatus.Value = dtApprovalStaffList(0).PRESENCECATEGORY
                End If

                'セールスマネージャー一覧作成
                Me.SC3070208_ApprovalStaffRepeater.DataSource = dtApprovalStaffList
                Me.SC3070208_ApprovalStaffRepeater.DataBind()

                Me.SC3070208_RequestButtonLiteral.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(SC3070208TableAdapter.ProgramId, 4))
                Me.SC3070208_NoSendAccountLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(SC3070208TableAdapter.ProgramId, 5))

                '価格相談中エリアを非表示にする
                Me.SC3070208_UnderRequestArea.Visible = False
                'キャンセルボタンエリアを非表示にする
                Me.SC3070208_CancelButton.Visible = False

                '依頼ボタンの活性/非活性制御
                Me.SC3070208_RequestButton.Attributes("Class") = String.Empty
                If OFFLINE.Equals(Me.SC3070208_SelectedManagerOnlineStatus.Value) Then
                    AddCssClass(Me.SC3070208_RequestButton, "disabled")
                End If

                'マネージャーがいない場合
                If dtApprovalStaffList.Count = 0 Then
                    Me.SC3070208_IsExistManager.Value = Boolean.FalseString
                Else
                    Me.SC3070208_IsExistManager.Value = Boolean.TrueString
                End If
            End If
        Next

        '上記で作成した価格相談画面のHTMLを返す
        Using sw As New System.IO.StringWriter(CultureInfo.CurrentCulture)
            Dim writer As HtmlTextWriter = New HtmlTextWriter(sw)
            Me.RenderControl(writer)
            Return sw.GetStringBuilder().ToString
        End Using

    End Function

    Private Sub AddCssClass(ByVal element As HtmlGenericControl, ByVal cssClass As String)
        If String.IsNullOrEmpty(element.Attributes("Class").Trim) Then
            element.Attributes("Class") = cssClass
        Else
            element.Attributes("Class") = element.Attributes("Class") & " " & cssClass
        End If
    End Sub

    Private Sub RemoveCssClass(ByVal element As HtmlGenericControl, ByVal cssClass As String)
        element.Attributes("Class") = element.Attributes("Class").Replace(cssClass, "")
    End Sub

    '更新： 2015/03/17 TCS 鈴木  次世代e-CRB 価格相談履歴参照機能開発 START
    '''' <summary>
    '''' スタッフメモの入力チェックを実施する
    '''' </summary>
    ''' <returns>入力チェックに成功した場合は空文字。それ以外の場合はメッセージ文字列。</returns>
    Private Function CheckApprovalInput(ByVal parameters As SC3070208DataSet.SC3070208ParameterDataTable) As String
        Dim parent As IEstimateInfoControl = CType(Me.Page, IEstimateInfoControl)

        If ((String.IsNullOrEmpty(parameters(0).STAFFMEMO) = False) _
            AndAlso (Validation.IsValidString(parameters(0).STAFFMEMO) = False)) Then
            'スタッフ入力コメントに禁則文字が含まれている場合
            Return WebWordUtility.GetWord(SC3070208TableAdapter.ProgramId, 945)
        End If

        Return ""
    End Function
    '更新： 2015/03/17 TCS 鈴木  次世代e-CRB 価格相談履歴参照機能開発 END


#End Region


#Region " ページクラス処理のバイパス処理 "

    ''' <summary>
    ''' RemoveValueBypass関数のバイパス
    ''' </summary>
    ''' <param name="pos">ポジジョン</param>
    ''' <param name="key">検索キー</param>
    ''' <remarks></remarks>
    Private Sub RemoveValueBypass(pos As Toyota.eCRB.SystemFrameworks.Web.ScreenPos, key As String)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_Start", GetCurrentMethod.Name), True)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name), True)
        GetPageInterface().RemoveValueBypass(pos, key)
    End Sub

    ''' <summary>
    ''' 親ページのインターフェース取得
    ''' </summary>
    ''' <returns>親ページのIEstimateInfoControl</returns>
    ''' <remarks></remarks>
    Private Function GetPageInterface() As IEstimateInfoControl
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_Start", GetCurrentMethod.Name), True)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name), True)
        Return CType(Me.Page, IEstimateInfoControl)
    End Function

#End Region


End Class
