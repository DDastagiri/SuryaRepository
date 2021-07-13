'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3010204TableAdapter.vb
'─────────────────────────────────────
'機能： SCメイン(KPI)
'補足： 
'作成：  
'更新： 2014/02/19 TCS 受注後フォロー機能開発
'削除： 2020/01/06 TS  重松 [TMTレスポンススロー] SLT基盤への横展
'─────────────────────────────────────
Imports System.Text
Imports System.Globalization
Imports System.Reflection
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Web

''' <summary>
''' SCメイン(KPI)のデータアクセスクラスです。
''' </summary>
''' <remarks></remarks>
Public NotInheritable Class SC3010204TableAdapter

    ''' <summary>
    ''' コンストラクタ
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub New()

    End Sub

#Region "KPI活用指標（名称）取得"
    ''' <summary>
    ''' KPI活用指標（名称）取得
    ''' </summary>
    ''' <returns>KPI活用指標（名称）</returns>
    ''' <remarks>活用指標KPI項目マスタより表示対象の指標名称を取得する。</remarks>
    Public Shared Function SelectProcessKpiItem() As SC3010204DataSet.SalesKpiItemDataTable
        Using query As New DBSelectQuery(Of SC3010204DataSet.SalesKpiItemDataTable)("SC3010204_002")
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SelectProcessKpiItem_Start")
            Dim sql As New StringBuilder

            sql.AppendLine("SELECT /* SC3010204_002 */")
            sql.AppendLine("       SALES_KPI_ITEM_CD")
            sql.AppendLine("     , CASE WHEN T2.WORD_VAL = ' ' THEN T2.WORD_VAL_ENG")
            sql.AppendLine("            WHEN T2.WORD_VAL <> ' ' THEN T2.WORD_VAL")
            sql.AppendLine("            WHEN T2.WORD_VAL IS NULL THEN NULL")
            sql.AppendLine("       END AS SALES_KPI_ITEM")
            sql.AppendLine("  FROM TB_M_SALES_KPI_ITEM T1")
            sql.AppendLine("     , TB_M_WORD T2")
            sql.AppendLine(" WHERE T1.SALES_KPI_ITEM = T2.WORD_CD(+)")
            sql.AppendLine("   AND T1.INUSE_FLG = '1'")
            sql.AppendLine(" ORDER BY T1.SORT_ORDER")

            query.CommandText = sql.ToString

            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SelectProcessKpiItem_End")

            Return query.GetData()
        End Using
    End Function
#End Region

#Region "KPI活用指標（日別）"
    ''' <summary>
    ''' KPI活用指標（日別）取得
    ''' </summary>
    ''' <param name="fromDate">検索対象開始日</param>
    ''' <param name="toDate">検索対象終了日</param>
    ''' <param name="stfcd">検索対象スタッフ</param>
    ''' <param name="mngFlg">1：マネージャー、アシスタント　0：以外</param>
    ''' <param name="orgnzid">検索対象組織　（マネージャーフラグ＝‘１’の場合のみデータ有）</param>
    ''' <returns>KPI活用指標（日別）</returns>
    ''' <remarks>
    ''' スタッフの場合、本人の日別のKPI活用指標の集計値を取得。
    ''' マネージャー、アシスタントの場合、配下スタッフの当月1日～前日までの商談数の月間合計数を取得。
    ''' </remarks>
    Public Shared Function SelectProcessKpiValue(fromDate As Date, toDate As Date, stfcd As String, mngFlg As String, orgnzid As String) As SC3010204DataSet.SalesKpiSummaryDataTable

        Using query As New DBSelectQuery(Of SC3010204DataSet.SalesKpiSummaryDataTable)("SC3010204_003")

            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SelectProcessKpiValue_Start")

            Dim sql As New StringBuilder

            sql.AppendLine("SELECT /* SC3010204_003 */")
            sql.AppendLine("       B.SALES_KPI_ITEM_CD")
            sql.AppendLine("     , B.TGT_DATE")
            sql.AppendLine("     , B.SUM_VAL")
            sql.AppendLine("  FROM TB_M_SALES_KPI_ITEM A")
            sql.AppendLine("     , TB_T_SALES_KPI B")
            sql.AppendLine(" WHERE A.SALES_KPI_ITEM_CD = B.SALES_KPI_ITEM_CD")
            sql.AppendLine("   AND A.INUSE_FLG = '1'")
            sql.AppendLine("   AND B.TGT_DATE BETWEEN :FROM_DATE AND :TO_DATE")
            sql.AppendLine("   AND B.TGT_DLR_CD = :DLR_CD")
            sql.AppendLine("   AND B.TGT_BRN_CD = :BRN_CD")
            If "1".Equals(mngFlg) Then
                'マネージャ・アシスタントの場合
                sql.AppendLine("   AND B.TGT_ORGNZ_ID IN (" & orgnzid & ")")
            Else
                '担当の場合
                sql.AppendLine("   AND B.TGT_STF_CD = :STF_CD")
                query.AddParameterWithTypeValue("STF_CD", OracleDbType.NVarchar2, stfcd)   'スタッフコード
            End If

            query.AddParameterWithTypeValue("FROM_DATE", OracleDbType.Date, fromDate)   '検索対象開始日
            query.AddParameterWithTypeValue("TO_DATE", OracleDbType.Date, toDate)       '検索対象終了日
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, StaffContext.Current.DlrCD)   '販売店コード
            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, StaffContext.Current.BrnCD)   '店舗コード

            query.CommandText = sql.ToString

            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SelectProcessKpiValue_End")

            Return query.GetData()
        End Using

    End Function

