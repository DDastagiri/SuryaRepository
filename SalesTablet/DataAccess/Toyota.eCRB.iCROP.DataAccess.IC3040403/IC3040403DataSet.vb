Imports System.Text
Imports Toyota.eCRB.SystemFrameworks.Configuration
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Oracle.DataAccess.Client

'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'IC3040403DataSet.vb
'─────────────────────────────────────
'機能： ｶﾚﾝﾀﾞｰ情報登録ｲﾝﾀｰﾌｪｰｽ
'補足： 
'更新：     2020/06/17 SKFC二村  TR-SLT-TKM-20200206-001横展(カレンダーIDの特定から販売店コード、店舗コードを除外)
'─────────────────────────────────────

Namespace IC3040403DataSetTableAdapters

    Public Class IdTable
        Inherits Global.System.ComponentModel.Component

        ''' <summary>
        ''' カレンダーIDを取得します。
        ''' </summary>
        ''' <param name="scheduleId">スケジュールID</param>
        ''' <param name="scheduleDiv">スケジュール区分</param>
        ''' <returns>カレンダーIDが格納されたdataTable</returns>
        ''' <remarks></remarks>
        Public Function GetCalenderId(ByVal scheduleId As String, _
                                      ByVal scheduleDiv As String) As String

            Dim table As IC3040403DataSet.IdTableDataTable

            Using query As New DBSelectQuery(Of IC3040403DataSet.IdTableDataTable)("IC3040403_001")

                Try

                    Dim sql As New StringBuilder
                    With sql
                        .Append("SELECT /* IC3040403_001 */ ")
                        .Append("       CALID IDS ")
                        .Append("  FROM TBL_CAL_ICROPINFO ")
                        .Append(" WHERE SCHEDULEDIV = :SCHEDULEDIV ")
                        .Append("   AND SCHEDULEID = :SCHEDULEID ")
                        .Append("   AND DELFLG = '0'")

                    End With

                    query.CommandText = sql.ToString()

                    'SQLパラメータ設定
                    query.AddParameterWithTypeValue(ConstCode.CalendarTableScheduleDiv, OracleDbType.Char, scheduleDiv)
                    query.AddParameterWithTypeValue(ConstCode.CalendarTableScheduleId, OracleDbType.Long, scheduleId)

                    'SQL実行（結果表を返却）
                    table = query.GetData()

                    If table.Count = 0 Then

                        Return Nothing

                    End If

                    Dim dataRow As IC3040403DataSet.IdTableRow = table.Rows(0)
                    Return dataRow.Ids

                Catch ex As SystemException
                    '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 START
                    Logger.Error(ex.Message, ex)
                    '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 END
                    Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.GetCalenderIdSqlError)

                End Try

            End Using

        End Function

        ''' <summary>
        ''' イベントIDを取得します。
        ''' </summary>
        ''' <param name="todoId">TodoId</param>
        ''' <returns>イベントID</returns>
        ''' <remarks></remarks>
        Public Function GetEventId(ByVal todoId As String) As String

            Dim table As IC3040403DataSet.IdTableDataTable

            Using query As New DBSelectQuery(Of IC3040403DataSet.IdTableDataTable)("IC3040403_002")

                Try

                    Dim sql As New StringBuilder
                    With sql
                        .Append(" SELECT /* IC3040403_002 */ ")
                        .Append("        EVENTID IDS ")
                        .Append("   FROM TBL_CAL_EVENTITEM ")
                        .Append("  WHERE TODOID  = :TODOID ")
                        .Append("    AND DELFLG = '0'")
                    End With

                    query.CommandText = sql.ToString()

                    'SQLパラメータ設定
                    query.AddParameterWithTypeValue(ConstCode.EventItemTableTodoId, OracleDbType.Varchar2, todoId)

                    'SQL実行（結果表を返却）
                    table = query.GetData()

                    If table.Count = 0 Then

                        Return Nothing

                    End If

                    Dim dataRow As IC3040403DataSet.IdTableRow = table.Rows(0)
                    Return dataRow.Ids

                Catch ex As SystemException
                    '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 START
                    Logger.Error(ex.Message, ex)
                    '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 END
                    Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.GetEventIdSqlError)

                End Try

            End Using

        End Function

        ''' <summary>
        ''' ToDOIDを取得します。
        ''' </summary>
        ''' <param name="processDiv">TodoId</param>
        ''' <returns>ToDOID</returns>
        ''' <remarks></remarks>
        ''' <history>2012/03/07 SKFC 加藤 【SALES_2】受注後工程の対応 START</history>
        Public Function GetToDoId(calenderId As String, ByVal ProcessDiv As String) As String

            Dim table As IC3040403DataSet.IdTableDataTable

            Using query As New DBSelectQuery(Of IC3040403DataSet.IdTableDataTable)("IC3040403_002")

                Try

                    Dim sql As New StringBuilder
                    With sql
                        .Append(" SELECT /* IC3040403_002 */ ")
                        .Append("        TODOID IDS ")
                        .Append("   FROM TBL_CAL_TODOITEM ")
                        .Append("  WHERE CALID       = :CALID ")
                        .Append("    AND PROCESSDIV  = :PROCESSDIV ")
                        .Append("    AND DELFLG = '0'")
                    End With

                    query.CommandText = sql.ToString()

                    'SQLパラメータ設定
                    query.AddParameterWithTypeValue(ConstCode.TodoItemTableCalId, OracleDbType.Varchar2, calenderId)
                    query.AddParameterWithTypeValue(ConstCode.TodoItemTableProcessDiv, OracleDbType.Varchar2, ProcessDiv)

                    'SQL実行（結果表を返却）
                    table = query.GetData()

                    If table.Count = 0 Then
                        Return Nothing
                    End If

                    Dim dataRow As IC3040403DataSet.IdTableRow = table.Rows(0)
                    Return dataRow.Ids

                Catch ex As SystemException
                    Logger.Error(ex.Message, ex)
                    Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.GetEventIdSqlError)

                End Try

            End Using

        End Function
    End Class





    Public Class CalCalenderDataTable
        Inherits Global.System.ComponentModel.Component

        ''' <summary>
        ''' カレンダーICROP情報管理に登録します。
        ''' </summary>
        ''' <param name="dataRow">引数</param>
        ''' <returns>登録件数</returns>
        ''' <remarks></remarks>
        Public Function InsertCalCalender(ByVal dataRow As IC3040403DataSet.CalCalenderDataTableRow) As Integer

            Using query As New DBUpdateQuery("IC3040403_003")
                Try

                    Dim sql As New StringBuilder
                    With sql
                        .Append(" INSERT /* IC3040403_003 */ ")
                        .Append("   INTO TBL_CAL_ICROPINFO ( ")
                        .Append("        CALID ")
                        .Append("      , DLRCD ")
                        .Append("      , STRCD ")
                        .Append("      , SCHEDULEDIV ")
                        .Append("      , SCHEDULEID ")
                        .Append("      , CUSTOMERDIV ")
                        .Append("      , CUSTCODE ")
                        .Append("      , DMSID ")
                        .Append("      , CUSTNAME ")
                        .Append("      , RECEPTIONDIV ")
                        .Append("      , SERVICECODE ")
                        .Append("      , MERCHANDISECD ")
                        .Append("      , STRSTATUS ")
                        .Append("      , REZSTATUS ")
                        .Append("      , DELFLG ")
                        .Append("      , DELDATE ")
                        .Append("      , CREATEDATE ")
                        .Append("      , UPDATEDATE ")
                        .Append("      , CREATEACCOUNT ")
                        .Append("      , UPDATEACCOUNT ")
                        .Append("      , CREATEID ")
                        .Append("      , UPDATEID )")
                        .Append(" VALUES (")
                        .Append("        :CALID ")
                        .Append("      , :DLRCD ")
                        .Append("      , :STRCD ")
                        .Append("      , :SCHEDULEDIV ")
                        .Append("      , :SCHEDULEID ")
                        .Append("      , :CUSTOMERDIV ")
                        .Append("      , :CUSTCODE ")
                        .Append("      , :DMSID ")
                        .Append("      , :CUSTNAME ")
                        .Append("      , :RECEPTIONDIV ")
                        .Append("      , :SERVICECODE ")
                        .Append("      , :MERCHANDISECD ")
                        .Append("      , :STRSTATUS ")
                        .Append("      , :REZSTATUS ")
                        .Append("      , :DELFLG ")
                        .Append("      , :DELDATE ")
                        .Append("      , SYSDATE ")
                        .Append("      , SYSDATE ")
                        .Append("      , :CREATEACCOUNT ")
                        .Append("      , :UPDATEACCOUNT ")
                        .Append("      , :CREATEID ")
                        .Append("      , :UPDATEID )")

                    End With

                    Using convert As New IC3040403DataSetTableAdapters.IC3040403DataAdapter

                        query.CommandText = sql.ToString()
                        'SQLパラメータ設定
                        query.AddParameterWithTypeValue(ConstCode.CalendarTableCalId, OracleDbType.NVarchar2, dataRow.CALID)
                        query.AddParameterWithTypeValue(ConstCode.CalendarTableDlrCD, OracleDbType.Char, dataRow.DLRCD)
                        query.AddParameterWithTypeValue(ConstCode.CalendarTableStrCD, OracleDbType.Char, dataRow.STRCD)
                        query.AddParameterWithTypeValue(ConstCode.CalendarTableScheduleDiv, OracleDbType.Char, dataRow.SCHEDULEDIV)
                        query.AddParameterWithTypeValue(ConstCode.CalendarTableScheduleId, OracleDbType.Char, dataRow.SCHEDULEID)
                        query.AddParameterWithTypeValue(ConstCode.CalendarTableCustomerDiv, OracleDbType.Char, dataRow.CUSTOMERDIV)
                        query.AddParameterWithTypeValue(ConstCode.CalendarTableCustCode, OracleDbType.NVarchar2, dataRow.CUSTCODE)
                        query.AddParameterWithTypeValue(ConstCode.CalendarTableDmsId, OracleDbType.NVarchar2, dataRow.DMSID)
                        query.AddParameterWithTypeValue(ConstCode.CalendarTableCustName, OracleDbType.NVarchar2, dataRow.CUSTNAME)
                        query.AddParameterWithTypeValue(ConstCode.CalendarTableReceptionDiv, OracleDbType.Char, dataRow.RECEPTIONDIV)
                        query.AddParameterWithTypeValue(ConstCode.CalendarTableServiceCode, OracleDbType.Char, dataRow.SERVICECODE)
                        query.AddParameterWithTypeValue(ConstCode.CalendarTableMerchandDisCD, OracleDbType.Char, dataRow.MERCHANDISECD)
                        query.AddParameterWithTypeValue(ConstCode.CalendarTableStrStatus, OracleDbType.Char, dataRow.STRSTATUS)
                        query.AddParameterWithTypeValue(ConstCode.CalendarTableRezStatus, OracleDbType.Int32, convert.ConvertStringEmpty(dataRow.REZSTATUS))
                        query.AddParameterWithTypeValue(ConstCode.CalendarTableDelFlg, OracleDbType.Char, dataRow.DELFLG)
                        query.AddParameterWithTypeValue(ConstCode.CalendarTableDelDate, OracleDbType.Date, convert.ConvertStringDateTime(dataRow.DELDATE))
                        query.AddParameterWithTypeValue(ConstCode.CalendarTableCreateAccount, OracleDbType.NVarchar2, dataRow.CREATEACCOUNT)
                        query.AddParameterWithTypeValue(ConstCode.CalendarTableUpdateAccount, OracleDbType.NVarchar2, dataRow.UPDATEACCOUNT)
                        query.AddParameterWithTypeValue(ConstCode.CalendarTableCreateId, OracleDbType.NVarchar2, dataRow.CREATEID)
                        query.AddParameterWithTypeValue(ConstCode.CalendarTableUpdateId, OracleDbType.NVarchar2, dataRow.UPDATEID)

                    End Using

                    'SQL実行（登録行を返却）
                    Dim Count As Integer = query.Execute()
                    Return Count

                Catch ex As SystemException
                    '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 START
                    Logger.Error(ex.Message, ex)
                    '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 END
                    Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.InsertCalCalenderSqlError)

                End Try

            End Using

        End Function

        ''' <summary>
        ''' カレンダーICROP情報管理を更新します
        ''' </summary>
        ''' <param name="dataRow">引数</param>
        ''' <returns>更新件数</returns>
        ''' <remarks></remarks>
        Public Function UpdateCalCalender(ByVal dataRow As IC3040403DataSet.CalCalenderDataTableRow) As Integer

            Using query As New DBUpdateQuery("IC3040403_004")

                Try

                    Dim sql As New StringBuilder

                    Using convert As New IC3040403DataSetTableAdapters.IC3040403DataAdapter

                        With sql
                            .Append(" UPDATE /* IC3040403_004 */ TBL_CAL_ICROPINFO")
                            .Append("    SET UPDATEDATE = SYSDATE")
                            .Append("      , UPDATEACCOUNT = :UPDATEACCOUNT ")
                            query.AddParameterWithTypeValue(ConstCode.CalendarTableUpdateAccount, OracleDbType.Char, dataRow.UPDATEACCOUNT)
                            .Append("      , UPDATEID = :UPDATEID")
                            query.AddParameterWithTypeValue(ConstCode.CalendarTableUpdateId, OracleDbType.Char, dataRow.UPDATEID)
                            If dataRow.CUSTOMERDIV IsNot Nothing Then
                                .Append("      , CUSTOMERDIV = :CUSTOMERDIV ")
                                query.AddParameterWithTypeValue(ConstCode.CalendarTableCustomerDiv, OracleDbType.Char, dataRow.CUSTOMERDIV)
                            End If
                            If dataRow.CUSTCODE IsNot Nothing Then
                                .Append("      , CUSTCODE = :CUSTCODE ")
                                query.AddParameterWithTypeValue(ConstCode.CalendarTableCustCode, OracleDbType.NVarchar2, dataRow.CUSTCODE)
                            End If
                            If dataRow.CUSTNAME IsNot Nothing Then
                                .Append("      , CUSTNAME = :CUSTNAME ")
                                query.AddParameterWithTypeValue(ConstCode.CalendarTableCustName, OracleDbType.NVarchar2, dataRow.CUSTNAME)
                            End If
                            If dataRow.DMSID IsNot Nothing Then
                                .Append("      , DMSID = :DMSID ")
                                query.AddParameterWithTypeValue(ConstCode.CalendarTableDmsId, OracleDbType.NVarchar2, dataRow.DMSID)
                            End If
                            If dataRow.RECEPTIONDIV IsNot Nothing Then
                                .Append("      , RECEPTIONDIV = :RECEPTIONDIV ")
                                query.AddParameterWithTypeValue(ConstCode.CalendarTableReceptionDiv, OracleDbType.Char, dataRow.RECEPTIONDIV)
                            End If
                            If dataRow.SERVICECODE IsNot Nothing Then
                                .Append("      , SERVICECODE = :SERVICECODE ")
                                query.AddParameterWithTypeValue(ConstCode.CalendarTableServiceCode, OracleDbType.Char, dataRow.SERVICECODE)
                            End If
                            If dataRow.MERCHANDISECD IsNot Nothing Then
                                .Append("      , MERCHANDISECD = :MERCHANDISECD ")
                                query.AddParameterWithTypeValue(ConstCode.CalendarTableMerchandDisCD, OracleDbType.Char, dataRow.MERCHANDISECD)
                            End If
                            If dataRow.STRSTATUS IsNot Nothing Then
                                .Append("      , STRSTATUS = :STRSTATUS ")
                                query.AddParameterWithTypeValue(ConstCode.CalendarTableStrStatus, OracleDbType.Char, dataRow.STRSTATUS)
                            End If
                            If dataRow.REZSTATUS IsNot Nothing Then
                                .Append("      , REZSTATUS = :REZSTATUS ")
                                query.AddParameterWithTypeValue(ConstCode.CalendarTableRezStatus, OracleDbType.Int32, convert.ConvertStringEmpty(dataRow.REZSTATUS))
                            End If
                            If dataRow.DELDATE IsNot Nothing Then
                                .Append("      , DELDATE = :DELDATE ")
                                query.AddParameterWithTypeValue(ConstCode.CalendarTableDelDate, OracleDbType.Date, convert.ConvertStringDateTime(dataRow.DELDATE))
                            End If
                            .Append(" WHERE  CALID = :CALID ")
                            query.AddParameterWithTypeValue(ConstCode.CalendarTableCalId, OracleDbType.Varchar2, dataRow.CALID)

                        End With

                    End Using

                    query.CommandText = sql.ToString()
                    'SQLパラメータ設定

                    'SQL実行（更新行を返却）
                    Dim Count As Integer = query.Execute()
                    Return Count

                Catch ex As SystemException
                    '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 START
                    Logger.Error(ex.Message, ex)
                    '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 END
                    Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.UpdateCalCalenderSqlError)

                End Try

            End Using

        End Function

        ''' <summary>
        ''' カレンダーICROP情報管理の削除フラグを更新します。
        ''' </summary>
        ''' <param name="dataRow">引数</param>
        ''' <returns>更新件数</returns>
        ''' <remarks></remarks>
        Public Function UpdateDeleteFlgCalItem(ByVal dataRow As IC3040403DataSet.CalCalenderDataTableRow) As Integer

            Using query As New DBUpdateQuery("IC3040403_005")

                Try

                    Dim sql As New StringBuilder

                    Using convert As New IC3040403DataSetTableAdapters.IC3040403DataAdapter


                        With sql
                            .Append(" UPDATE /* IC3040403_005 */ TBL_CAL_ICROPINFO")
                            .Append("    SET UPDATEDATE = SYSDATE")
                            .Append("      , UPDATEACCOUNT = :UPDATEACCOUNT ")
                            query.AddParameterWithTypeValue(ConstCode.TodoItemTableUpdateAccount, OracleDbType.Varchar2, dataRow.UPDATEACCOUNT)
                            .Append("      , UPDATEID = :UPDATEID")
                            If dataRow.DELDATE Is Nothing Then
                                .Append("      , DELDATE = SYSDATE")
                            Else
                                .Append("      , DELDATE = :DELDATE")
                                query.AddParameterWithTypeValue(ConstCode.CalendarTableDelDate, OracleDbType.Date, convert.ConvertStringDateTime(dataRow.DELDATE))
                            End If
                            .Append("      , DELFLG = '1'")
                            .Append("  WHERE CALID = :CALID  ")
                            .Append("    AND DELFLG = '0' ")
                        End With

                        query.CommandText = sql.ToString()


                        query.AddParameterWithTypeValue(ConstCode.CalendarTableUpdateId, OracleDbType.NVarchar2, dataRow.UPDATEID)
                        query.AddParameterWithTypeValue(ConstCode.CalendarTableCalId, OracleDbType.Varchar2, dataRow.CALID)

                    End Using

                    'SQL実行（更新行を返却）
                    Dim Count As Integer = query.Execute()
                    Return Count

                Catch ex As SystemException
                    '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 START
                    Logger.Error(ex.Message, ex)
                    '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 END
                    Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.UpdateDeleteFlgCalItemSqlError)

                End Try

            End Using

        End Function

    End Class

    Public Class CalTodoItemDataTable
        Inherits Global.System.ComponentModel.Component

        ''' <summary>
        ''' カレンダーTodo情報テーブルに登録します。
        ''' </summary>
        ''' <param name="dataRow">引数</param>
        ''' <returns>登録件数</returns>
        ''' <remarks></remarks>
        Public Function InsertCalTodoItem(ByVal dataRow As IC3040403DataSet.CalTodoItemDataTableRow) As Integer

            Using query As New DBUpdateQuery("IC3040403_006")

                Try

                    Dim sql As New StringBuilder
                    With sql
                        .Append(" INSERT /* IC3040403_006 */ ")
                        .Append("   INTO TBL_CAL_TODOITEM( ")
                        .Append("        TODOID ")
                        .Append("      , CALID ")
                        .Append("      , UNIQUEID ")
                        .Append("      , RECURRENCEID ")
                        .Append("      , CHGSEQNO ")
                        .Append("      , ACTSTAFFSTRCD ")
                        .Append("      , ACTSTAFFCD ")
                        .Append("      , RECSTAFFSTRCD ")
                        .Append("      , RECSTAFFCD ")
                        .Append("      , CONTACTNO ")
                        .Append("      , SUMMARY ")
                        .Append("      , STARTTIME ")
                        .Append("      , ENDTIME ")
                        .Append("      , STARTTIMEFLG ")
                        .Append("      , TIMEFLG ")
                        .Append("      , ALLDAYFLG ")
                        .Append("      , MEMO ")
                        .Append("      , ICROPCOLOR ")
                        .Append("      , PARENTDIV ")
                        .Append("      , RRULEFLG ")
                        .Append("      , RRULE_FREQ ")
                        .Append("      , RRULE_INTERVAL ")
                        .Append("      , RRULE_UNTIL ")
                        .Append("      , RRULE_TEXT ")
                        .Append("      , COMPLETIONFLG ")
                        .Append("      , COMPLETIONDATE ")
                        .Append("      , DELFLG ")
                        .Append("      , DELDATE ")
                        .Append("      , CREATEDATE ")
                        .Append("      , UPDATEDATE ")
                        .Append("      , CREATEACCOUNT ")
                        .Append("      , UPDATEACCOUNT ")
                        .Append("      , CREATEID ")
                        '2012/03/01 SKFC 加藤 【SALES_2】受注後工程の対応 START
                        '.Append("      , UPDATEID) ")
                        .Append("      , UPDATEID ")
                        '2014/04/03 SKFC 渡邊 NEXTSTEP_CALDAV START
                        '.Append("      , PROCESSDIV) ")
                        .Append("      , PROCESSDIV")
                        '2012/03/01 SKFC 加藤 【SALES_2】受注後工程の対応 END
                        .Append("      , CONTACT_NAME")
                        .Append("      , ACT_ODR_NAME")
                        .Append("      , ODR_DIV")
                        .Append("      , AFTER_ODR_ACT_ID)")
                        '2014/04/03 SKFC 渡邊 NEXTSTEP_CALDAV END
                        .Append(" VALUES (")
                        .Append("        :TODOID ")
                        .Append("      , :CALID ")
                        .Append("      , :UNIQUEID ")
                        .Append("      , :RECURRENCEID ")
                        .Append("      , :CHGSEQNO ")
                        .Append("      , :ACTSTAFFSTRCD ")
                        .Append("      , :ACTSTAFFCD ")
                        .Append("      , :RECSTAFFSTRCD ")
                        .Append("      , :RECSTAFFCD ")
                        .Append("      , :CONTACTNO ")
                        .Append("      , :SUMMARY ")
                        .Append("      , :STARTTIME ")
                        .Append("      , :ENDTIME ")
                        .Append("      , :STARTTIMEFLG ")
                        .Append("      , :TIMEFLG ")
                        .Append("      , :ALLDAYFLG ")
                        .Append("      , :MEMO ")
                        .Append("      , :ICROPCOLOR ")
                        .Append("      , :PARENTDIV ")
                        .Append("      , '0' ")
                        .Append("      , :RRULE_FREQ ")
                        .Append("      , :RRULE_INTERVAL ")
                        .Append("      , :RRULE_UNTIL ")
                        .Append("      , :RRULE_TEXT ")
                        .Append("      , :COMPLETIONFLG ")
                        .Append("      , :COMPLETIONDATE ")
                        .Append("      , :DELFLG ")
                        .Append("      , :DELDATE ")
                        .Append("      , SYSDATE ")
                        .Append("      , SYSDATE ")
                        .Append("      , :CREATEACCOUNT ")
                        .Append("      , :UPDATEACCOUNT ")
                        .Append("      , :CREATEID ")
                        '2012/03/01 SKFC 加藤 【SALES_2】受注後工程の対応 START
                        '.Append("      , :UPDATEID) ")
                        .Append("      , :UPDATEID ")
                        '2014/04/03 SKFC 渡邊 NEXTSTEP_CALDAV START
                        '.Append("      , :PROCESSDIV) ")
                        .Append("      , :PROCESSDIV ")
                        '2012/03/01 SKFC 加藤 【SALES_2】受注後工程の対応 END
                        .Append("      , :CONTACT_NAME ")
                        .Append("      , :ACT_ODR_NAME ")
                        .Append("      , :ODR_DIV ")
                        .Append("      , :AFTER_ODR_ACT_ID) ")
                        '2014/04/03 SKFC 渡邊 NEXTSTEP_CALDAV END
                    End With

                    query.CommandText = sql.ToString()

                    Using convert As New IC3040403DataSetTableAdapters.IC3040403DataAdapter

                        'SQLパラメータ設定
                        query.AddParameterWithTypeValue(ConstCode.TodoItemTableTodoId, OracleDbType.Varchar2, dataRow.TODOID)
                        query.AddParameterWithTypeValue(ConstCode.TodoItemTableCalId, OracleDbType.Varchar2, dataRow.CALID)
                        query.AddParameterWithTypeValue(ConstCode.TodoItemTableUniqueId, OracleDbType.Varchar2, dataRow.UNIQUEID)
                        query.AddParameterWithTypeValue(ConstCode.TodoItemTableRecurrenceId, OracleDbType.Varchar2, dataRow.RECURRENCEID)
                        query.AddParameterWithTypeValue(ConstCode.TodoItemTableChgSeqNo, OracleDbType.Int32, convert.ConvertStringEmpty(dataRow.CHGSEQNO))
                        query.AddParameterWithTypeValue(ConstCode.TodoItemTableActStaffStrCD, OracleDbType.Char, dataRow.ACTSTAFFSTRCD)
                        query.AddParameterWithTypeValue(ConstCode.TodoItemTableActStaffCD, OracleDbType.Varchar2, dataRow.ACTSTAFFCD)
                        query.AddParameterWithTypeValue(ConstCode.TodoItemTableRecStaffStrCD, OracleDbType.Char, dataRow.RECSTAFFSTRCD)
                        query.AddParameterWithTypeValue(ConstCode.TodoItemTableRecStaffCD, OracleDbType.Varchar2, dataRow.RECSTAFFCD)
                        '2014/07/15 SKFC渡邊 NEXTSTEP_CALDAV 不具合修正 Nvarchar2対応　START
                        'query.AddParameterWithTypeValue(ConstCode.TodoItemTableContactNo, OracleDbType.Int32, convert.ConvertStringEmpty(dataRow.CONTACTNO))
                        query.AddParameterWithTypeValue(ConstCode.TodoItemTableContactNo, OracleDbType.NVarchar2, dataRow.CONTACTNO)
                        '2014/07/15 SKFC渡邊 NEXTSTEP_CALDAV 不具合修正　END
                        query.AddParameterWithTypeValue(ConstCode.TodoItemTableSummary, OracleDbType.NVarchar2, dataRow.SUMMARY)
                        query.AddParameterWithTypeValue(ConstCode.TodoItemTableStartTime, OracleDbType.Date, convert.ConvertStringDateTime(dataRow.STARTTIME))
                        query.AddParameterWithTypeValue(ConstCode.TodoItemTableEndTime, OracleDbType.Date, convert.ConvertStringDateTime(dataRow.ENDTIME))
                        query.AddParameterWithTypeValue(ConstCode.TodoItemTableStartTimeFlg, OracleDbType.Char, dataRow.STARTTIMEFLG)
                        query.AddParameterWithTypeValue(ConstCode.TodoItemTableTimeFlg, OracleDbType.Char, dataRow.TIMEFLG)
                        query.AddParameterWithTypeValue(ConstCode.TodoItemTableAllDayFlg, OracleDbType.Char, dataRow.ALLDAYFLG)
                        query.AddParameterWithTypeValue(ConstCode.TodoItemTableMemo, OracleDbType.NVarchar2, dataRow.MEMO)
                        query.AddParameterWithTypeValue(ConstCode.TodoItemTableIcropColor, OracleDbType.Varchar2, dataRow.ICROPCOLOR)
                        query.AddParameterWithTypeValue(ConstCode.TodoItemTableRruleFreq, OracleDbType.Varchar2, dataRow.RRULE_FREQ)
                        query.AddParameterWithTypeValue(ConstCode.TodoItemTableRruleInterval, OracleDbType.Int32, convert.ConvertStringEmpty(dataRow.RRULE_INTERVAL))
                        query.AddParameterWithTypeValue(ConstCode.TodoItemTableRruleUntil, OracleDbType.Date, convert.ConvertStringDateTime(dataRow.RRULE_UNTIL))
                        query.AddParameterWithTypeValue(ConstCode.TodoItemTableRruleText, OracleDbType.Varchar2, dataRow.RRULE_TEXT)
                        query.AddParameterWithTypeValue(ConstCode.TodoItemTableCompletionFlg, OracleDbType.Char, dataRow.COMPLETIONFLG)
                        query.AddParameterWithTypeValue(ConstCode.TodoItemTableCompletionDate, OracleDbType.Date, convert.ConvertStringDateTime(dataRow.COMPLETIONDATE))
                        query.AddParameterWithTypeValue(ConstCode.TodoItemTableDelFlg, OracleDbType.Char, dataRow.DELFLG)
                        query.AddParameterWithTypeValue(ConstCode.TodoItemTableDelDate, OracleDbType.Date, convert.ConvertStringDateTime(dataRow.DELDATE))
                        query.AddParameterWithTypeValue(ConstCode.TodoItemTableCreateAccount, OracleDbType.Varchar2, dataRow.CREATEACCOUNT)
                        query.AddParameterWithTypeValue(ConstCode.TodoItemTableUpdateAccount, OracleDbType.Varchar2, dataRow.UPDATEACCOUNT)
                        query.AddParameterWithTypeValue(ConstCode.TodoItemTableCreateId, OracleDbType.Varchar2, dataRow.CREATEID)
                        query.AddParameterWithTypeValue(ConstCode.TodoItemTableUpdateId, OracleDbType.Varchar2, dataRow.UPDATEID)
                        query.AddParameterWithTypeValue(ConstCode.TodoItemTableParentDiv, OracleDbType.Char, dataRow.PARENTDIV)
                        '2012/03/01 SKFC 加藤 【SALES_2】受注後工程の対応 START
                        query.AddParameterWithTypeValue(ConstCode.TodoItemTableProcessDiv, OracleDbType.Varchar2, dataRow.PROCESSDIV)
                        '2012/03/01 SKFC 加藤 【SALES_2】受注後工程の対応 END
                        '2014/04/03 SKFC 渡邊 NEXTSTEP_CALDAV START
                        query.AddParameterWithTypeValue(ConstCode.TodoItemTableContactName, OracleDbType.NVarchar2, dataRow.CONTACT_NAME)
                        query.AddParameterWithTypeValue(ConstCode.TodoItemTableActOdrName, OracleDbType.NVarchar2, dataRow.ACT_ODR_NAME)
                        query.AddParameterWithTypeValue(ConstCode.TodoItemTableOdrDiv, OracleDbType.Char, dataRow.ODR_DIV)
                        query.AddParameterWithTypeValue(ConstCode.TodoItemTableAfterOdrActID, OracleDbType.Varchar2, dataRow.AFTER_ODR_ACT_ID)
                        '2014/04/03 SKFC 渡邊 NEXTSTEP_CALDAV END
                    End Using

                    'SQL実行（登録行を返却）
                    Dim Count As Integer = query.Execute()
                    Return Count

                Catch ex As SystemException
                    '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 START
                    Logger.Error(ex.Message, ex)
                    '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 END
                    Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.InsertCalTodoItemSqlError)

                End Try

            End Using

        End Function

        ''' <summary>
        ''' カレンダーTodo情報テーブルを更新します。
        ''' </summary>
        ''' <param name="dataRow">引数</param>
        ''' <returns>更新件数</returns>
        ''' <remarks></remarks>
        Public Function UpdateCalTodoItem(ByVal dataRow As IC3040403DataSet.CalTodoItemDataTableRow) As Integer

            Using query As New DBUpdateQuery("IC3040403_007")

                Try

                    Dim sql As New StringBuilder

                    Using Convert As New IC3040403DataSetTableAdapters.IC3040403DataAdapter

                        With sql
                            .Append(" UPDATE  /* IC3040403_007 */ TBL_CAL_TODOITEM")
                            .Append("    SET UPDATEDATE = SYSDATE")
                            .Append("      , UPDATEACCOUNT = :UPDATEACCOUNT ")
                            query.AddParameterWithTypeValue(ConstCode.TodoItemTableUpdateAccount, OracleDbType.Varchar2, dataRow.UPDATEACCOUNT)
                            .Append("      , UPDATEID = :UPDATEID")
                            query.AddParameterWithTypeValue(ConstCode.TodoItemTableUpdateId, OracleDbType.Varchar2, dataRow.UPDATEID)
                            If dataRow.ACTSTAFFSTRCD IsNot Nothing Then
                                .Append("      , ACTSTAFFSTRCD = :ACTSTAFFSTRCD ")
                                query.AddParameterWithTypeValue(ConstCode.TodoItemTableActStaffStrCD, OracleDbType.Varchar2, dataRow.ACTSTAFFSTRCD)
                            End If
                            If dataRow.ACTSTAFFCD IsNot Nothing Then
                                .Append("      , ACTSTAFFCD = :ACTSTAFFCD ")
                                query.AddParameterWithTypeValue(ConstCode.TodoItemTableActStaffCD, OracleDbType.Char, dataRow.ACTSTAFFCD)
                            End If
                            If dataRow.RECSTAFFSTRCD IsNot Nothing Then
                                .Append("      , RECSTAFFSTRCD = :RECSTAFFSTRCD ")
                                query.AddParameterWithTypeValue(ConstCode.TodoItemTableRecStaffStrCD, OracleDbType.Varchar2, dataRow.RECSTAFFSTRCD)
                            End If
                            If dataRow.RECSTAFFCD IsNot Nothing Then
                                .Append("      , RECSTAFFCD = :RECSTAFFCD ")
                                query.AddParameterWithTypeValue(ConstCode.TodoItemTableRecStaffCD, OracleDbType.Char, dataRow.RECSTAFFCD)
                            End If
                            If dataRow.CONTACTNO IsNot Nothing Then
                                .Append("      , CONTACTNO = :CONTACTNO ")
                                '2014/07/15 SKFC渡邊 NEXTSTEP_CALDAV 不具合修正 Nvarchar2対応　START
                                'query.AddParameterWithTypeValue(ConstCode.TodoItemTableContactNo, OracleDbType.Int32, Convert.ConvertStringEmpty(dataRow.CONTACTNO))
                                query.AddParameterWithTypeValue(ConstCode.TodoItemTableContactNo, OracleDbType.NVarchar2, dataRow.CONTACTNO)
                                '2014/07/15 SKFC渡邊 NEXTSTEP_CALDAV 不具合修正 Nvarchar2対応　END
                            End If
                            If dataRow.SUMMARY IsNot Nothing Then
                                .Append("      , SUMMARY = :SUMMARY ")
                                query.AddParameterWithTypeValue(ConstCode.TodoItemTableSummary, OracleDbType.NVarchar2, dataRow.SUMMARY)
                            End If
                            If dataRow.STARTTIME IsNot Nothing Then
                                .Append("      , STARTTIME = :STARTTIME ")
                                query.AddParameterWithTypeValue(ConstCode.TodoItemTableStartTime, OracleDbType.Date, Convert.ConvertStringDateTime(dataRow.STARTTIME))
                            End If
                            If dataRow.ENDTIME IsNot Nothing Then
                                .Append("      , ENDTIME = :ENDTIME ")
                                query.AddParameterWithTypeValue(ConstCode.TodoItemTableEndTime, OracleDbType.Date, Convert.ConvertStringDateTime(dataRow.ENDTIME))
                            End If
                            If dataRow.STARTTIMEFLG IsNot Nothing Then
                                .Append("      , STARTTIMEFLG = :STARTTIMEFLG ")
                                query.AddParameterWithTypeValue(ConstCode.TodoItemTableStartTimeFlg, OracleDbType.Char, dataRow.STARTTIMEFLG)
                            End If
                            If dataRow.TIMEFLG IsNot Nothing Then
                                .Append("      , TIMEFLG = :TIMEFLG ")
                                query.AddParameterWithTypeValue(ConstCode.TodoItemTableTimeFlg, OracleDbType.Char, dataRow.TIMEFLG)
                            End If
                            If dataRow.ALLDAYFLG IsNot Nothing Then
                                .Append("      , ALLDAYFLG = :ALLDAYFLG ")
                                query.AddParameterWithTypeValue(ConstCode.TodoItemTableAllDayFlg, OracleDbType.Char, dataRow.ALLDAYFLG)
                            End If
                            If dataRow.MEMO IsNot Nothing Then
                                .Append("      , MEMO = :MEMO ")
                                query.AddParameterWithTypeValue(ConstCode.TodoItemTableMemo, OracleDbType.NVarchar2, dataRow.MEMO)
                            End If
                            If dataRow.ICROPCOLOR IsNot Nothing Then
                                .Append("      , ICROPCOLOR = :ICROPCOLOR ")
                                query.AddParameterWithTypeValue(ConstCode.TodoItemTableIcropColor, OracleDbType.Varchar2, dataRow.ICROPCOLOR)
                            End If

                            '2012/03/01 SKFC 加藤 【SALES_2】受注後工程の対応 START
                            If dataRow.PROCESSDIV IsNot Nothing Then
                                .Append("      , PROCESSDIV = :PROCESSDIV ")
                                query.AddParameterWithTypeValue(ConstCode.TodoItemTableProcessDiv, OracleDbType.Varchar2, dataRow.PROCESSDIV)
                            End If

                            If dataRow.COMPLETIONFLG IsNot Nothing Then
                                .Append("      , COMPLETIONFLG = :COMPLETIONFLG ")
                                query.AddParameterWithTypeValue(ConstCode.TodoItemTableCompletionFlg, OracleDbType.Varchar2, dataRow.COMPLETIONFLG)
                            End If

                            If dataRow.COMPLETIONDATE IsNot Nothing Then
                                .Append("      , COMPLETIONDATE = :COMPLETIONDATE ")
                                query.AddParameterWithTypeValue(ConstCode.TodoItemTableCompletionDate, OracleDbType.Date, Convert.ConvertStringDateTime(dataRow.COMPLETIONDATE))
                            End If

                            '2012/03/01 SKFC 加藤 【SALES_2】受注後工程の対応 END

                            '2014/04/03 SKFC 渡邊 NEXTSTEP_CALDAV START
                            If dataRow.CONTACT_NAME IsNot Nothing Then
                                .Append("      , CONTACT_NAME = :CONTACT_NAME ")
                                query.AddParameterWithTypeValue(ConstCode.TodoItemTableContactName, OracleDbType.NVarchar2, dataRow.CONTACT_NAME)
                            End If

                            If dataRow.ODR_DIV IsNot Nothing Then
                                .Append("      , ODR_DIV = :ODR_DIV ")
                                query.AddParameterWithTypeValue(ConstCode.TodoItemTableOdrDiv, OracleDbType.Char, dataRow.ODR_DIV)
                            End If

                            '2014/07/22 SKFC 渡邉 NEXTSTEP_CALDAV 不具合修正 START
                            '2014/07/15 SKFC渡邊 NEXTSTEP_CALDAV 不具合修正　START
                            If dataRow.ACT_ODR_NAME IsNot Nothing Then
                                .Append("      , ACT_ODR_NAME = :ACT_ODR_NAME ")
                                query.AddParameterWithTypeValue(ConstCode.TodoItemTableActOdrName, OracleDbType.NVarchar2, dataRow.ACT_ODR_NAME)
                            End If
                            '2014/07/15 SKFC渡邊 NEXTSTEP_CALDAV 不具合修正　START
                            '2014/07/22 SKFC 渡邉 NEXTSTEP_CALDAV 不具合修正 END

                            '2014/07/01 SKFC 渡邉 NEXTSTEP_CALDAV 不具合修正 START
                            'If dataRow.AFTER_ODR_ACT_ID IsNot Nothing Then
                            '.Append("      , AFTER_ODR_ACT_ID = :AFTER_ODR_ACT_ID ")
                            'query.AddParameterWithTypeValue(ConstCode.TodoItemTableAfterOdrActID, OracleDbType.Varchar2, dataRow.AFTER_ODR_ACT_ID)
                            'End If
                            '2014/04/03 SKFC 渡邊 NEXTSTEP_CALDAV END
                            '2014/07/11 SKFC 渡邉 NEXTSTEP_CALDAV 不具合修正 START
                            If dataRow.AFTER_ODR_ACT_ID IsNot Nothing And dataRow.TODOID IsNot Nothing Then
                                .Append("  WHERE AFTER_ODR_ACT_ID = :AFTER_ODR_ACT_ID ")
                                query.AddParameterWithTypeValue(ConstCode.TodoItemTableAfterOdrActID, OracleDbType.Varchar2, dataRow.AFTER_ODR_ACT_ID)
                            ElseIf dataRow.AFTER_ODR_ACT_ID IsNot Nothing Then
                                .Append("  WHERE AFTER_ODR_ACT_ID = :AFTER_ODR_ACT_ID ")
                                query.AddParameterWithTypeValue(ConstCode.TodoItemTableAfterOdrActID, OracleDbType.Varchar2, dataRow.AFTER_ODR_ACT_ID)
                                '.Append(" WHERE  TODOID = :TODOID ")
                                'query.AddParameterWithTypeValue(ConstCode.TodoItemTableTodoId, OracleDbType.Varchar2, dataRow.TODOID)

                                'If dataRow.TODOID IsNot Nothing Then
                            Else
                                .Append(" WHERE  TODOID = :TODOID ")
                                query.AddParameterWithTypeValue(ConstCode.TodoItemTableTodoId, OracleDbType.Varchar2, dataRow.TODOID)
                            End If
                            '2014/07/01 SKFC 渡邉 NEXTSTEP_CALDAV 不具合修正 END
                            '2014/07/11 SKFC 渡邉 NEXTSTEP_CALDAV 不具合修正 END
                        End With

                    End Using

                    query.CommandText = sql.ToString()

                    'SQL実行（更新行を返却）
                    Dim Count As Integer = query.Execute()
                    Return Count

                Catch ex As SystemException
                    '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 START
                    Logger.Error(ex.Message, ex)
                    '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 END
                    Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.UpdateCalTodoItemSqlError)

                End Try

            End Using

        End Function

