'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'IC3040401BusinessLogic.vb
'─────────────────────────────────────
'機能： ToDo一覧データ取得クラス
'補足： 
'作成： 2012/01/26 TCS 竹内
'更新： 2012/09/28 TCS 渡邊   【SALES_Step3】GTMC120924022の不具合修正
'更新： 2013/06/30 TCS 武田   2013/10対応版　既存流用
'更新： 2014/02/17 TCS 山田   受注後フォロー機能開発
'更新： 2015/12/08 TCS 中村   (ﾄﾗｲ店ｼｽﾃﾑ評価)新車ﾀﾌﾞﾚｯﾄ(受注後)の管理機能開発
'更新： 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-3
'更新： 2017/12/07 TCS 河原 販売流通関連システムのiOS10.3適用
'削除： 2020/01/06 TS  重松 [TMTレスポンススロー] SLT基盤への横展
'─────────────────────────────────────

Imports System.Globalization
Imports System.Reflection.MethodBase
Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Reflection

Public NotInheritable Class SC3010401DataTableTableAdapter

#Region "定数"

    ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 受注状態：31
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StatusSuccess As String = "31"
    ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END

    ' 2014/02/17 TCS 山田 受注後フォロー機能開発 START
    ''' <summary>
    ''' 検索項目
    ''' </summary>
    ''' <remarks></remarks>
    Public Const IdSearchName As String = "001"        '顧客名称
    Public Const IdSearchTel As String = "002"         '電話番号/携帯番号
    Public Const IdSearchSocialNum As String = "003"   '国民ID
    Public Const IdSearchBookingNo As String = "004"   '注文番号
    Public Const IdSearchVin As String = "005"         'VIN

    ''' <summary>
    ''' 検索方向 (1:前方一致)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const IdSearchDirectionAfter As Integer = 1

    ''' <summary>
    ''' 検索方向 (2:あいまい検索)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const IdSearchDirectionAll As Integer = 2
    ' 2014/02/17 TCS 山田 受注後フォロー機能開発 END

#End Region

#Region "デフォルトコンストラクタ処理"
    ''' <summary>
    ''' デフォルトコンストラクタ処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub New()
        '処理なし
    End Sub
