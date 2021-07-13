'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'MC3040401DataSet.vb
'──────────────────────────────────
'機能： CalDAV連携バッチ
'補足： 
'作成： 2011/12/01 KN   梅村
'更新： 2013/05/27 TMEJ m.asano 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 $01
'更新： 2014/06/05 TMEJ y.gotoh 受注後フォロー機能開発 $02
'更新： 2019/04/23 NSK a.ohhira TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 $04
'──────────────────────────────────

Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core

Namespace MC3040401DataSetTableAdapters
    Public Class MC3040401ScheduleDataSetTableAdapters
        Inherits Global.System.ComponentModel.Component

#Region "定数"
        ''' <summary>
        ''' 機能ID
        ''' </summary>
        ''' <remarks></remarks>
        Private Const C_SYSTEM As String = "MC3040401"

        '$02 受注後フォロー機能開発 START
        ''' <summary>
        ''' 受注前後区分(受注前)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ODR_BEFORE_AFTER_DIV_BEFORE As String = "1"

        ''' <summary>
        ''' 受注前後区分(受注後)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ODR_BEFORE_AFTER_DIV_AFTER As String = "2"
        '$02 受注後フォロー機能開発 END
#End Region

#Region "前回バッチ起動日時取得"

        ''' <summary>
        ''' 前回バッチ起動日時の取得
        ''' </summary>
        ''' <returns>前回バッチ起動日時</returns>
        ''' <remarks></remarks>
        Public Function SelectLastProcInfo() As MC3040401DataSet.MC3040401BatchProcInfoDataTable

            Using query As New DBSelectQuery(Of MC3040401DataSet.MC3040401BatchProcInfoDataTable)("MC3040401_001")
                Dim sql As New StringBuilder

                With sql
                    .Append("SELECT /* MC3040401_001 */ ")
                    .Append("    VALUE AS LASTPROCDATETIME ")
                    .Append("FROM ")
                    .Append("    TBL_PROGRAMSETTING ")
                    .Append("WHERE ")
                    .Append("    PROGRAMID = '" & C_SYSTEM & "' AND ")
                    .Append("    SECTION = 'PROCINFO' AND ")
                    .Append("    KEY = 'LASTPROCDATETIME' ")
                End With

                query.CommandText = sql.ToString()
                Return query.GetData()
            End Using
        End Function

#End Region

#Region "未登録スケジュール情報取得"

        ''' <summary>
        ''' 再登録用の未登録スケジュール情報の取得
        ''' </summary>
        ''' <param name="odrBeforeAfterDiv">受注前後区分</param>
        ''' <returns>更新用スケジュール情報</returns>
        ''' <remarks></remarks>
        Public Function SelectUnregistScheduleInfo(ByVal odrBeforeAfterDiv As String) As MC3040401DataSet.MC3040401UpdateScheduleInfoDataTable

            Using query As New DBSelectQuery(Of MC3040401DataSet.MC3040401UpdateScheduleInfoDataTable)("MC3040401_002")
                Dim sql As New StringBuilder
                With sql
                    .Append("SELECT /* MC3040401_002 */ ")
                    .Append("    '1' AS UPDATEPROCDIV, ")
                    .Append("    A.DLRCD, ")
                    .Append("    A.STRCD, ")
                    .Append("    A.SCHEDULEDIV, ")
                    .Append("    A.SCHEDULEID, ")
                    .Append("    A.SCHEDULEID_SEQNO, ")
                    .Append("    A.ACTIONTYPE, ")
                    .Append("    A.COMPLETEFLG, ")
                    .Append("    A.COMPLETEDATE, ")
                    .Append("    A.ACTCREATESTAFFCD, ")
                    .Append("    A.ACTSTAFFSTRCD, ")
                    .Append("    A.ACTSTAFFCD, ")
                    .Append("    A.RECSTAFFSTRCD, ")
                    .Append("    A.RECSTAFFCD, ")
                    .Append("    A.CUSTDIV, ")
                    .Append("    A.CUSTID, ")
                    .Append("    A.CUSTNAME, ")
                    .Append("    A.DMSID, ")
                    .Append("    A.RECEPTIONDIV, ")
                    .Append("    A.SERVICECODE, ")
                    .Append("    A.MERCHANDISECD, ")
                    .Append("    A.STRSTATUS, ")
                    .Append("    A.REZSTATUS, ")
                    .Append("    A.PARENTDIV, ")
                    .Append("    A.REGISTFLG, ")
                    .Append("    A.CONTACTNO, ")
                    .Append("    A.SUMMARY, ")
                    .Append("    A.STARTTIME, ")
                    .Append("    A.ENDTIME, ")
                    .Append("    A.MEMO, ")
                    .Append("    A.BACKGROUNDCOLOR, ")
                    .Append("    A.ALARMNO, ")
                    .Append("    A.TODOID, ")
                    '  2012/02/29 KN 梅村 【SALES_2】受注後工程CalDAV連携対応 START
                    .Append("    A.DELETEDATE, ")
                    .Append("    A.PROCESSDIV, ")
                    .Append("    A.RESULTDATE, ")
                    '  2012/02/29 KN 梅村 【SALES_2】受注後工程CalDAV連携対応 END
                    '$02 受注後フォロー機能開発 START
                    .Append("    A.CONTACT_NAME, ")
                    .Append("    A.ACT_ODR_NAME, ")
                    .Append("    A.ODR_DIV, ")
                    .Append("    A.AFTER_ODR_ACT_ID ")
                    '$02 受注後フォロー機能開発 END
                    .Append("FROM ")
                    .Append("    TBL_UNREGIST_SCHEDULE A ")
                    .Append("WHERE ")
                    '  2012/02/29 KN 梅村 【SALES_2】受注後工程CalDAV連携対応 START
                    .Append("    A.UNREGIST_REASON = '2' ")
                    '  2012/02/29 KN 梅村 【SALES_2】受注後工程CalDAV連携対応 END
                    '$02 受注後フォロー機能開発 START
                    .Append("AND ")
                    If ODR_BEFORE_AFTER_DIV_BEFORE.Equals(odrBeforeAfterDiv) Then
                        .Append("    A.SCHEDULEDIV IN ('0', '1') ")
                    Else
                        .Append("    A.SCHEDULEDIV = '2'")
                    End If
                    '$02 受注後フォロー機能開発 END
                    .Append("ORDER BY ")
                    .Append("    A.DLRCD, ")
                    .Append("    A.STRCD, ")
                    .Append("    A.SCHEDULEDIV, ")
                    .Append("    A.SCHEDULEID, ")
                    '  2012/02/29 KN 梅村 【SALES_2】受注後工程CalDAV連携対応 START
                    .Append("    A.ACTIONTYPE, ")
                    .Append("    A.PROCESSDIV, ")
                    .Append("    A.PARENTDIV ")
                    '  2012/02/29 KN 梅村 【SALES_2】受注後工程CalDAV連携対応 END
                End With

                query.CommandText = sql.ToString()

                Return query.GetData()
            End Using
        End Function

#End Region

#Region "割当変更セールススタッフ情報取得"

        ''' <summary>
        ''' 割当済みに変更された活動担当スタッフ情報の取得
        ''' </summary>
        ''' <param name="lastProcDate">前回バッチ起動日時</param>
        ''' <returns>更新用スケジュール情報</returns>
        ''' <remarks></remarks>
        ''' <History>
        ''' 2019/04/23 NSK a.ohhira TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 $04
        ''' </History>
        Public Function SelectAllocatedSalesStaffInfo(ByVal lastProcDate As Date) As MC3040401DataSet.MC3040401UpdateScheduleInfoDataTable
            '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 START
            'Public Function SelectAllocatedSalesStaffInfo() As MC3040401DataSet.MC3040401UpdateScheduleInfoDataTable
            '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 END

            Using query As New DBSelectQuery(Of MC3040401DataSet.MC3040401UpdateScheduleInfoDataTable)("MC3040401_003")
                '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 START
                Dim sql As New StringBuilder
                With sql
                    .Append(" SELECT /* MC3040401_003 */ ")
                    .Append("        '2' AS UPDATEPROCDIV ")
                    .Append("      , B.DLRCD AS DLRCD ")
                    .Append("      , B.STRCD AS STRCD ")
                    .Append("      , B.SCHEDULEDIV AS SCHEDULEDIV ")
                    .Append("      , B.SCHEDULEID AS SCHEDULEID ")
                    .Append("      , B.ACTIONTYPE AS ACTIONTYPE ")
                    .Append("      , B.COMPLETEFLG AS COMPLETEFLG ")
                    .Append("      , B.COMPLETEDATE AS COMPLETEDATE ")
                    .Append("      , B.ACTCREATESTAFFCD AS ACTCREATESTAFFCD ")
                    .Append("      , A.SCHE_BRN_CD AS ACTSTAFFSTRCD ")
                    .Append("      , A.SCHE_STF_CD AS ACTSTAFFCD ")
                    .Append("      , B.RECSTAFFSTRCD AS RECSTAFFSTRCD ")
                    .Append("      , B.RECSTAFFCD AS RECSTAFFCD ")
                    .Append("      , B.CUSTDIV AS CUSTDIV ")
                    .Append("      , B.CUSTID AS CUSTID ")
                    .Append("      , B.CUSTNAME AS CUSTNAME ")
                    .Append("      , B.DMSID AS DMSID ")
                    .Append("      , B.RECEPTIONDIV AS RECEPTIONDIV ")
                    .Append("      , B.SERVICECODE AS SERVICECODE ")
                    .Append("      , B.MERCHANDISECD AS MERCHANDISECD ")
                    .Append("      , B.STRSTATUS AS STRSTATUS ")
                    .Append("      , B.REZSTATUS AS REZSTATUS ")
                    .Append("      , B.PARENTDIV AS PARENTDIV ")
                    .Append("      , B.REGISTFLG AS REGISTFLG ")
                    .Append("      , B.CONTACTNO AS CONTACTNO ")
                    .Append("      , B.SUMMARY AS SUMMARY ")
                    .Append("      , B.STARTTIME AS STARTTIME ")
                    .Append("      , B.ENDTIME AS ENDTIME ")
                    .Append("      , B.MEMO AS MEMO ")
                    .Append("      , B.BACKGROUNDCOLOR AS BACKGROUNDCOLOR ")
                    .Append("      , B.ALARMNO AS ALARMNO ")
                    .Append("      , B.TODOID AS TODOID ")
                    .Append("      , B.DELETEDATE AS DELETEDATE ")
                    '$02 受注後フォロー機能開発 START
                    .Append("      , B.CONTACT_NAME ")
                    .Append("      , B.ODR_DIV ")
                    '$02 受注後フォロー機能開発 END
                    .Append("   FROM  ")
                    .Append("      ( ")
                    .Append("         SELECT  ")
                    .Append("                SAL.SALES_ID ")
                    .Append("              , ACT.SCHE_STF_CD ")
                    .Append("              , ACT.SCHE_BRN_CD ")
                    .Append("           FROM  ")
                    .Append("                TB_T_SALES SAL ")
                    .Append("              , TB_T_ACTIVITY ACT ")
                    .Append("          WHERE SAL.REQ_ID = ACT.REQ_ID  ")
                    .Append("            AND SAL.ATT_ID = ACT.ATT_ID ")
                    .Append("            AND ACT.ACT_ID = ( ")
                    .Append("                 SELECT  ")
                    .Append("                        MAX(ACTM.ACT_ID) ")
                    .Append("                   FROM  ")
                    .Append("                        TB_T_ACTIVITY ACTM ")
                    .Append("                  WHERE SAL.REQ_ID = ACTM.REQ_ID  ")
                    .Append("                    AND SAL.ATT_ID = ACTM.ATT_ID ")
                    .Append("                ) ")
                    .Append("            AND TRIM(ACT.SCHE_STF_CD) IS NOT NULL ")
                    '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 START
                    .Append("            AND ACT.ROW_UPDATE_DATETIME >= :LASTPROCDATE ")
                    '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 END
                    .Append("      ) A, ")
                    .Append("      ( ")
                    .Append("         SELECT  ")
                    .Append("               DLRCD ")
                    .Append("             , STRCD ")
                    .Append("             , SCHEDULEDIV ")
                    .Append("             , SCHEDULEID ")
                    .Append("             , ACTIONTYPE ")
                    .Append("             , COMPLETEFLG ")
                    .Append("             , COMPLETEDATE ")
                    .Append("             , ACTCREATESTAFFCD ")
                    .Append("             , ACTSTAFFSTRCD ")
                    .Append("             , ACTSTAFFCD ")
                    .Append("             , RECSTAFFSTRCD ")
                    .Append("             , RECSTAFFCD ")
                    .Append("             , CUSTDIV ")
                    .Append("             , CUSTID ")
                    .Append("             , CUSTNAME ")
                    .Append("             , DMSID ")
                    .Append("             , RECEPTIONDIV ")
                    .Append("             , SERVICECODE ")
                    .Append("             , MERCHANDISECD ")
                    .Append("             , STRSTATUS ")
                    .Append("             , REZSTATUS ")
                    .Append("             , PARENTDIV ")
                    .Append("             , REGISTFLG ")
                    .Append("             , CONTACTNO ")
                    .Append("             , SUMMARY ")
                    .Append("             , STARTTIME ")
                    .Append("             , ENDTIME ")
                    .Append("             , MEMO ")
                    .Append("             , BACKGROUNDCOLOR ")
                    .Append("             , ALARMNO ")
                    .Append("             , TODOID ")
                    .Append("             , DELETEDATE ")
                    '$02 受注後フォロー機能開発 START
                    .Append("             , CONTACT_NAME ")
                    .Append("             , ODR_DIV ")
                    '$02 受注後フォロー機能開発 END
                    .Append("           FROM TBL_UNREGIST_SCHEDULE ")
                    '$02 受注後フォロー機能開発 START
                    .Append("          WHERE UNREGIST_REASON = '1'  ")
                    .Append("            AND SCHEDULEDIV = '0' ")
                    '$02 受注後フォロー機能開発 END
                    .Append("      ) B ")
                    .Append("  WHERE A.SALES_ID = B.SCHEDULEID ")
                    .Append("    AND (TRIM(A.SCHE_STF_CD) || ' ') <> (TRIM(B.ACTSTAFFCD) || ' ') ")
                    .Append("  ORDER BY B.DLRCD ")
                    .Append("         , B.STRCD ")
                    .Append("         , B.SCHEDULEDIV ")
                    .Append("         , B.SCHEDULEID ")
                    .Append("         , B.PARENTDIV ")
                    .Append("         , B.ACTSTAFFCD ")
                    .Append("         , B.TODOID ")
                End With
                '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 END

                query.CommandText = sql.ToString()
                '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 START
                query.AddParameterWithTypeValue("LASTPROCDATE", OracleDbType.Date, lastProcDate)
                '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 END

                Return query.GetData()
            End Using
        End Function

