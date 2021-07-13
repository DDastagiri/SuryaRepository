'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3080202TableAdapter.vb
'─────────────────────────────────────
'機能： 顧客詳細(商談情報)
'補足： 
'作成： 2011/11/24 TCS 小野
'更新： 2012/01/26 TCS 山口 【SALES_1B】
'更新： 2012/11/22 TCS 坪根 【A.STEP2】次世代e-CRB  新車タブレット横展開に向けた機能開発
'更新： 2013/06/30 TCS 趙   【A STEP2】i-CROP新DB適応に向けた機能開発（既存流用）
'更新： 2013/12/05 TCS 市川 Aカード情報相互連携開発
'更新： 2014/02/12 TCS 山口 受注後フォロー機能開発
'更新： 2014/02/02 TCS 松月 【A STEP2】希望車表示不具合対応（号口切替BTS-39） 
'更新： 2014/03/11 TCS 松月 【A STEP2】活動予定スタッフ取得不具合対応（問連TR-V4-GTMC140305008）
'更新： 2014/04/01 TCS 松月 【A STEP2】TMT不具合対応
'更新： 2014/05/26 TCS 松月 【A STEP2】活動回数不具合対応（問連TR-V4-GTMC140512003）
'更新： 2014/07/09 TCS 高橋 受注後活動完了条件変更対応
'更新： 2014/07/24 TCS 外崎 不具合対応（TMT 切替BTS-102）
'更新： 2014/09/04 TCS 武田 UAT不具合対応(最終活動表示)
'更新： 2014/09/16 TCS 松月 【A STEP2】SQL性能問題対応（問連TR-V4-GTMC140909002）
'更新： 2015/01/08 TCS 外崎 TMT2販社 ST-BTS 57
'更新： 2015/12/08 TCS 中村 (ﾄﾗｲ店ｼｽﾃﾑ評価)新車ﾀﾌﾞﾚｯﾄ(受注後)の管理機能開発
'更新： 2016/09/12 TCS 鈴木 性能改善（TR-SLT-TMT-20160726-002）
'更新： 2017/11/20 TCS 河原 TKM独自機能開発
'更新： 2018/04/17 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証
'更新： 2018/06/01 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1
'更新： 2018/08/07 TCS 中村 （FS）基幹連携を用いたセールス業務実績入力の検証
'更新： 2019/02/14 TCS 河原 TKM UAT0651対応(タブレットで契約した見積のIDは論削済みでも取得するように修正)
'更新： 2019/11/26 TS  髙橋(龍) SQL性能改善(TR-SLT-TMT-20190503-001)
'更新： 2019/12/06 TS  三浦 （FS）e-CRBシステム各種作業効率化の品質検証
'更新： 2020/01/06 TS  重松 [TMTレスポンススロー] SLT基盤への横展
'更新： 2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)
'─────────────────────────────────────

Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core
'2013/06/30 TCS 趙 2013/10対応版 START
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic

' 2013/06/30 TCS 趙 2013/10対応版　既存流用 END 
'2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 START
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
'2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 END

