'------------------------------------------------------------------------------
'SC3150102.aspx.vb
'------------------------------------------------------------------------------
'機能：TCメインメニュー_R/O情報タブ
'補足：
'作成：2012/01/30 KN 渡辺
'更新：
'------------------------------------------------------------------------------
Option Strict On
Option Explicit On

Imports System
Imports System.Data
Imports System.Web.Script.Serialization
Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.iCROP.BizLogic.SC3150102
Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.DataAccess.IC3801001
Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.DataAccess.IC3801004
Imports Toyota.eCRB.DMSLinkage.RepairOrderCreate.DataAccess.IC3801110
Imports Toyota.eCRB.DMSLinkage.RepairOrderCreate.DataAccess.IC3801113

Partial Class Pages_SC3150102
    Inherits BasePage

#Region "定数"

    ''' <summary>
    ''' 空白文字
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STRING_SPACE As String = ""
    ''' <summary>
    ''' 工数の初期値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const INIT_WORK_HOURS As Integer = 0
    ''' <summary>
    ''' 単価の初期値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const INIT_SELL_WORK_PRICE As Integer = 0

    ''' <summary>
    ''' 部品数量の初期値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const INIT_PARTS_QUANTITY As Integer = 0

    ''' <summary>
    ''' 基本情報・初期状態・燃料：空
    ''' </summary>
    ''' <remarks></remarks>
    Private Const BASIC_FUEL_EMPTY As String = "0"
    ''' <summary>
    ''' 基本情報・初期状態・燃料：4分の1
    ''' </summary>
    ''' <remarks></remarks>
    Private Const BASIC_FUEL_QUARTER As String = "1"
    ''' <summary>
    ''' 基本情報・初期状態・燃料：4分の2
    ''' </summary>
    ''' <remarks></remarks>
    Private Const BASIC_FUEL_HALF As String = "2"
    ''' <summary>
    ''' 基本情報・初期状態・燃料：4分の3
    ''' </summary>
    ''' <remarks></remarks>
    Private Const BASIC_FUEL_THREE_QUARTER As String = "3"
    ''' <summary>
    ''' 基本情報・初期状態・燃料：満タン
    ''' </summary>
    ''' <remarks></remarks>
    Private Const BASIC_FUEL_FULL As String = "4"

    ''' <summary>
    ''' 基本情報・初期状態・オーディオ：オフ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const BASIC_AUDIO_OFF As String = "0"
    ''' <summary>
    ''' 基本情報・初期状態・オーディオ：CD
    ''' </summary>
    ''' <remarks></remarks>
    Private Const BASIC_AUDIO_CD As String = "1"
    ''' <summary>
    ''' 基本情報・初期状態・オーディオ：FM
    ''' </summary>
    ''' <remarks></remarks>
    Private Const BASIC_AUDIO_FM As String = "2"

    ''' <summary>
    ''' 基本情報・初期状態・エアコン：OFF
    ''' </summary>
    ''' <remarks></remarks>
    Private Const BASIC_AIR_CONDITIONER_OFF As String = "0"
    ''' <summary>
    ''' 基本情報・初期状態・エアコン：ON
    ''' </summary>
    ''' <remarks></remarks>
    Private Const BASIC_AIR_CONDITIONER_ON As String = "1"

    ''' <summary>
    ''' 基本情報・初期状態・付属品：チェック
    ''' </summary>
    ''' <remarks></remarks>
    Private Const BASIC_ACCESSORY_CHECKED As String = "1"

    ''' <summary>
    ''' ご用命事項・確認事項・交換部品：持帰り
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_EXCHANGE_PARTS_TAKEOUT As String = "0"
    ''' <summary>
    ''' ご用命事項・確認事項・交換部品：保険提出
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_EXCHANGE_PARTS_INSURANCE As String = "1"
    ''' <summary>
    ''' ご用命事項・確認事項・交換部品：店内処分
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_EXCHANGE_PARTS_DISPOSE As String = "2"

    ''' <summary>
    ''' ご用命事項・確認事項・待ち方：店内
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_WAITING_IN As String = "0"
    ''' <summary>
    ''' ご用命事項・確認事項・待ち方：店外
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_WAITING_OUT As String = "1"

    ''' <summary>
    ''' ご用命事項・確認事項・洗車：する
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_WASHING_DO As String = "1"
    ''' <summary>
    ''' ご用命事項・確認事項・洗車：しない
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_WASHING_NONE As String = "0"

    ''' <summary>
    ''' ご用命事項・確認事項・支払い方法：現金
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_PAYMENT_CASH As String = "0"
    ''' <summary>
    ''' ご用命事項・確認事項・支払い方法：カード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_PAYMENT_CARD As String = "1"
    ''' <summary>
    ''' ご用命事項・確認事項・支払い方法：その他
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_PAYMENT_OTHER As String = "2"
    ''' <summary>
    ''' ご用命事項・確認事項・CSI時間：午前
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_CSI_AM As String = "1"
    ''' <summary>
    ''' ご用命事項・確認事項・CSI時間：午後
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_CSI_PM As String = "2"
    ''' <summary>
    ''' ご用命事項・確認事項・CSI時間：指定なし
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_CSI_ALWAYS As String = "0"

    ''' <summary>
    ''' ご用命事項・問診項目・WNG：常時点灯
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_WNG_ALWAYS As String = "1"
    ''' <summary>
    ''' ご用命事項・問診項目・WNG：頻繁に点灯
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_WNG_OFTEN As String = "2"
    ''' <summary>
    ''' ご用命事項・問診項目・WNG：表示なし
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_WNG_NONE As String = "0"

    ''' <summary>
    ''' ご用命事項・問診項目・故障発生時間：最近
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_OCCURRENCE_RECENTLY As String = "0"
    ''' <summary>
    ''' ご用命事項・問診項目・故障派生時間：一週間前
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_OCCURRENCE_WEEK As String = "1"
    ''' <summary>
    ''' ご用命事項・問診項目・故障発生時間：その他
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_OCCURRENCE_OTHER As String = "2"

    ''' <summary>
    ''' ご用命事項・問診項目・故障発生頻度：頻繁に
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_FREQUENCY_HIGH As String = "0"
    ''' <summary>
    ''' ご用命事項・問診項目・故障発生頻度：時々
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_FREQUENCY_OFTEN As String = "1"
    ''' <summary>
    ''' ご用命事項・問診項目・故障発生頻度：一回だけ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_FREQUENCY_ONCE As String = "2"

    ''' <summary>
    ''' ご用命事項・問診項目・再現可能：はい
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_REAPPEAR_YES As String = "1"
    ''' <summary>
    ''' ご用命事項・問診項目・再現項目：いいえ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_REAPPEAR_NO As String = "0"

    ''' <summary>
    ''' ご用命事項・問診項目・水温：冷
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_WATERT_LOW As String = "0"
    ''' <summary>
    ''' ご用命事項・問診項目・水温：熱
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_WATERT_HIGH As String = "1"

    ''' <summary>
    ''' ご用命事項・問診項目・気温：寒
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_TEMPERATURE_LOW As String = "0"
    ''' <summary>
    ''' ご用命事項・問診項目・気温：暑
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_TEMPERATURE_HIGH As String = "1"

    ''' <summary>
    ''' ご用命事項・問診項目・発生場所：駐車場
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_PLACE_PARKING As String = "0"
    ''' <summary>
    ''' ご用命事項・問診項目・発生場所：一般道路
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_PLACE_ORDINARY As String = "1"
    ''' <summary>
    ''' ご用命事項・問診項目・発生場所：高速道路
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_PLACE_MOTORWAY As String = "2"
    ''' <summary>
    ''' ご用命事項・問診項目・発生場所：坂道
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_PLACE_SLOPE As String = "3"

    ''' <summary>
    ''' ご用命事項・問診項目・渋滞状況：あり
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_TRAFFICJAM_HAPPEN As String = "1"
    ''' <summary>
    ''' ご用命事項・問診項目・渋滞状況：なし
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_TRAFFICJAM_NONE As String = "0"

    ''' <summary>
    ''' ご用命事項・問診項目・車両状態：オン
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_CARSTATUS_ON As String = "1"
    ''' <summary>
    ''' ご用命事項・問診項目・車両状態：オフ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_CARSTATUS_OFF As String = "0"
    ' ''' <summary>
    ' ''' ご用命事項・問診項目・車両状態：起動時
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const ORDER_CARSTATUS_STARTUP As Integer = 1
    ' ''' <summary>
    ' ''' ご用命事項・問診項目・車両状態：アイドル時
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const ORDER_CARSTATUS_IDLLING As Integer = 2
    ' ''' <summary>
    ' ''' ご用命事項・問診項目・車両状態：冷間時
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const ORDER_CARSTATUS_COLD As Integer = 3
    ' ''' <summary>
    ' ''' ご用命事項・問診項目・車両状態：温間時
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const ORDER_CARSTATUS_WARM As Integer = 4

    ''' <summary>
    ''' ご用命事項・問診項目・走行時：穏速
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_TRAVELING_LOWSPEED As String = "0"
    ''' <summary>
    ''' ご用命事項・問診項目・走行時：加速
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_TRAVELING_ACCELERATION As String = "1"
    ''' <summary>
    ''' ご用命事項・問診項目・走行時：減速
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_TRAVELING_SLOWDOWN As String = "2"

    ' ''' <summary>
    ' ''' ご用命事項・問診項目・車両状態：駐車時
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const ORDER_CARCONTROL1_PARKING As Integer = 1
    ' ''' <summary>
    ' ''' ご用命事項・問診項目・車両状態：前進時
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const ORDER_CARCONTROL1_ADVANCE As Integer = 2
    ' ''' <summary>
    ' ''' ご用命事項・問診項目・車両状態：変速時
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const ORDER_CARCONTROL1_SHIFTCHANGE As Integer = 3

    ' ''' <summary>
    ' ''' ご用命事項・問診項目・車両状態：後退時
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const ORDER_CARCONTROL2_BACK As Integer = 1
    ' ''' <summary>
    ' ''' ご用命事項・問診項目・車両状態：ブレーキ時
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const ORDER_CARCONTROL2_BRAKE As Integer = 2
    ' ''' <summary>
    ' ''' ご用命事項・問診項目・車両状態：迂回時
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const ORDER_CARCONTROL2_DETOUR As Integer = 3

    ''' <summary>
    ''' ご用命事項・問診項目・非純正用品：あり
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_NONGENUINE_YES As String = "1"
    ''' <summary>
    ''' ご用命事項・問診項目・非純正用品：なし
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_NONGENUINE_NO As String = "0"

    ''' <summary>
    ''' 部品準備完了フラグ：準備完了していない
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PARTS_REPARE_UNPREPARED As String = "0"
    ''' <summary>
    ''' 部品準備完了フラグ：準備完了
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PARTS_REPARE_PREPARED As String = "1"

    ''' <summary>
    ''' R/O情報欄のフィルタフラグ：フィルタをかけない
    ''' </summary>
    ''' <remarks></remarks>
    Private REPAIR_ORDER_FILTER_OFF As String = "0"

    ''' <summary>
    ''' 部品項目で、B/Oを表現する文字列
    ''' </summary>
    ''' <remarks></remarks>
    Private PARTS_BACK_ORDER_STRING As String = "B/O"

