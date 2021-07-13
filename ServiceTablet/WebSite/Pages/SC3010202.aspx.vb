Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.iCROP.BizLogic.SC3010202
Imports Toyota.eCRB.iCROP.DataAccess.SC3010202
Imports System.Globalization


Partial Class PagesSC3010202
    Inherits BasePage

#Region "定数"

    ''' <summary>
    ''' 画面ID
    ''' </summary>
    Private Const APPLICATIONID As String = "SC3010202"

    ''' <summary>
    ''' 進捗率グラフの比重
    ''' 例：進捗率が100%の場合、100 * 0.9(比重) = 90pxとなる。
    ''' </summary>
    Private Const DEFAULT_GRAPH_WIDTH_WEIGHT As Double = 0.9D


    ''' <summary>
    ''' グラフ幅の最大長(px)
    ''' </summary>
    Private Const MAX_GRAPH_WIDTH As Integer = 180

    ''' <summary>
    ''' 進捗率の表示位置の判断値(px)。
    ''' 進捗率がこの値より少ない場合、グラフ外に表示する。
    ''' 進捗率がこの値以上の場合、グラフ中に表示する。
    ''' </summary>
    Private Const DEFAULT_GRAPHVALUE_POSITION_BORDERLINE As Integer = 150

    ''' <summary>
    ''' 進捗率をグラフ外に表示するクラス名
    ''' </summary>
    Private Const CSS_STYLENAME_GRAPHVALUE_POSITION_OUT As String = "addText_GraphOut"

    ''' <summary>
    ''' 進捗率をグラフ内に表示するクラス名
    ''' </summary>
    Private Const CSS_STYLENAME_GRAPHVALUE_POSITION_IN As String = "addText_GraphIn"

    ''' <summary>
    ''' アプリケーションデータ名 - 基本アプリケーションデータ名
    ''' </summary>
    Private Const SESSION_BASE_APPLICATION As String = "SC3010202_APPDATA_"


#End Region

#Region "メンバ変数"

    ''' <summary>
    ''' 来店項目のグラフ幅(px)
    ''' </summary>
    Private GraphWidthWalkInValue As String

    ''' <summary>
    ''' 見積項目のグラフ幅(px)
    ''' </summary>
    Private GraphWidthQuotationValue As String

    ''' <summary>
    ''' 試乗項目のグラフ幅(px)
    ''' </summary>
    Private GraphWidthTestDriveValue As String

    ''' <summary>
    ''' 査定項目のグラフ幅(px)
    ''' </summary>
    Private GraphWidthEvaluationValue As String

    ''' <summary>
    ''' 納車項目のグラフ幅(px)
    ''' </summary>
    Private GraphWidthDeliveryValue As String

    ''' <summary>
    ''' 受付項目のグラフ幅(px)
    ''' </summary>
    Private GraphWidthColdValue As String

    ''' <summary>
    ''' 見込項目のグラフ幅(px)
    ''' </summary>
    Private GraphWidthWarmValue As String

    ''' <summary>
    ''' ホット項目のグラフ幅(px)
    ''' </summary>
    Private GraphWidthHotValue As String

    ''' <summary>
    ''' 受注項目のグラフ幅(px)
    ''' </summary>
    Private GraphWidthOrderValue As String

    ''' <summary>
    ''' 販売項目のグラフ幅(px)
    ''' </summary>
    Private GraphWidthSaleValue As String

#End Region

