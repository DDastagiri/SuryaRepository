Imports System.Text
Imports Toyota.eCRB.SystemFrameworks.Configuration
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Oracle.DataAccess.Client

Namespace DataAccess

    Public Class SelectCreateCalendarDataTable
        Inherits Global.System.ComponentModel.Component

        ' バインド変数
        Private Const Sql_Bind_StartTime As String = "STARTTIME"
        Private Const Sql_Bind_EndTime As String = "ENDTIME"
        ' 2014/04/25 SKFC 上田 NEXTSTEP_CALDAV START
        Private Const Sql_Bind_StartTime2 As String = "STARTTIME2"
        Private Const Sql_Bind_EndTime2 As String = "ENDTIME2"
        ' 2014/04/25 SKFC 上田 NEXTSTEP_CALDAV END
        Private Const Sql_Bind_StaffCode As String = "STAFFCODE"
        Private Const Sql_SalesStaffCode As String = "8"

        Private Const Sql_Bind_DeclearCode As String = "DLRCD"
        Private Const Sql_Bind_BranchCode As String = "STRCD"
        Private Const Sql_Bind_ScheduleID As String = "SCHEDULEID"
        Private Const Sql_Bind_ScheduleDiv As String = "SCHEDULEDIV"

        ''' <summary>
        ''' カレンダーXMLを作成する値を取得します。
        ''' </summary>
        ''' <param name="startTime">開始時間1</param>
        ''' <param name="endTime">終了時間1</param>
        ''' <param name="startTime2">開始時間2</param>
        ''' <param name="endTime2">終了時間2</param>
        ''' <param name="staffCode">スタッフコード</param>
        ''' <param name="permission">権利</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <history>2014/04/25 SKFC 上田 NEXTSTEP_CALDAV 引数変更</history>
        Public Function GetSelectCalendarTable(ByVal actionType As String, _
                                              ByVal startTime As DateTime, _
                                              ByVal endTime As DateTime, _
                                              ByVal startTime2 As DateTime, _
                                              ByVal endTime2 As DateTime, _
                                              ByVal staffCode As String, _
                                              ByVal permission As String) As CalenderXmlCreateClassDataSet.SelectCreateCalendarDataTableDataTable
            'Public Function GetSelectCalendarTable(ByVal startTime As DateTime, _
            '                                    ByVal endTime As DateTime, _
            '                                    ByVal staffCode As String, _
            '                                    ByVal permission As String) As CalenderXmlCreateClassDataSet.SelectCreateCalendarDataTableDataTable

            Dim sql As New StringBuilder

            Try

                Using query As New DBSelectQuery(Of CalenderXmlCreateClassDataSet.SelectCreateCalendarDataTableDataTable)("IC00001_001")

                    With sql
                        .Append("  SELECT /* CalenderXmlCreateClass_001 */ ")   '--------------------①
                        If Validation.Equals(Sql_SalesStaffCode, permission) Then
                            .Append("         /*+ INDEX(TOI IDX_CAL_TODOITEM_03) */ ")
                        Else
                            .Append("         /*+ INDEX(TOI IDX_CAL_TODOITEM_04) */ ")
                        End If
                        .Append("         '1'                 AS TODOEVENT_FLG")
                        .Append("       , TOI.CALID           AS CARENDAR_ID")
                        .Append("       , EVI.EVENTID         AS EVENT_ID ")         ' EVENTIDで並び替えることにより、TODOとEVENTの分離、アラーム部分の並び替えになる
                        .Append("       , TOI.TODOID          AS TODO_ID ")          ' TODOで並び替えることによって、複数あるTODOが固まるようにする
                        .Append("       , TOI.UNIQUEID        AS UNIQUE_ID")
                        .Append("       , ''                  AS CREATE_DATA_DIV")
                        .Append("       , CAL.DLRCD           AS DEALER_CODE")
                        .Append("       , CAL.STRCD           AS BRANCH_CODE")
                        .Append("       , CAL.SCHEDULEID      AS SCHEDULE_ID")
                        .Append("       , CAL.SCHEDULEDIV     AS SCHEDULE_DIV")
                        .Append("       , TOI.ACTSTAFFCD      AS SALES_STAFF_CODE")
                        .Append("       , TOI.RECSTAFFCD      AS SA_CODE")
                        .Append("       , CAL.CUSTOMERDIV     AS CUSTOMER_DIV")
                        .Append("       , CAL.CUSTCODE        AS CUSTOMER_CODE")
                        .Append("       , CAL.DMSID           AS DMS_ID")
                        .Append("       , CAL.CUSTNAME        AS CUSTOMER_NAME")
                        .Append("       , CAL.RECEPTIONDIV    AS RECEPTION_DIV")
                        .Append("       , TOI.CONTACTNO       AS CONTACT_NO")
                        .Append("       , TOI.SUMMARY         AS SUMMARY")
                        .Append("       , TO_CHAR(TOI.STARTTIME ,'YYYY/MM/DD HH24:MI:SS')       AS START_TIME")
                        .Append("       , TO_CHAR(TOI.ENDTIME ,'YYYY/MM/DD HH24:MI:SS')         AS END_TIME")
                        .Append("       , TOI.TIMEFLG         AS TIME_FLG")
                        .Append("       , TOI.ALLDAYFLG       AS ALLDAY_FLG")
                        .Append("       , TOI.MEMO            AS MEMO")
                        .Append("       , TOI.ICROPCOLOR      AS ICROPCOLOR")
                        .Append("       , TOI.COMPLETIONFLG   AS COMP_FLG")
                        .Append("       , TO_CHAR(TOI.UPDATEDATE  ,'YYYY/MM/DD HH24:MI:SS')     AS UPDATE_DATE")
                        .Append("       , TOA.STARTTRIGGER    AS ALARM_TRIGGER")
                        .Append("       , TOA.SEQNO           AS ALARM_SEQNO")
                        .Append("       , TOI.RRULEFLG        AS R_RULE_FLG")
                        .Append("       , TOI.RRULE_FREQ      AS R_RULE_FREQ")
                        .Append("       , TOI.RRULE_INTERVAL  AS R_RULE_INTERVAL")
                        .Append("       , TO_CHAR(TOI.RRULE_UNTIL ,'YYYY/MM/DD HH24:MI:SS')     AS R_RULE_UNTIL")
                        '2012/03/01 SKFC 加藤 【SALES_2】受注後工程の対応 START
                        .Append("       , TOI.PROCESSDIV      AS PROCESS_DIV")
                        '2012/03/01 SKFC 加藤 【SALES_2】受注後工程の対応 END
                        '2014/04/25 SKFC 上田 NEXTSTEP_CALDAV START
                        .Append("       , TOI.CONTACT_NAME     AS CONTACT_NAME")
                        .Append("       , TOI.ACT_ODR_NAME     AS ACT_ODR_NAME")
                        .Append("       , TOI.ODR_DIV          AS ODR_DIV")
                        .Append("       , TOI.AFTER_ODR_ACT_ID AS AFTER_ODR_ACT_ID")
                        '2014/04/25 SKFC 上田 NEXTSTEP_CALDAV END
                        .Append("    FROM TBL_CAL_TODOITEM    TOI")
                        .Append("       , TBL_CAL_ICROPINFO   CAL")
                        .Append("       , TBL_CAL_TODOALARM   TOA")
                        .Append("       , TBL_CAL_EVENTITEM   EVI")
                        .Append("   WHERE TOI.CALID = CAL.CALID")
                        .Append("     AND TOI.TODOID = TOA.TODOID(+)")
                        .Append("     AND TOI.TODOID = EVI.TODOID(+)")
                        .Append("     AND EVI.DELFLG(+) = '0'")
                        If Validation.Equals(Sql_SalesStaffCode, permission) Then
                            .Append("     AND TOI.ACTSTAFFCD = :STAFFCODE")
                        Else
                            .Append("     AND TOI.RECSTAFFCD = :STAFFCODE")
                        End If
                        .Append("     AND TOI.DELFLG = '0'")
                        '2012/03/15 SKFC 上田 【SALES_2】受注後工程の対応(改修) START
                        '.Append("     AND CAL.SCHEDULEDIV = '0' ")                  '0:来店予約
                        .Append("     AND CAL.SCHEDULEDIV IN ('0', '2') ")           '0:来店予約、'2':受注後工程
                        '2012/03/15 SKFC 上田 【SALES_2】受注後工程の対応(改修) END
                        .Append("     AND CAL.DELFLG = '0'")

                        '2014/04/25 SKFC 上田 NEXTSTEP_CALDAV START
                        Select Case actionType
                            Case ConstClass.ActionTypeToday
                                '2014/04/25 SKFC 上田 NEXTSTEP_CALDAV END
                                .Append("     AND ((TOI.STARTTIME <= :ENDTIME ")
                                .Append("     AND   TOI.STARTTIMEFLG = '1' ")
                                .Append("     AND   TOI.RRULEFLG = '0' ")
                                .Append("     AND   TOI.COMPLETIONFLG = '0')")
                                .Append("      OR  (TOI.ENDTIME <= :ENDTIME ")
                                .Append("     AND   TOI.STARTTIMEFLG = '0' ")
                                .Append("     AND   TOI.RRULEFLG = '0' ")
                                .Append("     AND   TOI.COMPLETIONFLG = '0')")
                                .Append("      OR  (:STARTTIME <= TOI.COMPLETIONDATE ")
                                .Append("     AND   TOI.COMPLETIONDATE <= :ENDTIME ")
                                .Append("     AND   TOI.COMPLETIONFLG = '1'))")               '1:完了フラグ
                                '2014/04/25 SKFC 上田 NEXTSTEP_CALDAV START
                            Case ConstClass.ActionTypeDone
                                .Append("     AND TOI.COMPLETIONDATE >= :STARTTIME2 ")
                                .Append("     AND TOI.COMPLETIONDATE <= :ENDTIME2 ")
                                .Append("     AND TOI.COMPLETIONFLG = '1'")               '1:完了フラグ
                            Case ConstClass.ActionTypeFuture
                                .Append("     AND ((TOI.STARTTIME >= :STARTTIME2 ")
                                .Append("     AND   TOI.STARTTIME <= :ENDTIME2 ")
                                .Append("     AND   TOI.STARTTIMEFLG = '1') ")
                                .Append("      OR  (TOI.ENDTIME >= :STARTTIME2 ")
                                .Append("     AND   TOI.ENDTIME <= :ENDTIME2 ")
                                .Append("     AND   TOI.STARTTIMEFLG = '0')) ")
                                .Append("     AND TOI.RRULEFLG = '0' ")
                                .Append("     AND TOI.COMPLETIONFLG = '0' ")
                        End Select
                        '2014/04/25 SKFC 上田 NEXTSTEP_CALDAV END

                        .Append("   UNION ALL")

                        .Append("  SELECT ")                                        '--------------------②
                        .Append("         '2'                 AS TODOEVENT_FLG")
                        .Append("       , EVI.CALID           AS CARENDAR_ID")
                        .Append("       , EVI.EVENTID         AS EVENT_ID")          ' EVENTIDで並び替えることにより、TODOとEVENTの分離、アラーム部分の並び替えになる
                        .Append("       , EVI.TODOID          AS TODO_ID")           ' TODOで並び替えることによって、複数あるTODOが固まるようにする
                        .Append("       , EVI.UNIQUEID        AS UNIQUE_ID")
                        .Append("       , ''                  AS CREATE_DATA_DIV")
                        .Append("       , CAL.DLRCD           AS DEALER_CODE")
                        .Append("       , CAL.STRCD           AS BRANCH_CODE")
                        .Append("       , CAL.SCHEDULEID      AS SCHEDULE_ID")
                        .Append("       , CAL.SCHEDULEDIV     AS SCHEDULE_DIV")
                        .Append("       , EVI.ACTSTAFFCD      AS SALES_STAFF_CODE")
                        .Append("       , EVI.RECSTAFFCD      AS SA_CODE")
                        .Append("       , CAL.CUSTOMERDIV     AS CUSTOMER_DIV")
                        .Append("       , CAL.CUSTCODE        AS CUSTOMER_CODE")
                        .Append("       , CAL.DMSID           AS DMS_ID")
                        .Append("       , CAL.CUSTNAME        AS CUSTOMER_NAME")
                        .Append("       , CAL.RECEPTIONDIV    AS RECEPTION_DIV")
                        .Append("       , EVI.CONTACTNO       AS CONTACT_NO")
                        .Append("       , EVI.SUMMARY         AS SUMMARY")
                        .Append("       , TO_CHAR(EVI.STARTTIME ,'YYYY/MM/DD HH24:MI:SS')       AS START_TIME")
                        .Append("       , TO_CHAR(EVI.ENDTIME ,'YYYY/MM/DD HH24:MI:SS')         AS END_TIME")
                        .Append("       , EVI.TIMEFLG         AS TIME_FLG")
                        .Append("       , EVI.ALLDAYFLG       AS ALLDAY_FLG")
                        .Append("       , EVI.MEMO            AS MEMO")
                        .Append("       , EVI.ICROPCOLOR      AS ICROPCOLOR")
                        .Append("       , ''                  AS COMP_FLG ")
                        .Append("       , TO_CHAR(EVI.UPDATEDATE ,'YYYY/MM/DD HH24:MI:SS')      AS UPDATE_DATE")
                        .Append("       , EVA.STARTTRIGGER    AS ALARM_TRIGGER")
                        .Append("       , EVA.SEQNO           AS ALARM_SEQNO")
                        .Append("       , EVI.RRULEFLG        AS R_RULE_FLG")
                        .Append("       , EVI.RRULE_FREQ      AS R_RULE_FREQ")
                        .Append("       , EVI.RRULE_INTERVAL  AS R_RULE_INTERVAL")
                        .Append("       , TO_CHAR(EVI.RRULE_UNTIL ,'YYYY/MM/DD HH24:MI:SS')     AS R_RULE_UNTIL")
                        '2012/03/01 SKFC 加藤 【SALES_2】受注後工程の対応 START
                        .Append("       , EVI.PROCESSDIV      AS PROCESS_DIV")
                        '2012/03/01 SKFC 加藤 【SALES_2】受注後工程の対応 END
                        '2014/04/25 SKFC 上田 NEXTSTEP_CALDAV START
                        .Append("       , EVI.CONTACT_NAME     AS CONTACT_NAME")
                        .Append("       , EVI.ACT_ODR_NAME     AS ACT_ODR_NAME")
                        .Append("       , EVI.ODR_DIV          AS ODR_DIV")
                        .Append("       , EVI.AFTER_ODR_ACT_ID AS AFTER_ODR_ACT_ID")
                        '2014/04/25 SKFC 上田 NEXTSTEP_CALDAV END
                        .Append("    FROM TBL_CAL_EVENTITEM   EVI")
                        .Append("       , TBL_CAL_ICROPINFO   CAL")
                        .Append("       , TBL_CAL_EVENTALARM  EVA")
                        .Append("   WHERE EVI.CALID = CAL.CALID(+)  ")
                        .Append("     AND EVI.EVENTID = EVA.EVENTID(+) ")
                        If Validation.Equals(Sql_SalesStaffCode, permission) Then
                            .Append("     AND EVI.ACTSTAFFCD = :STAFFCODE")
                        Else
                            .Append("     AND EVI.RECSTAFFCD = :STAFFCODE")
                        End If
                        .Append("     AND EVI.STARTTIME <= :ENDTIME ")
                        .Append("     AND :STARTTIME <= EVI.ENDTIME ")
                        .Append("     AND EVI.DELFLG = '0' ")
                        .Append("     AND EVI.RRULEFLG = '0'")
                        .Append("     AND ((CAL.DELFLG = '0'")
                        .Append("     AND NOT EXISTS(")
                        .Append("      SELECT 1")
                        .Append("        FROM TBL_CAL_TODOITEM   TOI2")
                        .Append("       WHERE TOI2.TODOID = EVI.TODOID")
                        .Append("         AND ((TOI2.COMPLETIONFLG = '1' ")
                        .Append("		  AND TOI2.COMPLETIONDATE < :STARTTIME)")
                        .Append("         OR TOI2.DELFLG = '1' )))")
                        .Append("      OR EVI.CALID = 'NATIVE')")

                        '2014/04/25 SKFC 上田 NEXTSTEP_CALDAV START
                        If Validation.Equals(actionType, ConstClass.ActionTypeToday) Or _
                           Validation.Equals(actionType, ConstClass.ActionTypeFuture) Then
                            '2014/04/25 SKFC 上田 NEXTSTEP_CALDAV END

                            .Append("   UNION ALL")                                     '--------------------③

                            .Append("  SELECT ")
                            .Append("         '1'                 AS TODOEVENT_FLG")
                            .Append("       , TOI.CALID           AS CARENDAR_ID")
                            .Append("       , EVI.EVENTID         AS EVENT_ID ")         ' EVENTIDで並び替えることにより、TODOとEVENTの分離、アラーム部分の並び替えになる
                            .Append("       , TOI.TODOID          AS TODO_ID ")          ' TODOで並び替えることによって、複数あるTODOが固まるようにする
                            .Append("       , TOI.UNIQUEID        AS UNIQUE_ID")
                            .Append("       , ''                  AS CREATE_DATA_DIV")
                            .Append("       , CAL.DLRCD           AS DEALER_CODE")
                            .Append("       , CAL.STRCD           AS BRANCH_CODE")
                            .Append("       , CAL.SCHEDULEID      AS SCHEDULE_ID")
                            .Append("       , CAL.SCHEDULEDIV     AS SCHEDULE_DIV")
                            .Append("       , TOI.ACTSTAFFCD      AS SALES_STAFF_CODE")
                            .Append("       , TOI.RECSTAFFCD      AS SA_CODE")
                            .Append("       , CAL.CUSTOMERDIV     AS CUSTOMER_DIV")
                            .Append("       , CAL.CUSTCODE        AS CUSTOMER_CODE")
                            .Append("       , CAL.DMSID           AS DMS_ID")
                            .Append("       , CAL.CUSTNAME        AS CUSTOMER_NAME")
                            .Append("       , CAL.RECEPTIONDIV    AS RECEPTION_DIV")
                            .Append("       , TOI.CONTACTNO       AS CONTACT_NO")
                            .Append("       , TOI.SUMMARY         AS SUMMARY")
                            .Append("       , TO_CHAR(TOI.STARTTIME ,'YYYY/MM/DD HH24:MI:SS')       AS START_TIME")
                            .Append("       , TO_CHAR(TOI.ENDTIME ,'YYYY/MM/DD HH24:MI:SS')         AS END_TIME")
                            .Append("       , TOI.TIMEFLG         AS TIME_FLG")
                            .Append("       , TOI.ALLDAYFLG       AS ALLDAY_FLG")
                            .Append("       , TOI.MEMO            AS MEMO")
                            .Append("       , TOI.ICROPCOLOR      AS ICROPCOLOR")
                            .Append("       , TOI.COMPLETIONFLG   AS COMP_FLG")
                            .Append("       , TO_CHAR(TOI.UPDATEDATE ,'YYYY/MM/DD HH24:MI:SS')      AS UPDATE_DATE")
                            .Append("       , TOA.STARTTRIGGER    AS ALARM_TRIGGER")
                            .Append("       , TOA.SEQNO           AS ALARM_SEQNO")
                            .Append("       , TOI.RRULEFLG        AS R_RULE_FLG")
                            .Append("       , TOI.RRULE_FREQ      AS R_RULE_FREQ")
                            .Append("       , TOI.RRULE_INTERVAL  AS R_RULE_INTERVAL")
                            .Append("       , TO_CHAR(TOI.RRULE_UNTIL ,'YYYY/MM/DD HH24:MI:SS')     AS R_RULE_UNTIL")
                            '2012/03/01 SKFC 加藤 【SALES_2】受注後工程の対応 START
                            .Append("       , TOI.PROCESSDIV      AS PROCESS_DIV")
                            '2012/03/01 SKFC 加藤 【SALES_2】受注後工程の対応 END
                            '2014/04/25 SKFC 上田 NEXTSTEP_CALDAV START
                            .Append("       , TOI.CONTACT_NAME     AS CONTACT_NAME")
                            .Append("       , TOI.ACT_ODR_NAME     AS ACT_ODR_NAME")
                            .Append("       , TOI.ODR_DIV          AS ODR_DIV")
                            .Append("       , TOI.AFTER_ODR_ACT_ID AS AFTER_ODR_ACT_ID")
                            '2014/04/25 SKFC 上田 NEXTSTEP_CALDAV END
                            .Append("    FROM TBL_CAL_TODOITEM    TOI")
                            .Append("       , TBL_CAL_ICROPINFO   CAL")
                            .Append("       , TBL_CAL_TODOALARM   TOA")
                            .Append("       , TBL_CAL_EVENTITEM   EVI")
                            .Append("   WHERE TOI.CALID = CAL.CALID(+) ")
                            .Append("     AND TOI.TODOID = TOA.TODOID(+)")
                            .Append("     AND TOI.TODOID = EVI.TODOID(+)")
                            .Append("     AND EVI.DELFLG(+) = '0'")
                            If Validation.Equals(Sql_SalesStaffCode, permission) Then
                                .Append("     AND TOI.ACTSTAFFCD = :STAFFCODE")
                            Else
                                .Append("     AND TOI.RECSTAFFCD = :STAFFCODE")
                            End If
                            .Append("     AND TOI.COMPLETIONFLG = '0'")                 '0:ToDO未完了
                            .Append("     AND TOI.DELFLG = '0'")                        '0:ToDO未削除
                            '2012/03/15 SKFC 上田 【SALES_2】受注後工程の対応(改修) START
                            '.Append("     AND CAL.SCHEDULEDIV = '0'")                   '0:来店予約
                            .Append("     AND CAL.SCHEDULEDIV IN ('0', '2') ")           '0:来店予約、'2':受注後工程
                            '2012/03/15 SKFC 上田 【SALES_2】受注後工程の対応(改修) END
                            .Append("     AND CAL.DELFLG = '0'")                        '0:ｶﾚﾝﾀﾞ未削除
                            .Append("     AND TOI.RRULEFLG = '1'")                      '0:ENENT削除
                            .Append("     AND TOI.RRULE_UNTIL  >= :STARTTIME")
                            '2014/04/25 SKFC 上田 NEXTSTEP_CALDAV START
                        End If
                        '2014/04/25 SKFC 上田 NEXTSTEP_CALDAV END

                            .Append("   UNION ALL")

                            .Append("  SELECT ")                                        '--------------------④
                            .Append("         '2'                 AS TODOEVENT_FLG")
                            .Append("       , EVI.CALID           AS CARENDAR_ID")
                            .Append("       , EVI.EVENTID         AS EVENT_ID")          ' EVENTIDで並び替えることにより、TODOとEVENTの分離、アラーム部分の並び替えになる
                            .Append("       , EVI.TODOID          AS TODO_ID")           ' TODOで並び替えることによって、複数あるTODOが固まるようにする
                            .Append("       , EVI.UNIQUEID        AS UNIQUE_ID")
                            .Append("       , ''                  AS CREATE_DATA_DIV")
                            .Append("       , CAL.DLRCD           AS DEALER_CODE")
                            .Append("       , CAL.STRCD           AS BRANCH_CODE")
                            .Append("       , CAL.SCHEDULEID      AS SCHEDULE_ID")
                            .Append("       , CAL.SCHEDULEDIV     AS SCHEDULE_DIV")
                            .Append("       , EVI.ACTSTAFFCD      AS SALES_STAFF_CODE")
                            .Append("       , EVI.RECSTAFFCD      AS SA_CODE")
                            .Append("       , CAL.CUSTOMERDIV     AS CUSTOMER_DIV")
                            .Append("       , CAL.CUSTCODE        AS CUSTOMER_CODE")
                            .Append("       , CAL.DMSID           AS DMS_ID")
                            .Append("       , CAL.CUSTNAME        AS CUSTOMER_NAME")
                            .Append("       , CAL.RECEPTIONDIV    AS RECEPTION_DIV")
                            .Append("       , EVI.CONTACTNO       AS CONTACT_NO")
                            .Append("       , EVI.SUMMARY         AS SUMMARY")
                            .Append("       , TO_CHAR(EVI.STARTTIME ,'YYYY/MM/DD HH24:MI:SS')       AS START_TIME")
                            .Append("       , TO_CHAR(EVI.ENDTIME ,'YYYY/MM/DD HH24:MI:SS')         AS END_TIME")
                            .Append("       , EVI.TIMEFLG         AS TIME_FLG")
                            .Append("       , EVI.ALLDAYFLG       AS ALLDAY_FLG")
                            .Append("       , EVI.MEMO            AS MEMO")
                            .Append("       , EVI.ICROPCOLOR      AS ICROPCOLOR")
                            .Append("       , ''                  AS COMP_FLG ")
                            .Append("       , TO_CHAR(EVI.UPDATEDATE ,'YYYY/MM/DD HH24:MI:SS')      AS UPDATE_DATE")
                            .Append("       , EVA.STARTTRIGGER    AS ALARM_TRIGGER")
                            .Append("       , EVA.SEQNO           AS ALARM_SEQNO")
                            .Append("       , EVI.RRULEFLG        AS R_RULE_FLG")
                            .Append("       , EVI.RRULE_FREQ      AS R_RULE_FREQ")
                            .Append("       , EVI.RRULE_INTERVAL  AS R_RULE_INTERVAL")
                            .Append("       , TO_CHAR(EVI.RRULE_UNTIL ,'YYYY/MM/DD HH24:MI:SS')     AS R_RULE_UNTIL")
                        '2012/03/01 SKFC 加藤 【SALES_2】受注後工程の対応 START
                            .Append("       , EVI.PROCESSDIV      AS PROCESS_DIV")
                        '2012/03/01 SKFC 加藤 【SALES_2】受注後工程の対応 END
                        '2014/04/25 SKFC 上田 NEXTSTEP_CALDAV START
                            .Append("       , EVI.CONTACT_NAME     AS CONTACT_NAME")
                        .Append("       , EVI.ACT_ODR_NAME     AS ACT_ODR_NAME")
                            .Append("       , EVI.ODR_DIV          AS ODR_DIV")
                            .Append("       , EVI.AFTER_ODR_ACT_ID AS AFTER_ODR_ACT_ID")
                        '2014/04/25 SKFC 上田 NEXTSTEP_CALDAV END
                            .Append("    FROM TBL_CAL_EVENTITEM   EVI")
                            .Append("       , TBL_CAL_ICROPINFO   CAL")
                            .Append("       , TBL_CAL_EVENTALARM  EVA")
                            .Append("   WHERE EVI.CALID = CAL.CALID(+) ")
                            .Append("     AND EVI.EVENTID = EVA.EVENTID(+) ")
                            If Validation.Equals(Sql_SalesStaffCode, permission) Then
                                .Append("     AND EVI.ACTSTAFFCD = :STAFFCODE")
                            Else
                                .Append("     AND EVI.RECSTAFFCD = :STAFFCODE")
                        End If
                            .Append("     AND EVI.RRULEFLG = '1'")
                            .Append("     AND EVI.RRULE_UNTIL  >= :STARTTIME")
                            .Append("     AND EVI.STARTTIME  <= :ENDTIME")
                            .Append("     AND EVI.DELFLG = '0' ")
                            .Append("     AND ((CAL.DELFLG = '0'")
                            .Append("     AND NOT EXISTS(")
                            .Append("      SELECT 1")
                            .Append("        FROM TBL_CAL_TODOITEM   TOI2")
                            .Append("       WHERE TOI2.TODOID = EVI.TODOID")
                            .Append("         AND ((TOI2.COMPLETIONFLG = '1' ")
                            .Append("		  AND TOI2.COMPLETIONDATE < :STARTTIME)")
                            .Append("         OR TOI2.DELFLG = '1' )))")
                            .Append("      OR EVI.CALID = 'NATIVE')")

                        '2012/03/15 SKFC 上田 【SALES_2】受注後工程の対応(改修) START
                        ''-------2012/03/01 SKFC 加藤 【SALES_2】受注後工程の対応 START

                        '.Append("   UNION ALL")

                        ''受注後工程のToDoの情報を取得する。
                        '.Append("  SELECT ")                                            '--------------------⑤
                        '.Append("         '1'                 AS TODOEVENT_FLG")
                        '.Append("       , TOI.CALID           AS CARENDAR_ID")
                        ''2012/03/08 SKFC 加藤 【SALES_2】受注後工程の対応(改修) START
                        ''.Append("       , TOI.EVENTID         AS EVENT_ID")          ' EVENTIDで並び替えることにより、TODOとEVENTの分離、アラーム部分の並び替えになる
                        '.Append("       , EVI.EVENTID         AS EVENT_ID")          ' EVENTIDで並び替えることにより、TODOとEVENTの分離、アラーム部分の並び替えになる
                        ''2012/03/08 SKFC 加藤 【SALES_2】受注後工程の対応(改修) END
                        '.Append("       , TOI.TODOID          AS TODO_ID")           ' TODOで並び替えることによって、複数あるTODOが固まるようにする
                        '.Append("       , TOI.UNIQUEID        AS UNIQUE_ID")
                        '.Append("       , ''                  AS CREATE_DATA_DIV")
                        '.Append("       , CAL.DLRCD           AS DEALER_CODE")
                        '.Append("       , CAL.STRCD           AS BRANCH_CODE")
                        '.Append("       , CAL.SCHEDULEID      AS SCHEDULE_ID")
                        '.Append("       , CAL.SCHEDULEDIV     AS SCHEDULE_DIV")
                        '.Append("       , TOI.ACTSTAFFCD      AS SALES_STAFF_CODE")
                        '.Append("       , TOI.RECSTAFFCD      AS SA_CODE")
                        '.Append("       , CAL.CUSTOMERDIV     AS CUSTOMER_DIV")
                        '.Append("       , CAL.CUSTCODE        AS CUSTOMER_CODE")
                        '.Append("       , CAL.DMSID           AS DMS_ID")
                        '.Append("       , CAL.CUSTNAME        AS CUSTOMER_NAME")
                        '.Append("       , CAL.RECEPTIONDIV    AS RECEPTION_DIV")
                        '.Append("       , TOI.CONTACTNO       AS CONTACT_NO")
                        '.Append("       , TOI.SUMMARY         AS SUMMARY")
                        '.Append("       , TO_CHAR(TOI.STARTTIME ,'YYYY/MM/DD HH24:MI:SS')       AS START_TIME")
                        '.Append("       , TO_CHAR(TOI.ENDTIME ,'YYYY/MM/DD HH24:MI:SS')         AS END_TIME")
                        '.Append("       , TOI.TIMEFLG         AS TIME_FLG")
                        '.Append("       , TOI.ALLDAYFLG       AS ALLDAY_FLG")
                        '.Append("       , TOI.MEMO            AS MEMO")
                        '.Append("       , TOI.ICROPCOLOR      AS ICROPCOLOR")
                        ''2012/03/06 SKFC 加藤 【SALES_2】受注後工程の対応(改修) START
                        ''.Append("       , ''                  AS COMP_FLG ")
                        '.Append("       , TOI.COMPLETIONFLG   AS COMP_FLG ")
                        ''2012/03/06 SKFC 加藤 【SALES_2】受注後工程の対応(改修) END
                        '.Append("       , TO_CHAR(TOI.UPDATEDATE ,'YYYY/MM/DD HH24:MI:SS')      AS UPDATE_DATE")
                        ''2012/03/06 SKFC 加藤 【SALES_2】受注後工程の対応(改修) START
                        ''.Append("       , EVA.STARTTRIGGER    AS ALARM_TRIGGER")
                        ''.Append("       , EVA.SEQNO           AS ALARM_SEQNO")
                        '.Append("       , TOA.STARTTRIGGER    AS ALARM_TRIGGER")
                        '.Append("       , TOA.SEQNO           AS ALARM_SEQNO")
                        ''2012/03/06 SKFC 加藤 【SALES_2】受注後工程の対応(改修) END
                        '.Append("       , TOI.RRULEFLG        AS R_RULE_FLG")
                        '.Append("       , TOI.RRULE_FREQ      AS R_RULE_FREQ")
                        '.Append("       , TOI.RRULE_INTERVAL  AS R_RULE_INTERVAL")
                        '.Append("       , TO_CHAR(TOI.RRULE_UNTIL ,'YYYY/MM/DD HH24:MI:SS')     AS R_RULE_UNTIL")
                        '.Append("       , TOI.PROCESSDIV      AS PROCESS_DIV")
                        ''2012/03/08 SKFC 加藤 【SALES_2】受注後工程の対応(改修) START
                        ''.Append("    FROM TBL_CAL_EVENTITEM   TOI")
                        ''.Append("       , TBL_CAL_ICROPINFO   CAL")
                        ''.Append("       , TBL_CAL_EVENTALARM  TOA")
                        ''.Append("   WHERE TOI.CALID = CAL.CALID(+)  ")
                        ''.Append("     AND TOI.EVENTID = TOA.EVENTID(+) ")
                        ''2012/03/08 SKFC 加藤 【SALES_2】受注後工程の対応(改修) END
                        '.Append("    FROM TBL_CAL_TODOITEM    TOI")
                        '.Append("       , TBL_CAL_ICROPINFO   CAL")
                        '.Append("       , TBL_CAL_TODOALARM   TOA")
                        '.Append("       , TBL_CAL_EVENTITEM   EVI")
                        '.Append("   WHERE TOI.CALID = CAL.CALID(+) ")
                        '.Append("     AND TOI.TODOID = TOA.TODOID(+)")
                        '.Append("     AND TOI.TODOID = EVI.TODOID(+)")
                        '.Append("     AND EVI.DELFLG(+) = '0'")
                        'If Validation.Equals(Sql_SalesStaffCode, permission) Then
                        '    .Append("     AND TOI.ACTSTAFFCD = :STAFFCODE")
                        'Else
                        '    .Append("     AND TOI.RECSTAFFCD = :STAFFCODE")
                        'End If
                        '.Append("     AND TOI.STARTTIME <= :ENDTIME ")
                        '.Append("     AND TOI.ENDTIME >= :STARTTIME ")
                        '.Append("     AND TOI.DELFLG = '0' ")
                        '.Append("     AND CAL.SCHEDULEDIV = '2' ")      '受注後工程
                        '.Append("     AND CAL.DELFLG = '0'")

                        '.Append("   UNION ALL")

                        ''受注後工程のEVENTデータの取得
                        '.Append("  SELECT ")                                        '--------------------⑥
                        '.Append("         '2'                 AS TODOEVENT_FLG")
                        '.Append("       , EVI.CALID           AS CARENDAR_ID")
                        '.Append("       , EVI.EVENTID         AS EVENT_ID")          ' EVENTIDで並び替えることにより、TODOとEVENTの分離、アラーム部分の並び替えになる
                        '.Append("       , EVI.TODOID          AS TODO_ID")           ' TODOで並び替えることによって、複数あるTODOが固まるようにする
                        '.Append("       , EVI.UNIQUEID        AS UNIQUE_ID")
                        '.Append("       , ''                  AS CREATE_DATA_DIV")
                        '.Append("       , CAL.DLRCD           AS DEALER_CODE")
                        '.Append("       , CAL.STRCD           AS BRANCH_CODE")
                        '.Append("       , CAL.SCHEDULEID      AS SCHEDULE_ID")
                        '.Append("       , CAL.SCHEDULEDIV     AS SCHEDULE_DIV")
                        '.Append("       , EVI.ACTSTAFFCD      AS SALES_STAFF_CODE")
                        '.Append("       , EVI.RECSTAFFCD      AS SA_CODE")
                        '.Append("       , CAL.CUSTOMERDIV     AS CUSTOMER_DIV")
                        '.Append("       , CAL.CUSTCODE        AS CUSTOMER_CODE")
                        '.Append("       , CAL.DMSID           AS DMS_ID")
                        '.Append("       , CAL.CUSTNAME        AS CUSTOMER_NAME")
                        '.Append("       , CAL.RECEPTIONDIV    AS RECEPTION_DIV")
                        '.Append("       , EVI.CONTACTNO       AS CONTACT_NO")
                        '.Append("       , EVI.SUMMARY         AS SUMMARY")
                        '.Append("       , TO_CHAR(EVI.STARTTIME ,'YYYY/MM/DD HH24:MI:SS')       AS START_TIME")
                        '.Append("       , TO_CHAR(EVI.ENDTIME ,'YYYY/MM/DD HH24:MI:SS')         AS END_TIME")
                        '.Append("       , EVI.TIMEFLG         AS TIME_FLG")
                        '.Append("       , EVI.ALLDAYFLG       AS ALLDAY_FLG")
                        '.Append("       , EVI.MEMO            AS MEMO")
                        '.Append("       , EVI.ICROPCOLOR      AS ICROPCOLOR")
                        '.Append("       , ''                  AS COMP_FLG ")
                        '.Append("       , TO_CHAR(EVI.UPDATEDATE ,'YYYY/MM/DD HH24:MI:SS')      AS UPDATE_DATE")
                        '.Append("       , EVA.STARTTRIGGER    AS ALARM_TRIGGER")
                        '.Append("       , EVA.SEQNO           AS ALARM_SEQNO")
                        '.Append("       , EVI.RRULEFLG        AS R_RULE_FLG")
                        '.Append("       , EVI.RRULE_FREQ      AS R_RULE_FREQ")
                        '.Append("       , EVI.RRULE_INTERVAL  AS R_RULE_INTERVAL")
                        '.Append("       , TO_CHAR(EVI.RRULE_UNTIL ,'YYYY/MM/DD HH24:MI:SS')     AS R_RULE_UNTIL")
                        '.Append("       , EVI.PROCESSDIV      AS PROCESS_DIV")
                        '.Append("    FROM TBL_CAL_EVENTITEM   EVI")
                        '.Append("       , TBL_CAL_ICROPINFO   CAL")
                        '.Append("       , TBL_CAL_EVENTALARM  EVA")
                        '.Append("   WHERE EVI.CALID = CAL.CALID(+)  ")
                        '.Append("     AND EVI.EVENTID = EVA.EVENTID(+) ")
                        'If Validation.Equals(Sql_SalesStaffCode, permission) Then
                        '    .Append("     AND EVI.ACTSTAFFCD = :STAFFCODE")
                        'Else
                        '    .Append("     AND EVI.RECSTAFFCD = :STAFFCODE")
                        'End If
                        '.Append("     AND EVI.STARTTIME <= :ENDTIME ")
                        '.Append("     AND EVI.ENDTIME >= :STARTTIME ")
                        '.Append("     AND EVI.DELFLG = '0' ")
                        '.Append("     AND EVI.RRULEFLG = '0'")
                        '.Append("     AND CAL.SCHEDULEDIV = '2' ")      '受注後工程
                        '.Append("     AND CAL.DELFLG = '0'")
                        ''2012/03/01 SKFC 加藤 【SALES_2】受注後工程の対応 END
                        '2012/03/15 SKFC 上田 【SALES_2】受注後工程の対応(改修) END

                            .Append(" ORDER BY CARENDAR_ID ASC NULLS FIRST ,TODOEVENT_FLG, EVENT_ID ASC NULLS FIRST , TODO_ID ASC NULLS FIRST")

                    End With

                    query.CommandText = sql.ToString()

                    'SQLパラメータ設定
                    query.AddParameterWithTypeValue(Sql_Bind_StartTime, OracleDbType.Date, startTime)
                    query.AddParameterWithTypeValue(Sql_Bind_EndTime, OracleDbType.Date, endTime)
                    '2014/04/25 SKFC 上田 NEXTSTEP_CALDAV START
                    If Validation.Equals(actionType, ConstClass.ActionTypeDone) Or _
                       Validation.Equals(actionType, ConstClass.ActionTypeFuture) Then
                        query.AddParameterWithTypeValue(Sql_Bind_StartTime2, OracleDbType.Date, startTime2)
                        query.AddParameterWithTypeValue(Sql_Bind_EndTime2, OracleDbType.Date, endTime2)
                    End If
                    '2014/04/25 SKFC 上田 NEXTSTEP_CALDAV END
                    query.AddParameterWithTypeValue(Sql_Bind_StaffCode, OracleDbType.Varchar2, staffCode)

                    'SQL実行（結果表を返却）
                    Return query.GetData()

                End Using

            Catch ex As SystemException

                Logger.Error(ex.Message, ex)
                Throw

            End Try



        End Function



        ''' <summary>
        ''' カレンダーXMLを作成する値を取得します。
        ''' </summary>
        ''' <param name="DeclearCD">検索条件の販売店コード</param>
        ''' <param name="BranchCD">検索条件の店舗コード</param>
        ''' <param name="ScheduleID">検索条件のスケジュールID</param>
        ''' <param name="ScheduleDiv">検索条件のスケジュール区分</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <history>2012/03/01 SKFC 加藤 【SALES_2】受注後工程の対応 START</history>
        Public Function GetSelectOrdersReceivedTable(ByVal DeclearCD As String, _
                                                       ByVal BranchCD As String, _
                                                       ByVal ScheduleID As String, _
                                                       ByVal ScheduleDiv As String) As CalenderXmlCreateClassDataSet.SelectCreateCalendarDataTableDataTable

            Dim sql As New StringBuilder

            Try

                Using query As New DBSelectQuery(Of CalenderXmlCreateClassDataSet.SelectCreateCalendarDataTableDataTable)("IC00001_002")

                    With sql
                        '受注後工程のToDoの情報を取得する。
                        .Append("  SELECT /* CalenderXmlCreateClass_004 */ ")           '--------------------①
                        .Append("         '1'                 AS TODOEVENT_FLG")
                        .Append("       , TOI.CALID           AS CARENDAR_ID")
                        '2012/03/08 SKFC 加藤 【SALES_2】受注後工程の対応 START
                        '.Append("       , TOI.EVENTID         AS EVENT_ID")          ' EVENTIDで並び替えることにより、TODOとEVENTの分離、アラーム部分の並び替えになる
                        .Append("       , EVI.EVENTID         AS EVENT_ID")          ' EVENTIDで並び替えることにより、TODOとEVENTの分離、アラーム部分の並び替えになる
                        '2012/03/08 SKFC 加藤 【SALES_2】受注後工程の対応 END
                        .Append("       , TOI.TODOID          AS TODO_ID")           ' TODOで並び替えることによって、複数あるTODOが固まるようにする
                        .Append("       , TOI.UNIQUEID        AS UNIQUE_ID")
                        .Append("       , ''                  AS CREATE_DATA_DIV")
                        .Append("       , CAL.DLRCD           AS DEALER_CODE")
                        .Append("       , CAL.STRCD           AS BRANCH_CODE")
                        .Append("       , CAL.SCHEDULEID      AS SCHEDULE_ID")
                        .Append("       , CAL.SCHEDULEDIV     AS SCHEDULE_DIV")
                        .Append("       , TOI.ACTSTAFFCD      AS SALES_STAFF_CODE")
                        .Append("       , TOI.RECSTAFFCD      AS SA_CODE")
                        .Append("       , CAL.CUSTOMERDIV     AS CUSTOMER_DIV")
                        .Append("       , CAL.CUSTCODE        AS CUSTOMER_CODE")
                        .Append("       , CAL.DMSID           AS DMS_ID")
                        .Append("       , CAL.CUSTNAME        AS CUSTOMER_NAME")
                        .Append("       , CAL.RECEPTIONDIV    AS RECEPTION_DIV")
                        .Append("       , TOI.CONTACTNO       AS CONTACT_NO")
                        .Append("       , TOI.SUMMARY         AS SUMMARY")
                        .Append("       , TO_CHAR(TOI.STARTTIME ,'YYYY/MM/DD HH24:MI:SS')       AS START_TIME")
                        .Append("       , TO_CHAR(TOI.ENDTIME ,'YYYY/MM/DD HH24:MI:SS')         AS END_TIME")
                        .Append("       , TOI.TIMEFLG         AS TIME_FLG")
                        .Append("       , TOI.ALLDAYFLG       AS ALLDAY_FLG")
                        .Append("       , TOI.MEMO            AS MEMO")
                        .Append("       , TOI.ICROPCOLOR      AS ICROPCOLOR")
                        .Append("       , ''                  AS COMP_FLG ")
                        .Append("       , TO_CHAR(TOI.UPDATEDATE ,'YYYY/MM/DD HH24:MI:SS')      AS UPDATE_DATE")
                        .Append("       , TOA.STARTTRIGGER    AS ALARM_TRIGGER")
                        .Append("       , TOA.SEQNO           AS ALARM_SEQNO")
                        .Append("       , TOI.RRULEFLG        AS R_RULE_FLG")
                        .Append("       , TOI.RRULE_FREQ      AS R_RULE_FREQ")
                        .Append("       , TOI.RRULE_INTERVAL  AS R_RULE_INTERVAL")
                        .Append("       , TO_CHAR(TOI.RRULE_UNTIL ,'YYYY/MM/DD HH24:MI:SS')     AS R_RULE_UNTIL")
                        .Append("       , TOI.PROCESSDIV      AS PROCESS_DIV")
                        '2014/04/25 SKFC 上田 NEXTSTEP_CALDAV START
                        .Append("       , TOI.CONTACT_NAME     AS CONTACT_NAME")
                        .Append("       , TOI.ACT_ODR_NAME     AS ACT_ODR_NAME")
                        .Append("       , TOI.ODR_DIV          AS ODR_DIV")
                        .Append("       , TOI.AFTER_ODR_ACT_ID AS AFTER_ODR_ACT_ID")
                        '2014/04/25 SKFC 上田 NEXTSTEP_CALDAV END
                        '2012/03/08 SKFC 加藤 【SALES_2】受注後工程の対応 START
                        '.Append("    FROM TBL_CAL_EVENTITEM   TOI")
                        '.Append("       , TBL_CAL_ICROPINFO   CAL")
                        '.Append("       , TBL_CAL_EVENTALARM  TOA")
                        '.Append("   WHERE TOI.CALID = CAL.CALID(+)  ")
                        '.Append("     AND TOI.TODOID = TOA.TODOID(+) ")
                        '.Append("     AND TOI.DELFLG = '0' ")
                        .Append("    FROM TBL_CAL_TODOITEM    TOI")
                        .Append("       , TBL_CAL_ICROPINFO   CAL")
                        .Append("       , TBL_CAL_TODOALARM   TOA")
                        .Append("       , TBL_CAL_EVENTITEM   EVI")
                        '2012/04/04 SKFC 上田 【SALES_2】受注後工程の対応 START
                        '.Append("   WHERE TOI.CALID = CAL.CALID(+) ")
                        .Append("   WHERE TOI.CALID = CAL.CALID ")
                        '2012/04/04 SKFC 上田 【SALES_2】受注後工程の対応 END
                        .Append("     AND TOI.TODOID = TOA.TODOID(+)")
                        .Append("     AND TOI.TODOID = EVI.TODOID(+)")
                        '2012/03/08 SKFC 加藤 【SALES_2】受注後工程の対応 END

                        '2012/03/09 SKFC 加藤 【SALES_2】受注後工程の対応 DEL START
                        '.Append("     AND TOI.STARTTIME <= :ENDTIME ")
                        '.Append("     AND TOI.ENDTIME >= :STARTTIME ")
                        '2012/03/09 SKFC 加藤 【SALES_2】受注後工程の対応 DEL END
                        .Append("     AND TOI.DELFLG = '0' ")
                        .Append("     AND TOI.RRULEFLG = '0'")
                        .Append("     AND CAL.DLRCD = :DLRCD ")                 '販売店コード
                        .Append("     AND CAL.STRCD = :STRCD ")                 '店舗コード
                        .Append("     AND CAL.SCHEDULEDIV = :SCHEDULEDIV ")     'スケジュール区分
                        .Append("     AND CAL.SCHEDULEID = :SCHEDULEID ")       'スケジュールID
                        .Append("     AND CAL.DELFLG = '0'")
                        '2012/04/04 SKFC 上田 【SALES_2】受注後工程の対応 START
                        .Append("     AND EVI.DELFLG(+) = '0' ")
                        '2012/04/04 SKFC 上田 【SALES_2】受注後工程の対応 END

                        .Append("   UNION ALL")

                        '受注後工程のEVENTデータの取得
                        .Append("  SELECT ")                                        '--------------------②
                        .Append("         '2'                 AS TODOEVENT_FLG")
                        .Append("       , EVI.CALID           AS CARENDAR_ID")
                        .Append("       , EVI.EVENTID         AS EVENT_ID")          ' EVENTIDで並び替えることにより、TODOとEVENTの分離、アラーム部分の並び替えになる
                        .Append("       , EVI.TODOID          AS TODO_ID")           ' TODOで並び替えることによって、複数あるTODOが固まるようにする
                        .Append("       , EVI.UNIQUEID        AS UNIQUE_ID")
                        .Append("       , ''                  AS CREATE_DATA_DIV")
                        .Append("       , CAL.DLRCD           AS DEALER_CODE")
                        .Append("       , CAL.STRCD           AS BRANCH_CODE")
                        .Append("       , CAL.SCHEDULEID      AS SCHEDULE_ID")
                        .Append("       , CAL.SCHEDULEDIV     AS SCHEDULE_DIV")
                        .Append("       , EVI.ACTSTAFFCD      AS SALES_STAFF_CODE")
                        .Append("       , EVI.RECSTAFFCD      AS SA_CODE")
                        .Append("       , CAL.CUSTOMERDIV     AS CUSTOMER_DIV")
                        .Append("       , CAL.CUSTCODE        AS CUSTOMER_CODE")
                        .Append("       , CAL.DMSID           AS DMS_ID")
                        .Append("       , CAL.CUSTNAME        AS CUSTOMER_NAME")
                        .Append("       , CAL.RECEPTIONDIV    AS RECEPTION_DIV")
                        .Append("       , EVI.CONTACTNO       AS CONTACT_NO")
                        .Append("       , EVI.SUMMARY         AS SUMMARY")
                        .Append("       , TO_CHAR(EVI.STARTTIME ,'YYYY/MM/DD HH24:MI:SS')       AS START_TIME")
                        .Append("       , TO_CHAR(EVI.ENDTIME ,'YYYY/MM/DD HH24:MI:SS')         AS END_TIME")
                        .Append("       , EVI.TIMEFLG         AS TIME_FLG")
                        .Append("       , EVI.ALLDAYFLG       AS ALLDAY_FLG")
                        .Append("       , EVI.MEMO            AS MEMO")
                        .Append("       , EVI.ICROPCOLOR      AS ICROPCOLOR")
                        .Append("       , ''                  AS COMP_FLG ")
                        .Append("       , TO_CHAR(EVI.UPDATEDATE ,'YYYY/MM/DD HH24:MI:SS')      AS UPDATE_DATE")
                        .Append("       , EVA.STARTTRIGGER    AS ALARM_TRIGGER")
                        .Append("       , EVA.SEQNO           AS ALARM_SEQNO")
                        .Append("       , EVI.RRULEFLG        AS R_RULE_FLG")
                        .Append("       , EVI.RRULE_FREQ      AS R_RULE_FREQ")
                        .Append("       , EVI.RRULE_INTERVAL  AS R_RULE_INTERVAL")
                        .Append("       , TO_CHAR(EVI.RRULE_UNTIL ,'YYYY/MM/DD HH24:MI:SS')     AS R_RULE_UNTIL")
                        .Append("       , EVI.PROCESSDIV      AS PROCESS_DIV")
                        '2014/04/25 SKFC 上田 NEXTSTEP_CALDAV START
                        .Append("       , EVI.CONTACT_NAME     AS CONTACT_NAME")
                        .Append("       , EVI.ACT_ODR_NAME     AS ACT_ODR_NAME")
                        .Append("       , EVI.ODR_DIV          AS ODR_DIV")
                        .Append("       , EVI.AFTER_ODR_ACT_ID AS AFTER_ODR_ACT_ID")
                        '2014/04/25 SKFC 上田 NEXTSTEP_CALDAV END
                        .Append("    FROM TBL_CAL_EVENTITEM   EVI")
                        .Append("       , TBL_CAL_ICROPINFO   CAL")
                        .Append("       , TBL_CAL_EVENTALARM  EVA")
                        '2012/04/04 SKFC 上田 【SALES_2】受注後工程の対応 START
                        '.Append("   WHERE EVI.CALID = CAL.CALID(+)  ")
                        .Append("   WHERE EVI.CALID = CAL.CALID ")
                        '2012/04/04 SKFC 上田 【SALES_2】受注後工程の対応 END
                        .Append("     AND EVI.EVENTID = EVA.EVENTID(+) ")
                        .Append("     AND EVI.DELFLG = '0' ")
                        .Append("     AND EVI.RRULEFLG = '0'")
                        '2012/03/09 SKFC 加藤 【SALES_2】受注後工程の対応 DEL START
                        '.Append("     AND EVI.STARTTIME <= :ENDTIME ")
                        '.Append("     AND EVI.ENDTIME >= :STARTTIME ")
                        '2012/03/09 SKFC 加藤 【SALES_2】受注後工程の対応 DEL END
                        .Append("     AND CAL.DLRCD = :DLRCD ")                 '販売店コード
                        .Append("     AND CAL.STRCD = :STRCD ")                 '店舗コード
                        .Append("     AND CAL.SCHEDULEDIV = :SCHEDULEDIV ")     'スケジュール区分
                        .Append("     AND CAL.SCHEDULEID = :SCHEDULEID ")       'スケジュールID
                        .Append("     AND CAL.DELFLG = '0'")

                        .Append("ORDER BY CARENDAR_ID ASC NULLS FIRST ,TODOEVENT_FLG, EVENT_ID ASC NULLS FIRST , TODO_ID ASC NULLS FIRST")

                    End With

                    query.CommandText = sql.ToString()

                    'SQLパラメータ設定
                    query.AddParameterWithTypeValue(Sql_Bind_DeclearCode, OracleDbType.Char, DeclearCD)
                    query.AddParameterWithTypeValue(Sql_Bind_BranchCode, OracleDbType.Char, BranchCD)
                    '2012/04/04 SKFC 上田 【SALES_2】受注後工程の対応 START
                    'query.AddParameterWithTypeValue(Sql_Bind_ScheduleID, OracleDbType.Char, ScheduleID)
                    'query.AddParameterWithTypeValue(Sql_Bind_ScheduleDiv, OracleDbType.Long, ScheduleDiv)
                    query.AddParameterWithTypeValue(Sql_Bind_ScheduleID, OracleDbType.Long, ScheduleID)
                    query.AddParameterWithTypeValue(Sql_Bind_ScheduleDiv, OracleDbType.Char, ScheduleDiv)
                    '2012/04/04 SKFC 上田 【SALES_2】受注後工程の対応 END

                    'SQL実行（結果表を返却）
                    Return query.GetData()

                End Using

            Catch ex As SystemException

                Logger.Error(ex.Message, ex)
                Throw

            End Try



        End Function

    End Class




    Public Class ExDateTable
        Inherits Global.System.ComponentModel.Component

        ' バインド変数
        Private Const Sql_Bind_StartTime As String = "STARTTIME"
        Private Const Sql_Bind_EndTime As String = "ENDTIME"
        Private Const Sql_Bind_StaffCode As String = "STAFFCODE"
        Private Const Sql_SalesStaffCode As String = "8"

        ''' <summary>
        ''' TODOIDに紐付く、繰り返し除外日を取得する
        ''' </summary>
        ''' <returns>繰り返し除外日DataTable</returns>
        ''' <remarks></remarks>
        Public Function GetTodoExDate(ByVal startTime As DateTime, _
                                      ByVal endTime As DateTime, _
                                      ByVal staffCode As String, _
                                      ByVal permission As String) As CalenderXmlCreateClassDataSet.ExDateTableDataTable

            Using query As New DBSelectQuery(Of CalenderXmlCreateClassDataSet.ExDateTableDataTable)("CalenderXmlCreateClass_002")

                Dim sql As New StringBuilder

                Try

                    With sql
                        .Append("  SELECT /* CalenderXmlCreateClass_002 */ ")
                        .Append("         TOE.TODOID        AS IDS")
                        .Append("       , TOE.EXDATE        AS EXDATE")
                        .Append("    FROM TBL_CAL_TODOEXDATE TOE")
                        .Append("       , TBL_CAL_TODOITEM   TOI")
                        .Append("   WHERE TOE.TODOID = TOI.TODOID")
                        If Validation.Equals(Sql_SalesStaffCode, permission) Then
                            .Append("     AND TOI.ACTSTAFFCD = :STAFFCODE")
                        Else
                            .Append("     AND TOI.RECSTAFFCD = :STAFFCODE")
                        End If
                        .Append("     AND :STARTTIME <= TOE.EXDATE ")
                        .Append("     AND TOE.EXDATE <= :ENDTIME ")

                    End With

                    query.CommandText = sql.ToString()

                    'SQLパラメータ設定
                    query.AddParameterWithTypeValue(Sql_Bind_StartTime, OracleDbType.Date, startTime)
                    query.AddParameterWithTypeValue(Sql_Bind_EndTime, OracleDbType.Date, endTime)
                    query.AddParameterWithTypeValue(Sql_Bind_StaffCode, OracleDbType.Varchar2, staffCode)

                    'SQL実行（結果表を返却）
                    Return query.GetData()

                Catch ex As SystemException

                    Logger.Error(ex.Message, ex)
                    Throw

                End Try

            End Using

        End Function

        ''' <summary>
        ''' EventIDに紐付く、繰り返し除外日を取得する
        ''' </summary>
        ''' <returns>繰り返し除外日DataTable</returns>
        ''' <remarks></remarks>
        Public Function GetEventExDate(ByVal startTime As DateTime, _
                                      ByVal endTime As DateTime, _
                                      ByVal staffCode As String, _
                                      ByVal permission As String) As CalenderXmlCreateClassDataSet.ExDateTableDataTable

            Using query As New DBSelectQuery(Of CalenderXmlCreateClassDataSet.ExDateTableDataTable)("CalenderXmlCreateClass_003")

                Dim sql As New StringBuilder

                Try

                    With sql
                        .Append("  SELECT /* CalenderXmlCreateClass_003 */ ")
                        .Append("         EVE.EVENTID       AS IDS")
                        .Append("       , EVE.EXDATE        AS EXDATE")
                        .Append("    FROM TBL_CAL_EVENTEXDATE EVE")
                        .Append("       , TBL_CAL_EVENTITEM   EVI")
                        .Append("   WHERE EVE.EVENTID = EVI.EVENTID")
                        If Validation.Equals(Sql_SalesStaffCode, permission) Then
                            .Append("     AND EVI.ACTSTAFFCD = :STAFFCODE")
                        Else
                            .Append("     AND EVI.RECSTAFFCD = :STAFFCODE")
                        End If
                        .Append("     AND :STARTTIME <= EVE.EXDATE ")
                        .Append("     AND EVE.EXDATE <= :ENDTIME ")

                    End With

                    query.CommandText = sql.ToString()

                    'SQLパラメータ設定
                    query.AddParameterWithTypeValue(Sql_Bind_StartTime, OracleDbType.Date, startTime)
                    query.AddParameterWithTypeValue(Sql_Bind_EndTime, OracleDbType.Date, endTime)
                    query.AddParameterWithTypeValue(Sql_Bind_StaffCode, OracleDbType.Varchar2, staffCode)

                    'SQL実行（結果表を返却）
                    Return query.GetData()

                Catch ex As SystemException

                    Logger.Error(ex.Message, ex)
                    Throw

                End Try

            End Using

        End Function

    End Class

End Namespace


Partial Public Class CalenderXmlCreateClassDataSet
    Partial Class SelectCreateCalendarDataTableDataTable

        Private Sub SelectCreateCalendarDataTableDataTable_ColumnChanging(sender As System.Object, e As System.Data.DataColumnChangeEventArgs) Handles Me.ColumnChanging
            If (e.Column.ColumnName = Me.CONTACT_NAMEColumn.ColumnName) Then
                'ユーザー コードをここに追加してください
            End If

        End Sub

    End Class

End Class
