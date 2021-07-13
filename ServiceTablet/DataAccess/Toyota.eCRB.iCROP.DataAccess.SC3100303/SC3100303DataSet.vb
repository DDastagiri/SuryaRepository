'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3100303DataSet.vb
'─────────────────────────────────────
'機能： 来店管理メインのデータセット
'補足： 
'作成： 2013/03/06 TMEJ 張	初版作成
'更新： 2012/04/25 TMEJ 張  ITxxxx_TSL自主研緊急対応（サービス）
'更新： 2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
'更新： 2014/02/26 TMEJ 張 IT9600_タブレット版SMB チーフテクニシャン機能開発
'更新： 2014/03/18 TMEJ 陳	TMEJ次世代サービス 工程管理機能開発
'更新： 2018/02/19 NSK 山田	REQ-SVT-TMT-20170615-001 来店管理に入庫予約を指定してROを発行する機能を追加
'更新：
'─────────────────────────────────────
Option Strict On
Option Explicit On

Imports System.Text
Imports System.Globalization
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic

Partial Class SC3100303DataSet

End Class

Namespace SC3100303DataSetTableAdapters
    Public Class SC3100303DataAdapter
        Inherits Global.System.ComponentModel.Component

#Region "定数"

        '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        ''' <summary>
        ''' 振当てステータス（振当済み）
        ''' </summary>
        Private Const AssignFinish As String = "2"

        ''' <summary>
        ''' 振当てステータス（退店）
        ''' </summary>
        Private Const DealerOut As String = "4"

        ''' <summary>
        ''' キャンセルフラグ(有効)
        ''' </summary>
        Private Const CancelFlagEffective As String = "0"

        '2014/02/26 TMEJ 張 IT9600_タブレット版SMB チーフテクニシャン機能開発 START
        ' ''' <summary>
        ' ''' 性別「0：男性」
        ' ''' </summary>
        ' ''' <remarks></remarks>
        'Private Const Male As String = "0"
        '2014/02/26 TMEJ 張 IT9600_タブレット版SMB チーフテクニシャン機能開発 END

        ''' <summary>
        ''' サービスステータス(未入庫)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const StatusNoIn As String = "00"

        ''' <summary>
        ''' 受付区分(予約客)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const AcceptanceTypeRez As String = "0"

        ''' <summary>
        ''' アプリケーションID
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ApplicationID As String = "SC3100303"

        ''' <summary>
        ''' DB日付省略値
        ''' </summary>
        ''' <remarks></remarks>
        Private Const MinDate As String = "1900/01/01 00:00:00"

        '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

#End Region