#End Region

#Region "商談数の月間合計数"
    ''' <summary>
    ''' 商談数の月間合計数取得
    ''' </summary>
    ''' <param name="fromDate">検索対象開始日</param>
    ''' <param name="toDate">検索対象終了日</param>
    ''' <param name="stfcd">検索対象スタッフ</param>
    ''' <param name="mngFlg">1：マネージャー、アシスタント　0：以外</param>
    ''' <param name="orgnzid">検索対象組織　（マネージャーフラグ＝‘１’の場合のみデータ有）</param>
    ''' <returns>商談数の月間合計数</returns>
    ''' <remarks>
    ''' スタッフの場合、本人の当月1日～前日まで（※）の商談数の月間合計数を取得。
    ''' マネージャー、アシスタントの場合、配下スタッフの当月1日～前日まで（※）の商談数の月間合計数を取得。
    ''' </remarks>
    Public Shared Function SelectNegotiationNumber(fromDate As Date, toDate As Date, stfcd As String, mngFlg As String, orgnzid As String) As SC3010204DataSet.SalesKpiSummaryMonthlyDataTable
        Using query As New DBSelectQuery(Of SC3010204DataSet.SalesKpiSummaryMonthlyDataTable)("SC3010204_004")

            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SelectNegotiationNumber_Start")

            Dim sql As New StringBuilder

            sql.AppendLine("SELECT /* SC3010204_004 */")
            sql.AppendLine("       SUM(SUM_VAL) SUM_VAL")
            sql.AppendLine("  FROM TB_T_SALES_KPI")
            sql.AppendLine(" WHERE SALES_KPI_ITEM_CD = '001'")
            sql.AppendLine("   AND TGT_DATE BETWEEN :FROM_DATE AND :TO_DATE")
            sql.AppendLine("   AND TGT_DLR_CD = :DLR_CD")
            sql.AppendLine("   AND TGT_BRN_CD = :BRN_CD")
            If "1".Equals(mngFlg) Then
                'マネージャ・アシスタントの場合
                sql.AppendLine("   AND TGT_ORGNZ_ID IN (" & orgnzid & ")")
            Else
                '担当の場合
                sql.AppendLine("   AND TGT_STF_CD = :STF_CD")
                query.AddParameterWithTypeValue("STF_CD", OracleDbType.NVarchar2, stfcd)    'スタッフコード
            End If

            query.AddParameterWithTypeValue("FROM_DATE", OracleDbType.Date, fromDate)   '検索対象開始日
            query.AddParameterWithTypeValue("TO_DATE", OracleDbType.Date, toDate)       '検索対象終了日
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, StaffContext.Current.DlrCD)   '販売店コード
            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, StaffContext.Current.BrnCD)   '店舗コード

            query.CommandText = sql.ToString

            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SelectNegotiationNumber_End")

            Return query.GetData()
        End Using

    End Function
#End Region

#Region "TCV活用数の月間合計数"
    ''' <summary>
    ''' TCV活用数の月間合計数取得
    ''' </summary>
    ''' <param name="fromDate">検索対象開始日</param>
    ''' <param name="toDate">検索対象終了日</param>
    ''' <param name="stfcd">検索対象スタッフ</param>
    ''' <param name="mngFlg">1：マネージャー、アシスタント　0：以外</param>
    ''' <param name="orgnzid">検索対象組織　（マネージャーフラグ＝‘１’の場合のみデータ有）</param>
    ''' <returns>TCV活用数の月間合計数</returns>
    ''' <remarks>
    ''' スタッフの場合、本人の当月1日～前日まで（※）のTCV活用数の月間合計数を取得。
    ''' マネージャー、アシスタントの場合、配下スタッフの当月1日～前日まで（※）のTCV活用数の月間合計数を取得。
    ''' </remarks>
    Public Shared Function SelectTcvNumber(fromDate As Date, toDate As Date, stfcd As String, mngFlg As String, orgnzid As String) As SC3010204DataSet.SalesKpiSummaryMonthlyDataTable
        Using query As New DBSelectQuery(Of SC3010204DataSet.SalesKpiSummaryMonthlyDataTable)("SC3010204_005")

            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SelectTcvNumber_Start")

            Dim sql As New StringBuilder

            sql.AppendLine("SELECT /* SC3010204_005 */")
            sql.AppendLine("       SUM(SUM_VAL) SUM_VAL")
            sql.AppendLine("  FROM TB_T_SALES_KPI")
            sql.AppendLine(" WHERE SALES_KPI_ITEM_CD = '002'")
            sql.AppendLine("   AND TGT_DATE BETWEEN :FROM_DATE AND :TO_DATE")
            sql.AppendLine("   AND TGT_DLR_CD = :DLR_CD")
            sql.AppendLine("   AND TGT_BRN_CD = :BRN_CD")
            If "1".Equals(mngFlg) Then
                'マネージャ・アシスタントの場合
                sql.AppendLine("   AND TGT_ORGNZ_ID IN (" & orgnzid & ")")
            Else
                '担当の場合
                sql.AppendLine("   AND TGT_STF_CD = :STF_CD")
                query.AddParameterWithTypeValue("STF_CD", OracleDbType.NVarchar2, stfcd)    'スタッフコード
            End If

            query.AddParameterWithTypeValue("FROM_DATE", OracleDbType.Date, fromDate)   '検索対象開始日
            query.AddParameterWithTypeValue("TO_DATE", OracleDbType.Date, toDate)       '検索対象終了日
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, StaffContext.Current.DlrCD)   '販売店コード
            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, StaffContext.Current.BrnCD)   '店舗コード

            query.CommandText = sql.ToString

            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SelectTcvNumber_End")

            Return query.GetData()
        End Using
    End Function
