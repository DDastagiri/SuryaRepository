'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3250106BusinessLogic.vb
'─────────────────────────────────────
'機能： 部品説明画面 残量グラフ ビジネスロジック
'補足： 
'作成： 2014/07/XX NEC 上野
'更新： 2019/12/10 NCN 吉川（FS）次世代サービス業務における車両型式別点検の検証
'更新： 
'─────────────────────────────────────

Option Explicit On
Option Strict On

Imports Toyota.eCRB.iCROP.DataAccess.SC3250106
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web
Imports System.Globalization
Imports System.Data
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.BizLogic
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.DataAccess
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.BizLogic
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.DataAccess

''' <summary>
''' 部品説明画面のビジネスロジック
''' </summary>
''' <remarks>部品説明のビジネスロジッククラス</remarks>
Public Class SC3250106BusinessLogic

    Inherits BaseBusinessComponent
    Implements IDisposable

#Region "PublicConst"

#Region "ログ文言"
    ''' <summary>
    ''' Log開始用文言
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ConsLogStart As String = "Start"

    ''' <summary>
    ''' Log終了文言
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ConsLogEnd As String = "End"
#End Region


    ''' <summary>
    ''' 基幹コード区分
    ''' </summary>
    Public Enum DmsCodeType

        ''' <summary>
        ''' 区分なし
        ''' </summary>
        ''' <remarks></remarks>
        None = 0

        ''' <summary>
        ''' 販売店コード
        ''' </summary>
        ''' <remarks></remarks>
        DealerCode = 1

        ''' <summary>
        ''' 店舗コード
        ''' </summary>
        ''' <remarks></remarks>
        BranchCode = 2

        ''' <summary>
        ''' ストールID
        ''' </summary>
        ''' <remarks></remarks>
        StallId = 3

        ''' <summary>
        ''' 顧客分類
        ''' </summary>
        ''' <remarks></remarks>
        CustomerClass = 4

        ''' <summary>
        ''' 作業ステータス
        ''' </summary>
        ''' <remarks></remarks>
        WorkStatus = 5

        ''' <summary>
        ''' 中断理由区分
        ''' </summary>
        ''' <remarks></remarks>
        JobStopReasonType = 6

        ''' <summary>
        ''' チップステータス
        ''' </summary>
        ''' <remarks></remarks>
        ChipStatus = 7

        ''' <summary>
        ''' 希望連絡時間帯
        ''' </summary>
        ''' <remarks></remarks>
        ContactTimeZone = 8

        ''' <summary>
        ''' メーカー区分
        ''' </summary>
        ''' <remarks></remarks>
        MakerType = 9

    End Enum

    ''' <summary>サービス戻り値(ResultID)：ServiceSuccess</summary>
    Private Const ServiceSuccess As String = "0"

    Public Const DATABASE_ERROR As Integer = -2

    Public Const WEBSERVICE_ERROR As Integer = -1

    ' ''' <summary>
    ' ''' メーカータイプ
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Public Enum MakerType
    '    ''' <summary>1: トヨタ</summary>
    '    TOYOTA = 1
    '    ''' <summary>2:レクサス</summary>
    '    LEXUS
    '    ''' <summary>3:その他</summary>
    '    ELSE_MAKER
    'End Enum

    ' ''' <summary>
    ' ''' タイミング
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Public Class DEF_TIMING
    '    ''' <summary>車両情報特定前</summary>
    '    Public Const UNKNOWN_VEHICLE As String = "0"
    '    ''' <summary>R/O発行前（顧客承認前）</summary>
    '    Public Const BEFORE_PUBLISH As String = "10"
    '    ''' <summary>R/O発行後（顧客承認後）</summary>
    '    Public Const AFTER_PUBLISH As String = "50"
    '    ''' <summary>追加作業起票後（PS見積もり後）</summary>
    '    Public Const AFTER_ADD_WK_MAKE As String = "35"
    '    ''' <summary>Close Job後</summary>
    '    Public Const COMPLETE As String = "85"
    '    ''' <summary>キャンセル</summary>
    '    Public Const CANCEL As String = "99"
    'End Class

    ' ''' <summary>
    ' ''' 点検種類
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Public Class InspectionType
    '    Public RESULT As String = String.Empty
    '    Public SUGGEST As String = String.Empty
    'End Class

#End Region

