'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
' SC3140102.aspx.vb
'─────────────────────────────────────
'機能: ダッシュボード コードビハインド
'補足: 
'作成: 2012/01/16 KN 森下
'─────────────────────────────────────
Option Explicit On
Option Strict On

Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.iCROP.BizLogic.SC3140102
Imports Toyota.eCRB.iCROP.DataAccess.SC3140102
Imports System.Globalization

Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.BizLogic.IC3801005
Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.DataAccess.IC3801005

Partial Class Pages_SC3140102
    Inherits BasePage

#Region "定数"

    ''' <summary>
    ''' 画面ID
    ''' </summary>
    Private Const APPLICATIONID As String = "SC3140102"

    ''' <summary>
    ''' 進捗率グラフの比重
    ''' 例：進捗率が100%の場合、100 * 0.875(比重) = 87.5pxとなる。
    ''' </summary>
    Private Const DEFAULT_GRAPH_WIDTH_WEIGHT As Double = 0.875D

    ''' <summary>
    ''' グラフ幅の最大長(px)
    ''' </summary>
    Private Const MAX_GRAPH_WIDTH As Integer = 175

    ''' <summary>
    ''' 進捗率の表示位置の判断値(px)。
    ''' 進捗率がこの値より少ない場合、グラフ外に表示する。
    ''' 進捗率がこの値以上の場合、グラフ中に表示する。
    ''' </summary>
    Private Const DEFAULT_GRAPHVALUE_POSITION_BORDERLINE As Integer = 140

    ''' <summary>
    ''' 進捗率をグラフ外に表示するクラス名
    ''' </summary>
    Private Const CSS_STYLENAME_GRAPHVALUE_POSITION_OUT As String = "addText_GraphOut"

    ''' <summary>
    ''' 進捗率をグラフ内に表示するクラス名
    ''' </summary>
    Private Const CSS_STYLENAME_GRAPHVALUE_POSITION_IN As String = "addText_GraphIn"

    ''' <summary>
    ''' 数値フォーマットの変換
    ''' </summary>
    Private Const VALUE_FORMAT As String = "{0:#,##0}"

    ''' <summary>
    ''' パーセント文字列
    ''' </summary>
    Private Const PERCENT_FORMAT As String = "{0}%"

    ''' <summary>
    ''' 数値の最大桁数(4桁)
    ''' </summary>
    Private Const MAX_NUMBER_FOUR As Integer = 4

    ''' <summary>
    ''' 数値の最大桁数(3桁)
    ''' </summary>
    Private Const MAX_NUMBER_THREE As Integer = 3

    ''' <summary>
    ''' 数値の最大数(9999)
    ''' </summary>
    Private Const MAX_NUMBER_9999 As Long = 9999

    ''' <summary>
    ''' 数値の最大数(999)
    ''' </summary>
    Private Const MAX_NUMBER_999 As Long = 999

    ''' <summary>
    ''' 画面表示最小値
    ''' </summary>
    Private Const MIN_VALUE As Integer = 0

    ''' <summary>
    ''' 比率最大値(200)
    ''' </summary>
    Private Const MAX_PERCENT_VALUE As Integer = 200

#End Region

#Region "メンバ変数"

#Region "当月の目標と進捗率　グラフ幅"
    ''' <summary>
    ''' 入庫台数（台)－合計のグラフ幅(px)
    ''' </summary>
    Private GraphWidthNowWarehousingNumberTotalValue As String = "0"

    ''' <summary>
    ''' 入庫台数（台)－定期点検のグラフ幅(px)
    ''' </summary>
    Private GraphWidthNowCheckValue As String = "0"

    ''' <summary>
    ''' 入庫台数（台)－一般整備のグラフ幅(px)
    ''' </summary>
    Private GraphWidthNowMaintenanceValue As String = "0"

    ''' <summary>
    ''' 入庫売上（千円)－合計のグラフ幅(px)
    ''' </summary>
    Private GraphWidthNowSaleTotalValue As String = "0"

    ''' <summary>
    ''' 入庫売上（千円)－定期点検のグラフ幅(px)
    ''' </summary>
    Private GraphWidthNowSaleCheckValue As String = "0"

    ''' <summary>
    ''' 入庫売上（千円)－一般整備のグラフ幅(px)
    ''' </summary>
    Private GraphWidthNowSaleMaintenanceValue As String = "0"

#End Region

#Region "前月の目標と進捗率　グラフ幅"
    ''' <summary>
    ''' 入庫台数（台)－合計のグラフ幅(px)
    ''' </summary>
    Private GraphWidthPreviewsWarehousingNumberTotalValue As String = "0"

    ''' <summary>
    ''' 入庫台数（台)－定期点検のグラフ幅(px)
    ''' </summary>
    Private GraphWidthPreviewsCheckValue As String = "0"

    ''' <summary>
    ''' 入庫台数（台)－一般整備のグラフ幅(px)
    ''' </summary>
    Private GraphWidthPreviewsMaintenanceValue As String = "0"

    ''' <summary>
    ''' 入庫売上（千円)－合計のグラフ幅(px)
    ''' </summary>
    Private GraphWidthPreviewsSaleTotalValue As String = "0"

    ''' <summary>
    ''' 入庫売上（千円)－定期点検のグラフ幅(px)
    ''' </summary>
    Private GraphWidthPreviewsSaleCheckValue As String = "0"

    ''' <summary>
    ''' 入庫売上（千円)－一般整備のグラフ幅(px)
    ''' </summary>
    Private GraphWidthPreviewsSaleMaintenanceValue As String = "0"

#End Region

#Region "当日の目標と進捗率　グラフ幅"
    ''' <summary>
    ''' 入庫台数（台)のグラフ幅(px)
    ''' </summary>
    Private GraphWidthTodayWarehousingNumberValue As String = "0"

    ''' <summary>
    ''' 入庫売上（千円)のグラフ幅(px)
    ''' </summary>
    Private GraphWidthTodayWarehousingSaleValue As String = "0"

#End Region

#End Region

#Region "プロパティ"

#Region "当月の目標と進捗率　グラフ幅"

    ''' <summary>
    ''' 入庫台数（台)－合計のグラフ幅(px)
    ''' </summary>
    ''' <value>入庫台数（台)－合計のグラフ幅(px)</value>
    ''' <remarks>読み取り専用</remarks>
    Public ReadOnly Property GraphWidthNowWarehousingNumberTotal() As String
        Get
            Return GraphWidthNowWarehousingNumberTotalValue
        End Get
    End Property

    ''' <summary>
    ''' 入庫台数（台)－定期点検のグラフ幅(px)
    ''' </summary>
    ''' <value>入庫台数（台)－定期点検のグラフ幅(px)</value>
    ''' <remarks>読み取り専用</remarks>
    Public ReadOnly Property GraphWidthNowCheck() As String
        Get
            Return GraphWidthNowCheckValue
        End Get
    End Property

    ''' <summary>
    ''' 入庫台数（台)－一般整備のグラフ幅(px)
    ''' </summary>
    ''' <value>入庫台数（台)－一般整備のグラフ幅(px)</value>
    ''' <remarks>読み取り専用</remarks>
    Public ReadOnly Property GraphWidthNowMaintenance() As String
        Get
            Return GraphWidthNowMaintenanceValue
        End Get
    End Property

    ''' <summary>
    ''' 入庫売上（千円)－合計のグラフ幅(px)
    ''' </summary>
    ''' <value>入庫売上（千円)－合計のグラフ幅(px)</value>
    ''' <remarks>読み取り専用</remarks>
    Public ReadOnly Property GraphWidthNowSaleTotal() As String
        Get
            Return GraphWidthNowSaleTotalValue
        End Get
    End Property

    ''' <summary>
    ''' 入庫売上（千円)－定期点検のグラフ幅(px)
    ''' </summary>
    ''' <value>入庫売上（千円)－定期点検のグラフ幅(px)</value>
    ''' <remarks>読み取り専用</remarks>
    Public ReadOnly Property GraphWidthNowSaleCheck() As String
        Get
            Return GraphWidthNowSaleCheckValue
        End Get
    End Property

    ''' <summary>
    ''' 入庫売上（千円)－一般整備のグラフ幅(px)
    ''' </summary>
    ''' <value>入庫売上（千円)－一般整備のグラフ幅(px)</value>
    ''' <remarks>読み取り専用</remarks>
    Public ReadOnly Property GraphWidthNowSaleMaintenance() As String
        Get
            Return GraphWidthNowSaleMaintenanceValue
        End Get
    End Property

#End Region

#Region "前月の目標と進捗率　グラフ幅"

    ''' <summary>
    ''' 入庫台数（台)－合計のグラフ幅(px)
    ''' </summary>
    ''' <value>入庫台数（台)－合計のグラフ幅(px)</value>
    ''' <remarks>読み取り専用</remarks>
    Public ReadOnly Property GraphWidthPreviewsWarehousingNumberTotal() As String
        Get
            Return GraphWidthPreviewsWarehousingNumberTotalValue
        End Get
    End Property

    ''' <summary>
    ''' 入庫台数（台)－定期点検のグラフ幅(px)
    ''' </summary>
    ''' <value>入庫台数（台)－定期点検のグラフ幅(px)</value>
    ''' <remarks>読み取り専用</remarks>
    Public ReadOnly Property GraphWidthPreviewsCheck() As String
        Get
            Return GraphWidthPreviewsCheckValue
        End Get
    End Property

    ''' <summary>
    ''' 入庫台数（台)－一般整備のグラフ幅(px)
    ''' </summary>
    ''' <value>入庫台数（台)－一般整備のグラフ幅(px)</value>
    ''' <remarks>読み取り専用</remarks>
    Public ReadOnly Property GraphWidthPreviewsMaintenance() As String
        Get
            Return GraphWidthPreviewsMaintenanceValue
        End Get
    End Property

    ''' <summary>
    ''' 入庫売上（千円)－合計のグラフ幅(px)
    ''' </summary>
    ''' <value>入庫売上（千円)－合計のグラフ幅(px)</value>
    ''' <remarks>読み取り専用</remarks>
    Public ReadOnly Property GraphWidthPreviewsSaleTotal() As String
        Get
            Return GraphWidthPreviewsSaleTotalValue
        End Get
    End Property

    ''' <summary>
    ''' 入庫売上（千円)－定期点検のグラフ幅(px)
    ''' </summary>
    ''' <value>入庫売上（千円)－定期点検のグラフ幅(px)</value>
    ''' <remarks>読み取り専用</remarks>
    Public ReadOnly Property GraphWidthPreviewsSaleCheck() As String
        Get
            Return GraphWidthPreviewsSaleCheckValue
        End Get
    End Property

    ''' <summary>
    ''' 入庫売上（千円)－一般整備のグラフ幅(px)
    ''' </summary>
    ''' <value>入庫売上（千円)－一般整備のグラフ幅(px)</value>
    ''' <remarks>読み取り専用</remarks>
    Public ReadOnly Property GraphWidthPreviewsSaleMaintenance() As String
        Get
            Return GraphWidthPreviewsSaleMaintenanceValue
        End Get
    End Property

#End Region

#Region "当日の目標と進捗率　グラフ幅"

    ''' <summary>
    ''' 入庫台数（台)のグラフ幅(px)
    ''' </summary>
    ''' <value>入庫台数（台)のグラフ幅(px)</value>
    ''' <remarks>読み取り専用</remarks>
    Public ReadOnly Property GraphWidthTodayWarehousingNumber() As String
        Get
            Return GraphWidthTodayWarehousingNumberValue
        End Get
    End Property

    ''' <summary>
    ''' 入庫売上（千円)のグラフ幅(px)
    ''' </summary>
    ''' <value>入庫売上（千円)のグラフ幅(px)</value>
    ''' <remarks>読み取り専用</remarks>
    Public ReadOnly Property GraphWidthTodayWarehousingSale() As String
        Get
            Return GraphWidthTodayWarehousingSaleValue
        End Get
    End Property

#End Region

#End Region

#Region " イベント処理 "

    ''' ------------------------------------------------------------------------------------------
    ''' <summary>
    ''' ページロード時の処理です。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks>
    ''' Postで呼ばれることを想定していないのでisPostBack判定は行いません
    ''' </remarks>
    ''' ------------------------------------------------------------------------------------------
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        '開始ログ出力
        Dim logStart As New StringBuilder
        With logStart
            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
            .Append(" Start")
        End With
        Logger.Info(logStart.ToString)

        Try
            'スタッフ情報を取得
            Dim staffInfo As StaffContext = StaffContext.Current

            'ダッシュボード設定値を取得
            Dim dt As IC3801005DataSet.IC3801005SAKPIDataTable
            Dim bl As SC3140102BusinessLogic = New SC3140102BusinessLogic
            dt = bl.GetIfStaffInformation(staffInfo)

            'ダッシュボード情報を判定
            Dim row As IC3801005DataSet.IC3801005SAKPIRow

            If Not IsNothing(dt) AndAlso dt.Count > 0 Then
                row = DirectCast(dt.Rows(0), IC3801005DataSet.IC3801005SAKPIRow)
                'ダッシュボード情報チェック
                row = Me.IsDashBoardInformationCheck(row)
                '画面項目に反映
                Me.SetDashBoardDisplayValue(row)
                '進捗率・実績率を反映
                Me.SetResultValue(row)
            End If

            'グラフの非表示設定
            Me.SetHidenGraph()

        Catch ex As OracleExceptionEx When ex.Number = 1013
        Finally
            '処理中アイコン終了
            StopIcon()
        End Try

        '終了ログ出力
        Dim logEnd As New StringBuilder
        With logEnd
            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
            .Append(" End")
        End With
        Logger.Info(logEnd.ToString)
    End Sub

    ''' ------------------------------------------------------------------------------------------
    ''' <summary>
    ''' 目標と実績から進捗率・実績率を計算する
    ''' </summary>
    ''' <param name="target">目標値</param>
    ''' <param name="result">実績値</param>
    ''' <returns>進捗率or実績率</returns>
    ''' <remarks>
    ''' 小数点第一位を四捨五入
    ''' </remarks>
    ''' ------------------------------------------------------------------------------------------
    Protected Function ConvertResultToPercent(ByVal target As Integer, ByVal result As Integer) As Integer

        '引数チェック
        'マイナスの場合0とする
        If target < 0 Then
            target = MIN_VALUE
        End If
        If result < 0 Then
            result = MIN_VALUE
        End If
        '目標値と実績値が0の場合は計算を行わない
        If target.Equals(0) And result.Equals(0) Then
            Return MIN_VALUE
        End If
        '実績値が0の場合は計算を行わない
        If result.Equals(0) Then
            Return MIN_VALUE
        End If
        '予定値が0で実績値が1以上の場合は最大値固定
        If target.Equals(0) And result >= 1 Then
            Return MAX_PERCENT_VALUE
        End If

        Dim persent As New Decimal(0)
        '実績を目標で除算し進捗・実績率を計算
        persent = Decimal.Divide(result, target) * 100
        '進捗・実績率を丸める
        Return Decimal.ToInt32(Decimal.Round(persent, 0))
    End Function

    ''' ------------------------------------------------------------------------------------------
    ''' <summary>
    ''' 実績進捗率からグラフの長さ(px)を出力する
    ''' </summary>
    ''' <param name="percent">進捗率</param>
    ''' <returns>グラフの長さ(px)</returns>
    ''' <remarks></remarks>
    ''' ------------------------------------------------------------------------------------------
    Protected Function CovertPercentToWidth(ByVal percent As Integer) As Integer

        Dim width As Integer = 0

        If percent > 0 Then
            width = CType(percent * DEFAULT_GRAPH_WIDTH_WEIGHT, Integer)

            'グラフの長さが175pxを超えさせない
            If width >= MAX_GRAPH_WIDTH Then
                width = MAX_GRAPH_WIDTH
            End If
        End If

        Return width

    End Function

    ''' ------------------------------------------------------------------------------------------
    ''' <summary>
    ''' 進捗率に適用するCSSクラス名を返却する。
    ''' </summary>
    ''' <param name="value">進捗率(px)</param>
    ''' <return>CSSクラス名</return>
    ''' <remarks>
    ''' valueが適正な値でない場合クラス名を返却しない
    ''' </remarks>
    ''' ------------------------------------------------------------------------------------------
    Protected Function CssClassNameForGraphBar(ByVal value As String) As String
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