#Region "Select系"
        ''' <summary>
        ''' 店舗稼動時間情報の取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <returns></returns>
        ''' <history>
        ''' 2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
        ''' </history>
        Public Function GetBranchOperatingHours(ByVal dealerCode As String, _
                                                ByVal branchCode As String) As SC3100303DataSet.SC3100303BranchOperatingHoursDataTable

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. dealerCode={1}, branchCode={2}" _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name, dealerCode, branchCode))

            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* SC3100303_001 */ ")
                .Append("       T1.PSTARTTIME AS STARTTIME ")
                .Append("      ,T1.PENDTIME AS ENDTIME ")
                .Append("      ,T2.DELAYTIME ")
                .Append("      ,T2.REFRESHTIME ")
                .Append("FROM TBL_STALLTIME T1,  ")
                .Append("     (SELECT ")
                .Append("       VISIT_DELAY_WARNING_LT AS DELAYTIME ")
                .Append("      ,VISIT_MANAGEMENT_REFRESH_TIME AS REFRESHTIME ")
                .Append("      FROM TBL_SERVICEINI ")
                .Append("      WHERE DLRCD = :DLRCD ")
                .Append("            AND STRCD = :STRCD ")
                .Append("            AND ROWNUM = 1 ")
                .Append("     ) T2 ")
                .Append("WHERE T1.DLRCD = :DLRCD ")
                .Append("  AND T1.STRCD = :STRCD ")
                .Append(" AND ROWNUM = 1 ")
            End With

            Using query As New DBSelectQuery(Of SC3100303DataSet.SC3100303BranchOperatingHoursDataTable)("SC3100303_001")
                query.CommandText = sql.ToString()

                '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

                'query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)

                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, branchCode)

                '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E", System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return query.GetData()
            End Using

        End Function

        ''' <summary>
        ''' 来店チップの一覧を取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="dateFrom">稼働時間From</param>
        ''' <param name="dateTo">稼働時間To</param>
        ''' <returns></returns>
        ''' <history>
        ''' 2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
        ''' </history>
        Public Function GetVisitChips(ByVal dealerCode As String, _
                                      ByVal branchCode As String, _
                                      ByVal dateFrom As Date, _
                                      ByVal dateTo As Date) As SC3100303DataSet.SC3100303VisitChipDataTable

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. dealerCode={1}, branchCode={2}, dateFrom={3}, dateTo={4}" _
                                      , System.Reflection.MethodBase.GetCurrentMethod.Name, dealerCode, branchCode, dateFrom, dateTo))

            Dim sql As New StringBuilder

            '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

            'With sql
            '    .Append("SELECT ")
            '    .Append("  /* SC3100303_002 */ ")
            '    .Append("  NVL(T1.PREZID,T1.REZID) AS REZID ")                                                                  '予約ID
            '    .Append("  , NVL(MIN(T1.REZ_PICK_DATE), TO_CHAR(MIN(T1.STARTTIME), 'YYYYMMDDHH24MI')) AS REZ_PICK_DATE ")     '来店予定時間
            '    .Append("  , MAX(T1.VCLREGNO) AS VCLREGNO ")                                                                    '登録№
            '    .Append("  , MAX(T1.VEHICLENAME) AS VEHICLENAME ")                                                              '車名
            '    .Append("  , MAX(T1.CUSTOMERNAME) AS CUSTOMERNAME ")                                                            '氏名
            '    .Append("  , MAX(T1.NOSHOWFOLLOWFLG) AS NOSHOWFOLLOWFLG ")                                                      'NoShowフォローフラグ
            '    .Append("  , MAX(T1.UPDATE_COUNT)  AS UPDATE_COUNT ")                                                           '更新カウント
            '    .Append("  , MIN(CASE T1.CUSTOMERFLAG WHEN '0' THEN (SELECT NVL(RTRIM(T4.SEX), '0') ")                          '性別
            '    .Append("                                              FROM TBLORG_CUSTOMER T4 ")
            '    .Append("                                             WHERE T1.INSDID = T4.ORIGINALID(+) ")
            '    .Append("                                               AND T4.DELFLG <> '1') ")
            '    .Append("                                             WHEN '1' THEN (SELECT NVL(RTRIM(T5.SEX), '0') ")
            '    .Append("                                              FROM TBL_NEWCUSTOMER T5 ")
            '    .Append("                                             WHERE T1.INSDID = T5.CSTID(+) ")
            '    .Append("                                               AND T5.DELFLG <> '1') ")
            '    .Append("                                             ELSE '0' END) AS SEX ")
            '    .Append("  , MAX(DECODE(T2.FREZID, NULL, '0', '1')) AS VISITFLG ")                                              '来店フラグ(点滅用、1の場合点滅)
            '    .Append("FROM TBL_STALLREZINFO T1 ")
            '    .Append("  , (SELECT T3.FREZID AS FREZID ")
            '    .Append("       FROM TBL_SERVICE_VISIT_MANAGEMENT T3 ")
            '    .Append("      WHERE T3.DLRCD = :DLRCD ")
            '    .Append("         AND T3.STRCD = :STRCD ")
            '    .Append("         AND T3.FREZID <> -1 ")
            '    .Append("         AND T3.ASSIGNSTATUS NOT IN('2', '4') ")
            '    .Append("         AND T3.CALLNO IS NULL ")
            '    .Append("         AND T3.VISITTIMESTAMP >= :TODAY ")
            '    .Append("         AND T3.VISITTIMESTAMP <  :NEXTDAY) T2 ")
            '    .Append("WHERE T1.REZID = T2.FREZID(+) ")
            '    .Append("  AND T1.DLRCD = :DLRCD ")
            '    .Append("  AND T1.STRCD = :STRCD ")
            '    .Append("  AND T1.STARTTIME >= TO_DATE(:FROMTIME, 'YYYYMMDDHH24MI') ")
            '    .Append("  AND T1.ENDTIME > TO_DATE(:FROMTIME, 'YYYYMMDDHH24MI') ")
            '    .Append("  AND T1.STATUS IN ('1', '2') ")
            '    .Append("  AND T1.STOPFLG = '0' ")
            '    .Append("  AND T1.CANCELFLG = '0' ")
            '    .Append("  AND T1.STRDATE IS NULL ")
            '    .Append("  AND DECODE(T1.REZCHILDNO,0,1,0) + DECODE(T1.REZCHILDNO,999,1,0) = 0 ")
            '    .Append("  AND NOT EXISTS(SELECT 1 ")
            '    .Append("                   FROM TBL_STALLREZINFO R3 ")
            '    .Append("                  WHERE T1.DLRCD = R3.DLRCD ")
            '    .Append("                    AND T1.STRCD = R3.STRCD ")
            '    .Append("                    AND (T1.PREZID = R3.PREZID OR T1.REZID = R3.REZID) ")
            '    .Append("                    AND R3.STOPFLG = '0' ")
            '    .Append("                    AND R3.CANCELFLG = '0' ")
            '    .Append("                    AND T1.STRDATE IS NULL ")
            '    .Append("                    AND DECODE(R3.REZCHILDNO,0,1,0) + DECODE(R3.REZCHILDNO,999,1,0) = 0 ")
            '    .Append("                    AND NVL(R3.REZ_PICK_DATE,TO_CHAR(R3.STARTTIME,'YYYYMMDDHH24MI')) < :FROMTIME) ")
            '    .Append("  AND NOT EXISTS (SELECT T5.FREZID AS FREZID ")
            '    .Append("                    FROM TBL_SERVICE_VISIT_MANAGEMENT T5 ")
            '    .Append("                   WHERE T1.DLRCD = T5.DLRCD ")
            '    .Append("                     AND T1.STRCD = T5.STRCD ")
            '    .Append("                     AND (T1.PREZID = T5.FREZID OR T1.REZID = T5.FREZID) ")
            '    .Append("                     AND T5.FREZID <> -1 ")
            '    .Append("                     AND (T5.ASSIGNSTATUS IN('2', '4') ")
            '    .Append("                        OR T5.CALLNO IS NOT NULL) ")
            '    .Append("                     AND T5.CALLNO IS NOT NULL ")
            '    .Append("                     AND T5.VISITTIMESTAMP >= :TODAY ")
            '    .Append("                     AND T5.VISITTIMESTAMP <  :NEXTDAY) ")
            '    .Append(" GROUP BY NVL(T1.PREZID,T1.REZID) ")
            '    .Append("HAVING NVL(MIN(T1.REZ_PICK_DATE),TO_CHAR(MIN(T1.STARTTIME),'YYYYMMDDHH24MI')) BETWEEN :FROMTIME AND :TOTIME ")
            '    .Append(" ORDER BY REZ_PICK_DATE, VCLREGNO ")

            'End With

            With sql

                .AppendLine("  SELECT  /* SC3100303_002 */ ")
                .AppendLine("          T1.SVCIN_ID AS REZID ")
                .AppendLine("         ,CASE MAX(T1.SCHE_SVCIN_DATETIME)  ")
                .AppendLine("               WHEN :MINDATE THEN TO_CHAR(MIN(T3.SCHE_START_DATETIME), 'YYYYMMDDHH24MI') ")
                .AppendLine("               ELSE TO_CHAR(MAX(T1.SCHE_SVCIN_DATETIME), 'YYYYMMDDHH24MI') ")
                .AppendLine("          END AS REZ_PICK_DATE ")
                .AppendLine("         ,TRIM(T7.REG_NUM) AS VCLREGNO ")
                .AppendLine("         ,NVL(TRIM(T8.MODEL_NAME), TRIM(T6.NEWCST_MODEL_NAME)) AS VEHICLENAME ")
                .AppendLine("         ,TRIM(T5.CST_NAME) AS CUSTOMERNAME ")
                .AppendLine("         ,T1.NOSHOW_FLLW_FLG AS NOSHOWFOLLOWFLG ")
                .AppendLine("         ,T1.ROW_LOCK_VERSION AS UPDATE_COUNT ")
                '2014/02/26 TMEJ 張 IT9600_タブレット版SMB チーフテクニシャン機能開発 START
                '.AppendLine("         ,NVL(TRIM(T5.CST_GENDER), :SEX) AS SEX ")
                '2014/02/26 TMEJ 張 IT9600_タブレット版SMB チーフテクニシャン機能開発 END
                .AppendLine("         ,DECODE(T4.FREZID, NULL, '0', '1') AS VISITFLG ")
                '2014/02/26 TMEJ 張 IT9600_タブレット版SMB チーフテクニシャン機能開発 START
                .AppendLine("         ,T9.NAMETITLE_NAME ")            '敬称
                .AppendLine("         ,T9.POSITION_TYPE ")             '敬称位置
                '2014/02/26 TMEJ 張 IT9600_タブレット版SMB チーフテクニシャン機能開発 END
                '2018/02/19 NSK 山田 REQ-SVT-TMT-20170615-001 来店管理に入庫予約を指定してROを発行する機能を追加 START
                .AppendLine("         ,T10.CST_TYPE ")                 '顧客種別
                '2018/02/19 NSK 山田 REQ-SVT-TMT-20170615-001 来店管理に入庫予約を指定してROを発行する機能を追加 END
                .AppendLine("    FROM ")
                .AppendLine("          TB_T_SERVICEIN T1 ")
                .AppendLine("         ,TB_T_JOB_DTL T2 ")
                .AppendLine("         ,TB_T_STALL_USE T3 ")
                .AppendLine("         ,TBL_SERVICE_VISIT_MANAGEMENT T4 ")
                .AppendLine("         ,TB_M_CUSTOMER T5 ")
                .AppendLine("         ,TB_M_VEHICLE T6 ")
                .AppendLine("         ,TB_M_VEHICLE_DLR T7 ")
                .AppendLine("         ,TB_M_MODEL T8 ")
                '2014/02/26 TMEJ 張 IT9600_タブレット版SMB チーフテクニシャン機能開発 START
                .AppendLine("         ,(SELECT S1.NAMETITLE_CD ")
                .AppendLine("                 ,S1.NAMETITLE_NAME ")
                .AppendLine("                 ,S1.POSITION_TYPE ")
                .AppendLine("            FROM TB_M_NAMETITLE S1 ")
                .AppendLine("           WHERE S1.INUSE_FLG = N'1' ) T9 ")
                '2018/02/19 NSK 山田 REQ-SVT-TMT-20170615-001 来店管理に入庫予約を指定してROを発行する機能を追加 START
                .AppendLine("         ,TB_M_CUSTOMER_DLR T10 ")
                '2018/02/19 NSK 山田 REQ-SVT-TMT-20170615-001 来店管理に入庫予約を指定してROを発行する機能を追加 END
                '2014/02/26 TMEJ 張 IT9600_タブレット版SMB チーフテクニシャン機能開発 END
                .AppendLine("    WHERE ")
                .AppendLine("          T1.SVCIN_ID = T2.SVCIN_ID ")
                .AppendLine("      AND T2.JOB_DTL_ID = T3.JOB_DTL_ID ")
                .AppendLine("      AND T1.SVCIN_ID = T4.FREZID(+) ")
                .AppendLine("      AND T1.DLR_CD = T4.DLRCD(+) ")
                .AppendLine("      AND T1.BRN_CD = T4.STRCD(+) ")
                .AppendLine("      AND T1.CST_ID = T5.CST_ID(+) ")
                .AppendLine("      AND T1.VCL_ID = T6.VCL_ID(+) ")
                .AppendLine("      AND T1.DLR_CD = T7.DLR_CD(+) ")
                .AppendLine("      AND T1.VCL_ID = T7.VCL_ID(+) ")
                .AppendLine("      AND T6.MODEL_CD = T8.MODEL_CD(+) ")
                '2014/02/26 TMEJ 張 IT9600_タブレット版SMB チーフテクニシャン機能開発 START
                .AppendLine("      AND T5.NAMETITLE_CD = T9.NAMETITLE_CD(+) ")
                '2014/02/26 TMEJ 張 IT9600_タブレット版SMB チーフテクニシャン機能開発 END
                '2018/02/19 NSK 山田 REQ-SVT-TMT-20170615-001 来店管理に入庫予約を指定してROを発行する機能を追加 START
                .AppendLine("      AND T1.DLR_CD = T10.DLR_CD(+) ")
                .AppendLine("      AND T1.CST_ID = T10.CST_ID(+) ")
                '2018/02/19 NSK 山田 REQ-SVT-TMT-20170615-001 来店管理に入庫予約を指定してROを発行する機能を追加 END
                .AppendLine("      AND T1.DLR_CD = :DLR_CD ")
                .AppendLine("      AND T1.BRN_CD = :BRN_CD ")
                .AppendLine("      AND T1.SVC_STATUS = :SVC_STATUS_00 ")
                .AppendLine("      AND T1.ACCEPTANCE_TYPE = :ACCEPTANCE_TYPE_0 ")
                .AppendLine("      AND T1.RSLT_SVCIN_DATETIME = :MINDATE ")
                .AppendLine("      AND T3.DLR_CD = :DLR_CD ")
                .AppendLine("      AND T3.BRN_CD = :BRN_CD ")
                .AppendLine("      AND T3.SCHE_START_DATETIME >= :FROMTIME ")
                .AppendLine("      AND T3.SCHE_END_DATETIME > :FROMTIME ")
                .AppendLine("      AND T4.FREZID(+) <> -1 ")
                .AppendLine("      AND T4.ASSIGNSTATUS(+) NOT IN(:ASSIGNSTATUS_02, :ASSIGNSTATUS_04) ")
                .AppendLine("      AND T4.VISITTIMESTAMP(+) >= :TODAY ")
                .AppendLine("      AND T4.VISITTIMESTAMP(+) < :NEXTDAY ")
                .AppendLine("      AND T4.CALLNO IS NULL ")
                .AppendLine("      AND NOT EXISTS(SELECT T2.JOB_DTL_ID ")
                .AppendLine("                       FROM TB_T_JOB_DTL WT1 ")
                .AppendLine("                      WHERE T2.JOB_DTL_ID = WT1.JOB_DTL_ID ")
                .AppendLine("                        AND WT1.CANCEL_FLG <> :CANCEL_FLG_0 ")
                .AppendLine("                     ) ")
                .AppendLine("      AND NOT EXISTS(SELECT T1.SVCIN_ID ")
                .AppendLine("                       FROM TBL_SERVICE_VISIT_MANAGEMENT WT4 ")
                .AppendLine("                      WHERE WT4.DLRCD = :DLR_CD ")
                .AppendLine("                        AND WT4.STRCD = :BRN_CD ")
                .AppendLine("                        AND WT4.FREZID = T1.SVCIN_ID ")
                .AppendLine("                        AND WT4.FREZID <> -1 ")
                .AppendLine("                        AND (WT4.ASSIGNSTATUS IN(:ASSIGNSTATUS_02, :ASSIGNSTATUS_04)  ")
                .AppendLine("                             OR WT4.CALLNO IS NOT NULL) ")
                '2014/03/18 TMEJ 陳	TMEJ次世代サービス 工程管理機能開発 START
                '.AppendLine("                        AND WT4.CALLNO IS NOT NULL ")
                '2014/03/18 TMEJ 陳	TMEJ次世代サービス 工程管理機能開発 END
                .AppendLine("                        AND WT4.VISITTIMESTAMP >= :TODAY ")
                .AppendLine("                        AND WT4.VISITTIMESTAMP < :NEXTDAY ")
                .AppendLine("                      ) ")
                .AppendLine(" GROUP BY T1.SVCIN_ID ")
                .AppendLine("         ,T7.REG_NUM ")
                .AppendLine("         ,T8.MODEL_NAME ")
                .AppendLine("         ,T6.NEWCST_MODEL_NAME ")
                .AppendLine("         ,T5.CST_NAME ")
                .AppendLine("         ,T1.NOSHOW_FLLW_FLG ")
                .AppendLine("         ,T1.ROW_LOCK_VERSION ")
                .AppendLine("         ,T5.CST_GENDER ")
                .AppendLine("         ,DECODE(T4.FREZID, NULL, '0', '1') ")
                '2014/02/26 TMEJ 張 IT9600_タブレット版SMB チーフテクニシャン機能開発 START
                .AppendLine("         ,T9.NAMETITLE_NAME  ")
                .AppendLine("         ,T9.POSITION_TYPE ")
                '2018/02/19 NSK 山田 REQ-SVT-TMT-20170615-001 来店管理に入庫予約を指定してROを発行する機能を追加 START
                .AppendLine("         ,T10.CST_TYPE ")
                '2018/02/19 NSK 山田 REQ-SVT-TMT-20170615-001 来店管理に入庫予約を指定してROを発行する機能を追加 END
                '2014/02/26 TMEJ 張 IT9600_タブレット版SMB チーフテクニシャン機能開発 END
                .AppendLine(" HAVING   DECODE(MAX(T1.SCHE_SVCIN_DATETIME), :MINDATE ,MIN(T3.SCHE_START_DATETIME), MAX(T1.SCHE_SVCIN_DATETIME)) ")
                .AppendLine("          BETWEEN :FROMTIME ")
                .AppendLine("              AND :TOTIME ")
                .AppendLine(" ORDER BY REZ_PICK_DATE ")
                .AppendLine("         ,REG_NUM ")

            End With

            '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

            Using query As New DBSelectQuery(Of SC3100303DataSet.SC3100303VisitChipDataTable)("SC3100303_002")

                'SQLを格納
                query.CommandText = sql.ToString()

                '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

                'query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)

                '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

                query.AddParameterWithTypeValue("TODAY", OracleDbType.Date, dateFrom)
                query.AddParameterWithTypeValue("NEXTDAY", OracleDbType.Date, dateTo)

                '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

                'query.AddParameterWithTypeValue("FROMTIME", OracleDbType.Char, Format(dateFrom, "yyyyMMddHHmm"))
                'query.AddParameterWithTypeValue("TOTIME", OracleDbType.Char, Format(dateTo, "yyyyMMddHHmm"))

                query.AddParameterWithTypeValue("FROMTIME", OracleDbType.Date, dateFrom)
                query.AddParameterWithTypeValue("TOTIME", OracleDbType.Date, dateTo)

                query.AddParameterWithTypeValue("MINDATE", OracleDbType.Date, Date.Parse(MinDate, CultureInfo.InvariantCulture))
                '2014/02/26 TMEJ 張 IT9600_タブレット版SMB チーフテクニシャン機能開発 START
                'query.AddParameterWithTypeValue("SEX", OracleDbType.NVarchar2, Male)
                '2014/02/26 TMEJ 張 IT9600_タブレット版SMB チーフテクニシャン機能開発 END
                query.AddParameterWithTypeValue("ACCEPTANCE_TYPE_0", OracleDbType.NVarchar2, AcceptanceTypeRez)
                query.AddParameterWithTypeValue("SVC_STATUS_00", OracleDbType.NVarchar2, StatusNoIn)
                query.AddParameterWithTypeValue("ASSIGNSTATUS_02", OracleDbType.NVarchar2, AssignFinish)
                query.AddParameterWithTypeValue("ASSIGNSTATUS_04", OracleDbType.NVarchar2, DealerOut)
                query.AddParameterWithTypeValue("CANCEL_FLG_0", OracleDbType.NVarchar2, CancelFlagEffective)
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, branchCode)

                '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E", System.Reflection.MethodBase.GetCurrentMethod.Name))

                Return query.GetData()

            End Using

        End Function

        ''' <summary>
        ''' UPDATE_COUNTを取得
        ''' </summary>
        ''' <param name="rezid">サービス入庫ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' 2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
        ''' </history>
        Public Function GetUpdateCount(ByVal rezid As Long) _
                                       As SC3100303DataSet.SC3100303UpdateCntDataTable

            '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

            'Public Function GetUpdateCount(ByVal rezid As Long _
            '                         , ByVal dlrCD As String _
            '                         , ByVal strCD As String) _
            '                           As SC3100303DataSet.SC3100303UpdateCntDataTable


            '    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. rezid={1}, dealerCode={2}, branchCode={3}" _
            '                           , System.Reflection.MethodBase.GetCurrentMethod.Name, rezid, dlrCD, strCD))

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. SVCIN_ID={1}" _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name, rezid))


            '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END


            Dim sql As New StringBuilder


            '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

            'With sql
            '    .Append("SELECT /* SC3100303_003 */ ")
            '    .Append("      NVL(UPDATE_COUNT, 0) AS UPDATE_COUNT ")
            '    .Append("FROM TBL_STALLREZINFO  ")
            '    .Append("WHERE DLRCD = :DLRCD ")
            '    .Append("  AND STRCD = :STRCD ")
            '    .Append("  AND REZID = :REZID ")
            '    .Append("  AND ROWNUM = 1 ")
            'End With

            With sql

                .AppendLine(" SELECT  /* SC3100303_003 */ ")
                .AppendLine("         T1.ROW_LOCK_VERSION AS UPDATE_COUNT ")
                .AppendLine("   FROM  TB_T_SERVICEIN T1 ")
                .AppendLine("  WHERE  T1.SVCIN_ID = :REZID ")

            End With

            '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

            Using query As New DBSelectQuery(Of SC3100303DataSet.SC3100303UpdateCntDataTable)("SC3100303_003")

                'SQL格納
                query.CommandText = sql.ToString()

                'バインド変更

                'サービス入庫ID
                query.AddParameterWithTypeValue("REZID", OracleDbType.Int64, rezid)

                '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

                'query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrCD)
                'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strCD)

                '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E", System.Reflection.MethodBase.GetCurrentMethod.Name))

                Return query.GetData()

            End Using

        End Function

        ''' <summary>
        ''' 今日営業時間内の点滅チップのidを取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="dateFrom">稼働時間From</param>
        ''' <param name="dateTo">稼働時間To</param>
        ''' <returns></returns>
        ''' <history>
        ''' 2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
        ''' </history>
        Public Function GetSwitchChipId(ByVal dealerCode As String, _
                                        ByVal branchCode As String, _
                                        ByVal dateFrom As Date, _
                                        ByVal dateTo As Date) As SC3100303DataSet.SC3100303SwitchChipIdDataTable

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. dealerCode={1}, branchCode={2}, dateFrom={3}, dateTo={4}" _
                                      , System.Reflection.MethodBase.GetCurrentMethod.Name, dealerCode, branchCode, dateFrom, dateTo))

            Dim sql As New StringBuilder
            With sql
                .Append(" SELECT /* SC3100303_004 */")
                .Append("      FREZID AS REZID  ")
                .Append(" FROM TBL_SERVICE_VISIT_MANAGEMENT T1  ")
                .Append(" WHERE T1.DLRCD = :DLRCD ")
                .Append("   AND T1.STRCD = :STRCD ")
                .Append("   AND T1.FREZID <> -1 ")

                '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

                '.Append("   AND T1.ASSIGNSTATUS NOT IN('2', '4') ")
                .Append("   AND T1.ASSIGNSTATUS NOT IN(N'2', N'4') ")

                '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

                .Append("   AND T1.CALLNO IS NULL ")
                .Append("   AND T1.VISITTIMESTAMP >= :STARTDATE ")
                .Append("   AND T1.VISITTIMESTAMP < :ENDDATE ")
            End With

            Using query As New DBSelectQuery(Of SC3100303DataSet.SC3100303SwitchChipIdDataTable)("SC3500101_004")
                query.CommandText = sql.ToString()


                '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

                'query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, branchCode)

                '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END


                query.AddParameterWithTypeValue("STARTDATE", OracleDbType.Date, dateFrom)
                query.AddParameterWithTypeValue("ENDDATE", OracleDbType.Date, dateTo)

                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E", System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return query.GetData()
            End Using

        End Function

        '2013/04/25 TMEJ 張 ITxxxx_TSL自主研緊急対応（サービス） START
        ''' <summary>
        ''' 来店実績台数を取得
        ''' </summary>
        ''' <param name="dlrCD">販売店コード</param>
        ''' <param name="strCD">店舗コード</param>
        ''' <param name="dateFrom">稼働時間From</param>
        ''' <param name="dateTo">稼働時間To</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' 2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
        ''' </history>
        Public Function GetVstCarCnt(ByVal dlrCD As String _
                                   , ByVal strCD As String _
                                   , ByVal dateFrom As Date, _
                                     ByVal dateTo As Date) As SC3100303DataSet.SC3100303VstCarCntDataTable

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. dealerCode={1}, branchCode={2}, dateFrom={3}, dateTo={4}" _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name, dlrCD, strCD, dateFrom, dateTo))

            Dim sql As New StringBuilder
            With sql
                .AppendLine("SELECT /* SC3100303_006 */ ")
                .AppendLine("       COUNT(*) AS VST_CAR_CNT ")
                .AppendLine("  FROM TBL_SERVICE_VISIT_MANAGEMENT  ")
                .AppendLine(" WHERE DLRCD = :DLRCD ")
                .AppendLine("   AND STRCD = :STRCD ")

                '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

                '.AppendLine("   AND ASSIGNSTATUS NOT IN('4') ")
                .AppendLine("   AND ASSIGNSTATUS NOT IN(N'4') ")

                '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

                .AppendLine("   AND VISITTIMESTAMP >= :STARTDATE ")
                .AppendLine("   AND VISITTIMESTAMP <  :ENDDATE ")
            End With

            Using query As New DBSelectQuery(Of SC3100303DataSet.SC3100303VstCarCntDataTable)("SC3100303_006")
                query.CommandText = sql.ToString()


                '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

                'query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrCD)
                'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strCD)
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrCD)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, strCD)

                '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END


                query.AddParameterWithTypeValue("STARTDATE", OracleDbType.Date, dateFrom)
                query.AddParameterWithTypeValue("ENDDATE", OracleDbType.Date, dateTo)

                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E", System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return query.GetData()
            End Using

        End Function
        '2013/04/25 TMEJ 張 ITxxxx_TSL自主研緊急対応（サービス） END

        '2018/02/19 NSK 山田 REQ-SVT-TMT-20170615-001 来店管理に入庫予約を指定してROを発行する機能を追加 START
        ''' <summary>
        ''' 顧客車両情報を取得
        ''' </summary>
        ''' <param name="svcInId">サービス入庫ID</param>
        ''' <returns>顧客車両情報</returns>
        ''' <remarks></remarks>
        Public Function GetCstVehicle(ByVal svcInId As Decimal) _
                                      As SC3100303DataSet.SC3100303CstVehicleDataTable

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. SVCIN_ID={1}" _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name, svcInId))

            Dim sql As New StringBuilder

            With sql

                .AppendLine(" SELECT /* SC3100303_007 */ ")
                .AppendLine("          T2.DMS_CST_CD_DISP ")
                .AppendLine("         ,T2.DMS_CST_CD ")
                .AppendLine("         ,T2.CST_NAME ")
                .AppendLine("         ,T2.CST_PHONE ")
                .AppendLine("         ,T2.CST_MOBILE ")
                .AppendLine("         ,T2.CST_EMAIL_1 ")
                .AppendLine("         ,T3.REG_NUM ")
                .AppendLine("         ,T4.VCL_VIN ")
                .AppendLine("         ,T4.VCL_KATASHIKI ")
                .AppendLine("         ,NVL(T5.MODEL_NAME,T4.NEWCST_MODEL_NAME) AS MODEL_NAME ")
                .AppendLine(" FROM ")
                .AppendLine("          TB_T_SERVICEIN T1 ")
                .AppendLine("         ,TB_M_CUSTOMER T2 ")
                .AppendLine("         ,TB_M_VEHICLE_DLR T3 ")
                .AppendLine("         ,TB_M_VEHICLE T4 ")
                .AppendLine("         ,TB_M_MODEL T5 ")
                .AppendLine(" WHERE ")
                .AppendLine("              T1.CST_ID = T2.CST_ID ")
                .AppendLine("          AND T1.DLR_CD = T3.DLR_CD ")
                .AppendLine("          AND T1.VCL_ID = T3.VCL_ID ")
                .AppendLine("          AND T1.VCL_ID = T4.VCL_ID ")
                .AppendLine("          AND T4.MODEL_CD = T5.MODEL_CD(+) ")
                .AppendLine("          AND T1.SVCIN_ID = :SVCIN_ID ")

            End With

            Using query As New DBSelectQuery(Of SC3100303DataSet.SC3100303CstVehicleDataTable)("SC3100303_007")

                'SQL格納
                query.CommandText = sql.ToString()

                'バインド変更

                'サービス入庫ID
                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, svcInId)

                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E" _
                                          , System.Reflection.MethodBase.GetCurrentMethod.Name))

                Return query.GetData()

            End Using
        End Function

        ''' <summary>
        ''' 来店者情報取得
        ''' </summary>
        ''' <param name="visitSeq">来店実績連番</param>
        ''' <returns>来店者情報</returns>
        ''' <remarks></remarks>
        Public Function GetContactInfo(ByVal visitSeq As Long) _
                                       As SC3100303DataSet.SC3100303ContactInfoDataTable

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. visitSeq={1}" _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name, visitSeq))

            Dim sql As New StringBuilder

            With sql

                .AppendLine(" SELECT /* SC3100303_008 */ ")
                .AppendLine("          T1.VISITNAME ")
                .AppendLine("         ,T1.VISITTELNO ")
                .AppendLine(" FROM ")
                .AppendLine("          TBL_SERVICE_VISIT_MANAGEMENT T1 ")
                .AppendLine(" WHERE ")
                .AppendLine("          T1.VISITSEQ = :VISITSEQ ")

            End With

            Using query As New DBSelectQuery(Of SC3100303DataSet.SC3100303ContactInfoDataTable)("SC3100303_008")

                'SQL格納
                query.CommandText = sql.ToString()

                'バインド変更

                '来店実績連番
                query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Long, visitSeq)

                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E" _
                                          , System.Reflection.MethodBase.GetCurrentMethod.Name))

                Return query.GetData()

            End Using
        End Function

        ''' <summary>
        ''' 未入庫予約存在チェック
        ''' </summary>
        ''' <param name="svcInId">サービス入庫ID</param>
        ''' <returns>データ件数</returns>
        ''' <remarks></remarks>
        Public Function IsNotCarInStatus(ByVal svcInId As Decimal) As Integer

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. SVCIN_ID={1}" _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name, svcInId))

            Dim sql As New StringBuilder

            With sql

                .AppendLine(" SELECT /* SC3100303_009 */ ")
                .AppendLine("          1 ")
                .AppendLine(" FROM ")
                .AppendLine("          TB_T_SERVICEIN T1 ")
                .AppendLine(" WHERE ")
                .AppendLine("              T1.SVCIN_ID = :SVCIN_ID ")
                .AppendLine("          AND T1.SVC_STATUS = :SVC_STATUS_00 ")
                .AppendLine(" FOR UPDATE WAIT 1 ")

            End With

            Using query As New DBSelectQuery(Of SC3100303DataSet.SC3100303VstCarCntDataTable)("SC3100303_009")

                'SQL格納
                query.CommandText = sql.ToString()

                'バインド変更

                'サービス入庫ID
                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, svcInId)
                'サービスステータス
                query.AddParameterWithTypeValue("SVC_STATUS_00", OracleDbType.NVarchar2, StatusNoIn)

                'SQLの実行
                Using dt As SC3100303DataSet.SC3100303VstCarCntDataTable = query.GetData()
                    ''終了ログの出力
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:ROWSCOUNT = {2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , dt.Rows.Count))

                    Return dt.Count

                End Using
            End Using
        End Function

        ''' <summary>
        ''' RO存在チェック
        ''' </summary>
        ''' <param name="visitSeq">来店実績連番</param>
        ''' <returns>データ件数</returns>
        ''' <remarks></remarks>
        Public Function CheckRoExists(ByVal visitSeq As Long) As Integer

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. VISIT_SEQ={1}" _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name, visitSeq))

            Dim sql As New StringBuilder

            With sql

                .AppendLine(" SELECT /* SC3100303_010 */ ")
                .AppendLine("          1 ")
                .AppendLine(" FROM ")
                .AppendLine("          TB_T_RO_INFO T1 ")
                .AppendLine(" WHERE ")
                .AppendLine("          T1.VISIT_ID = :VISIT_ID ")

            End With

            Using query As New DBSelectQuery(Of SC3100303DataSet.SC3100303VstCarCntDataTable)("SC3100303_010")

                'SQL格納
                query.CommandText = sql.ToString()

                'バインド変更

                '訪問ID
                query.AddParameterWithTypeValue("VISIT_ID", OracleDbType.Long, visitSeq)

                'SQLの実行
                Using dt As SC3100303DataSet.SC3100303VstCarCntDataTable = query.GetData()
                    ''終了ログの出力
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:ROWSCOUNT = {2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , dt.Rows.Count))

                    Return dt.Count

                End Using
            End Using
        End Function

        ''' <summary>
        ''' セッション格納用情報取得
        ''' </summary>
        ''' <param name="svcInId">サービス入庫ID</param>
        ''' <returns>セッション格納用情報</returns>
        ''' <remarks></remarks>
        Public Function GetSessionInfo(ByVal svcInId As Decimal) _
                                       As SC3100303DataSet.SC3100303SessionInfoDataTable

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. svcInId={1}" _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name, svcInId))

            Dim sql As New StringBuilder

            With sql

                .AppendLine(" SELECT /* SC3100303_011 */ ")
                .AppendLine("          T2.VCL_VIN ")
                .AppendLine("         ,T3.DMS_CST_CD_DISP ")
                .AppendLine("         ,T3.DMS_CST_CD ")
                .AppendLine(" FROM ")
                .AppendLine("          TB_T_SERVICEIN T1 ")
                .AppendLine("         ,TB_M_VEHICLE T2 ")
                .AppendLine("         ,TB_M_CUSTOMER T3 ")
                .AppendLine(" WHERE ")
                .AppendLine("              T1.VCL_ID = T2.VCL_ID ")
                .AppendLine("          AND T1.CST_ID = T3.CST_ID ")
                .AppendLine("          AND T1.SVCIN_ID = :SVCIN_ID ")

            End With

            Using query As New DBSelectQuery(Of SC3100303DataSet.SC3100303SessionInfoDataTable)("SC3100303_011")

                'SQL格納
                query.CommandText = sql.ToString()

                'バインド変更

                'サービス入庫ID
                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, svcInId)

                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E" _
                                          , System.Reflection.MethodBase.GetCurrentMethod.Name))

                Return query.GetData()

            End Using
        End Function

        ''' <summary>
        ''' 基幹作業内容IDを取得する
        ''' </summary>
        ''' <param name="svcInId">サービス入庫ID</param>
        ''' <param name="dlrCd">販売店コード</param>
        ''' <param name="brnCd">店舗コード</param>
        ''' <returns>基幹作業内容ID</returns>
        ''' <remarks></remarks>
        Public Function GetDmsJobDtlId(ByVal svcInId As Decimal, _
                                    ByVal dlrCd As String, _
                                    ByVal brnCd As String) _
                                    As SC3100303DataSet.SC3100303DmsJobDtlIdDataTable

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. svcInId={1},dealerCode={2}, branchCode={3}" _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name, svcInId, dlrCd, brnCd))

            Dim sql As New StringBuilder

            With sql

                .AppendLine(" SELECT /* SC3100303_012 */ ")
                .AppendLine("          T1.DMS_JOB_DTL_ID ")
                .AppendLine(" FROM ")
                .AppendLine("          TB_T_JOB_DTL T1 ")
                .AppendLine("         ,( ")
                .AppendLine("             SELECT ")
                .AppendLine("                 MIN(S1.JOB_DTL_ID) AS MIN_JOB_DTL_ID ")
                .AppendLine("             FROM ")
                .AppendLine("                 TB_T_JOB_DTL S1 ")
                .AppendLine("             WHERE ")
                .AppendLine("                     S1.SVCIN_ID = :SVCIN_ID ")
                .AppendLine("                 AND S1.DLR_CD = :DLR_CD ")
                .AppendLine("                 AND S1.BRN_CD = :BRN_CD ")
                .AppendLine("                 AND S1.CANCEL_FLG = :CANCEL_FLG_0 ")
                .AppendLine("          ) T2 ")
                .AppendLine(" WHERE ")
                .AppendLine("              T1.JOB_DTL_ID = T2.MIN_JOB_DTL_ID ")
                .AppendLine("          AND T1.SVCIN_ID = :SVCIN_ID ")

            End With

            Using query As New DBSelectQuery(Of SC3100303DataSet.SC3100303DmsJobDtlIdDataTable)("SC3100303_012")

                'SQL格納
                query.CommandText = sql.ToString()

                'バインド変更

                'サービス入庫ID
                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, svcInId)
                '販売店コード
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dlrCd)
                '店舗コード
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, brnCd)
                'キャンセルフラグ
                query.AddParameterWithTypeValue("CANCEL_FLG_0", OracleDbType.NVarchar2, CancelFlagEffective)

                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E" _
                                          , System.Reflection.MethodBase.GetCurrentMethod.Name))

                Return query.GetData()

            End Using

        End Function

        '2018/02/19 NSK 山田 REQ-SVT-TMT-20170615-001 来店管理に入庫予約を指定してROを発行する機能を追加 END
