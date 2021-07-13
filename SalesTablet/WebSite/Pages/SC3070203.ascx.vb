'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3070203.ascx.vb
'─────────────────────────────────────
'機能： 価格相談
'補足： 
'更新： 2013/06/30 TCS 葛西　  2013/10対応版　既存流用
'更新： 2013/11/28 TCS 森      Aカード情報相互連携開発
'更新： 2015/03/17 TCS 鈴木  次世代e-CRB 価格相談履歴参照機能開発
'─────────────────────────────────────

Option Explicit On

Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.Estimate.Quotation.BizLogic
Imports Toyota.eCRB.Estimate.Quotation.DataAccess.SC3070203DataSet
Imports Toyota.eCRB.iCROP.BizLogic.SC3070201


Partial Class Pages_SC3070203
    Inherits System.Web.UI.UserControl
    Implements ICallbackEventHandler

#Region "メンバ変数"
    '見積作成画面からの引継ぎ情報
    Private takingOverInfo As SC3070203TakingOverInfoDataTable
    Private callBackArgument As CallBackArgumentClass
#End Region

#Region "定数／列挙値"
    Private Enum ResultCode
        Success = 0
        Failure = -999
        DBTimeOut = -9
        CheckError = -1
    End Enum


    'ログインステータス－スタンバイ
    Private STANDBY As String = "1"
    'ログインステータス－商談中
    Private NEGOTIATION As String = "2"
    'ログインステータス－退席中
    Private LEAVING As String = "3"
    'ログインステータス－オフライン
    Private OFFLINE As String = "4"

    ' 金額フォーマット
    Private Const STR_MONEYFORMAT As String = "^[0-9]{1,9}(\.[0-9]{1,2})?$"
    '値引き金額の最小値
    Private MIN_VALUE As Double = 0
    '値引き金額の最大値
    Private MAX_VALUE As Double = 999999999.99

    '2015/03/06 TCS 鈴木【TMT課題対応(#72 セールスタブレットの価格相談履歴表示)】ADD START
    'スタッフ入力コメント桁数
    Private Const STAFF_MEMO_CNT As String = "128"
    '2015/03/06 TCS 鈴木【TMT課題対応(#72 セールスタブレットの価格相談履歴表示)】ADD END

#End Region