#End Region

    ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START DEL
    ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END

    ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 車種ＣＤ取得
    ''' </summary>
    ''' <param name="fllwupboxseqno">商談ID</param>
    ''' <returns>SC3010401GetSeriesCDDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetSeriesCD(ByVal fllwupboxseqno As Decimal, ByVal cractresult As String) As SC3010401DataSet.SC3010401GetSeriesCDDataTable

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0}_Start, fllwupboxseqno:[{1}]",
                                  MethodBase.GetCurrentMethod.Name, fllwupboxseqno))
        ' ======================== ログ出力 終了 ========================
        ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END

        Dim sql As New StringBuilder
        With sql
            ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
            .AppendLine(" SELECT /* SC3010401_002 */ ")
            .AppendLine("        T2.MODEL_CD AS SERIESCD ")
            .AppendLine("      , T2.GRADE_CD AS MODELCD ")
            .AppendLine("   FROM (SELECT T1.MODEL_CD ")
            .AppendLine("              , T1.GRADE_CD ")
            .AppendLine("           FROM TB_T_PREFER_VCL T1 ")
            .AppendLine("          WHERE T1.SALES_ID = :SALES_ID ")
            .AppendLine("          ORDER BY T1.PREF_VCL_SEQ) T2 ")
            .AppendLine("  WHERE ROWNUM = 1 ")
            .AppendLine("  UNION ALL ")
            .AppendLine(" SELECT T102.MODEL_CD AS SERIESCD ")
            .AppendLine("      , T102.GRADE_CD AS MODELCD ")
            .AppendLine("   FROM (SELECT T101.MODEL_CD ")
            .AppendLine("              , T101.GRADE_CD ")
            .AppendLine("           FROM TB_H_PREFER_VCL T101 ")
            .AppendLine("          WHERE T101.SALES_ID = :SALES_ID ")
            If StatusSuccess.Equals(cractresult) Then
                .AppendLine("            AND T101.SALES_STATUS = '31' ")
            End If
            .AppendLine("          ORDER BY T101.PREF_VCL_SEQ) T102 ")
            .AppendLine("  WHERE ROWNUM = 1 ")
            ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END
        End With
        Using query As New DBSelectQuery(Of SC3010401DataSet.SC3010401GetSeriesCDDataTable)("SC3010401_002")
            query.CommandText = sql.ToString()

            ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
            query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, fllwupboxseqno)  '商談ID

            ' SQLを実行
            Dim dt As SC3010401DataSet.SC3010401GetSeriesCDDataTable = query.GetData()

            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "{0}_End, Return RowCount:[{1}]",
                                      MethodBase.GetCurrentMethod.Name, dt.Rows.Count))
            ' ======================== ログ出力 終了 ========================
            Return dt
            ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END
        End Using

    End Function

    ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 顧客ＩＤから顧客情報を取得
    ''' </summary>
    ''' <param name="insdid">顧客ID</param>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <returns>SC3010401GetCustDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetCustomer(
        ByVal insdid As String,
        ByVal dlrcd As String) As SC3010401DataSet.SC3010401GetCustDataTable

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0}_Start, insdid:[{1}], dlrcd:[{2}]",
                                  MethodBase.GetCurrentMethod.Name, insdid, dlrcd))
        ' ======================== ログ出力 終了 ========================
        ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END

        Dim sql As New StringBuilder
        With sql
            ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
            .AppendLine(" SELECT /* SC3010401_003 */ ")
            .AppendLine("        T1.CST_ID AS KOKYAKUID ")
            .AppendLine("      , ' ' AS STAFFCD ")
            .AppendLine("      , T2.IMG_FILE_SMALL AS IMAGEFILE_S ")
            .AppendLine("      , T2.CST_TYPE AS KOKYAKUKBN ")
            .AppendLine("      , T1.NAMETITLE_NAME AS NAMETITLE_NAME ")
            ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-3 START
            .AppendLine("      , NVL(T4.CST_JOIN_TYPE, ' ') AS CSTJOINTYPE ")
            .AppendLine("  FROM TB_M_CUSTOMER T1 ")
            .AppendLine("      ,TB_M_CUSTOMER_DLR T2 ")
            .AppendLine("      ,TB_M_PRIVATE_FLEET_ITEM T3 ")
            .AppendLine("      ,TB_LM_PRIVATE_FLEET_ITEM T4 ")
            .AppendLine(" WHERE T1.CST_ID = T2.CST_ID ")
            .AppendLine("   AND T1.CST_ID = :KOKYAKUID ")
            .AppendLine("   AND T2.DLR_CD = :DLRCD ")
            .AppendLine("   AND T1.PRIVATE_FLEET_ITEM_CD = T3.PRIVATE_FLEET_ITEM_CD(+) ")
            .AppendLine("   AND T3.PRIVATE_FLEET_ITEM_CD = T4.PRIVATE_FLEET_ITEM_CD(+) ")
            .AppendLine("   AND T1.FLEET_FLG = T3.FLEET_FLG(+) ")
            .AppendLine("   AND T3.INUSE_FLG(+) = '1' ")
            ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-3 END
            ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END
        End With
        Using query As New DBSelectQuery(Of SC3010401DataSet.SC3010401GetCustDataTable)("SC3010401_003")
            query.CommandText = sql.ToString()
            ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
            query.AddParameterWithTypeValue("KOKYAKUID", OracleDbType.Decimal, insdid)      '顧客ＩＤ
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)      '販売店

            ' SQLを実行
            Dim dt As SC3010401DataSet.SC3010401GetCustDataTable = query.GetData()

            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "{0}_End, Return RowCount:[{1}]",
                                      MethodBase.GetCurrentMethod.Name, dt.Rows.Count))
            ' ======================== ログ出力 終了 ========================

            Return dt
            ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END
        End Using

    End Function
    ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 苦情情報取得
    ''' </summary>
    ''' <param name="actualdate">活動日</param>
    ''' <param name="insdid">顧客ID</param>
    ''' <returns>SC3010401GetComplaintDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetComplaint(ByVal actualdate As DateTime,
      ByVal insdid As String) As SC3010401DataSet.SC3010401GetComplaintDataTable

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0}_Start, actualdate:[{1}], insdid:[{2}]",
                                  MethodBase.GetCurrentMethod.Name,
                                  actualdate.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture), insdid))
        ' ======================== ログ出力 終了 ========================

        Dim sql As New StringBuilder
        With sql

            .AppendLine(" SELECT /* SC3010401_004 */ ")
            .AppendLine("        T0.CMPCNT0 + T100.CMPCNT100 AS CMPCNT")
            .AppendLine("  FROM (SELECT COUNT(1) AS CMPCNT0 ")
            .AppendLine("          FROM TB_T_COMPLAINT T1 ")
            .AppendLine("             , TB_T_REQUEST T2 ")
            .AppendLine("         WHERE T1.REQ_ID = T2.REQ_ID ")
            .AppendLine("           AND T2.CST_ID = :KOKYAKUID ")
            .AppendLine("           AND T1.RELATION_TYPE IN ('0', '1') ")
            .AppendLine("       ) T0 ")
            .AppendLine("     , (SELECT COUNT(1) AS CMPCNT100 ")
            .AppendLine("          FROM TB_H_COMPLAINT T101 ")
            .AppendLine("             , TB_H_REQUEST T102 ")
            .AppendLine("         WHERE T101.REQ_ID = T102.REQ_ID ")
            .AppendLine("           AND T102.CST_ID = :KOKYAKUID ")
            .AppendLine("           AND T101.RELATION_TYPE IN ('0', '1') ")
            .AppendLine("           AND EXISTS (SELECT 1 ")
            .AppendLine("                         FROM TB_H_COMPLAINT_DETAIL T103 ")
            .AppendLine("                        WHERE T101.CMPL_ID = T103.CMPL_ID ")
            .AppendLine("                          AND T103.FIRST_LAST_ACT_TYPE = '2' ")
            .AppendLine("                          AND T103.ACT_DATETIME >= :ACTUALDATE ")
            .AppendLine("                          AND T103.CMPL_DETAIL_ID = (SELECT MAX(T104.CMPL_DETAIL_ID) ")
            .AppendLine("                                                       FROM TB_H_COMPLAINT_DETAIL T104 ")
            .AppendLine("                                                      WHERE T101.CMPL_ID = T104.CMPL_ID) ")
            .AppendLine("               ) ")
            .AppendLine("       ) T100 ")

        End With
        Using query As New DBSelectQuery(Of SC3010401DataSet.SC3010401GetComplaintDataTable)("SC3010401_004")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("KOKYAKUID", OracleDbType.Decimal, insdid)        '顧客ＩＤ
            query.AddParameterWithTypeValue("ACTUALDATE", OracleDbType.Date, actualdate)   '基準日

            ' SQLを実行
            Dim dt As SC3010401DataSet.SC3010401GetComplaintDataTable = query.GetData()

            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "{0}_End, Return RowCount:[{1}]",
                                      MethodBase.GetCurrentMethod.Name, dt.Rows.Count))
            ' ======================== ログ出力 終了 ========================

            Return dt
        End Using

    End Function
    ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END

    ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START DEL
    ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END

    ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 車種名取得
    ''' </summary>
    ''' <param name="seriescd">モデルコード</param>
    ''' <param name="modelcd">グレードコード</param>
    ''' <returns>SC3010401GetSelectedSeriesDataTable</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' <para>更新： 2012/09/28 TCS 渡邊 【SALES_Step3】GTMC120924022の不具合修正</para>
    ''' </history>
    Public Shared Function GetSelectedSeries(ByVal seriescd As String,
          ByVal modelcd As String) As SC3010401DataSet.SC3010401GetSelectedSeriesDataTable

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0}_Start, seriescd:[{1}], modelcd:[{2}]",
                                  MethodBase.GetCurrentMethod.Name,
                                  seriescd, modelcd))
        ' ======================== ログ出力 終了 ========================
        ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END

        Dim sql As New StringBuilder
        With sql
            ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
            .AppendLine(" SELECT /* SC3010401_006 */ ")
            .AppendLine("        T1.MODEL_NAME AS SERIESNM ")
            .AppendLine("      , T2.GRADE_NAME AS VCLMODEL_NAME ")
            .AppendLine("   FROM TB_M_MODEL T1 ")
            .AppendLine("      , TB_M_GRADE T2")
            .AppendLine("  WHERE T1.MODEL_CD = T2.MODEL_CD(+) ")
            .AppendLine("    AND T1.MODEL_CD = :SERIESCD ")
            .AppendLine("    AND (T2.GRADE_CD(+) = :MODELCD ")
            .AppendLine("     OR T2.GRADE_CD(+) = 'X') ")
            ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END
        End With
        Using query As New DBSelectQuery(Of SC3010401DataSet.SC3010401GetSelectedSeriesDataTable)("SC3010401_006")
            query.CommandText = sql.ToString()
            ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
            query.AddParameterWithTypeValue("SERIESCD", OracleDbType.NVarchar2, seriescd)
            query.AddParameterWithTypeValue("MODELCD", OracleDbType.NVarchar2, modelcd)

            ' SQLを実行
            Dim dt As SC3010401DataSet.SC3010401GetSelectedSeriesDataTable = query.GetData()

            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "{0}_End, Return RowCount:[{1}]",
                                      MethodBase.GetCurrentMethod.Name, dt.Rows.Count))
            ' ======================== ログ出力 終了 ========================

            Return dt
            ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END
        End Using
    End Function

    ' 2014/02/17 TCS 山田 受注後フォロー機能開発 START
    ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
     ''' <summary>
     ''' 見積情報から注文番号を取得
     ''' </summary>
     ''' <param name="dlrcd">販売店コード</param>
     ''' <param name="fllwupboxseqno">商談ID</param>
     ''' <returns>SC3010401GetBookingNoDataTable</returns>
     ''' <remarks></remarks>
    Public Shared Function GetBookingNo(ByVal dlrcd As String,
          ByVal fllwupboxseqno As Decimal) As SC3010401DataSet.SC3010401GetBookingNoDataTable

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0}_Start, dlrcd:[{1}], fllwupboxseqno:{2}",
                                  MethodBase.GetCurrentMethod.Name,
                                  dlrcd, fllwupboxseqno))
        ' ======================== ログ出力 終了 ========================
        ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END

        Dim sql As New StringBuilder
        With sql
            .AppendLine("SELECT /* SC3010401_007 */")
            .AppendLine("       E.CONTRACTNO BOOKINGNO")
            .AppendLine("  FROM")
            .AppendLine("       TBL_ESTIMATEINFO E")
            .AppendLine(" WHERE")
            .AppendLine("       E.DLRCD = :DLRCD")
            .AppendLine("   AND E.FLLWUPBOX_SEQNO = :FLLWUPBOXSEQNO")
            .AppendLine("   AND E.CONTRACTFLG = 1")
            .AppendLine("   AND E.DELFLG = 0")
        End With
        Using query As New DBSelectQuery(Of SC3010401DataSet.SC3010401GetBookingNoDataTable)("SC3010401_007")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)      '販売店
            ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
            query.AddParameterWithTypeValue("FLLWUPBOXSEQNO", OracleDbType.Decimal, fllwupboxseqno)      '商談ＩＤ

            ' SQLを実行
            Dim dt As SC3010401DataSet.SC3010401GetBookingNoDataTable = query.GetData()
            ' 2014/02/17 TCS 山田 受注後フォロー機能開発 END

            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "{0}_End, Return RowCount:[{1}]",
                                      MethodBase.GetCurrentMethod.Name, dt.Rows.Count))
            ' ======================== ログ出力 終了 ========================

            Return dt
            ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END
        End Using

    End Function

    ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
    ''' <summary>
    ''' コンタクトアイコンパス取得
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <returns>SC3010401GetContactIconpathDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetContactIconPath(ByVal dlrcd As String) As SC3010401DataSet.SC3010401GetContactIconpathDataTable
        Using query As New DBSelectQuery(Of SC3010401DataSet.SC3010401GetContactIconpathDataTable)("SC3010401_008")

            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "{0}_Start, dlrcd:[{1}]",
                                      MethodBase.GetCurrentMethod.Name,
                                      dlrcd))
            ' ======================== ログ出力 終了 ========================
            ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END

            Dim sql As New StringBuilder
            With sql
                ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
                .AppendLine("SELECT /* SC3010401_008 */ ")
                .AppendLine("       T1.CONTACT_MTD AS CONTACTNO ")
                .AppendLine("     , NVL(T100.ICON_PATH, T200.ICON_PATH) AS CONTACTICONPATH ")
                .AppendLine("  FROM ")
                .AppendLine("       TB_M_CONTACT_MTD T1 ")
                .AppendLine("     , (SELECT T101.FIRST_KEY, T101.ICON_PATH ")
                .AppendLine("          FROM TB_M_IMG_PATH_CONTROL T101 ")
                .AppendLine("         WHERE T101.DLR_CD = :DLR_CD ")
                .AppendLine("           AND T101.TYPE_CD = 'CONTACT_MTD' ")
                .AppendLine("           AND T101.DEVICE_TYPE = '01' ")
                .AppendLine("           AND T101.SECOND_KEY = '10') T100 ")
                .AppendLine("     , (SELECT T201.FIRST_KEY, T201.ICON_PATH ")
                .AppendLine("          FROM TB_M_IMG_PATH_CONTROL T201 ")
                .AppendLine("         WHERE T201.DLR_CD = 'XXXXX' ")
                .AppendLine("           AND T201.TYPE_CD = 'CONTACT_MTD' ")
                .AppendLine("           AND T201.DEVICE_TYPE = '01' ")
                .AppendLine("           AND T201.SECOND_KEY = '10') T200 ")
                .AppendLine(" WHERE T1.CONTACT_MTD = T100.FIRST_KEY(+) ")
                .AppendLine("   AND T1.CONTACT_MTD = T200.FIRST_KEY(+) ")
                '2017/12/07 TCS 河原 販売流通関連システムのiOS10.3適用 DEL
                ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END
            End With
            query.CommandText = sql.ToString()

            ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dlrcd)      '販売店コード

            ' SQLを実行
            Dim dt As SC3010401DataSet.SC3010401GetContactIconpathDataTable = query.GetData()

            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "{0}_End, Return RowCount:[{1}]",
                                      MethodBase.GetCurrentMethod.Name, dt.Rows.Count))
            ' ======================== ログ出力 終了 ========================

            Return dt
            ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END
        End Using
    End Function

    ' 2014/02/17 TCS 山田 受注後フォロー機能開発 START DEL
    ' 2014/02/17 TCS 山田 受注後フォロー機能開発 END

    ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 主情報（用件）取得
    ''' </summary>
    ''' <param name="salesID">商談ID</param>
    ''' <returns>SC3010401GetMainDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetMainRequest(ByVal salesId As Decimal) As SC3010401DataSet.SC3010401GetMainDataTable

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0}_Start, salesId:[{1}]",
                                  MethodBase.GetCurrentMethod.Name,
                                  salesId))
        ' ======================== ログ出力 終了 ========================

        Dim sql As New StringBuilder
        With sql
            .AppendLine(" SELECT /* SC3010401_010 */ ")
            .AppendLine("        T2.DLR_CD AS DLRCD ")
            .AppendLine("      , T2.BRN_CD AS STRCD ")
            .AppendLine("      , T2.SALES_ID AS FLLWUPBOX_SEQNO ")
            .AppendLine("      , T4.CST_ID AS INSDID ")
            .AppendLine("      , T3.CST_TYPE AS CUSTSEGMENT ")
            .AppendLine("      , T1.REQ_STATUS AS CRACTRESULT ")
            .AppendLine("      , CASE ")
            .AppendLine("             WHEN T1.REQ_STATUS = '31' THEN 400 ")
            .AppendLine("             WHEN T1.REQ_STATUS = '32' THEN 500 ")
            .AppendLine("             WHEN T2.SALES_PROSPECT_CD = '30' THEN 100 ")
            .AppendLine("             WHEN T2.SALES_PROSPECT_CD = '20' THEN 200 ")
            .AppendLine("             ELSE 300 ")
            .AppendLine("        END AS CRRESULTSORT ")
            .AppendLine("      , T4.CST_VCL_TYPE AS CUSTOMERCLASS ")
            .AppendLine("   FROM TB_T_REQUEST T1 ")
            .AppendLine("      , TB_T_SALES T2 ")
            .AppendLine("      , TB_M_CUSTOMER_DLR T3 ")
            .AppendLine("      , TB_M_CUSTOMER_VCL T4 ")
            .AppendLine("  WHERE T1.REQ_ID = T2.REQ_ID ")
            .AppendLine("    AND T2.DLR_CD = T3.DLR_CD ")
            .AppendLine("    AND T3.CST_ID = T4.CST_ID ")
            .AppendLine("    AND T3.DLR_CD = T4.DLR_CD ")
            .AppendLine("    AND T1.VCL_ID = T4.VCL_ID ")
            .AppendLine("    AND T4.CST_VCL_TYPE = '1' ")
            .AppendLine("    AND T4.OWNER_CHG_FLG = '0' ")
            .AppendLine("    AND T2.SALES_ID = :FLLWUPBOX_SEQNO ")
            .AppendLine("  UNION ALL ")
            .AppendLine(" SELECT T102.DLR_CD AS DLRCD ")
            .AppendLine("      , T102.BRN_CD AS STRCD ")
            .AppendLine("      , T102.SALES_ID AS FLLWUPBOX_SEQNO ")
            .AppendLine("      , T104.CST_ID AS INSDID ")
            .AppendLine("      , T103.CST_TYPE AS CUSTSEGMENT ")
            .AppendLine("      , T101.REQ_STATUS AS CRACTRESULT ")
            .AppendLine("      , CASE ")
            .AppendLine("             WHEN T101.REQ_STATUS = '31' THEN 400 ")
            .AppendLine("             WHEN T101.REQ_STATUS = '32' THEN 500 ")
            .AppendLine("             WHEN T102.SALES_PROSPECT_CD = '30' THEN 100 ")
            .AppendLine("             WHEN T102.SALES_PROSPECT_CD = '20' THEN 200 ")
            .AppendLine("             ELSE 300 ")
            .AppendLine("        END AS CRRESULTSORT ")
            .AppendLine("      , T104.CST_VCL_TYPE AS CUSTOMERCLASS ")
            .AppendLine("   FROM TB_H_REQUEST T101 ")
            .AppendLine("      , TB_H_SALES T102 ")
            .AppendLine("      , TB_M_CUSTOMER_DLR T103 ")
            .AppendLine("      , TB_M_CUSTOMER_VCL T104 ")
            .AppendLine("  WHERE T101.REQ_ID = T102.REQ_ID ")
            .AppendLine("    AND T102.DLR_CD = T103.DLR_CD ")
            .AppendLine("    AND T103.CST_ID = T104.CST_ID ")
            .AppendLine("    AND T103.DLR_CD = T104.DLR_CD ")
            .AppendLine("    AND T101.VCL_ID = T104.VCL_ID ")
            .AppendLine("    AND T104.CST_VCL_TYPE = '1' ")
            .AppendLine("    AND T104.OWNER_CHG_FLG = '0' ")
            .AppendLine("    AND T102.SALES_ID = :FLLWUPBOX_SEQNO ")
        End With
        Using query As New DBSelectQuery(Of SC3010401DataSet.SC3010401GetMainDataTable)("SC3010401_010")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, salesId)      '商談ID

            Dim dt As SC3010401DataSet.SC3010401GetMainDataTable = query.GetData()

            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "{0}_End, Return RowCount:[{1}]",
                                      MethodBase.GetCurrentMethod.Name, dt.Rows.Count))
            ' ======================== ログ出力 終了 ========================

            Return dt
        End Using

    End Function
    ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END

    ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 主情報（誘致）取得
    ''' </summary>
    ''' <param name="salesID">商談ID</param>
    ''' <returns>SC3010401DataSet.SC3010401GetMainDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetMainAttract(ByVal salesId As Decimal) As SC3010401DataSet.SC3010401GetMainDataTable

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0}_Start, salesId:{1}",
                                  MethodBase.GetCurrentMethod.Name,
                                  salesId))
        ' ======================== ログ出力 終了 ========================

        Dim sql As New StringBuilder
        With sql
            .AppendLine(" SELECT /* SC3010401_011 */ ")
            .AppendLine("        T2.DLR_CD AS DLRCD ")
            .AppendLine("      , T2.BRN_CD AS STRCD ")
            .AppendLine("      , T2.SALES_ID AS FLLWUPBOX_SEQNO ")
            .AppendLine("      , T4.CST_ID AS INSDID ")
            .AppendLine("      , T3.CST_TYPE AS CUSTSEGMENT ")
            .AppendLine("      , T1.CONTINUE_ACT_STATUS AS CRACTRESULT ")
            .AppendLine("      , CASE ")
            .AppendLine("             WHEN T1.CONTINUE_ACT_STATUS = '31' THEN 400 ")
            .AppendLine("             WHEN T1.CONTINUE_ACT_STATUS = '32' THEN 500 ")
            .AppendLine("             WHEN T2.SALES_PROSPECT_CD = '30' THEN 100 ")
            .AppendLine("             WHEN T2.SALES_PROSPECT_CD = '20' THEN 200 ")
            .AppendLine("             ELSE 300 ")
            .AppendLine("        END AS CRRESULTSORT ")
            .AppendLine("      , T4.CST_VCL_TYPE AS CUSTOMERCLASS ")
            .AppendLine("   FROM TB_T_ATTRACT T1 ")
            .AppendLine("      , TB_T_SALES T2 ")
            .AppendLine("      , TB_M_CUSTOMER_DLR T3 ")
            .AppendLine("      , TB_M_CUSTOMER_VCL T4 ")
            .AppendLine("  WHERE T1.ATT_ID = T2.ATT_ID ")
            .AppendLine("    AND T2.DLR_CD = T3.DLR_CD ")
            .AppendLine("    AND T3.CST_ID = T4.CST_ID ")
            .AppendLine("    AND T3.DLR_CD = T4.DLR_CD ")
            .AppendLine("    AND T1.VCL_ID = T4.VCL_ID ")
            .AppendLine("    AND T4.CST_VCL_TYPE = '1' ")
            .AppendLine("    AND T4.OWNER_CHG_FLG = '0' ")
            .AppendLine("    AND T2.SALES_ID = :FLLWUPBOX_SEQNO ")
            .AppendLine("  UNION ALL ")
            .AppendLine(" SELECT T102.DLR_CD AS DLRCD ")
            .AppendLine("      , T102.BRN_CD AS STRCD ")
            .AppendLine("      , T102.SALES_ID AS FLLWUPBOX_SEQNO ")
            .AppendLine("      , T104.CST_ID AS INSDID ")
            .AppendLine("      , T103.CST_TYPE AS CUSTSEGMENT ")
            .AppendLine("      , T101.CONTINUE_ACT_STATUS AS CRACTRESULT ")
            .AppendLine("      , CASE ")
            .AppendLine("             WHEN T101.CONTINUE_ACT_STATUS = '31' THEN 400 ")
            .AppendLine("             WHEN T101.CONTINUE_ACT_STATUS = '32' THEN 500 ")
            .AppendLine("             WHEN T102.SALES_PROSPECT_CD = '30' THEN 100 ")
            .AppendLine("             WHEN T102.SALES_PROSPECT_CD = '20' THEN 200 ")
            .AppendLine("             ELSE 300 ")
            .AppendLine("        END AS CRRESULTSORT ")
            .AppendLine("      , T104.CST_VCL_TYPE AS CUSTOMERCLASS ")
            .AppendLine("   FROM TB_H_ATTRACT T101 ")
            .AppendLine("      , TB_H_SALES T102 ")
            .AppendLine("      , TB_M_CUSTOMER_DLR T103 ")
            .AppendLine("      , TB_M_CUSTOMER_VCL T104 ")
            .AppendLine("  WHERE T101.ATT_ID = T102.ATT_ID ")
            .AppendLine("    AND T102.DLR_CD = T103.DLR_CD ")
            .AppendLine("    AND T103.CST_ID = T104.CST_ID ")
            .AppendLine("    AND T103.DLR_CD = T104.DLR_CD ")
            .AppendLine("    AND T101.VCL_ID = T104.VCL_ID ")
            .AppendLine("    AND T104.CST_VCL_TYPE = '1' ")
            .AppendLine("    AND T104.OWNER_CHG_FLG = '0' ")
            .AppendLine("    AND T102.SALES_ID = :FLLWUPBOX_SEQNO ")
        End With
        Using query As New DBSelectQuery(Of SC3010401DataSet.SC3010401GetMainDataTable)("SC3010401_011")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, salesId)      '商談ID

            Dim dt As SC3010401DataSet.SC3010401GetMainDataTable = query.GetData()

            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "{0}_End, Return RowCount:[{1}]",
                                      MethodBase.GetCurrentMethod.Name, dt.Rows.Count))
            ' ======================== ログ出力 終了 ========================

            Return dt
        End Using

    End Function
    ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END

    ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 主情報（車両なし用件）取得
    ''' </summary>
    ''' <param name="salesID">商談ID</param>
    ''' <param name="cstId">顧客ID</param>
    ''' <returns>SC3010401GetMainDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetMainRequestNoVcl(ByVal salesId As Decimal,
      ByVal cstId As Decimal) As SC3010401DataSet.SC3010401GetMainDataTable

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0}_Start, salesId:[{1}], cstId:[{2}]",
                                  MethodBase.GetCurrentMethod.Name,
                                  salesId, cstId))
        ' ======================== ログ出力 終了 ========================

        Dim sql As New StringBuilder
        With sql
            .AppendLine(" SELECT /* SC3010401_012 */ ")
            .AppendLine("        T2.DLR_CD AS DLRCD ")
            .AppendLine("      , T2.BRN_CD AS STRCD ")
            .AppendLine("      , T2.SALES_ID AS FLLWUPBOX_SEQNO ")
            .AppendLine("      , T3.CST_ID AS INSDID ")
            .AppendLine("      , T3.CST_TYPE AS CUSTSEGMENT ")
            .AppendLine("      , T1.REQ_STATUS AS CRACTRESULT ")
            .AppendLine("      , CASE ")
            .AppendLine("             WHEN T1.REQ_STATUS = '31' THEN 400 ")
            .AppendLine("             WHEN T1.REQ_STATUS = '32' THEN 500 ")
            .AppendLine("             WHEN T2.SALES_PROSPECT_CD = '30' THEN 100 ")
            .AppendLine("             WHEN T2.SALES_PROSPECT_CD = '20' THEN 200 ")
            .AppendLine("             ELSE 300 ")
            .AppendLine("        END AS CRRESULTSORT ")
            .AppendLine("      , '1' AS CUSTOMERCLASS ")
            .AppendLine("   FROM TB_T_REQUEST T1 ")
            .AppendLine("      , TB_T_SALES T2 ")
            .AppendLine("      , TB_M_CUSTOMER_DLR T3 ")
            .AppendLine("  WHERE T1.REQ_ID = T2.REQ_ID ")
            .AppendLine("    AND T2.DLR_CD = T3.DLR_CD ")
            .AppendLine("    AND T3.CST_ID = :CST_ID ")
            .AppendLine("    AND T2.SALES_ID = :FLLWUPBOX_SEQNO ")
            .AppendLine("  UNION ALL ")
            .AppendLine(" SELECT T102.DLR_CD AS DLRCD ")
            .AppendLine("      , T102.BRN_CD AS STRCD ")
            .AppendLine("      , T102.SALES_ID AS FLLWUPBOX_SEQNO ")
            .AppendLine("      , T103.CST_ID AS INSDID ")
            .AppendLine("      , T103.CST_TYPE AS CUSTSEGMENT ")
            .AppendLine("      , T101.REQ_STATUS AS CRACTRESULT ")
            .AppendLine("      , CASE ")
            .AppendLine("             WHEN T101.REQ_STATUS = '31' THEN 400 ")
            .AppendLine("             WHEN T101.REQ_STATUS = '32' THEN 500 ")
            .AppendLine("             WHEN T102.SALES_PROSPECT_CD = '30' THEN 100 ")
            .AppendLine("             WHEN T102.SALES_PROSPECT_CD = '20' THEN 200 ")
            .AppendLine("             ELSE 300 ")
            .AppendLine("        END AS CRRESULTSORT ")
            .AppendLine("      , '1' AS CUSTOMERCLASS ")
            .AppendLine("   FROM TB_H_REQUEST T101 ")
            .AppendLine("      , TB_H_SALES T102 ")
            .AppendLine("      , TB_M_CUSTOMER_DLR T103 ")
            .AppendLine("  WHERE T101.REQ_ID = T102.REQ_ID ")
            .AppendLine("    AND T102.DLR_CD = T103.DLR_CD ")
            .AppendLine("    AND T103.CST_ID = :CST_ID ")
            .AppendLine("    AND T102.SALES_ID = :FLLWUPBOX_SEQNO ")
        End With
        Using query As New DBSelectQuery(Of SC3010401DataSet.SC3010401GetMainDataTable)("SC3010401_012")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, salesId)      '商談ID
            query.AddParameterWithTypeValue("CST_ID", OracleDbType.Decimal, cstId)                 '顧客ID

            Dim dt As SC3010401DataSet.SC3010401GetMainDataTable = query.GetData()

            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "{0}_End, Return RowCount:[{1}]",
                                      MethodBase.GetCurrentMethod.Name, dt.Rows.Count))
            ' ======================== ログ出力 終了 ========================

            Return dt
        End Using

    End Function

    ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 主情報（車両なし誘致）取得
    ''' </summary>
    ''' <param name="salesID">商談ID</param>
    ''' <param name="cstId">顧客ID</param>
    ''' <returns>SC3010401DataSet.SC3010401GetMainDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetMainAttractNoVcl(ByVal salesId As Decimal,
      ByVal cstId As Decimal) As SC3010401DataSet.SC3010401GetMainDataTable

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0}_Start, salesId:[{1}], cstId:[{2}]",
                                  MethodBase.GetCurrentMethod.Name,
                                  salesId, cstId))
        ' ======================== ログ出力 終了 ========================

        Dim sql As New StringBuilder
        With sql
            .AppendLine(" SELECT /* SC3010401_013 */ ")
            .AppendLine("        T2.DLR_CD AS DLRCD ")
            .AppendLine("      , T2.BRN_CD AS STRCD ")
            .AppendLine("      , T2.SALES_ID AS FLLWUPBOX_SEQNO ")
            .AppendLine("      , T3.CST_ID AS INSDID ")
            .AppendLine("      , T3.CST_TYPE AS CUSTSEGMENT ")
            .AppendLine("      , T1.CONTINUE_ACT_STATUS AS CRACTRESULT ")
            .AppendLine("      , CASE ")
            .AppendLine("             WHEN T1.CONTINUE_ACT_STATUS = '31' THEN 400 ")
            .AppendLine("             WHEN T1.CONTINUE_ACT_STATUS = '32' THEN 500 ")
            .AppendLine("             WHEN T2.SALES_PROSPECT_CD = '30' THEN 100 ")
            .AppendLine("             WHEN T2.SALES_PROSPECT_CD = '20' THEN 200 ")
            .AppendLine("             ELSE 300 ")
            .AppendLine("        END AS CRRESULTSORT ")
            .AppendLine("      , '1' AS CUSTOMERCLASS ")
            .AppendLine("   FROM TB_T_ATTRACT T1 ")
            .AppendLine("      , TB_T_SALES T2 ")
            .AppendLine("      , TB_M_CUSTOMER_DLR T3 ")
            .AppendLine("  WHERE T1.ATT_ID = T2.ATT_ID ")
            .AppendLine("    AND T2.DLR_CD = T3.DLR_CD ")
            .AppendLine("    AND T3.CST_ID = :CST_ID ")
            .AppendLine("    AND T2.SALES_ID = :FLLWUPBOX_SEQNO ")
            .AppendLine("  UNION ALL ")
            .AppendLine(" SELECT T102.DLR_CD AS DLRCD ")
            .AppendLine("      , T102.BRN_CD AS STRCD ")
            .AppendLine("      , T102.SALES_ID AS FLLWUPBOX_SEQNO ")
            .AppendLine("      , T103.CST_ID AS INSDID ")
            .AppendLine("      , T103.CST_TYPE AS CUSTSEGMENT ")
            .AppendLine("      , T101.CONTINUE_ACT_STATUS AS CRACTRESULT ")
            .AppendLine("      , CASE ")
            .AppendLine("             WHEN T101.CONTINUE_ACT_STATUS = '31' THEN 400 ")
            .AppendLine("             WHEN T101.CONTINUE_ACT_STATUS = '32' THEN 500 ")
            .AppendLine("             WHEN T102.SALES_PROSPECT_CD = '30' THEN 100 ")
            .AppendLine("             WHEN T102.SALES_PROSPECT_CD = '20' THEN 200 ")
            .AppendLine("             ELSE 300 ")
            .AppendLine("        END AS CRRESULTSORT ")
            .AppendLine("      , '1' AS CUSTOMERCLASS ")
            .AppendLine("   FROM TB_H_ATTRACT T101 ")
            .AppendLine("      , TB_H_SALES T102 ")
            .AppendLine("      , TB_M_CUSTOMER_DLR T103 ")
            .AppendLine("  WHERE T101.ATT_ID = T102.ATT_ID ")
            .AppendLine("    AND T102.DLR_CD = T103.DLR_CD ")
            .AppendLine("    AND T103.CST_ID = :CST_ID ")
            .AppendLine("    AND T102.SALES_ID = :FLLWUPBOX_SEQNO ")
        End With
        Using query As New DBSelectQuery(Of SC3010401DataSet.SC3010401GetMainDataTable)("SC3010401_013")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, salesId)      '商談ID
            query.AddParameterWithTypeValue("CST_ID", OracleDbType.Decimal, cstId)                 '顧客ID

            Dim dt As SC3010401DataSet.SC3010401GetMainDataTable = query.GetData()

            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "{0}_End, Return RowCount:[{1}]",
                                      MethodBase.GetCurrentMethod.Name, dt.Rows.Count))
            ' ======================== ログ出力 終了 ========================

            Return dt
        End Using

    End Function
    ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END

    ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 主情報（商談情報）取得
    ''' </summary>
    ''' <param name="salesID">商談ID</param>
    ''' <returns>SC3010401DataSet.SC3010401GetMainDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetMainSale(ByVal salesId As Decimal) As SC3010401DataSet.SC3010401GetMainSaleDataTable

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0}_Start, salesId:{1}",
                                  MethodBase.GetCurrentMethod.Name,
                                  salesId))
        ' ======================== ログ出力 終了 ========================

        Dim sql As New StringBuilder
        With sql
            .AppendLine(" SELECT /* SC3010401_014 */ ")
            .AppendLine("        T1.REQ_ID ")
            .AppendLine("      , T1.ATT_ID ")
            .AppendLine("      , T1.CST_ID ")
            .AppendLine("      , NVL(NVL(T2.VCL_ID, T3.VCL_ID), 0) AS VCL_ID ")
            .AppendLine("   FROM TB_T_SALES T1 ")
            .AppendLine("      , TB_T_REQUEST T2 ")
            .AppendLine("      , TB_T_ATTRACT T3 ")
            .AppendLine("  WHERE T1.REQ_ID = T2.REQ_ID(+) ")
            .AppendLine("    AND T1.ATT_ID = T3.ATT_ID(+) ")
            .AppendLine("    AND T2.REQ_ID(+) <> 0 ")
            .AppendLine("    AND T3.ATT_ID(+) <> 0 ")
            .AppendLine("    AND T1.SALES_ID = :FLLWUPBOX_SEQNO ")
            .AppendLine("  UNION ALL ")
            .AppendLine(" SELECT T101.REQ_ID ")
            .AppendLine("      , T101.ATT_ID ")
            .AppendLine("      , T101.CST_ID ")
            .AppendLine("      , NVL(NVL(T102.VCL_ID, T103.VCL_ID), 0) AS VCL_ID ")
            .AppendLine("   FROM TB_H_SALES T101 ")
            .AppendLine("      , TB_H_REQUEST T102 ")
            .AppendLine("      , TB_H_ATTRACT T103 ")
            .AppendLine("  WHERE T101.REQ_ID = T102.REQ_ID(+) ")
            .AppendLine("    AND T101.ATT_ID = T103.ATT_ID(+) ")
            .AppendLine("    AND T102.REQ_ID(+) <> 0 ")
            .AppendLine("    AND T103.ATT_ID(+) <> 0 ")
            .AppendLine("    AND T101.SALES_ID = :FLLWUPBOX_SEQNO ")
        End With
        Using query As New DBSelectQuery(Of SC3010401DataSet.SC3010401GetMainSaleDataTable)("SC3010401_014")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, salesId)      '商談ID

            Dim dt As SC3010401DataSet.SC3010401GetMainSaleDataTable = query.GetData()

            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "{0}_End, Return RowCount:[{1}]",
                                      MethodBase.GetCurrentMethod.Name, dt.Rows.Count))
            ' ======================== ログ出力 終了 ========================

            Return dt
        End Using

    End Function
    ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END

    ' 2014/02/17 TCS 山田 受注後フォロー機能開発 START
    ''' <summary>
    ''' 顧客検索条件項目取得
    ''' </summary>
    ''' <returns>SC3010401DataSet.SC3010401GetCstSearchCondDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetCstSearchCond() As SC3010401DataSet.SC3010401GetCstSearchCondDataTable

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0}_Start",
                                  MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        Dim sql As New StringBuilder
        With sql
            .AppendLine(" SELECT /* SC3010401_015 */ ")
            .AppendLine("        T1.CST_SEARCH_COND_CD, ")
            .AppendLine("        CASE T2.WORD_VAL ")
            .AppendLine("        WHEN N' ' THEN T2.WORD_VAL_ENG ")
            .AppendLine("        ELSE T2.WORD_VAL ")
            .AppendLine("        END AS WORD_VAL ")
            .AppendLine("   FROM TB_M_CST_SEARCH_COND T1 ")
            .AppendLine("      , TB_M_WORD T2 ")
            .AppendLine("  WHERE T1.CST_SEARCH_COND_NAME = T2.WORD_CD(+) ")
            .AppendLine("  ORDER BY T1.SORT_ORDER ")
        End With
        Using query As New DBSelectQuery(Of SC3010401DataSet.SC3010401GetCstSearchCondDataTable)("SC3010401_015")
            query.CommandText = sql.ToString()

            Dim dt As SC3010401DataSet.SC3010401GetCstSearchCondDataTable = query.GetData()

            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "{0}_End, Return RowCount:[{1}]",
                                      MethodBase.GetCurrentMethod.Name, dt.Rows.Count))
            ' ======================== ログ出力 終了 ========================

            Return dt
        End Using

    End Function

    ''' <summary>
    ''' 受注後工程アイコンパス取得
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="secondKey">セカンドキー</param>
    ''' <returns>SC3010401GetAfterOdrProcIconPathDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetAfterOdrProcIconPath(ByVal dlrcd As String, ByVal secondKey As String) As SC3010401DataSet.SC3010401GetAfterOdrProcIconPathDataTable
        Using query As New DBSelectQuery(Of SC3010401DataSet.SC3010401GetAfterOdrProcIconPathDataTable)("SC3010401_016")

            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "{0}_Start, dlrcd:[{1}]",
                                      MethodBase.GetCurrentMethod.Name,
                                      dlrcd))
            ' ======================== ログ出力 終了 ========================

            Dim sql As New StringBuilder
            With sql
                .AppendLine("SELECT /* SC3010401_016 */ ")
                .AppendLine("       T1.AFTER_ODR_PRCS_CD, ")
                .AppendLine("       NVL(T100.ICON_PATH, T200.ICON_PATH) AS ICON_PATH ")
                .AppendLine("  FROM TB_M_AFTER_ODR_PROC T1 , ")
                .AppendLine("       (SELECT T101.FIRST_KEY, T101.ICON_PATH  ")
                .AppendLine("          FROM TB_M_IMG_PATH_CONTROL T101 ")
                .AppendLine("         WHERE T101.DLR_CD = :DLR_CD ")
                .AppendLine("           AND T101.TYPE_CD = 'AFTER_ODR_PRCS_CD' ")
                .AppendLine("           AND T101.DEVICE_TYPE = '01' ")
                .AppendLine("           AND T101.SECOND_KEY = :SECOND_KEY) T100,  ")
                .AppendLine("       (SELECT T201.FIRST_KEY, T201.ICON_PATH  ")
                .AppendLine("          FROM TB_M_IMG_PATH_CONTROL T201 ")
                .AppendLine("         WHERE T201.DLR_CD = 'XXXXX' ")
                .AppendLine("           AND T201.TYPE_CD = 'AFTER_ODR_PRCS_CD' ")
                .AppendLine("           AND T201.DEVICE_TYPE = '01' ")
                .AppendLine("           AND T201.SECOND_KEY = :SECOND_KEY) T200 ")
                .AppendLine(" WHERE T1.AFTER_ODR_PRCS_CD = T100.FIRST_KEY(+) ")
                .AppendLine("   AND T1.AFTER_ODR_PRCS_CD = T200.FIRST_KEY(+) ")
                .AppendLine(" ORDER BY T1.SORT_ORDER ")
            End With
            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dlrcd)      '販売店コード
            query.AddParameterWithTypeValue("SECOND_KEY", OracleDbType.NVarchar2, secondKey)      'セカンドキー

            ' SQLを実行
            Dim dt As SC3010401DataSet.SC3010401GetAfterOdrProcIconPathDataTable = query.GetData()

            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "{0}_End, Return RowCount:[{1}]",
                                      MethodBase.GetCurrentMethod.Name, dt.Rows.Count))
            ' ======================== ログ出力 終了 ========================

            Return dt
        End Using
    End Function

    ''' <summary>
    ''' 顧客一覧作成
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="searchDirection">検索方向</param>
    ''' <param name="searchValue">検索文字列</param>
    ''' <param name="searchType">検索項目</param>
    ''' <returns>SC3010401GetCustomerListDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetCustomerList(ByVal dlrcd As String, ByVal searchDirection As Integer,
      ByVal searchValue As String, ByVal searchType As String, ByVal account As String) As SC3010401DataSet.SC3010401GetCustomerListDataTable
        Using query As New DBSelectQuery(Of SC3010401DataSet.SC3010401GetCustomerListDataTable)("SC3010401_017")

            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "{0}_Start, dlrcd:[{1}], searchDirection:[{2}], searchValue:[{3}], searchType:[{4}]",
                                      MethodBase.GetCurrentMethod.Name, dlrcd, searchDirection, searchValue, searchType))
            ' ======================== ログ出力 終了 ========================

            Dim sql As New StringBuilder
            With sql
                .AppendLine(" SELECT /* SC3010401_017 */  ")
                .AppendLine("     T1.CST_ID AS CRCUSTID   ")
                .AppendLine(" FROM  ")
                .AppendLine("    TB_M_CUSTOMER T1  ")
                .AppendLine("    INNER JOIN TB_M_CUSTOMER_DLR T2  ")
                .AppendLine("     ON T1.CST_ID = T2.CST_ID  ")
                .AppendLine("    INNER JOIN TB_M_CUSTOMER_VCL T3  ")
                .AppendLine("     ON T2.DLR_CD = T3.DLR_CD  ")
                .AppendLine("     AND T2.CST_ID = T3.CST_ID  ")
                .AppendLine("    LEFT JOIN TB_M_VEHICLE T4  ")
                .AppendLine("     ON T3.VCL_ID = T4.VCL_ID  ")
                .AppendLine(" WHERE  ")
                If (searchDirection = IdSearchDirectionAfter) Then
                    '前方一致
                    '1: 顧客名称、2: 電話番号/携帯番号、3: 国民ID、5: VIN
                    Select Case searchType
                        Case IdSearchName      '顧客名称
                            .Append("        T1.CST_NAME_SEARCH like :NAME || '%' AND ")
                        Case IdSearchTel       '電話番号/携帯番号
                            .Append("       (T1.CST_PHONE_SEARCH like :TELNO || '%' ")
                            .Append("    OR  T1.CST_MOBILE_SEARCH like :TELNO || '%') AND ")
                        Case IdSearchSocialNum '国民ID
                            .Append("        T1.CST_SOCIALNUM_SEARCH like :SOCIALNUM || '%' AND ")
                        Case IdSearchVin       'VIN
                            .Append("        T4.VCL_VIN_SEARCH like :VIN || '%' AND ")
                        Case Else
                    End Select
                Else
                    'あいまい検索
                    '1: 顧客名称、2: 電話番号/携帯番号、3: 国民ID、5: VIN
                    Select Case searchType
                        Case IdSearchName      '顧客名称
                            .Append("        T1.CST_NAME_SEARCH like '%' || :NAME || '%' AND ")
                        Case IdSearchTel       '電話番号/携帯番号
                            .Append("       (T1.CST_PHONE_SEARCH like '%' || :TELNO || '%' ")
                            .Append("    OR  T1.CST_MOBILE_SEARCH like '%' || :TELNO || '%') AND ")
                        Case IdSearchSocialNum '国民ID
                            .Append("        T1.CST_SOCIALNUM_SEARCH like '%' || :SOCIALNUM || '%' AND ")
                        Case IdSearchVin       'VIN
                            .Append("        T4.VCL_VIN_SEARCH like '%' || :VIN || '%' AND ")
                        Case Else
                    End Select
                End If
                .AppendLine("     ((T2.CST_TYPE ='1' AND  ")
                .AppendLine("     Trim(T4.VCL_VIN) IS NOT NULL ) OR  ")
                .AppendLine("     (T2.CST_TYPE ='2')) AND  ")
                .AppendLine("     T3.OWNER_CHG_FLG = '0' AND  ")
                .AppendLine("     T3.CST_VCL_TYPE = '1' AND")
                .AppendLine("     T2.DLR_CD = :DLRCD AND  ")
                .AppendLine("     T3.SLS_PIC_STF_CD = :STAFFCD  ")
            End With

            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)          '販売店コード

            '1: 顧客名称、2: 電話番号/携帯番号、3: 国民ID、5: VIN
            Select Case searchType
                Case IdSearchName      '顧客名称
                    query.AddParameterWithTypeValue("NAME", OracleDbType.NVarchar2, searchValue)
                Case IdSearchTel       '電話番号/携帯番号
                    query.AddParameterWithTypeValue("TELNO", OracleDbType.NVarchar2, searchValue)
                Case IdSearchSocialNum '国民ID
                    query.AddParameterWithTypeValue("SOCIALNUM", OracleDbType.NVarchar2, searchValue)
                Case IdSearchVin       'VIN
                    query.AddParameterWithTypeValue("VIN", OracleDbType.NVarchar2, searchValue)
                Case Else
            End Select

            query.AddParameterWithTypeValue("STAFFCD", OracleDbType.NVarchar2, account)

            ' SQLを実行
            Dim dt As SC3010401DataSet.SC3010401GetCustomerListDataTable = query.GetData()

            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "{0}_End, Return RowCount:[{1}]",
                                      MethodBase.GetCurrentMethod.Name, dt.Rows.Count))
            ' ======================== ログ出力 終了 ========================

            Return dt
        End Using
    End Function
    ' 2014/02/17 TCS 山田 受注後フォロー機能開発 END

    '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 DEL

End Class