#End Region

#Region "割当変更セールススタッフ情報取得（受注後）"

        '$02 受注後フォロー機能開発 START
        ''' <summary>
        ''' 割当済みに変更された活動担当スタッフ情報の取得(受注後工程)
        ''' </summary>
        ''' <param name="lastProcDate">前回バッチ起動日時</param>
        ''' <returns>更新用スケジュール情報</returns>
        ''' <remarks></remarks>
        ''' <History>
        ''' 2019/04/23 NSK a.ohhira TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 $04
        ''' </History>
        Public Function SelectAllocatedSalesStaffInfoAfterProcess(ByVal lastProcDate As Date) As MC3040401DataSet.MC3040401UpdateScheduleInfoDataTable
            '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 START
            'Public Function SelectAllocatedSalesStaffInfoAfterProcess() As MC3040401DataSet.MC3040401UpdateScheduleInfoDataTable
            '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 END

            Using query As New DBSelectQuery(Of MC3040401DataSet.MC3040401UpdateScheduleInfoDataTable)("MC3040401_004")
                Dim sql As New StringBuilder
                With sql
                    .Append(" SELECT /* MC3040401_004 */ ")
                    .Append("        '2' AS UPDATEPROCDIV ")
                    .Append("      , B.DLRCD ")
                    .Append("      , B.STRCD ")
                    .Append("      , B.SCHEDULEDIV ")
                    .Append("      , B.SCHEDULEID ")
                    .Append("      , B.ACTIONTYPE ")
                    .Append("      , B.COMPLETEFLG ")
                    .Append("      , B.COMPLETEDATE ")
                    .Append("      , B.ACTCREATESTAFFCD ")
                    .Append("      , A.SCHE_BRN_CD AS ACTSTAFFSTRCD ")
                    .Append("      , A.SCHE_STF_CD AS ACTSTAFFCD ")
                    .Append("      , B.RECSTAFFSTRCD ")
                    .Append("      , B.RECSTAFFCD ")
                    .Append("      , B.CUSTDIV ")
                    .Append("      , B.CUSTID ")
                    .Append("      , B.CUSTNAME ")
                    .Append("      , B.DMSID ")
                    .Append("      , B.RECEPTIONDIV ")
                    .Append("      , B.SERVICECODE ")
                    .Append("      , B.MERCHANDISECD ")
                    .Append("      , B.STRSTATUS ")
                    .Append("      , B.REZSTATUS ")
                    .Append("      , B.PARENTDIV ")
                    .Append("      , B.REGISTFLG ")
                    .Append("      , B.CONTACTNO ")
                    .Append("      , B.SUMMARY ")
                    .Append("      , B.STARTTIME ")
                    .Append("      , B.ENDTIME ")
                    .Append("      , B.MEMO ")
                    .Append("      , B.BACKGROUNDCOLOR ")
                    .Append("      , B.ALARMNO ")
                    .Append("      , B.TODOID ")
                    .Append("      , B.DELETEDATE ")
                    .Append("      , B.PROCESSDIV ")
                    .Append("      , B.RESULTDATE ")
                    .Append("      , B.CONTACT_NAME ")
                    .Append("      , B.ACT_ODR_NAME ")
                    .Append("      , B.ODR_DIV ")
                    .Append("      , B.AFTER_ODR_ACT_ID ")
                    .Append("   FROM  ")
                    .Append("        ( ")
                    .Append("     SELECT ")
                    .Append("            AFTER_ODR_ACT_ID ")
                    .Append("          , SCHE_STF_CD ")
                    .Append("          , SCHE_BRN_CD ")
                    .Append("       FROM ")
                    .Append("            TB_T_AFTER_ODR_ACT ")
                    .Append("      WHERE TRIM(SCHE_STF_CD) IS NOT NULL ")
                    '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 START
                    .Append("            AND ROW_UPDATE_DATETIME >= :LASTPROCDATE ")
                    '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 END
                    .Append("        ) A, ")
                    .Append("        ( ")
                    .Append("     SELECT  ")
                    .Append("            DLRCD ")
                    .Append("          , STRCD ")
                    .Append("          , SCHEDULEDIV ")
                    .Append("          , SCHEDULEID ")
                    .Append("          , ACTIONTYPE ")
                    .Append("          , COMPLETEFLG ")
                    .Append("          , COMPLETEDATE ")
                    .Append("          , ACTCREATESTAFFCD ")
                    .Append("          , ACTSTAFFSTRCD ")
                    .Append("          , ACTSTAFFCD ")
                    .Append("          , RECSTAFFSTRCD ")
                    .Append("          , RECSTAFFCD ")
                    .Append("          , CUSTDIV ")
                    .Append("          , CUSTID ")
                    .Append("          , CUSTNAME ")
                    .Append("          , DMSID ")
                    .Append("          , RECEPTIONDIV ")
                    .Append("          , SERVICECODE ")
                    .Append("          , MERCHANDISECD ")
                    .Append("          , STRSTATUS ")
                    .Append("          , REZSTATUS ")
                    .Append("          , PARENTDIV ")
                    .Append("          , REGISTFLG ")
                    .Append("          , CONTACTNO ")
                    .Append("          , SUMMARY ")
                    .Append("          , STARTTIME ")
                    .Append("          , ENDTIME ")
                    .Append("          , MEMO ")
                    .Append("          , BACKGROUNDCOLOR ")
                    .Append("          , ALARMNO ")
                    .Append("          , TODOID ")
                    .Append("          , DELETEDATE ")
                    .Append("          , PROCESSDIV ")
                    .Append("          , RESULTDATE ")
                    .Append("          , CONTACT_NAME ")
                    .Append("          , ACT_ODR_NAME ")
                    .Append("          , ODR_DIV ")
                    .Append("          , AFTER_ODR_ACT_ID ")
                    .Append("       FROM TBL_UNREGIST_SCHEDULE ")
                    .Append("      WHERE UNREGIST_REASON = '1' ")
                    .Append("        AND SCHEDULEDIV = '2' ")
                    .Append("        ) B ")
                    .Append("  WHERE TO_CHAR(A.AFTER_ODR_ACT_ID) = B.AFTER_ODR_ACT_ID ")
                    .Append("    AND (TRIM(A.SCHE_STF_CD) || ' ') <> (TRIM(B.ACTSTAFFCD) || ' ') ")
                    .Append("  ORDER BY B.DLRCD ")
                    .Append("         , B.STRCD ")
                    .Append("         , B.SCHEDULEDIV ")
                    .Append("         , B.SCHEDULEID ")
                    .Append("         , B.PARENTDIV ")
                    .Append("         , B.ACTSTAFFCD ")
                    .Append("         , B.TODOID ")
                End With

                query.CommandText = sql.ToString()
                '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 START
                query.AddParameterWithTypeValue("LASTPROCDATE", OracleDbType.Date, lastProcDate)
                '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 END

                Return query.GetData()
            End Using
        End Function
        '$02 受注後フォロー機能開発 END

#End Region

#Region "割当変更サービススタッフ変更情報取得"

        ''' <summary>
        ''' 割当済みに変更された受付担当スタッフ情報の取得
        ''' </summary>
        ''' <param name="lastProcDate">前回バッチ起動日時</param>
        ''' <returns>更新用スケジュール情報</returns>
        ''' <remarks></remarks>
        ''' <History>
        ''' 2019/04/23 NSK a.ohhira TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 $04
        ''' </History>
        Public Function SelectAllocatedSavicesStaffInfo(ByVal lastProcDate As Date) As MC3040401DataSet.MC3040401UpdateScheduleInfoDataTable
            '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 START
            'Public Function SelectAllocatedSavicesStaffInfo() As MC3040401DataSet.MC3040401UpdateScheduleInfoDataTable
            '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 END

            Using query As New DBSelectQuery(Of MC3040401DataSet.MC3040401UpdateScheduleInfoDataTable)("MC3040401_005")
                '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 START
                Dim sql As New StringBuilder
                With sql
                    .Append(" SELECT /* MC3040401_005 */  ")
                    .Append("        '3' AS UPDATEPROCDIV  ")
                    .Append("      , B.DLRCD AS DLRCD  ")
                    .Append("      , B.STRCD AS STRCD  ")
                    .Append("      , B.SCHEDULEDIV AS SCHEDULEDIV  ")
                    .Append("      , B.SCHEDULEID AS SCHEDULEID  ")
                    .Append("      , B.ACTIONTYPE AS ACTIONTYPE  ")
                    .Append("      , B.COMPLETEFLG AS COMPLETEFLG  ")
                    .Append("      , B.COMPLETEDATE AS COMPLETEDATE  ")
                    .Append("      , B.ACTCREATESTAFFCD AS ACTCREATESTAFFCD  ")
                    .Append("      , B.ACTSTAFFSTRCD AS ACTSTAFFSTRCD  ")
                    .Append("      , B.ACTSTAFFCD AS ACTSTAFFCD  ")
                    .Append("      , A.BRN_CD AS RECSTAFFSTRCD  ")
                    .Append("      , A.PIC_SA_STF_CD AS RECSTAFFCD  ")
                    .Append("      , B.CUSTDIV AS CUSTDIV  ")
                    .Append("      , B.CUSTID AS CUSTID  ")
                    .Append("      , B.CUSTNAME AS CUSTNAME  ")
                    .Append("      , B.DMSID AS DMSID  ")
                    .Append("      , B.RECEPTIONDIV AS RECEPTIONDIV  ")
                    .Append("      , B.SERVICECODE AS SERVICECODE  ")
                    .Append("      , B.MERCHANDISECD AS MERCHANDISECD  ")
                    .Append("      , B.STRSTATUS AS STRSTATUS  ")
                    .Append("      , B.REZSTATUS AS REZSTATUS  ")
                    .Append("      , B.PARENTDIV AS PARENTDIV  ")
                    .Append("      , B.REGISTFLG AS REGISTFLG  ")
                    .Append("      , B.CONTACTNO AS CONTACTNO  ")
                    .Append("      , B.SUMMARY AS SUMMARY  ")
                    .Append("      , B.STARTTIME AS STARTTIME  ")
                    .Append("      , B.ENDTIME AS ENDTIME  ")
                    .Append("      , B.MEMO AS MEMO  ")
                    .Append("      , B.BACKGROUNDCOLOR AS BACKGROUNDCOLOR  ")
                    .Append("      , B.ALARMNO AS ALARMNO  ")
                    .Append("      , B.TODOID AS TODOID  ")
                    .Append("      , B.DELETEDATE AS DELETEDATE  ")
                    '$02 受注後フォロー機能開発 START
                    .Append("      , B.CONTACT_NAME ")
                    .Append("      , B.ODR_DIV ")
                    '$02 受注後フォロー機能開発 END
                    .Append("   FROM  ")
                    .Append("     ( ")
                    .Append("         SELECT  ")
                    .Append("                SVC.DLR_CD ")
                    .Append("              , STAFF.BRN_CD ")
                    .Append("              , SVC.SVCIN_ID ")
                    .Append("              , SVC.PIC_SA_STF_CD  ")
                    .Append("           FROM  ")
                    .Append("                TB_T_SERVICEIN SVC ")
                    .Append("              , TB_M_STAFF STAFF  ")
                    .Append("          WHERE  ")
                    .Append("                SVC.DLR_CD = STAFF.DLR_CD ")
                    .Append("            AND SVC.PIC_SA_STF_CD = STAFF.STF_CD  ")
                    .Append("            AND TRIM(SVC.PIC_SA_STF_CD) IS NOT NULL ")
                    '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 START
                    .Append("            AND SVC.ROW_UPDATE_DATETIME >= :LASTPROCDATE ")
                    '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 END
                    .Append("     ) A,  ")
                    .Append("     ( ")
                    .Append("         SELECT  ")
                    .Append("                DLRCD  ")
                    .Append("              , STRCD  ")
                    .Append("              , SCHEDULEDIV  ")
                    .Append("              , SCHEDULEID  ")
                    .Append("              , ACTIONTYPE  ")
                    .Append("              , COMPLETEFLG  ")
                    .Append("              , COMPLETEDATE  ")
                    .Append("              , ACTCREATESTAFFCD  ")
                    .Append("              , ACTSTAFFSTRCD  ")
                    .Append("              , ACTSTAFFCD  ")
                    .Append("              , RECSTAFFSTRCD  ")
                    .Append("              , RECSTAFFCD  ")
                    .Append("              , CUSTDIV  ")
                    .Append("              , CUSTID  ")
                    .Append("              , CUSTNAME  ")
                    .Append("              , DMSID  ")
                    .Append("              , RECEPTIONDIV  ")
                    .Append("              , SERVICECODE  ")
                    .Append("              , MERCHANDISECD  ")
                    .Append("              , STRSTATUS  ")
                    .Append("              , REZSTATUS  ")
                    .Append("              , PARENTDIV  ")
                    .Append("              , REGISTFLG  ")
                    .Append("              , CONTACTNO  ")
                    .Append("              , SUMMARY  ")
                    .Append("              , STARTTIME  ")
                    .Append("              , ENDTIME  ")
                    .Append("              , MEMO  ")
                    .Append("              , BACKGROUNDCOLOR  ")
                    .Append("              , ALARMNO  ")
                    .Append("              , TODOID  ")
                    .Append("              , DELETEDATE  ")
                    '$02 受注後フォロー機能開発 START
                    .Append("              , CONTACT_NAME  ")
                    .Append("              , ODR_DIV  ")
                    '$02 受注後フォロー機能開発 END
                    .Append("           FROM  ")
                    .Append("                TBL_UNREGIST_SCHEDULE  ")
                    .Append("          WHERE  ")
                    '$02 受注後フォロー機能開発 START
                    .Append("                UNREGIST_REASON = '1'  ")
                    .Append("            AND SCHEDULEDIV = '1' ")
                    '$02 受注後フォロー機能開発 END
                    .Append("     ) B  ")
                    .Append(" WHERE  ")
                    .Append("       A.DLR_CD = TRIM(B.DLRCD)  ")
                    .Append("   AND A.BRN_CD = TRIM(B.STRCD)  ")
                    .Append("   AND A.SVCIN_ID = B.SCHEDULEID  ")
                    .Append("   AND (TRIM(A.PIC_SA_STF_CD) || ' ') <> (TRIM(B.RECSTAFFCD) || ' ')  ")
                    .Append(" ORDER BY  ")
                    .Append("       B.DLRCD,  ")
                    .Append("       B.STRCD,  ")
                    .Append("       B.SCHEDULEDIV,  ")
                    .Append("       B.SCHEDULEID,  ")
                    .Append("       B.PARENTDIV,  ")
                    .Append("       B.RECSTAFFCD,  ")
                    .Append("       B.TODOID ")
                End With
                '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 END

                query.CommandText = sql.ToString()
                '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 START
                query.AddParameterWithTypeValue("LASTPROCDATE", OracleDbType.Date, lastProcDate)
                '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 END

                Return query.GetData()
            End Using
        End Function

