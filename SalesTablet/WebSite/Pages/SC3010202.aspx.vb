Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.iCROP.BizLogic.SC3010202
Imports Toyota.eCRB.iCROP.DataAccess.SC3010202
Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic

'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3010202.aspx.vb
'─────────────────────────────────────
'機能： ダッシュボード
'補足： 
'作成：  
'更新： 2012/05/16 KN 浅野　HTMLエンコード対応
'更新： 2014/05/30 TMEJ y.gotoh 受注後フォロー機能開発 $02
'─────────────────────────────────────
Partial Class PagesSC3010202
    Inherits BasePage

#Region "定数"

    ''' <summary>
    ''' 画面ID
    ''' </summary>
    Private Const APPLICATIONID As String = "SC3010202"

    '$02 受注後フォロー機能開発 START
    ''' <summary>
    ''' 進捗率グラフの比重
    ''' 例：進捗率が100%の場合、100 * 1.21(比重) = 121pxとなる。
    ''' </summary>
    Private Const DEFAULT_GRAPH_WIDTH_WEIGHT As Double = 1.21D
    '$02 受注後フォロー機能開発 END

    ''' <summary>
    ''' グラフ幅の最大長(px)
    ''' </summary>
    Private Const MAX_GRAPH_WIDTH As Integer = 182

    ''' <summary>
    ''' 進捗率の表示位置の判断値(px)。
    ''' 進捗率がこの値より少ない場合、グラフ外に表示する。
    ''' 進捗率がこの値以上の場合、グラフ中に表示する。
    ''' </summary>
    Private Const DEFAULT_GRAPHVALUE_POSITION_BORDERLINE As Integer = 150

    ''' <summary>
    ''' アプリケーションデータ名 - 基本アプリケーションデータ名
    ''' </summary>
    Private Const SESSION_BASE_APPLICATION As String = "SC3010202_APPDATA_"

    '$02 受注後フォロー機能開発 START

    ''' <summary>
    ''' フォントサイズ(8px)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FONT_SIZE_FOUR_DIGITS = "8"

    ''' <summary>
    ''' フォントサイズ(10px)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FONT_SIZE_THREE_DIGITS_OR_LESS = "10"
    '$02 受注後フォロー機能開発 END

#End Region

#Region "メンバ変数"

#Region "グラフ幅"

    ''' <summary>
    ''' 来店項目のグラフ幅(px)
    ''' </summary>
    Private GraphWidthWalkInValue As String

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
    ''' 受注項目のグラフ幅(px)
    ''' </summary>
    Private GraphWidthOrderValue As String

#End Region

    '$02 受注後フォロー機能開発 START

#Region "目標値のフォントサイズ"

    ''' <summary>
    ''' 来店項目の目標値のフォントサイズ(px)
    ''' </summary>
    Private TargetFontSizeWalkInValue As String

    ''' <summary>
    ''' 試乗項目の目標値のフォントサイズ(px)
    ''' </summary>
    Private TargetFontSizeTestDriveValue As String

    ''' <summary>
    ''' 査定項目の目標値のフォントサイズ(px)
    ''' </summary>
    Private TargetFontSizeEvaluationValue As String

    ''' <summary>
    ''' 納車項目の目標値のフォントサイズ(px)
    ''' </summary>
    Private TargetFontSizeDeliveryValue As String

    ''' <summary>
    ''' 受注項目の目標値のフォントサイズ(px)
    ''' </summary>
    Private TargetFontSizeOrderValue As String

#End Region

