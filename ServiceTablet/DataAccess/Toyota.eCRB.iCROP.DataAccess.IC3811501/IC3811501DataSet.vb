'-------------------------------------------------------------------------
'IC3811501DataSet.vb
'-------------------------------------------------------------------------
'機能：予約情報を取得するデータクラス
'補足：
'作成：2012/09/05 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.42）
'更新：2013/06/27 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発
'更新：2013/12/26 TMEJ 陳　  TMEJ次世代サービス 工程管理機能開発
'更新：2014/03/13 TMEJ 小澤 BTS-292対応
'更新：2015/09/07 TMEJ 春日井 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発
'更新：2017/01/06 NSK  竹中 TR-SVT-TMT-20160602-001 顧客がWelcomeボードに表示されない
'更新：
'
Imports System.Text
Imports System.Globalization
Imports System.Text.RegularExpressions
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess

Namespace IC3811501DataSetTableAdapters

    ''' <summary>
    ''' 予約情報を取得するデータクラス
    ''' </summary>
    ''' <remarks></remarks>
    Public Class IC3811501TableAdapter
        Inherits Global.System.ComponentModel.Component

        '2013/06/27 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
#Region "定数"

        ''' <summary>
        ''' サービスステータス（02：キャンセル）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ServiceStatusCancel As String = "02"

        ''' <summary>
        ''' 受付区分（0：予約客）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const AcceptanceTypeReserve As String = "0"

        ''' <summary>
        ''' キャンセルフラグ（0：有効）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const CancelTypeEffective As String = "0"

        ''' <summary>
        ''' 日付最小値文字列
        ''' </summary>
        ''' <remarks></remarks>
        Private Const DateMinValue As String = "1900/01/01 00:00:00"

        ''' <summary>
        ''' ROステータス（0：RO番号なし）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const OrderNoTypeOff As String = "0"

        ''' <summary>
        ''' ROステータス（1：RO番号あり）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const OrderNoTypeOn As String = "1"

        '2014/03/13 TMEJ 小澤 BTS-292対応 START
        '2015/09/07 TMEJ 春日井 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START

        ''' <summary>
        ''' 顧客車両区分（1：所有者）
        ''' </summary>
        ''' <remarks></remarks>
        'Private Const CustomerVehicleTypeOwner As String = "1"

        ''' <summary>
        ''' 顧客車両区分（4：保険）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const CustomerVehicleTypeInsurance As String = "4"

        ''' <summary>
        ''' オーナーチェンジフラグ（0：未設定）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const OwnerChangeFlagUnset As String = "0"

        '2015/09/07 TMEJ 春日井 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END

        '2014/03/13 TMEJ 小澤 BTS-292対応 END

        '2017/01/06 NSK  竹中 TR-SVT-TMT-20160602-001 顧客がWelcomeボードに表示されない START

        ''' <summary>
        ''' DBデフォルト値(基幹顧客コード)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const DmsCstCdDefault As String = " "

        '2017/01/06 NSK  竹中 TR-SVT-TMT-20160602-001 顧客がWelcomeボードに表示されない END

#End Region
        '2013/06/27 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