#End Region

#Region "サービス関連変更情報取得"

        ''' <summary>
        ''' スケジュール情報に反映が必要なサービス関連情報の取得
        ''' </summary>
        ''' <param name="batchStartDate">前回バッチ起動日時</param>
        ''' <returns>更新用スケジュール情報</returns>
        ''' <remarks></remarks>
        Public Function SelectUpdateSarviceInfo(ByVal batchStartDate As Date) As MC3040401DataSet.MC3040401UpdateScheduleInfoDataTable

            Using query As New DBSelectQuery(Of MC3040401DataSet.MC3040401UpdateScheduleInfoDataTable)("MC3040401_006")
                '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 START
                Dim sql As New StringBuilder
                With sql
                    .Append("  SELECT /* MC3040401_006 */   ")
                    .Append("         '4' AS UPDATEPROCDIV   ")
                    .Append("       , B.DLRCD AS DLRCD   ")
                    .Append("       , B.STRCD AS STRCD   ")
                    .Append("       , B.SCHEDULEDIV AS SCHEDULEDIV   ")
                    .Append("       , B.SCHEDULEID AS SCHEDULEID   ")
                    .Append("       , '1' AS ACTIONTYPE   ")
                    .Append("       , B.CUSTOMERDIV AS CUSTDIV   ")
                    .Append("       , B.CUSTCODE AS CUSTID   ")
                    .Append("       , B.CUSTNAME AS CUSTNAME   ")
                    .Append("       , B.DMSID AS DMSID   ")
                    .Append("       , A.PICK_DELI_TYPE AS RECEPTIONDIV   ")
                    .Append("       , A.SVC_CLASS_CD AS SERVICECODE   ")
                    .Append("       , A.MERC_ID AS MERCHANDISECD   ")
                    .Append("       , A.STRSTATUS AS STRSTATUS   ")
                    .Append("       , A.REZSTATUS AS REZSTATUS   ")
                    .Append("       , A.JOB_DTL_MEMO AS MEMO   ")
                    .Append("       , '1' AS REGISTFLG   ")
                    .Append("       , B.ACTSTAFFSTRCD AS ACTSTAFFSTRCD   ")
                    .Append("       , B.ACTSTAFFCD AS ACTSTAFFCD   ")
                    .Append("       , B.RECSTAFFSTRCD AS RECSTAFFSTRCD   ")
                    .Append("       , B.RECSTAFFCD AS RECSTAFFCD   ")
                    .Append("       , DECODE(B.STARTTIME,' ','',TO_CHAR(B.STARTTIME,'YYYY/MM/DD HH24:MI:SS')) AS STARTTIME   ")
                    .Append("       , DECODE(B.ENDTIME,' ','',TO_CHAR(B.ENDTIME,'YYYY/MM/DD HH24:MI:SS')) AS ENDTIME   ")
                    .Append("       , B.ICROPCOLOR AS BACKGROUNDCOLOR   ")
                    .Append("       , B.PARENTDIV AS PARENTDIV   ")
                    .Append("       , B.SUMMARY AS SUMMARY   ")
                    '$02 受注後フォロー機能開発 START
                    .Append("       , B.CONTACT_NAME ")
                    .Append("       , B.ODR_DIV ")
                    '$02 受注後フォロー機能開発 END
                    .Append("    FROM   ")
                    .Append("       (   ")
                    .Append("           SELECT   ")
                    .Append("                 SVC.DLR_CD  ")
                    .Append("               , SVC.BRN_CD  ")
                    .Append("               , SVC.SVCIN_ID  ")
                    .Append("               , SVC.PICK_DELI_TYPE  ")
                    .Append("               , SVR.SVC_CLASS_CD  ")
                    .Append("               , JOB.MERC_ID  ")
                    .Append("               , CASE WHEN SVC.SVC_STATUS = '00' THEN '0' ELSE '1' END STRSTATUS  ")
                    .Append("               , CASE WHEN SVC.RESV_STATUS = '0' THEN 2 ELSE 1 END REZSTATUS  ")
                    .Append("               , JOB.JOB_DTL_MEMO   ")
                    .Append("           FROM  TB_T_SERVICEIN SVC   ")
                    .Append("               , TB_T_JOB_DTL JOB  ")
                    .Append("               , TB_M_SERVICE_CLASS SVR ")
                    .Append("           WHERE   ")
                    .Append("                 SVC.SVCIN_ID = JOB.SVCIN_ID  ")
                    '$02 受注後フォロー機能開発 START
                    .Append("             AND SVC.SVC_STATUS IN  ")
                    .Append("                 ('00', '01', '03', '04', '05', '06', '07', '08', '09', '10', '11', '12')  ")
                    '$02 受注後フォロー機能開発 END
                    .Append("             AND JOB.JOB_DTL_ID = (  ")
                    .Append("                       SELECT  ")
                    .Append("                              MIN(JOBM.JOB_DTL_ID)  ")
                    .Append("                         FROM  ")
                    .Append("                              TB_T_JOB_DTL JOBM  ")
                    .Append("                        WHERE  ")
                    .Append("                              SVC.SVCIN_ID = JOBM.SVCIN_ID  ")
                    .Append("                          AND JOBM.CANCEL_FLG = '0'  ")
                    .Append("                 )  ")
                    .Append("             AND JOB.SVC_CLASS_ID = SVR.SVC_CLASS_ID ")
                    '$02 受注後フォロー機能開発 START
                    .Append("             AND SVC.ROW_UPDATE_DATETIME >= :BATCHSTARTDATE ")
                    '$02 受注後フォロー機能開発 END
                    .Append("       ) A,   ")
                    .Append("       (   ")
                    .Append("           SELECT   ")
                    .Append("                  C.DLRCD  ")
                    .Append("                , C.STRCD  ")
                    .Append("                , C.SCHEDULEDIV  ")
                    .Append("                , C.SCHEDULEID  ")
                    .Append("                , C.CUSTOMERDIV  ")
                    .Append("                , C.CUSTCODE  ")
                    .Append("                , C.CUSTNAME  ")
                    .Append("                , C.DMSID  ")
                    .Append("                , C.RECEPTIONDIV  ")
                    .Append("                , C.SERVICECODE  ")
                    .Append("                , C.MERCHANDISECD  ")
                    .Append("                , C.STRSTATUS  ")
                    .Append("                , C.REZSTATUS  ")
                    .Append("                , D.MEMO  ")
                    .Append("                , D.ACTSTAFFSTRCD  ")
                    .Append("                , D.ACTSTAFFCD  ")
                    .Append("                , D.RECSTAFFSTRCD  ")
                    .Append("                , D.RECSTAFFCD  ")
                    .Append("                , D.STARTTIME  ")
                    .Append("                , D.ENDTIME  ")
                    .Append("                , D.ICROPCOLOR  ")
                    .Append("                , D.PARENTDIV  ")
                    .Append("                , D.SUMMARY  ")
                    '$02 受注後フォロー機能開発 START
                    .Append("                , D.CONTACT_NAME  ")
                    .Append("                , D.ODR_DIV  ")
                    '$02 受注後フォロー機能開発 END
                    .Append("             FROM   ")
                    .Append("                  TBL_CAL_ICROPINFO C  ")
                    .Append("                , TBL_CAL_TODOITEM D  ")
                    .Append("            WHERE C.CALID = D.CALID  ")
                    .Append("              AND C.SCHEDULEDIV = '1'  ")
                    .Append("              AND C.DELFLG = '0'  ")
                    .Append("              AND D.COMPLETIONFLG = '0' ")
                    .Append("              AND D.DELFLG = '0'  ")
                    .Append("       ) B ")
                    .Append("  WHERE A.DLR_CD = TRIM(B.DLRCD)")
                    .Append("    AND A.BRN_CD = TRIM(B.STRCD) ")
                    .Append("    AND A.SVCIN_ID = B.SCHEDULEID  ")
                    .Append("    AND (    A.PICK_DELI_TYPE <> TRIM(B.RECEPTIONDIV) ")
                    .Append("          OR A.SVC_CLASS_CD <> TRIM(B.SERVICECODE) ")
                    .Append("          OR A.MERC_ID <> TRIM(B.MERCHANDISECD) ")
                    .Append("          OR A.STRSTATUS <> TRIM(B.STRSTATUS) ")
                    .Append("          OR A.REZSTATUS <> TRIM(B.REZSTATUS) ")
                    .Append("          OR TRIM(A.JOB_DTL_MEMO) <> TRIM(B.MEMO) ")
                    .Append("        )   ")
                    .Append("  ORDER BY B.DLRCD ")
                    .Append("         , B.STRCD ")
                    .Append("         , B.SCHEDULEDIV ")
                    .Append("         , B.SCHEDULEID ")
                    .Append("         , B.PARENTDIV ")
                    .Append("         , A.PICK_DELI_TYPE ")
                    .Append("         , A.SVC_CLASS_CD ")
                    .Append("         , A.MERC_ID ")
                    .Append("         , A.STRSTATUS ")
                    .Append("         , A.REZSTATUS ")
                    .Append("         , A.JOB_DTL_MEMO ")
                End With
                '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 END

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("BATCHSTARTDATE", OracleDbType.Date, batchStartDate)

                Return query.GetData()
            End Using
        End Function

