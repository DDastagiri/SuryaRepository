'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3250106.aspx.vb
'─────────────────────────────────────
'機能： 部品説明画面 コードビハインド
'補足： 
'作成： 2014/07/XX NEC 上野
'更新： 2014/08/xx NEC 村瀬
'─────────────────────────────────────

Option Explicit On
Option Strict On

Imports System.Data
Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.iCROP.BizLogic.SC3250106
Imports Toyota.eCRB.iCROP.DataAccess.SC3250106
Imports System.Web.UI.DataVisualization.Charting
Imports System.Drawing
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.BizLogic
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.DataAccess
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.BizLogic
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.DataAccess

''' <summary>
''' 部品説明画面
''' </summary>
''' <remarks></remarks>
Partial Class SC3250106
    Inherits BasePage

#Region "変数"

    ''' <summary>ログインスタッフ情報</summary>
    Private staffInfo As StaffContext

    ''' <summary>Getパラメーター格納</summary>
    Private Params As New Parameters

#End Region

#Region "定数"

    ''' <summary>
    ''' 残量グラフの閾値
    ''' (MyChart.PrePaintメソッドで使用するため事前に取得値を退避させる必要がある)
    ''' </summary>
    Private GraphRecommendReplaceVal As Decimal = 0D

    ''' <summary>
    ''' 残量グラフのY軸のMIN値
    ''' </summary>
    Private GraphPartsMinVal As Decimal = 0D

    ''' <summary>
    ''' 残量グラフのY軸のMAX値
    ''' (MyChart.PrePaintメソッドで使用するため事前に取得値を退避させる必要がある)
    ''' </summary>
    Private GraphPartsMaxVal As Decimal = 0D

    ''' <summary>
    ''' Log開始用文言
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ConsLogStart As String = "Start"

    ''' <summary>
    ''' Log終了文言
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ConsLogEnd As String = "End"

    ''' <summary>
    ''' 折れ線グラフの名称
    ''' </summary>
    ''' <remarks></remarks>
    Private ConsMyLineChart As String = "MyLineChart"

    ''' <summary>
    ''' 日付フォーマット
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ConsDateFormat As String = "dd/MM/yyyy"

    ''' <summary>
    ''' PopWindowで表示するグラフエリアのID(.aspxで設定しているIDのこと)
    ''' </summary>
    ''' <remarks></remarks>
    Private ConsMyLargeChart As String = "MyLargeChart"

    '2019/07/05　TKM要件:型式対応　START　↓↓↓
    'ビジネスロジックのNewイベントが発生する際に型式利用フラグを設定します
    ''' <summary>
    ''' ビジネスロジック
    ''' </summary>
    ''' <remarks></remarks>
    Private Biz As SC3250106BusinessLogic = New SC3250106BusinessLogic
    '2019/07/05　TKM要件:型式対応　END　　↑↑↑

#End Region

#Region "クラス"
    ''' <summary>
    ''' Getパラメーター格納用クラス
    ''' </summary>
    ''' <remarks></remarks>
    Private Class Parameters
        ''' <summary>販売店コード</summary>
        Public DealerCode As String
        ''' <summary>店舗コード</summary>
        Public BranchCode As String
        ''' <summary>ログインユーザID</summary>
        Public LoginUserID As String
        ''' <summary>SAChipID</summary>
        Public SAChipID As String
        ''' <summary>BASREZID</summary>
        Public BASREZID As String
        ''' <summary>R/O</summary>
        Public R_O As String
        ''' <summary>SEQ_NO</summary>
        Public SEQ_NO As String
        ''' <summary>VIN_NO</summary>
        Public VIN_NO As String
        ''' <summary>ViewMode 1=Readonly / 0=Edit</summary>
        Public ViewMode As String
        ''' <summary>REQ_PART_CD</summary>
        Public REQ_PART_CD As String
        ''' <summary>INSPEC_ITEM_CD</summary>
        Public INSPEC_ITEM_CD As String
    End Class
#End Region

