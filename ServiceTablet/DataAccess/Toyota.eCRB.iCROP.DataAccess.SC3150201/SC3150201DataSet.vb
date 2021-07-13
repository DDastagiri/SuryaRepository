'------------------------------------------------------------------------------
'SC3150201DataSet.vb
'------------------------------------------------------------------------------
'機能：TCステータスモニター_データセット
'補足：
'作成：2013/02/21 TMEJ 成澤
'更新：2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
'更新：2013/12/12 TMEJ 成澤　IT9611_次世代サービス 工程管理機能開発
'更新：2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発
'更新：
'------------------------------------------------------------------------------
Option Strict On
Option Explicit On

Imports System.Text
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Oracle.DataAccess.Client
Imports System.Globalization


Namespace SC3150201DataSetTableAdapters
    Public Class SC3150201StallInfoDataTableAdapter
        Inherits Global.System.ComponentModel.Component
#Region "定数"
        '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
        ''' <summary>
        ''' 最小日付
        ''' </summary>
        ''' <remarks></remarks>
        Private Const MINDATE As String = "1900/01/01 00:00:00"
        ''' <summary>
        ''' 着工指示区分 "0":未着工 
        ''' </summary>
        ''' <remarks></remarks>
        Private Const CN_INSTRUCT_0 As String = "0"
        ''' <summary>
        ''' 着工指示区分 "2":着工指示 
        ''' </summary>
        ''' <remarks></remarks>
        Private Const CN_INSTRUCT_2 As String = "2"

        ''' <summary>
        ''' ストール利用ステータス"00":着工指示待ち
        ''' </summary>
        ''' <remarks></remarks>
        Private Const SU_STATUS_00 As String = "00"
        ''' <summary>
        ''' ストール利用ステータス"01":作業開始待ち
        ''' </summary>
        ''' <remarks></remarks>
        Private Const SU_STATUS_01 As String = "01"
        ''' <summary>
        ''' ストール利用ステータス"02":作業中
        ''' </summary>
        ''' <remarks></remarks>
        Private Const SU_STATUS_02 As String = "02"
        ''' <summary>
        ''' ストール利用ステータス"07":未来店客
        ''' </summary>
        ''' <remarks></remarks>
        Private Const SU_STATUS_07 As String = "07"
        ''' <summary>
        ''' サービスステータス"02":キャンセル
        ''' </summary>
        ''' <remarks></remarks>
        Private Const SVC_STATUS_02 As String = "02"
        ''' <summary>
        ''' キャンセルフラグ"0"
        ''' </summary>
        ''' <remarks></remarks>
        Private Const CANCEL_FLG_0 As String = "0"
        ''' <summary>
        ''' キャンセルフラグ"0"
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TEMP_FLG_0 As String = "0"
        '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        '2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 START
        ''' <summary>
        ''' ストール利用ステータス"02":作業中
        ''' </summary>
        ''' <remarks></remarks>
        Private Const SU_STATUS_04 As String = "04"

        '2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 END
