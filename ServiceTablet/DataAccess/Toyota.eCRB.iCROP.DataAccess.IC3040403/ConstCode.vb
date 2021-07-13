Imports Toyota.eCRB.SystemFrameworks.Core

''' <summary>
''' XMLの要素のデータの割り当てを指定します。
''' </summary>
''' <remarks></remarks>
Public Enum DataAssignment As Integer

    ''' <summary>必須項目</summary>
    ModeMandatory

    ''' <summary>オプション項目</summary>
    ModeOptional

End Enum

''' <summary>
''' スケジュール区分
''' </summary>
''' <remarks></remarks>
Public Enum ScheDuleDiv As Integer

    ' 来店予約
    VisitReservation = 0

    ' 入庫予約
    GRReservattion = 1

End Enum

''' <summary>
''' 処理フラグ
''' </summary>
''' <remarks></remarks>
Public Enum ActionType As Integer

    ''' <summary>None</summary>
    None = 0

    ' 登録・完了
    Entry = 1

    ' 更新処理
    Update = 2

    ' Event追加
    AddEvent = 3

End Enum

''' <summary>
''' 削除フラグ
''' </summary>
''' <remarks></remarks>
Public Enum Delflg As Integer

    ' 削除しない
    NotDel = 0

    ' 削除する
    Del = 1

End Enum

''' <summary>
''' 完了フラグ
''' </summary>
''' <remarks></remarks>
Public Enum CompletionFlg As Integer

    ''' <summary>None</summary>
    None = 0

    ''' <summary>完了なし</summary>
    FlgNotContinue = 1

    ''' <summary>Continue</summary>
    FlgContinue = 2

    ''' <summary>活動完了</summary>
    FlgActivityCompleted = 3

End Enum

''' <summary>
''' スケジュール作成区分
''' </summary>
''' <remarks></remarks>
Public Enum CreateScheduleDiv As Integer

    ''' <summary>None</summary>
    None = 0

    ''' <summary>Event+Todo</summary>
    FlgEventAndTodo = 1

    ''' <summary>Todo</summary>
    FlgTodo = 2

    ''' <summary>Event</summary>
    FlgEvent = 3

End Enum

''' <summary>
''' 文字チェックを行う際の型を指します
''' </summary>
''' <remarks></remarks>
Public Enum TypeConversion As Integer

    ''' <summary>チェックをしない</summary>
    None

    ''' <summary>文字列チェックを行う</summary>
    StringType

    ''' <summary>数値チェックを行う</summary>
    IntegerType

    ''' <summary>日付チェックを行う</summary>
    DateType

End Enum

''' <summary>
''' リターンコード
''' </summary>
''' <remarks></remarks>
Public Enum ReturnCode As Integer

    ' グループエラー、要素の値に＋して使用します　

    ''' <summary>必須要素非存在エラー</summary>
    NotXmlElementError = 2000

    ''' <summary>要素重複エラー</summary>
    ManyXmlElementError = 2500

    ''' <summary>要素内型変換エラー</summary>
    XmlParseError = 3000

    ''' <summary>要素内文字列桁数エラー</summary>
    XmlMaximumOfDigitError = 4000

    ''' <summary>要素値チェックエラー</summary>
    XmlValueCheckError = 5000

    ''' <summary>アプリケーション固有エラー</summary>
    UniqueError = 6000

    ''' <summary>データベースエラー</summary>
    DataBaseError = 9000

    ''' <summary>正常終了</summary>
    Successful = 0

    ''' <summary>XMLタグ不正エラー</summary>
    XmlIncorrect = -1

    ''' <summary>スタッフコードチェックエラー</summary>
    StaffCodeError = 1

    ''' <summary>エラーコード：システムエラー</summary>
    ErrCodeSys = 9999

End Enum

