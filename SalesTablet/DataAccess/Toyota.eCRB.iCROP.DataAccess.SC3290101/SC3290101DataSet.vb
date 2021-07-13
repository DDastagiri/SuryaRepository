'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3290101DataSet.vb
'──────────────────────────────────
'機能： 異常リスト
'補足： 
'作成： 2014/06/13 TMEJ y.gotoh
'更新： 
'──────────────────────────────────

Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Text


Namespace SC3290101DataSetTableAdapters
    ''' <summary>
    ''' 異常リストのデータアクセスクラス
    ''' </summary>
    ''' <remarks></remarks>
    Public Class SC3290101TableAdapter
        Inherits Global.System.ComponentModel.Component

#Region "定数"

        ''' <summary>
        ''' 管理対象フラグ：管理対象
        ''' </summary>
        ''' <remarks></remarks>
        Private Const MngTgtFlgManageTarget As String = "1"

        ''' <summary>
        ''' フォロー完了フラグ：完了
        ''' </summary>
        ''' <remarks></remarks>
        Private Const FllwCompleteFlgCompleted As String = "1"

        ''' <summary>
        ''' 異常分類コード：計画異常
        ''' </summary>
        ''' <remarks></remarks>
        Private Const IrregClassCodePlanAbnormal As String = "20"

        ''' <summary>
        ''' 乖離承認フラグ：未承認
        ''' </summary>
        ''' <remarks></remarks>
        Private Const GapApprovalFlgUnapproved As String = "0"

        ''' <summary>
        ''' 商談ステータス：Continue
        ''' </summary>
        ''' <remarks></remarks>
        Private Const SalesStatusContinue As String = "21"

        ''' <summary>
        ''' 活動遅れ承認フラグ：未承認
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ActDelayApprovalFlgUnapproved As String = "0"

        ''' <summary>
        ''' 実施フラグ：未実施
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ActHisResultFlg As String = "0"

        ''' <summary>
        ''' 表示フラグ：表示
        ''' </summary>
        ''' <remarks></remarks>
        Private Const AfterChipDispFlg As String = "1"

        ''' <summary>
        ''' 受注後活動ステータス：未入力
        ''' </summary>
        ''' <remarks></remarks>
        Private Const AfterOrderActStatusNoEntered As String = "0"

        ''' <summary>
        ''' 受注後活動ステータス：入力済み
        ''' </summary>
        ''' <remarks></remarks>
        Private Const AfterOrderActStatusEntered As String = "1"

        ''' <summary>
        ''' 予定時間指定フラグ：時間指定なし
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ScheDateOrTimeFlgNothing As String = "0"

        ''' <summary>
        ''' 予定時間指定フラグ：時間指定あり
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ScheDateOrTimeFlgExistence As String = "1"

        ''' <summary>
        ''' 日付のDB初期値
        ''' </summary>
        ''' <remarks></remarks>
        Private Const DefaultDateTime As String = "1900-01-01 00:00:00"

#End Region

