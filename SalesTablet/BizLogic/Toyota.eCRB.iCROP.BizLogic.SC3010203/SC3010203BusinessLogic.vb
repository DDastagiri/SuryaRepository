'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3010203BusinessLogic.vb
'─────────────────────────────────────
'機能： SCメイン
'補足： 
'作成： 2011/11/18 TCS 寺本
'更新： 2014/02/26 TCS 河原
'更新： 2015/02/19 TCS 安田
'─────────────────────────────────────

Imports System.Xml
Imports System.Text
Imports System.Web
Imports System.Globalization
Imports System.Reflection
Imports System.Reflection.MethodBase
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess.DlrEnvSettingDataSet
Imports Toyota.eCRB.iCROP.BizLogic.IC3040401
Imports Toyota.eCRB.iCROP.BizLogic.CalenderXmlCreateClass.BizLogic
Imports Toyota.eCRB.Common.MainMenu.DataAccess
Imports Toyota.eCRB.Common.MainMenu.DataAccess.SC3010203DataSet


''' <summary>
''' SC3010203(SCメイン)
''' Webページで使用するビジネスロジック
''' </summary>
''' <remarks></remarks>
Public NotInheritable Class SC3010203BusinessLogic
    Inherits BaseBusinessComponent


#Region "コンストラクタ"
    ''' <summary>
    ''' デフォルトコンストラクタ
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New(ByVal dlrcd As String, ByVal strcd As String, ByVal userid As String, ByVal operationCode As String)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
          "{0} Start > Params:dlrcd=[{1}] strcd=[{2}] userid=[{3}] operationCode=[{4}]", _
          GetCurrentMethod().Name, _
          dlrcd, _
          strcd, _
          userid, _
          operationCode))
        Me.DlrCd = dlrcd
        Me.StrCd = strcd
        Me.UserId = userid
        Me.OperationCode = operationCode
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0} End", GetCurrentMethod().Name))
    End Sub
#End Region


#Region "XML用定数"
    ''' <summary>
    ''' TODO時間指定あり
    ''' </summary>
    Private Const TIMEFLG_TIME As String = "1"
    ''' <summary>
    ''' TODO時間指定なし
    ''' </summary>
    Private Const TIMEFLG_NOTIME As String = "0"
    ''' <summary>
    ''' 遅れ
    ''' </summary>
    Private Const DELAYFLG_DELAY As String = "1"
    ''' <summary>
    ''' 遅れなし
    ''' </summary>
    Private Const DELAYFLG_NODELAY As String = "0"
    ''' <summary>
    ''' データ作成区分-ICROP
    ''' </summary>
    Private Const CREATEDATADIV_ICROP As String = "1"
    ''' <summary>
    ''' データ作成区分-NATIVE
    ''' </summary>
    Private Const CREATEDATADIV_NATIVE As String = "2"
    ''' <summary>
    ''' データ作成区分-遅れ
    ''' </summary>
    Private Const CREATEDATADIV_DELAY As String = "D"
    ''' <summary>
    ''' データ作成区分-完了
    ''' </summary>
    Private Const CREATEDATADIV_COMP As String = "C"
    ''' <summary>
    ''' スケジュール区分-来店
    ''' </summary>
    Private Const SCHEDULEDIV_WALKIN As String = "0"
    ''' <summary>
    ''' スケジュール区分-入庫予約
    ''' </summary>
    Private Const SCHEDULEDIV_SERVICE As String = "1"
    ''' <summary>
    ''' 完了フラグ-未完了
    ''' </summary>
    Private Const COMPFLG_NOCOMP As String = "0"
    ''' <summary>
    ''' 完了フラグ-完了
    ''' </summary>
    Private Const COMPFLG_COMP As String = "1"
    ''' <summary>
    ''' CalDav登録API-区分-追加
    ''' </summary>
    Private Const CALDAV_ACTIONTYPE_ADD As String = "0"

    ''' <summary>
    ''' TODO最大件数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MAX_TODO_COUNT As String = "MAX_TODO_COUNT"
    ''' <summary>
    ''' TODO 0件のXML
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TODO_NONE_XML As String = "<Calendar />"

#End Region


#Region "定数"
    Private DlrCd As String
    Private StrCd As String
    Private UserId As String
    Private OperationCode As String

    '敬称位置
    Private Const NAME_TITLE_POSITION As String = "KEISYO_ZENGO"

    '1: 名前の前に敬称(主に英語圏)
    Private Const NAME_TITLE_POSITION_PREFIX As String = "1"
    '2: 名前の後ろに敬称(中国など)
    Private Const NAME_TITLE_POSITION_SUFIX As String = "2"
    'スケジュール区分-来店
    Private Const SCHEDULE_DIV_VISIT As String = "0"
    'スケジュール区分-受注
    Private Const SCHEDULE_DIV_SUCCESS As String = "2"
    '接触方法-来店
    Private Const CONTACT_VISIT As String = "6"
    'CalDAV-TODO-未完了
    Private Const TODO_STATUS_NOT_COMPLETE As String = "0"
    'CalDAV-時間指定フラグ-有
    Private Const CALDAV_TIME_FLG As String = "1"
    '受注後工程-振当
    Private Const SUCCESS_ALLOCATION As String = "001"
    '受注後工程-入金
    Private Const SUCCESS_PAYMENT As String = "002"
    '受注後工程-登録
    Private Const SUCCESS_REGISTRATION As String = "003"
    '受注後工程-保険
    Private Const SUCCESS_INSURANCE As String = "004"
    '受注後工程-納車
    Private Const SUCCESS_DELIVERY As String = "005"

    '見込み度ー新規
    Private Const STATUS_NEW As String = "0"
    '見込み度ーHOT
    Private Const STATUS_HOT As String = "1"
    '見込み度ーWARM
    Private Const STATUS_WARM As String = "2"
    '見込み度ーSUCCESS
    Private Const STATUS_SUCCESS As String = "3"
    '見込み度ーCOLD
    Private Const STATUS_COLD As String = "4"
    '見込み度ーギブアップ
    Private Const STATUS_GIVEUP As String = "5"

    Private Const STATUS_ICON_PATH As String = "../Styles/Images/SC3100302/"
    Private Const STATUS_ICON_NEW As String = STATUS_ICON_PATH & "sc3100302_status_new.png"
    Private Const STATUS_ICON_COLD As String = STATUS_ICON_PATH & "sc3100302_status_cold.png"
    Private Const STATUS_ICON_GIVEUP As String = STATUS_ICON_PATH & "sc3100302_status_giveup.png"
    Private Const STATUS_ICON_WARM As String = STATUS_ICON_PATH & "sc3100302_status_warm.png"
    Private Const STATUS_ICON_HOT As String = STATUS_ICON_PATH & "sc3100302_status_hot.png"
    Private Const STATUS_ICON_SUCCESS As String = STATUS_ICON_PATH & "sc3100302_status_success.png"
    Private Const STATUS_ICON_ALLOCATION As String = STATUS_ICON_PATH & "sc3100302_status_allocation.png"
    Private Const STATUS_ICON_PAYMENT As String = STATUS_ICON_PATH & "sc3100302_status_payment.png"
    Private Const STATUS_ICON_DELIVERY As String = STATUS_ICON_PATH & "sc3100302_status_delivery.png"

    '遅れ状況-遅れ
    Public Const DELAY_STATUS_DELAY As String = "1"
    '遅れ状況-当日（活動結果未登録）
    Public Const DELAY_STATUS_DUE As String = "2"
    '遅れ状況-当日（活動結果登録済み）
    Public Const DELAY_STATUS_COMPLETE As String = "3"

    'フォローアップボックス商談-活動結果登録済み
    Private Const REGISTED_ACT_RESULT As String = "1"

    '顧客詳細（商談メモ）の編集モード
    Private Const EDIT_MODE As String = "3"

    Private Const OPERATIONCODE_ICON_BASE_PATH As String = "~/Styles/Images/Authority/"

    Private Const CST_VCL_TYPE_1 As String = "1"
    Private Const CST_CST_CLASS_1 As String = "1"

    Private Const AFTER_ODR_DELIVERY As String = "AFTER_ODR_DELIVERY"

    Private Const MAX_VISITACTUAL_COUNT As String = "MAX_VISITACTUAL_COUNT"

    Dim defultDate As Date = Date.ParseExact("1900/01/01 00:00:00", "yyyy/MM/dd HH:mm:ss", Nothing)

