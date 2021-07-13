'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3010205DataSet.vb
'──────────────────────────────────
'機能： ダッシュボード
'補足： 
'作成： 
'更新： 2013/05/27 TMEJ m.asano 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 $01
'更新： 2014/05/30 TMEJ y.gotoh 受注後フォロー機能開発 $02
'更新： 2015/01/16 TMEJ y.gotoh 組織IDの型変更 $03
'更新： 2020/02/14 NSK  m.sakamoto (トライ店システム評価)月別販売目標の設定機能におけるセキュリティ向上検証 $04
'──────────────────────────────────

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

#Region "定数"

#Region "来店実績ステータス"

        ''' <summary>
        ''' 来店実績ステータス（07:商談中）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const VisitStatusSalesStart As String = "07"

        ''' <summary>
        ''' 来店実績ステータス（08:商談終了）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const VisitStatusSalesEnd As String = "08"

#End Region

#End Region

#Region "メソッド"

#Region "バッチ動作時間取得"
        ''' <summary>
        ''' MC3C10102バッチの動作時間を取得する
        ''' </summary>
        ''' <param name="dealerCode">販売点コード</param>
        ''' <returns>MC3C10102バッチの動作時間</returns>
        ''' <remarks></remarks>
        Public Function GetStarBatchTime(ByVal dealerCode As String) As Date

            Logger.Info("GetStarBatchTime Start")

            Using query As New DBSelectQuery(Of SC3010202DataSet.SC3010202BatchStartTimeDataTable)("SC3010202_001")
                Dim sql As New StringBuilder
                '$02 受注後フォロー機能開発 START
                'SQL文作成
                With sql
                    .Append(" SELECT /* SC3010202_001 */ ")
                    .Append("        SETTING_VAL AS STARTTIME ")
                    .Append("   FROM TB_M_PROGRAM_SETTING ")
                    .Append("  WHERE PROGRAM_CD = :PROGRAM_CD ")
                    .Append("    AND SETTING_SECTION = :SETTING_SECTION ")
                    .Append("    AND SETTING_KEY = :SETTING_KEY ")
                End With

                query.CommandText = sql.ToString()

                'バインド変数
                query.AddParameterWithTypeValue("PROGRAM_CD", OracleDbType.NVarchar2, "MC3C10102")
                query.AddParameterWithTypeValue("SETTING_SECTION", OracleDbType.NVarchar2, dealerCode)
                query.AddParameterWithTypeValue("SETTING_KEY", OracleDbType.NVarchar2, "LAST_PROC_DATETIME")
                '$02 受注後フォロー機能開発 END

                '検索結果返却
                Dim dt As SC3010202DataSet.SC3010202BatchStartTimeDataTable = query.GetData()
                If dt.Count = 0 Then
                    'バッチの動作時間が取得できない場合は必ずリアル取得する必要があるため日付を常に未来にする
                    Logger.Info("GetStarBatchTime End Ret[" & Now.AddDays(+2) & "]")
                    Return Now.AddDays(+2)
                Else
                    Logger.Info("GetStarBatchTime End Ret[" & dt.Rows(0).Item("StartTime") & "]")
                    Return dt.Rows(0).Item("StartTime")
                End If
            End Using
        End Function

#End Region