#End Region

#Region " 非公開メソッド "

    ''' ------------------------------------------------------------------------------------------
    ''' <summary>
    ''' 画面項目：ダッシュボードに値を設定
    ''' </summary>
    ''' <param name="rowSC3140102DashBoard">ダッシュボードDataRow</param>
    ''' <remarks></remarks>
    ''' ------------------------------------------------------------------------------------------
    Private Sub SetDashBoardDisplayValue(ByVal rowSC3140102DashBoard As IC3801005DataSet.IC3801005SAKPIRow)

        'スタッフ情報を取得
        Dim staffInfo As StaffContext = StaffContext.Current
        'サーバ時間取得
        Dim dtmNow As Date = DateTimeFunc.Now(staffInfo.DlrCD)

        With rowSC3140102DashBoard

            '当月の目標と進捗率
            Me.Label_UpdateTime_ThisMonth.Text = Me.SetDataMakingDate(CType(.CURMONTHCREATETIME, Date), dtmNow)                             'データ作成日時
            Me.Label_NowWarehousingNumber_Target.Text = Me.IsNumberConvert(CType(.CURMONTHPLANNUM, Long), MAX_NUMBER_FOUR)                  '入庫台数(台)
            Me.Label_NowWarehousingNumberTotal_Target.Text = Me.IsNumberConvert(CType(.CURMONTHPLANADDNUM, Long), MAX_NUMBER_FOUR)          '入庫台数(台)-合計　目標
            Me.Label_NowWarehousingNumberTotal_Result.Text = Me.IsNumberConvert(CType(.CURMONTHACHNUM, Long), MAX_NUMBER_FOUR)              '入庫台数(台)-合計　実績
            Me.Label_NowCheck_Target.Text = Me.IsNumberConvert(CType(.CUREMONTHPLANMAINTAINADDNUM, Long), MAX_NUMBER_FOUR)                  '入庫台数(台)-定期点検　目標
            Me.Label_NowCheck_Result.Text = Me.IsNumberConvert(CType(.CURMONTHACHMAINTAINNUM, Long), MAX_NUMBER_FOUR)                       '入庫台数(台)-定期点検　実績
            Me.Label_NowMaintenance_Target.Text = Me.IsNumberConvert(CType(.CURMONTHPLANREPAIRADDNUM, Long), MAX_NUMBER_FOUR)               '入庫台数(台)-一般整備　目標
            Me.Label_NowMaintenance_Result.Text = Me.IsNumberConvert(CType(.CURMONTHACHREPAIRNUM, Long), MAX_NUMBER_FOUR)                   '入庫台数(台)-一般整備　実績
            Me.Label_NowWarehousingSale_Target.Text = Me.IsNumberConvert(CType(.CURMONTHPLANAMOUNT, Long), MAX_NUMBER_FOUR)                 '入庫売上(千円)
            Me.Label_NowSaleTotal_Target.Text = Me.IsNumberConvert(CType(.CURMONTHPLANADDAMOUNT, Long), MAX_NUMBER_FOUR)                    '入庫売上(千円)-合計　目標
            Me.Label_NowSaleTotal_Result.Text = Me.IsNumberConvert(CType(.CURMONTHACHADDAMOUNT, Long), MAX_NUMBER_FOUR)                     '入庫売上(千円)-合計　実績
            Me.Label_NowSaleCheck_Target.Text = Me.IsNumberConvert(CType(.CURMONTHPLANMAINTAINADDAMOUNT, Long), MAX_NUMBER_FOUR)            '入庫売上(千円)-定期点検　目標
            Me.Label_NowSaleCheck_Result.Text = Me.IsNumberConvert(CType(.CURMONTHACHMAINTAINADDAMOUNT, Long), MAX_NUMBER_FOUR)             '入庫売上(千円)-定期点検　実績
            Me.Label_NowSaleMaintenance_Target.Text = Me.IsNumberConvert(CType(.CURMONTHPLANREPAIRADDAMOUNT, Long), MAX_NUMBER_FOUR)        '入庫売上(千円)-一般整備　目標
            Me.Label_NowSaleMaintenance_Result.Text = Me.IsNumberConvert(CType(.CURMONTHACHREPAIRADDAMOUNT, Long), MAX_NUMBER_FOUR)         '入庫売上(千円)-一般整備　実績

            '前月の目標と実績率
            Me.Label_UpdateTime_LastMonth.Text = Me.SetDataMakingDate(CType(.PREMONTHCREATETIME, Date), dtmNow)                             'データ作成日時
            Me.Label_PreviewsWarehousingNumberTotal_Target.Text = Me.IsNumberConvert(CType(.PREMONTHPLANNUM, Long), MAX_NUMBER_FOUR)        '入庫台数(台)-合計　目標
            Me.Label_PreviewsWarehousingNumberTotal_Result.Text = Me.IsNumberConvert(CType(.PREMONTHACHNUM, Long), MAX_NUMBER_FOUR)         '入庫台数(台)-合計　実績
            Me.Label_PreviewsCheck_Target.Text = Me.IsNumberConvert(CType(.PREMONTHPLANMAINTAINNUM, Long), MAX_NUMBER_FOUR)                 '入庫台数(台)-定期点検　目標
            Me.Label_PreviewsCheck_Result.Text = Me.IsNumberConvert(CType(.PREMONTHACHMAINTAINNUM, Long), MAX_NUMBER_FOUR)                  '入庫台数(台)-定期点検　実績
            Me.Label_PreviewsMaintenance_Target.Text = Me.IsNumberConvert(CType(.PREMONTHPLANREPAIRNUM, Long), MAX_NUMBER_FOUR)             '入庫台数(台)-一般整備　目標
            Me.Label_PreviewsMaintenance_Result.Text = Me.IsNumberConvert(CType(.PREMONTHACHREPAIRNUM, Long), MAX_NUMBER_FOUR)              '入庫台数(台)-一般整備　実績
            Me.Label_PreviewsSaleTotal_Target.Text = Me.IsNumberConvert(CType(.PREMONTHPLANTOTALAMOUNT, Long), MAX_NUMBER_FOUR)             '入庫売上(千円)-合計　目標
            Me.Label_PreviewsSaleTotal_Result.Text = Me.IsNumberConvert(CType(.PREMONTHACHTOTALAMOUNT, Long), MAX_NUMBER_FOUR)              '入庫売上(千円)-合計　実績
            Me.Label_PreviewsSaleCheck_Target.Text = Me.IsNumberConvert(CType(.PREMONTHPLANMAINTAINAMOUNT, Long), MAX_NUMBER_FOUR)          '入庫売上(千円)-定期点検　目標
            Me.Label_PreviewsSaleCheck_Result.Text = Me.IsNumberConvert(CType(.PREMONTHACHMAINTAINAMOUNT, Long), MAX_NUMBER_FOUR)           '入庫売上(千円)-定期点検　実績
            Me.Label_PreviewsSaleMaintenance_Target.Text = Me.IsNumberConvert(CType(.PREMONTHPLANREPAIRAMOUNT, Long), MAX_NUMBER_FOUR)      '入庫売上(千円)-一般整備　目標
            Me.Label_PreviewsSaleMaintenance_Result.Text = Me.IsNumberConvert(CType(.PREMONTHACHREPAIRAMOUNT, Long), MAX_NUMBER_FOUR)       '入庫売上(千円)-一般整備　実績
            '当日の目標と進捗率
            Me.Label_UpdateTime_Today.Text = Me.SetDataMakingDate(CType(.CURDAYCREATETIME, Date), dtmNow)                                   'データ作成日時
            Me.Label_TodayWarehousingNumber_Target.Text = Me.IsNumberConvert(CType(.CURDAYPLANNUM, Long), MAX_NUMBER_THREE)                 '入庫台数(台)-目標
            Me.Label_TodayWarehousingNumber_Result.Text = Me.IsNumberConvert(CType(.CURDAYARCHNUM, Long), MAX_NUMBER_THREE)                 '入庫台数(台)-実績
            Me.Label_TodayWarehousingSale_Target.Text = Me.IsNumberConvert(CType(.CURDAYPLANAMOUNT, Long), MAX_NUMBER_THREE)                '入庫売上(千円)-目標
            Me.Label_TodayWarehousingSale_Result.Text = Me.IsNumberConvert(CType(.CURDAYARCHAMOUNT, Long), MAX_NUMBER_THREE)                '入庫売上(千円)-実績

        End With
    End Sub

    ''' ------------------------------------------------------------------------------------------
    ''' <summary>
    ''' 進捗率を設定
    ''' </summary>
    ''' <param name="rowSC3140102DashBoard">ダッシュボードDataRow</param>
    ''' <remarks></remarks>
    ''' ------------------------------------------------------------------------------------------
    Private Sub SetResultValue(ByVal rowSC3140102DashBoard As IC3801005DataSet.IC3801005SAKPIRow)

        With rowSC3140102DashBoard
            '進捗率を計算

            '当月の進捗率
            Dim NowWarehousingNumberTotal_Persent As Integer = Me.ConvertResultToPercent(CType(.CURMONTHPLANADDNUM, Integer), CType(.CURMONTHACHNUM, Integer))
            Dim NowCheck_Persent As Integer = Me.ConvertResultToPercent(CType(.CUREMONTHPLANMAINTAINADDNUM, Integer), CType(.CURMONTHACHMAINTAINNUM, Integer))
            Dim NowMaintenance_Persent As Integer = Me.ConvertResultToPercent(CType(.CURMONTHPLANREPAIRADDNUM, Integer), CType(.CURMONTHACHREPAIRNUM, Integer))
            Dim NowSaleTotal_Persent As Integer = Me.ConvertResultToPercent(CType(.CURMONTHPLANADDAMOUNT, Integer), CType(.CURMONTHACHADDAMOUNT, Integer))
            Dim NowSaleCheck_Persent As Integer = Me.ConvertResultToPercent(CType(.CURMONTHPLANMAINTAINADDAMOUNT, Integer), CType(.CURMONTHACHMAINTAINADDAMOUNT, Integer))
            Dim NowSaleMaintenance As Integer = Me.ConvertResultToPercent(CType(.CURMONTHPLANREPAIRADDAMOUNT, Integer), CType(.CURMONTHACHREPAIRADDAMOUNT, Integer))
            '前月の実績率
            Dim PreviewsWarehousingNumberTotal_Persent As Integer = Me.ConvertResultToPercent(CType(.PREMONTHPLANNUM, Integer), CType(.PREMONTHACHNUM, Integer))
            Dim PreviewsCheck_Persent As Integer = Me.ConvertResultToPercent(CType(.PREMONTHPLANMAINTAINNUM, Integer), CType(.PREMONTHACHMAINTAINNUM, Integer))
            Dim PreviewsMaintenance_Persent As Integer = Me.ConvertResultToPercent(CType(.PREMONTHPLANREPAIRNUM, Integer), CType(.PREMONTHACHREPAIRNUM, Integer))
            Dim PreviewsSaleTotal_Persent As Integer = Me.ConvertResultToPercent(CType(.PREMONTHPLANTOTALAMOUNT, Integer), CType(.PREMONTHACHTOTALAMOUNT, Integer))
            Dim PreviewsSaleCheck_Persent As Integer = Me.ConvertResultToPercent(CType(.PREMONTHPLANMAINTAINAMOUNT, Integer), CType(.PREMONTHACHMAINTAINAMOUNT, Integer))
            Dim PreviewsSaleMaintenance As Integer = Me.ConvertResultToPercent(CType(.PREMONTHPLANREPAIRAMOUNT, Integer), CType(.PREMONTHACHREPAIRAMOUNT, Integer))
            '当日の目標と進捗率
            Dim TodayWarehousingNumber_Persent As Integer = Me.ConvertResultToPercent(CType(.CURDAYPLANNUM, Integer), CType(.CURDAYARCHNUM, Integer))
            Dim TodayWarehousingSale As Integer = Me.ConvertResultToPercent(CType(.CURDAYPLANAMOUNT, Integer), CType(.CURDAYARCHAMOUNT, Integer))

            '画面項目に反映（進捗率)
            Me.Label_NowWarehousingNumberTotal_Percent.Text = String.Format(CultureInfo.CurrentCulture, PERCENT_FORMAT, NowWarehousingNumberTotal_Persent.ToString(CultureInfo.CurrentCulture))
            Me.Label_NowCheck_Percent.Text = String.Format(CultureInfo.CurrentCulture, PERCENT_FORMAT, NowCheck_Persent.ToString(CultureInfo.CurrentCulture))
            Me.Label_NowMaintenance_Percent.Text = String.Format(CultureInfo.CurrentCulture, PERCENT_FORMAT, NowMaintenance_Persent.ToString(CultureInfo.CurrentCulture))
            Me.Label_NowSaleTotal_Percent.Text = String.Format(CultureInfo.CurrentCulture, PERCENT_FORMAT, NowSaleTotal_Persent.ToString(CultureInfo.CurrentCulture))
            Me.Label_NowSaleCheck_Percent.Text = String.Format(CultureInfo.CurrentCulture, PERCENT_FORMAT, NowSaleCheck_Persent.ToString(CultureInfo.CurrentCulture))
            Me.Label_NowSaleMaintenance_Percent.Text = String.Format(CultureInfo.CurrentCulture, PERCENT_FORMAT, NowSaleMaintenance.ToString(CultureInfo.CurrentCulture))

            Me.Label_PreviewsWarehousingNumberTotal_Percent.Text = String.Format(CultureInfo.CurrentCulture, PERCENT_FORMAT, PreviewsWarehousingNumberTotal_Persent.ToString(CultureInfo.CurrentCulture))
            Me.Label_PreviewsCheck_Percent.Text = String.Format(CultureInfo.CurrentCulture, PERCENT_FORMAT, PreviewsCheck_Persent.ToString(CultureInfo.CurrentCulture))
            Me.Label_PreviewsMaintenance_Percent.Text = String.Format(CultureInfo.CurrentCulture, PERCENT_FORMAT, PreviewsMaintenance_Persent.ToString(CultureInfo.CurrentCulture))
            Me.Label_PreviewsSaleTotal_Percent.Text = String.Format(CultureInfo.CurrentCulture, PERCENT_FORMAT, PreviewsSaleTotal_Persent.ToString(CultureInfo.CurrentCulture))
            Me.Label_PreviewsSaleCheck_Percent.Text = String.Format(CultureInfo.CurrentCulture, PERCENT_FORMAT, PreviewsSaleCheck_Persent.ToString(CultureInfo.CurrentCulture))
            Me.Label_PreviewsSaleMaintenance_Percent.Text = String.Format(CultureInfo.CurrentCulture, PERCENT_FORMAT, PreviewsSaleMaintenance.ToString(CultureInfo.CurrentCulture))

            Me.Label_TodayWarehousingNumber_Percent.Text = String.Format(CultureInfo.CurrentCulture, PERCENT_FORMAT, TodayWarehousingNumber_Persent.ToString(CultureInfo.CurrentCulture))
            Me.Label_TodayWarehousingSale_Percent.Text = String.Format(CultureInfo.CurrentCulture, PERCENT_FORMAT, TodayWarehousingSale.ToString(CultureInfo.CurrentCulture))

            '画面項目に反映（進捗率)
            Me.GraphWidthNowWarehousingNumberTotalValue = Me.CovertPercentToWidth(NowWarehousingNumberTotal_Persent).ToString(CultureInfo.CurrentCulture)
            Me.GraphWidthNowCheckValue = Me.CovertPercentToWidth(NowCheck_Persent).ToString(CultureInfo.CurrentCulture)
            Me.GraphWidthNowMaintenanceValue = Me.CovertPercentToWidth(NowMaintenance_Persent).ToString(CultureInfo.CurrentCulture)
            Me.GraphWidthNowSaleTotalValue = Me.CovertPercentToWidth(NowSaleTotal_Persent).ToString(CultureInfo.CurrentCulture)
            Me.GraphWidthNowSaleCheckValue = Me.CovertPercentToWidth(NowSaleCheck_Persent).ToString(CultureInfo.CurrentCulture)
            Me.GraphWidthNowSaleMaintenanceValue = Me.CovertPercentToWidth(NowSaleMaintenance).ToString(CultureInfo.CurrentCulture)
            Me.GraphWidthPreviewsWarehousingNumberTotalValue = Me.CovertPercentToWidth(PreviewsWarehousingNumberTotal_Persent).ToString(CultureInfo.CurrentCulture)
            Me.GraphWidthPreviewsCheckValue = Me.CovertPercentToWidth(PreviewsCheck_Persent).ToString(CultureInfo.CurrentCulture)
            Me.GraphWidthPreviewsMaintenanceValue = Me.CovertPercentToWidth(PreviewsMaintenance_Persent).ToString(CultureInfo.CurrentCulture)
            Me.GraphWidthPreviewsSaleTotalValue = Me.CovertPercentToWidth(PreviewsSaleTotal_Persent).ToString(CultureInfo.CurrentCulture)
            Me.GraphWidthPreviewsSaleCheckValue = Me.CovertPercentToWidth(PreviewsSaleCheck_Persent).ToString(CultureInfo.CurrentCulture)
            Me.GraphWidthPreviewsSaleMaintenanceValue = Me.CovertPercentToWidth(PreviewsSaleMaintenance).ToString(CultureInfo.CurrentCulture)
            Me.GraphWidthTodayWarehousingNumberValue = Me.CovertPercentToWidth(TodayWarehousingNumber_Persent).ToString(CultureInfo.CurrentCulture)
            Me.GraphWidthTodayWarehousingSaleValue = Me.CovertPercentToWidth(TodayWarehousingSale).ToString(CultureInfo.CurrentCulture)

        End With
    End Sub

    ''' ------------------------------------------------------------------------------------------
    ''' <summary>
    ''' 進捗率の非表示の設定
    ''' </summary>
    ''' <remarks></remarks>
    ''' ------------------------------------------------------------------------------------------
    Private Sub SetHidenGraph()
        '進捗率が0%のグラフを非表示にする
        If Me.GraphWidthNowWarehousingNumberTotalValue.Equals("0") Then : Me.Div_NowWarehousingNumberTotal_Graph.Visible = False : End If
        If Me.GraphWidthNowCheckValue.Equals("0") Then : Me.Div_NowCheck_Graph.Visible = False : End If
        If Me.GraphWidthNowMaintenanceValue.Equals("0") Then : Me.Div_NowMaintenance_Graph.Visible = False : End If
        If Me.GraphWidthNowSaleTotalValue.Equals("0") Then : Me.Div_NowSaleTotal_Graph.Visible = False : End If
        If Me.GraphWidthNowSaleCheckValue.Equals("0") Then : Me.Div_NowSaleCheck_Graph.Visible = False : End If
        If Me.GraphWidthNowSaleMaintenanceValue.Equals("0") Then : Me.Div_NowSaleMaintenance_Graph.Visible = False : End If
        If Me.GraphWidthPreviewsWarehousingNumberTotalValue.Equals("0") Then : Me.Div_PreviewsWarehousingNumberTotal_Graph.Visible = False : End If
        If Me.GraphWidthPreviewsCheckValue.Equals("0") Then : Me.Div_PreviewsCheck_Graph.Visible = False : End If
        If Me.GraphWidthPreviewsMaintenanceValue.Equals("0") Then : Me.Div_PreviewsMaintenance_Graph.Visible = False : End If
        If Me.GraphWidthPreviewsSaleTotalValue.Equals("0") Then : Me.Div_PreviewsSaleTotal_Graph.Visible = False : End If
        If Me.GraphWidthPreviewsSaleCheckValue.Equals("0") Then : Me.Div_PreviewsSaleCheck_Graph.Visible = False : End If
        If Me.GraphWidthPreviewsSaleMaintenanceValue.Equals("0") Then : Me.Div_PreviewsSaleMaintenance_Graph.Visible = False : End If
        If Me.GraphWidthTodayWarehousingNumberValue.Equals("0") Then : Me.Div_TodayWarehousingNumber_Graph.Visible = False : End If
        If Me.GraphWidthTodayWarehousingSaleValue.Equals("0") Then : Me.Div_TodayWarehousingSale_Graph.Visible = False : End If
    End Sub

    ''' ------------------------------------------------------------------------------------------
    ''' <summary>
    ''' データ作成日時の変換 (hh:mm) 又は (mm/dd)　
    ''' </summary>
    ''' <param name="time">対象時間</param>
    ''' <param name="dtmNow">サーバ時間</param>
    ''' <remarks></remarks>
    ''' ------------------------------------------------------------------------------------------
    Private Function SetDataMakingDate(ByVal time As DateTime, ByVal dtmNow As DateTime) As String

        'データ作成日時
        Dim strMakingDate As String

        '当日作成の場合はHH:MMを表示
        '当日以外作成の場合はMM/DDを表示
        If time.ToString("yyyyMMdd", CultureInfo.CurrentCulture).Equals(dtmNow.ToString("yyyyMMdd", CultureInfo.CurrentCulture)) Then
            ' 当日 (hh:mm)
            strMakingDate = DateTimeFunc.FormatDate(14, time)
        Else
            ' 上記以外 (mm/dd)
            strMakingDate = DateTimeFunc.FormatDate(11, time)
        End If

        Return strMakingDate

    End Function

    ''' ------------------------------------------------------------------------------------------
    ''' <summary>
    ''' 画面表示数値の桁数チェック・変換
    ''' </summary>
    ''' <param name="target">桁数チェック対象</param>
    ''' <param name="maxNumber">数値の最大桁数</param>
    ''' <remarks></remarks>
    ''' ------------------------------------------------------------------------------------------
    Private Function IsNumberConvert(ByVal target As Long, ByVal maxNumber As Integer) As String

        '画面表示数値の変換後文字列
        Dim strFormat As String

        'マイナスの場合0とする
        If target < 0 Then
            strFormat = MIN_VALUE.ToString(CultureInfo.CurrentCulture)
            Return strFormat
        End If

        '桁数チェック
        If Validation.IsCorrectDigit(target.ToString(CultureInfo.CurrentCulture), maxNumber) Then
            'そのまま画面に表示
            strFormat = String.Format(CultureInfo.CurrentCulture, VALUE_FORMAT, target)
        Else
            '画面表示桁数をオーバーした場合、項目に応じて「9999」か「999」にする(項目幅を超えないように)
            If maxNumber.ToString(CultureInfo.CurrentCulture).Equals(MAX_NUMBER_FOUR.ToString(CultureInfo.CurrentCulture)) Then         'ダッシュボードエリアが当月、前月の場合
                strFormat = String.Format(CultureInfo.CurrentCulture, VALUE_FORMAT, MAX_NUMBER_9999)
            ElseIf maxNumber.ToString(CultureInfo.CurrentCulture).Equals(MAX_NUMBER_THREE.ToString(CultureInfo.CurrentCulture)) Then    'ダッシュボードエリアが当日の場合
                strFormat = String.Format(CultureInfo.CurrentCulture, VALUE_FORMAT, MAX_NUMBER_999)
            Else
                strFormat = String.Empty
            End If
        End If

        Return strFormat

    End Function

    ''' ------------------------------------------------------------------------------------------
    ''' <summary>
    ''' ダッシュボード情報チェック
    ''' </summary>
    ''' <param name="rowSC3140102DashBoard">ダッシュボードDataRow</param>
    ''' <remarks></remarks>
    ''' ------------------------------------------------------------------------------------------
    Private Function IsDashBoardInformationCheck(ByVal rowSC3140102DashBoard As IC3801005DataSet.IC3801005SAKPIRow) As IC3801005DataSet.IC3801005SAKPIRow

        ' IF情報でデータなしの可能性があるため値チェックし初期値を設定しておく
        With rowSC3140102DashBoard

            ' 当月の目標と進捗率
            If String.IsNullOrEmpty(.CURMONTHCREATETIME) Then
                .CURMONTHCREATETIME = Date.MinValue.ToString(CultureInfo.CurrentCulture) 'データ作成日時
                ' データ作成日時がない場合は該当項目を非表示
                Label_UpdateTime_ThisMonth.Visible = False
                Label_UpdateText_ThisMonth.Visible = False
            End If
            If String.IsNullOrEmpty(.CURMONTHPLANNUM) Then : .CURMONTHPLANNUM = MIN_VALUE.ToString(CultureInfo.CurrentCulture) : End If                                 '入庫台数(台)
            If String.IsNullOrEmpty(.CURMONTHPLANADDNUM) Then : .CURMONTHPLANADDNUM = MIN_VALUE.ToString(CultureInfo.CurrentCulture) : End If                           '入庫台数(台)-合計　目標
            If String.IsNullOrEmpty(.CURMONTHACHNUM) Then : .CURMONTHACHNUM = MIN_VALUE.ToString(CultureInfo.CurrentCulture) : End If                                   '入庫台数(台)-合計　実績
            If String.IsNullOrEmpty(.CUREMONTHPLANMAINTAINADDNUM) Then : .CUREMONTHPLANMAINTAINADDNUM = MIN_VALUE.ToString(CultureInfo.CurrentCulture) : End If         '入庫台数(台)-定期点検　目標
            If String.IsNullOrEmpty(.CURMONTHACHMAINTAINNUM) Then : .CURMONTHACHMAINTAINNUM = MIN_VALUE.ToString(CultureInfo.CurrentCulture) : End If                   '入庫台数(台)-定期点検　実績
            If String.IsNullOrEmpty(.CURMONTHPLANREPAIRADDNUM) Then : .CURMONTHPLANREPAIRADDNUM = MIN_VALUE.ToString(CultureInfo.CurrentCulture) : End If               '入庫台数(台)-一般整備　目標
            If String.IsNullOrEmpty(.CURMONTHACHREPAIRNUM) Then : .CURMONTHACHREPAIRNUM = MIN_VALUE.ToString(CultureInfo.CurrentCulture) : End If                       '入庫台数(台)-一般整備　実績
            If String.IsNullOrEmpty(.CURMONTHPLANAMOUNT) Then : .CURMONTHPLANAMOUNT = MIN_VALUE.ToString(CultureInfo.CurrentCulture) : End If                           '入庫売上(千円)
            If String.IsNullOrEmpty(.CURMONTHPLANADDAMOUNT) Then : .CURMONTHPLANADDAMOUNT = MIN_VALUE.ToString(CultureInfo.CurrentCulture) : End If                     '入庫売上(千円)-合計　目標
            If String.IsNullOrEmpty(.CURMONTHACHADDAMOUNT) Then : .CURMONTHACHADDAMOUNT = MIN_VALUE.ToString(CultureInfo.CurrentCulture) : End If                       '入庫売上(千円)-合計　実績
            If String.IsNullOrEmpty(.CURMONTHPLANMAINTAINADDAMOUNT) Then : .CURMONTHPLANMAINTAINADDAMOUNT = MIN_VALUE.ToString(CultureInfo.CurrentCulture) : End If     '入庫売上(千円)-定期点検　目標
            If String.IsNullOrEmpty(.CURMONTHACHMAINTAINADDAMOUNT) Then : .CURMONTHACHMAINTAINADDAMOUNT = MIN_VALUE.ToString(CultureInfo.CurrentCulture) : End If       '入庫売上(千円)-定期点検　実績
            If String.IsNullOrEmpty(.CURMONTHPLANREPAIRADDAMOUNT) Then : .CURMONTHPLANREPAIRADDAMOUNT = MIN_VALUE.ToString(CultureInfo.CurrentCulture) : End If         '入庫売上(千円)-一般整備　目標
            If String.IsNullOrEmpty(.CURMONTHACHREPAIRADDAMOUNT) Then : .CURMONTHACHREPAIRADDAMOUNT = MIN_VALUE.ToString(CultureInfo.CurrentCulture) : End If           '入庫売上(千円)-一般整備　実績
            '前月の目標と実績率
            If String.IsNullOrEmpty(.PREMONTHCREATETIME) Then
                .PREMONTHCREATETIME = Date.MinValue.ToString(CultureInfo.CurrentCulture) 'データ作成日時
                ' データ作成日時がない場合は該当項目を非表示
                Label_UpdateTime_LastMonth.Visible = False
                Label_UpdateText_LastMonth.Visible = False
            End If
            If String.IsNullOrEmpty(.PREMONTHPLANNUM) Then : .PREMONTHPLANNUM = MIN_VALUE.ToString(CultureInfo.CurrentCulture) : End If                                 '入庫台数(台)-合計　目標
            If String.IsNullOrEmpty(.PREMONTHACHNUM) Then : .PREMONTHACHNUM = MIN_VALUE.ToString(CultureInfo.CurrentCulture) : End If                                   '入庫台数(台)-合計　実績
            If String.IsNullOrEmpty(.PREMONTHPLANMAINTAINNUM) Then : .PREMONTHPLANMAINTAINNUM = MIN_VALUE.ToString(CultureInfo.CurrentCulture) : End If                 '入庫台数(台)-定期点検　目標
            If String.IsNullOrEmpty(.PREMONTHACHMAINTAINNUM) Then : .PREMONTHACHMAINTAINNUM = MIN_VALUE.ToString(CultureInfo.CurrentCulture) : End If                   '入庫台数(台)-定期点検　実績
            If String.IsNullOrEmpty(.PREMONTHPLANREPAIRNUM) Then : .PREMONTHPLANREPAIRNUM = MIN_VALUE.ToString(CultureInfo.CurrentCulture) : End If                     '入庫台数(台)-一般整備　目標
            If String.IsNullOrEmpty(.PREMONTHACHREPAIRNUM) Then : .PREMONTHACHREPAIRNUM = MIN_VALUE.ToString(CultureInfo.CurrentCulture) : End If                       '入庫台数(台)-一般整備　実績
            If String.IsNullOrEmpty(.PREMONTHPLANTOTALAMOUNT) Then : .PREMONTHPLANTOTALAMOUNT = MIN_VALUE.ToString(CultureInfo.CurrentCulture) : End If                 '入庫売上(千円)-合計　目標
            If String.IsNullOrEmpty(.PREMONTHACHTOTALAMOUNT) Then : .PREMONTHACHTOTALAMOUNT = MIN_VALUE.ToString(CultureInfo.CurrentCulture) : End If                   '入庫売上(千円)-合計　実績
            If String.IsNullOrEmpty(.PREMONTHPLANMAINTAINAMOUNT) Then : .PREMONTHPLANMAINTAINAMOUNT = MIN_VALUE.ToString(CultureInfo.CurrentCulture) : End If           '入庫売上(千円)-定期点検　目標
            If String.IsNullOrEmpty(.PREMONTHACHMAINTAINAMOUNT) Then : .PREMONTHACHMAINTAINAMOUNT = MIN_VALUE.ToString(CultureInfo.CurrentCulture) : End If             '入庫売上(千円)-定期点検　実績
            If String.IsNullOrEmpty(.PREMONTHPLANREPAIRAMOUNT) Then : .PREMONTHPLANREPAIRAMOUNT = MIN_VALUE.ToString(CultureInfo.CurrentCulture) : End If               '入庫売上(千円)-一般整備　目標
            If String.IsNullOrEmpty(.PREMONTHACHREPAIRAMOUNT) Then : .PREMONTHACHREPAIRAMOUNT = MIN_VALUE.ToString(CultureInfo.CurrentCulture) : End If                 '入庫売上(千円)-一般整備　実績
            '当日の目標と進捗率
            If String.IsNullOrEmpty(.CURDAYCREATETIME) Then
                .CURDAYCREATETIME = Date.MinValue.ToString(CultureInfo.CurrentCulture) 'データ作成日時
                ' データ作成日時がない場合は該当項目を非表示
                Label_UpdateTime_Today.Visible = False
                Label_UpdateText_Today.Visible = False
            End If
            If String.IsNullOrEmpty(.CURDAYPLANNUM) Then : .CURDAYPLANNUM = MIN_VALUE.ToString(CultureInfo.CurrentCulture) : End If         '入庫台数(台)-目標
            If String.IsNullOrEmpty(.CURDAYARCHNUM) Then : .CURDAYARCHNUM = MIN_VALUE.ToString(CultureInfo.CurrentCulture) : End If         '入庫台数(台)-実績
            If String.IsNullOrEmpty(.CURDAYPLANAMOUNT) Then : .CURDAYPLANAMOUNT = MIN_VALUE.ToString(CultureInfo.CurrentCulture) : End If   '入庫売上(千円)-目標
            If String.IsNullOrEmpty(.CURDAYARCHAMOUNT) Then : .CURDAYARCHAMOUNT = MIN_VALUE.ToString(CultureInfo.CurrentCulture) : End If   '入庫売上(千円)-実績

        End With

        Return rowSC3140102DashBoard

    End Function

    ''' ------------------------------------------------------------------------------------------
    ''' <summary>
    ''' 読み込み中アイコン終了処理
    ''' </summary>
    ''' <remarks></remarks>
    ''' ------------------------------------------------------------------------------------------
    Private Sub StopIcon()

        Dim script As New StringBuilder()

        With script
            .Append("<script type='text/javascript'>")
            ' 2012/02/22 KN 森下【SERVICE_1】START
            .Append(" window.parent.StopIcon('#loadingDashboard'); ")
            ' 2012/02/22 KN 森下【SERVICE_1】END
            .Append("</script>")
        End With

        Dim cs As ClientScriptManager = Page.ClientScript
        cs.RegisterStartupScript(Me.GetType, "alert", script.ToString())

    End Sub

#End Region

End Class