#End Region

#Region "作成済みスケジュールのセールススタッフ変更情報取得"

        ''' <summary>
        ''' 作成済みスケジュール情報のセールススタッフ変更情報の取得
        ''' </summary>
        ''' <param name="lastProcDate">前回バッチ起動日時</param>
        ''' <returns>更新用スケジュール情報</returns>
        ''' <remarks></remarks>
        ''' <History>
        ''' 2019/04/23 NSK a.ohhira TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 $04
        ''' </History>
        Public Function SelectUpdateSalesStaffInfo(ByVal lastProcDate As Date) As MC3040401DataSet.MC3040401UpdateScheduleInfoDataTable
            '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 START
            'Public Function SelectUpdateSalesStaffInfo() As MC3040401DataSet.MC3040401UpdateScheduleInfoDataTable
            '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 END

            Using query As New DBSelectQuery(Of MC3040401DataSet.MC3040401UpdateScheduleInfoDataTable)("MC3040401_007")
                Dim sql As New StringBuilder

                '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 START
                With sql
                    .Append(" SELECT /* MC3040401_007 */ ")
                    .Append("        '5' AS UPDATEPROCDIV ")
                    .Append("      , B.DLRCD AS DLRCD ")
                    .Append("      , B.STRCD AS STRCD ")
                    .Append("      , B.SCHEDULEDIV AS SCHEDULEDIV ")
                    .Append("      , B.SCHEDULEID AS SCHEDULEID ")
                    .Append("      , '2' AS ACTIONTYPE ")
                    '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 START
                    '.Append("      , A.SCHE_BRN_CD AS ACTSTAFFSTRCD ")
                    '.Append("      , A.SCHE_STF_CD AS ACTSTAFFCD ")
                    .Append("      , TRIM(A.SCHE_BRN_CD) AS ACTSTAFFSTRCD ")
                    .Append("      , TRIM(A.SCHE_STF_CD) AS ACTSTAFFCD ")
                    '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 END
                    .Append("      , B.TODOID AS TODOID ")
                    .Append("      , DECODE(B.STARTTIME, ' ', '', TO_CHAR(B.STARTTIME, 'YYYY/MM/DD HH24:MI:SS')) AS STARTTIME ")
                    .Append("      , DECODE(B.ENDTIME, ' ', '', TO_CHAR(B.ENDTIME, 'YYYY/MM/DD HH24:MI:SS')) AS ENDTIME ")
                    '$02 受注後フォロー機能開発 START
                    .Append("      , B.ODR_DIV ")
                    '$02 受注後フォロー機能開発 END
                    .Append("   FROM  ")
                    .Append("      ( ")
                    .Append("         SELECT  ")
                    .Append("                SAL.SALES_ID ")
                    .Append("              , ACT.SCHE_STF_CD ")
                    .Append("              , ACT.SCHE_BRN_CD ")
                    .Append("           FROM  ")
                    .Append("                TB_T_SALES SAL ")
                    .Append("              , TB_T_ACTIVITY ACT ")
                    .Append("          WHERE SAL.REQ_ID = ACT.REQ_ID  ")
                    .Append("            AND SAL.ATT_ID = ACT.ATT_ID ")
                    .Append("            AND ACT.ACT_ID = ( ")
                    .Append("                 SELECT  ")
                    .Append("                        MAX(ACTM.ACT_ID) ")
                    .Append("                   FROM  ")
                    .Append("                        TB_T_ACTIVITY ACTM ")
                    .Append("                  WHERE SAL.REQ_ID = ACTM.REQ_ID  ")
                    .Append("                    AND SAL.ATT_ID = ACTM.ATT_ID ")
                    .Append("                ) ")
                    '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 START
                    '.Append("            AND TRIM(ACT.SCHE_STF_CD) IS NOT NULL ")
                    .Append("             AND ACT.ROW_UPDATE_DATETIME >= :LASTPROCDATE ")
                    '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 END
                    .Append("      ) A, ")
                    .Append("      ( ")
                    .Append("         SELECT  ")
                    .Append("                C.DLRCD ")
                    .Append("              , C.STRCD ")
                    .Append("              , C.SCHEDULEDIV ")
                    .Append("              , C.SCHEDULEID ")
                    .Append("              , D.ACTSTAFFCD ")
                    .Append("              , D.TODOID ")
                    .Append("              , D.STARTTIME ")
                    .Append("              , D.ENDTIME ")
                    '$02 受注後フォロー機能開発 START
                    .Append("              , D.ODR_DIV ")
                    '$02 受注後フォロー機能開発 END
                    .Append("           FROM TBL_CAL_ICROPINFO C ")
                    .Append("              , TBL_CAL_TODOITEM D ")
                    .Append("          WHERE C.CALID = D.CALID ")
                    '$02 受注後フォロー機能開発 START
                    .Append("            AND C.SCHEDULEDIV = '0' ")
                    '$02 受注後フォロー機能開発 END
                    .Append("            AND C.DELFLG = '0' ")
                    .Append("            AND D.COMPLETIONFLG = '0' ")
                    .Append("            AND D.DELFLG = '0' ")
                    .Append("      ) B ")
                    .Append("  WHERE A.SALES_ID = B.SCHEDULEID ")
                    .Append("    AND (TRIM(A.SCHE_STF_CD) || ' ') <> (TRIM(B.ACTSTAFFCD) || ' ') ")
                    .Append("  ORDER BY B.DLRCD ")
                    .Append("         , B.STRCD ")
                    .Append("         , B.SCHEDULEDIV ")
                    .Append("         , B.SCHEDULEID ")
                    .Append("         , A.SCHE_STF_CD ")
                End With
                '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 END

                query.CommandText = sql.ToString()
                '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 START
                query.AddParameterWithTypeValue("LASTPROCDATE", OracleDbType.Date, lastProcDate)
                '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 END

                Return query.GetData()
            End Using
        End Function

#End Region

#Region "作成済みスケジュールのセールススタッフ変更情報取得（受注後）"

        '$02 受注後フォロー機能開発 START
        ''' <summary>
        ''' 作成済みスケジュール情報のセールススタッフ変更情報の取得（受注後）
        ''' </summary>
        ''' <param name="lastProcDate">前回バッチ起動日時</param>
        ''' <returns>更新用スケジュール情報</returns>
        ''' <remarks></remarks>
        ''' <History>
        ''' 2019/04/23 NSK a.ohhira TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 $04
        ''' </History>
        Public Function SelectUpdateSalesStaffInfoAfterProcess(ByVal lastProcDate As Date) As MC3040401DataSet.MC3040401UpdateScheduleInfoDataTable
            '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 START
            'Public Function SelectUpdateSalesStaffInfoAfterProcess() As MC3040401DataSet.MC3040401UpdateScheduleInfoDataTable
            '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 END

            Using query As New DBSelectQuery(Of MC3040401DataSet.MC3040401UpdateScheduleInfoDataTable)("MC3040401_008")
                Dim sql As New StringBuilder

                With sql
                    .Append(" SELECT /* MC3040401_008 */ ")
                    .Append("        '5' AS UPDATEPROCDIV ")
                    .Append("      , B.DLRCD ")
                    .Append("      , B.STRCD ")
                    .Append("      , B.SCHEDULEDIV ")
                    .Append("      , B.SCHEDULEID ")
                    .Append("      , '2' AS ACTIONTYPE ")
                    '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 START
                    '.Append("      , A.SCHE_BRN_CD AS ACTSTAFFSTRCD ")
                    '.Append("      , A.SCHE_STF_CD AS ACTSTAFFCD ")
                    .Append("      , TRIM(A.SCHE_BRN_CD) AS ACTSTAFFSTRCD ")
                    .Append("      , TRIM(A.SCHE_STF_CD) AS ACTSTAFFCD ")
                    '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 END
                    .Append("      , B.TODOID ")
                    .Append("      , DECODE(B.STARTTIME, ' ', '', TO_CHAR(B.STARTTIME, 'YYYY/MM/DD HH24:MI:SS')) AS STARTTIME ")
                    .Append("      , DECODE(B.ENDTIME, ' ', '', TO_CHAR(B.ENDTIME, 'YYYY/MM/DD HH24:MI:SS')) AS ENDTIME ")
                    .Append("      , B.PROCESSDIV ")
                    .Append("      , B.ODR_DIV ")
                    .Append("      , B.AFTER_ODR_ACT_ID ")
                    .Append("   FROM  ")
                    .Append("        ( ")
                    .Append("     SELECT  ")
                    .Append("            AFTER_ODR_ACT_ID ")
                    .Append("          , SCHE_STF_CD ")
                    .Append("          , SCHE_BRN_CD ")
                    .Append("       FROM  ")
                    .Append("            TB_T_AFTER_ODR_ACT ")
                    '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 START
                    '.Append("      WHERE TRIM(SCHE_STF_CD) IS NOT NULL ")
                    .Append("       WHERE ROW_UPDATE_DATETIME >= :LASTPROCDATE ")
                    '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 END
                    .Append("        ) A, ")
                    .Append("        ( ")
                    .Append("     SELECT  ")
                    .Append("            C.DLRCD ")
                    .Append("          , C.STRCD ")
                    .Append("          , C.SCHEDULEDIV ")
                    .Append("          , C.SCHEDULEID ")
                    .Append("          , D.ACTSTAFFCD ")
                    .Append("          , D.TODOID ")
                    .Append("          , D.STARTTIME ")
                    .Append("          , D.ENDTIME ")
                    .Append("          , D.PROCESSDIV ")
                    .Append("          , D.ODR_DIV ")
                    .Append("          , D.AFTER_ODR_ACT_ID ")
                    .Append("       FROM TBL_CAL_ICROPINFO C ")
                    .Append("          , TBL_CAL_TODOITEM D ")
                    .Append("      WHERE C.CALID = D.CALID ")
                    .Append("        AND C.SCHEDULEDIV = '2' ")
                    .Append("        AND C.DELFLG = '0' ")
                    .Append("        AND D.COMPLETIONFLG = '0' ")
                    .Append("        AND D.DELFLG = '0' ")
                    .Append("        ) B ")
                    .Append("  WHERE TO_CHAR(A.AFTER_ODR_ACT_ID) = B.AFTER_ODR_ACT_ID ")
                    .Append("    AND (TRIM(A.SCHE_STF_CD) || ' ') <> (TRIM(B.ACTSTAFFCD) || ' ') ")
                    .Append("  ORDER BY B.DLRCD ")
                    .Append("         , B.STRCD ")
                    .Append("         , B.SCHEDULEDIV ")
                    .Append("         , B.SCHEDULEID ")
                    .Append("         , A.SCHE_STF_CD ")
                End With

                query.CommandText = sql.ToString()
                '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 START
                query.AddParameterWithTypeValue("LASTPROCDATE", OracleDbType.Date, lastProcDate)
                '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 END

                Return query.GetData()
            End Using
        End Function
        '$02 受注後フォロー機能開発 END