#Region "当月目標情報取得"

        ''' <summary>
        ''' ログインユーザの目標値を取得する。
        ''' </summary>
        ''' <param name="dealerCode">検索対象販売点コード</param>
        ''' <param name="branchCode">検索対象店舗コード</param>
        ''' <param name="Month">検索対象年月(YYYYMM)</param>
        ''' <param name="Account">検索対象ユーザアカウント</param>
        ''' <returns>処理結果</returns>
        ''' <remarks></remarks>
        Public Function GetTargetInfo(ByVal dealerCode As String, ByVal branchCode As String, ByVal month As String, _
                                      ByVal account As String) As SC3010202DataSet.SC3010202TargetDataTable

            Logger.Info("GetTargetInfo Start Param[dealerCode=" & dealerCode & _
            ", branchCode=" & branchCode & ", month=" & month & _
            ", account=" & account & "]")

            'm.sakamoto (トライ店システム評価)月別販売目標の設定機能におけるセキュリティ向上検証 START
            Dim tempYear As String = month.Substring(0, 4)
            Dim tempMonth As String = month.Substring(4)
            Dim targetYearMonth As Date = New Date(tempYear, tempMonth, 1)
            'm.sakamoto (トライ店システム評価)月別販売目標の設定機能におけるセキュリティ向上検証 END

            Dim dt As SC3010202DataSet.SC3010202TargetDataTable = Nothing

            Using query As New DBSelectQuery(Of SC3010202DataSet.SC3010202TargetDataTable)("SC3010202_002")
                Dim sql As New StringBuilder

                'SQL文作成
                With sql

                    'm.sakamoto (トライ店システム評価)月別販売目標の設定機能におけるセキュリティ向上検証 START
                    '$02 受注後フォロー機能開発 START
                    '.Append(" SELECT /* SC3010202_002 */ ")
                    '.Append("        TARGET_WALKIN AS WALKIN ")
                    '.Append("      , TARGET_TESTDRIVE AS TESTDRIVE ")
                    '.Append("      , TARGET_EVALUATION AS EVALUATION ")
                    '.Append("      , TARGET_DELIDATE AS DELIVERY ")
                    '.Append("      , TARGET_SUCCESS AS ORDERS ")
                    '.Append("   FROM TBL_SALESACTIVE_TARGET ")
                    '.Append("  WHERE DLRCD = :DLRCD ")
                    '.Append("    AND STRCD = :STRCD ")
                    '.Append("    AND MONTH = :MONTH ")
                    '.Append("    AND ACCOUNT = :ACCOUNT ")
                    '$02 受注後フォロー機能開発 END

                    .Append(" SELECT /* SC3010202_002 */ ")
                    .Append("        WALKIN_TARGET_VAL AS WALKIN ")
                    .Append("      , TESTDRIVE_TARGET_VAL AS TESTDRIVE ")
                    .Append("      , ASSMNT_TARGET_VAL AS EVALUATION ")
                    .Append("      , DELI_TARGET_VAL AS DELIVERY ")
                    .Append("      , SUCCESS_TARGET_VAL AS ORDERS ")
                    .Append("   FROM TB_M_SLS_STF_ACT_TARGET ")
                    .Append("  WHERE DLR_CD = :DLR_CD ")
                    .Append("    AND BRN_CD = :BRN_CD ")
                    .Append("    AND TARGET_YEARMONTH = :TARGET_YEARMONTH ")
                    .Append("    AND STF_CD = :STF_CD ")
                    'm.sakamoto (トライ店システム評価)月別販売目標の設定機能におけるセキュリティ向上検証 END

                End With

                query.CommandText = sql.ToString()

                'm.sakamoto (トライ店システム評価)月別販売目標の設定機能におけるセキュリティ向上検証 START
                ''バインド変数
                'query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
                'query.AddParameterWithTypeValue("MONTH", OracleDbType.Char, month)
                'query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.Varchar2, account)

                'バインド変数
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.Varchar2, dealerCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.Varchar2, branchCode)
                query.AddParameterWithTypeValue("TARGET_YEARMONTH", OracleDbType.Date, targetYearMonth)
                query.AddParameterWithTypeValue("STF_CD", OracleDbType.Varchar2, account)
                'm.sakamoto (トライ店システム評価)月別販売目標の設定機能におけるセキュリティ向上検証 END

                'クエリ実行
                dt = query.GetData()
            End Using

            Logger.Info("GetTargetInfo End Ret[" & dt.TableName & "[Count =" & dt.Count & "]]")
            Return dt

        End Function

        '$02 受注後フォロー機能開発 START
        ''' <summary>
        ''' チームの目標値を取得する。
        ''' </summary>
        ''' <param name="dealerCode">検索対象販売点コード</param>
        ''' <param name="branchCode">検索対象店舗コード</param>
        ''' <param name="month">検索対象年月(YYYYMM)</param>
        ''' <param name="orgnzIdList">組織IDリスト</param>
        ''' <returns>処理結果</returns>
        ''' <remarks></remarks>
        Public Function GetTargetInfoOfTeam(ByVal dealerCode As String, ByVal branchCode As String, ByVal month As String, _
                                            ByVal orgnzIdList As List(Of String)) As SC3010202DataSet.SC3010202TargetDataTable

            Logger.Info("GetTargetInfoOfTeam Start Param[dealerCode=" & dealerCode & _
                        ", branchCode=" & branchCode & ", month=" & month & _
                        ", orgnzIdList=" & String.Join(",", orgnzIdList) & "]")

            'm.sakamoto (トライ店システム評価)月別販売目標の設定機能におけるセキュリティ向上検証 START
            Dim tempYear As String = month.Substring(0, 4)
            Dim tempMonth As String = month.Substring(4)
            Dim targetYearMonth As Date = New Date(tempYear, tempMonth, 1)
            'm.sakamoto (トライ店システム評価)月別販売目標の設定機能におけるセキュリティ向上検証 END

            Dim dt As SC3010202DataSet.SC3010202TargetDataTable = Nothing

            Using query As New DBSelectQuery(Of SC3010202DataSet.SC3010202TargetDataTable)("SC3010202_003")

                Dim sql As New StringBuilder

                'SQL文作成
                With sql

                    'm.sakamoto (トライ店システム評価)月別販売目標の設定機能におけるセキュリティ向上検証 START
                    '.Append(" SELECT /* SC3010202_003 */ ")
                    '.Append("        SUM(TARGET_WALKIN) AS WALKIN ")
                    '.Append("      , SUM(TARGET_TESTDRIVE) AS TESTDRIVE ")
                    '.Append("      , SUM(TARGET_EVALUATION) AS EVALUATION ")
                    '.Append("      , SUM(TARGET_DELIDATE) AS DELIVERY ")
                    '.Append("      , SUM(TARGET_SUCCESS) AS ORDERS ")
                    '.Append("   FROM TBL_SALESACTIVE_TARGET ")
                    '.Append("  WHERE DLRCD = :DLRCD ")
                    '.Append("    AND STRCD = :STRCD ")
                    '.Append("    AND MONTH = :MONTH ")
                    '.Append("    AND ACCOUNT IN ( ")
                    '.Append("     SELECT STF_CD ")
                    '.Append("       FROM TB_M_STAFF ")
                    '.Append("      WHERE DLR_CD = :DLRCD ")
                    '.Append("        AND BRN_CD = :STRCD ")
                    '.Append("        AND INUSE_FLG = '1' ")
                    '.Append("        AND ORGNZ_ID IN ( ")
                    '.Append(ConvertOrgnzIdStr(orgnzIdList))
                    '.Append("            )")
                    '.Append("        )")
                    '.Append("  GROUP BY DLRCD, STRCD, MONTH")

                    .Append(" SELECT /* SC3010202_003 */ ")
                    .Append("        SUM(WALKIN_TARGET_VAL) AS WALKIN ")
                    .Append("      , SUM(TESTDRIVE_TARGET_VAL) AS TESTDRIVE ")
                    .Append("      , SUM(ASSMNT_TARGET_VAL) AS EVALUATION ")
                    .Append("      , SUM(DELI_TARGET_VAL) AS DELIVERY ")
                    .Append("      , SUM(SUCCESS_TARGET_VAL) AS ORDERS ")
                    .Append("   FROM TB_M_SLS_STF_ACT_TARGET ")
                    .Append("  WHERE DLR_CD = :DLR_CD ")
                    .Append("    AND BRN_CD = :BRN_CD ")
                    .Append("    AND TARGET_YEARMONTH = :TARGET_YEARMONTH ")
                    .Append("    AND STF_CD IN ( ")
                    .Append("     SELECT STF_CD ")
                    .Append("       FROM TB_M_STAFF ")
                    .Append("      WHERE DLR_CD = :DLR_CD ")
                    .Append("        AND BRN_CD = :BRN_CD ")
                    .Append("        AND INUSE_FLG = '1' ")
                    .Append("        AND ORGNZ_ID IN ( ")
                    .Append(ConvertOrgnzIdStr(orgnzIdList))
                    .Append("            )")
                    .Append("        )")
                    .Append("  GROUP BY DLR_CD, BRN_CD, TARGET_YEARMONTH")
                    'm.sakamoto (トライ店システム評価)月別販売目標の設定機能におけるセキュリティ向上検証 END

                End With

                query.CommandText = sql.ToString()

                'm.sakamoto (トライ店システム評価)月別販売目標の設定機能におけるセキュリティ向上検証 START
                ''バインド変数
                'query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
                'query.AddParameterWithTypeValue("MONTH", OracleDbType.Char, month)

                'バインド変数
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.Varchar2, dealerCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.Varchar2, branchCode)
                query.AddParameterWithTypeValue("TARGET_YEARMONTH", OracleDbType.Date, targetYearMonth)
                'm.sakamoto (トライ店システム評価)月別販売目標の設定機能におけるセキュリティ向上検証 END

                'クエリ実行
                dt = query.GetData()
            End Using

            Logger.Info("GetTargetInfoOfTeam End Ret[" & dt.TableName & "[Count =" & dt.Count & "]]")
            Return dt

        End Function
        '$02 受注後フォロー機能開発 END

        ''' <summary>
        ''' 店舗の目標値を取得する。
        ''' </summary>
        ''' <param name="dealerCode">検索対象販売点コード</param>
        ''' <param name="branchCode">検索対象店舗コード</param>
        ''' <param name="Month">検索対象年月(YYYYMM)</param>
        ''' <returns>処理結果</returns>
        ''' <remarks></remarks>
        Public Function GetTargetInfoOfBranch(ByVal dealerCode As String, ByVal branchCode As String, _
                                              ByVal month As String) As SC3010202DataSet.SC3010202TargetDataTable

            Logger.Info("GetTargetInfoOfTeam Start Param[dealerCode=" & dealerCode & _
            ", branchCode=" & branchCode & ", month=" & month & "]")

            'm.sakamoto (トライ店システム評価)月別販売目標の設定機能におけるセキュリティ向上検証 START
            Dim tempYear As String = month.Substring(0, 4)
            Dim tempMonth As String = month.Substring(4)
            Dim targetYearMonth As Date = New Date(tempYear, tempMonth, 1)
            'm.sakamoto (トライ店システム評価)月別販売目標の設定機能におけるセキュリティ向上検証 END

            Dim dt As SC3010202DataSet.SC3010202TargetDataTable = Nothing

            Using query As New DBSelectQuery(Of SC3010202DataSet.SC3010202TargetDataTable)("SC3010202_004")
                Dim sql As New StringBuilder

                'SQL文作成
                With sql

                    'm.sakamoto (トライ店システム評価)月別販売目標の設定機能におけるセキュリティ向上検証 START
                    ''$02 受注後フォロー機能開発 START
                    '.Append(" SELECT /* SC3010202_004 */ ")
                    '.Append("        SUM(TARGET_WALKIN) AS WALKIN ")
                    '.Append("      , SUM(TARGET_TESTDRIVE) AS TESTDRIVE ")
                    '.Append("      , SUM(TARGET_EVALUATION) AS EVALUATION ")
                    '.Append("      , SUM(TARGET_DELIDATE) AS DELIVERY ")
                    '.Append("      , SUM(TARGET_SUCCESS) AS ORDERS ")
                    '.Append("   FROM TBL_SALESACTIVE_TARGET ")
                    '.Append("  WHERE DLRCD = :DLRCD ")
                    '.Append("    AND STRCD = :STRCD ")
                    '.Append("    AND MONTH = :MONTH ")
                    '.Append("    AND ACCOUNT IN ( ")
                    '.Append("     SELECT STF_CD ")
                    '.Append("       FROM TB_M_STAFF ")
                    '.Append("      WHERE DLR_CD = :DLRCD ")
                    '.Append("        AND BRN_CD = :STRCD ")
                    '.Append("        AND INUSE_FLG = '1' ")
                    '.Append("        )")
                    '.Append("  GROUP BY DLRCD, STRCD, MONTH")
                    ''$02 受注後フォロー機能開発 END

                    .Append(" SELECT /* SC3010202_004 */ ")
                    .Append("        SUM(WALKIN_TARGET_VAL) AS WALKIN ")
                    .Append("      , SUM(TESTDRIVE_TARGET_VAL) AS TESTDRIVE ")
                    .Append("      , SUM(ASSMNT_TARGET_VAL) AS EVALUATION ")
                    .Append("      , SUM(DELI_TARGET_VAL) AS DELIVERY ")
                    .Append("      , SUM(SUCCESS_TARGET_VAL) AS ORDERS ")
                    .Append("   FROM TB_M_SLS_STF_ACT_TARGET ")
                    .Append("  WHERE DLR_CD = :DLR_CD ")
                    .Append("    AND BRN_CD = :BRN_CD ")
                    .Append("    AND TARGET_YEARMONTH = :TARGET_YEARMONTH ")
                    .Append("    AND STF_CD IN ( ")
                    .Append("     SELECT STF_CD ")
                    .Append("       FROM TB_M_STAFF ")
                    .Append("      WHERE DLR_CD = :DLR_CD ")
                    .Append("        AND BRN_CD = :BRN_CD ")
                    .Append("        AND INUSE_FLG = '1' ")
                    .Append("        )")
                    .Append("  GROUP BY DLR_CD, BRN_CD, TARGET_YEARMONTH")
                    'm.sakamoto (トライ店システム評価)月別販売目標の設定機能におけるセキュリティ向上検証 END

                End With

                query.CommandText = sql.ToString()
                'm.sakamoto (トライ店システム評価)月別販売目標の設定機能におけるセキュリティ向上検証 START
                'バインド変数
                'query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
                'query.AddParameterWithTypeValue("MONTH", OracleDbType.Char, month)

                'バインド変数
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.Varchar2, dealerCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.Varchar2, branchCode)
                query.AddParameterWithTypeValue("TARGET_YEARMONTH", OracleDbType.Date, targetYearMonth)
                'm.sakamoto (トライ店システム評価)月別販売目標の設定機能におけるセキュリティ向上検証 END

                'クエリ実行
                dt = query.GetData()
            End Using

            Logger.Info("GetTargetInfoOfBranch End Ret[" & dt.TableName & "[Count =" & dt.Count & "]]")
            Return dt

        End Function