#Region "getter/setter"

    ''' <summary>
    ''' 来店項目のグラフ幅(px)
    ''' </summary>
    ''' <value>来店項目のグラフ幅(px)</value>
    ''' <remarks>読み取り専用</remarks>
    Public ReadOnly Property GraphWidthWalkIn() As String
        Get
            Return GraphWidthWalkInValue
        End Get
    End Property

    ''' <summary>
    ''' 見積項目のグラフ幅(px)
    ''' </summary>
    ''' <value>見積項目のグラフ幅(px)</value>
    ''' <remarks>読み取り専用</remarks>
    Public ReadOnly Property GraphWidthQuotation() As String
        Get
            Return GraphWidthQuotationValue
        End Get
    End Property

    ''' <summary>
    ''' 試乗項目のグラフ幅(px)
    ''' </summary>
    ''' <value>試乗項目のグラフ幅(px)</value>
    ''' <remarks>読み取り専用</remarks>
    Public ReadOnly Property GraphWidthTestDrive() As String
        Get
            Return GraphWidthTestDriveValue
        End Get
    End Property

    ''' <summary>
    ''' 査定項目のグラフ幅(px)
    ''' </summary>
    ''' <value>査定項目のグラフ幅(px)</value>
    ''' <remarks>読み取り専用</remarks>
    Public ReadOnly Property GraphWidthEvaluation() As String
        Get
            Return GraphWidthEvaluationValue
        End Get
    End Property

    ''' <summary>
    ''' 納車項目のグラフ幅(px)
    ''' </summary>
    ''' <value>納車項目のグラフ幅(px)</value>
    ''' <remarks>読み取り専用</remarks>
    Public ReadOnly Property GraphWidthDelivery() As String
        Get
            Return GraphWidthDeliveryValue
        End Get
    End Property

    ''' <summary>
    ''' 受付項目のグラフ幅(px)
    ''' </summary>
    ''' <value>受付項目のグラフ幅(px)</value>
    ''' <remarks>読み取り専用</remarks>
    Public ReadOnly Property GraphWidthCold() As String
        Get
            Return GraphWidthColdValue
        End Get
    End Property

    ''' <summary>
    ''' 見込項目のグラフ幅(px)
    ''' </summary>
    ''' <value>見込項目のグラフ幅(px)</value>
    ''' <remarks>読み取り専用</remarks>
    Public ReadOnly Property GraphWidthWarm() As String
        Get
            Return GraphWidthWarmValue
        End Get
    End Property

    ''' <summary>
    ''' ホット項目のグラフ幅(px)
    ''' </summary>
    ''' <value>ホット項目のグラフ幅(px)</value>
    ''' <remarks>読み取り専用</remarks>
    Public ReadOnly Property GraphWidthHot() As String
        Get
            Return GraphWidthHotValue
        End Get
    End Property

    ''' <summary>
    ''' 受注項目のグラフ幅(px)
    ''' </summary>
    ''' <value>受注項目のグラフ幅(px)</value>
    ''' <remarks>読み取り専用</remarks>
    Public ReadOnly Property GraphWidthOrder() As String
        Get
            Return GraphWidthOrderValue
        End Get
    End Property

    ''' <summary>
    ''' 販売項目のグラフ幅(px)
    ''' </summary>
    ''' <value>販売項目のグラフ幅(px)</value>
    ''' <remarks>読み取り専用</remarks>
    Public ReadOnly Property GraphWidthSale() As String
        Get
            Return GraphWidthSaleValue
        End Get
    End Property