#Region "プロパティ"
    Public Property TriggerClientId() As String
        Get
            Return Me.SC3070203PopOverForm.Attributes("data-TriggerClientID")
        End Get
        Set(ByVal value As String)
            Me.SC3070203PopOverForm.Attributes("data-TriggerClientID") = value
        End Set
    End Property
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
        Dim result As Long
        Dim xml As New System.Xml.XmlDocument
        xml.LoadXml("<item><name>wrench</name></item>")

        Try
            callBackArgument = New CallBackArgumentClass
            callBackArgument = serializer.Deserialize(Of CallBackArgumentClass)(eventArgument)
            callBackResult = New CallBackResultClass

            Logger.Info(String.Format(CultureInfo.CurrentCulture, "Method={0} Estimateid={1} RequestPrice={2} RequestStaffMemo={3} Customerid={4} CustomerName={5} CustomerClass={6} CustomerKind={7} FollowUpBoxStoreCode={8} FollowUpBoxNumber={9} VehicleSequenceNumber={10} SalesStaffCode={11} SeriesCode={12} SeriesName={13} ModelCode={14} ModelName={15} ManagerAccount={16} ManagerName={17} Reasonid={18} NoticeRequestid={19} " _
             , callBackArgument.Method, callBackArgument.Estimateid, callBackArgument.RequestPrice, callBackArgument.RequestStaffMemo, callBackArgument.Customerid, callBackArgument.CustomerName, callBackArgument.CustomerClass, callBackArgument.CustomerKind, callBackArgument.FollowUpBoxStoreCode, callBackArgument.FollowUpBoxNumber, callBackArgument.VehicleSequenceNumber, callBackArgument.SalesStaffCode, callBackArgument.SeriesCode, callBackArgument.SeriesName, callBackArgument.ModelCode, callBackArgument.ModelName, callBackArgument.ManagerAccount, callBackArgument.ManagerName, callBackArgument.Reasonid, callBackArgument.NoticeRequestid))
            '引継ぎ情報作成
            CreateTakingOverInfo()

            callBackResult.Caller = callBackArgument.Method
            Select Case callBackArgument.Method
                Case "CreateWindow"
                    '価格相談画面を作成する
                    Dim contents As String

                    contents = CreateWindow()
                    callBackResult.ResultCode = ResultCode.Success
                    callBackResult.Message = String.Empty
                    callBackResult.Contents = contents
                Case "InsertInfo"
                    '依頼ボタン押下処理
                    SC3070203_StaffMemo.Value = takingOverInfo.Rows(0).Item("RequestStaffMemo")

                    '2015/03/06 TCS 鈴木【TMT課題対応(#72 セールスタブレットの価格相談履歴表示)】ADD START
                    '入力チェック
                    Dim checkMessage As String = CheckApprovalInput()
                    If checkMessage <> "" Then
                        callBackResult.ResultCode = ResultCode.CheckError
                        callBackResult.Message = checkMessage
                    Else
                        ' 2013/11/28 TCS 森      Aカード情報相互連携開発 START
                        result = InsertInfo()

                        callBackResult.ResultCode = ResultCode.Success
                        callBackResult.Message = String.Empty
                        ' 2013/11/28 TCS 森      Aカード情報相互連携開発 END
                    End If
                    '2015/03/06 TCS 鈴木【TMT課題対応(#72 セールスタブレットの価格相談履歴表示)】ADD END

                Case "CancelInfo"
                    'キャンセルボタン押下処理
                    result = CancelInfo()
                    callBackResult.ResultCode = ResultCode.Success
                    callBackResult.Message = String.Empty
            End Select

        Catch ex As OracleExceptionEx
            If ex.ErrorCode = 111 Then
                callBackResult.ResultCode = ResultCode.DBTimeOut
                callBackResult.Message = WebWordUtility.GetWord("SC3070203", 901)
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
          "sc3070203Script.callBack.beginCallback = function () {{ {0}; }};", _
          Page.ClientScript.GetCallbackEventReference(Me, _
         "sc3070203Script.callBack.packedArgument", _
         "sc3070203Script.callBack.endCallback", _
         "", _
         False)), _
          True)

    End Sub

    Protected Sub SalesManagerRepeater_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles SC3070203_SalesManagerRepeater.ItemDataBound
        If e.Item.ItemType = ListItemType.Item _
         OrElse e.Item.ItemType = ListItemType.AlternatingItem Then

            Dim view As Data.DataView = DirectCast(e.Item.DataItem.DataView, Data.DataView)
            Dim row As SC3070203SalesManagerRow = DirectCast(e.Item.DataItem.row, SC3070203SalesManagerRow)
            Dim onlineStatusIconArea As HtmlGenericControl = DirectCast(e.Item.FindControl("SC3070203_OnlineStatusIconArea"), HtmlGenericControl)
            Dim salesMangerRow As HtmlGenericControl = DirectCast(e.Item.FindControl("SC3070203_SalesMangerRow"), HtmlGenericControl)

            salesMangerRow.Attributes("Class") = ""
            AddCssClass(onlineStatusIconArea, "ncv51OnOffIcn")


            Select Case row.PRESENCECATEGORY
                Case STANDBY, NEGOTIATION, LEAVING
                    'スタンバイ、商談中、退席中はオンライン
                    AddCssClass(salesMangerRow, "Online")
                    AddCssClass(onlineStatusIconArea, "ncv51OnIcn")
                Case Else
                    AddCssClass(salesMangerRow, "Offline")
                    AddCssClass(onlineStatusIconArea, "ncv51OffIcn")
            End Select

            If row.ACCOUNT.Equals(SC3070203_SelectedManagerAccount.Value) Then
                AddCssClass(salesMangerRow, "Check")
            End If

            If view.Count - 1 = e.Item.ItemIndex Then
                'リスト最終行の罫線の設定
                AddCssClass(salesMangerRow, "ListEnd")
            End If
        End If
    End Sub

    Protected Sub PriceConsultationResonRepeater_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles PriceConsultationResonRepeater.ItemDataBound
        If e.Item.ItemType = ListItemType.Item _
         OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim view As Data.DataView = DirectCast(e.Item.DataItem.DataView, Data.DataView)
            Dim priceConsultationResonRow As HtmlGenericControl = DirectCast(e.Item.FindControl("PriceConsultationResonRow"), HtmlGenericControl)

            priceConsultationResonRow.Attributes("Class") = ""

            If view.Count - 1 = e.Item.ItemIndex Then
                priceConsultationResonRow.Attributes("Class") = priceConsultationResonRow.Attributes("Class") & " ListEnd"
            End If
        End If
    End Sub

#End Region

#Region "パブリックメソッド"

#End Region

