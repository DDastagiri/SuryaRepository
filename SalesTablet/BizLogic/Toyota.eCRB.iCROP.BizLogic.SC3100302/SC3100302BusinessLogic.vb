Imports System.Globalization
Imports System.Reflection.MethodBase
Imports System.Xml
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic

Public Class SC3100302BusinessLogic
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

    '
    Private Const OPERATIONCODE_ICON_BASE_PATH As String = "~/Styles/Images/Authority/"

    '2013/06/30 TCS 坂井 2013/10対応版 既存流用 START
    Private Const CST_VCL_TYPE_1 As String = "1"
    Private Const CST_CST_CLASS_1 As String = "1"
    '2013/06/30 TCS 坂井 2013/10対応版 既存流用 END
#End Region

#Region "コンストラクタ"
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


#Region "Public"
    Public Function SelectVisitActualList() As SC3100302DataSet.SC3100302VisitActualDataTable
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0} Start", GetCurrentMethod().Name))

        Using ta As New SC3100302DataTableTableAdapter(Me.DlrCd, Me.StrCd, Me.UserId)
            '来店実績一覧を取得
            Dim dt As SC3100302DataSet.SC3100302VisitActualDataTable = ta.SelectVisitActualList()

            '来店実績一覧を編集する
            EditVisitActualList(dt)

            Return dt
        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0} End", GetCurrentMethod().Name))

    End Function

#End Region


