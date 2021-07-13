'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3010203TableAdapter.vb
'─────────────────────────────────────
'機能： SCメイン
'補足： 
'作成： 2011/11/18 TCS 寺本
'更新： 2014/02/26 TCS 河原
'更新： 2014/11/10 TCS 河原 TMT 切替BTS-201
'更新： 2015/03/30 TCS 山口 TMT M009
'更新： 2020/06/17  TS 山口 TR-SLT-TKM-20200616-001
'─────────────────────────────────────

Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Globalization
Imports System.Reflection
Imports System.Reflection.MethodBase

''' <summary>
''' SCメインのデータアクセスクラスです。
''' </summary>
''' <remarks></remarks>
Public NotInheritable Class SC3010203TableAdapter
    Inherits Global.System.ComponentModel.Component

#Region "定数/Enum"
    '見積情報-契約済
    Private Const DONE_CONTRACT As String = "1"

    Private Const C_FLAG_ON As String = "1"
    Private Const C_FLAG_OFF As String = "0"

#End Region

#Region "メンバ変数"
    Private DlrCd As String
    Private StrCd As String
    Private UserId As String
#End Region

#Region "コンストラクタ"
    Public Sub New(ByVal dlrcd As String, ByVal strcd As String, ByVal userid As String)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
          "{0} Start > Params:dlrcd=[{1}] strcd=[{2}] userid=[{3}]", _
          GetCurrentMethod().Name, _
          dlrcd, _
          strcd, _
          userid))
        Me.DlrCd = dlrcd
        Me.StrCd = strcd
        Me.UserId = userid
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0} End", GetCurrentMethod().Name))
    End Sub
