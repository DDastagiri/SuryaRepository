'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3070205TableAdapter.vb
'─────────────────────────────────────
'機能：
'補足： 
'作成：2013/11/27 TCS 河原 
'─────────────────────────────────────

Imports System.Text
Imports System.Data
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic

Public NotInheritable Class SC3070205TableAdapter

#Region "定数定義"
    ' 2012/02/24 TCS 堀 【SALES_1B】 START
    ''' <summary>
    ''' 依頼内容マスタの価格相談
    ''' </summary>
    ''' <remarks></remarks>
    Private Const REASON_DISCOUNT As String = "02"

    ''' <summary>
    ''' 中古車査定テーブルと結合時参照の通知情報テーブル・最終ステータス(4.受付)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NOTICE_STATUS As String = "4"

    ''' <summary>
    ''' 見積価格相談テーブルの返答フラグ(1.回答済)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RESPONSEFLG_ON As String = "1"
    ' 2012/02/24 TCS 堀 【SALES_1B】 END
#End Region

#Region "メソッド"

    ''' <summary>
    ''' デフォルトコンストラクタ
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub New()
        'デフォルトコンストラクタ
    End Sub

    ''' <summary>
    ''' 自社客個人情報取得
    ''' </summary>
    ''' <param name="originalid">自社客コード</param>
    ''' <returns>SC3070205ORG_CUSTOMER</returns>
    ''' <remarks></remarks>
    Public Shared Function GetOrgCustomer(ByVal originalid As String) As SC3070205DataSet.SC3070205ORGCUSTOMERDataTable

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetOrgCustomer_Start")
        'ログ出力 End *****************************************************************************

        Using query As New DBSelectQuery(Of SC3070205DataSet.SC3070205ORGCUSTOMERDataTable)("SC3070205_001")

            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* SC3070205_001 */ ")
                .Append("    'XXXXX' AS DLRCD, ")
                .Append("    T1.CST_ID AS ORIGINALID, ")
                .Append("    CASE WHEN T1.FLEET_FLG = '0' THEN '1' ")
                .Append("         WHEN T1.FLEET_FLG = '1' THEN '0' ")
                .Append("         ELSE ' ' END AS CUSTYPE, ")
                .Append("    T1.CST_SOCIALNUM AS SOCIALID, ")
                .Append("    T1.CST_NAME AS NAME, ")
                .Append("    T1.CST_ADDRESS AS ADDRESS, ")
                .Append("    T1.CST_ZIPCD AS ZIPCODE, ")
                .Append("    T1.CST_PHONE AS TELNO, ")
                .Append("    T1.CST_MOBILE AS MOBILE, ")
                .Append("    T1.CST_EMAIL_1 AS EMAIL1 ")
                .Append("FROM ")
                .Append("    TB_M_CUSTOMER T1 ")
                .Append("WHERE ")
                .Append("    T1.CST_ID = :ORIGINALID ")
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ORIGINALID", OracleDbType.Decimal, originalid)        '自社客コード
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetOrgCustomer_End")
            'ログ出力 End *****************************************************************************

            Return query.GetData()

        End Using


    End Function

    ''' <summary>
    ''' 未取引客個人情報取得
    ''' </summary>
    ''' <param name="cstId">未取引客コード</param>
    ''' <returns>SC3070205NEWCUSTOMER</returns>
    ''' <remarks></remarks>
    Public Shared Function GetNewCustomer(ByVal cstId As String) As SC3070205DataSet.SC3070205NEWCUSTOMERDataTable

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetNewCustomer_Start")
        'ログ出力 End *****************************************************************************

        Using query As New DBSelectQuery(Of SC3070205DataSet.SC3070205NEWCUSTOMERDataTable)("SC3070205_002")

            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* SC3070205_002 */ ")
                .Append("    T1.CST_ID AS CSTID, ")
                .Append("    T1.CST_NAME AS NAME, ")
                .Append("    T1.CST_ADDRESS AS ADDRESS, ")
                .Append("    T1.CST_ZIPCD AS ZIPCODE, ")
                .Append("    T1.CST_PHONE AS TELNO, ")
                .Append("    T1.CST_MOBILE AS MOBILE, ")
                .Append("    T1.CST_EMAIL_1 AS EMAIL1, ")
                .Append("    CASE WHEN T1.FLEET_FLG = '0' THEN '1' ")
                .Append("         WHEN T1.FLEET_FLG = '1' THEN '0' ")
                .Append("         ELSE ' ' END AS CUSTYPE, ")
                .Append("    T1.CST_SOCIALNUM AS SOCIALID ")
                .Append("FROM ")
                .Append("    TB_M_CUSTOMER T1 ")
                .Append("WHERE ")
                .Append("    T1.CST_ID = :CSTID ")
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("CSTID", OracleDbType.Decimal, cstId)        '未取引客コード

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetNewCustomer_End")
            'ログ出力 End *****************************************************************************

            Return query.GetData()

        End Using

    End Function

    ''' <summary>
    ''' 見積保険会社マスタ取得
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <returns>SC3070205ESTINSUCOMMAST</returns>
    ''' <remarks></remarks>
    Public Shared Function GetEstInsuranceComMst(ByVal dlrcd As String) As SC3070205DataSet.SC3070205ESTINSUCOMMASTDataTable
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetEstInsuranceComMst_Start")
        'ログ出力 End *****************************************************************************

        Using query As New DBSelectQuery(Of SC3070205DataSet.SC3070205ESTINSUCOMMASTDataTable)("SC3070205_003")

            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* SC3070205_003 */ ")
                .Append("    T1.DLR_CD AS DLRCD, ")
                .Append("    T1.INS_COMPANY_CD AS INSUCOMCD, ")
                .Append("    T1.INS_TYPE AS INSUDVS, ")
                .Append("    T2.INS_COMPANY_NAME AS INSUCOMNM ")
                .Append("FROM ")
                .Append("    TB_M_INSURANCE_COMPANY_DLR T1 , ")
                .Append("    TB_M_INSURANCE_COMPANY T2 ")
                .Append("WHERE ")
                .Append("    T1.INS_COMPANY_CD = T2.INS_COMPANY_CD ")
                .Append("    AND T1.DLR_CD = :DLRCD ")
                .Append("    AND T2.EST_FLG = '1' ")
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)                  '販売店コード

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetEstInsuranceComMst_End")
            'ログ出力 End *****************************************************************************

            Return query.GetData()

        End Using

    End Function

    ''' <summary>
    ''' 見積保険種別マスタ取得
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <returns>SC3070205INSUKINDMAST</returns>
    ''' <remarks></remarks>
    Public Shared Function GetInsuKindMst(ByVal dlrcd As String) As SC3070205DataSet.SC3070205ESTINSUKINDMASTDataTable
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetInsuKindMst_Start")
        'ログ出力 End *****************************************************************************

        Using query As New DBSelectQuery(Of SC3070205DataSet.SC3070205ESTINSUKINDMASTDataTable)("SC3070205_004")

            Dim sql As New StringBuilder
            With sql
                .Append(" SELECT /* SC3070205_004 */ ")
                .Append("    A.DLRCD, ")            '販売店コード
                .Append("    RTRIM(A.INSUCOMCD) AS INSUCOMCD, ")         '保険会社コード
                .Append("    A.INSUKIND, ")          '保険種別
                .Append("    A.INSUKINDNM ")        '保険種別名称
                .Append("FROM ")
                .Append("    TBL_EST_INSUKINDMAST A ")
                .Append("WHERE ")
                .Append("    A.DLRCD = :DLRCD ")
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)                  '販売店コード

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetInsuKindMst_End")
            'ログ出力 End *****************************************************************************

            Return query.GetData()

        End Using

    End Function

    ''' <summary>
    ''' 融資会社マスタ取得
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <returns>SC3070205FINANCECOMMAST</returns>
    ''' <remarks></remarks>
    Public Shared Function GetFinanceComMst(ByVal dlrcd As String) As SC3070205DataSet.SC3070205FINANCECOMMASTDataTable
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetFinanceComMst_Start")
        'ログ出力 End *****************************************************************************

        Using query As New DBSelectQuery(Of SC3070205DataSet.SC3070205FINANCECOMMASTDataTable)("SC3070205_005")

            Dim sql As New StringBuilder
            With sql
                .Append(" SELECT /* SC3070205_005 */ ")
                .Append("    T1.DLR_CD AS DLRCD, ")
                .Append("    T1.FNC_COMPANY_CD AS FINANCECOMCODE, ")
                .Append("    T2.FNC_COMPANY_NAME AS FINANCECOMNAME ")
                .Append("FROM ")
                .Append("    TB_M_FINANCE_COMPANY_DLR T1 , ")
                .Append("    TB_M_FINANCE_COMPANY T2 ")
                .Append("WHERE ")
                .Append("    T1.FNC_COMPANY_CD = T2.FNC_COMPANY_CD ")
                .Append("    AND T1.DLR_CD = :DLRCD ")
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)                  '販売店コード

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetFinanceComMst_End")
            'ログ出力 End *****************************************************************************
            Return query.GetData()

        End Using

    End Function

    ''' <summary>
    ''' モデル写真取得
    ''' </summary>
    ''' <param name="modelCD">モデルコード</param>
    ''' <param name="colorCD">外鈑色コード</param>
    ''' <returns>SC3070205MODELPICTURE</returns>
    ''' <remarks></remarks>
    Public Shared Function GetModelPicture(ByVal modelCD As String, ByVal colorCD As String) As SC3070205DataSet.SC3070205MODELPICTUREDataTable
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetModelPicture_Start")
        'ログ出力 End *****************************************************************************

        Using query As New DBSelectQuery(Of SC3070205DataSet.SC3070205MODELPICTUREDataTable)("SC3070205_006")

            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* SC3070205_006 */ ")
                .Append("    'XXXXX' AS DLRCD, ")
                .Append("    T1.MODEL_CD AS SERIESCD, ")
                .Append("    T1.VCL_KATASHIKI AS MODELCD, ")
                .Append("    T2.BODYCLR_CD AS COLORCD, ")
                .Append("    T2.VCL_PICTURE AS IMAGEFILE  ")
                .Append("FROM ")
                .Append("    TB_M_KATASHIKI T1 , ")
                .Append("    TB_M_KATASHIKI_PICTURE T2 , ")
                .Append("    TBL_MSTEXTERIOR T3 ")
                .Append("WHERE ")
                .Append("    T1.VCL_KATASHIKI = T2.VCL_KATASHIKI ")
                .Append("    AND T1.VCL_KATASHIKI = T3.VCLMODEL_CODE ")
                .Append("    AND T2.BODYCLR_CD = T3.COLOR_CD ")
                .Append("    AND T1.VCL_KATASHIKI = :MODELCD ")
                .Append("    AND T3.BODYCLR_CD = :COLORCD ")
            End With

            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("MODELCD", OracleDbType.NVarchar2, modelCD)              'モデルコード
            query.AddParameterWithTypeValue("COLORCD", OracleDbType.NVarchar2, colorCD)              '外鈑色コード

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetModelPicture_End")
            'ログ出力 End *****************************************************************************

            Return query.GetData()

        End Using

    End Function

    ''' <summary>
    ''' 中古車査定情報取得
    ''' </summary>
    ''' <param name="seqNo">Follow-up Box内連番</param>
    ''' <returns>中古車査定テーブル</returns>
    ''' <remarks></remarks>
    Public Shared Function GetUsedTradeinCar(ByVal seqNo As Decimal) As SC3070205DataSet.SC3070205UCarAssessmentDataTable

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetUsedTradeinCar_Start")
        'ログ出力 End *****************************************************************************

        Using query As New DBSelectQuery(Of SC3070205DataSet.SC3070205UCarAssessmentDataTable)("SC3070205_007")

            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* SC3070205_007 */ ")
                .Append("       ASSESSMENTNO ")                 '査定No
                .Append("     , VEHICLENAME ")                  '車名
                .Append("     , APPRISAL_PRICE ")               '提示価格
                .Append("  FROM TBL_UCARASSESSMENT A ")         '中古車査定テーブル
                .Append("     , TBL_NOTICEREQUEST B ")          '通知情報テーブル
                .Append(" WHERE A.NOTICEREQID = B.NOTICEREQID ")
                .Append("   AND B.STATUS = :STATUS ")
                .Append("   AND A.FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO ")
            End With

            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("STATUS", OracleDbType.Char, NOTICE_STATUS)        '最終ステータス
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, seqNo)      'Follow-up Box内連番

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetUsedTradeinCar_End")
            'ログ出力 End *****************************************************************************
            Return query.GetData()

        End Using

    End Function

    ''' <summary>
    ''' CR活動結果取得
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetFollowupboxStatus(ByVal estimateId As Long) As SC3070205DataSet.SC3070205FllwUpBoxDataTable
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetFollowupboxStatus_Start")
        'ログ出力 End *****************************************************************************

        Using query As New DBSelectQuery(Of SC3070205DataSet.SC3070205FllwUpBoxDataTable)("SC3070205_013")

            Dim sql As New StringBuilder
            With sql
                '用件
                .Append("SELECT /* SC3070205_013 */ ")
                .Append("       CASE ")
                .Append("            WHEN T3.REQ_STATUS = '31' THEN ")
                .Append("                 '4' ")
                .Append("            WHEN T3.REQ_STATUS = '32' THEN ")
                .Append("                 '5' ")
                .Append("            ELSE '1' ")
                .Append("       END AS CRACTRESULT ")
                .Append("  FROM TBL_ESTIMATEINFO T1 ")
                .Append("     , TB_T_SALES T2 ")
                .Append("     , TB_T_REQUEST T3 ")
                .Append(" WHERE T1.FLLWUPBOX_SEQNO = T2.SALES_ID ")
                .Append("   AND T2.REQ_ID = T3.REQ_ID ")
                .Append("   AND T1.ESTIMATEID = :ESTIMATEID ")
                '用件(History)
                .Append("UNION ")
                .Append(" SELECT ")
                .Append("       CASE ")
                .Append("            WHEN T3.REQ_STATUS = '31' THEN ")
                .Append("                 '4' ")
                .Append("            WHEN T3.REQ_STATUS = '32' THEN ")
                .Append("                 '5' ")
                .Append("            ELSE '1' ")
                .Append("       END AS CRACTRESULT ")
                .Append("  FROM TBL_ESTIMATEINFO T1 ")
                .Append("     , TB_H_SALES T2 ")
                .Append("     , TB_H_REQUEST T3 ")
                .Append(" WHERE T1.FLLWUPBOX_SEQNO = T2.SALES_ID ")
                .Append("   AND T2.REQ_ID = T3.REQ_ID ")
                .Append("   AND T1.ESTIMATEID = :ESTIMATEID ")
                '誘致
                .Append("UNION ")
                .Append("SELECT /* SC3070205_013 */ ")
                .Append("       CASE ")
                .Append("            WHEN T3.CONTINUE_ACT_STATUS = '31' THEN ")
                .Append("                 '4' ")
                .Append("            WHEN T3.CONTINUE_ACT_STATUS = '32' THEN ")
                .Append("                 '5' ")
                .Append("            ELSE '1' ")
                .Append("       END AS CRACTRESULT ")
                .Append("  FROM TBL_ESTIMATEINFO T1 ")
                .Append("     , TB_T_SALES T2 ")
                .Append("     , TB_T_ATTRACT T3 ")
                .Append(" WHERE T1.FLLWUPBOX_SEQNO = T2.SALES_ID ")
                .Append("   AND T2.ATT_ID = T3.ATT_ID ")
                .Append("   AND T1.ESTIMATEID = :ESTIMATEID ")
                '誘致(History)
                .Append("UNION ")
                .Append(" SELECT ")
                .Append("       CASE ")
                .Append("            WHEN T3.CONTINUE_ACT_STATUS = '31' THEN ")
                .Append("                 '4' ")
                .Append("            WHEN T3.CONTINUE_ACT_STATUS = '32' THEN ")
                .Append("                 '5' ")
                .Append("            ELSE '1' ")
                .Append("       END AS CRACTRESULT ")
                .Append("  FROM TBL_ESTIMATEINFO T1 ")
                .Append("     , TB_H_SALES T2 ")
                .Append("     , TB_H_ATTRACT T3 ")
                .Append(" WHERE T1.FLLWUPBOX_SEQNO = T2.SALES_ID ")
                .Append("   AND T2.ATT_ID = T3.ATT_ID ")
                .Append("   AND T1.ESTIMATEID = :ESTIMATEID ")
            End With

            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Long, estimateId)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetFollowupboxStatus_End")
            'ログ出力 End *****************************************************************************

            Return query.GetData()

        End Using

    End Function

    ' ''' <summary>
    ' ''' 見積管理ID取得
    ' ''' </summary>
    ' ''' <param name="fllwupboxSeqNo">Follow-up Box内連番</param>
    ' ''' <returns>見積管理IDテーブル</returns>
    ' ''' <remarks></remarks>
    'Public Shared Function GetEstimateId(ByVal fllwUpBoxSeqNo As Decimal) As SC3070205DataSet.SC3070205EstimateIdDataTable
    '    'ログ出力 Start ***************************************************************************
    '    Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetEstimateId_Start")
    '    'ログ出力 End *****************************************************************************

    '    Using query As New DBSelectQuery(Of SC3070205DataSet.SC3070205EstimateIdDataTable)("SC3070205_018")

    '        Dim sql As New StringBuilder
    '        With sql
    '            .Append("SELECT /* SC3070205_018 */ ")
    '            .Append("       ESTIMATEID ")                 '見積管理ID
    '            .Append("  FROM TBL_ESTIMATEINFO ")
    '            .Append(" WHERE FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO ")
    '            .Append(" ORDER BY ESTIMATEID ")
    '        End With

    '        query.CommandText = sql.ToString()
    '        query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwUpBoxSeqNo)      'Follow-up Box内連番
    '        'ログ出力 Start ***************************************************************************
    '        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetEstimateId_End")
    '        'ログ出力 End *****************************************************************************

    '        Return query.GetData()

    '    End Using


    'End Function

#End Region

End Class