Public NotInheritable Class SC3080202TableAdapter
    '2019/12/06 TS  三浦 （FS）e-CRBシステム各種作業効率化の品質検証 START
    Private Const ACT_STATUS_DISP_FLG_ON As String = "1"
    '2019/12/06 TS  三浦 （FS）e-CRBシステム各種作業効率化の品質検証 END

    '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 START
    ''' <summary>
    ''' サフィックス使用可否フラグ名称
    ''' </summary>
    ''' <remarks></remarks>
    Private Const USE_FLG_SUFFIX As String = "USE_FLG_SUFFIX"
    ''' <summary>
    ''' 内装色使用可否フラグ名称
    ''' </summary>
    ''' <remarks></remarks>
    Private Const USE_FLG_INTERIORCLR As String = "USE_FLG_INTERIORCLR"
    '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 END

    '2014/02/12 TCS 山口 受注後フォロー機能開発 START
    ''' <summary>
    ''' 取得日付区分
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum AfterOdrDate
        ScheStartDateOrTime = 0
        ScheEndDateOrTime = 1
        RsltStartDateOrTime = 2
        RsltEndDateOrTime = 3
    End Enum
    '2014/02/12 TCS 山口 受注後フォロー機能開発 END

    ''' <summary>
    ''' デフォルトコンストラクタ
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub New()
        '処理なし
    End Sub

    '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 見積ID取得
    ''' </summary>
    ''' <param name="fllwupboxSeqno">Follow-up Box内連番</param>
    ''' <returns>データセット(見積管理ID一式)</returns>
    ''' <remarks></remarks>
    Public Shared Function GetEstimateInfo(ByVal fllwupboxSeqno As String) As SC3080202DataSet.SC3080202GetEstimateidToDataTable
        Dim sql As New StringBuilder

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetEstimateInfo_Start")
        'ログ出力 End *****************************************************************************

        With sql
            .Append("SELECT ")
            .Append("  /* SC3080202_030 */ ")
            .Append("  ESTIMATEID, ")
            .Append("  CONTRACTFLG, ")
            .Append("  SUCCESSFLG ")
            .Append("FROM ")
            .Append("  TBL_ESTIMATEINFO ")
            .Append("WHERE ")
            .Append("      FLLWUPBOX_SEQNO = :FLLWUPBOXSEQNO ")
            .Append("  AND DELFLG = '0' ")
            .Append("ORDER BY ")
            .Append("  ESTIMATEID ")
        End With

        Using query As New DBSelectQuery(Of SC3080202DataSet.SC3080202GetEstimateidToDataTable)("SC3080202_030")
            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("FLLWUPBOXSEQNO", OracleDbType.Decimal, fllwupboxSeqno)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetEstimateInfo_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
            Return query.GetData()
        End Using
    End Function

    '2019/02/14 TCS 河原 TKM UAT0651対応(タブレットで契約した見積のIDは論削済みでも取得するように修正) START
    ''' <summary>
    ''' 見積ID取得
    ''' </summary>
    ''' <param name="fllwupboxSeqno">Follow-up Box内連番</param>
    ''' <returns>データセット(見積管理ID一式)</returns>
    ''' <remarks></remarks>
    Public Shared Function GetEstimateId(ByVal fllwupboxSeqno As String) As SC3080202DataSet.SC3080202GetEstimateidToDataTable
        Dim sql As New StringBuilder

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetEstimateId_Start")
        'ログ出力 End *****************************************************************************

        With sql
            .Append("SELECT ")
            .Append("  /* SC3080202_032 */ ")
            .Append("  ESTIMATEID, ")
            .Append("  CONTRACTFLG, ")
            .Append("  SUCCESSFLG ")
            .Append("FROM ")
            .Append("  TBL_ESTIMATEINFO ")
            .Append("WHERE ")
            .Append("      FLLWUPBOX_SEQNO = :FLLWUPBOXSEQNO ")
            .Append("  AND (DELFLG = '0' ")
            .Append("   OR CONTRACT_APPROVAL_STATUS = '2') ")
            .Append("ORDER BY ")
            .Append("  ESTIMATEID ")
        End With

        Using query As New DBSelectQuery(Of SC3080202DataSet.SC3080202GetEstimateidToDataTable)("SC3080202_032")
            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("FLLWUPBOXSEQNO", OracleDbType.Decimal, fllwupboxSeqno)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetEstimateId_End")
            'ログ出力 End *****************************************************************************

            Return query.GetData()
        End Using

    End Function
    '2019/02/14 TCS 河原 TKM UAT0651対応(タブレットで契約した見積のIDは論削済みでも取得するように修正) END

    '2014/02/12 TCS 山口 受注後フォロー機能開発 START
    '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 活動状態取得
    ''' </summary>
    ''' <param name="fllwupboxSeqno">Follow-up Box内連番</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetFollowupboxStatus(ByVal fllwupboxSeqno As String) As SC3080202DataSet.SC3080202GetStatusToDataTable
        Dim sql As New StringBuilder

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetFollowupboxStatus_Start")
        'ログ出力 End *****************************************************************************

        With sql
            .Append(" SELECT ")
            .Append("   /* SC3080202_129 */ ")
            .Append("   T1.CRACTRESULT , ")
            .Append("   T1.REQCATEGORY , ")
            '2014/07/09 TCS 高橋 受注後活動完了条件変更対応 START
            .Append("   CASE WHEN T1.CRACTRESULT = '3' ")
            .Append("             AND NVL(T2.CONTRACTNO, NULL) IS NOT NULL ")
            .Append("             AND NVL(T3.CANCEL_FLG, '0') = '0' ")
            .Append("          THEN ")
            '                    受注後活動中
            .Append("            CASE WHEN T12.SALES_ID IS NOT NULL AND T11.CNT IS NOT NULL AND T11.CNT > 0 ")
            .Append("                   THEN TO_CHAR(T12.AFTER_ODR_PIC_STF_CD) ")
            '                    受注後活動完了済
            .Append("                 WHEN T13.SALES_ID IS NOT NULL ")
            .Append("                   THEN TO_CHAR(T13.AFTER_ODR_PIC_STF_CD) ")
            '                    受注後活動なし
            .Append("                 ELSE T1.ACCOUNT_PLAN ")
            .Append("            END ")
            '2014/07/09 TCS 高橋 受注後活動完了条件変更対応 END
            .Append("        ELSE ")
            .Append("             T1.ACCOUNT_PLAN  END ")
            .Append("   AS ACCOUNT_PLAN , ")
            .Append("   T2.CONTRACTNO CONTRACTNO , ")
            '2014/07/09 TCS 高橋 受注後活動完了条件変更対応 START
            .Append("   CASE WHEN T13.SALES_ID IS NOT NULL THEN ")
            .Append("      '1' ")
            '2014/07/09 TCS 高橋 受注後活動完了条件変更対応 END
            .Append("      ELSE ")
            .Append("      '0' ")
            .Append("      END AS BOOCKEDAFTER_COMPLETEFLG, ")
            .Append("   T3.CANCEL_FLG AS CANCELFLG ")
            .Append(" FROM ")
            .Append("   ( ")
            .Append("   SELECT ")
            .Append("     T4.SALES_ID AS FLLWUPBOX_SEQNO , ")
            .Append("     CASE WHEN T5.REQ_STATUS = '31' THEN '3' ")
            .Append("          WHEN T5.REQ_STATUS = '32' THEN '5' ")
            .Append("          ELSE ")
            .Append("            CASE WHEN T4.SALES_PROSPECT_CD = '30' THEN '1' ")
            .Append("                 WHEN T4.SALES_PROSPECT_CD = '20' THEN '2' ")
            .Append("                 WHEN T4.SALES_PROSPECT_CD = '10' THEN '4' ")
            .Append("                 ELSE '4' END ")
            .Append("     END AS CRACTRESULT , ")
            .Append("     TO_CHAR(NVL(T6.RSLT_CONTACT_MTD,0)) AS REQCATEGORY , ")
            '2014/07/09 TCS 高橋 受注後活動完了条件変更対応 START
            .Append("     T6.ACT_COUNT AS ACT_COUNT , ")
            '2014/07/09 TCS 高橋 受注後活動完了条件変更対応 END
            .Append("     TO_CHAR(T6.SCHE_STF_CD) AS ACCOUNT_PLAN ")
            .Append("   FROM ")
            .Append("     TB_T_SALES T4 , ")
            .Append("     TB_T_REQUEST T5 , ")
            .Append("     TB_T_ACTIVITY T6 ")
            .Append("   WHERE ")
            .Append("         T4.REQ_ID = T6.REQ_ID ")
            .Append("     AND T5.REQ_ID = T4.REQ_ID ")
            .Append("     AND T4.SALES_ID = :FLLWUPBOXSEQNO ")
            .Append("   UNION ALL ")
            .Append("   SELECT ")
            .Append("     T4.SALES_ID AS FLLWUPBOX_SEQNO , ")
            .Append("     CASE WHEN T5.CONTINUE_ACT_STATUS = '31' THEN '3' ")
            .Append("          WHEN T5.CONTINUE_ACT_STATUS = '32' THEN '5' ")
            .Append("          ELSE ")
            .Append("            CASE WHEN T4.SALES_PROSPECT_CD = '30' THEN '1' ")
            .Append("                 WHEN T4.SALES_PROSPECT_CD = '20' THEN '2' ")
            .Append("                 WHEN T4.SALES_PROSPECT_CD = '10' THEN '4' ")
            .Append("                 ELSE '4' END ")
            .Append("     END AS CRACTRESULT , ")
            .Append("     TO_CHAR(NVL(T6.RSLT_CONTACT_MTD,0)) AS REQCATEGORY , ")
            '2014/07/09 TCS 高橋 受注後活動完了条件変更対応 START
            .Append("     T6.ACT_COUNT AS ACT_COUNT , ")
            '2014/07/09 TCS 高橋 受注後活動完了条件変更対応 END
            .Append("     TO_CHAR(T6.SCHE_STF_CD) AS ACCOUNT_PLAN ")
            .Append("   FROM ")
            .Append("     TB_T_SALES T4 , ")
            .Append("     TB_T_ATTRACT T5 , ")
            .Append("     TB_T_ACTIVITY T6 ")
            .Append("   WHERE ")
            .Append("         T4.ATT_ID = T6.ATT_ID ")
            .Append("     AND T5.ATT_ID = T4.ATT_ID ")
            .Append("     AND T4.SALES_ID = :FLLWUPBOXSEQNO ")
            .Append("   UNION ALL ")
            .Append("   SELECT ")
            .Append("     T4.SALES_ID AS FLLWUPBOX_SEQNO , ")
            .Append("     CASE WHEN T5.REQ_STATUS = '31' THEN '3' ")
            .Append("          WHEN T5.REQ_STATUS = '32' THEN '5' ")
            .Append("          ELSE ")
            .Append("            CASE WHEN T4.SALES_PROSPECT_CD = '30' THEN '1' ")
            .Append("                 WHEN T4.SALES_PROSPECT_CD = '20' THEN '2' ")
            .Append("                 WHEN T4.SALES_PROSPECT_CD = '10' THEN '4' ")
            .Append("                 ELSE '4' END ")
            .Append("     END AS CRACTRESULT , ")
            .Append("     TO_CHAR(NVL(T6.RSLT_CONTACT_MTD,0)) AS REQCATEGORY , ")
            '2014/07/09 TCS 高橋 受注後活動完了条件変更対応 START
            .Append("     T6.ACT_COUNT AS ACT_COUNT , ")
            '2014/07/09 TCS 高橋 受注後活動完了条件変更対応 END
            .Append("     TO_CHAR(T6.SCHE_STF_CD) AS ACCOUNT_PLAN ")
            .Append("   FROM ")
            .Append("     TB_H_SALES T4 , ")
            .Append("     TB_H_REQUEST T5 , ")
            .Append("     TB_H_ACTIVITY T6 ")
            .Append("   WHERE ")
            .Append("         T4.REQ_ID = T6.REQ_ID ")
            .Append("     AND T5.REQ_ID = T4.REQ_ID ")
            .Append("     AND T4.SALES_ID = :FLLWUPBOXSEQNO ")
            .Append("   UNION ALL ")
            .Append("   SELECT ")
            .Append("     T4.SALES_ID AS FLLWUPBOX_SEQNO , ")
            .Append("     CASE WHEN T5.CONTINUE_ACT_STATUS = '31' THEN '3' ")
            .Append("          WHEN T5.CONTINUE_ACT_STATUS = '32' THEN '5' ")
            .Append("          ELSE ")
            .Append("            CASE WHEN T4.SALES_PROSPECT_CD = '30' THEN '1' ")
            .Append("                 WHEN T4.SALES_PROSPECT_CD = '20' THEN '2' ")
            .Append("                 WHEN T4.SALES_PROSPECT_CD = '10' THEN '4' ")
            .Append("                 ELSE '4' END ")
            .Append("     END AS CRACTRESULT , ")
            .Append("     TO_CHAR(NVL(T6.RSLT_CONTACT_MTD,0)) AS REQCATEGORY , ")
            '2014/07/09 TCS 高橋 受注後活動完了条件変更対応 START
            .Append("     T6.ACT_COUNT AS ACT_COUNT , ")
            '2014/07/09 TCS 高橋 受注後活動完了条件変更対応 END
            .Append("     TO_CHAR(T6.SCHE_STF_CD) AS ACCOUNT_PLAN ")
            .Append("   FROM ")
            .Append("     TB_H_SALES T4 , ")
            .Append("     TB_H_ATTRACT T5 , ")
            .Append("     TB_H_ACTIVITY T6 ")
            .Append("   WHERE ")
            .Append("         T4.ATT_ID = T6.ATT_ID ")
            .Append("     AND T5.ATT_ID = T4.ATT_ID ")
            .Append("     AND T4.SALES_ID = :FLLWUPBOXSEQNO ")
            .Append("   UNION ALL ")
            .Append("   SELECT T12.FLLWUPBOX_SEQNO AS FLLWUPBOX_SEQNO ")
            .Append("        , ' ' AS CRACTRESULT ")
            .Append("        , '0' AS REQCATEGORY ")
            '2014/07/09 TCS 高橋 受注後活動完了条件変更対応 START
            .Append("        , 1 AS ACT_COUNT ")
            '2014/07/09 TCS 高橋 受注後活動完了条件変更対応 END
            .Append("        , TO_CHAR(T12.ACCOUNT_PLAN) AS ACCOUNT_PLAN ")
            .Append("     FROM TBL_FLLWUPBOX_SALES T12 ")
            .Append("    WHERE T12.FLLWUPBOX_SEQNO = :FLLWUPBOXSEQNO ")
            .Append("      AND T12.NEWFLLWUPBOXFLG = '1' ")
            .Append("   ) T1 , ")
            .Append("   TBL_ESTIMATEINFO T2 , ")
            .Append("   TB_T_SALESBOOKING T3, ")
            .Append("   ( ")
            .Append("   SELECT ")
            .Append("       T10.SALES_ID, ")
            .Append("       COUNT(1) AS CNT ")
            .Append("   FROM ")
            .Append("       TB_M_AFTER_ODR_PROC T7, ")
            .Append("       TB_M_AFTER_ODR_ACT T8, ")
            .Append("       TB_T_AFTER_ODR_ACT T9, ")
            .Append("       TB_T_AFTER_ODR T10 ")
            .Append("   WHERE ")
            .Append("           T7.AFTER_ODR_PRCS_CD = T8.AFTER_ODR_PRCS_CD ")
            .Append("       AND T8.MANDATORY_ACT_FLG = '1' ")
            .Append("       AND T8.AFTER_ODR_ACT_CD = T9.AFTER_ODR_ACT_CD(+) ")
            .Append("       AND T9.AFTER_ODR_ACT_STATUS <> '1' ")
            .Append("       AND T9.AFTER_ODR_ID = T10.AFTER_ODR_ID ")
            .Append("       AND T10.SALES_ID = :FLLWUPBOXSEQNO ")
            .Append("   GROUP BY T10.SALES_ID ")
            .Append("   ) T11, ")
            .Append("   TB_T_AFTER_ODR T12 ")
            '2014/07/09 TCS 高橋 受注後活動完了条件変更対応 START
            .Append(" , TB_H_AFTER_ODR T13 ")
            '2014/07/09 TCS 高橋 受注後活動完了条件変更対応 END
            .Append(" WHERE ")
            .Append("       T1.FLLWUPBOX_SEQNO = T2.FLLWUPBOX_SEQNO(+) ")
            .Append("   AND T2.CONTRACTFLG(+) = '1' ")
            .Append("   AND T2.DELFLG(+) = '0' ")
            .Append("   AND TRIM(T2.DLRCD) = T3.DLR_CD(+) ")
            .Append("   AND TRIM(T2.CONTRACTNO) = T3.SALESBKG_NUM(+) ")
            .Append("   AND T11.SALES_ID(+) = T1.FLLWUPBOX_SEQNO ")
            .Append("   AND T12.SALES_ID(+) = T1.FLLWUPBOX_SEQNO ")
            '2014/07/09 TCS 高橋 受注後活動完了条件変更対応 START
            .Append("   AND T13.SALES_ID(+) = T1.FLLWUPBOX_SEQNO ")
            .Append(" ORDER BY T1.FLLWUPBOX_SEQNO, T1.ACT_COUNT ")
            '2014/07/09 TCS 高橋 受注後活動完了条件変更対応 END

        End With

        Using query As New DBSelectQuery(Of SC3080202DataSet.SC3080202GetStatusToDataTable)("SC3080202_129")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("FLLWUPBOXSEQNO", OracleDbType.Decimal, fllwupboxSeqno)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetFollowupboxStatus_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 徐 2013/10対応版　既存流用 END

            Return query.GetData()
        End Using

    End Function
    '2014/02/12 TCS 山口 受注後フォロー機能開発 END

    '2015/01/08 TCS 外崎 TMT2販社 ST-BTS 57 START
    'Shared Function GetFollowupboxList(ByVal dlrcd As String,
    '                                   ByVal strcd As String,
    '                                   ByVal insdid As String) As SC3080202DataSet.SC3080202GetFollowupboxListDataTable
    '2015/01/08 TCS 外崎 TMT2販社 ST-BTS 57 END

    '2014/02/12 TCS 山口 受注後フォロー機能開発 START
    '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 活動リスト取得
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="insdid">顧客ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetFollowupboxList(ByVal dlrcd As String,
                                       ByVal insdid As String) As SC3080202DataSet.SC3080202GetFollowupboxListDataTable
        Dim sql As New StringBuilder

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetFollowupboxList_Start")
        'ログ出力 End *****************************************************************************

        With sql
            .Append(" SELECT ")
            .Append("   /* SC3080202_101 */ ")
            .Append("   T1.DLRCD , ")
            .Append("   T1.STRCD , ")
            .Append("   T1.FLLWUPBOX_SEQNO , ")
            .Append("   T1.CRACTCATEGORY , ")
            .Append("   T1.PROMOTION_ID , ")
            .Append("   T1.REQCATEGORY , ")
            .Append("   T1.CRACTRESULT , ")
            .Append("   T1.SERVICENAME , ")
            .Append("   T1.SUBCTGORGNAME , ")
            .Append("   T1.PROMOTIONNAME , ")
            .Append("   T1.CRACTRESULT_UPDATEDATE , ")
            .Append("   CASE WHEN T1.CRACTRESULT = '3' ")
            .Append("         AND NVL(T2.CONTRACTNO, NULL) IS NOT NULL ")
            .Append("         AND NVL(T3.CANCEL_FLG, '0') = '0' ")
            .Append("        THEN ")
            '2014/07/09 TCS 高橋 受注後活動完了条件変更対応 START
            .Append("             TO_CHAR(NVL(T4.AFTER_ODR_PIC_STF_CD, T1.ACCOUNT_PLAN)) ") '--契約済でも受注後の予定が作成されていない場合は受注前の担当
            '2014/07/09 TCS 高橋 受注後活動完了条件変更対応 END
            .Append("        ELSE ")
            .Append("             T1.ACCOUNT_PLAN  END  ")
            .Append("   AS ACCOUNT_PLAN , ")
            .Append("   T1.CONTRACTNO  ")
            .Append(" FROM ")
            .Append(GetFollowupboxListSql1())
            .Append(" UNION ALL ")
            .Append(GetFollowupboxListSql2())
            .Append("  UNION ALL ")
            .Append("      ( ")
            '2019/11/26 TS 髙橋(龍) SQL性能改善(TR-SLT-TMT-20190503-001) START
            .Append("      SELECT /*+ INDEX(B IDX_ESTIMATEINFO_02)*/")
            '2019/11/26 TS 髙橋(龍) SQL性能改善(TR-SLT-TMT-20190503-001) END
            .Append("          A.DLRCD DLRCD ")
            .Append("          ,A.STRCD STRCD ")
            .Append("          ,A.FLLWUPBOX_SEQNO FLLWUPBOX_SEQNO ")
            .Append("          ,NULL CRACTCATEGORY ")
            .Append("          ,NULL PROMOTION_ID ")
            .Append("          ,NULL REQCATEGORY ")
            .Append("          ,NULL CRACTSTATUS ")
            .Append("          ,NULL CRACTRESULT ")
            .Append("          ,NULL SERVICENAME ")
            .Append("          ,NULL SUBCTGORGNAME ")
            .Append("          ,NULL PROMOTIONNAME ")
            .Append("          ,NULL CRACTRESULT_UPDATEDATE ")
            .Append("          ,A.ACCOUNT_PLAN ")
            .Append("          ,B.CONTRACTNO ")
            .Append("      FROM ")
            .Append("          TBL_FLLWUPBOX_SALES A ")
            .Append("          ,TBL_ESTIMATEINFO B ")
            .Append("      WHERE ")
            .Append("          A.DLRCD = :DLRCD ")
            '2015/01/08 TCS 外崎 TMT2販社 ST-BTS 57 START
            '.Append("          AND TRIM(A.BRANCH_PLAN) = :STRCD ")
            '2015/01/08 TCS 外崎 TMT2販社 ST-BTS 57 END
            '2019/11/26 TS 髙橋(龍) SQL性能改善(TR-SLT-TMT-20190503-001) START
            .Append("          AND A.CRCUSTID = CAST(:INSDID AS CHAR(20 CHAR)) ")
            '2019/11/26 TS 髙橋(龍) SQL性能改善(TR-SLT-TMT-20190503-001) END
            .Append("          AND A.NEWFLLWUPBOXFLG = '1' ")
            .Append("          AND A.DLRCD = B.DLRCD(+) ")
            .Append("          AND A.STRCD = B.STRCD(+) ")
            .Append("          AND A.FLLWUPBOX_SEQNO = B.FLLWUPBOX_SEQNO(+) ")
            .Append("          AND B.CONTRACTFLG(+) = '1' ")
            .Append("          AND B.DELFLG(+) = '0' ")
            .Append("   )) T1 , ")
            .Append("   TBL_ESTIMATEINFO T2 , ")
            .Append("   TB_T_SALESBOOKING T3, ")
            .Append("   TB_T_AFTER_ODR T4")
            .Append(" WHERE ")
            .Append("       T1.FLLWUPBOX_SEQNO = T2.FLLWUPBOX_SEQNO(+) ")
            .Append("   AND T2.CONTRACTFLG(+) = '1' ")
            .Append("   AND T2.DELFLG(+) = '0' ")
            .Append("   AND TRIM(T2.DLRCD) = T3.DLR_CD(+) ")
            .Append("   AND TRIM(T2.CONTRACTNO) = T3.SALESBKG_NUM(+) ")
            .Append("   AND T4.SALES_ID(+) = T1.FLLWUPBOX_SEQNO ")
            .Append(" ORDER BY ")
            .Append("   T1.CRACTRESULT_UPDATEDATE DESC ")

        End With

        Using query As New DBSelectQuery(Of SC3080202DataSet.SC3080202GetFollowupboxListDataTable)("SC3080202_101")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)
            '2015/01/08 TCS 外崎 TMT2販社 ST-BTS 57 START
            'query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, strcd)
            '2015/01/08 TCS 外崎 TMT2販社 ST-BTS 57 END
            query.AddParameterWithTypeValue("INSDID", OracleDbType.Decimal, insdid)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetFollowupboxList_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
            Return query.GetData()
        End Using
    End Function


    ''' <summary>
    ''' 活動リスト取得SQL1(Active)
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Shared Function GetFollowupboxListSql1() As String
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetFollowupboxListSql1_Start")
        'ログ出力 End *****************************************************************************

        Dim sql As New StringBuilder

        With sql
            .Append("   (( ")
            .Append("   SELECT ")
            .Append("     TO_CHAR(T1.DLR_CD) AS DLRCD , ")
            .Append("     TO_CHAR(T1.BRN_CD) AS STRCD , ")
            .Append("     T1.SALES_ID AS FLLWUPBOX_SEQNO , ")
            .Append("     ' ' AS CRACTCATEGORY , ")
            .Append("     0 AS PROMOTION_ID , ")
            .Append("     CASE WHEN T3.RSLT_CONTACT_MTD = '11' THEN '1' ")
            .Append("          WHEN T3.RSLT_CONTACT_MTD = '12' THEN '2' ")
            .Append("          ELSE '0' ")
            .Append("     END AS REQCATEGORY , ")
            .Append("     T3.ACT_STATUS AS CRACTSTATUS , ")
            .Append("     CASE WHEN T2.REQ_STATUS = '31' THEN '3' ")
            .Append("          WHEN T2.REQ_STATUS = '32' THEN '5' ")
            .Append("          WHEN T2.REQ_STATUS = '21' THEN ")
            .Append("            CASE WHEN T1.SALES_PROSPECT_CD = '30' THEN '1' ")
            .Append("                 WHEN T1.SALES_PROSPECT_CD = '20' THEN '2' ")
            .Append("            ELSE '4' ")
            .Append("            END ")
            .Append("          ELSE ' ' ")
            .Append("     END AS CRACTRESULT , ")
            .Append("     TO_NCHAR(' ') AS SERVICENAME , ")
            .Append("     ' ' AS SUBCTGORGNAME , ")
            .Append("     ' ' AS PROMOTIONNAME , ")
            .Append("     NVL(NVL ")
            .Append("       ((SELECT ")
            .Append("           MAX(MAX(T16.ACTUALTIME_END)) ")
            .Append("         FROM ")
            .Append("           TBL_BOOKEDAFTERFOLLOWRSLT T16 ")
            .Append("         WHERE ")
            .Append("               T16.DLRCD(+) = T1.DLR_CD ")
            .Append("           AND T16.STRCD(+) = T1.BRN_CD ")
            .Append("           AND T16.FLLWUPBOX_SEQNO(+) = T1.SALES_ID ")
            .Append("         GROUP BY ")
            .Append("           T16.FLLWUPBOX_SEQNO) , ")
            .Append("        (SELECT ")
            .Append("           MAX(T17.RSLT_DATETIME) ")
            .Append("         FROM ")
            .Append("           TB_T_ACTIVITY T17 ")
            .Append("         WHERE ")
            .Append("               T17.REQ_ID(+) = T2.REQ_ID) ")
            .Append("       ) , ")
            .Append("     T2.REC_DATETIME) AS CRACTRESULT_UPDATEDATE , ")
            .Append("     TO_CHAR(T3.SCHE_STF_CD) AS ACCOUNT_PLAN , ")
            .Append("     T5.CONTRACTNO ")
            .Append("   FROM ")
            .Append("     TB_T_SALES T1 , ")
            .Append("     TB_T_REQUEST T2 , ")
            .Append("     TB_T_ACTIVITY T3 , ")
            .Append("     TBL_ESTIMATEINFO T5 ")
            .Append("   WHERE ")
            .Append("         T1.DLR_CD = :DLRCD ")
            '2015/01/08 TCS 外崎 TMT2販社 ST-BTS 57 START
            '.Append("     AND T1.BRN_CD = :STRCD ")
            '2015/01/08 TCS 外崎 TMT2販社 ST-BTS 57 END
            .Append("     AND T1.CST_ID = :INSDID ")
            .Append("     AND T1.REQ_ID = T2.REQ_ID ")
            .Append("     AND T2.LAST_ACT_ID = T3.ACT_ID ")
            .Append("     AND T3.ACT_STATUS IN ('21','31','32') ")
            .Append("     AND T1.SALES_ID = T5.FLLWUPBOX_SEQNO(+) ")
            .Append("     AND T1.SALES_PROSPECT_CD <> ' ' ")
            .Append("     AND T5.CONTRACTFLG(+) = '1' ")
            .Append("     AND T5.DELFLG(+) = '0' ")
            .Append("    ) ")
            .Append(" UNION ALL ")
            .Append("   ( ")
            .Append("   SELECT ")
            .Append("     TO_CHAR(T7.DLR_CD) AS DLRCD , ")
            .Append("     TO_CHAR(T7.BRN_CD) AS STRCD , ")
            .Append("     T7.SALES_ID AS FLLWUPBOX_SEQNO , ")
            .Append("    CASE WHEN T11.ATTPLAN_TYPE = '1' THEN  ")
            .Append("           CASE WHEN T8.SVC_PTN_TYPE = '1' THEN '1' ")
            .Append("                WHEN T8.SVC_PTN_TYPE = '2' THEN '2' ")
            .Append("           ELSE ' ' ")
            .Append("           END ")
            .Append("         WHEN T11.ATTPLAN_TYPE = '2' THEN ")
            .Append("           CASE WHEN T11.SPECIFY_TYPE = '05' THEN '4' ")
            .Append("           ELSE ' ' ")
            .Append("           END ")
            .Append("         ELSE ' ' ")
            .Append("    END AS CRACTCATEGORY , ")
            .Append("     T8.ATTPLAN_ID AS PROMOTION_ID , ")
            .Append("     CASE WHEN T9.RSLT_CONTACT_MTD = '11' THEN '1' ")
            .Append("          WHEN T9.RSLT_CONTACT_MTD = '12' THEN '2' ")
            .Append("          ELSE '0' ")
            .Append("     END AS REQCATEGORY , ")
            .Append("     T9.ACT_STATUS AS CRACTSTATUS , ")
            .Append("     CASE WHEN T8.CONTINUE_ACT_STATUS = '31' THEN '3' ")
            .Append("          WHEN T8.CONTINUE_ACT_STATUS = '32' THEN '5' ")
            .Append("          WHEN T8.CONTINUE_ACT_STATUS = '21' THEN ")
            .Append("            CASE WHEN T7.SALES_PROSPECT_CD = '30' THEN '1' ")
            .Append("                 WHEN T7.SALES_PROSPECT_CD = '20' THEN '2' ")
            .Append("            ELSE '4' ")
            .Append("            END ")
            .Append("          ELSE ' ' ")
            .Append("     END AS CRACTRESULT , ")
            .Append("     T10.SVC_NAME_MILE AS SERVICENAME , ")
            .Append("     TO_CHAR(T11.ATTPLAN_NAME) AS SUBCTGORGNAME , ")
            .Append("     TO_CHAR(T11.ATTPLAN_NAME) AS PROMOTIONNAME , ")
            .Append("     NVL(NVL ")
            .Append("       ((SELECT ")
            .Append("           MAX(MAX(T18.ACTUALTIME_END)) ")
            .Append("         FROM ")
            .Append("           TBL_BOOKEDAFTERFOLLOWRSLT T18 ")
            .Append("         WHERE ")
            .Append("               T18.DLRCD(+) = T7.DLR_CD ")
            .Append("           AND T18.STRCD(+) = T7.BRN_CD ")
            .Append("           AND T18.FLLWUPBOX_SEQNO(+) = T7.SALES_ID ")
            .Append("         GROUP BY ")
            .Append("           T18.FLLWUPBOX_SEQNO) , ")
            .Append("        (SELECT ")
            .Append("           MAX(T19.RSLT_DATETIME) ")
            .Append("         FROM ")
            .Append("           TB_T_ACTIVITY T19 ")
            .Append("         WHERE ")
            .Append("               T19.ATT_ID(+) = T8.ATT_ID) ")
            .Append("       ) , ")
            .Append("       T8.ATT_CREATE_DATETIME) AS CRACTRESULT_UPDATEDATE , ")
            .Append("       TO_CHAR(T9.SCHE_STF_CD) AS ACCOUNT_PLAN , ")
            .Append("       T12.CONTRACTNO  ")
            .Append("   FROM ")
            .Append("     TB_T_SALES T7 , ")
            .Append("     TB_T_ATTRACT T8 , ")
            .Append("     TB_T_ACTIVITY T9 , ")
            .Append("     TB_M_SERVICE T10 , ")
            .Append("     TB_M_ATTPLAN T11 , ")
            .Append("     TBL_ESTIMATEINFO T12 ")
            .Append("   WHERE T7.DLR_CD = :DLRCD ")
            '2015/01/08 TCS 外崎 TMT2販社 ST-BTS 57 START
            '.Append("     AND T7.BRN_CD = :STRCD ")
            '2015/01/08 TCS 外崎 TMT2販社 ST-BTS 57 END
            .Append("     AND T7.CST_ID = :INSDID ")
            .Append("     AND T7.ATT_ID = T8.ATT_ID ")
            .Append("     AND T10.DLR_CD(+) = 'XXXXX' ")
            .Append("     AND T8.SVC_CD = T10.SVC_CD (+) ")
            .Append("     AND T8.ATT_STATUS = '31' ")
            .Append("     AND T8.ATTPLAN_ID = T11.ATTPLAN_ID(+) ")
            .Append("     AND T8.LAST_ACT_ID = T9.ACT_ID ")
            .Append("     AND T9.ACT_STATUS IN ('21','31','32') ")
            .Append("     AND T7.SALES_ID = T12.FLLWUPBOX_SEQNO(+) ")
            .Append("     AND T7.SALES_PROSPECT_CD <> ' ' ")
            .Append("     AND T12.CONTRACTFLG(+) = '1' ")
            .Append("     AND T12.DELFLG(+) = '0' ")
            .Append("     AND T8.ATTPLAN_VERSION = T11.ATTPLAN_VERSION ")
            .Append("     AND T8.ATTPLAN_CREATE_DLR_CD = T11.DLR_CD ")
            .Append("     AND T8.ATTPLAN_CREATE_BRN_CD = T11.BRN_CD ")
            .Append("   ) ")
        End With

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetFollowupboxListSql1_End")
        'ログ出力 End *****************************************************************************

        Return sql.ToString()
    End Function

    ''' <summary>
    ''' 活動リスト取得SQL2(History)
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Shared Function GetFollowupboxListSql2() As String
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetFollowupboxListSql2_Start")
        'ログ出力 End *****************************************************************************

        Dim sql As New StringBuilder

        With sql
            .Append("   ( ")
            .Append("   SELECT ")
            .Append("     TO_CHAR(T101.DLR_CD) AS DLRCD , ")
            .Append("     TO_CHAR(T101.BRN_CD) AS STRCD , ")
            .Append("     T101.SALES_ID AS FLLWUPBOX_SEQNO , ")
            .Append("     ' ' AS CRACTCATEGORY , ")
            .Append("     0 AS PROMOTION_ID , ")
            .Append("     CASE WHEN T103.RSLT_CONTACT_MTD = '11' THEN '1' ")
            .Append("          WHEN T103.RSLT_CONTACT_MTD = '12' THEN '2' ")
            .Append("          ELSE '0' ")
            .Append("     END AS REQCATEGORY , ")
            .Append("     T103.ACT_STATUS AS CRACTSTATUS , ")
            .Append("     CASE WHEN T102.REQ_STATUS = '31' THEN '3' ")
            .Append("          WHEN T102.REQ_STATUS = '32' THEN '5' ")
            .Append("          WHEN T102.REQ_STATUS = '21' THEN ")
            .Append("            CASE WHEN T101.SALES_PROSPECT_CD = '30' THEN '1' ")
            .Append("                 WHEN T101.SALES_PROSPECT_CD = '20' THEN '2' ")
            .Append("            ELSE '4' ")
            .Append("            END ")
            .Append("           ELSE ' ' ")
            .Append("      END AS CRACTRESULT , ")
            .Append("     TO_NCHAR(' ') AS SERVICENAME , ")
            .Append("     ' ' AS SUBCTGORGNAME , ")
            .Append("     ' ' AS PROMOTIONNAME , ")
            .Append("     NVL(NVL ")
            .Append("       ((SELECT ")
            .Append("           MAX(MAX(T116.ACTUALTIME_END)) ")
            .Append("         FROM ")
            .Append("           TBL_BOOKEDAFTERFOLLOWRSLT T116 ")
            .Append("         WHERE ")
            .Append("               T116.DLRCD(+) = T101.DLR_CD ")
            .Append("           AND T116.STRCD(+) = T101.BRN_CD ")
            .Append("           AND T116.FLLWUPBOX_SEQNO(+) = T101.SALES_ID ")
            .Append("         GROUP BY ")
            .Append("           T116.FLLWUPBOX_SEQNO) , ")
            .Append("        (SELECT ")
            .Append("           MAX(T117.RSLT_DATETIME) ")
            .Append("         FROM ")
            .Append("           TB_H_ACTIVITY T117 ")
            .Append("         WHERE ")
            .Append("               T117.REQ_ID(+) = T102.REQ_ID) ")
            .Append("       ) , ")
            .Append("       T102.REC_DATETIME) AS CRACTRESULT_UPDATEDATE , ")
            .Append("       TO_CHAR(T103.SCHE_STF_CD) AS ACCOUNT_PLAN , ")
            .Append("       T105.CONTRACTNO ")
            .Append("   FROM ")
            .Append("     TB_H_SALES T101 , ")
            .Append("     TB_H_REQUEST T102 , ")
            .Append("     TB_H_ACTIVITY T103 , ")
            .Append("     TBL_ESTIMATEINFO T105 ")
            .Append("   WHERE T101.DLR_CD = :DLRCD ")
            '2015/01/08 TCS 外崎 TMT2販社 ST-BTS 57 START
            '.Append("     AND T101.BRN_CD = :STRCD ")
            '2015/01/08 TCS 外崎 TMT2販社 ST-BTS 57 END
            .Append("     AND T101.CST_ID = :INSDID ")
            .Append("     AND T101.REQ_ID = T102.REQ_ID ")
            .Append("     AND T102.LAST_ACT_ID = T103.ACT_ID ")
            .Append("     AND T103.ACT_STATUS IN ('21','31','32') ")
            .Append("     AND T101.SALES_ID = T105.FLLWUPBOX_SEQNO(+) ")
            .Append("     AND T101.SALES_PROSPECT_CD <> ' ' ")
            .Append("     AND T105.CONTRACTFLG(+) = '1' ")
            .Append("     AND T105.DELFLG(+) = '0' ")
            .Append("   ) ")
            .Append(" UNION ALL ")
            .Append("   ( ")
            .Append("   SELECT ")
            .Append("     TO_CHAR(T107.DLR_CD) AS DLRCD , ")
            .Append("     TO_CHAR(T107.BRN_CD) AS STRCD , ")
            .Append("     T107.SALES_ID AS FLLWUPBOX_SEQNO , ")
            .Append("    CASE WHEN T111.ATTPLAN_TYPE = '1' THEN  ")
            .Append("           CASE WHEN T108.SVC_PTN_TYPE = '1' THEN '1' ")
            .Append("                WHEN T108.SVC_PTN_TYPE = '2' THEN '2' ")
            .Append("           ELSE ' ' ")
            .Append("           END ")
            .Append("         WHEN T111.ATTPLAN_TYPE = '2' THEN ")
            .Append("           CASE WHEN T111.SPECIFY_TYPE = '05' THEN '4' ")
            .Append("           ELSE ' ' ")
            .Append("           END ")
            .Append("         ELSE ' ' ")
            .Append("    END AS CRACTCATEGORY , ")
            .Append("     T108.ATTPLAN_ID AS PROMOTION_ID , ")
            .Append("     CASE WHEN T109.RSLT_CONTACT_MTD = '11' THEN '1' ")
            .Append("          WHEN T109.RSLT_CONTACT_MTD = '12' THEN '2' ")
            .Append("          ELSE '0' ")
            .Append("      END AS REQCATEGORY , ")
            .Append("     T109.ACT_STATUS AS CRACTSTATUS , ")
            .Append("     CASE WHEN T108.CONTINUE_ACT_STATUS = '31' THEN '3' ")
            .Append("          WHEN T108.CONTINUE_ACT_STATUS = '32' THEN '5' ")
            .Append("          WHEN T108.CONTINUE_ACT_STATUS = '21' THEN ")
            .Append("            CASE WHEN T107.SALES_PROSPECT_CD = '30' THEN '1' ")
            .Append("                 WHEN T107.SALES_PROSPECT_CD = '20' THEN '2' ")
            .Append("            ELSE '4' ")
            .Append("            END ")
            .Append("          ELSE ' ' ")
            .Append("     END AS CRACTRESULT , ")
            .Append("     T110.SVC_NAME_MILE AS SERVICENAME , ")
            .Append("     TO_CHAR(T111.ATTPLAN_NAME) AS SUBCTGORGNAME , ")
            .Append("     TO_CHAR(T111.ATTPLAN_NAME) AS PROMOTIONNAME , ")
            .Append("     NVL(NVL ")
            .Append("       ((SELECT ")
            .Append("           MAX(MAX(T118.ACTUALTIME_END)) ")
            .Append("         FROM ")
            .Append("           TBL_BOOKEDAFTERFOLLOWRSLT T118 ")
            .Append("         WHERE ")
            .Append("               T118.DLRCD(+) = T107.DLR_CD ")
            .Append("           AND T118.STRCD(+) = T107.BRN_CD ")
            .Append("           AND T118.FLLWUPBOX_SEQNO(+) = T107.SALES_ID ")
            .Append("         GROUP BY ")
            .Append("           T118.FLLWUPBOX_SEQNO) , ")
            .Append("        (SELECT ")
            .Append("           MAX(T119.RSLT_DATETIME) ")
            .Append("         FROM ")
            .Append("           TB_H_ACTIVITY T119 ")
            .Append("         WHERE ")
            .Append("               T119.ATT_ID(+) = T108.ATT_ID) ")
            .Append("       ) , ")
            .Append("       T108.ATT_CREATE_DATETIME) AS CRACTRESULT_UPDATEDATE , ")
            .Append("       TO_CHAR(T109.SCHE_STF_CD) AS ACCOUNT_PLAN , ")
            .Append("       T112.CONTRACTNO  ")
            .Append("   FROM ")
            .Append("     TB_H_SALES T107 , ")
            .Append("     TB_H_ATTRACT T108 , ")
            .Append("     TB_H_ACTIVITY T109 , ")
            .Append("     TB_M_SERVICE T110 , ")
            .Append("     TB_M_ATTPLAN T111 , ")
            .Append("     TBL_ESTIMATEINFO T112 ")
            .Append("   WHERE T107.DLR_CD = :DLRCD ")
            '2015/01/08 TCS 外崎 TMT2販社 ST-BTS 57 START
            '.Append("     AND T107.BRN_CD = :STRCD ")
            '2015/01/08 TCS 外崎 TMT2販社 ST-BTS 57 END
            .Append("     AND T107.CST_ID = :INSDID ")
            .Append("     AND T107.ATT_ID = T108.ATT_ID ")
            .Append("     AND T110.DLR_CD(+) = 'XXXXX' ")
            .Append("     AND T108.SVC_CD = T110.SVC_CD(+) ")
            .Append("     AND T108.ATT_STATUS = '31' ")
            .Append("     AND T108.ATTPLAN_ID = T111.ATTPLAN_ID(+) ")
            .Append("     AND T108.LAST_ACT_ID = T109.ACT_ID ")
            .Append("     AND T109.ACT_STATUS IN ('21','31','32') ")
            .Append("     AND T107.SALES_ID = T112.FLLWUPBOX_SEQNO(+) ")
            .Append("     AND T107.SALES_PROSPECT_CD <> ' ' ")
            .Append("     AND T112.CONTRACTFLG(+) = '1' ")
            .Append("     AND T112.DELFLG(+) = '0' ")
            .Append("     AND T108.ATTPLAN_VERSION = T111.ATTPLAN_VERSION ")
            .Append("     AND T108.ATTPLAN_CREATE_DLR_CD = T111.DLR_CD ")
            .Append("     AND T108.ATTPLAN_CREATE_BRN_CD = T111.BRN_CD ")
            .Append("      ) ")
        End With

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetFollowupboxListSql2_End")
        'ログ出力 End *****************************************************************************

        Return sql.ToString()
    End Function

    '2014/02/12 TCS 山口 受注後フォロー機能開発 END

    '2016/09/12 TCS 鈴木 性能改善（TR-SLT-TMT-20160726-002）MOD START
    '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 活動詳細取得
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="fllwupboxseqno">Follow-up Box内連番</param>
    ''' <param name="activeOrHistoryFlg">アクティブ/ヒストリーフラグ（0:アクティブ　1:ヒストリー）</param>
    ''' <param name="reqOrAttFlg">用件/誘致フラグ（0:用件 1:誘致）</param>
    ''' <param name="afterOdrProcFlg">受注後工程利用フラグ（0：利用しない 1：利用する）</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetFollowupboxDetail(ByVal dlrcd As String,
                                         ByVal fllwupboxseqno As String,
                                         ByVal activeOrHistoryFlg As String,
                                         ByVal reqOrAttFlg As String,
                                         ByVal afterOdrProcFlg As String) As SC3080202DataSet.SC3080202GetFollowupboxDetailDataTable

        Dim sql As New StringBuilder

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetFollowupboxDetail_Start")
        'ログ出力 End *****************************************************************************

        With sql
            .Append("SELECT ")
            .Append("  /* SC3080202_102 */ ")
            .Append("  ACTID , ")
            .Append("  REQID , ")
            .Append("  ATTID , ")
            .Append("  COUNT , ")
            .Append("  SCHEDATEORTIME , ")
            .Append("  REQUESTLOCKVERSION , ")
            .Append("  ATTRACTLOCKVERSION , ")
            .Append("  SALESLOCKVERSION , ")
            .Append("  ACTIVITYLOCKVERSION , ")
            .Append("  COMPFLG , ")
            .Append("  GIVEUPVCLSEQ , ")
            .Append("  RSLTCONTACTMTD , ")
            .Append("  RSLTSALESCAT , ")
            .Append("  MODELCODE , ")
            .Append("  MODELNAME , ")
            .Append("  LASTCALLRSLTID , ")
            .Append("  CONTACT , ")
            .Append("  NVL(TRIM(CONTACTNO),0) AS CONTACTNO , ")
            .Append("  SALESSTARTTIME , ")
            .Append("  SALESENDTIME , ")
            .Append("  WALKINNUM , ")
            .Append("  USERNAME , ")
            .Append("  OPERATIONCODE , ")
            .Append("  ICON_IMGFILE , ")
            .Append("  NVL(COUNTVIEW,0) AS COUNTVIEW ")
            .Append("FROM( ")

            If String.Equals(activeOrHistoryFlg, "0") Then
                '【Active】                           
                If String.Equals(reqOrAttFlg, "0") Then
                    '【用件】
                    .Append("  ( ")
                    .Append("  SELECT DISTINCT ")
                    .Append("    T8.ACT_ID AS ACTID , ")
                    .Append("    T8.REQ_ID AS REQID , ")
                    .Append("    T8.ATT_ID AS ATTID , ")
                    .Append("    T8.ACT_COUNT AS COUNT , ")
                    .Append("    T8.SCHE_DATEORTIME AS SCHEDATEORTIME , ")
                    .Append("    T6.ROW_LOCK_VERSION AS REQUESTLOCKVERSION , ")
                    .Append("    0 AS ATTRACTLOCKVERSION , ")
                    .Append("    T1.ROW_LOCK_VERSION AS SALESLOCKVERSION , ")
                    .Append("    T8.ROW_LOCK_VERSION AS ACTIVITYLOCKVERSION , ")
                    .Append("    T1.SALES_COMPLETE_FLG AS COMPFLG , ")
                    .Append("    T1.GIVEUP_COMP_VCL_SEQ AS GIVEUPVCLSEQ , ")
                    .Append("    T8.RSLT_CONTACT_MTD AS RSLTCONTACTMTD , ")
                    .Append("    T5.RSLT_SALES_CAT AS RSLTSALESCAT , ")
                    .Append("    T5.MODEL_CD AS MODELCODE , ")
                    .Append("    T5.ASSMNT_VCL_NAME AS MODELNAME , ")
                    .Append("    T6.LAST_CALL_RSLT_ID AS LASTCALLRSLTID , ")
                    .Append("    T2.CONTACT_NAME AS CONTACT , ")
                    .Append("    T8.RSLT_CONTACT_MTD AS CONTACTNO , ")
                    .Append("    T9.STARTTIME AS SALESSTARTTIME , ")
                    .Append("    T9.ENDTIME AS SALESENDTIME , ")
                    .Append("    T9.WALKINNUM , ")
                    .Append("    T3.USERNAME , ")
                    .Append("    T3.OPERATIONCODE , ")
                    .Append("    T4.ICON_IMGFILE , ")
                    .Append("    T2.COUNT_DISP_FLG AS COUNTVIEW , ")
                    .Append("    T8.RSLT_DATETIME AS ACTUALDATE , ")
                    .Append("    T8.RSLT_DATETIME AS UPDATEDATE ")
                    .Append("  FROM ")
                    .Append("    TB_T_SALES T1 , ")
                    .Append("    TB_M_CONTACT_MTD T2 , ")
                    .Append("    TBL_USERS T3 , ")
                    .Append("    TBL_OPERATIONTYPE T4 , ")
                    .Append("    TB_T_SALES_ACT T5 , ")
                    .Append("    TB_T_REQUEST T6 , ")
                    .Append("    TB_M_CUSTOMER_VCL T7 , ")
                    .Append("    TB_T_ACTIVITY T8 , ")
                    .Append("    TBL_FLLWUPBOX_SALES T9 ")
                    .Append("  WHERE ")
                    .Append("        T8.RSLT_CONTACT_MTD = T2.CONTACT_MTD(+) ")
                    .Append("    AND T8.RSLT_STF_CD = T3.ACCOUNT ")
                    .Append("    AND T1.SALES_ID = :FLLWUPBOXSEQNO ")
                    .Append("    AND T3.OPERATIONCODE = T4.OPERATIONCODE ")
                    .Append("    AND T3.DLRCD = T4.DLRCD ")
                    .Append("    AND T4.DLRCD = :DLRCD ")
                    .Append("    AND T4.STRCD = '000' ")
                    .Append("    AND T1.REQ_ID = T8.REQ_ID ")
                    .Append("    AND T8.REQ_ID = T8.REQ_ID ")
                    .Append("    AND T1.SALES_ID  = T5.SALES_ID(+) ")
                    .Append("    AND T1.DLR_CD = T7.DLR_CD ")
                    .Append("    AND T1.CST_ID = T7.CST_ID ")
                    .Append("    AND T1.CST_ID = T6.CST_ID ")
                    .Append("    AND T9.FLLWUPBOX_SEQNO(+) = T1.SALES_ID ")
                    .Append("    AND T9.REGISTFLG(+) = '1' ")
                    '2014/09/04 TCS 武田 UAT不具合対応(最終活動表示) START
                    .Append("    AND T9.CST_SERVICE_TYPE(+) IN ('1', ' ') ")
                    '2014/09/04 TCS 武田 UAT不具合対応(最終活動表示) END
                    .Append("  ) ")
                Else
                    '【誘致】
                    .Append("  ( ")
                    .Append("  SELECT DISTINCT ")
                    .Append("    T8.ACT_ID AS ACTID , ")
                    .Append("    T8.REQ_ID AS REQID , ")
                    .Append("    T8.ATT_ID AS ATTID , ")
                    .Append("    T8.ACT_COUNT AS COUNT , ")
                    .Append("    T8.SCHE_DATEORTIME AS SCHEDATEORTIME , ")
                    .Append("    0 AS REQUESTLOCKVERSION , ")
                    .Append("    T6.ROW_LOCK_VERSION AS ATTRACTLOCKVERSION , ")
                    .Append("    T1.ROW_LOCK_VERSION AS SALESLOCKVERSION , ")
                    .Append("    T8.ROW_LOCK_VERSION AS ACTIVITYLOCKVERSION , ")
                    .Append("    T1.SALES_COMPLETE_FLG AS COMPFLG , ")
                    .Append("    T1.GIVEUP_COMP_VCL_SEQ AS GIVEUPVCLSEQ , ")
                    .Append("    T8.RSLT_CONTACT_MTD AS RSLTCONTACTMTD , ")
                    .Append("    T5.RSLT_SALES_CAT AS RSLTSALESCAT , ")
                    .Append("    T5.MODEL_CD AS MODELCODE , ")
                    .Append("    T5.ASSMNT_VCL_NAME AS MODELNAME , ")
                    .Append("    T6.LAST_RSLT_ID AS LASTCALLRSLTID , ")
                    .Append("    T2.CONTACT_NAME AS CONTACT , ")
                    .Append("    T8.RSLT_CONTACT_MTD AS CONTACTNO , ")
                    .Append("    T9.STARTTIME AS SALESSTARTTIME , ")
                    .Append("    T9.ENDTIME AS SALESENDTIME , ")
                    .Append("    T9.WALKINNUM , ")
                    .Append("    T3.USERNAME , ")
                    .Append("    T3.OPERATIONCODE , ")
                    .Append("    T4.ICON_IMGFILE , ")
                    .Append("    T2.COUNT_DISP_FLG AS COUNTVIEW , ")
                    .Append("    T8.RSLT_DATETIME AS ACTUALDATE , ")
                    .Append("    T8.RSLT_DATETIME AS UPDATEDATE ")
                    .Append("  FROM ")
                    .Append("    TB_T_SALES T1 , ")
                    .Append("    TB_M_CONTACT_MTD T2 , ")
                    .Append("    TBL_USERS T3 , ")
                    .Append("    TBL_OPERATIONTYPE T4 , ")
                    .Append("    TB_T_SALES_ACT T5 , ")
                    .Append("    TB_T_ATTRACT T6 , ")
                    .Append("    TB_M_CUSTOMER_VCL T7 , ")
                    .Append("    TB_T_ACTIVITY T8 , ")
                    .Append("    TBL_FLLWUPBOX_SALES T9 ")
                    .Append("  WHERE ")
                    .Append("        T8.RSLT_CONTACT_MTD = T2.CONTACT_MTD(+) ")
                    .Append("    AND T8.RSLT_STF_CD = T3.ACCOUNT ")
                    .Append("    AND T1.SALES_ID = :FLLWUPBOXSEQNO ")
                    .Append("    AND T3.OPERATIONCODE = T4.OPERATIONCODE ")
                    .Append("    AND T3.DLRCD = T4.DLRCD ")
                    .Append("    AND T4.DLRCD = :DLRCD ")
                    .Append("    AND T4.STRCD = '000' ")
                    .Append("    AND T1.ATT_ID = T8.ATT_ID ")
                    .Append("    AND T6.ATT_ID = T8.ATT_ID ")
                    .Append("    AND T1.SALES_ID  = T5.SALES_ID(+) ")
                    .Append("    AND T1.DLR_CD = T7.DLR_CD ")
                    .Append("    AND T1.CST_ID = T7.CST_ID ")
                    .Append("    AND T1.CST_ID = T6.CST_ID ")
                    .Append("    AND T9.FLLWUPBOX_SEQNO(+) = T1.SALES_ID ")
                    .Append("    AND T9.REGISTFLG(+) = '1' ")
                    '2014/09/04 TCS 武田 UAT不具合対応(最終活動表示) START
                    .Append("    AND T9.CST_SERVICE_TYPE(+) IN ('1', ' ') ")
                    '2014/09/04 TCS 武田 UAT不具合対応(最終活動表示) END
                    .Append("  ) ")
                End If
            Else
                '【History】
                If String.Equals(reqOrAttFlg, "0") Then
                    '【用件】
                    .Append("  ( ")
                    .Append("  SELECT DISTINCT ")
                    .Append("    T8.ACT_ID AS ACTID , ")
                    .Append("    T8.REQ_ID AS REQID , ")
                    .Append("    T8.ATT_ID AS ATTID , ")
                    .Append("    T8.ACT_COUNT AS COUNT , ")
                    .Append("    T8.SCHE_DATEORTIME AS SCHEDATEORTIME , ")
                    .Append("    T6.ROW_LOCK_VERSION AS REQUESTLOCKVERSION , ")
                    .Append("    0 AS ATTRACTLOCKVERSION , ")
                    .Append("    T1.ROW_LOCK_VERSION AS SALESLOCKVERSION , ")
                    .Append("    T8.ROW_LOCK_VERSION AS ACTIVITYLOCKVERSION , ")
                    .Append("    T1.SALES_COMPLETE_FLG AS COMPFLG , ")
                    .Append("    T1.GIVEUP_COMP_VCL_SEQ AS GIVEUPVCLSEQ , ")
                    .Append("    T8.RSLT_CONTACT_MTD AS RSLTCONTACTMTD , ")
                    .Append("    T5.RSLT_SALES_CAT AS RSLTSALESCAT , ")
                    .Append("    T5.MODEL_CD AS MODELCODE , ")
                    .Append("    T5.ASSMNT_VCL_NAME AS MODELNAME , ")
                    .Append("    T6.LAST_CALL_RSLT_ID AS LASTCALLRSLTID , ")
                    .Append("    T2.CONTACT_NAME AS CONTACT , ")
                    .Append("    T8.RSLT_CONTACT_MTD AS CONTACTNO , ")
                    .Append("    T9.STARTTIME AS SALESSTARTTIME , ")
                    .Append("    T9.ENDTIME AS SALESENDTIME , ")
                    .Append("    T9.WALKINNUM , ")
                    .Append("    T3.USERNAME , ")
                    .Append("    T3.OPERATIONCODE , ")
                    .Append("    T4.ICON_IMGFILE , ")
                    .Append("    T2.COUNT_DISP_FLG AS COUNTVIEW , ")
                    .Append("    T8.RSLT_DATETIME AS ACTUALDATE , ")
                    .Append("    T8.RSLT_DATETIME AS UPDATEDATE ")
                    .Append("  FROM ")
                    .Append("    TB_H_SALES T1 , ")
                    .Append("    TB_M_CONTACT_MTD T2 , ")
                    .Append("    TBL_USERS T3 , ")
                    .Append("    TBL_OPERATIONTYPE T4 , ")
                    .Append("    TB_H_SALES_ACT T5 , ")
                    .Append("    TB_H_REQUEST T6 , ")
                    .Append("    TB_M_CUSTOMER_VCL T7 , ")
                    .Append("    TB_H_ACTIVITY T8 , ")
                    .Append("    TBL_FLLWUPBOX_SALES T9 ")
                    .Append("  WHERE ")
                    .Append("        T8.RSLT_CONTACT_MTD = T2.CONTACT_MTD(+) ")
                    .Append("    AND T8.RSLT_STF_CD = T3.ACCOUNT ")
                    .Append("    AND T1.SALES_ID = :FLLWUPBOXSEQNO ")
                    .Append("    AND T3.OPERATIONCODE = T4.OPERATIONCODE ")
                    .Append("    AND T3.DLRCD = T4.DLRCD ")
                    .Append("    AND T4.DLRCD = :DLRCD ")
                    .Append("    AND T4.STRCD = '000' ")
                    .Append("    AND T1.REQ_ID = T8.REQ_ID ")
                    .Append("    AND T6.REQ_ID = T8.REQ_ID ")
                    .Append("    AND T1.SALES_ID  = T5.SALES_ID(+) ")
                    .Append("    AND T1.DLR_CD = T7.DLR_CD ")
                    .Append("    AND T1.CST_ID = T7.CST_ID ")
                    .Append("    AND T1.CST_ID = T6.CST_ID ")
                    .Append("    AND T9.FLLWUPBOX_SEQNO(+) = T1.SALES_ID ")
                    .Append("    AND T9.REGISTFLG(+) = '1' ")
                    '2014/09/04 TCS 武田 UAT不具合対応(最終活動表示) START
                    .Append("    AND T9.CST_SERVICE_TYPE(+) IN ('1', ' ') ")
                    '2014/09/04 TCS 武田 UAT不具合対応(最終活動表示) END
                    .Append("  ) ")
                Else
                    '【誘致】
                    .Append("  ( ")
                    .Append("  SELECT DISTINCT ")
                    .Append("    T8.ACT_ID AS ACTID , ")
                    .Append("    T8.REQ_ID AS REQID , ")
                    .Append("    T8.ATT_ID AS ATTID , ")
                    .Append("    T8.ACT_COUNT AS COUNT , ")
                    .Append("    T8.SCHE_DATEORTIME AS SCHEDATEORTIME , ")
                    .Append("    0 AS REQUESTLOCKVERSION , ")
                    .Append("    T6.ROW_LOCK_VERSION AS ATTRACTLOCKVERSION , ")
                    .Append("    T1.ROW_LOCK_VERSION AS SALESLOCKVERSION , ")
                    .Append("    T8.ROW_LOCK_VERSION AS ACTIVITYLOCKVERSION , ")
                    .Append("    T1.SALES_COMPLETE_FLG AS COMPFLG , ")
                    .Append("    T1.GIVEUP_COMP_VCL_SEQ AS GIVEUPVCLSEQ , ")
                    .Append("    T8.RSLT_CONTACT_MTD AS RSLTCONTACTMTD , ")
                    .Append("    T5.RSLT_SALES_CAT AS RSLTSALESCAT , ")
                    .Append("    T5.MODEL_CD AS MODELCODE , ")
                    .Append("    T5.ASSMNT_VCL_NAME AS MODELNAME , ")
                    .Append("    T6.LAST_RSLT_ID AS LASTCALLRSLTID , ")
                    .Append("    T2.CONTACT_NAME AS CONTACT , ")
                    .Append("    T8.RSLT_CONTACT_MTD AS CONTACTNO , ")
                    .Append("    T9.STARTTIME AS SALESSTARTTIME , ")
                    .Append("    T9.ENDTIME AS SALESENDTIME , ")
                    .Append("    T9.WALKINNUM , ")
                    .Append("    T3.USERNAME , ")
                    .Append("    T3.OPERATIONCODE , ")
                    .Append("    T4.ICON_IMGFILE , ")
                    .Append("    T2.COUNT_DISP_FLG AS COUNTVIEW , ")
                    .Append("    T8.RSLT_DATETIME AS ACTUALDATE , ")
                    .Append("    T8.RSLT_DATETIME AS UPDATEDATE ")
                    .Append("  FROM ")
                    .Append("    TB_H_SALES T1 , ")
                    .Append("    TB_M_CONTACT_MTD T2 , ")
                    .Append("    TBL_USERS T3 , ")
                    .Append("    TBL_OPERATIONTYPE T4 , ")
                    .Append("    TB_H_SALES_ACT T5 , ")
                    .Append("    TB_H_ATTRACT T6 , ")
                    .Append("    TB_M_CUSTOMER_VCL T7 , ")
                    .Append("    TB_H_ACTIVITY T8 , ")
                    .Append("    TBL_FLLWUPBOX_SALES T9 ")
                    .Append("  WHERE ")
                    .Append("        T8.RSLT_CONTACT_MTD = T2.CONTACT_MTD(+) ")
                    .Append("    AND T8.RSLT_STF_CD = T3.ACCOUNT ")
                    .Append("    AND T1.SALES_ID = :FLLWUPBOXSEQNO ")
                    .Append("    AND T3.OPERATIONCODE = T4.OPERATIONCODE ")
                    .Append("    AND T3.DLRCD = T4.DLRCD ")
                    .Append("    AND T4.DLRCD = :DLRCD ")
                    .Append("    AND T4.STRCD = '000' ")
                    .Append("    AND T1.ATT_ID = T8.ATT_ID ")
                    .Append("    AND T6.ATT_ID = T8.ATT_ID ")
                    .Append("    AND T1.CST_ID = T6.CST_ID ")
                    .Append("    AND T1.SALES_ID  = T5.SALES_ID(+) ")
                    .Append("    AND T1.DLR_CD = T7.DLR_CD ")
                    .Append("    AND T1.CST_ID = T7.CST_ID ")
                    .Append("    AND T9.FLLWUPBOX_SEQNO(+) = T1.SALES_ID ")
                    .Append("    AND T9.REGISTFLG(+) = '1' ")
                    '2014/09/04 TCS 武田 UAT不具合対応(最終活動表示) START
                    .Append("    AND T9.CST_SERVICE_TYPE(+) IN ('1', ' ') ")
                    '2014/09/04 TCS 武田 UAT不具合対応(最終活動表示) END
                    .Append("  ) ")
                End If

                If String.Equals(afterOdrProcFlg, "1") Then
                    '【受注後活動結果：利用する】
                    .Append("UNION ALL ")
                    .Append("  ( ")
                    .Append("  SELECT DISTINCT ")
                    .Append("    0 AS ACTID , ")
                    .Append("    0 AS REQID , ")
                    .Append("    0 AS ATTID , ")
                    .Append("    0 AS COUNT , ")
                    .Append("    SYSDATE AS SCHEDATEORTIME , ")
                    .Append("    0 AS REQUESTLOCKVERSION , ")
                    .Append("    0 AS ATTRACTLOCKVERSION , ")
                    .Append("    0 AS SALESLOCKVERSION , ")
                    .Append("    0 AS ACTIVITYLOCKVERSION , ")
                    .Append("    TO_NCHAR('') AS COMPFLG , ")
                    .Append("    0 AS GIVEUPVCLSEQ , ")
                    .Append("    TO_NCHAR('') AS RSLTCONTACTMTD , ")
                    .Append("    TO_NCHAR('') AS RSLTSALESCAT , ")
                    .Append("    TO_NCHAR('') AS MODELCODE , ")
                    .Append("    TO_NCHAR('') AS MODELNAME , ")
                    .Append("    0 AS LASTCALLRSLTID , ")
                    .Append("    T2.CONTACT_NAME AS CONTACT , ")
                    .Append("    TO_NCHAR(T1.CONTACTNO) AS CONTACTNO , ")
                    .Append("    T1.SALESSTARTTIME , ")
                    .Append("    T1.SALESENDTIME , ")
                    .Append("    T1.WALKINNUM , ")
                    .Append("    T3.USERNAME , ")
                    .Append("    T3.OPERATIONCODE , ")
                    .Append("    T4.ICON_IMGFILE , ")
                    .Append("    T2.COUNT_DISP_FLG AS COUNTVIEW , ")
                    .Append("    T1.ACTUALTIME_END AS ACTUALDATE , ")
                    .Append("    T1.UPDATEDATE ")
                    .Append("  FROM ")
                    .Append("    TBL_BOOKEDAFTERFOLLOWRSLT T1 , ")
                    .Append("    TB_M_CONTACT_MTD T2 , ")
                    .Append("    TBL_USERS T3 , ")
                    .Append("    TBL_OPERATIONTYPE T4 ")
                    .Append("  WHERE ")
                    '2014/09/04 TCS 武田 UAT不具合対応(最終活動表示) START
                    .Append("        T1.CONTACTNO = TO_NUMBER(T2.CONTACT_MTD(+)) ")
                    '2014/09/04 TCS 武田 UAT不具合対応(最終活動表示) END
                    .Append("    AND T1.ACTUALACCOUNT = T3.ACCOUNT ")
                    .Append("    AND T3.OPERATIONCODE = T4.OPERATIONCODE ")
                    .Append("    AND T4.DLRCD = T3.DLRCD ")
                    .Append("    AND T4.DLRCD = :DLRCD ")
                    .Append("    AND T4.STRCD = '000' ")
                    .Append("    AND T1.FLLWUPBOX_SEQNO = :FLLWUPBOXSEQNO ")
                    .Append("  ) ")
                End If
            End If

            .Append("ORDER BY ")
            '2014/05/26 TCS 松月 活動回数不具合対応（問連TR-V4-GTMC140512003）START
            .Append("  COUNT DESC , ")
            '2014/05/26 TCS 松月 活動回数不具合対応（問連TR-V4-GTMC140512003）END
            .Append("  ACTUALDATE DESC , ")
            .Append("  UPDATEDATE DESC , ")
            .Append("  SALESENDTIME DESC ")
            .Append("    ) ")
        End With

        Using query As New DBSelectQuery(Of SC3080202DataSet.SC3080202GetFollowupboxDetailDataTable)("SC3080202_102")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)
            query.AddParameterWithTypeValue("FLLWUPBOXSEQNO", OracleDbType.Decimal, fllwupboxseqno)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetFollowupboxDetail_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 徐 2013/10対応版　既存流用 END

            Return query.GetData()
        End Using
    End Function
    '2016/09/12 TCS 鈴木 性能改善（TR-SLT-TMT-20160726-002）MOD END

    '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 活動回数取得
    ''' </summary>
    ''' <param name="fllwupboxseqno">Follow-up Box内連番</param>
    ''' <param name="contactno">実施コンタクト方法</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetCategoryCount(ByVal fllwupboxseqno As String,
                                     ByVal contactno As String) As SC3080202DataSet.SC3080202GetCategoryCountDataTable

        Dim sql As New StringBuilder

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetCategoryCount_Start")
        'ログ出力 End *****************************************************************************

        With sql
            .Append("SELECT ")
            .Append("  /* SC3080202_103 */ ")
            .Append("  SUM(CNT) AS CNT ")
            .Append("FROM( ")
            .Append("  ( ")
            .Append("  SELECT ")
            .Append("    NVL(COUNT(1),0) CNT ")
            .Append("  FROM ")
            .Append("    TB_T_SALES T1 , ")
            .Append("    TB_T_ACTIVITY T2 ")
            .Append("  WHERE ")
            .Append("        T1.REQ_ID = T2.REQ_ID ")
            .Append("    AND T1.ATT_ID = T2.ATT_ID ")
            .Append("    AND T1.SALES_ID = :FLLWUPBOXSEQNO ")
            .Append("    AND T2.RSLT_CONTACT_MTD = :CONTACTNO ")
            .Append("  ) ")
            .Append("UNION ALL ")
            .Append("  ( ")
            .Append("  SELECT ")
            .Append("    NVL(COUNT(1),0) CNT ")
            .Append("  FROM ")
            .Append("    TB_H_SALES T1 , ")
            .Append("    TB_H_ACTIVITY T2 ")
            .Append("  WHERE ")
            .Append("        T1.REQ_ID = T2.REQ_ID ")
            .Append("    AND T1.ATT_ID = T2.ATT_ID ")
            .Append("    AND T1.SALES_ID = :FLLWUPBOXSEQNO ")
            .Append("    AND T2.RSLT_CONTACT_MTD = :CONTACTNO ")
            .Append("  ) ")
            .Append("UNION ALL ")
            .Append("  ( ")
            .Append("  SELECT ")
            .Append("    NVL(COUNT(1),0) CNT ")
            .Append("  FROM ")
            .Append("    TBL_BOOKEDAFTERFOLLOWRSLT ")
            .Append("  WHERE ")
            .Append("      FLLWUPBOX_SEQNO = :FLLWUPBOXSEQNO ")
            .Append("  AND CONTACTNO = :CONTACTNO ")
            .Append("  ) ")
            .Append("    ) ")

        End With

        Using query As New DBSelectQuery(Of SC3080202DataSet.SC3080202GetCategoryCountDataTable)("SC3080202_103")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("FLLWUPBOXSEQNO", OracleDbType.Decimal, fllwupboxseqno)
            query.AddParameterWithTypeValue("CONTACTNO", OracleDbType.NVarchar2, contactno)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetCategoryCount_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 徐 2013/10対応版　既存流用 END

            Return query.GetData()
        End Using
    End Function

    ' 2012/02/29 TCS 小野 【SALES_2】 START
    ' ''' <summary>
    ' ''' 選択希望車種リスト取得
    ' ''' </summary>
    ' ''' <param name="dlrcd">販売店コード</param>
    ' ''' <param name="strcd">店舗コード</param>
    ' ''' <param name="cntcd">国コード</param>
    ' ''' <param name="fllwupboxseqno">Follow-up Box内連番</param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Shared Function GetSelectedSeries(ByVal dlrcd As String,
    '                                  ByVal strcd As String,
    '                                  ByVal cntcd As String,
    '                                  ByVal fllwupboxseqno As Long) As SC3080202DataSet.SC3080202GetSelectedSeriesDataTable

    '    Dim sql As New StringBuilder
    '    With sql
    '        .Append(" SELECT /* SC3080202_004 */ ")
    '        .Append("     J.SERIESCD ")
    '        .Append("     ,J.SERIESNM ")
    '        .Append("     ,J.MODELCD ")
    '        .Append("     ,J.VCLMODEL_NAME ")
    '        .Append("     ,J.COLORCD ")
    '        .Append("     ,K.DISP_BDY_COLOR ")
    '        .Append("     ,L.IMAGEFILE PICIMAGE ")
    '        .Append("     ,M.LOGO_NOTSELECTED LOGOIMAGE ")
    '        .Append("     ,J.QUANTITY ")
    '        .Append("     ,J.SEQNO ")
    '        .Append(" FROM ")
    '        .Append("     (SELECT ")
    '        .Append("         F.SERIESCD ")
    '        .Append("         ,F.MODELCD ")
    '        .Append("         ,F.COLORCD ")
    '        .Append("         ,F.SERIESNM ")
    '        .Append("         ,I.VCLMODEL_NAME ")
    '        .Append("         ,F.QUANTITY ")
    '        .Append("         ,F.SEQNO ")
    '        .Append("     FROM ")
    '        .Append("         (SELECT ")
    '        .Append("             A.SERIESCD SERIESCD ")
    '        .Append("             ,A.MODELCD MODELCD ")
    '        .Append("             ,A.COLORCD COLORCD ")
    '        .Append("             ,E.COMSERIESCD COMSERIESCD ")
    '        .Append("             ,E.SERIESNM SERIESNM ")
    '        .Append("             ,A.QUANTITY QUANTITY ")
    '        .Append("             ,A.SEQNO SEQNO ")
    '        .Append("         FROM ")
    '        .Append("             TBL_FLLWUPBOX_SELECTED_SERIES A ")
    '        .Append("             ,(SELECT ")
    '        .Append("                 C.DLRCD ")
    '        .Append("                 ,B.SERIESCD ")
    '        .Append("                 ,B.SERIESNM ")
    '        .Append("                 ,B.COMSERIESCD ")
    '        .Append("             FROM ")
    '        .Append("                 TBLM_DEALER C ")
    '        .Append("                 ,TBLORG_SERIESMASTER B ")
    '        .Append("             WHERE ")
    '        .Append("                 C.DLRCD = B.DLRCD ")
    '        .Append("                 AND C.DLRCD = :DLRCD ")
    '        .Append("                 AND C.DELFLG = '0' ")
    '        .Append("                 AND C.CNTCD = :CNTCD ")
    '        .Append("                 OR (B.DLRCD = '00000'  ")
    '        .Append("                     AND C.DLRCD = :DLRCD  ")
    '        .Append("                     AND C.DELFLG = '0'  ")
    '        .Append("                     AND C.CNTCD = :CNTCD  ")
    '        .Append("                     AND NOT EXISTS (SELECT 1  ")
    '        .Append("                                     FROM TBLORG_SERIESMASTER D  ")
    '        .Append("                                     WHERE D.DLRCD = C.DLRCD  ")
    '        .Append("                                     AND D.SERIESCD = B.SERIESCD) ")
    '        .Append("                    ) ")
    '        .Append("             ) E ")
    '        .Append("         WHERE ")
    '        .Append("             A.DLRCD = :DLRCD  ")
    '        .Append("             AND A.STRCD = :STRCD  ")
    '        .Append("             AND A.FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO  ")
    '        .Append("             AND A.DLRCD    = E.DLRCD  ")
    '        .Append("             AND A.SERIESCD = E.SERIESCD  ")
    '        .Append("         ) F ")
    '        .Append("         ,(SELECT ")
    '        .Append("             G.CAR_NAME_CD_AI21 ")
    '        .Append("             ,H.VCLMODEL_CODE  ")
    '        .Append("             ,H.VCLMODEL_NAME ")
    '        .Append("         FROM ")
    '        .Append("             TBL_MSTCARNAME   G ")
    '        .Append("             ,TBL_MSTVHCLMODEL H ")
    '        .Append("         WHERE ")
    '        .Append("             G.VCLCLASS_CODE = H.VCLCLASS_CODE  ")
    '        .Append("             AND G.VCLCLASS_GENE = H.VCLCLASS_GENE ")
    '        .Append("         ) I ")
    '        .Append("     WHERE ")
    '        .Append("         F.COMSERIESCD = I.CAR_NAME_CD_AI21(+)   ")
    '        .Append("         AND F.MODELCD = I.VCLMODEL_CODE(+) ")
    '        .Append("     ) J ")
    '        .Append("     ,TBL_MSTEXTERIOR K ")
    '        .Append("     ,TBL_MODELPICTURE L ")
    '        .Append("     ,TBL_MODELLOGO M ")
    '        .Append(" WHERE ")
    '        .Append("     J.MODELCD = K.VCLMODEL_CODE(+)  ")
    '        .Append("     AND J.COLORCD = K.BODYCLR_CD(+) ")
    '        .Append("     AND L.DLRCD(+) = 'XXXXX' ")
    '        .Append("     AND J.SERIESCD = L.SERIESCD(+)  ")
    '        .Append("     AND NVL(J.MODELCD,' ') = L.MODELCD(+) ")
    '        .Append("     AND NVL(J.COLORCD,' ') = L.COLORCD(+) ")
    '        .Append("     AND M.DLRCD(+) = 'XXXXX' ")
    '        .Append("     AND J.SERIESCD = M.SERIESCD(+) ")
    '        .Append(" ORDER BY ")
    '        .Append("     J.SEQNO ")
    '    End With
    '    Using query As New DBSelectQuery(Of SC3080202DataSet.SC3080202GetSelectedSeriesDataTable)("SC3080202_004")
    '        query.CommandText = sql.ToString()
    '        query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)                          '販売店コード
    '        query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strcd)                          '店舗コード
    '        query.AddParameterWithTypeValue("CNTCD", OracleDbType.Char, cntcd)                            '国コード
    '        query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Int64, fllwupboxseqno)     'fllwupbox_seqno

    '        Return query.GetData()
    '    End Using
    'End Function
    ' 2012/02/29 TCS 小野 【SALES_2】 END

    '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 選択競合車種リスト取得
    ''' </summary>
    ''' <param name="fllwupboxseqno">Follow-up Box内連番</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetSelectedCompe(ByVal fllwupboxseqno As String) As SC3080202DataSet.SC3080202GetSelectedCompeDataTable
        Dim sql As New StringBuilder

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSelectedCompe_Start")
        'ログ出力 End *****************************************************************************

        With sql
            .Append("SELECT ")
            .Append("   /* SC3080202_105 */  ")
            .Append("   SERIESCD ,  ")
            .Append("   COMPETITORNM ,  ")
            .Append("   COMPETITIONMAKERNO ,  ")
            .Append("   COMPETITIONMAKER ,  ")
            .Append("   SEQNO  ")
            .Append("FROM ")
            .Append("    (SELECT  ")
            .Append("       T1.MODEL_CD AS SERIESCD ,  ")
            .Append("       T2.MODEL_NAME AS COMPETITORNM ,  ")
            .Append("       T2.MAKER_CD AS COMPETITIONMAKERNO ,  ")
            .Append("       T3.MAKER_NAME AS COMPETITIONMAKER ,  ")
            .Append("       T1.COMP_VCL_SEQ AS SEQNO , ")
            .Append("       T3.SORT_ORDER ,  ")
            .Append("       T2.MODEL_CD ")
            .Append("     FROM  ")
            .Append("       TB_T_COMPETITOR_VCL T1 ,  ")
            .Append("       TB_M_MODEL T2 ,  ")
            .Append("       TB_M_MAKER T3  ")
            .Append("     WHERE  ")
            .Append("           T1.MODEL_CD = T2.MODEL_CD  ")
            .Append("       AND T2.MAKER_CD = T3.MAKER_CD  ")
            .Append("       AND T1.SALES_ID = :FLLWUPBOXSEQNO  ")
            .Append("     UNION ALL  ")
            .Append("     SELECT  ")
            .Append("       T1.MODEL_CD AS SERIESCD ,  ")
            .Append("       T2.MODEL_NAME AS COMPETITORNM ,  ")
            .Append("       T2.MAKER_CD AS COMPETITIONMAKERNO ,  ")
            .Append("       T3.MAKER_NAME AS COMPETITIONMAKER ,  ")
            .Append("       T1.COMP_VCL_SEQ AS SEQNO , ")
            .Append("       T3.SORT_ORDER ,  ")
            .Append("       T2.MODEL_CD ")
            .Append("     FROM  ")
            .Append("       TB_H_COMPETITOR_VCL T1 ,  ")
            .Append("       TB_M_MODEL T2 ,  ")
            .Append("       TB_M_MAKER T3  ")
            .Append("     WHERE  ")
            .Append("          T1.MODEL_CD = T2.MODEL_CD  ")
            .Append("      AND T2.MAKER_CD = T3.MAKER_CD  ")
            .Append("      AND T1.SALES_ID = :FLLWUPBOXSEQNO )  ")
            .Append("ORDER BY ")
            .Append("   SORT_ORDER ,  ")
            .Append("   MODEL_CD ")
        End With

        Using query As New DBSelectQuery(Of SC3080202DataSet.SC3080202GetSelectedCompeDataTable)("SC3080202_105")

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("FLLWUPBOXSEQNO", OracleDbType.Decimal, fllwupboxseqno)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSelectedCompe_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
            Return query.GetData()
        End Using
    End Function

    '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 商談条件リスト取得
    ''' </summary>
    ''' <param name="fllwupboxseqno">Follow-up Box内連番</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetSalesCondition(ByVal fllwupboxseqno As String) As SC3080202DataSet.SC3080202GetSalesConditionDataTable
        Dim sql As New StringBuilder

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSalesCondition_Start")
        'ログ出力 End *****************************************************************************

        With sql
            .Append("SELECT ")
            .Append("  /* SC3080202_006 */ ")
            .Append("  A.SALESCONDITIONNO , ")
            .Append("  A.ITEMNO , ")
            .Append("  A.OTHERSALESCONDITION ")
            .Append("FROM ")
            .Append("  TBL_FLLWUPBOX_SALESCONDITION A ")
            .Append("WHERE ")
            .Append("  A.FLLWUPBOX_SEQNO = :FLLWUPBOXSEQNO ")
            .Append("ORDER BY ")
            .Append("  A.SALESCONDITIONNO, ")
            .Append("  A.ITEMNO ")
        End With

        Using query As New DBSelectQuery(Of SC3080202DataSet.SC3080202GetSalesConditionDataTable)("SC3080202_006")

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("FLLWUPBOXSEQNO", OracleDbType.Decimal, fllwupboxseqno)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSalesCondition_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
            Return query.GetData()
        End Using
    End Function

    '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 商談メモ取得
    ''' </summary>
    ''' <param name="fllwupboxseqno">Follow-up Box内連番</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetSalesMemoToday(ByVal fllwupboxseqno As String) As SC3080202DataSet.SC3080202GetSalesMemoTodayDataTable
        Dim sql As New StringBuilder

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSalesMemoToday_Start")
        'ログ出力 End *****************************************************************************

        With sql
            .Append("SELECT ")
            .Append("  /* SC3080202_007 */ ")
            .Append("  A.MEMO ")
            .Append("FROM ")
            .Append("  TBL_FLLWUPBOX_SALESMEMO_WK A ")
            .Append("WHERE ")
            .Append("  A.FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO ")
        End With

        Using query As New DBSelectQuery(Of SC3080202DataSet.SC3080202GetSalesMemoTodayDataTable)("SC3080202_007")

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupboxseqno)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSalesMemoToday_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
            Return query.GetData()
        End Using
    End Function

    '2014/02/12 TCS 山口 受注後フォロー機能開発 START
    '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 商談メモリスト取得
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="fllwupboxseqno">Follow-up Box内連番</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetSalesMemoHis(ByVal dlrcd As String,
                                    ByVal fllwupboxseqno As String) As SC3080202DataSet.SC3080202GetSalesMemoListDataTable

        Dim sql As New StringBuilder

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSalesMemoHis_Start")
        'ログ出力 End *****************************************************************************

        '2014/09/16 TCS 松月 【A STEP2】SQL性能問題対応（問連TR-V4-GTMC140909002）START
        With sql
            .Append("SELECT /* SC3080202_108 */ ")
            .Append("   T4.INPUTDATE, ")
            .Append("   T4.MEMO, ")
            .Append("   T5.USERNAME, ")
            .Append("   NVL(T6.ICON_IMGFILE, ' ') AS ICON_IMGFILE ")
            .Append(" FROM ( ")
            .Append("    SELECT /*+ INDEX(T2 TB_T_ACTIVITY_IX1) INDEX(T3 TB_T_ACTIVITY_MEMO_IX2) */ ")
            .Append("           T3.CREATE_DATETIME AS INPUTDATE ")
            .Append("          ,T3.CST_MEMO AS MEMO ")
            .Append("          ,T3.ACT_MEMO_ID AS ACT_MEMO_ID ")
            .Append("          ,T3.CREATE_STF_CD AS CREATE_STF_CD ")
            .Append("      FROM TB_T_ACTIVITY T2, ")
            .Append("           TB_T_ACTIVITY_MEMO T3 ")
            .Append("     WHERE T2.REQ_ID = (SELECT /*+ INDEX(T1 TB_T_SALES_PK) */ ")
            .Append("                               T1.REQ_ID ")
            .Append("                          FROM TB_T_SALES T1 ")
            .Append("                         WHERE T1.SALES_ID = :FLLWUPBOXSEQNO ")
            .Append("                           AND T1.REQ_ID <> 0) ")
            .Append("       AND T2.ACT_ID = T3.RELATION_ACT_ID ")
            .Append("    UNION ALL ")
            .Append("    SELECT /*+ INDEX(T2 TB_T_ACTIVITY_IX3) INDEX(T3 TB_T_ACTIVITY_MEMO_IX2) */ ")
            .Append("           T3.CREATE_DATETIME AS INPUTDATE ")
            .Append("          ,T3.CST_MEMO AS MEMO ")
            .Append("          ,T3.ACT_MEMO_ID AS ACT_MEMO_ID ")
            .Append("          ,T3.CREATE_STF_CD AS CREATE_STF_CD ")
            .Append("      FROM TB_T_ACTIVITY T2, ")
            .Append("           TB_T_ACTIVITY_MEMO T3 ")
            .Append("     WHERE T2.ATT_ID = (SELECT /*+ INDEX(T1 TB_T_SALES_PK) */ ")
            .Append("                               T1.ATT_ID ")
            .Append("                          FROM TB_T_SALES T1 ")
            .Append("                         WHERE T1.SALES_ID = :FLLWUPBOXSEQNO ")
            .Append("                           AND T1.ATT_ID <> 0) ")
            .Append("       AND T2.ACT_ID = T3.RELATION_ACT_ID ")
            .Append("    UNION ALL ")
            .Append("    SELECT /*+ INDEX(T2 TB_H_ACTIVITY_IX1) INDEX(T3 TB_H_ACTIVITY_MEMO_IX2) */ ")
            .Append("           T3.CREATE_DATETIME AS INPUTDATE ")
            .Append("          ,T3.CST_MEMO AS MEMO ")
            .Append("          ,T3.ACT_MEMO_ID AS ACT_MEMO_ID ")
            .Append("          ,T3.CREATE_STF_CD AS CREATE_STF_CD ")
            .Append("      FROM TB_H_ACTIVITY T2, ")
            .Append("           TB_H_ACTIVITY_MEMO T3 ")
            .Append("     WHERE T2.REQ_ID = (SELECT /*+ INDEX(T1 TB_H_SALES_PK) */ ")
            .Append("                               T1.REQ_ID ")
            .Append("                          FROM TB_H_SALES T1 ")
            .Append("                         WHERE T1.SALES_ID = :FLLWUPBOXSEQNO ")
            .Append("                           AND T1.REQ_ID <> 0) ")
            .Append("       AND T2.ACT_ID = T3.RELATION_ACT_ID ")
            .Append("    UNION ALL ")
            .Append("    SELECT /*+ INDEX(T2 TB_H_ACTIVITY_IX3) INDEX(T3 TB_H_ACTIVITY_MEMO_IX2) */ ")
            .Append("           T3.CREATE_DATETIME AS INPUTDATE ")
            .Append("          ,T3.CST_MEMO AS MEMO ")
            .Append("          ,T3.ACT_MEMO_ID AS ACT_MEMO_ID ")
            .Append("          ,T3.CREATE_STF_CD AS CREATE_STF_CD ")
            .Append("      FROM TB_H_ACTIVITY T2, ")
            .Append("           TB_H_ACTIVITY_MEMO T3 ")
            .Append("     WHERE T2.ATT_ID = (SELECT /*+ INDEX(T1 TB_H_SALES_PK) */ ")
            .Append("                               T1.ATT_ID ")
            .Append("                          FROM TB_H_SALES T1 ")
            .Append("                         WHERE T1.SALES_ID = :FLLWUPBOXSEQNO ")
            .Append("                           AND T1.ATT_ID <> 0) ")
            .Append("       AND T2.ACT_ID = T3.RELATION_ACT_ID ")
            .Append("  ) T4, ")
            .Append("  TBL_USERS T5, ")
            .Append("  TBL_OPERATIONTYPE T6 ")
            .Append(" WHERE ")
            .Append("      T5.ACCOUNT = T4.CREATE_STF_CD ")
            .Append("  AND T5.OPERATIONCODE = T6.OPERATIONCODE ")
            .Append("  AND T5.DLRCD = T6.DLRCD ")
            .Append("  AND T6.STRCD = :STRCD ")
            .Append(" ORDER BY ")
            .Append("   INPUTDATE DESC, ")
            .Append("   ACT_MEMO_ID DESC ")
        End With

        Using query As New DBSelectQuery(Of SC3080202DataSet.SC3080202GetSalesMemoListDataTable)("SC3080202_108")

            query.CommandText = sql.ToString()
            '2014/09/16 TCS 松月 【A STEP2】SQL性能問題対応（問連TR-V4-GTMC140909002）END
            query.AddParameterWithTypeValue("FLLWUPBOXSEQNO", OracleDbType.Decimal, fllwupboxseqno)
            query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, "000")
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSalesMemoHis_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
            Return query.GetData()
        End Using
    End Function
    '2014/02/12 TCS 山口 受注後フォロー機能開発 END

    ' 2012/02/29 TCS 小野 【SALES_2】 START
    ' ''' <summary>
    ' ''' プロセス取得
    ' ''' </summary>
    ' ''' <param name="dlrcd">販売店コード</param>
    ' ''' <param name="strcd">店舗コード</param>
    ' ''' <param name="fllwupboxseqno">Follow-up Box内連番</param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Shared Function GetProcess(ByVal dlrcd As String,
    '                                  ByVal strcd As String,
    '                                  ByVal fllwupboxseqno As Long) As SC3080202DataSet.SC3080202GetProcessDataTable

    '    Dim sql As New StringBuilder
    '    With sql
    '        .Append(" SELECT /* SC3080202_009 */ ")
    '        .Append("     SEQNO SEQNO ")
    '        .Append("     ,CTNTSEQNO CTNTSEQNO ")
    '        .Append("     ,ACTIONCD ACTIONCD ")
    '        .Append("     ,MAX(LASTACTDATE) LASTACTDATE ")
    '        .Append(" FROM ")
    '        .Append(" ( ")
    '        .Append("     (SELECT ")
    '        .Append("          A.SELECT_SERIES_SEQNO SEQNO ")
    '        .Append("         ,A.CTNTSEQNO CTNTSEQNO ")
    '        .Append("         ,A.ACTDATE LASTACTDATE ")
    '        .Append("         ,A.ACTIONCD ACTIONCD ")
    '        .Append("     FROM ")
    '        .Append("         TBL_FLLWUPBOXCRHIS A ")
    '        .Append("     WHERE ")
    '        .Append("         A.DLRCD = :DLRCD  ")
    '        .Append("         AND A.STRCD = :STRCD  ")
    '        .Append("         AND A.FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO  ")
    '        .Append("         AND A.ACTIONCD IN ('A22','A26','A30','A23') ")
    '        .Append("         AND A.SELECT_SERIES_SEQNO IS NOT NULL ")
    '        .Append("     ) ")
    '        .Append(" UNION ALL ")
    '        .Append("     (SELECT ")
    '        .Append("          A.SELECT_SERIES_SEQNO SEQNO ")
    '        .Append("         ,A.CTNTSEQNO CTNTSEQNO ")
    '        .Append("         ,A.ACTDATE LASTACTDATE ")
    '        .Append("         ,A.ACTIONCD ACTIONCD ")
    '        .Append("     FROM ")
    '        .Append("         TBL_FLLWUPBOXCRHIS_PAST A ")
    '        .Append("     WHERE ")
    '        .Append("         A.DLRCD = :DLRCD  ")
    '        .Append("         AND A.STRCD = :STRCD  ")
    '        .Append("         AND A.FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO  ")
    '        .Append("         AND A.ACTIONCD IN ('A22','A26','A30','A23') ")
    '        .Append("         AND A.SELECT_SERIES_SEQNO IS NOT NULL ")
    '        .Append("     ) ")
    '        .Append(" ) ")
    '        .Append(" GROUP BY  ")
    '        .Append("     SEQNO ")
    '        .Append("     ,CTNTSEQNO ")
    '        .Append("     ,ACTIONCD ")
    '        .Append(" ORDER BY  ")
    '        .Append("     SEQNO ")
    '        .Append("     ,CTNTSEQNO ")
    '        .Append("     ,ACTIONCD ")
    '    End With
    '    Using query As New DBSelectQuery(Of SC3080202DataSet.SC3080202GetProcessDataTable)("SC3080202_009")

    '        query.CommandText = sql.ToString()
    '        query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)                          '販売店コード
    '        query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strcd)                          '店舗コード
    '        query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Int64, fllwupboxseqno)     'fllwupbox_seqno

    '        Return query.GetData()
    '    End Using
    'End Function
    ' 2012/02/29 TCS 小野 【SALES_2】 END

    '2014/02/12 TCS 山口 受注後フォロー機能開発 START
    ''' <summary>
    ''' プロセスアイコン取得
    ''' </summary>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Public Shared Function GetProcessIcons() As SC3080202DataSet.SC3080202GetFllwupboxContentDataTable
        Dim sql As New StringBuilder

        With sql
            .Append(" SELECT /* SC3080202_031 */ ")
            .Append("        SEQNO AS CTNTSEQNO")
            .Append("      , ICONPATH_SALES_NOTSELECTED")
            .Append("      , ICONPATH_SALES_SELECTED")
            .Append("   FROM TBL_FLLWUPBOXCONTENT")
            .Append("  WHERE SEQNO IN(9,10,16,18)")
            .Append("    AND DELFLG = '0'")
        End With

        Using query As New DBSelectQuery(Of SC3080202DataSet.SC3080202GetFllwupboxContentDataTable)("SC3080202_031")
            query.CommandText = sql.ToString()
            Return query.GetData()
        End Using

    End Function
    '2014/02/12 TCS 山口 受注後フォロー機能開発 END

    ' 2012/02/29 TCS 小野 【SALES_2】 START
    ' ''' <summary>
    ' ''' ステータス取得
    ' ''' </summary>
    ' ''' <param name="dlrcd">販売店コード</param>
    ' ''' <param name="strcd">店舗コード</param>
    ' ''' <param name="fllwupboxseqno">Follow-up Box内連番</param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Shared Function GetStatus(ByVal dlrcd As String,
    '                          ByVal strcd As String,
    '                          ByVal fllwupboxseqno As Long) As SC3080202DataSet.SC3080202GetStatusDataTable

    '    Dim sql As New StringBuilder
    '    With sql
    '        .Append(" SELECT /* SC3080202_010 */ ")
    '        .Append("     CRACTRESULT ")
    '        .Append(" FROM ")
    '        .Append(" ( ")
    '        .Append("     (SELECT ")
    '        .Append("         A.CRACTRESULT CRACTRESULT ")
    '        .Append("     FROM ")
    '        .Append("         TBL_FLLWUPBOX A ")
    '        .Append("     WHERE ")
    '        .Append("         A.DLRCD = :DLRCD  ")
    '        .Append("         AND A.STRCD = :STRCD  ")
    '        .Append("         AND A.FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO ")
    '        .Append("     ) ")
    '        .Append(" UNION ALL ")
    '        .Append("     (SELECT ")
    '        .Append("         A.CRACTRESULT CRACTRESULT ")
    '        .Append("     FROM ")
    '        .Append("         TBL_FLLWUPBOX_PAST A ")
    '        .Append("     WHERE ")
    '        .Append("         A.DLRCD = :DLRCD  ")
    '        .Append("         AND A.STRCD = :STRCD  ")
    '        .Append("         AND A.FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO ")
    '        .Append("     ) ")
    '        .Append(" ) ")
    '    End With
    '    Using query As New DBSelectQuery(Of SC3080202DataSet.SC3080202GetStatusDataTable)("SC3080202_010")

    '        query.CommandText = sql.ToString()
    '        query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)                          '販売店コード
    '        query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strcd)                          '店舗コード
    '        query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Int64, fllwupboxseqno)      'fllwupbox_seqno

    '        Return query.GetData()
    '    End Using
    'End Function
    ' 2012/02/29 TCS 小野 【SALES_2】 END

    '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 希望車種車種マスタ取得
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetSelectedSeriesMaster(ByVal dlrcd As String) As SC3080202DataSet.SC3080202GetSeriesMasterDataTable

        Dim sql As New StringBuilder

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSelectedSeriesMaster_Start")
        'ログ出力 End *****************************************************************************

        With sql
            '2014/02/02 TCS 松月 希望車表示不具合対応（号口切替BTS-39）START
            .Append("SELECT DISTINCT  ")
            .Append("    /* SC3080202_111 */ ")
            .Append("    MODEL_CD AS SERIESCD, ")
            .Append("    MODEL_NAME AS SERIESNM ")
            .Append("FROM ")
            .Append("    (SELECT ")
            .Append("        T1.SORT_ORDER, ")
            .Append("        T1.MODEL_CD, ")
            .Append("        T2.MODEL_NAME ")
            .Append("    FROM ")
            .Append("        TB_M_MODEL_DLR T1, ")
            .Append("        TB_M_MODEL T2, ")
            .Append("        TB_M_MAKER T3 ")
            .Append("    WHERE ")
            .Append("        (T1.DLR_CD = 'XXXXX' OR T1.DLR_CD = :DLRCD) AND ")
            '2014/02/02 TCS 松月 希望車表示不具合対応（号口切替BTS-39）END
            '2014/07/24 TCS 外崎 不具合対応（TMT 切替BTS-102）START
            .Append("        (T1.SALES_FROM_DATE = TO_DATE('1900/1/1', 'YYYY/MM/DD HH24:MI:SS') OR SALES_FROM_DATE <= TRUNC(SYSDATE)) AND ")
            .Append("        (T1.SALES_TO_DATE   = TO_DATE('1900/1/1', 'YYYY/MM/DD HH24:MI:SS') OR SALES_TO_DATE   >= TRUNC(SYSDATE)) AND ")
            '2014/07/24 TCS 外崎 不具合対応（TMT 切替BTS-102）END
            .Append("        T1.MODEL_CD = T2.MODEL_CD AND ")
            .Append("        T2.MAKER_CD = T3.MAKER_CD AND ")
            .Append("        T2.INUSE_FLG = '1' AND ")
            .Append("        T3.MAKER_TYPE = '1' ")
            .Append("    ORDER BY ")
            .Append("        T1.SORT_ORDER, ")
            .Append("        T1.MODEL_CD) ")
        End With

        Using query As New DBSelectQuery(Of SC3080202DataSet.SC3080202GetSeriesMasterDataTable)("SC3080202_111")

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSelectedSeriesMaster_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 徐 2013/10対応版　既存流用 END

            Return query.GetData()
        End Using
    End Function

    '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 希望車種グレードマスタ取得
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetSelectedGradeMaster(ByVal dlrcd As String) As SC3080202DataSet.SC3080202GetModelMasterDataTable
        Dim sql As New StringBuilder

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSelectedGradeMaster_Start")
        'ログ出力 End *****************************************************************************

        With sql
            '2014/02/02 TCS 松月 希望車表示不具合対応（号口切替BTS-39）START
            .Append("SELECT DISTINCT  ")
            .Append("    /* SC3080202_112 */ ")
            .Append("    MODEL_CD AS SERIESCD, ")
            .Append("    GRADE_CD AS VCLMODEL_CODE, ")
            .Append("    GRADE_NAME AS VCLMODEL_NAME ")
            .Append("FROM ")
            .Append("    (SELECT ")
            .Append("        T1.SORT_ORDER, ")
            .Append("        T1.MODEL_CD, ")
            .Append("        T4.GRADE_CD, ")
            .Append("        T4.GRADE_NAME ")
            .Append("    FROM ")
            .Append("        TB_M_MODEL_DLR T1, ")
            .Append("        TB_M_MODEL T2, ")
            .Append("        TB_M_MAKER T3, ")
            .Append("        TB_M_GRADE T4 ")
            .Append("    WHERE ")
            .Append("        (T1.DLR_CD = 'XXXXX' OR T1.DLR_CD = :DLRCD) AND ")
            '2014/07/24 TCS 外崎 不具合対応（TMT 切替BTS-102）START
            .Append("        (T1.SALES_FROM_DATE = TO_DATE('1900/1/1', 'YYYY/MM/DD HH24:MI:SS') OR SALES_FROM_DATE <= TRUNC(SYSDATE)) AND ")
            .Append("        (T1.SALES_TO_DATE   = TO_DATE('1900/1/1', 'YYYY/MM/DD HH24:MI:SS') OR SALES_TO_DATE   >= TRUNC(SYSDATE)) AND ")
            '2014/07/24 TCS 外崎 不具合対応（TMT 切替BTS-102）END
            .Append("        T1.MODEL_CD = T2.MODEL_CD AND ")
            .Append("        T2.MAKER_CD = T3.MAKER_CD AND ")
            .Append("        T2.INUSE_FLG = '1' AND ")
            .Append("        T4.INUSE_FLG = '1' AND ")
            '2014/02/02 TCS 松月 希望車表示不具合対応（号口切替BTS-39）END
            .Append("        T3.MAKER_TYPE = '1' AND ")
            .Append("        T1.MODEL_CD = T4.MODEL_CD ")
            .Append("    ORDER BY ")
            .Append("        T1.SORT_ORDER, ")
            .Append("        T1.MODEL_CD, ")
            .Append("        T4.GRADE_CD) ")
        End With

        Using query As New DBSelectQuery(Of SC3080202DataSet.SC3080202GetModelMasterDataTable)("SC3080202_112")

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSelectedGradeMaster_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 徐 2013/10対応版　既存流用 END

            Return query.GetData()
        End Using
    End Function

    '2013/06/30 TCS 徐 2013/10対応版　既存流用 START

    '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 競合車種メーカマスタ取得
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetSelectedCompeMakerMaster(ByVal dlrcd As String) As SC3080202DataSet.SC3080202GetCompeMakerMasterDataTable

        Dim sql As New StringBuilder

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSelectedCompeMakerMaster_Start")
        'ログ出力 End *****************************************************************************

        With sql
            .Append("SELECT ")
            .Append("    /* SC3080202_114 */ ")
            .Append("  T1.MAKER_CD AS COMPETITIONMAKERNO , ")
            .Append("  T1.MAKER_NAME AS COMPETITIONMAKER ")
            .Append("FROM ")
            .Append("  TB_M_MAKER T1 ")
            .Append("WHERE ")
            .Append("  EXISTS ( ")
            .Append("    SELECT ")
            .Append("      1 ")
            .Append("    FROM ")
            .Append("      TB_M_MODEL T2, ")
            .Append("      TB_M_MODEL_COMPETITOR_DLR T3 ")
            .Append("    WHERE ")
            .Append("          T2.MODEL_CD = T3.COMP_MODEL_CD ")
            .Append("      AND T1.MAKER_CD = T2.MAKER_CD ")
            .Append("      AND T2.INUSE_FLG = '1' ")
            .Append("      AND (T3.DLR_CD = :DLRCD ")
            .Append("      OR T3.DLR_CD = 'XXXXX') ")
            .Append("         ) ")
            .Append("ORDER BY ")
            .Append("  T1.SORT_ORDER ")
        End With

        Using query As New DBSelectQuery(Of SC3080202DataSet.SC3080202GetCompeMakerMasterDataTable)("SC3080202_114")

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSelectedCompeMakerMaster_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 徐 2013/10対応版　既存流用 END

            Return query.GetData()
        End Using
    End Function

    '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 競合車種モデルマスタ取得
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetSelectedCompeModelMaster(ByVal dlrcd As String) As SC3080202DataSet.SC3080202GetCompeModelMasterDataTable

        Dim sql As New StringBuilder

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSelectedCompeModelMaster_Start")
        'ログ出力 End *****************************************************************************

        With sql
            .Append("SELECT DISTINCT ")
            .Append("  /* SC3080202_115 */ ")
            .Append("  T1.MODEL_CD AS COMPETITORCD , ")
            .Append("  T1.MODEL_NAME AS COMPETITORNM , ")
            .Append("  T1.MAKER_CD AS COMPETITIONMAKERNO ")
            .Append("FROM ")
            .Append("  TB_M_MODEL T1, ")
            .Append("  TB_M_MAKER T2, ")
            .Append("  TB_M_MODEL_COMPETITOR_DLR T3 ")
            .Append("WHERE ")
            .Append("      T1.MAKER_CD = T2.MAKER_CD ")
            .Append("  AND T1.MODEL_CD = T3.COMP_MODEL_CD ")
            .Append("  AND T1.INUSE_FLG = '1' ")
            .Append("  AND (T3.DLR_CD = :DLRCD ")
            .Append("      OR T3.DLR_CD = 'XXXXX') ")
            .Append("ORDER BY ")
            .Append("  T1.MODEL_CD ")
        End With

        Using query As New DBSelectQuery(Of SC3080202DataSet.SC3080202GetCompeModelMasterDataTable)("SC3080202_115")

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSelectedCompeModelMaster_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 徐 2013/10対応版　既存流用 END

            Return query.GetData()
        End Using
    End Function

    ''' <summary>
    ''' 商談条件マスタ取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetSalesConditionMaster() As SC3080202DataSet.SC3080202GetSalesConditionMasterDataTable

        Dim sql As New StringBuilder
        With sql
            .Append(" SELECT /* SC3080202_016 */ ")
            .Append("     A.SALESCONDITIONNO SALESCONDITIONNO, ")
            .Append("     A.TITLE TITLE, ")
            .Append("     A.AND_OR AND_OR, ")
            .Append("     B.ITEMNO ITEMNO, ")
            .Append("     B.TITLE ITEMTITLE, ")
            '2013/12/05 TCS 市川 Aカード情報相互連携開発 START
            .Append("     B.OTHER OTHER, ")
            .Append("     CASE C.DISP_SETTING_STATUS WHEN n'2' THEN 'True' ELSE 'False' END IS_MANDATORY ")
            .Append(" FROM ")
            .Append("     TBL_SALESCONDITION A , ")
            .Append("     TBL_SALESCONDITIONITEM B, ")
            .Append("     TBL_INPUT_ITEM_SETTING C ")
            .Append(" WHERE ")
            .Append("     A.SALESCONDITIONNO = B.SALESCONDITIONNO  ")
            .Append("     AND CAST(A.SALESCONDITIONNO AS nvarchar2(10)) = C.TGT_ITEM_DETAIL_ID ")
            .Append("     AND A.DELFLG = '0' ")
            .Append("     AND B.DELFLG = '0' ")
            .Append("     AND C.CHECK_TIMING_TYPE = '02' ")
            '2013/12/05 TCS 市川 Aカード情報相互連携開発 END
            .Append(" ORDER BY ")
            .Append("     A.SORT ASC ")
            .Append("    ,B.SORT ASC")
        End With
        Using query As New DBSelectQuery(Of SC3080202DataSet.SC3080202GetSalesConditionMasterDataTable)("SC3080202_016")

            query.CommandText = sql.ToString()

            Return query.GetData()
        End Using
    End Function

    '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
    ''' <summary>
    ''' Follow-up Box商談条件追加
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="strcd">店舗コード</param>
    ''' <param name="fllwupboxseqno">Follow-up Box内連番</param>
    ''' <param name="salesconditionno">商談条件No</param>
    ''' <param name="itemno">項目No</param>
    ''' <param name="othersalescondition">その他</param>
    ''' <param name="cstkind">顧客種別</param>
    ''' <param name="cstclass">顧客分類</param>
    ''' <param name="custcd">活動先顧客コード</param>
    ''' <param name="account">更新ユーザアカウント</param>
    ''' <param name="id">機能ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function AddFollowupboxSalesCondition(ByVal dlrcd As String,
                                                 ByVal strcd As String,
                                                 ByVal fllwupboxseqno As Decimal,
                                                 ByVal salesconditionno As Long,
                                                 ByVal itemno As Long,
                                                 ByVal othersalescondition As String,
                                                 ByVal cstkind As String,
                                                 ByVal cstclass As String,
                                                 ByVal custcd As String,
                                                 ByVal account As String,
                                                 ByVal id As String) As Integer

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("AddFollowupboxSalesCondition_Start")
        'ログ出力 End *****************************************************************************
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 END

        Dim sql As New StringBuilder
        With sql
            .Append(" INSERT /* SC3080202_017 */ INTO ")
            .Append("     TBL_FLLWUPBOX_SALESCONDITION ")
            .Append(" ( ")
            .Append("     DLRCD, ")
            .Append("     STRCD, ")
            .Append("     FLLWUPBOX_SEQNO, ")
            .Append("     SALESCONDITIONNO, ")
            .Append("     ITEMNO, ")
            .Append("     OTHERSALESCONDITION, ")
            .Append("     CSTKIND, ")
            .Append("     CUSTOMERCLASS, ")
            .Append("     CRCUSTID, ")
            .Append("     CREATEDATE, ")
            .Append("     UPDATEDATE, ")
            .Append("     CREATEACCOUNT, ")
            .Append("     UPDATEACCOUNT, ")
            .Append("     CREATEID, ")
            .Append("     UPDATEID ")
            .Append(" ) ")
            .Append(" VALUES ")
            .Append(" ( ")
            .Append("     :DLRCD, ")
            .Append("     :STRCD, ")
            .Append("     :FLLWUPBOX_SEQNO, ")
            .Append("     :SALESCONDITIONNO, ")
            .Append("     :ITEMNO, ")
            .Append("     :OTHERSALESCONDITION, ")
            .Append("     :CSTKIND, ")
            .Append("     :CSTCLASS, ")
            .Append("     :CUSTCD, ")
            .Append("     sysdate, ")
            .Append("     sysdate, ")
            .Append("     :ACCOUNT, ")
            .Append("     :ACCOUNT, ")
            .Append("     :ID, ")
            .Append("     :ID ")
            .Append(" ) ")
        End With
        Using query As New DBUpdateQuery("SC3080202_017")

            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)                          '販売店コード
            query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strcd)                          '店舗コード
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupboxseqno)      'fllwupbox_seqno
            query.AddParameterWithTypeValue("SALESCONDITIONNO", OracleDbType.Int64, salesconditionno)   '商談条件No
            query.AddParameterWithTypeValue("ITEMNO", OracleDbType.Int64, itemno)                       '項目No
            query.AddParameterWithTypeValue("OTHERSALESCONDITION", OracleDbType.NVarchar2, othersalescondition)                     'その他
            query.AddParameterWithTypeValue("CSTKIND", OracleDbType.Char, cstkind)                      '顧客種別(1:自社客  2:未取引客)
            query.AddParameterWithTypeValue("CSTCLASS", OracleDbType.Char, cstclass)                    '顧客分類(1:所有者、2:使用者、3:その他)
            query.AddParameterWithTypeValue("CUSTCD", OracleDbType.Char, custcd)                        '顧客コード
            query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.Varchar2, account)                  'アカウント
            query.AddParameterWithTypeValue("ID", OracleDbType.Varchar2, id)                            'ID

            '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("AddFollowupboxSalesCondition_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
            Return query.Execute()
        End Using
    End Function

    '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
    ''' <summary>
    ''' Follow-up Box商談条件削除
    ''' </summary>
    ''' <param name="fllwupboxseqno">Follow-up Box内連番</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function DeleteFollowupboxSalesCondition(ByVal fllwupboxseqno As String) As Integer

        Dim sql As New StringBuilder

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeleteFollowupboxSalesCondition_Start")
        'ログ出力 End *****************************************************************************

        With sql
            .Append("DELETE ")
            .Append("    /* SC3080202_018 */ ")
            .Append("FROM ")
            .Append("    TBL_FLLWUPBOX_SALESCONDITION ")
            .Append("WHERE ")
            .Append("    FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO ")
        End With

        Using query As New DBUpdateQuery("SC3080202_018")

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupboxseqno)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeleteFollowupboxSalesCondition_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
            Return query.Execute()
        End Using
    End Function

    '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
    ''' <summary>
    ''' Follow-up Box商談メモ編集
    ''' </summary>
    ''' <param name="fllwupboxseqno">Follow-up Box内連番</param>
    ''' <param name="memo">メモ</param>
    ''' <param name="account">更新ユーザアカウント</param>
    ''' <param name="id">機能ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function UpdateFollowupboxSalesMemo(ByVal fllwupboxseqno As String,
                                               ByVal memo As String,
                                               ByVal account As String,
                                               ByVal id As String) As Integer
        Dim sql As New StringBuilder

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateFollowupboxSalesMemo_Start")
        'ログ出力 End *****************************************************************************

        With sql
            .Append("UPDATE ")
            .Append("    /* SC3080202_019 */ ")
            .Append("    TBL_FLLWUPBOX_SALESMEMO_WK ")
            .Append("SET ")
            .Append("    MEMO = :MEMO, ")
            .Append("    UPDATEDATE = SYSDATE, ")
            .Append("    UPDATEACCOUNT = :ACCOUNT, ")
            .Append("    UPDATEID = :ID ")
            .Append("WHERE ")
            .Append("    FLLWUPBOX_SEQNO = :FLLWUPBOXSEQNO ")
        End With

        Using query As New DBUpdateQuery("SC3080202_019")

            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("FLLWUPBOXSEQNO", OracleDbType.Decimal, fllwupboxseqno)
            query.AddParameterWithTypeValue("MEMO", OracleDbType.NVarchar2, memo)
            query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, account)
            query.AddParameterWithTypeValue("ID", OracleDbType.NVarchar2, id)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateFollowupboxSalesMemo_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 徐 2013/10対応版　既存流用 END

            Return query.Execute()
        End Using
    End Function

    '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
    ''' <summary>
    ''' Follow-up Box商談メモ追加
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="strcd">店舗コード</param>
    ''' <param name="fllwupboxseqno">Follow-up Box内連番</param>
    ''' <param name="memo">メモ</param>
    ''' <param name="account">更新ユーザアカウント</param>
    ''' <param name="id">機能ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function AddFollowupboxSalesMemo(ByVal dlrcd As String,
                                            ByVal strcd As String,
                                            ByVal fllwupboxseqno As Decimal,
                                            ByVal memo As String,
                                            ByVal account As String,
                                            ByVal id As String) As Integer

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("AddFollowupboxSalesMemo_Start")
        'ログ出力 End *****************************************************************************
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
        Dim sql As New StringBuilder
        With sql
            .Append(" INSERT /* SC3080202_020 */ INTO ")
            .Append("     TBL_FLLWUPBOX_SALESMEMO_WK ")
            .Append(" ( ")
            .Append("     DLRCD, ")
            .Append("     STRCD, ")
            .Append("     FLLWUPBOX_SEQNO, ")
            .Append("     MEMO, ")
            .Append("     CREATEDATE, ")
            .Append("     UPDATEDATE, ")
            .Append("     CREATEACCOUNT, ")
            .Append("     UPDATEACCOUNT, ")
            .Append("     CREATEID, ")
            .Append("     UPDATEID ")
            .Append(" ) ")
            .Append(" VALUES ")
            .Append(" ( ")
            .Append("     :DLRCD, ")
            .Append("     :STRCD, ")
            .Append("     :FLLWUPBOX_SEQNO, ")
            .Append("     :MEMO, ")
            .Append("     SYSDATE, ")
            .Append("     SYSDATE, ")
            .Append("     :ACCOUNT, ")
            .Append("     :ACCOUNT, ")
            .Append("     :ID, ")
            .Append("     :ID ")
            .Append(" ) ")
        End With
        Using query As New DBUpdateQuery("SC3080202_020")

            query.CommandText = sql.ToString()

            '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)
            query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strcd)
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupboxseqno)
            query.AddParameterWithTypeValue("MEMO", OracleDbType.NVarchar2, memo)
            query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.Varchar2, account)
            query.AddParameterWithTypeValue("ID", OracleDbType.Varchar2, id)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("AddFollowupboxSalesMemo_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
            Return query.Execute()
        End Using
    End Function

    '2017/11/20 TCS 河原 TKM独自機能開発 START
    '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 選択車種追加
    ''' </summary>
    ''' <param name="fllwupboxseqno">商談ID</param>
    ''' <param name="seqno">希望車連番</param>
    ''' <param name="seriescd">モデルコード</param>
    ''' <param name="modelcd">グレードコード</param>
    ''' <param name="colorcd">外鈑色コード </param>
    ''' <param name="account">作成アカウント</param>
    ''' <param name="mostPerfCd">商談見込み度コード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function AddSelectedSeries(ByVal fllwupboxseqno As String,
                                      ByVal seqno As String,
                                      ByVal modelcd As String,
                                      ByVal gradecd As String,
                                      ByVal suffixcd As String,
                                      ByVal exteriorColorcd As String,
                                      ByVal interiorColorcd As String,
                                      ByVal account As String,
                                      ByVal mostPerfCd As String) As Integer
        Dim sql As New StringBuilder

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("AddSelectedSeries_Start")
        'ログ出力 End *****************************************************************************

        With sql
            .Append("INSERT ")
            .Append("    /* SC3080202_121 */ ")
            .Append("INTO TB_T_PREFER_VCL ( ")
            .Append("    SALES_ID, ")
            .Append("    PREF_VCL_SEQ, ")
            .Append("    SALES_STATUS, ")
            .Append("    MODEL_CD, ")
            .Append("    GRADE_CD, ")
            .Append("    SUFFIX_CD, ")
            .Append("    BODYCLR_CD, ")
            .Append("    INTERIORCLR_CD, ")
            .Append("    PREF_AMOUNT, ")
            .Append("    EST_RSLT_CONTACT_MTD, ")
            .Append("    EST_AMOUNT, ")
            .Append("    EST_RSLT_FLG, ")
            .Append("    EST_RSLT_STF_CD, ")
            .Append("    EST_RSLT_DEPT_ID, ")
            .Append("    SALESBKG_ACT_ID, ")
            .Append("    SALESBKG_NUM, ")
            .Append("    SALES_PROSPECT_CD, ")
            .Append("    ROW_CREATE_DATETIME, ")
            .Append("    ROW_CREATE_ACCOUNT, ")
            .Append("    ROW_CREATE_FUNCTION, ")
            .Append("    ROW_UPDATE_DATETIME, ")
            .Append("    ROW_UPDATE_ACCOUNT, ")
            .Append("    ROW_UPDATE_FUNCTION, ")
            .Append("    ROW_LOCK_VERSION ")
            .Append(") ")
            .Append("VALUES ( ")
            .Append("    :FLLWUPBOXSEQNO, ")
            .Append("    :SEQNO, ")
            .Append("    '21', ")
            .Append("    :MODEL_CD, ")
            If String.IsNullOrEmpty(Trim(gradecd)) Then
                .Append("     ' ', ")
            Else
                .Append("    :GRADE_CD, ")
            End If
            If String.IsNullOrEmpty(Trim(suffixcd)) Then
                .Append("     ' ', ")
            Else
                .Append("    :SUFFIX_CD, ")
            End If
            If String.IsNullOrEmpty(Trim(exteriorColorcd)) Then
                .Append("     ' ', ")
            Else
                .Append("    :BODYCLR_CD, ")
            End If
            If String.IsNullOrEmpty(Trim(interiorColorcd)) Then
                .Append("     ' ', ")
            Else
                .Append("    :INTERIORCLR_CD, ")
            End If
            .Append("    1, ")
            .Append("    '0', ")
            .Append("    0, ")
            .Append("    '0', ")
            .Append("    ' ', ")
            .Append("    0, ")
            .Append("    0, ")
            .Append("    ' ', ")
            .Append("    :SALES_PROSPECT_CD, ")
            .Append("    SYSDATE, ")
            .Append("    :ACCOUNT, ")
            .Append("    'SC3080202', ")
            .Append("    SYSDATE, ")
            .Append("    :ACCOUNT, ")
            .Append("    'SC3080202', ")
            .Append("0 ")
            .Append(") ")

        End With
        Using query As New DBUpdateQuery("SC3080202_121")

            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("FLLWUPBOXSEQNO", OracleDbType.Decimal, fllwupboxseqno)
            query.AddParameterWithTypeValue("SEQNO", OracleDbType.Decimal, seqno)
            query.AddParameterWithTypeValue("MODEL_CD", OracleDbType.NVarchar2, modelcd)
            If Not String.IsNullOrEmpty(Trim(gradecd)) Then
                query.AddParameterWithTypeValue("GRADE_CD", OracleDbType.NVarchar2, gradecd)
            End If
            If Not String.IsNullOrEmpty(Trim(suffixcd)) Then
                query.AddParameterWithTypeValue("SUFFIX_CD", OracleDbType.NVarchar2, suffixcd)
            End If
            If Not String.IsNullOrEmpty(Trim(exteriorColorcd)) Then
                query.AddParameterWithTypeValue("BODYCLR_CD", OracleDbType.NVarchar2, exteriorColorcd)
            End If
            If Not String.IsNullOrEmpty(Trim(interiorColorcd)) Then
                query.AddParameterWithTypeValue("INTERIORCLR_CD", OracleDbType.NVarchar2, interiorColorcd)
            End If
            query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, account)
            query.AddParameterWithTypeValue("SALES_PROSPECT_CD", OracleDbType.NVarchar2, mostPerfCd)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("AddSelectedSeries_End")
            'ログ出力 End *****************************************************************************

            Return query.Execute()
        End Using
    End Function
    '2017/11/20 TCS 河原 TKM独自機能開発 END

    '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 選択車種削除
    ''' </summary>
    ''' <param name="fllwupboxseqno">商談ID</param>
    ''' <param name="seqno">希望車連番</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function DeleteSelectedSeries(ByVal fllwupboxseqno As String,
                                         ByVal seqno As String,
                                         ByVal lockvr As Long) As Integer

        Dim sql As New StringBuilder

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeleteSelectedSeries_Start")
        'ログ出力 End *****************************************************************************

        With sql
            .Append("DELETE ")
            .Append("    /* SC3080202_123 */ ")
            .Append("FROM ")
            .Append("    TB_T_PREFER_VCL ")
            .Append("WHERE ")
            .Append("        SALES_ID = :FLLWUPBOX_SEQNO ")
            .Append("    AND PREF_VCL_SEQ = :SEQNO ")
            .Append("    AND ROW_LOCK_VERSION = :LOCKVR ")
        End With

        Using query As New DBUpdateQuery("SC3080202_123")

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupboxseqno)
            query.AddParameterWithTypeValue("SEQNO", OracleDbType.Decimal, seqno)
            query.AddParameterWithTypeValue("LOCKVR", OracleDbType.Int64, lockvr)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeleteSelectedSeries_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 徐 2013/10対応版　既存流用 END

            Return query.Execute()
        End Using
    End Function

    '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 選択競合車種追加
    ''' </summary>
    ''' <param name="fllwupboxseqno">商談ID</param>
    ''' <param name="seqno">競合車種連番</param>
    ''' <param name="seriescd">モデルコード</param>
    ''' <param name="account">作成アカウント</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function AddSelectedCompe(ByVal fllwupboxseqno As String,
                                     ByVal seqno As String,
                                     ByVal seriescd As String,
                                     ByVal account As String) As Integer
        Dim sql As New StringBuilder

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("AddSelectedCompe_Start")
        'ログ出力 End *****************************************************************************

        With sql
            .Append("INSERT ")
            .Append("    /* SC3080202_124 */ ")
            .Append("INTO TB_T_COMPETITOR_VCL ( ")
            .Append("    SALES_ID, ")
            .Append("    COMP_VCL_SEQ, ")
            .Append("    MODEL_CD, ")
            .Append("    ROW_CREATE_DATETIME, ")
            .Append("    ROW_CREATE_ACCOUNT, ")
            .Append("    ROW_CREATE_FUNCTION, ")
            .Append("    ROW_UPDATE_DATETIME, ")
            .Append("    ROW_UPDATE_ACCOUNT, ")
            .Append("    ROW_UPDATE_FUNCTION, ")
            .Append("    ROW_LOCK_VERSION ")
            .Append(") ")
            .Append("VALUES ( ")
            .Append("    :FLLWUPBOX_SEQNO, ")
            .Append("    :SEQNO, ")
            .Append("    :SERIESCD, ")
            .Append("    SYSDATE, ")
            .Append("    :ACCOUNT, ")
            .Append("    'SC3080202', ")
            .Append("    SYSDATE, ")
            .Append("    :ACCOUNT, ")
            .Append("    'SC3080202', ")
            .Append("0 ")
            .Append(") ")
        End With

        Using query As New DBUpdateQuery("SC3080202_124")

            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupboxseqno)
            query.AddParameterWithTypeValue("SEQNO", OracleDbType.Decimal, seqno)
            query.AddParameterWithTypeValue("SERIESCD", OracleDbType.NVarchar2, seriescd)
            query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, account)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("AddSelectedCompe_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
            Return query.Execute()
        End Using
    End Function

    '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 選択競合車種削除
    ''' </summary>
    ''' <param name="fllwupboxseqno">商談ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function DeleteSelectedCompe(ByVal fllwupboxseqno As String) As Integer

        Dim sql As New StringBuilder

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeleteSelectedCompe_Start")
        'ログ出力 End *****************************************************************************

        With sql
            .Append("DELETE ")
            .Append("    /* SC3080202_125 */ ")
            .Append("FROM ")
            .Append("    TB_T_COMPETITOR_VCL ")
            .Append("WHERE ")
            .Append("    SALES_ID = :FLLWUPBOX_SEQNO ")
        End With

        Using query As New DBUpdateQuery("SC3080202_125")

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupboxseqno)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeleteSelectedCompe_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
            Return query.Execute()
        End Using
    End Function

    '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 選択台数編集
    ''' </summary>
    ''' <param name="fllwupboxseqno">商談ID</param>
    ''' <param name="seqno">希望車連番</param>
    ''' <param name="quantity">希望数量</param>
    ''' <param name="account">更新アカウント</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function UpdateSelectedSeriesQuantity(ByVal fllwupboxseqno As String,
                                                 ByVal seqno As String,
                                                 ByVal quantity As String,
                                                 ByVal lockvr As Long,
                                                 ByVal account As String) As Integer
        Dim sql As New StringBuilder

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateSelectedSeriesQuantity_Start")
        'ログ出力 End *****************************************************************************

        With sql
            .Append("UPDATE ")
            .Append("    /* SC3080202_126 */ ")
            .Append("    TB_T_PREFER_VCL ")
            .Append("SET ")
            .Append("    PREF_AMOUNT = :QUANTITY, ")
            .Append("    ROW_UPDATE_DATETIME = SYSDATE, ")
            .Append("    ROW_UPDATE_ACCOUNT = :ACCOUNT, ")
            .Append("    ROW_UPDATE_FUNCTION = 'SC3080202', ")
            .Append("    ROW_LOCK_VERSION = :LOCKVR + 1 ")
            .Append("WHERE ")
            .Append("        SALES_ID = :FLLWUPBOX_SEQNO ")
            .Append("    AND PREF_VCL_SEQ = :SEQNO ")
            .Append("    AND ROW_LOCK_VERSION = :LOCKVR ")
        End With

        Using query As New DBUpdateQuery("SC3080202_126")


            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupboxseqno)
            query.AddParameterWithTypeValue("SEQNO", OracleDbType.Decimal, seqno)
            query.AddParameterWithTypeValue("QUANTITY", OracleDbType.Decimal, quantity)
            query.AddParameterWithTypeValue("LOCKVR", OracleDbType.Int64, lockvr)
            query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, account)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateSelectedSeriesQuantity_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
            Return query.Execute()
        End Using
    End Function

    '$01 2012/01/24 Version1.01 yamaguchi 【A.STEP2】代理商談入力機能開発 Start
    ' ''' <summary>
    ' ''' Follow-up BoxシーケンスNo取得
    ' ''' </summary>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Shared Function GetFllwupboxSeqno() As SC3080202DataSet.SC3080202GetFllwupboxNoDataTable

    '    Dim sql As New StringBuilder
    '    With sql
    '        .Append(" SELECT /* SC3080202_027 */ ")
    '        .Append("     SEQ_FLLWUPBOX_FLLWUPBOX_SEQNO.NEXTVAL SEQ ")
    '        .Append(" FROM ")
    '        .Append("     DUAL ")
    '    End With
    '    Using query As New DBSelectQuery(Of SC3080202DataSet.SC3080202GetFllwupboxNoDataTable)("SC3080202_027")

    '        query.CommandText = sql.ToString()

    '        Return query.GetData()
    '    End Using
    'End Function
    '$01 2012/01/24 Version1.01 yamaguchi 【A.STEP2】代理商談入力機能開発 End

    '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 希望車種シーケンスNo取得
    ''' </summary>
    ''' <param name="salesid">商談ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetSelectedSeriesSeqno(ByVal salesid As String) As SC3080202DataSet.SC3080202GetSelectedSeriesNoDataTable

        Dim sql As New StringBuilder

        With sql
            .Append("SELECT ")
            .Append("  /* SC3080202_128 */ ")
            .Append("  NVL(MAX(PREF_VCL_SEQ),0)+1 AS SEQ ")
            .Append("FROM ")
            .Append("  TB_T_PREFER_VCL ")
            .Append("WHERE ")
            .Append("  SALES_ID = :SALESID ")
        End With

        Using query As New DBSelectQuery(Of SC3080202DataSet.SC3080202GetSelectedSeriesNoDataTable)("SC3080202_128")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("SALESID", OracleDbType.Decimal, salesid)
            '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
            Return query.GetData()
        End Using
    End Function

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 商談テーブルロック処理
    ''' </summary>
    ''' <param name="fllwupbox_seqno">商談ID </param>
    ''' <remarks></remarks>
    Public Shared Sub SelectSalesLock(ByVal fllwupbox_seqno As String)

        Using query As New DBSelectQuery(Of DataTable)("SC3080202_202")

            Dim env As New SystemEnvSetting
            Dim sqlForUpdate As String = " FOR UPDATE WAIT " + env.GetLockWaitTime()
            Dim sql As New StringBuilder

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SelectSalesLock_Start")
            'ログ出力 End *****************************************************************************

            With sql
                .Append("SELECT ")
                .Append("  /* SC3080202_202 */ ")
                .Append("1 ")
                .Append("FROM ")
                .Append("  TB_T_SALES ")
                .Append("WHERE ")
                .Append("  SALES_ID = :FLLWUPBOX_SEQNO ")
                .Append(sqlForUpdate)
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupbox_seqno)
            query.GetData()
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SelectSalesLock_End")
            'ログ出力 End *****************************************************************************

        End Using

    End Sub
    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    ''' <summary>
    ''' Follow-up Box商談メモロック処理
    ''' </summary>
    ''' <param name="fllwupbox_seqno">商談ID </param>
    ''' <remarks></remarks>
    Public Shared Sub SelectFollowupBoxSalesConditionLock(ByVal fllwupbox_seqno As String)

        Using query As New DBSelectQuery(Of DataTable)("SC3080202_203")

            Dim env As New SystemEnvSetting
            Dim sqlForUpdate As String = " FOR UPDATE WAIT " + env.GetLockWaitTime()
            Dim sql As New StringBuilder

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SelectFollowupBoxSalesConditionLock_Start")
            'ログ出力 End *****************************************************************************

            With sql
                .Append("SELECT ")
                .Append("  /* SC3080202_203 */ ")
                .Append("1 ")
                .Append("FROM ")
                .Append("  TBL_FLLWUPBOX_SALESMEMO_WK ")
                .Append("WHERE ")
                .Append("  FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO ")
                .Append(sqlForUpdate)
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupbox_seqno)
            query.GetData()

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SelectFollowupBoxSalesConditionLock_End")
            'ログ出力 End *****************************************************************************
        End Using

    End Sub
    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END


    '2014/03/11 TCS 松月 【A STEP2】活動予定スタッフ取得不具合対応（問連TR-V4-GTMC140305008）Start
    ''' <summary>
    ''' 次回活動アカウント取得
    ''' </summary>
    ''' <param name="salesid">商談ID</param>
    ''' <returns>次回予定スタッフ</returns>
    ''' <remarks></remarks>
    Public Shared Function GetStaffPlan(ByVal salesid As Decimal) As String

        Dim sql As New StringBuilder
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetStaffPlan")
        'ログ出力 End *****************************************************************************
        With sql
            .AppendLine("SELECT ")
            .AppendLine("  /* SC3080202_204 */")
            .AppendLine("  T2.SCHE_STF_CD ")
            .AppendLine(" FROM")
            .AppendLine("  TB_T_SALES T1 ")
            .AppendLine("  ,TB_T_ACTIVITY T2")
            .AppendLine(" WHERE")
            .AppendLine("      T1.REQ_ID = T2.REQ_ID ")
            .AppendLine("  AND T1.ATT_ID = T2.ATT_ID ")
            .AppendLine("  AND T1.SALES_ID = :SALES_ID ")
            .AppendLine("  AND T2.RSLT_FLG = '0' ")
        End With

        Using query As New DBSelectQuery(Of DataTable)("SC3080203_508")

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, salesid)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetStaffPlan")
            'ログ出力 End *****************************************************************************
            Return query.GetData()(0)(0).ToString
        End Using

    End Function



    ''' <summary>
    ''' 活動データ存在スチェック
    ''' </summary>
    ''' <param name="salesid">商談ID</param>
    ''' <returns>対象商談有無</returns>
    ''' <remarks></remarks>
    Public Shared Function GetCountStaffPlan(ByVal salesid As Decimal) As Integer

        Dim sql As New StringBuilder

        With sql
            .AppendLine("SELECT ")
            .AppendLine("  /* SC3080202_205 */")
            .AppendLine("    COUNT(*) ")
            .AppendLine(" FROM")
            .AppendLine("  TB_T_SALES T1 ")
            .AppendLine("  ,TB_T_ACTIVITY T2")
            .AppendLine(" WHERE")
            .AppendLine("      T1.REQ_ID = T2.REQ_ID ")
            .AppendLine("  AND T1.ATT_ID = T2.ATT_ID ")
            .AppendLine("  AND T1.SALES_ID = :SALES_ID ")
            .AppendLine("  AND T2.RSLT_FLG = '0' ")
        End With

        Using query As New DBSelectQuery(Of DataTable)("SC3080202_205")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, salesid)
            query.CommandText = sql.ToString()
            Return Integer.Parse(query.GetData()(0)(0).ToString)

        End Using

    End Function
    '2014/03/11 TCS 松月 【A STEP2】活動予定スタッフ取得不具合対応（問連TR-V4-GTMC140305008）End