''' <summary>
''' SQL番号
''' </summary>
''' <remarks></remarks>
Public Enum DataBaseErrorCode As Integer

    ''' <summary>None</summary>
    None = 0

    ''' <summary>カレンダーID取得SQLエラー</summary>
    GetCalenderIdSqlError = 1

    ''' <summary>イベントＩＤ取得SQLエラー</summary>
    GetEventIdSqlError = 2

    ''' <summary>カレンダー管理登録SQLエラー</summary>
    InsertCalCalenderSqlError = 3

    ''' <summary>カレンダー管理更新SQLエラー</summary>
    UpdateCalCalenderSqlError = 4

    ''' <summary>カレンダー管理削除フラグ更新SQLエラー</summary>
    UpdateDeleteFlgCalItemSqlError = 5

    ''' <summary>Todo情報登録SQLエラー</summary>
    InsertCalTodoItemSqlError = 6

    ''' <summary>Todo情報更新SQLエラー</summary>
    UpdateCalTodoItemSqlError = 7

    ''' <summary>Todo情報削除フラグ更新SQLエラー</summary>
    UpdateDeleteFlgCalTodoItemSqlError = 8

    ''' <summary>Todo情報完了フラグ更新SQLエラー</summary>
    UpdateCompleteFlgCalTodoItemSqlError = 9

    ''' <summary>Todoアラーム登録SQLエラー</summary>
    InsertCalTodoAlarmsSqlError = 10

    ''' <summary>Todoアラーム削除SQLエラー</summary>
    DeleteCalTodoAlarmSqlError = 11

    ''' <summary>イベント情報登録SQLエラー</summary>
    InsertCalEventItemSqlError = 12

    ''' <summary>イベント情報更新SQLエラー</summary>
    UpdateCalEventItemSqlError = 13

    ''' <summary>イベント情報削除フラグ更新SQLエラー</summary>
    UpdateDeleteFlgCalEventItemSqlError = 14

    ''' <summary>紐づきイベント追加SQLエラー</summary>
    InsertLinkEventSqlError = 15

    ''' <summary>イベントアラーム登録SQLエラー</summary>
    InsertCalEventAlarmsSqlError = 16

    ''' <summary>イベントアラーム削除SQLエラー</summary>
    DeleteCalEventAlarmSqlError = 17

    ''' <summary>新規カレンダーID取得SQLエラー</summary>
    GetNewCalenderIdSqlError = 18

    ''' <summary>新規TodoID取得SQLエラー</summary>
    GetNewTodoIdSqlError = 19

    ''' <summary>新規イベントID取得SQLエラー</summary>
    GetNewEventIdSqlError = 20

    ''' <summary>新規ユニークID取得SQLエラー</summary>
    GetNewUniqueIdSqlError = 21

    ''' <summary>Todoスタッフ取得SQLエラー</summary>
    SelectStaffCodeTodoItemSqlError = 22

    ''' <summary>イベントスタッフ取得SQLエラー</summary>
    SelectStaffCodeEventItemSqlError = 23

    ''' <summary>スタッフ登録SQLエラー</summary>
    InsertCalCardLastModifySqlError = 24

    ''' <summary>スタッフ更新SQLエラー</summary>
    UpdateCalCardLastModifySqlError = 25

End Enum

''' <summary>
''' 要素番号
''' </summary>
''' <remarks></remarks>
Public Enum ElementName As Integer


    ''' <summary>None</summary>
    None = 0

    ''' <summary>RegistSchedule要素</summary>
    RegistSchedule = 1

    ''' <summary>Head要素</summary>
    Head = 2

    ''' <summary>MessageID要素</summary>
    MessageId = 3

    ''' <summary>CountryCode要素</summary>
    CountryCode = 4

    ''' <summary>LinkSystemCode要素</summary>
    LinkSystemCode = 5

    ''' <summary>TransmissionDate要素</summary>
    TransmissionDate = 6

    ''' <summary>Detail要素</summary>
    Detail = 7

    ''' <summary>Common要素</summary>
    Common = 8

    ''' <summary>DealerCode要素</summary>
    DealerCode = 9

    ''' <summary>BranchCode要素</summary>
    BranchCode = 10

    ''' <summary>ScheduleDiv要素</summary>
    ScheduleDiv = 11

    ''' <summary>ScheduleID要素</summary>
    ScheduleId = 12

    ''' <summary>ActionType要素</summary>
    ActionType = 13

    ''' <summary>ActivityCreateStaff要素</summary>
    ActivityCreateStaff = 14

    ''' <summary>ScheduleInfo要素</summary>
    ScheduleInfo = 15

    ''' <summary>CustomerDiv要素</summary>
    CustomerDiv = 16

    ''' <summary>CustomerCode要素</summary>
    CustomerCode = 17

    ''' <summary>DmsID要素</summary>
    DmsId = 18

    ''' <summary>CustomerName要素</summary>
    CustomerName = 19

    ''' <summary>ReceptionDiv要素</summary>
    ReceptionDiv = 20

    ''' <summary>ServiceCode要素</summary>
    ServiceCode = 21

    ''' <summary>MerchandiseCd要素</summary>
    MerchandiseCD = 22

    ''' <summary>StrStatus要素</summary>
    StrStatus = 23

    ''' <summary>RezStatus要素</summary>
    RezStatus = 24

    ''' <summary>CompletionDiv要素</summary>
    CompletionDiv = 25

    ''' <summary>CompletionDate要素</summary>
    CompletionDate = 26

    ''' <summary>DeleteDate要素</summary>
    DeleteDate = 27

    ''' <summary>Schedule要素</summary>
    Schedule = 28

    ''' <summary>CreateScheduleDiv要素</summary>
    CreateScheduleDiv = 29

    ''' <summary>ActivityStaffBranchCode要素</summary>
    ActivityStaffBranchCode = 30

    ''' <summary>ActivityStaffCode要素</summary>
    ActivityStaffCode = 31

    ''' <summary>ReceptionStaffBranchCode要素</summary>
    ReceptionStaffBranchCode = 32


    ''' <summary>ReceptionStaffCode要素</summary>
    ReceptionStaffCode = 33

    ''' <summary>ContactNo要素</summary>
    ContactNo = 34

    ''' <summary>Summary要素</summary>
    Summary = 35

    ''' <summary>StartTime要素</summary>
    StartTime = 36

    ''' <summary>EndTime要素</summary>
    EndTime = 37

    ''' <summary>Memo要素</summary>
    Memo = 38

    ''' <summary>XiCropColor要素</summary>
    XICropColor = 39

    ''' <summary>Alarm要素</summary>
    Alarm = 40

    ''' <summary>Trigger要素</summary>
    Trigger = 41

    ''' <summary>TodoID要素</summary>
    TodoId = 42

    ''' <summary>ParentDiv要素</summary>
    ParentDiv = 43


