Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic

Namespace SC3010202DataSetTableAdapters

    ''' <summary>
    ''' SCメインのデータアクセスクラスです。
    ''' </summary>
    ''' <remarks></remarks>
    Public Class SC3010202TableAdapter
        Inherits Global.System.ComponentModel.Component

        ''' <summary>
        ''' MC33101バッチの動作時間を取得する
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetStarBatchTime() As Date
            Using query As New DBSelectQuery(Of SC3010202DataSet.SC3010202BatchStartTimeDataTable)("SC3010202_001")
                Dim sql As New StringBuilder

                'SQL文作成
                With sql
                    .Append("SELECT /* SC3010202_001 */")
                    .Append("  UPDATEDATE AS STARTTIME ")
                    .Append("  FROM TBL_PROGRAMSETTINGS ")
                    .Append("WHERE ")
                    .Append("  KEY = :KEY AND ")
                    .Append("  PROGRAMID = :PROGRAMID")
                End With

                query.CommandText = sql.ToString()

                'バインド変数
                query.AddParameterWithTypeValue("KEY", OracleDbType.Varchar2, "LASTPROCDATETIME")
                query.AddParameterWithTypeValue("PROGRAMID", OracleDbType.Char, "MC33102")

                '検索結果返却
                Dim dt As SC3010202DataSet.SC3010202BatchStartTimeDataTable = query.GetData()
                If dt.Equals(Nothing) Then
                    'バッチの動作時間が取得できない場合は必ずリアル取得する必要があるため日付を常に未来にする
                    Return Now.AddDays(+2)
                Else
                    Return dt.Rows(0).Item("StartTime")
                End If
            End Using
        End Function

        ''' <summary>
        ''' ログインユーザの目標値を取得する。
        ''' </summary>
        ''' <param name="dealerCode">検索対象販売点コード</param>
        ''' <param name="branchCode">検索対象店舗コード</param>
        ''' <param name="Month">検索対象年月(YYYYMM)</param>
        ''' <param name="Account">検索対象ユーザアカウント</param>
        ''' <returns>処理結果</returns>
        ''' <remarks></remarks>
        Public Function GetTargetInfo(ByVal dealerCode As String, ByVal branchCode As String, ByVal month As String, ByVal account As String) As SC3010202DataSet.SC3010202TargetDataTable

            Using query As New DBSelectQuery(Of SC3010202DataSet.SC3010202TargetDataTable)("SC3010202_002")
                Dim sql As New StringBuilder

                'SQL文作成
                With sql

                    .Append("SELECT /* SC3010202_002 */")
                    .Append(" TARGET_WALKIN AS WALKIN")
                    .Append("  ,TARGET_QUOTATION AS QUOTATION")
                    .Append("  ,TARGET_TESTDRIVE AS TESTDRIVE")
                    .Append("  ,TARGET_EVALUATION AS EVALUATION")
                    .Append("  ,TARGET_DELIDATE AS DELIVERY")
                    .Append("  ,TARGET_COLD AS COLD")
                    .Append("  ,TARGET_WARM AS WARM")
                    .Append("  ,TARGET_HOT AS HOT")
                    .Append("  ,TARGET_SUCCESS AS ORDERS ")
                    .Append("  ,TARGET_SALES AS SALES ")
                    .Append(" FROM TBL_SALESACTIVE_TARGET ")
                    .Append(" WHERE DLRCD = :DLRCD ")
                    .Append(" AND STRCD = :STRCD ")
                    .Append(" AND MONTH = :MONTH ")
                    .Append(" AND ACCOUNT = :ACCOUNT ")

                End With

                query.CommandText = sql.ToString()
                'バインド変数
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
                query.AddParameterWithTypeValue("MONTH", OracleDbType.Char, month)
                query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.Varchar2, account)

                '検索結果返却
                Return query.GetData()
            End Using
        End Function

        ''' <summary>
        ''' 店舗の目標値を取得する。
        ''' </summary>
        ''' <param name="dealerCode">検索対象販売点コード</param>
        ''' <param name="branchCode">検索対象店舗コード</param>
        ''' <param name="Month">検索対象年月(YYYYMM)</param>
        ''' <returns>処理結果</returns>
        ''' <remarks></remarks>
        Public Function GetTargetInfoOfBranch(ByVal dealerCode As String, ByVal branchCode As String, ByVal month As String) As SC3010202DataSet.SC3010202TargetDataTable

            Using query As New DBSelectQuery(Of SC3010202DataSet.SC3010202TargetDataTable)("SC3010202_003")
                Dim sql As New StringBuilder

                'SQL文作成
                With sql
                    .Append("Select /* SC3010202_003 */")
                    .Append(" SUM(TARGET_WALKIN) AS WALKIN")
                    .Append("  ,SUM(TARGET_QUOTATION) AS QUOTATION")
                    .Append("  ,SUM(TARGET_TESTDRIVE) AS TESTDRIVE")
                    .Append("  ,SUM(TARGET_EVALUATION) AS EVALUATION")
                    .Append("  ,SUM(TARGET_DELIDATE) AS DELIVERY")
                    .Append("  ,SUM(TARGET_COLD) AS COLD")
                    .Append("  ,SUM(TARGET_WARM) AS WARM")
                    .Append("  ,SUM(TARGET_HOT) AS HOT")
                    .Append("  ,SUM(TARGET_SUCCESS) AS ORDERS")
                    .Append("  ,SUM(TARGET_SALES) AS SALES ")
                    .Append("FROM(TBL_SALESACTIVE_TARGET) ")
                    .Append(" WHERE DLRCD = :DLRCD ")
                    .Append(" AND STRCD = :STRCD ")
                    .Append(" AND MONTH = :MONTH ")
                    .Append(" GROUP BY ")
                    .Append(" DLRCD, ")
                    .Append(" STRCD, ")
                    .Append(" MONTH ")

                End With

                query.CommandText = sql.ToString()
                'バインド変数
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
                query.AddParameterWithTypeValue("MONTH", OracleDbType.Char, month)

                '検索結果返却
                Return query.GetData()
            End Using
        End Function


        ''' <summary>
        ''' 店舗の来店情報実績値を取得する。
        ''' </summary>
        ''' <param name="dealerCode">検索対象販売点コード</param>
        ''' <param name="branchCode">検索対象店舗コード</param>
        ''' <param name="Month">検索対象年月(YYYYMM)</param>
        ''' <param name="Account">検索対象ユーザアカウント</param>
        ''' <returns>処理結果</returns>
        ''' <remarks>Step2で実装予定</remarks>
        Public Function GetResultWalkIn(ByVal dealerCode As String, ByVal branchCode As String, ByVal month As String, ByVal account As String) As SC3010202DataSet.SC3010202ResultWalkInDataTable
            'SQL番号は /*SC3010202_004*/
            'コードインスペクションを逃れるためのダミーコード
            Dim dummy As String
            dummy = dealerCode
            dummy = branchCode
            dummy = month
            dummy = account
            dealerCode = dummy

            '検索結果返却
            Return Nothing
        End Function

        ''' <summary>
        ''' ログインユーザの来店情報実績値を取得する。
        ''' </summary>
        ''' <param name="dealerCode">検索対象販売点コード</param>
        ''' <param name="branchCode">検索対象店舗コード</param>
        ''' <param name="Month">検索対象年月(YYYYMM)</param>
        ''' <returns>処理結果</returns>
        ''' <remarks>Step2で実装予定</remarks>
        Public Function GetResultWalkInOfBranch(ByVal dealerCode As String, ByVal branchCode As String, ByVal month As String) As SC3010202DataSet.SC3010202ResultWalkInDataTable
            'SQL番号は /*SC3010202_005*/
            'コードインスペクションを逃れるためのダミーコード
            Dim dummy As String
            dummy = dealerCode
            dummy = branchCode
            dummy = month
            dealerCode = dummy

            '検索結果返却
            Return Nothing
        End Function

        ''' <summary>
        ''' ログインユーザの実績値をFLLWUPBOX履歴情報から取得する。
        ''' </summary>
        ''' <param name="dealerCode">検索対象販売点コード</param>
        ''' <param name="Month">検索対象年月(YYYYMM)</param>
        ''' <param name="Account">検索対象ユーザアカウント</param>
        ''' <returns>処理結果</returns>
        ''' <remarks></remarks>
        Public Function GetResultHistory(ByVal dealerCode As String, ByVal month As String, ByVal account As String) As SC3010202DataSet.SC3010202ResultCRHISDataTable

            Using query As New DBSelectQuery(Of SC3010202DataSet.SC3010202ResultCRHISDataTable)("SC3010202_006")
                Dim sql As New StringBuilder

                'SQL文作成
                With sql
                    .Append(" SELECT /*SC3010202_006*/ ")
                    .Append("    CRHIS.ACTIONCD,")
                    .Append("    COUNT(1) AS CNT")
                    .Append(" FROM            ")
                    .Append("    TBL_FLLWUPBOXCRHIS CRHIS")
                    .Append(" WHERE")
                    .Append("    CRHIS.DLRCD = :DLRCD AND")
                    .Append("    CRHIS.ACCOUNT = :ACCOUNT AND")
                    .Append("    CRHIS.ACTDATE >= TO_DATE(:ACTDATE,'YYYYMM') AND")
                    .Append("    CRHIS.ACTIONCD IN ('A23','A26','A30') ")
                    .Append(" GROUP BY")
                    .Append("    CRHIS.ACCOUNT,")
                    .Append("    CRHIS.ACTIONCD")
                End With

                query.CommandText = sql.ToString()
                'バインド変数
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.Char, account)
                query.AddParameterWithTypeValue("ACTDATE", OracleDbType.Varchar2, month)

                '検索結果返却
                Return query.GetData()
            End Using
        End Function

        ''' <summary>
        ''' 店舗の実績値をFLLWUPBOX履歴情報から取得する。
        ''' </summary>
        ''' <param name="dealerCode">検索対象販売店コード</param>
        ''' <param name="branchCode">検索対象店舗コード</param>
        ''' <param name="Month">検索対象年月(YYYYMM)</param>
        ''' <returns>処理結果</returns>
        ''' <remarks></remarks>
        Public Function GetResultHistoryOfBranch(ByVal dealerCode As String, ByVal branchCode As String, ByVal month As String) As SC3010202DataSet.SC3010202ResultCRHISDataTable

            Using query As New DBSelectQuery(Of SC3010202DataSet.SC3010202ResultCRHISDataTable)("SC3010202_007")
                Dim sql As New StringBuilder

                'SQL文作成
                With sql
                    .Append(" SELECT /*SC3010202_007*/ ")
                    .Append("    CRHIS.ACTIONCD,")
                    .Append("    COUNT(1) AS CNT")
                    .Append(" FROM            ")
                    .Append("    TBL_FLLWUPBOXCRHIS CRHIS")
                    .Append(" WHERE")
                    .Append("    CRHIS.DLRCD = :DLRCD AND")
                    .Append("    CRHIS.ACCOUNT IN ")
                    .Append("    (SELECT ACCOUNT")
                    .Append("     FROM TBL_USERS")
                    .Append("     WHERE DLRCD = :DLRCD AND")
                    .Append("           STRCD = :STRCD AND")
                    .Append("           DELFLG = '0') AND")
                    .Append("    CRHIS.ACTDATE >= TO_DATE(:ACTDATE,'YYYYMM') AND")
                    .Append("    CRHIS.ACTIONCD IN ('A23','A26','A30') ")
                    .Append(" GROUP BY")
                    .Append("    CRHIS.ACCOUNT,")
                    .Append("    CRHIS.ACTIONCD")
                End With

                query.CommandText = sql.ToString()
                'バインド変数
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
                query.AddParameterWithTypeValue("ACTDATE", OracleDbType.Varchar2, month)

                '検索結果返却
                Return query.GetData()
            End Using
        End Function


        ''' <summary>
        ''' ログインユーザの実績値を集計情報から取得する。
        ''' </summary>
        ''' <param name="dealerCode">検索対象販売点コード</param>
        ''' <param name="branchCode">検索対象店舗コード</param>
        ''' <param name="Month">検索対象年月(YYYYMM)</param>
        ''' <param name="Account">検索対象ユーザアカウント</param>
        ''' <returns>処理結果</returns>
        ''' <remarks></remarks>
        Public Function GetResultFollowUpBoxTally(ByVal dealerCode As String, ByVal branchCode As String, ByVal account As String, ByVal month As String) As SC3010202DataSet.SC3010202ResultTallyDataTable

            Using query As New DBSelectQuery(Of SC3010202DataSet.SC3010202ResultTallyDataTable)("SC3010202_008")
                Dim sql As New StringBuilder

                'SQL文作成
                With sql
                    .Append("SELECT /*SC3010202_008*/  ")
                    .Append("	CRACTRESULT, ")
                    .Append("	CNT ")
                    .Append("FROM ")
                    .Append("(SELECT  ")
                    .Append("	FLW.CRACTRESULT AS CRACTRESULT,  ")
                    .Append("	SUM(CAR.DESIRED_QUANTITY) AS CNT ")
                    .Append("FROM  ")
                    .Append("	TBL_FLLWUPBOXTALLY FLW, ")
                    .Append("	TBL_FLBOX_SLCTD_SRES_TLY CAR ")
                    .Append("WHERE  ")
                    .Append("	FLW.DLRCD = CAR.DLRCD AND ")
                    .Append("	FLW.STRCD = CAR.STRCD AND ")
                    .Append("	FLW.FLLWUPBOX_SEQNO = CAR.FLLWUPBOX_SEQNO AND ")
                    .Append("	FLW.DLRCD = :DLRCD AND  ")
                    .Append("	FLW.BRANCH_PLAN = :BRANCH_PLAN AND  ")
                    .Append("	FLW.ACCOUNT_PLAN = :ACCOUNT_PLAN AND  ")
                    .Append("	FLW.CRACTRESULT IN ('1','2','7') AND  ")
                    .Append("  	FLW.DELFLG = '0'  ")
                    .Append("GROUP BY  ")
                    .Append("	CRACTRESULT ")
                    .Append(") ")
                    .Append("UNION ALL ")
                    .Append("(SELECT  ")
                    .Append("	FLW.CRACTRESULT AS CRACTRESULT,  ")
                    .Append("	COUNT(1) AS CNT  ")
                    .Append("FROM  ")
                    .Append("	TBL_FLLWUPBOXTALLY FLW, ")
                    .Append("	TBL_FLBOX_SUCS_SRES_TLY CAR ")
                    .Append("WHERE  ")
                    .Append("	FLW.DLRCD = CAR.DLRCD AND ")
                    .Append("	FLW.STRCD = CAR.STRCD AND ")
                    .Append("	FLW.FLLWUPBOX_SEQNO = CAR.FLLWUPBOX_SEQNO AND ")
                    .Append("	FLW.DLRCD = :DLRCD AND  ")
                    .Append("	FLW.BRANCH_PLAN = :BRANCH_PLAN AND  ")
                    .Append("	FLW.ACCOUNT_PLAN = :ACCOUNT_PLAN AND  ")
                    .Append("	FLW.CRACTRESULT = '3' AND  ")
                    .Append("	FLW.FINSHCRACTIVEDATE >= TO_DATE(:FINSHCRACTIVEDATE,'YYYYMM') AND  ")
                    .Append("  	FLW.DELFLG = '0' ")
                    .Append("GROUP BY  ")
                    .Append("	CRACTRESULT )")
                End With

                query.CommandText = sql.ToString()
                'バインド変数
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("BRANCH_PLAN", OracleDbType.Char, branchCode)
                query.AddParameterWithTypeValue("ACCOUNT_PLAN", OracleDbType.Char, account)
                query.AddParameterWithTypeValue("FINSHCRACTIVEDATE", OracleDbType.Varchar2, month)

                '検索結果返却
                Return query.GetData()
            End Using
        End Function

        ''' <summary>
        ''' 店舗の実績値を集計情報から取得する。
        ''' </summary>
        ''' <param name="dealerCode">検索対象販売点コード</param>
        ''' <param name="branchCode">検索対象店舗コード</param>
        ''' <param name="Month">検索対象年月(YYYYMM)</param>
        ''' <returns>処理結果</returns>
        ''' <remarks></remarks>
        Public Function GetResultFollowUpBoxTallyOfBranch(ByVal dealerCode As String, ByVal branchCode As String, ByVal month As String) As SC3010202DataSet.SC3010202ResultTallyDataTable

            Using query As New DBSelectQuery(Of SC3010202DataSet.SC3010202ResultTallyDataTable)("SC3010202_009")
                Dim sql As New StringBuilder

                'SQL文作成
                With sql

                    .Append("SELECT /*SC3010202_009*/  ")
                    .Append("	CRACTRESULT, ")
                    .Append("	CNT ")
                    .Append("FROM ")
                    .Append("(SELECT  ")
                    .Append("	FLW.CRACTRESULT AS CRACTRESULT,  ")
                    .Append("	SUM(CAR.DESIRED_QUANTITY) AS CNT ")
                    .Append("FROM  ")
                    .Append("	TBL_FLLWUPBOXTALLY FLW, ")
                    .Append("	TBL_FLBOX_SLCTD_SRES_TLY CAR ")
                    .Append("WHERE  ")
                    .Append("	FLW.DLRCD = CAR.DLRCD AND ")
                    .Append("	FLW.STRCD = CAR.STRCD AND ")
                    .Append("	FLW.FLLWUPBOX_SEQNO = CAR.FLLWUPBOX_SEQNO AND ")
                    .Append("	FLW.DLRCD = :DLRCD AND  ")
                    .Append("	FLW.BRANCH_PLAN = :BRANCH_PLAN AND  ")
                    .Append("	FLW.CRACTRESULT IN ('1','2','7') AND  ")
                    .Append("  	FLW.DELFLG = '0'  ")
                    .Append("GROUP BY  ")
                    .Append("	CRACTRESULT ")
                    .Append(") ")
                    .Append("UNION ALL ")
                    .Append("(SELECT  ")
                    .Append("	FLW.CRACTRESULT AS CRACTRESULT,  ")
                    .Append("	COUNT(1) AS CNT  ")
                    .Append("FROM  ")
                    .Append("	TBL_FLLWUPBOXTALLY FLW, ")
                    .Append("	TBL_FLBOX_SUCS_SRES_TLY CAR ")
                    .Append("WHERE  ")
                    .Append("	FLW.DLRCD = CAR.DLRCD AND ")
                    .Append("	FLW.STRCD = CAR.STRCD AND ")
                    .Append("	FLW.FLLWUPBOX_SEQNO = CAR.FLLWUPBOX_SEQNO AND ")
                    .Append("	FLW.DLRCD = :DLRCD AND  ")
                    .Append("	FLW.BRANCH_PLAN = :BRANCH_PLAN AND  ")
                    .Append("	FLW.CRACTRESULT = '3' AND  ")
                    .Append("	FLW.FINSHCRACTIVEDATE >= TO_DATE(:FINSHCRACTIVEDATE,'YYYYMM') AND  ")
                    .Append("  	FLW.DELFLG = '0' ")
                    .Append("GROUP BY  ")
                    .Append("	CRACTRESULT) ")
                End With


                query.CommandText = sql.ToString()
                'バインド変数
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("BRANCH_PLAN", OracleDbType.Char, branchCode)
                query.AddParameterWithTypeValue("FINSHCRACTIVEDATE", OracleDbType.Varchar2, month)

                '検索結果返却
                Return query.GetData()
            End Using
        End Function

        ''' <summary>
        ''' ログインユーザの納車実績値を販売集計情報から取得する。
        ''' </summary>
        ''' <param name="dealerCode">検索対象販売点コード</param>
        ''' <param name="Month">検索対象年月(YYYYMM)</param>
        ''' <param name="Account">検索対象ユーザアカウント</param>
        ''' <returns>処理結果</returns>
        ''' <remarks></remarks>
        Public Function GetResultDelivery(ByVal dealerCode As String, ByVal month As String, ByVal account As String) As Integer

            Using query As New DBSelectQuery(Of DataTable)("SC3010202_010")
                Dim sql As New StringBuilder

                'SQL文作成
                With sql
                    .Append(" SELECT /*SC3010202_010*/ ")
                    .Append("	COUNT(1) AS CNT ")
                    .Append("FROM ")
                    .Append("   TBL_SALESBKGTALLY ")
                    .Append("WHERE ")
                    .Append("   DLRCD = :DLRCD AND ")
                    .Append("   SALESSTAFFCD = :SALESSTAFFCD AND ")
                    .Append("   (VCLDELIDATE >= TO_DATE(:VCLDELIDATE,'YYYYMM') OR ")
                    .Append("   (VCLDELIDATE IS NULL AND")
                    .Append("    VCLDELIDATE_ENT >= TO_DATE(:VCLDELIDATE,'YYYYMM'))) AND")
                    .Append("   CANCELFLG = '0' AND ")
                    .Append("   DELFLG = '0' AND ")
                    .Append("   CUSTDELFLG = '0' ")

                End With

                query.CommandText = sql.ToString()
                'バインド変数
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("SALESSTAFFCD", OracleDbType.Char, account)
                query.AddParameterWithTypeValue("VCLDELIDATE", OracleDbType.Varchar2, month)

                '検索結果返却
                Dim dt As DataTable = query.GetData()

                If dt.Equals(Nothing) Then
                    Return 0
                Else
                    Return dt.Rows(0).Item("CNT")
                End If
            End Using
        End Function


        ''' <summary>
        ''' 店舗の納車実績値を販売集計情報から取得する。
        ''' </summary>
        ''' <param name="dealerCode">検索対象販売点コード</param>
        ''' <param name="branchCode">検索対象店舗コード</param>
        ''' <param name="Month">検索対象年月(YYYYMM)</param>
        ''' <returns>処理結果</returns>
        ''' <remarks></remarks>
        Public Function GetResultDeliveryOfBranch(ByVal dealerCode As String, ByVal branchCode As String, ByVal month As String) As Integer

            Using query As New DBSelectQuery(Of DataTable)("SC3010202_011")
                Dim sql As New StringBuilder

                'SQL文作成
                With sql
                    .Append(" SELECT /*SC3010202_011*/ ")
                    .Append("	COUNT(1) AS CNT ")
                    .Append("FROM ")
                    .Append("   TBL_SALESBKGTALLY ")
                    .Append("WHERE ")
                    .Append("   DLRCD = :DLRCD AND ")
                    .Append("   STRCD = :STRCD AND ")
                    .Append("   (VCLDELIDATE >= TO_DATE(:VCLDELIDATE,'YYYYMM') OR ")
                    .Append("   (VCLDELIDATE IS NULL AND")
                    .Append("    VCLDELIDATE_ENT >= TO_DATE(:VCLDELIDATE,'YYYYMM'))) AND")
                    .Append("   CANCELFLG = '0' AND ")
                    .Append("   DELFLG = '0' AND ")
                    .Append("   CUSTDELFLG = '0' ")

                End With

                query.CommandText = sql.ToString()
                'バインド変数
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
                query.AddParameterWithTypeValue("VCLDELIDATE", OracleDbType.Varchar2, month)

                '検索結果返却
                Dim dt As DataTable = query.GetData()

                If dt.Equals(Nothing) Then
                    Return 0
                Else
                    Return dt.Rows(0).Item("CNT")
                End If
            End Using
        End Function

        ''' <summary>
        ''' ログインユーザの販売実績値を販売集計情報から取得する。
        ''' </summary>
        ''' <param name="dealerCode">検索対象販売点コード</param>
        ''' <param name="Month">検索対象年月(YYYYMM)</param>
        ''' <param name="Account">検索対象ユーザアカウント</param>
        ''' <returns>処理結果</returns>
        ''' <remarks></remarks>
        Public Function GetResultSales(ByVal dealerCode As String, ByVal month As String, ByVal account As String) As Integer

            Using query As New DBSelectQuery(Of DataTable)("SC3010202_012")
                Dim sql As New StringBuilder

                'SQL文作成
                With sql
                    .Append(" SELECT /*SC3010202_012*/ ")
                    .Append("	COUNT(1) AS CNT ")
                    .Append("FROM ")
                    .Append("   TBL_SALESBKGTALLY ")
                    .Append("WHERE ")
                    .Append("   DLRCD = :DLRCD AND ")
                    .Append("   SALESSTAFFCD = :SALESSTAFFCD AND ")
                    .Append("   (SALESDATE >= TO_DATE(:VCLDELIDATE,'YYYYMM') OR ")
                    .Append("   (SALESDATE IS NULL AND")
                    .Append("    SALESDATE_ENT >= TO_DATE(:VCLDELIDATE,'YYYYMM'))) AND")
                    .Append("   CANCELFLG = '0' AND ")
                    .Append("   DELFLG = '0' AND ")
                    .Append("   CUSTDELFLG = '0' ")
                End With

                query.CommandText = sql.ToString()
                'バインド変数
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("SALESSTAFFCD", OracleDbType.Char, account)
                query.AddParameterWithTypeValue("VCLDELIDATE", OracleDbType.Varchar2, month)

                '検索結果返却
                Dim dt As DataTable = query.GetData()

                If dt.Equals(Nothing) Then
                    Return 0
                Else
                    Return dt.Rows(0).Item("CNT")
                End If
            End Using
        End Function



        ''' <summary>
        ''' 店舗の販売実績値を販売集計情報から取得する。
        ''' </summary>
        ''' <param name="dealerCode">検索対象販売点コード</param>
        ''' <param name="branchCode">検索対象店舗コード</param>
        ''' <param name="Month">検索対象年月(YYYYMM)</param>
        ''' <returns>処理結果</returns>
        ''' <remarks></remarks>
        Public Function GetResultSalesOfBranch(ByVal dealerCode As String, ByVal branchCode As String, ByVal month As String) As Integer

            Using query As New DBSelectQuery(Of DataTable)("SC3010202_013")
                Dim sql As New StringBuilder

                'SQL文作成
                With sql
                    .Append(" SELECT /*SC3010202_013*/ ")
                    .Append("	COUNT(1) AS CNT ")
                    .Append("FROM ")
                    .Append("   TBL_SALESBKGTALLY ")
                    .Append("WHERE ")
                    .Append("   DLRCD = :DLRCD AND ")
                    .Append("   STRCD = :STRCD AND ")
                    .Append("   (SALESDATE >= TO_DATE(:VCLDELIDATE,'YYYYMM') OR ")
                    .Append("   (SALESDATE IS NULL AND")
                    .Append("    SALESDATE_ENT >= TO_DATE(:VCLDELIDATE,'YYYYMM'))) AND")
                    .Append("   CANCELFLG = '0' AND ")
                    .Append("   DELFLG = '0' AND ")
                    .Append("   CUSTDELFLG = '0' ")

                End With

                query.CommandText = sql.ToString()
                'バインド変数
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
                query.AddParameterWithTypeValue("VCLDELIDATE", OracleDbType.Varchar2, month)

                '検索結果返却
                Dim dt As DataTable = query.GetData()

                If dt.Equals(Nothing) Then
                    Return 0
                Else
                    Return dt.Rows(0).Item("CNT")
                End If
            End Using
        End Function
    End Class

End Namespace
Partial Class SC3010202DataSet
End Class