#End Region

#Region "作成済みスケジュールのサービススタッフ変更情報取得"

        ''' <summary>
        ''' 作成済みスケジュール情報のサービススタッフ変更情報の取得
        ''' </summary>
        ''' <param name="lastProcDate">前回バッチ起動日時</param>
        ''' <returns>更新用スケジュール情報</returns>
        ''' <remarks></remarks>
        ''' <History>
        ''' 2019/04/23 NSK a.ohhira TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 $04
        ''' </History>
        Public Function SelectUpdateSavicesStaffInfo(ByVal lastProcDate As Date) As MC3040401DataSet.MC3040401UpdateScheduleInfoDataTable
            '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 START
            'Public Function SelectUpdateSavicesStaffInfo() As MC3040401DataSet.MC3040401UpdateScheduleInfoDataTable
            '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 END

            Using query As New DBSelectQuery(Of MC3040401DataSet.MC3040401UpdateScheduleInfoDataTable)("MC3040401_009")
                Dim sql As New StringBuilder

                '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 START
                With sql
                    .Append(" SELECT /* MC3040401_009 */  ")
                    .Append("        '6' AS UPDATEPROCDIV ")
                    .Append("      , B.DLRCD AS DLRCD  ")
                    .Append("      , B.STRCD AS STRCD  ")
                    .Append("      , B.SCHEDULEDIV AS SCHEDULEDIV  ")
                    .Append("      , B.SCHEDULEID AS SCHEDULEID  ")
                    .Append("      , '2' AS ACTIONTYPE  ")
                    '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 START
                    '.Append("      , A.SCHE_BRN_CD AS ACTSTAFFSTRCD ")
                    '.Append("      , A.SCHE_STF_CD AS ACTSTAFFCD ")
                    .Append("      , TRIM(A.BRN_CD) AS RECSTAFFSTRCD  ")
                    .Append("      , TRIM(A.PIC_SA_STF_CD) AS RECSTAFFCD  ")
                    '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 END
                    .Append("      , B.TODOID AS TODOID  ")
                    .Append("      , DECODE(B.STARTTIME,' ','',TO_CHAR(B.STARTTIME,'YYYY/MM/DD HH24:MI:SS')) AS STARTTIME  ")
                    .Append("      , DECODE(B.ENDTIME,' ','',TO_CHAR(B.ENDTIME,'YYYY/MM/DD HH24:MI:SS')) AS ENDTIME  ")
                    .Append("      , A.CST_NAME AS CUSTNAME  ")
                    '$02 受注後フォロー機能開発 START
                    .Append("      , B.CONTACT_NAME ")
                    .Append("      , B.ODR_DIV ")
                    '$02 受注後フォロー機能開発 END
                    .Append("   FROM  ")
                    .Append("     (  ")
                    .Append("         SELECT  ")
                    .Append("               SVC.DLR_CD  ")
                    .Append("             , SVC.BRN_CD  ")
                    .Append("             , SVC.SVCIN_ID  ")
                    .Append("             , SVC.PIC_SA_STF_CD ")
                    .Append("             , CUST.CST_NAME  ")
                    .Append("          FROM  ")
                    .Append("               TB_T_SERVICEIN SVC ")
                    .Append("             , TB_M_CUSTOMER CUST ")
                    .Append("         WHERE SVC.CST_ID = CUST.CST_ID ")
                    '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 START
                    '.Append("           AND TRIM(SVC.PIC_SA_STF_CD) IS NOT NULL ")
                    .Append("            AND SVC.ROW_UPDATE_DATETIME >= :LASTPROCDATE ")
                    '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 END
                    .Append("     ) A,  ")
                    .Append("     (  ")
                    .Append("         SELECT  ")
                    .Append("                C.DLRCD ")
                    .Append("              , C.STRCD ")
                    .Append("              , C.SCHEDULEDIV ")
                    .Append("              , C.SCHEDULEID ")
                    .Append("              , D.RECSTAFFCD ")
                    .Append("              , D.TODOID ")
                    .Append("              , D.STARTTIME ")
                    .Append("              , D.ENDTIME ")
                    '$02 受注後フォロー機能開発 START
                    .Append("              , D.CONTACT_NAME ")
                    .Append("              , D.ODR_DIV ")
                    '$02 受注後フォロー機能開発 END
                    .Append("           FROM  ")
                    .Append("                TBL_CAL_ICROPINFO C ")
                    .Append("              , TBL_CAL_TODOITEM D ")
                    .Append("          WHERE C.CALID = D.CALID ")
                    .Append("            AND C.SCHEDULEDIV = '1' ")
                    .Append("            AND C.DELFLG = '0' ")
                    .Append("            AND D.COMPLETIONFLG = '0' ")
                    .Append("            AND D.DELFLG = '0' ")
                    .Append("     ) B  ")
                    .Append("  WHERE A.DLR_CD = TRIM(B.DLRCD) ")
                    .Append("    AND A.BRN_CD = TRIM(B.STRCD) ")
                    .Append("    AND A.SVCIN_ID = B.SCHEDULEID ")
                    .Append("    AND (TRIM(A.PIC_SA_STF_CD) || ' ') <> (TRIM(B.RECSTAFFCD) || ' ') ")
                    .Append(" ORDER BY  ")
                    .Append("         B.DLRCD  ")
                    .Append("       , B.STRCD  ")
                    .Append("       , B.SCHEDULEDIV  ")
                    .Append("       , B.SCHEDULEID  ")
                    .Append("       , A.PIC_SA_STF_CD  ")
                End With
                '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 END

                query.CommandText = sql.ToString()
                '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 START
                query.AddParameterWithTypeValue("LASTPROCDATE", OracleDbType.Date, lastProcDate)
                '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 END

                Return query.GetData()
            End Using
        End Function

#End Region

#Region "顧客名変更情報取得"

        '$02 受注後フォロー機能開発 START
        ''' <summary>
        ''' 顧客名の変更情報の取得
        ''' </summary>
        ''' <param name="odrBeforeAfterDiv">受注前後区分</param>
        ''' <param name="lastProcDate">前回バッチ起動日時</param>
        ''' <returns>更新用スケジュール情報</returns>
        ''' <remarks></remarks>
        ''' <History>
        ''' 2019/04/23 NSK a.ohhira TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 $04
        ''' </History>
        Public Function SelectUpdateCustInfo(ByVal odrBeforeAfterDiv As String, ByVal lastProcDate As Date) As MC3040401DataSet.MC3040401UpdateScheduleInfoDataTable
            '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 START
            'Public Function SelectUpdateCustInfo(ByVal odrBeforeAfterDiv As String) As MC3040401DataSet.MC3040401UpdateScheduleInfoDataTable
            '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 END

            Using query As New DBSelectQuery(Of MC3040401DataSet.MC3040401UpdateScheduleInfoDataTable)("MC3040401_010")
                Dim sql As New StringBuilder

                With sql
                    .Append(" SELECT /* MC3040401_010 */ ")
                    .Append("        '7' AS UPDATEPROCDIV ")
                    .Append("      , A.DLRCD ")
                    .Append("      , A.STRCD ")
                    .Append("      , A.SCHEDULEDIV ")
                    .Append("      , A.SCHEDULEID ")
                    .Append("      , '2' AS ACTIONTYPE ")
                    .Append("      , B.CST_NAME AS CUSTNAME ")
                    .Append("      , A.CUSTOMERDIV AS CUSTDIV ")
                    .Append("      , B.CST_ID AS CUSTID ")
                    .Append("      , A.TODOID ")
                    .Append("      , A.CONTACTNO ")
                    .Append("      , A.SERVICECODE ")
                    .Append("      , A.MERCHANDISECD ")
                    .Append("      , A.REZSTATUS ")
                    .Append("      , A.PARENTDIV ")
                    .Append("      , DECODE(A.STARTTIME,' ','',TO_CHAR(A.STARTTIME,'YYYY/MM/DD HH24:MI:SS')) AS STARTTIME ")
                    .Append("      , DECODE(A.ENDTIME,' ','',TO_CHAR(A.ENDTIME,'YYYY/MM/DD HH24:MI:SS')) AS ENDTIME,A.PROCESSDIV ")
                    .Append("      , A.PROCESSDIV ")
                    .Append("      , A.CONTACT_NAME ")
                    .Append("      , A.ACT_ODR_NAME ")
                    .Append("      , A.ODR_DIV ")
                    .Append("      , A.AFTER_ODR_ACT_ID ")
                    .Append("   FROM  ")
                    .Append("        (  ")
                    .Append("     SELECT C.DLRCD ")
                    .Append("          , C.STRCD ")
                    .Append("          , C.SCHEDULEDIV ")
                    .Append("          , C.SCHEDULEID ")
                    .Append("          , C.CUSTCODE ")
                    .Append("          , C.CUSTNAME ")
                    .Append("          , D.TODOID ")
                    .Append("          , D.CONTACTNO ")
                    .Append("          , C.SERVICECODE ")
                    .Append("          , C.MERCHANDISECD ")
                    .Append("          , C.REZSTATUS ")
                    .Append("          , D.PARENTDIV ")
                    .Append("          , D.STARTTIME ")
                    .Append("          , D.ENDTIME ")
                    .Append("          , D.PROCESSDIV ")
                    .Append("          , C.CUSTOMERDIV ")
                    .Append("          , D.CONTACT_NAME ")
                    .Append("          , D.ACT_ODR_NAME ")
                    .Append("          , D.ODR_DIV ")
                    .Append("          , D.AFTER_ODR_ACT_ID ")
                    .Append("       FROM TBL_CAL_ICROPINFO C ")
                    .Append("          , TBL_CAL_TODOITEM D ")
                    .Append("      WHERE C.CALID = D.CALID ")
                    .Append("        AND C.DELFLG = '0' ")
                    .Append("        AND D.COMPLETIONFLG = '0' ")
                    .Append("        AND D.DELFLG = '0' ")
                    If ODR_BEFORE_AFTER_DIV_BEFORE.Equals(odrBeforeAfterDiv) Then
                        .Append("        AND C.SCHEDULEDIV IN ('0', '1') ")
                    Else
                        .Append("        AND C.SCHEDULEDIV = '2' ")
                    End If
                    .Append("        ) A ")
                    .Append("      , TB_M_CUSTOMER B ")
                    .Append("  WHERE TO_NUMBER(A.CUSTCODE) = B.CST_ID ")
                    .Append("    AND A.CUSTNAME <> B.CST_NAME ")
                    '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 START
                    .Append("    AND B.ROW_UPDATE_DATETIME >= :LASTPROCDATE ")
                    '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 END
                    .Append("  ORDER BY A.DLRCD ")
                    .Append("         , A.STRCD  ")
                    .Append("         , A.SCHEDULEDIV  ")
                    .Append("         , A.SCHEDULEID  ")
                    .Append("         , A.PARENTDIV  ")
                    .Append("         , A.PROCESSDIV  ")
                End With

                query.CommandText = sql.ToString()
                '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 START
                query.AddParameterWithTypeValue("LASTPROCDATE", OracleDbType.Date, lastProcDate)
                '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 END

                Return query.GetData()
            End Using
        End Function
        '$02 受注後フォロー機能開発 END

#End Region

#Region "顧客削除情報取得"

        '$02 受注後フォロー機能開発 START
        ''' <summary>
        ''' 削除された顧客情報の取得
        ''' </summary>
        ''' <param name="odrBeforeAfterDiv">受注前後区分</param>
        ''' <returns>更新用スケジュール情報</returns>
        ''' <remarks></remarks>
        Public Function SelectDeleteCustInfo(ByVal odrBeforeAfterDiv As String) As MC3040401DataSet.MC3040401UpdateScheduleInfoDataTable

            Using query As New DBSelectQuery(Of MC3040401DataSet.MC3040401UpdateScheduleInfoDataTable)("MC3040401_011")

                Dim sql As New StringBuilder
                With sql
                    .Append(" SELECT /* MC3040401_011 */ ")
                    .Append("        '8' AS UPDATEPROCDIV ")
                    .Append("      , B.DLRCD ")
                    .Append("      , B.STRCD ")
                    .Append("      , B.SCHEDULEDIV ")
                    .Append("      , B.SCHEDULEID ")
                    .Append("      , '2' AS ACTIONTYPE ")
                    .Append("   FROM ")
                    .Append("        TB_M_CUSTOMER A ")
                    .Append("      , ( ")
                    .Append("     SELECT ")
                    .Append("            C.DLRCD ")
                    .Append("          , C.STRCD ")
                    .Append("          , C.SCHEDULEDIV ")
                    .Append("          , C.SCHEDULEID ")
                    .Append("          , C.CUSTCODE ")
                    .Append("       FROM TBL_CAL_ICROPINFO C ")
                    .Append("          , TBL_CAL_TODOITEM D ")
                    .Append("      WHERE C.CALID = D.CALID ")
                    .Append("        AND C.DELFLG = '0' ")
                    .Append("        AND D.COMPLETIONFLG = '0' ")
                    .Append("        AND D.DELFLG = '0' ")
                    If ODR_BEFORE_AFTER_DIV_BEFORE.Equals(odrBeforeAfterDiv) Then
                        .Append("        AND C.SCHEDULEDIV IN ('0', '1') ")
                    Else
                        .Append("        AND C.SCHEDULEDIV = '2' ")
                    End If
                    .Append("        ) B  ")
                    .Append("  WHERE TO_NUMBER(B.CUSTCODE) = A.CST_ID(+) ")
                    .Append("    AND A.CST_ID IS NULL ")
                    .Append("  ORDER BY B.DLRCD  ")
                    .Append("         , B.STRCD  ")
                    .Append("         , B.SCHEDULEDIV  ")
                    .Append("         , B.SCHEDULEID  ")
                End With

                query.CommandText = sql.ToString()

                Return query.GetData()
            End Using
        End Function
        '$02 受注後フォロー機能開発 END

#End Region

#Region "前回バッチ起動日時更新"

        ''' <summary>
        ''' 前回バッチ起動日時の更新
        ''' </summary>
        ''' <param name="value">前回バッチ起動日時</param>
        ''' <param name="updateDate">更新日時</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function UpdateBatchDateTimeInfo(ByVal value As String, ByVal updateDate As Date) As Boolean

            Using query As New DBUpdateQuery("MC3040401_012")
                Dim sql As New StringBuilder
                With sql

                    .Append("UPDATE /* MC3040401_012 */ ")
                    .Append("    TBL_PROGRAMSETTING ")
                    .Append("SET ")
                    .Append("    VALUE = :VALUE, ")
                    .Append("    UPDATEDATE = :UPDATEDATE ")
                    .Append("WHERE ")
                    .Append("    PROGRAMID = '" & C_SYSTEM & "' AND")
                    .Append("    SECTION = 'PROCINFO' AND ")
                    .Append("    KEY = 'LASTPROCDATETIME'")

                End With
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("VALUE", OracleDbType.Char, value)
                query.AddParameterWithTypeValue("UPDATEDATE", OracleDbType.Date, updateDate)
                query.Execute()

                Return True
            End Using
        End Function

