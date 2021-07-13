'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3070206.ascx.vb
'─────────────────────────────────────
'機能： 価格相談回答
'補足： 
'作成： 2013/12/09 TCS 外崎  Aカード情報相互連携開発
'─────────────────────────────────────

Option Explicit On

Imports Toyota.eCRB.Estimate.Quotation.BizLogic
Imports Toyota.eCRB.Estimate.Quotation.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Web.Controls
Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Globalization
Imports System.Reflection.MethodBase
Imports Toyota.eCRB.iCROP.BizLogic.SC3070201

Partial Class Pages_SC3070206
    Inherits System.Web.UI.UserControl

    Private _noticeReqId As Long
    Private _noticeReqCategory As IC3070201DataSet.IC3070201NoticeRequestRow
    Private _estimateIds As String
    Private _selectedEstimateIndex As Integer = -1
    Private _editMode As Boolean = False

    ''' <summary>
    ''' 見積作成画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_DISPID_QUOTATION As String = "SC3070201"

    ''' <summary>
    ''' メインメニュー画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_DISPID_MAINMENU As String = "SC3010203"

    ''' <summary>
    '''マネージャーコメント桁数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MANAGER_MEMO_CNT As String = "128"

    ''' <summary>
    ''' 通知依頼情報・最終ステータス（2:キャンセル）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NOTICE_REQ_STATUS_CANCEL As String = "2"

    ''' <summary>
    ''' 金額フォーマット
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_MONEYFORMAT As String = "^[0-9]{1,9}(\.[0-9]{1,2})?$"

    ''' <summary>
    ''' 編集モード
    ''' </summary>
    ''' <value></value>
    ''' <returns>True:編集可能 False:編集不可</returns>
    ''' <remarks></remarks>
    Public Property EditMode As Boolean
        Set(value As Boolean)
            _editMode = value
        End Set
        Get
            Return _editMode
        End Get
    End Property

    ''' <summary>
    ''' 通知依頼ID
    ''' </summary>
    ''' <remarks>親ページから渡される引数</remarks>
    Private ReadOnly Property NoticeReqId As Long
        Get
            Dim parent As IEstimateInfoControl = CType(Me.Page, IEstimateInfoControl)
            If _noticeReqId = Nothing Then
                _noticeReqId = CType(parent.GetValueBypass(ScreenPos.Current, "NoticeReqId", False), Long)
            End If
            Return _noticeReqId
        End Get
    End Property

    ''' <summary>
    ''' 見積ID（カンマ区切り）
    ''' </summary>
    ''' <remarks>親ページから渡される引数</remarks>
    Private ReadOnly Property EstimateIds As String
        Get
            Dim parent As IEstimateInfoControl = CType(Me.Page, IEstimateInfoControl)
            If _estimateIds = Nothing Then
                _estimateIds = CType(parent.GetValueBypass(ScreenPos.Current, "EstimateId", False), String)
            End If
            Return _estimateIds
        End Get
    End Property

    ''' <summary>
    ''' 選択中の見積り（インデックス番号）
    ''' </summary>
    ''' <remarks>親ページから渡される引数</remarks>
    Private ReadOnly Property SelectedEstimateIndex As Integer
        Get
            Dim parent As IEstimateInfoControl = CType(Me.Page, IEstimateInfoControl)
            If _selectedEstimateIndex = -1 Then
                If (parent.ContainsKeyBypass(ScreenPos.Current, "SelectedEstimateIndex")) Then
                    _selectedEstimateIndex = CType(parent.GetValueBypass(ScreenPos.Current, "SelectedEstimateIndex", False), Long)
                Else
                    _selectedEstimateIndex = 0
                End If
            End If
            Return _selectedEstimateIndex
        End Get
    End Property

    ''' <summary>
    ''' 現在選択中の見積ID
    ''' </summary>
    ''' <remarks></remarks>
    Private ReadOnly Property SelectedEstimateId As Long
        Get
            Dim estimateIds As String() = Me.EstimateIds.Split(","c)
            If (estimateIds.Length = 0) Then
                Return 0
            End If
            Return CType(estimateIds(Me.SelectedEstimateIndex), Long)
        End Get
    End Property

    ''' <summary>
    ''' ロード処理
    ''' </summary>
    Private Sub SC3070206_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If (Not Me.IsPostBack) Then
            Me.approvalDiscountMsgHiddenField.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(975))
            Dim bizLogic As New SC3070206BusinessLogic
            DispInitApprovalData(bizLogic.GetInitialData(Me.NoticeReqId))
        End If
    End Sub

    '''' <summary>
    '''' 価格入力欄 初期データ取得、表示
    '''' </summary>
    Private Sub DispInitApprovalData(ByVal dtApproval As SC3070206DataSet.SC3070206EstDiscountApprovalDataTable)
        For Each dr As SC3070206DataSet.SC3070206EstDiscountApprovalRow In dtApproval
            '依頼連番
            Me.seqNoHiddenField.Value = dr.Item("SEQNO").ToString

            'スタッフID
            If Not dr.IsSTAFFACCOUNTNull Then
                Me.staffCdHiddenField.Value = dr.Item("STAFFACCOUNT").ToString
            End If

            'スタッフ名
            If Not dr.IsUSERNAMENull Then
                Me.staffNameLabel.Text = HttpUtility.HtmlEncode(dr.Item("USERNAME").ToString)
            End If

            'スタッフ値引き額
            If Not dr.IsREQUESTPRICENull Then
                Me.requestDiscountPriceLabel.Text = dr.Item("REQUESTPRICE")
                Me.approvalPriceStaffHiddenField.Value = dr.Item("REQUESTPRICE")
            End If

            'スタッフ値引き理由
            If Not dr.IsMSG_DLRNull AndAlso _
                Not String.IsNullOrEmpty(dr.Item("MSG_DLR").ToString) Then
                Me.reasonLabel.Text = HttpUtility.HtmlEncode(dr.Item("MSG_DLR").ToString)
            End If

            'マネージャー値引き額
            If Not dr.IsAPPROVEDPRICENull AndAlso _
                Not String.IsNullOrEmpty(dr.Item("APPROVEDPRICE")) Then
                Me.ApprovalDiscountPriceLabel.Text = dr.Item("APPROVEDPRICE")
                Me.ApprovalDiscountPriceTextBox.Text = dr.Item("APPROVEDPRICE")
                Me.approvalPriceHiddenField.Value = dr.Item("APPROVEDPRICE")
            End If

            'スタッフ値引き額
            If Not dr.IsREQUESTPRICENull AndAlso _
                Not String.IsNullOrEmpty(dr.Item("REQUESTPRICE")) Then
                Me.requestDiscountPriceLabel.Text = dr.Item("REQUESTPRICE")
                Me.approvalPriceStaffHiddenField.Value = dr.Item("REQUESTPRICE")

            End If

            'マネージャーコメント
            If Not dr.IsSTAFFACCOUNTNull Then
                Me.managerMemoLabel.Text = HttpUtility.HtmlEncode(dr.Item("MANAGERMEMO").ToString)
                Me.managerMemoTextbox.Text = dr.Item("MANAGERMEMO").ToString
            End If

        Next
    End Sub

    ''' <summary>
    ''' サーバ側入力チェックを実施し、価格入力情報を保存する。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Private Sub sendManagerAnswer_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles sendButton.Click
        '入力チェック
        If Not CheckApprovalInputFormat() Then
            Exit Sub
        End If

        'マネージャー回答登録
        Dim staffInfo As StaffContext = StaffContext.Current

        Dim seqNo As Decimal = CType(Me.seqNoHiddenField.Value, Decimal)
        Dim managerMemo As String = Me.managerMemoTextbox.Text
        Dim discountPrice As Nullable(Of Double)
        Dim discountPriceDouble As Double
        If (Double.TryParse(ApprovalDiscountPriceTextBox.Text, discountPriceDouble)) Then
            discountPrice = discountPriceDouble
        End If

        'ビジネスロジックオブジェクト作成
        Dim bizLogic As SC3070206BusinessLogic = New SC3070206BusinessLogic

        'マネージャー回答送信前チェック
        If CheckApprovalStatus() = False Then
            '件数が0件、または、キャンセルの場合
            ScriptManager.RegisterStartupScript(Me, _
                    Me.GetType, _
                    "PageLoad", _
                    "dispLoading();alert(SC3070201HTMLDecode(""" + HttpUtility.HtmlEncode(WebWordUtility.GetWord(983)) + """));this_form.actionModeHiddenField.value = ""2"";this_form.submit();", _
                    True)
            Exit Sub
        End If

        '値引き額更新
        bizLogic.UpdateManagerAnswer(Me.SelectedEstimateId, _
                                           seqNo, _
                                           staffInfo.Account, _
                                           discountPrice, _
                                           managerMemo, _
                                           Me.NoticeReqId, _
                                           staffInfo.Account, _
                                           STR_DISPID_QUOTATION)

        'メインメニューへ遷移
        CType(Me.Page, BasePage).RedirectNextScreen(STR_DISPID_MAINMENU)
    End Sub

    '''' <summary>
    '''' 価格入力欄入力チェックを実施する（必須以外）
    '''' </summary>
    '''' <remarks></remarks>
    Private Function CheckApprovalInputFormat() As Boolean
        Dim bizLogic As New SC3070206BusinessLogic
        Dim parent As IEstimateInfoControl = CType(Me.Page, IEstimateInfoControl)

        If String.IsNullOrEmpty(ApprovalDiscountPriceTextBox.Text) Then
            'マネージャー値引き額が未入力でもOK
        ElseIf Not Validation.IsCorrectPattern(Me.ApprovalDiscountPriceTextBox.Text, STR_MONEYFORMAT) And _
            Not String.IsNullOrEmpty(Me.ApprovalDiscountPriceTextBox.Text) Then
            'マネージャー値引き額が数値でない場合
            parent.ShowMessageBoxBypass(975)

            Return False

        End If

        If Not Validation.IsCorrectDigit(managerMemoTextbox.Text, MANAGER_MEMO_CNT) And _
            Not String.IsNullOrEmpty(managerMemoTextbox.Text) Then
            'マネージャーコメントがX桁以上の場合
            parent.ShowMessageBoxBypass(976, MANAGER_MEMO_CNT.ToString())

            Return False

        ElseIf (Validation.IsValidString(managerMemoTextbox.Text) = False) And _
            Not String.IsNullOrEmpty(managerMemoTextbox.Text) Then
            'マネージャーコメントに禁則文字が含まれている場合
            parent.ShowMessageBoxBypass(977)

            Return False

        End If

        Return True
    End Function

    ''' <summary>
    ''' 価格相談状態チェック
    ''' </summary>
    ''' <returns>True:OK（キャンセル以外）、False:NG（キャンセル）</returns>
    Private Function CheckApprovalStatus() As Boolean
        Dim bizLogic As New SC3070206BusinessLogic
        Dim dtNoticeRequest As SC3070206DataSet.SC3070206NoticeRequestInfoDataTable = bizLogic.GetManagerAnswerCheck(NoticeReqId)

        If dtNoticeRequest.Rows.Count = 0 OrElse _
            dtNoticeRequest(0).STATUS.Equals(NOTICE_REQ_STATUS_CANCEL) Then
            '通知依頼が件数が0件、または、キャンセルの場合
            Return False
        Else
            'キャンセル以外の場合
            Return True
        End If
    End Function

    ''' <summary>
    ''' 契約状態チェック
    ''' </summary>
    ''' <returns>True:OK（未契約）、False:NG（契約済み）</returns>
    ''' <remarks>契約状態状態をチェックする</remarks>
    Private Function CheckContract() As Boolean
        Dim bizLogic As New SC3070206BusinessLogic
        Dim dt As SC3070206DataSet.SC3070206ContractDataTable = bizLogic.GetContract(Me.SelectedEstimateId)

        If (dt.Rows.Count = 0) OrElse (dt(0).CONTRACTFLG <> "1") Then
            Return True
        Else
            Return False
        End If
    End Function

End Class
