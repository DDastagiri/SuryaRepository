Imports System.Text
Imports Toyota.eCRB.SystemFrameworks.Configuration
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Oracle.DataAccess.Client

Namespace MC3040402DataSetTableAdapters

    ''' <summary>
    ''' カレンダーTODO退避ワークテーブル処理
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CalWKTodoPastAdapter
        Inherits Global.System.ComponentModel.Component

        ''' <summary>
        ''' カレンダーTODO退避ワークのトランケート
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function TruncateWKTodoPast() As Boolean

            Using query As New DBUpdateQuery("MC3040402_001")

                ' SQL生成
                Dim sql As New StringBuilder
                With sql
                    .Append("TRUNCATE /* MC3040402_001 */ ")
                    .Append("   TABLE TBL_CAL_WK_TODO_PAST ")
                End With

                query.CommandText = sql.ToString()

                ' SQL実行
                query.Execute()

                ' 戻り値返却
                Return True

            End Using

        End Function

    End Class

    ''' <summary>
    ''' カレンダーイベント退避ワークテーブル処理
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CalWKEventPastAdapter
        Inherits Global.System.ComponentModel.Component

        ''' <summary>
        ''' カレンダーイベント退避ワークのトランケート
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function TruncateWKEventPast() As Long

            Using query As New DBUpdateQuery("MC3040402_002")
                ' SQL生成
                Dim sql As New StringBuilder
                With sql
                    .Append("TRUNCATE /* MC3040402_002 */ ")
                    .Append("   TABLE TBL_CAL_WK_EVENT_PAST ")
                End With

                query.CommandText = sql.ToString()

                ' SQL実行
                query.Execute()

                ' 戻り値返却
                Return True

            End Using

        End Function

    End Class

    ''' <summary>
    ''' 退避対象抽出処理
    ''' </summary>
    ''' <remarks></remarks>
    Public Class PastDateDataTable
        Inherits Global.System.ComponentModel.Component

        ''' <summary>
        ''' 退避対象となるTodoIDを抽出する
        ''' </summary>
        ''' <param name="dataRow"></param>
        ''' <returns>処理件数</returns>
        ''' <remarks></remarks>
        ''' <history>2012/03/01 SKFC 加藤 【SALES_2】受注後工程の対応 START</history>

        Public Function InsertWKTodoPast(ByVal dataRow As MC3040402DataSet.PastDateRow) As Long

            Using query As New DBUpdateQuery("MC3040402_003")

                ' SQL生成
                Dim sql As New StringBuilder
                With sql
                    .Append("INSERT /* MC3040402_003 */ ")
                    .Append("  INTO TBL_CAL_WK_TODO_PAST ( ")
                    .Append("       TODOID ")
                    .Append("     , CALID ")
                    .Append("     , TODOFLG ")
                    .Append("     , CREATEDATE ")
                    .Append("     , UPDATEDATE ")
                    .Append("     , CREATEACCOUNT ")
                    .Append("     , UPDATEACCOUNT ")
                    .Append("     , CREATEID ")
                    .Append("     , UPDATEID ")
                    .Append(") ")
                    .Append("SELECT VTIT.TODOID ")
                    .Append("     , VTIT.CALID ")
                    .Append("     , '0' ")
                    .Append("     , SYSDATE ")
                    .Append("     , SYSDATE ")
                    .Append("     , :MODULEID ")
                    .Append("     , :MODULEID ")
                    .Append("     , :MODULEID ")
                    .Append("     , :MODULEID ")
                    .Append("  FROM (")

                    '■来店予約の完了データの抽出
                    .Append("    SELECT TIT.TODOID ")
                    .Append("         , TIT.CALID ")
                    .Append("      FROM TBL_CAL_TODOITEM TIT ")
                    .Append("         , TBL_CAL_ICROPINFO IIF ")
                    .Append("     WHERE TIT.CALID = IIF.CALID ")
                    .Append("       AND TIT.COMPLETIONFLG = '1' ")
                    .Append("       AND TIT.COMPLETIONDATE < :PASTDATE ")
                    '2012/03/30 SKFC 上田 【SALES_2】受注後工程の対応 START
                    .Append("       AND IIF.SCHEDULEDIV = '0' ")
                    '2012/03/30 SKFC 上田 【SALES_2】受注後工程の対応 END

                    '■削除データの抽出
                    .Append("     UNION ALL ")
                    .Append("    SELECT TIT.TODOID ")
                    .Append("         , TIT.CALID ")
                    .Append("      FROM TBL_CAL_TODOITEM TIT ")
                    .Append("         , TBL_CAL_ICROPINFO IIF ")
                    .Append("     WHERE TIT.CALID = IIF.CALID ")
                    .Append("       AND TIT.DELFLG = '1' ")
                    .Append("       AND TIT.DELDATE < :PASTDATE ")

                    '■入庫予約の終了日時を過ぎたデータの抽出
                    .Append("     UNION ALL ")
                    .Append("    SELECT TIT.TODOID ")
                    .Append("         , TIT.CALID ")
                    .Append("      FROM TBL_CAL_TODOITEM TIT ")
                    .Append("         , TBL_CAL_ICROPINFO IIF ")
                    .Append("     WHERE TIT.CALID = IIF.CALID ")
                    .Append("       AND IIF.SCHEDULEDIV = '1' ")
                    '2012/03/21 SKFC 加藤 【SALES_2】受注後工程の対応 START
                    '.Append("       AND TIT.ENDTIME < :PASTDATE) VTIT")
                    .Append("       AND TIT.ENDTIME < :PASTDATE")

                    '■受注後工程の全工程が完了したデータの抽出
                    .Append("     UNION ALL ")
                    '2012/03/30 SKFC 上田 【SALES_2】受注後工程の対応 START
                    '.Append("        SELECT    TIT1.CALID ")
                    '.Append("                , IIF1.EVENTID ")
                    '.Append("          FROM    TBL_CAL_TODOITEM   TIT1 ")
                    '.Append("                , TBL_CAL_EVENTITEM  IIF1 ")
                    '.Append("          WHERE NOT EXISTS ")
                    '.Append("          (SELECT   TIT2.CALID ")
                    '.Append("               FROM    TBL_CAL_TODOITEM TIT2 ")
                    '.Append("               WHERE   TIT2.COMPLETIONFLG = '0' ")
                    '.Append("               AND     TIT2.PROCESSDIV IS NOT NULL ")
                    '.Append("               AND     TIT1.CALID = TIT2.CALID ) ")
                    '.Append("               AND     TIT1.CALID = IIF1.CALID ")
                    '.Append("               AND     TIT1.ENDTIME < :PASTDATE")
                    '.Append("          GROUP BY TIT1.CALID, IIF1.EVENTID ")
                    '.Append("          ORDER BY TIT1.CALID, IIF1.EVENTID ")
                    .Append("    SELECT TIT.TODOID ")
                    .Append("         , TIT.CALID ")
                    .Append("      FROM TBL_CAL_TODOITEM TIT ")
                    .Append("         , TBL_CAL_ICROPINFO IIF ")
                    .Append("     WHERE TIT.CALID = IIF.CALID ")
                    .Append("       AND TIT.COMPLETIONFLG = '1' ")
                    .Append("       AND TIT.COMPLETIONDATE < :PASTDATE ")
                    .Append("       AND TIT.DELFLG = '0' ")
                    .Append("       AND NOT EXISTS( ")
                    .Append("        SELECT 1 ")
                    .Append("          FROM TBL_CAL_TODOITEM TIT2 ")
                    .Append("         WHERE TIT.CALID = TIT2.CALID ")
                    .Append("           AND ((TIT2.COMPLETIONFLG = '0') ")
                    .Append("            OR  (TIT2.COMPLETIONFLG = '1' ")
                    .Append("           AND   TIT2.COMPLETIONDATE >= :PASTDATE)) ")
                    .Append("           AND TIT2.DELFLG = '0') ")
                    .Append("       AND IIF.SCHEDULEDIV = '2' ")
                    '2012/03/30 SKFC 上田 【SALES_2】受注後工程の対応 END

                    .Append("       ) VTIT")
                    '2012/03/21 SKFC 加藤 【SALES_2】受注後工程の対応 END
                    .Append(" GROUP BY VTIT.TODOID, VTIT.CALID ")
                End With

                query.CommandText = sql.ToString()

                'SQLパラメータ設定
                With query
                    .AddParameterWithTypeValue("PASTDATE", OracleDbType.Date, dataRow.PASTDATE)
                    .AddParameterWithTypeValue("MODULEID", OracleDbType.Varchar2, dataRow.MODULEID)
                End With

                ' SQL実行(戻り値返却)
                Return query.Execute()

            End Using

        End Function

        ''' <summary>
        ''' 退避対象となるイベントIDを抽出する
        ''' </summary>
        ''' <param name="dataRow"></param>
        ''' <returns>処理件数</returns>
        ''' <remarks></remarks>
        Public Function InsertWKEventPast(ByVal dataRow As MC3040402DataSet.PastDateRow) As Long

            Using query As New DBUpdateQuery("MC3040402_004")

                ' SQL生成
                Dim sql As New StringBuilder
                With sql
                    .Append("INSERT /* MC3040402_004 */ ")
                    .Append("  INTO TBL_CAL_WK_EVENT_PAST ( ")
                    .Append("       EVENTID ")
                    .Append("     , CALID ")
                    .Append("     , CREATEDATE ")
                    .Append("     , UPDATEDATE ")
                    .Append("     , CREATEACCOUNT ")
                    .Append("     , UPDATEACCOUNT ")
                    .Append("     , CREATEID ")
                    .Append("     , UPDATEID ")
                    .Append(") ")
                    .Append("SELECT VEIT.EVENTID ")
                    .Append("     , VEIT.CALID ")
                    .Append("     , SYSDATE ")
                    .Append("     , SYSDATE ")
                    .Append("     , :MODULEID ")
                    .Append("     , :MODULEID ")
                    .Append("     , :MODULEID ")
                    .Append("     , :MODULEID ")
                    .Append("  FROM ( ")
                    .Append("    SELECT EVENTID ")
                    .Append("         , CALID ")
                    .Append("      FROM TBL_CAL_EVENTITEM ")
                    .Append("     WHERE CALID = 'NATIVE' ")
                    .Append("       AND RRULEFLG = '0' ")
                    .Append("       AND ENDTIME < :PASTDATE ")
                    .Append("    UNION ALL ")
                    .Append("    SELECT EVENTID ")
                    .Append("         , CALID ")
                    .Append("      FROM TBL_CAL_EVENTITEM ")
                    .Append("     WHERE CALID = 'NATIVE' ")
                    .Append("       AND RRULEFLG = '1' ")
                    .Append("       AND RRULE_UNTIL < :PASTDATE ")
                    .Append("    UNION ALL ")
                    .Append("    SELECT EVENTID ")
                    .Append("         , CALID ")
                    .Append("      FROM TBL_CAL_EVENTITEM ")
                    .Append("     WHERE CALID = 'NATIVE' ")
                    .Append("       AND DELFLG = '1' ")
                    .Append("       AND DELDATE < :PASTDATE) VEIT ")
                    .Append(" GROUP BY VEIT.EVENTID, VEIT.CALID ")
                End With
                query.CommandText = sql.ToString()

                'SQLパラメータ設定
                With query
                    .AddParameterWithTypeValue("PASTDATE", OracleDbType.Date, dataRow.PASTDATE)
                    .AddParameterWithTypeValue("MODULEID", OracleDbType.Varchar2, dataRow.MODULEID)
                End With

                ' SQL実行(戻り値返却)
                Return query.Execute()

            End Using

        End Function

    End Class

    ''' <summary>
    ''' カレンダーTODOアラーム退避テーブル処理
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CalTodoAlarmPastAdapter
        Inherits Global.System.ComponentModel.Component

        ''' <summary>
        ''' カレンダーTodoアラームの退避
        ''' </summary>
        ''' <returns>処理件数</returns>
        ''' <remarks></remarks>
        Public Function InsertTodoAlarmPast() As Long

            Using query As New DBUpdateQuery("MC3040402_008")

                ' SQL生成
                Dim sql As New StringBuilder
                With sql
                    .Append("INSERT /* MC3040402_008 */ ")
                    .Append("  INTO TBL_CAL_TODOALARM_PAST ( ")
                    .Append("       TODOID ")
                    .Append("     , SEQNO ")
                    .Append("     , STARTTRIGGER ")
                    .Append("     , CREATEDATE ")
                    .Append("     , UPDATEDATE ")
                    .Append("     , CREATEACCOUNT ")
                    .Append("     , UPDATEACCOUNT ")
                    .Append("     , CREATEID ")
                    .Append("     , UPDATEID ")
                    .Append(") ")
                    .Append("SELECT TAL.TODOID ")
                    .Append("     , TAL.SEQNO ")
                    .Append("     , TAL.STARTTRIGGER ")
                    .Append("     , TAL.CREATEDATE ")
                    .Append("     , TAL.UPDATEDATE ")
                    .Append("     , TAL.CREATEACCOUNT ")
                    .Append("     , TAL.UPDATEACCOUNT ")
                    .Append("     , TAL.CREATEID ")
                    .Append("     , TAL.UPDATEID ")
                    .Append("  FROM TBL_CAL_TODOALARM TAL ")
                    .Append("     , TBL_CAL_WK_TODO_PAST WTP ")
                    .Append(" WHERE TAL.TODOID = WTP.TODOID ")
                End With

                query.CommandText = sql.ToString()

                ' SQL実行(戻り値返却)
                Return query.Execute()

            End Using

        End Function

    End Class

    ''' <summary>
    ''' カレンダーTODO繰り返し除外日退避テーブル処理
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CalTodoExdatePastAdapter
        Inherits Global.System.ComponentModel.Component

        ''' <summary>
        ''' カレンダーTODO繰り返し除外日の退避
        ''' </summary>
        ''' <returns>処理件数</returns>
        ''' <remarks></remarks>
        Public Function InsertTodoExdatePast() As Long

            Using query As New DBUpdateQuery("MC3040402_009")

                ' SQL生成
                Dim sql As New StringBuilder
                With sql
                    .Append("INSERT /* MC3040402_009 */ ")
                    .Append("  INTO TBL_CAL_TODOEXDATE_PAST ( ")
                    .Append("       TODOID ")
                    .Append("     , SEQNO ")
                    .Append("     , EXDATE ")
                    .Append("     , CREATEDATE ")
                    .Append("     , UPDATEDATE ")
                    .Append("     , CREATEACCOUNT ")
                    .Append("     , UPDATEACCOUNT ")
                    .Append("     , CREATEID ")
                    .Append("     , UPDATEID ")
                    .Append(") ")
                    .Append("SELECT TEX.TODOID ")
                    .Append("     , TEX.SEQNO ")
                    .Append("     , TEX.EXDATE ")
                    .Append("     , TEX.CREATEDATE ")
                    .Append("     , TEX.UPDATEDATE ")
                    .Append("     , TEX.CREATEACCOUNT ")
                    .Append("     , TEX.UPDATEACCOUNT ")
                    .Append("     , TEX.CREATEID ")
                    .Append("     , TEX.UPDATEID ")
                    .Append("  FROM TBL_CAL_TODOEXDATE TEX ")
                    .Append("     , TBL_CAL_WK_TODO_PAST WTP ")
                    .Append(" WHERE TEX.TODOID = WTP.TODOID ")
                End With

                query.CommandText = sql.ToString()

                ' SQL実行(戻り値返却)
                Return query.Execute()

            End Using

        End Function

    End Class

    ''' <summary>
    ''' カレンダーTODO情報退避テーブル処理
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CalTodoItemPastAdapter
        Inherits Global.System.ComponentModel.Component

        ''' <summary>
        ''' カレンダーTODO情報の退避
        ''' </summary>
        ''' <returns>処理件数</returns>
        ''' <remarks></remarks>
        ''' <history>2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 START</history>
        Public Function InsertTodoItemPast() As Long

            Using query As New DBUpdateQuery("MC3040402_010")

                ' SQL生成
                Dim sql As New StringBuilder
                With sql
                    .Append("INSERT /* MC3040402_010 */ ")
                    .Append("  INTO TBL_CAL_TODOITEM_PAST ( ")
                    .Append("       TODOID ")
                    .Append("     , CALID ")
                    .Append("     , UNIQUEID ")
                    .Append("     , RECURRENCEID ")
                    .Append("     , CHGSEQNO ")
                    .Append("     , ACTSTAFFSTRCD ")
                    .Append("     , ACTSTAFFCD ")
                    .Append("     , RECSTAFFSTRCD ")
                    .Append("     , RECSTAFFCD ")
                    .Append("     , CONTACTNO ")
                    .Append("     , SUMMARY ")
                    .Append("     , STARTTIME ")
                    .Append("     , ENDTIME ")
                    .Append("     , STARTTIMEFLG ")
                    .Append("     , TIMEFLG ")
                    .Append("     , ALLDAYFLG ")
                    .Append("     , MEMO ")
                    .Append("     , ICROPCOLOR ")
                    .Append("     , PARENTDIV")
                    .Append("     , RRULE_FREQ ")
                    .Append("     , RRULE_INTERVAL ")
                    .Append("     , RRULE_UNTIL ")
                    .Append("     , RRULE_TEXT ")
                    .Append("     , COMPLETIONFLG ")
                    .Append("     , COMPLETIONDATE ")
                    .Append("     , DELFLG ")
                    .Append("     , DELDATE ")
                    .Append("     , CREATEDATE ")
                    .Append("     , UPDATEDATE ")
                    .Append("     , CREATEACCOUNT ")
                    .Append("     , UPDATEACCOUNT ")
                    .Append("     , CREATEID ")
                    .Append("     , UPDATEID ")
                    '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 START
                    .Append("     , PROCESSDIV ")
                    '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 END
                    '2014/04/05 SKFC 渡邊 NEXTSTEP_CALDAV START
                    .Append("     , CONTACT_NAME ")
                    .Append("     , ACT_ODR_NAME ")
                    .Append("     , ODR_DIV ")
                    .Append("     , AFTER_ODR_ACT_ID ")
                    '2014/04/05 SKFC 渡邊 NEXTSTEP_CALDAV END
                    .Append(") ")
                    .Append("SELECT TIT.TODOID ")
                    .Append("     , TIT.CALID ")
                    .Append("     , TIT.UNIQUEID ")
                    .Append("     , TIT.RECURRENCEID ")
                    .Append("     , TIT.CHGSEQNO ")
                    .Append("     , TIT.ACTSTAFFSTRCD ")
                    .Append("     , TIT.ACTSTAFFCD ")
                    .Append("     , TIT.RECSTAFFSTRCD ")
                    .Append("     , TIT.RECSTAFFCD ")
                    .Append("     , TIT.CONTACTNO ")
                    .Append("     , TIT.SUMMARY ")
                    .Append("     , TIT.STARTTIME ")
                    .Append("     , TIT.ENDTIME ")
                    .Append("     , TIT.STARTTIMEFLG ")
                    .Append("     , TIT.TIMEFLG ")
                    .Append("     , TIT.ALLDAYFLG ")
                    .Append("     , TIT.MEMO ")
                    .Append("     , TIT.ICROPCOLOR ")
                    .Append("     , TIT.PARENTDIV ")
                    .Append("     , TIT.RRULE_FREQ ")
                    .Append("     , TIT.RRULE_INTERVAL ")
                    .Append("     , TIT.RRULE_UNTIL ")
                    .Append("     , TIT.RRULE_TEXT ")
                    .Append("     , TIT.COMPLETIONFLG ")
                    .Append("     , TIT.COMPLETIONDATE ")
                    .Append("     , TIT.DELFLG ")
                    .Append("     , TIT.DELDATE ")
                    .Append("     , TIT.CREATEDATE ")
                    .Append("     , TIT.UPDATEDATE ")
                    .Append("     , TIT.CREATEACCOUNT ")
                    .Append("     , TIT.UPDATEACCOUNT ")
                    .Append("     , TIT.CREATEID ")
                    .Append("     , TIT.UPDATEID ")
                    '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 START
                    .Append("     , TIT.PROCESSDIV ")
                    '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 END
                    '2014/04/05 SKFC 渡邊 NEXTSTEP_CALDAV START
                    .Append("     , TIT.CONTACT_NAME ")
                    .Append("     , TIT.ACT_ODR_NAME ")
                    .Append("     , TIT.ODR_DIV ")
                    .Append("     , TIT.AFTER_ODR_ACT_ID ")
                    '2014/04/05 SKFC 渡邊 NEXTSTEP_CALDAV END
                    .Append("  FROM TBL_CAL_TODOITEM TIT ")
                    .Append("     , TBL_CAL_WK_TODO_PAST WTP ")
                    .Append(" WHERE TIT.TODOID = WTP.TODOID ")
                End With

                query.CommandText = sql.ToString()

                ' SQL実行(戻り値返却)
                Return query.Execute()

            End Using

        End Function
    End Class

    ''' <summary>
    ''' カレンダーイベントアラーム退避テーブル処理
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CalEventAlarmPastAdapter
        Inherits Global.System.ComponentModel.Component

        ''' <summary>
        ''' カレンダーイベントアラームの退避
        ''' </summary>
        ''' <returns>処理件数</returns>
        ''' <remarks></remarks>
        Public Function InsertEventAlarmPast() As Long

            Using query As New DBUpdateQuery("MC3040402_005")

                ' SQL生成
                Dim sql As New StringBuilder
                With sql
                    .Append("INSERT /* MC3040402_005 */ ")
                    .Append("  INTO TBL_CAL_EVENTALARM_PAST ( ")
                    .Append("       EVENTID ")
                    .Append("     , SEQNO ")
                    .Append("     , STARTTRIGGER ")
                    .Append("     , CREATEDATE ")
                    .Append("     , UPDATEDATE ")
                    .Append("     , CREATEACCOUNT ")
                    .Append("     , UPDATEACCOUNT ")
                    .Append("     , CREATEID ")
                    .Append("     , UPDATEID ")
                    .Append(") ")
                    .Append("SELECT EAL.EVENTID ")
                    .Append("     , EAL.SEQNO ")
                    .Append("     , EAL.STARTTRIGGER ")
                    .Append("     , EAL.CREATEDATE ")
                    .Append("     , EAL.UPDATEDATE ")
                    .Append("     , EAL.CREATEACCOUNT ")
                    .Append("     , EAL.UPDATEACCOUNT ")
                    .Append("     , EAL.CREATEID ")
                    .Append("     , EAL.UPDATEID ")
                    .Append("  FROM TBL_CAL_EVENTALARM EAL ")
                    .Append("     , TBL_CAL_EVENTITEM EIT ")
                    .Append("     , TBL_CAL_WK_TODO_PAST WTP ")
                    .Append(" WHERE EAL.EVENTID = EIT.EVENTID ")
                    .Append("   AND EIT.TODOID = WTP.TODOID ")
                End With

                query.CommandText = sql.ToString()

                ' SQL実行(戻り値返却)
                Return query.Execute()

            End Using

        End Function

        ''' <summary>
        ''' カレンダーイベントアラームの退避(Native分)
        ''' </summary>
        ''' <returns>処理件数</returns>
        ''' <remarks></remarks>
        Public Function InsertEventAlarmPastNative() As Long

            Using query As New DBUpdateQuery("MC3040402_011")

                ' SQL生成
                Dim sql As New StringBuilder
                With sql
                    .Append("INSERT /* MC3040402_011 */ ")
                    .Append("  INTO TBL_CAL_EVENTALARM_PAST ( ")
                    .Append("       EVENTID ")
                    .Append("     , SEQNO ")
                    .Append("     , STARTTRIGGER ")
                    .Append("     , CREATEDATE ")
                    .Append("     , UPDATEDATE ")
                    .Append("     , CREATEACCOUNT ")
                    .Append("     , UPDATEACCOUNT ")
                    .Append("     , CREATEID ")
                    .Append("     , UPDATEID ")
                    .Append(") ")
                    .Append("SELECT EAL.EVENTID ")
                    .Append("     , EAL.SEQNO ")
                    .Append("     , EAL.STARTTRIGGER ")
                    .Append("     , EAL.CREATEDATE ")
                    .Append("     , EAL.UPDATEDATE ")
                    .Append("     , EAL.CREATEACCOUNT ")
                    .Append("     , EAL.UPDATEACCOUNT ")
                    .Append("     , EAL.CREATEID ")
                    .Append("     , EAL.UPDATEID ")
                    .Append("  FROM TBL_CAL_EVENTALARM EAL ")
                    .Append("     , TBL_CAL_WK_EVENT_PAST WEP ")
                    .Append(" WHERE EAL.EVENTID = WEP.EVENTID ")
                End With

                query.CommandText = sql.ToString()

                ' SQL実行(戻り値返却)
                Return query.Execute()

            End Using

        End Function

    End Class

    ''' <summary>
    ''' カレンダーイベント繰り返し除外日退避テーブル処理
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CalEventExDatePastAdapter
        Inherits Global.System.ComponentModel.Component

        ''' <summary>
        ''' カレンダーイベント繰り返し除外日の退避
        ''' </summary>
        ''' <returns>処理件数</returns>
        ''' <remarks></remarks>
        Public Function InsertEventExdatePast() As Long

            Using query As New DBUpdateQuery("MC3040402_006")

                ' SQL生成
                Dim sql As New StringBuilder
                With sql
                    .Append("INSERT /* MC3040402_006 */ ")
                    .Append("  INTO TBL_CAL_EVENTEXDATE_PAST ( ")
                    .Append("       EVENTID ")
                    .Append("     , SEQNO ")
                    .Append("     , EXDATE ")
                    .Append("     , CREATEDATE ")
                    .Append("     , UPDATEDATE ")
                    .Append("     , CREATEACCOUNT ")
                    .Append("     , UPDATEACCOUNT ")
                    .Append("     , CREATEID ")
                    .Append("     , UPDATEID ")
                    .Append(") ")
                    .Append("SELECT EEX.EVENTID ")
                    .Append("     , EEX.SEQNO ")
                    .Append("     , EEX.EXDATE ")
                    .Append("     , EEX.CREATEDATE ")
                    .Append("     , EEX.UPDATEDATE ")
                    .Append("     , EEX.CREATEACCOUNT ")
                    .Append("     , EEX.UPDATEACCOUNT ")
                    .Append("     , EEX.CREATEID ")
                    .Append("     , EEX.UPDATEID ")
                    .Append("  FROM TBL_CAL_EVENTEXDATE EEX ")
                    .Append("     , TBL_CAL_EVENTITEM EIT ")
                    .Append("     , TBL_CAL_WK_TODO_PAST WTP ")
                    .Append(" WHERE EEX.EVENTID = EIT.EVENTID ")
                    .Append("   AND EIT.TODOID = WTP.TODOID ")
                End With

                query.CommandText = sql.ToString()

                ' SQL実行(戻り値返却)
                Return query.Execute()

            End Using

        End Function

        ''' <summary>
        ''' カレンダーイベント繰り返し除外日の退避(Native分)
        ''' </summary>
        ''' <returns>処理件数</returns>
        ''' <remarks></remarks>
        Public Function InsertEventExdatePastNative() As Long

            Using query As New DBUpdateQuery("MC3040402_012")

                ' SQL生成
                Dim sql As New StringBuilder
                With sql
                    .Append("INSERT /* MC3040402_012 */ ")
                    .Append("  INTO TBL_CAL_EVENTEXDATE_PAST ( ")
                    .Append("       EVENTID ")
                    .Append("     , SEQNO ")
                    .Append("     , EXDATE ")
                    .Append("     , CREATEDATE ")
                    .Append("     , UPDATEDATE ")
                    .Append("     , CREATEACCOUNT ")
                    .Append("     , UPDATEACCOUNT ")
                    .Append("     , CREATEID ")
                    .Append("     , UPDATEID ")
                    .Append(") ")
                    .Append("SELECT EEX.EVENTID ")
                    .Append("     , EEX.SEQNO ")
                    .Append("     , EEX.EXDATE ")
                    .Append("     , EEX.CREATEDATE ")
                    .Append("     , EEX.UPDATEDATE ")
                    .Append("     , EEX.CREATEACCOUNT ")
                    .Append("     , EEX.UPDATEACCOUNT ")
                    .Append("     , EEX.CREATEID ")
                    .Append("     , EEX.UPDATEID ")
                    .Append("  FROM TBL_CAL_EVENTEXDATE EEX ")
                    .Append("     , TBL_CAL_WK_EVENT_PAST WEP ")
                    .Append(" WHERE EEX.EVENTID = WEP.EVENTID ")

                End With

                query.CommandText = sql.ToString()

                ' SQL実行(戻り値返却)
                Return query.Execute()

            End Using

        End Function

    End Class

    ''' <summary>
    ''' カレンダーイベント情報退避テーブル処理
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CalEventItemPastAdapter
        Inherits Global.System.ComponentModel.Component

        ''' <summary>
        ''' カレンダーイベント情報の退避
        ''' </summary>
        ''' <returns>処理件数</returns>
        ''' <remarks></remarks>
        ''' <history>2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 START</history>
        Public Function InsertEventItemPast() As Long

            Using query As New DBUpdateQuery("MC3040402_007")

                ' SQL生成
                Dim sql As New StringBuilder
                With sql
                    .Append("INSERT /* MC3040402_007 */ ")
                    .Append("  INTO TBL_CAL_EVENTITEM_PAST ( ")
                    .Append("       EVENTID ")
                    .Append("     , CALID ")
                    .Append("     , TODOID ")
                    .Append("     , UNIQUEID ")
                    .Append("     , RECURRENCEID ")
                    .Append("     , CHGSEQNO ")
                    .Append("     , ACTSTAFFSTRCD ")
                    .Append("     , ACTSTAFFCD ")
                    .Append("     , RECSTAFFSTRCD ")
                    .Append("     , RECSTAFFCD ")
                    .Append("     , CONTACTNO ")
                    .Append("     , SUMMARY ")
                    .Append("     , STARTTIME ")
                    .Append("     , ENDTIME ")
                    .Append("     , TIMEFLG ")
                    .Append("     , ALLDAYFLG ")
                    .Append("     , MEMO ")
                    .Append("     , ICROPCOLOR ")
                    .Append("     , RRULE_FREQ ")
                    .Append("     , RRULE_INTERVAL ")
                    .Append("     , RRULE_UNTIL ")
                    .Append("     , RRULE_TEXT ")
                    .Append("     , LOCATION ")
                    .Append("     , ATTENDEE ")
                    .Append("     , TRANSP ")
                    .Append("     , URL ")
                    .Append("     , DELFLG ")
                    .Append("     , DELDATE ")
                    .Append("     , CREATEDATE ")
                    .Append("     , UPDATEDATE ")
                    .Append("     , CREATEACCOUNT ")
                    .Append("     , UPDATEACCOUNT ")
                    .Append("     , CREATEID ")
                    .Append("     , UPDATEID ")
                    '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 START
                    .Append("     , PROCESSDIV ")
                    '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 START
                    '2014/04/05 SKFC 渡邊 NEXTSTEP_CALDAV START
                    .Append("     , CONTACT_NAME ")
                    .Append("     , ACT_ODR_NAME ")
                    .Append("     , ODR_DIV ")
                    .Append("     , AFTER_ODR_ACT_ID ")
                    '2014/04/05 SKFC 渡邊 NEXTSTEP_CALDAV END
                    .Append(") ")
                    .Append("SELECT EIT.EVENTID ")
                    .Append("     , EIT.CALID ")
                    .Append("     , EIT.TODOID ")
                    .Append("     , EIT.UNIQUEID ")
                    .Append("     , EIT.RECURRENCEID ")
                    .Append("     , EIT.CHGSEQNO ")
                    .Append("     , EIT.ACTSTAFFSTRCD ")
                    .Append("     , EIT.ACTSTAFFCD ")
                    .Append("     , EIT.RECSTAFFSTRCD ")
                    .Append("     , EIT.RECSTAFFCD ")
                    .Append("     , EIT.CONTACTNO ")
                    .Append("     , EIT.SUMMARY ")
                    .Append("     , EIT.STARTTIME ")
                    .Append("     , EIT.ENDTIME ")
                    .Append("     , EIT.TIMEFLG ")
                    .Append("     , EIT.ALLDAYFLG ")
                    .Append("     , EIT.MEMO ")
                    .Append("     , EIT.ICROPCOLOR ")
                    .Append("     , EIT.RRULE_FREQ ")
                    .Append("     , EIT.RRULE_INTERVAL ")
                    .Append("     , EIT.RRULE_UNTIL ")
                    .Append("     , EIT.RRULE_TEXT ")
                    .Append("     , EIT.LOCATION ")
                    .Append("     , EIT.ATTENDEE ")
                    .Append("     , EIT.TRANSP ")
                    .Append("     , EIT.URL ")
                    .Append("     , EIT.DELFLG ")
                    .Append("     , EIT.DELDATE ")
                    .Append("     , EIT.CREATEDATE ")
                    .Append("     , EIT.UPDATEDATE ")
                    .Append("     , EIT.CREATEACCOUNT ")
                    .Append("     , EIT.UPDATEACCOUNT ")
                    .Append("     , EIT.CREATEID ")
                    .Append("     , EIT.UPDATEID ")
                    '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 START
                    .Append("     , EIT.PROCESSDIV ")
                    '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 START
                    '2014/04/05 SKFC 渡邊 NEXTSTEP_CALDAV START
                    .Append("     , EIT.CONTACT_NAME ")
                    .Append("     , EIT.ACT_ODR_NAME ")
                    .Append("     , EIT.ODR_DIV ")
                    .Append("     , EIT.AFTER_ODR_ACT_ID ")
                    '2014/04/05 SKFC 渡邊 NEXTSTEP_CALDAV END
                    .Append("  FROM TBL_CAL_EVENTITEM EIT ")
                    .Append("     , TBL_CAL_WK_TODO_PAST WTP ")
                    .Append(" WHERE EIT.TODOID = WTP.TODOID ")
                End With

                query.CommandText = sql.ToString()

                ' SQL実行(戻り値返却)
                Return query.Execute()

            End Using

        End Function

        ''' <summary>
        ''' カレンダーイベント情報の退避(Native分)
        ''' </summary>
        ''' <returns>処理件数</returns>
        ''' <remarks></remarks>
        ''' <history>2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 START</history>
        Public Function InsertEventItemPastNative() As Long

            Using query As New DBUpdateQuery("MC3040402_013")

                ' SQL生成
                Dim sql As New StringBuilder
                With sql
                    .Append("INSERT /* MC3040402_013 */ ")
                    .Append("  INTO TBL_CAL_EVENTITEM_PAST ( ")
                    .Append("       EVENTID ")
                    .Append("     , CALID ")
                    .Append("     , TODOID ")
                    .Append("     , UNIQUEID ")
                    .Append("     , RECURRENCEID ")
                    .Append("     , CHGSEQNO ")
                    .Append("     , ACTSTAFFSTRCD ")
                    .Append("     , ACTSTAFFCD ")
                    .Append("     , RECSTAFFSTRCD ")
                    .Append("     , RECSTAFFCD ")
                    .Append("     , CONTACTNO ")
                    .Append("     , SUMMARY ")
                    .Append("     , STARTTIME ")
                    .Append("     , ENDTIME ")
                    .Append("     , TIMEFLG ")
                    .Append("     , ALLDAYFLG ")
                    .Append("     , MEMO ")
                    .Append("     , ICROPCOLOR ")
                    .Append("     , RRULE_FREQ ")
                    .Append("     , RRULE_INTERVAL ")
                    .Append("     , RRULE_UNTIL ")
                    .Append("     , RRULE_TEXT ")
                    .Append("     , LOCATION ")
                    .Append("     , ATTENDEE ")
                    .Append("     , TRANSP ")
                    .Append("     , URL ")
                    .Append("     , DELFLG ")
                    .Append("     , DELDATE ")
                    .Append("     , CREATEDATE ")
                    .Append("     , UPDATEDATE ")
                    .Append("     , CREATEACCOUNT ")
                    .Append("     , UPDATEACCOUNT ")
                    .Append("     , CREATEID ")
                    .Append("     , UPDATEID ")
                    '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 START
                    .Append("     , PROCESSDIV ")
                    '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 END
                    '2014/04/05 SKFC 渡邊 NEXTSTEP_CALDAV START
                    .Append("     , CONTACT_NAME ")
                    .Append("     , ACT_ODR_NAME ")
                    .Append("     , ODR_DIV ")
                    .Append("     , AFTER_ODR_ACT_ID ")
                    '2014/04/05 SKFC 渡邊 NEXTSTEP_CALDAV END
                    .Append(") ")
                    .Append("SELECT EIT.EVENTID ")
                    .Append("     , EIT.CALID ")
                    .Append("     , EIT.TODOID ")
                    .Append("     , EIT.UNIQUEID ")
                    .Append("     , EIT.RECURRENCEID ")
                    .Append("     , EIT.CHGSEQNO ")
                    .Append("     , EIT.ACTSTAFFSTRCD ")
                    .Append("     , EIT.ACTSTAFFCD ")
                    .Append("     , EIT.RECSTAFFSTRCD ")
                    .Append("     , EIT.RECSTAFFCD ")
                    .Append("     , EIT.CONTACTNO ")
                    .Append("     , EIT.SUMMARY ")
                    .Append("     , EIT.STARTTIME ")
                    .Append("     , EIT.ENDTIME ")
                    .Append("     , EIT.TIMEFLG ")
                    .Append("     , EIT.ALLDAYFLG ")
                    .Append("     , EIT.MEMO ")
                    .Append("     , EIT.ICROPCOLOR ")
                    .Append("     , EIT.RRULE_FREQ ")
                    .Append("     , EIT.RRULE_INTERVAL ")
                    .Append("     , EIT.RRULE_UNTIL ")
                    .Append("     , EIT.RRULE_TEXT ")
                    .Append("     , EIT.LOCATION ")
                    .Append("     , EIT.ATTENDEE ")
                    .Append("     , EIT.TRANSP ")
                    .Append("     , EIT.URL ")
                    .Append("     , EIT.DELFLG ")
                    .Append("     , EIT.DELDATE ")
                    .Append("     , EIT.CREATEDATE ")
                    .Append("     , EIT.UPDATEDATE ")
                    .Append("     , EIT.CREATEACCOUNT ")
                    .Append("     , EIT.UPDATEACCOUNT ")
                    .Append("     , EIT.CREATEID ")
                    .Append("     , EIT.UPDATEID ")
                    '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 START
                    .Append("     , EIT.PROCESSDIV ")
                    '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 END
                    '2014/04/05 SKFC 渡邊 NEXTSTEP_CALDAV START
                    .Append("     , EIT.CONTACT_NAME ")
                    .Append("     , EIT.ACT_ODR_NAME ")
                    .Append("     , EIT.ODR_DIV ")
                    .Append("     , EIT.AFTER_ODR_ACT_ID ")
                    '2014/04/05 SKFC 渡邊 NEXTSTEP_CALDAV END
                    .Append("  FROM TBL_CAL_EVENTITEM EIT ")
                    .Append("     , TBL_CAL_WK_EVENT_PAST WEP ")
                    .Append(" WHERE EIT.EVENTID = WEP.EVENTID ")
                End With

                query.CommandText = sql.ToString()

                ' SQL実行(戻り値返却)
                Return query.Execute()

            End Using

        End Function

    End Class

    ''' <summary>
    ''' カレンダーTodoアラームテーブル処理
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CalTodoAlarmAdapter
        Inherits Global.System.ComponentModel.Component

        ''' <summary>
        ''' カレンダーTodoアラームより退避データの削除
        ''' </summary>
        ''' <returns>処理件数</returns>
        ''' <remarks></remarks>
        Public Function DeleteTodoAlarm() As Long

            Using query As New DBUpdateQuery("MC3040402_017")

                ' SQL生成
                Dim sql As New StringBuilder
                With sql
                    .Append("DELETE /* MC3040402_017 */ ")
                    .Append("  FROM TBL_CAL_TODOALARM TAL ")
                    .Append(" WHERE EXISTS ( ")
                    .Append("    SELECT 1 ")
                    .Append("      FROM TBL_CAL_WK_TODO_PAST WTP ")
                    .Append("     WHERE TAL.TODOID = WTP.TODOID) ")
                End With

                query.CommandText = sql.ToString()

                ' SQL実行(戻り値返却)
                Return query.Execute()

            End Using

        End Function

    End Class

    ''' <summary>
    ''' カレンダーTODO繰り返し除外日テーブル処理
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CalTodoExDateAdapter
        Inherits Global.System.ComponentModel.Component

        ''' <summary>
        ''' カレンダーTODO繰り返し除外日より退避データの削除
        ''' </summary>
        ''' <returns>処理件数</returns>
        ''' <remarks></remarks>
        Public Function DeleteTodoExdate() As Long

            Using query As New DBUpdateQuery("MC3040402_018")

                ' SQL生成
                Dim sql As New StringBuilder
                With sql
                    .Append("DELETE /* MC3040402_018 */ ")
                    .Append("  FROM TBL_CAL_TODOEXDATE TEX ")
                    .Append(" WHERE EXISTS ( ")
                    .Append("    SELECT 1 ")
                    .Append("      FROM TBL_CAL_WK_TODO_PAST WTP ")
                    .Append("     WHERE TEX.TODOID = WTP.TODOID) ")
                End With

                query.CommandText = sql.ToString()

                ' SQL実行(戻り値返却)
                Return query.Execute()

            End Using

        End Function

    End Class

    ''' <summary>
    ''' カレンダーTODO情報テーブル処理
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CalTodoItemAdapter
        Inherits Global.System.ComponentModel.Component

        ''' <summary>
        ''' カレンダーTODO情報より退避データの削除
        ''' </summary>
        ''' <returns>処理件数</returns>
        ''' <remarks></remarks>
        Public Function DeleteTodoItem() As Long

            Using query As New DBUpdateQuery("MC3040402_019")

                ' SQL生成
                Dim sql As New StringBuilder
                With sql
                    .Append("DELETE /* MC3040402_019 */ ")
                    .Append("  FROM TBL_CAL_TODOITEM TIT ")
                    .Append(" WHERE EXISTS ( ")
                    .Append("    SELECT 1 ")
                    .Append("      FROM TBL_CAL_WK_TODO_PAST WTP ")
                    .Append("     WHERE TIT.TODOID = WTP.TODOID) ")
                End With

                query.CommandText = sql.ToString()

                ' SQL実行(戻り値返却)
                Return query.Execute()

            End Using

        End Function

    End Class

    ''' <summary>
    ''' カレンダーイベントアラームテーブル処理
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CalEventAlarmAdapter
        Inherits Global.System.ComponentModel.Component

        ''' <summary>
        ''' カレンダーイベントアラームより退避データの削除
        ''' </summary>
        ''' <returns>処理件数</returns>
        ''' <remarks></remarks>
        Public Function DeleteEventAlarm() As Long

            Using query As New DBUpdateQuery("MC3040402_014")

                ' SQL生成
                Dim sql As New StringBuilder
                With sql
                    .Append("DELETE /* MC3040402_014 */ ")
                    .Append("  FROM TBL_CAL_EVENTALARM EAL ")
                    .Append(" WHERE EXISTS ( ")
                    .Append("    SELECT 1 ")
                    .Append("      FROM TBL_CAL_EVENTITEM EIT ")
                    .Append("         , TBL_CAL_WK_TODO_PAST WTP ")
                    .Append("     WHERE EAL.EVENTID = EIT.EVENTID ")
                    .Append("       AND EIT.TODOID = WTP.TODOID) ")
                End With

                query.CommandText = sql.ToString()

                ' SQL実行(戻り値返却)
                Return query.Execute()

            End Using

        End Function

        ''' <summary>
        ''' カレンダーイベントアラームより退避データの削除(Native分)
        ''' </summary>
        ''' <returns>処理件数</returns>
        ''' <remarks></remarks>
        Public Function DeleteEventAlarmNative() As Long

            Using query As New DBUpdateQuery("MC3040402_020")

                ' SQL生成
                Dim sql As New StringBuilder
                With sql
                    .Append("DELETE /* MC3040402_020 */ ")
                    .Append("  FROM TBL_CAL_EVENTALARM EAL ")
                    .Append(" WHERE EXISTS ( ")
                    .Append("    SELECT 1 ")
                    .Append("      FROM TBL_CAL_WK_EVENT_PAST WEP ")
                    .Append("     WHERE EAL.EVENTID = WEP.EVENTID) ")
                End With

                query.CommandText = sql.ToString()

                ' SQL実行(戻り値返却)
                Return query.Execute()

            End Using

        End Function

    End Class

    ''' <summary>
    ''' カレンダーイベント繰り返し除外日テーブル処理
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CalEventExdateAdapter
        Inherits Global.System.ComponentModel.Component

        ''' <summary>
        ''' カレンダーイベント繰り返し除外日より退避データの削除 
        ''' </summary>
        ''' <returns>処理件数</returns>
        ''' <remarks></remarks>
        Public Function DeleteEventExdate() As Long

            Using query As New DBUpdateQuery("MC3040402_015")

                ' SQL生成
                Dim sql As New StringBuilder
                With sql
                    .Append("DELETE /* MC3040402_015 */ ")
                    .Append("  FROM TBL_CAL_EVENTEXDATE EEX ")
                    .Append(" WHERE EXISTS ( ")
                    .Append("    SELECT 1 ")
                    .Append("      FROM TBL_CAL_EVENTITEM EIT ")
                    .Append("         , TBL_CAL_WK_TODO_PAST WTP ")
                    .Append("     WHERE EEX.EVENTID = EIT.EVENTID ")
                    .Append("       AND EIT.TODOID = WTP.TODOID) ")
                End With

                query.CommandText = sql.ToString()

                ' SQL実行(戻り値返却)
                Return query.Execute()

            End Using

        End Function

        ''' <summary>
        ''' カレンダーイベント繰り返し除外日より退避データの削除(Native分)
        ''' </summary>
        ''' <returns>処理件数</returns>
        ''' <remarks></remarks>
        Public Function DeleteEventExdateNative() As Long

            Using query As New DBUpdateQuery("MC3040402_021")

                ' SQL生成
                Dim sql As New StringBuilder
                With sql
                    .Append("DELETE /* MC3040402_021 */ ")
                    .Append("  FROM TBL_CAL_EVENTEXDATE EEX ")
                    .Append(" WHERE EXISTS ( ")
                    .Append("    SELECT 1 ")
                    .Append("      FROM TBL_CAL_WK_EVENT_PAST WEP ")
                    .Append("     WHERE EEX.EVENTID = WEP.EVENTID) ")
                End With

                query.CommandText = sql.ToString()

                ' SQL実行(戻り値返却)
                Return query.Execute()

            End Using

        End Function

    End Class

    ''' <summary>
    ''' カレンダーイベント情報テーブル処理
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CalEventItemAdapter
        Inherits Global.System.ComponentModel.Component

        ''' <summary>
        ''' カレンダーイベント情報より退避データの削除
        ''' </summary>
        ''' <returns>処理件数</returns>
        ''' <remarks></remarks>
        Public Function DeleteEventItem() As Long

            Using query As New DBUpdateQuery("MC3040402_016")

                ' SQL生成
                Dim sql As New StringBuilder
                With sql
                    .Append("DELETE /* MC3040402_016 */ ")
                    .Append("  FROM TBL_CAL_EVENTITEM TIT ")
                    .Append(" WHERE EXISTS ( ")
                    .Append("    SELECT 1 ")
                    .Append("      FROM TBL_CAL_WK_TODO_PAST WTP ")
                    .Append("     WHERE TIT.TODOID = WTP.TODOID) ")
                End With

                query.CommandText = sql.ToString()

                ' SQL実行(戻り値返却)
                Return query.Execute()

            End Using

        End Function

        ''' <summary>
        ''' カレンダーイベント情報より退避データの削除(Native分)
        ''' </summary>
        ''' <returns>処理件数</returns>
        ''' <remarks></remarks>
        Public Function DeleteEventItemNative() As Long

            Using query As New DBUpdateQuery("MC3040402_022")

                ' SQL生成
                Dim sql As New StringBuilder
                With sql
                    .Append("DELETE /* MC3040402_022 */ ")
                    .Append("  FROM TBL_CAL_EVENTITEM EIT ")
                    .Append(" WHERE EXISTS ( ")
                    .Append("    SELECT 1  ")
                    .Append("      FROM TBL_CAL_WK_EVENT_PAST WEP ")
                    .Append("     WHERE EIT.EVENTID = WEP.EVENTID) ")
                End With

                query.CommandText = sql.ToString()

                ' SQL実行(戻り値返却)
                Return query.Execute()

            End Using

        End Function

    End Class

    ''' <summary>
    ''' 退避対象抽出処理
    ''' </summary>
    ''' <remarks></remarks>
    Public Class UpdateTodoFlgDataTable
        Inherits Global.System.ComponentModel.Component

        ''' <summary>
        ''' カレンダーIDに該当するTodo情報の存在チェック
        ''' </summary>
        ''' <returns>処理件数</returns>
        ''' <remarks></remarks>
        Public Function UpdateTodoFlg(ByVal dataRow As MC3040402DataSet.UpdateTodoFlgRow) As Long

            Using query As New DBUpdateQuery("MC3040402_023")

                ' SQL生成
                Dim sql As New StringBuilder
                With sql
                    .Append("UPDATE /* MC3040402_023 */ ")
                    .Append("       TBL_CAL_WK_TODO_PAST WTP ")
                    .Append("   SET TODOFLG = '1' ")
                    .Append("     , UPDATEDATE = SYSDATE ")
                    .Append("     , UPDATEID = :MODULEID ")
                    .Append(" WHERE EXISTS ( ")
                    .Append("    SELECT 1 ")
                    .Append("      FROM TBL_CAL_TODOITEM TIT ")
                    .Append("     WHERE WTP.CALID = TIT.CALID) ")
                End With

                query.CommandText = sql.ToString()

                'SQLパラメータ設定
                With query
                    .AddParameterWithTypeValue("MODULEID", OracleDbType.Varchar2, dataRow.MODULEID)
                End With

                ' SQL実行(戻り値返却)
                Return query.Execute()

            End Using

        End Function

    End Class

    ''' <summary>
    ''' カレンダーiCROP情報退避テーブル処理
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CalIcropInfoPastAdapter
        Inherits Global.System.ComponentModel.Component

        ''' <summary>
        ''' カレンダーiCROP情報の退避
        ''' </summary>
        ''' <returns>処理件数</returns>
        ''' <remarks></remarks>
        Public Function InsertIcropInfoPast() As Long

            Using query As New DBUpdateQuery("MC3040402_024")

                ' SQL生成
                Dim sql As New StringBuilder
                With sql
                    .Append("INSERT /* MC3040402_024 */ ")
                    .Append("  INTO TBL_CAL_ICROPINFO_PAST ( ")
                    .Append("       CALID ")
                    .Append("     , DLRCD ")
                    .Append("     , STRCD ")
                    .Append("     , SCHEDULEDIV ")
                    .Append("     , SCHEDULEID ")
                    .Append("     , CUSTOMERDIV ")
                    .Append("     , CUSTCODE ")
                    .Append("     , DMSID ")
                    .Append("     , CUSTNAME ")
                    .Append("     , RECEPTIONDIV ")
                    .Append("     , SERVICECODE ")
                    .Append("     , MERCHANDISECD ")
                    .Append("     , STRSTATUS ")
                    .Append("     , REZSTATUS ")
                    .Append("     , DELFLG ")
                    .Append("     , DELDATE ")
                    .Append("     , CREATEDATE ")
                    .Append("     , UPDATEDATE ")
                    .Append("     , CREATEACCOUNT ")
                    .Append("     , UPDATEACCOUNT ")
                    .Append("     , CREATEID ")
                    .Append("     , UPDATEID ")
                    .Append(") ")
                    .Append("SELECT IIF.CALID ")
                    .Append("     , IIF.DLRCD ")
                    .Append("     , IIF.STRCD ")
                    .Append("     , IIF.SCHEDULEDIV ")
                    .Append("     , IIF.SCHEDULEID ")
                    .Append("     , IIF.CUSTOMERDIV ")
                    .Append("     , IIF.CUSTCODE ")
                    .Append("     , IIF.DMSID ")
                    .Append("     , IIF.CUSTNAME ")
                    .Append("     , IIF.RECEPTIONDIV ")
                    .Append("     , IIF.SERVICECODE ")
                    .Append("     , IIF.MERCHANDISECD ")
                    .Append("     , IIF.STRSTATUS ")
                    .Append("     , IIF.REZSTATUS ")
                    .Append("     , IIF.DELFLG ")
                    .Append("     , IIF.DELDATE ")
                    .Append("     , IIF.CREATEDATE ")
                    .Append("     , IIF.UPDATEDATE ")
                    .Append("     , IIF.CREATEACCOUNT ")
                    .Append("     , IIF.UPDATEACCOUNT ")
                    .Append("     , IIF.CREATEID ")
                    .Append("     , IIF.UPDATEID ")
                    .Append("  FROM TBL_CAL_ICROPINFO IIF ")
                    .Append(" WHERE EXISTS ( ")
                    .Append("    SELECT 1 ")
                    .Append("      FROM TBL_CAL_WK_TODO_PAST WPT ")
                    .Append("     WHERE IIF.CALID = WPT.CALID ")
                    .Append("       AND WPT.TODOFLG = '0') ")
                End With

                query.CommandText = sql.ToString()

                ' SQL実行(戻り値返却)
                Return query.Execute()

            End Using

        End Function

    End Class

    ''' <summary>
    ''' カレンダーiCROP情報テーブル処理
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CalIcropInfoAdapter
        Inherits Global.System.ComponentModel.Component

        ''' <summary>
        ''' カレンダーiCROP情報より退避データの削除
        ''' </summary>
        ''' <returns>処理件数</returns>
        ''' <remarks></remarks>
        Public Function DeleteIcropInfo() As Long

            Using query As New DBUpdateQuery("MC3040402_025")

                ' SQL生成
                Dim sql As New StringBuilder
                With sql
                    .Append("DELETE /* MC3040402_025 */ ")
                    .Append("  FROM TBL_CAL_ICROPINFO IIF ")
                    .Append(" WHERE EXISTS ( ")
                    .Append("    SELECT 1 ")
                    .Append("      FROM TBL_CAL_WK_TODO_PAST WPT ")
                    .Append("     WHERE IIF.CALID = WPT.CALID ")
                    .Append("       AND WPT.TODOFLG = '0') ")
                End With

                query.CommandText = sql.ToString()

                ' SQL実行(戻り値返却)
                Return query.Execute()

            End Using

        End Function

    End Class

End Namespace

Partial Class MC3040402DataSet
End Class