#End Region

#Region "顧客敬称取得"

        ''' <summary>
        ''' 顧客敬称の取得
        ''' </summary>
        ''' <param name="customerId">顧客コード</param>
        ''' <returns>顧客敬称情報</returns>
        ''' <remarks></remarks>
        Public Function GetNameTitleCustomer(ByVal customerId As String)

            Using query As New DBSelectQuery(Of MC3040401DataSet.MC3040401NameTitleInfoDataTable)("MC3040401_013")

                Dim sql As New StringBuilder
                With sql
                    .Append(" SELECT /* MC3040401_013 */  ")
                    .Append("     A.NAMETITLE_NAME AS NAMETITLE  ")
                    .Append(" FROM ")
                    .Append("     TB_M_CUSTOMER A ")
                    .Append(" WHERE ")
                    .Append("     A.CST_ID = :CUSTOMERID ")
                End With

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("CUSTOMERID", OracleDbType.Decimal, customerId)

                Return query.GetData()
            End Using
        End Function
        '$02 受注後フォロー機能開発 END

#End Region

#Region "接触方法名称取得"

        ''' <summary>
        ''' 接触方法名称の取得
        ''' </summary>
        ''' <param name="contactNo">接触方法No</param>
        ''' <returns>接触方法名称情報</returns>
        ''' <remarks></remarks>
        Public Function GetContactName(ByVal contactNo As String)

            Using query As New DBSelectQuery(Of MC3040401DataSet.MC3040401ContactNameInfoDataTable)("MC3040401_014")
                Dim sql As New StringBuilder

                '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 START
                With sql
                    .Append("SELECT /* MC3040401_014 */ ")
                    .Append("    A.CONTACT_NAME AS CONTACT ")
                    .Append("FROM ")
                    .Append("    TB_M_CONTACT_MTD A ")
                    .Append("WHERE ")
                    .Append("    A.CONTACT_MTD = :CONTACTNO ")
                End With
                '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 END

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("CONTACTNO", OracleDbType.Char, contactNo)

                Return query.GetData()
            End Using
        End Function