#Region "Aカード情報相互連携開発"
    '2013/12/05 TCS 市川 Aカード情報相互連携開発 START

    ''' <summary>
    ''' 用件ソース（1st）マスタ取得
    ''' </summary>
    ''' <param name="dlrCd">販売店コード</param>
    ''' <param name="brnCd">店舗コード</param>
    ''' <param name="contactMtd">コンタクト方法ID</param>
    ''' <returns></returns>
    ''' <remarks>活動きっかけ（用件ソース1st）のマスタを取得します。</remarks>
    Public Shared Function GetSourcesOfACardMaster(ByVal dlrCd As String, ByVal brnCd As String _
                                                 , ByVal contactMtd As String) As SC3080202DataSet.SC3080202SourcesOfACardMasterDataTable

        Dim ret As SC3080202DataSet.SC3080202SourcesOfACardMasterDataTable = Nothing
        Dim sql As New StringBuilder(10000)

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSourcesOfACardMaster_Start")
        'ログ出力 End *****************************************************************************

        Try
            With sql
                .AppendLine("SELECT /* SC3080202_204 */ ")
                .AppendLine("    SOURCE_1_CD ")
                .AppendLine("    ,SOURCE_1_NAME     ")
                .AppendLine("FROM  ")
                .AppendLine("    TB_M_SOURCE_1  ")
                .AppendLine("WHERE  ")
                .AppendLine("    CONTACT_MTD = :CONTACT_MTD ")
                .AppendLine("    AND INUSE_FLG = '1' ")
                .AppendLine("    AND SEL_FLG = '1' ")
                .AppendLine("    AND (DLR_CD = :DLR_CD OR DLR_CD = 'XXXXX') ")
                .AppendLine("    AND (BRN_CD = :BRN_CD OR BRN_CD = 'XXX') ")
                .AppendLine("ORDER BY ")
                .AppendLine("    SORT_ORDER ASC ")
            End With

            Using query As New DBSelectQuery(Of SC3080202DataSet.SC3080202SourcesOfACardMasterDataTable)("SC3080202_204")

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dlrCd)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, brnCd)
                query.AddParameterWithTypeValue("CONTACT_MTD", OracleDbType.NVarchar2, contactMtd)

                ret = query.GetData()

            End Using
        Finally
            sql.Clear()
        End Try

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSourcesOfACardMaster_End")
        'ログ出力 End *****************************************************************************

        Return ret

    End Function

    ' 2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)削除

    ''' <summary>
    ''' 用件ID誘致ID取得
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <returns></returns>
    ''' <remarks>商談テーブルの用件IDおよび誘致IDを取得します。</remarks>
    Public Shared Function GetRequestIDAttractIDBySalesID(ByVal salesId As Decimal) As SC3080202DataSet.SC3080202RequestIDAttractIDBySalesIDDataTable

        Dim ret As SC3080202DataSet.SC3080202RequestIDAttractIDBySalesIDDataTable = Nothing
        Dim sql As New StringBuilder(10000)

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetRequestIDAttractIDBySalesID_Start")
        'ログ出力 End *****************************************************************************

        Try
            With sql
                .AppendLine("SELECT /* SC3080202_208 */ ")
                .AppendLine("    SALES_ID ")
                .AppendLine("    ,REQ_ID ")
                .AppendLine("    ,ATT_ID ")
                .AppendLine("FROM  ")
                .AppendLine("    TB_T_SALES ")
                .AppendLine("WHERE  ")
                .AppendLine("    SALES_ID = :SALES_ID ")
            End With

            Using query As New DBSelectQuery(Of SC3080202DataSet.SC3080202RequestIDAttractIDBySalesIDDataTable)("SC3080202_208")

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, salesId)
                ret = query.GetData()

            End Using
        Finally
            sql.Clear()
        End Try

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetRequestIDAttractIDBySalesID_End")
        'ログ出力 End *****************************************************************************

        Return ret

    End Function

    '2017/11/20 TCS 河原 TKM独自機能開発 START
    ''' <summary>
    ''' 商談情報取得
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <returns></returns>
    ''' <remarks>商談情報画面表示用のデータを取得します。</remarks>
    Public Shared Function GetSalesInfoDetail(ByVal salesId As Decimal) As SC3080202DataSet.SC3080202SalesInfoDetailDataTable

        Dim ret As SC3080202DataSet.SC3080202SalesInfoDetailDataTable = Nothing
        Dim sql As New StringBuilder(10000)

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSalesInfoDetail_Start")
        'ログ出力 End *****************************************************************************

        Try
            With sql
                .AppendLine("SELECT /* SC3080202_217 */ ")
                .AppendLine("    NVL(T2.CONTRACTNO,T1.ACARD_NUM) DISPLAY_NUM ")
                .AppendLine("    ,CASE WHEN T2.CONTRACTNO IS NULL OR T2.CONTRACTNO = ' ' THEN 'False' ELSE 'True' END IS_CONTRACTED ")
                '2014/02/12 TCS 山口 受注後フォロー機能開発 START
                .AppendLine("    ,CASE WHEN T2.CONTRACTNO IS NULL THEN '0' ")
                .AppendLine("          ELSE '1' ")
                .AppendLine("     END AS CONTRACTNOFLG ")
                .AppendLine("    ,T2.ESTIMATEID ")
                '2014/02/12 TCS 山口 受注後フォロー機能開発 END
                .AppendLine("    ,T1.SOURCE_1_CD  ")
                .AppendLine("    ,T3.SOURCE_1_NAME  ")
                '2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)削除/追加 start
                .AppendLine("    ,NVL(T1.SOURCE_2_CD, '0') SOURCE_2_CD ")
                .AppendLine("    ,T5.REQ_SECOND_CAT_NAME  ")
                '2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)削除/追加 end
                .AppendLine("    ,T1.SALESLOCKVERSION ")
                .AppendLine("    ,T1.REQUESTLOCKVERSION ")
                .AppendLine("    ,T1.ATTRACTLOCKVERSION  ")
                .AppendLine("    ,T1.DIRECT_SALES_FLG  ")
                .AppendLine("    ,T1.DIRECT_SALES_FLG_UPDATE_FLG  ")
                '2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)追加 start
                .AppendLine("    ,T1.SOURCE_1_CHG_POSSIBLE_FLG  ")
                .AppendLine("    ,T1.SOURCE_2_CHG_POSSIBLE_FLG  ")
                .AppendLine("    ,NVL(T1.GET_TABLE_NO, '0') GET_TABLE_NO ")
                .AppendLine("    ,NVL(T1.ROW_LOCK_VERSION, 0) LTLOCKVERSION ")
                '2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)追加 end
                .AppendLine("FROM ")
                .AppendLine("    ( ")
                '--------------活動登録前
                .AppendLine("    SELECT     ")
                .AppendLine("        T10.SALES_ID  ")
                .AppendLine("        ,T10.ACARD_NUM  ")
                .AppendLine("        ,T10.SOURCE_1_CD ")
                '2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)追加 start
                .AppendLine("        ,NVL(T17.SOURCE_2_CD,'0') SOURCE_2_CD")
                '2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)追加 end
                .AppendLine("        ,T10.ROW_LOCK_VERSION SALESLOCKVERSION ")
                .AppendLine("        ,T10.ROW_LOCK_VERSION REQUESTLOCKVERSION ")
                .AppendLine("        ,T10.ROW_LOCK_VERSION ATTRACTLOCKVERSION ")
                .AppendLine("        ,T10.DIRECT_SALES_FLG DIRECT_SALES_FLG ")
                .AppendLine("        ,NULL DIRECT_SALES_FLG_UPDATE_FLG ")
                '2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)変更 start
                .AppendLine("        ,NVL(T17.SOURCE_1_CHG_POSSIBLE_FLG,'0') SOURCE_1_CHG_POSSIBLE_FLG  ")
                .AppendLine("        ,NVL(T17.SOURCE_2_CHG_POSSIBLE_FLG,'0') SOURCE_2_CHG_POSSIBLE_FLG  ")
                .AppendLine("        ,'1' GET_TABLE_NO  ")
                .AppendLine("        ,NVL(T17.ROW_LOCK_VERSION, 0) ROW_LOCK_VERSION  ")
                .AppendLine("    FROM TB_T_SALES_TEMP T10 ,TB_LT_SALES T17 ")
                '2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)変更 end
                .AppendLine("    WHERE ")
                .AppendLine("        T10.SALES_ID = :SALES_ID     ")
                '2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)追加 start
                .AppendLine("        AND T10.SALES_ID = T17.SALES_ID(+) ")
                '2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)追加 end
                .AppendLine("    UNION ALL ")
                '--------------受注前
                .AppendLine("    SELECT  ")
                .AppendLine("        T11.SALES_ID  ")
                .AppendLine("        ,T11.ACARD_NUM  ")
                .AppendLine("        ,NVL(T12.SOURCE_1_CD,T13.SOURCE_1_CD) SOURCE_1_CD  ")
                '2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)追加 start
                .AppendLine("        ,NVL(T12.SOURCE_2_CD,T13.SOURCE_2_CD) SOURCE_2_CD  ")
                '2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)追加 end
                .AppendLine("        ,T11.ROW_LOCK_VERSION SALESLOCKVERSION ")
                .AppendLine("        ,T12.ROW_LOCK_VERSION REQUESTLOCKVERSION ")
                .AppendLine("        ,T13.ROW_LOCK_VERSION ATTRACTLOCKVERSION  ")
                .AppendLine("        ,T11.DIRECT_SALES_FLG DIRECT_SALES_FLG  ")
                .AppendLine("        ,T11.DIRECT_SALES_FLG_UPDATE_FLG DIRECT_SALES_FLG_UPDATE_FLG  ")
                '2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)追加 start
                .AppendLine("        ,NVL(T18.SOURCE_1_CHG_POSSIBLE_FLG,'0') SOURCE_1_CHG_POSSIBLE_FLG  ")
                .AppendLine("        ,NVL(T18.SOURCE_2_CHG_POSSIBLE_FLG,'0') SOURCE_2_CHG_POSSIBLE_FLG  ")
                .AppendLine("        ,'2' GET_TABLE_NO  ")
                .AppendLine("        ,NVL(T18.ROW_LOCK_VERSION, 0) ROW_LOCK_VERSION  ")
                '2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)追加 end
                .AppendLine("    FROM TB_T_SALES T11 ")
                .AppendLine("        ,TB_T_REQUEST T12 ")
                .AppendLine("        ,TB_T_ATTRACT T13 ")
                '2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)追加 start
                .AppendLine("        ,TB_LT_SALES T18 ")
                '2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)追加 end
                .AppendLine("    WHERE ")
                .AppendLine("        T11.SALES_ID = :SALES_ID ")
                .AppendLine("        AND T11.REQ_ID = T12.REQ_ID(+) ")
                .AppendLine("        AND T11.ATT_ID = T13.ATT_ID(+)  ")
                '2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)追加 start
                .AppendLine("        AND T11.SALES_ID = T18.SALES_ID(+) ")
                '2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)追加 end
                .AppendLine("    UNION ALL ")
                '--------------受注後
                .AppendLine("    SELECT   ")
                .AppendLine("        T14.SALES_ID  ")
                .AppendLine("        ,T14.ACARD_NUM  ")
                .AppendLine("        ,NVL(T15.SOURCE_1_CD,T16.SOURCE_1_CD) SOURCE_1_CD  ")
                '2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)追加 start
                .AppendLine("        ,NVL(T15.SOURCE_2_CD,T16.SOURCE_2_CD) SOURCE_2_CD  ")
                '2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)追加 end
                .AppendLine("        ,T14.ROW_LOCK_VERSION SALESLOCKVERSION ")
                .AppendLine("        ,T15.ROW_LOCK_VERSION REQUESTLOCKVERSION ")
                .AppendLine("        ,T16.ROW_LOCK_VERSION ATTRACTLOCKVERSION  ")
                .AppendLine("        ,T14.DIRECT_SALES_FLG DIRECT_SALES_FLG  ")
                .AppendLine("        ,T14.DIRECT_SALES_FLG_UPDATE_FLG DIRECT_SALES_FLG_UPDATE_FLG  ")
                '2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)追加 start
                .AppendLine("        ,NVL(T19.SOURCE_1_CHG_POSSIBLE_FLG,'0') SOURCE_1_CHG_POSSIBLE_FLG  ")
                .AppendLine("        ,NVL(T19.SOURCE_2_CHG_POSSIBLE_FLG,'0') SOURCE_2_CHG_POSSIBLE_FLG  ")
                .AppendLine("        ,'3' GET_TABLE_NO  ")
                .AppendLine("        ,NVL(T19.ROW_LOCK_VERSION, 0) ROW_LOCK_VERSION  ")
                '2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)追加 end
                .AppendLine("    FROM TB_H_SALES T14 ")
                .AppendLine("        ,TB_H_REQUEST T15 ")
                .AppendLine("        ,TB_H_ATTRACT T16 ")
                '2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)追加 start
                .AppendLine("        ,TB_LT_SALES T19 ")
                '2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)追加 end
                .AppendLine("    WHERE ")
                .AppendLine("        T14.SALES_ID = :SALES_ID ")
                .AppendLine("        AND T14.REQ_ID = T15.REQ_ID(+) ")
                .AppendLine("        AND T14.ATT_ID = T16.ATT_ID(+)  ")
                '2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)追加 start
                .AppendLine("        AND T14.SALES_ID = T19.SALES_ID(+) ")
                '2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)追加 end
                .AppendLine("    ) T1  ")
                .AppendLine("    ,TBL_ESTIMATEINFO T2 ")
                .AppendLine("    ,TB_M_SOURCE_1 T3 ")
                '2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)追加/削除 start
                .AppendLine("    ,TB_M_SOURCE_2 T5 ")
                '2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)追加/削除 end
                .AppendLine("WHERE  ")
                .AppendLine("    T1.SALES_ID = T2.FLLWUPBOX_SEQNO(+) ")
                .AppendLine("    AND T2.CONTRACTFLG(+) = '1'  ")
                .AppendLine("    AND T2.DELFLG(+) = '0'  ")
                .AppendLine("    AND T1.SOURCE_1_CD = T3.SOURCE_1_CD(+) ")
                '2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)追加/削除 start
                .AppendLine("    AND T1.SOURCE_2_CD = T5.SOURCE_2_CD(+) ")
                .AppendLine("    AND T1.SOURCE_1_CD = T5.SOURCE_1_CD(+) ")
                '2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)追加/削除 end
            End With

            Using query As New DBSelectQuery(Of SC3080202DataSet.SC3080202SalesInfoDetailDataTable)("SC3080202_217")

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, salesId)
                ret = query.GetData()

            End Using
        Finally
            sql.Clear()
        End Try

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSalesInfoDetail_End")
        'ログ出力 End *****************************************************************************

        Return ret

    End Function
    '2017/11/20 TCS 河原 TKM独自機能開発 END

    ''' <summary>
    ''' 商談見込み度コードクリア
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <param name="updateAccount">アカウント</param>
    ''' <returns>処理結果（True:クリア成功/False:クリア失敗）</returns>
    ''' <remarks>対象商談の全希望車種の商談見込み度コードを倒します。</remarks>
    Public Shared Function ClearSalesProspectCd(ByVal salesId As Decimal, ByVal updateAccount As String) As Integer

        Dim ret As Integer = 0
        Dim sql As New StringBuilder(10000)
        Dim env As New SystemEnvSetting()

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("ClearSalesProspectCd_Start")
        'ログ出力 End *****************************************************************************

        Try
            With sql
                .AppendLine("UPDATE /* SC3080202_206 */ ")
                .AppendLine("    TB_T_PREFER_VCL ")
                .AppendLine("SET  ")
                .AppendLine("    SALES_PROSPECT_CD = ' ', ")
                .AppendLine("    ROW_UPDATE_DATETIME = SYSDATE,  ")
                .AppendLine("    ROW_UPDATE_ACCOUNT = :ACCOUNT,  ")
                .AppendLine("    ROW_UPDATE_FUNCTION = 'SC3080202',  ")
                .AppendLine("    ROW_LOCK_VERSION = ROW_LOCK_VERSION + 1  ")
                .AppendLine("WHERE  ")
                .AppendLine("    SALES_ID = :SALES_ID ")
            End With

            Using query As New DBUpdateQuery("SC3080202_206")

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, salesId)
                query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, updateAccount)

                ret = query.Execute()
            End Using
        Finally
            sql.Clear()
            env = Nothing
        End Try

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("ClearSalesProspectCd_End")
        'ログ出力 End *****************************************************************************

        Return ret

    End Function

    ''' <summary>
    ''' 商談見込み度コード既定値設定
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <param name="updateAccount">更新ユーザ</param>
    ''' <param name="salesProspectCd">商談見込み度コード</param>
    ''' <returns>処理結果（True:クリア成功/False:クリア失敗）</returns>
    ''' <remarks>対象商談の全希望車種の商談見込み度コードを倒します。</remarks>
    Public Shared Function SetDefaultSalesProspectCd(ByVal salesId As Decimal, ByVal updateAccount As String, ByVal salesProspectCd As String) As Integer

        Dim ret As Integer = 0
        Dim sql As New StringBuilder(10000)
        Dim env As New SystemEnvSetting()

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SetDefaultSalesProspectCd_Start")
        'ログ出力 End *****************************************************************************

        Try
            With sql
                .AppendLine("UPDATE /* SC3080202_207 */ ")
                .AppendLine("    TB_T_PREFER_VCL ")
                .AppendLine("SET  ")
                .AppendLine("    SALES_PROSPECT_CD = :SALES_PROSPECT_CD, ")
                .AppendLine("    ROW_UPDATE_DATETIME = SYSDATE,  ")
                .AppendLine("    ROW_UPDATE_ACCOUNT = :ACCOUNT,  ")
                .AppendLine("    ROW_UPDATE_FUNCTION = 'SC3080202',  ")
                .AppendLine("    ROW_LOCK_VERSION = ROW_LOCK_VERSION + 1  ")
                .AppendLine("WHERE  ")
                .AppendLine("    SALES_ID = :SALES_ID ")
                .AppendLine("    AND PREF_VCL_SEQ = ( ")
                .AppendLine("        SELECT MAX(PREF_VCL_SEQ)  ")
                .AppendLine("        FROM TB_T_PREFER_VCL ")
                .AppendLine("        WHERE SALES_ID = :SALES_ID) ")

            End With

            Using query As New DBUpdateQuery("SC3080202_207")

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("SALES_PROSPECT_CD", OracleDbType.NVarchar2, salesProspectCd)
                query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, salesId)
                query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, updateAccount)

                ret = query.Execute()
            End Using
        Finally
            sql.Clear()
            env = Nothing
        End Try

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SetDefaultSalesProspectCd_End")
        'ログ出力 End *****************************************************************************

        Return ret

    End Function

    ' 2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)削除  

#Region "用件ソース更新"


    ''' <summary>
    ''' 用件ソース1st更新（商談一時情報）
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <param name="sources1Cd">用件ソース1stID</param>
    ''' <param name="updateAccount">更新ユーザ</param>
    ''' <returns>処理結果（True:更新成功/False:更新失敗）</returns>
    ''' <remarks>商談一時情報テーブルの用件ソース1stを更新します。</remarks>
    Public Shared Function UpdateSourceOfACardTemp(ByVal salesId As Decimal, ByVal sources1Cd As Long _
                                                      , ByVal updateAccount As String) As Integer

        Dim ret As Integer = 0
        Dim sql As New StringBuilder(10000)
        Dim env As New SystemEnvSetting()

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateｄSourceOfACardTemp_Start")
        'ログ出力 End *****************************************************************************

        Try
            With sql
                .AppendLine("UPDATE /* SC3080202_210 */ ")
                .AppendLine("    TB_T_SALES_TEMP ")
                .AppendLine("SET  ")
                .AppendLine("    SOURCE_1_CD = :SOURCE_1_CD, ")
                .AppendLine("    ROW_UPDATE_DATETIME = SYSDATE,  ")
                .AppendLine("    ROW_UPDATE_ACCOUNT = :ACCOUNT,  ")
                .AppendLine("    ROW_UPDATE_FUNCTION = 'SC3080202',  ")
                .AppendLine("    ROW_LOCK_VERSION = ROW_LOCK_VERSION + 1  ")
                .AppendLine("WHERE  ")
                .AppendLine("    SALES_ID = :SALES_ID ")
            End With

            Using query As New DBUpdateQuery("SC3080202_210")

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, salesId)
                query.AddParameterWithTypeValue("SOURCE_1_CD", OracleDbType.Long, sources1Cd)
                query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, updateAccount)

                ret = query.Execute()
            End Using
        Finally
            sql.Clear()
            env = Nothing
        End Try

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateｄSourceOfACardTemp_End")
        'ログ出力 End *****************************************************************************

        Return ret

    End Function

    ''' <summary>
    ''' 用件ソース1st更新（用件）
    ''' </summary>
    ''' <param name="requestId">用件ID</param>
    ''' <param name="sources1Cd">用件ソース1stID</param>
    ''' <param name="updateAccount">更新ユーザ</param>
    ''' <returns>処理結果（True:更新成功/False:更新失敗）</returns>
    ''' <remarks>用件テーブルの用件ソース1stを更新します。</remarks>
    Public Shared Function UpdateSourceOfACardRequesst(ByVal requestId As Decimal, ByVal sources1Cd As Long _
                                                      , ByVal updateAccount As String) As Integer

        Dim ret As Integer = 0
        Dim sql As New StringBuilder(10000)
        Dim env As New SystemEnvSetting()

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateSourceOfACardRequesst_Start")
        'ログ出力 End *****************************************************************************

        Try
            With sql
                .AppendLine("UPDATE /* SC3080202_214 */ ")
                .AppendLine("    TB_T_REQUEST ")
                .AppendLine("SET  ")
                .AppendLine("    SOURCE_1_CD = :SOURCE_1_CD, ")
                .AppendLine("    ROW_UPDATE_DATETIME = SYSDATE,  ")
                .AppendLine("    ROW_UPDATE_ACCOUNT = :ACCOUNT,  ")
                .AppendLine("    ROW_UPDATE_FUNCTION = 'SC3080202',  ")
                .AppendLine("    ROW_LOCK_VERSION = ROW_LOCK_VERSION + 1  ")
                .AppendLine("WHERE  ")
                .AppendLine("    REQ_ID = :REQ_ID ")
            End With

            Using query As New DBUpdateQuery("SC3080202_214")

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("REQ_ID", OracleDbType.Decimal, requestId)
                query.AddParameterWithTypeValue("SOURCE_1_CD", OracleDbType.Long, sources1Cd)
                query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, updateAccount)

                ret = query.Execute()
            End Using
        Finally
            sql.Clear()
            env = Nothing
        End Try

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateSourceOfACardRequesst_End")
        'ログ出力 End *****************************************************************************

        Return ret

    End Function

    ''' <summary>
    ''' 用件ソース1st更新（誘致）
    ''' </summary>
    ''' <param name="attractId">誘致ID</param>
    ''' <param name="sources1Cd">用件ソース1stID</param>
    ''' <param name="updateAccount">更新ユーザ</param>
    ''' <returns>処理結果（True:更新成功/False:更新失敗）</returns>
    ''' <remarks>誘致テーブルの用件ソース1stを更新します。</remarks>
    Public Shared Function UpdateSourceOfACardAttract(ByVal attractId As Decimal, ByVal sources1Cd As Long _
                                                      , ByVal updateAccount As String) As Integer

        Dim ret As Integer = 0
        Dim sql As New StringBuilder(10000)
        Dim env As New SystemEnvSetting()

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateSourceOfACardAttract_Start")
        'ログ出力 End *****************************************************************************

        Try
            With sql
                .AppendLine("UPDATE /* SC3080202_216 */ ")
                .AppendLine("     TB_T_ATTRACT ")
                .AppendLine("SET  ")
                .AppendLine("    SOURCE_1_CD = :SOURCE_1_CD, ")
                .AppendLine("    ROW_UPDATE_DATETIME = SYSDATE,  ")
                .AppendLine("    ROW_UPDATE_ACCOUNT = :ACCOUNT,  ")
                .AppendLine("    ROW_UPDATE_FUNCTION = 'SC3080202',  ")
                .AppendLine("    ROW_LOCK_VERSION = ROW_LOCK_VERSION + 1  ")
                .AppendLine("WHERE  ")
                .AppendLine("      ATT_ID = :ATT_ID ")
            End With

            Using query As New DBUpdateQuery("SC3080202_216")

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("ATT_ID", OracleDbType.Decimal, attractId)
                query.AddParameterWithTypeValue("SOURCE_1_CD", OracleDbType.Long, sources1Cd)
                query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, updateAccount)

                ret = query.Execute()
            End Using
        Finally
            sql.Clear()
            env = Nothing
        End Try

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateSourceOfACardAttract_End")
        'ログ出力 End *****************************************************************************

        Return ret

    End Function