#Region "実績値のフォントサイズ"

    ''' <summary>
    ''' 来店項目の実績値のフォントサイズ(px)
    ''' </summary>
    Private ResultFontSizeWalkInValue As String

    ''' <summary>
    ''' 試乗項目の実績値のフォントサイズ(px)
    ''' </summary>
    Private ResultFontSizeTestDriveValue As String

    ''' <summary>
    ''' 査定項目の実績値のフォントサイズ(px)
    ''' </summary>
    Private ResultFontSizeEvaluationValue As String

    ''' <summary>
    ''' 納車項目の実績値のフォントサイズ(px)
    ''' </summary>
    Private ResultFontSizeDeliveryValue As String

    ''' <summary>
    ''' 受注項目の実績値のフォントサイズ(px)
    ''' </summary>
    Private ResultFontSizeOrderValue As String

#End Region

    '$02 受注後フォロー機能開発 END
#End Region

#Region "getter/setter"

#Region "グラフ幅"

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
    ''' 受注項目のグラフ幅(px)
    ''' </summary>
    ''' <value>受注項目のグラフ幅(px)</value>
    ''' <remarks>読み取り専用</remarks>
    Public ReadOnly Property GraphWidthOrder() As String
        Get
            Return GraphWidthOrderValue
        End Get
    End Property

#End Region

    '$02 受注後フォロー機能開発 START

#Region "目標値のフォントサイズ"

    ''' <summary>
    ''' 来店項目の目標値のフォントサイズ(px)
    ''' </summary>
    ''' <value>来店項目の目標値のフォントサイズ(px)</value>
    ''' <remarks>読み取り専用</remarks>
    Public ReadOnly Property TargetFontSizeWalkIn() As String
        Get
            Return TargetFontSizeWalkInValue
        End Get
    End Property

    ''' <summary>
    ''' 試乗項目の目標値のフォントサイズ(px)
    ''' </summary>
    ''' <value>試乗項目の目標値のフォントサイズ(px)</value>
    ''' <remarks>読み取り専用</remarks>
    Public ReadOnly Property TargetFontSizeTestDrive() As String
        Get
            Return TargetFontSizeTestDriveValue
        End Get
    End Property

    ''' <summary>
    ''' 査定項目の目標値のフォントサイズ(px)
    ''' </summary>
    ''' <value>査定項目の目標値のフォントサイズ(px)</value>
    ''' <remarks>読み取り専用</remarks>
    Public ReadOnly Property TargetFontSizeEvaluation() As String
        Get
            Return TargetFontSizeEvaluationValue
        End Get
    End Property

    ''' <summary>
    ''' 納車項目の目標値のフォントサイズ(px)
    ''' </summary>
    ''' <value>納車項目の目標値のフォントサイズ(px)</value>
    ''' <remarks>読み取り専用</remarks>
    Public ReadOnly Property TargetFontSizeDelivery() As String
        Get
            Return TargetFontSizeDeliveryValue
        End Get
    End Property

    ''' <summary>
    ''' 受注項目の目標値のフォントサイズ(px)
    ''' </summary>
    ''' <value>受注項目の目標値のフォントサイズ(px)</value>
    ''' <remarks>読み取り専用</remarks>
    Public ReadOnly Property TargetFontSizeOrder() As String
        Get
            Return TargetFontSizeOrderValue
        End Get
    End Property

#End Region

