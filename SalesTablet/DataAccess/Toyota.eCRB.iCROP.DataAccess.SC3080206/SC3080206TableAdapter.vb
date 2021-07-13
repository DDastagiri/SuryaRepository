'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3080206DataTableTableAdapter.vb
'─────────────────────────────────────
'機能： 車両編集 (ビジネスロジック)
'補足： 
'作成： 2011/11/15 TCS 安田
'更新： 2013/06/30 TCS 内藤  【A STEP2】i-CROP新DB適応に向けた機能開発（既存流用）
'更新： 2014/04/01 TCS 松月 【A STEP2】TMT不具合対応
'更新： 2014/04/21 TCS 松月 【A STEP2】サービス／保険担当店舗コード設定対応（号口切替BTS-355） 
'更新： 2014/05/01 TCS 松月 新PF残課題No.21
'更新： 2014/05/16 TCS 松月 TR-V4-GTMC140428004対応(仕様変更：活動区分を全車両データに反映)
'更新： 2014/07/24 TCS 外崎 不具合対応（TMT 切替BTS-89）
'更新： 2018/06/18 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1
'─────────────────────────────────────
Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core
'2013/06/30 TCS 内藤 2013/10対応版 既存流用 START
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic

Public NotInheritable Class SC3080206TableAdapter
    '2013/06/30 TCS 内藤 2013/10対応版 既存流用 END

    ''' <summary>
    ''' デフォルトコンストラクタ
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub New()
        '処理なし
    End Sub

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START 
    ''' <summary>
    ''' 自社客車両情報取得
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="vin">VIN</param>
    ''' <param name="originalid">顧客ID</param>
    ''' <returns>SC3080206OrgVehicleDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetOrgVehicle(ByVal dlrcd As String, _
                                  ByVal vin As String, _
                                  ByVal originalid As Decimal) As SC3080206DataSet.SC3080206VehicleDataTable

        Using query As New DBSelectQuery(Of SC3080206DataSet.SC3080206VehicleDataTable)("SC3080206_001")

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetOrgVehicle_Start")
            'ログ出力 End *****************************************************************************

            Dim sql As New StringBuilder
            With sql
                .Append("SELECT DISTINCT ")
                .Append("  /* SC3080206_001 */ ")
                .Append("  T3.MODEL_NAME AS SERIESNM , ")
                .Append("  T10.VCL_VIN AS VIN, ")
                .Append("  T10.REG_NUM AS VCLREGNO, ")
                .Append("  T4.MAKER_NAME AS MAKERNAME, ")
                .Append("  T10.VCL_KATASHIKI AS BASETYPE, ")
                .Append("  T10.GRADE_NAME AS GRADE, ")
                .Append("  T10.FUEL_TYPE AS FUELDVS, ")
                .Append("  T10.BODYCLR_CD AS BDYCLRCD, ")
                .Append("  T10.BODYCLR_NAME AS BDYCLRNM, ")
                .Append("  T10.ENGINE_NUM AS ENGINENO, ")
                .Append("  T10.VCL_TYPE AS NEWVCLDVS, ")
                .Append("  T8.BRN_CD AS STRCD, ")
                .Append("  T10.ACT_CAT_TYPE AS ACTVCTGRYID, ")
                .Append("  T10.OMIT_REASON_CD AS REASONID, ")
                '2014/07/24 TCS 外崎 不具合対応（TMT 切替BTS-89）START
                '.Append("  T10.REG_DATE AS VCLREGDATE, ")
                '.Append("  T10.DELI_DATE AS VCLDELIDATE, ")
                .Append("  CASE WHEN  T10.REG_DATE = TO_DATE('1900/1/1', 'YYYY/MM/DD HH24:MI:SS') THEN ")
                .Append("            NULL ")
                .Append("       ELSE  T10.REG_DATE END AS VCLREGDATE, ")
                .Append("  CASE WHEN  T10.DELI_DATE = TO_DATE('1900/1/1', 'YYYY/MM/DD HH24:MI:SS') THEN ")
                .Append("            NULL ")
                .Append("       ELSE  T10.DELI_DATE END AS VCLDELIDATE, ")
                '2014/07/24 TCS 外崎 不具合対応（TMT 切替BTS-89）END
                .Append("  T9.CPO_TYPE_NAME AS CPONM , ")
                .Append("  T10.VCLLCVER , ")
                .Append("  T10.VCLDLRLCVER , ")
                .Append("  T10.CSTVCLLCVER , ")
                .Append("  T10.VCLID , ")
                ' 2018/06/18 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 START
                .Append("  T10.VCL_MILE , ")
                .Append("  T10.MODEL_YEAR , ")
                .Append("  T10.LC_VCLDLRLCVER ")
                ' 2018/06/18 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 END
                .Append("FROM ")
                .Append("  (SELECT ")
                .Append("    T1.MODEL_CD, ")
                .Append("    T1.GRADE_NAME, ")
                .Append("    T1.SUFFIX_CD, ")
                .Append("    T2.DLR_CD, ")
                .Append("    T7.CST_ID, ")
                .Append("    T1.CPO_TYPE, ")
                .Append("    T1.VCL_VIN, ")
                .Append("    T2.REG_NUM, ")
                .Append("    T1.VCL_KATASHIKI, ")
                .Append("    T1.FUEL_TYPE, ")
                .Append("    T1.BODYCLR_CD, ")
                .Append("    T1.BODYCLR_NAME, ")
                .Append("    T1.ENGINE_NUM, ")
                .Append("    T2.VCL_TYPE, ")
                .Append("    T7.ACT_CAT_TYPE, ")
                .Append("    T7.OMIT_REASON_CD, ")
                .Append("    T2.REG_DATE, ")
                .Append("    T2.DELI_DATE, ")
                .Append("    T1.ROW_LOCK_VERSION AS VCLLCVER , ")
                .Append("    T2.ROW_LOCK_VERSION AS VCLDLRLCVER , ")
                .Append("    T7.ROW_LOCK_VERSION AS CSTVCLLCVER , ")
                .Append("    T1.VCL_ID AS VCLID , ")
                ' 2018/06/18 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 START
                .Append("    TO_CHAR(NVL(T11.VCL_MILE, T2.MAX_MILE)) AS VCL_MILE , ")
                .Append("    NVL(T11.MODEL_YEAR, ' ') AS MODEL_YEAR , ")
                .Append("    NVL(T11.ROW_LOCK_VERSION, 0) AS LC_VCLDLRLCVER ")
                ' 2018/06/18 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 END
                .Append("  FROM ")
                .Append("    TB_M_VEHICLE T1, ")
                .Append("    TB_M_VEHICLE_DLR T2, ")
                .Append("    TB_M_CUSTOMER_VCL T7, ")
                ' 2018/06/18 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 START
                .Append("    TB_LM_VEHICLE_DLR T11 ")
                ' 2018/06/18 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 END
                .Append("  WHERE ")
                .Append("        T1.VCL_ID = T2.VCL_ID ")
                .Append("    AND T1.VCL_ID = T7.VCL_ID ")
                .Append("    AND T2.DLR_CD = T7.DLR_CD ")
                ' 2018/06/18 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 START
                .Append("    AND T2.DLR_CD = T11.DLR_CD(+) ")
                .Append("    AND T2.VCL_ID = T11.VCL_ID(+) ")
                ' 2018/06/18 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 END
                .Append("    AND T1.VCL_VIN = :VIN ")
                .Append("    AND T2.DLR_CD = :DLRCD ")
                .Append("    AND T7.CST_ID = :ORIGINALID) T10, ")
                .Append("  TB_M_MODEL T3, ")
                .Append("  TB_M_MAKER T4, ")
                .Append("  TB_T_SALESBOOKING T8, ")
                .Append("  TB_M_CPO_TYPE T9 ")
                .Append("WHERE ")
                .Append("      T10.MODEL_CD = T3.MODEL_CD(+) ")
                .Append("  AND T3.MAKER_CD = T4.MAKER_CD(+) ")
                .Append("  AND T10.DLR_CD = T8.DLR_CD(+) ")
                .Append("  AND T10.VCL_VIN = T8.VCL_VIN(+) ")
                .Append("  AND T10.CST_ID = T8.CST_ID(+) ")
                .Append("  AND T10.CPO_TYPE = T9.CPO_TYPE(+) ")
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)                '販売店コード
            query.AddParameterWithTypeValue("VIN", OracleDbType.NVarchar2, vin)                    'VIN
            query.AddParameterWithTypeValue("ORIGINALID", OracleDbType.Decimal, originalid)        '顧客ID

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetOrgVehicle_End")
            'ログ出力 End *****************************************************************************
            Return query.GetData()

        End Using

    End Function
    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END


    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 自社客車両最終入庫情報取得
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="vin">VIN</param>
    ''' <param name="originalid">顧客ID</param>
    ''' <returns>SC3080206OrgMileageHisDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetMileageHis(ByVal dlrcd As String, _
                                  ByVal vin As String, _
                                  ByVal originalid As Decimal) As SC3080206DataSet.SC3080206MileageHisDataTable
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetMileageHis_Start")
        'ログ出力 End *****************************************************************************

        Using query As New DBSelectQuery(Of SC3080206DataSet.SC3080206MileageHisDataTable)("SC3080206_002")

            Dim sql As New StringBuilder
            With sql
                .Append("SELECT ")
                .Append("  /* SC3080206_002 */ ")
                .Append("  T4.SVCIN_DELI_DATE AS REGISTDATE, ")
                .Append("  T4.REG_MILE AS MILEAGE ")
                .Append("FROM ")
                .Append("  ( ")
                .Append("  SELECT ")
                .Append("    T3.SVCIN_DELI_DATE, ")
                .Append("    T2.REG_MILE ")
                .Append("  FROM ")
                .Append("    TB_M_VEHICLE T1, ")
                .Append("    TB_T_VEHICLE_MILEAGE T2, ")
                .Append("    TB_T_VEHICLE_SVCIN_HIS T3 ")
                .Append("  WHERE ")
                .Append("        T1.VCL_ID = T2.VCL_ID ")
                .Append("    AND T2.VCL_MILE_ID = T3.VCL_MILE_ID ")
                .Append("    AND T1.VCL_VIN = :VIN ")
                .Append("    AND T2.DLR_CD = :DLRCD ")
                .Append("    AND T2.CST_ID = :ORIGINALID ")
                .Append("  ORDER BY ")
                .Append("    T3.SVCIN_DELI_DATE DESC ")
                .Append("  )T4 ")
                .Append("WHERE ")
                .Append("  ROWNUM =1 ")
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)               '販売店コード
            query.AddParameterWithTypeValue("VIN", OracleDbType.NVarchar2, vin)                   'VIN
            query.AddParameterWithTypeValue("ORIGINALID", OracleDbType.Decimal, originalid)       '顧客ID

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetMileageHis_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

            Return query.GetData()

        End Using

    End Function


    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 未取引客車両情報取得
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="seqno">車両ID</param>
    ''' <returns>SC3080206OrgVehicleDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetNewVehicle(ByVal dlrcd As String, _
                                  ByVal seqno As Decimal) As SC3080206DataSet.SC3080206VehicleDataTable

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetNewVehicle_Start")
        'ログ出力 End *****************************************************************************
        Using query As New DBSelectQuery(Of SC3080206DataSet.SC3080206VehicleDataTable)("SC3080206_003")

            Dim sql As New StringBuilder
            With sql
                .Append("SELECT ")
                .Append("  /* SC3080206_003 */ ")
                .Append("  T2.NEWCST_MODEL_NAME AS SERIESNM, ")
                .Append("  T2.VCL_VIN AS VIN, ")
                .Append("  T1.REG_NUM AS VCLREGNO, ")
                .Append("  T2.NEWCST_MAKER_NAME AS MAKERNAME, ")
                .Append("  CASE WHEN T1.DELI_DATE = TO_DATE('1900/1/1', 'YYYY/MM/DD HH24:MI:SS') THEN ")
                .Append("            NULL ")
                .Append("       ELSE T1.DELI_DATE END AS DELIDATE, ")
                .Append("  T1.VCL_ID AS SEQNO , ")
                .Append("  T1.ROW_LOCK_VERSION AS VCLDLRLCVER , ")
                .Append("  T2.ROW_LOCK_VERSION AS VCLLCVER , ")
                ' 2018/06/18 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 START
                .Append("  NVL(TO_CHAR(T3.VCL_MILE), ' ') AS VCL_MILE , ")
                .Append("  NVL(T3.MODEL_YEAR, ' ') AS MODEL_YEAR , ")
                .Append("  NVL(T3.ROW_LOCK_VERSION, 0) AS LC_VCLDLRLCVER ")
                ' 2018/06/18 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 END
                .Append("FROM ")
                .Append("  TB_M_VEHICLE_DLR T1, ")
                .Append("  TB_M_VEHICLE T2, ")
                ' 2018/06/18 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 START
                .Append("  TB_LM_VEHICLE_DLR T3 ")
                ' 2018/06/18 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 END
                .Append("WHERE ")
                .Append("      T1.VCL_ID = T2.VCL_ID ")
                ' 2018/06/18 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 START
                .Append("  AND T1.DLR_CD = T3.DLR_CD(+) ")
                .Append("  AND T1.VCL_ID = T3.VCL_ID(+) ")
                ' 2018/06/18 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 END
                .Append("  AND T1.DLR_CD = :DLRCD ")
                .Append("  AND T1.VCL_ID = :SEQNO ")
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)             '販売店コード
            query.AddParameterWithTypeValue("SEQNO", OracleDbType.Decimal, seqno)               '車両ID

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetNewVehicle_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

            Return query.GetData()

        End Using

    End Function

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    ''' <summary>
    ''' オーナーサイト情報取得
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="vin">VIN</param>
    ''' <param name="originalid">顧客ID</param>
    ''' <returns>SC3080206OrgOwnersiteDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetOwnersite(ByVal dlrcd As String, _
                                 ByVal vin As String, _
                                 ByVal originalid As Decimal) As SC3080206DataSet.SC3080206OwnersiteDataTable
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetOwnersite_Start")
        'ログ出力 End *****************************************************************************

        'TBLORG_MEMSTATUSはこの条件だとN件取れる可能性があり、
        '       MEMREGSTATUSで何が取れるかわからない
        '修正する必要はない

        Using query As New DBSelectQuery(Of SC3080206DataSet.SC3080206OwnersiteDataTable)("SC3080206_004")

            Dim sql As New StringBuilder
            With sql
                .Append("SELECT ")
                .Append("    /* SC3080206_004 */ ")
                .Append("  MEM_SYSTEM_ID AS MEMSYSTEMID, ")
                .Append("  REG_STATUS AS MEMREGSTATUS ")
                .Append("FROM ")
                .Append("  TB_M_MEMBER ")
                .Append("WHERE ")
                .Append("      DLR_CD = :DLRCD ")
                .Append("  AND VCL_VIN = :VIN ")
                .Append("  AND CST_ID = :ORIGINALID ")
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)                '販売店コード
            query.AddParameterWithTypeValue("VIN", OracleDbType.NVarchar2, vin)                    'VIN
            query.AddParameterWithTypeValue("ORIGINALID", OracleDbType.Decimal, originalid)        '顧客ID

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetOwnersite_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

            Return query.GetData()

        End Using

    End Function

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    ''' <summary>
    ''' G-BOOK情報取得
    ''' </summary>
    ''' <param name="ownersid">オーナーズＩＤ</param>
    ''' <param name="vin">VIN</param>
    ''' <returns>SC3080206OrgGbookDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetGbook(ByVal ownersid As String, _
                             ByVal vin As String, _
                             ByVal dlrcd As String) As SC3080206DataSet.SC3080206GbookDataTable
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetGbook_Start")
        'ログ出力 End *****************************************************************************

        Using query As New DBSelectQuery(Of SC3080206DataSet.SC3080206GbookDataTable)("SC3080206_005")

            Dim sql As New StringBuilder
            With sql
                .Append("SELECT ")
                .Append("  /* SC3080206_005 */ ")
                .Append("  T1.CONTRACT_STATUS, ")
                .Append("  T1.CONTRACT_START_DATE, ")
                .Append("  T1.CONNECT_DVS, ")
                .Append("  T1.TELEMA_TELNUMBER1, ")
                .Append("  T1.TELEMA_TELNUMBER2, ")
                .Append("  T1.TELEMA_TELNUMBER3, ")
                .Append("  T1.CONTRACT_END_DATE, ")
                .Append("  T3.CONTACT_MTD_TLM AS GBOOKFLG ")
                .Append("FROM ")
                .Append("  TBL_TLM_CONTRACT T1, ")
                .Append("  TB_M_VEHICLE T2, ")
                .Append("  TB_M_VEHICLE_DLR T3 ")
                .Append("WHERE ")
                .Append("      T1.VIN = T2.VCL_VIN ")
                .Append("  AND T2.VCL_ID = T3.VCL_ID ")
                .Append("  AND T1.OWNERS_ID = :OWNERSID ")
                .Append("  AND T1.VIN = :VIN ")
                .Append("  AND T1.DELFLG = '0' ")
                .Append("  AND T3.DLR_CD = :DLRCD ")
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("OWNERSID", OracleDbType.Char, ownersid)
            query.AddParameterWithTypeValue("VIN", OracleDbType.Varchar2, vin)
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.Varchar2, dlrcd)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetGbook_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

            Return query.GetData()

        End Using

    End Function

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 未取引客車両情報更新
    ''' </summary>
    ''' <param name="seriesname">モデル名（未取引客）</param>
    ''' <param name="vin">VIN</param>
    ''' <param name="makername">メーカー名（未取引客）</param>
    ''' <param name="acount">更新アカウント</param>
    ''' <param name="seqno">車両ID</param>
    ''' <param name="vcllcver">ロックバージョン</param>
    ''' <returns>更新成功[True]/失敗[False]</returns>
    ''' <remarks></remarks>
    Public Shared Function UpdateNewcustomerVclre(ByVal seriesname As String, _
                                ByVal vin As String, _
                                ByVal makername As String, _
                                ByVal acount As String, _
                                ByVal seqno As Decimal, _
                                ByVal vcllcver As Long) As Integer
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateNewcustomerVclre_Start")
        'ログ出力 End *****************************************************************************

        Dim sql As New StringBuilder
        With sql
            .Append("UPDATE ")
            .Append("    /* SC3080206_006 */ ")
            .Append("    TB_M_VEHICLE ")
            .Append("SET ")
            .Append("    NEWCST_MODEL_NAME = :SERIESNAME, ")
            .Append("    VCL_VIN = :VIN, ")
            .Append("    NEWCST_MAKER_NAME = :MAKERNAME, ")
            .Append("    ROW_UPDATE_DATETIME = SYSDATE, ")
            .Append("    ROW_UPDATE_ACCOUNT = :ACOUNT, ")
            .Append("    ROW_UPDATE_FUNCTION = 'SC3080206', ")
            .Append("    ROW_LOCK_VERSION = :VCLLCVER + 1 ")
            .Append("WHERE ")
            .Append("        VCL_ID = :SEQNO ")
            .Append("    AND ROW_LOCK_VERSION = :VCLLCVER ")
        End With

        Using query As New DBUpdateQuery("SC3080206_006")

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("SERIESNAME", OracleDbType.NVarchar2, seriesname)          'モデル名（未取引客）
            query.AddParameterWithTypeValue("VIN", OracleDbType.NVarchar2, vin)                        'VIN
            query.AddParameterWithTypeValue("MAKERNAME", OracleDbType.NVarchar2, makername)            'メーカー名（未取引客）
            query.AddParameterWithTypeValue("ACOUNT", OracleDbType.NVarchar2, acount)                  '更新アカウント
            query.AddParameterWithTypeValue("SEQNO", OracleDbType.Decimal, seqno)                      '車両ID
            query.AddParameterWithTypeValue("VCLLCVER", OracleDbType.Int64, vcllcver)                  'ロックバージョン

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateNewcustomerVclre_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

            Return query.Execute()

        End Using

    End Function

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    ''' <summary>
    ''' メーカー名取得
    ''' </summary>
    ''' <param name="maker_cd">メーカーコード</param>
    ''' <returns>SC3080206OrgMakerDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetMaker(ByVal maker_cd As String) As SC3080206DataSet.SC3080206MakerDataTable
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetMaker_Start")
        'ログ出力 End *****************************************************************************

        Using query As New DBSelectQuery(Of SC3080206DataSet.SC3080206MakerDataTable)("SC3080206_007")

            Dim sql As New StringBuilder
            With sql
                .Append("SELECT ")
                .Append("  /* SC3080206_007 */ ")
                .Append("  MAKER_NAME AS MAKERNAME ")
                .Append("FROM ")
                .Append("  TB_M_MAKER ")
                .Append("WHERE ")
                .Append("  MAKER_CD = :MAKER_CD ")
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("MAKER_CD", OracleDbType.NVarchar2, maker_cd)            'メーカーコード

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetMaker_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

            Return query.GetData()

        End Using

    End Function

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 未取引客車両情報新規作成
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="cstid">顧客ID</param>
    ''' <param name="seqno">車両ID</param>
    ''' <param name="strcd">セールス担当店舗コード</param>
    ''' <param name="account">アカウント</param>
    ''' <returns>更新成功[True]/失敗[False]</returns>
    ''' <remarks></remarks>
    Public Shared Function InsertNewcustomerVclre(ByVal dlrcd As String, _
                                            ByVal cstid As Decimal, _
                                            ByVal seqno As Decimal, _
                                            ByVal strcd As String, _
                                            ByVal account As String) As Integer
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertNewcustomerVclre_Start")
        'ログ出力 End *****************************************************************************

        Dim sql As New StringBuilder
        With sql
            .Append("INSERT ")
            .Append("    /* SC3080206_008 */ ")
            .Append("INTO TB_M_CUSTOMER_VCL ( ")
            .Append("    DLR_CD, ")
            .Append("    CST_ID, ")
            .Append("    VCL_ID, ")
            .Append("    CST_VCL_TYPE, ")
            .Append("    ACT_CAT_TYPE, ")
            .Append("    OMIT_REASON_CD, ")
            .Append("    ACT_CAT_UPDATE_FUNCTION, ")
            .Append("    SLS_PIC_BRN_CD, ")
            .Append("    SLS_PIC_STF_CD, ")
            .Append("    SVC_PIC_BRN_CD, ")
            .Append("    SVC_PIC_STF_CD, ")
            .Append("    INS_PIC_BRN_CD, ")
            .Append("    INS_PIC_STF_CD, ")
            .Append("    OWNER_CHG_FLG, ")
            .Append("    ROW_CREATE_DATETIME, ")
            .Append("    ROW_CREATE_ACCOUNT, ")
            .Append("    ROW_CREATE_FUNCTION, ")
            .Append("    ROW_UPDATE_DATETIME, ")
            .Append("    ROW_UPDATE_ACCOUNT, ")
            .Append("    ROW_UPDATE_FUNCTION, ")
            .Append("    ROW_LOCK_VERSION ")
            .Append(") ")
            .Append("VALUES ( ")
            .Append("    :DLRCD, ")
            .Append("    :CSTID, ")
            .Append("    :SEQNO, ")
            .Append("    '1', ")
            .Append("    '1', ")
            .Append("    ' ', ")
            ' 2014/04/01 TCS 松月 TMT不具合対応 Modify Start
            .Append("    ' ', ")
            ' 2014/04/01 TCS 松月 TMT不具合対応 Modify End
            .Append("    :STRCD, ")
            .Append("    :ACCOUNT, ")
            '2014/04/21 TCS 松月 【A STEP2】サービス／保険担当店舗コード設定対応（号口切替BTS-355） START
            .Append("    :STRCD_S, ")
            '2014/04/21 TCS 松月 【A STEP2】サービス／保険担当店舗コード設定対応（号口切替BTS-355） END
            .Append("    ' ', ")
            '2014/04/21 TCS 松月 【A STEP2】サービス／保険担当店舗コード設定対応（号口切替BTS-355） START
            .Append("    :STRCD_H, ")
            '2014/04/21 TCS 松月 【A STEP2】サービス／保険担当店舗コード設定対応（号口切替BTS-355） END
            .Append("    ' ', ")
            .Append("    '0', ")
            .Append("    SYSDATE, ")
            .Append("    :ACCOUNT, ")
            .Append("    'SC3080206', ")
            .Append("    SYSDATE, ")
            .Append("    :ACCOUNT, ")
            .Append("    'SC3080206', ")
            .Append("0 ")
            .Append(") ")
        End With

        Using query As New DBUpdateQuery("SC3080206_008")

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)   '販売店コード
            query.AddParameterWithTypeValue("CSTID", OracleDbType.Decimal, cstid)   '顧客ID
            query.AddParameterWithTypeValue("SEQNO", OracleDbType.Decimal, seqno)  '車両ID
            query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, strcd)   'セールス担当店舗コード
            query.AddParameterWithTypeValue("STRCD_S", OracleDbType.NVarchar2, strcd)   'セールス担当店舗コード
            query.AddParameterWithTypeValue("STRCD_H", OracleDbType.NVarchar2, strcd)   'セールス担当店舗コード
            query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, account)   'アカウント

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertNewcustomerVclre_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

            Return query.Execute()

        End Using

    End Function

    ' ''' <summary>
    ' ''' ゲート通過車両情報取得
    ' ''' </summary>
    ' ''' <param name="dlrcd">販売店コード</param>
    ' ''' <param name="strcd">店舗コード</param>
    ' ''' <returns>SC3080206OrgVehicleDataTable</returns>
    ' ''' <remarks></remarks>
    'Public Shared Function GetRegNo(ByVal dlrcd As String, _
    '                         ByVal strcd As String) As SC3080206DataSet.SC3080206RegNoDataTable

    '    Using query As New DBSelectQuery(Of SC3080206DataSet.SC3080206RegNoDataTable)("SC3080206_009")

    '        Dim sql As New StringBuilder
    '        With sql
    '            .Append("SELECT /* SC3080206_009 */ ")
    '            .Append("    VCLREGNO ")
    '            .Append("FROM ")
    '            .Append("    ゲート通過情報 ")
    '            .Append("WHERE ")
    '            .Append("    DLRCD = :DLRCD AND ")
    '            .Append("    STRCD = :STRCD ")
    '        End With

    '        query.CommandText = sql.ToString()
    '        query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)          '販売店コード
    '        query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strcd)          '店舗コード

    '        Return query.GetData()

    '    End Using

    'End Function

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 車両シーケンス采番
    ''' </summary>
    ''' <returns>SC3080206OrgSeqDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetNewcustVclseq() As Long
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetNewcustVclseq_Start")
        'ログ出力 End *****************************************************************************

        Using query As New DBSelectQuery(Of SC3080206DataSet.SC3080206SeqDataTable)("SC3080206_010")

            Dim sql As New StringBuilder
            With sql
                .Append("SELECT ")
                .Append("  /* SC3080206_010 */ ")
                .Append("  SQ_VEHICLE.NEXTVAL AS SEQ ")
                .Append("FROM ")
                .Append("  DUAL ")
            End With
            query.CommandText = sql.ToString()

            Dim seqTbl As SC3080206DataSet.SC3080206SeqDataTable

            seqTbl = query.GetData()

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetNewcustVclseq_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

            Return seqTbl.Item(0).Seq

        End Using

    End Function

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 断念理由リスト取得
    ''' </summary>
    ''' <returns>SC3080206OrgGiveupReasonDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetGiveupReason() As SC3080206DataSet.SC3080206GiveupReasonDataTable
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetGiveupReason_Start")
        'ログ出力 End *****************************************************************************

        Using query As New DBSelectQuery(Of SC3080206DataSet.SC3080206GiveupReasonDataTable)("SC3080206_011")

            Dim sql As New StringBuilder
            With sql
                .Append("SELECT ")
                .Append("  /* SC3080206_011 */ ")
                .Append("  OMIT_OMIT_REASON_CD AS REASONID, ")
                .Append("  OMIT_REASON AS REASON ")
                .Append("FROM ")
                .Append("  TB_M_OMIT_OMIT_REASON ")
                .Append("ORDER BY ")
                .Append("  OMIT_OMIT_REASON_CD ")
            End With
            query.CommandText = sql.ToString()

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetGiveupReason_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

            Return query.GetData()

        End Using

    End Function

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    '2014/05/16 TCS 松月 TR-V4-GTMC140428004対応 Modify Start
    ''' <summary>
    ''' 販売店車両情報更新
    ''' </summary>
    ''' <param name="actvctgryid">活動分類区分</param>
    ''' <param name="acmodffuncdvs">活動分類区分変更機能</param>
    ''' <param name="reasonid">活動除外理由コード</param>
    ''' <param name="updateaccount">更新アカウント</param>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="vclid">車両ID</param>
    ''' <returns>更新成功[True]/失敗[False]</returns>
    ''' <remarks></remarks>
    Public Shared Function UpdateDlrCstVcl(ByVal actvctgryid As String, _
                                    ByVal acmodffuncdvs As String, _
                                    ByVal reasonid As String, _
                                    ByVal updateaccount As String, _
                                    ByVal dlrcd As String, _
                                    ByVal vclid As String) As Integer
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateDlrCstVcl_Start")
        'ログ出力 End *****************************************************************************

        Using query As New DBUpdateQuery("SC3080206_012")

            Dim sql As New StringBuilder
            With sql
                .Append("UPDATE ")
                .Append("    /* SC3080206_012 */ ")
                .Append("    TB_M_CUSTOMER_VCL T1 ")
                .Append("SET ")
                .Append("    T1.ACT_CAT_TYPE = :ACTVCTGRYID, ")
                .Append("    T1.ACT_CAT_UPDATE_FUNCTION = :AC_MODFFUNCDVS, ")
                .Append("    T1.ACT_CAT_UPDATE_DATETIME = SYSDATE, ")
                .Append("    T1.OMIT_REASON_CD = :REASONID, ")
                ' 2014/05/01 TCS 松月 新PF残課題No.21 Modify Start
                .Append("    T1.ACT_CAT_UPDATE_STF_CD = :UPDATEACCOUNT, ")
                ' 2014/05/01 TCS 松月 新PF残課題No.21 Modify End
                .Append("    T1.ROW_UPDATE_DATETIME = SYSDATE, ")
                .Append("    T1.ROW_UPDATE_ACCOUNT = :UPDATEACCOUNT, ")
                .Append("    T1.ROW_UPDATE_FUNCTION = 'SC3080206' ")
                .Append("WHERE ")
                .Append("        T1.DLR_CD = :DLRCD ")
                .Append("    AND T1.VCL_ID = :VCLID ")
                .Append("    AND T1.OWNER_CHG_FLG <> '1' ")
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ACTVCTGRYID", OracleDbType.NVarchar2, actvctgryid)             '活動分類区分
            query.AddParameterWithTypeValue("AC_MODFFUNCDVS", OracleDbType.NVarchar2, acmodffuncdvs)         '活動分類区分変更機能
            query.AddParameterWithTypeValue("REASONID", OracleDbType.NVarchar2, reasonid)                   '活動除外理由コード
            query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.NVarchar2, updateaccount)     '更新アカウント
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)                          '販売店コード
            query.AddParameterWithTypeValue("VCLID", OracleDbType.NVarchar2, vclid)                        'VIN

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateDlrCstVcl_End")
            'ログ出力 End *****************************************************************************
            Return query.Execute()

        End Using
        '2014/05/16 TCS 松月 TR-V4-GTMC140428004対応 Modify End
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

    End Function

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 未取引客車両（車両販売店）情報更新
    ''' </summary>
    ''' <param name="vclregno">車両登録番号 </param>
    ''' <param name="delidate">納車日 </param>
    ''' <param name="acount">更新アカウント </param>
    ''' <param name="dlrcd">販売店コード </param>
    ''' <param name="seqno">車両ID </param>
    ''' <param name="vcldlrlcver">ロックバージョン </param>
    ''' <returns>更新成功[True]/失敗[False]</returns>
    ''' <remarks></remarks>
    Public Shared Function UpdateDlrVcl(ByVal vclregno As String, _
                                      ByVal delidate As Date, _
                                      ByVal acount As String, _
                                      ByVal dlrcd As String, _
                                      ByVal seqno As Decimal, _
                                      ByVal vcldlrlcver As Long) As Integer
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateDlrVcl_Start")
        'ログ出力 End *****************************************************************************

        Dim sql As New StringBuilder
        With sql
            .Append("UPDATE ")
            .Append("    /* SC3080206_101 */ ")
            .Append("    TB_M_VEHICLE_DLR ")
            .Append("SET ")
            .Append("    REG_NUM = :VCLREGNO, ")
            .Append("    DELI_DATE = :DELIDATE, ")
            .Append("    ROW_UPDATE_DATETIME = SYSDATE, ")
            .Append("    ROW_UPDATE_ACCOUNT = :ACOUNT, ")
            .Append("    ROW_UPDATE_FUNCTION = 'SC3080206', ")
            .Append("    ROW_LOCK_VERSION = :VCLDLRLCVER + 1 ")
            .Append("WHERE ")
            .Append("        DLR_CD = :DLRCD ")
            .Append("    AND VCL_ID = :SEQNO ")
            .Append("    AND ROW_LOCK_VERSION = :VCLDLRLCVER ")
        End With
        Using query As New DBUpdateQuery("SC3080206_101")

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("VCLREGNO", OracleDbType.NVarchar2, vclregno)
            query.AddParameterWithTypeValue("DELIDATE", OracleDbType.Date, delidate)
            query.AddParameterWithTypeValue("ACOUNT", OracleDbType.NVarchar2, acount)
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)
            query.AddParameterWithTypeValue("SEQNO", OracleDbType.Decimal, seqno)
            query.AddParameterWithTypeValue("VCLDLRLCVER", OracleDbType.Int64, vcldlrlcver)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateDlrVcl_End")
            'ログ出力 End *****************************************************************************

            Return query.Execute()

        End Using

    End Function
    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START 
    ''' <summary>
    ''' 未取引客車両情報（車両）新規作成
    ''' </summary>
    ''' <param name="seqno">車両ID </param>
    ''' <param name="vin">VIN </param>
    ''' <param name="seriesname">モデル名（未取引客） </param>
    ''' <param name="makername">メーカー名（未取引客） </param>
    ''' <param name="modelcode">車両型式 </param>
    ''' <param name="account">作成アカウント </param>
    ''' <returns>更新成功[True]/失敗[False]</returns>
    ''' <remarks></remarks>
    Public Shared Function InsertVcl(ByVal seqno As Decimal, _
                           ByVal vin As String, _
                           ByVal seriesname As String, _
                           ByVal makername As String, _
                           ByVal modelcode As String, _
                           ByVal account As String) As Integer
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertVcl_Start")
        'ログ出力 End *****************************************************************************

        Dim sql As New StringBuilder
        With sql
            .Append("INSERT ")
            .Append("    /* SC3080206_102 */ ")
            .Append("INTO TB_M_VEHICLE ( ")
            .Append("    VCL_ID, ")
            .Append("    VCL_VIN, ")
            .Append("    MODEL_CD, ")
            .Append("    NEWCST_MODEL_NAME, ")
            .Append("    NEWCST_MAKER_NAME, ")
            .Append("    VCL_KATASHIKI, ")
            .Append("    SUFFIX_CD, ")
            .Append("    ENGINE_CD, ")
            .Append("    ENGINE_NUM, ")
            .Append("    FUEL_TYPE, ")
            .Append("    BODYCLR_CD, ")
            .Append("    INTERIORCLR_CD, ")
            .Append("    MISSION_NAME, ")
            .Append("    UCAR_WARRANTY_TYPE, ")
            .Append("    CPO_TYPE, ")
            .Append("    UPDATE_FUNCTION_JUDGE, ")
            .Append("    ROW_CREATE_DATETIME, ")
            .Append("    ROW_CREATE_ACCOUNT, ")
            .Append("    ROW_CREATE_FUNCTION, ")
            .Append("    ROW_UPDATE_DATETIME, ")
            .Append("    ROW_UPDATE_ACCOUNT, ")
            .Append("    ROW_UPDATE_FUNCTION, ")
            .Append("    ROW_LOCK_VERSION ")
            .Append(") ")
            .Append("VALUES ( ")
            .Append("    :SEQNO, ")
            .Append("    :VIN, ")
            .Append("    ' ', ")
            .Append("    :SERIESNAME, ")
            .Append("    :MAKERNAME, ")
            .Append("    :MODELCODE, ")
            .Append("    ' ', ")
            .Append("    ' ', ")
            .Append("    ' ', ")
            .Append("    ' ', ")
            .Append("    ' ', ")
            .Append("    ' ', ")
            .Append("    ' ', ")
            .Append("    ' ', ")
            .Append("    '9', ")
            .Append("    '11111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111', ")
            .Append("    SYSDATE, ")
            .Append("    :ACCOUNT, ")
            .Append("    'SC3080206', ")
            .Append("    SYSDATE, ")
            .Append("    :ACCOUNT, ")
            .Append("    'SC3080206', ")
            .Append("    0 ")
            .Append(") ")
        End With

        Using query As New DBUpdateQuery("SC3080206_102")

            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("SEQNO", OracleDbType.Decimal, seqno)
            query.AddParameterWithTypeValue("VIN", OracleDbType.NVarchar2, vin)
            query.AddParameterWithTypeValue("SERIESNAME", OracleDbType.NVarchar2, seriesname)
            query.AddParameterWithTypeValue("MAKERNAME", OracleDbType.NVarchar2, makername)
            query.AddParameterWithTypeValue("MODELCODE", OracleDbType.NVarchar2, modelcode)
            query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, account)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertVcl_End")
            'ログ出力 End *****************************************************************************
            Return query.Execute()

        End Using

    End Function
    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START 
    ''' <summary>
    ''' 未取引客車両（販売店車両）情報新規作成
    ''' </summary>
    ''' <param name="dlrcd">販売店コード </param>
    ''' <param name="seqno">車両ID </param>
    ''' <param name="delidate">納車日 </param>
    ''' <param name="vcltranregno">車両登録番号 </param>
    ''' <param name="account">作成アカウント </param>
    ''' <returns>更新成功[True]/失敗[False]</returns>
    ''' <remarks></remarks>
    Public Shared Function InsertDlrVcl(ByVal dlrcd As String, _
                           ByVal seqno As Decimal, _
                           ByVal delidate As Date, _
                           ByVal vcltranregno As String, _
                           ByVal account As String) As Integer
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertDlrVcl_Start")
        'ログ出力 End *****************************************************************************

        Dim sql As New StringBuilder
        With sql
            .Append("INSERT ")
            .Append("    /* SC3080206_103 */ ")
            .Append("INTO TB_M_VEHICLE_DLR ( ")
            .Append("    DLR_CD, ")
            .Append("    VCL_ID, ")
            .Append("    VCL_ACT_TYPE, ")
            .Append("    VCL_ACT_ERROR_TYPE, ")
            .Append("    ATT_SALES_TYPE, ")
            .Append("    ATT_SVC_TYPE, ")
            .Append("    ATT_OTHER_TYPE, ")
            .Append("    DAY_EXP_MILE, ")
            .Append("    MAX_MILE, ")
            .Append("    CONTACT_MTD_TLM, ")
            .Append("    SEVERE_FLG, ")
            .Append("    VCL_TYPE, ")
            .Append("    DELI_DATE, ")
            .Append("    DELI_MILE, ")
            .Append("    REG_NUM, ")
            .Append("    SALESBKG_NUM, ")
            .Append("    LAST_SVCIN_BRN_CD, ")
            .Append("    LAST_SVCIN_MILE, ")
            .Append("    LAST_SVCIN_MAINTE_CD, ")
            .Append("    LAST_SVCIN_SVC_CD, ")
            .Append("    PURCHASE_DLR_FLG, ")
            .Append("    UPDATE_FUNCTION_JUDGE, ")
            .Append("    ROW_CREATE_DATETIME, ")
            .Append("    ROW_CREATE_ACCOUNT, ")
            .Append("    ROW_CREATE_FUNCTION, ")
            .Append("    ROW_UPDATE_DATETIME, ")
            .Append("    ROW_UPDATE_ACCOUNT, ")
            .Append("    ROW_UPDATE_FUNCTION, ")
            .Append("    ROW_LOCK_VERSION ")
            .Append(") ")
            .Append("VALUES ( ")
            .Append("    :DLRCD, ")
            .Append("    :SEQNO, ")
            .Append("    '0', ")
            .Append("    ' ', ")
            .Append("    '1', ")
            .Append("    '1', ")
            .Append("    '1', ")
            .Append("    0, ")
            .Append("    0, ")
            .Append("    '0', ")
            .Append("    '0', ")
            .Append("    '0', ")
            .Append("    :DELIDATE, ")
            .Append("    0, ")
            .Append("    :VCLTRANREGNO, ")
            .Append("    ' ', ")
            .Append("    ' ', ")
            .Append("    0, ")
            .Append("    ' ', ")
            .Append("    ' ', ")
            .Append("    '0', ")
            .Append("    '0', ")
            .Append("    SYSDATE, ")
            .Append("    :ACCOUNT, ")
            .Append("    'SC3080206', ")
            .Append("    SYSDATE, ")
            .Append("    :ACCOUNT, ")
            .Append("    'SC3080206', ")
            .Append("0 ")
            .Append(") ")

        End With

        Using query As New DBUpdateQuery("SC3080206_103")

            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)
            query.AddParameterWithTypeValue("SEQNO", OracleDbType.Decimal, seqno)
            query.AddParameterWithTypeValue("DELIDATE", OracleDbType.Date, delidate)
            query.AddParameterWithTypeValue("VCLTRANREGNO", OracleDbType.NVarchar2, vcltranregno)
            query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, account)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertDlrVcl_End")
            'ログ出力 End *****************************************************************************

            Return query.Execute()

        End Using

    End Function
    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 車両テーブルロック処理
    ''' </summary>
    ''' <param name="seqno">車両ID </param>
    ''' <remarks></remarks>
    Public Shared Sub SelectVehicleForLock(ByVal seqno As Decimal)
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SelectVehicleForLock_Start")
        'ログ出力 End *****************************************************************************
        Using query As New DBSelectQuery(Of DataTable)("SC3080206_105")

            Dim env As New SystemEnvSetting
            Dim sqlForUpdate As String = " FOR UPDATE WAIT " + env.GetLockWaitTime()

            Dim sql As New StringBuilder
            With sql
                .Append("SELECT ")
                .Append("  /* SC3080206_105 */ ")
                .Append("1 ")
                .Append("FROM ")
                .Append("  TB_M_VEHICLE ")
                .Append("WHERE ")
                .Append("  VCL_ID = :SEQNO ")
                .Append(sqlForUpdate)
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("SEQNO", OracleDbType.Decimal, seqno)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SelectVehicleForLock_End")
            'ログ出力 End *****************************************************************************
            query.GetData()


        End Using

    End Sub
    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 定期点検誘致最新化用情報取得
    ''' </summary>
    ''' <param name="vclid">車両ID </param>
    ''' <returns>SC3080206OrgPeriodDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function SelectCstVclforPeriodTgt(ByVal vclid As Decimal) As SC3080206DataSet.SC3080206PeriodDataTable
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SelectCstVclforPeriodTgt_Start")
        'ログ出力 End *****************************************************************************

        Using query As New DBSelectQuery(Of SC3080206DataSet.SC3080206PeriodDataTable)("SC3080206_106")

            Dim sql As New StringBuilder
            With sql
                .Append("SELECT ")
                .Append("  /* SC3080206_106 */ ")
                .Append("  DLR_CD AS DLRCD, ")
                .Append("  VCL_ID AS VCLID ")
                .Append("FROM ")
                .Append("  TB_M_CUSTOMER_VCL ")
                .Append("WHERE ")
                .Append("      VCL_ID = :VCLID ")
                .Append("  AND CST_VCL_TYPE = '1' ")
                .Append("GROUP BY ")
                .Append("  DLR_CD, ")
                .Append("  VCL_ID ")
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("VCLID", OracleDbType.Decimal, vclid)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SelectCstVclforPeriodTgt_End")
            'ログ出力 End *****************************************************************************
            Return query.GetData()

        End Using

    End Function
    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 定期点検誘致最新化シーケンス取得
    ''' </summary>
    ''' <returns>SC3080206OrgPeriodSeqDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetSqPeriodTgt() As SC3080206DataSet.SC3080206PeriodSeqDataTable
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSqPeriodTgt_Start")
        'ログ出力 End *****************************************************************************

        Using query As New DBSelectQuery(Of SC3080206DataSet.SC3080206PeriodSeqDataTable)("SC3080206_107")

            Dim sql As New StringBuilder
            With sql
                .Append("SELECT ")
                .Append("  /* SC3080206_107 */ ")
                .Append("  SQ_PERIODICAL_ATT_NEW.NEXTVAL AS SEQ ")
                .Append("FROM ")
                .Append("  DUAL ")
            End With

            query.CommandText = sql.ToString()

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSqPeriodTgt_End")
            'ログ出力 End *****************************************************************************
            Return query.GetData()

        End Using

    End Function
    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START 
    ''' <summary>
    ''' 定期点検誘致最新化
    ''' </summary>
    ''' <param name="seqno">定期点検誘致最新化ID </param>
    ''' <param name="dlrcd">販売店コード </param>
    ''' <param name="vclid">車両ID </param>
    ''' <param name="account">作成アカウント </param>
    ''' <returns>更新成功[True]/失敗[False]</returns>
    ''' <remarks></remarks>
    Public Shared Function InsertPeriodTgt(ByVal seqno As Decimal,
                           ByVal dlrcd As String,
                           ByVal vclid As Decimal,
                           ByVal account As String) As Integer
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertPeriodTgt_Start")
        'ログ出力 End *****************************************************************************

        Dim sql As New StringBuilder
        With sql
            .Append("INSERT ")
            .Append("    /* SC3080206_108 */ ")
            .Append("INTO TB_T_PERIODICAL_ATT_TGT ( ")
            .Append("    PERIODICAL_ATT_NEW_ID, ")
            .Append("    DLR_CD, ")
            .Append("    VCL_ID, ")
            .Append("    REG_DATETIME, ")
            .Append("    ROW_CREATE_DATETIME, ")
            .Append("    ROW_CREATE_ACCOUNT, ")
            .Append("    ROW_CREATE_FUNCTION, ")
            .Append("    ROW_UPDATE_DATETIME, ")
            .Append("    ROW_UPDATE_ACCOUNT, ")
            .Append("    ROW_UPDATE_FUNCTION, ")
            .Append("    ROW_LOCK_VERSION ")
            .Append(") ")
            .Append("VALUES ( ")
            .Append("    :SEQNO, ")
            .Append("    :DLRCD, ")
            .Append("    :VCLID, ")
            .Append("    SYSDATE, ")
            .Append("    SYSDATE, ")
            .Append("    :ACCOUNT, ")
            .Append("    'SC3080206', ")
            .Append("    SYSDATE, ")
            .Append("    :ACCOUNT, ")
            .Append("    'SC3080206', ")
            .Append("0 ")
            .Append(") ")

        End With

        Using query As New DBUpdateQuery("SC3080206_108")

            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("SEQNO", OracleDbType.Decimal, seqno)
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)
            query.AddParameterWithTypeValue("VCLID", OracleDbType.Decimal, vclid)
            query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, account)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertPeriodTgt_End")
            'ログ出力 End *****************************************************************************

            Return query.Execute()

        End Using

    End Function
    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 誘致グループ所属車両最新化シーケンス取得
    ''' </summary>
    ''' <returns>SC3080206OrgGrpVclSeqDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetSqAttGroupVclTgt() As SC3080206DataSet.SC3080206GrpVclSeqDataTable
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSqAttGroupVclTgt_Start")
        'ログ出力 End *****************************************************************************

        Using query As New DBSelectQuery(Of SC3080206DataSet.SC3080206GrpVclSeqDataTable)("SC3080206_109")

            Dim sql As New StringBuilder
            With sql
                .Append("SELECT ")
                .Append("  /* SC3080206_109 */ ")
                .Append("  SQ_ATTGROUP_VCL_NEW.NEXTVAL AS SEQ ")
                .Append("FROM ")
                .Append("  DUAL ")
            End With

            query.CommandText = sql.ToString()

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSqAttGroupVclTgt_End")
            'ログ出力 End *****************************************************************************
            Return query.GetData()

        End Using

    End Function
    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START 
    ''' <summary>
    ''' 誘致グループ所属車両最新化
    ''' </summary>
    ''' <param name="seqno">定期点検誘致最新化ID </param>
    ''' <param name="dlrcd">販売店コード </param>
    ''' <param name="vclid">車両ID </param>
    ''' <param name="account">作成アカウント </param>
    ''' <returns>更新成功[True]/失敗[False]</returns>
    ''' <remarks></remarks>
    Public Shared Function InsertAttGroupVclTgt(ByVal seqno As Decimal, _
                           ByVal dlrcd As String, _
                           ByVal vclid As Decimal, _
                           ByVal account As String) As Integer
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertAttGroupVclTgt_Start")
        'ログ出力 End *****************************************************************************

        Dim sql As New StringBuilder
        With sql
            .Append("INSERT ")
            .Append("    /* SC3080206_110 */ ")
            .Append("INTO TB_T_ATTGROUP_VCL_NEW_TGT ( ")
            .Append("    ATTGROUP_VCL_NEW_ID, ")
            .Append("    DLR_CD, ")
            .Append("    VCL_ID, ")
            .Append("    REG_DATETIME, ")
            .Append("    ROW_CREATE_DATETIME, ")
            .Append("    ROW_CREATE_ACCOUNT, ")
            .Append("    ROW_CREATE_FUNCTION, ")
            .Append("    ROW_UPDATE_DATETIME, ")
            .Append("    ROW_UPDATE_ACCOUNT, ")
            .Append("    ROW_UPDATE_FUNCTION, ")
            .Append("    ROW_LOCK_VERSION ")
            .Append(") ")
            .Append("VALUES ( ")
            .Append("    :SEQNO, ")
            .Append("    :DLRCD, ")
            .Append("    :VCLID, ")
            .Append("    SYSDATE, ")
            .Append("    SYSDATE, ")
            .Append("    :ACCOUNT, ")
            .Append("    'SC3080206', ")
            .Append("    SYSDATE, ")
            .Append("    :ACCOUNT, ")
            .Append("    'SC3080206', ")
            .Append("0 ")
            .Append(") ")

        End With

        Using query As New DBUpdateQuery("SC3080206_110")

            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("SEQNO", OracleDbType.Decimal, seqno)
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)
            query.AddParameterWithTypeValue("VCLID", OracleDbType.Decimal, vclid)
            query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, account)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertAttGroupVclTgt_End")
            'ログ出力 End *****************************************************************************

            Return query.Execute()

        End Using

    End Function
    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 誘致グループ所属顧客最新化用情報取得
    ''' </summary>
    ''' <param name="cstid">顧客ID </param>
    ''' <returns>SC3080206OrgAttCstDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function SelectAttGroupCstTgt(ByVal cstid As Decimal) As SC3080206DataSet.SC3080206AttCstDataTable
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SelectAttGroupCstTgt_Start")
        'ログ出力 End *****************************************************************************

        Using query As New DBSelectQuery(Of SC3080206DataSet.SC3080206AttCstDataTable)("SC3080206_111")

            Dim sql As New StringBuilder
            With sql
                .Append("SELECT ")
                .Append("  /* SC3080206_111 */ ")
                .Append("  DLR_CD AS DLRCD,")
                .Append("  CST_ID AS CSTID")
                .Append(" FROM ")
                .Append("  TB_M_CUSTOMER_VCL ")
                .Append("WHERE ")
                .Append("      CST_ID = :CSTID ")
                .Append("  AND CST_VCL_TYPE = '1' ")
                .Append("GROUP BY ")
                .Append("  DLR_CD, ")
                .Append("  CST_ID ")
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("CSTID", OracleDbType.Decimal, cstid)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SelectAttGroupCstTgt_End")
            'ログ出力 End *****************************************************************************
            Return query.GetData()

        End Using

    End Function
    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 誘致グループ所属顧客最新化シーケンス取得
    ''' </summary>
    ''' <returns>SC3080206OrgGrpCstSeqDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetSqAttGroupCstTgt() As SC3080206DataSet.SC3080206GrpCstSeqDataTable
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSqAttGroupCstTgt_Start")
        'ログ出力 End *****************************************************************************

        Using query As New DBSelectQuery(Of SC3080206DataSet.SC3080206GrpCstSeqDataTable)("SC3080206_112")

            Dim sql As New StringBuilder
            With sql
                .Append("SELECT ")
                .Append("  /* SC3080206_112 */ ")
                .Append("  SQ_ATTGROUP_CST_NEW.NEXTVAL AS SEQ ")
                .Append("FROM ")
                .Append("  DUAL ")
            End With

            query.CommandText = sql.ToString()

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSqAttGroupCstTgt_End")
            'ログ出力 End *****************************************************************************
            Return query.GetData()

        End Using

    End Function
    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 誘致グループ所属顧客最新化
    ''' </summary>
    ''' <param name="seqno">定期点検誘致最新化ID </param>
    ''' <param name="dlrcd">販売店コード </param>
    ''' <param name="cstid">顧客ID </param>
    ''' <param name="account">作成アカウント </param>
    ''' <returns>更新成功[True]/失敗[False]</returns>
    ''' <remarks></remarks>
    Public Shared Function InsertAttGroupCstTgt(ByVal seqno As Decimal, _
                           ByVal dlrcd As String, _
                           ByVal cstid As Decimal, _
                           ByVal account As String) As Integer
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertAttGroupCstTgt_Start")
        'ログ出力 End *****************************************************************************

        Dim sql As New StringBuilder
        With sql
            .Append("INSERT ")
            .Append("/* SC3080206_113 */ ")
            .Append("INTO TB_T_ATTGROUP_CST_NEW_TGT ( ")
            .Append("    ATTGROUP_CST_NEW_ID, ")
            .Append("    DLR_CD, ")
            .Append("    CST_ID, ")
            .Append("    REG_DATETIME, ")
            .Append("    ROW_CREATE_DATETIME, ")
            .Append("    ROW_CREATE_ACCOUNT, ")
            .Append("    ROW_CREATE_FUNCTION, ")
            .Append("    ROW_UPDATE_DATETIME, ")
            .Append("    ROW_UPDATE_ACCOUNT, ")
            .Append("    ROW_UPDATE_FUNCTION, ")
            .Append("    ROW_LOCK_VERSION ")
            .Append(") ")
            .Append("VALUES ( ")
            .Append("    :SEQNO, ")
            .Append("    :DLRCD, ")
            .Append("    :CSTID, ")
            .Append("    SYSDATE, ")
            .Append("    SYSDATE, ")
            .Append("    :ACCOUNT, ")
            .Append("    'SC3080206', ")
            .Append("    SYSDATE, ")
            .Append("    :ACCOUNT, ")
            .Append("    'SC3080206', ")
            .Append("0 ")
            .Append(") ")
        End With

        Using query As New DBUpdateQuery("SC3080206_113")

            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("SEQNO", OracleDbType.Decimal, seqno)
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)
            query.AddParameterWithTypeValue("CSTID", OracleDbType.Decimal, cstid)
            query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, account)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertAttGroupCstTgt_End")
            'ログ出力 End *****************************************************************************

            Return query.Execute()

        End Using

    End Function
    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 起点日誘致最新化シーケンス取得
    ''' </summary>
    ''' <returns>SC3080206OrgSpecifySeqDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetSqSpecifyTgt() As SC3080206DataSet.SC3080206SpecifySeqDataTable
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSqSpecifyTgt_Start")
        'ログ出力 End *****************************************************************************

        Using query As New DBSelectQuery(Of SC3080206DataSet.SC3080206SpecifySeqDataTable)("SC3080206_114")

            Dim sql As New StringBuilder
            With sql
                .Append("SELECT ")
                .Append("  /* SC3080206_114 */ ")
                .Append("  SQ_SPECIFY_ATT_TGT.NEXTVAL AS SEQ ")
                .Append("FROM ")
                .Append("  DUAL ")
            End With

            query.CommandText = sql.ToString()
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSqSpecifyTgt_End")
            'ログ出力 End *****************************************************************************

            Return query.GetData()

        End Using

    End Function
    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START 
    ''' <summary>
    ''' 起点日誘致最新化
    ''' </summary>
    ''' <param name="seqno">定期点検誘致最新化ID </param>
    ''' <param name="dlrcd">販売店コード </param>
    ''' <param name="cstid">顧客ID </param>
    ''' <param name="vclid">車両ID </param>
    ''' <param name="account">作成アカウント </param>
    ''' <returns>更新成功[True]/失敗[False]</returns>
    ''' <remarks></remarks>
    Public Shared Function InsertSpecifyTgt(ByVal seqno As Decimal, _
                           ByVal dlrcd As String, _
                           ByVal cstid As Decimal, _
                           ByVal vclid As Decimal, _
                           ByVal account As String) As Integer
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertSpecifyTgt_Start")
        'ログ出力 End *****************************************************************************

        Dim sql As New StringBuilder
        With sql
            .Append("INSERT ")
            .Append("    /* SC3080206_115 */ ")
            .Append("INTO TB_T_SPECIFY_ATT_TGT ( ")
            .Append("    SPECIFY_ATT_TGT_ID, ")
            .Append("    DLR_CD, ")
            .Append("    SPECIFY_TYPE, ")
            .Append("    CST_ID, ")
            .Append("    VCL_ID, ")
            .Append("    SVCIN_NUM, ")
            .Append("    REG_DATETIME, ")
            .Append("    ROW_CREATE_DATETIME, ")
            .Append("    ROW_CREATE_ACCOUNT, ")
            .Append("    ROW_CREATE_FUNCTION, ")
            .Append("    ROW_UPDATE_DATETIME, ")
            .Append("    ROW_UPDATE_ACCOUNT, ")
            .Append("    ROW_UPDATE_FUNCTION, ")
            .Append("    ROW_LOCK_VERSION ")
            .Append(") ")
            .Append("VALUES ( ")
            .Append("    :SEQNO, ")
            .Append("    :DLRCD, ")
            .Append("    '01', ")
            .Append("    :CSTID, ")
            .Append("    :VCLID, ")
            .Append("    ' ', ")
            .Append("    SYSDATE, ")
            .Append("    SYSDATE, ")
            .Append("    :ACCOUNT, ")
            .Append("    'SC3080206', ")
            .Append("    SYSDATE, ")
            .Append("    :ACCOUNT, ")
            .Append("    'SC3080206', ")
            .Append("0 ")
            .Append(") ")

        End With

        Using query As New DBUpdateQuery("SC3080206_115")

            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("SEQNO", OracleDbType.Decimal, seqno)
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)
            query.AddParameterWithTypeValue("CSTID", OracleDbType.Decimal, cstid)
            query.AddParameterWithTypeValue("VCLID", OracleDbType.Decimal, vclid)
            query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, account)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertSpecifyTgt_End")
            'ログ出力 End *****************************************************************************

            Return query.Execute()

        End Using

    End Function
    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 誘致最新化用情報取得
    ''' </summary>
    ''' <param name="cstid">顧客ID </param>
    ''' <returns>SC3080206OrgPlanNewDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function SelectPlanNewTgt(ByVal cstid As Decimal) As SC3080206DataSet.SC3080206PlanNewDataTable
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SelectPlanNewTgt_Start")
        'ログ出力 End *****************************************************************************

        Using query As New DBSelectQuery(Of SC3080206DataSet.SC3080206PlanNewDataTable)("SC3080206_116")

            Dim sql As New StringBuilder
            With sql
                .Append("SELECT ")
                .Append("  /* SC3080206_116 */ ")
                .Append("  DLR_CD AS DLRCD, ")
                .Append("  CST_ID AS CSTID, ")
                .Append("  VCL_ID AS VCLID ")
                .Append("FROM ")
                .Append("  TB_M_CUSTOMER_VCL ")
                .Append("WHERE ")
                .Append("      CST_ID = :CSTID ")
                .Append("  AND CST_VCL_TYPE = '1' ")
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("CSTID", OracleDbType.Decimal, cstid)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SelectPlanNewTgt_End")
            'ログ出力 End *****************************************************************************

            Return query.GetData()

        End Using

    End Function
    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 誘致最新化シーケンス取得
    ''' </summary>
    ''' <returns>SC3080206OrgPlanSeqDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetSqPlanNewTgt() As SC3080206DataSet.SC3080206PlanSeqDataTable
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSqPlanNewTgt_Start")
        'ログ出力 End *****************************************************************************

        Using query As New DBSelectQuery(Of SC3080206DataSet.SC3080206PlanSeqDataTable)("SC3080205_117")

            Dim sql As New StringBuilder
            With sql
                .Append("SELECT ")
                .Append("  /* SC3080205_117 */ ")
                .Append("  SQ_ATT_NEW_TGT.NEXTVAL AS SEQ ")
                .Append("FROM ")
                .Append("  DUAL ")
            End With

            query.CommandText = sql.ToString()
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSqPlanNewTgt_End")
            'ログ出力 End *****************************************************************************

            Return query.GetData()

        End Using

    End Function
    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START 
    ''' <summary>
    ''' 誘致グループ所属顧客最新化
    ''' </summary>
    ''' <param name="seqno">計画最新化対象ID </param>
    ''' <param name="dlrcd">販売店コード </param>
    ''' <param name="cstid">顧客ID </param>
    ''' <param name="vclid">車両ID </param>
    ''' <param name="account">作成アカウント </param>
    ''' <returns>更新成功[True]/失敗[False]</returns>
    ''' <remarks></remarks>
    Public Shared Function InsertPlanNewTgt(ByVal seqno As Decimal, _
                           ByVal dlrcd As String, _
                           ByVal cstid As Decimal, _
                           ByVal vclid As Decimal, _
                           ByVal account As String) As Integer
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertPlanNewTgt_Start")
        'ログ出力 End *****************************************************************************

        Dim sql As New StringBuilder

        With sql
            .Append("INSERT ")
            .Append("    /* SC3080206_118 */ ")
            .Append("INTO TB_T_ATT_NEW_TGT ( ")
            .Append("    PLAN_NEW_TGT_ID, ")
            .Append("    DLR_CD, ")
            .Append("    CST_ID, ")
            .Append("    VCL_ID, ")
            .Append("    REG_DATETIME, ")
            .Append("    ROW_CREATE_DATETIME, ")
            .Append("    ROW_CREATE_ACCOUNT, ")
            .Append("    ROW_CREATE_FUNCTION, ")
            .Append("    ROW_UPDATE_DATETIME, ")
            .Append("    ROW_UPDATE_ACCOUNT, ")
            .Append("    ROW_UPDATE_FUNCTION, ")
            .Append("    ROW_LOCK_VERSION ")
            .Append(") ")
            .Append("VALUES ( ")
            .Append("    :SEQNO, ")
            .Append("    :DLRCD, ")
            .Append("    :CSTID, ")
            .Append("    :VCLID, ")
            .Append("    SYSDATE, ")
            .Append("    SYSDATE, ")
            .Append("    :ACCOUNT, ")
            .Append("    'SC3080206', ")
            .Append("    SYSDATE, ")
            .Append("    :ACCOUNT, ")
            .Append("    'SC3080206', ")
            .Append("0 ")
            .Append(") ")
        End With

        Using query As New DBUpdateQuery("SC3080206_118")

            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("SEQNO", OracleDbType.Decimal, seqno)
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)
            query.AddParameterWithTypeValue("CSTID", OracleDbType.Decimal, cstid)
            query.AddParameterWithTypeValue("VCLID", OracleDbType.Decimal, vclid)
            query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, account)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertPlanNewTgt_End")
            'ログ出力 End *****************************************************************************

            Return query.Execute()

        End Using

    End Function
    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END 

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 未取引客車両情報初期情報削除
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="cstid">顧客ID</param>
    ''' <returns>更新成功[True]/失敗[False]</returns>
    ''' <remarks></remarks>
    Public Shared Function DeleteNewcustomerVclre(ByVal dlrcd As String, _
                                            ByVal cstid As Decimal) As Integer
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeleteNewcustomerVclre_Start")
        'ログ出力 End *****************************************************************************

        Dim sql As New StringBuilder
        With sql
            .Append("DELETE ")
            .Append("    /* SC3080206_110 */ ")
            .Append("FROM ")
            .Append("    TB_M_CUSTOMER_VCL ")
            .Append("WHERE ")
            .Append("        DLR_CD = :DLRCD ")
            .Append("    AND CST_ID = :CSTID ")
            .Append("    AND VCL_ID = 0 ")
        End With

        Using query As New DBUpdateQuery("SC3080206_110")

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)   '販売店コード
            query.AddParameterWithTypeValue("CSTID", OracleDbType.Decimal, cstid)   '顧客ID

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeleteNewcustomerVclre_End")
            'ログ出力 End *****************************************************************************

            Return query.Execute()

        End Using

    End Function
    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

    '2013/05/01 TCS 松月 新PF残課題No.21 Start
    ''' <summary>
    ''' 活動分類区分変更履歴新規作成
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="cstid">自社客連番</param>
    ''' <param name="actvctgryid">AC</param>
    ''' <param name="reasonid">活動除外理由ID</param>
    ''' <param name="updateaccount">更新アカウント</param>
    ''' <returns>更新成功[True]/失敗[False]</returns>
    ''' <remarks></remarks>
    Public Shared Function InsertCstVclActCat(ByVal dlrcd As String, _
                                      ByVal cstid As String, _
                                      ByVal actvctgryid As String,
                                      ByVal reasonid As String, _
                                      ByVal updateaccount As String,
                                      ByVal vclid As String) As Integer

        Dim sql As New StringBuilder
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertCstVclActCat_Start")
        'ログ出力 End *****************************************************************************

        With sql
            .Append("INSERT ")
            .Append("    /* SC3080206_231 */ ")
            .Append("INTO TB_M_CUSTOMER_VCL_ACT_CAT ( ")
            .Append("    DLR_CD, ")
            .Append("    CST_ID, ")
            .Append("    VCL_ID, ")
            .Append("    CST_VCL_TYPE, ")
            .Append("    CST_VCL_ACT_CAT_SEQ, ")
            .Append("    CHG_DATETIME, ")
            .Append("    CHG_STF_CD, ")
            .Append("    CHG_REASON, ")
            .Append("    ACT_CAT_TYPE, ")
            .Append("    OMIT_REASON_CD, ")
            .Append("    ROW_CREATE_DATETIME, ")
            .Append("    ROW_CREATE_ACCOUNT, ")
            .Append("    ROW_CREATE_FUNCTION, ")
            .Append("    ROW_UPDATE_DATETIME, ")
            .Append("    ROW_UPDATE_ACCOUNT, ")
            .Append("    ROW_UPDATE_FUNCTION, ")
            .Append("    ROW_LOCK_VERSION ")
            .Append(") ")
            .Append("VALUES ( ")
            .Append("    :DLRCD, ")
            .Append("    :CSTID, ")
            .Append("    :VCLID, ")
            .Append("    '1', ")
            .Append("    NVL( (SELECT MAX(CST_VCL_ACT_CAT_SEQ) FROM TB_M_CUSTOMER_VCL_ACT_CAT WHERE DLR_CD = :DLRCD AND CST_ID = :CSTID AND VCL_ID = :VCLID ),0) + 1, ")
            .Append("    SYSDATE, ")
            .Append("    :UPDATEACCOUNT, ")
            .Append("    ' ', ")
            If actvctgryid = "" Then
                .Append("    ' ', ")
            Else
                .Append("    :ACTCATID, ")
            End If
            If reasonid = "" Then
                .Append("    ' ', ")
            Else
                .Append("    :REASONID, ")
            End If
            .Append("    SYSDATE, ")
            .Append("    :UPDATEACCOUNT, ")
            .Append("    'SC3080206', ")
            .Append("    SYSDATE, ")
            .Append("    :UPDATEACCOUNT, ")
            .Append("    'SC3080206', ")
            .Append("0 ")
            .Append(") ")
        End With

        Using query As New DBUpdateQuery("SC3080206_231")

            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)
            query.AddParameterWithTypeValue("CSTID", OracleDbType.Decimal, cstid)
            query.AddParameterWithTypeValue("VCLID", OracleDbType.Decimal, vclid)
            If actvctgryid = "" Then
            Else
                query.AddParameterWithTypeValue("ACTCATID", OracleDbType.NVarchar2, actvctgryid)
            End If
            If reasonid = "" Then
            Else
                query.AddParameterWithTypeValue("REASONID", OracleDbType.NVarchar2, reasonid)
            End If

            query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.NVarchar2, updateaccount)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertCstVclActCat_End")
            'ログ出力 End *****************************************************************************

            Return query.Execute()

        End Using

    End Function
    '2013/05/01 TCS 松月 新PF残課題No.21 End

    '2018/06/18 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 START
    ''' <summary>
    ''' 販売店車両ローカル新規作成
    ''' </summary>
    ''' <param name="dlrcd">販売店コード </param>
    ''' <param name="vclid">車両ID </param>
    ''' <param name="vclMile">走行距離 </param>
    ''' <param name="modelYear">年式 </param>
    ''' <param name="account">作成アカウント </param>
    ''' <returns>更新成功[True]/失敗[False]</returns>
    ''' <remarks></remarks>
    Public Shared Function InsertDlrVclLocal(ByVal dlrcd As String, _
                           ByVal vclid As Decimal, _
                           ByVal vclMile As String, _
                           ByVal modelYear As String, _
                           ByVal account As String) As Integer
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("InsertDlrVclLocal_Start")
        'ログ出力 End *****************************************************************************

        Dim sql As New StringBuilder
        With sql
            .Append("INSERT ")
            .Append("    /* SC3080206_120 */ ")
            .Append("INTO TB_LM_VEHICLE_DLR ( ")
            .Append("    DLR_CD, ")
            .Append("    VCL_ID, ")
            .Append("    VCL_MILE, ")
            .Append("    MODEL_YEAR, ")
            .Append("    ROW_CREATE_DATETIME, ")
            .Append("    ROW_CREATE_ACCOUNT, ")
            .Append("    ROW_CREATE_FUNCTION, ")
            .Append("    ROW_UPDATE_DATETIME, ")
            .Append("    ROW_UPDATE_ACCOUNT, ")
            .Append("    ROW_UPDATE_FUNCTION, ")
            .Append("    ROW_LOCK_VERSION ")
            .Append(") ")
            .Append("VALUES ( ")
            .Append("    :DLR_CD, ")
            .Append("    :VCL_ID, ")
            .Append("    :VCL_MILE, ")
            .Append("    :MODEL_YEAR, ")
            .Append("    SYSDATE, ")
            .Append("    :ACCOUNT, ")
            .Append("    'SC3080206', ")
            .Append("    SYSDATE, ")
            .Append("    :ACCOUNT, ")
            .Append("    'SC3080206', ")
            .Append("    0 ")
            .Append(") ")
        End With

        Using query As New DBUpdateQuery("SC3080206_120")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dlrcd)
            query.AddParameterWithTypeValue("VCL_ID", OracleDbType.Decimal, vclid)
            If String.IsNullOrWhiteSpace(vclMile) Then
                query.AddParameterWithTypeValue("VCL_MILE", OracleDbType.Double, 0)
            Else
                query.AddParameterWithTypeValue("VCL_MILE", OracleDbType.Double, vclMile)
            End If
            query.AddParameterWithTypeValue("MODEL_YEAR", OracleDbType.NVarchar2, modelYear)
            query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, account)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("InsertDlrVclLocal_End")
            'ログ出力 End *****************************************************************************

            Return query.Execute()
        End Using

    End Function

    ''' <summary>
    ''' 販売店車両ローカル更新
    ''' </summary>
    ''' <param name="vclMile">走行距離 </param>
    ''' <param name="modelYear">年式 </param>
    ''' <param name="acount">更新アカウント </param>
    ''' <param name="dlrcd">販売店コード </param>
    ''' <param name="seqno">車両ID </param>
    ''' <param name="vcldlrlcver">ロックバージョン </param>
    ''' <returns>更新成功[True]/失敗[False]</returns>
    ''' <remarks></remarks>
    Public Shared Function UpdateDlrVclLocal(ByVal vclMile As String, _
                                      ByVal modelYear As String, _
                                      ByVal acount As String, _
                                      ByVal dlrcd As String, _
                                      ByVal seqno As Decimal, _
                                      ByVal vcldlrlcver As Long) As Integer
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("UpdateDlrVclLocal_Start")
        'ログ出力 End *****************************************************************************

        Dim sql As New StringBuilder
        With sql
            .Append("UPDATE ")
            .Append("    /* SC3080206_121 */ ")
            .Append("    TB_LM_VEHICLE_DLR ")
            .Append("SET ")
            .Append("    VCL_MILE = :VCL_MILE, ")
            .Append("    MODEL_YEAR = :MODEL_YEAR, ")
            .Append("    ROW_UPDATE_DATETIME = SYSDATE, ")
            .Append("    ROW_UPDATE_ACCOUNT = :ACOUNT, ")
            .Append("    ROW_UPDATE_FUNCTION = 'SC3080206', ")
            .Append("    ROW_LOCK_VERSION = :VCLDLRLCVER + 1 ")
            .Append("WHERE ")
            .Append("        DLR_CD = :DLR_CD ")
            .Append("    AND VCL_ID = :VCL_ID ")
            .Append("    AND ROW_LOCK_VERSION = :VCLDLRLCVER ")
        End With

        Using query As New DBUpdateQuery("SC3080206_121")
            query.CommandText = sql.ToString()
            If String.IsNullOrWhiteSpace(vclMile) Then
                query.AddParameterWithTypeValue("VCL_MILE", OracleDbType.Double, 0)
            Else
                query.AddParameterWithTypeValue("VCL_MILE", OracleDbType.Double, vclMile)
            End If
            query.AddParameterWithTypeValue("MODEL_YEAR", OracleDbType.NVarchar2, modelYear)
            query.AddParameterWithTypeValue("ACOUNT", OracleDbType.NVarchar2, acount)
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dlrcd)
            query.AddParameterWithTypeValue("VCL_ID", OracleDbType.Decimal, seqno)
            query.AddParameterWithTypeValue("VCLDLRLCVER", OracleDbType.Int64, vcldlrlcver)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("UpdateDlrVclLocal_End")
            'ログ出力 End *****************************************************************************

            Return query.Execute()
        End Using

    End Function

    ''' <summary>
    ''' システム設定値取得
    ''' </summary>
    ''' <param name="settingName">システム設定名</param>
    ''' <returns>SC3080206SystemSettingDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetSystemSetting(ByVal settingName As String) As SC3080206DataSet.SC3080206SystemSettingDataTable

        Dim sql As New StringBuilder

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetSystemSetting_Start")
        'ログ出力 End *****************************************************************************

        With sql
            .Append("SELECT ")
            .Append("    /* SC3080206_122 */ ")
            .Append("    SETTING_VAL ")
            .Append("FROM ")
            .Append("    TB_M_SYSTEM_SETTING ")
            .Append("WHERE ")
            .Append("    SETTING_NAME = :SETTING_NAME ")
        End With

        Using query As New DBSelectQuery(Of SC3080206DataSet.SC3080206SystemSettingDataTable)("SC3080206_122")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("SETTING_NAME", OracleDbType.NVarchar2, settingName)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetSystemSetting_End")
            'ログ出力 End *****************************************************************************

            Return query.GetData()
        End Using
    End Function

    ''' <summary>
    ''' 販売店車両ローカル取得
    ''' </summary>
    ''' <param name="dlrcd"></param>
    ''' <param name="vclid"></param>
    ''' <returns>件数</returns>
    ''' <remarks>販売店車両ローカルの存在確認</remarks>
    Public Shared Function GetCountDlrVclLocal(ByVal dlrcd As String, ByVal vclid As Decimal) As Integer
        Dim sql As New StringBuilder
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetCountDlrVclLocal_Start")
        'ログ出力 End *****************************************************************************
        With sql
            .Append("SELECT /* SC3080206_123 */ ")
            .Append("    COUNT(1) AS CNT ")
            .Append("FROM ")
            .Append("    TB_LM_VEHICLE_DLR ")
            .Append("WHERE ")
            .Append("    DLR_CD = :DLR_CD AND ")
            .Append("    VCL_ID = :VCL_ID AND ")
            .Append("    ROWNUM <= 1 ")
        End With

        Using query As New DBSelectQuery(Of SC3080206DataSet.SC3080206CountDataTable)("SC3080206_123")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dlrcd)
            query.AddParameterWithTypeValue("VCL_ID", OracleDbType.Decimal, vclid)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetCountDlrVclLocal_End")
            'ログ出力 End *****************************************************************************
            Return query.GetCount()
        End Using
    End Function
    '2018/06/18 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 END 

    Public Function GetStringValue(ByVal val As String) As String
        Return val
    End Function

End Class

