' エラーコード番号
Public Enum ErrorCode

    ''' <summary>None</summary>
    None = 0

    ''' <summary>終了日付が開始日付より小さい</summary>
    SetTimeError = 903

    ''' <summary>スタッフコードが未設定</summary>
    NotStaffCode = 904

    ''' <summary>操作権限コードが未設定</summary>
    NotPermission = 905

    '2012/03/01 SKFC 加藤 【SALES_2】受注後工程の対応 START

    ''' <summary>販売店コードが未設定</summary>
    NotDealerCode = 906

    ''' <summary>店舗コードが未設定</summary>
    NotBranchCode = 907

    ''' <summary>スケジュール区分が未設定</summary>
    NotScheduleDiv = 908

    ''' <summary>スケジュールIDが未設定</summary>
    NotScheduleID = 909

    '2012/03/01 SKFC 加藤 【SALES_2】受注後工程の対応 END

    '2014/04/25 SKFC 上田 NEXTSTEP_CALDAV START
    ''' <summary>処理区分が不正</summary>
    ActionTypeError = 910
    ''' <summary>終了日付2が開始日付2より小さい</summary>
    SetTimeError2 = 911
    '2014/04/25 SKFC 上田 NEXTSTEP_CALDAV END

End Enum

Public NotInheritable Class ConstClass

    ' プライベートコンストラクタ
    Private Sub New()

    End Sub

    ' Ｒルールのフラグ
    Public Const RRuleFlgOn = "1"

    Public Const RRuleFlgOff = "0"

    ' 繰り返し周期の値
    ''' <summary>Rルールなし</summary>
    Public Const RRuleFreqNone = "NONE"

    ''' <summary>Rルールなし</summary>
    Public Const RRuleFreqDaily = "DAILY"

    ''' <summary>Rルールなし</summary>
    Public Const RRuleFreqWeekly = "WEEKLY"

    ''' <summary>Rルールなし</summary>
    Public Const RRuleFreqMonthly = "MONTHLY"

    ''' <summary>Rルールなし</summary>
    Public Const RRuleFreqYearly = "YEARLY"


    'XML要素名
    ''' <summary>Calendar要素</summary>
    Public Const XmlElementCalendar As String = "Calendar"

    ''' <summary>Detail要素</summary>
    Public Const XmlElementDetail As String = "Detail"

    ''' <summary>Common要素</summary>
    Public Const XmlElementCommon As String = "Common"

    ''' <summary>CreateLocation要素</summary>
    Public Const XmlElementCreateLocation As String = "CreateLocation"

    ''' <summary>DealerCode要素</summary>
    Public Const XmlElementDealerCode As String = "DealerCode"

    ''' <summary>BranchCode要素</summary>
    Public Const XmlElementBranchCode As String = "BranchCode"

    ''' <summary>ScheduleID要素</summary>
    Public Const XmlElementScheduleId As String = "ScheduleID"

    ''' <summary>ScheduleDiv要素</summary>
    Public Const XmlElementScheduleDiv As String = "ScheduleDiv"

    ''' <summary>ScheduleInfo要素</summary>
    Public Const XmlElementScheduleInfo As String = "ScheduleInfo"

    ''' <summary>CustomerDiv要素</summary>
    Public Const XmlElementCustomerDiv As String = "CustomerDiv"

    ''' <summary>CustomerCode要素</summary>
    Public Const XmlElementCustomerCode As String = "CustomerCode"

    ''' <summary>DmsID要素</summary>
    Public Const XmlElementDmsId As String = "DmsID"

    ''' <summary>CustomerName要素</summary>
    Public Const XmlElementCustomerName As String = "CustomerName"

    ''' <summary>ReceptionDiv要素</summary>
    Public Const XmlElementReceptionDiv As String = "ReceptionDiv"

    ''' <summary>VTodo要素</summary>
    Public Const XmlElementVTodo As String = "VTodo"

    ''' <summary>ContactNo要素</summary>
    Public Const XmlElementContactNo As String = "ContactNo"

    ''' <summary>Summary要素</summary>
    Public Const XmlElementSummary As String = "Summary"

    ''' <summary>DtStart要素</summary>
    Public Const XmlElementDTStart As String = "DtStart"

    ''' <summary>Due要素</summary>
    Public Const XmlElementDue As String = "Due"

    ''' <summary>TimeFlg要素</summary>
    Public Const XmlElementTimeFlg As String = "TimeFlg"

    ''' <summary>AllDayFlg要素</summary>
    Public Const XmlElementAllDayFlg As String = "AllDayFlg"

    ''' <summary>Description要素</summary>
    Public Const XmlElementDescription As String = "Description"

    ''' <summary>XiCropColor要素</summary>
    Public Const XmlElementXICropColor As String = "XiCropColor"

    ''' <summary>ProcessDiv要素</summary>
    ''' <history>2012/03/01 SKFC 加藤 【SALES_2】受注後工程の対応 START</history>
    Public Const XmlElementProcessDiv As String = "ProcessDiv"

    ''' <summary>VAlarm要素</summary>
    Public Const XmlElementVAlarm As String = "VAlarm"

    ''' <summary>Trigger要素</summary>
    Public Const XmlElementTrigger As String = "Trigger"

    ''' <summary>TodoID要素</summary>
    Public Const XmlElementTodoId As String = "TodoID"

    ''' <summary>CompFlg要素</summary>
    Public Const XmlElementCompFlg As String = "CompFlg"

    ''' <summary>EventFlg要素</summary>
    Public Const XmlElementEventFlg As String = "EventFlg"

    ''' <summary>VEvent要素</summary>
    Public Const XmlElementVEvent As String = "VEvent"

    ''' <summary>DtEnd要素</summary>
    Public Const XmlElementDTEnd As String = "DtEnd"

    ''' <summary>EventID要素</summary>
    Public Const XmlElementEventId As String = "EventID"

    ''' <summary>LinkTodoID要素</summary>
    Public Const XmlElementLinkTodoId As String = "LinkTodoID"

    ''' <summary>UpdateDate要素</summary>
    Public Const XmlElementUpdateDate As String = "UpdateDate"

    ' 2014/04/25 SKFC 上田 NEXTSTEP_CALDAV START
    ''' <summary>ContactName(接触方法名)要素</summary>
    Public Const XmlElementContactName As String = "ContactName"

    ''' <summary>ActOdrName(受注後活動名称)要素</summary>
    Public Const XmlElementActOdrName As String = "ActOdrName"

    ''' <summary>OdrDiv(受注区分)要素</summary>
    Public Const XmlElementOdrDiv As String = "OdrDiv"

    ''' <summary>AfterOdrI(受注後活動ID)要素</summary>
    Public Const XmlElementAfterOdrID As String = "AfterOdrID"
    ' 2014/04/25 SKFC 上田 NEXTSTEP_CALDAV END


    ''' <summary>Naitiveの値の場合のカレンダーIDの値（固定）</summary>
    Public Const SqlCalendarIdNative As String = "NATIVE"

    ''' <summary>CreateLocation要素の値がIcropの場合</summary>
    Public Const XmlCreateLocationIcrop As String = "1"

    ''' <summary>CreateLocation要素の値がNativeの場合</summary>
    Public Const XmlCreateLocationNative As String = "2"

    ''' <summary>TodoEventFlg－Todo</summary>
    Public Const TodoEventFlgTodo As String = "1"

    ''' <summary>TodoEventFlg－Event</summary>
    Public Const TodoEventFlgEvent As String = "2"

    ''' <summary>TODOイベントに紐付くEventIDが取得できている場合、EventFlgを1(True)とする</summary>
    Public Const EventFlgTrue As String = "1"

    ''' <summary>TODOイベントに紐付くEventIDが取得できていない場合、EventFlgを0(False)とする</summary>
    Public Const EventFlgFalse As String = "0"

    ' 2014/04/25 SKFC 上田 NEXTSTEP_CALDAV START
    ' カレンダー日付情報取得の処理区分
    ''' <summary>当日情報取得</summary>
    Public Const ActionTypeToday As String = "1"

    ''' <summary>過去情報取得</summary>
    Public Const ActionTypeDone As String = "2"

    ''' <summary>未来情報取得</summary>
    Public Const ActionTypeFuture As String = "3"
    ' 2014/04/25 SKFC 上田 NEXTSTEP_CALDAV END
End Class