#Region "表示対象異常項目一覧の取得"
        ''' <summary>
        ''' 表示対象異常項目一覧の取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <returns>異常情報データテーブル</returns>
        ''' <remarks></remarks>
        Public Function GetDisplayIrregularList(ByVal dealerCode As String, ByVal branchCode As String) As SC3290101DataSet.IrregularInfoDataTable

            Dim dt As SC3290101DataSet.IrregularInfoDataTable = Nothing

            Using query As New DBSelectQuery(Of SC3290101DataSet.IrregularInfoDataTable)("SC3290101_001")
                Dim sql As New StringBuilder

                'SQL文作成
                With sql
                    .Append(" SELECT /* SC3290101_001 */ ")
                    .Append("        BRNSET.IRREG_CLASS_CD ")
                    .Append("      , BRNSET.IRREG_ITEM_CD ")
                    .Append("      , SLSMNG.RELATION_CD ")
                    .Append("      , DECODE(WORD.WORD_VAL, ' ' ,WORD.WORD_VAL_ENG, WORD.WORD_VAL) AS IRREG_LIST_DISP_NAME ")
                    .Append("      , SLSMNG.SORT_ORDER ")
                    .Append("      , '0' AS IRREG_STAFF_COUNT ")
                    .Append("      , '0' AS IRREG_COUNT ")
                    .Append("   FROM TB_M_SLS_MANAGER_IRREG_MNG SLSMNG ")
                    .Append("      , TB_M_IRREG_BRN_SETTING BRNSET ")
                    .Append("      , TB_M_WORD WORD ")
                    .Append("  WHERE SLSMNG.IRREG_CLASS_CD = BRNSET.IRREG_CLASS_CD ")
                    .Append("    AND SLSMNG.IRREG_ITEM_CD = BRNSET.IRREG_ITEM_CD ")
                    .Append("    AND SLSMNG.IRREG_LIST_DISP_NAME = WORD.WORD_CD(+) ")
                    .Append("    AND BRNSET.DLR_CD = :DLR_CD ")
                    .Append("    AND BRNSET.BRN_CD = :BRN_CD ")
                    .Append("    AND BRNSET.MNG_TGT_FLG = :MNG_TGT_FLG  ")
                End With

                query.CommandText = sql.ToString()
                'バインド変数
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, branchCode)
                query.AddParameterWithTypeValue("MNG_TGT_FLG", OracleDbType.NVarchar2, MngTgtFlgManageTarget)
                'クエリ実行
                dt = query.GetData()
            End Using

            Return dt

        End Function
#End Region

#Region "目標未達情報取得"
        ''' <summary>
        ''' 目標未達情報取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="displayTargetDate">表示対象日付</param>
        ''' <returns>異常情報データテーブル</returns>
        ''' <remarks></remarks>
        Public Function GetGoalUnachieved(ByVal dealerCode As String, ByVal branchCode As String, _
                                          ByVal displayTargetDate As Date) As SC3290101DataSet.IrregularInfoDataTable

            Dim dt As SC3290101DataSet.IrregularInfoDataTable = Nothing

            Using query As New DBSelectQuery(Of SC3290101DataSet.IrregularInfoDataTable)("SC3290101_002")
                Dim sql As New StringBuilder

                'SQL文作成
                With sql
                    .Append(" SELECT /* SC3290101_002 */ ")
                    .Append("        FLLTGT.IRREG_CLASS_CD ")
                    .Append("      , FLLTGT.IRREG_ITEM_CD ")
                    .Append("      , ' ' AS RELATION_CD ")
                    .Append("      , MAX(DECODE(WORD.WORD_VAL, ' ' ,WORD.WORD_VAL_ENG, WORD.WORD_VAL)) AS IRREG_LIST_DISP_NAME ")
                    .Append("      , CASE WHEN MAX(SLSMNG.SORT_ORDER) IS NULL THEN 1000 ELSE MAX(SLSMNG.SORT_ORDER) END AS SORT_ORDER ")
                    .Append("      , COUNT(FLLTGT.STF_CD) AS IRREG_STAFF_COUNT ")
                    .Append("      , '0' AS IRREG_COUNT ")
                    .Append("   FROM TB_T_IRREG_FLLW_TGT FLLTGT ")
                    .Append("      , TB_T_IRREG_FLLW FLL ")
                    .Append("      , TB_M_STAFF STF ")
                    .Append("      , TB_M_SLS_MANAGER_IRREG_MNG SLSMNG ")
                    .Append("      , TB_M_WORD WORD ")
                    .Append("  WHERE FLLTGT.IRREG_CLASS_CD = SLSMNG.IRREG_CLASS_CD(+)  ")
                    .Append("    AND FLLTGT.IRREG_ITEM_CD = SLSMNG.IRREG_ITEM_CD(+) ")
                    .Append("    AND SLSMNG.IRREG_LIST_DISP_NAME = WORD.WORD_CD(+) ")
                    .Append("    AND FLLTGT.IRREG_CLASS_CD = FLL.IRREG_CLASS_CD(+) ")
                    .Append("    AND FLLTGT.IRREG_ITEM_CD = FLL.IRREG_ITEM_CD(+) ")
                    .Append("    AND FLLTGT.STF_CD = FLL.STF_CD(+) ")
                    .Append("    AND FLLTGT.STF_CD = STF.STF_CD ")
                    .Append("    AND STF.DLR_CD = :DLR_CD ")
                    .Append("    AND STF.BRN_CD = :BRN_CD ")
                    .Append("    AND ( ")
                    .Append("            FLL.IRREG_FLLW_ID IS NULL ")
                    .Append("         OR ( ")
                    .Append("                FLL.FLLW_COMPLETE_FLG = :FLLW_COMPLETE_FLG ")
                    .Append("            AND FLL.FLLW_EXPR_DATE < :DISPTGTDATE ")
                    .Append("            ) ")
                    .Append("        ) ")
                    .Append("  GROUP BY FLLTGT.IRREG_CLASS_CD, FLLTGT.IRREG_ITEM_CD ")
                End With

                query.CommandText = sql.ToString()
                'バインド変数
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, branchCode)
                query.AddParameterWithTypeValue("FLLW_COMPLETE_FLG", OracleDbType.NVarchar2, FllwCompleteFlgCompleted)
                query.AddParameterWithTypeValue("DISPTGTDATE", OracleDbType.Date, displayTargetDate)

                'クエリ実行
                dt = query.GetData()
            End Using

            Return dt

        End Function
#End Region

#Region "計画異常情報取得"
        ''' <summary>
        ''' 計画異常情報取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <returns>異常情報データテーブル</returns>
        ''' <remarks></remarks>
        Public Function GetPlanningAbnormal(ByVal dealerCode As String, ByVal branchCode As String) As SC3290101DataSet.IrregularInfoDataTable

            Dim dt As SC3290101DataSet.IrregularInfoDataTable = Nothing

            Using query As New DBSelectQuery(Of SC3290101DataSet.IrregularInfoDataTable)("SC3290101_003")
                Dim sql As New StringBuilder

                'SQL文作成
                With sql
                    .Append(" SELECT /* SC3290101_003 */")
                    .Append("        BRNSET.IRREG_CLASS_CD")
                    .Append("      , BRNSET.IRREG_ITEM_CD")
                    .Append("      , SLSMNG.RELATION_CD")
                    .Append("      , MAX(DECODE(WORD.WORD_VAL, ' ' ,WORD.WORD_VAL_ENG, WORD.WORD_VAL)) AS IRREG_LIST_DISP_NAME")
                    .Append("      , MAX(SLSMNG.SORT_ORDER) AS SORT_ORDER")
                    .Append("      , COUNT( DISTINCT AX1.STF_CD) AS IRREG_STAFF_COUNT ")
                    .Append("      , COUNT( AFT_CHIP.SPM_AFTER_ODR_CHIP_ID) AS IRREG_COUNT ")
                    .Append("   FROM TB_M_IRREG_BRN_SETTING BRNSET")
                    .Append("      , TB_M_SLS_MANAGER_IRREG_MNG SLSMNG ")
                    .Append("      , TB_M_WORD WORD")
                    .Append("      , TB_T_SPM_AFTER_ODR_CHIP AFT_CHIP ")
                    .Append("      , TB_M_SPM_SUM_AXIS AX1")
                    .Append("      , (SELECT T1.AFTER_ODR_ID")
                    .Append("              , DCR.DELAY_CHK_RANGE_CD")
                    .Append("              , DCR.EDPT_AFTER_ODR_ACT_CD")
                    .Append("           FROM TB_T_AFTER_ODR_ACT T1")
                    .Append("              , TB_M_DELAY_CHK_RANGE DCR")
                    .Append("          WHERE T1.AFTER_ODR_ACT_CD = DCR.EDPT_AFTER_ODR_ACT_CD")
                    .Append("            AND T1.SCHE_END_DATEORTIME > :DEFAULT_DATE_TIME")
                    .Append("            AND T1.STD_END_DATEORTIME > :DEFAULT_DATE_TIME")
                    .Append("            AND T1.RSLT_END_DATEORTIME = :DEFAULT_DATE_TIME")
                    .Append("            AND (T1.AFTER_ODR_ACT_STATUS = :STATUS_NO_ENTERED OR T1.AFTER_ODR_ACT_STATUS > :STATUS_ENTERED)")
                    .Append("            AND (")
                    .Append("                    (T1.SCHE_DATEORTIME_FLG = :DATEORTIME_FLG_NOTHING AND T1.STD_END_DATEORTIME < T1.SCHE_END_DATEORTIME)")
                    .Append("                    OR")
                    .Append("                    (T1.SCHE_DATEORTIME_FLG = :DATEORTIME_FLG_EXISTANCE AND TRUNC(T1.STD_END_DATEORTIME) < TRUNC(T1.SCHE_END_DATEORTIME))")
                    .Append("                  )")
                    .Append("            AND T1.GAP_APPROVAL_FLG = :GAP_APPROVAL_FLG")
                    .Append("        ) DCRC")
                    .Append("  WHERE BRNSET.IRREG_CLASS_CD = SLSMNG.IRREG_CLASS_CD")
                    .Append("    AND BRNSET.IRREG_ITEM_CD = SLSMNG.IRREG_ITEM_CD")
                    .Append("    AND SLSMNG.IRREG_LIST_DISP_NAME = WORD.WORD_CD(+)")
                    .Append("    AND AFT_CHIP.DLR_CD = BRNSET.DLR_CD")
                    .Append("    AND AFT_CHIP.BRN_CD = BRNSET.BRN_CD")
                    .Append("    AND AFT_CHIP.DLR_CD = AX1.DLR_CD")
                    .Append("    AND AFT_CHIP.BRN_CD = AX1.BRN_CD")
                    .Append("    AND AFT_CHIP.ORGNZ_ID = AX1.ORGNZ_ID")
                    .Append("    AND AFT_CHIP.SALES_PIC_STF_CD = AX1.STF_CD")
                    .Append("    AND AFT_CHIP.AFTER_ODR_ID = DCRC.AFTER_ODR_ID")
                    .Append("    AND SLSMNG.RELATION_CD = DCRC.DELAY_CHK_RANGE_CD")
                    .Append("    AND BRNSET.DLR_CD = :DLR_CD")
                    .Append("    AND BRNSET.BRN_CD = :BRN_CD")
                    .Append("    AND BRNSET.IRREG_CLASS_CD = :IRREG_CLASS_CD")
                    .Append("    AND BRNSET.MNG_TGT_FLG = :MNG_TGT_FLG")
                    .Append("    AND AFT_CHIP.DISP_FLG = :AFT_CHIP_DISP_FLG")
                    .Append("  GROUP BY BRNSET.IRREG_CLASS_CD")
                    .Append("         , BRNSET.IRREG_ITEM_CD")
                    .Append("         , SLSMNG.RELATION_CD")
                End With

                query.CommandText = sql.ToString()
                'バインド変数
                query.AddParameterWithTypeValue("DEFAULT_DATE_TIME", OracleDbType.Date, Date.Parse(DefaultDateTime))
                query.AddParameterWithTypeValue("GAP_APPROVAL_FLG", OracleDbType.NVarchar2, GapApprovalFlgUnapproved)
                query.AddParameterWithTypeValue("IRREG_CLASS_CD", OracleDbType.NVarchar2, IrregClassCodePlanAbnormal)
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, branchCode)
                query.AddParameterWithTypeValue("MNG_TGT_FLG", OracleDbType.NVarchar2, MngTgtFlgManageTarget)
                query.AddParameterWithTypeValue("AFT_CHIP_DISP_FLG", OracleDbType.NVarchar2, AfterChipDispFlg)
                query.AddParameterWithTypeValue("STATUS_NO_ENTERED", OracleDbType.NVarchar2, AfterOrderActStatusNoEntered)
                query.AddParameterWithTypeValue("STATUS_ENTERED", OracleDbType.NVarchar2, AfterOrderActStatusEntered)
                query.AddParameterWithTypeValue("DATEORTIME_FLG_NOTHING", OracleDbType.NVarchar2, ScheDateOrTimeFlgNothing)
                query.AddParameterWithTypeValue("DATEORTIME_FLG_EXISTANCE", OracleDbType.NVarchar2, ScheDateOrTimeFlgExistence)

                'クエリ実行
                dt = query.GetData()
            End Using

            Return dt

        End Function