'2019/02/28 SKFC二村 TR-V4-TMT-20190131-001 START
        ''' <summary>
        ''' カレンダーTodo情報テーブル更新ロックを取得
        ''' </summary>
        ''' <param name="dataRow">引数</param>
        ''' <returns>対象件数</returns>
        ''' <remarks></remarks>
        Public Function GetCalTodoItemLock(ByVal dataRow As IC3040403DataSet.CalTodoItemDataTableRow) As Integer

            Dim table As IC3040403DataSet.IdTableDataTable
            Dim result As Integer = -1

            Using query As New DBSelectQuery(Of IC3040403DataSet.IdTableDataTable)("IC3040403_028")

                Try

                    Dim sql As New StringBuilder

                    With sql
                        .AppendLine(" SELECT /* IC3040403_028 */ ")
                        .AppendLine("        TODOID ")
                        .AppendLine("   FROM TBL_CAL_TODOITEM ")
                        If dataRow.AFTER_ODR_ACT_ID IsNot Nothing And dataRow.TODOID IsNot Nothing Then
                            .AppendLine("  WHERE AFTER_ODR_ACT_ID = :AFTER_ODR_ACT_ID ")
                            query.AddParameterWithTypeValue(ConstCode.TodoItemTableAfterOdrActID, OracleDbType.Varchar2, dataRow.AFTER_ODR_ACT_ID)
                        ElseIf dataRow.AFTER_ODR_ACT_ID IsNot Nothing Then
                            .AppendLine("  WHERE AFTER_ODR_ACT_ID = :AFTER_ODR_ACT_ID ")
                            query.AddParameterWithTypeValue(ConstCode.TodoItemTableAfterOdrActID, OracleDbType.Varchar2, dataRow.AFTER_ODR_ACT_ID)
                        Else
                            .AppendLine("  WHERE TODOID = :TODOID ")
                            query.AddParameterWithTypeValue(ConstCode.TodoItemTableTodoId, OracleDbType.Varchar2, dataRow.TODOID)
                        End If
                        .AppendLine("  ORDER BY TODOID ASC ")
                        Dim env As New SystemEnvSetting
                        .AppendLine("    FOR UPDATE WAIT " + env.GetLockWaitTime())
                    End With

                    query.CommandText = sql.ToString()
                    'SQLパラメータ設定

                    'SQL実行（結果表を返却）
                    table = query.GetData()

                    If table.Count >= 0 Then
                        result = table.Count
                    End If

                    Return result

                Catch ex As Exception
                    Logger.Error(ex.Message, ex)
                    Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.UpdateCalTodoItemSqlError)

                End Try

            End Using

        End Function