#Region "メソッド"

        ''' <summary>
        ''' 予約情報の取得
        ''' </summary>
        ''' <param name="inDealerCode">販売店コード</param>
        ''' <param name="inStoreCode">店舗コード</param>
        ''' <param name="inCustomerCode">顧客コード</param>
        ''' <param name="inVclRegNo">登録No.</param>
        ''' <param name="inVin">VIN</param>
        ''' <param name="inBaseDate">取得基準日</param>
        ''' <param name="isGetDmsCstFlg">自社客取得フラグ</param>
        ''' <returns>予約情報</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' 2013/06/27 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発
        ''' 2014/03/13 TMEJ 小澤 BTS-292対応
        ''' 2015/09/07 TMEJ 春日井 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発
        ''' 2017/01/06 NSK  竹中 TR-SVT-TMT-20160602-001 顧客がWelcomeボードに表示されない
        ''' </history>
        Public Function GetReserveInfo(ByVal inDealerCode As String, _
                                       ByVal inStoreCode As String, _
                                       ByVal inCustomerCode As String, _
                                       ByVal inVclRegNo As String, _
                                       ByVal inVin As String, _
                                       ByVal inBaseDate As String, _
                                       ByVal isGetDmsCstFlg As Boolean) As IC3811501DataSet.IC3811501ReservationListDataTable
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} inDealerCode:{2} inStoreCode:{3} inCustomerCode:{4} inVclRegNo:{5} inVin:{6} inBaseDate:{7}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , inDealerCode, inStoreCode, inCustomerCode, inVclRegNo, inVin, inBaseDate))

            'Public Function GetReserveInfo(ByVal inDealerCode As String, _
            '                               ByVal inStoreCode As String, _
            '                               ByVal inCustomerCode As String, _
            '                               ByVal inVclRegNo As String, _
            '                               ByVal inVin As String, _
            '                               ByVal inBaseDate As String) As IC3811501DataSet.IC3811501ReservationListDataTable
            '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
            '                , "{0}.{1} inDealerCode:{2} inStoreCode:{3} inCustomerCode:{4} inVclRegNo:{5} inVin:{6} inBaseDate:{7}" _
            '                , Me.GetType.ToString _
            '                , System.Reflection.MethodBase.GetCurrentMethod.Name _
            '                , inDealerCode, inStoreCode, inCustomerCode, inVclRegNo, inVin, inBaseDate))

            Dim sql As New StringBuilder

            'SQL文作成
            With sql
                '2013/06/27 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
                'sql.AppendLine("SELECT /*IC3811501_001*/")
                'sql.AppendLine("       T1.REZID ")
                'sql.AppendLine("      ,T2.REZ_PICK_DATE_MIN AS REZSTARTTIME ")
                'sql.AppendLine("      ,T2.REZ_DELI_DATE_MAX AS REZENDTIME ")
                'sql.AppendLine("      ,T1.ORDERNO ")
                'sql.AppendLine("      ,T3.MERCHANDISENAME AS SERVICENAME ")
                'sql.AppendLine("      ,NVL2(T1.ORDERNO, 1, 0) AS ROSTATUSCODE ")
                'sql.AppendLine("  FROM ")
                'sql.AppendLine("       TBL_STALLREZINFO T1, ")
                'sql.AppendLine("       ( ")
                'sql.AppendLine("        SELECT  ")
                'sql.AppendLine("               M2.DLRCD ")
                'sql.AppendLine("              ,M2.STRCD ")
                'sql.AppendLine("              ,M3.REZID ")
                'sql.AppendLine("              ,MIN(NVL(TO_DATE(M2.REZ_PICK_DATE,'YYYYMMDDHH24MI'),M2.STARTTIME)) AS REZ_PICK_DATE_MIN ")
                'sql.AppendLine("              ,MAX(NVL(TO_DATE(M2.REZ_DELI_DATE,'YYYYMMDDHH24MI'),M2.ENDTIME)) AS REZ_DELI_DATE_MAX ")
                'sql.AppendLine("          FROM  ")
                'sql.AppendLine("               TBL_STALLREZINFO M2  ")
                'sql.AppendLine("              ,( ")
                'sql.AppendLine("                SELECT  ")
                'sql.AppendLine("                       M1.DLRCD ")
                'sql.AppendLine("                      ,M1.STRCD ")
                'sql.AppendLine("                      ,DECODE(M1.PREZID,-1,M1.REZID,NVL(M1.PREZID,M1.REZID)) AS REZID ")
                'sql.AppendLine("                  FROM  ")
                'sql.AppendLine("                       TBL_STALLREZINFO M1 ")
                'sql.AppendLine("                 WHERE  ")
                'sql.AppendLine("                       M1.DLRCD = :DLRCD ")
                'sql.AppendLine("                   AND M1.STRCD = :STRCD ")
                'sql.AppendLine("                   AND M1.CUSTCD = :CUSTCD ")
                'sql.AppendLine("                   AND M1.WALKIN = '0' ")
                'sql.AppendLine("                   AND (M1.PREZID IS NULL OR M1.PREZID = -1 OR M1.REZID = M1.PREZID)  ")
                'If (Not (String.IsNullOrEmpty(inVclRegNo)) AndAlso 0 < inVclRegNo.Length) AndAlso _
                '   (Not (String.IsNullOrEmpty(inVin)) AndAlso 0 < inVin.Length) Then
                '    '「車両登録No.」と「VIN」が両方存在している場合
                '    sql.AppendLine("   AND (M1.VCLREGNO = :VCLREGNO OR M1.VIN = :VIN)")

                'ElseIf Not (String.IsNullOrEmpty(inVclRegNo)) AndAlso 0 < inVclRegNo.Length Then
                '    '「車両登録No.」が存在している場合
                '    sql.AppendLine("   AND M1.VCLREGNO = :VCLREGNO")

                'ElseIf Not (String.IsNullOrEmpty(inVin)) AndAlso 0 < inVin.Length Then
                '    '「VIN」が存在している場合
                '    sql.AppendLine("   AND M1.VIN = :VIN")
                'End If
                'sql.AppendLine("                   AND NOT EXISTS ( ")
                'sql.AppendLine("                       SELECT 1 ")
                'sql.AppendLine("                         FROM TBL_STALLREZINFO M4 ")
                'sql.AppendLine("                        WHERE M4.DLRCD = M1.DLRCD ")
                'sql.AppendLine("                          AND M4.STRCD = M1.STRCD ")
                'sql.AppendLine("                          AND M4.REZID = M1.REZID ")
                'sql.AppendLine("                          AND M4.STOPFLG = '0' ")
                'sql.AppendLine("                          AND M4.CANCELFLG = '1') ")
                'sql.AppendLine("               ) M3 ")
                'sql.AppendLine("         WHERE  ")
                'sql.AppendLine("               M2.DLRCD = M3.DLRCD ")
                'sql.AppendLine("           AND M2.STRCD = M3.STRCD ")
                'sql.AppendLine("           AND (M2.REZID = M3.REZID OR M2.PREZID = M3.REZID) ")
                'sql.AppendLine("           AND M2.ACTUAL_STIME IS NULL ")
                'sql.AppendLine("         GROUP BY M2.DLRCD,M2.STRCD,M3.REZID ")
                'sql.AppendLine("       ) T2 ")
                'sql.AppendLine("      ,TBL_MERCHANDISEMST T3 ")
                'sql.AppendLine(" WHERE ")
                'sql.AppendLine("       T1.DLRCD = T2.DLRCD ")
                'sql.AppendLine("   AND T1.STRCD = T2.STRCD ")
                'sql.AppendLine("   AND T1.REZID = T2.REZID ")
                'sql.AppendLine("   AND T1.DLRCD = T3.DLRCD(+) ")
                'sql.AppendLine("   AND T1.MERCHANDISECD = T3.MERCHANDISECD(+) ")
                'sql.AppendLine("   AND TRUNC(T2.REZ_PICK_DATE_MIN) >= TRUNC(TO_DATE(:BASEDATE,'YYYYMMDDHH24MI'))  ")
                'sql.AppendLine(" ORDER BY REZSTARTTIME ")
                '2017/01/06 NSK  竹中 TR-SVT-TMT-20160602-001 顧客がWelcomeボードに表示されない START
                '.AppendLine("SELECT /* IC3811501_001 */")
                '.AppendLine("       K1.REZID ")
                '.AppendLine("      ,K1.REZSTARTTIME ")
                '.AppendLine("      ,K1.REZENDTIME ")
                '.AppendLine("      ,K1.ORDERNO ")
                '.AppendLine("      ,K1.SERVICENAME ")
                '.AppendLine("      ,K1.ROSTATUSCODE ")
                ''2013/12/26 TMEJ 陳　 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
                '.AppendLine("      ,K1.DMS_JOB_DTL_ID ")
                ''2013/12/26 TMEJ 陳　 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END
                '.AppendLine("  FROM ")
                '.AppendLine("       (SELECT T6.REZID ")
                '.AppendLine("              ,T6.REZSTARTTIME ")
                '.AppendLine("              ,T6.REZENDTIME ")
                '.AppendLine("              ,T6.ORDERNO ")
                '.AppendLine("              ,T6.SERVICENAME ")
                '.AppendLine("              ,T6.ROSTATUSCODE ")
                ''2013/12/26 TMEJ 陳　 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
                '.AppendLine("              ,T6.DMS_JOB_DTL_ID ")
                ''2013/12/26 TMEJ 陳　 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END
                ''2015/09/07 TMEJ 春日井 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START
                '.AppendLine("              ,T6.CST_VCL_TYPE ")
                ''2015/09/07 TMEJ 春日井 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END
                '.AppendLine("          FROM TBL_SERVICEIN_APPEND T1 ")
                '.AppendLine("              ,TB_M_CUSTOMER_DLR T2 ")
                '.AppendLine("              ,TB_M_CUSTOMER T3 ")
                '.AppendLine("              ,TB_M_VEHICLE_DLR T4 ")
                '.AppendLine("              ,TB_M_VEHICLE T5 ")
                ''2013/12/26 TMEJ 陳　 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
                ''.AppendLine("              ,(SELECT M2.JOB_DTL_ID AS REZID ")
                '.AppendLine("              ,(SELECT M1.SVCIN_ID AS REZID ")
                '.AppendLine("                      ,M2.DMS_JOB_DTL_ID ")
                ''2013/12/26 TMEJ 陳　 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END
                '.AppendLine("                      ,M1.CST_ID ")
                '.AppendLine("                      ,M1.VCL_ID ")
                '.AppendLine("                      ,M3.SCHE_START_DATETIME_MIN AS REZSTARTTIME ")
                '.AppendLine("                      ,M3.SCHE_END_DATETIME_MAX AS REZENDTIME ")
                '.AppendLine("                      ,TRIM(M1.RO_NUM) AS ORDERNO ")
                '.AppendLine("                      ,M4.SVC_CLASS_NAME AS SERVICENAME ")
                ''2013/12/26 TMEJ 陳　 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
                ''.AppendLine("                      ,NVL2(TRIM(M1.RO_NUM), :RO_NUM_1, :RO_NUM_0) AS ROSTATUSCODE ")
                '.AppendLine("                     ,CASE WHEN EXISTS(SELECT 1 FROM TB_T_SERVICEIN A1, TB_T_RO_INFO A2 WHERE A1.SVCIN_ID = A2.SVCIN_ID AND A1.SVCIN_ID = M2.SVCIN_ID) THEN :RO_NUM_1 ELSE :RO_NUM_0 END AS ROSTATUSCODE ")
                ''2013/12/26 TMEJ 陳　 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END
                ''2015/09/07 TMEJ 春日井 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START
                '.AppendLine("                      ,M1.CST_VCL_TYPE ")
                ''2015/09/07 TMEJ 春日井 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END
                '.AppendLine("                  FROM TB_T_SERVICEIN M1 ")
                '.AppendLine("                      ,TB_T_JOB_DTL M2 ")
                '.AppendLine("                      ,(SELECT T1.SVCIN_ID ")
                '.AppendLine("                              ,MIN(T2.JOB_DTL_ID) AS JOB_DTL_ID_MIN ")
                '.AppendLine("                              ,MIN(DECODE(T1.SCHE_SVCIN_DATETIME, :MINDATE, T3.SCHE_START_DATETIME, T1.SCHE_SVCIN_DATETIME)) AS SCHE_START_DATETIME_MIN ")
                '.AppendLine("                              ,MAX(DECODE(T1.SCHE_DELI_DATETIME, :MINDATE, T3.SCHE_END_DATETIME, T1.SCHE_DELI_DATETIME)) AS SCHE_END_DATETIME_MAX ")
                '.AppendLine("                          FROM TB_T_SERVICEIN T1 ")
                '.AppendLine("                              ,TB_T_JOB_DTL T2 ")
                '.AppendLine("                              ,TB_T_STALL_USE T3 ")
                '.AppendLine("                         WHERE T1.SVCIN_ID = T2.SVCIN_ID ")
                '.AppendLine("                           AND T2.JOB_DTL_ID = T3.JOB_DTL_ID ")
                '.AppendLine("                           AND T1.DLR_CD = :DLR_CD ")
                '.AppendLine("                           AND T1.BRN_CD = :BRN_CD ")
                '.AppendLine("                           AND T2.DLR_CD = :DLR_CD ")
                '.AppendLine("                           AND T2.BRN_CD = :BRN_CD ")
                '.AppendLine("                           AND T2.CANCEL_FLG = :CANCEL_FLG_0 ")
                '.AppendLine("                           AND T3.DLR_CD = :DLR_CD ")
                '.AppendLine("                           AND T3.BRN_CD = :BRN_CD ")
                '.AppendLine("                           AND T3.RSLT_START_DATETIME = :MINDATE ")
                '.AppendLine("                         GROUP BY T1.SVCIN_ID) M3 ")
                '.AppendLine("                      ,TB_M_SERVICE_CLASS M4 ")
                '.AppendLine("                 WHERE ")
                '.AppendLine("                       M1.SVCIN_ID = M2.SVCIN_ID ")
                '.AppendLine("                   AND M2.SVCIN_ID = M3.SVCIN_ID ")
                '.AppendLine("                   AND M2.JOB_DTL_ID = M3.JOB_DTL_ID_MIN ")
                '.AppendLine("                   AND M2.SVC_CLASS_ID = M4.SVC_CLASS_ID(+) ")
                '.AppendLine("                   AND M1.DLR_CD = :DLR_CD ")
                '.AppendLine("                   AND M1.BRN_CD = :BRN_CD ")
                '.AppendLine("                   AND NOT EXISTS (SELECT 1 ")
                '.AppendLine("                                     FROM TB_T_SERVICEIN F1 ")
                '.AppendLine("                                    WHERE F1.SVCIN_ID = M1.SVCIN_ID ")
                '.AppendLine("                                      AND F1.SVC_STATUS = :SVC_STATUS_02) ")
                '.AppendLine("                   AND M1.ACCEPTANCE_TYPE = :ACCEPTANCE_TYPE_0 ")
                '.AppendLine("                   AND M2.DLR_CD = :DLR_CD ")
                '.AppendLine("                   AND M2.BRN_CD = :BRN_CD ")
                '.AppendLine("                   AND M2.CANCEL_FLG = :CANCEL_FLG_0 ")
                '.AppendLine("                   AND M3.SCHE_START_DATETIME_MIN >= TRUNC(TO_DATE(:BASEDATE, 'YYYYMMDDHH24MI'))) T6 ")
                '.AppendLine("         WHERE T1.CST_ID = T2.CST_ID ")
                '.AppendLine("           AND T2.CST_ID = T3.CST_ID ")
                '.AppendLine("           AND T1.VCL_ID = T4.VCL_ID ")
                '.AppendLine("           AND T4.VCL_ID = T5.VCL_ID ")
                '.AppendLine("           AND T1.CST_ID = T6.CST_ID ")
                '.AppendLine("           AND T1.VCL_ID = T6.VCL_ID ")
                '.AppendLine("           AND T2.DLR_CD = :DLR_CD ")
                '.AppendLine("           AND T4.DLR_CD = :DLR_CD ")
                '.AppendLine("           AND T1.DMS_CST_CD = :DMS_CST_CD ")
                'If Not (String.IsNullOrEmpty(inVclRegNo)) AndAlso _
                '   Not (String.IsNullOrEmpty(inVin)) Then
                '    '「車両登録No.」と「VIN」が両方存在している場合
                '    .AppendLine("          AND (T5.VCL_VIN_SEARCH = UPPER(:VCL_VIN_SEARCH) ")
                '    .AppendLine("           OR T4.REG_NUM_SEARCH = UPPER(:REG_NUM_SEARCH)) ")

                'ElseIf Not (String.IsNullOrEmpty(inVclRegNo)) Then
                '    '「車両登録No.」が存在している場合
                '    .AppendLine("          AND T4.REG_NUM_SEARCH = UPPER(:REG_NUM_SEARCH) ")

                'ElseIf Not (String.IsNullOrEmpty(inVin)) Then
                '    '「VIN」が存在している場合
                '    .AppendLine("          AND T5.VCL_VIN_SEARCH = UPPER(:VCL_VIN_SEARCH) ")

                'End If
                '.AppendLine(" UNION ALL ")
                '.AppendLine("       SELECT Q6.REZID ")
                '.AppendLine("             ,Q6.REZSTARTTIME ")
                '.AppendLine("             ,Q6.REZENDTIME ")
                '.AppendLine("             ,Q6.ORDERNO ")
                '.AppendLine("             ,Q6.SERVICENAME ")
                '.AppendLine("             ,Q6.ROSTATUSCODE ")
                ''2013/12/26 TMEJ 陳　 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
                '.AppendLine("             ,Q6.DMS_JOB_DTL_ID ")
                ''2013/12/26 TMEJ 陳　 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END
                ''2015/09/07 TMEJ 春日井 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START
                '.AppendLine("             ,Q1.CST_VCL_TYPE ")
                ''2015/09/07 TMEJ 春日井 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END
                '.AppendLine("         FROM TB_M_CUSTOMER_VCL Q1 ")
                '.AppendLine("             ,TB_M_CUSTOMER_DLR Q2 ")
                '.AppendLine("             ,TB_M_CUSTOMER Q3 ")
                '.AppendLine("             ,TB_M_VEHICLE_DLR Q4 ")
                '.AppendLine("             ,TB_M_VEHICLE Q5 ")
                ''2013/12/26 TMEJ 陳　 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
                ''.AppendLine("             ,(SELECT W2.JOB_DTL_ID AS REZID ")
                '.AppendLine("             ,(SELECT W1.SVCIN_ID AS REZID ")
                '.AppendLine("                     ,W2.DMS_JOB_DTL_ID ")
                ''2013/12/26 TMEJ 陳　 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END
                '.AppendLine("                     ,W1.CST_ID ")
                '.AppendLine("                     ,W1.VCL_ID ")
                '.AppendLine("                     ,W3.SCHE_START_DATETIME_MIN AS REZSTARTTIME ")
                '.AppendLine("                     ,W3.SCHE_END_DATETIME_MAX AS REZENDTIME ")
                '.AppendLine("                     ,TRIM(W1.RO_NUM) AS ORDERNO ")
                '.AppendLine("                     ,W4.SVC_CLASS_NAME AS SERVICENAME ")
                ''2013/12/26 TMEJ 陳　 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
                ''.AppendLine("                     ,NVL2(TRIM(W1.RO_NUM), :RO_NUM_1, :RO_NUM_0) AS ROSTATUSCODE ")
                '.AppendLine("                     ,CASE WHEN EXISTS(SELECT 1 FROM TB_T_SERVICEIN A1, TB_T_RO_INFO A2 WHERE A1.SVCIN_ID = A2.SVCIN_ID AND A1.SVCIN_ID = W2.SVCIN_ID) THEN :RO_NUM_1 ELSE :RO_NUM_0 END AS ROSTATUSCODE ")
                ''2013/12/26 TMEJ 陳　 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END
                ''2015/09/07 TMEJ 春日井 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START
                '.AppendLine("                     ,W1.CST_VCL_TYPE ")
                ''2015/09/07 TMEJ 春日井 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END
                '.AppendLine("                 FROM TB_T_SERVICEIN W1 ")
                '.AppendLine("                     ,TB_T_JOB_DTL W2 ")
                '.AppendLine("                     ,(SELECT T1.SVCIN_ID ")
                '.AppendLine("                             ,MIN(T2.JOB_DTL_ID) AS JOB_DTL_ID_MIN ")
                '.AppendLine("                             ,MIN(DECODE(T1.SCHE_SVCIN_DATETIME, :MINDATE, T3.SCHE_START_DATETIME, T1.SCHE_SVCIN_DATETIME)) AS SCHE_START_DATETIME_MIN ")
                '.AppendLine("                             ,MAX(DECODE(T1.SCHE_DELI_DATETIME, :MINDATE, T3.SCHE_END_DATETIME, T1.SCHE_DELI_DATETIME)) AS SCHE_END_DATETIME_MAX ")
                '.AppendLine("                         FROM TB_T_SERVICEIN T1 ")
                '.AppendLine("                             ,TB_T_JOB_DTL T2 ")
                '.AppendLine("                             ,TB_T_STALL_USE T3 ")
                '.AppendLine("                        WHERE T1.SVCIN_ID = T2.SVCIN_ID ")
                '.AppendLine("                          AND T2.JOB_DTL_ID = T3.JOB_DTL_ID ")
                '.AppendLine("                          AND T1.DLR_CD = :DLR_CD ")
                '.AppendLine("                          AND T1.BRN_CD = :BRN_CD ")
                '.AppendLine("                          AND T2.DLR_CD = :DLR_CD ")
                '.AppendLine("                          AND T2.BRN_CD = :BRN_CD ")
                '.AppendLine("                          AND T2.CANCEL_FLG = :CANCEL_FLG_0 ")
                '.AppendLine("                          AND T3.DLR_CD = :DLR_CD ")
                '.AppendLine("                          AND T3.BRN_CD = :BRN_CD ")
                '.AppendLine("                          AND T3.RSLT_START_DATETIME = :MINDATE ")
                '.AppendLine("                        GROUP BY T1.SVCIN_ID) W3 ")
                '.AppendLine("                     ,TB_M_SERVICE_CLASS W4 ")
                '.AppendLine("                WHERE ")
                '.AppendLine("                      W1.SVCIN_ID = W2.SVCIN_ID ")
                '.AppendLine("                  AND W2.SVCIN_ID = W3.SVCIN_ID ")
                '.AppendLine("                  AND W2.JOB_DTL_ID = W3.JOB_DTL_ID_MIN ")
                '.AppendLine("                  AND W2.SVC_CLASS_ID = W4.SVC_CLASS_ID(+) ")
                '.AppendLine("                  AND W1.DLR_CD = :DLR_CD ")
                '.AppendLine("                  AND W1.BRN_CD = :BRN_CD ")
                '.AppendLine("                  AND NOT EXISTS (SELECT 1 ")
                '.AppendLine("                                    FROM TB_T_SERVICEIN F1 ")
                '.AppendLine("                                   WHERE F1.SVCIN_ID = W1.SVCIN_ID ")
                '.AppendLine("                                     AND F1.SVC_STATUS = :SVC_STATUS_02) ")
                '.AppendLine("                  AND W1.ACCEPTANCE_TYPE = :ACCEPTANCE_TYPE_0 ")
                '.AppendLine("                  AND W2.DLR_CD = :DLR_CD ")
                '.AppendLine("                  AND W2.BRN_CD = :BRN_CD ")
                '.AppendLine("                  AND W2.CANCEL_FLG = :CANCEL_FLG_0 ")
                '.AppendLine("                  AND W3.SCHE_START_DATETIME_MIN >= TRUNC(TO_DATE(:BASEDATE, 'YYYYMMDDHH24MI'))) Q6 ")
                '.AppendLine("        WHERE Q1.CST_ID = Q2.CST_ID ")
                '.AppendLine("          AND Q2.CST_ID = Q3.CST_ID ")
                '.AppendLine("          AND Q1.VCL_ID = Q4.VCL_ID ")
                '.AppendLine("          AND Q4.VCL_ID = Q5.VCL_ID ")
                '.AppendLine("          AND Q1.CST_ID = Q6.CST_ID ")
                '.AppendLine("          AND Q1.VCL_ID = Q6.VCL_ID ")
                ''2015/09/07 TMEJ 春日井 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START
                '.AppendLine("          AND Q1.CST_VCL_TYPE = Q6.CST_VCL_TYPE ")
                ''2015/09/07 TMEJ 春日井 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END
                '.AppendLine("          AND Q1.DLR_CD = :DLR_CD ")
                '.AppendLine("          AND Q2.DLR_CD = :DLR_CD ")
                '.AppendLine("          AND Q4.DLR_CD = :DLR_CD ")

                ''2014/03/13 TMEJ 小澤 BTS-292対応 START
                ''2015/09/07 TMEJ 春日井 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START
                ''.AppendLine("          AND Q1.CST_VCL_TYPE = :CST_VCL_TYPE_1 ")
                '.AppendLine("          AND Q1.CST_VCL_TYPE <> :CST_VCL_TYPE_4 ")
                '.AppendLine("          AND Q1.OWNER_CHG_FLG = :OWNER_CHG_FLG_0 ")
                '.AppendLine("          AND Q1.VCL_ID IN (        ")
                '.AppendLine("            SELECT")
                '.AppendLine("                   Q7.VCL_ID")
                '.AppendLine("              FROM TB_M_CUSTOMER Q8")
                '.AppendLine("                  ,TB_M_CUSTOMER_VCL Q7")
                '.AppendLine("             WHERE Q8.CST_ID = Q7.CST_ID")
                '.AppendLine("               AND Q8.DMS_CST_CD = :DMS_CST_CD")
                '.AppendLine("               AND Q7.DLR_CD = :DLR_CD")
                '.AppendLine("               AND Q7.OWNER_CHG_FLG = :OWNER_CHG_FLG_0")
                '.AppendLine("          )")
                ''2015/09/07 TMEJ 春日井 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END
                ''2014/03/13 TMEJ 小澤 BTS-292対応 END
                ''2015/09/07 TMEJ 春日井 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START
                ''.AppendLine("          AND Q3.DMS_CST_CD = :DMS_CST_CD ")
                ''2015/09/07 TMEJ 春日井 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END
                'If Not (String.IsNullOrEmpty(inVclRegNo)) AndAlso _
                '   Not (String.IsNullOrEmpty(inVin)) Then
                '    '「車両登録No.」と「VIN」が両方存在している場合
                '    .AppendLine("          AND (Q5.VCL_VIN_SEARCH = UPPER(:VCL_VIN_SEARCH) ")
                '    .AppendLine("           OR Q4.REG_NUM_SEARCH = UPPER(:REG_NUM_SEARCH)) ")

                'ElseIf Not (String.IsNullOrEmpty(inVclRegNo)) Then
                '    '「車両登録No.」が存在している場合
                '    .AppendLine("          AND Q4.REG_NUM_SEARCH = UPPER(:REG_NUM_SEARCH) ")

                'ElseIf Not (String.IsNullOrEmpty(inVin)) Then
                '    '「VIN」が存在している場合
                '    .AppendLine("          AND Q5.VCL_VIN_SEARCH = UPPER(:VCL_VIN_SEARCH) ")

                'End If
                '.AppendLine("       ) K1 ")
                ''2015/09/07 TMEJ 春日井 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START
                ''.AppendLine("ORDER BY K1.REZSTARTTIME ")
                '.AppendLine("ORDER BY K1.REZSTARTTIME, K1.CST_VCL_TYPE ")
                ''2015/09/07 TMEJ 春日井 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END

                ''2013/06/27 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END


                .AppendLine("SELECT /* IC3811501_001 */")
                .AppendLine("        Q6.REZID ")
                .AppendLine("       ,Q6.REZSTARTTIME ")
                .AppendLine("       ,Q6.REZENDTIME ")
                .AppendLine("       ,Q6.ORDERNO ")
                .AppendLine("       ,Q6.SERVICENAME ")
                .AppendLine("       ,Q6.ROSTATUSCODE ")
                .AppendLine("       ,Q6.DMS_JOB_DTL_ID ")
                .AppendLine("       ,Q3.CST_NAME ")
                .AppendLine("         FROM TB_M_CUSTOMER_VCL Q1 ")
                .AppendLine("       ,TB_M_CUSTOMER_DLR Q2 ")
                .AppendLine("       ,TB_M_CUSTOMER Q3 ")
                .AppendLine("       ,TB_M_VEHICLE_DLR Q4 ")
                .AppendLine("       ,TB_M_VEHICLE Q5 ")
                .AppendLine("       ,(SELECT W1.SVCIN_ID AS REZID ")
                .AppendLine("               ,W2.DMS_JOB_DTL_ID ")
                .AppendLine("               ,W1.CST_ID ")
                .AppendLine("               ,W1.VCL_ID ")
                .AppendLine("               ,W3.SCHE_START_DATETIME_MIN AS REZSTARTTIME ")
                .AppendLine("               ,W3.SCHE_END_DATETIME_MAX AS REZENDTIME ")
                .AppendLine("               ,TRIM(W1.RO_NUM) AS ORDERNO ")
                .AppendLine("               ,W4.SVC_CLASS_NAME AS SERVICENAME ")
                .AppendLine("               ,CASE WHEN EXISTS(SELECT 1 FROM TB_T_SERVICEIN A1, TB_T_RO_INFO A2 WHERE A1.SVCIN_ID = A2.SVCIN_ID AND A1.SVCIN_ID = W2.SVCIN_ID) THEN :RO_NUM_1 ELSE :RO_NUM_0 END AS ROSTATUSCODE ")
                .AppendLine("               ,W1.CST_VCL_TYPE ")
                .AppendLine("           FROM TB_T_SERVICEIN W1 ")
                .AppendLine("               ,TB_T_JOB_DTL W2 ")
                .AppendLine("               ,(SELECT T1.SVCIN_ID ")
                .AppendLine("                       ,MIN(T2.JOB_DTL_ID) AS JOB_DTL_ID_MIN ")
                .AppendLine("                       ,MIN(DECODE(T1.SCHE_SVCIN_DATETIME, :MINDATE, T3.SCHE_START_DATETIME, T1.SCHE_SVCIN_DATETIME)) AS SCHE_START_DATETIME_MIN ")
                .AppendLine("                       ,MAX(DECODE(T1.SCHE_DELI_DATETIME, :MINDATE, T3.SCHE_END_DATETIME, T1.SCHE_DELI_DATETIME)) AS SCHE_END_DATETIME_MAX ")
                .AppendLine("                   FROM TB_T_SERVICEIN T1 ")
                .AppendLine("                       ,TB_T_JOB_DTL T2 ")
                .AppendLine("                       ,TB_T_STALL_USE T3 ")
                .AppendLine("                  WHERE T1.SVCIN_ID = T2.SVCIN_ID ")
                .AppendLine("                    AND T2.JOB_DTL_ID = T3.JOB_DTL_ID ")
                .AppendLine("                    AND T1.DLR_CD = :DLR_CD ")
                .AppendLine("                    AND T1.BRN_CD = :BRN_CD ")
                .AppendLine("                    AND T2.DLR_CD = :DLR_CD ")
                .AppendLine("                    AND T2.BRN_CD = :BRN_CD ")
                .AppendLine("                    AND T2.CANCEL_FLG = :CANCEL_FLG_0 ")
                .AppendLine("                    AND T3.DLR_CD = :DLR_CD ")
                .AppendLine("                    AND T3.BRN_CD = :BRN_CD ")
                .AppendLine("                    AND T3.RSLT_START_DATETIME = :MINDATE ")
                .AppendLine("                  GROUP BY T1.SVCIN_ID) W3 ")
                .AppendLine("               ,TB_M_SERVICE_CLASS W4 ")
                .AppendLine("          WHERE ")
                .AppendLine("                W1.SVCIN_ID = W2.SVCIN_ID ")
                .AppendLine("            AND W2.SVCIN_ID = W3.SVCIN_ID ")
                .AppendLine("            AND W2.JOB_DTL_ID = W3.JOB_DTL_ID_MIN ")
                .AppendLine("            AND W2.SVC_CLASS_ID = W4.SVC_CLASS_ID(+) ")
                .AppendLine("            AND W1.DLR_CD = :DLR_CD ")
                .AppendLine("            AND W1.BRN_CD = :BRN_CD ")
                .AppendLine("            AND NOT EXISTS (SELECT 1 ")
                .AppendLine("                              FROM TB_T_SERVICEIN F1 ")
                .AppendLine("                             WHERE F1.SVCIN_ID = W1.SVCIN_ID ")
                .AppendLine("                               AND F1.SVC_STATUS = :SVC_STATUS_02) ")
                .AppendLine("            AND W1.ACCEPTANCE_TYPE = :ACCEPTANCE_TYPE_0 ")
                .AppendLine("            AND W2.DLR_CD = :DLR_CD ")
                .AppendLine("            AND W2.BRN_CD = :BRN_CD ")
                .AppendLine("            AND W2.CANCEL_FLG = :CANCEL_FLG_0 ")
                .AppendLine("            AND W3.SCHE_START_DATETIME_MIN >= TRUNC(TO_DATE(:BASEDATE, 'YYYYMMDDHH24MI'))) Q6 ")
                .AppendLine("        WHERE Q1.CST_ID = Q2.CST_ID ")
                .AppendLine("          AND Q2.CST_ID = Q3.CST_ID ")
                .AppendLine("          AND Q1.VCL_ID = Q4.VCL_ID ")
                .AppendLine("          AND Q4.VCL_ID = Q5.VCL_ID ")
                .AppendLine("          AND Q1.CST_ID = Q6.CST_ID ")
                .AppendLine("          AND Q1.VCL_ID = Q6.VCL_ID ")
                .AppendLine("          AND Q1.CST_VCL_TYPE = Q6.CST_VCL_TYPE ")
                .AppendLine("          AND Q1.DLR_CD = :DLR_CD ")
                .AppendLine("          AND Q2.DLR_CD = :DLR_CD ")
                .AppendLine("          AND Q4.DLR_CD = :DLR_CD ")
                .AppendLine("          AND Q1.CST_VCL_TYPE <> :CST_VCL_TYPE_4 ")
                .AppendLine("          AND Q1.OWNER_CHG_FLG = :OWNER_CHG_FLG_0 ")
                If isGetDmsCstFlg Then
                    .AppendLine("          AND Q1.VCL_ID IN (        ")
                    .AppendLine("            SELECT")
                    .AppendLine("             Q7.VCL_ID")
                    .AppendLine("        FROM TB_M_CUSTOMER Q8")
                    .AppendLine("            ,TB_M_CUSTOMER_VCL Q7")
                    .AppendLine("       WHERE Q8.CST_ID = Q7.CST_ID")
                    .AppendLine("         AND Q8.DMS_CST_CD = :DMS_CST_CD")
                    .AppendLine("         AND Q7.DLR_CD = :DLR_CD")
                    .AppendLine("         AND Q7.OWNER_CHG_FLG = :OWNER_CHG_FLG_0")
                    .AppendLine("          )")
                Else
                    .AppendLine("          AND Q3.DMS_CST_CD = :DMS_CST_CD")
                End If

                If Not (String.IsNullOrEmpty(inVclRegNo)) AndAlso _
                   Not (String.IsNullOrEmpty(inVin)) Then
                    '「車両登録No.」と「VIN」が両方存在している場合
                    .AppendLine("          AND (Q5.VCL_VIN_SEARCH = UPPER(:VCL_VIN_SEARCH) ")
                    .AppendLine("           OR Q4.REG_NUM_SEARCH = UPPER(:REG_NUM_SEARCH)) ")

                ElseIf Not (String.IsNullOrEmpty(inVclRegNo)) Then
                    '「車両登録No.」が存在している場合
                    .AppendLine("          AND Q4.REG_NUM_SEARCH = UPPER(:REG_NUM_SEARCH) ")

                ElseIf Not (String.IsNullOrEmpty(inVin)) Then
                    '「VIN」が存在している場合
                    .AppendLine("          AND Q5.VCL_VIN_SEARCH = UPPER(:VCL_VIN_SEARCH) ")

                End If
                .AppendLine("ORDER BY Q6.REZSTARTTIME, Q1.CST_VCL_TYPE ")
                '2017/01/06 NSK  竹中 TR-SVT-TMT-20160602-001 顧客がWelcomeボードに表示されない END
            End With

            Using query As New DBSelectQuery(Of IC3811501DataSet.IC3811501ReservationListDataTable)("IC3811501_001")
                query.CommandText = sql.ToString()

                '2013/06/27 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
                'query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, inDealerCode)
                'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, inStoreCode)
                'query.AddParameterWithTypeValue("CUSTCD", OracleDbType.Varchar2, inCustomerCode)
                'query.AddParameterWithTypeValue("BASEDATE", OracleDbType.Varchar2, inBaseDate)
                'If (Not (String.IsNullOrEmpty(inVclRegNo)) AndAlso 0 < inVclRegNo.Length) AndAlso _
                '   (Not (String.IsNullOrEmpty(inVin)) AndAlso 0 < inVin.Length) Then
                '    '「車両登録No.」と「VIN」が両方存在している場合
                '    query.AddParameterWithTypeValue("VCLREGNO", OracleDbType.Varchar2, inVclRegNo)
                '    query.AddParameterWithTypeValue("VIN", OracleDbType.Varchar2, inVin)

                'ElseIf Not (String.IsNullOrEmpty(inVclRegNo)) AndAlso 0 < inVclRegNo.Length Then
                '    '「車両登録No.」が存在している場合
                '    query.AddParameterWithTypeValue("VCLREGNO", OracleDbType.Varchar2, inVclRegNo)

                'ElseIf Not (String.IsNullOrEmpty(inVin)) AndAlso 0 < inVin.Length Then
                '    '「VIN」が存在している場合
                '    query.AddParameterWithTypeValue("VIN", OracleDbType.Varchar2, inVin)
                'End If
                query.AddParameterWithTypeValue("RO_NUM_1", OracleDbType.NVarchar2, OrderNoTypeOn)
                query.AddParameterWithTypeValue("RO_NUM_0", OracleDbType.NVarchar2, OrderNoTypeOff)
                query.AddParameterWithTypeValue("MINDATE", OracleDbType.Date, Date.Parse(DateMinValue, Nothing))
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, inDealerCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, inStoreCode)
                query.AddParameterWithTypeValue("CANCEL_FLG_0", OracleDbType.NVarchar2, CancelTypeEffective)
                query.AddParameterWithTypeValue("SVC_STATUS_02", OracleDbType.NVarchar2, ServiceStatusCancel)
                query.AddParameterWithTypeValue("ACCEPTANCE_TYPE_0", OracleDbType.NVarchar2, AcceptanceTypeReserve)
                query.AddParameterWithTypeValue("BASEDATE", OracleDbType.NVarchar2, inBaseDate)
                '2017/01/06 NSK  竹中 TR-SVT-TMT-20160602-001 顧客がWelcomeボードに表示されない START
                If (isGetDmsCstFlg) Then
                    query.AddParameterWithTypeValue("DMS_CST_CD", OracleDbType.NVarchar2, inCustomerCode)
                Else
                    query.AddParameterWithTypeValue("DMS_CST_CD", OracleDbType.NVarchar2, DmsCstCdDefault)
                End If
                '2017/01/06 NSK  竹中 TR-SVT-TMT-20160602-001 顧客がWelcomeボードに表示されない END
                If Not (String.IsNullOrEmpty(inVclRegNo)) AndAlso _
                   Not (String.IsNullOrEmpty(inVin)) Then
                    '「車両登録No.」と「VIN」が両方存在している場合
                    query.AddParameterWithTypeValue("VCL_VIN_SEARCH", OracleDbType.NVarchar2, inVin)
                    query.AddParameterWithTypeValue("REG_NUM_SEARCH", OracleDbType.NVarchar2, inVclRegNo)

                ElseIf Not (String.IsNullOrEmpty(inVclRegNo)) Then
                    '「車両登録No.」が存在している場合
                    query.AddParameterWithTypeValue("REG_NUM_SEARCH", OracleDbType.NVarchar2, inVclRegNo)

                ElseIf Not (String.IsNullOrEmpty(inVin)) Then
                    '「VIN」が存在している場合
                    query.AddParameterWithTypeValue("VCL_VIN_SEARCH", OracleDbType.NVarchar2, inVin)
                End If
                '2013/06/27 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

                '2014/03/13 TMEJ 小澤 BTS-292対応 START
                '2015/09/07 TMEJ 春日井 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START
                'query.AddParameterWithTypeValue("CST_VCL_TYPE_1", OracleDbType.NVarchar2, CustomerVehicleTypeOwner)
                query.AddParameterWithTypeValue("CST_VCL_TYPE_4", OracleDbType.NVarchar2, CustomerVehicleTypeInsurance)
                query.AddParameterWithTypeValue("OWNER_CHG_FLG_0", OracleDbType.NVarchar2, OwnerChangeFlagUnset)
                '2015/09/07 TMEJ 春日井 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END
                '2014/03/13 TMEJ 小澤 BTS-292対応 END

                Dim dtReservationList As IC3811501DataSet.IC3811501ReservationListDataTable = query.GetData()

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} QUERY:COUNT = {2}" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                            , dtReservationList.Count))
                Return dtReservationList
            End Using
        End Function

#End Region

    End Class
End Namespace

Partial Class IC3811501DataSet
End Class