#End Region

#Region "受注前活動遅れ情報取得"
        ''' <summary>
        ''' 受注前活動遅れ情報取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="todayDate">本日日付</param>
        ''' <returns>活動遅れ情報データテーブル</returns>
        ''' <remarks></remarks>
        Public Function GetBeforeOrderDelayActivity(ByVal dealerCode As String, ByVal branchCode As String, _
                                                    ByVal todayDate As Date) As SC3290101DataSet.ActivityDelayInfoDataTable

            Dim dt As SC3290101DataSet.ActivityDelayInfoDataTable = Nothing

            Using query As New DBSelectQuery(Of SC3290101DataSet.ActivityDelayInfoDataTable)("SC3290101_004")
                Dim sql As New StringBuilder

                'SQL文作成
                With sql
                    .Append(" SELECT /* SC3290101_004 */")
                    .Append("        COUNT(DISTINCT BEF_CHIP.SALES_PIC_STF_CD) AS IRREG_STAFF_COUNT")
                    .Append("      , COUNT(BEF_CHIP.SALES_ID) AS IRREG_COUNT")
                    .Append("   FROM TB_T_SPM_BEFORE_ODR_CHIP BEF_CHIP")
                    .Append("      , TB_M_SPM_SUM_AXIS AXIS")
                    .Append("      , TB_T_SPM_ACT_HIS ACT_HIS")
                    .Append("      , TB_T_ACTIVITY ACT")
                    .Append("  WHERE BEF_CHIP.DLR_CD = AXIS.DLR_CD")
                    .Append("    AND BEF_CHIP.BRN_CD = AXIS.BRN_CD")
                    .Append("    AND BEF_CHIP.ORGNZ_ID = AXIS.ORGNZ_ID")
                    .Append("    AND BEF_CHIP.SALES_PIC_STF_CD = AXIS.STF_CD")
                    .Append("    AND BEF_CHIP.SALES_ID = ACT_HIS.SALES_ID")
                    .Append("    AND ACT_HIS.ACT_ID = ACT.ACT_ID")
                    .Append("    AND AXIS.DLR_CD = :DLR_CD")
                    .Append("    AND AXIS.BRN_CD = :BRN_CD")
                    .Append("    AND ACT_HIS.RSLT_FLG = :ACT_HIS_RSLT_FLG")
                    .Append("    AND ACT_HIS.SCHE_DATE < :TODAY_DATE")
                    .Append("    AND BEF_CHIP.SALES_STATUS = :SALES_STATUS")
                    .Append("    AND ACT.ACT_DELAY_APPROVAL_FLG = :ACT_DELAY_APPROVAL_FLG")
                End With

                query.CommandText = sql.ToString()
                'バインド変数
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, branchCode)
                query.AddParameterWithTypeValue("ACT_HIS_RSLT_FLG", OracleDbType.NVarchar2, ActHisResultFlg)
                query.AddParameterWithTypeValue("SALES_STATUS", OracleDbType.NVarchar2, SalesStatusContinue)
                query.AddParameterWithTypeValue("TODAY_DATE", OracleDbType.Date, todayDate)
                query.AddParameterWithTypeValue("ACT_DELAY_APPROVAL_FLG", OracleDbType.NVarchar2, ActDelayApprovalFlgUnapproved)

                'クエリ実行
                dt = query.GetData()
            End Using

            Return dt

        End Function