#End Region

#Region "当月実績情報取得"

#Region "来店"

        ''' <summary>
        ''' ログインユーザの来店実績値を取得する。
        ''' </summary>
        ''' <param name="dealerCode">検索対象販売点コード</param>
        ''' <param name="branchCode">検索対象店舗コード</param>
        ''' <param name="Month">検索対象年月(YYYYMM)</param>
        ''' <param name="Account">検索対象ユーザアカウント</param>
        ''' <returns>実績データテーブル</returns>
        ''' <remarks>Step2で実装</remarks>
        Public Function GetResultWalkIn( _
                ByVal dealerCode As String, ByVal branchCode As String, ByVal month As String, _
                ByVal account As String) As SC3010202DataSet.SC3010202ResultWalkInDataTable

            Logger.Info("GetResultWalkIn Start Param[dealerCode=" & dealerCode & _
                        ", branchCode=" & branchCode & ", month=" & month & _
                        ", account=" & account & "]")

            ' 実績データセット
            Dim dt As SC3010202DataSet.SC3010202ResultWalkInDataTable = Nothing

            Using query As New DBSelectQuery( _
                    Of SC3010202DataSet.SC3010202ResultWalkInDataTable)("SC3010202_005")
                ' SQL文
                Dim sql As New StringBuilder

                ' SQL文作成
                With sql
                    .Append(" SELECT /* SC3010202_005 */ ")
                    .Append("        COUNT(1) AS WALKIN ")
                    .Append("   FROM ( ")
                    .Append("     SELECT 1")
                    .Append("       FROM TBL_VISIT_SALES T1 ")
                    .Append("      WHERE T1.DLRCD = :DLRCD ")
                    .Append("        AND T1.STRCD = :STRCD ")
                    .Append("        AND TO_DATE(:SALESSTART, 'YYYYMM') <= T1.SALESSTART ")
                    .Append("        AND T1.VISITSTATUS IN (:VISITSTATUS_SALES_START, :VISITSTATUS_SALES_END) ")
                    .Append("        AND T1.ACCOUNT = :ACCOUNT ")
                    .Append("        ) T2")
                End With

                query.CommandText = sql.ToString()
                sql = Nothing

                ' バインド変数を設定
                With query
                    .AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                    .AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
                    .AddParameterWithTypeValue("SALESSTART", OracleDbType.Char, month)
                    .AddParameterWithTypeValue("VISITSTATUS_SALES_START", OracleDbType.Char, _
                            VisitStatusSalesStart)
                    .AddParameterWithTypeValue("VISITSTATUS_SALES_END", OracleDbType.Char, _
                            VisitStatusSalesEnd)
                    .AddParameterWithTypeValue("ACCOUNT", OracleDbType.Varchar2, account)
                End With

                ' クエリの実行
                dt = query.GetData()
            End Using

            Logger.Info("GetResultWalkIn End Ret[" & dt.TableName & "[Count =" & dt.Count & "]]")

            ' 戻り値に実績データテーブルを設定する
            Return dt

        End Function

        '$02 受注後フォロー機能開発 START
        ''' <summary>
        ''' チームの来店実績値を取得する。
        ''' </summary>
        ''' <param name="dealerCode">検索対象販売点コード</param>
        ''' <param name="branchCode">検索対象店舗コード</param>
        ''' <param name="Month">検索対象年月(YYYYMM)</param>
        ''' <param name="orgnzIdList">組織IDリスト</param>
        ''' <returns>実績データテーブル</returns>
        ''' <remarks></remarks>
        Public Function GetResultWalkInOfTeam( _
                ByVal dealerCode As String, ByVal branchCode As String, ByVal month As String, _
                ByVal orgnzIdList As List(Of String)) As SC3010202DataSet.SC3010202ResultWalkInDataTable

            Logger.Info("GetResultWalkInOfTeam Start Param[dealerCode=" & dealerCode & _
            ", branchCode=" & branchCode & ", month=" & month & _
            ", orgnzIdList=" & String.Join(",", orgnzIdList) & "]")

            ' 実績データセット
            Dim dt As SC3010202DataSet.SC3010202ResultWalkInDataTable = Nothing

            Using query As New DBSelectQuery( _
                    Of SC3010202DataSet.SC3010202ResultWalkInDataTable)("SC3010202_006")
                ' SQL文
                Dim sql As New StringBuilder

                ' SQL文作成
                With sql
                    .Append(" SELECT /* SC3010202_006 */ ")
                    .Append("        COUNT(1) AS WALKIN ")
                    .Append("   FROM ( ")
                    .Append("     SELECT 1 ")
                    .Append("       FROM TBL_VISIT_SALES T1 ")
                    .Append("      WHERE T1.DLRCD = :DLRCD ")
                    .Append("        AND T1.STRCD = :STRCD ")
                    .Append("        AND TO_DATE(:SALESSTART, 'YYYYMM') <= T1.SALESSTART ")
                    .Append("        AND T1.VISITSTATUS IN (:VISITSTATUS_SALES_START, :VISITSTATUS_SALES_END) ")
                    .Append("        AND T1.ACCOUNT IN ( ")
                    .Append("          SELECT STF_CD ")
                    .Append("            FROM TB_M_STAFF ")
                    .Append("           WHERE DLR_CD = :DLRCD ")
                    .Append("             AND BRN_CD = :STRCD ")
                    .Append("             AND INUSE_FLG = '1' ")
                    .Append("             AND ORGNZ_ID IN ( ")
                    .Append(ConvertOrgnzIdStr(orgnzIdList))
                    .Append("                 ) ")
                    .Append("            ) ")
                    .Append("        ) T2")
                End With

                query.CommandText = sql.ToString()
                sql = Nothing

                ' バインド変数を設定
                With query
                    .AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                    .AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
                    .AddParameterWithTypeValue("SALESSTART", OracleDbType.Char, month)
                    .AddParameterWithTypeValue("VISITSTATUS_SALES_START", OracleDbType.Char, _
                            VisitStatusSalesStart)
                    .AddParameterWithTypeValue("VISITSTATUS_SALES_END", OracleDbType.Char, _
                            VisitStatusSalesEnd)
                End With

                ' クエリの実行
                dt = query.GetData()
            End Using

            Logger.Info("GetResultWalkInOfTeam End Ret[" & dt.TableName & "[Count =" & dt.Count & "]]")

            ' 戻り値に実績データテーブルを設定する
            Return dt

        End Function
        '$02 受注後フォロー機能開発 END

        ''' <summary>
        ''' 店舗の来店実績値を取得する。
        ''' </summary>
        ''' <param name="dealerCode">検索対象販売点コード</param>
        ''' <param name="branchCode">検索対象店舗コード</param>
        ''' <param name="Month">検索対象年月(YYYYMM)</param>
        ''' <returns>実績データテーブル</returns>
        ''' <remarks>Step2で実装</remarks>
        Public Function GetResultWalkInOfBranch( _
                ByVal dealerCode As String, ByVal branchCode As String, ByVal month As String) _
                As SC3010202DataSet.SC3010202ResultWalkInDataTable

            Logger.Info("GetResultWalkInOfBranch Start Param[dealerCode=" & dealerCode & _
                        ", branchCode=" & branchCode & ", month=" & month & "]")

            ' 実績データセット
            Dim dt As SC3010202DataSet.SC3010202ResultWalkInDataTable = Nothing

            Using query As New DBSelectQuery( _
                    Of SC3010202DataSet.SC3010202ResultWalkInDataTable)("SC3010202_007")
                ' SQL文
                Dim sql As New StringBuilder

                ' SQL文作成
                With sql
                    .Append(" SELECT /* SC3010202_007 */ ")
                    .Append("        COUNT(1) AS WALKIN ")
                    .Append("   FROM ( ")
                    .Append("     SELECT 1 ")
                    .Append("       FROM TBL_VISIT_SALES T1 ")
                    .Append("      WHERE T1.DLRCD = :DLRCD ")
                    .Append("        AND T1.STRCD = :STRCD ")
                    .Append("        AND TO_DATE(:SALESSTART, 'YYYYMM') <= T1.SALESSTART ")
                    .Append("        AND T1.VISITSTATUS IN (:VISITSTATUS_SALES_START, :VISITSTATUS_SALES_END) ")
                    .Append("        AND T1.ACCOUNT IN ( ")
                    .Append("          SELECT STF_CD ")
                    .Append("            FROM TB_M_STAFF ")
                    .Append("           WHERE DLR_CD = :DLRCD ")
                    .Append("             AND BRN_CD = :STRCD ")
                    .Append("             AND INUSE_FLG = '1' ")
                    .Append("            ) ")
                    .Append("        ) T2")
                End With

                query.CommandText = sql.ToString()
                sql = Nothing

                ' バインド変数を設定
                With query
                    .AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                    .AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
                    .AddParameterWithTypeValue("SALESSTART", OracleDbType.Char, month)
                    .AddParameterWithTypeValue("VISITSTATUS_SALES_START", OracleDbType.Char, _
                            VisitStatusSalesStart)
                    .AddParameterWithTypeValue("VISITSTATUS_SALES_END", OracleDbType.Char, _
                            VisitStatusSalesEnd)
                End With

                ' クエリの実行
                dt = query.GetData()
            End Using

            Logger.Info("GetResultWalkInOfBranch End Ret[" & dt.TableName & "[Count =" & dt.Count & "]]")

            ' 戻り値に実績データテーブルを設定する
            Return dt

        End Function

