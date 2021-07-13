'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3190601TableAdapter.vb
'─────────────────────────────────────
'機能： B/O管理ボード (データ)
'補足： 
'作成： 2014/08/25 TMEJ M.Asano
'更新： 
'─────────────────────────────────────

Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic

Public NotInheritable Class SC3190601TableAdapter

#Region "コンストラクタ"

    ''' <summary>
    ''' デフォルトコンストラクタ処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub New()
        '処理なし
    End Sub

#End Region

#Region "公開メソッド"

    ''' <summary>
    ''' 部品情報一覧の取得
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="nowDate">本日日付</param>
    ''' <param name="judgmentDate">直近到着予定部品判定日付</param>
    ''' <returns>BoPartsInfoListDataTable(部品情報)</returns>
    ''' <remarks></remarks>
    Public Shared Function GetPartsInfoList(ByVal dealerCode As String, _
                                            ByVal branchCode As String, _
                                            ByVal nowDate As Date, _
                                            ByVal judgmentDate As Date) _
                                            As SC3190601DataSet.BoPartsInfoListDataTable

        Dim boPartsListDataTable As SC3190601DataSet.BoPartsInfoListDataTable = Nothing

        Using query As New DBSelectQuery(Of SC3190601DataSet.BoPartsInfoListDataTable)("SC3190601_001")
            Dim sql As New StringBuilder
            With sql
                .Append(" SELECT /* SC3190601_001 */ ")
                .Append("        0 AS No ")
                .Append("      , SUB.BO_ID ")
                .Append("      , SUB.PO_NUM ")
                .Append("      , SUB.RO_NUM ")
                .Append("      , SUB.BO_JOB_ID ")
                .Append("      , SUB.JOB_NAME ")
                .Append("      , SUB.PARTS_NAME ")
                .Append("      , SUB.PARTS_CD ")
                .Append("      , SUB.PARTS_AMOUNT ")
                .Append("      , SUB.ODR_DATE ")
                .Append("      , SUB.ARRIVAL_SCHE_DATE ")
                .Append("      , SUB.VCL_PARTAKE_FLG ")
                .Append("      , SUB.CST_APPOINTMENT_DATE ")
                .Append("      , SUB.PO_DELAY_FLAG ")
                .Append("      , SUB.PARTS_DELAY_FLAG ")
                .Append("      , COUNT(DISTINCT SUB.BO_ID) OVER () AS PO_COUNT ")
                .Append("      , COUNT(DISTINCT CASE WHEN SUB.PO_DELAY_FLAG = '1' THEN SUB.BO_ID ELSE NULL END) OVER () AS PO_DELAY_COUNT ")
                .Append("      , COUNT(SUB.BO_PARTS_ID) OVER () AS PARTS_COUNT ")
                .Append("      , COUNT(CASE WHEN SUB.PO_DELAY_FLAG = '1' THEN SUB.BO_PARTS_ID ELSE NULL END) OVER () AS PARTS_DELAY_COUNT ")
                .Append("   FROM ( ")
                .Append("         SELECT MGR.BO_ID ")
                .Append("              , BO.BO_JOB_ID ")
                .Append("              , PARTS.BO_PARTS_ID ")
                .Append("              , MGR.PO_NUM ")
                .Append("              , MGR.RO_NUM ")
                .Append("              , BO.JOB_NAME ")
                .Append("              , PARTS.PARTS_NAME ")
                .Append("              , PARTS.PARTS_CD ")
                .Append("              , PARTS.PARTS_AMOUNT ")
                .Append("              , PARTS.ODR_DATE ")
                .Append("              , PARTS.ARRIVAL_SCHE_DATE ")
                .Append("              , MGR.VCL_SVCIN_FLG AS VCL_PARTAKE_FLG ")
                .Append("              , MGR.CST_APPO_DATE AS CST_APPOINTMENT_DATE ")
                .Append("              , MIN(CASE WHEN PARTS.ARRIVAL_SCHE_DATE = :DEFAULT_DATE THEN NULL ELSE PARTS.ARRIVAL_SCHE_DATE END) OVER (PARTITION BY MGR.BO_ID) AS SORT_KEY1 ")
                .Append("              , MIN(CASE WHEN PARTS.ARRIVAL_SCHE_DATE = :DEFAULT_DATE THEN NULL ELSE PARTS.ARRIVAL_SCHE_DATE END) OVER (PARTITION BY MGR.BO_ID, BO.BO_JOB_ID) AS SORT_KEY2 ")
                .Append("              , MIN(CASE WHEN PARTS.ODR_DATE = :DEFAULT_DATE THEN NULL ELSE PARTS.ODR_DATE END) OVER (PARTITION BY MGR.BO_ID, BO.BO_JOB_ID) AS SORT_KEY3  ")
                .Append("              , CASE WHEN MGR.CST_APPO_DATE = :DEFAULT_DATE THEN '0' ")
                .Append("                     ELSE CASE WHEN MGR.CST_APPO_DATE < :NOW_DATE THEN '1' ELSE '0' END ")
                .Append("                END AS PO_DELAY_FLAG ")
                .Append("              , CASE WHEN PARTS.ARRIVAL_SCHE_DATE = :DEFAULT_DATE THEN '0' ")
                .Append("                     ELSE CASE WHEN PARTS.ARRIVAL_SCHE_DATE < :JUDGMENT_DATE THEN '1' ELSE '0' END ")
                .Append("                END AS PARTS_DELAY_FLAG ")
                .Append("           FROM TB_T_BO_MNG_INFO MGR ")
                .Append("              , TB_T_BO_JOB_INFO BO ")
                .Append("              , TB_T_BO_PARTS_INFO PARTS ")
                .Append("          WHERE MGR.BO_ID = BO.BO_ID ")
                .Append("            AND BO.BO_JOB_ID = PARTS.BO_JOB_ID ")
                .Append("            AND MGR.DLR_CD = :DLR_CD ")
                .Append("            AND MGR.BRN_CD = :BRN_CD ")
                .Append("         ) SUB ")
                .Append("  ORDER BY SUB.SORT_KEY1 ")
                .Append("         , CASE WHEN SUB.RO_NUM = :DEFAULT_STRING THEN NULL ELSE SUB.RO_NUM END ")
                .Append("         , SUB.BO_ID ")
                .Append("         , SUB.SORT_KEY2 ")
                .Append("         , SUB.SORT_KEY3 ")
                .Append("         , SUB.BO_JOB_ID ")
                .Append("         , CASE WHEN SUB.ARRIVAL_SCHE_DATE = :DEFAULT_DATE THEN NULL ELSE SUB.ARRIVAL_SCHE_DATE END ")
                .Append("         , CASE WHEN SUB.ODR_DATE = :DEFAULT_DATE THEN NULL ELSE SUB.ODR_DATE END ")
                .Append("         , SUB.BO_PARTS_ID ")
            End With

            query.CommandText = sql.ToString()
            sql = Nothing

            'バインド変数
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)
            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, branchCode)
            query.AddParameterWithTypeValue("NOW_DATE", OracleDbType.Date, nowDate)
            query.AddParameterWithTypeValue("JUDGMENT_DATE", OracleDbType.Date, judgmentDate)
            query.AddParameterWithTypeValue("DEFAULT_DATE", OracleDbType.Date, New Date(1900, 1, 1, 0, 0, 0))
            query.AddParameterWithTypeValue("DEFAULT_STRING", OracleDbType.NVarchar2, " ")

            'クエリ実行
            boPartsListDataTable = query.GetData()
        End Using

        ' 検索結果返却
        Return boPartsListDataTable

    End Function

#End Region

End Class
