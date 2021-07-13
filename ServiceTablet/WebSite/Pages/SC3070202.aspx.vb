Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web.Controls
Imports Toyota.eCRB.Estimate.Quotation.BizLogic
Imports Toyota.eCRB.Estimate.Quotation.DataAccess
Imports Toyota.eCRB.Estimate.Quotation.DataAccess.IC3070201DataSet
Imports System.Globalization
Imports System.Reflection

Partial Class Pages_SC3070202
    Inherits BasePage

#Region "定数"
    ''' <summary>
    ''' 書式　数値(金額)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FORMAT_NUMBER As String = "#,#0.00"

    ''' <summary>
    ''' 日付変換ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONVERT_ID As Integer = 3

    ''' <summary>
    ''' セッションキー　見積管理ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_ESTID As String = "estimateId"

    ''' <summary>
    ''' セッションキー　支払い方法
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_PAYMETHOD As String = "paymentMethod"

    ''' <summary>
    ''' データテーブル名　見積情報
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TBL_ESTIMATEINFO As String = "SC3070202EstimateInfo"

    ''' <summary>
    ''' データテーブル名　オプション情報
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TBL_OPTIONINFO As String = "SC3070202EstVclOptionInfo"

    ''' <summary>
    ''' データテーブル名　下取り車両情報
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TBL_TRADEINCARINFO As String = "SC3070202EstTradeInCarInfo"

    ''' <summary>
    ''' 行が増えない場合の画面の高さ(pixel)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const BASE_HEIGHT As Integer = 1700

    ''' <summary>
    ''' 行が増えない場合のスクロールの高さ(pixel)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const BASE_SCROLL_HEIGHT As Integer = 1220

    ''' <summary>
    ''' 画面が1行増える毎に画面に追加する高さ(pixel)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ADD_HEIGHT As Integer = 70

    ''' <summary>
    ''' 画面が1行増える毎にスクロールに追加する高さ(pixel)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SCROLL_ADD_HEIGHT As Integer = 35

#End Region

#Region "イベント処理"
    ''' <summary>
    ''' ロードの処理を実施します。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            '見積作成画面からセッション情報を取得
            Dim sessionTbl As SC3070202DataSet.SC3070202SessionDataTable
            sessionTbl = Me.GetSession()

            Dim msgID As Integer = 0
            Dim bizClass As New SC3070202BusinessLogic
            Dim estDataSet As SC3070202DataSet

            '初期表示情報取得
            estDataSet = bizClass.GetInitialData(sessionTbl)

            If estDataSet IsNot Nothing Then
                '画面コントロールに値を設定
                SetControl(estDataSet)
            Else
                'エラーメッセージ表示
                Me.ShowMessageBox(bizClass.ResultId)
            End If

        End If
    End Sub

    ''' <summary>
    ''' 戻るボタンを押下時の処理を実行します。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub CloseButton_Click(sender As Object, e As System.EventArgs) Handles closeButton.Click

        '見積作成画面へ戻る
        Me.RedirectPrevScreen()

    End Sub

    ''' <summary>
    ''' 印刷ボタンを押下時の処理を実行します。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub PrintButton_Click(sender As Object, e As System.EventArgs) Handles printButton.Click

        '見積作成画面からセッション情報を取得
        Dim sessionTbl As SC3070202DataSet.SC3070202SessionDataTable
        sessionTbl = Me.GetSession()

        Dim bizClass As New SC3070202BusinessLogic

        '見積テーブルの見積印刷日更新処理
        bizClass.UpdatePrintDate(sessionTbl)

        '印刷処理スクリプト実行
        Me.SetScript()

    End Sub
#End Region