#End Region

#Region "試乗、査定"

        ''' <summary>
        ''' ログインユーザの試乗、査定実績値を取得する。
        ''' </summary>
        ''' <param name="dealerCode">検索対象販売点コード</param>
        ''' <param name="month">検索対象年月(YYYYMM)</param>
        ''' <param name="Account">検索対象ユーザアカウント</param>
        ''' <param name="isHistory">historyテーブルから取得するかどうか</param>
        ''' <returns>処理結果</returns>
        ''' <remarks></remarks>
        Public Function GetResultHistory(ByVal dealerCode As String, ByVal month As String, _
                                         ByVal account As String, ByVal isHistory As Boolean) _
                                     As SC3010202DataSet.SC3010202ResultCRHISDataTable

            Logger.Info("GetResultHistory Start Param[dealerCode=" & dealerCode & _
            ", month=" & month & ", account=" & account & ", isHistory=" & isHistory & "]")

            Dim dt As SC3010202DataSet.SC3010202ResultCRHISDataTable = Nothing

            Using query As New DBSelectQuery(Of SC3010202DataSet.SC3010202ResultCRHISDataTable)("SC3010202_008")
                Dim sql As New StringBuilder

                '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 START
                'SQL文作成
                With sql
                    .Append(" SELECT /*SC3010202_008*/ ")
                    .Append("        RSLT_SALES_CAT AS ACTIONCD ")
                    .Append("      , COUNT(1) AS CNT ")
                    .Append("   FROM ( ")
                    '$02 受注後フォロー機能開発 START
                    If isHistory Then
                        .Append("     SELECT SAL_ACT_H.RSLT_SALES_CAT ")
                        .Append("       FROM TB_H_ACTIVITY ACT_H ")
                        .Append("          , TB_H_SALES_ACT SAL_ACT_H ")
                        .Append("      WHERE ACT_H.ACT_ID = SAL_ACT_H.ACT_ID ")
                        .Append("        AND ACT_H.RSLT_FLG = '1' ")
                        .Append("        AND ACT_H.RSLT_DLR_CD = :DLRCD ")
                        .Append("        AND ACT_H.RSLT_STF_CD = :ACCOUNT ")
                        .Append("        AND ACT_H.RSLT_DATETIME >= TO_DATE(:ACTDATE, 'YYYYMM') ")
                        .Append("        AND SAL_ACT_H.RSLT_SALES_CAT IN('4','7') ")
                    Else
                        .Append("     SELECT SAL_ACT.RSLT_SALES_CAT ")
                        .Append("       FROM TB_T_ACTIVITY ACT ")
                        .Append("          , TB_T_SALES_ACT SAL_ACT ")
                        .Append("      WHERE ACT.ACT_ID = SAL_ACT.ACT_ID ")
                        .Append("        AND ACT.RSLT_FLG = '1' ")
                        .Append("        AND ACT.RSLT_DLR_CD = :DLRCD ")
                        .Append("        AND ACT.RSLT_STF_CD = :ACCOUNT ")
                        .Append("        AND ACT.RSLT_DATETIME >= TO_DATE(:ACTDATE, 'YYYYMM') ")
                        .Append("        AND SAL_ACT.RSLT_SALES_CAT IN('4','7') ")
                    End If
                    '$02 受注後フォロー機能開発 END
                    .Append("        )  ")
                    .Append("  GROUP BY RSLT_SALES_CAT ")
                End With
                '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 END

                query.CommandText = sql.ToString()
                'バインド変数
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.Char, account)
                query.AddParameterWithTypeValue("ACTDATE", OracleDbType.Varchar2, month)

                'クエリ実行
                dt = query.GetData()
            End Using

            Logger.Info("GetResultHistory End Ret[" & dt.TableName & "[Count =" & dt.Count & "]]")
            Return dt

        End Function

        '$02 受注後フォロー機能開発 START
        ''' <summary>
        ''' チームの試乗、査定実績値を取得する。
        ''' </summary>
        ''' <param name="dealerCode">検索対象販売点コード</param>
        ''' <param name="branchCode">検索対象店舗コード</param>
        ''' <param name="month">検索対象年月(YYYYMM)</param>
        ''' <param name="orgnzIdList">検索対象組織IDリスト</param>
        ''' <param name="isHistory">historyテーブルから取得するかどうか</param>
        ''' <returns>処理結果</returns>
        ''' <remarks></remarks>
        Public Function GetResultHistoryOfTeam(ByVal dealerCode As String, ByVal branchCode As String, _
                                               ByVal month As String, ByVal orgnzIdList As List(Of String), _
                                               ByVal isHistory As Boolean) As SC3010202DataSet.SC3010202ResultCRHISDataTable

            Logger.Info("GetResultHistoryOfTeam Start Param[dealerCode=" & dealerCode & _
                        ", branchCode=" & branchCode & ", month=" & month & _
                        ", orgnzIdList=" & String.Join(",", orgnzIdList) & ", isHistory=" & isHistory & "]")

            Dim dt As SC3010202DataSet.SC3010202ResultCRHISDataTable = Nothing

            Using query As New DBSelectQuery(Of SC3010202DataSet.SC3010202ResultCRHISDataTable)("SC3010202_009")
                Dim sql As New StringBuilder

                '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 START
                'SQL文作成
                With sql
                    .Append(" SELECT /*SC3010202_009*/ ")
                    .Append("        RSLT_SALES_CAT AS ACTIONCD ")
                    .Append("      , COUNT(1) AS CNT ")
                    .Append("   FROM ( ")

                    If isHistory Then
                        .Append("     SELECT SAL_ACT_H.RSLT_SALES_CAT ")
                        .Append("       FROM TB_H_ACTIVITY ACT_H ")
                        .Append("          , TB_H_SALES_ACT SAL_ACT_H ")
                        .Append("      WHERE ACT_H.ACT_ID = SAL_ACT_H.ACT_ID ")
                        .Append("        AND ACT_H.RSLT_FLG = '1' ")
                        .Append("        AND ACT_H.RSLT_DLR_CD = :DLRCD ")
                        .Append("        AND ACT_H.RSLT_STF_CD IN ( ")
                        .Append("         SELECT STF_CD ")
                        .Append("           FROM TB_M_STAFF ")
                        .Append("          WHERE DLR_CD = :DLRCD ")
                        .Append("            AND BRN_CD = :STRCD ")
                        .Append("            AND INUSE_FLG = '1' ")
                        .Append("            AND ORGNZ_ID IN ( ")
                        .Append(ConvertOrgnzIdStr(orgnzIdList))
                        .Append("                )")
                        .Append("            )")
                        .Append("        AND ACT_H.RSLT_DATETIME >= TO_DATE(:ACTDATE, 'YYYYMM') ")
                        .Append("        AND SAL_ACT_H.RSLT_SALES_CAT IN('4','7') ")
                    Else
                        .Append("     SELECT SAL_ACT.RSLT_SALES_CAT ")
                        .Append("       FROM TB_T_ACTIVITY ACT ")
                        .Append("          , TB_T_SALES_ACT SAL_ACT ")
                        .Append("      WHERE ACT.ACT_ID = SAL_ACT.ACT_ID ")
                        .Append("        AND ACT.RSLT_FLG = '1' ")
                        .Append("        AND ACT.RSLT_DLR_CD = :DLRCD ")
                        .Append("        AND ACT.RSLT_STF_CD IN ( ")
                        .Append("         SELECT STF_CD ")
                        .Append("           FROM TB_M_STAFF ")
                        .Append("          WHERE DLR_CD = :DLRCD")
                        .Append("            AND BRN_CD = :STRCD ")
                        .Append("            AND INUSE_FLG = '1' ")
                        .Append("            AND ORGNZ_ID IN (")
                        .Append(ConvertOrgnzIdStr(orgnzIdList))
                        .Append("                )")
                        .Append("            )")
                        .Append("        AND ACT.RSLT_DATETIME >= TO_DATE(:ACTDATE, 'YYYYMM') ")
                        .Append("        AND SAL_ACT.RSLT_SALES_CAT IN('4','7') ")
                    End If

                    .Append("        )  ")
                    .Append("  GROUP BY RSLT_SALES_CAT ")
                End With
                '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 END

                query.CommandText = sql.ToString()
                'バインド変数
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
                query.AddParameterWithTypeValue("ACTDATE", OracleDbType.Varchar2, month)

                'クエリ実行
                dt = query.GetData()
            End Using

            Logger.Info("GetResultHistoryOfTeam End Ret[" & dt.TableName & "[Count =" & dt.Count & "]]")
            Return dt

        End Function
        '$02 受注後フォロー機能開発 END

        ''' <summary>
        ''' 店舗の試乗、査定実績値を取得する。
        ''' </summary>
        ''' <param name="dealerCode">検索対象販売店コード</param>
        ''' <param name="branchCode">検索対象店舗コード</param>
        ''' <param name="Month">検索対象年月(YYYYMM)</param>
        ''' <param name="isHistory">historyテーブルから取得するかどうか</param>
        ''' <returns>処理結果</returns>
        ''' <remarks></remarks>
        Public Function GetResultHistoryOfBranch(ByVal dealerCode As String, ByVal branchCode As String, _
                                                 ByVal month As String, ByVal isHistory As Boolean) _
                                             As SC3010202DataSet.SC3010202ResultCRHISDataTable

            Logger.Info("GetResultHistoryOfBranch Start Param[dealerCode=" & dealerCode & _
            ", branchCode=" & branchCode & ", month=" & month & _
             ", isHistory=" & isHistory & "]")

            Dim dt As SC3010202DataSet.SC3010202ResultCRHISDataTable = Nothing

            Using query As New DBSelectQuery(Of SC3010202DataSet.SC3010202ResultCRHISDataTable)("SC3010202_010")
                Dim sql As New StringBuilder

                '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 START
                'SQL文作成
                With sql
                    .Append(" SELECT /*SC3010202_010*/ ")
                    .Append("        RSLT_SALES_CAT AS ACTIONCD ")
                    .Append("      , COUNT(1) AS CNT ")
                    .Append("   FROM ( ")

                    '$02 受注後フォロー機能開発 START
                    If isHistory Then
                        .Append("     SELECT SAL_ACT_H.RSLT_SALES_CAT ")
                        .Append("       FROM TB_H_ACTIVITY ACT_H ")
                        .Append("          , TB_H_SALES_ACT SAL_ACT_H ")
                        .Append("      WHERE ACT_H.ACT_ID = SAL_ACT_H.ACT_ID  ")
                        .Append("        AND ACT_H.RSLT_FLG = '1' ")
                        .Append("        AND ACT_H.RSLT_DLR_CD = :DLRCD ")
                        .Append("        AND ACT_H.RSLT_BRN_CD = :STRCD ")
                        .Append("        AND ACT_H.RSLT_STF_CD IN ( ")
                        .Append("         SELECT STF_CD ")
                        .Append("           FROM TB_M_STAFF ")
                        .Append("          WHERE DLR_CD = :DLRCD ")
                        .Append("            AND BRN_CD = :STRCD ")
                        .Append("            AND INUSE_FLG = '1' ")
                        .Append("            )")
                        .Append("        AND ACT_H.RSLT_DATETIME >= TO_DATE(:ACTDATE,'YYYYMM') ")
                        .Append("        AND SAL_ACT_H.RSLT_SALES_CAT IN('4','7') ")
                    Else
                        .Append("     SELECT SAL_ACT.RSLT_SALES_CAT ")
                        .Append("       FROM TB_T_ACTIVITY ACT ")
                        .Append("          , TB_T_SALES_ACT SAL_ACT ")
                        .Append("      WHERE ACT.ACT_ID = SAL_ACT.ACT_ID ")
                        .Append("        AND ACT.RSLT_FLG = '1'  ")
                        .Append("        AND ACT.RSLT_DLR_CD = :DLRCD ")
                        .Append("        AND ACT.RSLT_BRN_CD = :STRCD ")
                        .Append("        AND ACT.RSLT_STF_CD IN ( ")
                        .Append("         SELECT STF_CD ")
                        .Append("           FROM TB_M_STAFF ")
                        .Append("          WHERE DLR_CD = :DLRCD ")
                        .Append("            AND BRN_CD = :STRCD ")
                        .Append("            AND INUSE_FLG = '1' ")
                        .Append("            )")
                        .Append("        AND ACT.RSLT_DATETIME >= TO_DATE(:ACTDATE, 'YYYYMM')  ")
                        .Append("        AND SAL_ACT.RSLT_SALES_CAT IN('4','7')  ")
                    End If
                    '$02 受注後フォロー機能開発 END
                    .Append("        )  ")
                    .Append("  GROUP BY RSLT_SALES_CAT ")
                End With
                '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 END

                query.CommandText = sql.ToString()
                'バインド変数
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
                query.AddParameterWithTypeValue("ACTDATE", OracleDbType.Varchar2, month)

                'クエリ実行
                dt = query.GetData()
            End Using

            Logger.Info("GetResultHistoryOfBranch End Ret[" & dt.TableName & "[Count =" & dt.Count & "]]")
            Return dt

        End Function