#End Region

#Region "見積り数の月間合計数"
    ''' <summary>
    ''' 見積り数の月間合計数取得
    ''' </summary>
    ''' <param name="fromDate">検索対象開始日</param>
    ''' <param name="toDate">検索対象終了日</param>
    ''' <param name="stfcd">検索対象スタッフ</param>
    ''' <param name="mngFlg">1：マネージャー、アシスタント　0：以外</param>
    ''' <param name="orgnzid">検索対象組織　（マネージャーフラグ＝‘１’の場合のみデータ有）</param>
    ''' <returns>見積り数の月間合計数</returns>
    ''' <remarks>
    ''' スタッフの場合、本人の当月1日～前日まで（※）の見積り数の月間合計数を取得。
    ''' マネージャー、アシスタントの場合、配下スタッフの当月1日～前日まで（※）の見積り数の月間合計数を取得。
    ''' </remarks>
    Public Shared Function SelectQuotatinNumber(fromDate As Date, toDate As Date, stfcd As String, mngFlg As String, orgnzid As String) As SC3010204DataSet.SalesKpiSummaryMonthlyDataTable
        Using query As New DBSelectQuery(Of SC3010204DataSet.SalesKpiSummaryMonthlyDataTable)("SC3010204_006")

            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SelectQuotatinNumber_Start")

            Dim sql As New StringBuilder

            sql.AppendLine("SELECT /* SC3010204_006 */")
            sql.AppendLine("       SUM(SUM_VAL) SUM_VAL")
            sql.AppendLine("  FROM TB_T_SALES_KPI")
            sql.AppendLine(" WHERE SALES_KPI_ITEM_CD = '003'")
            sql.AppendLine("   AND TGT_DATE BETWEEN :FROM_DATE AND :TO_DATE")
            sql.AppendLine("   AND TGT_DLR_CD = :DLR_CD")
            sql.AppendLine("   AND TGT_BRN_CD = :BRN_CD")
            If "1".Equals(mngFlg) Then
                'マネージャ・アシスタントの場合
                sql.AppendLine("   AND TGT_ORGNZ_ID IN (" & orgnzid & ")")
            Else
                '担当の場合
                sql.AppendLine("   AND TGT_STF_CD = :STF_CD")
                query.AddParameterWithTypeValue("STF_CD", OracleDbType.NVarchar2, stfcd)    'スタッフコード
            End If

            query.AddParameterWithTypeValue("FROM_DATE", OracleDbType.Date, fromDate)   '検索対象開始日
            query.AddParameterWithTypeValue("TO_DATE", OracleDbType.Date, toDate)       '検索対象終了日
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, StaffContext.Current.DlrCD)   '販売店コード
            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, StaffContext.Current.BrnCD)   '店舗コード

            query.CommandText = sql.ToString

            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SelectQuotatinNumber_End")

            Return query.GetData()
        End Using
    End Function
#End Region

#Region "試乗数の月間合計数"

    ''' <summary>
    ''' 試乗数の月間合計数取得
    ''' </summary>
    ''' <param name="fromDate">検索対象開始日</param>
    ''' <param name="toDate">検索対象終了日</param>
    ''' <param name="stfcd">検索対象スタッフ</param>
    ''' <param name="mngFlg">1：マネージャー、アシスタント　0：以外</param>
    ''' <param name="orgnzid">検索対象組織　（マネージャーフラグ＝‘１’の場合のみデータ有）</param>
    ''' <returns>試乗数の月間合計数</returns>
    ''' <remarks>
    ''' スタッフの場合、本人の当月1日～前日まで（※）の試乗数の月間合計数を取得。
    ''' マネージャー、アシスタントの場合、配下スタッフの当月1日～前日まで（※）の試乗数の月間合計数を取得。
    ''' </remarks>
    Public Shared Function SelectTestDriveNumber(fromDate As Date, toDate As Date, stfcd As String, mngFlg As String, orgnzid As String) As SC3010204DataSet.SalesKpiSummaryMonthlyDataTable
        Using query As New DBSelectQuery(Of SC3010204DataSet.SalesKpiSummaryMonthlyDataTable)("SC3010204_007")

            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SelectTestDriveNumber_Start")

            Dim sql As New StringBuilder

            sql.AppendLine("SELECT /* SC3010204_007 */")
            sql.AppendLine("       SUM(SUM_VAL) SUM_VAL")
            sql.AppendLine("  FROM TB_T_SALES_KPI")
            sql.AppendLine(" WHERE SALES_KPI_ITEM_CD = '004'")
            sql.AppendLine("   AND TGT_DATE BETWEEN :FROM_DATE AND :TO_DATE")
            sql.AppendLine("   AND TGT_DLR_CD = :DLR_CD")
            sql.AppendLine("   AND TGT_BRN_CD = :BRN_CD")
            If "1".Equals(mngFlg) Then
                'マネージャ・アシスタントの場合
                sql.AppendLine("   AND TGT_ORGNZ_ID IN (" & orgnzid & ")")
            Else
                '担当の場合
                sql.AppendLine("   AND TGT_STF_CD = :STF_CD")
                query.AddParameterWithTypeValue("STF_CD", OracleDbType.NVarchar2, stfcd)    'スタッフコード
            End If

            query.AddParameterWithTypeValue("FROM_DATE", OracleDbType.Date, fromDate)   '検索対象開始日
            query.AddParameterWithTypeValue("TO_DATE", OracleDbType.Date, toDate)       '検索対象終了日
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, StaffContext.Current.DlrCD)   '販売店コード
            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, StaffContext.Current.BrnCD)   '店舗コード

            query.CommandText = sql.ToString

            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SelectTestDriveNumber_End")

            Return query.GetData()
        End Using
    End Function
