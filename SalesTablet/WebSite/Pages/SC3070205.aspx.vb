'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3070205.aspx.vb
'─────────────────────────────────────
'機能： 見積作成
'補足： 
'作成： 2013/12/11 TCS 河原
'─────────────────────────────────────

Imports System.Data
Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.Estimate.Quotation.BizLogic
Imports Toyota.eCRB.Estimate.Quotation.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic

''' <summary>
''' 見積作成画面
''' </summary>
''' <remarks></remarks>
Partial Class Pages_SC3070205
    Inherits BasePage

#Region "定数定義"

    ''' <summary>
    ''' TRUE
    ''' </summary>
    ''' <remarks></remarks>
    Protected Const StrTrue As String = "TRUE"

    ''' <summary>
    ''' FALSE
    ''' </summary>
    ''' <remarks></remarks>
    Protected Const StrFalse As String = "FALSE"

    ''' <summary>
    ''' 契約書状況フラグ (０：未契約)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_CONTRACTFLG_NOT As String = "0"

    ''' <summary>
    ''' 削除フラグ (０：未削除)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_DELETEFLG_NOT As String = "0"

    ''' <summary>
    ''' 契約顧客種別（１：所有者）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_CONTCUSTTYPE_SYOYUSYA As String = "1"

    ''' <summary>
    ''' 契約顧客種別（２：使用者）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_CONTCUSTTYPE_SHIYOSYA As String = "2"

    ''' <summary>
    ''' 顧客種別（１：自社客）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_CUSTKIND_JISYA As String = "1"

    ''' <summary>
    ''' 見積作成画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_DISPID_QUOTATION As String = "SC3070205"

    ''' <summary>
    ''' 見積書印刷画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_DISPID_QUOTATIONPREVIEW As String = "SC3070202"

    ''' <summary>
    ''' 契約書印刷画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_DISPID_CONTRACTPREVIEW As String = "SC3070301"

    ''' <summary>
    ''' メインメニュー画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_DISPID_MAINMENU As String = "SC3010203"

    ''' <summary>
    ''' 支払方法区分（１：現金)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_PAYMETHOD_CASH As Integer = 1

    ''' <summary>
    ''' 支払方法区分（２：ローン)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_PAYMETHOD_LOAN As Integer = 2

    ''' <summary>
    ''' 費用項目コード（１)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_ITEMCODE_1 As Integer = 1

    ''' <summary>
    ''' 費用項目コード（２)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_ITEMCODE_2 As Integer = 2

    ''' <summary>
    ''' 支払い方法（現金）特定文字列
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_GETCASH As String = "PAYMENTMETHOD='1'"

    ''' <summary>
    ''' 支払い方法（ローン）特定文字列
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_GETLOAN As String = "PAYMENTMETHOD='2'"

    ''' <summary>
    ''' 支払い方法特定文字列
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_GETPAYMETHOD As String = "DELFLG='0'"

    ''' <summary>
    ''' 諸費用項目（車両購入税用）特定文字列
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_GETCARBUYTAX As String = "ITEMCODE='1'"

    ''' <summary>
    ''' 諸費用項目（登録費用）特定文字列
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_GETREGCOST As String = "ITEMCODE='2'"

    ''' <summary>
    ''' 諸費用項目（手入力）特定文字列
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_GET_CHARGEFREE As String = "ITEMCODE<>'1' AND ITEMCODE<>'2'"

    ''' <summary>
    ''' 見積顧客情報（所有者）特定文字列
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_GETCUSTSHOYUSYA As String = "CONTRACTCUSTTYPE='1'"

    ''' <summary>
    ''' 見積顧客情報（使用者）特定文字列
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_GETCUSTSHIYOSYA As String = "CONTRACTCUSTTYPE='2'"

    ''' <summary>
    ''' 見積顧客情報件数（見積新規作成時)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const INT_CUSTOMERCOUNT_NEW As Integer = 0

    ''' <summary>
    ''' 契約状況フラグ（１：契約済み)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_CONTRACTFLG_COMP As String = "1"

    ''' <summary>
    ''' 顧客区分（１：個人)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_CUSTPART_KOJIN As String = "1"

    ''' <summary>
    ''' 顧客区分（２：法人)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_CUSTPART_HOJIN As String = "2"

    ''' <summary>
    ''' 顧客区分（０：法人)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_ORG_CUSTPART_HOJIN As String = "0"

    ''' <summary>
    ''' 保険区分（１：自社)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_INSUDVS_JISYA As String = "1"

    ''' <summary>
    ''' 保険区分（２：他社)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_INSUDVS_TASYA As String = "2"

    '''
    ''' <summary>
    '''価格相談モード（0：通常)
    ''' </summary>
    ''' <remarks></remarks>
    Protected Const ModeNormal As String = "0"

    ''' <summary>
    '''価格相談モード（1：マネージャ価格相談)
    ''' </summary>
    ''' <remarks></remarks>
    Protected Const ModeApprovalManager As String = "1"

    ''' <summary>
    '''価格相談モード（2:スタッフ回答参照)
    ''' </summary>
    ''' <remarks></remarks>
    Protected Const ModeApprovalStaff As String = "2"

    ''' <summary>
    '''マネージャーコメント桁数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MANAGER_MEMO_CNT As String = "512"

    ''' <summary>
    '''CR活動結果(SUCCESS)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CRACTRESULT_SUCCESS As String = "3"

    ''' <summary>
    '''CR活動結果(GIVEUP)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CRACTRESULT_GIVEUP As String = "5"

    ''' <summary>
    '''CR活動結果(ENQUIRY_COMPLETED)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CRACTRESULT_ENQUIRY_COMPLETED As String = "6"

    ''' <summary>
    '''価格相談ボタン押下フラグ(ON)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const APPROVAL_BUTTON_FLG_ON As String = "1"

    ''' <summary>
    '''価格相談ボタン押下フラグ(OFF)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const APPROVAL_BUTTON_FLG_OFF As String = "0"

    ''' <summary>
    ''' 実行モード（１：契約）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_ACTIONMODE_CONTRACT As String = "1"

    '''
    ''' <summary>
    ''' 実行モード（2：メインメニュー移動）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_ACTIONMODE_MENU As String = "2"

    ''' <summary>
    ''' 処理区分（見積切替）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_ACTIONMODE_ESTIMATE_CHANGE As String = "3"

    ''' <summary>
    ''' 処理区分（見積書印刷）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_ACTIONMODE_ESTIMATE_PRINT As String = "4"

    ''' <summary>
    ''' 処理区分（契約書印刷）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_ACTIONMODE_CONTRACT_PRINT As String = "5"

    ''' <summary>
    ''' 処理区分（契約確定）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_ACTIONMODE_CONTRACT_SEND As String = "6"

    ''' <summary>
    ''' 処理区分（契約キャンセル）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_ACTIONMODE_CONTRACT_CANCEL As String = "7"

    ''' <summary>
    ''' 処理区分（注文書印刷）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_ACTIONMODE_ORDER_PRINT As String = "8"

    ''' <summary>
    ''' 敬称位置（１：前）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_NAMETITLE_MAE As String = "1"

    ''' <summary>
    ''' 敬称位置（２：後）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_NAMETITLE_ATO As String = "2"

    ''' <summary>
    ''' オプション区分（TCV：メーカー）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_OPTIONPART_MAKER As String = "1"

    ''' <summary>
    ''' オプション区分（TCV：販売店）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_OPTIONPART_DEALER As String = "2"

    ''' <summary>
    ''' オプション区分（i-CROP：販売店）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_OPTIONPART_DEALER_ICROP As String = "9"

    ''' <summary>
    ''' TCVパラメータ（データ読み込み元）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_ESTIMATEID As String = "EstimateId"

    ''' <summary>
    ''' 金額フォーマット
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_MONEYFORMAT As String = "^[0-9]{1,9}(\.[0-9]{1,2})?$"

    ''' <summary>
    ''' マイナス付の金額フォーマット
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_MONEYFORMAT_MINUS As String = "^([\-])?[0-9]{1,9}(\.[0-9]{1,2})?$"

    ''' <summary>
    ''' 利息フォーマット
    ''' </summary>
    ''' <remarks>整数3桁以内、小数点以下3桁以内</remarks>
    Private Const STR_INTERESTRATE_FORMAT As String = "^[0-9]{1,3}(\.[0-9]{1,3})?$"

    '''
    ''' <summary>
    ''' 通知依頼情報・最終ステータス（2:キャンセル）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NOTICE_REQ_STATUS_CANCEL As String = "2"

    ''' <summary>
    ''' セッションキー(FollowUpBoxNo)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_FOLLOW_UP_BOX As String = "SearchKey.FOLLOW_UP_BOX"

    ''' <summary>
    ''' 選択フラグ(選択)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_SELECTFLG_SELECTED As String = "1"

    ''' <summary>
    ''' 選択フラグ(未選択)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_SELECTFLG_NOT As String = "0"

#End Region

#Region "メンバ変数"
    Private commonMasterPage As CommonMasterPage
    Private mainMenuButton As CommonMasterFooterButton
    Private customerButton As CommonMasterFooterButton
    Private BtnPrint As LinkButton
    Private BtnDiscountApproval As LinkButton
    Protected dlrOptionCount As Integer
    Protected Property dlrOptionDataTable As IEnumerable = New List(Of Integer)
    Protected tradeInCarCount As Integer
    Private tradeInCarDataTable As DataTable
    Private tcvMkrOptionDataTable As DataTable
    Private tcvDlrOptionDataTable As DataTable
    Private chargeInfoDataTable As DataTable

    '呼び出し時の引数
    Protected Account As String
    Protected Dlrcd As String
    Protected Strcd As String
    Protected EstimateId As String
    Protected SelectedEstimateId As String
    Protected SalesFlg As String
    Protected DispModeFlg As String
    Protected ApprovalStatus As String
    Protected NoCustomerFlg As String

#End Region

    ''' <summary>
    ''' ロード時の処理を実施します。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <History>
    '''  2012/03/15 TCS 堀 【SALES_1B】号口課題No.81対応
    ''' </History>
    ''' <remarks></remarks>
    Private Sub SC3070205_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SC3070205_Load Start")

        '初期化
        dlrOptionCount = 0                       '販売店オプションID用
        tradeInCarCount = 0                      '下取り車両ID用

        '呼び出し元画面からの引数を設定
        initParam()

        'HIDDEN値設定
        'If OperationLocked Then
        '    Me.ReferenceModeHiddenField.Value = StrTrue     'ロックモード
        'Else
        '    Me.ReferenceModeHiddenField.Value = StrFalse     'ロックモード
        'End If

        If String.Equals(DispModeFlg, "1") Then
            Me.ReferenceModeHiddenField.Value = StrFalse
        Else
            Me.ReferenceModeHiddenField.Value = StrTrue
        End If


        If String.Equals(DispModeFlg, "1") Then
            Me.operationLockedHiddenField.Value = StrFalse
        Else
            Me.operationLockedHiddenField.Value = StrTrue
        End If

        'Me.operationLockedHiddenField.Value = Me.ReferenceModeHiddenField.Value     'ロックモード

        'If Not (Not Me.IsPostBack AndAlso Not Me.IsCallback) Then
        '    SetSessionReload()
        'End If

        '価格相談モード設定
        InitApprovalMode()

        'セッション値読み込みと読取専用フラグ判定
        'InitTcvParam()

        'カーアイコン定義
        InitButtonEvent()

        If (Not Me.IsPostBack AndAlso Not Me.IsCallback) Then

            '初期設定
            InitialSetting()

            '初期データ取得、表示
            DispInitData()

            If Me.strApprovalModeHiddenField.Value.Equals(ModeApprovalStaff) And Not String.IsNullOrEmpty(Me.lngFollowupBoxSeqNoHiddenField.Value) Then
                '活動に紐づく見積管理IDをセッションに設定
                'SetEstimateIdSession()
                '見積管理IDをHIDDEN値に設定
                SetEstimateIdHidden()
            End If

            '見積アイコンの表示
            DispInitCarIcon()

            SetClientMessage()

            '在庫情報への条件渡し
            'InitImsParam()

            '遷移時状態チェック
            'CheckStatus()
        Else

            'メインメニュー移動モード
            If Me.actionModeHiddenField.Value.Equals(STR_ACTIONMODE_MENU) Then

                'メインメニューへ遷移
                Me.RedirectNextScreen(STR_DISPID_MAINMENU)

            End If

            If Me.actionModeHiddenField.Value.Equals(STR_ACTIONMODE_ESTIMATE_CHANGE) Or
                Me.actionModeHiddenField.Value.Equals(STR_ACTIONMODE_ESTIMATE_PRINT) Or
                Me.actionModeHiddenField.Value.Equals(STR_ACTIONMODE_CONTRACT_PRINT) Or
                Me.actionModeHiddenField.Value.Equals(STR_ACTIONMODE_CONTRACT_SEND) Or
                Me.actionModeHiddenField.Value.Equals(STR_ACTIONMODE_CONTRACT_CANCEL) Or
                Me.actionModeHiddenField.Value.Equals(STR_ACTIONMODE_ORDER_PRINT) Then

                '初期化処理
                ReloadInitialSetting()

                '初期データ取得、表示
                DispInitData()

                '見積アイコンの表示
                DispInitCarIcon()

                '在庫情報への条件渡し
                'InitImsParam()

                '遷移時状態チェック
                'CheckStatus()

                Me.actionModeHiddenField.Value = vbEmpty

            Else
                'ポストバック時の値復元処理
                PreservateData()
            End If

        End If

        If ApprovalStatus.Equals("1") OrElse ApprovalStatus.Equals("2") Then
            Me.chargeSegmentedButton.Enabled = False
        End If

        Me.DispModeHiddenFlg.Value = DispModeFlg

        If String.Equals(DispModeFlg, "1") Then
            '通常時
            DefaultMode()
        Else
            '参照モード時
            ReferenceMode()
        End If

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SC3070205_Load End")

    End Sub

    ''' <summary>
    ''' 初期設定
    ''' </summary>
    ''' <remarks></remarks>
    ''' <History>
    '''  2012/04/24 TCS 鈴木(健) HTMLエンコード対応
    ''' </History>
    Private Sub InitialSetting()

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InitialSetting Start")

        '初期化
        Me.blnInputChangedClientHiddenField.Value = False

        '初期表示

        '所有者/使用者セグメントボタン表示
        With custClassSegmentedButton
            .Items.Add(New ListItem(HttpUtility.HtmlEncode(WebWordUtility.GetWord(5)), "1"))
            .Items.Add(New ListItem(HttpUtility.HtmlEncode(WebWordUtility.GetWord(6)), "2"))
        End With

        '初期選択
        custClassSegmentedButton.SelectedValue = "1"

        '現金/ローンセグメントボタン表示
        With payMethodSegmentedButton
            .Items.Add(New ListItem(HttpUtility.HtmlEncode(WebWordUtility.GetWord(43)), "1"))
            .Items.Add(New ListItem(HttpUtility.HtmlEncode(WebWordUtility.GetWord(44)), "2"))
        End With

        '初期選択
        payMethodSegmentedButton.SelectedValue = "1"

        '販売店/個人セグメントボタン表示
        With chargeSegmentedButton
            .Items.Add(New ListItem(HttpUtility.HtmlEncode(WebWordUtility.GetWord(73)), "1"))
            .Items.Add(New ListItem(HttpUtility.HtmlEncode(WebWordUtility.GetWord(74)), "2"))
        End With

        '初期選択
        chargeSegmentedButton.SelectedValue = "1"

        'セッション情報取得
        Dim lngEstimateId As Long               '見積管理ID
        Dim blnLockStatus As Boolean            'ロック状態

        Dim estimateId As String
        Dim selectedEstimateIndex As Long

        '見積ID(カンマ区切り)
        estimateId = CType(Me.GetValue(ScreenPos.Current, "EstimateId", False), String)

        '選択している見積IDのIndex
        If Me.ContainsKey(ScreenPos.Current, "SelectedEstimateIndex") Then
            selectedEstimateIndex = CType(Me.GetValue(ScreenPos.Current, "SelectedEstimateIndex", False), Long)
        Else
            selectedEstimateIndex = 0
        End If

        '選択している見積ID
        lngEstimateId = CType(GetSelectedEstimateId(estimateId, selectedEstimateIndex), Long)

        If Me.ContainsKey(ScreenPos.Current, "MenuLockFlag") Then
            blnLockStatus = Me.GetValue(ScreenPos.Current, "MenuLockFlag", False)
        Else
            blnLockStatus = False
        End If

        'HIDDEN値設定
        Me.lngEstimateIdHiddenField.Value = CType(lngEstimateId, String)

        Me.estimateIdHiddenField.Value = CType(estimateId, String)
        Me.selectedEstimateIndexHiddenField.Value = CType(selectedEstimateIndex, String)

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InitialSetting End")

    End Sub

    ''' <summary>
    ''' 呼び出しパラメータ設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub initParam()

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("initParam Start")

        Account = Request("Account")
        Dlrcd = Request("Dlrcd")
        Strcd = Request("Strcd")
        EstimateId = Request("EstimateId")
        SelectedEstimateId = Request("SelectedEstimateId")
        SalesFlg = Request("SalesFlg")
        DispModeFlg = Request("DispModeFlg")
        ApprovalStatus = Request("ApprovalStatus")
        NoCustomerFlg = Request("NoCustomerFlg")

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("initParam End")

    End Sub

    ''' <summary>
    ''' 初期データ取得、表示
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub DispInitData()

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DispInitData Start")

        Dim bizLogic As SC3070205BusinessLogic

        'ビジネスロジックオブジェクト作成
        bizLogic = New SC3070205BusinessLogic

        '初期表示データ取得（API使用）
        Dim dsEstimation As IC3070201DataSet    '見積情報格納用

        dsEstimation = New IC3070201DataSet

        '見積情報データテーブル作成
        Dim dtEstimateData As New SC3070205DataSet.SC3070205ESTIMATEDATADataTable
        Dim drEstimateData As DataRow = dtEstimateData.NewRow

        drEstimateData("ESTIMATEID") = Long.Parse(Me.lngEstimateIdHiddenField.Value, Globalization.CultureInfo.CurrentCulture)
        dtEstimateData.Rows.Add(drEstimateData)

        dsEstimation = bizLogic.GetEstimateInitialData(dtEstimateData)

        'ビューステートに見積情報保存
        ViewState("DataSetEstimation") = dsEstimation

        If dsEstimation.Tables("IC3070201EstimationInfo").Rows.Count <> 0 Then

            'HIDDEN値設定
            Me.strDlrcdHiddenField.Value = dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("DLRCD")
            If Not IsDBNull(dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("STRCD")) Then
                Me.strStrCdHiddenField.Value = dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("STRCD")
            End If
            If Not IsDBNull(dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("FLLWUPBOX_SEQNO")) Then
                Me.lngFollowupBoxSeqNoHiddenField.Value = dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("FLLWUPBOX_SEQNO")
            End If
            If Not IsDBNull(dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("CSTKIND")) Then
                Me.strCstKindHiddenField.Value = dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("CSTKIND")
            End If
            If Not IsDBNull(dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("CUSTOMERCLASS")) Then
                Me.strCustomerClassHiddenField.Value = dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("CUSTOMERCLASS")
            End If
            If Not IsDBNull(dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("CRCUSTID")) Then
                Me.strCRCustIdHiddenField.Value = dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("CRCUSTID")
            End If
            Me.basePriceHiddenField.Value = dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("BASEPRICE")
            Me.contractFlgHiddenField.Value = dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("CONTRACTFLG")

            '金額

            If Not IsDBNull(dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("DISCOUNTPRICE")) Then
                Me.discountPriceValueHiddenField.Value = dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("DISCOUNTPRICE")
            End If

            'メモ最大桁数取得
            Dim drEstMemoMax As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow

            drEstMemoMax = bizLogic.GetMemoMax()

            Me.memoMaxHiddenField.Value = drEstMemoMax.PARAMVALUE

            '初期表示データ取得
            Dim dsEstimateExtraData As SC3070205DataSet

            '見積情報データテーブル更新
            dtEstimateData.Clear()

            drEstimateData("ESTIMATEID") = Long.Parse(Me.lngEstimateIdHiddenField.Value, Globalization.CultureInfo.CurrentCulture)
            drEstimateData("DLRCD") = dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("DLRCD")
            dtEstimateData.Rows.Add(drEstimateData)

            '通知依頼IDをセット
            Dim lngNoticeReqId As Long
            If Not String.IsNullOrEmpty(Me.noticeReqIdHiddenField.Value) Then
                lngNoticeReqId = CType(Me.noticeReqIdHiddenField.Value, Long)
            Else
                lngNoticeReqId = 0
            End If

            '初期表示データ取得
            dsEstimateExtraData = bizLogic.GetInitialData(dtEstimateData, dsEstimation, lngNoticeReqId)

            '氏名敬称取得

            '敬称の設定値を取得
            Dim dtSysEnvSet As SC3070205DataSet.SC3070205SYSTEMENVSETTINGDataTable

            Using sysenvDataTbl As New SC3070205DataSet.SC3070205SYSTEMENVSETTINGDataTable

                Dim sysenvDataRow As SC3070205DataSet.SC3070205SYSTEMENVSETTINGRow
                sysenvDataRow = sysenvDataTbl.NewSC3070205SYSTEMENVSETTINGRow
                sysenvDataTbl.Rows.Add(sysenvDataRow)

                dtSysEnvSet = bizLogic.GetNameTitleSysenv(sysenvDataTbl)

            End Using

            '画面に取得した値を設定

            '■作成日/契約日
            If dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("CONTRACTFLG") = STR_CONTRACTFLG_COMP Then
                '契約済のとき

                Me.estPrintDateLabel.Visible = False
                Me.dateLabel.Text = DateTimeFunc.FormatDate(3, dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("CONTRACTDATE"))


                Me.contractNoTitleLabel.Visible = True
                If IsDBNull(dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("CONTRACTNO")) Then
                    Me.contractNoLabel.Text = ""
                Else
                    Me.contractNoLabel.Text = dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("CONTRACTNO")
                End If

            Else
                '未契約、キャンセルのとき

                If IsDBNull(dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("ESTPRINTDATE")) Then
                    '印刷未実行のとき

                    Me.estPrintDateLabel.Visible = False
                    Me.contractDateLabel.Visible = False
                    Me.dateLabel.Visible = False

                Else

                    Me.contractDateLabel.Visible = False
                    Me.dateLabel.Text = DateTimeFunc.FormatDate(3, dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("ESTPRINTDATE"))

                End If

                Me.contractNoTitleLabel.Visible = False
                Me.contractNoLabel.Visible = False

            End If

            '敬称
            If String.Equals(dtSysEnvSet.Rows(0).Item("NAMETITLEPOSITION"), STR_NAMETITLE_MAE) Then
                Me.shoyusyaKeisyoMaeLabel.Text = HttpUtility.HtmlEncode(dtSysEnvSet.Rows(0).Item("DEFOLTNAMETITLE"))
                Me.shiyosyaKeisyoMaeLabel.Text = HttpUtility.HtmlEncode(dtSysEnvSet.Rows(0).Item("DEFOLTNAMETITLE"))

            ElseIf String.Equals(dtSysEnvSet.Rows(0).Item("NAMETITLEPOSITION"), STR_NAMETITLE_ATO) Then
                Me.shoyusyaKeisyoMaeLabel.Visible = False
                Me.shiyosyaKeisyoMaeLabel.Visible = False
                Me.shoyusyaKeisyoAtoLabel.Text = HttpUtility.HtmlEncode(dtSysEnvSet.Rows(0).Item("DEFOLTNAMETITLE"))
                Me.shiyosyaKeisyoAtoLabel.Text = HttpUtility.HtmlEncode(dtSysEnvSet.Rows(0).Item("DEFOLTNAMETITLE"))
            End If


            '保険会社リスト作成
            Dim intI As Integer
            Dim InsComInsuComCd As New StringBuilder
            Dim InsComInsuKubun As New StringBuilder
            Dim InsComInsuComName As New StringBuilder

            For intI = 0 To dsEstimateExtraData.Tables("SC3070205ESTINSUCOMMAST").Rows.Count - 1

                InsComInsuComCd.Append(HttpUtility.HtmlEncode(HttpUtility.UrlEncode(dsEstimateExtraData.Tables("SC3070205ESTINSUCOMMAST").Rows(intI).Item("INSUCOMCD"))))
                InsComInsuKubun.Append(HttpUtility.HtmlEncode(HttpUtility.UrlEncode(dsEstimateExtraData.Tables("SC3070205ESTINSUCOMMAST").Rows(intI).Item("INSUDVS"))))
                InsComInsuComName.Append(HttpUtility.HtmlEncode(HttpUtility.UrlEncode(dsEstimateExtraData.Tables("SC3070205ESTINSUCOMMAST").Rows(intI).Item("INSUCOMNM"))))

                If intI <> dsEstimateExtraData.Tables("SC3070205ESTINSUCOMMAST").Rows.Count - 1 Then
                    InsComInsuComCd.Append(",")
                    InsComInsuKubun.Append(",")
                    InsComInsuComName.Append(",")
                End If
            Next
            Me.InsComInsuComCdHidden.Value = InsComInsuComCd.ToString
            Me.InsComInsuKubunHidden.Value = InsComInsuKubun.ToString
            Me.InsComInsuComNameHidden.Value = InsComInsuComName.ToString


            '保険種別リスト作成
            Dim intJ As Integer
            Dim InsKindInsuComCd As New StringBuilder
            Dim InsKindInsuKindCd As New StringBuilder
            Dim InsKindInsuKindNm As New StringBuilder

            For intJ = 0 To dsEstimateExtraData.Tables("SC3070205ESTINSUKINDMAST").Rows.Count - 1

                InsKindInsuComCd.Append(HttpUtility.HtmlEncode(HttpUtility.UrlEncode(dsEstimateExtraData.Tables("SC3070205ESTINSUKINDMAST").Rows(intJ).Item("INSUCOMCD"))))
                InsKindInsuKindCd.Append(HttpUtility.HtmlEncode(HttpUtility.UrlEncode(dsEstimateExtraData.Tables("SC3070205ESTINSUKINDMAST").Rows(intJ).Item("INSUKIND"))))
                InsKindInsuKindNm.Append(HttpUtility.HtmlEncode(HttpUtility.UrlEncode(dsEstimateExtraData.Tables("SC3070205ESTINSUKINDMAST").Rows(intJ).Item("INSUKINDNM"))))

                If intJ <> dsEstimateExtraData.Tables("SC3070205ESTINSUKINDMAST").Rows.Count - 1 Then
                    InsKindInsuComCd.Append(",")
                    InsKindInsuKindCd.Append(",")
                    InsKindInsuKindNm.Append(",")
                End If
            Next
            Me.InsKindInsuComCdHidden.Value = InsKindInsuComCd.ToString
            Me.InsKindInsuKindCdHidden.Value = InsKindInsuKindCd.ToString
            Me.InsKindInsuKindNmHidden.Value = InsKindInsuKindNm.ToString

            'データソース設定
            loanFinanceComRepeater.DataSource = dsEstimateExtraData.Tables("SC3070205FINANCECOMMAST")
            loanFinanceComRepeater.DataBind()

            '下取り車両件数 HIDDEN値設定
            Me.tradeInCarCountHiddenField.Value = dsEstimation.Tables("IC3070201TradeincarInfo").Rows.Count()

            Dim drChargeInfoFreeRows As DataRow()

            '諸費用項目が手入力に該当件数を設定
            drChargeInfoFreeRows = dsEstimation.Tables("IC3070201ChargeInfo").Select(STR_GET_CHARGEFREE)
            Me.chargeInfoCountHiddenField.Value = drChargeInfoFreeRows.Count

            drChargeInfoFreeRows = Nothing

            Me.payMethodHiddenField.Value = payMethodSegmentedButton.SelectedValue.ToString

            '画面表示項目設定

            If dsEstimation.Tables("IC3070201CustomerInfo").Rows.Count = INT_CUSTOMERCOUNT_NEW Then
                '見積り新規作成時

                If Not IsDBNull(dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("CRCUSTID")) Then
                    ' ■見積／契約者情報
                    ' ■□所有者
                    If String.Equals(dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("CSTKIND"), STR_CUSTKIND_JISYA) Then

                        If dsEstimateExtraData.Tables("SC3070205ORGCUSTOMER").Rows.Count <> 0 Then
                            ' 自社客
                            ' □氏名
                            Me.shoyusyaNameTextBox.Text = Trim(dsEstimateExtraData.Tables("SC3070205ORGCUSTOMER").Rows(0).Item("NAME"))
                            ' □住所
                            Me.shoyusyaZipCodeTextBox.Text = Trim(dsEstimateExtraData.Tables("SC3070205ORGCUSTOMER").Rows(0).Item("ZIPCODE"))
                            Me.shoyusyaAddressTextBox.Text = Trim(dsEstimateExtraData.Tables("SC3070205ORGCUSTOMER").Rows(0).Item("ADDRESS"))
                            ' □連絡先
                            Me.shoyusyaMobileTextBox.Text = Trim(dsEstimateExtraData.Tables("SC3070205ORGCUSTOMER").Rows(0).Item("MOBILE"))
                            Me.shoyusyaTelTextBox.Text = Trim(dsEstimateExtraData.Tables("SC3070205ORGCUSTOMER").Rows(0).Item("TELNO"))
                            ' □E-Mail
                            Me.shoyusyaEmailTextBox.Text = Trim(dsEstimateExtraData.Tables("SC3070205ORGCUSTOMER").Rows(0).Item("EMAIL1"))
                            ' □国民ID
                            Me.shoyusyaIDTextBox.Text = Trim(dsEstimateExtraData.Tables("SC3070205ORGCUSTOMER").Rows(0).Item("SOCIALID"))
                            ' □顧客区分
                            If String.Equals(dsEstimateExtraData.Tables("SC3070205ORGCUSTOMER").Rows(0).Item("CUSTYPE"), STR_ORG_CUSTPART_HOJIN) Then
                                Me.shoyusyaHojinCheckMark.Value = StrTrue
                            Else
                                Me.shoyusyaKojinCheckMark.Value = StrTrue
                            End If
                        End If
                    Else

                        If dsEstimateExtraData.Tables("SC3070205NEWCUSTOMER").Rows.Count <> 0 Then
                            ' 未取引客
                            '□氏名
                            Me.shoyusyaNameTextBox.Text = Trim(dsEstimateExtraData.Tables("SC3070205NEWCUSTOMER").Rows(0).Item("NAME"))
                            '□住所
                            Me.shoyusyaZipCodeTextBox.Text = Trim(dsEstimateExtraData.Tables("SC3070205NEWCUSTOMER").Rows(0).Item("ZIPCODE"))
                            Me.shoyusyaAddressTextBox.Text = Trim(dsEstimateExtraData.Tables("SC3070205NEWCUSTOMER").Rows(0).Item("ADDRESS"))
                            '□連絡先
                            Me.shoyusyaMobileTextBox.Text = Trim(dsEstimateExtraData.Tables("SC3070205NEWCUSTOMER").Rows(0).Item("MOBILE"))
                            Me.shoyusyaTelTextBox.Text = Trim(dsEstimateExtraData.Tables("SC3070205NEWCUSTOMER").Rows(0).Item("TELNO"))
                            '□E-Mail
                            Me.shoyusyaEmailTextBox.Text = Trim(dsEstimateExtraData.Tables("SC3070205NEWCUSTOMER").Rows(0).Item("EMAIL1"))
                            '□国民ID
                            If Not IsDBNull(dsEstimateExtraData.Tables("SC3070205NEWCUSTOMER").Rows(0).Item("SOCIALID")) Then
                                Me.shoyusyaIDTextBox.Text = Trim(dsEstimateExtraData.Tables("SC3070205NEWCUSTOMER").Rows(0).Item("SOCIALID"))
                            End If

                            '□顧客区分
                            If String.Equals(dsEstimateExtraData.Tables("SC3070205NEWCUSTOMER").Rows(0).Item("CUSTYPE"), STR_ORG_CUSTPART_HOJIN) Then
                                Me.shoyusyaHojinCheckMark.Value = StrTrue
                            Else
                                Me.shoyusyaKojinCheckMark.Value = StrTrue
                            End If

                        End If

                    End If
                Else
                    '客がいない場合、初期選択
                    Me.shoyusyaKojinCheckMark.Value = StrTrue

                End If


                Me.shiyosyaKojinCheckMark.Value = StrTrue
                Me.jisyaCheckMark.Value = StrTrue


            Else
                '見積り保存後

                If dsEstimation.Tables("IC3070201PaymentInfo").Rows.Count <> INT_CUSTOMERCOUNT_NEW Then

                    'NumericBox変更検知用
                    If Not IsDBNull(dsEstimation.Tables("IC3070201PaymentInfo").Rows(1).Item("PAYMENTPERIOD")) Then
                        Me.periodInitialValueHiddenField.Value = dsEstimation.Tables("IC3070201PaymentInfo").Rows(1).Item("PAYMENTPERIOD")
                    End If
                    If Not IsDBNull(dsEstimation.Tables("IC3070201PaymentInfo").Rows(1).Item("DUEDATE")) Then
                        Me.firstPayInitialValueHiddenField.Value = dsEstimation.Tables("IC3070201PaymentInfo").Rows(1).Item("DUEDATE")
                    End If

                    Me.savedEstimationFlgHiddenField.Value = "1"
                Else
                    Me.savedEstimationFlgHiddenField.Value = "0"

                End If


                ' ■見積／契約者情報
                ' ■□所有者

                '所有者行取得
                Dim drCustShoyusya As DataRow()
                Dim drCustShoyusyaRow As DataRow
                drCustShoyusya = dsEstimation.Tables("IC3070201CustomerInfo").Select(STR_GETCUSTSHOYUSYA)

                For Each drCustShoyusyaRow In drCustShoyusya

                    ' □氏名
                    If IsDBNull(drCustShoyusyaRow.Item("NAME")) Then
                        Me.shoyusyaNameTextBox.Text = ""
                    Else
                        Me.shoyusyaNameTextBox.Text = Trim(drCustShoyusyaRow.Item("NAME"))
                    End If
                    ' □住所
                    If IsDBNull(drCustShoyusyaRow.Item("ZIPCODE")) Then
                        Me.shoyusyaZipCodeTextBox.Text = ""
                    Else
                        Me.shoyusyaZipCodeTextBox.Text = Trim(drCustShoyusyaRow.Item("ZIPCODE"))
                    End If
                    If IsDBNull(drCustShoyusyaRow.Item("ADDRESS")) Then
                        Me.shoyusyaAddressTextBox.Text = ""
                    Else
                        Me.shoyusyaAddressTextBox.Text = Trim(drCustShoyusyaRow.Item("ADDRESS"))
                    End If
                    ' □連絡先
                    If IsDBNull(drCustShoyusyaRow.Item("MOBILE")) Then
                        Me.shoyusyaMobileTextBox.Text = ""
                    Else
                        Me.shoyusyaMobileTextBox.Text = Trim(drCustShoyusyaRow.Item("MOBILE"))
                    End If
                    If IsDBNull(drCustShoyusyaRow.Item("TELNO")) Then
                        Me.shoyusyaTelTextBox.Text = ""
                    Else
                        Me.shoyusyaTelTextBox.Text = Trim(drCustShoyusyaRow.Item("TELNO"))
                    End If
                    ' □E-Mail
                    If IsDBNull(drCustShoyusyaRow.Item("EMAIL")) Then
                        Me.shoyusyaEmailTextBox.Text = ""
                    Else
                        Me.shoyusyaEmailTextBox.Text = Trim(drCustShoyusyaRow.Item("EMAIL"))
                    End If
                    ' □国民ID
                    If IsDBNull(drCustShoyusyaRow.Item("SOCIALID")) Then
                        Me.shoyusyaIDTextBox.Text = ""
                    Else
                        Me.shoyusyaIDTextBox.Text = Trim(drCustShoyusyaRow.Item("SOCIALID"))
                    End If
                    ' □顧客区分
                    If String.Equals(drCustShoyusyaRow.Item("CUSTPART"), STR_CUSTPART_KOJIN) Then
                        Me.shoyusyaKojinCheckMark.Value = StrTrue
                    Else
                        Me.shoyusyaHojinCheckMark.Value = StrTrue
                    End If

                Next

                ' ■□使用者

                '使用者行取得
                Dim drCustShiyosya As DataRow()
                Dim drCustShiyosyaRow As DataRow
                drCustShiyosya = dsEstimation.Tables("IC3070201CustomerInfo").Select(STR_GETCUSTSHIYOSYA)

                For Each drCustShiyosyaRow In drCustShiyosya

                    ' □氏名
                    If IsDBNull(drCustShiyosyaRow.Item("NAME")) Then
                        Me.shiyosyaNameTextBox.Text = ""
                    Else
                        Me.shiyosyaNameTextBox.Text = Trim(drCustShiyosyaRow.Item("NAME"))
                    End If
                    ' □住所
                    If IsDBNull(drCustShiyosyaRow.Item("ZIPCODE")) Then
                        Me.shiyosyaZipCodeTextBox.Text = ""
                    Else
                        Me.shiyosyaZipCodeTextBox.Text = Trim(drCustShiyosyaRow.Item("ZIPCODE"))
                    End If
                    If IsDBNull(drCustShiyosyaRow.Item("ADDRESS")) Then
                        Me.shiyosyaAddressTextBox.Text = ""
                    Else
                        Me.shiyosyaAddressTextBox.Text = Trim(drCustShiyosyaRow.Item("ADDRESS"))
                    End If
                    ' □連絡先
                    If IsDBNull(drCustShiyosyaRow.Item("MOBILE")) Then
                        Me.shiyosyaMobileTextBox.Text = ""
                    Else
                        Me.shiyosyaMobileTextBox.Text = Trim(drCustShiyosyaRow.Item("MOBILE"))
                    End If
                    If IsDBNull(drCustShiyosyaRow.Item("TELNO")) Then
                        Me.shiyosyaTelTextBox.Text = ""
                    Else
                        Me.shiyosyaTelTextBox.Text = Trim(drCustShiyosyaRow.Item("TELNO"))
                    End If
                    ' □E-Mail
                    If IsDBNull(drCustShiyosyaRow.Item("EMAIL")) Then
                        Me.shiyosyaEmailTextBox.Text = ""
                    Else
                        Me.shiyosyaEmailTextBox.Text = Trim(drCustShiyosyaRow.Item("EMAIL"))
                    End If
                    ' □国民ID
                    If IsDBNull(drCustShiyosyaRow.Item("SOCIALID")) Then
                        Me.shiyosyaIDTextBox.Text = ""
                    Else
                        Me.shiyosyaIDTextBox.Text = Trim(drCustShiyosyaRow.Item("SOCIALID"))
                    End If
                    ' □顧客区分
                    If String.Equals(drCustShiyosyaRow.Item("CUSTPART"), STR_CUSTPART_KOJIN) Then
                        Me.shiyosyaKojinCheckMark.Value = StrTrue
                    Else
                        Me.shiyosyaHojinCheckMark.Value = StrTrue
                    End If
                Next

                If dsEstimation.Tables("IC3070201PaymentInfo").Rows.Count <> INT_CUSTOMERCOUNT_NEW Then

                    ' ■諸費用
                    ' 車両購入税取得
                    Dim drCarBuyTax = dsEstimation.Tables("IC3070201ChargeInfo").Select(STR_GETCARBUYTAX)
                    For Each drCarBuyTaxRow As DataRow In drCarBuyTax

                        If IsDBNull(drCarBuyTaxRow.Item("PRICE")) Then
                            Me.CarBuyTaxTextBox.Text = ""
                        Else
                            Me.CarBuyTaxTextBox.Text = drCarBuyTaxRow.Item("PRICE")
                            Me.carBuyTaxHiddenField.Value = drCarBuyTaxRow.Item("PRICE")
                        End If
                    Next

                    '登録費用行取得
                    Dim drRegCost As DataRow()
                    Dim drRegCostRow As DataRow
                    drRegCost = dsEstimation.Tables("IC3070201ChargeInfo").Select(STR_GETREGCOST)

                    For Each drRegCostRow In drRegCost

                        If IsDBNull(drRegCostRow.Item("PRICE")) Then
                            Me.regPriceTextBox.Text = "0"
                        Else
                            Me.regPriceTextBox.Text = drRegCostRow.Item("PRICE")
                            Me.regCostValueHiddenField.Value = drRegCostRow.Item("PRICE")
                        End If
                    Next

                    '諸費用区分の初期選択
                    If dsEstimation.Tables("IC3070201ChargeInfo").Rows.Count > 0 Then
                        chargeSegmentedButton.SelectedValue = dsEstimation.Tables("IC3070201ChargeInfo").Rows(0).Item("CHARGEDVS")
                    End If
                    '$99 Ken-Suzuki Add End

                    '■保険

                    '□保険区分
                    If String.Equals(dsEstimation.Tables("IC3070201EstInsuranceInfo").Rows(0).Item("INSUDVS"), STR_INSUDVS_JISYA) Then
                        Me.jisyaCheckMark.Value = StrTrue
                    Else
                        Me.tasyaCheckMark.Value = StrTrue
                    End If

                    '□保険会社
                    'javascriptにて初期表示
                    If IsDBNull(dsEstimation.Tables("IC3070201EstInsuranceInfo").Rows(0).Item("INSUCOMCD")) Then
                        Me.SelectInsuComCdHidden.Value = ""
                    Else
                        Me.SelectInsuComCdHidden.Value = dsEstimation.Tables("IC3070201EstInsuranceInfo").Rows(0).Item("INSUCOMCD")
                    End If

                    '□保険種別
                    'javascriptにて初期表示
                    If IsDBNull(dsEstimation.Tables("IC3070201EstInsuranceInfo").Rows(0).Item("INSUKIND")) Then
                        Me.SelectInsuKindCdHidden.Value = ""
                    Else
                        Me.SelectInsuKindCdHidden.Value = dsEstimation.Tables("IC3070201EstInsuranceInfo").Rows(0).Item("INSUKIND")
                    End If

                    '□年額
                    If IsDBNull(dsEstimation.Tables("IC3070201EstInsuranceInfo").Rows(0).Item("AMOUNT")) Then
                        Me.insuranceAmountTextBox.Text = ""
                    Else
                        Me.insuranceAmountTextBox.Text = dsEstimation.Tables("IC3070201EstInsuranceInfo").Rows(0).Item("AMOUNT")
                        Me.insuAmountValueHiddenField.Value = dsEstimation.Tables("IC3070201EstInsuranceInfo").Rows(0).Item("AMOUNT")
                    End If

                    ' ■お支払い方法
                    ' ■□現金
                    '現金行取得
                    Dim drCash As DataRow()
                    Dim drCashRow As DataRow
                    drCash = dsEstimation.Tables("IC3070201PaymentInfo").Select(STR_GETCASH)

                    For Each drCashRow In drCash
                        ' □頭金
                        If IsDBNull(drCashRow.Item("DEPOSIT")) Then
                            Me.cashDepositTextBox.Text = ""
                        Else
                            Me.cashDepositTextBox.Text = drCashRow.Item("DEPOSIT")
                            Me.cashDepositValueHiddenField.Value = drCashRow.Item("DEPOSIT")
                        End If

                        If drCashRow.Item("SELECTFLG").Equals(STR_SELECTFLG_SELECTED) Then
                            Me.payMethodHiddenField.Value = STR_PAYMETHOD_CASH.ToString
                            payMethodSegmentedButton.SelectedValue = STR_PAYMETHOD_CASH.ToString
                        End If

                    Next

                    ' ■□ローン

                    'ローン行取得
                    Dim drLoan As DataRow()
                    Dim drLoanRow As DataRow
                    drLoan = dsEstimation.Tables("IC3070201PaymentInfo").Select(STR_GETLOAN)

                    For Each drLoanRow In drLoan
                        ' □融資会社
                        If IsDBNull(drLoanRow.Item("FINANCECOMCODE")) Then
                            Me.SelectFinanceComHiddenField.Value = ""
                        Else
                            Me.SelectFinanceComHiddenField.Value = drLoanRow.Item("FINANCECOMCODE")
                        End If
                        ' □期間(月)
                        If IsDBNull(drLoanRow.Item("PAYMENTPERIOD")) Then
                            Me.loanPayPeriodNumericBox.Value = Nothing
                        Else
                            Me.loanPayPeriodNumericBox.Value = CDec(drLoanRow.Item("PAYMENTPERIOD"))
                        End If
                        ' □月額
                        If IsDBNull(drLoanRow.Item("MONTHLYPAYMENT")) Then
                            Me.loanMonthlyPayTextBox.Text = ""
                        Else
                            Me.loanMonthlyPayTextBox.Text = drLoanRow.Item("MONTHLYPAYMENT")
                            Me.loanMonthlyValueHiddenField.Value = drLoanRow.Item("MONTHLYPAYMENT")
                        End If
                        ' □頭金
                        If IsDBNull(drLoanRow.Item("DEPOSIT")) Then
                            Me.loanDepositTextBox.Text = ""
                        Else
                            Me.loanDepositTextBox.Text = drLoanRow.Item("DEPOSIT")
                            Me.loanDepositValueHiddenField.Value = drLoanRow.Item("DEPOSIT")
                        End If
                        ' □ボーナス
                        If IsDBNull(drLoanRow.Item("BONUSPAYMENT")) Then
                            Me.loanBonusPayTextBox.Text = ""
                        Else
                            Me.loanBonusPayTextBox.Text = drLoanRow.Item("BONUSPAYMENT")
                            Me.loanBonusValueHiddenField.Value = drLoanRow.Item("BONUSPAYMENT")
                        End If
                        ' □初回支払(日)
                        If IsDBNull(drLoanRow.Item("DUEDATE")) Then
                            Me.loanDueDateNumericBox.Value = Nothing
                        Else
                            Me.loanDueDateNumericBox.Value = CDec(drLoanRow.Item("DUEDATE"))
                        End If

                        If drLoanRow.Item("SELECTFLG").Equals(STR_SELECTFLG_SELECTED) Then
                            Me.payMethodHiddenField.Value = STR_PAYMETHOD_LOAN.ToString
                            payMethodSegmentedButton.SelectedValue = STR_PAYMETHOD_LOAN.ToString
                        End If

                        ' □利息
                        If IsDBNull(drLoanRow.Item("INTERESTRATE")) Then
                            Me.loanInterestrateTextBox.Text = ""
                        Else
                            Me.loanInterestrateTextBox.Text = drLoanRow.Item("INTERESTRATE")
                            Me.loanInterestrateValueHiddenField.Value = drLoanRow.Item("INTERESTRATE")
                        End If

                        If IsDBNull(drLoanRow.Item("FINANCECOMNAME")) Then
                            Me.selectFinanceComNmHiddenField.Value = ""
                        Else
                            Me.selectFinanceComNmHiddenField.Value = HttpUtility.HtmlEncode(drLoanRow.Item("FINANCECOMNAME"))
                        End If

                    Next

                    '契約済みの場合

                    '現金行取得
                    Dim drPayMethod As DataRow()
                    Dim drPayMethodRow As DataRow
                    drPayMethod = dsEstimation.Tables("IC3070201PaymentInfo").Select(STR_GETPAYMETHOD)

                    If drPayMethod.Count() = 1 Then

                        '契約実行後（キャンセル含む）フラグ
                        Me.contractAfterFlgHiddenField.Value = "1"

                        For Each drPayMethodRow In drPayMethod

                            If drPayMethodRow.Item("PAYMENTMETHOD") = STR_PAYMETHOD_CASH Then
                                Me.payMethodHiddenField.Value = "1"
                                payMethodSegmentedButton.SelectedValue = "1"
                            Else
                                Me.payMethodHiddenField.Value = "2"
                                payMethodSegmentedButton.SelectedValue = "2"
                            End If
                        Next

                        Me.payMethodSegmentedButton.Enabled = False
                    End If

                End If

                ' ■お支払い金額

                ' □値引き額
                If IsDBNull(dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("DISCOUNTPRICE")) Then
                    Me.discountPriceTextBox.Text = ""
                Else
                    Me.discountPriceTextBox.Text = dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("DISCOUNTPRICE")
                    Me.discountPriceValueHiddenField.Value = dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("DISCOUNTPRICE")
                End If
                ' □納車予定日
                If IsDBNull(dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("DELIDATE")) Then
                    Me.deliDateDateTimeSelector.Value = Nothing
                Else
                    Me.deliDateDateTimeSelector.Value = dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("DELIDATE")
                End If

                ' ■メモ
                If IsDBNull(dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("MEMO")) Then
                    Me.memoTextBox.Text = ""
                Else
                    Me.memoTextBox.Text = dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("MEMO")
                End If
            End If

            '■見積／契約者情報
            '敬称

            '' □所有者
            If Me.shoyusyaHojinCheckMark.Value = StrTrue Then
                Me.shoyusyaKeisyoMaeLabel.Style.Item("display") = "none"
                Me.shoyusyaKeisyoAtoLabel.Style.Item("display") = "none"
            End If
            ''□使用者
            If Me.shiyosyaHojinCheckMark.Value = StrTrue Then
                Me.shiyosyaKeisyoMaeLabel.Style.Item("display") = "none"
                Me.shiyosyaKeisyoAtoLabel.Style.Item("display") = "none"
            End If

            ' ■車両情報
            ' □車種

            'データソース設定
            vclInfoRepeater.DataSource = dsEstimation.Tables("IC3070201EstimationInfo")
            vclInfoRepeater.DataBind()

            Me.seriesNameHiddenField.Value = HttpUtility.HtmlEncode(CType(dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("SERIESNM"), String))
            Me.modelNameHiddenField.Value = HttpUtility.HtmlEncode(CType(dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("MODELNM"), String))

            Me.seriesCdHiddenField.Value = CType(dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("SERIESCD"), String)
            Me.modelCdHiddenField.Value = CType(dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("MODELCD"), String)
            Me.suffixCdHiddenField.Value = CType(dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("SUFFIXCD"), String)
            Me.extColorCdHiddenField.Value = CType(dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("EXTCOLORCD"), String)
            Me.modelNumberHiddenField.Value = CType(dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("MODELNUMBER"), String)

            '□外装追加費用
            If dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("EXTAMOUNT") <> 0 Then
                Me.extOptionFlgHiddenField.Value = "1"
                Me.extOptionPriceHiddenField.Value = dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("EXTAMOUNT")
            End If
            '□内装追加費用
            If dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("INTAMOUNT") <> 0 Then
                Me.intOptionFlgHiddenField.Value = "1"
                Me.intOptionPriceHiddenField.Value = dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("INTAMOUNT")
            End If

            '□メーカーオプション（TCV）
            If Not tcvMkrOptionDataTable Is Nothing Then
                tcvMkrOptionDataTable.Dispose()
            End If
            tcvMkrOptionDataTable = Me.CreateTcvOptionDataTable(dsEstimation.Tables("IC3070201VclOptionInfo"), _
                                                                STR_OPTIONPART_MAKER)
            mkrOptionRepeater.DataSource = tcvMkrOptionDataTable
            mkrOptionRepeater.DataBind()

            Me.mkrOptionCountHiddenField.Value = tcvMkrOptionDataTable.Rows.Count

            '□販売店オプション（TCV）
            If Not tcvDlrOptionDataTable Is Nothing Then
                tcvDlrOptionDataTable.Dispose()
            End If
            tcvDlrOptionDataTable = Me.CreateTcvOptionDataTable(dsEstimation.Tables("IC3070201VclOptionInfo"), _
                                                                STR_OPTIONPART_DEALER)
            dlrOptionRepeater.DataSource = tcvDlrOptionDataTable
            dlrOptionRepeater.DataBind()

            Me.tcvDlrOptionCountHiddenField.Value = tcvDlrOptionDataTable.Rows.Count

            '□販売店オプション（i-CROP）
            '販売店行取得
            Dim filterExp As String = String.Format(CultureInfo.InvariantCulture, "OptionPart = '{0}'", STR_OPTIONPART_DEALER_ICROP)
            Dim drDealer As DataRow() = dsEstimation.Tables("IC3070201VclOptionInfo").Select(filterExp)
            Me.dlrOptionDataTable = From n In dsEstimation.Tables("IC3070201VclOptionInfo")
                                    Where n("OptionPart") = STR_OPTIONPART_DEALER_ICROP

            Me.dlrOptionCountHiddenField.Value = drDealer.Count()

            '□車両画像

            '□車両画像
            If dsEstimateExtraData.Tables("SC3070205MODELPICTURE").Rows.Count = 0 Then
                Me.carImgFileHidden.Value = ""
            Else
                Me.carImgFileHidden.Value = ResolveClientUrl(dsEstimateExtraData.Tables("SC3070205MODELPICTURE").Rows(0).Item("IMAGEFILE"))
            End If

            '■諸費用
            Dim dcmExtOptPrice As Decimal
            dcmExtOptPrice = 0.0

            '□車両購入税
            If Not chargeInfoDataTable Is Nothing Then
                chargeInfoDataTable.Dispose()
            End If
            chargeInfoDataTable = Me.CreateChargeInfoDataTable(dsEstimation.Tables("IC3070201ChargeInfo"))
            chargeInfoDataTableRep.DataSource = chargeInfoDataTable
            chargeInfoDataTableRep.DataBind()

            ' ■お支払い金額

            ' □下取り車両
            If Not tradeInCarDataTable Is Nothing Then
                tradeInCarDataTable.Dispose()
            End If
            tradeInCarDataTable = CreateTradeInCarDataTable(dsEstimation.Tables("IC3070201TradeincarInfo"))
            tradeInCarDataTableRep.DataSource = tradeInCarDataTable
            tradeInCarDataTableRep.DataBind()

            '情報取得ボタン活性判定
            Dim dlrCd As String = Me.strDlrcdHiddenField.Value
            Dim strCd As String = Me.strStrCdHiddenField.Value

            Dim fuSeqNo As Decimal
            If Not String.IsNullOrEmpty(Me.lngFollowupBoxSeqNoHiddenField.Value) Then
                fuSeqNo = CType(Me.lngFollowupBoxSeqNoHiddenField.Value, Decimal)
            End If

            Dim estimateId As Long = CType(Me.lngEstimateIdHiddenField.Value, Long)

            '中古車査定情報取得
            Dim tradeInCarCheckDataTable As IC3070201DataSet.IC3070201TradeincarInfoDataTable = bizLogic.GetUcarAssessmentInfo(dlrCd, strCd, fuSeqNo, estimateId)
            If tradeInCarCheckDataTable.Rows.Count = 0 Then
                '中古車査定にデータがない場合、情報取得ボタン非表示
                Me.tradeInCarButton.Visible = False
            End If
            Me.tradeInCarButton.Text = WebWordUtility.GetWord(75)

            '敬称付き氏名
            If String.Equals(dtSysEnvSet.Rows(0).Item("NAMETITLEPOSITION"), STR_NAMETITLE_MAE) Then

                Me.cstEstNameHiddenField.Value = HttpUtility.HtmlEncode(Me.shoyusyaKeisyoMaeLabel.Text + " " + Me.shoyusyaNameTextBox.Text)
            ElseIf String.Equals(dtSysEnvSet.Rows(0).Item("NAMETITLEPOSITION"), STR_NAMETITLE_ATO) Then
                Me.cstEstNameHiddenField.Value = HttpUtility.HtmlEncode(Me.shoyusyaNameTextBox.Text + " " + Me.shoyusyaKeisyoAtoLabel.Text)

            End If

            'セッションにFollowBoxSeqNoをセット（通知一覧用）
            MyBase.SetValue(ScreenPos.Current, "SearchKey.FOLLOW_UP_BOX", Me.lngFollowupBoxSeqNoHiddenField.Value)

        End If

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DispInitData End")

    End Sub

    ''' <summary>
    ''' 通常モード時の処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub DefaultMode()

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DefaultMode Start")

        '活性化
        Me.memoTextBox.Enabled = True

        Me.popOver1.Visible = True

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DefaultMode End")

    End Sub

    ''' <summary>
    ''' 参照モード時の処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ReferenceMode()

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("ReferenceMode Start")

        '見積／契約者情報
        '■□所有者
        If String.IsNullOrEmpty(Me.strCRCustIdHiddenField.Value) Or OperationLocked = True Or Not Me.strApprovalModeHiddenField.Value.Equals(ModeApprovalManager) Then

            Me.shoyusyaNameLabel.Text = HttpUtility.HtmlEncode(Me.shoyusyaNameTextBox.Text)
        Else
            Me.shoyusyaNameLabel2.Text = HttpUtility.HtmlEncode(Me.shoyusyaNameTextBox.Text)

        End If

        Me.shoyusyaNameHiddenField.Value = HttpUtility.HtmlEncode(Me.shoyusyaNameTextBox.Text)

        '■□所有者
        Me.shoyusyaZipCodeLabel.Text = HttpUtility.HtmlEncode(Me.shoyusyaZipCodeTextBox.Text)
        Me.shoyusyaAddressLabel.Text = HttpUtility.HtmlEncode(Me.shoyusyaAddressTextBox.Text)
        Me.shoyusyaMobileLabel.Text = HttpUtility.HtmlEncode(Me.shoyusyaMobileTextBox.Text)
        Me.shoyusyaTelLabel.Text = HttpUtility.HtmlEncode(Me.shoyusyaTelTextBox.Text)
        Me.shoyusyaEmailLabel.Text = HttpUtility.HtmlEncode(Me.shoyusyaEmailTextBox.Text)
        Me.shoyusyaIDLabel.Text = HttpUtility.HtmlEncode(Me.shoyusyaIDTextBox.Text)

        '■□使用者
        Me.shiyosyaNameLabel.Text = HttpUtility.HtmlEncode(Me.shiyosyaNameTextBox.Text)
        Me.shiyosyaZipCodeLabel.Text = HttpUtility.HtmlEncode(Me.shiyosyaZipCodeTextBox.Text)
        Me.shiyosyaAddressLabel.Text = HttpUtility.HtmlEncode(Me.shiyosyaAddressTextBox.Text)
        Me.shiyosyaMobileLabel.Text = HttpUtility.HtmlEncode(Me.shiyosyaMobileTextBox.Text)
        Me.shiyosyaTelLabel.Text = HttpUtility.HtmlEncode(Me.shiyosyaTelTextBox.Text)
        Me.shiyosyaEmailLabel.Text = HttpUtility.HtmlEncode(Me.shiyosyaEmailTextBox.Text)
        Me.shiyosyaIDLabel.Text = HttpUtility.HtmlEncode(Me.shiyosyaIDTextBox.Text)


        '■諸費用
        Me.regPriceLabel.Text = Me.regCostValueHiddenField.Value


        '■車両登録税
        Me.CarBuyTaxCustomLabel.Text = Me.carBuyTaxHiddenField.Value


        '■保険

        Me.insuComLabel.Text = HttpUtility.HtmlEncode(Request.Form.Item("insuComSelect"))
        Me.insuComKindLabel.Text = HttpUtility.HtmlEncode(Request.Form.Item("insuComKindSelect"))



        Me.loanFinanceComLabel.Text = Me.selectFinanceComNmHiddenField.Value


        Me.insuranceAmountLabel.Text = Me.insuAmountValueHiddenField.Value

        '■お支払い方法
        '■□現金

        Me.cashDepositLabel.Text = Me.cashDepositValueHiddenField.Value

        '■□ローン
        If Me.loanPayPeriodNumericBox.Value Is Nothing Then
            Me.loanPayPeriodLabel.Text = ""
        Else
            Me.loanPayPeriodLabel.Text = Me.loanPayPeriodNumericBox.Value
        End If

        Me.loanMonthlyPayLabel.Text = Me.loanMonthlyValueHiddenField.Value
        Me.loanDepositLabel.Text = Me.loanDepositValueHiddenField.Value
        Me.loanBonusPayLabel.Text = Me.loanBonusValueHiddenField.Value
        If Me.loanDueDateNumericBox.Value Is Nothing Then
            Me.loanDueDateLabel.Text = ""
        Else
            Me.loanDueDateLabel.Text = Me.loanDueDateNumericBox.Value
        End If


        Me.loanInterestrateLabel.Text = Me.loanInterestrateValueHiddenField.Value


        '■お支払い金額
        '□値引き額

        Me.discountPriceLabel.Text = Me.discountPriceValueHiddenField.Value

        '□納車日
        If Not (Me.deliDateDateTimeSelector.Value Is Nothing) Then
            Me.deliDateLabel.Text = DateTimeFunc.FormatDate(3, Me.deliDateDateTimeSelector.Value)
            '入力変更検知用
            If Me.initialFlgHiddenField.Value.Length = 0 Then

                '日付変更チェックの日付フォーマットが誤っていたため、修正。
                Me.deliDateInitialValueHiddenField.Value = Me.deliDateDateTimeSelector.Value.Value.ToString("yyyy-MM-dd")
                Me.initialFlgHiddenField.Value = "1"
            End If
            '日付変更チェックの日付フォーマットが誤っていたため、修正。
            Me.deliDateAfterValueHiddenField.Value = Me.deliDateDateTimeSelector.Value.Value.ToString("yyyy-MM-dd")
        End If


        '非活性化

        Me.memoTextBox.Enabled = False

        Me.popOver1.Visible = False

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("ReferenceMode End")

    End Sub

    ''' <summary>
    ''' ポストバック時の値復元処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub PreservateData()

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("PreservateData Start")

        'TCVメーカーオプション欄復元用
        Using dtTcvMkrOption As New DataTable
            dtTcvMkrOption.Locale = Globalization.CultureInfo.InvariantCulture

            dtTcvMkrOption.Columns.Add("OPTIONPART")
            dtTcvMkrOption.Columns.Add("OPTIONNAME")
            dtTcvMkrOption.Columns.Add("PRICE", GetType(System.Double))
            dtTcvMkrOption.Columns.Add("INSTALLCOST", GetType(System.Double))

            Dim drTcvMkrOption As DataRow
            Dim intTcvMkrOptionCount As Integer
            For intTcvMkrOptionCount = 0 To Integer.Parse(Me.mkrOptionCountHiddenField.Value, Globalization.CultureInfo.CurrentCulture) - 1

                ' リピータ行
                Dim formRow As RepeaterItem = Me.mkrOptionRepeater.Items(intTcvMkrOptionCount)
                If Not IsNothing(formRow) Then
                    'ラベルを取得
                    Dim lblMkrOptionName As Web.Controls.CustomLabel
                    lblMkrOptionName = DirectCast(formRow.FindControl("mkrOptionNameLabelCustomLabel"), Web.Controls.CustomLabel)

                    ' コントロールのIDを取得
                    Dim mkrOptionPriceID As String = DirectCast(formRow.FindControl("mkrOptionPriceText"), TextBox).UniqueID
                    Dim mkrOptionPrice As String = Request.Form(mkrOptionPriceID)

                    drTcvMkrOption = dtTcvMkrOption.NewRow
                    drTcvMkrOption.Item("OPTIONPART") = STR_OPTIONPART_MAKER

                    'オプション名
                    If (lblMkrOptionName Is Nothing) OrElse (String.IsNullOrEmpty(lblMkrOptionName.Text)) Then
                        drTcvMkrOption.Item("OPTIONNAME") = String.Empty
                    Else
                        drTcvMkrOption.Item("OPTIONNAME") = lblMkrOptionName.Text
                    End If

                    'オプション価格
                    If String.IsNullOrEmpty(mkrOptionPrice) Then
                        drTcvMkrOption.Item("PRICE") = 0
                    Else
                        drTcvMkrOption.Item("PRICE") = CType(mkrOptionPrice, System.Double)
                    End If

                    drTcvMkrOption.Item("INSTALLCOST") = 0
                    dtTcvMkrOption.Rows.Add(drTcvMkrOption)
                End If

            Next

            tcvMkrOptionDataTable = dtTcvMkrOption
            mkrOptionRepeater.DataSource = dtTcvMkrOption
            mkrOptionRepeater.DataBind()

        End Using

        'TCV販売店オプション欄復元用
        Using dtTcvDlrOption As New DataTable
            dtTcvDlrOption.Locale = Globalization.CultureInfo.InvariantCulture

            dtTcvDlrOption.Columns.Add("OPTIONPART")
            dtTcvDlrOption.Columns.Add("OPTIONNAME")
            dtTcvDlrOption.Columns.Add("PRICE", GetType(System.Double))
            dtTcvDlrOption.Columns.Add("INSTALLCOST", GetType(System.Double))

            Dim drTcvDlrOption As DataRow
            Dim intTcvDlrOptionCount As Integer
            For intTcvDlrOptionCount = 0 To Integer.Parse(Me.tcvDlrOptionCountHiddenField.Value, Globalization.CultureInfo.CurrentCulture) - 1

                ' リピータ行
                Dim formRow As RepeaterItem = Me.dlrOptionRepeater.Items(intTcvDlrOptionCount)
                If Not IsNothing(formRow) Then
                    ' コントロールのIDを取得
                    'ラベルを取得
                    Dim lblDlrOptionName As Web.Controls.CustomLabel
                    lblDlrOptionName = DirectCast(formRow.FindControl("tcvDlrOptionNameCustomLabel"), Web.Controls.CustomLabel)

                    Dim dlrOptionPriceID As String = DirectCast(formRow.FindControl("tcvDlrOptionPriceText"), TextBox).UniqueID
                    Dim dlrOptionPrice As String = Request.Form(dlrOptionPriceID)
                    Dim dlrOptionInstallcostID As String = DirectCast(formRow.FindControl("tcvDlrOptionInstallCostText"), TextBox).UniqueID
                    Dim dlrOptionInstallcost As String = Request.Form(dlrOptionInstallcostID)

                    drTcvDlrOption = dtTcvDlrOption.NewRow
                    drTcvDlrOption.Item("OPTIONPART") = STR_OPTIONPART_DEALER

                    'オプション名
                    If (lblDlrOptionName Is Nothing) OrElse (String.IsNullOrEmpty(lblDlrOptionName.Text)) Then
                        drTcvDlrOption.Item("OPTIONNAME") = String.Empty
                    Else
                        drTcvDlrOption.Item("OPTIONNAME") = lblDlrOptionName.Text
                    End If

                    'オプション価格
                    If String.IsNullOrEmpty(dlrOptionPrice) Then
                        drTcvDlrOption.Item("PRICE") = 0
                    Else
                        drTcvDlrOption.Item("PRICE") = CType(dlrOptionPrice, System.Double)
                    End If

                    'オプション取付費用
                    If String.IsNullOrEmpty(dlrOptionPrice) Then
                        drTcvDlrOption.Item("INSTALLCOST") = 0
                    Else
                        drTcvDlrOption.Item("INSTALLCOST") = CType(dlrOptionInstallcost, System.Double)
                    End If

                    dtTcvDlrOption.Rows.Add(drTcvDlrOption)
                End If

            Next

            tcvDlrOptionDataTable = dtTcvDlrOption
            dlrOptionRepeater.DataSource = dtTcvDlrOption
            dlrOptionRepeater.DataBind()

        End Using


        '販売店オプション欄復元用
        Using dtDlrOption As New DataTable

            dtDlrOption.Locale = Globalization.CultureInfo.InvariantCulture


            dtDlrOption.Columns.Add("OPTIONNAME")
            dtDlrOption.Columns.Add("PRICE")
            dtDlrOption.Columns.Add("INSTALLCOST")

            Dim drDlrOption As DataRow
            Dim intCount As Integer

            For intCount = 1 To Integer.Parse(Me.dlrOptionCountHiddenField.Value, Globalization.CultureInfo.CurrentCulture)

                drDlrOption = dtDlrOption.NewRow
                drDlrOption.Item("OPTIONNAME") = Request.Form.Item(String.Concat("optionNameText", intCount))
                drDlrOption.Item("PRICE") = Request.Form.Item(String.Concat("optionPriceText", intCount))
                drDlrOption.Item("INSTALLCOST") = Request.Form.Item(String.Concat("optionMoneyText", intCount))
                dtDlrOption.Rows.Add(drDlrOption)

            Next

            dlrOptionDataTable = dtDlrOption.AsEnumerable()
        End Using


        '諸費用欄復元用
        Using dtChargeInfo As New DataTable
            dtChargeInfo.Locale = Globalization.CultureInfo.InvariantCulture

            dtChargeInfo.Columns.Add("ITEMCODE")
            dtChargeInfo.Columns.Add("ITEMNAME")
            dtChargeInfo.Columns.Add("PRICE")

            Dim drChargeInfo As DataRow
            Dim intChargeCount As Integer
            Dim intChargeIndex As Integer
            For intChargeCount = 1 To Integer.Parse(Me.chargeInfoCountHiddenField.Value, Globalization.CultureInfo.CurrentCulture)
                '連番は11から始まるのでインデックス算出
                intChargeIndex = intChargeCount + 10

                drChargeInfo = dtChargeInfo.NewRow
                drChargeInfo.Item("ITEMCODE") = intChargeIndex
                drChargeInfo.Item("ITEMNAME") = Request.Form.Item(String.Concat("chargeInfoText", intChargeIndex))
                drChargeInfo.Item("PRICE") = Request.Form.Item(String.Concat("chargeInfoPrice", intChargeIndex))
                dtChargeInfo.Rows.Add(drChargeInfo)

            Next

            'データ行数が10行未満だった場合のみ、空行を追加
            If dtChargeInfo.Rows.Count < 10 Then
                '空行を追加
                drChargeInfo = dtChargeInfo.NewRow
                drChargeInfo.Item("ITEMCODE") = dtChargeInfo.Rows.Count + 11
                drChargeInfo.Item("ITEMNAME") = String.Empty
                drChargeInfo.Item("PRICE") = String.Empty
                dtChargeInfo.Rows.Add(drChargeInfo)
            End If

            chargeInfoDataTable = dtChargeInfo
            chargeInfoDataTableRep.DataSource = dtChargeInfo
            chargeInfoDataTableRep.DataBind()
        End Using


        '下取り車両欄復元用
        Using dtTradeInCar As New DataTable

            dtTradeInCar.Locale = Globalization.CultureInfo.InvariantCulture


            dtTradeInCar.Columns.Add("NO")

            dtTradeInCar.Columns.Add("VEHICLENAME")
            dtTradeInCar.Columns.Add("ASSESSEDPRICE")

            Dim drTradeInCar As DataRow
            Dim intCarCount As Integer

            For intCarCount = 1 To Integer.Parse(Me.tradeInCarCountHiddenField.Value, Globalization.CultureInfo.CurrentCulture)
                drTradeInCar = dtTradeInCar.NewRow
                drTradeInCar.Item("NO") = intCarCount
                drTradeInCar.Item("VEHICLENAME") = Request.Form.Item(String.Concat("tradeInCarText", intCarCount))
                drTradeInCar.Item("ASSESSEDPRICE") = Request.Form.Item(String.Concat("tradeInCarPrice", intCarCount))
                dtTradeInCar.Rows.Add(drTradeInCar)
            Next

            drTradeInCar = dtTradeInCar.NewRow
            drTradeInCar.Item("NO") = dtTradeInCar.Rows.Count + 1
            drTradeInCar.Item("VEHICLENAME") = String.Empty
            drTradeInCar.Item("ASSESSEDPRICE") = String.Empty
            dtTradeInCar.Rows.Add(drTradeInCar)

            tradeInCarDataTable = dtTradeInCar
            tradeInCarDataTableRep.DataSource = tradeInCarDataTable
            tradeInCarDataTableRep.DataBind()

        End Using

        '金額欄復元用
        Me.regPriceTextBox.Text = Me.regCostValueHiddenField.Value
        Me.CarBuyTaxTextBox.Text = Me.carBuyTaxHiddenField.Value
        Me.insuranceAmountTextBox.Text = Me.insuAmountValueHiddenField.Value
        Me.cashDepositTextBox.Text = Me.cashDepositValueHiddenField.Value
        Me.loanMonthlyPayTextBox.Text = Me.loanMonthlyValueHiddenField.Value
        Me.loanDepositTextBox.Text = Me.loanDepositValueHiddenField.Value
        Me.loanBonusPayTextBox.Text = Me.loanBonusValueHiddenField.Value
        Me.loanInterestrateTextBox.Text = Me.loanInterestrateValueHiddenField.Value
        Me.discountPriceTextBox.Text = Me.discountPriceValueHiddenField.Value

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("PreservateData End")

    End Sub

    ''' <summary>
    ''' クライアント側で使用する文言の設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetClientMessage()

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SetClientMessage Start")

        Me.regPriceHiddenField.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(936))
        Me.shoyusyaNameMsgHiddenField.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(901))
        Me.shoyusyaZipcodeMsgHiddenField.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(902))
        Me.shoyusyaAddressMsgHiddenField.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(903))
        Me.shoyusyaIdMsgHiddenField.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(904))
        Me.shiyosyaNameMsgHiddenField.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(905))
        Me.shiyosyaZipcodeMsgHiddenField.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(906))
        Me.shiyosyaAddressMsgHiddenField.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(907))
        Me.shiyosyaIdMsgHiddenField.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(908))
        Me.minusLabelHiddenField.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(58))
        Me.optionPriceMsgHiddenField.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(924))
        Me.optionInstallFeeMsgHiddenField.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(925))
        Me.regFeeMsgHiddenField.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(956))
        Me.insuranceFeeMsgHiddenField.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(926))
        Me.cashDownMsgHiddenField.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(927))
        Me.loanMonthlyPayMsgHiddenField.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(929))
        Me.loanDownMsgHiddenField.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(930))
        Me.loanBonusMsgHiddenField.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(931))
        Me.discountMsgHiddenField.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(933))
        Me.tradeInPriceMsgHiddenField.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(955))
        Me.inputDataDeleteMsgHiddenField.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(935))
        Me.customerDeleteMsgHiddenField.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(937))
        Me.numericKeyPadCancelHiddenField.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(71))
        Me.numericKeyPadDoneHiddenField.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(72))
        Me.carBuyTaxFeeMsgHiddenField.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(986))
        Me.chargeInfoPriceMsgHiddenField.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(991))
        Me.loanInterestrateMsgHiddenField.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(992))
        Me.payTotalMsgHiddenField.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(993))

        '以下エラー用文言
        Me.errorWord901.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(901))
        Me.errorWord902.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(902))
        Me.errorWord903.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(903))
        Me.errorWord938.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(938))
        Me.errorWord904.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(904))
        Me.errorWord905.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(905))
        Me.errorWord906.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(906))
        Me.errorWord907.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(907))
        Me.errorWord939.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(939))
        Me.errorWord908.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(908))
        Me.errorWord936.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(936))
        Me.errorWord943.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(943))
        Me.errorWord944.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(944))
        Me.errorWord945.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(945))
        Me.errorWord946.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(946))
        Me.errorWord947.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(947))
        Me.errorWord948.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(948))
        Me.errorWord949.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(949))
        Me.errorWord950.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(950))
        Me.errorWord951.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(951))

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SetClientMessage End")

    End Sub

    ''' <summary>
    ''' サーバ側入力チェックを実施し、見積情報を保存する。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Private Sub saveEstimation_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles saveLinkButton.Click

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("saveEstimation_Click Start")

        '入力チェック（必須以外）
        If Not CheckInputFormat() Then
            Exit Sub
        End If

        '支払い総額のチェック
        If Not CheckTotalPrice() Then
            Exit Sub
        End If

        '見積登録情報用データセット
        Dim dsRegEstimation As IC3070202DataSet

        '見積情報登録用データセット作成
        dsRegEstimation = CreateEstimateDataSet()

        Dim bizLogic As SC3070205BusinessLogic      'ビジネスロジックオブジェクト
        Dim blnResult As IC3070202DataSet.IC3070202EstResultDataTable       '戻り値

        'ビジネスロジックオブジェクト作成
        bizLogic = New SC3070205BusinessLogic

        '見積情報登録
        blnResult = bizLogic.UpdateEstimation(dsRegEstimation)

        '保存済みフラグ
        Me.savedEstimationFlgHiddenField.Value = "1"

        '入力内容変更フラグ
        Me.blnInputChangedClientHiddenField.Value = False

        '初期化
        Me.deliDateInitialValueHiddenField.Value = Me.deliDateChangeValueHiddenField.Value
        Me.periodInitialValueHiddenField.Value = Me.periodChangeValueHiddenField.Value
        Me.firstPayInitialValueHiddenField.Value = Me.firstPayChangeValueHiddenField.Value
        Me.payMethodHiddenField.Value = Me.payMethodSegmentedButton.SelectedValue

        'CREATEDATE対応（STEP1.5以降に使用予定）
        Me.createDateHiddenField.Value = blnResult.Rows(0).Item("CreateDate")

        'オブジェクト開放
        bizLogic = Nothing

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("saveEstimation_Click End")

        Return

    End Sub

    ''' <summary>
    ''' サーバ側入力チェックを実施し、見積情報を保存する。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Private Sub DummySaveButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles DummySaveButton.Click

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DummySaveButton_Click Start")

        '入力チェック（必須以外）
        If Not CheckInputFormat() Then
            Exit Sub
        End If

        '支払い総額のチェック
        If Not CheckTotalPrice() Then
            Exit Sub
        End If

        '見積登録情報用データセット
        Dim dsRegEstimation As IC3070202DataSet

        '見積情報登録用データセット作成
        dsRegEstimation = CreateEstimateDataSet()

        Dim bizLogic As SC3070205BusinessLogic      'ビジネスロジックオブジェクト
        Dim blnResult As IC3070202DataSet.IC3070202EstResultDataTable       '戻り値

        'ビジネスロジックオブジェクト作成
        bizLogic = New SC3070205BusinessLogic

        '見積情報登録
        blnResult = bizLogic.UpdateEstimation(dsRegEstimation)

        '保存済みフラグ
        Me.savedEstimationFlgHiddenField.Value = "1"

        '入力内容変更フラグ
        Me.blnInputChangedClientHiddenField.Value = False

        '初期化
        Me.deliDateInitialValueHiddenField.Value = Me.deliDateChangeValueHiddenField.Value
        Me.periodInitialValueHiddenField.Value = Me.periodChangeValueHiddenField.Value
        Me.firstPayInitialValueHiddenField.Value = Me.firstPayChangeValueHiddenField.Value
        Me.payMethodHiddenField.Value = Me.payMethodSegmentedButton.SelectedValue

        'CREATEDATE対応（STEP1.5以降に使用予定）
        Me.createDateHiddenField.Value = blnResult.Rows(0).Item("CreateDate")

        'オブジェクト開放
        bizLogic = Nothing

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DummySaveButton_Click End")

        Return

    End Sub

    ''' <summary>
    ''' 入力チェックを実施する(必須)
    ''' </summary>
    ''' <remarks></remarks>
    Private Function CheckInputMandatory() As Boolean

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("CheckInputMandatory Start")

        '■見積／契約者情報
        '□所有者欄
        If String.IsNullOrEmpty(shoyusyaNameTextBox.Text) Then
            '氏名（所有者）が未入力の場合
            Me.actionModeHiddenField.Value = ""

            Me.mandatryCheckMsgHiddenField.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(901))

            Return False

        ElseIf String.IsNullOrEmpty(shoyusyaZipCodeTextBox.Text) Then
            '郵便番号（所有者）が未入力の場合
            Me.actionModeHiddenField.Value = ""

            Me.mandatryCheckMsgHiddenField.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(902))

            Return False

        ElseIf String.IsNullOrEmpty(shoyusyaAddressTextBox.Text) Then
            '住所（所有者）が未入力の場合
            Me.actionModeHiddenField.Value = ""

            Me.mandatryCheckMsgHiddenField.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(903))

            Return False

        ElseIf String.IsNullOrEmpty(shoyusyaMobileTextBox.Text) And String.IsNullOrEmpty(shoyusyaTelTextBox.Text) Then
            '携帯（所有者）、電話（所有者）いずれも未入力
            Me.actionModeHiddenField.Value = ""

            Me.mandatryCheckMsgHiddenField.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(938))

            Return False

        ElseIf String.IsNullOrEmpty(shoyusyaIDTextBox.Text) Then
            'ID（所有者）が未入力の場合
            Me.actionModeHiddenField.Value = ""

            Me.mandatryCheckMsgHiddenField.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(904))

            Return False

            '□使用者欄
        ElseIf String.IsNullOrEmpty(shiyosyaNameTextBox.Text) Then
            '氏名（使用者）が未入力の場合
            Me.actionModeHiddenField.Value = ""

            Me.mandatryCheckMsgHiddenField.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(905))

            Return False

        ElseIf String.IsNullOrEmpty(shiyosyaZipCodeTextBox.Text) Then
            '郵便番号（使用者）が未入力の場合
            Me.actionModeHiddenField.Value = ""

            Me.mandatryCheckMsgHiddenField.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(906))

            Return False

        ElseIf String.IsNullOrEmpty(shiyosyaAddressTextBox.Text) Then
            '住所（使用者）が未入力の場合
            Me.actionModeHiddenField.Value = ""

            Me.mandatryCheckMsgHiddenField.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(907))

            Return False

        ElseIf String.IsNullOrEmpty(shiyosyaMobileTextBox.Text) And String.IsNullOrEmpty(shiyosyaTelTextBox.Text) Then
            '携帯（使用者）、電話（使用者）いずれも未入力
            Me.actionModeHiddenField.Value = ""

            Me.mandatryCheckMsgHiddenField.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(939))

            Return False

        ElseIf String.IsNullOrEmpty(shiyosyaIDTextBox.Text) Then
            'ID（使用者）が未入力の場合
            Me.actionModeHiddenField.Value = ""

            Me.mandatryCheckMsgHiddenField.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(908))

            Return False


            '■諸費用欄
        ElseIf String.IsNullOrEmpty(regPriceTextBox.Text) Then
            '登録費用が未入力の場合
            Me.actionModeHiddenField.Value = ""
            Me.mandatryCheckMsgHiddenField.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(936))

            Return False

            '■保険欄
        ElseIf String.IsNullOrEmpty(Me.SelectInsuComCdHidden.Value) And (Not String.IsNullOrEmpty(insuAmountValueHiddenField.Value)) Then

            '保険金額が入力されており、保険会社が未選択の場合
            Me.actionModeHiddenField.Value = ""

            Me.mandatryCheckMsgHiddenField.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(943))

            Return False

        ElseIf String.IsNullOrEmpty(Me.SelectInsuKindCdHidden.Value) And (Not String.IsNullOrEmpty(Me.SelectInsuComCdHidden.Value)) Then

            '保険会社が選択されており、保険種別が未選択の場合
            Me.actionModeHiddenField.Value = ""

            Me.mandatryCheckMsgHiddenField.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(944))

            Return False

        ElseIf String.IsNullOrEmpty(insuAmountValueHiddenField.Value) And (Not String.IsNullOrEmpty(Me.SelectInsuComCdHidden.Value)) Then

            '保険会社が選択されており、保険金額が未入力の場合
            Me.actionModeHiddenField.Value = ""

            Me.mandatryCheckMsgHiddenField.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(945))

            Return False

            '■お支払い方法欄
            '□現金
        ElseIf String.IsNullOrEmpty(cashDepositValueHiddenField.Value) And (Me.payMethodSegmentedButton.SelectedItem.Value = 1) Then
            'お支払い方法に現金が選択されており、頭金（現金）が未入力の場合
            Me.actionModeHiddenField.Value = ""

            Me.mandatryCheckMsgHiddenField.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(946))

            Return False

            '□ローン

        ElseIf String.IsNullOrEmpty(Me.SelectFinanceComHiddenField.Value) And (Me.payMethodSegmentedButton.SelectedItem.Value = 2) Then

            'お支払い方法にローンが選択されており、融資会社が未選択の場合
            Me.actionModeHiddenField.Value = ""

            Me.mandatryCheckMsgHiddenField.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(947))

            Return False

        ElseIf Me.loanPayPeriodNumericBox.Value Is Nothing And (Me.payMethodSegmentedButton.SelectedItem.Value = 2) Then
            'お支払い方法にローンが選択されており、期間が未入力の場合
            Me.actionModeHiddenField.Value = ""

            Me.mandatryCheckMsgHiddenField.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(948))


            Return False

        ElseIf String.IsNullOrEmpty(Me.loanMonthlyValueHiddenField.Value) And (Me.payMethodSegmentedButton.SelectedItem.Value = 2) Then
            'お支払い方法にローンが選択されており、月額が未入力の場合
            Me.actionModeHiddenField.Value = ""

            Me.mandatryCheckMsgHiddenField.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(949))


            Return False

        ElseIf String.IsNullOrEmpty(Me.loanDepositValueHiddenField.Value) And (Me.payMethodSegmentedButton.SelectedItem.Value = 2) Then
            'お支払い方法にローンが選択されており、頭金（ローン）が未入力の場合
            Me.actionModeHiddenField.Value = ""

            Me.mandatryCheckMsgHiddenField.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(950))


            Return False

        ElseIf Me.loanDueDateNumericBox.Value Is Nothing And (Me.payMethodSegmentedButton.SelectedItem.Value = 2) Then
            'お支払い方法にローンが選択されており、初回支払いが未入力の場合
            Me.actionModeHiddenField.Value = ""

            Me.mandatryCheckMsgHiddenField.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(951))

            Return False

        End If

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("CheckInputMandatory End")

        Return True

    End Function

    ''' <summary>
    ''' 入力チェックを実施する(必須以外)
    ''' </summary>
    ''' <remarks></remarks>
    Private Function CheckInputFormat() As Boolean

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("CheckInputFormat Start")

        '■見積／契約者情報
        '□所有者欄
        If Not String.IsNullOrEmpty(shoyusyaNameTextBox.Text) AndAlso Not Validation.IsCorrectDigit(shoyusyaNameTextBox.Text, 256) Then
            '氏名（所有者）が256文字以上の場合
            Me.actionModeHiddenField.Value = ""
            MyBase.ShowMessageBox(909)

            Return False

        ElseIf Not Validation.IsValidString(shoyusyaNameTextBox.Text) Then
            '氏名（所有者）に禁則文字が含まれている場合
            Me.actionModeHiddenField.Value = ""
            MyBase.ShowMessageBox(957)
            Return False

        ElseIf Not String.IsNullOrEmpty(shoyusyaZipCodeTextBox.Text) AndAlso Not Validation.IsPostalCode(shoyusyaZipCodeTextBox.Text) Then
            '郵便番号（所有者）の書式が誤り
            Me.actionModeHiddenField.Value = ""
            MyBase.ShowMessageBox(910)

            Return False

        ElseIf Not Validation.IsValidString(shoyusyaZipCodeTextBox.Text) Then
            '郵便番号（所有者）に禁則文字が含まれている場合
            Me.actionModeHiddenField.Value = ""
            MyBase.ShowMessageBox(958)

            Return False

        ElseIf Not String.IsNullOrEmpty(shoyusyaAddressTextBox.Text) AndAlso Not Validation.IsCorrectDigit(shoyusyaAddressTextBox.Text, 320) Then
            '住所（所有者）が320文字以上の場合
            Me.actionModeHiddenField.Value = ""
            MyBase.ShowMessageBox(911)

            Return False

        ElseIf Not Validation.IsValidString(shoyusyaAddressTextBox.Text) Then
            '住所（所有者）に禁則文字が含まれている場合
            Me.actionModeHiddenField.Value = ""
            MyBase.ShowMessageBox(959)

            Return False

        ElseIf Not String.IsNullOrEmpty(shoyusyaMobileTextBox.Text) AndAlso Not Validation.IsMobilePhoneNumber(shoyusyaMobileTextBox.Text) Then
            '携帯（所有者）の書式が誤り 
            Me.actionModeHiddenField.Value = ""
            MyBase.ShowMessageBox(912)

            Return False

        ElseIf Not Validation.IsValidString(shoyusyaMobileTextBox.Text) Then
            '携帯（所有者）に禁則文字が含まれている場合
            Me.actionModeHiddenField.Value = ""
            MyBase.ShowMessageBox(960)

            Return False

        ElseIf Not String.IsNullOrEmpty(shoyusyaTelTextBox.Text) AndAlso Not Validation.IsPhoneNumber(shoyusyaTelTextBox.Text) Then
            '電話（所有者）の書式が誤り
            Me.actionModeHiddenField.Value = ""
            MyBase.ShowMessageBox(913)

            Return False

        ElseIf Not Validation.IsValidString(shoyusyaTelTextBox.Text) Then
            '電話（所有者）に禁則文字が含まれている場合
            Me.actionModeHiddenField.Value = ""
            MyBase.ShowMessageBox(961)

            Return False

        ElseIf Not String.IsNullOrEmpty(shoyusyaEmailTextBox.Text) AndAlso Not Validation.IsMail(shoyusyaEmailTextBox.Text) Then
            'E-Mail（所有者）の書式が誤り
            Me.actionModeHiddenField.Value = ""
            MyBase.ShowMessageBox(914)

            Return False

        ElseIf Not Validation.IsValidString(shoyusyaEmailTextBox.Text) Then
            'E-Mail（所有者）に禁則文字が含まれている場合
            Me.actionModeHiddenField.Value = ""
            MyBase.ShowMessageBox(962)

            Return False

        ElseIf Not String.IsNullOrEmpty(shoyusyaIDTextBox.Text) AndAlso Not Validation.IsCorrectDigit(shoyusyaIDTextBox.Text, 32) Then
            'ID（所有者）が32文字以上の場合
            Me.actionModeHiddenField.Value = ""
            MyBase.ShowMessageBox(915)

            Return False

        ElseIf Not Validation.IsValidString(shoyusyaIDTextBox.Text) Then
            'ID（所有者）に禁則文字が含まれている場合
            Me.actionModeHiddenField.Value = ""
            MyBase.ShowMessageBox(963)

            Return False

            '□使用者欄
        ElseIf Not String.IsNullOrEmpty(shiyosyaNameTextBox.Text) AndAlso Not Validation.IsCorrectDigit(shiyosyaNameTextBox.Text, 256) Then
            '氏名（使用者）が256文字以上の場合
            Me.actionModeHiddenField.Value = ""
            MyBase.ShowMessageBox(916)

            Return False

        ElseIf Not Validation.IsValidString(shiyosyaNameTextBox.Text) Then
            '氏名（使用者）に禁則文字が含まれている場合
            Me.actionModeHiddenField.Value = ""
            MyBase.ShowMessageBox(964)

            Return False

        ElseIf Not String.IsNullOrEmpty(shiyosyaZipCodeTextBox.Text) AndAlso Not Validation.IsPostalCode(shiyosyaZipCodeTextBox.Text) Then
            '郵便番号（使用者）の書式が誤り
            Me.actionModeHiddenField.Value = ""
            MyBase.ShowMessageBox(917)

            Return False

        ElseIf Not Validation.IsValidString(shiyosyaZipCodeTextBox.Text) Then
            '郵便番号（使用者）に禁則文字が含まれている場合
            Me.actionModeHiddenField.Value = ""
            MyBase.ShowMessageBox(965)

            Return False

        ElseIf Not String.IsNullOrEmpty(shiyosyaAddressTextBox.Text) AndAlso Not Validation.IsCorrectDigit(shiyosyaAddressTextBox.Text, 320) Then
            '住所（使用者）が320文字以上の場合
            Me.actionModeHiddenField.Value = ""
            MyBase.ShowMessageBox(918)

            Return False

        ElseIf Not Validation.IsValidString(shiyosyaAddressTextBox.Text) Then
            '住所（使用者）に禁則文字が含まれている場合
            Me.actionModeHiddenField.Value = ""
            MyBase.ShowMessageBox(966)

            Return False

        ElseIf Not String.IsNullOrEmpty(shiyosyaMobileTextBox.Text) AndAlso Not Validation.IsMobilePhoneNumber(shiyosyaMobileTextBox.Text) Then
            '携帯（使用者）の書式が誤り
            Me.actionModeHiddenField.Value = ""
            MyBase.ShowMessageBox(919)

            Return False

        ElseIf Not Validation.IsValidString(shiyosyaMobileTextBox.Text) Then
            '携帯（使用者）に禁則文字が含まれている場合
            Me.actionModeHiddenField.Value = ""
            MyBase.ShowMessageBox(967)

            Return False

        ElseIf Not String.IsNullOrEmpty(shiyosyaTelTextBox.Text) AndAlso Not Validation.IsPhoneNumber(shiyosyaTelTextBox.Text) Then
            '電話（使用者）の書式が誤り
            Me.actionModeHiddenField.Value = ""
            MyBase.ShowMessageBox(920)

            Return False

        ElseIf Not Validation.IsValidString(shiyosyaTelTextBox.Text) Then
            '電話（使用者）に禁則文字が含まれている場合
            Me.actionModeHiddenField.Value = ""
            MyBase.ShowMessageBox(968)

            Return False

        ElseIf Not String.IsNullOrEmpty(shiyosyaEmailTextBox.Text) AndAlso Not Validation.IsMail(shiyosyaEmailTextBox.Text) Then
            'E-Mail（使用者）の書式が誤り
            Me.actionModeHiddenField.Value = ""
            MyBase.ShowMessageBox(921)

            Return False

        ElseIf Not Validation.IsValidString(shiyosyaEmailTextBox.Text) Then
            'E-Mail（使用者）に禁則文字が含まれている場合
            Me.actionModeHiddenField.Value = ""
            MyBase.ShowMessageBox(969)

            Return False

        ElseIf Not String.IsNullOrEmpty(shiyosyaIDTextBox.Text) AndAlso Not Validation.IsCorrectDigit(shiyosyaIDTextBox.Text, 32) Then
            'ID（使用者）が32文字以上の場合
            Me.actionModeHiddenField.Value = ""
            MyBase.ShowMessageBox(922)

            Return False

        ElseIf Not Validation.IsValidString(shiyosyaIDTextBox.Text) Then
            'ID（使用者）に禁則文字が含まれている場合
            Me.actionModeHiddenField.Value = ""
            MyBase.ShowMessageBox(970)

            Return False
        End If


        '■車両情報欄
        '□販売店オプション
        For intCount = 1 To Integer.Parse(Me.dlrOptionCountHiddenField.Value, Globalization.CultureInfo.CurrentCulture)

            If String.IsNullOrEmpty(Request.Form.Item(String.Concat("optionNameText", intCount))) Then
                'オプション名が未入力の場合
                Me.actionModeHiddenField.Value = ""
                MyBase.ShowMessageBox(940)
                Return False

            ElseIf Not Validation.IsCorrectDigit(Request.Form.Item(String.Concat("optionNameText", intCount)), 64) Then
                'オプション名が64桁以上の場合
                Me.actionModeHiddenField.Value = ""
                MyBase.ShowMessageBox(923)
                Return False

            ElseIf Not Validation.IsValidString(Request.Form.Item(String.Concat("optionNameText", intCount))) Then
                'オプション名に禁則文字が含まれている場合
                Me.actionModeHiddenField.Value = ""
                MyBase.ShowMessageBox(971)

                Return False

            ElseIf String.IsNullOrEmpty(Request.Form.Item(String.Concat("optionPriceText", intCount))) Then
                '価格が未入力の場合
                Me.actionModeHiddenField.Value = ""
                MyBase.ShowMessageBox(941)
                Return False


                'ElseIf Not Validation.IsCorrectPattern(Request.Form.Item(String.Concat("optionPriceText", intCount)), STR_MONEYFORMAT) And Not String.IsNullOrEmpty(Request.Form.Item(String.Concat("optionPriceText", intCount))) Then
            ElseIf Not Validation.IsCorrectPattern(Request.Form.Item(String.Concat("optionPriceText", intCount)), STR_MONEYFORMAT_MINUS) And Not String.IsNullOrEmpty(Request.Form.Item(String.Concat("optionPriceText", intCount))) Then


                '価格の書式が誤り
                Me.actionModeHiddenField.Value = ""
                '（整数9桁以内、小数点以下2桁以外の場合）
                MyBase.ShowMessageBox(924)
                Return False

            ElseIf String.IsNullOrEmpty(Request.Form.Item(String.Concat("optionMoneyText", intCount))) Then
                '取付額が未入力の場合
                Me.actionModeHiddenField.Value = ""
                MyBase.ShowMessageBox(942)
                Return False

            ElseIf Not Validation.IsCorrectPattern(Request.Form.Item(String.Concat("optionMoneyText", intCount)), STR_MONEYFORMAT) And Not String.IsNullOrEmpty(Request.Form.Item(String.Concat("optionMoneyText", intCount))) Then
                '取付額の書式が誤り
                '（整数9桁以内、小数点以下2桁以外の場合）
                Me.actionModeHiddenField.Value = ""
                MyBase.ShowMessageBox(925)
                Return False

            End If

        Next


        '■諸費用欄(車両購入税)
        If Not Validation.IsCorrectPattern(CarBuyTaxTextBox.Text, STR_MONEYFORMAT) And Not String.IsNullOrEmpty(CarBuyTaxTextBox.Text) Then
            '車両購入税の書式が誤り
            '（整数9桁以内、小数点以下2桁以外の場合）
            Me.actionModeHiddenField.Value = ""
            MyBase.ShowMessageBox(986)

            Return False
        End If


        '■諸費用欄(登録費用)
        If Not Validation.IsCorrectPattern(regPriceTextBox.Text, STR_MONEYFORMAT) And Not String.IsNullOrEmpty(regPriceTextBox.Text) Then
            '登録費用の書式が誤り
            '（整数9桁以内、小数点以下2桁以外の場合）
            Me.actionModeHiddenField.Value = ""
            MyBase.ShowMessageBox(956)

            Return False
        End If


        Dim intChargeIndex As Integer
        '■諸費用欄(手入力)
        For intChargeCount = 1 To Integer.Parse(Me.chargeInfoCountHiddenField.Value, Globalization.CultureInfo.CurrentCulture)
            '連番は11から始まるのでインデックス算出
            intChargeIndex = intChargeCount + 10

            If String.IsNullOrEmpty(Request.Form.Item(String.Concat("chargeInfoText", intChargeIndex))) Then
                '手入力諸費用項目が未入力の場合
                Me.actionModeHiddenField.Value = ""
                MyBase.ShowMessageBox(987)
                Return False

            ElseIf Not Validation.IsCorrectDigit(Request.Form.Item(String.Concat("chargeInfoText", intChargeIndex)), 64) And Not String.IsNullOrEmpty(Request.Form.Item(String.Concat("chargeInfoText", intChargeIndex))) Then
                '手入力諸費用項目が64桁以上の場合
                Me.actionModeHiddenField.Value = ""
                MyBase.ShowMessageBox(988)
                Return False

            ElseIf Not Validation.IsValidString(Request.Form.Item(String.Concat("chargeInfoText", intChargeIndex))) Then
                '手入力諸費用項目に禁則文字が含まれている場合
                Me.actionModeHiddenField.Value = ""
                MyBase.ShowMessageBox(989)

                Return False

            ElseIf String.IsNullOrEmpty(Request.Form.Item(String.Concat("chargeInfoPrice", intChargeIndex))) Then
                '手入力諸費用金額が未入力の場合
                Me.actionModeHiddenField.Value = ""
                MyBase.ShowMessageBox(990)
                Return False

            ElseIf Not Validation.IsCorrectPattern(Request.Form.Item(String.Concat("chargeInfoPrice", intChargeIndex)), STR_MONEYFORMAT_MINUS) And Not String.IsNullOrEmpty(Request.Form.Item(String.Concat("chargeInfoPrice", intChargeIndex))) Then
                '手入力諸費用金額の書式が誤り
                '（整数9桁以内、小数点以下2桁以外の場合）
                Me.actionModeHiddenField.Value = ""
                MyBase.ShowMessageBox(991)
                Return False

            End If

        Next


        '■保険欄
        If Not Validation.IsCorrectPattern(insuranceAmountTextBox.Text, STR_MONEYFORMAT) And Not String.IsNullOrEmpty(insuranceAmountTextBox.Text) Then
            '保険年額の書式が誤り
            '（整数9桁以内、小数点以下2桁以外の場合）
            Me.actionModeHiddenField.Value = ""
            MyBase.ShowMessageBox(926)

            Return False

            '■お支払い方法欄
            '□現金
        ElseIf Not Validation.IsCorrectPattern(cashDepositTextBox.Text, STR_MONEYFORMAT) And Not String.IsNullOrEmpty(cashDepositTextBox.Text) Then
            '頭金（現金）の書式が誤り
            '（整数9桁以内、小数点以下2桁以外の場合）
            Me.actionModeHiddenField.Value = ""
            MyBase.ShowMessageBox(927)

            Return False

            '□ローン
        ElseIf Not (loanPayPeriodNumericBox.Value Is Nothing) AndAlso Not (Validation.IsHankakuNumber(loanPayPeriodNumericBox.Value)) Then
            '期間が数値でない場合
            Me.actionModeHiddenField.Value = ""
            MyBase.ShowMessageBox(928)

            Return False

        ElseIf Not (loanPayPeriodNumericBox.Value Is Nothing) AndAlso Not (Validation.IsCorrectDigit(loanPayPeriodNumericBox.Value, 3)) Then
            '期間が3桁以上の場合
            Me.actionModeHiddenField.Value = ""
            MyBase.ShowMessageBox(928)

            Return False

        ElseIf Not Validation.IsCorrectPattern(loanMonthlyPayTextBox.Text, STR_MONEYFORMAT) And Not String.IsNullOrEmpty(loanMonthlyPayTextBox.Text) Then
            '月額の書式が誤り
            '（整数9桁以内、小数点以下2桁以外の場合）
            Me.actionModeHiddenField.Value = ""
            MyBase.ShowMessageBox(929)

            Return False

        ElseIf Not Validation.IsCorrectPattern(loanDepositTextBox.Text, STR_MONEYFORMAT) And Not String.IsNullOrEmpty(loanDepositTextBox.Text) Then
            '頭金（ローン）の書式が誤り
            '（整数9桁以内、小数点以下2桁以外の場合）
            Me.actionModeHiddenField.Value = ""
            MyBase.ShowMessageBox(930)

            Return False

        ElseIf Not Validation.IsCorrectPattern(loanBonusPayTextBox.Text, STR_MONEYFORMAT) And Not String.IsNullOrEmpty(loanBonusPayTextBox.Text) Then
            'ボーナスの書式が誤り
            '（整数9桁以内、小数点以下2桁以外の場合）
            Me.actionModeHiddenField.Value = ""
            MyBase.ShowMessageBox(931)

            Return False

        ElseIf Not (loanDueDateNumericBox.Value Is Nothing) AndAlso Not (Validation.IsHankakuNumber(loanDueDateNumericBox.Value)) Then
            '初回支払いが数値でない場合
            Me.actionModeHiddenField.Value = ""
            MyBase.ShowMessageBox(932)

            Return False

        ElseIf Not (loanDueDateNumericBox.Value Is Nothing) AndAlso Not (Validation.IsCorrectDigit(loanDueDateNumericBox.Value, 3)) Then
            '初回支払いが3桁以上の場合
            Me.actionModeHiddenField.Value = ""
            MyBase.ShowMessageBox(932)

            Return False


        ElseIf Not Validation.IsCorrectPattern(loanInterestrateTextBox.Text, STR_INTERESTRATE_FORMAT) And Not String.IsNullOrEmpty(loanInterestrateTextBox.Text) Then
            '利息の書式が誤り
            '（整数4桁以上、小数点以下4桁以上の場合）
            Me.actionModeHiddenField.Value = ""
            MyBase.ShowMessageBox(992)

            Return False

        End If

        '□下取り車両
        For intCarCount = 1 To Integer.Parse(Me.tradeInCarCountHiddenField.Value, Globalization.CultureInfo.CurrentCulture)

            If String.IsNullOrEmpty(Request.Form.Item(String.Concat("tradeInCarText", intCarCount))) Then
                '車名が未入力の場合
                Me.actionModeHiddenField.Value = ""
                MyBase.ShowMessageBox(952)
                Return False

            ElseIf Not Validation.IsCorrectDigit(Request.Form.Item(String.Concat("tradeInCarText", intCarCount)), 128) And Not String.IsNullOrEmpty(Request.Form.Item(String.Concat("tradeInCarText", intCarCount))) Then
                '車名が128桁以上の場合
                Me.actionModeHiddenField.Value = ""
                MyBase.ShowMessageBox(954)
                Return False

            ElseIf Not Validation.IsValidString(Request.Form.Item(String.Concat("tradeInCarText", intCarCount))) Then
                '車名に禁則文字が含まれている場合
                Me.actionModeHiddenField.Value = ""
                MyBase.ShowMessageBox(972)

                Return False

            ElseIf String.IsNullOrEmpty(Request.Form.Item(String.Concat("tradeInCarPrice", intCarCount))) Then
                '価格が未入力の場合
                Me.actionModeHiddenField.Value = ""
                MyBase.ShowMessageBox(953)
                Return False

            ElseIf Not Validation.IsCorrectPattern(Request.Form.Item(String.Concat("tradeInCarPrice", intCarCount)), STR_MONEYFORMAT) And Not String.IsNullOrEmpty(Request.Form.Item(String.Concat("tradeInCarPrice", intCarCount))) Then
                '価格の書式が誤り
                '（整数9桁以内、小数点以下2桁以外の場合）
                Me.actionModeHiddenField.Value = ""
                MyBase.ShowMessageBox(955)
                Return False

            End If

        Next

        'If Not Validation.IsCorrectPattern(discountPriceTextBox.Text, "^[0-9]{1,9}(\.[0-9]{1,2})?$") And Not String.IsNullOrEmpty(discountPriceTextBox.Text) Then
        If Not Validation.IsCorrectPattern(discountPriceTextBox.Text, STR_MONEYFORMAT) And Not String.IsNullOrEmpty(discountPriceTextBox.Text) Then
            '値引き額が数値でない場合
            Me.actionModeHiddenField.Value = ""
            MyBase.ShowMessageBox(933)

            Return False

        ElseIf Not Validation.IsCorrectDigit(memoTextBox.Text, Integer.Parse(memoMaxHiddenField.Value, Globalization.CultureInfo.CurrentCulture)) And Not String.IsNullOrEmpty(memoTextBox.Text) Then
            'メモがX桁以上の場合(tbl_systemenvsettingにて設定)
            Me.actionModeHiddenField.Value = ""
            MyBase.ShowMessageBox(934, memoMaxHiddenField.Value)

            Return False

        ElseIf (Validation.IsValidString(memoTextBox.Text) = False) And Not String.IsNullOrEmpty(memoTextBox.Text) Then
            'メモに禁則文字が含まれている場合
            Me.actionModeHiddenField.Value = ""
            MyBase.ShowMessageBox(973)

            Return False

        End If

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("CheckInputFormat End")

        Return True

    End Function

    ''' <summary>
    ''' 支払い総額のチェックを実施する
    ''' </summary>
    ''' <returns>True:エラーなし、False:エラーあり</returns>
    ''' <remarks></remarks>
    Private Function CheckTotalPrice() As Boolean

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("CheckTotalPrice Start")

        Dim dblPayTotal As Double = 0           '支払い総額

        If Not String.IsNullOrEmpty(Me.payTotalHiddenField.Value) Then
            dblPayTotal = CType(Me.payTotalHiddenField.Value, Double)
        End If

        '支払い総額が0未満の場合は、エラー
        If dblPayTotal < 0 Then
            Me.actionModeHiddenField.Value = ""
            MyBase.ShowMessageBox(993)

            Return False
        End If

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("CheckTotalPrice End")

        Return True
    End Function

    ''' <summary>
    ''' 見積情報登録用データセットを作成する。
    ''' </summary>
    ''' <remarks></remarks>
    ''' <History>
    '''  2012/04/24 TCS 鈴木(健) HTMLエンコード対応
    '''  2012/12/17 TCS 神本     GTMC121218107対応
    '''  2013/01/18 TCS 上田     GL0871対応
    ''' </History>
    Private Function CreateEstimateDataSet() As IC3070202DataSet

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("CreateEstimateDataSet Start")

        'ログイン情報
        Dim staffInfo As StaffContext
        Dim strBrnCd As String          '店舗コード
        Dim strAccount As String        'アカウント

        'ログインスタッフ情報取得
        staffInfo = StaffContext.Current
        strBrnCd = staffInfo.BrnCD
        strAccount = staffInfo.Account

        '登録見積情報データセット作成
        Dim dsRegEstimation As New IC3070202DataSet     '見積登録情報
        Dim dtEstimationInfo = dsRegEstimation.IC3070202EstimationInfo           '見積情報データテーブル
        Dim dtCustomerInfo = dsRegEstimation.IC3070202EstCustomerInfo           '見積顧客情報データテーブル
        Dim dtVclOptionInfo = dsRegEstimation.IC3070202EstVclOptionInfo        '見積車両オプション情報データテーブル
        Dim dtChargeInfo = dsRegEstimation.IC3070202EstChargeInfo              '見積諸費用情報データテーブル
        Dim dtPayInfo = dsRegEstimation.IC3070202EstPaymentInfo                    '見積支払方法情報データテーブル
        Dim dtTradeInCarInfo = dsRegEstimation.IC3070202EstTradeInCarInfo       '見積下取車両情報データテーブル
        Dim dtInsuranceInfo = dsRegEstimation.IC3070202EstInsuranceInfo       '見積保険情報データテーブル

        '後でコメントアウト解除
        'Dim dtCustomer = dsRegEstimation.IC3070203Customer
        'Dim dtCustomerDlr = dsRegEstimation.IC3070203CustomerDlr
        'Dim dtCstContactTimeslot = dsRegEstimation.IC3070203CstContactTimeslot

        Dim dsEstimation As IC3070201DataSet        '見積取得情報
        dsEstimation = CType(ViewState("DataSetEstimation"), IC3070201DataSet)

        dtEstimationInfo.Merge(dsEstimation.Tables("IC3070201EstimationInfo"))
        dtCustomerInfo.Merge(dsEstimation.Tables("IC3070201CustomerInfo"))
        dtVclOptionInfo.Merge(dsEstimation.Tables("IC3070201VclOptionInfo"))
        dtChargeInfo.Merge(dsEstimation.Tables("IC3070201ChargeInfo"))
        dtPayInfo.Merge(dsEstimation.Tables("IC3070201PaymentInfo"))
        dtTradeInCarInfo.Merge(dsEstimation.Tables("IC3070201TradeincarInfo"))
        dtInsuranceInfo.Merge(dsEstimation.Tables("IC3070201EstInsuranceInfo"))

        '後でコメントアウト解除
        'dtCustomer.Merge(dsEstimation.IC3070203Customer)
        'dtCustomerDlr.Merge(dsEstimation.IC3070203CustomerDlr)
        'dtCstContactTimeslot.Merge(dsEstimation.IC3070203CstContactTimeslot)

        '■見積情報データテーブル
        '納車予定日
        If String.IsNullOrEmpty(Me.deliDateChangeValueHiddenField.Value) Then
            dtEstimationInfo.Rows(0).Item("DeliDate") = DBNull.Value
        Else
            dtEstimationInfo.Rows(0).Item("DeliDate") = Me.deliDateChangeValueHiddenField.Value
        End If

        '値引き額
        If String.IsNullOrEmpty(Me.discountPriceValueHiddenField.Value) Then
            dtEstimationInfo.Rows(0).Item("DiscountPrice") = DBNull.Value
        Else
            dtEstimationInfo.Rows(0).Item("DiscountPrice") = Double.Parse(Me.discountPriceValueHiddenField.Value, Globalization.CultureInfo.CurrentCulture)
        End If

        'メモ
        dtEstimationInfo.Rows(0).Item("Memo") = Me.memoTextBox.Text

        '更新ユーザアカウント
        dtEstimationInfo.Rows(0).Item("UpdateAccount") = strAccount

        '更新機能ID
        dtEstimationInfo.Rows(0).Item("UpdateId") = STR_DISPID_QUOTATION


        If dsEstimation.Tables("IC3070201CustomerInfo").Rows.Count = INT_CUSTOMERCOUNT_NEW Then
            '見積新規作成時

            '■見積顧客情報データテーブル
            '所有者データレコード作成
            Dim drRegCustomerSyoyusya As IC3070202DataSet.IC3070202EstCustomerInfoRow
            drRegCustomerSyoyusya = dtCustomerInfo.NewRow

            '見積管理ID
            drRegCustomerSyoyusya.ESTIMATEID = Long.Parse(Me.lngEstimateIdHiddenField.Value, Globalization.CultureInfo.CurrentCulture)

            '契約顧客種別
            drRegCustomerSyoyusya.CONTRACTCUSTTYPE = STR_CONTCUSTTYPE_SYOYUSYA

            '顧客区分
            If Me.shoyusyaKojinCheckMark.Value = StrTrue Then
                drRegCustomerSyoyusya.CUSTPART = STR_CUSTPART_KOJIN
            Else
                drRegCustomerSyoyusya.CUSTPART = STR_CUSTPART_HOJIN
            End If

            '氏名
            drRegCustomerSyoyusya.NAME = Me.shoyusyaNameTextBox.Text

            '国民番号
            drRegCustomerSyoyusya.SOCIALID = Me.shoyusyaIDTextBox.Text

            '郵便番号
            drRegCustomerSyoyusya.ZIPCODE = Me.shoyusyaZipCodeTextBox.Text

            '住所
            drRegCustomerSyoyusya.ADDRESS = Me.shoyusyaAddressTextBox.Text

            '電話番号
            drRegCustomerSyoyusya.TELNO = Me.shoyusyaTelTextBox.Text

            '携帯電話番号
            drRegCustomerSyoyusya.MOBILE = Me.shoyusyaMobileTextBox.Text

            'e-MAILアドレス
            drRegCustomerSyoyusya.EMAIL = Me.shoyusyaEmailTextBox.Text

            '所有者データレコード追加
            dtCustomerInfo.Rows.Add(drRegCustomerSyoyusya)

            'データレコード開放
            drRegCustomerSyoyusya = Nothing

            '使用者データレコード作成
            Dim drRegCustomerShiyosya As IC3070202DataSet.IC3070202EstCustomerInfoRow
            drRegCustomerShiyosya = dtCustomerInfo.NewRow

            '見積管理ID
            drRegCustomerShiyosya.ESTIMATEID = Long.Parse(Me.lngEstimateIdHiddenField.Value, Globalization.CultureInfo.CurrentCulture)

            '契約顧客種別
            drRegCustomerShiyosya.CONTRACTCUSTTYPE = STR_CONTCUSTTYPE_SHIYOSYA

            '顧客区分

            If Me.shiyosyaKojinCheckMark.Value = StrTrue Then
                drRegCustomerShiyosya.CUSTPART = STR_CUSTPART_KOJIN
            Else
                drRegCustomerShiyosya.CUSTPART = STR_CUSTPART_HOJIN
            End If

            '氏名
            drRegCustomerShiyosya.NAME = Me.shiyosyaNameTextBox.Text

            '国民番号
            drRegCustomerShiyosya.SOCIALID = Me.shiyosyaIDTextBox.Text

            '郵便番号
            drRegCustomerShiyosya.ZIPCODE = Me.shiyosyaZipCodeTextBox.Text

            '住所
            drRegCustomerShiyosya.ADDRESS = Me.shiyosyaAddressTextBox.Text

            '電話番号
            drRegCustomerShiyosya.TELNO = Me.shiyosyaTelTextBox.Text

            '携帯電話番号
            drRegCustomerShiyosya.MOBILE = Me.shiyosyaMobileTextBox.Text

            'e-MAILアドレス
            drRegCustomerShiyosya.EMAIL = Me.shiyosyaEmailTextBox.Text

            '使用者データレコード追加
            dtCustomerInfo.Rows.Add(drRegCustomerShiyosya)

            'データレコード開放
            drRegCustomerShiyosya = Nothing

            '■見積保険情報データテーブル

            Dim drRegInsuranceInfo As IC3070202DataSet.IC3070202EstInsuranceInfoRow
            drRegInsuranceInfo = dtInsuranceInfo.NewRow

            '見積管理ID
            drRegInsuranceInfo.ESTIMATEID = Long.Parse(Me.lngEstimateIdHiddenField.Value, Globalization.CultureInfo.CurrentCulture)

            '保険区分(1:自社、2:他社)
            If Me.jisyaCheckMark.Value = StrTrue Then
                drRegInsuranceInfo.INSUDVS = STR_INSUDVS_JISYA
            Else
                drRegInsuranceInfo.INSUDVS = STR_INSUDVS_TASYA
            End If

            '保険会社コード
            drRegInsuranceInfo.INSUCOMCD = Me.SelectInsuComCdHidden.Value

            '保険種別
            drRegInsuranceInfo.INSUKIND = Me.SelectInsuKindCdHidden.Value

            '保険金額
            If Not String.IsNullOrEmpty(Me.insuAmountValueHiddenField.Value) Then
                drRegInsuranceInfo.AMOUNT = Double.Parse(Me.insuAmountValueHiddenField.Value, Globalization.CultureInfo.CurrentCulture)
            End If

            'データレコード追加
            dtInsuranceInfo.Rows.Add(drRegInsuranceInfo)

            'データレコード開放
            drRegInsuranceInfo = Nothing

            '■見積支払方法情報データテーブル
            '□現金
            '現金データレコード作成
            Dim drRegPayInfoCash As IC3070202DataSet.IC3070202EstPaymentInfoRow
            drRegPayInfoCash = dtPayInfo.NewRow

            '見積管理ID
            drRegPayInfoCash.ESTIMATEID = CType(Me.lngEstimateIdHiddenField.Value, Long)

            '支払方法区分
            drRegPayInfoCash.PAYMENTMETHOD = STR_PAYMETHOD_CASH

            '頭金
            If Not String.IsNullOrEmpty(Me.cashDepositValueHiddenField.Value) Then
                drRegPayInfoCash.DEPOSIT = Double.Parse(Me.cashDepositValueHiddenField.Value, Globalization.CultureInfo.CurrentCulture)
            End If

            '削除フラグ
            drRegPayInfoCash.DELFLG = STR_DELETEFLG_NOT

            '選択フラグ
            If Me.payMethodSegmentedButton.SelectedValue.Equals(STR_PAYMETHOD_CASH.ToString) Then
                drRegPayInfoCash.SELECTFLG = STR_SELECTFLG_SELECTED
            Else
                drRegPayInfoCash.SELECTFLG = STR_SELECTFLG_NOT
            End If

            '現金データレコード追加
            dtPayInfo.Rows.Add(drRegPayInfoCash)

            'データレコード開放
            drRegPayInfoCash = Nothing

            '□ローン
            'ローンデータレコード作成
            Dim drRegPayInfoLoan As IC3070202DataSet.IC3070202EstPaymentInfoRow
            drRegPayInfoLoan = dtPayInfo.NewRow()

            '見積管理ID
            drRegPayInfoLoan.ESTIMATEID = Long.Parse(Me.lngEstimateIdHiddenField.Value, Globalization.CultureInfo.CurrentCulture)

            '支払方法区分
            drRegPayInfoLoan.PAYMENTMETHOD = STR_PAYMETHOD_LOAN

            '融資会社コード
            drRegPayInfoLoan.FINANCECOMCODE = Me.SelectFinanceComHiddenField.Value

            '支払期間
            If Not (loanPayPeriodNumericBox.Value Is Nothing) Then
                drRegPayInfoLoan.PAYMENTPERIOD = Integer.Parse(Me.loanPayPeriodNumericBox.Value, Globalization.CultureInfo.CurrentCulture)
            End If

            '毎月返済額
            If Not String.IsNullOrEmpty(Me.loanMonthlyValueHiddenField.Value) Then
                drRegPayInfoLoan.MONTHLYPAYMENT = Double.Parse(Me.loanMonthlyValueHiddenField.Value, Globalization.CultureInfo.CurrentCulture)
            End If

            '頭金
            If Not String.IsNullOrEmpty(Me.loanDepositValueHiddenField.Value) Then
                drRegPayInfoLoan.DEPOSIT = Double.Parse(Me.loanDepositValueHiddenField.Value, Globalization.CultureInfo.CurrentCulture)
            End If

            'ボーナス時返済額
            If Not String.IsNullOrEmpty(Me.loanBonusValueHiddenField.Value) Then
                drRegPayInfoLoan.BONUSPAYMENT = Double.Parse(Me.loanBonusValueHiddenField.Value, Globalization.CultureInfo.CurrentCulture)
            End If

            '初回支払期限
            If Not (Me.loanDueDateNumericBox.Value Is Nothing) Then
                drRegPayInfoLoan.DUEDATE = Integer.Parse(Me.loanDueDateNumericBox.Value, Globalization.CultureInfo.CurrentCulture)
            End If

            '削除フラグ
            drRegPayInfoLoan.DELFLG = STR_DELETEFLG_NOT


            '選択フラグ
            If Me.payMethodSegmentedButton.SelectedValue.Equals(STR_PAYMETHOD_LOAN.ToString) Then
                drRegPayInfoLoan.SELECTFLG = STR_SELECTFLG_SELECTED
            Else
                drRegPayInfoLoan.SELECTFLG = STR_SELECTFLG_NOT
            End If

            '利息
            If Not String.IsNullOrEmpty(Me.loanInterestrateValueHiddenField.Value) Then
                drRegPayInfoLoan.INTERESTRATE = Double.Parse(Me.loanInterestrateValueHiddenField.Value, Globalization.CultureInfo.CurrentCulture)
            End If

            'ローンデータレコード追加
            dtPayInfo.Rows.Add(drRegPayInfoLoan)

            'データレコード開放
            drRegPayInfoLoan = Nothing

        Else
            '見積情報保存後

            '■見積顧客情報データテーブル
            '□所有者
            '顧客区分
            If Me.shoyusyaKojinCheckMark.Value = StrTrue Then
                dtCustomerInfo.Rows(0).Item("CustPart") = STR_CUSTPART_KOJIN
            Else
                dtCustomerInfo.Rows(0).Item("CustPart") = STR_CUSTPART_HOJIN
            End If

            '氏名
            dtCustomerInfo.Rows(0).Item("Name") = Me.shoyusyaNameTextBox.Text

            '国民番号
            dtCustomerInfo.Rows(0).Item("SocialId") = Me.shoyusyaIDTextBox.Text

            '郵便番号
            dtCustomerInfo.Rows(0).Item("ZipCode") = Me.shoyusyaZipCodeTextBox.Text

            '住所
            dtCustomerInfo.Rows(0).Item("Address") = Me.shoyusyaAddressTextBox.Text

            '電話番号
            dtCustomerInfo.Rows(0).Item("TelNo") = Me.shoyusyaTelTextBox.Text

            '携帯電話番号
            dtCustomerInfo.Rows(0).Item("Mobile") = Me.shoyusyaMobileTextBox.Text

            'e-MAILアドレス
            dtCustomerInfo.Rows(0).Item("Email") = Me.shoyusyaEmailTextBox.Text


            '□使用者
            '顧客区分
            If Me.shiyosyaKojinCheckMark.Value = StrTrue Then
                dtCustomerInfo.Rows(1).Item("CustPart") = STR_CUSTPART_KOJIN
            Else
                dtCustomerInfo.Rows(1).Item("CustPart") = STR_CUSTPART_HOJIN
            End If

            '氏名
            dtCustomerInfo.Rows(1).Item("Name") = Me.shiyosyaNameTextBox.Text

            '国民番号
            dtCustomerInfo.Rows(1).Item("SocialId") = Me.shiyosyaIDTextBox.Text

            '郵便番号
            dtCustomerInfo.Rows(1).Item("ZipCode") = Me.shiyosyaZipCodeTextBox.Text

            '住所
            dtCustomerInfo.Rows(1).Item("Address") = Me.shiyosyaAddressTextBox.Text

            '電話番号
            dtCustomerInfo.Rows(1).Item("TelNo") = Me.shiyosyaTelTextBox.Text

            '携帯電話番号
            dtCustomerInfo.Rows(1).Item("Mobile") = Me.shiyosyaMobileTextBox.Text

            'e-MAILアドレス
            dtCustomerInfo.Rows(1).Item("Email") = Me.shiyosyaEmailTextBox.Text


            If dsEstimation.Tables("IC3070201PaymentInfo").Rows.Count = 0 Then

                '■見積保険情報データテーブル
                Dim drRegInsuranceInfoNewRow As IC3070202DataSet.IC3070202EstInsuranceInfoRow
                drRegInsuranceInfoNewRow = dtInsuranceInfo.NewRow
                '見積管理ID
                drRegInsuranceInfoNewRow.ESTIMATEID = CType(Me.lngEstimateIdHiddenField.Value, Long)
                '保険区分
                drRegInsuranceInfoNewRow.INSUDVS = String.Empty
                'データレコード追加
                dtInsuranceInfo.Rows.Add(drRegInsuranceInfoNewRow)
                'データレコード開放
                drRegInsuranceInfoNewRow = Nothing

                '■見積支払方法情報データテーブル
                '□現金
                '現金データレコード作成
                Dim drRegPayInfoCashNewRow As IC3070202DataSet.IC3070202EstPaymentInfoRow
                drRegPayInfoCashNewRow = dtPayInfo.NewRow
                '見積管理ID
                drRegPayInfoCashNewRow.ESTIMATEID = CType(Me.lngEstimateIdHiddenField.Value, Long)
                '支払方法区分
                drRegPayInfoCashNewRow.PAYMENTMETHOD = STR_PAYMETHOD_CASH
                '削除フラグ
                drRegPayInfoCashNewRow.DELFLG = STR_DELETEFLG_NOT
                '現金データレコード追加
                dtPayInfo.Rows.Add(drRegPayInfoCashNewRow)
                'データレコード開放
                drRegPayInfoCashNewRow = Nothing

                '□ローン
                'ローンデータレコード作成
                Dim drRegPayInfoLoanNewRow As IC3070202DataSet.IC3070202EstPaymentInfoRow
                drRegPayInfoLoanNewRow = dtPayInfo.NewRow()
                '見積管理ID
                drRegPayInfoLoanNewRow.ESTIMATEID = CType(Me.lngEstimateIdHiddenField.Value, Long)
                '支払方法区分
                drRegPayInfoLoanNewRow.PAYMENTMETHOD = STR_PAYMETHOD_LOAN
                '削除フラグ
                drRegPayInfoLoanNewRow.DELFLG = STR_DELETEFLG_NOT
                'ローンデータレコード追加
                dtPayInfo.Rows.Add(drRegPayInfoLoanNewRow)
                'データレコード開放
                drRegPayInfoLoanNewRow = Nothing

            End If

            ''■見積諸費用情報データテーブル
            ''□車両購入税
            ''費用項目名
            '■見積保険情報データテーブル

            '保険区分(1:自社、2:他社)
            If Me.jisyaCheckMark.Value = StrTrue Then
                dtInsuranceInfo.Rows(0).Item("InsuDvs") = STR_INSUDVS_JISYA
            Else
                dtInsuranceInfo.Rows(0).Item("InsuDvs") = STR_INSUDVS_TASYA
            End If

            '保険会社コード
            dtInsuranceInfo.Rows(0).Item("InsucomCd") = Me.SelectInsuComCdHidden.Value

            '保険種別
            dtInsuranceInfo.Rows(0).Item("InsuKind") = Me.SelectInsuKindCdHidden.Value

            '保険金額
            If String.IsNullOrEmpty(Me.insuAmountValueHiddenField.Value) Then
                dtInsuranceInfo.Rows(0).Item("Amount") = DBNull.Value
            Else
                dtInsuranceInfo.Rows(0).Item("Amount") = Double.Parse(Me.insuAmountValueHiddenField.Value, Globalization.CultureInfo.CurrentCulture)
            End If

            '■見積支払方法情報データテーブル
            '□現金
            '頭金
            If String.IsNullOrEmpty(Me.cashDepositValueHiddenField.Value) Then
                dtPayInfo.Rows(0).Item("Deposit") = DBNull.Value
            Else
                dtPayInfo.Rows(0).Item("Deposit") = Double.Parse(Me.cashDepositValueHiddenField.Value, Globalization.CultureInfo.CurrentCulture)
            End If

            '選択フラグ
            If Me.payMethodSegmentedButton.SelectedValue.Equals(STR_PAYMETHOD_CASH.ToString) Then
                dtPayInfo.Rows(0).Item("SELECTFLG") = STR_SELECTFLG_SELECTED
            Else
                dtPayInfo.Rows(0).Item("SELECTFLG") = STR_SELECTFLG_NOT
            End If

            '□ローン
            '融資会社コード
            dtPayInfo.Rows(1).Item("FinanceComCode") = Me.SelectFinanceComHiddenField.Value

            '支払期間
            If loanPayPeriodNumericBox.Value Is Nothing Then
                dtPayInfo.Rows(1).Item("PaymentPeriod") = DBNull.Value
            Else
                dtPayInfo.Rows(1).Item("PaymentPeriod") = Integer.Parse(Me.loanPayPeriodNumericBox.Value, Globalization.CultureInfo.CurrentCulture)
            End If

            '毎月返済額
            If String.IsNullOrEmpty(Me.loanMonthlyValueHiddenField.Value) Then
                dtPayInfo.Rows(1).Item("MonthlyPayment") = DBNull.Value
            Else
                dtPayInfo.Rows(1).Item("MonthlyPayment") = Double.Parse(Me.loanMonthlyValueHiddenField.Value, Globalization.CultureInfo.CurrentCulture)
            End If

            '頭金
            If String.IsNullOrEmpty(Me.loanDepositValueHiddenField.Value) Then
                dtPayInfo.Rows(1).Item("Deposit") = DBNull.Value
            Else
                dtPayInfo.Rows(1).Item("Deposit") = Double.Parse(Me.loanDepositValueHiddenField.Value, Globalization.CultureInfo.CurrentCulture)
            End If

            'ボーナス時返済額
            If String.IsNullOrEmpty(Me.loanBonusValueHiddenField.Value) Then
                dtPayInfo.Rows(1).Item("BonusPayment") = DBNull.Value
            Else
                dtPayInfo.Rows(1).Item("BonusPayment") = Double.Parse(Me.loanBonusValueHiddenField.Value, Globalization.CultureInfo.CurrentCulture)
            End If

            '初回支払期限
            If Me.loanDueDateNumericBox.Value Is Nothing Then
                dtPayInfo.Rows(1).Item("DueDate") = DBNull.Value
            Else
                dtPayInfo.Rows(1).Item("DueDate") = Integer.Parse(Me.loanDueDateNumericBox.Value, Globalization.CultureInfo.CurrentCulture)
            End If

            '選択フラグ
            If Me.payMethodSegmentedButton.SelectedValue.Equals(STR_PAYMETHOD_LOAN.ToString) Then
                dtPayInfo.Rows(1).Item("SELECTFLG") = STR_SELECTFLG_SELECTED
            Else
                dtPayInfo.Rows(1).Item("SELECTFLG") = STR_SELECTFLG_NOT
            End If

            '利息
            If String.IsNullOrEmpty(Me.loanInterestrateValueHiddenField.Value) Then
                dtPayInfo.Rows(1).Item("INTERESTRATE") = DBNull.Value
            Else
                dtPayInfo.Rows(1).Item("INTERESTRATE") = Double.Parse(Me.loanInterestrateValueHiddenField.Value, Globalization.CultureInfo.CurrentCulture)
            End If
        End If


        '■見積車両オプション情報データテーブル
        ''オプションデータレコード削除
        dtVclOptionInfo.Clear()

        '□メーカーオプションデータ格納
        Dim drRegMkrOption As DataRow()

        Dim filterExpMaker As String = String.Format(CultureInfo.InvariantCulture, "OptionPart = '{0}'", STR_OPTIONPART_MAKER)
        drRegMkrOption = dsEstimation.Tables("IC3070201VclOptionInfo").Select(filterExpMaker)

        Dim indexMkrOptionCnt = 0
        For Each drRegMkrOptRow As IC3070201DataSet.IC3070201VclOptionInfoRow In drRegMkrOption
            ' リピータ行
            Dim row As RepeaterItem = Me.mkrOptionRepeater.Items(indexMkrOptionCnt)

            If Not IsNothing(row) Then
                ' コントロールのIDを取得
                Dim mkrOptionPriceID As String = DirectCast(row.FindControl("mkrOptionPriceText"), TextBox).UniqueID

                ' オプション価格を反映
                Dim optionPrice As String = Request.Form(mkrOptionPriceID)
                If String.IsNullOrEmpty(optionPrice) Then
                    drRegMkrOptRow.PRICE = 0
                Else
                    drRegMkrOptRow.PRICE = Double.Parse(optionPrice, CultureInfo.InvariantCulture)
                End If

                ' 行の追加
                dtVclOptionInfo.ImportRow(drRegMkrOptRow)
            End If

            indexMkrOptionCnt += 1
        Next


        ' 販売店オプションデータ（TCV）格納
        Dim indexCnt = 0
        Dim filterExpDlrOpt As String = String.Format(CultureInfo.InvariantCulture, "OptionPart = '{0}'", STR_OPTIONPART_DEALER)
        Dim drRegDlrOpt As DataRow() = dsEstimation.Tables("IC3070201VclOptionInfo").Select(filterExpDlrOpt)

        For Each drRegDlrOptRow As IC3070201DataSet.IC3070201VclOptionInfoRow In drRegDlrOpt

            ' リピータ行
            Dim row As RepeaterItem = Me.dlrOptionRepeater.Items(indexCnt)

            If Not IsNothing(row) Then
                ' コントロールのIDを取得
                Dim optionPriceID As String = DirectCast(row.FindControl("tcvDlrOptionPriceText"), TextBox).UniqueID
                Dim installCostID As String = DirectCast(row.FindControl("tcvDlrOptionInstallCostText"), TextBox).UniqueID

                ' オプション価格を反映
                Dim optionPrice As String = Request.Form(optionPriceID)
                If String.IsNullOrEmpty(optionPrice) Then
                    drRegDlrOptRow.PRICE = 0
                Else
                    drRegDlrOptRow.PRICE = Double.Parse(optionPrice, CultureInfo.InvariantCulture)
                End If

                ' オプション価格を反映
                Dim installCost As String = Request.Form(installCostID)
                If String.IsNullOrEmpty(installCost) Then
                    drRegDlrOptRow.INSTALLCOST = 0
                Else
                    drRegDlrOptRow.INSTALLCOST = Double.Parse(installCost, CultureInfo.InvariantCulture)
                End If

                ' 行の追加
                dtVclOptionInfo.ImportRow(drRegDlrOptRow)
            End If

            indexCnt += 1
        Next


        '□販売店オプションデータ格納
        Dim j As Integer


        For j = 1 To Integer.Parse(Me.dlrOptionCountHiddenField.Value, Globalization.CultureInfo.CurrentCulture)

            '販売店オプションデータレコード作成
            Dim drRegDlrOption As IC3070202DataSet.IC3070202EstVclOptionInfoRow
            drRegDlrOption = dtVclOptionInfo.NewRow

            '見積管理ID
            drRegDlrOption.ESTIMATEID = Long.Parse(Me.lngEstimateIdHiddenField.Value, Globalization.CultureInfo.CurrentCulture)

            'オプション区分
            drRegDlrOption.OPTIONPART = STR_OPTIONPART_DEALER_ICROP

            'オプションコード
            drRegDlrOption.OPTIONCODE = j

            'オプション名
            drRegDlrOption.OPTIONNAME = Request.Form.Item(String.Concat("optionNameText", j))

            '価格
            drRegDlrOption.PRICE = Double.Parse(Request.Form.Item(String.Concat("optionPriceText", j)), Globalization.CultureInfo.CurrentCulture)

            '取付費用
            drRegDlrOption.INSTALLCOST = Double.Parse(Request.Form.Item(String.Concat("optionMoneyText", j)), Globalization.CultureInfo.CurrentCulture)

            'データレコード追加
            dtVclOptionInfo.Rows.Add(drRegDlrOption)

            'データレコード開放
            drRegDlrOption = Nothing

        Next

        '■見積諸費用情報データテーブル

        '諸費用データレコード削除
        dtChargeInfo.Clear()

        '□車両購入税
        '車両購入税データレコード作成
        Dim drRegChargePurchaseTax As IC3070202DataSet.IC3070202EstChargeInfoRow
        drRegChargePurchaseTax = dtChargeInfo.NewRow

        '見積管理ID
        drRegChargePurchaseTax.ESTIMATEID = Long.Parse(Me.lngEstimateIdHiddenField.Value, Globalization.CultureInfo.CurrentCulture)

        '費用項目コード
        drRegChargePurchaseTax.ITEMCODE = STR_ITEMCODE_1

        '費用項目名
        drRegChargePurchaseTax.ITEMNAME = HttpUtility.HtmlDecode(Me.CarBuyTaxLabelCustomLabel.Text)

        '価格
        If Not String.IsNullOrEmpty(Me.carBuyTaxHiddenField.Value) Then
            drRegChargePurchaseTax.PRICE = Double.Parse(Me.carBuyTaxHiddenField.Value, Globalization.CultureInfo.CurrentCulture)
        End If

        '諸費用区分
        drRegChargePurchaseTax.CHARGEDVS = Me.chargeSegmentedButton.SelectedValue

        '車両購入税データレコード追加
        dtChargeInfo.Rows.Add(drRegChargePurchaseTax)

        'データレコード開放
        drRegChargePurchaseTax = Nothing

        '□登録費用
        '登録費用データレコード作成
        Dim drRegChargeRegExpense As IC3070202DataSet.IC3070202EstChargeInfoRow
        drRegChargeRegExpense = dtChargeInfo.NewRow

        '見積管理ID
        drRegChargeRegExpense.ESTIMATEID = Long.Parse(Me.lngEstimateIdHiddenField.Value, Globalization.CultureInfo.CurrentCulture)

        '費用項目コード
        drRegChargeRegExpense.ITEMCODE = STR_ITEMCODE_2

        '費用項目名
        drRegChargeRegExpense.ITEMNAME = HttpUtility.HtmlDecode(Me.regPriceLabelCustomLabel.Text)

        '価格
        If Not String.IsNullOrEmpty(Me.regCostValueHiddenField.Value) Then
            drRegChargeRegExpense.PRICE = Double.Parse(Me.regCostValueHiddenField.Value, Globalization.CultureInfo.CurrentCulture)
        End If

        '諸費用区分
        drRegChargeRegExpense.CHARGEDVS = Me.chargeSegmentedButton.SelectedValue

        '登録費用データレコード追加
        dtChargeInfo.Rows.Add(drRegChargeRegExpense)

        'データレコード開放
        drRegChargeRegExpense = Nothing

        '□手入力諸費用
        Dim intChargeCount As Integer
        Dim intChargeIndex As Integer
        For intChargeCount = 1 To Integer.Parse(Me.chargeInfoCountHiddenField.Value, Globalization.CultureInfo.CurrentCulture)

            '手入力諸費用データレコード作成
            Dim drChargeInfo As IC3070202DataSet.IC3070202EstChargeInfoRow
            drChargeInfo = dtChargeInfo.NewRow

            '連番を算出
            intChargeIndex = intChargeCount + 10

            '見積管理ID
            drChargeInfo.ESTIMATEID = Long.Parse(Me.lngEstimateIdHiddenField.Value, Globalization.CultureInfo.CurrentCulture)

            '費用項目コード
            drChargeInfo.ITEMCODE = CType(intChargeIndex, String)

            '費用項目名
            drChargeInfo.ITEMNAME = Request.Form.Item(String.Concat("chargeInfoText", intChargeIndex))

            '価格
            If Not String.IsNullOrEmpty(Me.regCostValueHiddenField.Value) Then
                drChargeInfo.PRICE = Double.Parse(Request.Form.Item(String.Concat("chargeInfoPrice", intChargeIndex)), Globalization.CultureInfo.CurrentCulture)
            End If

            '諸費用区分
            drChargeInfo.CHARGEDVS = Me.chargeSegmentedButton.SelectedValue

            '手入力諸費用データレコード追加
            dtChargeInfo.Rows.Add(drChargeInfo)

            'データレコード開放
            drChargeInfo = Nothing

        Next


        '■見積下取車両情報データテーブル
        'データ削除
        dtTradeInCarInfo.Clear()

        Dim intCarCount As Integer


        For intCarCount = 1 To Integer.Parse(Me.tradeInCarCountHiddenField.Value, Globalization.CultureInfo.CurrentCulture)

            '見積下取車両データレコード作成
            Dim drRegTradeInCar As IC3070202DataSet.IC3070202EstTradeInCarInfoRow
            drRegTradeInCar = dtTradeInCarInfo.NewRow


            '見積管理ID
            drRegTradeInCar.ESTIMATEID = Long.Parse(Me.lngEstimateIdHiddenField.Value, Globalization.CultureInfo.CurrentCulture)

            '連番
            drRegTradeInCar.SEQNO = intCarCount

            '車名
            drRegTradeInCar.VEHICLENAME = Request.Form.Item(String.Concat("tradeInCarText", intCarCount))

            ''提示価格
            drRegTradeInCar.ASSESSEDPRICE = Double.Parse(Request.Form.Item(String.Concat("tradeInCarPrice", intCarCount)), Globalization.CultureInfo.CurrentCulture)


            '見積下取車両データレコード追加
            dtTradeInCarInfo.Rows.Add(drRegTradeInCar)

            'データレコード開放
            drRegTradeInCar = Nothing

        Next

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("CreateEstimateDataSet End")

        Return dsRegEstimation

    End Function

    ''' <summary>
    ''' 入力内容変更フラグを立てる
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub InputChanged()

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InputChanged Start")

        Me.blnInputChangedClientHiddenField.Value = StrTrue

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InputChanged End")

    End Sub

    ''' <summary>
    ''' 見積保存ボタンの制御
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitButtonEvent()
        'カーアイコン
        'ロード中のクライアントサイドスクリプトを埋め込む
        saveLinkButton.OnClientClick = "return saveLinkClick();"
    End Sub

    ''' <summary>
    ''' 下取車両情報を再取得する。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Private Sub updateTradeInCars_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles tradeInCarButton.Click
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("updateTradeInCars_Click Start")

        Dim dlrCd As String = Me.strDlrcdHiddenField.Value
        Dim strCd As String = Me.strStrCdHiddenField.Value

        Dim fuSeqNo As Decimal = CType(Me.lngFollowupBoxSeqNoHiddenField.Value, Decimal)

        Dim estimateId As Long = CType(Me.lngEstimateIdHiddenField.Value, Long)

        Dim bizLogic As SC3070205BusinessLogic      'ビジネスロジックオブジェクト

        'ビジネスロジックオブジェクト作成
        bizLogic = New SC3070205BusinessLogic

        '下取車両価格再取得
        If Not tradeInCarDataTable Is Nothing Then
            tradeInCarDataTable.Dispose()
        End If
        tradeInCarDataTable = CreateTradeInCarDataTable(bizLogic.GetUcarAssessmentInfo(dlrCd, _
                                                                                       strCd, _
                                                                                       fuSeqNo, _
                                                                                       estimateId))

        '下取り車両件数 HIDDEN値設定(空行が1行含まれるので1引く)
        Me.tradeInCarCountHiddenField.Value = tradeInCarDataTable.Rows.Count() - 1

        ' □下取り車両セット
        tradeInCarDataTableRep.DataSource = tradeInCarDataTable
        tradeInCarDataTableRep.DataBind()

        '入力内容変更フラグ
        Me.blnInputChangedClientHiddenField.Value = True

        'オブジェクト開放
        bizLogic = Nothing

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("updateTradeInCars_Click End")
    End Sub

    ''' <summary>
    ''' 顧客リンク押下時処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Private Sub CustomerButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles shoyusyaNameLinkButton.Click
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("CustomerButton_Click Start")

        '顧客詳細画面に渡す引数を設定
        MyBase.SetValue(ScreenPos.Next, "SearchKey.FLLWUPBOX_STRCD", Me.strStrCdHiddenField.Value)
        MyBase.SetValue(ScreenPos.Next, "SearchKey.FOLLOW_UP_BOX", Me.lngFollowupBoxSeqNoHiddenField.Value)
        MyBase.SetValue(ScreenPos.Next, "SearchKey.CSTKIND", Me.strCstKindHiddenField.Value)
        MyBase.SetValue(ScreenPos.Next, "SearchKey.CUSTOMERCLASS", Me.strCustomerClassHiddenField.Value)
        MyBase.SetValue(ScreenPos.Next, "SearchKey.CRCUSTID", Me.strCRCustIdHiddenField.Value)

        Me.RedirectNextScreen("SC3080201")

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("CustomerButton_Click End")
    End Sub

    ''' <summary>
    ''' ダミー保存ボタン押下時処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Private Sub UpdateButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UpdateButton.Click
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateButton_Click Start")
        SaveEstimation()
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateButton_Click End")
    End Sub

    ''' <summary>
    ''' サーバ側入力チェックを実施し、見積情報を保存する
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SaveEstimation()
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("saveEstimation Start")

        '入力チェック（必須以外）
        If Not CheckInputFormat() Then
            Exit Sub
        End If

        '支払い総額のチェック
        If Not CheckTotalPrice() Then
            Exit Sub
        End If

        '見積登録情報用データセット
        Dim dsRegEstimation As IC3070202DataSet

        '見積情報登録用データセット作成
        dsRegEstimation = CreateEstimateDataSet()

        Dim bizLogic As SC3070205BusinessLogic      'ビジネスロジックオブジェクト
        Dim blnResult As IC3070202DataSet.IC3070202EstResultDataTable       '戻り値

        'ビジネスロジックオブジェクト作成
        bizLogic = New SC3070205BusinessLogic

        '見積情報登録
        blnResult = bizLogic.UpdateEstimation(dsRegEstimation)

        '保存済みフラグ
        Me.savedEstimationFlgHiddenField.Value = "1"

        '価格相談呼び出しフラグ
        Me.approvalButtonFlgHiddenField.Value = "1"

        '入力内容変更フラグ
        Me.blnInputChangedClientHiddenField.Value = False

        '初期化
        Me.deliDateInitialValueHiddenField.Value = Me.deliDateChangeValueHiddenField.Value
        Me.periodInitialValueHiddenField.Value = Me.periodChangeValueHiddenField.Value
        Me.firstPayInitialValueHiddenField.Value = Me.firstPayChangeValueHiddenField.Value
        Me.payMethodHiddenField.Value = Me.payMethodSegmentedButton.SelectedValue

        'CREATEDATE対応（STEP1.5以降に使用予定）
        Me.createDateHiddenField.Value = blnResult.Rows(0).Item("CreateDate")

        'オブジェクト開放
        bizLogic = Nothing

        'パネル更新
        UpdatePanel2.Update()

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("saveEstimation End")
    End Sub

    ''' <summary>
    ''' 下取り車両データテーブル変換
    ''' </summary>
    ''' <param name="dt">入力データテーブル</param>
    ''' <remarks></remarks>
    Private Function CreateTradeInCarDataTable(ByVal dt As IC3070201DataSet.IC3070201TradeincarInfoDataTable) As DataTable
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("createTradeInCarDataTable Start")

        Using dtTradeInCar As New DataTable
            dtTradeInCar.Locale = Globalization.CultureInfo.InvariantCulture

            dtTradeInCar.Columns.Add("NO")
            dtTradeInCar.Columns.Add("VEHICLENAME")
            dtTradeInCar.Columns.Add("ASSESSEDPRICE")

            Dim drTradeInCar As DataRow
            Dim i As Integer = 1

            For Each dr As IC3070201DataSet.IC3070201TradeincarInfoRow In dt

                drTradeInCar = dtTradeInCar.NewRow
                drTradeInCar.Item("NO") = i
                drTradeInCar.Item("VEHICLENAME") = dr.Item("VEHICLENAME")
                drTradeInCar.Item("ASSESSEDPRICE") = dr.Item("ASSESSEDPRICE")
                dtTradeInCar.Rows.Add(drTradeInCar)
                i = i + 1
            Next

            '空行を追加
            drTradeInCar = dtTradeInCar.NewRow
            drTradeInCar.Item("NO") = i
            drTradeInCar.Item("VEHICLENAME") = String.Empty
            drTradeInCar.Item("ASSESSEDPRICE") = String.Empty
            dtTradeInCar.Rows.Add(drTradeInCar)

            Return dtTradeInCar
        End Using

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("createTradeInCarDataTable End")
    End Function

    ''' <summary>
    ''' TCVオプションデータテーブル変換
    ''' </summary>
    ''' <param name="dt">TCVメーカーオプション入力データテーブル</param>
    ''' <param name="optionPart">オプション区分</param>
    ''' <returns>変換したデータテーブル</returns>
    ''' <remarks></remarks>
    Private Function CreateTcvOptionDataTable(ByVal dt As IC3070201DataSet.IC3070201VclOptionInfoDataTable, ByVal optionPart As String) As DataTable
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("CreateTcvOptionDataTable Start")

        Using dtTcvOption As New DataTable
            dtTcvOption.Locale = Globalization.CultureInfo.InvariantCulture

            dtTcvOption.Columns.Add("OPTIONPART")
            dtTcvOption.Columns.Add("OPTIONNAME")
            dtTcvOption.Columns.Add("PRICE", GetType(System.Double))
            dtTcvOption.Columns.Add("INSTALLCOST", GetType(System.Double))

            '該当件数を設定
            Dim drTcvOptionFreeRows As DataRow() = Nothing

            If optionPart = STR_OPTIONPART_MAKER Then
                drTcvOptionFreeRows = dt.Select(String.Concat("OPTIONPART = ", STR_OPTIONPART_MAKER))
            ElseIf optionPart = STR_OPTIONPART_DEALER Then
                drTcvOptionFreeRows = dt.Select(String.Concat("OPTIONPART = ", STR_OPTIONPART_DEALER))
            End If

            Dim drTcvOption As DataRow = Nothing
            For Each dr As IC3070201DataSet.IC3070201VclOptionInfoRow In drTcvOptionFreeRows

                drTcvOption = dtTcvOption.NewRow
                drTcvOption.Item("OPTIONPART") = dr.Item("OPTIONPART")
                drTcvOption.Item("OPTIONNAME") = dr.Item("OPTIONNAME")
                drTcvOption.Item("PRICE") = dr.Item("PRICE")
                drTcvOption.Item("INSTALLCOST") = dr.Item("INSTALLCOST")
                dtTcvOption.Rows.Add(drTcvOption)

            Next

            drTcvOptionFreeRows = Nothing
            drTcvOption = Nothing

            Return dtTcvOption
        End Using

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("CreateTcvOptionDataTable End")
    End Function

    ''' <summary>
    ''' 諸費用データテーブル変換
    ''' </summary>
    ''' <param name="dt">諸費用入力データテーブル</param>
    ''' <returns>変換したデータテーブル</returns>
    ''' <remarks></remarks>
    Private Function CreateChargeInfoDataTable(ByVal dt As IC3070201DataSet.IC3070201ChargeInfoDataTable) As DataTable
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("CreateChargeInfoDataTable Start")

        Using dtChargeInfo As New DataTable
            dtChargeInfo.Locale = Globalization.CultureInfo.InvariantCulture

            dtChargeInfo.Columns.Add("ITEMCODE")
            dtChargeInfo.Columns.Add("ITEMNAME")
            dtChargeInfo.Columns.Add("PRICE")

            '諸費用項目が手入力に該当件数を設定
            Dim drChargeInfoFreeRows As DataRow()
            drChargeInfoFreeRows = dt.Select(STR_GET_CHARGEFREE)

            Dim drChargeInfo As DataRow = Nothing
            Dim i As Integer = 11

            For Each dr As IC3070201DataSet.IC3070201ChargeInfoRow In drChargeInfoFreeRows

                drChargeInfo = dtChargeInfo.NewRow
                drChargeInfo.Item("ITEMCODE") = i
                drChargeInfo.Item("ITEMNAME") = dr.Item("ITEMNAME")
                drChargeInfo.Item("PRICE") = dr.Item("PRICE")
                dtChargeInfo.Rows.Add(drChargeInfo)
                i = i + 1
            Next

            'データ行数が10行未満だった場合のみ、空行を追加
            If drChargeInfoFreeRows.Count < 10 Then
                '空行を追加
                drChargeInfo = dtChargeInfo.NewRow
                drChargeInfo.Item("ITEMCODE") = i
                drChargeInfo.Item("ITEMNAME") = String.Empty
                drChargeInfo.Item("PRICE") = String.Empty
                dtChargeInfo.Rows.Add(drChargeInfo)
            End If

            drChargeInfoFreeRows = Nothing
            drChargeInfo = Nothing

            Return dtChargeInfo
        End Using

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("CreateChargeInfoDataTable End")
    End Function


    ''' <summary>
    ''' 対象見積管理ID取得
    ''' </summary>
    ''' <param name="allEstimeId">見積管理ID(カンマ区切り)</param>
    ''' <param name="Index">対象Index番号</param>
    ''' <returns>見積管理ID</returns>
    ''' <remarks>Indexに該当する見積管理IDを返す</remarks>
    Private Function GetSelectedEstimateId(ByVal allEstimeId As String, ByVal Index As Long) As Long
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSelectedEstimateId Start")

        Dim estimetaId = allEstimeId.Split(","c)

        Return CType(estimetaId(Index), Long)

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSelectedEstimateId End")
    End Function

    ''' <summary>
    ''' 再表示時初期設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ReloadInitialSetting()
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("ReloadInitialSetting Start")

        '----------------------------------------------------
        ' 画面初期化
        '----------------------------------------------------
        Me.blnInputChangedClientHiddenField.Value = False

        'HIDDEN項目
        Me.strDlrcdHiddenField.Value = ""
        Me.strStrCdHiddenField.Value = ""
        Me.lngFollowupBoxSeqNoHiddenField.Value = ""
        Me.strCstKindHiddenField.Value = ""
        Me.strCustomerClassHiddenField.Value = ""
        Me.strCRCustIdHiddenField.Value = ""
        Me.basePriceHiddenField.Value = ""
        Me.contractFlgHiddenField.Value = ""

        'メモ最大桁数取得
        Me.memoMaxHiddenField.Value = ""

        '顧客氏名
        Me.cstNameHiddenField.Value = ""

        '保険会社リスト
        Me.InsComInsuComCdHidden.Value = ""
        Me.InsComInsuKubunHidden.Value = ""
        Me.InsComInsuComNameHidden.Value = ""

        '保険種別リスト作成
        Me.InsKindInsuComCdHidden.Value = ""
        Me.InsKindInsuKindCdHidden.Value = ""
        Me.InsKindInsuKindNmHidden.Value = ""

        '印刷日、契約日、契約書No
        Me.estPrintDateLabel.Visible = True
        Me.contractDateLabel.Visible = True
        Me.dateLabel.Visible = True
        Me.contractNoTitleLabel.Visible = True
        Me.contractNoLabel.Visible = True

        '見積アイコンの初期化
        Me.carIcon0.Visible = True
        Me.carIcon1.Visible = True
        Me.carIcon2.Visible = True
        Me.carIcon3.Visible = True
        Me.carIcon4.Visible = True
        RemoveCssClass(Me.carIcon0, "On", "Off")
        RemoveCssClass(Me.carIcon1, "On", "Off")
        RemoveCssClass(Me.carIcon2, "On", "Off")
        RemoveCssClass(Me.carIcon3, "On", "Off")
        RemoveCssClass(Me.carIcon4, "On", "Off")
        RemoveCssClass(Me.carIcon0, "Save", "Off")
        RemoveCssClass(Me.carIcon1, "Save", "Off")
        RemoveCssClass(Me.carIcon2, "Save", "Off")
        RemoveCssClass(Me.carIcon3, "Save", "Off")
        RemoveCssClass(Me.carIcon4, "Save", "Off")

        ' ■見積／契約者情報
        Me.periodInitialValueHiddenField.Value = ""
        Me.firstPayInitialValueHiddenField.Value = ""
        Me.savedEstimationFlgHiddenField.Value = "0"

        ' ■所有者/使用者
        ' □所有者/使用者セグメントボタン初期選択
        Me.custClassSegmentedButton.SelectedValue = "1"
        ' □敬称付き氏名
        Me.cstEstNameHiddenField.Value = ""
        ' ■□所有者
        ' □氏名
        Me.shoyusyaNameTextBox.Text = ""
        ' □住所
        Me.shoyusyaZipCodeTextBox.Text = ""
        Me.shoyusyaAddressTextBox.Text = ""
        ' □連絡先
        Me.shoyusyaMobileTextBox.Text = ""
        Me.shoyusyaTelTextBox.Text = ""
        ' □E-Mail
        Me.shoyusyaEmailTextBox.Text = ""
        ' □国民ID
        Me.shoyusyaIDTextBox.Text = ""
        ' □顧客区分
        Me.shoyusyaKojinCheckMark.Value = ""
        Me.shoyusyaHojinCheckMark.Value = ""
        ' □敬称
        Me.shoyusyaKeisyoMaeLabel.Text = ""
        Me.shoyusyaKeisyoAtoLabel.Text = ""
        ' ■□使用者
        ' □氏名
        Me.shiyosyaNameTextBox.Text = ""
        ' □住所
        Me.shiyosyaZipCodeTextBox.Text = ""
        Me.shiyosyaAddressTextBox.Text = ""
        ' □連絡先
        Me.shiyosyaMobileTextBox.Text = ""
        Me.shiyosyaTelTextBox.Text = ""
        ' □E-Mail
        Me.shiyosyaEmailTextBox.Text = ""
        ' □国民ID
        Me.shiyosyaIDTextBox.Text = ""
        ' □顧客区分
        Me.shiyosyaKojinCheckMark.Value = ""
        Me.shiyosyaHojinCheckMark.Value = ""
        ' □敬称
        Me.shiyosyaKeisyoMaeLabel.Text = ""
        Me.shiyosyaKeisyoAtoLabel.Text = ""

        ' ■諸費用
        ' □諸費用区分の初期選択
        Me.chargeSegmentedButton.Enabled = True
        Me.chargeSegmentedButton.SelectedValue = "1"
        ' □車両購入税
        Me.CarBuyTaxTextBox.Text = ""
        Me.carBuyTaxHiddenField.Value = ""

        ' □登録費用
        Me.regPriceTextBox.Text = ""
        Me.regCostValueHiddenField.Value = "0"

        Me.chargeInfoCountHiddenField.Value = ""

        ' ■保険
        ' □保険区分
        Me.jisyaCheckMark.Value = ""
        Me.tasyaCheckMark.Value = ""
        ' □保険会社
        Me.SelectInsuComCdHidden.Value = ""
        Me.SelectInsuComNmHidden.Value = ""
        ' □保険種別
        Me.SelectInsuKindCdHidden.Value = ""
        Me.SelectInsuKindNmHidden.Value = ""
        ' □年額
        Me.insuranceAmountTextBox.Text = ""
        Me.insuAmountValueHiddenField.Value = ""

        Me.selectFinanceComNmHiddenField.Value = ""

        ' ■お支払い方法
        Me.payMethodSegmentedButton.Enabled = True
        Me.contractAfterFlgHiddenField.Value = ""
        Me.payMethodSegmentedButton.SelectedValue = "1"
        Me.payMethodHiddenField.Value = ""
        ' ■□現金
        ' □頭金
        Me.cashDepositTextBox.Text = ""
        Me.cashDepositValueHiddenField.Value = ""
        ' ■□ローン
        ' □融資会社
        Me.SelectFinanceComHiddenField.Value = ""
        ' □期間(月)
        Me.loanPayPeriodNumericBox.Value = Nothing
        ' □月額
        Me.loanMonthlyPayTextBox.Text = ""
        Me.loanMonthlyValueHiddenField.Value = ""
        ' □頭金
        Me.loanDepositTextBox.Text = ""
        Me.loanDepositValueHiddenField.Value = ""
        ' □ボーナス
        Me.loanBonusPayTextBox.Text = ""
        Me.loanBonusValueHiddenField.Value = ""
        ' □初回支払(日)
        Me.loanDueDateNumericBox.Value = Nothing

        ' □利息
        Me.loanInterestrateTextBox.Text = ""
        Me.loanInterestrateValueHiddenField.Value = ""

        ' ■お支払い金額
        ' □下取り車両
        Me.tradeInCarButton.Visible = True
        Me.tradeInCarCountHiddenField.Value = ""
        Me.tradeInCarButton.Text = ""
        ' □値引き額
        Me.discountPriceTextBox.Text = ""
        Me.discountPriceValueHiddenField.Value = ""
        ' □納車予定日
        Me.deliDateDateTimeSelector.Value = Nothing
        Me.initialFlgHiddenField.Value = ""
        Me.deliDateAfterValueHiddenField.Value = ""
        Me.deliDateLabel.Text = ""
        ' ■お支払い金額

        ' ■メモ
        Me.memoTextBox.Text = ""

        ' ■車両情報
        '□車両画像
        Me.carImgFileHidden.Value = ""
        ' □車種
        Me.seriesNameHiddenField.Value = ""
        Me.modelNameHiddenField.Value = ""
        Me.seriesCdHiddenField.Value = ""
        Me.modelCdHiddenField.Value = ""
        Me.suffixCdHiddenField.Value = ""
        Me.extColorCdHiddenField.Value = ""
        Me.modelNumberHiddenField.Value = ""
        ' □外装追加費用
        Me.extOptionFlgHiddenField.Value = "0"
        Me.extOptionPriceHiddenField.Value = "0"
        ' □内装追加費用
        Me.intOptionFlgHiddenField.Value = "0"
        Me.intOptionPriceHiddenField.Value = "0"
        ' □メーカーオプション
        ' □販売店オプション（TCV）
        ' □販売店オプション（i-CROP）
        Me.dlrOptionCountHiddenField.Value = ""

        '----------------------------------------------------
        ' HIDDEN値設定
        '----------------------------------------------------
        '見積管理ID
        SetEstimateIdHidden()

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("ReloadInitialSetting End")
    End Sub

    ''' <summary>
    ''' Class名変更
    ''' </summary>
    ''' <param name="element">対象オブジェクト</param>
    ''' <param name="oldCssClass">変更前クラス名</param>
    ''' <param name="newCssClass">変更後クラス名</param>
    ''' <remarks></remarks>
    Private Sub RemoveCssClass(ByVal element As HtmlControl, ByVal oldCssClass As String, ByVal newCssClass As String)
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("RemoveCssClass Start")

        element.Attributes("Class") = element.Attributes("Class").Replace(oldCssClass, newCssClass)

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("RemoveCssClass End")
    End Sub

    ''' <summary>
    ''' 見積アイコンの表示
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub DispInitCarIcon()
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DispInitCarIcon Start")

        Dim selectedEstimateIndex As Long = CType(Me.selectedEstimateIndexHiddenField.Value, Long)
        Dim estimateId As String() = CType(Me.estimateIdHiddenField.Value, String).Split(","c)
        Dim cnt As Long = 0
        Dim idx As Long
        Dim newCssClass As String
        Dim cariconFlg As Boolean

        For Each eId As String In estimateId
            If Not String.IsNullOrEmpty(eId) Then
                cnt = cnt + 1
            End If
        Next

        If cnt <= 1 Then
            '見積管理IDが1件の場合
            If ApprovalStatus.Equals("1") OrElse ApprovalStatus.Equals("2") Then
                '承認依頼中・承認済
                cariconFlg = False
            ElseIf String.IsNullOrEmpty(Me.lngFollowupBoxSeqNoHiddenField.Value) Then
                'メニュー遷移時
                cariconFlg = True
            ElseIf Me.strApprovalModeHiddenField.Value.Equals(ModeApprovalManager) Then
                'マネージャ価格相談
                cariconFlg = False
            ElseIf Me.strApprovalModeHiddenField.Value.Equals(ModeApprovalStaff) Then
                'スタッフ回答参照
                cariconFlg = True
            Else
                If CheckCRActresult(CType(Me.lngEstimateIdHiddenField.Value, Long)) Then
                    '活動完了
                    cariconFlg = False
                Else
                    '活動未完了
                    cariconFlg = True
                End If
            End If
        Else
            '見積管理IDが複数件の場合
            cariconFlg = True
        End If

        If cariconFlg Then
            '見積アイコン表示
            For Each eId As String In estimateId
                If idx = selectedEstimateIndex Then
                    '見表示中アイコン
                    newCssClass = "On"
                ElseIf String.IsNullOrEmpty(eId) Then
                    '見積保存なし
                    newCssClass = "Off"
                Else
                    '見積保存中アイコン
                    newCssClass = "Save"
                End If
                If idx = 0 Then
                    RemoveCssClass(Me.carIcon0, "Off", newCssClass)
                ElseIf idx = 1 Then
                    RemoveCssClass(Me.carIcon1, "Off", newCssClass)
                ElseIf idx = 2 Then
                    RemoveCssClass(Me.carIcon2, "Off", newCssClass)
                ElseIf idx = 3 Then
                    RemoveCssClass(Me.carIcon3, "Off", newCssClass)
                ElseIf idx = 4 Then
                    RemoveCssClass(Me.carIcon4, "Off", newCssClass)
                End If
                idx = idx + 1
            Next

            '保存ポップアップトリガ項目設定
            Me.popOver1.TriggerClientId = "carIcon" & CType(Me.selectedEstimateIndexHiddenField.Value, Long).ToString
        Else
            '見積アイコン非表示
            Me.carIcon0.Visible = False
            Me.carIcon1.Visible = False
            Me.carIcon2.Visible = False
            Me.carIcon3.Visible = False
            Me.carIcon4.Visible = False
        End If

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DispInitCarIcon End")
    End Sub

    ''' <summary>
    ''' 見積管理IDをHiddenに設定
    ''' </summary>
    ''' <remarks>見積管理IDをHiddenに格納する</remarks>
    Private Sub SetEstimateIdHidden()
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SetEstimateIdHidden Start")

        Dim estimateId As String
        Dim selectedEstimateIndex As Long
        Dim lngEstimateId As Long               '見積管理ID

        estimateId = CType(Me.GetValue(ScreenPos.Current, "EstimateId", False), String)
        '選択している見積IDのIndex
        If Me.ContainsKey(ScreenPos.Current, "SelectedEstimateIndex") Then
            selectedEstimateIndex = CType(Me.GetValue(ScreenPos.Current, "SelectedEstimateIndex", False), Long)
        Else
            selectedEstimateIndex = 0
        End If
        '選択している見積ID
        lngEstimateId = CType(GetSelectedEstimateId(estimateId, selectedEstimateIndex), Long)

        Me.lngEstimateIdHiddenField.Value = CType(lngEstimateId, String)
        Me.estimateIdHiddenField.Value = CType(estimateId, String)
        Me.selectedEstimateIndexHiddenField.Value = CType(selectedEstimateIndex, String)

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SetEstimateIdHidden End")
    End Sub

    ''' <summary>
    ''' CR活動結果チェック
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <returns>活動完了：True 活動未完了：False</returns>
    ''' <remarks></remarks>
    Private Function CheckCRActresult(ByVal estimateId As Long) As Boolean
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("CheckCRActresult Start")

        Dim crActresultFlg As Boolean
        Dim bizLogic As New SC3070205BusinessLogic
        Dim dt As SC3070205DataSet.SC3070205FllwUpBoxDataTable = bizLogic.GetCRActresult(estimateId)
        bizLogic = Nothing

        crActresultFlg = False

        If dt.Rows.Count > 0 Then
            '活動結果の取得
            Dim crActresult As String = CType(dt.Rows(0).Item("CRACTRESULT"), String)

            If crActresult.Equals(CRACTRESULT_SUCCESS) Or
                crActresult.Equals(CRACTRESULT_GIVEUP) Or _
                crActresult.Equals(CRACTRESULT_ENQUIRY_COMPLETED) Then
                'CR活動結果が終了している場合
                crActresultFlg = True
            End If
        End If

        Return crActresultFlg

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("CheckCRActresult End")

    End Function

    '''
    ''' <summary>
    ''' 価格相談モード判定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitApprovalMode()
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InitApprovalMode Start")

        Dim strApprovalMode As String = String.Empty               '価格相談モードフラグ

        'OperationCode取得
        Dim opeCd As iCROP.BizLogic.Operation
        If Me.ContainsKey(ScreenPos.Current, "OperationCode") Then
            'セッションに格納されている場合はセッション値を使用
            opeCd = CType(Me.GetValue(ScreenPos.Current, "OperationCode", False), Integer)
        Else
            'セッションから取得できない場合はログインユーザのOperationCodeを使用
            Dim staffInfo As StaffContext = StaffContext.Current
            opeCd = StaffContext.Current.OpeCD
        End If

        'セッション情報取得（通知依頼IDがセットされている場合は価格相談モード＝通知一覧より起動）
        If Me.ContainsKey(ScreenPos.Current, "NoticeReqId") Then

            '通知依頼IDをHIDDEN設定
            Me.noticeReqIdHiddenField.Value = CType(Me.GetValue(ScreenPos.Current, "NoticeReqId", False), String)

            If opeCd.Equals(iCROP.BizLogic.Operation.SSM) Or _
                opeCd.Equals(iCROP.BizLogic.Operation.BM) Then
                'ブランチマネージャ又はセールスマネージャの場合

                'HIDDEN値設定（マネージャ）
                Me.strApprovalModeHiddenField.Value = ModeApprovalManager

            Else
                'HIDDEN値設定（スタッフ）
                Me.strApprovalModeHiddenField.Value = ModeApprovalStaff

            End If

        Else
            'HIDDEN値設定（通常）
            Me.strApprovalModeHiddenField.Value = ModeNormal

        End If

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InitApprovalMode End")
    End Sub


    ' ''' <summary>
    ' ''' 再表示時セッション設定
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Sub SetSessionReload()
    '    Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SetSessionReload Start")

    '    If Me.actionModeHiddenField.Value.Equals(STR_ACTIONMODE_ESTIMATE_CHANGE) Then
    '        '見積切替時
    '        MyBase.SetValue(ScreenPos.Current, "SelectedEstimateIndex", Me.selectedEstimateIndexHiddenField.Value)
    '        MyBase.RemoveValue(ScreenPos.Current, "NoticeReqId")
    '    ElseIf Me.actionModeHiddenField.Value.Equals(STR_ACTIONMODE_CONTRACT_SEND) Then
    '        '契約確定時
    '        MyBase.SetValue(ScreenPos.Current, "EstimateId", Me.estimateIdHiddenField.Value)
    '        MyBase.SetValue(ScreenPos.Current, "SelectedEstimateIndex", Me.selectedEstimateIndexHiddenField.Value)
    '    ElseIf Me.actionModeHiddenField.Value.Equals(STR_ACTIONMODE_CONTRACT_CANCEL) Then
    '        '契約キャンセル時
    '        '活動に紐づく見積管理IDをセッションに設定
    '        SetEstimateIdSession()
    '    End If

    '    Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SetSessionReload End")
    'End Sub

    ' ''' <summary>
    ' ''' 活動に紐づく見積管理IDをSessionに設定
    ' ''' </summary>
    ' ''' <remarks>フォローアップBoxに該当する見積管理IDを全て取得し、セッションに格納する</remarks>
    'Private Sub SetEstimateIdSession()
    '    Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SetEstimateIdSession Start")

    '    Dim bizLogic As SC3070205BusinessLogic
    '    Dim dsEstimateId As SC3070205DataSet.SC3070205EstimateIdDataTable   '活動に紐づく見積管理ID格納用
    '    Dim lngEstimateId As Long
    '    Dim selectedEstimateIndex As Long
    '    Dim estimateId As New StringBuilder
    '    Dim i As Long

    '    lngEstimateId = CType(Me.lngEstimateIdHiddenField.Value, Long)

    '    'ビジネスロジックオブジェクト作成
    '    bizLogic = New SC3070205BusinessLogic

    '    '活動に紐づく全ての見積管理IDを取得
    '    dsEstimateId = bizLogic.GetEstimateId(CType(Me.strDlrcdHiddenField.Value, String),
    '                                          CType(Me.strStrCdHiddenField.Value, String),
    '                                          CType(Me.lngFollowupBoxSeqNoHiddenField.Value, Decimal))


    '    For i = 0 To dsEstimateId.Rows.Count - 1
    '        If Not String.IsNullOrEmpty(estimateId.ToString) Then
    '            estimateId.Append(",")
    '        End If
    '        estimateId.Append(dsEstimateId(i).Item("ESTIMATEID"))
    '        If dsEstimateId.Rows(i).Item("ESTIMATEID").Equals(lngEstimateId) Then
    '            '選択している見積管理IDのIndex設定
    '            selectedEstimateIndex = i
    '        End If
    '    Next

    '    '見積管理ID(カンマ区切り）の設定
    '    Me.estimateIdHiddenField.Value = CType(estimateId.ToString, String)

    '    'セッション情報格納
    '    MyBase.SetValue(ScreenPos.Current, "EstimateId", estimateId.ToString)
    '    MyBase.SetValue(ScreenPos.Current, "SelectedEstimateIndex", selectedEstimateIndex)

    '    Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SetEstimateIdSession End")
    'End Sub

End Class