#End Region

#Region "受注"

        ''' <summary>
        ''' ログインユーザの受注実績値を取得する。
        ''' </summary>
        ''' <param name="dealerCode">検索対象販売点コード</param>
        ''' <param name="branchCode">検索対象店舗コード</param>
        ''' <param name="Month">検索対象年月(YYYYMM)</param>
        ''' <param name="Account">検索対象ユーザアカウント</param>
        ''' <returns>処理結果</returns>
        ''' <remarks></remarks>
        Public Function GetResultFollowUpBoxTally(ByVal dealerCode As String, ByVal branchCode As String, _
                                                  ByVal account As String, ByVal month As String) _
                                              As SC3010202DataSet.SC3010202ResultTallyDataTable

            Logger.Info("GetResultFollowUpBoxTally Start Param[dealerCode=" & dealerCode & _
            ", branchCode=" & branchCode & ", account=" & account & ", month=" & month & "]")

            Dim dt As SC3010202DataSet.SC3010202ResultTallyDataTable = Nothing

            Using query As New DBSelectQuery(Of SC3010202DataSet.SC3010202ResultTallyDataTable)("SC3010202_011")
                Dim sql As New StringBuilder

                '$02 受注後フォロー機能開発 START
                'SQL文作成
                With sql
                    .Append(" SELECT /*SC3010202_011*/  ")
                    .Append("        (CASE WHEN SUM(PREF_AMOUNT) IS NULL THEN 0 ELSE SUM(PREF_AMOUNT) END) AS CNT ")
                    .Append("   FROM ( ")
                    .Append("     SELECT PREF_AMOUNT ")
                    .Append("       FROM TB_T_SPM_SUCCESS_VCL ")
                    .Append("      WHERE SALES_ID IN ( ")
                    .Append("         SELECT SALES_ID ")
                    .Append("           FROM TB_T_SPM_BEFORE_ODR_CHIP ")
                    .Append("          WHERE DLR_CD = :DLR_CD ")
                    .Append("            AND BRN_CD = :BRN_CD ")
                    .Append("            AND SALES_PIC_STF_CD = :SALES_PIC_STF_CD ")
                    .Append("            AND SALES_STATUS = '31' ")
                    .Append("            AND LAST_ACT_DATE >=TO_DATE(:LAST_ACT_DATE,'YYYYMM') ")
                    .Append("            ) ")
                    .Append("      UNION ALL ")
                    .Append("     SELECT PREF_AMOUNT ")
                    .Append("       FROM TB_H_SPM_SUCCESS_VCL ")
                    .Append("      WHERE SALES_ID IN ( ")
                    .Append("         SELECT SALES_ID ")
                    .Append("           FROM TB_H_SPM_BEFORE_ODR_CHIP ")
                    .Append("          WHERE DLR_CD = :DLR_CD ")
                    .Append("            AND BRN_CD = :BRN_CD ")
                    .Append("            AND SALES_PIC_STF_CD = :SALES_PIC_STF_CD ")
                    .Append("            AND SALES_STATUS = '31' ")
                    .Append("            AND LAST_ACT_DATE >=TO_DATE(:LAST_ACT_DATE,'YYYYMM') ")
                    .Append("            ) ")
                    .Append("        ) ")
                End With

                query.CommandText = sql.ToString()
                'バインド変数
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.Char, branchCode)
                query.AddParameterWithTypeValue("SALES_PIC_STF_CD", OracleDbType.Char, account)
                query.AddParameterWithTypeValue("LAST_ACT_DATE", OracleDbType.Varchar2, month)
                '$02 受注後フォロー機能開発 END

                'クエリ実行
                dt = query.GetData()
            End Using

            Logger.Info("GetResultFollowUpBoxTally End Ret[" & dt.TableName & "[Count =" & dt.Count & "]]")
            Return dt

        End Function

        '$02 受注後フォロー機能開発 START
        ''' <summary>
        ''' チームの受注実績値を取得する。
        ''' </summary>
        ''' <param name="dealerCode">検索対象販売点コード</param>
        ''' <param name="branchCode">検索対象店舗コード</param>
        ''' <param name="Month">検索対象年月(YYYYMM)</param>
        ''' <param name="orgnzIdList">検索対象組織IDリスト</param>
        ''' <returns>処理結果</returns>
        ''' <remarks></remarks>
        Public Function GetResultFollowUpBoxTallyOfTeam(ByVal dealerCode As String, ByVal branchCode As String, _
                                                        ByVal orgnzIdList As List(Of String), ByVal month As String) _
                                              As SC3010202DataSet.SC3010202ResultTallyDataTable

            Logger.Info("GetResultFollowUpBoxTallyOfTeam Start Param[dealerCode=" & dealerCode & _
            ", branchCode=" & branchCode & ", orgnzIdList=" & String.Join(",", orgnzIdList) & _
            ", month=" & month & "]")

            Dim dt As SC3010202DataSet.SC3010202ResultTallyDataTable = Nothing

            Using query As New DBSelectQuery(Of SC3010202DataSet.SC3010202ResultTallyDataTable)("SC3010202_012")
                Dim sql As New StringBuilder

                'SQL文作成
                With sql
                    .Append(" SELECT /*SC3010202_012*/  ")
                    .Append("        (CASE WHEN SUM(PREF_AMOUNT) IS NULL THEN 0 ELSE SUM(PREF_AMOUNT) END) AS CNT ")
                    .Append("   FROM ( ")
                    .Append("     SELECT PREF_AMOUNT ")
                    .Append("       FROM TB_T_SPM_SUCCESS_VCL ")
                    .Append("      WHERE SALES_ID IN ( ")
                    .Append("         SELECT SALES_ID ")
                    .Append("           FROM TB_T_SPM_BEFORE_ODR_CHIP ")
                    .Append("          WHERE DLR_CD = :DLR_CD ")
                    .Append("            AND BRN_CD = :BRN_CD ")
                    .Append("            AND SALES_PIC_STF_CD IN (")
                    .Append("             SELECT STF_CD ")
                    .Append("               FROM TB_M_STAFF ")
                    .Append("              WHERE DLR_CD = :DLR_CD")
                    .Append("                AND BRN_CD = :BRN_CD ")
                    .Append("                AND INUSE_FLG = '1' ")
                    .Append("                AND ORGNZ_ID IN (")
                    .Append(ConvertOrgnzIdStr(orgnzIdList))
                    .Append("                    )")
                    .Append("                )")
                    .Append("                AND SALES_STATUS = '31' ")
                    .Append("                AND LAST_ACT_DATE >=TO_DATE(:LAST_ACT_DATE,'YYYYMM') ")
                    .Append("            )")
                    .Append("      UNION ALL ")
                    .Append("     SELECT PREF_AMOUNT ")
                    .Append("       FROM TB_H_SPM_SUCCESS_VCL ")
                    .Append("      WHERE SALES_ID IN ( ")
                    .Append("         SELECT SALES_ID ")
                    .Append("           FROM TB_H_SPM_BEFORE_ODR_CHIP ")
                    .Append("          WHERE DLR_CD = :DLR_CD ")
                    .Append("            AND BRN_CD = :BRN_CD ")
                    .Append("            AND SALES_PIC_STF_CD IN (")
                    .Append("             SELECT STF_CD ")
                    .Append("               FROM TB_M_STAFF ")
                    .Append("              WHERE DLR_CD = :DLR_CD")
                    .Append("                AND BRN_CD = :BRN_CD ")
                    .Append("                AND INUSE_FLG = '1' ")
                    .Append("                AND ORGNZ_ID IN (")
                    .Append(ConvertOrgnzIdStr(orgnzIdList))
                    .Append("                    )")
                    .Append("                )")
                    .Append("                AND SALES_STATUS = '31' ")
                    .Append("                AND LAST_ACT_DATE >=TO_DATE(:LAST_ACT_DATE,'YYYYMM') ")
                    .Append("            ) ")
                    .Append("        ) ")
                End With

                query.CommandText = sql.ToString()
                'バインド変数
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.Char, branchCode)
                query.AddParameterWithTypeValue("LAST_ACT_DATE", OracleDbType.Varchar2, month)

                'クエリ実行
                dt = query.GetData()
            End Using

            Logger.Info("GetResultFollowUpBoxTallyOfTeam End Ret[" & dt.TableName & "[Count =" & dt.Count & "]]")
            Return dt

        End Function
        '$02 受注後フォロー機能開発 END

        ''' <summary>
        ''' 店舗の受注実績値を取得する。
        ''' </summary>
        ''' <param name="dealerCode">検索対象販売点コード</param>
        ''' <param name="branchCode">検索対象店舗コード</param>
        ''' <param name="Month">検索対象年月(YYYYMM)</param>
        ''' <returns>処理結果</returns>
        ''' <remarks></remarks>
        Public Function GetResultFollowUpBoxTallyOfBranch(ByVal dealerCode As String, ByVal branchCode As String, _
                                                          ByVal month As String) As SC3010202DataSet.SC3010202ResultTallyDataTable

            
            Logger.Info("GetResultFollowUpBoxTallyOfBranch Start Param[dealerCode=" & dealerCode & _
            ", branchCode=" & branchCode & ", month=" & month & "]")

            Dim dt As SC3010202DataSet.SC3010202ResultTallyDataTable = Nothing

            Using query As New DBSelectQuery(Of SC3010202DataSet.SC3010202ResultTallyDataTable)("SC3010202_013")
                Dim sql As New StringBuilder

                '$02 受注後フォロー機能開発 START
                'SQL文作成
                With sql
                    .Append(" SELECT /*SC3010202_013*/  ")
                    .Append("        (CASE WHEN SUM(PREF_AMOUNT) IS NULL THEN 0 ELSE SUM(PREF_AMOUNT) END) AS CNT ")
                    .Append("   FROM ( ")
                    .Append("     SELECT PREF_AMOUNT ")
                    .Append("       FROM TB_T_SPM_SUCCESS_VCL ")
                    .Append("      WHERE SALES_ID IN ( ")
                    .Append("         SELECT SALES_ID ")
                    .Append("           FROM TB_T_SPM_BEFORE_ODR_CHIP ")
                    .Append("          WHERE DLR_CD = :DLR_CD ")
                    .Append("            AND BRN_CD = :BRN_CD ")
                    .Append("            AND SALES_PIC_STF_CD IN (")
                    .Append("             SELECT STF_CD ")
                    .Append("               FROM TB_M_STAFF ")
                    .Append("              WHERE DLR_CD = :DLR_CD")
                    .Append("                AND BRN_CD = :BRN_CD ")
                    .Append("                AND INUSE_FLG = '1' ")
                    .Append("                )")
                    .Append("            AND SALES_STATUS = '31' ")
                    .Append("            AND LAST_ACT_DATE >=TO_DATE(:LAST_ACT_DATE,'YYYYMM') ")
                    .Append("            ) ")
                    .Append("      UNION ALL ")
                    .Append("     SELECT PREF_AMOUNT ")
                    .Append("       FROM TB_H_SPM_SUCCESS_VCL ")
                    .Append("      WHERE SALES_ID IN ( ")
                    .Append("         SELECT SALES_ID ")
                    .Append("           FROM TB_H_SPM_BEFORE_ODR_CHIP ")
                    .Append("          WHERE DLR_CD = :DLR_CD ")
                    .Append("            AND BRN_CD = :BRN_CD ")
                    .Append("            AND SALES_PIC_STF_CD IN (")
                    .Append("             SELECT STF_CD ")
                    .Append("               FROM TB_M_STAFF ")
                    .Append("              WHERE DLR_CD = :DLR_CD")
                    .Append("                AND BRN_CD = :BRN_CD ")
                    .Append("                AND INUSE_FLG = '1' ")
                    .Append("                )")
                    .Append("            AND SALES_STATUS = '31' ")
                    .Append("            AND LAST_ACT_DATE >=TO_DATE(:LAST_ACT_DATE,'YYYYMM') ")
                    .Append("            ) ")
                    .Append("        ) ")
                End With

                query.CommandText = sql.ToString()
                'バインド変数
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.Char, branchCode)
                query.AddParameterWithTypeValue("LAST_ACT_DATE", OracleDbType.Varchar2, month)
                '$02 受注後フォロー機能開発 END

                'クエリ実行
                dt = query.GetData()
            End Using

            Logger.Info("GetResultFollowUpBoxTallyOfBranch End Ret[" & dt.TableName & "[Count =" & dt.Count & "]]")
            Return dt

        End Function