#End Region

#Region "成約数の月間合計数"
    ''' <summary>
    ''' 成約数の月間合計数取得
    ''' </summary>
    ''' <param name="fromDate">検索対象開始日</param>
    ''' <param name="toDate">検索対象終了日</param>
    ''' <param name="stfcd">検索対象スタッフ</param>
    ''' <param name="mngFlg">1：マネージャー、アシスタント　0：以外</param>
    ''' <param name="orgnzid">検索対象組織　（マネージャーフラグ＝‘１’の場合のみデータ有）</param>
    ''' <returns>KPI活用指標（日別）</returns>
    ''' <remarks>
    ''' スタッフの場合、本人の当月1日～前日まで（※）の成約数の月間合計数を取得。
    ''' マネージャー、アシスタントの場合、配下スタッフの当月1日～前日まで（※）の成約数の月間合計数を取得。
    ''' </remarks>
    Public Shared Function SelectBookingNumber(fromDate As Date, toDate As Date, stfcd As String, mngFlg As String, orgnzid As String) As SC3010204DataSet.SalesKpiSummaryMonthlyDataTable
        Using query As New DBSelectQuery(Of SC3010204DataSet.SalesKpiSummaryMonthlyDataTable)("SC3010204_008")

            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SelectBookingNumber_Start")

            Dim sql As New StringBuilder

            sql.AppendLine("SELECT /* SC3010204_008 */")
            sql.AppendLine("       SUM(SUM_VAL) SUM_VAL")
            sql.AppendLine("  FROM TB_T_SALES_KPI")
            sql.AppendLine(" WHERE SALES_KPI_ITEM_CD = '005'")
            sql.AppendLine("   AND TGT_DATE BETWEEN :FROM_DATE AND :TO_DATE")
            sql.AppendLine("   AND TGT_DLR_CD = :DLR_CD")
            sql.AppendLine("   AND TGT_BRN_CD = :BRN_CD")
            If "1".Equals(mngFlg) Then
                'マネージャ・アシスタントの場合
                sql.AppendLine("   AND TGT_ORGNZ_ID IN (" & orgnzid & ")")
            Else
                '担当の場合
                sql.AppendLine("   AND TGT_STF_CD = :STF_CD")
                query.AddParameterWithTypeValue("STF_CD", OracleDbType.NVarchar2, stfcd)    'スタッフコード
            End If

            query.AddParameterWithTypeValue("FROM_DATE", OracleDbType.Date, fromDate)   '検索対象開始日
            query.AddParameterWithTypeValue("TO_DATE", OracleDbType.Date, toDate)       '検索対象終了日
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, StaffContext.Current.DlrCD)   '販売店コード
            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, StaffContext.Current.BrnCD)   '店舗コード

            query.CommandText = sql.ToString

            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SelectBookingNumber_End")

            Return query.GetData()
        End Using
    End Function
#End Region

#Region "納車数の月間合計数"

    ''' <summary>
    ''' 納車数の月間合計数取得
    ''' </summary>
    ''' <param name="fromDate">検索対象開始日</param>
    ''' <param name="toDate">検索対象終了日</param>
    ''' <param name="stfcd">検索対象スタッフ</param>
    ''' <param name="mngFlg">1：マネージャー、アシスタント　0：以外</param>
    ''' <param name="orgnzid">検索対象組織　（マネージャーフラグ＝‘１’の場合のみデータ有）</param>
    ''' <returns>KPI活用指標（日別）</returns>
    ''' <remarks>
    ''' スタッフの場合、本人の当月1日～前日まで（※）の納車数の月間合計数を取得。
    ''' マネージャー、アシスタントの場合、配下スタッフの当月1日～前日まで（※）の納車数の月間合計数を取得。
    ''' </remarks>
    Public Shared Function SelectB2DNegotiationNumber(fromDate As Date, toDate As Date, stfcd As String, mngFlg As String, orgnzid As String) As SC3010204DataSet.SalesKpiSummaryMonthlyDataTable
        Using query As New DBSelectQuery(Of SC3010204DataSet.SalesKpiSummaryMonthlyDataTable)("SC3010204_009")

            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SelectB2DNegotiationNumber_Start")

            Dim sql As New StringBuilder

            sql.AppendLine("SELECT /* SC3010204_009 */")
            sql.AppendLine("       SUM(SUM_VAL) SUM_VAL")
            sql.AppendLine("  FROM TB_T_SALES_KPI")
            sql.AppendLine(" WHERE SALES_KPI_ITEM_CD = '006'")
            sql.AppendLine("   AND TGT_DATE BETWEEN :FROM_DATE AND :TO_DATE")
            sql.AppendLine("   AND TGT_DLR_CD = :DLR_CD")
            sql.AppendLine("   AND TGT_BRN_CD = :BRN_CD")
            If "1".Equals(mngFlg) Then
                'マネージャ・アシスタントの場合
                sql.AppendLine("   AND TGT_ORGNZ_ID IN (" & orgnzid & ")")
            Else
                '担当の場合
                sql.AppendLine("   AND TGT_STF_CD = :STF_CD")
                query.AddParameterWithTypeValue("STF_CD", OracleDbType.NVarchar2, stfcd)    'スタッフコード
            End If

            query.AddParameterWithTypeValue("FROM_DATE", OracleDbType.Date, fromDate)   '検索対象開始日
            query.AddParameterWithTypeValue("TO_DATE", OracleDbType.Date, toDate)       '検索対象終了日
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, StaffContext.Current.DlrCD)   '販売店コード
            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, StaffContext.Current.BrnCD)   '店舗コード

            query.CommandText = sql.ToString

            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SelectB2DNegotiationNumber_End")

            Return query.GetData()
        End Using
    End Function
