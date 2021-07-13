'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3070210TableAdapter.vb
'─────────────────────────────────────
'機能： 相談履歴
'補足： 
'作成： 2015/03/17 TCS 鈴木  次世代e-CRB 価格相談履歴参照機能開発
'─────────────────────────────────────

Imports System.Text
Imports System.Reflection.MethodBase
Imports System.Globalization
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core


Public NotInheritable Class SC3070210TableAdapter

    Public Shared Function GetDiscountApproval(ByVal estimateId As Long) As SC3070210DataSet.SC3070210DISCOUNTAPPROVALDataTable
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_Start", GetCurrentMethod.Name), True)

        Using query As New DBSelectQuery(Of SC3070210DataSet.SC3070210DISCOUNTAPPROVALDataTable)("SC3070210_001")

            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* SC3070210_001 */ ")
                .Append("    A.ESTIMATEID, ")
                .Append("    A.SEQNO, ")
                .Append("    A.STAFFACCOUNT, ")
                .Append("    B.USERNAME AS STAFFNAME, ")
                .Append("    A.REQUESTPRICE, ")
                .Append("    A.REASONID, ")
                .Append("    D.MSG_DLR AS REASON, ")
                .Append("    A.REQUESTDATE, ")
                .Append("    A.MANAGERACCOUNT, ")
                .Append("    C.USERNAME AS MANAGERNAME, ")
                .Append("    A.APPROVEDPRICE, ")
                .Append("    A.MANAGERMEMO, ")
                .Append("    A.APPROVEDDATE, ")
                .Append("    A.RESPONSEFLG, ")
                .Append("    A.NOTICEREQID, ")
                .Append("    CASE WHEN NVL(E.STATUS,'0') = '2' THEN '1' ELSE '0' END AS CANCELFLG, ")
                .Append("    CASE WHEN NVL(E.STATUS,'0') = '2' THEN E.SENDDATE ELSE NULL END AS CANCELDATE, ")
                .Append("    A.STAFFMEMO ")
                .Append("FROM ")
                .Append("    TBL_EST_DISCOUNTAPPROVAL A ")
                .Append("INNER JOIN ")
                .Append("    TBL_USERS B ON B.ACCOUNT = A.STAFFACCOUNT ")
                .Append("LEFT JOIN ")
                .Append("    TBL_USERS C ON C.ACCOUNT = A.MANAGERACCOUNT ")
                .Append("LEFT JOIN ")
                .Append("    TBL_REQUESTINFOMST D ON D.DLRCD = A.DLRCD AND D.REQCLASS = '02' AND D.ID = A.REASONID ")
                .Append("LEFT JOIN ")
                .Append("    (SELECT X.NOTICEREQID, X.STATUS, Y.SENDDATE FROM TBL_NOTICEREQUEST X INNER JOIN TBL_NOTICEINFO Y ON X.LASTNOTICEID = Y.NOTICEID) E ON E.NOTICEREQID = A.NOTICEREQID ")
                .Append("WHERE ")
                .Append("    A.ESTIMATEID = :ESTIMATEID ")
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Decimal, estimateId)

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name), True)
            Return query.GetData()

        End Using
    End Function

    Public Shared Function GetContracatApproval(ByVal estimateId As Long) As SC3070210DataSet.SC3070210CONTRACTAPPROVALDataTable
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_Start", GetCurrentMethod.Name), True)

        Using query As New DBSelectQuery(Of SC3070210DataSet.SC3070210CONTRACTAPPROVALDataTable)("SC3070210_002")

            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* SC3070210_002 */ ")
                .Append("    A.ESTIMATEID, ")
                .Append("    A.SEQNO, ")
                .Append("    A.STAFFACCOUNT, ")
                .Append("    B.USERNAME AS STAFFNAME, ")
                .Append("    A.REQUESTDATE, ")
                .Append("    A.MANAGERACCOUNT, ")
                .Append("    C.USERNAME AS MANAGERNAME, ")
                .Append("    A.MANAGERMEMO, ")
                .Append("    A.APPROVEDDATE, ")
                .Append("    A.RESPONSEFLG, ")
                .Append("    A.NOTICEREQID, ")
                .Append("    CASE WHEN NVL(E.STATUS,'0') = '2' THEN '1' ELSE '0' END AS CANCELFLG, ")
                .Append("    CASE WHEN NVL(E.STATUS,'0') = '2' THEN E.SENDDATE ELSE NULL END AS CANCELDATE, ")
                .Append("    A.STAFFMEMO ")
                .Append("FROM ")
                .Append("    TBL_EST_CONTRACTAPPROVAL A ")
                .Append("INNER JOIN ")
                .Append("    TBL_USERS B ON B.ACCOUNT = A.STAFFACCOUNT ")
                .Append("LEFT JOIN ")
                .Append("    TBL_USERS C ON C.ACCOUNT = A.MANAGERACCOUNT ")
                .Append("LEFT JOIN ")
                .Append("    (SELECT X.NOTICEREQID, X.STATUS, Y.SENDDATE FROM TBL_NOTICEREQUEST X INNER JOIN TBL_NOTICEINFO Y ON X.LASTNOTICEID = Y.NOTICEID) E ON E.NOTICEREQID = A.NOTICEREQID ")
                .Append("WHERE ")
                .Append("    A.ESTIMATEID = :ESTIMATEID ")
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Decimal, estimateId)

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name), True)
            Return query.GetData()

        End Using
    End Function

    Public Shared Function GetBookedAfterProcessCount(ByVal estimateId As Long) As Integer
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_Start", GetCurrentMethod.Name), True)

        Dim sql As New StringBuilder
        With sql
            .Append("SELECT /* SC3080210_003 */ ")
            .Append("     COUNT(1) ")
            .Append(" FROM ")
            .Append("     TBL_ESTIMATEINFO A ")
            .Append(" INNER JOIN ")
            .Append("     TB_T_AFTER_ODR B ON (B.SALES_ID = A.FLLWUPBOX_SEQNO) OR (B.DLR_CD = A.DLRCD AND B.SALESBKG_NUM = A.CONTRACTNO) ")
            .Append(" WHERE ")
            .Append("     A.ESTIMATEID = :ESTIMATEID AND A.CONTRACTFLG = '1' ")
        End With

        Using query As New DBSelectQuery(Of DataTable)("SC3080210_003")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Decimal, estimateId)

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name), True)
            Return query.GetCount()
        End Using
    End Function

    Public Shared Function GetBookedVehicle(ByVal estimateId As Long) As SC3070210DataSet.SC3070210VEHICLEDataTable
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_Start", GetCurrentMethod.Name), True)

        Using query As New DBSelectQuery(Of SC3070210DataSet.SC3070210VEHICLEDataTable)("SC3070210_004")

            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* SC3070210_004 */ ")
                .Append("    B.DLR_CD, ")
                .Append("    B.VCL_ID, ")
                .Append("    B.DELI_DATE ")
                .Append("FROM ")
                .Append("     TBL_ESTIMATEINFO A ")
                .Append(" INNER JOIN ")
                .Append("     TB_M_VEHICLE_DLR B ON B.DLR_CD = A.DLRCD AND B.SALESBKG_NUM = A.CONTRACTNO ")
                .Append("WHERE ")
                .Append("    A.ESTIMATEID = :ESTIMATEID AND A.CONTRACTFLG = '1' ")
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Decimal, estimateId)

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name), True)
            Return query.GetData()

        End Using
    End Function

    Public Shared Function GetRequestInfo(ByVal estimateId As Long) As SC3070210DataSet.SC3070210REQUESTDataTable
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_Start", GetCurrentMethod.Name), True)

        Using query As New DBSelectQuery(Of SC3070210DataSet.SC3070210REQUESTDataTable)("SC3070210_005")

            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* SC3070210_005 */ ")
                .Append("    C.REQ_ID, ")
                .Append("    C.REQ_STATUS ")
                .Append("FROM ")
                .Append("     TBL_ESTIMATEINFO A ")
                .Append(" INNER JOIN ")
                .Append("     TB_H_SALES B ON B.SALES_ID = A.FLLWUPBOX_SEQNO ")
                .Append(" INNER JOIN ")
                .Append("     TB_H_REQUEST C ON C.REQ_ID = B.REQ_ID ")
                .Append("WHERE ")
                .Append("    A.ESTIMATEID = :ESTIMATEID ")
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Decimal, estimateId)

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name), True)
            Return query.GetData()

        End Using
    End Function
End Class