#End Region

#Region "受注後活動遅れ情報取得"
        ''' <summary>
        ''' 受注後活動遅れ情報取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="todayDate">本日日付</param>
        ''' <param name="afterOdrActType">受注後活動区分</param>
        ''' <returns>活動遅れ情報データテーブル</returns>
        ''' <remarks></remarks>
        Public Function GetAfterOrderDelayActivity(ByVal dealerCode As String, ByVal branchCode As String, _
                                            ByVal todayDate As Date, ByVal afterOdrActType As String) As SC3290101DataSet.ActivityDelayInfoDataTable

            Dim dt As SC3290101DataSet.ActivityDelayInfoDataTable = Nothing

            Using query As New DBSelectQuery(Of SC3290101DataSet.ActivityDelayInfoDataTable)("SC3290101_005")
                Dim sql As New StringBuilder

                'SQL文作成
                With sql
                    .Append(" SELECT /* SC3290101_005 */")
                    .Append("        COUNT(DISTINCT AFT_CHIP.SALES_PIC_STF_CD) AS IRREG_STAFF_COUNT")
                    .Append("      , COUNT(AFT_CHIP.SPM_AFTER_ODR_CHIP_ID) AS IRREG_COUNT")
                    .Append("   FROM TB_T_SPM_AFTER_ODR_CHIP AFT_CHIP")
                    .Append("      , TB_M_SPM_SUM_AXIS T3")
                    .Append("      , TB_T_AFTER_ODR_ACT AFT_ACT")
                    .Append("      , TB_M_AFTER_ODR_ACT ACT_MST")
                    .Append("      , TB_M_SPM_AFTER_ODR_ACT SPM_AFT_MST")
                    .Append("  WHERE AFT_CHIP.DLR_CD = T3.DLR_CD")
                    .Append("    AND AFT_CHIP.BRN_CD = T3.BRN_CD")
                    .Append("    AND AFT_CHIP.ORGNZ_ID = T3.ORGNZ_ID")
                    .Append("    AND AFT_CHIP.SALES_PIC_STF_CD = T3.STF_CD")
                    .Append("    AND AFT_CHIP.AFTER_ODR_ID = AFT_ACT.AFTER_ODR_ID")
                    .Append("    AND AFT_CHIP.AFTER_ODR_ACT_CD = AFT_ACT.AFTER_ODR_ACT_CD")
                    .Append("    AND AFT_CHIP.AFTER_ODR_ACT_CD = ACT_MST.AFTER_ODR_ACT_CD")
                    .Append("    AND AFT_CHIP.AFTER_ODR_ACT_CD = SPM_AFT_MST.AFTER_ODR_ACT_CD")
                    .Append("    AND T3.DLR_CD = :DLR_CD")
                    .Append("    AND T3.BRN_CD = :BRN_CD")
                    .Append("    AND AFT_CHIP.DISP_FLG = :AFT_CHIP_DISP_FLG")
                    .Append("    AND AFT_ACT.SCHE_END_DATEORTIME > :DEFAULT_DATE_TIME")
                    .Append("    AND SPM_AFT_MST.SPM_DISP_TYPE = :SPM_DISP_TYPE")
                    .Append("    AND (AFT_ACT.AFTER_ODR_ACT_STATUS = :STATUS_NO_ENTERED OR AFT_ACT.AFTER_ODR_ACT_STATUS > :STATUS_ENTERED)")
                    .Append("    AND (")
                    .Append("         (AFT_ACT.SCHE_DATEORTIME_FLG = :DATEORTIME_FLG_NOTHING AND AFT_ACT.SCHE_END_DATEORTIME < :TODAY_DATE)")
                    .Append("          OR ")
                    .Append("         (AFT_ACT.SCHE_DATEORTIME_FLG = :DATEORTIME_FLG_EXISTANCE AND AFT_ACT.SCHE_END_DATEORTIME < :YESTERDAY_DATE)")
                    .Append("        )")
                    .Append("    AND AFT_ACT.ACT_DELAY_APPROVAL_FLG = :ACT_DELAY_APPROVAL_FLG")
                End With

                query.CommandText = sql.ToString()
                'バインド変数
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, branchCode)
                query.AddParameterWithTypeValue("AFT_CHIP_DISP_FLG", OracleDbType.NVarchar2, AfterChipDispFlg)
                query.AddParameterWithTypeValue("DEFAULT_DATE_TIME", OracleDbType.Date, Date.Parse(DefaultDateTime))
                query.AddParameterWithTypeValue("TODAY_DATE", OracleDbType.Date, todayDate)
                query.AddParameterWithTypeValue("YESTERDAY_DATE", OracleDbType.Date, DateAdd("s", -1, todayDate))
                query.AddParameterWithTypeValue("SPM_DISP_TYPE", OracleDbType.NVarchar2, afterOdrActType)
                query.AddParameterWithTypeValue("STATUS_NO_ENTERED", OracleDbType.NVarchar2, AfterOrderActStatusNoEntered)
                query.AddParameterWithTypeValue("STATUS_ENTERED", OracleDbType.NVarchar2, AfterOrderActStatusEntered)
                query.AddParameterWithTypeValue("DATEORTIME_FLG_NOTHING", OracleDbType.NVarchar2, ScheDateOrTimeFlgNothing)
                query.AddParameterWithTypeValue("DATEORTIME_FLG_EXISTANCE", OracleDbType.NVarchar2, ScheDateOrTimeFlgExistence)
                query.AddParameterWithTypeValue("ACT_DELAY_APPROVAL_FLG", OracleDbType.NVarchar2, ActDelayApprovalFlgUnapproved)

                'クエリ実行
                dt = query.GetData()
            End Using

            Return dt

        End Function
#End Region

#Region "更新日時の取得"
        ''' <summary>
        ''' 更新日時の取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <returns>最終更新日時</returns>
        ''' <remarks></remarks>
        Public Function GetUpdatetime(ByVal dealerCode As String, ByVal branchCode As String) As Date

            Dim dt As DataTable = Nothing

            Using query As New DBSelectQuery(Of DataTable)("SC3290101_006")
                Dim sql As New StringBuilder

                'SQL文作成
                With sql
                    .Append(" SELECT /* SC3290101_006 */ ")
                    .Append("        MAX(FLLTCT.ROW_UPDATE_DATETIME) AS ROW_UPDATE_DATETIME ")
                    .Append("   FROM ")
                    .Append("        TB_T_IRREG_FLLW_TGT FLLTCT ")
                    .Append("      , TB_M_STAFF STF ")
                    .Append("  WHERE ")
                    .Append("         FLLTCT.STF_CD = STF.STF_CD ")
                    .Append("     AND STF.DLR_CD = :DLR_CD ")
                    .Append("     AND STF.BRN_CD = :BRN_CD ")
                End With

                query.CommandText = sql.ToString()
                'バインド変数
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, branchCode)

                'クエリ実行
                dt = query.GetData()
            End Using

            If 0 < dt.Rows.Count AndAlso IsDate(dt.Rows(0).Item("ROW_UPDATE_DATETIME")) Then
                '戻り値
                Return CDate(dt.Rows(0).Item("ROW_UPDATE_DATETIME"))
            Else
                Return New Date(1900, 1, 1)
            End If

        End Function
#End Region

    End Class
End Namespace


Partial Class SC3290101DataSet
End Class