#Region "列挙体"
    ''' <summary>
    ''' 文言ID管理(Client端必要な文言)←不要かもしれない
    ''' </summary>
    ''' <remarks></remarks>
    Private Enum WordID
        ''' <summary>マスタ無しエラーメッセージ</summary>
        id001 = 1
    End Enum

    ''' <summary>
    ''' 残量グラフ表示順
    ''' </summary>
    ''' <remarks></remarks>
    Private Enum Enum_GraphDspOrder
        FrontLeft = 1
        FrontRight = 2
        RearLeft = 3
        RearRight = 4
    End Enum

    ''' <summary>
    ''' 残量グラフエリア
    ''' </summary>
    ''' <remarks></remarks>
    Private Enum Enum_GraphArea
        Normal = 0
        Large = 1
    End Enum

#End Region

#Region "プロパティ"
    ''' <summary>
    ''' X軸のMAX値
    ''' </summary>
    ''' <remarks>販売店システム設定より取得しセットする</remarks>
    Private _settinggraphdispmaxmile As Double
    Public Property SettingGraphDispMaxMile() As Double
        Get
            Return _settinggraphdispmaxmile
        End Get
        Set(ByVal value As Double)
            _settinggraphdispmaxmile = value
        End Set
    End Property

    ''' <summary>
    ''' X軸の目盛り間隔(主要線)
    ''' </summary>
    ''' <remarks>主要線の間隔</remarks>
    Private Property SettingMajorGridIntervalValByAxisX As Double = 10000

    ''' <summary>
    ''' X軸の目盛り間隔(補助線)
    ''' </summary>
    ''' <remarks>補助線の間隔(主要線の1/2とする)</remarks>
    Private Property SettingMinorGridIntervalValByAxisX As Double = 5000

    ''' <summary>
    ''' X軸の最低ラインMAX値(データポイント数が少なかったときの最低ライン)
    ''' </summary>
    ''' <remarks>MAX値の1/10をセットする</remarks>
    Private Property SettingGraphDispLowestMaxMile As Double = 65000

    ''' <summary>
    ''' グラフの最低ラインWidth(通常グラフ)
    ''' </summary>
    ''' <remarks>拡大グラフは通常グラフの1.5倍</remarks>
    Private Property SettingGraphLowestMaxWidth As Double = 450

#End Region

#Region "イベントハンドラ"

    ''' <summary>
    ''' Page_Loadイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        'System.GC.Collect()                  'アクセス不可能なオブジェクトを除去
        'System.GC.WaitForPendingFinalizers() 'ファイナライゼーションが終わるまでスレッド待機
        'System.GC.Collect()                  'ファイナライズされたばかりのオブジェクトに関連するメモリを開放

        '開始ログの記録
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2} " _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , ConsLogStart))

        ''バージョンログの記録（＊＊＊＊＊＊＊＊配信時に必ず日付変更すること！！！！＊＊＊＊＊＊＊＊＊＊＊＊＊）
        'Logger.Error("***** Version:2014/06/03 11:57 *****")
        ''リクエストURLの記録
        'Logger.Error(String.Format("URL:{0}", Request.Url.ToString))

        ' 初期処理
        InitProc()

        'グラフ作成処理(完成検査用点検項目表示順ごとに作成）
        '設定データ取得
        Using upsellChartSettingData As SC3250106DataSet.UpsellChartSettingDataTable = _
            GetUpsellChartSettingData(Params.VIN_NO, _
                                      Params.INSPEC_ITEM_CD, _
                                      staffInfo.DlrCD, _
                                      staffInfo.BrnCD)
            '取得件数チェック
            If upsellChartSettingData.Rows.Count = 0 Then
                'マスタがない旨メッセージを表示
                lblErrMessage.Text = vbTab & WebWordUtility.GetWord(WordID.id001)
                'メッセージのみ表示し、グラフは非表示
                lblErrMessage.Style.Remove("display")
                MainGraph.Style.Add("display", "none")
                main.Style.Add("display", "none")
            Else
                'マスタがあったらメッセージは非表示
                lblErrMessage.Style.Add("display", "none")
                MainGraph.Style.Remove("display")
                '閾値とY軸MAX値を退避する(取得データ1件目固定とする。PrePaintイベントで使用)
                GraphRecommendReplaceVal = upsellChartSettingData(0).GRAPH_RECOMMEND_REPLACE_VAL
                GraphPartsMaxVal = upsellChartSettingData(0).GRAPH_PARTS_MAX_VAL
                'グラフ表示データ取得
                Call CraeteChartProc(upsellChartSettingData, Enum_GraphDspOrder.FrontLeft, Enum_GraphArea.Normal)
                Call CraeteChartProc(upsellChartSettingData, Enum_GraphDspOrder.FrontRight, Enum_GraphArea.Normal)
                Call CraeteChartProc(upsellChartSettingData, Enum_GraphDspOrder.RearLeft, Enum_GraphArea.Normal)
                Call CraeteChartProc(upsellChartSettingData, Enum_GraphDspOrder.RearRight, Enum_GraphArea.Normal)
            End If

            If IsPostBack Then
                'ポストバック処理（拡大画面を表示する）
                Me.ShowPopUpWindow(upsellChartSettingData)
            End If
        End Using


        '終了ログの記録
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , ConsLogEnd))

    End Sub

#End Region

#Region "初期設定処理"

    ''' <summary>
    ''' 初期処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitProc()

        'メソッド名取得
        Dim methodName As String = System.Reflection.MethodBase.GetCurrentMethod.Name

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} {2}" _
            , Me.GetType.ToString _
            , methodName _
            , ConsLogStart))

            Dim ret As String = String.Empty
            '残量グラフのX軸最大値
            ret = biz.GetDlrSystemSettingValueBySettingName(SC3250106BusinessLogic.ConsKeySettingGraphMileageMaxVal)
            If ret.TrimEnd.Length = 0 Then
                Me.SettingGraphDispMaxMile = SC3250106BusinessLogic.ConsValueSettingGraphMileageMaxVal
            Else
                Me.SettingGraphDispMaxMile = Integer.Parse(ret.TrimEnd)
            End If

        'スタッフ情報を取得
        staffInfo = StaffContext.Current

        ''***パラメータを取得する
        '販売店コード(DealerCode)
        Params.DealerCode = DirectCast(GetValue(ScreenPos.Current, "DealerCode", False), String)
        '店舗コード(BranchCode)
        Params.BranchCode = DirectCast(GetValue(ScreenPos.Current, "BranchCode", False), String)
        'ログインID(LoginUserID)
        Params.LoginUserID = DirectCast(GetValue(ScreenPos.Current, "LoginUserID", False), String)
        '来店実績連番(SAChipID)
        Params.SAChipID = DirectCast(GetValue(ScreenPos.Current, "SAChipID", False), String)
        'DMS予約ID（BASREZID）
        Params.BASREZID = DirectCast(GetValue(ScreenPos.Current, "BASREZID", False), String)
        'RO番号（R_O）
        Params.R_O = DirectCast(GetValue(ScreenPos.Current, "R_O", False), String)
        'RO作業連番（SEQ_NO）
        Params.SEQ_NO = DirectCast(GetValue(ScreenPos.Current, "SEQ_NO", False), String)
        'VIN（VIN_NO）
        Params.VIN_NO = DirectCast(GetValue(ScreenPos.Current, "VIN_NO", False), String)
        '編集モード（ViewMode）
        Params.ViewMode = DirectCast(GetValue(ScreenPos.Current, "ViewMode", False), String)
        '商品訴求部位モード（REQ_PART_CD）
        Params.REQ_PART_CD = DirectCast(GetValue(ScreenPos.Current, "ReqPartCD", False), String)
        '点検項目コード（INSPEC_ITEM_CD）
        Params.INSPEC_ITEM_CD = DirectCast(GetValue(ScreenPos.Current, "InspecItemCD", False), String)

        '販売店コード、店舗コード、店舗コードに関してはパラメータより取得できなかった場合、
        If String.IsNullOrWhiteSpace(Params.DealerCode) Then
            Params.DealerCode = staffInfo.DlrCD
        End If
        If String.IsNullOrWhiteSpace(Params.BranchCode) Then
            Params.BranchCode = staffInfo.BrnCD
        End If
        If String.IsNullOrWhiteSpace(Params.LoginUserID) Then
            Params.LoginUserID = staffInfo.Account
        End If

        'ユーザーIDに@が無ければ、「スタッフ識別文字列 + "@" + 販売店コード」の形にする
        If Not Params.LoginUserID.Contains("@") Then
            Params.LoginUserID = String.Format("{0}@{1}", Params.LoginUserID, Params.DealerCode)
        End If

        '2019/07/05　TKM要件:型式対応　START　↓↓↓
        Biz = New SC3250106BusinessLogic(Params.R_O, Params.DealerCode, Params.BranchCode)
        '2019/07/05　TKM要件:型式対応　END　　↑↑↑

        '***取得したパラメータ情報をログに記録
        Logger.Info(String.Format("Params:DealerCode:[{0}], BranchCode:[{1}], LoginUserID:[{2}], SAChipID:[{3}], BASREZID:[{4}], R_O:[{5}], SEQ_NO:[{6}], VIN_NO:[{7}], ViewMode:[{8}], REQ_PART_CD:[{9}], INSPEC_ITEM_CD:[{10}]", _
                                    Params.DealerCode, _
                                    Params.BranchCode, _
                                    Params.LoginUserID, _
                                    Params.SAChipID, _
                                    Params.BASREZID, _
                                    Params.R_O, _
                                    Params.SEQ_NO, _
                                    Params.VIN_NO, _
                                    Params.ViewMode, _
                                    Params.REQ_PART_CD, _
                                    Params.INSPEC_ITEM_CD
                                  ))
        Logger.Info(String.Format("StaffInfo:DlrCD:[{0}], BrnCD:[{1}], Account:[{2}]", staffInfo.DlrCD, staffInfo.BrnCD, staffInfo.Account))

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} {2}" _
            , Me.GetType.ToString _
            , methodName _
            , ConsLogEnd))

    End Sub

#End Region

#Region "グラフ作成処理"

    ''' <summary>
    ''' グラフ設定データ取得
    ''' </summary>
    ''' <param name="vclVin">VIN</param>
    ''' <param name="inspecItemcd">点検項目コード</param>
    ''' <param name="dealerCd">販売店コード</param>
    ''' <param name="branchCd">店舗コード</param>
    ''' <returns>DataTable</returns>
    ''' <remarks></remarks>
    Private Function GetUpsellChartSettingData( _
                                            ByVal vclVin As String, _
                                            ByVal inspecItemCd As String, _
                                            ByVal dealerCd As String, _
                                            ByVal branchCd As String
                                              ) As SC3250106DataSet.UpsellChartSettingDataTable
        'メソッド名取得
        Dim methodName As String = System.Reflection.MethodBase.GetCurrentMethod.Name

        '開始ログの記録
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2} P1{3} P2{4} P3{5} P4{6}" _
                  , Me.GetType.ToString _
                  , methodName _
                  , ConsLogStart _
                  , vclVin _
                  , inspecItemCd _
                  , dealerCd _
                  , branchCd))

        '2019/07/05　TKM要件:型式対応　START　↓↓↓
            Using upsellChartSettingData As SC3250106DataSet.UpsellChartSettingDataTable = _
        Biz.GetUpsellChartSettingData(vclVin, _
                                                                    inspecItemCd, _
                                                                    dealerCd, _
                                                                    branchCd)
            '2019/07/05　TKM要件:型式対応　END　　↑↑↑
                '終了ログの記録
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                          , "{0}.{1} {2} " _
                          , Me.GetType.ToString _
                          , methodName _
                          , ConsLogEnd))
                Return upsellChartSettingData
            End Using

    End Function

    ''' <summary>
    ''' グラフ表示データ取得及びグラフ作成処理
    ''' </summary>
    ''' <param name="upsellChartSettingData">グラフ設定データ</param>
    ''' <param name="graphDspOrder">グラフ表示順</param>
    ''' <remarks></remarks>
    Private Sub CraeteChartProc(ByVal upsellChartSettingData As SC3250106DataSet.UpsellChartSettingDataTable, _
                                ByVal graphDspOrder As Enum_GraphDspOrder, _
                                ByVal graphArea As Enum_GraphArea)

        'メソッド名取得
        Dim methodName As String = System.Reflection.MethodBase.GetCurrentMethod.Name

        '開始ログの記録
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2} " _
                  , Me.GetType.ToString _
                  , methodName _
                  , ConsLogStart))

        '2019/07/05　TKM要件:型式対応　START　↓↓↓
        'グラフ表示対象データを取得
            Using upsellChartData As SC3250106DataSet.UpsellChartDataDataTable = _
        Biz.GetUpsellChartData(Params.VIN_NO, _
                                        upsellChartSettingData(0).PARTS_GROUP_CD, _
                                        graphDspOrder, _
                                        GraphRecommendReplaceVal, _
                                        staffInfo.DlrCD, _
                                        staffInfo.BrnCD)
            '2019/07/05　TKM要件:型式対応　END　　↑↑↑ 

                '取得件数チェック
                If upsellChartData.Rows.Count = 0 Then
                    '該当エリアを非表示とする
                    Select Case graphDspOrder
                        Case Enum_GraphDspOrder.FrontLeft
                            ChartArea1.Style.Add("visibility", "hidden")
                            'ChartArea1.Style.Add("display", "none")
                        Case Enum_GraphDspOrder.FrontRight
                            ChartArea2.Style.Add("visibility", "hidden")
                            'ChartArea2.Style.Add("display", "none")
                        Case Enum_GraphDspOrder.RearLeft
                            ChartArea3.Style.Add("visibility", "hidden")
                            'ChartArea3.Style.Add("display", "none")
                        Case Enum_GraphDspOrder.RearRight
                            ChartArea4.Style.Add("visibility", "hidden")
                            'ChartArea4.Style.Add("display", "none")
                    End Select
                Else
                    '表示先のグラフを判断
                    Dim activeChart As Chart = Nothing
                    Select Case graphArea
                        Case Enum_GraphArea.Normal
                            'ノーマルエリア
                            Select Case graphDspOrder
                                Case Enum_GraphDspOrder.FrontLeft
                                    activeChart = Me.MyChartFL
                                    SubTitleTextFL.InnerText = upsellChartData(0).SUB_INSPEC_ITEM_NAME
                                Case Enum_GraphDspOrder.FrontRight
                                    activeChart = Me.MyChartFR
                                    SubTitleTextFR.InnerText = upsellChartData(0).SUB_INSPEC_ITEM_NAME
                                Case Enum_GraphDspOrder.RearLeft
                                    activeChart = Me.MyChartRL
                                    SubTitleTextRL.InnerText = upsellChartData(0).SUB_INSPEC_ITEM_NAME
                                Case Enum_GraphDspOrder.RearRight
                                    activeChart = Me.MyChartRR
                                    SubTitleTextRR.InnerText = upsellChartData(0).SUB_INSPEC_ITEM_NAME
                            End Select
                        Case Enum_GraphArea.Large
                            '拡大エリア
                            activeChart = Me.MyLargeChart
                            LargeContentTitle.Text = upsellChartData(0).SUB_INSPEC_ITEM_NAME
                    End Select

                    'X軸、Y軸等の設定(マスタ取得データ1件目固定)
                    Call SettingChartAreas(activeChart, upsellChartSettingData(0))

                    'グラフ描画処理
                    For i = 0 To upsellChartData.Rows.Count - 1
                        '最終データだったらX軸のMAX値を算出してセットする
                        If i = upsellChartData.Count - 1 Then
                            '最終データの走行距離が0kmだったら●マーカーを追加する
                            If upsellChartData(i).REG_MILE = 0 Then
                                Call SetDataPointAndCalloutAnnotationCircle( _
                                                        activeChart, _
                                                        upsellChartData(i))
                            Else
                                '計測値と閾値をチェック
                                If upsellChartData(i).RSLT_VAL = GraphRecommendReplaceVal Then
                                    '計測値が閾値と同じ値だったら★マーカーを追加する
                                    Call SetDataPointAndCalloutAnnotationStar( _
                                                            activeChart, _
                                                            upsellChartData(i))
                                Else
                                    '計測値が閾値以外だったら●マーカーを追加する
                                    Call SetDataPointAndCalloutAnnotationCircle( _
                                                            activeChart, _
                                                            upsellChartData(i))
                                End If
                            End If
                            '走行距離をもとにX軸のMAX値を算出
                            Call SetMaxValueByAxisXAndWidth(activeChart, upsellChartData(i).REG_MILE)
                        Else
                            '最終データでなかったら●マーカーを追加する
                            Call SetDataPointAndCalloutAnnotationCircle( _
                                                    activeChart, _
                                                        upsellChartData(i))
                        End If
                    Next
                End If
                '終了ログの記録
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                          , "{0}.{1} {2} COUNT={3}" _
                          , Me.GetType.ToString _
                          , methodName _
                          , ConsLogEnd _
                          , upsellChartData.Rows.Count.ToString))
            End Using

    End Sub

    'X軸、Y軸の設定
    ''' <summary>
    ''' 指定グラフに対し、X軸及びY軸の設定を行う
    ''' </summary>
    ''' <param name="chart">設定先グラフ</param>
    ''' <param name="row">設定データ</param>
    ''' <remarks></remarks>
    Private Sub SettingChartAreas(ByVal chart As Chart, ByVal row As SC3250106DataSet.UpsellChartSettingRow)

        'メソッド名取得
        Dim methodName As String = System.Reflection.MethodBase.GetCurrentMethod.Name

        '開始ログの記録
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2} " _
                  , Me.GetType.ToString _
                  , methodName _
                  , ConsLogStart))

        '大枠の設定
        With chart
            'グラフの外部分をグラデーションで描画
            .BackGradientStyle = GradientStyle.TopBottom
            .BackColor = Color.FromArgb(250, 250, 250)
            .BackSecondaryColor = Color.FromArgb(255, 255, 255)
            'グラフ追加
            .Series.Add(ConsMyLineChart)
            '種別を折れ線グラフに設定
            .Series(ConsMyLineChart).ChartType = SeriesChartType.Line
            .Series(ConsMyLineChart).BorderWidth = 2
            'ラベルの設定(ポイントの値(mm,km)を表示しない)
            .Series(ConsMyLineChart).IsValueShownAsLabel = False
            'グラフの色
            .Series(ConsMyLineChart).Color = Color.LimeGreen
            .Series(ConsMyLineChart).BorderDashStyle = ChartDashStyle.Solid '実線
        End With

        'ChartAreaの追加
        chart.ChartAreas.Add(New ChartArea)

        'グラフ内の設定
        With chart.ChartAreas(0)
            .BackGradientStyle = GradientStyle.None
            .BackColor = Color.FromArgb(220, 255, 255, 255)

            'グラフエリアの配置設定（余白をなくして全画面に表示する状態にする）
            With .Position
                .Auto = False   '自動配置設定
                .Width = 100    '幅(%)
                .Height = 100   '高さ(%)
                .X = 0          '左上座標(X)
                .Y = 0          '左上座標(Y)
            End With

            'X軸の設定
            With .AxisX
                .Minimum = 0                                        '走行距離kmの最小値
                '.Maximum = Me.SettingGraphMileageMaxVal            '走行距離kmの最大値(後に算出するのでここでは設定不要)
                .Interval = Me.SettingMajorGridIntervalValByAxisX   '走行距離kmのメモリ間隔
                .LineDashStyle = ChartDashStyle.Solid
                .LabelAutoFitMaxFontSize = 7
                .LabelAutoFitMinFontSize = 7
                .LineColor = Color.Black
                .LineWidth = 1
                '主要線の書式
                .MajorGrid.LineDashStyle = ChartDashStyle.Dot
                .MajorGrid.LineWidth = 1
                .MajorGrid.LineColor = Color.Gray
                .MajorTickMark.Enabled = False
                '補助線の書式
                .MinorGrid.LineDashStyle = ChartDashStyle.Dot
                .MinorGrid.LineWidth = 1
                .MinorGrid.Interval = Me.SettingMinorGridIntervalValByAxisX '補助線目盛り間隔
                .MinorGrid.LineColor = Color.LightGray
                .MinorGrid.Enabled = True
            End With

            '第2X軸の設定
            With .AxisX2
                .Enabled = AxisEnabled.True     '軸線を表示する
                .LineDashStyle = ChartDashStyle.Solid    '実線
                .MajorGrid.Enabled = False      '主要線を引かない
                .MajorTickMark.Enabled = False  '目盛りマークを表示しない
                .LabelStyle.Enabled = False     'ラベルを表示しない
            End With

            'Y軸の設定
            With .AxisY
                .TitleAlignment = StringAlignment.Center
                .Minimum = GraphPartsMinVal               'Y軸の最小値
                .Maximum = row.GRAPH_PARTS_MAX_VAL        '訴求用グラフ設定マスタ.部品初期値
                .Interval = row.GRAPH_GRADUATION          '訴求用グラフ設定マスタ.目盛り間隔
                .LabelStyle.Format = "{#0.0} " & row.GRAPH_DISP_UNIT    '訴求用グラフ設定マスタ.表示単位
                .LineDashStyle = ChartDashStyle.Solid     '実線
                .LabelAutoFitMaxFontSize = 7
                .LabelAutoFitMinFontSize = 7
                .LineColor = Color.Black
                .LineWidth = 1
                '主要線の書式
                .MajorGrid.LineDashStyle = ChartDashStyle.Dot    'ドット線
                .MajorGrid.LineWidth = 1
                .MajorGrid.LineColor = Color.Gray
                .MajorTickMark.Enabled = False  '目盛りマークを表示しない
            End With

            '第2Y軸の設定
            With .AxisY2
                .Enabled = AxisEnabled.True     '軸線を表示する
                .LineDashStyle = ChartDashStyle.Solid    '実線
                .MajorGrid.Enabled = False      '主要線を引かない
                .MajorTickMark.Enabled = False  '目盛りマークを表示しない
                .LabelStyle.Enabled = False     'ラベルを表示しない
            End With

        End With

        '終了ログの記録
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2} " _
                  , Me.GetType.ToString _
                  , methodName _
                  , ConsLogEnd))

    End Sub

    ''' <summary>
    ''' データポイントの設定を行う(ポイントマーカー:●)
    ''' </summary>
    ''' <param name="chart">表示先のグラフ</param>
    ''' <param name="row">セットするデータ</param>
    ''' <remarks>この処理から注釈作成処理を呼び出し、設定している</remarks>
    Private Sub SetDataPointAndCalloutAnnotationCircle( _
                                ByVal chart As Chart, _
                                ByVal row As SC3250106DataSet.UpsellChartDataRow)

        'メソッド名取得
        Dim methodName As String = System.Reflection.MethodBase.GetCurrentMethod.Name

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} {2}" _
            , Me.GetType.ToString _
            , methodName _
            , ConsLogStart))
        'データポイントセット処理呼び出し
        Call SetCalloutAnnotation( _
                                chart, _
                                row, _
                                MarkerStyle.Circle)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} {2}" _
            , Me.GetType.ToString _
            , methodName _
            , ConsLogEnd))
    End Sub

    ''' <summary>
    ''' データポイントの設定を行う(ポイントマーカー:★)
    ''' </summary>
    ''' <param name="chart">表示先のグラフ</param>
    ''' <param name="row">セットするデータ</param>
    ''' <remarks>この処理から注釈作成処理を呼び出し、設定している</remarks>
    Private Sub SetDataPointAndCalloutAnnotationStar( _
                                ByVal chart As Chart, _
                                ByVal row As SC3250106DataSet.UpsellChartDataRow)

        'メソッド名取得
        Dim methodName As String = System.Reflection.MethodBase.GetCurrentMethod.Name

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} {2}" _
            , Me.GetType.ToString _
            , methodName _
            , ConsLogStart))

        'データポイントセット処理呼び出し
        Call SetCalloutAnnotation( _
                                chart, _
                                row, _
                                MarkerStyle.Star5)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} {2}" _
            , Me.GetType.ToString _
            , methodName _
            , ConsLogEnd))
    End Sub

    ''' <summary>
    ''' データポイントに付加する注釈を設定する
    ''' </summary>
    ''' <param name="chart">表示先のグラフ</param>
    ''' <param name="row">セットするデータ</param>
    ''' <param name="markerStyle">ポイントの形状</param>
    ''' <remarks></remarks>
    Private Sub SetCalloutAnnotation(ByVal chart As Chart, _
                                     ByVal row As SC3250106DataSet.UpsellChartDataRow, _
                                     ByVal markerStyle As MarkerStyle)

        'メソッド名取得
        Dim methodName As String = System.Reflection.MethodBase.GetCurrentMethod.Name

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} {2}" _
            , Me.GetType.ToString _
            , methodName _
            , ConsLogStart))

        '計測値をデータポイントにセットする
        Using dataPoint As DataPoint = New DataPoint( _
                                                row.REG_MILE, _
                                                row.RSLT_VAL)
            'データポイントを追加 
            chart.Series(ConsMyLineChart).Points.Add(dataPoint)

            '注釈の設定
            Using ca As CalloutAnnotation = New CalloutAnnotation
                ca.Visible = True
                ca.SmartLabelStyle.Enabled = True
                ca.SmartLabelStyle.IsMarkerOverlappingAllowed = False   '注釈が重ならないようにする
                ca.BackColor = Color.Ivory                  '注釈の中の色
                ca.ShadowColor = Color.Gray                 '注釈の影の色
                ca.ShadowOffset = 3                         '注釈の影のサイズ
                ca.CalloutStyle = CalloutStyle.Rectangle    '注釈の形状：吹き出し型
                ca.AnchorOffsetX = 2                        'アンカーポイント⇒注釈のサイズX値
                ca.AnchorOffsetY = 5                        'アンカーポイント⇒注釈のサイズY値
                ca.AnchorAlignment = ContentAlignment.BottomCenter 'アンカーポイントに対する注釈の位置
                ca.Height = 8
                ca.Alignment = ContentAlignment.MiddleCenter
                ca.Font = New Font("メイリオ", 8)
                ca.Text = Format(row.INSPECTION_APPROVAL_DATETIME, ConsDateFormat)    '引数.日付(注釈に表示する文字列)

                'ポイント情報のセット
                ca.AnchorDataPoint = dataPoint                'データポイント
                ca.AnchorDataPoint.MarkerStyle = markerStyle  '引数.ポイントの形状(円形,星型等)
                'ポイントの形状によりマーカーサイズ及び色をセット
                Select Case markerStyle
                    Case DataVisualization.Charting.MarkerStyle.Star5
                        '★
                        ca.AnchorDataPoint.MarkerSize = 16
                        ca.AnchorDataPoint.MarkerColor = Color.DarkOrange
                    Case DataVisualization.Charting.MarkerStyle.Circle
                        '●
                        ca.AnchorDataPoint.MarkerSize = 8
                        ca.AnchorDataPoint.MarkerColor = Color.LimeGreen
                    Case Else
                        'この処理で上記以外のケースはないが●と同じ設定を記載
                        ca.AnchorDataPoint.MarkerSize = 8
                        ca.AnchorDataPoint.MarkerColor = Color.LimeGreen
                End Select

                'グラフに注釈を追加
                chart.Annotations.Add(ca)
            End Using
        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} {2}" _
            , Me.GetType.ToString _
            , methodName _
            , ConsLogEnd))

    End Sub

    'Protected Sub MyChart_Disposed(sender As Object, e As System.EventArgs) Handles _
    '    MyChartFL.Disposed, MyChartFR.Disposed, MyChartRL.Disposed, MyChartRR.Disposed, MyLargeChart.Disposed
    '    Logger.Error(String.Format(CultureInfo.CurrentCulture _
    '        , "{0}.{1} {2}" _
    '        , Me.GetType.ToString _
    '        , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '        , "イベント発生"))
    'End Sub

    'Protected Sub MyChart_Init(sender As Object, e As System.EventArgs) Handles _
    '    MyChartFL.Init, MyChartFR.Init, MyChartRL.Init, MyChartRR.Init, MyLargeChart.Init
    '    Logger.Error(String.Format(CultureInfo.CurrentCulture _
    '        , "{0}.{1} {2}" _
    '        , Me.GetType.ToString _
    '        , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '        , "イベント発生"))
    'End Sub

    'Protected Sub MyChart_Load(sender As Object, e As System.EventArgs) Handles _
    '    MyChartFL.Load, MyChartFR.Load, MyChartRL.Load, MyChartRR.Load, MyLargeChart.Load
    '    Logger.Error(String.Format(CultureInfo.CurrentCulture _
    '        , "{0}.{1} {2}" _
    '        , Me.GetType.ToString _
    '        , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '        , "イベント発生"))
    'End Sub

    'Protected Sub MyChart_PostPaint(sender As Object, e As System.Web.UI.DataVisualization.Charting.ChartPaintEventArgs) Handles _
    '    MyChartFL.PostPaint, MyChartFR.PostPaint, MyChartRL.PostPaint, MyChartRR.PostPaint, MyLargeChart.PostPaint
    '    Logger.Error(String.Format(CultureInfo.CurrentCulture _
    '        , "{0}.{1} {2}" _
    '        , Me.GetType.ToString _
    '        , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '        , "イベント発生"))
    'End Sub

    'Protected Sub MyChart_PreRender(sender As Object, e As System.EventArgs) Handles _
    '    MyChartFL.PreRender, MyChartFR.PreRender, MyChartRL.PreRender, MyChartRR.PreRender, MyLargeChart.PreRender
    '    Logger.Error(String.Format(CultureInfo.CurrentCulture _
    '        , "{0}.{1} {2}" _
    '        , Me.GetType.ToString _
    '        , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '        , "イベント発生"))
    'End Sub

    'Protected Sub MyChart_Unload(sender As Object, e As System.EventArgs) Handles _
    '    MyChartFL.Unload, MyChartFR.Unload, MyChartRL.Unload, MyChartRR.Unload, MyLargeChart.Unload
    '    Logger.Error(String.Format(CultureInfo.CurrentCulture _
    '        , "{0}.{1} {2}" _
    '        , Me.GetType.ToString _
    '        , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '        , "イベント発生"))
    'End Sub

    '水平線の描画(MyChartFL/PrePaint=グラフ要素の背景を描画した後に発生するイベント)
    Private Sub MyChart_PrePaint(ByVal sender As Object, ByVal e As ChartPaintEventArgs) Handles _
        MyChartFL.PrePaint, MyChartFR.PrePaint, MyChartRL.PrePaint, MyChartRR.PrePaint, MyLargeChart.PrePaint

        'メソッド名取得
        Dim methodName As String = System.Reflection.MethodBase.GetCurrentMethod.Name

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} {2}" _
            , Me.GetType.ToString _
            , methodName _
            , ConsLogStart))

        '水平線の描画を行う
        If TypeOf e.ChartElement Is ChartArea Then
            Dim area As ChartArea = DirectCast(e.ChartElement, ChartArea)
            Dim x1 As Single = CSng(area.AxisX.ValueToPixelPosition(area.AxisX.Minimum))      '始点をセット
            Dim x2 As Single = CSng(area.AxisX.ValueToPixelPosition(area.AxisX.Maximum))      '終点をセット
            Dim y As Single = CSng(area.AxisY.ValueToPixelPosition(GraphRecommendReplaceVal)) '閾値(訴求用グラフ設定マスタ.推奨交換値)をセット
            'ペンの設定
            Using MyPen As Pen = New Pen(Color.DeepPink)  '色
                MyPen.Width = 2                             '太さ
                MyPen.DashStyle = Drawing2D.DashStyle.Dash  '線種(破線)
                '描画
                e.ChartGraphics.Graphics.DrawLine(MyPen, x1, y, x2, y)
            End Using
            'X軸のMIN値のテキストはここで消去する
            For Each cl As CustomLabel In area.AxisX.CustomLabels
                If cl.Text.StartsWith(GraphPartsMinVal.ToString) OrElse
                   cl.Text.StartsWith(Me.SettingGraphDispMaxMile.ToString) Then
                    cl.Text = ""
                End If
            Next
            'Y軸のMIN値及びMAX値のテキストはここで消去する
            For Each cl As CustomLabel In area.AxisY.CustomLabels
                If cl.Text.StartsWith(GraphPartsMinVal.ToString) OrElse
                   cl.Text.StartsWith(GraphPartsMaxVal.ToString) Then
                    cl.Text = ""
                End If
            Next
        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} {2}" _
            , Me.GetType.ToString _
            , methodName _
            , ConsLogEnd))
    End Sub




    ''水平線の描画(MyChartFL/PrePaint=グラフ要素の背景を描画した後に発生するイベント)
    'Private Sub MyChartFL_PrePaint(ByVal sender As Object, ByVal e As ChartPaintEventArgs) Handles MyChartFL.PrePaint
    '    Call MyChart_PrePaint(sender, e)
    'End Sub

    ''水平線の描画(PrePaint=グラフ要素の背景を描画した後に発生するイベント)
    'Private Sub MyChartFR_PrePaint(ByVal sender As Object, ByVal e As ChartPaintEventArgs) Handles MyChartFR.PrePaint
    '    Call MyChart_PrePaint(sender, e)
    'End Sub

    ''水平線の描画(MyChartFR/PrePaint=グラフ要素の背景を描画した後に発生するイベント)
    'Private Sub MyChartRL_PrePaint(ByVal sender As Object, ByVal e As ChartPaintEventArgs) Handles MyChartRL.PrePaint
    '    Call MyChart_PrePaint(sender, e)
    'End Sub

    ''水平線の描画(MyChartFR/PrePaint=グラフ要素の背景を描画した後に発生するイベント)
    'Private Sub MyChartRR_PrePaint(ByVal sender As Object, ByVal e As ChartPaintEventArgs) Handles MyChartRR.PrePaint
    '    Call MyChart_PrePaint(sender, e)
    'End Sub

    ''水平線の描画(MyLargeChart/PrePaint=グラフ要素の背景を描画した後に発生するイベント)
    'Private Sub MyLargeChart_PrePaint(ByVal sender As Object, ByVal e As ChartPaintEventArgs) Handles MyLargeChart.PrePaint
    '    Call MyChart_PrePaint(sender, e)
    'End Sub

    ' ''' <summary>
    ' ''' 水平線の描画(PrePaint=グラフ要素の背景を描画した後に発生するイベント)
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Sub MyChart_PrePaint(ByVal sender As Object, _
    '                             ByVal e As ChartPaintEventArgs)

    '    'メソッド名取得
    '    Dim methodName As String = System.Reflection.MethodBase.GetCurrentMethod.Name

    '    Logger.Error(String.Format(CultureInfo.CurrentCulture _
    '        , "{0}.{1} {2}" _
    '        , Me.GetType.ToString _
    '        , methodName _
    '        , ConsLogStart))

    '    '水平線の描画を行う
    '    If TypeOf e.ChartElement Is ChartArea Then
    '        Dim area As ChartArea = DirectCast(e.ChartElement, ChartArea)
    '        Dim x1 As Single = CSng(area.AxisX.ValueToPixelPosition(area.AxisX.Minimum))      '始点をセット
    '        Dim x2 As Single = CSng(area.AxisX.ValueToPixelPosition(area.AxisX.Maximum))      '終点をセット
    '        Dim y As Single = CSng(area.AxisY.ValueToPixelPosition(GraphRecommendReplaceVal)) '閾値(訴求用グラフ設定マスタ.推奨交換値)をセット
    '        'ペンの設定
    '        Using MyPen As Pen = New Pen(Color.DeepPink)  '色
    '            MyPen.Width = 2                             '太さ
    '            MyPen.DashStyle = Drawing2D.DashStyle.Dash  '線種(破線)
    '            '描画
    '            e.ChartGraphics.Graphics.DrawLine(MyPen, x1, y, x2, y)
    '        End Using
    '        'X軸のMIN値のテキストはここで消去する
    '        For Each cl As CustomLabel In area.AxisX.CustomLabels
    '            If cl.Text.StartsWith(GraphPartsMinVal.ToString) OrElse
    '               cl.Text.StartsWith(Me.SettingGraphDispMaxMile.ToString) Then
    '                cl.Text = ""
    '            End If
    '        Next
    '        'Y軸のMIN値及びMAX値のテキストはここで消去する
    '        For Each cl As CustomLabel In area.AxisY.CustomLabels
    '            If cl.Text.StartsWith(GraphPartsMinVal.ToString) OrElse
    '               cl.Text.StartsWith(GraphPartsMaxVal.ToString) Then
    '                cl.Text = ""
    '            End If
    '        Next
    '    End If

    '    Logger.Error(String.Format(CultureInfo.CurrentCulture _
    '        , "{0}.{1} {2}" _
    '        , Me.GetType.ToString _
    '        , methodName _
    '        , ConsLogEnd))

    'End Sub

    ''' <summary>
    ''' 閾値と重なる距離をもとにX軸のMAX値及びWidth値を求める
    ''' </summary>
    ''' <param name="chart">表示先グラフ</param>
    ''' <param name="mile">閾値と重なる走行距離</param>
    ''' <remarks></remarks>
    Private Sub SetMaxValueByAxisXAndWidth(ByVal chart As Chart, ByVal mile As Double)

        'メソッド名取得
        Dim methodName As String = System.Reflection.MethodBase.GetCurrentMethod.Name
        Try
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} {2}" _
                , Me.GetType.ToString _
                , methodName _
                , ConsLogStart))

            '補助線目盛り間隔をもとにMAX値を求める
            '(MAX走行距離 / 補助線目盛り間隔)の小数点以下切り上げ * 補助線目盛り間隔
            With chart.ChartAreas(0)

                Logger.Info("DEBUG:mile = " & mile.ToString)
                Logger.Info("DEBUG:.AxisX.MinorGrid.Interval = " & .AxisX.MinorGrid.Interval.ToString)

                If mile > Me.SettingGraphDispMaxMile Then
                    .AxisX.Maximum = Me.SettingGraphDispMaxMile
                Else
                    '距離／間隔の結果を小数点以下切り上げてMAX値を算出する
                    Dim maxValue As Double = Math.Ceiling(mile / .AxisX.MinorGrid.Interval) * .AxisX.MinorGrid.Interval
                    '算出MAX値が最低MAX値に満たなかったときは最低ラインMAX値をセットする
                    If maxValue < Me.SettingGraphDispLowestMaxMile Then
                        maxValue = Me.SettingGraphDispLowestMaxMile
                    End If
                    .AxisX.Maximum = maxValue
                    Logger.Info("DEBUG:.AxisX.Maximum = " & .AxisX.Maximum.ToString)
                End If

                '2014/08/20　グラフサイズ設定　START　↓↓↓
                'グラフ左側（Y軸の数値部分）の余白幅(px)
                Dim ChartMarginLeft As Integer = 50
                'グラフ右側の余白幅(px)
                Dim ChartMarginRight As Integer = 15
                'グラフ上側の余白幅(px)
                Dim ChartMarginTop As Integer = 14
                'グラフ下側の余白幅（X軸の数値部分）(px)
                Dim ChartMarginBottom As Integer = 29

                '1補助線辺りの幅(px)
                Dim ChartMemWidth As Integer = 0
                If chart.ID.Equals(ConsMyLargeChart) Then
                    ChartMemWidth = 40
                Else
                    ChartMemWidth = 30
                End If

                Logger.Info("DEBUG:.AxisX.Maximum = " & .AxisX.Maximum.ToString)
                Logger.Info("DEBUG:.AxisX.MinorGrid.Interval = " & .AxisX.MinorGrid.Interval.ToString)
                Logger.Info("DEBUG:ChartMemWidth = " & ChartMemWidth.ToString)
                Logger.Info("DEBUG:ChartMarginLeft = " & ChartMarginLeft.ToString)
                Logger.Info("DEBUG:ChartMarginRight = " & ChartMarginRight.ToString)
                Logger.Info("DEBUG:ChartMarginTop = " & ChartMarginTop.ToString)
                Logger.Info("DEBUG:ChartMarginBottom = " & ChartMarginBottom.ToString)

                'グラフ全体の幅（ 軸の最大値 / グリッド間隔 * グリッドの幅(px) + 左側余白(px) + 右側余白(px) ）
                Dim ChartWidth As Integer = Integer.Parse((.AxisX.Maximum / .AxisX.MinorGrid.Interval * ChartMemWidth + ChartMarginLeft + ChartMarginRight).ToString("0")) - 5
                'グラフ全体の高さ
                Dim ChartHeight As Integer = Integer.Parse(chart.Height.Value.ToString)
                'グラフ全体の幅設定
                chart.Width = Unit.Pixel(ChartWidth)

                Logger.Info("DEBUG:ChartWidth = " & ChartWidth.ToString)
                Logger.Info("DEBUG:ChartHeight = " & ChartHeight.ToString)

                'グラフエリア内部プロットの配置設定
                With .InnerPlotPosition
                    '手動設定のため自動配置設定をFalseとする
                    .Auto = False
                    '内部プロットの幅（　( グラフ全体幅 - 左右余白 ) / グラフ全体幅 * 100　）
                    .Width = Single.Parse(((ChartWidth - (ChartMarginLeft + ChartMarginRight)) / ChartWidth * 100).ToString("0.00"))
                    '内部プロットの高さ（　( グラフ全体高さ - 上下余白 ) / グラフ全体高さ * 100　）
                    .Height = Single.Parse(((ChartHeight - (ChartMarginTop + ChartMarginBottom)) / ChartHeight * 100).ToString("0.00"))
                    '内部プロットの左上座標(X)（　左側余白 / グラフ全体幅 * 100　）
                    .X = Single.Parse((ChartMarginLeft / ChartWidth * 100).ToString("0.00"))
                    '内部プロットの左上座標(Y)（　上側余白 / グラフ全体幅 * 100　）
                    .Y = Single.Parse((ChartMarginTop / ChartHeight * 100).ToString("0.00"))

                    Logger.Info("DEBUG:.InnerPlotPosition.Width = " & .Width.ToString)
                    Logger.Info("DEBUG:.InnerPlotPosition.Height = " & .Height.ToString)
                    Logger.Info("DEBUG:.InnerPlotPosition.X = " & .X.ToString)
                    Logger.Info("DEBUG:.InnerPlotPosition.Y = " & .Y.ToString)
                End With
                '2014/08/20　グラフサイズ設定　END　　↑↑↑
            End With

        Catch ex As Exception
            'エラー時
            Logger.Error(String.Format(CultureInfo.CurrentCulture _
                         , "{0}.{1} OUT:ErrSetMaxValueByAxisXAndWidth = {2}" _
                         , Me.GetType.ToString _
                         , methodName _
                         , ex.Message))
            'グラフ幅に固定値をセットする
            chart.Width = 540
            '手動設定のため自動配置設定をTrueとする
            chart.ChartAreas(0).InnerPlotPosition.Auto = True

        Finally
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} {2}" _
                , Me.GetType.ToString _
                , methodName _
                , ConsLogEnd))
        End Try

    End Sub

    ''' <summary>
    ''' 拡大画面を表示する
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ShowPopUpWindow(ByVal upsellChartSettingData As SC3250106DataSet.UpsellChartSettingDataTable)

        'メソッド名取得
        Dim methodName As String = System.Reflection.MethodBase.GetCurrentMethod.Name

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} {2}" _
            , Me.GetType.ToString _
            , methodName _
            , ConsLogStart))


        'どのチャートがタップされたか取得する
        Dim ChartNo As String = hdnSelectChart.Value

        '拡大画面を表示する
        '黒色半透明画面
        contentsMainonBoard.Style.Add("display", "block")
        '拡大画面を表示する
        popUpWindow.Style.Add("display", "block")

        '閉じるボタンを表示
        closeBtn.Style.Add("display", "block")

        'グラフ描画
        Call CraeteChartProc(upsellChartSettingData, DirectCast(Integer.Parse(ChartNo), Enum_GraphDspOrder), Enum_GraphArea.Large)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} {2}" _
            , Me.GetType.ToString _
            , methodName _
            , ConsLogEnd))

    End Sub
#End Region

End Class