'2019/02/28 SKFC二村 TR-V4-TMT-20190131-001 END

        ''' <summary>
        ''' カレンダーTodo情報テーブルの削除フラグを更新します。
        ''' </summary>
        ''' <param name="dataRow">引数</param>
        ''' <returns>更新件数</returns>
        ''' <remarks></remarks>
        Public Function UpdateDeleteFlgCalTodoItem(ByVal dataRow As IC3040403DataSet.CalTodoItemDataTableRow) As Integer

            Using query As New DBUpdateQuery("IC3040403_008")

                Try

                    Dim sql As New StringBuilder

                    Using Convert As New IC3040403DataSetTableAdapters.IC3040403DataAdapter

                        With sql
                            .Append(" UPDATE /* IC3040403_008 */ TBL_CAL_TODOITEM")
                            .Append("    SET UPDATEDATE = SYSDATE")
                            .Append("      , UPDATEACCOUNT = :UPDATEACCOUNT ")
                            query.AddParameterWithTypeValue(ConstCode.TodoItemTableUpdateAccount, OracleDbType.Varchar2, dataRow.UPDATEACCOUNT)
                            .Append("      , UPDATEID = :UPDATEID")
                            query.AddParameterWithTypeValue(ConstCode.TodoItemTableUpdateId, OracleDbType.Varchar2, dataRow.UPDATEID)
                            .Append("      , DELFLG = '1'")
                            If dataRow.DELDATE Is Nothing Then
                                .Append("      , DELDATE = SYSDATE")
                            Else
                                .Append("      , DELDATE = :DELDATE")
                                query.AddParameterWithTypeValue(ConstCode.TodoItemTableDelDate, OracleDbType.Date, Convert.ConvertStringDateTime(dataRow.DELDATE))
                            End If
                            .Append("  WHERE DELFLG = '0' ")
                            ' 2012/03/01 SKFC 加藤 【SALES_2】受注後工程の対応 START
                            '.Append("    AND COMPLETIONFLG = '0' ")
                            If dataRow.COMPLETIONFLG IsNot Nothing Then
                                .Append("  AND COMPLETIONFLG = :COMPLETIONFLG ")
                                query.AddParameterWithTypeValue(ConstCode.TodoItemTableCompletionFlg, OracleDbType.Char, dataRow.COMPLETIONFLG)
                            End If
                            ' 2012/03/01 SKFC 加藤 【SALES_2】受注後工程の対応 END
                            If dataRow.CALID IsNot Nothing Then
                                .Append("  AND CALID = :CALID ")
                                query.AddParameterWithTypeValue(ConstCode.TodoItemTableCalId, OracleDbType.Varchar2, dataRow.CALID)
                            End If
                            If dataRow.TODOID IsNot Nothing Then
                                .Append("  AND TODOID = :TODOID ")
                                query.AddParameterWithTypeValue(ConstCode.TodoAlarmTableTodoId, OracleDbType.Varchar2, dataRow.TODOID)
                            End If
                            '2014/07/11 SKFC 渡邉 NEXTSTEP_CALDAV 不具合修正 START
                            If dataRow.AFTER_ODR_ACT_ID IsNot Nothing Then
                                .Append("  AND AFTER_ODR_ACT_ID = :AFTER_ODR_ACT_ID ")
                                query.AddParameterWithTypeValue(ConstCode.TodoItemTableAfterOdrActID, OracleDbType.Varchar2, dataRow.AFTER_ODR_ACT_ID)
                            End If
                            '2014/07/11 SKFC 渡邉 NEXTSTEP_CALDAV 不具合修正 END
                        End With

                    End Using

                    query.CommandText = sql.ToString()

                    'SQL実行（更新行を返却）
                    Dim Count As Integer = query.Execute()
                    Return Count

                Catch ex As SystemException
                    '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 START
                    Logger.Error(ex.Message, ex)
                    '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 END
                    Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.UpdateDeleteFlgCalTodoItemSqlError)

                End Try

            End Using

        End Function

'2019/02/28 SKFC二村 TR-V4-TMT-20190131-001 START
        ''' <summary>
        ''' カレンダーTodo情報テーブルの削除フラグ更新ロックを取得
        ''' </summary>
        ''' <param name="dataRow">引数</param>
        ''' <returns>対象件数</returns>
        ''' <remarks></remarks>
        Public Function GetDeleteFlgCalTodoItemLock(ByVal dataRow As IC3040403DataSet.CalTodoItemDataTableRow) As Integer

            Dim table As IC3040403DataSet.IdTableDataTable
            Dim result As Integer = -1

            Using query As New DBSelectQuery(Of IC3040403DataSet.IdTableDataTable)("IC3040403_029")

                Try

                    Dim sql As New StringBuilder

                    With sql
                        .AppendLine(" SELECT /* IC3040403_029 */ ")
                        .AppendLine("        TODOID ")
                        .AppendLine("   FROM TBL_CAL_TODOITEM ")
                        .AppendLine("  WHERE DELFLG = '0' ")
                        If dataRow.COMPLETIONFLG IsNot Nothing Then
                            .AppendLine("  AND COMPLETIONFLG = :COMPLETIONFLG ")
                            query.AddParameterWithTypeValue(ConstCode.TodoItemTableCompletionFlg, OracleDbType.Char, dataRow.COMPLETIONFLG)
                        End If
                        If dataRow.CALID IsNot Nothing Then
                            .AppendLine("  AND CALID = :CALID ")
                            query.AddParameterWithTypeValue(ConstCode.TodoItemTableCalId, OracleDbType.Varchar2, dataRow.CALID)
                        End If
                        If dataRow.TODOID IsNot Nothing Then
                            .AppendLine("  AND TODOID = :TODOID ")
                            query.AddParameterWithTypeValue(ConstCode.TodoAlarmTableTodoId, OracleDbType.Varchar2, dataRow.TODOID)
                        End If
                        If dataRow.AFTER_ODR_ACT_ID IsNot Nothing Then
                            .AppendLine("  AND AFTER_ODR_ACT_ID = :AFTER_ODR_ACT_ID ")
                            query.AddParameterWithTypeValue(ConstCode.TodoItemTableAfterOdrActID, OracleDbType.Varchar2, dataRow.AFTER_ODR_ACT_ID)
                        End If
                        .AppendLine("  ORDER BY TODOID ASC ")
                        Dim env As New SystemEnvSetting
                        .AppendLine("    FOR UPDATE WAIT " + env.GetLockWaitTime())
                    End With

                    query.CommandText = sql.ToString()
                    'SQLパラメータ設定

                    'SQL実行（結果表を返却）
                    table = query.GetData()

                    If table.Count >= 0 Then
                        result = table.Count
                    End If

                    Return result

                Catch ex As Exception
                    Logger.Error(ex.Message, ex)
                    Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.UpdateDeleteFlgCalTodoItemSqlError)

                End Try

            End Using

        End Function