#End Region

#Region "行ロック"

    ''' <summary>
    ''' 商談一時情報データロック
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <returns>処理結果（ロック成功行数）</returns>
    ''' <remarks>商談一時情報を行ロックする。</remarks>
    Public Shared Function LockSalesTemp(ByVal salesId As Decimal) As Integer

        Dim ret As Integer = 0
        Dim sql As New StringBuilder(10000)
        Dim env As New SystemEnvSetting()

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("LockSalesTemp_Start")
        'ログ出力 End *****************************************************************************

        Try
            With sql
                .AppendLine("SELECT /* SC3080202_211 */ ")
                .AppendLine("   1 ")
                .AppendLine("FROM TB_T_SALES_TEMP ")
                .AppendLine("WHERE SALES_ID = :SALES_ID ")
                .AppendFormat("FOR UPDATE WAIT {0} ", env.GetLockWaitTime())
            End With

            Using query As New DBSelectQuery(Of DataTable)("SC3080202_211")

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, salesId)

                ret = query.GetCount()
            End Using
        Finally
            sql.Clear()
            env = Nothing
        End Try

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("LockSalesTemp_End")
        'ログ出力 End *****************************************************************************

        Return ret

    End Function

    ''' <summary>
    ''' 用件ロック処理
    ''' </summary>
    ''' <param name="requestId">用件ID</param>
    ''' <returns>処理結果（ロック成功行数）</returns>
    ''' <remarks>用件テーブルを行ロックします。</remarks>
    Public Shared Function LockRequest(ByVal requestId As Decimal) As Integer

        Dim ret As Integer = 0
        Dim sql As New StringBuilder(10000)
        Dim env As New SystemEnvSetting()

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("LockRequest_Start")
        'ログ出力 End *****************************************************************************

        Try
            With sql
                .AppendLine("SELECT /* SC3080202_213 */ ")
                .AppendLine("   1 ")
                .AppendLine("FROM TB_T_REQUEST ")
                .AppendLine("WHERE REQ_ID = :REQ_ID ")
                .AppendFormat("FOR UPDATE WAIT {0} ", env.GetLockWaitTime())
            End With

            Using query As New DBSelectQuery(Of DataTable)("SC3080202_213")

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("REQ_ID", OracleDbType.Decimal, requestId)

                ret = query.GetCount()
            End Using
        Finally
            sql.Clear()
            env = Nothing
        End Try

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("LockRequest_End")
        'ログ出力 End *****************************************************************************

        Return ret

    End Function

    ''' <summary>
    ''' 誘致ロック処理
    ''' </summary>
    ''' <param name="attractId">誘致ID</param>
    ''' <returns>処理結果（ロック成功行数）</returns>
    ''' <remarks>誘致テーブルを行ロックします。</remarks>
    Public Shared Function LockAttract(ByVal attractId As Decimal) As Integer

        Dim ret As Integer = 0
        Dim sql As New StringBuilder(10000)
        Dim env As New SystemEnvSetting()

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("LockAttract_Start")
        'ログ出力 End *****************************************************************************

        Try
            With sql
                .AppendLine("SELECT /* SC3080202_215 */ ")
                .AppendLine("   1 ")
                .AppendLine("FROM TB_T_ATTRACT ")
                .AppendLine("WHERE ATT_ID = :ATT_ID ")
                .AppendFormat("FOR UPDATE WAIT {0} ", env.GetLockWaitTime())
            End With

            Using query As New DBSelectQuery(Of DataTable)("SC3080202_215")

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("ATT_ID", OracleDbType.Decimal, attractId)

                ret = query.GetCount()
            End Using
        Finally
            sql.Clear()
            env = Nothing
        End Try

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("LockAttract_End")
        'ログ出力 End *****************************************************************************

        Return ret

    End Function

#End Region

    '2013/12/05 TCS 市川 Aカード情報相互連携開発 END
#End Region

    '2014/02/12 TCS 山口 受注後フォロー機能開発 START
#Region "受注後フォロー機能開発"
    ''' <summary>
    ''' 契約車両情報取得
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <returns>契約車両情報</returns>
    ''' <remarks></remarks>
    Public Shared Function GetContractCarData(ByVal salesId As Decimal) As SC3080202DataSet.SC3080202GetContractCarDataTable
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetContractCarData_Start")
        'ログ出力 End *****************************************************************************

        Dim sql As New StringBuilder()

        With sql
            .Append(" SELECT /* SC3080202_218 */ ")
            .Append("        T1.DLRCD ")
            .Append("      , T1.CONTRACTNO ")
            .Append("      , T1.CONTRACTDATE ")
            .Append("      , T2.SUFFIXCD ")
            .Append("   FROM TBL_ESTIMATEINFO T1 ")
            .Append("      , TBL_EST_VCLINFO T2 ")
            .Append("  WHERE T1.DELFLG = 0 ")
            .Append("    AND T1.FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO ")
            .Append("    AND T1.ESTIMATEID = T2.ESTIMATEID ")
        End With

        Using query As New DBSelectQuery(Of SC3080202DataSet.SC3080202GetContractCarDataTable)("SC3080202_218")
            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, salesId)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetContractCarData_End")
            'ログ出力 End *****************************************************************************

            Return query.GetData()
        End Using

    End Function

    ''' <summary>
    ''' 受注後工程詳細情報日付取得
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <param name="afterOdrActId">受注後活動コード</param>
    ''' <param name="afterOdrDate">取得日付区分</param>
    ''' <returns>受注後工程詳細情報日付</returns>
    ''' <remarks></remarks>
    Public Shared Function GetBookedAfterDetailInfoDate(ByVal salesId As Decimal, _
                                                        ByVal afterOdrActId As String, _
                                                        ByVal afterOdrDate As AfterOdrDate) As SC3080202DataSet.SC3080202GetBookedAfterDetailInfoDateDataTable
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetBookedAfterDetailInfoDate_Start")
        'ログ出力 End *****************************************************************************

        Dim sql As New StringBuilder()

        Using query As New DBSelectQuery(Of SC3080202DataSet.SC3080202GetBookedAfterDetailInfoDateDataTable)("SC3080202_219")
            With sql
                .AppendLine(" SELECT /* SC3080202_219 */ ")
                Select Case afterOdrDate
                    Case afterOdrDate.ScheStartDateOrTime
                        .AppendLine("     T2.SCHE_START_DATEORTIME AS START_END_DATETIME, ")
                        .AppendLine("     T2.SCHE_DATEORTIME_FLG AS DATEORTIME_FLG ")
                    Case afterOdrDate.ScheEndDateOrTime
                        .AppendLine("     T2.SCHE_END_DATEORTIME AS START_END_DATETIME, ")
                        .AppendLine("     T2.SCHE_DATEORTIME_FLG AS DATEORTIME_FLG ")
                    Case afterOdrDate.RsltStartDateOrTime
                        .AppendLine("     T2.RSLT_START_DATEORTIME AS START_END_DATETIME, ")
                        .AppendLine("     T2.RSLT_DATEORTIME_FLG AS DATEORTIME_FLG ")
                    Case afterOdrDate.RsltEndDateOrTime
                        .AppendLine("     T2.RSLT_END_DATEORTIME AS START_END_DATETIME, ")
                        .AppendLine("     T2.RSLT_DATEORTIME_FLG AS DATEORTIME_FLG ")
                End Select
                .AppendLine(" FROM ")
                .AppendLine("     TB_T_AFTER_ODR T1, ")
                .AppendLine("     TB_T_AFTER_ODR_ACT T2 ")
                .AppendLine(" WHERE ")
                .AppendLine("         T1.AFTER_ODR_ID = T2.AFTER_ODR_ID ")
                .AppendLine("     AND T1.SALES_ID = :SALES_ID ")
                .AppendLine("     AND T2.AFTER_ODR_ACT_CD = :AFTER_ODR_ACT_CD ")
                .AppendLine(" UNION ALL ")
                .AppendLine(" SELECT ")
                Select Case afterOdrDate
                    Case afterOdrDate.ScheStartDateOrTime
                        .AppendLine("     T2.SCHE_START_DATEORTIME AS START_END_DATETIME, ")
                        .AppendLine("     T2.SCHE_DATEORTIME_FLG AS DATEORTIME_FLG ")
                    Case afterOdrDate.ScheEndDateOrTime
                        .AppendLine("     T2.SCHE_END_DATEORTIME AS START_END_DATETIME, ")
                        .AppendLine("     T2.SCHE_DATEORTIME_FLG AS DATEORTIME_FLG ")
                    Case afterOdrDate.RsltStartDateOrTime
                        .AppendLine("     T2.RSLT_START_DATEORTIME AS START_END_DATETIME, ")
                        .AppendLine("     T2.RSLT_DATEORTIME_FLG AS DATEORTIME_FLG ")
                    Case afterOdrDate.RsltEndDateOrTime
                        .AppendLine("     T2.RSLT_END_DATEORTIME AS START_END_DATETIME, ")
                        .AppendLine("     T2.RSLT_DATEORTIME_FLG AS DATEORTIME_FLG ")
                End Select
                .AppendLine(" FROM ")
                .AppendLine("     TB_H_AFTER_ODR T1, ")
                .AppendLine("     TB_H_AFTER_ODR_ACT T2 ")
                .AppendLine(" WHERE ")
                .AppendLine("         T1.AFTER_ODR_ID = T2.AFTER_ODR_ID ")
                .AppendLine("     AND T1.SALES_ID = :SALES_ID ")
                .AppendLine("     AND T2.AFTER_ODR_ACT_CD = :AFTER_ODR_ACT_CD ")
            End With

            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, salesId)
            query.AddParameterWithTypeValue("AFTER_ODR_ACT_CD", OracleDbType.NVarchar2, afterOdrActId)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetBookedAfterDetailInfoDate_End")
            'ログ出力 End *****************************************************************************

            Return query.GetData()
        End Using

    End Function

    '2019/12/06 TS  三浦 （FS）e-CRBシステム各種作業効率化の品質検証 START
    ''' <summary>
    ''' 受注後工程詳細情報ステイタス取得
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <param name="afterOdrActId">受注後活動コード</param>
    ''' <returns>受注後工程詳細情報ステイタス</returns>
    ''' <remarks></remarks>
    Public Shared Function GetBookedAfterDetailInfoStatus(ByVal salesId As Decimal, _
                                                          ByVal afterOdrActId As String) As SC3080202DataSet.SC3080202GetBookedAfterDetailInfoStatusDataTable
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetBookedAfterDetailInfoStatus_Start")
        'ログ出力 End *****************************************************************************

        Dim sql As New StringBuilder()

        Using query As New DBSelectQuery(Of SC3080202DataSet.SC3080202GetBookedAfterDetailInfoStatusDataTable)("SC3080202_220")
            With sql
                .Append(" SELECT /* SC3080202_220 */ ")
                .Append("     CASE WHEN T4.WORD_VAL IS NULL THEN NULL ")
                .Append("          WHEN T4.WORD_VAL = ' ' THEN TRIM(T4.WORD_VAL_ENG) ")
                .Append("          ELSE TRIM(T4.WORD_VAL) ")
                .Append("     END AS AFTER_ODR_ACT_STATUS_NAME ")
                .Append(" FROM ")
                .Append("     TB_T_AFTER_ODR T1, ")
                .Append("     TB_T_AFTER_ODR_ACT T2, ")
                .Append("     TB_M_SPM_ODR_ACT_STATUS T3, ")
                .Append("     TB_M_WORD T4 ")
                .Append(" WHERE ")
                .Append("         T1.AFTER_ODR_ID = T2.AFTER_ODR_ID ")
                .Append("     AND T1.SALES_ID = :SALES_ID ")
                .Append("     AND T2.AFTER_ODR_ACT_CD = :AFTER_ODR_ACT_CD ")
                .Append("     AND T2.AFTER_ODR_ACT_CD = T3.AFTER_ODR_ACT_CD ")
                .Append("     AND T2.AFTER_ODR_ACT_STATUS = T3.AFTER_ODR_ACT_STATUS ")
                .Append("     AND T3.AFTER_ODR_ACT_STATUS_NAME = T4.WORD_CD(+) ")
                .Append(" UNION ALL ")
                .Append(" SELECT ")
                .Append("     CASE WHEN T4.WORD_VAL IS NULL THEN NULL ")
                .Append("          WHEN T4.WORD_VAL = ' ' THEN TRIM(T4.WORD_VAL_ENG) ")
                .Append("          ELSE TRIM(T4.WORD_VAL) ")
                .Append("     END AS AFTER_ODR_ACT_STATUS_NAME ")
                .Append(" FROM ")
                .Append("     TB_H_AFTER_ODR T1, ")
                .Append("     TB_H_AFTER_ODR_ACT T2, ")
                .Append("     TB_M_SPM_ODR_ACT_STATUS T3, ")
                .Append("     TB_M_WORD T4 ")
                .Append(" WHERE ")
                .Append("         T1.AFTER_ODR_ID = T2.AFTER_ODR_ID ")
                .Append("     AND T1.SALES_ID = :SALES_ID ")
                .Append("     AND T2.AFTER_ODR_ACT_CD = :AFTER_ODR_ACT_CD ")
                .Append("     AND T2.AFTER_ODR_ACT_CD = T3.AFTER_ODR_ACT_CD ")
                .Append("     AND T2.AFTER_ODR_ACT_STATUS = T3.AFTER_ODR_ACT_STATUS ")
                .Append("     AND T3.AFTER_ODR_ACT_STATUS_NAME = T4.WORD_CD(+) ")
            End With

            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, salesId)
            query.AddParameterWithTypeValue("AFTER_ODR_ACT_CD", OracleDbType.NVarchar2, afterOdrActId)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetBookedAfterDetailInfoStatus_End")
            'ログ出力 End *****************************************************************************

            Return query.GetData()
        End Using
    End Function

    ''' <summary>
    ''' 受注後工程詳細情報ステイタス取得(VDQI)
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <param name="afterOdrActId1">受注後活動コード1</param>
    ''' <param name="afterOdrActId2">受注後活動コード2</param>
    ''' <returns>受注後工程詳細情報ステイタス</returns>
    ''' <remarks></remarks>
    Public Shared Function GetBookedAfterDetailInfoVDQIStatus(ByVal salesId As Decimal, _
                                                              ByVal afterOdrActId1 As String, _
                                                              ByVal afterOdrActId2 As String) As SC3080202DataSet.SC3080202GetBookedAfterDetailInfoStatusDataTable
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetBookedAfterDetailInfoStatus_Start")
        'ログ出力 End *****************************************************************************

        Dim sql As New StringBuilder()

        Using query As New DBSelectQuery(Of SC3080202DataSet.SC3080202GetBookedAfterDetailInfoStatusDataTable)("SC3080202_220")
            With sql
                .Append(" SELECT /* SC3080202_220 */ ")
                .Append("     CASE WHEN T4.WORD_VAL IS NULL THEN NULL ")
                .Append("          WHEN T4.WORD_VAL = ' ' THEN TRIM(T4.WORD_VAL_ENG) ")
                .Append("          ELSE TRIM(T4.WORD_VAL) ")
                .Append("     END AS AFTER_ODR_ACT_STATUS_NAME ")
                .Append(" FROM ")
                .Append("     ( ")
                .Append("         SELECT ")
                .Append("             CASE WHEN T22.AFTER_ODR_ACT_STATUS <> 0 THEN T22.AFTER_ODR_ACT_CD ")
                .Append("                  ELSE T21.AFTER_ODR_ACT_CD ")
                .Append("             END AS AFTER_ODR_ACT_CD, ")
                .Append("             CASE WHEN T22.AFTER_ODR_ACT_STATUS <> 0 THEN T22.AFTER_ODR_ACT_STATUS ")
                .Append("                  ELSE T21.AFTER_ODR_ACT_STATUS ")
                .Append("             END AS AFTER_ODR_ACT_STATUS ")
                .Append("         FROM ")
                .Append("             TB_T_AFTER_ODR T1, ")
                .Append("             TB_T_AFTER_ODR_ACT T21, ")
                .Append("             TB_T_AFTER_ODR_ACT T22 ")
                .Append("         WHERE ")
                .Append("                 T1.SALES_ID = :SALES_ID ")
                .Append("             AND T1.AFTER_ODR_ID = T21.AFTER_ODR_ID(+) ")
                .Append("             AND T1.AFTER_ODR_ID = T22.AFTER_ODR_ID(+) ")
                .Append("             AND T21.AFTER_ODR_ACT_CD(+) = :AFTER_ODR_ACT_CD1 ")
                .Append("             AND T22.AFTER_ODR_ACT_CD(+) = :AFTER_ODR_ACT_CD2 ")
                .Append("     ) T2, ")
                .Append("     TB_M_SPM_ODR_ACT_STATUS T3, ")
                .Append("     TB_M_WORD T4 ")
                .Append(" WHERE ")
                .Append("         T2.AFTER_ODR_ACT_CD = T3.AFTER_ODR_ACT_CD ")
                .Append("     AND T2.AFTER_ODR_ACT_STATUS = T3.AFTER_ODR_ACT_STATUS ")
                .Append("     AND T3.AFTER_ODR_ACT_STATUS_NAME = T4.WORD_CD(+) ")
                .Append(" UNION ALL ")
                .Append(" SELECT /* SC3080202_220 */ ")
                .Append("     CASE WHEN T4.WORD_VAL IS NULL THEN NULL ")
                .Append("          WHEN T4.WORD_VAL = ' ' THEN TRIM(T4.WORD_VAL_ENG) ")
                .Append("          ELSE TRIM(T4.WORD_VAL) ")
                .Append("     END AS AFTER_ODR_ACT_STATUS_NAME ")
                .Append(" FROM ")
                .Append("     ( ")
                .Append("         SELECT ")
                .Append("             CASE WHEN T22.AFTER_ODR_ACT_STATUS <> 0 THEN T22.AFTER_ODR_ACT_CD ")
                .Append("                  ELSE T21.AFTER_ODR_ACT_CD ")
                .Append("             END AS AFTER_ODR_ACT_CD, ")
                .Append("             CASE WHEN T22.AFTER_ODR_ACT_STATUS <> 0 THEN T22.AFTER_ODR_ACT_STATUS ")
                .Append("                  ELSE T21.AFTER_ODR_ACT_STATUS ")
                .Append("             END AS AFTER_ODR_ACT_STATUS ")
                .Append("         FROM ")
                .Append("             TB_H_AFTER_ODR T1, ")
                .Append("             TB_H_AFTER_ODR_ACT T21, ")
                .Append("             TB_H_AFTER_ODR_ACT T22 ")
                .Append("         WHERE ")
                .Append("                 T1.SALES_ID = :SALES_ID ")
                .Append("             AND T1.AFTER_ODR_ID = T21.AFTER_ODR_ID(+) ")
                .Append("             AND T1.AFTER_ODR_ID = T22.AFTER_ODR_ID(+) ")
                .Append("             AND T21.AFTER_ODR_ACT_CD(+) = :AFTER_ODR_ACT_CD1 ")
                .Append("             AND T22.AFTER_ODR_ACT_CD(+) = :AFTER_ODR_ACT_CD2 ")
                .Append("     ) T2, ")
                .Append("     TB_M_SPM_ODR_ACT_STATUS T3, ")
                .Append("     TB_M_WORD T4 ")
                .Append(" WHERE ")
                .Append("         T2.AFTER_ODR_ACT_CD = T3.AFTER_ODR_ACT_CD ")
                .Append("     AND T2.AFTER_ODR_ACT_STATUS = T3.AFTER_ODR_ACT_STATUS ")
                .Append("     AND T3.AFTER_ODR_ACT_STATUS_NAME = T4.WORD_CD(+) ")

            End With

            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, salesId)
            query.AddParameterWithTypeValue("AFTER_ODR_ACT_CD1", OracleDbType.NVarchar2, afterOdrActId1)
            query.AddParameterWithTypeValue("AFTER_ODR_ACT_CD2", OracleDbType.NVarchar2, afterOdrActId2)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetBookedAfterDetailInfoStatus_End")
            'ログ出力 End *****************************************************************************

            Return query.GetData()
        End Using
    End Function

    ''' <summary>
    ''' 受注後工程詳細情報ステイタス取得(登録)
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <param name="afterOdrActId1">受注後活動コード1</param>
    ''' <param name="afterOdrActId2">受注後活動コード2</param>
    ''' <param name="afterOdrActId3">受注後活動コード3</param>
    ''' <returns>受注後工程詳細情報ステイタス</returns>
    ''' <remarks></remarks>
    Public Shared Function GetBookedAfterDetailInfoRegistStatus(ByVal salesId As Decimal, _
                                                              ByVal afterOdrActId1 As String, _
                                                              ByVal afterOdrActId2 As String, _
                                                              ByVal afterOdrActId3 As String) As SC3080202DataSet.SC3080202GetBookedAfterDetailInfoStatusDataTable
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetBookedAfterDetailInfoStatus_Start")
        'ログ出力 End *****************************************************************************

        Dim sql As New StringBuilder()

        Using query As New DBSelectQuery(Of SC3080202DataSet.SC3080202GetBookedAfterDetailInfoStatusDataTable)("SC3080202_220")
            With sql
                .Append(" SELECT /* SC3080202_224 */ ")
                .Append("     CASE WHEN T4.WORD_VAL IS NULL THEN NULL ")
                .Append("          WHEN T4.WORD_VAL = ' ' THEN TRIM(T4.WORD_VAL_ENG) ")
                .Append("          ELSE TRIM(T4.WORD_VAL) ")
                .Append("     END AS AFTER_ODR_ACT_STATUS_NAME ")
                .Append(" FROM ")
                .Append("     ( ")
                .Append("         SELECT ")
                .Append("             CASE WHEN T23.AFTER_ODR_ACT_STATUS <> 0 THEN T23.AFTER_ODR_ACT_CD ")
                .Append("                  WHEN T22.AFTER_ODR_ACT_STATUS <> 0 THEN T22.AFTER_ODR_ACT_CD ")
                .Append("                  ELSE T21.AFTER_ODR_ACT_CD ")
                .Append("             END AS AFTER_ODR_ACT_CD, ")
                .Append("             CASE WHEN T23.AFTER_ODR_ACT_STATUS <> 0 THEN T23.AFTER_ODR_ACT_STATUS ")
                .Append("                  WHEN T22.AFTER_ODR_ACT_STATUS <> 0 THEN T22.AFTER_ODR_ACT_STATUS ")
                .Append("                  ELSE T21.AFTER_ODR_ACT_STATUS ")
                .Append("             END AS AFTER_ODR_ACT_STATUS ")
                .Append("         FROM ")
                .Append("             TB_T_AFTER_ODR T1, ")
                .Append("             TB_T_AFTER_ODR_ACT T21, ")
                .Append("             TB_T_AFTER_ODR_ACT T22, ")
                .Append("             TB_T_AFTER_ODR_ACT T23 ")
                .Append("         WHERE ")
                .Append("                 T1.SALES_ID = :SALES_ID ")
                .Append("             AND T1.AFTER_ODR_ID = T21.AFTER_ODR_ID(+) ")
                .Append("             AND T1.AFTER_ODR_ID = T22.AFTER_ODR_ID(+) ")
                .Append("             AND T1.AFTER_ODR_ID = T23.AFTER_ODR_ID(+) ")
                .Append("             AND T21.AFTER_ODR_ACT_CD(+) = :AFTER_ODR_ACT_CD1 ")
                .Append("             AND T22.AFTER_ODR_ACT_CD(+) = :AFTER_ODR_ACT_CD2 ")
                .Append("             AND T23.AFTER_ODR_ACT_CD(+) = :AFTER_ODR_ACT_CD3 ")
                .Append("     ) T2, ")
                .Append("     TB_M_SPM_ODR_ACT_STATUS T3, ")
                .Append("     TB_M_WORD T4 ")
                .Append(" WHERE ")
                .Append("         T2.AFTER_ODR_ACT_CD = T3.AFTER_ODR_ACT_CD ")
                .Append("     AND T2.AFTER_ODR_ACT_STATUS = T3.AFTER_ODR_ACT_STATUS ")
                .Append("     AND T3.AFTER_ODR_ACT_STATUS_NAME = T4.WORD_CD(+) ")
                .Append(" UNION ALL ")
                .Append(" SELECT /* SC3080202_224 */ ")
                .Append("     CASE WHEN T4.WORD_VAL IS NULL THEN NULL ")
                .Append("          WHEN T4.WORD_VAL = ' ' THEN TRIM(T4.WORD_VAL_ENG) ")
                .Append("          ELSE TRIM(T4.WORD_VAL) ")
                .Append("     END AS AFTER_ODR_ACT_STATUS_NAME ")
                .Append(" FROM ")
                .Append("     ( ")
                .Append("         SELECT ")
                .Append("             CASE WHEN T23.AFTER_ODR_ACT_STATUS <> 0 THEN T23.AFTER_ODR_ACT_CD ")
                .Append("                  WHEN T22.AFTER_ODR_ACT_STATUS <> 0 THEN T22.AFTER_ODR_ACT_CD ")
                .Append("                  ELSE T21.AFTER_ODR_ACT_CD ")
                .Append("             END AS AFTER_ODR_ACT_CD, ")
                .Append("             CASE WHEN T23.AFTER_ODR_ACT_STATUS <> 0 THEN T23.AFTER_ODR_ACT_STATUS ")
                .Append("                  WHEN T22.AFTER_ODR_ACT_STATUS <> 0 THEN T22.AFTER_ODR_ACT_STATUS ")
                .Append("                  ELSE T21.AFTER_ODR_ACT_STATUS ")
                .Append("             END AS AFTER_ODR_ACT_STATUS ")
                .Append("         FROM ")
                .Append("             TB_H_AFTER_ODR T1, ")
                .Append("             TB_H_AFTER_ODR_ACT T21, ")
                .Append("             TB_H_AFTER_ODR_ACT T22, ")
                .Append("             TB_H_AFTER_ODR_ACT T23 ")
                .Append("         WHERE ")
                .Append("                 T1.SALES_ID = :SALES_ID ")
                .Append("             AND T1.AFTER_ODR_ID = T21.AFTER_ODR_ID(+) ")
                .Append("             AND T1.AFTER_ODR_ID = T22.AFTER_ODR_ID(+) ")
                .Append("             AND T1.AFTER_ODR_ID = T23.AFTER_ODR_ID(+) ")
                .Append("             AND T21.AFTER_ODR_ACT_CD(+) = :AFTER_ODR_ACT_CD1 ")
                .Append("             AND T22.AFTER_ODR_ACT_CD(+) = :AFTER_ODR_ACT_CD2 ")
                .Append("             AND T23.AFTER_ODR_ACT_CD(+) = :AFTER_ODR_ACT_CD3 ")
                .Append("     ) T2, ")
                .Append("     TB_M_SPM_ODR_ACT_STATUS T3, ")
                .Append("     TB_M_WORD T4 ")
                .Append(" WHERE ")
                .Append("         T2.AFTER_ODR_ACT_CD = T3.AFTER_ODR_ACT_CD ")
                .Append("     AND T2.AFTER_ODR_ACT_STATUS = T3.AFTER_ODR_ACT_STATUS ")
                .Append("     AND T3.AFTER_ODR_ACT_STATUS_NAME = T4.WORD_CD(+) ")

            End With

            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, salesId)
            query.AddParameterWithTypeValue("AFTER_ODR_ACT_CD1", OracleDbType.NVarchar2, afterOdrActId1)
            query.AddParameterWithTypeValue("AFTER_ODR_ACT_CD2", OracleDbType.NVarchar2, afterOdrActId2)
            query.AddParameterWithTypeValue("AFTER_ODR_ACT_CD3", OracleDbType.NVarchar2, afterOdrActId3)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetBookedAfterDetailInfoStatus_End")
            'ログ出力 End *****************************************************************************

            Return query.GetData()
        End Using
    End Function
    '2018/08/07 TCS 中村 （FS）基幹連携を用いたセールス業務実績入力の検証 START DEL
    '2018/08/07 TCS 中村 （FS）基幹連携を用いたセールス業務実績入力の検証 END
    '2019/12/06 TS  三浦 （FS）e-CRBシステム各種作業効率化の品質検証 END

    ''' <summary>
    ''' 受注時説明登録確認
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <returns>件数</returns>
    ''' <remarks></remarks>
    Public Shared Function IsOrderExplanation(ByVal salesId As Decimal) As Integer
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("IsOrderExplanation_Start")
        'ログ出力 End *****************************************************************************

        Dim ret As Integer = 0
        Dim sql As New StringBuilder()

        Try
            With sql
                .AppendLine(" SELECT /* SC3080202_221 */ ")
                .AppendLine("     1 ")
                .AppendLine(" FROM ")
                .AppendLine("     TB_T_AFTER_ODR T1 ")
                .AppendLine(" WHERE ")
                .AppendLine("     T1.SALES_ID = :SALES_ID ")
                .AppendLine(" UNION ALL ")
                .AppendLine(" SELECT ")
                .AppendLine("     1 ")
                .AppendLine(" FROM ")
                .AppendLine("     TB_H_AFTER_ODR T1 ")
                .AppendLine(" WHERE ")
                .AppendLine("     T1.SALES_ID = :SALES_ID ")
            End With

            Using query As New DBSelectQuery(Of DataTable)("SC3080202_221")

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, salesId)

                ret = query.GetCount()
            End Using
        Finally
            sql.Clear()
        End Try

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("IsOrderExplanation_End")
        'ログ出力 End *****************************************************************************

        Return ret
    End Function

    '2019/12/06 TS  三浦 （FS）e-CRBシステム各種作業効率化の品質検証 START
    ''' <summary>
    ''' 契約車両情報取得(VIN)
    ''' </summary>
    ''' <param name="dlrCd">販売店コード</param>
    ''' <param name="salesBkgNum">注文番号</param>
    ''' <param name="DispFlgActStatus">活動表示フラグ</param>
    ''' <returns>契約車両情報</returns>
    ''' <remarks></remarks>
    Public Shared Function GetContractCarDataVin(ByVal dlrCd As String, _
                                                 ByVal salesBkgNum As String, ByVal DispFlgActStatus As String) As SC3080202DataSet.SC3080202GetContractCarVinDataTable
        '2019/12/06 TS  三浦 （FS）e-CRBシステム各種作業効率化の品質検証 END
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetContractCarDataVin_Start")
        'ログ出力 End *****************************************************************************

        Dim sql As New StringBuilder()

        With sql
            '2019/12/06 TS  三浦 （FS）e-CRBシステム各種作業効率化の品質検証 START
            If DispFlgActStatus.Equals(ACT_STATUS_DISP_FLG_ON) Then
                .Append("  SELECT /* SC3080202_222 */ ")
                .Append("         ASSIGN_TEMP_VCL_VIN  ")
                .Append("    FROM ")
                .Append(" (SELECT ASSIGN_TEMP_VCL_VIN, 1 AS NO ")
                .Append("    FROM TB_T_SPM_AFTER_ODR_CHIP ")
                .Append("   WHERE DLR_CD = :DLR_CD ")
                .Append("     AND SALESBKG_NUM = :SALESBKG_NUM ")
                .Append("   UNION ALL ")
                .Append("  SELECT ASSIGN_TEMP_VCL_VIN, 2 AS NO ")
                .Append("    FROM TB_H_SPM_AFTER_ODR_CHIP ")
                .Append("   WHERE DLR_CD = :DLR_CD ")
                .Append("     AND SALESBKG_NUM = :SALESBKG_NUM ")
                .Append("   UNION ALL ")
                .Append("  SELECT VCL_VIN AS ASSIGN_TEMP_VCL_VIN, 3 AS NO ")
                .Append("    FROM TB_T_SALESBOOKING ")
                .Append("   WHERE DLR_CD = :DLR_CD ")
                .Append("     AND SALESBKG_NUM = :SALESBKG_NUM ")
                .Append("     AND VCL_VIN <> ' ' ) ")
                .Append(" ORDER BY NO ASC")
            Else
                '2018/08/07 TCS 中村 （FS）基幹連携を用いたセールス業務実績入力の検証 UPDATE START
                .Append(" SELECT /* SC3080202_222 */ ")
                .Append("        ASSIGN_TEMP_VCL_VIN ")
                .Append("   FROM TB_T_SPM_AFTER_ODR_CHIP ")
                .Append("  WHERE DLR_CD = :DLR_CD ")
                .Append("    AND SALESBKG_NUM = :SALESBKG_NUM ")
                .Append("  UNION ALL ")
                .Append(" SELECT ASSIGN_TEMP_VCL_VIN ")
                .Append("   FROM TB_H_SPM_AFTER_ODR_CHIP ")
                .Append("  WHERE DLR_CD = :DLR_CD ")
                .Append("    AND SALESBKG_NUM = :SALESBKG_NUM ")
                '2018/08/07 TCS 中村 （FS）基幹連携を用いたセールス業務実績入力の検証 UPDATE END
            End If
            '2019/12/06 TS  三浦 （FS）e-CRBシステム各種作業効率化の品質検証 END
        End With

        Using query As New DBSelectQuery(Of SC3080202DataSet.SC3080202GetContractCarVinDataTable)("SC3080202_221")
            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dlrCd)
            query.AddParameterWithTypeValue("SALESBKG_NUM", OracleDbType.NVarchar2, salesBkgNum)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetContractCarDataVin_End")
            'ログ出力 End *****************************************************************************

            Return query.GetData()
        End Using

    End Function
#End Region
    '2014/02/12 TCS 山口 受注後フォロー機能開発 END

    '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 DEL

    '2016/09/12 TCS 鈴木 性能改善（TR-SLT-TMT-20160726-002）ADD START
#Region "性能改善（TR-SLT-TMT-20160726-002）"
    ''' <summary>
    ''' 商談存在確認
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <returns>取得結果</returns>
    ''' <remarks></remarks>
    Public Shared Function GetSales(ByVal salesId As Decimal) As SC3080202DataSet.SC3080202GetSalesDataTable
        Dim sql As New StringBuilder
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSales")
        'ログ出力 End *****************************************************************************

        With sql
            .AppendLine(" SELECT /* SC3080202_225 */ ")
            .AppendLine("        '0' AS ACTIVE_OR_HIS_FLG, ")
            .AppendLine("        REQ_ID, ")
            .AppendLine("        ATT_ID ")
            .AppendLine("   FROM TB_T_SALES ")
            .AppendLine(" WHERE ")
            .AppendLine("        SALES_ID = :SALES_ID ")
            .AppendLine(" UNION ALL ")
            .AppendLine(" SELECT ")
            .AppendLine("        '1' AS ACTIVE_OR_HIS_FLG, ")
            .AppendLine("        REQ_ID, ")
            .AppendLine("        ATT_ID ")
            .AppendLine("   FROM TB_H_SALES ")
            .AppendLine(" WHERE ")
            .AppendLine("        SALES_ID = :SALES_ID ")
        End With

        Using query As New DBSelectQuery(Of SC3080202DataSet.SC3080202GetSalesDataTable)("SC3080202_225")

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, salesId)
            sql.Clear()

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSales")
            'ログ出力 End *****************************************************************************

            Return query.GetData()
        End Using
    End Function