#Region "Private"
    ''' <summary>
    ''' 来店実績一覧編集
    ''' </summary>
    ''' <parameter>
    ''' SC3100302DataSet.SC3100302VisitActualDataTable
    ''' </parameter>
    ''' <remarks>
    ''' 来店実績情報を編集する（次回活動の設定、アイコンパス設定)
    ''' </remarks>
    Private Sub EditVisitActualList(ByVal actualList As SC3100302DataSet.SC3100302VisitActualDataTable)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0} Start", GetCurrentMethod().Name))

        Dim schedule As XDocument = GetSchedule()
        Dim contactIconList As SC3100302DataSet.SC3100302ContactIconDataTable
        Dim afterFollowIconList As SC3100302DataSet.SC3100302AfterFollowtIconDataTable

        Using ta As New SC3100302DataTableTableAdapter(Me.DlrCd, Me.StrCd, Me.UserId)
            '接触方法アイコンを取得する 。
            contactIconList = ta.SelectContactIcon
            '受注後工程アイコンを取得する。
            afterFollowIconList = ta.SelectAfterFollowtIcon
        End Using

        For Each dr As SC3100302DataSet.SC3100302VisitActualRow In actualList
            '顧客情報を設定する。
            SetCustomerInfo(dr)

            '見込み度を設定する。
            SetStatus(dr)

            '次回活動情報を設定する。
            SetNextActivity(dr, schedule, contactIconList, afterFollowIconList)

            '遅れ状況を設定する
            SetDelayStatus(dr)

            '一次対応者権限アイコンのパスを編集する
            EditTempStaff(dr)

        Next

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0} End", GetCurrentMethod().Name))

    End Sub

    ''' <summary>
    ''' カレンダー情報取得
    ''' </summary>
    ''' <parameter>
    ''' Nothing
    ''' </parameter>
    ''' <returns>
    ''' XmlDocument
    ''' </returns>
    ''' <remarks>
    ''' CalDAVよりカレンダー情報を取得する
    ''' </remarks>
    Private Function GetSchedule() As XDocument
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0} Start", GetCurrentMethod().Name))
        Dim nowDate As Date = DateTimeFunc.Now(Me.DlrCd)
        Dim startDate As Date = nowDate.Date
        Dim endDate As Date = nowDate.AddYears(1000).AddDays(1).Date.AddSeconds(-1)
        Dim schedule As String

        'CalDAVからカレンダー情報を取得する。
        Using calDAV As New Toyota.eCRB.iCROP.BizLogic.CalenderXmlCreateClass.BizLogic.ClassLibraryBusinessLogic()
            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
            "Call Start {0} params:startDate=[{1}],endDate=[{2}],UserId=[{3}],OperationCode=[{4}]", "calDAV.GetCalender", _
            startDate, endDate, Me.UserId, Me.OperationCode))
            schedule = calDAV.GetCalender(startDate, endDate, Me.UserId, Me.OperationCode)
            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
              "Call End {0} ", "calDAV.GetCalender"))
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "CalDAV ReturnValue=[{0}]", schedule))
            Return XDocument.Parse(schedule)
        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0} End", GetCurrentMethod().Name))

    End Function

    ''' <summary>
    ''' 顧客情報設定
    ''' </summary>
    ''' <parameter>
    ''' SC3100302DataSet.SC3100302VisitActualRow
    ''' </parameter>
    ''' <remarks>
    ''' 顧客情報を設定する。
    ''' </remarks>
    Private Sub SetCustomerInfo(ByVal actualListRow As SC3100302DataSet.SC3100302VisitActualRow)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0} Start", GetCurrentMethod().Name))
        Using ta As New SC3100302DataTableTableAdapter(Me.DlrCd, Me.StrCd, Me.UserId)
            '敬称位置取得
            Dim sysEnv As New SystemEnvSetting
            Dim nameTitlePosition As String = sysEnv.GetSystemEnvSetting(NAME_TITLE_POSITION).PARAMVALUE

            '2013/06/30 TCS 坂井 2013/10対応版 既存流用 START
            '顧客と車両の情報を取得
            Dim data As SC3100302DataSet.SC3100302CustomerNameDataTable = ta.SelectCustomerVehicleInfo(actualListRow.FLLWUPBOX_SEQNO)
            Dim dt As SC3100302DataSet.SC3100302CustomerNameDataTable

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
            End If
            '2013/06/30 TCS 坂井 2013/10対応版 既存流用 END

            '敬称付き名称作成
            Dim customerNameWithTitle As New Text.StringBuilder
            If NAME_TITLE_POSITION_PREFIX.Equals(nameTitlePosition) Then
                customerNameWithTitle.Append(dt(0).NAMETITLE)
                customerNameWithTitle.Append(" ")
            End If

            customerNameWithTitle.Append(dt(0).NAME)

            If NAME_TITLE_POSITION_SUFIX.Equals(nameTitlePosition) Then
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

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0} End", GetCurrentMethod().Name))

    End Sub

    ''' <summary>
    ''' 見込み度設定
    ''' </summary>
    ''' <parameters>
    ''' <parameter>
    ''' SC3100302DataSet.SC3100302VisitActualRow
    ''' </parameter>
    ''' </parameters>
    ''' <remarks>
    ''' 見込み度を設定する。
    ''' </remarks>
    Private Sub SetStatus(ByVal actualListRow As SC3100302DataSet.SC3100302VisitActualRow)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0} Start", GetCurrentMethod().Name))
        Dim statusTable As ActivityInfoDataSet.ActivityInfoGetStatusToDataTable
        Dim status As String

        'ステータスを取得する
        Using param As New ActivityInfoDataSet.ActivityInfoGetStatusFromDataTable
            Dim paramRow As ActivityInfoDataSet.ActivityInfoGetStatusFromRow = param.NewActivityInfoGetStatusFromRow
            paramRow.DLRCD = actualListRow.DLRCD
            paramRow.STRCD = actualListRow.STRCD
            paramRow.FLLWUPBOX_SEQNO = actualListRow.FLLWUPBOX_SEQNO
            param.AddActivityInfoGetStatusFromRow(paramRow)

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "Call {0} Start Param:DLRCD=[{1}] STRCD=[{2}] FLLWUPBOX_SEQNO=[{3}]", _
              "ActivityInfoBusinessLogic.GetStatus", paramRow.DLRCD, paramRow.STRCD, paramRow.FLLWUPBOX_SEQNO))

            statusTable = ActivityInfoBusinessLogic.GetStatus(param)

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "Call {0} End ReturnValue:Count=[{1}]", "ActivityInfoBusinessLogic.GetStatus", statusTable.Count))

        End Using


        If statusTable.Count > 0 Then
            '2012/3/19 MOD TCS 竹内 STR
            'status = statusTable(0).CRACTRESULT
            Select Case statusTable(0).CRACTRESULT
                Case STATUS_HOT, STATUS_WARM, STATUS_SUCCESS, STATUS_GIVEUP
                    status = statusTable(0).CRACTRESULT
                Case Else
                    status = STATUS_COLD
            End Select
            '2012/3/19 MOD TCS 竹内 END
        Else
            status = STATUS_NEW
        End If

        If STATUS_SUCCESS.Equals(status) Then
            If actualListRow.IsVCLASIDATENull = False Then
                Select Case actualListRow.WAITING_OBJECT
                    Case SUCCESS_ALLOCATION
                        status = STATUS_SUCCESS
                    Case SUCCESS_PAYMENT
                        status = SUCCESS_ALLOCATION
                    Case SUCCESS_DELIVERY
                        status = SUCCESS_PAYMENT
                    Case Else
                        If actualListRow.IsVCLDELIDATENull = False Then
                            status = SUCCESS_DELIVERY
                        End If
                End Select
            End If
        End If

        '見込み度を設定する
        actualListRow.STATUS = status
        '見込み度アイコンを設定する
        Select Case status
            Case STATUS_NEW
                actualListRow.STATUS_ICON = STATUS_ICON_NEW
            Case STATUS_HOT
                actualListRow.STATUS_ICON = STATUS_ICON_HOT
            Case STATUS_WARM
                actualListRow.STATUS_ICON = STATUS_ICON_WARM
            Case STATUS_SUCCESS
                actualListRow.STATUS_ICON = STATUS_ICON_SUCCESS
            Case STATUS_COLD
                actualListRow.STATUS_ICON = STATUS_ICON_COLD
            Case STATUS_GIVEUP
                actualListRow.STATUS_ICON = STATUS_ICON_GIVEUP
            Case SUCCESS_ALLOCATION
                actualListRow.STATUS_ICON = STATUS_ICON_ALLOCATION
            Case SUCCESS_PAYMENT
                actualListRow.STATUS_ICON = STATUS_ICON_PAYMENT
            Case SUCCESS_DELIVERY
                actualListRow.STATUS_ICON = STATUS_ICON_DELIVERY
        End Select

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0} End", GetCurrentMethod().Name))

    End Sub


    ''' <summary>
    ''' 次回活動情報設定
    ''' </summary>
    ''' <parameters>
    ''' <parameter>
    ''' SC3100302DataSet.SC3100302VisitActualRow
    ''' </parameter>
    ''' <parameter>
    ''' XmlDocument
    ''' </parameter>
    ''' </parameters>
    ''' <remarks>
    ''' 次回活動情報を設定する。
    ''' </remarks>
    Private Sub SetNextActivity(ByVal actualListRow As SC3100302DataSet.SC3100302VisitActualRow,
        ByVal xmlSchedule As XDocument,
        ByVal contactIconList As SC3100302DataSet.SC3100302ContactIconDataTable,
        ByVal afterFollowIconList As SC3100302DataSet.SC3100302AfterFollowtIconDataTable)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0} Start", GetCurrentMethod().Name))
        'スケジュール区分取得
        Dim xDetail As XElement = (
         From detail In xmlSchedule.<Calendar>.<Detail>
         Where detail.<Common>.<ScheduleID>.Value = actualListRow.FLLWUPBOX_SEQNO.ToString(CultureInfo.InvariantCulture)
         ).FirstOrDefault

        '2012/3/15 MOD TCS 竹内 STR
        'If IsNothing(xDetail) Then
        If REGISTED_ACT_RESULT.Equals(actualListRow.REGISTFLG) = False OrElse IsNothing(xDetail) Then           'フォローアップボックスに紐付くスケジュールがない場合は、処理終了
            '2012/3/15 MOD TCS 竹内 END
            actualListRow.NEXT_ACTION = String.Empty
            actualListRow.NEXT_ACTION_DATE = String.Empty
            actualListRow.NEXT_ACTION_ICON = String.Empty
            Return
        End If


        'スケジュール区分取得
        Dim scheduleDiv As String = xDetail.<Common>.<ScheduleDiv>.Value
        Dim nextActivity As String
        Dim nextActivityDate As String
        Dim nextActivityIcon As String
        Dim nextTimeFlg As String

        If SCHEDULE_DIV_VISIT.Equals(scheduleDiv) Then
            '20120321 MOD STR TCS竹内
            Select Case actualListRow.STATUS
                Case STATUS_SUCCESS, STATUS_GIVEUP
                    '成約、断念の場合は、次回活動を表示しない
                    actualListRow.NEXT_ACTION = String.Empty
                    actualListRow.NEXT_ACTION_DATE = String.Empty
                    actualListRow.NEXT_ACTION_ICON = String.Empty
                    Return
                Case Else
                    'NONE
            End Select
            '20120321 MOD END TCS竹内
            '20120319 MOD STR TCS竹内
            'VTODOに来店フォローと来店がある場合は、来店フォローを次回活動とする
            'Dim xTodoForVisit = (
            '   From todo In xDetail.<VTodo>
            '   Order By todo.<ContactNo>.Value Ascending
            '   Select todo.<ContactNo>, todo.<DtStart>, todo.<Due>, todo.<TimeFlg>
            '   ).FirstOrDefault
            Dim xTodoForVisit = (
               From todo In xDetail.<VTodo>
               Order By todo.<CompFlg>.Value Ascending, todo.<ContactNo>.Value Ascending
               Select todo.<ContactNo>, todo.<DtStart>, todo.<Due>, todo.<TimeFlg>
               ).FirstOrDefault
            '20120319 MOD END TCS竹内
            '次回活動を設定
            nextActivity = xTodoForVisit.ContactNo.Value
            '次回活動日を設定
            If IsNothing(xTodoForVisit.DtStart.Value) Then
                nextActivityDate = xTodoForVisit.Due.Value
            Else
                nextActivityDate = xTodoForVisit.DtStart.Value
            End If
            '時間指定フラグ
            nextTimeFlg = xTodoForVisit.TimeFlg.Value
            '次回活動アイコンを設定
            Dim iconPathForContact As String = (
              From n In contactIconList
               Where n.CONTACTNO = nextActivity
               Select n.ICONPATH
              ).FirstOrDefault

            nextActivityIcon = System.Web.VirtualPathUtility.ToAbsolute(iconPathForContact)

        Else

            Dim xTodoForAfterFollow = (
              From todo In xDetail.<VTodo>
              Where todo.<CompFlg>.Value = TODO_STATUS_NOT_COMPLETE
              Order By todo.<ProcessDiv>.Value Ascending
              Select todo.<ProcessDiv>, todo.<DtStart>, todo.<Due>, todo.<TimeFlg>
              ).FirstOrDefault

            If IsNothing(xTodoForAfterFollow) Then
                '受注後工程がすべて完了している場合は、次回活動を表示しない
                actualListRow.NEXT_ACTION = String.Empty
                actualListRow.NEXT_ACTION_DATE = String.Empty
                actualListRow.NEXT_ACTION_ICON = String.Empty
                Return
            End If
            '次回活動を設定
            nextActivity = xTodoForAfterFollow.ProcessDiv.Value
            '次回活動日を設定
            If IsNothing(xTodoForAfterFollow.DtStart.Value) Then
                nextActivityDate = xTodoForAfterFollow.Due.Value
            Else
                nextActivityDate = xTodoForAfterFollow.DtStart.Value
            End If
            '時間指定フラグ
            nextTimeFlg = xTodoForAfterFollow.TimeFlg.Value
            '次回活動アイコンを取得する
            Dim iconPathForAfterFollow As String = (
             From n In afterFollowIconList
             Where n.STARTPROCESSCD = nextActivity
             Select n.ICON_TODOTIP
            ).FirstOrDefault

            '次回活動アイコンを設定
            nextActivityIcon = iconPathForAfterFollow

        End If

        With actualListRow
            .SCHEDULEDIV = scheduleDiv
            .NEXT_ACTION = nextActivity
            Dim wkDate As Date
            '20120326 MOD STR TCS竹内
            'If Date.TryParseExact(nextActivityDate, "yyyy/MM/dd hh:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, wkDate) Then
            If Date.TryParseExact(nextActivityDate, "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, wkDate) Then
                '20120326 MOD END TCS竹内
                If CALDAV_TIME_FLG.Equals(nextTimeFlg) Then
                    '20120326 MOD STR TCS竹内
                    '.NEXT_ACTION_DATE = DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, DateTime.ParseExact(nextActivityDate, "yyyy/MM/dd hh:mm:ss", CultureInfo.InvariantCulture), Me.DlrCd)
                    .NEXT_ACTION_DATE = DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, DateTime.ParseExact(nextActivityDate, "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture), Me.DlrCd)
                    '20120326 MOD END TCS竹内
                Else
                    '20120326 MOD STR TCS竹内
                    '.NEXT_ACTION_DATE = DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, DateTime.ParseExact(nextActivityDate, "yyyy/MM/dd hh:mm:ss", CultureInfo.InvariantCulture), DateTimeFunc.Now(Me.DlrCd), Me.DlrCd, False)
                    .NEXT_ACTION_DATE = DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, DateTime.ParseExact(nextActivityDate, "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture), DateTimeFunc.Now(Me.DlrCd), Me.DlrCd, False)
                    '20120326 MOD END TCS竹内
                End If
            Else
                .NEXT_ACTION_DATE = String.Empty
            End If

            .NEXT_ACTION_ICON = nextActivityIcon
        End With


        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0} End", GetCurrentMethod().Name))

    End Sub

    ''' <summary>
    ''' 遅れ状況設定
    ''' </summary>
    ''' <parameters>
    ''' <parameter>
    ''' SC3100302DataSet.SC3100302VisitActualRow
    ''' </parameter>
    ''' </parameters>
    ''' <remarks>
    ''' 遅れ状況を設定する
    ''' </remarks>
    Private Sub SetDelayStatus(ByVal actualListRow As SC3100302DataSet.SC3100302VisitActualRow)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0} Start", GetCurrentMethod().Name))
        Dim nowDate As Date = DateTimeFunc.Now(Me.DlrCd).Date
        Dim startDate As Date = actualListRow.STARTTIME.Date

        actualListRow.SALES_STATUS = String.Empty

        If nowDate.CompareTo(startDate) > 0 Then
            '過去
            '遅れ状況を設定する
            actualListRow.DELAY_STATUS = DELAY_STATUS_DELAY
            '商談開始日を編集する
            actualListRow.SALES_DATE = DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, actualListRow.STARTTIME, Me.DlrCd)
            actualListRow.SALES_STATUS = EDIT_MODE
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


        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0} End", GetCurrentMethod().Name))
    End Sub


    ''' <summary>
    ''' 一次対応者情報編集
    ''' </summary>
    ''' <parameters>
    ''' <parameter>
    ''' SC3100302DataSet.SC3100302VisitActualRow
    ''' </parameter>
    ''' </parameters>
    ''' <remarks>
    ''' 一次対応者の情報を編集する
    ''' </remarks>
    Private Sub EditTempStaff(ByVal actualListRow As SC3100302DataSet.SC3100302VisitActualRow)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0} Start", GetCurrentMethod().Name))

        If actualListRow.ACCOUNT_PLAN.Equals(actualListRow.ACTUALACCOUNT) Then
            '一次対応者がいない場合は、一次対応者情報をクリアする
            actualListRow.TEMP_STAFFNAME = String.Empty
            actualListRow.TEMP_STAFF_OPERATIONCODE = String.Empty
            actualListRow.TEMP_STAFF_OPERATIONCODE_ICON = String.Empty
            Return
        End If
        '一次対応者権限アイコンのパスを編集する

        actualListRow.TEMP_STAFF_OPERATIONCODE_ICON = OPERATIONCODE_ICON_BASE_PATH & actualListRow.TEMP_STAFF_OPERATIONCODE_ICON


        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0} End", GetCurrentMethod().Name))
    End Sub
#End Region

End Class