End Enum

Public NotInheritable Class ConstCode

    ' プライベートコンストラクタ
    Private Sub New()

    End Sub



    ''' <summary>完了フラグあり</summary>
    Public Const CompleteFlgYes As String = "1"
    ''' <summary>完了フラグなし</summary>
    Public Const CompleteFlgNo As String = "0"

    ''' <summary>開始日指定フラグあり</summary>
    Public Const StartTimeFlgYes As String = "1"
    ''' <summary>開始日指定フラグなし</summary>
    Public Const StartTimeFlgNo As String = "0"

    ''' <summary>時刻指定フラグあり</summary>
    Public Const TimeFlgYes As String = "1"
    ''' <summary>時刻指定フラグなし</summary>
    Public Const TimeFlgNo As String = "0"

    ''' <summary>終日フラグあり</summary>
    Public Const AllDayFlgYes As String = "1"
    ''' <summary>終日フラグなし</summary>
    Public Const AllDayFlgNo As String = "0"

    ''' <summary>日付のみデータの場合の文字列長</summary>
    Public Const DateLength As Integer = 10

    ''' <summary>空文字列</summary>
    Public Const EmptyString As String = ""

    ''' <summary>作成機能ＩＤ</summary>
    Public Const CreateId As String = "IC3040403"

    ''' <summary>更新機能ＩＤ</summary>
    Public Const UpdateId As String = "IC3040403"

    ''' <summary>R-ルールを使用しない場合に格納する値</summary>
    Public Const RruleNone As String = "NONE"

    ''' <summary>XMLの要素内の要素を取得する際の先頭につけるもの</summary>
    Public Const XmlRootDirectry As String = "//"

    ''' <summary>RECURRENCEIDの要素内に設定する固定値</summary>
    Public Const RecurrenceIdElement As String = " "

    ''' <summary>正常値のメッセージＩＤ</summary>
    Public Const TrueMessageId As String = "IC3040403"

    ''' <summary>正常値のLinkSystemCode</summary>
    Public Const TrueLinkSystemCode As String = "0"
    ' XML要素名一覧
    ''' <summary>RegistSchedule要素</summary>
    Public Const XmlNameRegistSchedule As String = "RegistSchedule"

    ''' <summary>Head要素</summary>
    Public Const XmlNameHead As String = "Head"

    ''' <summary>MessageID要素</summary>
    Public Const XmlNameMessageId As String = "MessageID"

    ''' <summary>CountryCode要素</summary>
    Public Const XmlNameCountryCode As String = "CountryCode"

    ''' <summary>LinkSystemCode要素</summary>
    Public Const XmlNameLinkSystemCode As String = "LinkSystemCode"

    ''' <summary>TransmissionDate要素</summary>
    Public Const XmlNameTransmissionDate As String = "TransmissionDate"

    ''' <summary>Detail要素</summary>
    Public Const XmlNameDetailName As String = "Detail"

    ''' <summary>Common要素</summary>
    Public Const XmlNameCommonName As String = "Common"

    ''' <summary>DealerCode要素</summary>
    Public Const XmlNameDealerCode As String = "DealerCode"

    ''' <summary>BranchCode要素</summary>
    Public Const XmlNameBranchCode As String = "BranchCode"

    ''' <summary>ScheduleDiv要素</summary>
    Public Const XmlNameScheduleDiv As String = "ScheduleDiv"

    ''' <summary>ScheduleID要素</summary>
    Public Const XmlNameScheduleId As String = "ScheduleID"

    ''' <summary>ActionType要素</summary>
    Public Const XmlNameActionType As String = "ActionType"

    ''' <summary>ActivityCreateStaff要素</summary>
    Public Const XmlNameActivityCreateStaff As String = "ActivityCreateStaff"

    ''' <summary>ScheduleInfo要素</summary>
    Public Const XmlNameScheduleInfoName As String = "ScheduleInfo"

    ''' <summary>CustomerDiv要素</summary>
    Public Const XmlNameCustomerDiv As String = "CustomerDiv"

    ''' <summary>CustomerCode要素</summary>
    Public Const XmlNameCustomerCode As String = "CustomerCode"

    ''' <summary>DmsID要素</summary>
    Public Const XmlNameDmsId As String = "DmsID"

    ''' <summary>CustomerName要素</summary>
    Public Const XmlNameCustomerName As String = "CustomerName"

    ''' <summary>ReceptionDiv要素</summary>
    Public Const XmlNameReceptionDiv As String = "ReceptionDiv"

    ''' <summary>ServiceCode要素</summary>
    Public Const XmlNameServiceCode As String = "ServiceCode"

    ''' <summary>MerchandiseCd要素</summary>
    Public Const XmlNameMerchandiseCD As String = "MerchandiseCd"

    ''' <summary>StrStatus要素</summary>
    Public Const XmlNameStrStatus As String = "StrStatus"

    ''' <summary>RezStatus要素</summary>
    Public Const XmlNameRezStatus As String = "RezStatus"

    ''' <summary>CompletionDiv要素</summary>
    Public Const XmlNameCompletionDiv As String = "CompletionDiv"

    ''' <summary>CompletionDate要素</summary>
    Public Const XmlNameCompletionDate As String = "CompletionDate"

    ''' <summary>DeleteDate要素</summary>
    Public Const XmlNameDeleteDate As String = "DeleteDate"

    ''' <summary>Schedule要素</summary>
    Public Const XmlNameScheduleName As String = "Schedule"

    ''' <summary>CreateScheduleDiv要素</summary>
    Public Const XmlNameCreateScheduleDiv As String = "CreateScheduleDiv"

    ''' <summary>ActivityStaffBranchCode要素</summary>
    Public Const XmlNameActivityStaffBranchCode As String = "ActivityStaffBranchCode"

    ''' <summary>ActivityStaffCode要素</summary>
    Public Const XmlNameActivityStaffCode As String = "ActivityStaffCode"

    ''' <summary>ReceptionStaffBranchCode要素</summary>
    Public Const XmlNameReceptionStaffBranchCode As String = "ReceptionStaffBranchCode"

    ''' <summary>ReceptionStaffCode要素</summary>
    Public Const XmlNameReceptionStaffCode As String = "ReceptionStaffCode"

    ''' <summary>ContactNo要素</summary>
    Public Const XmlNameContactNo As String = "ContactNo"

    ''' <summary>Summary要素</summary>
    Public Const XmlNameSummary As String = "Summary"

    ''' <summary>StartTime要素</summary>
    Public Const XmlNameStartTime As String = "StartTime"

    ''' <summary>EndTime要素</summary>
    Public Const XmlNameEndTime As String = "EndTime"

    ''' <summary>Memo要素</summary>
    Public Const XmlNameMemo As String = "Memo"

    ''' <summary>XiCropColor要素</summary>
    Public Const XmlNameXICropColor As String = "XiCropColor"

    ''' <summary>Alarm要素</summary>
    Public Const XmlNameAlarm As String = "Alarm"

    ''' <summary>Trigger要素</summary>
    Public Const XmlNameTrigger As String = "Trigger"

    ''' <summary>TodoID要素</summary>
    Public Const XmlNameTodoId As String = "TodoID"

    ''' <summary>ParentDiv要素</summary>
    Public Const XmlNameParentDiv As String = "ParentDiv"

    ' 戻り値のみの要素
    ''' <summary>Response要素</summary>
    Public Const XmlNameResponse As String = "Response"

    ''' <summary>ReceptionDate要素</summary>
    Public Const XmlNameReceptionDate As String = "ReceptionDate"

    ''' <summary>ResultId要素</summary>
    Public Const XmlNameResultId As String = "ResultId"

    ''' <summary>Message要素</summary>
    Public Const XmlNameMessage As String = "Message"


    ' データテーブルカラム名

    ' カレンダー管理情報データテーブル

    ''' <summary>カレンダーID要素</summary>
    Public Const CalendarTableCalId As String = "CALID"

    ''' <summary>販売店コード要素</summary>
    Public Const CalendarTableDlrCD As String = "DLRCD"

    ''' <summary>店舗コード要素</summary>
    Public Const CalendarTableStrCD As String = "STRCD"

    ''' <summary>スケジュール区分要素</summary>
    Public Const CalendarTableScheduleDiv As String = "SCHEDULEDIV"

    ''' <summary>スケジュールID要素</summary>
    Public Const CalendarTableScheduleId As String = "SCHEDULEID"

    ''' <summary>顧客区分要素</summary>
    Public Const CalendarTableCustomerDiv As String = "CUSTOMERDIV"

    ''' <summary>顧客コード要素</summary>
    Public Const CalendarTableCustCode As String = "CUSTCODE"

    ''' <summary>顧客名要素</summary>
    Public Const CalendarTableCustName As String = "CUSTNAME"

    ''' <summary>DMSID要素</summary>
    Public Const CalendarTableDmsId As String = "DMSID"

    ''' <summary>受付納車区分要素</summary>
    Public Const CalendarTableReceptionDiv As String = "RECEPTIONDIV"

    ''' <summary>サービスコード要素</summary>
    Public Const CalendarTableServiceCode As String = "SERVICECODE"

    ''' <summary>商品コード要素</summary>
    Public Const CalendarTableMerchandDisCD As String = "MERCHANDISECD"

    ''' <summary>入庫ステータス要素</summary>
    Public Const CalendarTableStrStatus As String = "STRSTATUS"

    ''' <summary>予約ステータス要素</summary>
    Public Const CalendarTableRezStatus As String = "REZSTATUS"

    ''' <summary>削除フラグ要素</summary>
    Public Const CalendarTableDelFlg As String = "DELFLG"

    ''' <summary>削除日要素</summary>
    Public Const CalendarTableDelDate As String = "DELDATE"

    ''' <summary>作成日要素</summary>
    Public Const CalendarTableCreateDate As String = "CREATEDATE"

    ''' <summary>更新日要素</summary>
    Public Const CalendarTableUpdateDate As String = "UPDATEDATE"

    ''' <summary>作成アカウント要素</summary>
    Public Const CalendarTableCreateAccount As String = "CREATEACCOUNT"

    ''' <summary>更新アカウント要素</summary>
    Public Const CalendarTableUpdateAccount As String = "UPDATEACCOUNT"

    ''' <summary>作成機能ID要素</summary>
    Public Const CalendarTableCreateId As String = "CREATEID"

    ''' <summary>更新機能ID要素</summary>
    Public Const CalendarTableUpdateId As String = "UPDATEID"


    ' カレンダーToDo情報テーブル
    ''' <summary>TodoID要素</summary>
    Public Const TodoItemTableTodoId As String = "TODOID"

    ''' <summary>カレンダーID要素</summary>
    Public Const TodoItemTableCalId As String = "CALID"

    ''' <summary>ユニークID要素</summary>
    Public Const TodoItemTableUniqueId As String = "UNIQUEID"

    ''' <summary>リカレンスID要素</summary>
    Public Const TodoItemTableRecurrenceId As String = "RECURRENCEID"

    ''' <summary>変更シーケンス要素</summary>
    Public Const TodoItemTableChgSeqNo As String = "CHGSEQNO"

    ''' <summary>活動担当スタッフ店舗コード要素</summary>
    Public Const TodoItemTableActStaffStrCD As String = "ACTSTAFFSTRCD"

    ''' <summary>活動スタッフコード要素</summary>
    Public Const TodoItemTableActStaffCD As String = "ACTSTAFFCD"

    ''' <summary>受付担当スタッフ店舗コード要素</summary>
    Public Const TodoItemTableRecStaffStrCD As String = "RECSTAFFSTRCD"

    ''' <summary>受付担当スタッフコード要素</summary>
    Public Const TodoItemTableRecStaffCD As String = "RECSTAFFCD"

    ''' <summary>接触方法No要素</summary>
    Public Const TodoItemTableContactNo As String = "CONTACTNO"

    ''' <summary>タイトル要素</summary>
    Public Const TodoItemTableSummary As String = "SUMMARY"

    ''' <summary>開始日時要素</summary>
    Public Const TodoItemTableStartTime As String = "STARTTIME"

    ''' <summary>終了日時要素</summary>
    Public Const TodoItemTableEndTime As String = "ENDTIME"

    ''' <summary>開始日時指定フラグ要素</summary>
    Public Const TodoItemTableStartTimeFlg As String = "STARTTIMEFLG"

    ''' <summary>時刻指定フラグ要素</summary>
    Public Const TodoItemTableTimeFlg As String = "TIMEFLG"

    ''' <summary>終日フラグ要素</summary>
    Public Const TodoItemTableAllDayFlg As String = "ALLDAYFLG"

    ''' <summary>メモ要素</summary>
    Public Const TodoItemTableMemo As String = "MEMO"

    ''' <summary>色設定要素</summary>
    Public Const TodoItemTableIcropColor As String = "ICROPCOLOR"

    ''' <summary>親子区分要素</summary>
    Public Const TodoItemTableParentDiv As String = "ParentDiv"

    ''' <summary>繰り返し周期要素</summary>
    Public Const TodoItemTableRruleFreq As String = "RRULE_FREQ"

    ''' <summary>繰り返し間隔要素</summary>
    Public Const TodoItemTableRruleInterval As String = "RRULE_INTERVAL"

    ''' <summary>繰り返し終了日要素</summary>
    Public Const TodoItemTableRruleUntil As String = "RRULE_UNTIL"

    ''' <summary>繰り返しテキスト要素</summary>
    Public Const TodoItemTableRruleText As String = "RRULE_TEXT"

    ''' <summary>完了フラグ要素</summary>
    Public Const TodoItemTableCompletionFlg As String = "COMPLETIONFLG"

    ''' <summary>完了日要素</summary>
    Public Const TodoItemTableCompletionDate As String = "COMPLETIONDATE"

    ''' <summary>削除フラグ要素</summary>
    Public Const TodoItemTableDelFlg As String = "DELFLG"

    ''' <summary>削除日要素</summary>
    Public Const TodoItemTableDelDate As String = "DELDATE"

    ''' <summary>作成日要素</summary>
    Public Const TodoItemTableCreateDate As String = "CREATEDATE"

    ''' <summary>更新日要素</summary>
    Public Const TodoItemTableUpdateDate As String = "UPDATEDATE"

    ''' <summary>作成アカウント要素</summary>
    Public Const TodoItemTableCreateAccount As String = "CREATEACCOUNT"

    ''' <summary>更新アカウント要素</summary>
    Public Const TodoItemTableUpdateAccount As String = "UPDATEACCOUNT"

    ''' <summary>作成機能ID要素</summary>
    Public Const TodoItemTableCreateId As String = "CREATEID"

    ''' <summary>更新機能ID要素</summary>
    Public Const TodoItemTableUpdateId As String = "UPDATEID"

    ' カレンダーTodoアラームテーブル
    ''' <summary>TodoID要素</summary>
    Public Const TodoAlarmTableTodoId As String = "TODOID"

    ''' <summary>シーケンス番号要素</summary>
    Public Const TodoAlarmTableSeqNo As String = "SEQNO"

    ''' <summary>起動タイミング要素</summary>
    Public Const TodoAlarmTableStartTrigger As String = "STARTTRIGGER"

    ''' <summary>作成日要素</summary>
    Public Const TodoAlarmTableCreateDate As String = "CREATEDATE"

    ''' <summary>更新日要素</summary>
    Public Const TodoAlarmTableUpdateDate As String = "UPDATEDATE"

    ''' <summary>作成アカウント要素</summary>
    Public Const TodoAlarmTableCreateAccount As String = "CREATEACCOUNT"

    ''' <summary>更新アカウント要素</summary>
    Public Const TodoAlarmTableUpdateAccount As String = "UPDATEACCOUNT"

    ''' <summary>作成機能ID要素</summary>
    Public Const TodoAlarmTableCreateId As String = "CREATEID"

    ''' <summary>更新機能ID要素</summary>
    Public Const TodoAlarmTableUpdateId As String = "UPDATEID"

    'カレンダーイベント情報テーブル
    ''' <summary>イベントID要素</summary>
    Public Const EventItemTableEventId As String = "EVENTID"

    ''' <summary>カレンダーID要素</summary>
    Public Const EventItemTableCalId As String = "CALID"

    ''' <summary>TodoID要素</summary>
    Public Const EventItemTableTodoId As String = "TODOID"

    ''' <summary>ユニークID要素</summary>
    Public Const EventItemTableUniqueId As String = "UNIQUEID"

    ''' <summary>リカレンスID要素</summary>
    Public Const EventItemTableRecurrenceId As String = "RECURRENCEID"

    ''' <summary>変更シーケンス要素</summary>
    Public Const EventItemTableChgSeqNo As String = "CHGSEQNO"

    ''' <summary>活動担当スタッフ店舗コード要素</summary>
    Public Const EventItemTableActStaffStrCD As String = "ACTSTAFFSTRCD"

    ''' <summary>活動スタッフコード要素</summary>
    Public Const EventItemTableActStaffCD As String = "ACTSTAFFCD"

    ''' <summary>受付担当スタッフ店舗コード要素</summary>
    Public Const EventItemTableRecStaffStrCD As String = "RECSTAFFSTRCD"

    ''' <summary>受付担当スタッフコード要素</summary>
    Public Const EventItemTableRecStaffCD As String = "RECSTAFFCD"

    ''' <summary>接触方法No要素</summary>
    Public Const EventItemTableContactNo As String = "CONTACTNO"

    ''' <summary>タイトル要素</summary>
    Public Const EventItemTableSummary As String = "SUMMARY"

    ''' <summary>開始日時要素</summary>
    Public Const EventItemTableStartTime As String = "STARTTIME"

    ''' <summary>終了日時要素</summary>
    Public Const EventItemTableEndTime As String = "ENDTIME"

    ''' <summary>時刻指定フラグ要素</summary>
    Public Const EventItemTableTimeFlg As String = "TIMEFLG"

    ''' <summary>終日フラグ要素</summary>
    Public Const EventItemTableAllDayFlg As String = "ALLDAYFLG"

    ''' <summary>メモ要素</summary>
    Public Const EventItemTableMemo As String = "MEMO"

    ''' <summary>色設定要素</summary>
    Public Const EventItemTableIcropColor As String = "ICROPCOLOR"

    ''' <summary>繰り返し周期要素</summary>
    Public Const EventItemTableRruleFreq As String = "RRULE_FREQ"

    ''' <summary>繰り返し間隔要素</summary>
    Public Const EventItemTableRruleInterVal As String = "RRULE_INTERVAL"

    ''' <summary>繰り返し終了日要素</summary>
    Public Const EventItemTableRruleUntil As String = "RRULE_UNTIL"

    ''' <summary>繰り返しテキスト要素</summary>
    Public Const EventItemTableRruleText As String = "RRULE_TEXT"

    ''' <summary>場所要素</summary>
    Public Const EventItemTableLocation As String = "LOCATION"

    ''' <summary>連絡先要素</summary>
    Public Const EventItemTableAttenDee As String = "ATTENDEE"

    ''' <summary>空き時間要素</summary>
    Public Const EventItemTableTransp As String = "TRANSP"

    ''' <summary>URL要素</summary>
    Public Const EventItemTableUrl As String = "URL"

    ''' <summary>削除フラグ要素</summary>
    Public Const EventItemTableDelFlg As String = "DELFLG"

    ''' <summary>削除日要素</summary>
    Public Const EventItemTableDelDate As String = "DELDATE"

    ''' <summary>作成日要素</summary>
    Public Const EventItemTableCreateDate As String = "CREATEDATE"

    ''' <summary>更新日要素</summary>
    Public Const EventItemTableUpdateDate As String = "UPDATEDATE"

    ''' <summary>作成アカウント要素</summary>
    Public Const EventItemTableCreateAccount As String = "CREATEACCOUNT"

    ''' <summary>更新アカウント要素</summary>
    Public Const EventItemTableUpdateAccount As String = "UPDATEACCOUNT"

    ''' <summary>作成機能ID要素</summary>
    Public Const EventItemTableCreateId As String = "CREATEID"

    ''' <summary>更新機能ID要素</summary>
    Public Const EventItemTableUpdateId As String = "UPDATEID"


    ' カレンダーイベントアラームテーブル
    ''' <summary>EventID要素</summary>
    Public Const EventAlarmTableEventId As String = "EVENTID"

    ''' <summary>シーケンス番号要素</summary>
    Public Const EventAlarmTableSeqNo As String = "SEQNO"

    ''' <summary>起動タイミング要素</summary>
    Public Const EventAlarmTableStartTrigger As String = "STARTTRIGGER"

    ''' <summary>作成日要素</summary>
    Public Const EventAlarmTableCreateDate As String = "CREATEDATE"

    ''' <summary>更新日要素</summary>
    Public Const EventAlarmTableUpdateDate As String = "UPDATEDATE"

    ''' <summary>作成アカウント要素</summary>
    Public Const EventAlarmTableCreateAccount As String = "CREATEACCOUNT"

    ''' <summary>更新アカウント要素</summary>
    Public Const EventAlarmTableUpdateAccount As String = "UPDATEACCOUNT"

    ''' <summary>作成機能ID要素</summary>
    Public Const EventAlarmTableCreateId As String = "CREATEID"

    ''' <summary>更新機能ID要素</summary>
    Public Const EventAlarmTableUpdateId As String = "UPDATEID"

    ' カレンダーTodo繰り返し除外日テーブル
    ''' <summary>TodoID要素</summary>
    Public Const TodoExDateTableTodoId As String = "TODOID"

    ''' <summary>シーケンス番号要素</summary>
    Public Const TodoExDateTableSeqNo As String = "SEQNO"

    ''' <summary>除外日要素</summary>
    Public Const TodoExDateTableEXDate As String = "EXDATE"

    ''' <summary>作成アカウント要素</summary>
    Public Const TodoExDateTableCreateAccount As String = "CREATEACCOUNT"

    ''' <summary>更新アカウント要素</summary>
    Public Const TodoExDateTableUpdateAccount As String = "UPDATEACCOUNT"

    ''' <summary>作成機能ID要素</summary>
    Public Const TodoExDateTableCreateId As String = "CREATEID"

    ''' <summary>更新機能ID要素</summary>
    Public Const TodoExDateTableUpdateId As String = "UPDATEID"

    ' カレンダーイベント繰り返し除外日
    ''' <summary>EventID要素</summary>
    Public Const EventExDateTableEventId As String = "EVENTID"

    ''' <summary>シーケンス番号要素</summary>
    Public Const EventExDateTableSeqNo As String = "SEQNO"

    ''' <summary>除外日要素</summary>
    Public Const EventExDateTableEXDate As String = "EXDATE"

    ''' <summary>作成アカウント要素</summary>
    Public Const EventExDateTableCreateAccount As String = "CREATEACCOUNT"

    ''' <summary>更新アカウント要素</summary>
    Public Const EventExDateTableUpdateAccount As String = "UPDATEACCOUNT"

    ''' <summary>作成機能ID要素</summary>
    Public Const EventExDateTableCreateId As String = "CREATEID"

    ''' <summary>更新機能ID要素</summary>
    Public Const EventExDateTableUpdateId As String = "UPDATEID"

    ' カレンダーアドレス最終更新日
    ''' <summary>STAFFCD要素</summary>
    Public Const LastModifyDateTableStaffCD As String = "STAFFCD"

    ''' <summary>CALUPDATEDATE要素</summary>
    Public Const LastModifyDateTableCalUpdateDate As String = "CALUPDATEDATE"

    ''' <summary>CARDUPDATEDATE要素</summary>
    Public Const LastModifyDateTableCardUpdateDate As String = "CARDUPDATEDATE"

    ''' <summary>CREATEDATE要素</summary>
    Public Const LastModifyDateTableCreateDate As String = "CREATEDATE"

    ''' <summary>UPDATEDATE要素</summary>
    Public Const LastModifyDateTableUpdateDate As String = "UPDATEDATE"

    ''' <summary>CREATEACCOUNT要素</summary>
    Public Const LastModifyDateTableCreateAccount As String = "CREATEACCOUNT"

    ''' <summary>UPDATEACCOUNT要素</summary>
    Public Const LastModifyDateTableUpdateAccount As String = "UPDATEACCOUNT"

    ''' <summary>CREATEID要素</summary>
    Public Const LastModifyDateTableCreateId As String = "CREATEID"

    ''' <summary>UPDATEID要素</summary>
    Public Const LastModifyDateTableUpdateId As String = "UPDATEID"

    ''' <summary>UPDATEID要素</summary>
    Public Const SequencdIdCountryCode As String = "COUNTRYCODE"


End Class