#Region "定数"

    ''' <summary>
    ''' Log開始用文言
    ''' </summary>
    ''' <remarks></remarks>
    Private Const LOG_START As String = "Start"

    ''' <summary>
    ''' Log終了文言
    ''' </summary>
    ''' <remarks></remarks>
    Private Const LOG_END As String = "End"

    ''' <summary>
    ''' 全販売店を意味するワイルドカード販売店コード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ConsAllDealerCode As String = "XXXXX"

    ''' <summary>
    ''' 全店舗を意味するワイルドカード店舗コード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ConsAllBranchCode As String = "XXX"

    ''' <summary>
    ''' 点検結果(Replace)
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared ConsInspecRsltCd_NeedReplace As Integer = 3

    ''' <summary>
    ''' カンマ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ConsComma As Char = ","c

    '2019/07/05　TKM要件:型式対応　START　↓↓↓

    ''' <summary>
    ''' システム設定不備エラー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ErrorSystemSetting As String = "1101"

#End Region

#Region "列挙型"
    ''' <summary>
    ''' 閾値との交点を算出際に使用するカラムIndex
    ''' </summary>
    ''' <remarks></remarks>
    Private Enum Enum_ColIndex
        InspectionApprovalDateTime = 0
        Mile = 1
        Val = 2
    End Enum
#End Region

#Region "販売店システム設定"
    ''' <summary>
    ''' 残量グラフのX軸最大値
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared ConsKeySettingGraphMileageMaxVal As String = "GRAPH_DISP_MAX_MILE"
    Public Shared ConsValueSettingGraphMileageMaxVal As Integer = 500000

#End Region

    '2019/07/05　TKM要件:型式対応　START　↓↓↓
#Region "Private変数"

    ''' <summary>
    ''' 型式使用フラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private useFlgKatashiki As Boolean

#End Region

