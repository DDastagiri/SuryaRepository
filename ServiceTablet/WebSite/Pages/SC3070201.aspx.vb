Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.iCROP.DataAccess.SC3070201
Imports Toyota.eCRB.Estimate.Quotation
Imports System.Data.SqlTypes
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.iCROP.BizLogic.SC3070201
Imports System.Data
Imports Toyota.eCRB.Estimate.Quotation.DataAccess



Partial Class Pages_SC3070201
    Inherits BasePage
    Implements ICustomerForm



#Region "定数定義"

    ''' <summary>
    ''' TRUE
    ''' </summary>
    ''' <remarks></remarks>
    Public Const STR_TRUE As String = "TRUE"

    ''' <summary>
    ''' FALSE
    ''' </summary>
    ''' <remarks></remarks>
    Public Const STR_FALSE As String = "FALSE"

    ''' <summary>
    ''' 契約書状況フラグ (０：未契約)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const STR_CONTRACTFLG_NOT As String = "0"

    ''' <summary>
    ''' 削除フラグ (０：未削除)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const STR_DELETEFLG_NOT As String = "0"

    ''' <summary>
    ''' 契約顧客種別（１：所有者）
    ''' </summary>
    ''' <remarks></remarks>
    Public Const STR_CONTCUSTTYPE_SYOYUSYA As String = "1"

    ''' <summary>
    ''' 契約顧客種別（２：使用者）
    ''' </summary>
    ''' <remarks></remarks>
    Public Const STR_CONTCUSTTYPE_SHIYOSYA As String = "2"

    ''' <summary>
    ''' 顧客種別（１：自社客）
    ''' </summary>
    ''' <remarks></remarks>
    Public Const STR_CUSTKIND_JISYA As String = "1"

    ''' <summary>
    ''' 見積作成画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Public Const STR_DISPID_QUOTATION As String = "SC3070201"

    ''' <summary>
    ''' 見積書印刷画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Public Const STR_DISPID_QUOTATIONPREVIEW As String = "SC3070202"

    ''' <summary>
    ''' 契約書印刷画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Public Const STR_DISPID_CONTRACTPREVIEW As String = "SC3070301"

    ''' <summary>
    ''' メインメニュー画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Public Const STR_DISPID_MAINMENU As String = "SC3010203"

    ''' <summary>
    ''' 支払方法区分（１：現金)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const STR_PAYMETHOD_CASH As Integer = 1

    ''' <summary>
    ''' 支払方法区分（２：ローン)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const STR_PAYMETHOD_LOAN As Integer = 2


    ''' <summary>
    ''' 費用項目コード（１)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const STR_ITEMCODE_1 As Integer = 1

    ''' <summary>
    ''' 費用項目コード（２)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const STR_ITEMCODE_2 As Integer = 2

    ''' <summary>
    ''' メーカーオプション行特定文字列
    ''' </summary>
    ''' <remarks></remarks>
    Public Const STR_GETMKROPTION As String = "OptionPart = '1'"

    ''' <summary>
    ''' 販売店オプション行特定文字列
    ''' </summary>
    ''' <remarks></remarks>
    Public Const STR_GETDLROPTION As String = "OptionPart = '2'"

    ''' <summary>
    ''' 支払い方法（現金）特定文字列
    ''' </summary>
    ''' <remarks></remarks>
    Public Const STR_GETCASH As String = "PAYMENTMETHOD='1'"

    ''' <summary>
    ''' 支払い方法（ローン）特定文字列
    ''' </summary>
    ''' <remarks></remarks>
    Public Const STR_GETLOAN As String = "PAYMENTMETHOD='2'"

    ''' <summary>
    ''' 支払い方法特定文字列
    ''' </summary>
    ''' <remarks></remarks>
    Public Const STR_GETPAYMETHOD As String = "DELFLG='0'"

    ' $99 Ken-Suzuki Add Start
    ''' <summary>
    ''' 諸費用項目（車両購入税用）特定文字列
    ''' </summary>
    ''' <remarks></remarks>
    Public Const STR_GETCARBUYTAX As String = "ITEMCODE='1'"
    ' $99 Ken-Suzuki Add End

    ''' <summary>
    ''' 諸費用項目（登録費用）特定文字列
    ''' </summary>
    ''' <remarks></remarks>
    Public Const STR_GETREGCOST As String = "ITEMCODE='2'"

    ''' <summary>
    ''' 見積顧客情報（所有者）特定文字列
    ''' </summary>
    ''' <remarks></remarks>
    Public Const STR_GETCUSTSHOYUSYA As String = "CONTRACTCUSTTYPE='1'"

    ''' <summary>
    ''' 見積顧客情報（使用者）特定文字列
    ''' </summary>
    ''' <remarks></remarks>
    Public Const STR_GETCUSTSHIYOSYA As String = "CONTRACTCUSTTYPE='2'"

    ''' <summary>
    ''' オプション区分（２：販売店）
    ''' </summary>
    ''' <remarks></remarks>
    Public Const STR_OPTIONPART_DLR As String = "2"

    ''' <summary>
    ''' 見積顧客情報件数（見積新規作成時)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const INT_CUSTOMERCOUNT_NEW As Integer = 0

    ''' <summary>
    ''' 契約状況フラグ（１：契約済み)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const STR_CONTRACTFLG_COMP As String = "1"

    ''' <summary>
    ''' 顧客区分（１：個人)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const STR_CUSTPART_KOJIN As String = "1"

    ''' <summary>
    ''' 顧客区分（２：法人)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const STR_CUSTPART_HOJIN As String = "2"

    ''' <summary>
    ''' 保険区分（１：自社)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const STR_INSUDVS_JISYA As String = "1"

    ''' <summary>
    ''' 保険区分（２：他社)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const STR_INSUDVS_TASYA As String = "2"


    ''' <summary>
    ''' フッターメニュー番号（TCV_車両紹介)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SUBMENU_TCV_CARINVITATION As Integer = 301
    ''' <summary>
    ''' フッターメニュー番号（TCV_諸元表)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SUBMENU_TCV_ORIGINALLIST As Integer = 302
    ''' <summary>
    ''' フッターメニュー番号（TCV_競合車比較)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SUBMENU_TCV_COMPARECOMPETITOR As Integer = 303
    ''' <summary>
    ''' フッターメニュー番号（TCV_ライブラリ)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SUBMENU_TCV_LIBRARY As Integer = 304
    ''' <summary>
    ''' フッターメニュー番号（TCV_見積もり)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SUBMENU_TCV_QUOTATION As Integer = 305

    ''' <summary>
    ''' TCV（車種選択）画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_DISPID_TCV_SELECTSERIES As String = "SC3050101"
    ''' <summary>
    ''' TCV（車両紹介）画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_DISPID_TCV_CARINVITATION As String = "SC3050201"
    ''' <summary>
    ''' TCV（諸元表）画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_DISPID_TCV_ORIGINALLIST As String = "SC3050301"
    ''' <summary>
    ''' TCV（競合車比較）画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_DISPID_TCV_COMPARECOMPETITOR As String = "SC3050401"
    ''' <summary>
    ''' TCV（ライブラリ）画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_DISPID_TCV_LIBRARY As String = "SC3050501"

    ''' TCVコールバック関数(クローズ)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_CALLBACKMETHOD_CLOSE As String = "icropScript.tcvCallback"

    ''' TCVコールバック関数(ステータス)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_CALLBACKMETHOD_STATUS As String = "statusCallbackFunction"

    ''' 実行モード（１：契約）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_ACTIONMODE_CONTRACT As String = "1"

    ''' 敬称位置（１：前）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_NAMETITLE_MAE As String = "1"

    ''' 敬称位置（２：後）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_NAMETITLE_ATO As String = "2"

    ''' オプション区分（メーカー）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_OPTIONPART_MAKER As String = "1"

    ''' オプション区分（販売店）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_OPTIONPART_DEALER As String = "2"

    ''' TCVパラメータ（データ読み込み元）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_ESTIMATEID As String = "EstimateId"

    ''' 金額フォーマット
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_MONEYFORMAT As String = "^[0-9]{1,9}(\.[0-9]{1,2})?$"

#End Region

#Region "メンバ変数"

    Private commonMaster As CommonMasterPage


    Protected intDlrOptionCount As Integer                                                              '販売店オプションID用
    Protected Property dlrOptionDataTable As IEnumerable = New List(Of Integer)                         '販売店オプションテーブル

    Protected intTradeInCarCount As Integer                                                             '下取り車両ID用
    Protected Property tradeInCarDataTable As IEnumerable = New List(Of Integer)                        '下取り車両テーブル

#End Region



    ''' <summary>
    ''' フッターを表示します。
    ''' </summary>
    ''' <param name="commonMaster">イベント発生元</param>
    ''' <param name="category">イベントデータ</param>
    ''' <remarks></remarks>
    Public Overrides Function DeclareCommonMasterFooter(ByVal commonMaster As CommonMasterPage, ByRef category As FooterMenuCategory) As Integer()
        Me.commonMaster = commonMaster


        ''自ページの所属メニューを宣言
        'category = FooterMenuCategory.TCV

        '使用するサブメニューボタンを宣言
        Return {SUBMENU_TCV_CARINVITATION, SUBMENU_TCV_ORIGINALLIST, SUBMENU_TCV_COMPARECOMPETITOR, SUBMENU_TCV_LIBRARY, SUBMENU_TCV_QUOTATION}
    End Function


    ''' <summary>
    ''' ロード時の処理を実施します。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Private Sub SC3070201_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        '初期化
        intDlrOptionCount = 0                       '販売店オプションID用
        intTradeInCarCount = 0                      '下取り車両ID用

        'HIDDEN値設定
        Me.ReferenceModeHiddenField.Value = OperationLocked     'ロックモード


        'ヘッダ表示設定
        '戻るボタン非活性化
        CType(Master, CommonMasterPage).IsRewindButtonEnabled = False

        ''フッタ表示設定

        'メニューボタン定義
        'メインメニュー
        Dim mainMenuButton As CommonMasterFooterButton = commonMaster.GetFooterButton(FooterMenuCategory.MainMenu)
        AddHandler mainMenuButton.Click, AddressOf mainMenuButton_Click
        ''顧客
        'Dim customerButton As CommonMasterFooterButton = commonMaster.GetFooterButton(FooterMenuCategory.Customer)
        'AddHandler customerButton.Click, AddressOf customerButton_Click
        ''TCV
        'Dim tcvButton As CommonMasterFooterButton = commonMaster.GetFooterButton(FooterMenuCategory.TCV)
        'AddHandler tcvButton.Click, AddressOf tcvButton_Click


        'サブメニューボタン定義
        '車両紹介
        Dim carInvitationButton As CommonMasterFooterButton = commonMaster.GetFooterButton(SUBMENU_TCV_CARINVITATION)
        AddHandler carInvitationButton.Click, AddressOf carInvitationButton_Click
        '緒元表
        Dim originaiListButton As CommonMasterFooterButton = commonMaster.GetFooterButton(SUBMENU_TCV_ORIGINALLIST)
        AddHandler originaiListButton.Click, AddressOf originaiListButton_Click
        '競合車比較
        Dim compareCompetitorButton As CommonMasterFooterButton = commonMaster.GetFooterButton(SUBMENU_TCV_COMPARECOMPETITOR)
        AddHandler compareCompetitorButton.Click, AddressOf compareCompetitorButton_Click
        'ライブラリ
        Dim libraryButton As CommonMasterFooterButton = commonMaster.GetFooterButton(SUBMENU_TCV_LIBRARY)
        AddHandler libraryButton.Click, AddressOf libraryButton_Click
        '見積もり
        Dim quotationButton As CommonMasterFooterButton = commonMaster.GetFooterButton(SUBMENU_TCV_QUOTATION)
        '選択状態
        quotationButton.Selected = True
        '非活性化
        quotationButton.Enabled = False

        '画面固有ボタン定義
        Dim BtnMitsumoriPreview As LinkButton = Me.MitsumoriprintButton
        Dim BtnKeiyakushoPrint As LinkButton = Me.KeiyakushoprintButton

        'ヘッダーボタンconfirm出力用
        InitHeaderEvent()

        'フッターボタンconfirm出力用
        InitFooterEvent()

        'ロード中表示用
        InitButtonEvent()

        If OperationLocked Then
            'ロック時

            'フッターメニュー
            'メニューボタンを非表示
            mainMenuButton.Visible = False
            '顧客ボタンを非表示
            'customerButton.Visible = False
            '契約書ボタン非表示
            BtnKeiyakushoPrint.Visible = False

            ' $99 Ken-Suzuki Add Start
            Me.chargeSegmentedButton.Enabled = False
            ' $99 Ken-Suzuki Add End
        Else
            '通常時

            'フッターメニュー
            'メニューボタンを表示
            mainMenuButton.Visible = True
            '顧客ボタンを表示
            'customerButton.Visible = True
            '契約書ボタン表示
            BtnKeiyakushoPrint.Visible = True

            ' $99 Ken-Suzuki Add Start
            Me.chargeSegmentedButton.Enabled = True
            ' $99 Ken-Suzuki Add End
        End If


        If (Not Me.IsPostBack AndAlso Not Me.IsCallback) Then



            '初期化
            Me.blnInputChangedClientHiddenField.Value = False

            '初期表示

            '所有者/使用者セグメントボタン表示
            With custClassSegmentedButton
                .Items.Add(New ListItem(WebWordUtility.GetWord(5), "1"))
                .Items.Add(New ListItem(WebWordUtility.GetWord(6), "2"))
            End With
            '初期選択
            custClassSegmentedButton.SelectedValue = "1"


            '現金/ローンセグメントボタン表示
            With payMethodSegmentedButton
                .Items.Add(New ListItem(WebWordUtility.GetWord(43), "1"))
                .Items.Add(New ListItem(WebWordUtility.GetWord(44), "2"))
            End With
            '初期選択
            payMethodSegmentedButton.SelectedValue = "1"

            '$99 Ken-Suzuki Add Start
            '販売店/個人セグメントボタン表示
            With chargeSegmentedButton
                .Items.Add(New ListItem(WebWordUtility.GetWord(73), "1"))
                .Items.Add(New ListItem(WebWordUtility.GetWord(74), "2"))
            End With
            '初期選択
            chargeSegmentedButton.SelectedValue = "1"
            '$99 Ken-Suzuki Add End

            'セッション情報取得
            Dim lngEstimateId As Long               '見積管理ID
            Dim blnLockStatus As Boolean            'ロック状態
            Dim blnNewActFlag As Boolean            '未保存フラグ

            lngEstimateId = CType(Me.GetValue(ScreenPos.Current, "EstimateId", False), Long)

            blnLockStatus = Me.GetValue(ScreenPos.Current, "MenuLockFlag", False)

            blnNewActFlag = Me.GetValue(ScreenPos.Current, "NewActFlag", False)


            'HIDDEN値設定
            Me.lngEstimateIdHiddenField.Value = CType(lngEstimateId, String)

            Me.blnNewActFlagHiddenField.Value = CType(blnNewActFlag, String)


            Dim bizLogic As SC3070201BusinessLogic

            'ビジネスロジックオブジェクト作成
            bizLogic = New SC3070201BusinessLogic



            '初期表示データ取得（API使用）

            Dim dsEstimation As IC3070201DataSet    '見積情報格納用

            dsEstimation = New IC3070201DataSet

            '見積情報データテーブル作成
            Dim dtEstimateData As New SC3070201DataSet.SC3070201ESTIMATEDATADataTable
            Dim drEstimateData As DataRow = dtEstimateData.NewRow

            drEstimateData("ESTIMATEID") = lngEstimateId
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


                '車両購入税比率取得
                Dim drEstVclTaxRatio As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow

                drEstVclTaxRatio = bizLogic.GetEstimateVehicleTaxRatio()

                ' $99 Ken-Suzuki Add Start
                Me.estVclTaxRatioHiddenField.Value = drEstVclTaxRatio.PARAMVALUE
                ' $99 Ken-Suzuki Add End

                'メモ最大桁数取得
                Dim drEstMemoMax As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow

                drEstMemoMax = bizLogic.GetMemoMax()

                Me.memoMaxHiddenField.Value = drEstMemoMax.PARAMVALUE

                '初期表示データ取得
                Dim dsEstimateExtraData As SC3070201DataSet

                '見積情報データテーブル更新
                dtEstimateData.Clear()

                drEstimateData("ESTIMATEID") = lngEstimateId
                drEstimateData("DLRCD") = dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("DLRCD")
                dtEstimateData.Rows.Add(drEstimateData)


                dsEstimateExtraData = bizLogic.GetInitialData(dtEstimateData, dsEstimation)


                '氏名敬称取得

                '敬称の設定値を取得
                Dim dtSysEnvSet As SC3070201DataSet.SC3070201SYSTEMENVSETTINGDataTable
                Dim sysenvDataTbl As New SC3070201DataSet.SC3070201SYSTEMENVSETTINGDataTable
                Dim sysenvDataRow As SC3070201DataSet.SC3070201SYSTEMENVSETTINGRow
                sysenvDataRow = sysenvDataTbl.NewSC3070201SYSTEMENVSETTINGRow
                sysenvDataTbl.Rows.Add(sysenvDataRow)

                dtSysEnvSet = bizLogic.GetNameTitleSysenv(sysenvDataTbl)


                '画面に取得した値を設定

                '■作成日/契約日
                If dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("CONTRACTFLG") = STR_CONTRACTFLG_COMP Then
                    '契約済のとき

                    Me.estPrintDateLabel.Visible = False
                    Me.dateLabel.Text = DateTimeFunc.FormatDate(3, dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("CONTRACTDATE"))

                    ' $99 Ken-Suzuki Add Start
                    Me.chargeSegmentedButton.Enabled = False
                    ' $99 Ken-Suzuki Add End
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

                End If

                '敬称
                If String.Equals(dtSysEnvSet.Rows(0).Item("NAMETITLEPOSITION"), STR_NAMETITLE_MAE) Then
                    Me.shoyusyaKeisyoMaeLabel.Text = dtSysEnvSet.Rows(0).Item("DEFOLTNAMETITLE")
                    Me.shiyosyaKeisyoMaeLabel.Text = dtSysEnvSet.Rows(0).Item("DEFOLTNAMETITLE")
                ElseIf String.Equals(dtSysEnvSet.Rows(0).Item("NAMETITLEPOSITION"), STR_NAMETITLE_ATO) Then

                    Me.shoyusyaKeisyoMaeLabel.Visible = False
                    Me.shiyosyaKeisyoMaeLabel.Visible = False
                    Me.shoyusyaKeisyoAtoLabel.Text = dtSysEnvSet.Rows(0).Item("DEFOLTNAMETITLE")
                    Me.shiyosyaKeisyoAtoLabel.Text = dtSysEnvSet.Rows(0).Item("DEFOLTNAMETITLE")
                End If


                '保険会社リスト作成
                Dim intI As Integer
                Dim InsComInsuComCd As New StringBuilder
                Dim InsComInsuKubun As New StringBuilder
                Dim InsComInsuComName As New StringBuilder

                For intI = 0 To dsEstimateExtraData.Tables("SC3070201ESTINSUCOMMAST").Rows.Count - 1
                    InsComInsuComCd.Append(dsEstimateExtraData.Tables("SC3070201ESTINSUCOMMAST").Rows(intI).Item("INSUCOMCD"))
                    InsComInsuKubun.Append(dsEstimateExtraData.Tables("SC3070201ESTINSUCOMMAST").Rows(intI).Item("INSUDVS"))
                    InsComInsuComName.Append(dsEstimateExtraData.Tables("SC3070201ESTINSUCOMMAST").Rows(intI).Item("INSUCOMNM"))
                    If intI <> dsEstimateExtraData.Tables("SC3070201ESTINSUCOMMAST").Rows.Count - 1 Then
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

                For intJ = 0 To dsEstimateExtraData.Tables("SC3070201ESTINSUKINDMAST").Rows.Count - 1
                    InsKindInsuComCd.Append(dsEstimateExtraData.Tables("SC3070201ESTINSUKINDMAST").Rows(intJ).Item("INSUCOMCD"))
                    InsKindInsuKindCd.Append(dsEstimateExtraData.Tables("SC3070201ESTINSUKINDMAST").Rows(intJ).Item("INSUKIND"))
                    InsKindInsuKindNm.Append(dsEstimateExtraData.Tables("SC3070201ESTINSUKINDMAST").Rows(intJ).Item("INSUKINDNM"))
                    If intJ <> dsEstimateExtraData.Tables("SC3070201ESTINSUKINDMAST").Rows.Count - 1 Then
                        InsKindInsuComCd.Append(",")
                        InsKindInsuKindCd.Append(",")
                        InsKindInsuKindNm.Append(",")
                    End If
                Next
                Me.InsKindInsuComCdHidden.Value = InsKindInsuComCd.ToString
                Me.InsKindInsuKindCdHidden.Value = InsKindInsuKindCd.ToString
                Me.InsKindInsuKindNmHidden.Value = InsKindInsuKindNm.ToString


                'データソース設定
                loanFinanceComRepeater.DataSource = dsEstimateExtraData.Tables("SC3070201FINANCECOMMAST")
                loanFinanceComRepeater.DataBind()


                '下取り車両件数 HIDDEN値設定
                Me.tradeInCarCountHiddenField.Value = dsEstimation.Tables("IC3070201TradeincarInfo").Rows.Count()

                '画面表示項目設定

                If dsEstimation.Tables("IC3070201CustomerInfo").Rows.Count = INT_CUSTOMERCOUNT_NEW Then
                    '見積り新規作成時

                    If Not IsDBNull(dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("CRCUSTID")) Then
                        ' ■見積／契約者情報
                        ' ■□所有者
                        If String.Equals(dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("CSTKIND"), STR_CUSTKIND_JISYA) Then

                            If dsEstimateExtraData.Tables("SC3070201ORGCUSTOMER").Rows.Count <> 0 Then
                                ' 自社客
                                ' □氏名

                                Me.shoyusyaNameTextBox.Text = Trim(dsEstimateExtraData.Tables("SC3070201ORGCUSTOMER").Rows(0).Item("NAME"))
                                ' □住所
                                Me.shoyusyaZipCodeTextBox.Text = Trim(dsEstimateExtraData.Tables("SC3070201ORGCUSTOMER").Rows(0).Item("ZIPCODE"))
                                Me.shoyusyaAddressTextBox.Text = Trim(dsEstimateExtraData.Tables("SC3070201ORGCUSTOMER").Rows(0).Item("ADDRESS"))
                                ' □連絡先
                                Me.shoyusyaMobileTextBox.Text = Trim(dsEstimateExtraData.Tables("SC3070201ORGCUSTOMER").Rows(0).Item("MOBILE"))
                                Me.shoyusyaTelTextBox.Text = Trim(dsEstimateExtraData.Tables("SC3070201ORGCUSTOMER").Rows(0).Item("TELNO"))
                                ' □E-Mail
                                Me.shoyusyaEmailTextBox.Text = Trim(dsEstimateExtraData.Tables("SC3070201ORGCUSTOMER").Rows(0).Item("EMAIL1"))
                                ' □国民ID
                                Me.shoyusyaIDTextBox.Text = Trim(dsEstimateExtraData.Tables("SC3070201ORGCUSTOMER").Rows(0).Item("SOCIALID"))
                                ' □顧客区分
                                If String.Equals(dsEstimateExtraData.Tables("SC3070201ORGCUSTOMER").Rows(0).Item("CUSTYPE"), STR_CUSTPART_KOJIN) Then
                                    Me.shoyusyaKojinCheckMark.Value = STR_TRUE
                                Else
                                    Me.shoyusyaHojinCheckMark.Value = STR_TRUE
                                End If


                            End If



                        Else

                            If dsEstimateExtraData.Tables("SC3070201NEWCUSTOMER").Rows.Count <> 0 Then
                                ' 未取引客
                                '□氏名
                                Me.shoyusyaNameTextBox.Text = Trim(dsEstimateExtraData.Tables("SC3070201NEWCUSTOMER").Rows(0).Item("NAME"))
                                '□住所
                                Me.shoyusyaZipCodeTextBox.Text = Trim(dsEstimateExtraData.Tables("SC3070201NEWCUSTOMER").Rows(0).Item("ZIPCODE"))
                                Me.shoyusyaAddressTextBox.Text = Trim(dsEstimateExtraData.Tables("SC3070201NEWCUSTOMER").Rows(0).Item("ADDRESS"))
                                '□連絡先
                                Me.shoyusyaMobileTextBox.Text = Trim(dsEstimateExtraData.Tables("SC3070201NEWCUSTOMER").Rows(0).Item("MOBILE"))
                                Me.shoyusyaTelTextBox.Text = Trim(dsEstimateExtraData.Tables("SC3070201NEWCUSTOMER").Rows(0).Item("TELNO"))
                                '□E-Mail
                                Me.shoyusyaEmailTextBox.Text = Trim(dsEstimateExtraData.Tables("SC3070201NEWCUSTOMER").Rows(0).Item("EMAIL1"))
                                '□国民ID
                                If Not IsDBNull(dsEstimateExtraData.Tables("SC3070201NEWCUSTOMER").Rows(0).Item("SOCIALID")) Then
                                    Me.shoyusyaIDTextBox.Text = Trim(dsEstimateExtraData.Tables("SC3070201NEWCUSTOMER").Rows(0).Item("SOCIALID"))
                                End If

                                '□顧客区分

                                If String.Equals(dsEstimateExtraData.Tables("SC3070201NEWCUSTOMER").Rows(0).Item("CUSTYPE"), STR_CUSTPART_KOJIN) Then
                                    Me.shoyusyaKojinCheckMark.Value = STR_TRUE
                                Else
                                    Me.shoyusyaHojinCheckMark.Value = STR_TRUE
                                End If
                            End If

                        End If
                    Else
                        '客がいない場合、初期選択
                        Me.shoyusyaKojinCheckMark.Value = STR_TRUE
                        Me.shiyosyaKojinCheckMark.Value = STR_TRUE
                        Me.jisyaCheckMark.Value = STR_TRUE
                    End If

                Else
                    '見積り保存後
                    Me.savedEstimationFlgHiddenField.Value = "1"
                    'NumericBox変更検知用
                    If Not IsDBNull(dsEstimation.Tables("IC3070201PaymentInfo").Rows(1).Item("PAYMENTPERIOD")) Then
                        Me.periodInitialValueHiddenField.Value = dsEstimation.Tables("IC3070201PaymentInfo").Rows(1).Item("PAYMENTPERIOD")
                    End If
                    If Not IsDBNull(dsEstimation.Tables("IC3070201PaymentInfo").Rows(1).Item("DUEDATE")) Then
                        Me.firstPayInitialValueHiddenField.Value = dsEstimation.Tables("IC3070201PaymentInfo").Rows(1).Item("DUEDATE")
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
                            Me.shoyusyaKojinCheckMark.Value = STR_TRUE
                        Else
                            Me.shoyusyaHojinCheckMark.Value = STR_TRUE
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
                            Me.shiyosyaKojinCheckMark.Value = STR_TRUE
                        Else
                            Me.shiyosyaHojinCheckMark.Value = STR_TRUE
                        End If
                    Next

                    ' ■諸費用
                    '$99 Ken-Suzuki Add Start
                    ' 車両購入税取得
                    Dim drCarBuyTax = dsEstimation.Tables("IC3070201ChargeInfo").Select(STR_GETCARBUYTAX)
                    For Each drCarBuyTaxRow As DataRow In drCarBuyTax

                        If IsDBNull(drCarBuyTaxRow.Item("PRICE")) Then
                            Me.CarBuyTaxCustomLabel.Text = "0"
                        Else
                            Me.CarBuyTaxCustomLabel.Text = drCarBuyTaxRow.Item("PRICE")
                            Me.carBuyTaxHiddenField.Value = drCarBuyTaxRow.Item("PRICE")
                        End If
                    Next
                    '$99 Ken-Suzuki Add End

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

                    '$99 Ken-Suzuki Add Start
                    '諸費用区分の初期選択
                    If dsEstimation.Tables("IC3070201ChargeInfo").Rows.Count > 0 Then
                        chargeSegmentedButton.SelectedValue = dsEstimation.Tables("IC3070201ChargeInfo").Rows(0).Item("CHARGEDVS")
                    End If
                    '$99 Ken-Suzuki Add End

                    '■保険

                    '□保険区分
                    If String.Equals(dsEstimation.Tables("IC3070201EstInsuranceInfo").Rows(0).Item("INSUDVS"), STR_INSUDVS_JISYA) Then
                        Me.jisyaCheckMark.Value = STR_TRUE
                    Else
                        Me.tasyaCheckMark.Value = STR_TRUE
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
                If Me.shoyusyaHojinCheckMark.Value = STR_TRUE Then
                    Me.shoyusyaKeisyoMaeLabel.Style.Item("display") = "none"
                    Me.shoyusyaKeisyoAtoLabel.Style.Item("display") = "none"
                End If
                ''□使用者
                If Me.shiyosyaHojinCheckMark.Value = STR_TRUE Then
                    Me.shiyosyaKeisyoMaeLabel.Style.Item("display") = "none"
                    Me.shiyosyaKeisyoAtoLabel.Style.Item("display") = "none"
                End If

                ' ■車両情報
                ' □車種

                'データソース設定
                vclInfoRepeater.DataSource = dsEstimation.Tables("IC3070201EstimationInfo")
                vclInfoRepeater.DataBind()


                '□外装追加費用
                If dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("EXTAMOUNT") <> 0 Then
                    Me.extOptionFlgHiddenField.Value = "1"
                    ' $99 Ken-Suzuki Modify Start
                    'Me.extColorOptionNameLabel.Text = dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("EXTCOLOR")
                    'Me.extColorOptionPriceLabel.Text = dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("EXTAMOUNT")
                    'Me.extColorOptionPriceTotalLabel.Text = Me.extColorOptionPriceLabel.Text
                    Me.extOptionPriceHiddenField.Value = dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("EXTAMOUNT")
                    ' $99 Ken-Suzuki Modify End
                End If
                '□内装追加費用
                If dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("INTAMOUNT") <> 0 Then
                    Me.intOptionFlgHiddenField.Value = "1"
                    ' $99 Ken-Suzuki Modify Start
                    'Me.intColorOptionNameLabel.Text = dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("INTCOLOR")
                    'Me.intColorOptionPriceLabel.Text = dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("INTAMOUNT")
                    'Me.intColorOptionPriceTotalLabel.Text = Me.intColorOptionPriceLabel.Text
                    Me.intOptionPriceHiddenField.Value = dsEstimation.Tables("IC3070201EstimationInfo").Rows(0).Item("INTAMOUNT")
                    ' $99 Ken-Suzuki Modify End
                End If


                '□メーカーオプション
                Dim i As IEnumerable = From n In dsEstimation.Tables("IC3070201VclOptionInfo") _
                        Where n("OptionPart") = STR_OPTIONPART_MAKER

                mkrOptionRepeater.DataSource = i
                mkrOptionRepeater.DataBind()


                '□販売店オプション
                '販売店行取得
                Dim drDealer As DataRow()
                drDealer = dsEstimation.Tables("IC3070201VclOptionInfo").Select(STR_GETDLROPTION)
                Dim j As IEnumerable = From n In dsEstimation.Tables("IC3070201VclOptionInfo") _
                                        Where n("OptionPart") = STR_OPTIONPART_DEALER

                dlrOptionDataTable = j

                Me.dlrOptionCountHiddenField.Value = drDealer.Count()


                '□合計
                'javascriptにて表示


                '□車両画像

                '□車両画像
                If dsEstimateExtraData.Tables("SC3070201MODELPICTURE").Rows.Count = 0 Then
                    Me.carImgFileHidden.Value = ""
                Else
                    Me.carImgFileHidden.Value = ResolveClientUrl(dsEstimateExtraData.Tables("SC3070201MODELPICTURE").Rows(0).Item("IMAGEFILE"))
                End If


                '■諸費用
                Dim dcmExtOptPrice As Decimal
                dcmExtOptPrice = 0.0

                '□車両購入税

                ' $99 Ken-Suzuki Add Start
                ' 車両本体価格
                Dim dcmBasePrice As Decimal = Decimal.Parse(Me.basePriceHiddenField.Value)

                ' 外装色追加費用
                ' $99 Ken-Suzuki Add End
                If String.Equals(Me.extOptionFlgHiddenField.Value, "1") Then
                    dcmExtOptPrice = Decimal.Parse(Me.extOptionPriceHiddenField.Value)
                End If

                '$99 Ken-Suzuki Modify Start
                'Me.CarBuyTaxCustomLabel.Text = (Decimal.Parse(Me.basePriceHiddenField.Value) + dcmExtOptPrice) * drEstVclTaxRatio.PARAMVALUE

                ' 内装色追加費用
                Dim dcmIntOptPrice As Decimal = 0.0
                If String.Equals(Me.intOptionFlgHiddenField.Value, "1") Then
                    '$99 Ken-Suzuki Modify Start
                    'dcmIntOptPrice = Decimal.Parse(Me.intColorOptionPriceLabel.Text)
                    dcmIntOptPrice = Decimal.Parse(Me.intOptionPriceHiddenField.Value)
                    '$99 Ken-Suzuki Modify End
                End If

                ' 値引き額
                Dim dcmDiscountPrice As Decimal = 0.0
                If Not String.IsNullOrEmpty(Me.discountPriceValueHiddenField.Value) Then
                    dcmDiscountPrice = Decimal.Parse(Me.discountPriceValueHiddenField.Value)
                End If

                ' 車両購入税（計算結果） = (車両本体価格 + 外装色追加費用 + 内装色追加費用 - 値引き額) × 車両購入税率
                Dim carBuyTax As Decimal =
                    (dcmBasePrice + dcmExtOptPrice + dcmIntOptPrice - dcmDiscountPrice) * drEstVclTaxRatio.PARAMVALUE

                ' 車両購入税（最低価格） = マスタより取得
                Dim carBuyTaxMast As Decimal = 0.0
                If dsEstimateExtraData.Tables("SC3070201VclPurchaseTaxMast").Rows.Count > 0 Then
                    carBuyTaxMast = dsEstimateExtraData.Tables("SC3070201VclPurchaseTaxMast").Rows(0).Item("MINIMUMPRICE")
                End If
                Me.carBuyTaxMastHiddenField.Value = carBuyTaxMast

                ' 車両購入税（計算結果） ＜ 車両購入税（最低価格） の場合は車両購入税（最低価格）を採用
                If carBuyTax < carBuyTaxMast Then
                    carBuyTax = carBuyTaxMast
                End If

                ' 小数点以下の切り捨て
                carBuyTax = Math.Floor(carBuyTax)

                ' 車両購入税（初期値）
                Me.carBuyDefaultTaxHiddenField.Value = carBuyTax

                ' 車両購入税（表示値）
                If Me.chargeSegmentedButton.SelectedValue = "2" Then
                    carBuyTax = 0.0
                End If

                If Not String.IsNullOrEmpty(Me.carBuyTaxHiddenField.Value) Then
                    ' DBの値と表示値が異なる場合は入力内容変更フラグを立てる
                    If carBuyTax <> Decimal.Parse(Me.carBuyTaxHiddenField.Value) Then
                        Me.inputChanged()
                    End If
                End If

                Me.CarBuyTaxCustomLabel.Text = carBuyTax
                Me.carBuyTaxHiddenField.Value = carBuyTax
                '$99 Ken-Suzuki Modify End

                ' ■お支払い金額

                ' □下取り車両
                tradeInCarDataTable = dsEstimation.Tables("IC3070201TradeincarInfo")


            End If

            'クライアント側使用文言 HIDDEN値設定
            Me.shoyusyaNameMsgHiddenField.Value = WebWordUtility.GetWord(901)
            Me.shoyusyaZipcodeMsgHiddenField.Value = WebWordUtility.GetWord(902)
            Me.shoyusyaAddressMsgHiddenField.Value = WebWordUtility.GetWord(903)
            Me.shoyusyaIdMsgHiddenField.Value = WebWordUtility.GetWord(904)
            Me.shiyosyaNameMsgHiddenField.Value = WebWordUtility.GetWord(905)
            Me.shiyosyaZipcodeMsgHiddenField.Value = WebWordUtility.GetWord(906)
            Me.shiyosyaAddressMsgHiddenField.Value = WebWordUtility.GetWord(907)
            Me.shiyosyaIdMsgHiddenField.Value = WebWordUtility.GetWord(908)
            Me.regPriceHiddenField.Value = WebWordUtility.GetWord(936)
            Me.minusLabelHiddenField.Value = WebWordUtility.GetWord(58)
            Me.optionPriceMsgHiddenField.Value = WebWordUtility.GetWord(924)
            Me.optionInstallFeeMsgHiddenField.Value = WebWordUtility.GetWord(925)
            Me.regFeeMsgHiddenField.Value = WebWordUtility.GetWord(956)
            Me.insuranceFeeMsgHiddenField.Value = WebWordUtility.GetWord(926)
            Me.cashDownMsgHiddenField.Value = WebWordUtility.GetWord(927)
            Me.loanMonthlyPayMsgHiddenField.Value = WebWordUtility.GetWord(929)
            Me.loanDownMsgHiddenField.Value = WebWordUtility.GetWord(930)
            Me.loanBonusMsgHiddenField.Value = WebWordUtility.GetWord(931)
            Me.discountMsgHiddenField.Value = WebWordUtility.GetWord(933)
            Me.tradeInPriceMsgHiddenField.Value = WebWordUtility.GetWord(955)
            Me.inputDataDeleteMsgHiddenField.Value = WebWordUtility.GetWord(935)
            Me.customerDeleteMsgHiddenField.Value = WebWordUtility.GetWord(937)
            Me.numericKeyPadCancelHiddenField.Value = WebWordUtility.GetWord(71)
            Me.numericKeyPadDoneHiddenField.Value = WebWordUtility.GetWord(72)


        Else
            '契約モード
            If Me.actionModeHiddenField.Value = STR_ACTIONMODE_CONTRACT Then

                '契約書印刷画面遷移メソッド
                gotoPrintContract()

            End If


            '販売店オプション欄復元用
            Dim dtDlrOption As New DataTable
            dtDlrOption.Columns.Add("OPTIONNAME")
            dtDlrOption.Columns.Add("PRICE")
            dtDlrOption.Columns.Add("INSTALLCOST")

            Dim drDlrOption As DataRow
            Dim intCount As Integer

            For intCount = 1 To Integer.Parse(Me.dlrOptionCountHiddenField.Value)

                drDlrOption = dtDlrOption.NewRow
                drDlrOption.Item("OPTIONNAME") = Request.Form.Item(String.Concat("optionNameText", intCount))
                drDlrOption.Item("PRICE") = Request.Form.Item(String.Concat("optionPriceText", intCount))
                drDlrOption.Item("INSTALLCOST") = Request.Form.Item(String.Concat("optionMoneyText", intCount))
                dtDlrOption.Rows.Add(drDlrOption)

            Next

            dlrOptionDataTable = dtDlrOption.AsEnumerable()


            '下取り車両欄復元用
            Dim dtTradeInCar As New DataTable
            dtTradeInCar.Columns.Add("VEHICLENAME")
            dtTradeInCar.Columns.Add("ASSESSEDPRICE")

            Dim drTradeInCar As DataRow
            Dim intCarCount As Integer

            For intCarCount = 1 To Integer.Parse(Me.tradeInCarCountHiddenField.Value)

                drTradeInCar = dtTradeInCar.NewRow
                drTradeInCar.Item("VEHICLENAME") = Request.Form.Item(String.Concat("tradeInCarText", intCarCount))
                drTradeInCar.Item("ASSESSEDPRICE") = Request.Form.Item(String.Concat("tradeInCarPrice", intCarCount))
                dtTradeInCar.Rows.Add(drTradeInCar)

            Next

            tradeInCarDataTable = dtTradeInCar.AsEnumerable()

            '金額欄復元用
            Me.regPriceTextBox.Text = Me.regCostValueHiddenField.Value
            ' $99 Ken-Suzuki Add Start
            Me.CarBuyTaxCustomLabel.Text = Me.carBuyTaxHiddenField.Value
            ' $99 Ken-Suzuki Add End
            Me.insuranceAmountTextBox.Text = Me.insuAmountValueHiddenField.Value
            Me.cashDepositTextBox.Text = Me.cashDepositValueHiddenField.Value
            Me.loanMonthlyPayTextBox.Text = Me.loanMonthlyValueHiddenField.Value
            Me.loanDepositTextBox.Text = Me.loanDepositValueHiddenField.Value
            Me.loanBonusPayTextBox.Text = Me.loanBonusValueHiddenField.Value
            Me.discountPriceTextBox.Text = Me.discountPriceValueHiddenField.Value


        End If

        If Me.contractFlgHiddenField.Value = STR_CONTRACTFLG_COMP Then
            '契約実行後

            '参照モード設定
            Me.ReferenceModeHiddenField.Value = STR_TRUE
            '見積ボタン非表示
            BtnMitsumoriPreview.Visible = False

            ' $99 Ken-Suzuki Add Start
            Me.chargeSegmentedButton.Enabled = False
            ' $99 Ken-Suzuki Add End
        End If

        If String.Equals(Me.lngFollowupBoxSeqNoHiddenField.Value, "") Then
            '活動がない場合

            '契約ボタン非表示
            BtnKeiyakushoPrint.Visible = False

        End If

        If (Me.ReferenceModeHiddenField.Value).ToUpper() = STR_TRUE Then
            '参照モード時

            '値コピー
            '見積／契約者情報
            '■□所有者
            Me.shoyusyaNameLabel.Text = Me.shoyusyaNameTextBox.Text
            Me.shoyusyaZipCodeLabel.Text = Me.shoyusyaZipCodeTextBox.Text
            Me.shoyusyaAddressLabel.Text = Me.shoyusyaAddressTextBox.Text
            Me.shoyusyaMobileLabel.Text = Me.shoyusyaMobileTextBox.Text
            Me.shoyusyaTelLabel.Text = Me.shoyusyaTelTextBox.Text
            Me.shoyusyaEmailLabel.Text = Me.shoyusyaEmailTextBox.Text
            Me.shoyusyaIDLabel.Text = Me.shoyusyaIDTextBox.Text


            '■□使用者
            Me.shiyosyaNameLabel.Text = Me.shiyosyaNameTextBox.Text
            Me.shiyosyaZipCodeLabel.Text = Me.shiyosyaZipCodeTextBox.Text
            Me.shiyosyaAddressLabel.Text = Me.shiyosyaAddressTextBox.Text
            Me.shiyosyaMobileLabel.Text = Me.shiyosyaMobileTextBox.Text
            Me.shiyosyaTelLabel.Text = Me.shiyosyaTelTextBox.Text
            Me.shiyosyaEmailLabel.Text = Me.shiyosyaEmailTextBox.Text
            Me.shiyosyaIDLabel.Text = Me.shiyosyaIDTextBox.Text

            '■諸費用
            Me.regPriceLabel.Text = Me.regCostValueHiddenField.Value

            ' $99 Ken-Suzuki Add Start
            '■車両登録税
            Me.CarBuyTaxCustomLabel.Text = Me.carBuyTaxHiddenField.Value
            ' $99 Ken-Suzuki Add End

            '■保険
            Me.insuComLabel.Text = Request.Form.Item("insuComSelect")
            Me.insuComKindLabel.Text = Request.Form.Item("insuComKindSelect")

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



            '■お支払い金額
            '□値引き額

            Me.discountPriceLabel.Text = Me.discountPriceValueHiddenField.Value

            '□納車日
            If Not (Me.deliDateDateTimeSelector.Value Is Nothing) Then
                Me.deliDateLabel.Text = DateTimeFunc.FormatDate(3, Me.deliDateDateTimeSelector.Value)
                '入力変更検知用
                If Me.initialFlgHiddenField.Value = "" Then
                    Me.deliDateInitialValueHiddenField.Value = DateTimeFunc.FormatDate(3, Me.deliDateDateTimeSelector.Value)
                    Me.initialFlgHiddenField.Value = "1"
                End If
                Me.deliDateAfterValueHiddenField.Value = DateTimeFunc.FormatDate(3, Me.deliDateDateTimeSelector.Value)
            End If


            '非活性化

            Me.memoTextBox.Enabled = False

            Me.popOver1.Visible = False

        Else
            '通常時

            '活性化
            Me.memoTextBox.Enabled = True

            Me.popOver1.Visible = True


        End If


    End Sub


    ' ''' <summary>
    ' ''' サーバ側入力チェックを実施し、見積情報を保存する。
    ' ''' </summary>
    ' ''' <param name="sender">イベント発生元</param>
    ' ''' <param name="e">イベントデータ</param>
    ' ''' <remarks></remarks>
    Private Sub saveEstimation_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles saveLinkButton.Click


        '入力チェック（必須以外）
        If Not checkInputFormat() Then
            Exit Sub
        End If

        '見積登録情報用データセット
        Dim dsRegEstimation As IC3070202DataSet

        '見積情報登録用データセット作成
        dsRegEstimation = createEstimateDataSet()

        Dim bizLogic As SC3070201BusinessLogic      'ビジネスロジックオブジェクト
        Dim blnResult As IC3070202DataSet.IC3070202EstResultDataTable       '戻り値


        'ビジネスロジックオブジェクト作成
        bizLogic = New SC3070201BusinessLogic

        '見積情報登録
        blnResult = bizLogic.UpdateEstimation(dsRegEstimation)

        '保存済みフラグ
        Me.savedEstimationFlgHiddenField.Value = "1"

        '入力内容変更フラグ
        Me.blnInputChangedClientHiddenField.Value = False
        '初期化
        Me.deliDateInitialValueHiddenField.Value = ""

        'CREATEDATE対応（STEP1.5以降に使用予定）
        Me.createDateHiddenField.Value = blnResult.Rows(0).Item("CreateDate")

        'オブジェクト開放
        bizLogic = Nothing

        Return

    End Sub


    '''' <summary>
    '''' 見積書印刷画面へ遷移
    '''' </summary>
    '''' <param name="sender">イベント発生元</param>
    '''' <param name="e">イベントデータ</param>
    '''' <remarks></remarks>
    Private Sub gotoPrintEstimation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MitsumoriprintButton.Click

        'HIDDEN値取得
        Dim lngEstimateId As Long               '見積管理ID

        lngEstimateId = Long.Parse(Me.lngEstimateIdHiddenField.Value)


        '見積もり初回または入力に変更がある場合
        If (Me.savedEstimationFlgHiddenField.Value = "0") Or (Me.blnInputChangedClientHiddenField.Value = STR_TRUE) Then

            ''入力チェック（必須以外）
            If Not checkInputFormat() Then
                Exit Sub
            End If


            ''見積情報登録
            Dim dsRegEstimation As IC3070202DataSet     '見積情報登録用データセット

            '見積情報登録用データセット作成
            dsRegEstimation = createEstimateDataSet()

            Dim bizLogic As SC3070201BusinessLogic      'ビジネスロジックオブジェクト

            Dim blnResult As IC3070202DataSet.IC3070202EstResultDataTable       '戻り値

            bizLogic = New SC3070201BusinessLogic

            '見積情報登録
            blnResult = bizLogic.UpdateEstimation(dsRegEstimation)

            ''保存済みフラグ
            Me.savedEstimationFlgHiddenField.Value = "1"

            'CREATEDATE対応（STEP1.5以降に使用予定）
            Me.createDateHiddenField.Value = blnResult.Rows(0).Item("CreateDate")

            'オブジェクト開放
            bizLogic = Nothing

        End If

        'セッション情報格納

        MyBase.SetValue(ScreenPos.Current, "MenuLockFlag", OperationLocked)
        MyBase.SetValue(ScreenPos.Current, "paymentMethod", Me.payMethodSegmentedButton.SelectedItem.Value)

        MyBase.SetValue(ScreenPos.Next, "estimateId", lngEstimateId)                                        '見積管理ID
        MyBase.SetValue(ScreenPos.Next, "paymentMethod", Me.payMethodSegmentedButton.SelectedItem.Value)    '表示している支払方法区分(1:現金、2:ローン)

        '画面遷移
        Me.RedirectNextScreen(STR_DISPID_QUOTATIONPREVIEW)

    End Sub


    '''' <summary>
    '''' 契約書印刷画面へ遷移
    '''' </summary>
    '''' <param name="sender">イベント発生元</param>
    '''' <param name="e">イベントデータ</param>
    '''' <remarks></remarks>
    Private Sub gotoPrintContract()

        '契約済みでないとき
        If Me.contractFlgHiddenField.Value <> 1 Then

            '入力チェック
            If Not checkInputMandatory() Then
                '必須入力チェックエラー
                Exit Sub

            ElseIf Not checkInputFormat() Then
                '必須以外入力チェックエラー
                Exit Sub

            End If


            '入力に変更がある場合
            If Me.blnInputChangedClientHiddenField.Value = STR_TRUE Then

                ''見積情報登録
                Dim dsRegEstimation As IC3070202DataSet     '見積情報登録用データセット

                '見積情報登録用データセット作成
                dsRegEstimation = createEstimateDataSet()

                Dim bizLogic As SC3070201BusinessLogic      'ビジネスロジックオブジェクト

                Dim blnResult As IC3070202DataSet.IC3070202EstResultDataTable       '戻り値

                bizLogic = New SC3070201BusinessLogic

                '見積情報登録
                blnResult = bizLogic.UpdateEstimation(dsRegEstimation)

                ''保存済みフラグ
                Me.savedEstimationFlgHiddenField.Value = "1"

                'CREATEDATE対応（STEP1.5以降に使用予定）
                Me.createDateHiddenField.Value = blnResult.Rows(0).Item("CreateDate")

                'オブジェクト開放
                bizLogic = Nothing
            End If

        End If


        'HIDDEN値取得
        Dim lngEstimateId As Long               '見積管理ID
        lngEstimateId = Long.Parse(Me.lngEstimateIdHiddenField.Value)


        'セッション情報格納
        MyBase.SetValue(ScreenPos.Current, "paymentMethod", Me.payMethodSegmentedButton.SelectedItem.Value)

        MyBase.SetValue(ScreenPos.Next, "estimateId", lngEstimateId)                                        '見積管理ID
        MyBase.SetValue(ScreenPos.Next, "paymentMethod", Me.payMethodSegmentedButton.SelectedItem.Value)    '表示している支払方法区分(1:現金、2:ローン)
        '0118 Matsumoto Add Start
        MyBase.SetValue(ScreenPos.Current, "MenuLockFlag", OperationLocked)
        '0118 Matsumoto Add End

        '画面遷移
        Me.RedirectNextScreen(STR_DISPID_CONTRACTPREVIEW)

    End Sub


    '''' <summary>
    '''' 入力チェックを実施する（必須）
    '''' </summary>
    '''' <remarks></remarks>
    Private Function checkInputMandatory() As Boolean



        '■見積／契約者情報
        '□所有者欄
        If String.IsNullOrEmpty(shoyusyaNameTextBox.Text) Then
            '氏名（所有者）が未入力の場合
            Me.actionModeHiddenField.Value = ""
            MyBase.ShowMessageBox(901)

            Return False

        ElseIf String.IsNullOrEmpty(shoyusyaZipCodeTextBox.Text) Then
            '郵便番号（所有者）が未入力の場合
            Me.actionModeHiddenField.Value = ""
            MyBase.ShowMessageBox(902)

            Return False

        ElseIf String.IsNullOrEmpty(shoyusyaAddressTextBox.Text) Then
            '住所（所有者）が未入力の場合
            Me.actionModeHiddenField.Value = ""
            MyBase.ShowMessageBox(903)

            Return False

        ElseIf String.IsNullOrEmpty(shoyusyaMobileTextBox.Text) And String.IsNullOrEmpty(shoyusyaTelTextBox.Text) Then
            '携帯（所有者）、電話（所有者）いずれも未入力
            Me.actionModeHiddenField.Value = ""
            MyBase.ShowMessageBox(938)

            Return False

        ElseIf String.IsNullOrEmpty(shoyusyaIDTextBox.Text) Then
            'ID（所有者）が未入力の場合
            Me.actionModeHiddenField.Value = ""
            MyBase.ShowMessageBox(904)

            Return False

            '□使用者欄
        ElseIf String.IsNullOrEmpty(shiyosyaNameTextBox.Text) Then
            '氏名（使用者）が未入力の場合
            Me.actionModeHiddenField.Value = ""
            MyBase.ShowMessageBox(905)

            Return False

        ElseIf String.IsNullOrEmpty(shiyosyaZipCodeTextBox.Text) Then
            '郵便番号（使用者）が未入力の場合
            Me.actionModeHiddenField.Value = ""
            MyBase.ShowMessageBox(906)

            Return False

        ElseIf String.IsNullOrEmpty(shiyosyaAddressTextBox.Text) Then
            '住所（使用者）が未入力の場合
            Me.actionModeHiddenField.Value = ""
            MyBase.ShowMessageBox(907)

            Return False

        ElseIf String.IsNullOrEmpty(shiyosyaMobileTextBox.Text) And String.IsNullOrEmpty(shiyosyaTelTextBox.Text) Then
            '携帯（使用者）、電話（使用者）いずれも未入力
            Me.actionModeHiddenField.Value = ""
            MyBase.ShowMessageBox(939)

            Return False

        ElseIf String.IsNullOrEmpty(shiyosyaIDTextBox.Text) Then
            'ID（使用者）が未入力の場合
            Me.actionModeHiddenField.Value = ""
            MyBase.ShowMessageBox(908)

            Return False



            '■諸費用欄
        ElseIf String.IsNullOrEmpty(regPriceTextBox.Text) Then
            '登録費用が未入力の場合
            Me.actionModeHiddenField.Value = ""
            MyBase.ShowMessageBox(936)

            Return False

            '■保険欄
            '$99 01/16 modify start kassai 
            'ElseIf (Me.SelectInsuComCdHidden.Value = "") And (Not String.IsNullOrEmpty(insuranceAmountTextBox.Text)) Then
        ElseIf (Me.SelectInsuComCdHidden.Value = "") And (Not String.IsNullOrEmpty(insuAmountValueHiddenField.Value)) Then
            '$99 01/16 modify end kassai 

            '保険金額が入力されており、保険会社が未選択の場合
            Me.actionModeHiddenField.Value = ""
            MyBase.ShowMessageBox(943)
            Return False

        ElseIf (Me.SelectInsuKindCdHidden.Value = "") And (Me.SelectInsuComCdHidden.Value <> "") Then
            '保険会社が選択されており、保険種別が未選択の場合
            Me.actionModeHiddenField.Value = ""
            MyBase.ShowMessageBox(944)
            Return False

            'ElseIf String.IsNullOrEmpty(insuranceAmountTextBox.Text) And (Me.SelectInsuComCdHidden.Value <> "") Then
        ElseIf String.IsNullOrEmpty(insuAmountValueHiddenField.Value) And (Me.SelectInsuComCdHidden.Value <> "") Then
            '保険会社が選択されており、保険金額が未入力の場合
            Me.actionModeHiddenField.Value = ""
            MyBase.ShowMessageBox(945)

            Return False


            '■お支払い方法欄
            '□現金
        ElseIf String.IsNullOrEmpty(cashDepositValueHiddenField.Value) And (Me.payMethodSegmentedButton.SelectedItem.Value = 1) Then
            'お支払い方法に現金が選択されており、頭金（現金）が未入力の場合
            Me.actionModeHiddenField.Value = ""
            MyBase.ShowMessageBox(946)

            Return False

            '□ローン
        ElseIf (Me.SelectFinanceComHiddenField.Value = "") And (Me.payMethodSegmentedButton.SelectedItem.Value = 2) Then
            'お支払い方法にローンが選択されており、融資会社が未選択の場合
            Me.actionModeHiddenField.Value = ""
            MyBase.ShowMessageBox(947)
            Return False

        ElseIf Me.loanPayPeriodNumericBox.Value Is Nothing And (Me.payMethodSegmentedButton.SelectedItem.Value = 2) Then
            'お支払い方法にローンが選択されており、期間が未入力の場合
            Me.actionModeHiddenField.Value = ""
            MyBase.ShowMessageBox(948)

            Return False

            'ElseIf String.IsNullOrEmpty(Me.loanMonthlyPayTextBox.Text) And (Me.payMethodSegmentedButton.SelectedItem.Value = 2) Then
        ElseIf String.IsNullOrEmpty(Me.loanMonthlyValueHiddenField.Value) And (Me.payMethodSegmentedButton.SelectedItem.Value = 2) Then
            'お支払い方法にローンが選択されており、月額が未入力の場合
            Me.actionModeHiddenField.Value = ""
            MyBase.ShowMessageBox(949)

            Return False

            'ElseIf String.IsNullOrEmpty(Me.loanDepositTextBox.Text) And (Me.payMethodSegmentedButton.SelectedItem.Value = 2) Then
        ElseIf String.IsNullOrEmpty(Me.loanDepositValueHiddenField.Value) And (Me.payMethodSegmentedButton.SelectedItem.Value = 2) Then
            'お支払い方法にローンが選択されており、頭金（ローン）が未入力の場合
            Me.actionModeHiddenField.Value = ""
            MyBase.ShowMessageBox(950)

            Return False

        ElseIf Me.loanDueDateNumericBox.Value Is Nothing And (Me.payMethodSegmentedButton.SelectedItem.Value = 2) Then
            'お支払い方法にローンが選択されており、初回支払いが未入力の場合
            Me.actionModeHiddenField.Value = ""
            MyBase.ShowMessageBox(951)

            Return False

        End If

        Return True

    End Function


    '''' <summary>
    '''' 入力チェックを実施する（必須以外）
    '''' </summary>
    '''' <remarks></remarks>
    Private Function checkInputFormat() As Boolean

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
        For intCount = 1 To Integer.Parse(Me.dlrOptionCountHiddenField.Value)

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

            ElseIf Not Validation.IsCorrectPattern(Request.Form.Item(String.Concat("optionPriceText", intCount)), STR_MONEYFORMAT) And Not String.IsNullOrEmpty(Request.Form.Item(String.Concat("optionPriceText", intCount))) Then
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

        '■諸費用欄
        If Not Validation.IsCorrectPattern(regPriceTextBox.Text, STR_MONEYFORMAT) And Not String.IsNullOrEmpty(regPriceTextBox.Text) Then
            '登録費用の書式が誤り
            '（整数9桁以内、小数点以下2桁以外の場合）
            Me.actionModeHiddenField.Value = ""
            MyBase.ShowMessageBox(956)

            Return False
        End If

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
        End If


        '□下取り車両
        For intCarCount = 1 To Integer.Parse(Me.tradeInCarCountHiddenField.Value)

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

        ElseIf Not Validation.IsCorrectDigit(memoTextBox.Text, Integer.Parse(memoMaxHiddenField.Value)) And Not String.IsNullOrEmpty(memoTextBox.Text) Then
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



        Return True

    End Function


    '''' <summary>
    '''' 見積情報登録用データセットを作成する。
    '''' </summary>
    '''' <remarks></remarks>
    Private Function createEstimateDataSet() As IC3070202DataSet

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

        Dim dsEstimation As IC3070201DataSet        '見積取得情報
        dsEstimation = CType(ViewState("DataSetEstimation"), IC3070201DataSet)


        dtEstimationInfo.Merge(dsEstimation.IC3070201EstimationInfo)
        dtCustomerInfo.Merge(dsEstimation.IC3070201CustomerInfo)
        dtVclOptionInfo.Merge(dsEstimation.IC3070201VclOptionInfo)
        dtChargeInfo.Merge(dsEstimation.IC3070201ChargeInfo)
        dtPayInfo.Merge(dsEstimation.IC3070201PaymentInfo)
        dtTradeInCarInfo.Merge(dsEstimation.IC3070201TradeincarInfo)
        dtInsuranceInfo.Merge(dsEstimation.IC3070201EstInsuranceInfo)


        '■見積情報データテーブル


        '納車予定日
        If Me.deliDateDateTimeSelector.Value Is Nothing Then
            dtEstimationInfo.Rows(0).Item("DeliDate") = DBNull.Value
        Else
            dtEstimationInfo.Rows(0).Item("DeliDate") = Me.deliDateDateTimeSelector.Value
        End If

        '値引き額
        If String.IsNullOrEmpty(Me.discountPriceValueHiddenField.Value) Then
            dtEstimationInfo.Rows(0).Item("DiscountPrice") = DBNull.Value
        Else
            dtEstimationInfo.Rows(0).Item("DiscountPrice") = Double.Parse(Me.discountPriceValueHiddenField.Value)

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
            drRegCustomerSyoyusya.ESTIMATEID = Long.Parse(Me.lngEstimateIdHiddenField.Value)

            '契約顧客種別
            drRegCustomerSyoyusya.CONTRACTCUSTTYPE = STR_CONTCUSTTYPE_SYOYUSYA

            '顧客区分
            If Me.shoyusyaKojinCheckMark.Value = STR_TRUE Then
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
            drRegCustomerShiyosya.ESTIMATEID = Long.Parse(Me.lngEstimateIdHiddenField.Value)

            '契約顧客種別
            drRegCustomerShiyosya.CONTRACTCUSTTYPE = STR_CONTCUSTTYPE_SHIYOSYA

            '顧客区分

            If Me.shiyosyaKojinCheckMark.Value = STR_TRUE Then
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



            '■見積諸費用情報データテーブル
            '□車両購入税
            '車両購入税データレコード作成
            Dim drRegChargePurchaseTax As IC3070202DataSet.IC3070202EstChargeInfoRow
            drRegChargePurchaseTax = dtChargeInfo.NewRow

            '見積管理ID
            drRegChargePurchaseTax.ESTIMATEID = Long.Parse(Me.lngEstimateIdHiddenField.Value)

            '費用項目コード
            drRegChargePurchaseTax.ITEMCODE = STR_ITEMCODE_1

            '費用項目名
            drRegChargePurchaseTax.ITEMNAME = Me.CarBuyTaxLabelCustomLabel.Text

            '価格
            ' $99 Ken-Suzuki Modify Start
            'drRegChargePurchaseTax.PRICE = Double.Parse(Me.CarBuyTaxCustomLabel.Text)
            If Not String.IsNullOrEmpty(Me.carBuyTaxHiddenField.Value) Then
                drRegChargePurchaseTax.PRICE = Double.Parse(Me.carBuyTaxHiddenField.Value)
            End If

            '諸費用区分
            drRegChargePurchaseTax.CHARGEDVS = Me.chargeSegmentedButton.SelectedValue
            ' $99 Ken-Suzuki Modify End


            '車両購入税データレコード追加
            dtChargeInfo.Rows.Add(drRegChargePurchaseTax)

            'データレコード開放
            drRegChargePurchaseTax = Nothing


            '□登録費用
            '登録費用データレコード作成
            Dim drRegChargeRegExpense As IC3070202DataSet.IC3070202EstChargeInfoRow
            drRegChargeRegExpense = dtChargeInfo.NewRow

            '見積管理ID
            drRegChargeRegExpense.ESTIMATEID = Long.Parse(Me.lngEstimateIdHiddenField.Value)

            '費用項目コード
            drRegChargeRegExpense.ITEMCODE = STR_ITEMCODE_2

            '費用項目名
            drRegChargeRegExpense.ITEMNAME = Me.regPriceLabelCustomLabel.Text

            '価格
            If Not String.IsNullOrEmpty(Me.regCostValueHiddenField.Value) Then
                drRegChargeRegExpense.PRICE = Double.Parse(Me.regCostValueHiddenField.Value)
            End If

            ' $99 Ken-Suzuki Add Start
            '諸費用区分
            drRegChargeRegExpense.CHARGEDVS = Me.chargeSegmentedButton.SelectedValue
            ' $99 Ken-Suzuki Add End

            '車両購入税データレコード追加
            dtChargeInfo.Rows.Add(drRegChargeRegExpense)

            'データレコード開放
            drRegChargeRegExpense = Nothing


            '■見積保険情報データテーブル

            Dim drRegInsuranceInfo As IC3070202DataSet.IC3070202EstInsuranceInfoRow
            drRegInsuranceInfo = dtInsuranceInfo.NewRow

            '見積管理ID
            drRegInsuranceInfo.ESTIMATEID = Long.Parse(Me.lngEstimateIdHiddenField.Value)

            '保険区分(1:自社、2:他社)
            If Me.jisyaCheckMark.Value = STR_TRUE Then

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
                drRegInsuranceInfo.AMOUNT = Double.Parse(Me.insuAmountValueHiddenField.Value)

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
                drRegPayInfoCash.DEPOSIT = Double.Parse(Me.cashDepositValueHiddenField.Value)

            End If

            '削除フラグ
            drRegPayInfoCash.DELFLG = STR_DELETEFLG_NOT


            '現金データレコード追加
            dtPayInfo.Rows.Add(drRegPayInfoCash)

            'データレコード開放
            drRegPayInfoCash = Nothing


            '□ローン
            'ローンデータレコード作成
            Dim drRegPayInfoLoan As IC3070202DataSet.IC3070202EstPaymentInfoRow
            drRegPayInfoLoan = dtPayInfo.NewRow()

            '見積管理ID
            drRegPayInfoLoan.ESTIMATEID = Long.Parse(Me.lngEstimateIdHiddenField.Value)

            '支払方法区分
            drRegPayInfoLoan.PAYMENTMETHOD = STR_PAYMETHOD_LOAN

            '融資会社コード
            drRegPayInfoLoan.FINANCECOMCODE = Me.SelectFinanceComHiddenField.Value

            '支払期間
            If Not (loanPayPeriodNumericBox.Value Is Nothing) Then
                drRegPayInfoLoan.PAYMENTPERIOD = Integer.Parse(Me.loanPayPeriodNumericBox.Value)
            End If

            '毎月返済額
            If Not String.IsNullOrEmpty(Me.loanMonthlyValueHiddenField.Value) Then
                drRegPayInfoLoan.MONTHLYPAYMENT = Double.Parse(Me.loanMonthlyValueHiddenField.Value)

            End If

            '頭金
            If Not String.IsNullOrEmpty(Me.loanDepositValueHiddenField.Value) Then
                drRegPayInfoLoan.DEPOSIT = Double.Parse(Me.loanDepositValueHiddenField.Value)

            End If

            'ボーナス時返済額
            If Not String.IsNullOrEmpty(Me.loanBonusValueHiddenField.Value) Then
                drRegPayInfoLoan.BONUSPAYMENT = Double.Parse(Me.loanBonusValueHiddenField.Value)

            End If

            '初回支払期限
            If Not (Me.loanDueDateNumericBox.Value Is Nothing) Then
                drRegPayInfoLoan.DUEDATE = Integer.Parse(Me.loanDueDateNumericBox.Value)
            End If

            '削除フラグ
            drRegPayInfoLoan.DELFLG = STR_DELETEFLG_NOT



            'ローンデータレコード追加
            dtPayInfo.Rows.Add(drRegPayInfoLoan)

            'データレコード開放
            drRegPayInfoLoan = Nothing


        Else
            '見積情報保存後

            '■見積顧客情報データテーブル
            '□所有者
            '顧客区分
            If Me.shoyusyaKojinCheckMark.Value = STR_TRUE Then
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

            If Me.shiyosyaKojinCheckMark.Value = STR_TRUE Then
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



            '■見積諸費用情報データテーブル
            '□車両購入税
            '費用項目名
            dtChargeInfo.Rows(0).Item("ItemName") = Me.CarBuyTaxLabelCustomLabel.Text

            '価格
            ' $99 Ken-Suzuki Modify Start
            'dtChargeInfo.Rows(0).Item("Price") = Me.CarBuyTaxCustomLabel.Text
            If String.IsNullOrEmpty(Me.carBuyTaxHiddenField.Value) Then
                dtChargeInfo.Rows(0).Item("Price") = DBNull.Value
            Else
                dtChargeInfo.Rows(0).Item("Price") = Double.Parse(Me.carBuyTaxHiddenField.Value)
            End If
            ' $99 Ken-Suzuki Modify End

            '$99 Ken-Suzuki Add Start
            '諸費用区分
            dtChargeInfo.Rows(0).Item("CHARGEDVS") = Me.chargeSegmentedButton.SelectedValue
            '$99 Ken-Suzuki Add End

            '□登録費用
            '費用項目名
            dtChargeInfo.Rows(1).Item("ItemName") = Me.regPriceLabelCustomLabel.Text

            '価格
            If String.IsNullOrEmpty(Me.regCostValueHiddenField.Value) Then
                dtChargeInfo.Rows(1).Item("Price") = DBNull.Value
            Else
                dtChargeInfo.Rows(1).Item("Price") = Double.Parse(Me.regCostValueHiddenField.Value)

            End If

            '$99 Ken-Suzuki Add Start
            '諸費用区分
            dtChargeInfo.Rows(1).Item("CHARGEDVS") = Me.chargeSegmentedButton.SelectedValue
            '$99 Ken-Suzuki Add End


            '■見積保険情報データテーブル

            '保険区分(1:自社、2:他社)
            If Me.jisyaCheckMark.Value = STR_TRUE Then

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
                dtInsuranceInfo.Rows(0).Item("Amount") = Double.Parse(Me.insuAmountValueHiddenField.Value)

            End If



            '■見積支払方法情報データテーブル
            '□現金
            '頭金
            If String.IsNullOrEmpty(Me.cashDepositValueHiddenField.Value) Then
                dtPayInfo.Rows(0).Item("Deposit") = DBNull.Value
            Else
                dtPayInfo.Rows(0).Item("Deposit") = Double.Parse(Me.cashDepositValueHiddenField.Value)

            End If


            '□ローン
            '融資会社コード
            dtPayInfo.Rows(1).Item("FinanceComCode") = Me.SelectFinanceComHiddenField.Value

            '支払期間
            If loanPayPeriodNumericBox.Value Is Nothing Then
                dtPayInfo.Rows(1).Item("PaymentPeriod") = DBNull.Value
            Else
                dtPayInfo.Rows(1).Item("PaymentPeriod") = Integer.Parse(Me.loanPayPeriodNumericBox.Value)
            End If

            '毎月返済額
            If String.IsNullOrEmpty(Me.loanMonthlyValueHiddenField.Value) Then
                dtPayInfo.Rows(1).Item("MonthlyPayment") = DBNull.Value
            Else
                dtPayInfo.Rows(1).Item("MonthlyPayment") = Double.Parse(Me.loanMonthlyValueHiddenField.Value)

            End If

            '頭金
            If String.IsNullOrEmpty(Me.loanDepositValueHiddenField.Value) Then
                dtPayInfo.Rows(1).Item("Deposit") = DBNull.Value
            Else
                dtPayInfo.Rows(1).Item("Deposit") = Double.Parse(Me.loanDepositValueHiddenField.Value)

            End If

            'ボーナス時返済額
            If String.IsNullOrEmpty(Me.loanBonusValueHiddenField.Value) Then
                dtPayInfo.Rows(1).Item("BonusPayment") = DBNull.Value
            Else
                dtPayInfo.Rows(1).Item("BonusPayment") = Double.Parse(Me.loanBonusValueHiddenField.Value)

            End If

            '初回支払期限
            If Me.loanDueDateNumericBox.Value Is Nothing Then
                dtPayInfo.Rows(1).Item("DueDate") = DBNull.Value
            Else
                dtPayInfo.Rows(1).Item("DueDate") = Integer.Parse(Me.loanDueDateNumericBox.Value)
            End If



        End If


        '■見積車両オプション情報データテーブル
        ''オプションデータレコード削除
        dtVclOptionInfo.Clear()

        '□メーカーオプションデータ格納
        Dim drRegMkrOption As DataRow()
        Dim drRegMkrOptionRow As DataRow
        drRegMkrOption = dsEstimation.Tables("IC3070201VclOptionInfo").Select(STR_GETMKROPTION)

        For Each drRegMkrOptionRow In drRegMkrOption
            dtVclOptionInfo.ImportRow(drRegMkrOptionRow)
        Next


        '□販売店オプションデータ格納
        Dim j As Integer


        For j = 1 To Integer.Parse(Me.dlrOptionCountHiddenField.Value)

            '販売店オプションデータレコード作成
            Dim drRegDlrOption As IC3070202DataSet.IC3070202EstVclOptionInfoRow
            drRegDlrOption = dtVclOptionInfo.NewRow

            '見積管理ID
            drRegDlrOption.ESTIMATEID = Long.Parse(Me.lngEstimateIdHiddenField.Value)

            'オプション区分
            drRegDlrOption.OPTIONPART = STR_OPTIONPART_DLR

            'オプションコード
            drRegDlrOption.OPTIONCODE = j

            'オプション名
            drRegDlrOption.OPTIONNAME = Request.Form.Item(String.Concat("optionNameText", j))

            '価格
            drRegDlrOption.PRICE = Double.Parse(Request.Form.Item(String.Concat("optionPriceText", j)))

            '取付費用
            drRegDlrOption.INSTALLCOST = Double.Parse(Request.Form.Item(String.Concat("optionMoneyText", j)))


            'データレコード追加
            dtVclOptionInfo.Rows.Add(drRegDlrOption)


            'データレコード開放
            drRegDlrOption = Nothing

        Next


        '■見積下取車両情報データテーブル
        'データ削除
        dtTradeInCarInfo.Clear()

        Dim intCarCount As Integer


        For intCarCount = 1 To Integer.Parse(Me.tradeInCarCountHiddenField.Value)

            '見積下取車両データレコード作成
            Dim drRegTradeInCar As IC3070202DataSet.IC3070202EstTradeInCarInfoRow
            drRegTradeInCar = dtTradeInCarInfo.NewRow


            '見積管理ID
            drRegTradeInCar.ESTIMATEID = Long.Parse(Me.lngEstimateIdHiddenField.Value)

            '連番
            drRegTradeInCar.SEQNO = intCarCount

            '車名
            drRegTradeInCar.VEHICLENAME = Request.Form.Item(String.Concat("tradeInCarText", intCarCount))

            ''提示価格
            drRegTradeInCar.ASSESSEDPRICE = Double.Parse(Request.Form.Item(String.Concat("tradeInCarPrice", intCarCount)))


            '見積下取車両データレコード追加
            dtTradeInCarInfo.Rows.Add(drRegTradeInCar)

            'データレコード開放
            drRegTradeInCar = Nothing

        Next


        Return dsRegEstimation

    End Function


    '''' <summary>
    '''' 入力内容変更フラグを立てる
    '''' </summary>
    '''' <param name="sender">イベント発生元</param>
    '''' <param name="e">イベントデータ</param>
    '''' <remarks></remarks>
    Protected Sub inputChanged()

        Me.blnInputChangedClientHiddenField.Value = STR_TRUE
    End Sub


    '''' <summary>
    '''' メインメニューへ遷移する。
    '''' </summary>
    '''' <param name="sender">イベント発生元</param>
    '''' <param name="e">イベントデータ</param>
    '''' <remarks></remarks>
    Private Sub mainMenuButton_Click(sender As Object, e As CommonMasterFooterButtonClickEventArgs)

        ''メインメニューへ遷移
        Me.RedirectNextScreen(STR_DISPID_MAINMENU)


    End Sub


    '''' <summary>
    '''' 顧客詳細画面へ遷移する。
    '''' </summary>
    '''' <param name="sender">イベント発生元</param>
    '''' <param name="e">イベントデータ</param>
    '''' <remarks></remarks>
    Private Sub customerButton_Click(sender As Object, e As CommonMasterFooterButtonClickEventArgs)


        '顧客詳細画面に渡す引数を設定
        MyBase.SetValue(ScreenPos.Next, "SearchKey.FLLWUPBOX_STRCD", Me.strStrCdHiddenField.Value)
        MyBase.SetValue(ScreenPos.Next, "SearchKey.FOLLOW_UP_BOX", Me.lngFollowupBoxSeqNoHiddenField.Value)
        MyBase.SetValue(ScreenPos.Next, "SearchKey.CSTKIND", Me.strCstKindHiddenField.Value)
        MyBase.SetValue(ScreenPos.Next, "SearchKey.CUSTOMERCLASS", Me.strCustomerClassHiddenField.Value)
        MyBase.SetValue(ScreenPos.Next, "SearchKey.CRCUSTID", Me.strCRCustIdHiddenField.Value)

        Me.RedirectNextScreen("SC3080201")

        '顧客詳細画面へ遷移する。
        'gotoCustomer()

    End Sub


    '''' <summary>
    '''' TCV（車種選択）画面へ遷移する。
    '''' </summary>
    '''' <param name="sender">イベント発生元</param>
    '''' <param name="e">イベントデータ</param>
    '''' <remarks></remarks>
    Private Sub tcvButton_Click(sender As Object, e As CommonMasterFooterButtonClickEventArgs)


        'ログイン情報
        Dim staffInfo As StaffContext
        Dim strBrnCd As String          '店舗コード
        Dim strAccount As String        'アカウント

        'ログインスタッフ情報取得
        staffInfo = StaffContext.Current
        strBrnCd = staffInfo.BrnCD
        strAccount = staffInfo.Account

        '未保存フラグ
        Dim blnNewActFlg As Boolean
        If Me.blnNewActFlagHiddenField.Value.ToUpper() = STR_TRUE Then
            blnNewActFlg = True
        Else
            blnNewActFlg = False
        End If


        ''TCV（車種選択）へ遷移
        e.Parameters.Add("DataSource", STR_ESTIMATEID)                              'データ読み込み元
        e.Parameters.Add("MenuLockFlag", OperationLocked)                           'メニューロック状態
        e.Parameters.Add("AccountStrCd", strBrnCd)                                  'ログインユーザー店舗コード
        e.Parameters.Add("Account", strAccount)                                     'ログインユーザーアカウント
        e.Parameters.Add("NewActFlag", blnNewActFlg)                                '未保存フラグ
        e.Parameters.Add("DlrCd", Me.strDlrcdHiddenField.Value)                     '販売店コード
        e.Parameters.Add("StartPageId", STR_DISPID_TCV_SELECTSERIES)                '初期表示画面ID
        e.Parameters.Add("EstimateId", Me.lngEstimateIdHiddenField.Value)           '見積ID
        e.Parameters.Add("SelectedEstimateIndex", "0")                              '選択している見積IDのindex

    End Sub


    '''' <summary>
    '''' TCV（車両紹介）画面へ遷移する。
    '''' </summary>
    '''' <param name="sender">イベント発生元</param>
    '''' <param name="e">イベントデータ</param>
    '''' <remarks></remarks>
    Private Sub carInvitationButton_Click(sender As Object, e As CommonMasterFooterButtonClickEventArgs)


        'ログイン情報
        Dim staffInfo As StaffContext
        Dim strBrnCd As String          '店舗コード
        Dim strAccount As String        'アカウント

        'ログインスタッフ情報取得
        staffInfo = StaffContext.Current
        strBrnCd = staffInfo.BrnCD
        strAccount = staffInfo.Account

        '未保存フラグ
        Dim blnNewActFlg As Boolean
        If Me.blnNewActFlagHiddenField.Value.ToUpper() = STR_TRUE Then
            blnNewActFlg = True
        Else
            blnNewActFlg = False
        End If

        ''TCV（車両紹介）へ遷移
        e.TCVFunction = True

        e.Parameters.Add("DataSource", STR_ESTIMATEID)                              'データ読み込み元
        e.Parameters.Add("MenuLockFlag", OperationLocked)                           'メニューロック状態
        e.Parameters.Add("AccountStrCd", strBrnCd)                                  'ログインユーザー店舗コード
        e.Parameters.Add("Account", strAccount)                                     'ログインユーザーアカウント
        e.Parameters.Add("NewActFlag", blnNewActFlg)                                '未保存フラグ
        e.Parameters.Add("DlrCd", Me.strDlrcdHiddenField.Value)                     '販売店コード
        e.Parameters.Add("StartPageId", STR_DISPID_TCV_CARINVITATION)               '初期表示画面ID
        e.Parameters.Add("EstimateId", Me.lngEstimateIdHiddenField.Value)           '見積ID
        e.Parameters.Add("SelectedEstimateIndex", "0")                              '選択している見積IDのindex


    End Sub


    '''' <summary>
    '''' TCV（諸元表）画面へ遷移する。
    '''' </summary>
    '''' <param name="sender">イベント発生元</param>
    '''' <param name="e">イベントデータ</param>
    '''' <remarks></remarks>
    Private Sub originaiListButton_Click(sender As Object, e As CommonMasterFooterButtonClickEventArgs)


        'ログイン情報
        Dim staffInfo As StaffContext
        Dim strBrnCd As String          '店舗コード
        Dim strAccount As String        'アカウント

        'ログインスタッフ情報取得
        staffInfo = StaffContext.Current
        strBrnCd = staffInfo.BrnCD
        strAccount = staffInfo.Account

        '未保存フラグ
        Dim blnNewActFlg As Boolean
        If Me.blnNewActFlagHiddenField.Value.ToUpper() = STR_TRUE Then
            blnNewActFlg = True
        Else
            blnNewActFlg = False
        End If

        ''TCV（諸元表）へ遷移
        e.TCVFunction = True
        e.Parameters.Add("DataSource", STR_ESTIMATEID)                              'データ読み込み元
        e.Parameters.Add("MenuLockFlag", OperationLocked)                           'メニューロック状態
        e.Parameters.Add("AccountStrCd", strBrnCd)                                  'ログインユーザー店舗コード
        e.Parameters.Add("Account", strAccount)                                     'ログインユーザーアカウント
        e.Parameters.Add("NewActFlag", blnNewActFlg)                                '未保存フラグ
        e.Parameters.Add("DlrCd", Me.strDlrcdHiddenField.Value)                     '販売店コード
        e.Parameters.Add("StartPageId", STR_DISPID_TCV_ORIGINALLIST)                '初期表示画面ID
        e.Parameters.Add("EstimateId", Me.lngEstimateIdHiddenField.Value)           '見積ID
        e.Parameters.Add("SelectedEstimateIndex", "0")                              '選択している見積IDのindex

    End Sub


    '''' <summary>
    '''' TCV（競合車比較）画面へ遷移する。
    '''' </summary>
    '''' <param name="sender">イベント発生元</param>
    '''' <param name="e">イベントデータ</param>
    '''' <remarks></remarks>
    Private Sub compareCompetitorButton_Click(sender As Object, e As CommonMasterFooterButtonClickEventArgs)


        'ログイン情報
        Dim staffInfo As StaffContext
        Dim strBrnCd As String          '店舗コード
        Dim strAccount As String        'アカウント

        'ログインスタッフ情報取得
        staffInfo = StaffContext.Current
        strBrnCd = staffInfo.BrnCD
        strAccount = staffInfo.Account

        '未保存フラグ
        Dim blnNewActFlg As Boolean
        If Me.blnNewActFlagHiddenField.Value.ToUpper() = STR_TRUE Then
            blnNewActFlg = True
        Else
            blnNewActFlg = False
        End If

        ''TCV（競合車比較）へ遷移
        e.TCVFunction = True
        e.Parameters.Add("DataSource", STR_ESTIMATEID)                              'データ読み込み元
        e.Parameters.Add("MenuLockFlag", OperationLocked)                           'メニューロック状態
        e.Parameters.Add("AccountStrCd", strBrnCd)                                  'ログインユーザー店舗コード
        e.Parameters.Add("Account", strAccount)                                     'ログインユーザーアカウント
        e.Parameters.Add("NewActFlag", blnNewActFlg)                                '未保存フラグ
        e.Parameters.Add("DlrCd", Me.strDlrcdHiddenField.Value)                     '販売店コード
        e.Parameters.Add("StartPageId", STR_DISPID_TCV_COMPARECOMPETITOR)           '初期表示画面ID
        e.Parameters.Add("EstimateId", Me.lngEstimateIdHiddenField.Value)           '見積ID
        e.Parameters.Add("SelectedEstimateIndex", "0")                              '選択している見積IDのindex

    End Sub


    '''' <summary>
    '''' TCV（ライブラリ）画面へ遷移する。
    '''' </summary>
    '''' <param name="sender">イベント発生元</param>
    '''' <param name="e">イベントデータ</param>
    '''' <remarks></remarks>
    Private Sub libraryButton_Click(sender As Object, e As CommonMasterFooterButtonClickEventArgs)


        'ログイン情報
        Dim staffInfo As StaffContext
        Dim strBrnCd As String          '店舗コード
        Dim strAccount As String        'アカウント

        'ログインスタッフ情報取得
        staffInfo = StaffContext.Current
        strBrnCd = staffInfo.BrnCD
        strAccount = staffInfo.Account

        '未保存フラグ
        Dim blnNewActFlg As Boolean
        If Me.blnNewActFlagHiddenField.Value.ToUpper() = STR_TRUE Then
            blnNewActFlg = True
        Else
            blnNewActFlg = False
        End If

        ''TCV（ライブラリ）へ遷移
        e.TCVFunction = True
        e.Parameters.Add("DataSource", STR_ESTIMATEID)                              'データ読み込み元
        e.Parameters.Add("MenuLockFlag", OperationLocked)                           'メニューロック状態
        e.Parameters.Add("AccountStrCd", strBrnCd)                                  'ログインユーザー店舗コード
        e.Parameters.Add("Account", strAccount)                                     'ログインユーザーアカウント
        e.Parameters.Add("NewActFlag", blnNewActFlg)                                '未保存フラグ
        e.Parameters.Add("DlrCd", Me.strDlrcdHiddenField.Value)                     '販売店コード
        e.Parameters.Add("StartPageId", STR_DISPID_TCV_LIBRARY)                     '初期表示画面ID
        e.Parameters.Add("EstimateId", Me.lngEstimateIdHiddenField.Value)           '見積ID
        e.Parameters.Add("SelectedEstimateIndex", "0")                              '選択している見積IDのindex

    End Sub


    ''' <summary>
    ''' ヘッダーボタンの制御
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitHeaderEvent()

        'ログアウト
        '活動破棄チェックのクライアントサイドスクリプトを埋め込む
        CType(Me.Master, CommonMasterPage).GetHeaderButton(HeaderButton.Logout).OnClientClick = "return deleteCheck();"


    End Sub


    ''' <summary>
    ''' フッターボタンの制御
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitFooterEvent()

        '活動破棄チェックのクライアントサイドスクリプトを埋め込む
        'メニュー
        CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.MainMenu).OnClientClick = "return deleteCheck();"

        '入力内容破棄チェックのクライアントサイドスクリプトを埋め込む
        '顧客
        'CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.Customer).OnClientClick = "return inputUpdateCheck();"
        ''TCV
        'CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.TCV).OnClientClick = "return inputUpdateCheck();"
        '車両紹介
        CType(Me.Master, CommonMasterPage).GetFooterButton(SUBMENU_TCV_CARINVITATION).OnClientClick = "return inputUpdateCheck();"
        '緒元表
        CType(Me.Master, CommonMasterPage).GetFooterButton(SUBMENU_TCV_ORIGINALLIST).OnClientClick = "return inputUpdateCheck();"
        '競合車比較
        CType(Me.Master, CommonMasterPage).GetFooterButton(SUBMENU_TCV_COMPARECOMPETITOR).OnClientClick = "return inputUpdateCheck();"
        'ライブラリ
        CType(Me.Master, CommonMasterPage).GetFooterButton(SUBMENU_TCV_LIBRARY).OnClientClick = "return inputUpdateCheck();"

        '入力内容変更チェックのクライアントサイドスクリプトを埋め込む

    End Sub


    ''' <summary>
    ''' ボタンの制御
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitButtonEvent()

        'カーアイコン
        'ロード中のクライアントサイドスクリプトを埋め込む
        saveLinkButton.OnClientClick = "return dispLoading();"
        MitsumoriprintButton.OnClientClick = "return estPreviewClientClick();"

    End Sub


    'ロック機能
    Public ReadOnly Property DefaultOperationLocked As Boolean Implements Toyota.eCRB.SystemFrameworks.Web.ICustomerForm.DefaultOperationLocked
        Get

            Dim blnLockStatus As String            'ロック状態

            blnLockStatus = Me.GetValue(ScreenPos.Current, "MenuLockFlag", False)



            If String.Equals(blnLockStatus.ToUpper(), STR_TRUE) Then
                Return True

            Else
                Return False
            End If
        End Get
    End Property
End Class