#Region "プライベートメソッド"
    Private Sub CreateTakingOverInfo()
        '見積作成画面からの引継ぎ情報作成
        Me.takingOverInfo = New SC3070203TakingOverInfoDataTable
        Dim takingOverInfoRow As SC3070203TakingOverInfoRow = takingOverInfo.NewSC3070203TakingOverInfoRow()
        With takingOverInfoRow
            .ESTIMATEID = Me.callBackArgument.Estimateid
            If Me.callBackArgument.RequestPrice.HasValue Then
                .REQUESTPRICE = Me.callBackArgument.RequestPrice.Value
            End If
            '2015/03/06 TCS 鈴木 【TMT課題対応(#72 セールスタブレットの価格相談履歴表示)】ADD START
            .RequestStaffMemo = Me.callBackArgument.RequestStaffMemo
            '2015/03/06 TCS 鈴木 【TMT課題対応(#72 セールスタブレットの価格相談履歴表示)】ADD END
            .Customerid = Me.callBackArgument.Customerid
            .CustomerName = Me.callBackArgument.CustomerName
            .CUSTOMERCLASS = Me.callBackArgument.CustomerClass
            .CustomerKind = Me.callBackArgument.CustomerKind
            .FOLLOWUPBOXSTORECODE = Me.callBackArgument.FollowUpBoxStoreCode
            If Me.callBackArgument.FollowUpBoxNumber.HasValue Then
                .FOLLOWUPBOXNUMBER = Me.callBackArgument.FollowUpBoxNumber
            End If
            .VEHICLESEQUENCENUMBER = Me.callBackArgument.VehicleSequenceNumber
            .SALESSTAFFCODE = Me.callBackArgument.SalesStaffCode
            .ManagerAccount = Me.callBackArgument.ManagerAccount
            .ManagerName = Me.callBackArgument.ManagerName
            If Me.callBackArgument.Reasonid.HasValue Then
                .Reasonid = Me.callBackArgument.Reasonid.Value
            End If
            If Me.callBackArgument.NoticeRequestid Then
                .NoticeRequestid = Me.callBackArgument.NoticeRequestid
            End If
            .SeriesCode = Me.callBackArgument.SeriesCode
            .SeriesName = Me.callBackArgument.SeriesName
            .ModelCode = Me.callBackArgument.ModelCode
            .ModelName = Me.callBackArgument.ModelName
        End With
        takingOverInfo.AddSC3070203TakingOverInfoRow(takingOverInfoRow)
    End Sub

    Private Sub InitializePriceConsultation()
        '価格相談画面を初期化する
        Me.SC3070203_SelectedSalesMangerName_Display.Text = String.Empty
        Me.SC3070203_SelectedSalesMangerName.Value = String.Empty
        Me.SC3070203_SelectedManagerAccount.Value = String.Empty
        Me.SC3070203_SelectedManagerOnlineStatus.Value = String.Empty

        Me.SC3070203_SelectedResonName_Display.Text = String.Empty
        Me.SC3070203_SelectedResonName.Value = String.Empty
        Me.SC3070203_SelectedResonid.Value = String.Empty

        Me.RequestPriceNew_Display.Text = String.Empty
        Me.RequestPriceNew.Value = String.Empty

        '2015/03/06 TCS 鈴木【TMT課題対応(#72 セールスタブレットの価格相談履歴表示)】ADD START
        Me.SC3070203_StaffMemo.Value = String.Empty
        '2015/03/06 TCS 鈴木【TMT課題対応(#72 セールスタブレットの価格相談履歴表示)】ADD END
    End Sub

    Private Function CreateWindow() As String
        '価格相談画面初期化
        InitializePriceConsultation()

        Dim bizLogic As New SC3070203BusinessLogic(Me.takingOverInfo)

        '価格相談状況取得
        Dim isUnderPriceConsultation As Boolean = bizLogic.IsUnderPriceConsultation

        '2015/03/10 TCS 鈴木【TMT課題対応(#72 セールスタブレットの価格相談履歴表示)】スコープ変更 ADD START
        '価格相談中の内容を取得する
        Dim priceConsultationInfo As SC3070203PriceConsultationInfoDataTable = bizLogic.SelectUnderPriceConsultationInfo()
        '2015/03/10 TCS 鈴木【TMT課題対応(#72 セールスタブレットの価格相談履歴表示)】スコープ変更 ADD END

        If isUnderPriceConsultation Then
            '価格相談中の場合
            Me.SC3070203_IsUnderRequest.Value = Boolean.TrueString

            Dim row As SC3070203PriceConsultationInfoRow = priceConsultationInfo(0)

            Me.SC3070203_RequestDate.Text = DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, row.REQUESTDATE, StaffContext.Current.DlrCD)
            Me.SC3070203_SelectedSalesMangerName_Display.Text = row.MANAGER_NAME
            Me.SC3070203_SelectedSalesMangerName.Value = row.MANAGER_NAME
            Me.SC3070203_SelectedManagerAccount.Value = row.MANAGER_ACCOUNT
            Me.SC3070203_SelectedResonName_Display.Text = row.REASON_MSG_DLR
            Me.SC3070203_SelectedResonName.Value = row.REASON_MSG_DLR
            If row.IsREASONIDNull = False Then
                Me.SC3070203_SelectedResonid.Value = row.REASONID
            End If

            Me.RequestPriceNew_Display.Text = row.REQUESTPRICE.ToString("F2", CultureInfo.CurrentCulture)
            Me.RequestPriceNew.Value = row.REQUESTPRICE.ToString("F2", CultureInfo.CurrentCulture)
            '2015/03/06 TCS 鈴木【TMT課題対応(#72 セールスタブレットの価格相談履歴表示)】ADD START
            If row.IsSTAFFMEMONull = False Then
                Me.SC3070203_StaffMemo.Value = row.STAFFMEMO
            End If
            '2015/03/06 TCS 鈴木【TMT課題対応(#72 セールスタブレットの価格相談履歴表示)】ADD END

            Me.SC3070203_NoticeRequestid.Value = row.NOTICEREQID.ToString(CultureInfo.CurrentCulture)

            'マネージャー欄、値引き理由欄、値引き金額の非活性スタイル
            AddCssClass(Me.SC3070203_SelectedSalesMangerNameAreaRow, "disabled")
            AddCssClass(Me.SC3070203_SelectedResonNameAreaRow, "disabled")
            AddCssClass(Me.RequestPriceNewArea, "disabled")
            '2015/03/10 TCS 鈴木【TMT課題対応(#72 セールスタブレットの価格相談履歴表示)】ADD START
            AddCssClass(Me.SC3070203_StaffMemoArea, "disabled")
            '2015/03/10 TCS 鈴木【TMT課題対応(#72 セールスタブレットの価格相談履歴表示)】ADD END

            Me.SC3070203_UnderRequest.Text = WebWordUtility.GetWord("SC3070203", 4)
            Me.SC3070203_CancelButtonLiteral.Text = WebWordUtility.GetWord("SC3070203", 5)
            '履歴エリアを非表示にする
            Me.SC3070203_NewestHistoryArea.Visible = False
            '依頼ボタンエリアを非表示にする
            Me.SC3070203_RequestButton.Visible = False
            Me.SC3070203_IsExistManager.Value = Boolean.TrueString

        Else
            '価格相談中でない場合
            Me.SC3070203_IsUnderRequest.Value = Boolean.FalseString

            'セールスマネージャー一覧を取得する
            Dim managerList As SC3070203SalesManagerDataTable = bizLogic.SelectSalesManagerList()
            '値引き理由一覧を取得する
            Dim reasonList As SC3070203PriceConsultationReasonDataTable = bizLogic.SelectPriceConsultationResonList

            '価格相談の最新履歴を取得する
            Dim newestHistory As SC3070203PriceConsultationInfoDataTable = bizLogic.SelectPriceConsultationNewestHistory()

            ' 2013/11/28 TCS 森      Aカード情報相互連携開発 START
            Dim estCstDiscountPrice As Integer = bizLogic.SelectDiscountPriceInfo()
            ' 2013/11/28 TCS 森      Aカード情報相互連携開発 END

            '最新履歴を設定する
            If newestHistory.Count > 0 Then
                Me.SC3070203_HasHistory.Value = Boolean.TrueString
                Me.SC3070203_ApprovedDate.Text = DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, newestHistory(0).APPROVEDDATE, StaffContext.Current.DlrCD)
                '承認額は、マイナス表示するため-1を掛ける
                If (newestHistory(0).IsAPPROVEDPRICENull()) Then
                    Me.SC3070203_ApprovedPrice.Text = String.Empty
                Else
                    Me.SC3070203_ApprovedPrice.Text = (newestHistory(0).APPROVEDPRICE * -1).ToString("F2", CultureInfo.CurrentCulture)
                End If

                '前回の相談先をデフォルト値として選択状態にする
                Me.SC3070203_SelectedSalesMangerName_Display.Text = newestHistory(0).MANAGER_NAME
                Me.SC3070203_SelectedSalesMangerName.Value = newestHistory(0).MANAGER_NAME
                Me.SC3070203_SelectedManagerAccount.Value = newestHistory(0).MANAGER_ACCOUNT
                Me.SC3070203_SelectedManagerOnlineStatus.Value = newestHistory(0).PRESENCECATEGORY
            Else
                Me.SC3070203_HasHistory.Value = Boolean.FalseString
                Me.SC3070203_NewestHistoryArea.Visible = False
                If managerList.Count > 0 Then
                    'セールスマネージャーリストの先頭をデフォルト値として選択状態にする
                    Me.SC3070203_SelectedSalesMangerName_Display.Text = managerList(0).USERNAME
                    Me.SC3070203_SelectedSalesMangerName.Value = managerList(0).USERNAME
                    Me.SC3070203_SelectedManagerAccount.Value = managerList(0).ACCOUNT
                    Me.SC3070203_SelectedManagerOnlineStatus.Value = managerList(0).PRESENCECATEGORY
                End If
            End If

            'セールスマネージャー一覧作成
            Me.SC3070203_SalesManagerRepeater.DataSource = managerList
            Me.SC3070203_SalesManagerRepeater.DataBind()

            '値引き理由一覧作成
            Me.PriceConsultationResonRepeater.DataSource = reasonList
            Me.PriceConsultationResonRepeater.DataBind()

            '値引き金額の設定
            ' 2013/11/28 TCS 森      Aカード情報相互連携開発 START

            Me.RequestPriceNew_Display.Text = estCstDiscountPrice.ToString("F2", CultureInfo.CurrentCulture)
            Me.RequestPriceNew.Value = estCstDiscountPrice.ToString("F2", CultureInfo.CurrentCulture)

            ' 2013/11/28 TCS 森      Aカード情報相互連携開発 END

            Me.SC3070203_RequestButtonLiteral.Text = WebWordUtility.GetWord("SC3070203", 3)
            Me.SC3070203_NoSendAccountLabel.Text = WebWordUtility.GetWord("SC3070203", 9)

            '価格相談中エリアを非表示にする
            Me.SC3070203_UnderRequestArea.Visible = False
            'キャンセルボタンエリアを非表示にする
            Me.SC3070203_CancelButton.Visible = False

            '依頼ボタンの活性/非活性制御
            Me.SC3070203_RequestButton.Attributes("Class") = String.Empty
            If OFFLINE.Equals(Me.SC3070203_SelectedManagerOnlineStatus.Value) _
            OrElse String.IsNullOrEmpty(Me.RequestPriceNew.Value) Then
                AddCssClass(Me.SC3070203_RequestButton, "disabled")
            End If

            'マネージャーがいない場合
            If managerList.Count = 0 Then
                Me.SC3070203_IsExistManager.Value = Boolean.FalseString
            Else
                Me.SC3070203_IsExistManager.Value = Boolean.TrueString
            End If
        End If

        '上記で作成した価格相談画面のHTMLを返す
        Using sw As New System.IO.StringWriter(CultureInfo.CurrentCulture)
            Dim writer As HtmlTextWriter = New HtmlTextWriter(sw)
            Me.RenderControl(writer)
            Return sw.GetStringBuilder().ToString
        End Using

    End Function


    Private Function InsertInfo() As Long
        '依頼ボタン押下処理
        Dim bizLogic As New SC3070203BusinessLogic(Me.takingOverInfo)

        Return bizLogic.InsertPriceConsultationInfo()
    End Function


    Private Function CancelInfo() As Long
        'キャンセルボタン押下処理
        Dim bizLogic As New SC3070203BusinessLogic(Me.takingOverInfo)

        Return bizLogic.CancelPriceConsultationInfo()
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
    Private Function CheckApprovalInput() As String
        Dim parent As IEstimateInfoControl = CType(Me.Page, IEstimateInfoControl)

        If ((String.IsNullOrEmpty(SC3070203_StaffMemo.Value) = False) _
            AndAlso (Validation.IsValidString(SC3070203_StaffMemo.Value) = False)) Then
            'スタッフ入力コメントに禁則文字が含まれている場合
            Return WebWordUtility.GetWord("SC3070203", 906)
        End If

        Return ""
    End Function
    '更新： 2015/03/17 TCS 鈴木  次世代e-CRB 価格相談履歴参照機能開発 END


#End Region




End Class