#End Region

        ''' <summary>
        ''' ログインアカウントが所属するストール情報の取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="account">ログインアカウント</param>
        ''' <param name="stallId">ストールID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <History>
        ''' 2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
        ''' </History>
        ''' 
        ''' <History>
        ''' 2013/12/12　TMEJ 成澤　IT9611_次世代サービス 工程管理機能開発
        ''' </History>
        Public Function GetStallInfo(ByVal dealerCode As String, _
                                     ByVal branchCode As String, _
                                     ByVal account As String, _
                                     ByVal stallId As Decimal) As SC3150201DataSet.SC3150201StallInfoDataTable

            Logger.Info("[S]GetStallInfo()")

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3150201DataSet.SC3150201StallInfoDataTable)("SC3150201_002")

                Dim sql As New StringBuilder

                ' SQL文の作成
                With sql

                    '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
                    '.Append("SELECT /* SC3150201_002 */ ")
                    '.Append("       T3.STALLID AS STALLID, ")
                    '.Append("       T4.PSTARTTIME AS PSTARTTIME, ")
                    '.Append("       T4.PENDTIME AS PENDTIME ")
                    '.Append("  FROM TBL_SSTAFF      T1, ")
                    '.Append("       TBL_WSTAFFSTALL T2, ")
                    '.Append("       TBL_STALL       T3, ")
                    '.Append("       TBL_STALLTIME   T4 ")
                    '.Append(" WHERE T2.DLRCD    = T1.DLRCD ")
                    '.Append("   AND T2.STRCD    = T1.STRCD ")
                    '.Append("   AND T2.STAFFCD  = T1.STAFFCD ")
                    '.Append("   AND T3.STALLID  = T2.STALLID ")
                    '.Append("   AND T4.DLRCD    = T2.DLRCD ")
                    '.Append("   AND T4.STRCD    = T2.STRCD")
                    '.Append("   AND T1.DLRCD    = :DLRCD")
                    '.Append("   AND T1.STRCD    = :STRCD")
                    '.Append("   AND T1.ACCOUNT  = :ACCOUNT ")
                    '.Append("   AND T2.WORKDATE = :WORKDATE ")

                    '2013/12/12　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 START

                    '.Append("SELECT /* SC3150201_002 */ ")
                    '.Append("       T3.STALLID AS STALLID ")
                    '.Append("     , T4.PSTARTTIME AS PSTARTTIME ")
                    '.Append("     , T4.PENDTIME AS PENDTIME ")
                    '.Append("  FROM TB_M_STAFF       T1 ")
                    '.Append("     , TB_M_STAFF_STALL T2 ")
                    '.Append("     , TBL_STALL        T3 ")
                    '.Append("     , TBL_STALLTIME    T4 ")
                    '.Append(" WHERE T1.STF_CD   = T2.STF_CD ")
                    '.Append("   AND T2.STALL_ID = T3.STALLID ")
                    '.Append("   AND T4.DLRCD    = T1.DLR_CD ")
                    '.Append("   AND T4.STRCD    = T1.BRN_CD ")
                    '.Append("   AND T1.DLR_CD   = :DLR_CD")
                    '.Append("   AND T1.BRN_CD   = :BRN_CD")
                    '.Append("   AND T1.STF_CD   = :STF_CD ")

                    '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

                    .Append("SELECT /* SC3150201_002 */ ")
                    .Append("       T3.STALLID AS STALLID ")
                    .Append("     , T4.PSTARTTIME AS PSTARTTIME ")
                    .Append("     , T4.PENDTIME AS PENDTIME ")

                    '引数にストールIdがない場合、スタッフコードで検索
                    If stallId = 0 Then
                        .Append("  FROM TB_M_STAFF       T1 ")
                        .Append("     , TB_M_STAFF_STALL T2 ")
                        .Append("     , TBL_STALL        T3 ")
                        .Append("     , TBL_STALLTIME    T4 ")
                        .Append(" WHERE T1.STF_CD   = T2.STF_CD ")
                        .Append("   AND T2.STALL_ID = T3.STALLID ")
                        .Append("   AND T4.DLRCD    = T1.DLR_CD ")
                        .Append("   AND T4.STRCD    = T1.BRN_CD ")
                        .Append("   AND T1.DLR_CD   = :DLR_CD")
                        .Append("   AND T1.BRN_CD   = :BRN_CD")
                        .Append("   AND T1.STF_CD   = :STF_CD ")
                    Else
                        .Append("  FROM TBL_STALL        T3 ")
                        .Append("     , TBL_STALLTIME    T4 ")
                        .Append(" WHERE T3.DLRCD   = T4.DLRCD ")
                        .Append("   AND T3.STRCD   = T4.STRCD ")
                        .Append("   AND T3.DLRCD   = :DLR_CD ")
                        .Append("   AND T3.STRCD   = :BRN_CD ")
                        .Append("   AND T3.STALLID = :STALL_ID ")
                    End If
                    '2013/12/12　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 END
                End With

                query.CommandText = sql.ToString()

                ' バインド変数定義
                '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
                'query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
                'query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.Varchar2, account)
                'query.AddParameterWithTypeValue("WORKDATE", OracleDbType.Char, workDate)

                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)  '販売店コード
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, branchCode)  '店舗コード

                '2013/12/12　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 START
                '引数にストールIdがない場合、スタッフコードで検索
                If stallId = 0 Then
                    query.AddParameterWithTypeValue("STF_CD", OracleDbType.NVarchar2, account) 'スタッフコード
                Else
                    query.AddParameterWithTypeValue("STALL_ID", OracleDbType.Decimal, stallId) 'ストールID
                End If
                '2013/12/12　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 END

                '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

                Logger.Info("[E]GetStallInfo()")

                ' 検索結果の返却
                Return query.GetData()

            End Using

        End Function

        ''' <summary>
        ''' 予約・実績チップ情報の取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="stallId">ストールID</param>
        ''' <param name="dateFrom">稼働時間From</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <History>
        ''' 2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
        ''' </History>
        Public Function GetResultChipInfo(ByVal dealerCode As String, _
                                          ByVal branchCode As String, _
                                          ByVal stallId As Decimal, _
                                          ByVal dateFrom As Date) As SC3150201DataSet.SC3150201ChipInfoDataTable


            Logger.Info("[S]GetResultChipInfo()")

            Dim sql As New StringBuilder


            ' SQL文の作成
            With sql
                '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
                '.Append("SELECT /* SC3150201_003 */")
                '.Append("       T3.VCLREGNO AS VCLREGNO")
                '.Append("     , T3.REZ_DELI_DATE AS REZ_DELI_DATE")
                '.Append("     , T3.STARTTIME AS STARTTIME")
                '.Append("     , T3.ENDTIME AS ENDTIME")
                '.Append("     , T3.RESULT_START_TIME AS RESULT_START_TIME")
                '.Append("     , T3.INSTRUCT AS INSTRUCT")
                '.Append("  FROM ( SELECT T1.VCLREGNO AS VCLREGNO")
                '.Append("              , T1.REZ_DELI_DATE AS REZ_DELI_DATE")
                '.Append("              , NVL(TO_DATE(T2.RESULT_START_TIME,  'YYYYMMDDHH24MISS'),T1.STARTTIME) AS STARTTIME")
                '.Append("              , NVL(TO_DATE(T2.RESULT_END_TIME,  'YYYYMMDDHH24MISS'), T1.ENDTIME) AS ENDTIME")
                '.Append("              , T1.INSTRUCT AS INSTRUCT")
                '.Append("              , T2.RESULT_START_TIME AS RESULT_START_TIME")
                '.Append("              , DECODE( T2.RESULT_STATUS, '20', 1, 2 ) AS SORTKEY1")
                '.Append("           FROM TBL_STALLREZINFO T1")
                '.Append("              , TBL_STALLPROCESS T2")
                '.Append("          WHERE T1.DLRCD = T2.DLRCD (+)")
                '.Append("            AND T1.STRCD = T2.STRCD (+)")
                '.Append("            AND T1.REZID = T2.REZID (+)")
                '.Append("            AND T1.STALLID = T2.RESULT_STALLID (+)")
                '.Append("            AND '20' = T2.RESULT_STATUS (+)")
                '.Append("            AND T1.DLRCD = :DLRCD1")
                '.Append("            AND T1.STRCD = :STRCD1")
                '.Append("            AND T1.STALLID = :RESULT_STALLID1")
                '.Append("            AND T1.STARTTIME >= TO_DATE(:RESULT_START_TIME1, 'YYYY/MM/DD HH24:MI:SS')")
                '.Append("            AND T1.STARTTIME <  TO_DATE(:RESULT_START_TIME2, 'YYYY/MM/DD HH24:MI:SS')")
                '.Append("            AND T1.STARTTIME <> T1.ENDTIME")
                '.Append("            AND T1.STATUS IN ('1', '2')")
                '.Append("            AND T1.ACTUAL_ETIME IS NULL")
                '.Append("            AND T1.CANCELFLG = '0'")
                '.Append("            AND T1.STOPFLG = '0'")
                '.Append("          ORDER BY SORTKEY1, STARTTIME")
                '.Append("       ) T3")
                '.Append(" WHERE ROWNUM = 1")


                .Append("SELECT /* SC3150201_003 */ ")
                .Append("       T5.INSTRUCT ")
                .Append("     , T5.SCHE_DELI_DATETIME AS REZ_DELI_DATE ")
                .Append("     , T5.STARTTIME ")
                .Append("     , T5.ENDTIME ")
                .Append("     , T5.RSLT_START_DATETIME AS RESULT_START_TIME ")
                .Append("     , T5.REG_NUM AS VCLREGNO ")
                .Append("  FROM ( SELECT DECODE(T3.STALL_USE_STATUS,:SU_STATUS_00,:CN_INSTRUCT_0,:SU_STATUS_07,:CN_INSTRUCT_0,:CN_INSTRUCT_2) AS INSTRUCT ")

                '2013/12/12 TMEJ 成澤　IT9611_次世代サービス 工程管理機能開発 START
                '.Append("              , DECODE(T1.SCHE_DELI_DATETIME,TO_DATE(:MINDATE, 'YYYY/MM/DD HH24:MI:SS'),'',TO_CHAR(T1.SCHE_DELI_DATETIME,'YYYYMMDDHH24MI')) AS SCHE_DELI_DATETIME ")
                '.Append("              , DECODE(T3.RSLT_START_DATETIME,TO_DATE(:MINDATE, 'YYYY/MM/DD HH24:MI:SS'),T3.SCHE_START_DATETIME, T3.RSLT_START_DATETIME) AS STARTTIME  ")
                '.Append("              , DECODE(T3.PRMS_END_DATETIME,TO_DATE(:MINDATE, 'YYYY/MM/DD HH24:MI:SS'),T3.SCHE_END_DATETIME, T3.PRMS_END_DATETIME) AS ENDTIME ")
                '.Append("              , DECODE(T3.RSLT_START_DATETIME,TO_DATE(:MINDATE, 'YYYY/MM/DD HH24:MI:SS'),Null,TO_CHAR(T3.RSLT_START_DATETIME,'YYYYMMDDHH24MI')) AS RSLT_START_DATETIME ")
                .Append("              , DECODE(T1.SCHE_DELI_DATETIME,:MINDATE,TO_DATE(Null),T1.SCHE_DELI_DATETIME) AS SCHE_DELI_DATETIME ")
                .Append("              , DECODE(T3.RSLT_START_DATETIME,:MINDATE,T3.SCHE_START_DATETIME, T3.RSLT_START_DATETIME) AS STARTTIME  ")
                .Append("              , DECODE(T3.PRMS_END_DATETIME,:MINDATE,T3.SCHE_END_DATETIME, T3.PRMS_END_DATETIME) AS ENDTIME ")
                .Append("              , DECODE(T3.RSLT_START_DATETIME,:MINDATE,TO_DATE(Null),T3.RSLT_START_DATETIME) AS RSLT_START_DATETIME ")
                '2013/12/12 TMEJ 成澤　IT9611_次世代サービス 工程管理機能開発 END

                .Append("              , T4.REG_NUM AS REG_NUM")
                .Append("              , DECODE(T3.STALL_USE_STATUS, :SU_STATUS_02 , 1, 2) AS SORTKEY1")
                .Append("           FROM TB_T_SERVICEIN T1")
                .Append("              , TB_T_JOB_DTL T2")
                .Append("              , TB_T_STALL_USE T3")
                .Append("              , TB_M_VEHICLE_DLR T4")
                .Append("          WHERE T1.SVCIN_ID = T2.SVCIN_ID(+)")
                .Append("            AND T2.JOB_DTL_ID = T3.JOB_DTL_ID(+)")
                .Append("            AND T1.DLR_CD = T4.DLR_CD(+)")
                .Append("            AND T1.VCL_ID = T4.VCL_ID(+)")
                .Append("            AND T1.DLR_CD = :DLR_CD")
                .Append("            AND T1.BRN_CD = :BRN_CD")
                .Append("            AND T3.DLR_CD = :DLR_CD")
                .Append("            AND T3.BRN_CD = :BRN_CD")
                .Append("            AND T3.STALL_ID = :STALL_ID")
                .Append("            AND T3.SCHE_START_DATETIME >= TO_DATE(:SCHE_START_DATETIME1, 'YYYY/MM/DD HH24:MI:SS')")
                .Append("            AND T3.SCHE_START_DATETIME <> T3.SCHE_END_DATETIME")
                .Append("            AND T2.CANCEL_FLG = :CANCEL_FLG_0 ")
                .Append("            AND T1.SVC_STATUS <> :SVC_STATUS_02")
                '2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 START
                '.Append("            AND T3.STALL_USE_STATUS IN(:SU_STATUS_00,:SU_STATUS_01,:SU_STATUS_02)")
                .Append("            AND T3.STALL_USE_STATUS IN(:SU_STATUS_00,:SU_STATUS_01,:SU_STATUS_02,:SU_STATUS_04)")
                '2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 END
                .Append("            AND T3.TEMP_FLG = :TEMP_FLG_0")
                .Append("          ORDER BY SORTKEY1, SCHE_START_DATETIME")
                .Append("       ) T5")
                .Append(" WHERE ROWNUM = 1")
                '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END
            End With

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3150201DataSet.SC3150201ChipInfoDataTable)("SC3150201_003")

                query.CommandText = sql.ToString()

                ' バインド変数定義
                Dim workTimeFrom As String = dateFrom.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture()) ' 稼働時間From
                '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
                'query.AddParameterWithTypeValue("DLRCD1", OracleDbType.Char, dealerCode)                ' 販売店コード
                'query.AddParameterWithTypeValue("STRCD1", OracleDbType.Char, branchCode)                ' 店舗コード
                'query.AddParameterWithTypeValue("RESULT_STALLID1", OracleDbType.Int64, stallId)         ' ストールID
                'query.AddParameterWithTypeValue("RESULT_START_TIME1", OracleDbType.Char, workTimeFrom)  ' 稼働時間From
                'query.AddParameterWithTypeValue("RESULT_START_TIME2", OracleDbType.Char, workTimeTo)    ' 稼働時間To

                '2013/12/12 TMEJ 成澤　IT9611_次世代サービス 工程管理機能開発 START
                'query.AddParameterWithTypeValue("MINDATE", OracleDbType.NVarchar2, MINDATE)
                query.AddParameterWithTypeValue("MINDATE", OracleDbType.Date, Date.Parse(MINDATE, CultureInfo.CurrentCulture()))
                '2013/12/12 TMEJ 成澤　IT9611_次世代サービス 工程管理機能開発 END
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)                ' 販売店コード
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, branchCode)                ' 店舗コード
                query.AddParameterWithTypeValue("STALL_ID", OracleDbType.Decimal, stallId)                ' ストールID
                query.AddParameterWithTypeValue("SCHE_START_DATETIME1", OracleDbType.NVarchar2, workTimeFrom)  ' 稼働時間From
                query.AddParameterWithTypeValue("SU_STATUS_00", OracleDbType.NVarchar2, SU_STATUS_00) 'ストール利用ステータス"00"
                query.AddParameterWithTypeValue("SU_STATUS_01", OracleDbType.NVarchar2, SU_STATUS_01) 'ストール利用ステータス"01"
                query.AddParameterWithTypeValue("SU_STATUS_02", OracleDbType.NVarchar2, SU_STATUS_02) 'ストール利用ステータス"02"
                query.AddParameterWithTypeValue("SU_STATUS_07", OracleDbType.NVarchar2, SU_STATUS_07) 'ストール利用ステータス"07"
                query.AddParameterWithTypeValue("CN_INSTRUCT_0", OracleDbType.NVarchar2, CN_INSTRUCT_0) '着工指示区分"0"
                query.AddParameterWithTypeValue("CN_INSTRUCT_2", OracleDbType.NVarchar2, CN_INSTRUCT_2) '着工指示区分"2"
                query.AddParameterWithTypeValue("SVC_STATUS_02", OracleDbType.NVarchar2, SVC_STATUS_02) 'サービスステータス"02"
                query.AddParameterWithTypeValue("CANCEL_FLG_0", OracleDbType.NVarchar2, CANCEL_FLG_0) 'キャンセルフラグ
                query.AddParameterWithTypeValue("TEMP_FLG_0", OracleDbType.NVarchar2, TEMP_FLG_0) '仮置きフラグ
                '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

                '2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 START
                query.AddParameterWithTypeValue("SU_STATUS_04", OracleDbType.NVarchar2, SU_STATUS_04) 'ストール利用ステータス"04"
                '2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 END

                Logger.Info("[E]GetResultChipInfo()")

                ' 検索結果の返却
                Return query.GetData()

            End Using

        End Function

        ''' <summary>
        ''' リフレッシュタイムの取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetRefreshTime(ByVal dealerCode As String, _
                                           ByVal branchCode As String) As SC3150201DataSet.SC3150201RefreshTimeDataTable

            Logger.Info("[S]GetRefreshTime()")

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3150201DataSet.SC3150201RefreshTimeDataTable)("SC3150201_001")

                Dim sql As New StringBuilder

                ' SQL文の作成
                With sql
                    .Append("  SELECT /* SC3150201_001 */ ")
                    .Append("         TCSTATUS_REFRESH_TIME")
                    .Append("    FROM TBL_SERVICEINI ")
                    .Append("   WHERE DLRCD = :DLRCD ")
                    .Append("     AND STRCD = :STRCD ")
                End With

                query.CommandText = sql.ToString()

                ' バインド変数定義
                '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
                'query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)

                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, branchCode)
                '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END
                Logger.Info("[E]GetRefreshTime()")

                ' 検索結果の返却
                Return query.GetData()

            End Using

        End Function

    End Class

End Namespace

Partial Class SC3150201DataSet
End Class