'2019/02/28 SKFC二村 TR-V4-TMT-20190131-001 END

        ''' <summary>
        ''' カレンダーTodo情報テーブルの削除フラグを更新します。
        ''' </summary>
        ''' <param name="dataRow">引数</param>
        ''' <returns>更新件数</returns>
        ''' <remarks></remarks>
        Public Function UpdateAfterOrderDeleteFlgCalTodoItem(ByVal dataRow As IC3040403DataSet.CalTodoItemDataTableRow) As Integer

            Using query As New DBUpdateQuery("IC3040403_026")

                Try

                    Dim sql As New StringBuilder

                    Using Convert As New IC3040403DataSetTableAdapters.IC3040403DataAdapter

                        With sql
                            .Append(" UPDATE /* IC3040403_026 */ TBL_CAL_TODOITEM T01")
                            .Append("    SET UPDATEDATE = SYSDATE")
                            .Append("      , UPDATEACCOUNT = :UPDATEACCOUNT ")
                            query.AddParameterWithTypeValue(ConstCode.TodoItemTableUpdateAccount, OracleDbType.Varchar2, dataRow.UPDATEACCOUNT)
                            .Append("      , UPDATEID = :UPDATEID")
                            query.AddParameterWithTypeValue(ConstCode.TodoItemTableUpdateId, OracleDbType.Varchar2, dataRow.UPDATEID)
                            .Append("      , DELFLG = '1'")
                            If dataRow.DELDATE Is Nothing Then
                                .Append("      , DELDATE = SYSDATE")
                            Else
                                .Append("      , DELDATE = :DELDATE")
                                query.AddParameterWithTypeValue(ConstCode.TodoItemTableDelDate, OracleDbType.Date, Convert.ConvertStringDateTime(dataRow.DELDATE))
                            End If
                            .Append("  WHERE DELFLG = '0' ")
                            If dataRow.CALID IsNot Nothing Then
                                .Append("  AND CALID = :CALID ")
                                query.AddParameterWithTypeValue(ConstCode.EventItemTableCalId, OracleDbType.Varchar2, dataRow.CALID)
                            End If
                            .Append("    AND EXISTS(")
                            .Append("     SELECT 1 ")
                            .Append("       FROM TBL_CAL_TODOITEM T02 ")
                            .Append("      WHERE T02.TODOID = T01.TODOID ")
                            .Append("        AND COMPLETIONFLG = '0'")
                            .Append("        AND DELFLG = '0')")
                        End With

                    End Using

                    query.CommandText = sql.ToString()

                    'SQL実行（更新行を返却）
                    Dim Count As Integer = query.Execute()
                    Return Count

                Catch ex As SystemException
                    Logger.Error(ex.Message, ex)
                    Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.UpdateAfterOrderDeleteFlgCalTodoItemSqlError)

                End Try

            End Using

        End Function

'2019/02/28 SKFC二村 TR-V4-TMT-20190131-001 START
        ''' <summary>
        ''' カレンダーTodo情報テーブルの削除フラグ更新ロックを取得
        ''' </summary>
        ''' <param name="dataRow">引数</param>
        ''' <returns>対象件数</returns>
        ''' <remarks></remarks>
        Public Function GetAfterOrderDeleteFlgCalTodoItemLock(ByVal dataRow As IC3040403DataSet.CalTodoItemDataTableRow) As Integer

            Dim table As IC3040403DataSet.IdTableDataTable
            Dim result As Integer = -1

            Using query As New DBSelectQuery(Of IC3040403DataSet.IdTableDataTable)("IC3040403_030")

                Try

                    Dim sql As New StringBuilder

                    With sql
                        .AppendLine(" SELECT /* IC3040403_030 */ ")
                        .AppendLine("        T01.TODOID")
                        .AppendLine("   FROM TBL_CAL_TODOITEM T01 ")
                        .AppendLine("  WHERE T01.DELFLG = '0' ")
                        If dataRow.CALID IsNot Nothing Then
                            .AppendLine("  AND T01.CALID = :CALID ")
                            query.AddParameterWithTypeValue(ConstCode.EventItemTableCalId, OracleDbType.Varchar2, dataRow.CALID)
                        End If
                        .AppendLine("    AND EXISTS(")
                        .AppendLine("     SELECT 1 ")
                        .AppendLine("       FROM TBL_CAL_TODOITEM T02 ")
                        .AppendLine("      WHERE T02.TODOID = T01.TODOID ")
                        .AppendLine("        AND T02.COMPLETIONFLG = '0'")
                        .AppendLine("        AND T02.DELFLG = '0')")
                        .AppendLine("  ORDER BY T01.TODOID ASC ")
                        Dim env As New SystemEnvSetting
                        .AppendLine("    FOR UPDATE WAIT " + env.GetLockWaitTime())
                    End With

                    query.CommandText = sql.ToString()
                    'SQLパラメータ設定

                    'SQL実行（結果表を返却）
                    table = query.GetData()

                    If table.Count >= 0 Then
                        result = table.Count
                    End If

                    Return result

                Catch ex As Exception
                    Logger.Error(ex.Message, ex)
                    Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.UpdateAfterOrderDeleteFlgCalTodoItemSqlError)

                End Try

            End Using

        End Function
'2019/02/28 SKFC二村 TR-V4-TMT-20190131-001 END

        ''' <summary>
        ''' カレンダーTodo情報テーブルの完了フラグを更新します
        ''' </summary>
        ''' <param name="dataRow">引数</param>
        ''' <returns>更新件数</returns>
        ''' <remarks></remarks>
        Public Function UpdateCompleteFlgCalTodoItem(ByVal dataRow As IC3040403DataSet.CalTodoItemDataTableRow) As Integer

            Using query As New DBUpdateQuery("IC3040403_009")

                Try

                    Dim sql As New StringBuilder

                    Using Convert As New IC3040403DataSetTableAdapters.IC3040403DataAdapter

                        With sql
                            .Append(" UPDATE /* IC3040403_009 */ TBL_CAL_TODOITEM")
                            .Append("    SET UPDATEDATE = SYSDATE ")
                            .Append("      , UPDATEACCOUNT = :UPDATEACCOUNT ")
                            query.AddParameterWithTypeValue(ConstCode.TodoItemTableUpdateAccount, OracleDbType.Varchar2, dataRow.UPDATEACCOUNT)
                            .Append("      , UPDATEID = :UPDATEID ")
                            query.AddParameterWithTypeValue(ConstCode.TodoItemTableUpdateId, OracleDbType.Varchar2, dataRow.UPDATEID)
                            .Append("      , COMPLETIONFLG = '1' ")
                            .Append("      , COMPLETIONDATE = :COMPLETIONDATE")
                            query.AddParameterWithTypeValue(ConstCode.TodoItemTableCompletionDate, OracleDbType.Date, Convert.ConvertStringDateTime(dataRow.COMPLETIONDATE))
                            .Append(" WHERE DELFLG = '0' ")
                            .Append("   AND COMPLETIONFLG = '0' ")
                            .Append("   AND PARENTDIV = '1' ")
                            .Append("   AND CALID = :CALID ")

                            query.AddParameterWithTypeValue(ConstCode.TodoItemTableCalId, OracleDbType.Varchar2, dataRow.CALID)
                        End With

                    End Using

                    query.CommandText = sql.ToString()

                    'SQL実行（更新行を返却）
                    Dim Count As Integer = query.Execute()
                    Return Count

                Catch ex As SystemException
                    '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 START
                    Logger.Error(ex.Message, ex)
                    '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 END
                    Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.UpdateCompleteFlgCalTodoItemSqlError)

                End Try

            End Using

        End Function

'2019/02/28 SKFC二村 TR-V4-TMT-20190131-001 START
        ''' <summary>
        ''' カレンダーTodo情報テーブルの完了フラグ更新ロックを取得
        ''' </summary>
        ''' <param name="dataRow">引数</param>
        ''' <returns>対象件数</returns>
        ''' <remarks></remarks>
        Public Function GetCompleteFlgCalTodoItemLock(ByVal dataRow As IC3040403DataSet.CalTodoItemDataTableRow) As Integer

            Dim table As IC3040403DataSet.IdTableDataTable
            Dim result As Integer = -1

            Using query As New DBSelectQuery(Of IC3040403DataSet.IdTableDataTable)("IC3040403_031")

                Try

                    Dim sql As New StringBuilder

                    With sql
                        .AppendLine(" SELECT /* IC3040403_031 */ ")
                        .AppendLine("        TODOID ")
                        .AppendLine("   FROM TBL_CAL_TODOITEM ")
                        .AppendLine(" WHERE DELFLG = '0' ")
                        .AppendLine("   AND COMPLETIONFLG = '0' ")
                        .AppendLine("   AND PARENTDIV = '1' ")
                        .AppendLine("   AND CALID = :CALID ")
                        query.AddParameterWithTypeValue(ConstCode.TodoItemTableCalId, OracleDbType.Varchar2, dataRow.CALID)
                        .AppendLine("  ORDER BY TODOID ASC ")
                        Dim env As New SystemEnvSetting
                        .AppendLine("    FOR UPDATE WAIT " + env.GetLockWaitTime())
                    End With

                    query.CommandText = sql.ToString()
                    'SQLパラメータ設定

                    'SQL実行（結果表を返却）
                    table = query.GetData()

                    If table.Count >= 0 Then
                        result = table.Count
                    End If

                    Return result

                Catch ex As Exception
                    Logger.Error(ex.Message, ex)
                    Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.UpdateCompleteFlgCalTodoItemSqlError)

                End Try

            End Using

        End Function
'2019/02/28 SKFC二村 TR-V4-TMT-20190131-001 END

    End Class

    Public Class CalTodoAlarmDataTable
        Inherits Global.System.ComponentModel.Component

        ''' <summary>
        ''' カレンダーTodoアラームテーブルに登録します。
        ''' </summary>
        ''' <param name="dataTable">引数</param>
        ''' <returns>登録件数</returns>
        ''' <remarks></remarks>
        Public Function InsertCalTodoAlarms(ByVal dataTable As IC3040403DataSet.CalTodoAlarmDataTableDataTable) As Integer

            Dim insertCount As Integer = 0

            For Each dataRow As IC3040403DataSet.CalTodoAlarmDataTableRow In dataTable

                Using query As New DBUpdateQuery("IC3040403_010")

                    Try

                        Dim sql As New StringBuilder
                        With sql
                            .Append(" INSERT /* IC3040403_010 */ ")
                            .Append("   INTO TBL_CAL_TODOALARM( ")
                            .Append("        TODOID ")
                            .Append("      , SEQNO ")
                            .Append("      , STARTTRIGGER ")
                            .Append("      , CREATEDATE ")
                            .Append("      , UPDATEDATE ")
                            .Append("      , CREATEACCOUNT ")
                            .Append("      , UPDATEACCOUNT ")
                            .Append("      , CREATEID ")
                            .Append("      , UPDATEID ) ")
                            .Append(" VALUES (")
                            .Append("        :TODOID ")
                            .Append("      , :SEQNO ")
                            .Append("      , :STARTTRIGGER ")
                            .Append("      , SYSDATE ")
                            .Append("      , SYSDATE ")
                            .Append("      , :CREATEACCOUNT ")
                            .Append("      , :UPDATEACCOUNT ")
                            .Append("      , :CREATEID ")
                            .Append("      , :UPDATEID ) ")
                        End With

                        query.CommandText = sql.ToString()
                        'SQLパラメータ設定

                        Using Convert As New IC3040403DataSetTableAdapters.IC3040403DataAdapter

                            query.AddParameterWithTypeValue(ConstCode.TodoAlarmTableTodoId, OracleDbType.Varchar2, dataRow.TODOID)
                            query.AddParameterWithTypeValue(ConstCode.TodoAlarmTableSeqNo, OracleDbType.Int32, Convert.ConvertStringEmpty(dataRow.SEQNO))
                            query.AddParameterWithTypeValue(ConstCode.TodoAlarmTableStartTrigger, OracleDbType.Char, dataRow.STARTTRIGGER)
                            query.AddParameterWithTypeValue(ConstCode.TodoAlarmTableCreateAccount, OracleDbType.Varchar2, dataRow.CREATEACCOUNT)
                            query.AddParameterWithTypeValue(ConstCode.TodoAlarmTableUpdateAccount, OracleDbType.Varchar2, dataRow.UPDATEACCOUNT)
                            query.AddParameterWithTypeValue(ConstCode.TodoAlarmTableCreateId, OracleDbType.Varchar2, dataRow.CREATEID)
                            query.AddParameterWithTypeValue(ConstCode.TodoAlarmTableUpdateId, OracleDbType.Varchar2, dataRow.UPDATEID)

                        End Using

                        'SQL実行（登録行を返却）
                        insertCount = query.Execute() + insertCount

                    Catch ex As SystemException
                        '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 START
                        Logger.Error(ex.Message, ex)
                        '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 END
                        Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.InsertCalTodoAlarmsSqlError)

                    End Try

                End Using

            Next

            Return insertCount

        End Function

        ''' <summary>
        ''' カレンダーTodoアラームテーブルの値を削除します。
        ''' </summary>
        ''' <param name="dataRow">引数</param>
        ''' <returns>削除件数</returns>
        ''' <remarks></remarks>
        Public Function DeleteCalTodoAlarm(ByVal dataRow As IC3040403DataSet.CalTodoAlarmDataTableRow) As Integer

            Using query As New DBUpdateQuery("IC3040403_011")

                Try

                    Dim sql As New StringBuilder

                    With sql
                        .Append(" DELETE /* IC3040403_011 */ ")
                        .Append("   FROM TBL_CAL_TODOALARM ")
                        .Append("  WHERE TODOID = :TODOID ")
                    End With

                    query.CommandText = sql.ToString()
                    'SQLパラメータ設定

                    query.AddParameterWithTypeValue(ConstCode.TodoAlarmTableTodoId, OracleDbType.Char, dataRow.TODOID)


                    'SQL実行（削除行を返却）
                    Return query.Execute()

                Catch ex As SystemException
                    '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 START
                    Logger.Error(ex.Message, ex)
                    '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 END
                    Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.DeleteCalTodoAlarmSqlError)

                End Try

            End Using

        End Function

'2019/02/28 SKFC二村 TR-V4-TMT-20190131-001 START
        ''' <summary>
        ''' カレンダーTodoアラームテーブルの値削除ロックを取得
        ''' </summary>
        ''' <param name="dataRow">引数</param>
        ''' <returns>対象件数</returns>
        ''' <remarks></remarks>
        Public Function GetCalTodoAlarmLock(ByVal dataRow As IC3040403DataSet.CalTodoAlarmDataTableRow) As Integer

            Dim table As IC3040403DataSet.IdTableDataTable
            Dim result As Integer = -1

            Using query As New DBSelectQuery(Of IC3040403DataSet.IdTableDataTable)("IC3040403_032")

                Try

                    Dim sql As New StringBuilder

                    With sql
                        .AppendLine(" SELECT /* IC3040403_032 */ ")
                        .AppendLine("        TODOID ")
                        .AppendLine("   FROM TBL_CAL_TODOALARM ")
                        .AppendLine("  WHERE TODOID = :TODOID ")
                        query.AddParameterWithTypeValue(ConstCode.TodoAlarmTableTodoId, OracleDbType.Char, dataRow.TODOID)
                        .AppendLine("  ORDER BY TODOID, SEQNO ASC ")
                        Dim env As New SystemEnvSetting
                        .AppendLine("    FOR UPDATE WAIT " + env.GetLockWaitTime())
                    End With

                    query.CommandText = sql.ToString()
                    'SQLパラメータ設定

                    'SQL実行（結果表を返却）
                    table = query.GetData()

                    If table.Count >= 0 Then
                        result = table.Count
                    End If

                    Return result

                Catch ex As Exception
                    Logger.Error(ex.Message, ex)
                    Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.DeleteCalTodoAlarmSqlError)

                End Try

            End Using

        End Function
