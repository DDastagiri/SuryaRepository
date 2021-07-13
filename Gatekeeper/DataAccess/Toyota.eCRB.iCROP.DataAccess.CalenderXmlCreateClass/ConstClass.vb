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

End Class