#End Region

#Region "サービス名の取得"

        ''' <summary>
        ''' サービス名の取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="serviceCode">サービスコード</param>
        ''' <returns>顧客敬称情報</returns>
        ''' <remarks></remarks>
        Public Function GetServiceName(ByVal dealerCode As String,
                                       ByVal branchCode As String,
                                       ByVal serviceCode As String)

            Using query As New DBSelectQuery(Of MC3040401DataSet.MC3040401ServiceNameInfoDataTable)("MC3040401_015")

                '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 START
                Dim sql As New StringBuilder
                With sql
                    .Append("  SELECT /* MC3040401_015 */  ")
                    .Append("         A.SVC_CLASS_NAME AS SVCORGNAME  ")
                    .Append("       , A.SVC_CLASS_NAME_ENG AS SVCENGNAME  ")
                    .Append("    FROM TB_M_SERVICE_CLASS A  ")
                    .Append("       , TB_M_BRANCH_SERVICE_CLASS B ")
                    .Append("   WHERE A.SVC_CLASS_ID = B.SVC_CLASS_ID  ")
                    .Append("     AND B.DLR_CD = :DLR_CD ")
                    .Append("     AND B.BRN_CD = :BRN_CD ")
                    .Append("     AND A.SVC_CLASS_CD = :SERVICECODE ")
                End With

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, Trim(branchCode))
                query.AddParameterWithTypeValue("SERVICECODE", OracleDbType.NVarchar2, serviceCode)
                '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 END

                Return query.GetData()
            End Using
        End Function

