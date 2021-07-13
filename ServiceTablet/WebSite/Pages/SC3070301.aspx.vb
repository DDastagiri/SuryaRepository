'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3070301.aspx.vb
'─────────────────────────────────────
'機能： 契約書印刷
'補足： 
'作成： 2011/12/01 TCS 相田
'更新： 2012/02/03 TCS 藤井  【SALES_1A】号口(課題No.46)対応
'─────────────────────────────────────

Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.Estimate.Order.DataAccess
Imports Toyota.eCRB.Estimate.Order.BizLogic
Imports System.Globalization

Partial Class Pages_SC3070301
    Inherits BasePage

    ''' <summary>
    ''' 契約情報データテーブル
    ''' </summary>
    ''' <value>データテーブル</value>
    ''' <returns>契約情報データテーブル</returns>
    ''' <remarks></remarks>
    Private Property ContractDt() As SC3070301DataSet.ConstractInfoDataTable
        Get
            Return ViewState("ContractDt")
        End Get
        Set(value As SC3070301DataSet.ConstractInfoDataTable)
            ViewState("ContractDt") = value
        End Set
    End Property

    ''' <summary>
    ''' 契約状況フラグ　契約済み
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONSTRACTFLG_CONSTRACT As String = "1"
    ''' <summary>
    ''' 印刷フラグ　印刷済み
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PRINTFLG_PRINT As String = "1"
    ''' <summary>
    ''' 契約状況フラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONSTRACTFLG As String = "CONTRACTFLG"
    ''' <summary>
    ''' 印刷フラグ　
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PRINTFLG As String = "CONTPRINTFLG"
    ''' <summary>
    ''' 契約書No　
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONTRACTNO As String = "CONTRACTNO"
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
    ''' 書式　数値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FORMAT_NUMBER As String = "#,##"
    ''' <summary>
    ''' 書式　漢語表示用
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FORMAT_KANJI As String = "0.00"
    ''' <summary>
    ''' 契約状況フラグ　契約済み
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONTRACTFLG_CONTRACT As String = "1"
    ''' <summary>
    ''' 契約状況フラグ　キャンセル
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONTRACTFLG_CANCEL As String = "2"
    ''' <summary>
    ''' 台数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CARCOUNT_1 As String = "1"
    ''' <summary>
    ''' \
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ENMARK As String = "&#165;"
    ''' <summary>
    ''' 漢語の単位
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TANI_KANJI As String = "圆整"
    ''' <summary>
    ''' 印刷処理フラグ 印刷処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FLG_DO_PRINT As String = "1"
    ''' <summary>
    ''' 印刷処理フラグ 印刷処理でない
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FLG_NOT_PRINT As String = "0"

    ''' <summary>
    ''' ロードの処理を実施します。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            'セッション情報の取得
            Dim tblPageLoad As SC3070301DataSet.SessionDataTable
            tblPageLoad = GetSession()
            '見積情報取得
            Dim msgIdPageLoad As Integer = Integer.MinValue
            ContractDt = SC3070301BusinessLogic.GetConstractInfo(tblPageLoad, msgIdPageLoad)

            If Not ContractDt Is Nothing Then
                '文言の設定
                SetWord(ContractDt, tblPageLoad)
                'ボタン表示非表示
                ControlButton(ContractDt.Rows(0).Item(CONSTRACTFLG), ContractDt.Rows(0).Item(PRINTFLG))
                'hidden項目設定
                Me.printFlgHiddenField.Value = ContractDt.Rows(0).Item(PRINTFLG)

            Else
                'エラーメッセージ表示
                Me.ShowMessageBox(msgIdPageLoad)
            End If

        End If
    End Sub


    ''' <summary>
    ''' 閉じるボタンを押下時の処理を実行します。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub CloseButton_Click(sender As Object, e As System.EventArgs) Handles CloseButton.Click

        '見積作成画面へ戻る
        Me.RedirectPrevScreen()

    End Sub

    ''' <summary>
    ''' 印刷ボタンを押下時の処理を実行します。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub PrintButton_Click(sender As Object, e As System.EventArgs) Handles PrintButton.Click

        '契約書印刷フラグの判定
        If Not PRINTFLG_PRINT.Equals(ContractDt.Rows(0).Item(PRINTFLG)) Then
            'セッション情報の取得
            Dim tblPrint As SC3070301DataSet.SessionDataTable
            tblPrint = GetSession()

            Dim msgIdPrint As Integer = 0
            Dim bizClass As New SC3070301BusinessLogic
            '契約書印刷フラグの更新
            If Not bizClass.UpdatePrintFlg(tblPrint, msgIdPrint) Then
                If Not msgIdPrint = Integer.MinValue Then
                    'エラーメッセージ表示
                    Me.ShowMessageBox(msgIdPrint)
                End If
            Else
                'ボタン表示非表示
                ContractDt.Rows(0).Item(PRINTFLG) = PRINTFLG_PRINT
                ControlButton(ContractDt.Rows(0).Item(CONSTRACTFLG), ContractDt.Rows(0).Item(PRINTFLG))
                'hidden項目設定
                printFlgHiddenField.Value = ContractDt.Rows(0).Item(PRINTFLG)
            End If
        End If

        'スクリプト実行
        SetScript()

    End Sub

    ''' <summary>
    ''' 実行ボタンを押下時の処理を実行します。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub SendButton_Click(sender As Object, e As System.EventArgs) Handles SendButton.Click
        'セッション情報の取得
        Dim tblSend As SC3070301DataSet.SessionDataTable
        tblSend = GetSession()

        Dim bizClass As New SC3070301BusinessLogic
        Dim msgIdSend As Integer = 0

        '契約情報の更新処理
        If Not bizClass.UpdateConstractInfoSend(tblSend, msgIdSend, ContractDt) Then
            If Not msgIdSend = Integer.MinValue Then
                'エラーメッセージ表示
                Me.ShowMessageBox(msgIdSend)
            End If

        Else
            'ボタン表示非表示()
            ContractDt.Rows(0).Item(CONSTRACTFLG) = CONTRACTFLG_CONTRACT
            ControlButton(ContractDt.Rows(0).Item(CONSTRACTFLG), ContractDt.Rows(0).Item(PRINTFLG))
            '契約書No
            contractNoLabel.Text = ContractDt.Rows(0).Item(CONTRACTNO)
        End If
    End Sub

    ''' <summary>
    ''' キャンセルボタン押下時の処理を実行します。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub CancelButton_Click(sender As Object, e As System.EventArgs) Handles CancelButton.Click
        'セッション情報の取得
        Dim tblCancel As SC3070301DataSet.SessionDataTable
        tblCancel = GetSession()
        'メッセージID
        Dim msgIdCancel As Integer = 0
        Dim bizClass As New SC3070301BusinessLogic

        '契約情報の更新処理
        If Not bizClass.UpdateConstractInfoCancel(tblCancel, msgIdCancel) Then
            If Not msgIdCancel = Integer.MinValue Then
                'エラーメッセージ表示
                Me.ShowMessageBox(msgIdCancel)
            End If
        Else
            'ボタン表示非表示
            ContractDt.Rows(0).Item(CONSTRACTFLG) = CONTRACTFLG_CANCEL
            ControlButton(ContractDt.Rows(0).Item(CONSTRACTFLG), ContractDt.Rows(0).Item(PRINTFLG))
            '契約書No
            contractNoLabel.Text = ""
        End If
    End Sub

    ''' <summary>
    ''' セッション情報をセットします。
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetSession() As SC3070301DataSet.SessionDataTable
        Using tbl As New SC3070301DataSet.SessionDataTable
            Dim tblRow As SC3070301DataSet.SessionRow = tbl.NewSessionRow
            '見積管理ID取得
            tblRow.ESTIMATEID = DirectCast(Me.GetValue(ScreenPos.Current, "estimateId", False), Long)
            '支払方法区分取得
            tblRow.PAYMENTMETHOD = DirectCast(Me.GetValue(ScreenPos.Current, "paymentMethod", False), String)

            tbl.Rows.Add(tblRow)

            Return tbl
        End Using
    End Function

    ''' <summary>
    ''' 文言の設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetWord(ByVal constractDt As SC3070301DataSet.ConstractInfoDataTable,
                        ByVal tbl As SC3070301DataSet.SessionDataTable)

        Dim tblDataRow As SC3070301DataSet.ConstractInfoRow
        tblDataRow = constractDt.Rows(0)

        'ボタン
        PrintButton.Text = WebWordUtility.GetWord(550)
        SendButton.Text = WebWordUtility.GetWord(551)
        CancelButton.Text = WebWordUtility.GetWord(552)

        ' 2012/02/03 TCS 藤井 【SALES_1A】号口(課題No.46)対応 ADD START
        'ボタン押下時の確認メッセージ
        sendCheckMsg.Value = WebWordUtility.GetWord(904)
        cancelCheckMsg.Value = WebWordUtility.GetWord(905)
        ' 2012/02/03 TCS 藤井 【SALES_1A】号口(課題No.46)対応 ADD END

        '備考欄
        vehicleAmountRemarksLabel.Text = WebWordUtility.GetWord(300)
        optionRemarksLabel.Text = WebWordUtility.GetWord(301)
        insuranceCostsRemarksLabel.Text = WebWordUtility.GetWord(303)
        additionalCostsRemarksLabel.Text = WebWordUtility.GetWord(305)
        additionalCosts2RemarksLabel.Text = WebWordUtility.GetWord(304)
        carCountRemarksLabel.Text = WebWordUtility.GetWord(306)
        priceAmountRemarksLabel.Text = WebWordUtility.GetWord(307)

        '売方・買方情報
        dealerNameWordLabel.Text = "广州长润汽车销售有限公司"
        buyerNameWordLabel.Text = tblDataRow.BUYERNAME
        dealerAddressWordLabel.Text = tblDataRow.ADDR_LOCAL
        buyerAddressWordLabel.Text = tblDataRow.BUYERADDRESS
        dealerSalesHotLineWordLabel.Text = tblDataRow.SALTEL
        buyerIdWordLabel.Text = tblDataRow.BUYERSOCIALID
        dealerServiceHotLineWordLabel.Text = tblDataRow.SRVSTEL
        buyerTellHomeWordLabel.Text = tblDataRow.BUYERTELNO
        buyerFaxWordLabel.Text = tblDataRow.BUYERFAXNO
        dealerFaxWordLabel.Text = tblDataRow.SALFAXNO
        buyerTellMobileWordLabel.Text = tblDataRow.BUYERMOBILE
        buyerPostNoWordLabel.Text = tblDataRow.BUYERZIPCODE

        '車両情報
        carNameWordLabel.Text = tblDataRow.SERIESNM
        gradeWordLabel.Text = tblDataRow.MODELNM
        modelWordLabel.Text = tblDataRow.MODELNUMBER
        suffixWordLabel.Text = tblDataRow.SUFFIXCD
        bodyColorWordLabel.Text = tblDataRow.EXTCOLOR
        interiorColorWordLabel.Text = tblDataRow.INTCOLOR

        '納車日
        DeliveryDateLabel.Text = tblDataRow.DELIDATE_SCREEN

        '車両価格
        vehicleAmountWordLabel.Text = GetPrice(tblDataRow.VCLPRICE.ToString(FORMAT_NUMBER, CultureInfo.CurrentCulture), "")
        Dim vehicleAmountKanji As String = ConvertKanji(tblDataRow.VCLPRICE.ToString(FORMAT_KANJI, CultureInfo.CurrentCulture))
        vehicleAmountKanjiLabel.Text = GetPrice(vehicleAmountKanji, TANI_KANJI)

        'オプション
        optionWordLabel.Text = GetPrice(tblDataRow.OPTIONPRICE.ToString(FORMAT_NUMBER, CultureInfo.CurrentCulture), "")
        Dim optionKanji As String = ConvertKanji(tblDataRow.OPTIONPRICE.ToString(FORMAT_KANJI, CultureInfo.CurrentCulture))
        optionKanjiLiteral.Text = GetPrice(optionKanji, TANI_KANJI)

        '保険
        insuranceCostsWordLabel.Text = GetPrice(tblDataRow.INSUPRICE.ToString(FORMAT_NUMBER, CultureInfo.CurrentCulture), "")
        Dim insuranceCostsKanji As String = ConvertKanji(tblDataRow.INSUPRICE.ToString(FORMAT_KANJI, CultureInfo.CurrentCulture))
        insuranceCostsKanjiLabel.Text = GetPrice(insuranceCostsKanji, TANI_KANJI)

        '諸費用
        additionalCostsWordLabel.Text = GetPrice(tblDataRow.ITEMPRICE1.ToString(FORMAT_NUMBER, CultureInfo.CurrentCulture), "")
        Dim additionalCostsKanji As String = ConvertKanji(tblDataRow.ITEMPRICE1.ToString(FORMAT_KANJI, CultureInfo.CurrentCulture))
        additionalCostsKanjiLabel.Text = GetPrice(additionalCostsKanji, TANI_KANJI)

        '諸費用
        additionalCosts2WordLabel.Text = GetPrice(tblDataRow.ITEMPRICE2.ToString(FORMAT_NUMBER, CultureInfo.CurrentCulture), "")
        Dim additionalCostsKanji2 As String = ConvertKanji(tblDataRow.ITEMPRICE2.ToString(FORMAT_KANJI, CultureInfo.CurrentCulture))
        additionalCostsKanji2Label.Text = GetPrice(additionalCostsKanji2, TANI_KANJI)

        '台数
        carCountWordLabel.Text = CARCOUNT_1
        carCountKanjiLiteral.Text = ConvertKanji(CARCOUNT_1)

        '合計
        priceAmountWordLabel.Text = GetPrice(tblDataRow.PRICEAMOUNT.ToString(FORMAT_NUMBER, CultureInfo.CurrentCulture), "")
        Dim priceAmountKanji As String = ConvertKanji(tblDataRow.PRICEAMOUNT.ToString(FORMAT_KANJI, CultureInfo.CurrentCulture))
        priceAmountKanjiLabel.Text = GetPrice(priceAmountKanji, TANI_KANJI)

        'お支払方法
        ControlPaymentMethod(tbl.Rows(0).Item("PAYMENTMETHOD"), tblDataRow)

        '契約書No
        contractNoLabel.Text = tblDataRow.CONTRACTNO
    End Sub

    ''' <summary>
    ''' ボタンの表示非表示を制御します。
    ''' </summary>
    ''' <param name="constractFlg">契約状況フラグ</param>
    ''' <param name="printFlg">契約書印刷フラグ</param>
    ''' <remarks></remarks>
    Private Sub ControlButton(ByVal constractFlg As String, ByVal printFlg As String)

        '印刷フラグ判定
        If PRINTFLG_PRINT.Equals(printFlg) Then
            SendButton.Enabled = True

            '契約状況フラグ判定
            If CONSTRACTFLG_CONSTRACT.Equals(constractFlg) Then
                CancelButton.Visible = True
                SendButton.Visible = False
            Else
                CancelButton.Visible = False
                SendButton.Visible = True
            End If

        Else
            SendButton.Visible = True
            SendButton.Enabled = False
            CancelButton.Visible = False
        End If

    End Sub


    ''' <summary>
    ''' お支払方法の表示非表示を制御します。
    ''' </summary>
    ''' <param name="paymentMethod">お支払方法フラグ</param>
    ''' <param name="tblDataRow">残金</param>
    ''' <remarks></remarks>
    Private Sub ControlPaymentMethod(ByVal paymentMethod As String,
                                     ByVal tblDataRow As SC3070301DataSet.ConstractInfoRow)

        'お支払方法の判定処理
        If PAYMENTMETHOD_MONEY.Equals(paymentMethod) Then
            If tblDataRow.ONLYPAY > 0 Then
                'お支払方法：現金　残金：0以上の場合
                Dim depositKanji As String = ConvertKanji(tblDataRow.DEPOSIT.ToString(FORMAT_KANJI, CultureInfo.CurrentCulture))
                depositKanjiLabel.Text = GetPrice(depositKanji, TANI_KANJI)
                depositLabel.Text = GetPrice(tblDataRow.DEPOSIT.ToString(FORMAT_NUMBER, CultureInfo.CurrentCulture), "")

                Dim onlyPayKanji As String = ConvertKanji(tblDataRow.ONLYPAY.ToString(FORMAT_KANJI, CultureInfo.CurrentCulture))
                onlyPayKanjiLabel.Text = GetPrice(onlyPayKanji, TANI_KANJI)
                onlyPayWordLabel.Text = GetPrice(tblDataRow.ONLYPAY.ToString(FORMAT_NUMBER, CultureInfo.CurrentCulture), "")

                'チェックボックス画像
                checkBoxOffInstallmentImg.Visible = False
                checkBoxOnLoanImg.Visible = False
                checkBoxOnlumpSumImg.Visible = False
            Else
                'お支払方法：現金　残金：0の場合
                Dim lumpSumKanji As String = ConvertKanji(tblDataRow.DEPOSIT.ToString(FORMAT_KANJI, CultureInfo.CurrentCulture))
                lumpSumKanjiLiteral.Text = GetPrice(lumpSumKanji, TANI_KANJI)
                lumpSumLiteral.Text = GetPrice(tblDataRow.DEPOSIT.ToString(FORMAT_NUMBER, CultureInfo.CurrentCulture), "")
                'チェックボックス画像
                checkBoxOnInstallmentImg.Visible = False
                checkBoxOnLoanImg.Visible = False
                checkBoxOfflumpSumImg.Visible = False

            End If
        Else
            'お支払方法：ローンの場合
            Dim loanDayKanji As String = ConvertKanji(tblDataRow.DEPOSIT.ToString(FORMAT_KANJI, CultureInfo.CurrentCulture))
            loanDayKanjiLiteral.Text = GetPrice(loanDayKanji, TANI_KANJI)
            loanDayLiteral.Text = GetPrice(tblDataRow.DEPOSIT.ToString(FORMAT_NUMBER, CultureInfo.CurrentCulture), "")
            loanTimeForPaymentLabel.Text = tblDataRow.DUEDATE
            'チェックボックス画像
            checkBoxOnInstallmentImg.Visible = False
            checkBoxOffLoanImg.Visible = False
            checkBoxOnlumpSumImg.Visible = False
        End If

    End Sub


    ''' <summary>
    ''' 数値を漢語へ変換します。
    ''' </summary>
    ''' <param name="str">変換対象</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ConvertKanji(ByVal str As String) As String

        Dim prices As String() = str.Split(".")
        Dim rtnStr As New StringBuilder

        If Not String.IsNullOrEmpty(prices(0)) Then
            Select Case prices(0).Length
                Case 1
                    rtnStr.Append(GetKanji(prices(0), 0))
                Case 2
                    rtnStr.Append(GetKanji(prices(0).Substring(0, 1), 1))
                    rtnStr.Append(GetKanji(prices(0).Substring(1, 1), 0))
                Case 3
                    rtnStr.Append(GetKanji(prices(0).Substring(0, 1), 2))
                    rtnStr.Append(GetKanji(prices(0).Substring(1, 1), 1))
                    rtnStr.Append(GetKanji(prices(0).Substring(2, 1), 0))
                Case 4
                    rtnStr.Append(GetKanji(prices(0).Substring(0, 1), 3))
                    rtnStr.Append(GetKanji(prices(0).Substring(1, 1), 2))
                    rtnStr.Append(GetKanji(prices(0).Substring(2, 1), 1))
                    rtnStr.Append(GetKanji(prices(0).Substring(3, 1), 0))
                Case 5
                    rtnStr.Append(GetKanji(prices(0).Substring(0, 1), 4))
                    rtnStr.Append(GetKanji(prices(0).Substring(1, 1), 3))
                    rtnStr.Append(GetKanji(prices(0).Substring(2, 1), 2))
                    rtnStr.Append(GetKanji(prices(0).Substring(3, 1), 1))
                    rtnStr.Append(GetKanji(prices(0).Substring(4, 1), 0))
                Case 6
                    If "0".Equals(prices(0).Substring(1, 1)) Then
                        rtnStr.Append(GetKanji(prices(0).Substring(0, 1), 8))
                    Else
                        rtnStr.Append(GetKanji(prices(0).Substring(0, 1), 5))
                        rtnStr.Append(GetKanji(prices(0).Substring(1, 1), 4))
                    End If

                    rtnStr.Append(GetKanji(prices(0).Substring(2, 1), 3))
                    rtnStr.Append(GetKanji(prices(0).Substring(3, 1), 2))
                    rtnStr.Append(GetKanji(prices(0).Substring(4, 1), 1))
                    rtnStr.Append(GetKanji(prices(0).Substring(5, 1), 0))
                Case 7
                    If "00".Equals(prices(0).Substring(1, 2)) Then
                        rtnStr.Append(GetKanji(prices(0).Substring(0, 1), 9))
                    Else
                        rtnStr.Append(GetKanji(prices(0).Substring(0, 1), 6))
                        If "0".Equals(prices(0).Substring(2, 1)) Then
                            rtnStr.Append(GetKanji(prices(0).Substring(1, 1), 8))
                        Else
                            rtnStr.Append(GetKanji(prices(0).Substring(1, 1), 5))
                        End If

                        rtnStr.Append(GetKanji(prices(0).Substring(2, 1), 4))
                    End If

                    rtnStr.Append(GetKanji(prices(0).Substring(3, 1), 3))
                    rtnStr.Append(GetKanji(prices(0).Substring(4, 1), 2))
                    rtnStr.Append(GetKanji(prices(0).Substring(5, 1), 1))
                    rtnStr.Append(GetKanji(prices(0).Substring(6, 1), 0))
                Case 8
                    If "000".Equals(prices(0).Substring(1, 3)) Then
                        rtnStr.Append(GetKanji(prices(0).Substring(0, 1), 10))
                    Else
                        rtnStr.Append(GetKanji(prices(0).Substring(0, 1), 7))
                        If "00".Equals(prices(0).Substring(2, 2)) Then
                            rtnStr.Append(GetKanji(prices(0).Substring(1, 1), 9))
                        Else
                            rtnStr.Append(GetKanji(prices(0).Substring(1, 1), 6))
                            If "0".Equals(prices(0).Substring(3, 1)) Then
                                rtnStr.Append(GetKanji(prices(0).Substring(2, 1), 8))
                            Else
                                rtnStr.Append(GetKanji(prices(0).Substring(2, 1), 5))
                            End If
                        End If
                    End If

                    rtnStr.Append(GetKanji(prices(0).Substring(3, 1), 4))
                    rtnStr.Append(GetKanji(prices(0).Substring(4, 1), 3))
                    rtnStr.Append(GetKanji(prices(0).Substring(5, 1), 2))
                    rtnStr.Append(GetKanji(prices(0).Substring(6, 1), 1))
                    rtnStr.Append(GetKanji(prices(0).Substring(7, 1), 0))
            End Select
        End If

        If prices.Length > 1 Then
            If Not "00".Equals(prices(1)) Then
                If Not rtnStr.Length = 0 Then
                    rtnStr.Append(".")
                    If "0".Equals(prices(1).Substring(0, 1)) Then
                        rtnStr.Append(GetKanji("00", 0))
                    Else
                        rtnStr.Append(GetKanji(prices(1).Substring(0, 1), 0))
                    End If

                    If Not "0".Equals(prices(1).Substring(1, 1)) Then
                        rtnStr.Append(GetKanji(prices(1).Substring(1, 1), 0))
                    End If
                End If
            End If
        End If

        Return rtnStr.ToString()
    End Function

    ''' <summary>
    ''' 数値の漢語を返却します。
    ''' </summary>
    ''' <param name="str">変換対象</param>
    ''' <param name="intParam">桁数に対応する数値</param>
    ''' <returns>変換対象に対応する漢語</returns>
    ''' <remarks></remarks>
    Private Function GetKanji(ByVal str As String, ByVal intParam As Integer) As String

        Dim rtnStr As String = ""

        Select Case str
            Case "00"
                rtnStr = WebWordUtility.GetWord(218)
            Case "1"
                rtnStr = WebWordUtility.GetWord(200)
            Case "2"
                rtnStr = WebWordUtility.GetWord(201)
            Case "3"
                rtnStr = WebWordUtility.GetWord(202)
            Case "4"
                rtnStr = WebWordUtility.GetWord(203)
            Case "5"
                rtnStr = WebWordUtility.GetWord(204)
            Case "6"
                rtnStr = WebWordUtility.GetWord(205)
            Case "7"
                rtnStr = WebWordUtility.GetWord(206)
            Case "8"
                rtnStr = WebWordUtility.GetWord(207)
            Case "9"
                rtnStr = WebWordUtility.GetWord(208)

        End Select


        If intParam = 0 Then
            If String.IsNullOrEmpty(rtnStr) Then
                Return ""
            Else
                Return rtnStr
            End If
        Else
            If Not String.IsNullOrEmpty(rtnStr) Then
                Return rtnStr & GetKeta(intParam)
            End If
        End If

        Return rtnStr
    End Function

    ''' <summary>
    ''' 桁数の漢語を返却します。
    ''' </summary>
    ''' <returns>桁数</returns>
    ''' <param name="intParam">桁数に対応する数値</param>
    ''' <remarks></remarks>
    Private Function GetKeta(ByVal intParam As Integer) As String

        Select Case intParam
            Case 1
                Return WebWordUtility.GetWord(209)
            Case 2
                Return WebWordUtility.GetWord(210)
            Case 3
                Return WebWordUtility.GetWord(211)
            Case 4
                Return WebWordUtility.GetWord(212)
            Case 5
                Return WebWordUtility.GetWord(209)
            Case 6
                Return WebWordUtility.GetWord(210)
            Case 7
                Return WebWordUtility.GetWord(211)
            Case 8
                Return WebWordUtility.GetWord(213)
            Case 9
                Return WebWordUtility.GetWord(214)
            Case 10
                Return WebWordUtility.GetWord(215)
        End Select

        Return ""
    End Function

    ''' <summary>
    ''' スクリプト実行
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetScript()
        Dim script As New StringBuilder
        script.Append("<script type='text/javascript'>")
        script.Append("  printDialog()")
        script.Append("</script>")

        Dim cs As ClientScriptManager = Page.ClientScript
        cs.RegisterStartupScript(Me.GetType(), "printScript", script.ToString())
    End Sub

    ''' <summary>
    ''' 画面に表示する金額を取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetPrice(ByVal price As String, ByVal tani As String) As String

        If String.IsNullOrEmpty(price) Or "".Equals(price) Then
            Return price
        Else
            Return ENMARK & price & tani
        End If

    End Function
End Class
