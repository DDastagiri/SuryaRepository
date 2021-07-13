Imports System.Text
Imports Toyota.eCRB.SystemFrameworks.Configuration
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Oracle.DataAccess.Client

Namespace IC3040404.Api.DataAccess

    ''' <summary>
    ''' CalDAVのDB操作のためのクラス
    ''' </summary>
    ''' <remarks></remarks>
    Public Class IC3040404DataTable
        Inherits Global.System.ComponentModel.Component

        Private Const ORACLE_EXCEPTION As Integer = 9000
        Private Const ORACLE_EXCEPTION_EX As Integer = 9100

        ''' <summary>
        ''' TBL_CAL_EVENTITEMから対象データセットを取得する
        ''' </summary>
        ''' <param name="StaffCd">アカウントID</param>
        ''' <param name="StartDate">対象の開始日</param>
        ''' <param name="EndDate">対象の終了日</param>
        ''' <param name="OpeCd">対象のOPERATION CODE</param>
        ''' <param name="Kind">カレンダー=0 参照=1</param>
        ''' <returns>取得したデータセット</returns>
        ''' <remarks>
        ''' カレンダーのイベントから、対象となるデータを取得する
        ''' 条件は、スタッフコード、対象の期間（開始日、終了日）である
        ''' </remarks>
        Public Function GetEventItem(staffCD As String, startDate As DateTime, endDate As DateTime, _
                                     opeCD As String, kind As Integer) As IC3040404DataSet.TableDataTableDataTable

            Using Query As New DBSelectQuery(Of IC3040404DataSet.TableDataTableDataTable)("IC3040404_001")

                '結果のデータセット
                Dim DataTable As IC3040404DataSet.TableDataTableDataTable = Nothing
                'Dim NowDate As DateTime = Today() '今日の 00:00:00

                Try
                    'SQLを作成
                    Dim Sql As New StringBuilder
                    With Sql
                        '対象のデータを取得
                        .Append(" SELECT  /* IC3040404_001 */ ")
                        .Append("	     A.EVENTID          EVENTID")
                        .Append("	   , A.CALID            CALID          ")
                        .Append("	   , A.TODOID           TODOID         ")
                        .Append("	   , A.UNIQUEID         UNIQUEID       ")
                        .Append("	   , A.RECURRENCEID     RECURRENCEID   ")
                        .Append("	   , A.CHGSEQNO         CHGSEQNO       ")
                        .Append("	   , A.ACTSTAFFSTRCD    ACTSTAFFSTRCD  ")
                        .Append("	   , A.ACTSTAFFCD       ACTSTAFFCD     ")
                        .Append("	   , A.RECSTAFFSTRCD    RECSTAFFSTRCD  ")
                        .Append("	   , A.RECSTAFFCD       RECSTAFFCD     ")
                        .Append("	   , A.CONTACTNO        CONTACTNO      ")
                        .Append("	   , A.SUMMARY          SUMMARY        ")
                        .Append("	   , A.STARTTIME        STARTTIME      ")
                        .Append("	   , A.ENDTIME          ENDTIME        ")
                        .Append("	   , A.TIMEFLG          TIMEFLG        ")
                        .Append("	   , A.ALLDAYFLG        ALLDAYFLG      ")
                        .Append("	   , A.MEMO             MEMO           ")
                        .Append("	   , A.ICROPCOLOR       ICROPCOLOR     ")
                        .Append("	   , A.RRULEFLG         RRULEFLG       ")
                        .Append("	   , A.RRULE_FREQ       RRULE_FREQ     ")
                        .Append("	   , A.RRULE_INTERVAL   RRULE_INTERVAL ")
                        .Append("	   , A.RRULE_UNTIL      RRULE_UNTIL    ")
                        .Append("	   , A.RRULE_TEXT       RRULE_TEXT     ")
                        .Append("	   , A.LOCATION         LOCATION       ")
                        .Append("	   , A.ATTENDEE         ATTENDEE       ")
                        .Append("	   , A.TRANSP           TRANSP         ")
                        .Append("	   , A.URL              URL            ")

                        .Append("	   , A.CREATEDATE       CREATEDATE     ")
                        .Append("	   , A.UPDATEDATE       UPDATEDATE     ")
                        .Append("	   , A.CREATEACCOUNT    CREATEACCOUNT  ")
                        .Append("	   , A.UPDATEACCOUNT    UPDATEACCOUNT  ")
                        .Append("	   , A.CREATEID         CREATEID       ")
                        .Append("	   , A.UPDATEID         UPDATEID       ")

                        .Append("      , C.SCHEDULEDIV      SCHEDULEDIV    ") 'Add 
                        '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 START
                        .Append("      , A.PROCESSDIV       PROCESSDIV     ")
                        '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 END

                        .Append("   FROM TBL_CAL_EVENTITEM  A")
                        .Append("      , TBL_CAL_ICROPINFO  C")
                        '.Append("      , TBL_CAL_TODOITEM   D")    '機能削除　2011/12/22
                        .Append("   WHERE A.CALID   = C.CALID(+)")
                        '.Append("    AND A.TODOID  = D.TODOID(+)") '機能削除　2011/12/22

                        '対象のデータを取得する
                        If String.Equals(opeCD, "8") Then
                            .Append("  AND ( A.ACTSTAFFCD = :STAFF )")
                        ElseIf String.Equals(opeCD, "9") Then ' ope = "9" Then
                            .Append("  AND ( A.RECSTAFFCD = :STAFF )")
                        Else 'マッチングさせない
                            .Append("  AND ( 'XXXXXX@XXXXX' = :STAFF )")
                        End If

                        .Append("    AND (( :EDDATE BETWEEN A.STARTTIME AND A.ENDTIME     )")
                        .Append("	  OR  ( :STDATE BETWEEN A.STARTTIME AND A.ENDTIME     )")
                        .Append("	  OR  ( :STDATE <= A.STARTTIME AND A.ENDTIME <= :EDDATE ))")
                        .Append("    AND A.DELFLG = '0' ")

                        'ネィティブは表示、icropは未完＋当日以降の完了を表示
                        ' 　→　2011/12/22 変更で全部出力になりました
                        '.Append("    AND (A.CALID = 'NATIVE' ") 'ネィティブは表示
                        '.Append("         OR ( D.DELFLG = '0' ")    'icropは未完了または完了で当日以降は表示
                        '.Append("             AND (   D.COMPLETIONFLG = '0' ")
                        '.Append("               OR  ( D.COMPLETIONFLG = '1' AND D.COMPLETIONDATE >= :TODAY )")
                        '.Append("                 )")
                        '.Append("            )")
                        '.Append("        )")

                        'iCal表示条件
                        'オペコード8（SC：活動スタッフ）はNATIVEとSCHEDULEDIV = '0'（入庫）を表示
                        'オペコード9（SA：サービスｽﾀｯﾌ）はNATIVEのみ表示
                        If kind = 0 Then    'iCal(カレンダー)
                            If String.Equals(opeCD, "8") Then   'nativeの場合はSCHEDULEDIVがなくてよい
                                ' SCHEDULEDIVが逆だったので修正　2011/12/14  1->0
                                '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 START
                                '.Append(" AND (A.CALID = 'NATIVE' OR C.SCHEDULEDIV = '0')")
                                '2012/03/27 SKFC 加藤 【SALES_2】受注後工程(不具合対応) START
                                '.Append(" AND (A.CALID = 'NATIVE' OR ( C.SCHEDULEDIV = '0' OR C.SCHEDULEDIV = '1' ))")        '0:来店 or 2:受注
                                .Append(" AND (A.CALID = 'NATIVE' OR ( C.SCHEDULEDIV = '0' OR C.SCHEDULEDIV = '2' ))")        '0:来店 or 2:受注
                                '2012/03/27 SKFC 加藤 【SALES_2】受注後工程(不具合対応) END
                                '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 END

                            ElseIf String.Equals(opeCD, "9") Then
                                'NATIVEは対象
                                .Append(" AND (A.CALID = 'NATIVE')")
                            End If
                        Else    '照会の表示条件　SCHEDULEDIV = '1'（入庫）
                            ' SCHEDULEDIVが逆だったので修正　2011/12/14  0->1
                            .Append(" AND (C.SCHEDULEDIV = '1')")
                        End If

                        .Append(" ORDER BY A.UNIQUEID, A.RECURRENCEID")
                    End With

                    'コマンド生成
                    Query.CommandText = Sql.ToString()

                    'SQLパラメータ設定
                    Query.AddParameterWithTypeValue("STAFF", OracleDbType.Char, staffCD)
                    Query.AddParameterWithTypeValue("STDATE", OracleDbType.Date, startDate)
                    Query.AddParameterWithTypeValue("EDDATE", OracleDbType.Date, endDate)
                    'query.AddParameterWithTypeValue("TODAY", OracleDbType.Date, nowDate)

                    'SQL実行（結果表を返却）
                    DataTable = Query.GetData()

                Catch ex As OracleException
                    Logger.Error("OracleException  : IC30404dataSet: GetEventItem:")
                    Throw New ApplicationException(ORACLE_EXCEPTION)

                Catch ex As OracleExceptionEx
                    Logger.Error("OracleExceptionEx: IC30404dataSet: GetEventItem:")
                    Throw New ApplicationException(ORACLE_EXCEPTION_EX)

                End Try

                Return DataTable

            End Using

        End Function


        ''' <summary>
        ''' 日付のヌル対応
        ''' </summary>
        ''' <param name="inpDate">チェックしたい日付</param>
        ''' <returns></returns>
        ''' <remarks>
        ''' Datetime型に定数DATE_NOTHING_VALUEを代入すると
        ''' Nothingオブジェクトを返す
        ''' </remarks>
        Private Function DateObject(InpDate As DateTime) As Object
            'DATETIMEにNothingを設定すると 0001/1/1 0:0:00になるので回避策 0001/1/1 1:1:1
            Const DateNothingValue As DateTime = #1:01:01 AM#             '0001/01/01 01:01:01

            Dim AnyObject As Object
            If InpDate = DateNothingValue Then
                AnyObject = Nothing
            Else
                AnyObject = InpDate
            End If
            Return AnyObject

        End Function

        ''' <summary>
        ''' 同名関数のOverload
        ''' </summary>
        ''' <param name="UniqueId">検索したいuid</param>
        ''' <param name="recur">リカレンスID</param>
        ''' <returns></returns>
        ''' <remarks>
        ''' 検索キーがuid
        ''' UNIQUE idで要求がくるので、TBL_CAL_ICROPINFOや
        ''' TBL_CAL_TODOITEMのjoinは行わない（該当のみ来るので必要がない）
        ''' </remarks>
        Public Function SelectEventItem(uniqueId As String, Optional recur As String = "") As IC3040404DataSet.TableDataTableDataTable

            Using Query As New DBSelectQuery(Of IC3040404DataSet.TableDataTableDataTable)("IC3040404_002")

                '結果のデータセット
                Dim DataTable As IC3040404DataSet.TableDataTableDataTable = Nothing

                Try
                    'SQLを作成
                    Dim Sql As New StringBuilder
                    With Sql
                        '対象のデータを取得
                        .Append(" SELECT /* IC3040404_002 */ ")
                        .Append("	     EVENTID        ")
                        .Append("	   , CALID          ")
                        .Append("	   , TODOID         ")
                        .Append("	   , UNIQUEID       ")
                        .Append("	   , RECURRENCEID   ")
                        .Append("	   , CHGSEQNO       ")
                        .Append("	   , ACTSTAFFSTRCD  ")
                        .Append("	   , ACTSTAFFCD     ")
                        .Append("	   , RECSTAFFSTRCD  ")
                        .Append("	   , RECSTAFFCD     ")
                        .Append("	   , CONTACTNO      ")
                        .Append("	   , SUMMARY        ")
                        .Append("	   , STARTTIME      ")
                        .Append("	   , ENDTIME        ")
                        .Append("	   , TIMEFLG        ")
                        .Append("	   , ALLDAYFLG      ")
                        .Append("	   , MEMO           ")
                        .Append("	   , ICROPCOLOR     ")
                        .Append("	   , RRULEFLG       ")
                        .Append("	   , RRULE_FREQ     ")
                        .Append("	   , RRULE_INTERVAL ")
                        .Append("	   , RRULE_UNTIL    ")
                        .Append("	   , RRULE_TEXT     ")
                        .Append("	   , LOCATION       ")
                        .Append("	   , ATTENDEE       ")
                        .Append("	   , TRANSP         ")
                        .Append("	   , URL            ")
                        .Append("	   , CREATEDATE     ")
                        .Append("	   , UPDATEDATE     ")
                        .Append("	   , CREATEACCOUNT  ")
                        .Append("	   , UPDATEACCOUNT  ")
                        .Append("	   , CREATEID       ")
                        .Append("	   , UPDATEID       ")
                        '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 START
                        .Append("      , PROCESSDIV     ")
                        '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 END


                        'アラームテーブルを連結しないように変更 2011/12/20
                        .Append("   FROM TBL_CAL_EVENTITEM")
                        .Append("  WHERE UNIQUEID = :UNIQUEID")
                        If Not String.IsNullOrEmpty(recur) Then
                            .Append("   AND  RECURRENCEID =  :RECURRENCEID")
                        End If

                        .Append("   AND DELFLG = '0' ")

                    End With

                    'コマンド生成
                    Query.CommandText = Sql.ToString()

                    'SQLパラメータ設定
                    Query.AddParameterWithTypeValue("UNIQUEID", OracleDbType.Varchar2, uniqueId)
                    If Not String.IsNullOrEmpty(recur) Then
                        Query.AddParameterWithTypeValue("RECURRENCEID", OracleDbType.Varchar2, recur)
                    End If

                    'SQL実行（結果表を返却）
                    DataTable = Query.GetData()

                Catch ex As OracleException
                    Logger.Error("OracleException  : IC30404dataSet: SelectEventItem(ov):")
                    Throw New ApplicationException(ORACLE_EXCEPTION)

                Catch ex As OracleExceptionEx
                    Logger.Error("OracleExceptionEx: IC30404dataSet: SelectEventItem(ov):")
                    Throw New ApplicationException(ORACLE_EXCEPTION_EX)

                End Try

                Return DataTable

            End Using
        End Function


        ''' <summary>
        ''' EventItemテーブルの内容を更新する
        ''' </summary>
        ''' <param name="Argument"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' 現状は、icropcolorを更新しない仕様に変更
        ''' </remarks>
        Public Function UpdateEventItem(argument As CalEventItem) As Integer

            Using Query As New DBUpdateQuery("IC3040404_003")
                Dim Sql As New StringBuilder

                Dim Count As Integer = 0 '適用件数
                Try
                    With Sql
                        .Append(" UPDATE /* IC3040404_003 */ ")
                        .Append("        TBL_CAL_EVENTITEM")
                        .Append("    SET RECURRENCEID   = :RECURRENCEID")
                        .Append(" 	   , SUMMARY        = :SUMMARY")
                        .Append(" 	   , STARTTIME      = :STARTTIME")
                        .Append(" 	   , ENDTIME        = :ENDTIME")
                        .Append(" 	   , TIMEFLG        = :TIMEFLG")
                        .Append(" 	   , ALLDAYFLG      = :ALLDAYFLG")
                        .Append(" 	   , MEMO           = :MEMO")
                        .Append(" 	   , RRULEFLG       = :RRULEFLG")
                        .Append(" 	   , RRULE_FREQ     = :RRULE_FREQ")
                        .Append(" 	   , RRULE_INTERVAL = :RRULE_INTERVAL")
                        .Append(" 	   , RRULE_UNTIL    = :RRULE_UNTIL")
                        .Append(" 	   , RRULE_TEXT     = :RRULE_TEXT")
                        .Append(" 	   , LOCATION       = :LOCATION")
                        .Append(" 	   , ATTENDEE       = :ATTENDEE")
                        .Append(" 	   , TRANSP         = :TRANSP")
                        .Append(" 	   , URL            = :URL")
                        .Append(" 	   , UPDATEDATE     = :UPDATEDATE")
                        .Append(" 	   , UPDATEACCOUNT  = :UPDATEACCOUNT")
                        .Append(" 	   , UPDATEID       = :UPDATEID")
                        .Append("  WHERE UNIQUEID       = :UNIQUEID")
                        .Append("    AND RECURRENCEID   = :RECURRENCEID")    'Add 2011/12/18
                    End With

                    Query.CommandText = Sql.ToString()

                    'SQLパラメータ設定
                    With argument
                        Query.AddParameterWithTypeValue("RECURRENCEID", OracleDbType.Varchar2, .RecurrenceId)
                        Query.AddParameterWithTypeValue("SUMMARY", OracleDbType.NVarchar2, .Summary)
                        Query.AddParameterWithTypeValue("STARTTIME", OracleDbType.Date, DateObject(.StartTime))
                        Query.AddParameterWithTypeValue("ENDTIME", OracleDbType.Date, DateObject(.EndTime))
                        Query.AddParameterWithTypeValue("TIMEFLG", OracleDbType.Char, .TimeFlg)
                        Query.AddParameterWithTypeValue("ALLDAYFLG", OracleDbType.Char, .AlldayFlg)
                        Query.AddParameterWithTypeValue("MEMO", OracleDbType.NVarchar2, .Memo)
                        Query.AddParameterWithTypeValue("RRULEFLG", OracleDbType.Char, .RruleFlg)
                        Query.AddParameterWithTypeValue("RRULE_FREQ", OracleDbType.Varchar2, .RruleFreq)
                        'NULLがあるのでVarchar2
                        Query.AddParameterWithTypeValue("RRULE_INTERVAL", OracleDbType.Varchar2, .RruleInterval)
                        Query.AddParameterWithTypeValue("RRULE_UNTIL", OracleDbType.Date, DateObject(.RruleUntil))
                        Query.AddParameterWithTypeValue("RRULE_TEXT", OracleDbType.Varchar2, .RruleText)
                        Query.AddParameterWithTypeValue("LOCATION", OracleDbType.NVarchar2, .Location)
                        Query.AddParameterWithTypeValue("ATTENDEE", OracleDbType.NVarchar2, .Attendee)
                        Query.AddParameterWithTypeValue("TRANSP", OracleDbType.Varchar2, .Transp)
                        Query.AddParameterWithTypeValue("URL", OracleDbType.NVarchar2, .Url)
                        Query.AddParameterWithTypeValue("UPDATEDATE", OracleDbType.Date, DateObject(.UpdateDate))
                        Query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, .UpdateAccount)
                        Query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, .UpdateId)
                        Query.AddParameterWithTypeValue("UNIQUEID", OracleDbType.Varchar2, .UniqueId)
                    End With

                    'SQL実行（処理件数を返却）
                    Count = Query.Execute()

                Catch ex As OracleException
                    Logger.Error("OracleException  : IC30404dataSet: UpdateEventItem:")
                    Throw New ApplicationException(ORACLE_EXCEPTION)

                Catch ex As OracleExceptionEx
                    Logger.Error("OracleExceptionEx: IC30404dataSet: UpdateEventItem:")
                    Throw New ApplicationException(ORACLE_EXCEPTION_EX)

                End Try

                Return Count

            End Using

        End Function


        ''' <summary>
        ''' EventItemテーブルに1件データ追加する
        ''' </summary>
        ''' <param name="Argument"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' EVENTIDはシーケンスで自動生成する
        ''' </remarks>
        ''' <history>2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 START</history>
        Public Function InsertEventItem(argument As CalEventItem) As Integer

            Using Query As New DBUpdateQuery("IC3040404_004")
                Dim Sql As New StringBuilder

                Dim Count As Integer = 0
                Try

                    With Sql
                        .Append(" INSERT /* IC3040404_004 */ ")
                        .Append("   INTO TBL_CAL_EVENTITEM")
                        .Append("      ( EVENTID        ")
                        .Append("	   , CALID          ")
                        .Append("	   , TODOID         ")
                        .Append("	   , UNIQUEID       ")
                        .Append("	   , RECURRENCEID   ")
                        .Append("	   , CHGSEQNO       ")
                        .Append("	   , ACTSTAFFSTRCD  ")
                        .Append("	   , ACTSTAFFCD     ")
                        .Append("	   , RECSTAFFSTRCD  ")
                        .Append("	   , RECSTAFFCD     ")
                        .Append("	   , CONTACTNO      ")
                        .Append("	   , SUMMARY        ")
                        .Append("	   , STARTTIME      ")
                        .Append("	   , ENDTIME        ")
                        .Append("	   , TIMEFLG        ")
                        .Append("	   , ALLDAYFLG      ")
                        .Append("	   , MEMO           ")
                        .Append("	   , ICROPCOLOR     ")
                        .Append("	   , RRULEFLG       ")
                        .Append("	   , RRULE_FREQ     ")
                        .Append("	   , RRULE_INTERVAL ")
                        .Append("	   , RRULE_UNTIL    ")
                        .Append("	   , RRULE_TEXT     ")
                        .Append("	   , LOCATION       ")
                        .Append("	   , ATTENDEE       ")
                        .Append("	   , TRANSP         ")
                        .Append("	   , URL            ")
                        .Append("	   , DELFLG         ")
                        .Append("	   , DELDATE        ")
                        .Append("	   , CREATEDATE     ")
                        .Append("	   , UPDATEDATE     ")
                        .Append("	   , CREATEACCOUNT  ")
                        .Append("	   , UPDATEACCOUNT  ")
                        .Append("	   , CREATEID       ")
                        .Append("	   , UPDATEID       ")
                        '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 START
                        .Append("      , PROCESSDIV     ")
                        '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 END
                        .Append("      )")
                        .Append(" VALUES")
                        .Append("      ( LPAD(TO_CHAR(SEQ_CAL_EVENTITEM_EVENTID.NEXTVAL), 20, '0')")
                        .Append("	   , :CALID          ")
                        .Append("	   , :TODOID         ")
                        .Append("	   , :UNIQUEID       ")
                        .Append("	   , :RECURRENCEID   ")
                        .Append("	   , :CHGSEQNO       ")
                        .Append("	   , :ACTSTAFFSTRCD  ")
                        .Append("	   , :ACTSTAFFCD     ")
                        .Append("	   , :RECSTAFFSTRCD  ")
                        .Append("	   , :RECSTAFFCD     ")
                        .Append("	   , :CONTACTNO      ")
                        .Append("	   , :SUMMARY        ")
                        .Append("	   , :STARTTIME      ")
                        .Append("	   , :ENDTIME        ")
                        .Append("	   , :TIMEFLG        ")
                        .Append("	   , :ALLDAYFLG      ")
                        .Append("	   , :MEMO           ")
                        .Append("	   , :ICROPCOLOR     ")
                        .Append("	   , :RRULEFLG       ")
                        .Append("	   , :RRULE_FREQ     ")
                        .Append("	   , :RRULE_INTERVAL ")
                        .Append("	   , :RRULE_UNTIL    ")
                        .Append("	   , :RRULE_TEXT     ")
                        .Append("	   , :LOCATION       ")
                        .Append("	   , :ATTENDEE       ")
                        .Append("	   , :TRANSP         ")
                        .Append("	   , :URL            ")
                        .Append("	   , :DELFLG         ")
                        .Append("	   , :DELDATE        ")
                        .Append("	   , :CREATEDATE     ")
                        .Append("	   , :UPDATEDATE     ")
                        .Append("	   , :CREATEACCOUNT  ")
                        .Append("	   , :UPDATEACCOUNT  ")
                        .Append("	   , :CREATEID       ")
                        .Append("	   , :UPDATEID       ")
                        '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 START
                        '2012/03/23 SKFC 加藤 【SALES_2】受注後工程(不具合対応) START
                        '.Append("      , PROCESSDIV      ")
                        .Append("      , :PROCESSDIV      ")
                        '2012/03/23 SKFC 加藤 【SALES_2】受注後工程(不具合対応) END
                        '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 END
                        .Append("    )")
                    End With

                    Query.CommandText = Sql.ToString()

                    'SQLパラメータ設定
                    With argument
                        Query.AddParameterWithTypeValue("CALID", OracleDbType.Varchar2, .CalId)
                        Query.AddParameterWithTypeValue("TODOID", OracleDbType.Varchar2, .TodoId)
                        Query.AddParameterWithTypeValue("UNIQUEID", OracleDbType.Varchar2, .UniqueId)
                        Query.AddParameterWithTypeValue("RECURRENCEID", OracleDbType.Varchar2, .RecurrenceId)
                        'CalDAVサーバーでは使用しないのでVarchar2にする
                        Query.AddParameterWithTypeValue("CHGSEQNO", OracleDbType.Varchar2, .ChgSeqNo)
                        Query.AddParameterWithTypeValue("ACTSTAFFSTRCD", OracleDbType.Char, .ActStaffStrCD)
                        Query.AddParameterWithTypeValue("ACTSTAFFCD", OracleDbType.Varchar2, .ActStaffCD)
                        Query.AddParameterWithTypeValue("RECSTAFFSTRCD", OracleDbType.Char, .RecStaffStrCD)
                        Query.AddParameterWithTypeValue("RECSTAFFCD", OracleDbType.Varchar2, .RecStaffCD)
                        'CalDAVサーバーではCONTACTNOを使用しないのでVarchar2にする
                        Query.AddParameterWithTypeValue("CONTACTNO", OracleDbType.Varchar2, .ContactNo)
                        Query.AddParameterWithTypeValue("SUMMARY", OracleDbType.NVarchar2, .Summary)
                        Query.AddParameterWithTypeValue("STARTTIME", OracleDbType.Date, DateObject(.StartTime))
                        Query.AddParameterWithTypeValue("ENDTIME", OracleDbType.Date, DateObject(.EndTime))
                        Query.AddParameterWithTypeValue("TIMEFLG", OracleDbType.Char, .TimeFlg)
                        Query.AddParameterWithTypeValue("ALLDAYFLG", OracleDbType.Char, .AlldayFlg)
                        Query.AddParameterWithTypeValue("MEMO", OracleDbType.NVarchar2, .Memo)
                        Query.AddParameterWithTypeValue("ICROPCOLOR", OracleDbType.Varchar2, .IcropColor)
                        Query.AddParameterWithTypeValue("RRULEFLG", OracleDbType.Char, .RruleFlg)
                        Query.AddParameterWithTypeValue("RRULE_FREQ", OracleDbType.Varchar2, .RruleFreq)
                        'データがないときNULLにしたいのでVarchar2 
                        Query.AddParameterWithTypeValue("RRULE_INTERVAL", OracleDbType.Varchar2, .RruleInterval)
                        Query.AddParameterWithTypeValue("RRULE_UNTIL", OracleDbType.Date, DateObject(.RruleUntil))
                        Query.AddParameterWithTypeValue("RRULE_TEXT", OracleDbType.Varchar2, .RruleText)
                        Query.AddParameterWithTypeValue("LOCATION", OracleDbType.NVarchar2, .Location)
                        Query.AddParameterWithTypeValue("ATTENDEE", OracleDbType.NVarchar2, .Attendee)
                        Query.AddParameterWithTypeValue("TRANSP", OracleDbType.Varchar2, .Transp)
                        Query.AddParameterWithTypeValue("URL", OracleDbType.NVarchar2, .Url)
                        Query.AddParameterWithTypeValue("DELFLG", OracleDbType.Char, .DelFlg)
                        Query.AddParameterWithTypeValue("DELDATE", OracleDbType.Date, DateObject(.DelDate))
                        Query.AddParameterWithTypeValue("CREATEDATE", OracleDbType.Date, DateObject(.CreateDate))
                        Query.AddParameterWithTypeValue("UPDATEDATE", OracleDbType.Date, DateObject(.UpdateDate))
                        Query.AddParameterWithTypeValue("CREATEACCOUNT", OracleDbType.Varchar2, .CreateAccount)
                        Query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, .UpdateAccount)
                        Query.AddParameterWithTypeValue("CREATEID", OracleDbType.Varchar2, .CreateId)
                        Query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, .UpdateId)
                        '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 START
                        '----- '2012/03/23 SKFC 加藤 START
                        'Query.AddParameterWithTypeValue("PROCESSDIV", OracleDbType.Varchar2, .UpdateId)
                        Query.AddParameterWithTypeValue("PROCESSDIV", OracleDbType.Char, .ProcessDiv)
                        '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 END
                        '----- '2012/03/23 SKFC 加藤 END
                    End With
                    'SQL実行（適用件数）
                    Count = Query.Execute()

                Catch ex As OracleException
                    Logger.Error("OracleException  : IC30404dataSet: InsertEventItem:")
                    Throw New ApplicationException(ORACLE_EXCEPTION)

                Catch ex As OracleExceptionEx
                    Logger.Error("OracleExceptionEx: IC30404dataSet: InsertEventItem:")
                    Throw New ApplicationException(ORACLE_EXCEPTION_EX)

                End Try

                Return Count

            End Using
        End Function


        ''' <summary>
        ''' アラームデータを物理削除
        ''' </summary>
        ''' <param name="Argument">アラームデータの構造体（クラス）</param>
        ''' <returns>適用件数</returns>
        ''' <remarks>
        ''' アラームデータは再編成する必要があるので
        ''' 更新時はいったん削除する SEQ1およびSEQ2双方
        ''' </remarks>
        Public Function DeleteEventAlarm(argument As CalEventAlarm) As Integer
            Using Query As New DBUpdateQuery("IC3040404_005")
                Dim Sql As New StringBuilder
                Dim Count As Integer = 0

                Try
                    With Sql
                        .Append(" DELETE  /* IC3040404_005 */ ")
                        .Append("   FROM TBL_CAL_EVENTALARM")
                        .Append("  WHERE EVENTID = :EVENTID")
                    End With

                    Query.CommandText = Sql.ToString()

                    'SQLパラメータ設定
                    With argument
                        Query.AddParameterWithTypeValue("EVENTID", OracleDbType.Varchar2, .EventId)
                    End With

                    'SQL実行（適用データ数を返却）
                    Count = Query.Execute()

                Catch ex As OracleException
                    Logger.Error("OracleException  : IC30404dataSet: DeleteEventAlarm:")
                    Throw New ApplicationException(ORACLE_EXCEPTION)

                Catch ex As OracleExceptionEx
                    Logger.Error("OracleExceptionEx: IC30404dataSet: DeleteEventAlarm:")
                    Throw New ApplicationException(ORACLE_EXCEPTION_EX)

                End Try

                Return Count
            End Using

        End Function

        ''' <summary>
        ''' アラームデータを挿入
        ''' </summary>
        ''' <param name="Argument">アラームデータの構造体（クラス）</param>
        ''' <returns>適用件数</returns>
        ''' <remarks></remarks>
        Public Function InsertEventAlarm(argument As CalEventAlarm) As Integer

            Using Query As New DBUpdateQuery("IC3040404_006")
                Dim Sql As New StringBuilder
                Dim Count As Integer = 0
                Try
                    With Sql
                        .Append(" INSERT /* IC3040404_006 */ ")
                        .Append("   INTO TBL_CAL_EVENTALARM")
                        .Append("      ( EVENTID      ")
                        .Append("      , SEQNO        ")
                        .Append("      , STARTTRIGGER ")
                        .Append("      , CREATEDATE   ")
                        .Append("      , UPDATEDATE   ")
                        .Append("      , CREATEACCOUNT")
                        .Append("      , UPDATEACCOUNT")
                        .Append("      , CREATEID     ")
                        .Append("      , UPDATEID     ")
                        .Append("      )")

                        .Append(" VALUES ")
                        .Append("      ( :EVENTID      ")
                        .Append("      , :SEQNO        ")
                        .Append("      , :STARTTRIGGER ")
                        .Append("      , :CREATEDATE   ")
                        .Append("      , :UPDATEDATE   ")
                        .Append("      , :CREATEACCOUNT")
                        .Append("      , :UPDATEACCOUNT")
                        .Append("      , :CREATEID     ")
                        .Append("      , :UPDATEID     ")
                        .Append("      )")
                    End With

                    Query.CommandText = Sql.ToString()

                    'SQLパラメータ設定
                    With argument
                        Query.AddParameterWithTypeValue("EVENTID", OracleDbType.Varchar2, .EventId)
                        Query.AddParameterWithTypeValue("SEQNO", OracleDbType.Int32, .SeqNo)
                        Query.AddParameterWithTypeValue("STARTTRIGGER", OracleDbType.Char, .StartTrigger)
                        Query.AddParameterWithTypeValue("CREATEDATE", OracleDbType.Date, DateObject(.CreateDate))
                        Query.AddParameterWithTypeValue("UPDATEDATE", OracleDbType.Date, DateObject(.UpdateDate))
                        Query.AddParameterWithTypeValue("CREATEACCOUNT", OracleDbType.Varchar2, .CreateAccount)
                        Query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, .UpdateAccount)
                        Query.AddParameterWithTypeValue("CREATEID", OracleDbType.Varchar2, .CreateId)
                        Query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, .UpdateId)
                    End With

                    'SQL実行（適用データ数を返却）
                    Count = Query.Execute()

                Catch ex As OracleException
                    Logger.Error("OracleException  : IC30404dataSet: InsertEventAlarm:")
                    Throw New ApplicationException(ORACLE_EXCEPTION)

                Catch ex As OracleExceptionEx
                    Logger.Error("OracleExceptionEx: IC30404dataSet: InsertEventAlarm:")
                    Throw New ApplicationException(ORACLE_EXCEPTION_EX)

                End Try

                Return Count
            End Using
        End Function


        ''' <summary>
        ''' カレンダデータを論理削除
        ''' </summary>
        ''' <param name="Argument"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' リカレンスIDがスペースでない場合" "はりかれんすIDも削除キーとする
        ''' 2011/12/18 変更
        ''' </remarks>
        Public Function DeleteEventItem(argument As CalEventItem, Optional rec As Boolean = False) As Integer

            Using Query As New DBUpdateQuery("IC3040404_007")
                Dim Sql As New StringBuilder
                Dim Count As Integer = 0
                Try
                    With Sql
                        '復活したい場合もあるので、DELFLG=1にはしない
                        ' Reccurenceのデータも削除する
                        .Append(" UPDATE /* IC3040404_007 */ ")
                        .Append("        TBL_CAL_EVENTITEM   ")
                        .Append("    SET DELFLG        = :DELFLG")
                        .Append("      , DELDATE       = :DELDATE")
                        .Append("      , UPDATEDATE    = :UPDATEDATE")
                        .Append("      , UPDATEACCOUNT = :UPDATEACCOUNT")
                        .Append("      , UPDATEID      = :UPDATEID")
                        .Append("  WHERE UNIQUEID      = :UNIQUEID")
                        'リカレンスIDのある場合
                        'If arg.RECURRENCEID.Trim <> "" Then
                        If rec Then
                            .Append(" AND RECURRENCEID  <> ' ' ")
                        End If
                        'End If
                    End With

                    Query.CommandText = Sql.ToString()

                    'SQLパラメータ設定
                    With argument
                        Query.AddParameterWithTypeValue("DELFLG", OracleDbType.Char, .DelFlg)
                        Query.AddParameterWithTypeValue("DELDATE", OracleDbType.Date, .DelDate)
                        Query.AddParameterWithTypeValue("UPDATEDATE", OracleDbType.Date, .UpdateDate)
                        Query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, .UpdateAccount)
                        Query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, .UpdateId)
                        Query.AddParameterWithTypeValue("UNIQUEID", OracleDbType.Varchar2, .UniqueId)
                        'If arg.RECURRENCEID.Trim <> "" Then
                        '    query.AddParameterWithTypeValue("RECURRENCEID", OracleDbType.Varchar2, .RECURRENCEID)
                        'End If
                    End With

                    'SQL実行（結果数を返却）
                    Count = Query.Execute()

                Catch ex As OracleException
                    Logger.Error("OracleException  : IC30404dataSet: DeleteEventItem:")
                    Throw New ApplicationException(ORACLE_EXCEPTION)

                Catch ex As OracleExceptionEx
                    Logger.Error("OracleExceptionEx: IC30404dataSet: DeleteEventItem:")
                    Throw New ApplicationException(ORACLE_EXCEPTION_EX)

                End Try

                Return Count
            End Using
        End Function


        ''' <summary>
        ''' TBL_CAL_CARD_LASTMODIFYのInsert
        ''' </summary>
        ''' <param name="Argument"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function InsertCalCardLastModify(argument As CalCardLastModify) As Integer

            Using Query As New DBUpdateQuery("IC3040404_008")
                Dim Sql As New StringBuilder
                Dim Count As Integer = 0
                Try

                    With Sql
                        .Append(" INSERT /* IC3040404_008 */ ")
                        .Append("   INTO TBL_CAL_CARD_LASTMODIFY ")
                        .Append("      ( STAFFCD       ")
                        .Append("      , CALUPDATEDATE ")
                        .Append("      , CARDUPDATEDATE")
                        .Append("      , CREATEDATE    ")
                        .Append("      , UPDATEDATE    ")
                        .Append("      , CREATEACCOUNT ")
                        .Append("      , UPDATEACCOUNT ")
                        .Append("      , CREATEID      ")
                        .Append("      , UPDATEID      ")
                        .Append("      )")
                        .Append(" VALUES")
                        .Append("      ( :STAFFCD       ")
                        .Append("      , :CALUPDATEDATE ")
                        .Append("      , :CARDUPDATEDATE")
                        .Append("      , :CREATEDATE    ")
                        .Append("      , :UPDATEDATE    ")
                        .Append("      , :CREATEACCOUNT ")
                        .Append("      , :UPDATEACCOUNT ")
                        .Append("      , :CREATEID      ")
                        .Append("      , :UPDATEID      ")
                        .Append("      ) ")
                    End With

                    Query.CommandText = Sql.ToString()

                    'SQLパラメータ設定
                    With argument
                        Query.AddParameterWithTypeValue("STAFFCD", OracleDbType.Varchar2, .StaffCD)
                        Query.AddParameterWithTypeValue("CALUPDATEDATE", OracleDbType.Date, .CalUpdateDate)
                        Query.AddParameterWithTypeValue("CARDUPDATEDATE", OracleDbType.Date, .CardUpdateDate)
                        Query.AddParameterWithTypeValue("CREATEDATE", OracleDbType.Date, .CreateDate)
                        Query.AddParameterWithTypeValue("UPDATEDATE", OracleDbType.Date, .UpdateDate)
                        Query.AddParameterWithTypeValue("CREATEACCOUNT", OracleDbType.Varchar2, .CreateAccount)
                        Query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, .UpdateAccount)
                        Query.AddParameterWithTypeValue("CREATEID", OracleDbType.Varchar2, .CreateId)
                        Query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, .UpdateId)
                    End With

                    'SQL実行（結果数を返却）

                    Count = Query.Execute()
                Catch ex As OracleException
                    Logger.Error("OracleException  : IC30404dataSet: InsertCalCardLastModify:")
                    Throw New ApplicationException(ORACLE_EXCEPTION)

                Catch ex As OracleExceptionEx
                    Logger.Error("OracleExceptionEx: IC30404dataSet: InsertCalCardLastModify:")
                    Throw New ApplicationException(ORACLE_EXCEPTION_EX)

                End Try

                Return Count
            End Using
        End Function


        ''' <summary>
        ''' TBL_CAL_CARD_LASTMODIFYのUPDATE
        ''' </summary>
        ''' <param name="argument"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function UpdateCalCardLastModify(argument As CalCardLastModify) As Integer

            Using Query As New DBUpdateQuery("IC3040404_009")
                Dim Sql As New StringBuilder
                Dim Count As Integer = 0
                Try

                    With Sql
                        .Append(" UPDATE /* IC3040404_009 */     ")
                        .Append("        TBL_CAL_CARD_LASTMODIFY ")
                        .Append("    SET CALUPDATEDATE = :CALUPDATEDATE ")
                        .Append("      , UPDATEDATE    = :UPDATEDATE    ")
                        .Append("      , UPDATEACCOUNT = :UPDATEACCOUNT ")
                        .Append("      , UPDATEID      = :UPDATEID      ")
                        .Append("  WHERE STAFFCD       = :STAFFCD       ")
                    End With

                    Query.CommandText = Sql.ToString()

                    'SQLパラメータ設定
                    With argument
                        Query.AddParameterWithTypeValue("CALUPDATEDATE", OracleDbType.Date, .CalUpdateDate)
                        Query.AddParameterWithTypeValue("UPDATEDATE", OracleDbType.Date, .UpdateDate)
                        Query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, .UpdateAccount)
                        Query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, .UpdateId)
                        Query.AddParameterWithTypeValue("STAFFCD", OracleDbType.Varchar2, .StaffCD)
                    End With

                    'SQL実行（結果表を返却）

                    Count = Query.Execute()
                Catch ex As OracleException
                    Logger.Error("OracleException  : IC30404dataSet: updateCalCardLastModify:")
                    Throw New ApplicationException(ORACLE_EXCEPTION)

                Catch ex As OracleExceptionEx
                    Logger.Error("OracleExceptionEx: IC30404dataSet: updateCalCardLastModify:")
                    Throw New ApplicationException(ORACLE_EXCEPTION_EX)

                End Try
                Return Count
            End Using

        End Function


        ''' <summary>
        ''' TBL_CAL_CARD_LASTMODIFYの情報を得る
        ''' </summary>
        ''' <param name="StaffCd"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetLastModify(staffCD As String) As IC3040404DataSet.TableDataTableDataTable

            Using Query As New DBSelectQuery(Of IC3040404DataSet.TableDataTableDataTable)("IC3040404_010")

                '結果のデータセット
                Dim DataTable As IC3040404DataSet.TableDataTableDataTable = Nothing

                Try
                    'SQLを作成
                    Dim sql As New StringBuilder
                    With sql
                        '対象のデータを取得   
                        .Append(" SELECT /* IC3040404_010 */ ")
                        .Append("	     STAFFCD             ")
                        .Append("	   , CALUPDATEDATE       ")
                        .Append("	   , CARDUPDATEDATE      ")
                        .Append("	   , CREATEDATE          ")
                        .Append("	   , UPDATEDATE          ")
                        .Append("	   , CREATEACCOUNT       ")
                        .Append("	   , UPDATEACCOUNT       ")
                        .Append("	   , CREATEID            ")
                        .Append("	   , UPDATEID            ")
                        .Append("   FROM TBL_CAL_CARD_LASTMODIFY")
                        .Append("  WHERE STAFFCD = :STAFF")
                    End With

                    'コマンド生成
                    Query.CommandText = sql.ToString()

                    'SQLパラメータ設定
                    Query.AddParameterWithTypeValue("STAFF", OracleDbType.Char, staffCD)

                    'SQL実行（結果表を返却）
                    DataTable = Query.GetData()

                Catch ex As OracleException
                    Logger.Error("OracleException  : IC30404dataSet: GetLastModify:")
                    Throw New ApplicationException(ORACLE_EXCEPTION)

                Catch ex As OracleExceptionEx
                    Logger.Error("OracleExceptionEx: IC30404dataSet: GetLastModify:")
                    Throw New ApplicationException(ORACLE_EXCEPTION_EX)

                End Try

                Return DataTable
            End Using
        End Function


        ''' <summary>
        ''' 除外日を物理削除
        ''' </summary>
        ''' <param name="Argument"></param>
        ''' <returns>適用件数</returns>
        ''' <remarks>
        ''' 除外日データは再編成する必要があるので
        ''' 更新時はいったん削除する SEQ(n)全部
        ''' </remarks>
        Public Function DeleteEventExDate(argument As CalEventExDate) As Integer
            Using Query As New DBUpdateQuery("IC3040404_011")
                Dim Sql As New StringBuilder
                Dim Count As Integer = 0

                Try
                    With Sql
                        .Append(" DELETE /* IC3040404_011 */ ")
                        .Append("   FROM TBL_CAL_EVENTEXDATE")
                        .Append("  WHERE EVENTID = :EVENTID")
                    End With

                    Query.CommandText = Sql.ToString()

                    'SQLパラメータ設定
                    With argument
                        Query.AddParameterWithTypeValue("EVENTID", OracleDbType.Varchar2, .EventId)
                    End With

                    'SQL実行（適用データ数を返却）
                    Count = Query.Execute()
                Catch ex As OracleException
                    Logger.Error("OracleException  : IC30404dataSet: DeleteEventExDate:")
                    Throw New ApplicationException(ORACLE_EXCEPTION)

                Catch ex As OracleExceptionEx
                    Logger.Error("OracleExceptionEx: IC30404dataSet: DeleteEventExDate:")
                    Throw New ApplicationException(ORACLE_EXCEPTION_EX)

                End Try
                Return Count
            End Using
        End Function

        ''' <summary>
        ''' 除外日データを登録する
        ''' </summary>
        ''' <param name="Argument"></param>
        ''' <returns>適用件数</returns>
        ''' <remarks></remarks>
        Public Function InsertEventExDate(argument As CalEventExDate) As Integer

            Using Query As New DBUpdateQuery("IC3040404_012")
                Dim Sql As New StringBuilder
                Dim Count As Integer = 0
                Try

                    With Sql
                        .Append(" INSERT /* IC3040404_012 */ ")
                        .Append("   INTO TBL_CAL_EVENTEXDATE")
                        .Append("      ( EVENTID      ")
                        .Append("      , SEQNO        ")
                        .Append("      , EXDATE       ")
                        .Append("      , CREATEDATE   ")
                        .Append("      , UPDATEDATE   ")
                        .Append("      , CREATEACCOUNT")
                        .Append(" 	   , UPDATEACCOUNT")
                        .Append("      , CREATEID     ")
                        .Append("      , UPDATEID     ")
                        .Append("      )              ")

                        .Append(" VALUES ")
                        .Append("      ( :EVENTID      ")
                        .Append("      , :SEQNO        ")
                        .Append("      , :EXDATE       ")
                        .Append("      , :CREATEDATE   ")
                        .Append("      , :UPDATEDATE   ")
                        .Append("      , :CREATEACCOUNT")
                        .Append(" 	   , :UPDATEACCOUNT")
                        .Append("      , :CREATEID     ")
                        .Append("      , :UPDATEID     ")
                        .Append("      )               ")
                    End With

                    Query.CommandText = Sql.ToString()

                    'SQLパラメータ設定
                    With argument
                        Query.AddParameterWithTypeValue("EVENTID", OracleDbType.Varchar2, .EventId)
                        Query.AddParameterWithTypeValue("SEQNO", OracleDbType.Int32, .SeqNo)
                        Query.AddParameterWithTypeValue("EXDATE", OracleDbType.Date, .ExDate)
                        Query.AddParameterWithTypeValue("CREATEDATE", OracleDbType.Date, DateObject(.CreateDate))
                        Query.AddParameterWithTypeValue("UPDATEDATE", OracleDbType.Date, DateObject(.UpdateDate))
                        Query.AddParameterWithTypeValue("CREATEACCOUNT", OracleDbType.Varchar2, .CreateAccount)
                        Query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, .UpdateAccount)
                        Query.AddParameterWithTypeValue("CREATEID", OracleDbType.Varchar2, .CreateId)
                        Query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, .UpdateId)
                    End With

                    'SQL実行（適用データ数を返却）
                    Count = Query.Execute()

                Catch ex As OracleException
                    Logger.Error("OracleException  : IC30404dataSet: InsertEventExDate:")
                    Throw New ApplicationException(ORACLE_EXCEPTION)

                Catch ex As OracleExceptionEx
                    Logger.Error("OracleExceptionEx: IC30404dataSet: InsertEventExDate:")
                    Throw New ApplicationException(ORACLE_EXCEPTION_EX)

                End Try
                Return Count
            End Using
        End Function


        ''' <summary>
        ''' 除外日情報を取得
        ''' </summary>
        ''' <param name="EventId"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetEventExDate(eventId As String) As IC3040404DataSet.TableDataTableDataTable

            Using Query As New DBSelectQuery(Of IC3040404DataSet.TableDataTableDataTable)("IC3040404_013")

                '結果のデータセット
                Dim DataTable As IC3040404DataSet.TableDataTableDataTable = Nothing

                Try
                    'SQLを作成
                    Dim Sql As New StringBuilder
                    With Sql
                        '対象のデータを取得
                        .Append(" SELECT /* IC3040404_013 */ ")
                        .Append("	     EVENTID       ")
                        .Append("	   , SEQNO         ")
                        .Append("	   , EXDATE        ")
                        .Append("	   , CREATEDATE     ")
                        .Append("	   , UPDATEDATE    ")
                        .Append("	   , CREATEACCOUNT ")
                        .Append("	   , UPDATEACCOUNT ")
                        .Append("	   , CREATEID       ")
                        .Append("	   , UPDATEID      ")
                        .Append("   FROM TBL_CAL_EVENTEXDATE")
                        .Append("  WHERE EVENTID = :EVENTID")
                        .Append("  ORDER BY SEQNO")
                    End With

                    'コマンド生成
                    Query.CommandText = Sql.ToString()

                    'SQLパラメータ設定
                    Query.AddParameterWithTypeValue("EVENTID", OracleDbType.Char, eventId)

                    'SQL実行（結果表を返却）
                    DataTable = Query.GetData()

                Catch ex As OracleException
                    Logger.Error("OracleException  : IC30404dataSet: GetEventExDate:")
                    Throw New ApplicationException(ORACLE_EXCEPTION)

                Catch ex As OracleExceptionEx
                    Logger.Error("OracleExceptionEx: IC30404dataSet: GetEventExDate:")
                    Throw New ApplicationException(ORACLE_EXCEPTION_EX)

                End Try

                Return DataTable
            End Using
        End Function

        ''' <summary>
        ''' アラーム情報を取得
        ''' </summary>
        ''' <param name="EventId"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' アラーム情報の取得を  IC3040404_001から除外
        ''' RECURRENCE情報取得でSQLが複雑になり、パフォーマンスが低下するため
        ''' </remarks>
        Public Function GetAlarm(eventId As String) As IC3040404DataSet.TableDataTableDataTable

            Using Query As New DBSelectQuery(Of IC3040404DataSet.TableDataTableDataTable)("IC3040404_014")

                '結果のデータセット
                Dim DataTable As IC3040404DataSet.TableDataTableDataTable = Nothing

                Try
                    'SQLを作成
                    Dim Sql As New StringBuilder
                    With Sql
                        '対象のデータを取得
                        .Append(" SELECT /* IC3040404_014 */ ")
                        .Append("	     EVENTID")
                        .Append("	   , SEQNO")
                        .Append("	   , STARTTRIGGER")
                        .Append("	   , CREATEDATE")
                        .Append("	   , UPDATEDATE")
                        .Append("	   , CREATEACCOUNT")
                        .Append("	   , UPDATEACCOUNT")
                        .Append("	   , CREATEID")
                        .Append("	   , UPDATEID")
                        .Append("   FROM TBL_CAL_EVENTALARM")
                        .Append("  WHERE EVENTID = :EVENTID")
                        .Append("  ORDER BY SEQNO")
                    End With

                    'コマンド生成
                    Query.CommandText = Sql.ToString()

                    'SQLパラメータ設定
                    Query.AddParameterWithTypeValue("EVENTID", OracleDbType.Char, eventId)

                    'SQL実行（結果表を返却）
                    DataTable = Query.GetData()

                Catch ex As OracleException
                    Logger.Error("OracleException  : IC30404dataSet: GetAlarm:")
                    Throw New ApplicationException(ORACLE_EXCEPTION)

                Catch ex As OracleExceptionEx
                    Logger.Error("OracleExceptionEx: IC30404dataSet: GetAlarm:")
                    Throw New ApplicationException(ORACLE_EXCEPTION_EX)

                End Try

                Return DataTable

            End Using

        End Function

        Public Sub New()

        End Sub

    End Class


    ''' <summary>
    ''' TBL_CAL_CARD_LASTMODIFYの引数の構造体
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CalCardLastModify
        Private InStaffCD As String
        Private InCalUpdateDate As DateTime
        Private InCardUpdateDate As DateTime
        Private InCreateDate As DateTime
        Private InUpdateDate As DateTime
        Private InCreateAccount As String
        Private inUpdateAccount As String
        Private InCreateId As String
        Private InUpdateId As String

        'コンストラクタ
        Sub New()

        End Sub

        ''' <summary>
        ''' Getter Setter群
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property StaffCD As String
            Get
                Return InStaffCD
            End Get
            Set(Value As String)
                InStaffCD = Value
            End Set
        End Property

        Property CalUpdateDate As DateTime
            Get
                Return InCalUpdateDate
            End Get
            Set(Value As DateTime)
                InCalUpdateDate = Value
            End Set
        End Property

        Property CardUpdateDate As DateTime
            Get
                Return InCardUpdateDate
            End Get
            Set(Value As DateTime)
                InCardUpdateDate = Value
            End Set
        End Property

        Property CreateDate As DateTime
            Get
                Return InCreateDate
            End Get
            Set(Value As DateTime)
                InCreateDate = Value
            End Set
        End Property

        Property UpdateDate As DateTime
            Get
                Return InUpdateDate
            End Get
            Set(Value As DateTime)
                InUpdateDate = Value
            End Set
        End Property

        Property CreateAccount As String
            Get
                Return InCreateAccount
            End Get
            Set(Value As String)
                InCreateAccount = Value
            End Set
        End Property

        Property UpdateAccount As String
            Get
                Return inUpdateAccount
            End Get
            Set(Value As String)
                inUpdateAccount = Value
            End Set
        End Property

        Property CreateId As String
            Get
                Return InCreateId
            End Get
            Set(Value As String)
                InCreateId = Value
            End Set
        End Property

        Property UpdateId As String
            Get
                Return InUpdateId
            End Get
            Set(Value As String)
                InUpdateId = Value
            End Set
        End Property

    End Class

    ''' <summary>
    ''' TBL_CAL_EVENTITEMの引数の構造体
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CalEventItem
        Private InCalId As String
        Private InTodoId As String
        Private InUniqueId As String
        Private InRecurrenceId As String
        Private InChgSeqNo As String    'CALDAVサーバーでは使用しないのでString
        Private InActStaffStrCD As String
        Private InActStaffCD As String
        Private InRecStaffStrCD As String
        Private InRecStaffCD As String
        Private InContactNo As String   'CALDAVサーバーでは使用しないのでString
        Private InSummary As String
        Private InStartTime As DateTime
        Private InEndTime As DateTime
        Private InTimeFlg As String
        Private InAlldayFlg As String
        Private InMemo As String
        Private InIcropColor As String
        Private InRruleFlg As String
        Private InRruleFreq As String
        Private InRruleInterval As String 'NULLの場合もあるのでString
        Private InRruleUntil As DateTime
        Private InRruleText As String
        Private InLocation As String
        Private InAttendee As String
        Private InTransp As String
        Private InUrl As String
        Private InDelFlg As String
        Private InDelDate As DateTime
        Private InCreateDate As DateTime
        Private InUpdateDate As DateTime
        Private InCreateAccount As String
        Private InUpdateAccount As String
        Private InCreateId As String
        Private InUpdateId As String
        '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 START
        Private InProcessDiv As String
        '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 END

        ''' <summary>
        ''' Setter Getter
        ''' </summary>
        ''' <Value></Value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property CalId As String
            Get
                Return InCalId
            End Get
            Set(Value As String)
                InCalId = Value
            End Set
        End Property

        Property TodoId As String
            Get
                Return InTodoId
            End Get
            Set(Value As String)
                InTodoId = Value
            End Set
        End Property

        Property UniqueId As String
            Get
                Return InUniqueId
            End Get
            Set(Value As String)
                InUniqueId = Value
            End Set
        End Property

        Property RecurrenceId As String
            Get
                Return InRecurrenceId
            End Get
            Set(Value As String)
                InRecurrenceId = Value
            End Set
        End Property

        Property ChgSeqNo As String 'Update
            Get
                Return InChgSeqNo
            End Get
            Set(Value As String)
                InChgSeqNo = Value
            End Set
        End Property

        Property ActStaffStrCD As String
            Get
                Return InActStaffStrCD
            End Get
            Set(Value As String)
                InActStaffStrCD = Value
            End Set
        End Property

        Property ActStaffCD As String
            Get
                Return InActStaffCD
            End Get
            Set(Value As String)
                InActStaffCD = Value
            End Set
        End Property

        Property RecStaffStrCD As String
            Get
                Return InRecStaffStrCD
            End Get
            Set(Value As String)
                InRecStaffStrCD = Value
            End Set
        End Property

        Property RecStaffCD As String
            Get
                Return InRecStaffCD
            End Get
            Set(Value As String)
                InRecStaffCD = Value
            End Set
        End Property

        'CONTACTNOはCalDAVサーバーでは使用しないのでstringにする
        Property ContactNo As String
            Get
                Return InContactNo
            End Get
            Set(Value As String)
                InContactNo = Value
            End Set
        End Property

        Property Summary As String
            Get
                Return InSummary
            End Get
            Set(Value As String)
                InSummary = Value
            End Set
        End Property

        Property StartTime As DateTime
            Get
                Return InStartTime
            End Get
            Set(Value As DateTime)
                InStartTime = Value
            End Set
        End Property

        Property EndTime As DateTime
            Get
                Return InEndTime
            End Get
            Set(Value As DateTime)
                InEndTime = Value
            End Set
        End Property

        Property TimeFlg As String
            Get
                Return InTimeFlg
            End Get
            Set(Value As String)
                InTimeFlg = Value
            End Set
        End Property

        Property AlldayFlg As String
            Get
                Return InAlldayFlg
            End Get
            Set(Value As String)
                InAlldayFlg = Value
            End Set
        End Property

        Property Memo As String
            Get
                Return InMemo
            End Get
            Set(Value As String)
                InMemo = Value
            End Set
        End Property

        Property IcropColor As String
            Get
                Return InIcropColor
            End Get
            Set(Value As String)
                InIcropColor = Value
            End Set
        End Property

        Property RruleFlg As String
            Get
                Return InRruleFlg
            End Get
            Set(Value As String)
                InRruleFlg = Value
            End Set
        End Property

        Property RruleFreq As String
            Get
                Return InRruleFreq
            End Get
            Set(Value As String)
                InRruleFreq = Value
            End Set
        End Property

        'DBは整数型であるが、データがないときNULLなので
        '文字列にする
        Property RruleInterval As String
            Get
                Return InRruleInterval
            End Get
            Set(Value As String)
                InRruleInterval = Value
            End Set
        End Property

        Property RruleUntil As DateTime
            Get
                Return InRruleUntil
            End Get
            Set(Value As DateTime)
                InRruleUntil = Value
            End Set
        End Property

        Property RruleText As String
            Get
                Return InRruleText
            End Get
            Set(Value As String)
                InRruleText = Value
            End Set
        End Property

        Property Location As String
            Get
                Return InLocation
            End Get
            Set(Value As String)
                InLocation = Value
            End Set
        End Property

        Property Attendee As String
            Get
                Return InAttendee
            End Get
            Set(Value As String)
                InAttendee = Value
            End Set
        End Property

        Property Transp As String
            Get
                Return InTransp
            End Get
            Set(Value As String)
                InTransp = Value
            End Set
        End Property

        Property Url As String
            Get
                Return InUrl
            End Get
            Set(Value As String)
                InUrl = Value
            End Set
        End Property

        Property DelFlg As String
            Get
                Return InDelFlg
            End Get
            Set(Value As String)
                InDelFlg = Value
            End Set
        End Property

        Property DelDate As DateTime
            Get
                Return InDelDate
            End Get
            Set(Value As DateTime)
                InDelDate = Value
            End Set
        End Property

        Property CreateDate As DateTime
            Get
                Return InCreateDate
            End Get
            Set(Value As DateTime)
                InCreateDate = Value
            End Set
        End Property

        Property UpdateDate As DateTime
            Get
                Return InUpdateDate
            End Get
            Set(Value As DateTime)
                InUpdateDate = Value
            End Set
        End Property

        Property CreateAccount As String
            Get
                Return InCreateAccount
            End Get
            Set(Value As String)
                InCreateAccount = Value
            End Set
        End Property

        Property UpdateAccount As String
            Get
                Return InUpdateAccount
            End Get
            Set(Value As String)
                InUpdateAccount = Value
            End Set
        End Property

        Property CreateId As String
            Get
                Return InCreateId
            End Get
            Set(Value As String)
                InCreateId = Value
            End Set
        End Property

        Property UpdateId As String
            Get
                Return InUpdateId
            End Get
            Set(Value As String)
                InUpdateId = Value
            End Set
        End Property

        '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 START
        Property ProcessDiv As String
            Get
                Return InProcessDiv
            End Get
            Set(Value As String)
                InProcessDiv = Value
            End Set
        End Property
        '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 END

        'コンストラクタ
        Sub New()

            InCalId = ""
            InTodoId = ""
            InUniqueId = ""
            InRecurrenceId = ""
            InChgSeqNo = ""
            InActStaffStrCD = ""
            InActStaffCD = ""
            InRecStaffStrCD = ""
            InRecStaffCD = ""
            InContactNo = ""
            InSummary = ""
            InStartTime = #1:01:01 AM#
            InEndTime = #1:01:01 AM#
            InTimeFlg = ""
            InAlldayFlg = ""
            InMemo = ""
            InIcropColor = ""
            InRruleFlg = ""
            InRruleFreq = ""
            InRruleInterval = ""
            InRruleUntil = #1:01:01 AM#
            InRruleText = ""
            InLocation = ""
            InAttendee = ""
            InTransp = ""
            InUrl = ""
            InDelFlg = ""
            InDelDate = #1:01:01 AM#
            InCreateDate = #1:01:01 AM#
            InUpdateDate = #1:01:01 AM#
            InCreateAccount = ""
            InUpdateAccount = ""
            InCreateId = ""
            InUpdateId = ""
            '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 START
            InProcessDiv = ""
            '2012/03/05 SKFC 加藤 【SALES_2】受注後工程の対応 END

        End Sub

    End Class


    ''' <summary>
    ''' TBL_CAL_EVENTALARMの引数の構造体
    ''' </summary>
    ''' <remarks>
    ''' アラームテーブル
    ''' </remarks>
    Public Class CalEventAlarm

        Private InEventId As String
        Private InSeqNo As Integer 'Update
        Private InStartTrigger As String

        Private InCreateDate As DateTime
        Private inUpdateDate As DateTime
        Private InCreateAccount As String
        Private InUpdateAccount As String
        Private InCreateId As String
        Private InUpdateId As String

        Property EventId As String
            Get
                Return InEventId
            End Get
            Set(Value As String)
                InEventId = Value
            End Set
        End Property

        Property SeqNo As Integer
            Get
                Return InSeqNo
            End Get
            Set(Value As Integer)
                InSeqNo = Value
            End Set
        End Property

        Property StartTrigger As String
            Get
                Return InStartTrigger
            End Get
            Set(Value As String)
                InStartTrigger = Value
            End Set
        End Property

        Property CreateDate As DateTime
            Get
                Return InCreateDate
            End Get
            Set(Value As DateTime)
                InCreateDate = Value
            End Set
        End Property

        Property UpdateDate As DateTime
            Get
                Return inUpdateDate
            End Get
            Set(Value As DateTime)
                inUpdateDate = Value
            End Set
        End Property

        Property CreateAccount As String
            Get
                Return InCreateAccount
            End Get
            Set(Value As String)
                InCreateAccount = Value
            End Set
        End Property

        Property UpdateAccount As String
            Get
                Return InUpdateAccount
            End Get
            Set(Value As String)
                InUpdateAccount = Value
            End Set
        End Property

        Property CreateId As String
            Get
                Return InCreateId
            End Get
            Set(Value As String)
                InCreateId = Value
            End Set
        End Property

        Property UpdateId As String
            Get
                Return InUpdateId
            End Get
            Set(Value As String)
                InUpdateId = Value
            End Set
        End Property

        'コンストラクタ
        Sub New()

        End Sub

    End Class

    ''' <summary>
    ''' TBL_CAL_EVENTEXDATEの引数の構造体
    ''' </summary>
    ''' <remarks>
    ''' 除外日テーブル
    ''' </remarks>
    Public Class CalEventExDate

        Private InEventId As String
        Private InSeqNo As Integer 'Update
        Private InExDate As DateTime

        Private InCreateDate As DateTime
        Private InUpDateDate As DateTime
        Private InCreateAccount As String
        Private InUpdateAccount As String
        Private InCreateId As String
        Private InUpdateId As String

        Property EventId As String
            Get
                Return InEventId
            End Get
            Set(Value As String)
                InEventId = Value
            End Set
        End Property

        Property SeqNo As Integer
            Get
                Return InSeqNo
            End Get
            Set(Value As Integer)
                InSeqNo = Value
            End Set
        End Property

        Property ExDate As DateTime
            Get
                Return InExDate
            End Get
            Set(Value As DateTime)
                InExDate = Value
            End Set
        End Property

        Property CreateDate As DateTime
            Get
                Return InCreateDate
            End Get
            Set(Value As DateTime)
                InCreateDate = Value
            End Set
        End Property

        Property UpdateDate As DateTime
            Get
                Return InUpDateDate
            End Get
            Set(Value As DateTime)
                InUpDateDate = Value
            End Set
        End Property

        Property CreateAccount As String
            Get
                Return InCreateAccount
            End Get
            Set(Value As String)
                InCreateAccount = Value
            End Set
        End Property

        Property UpdateAccount As String
            Get
                Return InUpdateAccount
            End Get
            Set(Value As String)
                InUpdateAccount = Value
            End Set
        End Property

        Property CreateId As String
            Get
                Return InCreateId
            End Get
            Set(Value As String)
                InCreateId = Value
            End Set
        End Property

        Property UpdateId As String
            Get
                Return InUpdateId
            End Get
            Set(Value As String)
                InUpdateId = Value
            End Set
        End Property

        'コンストラクタ
        Sub New()

        End Sub

    End Class

End Namespace


Partial Class IC3040404DataSet

End Class