#End Region


    ''' <summary>
    ''' ページロード時の処理です。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks>Postで呼ばれることを想定していないのでisPostBack判定は行いません</remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Logger.Debug("Page_Load Start")
        Dim culture As CultureInfo = New CultureInfo("en")

        'スタッフ情報を取得
        Dim staffInfo As StaffContext = StaffContext.Current
        Dim ApplicationData As SC3010202Sturuct

        'アプリケーション名の作成
        Dim ApplicationName = SESSION_BASE_APPLICATION + staffInfo.Account

        '画面文言の設定
        SetWord()

        'バッチ更新時間を
        Dim BusinessLogic As SC3010202BusinessLogic = New SC3010202BusinessLogic
        Dim BatchStartTime As Date = BusinessLogic.GetBatchStartTime

        'アプリケーションデータで保持しているバッチ更新時間(①)とバッチ更新時間(②)を比較
        'バッチ更新時間(②)が大きい場合DBから値を取得する。
        'そうでない場合はアプリケーションデータで保持している情報を取得する。
        Dim isUseApplicationData As Boolean

        'アプリケーションデータのtemp元が違う場合がある。
        'その場合、アプリケーションデータのコピーができないので
        'tryCastしておく
        ApplicationData = TryCast(Application(ApplicationName), SC3010202Sturuct)
        If Not IsNothing(ApplicationData) Then
            Dim SessionStartTime As Date = ApplicationData.StartTime
            If Date.Compare(BatchStartTime, SessionStartTime).Equals(1) Then
                'バッチの実行時間の方が大きい
                'アプリケーションデータに更新時間を設定
                ApplicationData.StartTime = BatchStartTime
                isUseApplicationData = False
            Else
                'アプリケーションデータの実行時間の方が大きい（または同じ)
                isUseApplicationData = True
            End If
        Else
            'アプリケーションデータがNothingになっているので生成
            ApplicationData = New SC3010202Sturuct

            'アプリケーションデータが登録されていない場合はDBから取得する
            ApplicationData.StartTime = BatchStartTime
            isUseApplicationData = False
        End If

        Logger.Debug("Batch(MC30102) EndTime is " + BatchStartTime.ToShortTimeString)
        Logger.Debug("Use SessionData:" + isUseApplicationData.ToString())

        '画面項目に反映(更新日時)
        Label_UpdateTime.Text = DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, BatchStartTime, staffInfo.DlrCD)

        '目標値を取得
        Dim SC3010202TargetTable As SC3010202DataSet.SC3010202TargetDataTable
        If isUseApplicationData Then
            SC3010202TargetTable = ApplicationData.TargetTable
        Else
            SC3010202TargetTable = BusinessLogic.GetTargetInfo(staffInfo)
        End If

        '目標情報を判定
        Dim SC3010202TargetRow As SC3010202DataSet.SC3010202TargetRow
        If Not IsNothing(SC3010202TargetTable) And SC3010202TargetTable.Count > 0 Then
            SC3010202TargetRow = SC3010202TargetTable.Rows(0)
        Else
            'データが内場合は空の(すべて0)のデータテーブルを準備する
            SC3010202TargetRow = SC3010202TargetTable.NewSC3010202TargetRow()
        End If

        '画面項目に反映(目標値)
        SetTargetDisplayValue(SC3010202TargetRow, culture)

        'アプリケーションデータに情報をセット
        ApplicationData.TargetTable = SC3010202TargetTable

        '実績情報を取得
        Dim SC3010202ResultTable As SC3010202DataSet.SC3010202ResultDataTable
        If isUseApplicationData Then
            SC3010202ResultTable = ApplicationData.ResultTable
        Else
            SC3010202ResultTable = BusinessLogic.GetResultInfo(staffInfo)
        End If

        Dim sc3010202ResultRow As SC3010202DataSet.SC3010202ResultRow
        If Not IsNothing(SC3010202ResultTable) And SC3010202ResultTable.Count > 0 Then
            sc3010202ResultRow = SC3010202ResultTable.Rows(0)
        Else
            'データが内場合は空の(すべて0)のデータテーブルを準備する
            sc3010202ResultRow = SC3010202ResultTable.NewSC3010202ResultRow()
        End If

        '画面項目に反映(実績値)
        SetResultDisplayValue(sc3010202ResultRow, culture)

        '画面に進捗率を反映
        SetResultValue(SC3010202TargetRow, sc3010202ResultRow)

        'グラフの非表示設定
        SetHidenGraph()

        'アプリケーションデータに情報をセット
        ApplicationData.ResultTable = SC3010202ResultTable

        'DBからデータを取得した場合アプリケーションデータに取得情報を設定
        If Not isUseApplicationData Then
            Application(ApplicationName) = ApplicationData
        End If

        'ステップ1は来店非動作のため強制的に空白にする
        Label_WalkIn_Percent.Visible = False
        Div_WalkIn_Graph.Visible = False

        Logger.Debug("Page_Load End")
    End Sub

    ''' <summary>
    ''' 文言の設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetWord()
        Label_Title.Text = WebWordUtility.GetWord(APPLICATIONID, 1)
        Label_UpdateText.Text = WebWordUtility.GetWord(APPLICATIONID, 2)
        Label_Subtitle_Action.Text = WebWordUtility.GetWord(APPLICATIONID, 3)
        Label_WalkIn.Text = WebWordUtility.GetWord(APPLICATIONID, 4)
        Label_Quotation.Text = WebWordUtility.GetWord(APPLICATIONID, 5)
        Label_TestDrive.Text = WebWordUtility.GetWord(APPLICATIONID, 6)
        Label_Evaluation.Text = WebWordUtility.GetWord(APPLICATIONID, 7)
        Label_Delivery.Text = WebWordUtility.GetWord(APPLICATIONID, 8)
        Label_Subtitle_Prospect.Text = WebWordUtility.GetWord(APPLICATIONID, 9)
        Label_Cold.Text = WebWordUtility.GetWord(APPLICATIONID, 10)
        Label_Warm.Text = WebWordUtility.GetWord(APPLICATIONID, 11)
        Label_Hot.Text = WebWordUtility.GetWord(APPLICATIONID, 12)
        Label_Subtitle_Sale.Text = WebWordUtility.GetWord(APPLICATIONID, 13)
        Label_Order.Text = WebWordUtility.GetWord(APPLICATIONID, 14)
        Label_Sale.Text = WebWordUtility.GetWord(APPLICATIONID, 15)
    End Sub

    ''' <summary>
    ''' 画面項目：目標値に値を設定
    ''' </summary>
    ''' <param name="SC3010202TargetRow">目標値テーブル</param>
    ''' <param name="culture">国情報</param>
    ''' <remarks></remarks>
    Private Sub SetTargetDisplayValue(ByVal SC3010202TargetRow As SC3010202DataSet.SC3010202TargetRow, ByVal culture As CultureInfo)
        With SC3010202TargetRow
            'Label_WalkIn_Target.Text = .WalkIn.ToString(culture).Substring(If(.WalkIn.ToString(culture).Length < 3, 0, .WalkIn.ToString(culture).Length - 3))
            Label_Quotation_Target.Text = .QUOTATION.ToString(Culture).Substring(If(.QUOTATION.ToString(Culture).Length < 3, 0, .QUOTATION.ToString(Culture).Length - 3))
            Label_TestDrive_Target.Text = .TESTDRIVE.ToString(Culture).Substring(If(.TESTDRIVE.ToString(Culture).Length < 3, 0, .TESTDRIVE.ToString(Culture).Length - 3))
            Label_Evaluation_Target.Text = .EVALUATION.ToString(Culture).Substring(If(.EVALUATION.ToString(Culture).Length < 3, 0, .EVALUATION.ToString(Culture).Length - 3))
            Label_Delivery_Target.Text = .DELIVERY.ToString(Culture).Substring(If(.DELIVERY.ToString(Culture).Length < 3, 0, .DELIVERY.ToString(Culture).Length - 3))
            Label_Cold_Target.Text = .COLD.ToString(Culture).Substring(If(.COLD.ToString(Culture).Length < 3, 0, .COLD.ToString(Culture).Length - 3))
            Label_Warm_Target.Text = .WARM.ToString(Culture).Substring(If(.WARM.ToString(Culture).Length < 3, 0, .WARM.ToString(Culture).Length - 3))
            Label_Hot_Target.Text = .HOT.ToString(Culture).Substring(If(.HOT.ToString(Culture).Length < 3, 0, .HOT.ToString(Culture).Length - 3))
            Label_Order_Target.Text = .ORDERS.ToString(Culture).Substring(If(.ORDERS.ToString(Culture).Length < 3, 0, .ORDERS.ToString(Culture).Length - 3))
            Label_Sale_Target.Text = .SALES.ToString(Culture).Substring(If(.SALES.ToString(Culture).Length < 3, 0, .SALES.ToString(Culture).Length - 3))
        End With
    End Sub

    ''' <summary>
    ''' 画面項目：目標値に値を設定
    ''' </summary>
    ''' <param name="SC3010202ResultRow"> 実績値情報</param>
    ''' <param name="culture">国情報</param>
    ''' <remarks></remarks>
    Private Sub SetResultDisplayValue(ByVal SC3010202ResultRow As SC3010202DataSet.SC3010202ResultRow, ByVal culture As CultureInfo)
        With SC3010202ResultRow
            'Label_WalkIn_Result.Text = .WalkIn.ToString(culture).Substring(If(.WalkIn.ToString(culture).Length < 3, 0, .WalkIn.ToString(culture).Length - 3))
            Label_Quotation_Result.Text = .QUOTATION.ToString(Culture).Substring(If(.QUOTATION.ToString(Culture).Length < 3, 0, .QUOTATION.ToString(Culture).Length - 3))
            Label_TestDrive_Result.Text = .TESTDRIVE.ToString(Culture).Substring(If(.TESTDRIVE.ToString(Culture).Length < 3, 0, .TESTDRIVE.ToString(Culture).Length - 3))
            Label_Evaluation_Result.Text = .EVALUATION.ToString(Culture).Substring(If(.EVALUATION.ToString(Culture).Length < 3, 0, .EVALUATION.ToString(Culture).Length - 3))
            Label_Delivery_Result.Text = .DELIVERY.ToString(Culture).Substring(If(.DELIVERY.ToString(Culture).Length < 3, 0, .DELIVERY.ToString(Culture).Length - 3))
            Label_Cold_Result.Text = .COLD.ToString(Culture).Substring(If(.COLD.ToString(Culture).Length < 3, 0, .COLD.ToString(Culture).Length - 3))
            Label_Warm_Result.Text = .WARM.ToString(Culture).Substring(If(.WARM.ToString(Culture).Length < 3, 0, .WARM.ToString(Culture).Length - 3))
            Label_Hot_Result.Text = .HOT.ToString(Culture).Substring(If(.HOT.ToString(Culture).Length < 3, 0, .HOT.ToString(Culture).Length - 3))
            Label_Order_Result.Text = .ORDERS.ToString(Culture).Substring(If(.ORDERS.ToString(Culture).Length < 3, 0, .ORDERS.ToString(Culture).Length - 3))
            Label_Sale_Result.Text = .SALES.ToString(Culture).Substring(If(.SALES.ToString(Culture).Length < 3, 0, .SALES.ToString(Culture).Length - 3))
        End With
    End Sub

    ''' <summary>
    ''' 進捗率の非表示の設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetHidenGraph()
        '進捗率が0%のグラフを非表示にする
        If GraphWidthWalkInValue.Equals("0") Then : Div_WalkIn_Graph.Visible = False : End If
        If GraphWidthQuotationValue.Equals("0") Then : Div_Quotation_Graph.Visible = False : End If
        If GraphWidthTestDriveValue.Equals("0") Then : Div_TestDrive_Graph.Visible = False : End If
        If GraphWidthEvaluationValue.Equals("0") Then : Div_Evaluation_Graph.Visible = False : End If
        If GraphWidthDeliveryValue.Equals("0") Then : Div_Delivery_Graph.Visible = False : End If
        If GraphWidthColdValue.Equals("0") Then : Div_Cold_Graph.Visible = False : End If
        If GraphWidthWarmValue.Equals("0") Then : Div_Warm_Graph.Visible = False : End If
        If GraphWidthHotValue.Equals("0") Then : Div_Hot_Graph.Visible = False : End If
        If GraphWidthOrderValue.Equals("0") Then : Div_Order_Graph.Visible = False : End If
        If GraphWidthSaleValue.Equals("0") Then : Div_Sale_Graph.Visible = False : End If
    End Sub

    ''' <summary>
    ''' 実績と進捗率を設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetResultValue(ByVal SC3010202TargetRow As SC3010202DataSet.SC3010202TargetRow, ByVal sc3010202ResultRow As SC3010202DataSet.SC3010202ResultRow)
        '進捗率を計算
        Dim WalkIn_Persent As Integer = ConvertResultToPercent(SC3010202TargetRow.WalkIn, sc3010202ResultRow.WALKIN)
        Dim Quotation_Persent As Integer = ConvertResultToPercent(SC3010202TargetRow.QUOTATION, sc3010202ResultRow.QUOTATION)
        Dim TestDRive_Persent As Integer = ConvertResultToPercent(SC3010202TargetRow.TESTDRIVE, sc3010202ResultRow.TESTDRIVE)
        Dim Evaluation_Persent As Integer = ConvertResultToPercent(SC3010202TargetRow.EVALUATION, sc3010202ResultRow.EVALUATION)
        Dim Delivery_Persent As Integer = ConvertResultToPercent(SC3010202TargetRow.DELIVERY, sc3010202ResultRow.DELIVERY)
        Dim Cold_Persent As Integer = ConvertResultToPercent(SC3010202TargetRow.COLD, sc3010202ResultRow.COLD)
        Dim Warm_Persent As Integer = ConvertResultToPercent(SC3010202TargetRow.WARM, sc3010202ResultRow.WARM)
        Dim Hot_Persent As Integer = ConvertResultToPercent(SC3010202TargetRow.HOT, sc3010202ResultRow.HOT)
        Dim Order_Persent As Integer = ConvertResultToPercent(SC3010202TargetRow.ORDERS, sc3010202ResultRow.ORDERS)
        Dim Sale_Persent As Integer = ConvertResultToPercent(SC3010202TargetRow.SALES, sc3010202ResultRow.SALES)

        '画面項目に反映（進捗率)
        Dim culture As CultureInfo = New CultureInfo("en")
        Label_WalkIn_Percent.Text = WalkIn_Persent.ToString(culture)
        Label_Quotation_Percent.Text = Quotation_Persent.ToString(culture)
        Label_TestDrive_Percent.Text = TestDRive_Persent.ToString(culture)
        Label_Evaluation_Percent.Text = Evaluation_Persent.ToString(culture)
        Label_Delivery_Percent.Text = Delivery_Persent.ToString(culture)
        Label_Cold_Percent.Text = Cold_Persent.ToString(culture)
        Label_Warm_Percent.Text = Warm_Persent.ToString(culture)
        Label_Hot_Percent.Text = Hot_Persent.ToString(culture)
        Label_Order_Percent.Text = Order_Persent.ToString(culture)
        Label_Sale_Percent.Text = Sale_Persent.ToString(culture)

        '画面項目に反映（進捗率)
        GraphWidthWalkInValue = CovertPercentToWidth(WalkIn_Persent).ToString(culture)
        GraphWidthQuotationValue = CovertPercentToWidth(Quotation_Persent).ToString(culture)
        GraphWidthTestDriveValue = CovertPercentToWidth(TestDRive_Persent).ToString(culture)
        GraphWidthEvaluationValue = CovertPercentToWidth(Evaluation_Persent).ToString(culture)
        GraphWidthDeliveryValue = CovertPercentToWidth(Delivery_Persent).ToString(culture)
        GraphWidthColdValue = CovertPercentToWidth(Cold_Persent).ToString(culture)
        GraphWidthWarmValue = CovertPercentToWidth(Warm_Persent).ToString(culture)
        GraphWidthHotValue = CovertPercentToWidth(Hot_Persent).ToString(culture)
        GraphWidthOrderValue = CovertPercentToWidth(Order_Persent).ToString(culture)
        GraphWidthSaleValue = CovertPercentToWidth(Sale_Persent).ToString(culture)
    End Sub

    ''' <summary>
    ''' 目標と実績から進捗率を計算する
    ''' </summary>
    ''' <param name="target">目標値</param>
    ''' <param name="result">実績値</param>
    ''' <returns>進捗率</returns>
    ''' <remarks>小数点第一位を四捨五入</remarks>
    Protected Shared Function ConvertResultToPercent(ByVal target As Integer, ByVal result As Integer) As Integer

        '引数チェック
        '目標値または実績値が0の場合は計算を行わない
        If target.Equals(0) Or result.Equals(0) Then
            Return 0
        End If

        '現在の目標値の係数を計算
        '目標数 * (経過日数/今月の日数)
        Dim nowTarget As Decimal = Decimal.Multiply(target, Decimal.Divide(Now.Day, DateTime.DaysInMonth(Now.Year, Now.Month)))

        Dim persent As New Decimal(0)
        '実績を目標で除算し進捗率を計算
        persent = Decimal.Divide(result, nowTarget) * 100
        '進捗率を丸める(四捨五入)
        Return Decimal.ToInt32(Decimal.Round(persent, 0))
    End Function


    ''' <summary>
    ''' 実績進捗率からグラフの長さ(px)を出力する
    ''' </summary>
    ''' <param name="percent">進捗率</param>
    ''' <returns>グラフの長さ(px)</returns>
    ''' <remarks></remarks>
    Protected Shared Function CovertPercentToWidth(ByVal percent As Integer) As Integer

        Dim width As Integer = 0

        If percent > 0 Then
            width = percent * DEFAULT_GRAPH_WIDTH_WEIGHT

            'グラフの長さが180pxを超えさせない
            If width >= MAX_GRAPH_WIDTH Then
                width = MAX_GRAPH_WIDTH
            End If
        End If

        Return width

    End Function

    ''' <summary>
    ''' 進捗率に適用するCSSクラス名を返却する。
    ''' </summary>
    ''' <param name="value">進捗率(px)</param>
    ''' <return>CSSクラス名</return>
    ''' <remarks>valueが適正な値でない場合クラス名を返却しない</remarks>
    Protected Shared Function CssClassNameForGraphBar(ByVal value As String) As String
        'value値の引数チェック
        Dim graphPersent As Integer
        If Integer.TryParse(value, graphPersent) Then
            '数値に変換可能

            '基準値との大小を判断
            If graphPersent < DEFAULT_GRAPHVALUE_POSITION_BORDERLINE Then
                '基準値より小さい、グラフ外に表示
                Return CSS_STYLENAME_GRAPHVALUE_POSITION_OUT
            Else
                '基準値以上のため、グラフ内に表示
                Return CSS_STYLENAME_GRAPHVALUE_POSITION_IN
            End If
        Else
            '数値以外の値が入力された為、空白を返却する
            Return String.Empty
        End If

    End Function

    ''' <summary>
    ''' SC3010202のアプリケーションデータに保存するデータ構造体
    ''' </summary>
    ''' <remarks></remarks>
    Private Class SC3010202Sturuct
        'バッチ更新日時
        Private StartTimeValue As Date
        '目標情報
        Private TargetTableValue As SC3010202DataSet.SC3010202TargetDataTable
        '実績情報
        Private ResultTableValue As SC3010202DataSet.SC3010202ResultDataTable

        'getter/setter

        ''' <summary>
        ''' バッチ更新日時
        ''' </summary>
        ''' <value>バッチ更新日時</value>
        ''' <remarks></remarks>
        Public Property StartTime() As Date
            Get
                Return StartTimeValue
            End Get
            Set(ByVal value As Date)
                StartTimeValue = value
            End Set
        End Property

        ''' <summary>
        ''' 実績情報
        ''' </summary>
        ''' <value>実績情報データテーブル</value>
        ''' <remarks></remarks>
        Public Property TargetTable() As SC3010202DataSet.SC3010202TargetDataTable
            Get
                Return TargetTableValue
            End Get
            Set(ByVal value As SC3010202DataSet.SC3010202TargetDataTable)
                TargetTableValue = value
            End Set
        End Property

        ''' <summary>
        ''' 目標情報
        ''' </summary>
        ''' <value>目標情報データテーブル</value>
        ''' <remarks></remarks>
        Public Property ResultTable() As SC3010202DataSet.SC3010202ResultDataTable
            Get
                Return ResultTableValue
            End Get
            Set(ByVal value As SC3010202DataSet.SC3010202ResultDataTable)
                ResultTableValue = value
            End Set
        End Property
        Public Sub New()
            StartTime = New Date
        End Sub
    End Class
End Class