#End Region


    ''' <summary>
    ''' チップ背景色取得
    ''' </summary>
    ''' <param name="dlrCD">販売店コード</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Public Shared Function ReadChipColorSetting(ByVal dlrCD As String) As SC3010203DataSet.SC3010203TodoColorDataTable

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_Start", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        Using query As New DBSelectQuery(Of SC3010203DataSet.SC3010203TodoColorDataTable)("SC3010203_001")

            Dim sql As New StringBuilder

            'SQL文作成
            With sql
                .Append("SELECT /* SC3010203_001 */ ")
                .Append("    A.CREATEDATADIV, ")
                .Append("    A.SCHEDULEDVS, ")
                .Append("    A.CONTACTNO, ")
                .Append("    A.PROCESSCD, ")
                .Append("    A.BACKGROUNDCOLOR, ")
                .Append("    CASE ")
                .Append("    WHEN A.PROCESSCD = '0' THEN ")
                .Append("        NVL(T4.ICON_PATH, T5.ICON_PATH) ")
                .Append("    ELSE ")
                .Append("        NVL(T8.ICON_PATH, T9.ICON_PATH) ")
                .Append("    END AS ICONPATH ")
                .Append("FROM ")
                .Append("       TBL_TODO_TIP_COLOR A, ")
                .Append("    TB_M_CONTACT_MTD T1, ")
                .Append("    ( ")
                .Append("    SELECT ")
                .Append("        T2.FIRST_KEY, ")
                .Append("        T2.ICON_PATH ")
                .Append("    FROM ")
                .Append("        TB_M_IMG_PATH_CONTROL T2 ")
                .Append("    WHERE ")
                .Append("            T2.DLR_CD = :DLR_CD ")
                .Append("        AND T2.TYPE_CD = 'CONTACT_MTD' ")
                .Append("        AND T2.DEVICE_TYPE = '01' ")
                .Append("        AND T2.SECOND_KEY = ' ' ")
                .Append("    ) T4, ")
                .Append("    ( ")
                .Append("    SELECT ")
                .Append("        T3.FIRST_KEY, ")
                .Append("        T3.ICON_PATH ")
                .Append("    FROM ")
                .Append("        TB_M_IMG_PATH_CONTROL T3 ")
                .Append("    WHERE ")
                .Append("            T3.DLR_CD = 'XXXXX' ")
                .Append("        AND T3.TYPE_CD = 'CONTACT_MTD' ")
                .Append("        AND T3.DEVICE_TYPE = '01' ")
                .Append("        AND T3.SECOND_KEY = ' ' ")
                .Append("    ) T5, ")
                .Append("    ( ")
                .Append("    SELECT ")
                .Append("        T6.FIRST_KEY, ")
                .Append("        T6.ICON_PATH ")
                .Append("    FROM ")
                .Append("        TB_M_IMG_PATH_CONTROL T6 ")
                .Append("    WHERE ")
                .Append("            T6.DLR_CD = :DLR_CD ")
                .Append("        AND T6.TYPE_CD = 'AFTER_ODR_PRCS_CD' ")
                .Append("        AND T6.DEVICE_TYPE = '01' ")
                .Append("        AND T6.SECOND_KEY = ' ' ")
                .Append("    ) T8, ")
                .Append("    ( ")
                .Append("    SELECT ")
                .Append("        T7.FIRST_KEY, ")
                .Append("        T7.ICON_PATH ")
                .Append("    FROM ")
                .Append("        TB_M_IMG_PATH_CONTROL T7 ")
                .Append("    WHERE ")
                .Append("            T7.DLR_CD = 'XXXXX' ")
                .Append("        AND T7.TYPE_CD = 'AFTER_ODR_PRCS_CD' ")
                .Append("        AND T7.DEVICE_TYPE = '01' ")
                .Append("        AND T7.SECOND_KEY = ' ' ")
                .Append("    ) T9 ")
                .Append("WHERE ")
                .Append("        T1.CONTACT_MTD = T4.FIRST_KEY(+) ")
                .Append("    AND T1.CONTACT_MTD = T5.FIRST_KEY(+) ")
                '2014/03/13 TCS 葛西 TR-V4-GTMC140224007 START
                '2014/03/13 TCS 葛西 TR-V4-GTMC140224007 END
                .Append("    AND TO_CHAR(A.CONTACTNO) = T1.CONTACT_MTD(+) ")
                .Append("    AND RTRIM(A.PROCESSCD) = T8.FIRST_KEY(+) ")
                .Append("    AND RTRIM(A.PROCESSCD) = T9.FIRST_KEY(+) ")
                .Append("    AND A.DLRCD = 'XXXXX' ")
                .Append("    AND A.NEXTACTIONDVS IN('0','X','2') ")
            End With
            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dlrCD)      '販売店コード

            '検索結果返却
            Dim rtnDt As SC3010203DataSet.SC3010203TodoColorDataTable = query.GetData()

            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_End, Return:[{1}]", MethodBase.GetCurrentMethod.Name, rtnDt.Rows.Count))
            ' ======================== ログ出力 終了 ========================

            Return rtnDt
        End Using
    End Function


    ''' <summary>
    ''' 誘致先車両情報取得(受注前)
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <returns>取得結果</returns>
    ''' <remarks></remarks>
    Public Shared Function GetVclInfoActive(ByVal salesId As Decimal) As SC3010203DataSet.SC3010203VclInfoDataTable

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_Start", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        Using query As New DBSelectQuery(Of SC3010203DataSet.SC3010203VclInfoDataTable)("SC3010203_002")

            Dim sql As New StringBuilder

            'SQL文作成
            With sql
                .Append(" SELECT /* SC3010203_002 */")
                .Append("         CASE WHEN T2.VCL_ID IS NULL THEN")
                .Append("              T3.VCL_ID")
                .Append("         ELSE")
                .Append("              T2.VCL_ID")
                .Append("         END AS VCL_ID")
                .Append("   FROM TB_T_SALES T1")
                .Append("      , TB_T_REQUEST T2")
                .Append("      , TB_T_ATTRACT T3")
                .Append("  WHERE T1.REQ_ID = T2.REQ_ID(+)")
                .Append("    AND T1.ATT_ID = T3.ATT_ID(+)")
                .Append("    AND T1.SALES_ID = :SALES_ID")
            End With
            query.CommandText = sql.ToString()
            'バインド変数
            query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, salesId)

            '検索結果返却
            Dim rtnDt As SC3010203DataSet.SC3010203VclInfoDataTable = query.GetData()

            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_End, Return:[{1}]", MethodBase.GetCurrentMethod.Name, rtnDt.Rows.Count))
            ' ======================== ログ出力 終了 ========================

            Return rtnDt
        End Using
    End Function


    ''' <summary>
    ''' 誘致先顧客情報取得(受注前, 車両なし)
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <returns>取得結果</returns>
    ''' <remarks></remarks>
    Public Shared Function GetCustInfoActiveWithoutCar(ByVal salesId As Decimal) As SC3010203DataSet.SC3010203CustInfoDataTable

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_Start", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        Using query As New DBSelectQuery(Of SC3010203DataSet.SC3010203CustInfoDataTable)("SC3010203_003")

            Dim sql As New StringBuilder

            'SQL文作成
            With sql
                .Append("  SELECT /* SC3010203_003 */")
                .Append("         T3.BRN_CD AS STRCD")
                .Append("       , T3.SALES_ID AS FLLWUPBOX_SEQNO")
                .Append("       , T2.CST_TYPE AS CUSTSEGMENT")
                .Append("       , T3.CST_ID AS CRCUSTID")
                .Append("       , T1.REC_CST_VCL_TYPE AS CUSTOMERCLASS")
                .Append("    FROM TB_T_REQUEST T1")
                .Append("       , TB_M_CUSTOMER_DLR T2")
                .Append("       , TB_T_SALES T3")
                .Append("   WHERE T3.REQ_ID = T1.REQ_ID")
                .Append("      AND T3.DLR_CD = T2.DLR_CD")
                .Append("      AND T1.CST_ID = T2.CST_ID")
                .Append("      AND T3.SALES_ID = :SALES_ID")
                .Append(" UNION ALL")
                .Append("  SELECT")
                .Append("         T3.BRN_CD AS STRCD")
                .Append("       , T3.SALES_ID AS FLLWUPBOX_SEQNO")
                .Append("       , T2.CST_TYPE AS CUSTSEGMENT")
                .Append("       , T3.CST_ID AS CRCUSTID")
                .Append("       , T1.CST_VCL_TYPE AS CUSTOMERCLASS")
                .Append("    FROM TB_T_ATTRACT T1")
                .Append("       , TB_M_CUSTOMER_DLR T2")
                .Append("       , TB_T_SALES T3")
                .Append("   WHERE T3.ATT_ID = T1.ATT_ID")
                .Append("      AND T3.DLR_CD = T2.DLR_CD")
                .Append("      AND T1.CST_ID = T2.CST_ID")
                .Append("      AND T3.SALES_ID = :SALES_ID")
            End With
            query.CommandText = sql.ToString()
            'バインド変数
            query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, salesId)

            '検索結果返却
            Dim rtnDt As SC3010203DataSet.SC3010203CustInfoDataTable = query.GetData()

            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_End, Return:[{1}]", MethodBase.GetCurrentMethod.Name, rtnDt.Rows.Count))
            ' ======================== ログ出力 終了 ========================

            Return rtnDt
        End Using
    End Function


    ''' <summary>
    ''' 誘致先顧客情報取得(受注前, 車両あり)
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <returns>取得結果</returns>
    ''' <remarks></remarks>
    Public Shared Function GetCustInfoActiveWithCar(ByVal salesId As Decimal) As SC3010203DataSet.SC3010203CustInfoDataTable

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_Start", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        Using query As New DBSelectQuery(Of SC3010203DataSet.SC3010203CustInfoDataTable)("SC3010203_004")

            Dim sql As New StringBuilder

            'SQL文作成
            With sql
                .Append("   SELECT /* SC3010203_004 */")
                .Append("          T4.BRN_CD AS STRCD")
                .Append("        , T4.SALES_ID AS FLLWUPBOX_SEQNO")
                .Append("        , T3.CST_TYPE AS CUSTSEGMENT")
                .Append("        , T2.CST_ID AS CRCUSTID")
                .Append("        , T2.CST_VCL_TYPE AS CUSTOMERCLASS")
                .Append("     FROM TB_T_REQUEST T1")
                .Append("        , TB_M_CUSTOMER_VCL T2")
                .Append("        , TB_M_CUSTOMER_DLR T3")
                .Append("        , TB_T_SALES T4")
                .Append("    WHERE T2.CST_ID = T3.CST_ID")
                .Append("       AND T2.DLR_CD = T3.DLR_CD")
                .Append("       AND T4.DLR_CD = T2.DLR_CD")
                .Append("       AND T4.REQ_ID = T1.REQ_ID")
                .Append("       AND T1.VCL_ID = T2.VCL_ID")
                .Append("       AND T2.CST_VCL_TYPE = '1'")
                .Append("       AND T2.OWNER_CHG_FLG = '0'")
                .Append("       AND T4.SALES_ID = :SALES_ID")
                .Append("  UNION ALL")
                .Append("   SELECT")
                .Append("          T4.BRN_CD AS STRCD")
                .Append("        , T4.SALES_ID AS FLLWUPBOX_SEQNO")
                .Append("        , T3.CST_TYPE AS CUSTSEGMENT")
                .Append("        , T2.CST_ID AS CRCUSTID")
                .Append("        , T2.CST_VCL_TYPE AS CUSTOMERCLASS")
                .Append("     FROM TB_T_ATTRACT T1")
                .Append("        , TB_M_CUSTOMER_VCL T2")
                .Append("        , TB_M_CUSTOMER_DLR T3")
                .Append("        , TB_T_SALES T4")
                .Append("    WHERE T2.CST_ID = T3.CST_ID")
                .Append("       AND T2.DLR_CD = T3.DLR_CD")
                .Append("       AND T4.DLR_CD = T2.DLR_CD")
                .Append("       AND T4.ATT_ID = T1.ATT_ID")
                .Append("       AND T1.VCL_ID = T2.VCL_ID")
                .Append("       AND T2.CST_VCL_TYPE = '1'")
                .Append("       AND T2.OWNER_CHG_FLG = '0'")
                .Append("       AND T4.SALES_ID = :SALES_ID")
            End With
            query.CommandText = sql.ToString()
            'バインド変数
            query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, salesId)

            '検索結果返却
            Dim rtnDt As SC3010203DataSet.SC3010203CustInfoDataTable = query.GetData()

            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_End, Return:[{1}]", MethodBase.GetCurrentMethod.Name, rtnDt.Rows.Count))
            ' ======================== ログ出力 終了 ========================

            Return rtnDt
        End Using
    End Function


    ''' <summary>
    ''' 誘致先車両情報取得(受注後)
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <returns>取得結果</returns>
    ''' <remarks></remarks>
    Public Shared Function GetVclInfoHistory(ByVal salesId As Decimal) As SC3010203DataSet.SC3010203VclInfoDataTable

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_Start", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        Using query As New DBSelectQuery(Of SC3010203DataSet.SC3010203VclInfoDataTable)("SC3010203_005")

            Dim sql As New StringBuilder

            'SQL文作成
            With sql
                .Append(" SELECT /* SC3010203_005 */")
                .Append("         CASE WHEN T2.VCL_ID IS NULL THEN")
                .Append("              T3.VCL_ID")
                .Append("         ELSE")
                .Append("              T2.VCL_ID")
                .Append("         END AS VCL_ID")
                .Append("   FROM TB_H_SALES T1")
                .Append("      , TB_H_REQUEST T2")
                .Append("      , TB_H_ATTRACT T3")
                .Append("  WHERE T1.REQ_ID = T2.REQ_ID(+)")
                .Append("    AND T1.ATT_ID = T3.ATT_ID(+)")
                .Append("    AND T1.SALES_ID = :SALES_ID")
            End With
            query.CommandText = sql.ToString()
            'バインド変数
            query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, salesId)

            '検索結果返却
            Dim rtnDt As SC3010203DataSet.SC3010203VclInfoDataTable = query.GetData()

            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_End, Return:[{1}]", MethodBase.GetCurrentMethod.Name, rtnDt.Rows.Count))
            ' ======================== ログ出力 終了 ========================

            Return rtnDt
        End Using
    End Function


    ''' <summary>
    ''' 誘致先顧客情報取得(受注後, 車両なし)
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <returns>取得結果</returns>
    ''' <remarks></remarks>
    Public Shared Function GetCustInfoHistoryWithoutCar(ByVal salesId As Decimal) As SC3010203DataSet.SC3010203CustInfoDataTable

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_Start", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        Using query As New DBSelectQuery(Of SC3010203DataSet.SC3010203CustInfoDataTable)("SC3010203_006")

            Dim sql As New StringBuilder

            'SQL文作成
            With sql
                .Append(" SELECT /* SC3010203_006 */")
                .Append("        T3.BRN_CD AS STRCD")
                .Append("      , T3.SALES_ID AS FLLWUPBOX_SEQNO")
                .Append("      , T2.CST_TYPE AS CUSTSEGMENT")
                .Append("      , T3.CST_ID AS CRCUSTID")
                .Append("      , T1.REC_CST_VCL_TYPE AS CUSTOMERCLASS")
                .Append("   FROM TB_H_REQUEST T1")
                .Append("      , TB_M_CUSTOMER_DLR T2")
                .Append("      , TB_H_SALES T3")
                .Append("  WHERE T3.REQ_ID = T1.REQ_ID")
                .Append("     AND T3.DLR_CD = T2.DLR_CD")
                .Append("     AND T1.CST_ID = T2.CST_ID")
                .Append("     AND T3.SALES_ID = :SALES_ID")
                .Append(" UNION ALL")
                .Append(" SELECT")
                .Append("        T3.BRN_CD AS STRCD")
                .Append("      , T3.SALES_ID AS FLLWUPBOX_SEQNO")
                .Append("      , T2.CST_TYPE AS CUSTSEGMENT")
                .Append("      , T3.CST_ID AS CRCUSTID")
                .Append("      , T1.CST_VCL_TYPE AS CUSTOMERCLASS")
                .Append("   FROM TB_H_ATTRACT T1")
                .Append("      , TB_M_CUSTOMER_DLR T2")
                .Append("      , TB_H_SALES T3")
                .Append("  WHERE T3.ATT_ID = T1.ATT_ID")
                .Append("     AND T3.DLR_CD = T2.DLR_CD")
                .Append("     AND T1.CST_ID = T2.CST_ID")
                .Append("     AND T3.SALES_ID = :SALES_ID")
            End With
            query.CommandText = sql.ToString()
            'バインド変数
            query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, salesId)

            '検索結果返却
            Dim rtnDt As SC3010203DataSet.SC3010203CustInfoDataTable = query.GetData()

            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_End, Return:[{1}]", MethodBase.GetCurrentMethod.Name, rtnDt.Rows.Count))
            ' ======================== ログ出力 終了 ========================

            Return rtnDt
        End Using
    End Function


    ''' <summary>
    ''' 誘致先顧客情報取得(受注後, 車両あり)
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <returns>取得結果</returns>
    ''' <remarks></remarks>
    Public Shared Function GetCustInfoHistoryWithCar(ByVal salesId As Decimal) As SC3010203DataSet.SC3010203CustInfoDataTable

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_Start", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        Using query As New DBSelectQuery(Of SC3010203DataSet.SC3010203CustInfoDataTable)("SC3010203_007")

            Dim sql As New StringBuilder

            'SQL文作成
            With sql
                .Append(" SELECT /* SC3010203_007 */")
                .Append("        T4.BRN_CD AS STRCD")
                .Append("      , T4.SALES_ID AS FLLWUPBOX_SEQNO")
                .Append("      , T3.CST_TYPE AS CUSTSEGMENT")
                .Append("      , T2.CST_ID AS CRCUSTID")
                .Append("      , T2.CST_VCL_TYPE AS CUSTOMERCLASS")
                .Append("   FROM TB_H_REQUEST T1")
                .Append("      , TB_M_CUSTOMER_VCL T2")
                .Append("      , TB_M_CUSTOMER_DLR T3")
                .Append("      , TB_H_SALES T4")
                .Append("  WHERE T2.CST_ID = T3.CST_ID")
                .Append("     AND T2.DLR_CD = T3.DLR_CD")
                .Append("     AND T4.DLR_CD = T2.DLR_CD")
                .Append("     AND T4.REQ_ID = T1.REQ_ID")
                .Append("     AND T1.VCL_ID = T2.VCL_ID")
                .Append("     AND T2.CST_VCL_TYPE = '1'")
                .Append("     AND T2.OWNER_CHG_FLG = '0'")
                .Append("     AND T4.SALES_ID = :SALES_ID")
                .Append(" UNION ALL")
                .Append(" SELECT")
                .Append("        T4.BRN_CD AS STRCD")
                .Append("      , T4.SALES_ID AS FLLWUPBOX_SEQNO")
                .Append("      , T3.CST_TYPE AS CUSTSEGMENT")
                .Append("      , T2.CST_ID AS CRCUSTID")
                .Append("      , T2.CST_VCL_TYPE AS CUSTOMERCLASS")
                .Append("   FROM TB_H_ATTRACT T1")
                .Append("      , TB_M_CUSTOMER_VCL T2")
                .Append("      , TB_M_CUSTOMER_DLR T3")
                .Append("      , TB_H_SALES T4")
                .Append("  WHERE T2.CST_ID = T3.CST_ID")
                .Append("     AND T2.DLR_CD = T3.DLR_CD")
                .Append("     AND T4.DLR_CD = T2.DLR_CD")
                .Append("     AND T4.ATT_ID = T1.ATT_ID")
                .Append("     AND T1.VCL_ID = T2.VCL_ID")
                .Append("     AND T2.CST_VCL_TYPE = '1'")
                .Append("     AND T2.OWNER_CHG_FLG = '0'")
                .Append("     AND T4.SALES_ID = :SALES_ID")
            End With
            query.CommandText = sql.ToString()
            'バインド変数
            query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, salesId)

            '検索結果返却
            Dim rtnDt As SC3010203DataSet.SC3010203CustInfoDataTable = query.GetData()

            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_End, Return:[{1}]", MethodBase.GetCurrentMethod.Name, rtnDt.Rows.Count))
            ' ======================== ログ出力 終了 ========================

            Return rtnDt
        End Using
    End Function


    ''' <summary>
    ''' 初回商談日取得
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <returns>取得結果</returns>
    ''' <remarks></remarks>
    Public Function ReadFirstSalesDate(ByVal salesId As String) As SC3010203DataSet.SC3010203SalesInfoDataTable

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_Start", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        Using query As New DBSelectQuery(Of SC3010203DataSet.SC3010203SalesInfoDataTable)("SC3010203_013")

            Dim sql As New StringBuilder

            'SQL文作成
            With sql
                .Append("SELECT /* SC3010203_013 */ ")
                .Append("    A.SALES_ID, ")
                .Append("    B.RSLT_DATETIME, ")
                .Append("   '0' AS ODRDIV ")
                .Append("FROM ")
                .Append("    TB_T_SALES A, ")
                .Append("    TB_T_ACTIVITY B ")
                .Append("WHERE ")
                .Append("    A.SALES_ID IN (")
                .Append(salesId)
                .Append("） ")
                .Append("    AND A.FIRST_SALES_ACT_ID = B.ACT_ID ")
            End With

            query.CommandText = sql.ToString()

            '検索結果返却
            Dim rtnDt As SC3010203DataSet.SC3010203SalesInfoDataTable = query.GetData()

            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_End, Return:[{1}]", MethodBase.GetCurrentMethod.Name, rtnDt.Rows.Count))
            ' ======================== ログ出力 終了 ========================

            Return rtnDt
        End Using
    End Function


    ''' <summary>
    ''' 成約日取得
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <returns>取得結果</returns>
    ''' <remarks></remarks>
    Public Function ReadSuccessDate(ByVal salesId As String) As SC3010203DataSet.SC3010203SalesInfoDataTable

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_Start", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        Using query As New DBSelectQuery(Of SC3010203DataSet.SC3010203SalesInfoDataTable)("SC3010203_014")

            Dim sql As New StringBuilder

            'SQL文作成
            With sql
                .Append("SELECT /* SC3010203_014 */")
                .Append("    A.SALES_ID, ")
                .Append("    B.RSLT_DATETIME, ")
                .Append("   '1' AS ODRDIV ")
                .Append("FROM ")
                .Append("    TB_H_SALES A, ")
                .Append("    TB_H_ACTIVITY B ")
                .Append("WHERE ")
                .Append("    A.SALES_ID IN (")
                .Append(salesId)
                .Append("） ")
                .Append("    AND A.REQ_ID = B.REQ_ID ")
                .Append("    AND A.ATT_ID = 0 ")
                .Append("    AND B.ACT_STATUS = '31' ")
                .Append("UNION ALL ")
                .Append("SELECT ")
                .Append("    A.SALES_ID, ")
                .Append("    B.RSLT_DATETIME, ")
                .Append("   '1' AS ODRDIV ")
                .Append("FROM ")
                .Append("    TB_H_SALES A, ")
                .Append("    TB_H_ACTIVITY B ")
                .Append("WHERE ")
                .Append("    A.SALES_ID IN (")
                .Append(salesId)
                .Append("） ")
                .Append("    AND A.ATT_ID = B.ATT_ID ")
                .Append("    AND A.REQ_ID = 0 ")
                .Append("    AND B.ACT_STATUS = '31' ")
            End With

            query.CommandText = sql.ToString()

            '検索結果返却
            Dim rtnDt As SC3010203DataSet.SC3010203SalesInfoDataTable = query.GetData()

            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_End, Return:[{1}]", MethodBase.GetCurrentMethod.Name, rtnDt.Rows.Count))
            ' ======================== ログ出力 終了 ========================

            Return rtnDt
        End Using
    End Function


    ''' <summary>
    ''' 納車日取得
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <returns>取得結果</returns>
    ''' <remarks></remarks>
    Public Function ReadDeliveryDate(ByVal salesId As String, ByVal afterOrderId As String) As SC3010203DataSet.SC3010203SalesInfoDataTable

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_Start", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        Using query As New DBSelectQuery(Of SC3010203DataSet.SC3010203SalesInfoDataTable)("SC3010203_015")

            Dim sql As New StringBuilder

            'SQL文作成
            With sql
                .Append("SELECT /* SC3010203_015 */ ")
                .Append("    A.SALES_ID, ")
                .Append("    B.RSLT_END_DATEORTIME AS RSLT_DATETIME, ")
                .Append("   '2' AS ODRDIV ")
                .Append("FROM ")
                .Append("    TB_T_AFTER_ODR A, ")
                .Append("    TB_T_AFTER_ODR_ACT B ")
                .Append("WHERE ")
                .Append("    A.SALES_ID IN (")
                .Append(salesId)
                .Append("） ")
                .Append("    AND A.AFTER_ODR_ID = B.AFTER_ODR_ID ")
                .Append("    AND B.AFTER_ODR_ACT_CD = :AFTERORDERID ")
                .Append(" UNION ALL ")
                .Append("SELECT /* SC3010203_015 */ ")
                .Append("    A.SALES_ID, ")
                .Append("    B.RSLT_END_DATEORTIME AS RSLT_DATETIME, ")
                .Append("   '2' AS ODRDIV ")
                .Append("FROM ")
                .Append("    TB_H_AFTER_ODR A, ")
                .Append("    TB_H_AFTER_ODR_ACT B ")
                .Append("WHERE ")
                .Append("    A.SALES_ID IN (")
                .Append(salesId)
                .Append("） ")
                .Append("    AND A.AFTER_ODR_ID = B.AFTER_ODR_ID ")
                .Append("    AND B.AFTER_ODR_ACT_CD = :AFTERORDERID ")

            End With

            query.CommandText = sql.ToString()

            'バインド変数
            query.AddParameterWithTypeValue("AFTERORDERID", OracleDbType.Varchar2, afterOrderId)

            '検索結果返却
            Dim rtnDt As SC3010203DataSet.SC3010203SalesInfoDataTable = query.GetData()

            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_End, Return:[{1}]", MethodBase.GetCurrentMethod.Name, rtnDt.Rows.Count))
            ' ======================== ログ出力 終了 ========================

            Return rtnDt
        End Using
    End Function


#Region "来店実績"


    ''' <summary>
    ''' 来店実績一覧取得
    ''' </summary>
    ''' <param name="mode">処理モード(1:当日、2:過去日)</param>
    ''' <param name="startDatetime">処理モードが過去日の場合の対象日</param>
    ''' <param name="endDatetime">処理モードが過去日の場合の対象日</param>
    ''' <returns>取得結果</returns>
    ''' <remarks></remarks>
    Public Function SelectVisitActualList(ByVal mode As String, ByVal startDatetime As Date, ByVal endDatetime As Date, ByVal visitActualCnt As String) As SC3010203DataSet.SC3010203VisitActualDataTable

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_Start", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        Dim sql As New StringBuilder
        '2015/03/30 TCS 山口 TMT M009 START
        '2014/11/10 TCS 河原 TMT 切替BTS-201 START
        With sql
            .Append("SELECT /* SC3010203_008 */ ")
            .Append("   DLRCD, ")
            .Append("   STRCD, ")
            .Append("   BRANCH_PLAN, ")
            .Append("   FLLWUPBOX_SEQNO, ")
            .Append("   ACTUALACCOUNT, ")
            .Append("   TEMP_STAFFNAME, ")
            .Append("   TEMP_STAFF_OPERATIONCODE, ")
            .Append("   TEMP_STAFF_OPERATIONCODE_ICON, ")
            .Append("   STARTTIME, ")
            .Append("   ENDTIME, ")
            .Append("   CUSTSEGMENT, ")
            .Append("   CUSTOMERCLASS, ")
            .Append("   CRCUSTID, ")
            .Append("   REGISTFLG, ")
            .Append("   ACCOUNT_PLAN, ")
            .Append("   CST_SERVICE_TYPE ")
            .Append("FROM ( ")
            .Append(SelectVisitActualListBeforeSql(mode))
            If String.Equals(mode, "1") Then
                '処理モード=1(当日)
                .Append("UNION ALL ")
                .Append(SelectVisitActualListNowSql())
            End If
            .Append("ORDER BY ")
            .Append("    REGISTFLG, STARTTIME ")
            .Append(") ")
            .Append("WHERE ROWNUM <= :MAX_CNT ")
        End With
        '2014/11/10 TCS 河原 TMT 切替BTS-201 END
        '2015/03/30 TCS 山口 TMT M009 END

        Using query As New DBSelectQuery(Of SC3010203DataSet.SC3010203VisitActualDataTable)("SC3010203_008")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, Me.DlrCd)
            query.AddParameterWithTypeValue("BRANCH_PLAN", OracleDbType.Char, Me.StrCd)
            query.AddParameterWithTypeValue("ACCOUNT_PLAN", OracleDbType.Char, Me.UserId)
            If String.Equals(mode, "1") Then
                query.AddParameterWithTypeValue("NOW", OracleDbType.Date, DateTimeFunc.Now(Me.DlrCd).Date)
            ElseIf String.Equals(mode, "2") Then
                query.AddParameterWithTypeValue("STARTDATETIME", OracleDbType.Date, startDatetime)
                query.AddParameterWithTypeValue("ENDDATETIME", OracleDbType.Date, endDatetime)
            End If
            query.AddParameterWithTypeValue("MAX_CNT", OracleDbType.Char, visitActualCnt)
            Return query.GetData()
        End Using
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0} End", GetCurrentMethod().Name))
    End Function

    '2015/03/30 TCS 山口 TMT M009 START
    ''' <summary>
    ''' 来店実績一覧取得SQL 前日以前の来店実績
    ''' </summary>
    ''' <param name="mode">処理モード(1:当日、2:過去日)</param>
    ''' <returns>SQL文字列</returns>
    ''' <remarks></remarks>
    Private Shared Function SelectVisitActualListBeforeSql(ByVal mode As String) As String

        Dim sql As New StringBuilder
        'ログ出力 Start ***************************************************************************
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_Start", MethodBase.GetCurrentMethod.Name))
        'ログ出力 End *****************************************************************************
        With sql
            '2020/06/17 TS 山口 TR-SLT-TKM-20200616-001 START
            .Append("SELECT /* 前日以前の来店実績 商談 */ /*+ INDEX(HACT TB_H_ACTIVITY_IX1) */ ")
            '2020/06/17 TS 山口 TR-SLT-TKM-20200616-001 END
            .Append("    DISTINCT ")
            .Append("    SALES.DLRCD, ")
            .Append("    SALES.STRCD, ")
            .Append("    SALES.BRANCH_PLAN, ")
            .Append("    SALES.FLLWUPBOX_SEQNO, ")
            .Append("    SALES.ACTUALACCOUNT, ")
            .Append("    USERS.USERNAME TEMP_STAFFNAME, ")
            .Append("    USERS.OPERATIONCODE TEMP_STAFF_OPERATIONCODE, ")
            .Append("    OPE.ICON_IMGFILE TEMP_STAFF_OPERATIONCODE_ICON, ")
            .Append("    SALES.STARTTIME, ")
            .Append("    SALES.ENDTIME, ")
            .Append("    SALES.CUSTSEGMENT, ")
            .Append("    SALES.CUSTOMERCLASS, ")
            .Append("    SALES.CRCUSTID, ")
            .Append("    NVL2(HACT.ACT_STATUS,'1',SALES.REGISTFLG) AS REGISTFLG, ")
            .Append("    SALES.ACCOUNT_PLAN, ")
            .Append("    SALES.CST_SERVICE_TYPE, ")
            .Append("    SALES.SALES_SEQNO ")
            .Append("FROM ")
            .Append("    TBL_FLLWUPBOX_SALES SALES, ")
            .Append("    TBL_USERS USERS, ")
            .Append("    TBL_OPERATIONTYPE OPE, ")
            .Append("    TB_H_SALES HSALES, ")
            .Append("    TB_H_ACTIVITY HACT ")
            .Append("WHERE ")
            .Append("        SALES.ACTUALACCOUNT = USERS.ACCOUNT(+) ")
            .Append("    AND USERS.OPERATIONCODE = OPE.OPERATIONCODE ")
            .Append("    AND USERS.DLRCD = OPE.DLRCD ")
            .Append("    AND OPE.STRCD = '000' ")
            .Append("    AND SALES.DLRCD = :DLRCD ")
            .Append("    AND SALES.BRANCH_PLAN = :BRANCH_PLAN ")
            .Append("    AND SALES.ACCOUNT_PLAN = :ACCOUNT_PLAN ")
            .Append("    AND SALES.CST_SERVICE_TYPE in ('1',' ') ")
            .Append("    AND HSALES.SALES_ID(+) = SALES.FLLWUPBOX_SEQNO ")
            .Append("    AND HACT.REQ_ID(+) = HSALES.REQ_ID ")
            .Append("    AND HACT.ATT_ID(+) = HSALES.ATT_ID ")
            .Append("    AND HACT.ACT_STATUS(+) in ('31','32') ")
            If String.Equals(mode, "1") Then
                .Append("    AND NVL2(HACT.ACT_STATUS,'1',SALES.REGISTFLG) = '0' ")
                .Append("    AND SALES.ENDTIME < :NOW ")
                .Append("    AND SALES.ENDTIME IS NOT NULL ")
            ElseIf String.Equals(mode, "2") Then
                .Append("    AND NVL2(HACT.ACT_STATUS,'1',SALES.REGISTFLG) = '1' ")
                .Append("    AND SALES.ENDTIME >= :STARTDATETIME  ")
                .Append("    AND SALES.ENDTIME <= :ENDDATETIME  ")
                .Append("    AND SALES.ENDTIME IS NOT NULL ")
            End If
            .Append("UNION ALL ")
            .Append("SELECT /* 前日以前の来店実績 納車作業 */ ")
            .Append("    SALES.DLRCD, ")
            .Append("    SALES.STRCD, ")
            .Append("    SALES.BRANCH_PLAN, ")
            .Append("    SALES.FLLWUPBOX_SEQNO, ")
            .Append("    SALES.ACTUALACCOUNT, ")
            .Append("    USERS.USERNAME TEMP_STAFFNAME, ")
            .Append("    USERS.OPERATIONCODE TEMP_STAFF_OPERATIONCODE, ")
            .Append("    OPE.ICON_IMGFILE TEMP_STAFF_OPERATIONCODE_ICON, ")
            .Append("    SALES.STARTTIME, ")
            .Append("    SALES.ENDTIME, ")
            .Append("    SALES.CUSTSEGMENT, ")
            .Append("    SALES.CUSTOMERCLASS, ")
            .Append("    SALES.CRCUSTID, ")
            .Append("    NVL2(DECODE(BKG.CANCEL_FLG, '1', '', EST.CONTRACTNO),NVL2(HAODR.SALES_ID, '1', SALES.REGISTFLG),'1') AS REGISTFLG, ")
            .Append("    SALES.ACCOUNT_PLAN, ")
            .Append("    SALES.CST_SERVICE_TYPE, ")
            .Append("    SALES.SALES_SEQNO ")
            .Append("FROM ")
            .Append("    TBL_FLLWUPBOX_SALES SALES, ")
            .Append("    TBL_USERS USERS, ")
            .Append("    TBL_OPERATIONTYPE OPE, ")
            .Append("    TB_H_AFTER_ODR HAODR, ")
            .Append("    TBL_ESTIMATEINFO EST, ")
            .Append("    TB_T_SALESBOOKING BKG ")
            .Append("WHERE ")
            .Append("        SALES.ACTUALACCOUNT = USERS.ACCOUNT(+) ")
            .Append("    AND USERS.OPERATIONCODE = OPE.OPERATIONCODE ")
            .Append("    AND USERS.DLRCD = OPE.DLRCD ")
            .Append("    AND OPE.STRCD = '000' ")
            .Append("    AND SALES.DLRCD = :DLRCD ")
            .Append("    AND SALES.BRANCH_PLAN = :BRANCH_PLAN ")
            .Append("    AND SALES.ACCOUNT_PLAN = :ACCOUNT_PLAN ")
            .Append("    AND SALES.CST_SERVICE_TYPE = '2' ")
            .Append("    AND HAODR.SALES_ID(+) = SALES.FLLWUPBOX_SEQNO ")
            .Append("    AND EST.FLLWUPBOX_SEQNO(+) = SALES.FLLWUPBOX_SEQNO ")
            .Append("    AND BKG.DLR_CD(+) = EST.DLRCD ")
            .Append("    AND BKG.SALESBKG_NUM(+) = RTRIM(EST.CONTRACTNO) ")
            .Append("    AND EST.DELFLG(+) = '0' ")
            .Append("    AND EST.CONTRACTFLG(+) = '1' ")
            If String.Equals(mode, "1") Then
                .Append("    AND NVL2(DECODE(BKG.CANCEL_FLG, '1', '', EST.CONTRACTNO),NVL2(HAODR.SALES_ID, '1', SALES.REGISTFLG),'1') = '0' ")
                .Append("    AND SALES.ENDTIME < :NOW ")
                .Append("    AND SALES.ENDTIME IS NOT NULL ")
            ElseIf String.Equals(mode, "2") Then
                .Append("    AND NVL2(DECODE(BKG.CANCEL_FLG, '1', '', EST.CONTRACTNO),NVL2(HAODR.SALES_ID, '1', SALES.REGISTFLG),'1') = '1' ")
                .Append("    AND SALES.ENDTIME >= :STARTDATETIME  ")
                .Append("    AND SALES.ENDTIME <= :ENDDATETIME  ")
                .Append("    AND SALES.ENDTIME IS NOT NULL ")
            End If
        End With
        'ログ出力 Start ***************************************************************************
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_End", MethodBase.GetCurrentMethod.Name))
        'ログ出力 End *****************************************************************************

        Return sql.ToString()
    End Function

    ''' <summary>
    ''' 来店実績一覧取得SQL 当日の来店実績
    ''' </summary>
    ''' <returns>SQL文字列</returns>
    ''' <remarks></remarks>
    Private Shared Function SelectVisitActualListNowSql() As String

        Dim sql As New StringBuilder
        'ログ出力 Start ***************************************************************************
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_Start", MethodBase.GetCurrentMethod.Name))
        'ログ出力 End *****************************************************************************
        With sql
            '2020/06/17 TS 山口 TR-SLT-TKM-20200616-001 START
            .Append("SELECT /* 当日の来店実績 商談 */ /*+ INDEX(HACT TB_H_ACTIVITY_IX1) */ ")
            '2020/06/17 TS 山口 TR-SLT-TKM-20200616-001 END
            .Append("    DISTINCT ")
            .Append("    SALES.DLRCD, ")
            .Append("    SALES.STRCD, ")
            .Append("    SALES.BRANCH_PLAN, ")
            .Append("    SALES.FLLWUPBOX_SEQNO, ")
            .Append("    SALES.ACTUALACCOUNT, ")
            .Append("    USERS.USERNAME, ")
            .Append("    USERS.OPERATIONCODE, ")
            .Append("    OPE.ICON_IMGFILE, ")
            .Append("    SALES.STARTTIME, ")
            .Append("    SALES.ENDTIME, ")
            .Append("    SALES.CUSTSEGMENT, ")
            .Append("    SALES.CUSTOMERCLASS, ")
            .Append("    SALES.CRCUSTID, ")
            .Append("    NVL2(HACT.ACT_STATUS,'1',SALES.REGISTFLG) AS REGISTFLG, ")
            .Append("    SALES.ACCOUNT_PLAN, ")
            .Append("    SALES.CST_SERVICE_TYPE, ")
            .Append("    SALES.SALES_SEQNO ")
            .Append("FROM ")
            .Append("    TBL_FLLWUPBOX_SALES SALES, ")
            .Append("    TBL_USERS USERS, ")
            .Append("    TBL_OPERATIONTYPE OPE, ")
            .Append("    TB_H_SALES HSALES, ")
            .Append("    TB_H_ACTIVITY HACT ")
            .Append("WHERE ")
            .Append("        SALES.ACTUALACCOUNT = USERS.ACCOUNT(+) ")
            .Append("    AND USERS.OPERATIONCODE = OPE.OPERATIONCODE ")
            .Append("    AND USERS.DLRCD = OPE.DLRCD ")
            .Append("    AND OPE.STRCD = '000' ")
            .Append("    AND SALES.DLRCD = :DLRCD ")
            .Append("    AND SALES.BRANCH_PLAN = :BRANCH_PLAN ")
            .Append("    AND SALES.ACCOUNT_PLAN = :ACCOUNT_PLAN ")
            .Append("    AND SALES.CST_SERVICE_TYPE in ('1',' ') ")
            .Append("    AND HSALES.SALES_ID(+) = SALES.FLLWUPBOX_SEQNO ")
            .Append("    AND HACT.REQ_ID(+) = HSALES.REQ_ID ")
            .Append("    AND HACT.ATT_ID(+) = HSALES.ATT_ID ")
            .Append("    AND HACT.ACT_STATUS(+) in ('31','32') ")
            .Append("    AND SALES.ENDTIME >= :NOW ")
            .Append("UNION ALL ")
            .Append("SELECT /* 当日の来店実績 納車作業 */ ")
            .Append("    SALES.DLRCD, ")
            .Append("    SALES.STRCD, ")
            .Append("    SALES.BRANCH_PLAN, ")
            .Append("    SALES.FLLWUPBOX_SEQNO, ")
            .Append("    SALES.ACTUALACCOUNT, ")
            .Append("    USERS.USERNAME, ")
            .Append("    USERS.OPERATIONCODE, ")
            .Append("    OPE.ICON_IMGFILE, ")
            .Append("    SALES.STARTTIME, ")
            .Append("    SALES.ENDTIME, ")
            .Append("    SALES.CUSTSEGMENT, ")
            .Append("    SALES.CUSTOMERCLASS, ")
            .Append("    SALES.CRCUSTID, ")
            .Append("    NVL2(DECODE(BKG.CANCEL_FLG, '1', '', EST.CONTRACTNO),NVL2(HAODR.SALES_ID, '1', SALES.REGISTFLG),'1') AS REGISTFLG, ")
            .Append("    SALES.ACCOUNT_PLAN, ")
            .Append("    SALES.CST_SERVICE_TYPE, ")
            .Append("    SALES.SALES_SEQNO ")
            .Append("FROM ")
            .Append("    TBL_FLLWUPBOX_SALES SALES, ")
            .Append("    TBL_USERS USERS, ")
            .Append("    TBL_OPERATIONTYPE OPE, ")
            .Append("    TB_H_AFTER_ODR HAODR, ")
            .Append("    TBL_ESTIMATEINFO EST, ")
            .Append("    TB_T_SALESBOOKING BKG ")
            .Append("WHERE ")
            .Append("        SALES.ACTUALACCOUNT = USERS.ACCOUNT(+) ")
            .Append("    AND USERS.OPERATIONCODE = OPE.OPERATIONCODE ")
            .Append("    AND USERS.DLRCD = OPE.DLRCD ")
            .Append("    AND OPE.STRCD = '000' ")
            .Append("    AND SALES.DLRCD = :DLRCD ")
            .Append("    AND SALES.BRANCH_PLAN = :BRANCH_PLAN ")
            .Append("    AND SALES.ACCOUNT_PLAN = :ACCOUNT_PLAN ")
            .Append("    AND SALES.CST_SERVICE_TYPE = '2' ")
            .Append("    AND HAODR.SALES_ID(+) = SALES.FLLWUPBOX_SEQNO ")
            .Append("    AND EST.FLLWUPBOX_SEQNO(+) = SALES.FLLWUPBOX_SEQNO ")
            .Append("    AND BKG.DLR_CD(+) = EST.DLRCD ")
            .Append("    AND BKG.SALESBKG_NUM(+) = RTRIM(EST.CONTRACTNO) ")
            .Append("    AND EST.DELFLG(+) = '0' ")
            .Append("    AND EST.CONTRACTFLG(+) = '1' ")
            .Append("    AND SALES.ENDTIME >= :NOW ")
        End With
        'ログ出力 Start ***************************************************************************
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_End", MethodBase.GetCurrentMethod.Name))
        'ログ出力 End *****************************************************************************

        Return sql.ToString()
    End Function
    '2015/03/30 TCS 山口 TMT M009 END

    ''' <summary>
    ''' 顧客車両情報(受注前)取得
    ''' </summary>
    ''' <returns>取得結果</returns>
    ''' <remarks></remarks>
    Public Function SelectCustomerVehicleInfo(ByVal salesid As Decimal) As SC3010203DataSet.SC3010203CustomerNameDataTable
        Dim sql As New StringBuilder

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0} Start Params:salesid=[{1}]", GetCurrentMethod().Name, salesid))

        With sql
            .AppendLine(" SELECT /* SC3010203_009 */ ")
            .AppendLine("        T1.DLR_CD ")
            .AppendLine("      , T2.VCL_ID ")
            .AppendLine("      , T2.CST_ID ")
            .AppendLine("      , T2.REC_CST_VCL_TYPE AS CST_VCL_TYPE ")
            .AppendLine("   FROM TB_T_SALES T1 ")
            .AppendLine("      , TB_T_REQUEST T2 ")
            .AppendLine("  WHERE T1.REQ_ID = T2.REQ_ID ")
            .AppendLine("    AND T1.SALES_ID = :SALES_ID ")
            .AppendLine("  UNION ALL ")
            .AppendLine(" SELECT ")
            .AppendLine("        T3.DLR_CD ")
            .AppendLine("      , T4.VCL_ID ")
            .AppendLine("      , T4.CST_ID ")
            .AppendLine("      , T4.CST_VCL_TYPE ")
            .AppendLine("   FROM TB_T_SALES T3 ")
            .AppendLine("      , TB_T_ATTRACT T4 ")
            .AppendLine("  WHERE T3.ATT_ID = T4.ATT_ID ")
            .AppendLine("    AND T3.SALES_ID = :SALES_ID ")
        End With

        Using query As New DBSelectQuery(Of SC3010203DataSet.SC3010203CustomerNameDataTable)("SC3010203_009")
            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, salesid)

            Return query.GetData()
        End Using
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0} End", GetCurrentMethod().Name))
    End Function


    ''' <summary>
    ''' 顧客車両情報(受注後)取得
    ''' </summary>
    ''' <returns>取得結果</returns>
    ''' <remarks></remarks>
    Public Function SelectCustomerVehicleInfoHistory(ByVal salesid As Decimal) As SC3010203DataSet.SC3010203CustomerNameDataTable
        Dim sql As New StringBuilder

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0} Start Params:salesid=[{1}]", GetCurrentMethod().Name, salesid))

        With sql
            .AppendLine(" SELECT /* SC3010203_010 */ ")
            .AppendLine("        T1.DLR_CD ")
            .AppendLine("      , T2.VCL_ID ")
            .AppendLine("      , T2.CST_ID ")
            .AppendLine("      , T2.REC_CST_VCL_TYPE AS CST_VCL_TYPE ")
            .AppendLine("   FROM TB_H_SALES T1 ")
            .AppendLine("      , TB_H_REQUEST T2 ")
            .AppendLine("  WHERE T1.REQ_ID = T2.REQ_ID ")
            .AppendLine("    AND T1.SALES_ID = :SALES_ID ")
            .AppendLine("  UNION ALL ")
            .AppendLine(" SELECT ")
            .AppendLine("        T3.DLR_CD ")
            .AppendLine("      , T4.VCL_ID ")
            .AppendLine("      , T4.CST_ID ")
            .AppendLine("      , T4.CST_VCL_TYPE ")
            .AppendLine("   FROM TB_H_SALES T3 ")
            .AppendLine("      , TB_H_ATTRACT T4 ")
            .AppendLine("  WHERE T3.ATT_ID = T4.ATT_ID ")
            .AppendLine("    AND T3.SALES_ID = :SALES_ID ")
        End With

        Using query As New DBSelectQuery(Of SC3010203DataSet.SC3010203CustomerNameDataTable)("SC3010203_010")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, salesid)

            Return query.GetData()
        End Using
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0} End", GetCurrentMethod().Name))
    End Function


    ''' <summary>
    ''' 敬称付き顧客名称(所有者)取得
    ''' </summary>
    ''' <returns>取得結果</returns>
    ''' <remarks></remarks>
    Public Function SelectCustomerNameWithNameTitleOwner(ByVal custid As Decimal, ByVal dlrcd As String) As SC3010203DataSet.SC3010203CustomerNameDataTable

        Dim sql As New StringBuilder

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0} Start Params:custid=[{1}]", GetCurrentMethod().Name, custid))

        With sql
            .AppendLine(" SELECT /* SC3010203_011 */ ")
            .AppendLine("        T1.CST_NAME AS NAME ")
            .AppendLine("      , T1.NAMETITLE_NAME AS NAMETITLE ")
            .AppendLine("      , T2.CST_TYPE ")
            .AppendLine("   FROM TB_M_CUSTOMER T1 ")
            .AppendLine("      , TB_M_CUSTOMER_DLR T2 ")
            .AppendLine("  WHERE T1.CST_ID = T2.CST_ID ")
            .AppendLine("    AND T1.CST_ID = :CST_ID ")
            .AppendLine("    AND T2.DLR_CD = :DLR_CD ")
        End With

        Using query As New DBSelectQuery(Of SC3010203DataSet.SC3010203CustomerNameDataTable)("SC3010203_011")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("CST_ID", OracleDbType.Decimal, custid)
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dlrcd)

            Return query.GetData()
        End Using
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0} End", GetCurrentMethod().Name))
    End Function


    ''' <summary>
    ''' 敬称付き顧客名称(所有者以外)取得
    ''' </summary>
    ''' <returns>取得結果</returns>
    ''' <remarks></remarks>
    Public Function SelectCustomerNameWithNameTitleNotOwner(ByVal dlrcd As String, ByVal vclid As Decimal) As SC3010203DataSet.SC3010203CustomerNameDataTable

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_Start", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        Dim sql As New StringBuilder

        With sql
            .AppendLine(" SELECT /* SC3010203_012 */ ")
            .AppendLine("          T1.CST_NAME AS NAME ")
            .AppendLine("        , T1.NAMETITLE_NAME AS NAMETITLE ")
            .AppendLine("        , T2.CST_ID ")
            .AppendLine("        , T3.CST_TYPE ")
            .AppendLine("     FROM TB_M_CUSTOMER T1 ")
            .AppendLine("        , TB_M_CUSTOMER_VCL T2 ")
            .AppendLine("        , TB_M_CUSTOMER_DLR T3 ")
            .AppendLine("    WHERE T1.CST_ID = T2.CST_ID ")
            .AppendLine("      AND T2.DLR_CD = T3.DLR_CD ")
            .AppendLine("      AND T2.CST_ID = T3.CST_ID ")
            .AppendLine("      AND T2.DLR_CD = :DLR_CD ")
            .AppendLine("      AND T2.VCL_ID = :VCL_ID ")
            .AppendLine("      AND T2.CST_VCL_TYPE = :FLAG_ON ")
            .AppendLine("      AND T2.OWNER_CHG_FLG = :FLAG_OFF ")
        End With

        Using query As New DBSelectQuery(Of SC3010203DataSet.SC3010203CustomerNameDataTable)("SC3010203_012")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dlrcd)
            query.AddParameterWithTypeValue("VCL_ID", OracleDbType.Decimal, vclid)
            query.AddParameterWithTypeValue("FLAG_ON", OracleDbType.Char, C_FLAG_ON)
            query.AddParameterWithTypeValue("FLAG_OFF", OracleDbType.Char, C_FLAG_OFF)
            Return query.GetData()
        End Using
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0} End", GetCurrentMethod().Name))
    End Function


#End Region


End Class