'2019/02/28 SKFC二村 TR-V4-TMT-20190131-001 END

    End Class

    Public Class CalEventItemDataTable
        Inherits Global.System.ComponentModel.Component

        ''' <summary>
        ''' カレンダーイベント情報テーブルに登録します。
        ''' </summary>
        ''' <param name="dataRow">引数</param>
        ''' <returns>登録件数</returns>
        ''' <remarks></remarks>
        Public Function InsertCalEventItem(ByVal dataRow As IC3040403DataSet.CalEventItemDataTableRow) As Integer

            Try

                Using query As New DBUpdateQuery("IC3040403_012")

                    Dim sql As New StringBuilder
                    With sql
                        .Append(" INSERT /* IC3040403_012 */ ")
                        .Append("   INTO TBL_CAL_EVENTITEM( ")
                        .Append("        EVENTID ")
                        .Append("      , CALID ")
                        .Append("      , TODOID ")
                        .Append("      , UNIQUEID ")
                        .Append("      , RECURRENCEID ")
                        .Append("      , CHGSEQNO ")
                        .Append("      , ACTSTAFFSTRCD ")
                        .Append("      , ACTSTAFFCD ")
                        .Append("      , RECSTAFFSTRCD ")
                        .Append("      , RECSTAFFCD ")
                        .Append("      , CONTACTNO ")
                        .Append("      , SUMMARY ")
                        .Append("      , STARTTIME ")
                        .Append("      , ENDTIME ")
                        .Append("      , TIMEFLG ")
                        .Append("      , ALLDAYFLG ")
                        .Append("      , MEMO ")
                        .Append("      , ICROPCOLOR ")
                        .Append("      , RRULEFLG ")
                        .Append("      , RRULE_FREQ ")
                        .Append("      , RRULE_INTERVAL ")
                        .Append("      , RRULE_UNTIL ")
                        .Append("      , RRULE_TEXT ")
                        .Append("      , LOCATION ")
                        .Append("      , ATTENDEE ")
                        .Append("      , TRANSP ")
                        .Append("      , URL ")
                        .Append("      , DELFLG ")
                        .Append("      , DELDATE ")
                        .Append("      , CREATEDATE ")
                        .Append("      , UPDATEDATE ")
                        .Append("      , CREATEACCOUNT ")
                        .Append("      , UPDATEACCOUNT ")
                        .Append("      , CREATEID ")
                        '2012/03/01 SKFC 加藤 【SALES_2】受注後工程の対応 START
                        '.Append("      , UPDATEID) ")
                        .Append("      , UPDATEID ")
                        '2014/04/03 SKFC 渡邊 NEXTSTEP_CALDAV START
                        '.Append("      , PROCESSDIV) ")
                        .Append("      , PROCESSDIV")
                        '2012/03/01 SKFC 加藤 【SALES_2】受注後工程の対応 END
                        .Append("      , CONTACT_NAME")
                        .Append("      , ACT_ODR_NAME")
                        .Append("      , ODR_DIV")
                        .Append("      , AFTER_ODR_ACT_ID)")
                        '2014/04/03 SKFC 渡邊 NEXTSTEP_CALDAV END
                        .Append(" VALUES (")
                        .Append("        :EVENTID ")
                        .Append("      , :CALID ")
                        .Append("      , :TODOID ")
                        .Append("      , :UNIQUEID ")
                        .Append("      , :RECURRENCEID ")
                        .Append("      , :CHGSEQNO ")
                        .Append("      , :ACTSTAFFSTRCD ")
                        .Append("      , :ACTSTAFFCD ")
                        .Append("      , :RECSTAFFSTRCD ")
                        .Append("      , :RECSTAFFCD ")
                        .Append("      , :CONTACTNO ")
                        .Append("      , :SUMMARY ")
                        .Append("      , :STARTTIME ")
                        .Append("      , :ENDTIME ")
                        .Append("      , :TIMEFLG ")
                        .Append("      , :ALLDAYFLG ")
                        .Append("      , :MEMO ")
                        .Append("      , :ICROPCOLOR ")
                        .Append("      , '0' ")
                        .Append("      , :RRULE_FREQ ")
                        .Append("      , :RRULE_INTERVAL ")
                        .Append("      , :RRULE_UNTIL ")
                        .Append("      , :RRULE_TEXT ")
                        .Append("      , :LOCATION ")
                        .Append("      , :ATTENDEE ")
                        .Append("      , :TRANSP ")
                        .Append("      , :URL ")
                        .Append("      , :DELFLG ")
                        .Append("      , :DELDATE ")
                        .Append("      , SYSDATE ")
                        .Append("      , SYSDATE ")
                        .Append("      , :CREATEACCOUNT ")
                        .Append("      , :UPDATEACCOUNT ")
                        .Append("      , :CREATEID ")
                        '2012/03/01 SKFC 加藤 【SALES_2】受注後工程の対応 START
                        '.Append("      , :UPDATEID) ")
                        .Append("      , :UPDATEID ")
                        '2014/04/03 SKFC 渡邊 NEXTSTEP_CALDAV START
                        '.Append("      , :PROCESSDIV) ")
                        .Append("      , :PROCESSDIV ")
                        '2012/03/01 SKFC 加藤 【SALES_2】受注後工程の対応 END
                        .Append("      , :CONTACT_NAME ")
                        .Append("      , :ACT_ODR_NAME ")
                        .Append("      , :ODR_DIV ")
                        .Append("      , :AFTER_ODR_ACT_ID) ")
                        '2014/04/03 SKFC 渡邊 NEXTSTEP_CALDAV END

                    End With

                    query.CommandText = sql.ToString()

                    Using Convert As New IC3040403DataSetTableAdapters.IC3040403DataAdapter

                        'SQLパラメータ設定
                        query.AddParameterWithTypeValue(ConstCode.EventItemTableEventId, OracleDbType.Varchar2, dataRow.EVENTID)
                        query.AddParameterWithTypeValue(ConstCode.EventItemTableCalId, OracleDbType.Varchar2, dataRow.CALID)
                        query.AddParameterWithTypeValue(ConstCode.EventItemTableTodoId, OracleDbType.Varchar2, dataRow.TODOID)
                        query.AddParameterWithTypeValue(ConstCode.EventItemTableUniqueId, OracleDbType.Varchar2, dataRow.UNIQUEID)
                        query.AddParameterWithTypeValue(ConstCode.EventItemTableRecurrenceId, OracleDbType.Varchar2, dataRow.RECURRENCEID)
                        query.AddParameterWithTypeValue(ConstCode.EventItemTableChgSeqNo, OracleDbType.Int32, Convert.ConvertStringEmpty(dataRow.CHGSEQNO))
                        query.AddParameterWithTypeValue(ConstCode.EventItemTableActStaffStrCD, OracleDbType.Varchar2, dataRow.ACTSTAFFSTRCD)
                        query.AddParameterWithTypeValue(ConstCode.EventItemTableActStaffCD, OracleDbType.Char, dataRow.ACTSTAFFCD)
                        query.AddParameterWithTypeValue(ConstCode.EventItemTableRecStaffStrCD, OracleDbType.Varchar2, dataRow.RECSTAFFSTRCD)
                        query.AddParameterWithTypeValue(ConstCode.EventItemTableRecStaffCD, OracleDbType.Char, dataRow.RECSTAFFCD)
                        '2014/07/15 SKFC渡邊 NEXTSTEP_CALDAV 不具合修正 Nvarchar2対応　START
                        'query.AddParameterWithTypeValue(ConstCode.EventItemTableContactNo, OracleDbType.Int32, convert.ConvertStringEmpty(dataRow.CONTACTNO))
                        query.AddParameterWithTypeValue(ConstCode.EventItemTableContactNo, OracleDbType.NVarchar2, dataRow.CONTACTNO)
                        '2014/07/15 SKFC渡邊 NEXTSTEP_CALDAV 不具合修正　END
                        query.AddParameterWithTypeValue(ConstCode.EventItemTableSummary, OracleDbType.NVarchar2, dataRow.SUMMARY)
                        query.AddParameterWithTypeValue(ConstCode.EventItemTableStartTime, OracleDbType.Date, Convert.ConvertStringDateTime(dataRow.STARTTIME))
                        query.AddParameterWithTypeValue(ConstCode.EventItemTableEndTime, OracleDbType.Date, Convert.ConvertStringDateTime(dataRow.ENDTIME))
                        query.AddParameterWithTypeValue(ConstCode.EventItemTableTimeFlg, OracleDbType.Char, dataRow.TIMEFLG)
                        query.AddParameterWithTypeValue(ConstCode.EventItemTableAllDayFlg, OracleDbType.Char, dataRow.ALLDAYFLG)
                        query.AddParameterWithTypeValue(ConstCode.EventItemTableMemo, OracleDbType.NVarchar2, dataRow.MEMO)
                        query.AddParameterWithTypeValue(ConstCode.EventItemTableIcropColor, OracleDbType.Varchar2, dataRow.ICROPCOLOR)
                        query.AddParameterWithTypeValue(ConstCode.EventItemTableRruleFreq, OracleDbType.Varchar2, dataRow.RRULE_FREQ)
                        query.AddParameterWithTypeValue(ConstCode.EventItemTableRruleInterVal, OracleDbType.Int32, Convert.ConvertStringEmpty(dataRow.RRULE_INTERVAL))
                        query.AddParameterWithTypeValue(ConstCode.EventItemTableRruleUntil, OracleDbType.Date, Convert.ConvertStringDateTime(dataRow.RRULE_UNTIL))
                        query.AddParameterWithTypeValue(ConstCode.EventItemTableRruleText, OracleDbType.Varchar2, dataRow.RRULE_TEXT)
                        query.AddParameterWithTypeValue(ConstCode.EventItemTableLocation, OracleDbType.NVarchar2, dataRow.LOCATION)
                        query.AddParameterWithTypeValue(ConstCode.EventItemTableAttenDee, OracleDbType.NVarchar2, dataRow.ATTENDEE)
                        query.AddParameterWithTypeValue(ConstCode.EventItemTableTransp, OracleDbType.Varchar2, dataRow.TRANSP)
                        query.AddParameterWithTypeValue(ConstCode.EventItemTableUrl, OracleDbType.NVarchar2, dataRow.URL)
                        query.AddParameterWithTypeValue(ConstCode.EventItemTableDelFlg, OracleDbType.Char, dataRow.DELFLG)
                        query.AddParameterWithTypeValue(ConstCode.EventItemTableDelDate, OracleDbType.Date, Convert.ConvertStringDateTime(dataRow.DELDATE))
                        query.AddParameterWithTypeValue(ConstCode.EventItemTableCreateAccount, OracleDbType.Varchar2, dataRow.CREATEACCOUNT)
                        query.AddParameterWithTypeValue(ConstCode.EventItemTableUpdateAccount, OracleDbType.Varchar2, dataRow.UPDATEACCOUNT)
                        query.AddParameterWithTypeValue(ConstCode.EventItemTableCreateId, OracleDbType.Varchar2, dataRow.CREATEID)
                        query.AddParameterWithTypeValue(ConstCode.EventItemTableUpdateId, OracleDbType.Varchar2, dataRow.UPDATEID)
                        '2012/03/01 SKFC 加藤 【SALES_2】受注後工程の対応 START
                        query.AddParameterWithTypeValue(ConstCode.EventItemTableProcessDiv, OracleDbType.Varchar2, dataRow.PROCESSDIV)
                        '2012/03/01 SKFC 加藤 【SALES_2】受注後工程の対応 END
                        '2014/04/03 SKFC 渡邊 NEXTSTEP_CALDAV START
                        query.AddParameterWithTypeValue(ConstCode.EventItemTableContactName, OracleDbType.NVarchar2, dataRow.CONTACT_NAME)
                        query.AddParameterWithTypeValue(ConstCode.EventItemTableActOdrName, OracleDbType.NVarchar2, dataRow.ACT_ODR_NAME)
                        query.AddParameterWithTypeValue(ConstCode.EventItemTableOdrDiv, OracleDbType.Char, dataRow.ODR_DIV)
                        query.AddParameterWithTypeValue(ConstCode.EventItemTableAfterOdrActID, OracleDbType.Varchar2, dataRow.AFTER_ODR_ACT_ID)
                        '2014/04/03 SKFC 渡邊 NEXTSTEP_CALDAV END
                    End Using

                    'SQL実行（登録行を返却）
                    Dim Count As Integer = query.Execute()
                    Return Count

                End Using

            Catch ex As SystemException
                '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 START
                Logger.Error(ex.Message, ex)
                '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 END
                Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.InsertCalEventItemSqlError)

            End Try



        End Function

        ''' <summary>
        ''' カレンダーイベント情報テーブルを更新します。
        ''' </summary>
        ''' <param name="dataRow">引数</param>
        ''' <returns>更新件数</returns>
        ''' <remarks></remarks>
        Public Function UpdateCalEventItem(ByVal dataRow As IC3040403DataSet.CalEventItemDataTableRow) As Integer

            Using query As New DBUpdateQuery("IC3040403_013")

                Try

                    Dim sql As New StringBuilder

                    Using Convert As New IC3040403DataSetTableAdapters.IC3040403DataAdapter

                        With sql
                            .Append(" UPDATE /* IC3040403_013 */ TBL_CAL_EVENTITEM")
                            .Append("    SET UPDATEDATE = SYSDATE")
                            .Append("      , UPDATEACCOUNT = :UPDATEACCOUNT ")
                            query.AddParameterWithTypeValue(ConstCode.EventItemTableUpdateAccount, OracleDbType.Varchar2, dataRow.UPDATEACCOUNT)
                            .Append("      , UPDATEID = :UPDATEID")
                            query.AddParameterWithTypeValue(ConstCode.EventItemTableUpdateId, OracleDbType.Varchar2, dataRow.UPDATEID)
                            If dataRow.ACTSTAFFSTRCD IsNot Nothing Then
                                .Append("      , ACTSTAFFSTRCD = :ACTSTAFFSTRCD ")
                                query.AddParameterWithTypeValue(ConstCode.EventItemTableActStaffStrCD, OracleDbType.Varchar2, dataRow.ACTSTAFFSTRCD)
                            End If
                            If dataRow.ACTSTAFFCD IsNot Nothing Then
                                .Append("      , ACTSTAFFCD = :ACTSTAFFCD ")
                                query.AddParameterWithTypeValue(ConstCode.EventItemTableActStaffCD, OracleDbType.Char, dataRow.ACTSTAFFCD)
                            End If
                            If dataRow.RECSTAFFSTRCD IsNot Nothing Then
                                .Append("      , RECSTAFFSTRCD = :RECSTAFFSTRCD ")
                                query.AddParameterWithTypeValue(ConstCode.EventItemTableRecStaffStrCD, OracleDbType.Varchar2, dataRow.RECSTAFFSTRCD)
                            End If
                            If dataRow.RECSTAFFCD IsNot Nothing Then
                                .Append("      , RECSTAFFCD = :RECSTAFFCD ")
                                query.AddParameterWithTypeValue(ConstCode.EventItemTableRecStaffCD, OracleDbType.Char, dataRow.RECSTAFFCD)
                            End If
                            If dataRow.CONTACTNO IsNot Nothing Then
                                .Append("      , CONTACTNO = :CONTACTNO ")
                                '2014/07/15 SKFC渡邊 NEXTSTEP_CALDAV 不具合修正 Nvarchar2対応　START
                                'query.AddParameterWithTypeValue(ConstCode.EventItemTableContactNo, OracleDbType.Int32, Convert.ConvertStringEmpty(dataRow.CONTACTNO))
                                query.AddParameterWithTypeValue(ConstCode.EventItemTableContactNo, OracleDbType.NVarchar2, dataRow.CONTACTNO)
                                '2014/07/15 SKFC渡邊 NEXTSTEP_CALDAV 不具合修正 Nvarchar2対応　END
                            End If
                            If dataRow.SUMMARY IsNot Nothing Then
                                .Append("      , SUMMARY = :SUMMARY ")
                                query.AddParameterWithTypeValue(ConstCode.EventItemTableSummary, OracleDbType.NVarchar2, dataRow.SUMMARY)
                            End If
                            If dataRow.STARTTIME IsNot Nothing Then
                                .Append("      , STARTTIME = :STARTTIME ")
                                query.AddParameterWithTypeValue(ConstCode.EventItemTableStartTime, OracleDbType.Date, Convert.ConvertStringDateTime(dataRow.STARTTIME))
                            End If
                            If dataRow.ENDTIME IsNot Nothing Then
                                .Append("      , ENDTIME = :ENDTIME ")
                                query.AddParameterWithTypeValue(ConstCode.EventItemTableEndTime, OracleDbType.Date, Convert.ConvertStringDateTime(dataRow.ENDTIME))
                            End If
                            If dataRow.TIMEFLG IsNot Nothing Then
                                .Append("      , TIMEFLG = :TIMEFLG ")
                                query.AddParameterWithTypeValue(ConstCode.EventItemTableTimeFlg, OracleDbType.Char, dataRow.TIMEFLG)
                            End If
                            If dataRow.ALLDAYFLG IsNot Nothing Then
                                .Append("      , ALLDAYFLG = :ALLDAYFLG ")
                                query.AddParameterWithTypeValue(ConstCode.EventItemTableAllDayFlg, OracleDbType.Char, dataRow.ALLDAYFLG)
                            End If
                            If dataRow.MEMO IsNot Nothing Then
                                .Append("      , MEMO = :MEMO ")
                                query.AddParameterWithTypeValue(ConstCode.EventItemTableMemo, OracleDbType.NVarchar2, dataRow.MEMO)
                            End If
                            If dataRow.ICROPCOLOR IsNot Nothing Then
                                .Append("      , ICROPCOLOR = :ICROPCOLOR ")
                                query.AddParameterWithTypeValue(ConstCode.EventItemTableIcropColor, OracleDbType.Varchar2, dataRow.ICROPCOLOR)
                            End If
                            If dataRow.DELDATE IsNot Nothing Then
                                .Append("      , DELDATE = :DELDATE ")
                                query.AddParameterWithTypeValue(ConstCode.EventItemTableDelDate, OracleDbType.Date, Convert.ConvertStringDateTime(dataRow.DELDATE))
                            End If

                            '2014/04/03 SKFC 渡邊 NEXTSTEP_CALDAV START
                            If dataRow.CONTACT_NAME IsNot Nothing Then
                                .Append("      , CONTACT_NAME = :CONTACT_NAME ")
                                query.AddParameterWithTypeValue(ConstCode.EventItemTableContactName, OracleDbType.NVarchar2, dataRow.CONTACT_NAME)
                            End If

                            If dataRow.ACT_ODR_NAME IsNot Nothing Then
                                .Append("      , ACT_ODR_NAME = :ACT_ODR_NAME ")
                                query.AddParameterWithTypeValue(ConstCode.EventItemTableActOdrName, OracleDbType.NVarchar2, dataRow.ACT_ODR_NAME)
                            End If

                            If dataRow.ODR_DIV IsNot Nothing Then
                                .Append("      , ODR_DIV = :ODR_DIV ")
                                query.AddParameterWithTypeValue(ConstCode.EventItemTableOdrDiv, OracleDbType.Char, dataRow.ODR_DIV)
                            End If

                            If dataRow.AFTER_ODR_ACT_ID IsNot Nothing Then
                                .Append("      , AFTER_ODR_ACT_ID = :AFTER_ODR_ACT_ID ")
                                query.AddParameterWithTypeValue(ConstCode.EventItemTableAfterOdrActID, OracleDbType.Varchar2, dataRow.AFTER_ODR_ACT_ID)
                            End If
                            '2014/04/03 SKFC 渡邊 NEXTSTEP_CALDAV END

                            '' 2014/07/18 SKFC 渡邊　NEXTSTEP CalDAV　Event追加　START
                            If dataRow.AFTER_ODR_ACT_ID IsNot Nothing And dataRow.TODOID IsNot Nothing Then
                                .Append("  WHERE AFTER_ODR_ACT_ID = :AFTER_ODR_ACT_ID ")
                                query.AddParameterWithTypeValue(ConstCode.TodoItemTableAfterOdrActID, OracleDbType.Varchar2, dataRow.AFTER_ODR_ACT_ID)
                            ElseIf dataRow.AFTER_ODR_ACT_ID IsNot Nothing Then
                                .Append("  WHERE AFTER_ODR_ACT_ID = :AFTER_ODR_ACT_ID ")
                                query.AddParameterWithTypeValue(ConstCode.TodoItemTableAfterOdrActID, OracleDbType.Varchar2, dataRow.AFTER_ODR_ACT_ID)
                            Else
                                .Append(" WHERE  TODOID = :TODOID ")
                                query.AddParameterWithTypeValue(ConstCode.TodoItemTableTodoId, OracleDbType.Varchar2, dataRow.TODOID)
                            End If

                            '.Append(" WHERE  TODOID = :TODOID ")
                            'query.AddParameterWithTypeValue(ConstCode.EventItemTableTodoId, OracleDbType.Varchar2, dataRow.TODOID)
                            ' 2014/07/18 SKFC 渡邊　NEXTSTEP CalDAV　Event追加　END

                            '2012/03/01 SKFC 加藤 【SALES_2】受注後工程の対応 START
                            If dataRow.PROCESSDIV IsNot Nothing Then
                                .Append("      AND PROCESSDIV = :PROCESSDIV ")
                                query.AddParameterWithTypeValue(ConstCode.TodoItemTableProcessDiv, OracleDbType.Varchar2, dataRow.PROCESSDIV)
                            End If
                            '2012/03/01 SKFC 加藤 【SALES_2】受注後工程の対応 END


                        End With

                    End Using

                    query.CommandText = sql.ToString()

                    'SQL実行（更新行を返却）
                    Dim Count As Integer = query.Execute()
                    Return Count

                Catch ex As SystemException
                    '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 START
                    Logger.Error(ex.Message, ex)
                    '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 END
                    Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.UpdateCalEventItemSqlError)

                End Try

            End Using

        End Function