#End Region

#Region "商品名取得"

        ''' <summary>
        ''' 商品名の取得
        ''' </summary>
        ''' <param name="merchandiseCode">商品コード</param>
        ''' <returns>商品名情報</returns>
        ''' <remarks></remarks>
        Public Function GetMerchandiseName(ByVal merchandiseCode As Decimal)

            Using query As New DBSelectQuery(Of MC3040401DataSet.MC3040401MerchandiseNameInfoDataTable)("MC3040401_016")

                '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 START
                Dim sql As New StringBuilder
                With sql
                    .Append(" SELECT /* MC3040401_016 */  ")
                    .Append("        A.MERC_NAME AS MERCHANDISENAME_EX  ")
                    .Append("      , A.MERC_NAME_ENG AS MERCHANDISENAME_ENG  ")
                    .Append("   FROM TB_M_MERCHANDISE A  ")
                    .Append("  WHERE A.MERC_ID = :MERCHANDISECD ")
                End With
                '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 END

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("MERCHANDISECD", OracleDbType.Decimal, merchandiseCode)

                Return query.GetData()
            End Using
        End Function

#End Region

#Region "文言取得"

        ''' <summary>
        ''' 文言の取得
        ''' </summary>
        ''' <param name="DisplayId">表示ID</param>
        ''' <param name="DisplayNo">表示No</param>
        ''' <returns>取得文言</returns>
        ''' <remarks></remarks>
        Public Function GetWord(ByVal displayId As String, ByVal displayNo As Integer) As String

            Using query As New DBSelectQuery(Of DataTable)("MC3040401_017")
                Dim sql As New StringBuilder

                With sql
                    .Append("SELECT /* MC3040401_017 */")
                    .Append("   DECODE(DLR.WORD,")
                    .Append("   NULL, INI.WORD,")
                    .Append("   DLR.WORD)")
                    .Append("FROM ")
                    .Append("	TBL_WORD_DLR DLR,")
                    .Append("   TBL_WORD_INI INI")
                    .Append(" WHERE ")
                    .Append("	INI.DISPLAYID = DLR.DISPLAYID(+) AND")
                    .Append("	INI.DISPLAYNO = DLR.DISPLAYNO(+) AND")
                    .Append("	INI.DISPLAYID = :DISPLAYID AND")
                    .Append("	INI.DISPLAYNO = :DISPLAYNO AND")
                    .Append("	DLR.DLRCD(+) = :DLRCD")
                End With

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, "XXXXX")
                query.AddParameterWithTypeValue("DISPLAYID", OracleDbType.Varchar2, displayId)
                query.AddParameterWithTypeValue("DISPLAYNO", OracleDbType.Int32, displayNo)

                Dim dt As DataTable = query.GetData

                If Not dt.Rows Is Nothing AndAlso dt.Rows.Count > 0 Then
                    Return dt.Rows(0).Item(0).ToString()
                Else
                    Return String.Empty
                End If
            End Using
        End Function

#End Region

#Region "過去未登録スケジュール情報削除"

        ''' <summary>
        ''' 過去の未登録スケジュール情報削除
        ''' </summary>
        ''' <param name="batchStartDateTime">バッチ起動日時</param>
        ''' <remarks></remarks>
        ''' <History>
        '''  2019/04/23 NSK a.ohhira TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 $04
        ''' </History>
        Public Sub DeleteUnregistScheduleInfo(ByVal batchStartDateTime As Date)
            '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 START
            'Public Sub DeleteUnregistScheduleInfo(ByVal systemDate As Date, ByVal odrBeforeAfterDiv As String)
            '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 END

            Using query As New DBUpdateQuery("MC3040401_018")
                Dim sql As New StringBuilder
                With sql
                    .Append("DELETE /* MC3040401_018 */ ")
                    .Append("FROM ")
                    .Append("    TBL_UNREGIST_SCHEDULE A ")
                    .Append("WHERE ")
                    '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 START
                    '.Append("    TO_DATE(A.ENDTIME, 'yyyy/mm/dd hh24:mi:ss') < :SYSTEMDATE ")
                    .Append("    TO_DATE(A.ENDTIME, 'yyyy/mm/dd hh24:mi:ss') < :BATCHSTARTDATE ")
                    '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 END
                    '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 START
                    '$02 受注後フォロー機能開発 START
                    'If ODR_BEFORE_AFTER_DIV_BEFORE.Equals(odrBeforeAfterDiv) Then
                    '    .Append("AND A.SCHEDULEDIV IN ('0', '1') ")
                    'Else
                    '    .Append("AND A.SCHEDULEDIV = '2' ")
                    'End If
                    '$02 受注後フォロー機能開発 END
                    '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 END
                End With

                query.CommandText = sql.ToString()
                '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 START
                'query.AddParameterWithTypeValue("SYSTEMDATE", OracleDbType.Date, systemDate)
                query.AddParameterWithTypeValue("BATCHSTARTDATE", OracleDbType.Date, batchStartDateTime)
                '$04 TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 END
                query.Execute()
            End Using
        End Sub

#End Region

    End Class
End Namespace

Partial Class MC3040401DataSet

End Class