#End Region

#Region "N分超過接客数の月間合計数"

    ''' <summary>
    ''' N分超過接客数の月間合計数取得
    ''' </summary>
    ''' <param name="fromDate">検索対象開始日</param>
    ''' <param name="toDate">検索対象終了日</param>
    ''' <param name="stfcd">検索対象スタッフ</param>
    ''' <param name="mngFlg">1：マネージャー、アシスタント　0：以外</param>
    ''' <param name="orgnzid">検索対象組織　（マネージャーフラグ＝‘１’の場合のみデータ有）</param>
    ''' <returns>N分超過接客数の月間合計数</returns>
    ''' <remarks>
    ''' スタッフの場合、本人の当月1日～前日まで（※）のN分超過接客数の月間合計数を取得。
    ''' マネージャー、アシスタントの場合、配下スタッフの当月1日～前日まで（※）のN分超過接客数の月間合計数を取得。
    ''' </remarks>
    Public Shared Function SelectNormalNegotiationNumber(fromDate As Date, toDate As Date, stfcd As String, mngFlg As String, orgnzid As String) As SC3010204DataSet.SalesKpiSummaryMonthlyDataTable
        Using query As New DBSelectQuery(Of SC3010204DataSet.SalesKpiSummaryMonthlyDataTable)("SC3010204_010")

            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SelectNormalNegotiationNumber_Start")

            Dim sql As New StringBuilder

            sql.AppendLine("SELECT /* SC3010204_010 */")
            sql.AppendLine("       SUM(SUM_VAL) SUM_VAL")
            sql.AppendLine("  FROM TB_T_SALES_KPI")
            sql.AppendLine(" WHERE SALES_KPI_ITEM_CD = '007'")
            sql.AppendLine("   AND TGT_DATE BETWEEN :FROM_DATE AND :TO_DATE")
            sql.AppendLine("   AND TGT_DLR_CD = :DLR_CD")
            sql.AppendLine("   AND TGT_BRN_CD = :BRN_CD")
            If "1".Equals(mngFlg) Then
                'マネージャ・アシスタントの場合
                sql.AppendLine("   AND TGT_ORGNZ_ID IN (" & orgnzid & ")")
            Else
                '担当の場合
                sql.AppendLine("   AND TGT_STF_CD = :STF_CD")
                query.AddParameterWithTypeValue("STF_CD", OracleDbType.NVarchar2, stfcd)    'スタッフコード
            End If

            query.AddParameterWithTypeValue("FROM_DATE", OracleDbType.Date, fromDate)   '検索対象開始日
            query.AddParameterWithTypeValue("TO_DATE", OracleDbType.Date, toDate)       '検索対象終了日
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, StaffContext.Current.DlrCD)   '販売店コード
            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, StaffContext.Current.BrnCD)   '店舗コード

            query.CommandText = sql.ToString

            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SelectNormalNegotiationNumber_End")

            Return query.GetData()
        End Using
    End Function
#End Region

#Region "配下組織取得"
    ''' <summary>
    ''' 店舗セールス組織取得
    ''' </summary>
    ''' <returns>SC3010204DataSet.BranchSalesOrganzDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetBranchSalesOrganizations() As SC3010204DataSet.BranchSalesOrganzDataTable

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetBranchSalesOrganizations_Start")
        'ログ出力 End *****************************************************************************

        Dim dlrCd As String = StaffContext.Current.DlrCD
        Dim brnCd As String = StaffContext.Current.BrnCD

        Dim sql As New StringBuilder
        With sql
            .Append("SELECT /* SC3010204_001 */")
            .Append("    ORGNZ_ID")
            .Append("    ,PARENT_ORGNZ_ID")
            .Append("    ,ORGNZ_SC_FLG ")
            .Append("FROM ")
            .Append("    TB_M_ORGANIZATION ")
            .Append("WHERE ")
            .Append("    DLR_CD = :DLR_CD ")
            .Append("    AND BRN_CD = :BRN_CD ")
        End With

        Using query As New DBSelectQuery(Of SC3010204DataSet.BranchSalesOrganzDataTable)("SC3010204_001")

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.Char, dlrCd)
            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.Char, brnCd)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetBranchSalesOrganizations_End")
            'ログ出力 End *****************************************************************************

            Return query.GetData()

        End Using

    End Function
#End Region

#Region "V4 システム設定値取得"

    '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 DEL
    '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 DEL

#End Region

End Class