#End Region

#Region "納車"

        ''' <summary>
        ''' ログインユーザの納車実績値を取得する。
        ''' </summary>
        ''' <param name="dealerCode">検索対象販売点コード</param>
        ''' <param name="branchCode">検索対象店舗コード</param>
        ''' <param name="month">検索対象年月(YYYYMM)</param>
        ''' <param name="account">検索対象ユーザアカウント</param>
        ''' <param name="deliveryActCode">納車活動コード</param>
        ''' <returns>処理結果</returns>
        ''' <remarks></remarks>
        Public Function GetResultDelivery(ByVal dealerCode As String, ByVal branchCode As String, _
                                          ByVal month As String, ByVal account As String, _
                                          ByVal deliveryActCode As String) As Integer

            Logger.Info("GetResultDelivery Start Param[dealerCode=" & dealerCode & _
                        ", branchCode=" & branchCode & ", month=" & month & _
                        ", account=" & account & ", deliveryActCode=" & deliveryActCode & "]")

            Using query As New DBSelectQuery(Of DataTable)("SC3010202_014")
                Dim sql As New StringBuilder

                '$02 受注後フォロー機能開発 START
                'SQL文作成
                With sql
                    .Append(" SELECT /*SC3010202_014*/ ")
                    .Append("        SUM(CNT) AS SUMCNT ")
                    .Append("   FROM ( ")
                    .Append("     SELECT COUNT(1) AS CNT ")
                    .Append("       FROM TB_T_AFTER_ODR_ACT ")
                    .Append("      WHERE RSLT_DLR_CD = :DLR_CD ")
                    .Append("        AND RSLT_BRN_CD = :BRN_CD ")
                    .Append("        AND RSLT_STF_CD = :RSLT_STF_CD ")
                    .Append("        AND AFTER_ODR_ACT_CD = :AFTER_ODR_ACT_CD ")
                    .Append("        AND RSLT_END_DATEORTIME >= TO_DATE(:RSLT_END_DATEORTIME,'YYYYMM') ")
                    .Append("      UNION ALL ")
                    .Append("     SELECT COUNT(1) AS CNT  ")
                    .Append("       FROM TB_H_AFTER_ODR_ACT ")
                    .Append("      WHERE RSLT_DLR_CD= :DLR_CD ")
                    .Append("        AND RSLT_BRN_CD = :BRN_CD ")
                    .Append("        AND RSLT_STF_CD = :RSLT_STF_CD ")
                    .Append("        AND AFTER_ODR_ACT_CD = :AFTER_ODR_ACT_CD ")
                    .Append("        AND RSLT_END_DATEORTIME >= TO_DATE(:RSLT_END_DATEORTIME,'YYYYMM') ")
                    .Append("        ) ")

                End With

                query.CommandText = sql.ToString()
                'バインド変数
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.Char, branchCode)
                query.AddParameterWithTypeValue("RSLT_STF_CD", OracleDbType.Char, account)
                query.AddParameterWithTypeValue("AFTER_ODR_ACT_CD", OracleDbType.Char, deliveryActCode)
                query.AddParameterWithTypeValue("RSLT_END_DATEORTIME", OracleDbType.Varchar2, month)
                '$02 受注後フォロー機能開発 END

                '検索結果返却
                Dim dt As DataTable = query.GetData()

                If dt.Equals(Nothing) Then
                    Logger.Info("GetResultDelivery End Ret[0]")
                    Return 0
                Else
                    Logger.Info("GetResultDelivery End Ret[" & dt.Rows(0).Item("SUMCNT") & "]")
                    Return dt.Rows(0).Item("SUMCNT")
                End If
            End Using
        End Function

        ''' <summary>
        ''' チームの納車実績値を取得する。
        ''' </summary>
        ''' <param name="dealerCode">検索対象販売点コード</param>
        ''' <param name="branchCode">検索対象店舗コード</param>
        ''' <param name="month">検索対象年月(YYYYMM)</param>
        ''' <param name="orgnzIdList">検索対象組織IDリスト</param>
        ''' <param name="deliveryActCode">納車活動コード</param>
        ''' <returns>処理結果</returns>
        ''' <remarks></remarks>
        Public Function GetResultDeliveryOfTeam(ByVal dealerCode As String, ByVal branchCode As String, _
                                          ByVal month As String, ByVal orgnzIdList As List(Of String), _
                                          ByVal deliveryActCode As String) As Integer

            Logger.Info("GetResultDeliveryOfTeam Start Param[dealerCode=" & dealerCode & _
            ", branchCode=" & branchCode & ", month=" & month & _
            ", orgnzIdList=" & String.Join(",", orgnzIdList) & ", deliveryActCode=" & deliveryActCode & "]")

            Using query As New DBSelectQuery(Of DataTable)("SC3010202_015")
                Dim sql As New StringBuilder

                '$02 受注後フォロー機能開発 START
                'SQL文作成
                With sql
                    .Append(" SELECT /*SC3010202_015*/ ")
                    .Append("       SUM(CNT) AS SUMCNT ")
                    .Append("   FROM ( ")
                    .Append("     SELECT COUNT(1) AS CNT ")
                    .Append("       FROM TB_T_AFTER_ODR_ACT ")
                    .Append("      WHERE RSLT_DLR_CD = :DLR_CD ")
                    .Append("        AND RSLT_BRN_CD = :BRN_CD ")
                    .Append("        AND RSLT_STF_CD IN ( ")
                    .Append("         SELECT STF_CD ")
                    .Append("           FROM TB_M_STAFF ")
                    .Append("          WHERE DLR_CD = :DLR_CD ")
                    .Append("            AND BRN_CD = :BRN_CD ")
                    .Append("            AND INUSE_FLG = '1' ")
                    .Append("            AND ORGNZ_ID IN ( ")
                    .Append(ConvertOrgnzIdStr(orgnzIdList))
                    .Append("                )")
                    .Append("            )")
                    .Append("        AND AFTER_ODR_ACT_CD = :AFTER_ODR_ACT_CD ")
                    .Append("        AND RSLT_END_DATEORTIME >= TO_DATE(:RSLT_END_DATEORTIME,'YYYYMM') ")
                    .Append("      UNION ALL ")
                    .Append("     SELECT COUNT(1) AS CNT  ")
                    .Append("       FROM TB_H_AFTER_ODR_ACT ")
                    .Append("      WHERE RSLT_DLR_CD= :DLR_CD ")
                    .Append("        AND RSLT_BRN_CD = :BRN_CD ")
                    .Append("        AND RSLT_STF_CD IN ( ")
                    .Append("         SELECT STF_CD ")
                    .Append("           FROM TB_M_STAFF ")
                    .Append("          WHERE DLR_CD = :DLR_CD ")
                    .Append("            AND BRN_CD = :BRN_CD ")
                    .Append("            AND INUSE_FLG = '1' ")
                    .Append("            AND ORGNZ_ID IN ( ")
                    .Append(ConvertOrgnzIdStr(orgnzIdList))
                    .Append("                )")
                    .Append("            )")
                    .Append("        AND AFTER_ODR_ACT_CD = :AFTER_ODR_ACT_CD ")
                    .Append("        AND RSLT_END_DATEORTIME >= TO_DATE(:RSLT_END_DATEORTIME,'YYYYMM') ")
                    .Append("        ) ")

                End With

                query.CommandText = sql.ToString()
                'バインド変数
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.Char, branchCode)
                query.AddParameterWithTypeValue("AFTER_ODR_ACT_CD", OracleDbType.Char, deliveryActCode)
                query.AddParameterWithTypeValue("RSLT_END_DATEORTIME", OracleDbType.Varchar2, month)
                '$02 受注後フォロー機能開発 END

                '検索結果返却
                Dim dt As DataTable = query.GetData()

                If dt.Equals(Nothing) Then
                    Logger.Info("GetResultDeliveryOfTeam End Ret[0]")
                    Return 0
                Else
                    Logger.Info("GetResultDeliveryOfTeam End Ret[" & dt.Rows(0).Item("SUMCNT") & "]")
                    Return dt.Rows(0).Item("SUMCNT")
                End If
            End Using
        End Function

        ''' <summary>
        ''' 店舗の納車実績値を取得する。
        ''' </summary>
        ''' <param name="dealerCode">検索対象販売点コード</param>
        ''' <param name="branchCode">検索対象店舗コード</param>
        ''' <param name="Month">検索対象年月(YYYYMM)</param>
        ''' <param name="deliveryActCode">納車活動コード</param>
        ''' <returns>処理結果</returns>
        ''' <remarks></remarks>
        Public Function GetResultDeliveryOfBranch(ByVal dealerCode As String, ByVal branchCode As String, _
                                                  ByVal month As String, ByVal deliveryActCode As String) As Integer

            Logger.Info("GetResultDeliveryOfBranch Start Param[dealerCode=" & dealerCode & _
                        ", branchCode=" & branchCode & ", month=" & month & _
                        ", deliveryActCode=" & deliveryActCode & "]")

            Using query As New DBSelectQuery(Of DataTable)("SC3010202_016")
                Dim sql As New StringBuilder

                '$02 受注後フォロー機能開発 START
                'SQL文作成
                With sql
                    .Append(" SELECT /*SC3010202_016*/ ")
                    .Append("        SUM(CNT) AS SUMCNT ")
                    .Append("   FROM ( ")
                    .Append("     SELECT COUNT(1) AS CNT ")
                    .Append("       FROM TB_T_AFTER_ODR_ACT ")
                    .Append("      WHERE RSLT_DLR_CD = :DLR_CD ")
                    .Append("        AND RSLT_BRN_CD = :BRN_CD ")
                    .Append("        AND RSLT_STF_CD IN ( ")
                    .Append("         SELECT STF_CD ")
                    .Append("           FROM TB_M_STAFF ")
                    .Append("          WHERE DLR_CD = :DLR_CD ")
                    .Append("            AND BRN_CD = :BRN_CD ")
                    .Append("            AND INUSE_FLG = '1' ")
                    .Append("            )")
                    .Append("        AND AFTER_ODR_ACT_CD = :AFTER_ODR_ACT_CD ")
                    .Append("        AND RSLT_END_DATEORTIME >= TO_DATE(:RSLT_END_DATEORTIME,'YYYYMM') ")
                    .Append("      UNION ALL ")
                    .Append("     SELECT COUNT(1) AS CNT  ")
                    .Append("       FROM TB_H_AFTER_ODR_ACT ")
                    .Append("      WHERE RSLT_DLR_CD= :DLR_CD ")
                    .Append("        AND RSLT_BRN_CD = :BRN_CD ")
                    .Append("        AND RSLT_STF_CD IN ( ")
                    .Append("         SELECT STF_CD ")
                    .Append("           FROM TB_M_STAFF ")
                    .Append("          WHERE DLR_CD = :DLR_CD ")
                    .Append("            AND BRN_CD = :BRN_CD ")
                    .Append("            AND INUSE_FLG = '1' ")
                    .Append("            )")
                    .Append("        AND AFTER_ODR_ACT_CD = :AFTER_ODR_ACT_CD ")
                    .Append("        AND RSLT_END_DATEORTIME >= TO_DATE(:RSLT_END_DATEORTIME,'YYYYMM') ")
                    .Append("        ) ")

                End With

                query.CommandText = sql.ToString()
                'バインド変数
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.Char, branchCode)
                query.AddParameterWithTypeValue("AFTER_ODR_ACT_CD", OracleDbType.Char, deliveryActCode)
                query.AddParameterWithTypeValue("RSLT_END_DATEORTIME", OracleDbType.Varchar2, month)
                '$02 受注後フォロー機能開発 END

                '検索結果返却
                Dim dt As DataTable = query.GetData()

                If dt.Equals(Nothing) Then
                    Logger.Info("GetResultDeliveryOfBranch End Ret[0]")
                    Return 0
                Else
                    Logger.Info("GetResultDeliveryOfBranch End Ret[" & dt.Rows(0).Item("SUMCNT") & "]")
                    Return dt.Rows(0).Item("SUMCNT")
                End If
            End Using
        End Function

#End Region

#End Region

#Region "組織情報リスト取得"

        '$02 受注後フォロー機能開発 START
        ''' <summary>
        ''' 組織情報リストを取得する
        ''' </summary>
        ''' <param name="dealerCode">検索対象販売点コード</param>
        ''' <param name="branchCode">検索対象店舗コード</param>
        ''' <returns>組織情報リスト</returns>
        ''' <remarks></remarks>
        Public Function GetTeamList(ByVal dealerCode As String, ByVal branchCode As String) _
            As SC3010202DataSet.SC3010202OrganizationInfoDataTable

            Logger.Info("GetTeamList Start Param[dealerCode=" & dealerCode & _
            ", branchCode=" & branchCode & "]")

            Dim dt As SC3010202DataSet.SC3010202OrganizationInfoDataTable = Nothing

            Using query As New DBSelectQuery(Of SC3010202DataSet.SC3010202OrganizationInfoDataTable)("SC3010202_017")
                Dim sql As New StringBuilder

                'SQL文作成
                With sql

                    .Append(" SELECT /* SC3010202_017 */ ")
                    .Append("        ORGNZ_ID ")
                    .Append("      , PARENT_ORGNZ_ID ")
                    .Append("   FROM TB_M_ORGANIZATION ")
                    .Append("  WHERE DLR_CD = :DLR_CD ")
                    .Append("    AND BRN_CD = :BRN_CD ")
                    .Append("    AND INUSE_FLG = '1' ")

                End With

                query.CommandText = sql.ToString()
                'バインド変数
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.Char, branchCode)

                'クエリ実行
                dt = query.GetData()
            End Using

            Logger.Info("GetTeamList End Ret[" & dt.TableName & "[Count =" & dt.Count & "]]")
            Return dt

        End Function
        '$02 受注後フォロー機能開発 END