#Region "実績値のフォントサイズ"

    ''' <summary>
    ''' 来店項目の実績値のフォントサイズ(px)
    ''' </summary>
    ''' <value>来店項目の実績値のフォントサイズ(px)</value>
    ''' <remarks>読み取り専用</remarks>
    Public ReadOnly Property ResultFontSizeWalkIn() As String
        Get
            Return ResultFontSizeWalkInValue
        End Get
    End Property

    ''' <summary>
    ''' 試乗項目の実績値のフォントサイズ(px)
    ''' </summary>
    ''' <value>試乗項目の実績値のフォントサイズ(px)</value>
    ''' <remarks>読み取り専用</remarks>
    Public ReadOnly Property ResultFontSizeTestDrive() As String
        Get
            Return ResultFontSizeTestDriveValue
        End Get
    End Property

    ''' <summary>
    ''' 査定項目の実績値のフォントサイズ(px)
    ''' </summary>
    ''' <value>査定項目の実績値のフォントサイズ(px)</value>
    ''' <remarks>読み取り専用</remarks>
    Public ReadOnly Property ResultFontSizeEvaluation() As String
        Get
            Return ResultFontSizeEvaluationValue
        End Get
    End Property

    ''' <summary>
    ''' 納車項目の実績値のフォントサイズ(px)
    ''' </summary>
    ''' <value>納車項目の実績値のフォントサイズ(px)</value>
    ''' <remarks>読み取り専用</remarks>
    Public ReadOnly Property ResultFontSizeDelivery() As String
        Get
            Return ResultFontSizeDeliveryValue
        End Get
    End Property

    ''' <summary>
    ''' 受注項目の実績値のフォントサイズ(px)
    ''' </summary>
    ''' <value>受注項目の実績値のフォントサイズ(px)</value>
    ''' <remarks>読み取り専用</remarks>
    Public ReadOnly Property ResultFontSizeOrder() As String
        Get
            Return ResultFontSizeOrderValue
        End Get
    End Property

#End Region

    '$02 受注後フォロー機能開発 END
#End Region


    ''' <summary>
    ''' ページロード時の処理です。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks>Postで呼ばれることを想定していないのでisPostBack判定は行いません</remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Logger.Info("Page_Load Start")
        Dim culture As CultureInfo = New CultureInfo("en")

        'スタッフ情報を取得
        Dim staffInfo As StaffContext = StaffContext.Current
        Dim ApplicationData As SC3010202Sturuct

        'アプリケーション名の作成
        Dim ApplicationName = SESSION_BASE_APPLICATION + staffInfo.Account

        '画面文言の設定
        SetWord()

        'バッチ更新時間を取得
        Dim BusinessLogic As SC3010202BusinessLogic = New SC3010202BusinessLogic
        Dim BatchStartTime As Date = BusinessLogic.GetBatchStartTime(staffInfo.DlrCD)

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

        Logger.Info("Batch(MC30102) EndTime is " + BatchStartTime.ToShortTimeString)
        Logger.Info("Use SessionData:" + isUseApplicationData.ToString())

        '$02 受注後フォロー機能開発 START
        '画面項目に反映(更新日時)
        Label_UpdateTime.Text = DateTimeFunc.FormatDate(14, BatchStartTime, staffInfo.DlrCD)

        Dim orgnzIdList As List(Of String)
        If staffInfo.OpeCD.Equals(Operation.SL) And staffInfo.TeamLeader Then
            orgnzIdList = BusinessLogic.GetTeamList(staffInfo, staffInfo.TeamCD)
        Else
            orgnzIdList = New List(Of String)
        End If
        '$02 受注後フォロー機能開発 END

        '目標値を取得
        Dim SC3010202TargetTable As SC3010202DataSet.SC3010202TargetDataTable
        If isUseApplicationData Then
            SC3010202TargetTable = ApplicationData.TargetTable
        Else
            '$02 受注後フォロー機能開発 START
            SC3010202TargetTable = BusinessLogic.GetTargetInfo(staffInfo, orgnzIdList)
            '$02 受注後フォロー機能開発 END
        End If

        '目標情報を判定
        Dim SC3010202TargetRow As SC3010202DataSet.SC3010202TargetRow
        If Not IsNothing(SC3010202TargetTable) And SC3010202TargetTable.Count > 0 Then
            SC3010202TargetRow = SC3010202TargetTable.Rows(0)
        Else
            'データがない場合は空(すべて0)のデータテーブルを準備する
            SC3010202TargetRow = SC3010202TargetTable.NewSC3010202TargetRow()
        End If

        '画面項目に反映(目標値)
        SetTargetDisplayValue(SC3010202TargetRow, culture)

        '$02 受注後フォロー機能開発 START
        'フォントサイズを反映(目標値)
        SetTargetFontSizeValue(SC3010202TargetRow)
        '$02 受注後フォロー機能開発 END

        'アプリケーションデータに情報をセット
        ApplicationData.TargetTable = SC3010202TargetTable

        '実績情報を取得
        Dim SC3010202ResultTable As SC3010202DataSet.SC3010202ResultDataTable
        If isUseApplicationData Then
            SC3010202ResultTable = ApplicationData.ResultTable
        Else
            '$02 受注後フォロー機能開発 START
            SC3010202ResultTable = BusinessLogic.GetResultInfo(staffInfo, orgnzIdList)
            '$02 受注後フォロー機能開発 END
        End If

        Dim sc3010202ResultRow As SC3010202DataSet.SC3010202ResultRow
        If Not IsNothing(SC3010202ResultTable) And SC3010202ResultTable.Count > 0 Then
            sc3010202ResultRow = SC3010202ResultTable.Rows(0)
        Else
            'データがない場合は空(すべて0)のデータテーブルを準備する
            sc3010202ResultRow = SC3010202ResultTable.NewSC3010202ResultRow()
        End If

        '画面項目に反映(実績値)
        SetResultDisplayValue(sc3010202ResultRow, culture)

        '$02 受注後フォロー機能開発 START
        'フォントサイズを反映(実績値)
        SetResultFontSizeValue(sc3010202ResultRow)
        '$02 受注後フォロー機能開発 END

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

        Logger.Info("Page_Load End")
    End Sub

    ''' <summary>
    ''' 文言の設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetWord()
        '$02 受注後フォロー機能開発 START
        ' 2012/05/16 KN 浅野　HTMLエンコード対応 START
        Label_UpdateText.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(APPLICATIONID, 2))
        Label_WalkIn.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(APPLICATIONID, 4))
        Label_TestDrive.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(APPLICATIONID, 6))
        Label_Evaluation.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(APPLICATIONID, 7))
        Label_Delivery.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(APPLICATIONID, 8))
        Label_Order.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(APPLICATIONID, 14))
        ' 2012/05/16 KN 浅野　HTMLエンコード対応 END
        '$02 受注後フォロー機能開発 END
    End Sub

    ''' <summary>
    ''' 画面項目：目標値に値を設定
    ''' </summary>
    ''' <param name="SC3010202TargetRow">目標値テーブル</param>
    ''' <param name="culture">国情報</param>
    ''' <remarks></remarks>
    Private Sub SetTargetDisplayValue(ByVal SC3010202TargetRow As SC3010202DataSet.SC3010202TargetRow, ByVal culture As CultureInfo)
        With SC3010202TargetRow
            '$02 受注後フォロー機能開発 START
            Label_WalkIn_Target.Text = .WalkIn.ToString(culture).Substring(If(.WalkIn.ToString(culture).Length < 4, 0, .WalkIn.ToString(culture).Length - 4))
            Label_TestDrive_Target.Text = .TESTDRIVE.ToString(culture).Substring(If(.TESTDRIVE.ToString(culture).Length < 4, 0, .TESTDRIVE.ToString(culture).Length - 4))
            Label_Evaluation_Target.Text = .EVALUATION.ToString(culture).Substring(If(.EVALUATION.ToString(culture).Length < 4, 0, .EVALUATION.ToString(culture).Length - 4))
            Label_Delivery_Target.Text = .DELIVERY.ToString(culture).Substring(If(.DELIVERY.ToString(culture).Length < 4, 0, .DELIVERY.ToString(culture).Length - 4))
            Label_Order_Target.Text = .ORDERS.ToString(culture).Substring(If(.ORDERS.ToString(culture).Length < 4, 0, .ORDERS.ToString(culture).Length - 4))
            '$02 受注後フォロー機能開発 END
        End With
    End Sub

    '$02 受注後フォロー機能開発 START
    ''' <summary>
    ''' 目標値のフォントサイズを設定
    ''' </summary>
    ''' <param name="SC3010202TargetRow">目標値テーブル</param>
    ''' <remarks></remarks>
    Private Sub SetTargetFontSizeValue(ByVal SC3010202TargetRow As SC3010202DataSet.SC3010202TargetRow)
        With SC3010202TargetRow

            TargetFontSizeWalkInValue = GetFontSize(.WalkIn)
            TargetFontSizeTestDriveValue = GetFontSize(.TESTDRIVE)
            TargetFontSizeEvaluationValue = GetFontSize(.EVALUATION)
            TargetFontSizeDeliveryValue = GetFontSize(.DELIVERY)
            TargetFontSizeOrderValue = GetFontSize(.ORDERS)

        End With
    End Sub
    '$02 受注後フォロー機能開発 END

    ''' <summary>
    ''' 表示可能文字数より長い場合、文字列の末尾をカットする
    ''' </summary>
    ''' <param name="original">調整前の文字列</param>
    ''' <param name="length">表示可能文字数</param>
    ''' <returns>調整した文字列</returns>
    ''' <remarks></remarks>
    Private Function CutTailString(ByVal original As String, ByVal length As Integer) As String

        With New StringBuilder("CutTailString_Start Param[")
            Logger.Info(.Append(original).Append(", ").Append(length).Append("]").ToString())
        End With

        ' 調整した文字列
        Dim result As String = Nothing

        ' 表示可能文字数以内
        If original.Length <= length Then
            Logger.Info("CutTailString_001")
            result = original

            ' 表示可能文字数を超過
        Else
            Logger.Info("CutTailString_002")
            result = original.Substring(original.Length - length)
        End If

        With New StringBuilder("CutTailString_End Ret[")
            Logger.Info(.Append(result).Append("]").ToString())
        End With

        ' 戻り値に調整した文字列を設定
        Return result

    End Function

    ''' <summary>
    ''' 画面項目：実績値に値を設定
    ''' </summary>
    ''' <param name="SC3010202ResultRow"> 実績値情報</param>
    ''' <param name="culture">国情報</param>
    ''' <remarks></remarks>
    Private Sub SetResultDisplayValue(ByVal SC3010202ResultRow As SC3010202DataSet.SC3010202ResultRow, ByVal culture As CultureInfo)
        With SC3010202ResultRow
            '$02 受注後フォロー機能開発 START
            Label_WalkIn_Result.Text = .WALKIN.ToString(culture).Substring(If(.WALKIN.ToString(culture).Length < 4, 0, .WALKIN.ToString(culture).Length - 4))
            Label_TestDrive_Result.Text = .TESTDRIVE.ToString(culture).Substring(If(.TESTDRIVE.ToString(culture).Length < 4, 0, .TESTDRIVE.ToString(culture).Length - 4))
            Label_Evaluation_Result.Text = .EVALUATION.ToString(culture).Substring(If(.EVALUATION.ToString(culture).Length < 4, 0, .EVALUATION.ToString(culture).Length - 4))
            Label_Delivery_Result.Text = .DELIVERY.ToString(culture).Substring(If(.DELIVERY.ToString(culture).Length < 4, 0, .DELIVERY.ToString(culture).Length - 4))
            Label_Order_Result.Text = .ORDERS.ToString(culture).Substring(If(.ORDERS.ToString(culture).Length < 4, 0, .ORDERS.ToString(culture).Length - 4))
            '$02 受注後フォロー機能開発 END
        End With
    End Sub

    '$02 受注後フォロー機能開発 START
    ''' <summary>
    ''' 実績値のフォントサイズを設定
    ''' </summary>
    ''' <param name="SC3010202ResultRow">実績値テーブル</param>
    ''' <remarks></remarks>
    Private Sub SetResultFontSizeValue(ByVal SC3010202ResultRow As SC3010202DataSet.SC3010202ResultRow)
        With SC3010202ResultRow

            ResultFontSizeWalkInValue = GetFontSize(.WALKIN)
            ResultFontSizeTestDriveValue = GetFontSize(.TESTDRIVE)
            ResultFontSizeEvaluationValue = GetFontSize(.EVALUATION)
            ResultFontSizeDeliveryValue = GetFontSize(.DELIVERY)
            ResultFontSizeOrderValue = GetFontSize(.ORDERS)

        End With
    End Sub
    '$02 受注後フォロー機能開発 END

    ''' <summary>
    ''' 進捗率の非表示の設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetHidenGraph()
        '$02 受注後フォロー機能開発 START
        '進捗率が0%のグラフを非表示にする
        If GraphWidthWalkInValue.Equals("0") Then : Div_WalkIn_Graph.Visible = False : End If
        If GraphWidthTestDriveValue.Equals("0") Then : Div_TestDrive_Graph.Visible = False : End If
        If GraphWidthEvaluationValue.Equals("0") Then : Div_Evaluation_Graph.Visible = False : End If
        If GraphWidthDeliveryValue.Equals("0") Then : Div_Delivery_Graph.Visible = False : End If
        If GraphWidthOrderValue.Equals("0") Then : Div_Order_Graph.Visible = False : End If
        '$02 受注後フォロー機能開発 END
    End Sub

    ''' <summary>
    ''' 実績と進捗率を設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetResultValue(ByVal SC3010202TargetRow As SC3010202DataSet.SC3010202TargetRow, ByVal sc3010202ResultRow As SC3010202DataSet.SC3010202ResultRow)
        '$02 受注後フォロー機能開発 START
        '進捗率を計算
        Dim WalkIn_Persent As Integer = ConvertResultToPercent(SC3010202TargetRow.WalkIn, sc3010202ResultRow.WALKIN)
        Dim TestDRive_Persent As Integer = ConvertResultToPercent(SC3010202TargetRow.TESTDRIVE, sc3010202ResultRow.TESTDRIVE)
        Dim Evaluation_Persent As Integer = ConvertResultToPercent(SC3010202TargetRow.EVALUATION, sc3010202ResultRow.EVALUATION)
        Dim Delivery_Persent As Integer = ConvertResultToPercent(SC3010202TargetRow.DELIVERY, sc3010202ResultRow.DELIVERY)
        Dim Order_Persent As Integer = ConvertResultToPercent(SC3010202TargetRow.ORDERS, sc3010202ResultRow.ORDERS)

        '画面項目に反映（進捗率)
        Dim culture As CultureInfo = New CultureInfo("en")
        GraphWidthWalkInValue = CovertPercentToWidth(WalkIn_Persent).ToString(Culture)
        GraphWidthTestDriveValue = CovertPercentToWidth(TestDRive_Persent).ToString(culture)
        GraphWidthEvaluationValue = CovertPercentToWidth(Evaluation_Persent).ToString(culture)
        GraphWidthDeliveryValue = CovertPercentToWidth(Delivery_Persent).ToString(culture)
        GraphWidthOrderValue = CovertPercentToWidth(Order_Persent).ToString(culture)
        '$02 受注後フォロー機能開発 END
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

        '$02 受注後フォロー機能開発 START
        '進捗率を計算(実績値÷目標値×100)
        Dim persent As New Decimal(0)
        persent = result / target * 100
        '$02 受注後フォロー機能開発 END

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

            'グラフの長さが最大長を超えさせない
            If width >= MAX_GRAPH_WIDTH Then
                width = MAX_GRAPH_WIDTH
            End If
        End If

        Return width

    End Function

    '$02 受注後フォロー機能開発 START
    ''' <summary>
    ''' 進捗線の位置を取得する
    ''' </summary>
    ''' <returns>進捗線の位置</returns>
    ''' <remarks></remarks>
    Protected Shared Function GetProgressLinePosition() As String

        Dim daysInMonth As Integer = DateTime.DaysInMonth(Now.Year, Now.Month)

        '当日時点の目標値に対する進捗率(現在日 ÷ 当月の日数 × 100)
        Dim percent As Integer = Now.Day / daysInMonth * 100

        Dim left As Integer = percent * DEFAULT_GRAPH_WIDTH_WEIGHT

        Return left
    End Function

    ''' <summary>
    ''' 目標値、実績値からフォントサイズを取得する
    ''' </summary>
    ''' <param name="value">目標値、実績値</param>
    ''' <returns>フォントサイズ</returns>
    ''' <remarks></remarks>
    Private Function GetFontSize(ByVal value As Integer) As String

        If value < 1000 Then
            Return FONT_SIZE_THREE_DIGITS_OR_LESS
        Else
            Return FONT_SIZE_FOUR_DIGITS
        End If

    End Function

    '$02 受注後フォロー機能開発 END

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