#End Region

#Region "Update系"

        ''' <summary>
        ''' NoShowフォローフラグを更新する
        ''' </summary>
        ''' <param name="rezid">サービス入庫ID</param>
        ''' <param name="noShowFollowFlg">NoShowフォローフラグ</param>
        ''' <param name="updateAccount">更新アカウント</param>
        ''' <param name="inPresentTime">現在日時</param>
        ''' <returns>正常終了：0、異常終了：エラーコード</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' 2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
        ''' </history>
        Public Function UpdateFollowFlg(ByVal rezid As Long _
                                      , ByVal noShowFollowFlg As String _
                                      , ByVal updateAccount As String _
                                      , ByVal inPresentTime As Date) As Integer


            '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

            'Public Function UpdateFollowFlg(ByVal rezid As Long _
            '                         , ByVal noShowFollowFlg As String _
            '                         , ByVal dlrCD As String _
            '                         , ByVal strCD As String _
            '                         , ByVal updateAccount As String) As Integer

            '    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. rezid={1}, updateAccount={2}, updateAccount={3}, updateAccount={4}, updateAccount={5}" _
            '                              , System.Reflection.MethodBase.GetCurrentMethod.Name, rezid, noShowFollowFlg, dlrCD, strCD, updateAccount))

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. SVCIN_ID={1}, SHOWFOLLOWFLG={2}, UPDATEACCOUNT={3}, PRESENTTIME={4}" _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name, rezid, noShowFollowFlg, updateAccount, inPresentTime))



            '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

            Dim sql As New StringBuilder

            '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

            'With sql
            '    .Append(" UPDATE /* SC3100303_005 */ ")
            '    .Append("        TBL_STALLREZINFO ")
            '    .Append("    SET NOSHOWFOLLOWFLG = :NOSHOWFOLLOWFLG ")
            '    .Append("      , UPDATEDATE = SYSDATE ")
            '    .Append("      , UPDATEACCOUNT = :UPDATEACCOUNT ")
            '    .Append("      , UPDATE_COUNT = UPDATE_COUNT+1 ")
            '    .Append("  WHERE REZID = :REZID ")
            '    .Append("      AND DLRCD = :DLRCD ")
            '    .Append("      AND STRCD = :STRCD ")
            'End With


            With sql

                .AppendLine(" UPDATE  /* SC3100303_005 */ ")
                .AppendLine("         TB_T_SERVICEIN T1 ")
                .AppendLine("    SET  T1.NOSHOW_FLLW_FLG = :NOSHOW_FLLW_FLG ")
                .AppendLine("  WHERE  T1.SVCIN_ID = :REZID ")

            End With

            '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

            Using query As New DBUpdateQuery("SC3100303_005")

                'SQL格納
                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("REZID", OracleDbType.Int64, rezid)


                '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

                'query.AddParameterWithTypeValue("NOSHOWFOLLOWFLG", OracleDbType.Char, noShowFollowFlg)
                'query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrCD)
                'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strCD)
                'query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Char, updateAccount)

                query.AddParameterWithTypeValue("NOSHOW_FLLW_FLG", OracleDbType.NVarchar2, noShowFollowFlg)

                '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E", System.Reflection.MethodBase.GetCurrentMethod.Name))

                Return query.Execute()

            End Using

        End Function

#End Region

    End Class
End Namespace