#End Region
    '2016/09/12 TCS 鈴木 性能改善（TR-SLT-TMT-20160726-002）ADD END

    '2017/11/20 TCS 河原 TKM独自機能開発 START
#Region "TKM独自機能開発"

    ''' <summary>
    ''' 直販フラグ更新更新（商談一時情報）
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <param name="directBillingFlg">直販フラグ</param>
    ''' <param name="updateAccount">更新ユーザ</param>
    ''' <returns>処理結果（True:更新成功/False:更新失敗）</returns>
    ''' <remarks>商談一時情報テーブルのブランド認知理由を更新します。</remarks>
    Public Shared Function UpdateDirectBilling_Temp(ByVal salesId As Decimal, ByVal directBillingFlg As String, ByVal updateAccount As String) As Integer

        Dim ret As Integer = 0
        Dim sql As New StringBuilder(10000)

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateDirectBilling_Temp_Start")
        'ログ出力 End *****************************************************************************

        Try
            With sql
                .AppendLine("UPDATE /* SC3080202_226 */ ")
                .AppendLine("    TB_T_SALES_TEMP ")
                .AppendLine("SET  ")
                .AppendLine("    DIRECT_SALES_FLG = :DIRECT_SALES_FLG, ")
                .AppendLine("    ROW_UPDATE_DATETIME = SYSDATE,  ")
                .AppendLine("    ROW_UPDATE_ACCOUNT = :ACCOUNT,  ")
                .AppendLine("    ROW_UPDATE_FUNCTION = :ROW_UPDATE_FUNCTION ")
                .AppendLine("WHERE  ")
                .AppendLine("    SALES_ID = :SALES_ID ")
            End With

            Using query As New DBUpdateQuery("SC3080202_226")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, salesId)
                query.AddParameterWithTypeValue("DIRECT_SALES_FLG", OracleDbType.NVarchar2, directBillingFlg)
                query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, updateAccount)
                query.AddParameterWithTypeValue("ROW_UPDATE_FUNCTION", OracleDbType.NVarchar2, "SC3080202")
                ret = query.Execute()
            End Using
        Finally
            sql.Clear()
        End Try

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateDirectBilling_Temp_End")
        'ログ出力 End *****************************************************************************

        Return ret

    End Function

    ''' <summary>
    ''' 直販フラグ更新更新（商談）
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <param name="directBillingFlg">直販フラグ</param>
    ''' <param name="updateAccount">更新ユーザ</param>
    ''' <returns>処理結果（True:更新成功/False:更新失敗）</returns>
    ''' <remarks>商談テーブルのブランド認知理由を更新します。</remarks>
    Public Shared Function UpdateDirectBilling_Sales(ByVal salesId As Decimal, ByVal directBillingFlg As String, ByVal updateAccount As String) As Integer

        Dim ret As Integer = 0
        Dim sql As New StringBuilder(10000)

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateDirectBilling_Sales_Start")
        'ログ出力 End *****************************************************************************

        Try
            With sql
                .AppendLine("UPDATE /* SC3080202_227 */ ")
                .AppendLine("    TB_T_SALES ")
                .AppendLine("SET  ")
                .AppendLine("    DIRECT_SALES_FLG = :DIRECT_SALES_FLG, ")
                .AppendLine("    ROW_UPDATE_DATETIME = SYSDATE,  ")
                .AppendLine("    ROW_UPDATE_ACCOUNT = :ACCOUNT,  ")
                .AppendLine("    ROW_UPDATE_FUNCTION = :ROW_UPDATE_FUNCTION ")
                .AppendLine("WHERE  ")
                .AppendLine("    SALES_ID = :SALES_ID ")
            End With

            Using query As New DBUpdateQuery("SC3080202_227")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, salesId)
                query.AddParameterWithTypeValue("DIRECT_SALES_FLG", OracleDbType.NVarchar2, directBillingFlg)
                query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, updateAccount)
                query.AddParameterWithTypeValue("ROW_UPDATE_FUNCTION", OracleDbType.NVarchar2, "SC3080202")
                ret = query.Execute()
            End Using
        Finally
            sql.Clear()
        End Try

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateDirectBilling_Sales_End")
        'ログ出力 End *****************************************************************************

        Return ret

    End Function

    ''' <summary>
    ''' 希望車種サフィックスマスタ取得
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetSelectedSuffixMaster(ByVal dlrcd As String) As SC3080202DataSet.SC3080202GetSuffixMasterDataTable

        Dim sql As New StringBuilder

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSelectedSuffixMaster_Start")
        'ログ出力 End *****************************************************************************

        With sql
            .Append("SELECT ")
            .Append("    /* SC3080202_XXX */ ")
            .Append("    DISTINCT  ")
            .Append("    MODEL_CD, ")
            .Append("    GRADE_CD, ")
            .Append("    SUFFIX_CD, ")
            .Append("    SUFFIX_NAME ")
            .Append("FROM ")
            .Append("    ( ")
            .Append("    SELECT ")
            .Append("        T1.SORT_ORDER, ")
            .Append("        T1.MODEL_CD, ")
            .Append("        T4.GRADE_CD, ")
            .Append("        T5.SUFFIX_CD, ")
            .Append("        T5.SUFFIX_NAME ")
            .Append("    FROM ")
            .Append("        TB_M_MODEL_DLR T1, ")
            .Append("        TB_M_MODEL T2, ")
            .Append("        TB_M_MAKER T3, ")
            .Append("        TB_M_GRADE T4, ")
            .Append("        TB_M_SUFFIX T5 ")
            .Append("    WHERE ")
            .Append("            (T1.DLR_CD = 'XXXXX' OR T1.DLR_CD = :DLRCD) ")
            .Append("        AND (T1.SALES_FROM_DATE = TO_DATE('1900/1/1','YYYY/MM/DD HH24:MI:SS') OR SALES_FROM_DATE <= TRUNC(SYSDATE)) ")
            .Append("        AND (T1.SALES_TO_DATE = TO_DATE('1900/1/1','YYYY/MM/DD HH24:MI:SS') OR SALES_TO_DATE >= TRUNC(SYSDATE)) ")
            .Append("        AND T1.MODEL_CD = T2.MODEL_CD ")
            .Append("        AND T2.MAKER_CD = T3.MAKER_CD ")
            .Append("        AND T2.INUSE_FLG = '1' ")
            .Append("        AND T4.INUSE_FLG = '1' ")
            .Append("        AND T3.MAKER_TYPE = '1' ")
            .Append("        AND T1.MODEL_CD = T4.MODEL_CD ")
            .Append("        AND T5.MODEL_CD = T1.MODEL_CD ")
            .Append("        AND (T5.GRADE_CD = T4.GRADE_CD OR T5.GRADE_CD = 'X') ")
            .Append("        AND T5.INUSE_FLG = '1' ")
            .Append("    ORDER BY ")
            .Append("        T1.SORT_ORDER, ")
            .Append("        T1.MODEL_CD, ")
            .Append("        T4.GRADE_CD, ")
            .Append("        T5.SUFFIX_CD ")
            .Append("    ) ")
        End With

        Using query As New DBSelectQuery(Of SC3080202DataSet.SC3080202GetSuffixMasterDataTable)("SC3080202_XXX")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSelectedSuffixMaster_End")
            'ログ出力 End *****************************************************************************

            Return query.GetData()
        End Using
    End Function

    ''' <summary>
    ''' 希望車種外装色マスタ取得
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetSelectedColorMaster(ByVal dlrcd As String) As SC3080202DataSet.SC3080202GetExteriorColorMasterDataTable

        Dim sql As New StringBuilder

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSelectedColorMaster_Start")
        'ログ出力 End *****************************************************************************
        '2018/04/17 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 START

        '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 START
        'サフィックス使用可否フラグ(設定値が無ければ0)
        Dim suffixIsAvailable As Boolean = False

        Dim systemBiz As New SystemSetting
        Dim dataRow As SystemSettingDataSet.TB_M_SYSTEM_SETTINGRow
        dataRow = systemBiz.GetSystemSetting(USE_FLG_SUFFIX)

        If IsNothing(dataRow) Then
            suffixIsAvailable = False
        ElseIf dataRow.SETTING_VAL.Equals("1") Then
            suffixIsAvailable = True
        Else
            suffixIsAvailable = False
        End If
        '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 END

        '2018/04/17 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 END

        With sql
            .Append("SELECT DISTINCT ")
            .Append("    /* SC3080202_113 */ ")
            .Append("    MODEL_CD AS SERIESCD , ")
            .Append("    GRADE_CD AS VCLMODEL_CODE, ")
            .Append("    SUFFIX_CD , ")
            .Append("    BODYCLR_CD , ")
            .Append("    BODYCLR_NAME AS DISP_BDY_COLOR ")
            .Append("FROM ")
            .Append("    (SELECT ")
            .Append("        T1.SORT_ORDER, ")
            .Append("        T1.MODEL_CD, ")
            .Append("        T4.GRADE_CD, ")
            '2018/04/17 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 START
            'サフィックスコードの取得
            If (suffixIsAvailable) Then
                .Append("        T6.SUFFIX_CD, ")
            Else
                .Append("        ' ' AS SUFFIX_CD, ")
            End If
            .Append("        T5.BODYCLR_CD, ")
            .Append("        T5.BODYCLR_NAME ")
            .Append("    FROM ")
            .Append("        TB_M_MODEL_DLR T1, ")
            .Append("        TB_M_MODEL T2, ")
            .Append("        TB_M_MAKER T3, ")
            .Append("        TB_M_GRADE T4, ")
            'サフィックスマスタとの結合
            If (suffixIsAvailable) Then
                .Append("        TB_M_SUFFIX T6, ")
            End If
            .Append("        TB_M_BODYCOLOR T5 ")
            .Append("    WHERE ")
            .Append("            (T1.DLR_CD = 'XXXXX' OR T1.DLR_CD = :DLRCD) ")
            .Append("        AND (T1.SALES_FROM_DATE = TO_DATE('1900/1/1', 'YYYY/MM/DD HH24:MI:SS') OR SALES_FROM_DATE <= TRUNC(SYSDATE)) ")
            .Append("        AND (T1.SALES_TO_DATE   = TO_DATE('1900/1/1', 'YYYY/MM/DD HH24:MI:SS') OR SALES_TO_DATE   >= TRUNC(SYSDATE)) ")
            .Append("        AND T1.MODEL_CD = T2.MODEL_CD ")
            .Append("        AND T2.MAKER_CD = T3.MAKER_CD ")
            .Append("        AND T2.INUSE_FLG = '1' ")
            .Append("        AND T3.MAKER_TYPE = '1' ")
            .Append("        AND T4.MODEL_CD = T1.MODEL_CD ")
            .Append("        AND T4.INUSE_FLG = '1' ")
            'サフィックスマスタとの結合
            If (suffixIsAvailable) Then
                .Append("        AND T6.MODEL_CD = T1.MODEL_CD ")
                .Append("        AND (T6.GRADE_CD = T4.GRADE_CD OR T6.GRADE_CD = 'X') ")
                .Append("        AND T6.INUSE_FLG = '1' ")
            End If
            .Append("        AND T5.MODEL_CD = T1.MODEL_CD ")
            .Append("        AND ( ")
            'グレードコードの指定
            If (suffixIsAvailable) Then
                .Append("             T5.GRADE_CD = T6.GRADE_CD OR ")
            End If
            .Append("             T5.GRADE_CD = T4.GRADE_CD OR T5.GRADE_CD = 'X' ")
            .Append("            ) ")
            'サフィックスコードの指定
            If (suffixIsAvailable) Then
                .Append("        AND (T5.SUFFIX_CD = T6.SUFFIX_CD OR T5.SUFFIX_CD = 'X') ")
            End If
            '2018/04/17 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 END
            .Append("        AND T5.INUSE_FLG = '1' ")
            .Append("    ORDER BY ")
            .Append("        T1.SORT_ORDER, ")
            .Append("        T1.MODEL_CD, ")
            .Append("        T4.GRADE_CD, ")
            .Append("        T5.BODYCLR_CD) ")
        End With

        Using query As New DBSelectQuery(Of SC3080202DataSet.SC3080202GetExteriorColorMasterDataTable)("SC3080202_113")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSelectedColorMaster_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 徐 2013/10対応版　既存流用 END

            Return query.GetData()
        End Using
    End Function

    ''' <summary>
    ''' 希望車種内装色マスタ取得
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetSelectedInteriorColorMaster(ByVal dlrcd As String) As SC3080202DataSet.SC3080202GetInteriorColorMasterDataTable

        Dim sql As New StringBuilder

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSelectedInteriorColorMaster_Start")
        'ログ出力 End *****************************************************************************
        '2018/04/17 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 START
        '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 START
        'サフィックス使用可否フラグ(設定値が無ければ0)
        Dim suffixIsAvailable As Boolean = False
        Dim systemBiz As New SystemSetting
        Dim dataRow As SystemSettingDataSet.TB_M_SYSTEM_SETTINGRow
        dataRow = systemBiz.GetSystemSetting(USE_FLG_SUFFIX)

        If IsNothing(dataRow) Then
            suffixIsAvailable = False
        ElseIf dataRow.SETTING_VAL.Equals("1") Then
            suffixIsAvailable = True
        Else
            suffixIsAvailable = False
        End If
        '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 END
        '2018/04/17 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 END

        With sql
            .Append("SELECT ")
            .Append("    /* SC3080202_XXX */ ")
            .Append("    DISTINCT  ")
            .Append("    MODEL_CD, ")
            .Append("    GRADE_CD, ")
            .Append("    SUFFIX_CD, ")
            .Append("    BODYCLR_CD, ")
            .Append("    INTERIORCLR_CD, ")
            .Append("    INTERIORCLR_NAME ")
            .Append("FROM ")
            .Append("    ( ")
            .Append("    SELECT ")
            .Append("        T1.SORT_ORDER, ")
            .Append("        T1.MODEL_CD, ")
            .Append("        T4.GRADE_CD, ")
            '2018/04/17 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 START
            'サフィックスコードの取得
            If (suffixIsAvailable) Then
                .Append("        T5.SUFFIX_CD, ")
            Else
                .Append("        ' ' AS SUFFIX_CD, ")
            End If
            .Append("        T6.BODYCLR_CD, ")
            .Append("        T7.INTERIORCLR_CD, ")
            .Append("        T7.INTERIORCLR_NAME ")
            .Append("    FROM ")
            .Append("        TB_M_MODEL_DLR T1, ")
            .Append("        TB_M_MODEL T2, ")
            .Append("        TB_M_MAKER T3, ")
            .Append("        TB_M_GRADE T4, ")
            'サフィックスマスタとの結合
            If (suffixIsAvailable) Then
                .Append("        TB_M_SUFFIX T5, ")
            End If
            .Append("        TB_M_BODYCOLOR T6, ")
            .Append("        TB_M_INTERIORCOLOR T7 ")
            .Append("    WHERE ")
            .Append("            (T1.DLR_CD = 'XXXXX' OR T1.DLR_CD = :DLRCD) ")
            .Append("        AND (T1.SALES_FROM_DATE = TO_DATE('1900/1/1','YYYY/MM/DD HH24:MI:SS') OR SALES_FROM_DATE <= TRUNC(SYSDATE)) ")
            .Append("        AND (T1.SALES_TO_DATE = TO_DATE('1900/1/1','YYYY/MM/DD HH24:MI:SS') OR SALES_TO_DATE >= TRUNC(SYSDATE)) ")
            .Append("        AND T1.MODEL_CD = T2.MODEL_CD ")
            .Append("        AND T2.MAKER_CD = T3.MAKER_CD ")
            .Append("        AND T2.INUSE_FLG = '1' ")
            .Append("        AND T4.INUSE_FLG = '1' ")
            .Append("        AND T3.MAKER_TYPE = '1' ")
            .Append("        AND T1.MODEL_CD = T4.MODEL_CD ")
            'サフィックスマスタとの結合
            If (suffixIsAvailable) Then
                .Append("        AND T5.MODEL_CD = T1.MODEL_CD ")
                .Append("        AND (T5.GRADE_CD = T4.GRADE_CD OR T5.GRADE_CD = 'X') ")
                .Append("        AND T5.INUSE_FLG = '1' ")
            End If
            '外装色マスタとの結合
            If (suffixIsAvailable) Then
                .Append("        AND T6.MODEL_CD = T5.MODEL_CD ")
                .Append("        AND ( ")
                .Append("             T6.GRADE_CD = T5.GRADE_CD OR ")
                .Append("             T6.GRADE_CD = T4.GRADE_CD OR T6.GRADE_CD = 'X' ")
                .Append("            ) ")
                .Append("        AND T6.INUSE_FLG = '1' ")
                .Append("        AND ( ")
                .Append("             T6.SUFFIX_CD = T5.SUFFIX_CD OR ")
                .Append("             T6.SUFFIX_CD = 'X' ")
                .Append("            ) ")
            Else
                .Append("        AND T6.MODEL_CD = T4.MODEL_CD ")
                .Append("        AND ( ")
                .Append("             T6.GRADE_CD = T4.GRADE_CD OR T6.GRADE_CD = 'X' ")
                .Append("            ) ")
                .Append("        AND T6.INUSE_FLG = '1' ")
            End If
            .Append("        AND T7.MODEL_CD = T6.MODEL_CD ")
            .Append("        AND ( ")
            .Append("             T7.GRADE_CD = T6.GRADE_CD OR ")
            'グレードコードの指定
            If (suffixIsAvailable) Then
                .Append("             T7.GRADE_CD = T5.GRADE_CD OR ")
            End If
            .Append("             T7.GRADE_CD = T4.GRADE_CD OR T7.GRADE_CD = 'X' ")
            .Append("            ) ")
            .Append("        AND T7.INUSE_FLG = '1' ")
            'サフィックスコードの指定
            If (suffixIsAvailable) Then
                .Append("        AND (T7.SUFFIX_CD = T6.SUFFIX_CD OR T7.SUFFIX_CD = T5.SUFFIX_CD OR T7.SUFFIX_CD = 'X') ")
            End If
            .Append("        AND (T7.BODYCLR_CD = T6.BODYCLR_CD OR T7.BODYCLR_CD = 'X') ")

            '2018/04/17 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 END
            .Append("    ORDER BY ")
            .Append("        T1.SORT_ORDER, ")
            .Append("        T1.MODEL_CD, ")
            .Append("        T4.GRADE_CD, ")
            .Append("        T6.BODYCLR_CD ")
            .Append("    ) ")
        End With

        Using query As New DBSelectQuery(Of SC3080202DataSet.SC3080202GetInteriorColorMasterDataTable)("SC3080202_XXX")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSelectedInteriorColorMaster_End")
            'ログ出力 End *****************************************************************************

            Return query.GetData()
        End Using
    End Function

    ''' <summary>
    ''' 選択車種編集
    ''' </summary>
    ''' <param name="fllwupboxseqno">商談ID</param>
    ''' <param name="seriescd">モデルコード</param>
    ''' <param name="modelcd">グレードコード</param>
    ''' <param name="colorcd">外鈑色コード</param>
    ''' <param name="account">更新ユーザアカウント</param>
    ''' <param name="salesProspectCd">商談見込み度コード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function UpdateSelectedSeries(ByVal fllwupboxseqno As String,
                                         ByVal seqno As Decimal,
                                         ByVal modelcd As String,
                                         ByVal gradecd As String,
                                         ByVal suffixcd As String,
                                         ByVal exteriorColorcd As String,
                                         ByVal interiorColorcd As String,
                                         ByVal lockvr As Long,
                                         ByVal account As String,
                                         ByVal salesProspectCd As String) As Integer

        Dim sql As New StringBuilder

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateSelectedSeries_Start")
        'ログ出力 End *****************************************************************************

        With sql
            .Append("UPDATE ")
            .Append("    /* SC3080202_122 */ ")
            .Append("    TB_T_PREFER_VCL ")
            .Append("SET ")
            .Append("    MODEL_CD = :MODEL_CD, ")
            .Append("    GRADE_CD = :GRADE_CD, ")
            .Append("    SUFFIX_CD = :SUFFIX_CD, ")
            .Append("    BODYCLR_CD = :BODYCLR_CD, ")
            .Append("    INTERIORCLR_CD = :INTERIORCLR_CD, ")
            .Append("    SALES_PROSPECT_CD = :SALES_PROSPECT_CD, ")
            .Append("    ROW_UPDATE_DATETIME = SYSDATE, ")
            .Append("    ROW_UPDATE_ACCOUNT = :ACCOUNT, ")
            .Append("    ROW_UPDATE_FUNCTION = 'SC3080202', ")
            .Append("    ROW_LOCK_VERSION = :LOCKVR + 1 ")
            .Append("WHERE ")
            .Append("        SALES_ID = :FLLWUPBOX_SEQNO ")
            .Append("    AND PREF_VCL_SEQ = :SEQNO ")
            .Append("    AND ROW_LOCK_VERSION = :LOCKVR ")
        End With
        Using query As New DBUpdateQuery("SC3080202_122")

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupboxseqno)
            query.AddParameterWithTypeValue("SEQNO", OracleDbType.Decimal, seqno)
            query.AddParameterWithTypeValue("MODEL_CD", OracleDbType.NVarchar2, modelcd)
            query.AddParameterWithTypeValue("GRADE_CD", OracleDbType.NVarchar2, gradecd)
            query.AddParameterWithTypeValue("SUFFIX_CD", OracleDbType.NVarchar2, suffixcd)
            query.AddParameterWithTypeValue("BODYCLR_CD", OracleDbType.NVarchar2, exteriorColorcd)
            query.AddParameterWithTypeValue("INTERIORCLR_CD", OracleDbType.NVarchar2, interiorColorcd)
            query.AddParameterWithTypeValue("LOCKVR", OracleDbType.Int64, lockvr)
            query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, account)
            query.AddParameterWithTypeValue("SALES_PROSPECT_CD", OracleDbType.NVarchar2, salesProspectCd)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateSelectedSeries_End")
            'ログ出力 End *****************************************************************************

            Return query.Execute()
        End Using
    End Function

    ''' <summary>
    ''' 商談テーブルロック処理
    ''' </summary>
    ''' <param name="fllwupbox_seqno">商談ID </param>
    ''' <remarks></remarks>
    Public Shared Sub SelectSalesHisLock(ByVal fllwupbox_seqno As String)

        Using query As New DBSelectQuery(Of DataTable)("SC3080202_127")

            Dim env As New SystemEnvSetting
            Dim sqlForUpdate As String = " FOR UPDATE WAIT " + env.GetLockWaitTime()
            Dim sql As New StringBuilder

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SelectSalesHisLock_Start")
            'ログ出力 End *****************************************************************************

            With sql
                .Append("SELECT ")
                .Append("  /* SC3080202_127 */ ")
                .Append("1 ")
                .Append("FROM ")
                .Append("  TB_H_SALES ")
                .Append("WHERE ")
                .Append("  SALES_ID = :FLLWUPBOX_SEQNO ")
                .Append(sqlForUpdate)
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupbox_seqno)
            query.GetData()
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SelectSalesHisLock_End")
            'ログ出力 End *****************************************************************************

        End Using

    End Sub

