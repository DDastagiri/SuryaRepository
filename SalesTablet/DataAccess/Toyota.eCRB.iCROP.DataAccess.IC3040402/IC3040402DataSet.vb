
'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'IC3040402DataSet.vb
'─────────────────────────────────────
'機能： CalDAV登録支援インタフェースデータアクセス
'補足： 
'作成： 
'更新： 2013/05/27 TMEJ m.asano 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 $01
'更新： 2014/05/12 TMEJ y.gotoh 受注後フォロー機能開発 $02
'─────────────────────────────────────
Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Globalization

Namespace IC3040402DataSetTableAdapters
    Public Class IC3040402ScheduleDataSetTableAdapters
        Inherits Global.System.ComponentModel.Component

        ''' <summary>
        ''' 機能ID
        ''' </summary>
        ''' <remarks></remarks>
        Private Const C_SYSTEM As String = "IC3040402"

        ''' <summary>
        ''' 未登録スケジュール情報登録
        ''' </summary>
        ''' <param name="scheduleInfo">スケジュール情報</param>
        ''' <param name="sequenceIdSeqno">スケジュールID連番</param>
        ''' <param name="unregistReason">未登録理由</param>
        ''' <returns>更新成功[True]/失敗[False]</returns>
        ''' <remarks></remarks>
        Public Function InsertUnregistScheduleInfo(ByVal scheduleInfo As IC3040402DataSet.IC3040402ScheduleInfoRow,
                                                   ByVal sequenceIdSeqno As Integer,
                                                   ByVal unregistReason As String) As Boolean

            Using query As New DBUpdateQuery("IC3040402_001")

                Dim sql As New StringBuilder
                With sql
                    .Append("INSERT /* IC3040402_001 */ ")
                    .Append("INTO ")
                    .Append("    TBL_UNREGIST_SCHEDULE ")
                    .Append("( ")
                    .Append("    DLRCD, ")
                    .Append("    STRCD, ")
                    .Append("    SCHEDULEDIV, ")
                    .Append("    SCHEDULEID, ")
                    .Append("    SCHEDULEID_SEQNO, ")
                    .Append("    UNREGIST_REASON, ")
                    .Append("    ACTIONTYPE, ")
                    .Append("    COMPLETEFLG, ")
                    .Append("    COMPLETEDATE, ")
                    .Append("    ACTCREATESTAFFCD, ")
                    .Append("    ACTSTAFFSTRCD, ")
                    .Append("    ACTSTAFFCD, ")
                    .Append("    RECSTAFFSTRCD, ")
                    .Append("    RECSTAFFCD, ")
                    .Append("    CUSTDIV, ")
                    .Append("    CUSTID, ")
                    .Append("    CUSTNAME, ")
                    .Append("    DMSID, ")
                    .Append("    RECEPTIONDIV, ")
                    .Append("    SERVICECODE, ")
                    .Append("    MERCHANDISECD, ")
                    .Append("    STRSTATUS, ")
                    .Append("    REZSTATUS, ")
                    .Append("    PARENTDIV, ")
                    .Append("    REGISTFLG, ")
                    .Append("    CONTACTNO, ")
                    .Append("    SUMMARY, ")
                    .Append("    STARTTIME, ")
                    .Append("    ENDTIME, ")
                    .Append("    MEMO, ")
                    .Append("    BACKGROUNDCOLOR, ")
                    .Append("    ALARMNO, ")
                    .Append("    TODOID, ")
                    .Append("    DELETEDATE, ")
                    .Append("    CREATEDATE, ")
                    .Append("    UPDATEDATE, ")
                    .Append("    CREATEACCOUNT, ")
                    .Append("    UPDATEACCOUNT, ")
                    .Append("    CREATEID, ")
                    '  2012/02/20 KN 梅村 【SALES_2】受注後工程CalDAV連携対応 START
                    '.Append("    UPDATEID ")
                    .Append("    UPDATEID, ")
                    .Append("    PROCESSDIV, ")
                    '$02 受注後フォロー機能開発 START
                    .Append("    RESULTDATE, ")
                    '$02 受注後フォロー機能開発 END
                    '  2012/02/20 KN 梅村 【SALES_2】受注後工程CalDAV連携対応 END
                    '$02 受注後フォロー機能開発 START
                    .Append("    CONTACT_NAME, ")
                    .Append("    ACT_ODR_NAME, ")
                    .Append("    ODR_DIV, ")
                    .Append("    AFTER_ODR_ACT_ID ")
                    '$02 受注後フォロー機能開発 END
                    .Append(") ")
                    .Append("VALUES ")
                    .Append("( ")
                    .Append("    :DLRCD, ")
                    .Append("    :STRCD, ")
                    .Append("    :SCHEDULEDIV, ")
                    .Append("    :SCHEDULEID, ")
                    .Append("    :SCHEDULEID_SEQNO, ")
                    .Append("    :UNREGIST_REASON, ")
                    .Append("    :ACTIONTYPE, ")
                    .Append("    :COMPLETEFLG, ")
                    .Append("    :COMPLETEDATE, ")
                    .Append("    :ACTCREATESTAFFCD, ")
                    .Append("    :ACTSTAFFSTRCD, ")
                    .Append("    :ACTSTAFFCD, ")
                    .Append("    :RECSTAFFSTRCD, ")
                    .Append("    :RECSTAFFCD, ")
                    .Append("    :CUSTDIV, ")
                    .Append("    :CUSTID, ")
                    .Append("    :CUSTNAME, ")

                    ' $01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 START
                    ' DMSIDは使用されていない為、一律NULL更新する。
                    .Append("    NULL, ")
                    ' $01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 END

                    .Append("    :RECEPTIONDIV, ")
                    .Append("    :SERVICECODE, ")
                    .Append("    :MERCHANDISECD, ")
                    .Append("    :STRSTATUS, ")
                    .Append("    :REZSTATUS, ")
                    .Append("    :PARENTDIV, ")
                    .Append("    :REGISTFLG, ")
                    .Append("    :CONTACTNO, ")
                    .Append("    :SUMMARY, ")
                    .Append("    :STARTTIME, ")
                    .Append("    :ENDTIME, ")
                    .Append("    :MEMO, ")
                    .Append("    :BACKGROUNDCOLOR, ")
                    .Append("    :ALARMNO, ")
                    .Append("    :TODOID, ")
                    If Not scheduleInfo Is Nothing AndAlso Not String.IsNullOrEmpty(scheduleInfo.DeleteDate) Then
                        .Append("    :DELETEDATE, ")
                    Else
                        .Append("    NULL, ")
                    End If
                    .Append("    SYSDATE, ")
                    .Append("    SYSDATE, ")
                    .Append("    :CREATEACCOUNT, ")
                    .Append("    :UPDATEACCOUNT, ")
                    .Append("    :CREATEID, ")
                    '  2012/02/20 KN 梅村 【SALES_2】受注後工程CalDAV連携対応 START
                    '.Append("    :UPDATEID ")
                    .Append("    :UPDATEID, ")
                    .Append("    :PROCESSDIV, ")
                    '$02 受注後フォロー機能開発 START
                    .Append("    :RESULTDATE, ")
                    '$02 受注後フォロー機能開発 END
                    '  2012/02/20 KN 梅村 【SALES_2】受注後工程CalDAV連携対応 END
                    '$02 受注後フォロー機能開発 START
                    .Append("    :CONTACT_NAME, ")
                    .Append("    :ACT_ODR_NAME, ")
                    .Append("    :ODR_DIV, ")
                    .Append("    :AFTER_ODR_ACT_ID ")
                    '$02 受注後フォロー機能開発 END
                    .Append(") ")
                End With

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, scheduleInfo.DealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, scheduleInfo.BranchCode)
                query.AddParameterWithTypeValue("SCHEDULEDIV", OracleDbType.Char, scheduleInfo.ScheduleDiv)


                '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 START
                If String.IsNullOrEmpty(scheduleInfo.ScheduleID) Then
                    query.AddParameterWithTypeValue("SCHEDULEID", OracleDbType.Decimal, Nothing)
                Else
                    query.AddParameterWithTypeValue("SCHEDULEID", OracleDbType.Decimal, scheduleInfo.ScheduleID)
                End If
                '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 END

                query.AddParameterWithTypeValue("SCHEDULEID_SEQNO", OracleDbType.Int64, sequenceIdSeqno)
                query.AddParameterWithTypeValue("UNREGIST_REASON", OracleDbType.Char, unregistReason)
                query.AddParameterWithTypeValue("ACTIONTYPE", OracleDbType.Char, scheduleInfo.ActionType)
                query.AddParameterWithTypeValue("COMPLETEFLG", OracleDbType.Char, scheduleInfo.CompletionDiv)
                query.AddParameterWithTypeValue("COMPLETEDATE", OracleDbType.Char, scheduleInfo.CompletionDate)
                query.AddParameterWithTypeValue("ACTCREATESTAFFCD", OracleDbType.Varchar2, scheduleInfo.ActivityCreateStaff)
                query.AddParameterWithTypeValue("ACTSTAFFSTRCD", OracleDbType.Char, scheduleInfo.ActivityStaffBranchCode)
                query.AddParameterWithTypeValue("ACTSTAFFCD", OracleDbType.Varchar2, scheduleInfo.ActivityStaffCode)
                query.AddParameterWithTypeValue("RECSTAFFSTRCD", OracleDbType.Char, scheduleInfo.ReceptionStaffBranchCode)
                query.AddParameterWithTypeValue("RECSTAFFCD", OracleDbType.Varchar2, scheduleInfo.ReceptionStaffCode)
                query.AddParameterWithTypeValue("CUSTDIV", OracleDbType.Char, scheduleInfo.CustomerDiv)
                query.AddParameterWithTypeValue("CUSTID", OracleDbType.NVarchar2, scheduleInfo.CustomerCode)
                query.AddParameterWithTypeValue("CUSTNAME", OracleDbType.NVarchar2, scheduleInfo.CustomerName)

                ' $01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 START
                ' query.AddParameterWithTypeValue("DMSID", OracleDbType.NVarchar2, scheduleInfo.DmsID)
                ' $01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 END

                query.AddParameterWithTypeValue("RECEPTIONDIV", OracleDbType.Char, scheduleInfo.ReceptionDiv)
                query.AddParameterWithTypeValue("SERVICECODE", OracleDbType.Char, scheduleInfo.ServiceCode)
                query.AddParameterWithTypeValue("MERCHANDISECD", OracleDbType.Char, scheduleInfo.MerchandiseCd)
                query.AddParameterWithTypeValue("STRSTATUS", OracleDbType.Char, scheduleInfo.StrStatus)
                query.AddParameterWithTypeValue("REZSTATUS", OracleDbType.Char, scheduleInfo.RezStatus)
                query.AddParameterWithTypeValue("PARENTDIV", OracleDbType.Char, scheduleInfo.ParentDiv)
                query.AddParameterWithTypeValue("REGISTFLG", OracleDbType.Char, scheduleInfo.CreateScheduleDiv)

                If String.IsNullOrEmpty(scheduleInfo.ContactNo) Then
                    query.AddParameterWithTypeValue("CONTACTNO", OracleDbType.Int64, Nothing)
                Else
                    query.AddParameterWithTypeValue("CONTACTNO", OracleDbType.Int64, scheduleInfo.ContactNo)
                End If

                query.AddParameterWithTypeValue("SUMMARY", OracleDbType.NVarchar2, scheduleInfo.Summary)
                query.AddParameterWithTypeValue("STARTTIME", OracleDbType.Char, scheduleInfo.StartTime)
                query.AddParameterWithTypeValue("ENDTIME", OracleDbType.Char, scheduleInfo.EndTime)
                query.AddParameterWithTypeValue("MEMO", OracleDbType.NVarchar2, scheduleInfo.Memo)
                query.AddParameterWithTypeValue("BACKGROUNDCOLOR", OracleDbType.NVarchar2, scheduleInfo.XiCropColor)

                If String.IsNullOrEmpty(scheduleInfo.Trigger) Then
                    query.AddParameterWithTypeValue("ALARMNO", OracleDbType.Int64, Nothing)
                Else
                    query.AddParameterWithTypeValue("ALARMNO", OracleDbType.Int64, scheduleInfo.Trigger)
                End If

                query.AddParameterWithTypeValue("TODOID", OracleDbType.Varchar2, scheduleInfo.TodoID)
                If Not String.IsNullOrEmpty(scheduleInfo.DeleteDate) Then
                    query.AddParameterWithTypeValue("DELETEDATE", OracleDbType.Date, DateTime.Parse(scheduleInfo.DeleteDate, CultureInfo.InvariantCulture()))
                End If
                query.AddParameterWithTypeValue("CREATEACCOUNT", OracleDbType.Char, scheduleInfo.ActivityCreateStaff)
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Char, scheduleInfo.ActivityCreateStaff)
                query.AddParameterWithTypeValue("CREATEID", OracleDbType.Char, C_SYSTEM)
                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Char, C_SYSTEM)
                '  2012/02/20 KN 梅村 【SALES_2】受注後工程CalDAV連携対応 START
                query.AddParameterWithTypeValue("PROCESSDIV", OracleDbType.Char, scheduleInfo.ProcessDiv)
                query.AddParameterWithTypeValue("RESULTDATE", OracleDbType.Char, scheduleInfo.ResultDate)
                '  2012/02/20 KN 梅村 【SALES_2】受注後工程CalDAV連携対応 END
                '$02 受注後フォロー機能開発 START
                If String.IsNullOrEmpty(scheduleInfo.ContactName) Then
                    query.AddParameterWithTypeValue("CONTACT_NAME", OracleDbType.NVarchar2, " ")
                Else
                    query.AddParameterWithTypeValue("CONTACT_NAME", OracleDbType.NVarchar2, scheduleInfo.ContactName)
                End If
                If String.IsNullOrEmpty(scheduleInfo.ActOdrName) Then
                    query.AddParameterWithTypeValue("ACT_ODR_NAME", OracleDbType.NVarchar2, " ")
                Else
                    query.AddParameterWithTypeValue("ACT_ODR_NAME", OracleDbType.NVarchar2, scheduleInfo.ActOdrName)
                End If
                If String.IsNullOrEmpty(scheduleInfo.OdrDiv) Then
                    query.AddParameterWithTypeValue("ODR_DIV", OracleDbType.Char, " ")
                Else
                    query.AddParameterWithTypeValue("ODR_DIV", OracleDbType.Char, scheduleInfo.OdrDiv)
                End If
                If String.IsNullOrEmpty(scheduleInfo.AfterOdrActId) Then
                    query.AddParameterWithTypeValue("AFTER_ODR_ACT_ID", OracleDbType.Varchar2, " ")
                Else
                    query.AddParameterWithTypeValue("AFTER_ODR_ACT_ID", OracleDbType.Varchar2, scheduleInfo.AfterOdrActId)
                End If
                '$02 受注後フォロー機能開発 EMD

                Return query.Execute()
            End Using
        End Function

        ''' <summary>
        ''' 未登録スケジュール情報削除
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="scheduleDiv">スケジュール区分</param>
        ''' <param name="scheduleId">スケジュールID</param>
        ''' <returns>更新成功[True]/失敗[False]</returns>
        ''' <remarks></remarks>
        Public Function DeleteUnregistScheduleInfo(ByVal dealerCode As String,
                                                   ByVal branchCode As String,
                                                   ByVal scheduleDiv As String,
                                                   ByVal scheduleId As String) As Boolean

            Using query As New DBUpdateQuery("IC3040402_002")

                Dim sql As New StringBuilder
                With sql
                    .Append("DELETE /* IC3040402_002 */ ")
                    .Append("FROM ")
                    .Append("    TBL_UNREGIST_SCHEDULE A ")
                    .Append("WHERE ")
                    .Append("    A.DLRCD = :DLRCD AND ")
                    .Append("    A.STRCD = :STRCD AND ")
                    .Append("    A.SCHEDULEDIV = :SCHEDULEDIV AND ")
                    .Append("    A.SCHEDULEID = :SCHEDULEID")
                End With

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
                query.AddParameterWithTypeValue("SCHEDULEDIV", OracleDbType.Char, scheduleDiv)
                '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 START
                query.AddParameterWithTypeValue("SCHEDULEID", OracleDbType.Decimal, Decimal.Parse(scheduleId, CultureInfo.InvariantCulture))
                '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 END

                Return query.Execute()
            End Using
        End Function

        ''' <summary>
        ''' 未登録スケジュール情報のスケジュールID連番の最大値を取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="scheduleDiv">スケジュール区分</param>
        ''' <param name="scheduleId">スケジュールID</param>
        ''' <returns>スケジュールID連番の最大値</returns>
        ''' <remarks></remarks>
        Public Function SelectScheduleIdSeqnoMax(ByVal dealerCode As String,
                                                 ByVal branchCode As String,
                                                 ByVal scheduleDiv As String,
                                                 ByVal scheduleId As String) As Integer

            Using query As New DBSelectQuery(Of IC3040402DataSet.IC3040402ScheduleIdSeqnoDataTable)("IC3040402_003")
                Dim tbl As IC3040402DataSet.IC3040402ScheduleIdSeqnoDataTable
                Dim scheduleIdSeqno As Integer

                Dim sql As New StringBuilder
                With sql
                    .Append("SELECT /* IC3040402_003 */ ")
                    .Append("    MAX(A.SCHEDULEID_SEQNO) SCHEDULEID_SEQNO ")
                    .Append("FROM ")
                    .Append("    TBL_UNREGIST_SCHEDULE A ")
                    .Append("WHERE ")
                    .Append("    A.DLRCD = :DLRCD AND ")
                    .Append("    A.STRCD = :STRCD AND ")
                    .Append("    A.SCHEDULEDIV = :SCHEDULEDIV AND ")
                    .Append("    A.SCHEDULEID = :SCHEDULEID")
                End With

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
                query.AddParameterWithTypeValue("SCHEDULEDIV", OracleDbType.Char, scheduleDiv)
                '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 START
                query.AddParameterWithTypeValue("SCHEDULEID", OracleDbType.Decimal, scheduleId)
                '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 END

                tbl = query.GetData()
                If tbl.Item(0).IsScheduleId_SeqnoNull Then
                    scheduleIdSeqno = 1
                Else
                    scheduleIdSeqno = CType(tbl.Item(0).ScheduleId_Seqno, Integer) + 1
                End If

                Return scheduleIdSeqno
            End Using
        End Function

    End Class
End Namespace

Partial Class IC3040402DataSet
End Class
