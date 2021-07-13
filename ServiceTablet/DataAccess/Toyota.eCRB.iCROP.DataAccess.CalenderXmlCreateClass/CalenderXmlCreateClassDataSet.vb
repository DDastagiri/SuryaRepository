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
        Private Const Sql_Bind_StaffCode As String = "STAFFCODE"
        Private Const Sql_SalesStaffCode As String = "8"

        ''' <summary>
        ''' カレンダーXMLを作成する値を取得します。
        ''' </summary>
        ''' <param name="startTime">開始時間</param>
        ''' <param name="endTime">終了時間</param>
        ''' <param name="staffCode">スタッフコード</param>
        ''' <param name="permission">権利</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetSelectCalendarTable(ByVal startTime As DateTime, _
                                            ByVal endTime As DateTime, _
                                            ByVal staffCode As String, _
                                            ByVal permission As String) As CalenderXmlCreateClassDataSet.SelectCreateCalendarDataTableDataTable

            Dim sql As New StringBuilder

            Try

                Using query As New DBSelectQuery(Of CalenderXmlCreateClassDataSet.SelectCreateCalendarDataTableDataTable)("IC00001_001")

                    With sql
                        .Append("  SELECT /* CalenderXmlCreateClass_001 */ ")
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
                        .Append("     AND CAL.SCHEDULEDIV = '0' ")
                        .Append("     AND CAL.DELFLG = '0'")
                        .Append("     AND ((TOI.STARTTIME <= :ENDTIME ")
                        .Append("     AND TOI.STARTTIMEFLG = '1' ")
                        .Append("     AND TOI.RRULEFLG = '0' ")
                        .Append("     AND TOI.COMPLETIONFLG = '0')")
                        .Append("      OR (TOI.ENDTIME <= :ENDTIME ")
                        .Append("     AND TOI.STARTTIMEFLG = '0' ")
                        .Append("     AND TOI.RRULEFLG = '0' ")
                        .Append("     AND TOI.COMPLETIONFLG = '0')")
                        .Append("      OR (:STARTTIME <= TOI.COMPLETIONDATE ")
                        .Append("     AND TOI.COMPLETIONDATE <= :ENDTIME ")
                        .Append("     AND TOI.COMPLETIONFLG = '1'))")

                        .Append("   UNION ALL")

                        .Append("  SELECT ")
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

                        .Append("   UNION ALL")

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
                        .Append("     AND TOI.COMPLETIONFLG = '0'")
                        .Append("     AND TOI.DELFLG = '0'")
                        .Append("     AND CAL.SCHEDULEDIV = '0'")
                        .Append("     AND CAL.DELFLG = '0'")
                        .Append("     AND TOI.RRULEFLG = '1'")
                        .Append("     AND TOI.RRULE_UNTIL  >= :STARTTIME")


                        .Append("   UNION ALL")

                        .Append("  SELECT ")
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
                        .Append("ORDER BY CARENDAR_ID ASC NULLS FIRST ,TODOEVENT_FLG, EVENT_ID ASC NULLS FIRST , TODO_ID ASC NULLS FIRST")

                    End With

                    query.CommandText = sql.ToString()

                    'SQLパラメータ設定
                    query.AddParameterWithTypeValue(Sql_Bind_StartTime, OracleDbType.Date, startTime)
                    query.AddParameterWithTypeValue(Sql_Bind_EndTime, OracleDbType.Date, endTime)
                    query.AddParameterWithTypeValue(Sql_Bind_StaffCode, OracleDbType.Varchar2, staffCode)

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