#Region "公開メソッド"

    ''' <summary>
    ''' 型式使用フラグの取得
    ''' </summary>
    ''' <param name="strRoNum">R/O番号</param>
    ''' <param name="strDlrCd">販売店コード</param>
    ''' <param name="strBrnCd">店舗コード</param>
    ''' <remarks></remarks>
    Public Sub New(Optional ByVal strRoNum As String = "", Optional ByVal strDlrCd As String = "", Optional ByVal strBrnCd As String = "")

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                        , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        If String.IsNullOrEmpty(strRoNum) Or String.IsNullOrEmpty(strDlrCd) Or String.IsNullOrEmpty(strBrnCd) Then
            Return
        End If

        Dim tableAdapter As New SC3250106DataSet
        Dim dt As DataTable = tableAdapter.GetDlrCdExistMst(strRoNum, strDlrCd, strBrnCd)
        Dim　katashiki_exist As Boolean = False
        If dt.Rows.Count > 0 Then
            katashiki_exist = "1".Equals(dt(0)("KATASHIKI_EXIST").ToString())
        End If
        
        If katashiki_exist = True Then
            SetUseFlgKatashiki(True)
        Else
            SetUseFlgKatashiki(False)
        End If

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END [Result=Return:{2}]" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , katashiki_exist.ToString))

    End Sub

    '2019/07/05　TKM要件:型式対応　END　↑↑↑

    ''' <summary>
    ''' グラフ設定情報取得処理
    ''' </summary>
    ''' <param name="vclVin">VIN</param>
    ''' <param name="inspecItemcd">点検項目コード</param>
    ''' <param name="dealerCd">販売店コード</param>
    ''' <param name="branchCd">店舗コード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetUpsellChartSettingData(
                        ByVal vclVin As String, _
                        ByVal inspecItemCd As String, _
                        ByVal dealerCd As String, _
                        ByVal branchCd As String
                                            ) As SC3250106DataSet.UpsellChartSettingDataTable

        'メソッド名取得
        Dim methodName As String = System.Reflection.MethodBase.GetCurrentMethod.Name

        'ログ出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                "{0}.{1} {2} P1:{3} P2:{4} P3:{5} P4:{6}", _
                                Me.GetType.ToString, _
                                methodName, _
                                ConsLogStart, _
                                vclVin, _
                                inspecItemCd, _
                                dealerCd, _
                                branchCd, _
                                Me.useFlgKatashiki
                                ))

        '2020/01/31 NCN 吉川 TKM要件：型式対応 Start
        'データ取得
        '①型式フラグ TRUE:自店舗コード+型式 　FALSE 自店舗コード+モデル 
        Dim dt As SC3250106DataSet.UpsellChartSettingDataTable _
                = SC3250106DataSet.GetUpsellChartSetting(vclVin, _
                                                         inspecItemCd, _
                                                         dealerCd, _
                                                         branchCd, _
                                                         Me.useFlgKatashiki)
        '②全店舗コードにして再取得
        If dt.Rows.Count = 0 Then
            dt = SC3250106DataSet.GetUpsellChartSetting(vclVin,
                             inspecItemCd, _
                             "XXXXX", _
                             "XXX", _
                             Me.useFlgKatashiki)
        End If


        '型式からデータ取得できなかった場合は、型式を使用しないデータ取得を実行
        If Me.useFlgKatashiki Then
            If dt.Rows.Count = 0 Then
                SetUseFlgKatashiki(False)
                '③自店舗+モデルで再取得
                dt = SC3250106DataSet.GetUpsellChartSetting(vclVin, _
                                                         inspecItemCd, _
                                                         dealerCd, _
                                                         branchCd, _
                                                         Me.useFlgKatashiki)
            End If

            If dt.Rows.Count = 0 Then
                '⓸全店舗+モデルで再取得
                dt = SC3250106DataSet.GetUpsellChartSetting(vclVin,
                                 inspecItemCd, _
                                 "XXXXX", _
                                 "XXX", _
                                 Me.useFlgKatashiki)
            End If
        End If
                    'ログ出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                "{0}.{1} {2}  QUERY:COUNT = {3}", _
                                Me.GetType.ToString, _
                                methodName, _
                                ConsLogEnd, _
                                    dt.Rows.Count
                                    ))
        '2020/01/31 NCN 吉川 TKM要件：型式対応  END

        Return dt

    End Function

    ''' <summary>
    ''' グラフ表示データ取得処理
    ''' </summary>
    ''' <param name="vclVin">VIN</param>
    ''' <param name="partsGroupCd">部品グループコード(取得用)</param>
    ''' <param name="graphDspOrder">グラフ表示順</param>
    ''' <param name="graphRecommendReplaceVal">推奨交換値</param>
    ''' <param name="dealerCd">販売店コード</param>
    ''' <param name="branchCd">店舗コード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetUpsellChartData(
                            ByVal vclVin As String, _
                            ByVal partsGroupCd As String, _
                            ByVal graphDspOrder As Integer, _
                            ByVal graphRecommendReplaceVal As Decimal, _
                            ByVal dealerCd As String, _
                            ByVal branchCd As String
                                            ) As SC3250106DataSet.UpsellChartDataDataTable

        'メソッド名取得
        Dim methodName As String = System.Reflection.MethodBase.GetCurrentMethod.Name

        'ログ出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                "{0}.{1} {2} P1:{3} P2:{4} P3:{5} P4:{6} P5:{7} P6:{8}", _
                                Me.GetType.ToString, _
                                methodName, _
                                ConsLogStart, _
                                vclVin, _
                                partsGroupCd, _
                                graphDspOrder, _
                                graphRecommendReplaceVal, _
                                dealerCd, _
                                branchCd
                                ))

        'dtTempのCloneを作成する(返却用)
        Using dtResultData As SC3250106DataSet.UpsellChartDataDataTable = New SC3250106DataSet.UpsellChartDataDataTable
            'データ取得
            Using dtTemp As SC3250106DataSet.TempUpsellChartDataDataTable _
                    = SC3250106DataSet.GetUpsellChartData(vclVin, _
                                                          partsGroupCd, _
                                                          graphDspOrder, _
                                                          dealerCd, _
                                                          branchCd)

                '取得件数チェック
                If dtTemp.Rows.Count > 0 Then
                    'グラフ描画処理
                    '予測値算出⇒直近で一番数値の高いデータ以降から算出
                    Dim backRsltVal As Decimal = 0D '前データのBefore値を取得
                    'dtTempのCloneを作成する(返却用)
                    '取得テーブルから作業用テーブルをCloneで作成
                    Using dtWork As SC3250106DataSet.UpsellChartDataDataTable = DirectCast(dtResultData.Clone, SC3250106DataSet.UpsellChartDataDataTable)
                        '抽出テーブルから走行距離の合計値を求める
                        Dim SumRegMile As Long = 0
                        Dim SumObj As Object = dtTemp.Compute("Sum(REG_MILE)", Nothing)
                        If Not IsNothing(SumObj) Then
                            '集計結果がNothingでなかったらオブジェクトを数値変換する
                            SumRegMile = Long.Parse(SumObj.ToString)
                        End If
                        '走行距離合計値がゼロだったら最初の1件のみ返却テーブルに移送
                        If SumRegMile = 0 Then
                            'ログ出力
                            Logger.Info("graphDspOrder = " & graphDspOrder.ToString & " And Sum(REG_MILE) = 0")
                            '返却用テーブルにも移送
                            dtResultData.ImportRow(dtTemp(0))
                        Else
                            For i = 0 To dtTemp.Rows.Count - 1
                                '前回入力値を上回っていたら作業用テーブルをクリアする
                                If backRsltVal < dtTemp(i).RSLT_VAL Then
                                    dtWork.Clear()
                                End If
                                '前回入力値を退避する
                                backRsltVal = dtTemp(i).RSLT_VAL
                                '入力値を一時テーブルに移送
                                Dim newRow As SC3250106DataSet.UpsellChartDataRow = dtWork.NewUpsellChartDataRow
                                newRow.INSPECTION_APPROVAL_DATETIME = dtTemp(i).INSPECTION_APPROVAL_DATETIME
                                newRow.REG_MILE = dtTemp(i).REG_MILE
                                newRow.RSLT_VAL = dtTemp(i).RSLT_VAL
                                newRow.SUB_INSPEC_ITEM_NAME = dtTemp(i).SUB_INSPEC_ITEM_NAME
                                dtWork.Rows.Add(newRow)
                                '返却用テーブルにも移送
                                dtResultData.ImportRow(newRow)
                            Next
                            '予測値算出元データが2件以上かつ、直近の計測値が交換推奨値を上回っているとき
                            If dtWork.Rows.Count > 1 AndAlso _
                               dtWork(dtWork.Rows.Count - 1).RSLT_VAL > graphRecommendReplaceVal Then
                                '予測値の算出を行う
                                Using dtRecommendReplaceData As SC3250106DataSet.UpsellChartDataDataTable _
                                    = GetGraphRecommendReplaceVal(dtWork, graphRecommendReplaceVal)
                                    '予測値が返却されたら返却用データテーブルに行を追加する
                                    If dtRecommendReplaceData.Rows.Count > 0 Then
                                        '算出した日付及び走行距離をDataTableに追加
                                        Dim row As SC3250106DataSet.UpsellChartDataRow = DirectCast(dtResultData.NewRow, SC3250106DataSet.UpsellChartDataRow)
                                        row.INSPECTION_APPROVAL_DATETIME = dtRecommendReplaceData(0).INSPECTION_APPROVAL_DATETIME
                                        row.REG_MILE = dtRecommendReplaceData(0).REG_MILE '距離
                                        row.RSLT_VAL = dtRecommendReplaceData(0).RSLT_VAL '計測値
                                        row.SUB_INSPEC_ITEM_NAME = dtRecommendReplaceData(0).SUB_INSPEC_ITEM_NAME
                                        dtResultData.Rows.Add(row)
                                    End If
                                End Using
                            End If
                        End If
                    End Using
                    'ログ出力
                    Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                            "{0}.{1} {2} QUERY:COUNT = {3}", _
                                            Me.GetType.ToString, _
                                            methodName, _
                                            ConsLogEnd, _
                                            dtResultData.Rows.Count
                                            ))

                End If
            End Using
            Return dtResultData
        End Using
    End Function

    ''' <summary>
    ''' 販売店システム設定値を設定値名を条件に取得する
    ''' </summary>
    ''' <param name="settingName">販売店システム設定値名</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDlrSystemSettingValueBySettingName(ByVal settingName As String) As String

        'メソッド名取得
        Dim methodName As String = System.Reflection.MethodBase.GetCurrentMethod.Name

        Logger.Info(String.Format(CultureInfo.InvariantCulture _
                                  , "{0} {1} SETTINGNAME={2}" _
                                  , methodName _
                                  , ConsLogStart _
                                  , settingName))

        '戻り値
        Dim retValue As String = String.Empty
        'ログイン情報
        Dim userContext As StaffContext = StaffContext.Current
        '販売店システム設定テーブルから取得
        Using dt As SC3250106DataSet.DlrSystemSettingValueDataTable _
                                = SC3250106DataSet.GetDlrSystemSettingValue(userContext.DlrCD, _
                                                                          userContext.BrnCD, _
                                                                          ConsAllDealerCode, _
                                                                          ConsAllBranchCode, _
                                                                          settingName)
            If dt.Rows.Count > 0 Then
                '設定値を取得
                retValue = dt.Item(0).SETTING_VAL
            Else
                retValue = ""
            End If
        End Using

        '終了ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} {2} OUT:retValue = {3}" _
                    , Me.GetType.ToString _
                    , methodName _
                    , ConsLogEnd _
                    , retValue))


        Return retValue

    End Function

    ''' <summary>
    ''' 閾値到達時の日付及び走行距離を算出する
    ''' </summary>
    ''' <param name="graphRecommendReplaceVal">推奨交換値</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetGraphRecommendReplaceVal(ByVal dt As SC3250106DataSet.UpsellChartDataDataTable, _
                                                 ByVal graphRecommendReplaceVal As Decimal) As SC3250106DataSet.UpsellChartDataDataTable

        'メソッド名取得
        Dim methodName As String = System.Reflection.MethodBase.GetCurrentMethod.Name
        Dim retData As SC3250106DataSet.UpsellChartDataDataTable = Nothing
        Try

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} {2} " _
                        , Me.GetType.ToString _
                        , methodName _
                        , ConsLogStart))

            Dim dateList As List(Of Date) = New List(Of Date)           '部品交換日
            Dim mileList_x As List(Of Decimal) = New List(Of Decimal)   '走行距離(Km)
            Dim valList_y As List(Of Decimal) = New List(Of Decimal)    '計測値(mm)
            Dim daysList_z As List(Of Long) = New List(Of Long)         '日数差(日)

            Dim sumVal_x As Decimal = 0
            Dim sumVal_y As Decimal = 0
            Dim sumVal_z As Decimal = 0
            Dim sumVal_xx As Decimal = 0
            Dim sumVal_xy As Decimal = 0
            Dim sumVal_zz As Decimal = 0
            Dim sumVal_xz As Decimal = 0

            '初回との差を算出
            For i As Integer = 0 To dt.Rows.Count - 1
                Logger.Info("DEBUG:i = " & i.ToString)
                Dim row As SC3250106DataSet.UpsellChartDataRow = dt(i)
                If i > 0 Then
                    '部品交換日セット
                    dateList.Add(row.INSPECTION_APPROVAL_DATETIME)

                    Logger.Info("DEBUG:row.REG_MILE = " & row.REG_MILE.ToString)
                    Logger.Info("DEBUG:dt(0).REG_MILE = " & dt(0).REG_MILE.ToString)
                    Logger.Info("DEBUG:row.RSLT_VAL = " & row.RSLT_VAL.ToString)
                    Logger.Info("DEBUG:dt(0).RSLT_VAL = " & dt(0).RSLT_VAL.ToString)

                    'Km差及び合計算出
                    mileList_x.Add(row.REG_MILE - dt(0).REG_MILE)
                    sumVal_x = sumVal_x + row.REG_MILE - dt(0).REG_MILE

                    'mm差及び合計算出
                    valList_y.Add((row.RSLT_VAL - dt(0).RSLT_VAL) * -1)
                    sumVal_y = sumVal_y + (row.RSLT_VAL - dt(0).RSLT_VAL) * -1

                    '経過日数を算出する
                    Dim daysCount As Long = 0
                    daysCount = DateDiff(DateInterval.Day, _
                                       dt(0).INSPECTION_APPROVAL_DATETIME.Date, _
                                       row.INSPECTION_APPROVAL_DATETIME.Date)

                    Logger.Info(String.Format("DEBUG:dt(0).INSPECTION_APPROVAL_DATETIME = {0} -> {1}", dt(0).INSPECTION_APPROVAL_DATETIME.ToString _
                                                                                                      , dt(0).INSPECTION_APPROVAL_DATETIME.Date.ToString))
                    Logger.Info(String.Format("DEBUG:row.INSPECTION_APPROVAL_DATETIME = {0} -> {1}", row.INSPECTION_APPROVAL_DATETIME.ToString _
                                                                                                    , row.INSPECTION_APPROVAL_DATETIME.Date.ToString))
                    Logger.Info("DEBUG:DateDiff = " & daysCount)

                    '日数差及び合計算出
                    daysList_z.Add(daysCount)
                    sumVal_z = sumVal_z + daysCount
                End If
            Next

            '経過日数合計=0は予測値算出不可能と判断する
            If sumVal_z = 0 Then
                '0件データを返す
                retData = New SC3250106DataSet.UpsellChartDataDataTable
                Return retData
            End If

            '1km当たり消費量の算出のために一時計算
            For i As Integer = 0 To daysList_z.Count - 1
                'MathクラスはDoubleを返すので使用しない
                'km差の2乗 合計算出
                sumVal_xx = sumVal_xx + mileList_x(i) * mileList_x(i)
                'km差×mm差 合計算出
                sumVal_xy = sumVal_xy + mileList_x(i) * valList_y(i)
                '日数差×日数差 合計算出
                sumVal_zz = sumVal_zz + daysList_z(i) * daysList_z(i)
                '日数差×Km差 合計算出
                sumVal_xz = sumVal_xz + mileList_x(i) * daysList_z(i)
            Next
            Logger.Info("DEBUG:sumVal_x = " & sumVal_x.ToString)
            Logger.Info("DEBUG:sumVal_y = " & sumVal_y.ToString)
            Logger.Info("DEBUG:sumVal_z = " & sumVal_z.ToString)
            Logger.Info("DEBUG:sumVal_xx = " & sumVal_xx.ToString)
            Logger.Info("DEBUG:sumVal_xy = " & sumVal_xy.ToString)
            Logger.Info("DEBUG:sumVal_zz = " & sumVal_zz.ToString)
            Logger.Info("DEBUG:sumVal_xz = " & sumVal_xz.ToString)

            Logger.Info("DEBUG:(sumVal_x * sumVal_y) = " & (sumVal_x * sumVal_y).ToString)
            Logger.Info("DEBUG:(sumVal_x * sumVal_x) = " & (sumVal_x * sumVal_x).ToString)

            Logger.Info("DEBUG:(sumVal_x * sumVal_y / dt.Rows.Count) = " & (sumVal_x * sumVal_y / dt.Rows.Count).ToString)
            Logger.Info("DEBUG:(sumVal_x * sumVal_x / dt.Rows.Count) = " & (sumVal_x * sumVal_x / dt.Rows.Count).ToString)

            Logger.Info("DEBUG:(sumVal_xy - (sumVal_x * sumVal_y / dt.Rows.Count)) = " & (sumVal_xy - (sumVal_x * sumVal_y / dt.Rows.Count)).ToString)
            Logger.Info("DEBUG:(sumVal_xx - (sumVal_x * sumVal_x / dt.Rows.Count)) = " & (sumVal_xx - (sumVal_x * sumVal_x / dt.Rows.Count)).ToString)

            '1km当たり消費量の算出
            Dim ans_val As Decimal = _
                (sumVal_xy - (sumVal_x * sumVal_y / dt.Rows.Count)) / _
                (sumVal_xx - (sumVal_x * sumVal_x / dt.Rows.Count))
            Logger.Info("DEBUG:ans_val = " & ans_val.ToString)

            '日当たり走行距離の算出
            Dim ans_mile As Decimal = _
                (sumVal_xz - (sumVal_z * sumVal_x / dt.Rows.Count)) / _
                (sumVal_zz - (sumVal_z * sumVal_z / dt.Rows.Count))
            Logger.Info("DEBUG:ans_mile = " & ans_mile.ToString)

            Logger.Info("DEBUG:dt(dt.Rows.Count - 1).RSLT_VAL = " & dt(dt.Rows.Count - 1).RSLT_VAL.ToString)
            Logger.Info("DEBUG:graphRecommendReplaceVal = " & graphRecommendReplaceVal.ToString)

            '閾値に到達時の走行距離を算出(小数点以下切り捨て:"閾値-最終計測値"/1km当たり消費量+直近の走行距離)
            Dim retTempMile As Decimal = Math.Truncate((dt(dt.Rows.Count - 1).RSLT_VAL - graphRecommendReplaceVal) / ans_val)
            Logger.Info("DEBUG:retTempMile:((dt(dt.Rows.Count - 1).RSLT_VAL - graphRecommendReplaceVal) / ans_val) = " & retTempMile.ToString)
            Dim retMile As Decimal = Math.Truncate(retTempMile + dt(dt.Rows.Count - 1).REG_MILE)
            Logger.Info("DEBUG:retMile:(retTempMile + dt(dt.Rows.Count - 1).REG_MILE) = " & retMile.ToString)

            '閾値に到達するまでの日数を算出(小数点以下切り捨て:閾値に到達時するまでの走行距離/日当たり走行距離)
            Dim retDays As Decimal = Math.Truncate(retTempMile / ans_mile)
            Logger.Info("DEBUG:retDays:(retTempMile / ans_mile) = " & retDays.ToString)

            '閾値到達時の日付を取得(完成検査承認日時+閾値に到達するまでの日数)
            Dim retDate As Date = dateList(dateList.Count - 1).AddDays(CDec(retDays))
            Logger.Info("DEBUG:retDate = " & retDate.ToString)

            ''閾値に到達するまでの日数を算出(小数点以下切り捨て:"閾値-最終計測値"/日当たり消費量)
            'Dim retDays As Decimal = Math.Truncate((dt(dt.Rows.Count - 1).RSLT_VAL - graphRecommendReplaceVal) / ans_val + 0.5D)

            '返却用に値をセット
            retData = New SC3250106DataSet.UpsellChartDataDataTable
            Dim newRow As SC3250106DataSet.UpsellChartDataRow = retData.NewUpsellChartDataRow
            newRow.INSPECTION_APPROVAL_DATETIME = retDate
            newRow.REG_MILE = retMile
            newRow.RSLT_VAL = graphRecommendReplaceVal
            newRow.SUB_INSPEC_ITEM_NAME = " "
            retData.Rows.Add(newRow)
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} {2} " _
                        , Me.GetType.ToString _
                        , methodName _
                        , ConsLogEnd))

            Return retData

        Catch ex As Exception
            'エラー時
            Logger.Error(String.Format(CultureInfo.CurrentCulture _
                         , "{0}.{1} OUT:ErrGetGraphRecommendReplaceVal = {2}" _
                         , Me.GetType.ToString _
                         , methodName _
                         , ex.Message))

            'エラー時には0件データを返す
            retData = New SC3250106DataSet.UpsellChartDataDataTable
            Return retData
        Finally
            'オブジェクトの解放
            If Not IsNothing(retData) Then
                retData.Dispose()
            End If

        End Try

    End Function

    ' ''' <summary>
    ' ''' 閾値到達時の日付及び走行距離を算出する
    ' ''' </summary>
    ' ''' <param name="startList">始点データ</param>
    ' ''' <param name="endList">終点データ</param>
    ' ''' <param name="graphRecommendReplaceVal">推奨交換値</param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Private Function GetGraphRecommendReplaceVal(ByVal startList As String, _
    '                                             ByVal endList As String, _
    '                                             ByVal graphRecommendReplaceVal As Double) As String

    '    'メソッド名取得
    '    Dim methodName As String = System.Reflection.MethodBase.GetCurrentMethod.Name

    '    Logger.Error(String.Format(CultureInfo.CurrentCulture _
    '                , "{0}.{1} {2} " _
    '                , Me.GetType.ToString _
    '                , methodName _
    '                , ConsLogStart))

    '    Dim startStr() As String = GetStringValue(startList) '始点の値
    '    Dim endStr() As String = GetStringValue(endList)     '終点の値

    '    '2点間の経過日数を算出(DateDiff関数使用・始点.完成検査承認日時, 始点.完成検査承認日時)
    '    Dim days As Long = DateDiff(DateInterval.Day, _
    '                               ConvStringToDateValue(startStr(Enum_ColIndex.InspectionApprovalDateTime)), _
    '                               ConvStringToDateValue(endStr(Enum_ColIndex.InspectionApprovalDateTime)))

    '    '2点間の溝の減少値を算出(始点.溝の深さ-終点.溝の深さ)
    '    Dim groove As Double = ConvStringToDoubleValue(startStr(Enum_ColIndex.Val)) - ConvStringToDoubleValue(endStr(Enum_ColIndex.Val))
    '    '2点間の走行距離の加算値を算出(終点.走行距離-終点.走行距離)
    '    Dim mile As Double = ConvStringToDoubleValue(endStr(Enum_ColIndex.Mile)) - ConvStringToDoubleValue(startStr(Enum_ColIndex.Mile))

    '    '1日あたりの溝の減少値を算出(溝の減少値/経過日数)
    '    Dim avgGroove As Double = groove / days
    '    '1日あたりの走行距離の加算値を算出(走行距離の加算値/経過日数)
    '    Dim avgMile As Double = mile / days

    '    '閾値に到達するまでの日数を算出(小数点以下切り上げ:"閾値-終点.溝の深さ"/1日あたりの溝の減少値)
    '    Dim d As Double = Math.Ceiling((ConvStringToDoubleValue(endStr(Enum_ColIndex.Val)) - graphRecommendReplaceVal) / avgGroove)
    '    '閾値到達時の日付を取得(終点.完成検査承認日時+閾値に到達するまでの日数)
    '    Dim retDate As Date = ConvStringToDateValue(endStr(Enum_ColIndex.InspectionApprovalDateTime)).AddDays(d)
    '    '閾値に到達時の走行距離を算出(1日あたりの走行距離*閾値に到達するまでの日数)
    '    Dim m As Double = avgMile * d

    '    '返却値の作成(閾値到達時の日付+走行距離+溝(=推奨交換値))
    '    Dim s As String = SetStringValue({ConvDateToStringValue(retDate), _
    '                                      ConvDoubleToStringValue(m), _
    '                                      ConvDoubleToStringValue(graphRecommendReplaceVal)})

    '    Logger.Error(String.Format(CultureInfo.CurrentCulture _
    '                , "{0}.{1} {2} " _
    '                , Me.GetType.ToString _
    '                , methodName _
    '                , ConsLogEnd))

    '    Return s
    'End Function

    ''' <summary>
    ''' 日付の変換(取得)
    ''' </summary>
    '''<param name="s">日付が格納されている文字列</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ConvStringToDateValue(ByVal s As String) As Date
        Return Date.Parse(s)
    End Function

    ''' <summary>
    ''' 日付の変換(設定)
    ''' </summary>
    '''<param name="d">日付</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ConvDateToStringValue(ByVal d As Date) As String
        Return d.ToString
    End Function

    ''' <summary>
    ''' 数値の変換(取得)
    ''' </summary>
    '''<param name="s">数値が格納されている文字列</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ConvStringToDoubleValue(ByVal s As String) As Double
        Return Double.Parse(s)
    End Function

    ''' <summary>
    ''' 数値の変換(設定)
    ''' </summary>
    '''<param name="d">数値</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ConvDoubleToStringValue(ByVal d As Double) As String
        Return d.ToString
    End Function

    ''' <summary>
    ''' 数値の変換(取得)
    ''' </summary>
    '''<param name="s">数値が格納されている文字列</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ConvStringToDecimalValue(ByVal s As String) As Decimal
        Return Decimal.Parse(s)
    End Function

    ''' <summary>
    ''' 数値の変換(設定)
    ''' </summary>
    '''<param name="d">数値</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ConvDecimalToStringValue(ByVal d As Decimal) As String
        Return d.ToString
    End Function

    ''' <summary>
    ''' 文字列のカンマ分割
    ''' </summary>
    '''<param name="str">連結したい文字列</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetStringValue(ByVal str As String) As String()
        Dim s() As String = str.Split(ConsComma)
        Return s
    End Function

    ''' <summary>
    ''' 文字列のカンマ連結
    ''' </summary>
    '''<param name="str">連結したい文字列</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SetStringValue(ByVal str() As String) As String
        Dim s As String = String.Concat(str.ToList.ToString, ConsComma.ToString)
        Return s
    End Function

    ''' <summary>
    ''' 型式使用フラグ設定
    ''' </summary>
    '''<param name="str">型式使用フラグ</param>
    ''' <returns></returns>
    ''' <remarks></remarks
    Private Sub SetUseFlgKatashiki(ByVal useFlgKatashiki As Boolean)
        Me.useFlgKatashiki = useFlgKatashiki
    End Sub

#End Region

#Region "IDisposable Support"
    Private disposedValue As Boolean ' 重複する呼び出しを検出するには

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: マネージ状態を破棄します (マネージ オブジェクト)。
            End If

            ' TODO: アンマネージ リソース (アンマネージ オブジェクト) を解放し、下の Finalize() をオーバーライドします。
            ' TODO: 大きなフィールドを null に設定します。
        End If
        Me.disposedValue = True
    End Sub

    ' TODO: 上の Dispose(ByVal disposing As Boolean) にアンマネージ リソースを解放するコードがある場合にのみ、Finalize() をオーバーライドします。
    'Protected Overrides Sub Finalize()
    '    ' このコードを変更しないでください。クリーンアップ コードを上の Dispose(ByVal disposing As Boolean) に記述します。
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' このコードは、破棄可能なパターンを正しく実装できるように Visual Basic によって追加されました。
    Public Sub Dispose() Implements IDisposable.Dispose
        ' このコードを変更しないでください。クリーンアップ コードを上の Dispose(ByVal disposing As Boolean) に記述します。
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

    '#Region "DMS情報取得"

    '    ''' <summary>
    '    ''' DMS情報取得
    '    ''' </summary>
    '    ''' <param name="inStaffInfo">sスタッフ情報</param>
    '    ''' <returns>DMS情報</returns>
    '    ''' <remarks></remarks>
    '    Public Function GetDmsDealerData(ByVal inStaffInfo As StaffContext) As ServiceCommonClassDataSet.DmsCodeMapDataTable

    '        'メソッド名取得
    '        Dim methodName As String = System.Reflection.MethodBase.GetCurrentMethod.Name

    '        Logger.Error(String.Format(CultureInfo.CurrentCulture _
    '                    , "{0}.{1} {2} " _
    '                    , Me.GetType.ToString _
    '                    , methodName _
    '                    , ConsLogStart))

    '        Using biz As New ServiceCommonClassBusinessLogic
    '            'DMS販売店データの取得
    '            Dim dtDmsCodeMapDataTable As ServiceCommonClassDataSet.DmsCodeMapDataTable = _
    '                biz.GetIcropToDmsCode(inStaffInfo.DlrCD,
    '                                      ServiceCommonClassBusinessLogic.DmsCodeType.BranchCode, _
    '                                      inStaffInfo.DlrCD, _
    '                                      inStaffInfo.BrnCD, _
    '                                      String.Empty, _
    '                                      inStaffInfo.Account)

    '            If dtDmsCodeMapDataTable.Count <= 0 Then
    '                'データが取得できない場合はエラー
    '                Logger.Error(String.Format(CultureInfo.CurrentCulture _
    '                            , "{0}.{1} ERROR:TB_M_DMS_CODE_MAP is nothing" _
    '                            , Me.GetType.ToString _
    '                            , System.Reflection.MethodBase.GetCurrentMethod.Name))
    '                Return Nothing
    '            ElseIf 1 < dtDmsCodeMapDataTable.Count Then
    '                'データが2件以上取得できた場合は一意に決定できないためエラー
    '                Logger.Error(String.Format(CultureInfo.CurrentCulture _
    '                            , "{0}.{1} ERROR:TB_M_DMS_CODE_MAP is sum data" _
    '                            , Me.GetType.ToString _
    '                            , System.Reflection.MethodBase.GetCurrentMethod.Name))
    '                Return Nothing
    '            Else
    '                Logger.Error(String.Format(CultureInfo.CurrentCulture _
    '                            , "{0}.{1} {2} QUERY:COUNT = {3}" _
    '                            , Me.GetType.ToString _
    '                    , methodName _
    '                    , ConsLogStart _
    '                    , dtDmsCodeMapDataTable.Rows.Count))

    '                Return dtDmsCodeMapDataTable
    '            End If

    '        End Using
    '    End Function

    '#End Region

End Class