#Region "Privateメソッド"
    ''' <summary>
    ''' 画面コントロールに値を設定
    ''' </summary>
    ''' <param name="estimateDataSet"></param>
    ''' <remarks></remarks>
    Private Sub SetControl(ByVal estimateDataSet As SC3070202DataSet)

        '見積情報テーブル
        Dim estTblData As SC3070202DataSet.SC3070202EstimateInfoDataTable
        Dim estTblDataRow As SC3070202DataSet.SC3070202EstimateInfoRow
        estTblData = estimateDataSet.Tables(TBL_ESTIMATEINFO)
        estTblDataRow = estTblData.Rows(0)

        'お客様情報
        Me.customerNameWordLabel.Text = estTblDataRow.CUSTOMERNM
        Me.vehicleWordLabel.Text = estTblDataRow.CARTYPE
        Me.gradeSpecWordLabel.Text = estTblDataRow.GRADESPEC
        Me.dateWordLabel.Text = estTblDataRow.SYSDATE
        Me.dealerWordLabel.Text = estTblDataRow.BRANCHNM
        Me.telNoWordLabel.Text = estTblDataRow.TELNO
        Me.salesStaffWordLabel.Text = estTblDataRow.SALESSTAFF

        '車両情報
        Me.bodyTypeWordLabel.Text = estTblDataRow.BODYTYPE
        Me.displacementWordLabel.Text = estTblDataRow.DISPLACEMENT
        Me.drivingWordLabel.Text = estTblDataRow.DRIVING
        Me.missionWordLabel.Text = estTblDataRow.MISSION
        Me.outColorWordLabel.Text = estTblDataRow.OUTERCOLOR
        Me.inColorWordLabel.Text = estTblDataRow.INNERCOLOR
        Me.carNoWordLabel.Text = estTblDataRow.CARNO

        '車両価格
        Me.carBodyPriceWordLabel.Text = Me.GetCheckZeroDouble(estTblDataRow.BASEPRICE)
        '2011/01/05 myose del start
        'Me.optionPriceWordLabel.Text = Me.GetCheckZeroDouble(estTblDataRow.OPTIONPRICE)
        '2011/01/05 myose del end
        Me.discountWordLabel.Text = estTblDataRow.DISCOUNT.ToString(FORMAT_NUMBER, CultureInfo.CurrentCulture)
        Me.summaryCarPriceWordLabel.Text = Me.GetCheckZeroDouble(estTblDataRow.CARPRICESAMMARY)

        'オプション明細
        Me.SetHiddenOptionDetail(estimateDataSet)
        Me.optionPriceSammaryWordLabel.Text = Me.GetCheckZeroDouble(estTblDataRow.OPTIONPRICE)

        'メモ
        Me.memoWordLabel.Text = estTblDataRow.MEMO

        '諸費用
        '2012/01/17 myose modify start
        'Me.carBuyingTaxWordLabel.Text = Me.GetCheckZeroDouble(estTblDataRow.ITEMPRICE1)
        'Me.expenseRegistWordLabel.Text = Me.GetCheckZeroDouble(estTblDataRow.ITEMPRICE2)
        'Me.expenseSammaryWordLabel.Text = Me.GetCheckZeroDouble(estTblDataRow.EXPENSESAMMARY)
        Me.carBuyingTaxWordLabel.Text = estTblDataRow.ITEMPRICE1.ToString(FORMAT_NUMBER, CultureInfo.CurrentCulture)
        Me.expenseRegistWordLabel.Text = estTblDataRow.ITEMPRICE2.ToString(FORMAT_NUMBER, CultureInfo.CurrentCulture)
        Me.expenseSammaryWordLabel.Text = estTblDataRow.EXPENSESAMMARY.ToString(FORMAT_NUMBER, CultureInfo.CurrentCulture)
        '2012/01/17 myose modify end

        '保険
        Me.insuranceCompanyWordLabel.Text = estTblDataRow.INSURANCECOM
        Me.insuranceTypeWordLabel.Text = estTblDataRow.INSURANCETYPE
        Me.yearlyAmountWordLabel.Text = Me.GetCheckZeroDouble(estTblDataRow.YEARLYAMOUNT)

        'お支払い方法
        Me.paymentLabel.Text = WebWordUtility.GetWord(28) + Space(1) + WebWordUtility.GetWord(1) + Space(1) + Me.GetPaymentMethod(estTblDataRow)
        Me.financeCompanyWordLabel.Text = estTblDataRow.FINANCECOM
        Me.periodWordLabel.Text = Me.GetCheckZeroShort(estTblDataRow.PERIOD)
        Me.monthlyWordLabel.Text = Me.GetCheckZeroDouble(estTblDataRow.MONTHLY)
        Me.depositWordLabel.Text = Me.GetCheckZeroDouble(estTblDataRow.DEPOSIT)
        Me.bonusWordLabel.Text = Me.GetCheckZeroDouble(estTblDataRow.BONUS)
        Me.firstPaymentDayWordLabel.Text = Me.GetCheckZeroShort(estTblDataRow.FIRSTPAYMENT)

        'お支払い金額
        Me.tradeInSummaryValueWordLabel.Text = Me.MakeMinusValue(estTblDataRow.TRADEINSAMMARY)
        Me.carDeliveryDateWordLabel.Text = Me.GetCheckMinValDate(estTblDataRow.DELIVERY)
        Me.paymentSummaryWordLabel.Text = Me.GetCheckZeroDouble(estTblDataRow.PAYMENTSAMMARY)

        '印刷ボタン
        Me.printButton.Text = WebWordUtility.GetWord(46)

        '下取り車両
        Me.SetHiddenTradeInCar(estimateDataSet)

        '値引き額欄表示/非表示のフラグセット
        Me.SetHiddenDiscountFlg(estTblDataRow.DISCOUNT)

        '画面の高さをHiddenに格納する
        Me.SetHiddenHeightPixel(estTblDataRow.ADDROWCOUNT)

    End Sub

    ''' <summary>
    ''' セッション情報を取得する
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetSession() As SC3070202DataSet.SC3070202SessionDataTable
        Using tbl As New SC3070202DataSet.SC3070202SessionDataTable
            Dim tblRow As SC3070202DataSet.SC3070202SessionRow = tbl.NewSC3070202SessionRow
            '見積管理ID取得
            tblRow.ESTIMATEID = DirectCast(Me.GetValue(ScreenPos.Current, SESSION_ESTID, False), Long)
            '支払方法区分取得
            tblRow.PAYMENTMETHOD = DirectCast(Me.GetValue(ScreenPos.Current, SESSION_PAYMETHOD, False), String)

            tbl.Rows.Add(tblRow)

            Return tbl
        End Using
    End Function

    ''' <summary>
    ''' お支払い方法を取得する
    ''' </summary>
    ''' <param name="estTblDataRow"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetPaymentMethod(ByVal estTblDataRow As SC3070202DataSet.SC3070202EstimateInfoRow) As String

        Dim rtnVal As String = String.Empty

        If estTblDataRow.PAYMENT.Equals("1") And (estTblDataRow.PAYMENTSAMMARY - estTblDataRow.DEPOSIT) = 0 Then
            '現金一括
            rtnVal = WebWordUtility.GetWord(31)
        ElseIf estTblDataRow.PAYMENT.Equals("1") Then
            '現金
            rtnVal = WebWordUtility.GetWord(30)
        Else
            'ローン
            rtnVal = WebWordUtility.GetWord(29)
        End If

        Return rtnVal

    End Function

    ''' <summary>
    ''' Double型のゼロチェックをして、ゼロならばEmptyを返却する
    ''' </summary>
    ''' <param name="dblVal"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetCheckZeroDouble(ByVal dblVal As Double) As String

        Dim rtnVal As String = String.Empty

        'ゼロでなければ値を数値型にフォーマットし、文字列型に変換して返却
        If Not dblVal = 0 Then
            rtnVal = dblVal.ToString(FORMAT_NUMBER, CultureInfo.CurrentCulture)
        End If

        Return rtnVal
    End Function

    ''' <summary>
    ''' Short型のゼロチェックをして、ゼロならばEmptyを返却する
    ''' </summary>
    ''' <param name="shtVal"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetCheckZeroShort(ByVal shtVal As Short) As String

        Dim rtnVal As String = String.Empty

        'ゼロでなければ値を文字列型に変換して返却
        If Not shtVal = 0 Then
            rtnVal = shtVal.ToString(CultureInfo.CurrentCulture)
        End If

        Return rtnVal
    End Function

    ''' <summary>
    ''' String型日付の最低値チェックをして、最低値ならばEmptyを返却する
    ''' </summary>
    ''' <param name="dateVal"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetCheckMinValDate(ByVal dateVal As String) As String

        Dim rtnVal As String = String.Empty

        '日付型の最低値でなければ、引数の文字列型日付を返却する
        If Not dateVal.Equals(DateTimeFunc.FormatDate(CONVERT_ID, Date.MinValue)) Then
            rtnVal = dateVal
        End If

        Return rtnVal
    End Function

    ''' <summary>
    ''' オプション明細のデータをHiddenにセットする
    ''' </summary>
    ''' <param name="estimateDataSet"></param>
    ''' <remarks></remarks>
    Private Sub SetHiddenOptionDetail(ByVal estimateDataSet As SC3070202DataSet)

        'オプション明細のデータ取得
        Dim strOpName As String = String.Empty
        Dim strOpPrice As String = String.Empty
        For Each opTblDataRow As SC3070202DataSet.SC3070202EstVclOptionInfoRow In estimateDataSet.Tables(TBL_OPTIONINFO).Rows()
            strOpName = strOpName + opTblDataRow.OPTIONNAME + "|"
            strOpPrice = strOpPrice + opTblDataRow.PRICE.ToString(FORMAT_NUMBER, CultureInfo.CurrentCulture) + "|"
        Next

        'オプションがない場合
        If String.IsNullOrEmpty(strOpName) Then
            '最低１３個はデータがあるようにする
            For i As Integer = 0 To 12
                strOpName = strOpName + Space(1) + "|"
                strOpPrice = strOpPrice + Space(1) + "|"
            Next
        Else
            'オプションが１３個未満の場合
            If strOpName.Split("|").Length < 14 Then
                '最低１３個はデータがあるようにする
                For j As Integer = 0 To (13 - strOpName.Split("|").Length)
                    strOpName = strOpName + Space(1) + "|"
                    strOpPrice = strOpPrice + Space(1) + "|"
                Next
            End If
        End If
        '末尾の「|」を削除
        strOpName = strOpName.Substring(0, strOpName.LastIndexOf("|", StringComparison.CurrentCulture))
        strOpPrice = strOpPrice.Substring(0, strOpPrice.LastIndexOf("|", StringComparison.CurrentCulture))

        'オプション明細情報をHiddenに格納
        Me.optionNameHiddenField.Value = strOpName
        Me.optionPriceHiddenField.Value = strOpPrice
    End Sub

    ''' <summary>
    ''' 下取り車両のデータをHiddenにセットする
    ''' </summary>
    ''' <param name="estimateDataSet"></param>
    ''' <remarks></remarks>
    Private Sub SetHiddenTradeInCar(ByVal estimateDataSet As SC3070202DataSet)

        '下取り車両データ取得
        Dim strTIName As String = String.Empty
        Dim strTIPrice As String = String.Empty

        '下取り車両の数だけループ
        For Each tiTblDataRow As SC3070202DataSet.SC3070202EstTradeInCarInfoRow In estimateDataSet.Tables(TBL_TRADEINCARINFO).Rows()
            strTIName = strTIName + tiTblDataRow.VEHICLENAME + "|"
            strTIPrice = strTIPrice + "-" + tiTblDataRow.ASSESSEDPRICE.ToString(FORMAT_NUMBER, CultureInfo.CurrentCulture) + "|"
        Next

        '下取り車両がない場合
        If String.IsNullOrEmpty(strTIName) Then
            '最低２つはデータがあるようにする
            strTIName = Space(1) + "|" + Space(1)
            strTIPrice = Space(1) + "|" + Space(1)
        Else
            '下取り車両が１台のとき
            If strTIName.Split("|").Length.Equals(2) Then
                strTIName = strTIName + Space(1)
                strTIPrice = strTIPrice + Space(1)
            Else
                '末尾の「|」を削除
                strTIName = strTIName.Substring(0, strTIName.LastIndexOf("|", StringComparison.CurrentCulture))
                strTIPrice = strTIPrice.Substring(0, strTIPrice.LastIndexOf("|", StringComparison.CurrentCulture))
            End If
        End If

        '下取り車両情報をHiddenに格納
        Me.tradeInNameHiddenField.Value = strTIName
        Me.tradeInPriceHiddenField.Value = strTIPrice
        Me.tradeInNumHiddenField.Value = strTIName.Split("|").Length

    End Sub

    ''' <summary>
    ''' 値引き額欄の表示/非表示判定フラグをセットする
    ''' </summary>
    ''' <param name="dblVal"></param>
    ''' <remarks></remarks>
    Private Sub SetHiddenDiscountFlg(ByVal dblVal As Double)

        'ゼロなら値引き額欄を削除する
        If dblVal = 0 Then
            Me.isDiscountHiddenField.Value = "0"
        Else
            Me.isDiscountHiddenField.Value = "1"
        End If
    End Sub

    ''' <summary>
    ''' 実行スクリプトをセットする
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetScript()
        Dim script As New StringBuilder
        script.Append("<script type='text/javascript'>")
        script.Append(" printDialog();")
        script.Append("</script>")

        Dim cs As ClientScriptManager = Page.ClientScript
        cs.RegisterStartupScript(Me.GetType(), "printScript", script.ToString())

    End Sub

    ''' <summary>
    ''' 画面とスクロールの高さ(pixel)をHiddenにセットする
    ''' </summary>
    ''' <param name="addRowCount"></param>
    ''' <remarks></remarks>
    Private Sub SetHiddenHeightPixel(ByVal addRowCount As String)

        '標準値に増えた行数分の高さを加算する
        Dim divAddHeight As Integer = ADD_HEIGHT * CInt(addRowCount)
        Dim scrollAddHeight As Integer = SCROLL_ADD_HEIGHT * CInt(addRowCount)
        Dim divHeight As Integer = BASE_HEIGHT + divAddHeight
        Dim scrollHeight As Integer = BASE_SCROLL_HEIGHT + scrollAddHeight

        '画面の高さ
        Me.DisplayHeightValueHiddenField.Value = divHeight.ToString(CultureInfo.CurrentCulture) + "px"
        'スクロールの高さ(実際に印刷される対象の高さ)
        Me.ScrollHeightValueHiddenField.Value = scrollHeight.ToString(CultureInfo.CurrentCulture) + "px"
    End Sub

    ''' <summary>
    ''' マイナス金額の文字列を作成する
    ''' </summary>
    ''' <param name="dblVal"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function MakeMinusValue(ByVal dblVal As Double) As String

        Dim sb As New StringBuilder

        If dblVal = 0 Then
            sb.Append(String.Empty)
        Else
            sb.Append("-")
            sb.Append(dblVal.ToString(FORMAT_NUMBER, CultureInfo.CurrentCulture))
        End If

        Return sb.ToString
    End Function
#End Region

End Class