#End Region

#Region "変数定義"

    ''' <summary>
    ''' ユーザ情報（セッションより）
    ''' </summary>
    ''' <remarks></remarks>
    Private accountStaffContext As StaffContext
    ''' <summary>
    ''' オーダーNo
    ''' </summary>
    ''' <remarks></remarks>
    Private repairOrderNo As String
    ''' <summary>
    ''' 子番号
    ''' </summary>
    ''' <remarks></remarks>
    Private childNumber As String
    ''' <summary>
    ''' ビジネスロジック
    ''' </summary>
    ''' <remarks></remarks>
    Private businessLogic As New SC3150102BusinessLogic

    Private repairOrderDataTable As IC3801001DataSet.IC3801001OrderCommDataTable
    Private repairOrderData As IC3801001DataSet.IC3801001OrderCommRow

#End Region

#Region "初期処理"
    ''' <summary>
    ''' ページロード時の処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Logger.Info("Page_Load Start")
        'LabelNow.Text = DateTime.Now.ToString()

        'ページ内変数の初期化（セッション情報の格納）.
        SetInitVariable()

        'R/Oの追加作業アイコンの親オーダー時の文字を格納する.
        Me.HiddenFieldRepairOrderInitialWord.Value = WebWordUtility.GetWord(316)

        'R/O基本情報の取得.
        Me.repairOrderDataTable = Nothing
        Logger.Info("Page_Load OrderNumber:" + Me.repairOrderNo)
        'If (Me.repairOrderNo <> "") Then
        If (Not String.IsNullOrEmpty(Me.repairOrderNo)) Then
            Logger.Info("Page_Load Not IsNullOrEmpty OrderNumber")
            Me.repairOrderDataTable = Me.businessLogic.GetRepairOrderBaseData(Me.accountStaffContext.DlrCD, Me.repairOrderNo)
        End If

        Me.repairOrderData = Nothing
        If (Not IsNothing(Me.repairOrderDataTable)) Then
            Logger.Info("Page_Load Not IsNothing repairOrderDataTable")
            Logger.Info("Page_Load repairOrderDataTable.Rows.Count:" + CType(repairOrderDataTable.Rows.Count, String))

            If (Me.repairOrderDataTable.Rows.Count > 0) Then
                Logger.Info("Page_Load repairOrderDataTable.Rows.Count > 0")
                Me.repairOrderData = CType(Me.repairOrderDataTable.Rows(0), IC3801001DataSet.IC3801001OrderCommRow)
            End If
        End If

        'R/O基本情報を取得できていない場合に備え、各項目に初期値を設定する.
        Me.HiddenFieldPartsReady.Value = PARTS_REPARE_UNPREPARED
        Me.HiddenFieldOrderStatus.Value = "0"
        Me.HiddenFieldAddWorkCount.Value = "0"
        Me.HiddenFieldSAName.Value = ""

        'R/O基本情報を取得できている場合のみ、各項目への情報格納処理を実施する.
        If (Not IsNothing(Me.repairOrderData)) Then
            Logger.Info("Page_Load Start Not IsNothing(Me.repairOrderData)")

            '部品準備完了情報を設定
            If (Not Me.repairOrderData.IspartsRepareFlgNull) Then
                Logger.Info("Page_Load partsRepareFlg is not DBNull")
                Me.HiddenFieldPartsReady.Value = Me.repairOrderData.partsRepareFlg
            End If

            'R/O作業ステータス情報を設定
            If (Not Me.repairOrderData.IsOrderStatusNull) Then
                Logger.Info("Page_Load OrderStatus is not DBNull")
                Me.HiddenFieldOrderStatus.Value = Me.repairOrderData.OrderStatus
            End If

            '追加作業件数を設定
            If (Not Me.repairOrderData.IsaddSrvCountNull) Then
                Logger.Info("Page_Load addSrvCount is not DBNull")
                Me.HiddenFieldAddWorkCount.Value = CType(Me.repairOrderData.addSrvCount, String)
            End If

            'SA名をHiddenに格納する.
            If (Not Me.repairOrderData.IsOrderSaNameNull) Then
                Logger.Info("Page_Load OrderSaName is not DBNull")
                Me.HiddenFieldSAName.Value = Me.repairOrderData.OrderSaName
            End If

            '基本情報パネルのデータを表示.
            SetBasicInfoData()
            'ご用命事項パネルのデータを表示.
            SetOrdersInfoData()
            '基本情報パネルの顧客情報の設定.
            SetBasicInfo()
        End If
        Logger.Info("Page_Load Start HiddenFieldPartsReady:" + Me.HiddenFieldPartsReady.Value)
        Logger.Info("Page_Load Start HiddenFieldOrderStatus:" + Me.HiddenFieldOrderStatus.Value)
        Logger.Info("Page_Load Start HiddenFieldAddWorkCount:" + Me.HiddenFieldAddWorkCount.Value)
        Logger.Info("Page_Load Start HiddenFieldSAName:" + Me.HiddenFieldSAName.Value)

        '履歴情報の設定.
        SetHistoryInfo()
        '作業内容パネルの作業内容の設定.
        SetWorkInfo()
        '作業内容パネルの部品項目の設定.
        SetPartsInfo()

        Logger.Info("Page_Load End")

    End Sub


    ''' <summary>
    ''' ページ内変数の初期設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetInitVariable()

        Logger.Info("SetInitVariable Start")

        'ユーザ情報の取得
        Me.accountStaffContext = StaffContext.Current

        'ビジネスロジックを新規作成
        Me.businessLogic = New SC3150102BusinessLogic

        '各情報の初期化
        Me.repairOrderNo = ""
        Me.Hidden01Box03Filter.Value = REPAIR_ORDER_FILTER_OFF
        Me.childNumber = "0"

        'セッション情報の取得処理.
        If (MyBase.ContainsKey(ScreenPos.Current, "Redirect.ORDERNO")) Then
            Logger.Info("Page_Load if roid")
            'オーダー番号を取得する
            Me.repairOrderNo = MyBase.GetValue(ScreenPos.Current, "Redirect.ORDERNO", False).ToString().Trim()
        End If

        '子番号を取得する.
        If (MyBase.ContainsKey(ScreenPos.Current, "Redirect.SRVADDSEQ")) Then
            Logger.Info("Page_Load if childNumber")
            Me.childNumber = MyBase.GetValue(ScreenPos.Current, "Redirect.SRVADDSEQ", False).ToString()
        End If
        Me.HiddenFieldSelectedAddWork.Value = Me.childNumber

        'R/O情報にグレーフィルタ情報を取得する.
        If (MyBase.ContainsKey(ScreenPos.Current, "Redirect.FILTERFLG")) Then
            Logger.Info("Page_Load if filter")
            Me.Hidden01Box03Filter.Value = MyBase.GetValue(ScreenPos.Current, "Redirect.FILTERFLG", False).ToString()
        End If

        Logger.Info("SetInitVariable repairOrderNumber:" + Me.repairOrderNo)
        Logger.Info("SetInitVariable childNumber:" + Me.childNumber)
        Logger.Info("SetInitVariable R/O filter:" + Me.Hidden01Box03Filter.Value)

        Logger.Info("SetInitVariable End")

    End Sub