#End Region

#Region "変数"
    Private Shared sysEnvMaxTodoCount As String
    Private Shared sysEnvNameTitlePosition As String
#End Region

#Region "CalDav連携Xml取得・解析処理"


    ''' <summary>
    ''' スケジュール情報取得
    ''' </summary>
    ''' <returns>XML形式の文字列</returns>
    ''' <remarks></remarks>
    ''' 2014/02/17 TCS 大岩 XXXXXXXXXXXXXXXXXXXX START
    Public Function ReadMySchedule(isDisplayDate As String) As String

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_Start", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        Try
            Dim service As New ClassLibraryBusinessLogic
            Dim context As StaffContext = StaffContext.Current      'スタッフ情報

            Dim nowDate As Date = New Date(Now().Year, Now().Month, Now().Day)

            '現在日時より時分秒切捨て
            Dim truncNow As Date = DateValue(isDisplayDate)
            Dim dt As SC3010203TodoColorDataTable

            Dim mode As String = Nothing
            Dim startTime1 As Date = Nothing
            Dim endTIme1 As Date = Nothing
            Dim startTime2 As Date = Nothing
            Dim endTime2 As Date = Nothing

            If nowDate = truncNow Then
                '当日
                mode = "1"
                startTime1 = truncNow
                endTIme1 = New Date(truncNow.Year, truncNow.Month, truncNow.Day, 23, 59, 59)
            ElseIf truncNow < nowDate Then
                '過去
                mode = "2"
                startTime1 = truncNow
                endTIme1 = New Date(truncNow.Year, truncNow.Month, truncNow.Day, 23, 59, 59)
                startTime2 = truncNow
                endTime2 = New Date(truncNow.Year, truncNow.Month, truncNow.Day, 23, 59, 59)
            Else
                '未来
                mode = "3"
                startTime1 = truncNow
                endTIme1 = New Date(truncNow.Year, truncNow.Month, truncNow.Day, 23, 59, 59)
                startTime2 = truncNow
                endTime2 = New Date(truncNow.Year, truncNow.Month, truncNow.Day, 23, 59, 59)
            End If

            'スケジュール取得
            Dim xmlText As String = service.GetDayCalender(mode, startTime1, endTIme1, startTime2, endTime2, context.Account, CType(context.OpeCD, String))

            'パース
            Dim xml As New XmlDocument
            xml.LoadXml(xmlText)

            '色情報取得
            dt = SC3010203BusinessLogic.GetChipColorInfo()

            Dim totalXml As Integer = 0
            Dim todoTotalCount As Integer = 0
            Dim toDoIdDeleteList As New List(Of String)

            '商談IDリスト
            Dim BookedBeforeSalesID As New List(Of String)
            Dim BookedAfterSalesID As New List(Of String)
            Dim DeliAfterSalesID As New List(Of String)

            '最大件数取得
            Dim sysEnv As New SystemEnvSetting
            sysEnvMaxTodoCount = sysEnv.GetSystemEnvSetting(MAX_TODO_COUNT).PARAMVALUE

            'Detailタグ分繰り返し
            For Each detailNode As XmlNode In xml.SelectNodes("Calendar/Detail")

                'Commonタグ読み出し
                Dim common As Dictionary(Of String, String)
                common = SC3010203BusinessLogic.CreateElementsData(detailNode.SelectSingleNode("Common"), {"CreateLocation", "ScheduleDiv", "ScheduleID"})

                If Not detailNode.SelectSingleNode("VTodo") Is Nothing Then

                    For Each vToDoNode As XmlNode In detailNode.SelectNodes("VTodo")

                        If vToDoNode.SelectSingleNode("OdrDiv").InnerText = "0" Then

                            If Not BookedBeforeSalesID.Contains(common("ScheduleID")) Then
                                BookedBeforeSalesID.Add(common("ScheduleID"))
                            End If

                        ElseIf vToDoNode.SelectSingleNode("OdrDiv").InnerText = "1" Then

                            If Not BookedAfterSalesID.Contains(common("ScheduleID")) Then
                                BookedAfterSalesID.Add(common("ScheduleID"))
                            End If

                        ElseIf vToDoNode.SelectSingleNode("OdrDiv").InnerText = "2" Then

                            If Not DeliAfterSalesID.Contains(common("ScheduleID")) Then
                                DeliAfterSalesID.Add(common("ScheduleID"))
                            End If

                        End If
                    Next

                End If

                todoTotalCount += SC3010203BusinessLogic.EditTodo(dt, xml, context, detailNode, common, nowDate, todoTotalCount, toDoIdDeleteList)

                'スケジュールの編集
                SC3010203BusinessLogic.EditSchedule(dt, xml, detailNode, common, truncNow, toDoIdDeleteList)

                totalXml += detailNode.InnerXml.Length
            Next

            Dim biz As New SC3010203BusinessLogic(Me.DlrCd, Me.StrCd, Me.UserId, Me.OperationCode)
            Dim salesDt As New SC3010203DataSet.SC3010203SalesInfoDataTable

            salesDt = biz.GetSalesInfo(BookedBeforeSalesID, BookedAfterSalesID, DeliAfterSalesID)

            'Detailタグ分繰り返し
            For Each detailNode As XmlNode In xml.SelectNodes("Calendar/Detail")

                'Commonタグ読み出し
                Dim common As Dictionary(Of String, String)
                common = SC3010203BusinessLogic.CreateElementsData(detailNode.SelectSingleNode("Common"), {"CreateLocation", "ScheduleDiv", "ScheduleID"})

                For Each rw As SC3010203DataSet.SC3010203SalesInfoRow In salesDt

                    If detailNode.SelectNodes("VTodo").Count > 0 Then

                        For Each vTodoNode As XmlNode In detailNode.SelectNodes("VTodo")

                            If common("ScheduleID") = rw.SALES_ID And vTodoNode.SelectSingleNode("OdrDiv").InnerText = rw.ODRDIV AndAlso rw.RSLT_DATETIME <> defultDate Then
                                Dim backElement As XmlElement = xml.CreateElement("Rslt")
                                Dim rsltDatetime As String = DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, rw.RSLT_DATETIME, nowDate, DlrCd)
                                backElement.InnerText = rsltDatetime
                                vTodoNode.AppendChild(backElement)
                            End If

                        Next

                    End If

                Next

            Next

            If TODO_NONE_XML.Equals(xml.OuterXml) Then
                Return xml.OuterXml.Replace(TODO_NONE_XML, "<Calendar><TotalDetailCount>" & todoTotalCount & "</TotalDetailCount></Calendar>")
            Else
                Return xml.OuterXml.Replace("<Calendar>", "<Calendar><TotalDetailCount>" & todoTotalCount & "</TotalDetailCount>")
            End If

        Catch ex As Exception
            Logger.Error(ex.Message, ex)
            Throw
        End Try

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_End", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

    End Function


    ''' <summary>
    ''' チップ背景色を取得します。
    ''' </summary>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Private Shared Function GetChipColorInfo() As SC3010203TodoColorDataTable

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_Start", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        Dim dt As SC3010203TodoColorDataTable

        '検索処理
        Dim context = StaffContext.Current
        dt = SC3010203TableAdapter.ReadChipColorSetting(context.DlrCD)

        '処理結果返却
        Return dt

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_End", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

    End Function


    ''' <summary>
    ''' TODOノードの編集
    ''' </summary>
    ''' <param name="dt"></param>
    ''' <param name="doc">xmlドキュメント</param>
    ''' <param name="context">contextの内容</param>
    ''' <param name="detailNode">Detailノード</param>
    ''' <param name="common">Commonタグの内容</param>
    ''' <param name="nowDate">現在日付</param>
    ''' <param name="toDoTotalCount">Todo総件数</param>
    ''' <param name="toDoIdDeleteList">TodoId削除一覧</param>
    ''' <returns>toDo追加件数</returns>
    ''' <remarks></remarks>
    Private Shared Function EditTodo(ByVal dt As SC3010203TodoColorDataTable, _
                         ByVal doc As XmlDocument, _
                         ByVal context As StaffContext, _
                         ByVal detailNode As XmlNode, _
                         ByVal common As Dictionary(Of String, String), _
                         ByVal nowDate As Date, _
                         ByVal toDoTotalCount As Integer, _
                         ByVal toDoIdDeleteList As List(Of String)) As Integer

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_Start", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        Dim todoList As New List(Of Dictionary(Of String, String))

        Dim toDoCount As Integer = 0

        For Each todoNode As XmlNode In detailNode.SelectNodes("VTodo")
            Dim truncNow As Date = New Date(nowDate.Year, nowDate.Month, nowDate.Day)
            Dim todo As Dictionary(Of String, String)

            'タグ情報読み取り
            todo = SC3010203BusinessLogic.CreateElementsData(todoNode, {"ContactNo", "ProcessDiv", "DtStart", "Due", "XiCropColor", "TimeFlg", "TodoID", "OdrDiv"})

            'カウントアップ
            toDoCount += 1
            'TODOチップの最大表示件数を超えているか？
            If (toDoTotalCount + toDoCount) > CDbl(sysEnvMaxTodoCount) Then
                'TodoId削除一覧追加
                toDoIdDeleteList.Add(todo("TodoID"))
                'ToDoチップ削除
                todoNode.RemoveAll()
                detailNode.RemoveChild(todoNode)
                Continue For
            End If

            '納期を日付型に変換
            Dim due As Date = Date.ParseExact(todo("Due"), "yyyy/MM/dd HH:mm:ss", Nothing)
            Dim truncDue As Date = New Date(due.Year, due.Month, due.Day)

            '開始日時
            Dim dtSt As Nullable(Of Date)
            If (todo("DtStart").Trim.Length <= 0) Then
                '指定なし
                dtSt = Nothing
            Else
                '設定あり
                dtSt = Date.ParseExact(todo("DtStart"), "yyyy/MM/dd HH:mm:ss", Nothing)
            End If

            '時間指定なし or 過去日のTODOは納期のみ表示
            If todo("TimeFlg").Equals(TIMEFLG_NOTIME) Or truncDue < truncNow Then
                dtSt = Nothing
            End If

            '表示用日付タグ作成
            todoNode.AppendChild(SC3010203BusinessLogic.CreateDispDateElement(doc, nowDate, dtSt, due, context.DlrCD, todo("TimeFlg")))
            '遅れフラグタグ作成
            Dim delayElement As XmlElement = SC3010203BusinessLogic.CreateDelayElement(doc, nowDate, due, todo("TimeFlg"))
            todoNode.AppendChild(delayElement)
            '遅れフラグを保存
            todo("Delay") = delayElement.InnerText

            '色・アイコン
            Dim backColor As String = todo("XiCropColor")
            Dim contactNo As Integer
            Dim processDiv As String = String.Empty
            Dim OdrDiv As String = String.Empty

            If todo("ContactNo").Trim.Length > 0 Then
                contactNo = Integer.Parse(todo("ContactNo"), CultureInfo.InvariantCulture)
            End If
            If todo("ProcessDiv").Trim.Length > 0 Then
                processDiv = todo("ProcessDiv")
            End If

            OdrDiv = todo("OdrDiv")

            SC3010203BusinessLogic.TodoColorChange(dt, doc, todoNode, common, backColor, contactNo, processDiv, OdrDiv)

            'リストに登録
            todoList.Add(todo)
        Next

        Return toDoCount

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_End", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

    End Function


    ''' <summary>
    ''' スケジュールノードの編集
    ''' </summary>
    ''' <param name="doc">xmlドキュメント</param>
    ''' <param name="detailNode">Detailノード</param>
    ''' <param name="common">Commonタグの内容</param>
    ''' <param name="nowDate">現在日付</param>
    ''' <param name="toDoIdDeleteList">ToDo削除一覧</param>
    ''' <remarks></remarks>
    Private Shared Sub EditSchedule(ByVal dt As SC3010203TodoColorDataTable, _
                         ByVal doc As XmlDocument, _
                         ByVal detailNode As XmlNode, _
                         ByVal common As Dictionary(Of String, String), _
                         ByVal nowDate As Date, _
                         ByVal toDoIdDeleteList As List(Of String))

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_Start", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        For Each sheduleNode As XmlNode In detailNode.SelectNodes("VEvent")

            Dim truncNow As Date = New Date(nowDate.Year, nowDate.Month, nowDate.Day)
            Dim shedule As Dictionary(Of String, String)

            'タグ情報読み取り
            shedule = SC3010203BusinessLogic.CreateElementsData(sheduleNode, {"ContactNo", "ProcessDiv", "DtStart", "DtEnd", "XiCropColor", "LinkTodoID", "OdrDiv"})

            'ToDo削除一覧にLinkTodoIDが存在しているか？
            If toDoIdDeleteList.Contains(shedule("LinkTodoID")) Then
                'スケジュール削除
                sheduleNode.RemoveAll()
                Continue For
            End If
            '2012/3/21 TCS 松野 【SALES_2】 END

            '遅れ判定
            Dim delay As String = DELAYFLG_NODELAY

            '遅れエレメントを作成
            Dim delayElement As XmlElement = doc.CreateElement("Delay")
            delayElement.InnerText = delay
            sheduleNode.AppendChild(delayElement)

            '色・アイコン
            Dim backColor As String = shedule("XiCropColor")
            Dim contactNo As Integer
            Dim processDiv As String = String.Empty
            Dim OdrDiv As String = String.Empty

            If shedule("ContactNo").Trim.Length > 0 Then
                contactNo = Integer.Parse(shedule("ContactNo"), CultureInfo.InvariantCulture)
            End If
            If shedule("ProcessDiv").Trim.Length > 0 Then
                processDiv = shedule("ProcessDiv")
            End If

            OdrDiv = shedule("OdrDiv")

            SC3010203BusinessLogic.TodoColorChange(dt, doc, sheduleNode, common, backColor, contactNo, processDiv, OdrDiv)

            '開始・終了を設定
            Dim dtStart As Date = Date.ParseExact(shedule("DtStart"), "yyyy/MM/dd HH:mm:ss", Nothing)
            Dim dtEnd As Date = Date.ParseExact(shedule("DtEnd"), "yyyy/MM/dd HH:mm:ss", Nothing)

            '日跨ぎスケジュール考慮(スケジュール時間を今日に収める)
            If truncNow.ToString("yyyyMMdd", CultureInfo.InvariantCulture) > dtStart.ToString("yyyyMMdd", CultureInfo.InvariantCulture) Then
                '前日からのスケジュール
                dtStart = truncNow
                '差し替え
                sheduleNode.SelectSingleNode("DtStart").InnerText = dtStart.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture)
            End If

            If truncNow.ToString("yyyyMMdd", CultureInfo.InvariantCulture) < dtEnd.ToString("yyyyMMdd", CultureInfo.InvariantCulture) Then
                '翌日までのスケジュール
                dtEnd = New Date(truncNow.Year, truncNow.Month, truncNow.Day, 23, 59, 59)
                '差し替え
                sheduleNode.SelectSingleNode("DtEnd").InnerText = dtEnd.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture)
            ElseIf truncNow.ToString("yyyyMMdd", CultureInfo.InvariantCulture) > dtEnd.ToString("yyyyMMdd", CultureInfo.InvariantCulture) Then
                dtEnd = New Date(truncNow.Year, truncNow.Month, truncNow.Day, dtEnd.Hour, dtEnd.Minute, dtEnd.Second)
                sheduleNode.SelectSingleNode("DtEnd").InnerText = dtEnd.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture)
            End If

        Next

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_End", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================


    End Sub


    ''' <summary>
    ''' ノード内のタグ情報を取得します。
    ''' </summary>
    ''' <param name="node">ノード</param>
    ''' <param name="tagNames">読み込みを行うタグ名の配列</param>
    ''' <returns>ハッシュ</returns>
    ''' <remarks></remarks>
    Private Shared Function CreateElementsData(ByVal node As XmlNode, ByVal tagNames() As String) As Dictionary(Of String, String)

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_Start", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        Dim dict As New Dictionary(Of String, String)

        '指定タグ名分ループ
        For Each tagName As String In tagNames
            If node.SelectNodes(tagName).Count >= 1 Then
                'タグあり
                dict.Add(tagName, node.SelectSingleNode(tagName).InnerText)
            Else
                'タグなし
                dict.Add(tagName, String.Empty)
            End If
        Next

        '処理結果返却
        Return dict

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_End", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

    End Function


    ''' <summary>
    ''' 色情報取得
    ''' </summary>
    ''' <param name="dt"></param>
    ''' <param name="doc"></param>
    ''' <param name="node"></param>
    ''' <param name="common"></param>
    ''' <param name="backcolor"></param>
    ''' <param name="contactNo"></param>
    ''' <param name="processDiv"></param>
    ''' <remarks></remarks>
    Private Shared Sub TodoColorChange(ByVal dt As SC3010203TodoColorDataTable, _
                                       ByVal doc As XmlDocument, _
                                       ByVal node As XmlNode, _
                                       ByVal common As Dictionary(Of String, String), _
                                       ByVal backcolor As String, _
                                       ByVal contactNo As Integer, _
                                       ByVal processDiv As String, _
                                       ByVal odrDiv As String)

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_Start", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        'アイコン
        Dim iconPath As String = String.Empty
        If contactNo > 0 And String.Equals(odrDiv, "0") Then
            'LINQ問い合わせd
            Dim contactNoRows As IEnumerable(Of SC3010203TodoColorRow) = _
                    From n In dt Where n.CONTACTNO = contactNo Select n
            'アイコンパス格納
            For Each contactNoRow In contactNoRows
                If Not contactNoRow.IsICONPATHNull Then
                    iconPath = contactNoRow.ICONPATH
                Else
                    iconPath = ""
                End If
            Next
            If Not String.IsNullOrEmpty(Trim(iconPath)) Then
                'アプリケーション相対パスから、クライアントパスに変換
                iconPath = VirtualPathUtility.ToAbsolute(iconPath)
            End If
        ElseIf Not String.IsNullOrEmpty(processDiv) Then
            'LINQ問い合わせd
            Dim processCdRows As IEnumerable(Of SC3010203TodoColorRow) = _
                    From n In dt Where Trim(n.PROCESSCD) = Trim(processDiv) Select n
            'アイコンパス格納
            For Each processCdRow In processCdRows
                If Not processCdRow.IsICONPATHNull Then
                    iconPath = processCdRow.ICONPATH
                Else
                    iconPath = ""
                End If
            Next
        End If

        'アイコンパスエレメント作成
        Dim iconElement As XmlElement = doc.CreateElement("IconPath")
        iconElement.InnerText = iconPath
        node.AppendChild(iconElement)

        Dim cngBackColor As String = backcolor.Replace("""", "")
        Dim cngBackColor2 As String = backcolor.Replace("""", "")

        If node.SelectNodes("XiCropColor").Count > 0 Then
            'トリム処理
            node.SelectSingleNode("XiCropColor").InnerText = cngBackColor
        End If

        Dim compFlg As String = COMPFLG_NOCOMP
        If node.SelectNodes("CompFlg").Count > 0 Then
            '完了フラグ取得
            compFlg = node.SelectSingleNode("CompFlg").InnerText
        End If

        Dim scheduleColor As String = String.Empty
        '色指定がある場合は、オリジナル色を別タグに退避 (以降で遅れ、完了の色で上書きするため）
        If node.SelectNodes("XiCropColor").Count > 0 Then
            scheduleColor = node.SelectSingleNode("XiCropColor").InnerText
        End If

        '遅れフラグ
        Dim delay As String = node.SelectSingleNode("Delay").InnerText

        'LINQ問い合わせd
        Dim rowsBack As IEnumerable(Of SC3010203TodoColorRow) = Nothing
        Dim rowsBack2 As IEnumerable(Of SC3010203TodoColorRow) = Nothing

        If common("CreateLocation").Equals(CREATEDATADIV_NATIVE) Then
            '個人スケジュール
            rowsBack = From n In dt Where n.CREATEDATADIV = CREATEDATADIV_NATIVE
        ElseIf compFlg.Equals(COMPFLG_COMP) Then
            '完了
            rowsBack = From n In dt Where n.CREATEDATADIV = CREATEDATADIV_COMP
        ElseIf delay.Equals(DELAYFLG_DELAY) Then
            '遅れ
            rowsBack = From n In dt Where n.CREATEDATADIV = CREATEDATADIV_DELAY

            If contactNo <> 0 Then
                '通常
                rowsBack2 = From n In dt Where n.CREATEDATADIV = CREATEDATADIV_ICROP And n.CONTACTNO = contactNo
            ElseIf Not String.IsNullOrEmpty(processDiv) Then
                '通常
                rowsBack2 = From n In dt Where n.CREATEDATADIV = CREATEDATADIV_ICROP And n.PROCESSCD = processDiv
            End If

            If rowsBack2 IsNot Nothing Then
                For Each rowBack In rowsBack2
                    cngBackColor2 = rowBack.BACKGROUNDCOLOR
                Next
            End If
        Else
            If contactNo <> 0 Then
                '通常
                rowsBack = From n In dt Where n.CREATEDATADIV = CREATEDATADIV_ICROP And n.CONTACTNO = contactNo
            ElseIf Not String.IsNullOrEmpty(processDiv) Then
                '通常
                rowsBack = From n In dt Where n.CREATEDATADIV = CREATEDATADIV_ICROP And n.PROCESSCD = processDiv
            End If
        End If

        If rowsBack IsNot Nothing Then

            '背景色取得
            For Each rowBack In rowsBack
                cngBackColor = rowBack.BACKGROUNDCOLOR
                If common("CreateLocation").Equals(CREATEDATADIV_NATIVE) Then
                    '個人スケジュール
                    scheduleColor = rowBack.BACKGROUNDCOLOR
                End If
            Next

            'Todo用背景色設定
            If node.SelectNodes("XiCropColor").Count > 0 Then
                '更新
                node.SelectSingleNode("XiCropColor").InnerText = cngBackColor
            Else
                '追加
                Dim backElement As XmlElement = doc.CreateElement("XiCropColor")
                backElement.InnerText = cngBackColor
                node.AppendChild(backElement)
                scheduleColor = cngBackColor
            End If

        End If

        'スケジュール用背景色設定
        Dim sheduleColorElement As XmlElement = doc.CreateElement("ScheduleColor")
        If cngBackColor2 <> "" Then
            sheduleColorElement.InnerText = cngBackColor2
        Else
            sheduleColorElement.InnerText = scheduleColor
        End If

        node.AppendChild(sheduleColorElement)

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_End", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

    End Sub


    ''' <summary>
    ''' 表示用日付書式を作成
    ''' </summary>
    ''' <param name="nowDate">現在日時</param>
    ''' <param name="fromDate">開始</param>
    ''' <param name="toDate">終了</param>
    ''' <returns>日付文字列エレメント</returns>
    ''' <remarks></remarks>
    Private Shared Function CreateDispDateElement(ByVal doc As XmlDocument, _
                                                  ByVal nowDate As Date, _
                                                  ByVal fromDate As Nullable(Of Date), _
                                                  ByVal toDate As Date, _
                                                  ByVal dlrCd As String, _
                                                  ByVal TimeFlg As String) As XmlElement

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_Start", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        Dim dispText As String
        If fromDate Is Nothing Then
            '納期のみ表示
            If TimeFlg.Equals(TIMEFLG_NOTIME) Then
                '時間指定なし
                dispText = DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, toDate, nowDate, dlrCd, False)

            Else
                '時間指定あり
                dispText = DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, toDate, nowDate, dlrCd)
            End If

        Else
            '表示対象が翌日の場合
            Dim Today As Date = New Date(Now().Year, Now().Month, Now().Day)
            Dim DispDay As Date = New Date(toDate.Year, toDate.Month, toDate.Day)

            If Today <> DispDay Then
                '納期のみ表示
                Dim sb As New StringBuilder
                sb.Append(DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, toDate, nowDate, dlrCd))
                dispText = sb.ToString()
            Else
                'FROM-TOで表示
                Dim sb As New StringBuilder
                sb.Append(DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, fromDate.Value, nowDate, dlrCd))
                sb.Append(WebWordUtility.GetWord(8))
                sb.Append(DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, toDate, nowDate, dlrCd))
                dispText = sb.ToString()
            End If
        End If

        '表示用日付ノード作成
        Dim dispDateNode As XmlElement = doc.CreateElement("DispTime")
        dispDateNode.InnerText = dispText

        Return dispDateNode

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_End", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

    End Function


    ''' <summary>
    ''' 遅れフラグエレメントを取得します。
    ''' </summary>
    ''' <param name="doc">XMLドキュメント</param>
    ''' <param name="nowDate">現在日時</param>
    ''' <param name="targetDate">対象日時</param>
    ''' <returns>遅れエレメント</returns>
    ''' <remarks></remarks>
    Private Shared Function CreateDelayElement(ByVal doc As XmlDocument, ByVal nowDate As Date, ByVal targetDate As Date, ByVal timeFlg As String) As XmlElement

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_Start", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        '遅れフラグ
        Dim delay As String = SC3010203BusinessLogic.CheckDelay(nowDate, targetDate, timeFlg)
        Dim delayElement As XmlElement = doc.CreateElement("Delay")
        delayElement.InnerText = delay

        '処理結果返却
        Return delayElement

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_End", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

    End Function


    ''' <summary>
    ''' 遅れチェック
    ''' </summary>
    ''' <param name="nowDate">現在日時</param>
    ''' <param name="targetDate">対象日時</param>
    ''' <returns>遅れフラグ</returns>
    ''' <remarks></remarks>
    Private Shared Function CheckDelay(ByVal nowDate As Date, ByVal targetDate As Date, ByVal timeFlg As String) As String

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_Start", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        nowDate = Now()

        If timeFlg.Equals(TIMEFLG_TIME) Then
            '時間指定あり
            If nowDate > targetDate Then
                '遅れ
                Return DELAYFLG_DELAY
            Else
                'なし
                Return DELAYFLG_NODELAY
            End If
        Else
            '時間指定なし
            Dim truncNow As Date = New Date(nowDate.Year, nowDate.Month, nowDate.Day)
            If truncNow.Subtract(targetDate).TotalDays > 0 Then
                '遅れ
                Return DELAYFLG_DELAY
            Else
                'なし
                Return DELAYFLG_NODELAY
            End If
        End If

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_End", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

    End Function


    ''' <summary>
    ''' 商談情報取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetSalesInfo(ByVal BookedBeforeSalesID As List(Of String), ByVal BookedAfterSalesID As List(Of String), ByVal DeliAfterSalesID As List(Of String)) As SC3010203DataSet.SC3010203SalesInfoDataTable

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_Start", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        Dim salesInfoDt As New SC3010203DataSet.SC3010203SalesInfoDataTable
        Dim salesInfoTempDt As New SC3010203DataSet.SC3010203SalesInfoDataTable
        Dim SalesID As New Text.StringBuilder

        SalesID.Length = 0
        '引数.アカウントリストをIN句用に変換
        If BookedBeforeSalesID.Count > 0 Then
            For Each id In BookedBeforeSalesID
                SalesID.Append(id)
                SalesID.Append(",")
            Next
            SalesID.Remove(SalesID.Length - 1, 1)
        End If

        '初回商談日を取得
        If SalesID.Length <> 0 Then
            Using ta As New SC3010203TableAdapter(Me.DlrCd, Me.StrCd, Me.UserId)
                salesInfoTempDt = ta.ReadFirstSalesDate(SalesID.ToString)
                For Each dr As SC3010203DataSet.SC3010203SalesInfoRow In salesInfoTempDt
                    salesInfoDt.ImportRow(dr)
                Next
            End Using
        End If

        '引数.アカウントリストをIN句用に変換
        SalesID.Length = 0
        If BookedAfterSalesID.Count > 0 Then
            For Each id In BookedAfterSalesID
                SalesID.Append(id)
                SalesID.Append(",")
            Next
            SalesID.Remove(SalesID.Length - 1, 1)
        End If

        If SalesID.Length <> 0 Then
            Using ta As New SC3010203TableAdapter(Me.DlrCd, Me.StrCd, Me.UserId)
                '契約日を取得
                salesInfoTempDt = ta.ReadSuccessDate(SalesID.ToString)
                For Each dr As SC3010203DataSet.SC3010203SalesInfoRow In salesInfoTempDt
                    salesInfoDt.ImportRow(dr)
                Next
            End Using
        End If

        '引数.アカウントリストをIN句用に変換
        SalesID.Length = 0
        If DeliAfterSalesID.Count > 0 Then
            For Each id In DeliAfterSalesID
                SalesID.Append(id)
                SalesID.Append(",")
            Next
            SalesID.Remove(SalesID.Length - 1, 1)
        End If

        If SalesID.Length <> 0 Then
            Using ta As New SC3010203TableAdapter(Me.DlrCd, Me.StrCd, Me.UserId)
                '納車日を取得
                '納車活動の受注後活動IDを取得
                Dim sysEnv As New SystemEnvSetting
                Dim afterOrderId As String = sysEnv.GetSystemEnvSetting(AFTER_ODR_DELIVERY).PARAMVALUE

                salesInfoTempDt = ta.ReadDeliveryDate(SalesID.ToString, afterOrderId)
                For Each dr As SC3010203DataSet.SC3010203SalesInfoRow In salesInfoTempDt
                    salesInfoDt.ImportRow(dr)
                Next
            End Using
        End If

        Return salesInfoDt

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_End", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

    End Function