'2019/02/28 SKFC二村 TR-V4-TMT-20190131-001 START
        ''' <summary>
        ''' カレンダーイベント情報テーブル更新ロックを取得
        ''' </summary>
        ''' <param name="dataRow">引数</param>
        ''' <returns>更新件数</returns>
        ''' <remarks></remarks>
        Public Function GetCalEventItemLock(ByVal dataRow As IC3040403DataSet.CalEventItemDataTableRow) As Integer

            Dim table As IC3040403DataSet.IdTableDataTable
            Dim result As Integer = -1

            Using query As New DBSelectQuery(Of IC3040403DataSet.IdTableDataTable)("IC3040403_033")

                Try

                    Dim sql As New StringBuilder

                    With sql
                        .AppendLine(" SELECT /* IC3040403_033 */ ")
                        .AppendLine("        EVENTID ")
                        .AppendLine("   FROM TBL_CAL_EVENTITEM ")
                        If dataRow.AFTER_ODR_ACT_ID IsNot Nothing And dataRow.TODOID IsNot Nothing Then
                            .AppendLine("  WHERE AFTER_ODR_ACT_ID = :AFTER_ODR_ACT_ID ")
                            query.AddParameterWithTypeValue(ConstCode.TodoItemTableAfterOdrActID, OracleDbType.Varchar2, dataRow.AFTER_ODR_ACT_ID)
                        ElseIf dataRow.AFTER_ODR_ACT_ID IsNot Nothing Then
                            .AppendLine("  WHERE AFTER_ODR_ACT_ID = :AFTER_ODR_ACT_ID ")
                            query.AddParameterWithTypeValue(ConstCode.TodoItemTableAfterOdrActID, OracleDbType.Varchar2, dataRow.AFTER_ODR_ACT_ID)
                        Else
                            .AppendLine("  WHERE TODOID = :TODOID ")
                            query.AddParameterWithTypeValue(ConstCode.TodoItemTableTodoId, OracleDbType.Varchar2, dataRow.TODOID)
                        End If
                        If dataRow.PROCESSDIV IsNot Nothing Then
                            .AppendLine("    AND PROCESSDIV = :PROCESSDIV ")
                            query.AddParameterWithTypeValue(ConstCode.TodoItemTableProcessDiv, OracleDbType.Varchar2, dataRow.PROCESSDIV)
                        End If
                        .AppendLine("  ORDER BY EVENTID ASC ")
                        Dim env As New SystemEnvSetting
                        .AppendLine("    FOR UPDATE WAIT " + env.GetLockWaitTime())
                    End With

                    query.CommandText = sql.ToString()
                    'SQLパラメータ設定

                    'SQL実行（結果表を返却）
                    table = query.GetData()

                    If table.Count >= 0 Then
                        result = table.Count
                    End If

                Catch ex As Exception
                    Logger.Error(ex.Message, ex)
                    Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.UpdateCalEventItemSqlError)

                End Try

            End Using

        End Function
'2019/02/28 SKFC二村 TR-V4-TMT-20190131-001 END

        ''' <summary>
        ''' カレンダーイベント情報テーブルの削除フラグを更新します。
        ''' </summary>
        ''' <param name="dataRow">引数</param>
        ''' <param name="CompletionFlg">完了フラグ</param>
        ''' <returns>更新件数</returns>
        ''' <remarks></remarks>
        Public Function UpdateDeleteFlgCalEventItem(ByVal dataRow As IC3040403DataSet.CalEventItemDataTableRow, ByVal CompletionFlg As String) As Integer
            '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 START
            'Public Function UpdateDeleteFlgCalEventItem(ByVal dataRow As IC3040403DataSet.CalEventItemDataTableRow) As Integer
            '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 END

            Using query As New DBUpdateQuery("IC3040403_014")

                Try

                    Dim sql As New StringBuilder

                    Using Convert As New IC3040403DataSetTableAdapters.IC3040403DataAdapter

                        With sql
                            .Append(" UPDATE /* IC3040403_014 */ TBL_CAL_EVENTITEM EVI")
                            .Append("    SET UPDATEDATE = SYSDATE")
                            .Append("      , UPDATEACCOUNT = :UPDATEACCOUNT ")
                            query.AddParameterWithTypeValue(ConstCode.EventItemTableUpdateAccount, OracleDbType.Varchar2, dataRow.UPDATEACCOUNT)
                            .Append("      , UPDATEID = :UPDATEID")
                            query.AddParameterWithTypeValue(ConstCode.EventItemTableUpdateId, OracleDbType.Varchar2, dataRow.UPDATEID)
                            .Append("      , DELFLG = '1'")
                            If dataRow.DELDATE Is Nothing Then
                                .Append("      , DELDATE = SYSDATE")
                            Else
                                .Append("      , DELDATE = :DELDATE")
                                query.AddParameterWithTypeValue(ConstCode.EventItemTableDelDate, OracleDbType.Date, Convert.ConvertStringDateTime(dataRow.DELDATE))
                            End If
                            .Append("  WHERE DELFLG = '0' ")
                            If dataRow.CALID IsNot Nothing Then
                                .Append("  AND CALID = :CALID ")
                                query.AddParameterWithTypeValue(ConstCode.EventItemTableCalId, OracleDbType.Varchar2, dataRow.CALID)
                            End If
                            If dataRow.EVENTID IsNot Nothing Then
                                .Append("  AND EVENTID = :EVENTID ")
                                '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 START
                                '不具合修正
                                'query.AddParameterWithTypeValue(ConstCode.EventItemTableTodoId, OracleDbType.Varchar2, dataRow.TODOID)
                                query.AddParameterWithTypeValue(ConstCode.EventItemTableEventId, OracleDbType.Varchar2, dataRow.EVENTID)
                                '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 END
                            End If
                            '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 START
                            '.Append("    AND EXISTS(")
                            '.Append("     SELECT 1 ")
                            '.Append("       FROM TBL_CAL_TODOITEM TOI ")
                            '.Append("      WHERE TOI.TODOID = EVI.TODOID ")
                            '.Append("        AND COMPLETIONFLG = '0')")                            '条件追加
                            If CompletionFlg IsNot Nothing Then
                                .Append("    AND EXISTS(")
                                .Append("     SELECT 1 ")
                                .Append("       FROM TBL_CAL_TODOITEM TOI ")
                                .Append("      WHERE TOI.TODOID = EVI.TODOID ")
                                .Append("        AND COMPLETIONFLG = :COMPLETIONFLG)")
                                query.AddParameterWithTypeValue(ConstCode.TodoItemTableCompletionFlg, OracleDbType.Char, CompletionFlg)
                            End If
                            '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 END
                        End With

                    End Using

                    query.CommandText = sql.ToString()

                    'SQL実行（更新行を返却）
                    Dim Count As Integer = query.Execute()
                    Return Count

                Catch ex As SystemException
                    '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 START
                    Logger.Error(ex.Message, ex)
                    '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 END
                    Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.UpdateDeleteFlgCalEventItemSqlError)

                End Try

            End Using

        End Function

'2019/02/28 SKFC二村 TR-V4-TMT-20190131-001 START
        ''' <summary>
        ''' カレンダーイベント情報テーブルの削除フラグ更新ロックを取得
        ''' </summary>
        ''' <param name="dataRow">引数</param>
        ''' <param name="CompletionFlg">完了フラグ</param>
        ''' <returns>対象件数</returns>
        ''' <remarks></remarks>
        Public Function GetDeleteFlgCalEventItemLock(ByVal dataRow As IC3040403DataSet.CalEventItemDataTableRow, ByVal CompletionFlg As String) As Integer

            Dim table As IC3040403DataSet.IdTableDataTable
            Dim result As Integer = -1

            Using query As New DBSelectQuery(Of IC3040403DataSet.IdTableDataTable)("IC3040403_034")

                Try

                    Dim sql As New StringBuilder

                    With sql
                        .AppendLine(" SELECT /* IC3040403_034 */ ")
                        .AppendLine("        EVI.EVENTID ")
                        .AppendLine("   FROM TBL_CAL_EVENTITEM EVI")
                        .AppendLine("  WHERE EVI.DELFLG = '0' ")
                        If dataRow.CALID IsNot Nothing Then
                            .AppendLine("  AND EVI.CALID = :CALID ")
                            query.AddParameterWithTypeValue(ConstCode.EventItemTableCalId, OracleDbType.Varchar2, dataRow.CALID)
                        End If
                        If dataRow.EVENTID IsNot Nothing Then
                            .AppendLine("  AND EVI.EVENTID = :EVENTID ")
                            query.AddParameterWithTypeValue(ConstCode.EventItemTableEventId, OracleDbType.Varchar2, dataRow.EVENTID)
                        End If
                        If CompletionFlg IsNot Nothing Then
                            .AppendLine("    AND EXISTS(")
                            .AppendLine("     SELECT 1 ")
                            .AppendLine("       FROM TBL_CAL_TODOITEM TOI ")
                            .AppendLine("      WHERE TOI.TODOID = EVI.TODOID ")
                            .AppendLine("        AND TOI.COMPLETIONFLG = :COMPLETIONFLG) ")
                            query.AddParameterWithTypeValue(ConstCode.TodoItemTableCompletionFlg, OracleDbType.Char, CompletionFlg)
                            .AppendLine("  ORDER BY EVI.EVENTID ASC ")
                            Dim env As New SystemEnvSetting
                            .AppendLine("    FOR UPDATE WAIT " + env.GetLockWaitTime())
                        End If
                    End With

                    query.CommandText = sql.ToString()
                    'SQLパラメータ設定

                    'SQL実行（結果表を返却）
                    table = query.GetData()

                    If table.Count >= 0 Then
                        result = table.Count
                    End If

                Catch ex As Exception
                    Logger.Error(ex.Message, ex)
                    Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.UpdateDeleteFlgCalEventItemSqlError)

                End Try

            End Using

        End Function
'2019/02/28 SKFC二村 TR-V4-TMT-20190131-001 END

        ''' <summary>
        ''' カレンダーイベント情報テーブルの削除フラグを更新します。(処理区分：4対応)
        ''' </summary>
        ''' <param name="dataRow">引数</param>
        ''' <returns>更新件数</returns>
        ''' <remarks></remarks>
        Public Function UpdateAfterOrderDeleteFlgCalEventItem(ByVal dataRow As IC3040403DataSet.CalEventItemDataTableRow) As Integer

            Using query As New DBUpdateQuery("IC3040403_027")

                Try

                    Dim sql As New StringBuilder

                    Using Convert As New IC3040403DataSetTableAdapters.IC3040403DataAdapter

                        With sql
                            .Append(" UPDATE /* IC3040403_027 */ TBL_CAL_EVENTITEM EVI")
                            .Append("    SET UPDATEDATE = SYSDATE")
                            .Append("      , UPDATEACCOUNT = :UPDATEACCOUNT ")
                            query.AddParameterWithTypeValue(ConstCode.EventItemTableUpdateAccount, OracleDbType.Varchar2, dataRow.UPDATEACCOUNT)
                            .Append("      , UPDATEID = :UPDATEID")
                            query.AddParameterWithTypeValue(ConstCode.EventItemTableUpdateId, OracleDbType.Varchar2, dataRow.UPDATEID)
                            .Append("      , DELFLG = '1'")
                            If dataRow.DELDATE Is Nothing Then
                                .Append("      , DELDATE = SYSDATE")
                            Else
                                .Append("      , DELDATE = :DELDATE")
                                query.AddParameterWithTypeValue(ConstCode.EventItemTableDelDate, OracleDbType.Date, Convert.ConvertStringDateTime(dataRow.DELDATE))
                            End If
                            .Append("  WHERE DELFLG = '0' ")
                            If dataRow.CALID IsNot Nothing Then
                                .Append("  AND CALID = :CALID ")
                                query.AddParameterWithTypeValue(ConstCode.EventItemTableCalId, OracleDbType.Varchar2, dataRow.CALID)
                            End If
                            .Append("    AND EXISTS(")
                            .Append("     SELECT 1 ")
                            .Append("       FROM TBL_CAL_TODOITEM TOI ")
                            .Append("      WHERE TOI.TODOID = EVI.TODOID ")
                            .Append("        AND COMPLETIONFLG = '0'")
                            .Append("        AND DELFLG = '0')")
                        End With

                    End Using

                    query.CommandText = sql.ToString()

                    'SQL実行（更新行を返却）
                    Dim Count As Integer = query.Execute()
                    Return Count

                Catch ex As SystemException
                    Logger.Error(ex.Message, ex)
                    Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.UpdateAfterOrderDeleteFlgCalEventItemSqlError)

                End Try

            End Using

        End Function