#End Region

#Region "データ格納処理"
    ''' <summary>
    ''' 基本情報パネルの顧客情報の設定.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetBasicInfo()

        Logger.Info("SetBasicInfo Start")

        'オーナー名の表示.
        Dim buyerName As String = ""
        If (Not repairOrderData.IsbuyerNameNull()) Then
            Logger.Info("SetBasicInfo buyerName is not DBNull")
            buyerName = repairOrderData.buyerName
        End If
        Logger.Info("SetBasicInfo buyerName:" + buyerName)
        Me.LiteralBuyerName.Text = buyerName

        '顧客名称の表示.
        Dim customerName As String = repairOrderData.OrderCustomerName
        Logger.Info("SetBasicInfo customerName:" + customerName)
        Me.LiteralOrderCustomerName.Text = customerName

        'メーカー名の表示.
        Dim makerName As String = ""
        If (Not repairOrderData.IsOrderModelNull()) Then
            Logger.Info("SetBasicInfo OrderModel is not DBNull")
            makerName = repairOrderData.OrderModel
        End If
        Logger.Info("SetBasicInfo orderModel:" + makerName)
        Me.LiteralMakerType.Text = makerName

        '車種名称の表示.
        Dim vehiclesName As String = ""
        If (Not repairOrderData.IsOrderVhcNameNull()) Then
            Logger.Info("SetBasicInfo OrderVhcName is not DBNull")
            vehiclesName = repairOrderData.OrderVhcName
        End If
        Logger.Info("SetBasicInfo vehiclesName:" + vehiclesName)
        Me.LiteralOrderVehicleName.Text = vehiclesName

        'グレードの表示.
        Dim vehiclesGrade As String = ""
        If (Not repairOrderData.IsOrderGradeNull()) Then
            Logger.Info("SetBasicInfo OrderGrade is not DBNull")
            vehiclesGrade = repairOrderData.OrderGrade
        End If
        Logger.Info("SetBasicInfo vehiclesGrade:" + vehiclesGrade)
        Me.LiteralOrderGrade.Text = vehiclesGrade

        'VINの表示.
        Dim vinNumber As String = ""
        If (Not repairOrderData.IsOrderVinNoNull()) Then
            Logger.Info("SetBasicInfo OrderVinNo is not DBNull")
            vinNumber = repairOrderData.OrderVinNo
        End If
        Logger.Info("SetBasicInfo vinNumber:" + vinNumber)
        Me.LiteralOrderVinNo.Text = vinNumber

        '車両番号の表示.
        Dim registerNumber As String = ""
        If (Not repairOrderData.IsOrderRegisterNoNull()) Then
            Logger.Info("SetBasicInfo OrderRegisterNo is not DBNull")
            registerNumber = repairOrderData.OrderRegisterNo
        End If
        Logger.Info("SetBasicInfo registerNumber:" + registerNumber)
        Me.LiteralOrderRegisterNo.Text = registerNumber

        '年式の表示.
        Dim modelValue As String = ""
        If (Not repairOrderData.IsOrderModelNull()) Then
            Logger.Info("SetBasicInfo OrderModel is not DBNull")
            modelValue = repairOrderData.OrderModel
        End If
        Logger.Info("SetBasicInfo ")
        Me.LiteralOrderModel.Text = modelValue

        '納車日の表示.
        Dim deliveryDate As String = ""
        If (Not repairOrderData.IsDeliverDateNull()) Then
            Logger.Info("SetBasicInfo DeliverDate is not DBNull")
            deliveryDate = DateTimeFunc.FormatDate(3, repairOrderData.DeliverDate)
        End If
        Logger.Info("SetBasicInfo deliveryDate=" + deliveryDate)
        Me.LiteralDeliverDate.Text = ExchangeDataToHtmlString(repairOrderData("deliverDate").ToString())

        '走行距離の表示.
        Dim mailageStringBuilder As New StringBuilder
        If (Not repairOrderData.IsOrderMileAgeNull()) Then
            Logger.Info("SetBasicInfo OrderMileAge is not DBNull")
            mailageStringBuilder.Append(CType(repairOrderData.OrderMileAge, String))
            mailageStringBuilder.Append(WebWordUtility.GetWord(109).Replace("%1", " "))
        End If
        Logger.Info("SetBasicInfo mailage:" + mailageStringBuilder.ToString())
        Me.LiteralOrderMileage.Text = mailageStringBuilder.ToString()

        Logger.Info("SetBasicInfo End")

    End Sub

    ''' <summary>
    ''' 履歴情報の設定.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetHistoryInfo()

        Logger.Info("SetHistoryInfo Start")

        '整備内容参照を取得する.
        Dim dt As IC3801004DataSet.IC3801004OderSrvDataTable
        dt = Me.businessLogic.GetHistoryData(Me.accountStaffContext.DlrCD, Me.repairOrderNo)

        '取得した整備内容がNothingでない場合、情報の設定を実施する.
        If (Not IsNothing(dt)) Then

            Logger.Info("SetHistoryInfo GetHistoryData_Count=" + CType(dt.Rows.Count, String))
            'コントロールにバインドする.
            Me.RepeaterHistoryInfo.DataSource = dt
            Me.RepeaterHistoryInfo.DataBind()

            '当日日付情報を取得する.

            'データを設定する.
            For i = 0 To RepeaterHistoryInfo.Items.Count - 1
                Logger.Info("SetHistoryInfo Repeater Roop Index=" + CType(i, String))

                Dim rInfo As Control = RepeaterHistoryInfo.Items(i)

                '受注日を表示する.
                Dim strOrderAcceptDate As String = STRING_SPACE
                If Not String.IsNullOrEmpty(Trim(CType(rInfo.FindControl("HiddenFieldHAcceptDate"), HiddenField).Value)) Then
                    Logger.Info("SetHistoryInfo Not IsNullOrEmpty AcceptDate")
                    Dim acceptDate As Date
                    If (Not Date.TryParse(CType(rInfo.FindControl("HiddenFieldHAcceptDate"), HiddenField).Value, acceptDate)) Then
                        Logger.Info("SetHistoryInfo Date Parse Faile:AcceptDate")
                        acceptDate = Date.MinValue
                    End If
                    'Dim acceptDate As Date = DateTimeFunc.FormatString("yyyy/MM/dd HH:mm:ss", CType(rInfo.FindControl("HiddenFieldHAcceptDate"), HiddenField).Value)
                    If acceptDate > Date.MinValue Then
                        Logger.Info("SetHistoryInfo acceptDate is bigger than Date.MinValue")
                        If (acceptDate.Date = Date.Today) Then
                            Logger.Info("SetHistoryInfo acceptDate is Today")
                            '受注日が当日である場合、HH:mm形式にて表示.
                            strOrderAcceptDate = DateTimeFunc.FormatDate(14, acceptDate)
                        Else
                            Logger.Info("SetHistoryInfo acceptDate is not Today")
                            '受注日が当日でない場合、MM/dd形式にて表示.
                            strOrderAcceptDate = DateTimeFunc.FormatDate(11, acceptDate)
                        End If
                        'If String.IsNullOrEmpty(strOrderAcceptDate) Then
                    Else
                        Logger.Info("SetHistoryInfo acceptDate is smaller than Date.MinValue")
                        strOrderAcceptDate = STRING_SPACE
                    End If
                End If
                Logger.Info("SetHistoryInfo AcceptDate=" + strOrderAcceptDate)
                CType(rInfo.FindControl("LiteralHAcceptDate"), Literal).Text = strOrderAcceptDate

                'オーダーNoを表示する.
                Dim stringOrderNo As String = CType(rInfo.FindControl("HiddenFieldHOrderNo"), HiddenField).Value
                If String.IsNullOrEmpty(stringOrderNo) Then
                    Logger.Info("SetHistoryInfo orderNo is NullOrEmpty")
                    stringOrderNo = STRING_SPACE
                End If
                Logger.Info("SetHistoryInfo OrderNo=" + stringOrderNo)
                CType(rInfo.FindControl("LiteralHOrderNo"), Literal).Text = stringOrderNo

                '代表整備名称を表示する.
                Dim strSrvTypeName As String = CType(rInfo.FindControl("HiddenFieldHTypicalSrvTypeName"), HiddenField).Value
                If String.IsNullOrEmpty(strSrvTypeName) Then
                    Logger.Info("SetHistoryInfo typicalSrvTypeName is NullOrEmpty")
                    strSrvTypeName = STRING_SPACE
                End If
                Logger.Info("SetHistoryInfo TypicalSrvTypeName=" + strSrvTypeName)
                CType(rInfo.FindControl("LiteralHTypicalSrvTypeName"), Literal).Text = strSrvTypeName

                '代表整備項目を表示する.
                Dim strSrvType As String = CType(rInfo.FindControl("HiddenFieldHTypicalSrvType"), HiddenField).Value
                If String.IsNullOrEmpty(strSrvType) Then
                    Logger.Info("SetHistoryInfo typicalSrvType is NullOrEmpty")
                    strSrvType = STRING_SPACE
                End If
                Logger.Info("SetHistoryInfo TypicalSrvType=" + strSrvType)
                CType(rInfo.FindControl("LiteralHTypicalSrvType"), Literal).Text = strSrvType

                '顧客名称を表示する.
                Dim strCustomerName As String = CType(rInfo.FindControl("HiddenFieldHOrderCustomerName"), HiddenField).Value
                If String.IsNullOrEmpty(strCustomerName) Then
                    Logger.Info("SetHistoryInfo customerName is NullOrEmpty")
                    strCustomerName = STRING_SPACE
                End If
                Logger.Info("SetHistoryInfo CustomerName=" + strCustomerName)
                CType(rInfo.FindControl("LiteralHCustomerName"), Literal).Text = strCustomerName
            Next

        End If
        Logger.Info("SetHistoryInfo() End")

    End Sub

    ''' <summary>
    ''' 作業内容パネルの作業内容の設定.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetWorkInfo()

        Logger.Info("SetWorkInfo Start")

        '整備内容参照を取得する.
        Dim dt As IC3801110DataSet.IC3801110SrvDetailDataTableCommDataTable
        dt = Me.businessLogic.GetServiceDetailData(Me.accountStaffContext.DlrCD, Me.repairOrderNo, Me.childNumber)
        '擬似的なデータを作成し、取得する
        'Dim dt As DataTable = CreateSampleWorkInfoDataTable()

        '取得した作業内容がNothingでない場合、情報の設定を実施する.
        If (Not IsNothing(dt)) Then
            Logger.Info("SetWorkInfo GetServiceDetailData_Count=" + CType(dt.Rows.Count, String))

            'コントロールにバインドする.
            Me.RepeaterWorkInfo.DataSource = dt
            Me.RepeaterWorkInfo.DataBind()

            'データカウンタを初期化する.
            Dim dataCount = 0
            '工数単位の文字列を取得する.
            Dim unitWork As String
            unitWork = WebWordUtility.GetWord(309)
            unitWork = unitWork.Replace("%1", " ")

            'データを設定する.
            For i = 0 To RepeaterWorkInfo.Items.Count - 1
                Logger.Info("SetWorkInfo Repeater Roop Index=" + CType(i, String))

                Dim rWorkInfo As Control = RepeaterWorkInfo.Items(i)

                'データカウンタを更新する.
                dataCount += 1
                Logger.Info("SetWorkInfo WorkNo=" + CType(dataCount, String))
                CType(rWorkInfo.FindControl("LiteralWorkNo"), Literal).Text = CType(dataCount, String)

                '作業名称を表示する.
                Dim strSrvName As String = CType(rWorkInfo.FindControl("HiddenFieldSrvName"), HiddenField).Value
                If String.IsNullOrEmpty(strSrvName) Then
                    Logger.Info("SetWorkInfo srvName is NullOrEmpty")
                    strSrvName = STRING_SPACE
                End If
                Logger.Info("SetWorkInfo SrvName=" + strSrvName)
                CType(rWorkInfo.FindControl("LiteralSrvName"), Literal).Text = strSrvName

                '工数を表示する.
                Dim strWorkHours As String = CType(rWorkInfo.FindControl("HiddenFieldWorkHours"), HiddenField).Value
                Dim dblWorkHours As Double = INIT_WORK_HOURS
                If Not String.IsNullOrEmpty(strWorkHours.ToString()) Then
                    Logger.Info("SetWorkInfo workHours is NullOrEmpty")
                    dblWorkHours = CType(strWorkHours, Double)
                End If
                Dim builderWorkHours As New System.Text.StringBuilder
                builderWorkHours.Append(dblWorkHours.ToString("0.00", CultureInfo.CurrentCulture()))
                builderWorkHours.Append(unitWork)
                Logger.Info("SetWorkInfo WorkHours=" + builderWorkHours.ToString())
                CType(rWorkInfo.FindControl("LiteralWorkHours"), Literal).Text = builderWorkHours.ToString()

                '単価を表示する
                Dim strSellWorkPrice As String = CType(rWorkInfo.FindControl("HiddenFieldSellWorkPrice"), HiddenField).Value
                Dim dblSellWorkPrice As Double = INIT_SELL_WORK_PRICE
                If Not String.IsNullOrEmpty(strSellWorkPrice) Then
                    Logger.Info("SetWorkInfo sellWorkPrice is NullOrEmpty")
                    dblSellWorkPrice = CType(strSellWorkPrice, Double)
                End If
                Logger.Info("SetWorkInfo SellWorkPrice=" + CType(dblSellWorkPrice, String))
                CType(rWorkInfo.FindControl("LiteralSellWorkPrice"), Literal).Text = dblSellWorkPrice.ToString("#,0.00", CultureInfo.CurrentCulture())

                '小計を表示する
                Dim dblSubtotal As Double
                dblSubtotal = dblWorkHours * dblSellWorkPrice
                Logger.Info("SetWorkInfo Subtotal=" + CType(dblSubtotal, String))
                CType(rWorkInfo.FindControl("LiteralSubtotal"), Literal).Text = dblSubtotal.ToString("#,0.00", CultureInfo.CurrentCulture())
            Next
        End If

        Logger.Info("SetWorkInfo End")

    End Sub


    ''' <summary>
    ''' 作業内容パネルの部品項目の設定.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetPartsInfo()

        Logger.Info("SetPartsInfo Start")

        '部品明細数を初期化する.
        Me.HiddenFieldPartsCount.Value = "0"
        '部品明細のB/O数を初期化する.
        Me.HiddenFieldPartsBackOrderCount.Value = "0"

        '部品明細参照を取得する.
        Dim dt As IC3801113DataSet.IC3801113PartsDataTable
        dt = Me.businessLogic.GetPartsDetailData(Me.accountStaffContext.DlrCD, Me.repairOrderNo, Me.childNumber)
        '擬似的なデータを作成し、取得する
        'Dim dt As DataTable = CreateSamplePartsInfoDataTable()

        '取得した部品明細がNothingでない場合、情報の設定を実施する.
        If (Not IsNothing(dt)) Then
            Logger.Info("SetPartsInfo GetPartsDetailData_Count=" + CType(dt.Rows.Count, String))

            'コントロールにバインドする.
            Me.RepeaterPartsInfo.DataSource = dt
            Me.RepeaterPartsInfo.DataBind()

            'データカウンタを初期化する.
            Dim dataCount = 0
            'B/Oカウンタを初期化する.
            Dim backOrderCount = 0
            '部品明細数を格納する.
            Me.HiddenFieldPartsCount.Value = CType(RepeaterPartsInfo.Items.Count, String)

            'データを設定する.
            For i = 0 To RepeaterPartsInfo.Items.Count - 1
                Logger.Info("SetPartsInfo Repeater Roop Index=" + CType(i, String))

                Dim partsInfo As Control = RepeaterPartsInfo.Items(i)

                'データカウンタを更新する.
                dataCount += 1
                Logger.Info("SetPartsInfo PartsNo=" + CType(dataCount, String))
                CType(partsInfo.FindControl("LiteralPartsNo"), Literal).Text = CType(dataCount, String)

                '部品名称を表示する.
                Dim strPartsName As String = CType(partsInfo.FindControl("HiddenFieldPartsName"), HiddenField).Value
                If String.IsNullOrEmpty(strPartsName) Then
                    Logger.Info("SetPartsInfo partsName is NullOrEmpty")
                    strPartsName = STRING_SPACE
                End If
                Logger.Info("SetPartsInfo PartsName=" + strPartsName)
                CType(partsInfo.FindControl("LiteralPartsName"), Literal).Text = strPartsName

                '区分を表示する.
                Dim strPartsType As String = CType(partsInfo.FindControl("HiddenFieldPartsType"), HiddenField).Value
                If String.IsNullOrEmpty(strPartsType) Then
                    Logger.Info("SetPartsInfo partsType is NullOrEmpty")
                    strPartsType = STRING_SPACE
                End If
                Logger.Info("SetPartsInfo PartsType=" + strPartsType)
                CType(partsInfo.FindControl("LiteralPartsType"), Literal).Text = strPartsType

                '数量を表示する.
                Dim strPartsQuantity As String = CType(partsInfo.FindControl("HiddenFieldPartsQuantity"), HiddenField).Value
                Dim dblPartsQuantity As Double = INIT_PARTS_QUANTITY
                If Not String.IsNullOrEmpty(strPartsQuantity) Then
                    Logger.Info("SetPartsInfo partsQuantity is NullOrEmpty")
                    dblPartsQuantity = CType(strPartsQuantity, Double)
                End If
                Logger.Info("SetPartsInfo PartsQuantity=" + CType(dblPartsQuantity, String))
                CType(partsInfo.FindControl("LiteralPartsQuantity"), Literal).Text = dblPartsQuantity.ToString("0", CultureInfo.CurrentCulture())

                '単位を表示する.
                Dim strPartsUnit As String = CType(partsInfo.FindControl("HiddenFieldPartsUnit"), HiddenField).Value
                If String.IsNullOrEmpty(strPartsUnit) Then
                    Logger.Info("SetPartsInfo partsUnit is NullOrEmpty")
                    strPartsUnit = STRING_SPACE
                End If
                Logger.Info("SetPartsInfo PartsUnit=" + strPartsUnit)
                CType(partsInfo.FindControl("LiteralPartsUnit"), Literal).Text = strPartsUnit

                'B/Oを表示する.
                Dim strPartsOrderStatus As String = CType(partsInfo.FindControl("HiddenFieldPartsOrderStatus"), HiddenField).Value
                If String.IsNullOrEmpty(strPartsOrderStatus) Then
                    Logger.Info("SetPartsInfo BOFlag is NullOrEmpty")
                    strPartsOrderStatus = STRING_SPACE
                End If
                Logger.Info("SetPartsInfo BOFlag=" + strPartsOrderStatus)
                CType(partsInfo.FindControl("LiteralPartsOrderStatus"), Literal).Text = strPartsOrderStatus
                'B/Oカウンタを更新する.
                If (PARTS_BACK_ORDER_STRING.Equals(strPartsOrderStatus)) Then
                    Logger.Info("SetPartsInfo BOFlag is BackOrder")
                    backOrderCount += 1
                End If
            Next

            'B/Oカウンタを格納する.
            Logger.Info("SetPartsInfo backOrderCount=" + CType(backOrderCount, String))
            Me.HiddenFieldPartsBackOrderCount.Value = CType(backOrderCount, String)
        End If

        Logger.Info("SetPartsInfo PartsCount=" + Me.HiddenFieldPartsCount.Value)
        Logger.Info("SetPartsInfo End")

    End Sub


    ''' <summary>
    ''' 基本情報パネルのデータを表示.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetBasicInfoData()

        Logger.Info("SetBasicInfoData Start")

        '基本情報パネル
        'Me.LabelBasicTab.Text = "基本情報"

        '燃料初期状態
        Me.HiddenField05_Fuel.Value = repairOrderData("fuelStatus").ToString()
        Logger.Info("SetBasicInfoData fuelStatus=" + repairOrderData("fuelStatus").ToString())

        'オーディオ初期状態
        Me.HiddenField05_Audio.Value = repairOrderData("audioStatus").ToString()
        Logger.Info("SetBasicInfoData audioStatus=" + repairOrderData("audioStatus").ToString())

        'エアコン初期状態
        Me.HiddenField05_AirConditioner.Value = repairOrderData("airConStatus").ToString()
        Logger.Info("SetBasicInfoData airConStatus=" + repairOrderData("airConStatus").ToString())

        '付属品1初期状態
        Me.HiddenField05_Accessory1.Value = repairOrderData("accesOne").ToString()
        Logger.Info("SetBasicInfoData accesOne=" + repairOrderData("accesOne").ToString())

        '付属品2初期状態
        Me.HiddenField05_Accessory2.Value = repairOrderData("accesTwo").ToString()
        Logger.Info("SetBasicInfoData accesTwo=" + repairOrderData("accesTwo").ToString())

        '付属品3初期状態
        Me.HiddenField05_Accessory3.Value = repairOrderData("accesThree").ToString()
        Logger.Info("SetBasicInfoData accesThree=" + repairOrderData("accesThree").ToString())

        '付属品4初期状態
        'Me.HiddenField05_Accessory4.Value = repairOrderData("accesFour").ToString()
        Me.HiddenField05_Accessory4.Value = repairOrderData("ccesFour").ToString()
        Logger.Info("SetBasicInfoData accesFour=" + repairOrderData("ccesFour").ToString())

        '付属品5初期状態
        Me.HiddenField05_Accessory5.Value = repairOrderData("accesFive").ToString()
        Logger.Info("SetBasicInfoData accesFive=" + repairOrderData("accesFive").ToString())

        '付属品6初期状態
        Me.HiddenField05_Accessory6.Value = repairOrderData("accesSix").ToString()
        Logger.Info("SetBasicInfoData accesSix=" + repairOrderData("accesSix").ToString())

        'エアコン温度初期状態
        Dim airControlerTemp As New StringBuilder
        airControlerTemp.Append(ExchangeDataToHtmlString(repairOrderData("airContpr").ToString()))
        airControlerTemp.Append(WebWordUtility.GetWord(121).Replace("%1", " "))
        Me.LiteralAirConditionerTemperature.Text = airControlerTemp.ToString()
        Logger.Info("SetBasicInfoData airConditionerTemp=" + airControlerTemp.ToString())

        '貴重品メモ初期状態
        Dim valuablesMemo As String
        valuablesMemo = ExchangeDataToHtmlString(repairOrderData("valMemo").ToString())
        Me.LiteralValuablesMemo.Text = valuablesMemo
        Logger.Info("SetBasicInfoData valMemo=" + valuablesMemo)

        Logger.Info("SetBasicInfoData End")

    End Sub


    ''' <summary>
    ''' ご用命事項パネルのデータを表示.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetOrdersInfoData()

        Logger.Info("SetOrdersInfoData Start")

        'ご用命項目パネルタイトル
        'Me.LabelOrdersTab.Text = "ご用命事項"

        'ご用命事項エリア
        Me.HiddenField07_ExchangeParts.Value = repairOrderData("CHANGEDACCFLAG").ToString() '部品交換後処理
        Logger.Info("SetOrdersInfoData changeDaccFlag=" + repairOrderData("CHANGEDACCFLAG").ToString())
        Me.HiddenField07_Waiting.Value = repairOrderData("WAITFLAG").ToString() '待ち方
        Logger.Info("SetOrdersInfoData waitFlag=" + repairOrderData("WAITFLAG").ToString())
        Me.HiddenField07_Washing.Value = repairOrderData("CLEANFLAG").ToString() '洗車
        Logger.Info("SetOrdersInfoData cleanFlag=" + repairOrderData("CLEANFLAG").ToString())
        Me.HiddenField07_Payment.Value = repairOrderData("PAYMENTFLAG").ToString() '支払方法
        Logger.Info("SetOrdersInfoData paymentFlag=" + repairOrderData("PAYMENTFLAG").ToString())
        Me.HiddenField07_Csi.Value = repairOrderData("CSIFLAG").ToString() 'CSI時間
        Logger.Info("SetOrdersInfoData csiFlag=" + repairOrderData("CSIFLAG").ToString())
        Dim invoiceAddress As String = ExchangeDataToHtmlString(repairOrderData("INVOICEADDRESS").ToString()) '請求書送付先
        Me.LiteralInvoiceAddress.Text = invoiceAddress
        Logger.Info("SetOrdersInfoData invoiceAddress=" + invoiceAddress)

        Dim orderMemo As String = ExchangeDataToHtmlString(repairOrderData("orderMemo").ToString()) 'ご用命事項
        Me.LiteralOrderMemo.Text = orderMemo
        Logger.Info("SetOrdersInfoData orderMemo=" + orderMemo)

        Me.HiddenField07_Warning.Value = repairOrderData("WNGFLAG").ToString() 'WNG
        Logger.Info("SetOrdersInfoData wngFlag=" + repairOrderData("WNGFLAG").ToString())
        Me.HiddenField07_Occurrence.Value = repairOrderData("TRBOCCURTIME").ToString() '故障発生時間
        Logger.Info("SetOrdersInfoData trbOccurTime=" + repairOrderData("TRBOCCURTIME").ToString())
        Me.HiddenField07_Frequency.Value = repairOrderData("TRBOCCURCYC").ToString() '故障発生頻度
        Logger.Info("SetOrdersInfoData trbOccurCyc=" + repairOrderData("TRBOCCURCYC").ToString())
        Me.HiddenField07_Reappear.Value = repairOrderData("RECURRENCEFLAG").ToString() '再現可能
        Logger.Info("SetOrdersInfoData recurrenceFlag" + repairOrderData("RECURRENCEFLAG").ToString())
        Me.HiddenField07_WaterT.Value = repairOrderData("wtemFlag").ToString() '水温フラグ
        Logger.Info("SetOrdersInfoData waterTempFlag=" + repairOrderData("wtemFlag").ToString())
        Dim waterTemperature As String = ExchangeDataToHtmlString(repairOrderData("WTEMP").ToString()) '水温
        'Me.TextBoxHearingWTemperature.Text = ExchangeDataToHtmlString(repairOrderData("WTEMP").ToString())
        Me.CustomLabelHearingWTemperature.Text = waterTemperature
        Logger.Info("SetOrdersInfoData waterTemp=" + waterTemperature)
        Me.HiddenField07_Temperature.Value = repairOrderData("AIRTEMPFLAG").ToString() '気温フラグ
        Logger.Info("SetOrdersInfoData airTempFlag=" + repairOrderData("AIRTEMPFLAG").ToString())
        Dim airTemperature As String = ExchangeDataToHtmlString(repairOrderData("AIRTEMP").ToString()) '気温
        'Me.TextBoxHearingTemperature.Text = ExchangeDataToHtmlString(repairOrderData("AIRTEMP").ToString())
        Me.CustomLabelHearingTemperature.Text = airTemperature
        Logger.Info("SetOrdersInfoData airTemp=" + airTemperature)
        Me.HiddenField07_Place.Value = repairOrderData("OCCURPLACEFLAG").ToString() '発生場所
        Logger.Info("SetOrdersInfoData occurPlaceFlag=" + repairOrderData("OCCURPLACEFLAG").ToString())
        Me.HiddenField07_TrafficJam.Value = repairOrderData("TRAFFICJAMFLAG").ToString() '渋滞状況
        Logger.Info("SetOrdersInfoData trafficJamFlag=" + repairOrderData("TRAFFICJAMFLAG").ToString())
        Me.HiddenField07_CarStatus_Startup.Value = repairOrderData("VHCSTATUS1").ToString() '車両状態1（起動時）
        Logger.Info("SetOrdersInfoData vhcStatus1=" + repairOrderData("VHCSTATUS1").ToString())
        Me.HiddenField07_CarStatus_Idling.Value = repairOrderData("VHCSTATUS2").ToString() '車両状態2（アイドリング時）
        Logger.Info("SetOrdersInfoData vhcStatus2=" + repairOrderData("VHCSTATUS2").ToString())
        Me.HiddenField07_CarStatus_Cold.Value = repairOrderData("VHCSTATUS3").ToString() '車両状態3（冷間時）
        Logger.Info("SetOrdersInfoData vhcStatus3=" + repairOrderData("VHCSTATUS3").ToString())
        Me.HiddenField07_CarStatus_Warm.Value = repairOrderData("VHCSTATUS4").ToString() '車両状態4（熱間時）
        Logger.Info("SetOrdersInfoData vhcStatus4=" + repairOrderData("VHCSTATUS4").ToString())
        Me.HiddenField07_Traveling.Value = repairOrderData("VHCSTATUS6").ToString() '車両状態6（穏速・加速・減速）
        Logger.Info("SetOrdersInfoData vhcStatus6=" + repairOrderData("VHCSTATUS6").ToString())
        Me.HiddenField07_CarStatus_Parking.Value = repairOrderData("VHCSTATUS9").ToString() '車両状態9（駐車時）
        Logger.Info("SetOrdersInfoData vhcStatus9=" + repairOrderData("VHCSTATUS9").ToString())
        Me.HiddenField07_CarStatus_Advance.Value = repairOrderData("VHCSTATUS10").ToString() '車両状態10（進行時）
        Logger.Info("SetOrdersInfoData vhcStatus10=" + repairOrderData("VHCSTATUS10").ToString())
        Me.HiddenField07_CarStatus_ShiftChange.Value = repairOrderData("VHCSTATUS11").ToString() '車両状態11（変速時）
        Logger.Info("SetOrdersInfoData vhcStatus11=" + repairOrderData("VHCSTATUS11").ToString())
        Me.HiddenField07_CarStatus_Back.Value = repairOrderData("VHCSTATUS12").ToString() '車両状態12（後退時）
        Logger.Info("SetOrdersInfoData vhcStatus12=" + repairOrderData("VHCSTATUS12").ToString())
        Me.HiddenField07_CarStatus_Brake.Value = repairOrderData("VHCSTATUS13").ToString() '車両状態13（ブレーキ時）
        Logger.Info("SetOrdersInfoData vhcStatus13=" + repairOrderData("VHCSTATUS13").ToString())
        Me.HiddenField07_CarStatus_Detour.Value = repairOrderData("VHCSTATUS14").ToString() '車両状態14（曲がる時）
        Logger.Info("SetOrdersInfoData vhcStatus14=" + repairOrderData("VHCSTATUS14").ToString())
        Me.HiddenField07_NonGenuine.Value = repairOrderData("ACCESSORYFLAG").ToString() '非純正部品使用フラグ
        Logger.Info("SetOrdersInfoData accessoryFlag=" + repairOrderData("ACCESSORYFLAG").ToString())
        Dim carSpeed As String = ExchangeDataToHtmlString(repairOrderData("CARSPEED").ToString()) '車両速度
        Me.TextBoxHearingSpeedRate.Text = carSpeed
        Logger.Info("SetOrdersInfoData carSpeed=" + carSpeed)
        Dim carGear As String = ExchangeDataToHtmlString(repairOrderData("CARBADDISH").ToString()) '車両ギア
        Me.TextBoxHearingSpeedGear.Text = carGear
        Logger.Info("SetOrdersInfoData carBadDish=" + carGear)
        Dim passengerCount As String = ExchangeDataToHtmlString(repairOrderData("PASSENGER").ToString()) '乗車人数
        Me.TextBoxHearingPeopleNumber.Text = passengerCount
        Logger.Info("SetOrdersInfoData passenger=" + passengerCount)
        Dim passengerLoad As String = ExchangeDataToHtmlString(repairOrderData("LOAD").ToString()) '車両負荷
        Me.TextBoxHearingPeopleTooHeavy.Text = passengerLoad
        Logger.Info("SetOrdersInfoData load=" + passengerLoad)

        Logger.Info("SetOrdersInfoData End")

    End Sub



    ''' <summary>
    ''' 文字列データに対して、空の場合もHTMLに表示するためスペース文字を返す.
    ''' </summary>
    ''' <param name="aStrData"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ExchangeDataToHtmlString(ByVal aStrData As String) As String

        Logger.Info("ExchangeDataToHtmlString Start param1:" + aStrData)

        Dim strHtmlString As String = aStrData
        If String.IsNullOrEmpty(strHtmlString) Then
            strHtmlString = STRING_SPACE
        End If

        Logger.Info("ExchangeDataToHtmlString End return:" + strHtmlString)
        Return strHtmlString

    End Function
#End Region

End Class