#End Region


#Region "スケジュール登録API呼び出し"
    ''' <summary>
    ''' スケジュール登録
    ''' </summary>
    ''' <param name="registData"></param>
    ''' <remarks></remarks>
    Public Shared Function RegistMySchedule(ByVal registData As SC3010203CalDavRegistInfoDataTable) As String

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_Start", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        If Not (registData IsNot Nothing AndAlso registData.Count >= 0) Then
            '検証registData
            Throw New ArgumentException("SC3010203BusinessLogic.RegistMySchedule", "registData")
        End If

        Dim context As StaffContext = StaffContext.Current      'スタッフ情報

        Dim dlrEnvSetting As New DealerEnvSetting
        Dim envSettingRow As DLRENVSETTINGRow = dlrEnvSetting.GetEnvSetting(context.DlrCD, "CALDAV_WEBSERVICE_URL")

        '登録情報設定
        Dim row As SC3010203CalDavRegistInfoRow = CType(registData.Rows(0), SC3010203CalDavRegistInfoRow)

        Try
            'サービスエージェントを設定
            Using service As New IC3040401BusinessLogic
                service.CreateCommon()
                service.ActionType = CALDAV_ACTIONTYPE_ADD
                service.DealerCode = row.DLRCD
                service.BranchCode = row.BRNCD
                service.ScheduleId = row.SCHEDULEID
                service.ScheduleDivision = SCHEDULEDIV_WALKIN
                service.ActivityCreateStaffCode = context.Account
                service.CreateScheduleInfo()
                service.TodoId(0) = row.TODOID
                service.StartTime(0) = row.STARTTIME.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture)
                service.EndTime(0) = row.ENDTIME.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture)
                '実行
                Return service.SendScheduleInfo(envSettingRow.PARAMVALUE)
            End Using
        Catch ex As Exception
            Logger.Error(ex.Message, ex)
            Throw
        End Try

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_End", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

    End Function
#End Region


#Region "次画面遷移情報取得"


    ''' <summary>
    ''' 活動先の顧客情報を取得する。
    ''' </summary>
    ''' <param name="dtParam">引数</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Public Shared Function GetCustInfo(ByVal dtParam As SC3010203CustInfoDataTable) As SC3010203CustInfoDataTable

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_Start", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        If Not (dtParam IsNot Nothing AndAlso dtParam.Count >= 0) Then
            '検証エラー
            Throw New ArgumentException("SC3010203BusinessLogic.GetCustInfo", "dtParam")
        End If

        Dim vclInfodt As SC3010203VclInfoDataTable
        Dim custInfodt As SC3010203CustInfoDataTable
        Dim salesId As Decimal = dtParam(0).FLLWUPBOX_SEQNO

        '誘致先車両情報取得（受注前）
        vclInfodt = SC3010203TableAdapter.GetVclInfoActive(salesId)

        If vclInfodt.Rows.Count > 0 Then
            If vclInfodt(0).VCL_ID = 0 Then
                '誘致先顧客情報取得（受注前, 車両なし）
                custInfodt = SC3010203TableAdapter.GetCustInfoActiveWithoutCar(salesId)
            Else
                '誘致先顧客情報取得（受注前, 車両あり）
                custInfodt = SC3010203TableAdapter.GetCustInfoActiveWithCar(salesId)
            End If
        Else
            '誘致先車両情報取得（受注後）
            vclInfodt = SC3010203TableAdapter.GetVclInfoHistory(salesId)

            If vclInfodt(0).VCL_ID = 0 Then
                '誘致先顧客情報取得（受注後, 車両なし）
                custInfodt = SC3010203TableAdapter.GetCustInfoHistoryWithoutCar(salesId)
            Else
                '誘致先顧客情報取得（受注後, 車両あり）
                custInfodt = SC3010203TableAdapter.GetCustInfoHistoryWithCar(salesId)
            End If
        End If

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_End", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        '処理結果返却
        Return custInfodt

    End Function


#End Region


#Region "来店実績"


    ''' <summary>
    ''' 来店実績一覧取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SelectVisitActualList(ByVal mode As String, ByVal startDatetime As Date, ByVal endDatetime As Date) As SC3010203DataSet.SC3010203VisitActualDataTable

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_Start", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        '最大件数取得
        Dim sysEnv As New SystemEnvSetting
        Dim visitActualCnt As String = sysEnv.GetSystemEnvSetting(MAX_VISITACTUAL_COUNT).PARAMVALUE

        Dim dt As SC3010203DataSet.SC3010203VisitActualDataTable

        Using ta As New SC3010203TableAdapter(Me.DlrCd, Me.StrCd, Me.UserId)
            '来店実績一覧を取得
            dt = ta.SelectVisitActualList(mode, startDatetime, endDatetime, visitActualCnt)

            '来店実績一覧を編集する
            EditVisitActualList(dt)
        End Using

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_End", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        Return dt
    End Function


    ''' <summary>
    ''' 来店実績一覧編集
    ''' </summary>
    ''' <parameter>
    ''' SC3010203DataSet.SC3010203VisitActualDataTable
    ''' </parameter>
    ''' <remarks>
    ''' 来店実績情報を編集する（次回活動の設定、アイコンパス設定)
    ''' </remarks>
    Private Sub EditVisitActualList(ByVal actualList As SC3010203DataSet.SC3010203VisitActualDataTable)

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_Start", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================
        '敬称位置取得
        Dim sysEnv As New SystemEnvSetting
        sysEnvNameTitlePosition = sysEnv.GetSystemEnvSetting(NAME_TITLE_POSITION).PARAMVALUE

        For Each dr As SC3010203DataSet.SC3010203VisitActualRow In actualList
            '顧客情報を設定する。
            SetCustomerInfo(dr)

            '遅れ状況を設定する
            SetDelayStatus(dr)

            '一次対応者権限アイコンのパスを編集する
            EditTempStaff(dr)
        Next

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_End", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

    End Sub


    ''' <summary>
    ''' 顧客情報設定
    ''' </summary>
    ''' <parameter>
    ''' SC3010203DataSet.SC3010203VisitActualRow
    ''' </parameter>
    ''' <remarks>
    ''' 顧客情報を設定する。
    ''' </remarks>
    Private Sub SetCustomerInfo(ByVal actualListRow As SC3010203DataSet.SC3010203VisitActualRow)

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_Start", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        Using ta As New SC3010203TableAdapter(Me.DlrCd, Me.StrCd, Me.UserId)

            '顧客と車両の情報を取得
            Dim data As SC3010203DataSet.SC3010203CustomerNameDataTable = ta.SelectCustomerVehicleInfo(actualListRow.FLLWUPBOX_SEQNO)
            Dim dt As SC3010203DataSet.SC3010203CustomerNameDataTable

            If data.Count = 0 Then
                data = ta.SelectCustomerVehicleInfoHistory(actualListRow.FLLWUPBOX_SEQNO)
            End If

            '顧客の氏名＋敬称を取得
            If data.Count > 0 Then
                If (CST_VCL_TYPE_1.Equals(data(0).CST_VCL_TYPE)) Then
                    dt = ta.SelectCustomerNameWithNameTitleOwner(data(0).CST_ID, data(0).DLR_CD)

                    '来店実績一覧に顧客情報を設定する。
                    With actualListRow
                        .CUSTSEGMENT = dt(0).CST_TYPE
                        .CUSTOMERCLASS = data(0).CST_VCL_TYPE
                        .CRCUSTID = CStr(data(0).CST_ID)
                    End With
                Else
                    dt = ta.SelectCustomerNameWithNameTitleNotOwner(data(0).DLR_CD, data(0).VCL_ID)
                    '来店実績一覧に顧客情報を設定する。
                    With actualListRow
                        .CUSTSEGMENT = dt(0).CST_TYPE
                        .CUSTOMERCLASS = CST_CST_CLASS_1
                        .CRCUSTID = CStr(dt(0).CST_ID)
                    End With
                End If
            Else
                dt = ta.SelectCustomerNameWithNameTitleOwner(CDec(actualListRow.CRCUSTID), actualListRow.DLRCD)
                '2015/02/19 TCS 安田 不具合対応(M005) Start
                '来店実績一覧に顧客情報を設定する。
                With actualListRow
                    .CUSTSEGMENT = dt(0).CST_TYPE
                End With
                '2015/02/19 TCS 安田 不具合対応(M005) End
            End If

            '敬称付き名称作成
            Dim customerNameWithTitle As New Text.StringBuilder
            If NAME_TITLE_POSITION_PREFIX.Equals(sysEnvNameTitlePosition) Then
                customerNameWithTitle.Append(dt(0).NAMETITLE)
                customerNameWithTitle.Append(" ")
            End If

            customerNameWithTitle.Append(dt(0).NAME)

            If NAME_TITLE_POSITION_SUFIX.Equals(sysEnvNameTitlePosition) Then
                customerNameWithTitle.Append(" ")
                customerNameWithTitle.Append(dt(0).NAMETITLE)
            End If

            '来店実績一覧に顧客名を設定する。
            With actualListRow
                .CUST_NAME = dt(0).NAME
                .CUST_NAMETITLE = dt(0).NAMETITLE
                .CUST_NAME_WITH_TITLE = customerNameWithTitle.ToString
            End With
        End Using

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_End", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

    End Sub


    ''' <summary>
    ''' 遅れ状況設定
    ''' </summary>
    ''' <parameters>
    ''' <parameter>
    ''' SC3010203DataSet.SC3010203VisitActualRow
    ''' </parameter>
    ''' </parameters>
    ''' <remarks>
    ''' 遅れ状況を設定する
    ''' </remarks>
    Private Sub SetDelayStatus(ByVal actualListRow As SC3010203DataSet.SC3010203VisitActualRow)

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_Start", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        Dim nowDate As Date = DateTimeFunc.Now(Me.DlrCd).Date
        Dim startDate As Date = actualListRow.STARTTIME.Date

        actualListRow.SALES_STATUS = String.Empty

        If nowDate.CompareTo(startDate) > 0 Then
            If REGISTED_ACT_RESULT.Equals(actualListRow.REGISTFLG) Then
                '活動結果登録済
                actualListRow.DELAY_STATUS = DELAY_STATUS_COMPLETE
            Else
                '過去
                '遅れ状況を設定する
                actualListRow.DELAY_STATUS = DELAY_STATUS_DELAY
                actualListRow.SALES_STATUS = EDIT_MODE
            End If
            '商談開始日を編集する
            actualListRow.SALES_DATE = DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, actualListRow.STARTTIME, Me.DlrCd)
        Else
            '現在
            '商談開始日を編集する
            Dim startTime As String = DateTimeFunc.FormatDate(14, actualListRow.STARTTIME)
            Dim endTime As String = DateTimeFunc.FormatDate(14, actualListRow.ENDTIME)
            Dim salesDate As String = String.Format(CultureInfo.InvariantCulture, "{0}-{1}", startTime, endTime)
            actualListRow.SALES_DATE = salesDate
            If REGISTED_ACT_RESULT.Equals(actualListRow.REGISTFLG) Then
                '活動結果登録済
                actualListRow.DELAY_STATUS = DELAY_STATUS_COMPLETE
            Else
                '活動結果未登録
                actualListRow.DELAY_STATUS = DELAY_STATUS_DUE
                actualListRow.SALES_STATUS = EDIT_MODE
            End If
        End If

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_End", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

    End Sub


    ''' <summary>
    ''' 一次対応者情報編集
    ''' </summary>
    ''' <parameters>
    ''' <parameter>
    ''' SC3010203DataSet.SC3010203VisitActualRow
    ''' </parameter>
    ''' </parameters>
    ''' <remarks>
    ''' 一次対応者の情報を編集する
    ''' </remarks>
    Private Sub EditTempStaff(ByVal actualListRow As SC3010203DataSet.SC3010203VisitActualRow)

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_Start", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        If actualListRow.ACCOUNT_PLAN.Equals(actualListRow.ACTUALACCOUNT) Then
            '一次対応者がいない場合は、一次対応者情報をクリアする
            actualListRow.TEMP_STAFFNAME = String.Empty
            actualListRow.TEMP_STAFF_OPERATIONCODE = String.Empty
            actualListRow.TEMP_STAFF_OPERATIONCODE_ICON = String.Empty
            Return
        End If
        '一次対応者権限アイコンのパスを編集する

        actualListRow.TEMP_STAFF_OPERATIONCODE_ICON = OPERATIONCODE_ICON_BASE_PATH & actualListRow.TEMP_STAFF_OPERATIONCODE_ICON

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_End", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

    End Sub


#End Region


End Class