#End Region
    '2017/11/20 TCS 河原 TKM独自機能開発 END

    '2018/04/17 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 START
#Region "（トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証"

    '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 DEL

#End Region
    '2018/04/17 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 END

#Region "TKMローカル"

    '2018/06/01 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 START
    ''' <summary>
    ''' 購入分類マスタローカル取得
    ''' </summary>
    ''' <returns>取得結果</returns>
    ''' <remarks></remarks>
    Public Shared Function GetDemandStructureLocal() As SC3080202DataSet.SC3080202GetDemandStructureLocalDataTable

        Dim sql As New StringBuilder

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetDemandStructureLocal_Start")
        'ログ出力 End *****************************************************************************

        With sql
            .Append("SELECT ")
            .Append("    /* SC3080202_229 */ ")
            .Append("    DEMAND_STRUCTURE_CD , ")
            .Append("    DEMAND_STRUCTURE_NAME , ")
            .Append("    TRADEINCAR_ENABLED_FLG , ")
            .Append("    SORT_ORDER ")
            .Append("FROM ")
            .Append("    TB_LM_DEMAND_STRUCTURE ")
            .Append("WHERE ")
            .Append("    INUSE_FLG = '1' ")
            .Append("ORDER BY ")
            .Append("    SORT_ORDER ")
        End With

        Using query As New DBSelectQuery(Of SC3080202DataSet.SC3080202GetDemandStructureLocalDataTable)("SC3080202_229")
            query.CommandText = sql.ToString()
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetDemandStructureLocal_End")
            'ログ出力 End *****************************************************************************

            Return query.GetData()
        End Using
    End Function

    ''' <summary>
    ''' 下取り車両メーカーマスタローカル取得
    ''' </summary>
    ''' <returns>取得結果</returns>
    ''' <remarks></remarks>
    Public Shared Function GetTradeincarMakerLocal() As SC3080202DataSet.SC3080202GetTradeincarMakerLocalDataTable

        Dim sql As New StringBuilder

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetTradeincarMakerLocal_Start")
        'ログ出力 End *****************************************************************************

        With sql
            .Append("SELECT ")
            .Append("    /* SC3080202_230 */ ")
            .Append("    MAKER_CD , ")
            .Append("    MAKER_NAME , ")
            .Append("    SORT_ORDER ")
            .Append("FROM ")
            .Append("    TB_M_MAKER ")
            .Append("ORDER BY ")
            .Append("    SORT_ORDER ")
        End With

        Using query As New DBSelectQuery(Of SC3080202DataSet.SC3080202GetTradeincarMakerLocalDataTable)("SC3080202_230")
            query.CommandText = sql.ToString()

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetTradeincarMakerLocal_End")
            'ログ出力 End *****************************************************************************

            Return query.GetData()
        End Using
    End Function

    ''' <summary>
    ''' 下取り車両モデルマスタローカル取得
    ''' </summary>
    ''' <returns>取得結果</returns>
    ''' <remarks></remarks>
    Public Shared Function GetTradeincarModelLocal(ByVal tradeincar_maker_cd As String) As SC3080202DataSet.SC3080202GetTradeincarModelLocalDataTable

        Dim sql As New StringBuilder

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetTradeincarModelLocal_Start")
        'ログ出力 End *****************************************************************************

        With sql
            .Append("SELECT ")
            .Append("    /* SC3080202_231 */ ")
            .Append("    MAKER_CD , ")
            .Append("    MODEL_CD , ")
            .Append("    MODEL_NAME ")
            .Append("FROM ")
            .Append("    TB_M_MODEL ")
            .Append("WHERE ")
            .Append("    MAKER_CD = :TRADEINCAR_MAKER_CD ")
            .Append("AND INUSE_FLG = '1' ")
            .Append("ORDER BY ")
            .Append("    MODEL_NAME ")
        End With

        Using query As New DBSelectQuery(Of SC3080202DataSet.SC3080202GetTradeincarModelLocalDataTable)("SC3080202_231")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("TRADEINCAR_MAKER_CD", OracleDbType.NVarchar2, tradeincar_maker_cd)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetTradeincarModelLocal_End")
            'ログ出力 End *****************************************************************************

            Return query.GetData()
        End Using
    End Function

    ''' <summary>
    ''' 商談ローカル取得
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <returns>取得結果</returns>
    ''' <remarks></remarks>
    Public Shared Function GetSalesLocal(ByVal salesId As Decimal) As SC3080202DataSet.SC3080202GetSalesLocalDataTable

        Dim sql As New StringBuilder

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetSalesLocal_Start")
        'ログ出力 End *****************************************************************************

        With sql
            .Append(" SELECT ")
            .Append("     /* SC3080202_232 */ ")
            .Append("     A.SALES_ID , ")
            .Append("     A.DEMAND_STRUCTURE_CD , ")
            .Append("     A.TRADEINCAR_MAKER_CD , ")
            .Append("     A.TRADEINCAR_MODEL_CD , ")
            .Append("     A.TRADEINCAR_MILE ,  ")
            .Append("     A.TRADEINCAR_MODEL_YEAR, ")
            .Append("     A.ROW_LOCK_VERSION , ")
            .Append("     B.MAKER_NAME, ")
            .Append("     C.MODEL_NAME ")
            .Append(" FROM  ")
            .Append("     TB_LT_SALES A, ")
            .Append("     TB_M_MAKER B, ")
            .Append("     TB_M_MODEL C ")
            .Append(" WHERE ")
            .Append("     A.SALES_ID = :SALES_ID ")
            .Append(" AND B.MAKER_CD(+) = A.TRADEINCAR_MAKER_CD ")
            .Append(" AND C.MAKER_CD(+) = A.TRADEINCAR_MAKER_CD ")
            .Append(" AND C.MODEL_CD(+) = A.TRADEINCAR_MODEL_CD ")
        End With

        Using query As New DBSelectQuery(Of SC3080202DataSet.SC3080202GetSalesLocalDataTable)("SC3080202_232")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, salesId)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetSalesLocal_End")
            'ログ出力 End *****************************************************************************

            Return query.GetData()
        End Using
    End Function

    ''' <summary>
    ''' 商談ローカルテーブルロック処理
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <remarks></remarks>
    Public Shared Sub SelectSalesLocalLock(ByVal salesId As Decimal)

        Using query As New DBSelectQuery(Of DataTable)("SC3080202_233")

            Dim env As New SystemEnvSetting
            Dim sqlForUpdate As String = " FOR UPDATE WAIT " + env.GetLockWaitTime()
            Dim sql As New StringBuilder

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("SelectSalesLocalLock_Start")
            'ログ出力 End *****************************************************************************

            With sql
                .Append("SELECT ")
                .Append("  /* SC3080202_233 */ ")
                .Append("1 ")
                .Append("FROM ")
                .Append("  TB_LT_SALES ")
                .Append("WHERE ")
                .Append("  SALES_ID = :SALES_ID ")
                .Append(sqlForUpdate)
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, salesId)
            query.GetData()
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("SelectSalesLocalLock_End")
            'ログ出力 End *****************************************************************************

        End Using
    End Sub

    ''' <summary>
    ''' 商談ローカル更新 
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <param name="demandCd">購入分類コード</param>
    ''' <param name="tradeinMakerCd">下取り車両メーカーコード</param>
    ''' <param name="tradeinModelCd">下取り車両モデルコード</param>
    ''' <param name="mile">走行距離</param>
    ''' <param name="modelYear">年式</param>
    ''' <param name="account">スタッフコード</param>
    ''' <param name="funcId">機能ID</param>
    ''' <param name="lockvr">行ロックバージョン</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Shared Function UpdateSalesLocal(ByVal salesId As Decimal,
                                     ByVal demandCd As String,
                                     ByVal tradeinMakerCd As String,
                                     ByVal tradeinModelCd As String,
                                     ByVal mile As Double,
                                     ByVal modelYear As String,
                                     ByVal account As String,
                                     ByVal funcId As String,
                                     ByVal lockvr As Long) As Integer

        Dim sql As New StringBuilder

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("UpdateSalesLocal_Start")
        'ログ出力 End *****************************************************************************

        With sql
            .Append("UPDATE ")
            .Append("    /* SC3080202_234 */ ")
            .Append("    TB_LT_SALES ")
            .Append("SET ")
            .Append("    DEMAND_STRUCTURE_CD = :DEMAND_STRUCTURE_CD, ")
            .Append("    TRADEINCAR_MAKER_CD = :TRADEINCAR_MAKER_CD, ")
            .Append("    TRADEINCAR_MODEL_CD = :TRADEINCAR_MODEL_CD, ")
            .Append("    TRADEINCAR_MILE = :TRADEINCAR_MILE, ")
            .Append("    TRADEINCAR_MODEL_YEAR = :TRADEINCAR_MODEL_YEAR, ")
            .Append("    ROW_UPDATE_DATETIME = SYSDATE, ")
            .Append("    ROW_UPDATE_ACCOUNT = :ACCOUNT, ")
            .Append("    ROW_UPDATE_FUNCTION = :FUNC_ID, ")
            .Append("    ROW_LOCK_VERSION = :LOCKVR + 1 ")
            .Append("WHERE ")
            .Append("        SALES_ID = :SALES_ID ")
            .Append("    AND ROW_LOCK_VERSION = :LOCKVR ")
        End With
        Using query As New DBUpdateQuery("SC3080202_234")

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DEMAND_STRUCTURE_CD", OracleDbType.NVarchar2, demandCd)
            query.AddParameterWithTypeValue("TRADEINCAR_MAKER_CD", OracleDbType.NVarchar2, tradeinMakerCd)
            query.AddParameterWithTypeValue("TRADEINCAR_MODEL_CD", OracleDbType.NVarchar2, tradeinModelCd)
            query.AddParameterWithTypeValue("TRADEINCAR_MILE", OracleDbType.Double, mile)
            query.AddParameterWithTypeValue("TRADEINCAR_MODEL_YEAR", OracleDbType.NVarchar2, modelYear)
            query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, account)
            query.AddParameterWithTypeValue("FUNC_ID", OracleDbType.NVarchar2, funcId)
            query.AddParameterWithTypeValue("LOCKVR", OracleDbType.Long, lockvr)
            query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, salesId)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("UpdateSalesLocal_End")
            'ログ出力 End *****************************************************************************

            Return query.Execute()
        End Using
    End Function

    ''' <summary>
    ''' 商談ローカル追加
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <param name="demandCd">購入分類コード</param>
    ''' <param name="tradeinMakerCd">下取り車両メーカーコード</param>
    ''' <param name="tradeinModelCd">下取り車両モデルコード</param>
    ''' <param name="mile">走行距離</param>
    ''' <param name="modelYear">年式</param>
    ''' <param name="account">スタッフコード</param>
    ''' <param name="funcId">機能ID</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Shared Function AddSalesLocal(ByVal salesId As Decimal,
                                  ByVal demandCd As String,
                                  ByVal tradeinMakerCd As String,
                                  ByVal tradeinModelCd As String,
                                  ByVal mile As Double,
                                  ByVal modelYear As String,
                                  ByVal account As String,
                                  ByVal funcId As String) As Integer

        Dim sql As New StringBuilder

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("AddSalesLocal_Start")
        'ログ出力 End *****************************************************************************

        With sql
            .Append("INSERT ")
            .Append("    /* SC3080202_235 */ ")
            .Append("INTO TB_LT_SALES ( ")
            .Append("    SALES_ID, ")
            .Append("    DEMAND_STRUCTURE_CD, ")
            .Append("    TRADEINCAR_MAKER_CD, ")
            .Append("    TRADEINCAR_MODEL_CD, ")
            .Append("    TRADEINCAR_MILE, ")
            .Append("    TRADEINCAR_MODEL_YEAR, ")
            .Append("    ROW_CREATE_DATETIME, ")
            .Append("    ROW_CREATE_ACCOUNT, ")
            .Append("    ROW_CREATE_FUNCTION, ")
            .Append("    ROW_UPDATE_DATETIME, ")
            .Append("    ROW_UPDATE_ACCOUNT, ")
            .Append("    ROW_UPDATE_FUNCTION, ")
            .Append("    ROW_LOCK_VERSION ")
            .Append(") ")
            .Append("VALUES ( ")
            .Append("    :SALES_ID, ")
            .Append("    :DEMAND_STRUCTURE_CD, ")
            .Append("    :TRADEINCAR_MAKER_CD, ")
            .Append("    :TRADEINCAR_MODEL_CD, ")
            .Append("    :TRADEINCAR_MILE, ")
            .Append("    :TRADEINCAR_MODEL_YEAR, ")
            .Append("    SYSDATE, ")
            .Append("    :ACCOUNT, ")
            .Append("    :FUNC_ID, ")
            .Append("    SYSDATE, ")
            .Append("    :ACCOUNT, ")
            .Append("    :FUNC_ID, ")
            .Append("    0 ")
            .Append(") ")
        End With

        Using query As New DBUpdateQuery("SC3080202_235")

            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, salesId)
            query.AddParameterWithTypeValue("DEMAND_STRUCTURE_CD", OracleDbType.NVarchar2, demandCd)
            query.AddParameterWithTypeValue("TRADEINCAR_MAKER_CD", OracleDbType.NVarchar2, tradeinMakerCd)
            query.AddParameterWithTypeValue("TRADEINCAR_MODEL_CD", OracleDbType.NVarchar2, tradeinModelCd)
            query.AddParameterWithTypeValue("TRADEINCAR_MILE", OracleDbType.Double, mile)
            query.AddParameterWithTypeValue("TRADEINCAR_MODEL_YEAR", OracleDbType.NVarchar2, modelYear)
            query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, account)
            query.AddParameterWithTypeValue("FUNC_ID", OracleDbType.NVarchar2, funcId)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("AddSalesLocal_End")
            'ログ出力 End *****************************************************************************

            Return query.Execute()
        End Using
    End Function
    '2018/06/01 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 END

    '2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)追加 start
#Region "用件ソース(2nd)"

    ''' <summary>
    ''' 選択された用件ソース（1st）に紐づく用件ソース（2nd）のマスタ取得
    ''' </summary>
    ''' <param name="dlrCd">販売店コード</param>
    ''' <param name="brnCd">店舗コード</param>
    ''' <param name="source1Cd">紐づくソース1のコード</param>
    ''' <returns></returns>
    ''' <remarks>活動きっかけ（用件ソース2nd）のマスタを取得します。</remarks>
    Public Shared Function GetSource2Master(ByVal dlrCd As String, ByVal brnCd As String _
                                                 , ByVal source1Cd As Long) As SC3080202DataSet.SC3080202Sources2OfACardMasterDataTable

        Dim ret As SC3080202DataSet.SC3080202Sources2OfACardMasterDataTable = Nothing
        Dim sql As New StringBuilder(10000)

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetSource2Master_Start")
        'ログ出力 End *****************************************************************************

        Try
            With sql
                .AppendLine("SELECT /* SC3080202_236 */ ")
                .AppendLine("    SOURCE_2_CD ")
                .AppendLine("    ,REQ_SECOND_CAT_NAME     ")
                .AppendLine("FROM  ")
                .AppendLine("    TB_M_SOURCE_2  ")
                .AppendLine("WHERE  ")
                .AppendLine("    SOURCE_1_CD = :SOURCE_1_CD ")
                .AppendLine("    AND INUSE_FLG = '1' ")
                .AppendLine("    AND SEL_FLG = '1' ")
                .AppendLine("    AND (DLR_CD = :DLR_CD OR DLR_CD = 'XXXXX') ")
                .AppendLine("    AND (BRN_CD = :BRN_CD OR BRN_CD = 'XXX') ")
                .AppendLine("ORDER BY ")
                .AppendLine("    SORT_ORDER ASC ")
            End With

            Using query As New DBSelectQuery(Of SC3080202DataSet.SC3080202Sources2OfACardMasterDataTable)("SC3080202_236")

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dlrCd)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, brnCd)
                query.AddParameterWithTypeValue("SOURCE_1_CD", OracleDbType.Long, source1Cd)

                ret = query.GetData()

            End Using
        Finally
            sql.Clear()
        End Try

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetSource2Master_End")
        'ログ出力 End *****************************************************************************

        Return ret

    End Function

    ''' <summary>
    ''' 用件ソース2nd更新（用件）
    ''' </summary>
    ''' <param name="requestId">用件ID</param>
    ''' <param name="sources2Cd">用件ソース2ndID</param>
    ''' <param name="updateAccount">更新ユーザ</param>
    ''' <returns>処理結果（True:更新成功/False:更新失敗）</returns>
    ''' <remarks>用件テーブルの用件ソース2stを更新します。</remarks>
    Public Shared Function UpdateSource2_Requesst(ByVal requestId As Decimal, ByVal sources2Cd As Long _
                                                      , ByVal updateAccount As String) As Integer

        Dim ret As Integer = 0
        Dim sql As New StringBuilder(10000)
        Dim env As New SystemEnvSetting()

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("UpdateSource2_Requesst_Start")
        'ログ出力 End *****************************************************************************

        Try
            With sql
                .AppendLine("UPDATE /* SC3080202_237 */ ")
                .AppendLine("    TB_T_REQUEST ")
                .AppendLine("SET  ")
                .AppendLine("    SOURCE_2_CD = :SOURCE_2_CD, ")
                .AppendLine("    ROW_UPDATE_DATETIME = SYSDATE,  ")
                .AppendLine("    ROW_UPDATE_ACCOUNT = :ACCOUNT,  ")
                .AppendLine("    ROW_UPDATE_FUNCTION = 'SC3080202',  ")
                .AppendLine("    ROW_LOCK_VERSION = ROW_LOCK_VERSION + 1  ")
                .AppendLine("WHERE  ")
                .AppendLine("    REQ_ID = :REQ_ID ")
            End With

            Using query As New DBUpdateQuery("SC3080202_237")

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("SOURCE_2_CD", OracleDbType.Long, sources2Cd)
                query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, updateAccount)
                query.AddParameterWithTypeValue("REQ_ID", OracleDbType.Decimal, requestId)

                ret = query.Execute()
            End Using
        Finally
            sql.Clear()
            env = Nothing
        End Try

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("UpdateSource2_Requesst_End")
        'ログ出力 End *****************************************************************************

        Return ret

    End Function

    ''' <summary>
    ''' 用件ソース2st更新（誘致）
    ''' </summary>
    ''' <param name="attractId">誘致ID</param>
    ''' <param name="sources2Cd">用件ソース2ndID</param>
    ''' <param name="updateAccount">更新ユーザ</param>
    ''' <returns>処理結果（True:更新成功/False:更新失敗）</returns>
    ''' <remarks>誘致テーブルの用件ソース2ndを更新します。</remarks>
    Public Shared Function UpdateSource2_Attract(ByVal attractId As Decimal, ByVal sources2Cd As Long _
                                                      , ByVal updateAccount As String) As Integer

        Dim ret As Integer = 0
        Dim sql As New StringBuilder(10000)
        Dim env As New SystemEnvSetting()

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("UpdateSource2_Attract_Start")
        'ログ出力 End *****************************************************************************

        Try
            With sql
                .AppendLine("UPDATE /* SC3080202_238 */ ")
                .AppendLine("     TB_T_ATTRACT ")
                .AppendLine("SET  ")
                .AppendLine("    SOURCE_2_CD = :SOURCE_2_CD, ")
                .AppendLine("    ROW_UPDATE_DATETIME = SYSDATE,  ")
                .AppendLine("    ROW_UPDATE_ACCOUNT = :ACCOUNT,  ")
                .AppendLine("    ROW_UPDATE_FUNCTION = 'SC3080202',  ")
                .AppendLine("    ROW_LOCK_VERSION = ROW_LOCK_VERSION + 1  ")
                .AppendLine("WHERE  ")
                .AppendLine("      ATT_ID = :ATT_ID ")
            End With

            Using query As New DBUpdateQuery("SC3080202_238")

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("SOURCE_2_CD", OracleDbType.Long, sources2Cd)
                query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, updateAccount)
                query.AddParameterWithTypeValue("ATT_ID", OracleDbType.Decimal, attractId)

                ret = query.Execute()
            End Using
        Finally
            sql.Clear()
            env = Nothing
        End Try

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("UpdateSource2_Attract_End")
        'ログ出力 End *****************************************************************************

        Return ret

    End Function

    ''' <summary>
    ''' 商談ローカル更新 (用件ソース2専用）
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <param name="sources2Cd">用件ソース2ndID</param>
    ''' <param name="updateAccount">スタッフコード</param>
    ''' <param name="rowlock">行ロックバージョン</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Shared Function UpdatedSource2_Local(ByVal salesId As Decimal,
                                     ByVal sources2Cd As Long,
                                     ByVal updateAccount As String,
                                     ByVal rowlock As Decimal) As Integer

        Dim sql As New StringBuilder

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("UpdatedSource2_Local_Start")
        'ログ出力 End *****************************************************************************

        With sql
            .Append("UPDATE ")
            .Append("    /* SC3080202_239 */ ")
            .Append("    TB_LT_SALES ")
            .Append("SET ")
            .Append("    SOURCE_2_CD = :SOURCE_2_CD, ")
            .Append("    ROW_UPDATE_DATETIME = SYSDATE, ")
            .Append("    ROW_UPDATE_ACCOUNT = :ACCOUNT, ")
            .Append("    ROW_UPDATE_FUNCTION = 'SC3080202', ")
            .Append("    ROW_LOCK_VERSION = :LOCKVR + 1  ")
            .Append("WHERE ")
            .Append("        SALES_ID = :SALES_ID ")
            .Append("    AND ROW_LOCK_VERSION = :LOCKVR ")
        End With
        Using query As New DBUpdateQuery("SC3080202_239")

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("SOURCE_2_CD", OracleDbType.Long, sources2Cd)
            query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, updateAccount)
            query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, salesId)
            query.AddParameterWithTypeValue("LOCKVR", OracleDbType.Decimal, rowlock)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("UpdatedSource2_Local_End")
            'ログ出力 End *****************************************************************************

            Return query.Execute()
        End Using
    End Function

    ''' <summary>
    ''' 商談ローカル更新 (用件ソース1編集フラグ専用）
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <param name="updateAccount">スタッフコード</param>
    ''' <param name="rowlock">行ロックバージョン</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Shared Function UpdatedSource1Flg_Local(ByVal salesId As Decimal,
                                     ByVal updateAccount As String, ByVal rowlock As Decimal) As Integer

        Dim sql As New StringBuilder

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("UpdatedSource1Flg_Local_Start")
        'ログ出力 End *****************************************************************************

        With sql
            .Append("UPDATE ")
            .Append("    /* SC3080202_240 */ ")
            .Append("    TB_LT_SALES ")
            .Append("SET ")
            .Append("    SOURCE_1_CHG_POSSIBLE_FLG = '1', ")
            .Append("    ROW_UPDATE_DATETIME = SYSDATE, ")
            .Append("    ROW_UPDATE_ACCOUNT = :ACCOUNT, ")
            .Append("    ROW_UPDATE_FUNCTION = 'SC3080202', ")
            .Append("    ROW_LOCK_VERSION = ROW_LOCK_VERSION + 1  ")
            .Append("WHERE ")
            .Append("        SALES_ID = :SALES_ID ")
            .Append("    AND ROW_LOCK_VERSION = :LOCKVR ")
        End With
        Using query As New DBUpdateQuery("SC3080202_240")

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, updateAccount)
            query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, salesId)
            query.AddParameterWithTypeValue("LOCKVR", OracleDbType.Decimal, rowlock)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("UpdatedSource1Flg_Local_End")
            'ログ出力 End *****************************************************************************

            Return query.Execute()
        End Using
    End Function

    ''' <summary>
    ''' 商談ローカル更新 (用件ソース2編集フラグ専用）
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <param name="updateAccount">スタッフコード</param>
    ''' <param name="rowlock">行ロックバージョン</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Shared Function UpdatedSource2Flg_Local(ByVal salesId As Decimal,
                                     ByVal updateAccount As String, ByVal rowlock As Decimal) As Integer

        Dim sql As New StringBuilder

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("UpdatedSource2Flg_Local_Start")
        'ログ出力 End *****************************************************************************

        With sql
            .Append("UPDATE ")
            .Append("    /* SC3080202_241 */ ")
            .Append("    TB_LT_SALES ")
            .Append("SET ")
            .Append("    SOURCE_2_CHG_POSSIBLE_FLG = '1', ")
            .Append("    ROW_UPDATE_DATETIME = SYSDATE, ")
            .Append("    ROW_UPDATE_ACCOUNT = :ACCOUNT, ")
            .Append("    ROW_UPDATE_FUNCTION = 'SC3080202', ")
            .Append("    ROW_LOCK_VERSION = ROW_LOCK_VERSION + 1  ")
            .Append("WHERE ")
            .Append("        SALES_ID = :SALES_ID ")
            .Append("    AND ROW_LOCK_VERSION = :LOCKVR ")
        End With
        Using query As New DBUpdateQuery("SC3080202_241")

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, updateAccount)
            query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, salesId)
            query.AddParameterWithTypeValue("LOCKVR", OracleDbType.Decimal, rowlock)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("UpdatedSource2Flg_Local_End")
            'ログ出力 End *****************************************************************************

            Return query.Execute()
        End Using
    End Function

    ''' <summary>
    ''' 商談ローカル追加（レコードの新規作成目的）
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <param name="Account">スタッフコード</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Shared Function AddSalesLocalRecord(ByVal salesId As Decimal,
                                     ByVal Account As String) As Integer

        Dim sql As New StringBuilder

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("AddSalesLocalRecord_Start")
        'ログ出力 End *****************************************************************************

        With sql
            .Append("INSERT ")
            .Append("    /* SC3080202_242 */ ")
            .Append("INTO TB_LT_SALES ( ")
            .Append("    SALES_ID, ")
            .Append("    ROW_CREATE_DATETIME, ")
            .Append("    ROW_CREATE_ACCOUNT, ")
            .Append("    ROW_CREATE_FUNCTION, ")
            .Append("    ROW_UPDATE_DATETIME, ")
            .Append("    ROW_UPDATE_ACCOUNT, ")
            .Append("    ROW_UPDATE_FUNCTION, ")
            .Append("    ROW_LOCK_VERSION ")
            .Append(") ")
            .Append("VALUES ( ")
            .Append("    :SALES_ID, ")
            .Append("     SYSDATE, ")
            .Append("    :ACCOUNT, ")
            .Append("     'SC3080202', ")
            .Append("     SYSDATE, ")
            .Append("    :ACCOUNT, ")
            .Append("     'SC3080202', ")
            .Append("     0 ")
            .Append(") ")
        End With

        Using query As New DBUpdateQuery("SC3080202_242")

            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, salesId)
            query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, Account)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("AddSalesLocalRecord_End")
            'ログ出力 End *****************************************************************************

            Return query.Execute()
        End Using
    End Function
#End Region
    '2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)追加 end
#End Region

End Class