'2019/02/28 SKFC二村 TR-V4-TMT-20190131-001 START
        ''' <summary>
        ''' カレンダーイベント情報テーブルの削除フラグ更新ロックを取得
        ''' </summary>
        ''' <param name="dataRow">引数</param>
        ''' <returns>対象件数</returns>
        ''' <remarks></remarks>
        Public Function GetAfterOrderDeleteFlgCalEventItemLock(ByVal dataRow As IC3040403DataSet.CalEventItemDataTableRow) As Integer

            Dim table As IC3040403DataSet.IdTableDataTable
            Dim result As Integer = -1

            Using query As New DBSelectQuery(Of IC3040403DataSet.IdTableDataTable)("IC3040403_035")

                Try

                    Dim sql As New StringBuilder

                    With sql
                        .AppendLine(" SELECT /* IC3040403_035 */ ")
                        .AppendLine("        EVI.EVENTID ")
                        .AppendLine("   FROM TBL_CAL_EVENTITEM EVI ")
                        .AppendLine("  WHERE EVI.DELFLG = '0' ")
                        If dataRow.CALID IsNot Nothing Then
                            .AppendLine("  AND EVI.CALID = :CALID ")
                            query.AddParameterWithTypeValue(ConstCode.EventItemTableCalId, OracleDbType.Varchar2, dataRow.CALID)
                        End If
                        .AppendLine("    AND EXISTS( ")
                        .AppendLine("     SELECT 1 ")
                        .AppendLine("       FROM TBL_CAL_TODOITEM TOI ")
                        .AppendLine("      WHERE TOI.TODOID = EVI.TODOID ")
                        .AppendLine("        AND TOI.COMPLETIONFLG = '0' ")
                        .AppendLine("        AND TOI.DELFLG = '0') ")
                        .AppendLine("  ORDER BY EVI.EVENTID ASC ")
                        Dim env As New SystemEnvSetting
                        .AppendLine("    FOR UPDATE WAIT " + env.GetLockWaitTime())
                    End With

                    query.CommandText = sql.ToString()
                    'SQLパラメータ設定

                    'SQL実行（結果表を返却）
                    table = query.GetData()

                    If table.Count >= 0 Then
                        result = table.Count
                    End If

                Catch ex As Exception
                    Logger.Error(ex.Message, ex)
                    Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.UpdateAfterOrderDeleteFlgCalEventItemSqlError)

                End Try

            End Using

        End Function
'2019/02/28 SKFC二村 TR-V4-TMT-20190131-001 END

        ''' <summary>
        ''' Todoに紐づけた状態のイベントの登録を行う。
        ''' </summary>
        ''' <param name="dataRow">引数</param>
        ''' <returns>登録件数</returns>
        ''' <remarks></remarks>
        Public Function InsertLinkEvent(ByVal dataRow As IC3040403DataSet.CalEventItemDataTableRow) As Integer



            Try

                Using query As New DBUpdateQuery("IC3040403_015")

                    Dim sql As New StringBuilder
                    With sql
                        .Append(" INSERT /* IC3040403_015 */ ")
                        .Append("   INTO TBL_CAL_EVENTITEM( ")
                        .Append("        EVENTID ")
                        .Append("      , CALID ")
                        .Append("      , TODOID ")
                        .Append("      , UNIQUEID ")
                        .Append("      , RECURRENCEID ")
                        .Append("      , CHGSEQNO ")
                        .Append("      , ACTSTAFFSTRCD ")
                        .Append("      , ACTSTAFFCD ")
                        .Append("      , RECSTAFFSTRCD ")
                        .Append("      , RECSTAFFCD ")
                        .Append("      , CONTACTNO ")
                        .Append("      , SUMMARY ")
                        .Append("      , STARTTIME ")
                        .Append("      , ENDTIME ")
                        .Append("      , TIMEFLG ")
                        .Append("      , ALLDAYFLG ")
                        .Append("      , MEMO ")
                        .Append("      , ICROPCOLOR ")
                        .Append("      , RRULEFLG ")
                        .Append("      , RRULE_FREQ ")
                        .Append("      , RRULE_INTERVAL ")
                        .Append("      , RRULE_UNTIL ")
                        .Append("      , RRULE_TEXT ")
                        .Append("      , LOCATION ")
                        .Append("      , ATTENDEE ")
                        .Append("      , TRANSP ")
                        .Append("      , URL ")
                        .Append("      , DELFLG ")
                        .Append("      , DELDATE ")
                        .Append("      , CREATEDATE ")
                        .Append("      , UPDATEDATE ")
                        .Append("      , CREATEACCOUNT ")
                        .Append("      , UPDATEACCOUNT ")
                        .Append("      , CREATEID ")
                        '2012/03/27 SKFC 上田 【SALES_2】受注後工程の対応 START
                        '.Append("      , UPDATEID) ")
                        .Append("      , UPDATEID ")
                        '2014/04/03 SKFC 渡邊 NEXTSTEP_CALDAV START
                        '.Append("      , PROCESSDIV) ")
                        .Append("      , PROCESSDIV ")
                        '2012/03/27 SKFC 上田 【SALES_2】受注後工程の対応 START
                        .Append("      , CONTACT_NAME ")
                        .Append("      , ACT_ODR_NAME ")
                        .Append("      , ODR_DIV ")
                        .Append("      , AFTER_ODR_ACT_ID) ")
                        '2014/04/03 SKFC 渡邊 NEXTSTEP_CALDAV END
                        .Append(" SELECT :EVENTID ")
                        .Append("      , CALID ")
                        .Append("      , TODOID ")
                        .Append("      , :UNIQUEID ")
                        .Append("      , :RECURRENCEID ")
                        .Append("      , :CHGSEQNO ")
                        .Append("      , ACTSTAFFSTRCD ")
                        .Append("      , ACTSTAFFCD ")
                        .Append("      , RECSTAFFSTRCD ")
                        .Append("      , RECSTAFFCD ")
                        .Append("      , CONTACTNO ")
                        .Append("      , SUMMARY ")
                        .Append("      , :STARTTIME ")
                        .Append("      , :ENDTIME ")
                        .Append("      , :TIMEFLG ")
                        .Append("      , :ALLDAYFLG ")
                        .Append("      , MEMO ")
                        .Append("      , ICROPCOLOR ")
                        .Append("      , RRULEFLG ")
                        .Append("      , RRULE_FREQ ")
                        .Append("      , RRULE_INTERVAL ")
                        .Append("      , RRULE_UNTIL ")
                        .Append("      , RRULE_TEXT ")
                        .Append("      , :LOCATION ")
                        .Append("      , :ATTENDEE ")
                        .Append("      , :TRANSP ")
                        .Append("      , :URL ")
                        .Append("      , :DELFLG ")
                        .Append("      , :DELDATE ")
                        .Append("      , SYSDATE ")
                        .Append("      , SYSDATE ")
                        .Append("      , :CREATEACCOUNT ")
                        .Append("      , :UPDATEACCOUNT ")
                        .Append("      , :CREATEID ")
                        .Append("      , :UPDATEID ")
                        '2012/03/27 SKFC 上田 【SALES_2】受注後工程の対応 START
                        .Append("      , PROCESSDIV ")
                        '2012/03/27 SKFC 上田 【SALES_2】受注後工程の対応 START
                        '2014/04/03 SKFC 渡邊 NEXTSTEP_CALDAV START
                        .Append("      , CONTACT_NAME ")
                        .Append("      , ACT_ODR_NAME ")
                        .Append("      , ODR_DIV ")
                        .Append("      , AFTER_ODR_ACT_ID ")
                        '2014/04/03 SKFC 渡邊 NEXTSTEP_CALDAV END
                        .Append("   FROM TBL_CAL_TODOITEM ")
                        .Append("  WHERE TODOID = :TODOID ")

                    End With

                    query.CommandText = sql.ToString()

                    Using Convert As New IC3040403DataSetTableAdapters.IC3040403DataAdapter

                        'SQLパラメータ設定
                        query.AddParameterWithTypeValue(ConstCode.EventItemTableUniqueId, OracleDbType.Varchar2, dataRow.UNIQUEID)
                        query.AddParameterWithTypeValue(ConstCode.EventItemTableRecurrenceId, OracleDbType.Varchar2, dataRow.RECURRENCEID)
                        query.AddParameterWithTypeValue(ConstCode.EventItemTableChgSeqNo, OracleDbType.Int32, Convert.ConvertStringEmpty(dataRow.CHGSEQNO))
                        query.AddParameterWithTypeValue(ConstCode.EventItemTableEventId, OracleDbType.Varchar2, dataRow.EVENTID)
                        query.AddParameterWithTypeValue(ConstCode.EventItemTableTodoId, OracleDbType.Varchar2, dataRow.TODOID)
                        query.AddParameterWithTypeValue(ConstCode.EventItemTableStartTime, OracleDbType.Date, Convert.ConvertStringDateTime(dataRow.STARTTIME))
                        query.AddParameterWithTypeValue(ConstCode.EventItemTableEndTime, OracleDbType.Date, Convert.ConvertStringDateTime(dataRow.ENDTIME))
                        query.AddParameterWithTypeValue(ConstCode.EventItemTableLocation, OracleDbType.NVarchar2, dataRow.LOCATION)
                        query.AddParameterWithTypeValue(ConstCode.EventItemTableTimeFlg, OracleDbType.Char, dataRow.TIMEFLG)
                        query.AddParameterWithTypeValue(ConstCode.EventItemTableAllDayFlg, OracleDbType.Char, dataRow.ALLDAYFLG)
                        query.AddParameterWithTypeValue(ConstCode.EventItemTableAttenDee, OracleDbType.Char, dataRow.ATTENDEE)
                        query.AddParameterWithTypeValue(ConstCode.EventItemTableTransp, OracleDbType.Varchar2, dataRow.TRANSP)
                        query.AddParameterWithTypeValue(ConstCode.EventItemTableUrl, OracleDbType.NVarchar2, dataRow.URL)
                        query.AddParameterWithTypeValue(ConstCode.EventItemTableDelFlg, OracleDbType.Char, dataRow.DELFLG)
                        query.AddParameterWithTypeValue(ConstCode.EventItemTableDelDate, OracleDbType.Date, Convert.ConvertStringDateTime(dataRow.DELDATE))
                        query.AddParameterWithTypeValue(ConstCode.EventItemTableCreateAccount, OracleDbType.Varchar2, dataRow.CREATEACCOUNT)
                        query.AddParameterWithTypeValue(ConstCode.EventItemTableUpdateAccount, OracleDbType.Varchar2, dataRow.UPDATEACCOUNT)
                        query.AddParameterWithTypeValue(ConstCode.EventItemTableCreateId, OracleDbType.Varchar2, dataRow.CREATEID)
                        query.AddParameterWithTypeValue(ConstCode.EventItemTableUpdateId, OracleDbType.Varchar2, dataRow.UPDATEID)

                    End Using

                    'SQL実行（登録行を返却）
                    Dim Count As Integer = query.Execute()
                    Return Count

                End Using

            Catch ex As SystemException
                '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 START
                Logger.Error(ex.Message, ex)
                '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 END
                Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.InsertLinkEventSqlError)

            End Try

        End Function

    End Class

    Public Class CalEventAlarmDataTable
        Inherits Global.System.ComponentModel.Component

        ''' <summary>
        ''' カレンダーEventアラームテーブルに登録します。
        ''' </summary>
        ''' <param name="dataTable"></param>
        ''' <returns>登録件数</returns>
        ''' <remarks></remarks>
        Public Function InsertCalEventAlarms(ByVal dataTable As IC3040403DataSet.CalEventAlarmDataTableDataTable) As Integer

            Dim insertCount As Integer = 0

            For Each dataRow As IC3040403DataSet.CalEventAlarmDataTableRow In dataTable

                Using query As New DBUpdateQuery("IC3040403_016")

                    Try

                        Dim sql As New StringBuilder
                        With sql
                            .Append(" INSERT /* IC3040403_016 */ ")
                            .Append("   INTO TBL_CAL_EVENTALARM( ")
                            .Append("        EVENTID ")
                            .Append("      , SEQNO ")
                            .Append("      , STARTTRIGGER ")
                            .Append("      , CREATEDATE ")
                            .Append("      , UPDATEDATE ")
                            .Append("      , CREATEACCOUNT ")
                            .Append("      , UPDATEACCOUNT ")
                            .Append("      , CREATEID ")
                            .Append("      , UPDATEID ) ")
                            .Append(" VALUES (")
                            .Append("        :EVENTID ")
                            .Append("      , :SEQNO ")
                            .Append("      , :STARTTRIGGER ")
                            .Append("      , SYSDATE ")
                            .Append("      , SYSDATE ")
                            .Append("      , :CREATEACCOUNT ")
                            .Append("      , :UPDATEACCOUNT ")
                            .Append("      , :CREATEID ")
                            .Append("      , :UPDATEID ) ")
                        End With

                        query.CommandText = sql.ToString()
                        'SQLパラメータ設定

                        Using Convert As New IC3040403DataSetTableAdapters.IC3040403DataAdapter

                            query.AddParameterWithTypeValue(ConstCode.EventAlarmTableEventId, OracleDbType.Varchar2, dataRow.EVENTID)
                            query.AddParameterWithTypeValue(ConstCode.EventAlarmTableSeqNo, OracleDbType.Int32, Convert.ConvertStringEmpty(dataRow.SEQNO))
                            query.AddParameterWithTypeValue(ConstCode.EventAlarmTableStartTrigger, OracleDbType.Char, dataRow.STARTTRIGGER)
                            query.AddParameterWithTypeValue(ConstCode.EventAlarmTableCreateAccount, OracleDbType.Varchar2, dataRow.CREATEACCOUNT)
                            query.AddParameterWithTypeValue(ConstCode.EventAlarmTableUpdateAccount, OracleDbType.Varchar2, dataRow.UPDATEACCOUNT)
                            query.AddParameterWithTypeValue(ConstCode.EventAlarmTableCreateId, OracleDbType.Varchar2, dataRow.CREATEID)
                            query.AddParameterWithTypeValue(ConstCode.EventAlarmTableUpdateId, OracleDbType.Varchar2, dataRow.UPDATEID)

                        End Using

                        'SQL実行（登録行を返却）
                        insertCount = query.Execute() + insertCount

                    Catch ex As SystemException
                        '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 START
                        Logger.Error(ex.Message, ex)
                        '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 END
                        Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.InsertCalEventAlarmsSqlError)

                    End Try

                End Using

            Next

            Return insertCount



        End Function

        ''' <summary>
        ''' カレンダーイベントアラームテーブルの値を削除します。
        ''' </summary>
        ''' <param name="dataRow"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function DeleteCalEventAlarm(ByVal dataRow As IC3040403DataSet.CalEventAlarmDataTableRow) As Integer

            Using query As New DBUpdateQuery("IC3040403_017")

                Try

                    Dim sql As New StringBuilder
                    With sql
                        .Append(" DELETE /* IC3040403_017 */ ")
                        .Append("   FROM TBL_CAL_EVENTALARM ")
                        .Append("  WHERE EVENTID = :EVENTID ")
                    End With

                    query.CommandText = sql.ToString()
                    'SQLパラメータ設定

                    query.AddParameterWithTypeValue(ConstCode.EventAlarmTableEventId, OracleDbType.Char, dataRow.EVENTID)


                    'SQL実行（削除行を返却）
                    Return query.Execute()

                Catch ex As SystemException
                    '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 START
                    Logger.Error(ex.Message, ex)
                    '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 END
                    Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.DeleteCalEventAlarmSqlError)

                End Try

            End Using

        End Function