#End Region

#Region "システム設定取得"

        '$02 受注後フォロー機能開発 START
        ''' <summary>
        ''' システム設定から設定値を取得する
        ''' </summary>
        ''' <param name="settingName">システム設定名</param>
        ''' <returns>システム設定値</returns>
        ''' <remarks></remarks>
        Public Function GetSytemSetting(ByVal settingName As String) As String

            Logger.Info("GetSytemSetting Start Param[settingName=" & settingName & "]")

            Using query As New DBSelectQuery(Of DataTable)("SC3010202_018")
                Dim sql As New StringBuilder

                'SQL文作成
                With sql

                    .Append(" SELECT /* SC3010202_018 */ ")
                    .Append("       SETTING_VAL ")
                    .Append("   FROM TB_M_SYSTEM_SETTING ")
                    .Append("  WHERE SETTING_NAME = :SETTING_NAME ")

                End With

                query.CommandText = sql.ToString()
                'バインド変数
                query.AddParameterWithTypeValue("SETTING_NAME", OracleDbType.NVarchar2, settingName)

                Dim dt As DataTable = query.GetData()

                If dt.Rows.Count = 0 Then
                    Logger.Info("GetSytemSetting End Ret[" & String.Empty & "]")
                    Return String.Empty
                Else
                    Logger.Info("GetSytemSetting End Ret[" & dt.Rows(0).Item("SETTING_VAL") & "]")
                    Return dt.Rows(0).Item("SETTING_VAL")
                End If

            End Using

        End Function
        '$02 受注後フォロー機能開発 END

#End Region

#Region "組織リストID変換"

        '$02 受注後フォロー機能開発 START
        ''' <summary>
        ''' 組織IDリストをカンマ区切りの文字列に変換します。
        ''' </summary>
        ''' <param name="orgnzIdList">組織IDリスト</param>
        ''' <returns>組織IDリストをカンマ区切りにした文字列</returns>
        ''' <remarks></remarks>
        Private Function ConvertOrgnzIdStr(ByVal orgnzIdList As List(Of String)) As String

            Logger.Info("ConvertOrgnzIdStr Start Param[orgnzIdList=" & String.Join(",", orgnzIdList) & "]")

            Dim result As New StringBuilder
            Dim isFirst As Boolean = True
            For Each orgnzId As String In orgnzIdList

                If isFirst Then
                    isFirst = False
                Else
                    result.Append(",")
                End If

                result.Append(orgnzId)
            Next

            Logger.Info("ConvertOrgnzIdStr End Ret[" & result.ToString & "]")
            Return result.ToString
        End Function
        '$02 受注後フォロー機能開発 END
#End Region


#End Region

    End Class

End Namespace
Partial Class SC3010202DataSet
End Class