'2019/02/28 SKFC二村 TR-V4-TMT-20190131-001 START
        ''' <summary>
        ''' カレンダーイベントアラームテーブルの値削除ロックを取得。
        ''' </summary>
        ''' <param name="dataRow"></param>
        ''' <returns>対象件数</returns>
        ''' <remarks></remarks>
        Public Function GetCalEventAlarmLock(ByVal dataRow As IC3040403DataSet.CalEventAlarmDataTableRow) As Integer

            Dim table As IC3040403DataSet.IdTableDataTable
            Dim result As Integer = -1

            Using query As New DBSelectQuery(Of IC3040403DataSet.IdTableDataTable)("IC3040403_036")

                Try

                    Dim sql As New StringBuilder

                    With sql
                        .Append(" SELECT /* IC3040403_036 */ ")
                        .Append("        EVENTID ")
                        .Append("   FROM TBL_CAL_EVENTALARM ")
                        .Append("  WHERE EVENTID = :EVENTID ")
                        query.AddParameterWithTypeValue(ConstCode.EventAlarmTableEventId, OracleDbType.Char, dataRow.EVENTID)
                        .AppendLine("  ORDER BY EVENTID, SEQNO ASC ")
                        Dim env As New SystemEnvSetting
                        .AppendLine("    FOR UPDATE WAIT " + env.GetLockWaitTime())
                    End With

                    query.CommandText = sql.ToString()
                    'SQLパラメータ設定

                    'SQL実行（結果表を返却）
                    table = query.GetData()

                    If table.Count >= 0 Then
                        result = table.Count
                    End If

                Catch ex As Exception
                    Logger.Error(ex.Message, ex)
                    Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.DeleteCalEventAlarmSqlError)

                End Try

            End Using

        End Function
'2019/02/28 SKFC二村 TR-V4-TMT-20190131-001 END

    End Class

    Public Class SequenceTable
        Inherits Global.System.ComponentModel.Component

        ''' <summary>
        ''' カレンダーIDシーケンスから、新規カレンダーIDの取得
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property GetNewCalenderId() As String
            Get
                Using query As New DBSelectQuery(Of IC3040403DataSet.SequenceTableDataTable)("IC3040403_018")

                    Try

                        Dim sql As New StringBuilder
                        With sql
                            .Append("SELECT /* IC3040403_018 */ ")
                            .Append("       LPAD(SEQ_CAL_ICROPINFO_CALID.NEXTVAL, 20, '0') Sequence_Id ")
                            .Append("  FROM DUAL ")
                        End With

                        query.CommandText = sql.ToString()

                        'SQL実行（結果表を返却）
                        Dim dataTable As IC3040403DataSet.SequenceTableDataTable
                        dataTable = query.GetData()

                        Dim dataRow As IC3040403DataSet.SequenceTableRow = dataTable.Rows(0)
                        Return dataRow.Sequence_Id

                    Catch ex As SystemException
                        '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 START
                        Logger.Error(ex.Message, ex)
                        '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 END
                        Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.GetNewCalenderIdSqlError)

                    End Try

                End Using

            End Get
        End Property


        ''' <summary>
        ''' TodoIDシーケンスから、新規TodoIDの取得
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property GetNewTodoId() As String
            Get
                Using query As New DBSelectQuery(Of IC3040403DataSet.SequenceTableDataTable)("IC3040403_019")

                    Try
                        Dim sql As New StringBuilder
                        With sql
                            .Append("SELECT /* IC3040403_019 */ ")
                            .Append("       LPAD(SEQ_CAL_TODOITEM_TODOID.NEXTVAL, 20, '0') Sequence_Id ")
                            .Append("  FROM DUAL ")
                        End With
                        query.CommandText = sql.ToString()

                        'SQL実行（結果表を返却）
                        Dim dataTable As IC3040403DataSet.SequenceTableDataTable
                        dataTable = query.GetData()

                        Dim dataRow As IC3040403DataSet.SequenceTableRow = dataTable.Rows(0)
                        Return dataRow.Sequence_Id

                    Catch ex As SystemException
                        '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 START
                        Logger.Error(ex.Message, ex)
                        '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 END
                        Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.GetNewTodoIdSqlError)

                    End Try

                End Using

            End Get
        End Property


        ''' <summary>
        ''' イベントIDシーケンスから、新規イベントIDの取得
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property GetNewEventId() As String
            Get
                Using query As New DBSelectQuery(Of IC3040403DataSet.SequenceTableDataTable)("IC3040403_020")

                    Try
                        Dim sql As New StringBuilder
                        With sql
                            .Append("SELECT /* IC3040403_020 */ ")
                            .Append("       LPAD(SEQ_CAL_EVENTITEM_EVENTID.NEXTVAL, 20, '0') Sequence_Id")
                            .Append("  FROM DUAL ")
                        End With
                        query.CommandText = sql.ToString()

                        'SQL実行（結果表を返却）
                        Dim dataTable As IC3040403DataSet.SequenceTableDataTable
                        dataTable = query.GetData()

                        Dim dataRow As IC3040403DataSet.SequenceTableRow = dataTable.Rows(0)
                        Return dataRow.Sequence_Id

                    Catch ex As SystemException
                        '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 START
                        Logger.Error(ex.Message, ex)
                        '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 END
                        Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.GetNewEventIdSqlError)

                    End Try

                End Using

            End Get
        End Property


        ''' <summary>
        ''' ユニークIDシーケンスから、新規のユニークIDを取得
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetNewUniqueId(ByVal countryCode As String) As String

            Using query As New DBSelectQuery(Of IC3040403DataSet.SequenceTableDataTable)("IC3040403_021")

                Try

                    Dim sql As New StringBuilder
                    With sql
                        .Append("SELECT /* IC3040403_021 */ ")
                        .Append("       'ICROP' || :COUNTRYCODE || LPAD(SEQ_CAL_UNIQUEID.NEXTVAL, 20, '0') Sequence_Id ")
                        .Append("  FROM DUAL ")
                    End With

                    query.CommandText = sql.ToString()

                    query.AddParameterWithTypeValue(ConstCode.SequencdIdCountryCode, OracleDbType.Varchar2, countryCode)

                    'SQL実行（結果表を返却）
                    Dim dataTable As IC3040403DataSet.SequenceTableDataTable
                    dataTable = query.GetData()

                    Dim dataRow As IC3040403DataSet.SequenceTableRow = dataTable.Rows(0)
                    Return dataRow.Sequence_Id

                Catch ex As SystemException
                    '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 START
                    Logger.Error(ex.Message, ex)
                    '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 END
                    Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.GetNewUniqueIdSqlError)

                End Try

            End Using

        End Function

    End Class

    Public Class StaffCodeDataTable
        Inherits Global.System.ComponentModel.Component

        ''' <summary>
        ''' TODO一覧情報テーブルから、ＩＤに紐付く行のスタッフコードを取得します
        ''' </summary>
        ''' <param name="calenderId">カレンダーＩＤ</param>
        ''' <param name="todoId">TodoＩＤ</param>
        ''' <returns>スタッフコードの格納されたDataTable</returns>
        ''' <remarks></remarks>
        Public Function SelectStaffCodeTodoItem(ByVal calenderId As String, _
                                                ByVal todoId As String,
                                                ByVal afterodractId As String) As IC3040403DataSet.StaffCodeDataTableDataTable

            'Public Function SelectStaffCodeTodoItem(ByVal calenderId As String, _
            '                                        ByVal todoId As String) As IC3040403DataSet.StaffCodeDataTableDataTable

            Using query As New DBSelectQuery(Of IC3040403DataSet.StaffCodeDataTableDataTable)("IC3040403_022")

                Try

                    Dim sql As New StringBuilder
                    With sql
                        .Append("SELECT /* IC3040403_022 */ ")
                        .Append("       ACTSTAFFCD ")
                        .Append("     , RECSTAFFCD ")
                        .Append("  FROM TBL_CAL_TODOITEM ")
                        .Append(" WHERE DELFLG = '0' ")
                        .Append("   AND COMPLETIONFLG = '0' ")
                        If calenderId IsNot Nothing Then
                            .Append("  AND CALID = :CALID ")
                            query.AddParameterWithTypeValue(ConstCode.TodoItemTableCalId, OracleDbType.Char, calenderId)
                        End If
                        If todoId IsNot Nothing Then
                            .Append("  AND TODOID = :TODOID ")
                            query.AddParameterWithTypeValue(ConstCode.TodoItemTableTodoId, OracleDbType.Char, todoId)
                        End If
                        '2014/07/01 SKFC渡邊 NEXTSTEP CALDAV 不具合修正　START
                        If afterodractId IsNot Nothing Then
                            .Append("  AND AFTER_ODR_ACT_ID = :AFTER_ODR_ACT_ID ")
                            query.AddParameterWithTypeValue(ConstCode.TodoItemTableAfterOdrActID, OracleDbType.Varchar2, afterodractId)
                        End If
                        '2014/07/01 SKFC渡邊 NEXTSTEP CALDAV 不具合修正　END

                    End With

                    query.CommandText = sql.ToString()
                    'SQLパラメータ設定

                    'SQL実行（結果表を返却）
                    Return query.GetData()

                Catch ex As SystemException
                    '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 START
                    Logger.Error(ex.Message, ex)
                    '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 END
                    Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.SelectStaffCodeTodoItemSqlError)

                End Try

            End Using

        End Function

        ''' <summary>
        ''' イベント一覧情報テーブルから、カレンダーＩＤに紐付く行のスタッフコードを取得します
        ''' </summary>
        ''' <param name="todoId">ToDoId</param>
        ''' <returns>スタッフコードの格納されたDataTable</returns>
        ''' <remarks></remarks>
        Public Function SelectStaffCodeEventItem(ByVal calenderId As String, _
                                                 ByVal todoId As String) As IC3040403DataSet.StaffCodeDataTableDataTable

            Using query As New DBSelectQuery(Of IC3040403DataSet.StaffCodeDataTableDataTable)("IC3040403_023")

                Try

                    Dim sql As New StringBuilder
                    With sql
                        .Append("SELECT /* IC3040403_023 */ ")
                        .Append("       ACTSTAFFCD ")
                        .Append("     , RECSTAFFCD ")
                        .Append("  FROM TBL_CAL_EVENTITEM ")
                        .Append(" WHERE DELFLG = '0' ")
                        If calenderId IsNot Nothing Then
                            .Append("  AND CALID = :CALID ")
                            query.AddParameterWithTypeValue(ConstCode.EventItemTableCalId, OracleDbType.Varchar2, calenderId)
                        End If
                        If todoId IsNot Nothing Then
                            .Append("  AND TODOID = :TODOID ")
                            query.AddParameterWithTypeValue(ConstCode.EventItemTableTodoId, OracleDbType.Varchar2, todoId)
                        End If

                    End With

                    query.CommandText = sql.ToString()
                    'SQLパラメータ設定

                    'SQL実行（結果表を返却）
                    Return query.GetData()


                Catch ex As SystemException
                    '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 START
                    Logger.Error(ex.Message, ex)
                    '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 END
                    Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.SelectStaffCodeEventItemSqlError)

                End Try

            End Using

        End Function

    End Class

    Public Class CalCardLastModifyDataTable
        Inherits Global.System.ComponentModel.Component

        ''' <summary>
        ''' カレンダーアドレス最終更新日テーブルに、新しいスタッフを追加します。
        ''' </summary>
        ''' <param name="dataRow">引数</param>
        ''' <returns>登録件数</returns>
        ''' <remarks></remarks>
        Public Function InsertCalCardLastModify(ByVal dataRow As IC3040403DataSet.CalCardLastModifyDataTableRow) As Integer

            Using query As New DBUpdateQuery("IC3040403_024")

                Try
                    Dim sql As New StringBuilder
                    With sql
                        .Append(" INSERT /* IC3040403_024 */ ")
                        .Append("   INTO TBL_CAL_CARD_LASTMODIFY( ")
                        .Append("        STAFFCD ")
                        .Append("      , CALUPDATEDATE ")
                        .Append("      , CARDUPDATEDATE ")
                        .Append("      , CREATEDATE ")
                        .Append("      , UPDATEDATE ")
                        .Append("      , CREATEACCOUNT ")
                        .Append("      , UPDATEACCOUNT ")
                        .Append("      , CREATEID ")
                        .Append("      , UPDATEID ) ")
                        .Append(" VALUES (")
                        .Append("        :STAFFCD ")
                        .Append("      , SYSDATE ")
                        .Append("      , SYSDATE ")
                        .Append("      , SYSDATE ")
                        .Append("      , SYSDATE ")
                        .Append("      , :CREATEACCOUNT ")
                        .Append("      , :UPDATEACCOUNT ")
                        .Append("      , :CREATEID ")
                        .Append("      , :UPDATEID) ")
                    End With

                    query.CommandText = sql.ToString()
                    'SQLパラメータ設定

                    Using Convert As New IC3040403DataSetTableAdapters.IC3040403DataAdapter

                        query.AddParameterWithTypeValue(ConstCode.LastModifyDateTableStaffCD, OracleDbType.Varchar2, dataRow.STAFFCD)
                        query.AddParameterWithTypeValue(ConstCode.LastModifyDateTableCreateAccount, OracleDbType.Varchar2, dataRow.CREATEACCOUNT)
                        query.AddParameterWithTypeValue(ConstCode.LastModifyDateTableUpdateAccount, OracleDbType.Varchar2, dataRow.UPDATEACCOUNT)
                        query.AddParameterWithTypeValue(ConstCode.LastModifyDateTableCreateId, OracleDbType.Varchar2, dataRow.CREATEID)
                        query.AddParameterWithTypeValue(ConstCode.LastModifyDateTableUpdateId, OracleDbType.Varchar2, dataRow.UPDATEID)

                    End Using

                    'SQL実行（更新行を返却）
                    Dim Count As Integer = query.Execute()
                    Return Count

                Catch ex As SystemException
                    '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 START
                    Logger.Error(ex.Message, ex)
                    '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 END
                    Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.InsertCalCardLastModifySqlError)

                End Try

            End Using

        End Function

        ''' <summary>
        ''' カレンダーアドレス最終更新日テーブルのカレンダー情報更新日を更新します
        ''' </summary>
        ''' <param name="dataRow">引数</param>
        ''' <returns>更新結果</returns>
        ''' <remarks></remarks>
        Public Function UpdateCalCardLastModify(ByVal dataRow As IC3040403DataSet.CalCardLastModifyDataTableRow) As Integer

            Using query As New DBUpdateQuery("IC3040403_025")

                Try

                    Dim sql As New StringBuilder

                    Using Convert As New IC3040403DataSetTableAdapters.IC3040403DataAdapter

                        With sql
                            .Append(" UPDATE /* IC3040403_025 */ TBL_CAL_CARD_LASTMODIFY")
                            .Append("    SET UPDATEDATE = SYSDATE")
                            .Append("      , UPDATEACCOUNT = :UPDATEACCOUNT ")
                            .Append("      , UPDATEID = :UPDATEID")
                            .Append("      , CALUPDATEDATE = SYSDATE")
                            .Append(" WHERE  STAFFCD = :STAFFCD ")

                        End With

                    End Using

                    query.CommandText = sql.ToString()

                    query.AddParameterWithTypeValue(ConstCode.LastModifyDateTableStaffCD, OracleDbType.Varchar2, dataRow.STAFFCD)
                    query.AddParameterWithTypeValue(ConstCode.LastModifyDateTableUpdateAccount, OracleDbType.Varchar2, dataRow.UPDATEACCOUNT)
                    query.AddParameterWithTypeValue(ConstCode.LastModifyDateTableUpdateId, OracleDbType.Varchar2, dataRow.UPDATEID)

                    'SQL実行（更新行を返却）
                    Dim Count As Integer = query.Execute()
                    Return Count

                Catch ex As SystemException
                    '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 START
                    Logger.Error(ex.Message, ex)
                    '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 END
                    Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.UpdateCalCardLastModifySqlError)

                End Try

            End Using

        End Function

    End Class


#Region "変換メソッド"

    Public Class IC3040403DataAdapter
        Inherits Global.System.ComponentModel.Component
        Public Function ConvertStringDateTime(ByVal dateValue As String) As Object

            ' 文字列が空の場合、Nothingを返す
            If dateValue Is Nothing Or Validation.Equals(dateValue, ConstCode.EmptyString) Then

                Return Nothing

            End If

            ' 文字列をDateTimeに変換する。
            Dim dateData As DateTime = Nothing

            If DateTime.TryParse(dateValue, dateData) Then
                '変換に成功すれば、変換した値を返す

                Return dateData

            End If
            ' 変換に失敗した場合、Nothingを返す
            Return Nothing

        End Function

        ''' <summary>
        ''' 空文字をNothingに置き換える関数
        ''' </summary>
        ''' <param name="target">対象文字列</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ConvertStringEmpty(ByVal target As String) As String

            If Validation.Equals(target, "") Then

                Return Nothing

            End If

            Return target

        End Function

    End Class

#End Region

End Namespace



Partial Class IC3040403DataSet
End Class
