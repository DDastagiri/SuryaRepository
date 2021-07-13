'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3240601DataSet.vb
'─────────────────────────────────────
'機能： WarningMileage データセット
'補足： 
'作成： 2014/06/24 TMEJ 陳 IT9678_タブレット版SMB（テレマ走行距離機能開発）
'更新： 
'─────────────────────────────────────

Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web
Imports System.Globalization
Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SMB.Telema.DataAccess.SC3240601DataSet
Imports System.Reflection


Namespace SC3240601DataSetTableAdapters
    Public Class SC3240601DataTableAdapter
        Inherits Global.System.ComponentModel.Component

#Region "定数"

        ''' <summary>
        ''' 所有者フラグ1:自社客 
        ''' </summary>
        ''' <remarks></remarks>
        Private Const CST_VCL_TYPE_1 As String = "1"
        ''' <summary>
        ''' Deleteフラグ 0:削除されない
        ''' </summary>
        ''' <remarks></remarks>
        Private Const DELFLG_0 As String = "0"
        ''' <summary>
        ''' オリジナルフラグ1:代表走行距離履歴
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ORIGINALFLG_1 As String = "1"
        ''' <summary>
        ''' 使用フラグ1:使用する
        ''' </summary>
        ''' <remarks></remarks>
        Private Const INUSE_FLG_1 As String = "1"
        ''' <summary>
        ''' 登録方法1:基幹入庫履歴
        ''' </summary>
        ''' <remarks></remarks>
        Private Const REG_MTD_1 As String = "1"
        ''' <summary>
        ''' 登録方法2:サイト入力
        ''' </summary>
        ''' <remarks></remarks>
        Private Const REG_MTD_2 As String = "2"
        ''' <summary>
        ''' 登録方法3:走行距離アンケート
        ''' </summary>
        ''' <remarks></remarks>
        Private Const REG_MTD_3 As String = "3"
        ''' <summary>
        ''' 登録方法4:コールセンター入力
        ''' </summary>
        ''' <remarks></remarks>
        Private Const REG_MTD_4 As String = "4"
#End Region

#Region "メイン"

        ''' <summary>
        ''' SC3240601_001：走行履歴一覧Warning情報を取得
        ''' </summary>
        ''' <param name="inOwnerId">オーナーズID</param>
        ''' <param name="inVin">VIN</param>
        ''' <param name="inOccurdate">発生日時</param>
        ''' <param name="inReceiveSeq">受信連番</param>
        ''' <returns>走行履歴一覧DataTable</returns>
        ''' <remarks></remarks>
        Public Function GetMileageWarningList(ByVal inOwnerId As String, _
                                              ByVal inVin As String, _
                                              ByVal inOccurdate As Date, _
                                              ByVal inReceiveSeq As Long) As SC3240601TelemaInfoDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} START inOwnerId:{2};inVin:{3};inOccurdate:{4};inReceiveSeq:{5};" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , inOwnerId _
                        , inVin _
                        , inOccurdate _
                        , inReceiveSeq))

            'データ格納用
            Dim dt As SC3240601TelemaInfoDataTable

            Dim sql As New StringBuilder

            'SQL文作成
            With sql
                .AppendLine(" SELECT /* SC3240601_001 */ ")
                .AppendLine("        0 AS VCL_MILE_ID ")
                .AppendLine("       ,40 AS MARK_SORT ")
                .AppendLine("       ,NULL AS DLR_CD ")
                .AppendLine("       ,0 AS VCL_ID ")
                .AppendLine("       ,0 AS CST_ID ")
                .AppendLine("       ,N' ' AS CST_NAME ")
                .AppendLine("       ,TO_DATE(TO_CHAR(T4.OCCURDATE, 'YYYY/MM/DD'), 'YYYY/MM/DD') AS REG_DATE ")
                .AppendLine("       ,T4.MILEAGE AS REG_MILE ")
                .AppendLine("       ,N'0' AS REG_MTD ")
                .AppendLine("       ,NULL AS REG_STF_CD ")
                .AppendLine("       ,NULL AS STF_NAME ")
                .AppendLine("       ,NULL AS DMS_TAKEIN_DATETIME ")
                .AppendLine("       ,T4.OWNERS_ID ")
                .AppendLine("       ,T4.VIN ")
                .AppendLine("       ,T4.RECEIVESEQ ")
                .AppendLine("       ,T4.SEQNO ")
                .AppendLine("       ,T4.OCCURDATE ")
                .AppendLine("       ,T4.WARNINGCODE ")
                .AppendLine("       ,T4.CREATEDATE ")
                .AppendLine("       ,T4.UPDATEDATE ")
                .AppendLine("       ,NULL AS SVC_NAME_MILE ")
                .AppendLine("   FROM ")
                .AppendLine("       (SELECT ")
                .AppendLine("               T1.OWNERS_ID ")
                .AppendLine("              ,T1.VIN ")
                .AppendLine("              ,T1.RECEIVESEQ ")
                .AppendLine("              ,T2.SEQNO ")
                .AppendLine("              ,T1.MILEAGE ")
                .AppendLine("              ,T1.OCCURDATE ")
                .AppendLine("              ,T2.WARNINGCODE ")
                .AppendLine("              ,T1.CREATEDATE ")
                .AppendLine("              ,T1.UPDATEDATE ")
                .AppendLine("              ,RTRIM(T1.VIN) AS VIN_RTRIM ")
                .AppendLine("          FROM ")
                .AppendLine("               TBL_TLM_WARNING T1 ")
                .AppendLine("              ,TBL_TLM_WARNING_DETAIL T2 ")
                .AppendLine("         WHERE ")
                .AppendLine("               T1.OWNERS_ID = T2.OWNERS_ID ")
                .AppendLine("           AND T1.VIN = T2.VIN ")
                .AppendLine("           AND T1.RECEIVESEQ = T2.RECEIVESEQ ")
                .AppendLine("       ) T4 ")
                .AppendLine("  WHERE ")
                .AppendLine("        T4.VIN_RTRIM = :VIN ")
                .AppendLine("    AND T4.OWNERS_ID = :OWNERS_ID ")
                .AppendLine("    AND T4.OCCURDATE = :OCCURDATE ")
                .AppendLine("    AND T4.RECEIVESEQ = :RECEIVESEQ ")
                .AppendLine("  ORDER BY T4.SEQNO ASC ")

            End With

            Using query As New DBSelectQuery(Of SC3240601TelemaInfoDataTable)("SC3240601_001")
                query.CommandText = sql.ToString()

                'バインド変数
                query.AddParameterWithTypeValue("VIN", OracleDbType.NVarchar2, inVin)
                query.AddParameterWithTypeValue("OWNERS_ID", OracleDbType.Char, inOwnerId)
                query.AddParameterWithTypeValue("OCCURDATE", OracleDbType.Date, inOccurdate)
                query.AddParameterWithTypeValue("RECEIVESEQ", OracleDbType.Long, inReceiveSeq)

                'データ取得
                dt = query.GetData()

            End Using

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END OUT:COUNT = {2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , dt.Rows.Count.ToString(CultureInfo.CurrentCulture)))
            Return dt

        End Function

        ''' <summary>
        ''' SC3240601_008：走行履歴一覧取得
        ''' </summary>
        ''' <param name="inVclId">車両ID</param>
        ''' <param name="inVin">VIN</param>
        ''' <param name="inOwnersId">オーナーズID</param>
        ''' <param name="inOccurdate">発生日時</param>
        ''' <param name="inStartIndex">検索開始Index</param>
        ''' <param name="inEndIndex">検索終了Index</param>
        ''' <param name="inTelemaDisplayCount">GBOOK表示件数</param>
        ''' <returns>走行履歴一覧DataTable</returns>
        ''' <remarks></remarks>
        Public Function GetMileageList(ByVal inVclId As Decimal, _
                                       ByVal inVin As String, _
                                       ByVal inOwnersId As String, _
                                       ByVal inOccurdate As Date, _
                                       ByVal inStartIndex As Long, _
                                       ByVal inEndIndex As Long, _
                                       ByVal inTelemaDisplayCount As Long) As SC3240601TelemaInfoDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} START inVclId:{2};inVin:{3};inOwnersId:{4};inOccurdate:{5};inStartIndex:{6};inEndIndex:{7};inTelemaDispCount:{8};" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , inVclId _
                        , inVin _
                        , inOwnersId _
                        , inOccurdate _
                        , inStartIndex _
                        , inEndIndex _
                        , inTelemaDisplayCount))

            'データ格納用
            Dim dt As SC3240601TelemaInfoDataTable

            Dim sql As New StringBuilder

            'SQL文作成
            With sql
                .AppendLine(" SELECT /* SC3240601_008 */ ")
                .AppendLine("        A1.NO ")
                .AppendLine("       ,A1.VCL_MILE_ID ")
                .AppendLine("       ,A1.MARK_SORT ")
                .AppendLine("       ,A1.DLR_CD ")
                .AppendLine("       ,A1.BRN_CD ")
                .AppendLine("       ,A1.VCL_ID ")
                .AppendLine("       ,A1.CST_ID ")
                .AppendLine("       ,A1.CST_NAME ")
                .AppendLine("       ,A1.REG_DATE ")
                .AppendLine("       ,A1.REG_MILE ")
                .AppendLine("       ,A1.REG_MTD ")
                .AppendLine("       ,A1.REG_STF_CD ")
                .AppendLine("       ,A1.STF_NAME ")
                .AppendLine("       ,A1.DMS_TAKEIN_DATETIME ")
                .AppendLine("       ,A1.OWNERS_ID ")
                .AppendLine("       ,A1.VIN ")
                .AppendLine("       ,A1.RECEIVESEQ ")
                .AppendLine("       ,A1.OCCURDATE ")
                .AppendLine("       ,A1.CREATEDATE ")
                .AppendLine("       ,A1.UPDATEDATE ")
                .AppendLine("       ,A1.SVC_NAME_MILE ")
                .AppendLine("   FROM ")
                .AppendLine("       (SELECT ")
                .AppendLine("               ROWNUM AS NO ")
                .AppendLine("              ,N1.VCL_MILE_ID ")
                .AppendLine("              ,N1.MARK_SORT ")
                .AppendLine("              ,N1.DLR_CD ")
                .AppendLine("              ,N1.BRN_CD ")
                .AppendLine("              ,N1.VCL_ID ")
                .AppendLine("              ,N1.CST_ID ")
                .AppendLine("              ,N1.CST_NAME ")
                .AppendLine("              ,N1.REG_DATE ")
                .AppendLine("              ,N1.REG_MILE ")
                .AppendLine("              ,N1.REG_MTD ")
                .AppendLine("              ,N1.REG_STF_CD ")
                .AppendLine("              ,N1.STF_NAME ")
                .AppendLine("              ,N1.DMS_TAKEIN_DATETIME ")
                .AppendLine("              ,N1.OWNERS_ID ")
                .AppendLine("              ,N1.VIN ")
                .AppendLine("              ,N1.RECEIVESEQ ")
                .AppendLine("              ,N1.OCCURDATE ")
                .AppendLine("              ,N1.CREATEDATE ")
                .AppendLine("              ,N1.UPDATEDATE ")
                .AppendLine("              ,N1.SVC_NAME_MILE ")
                .AppendLine("          FROM ")
                .AppendLine("              (SELECT ")
                .AppendLine("                      M1.VCL_MILE_ID ")
                .AppendLine("                     ,M1.MARK_SORT ")
                .AppendLine("                     ,M1.DLR_CD ")
                .AppendLine("                     ,M1.BRN_CD ")
                .AppendLine("                     ,M1.VCL_ID ")
                .AppendLine("                     ,M1.CST_ID ")
                .AppendLine("                     ,M1.CST_NAME ")
                .AppendLine("                     ,M1.REG_DATE ")
                .AppendLine("                     ,M1.REG_MILE ")
                .AppendLine("                     ,M1.REG_MTD ")
                .AppendLine("                     ,M1.REG_STF_CD ")
                .AppendLine("                     ,M1.STF_NAME ")
                .AppendLine("                     ,M1.DMS_TAKEIN_DATETIME ")
                .AppendLine("                     ,M1.OWNERS_ID ")
                .AppendLine("                     ,M1.VIN ")
                .AppendLine("                     ,M1.RECEIVESEQ ")
                .AppendLine("                     ,M1.OCCURDATE ")
                .AppendLine("                     ,M1.CREATEDATE ")
                .AppendLine("                     ,M1.UPDATEDATE ")
                .AppendLine("                     ,M1.SVC_NAME_MILE ")
                .AppendLine("                 FROM ")
                .AppendLine("                     (SELECT ")
                .AppendLine("                             T1.VCL_MILE_ID ")
                .AppendLine("                            ,CASE WHEN T1.REG_MTD = 2 OR T1.REG_MTD = 3 OR T1.REG_MTD = 4 THEN 20 ")
                .AppendLine("                                  WHEN T1.REG_MTD = 1 THEN 30 ELSE 0 END  MARK_SORT ")
                .AppendLine("                            ,T1.DLR_CD ")
                .AppendLine("                            ,T102.PIC_BRN_CD AS BRN_CD ")
                .AppendLine("                            ,T1.VCL_ID ")
                .AppendLine("                            ,T1.CST_ID ")
                .AppendLine("                            ,T101.CST_NAME ")
                .AppendLine("                            ,TO_DATE(TO_CHAR(T1.REG_DATE, 'YYYY/MM/DD'), 'YYYY/MM/DD') AS REG_DATE ")
                .AppendLine("                            ,T1.REG_MILE ")
                .AppendLine("                            ,T1.REG_MTD ")
                .AppendLine("                            ,T1.REG_STF_CD ")
                .AppendLine("                            ,T104.STF_NAME ")
                .AppendLine("                            ,T1.DMS_TAKEIN_DATETIME AS DMS_TAKEIN_DATETIME ")
                .AppendLine("                            ,' ' AS OWNERS_ID ")
                .AppendLine("                            ,' ' AS VIN ")
                .AppendLine("                            ,0 AS RECEIVESEQ ")
                .AppendLine("                            ,NULL AS OCCURDATE ")
                .AppendLine("                            ,NULL AS CREATEDATE ")
                .AppendLine("                            ,NULL AS UPDATEDATE ")
                .AppendLine("                            ,T103.SVC_NAME_MILE AS SVC_NAME_MILE ")
                .AppendLine("                        FROM ")
                .AppendLine("                             TB_T_VEHICLE_MILEAGE T1 ")
                .AppendLine("                            ,TB_M_CUSTOMER T101 ")
                .AppendLine("                            ,TB_T_VEHICLE_SVCIN_HIS T102 ")
                .AppendLine("                            ,TB_M_SERVICE T103 ")
                .AppendLine("                            ,TB_M_STAFF T104 ")
                .AppendLine("                       WHERE ")
                .AppendLine("                             T1.CST_ID = T101.CST_ID(+) ")
                .AppendLine("                         AND T1.VCL_MILE_ID = T102.VCL_MILE_ID(+) ")
                .AppendLine("                         AND T1.REG_STF_CD = T104.STF_CD(+) ")
                .AppendLine("                         AND T102.DLR_CD = T103.DLR_CD(+) ")
                .AppendLine("                         AND T102.SVC_CD = T103.SVC_CD(+) ")
                .AppendLine("                         AND T1.VCL_ID = :VCL_ID ")
                .AppendLine("                         AND T1.REG_MTD IN (:REG_MTD_1, :REG_MTD_2, :REG_MTD_3, :REG_MTD_4) ")
                .AppendLine("                   UNION ALL ")
                .AppendLine("                      SELECT ")
                .AppendLine("                             0 AS VCL_MILE_ID ")
                .AppendLine("                            ,40 AS MARK_SORT ")
                .AppendLine("                            ,NULL AS DLR_CD ")
                .AppendLine("                            ,NULL AS BRN_CD ")
                .AppendLine("                            ,0 AS VCL_ID ")
                .AppendLine("                            ,0 AS CST_ID ")
                .AppendLine("                            ,N' ' AS CST_NAME ")
                .AppendLine("                            ,TO_DATE(TO_CHAR(T4.OCCURDATE, 'YYYY/MM/DD'), 'YYYY/MM/DD') AS REG_DATE ")
                .AppendLine("                            ,T4.MILEAGE AS REG_MILE ")
                .AppendLine("                            ,N'0' AS REG_MTD ")
                .AppendLine("                            ,NULL AS REG_STF_CD ")
                .AppendLine("                            ,NULL AS STF_NAME ")
                .AppendLine("                            ,NULL AS DMS_TAKEIN_DATETIME ")
                .AppendLine("                            ,T4.OWNERS_ID ")
                .AppendLine("                            ,T4.VIN ")
                .AppendLine("                            ,T4.RECEIVESEQ ")
                .AppendLine("                            ,T4.OCCURDATE ")
                .AppendLine("                            ,T4.CREATEDATE ")
                .AppendLine("                            ,T4.UPDATEDATE ")
                .AppendLine("                            ,NULL AS SVC_NAME_MILE ")
                .AppendLine("                        FROM ")
                .AppendLine("                            (SELECT ")
                .AppendLine("                                    T1.OWNERS_ID ")
                .AppendLine("                                   ,T1.VIN ")
                .AppendLine("                                   ,T1.RECEIVESEQ ")
                .AppendLine("                                   ,T2.SEQNO ")
                .AppendLine("                                   ,T1.MILEAGE ")
                .AppendLine("                                   ,T1.OCCURDATE ")
                .AppendLine("                                   ,T2.WARNINGCODE ")
                .AppendLine("                                   ,T1.CREATEDATE ")
                .AppendLine("                                   ,T1.UPDATEDATE ")
                .AppendLine("                                   ,RTRIM(T1.VIN) AS VIN_RTRIM ")
                .AppendLine("                               FROM ")
                .AppendLine("                                    TBL_TLM_WARNING T1 ")
                .AppendLine("                                   ,TBL_TLM_WARNING_DETAIL T2 ")
                .AppendLine("                              WHERE ")
                .AppendLine("                                    T1.OWNERS_ID = T2.OWNERS_ID ")
                .AppendLine("                                AND T1.VIN = T2.VIN ")
                .AppendLine("                                AND T1.RECEIVESEQ = T2.RECEIVESEQ ")
                .AppendLine("                            ) T4 ")
                .AppendLine("                       WHERE ")
                .AppendLine("                             T4.VIN_RTRIM = :VIN ")
                .AppendLine("                         AND T4.OWNERS_ID = :OWNERS_ID ")
                .AppendLine("                         AND T4.OCCURDATE >= TO_DATE(:OCCURDATE, 'YYYY/MM/DD HH24:MI:SS') ")
                .AppendLine("                    GROUP BY ")
                .AppendLine("                             T4.OCCURDATE ")
                .AppendLine("                            ,T4.MILEAGE ")
                .AppendLine("                            ,T4.OWNERS_ID ")
                .AppendLine("                            ,T4.VIN ")
                .AppendLine("                            ,T4.RECEIVESEQ ")
                .AppendLine("                            ,T4.OCCURDATE ")
                .AppendLine("                            ,T4.CREATEDATE ")
                .AppendLine("                            ,T4.UPDATEDATE ")
                .AppendLine("                   UNION ALL ")
                .AppendLine("                      SELECT ")
                .AppendLine("                             0 AS VCL_MILE_ID ")
                .AppendLine("                            ,10 AS MARK_SORT ")
                .AppendLine("                            ,CAST (T6.DLRCD AS NVARCHAR2(5)) AS DLR_CD ")
                .AppendLine("                            ,NULL AS BRN_CD ")
                .AppendLine("                            ,T6.VCL_ID ")
                .AppendLine("                            ,T6.CST_ID ")
                .AppendLine("                            ,N' ' AS CST_NAME ")
                .AppendLine("                            ,TO_DATE(TO_CHAR(T6.REGISTDATE, 'YYYY/MM/DD'), 'YYYY/MM/DD') AS REG_DATE ")
                .AppendLine("                            ,T6.MILEAGE AS REG_MILE ")
                .AppendLine("                            ,N'5' AS REG_MTD ")
                .AppendLine("                            ,NULL AS REG_STF_CD ")
                .AppendLine("                            ,NULL AS STF_NAME ")
                .AppendLine("                            ,NULL AS DMS_TAKEIN_DATETIME ")
                .AppendLine("                            ,T6.OWNERS_ID ")
                .AppendLine("                            ,T6.VIN ")
                .AppendLine("                            ,T6.MILEAGESEQ AS RECEIVESEQ ")
                .AppendLine("                            ,NULL AS OCCURDATE ")
                .AppendLine("                            ,NULL AS CREATEDATE ")
                .AppendLine("                            ,NULL AS UPDATEDATE ")
                .AppendLine("                            ,NULL AS SVC_NAME_MILE ")
                .AppendLine("                        FROM ")
                .AppendLine("                            (SELECT ")
                .AppendLine("                                    T5.DLRCD ")
                .AppendLine("                                   ,T5.VCL_ID ")
                .AppendLine("                                   ,T5.CST_ID ")
                .AppendLine("                                   ,T5.REGISTDATE ")
                .AppendLine("                                   ,T5.MILEAGE ")
                .AppendLine("                                   ,T5.OWNERS_ID ")
                .AppendLine("                                   ,T5.VIN ")
                .AppendLine("                                   ,T5.MILEAGESEQ ")
                .AppendLine("                               FROM ")
                .AppendLine("                                    TBL_TLM_MILEAGEHIS T5 ")
                .AppendLine("                              WHERE ")
                .AppendLine("                                    T5.VCL_ID = :VCL_ID ")
                .AppendLine("                                AND T5.ORIGINALFLG = :ORIGINALFLG ")
                .AppendLine("                                AND T5.DELFLG = :DELFLG ")
                .AppendLine("                              ORDER BY T5.REGISTDATE DESC ")
                .AppendLine("                            ) T6 ")
                .AppendLine("                       WHERE ROWNUM <= :TLM_DISP_COUNT ")
                .AppendLine("                     ) M1 ")
                .AppendLine("             ORDER BY ")
                .AppendLine("                      M1.REG_DATE DESC ")
                .AppendLine("                     ,M1.MARK_SORT DESC ")
                .AppendLine("                     ,M1.RECEIVESEQ DESC ")
                .AppendLine("              ) N1 ")
                .AppendLine("       ) A1 ")
                .AppendLine("  WHERE    A1.NO BETWEEN :STARTINDEX AND :ENDINDEX ")
                .AppendLine("  ORDER BY A1.NO ASC ")
            End With

            Using query As New DBSelectQuery(Of SC3240601TelemaInfoDataTable)("SC3240601_008")
                query.CommandText = sql.ToString()

                'バインド変数
                query.AddParameterWithTypeValue("VCL_ID", OracleDbType.Decimal, inVclId)
                query.AddParameterWithTypeValue("REG_MTD_1", OracleDbType.NVarchar2, REG_MTD_1)
                query.AddParameterWithTypeValue("REG_MTD_2", OracleDbType.NVarchar2, REG_MTD_2)
                query.AddParameterWithTypeValue("REG_MTD_3", OracleDbType.NVarchar2, REG_MTD_3)
                query.AddParameterWithTypeValue("REG_MTD_4", OracleDbType.NVarchar2, REG_MTD_4)
                query.AddParameterWithTypeValue("VIN", OracleDbType.NVarchar2, inVin)
                query.AddParameterWithTypeValue("OWNERS_ID", OracleDbType.Char, inOwnersId)
                query.AddParameterWithTypeValue("OCCURDATE", OracleDbType.Date, inOccurdate)
                query.AddParameterWithTypeValue("ORIGINALFLG", OracleDbType.Char, ORIGINALFLG_1)
                query.AddParameterWithTypeValue("DELFLG", OracleDbType.Char, DELFLG_0)
                query.AddParameterWithTypeValue("STARTINDEX", OracleDbType.Long, inStartIndex)
                query.AddParameterWithTypeValue("ENDINDEX", OracleDbType.Long, inEndIndex)
                query.AddParameterWithTypeValue("TLM_DISP_COUNT", OracleDbType.Long, inTelemaDisplayCount)

                'データ取得
                dt = query.GetData()

            End Using

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END OUT:COUNT = {2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , dt.Rows.Count.ToString(CultureInfo.CurrentCulture)))
            Return dt

        End Function

        ''' <summary>
        ''' SC3240601_006：走行履歴一覧件数取得
        ''' </summary>
        ''' <param name="inVclId">車両ID</param>
        ''' <param name="inVin">VIN</param>
        ''' <param name="inOwnersId">オーナーズID</param>
        ''' <param name="inOccurdate">発生日時</param>
        ''' <param name="inTelemaDisplayCount">GBOOK表示件数</param>
        ''' <returns>走行履歴一覧件数；-1の場合取得エラー発生</returns>
        ''' <remarks></remarks>
        Public Function GetMileageListCount(ByVal inVclId As Decimal, _
                                            ByVal inVin As String, _
                                            ByVal inOwnersId As String, _
                                            ByVal inOccurdate As Date, _
                                            ByVal inTelemaDisplayCount As Long) As SC3240601TelemaInfoDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} START inVclId:{2};inVin:{3};inOwnersId:{4};inOccurdate:{5};inTelemaDisplayCount:{6};" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , inVclId _
                        , inVin _
                        , inOwnersId _
                        , inOccurdate _
                        , inTelemaDisplayCount))

            'データ格納用
            Dim dt As SC3240601TelemaInfoDataTable

            Dim sql As New StringBuilder

            'SQL文作成
            With sql
                .AppendLine(" SELECT /* SC3240601_006 */ ")
                .AppendLine("        1 ")
                .AppendLine("   FROM ")
                .AppendLine("       (SELECT ")
                .AppendLine("               1 ")
                .AppendLine("          FROM ")
                .AppendLine("               TB_T_VEHICLE_MILEAGE T1 ")
                .AppendLine("              ,TB_M_CUSTOMER T101 ")
                .AppendLine("              ,TB_T_VEHICLE_SVCIN_HIS T102 ")
                .AppendLine("              ,TB_M_SERVICE T103 ")
                .AppendLine("              ,TB_M_STAFF T104 ")
                .AppendLine("         WHERE ")
                .AppendLine("               T1.CST_ID = T101.CST_ID(+) ")
                .AppendLine("           AND T1.VCL_MILE_ID = T102.VCL_MILE_ID(+) ")
                .AppendLine("           AND T1.REG_STF_CD = T104.STF_CD(+) ")
                .AppendLine("           AND T102.DLR_CD = T103.DLR_CD(+) ")
                .AppendLine("           AND T102.SVC_CD = T103.SVC_CD(+) ")
                .AppendLine("           AND T1.VCL_ID = :VCL_ID ")
                .AppendLine("           AND T1.REG_MTD IN (:REG_MTD_1, :REG_MTD_2, :REG_MTD_3, :REG_MTD_4) ")
                .AppendLine("     UNION ALL ")
                .AppendLine("        SELECT ")
                .AppendLine("               1 ")
                .AppendLine("         FROM ")
                .AppendLine("             (SELECT ")
                .AppendLine("                     T1.OWNERS_ID ")
                .AppendLine("                    ,T1.VIN ")
                .AppendLine("                    ,T1.RECEIVESEQ ")
                .AppendLine("                    ,T2.SEQNO ")
                .AppendLine("                    ,T1.MILEAGE ")
                .AppendLine("                    ,T1.OCCURDATE ")
                .AppendLine("                    ,T2.WARNINGCODE ")
                .AppendLine("                    ,T1.CREATEDATE ")
                .AppendLine("                    ,T1.UPDATEDATE ")
                .AppendLine("                    ,RTRIM(T1.VIN) AS VIN_RTRIM ")
                .AppendLine("                FROM ")
                .AppendLine("                     TBL_TLM_WARNING T1 ")
                .AppendLine("                    ,TBL_TLM_WARNING_DETAIL T2 ")
                .AppendLine("               WHERE ")
                .AppendLine("                     T1.OWNERS_ID = T2.OWNERS_ID ")
                .AppendLine("                 AND T1.VIN = T2.VIN ")
                .AppendLine("                 AND T1.RECEIVESEQ = T2.RECEIVESEQ ")
                .AppendLine("             ) T4 ")
                .AppendLine("        WHERE ")
                .AppendLine("              T4.VIN_RTRIM = :VIN ")
                .AppendLine("          AND T4.OWNERS_ID = :OWNERS_ID ")
                .AppendLine("          AND T4.OCCURDATE >= TO_DATE(:OCCURDATE, 'YYYY/MM/DD HH24:MI:SS') ")
                .AppendLine("     GROUP BY ")
                .AppendLine("              T4.OCCURDATE ")
                .AppendLine("             ,T4.MILEAGE ")
                .AppendLine("             ,T4.OWNERS_ID ")
                .AppendLine("             ,T4.VIN ")
                .AppendLine("             ,T4.RECEIVESEQ ")
                .AppendLine("             ,T4.OCCURDATE ")
                .AppendLine("             ,T4.CREATEDATE ")
                .AppendLine("             ,T4.UPDATEDATE ")
                .AppendLine("     UNION ALL ")
                .AppendLine("        SELECT ")
                .AppendLine("               1 ")
                .AppendLine("          FROM ")
                .AppendLine("              (SELECT ")
                .AppendLine("                      T5.DLRCD ")
                .AppendLine("                     ,T5.VCL_ID ")
                .AppendLine("                     ,T5.CST_ID ")
                .AppendLine("                     ,T5.REGISTDATE ")
                .AppendLine("                     ,T5.MILEAGE ")
                .AppendLine("                     ,T5.OWNERS_ID ")
                .AppendLine("                     ,T5.VIN ")
                .AppendLine("                     ,T5.MILEAGESEQ ")
                .AppendLine("                 FROM ")
                .AppendLine("                      TBL_TLM_MILEAGEHIS T5 ")
                .AppendLine("                WHERE ")
                .AppendLine("                      T5.VCL_ID = :VCL_ID ")
                .AppendLine("                  AND T5.ORIGINALFLG = :ORIGINALFLG ")
                .AppendLine("                  AND T5.DELFLG = :DELFLG ")
                .AppendLine("              ) T6 ")
                .AppendLine("         WHERE ROWNUM <= :TLM_DISP_COUNT ")
                .AppendLine("       ) M1 ")

            End With

            Using query As New DBSelectQuery(Of SC3240601TelemaInfoDataTable)("SC3240601_006")
                query.CommandText = sql.ToString()

                'バインド変数
                query.AddParameterWithTypeValue("VCL_ID", OracleDbType.Decimal, inVclId)
                query.AddParameterWithTypeValue("REG_MTD_1", OracleDbType.NVarchar2, REG_MTD_1)
                query.AddParameterWithTypeValue("REG_MTD_2", OracleDbType.NVarchar2, REG_MTD_2)
                query.AddParameterWithTypeValue("REG_MTD_3", OracleDbType.NVarchar2, REG_MTD_3)
                query.AddParameterWithTypeValue("REG_MTD_4", OracleDbType.NVarchar2, REG_MTD_4)
                query.AddParameterWithTypeValue("VIN", OracleDbType.NVarchar2, inVin)
                query.AddParameterWithTypeValue("OWNERS_ID", OracleDbType.Char, inOwnersId)
                query.AddParameterWithTypeValue("OCCURDATE", OracleDbType.Date, inOccurdate)
                query.AddParameterWithTypeValue("ORIGINALFLG", OracleDbType.Char, ORIGINALFLG_1)
                query.AddParameterWithTypeValue("DELFLG", OracleDbType.Char, DELFLG_0)
                query.AddParameterWithTypeValue("TLM_DISP_COUNT", OracleDbType.Long, inTelemaDisplayCount)

                'データ取得
                dt = query.GetData()

            End Using

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END OUT:COUNT = {2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , dt.Rows.Count.ToString(CultureInfo.CurrentCulture)))
            Return dt

        End Function

        ''' <summary>
        ''' SC3240601_007：Graph情報を取得
        ''' </summary>
        ''' <param name="inVclId">車両ID</param>
        ''' <param name="inVin">VIN</param>
        ''' <param name="inOwnersId">オーナーズID</param>
        ''' <param name="inOccurdate">発生日時</param>
        ''' <param name="inStartDate">検索開始日時</param>
        ''' <param name="inEndDate">検索終了日時</param>
        ''' <returns>走行履歴一覧DataTable</returns>
        ''' <remarks></remarks>
        Public Function GetMileageGraph(ByVal inVclId As Decimal, _
                                        ByVal inVin As String, _
                                        ByVal inOwnersId As String, _
                                        ByVal inOccurdate As Date, _
                                        ByVal inStartDate As Date, _
                                        ByVal inEndDate As Date) As SC3240601TelemaInfoDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} START inVclId:{2};inVin:{3};inOwnersId:{4};inOccurdate:{5};inStartDate:{6};inEndDate:{7};" _
                        , Me.GetType.ToString _
                        , MethodBase.GetCurrentMethod.Name _
                        , inVclId _
                        , inVin _
                        , inOwnersId _
                        , inOccurdate _
                        , inStartDate _
                        , inEndDate))

            'データ格納用
            Dim dt As SC3240601TelemaInfoDataTable

            Dim sql As New StringBuilder

            'SQL文作成
            With sql
                .AppendLine("    SELECT /* SC3240601_007 */ ")
                .AppendLine("           A1.NO ")
                .AppendLine("          ,A1.VCL_MILE_ID ")
                .AppendLine("          ,A1.MARK_SORT ")
                .AppendLine("          ,A1.DLR_CD ")
                .AppendLine("          ,A1.VCL_ID ")
                .AppendLine("          ,A1.CST_ID ")
                .AppendLine("          ,A1.CST_NAME ")
                .AppendLine("          ,A1.REG_DATE ")
                .AppendLine("          ,A1.REG_MILE ")
                .AppendLine("          ,A1.REG_MTD ")
                .AppendLine("          ,A1.REG_STF_CD ")
                .AppendLine("          ,A1.STF_NAME ")
                .AppendLine("          ,A1.DMS_TAKEIN_DATETIME ")
                .AppendLine("          ,TRIM(A1.OWNERS_ID) AS OWNERS_ID ")
                .AppendLine("          ,A1.VIN ")
                .AppendLine("          ,A1.RECEIVESEQ ")
                .AppendLine("          ,A1.SEQNO ")
                .AppendLine("          ,A1.OCCURDATE ")
                .AppendLine("          ,A1.ERRORCODE ")
                .AppendLine("          ,A1.CREATEDATE ")
                .AppendLine("          ,A1.UPDATEDATE ")
                .AppendLine("          ,A1.SVC_NAME_MILE ")
                .AppendLine("      FROM ")
                .AppendLine("          (SELECT ")
                .AppendLine("                  ROWNUM AS NO ")
                .AppendLine("                 ,N1.VCL_MILE_ID ")
                .AppendLine("                 ,N1.MARK_SORT ")
                .AppendLine("                 ,N1.DLR_CD ")
                .AppendLine("                 ,N1.VCL_ID ")
                .AppendLine("                 ,N1.CST_ID ")
                .AppendLine("                 ,N1.CST_NAME ")
                .AppendLine("                 ,N1.REG_DATE ")
                .AppendLine("                 ,N1.REG_MILE ")
                .AppendLine("                 ,N1.REG_MTD ")
                .AppendLine("                 ,N1.REG_STF_CD ")
                .AppendLine("                 ,N1.STF_NAME ")
                .AppendLine("                 ,N1.DMS_TAKEIN_DATETIME ")
                .AppendLine("                 ,N1.OWNERS_ID ")
                .AppendLine("                 ,N1.VIN ")
                .AppendLine("                 ,N1.RECEIVESEQ ")
                .AppendLine("                 ,N1.SEQNO ")
                .AppendLine("                 ,N1.OCCURDATE ")
                .AppendLine("                 ,N1.ERRORCODE ")
                .AppendLine("                 ,N1.CREATEDATE ")
                .AppendLine("                 ,N1.UPDATEDATE ")
                .AppendLine("                 ,N1.SVC_NAME_MILE ")
                .AppendLine("             FROM ")
                .AppendLine("                 (SELECT ")
                .AppendLine("                         M1.VCL_MILE_ID ")
                .AppendLine("                        ,M1.MARK_SORT ")
                .AppendLine("                        ,M1.DLR_CD ")
                .AppendLine("                        ,M1.VCL_ID ")
                .AppendLine("                        ,M1.CST_ID ")
                .AppendLine("                        ,M1.CST_NAME ")
                .AppendLine("                        ,M1.REG_DATE ")
                .AppendLine("                        ,M1.REG_MILE ")
                .AppendLine("                        ,M1.REG_MTD ")
                .AppendLine("                        ,M1.REG_STF_CD ")
                .AppendLine("                        ,M1.STF_NAME ")
                .AppendLine("                        ,M1.DMS_TAKEIN_DATETIME ")
                .AppendLine("                        ,M1.OWNERS_ID ")
                .AppendLine("                        ,M1.VIN ")
                .AppendLine("                        ,M1.RECEIVESEQ ")
                .AppendLine("                        ,M1.SEQNO ")
                .AppendLine("                        ,M1.OCCURDATE ")
                .AppendLine("                        ,M1.ERRORCODE ")
                .AppendLine("                        ,M1.CREATEDATE ")
                .AppendLine("                        ,M1.UPDATEDATE ")
                .AppendLine("                        ,M1.SVC_NAME_MILE ")
                .AppendLine("                   FROM ")
                .AppendLine("                        (SELECT ")
                .AppendLine("                                T1.VCL_MILE_ID AS VCL_MILE_ID ")
                .AppendLine("                               ,CASE WHEN T1.REG_MTD = 2 OR T1.REG_MTD = 3 OR T1.REG_MTD = 4 THEN 20 ")
                .AppendLine("                                     WHEN T1.REG_MTD = 1 THEN 30 ELSE 0 END  MARK_SORT ")
                .AppendLine("                               ,T1.DLR_CD ")
                .AppendLine("                               ,T1.VCL_ID ")
                .AppendLine("                               ,T1.CST_ID ")
                .AppendLine("                               ,T101.CST_NAME ")
                .AppendLine("                               ,TO_DATE(TO_CHAR(T1.REG_DATE, 'YYYY/MM/DD'), 'YYYY/MM/DD') AS REG_DATE ")
                .AppendLine("                               ,T1.REG_MILE ")
                .AppendLine("                               ,T1.REG_MTD ")
                .AppendLine("                               ,T1.REG_STF_CD ")
                .AppendLine("                               ,T104.STF_NAME ")
                .AppendLine("                               ,T1.DMS_TAKEIN_DATETIME ")
                .AppendLine("                               ,' ' AS OWNERS_ID ")
                .AppendLine("                               ,' ' AS VIN ")
                .AppendLine("                               ,0 AS RECEIVESEQ ")
                .AppendLine("                               ,0 AS SEQNO ")
                .AppendLine("                               ,NULL AS OCCURDATE ")
                .AppendLine("                               ,N' ' AS ERRORCODE ")
                .AppendLine("                               ,NULL AS CREATEDATE ")
                .AppendLine("                               ,NULL AS UPDATEDATE ")
                .AppendLine("                               ,T103.SVC_NAME_MILE AS SVC_NAME_MILE ")
                .AppendLine("                           FROM ")
                .AppendLine("                                TB_T_VEHICLE_MILEAGE T1 ")
                .AppendLine("                               ,TB_M_CUSTOMER T101 ")
                .AppendLine("                               ,TB_T_VEHICLE_SVCIN_HIS T102 ")
                .AppendLine("                               ,TB_M_SERVICE T103 ")
                .AppendLine("                               ,TB_M_STAFF T104 ")
                .AppendLine("                          WHERE ")
                .AppendLine("                                T1.CST_ID = T101.CST_ID(+) ")
                .AppendLine("                            AND T1.VCL_MILE_ID = T102.VCL_MILE_ID(+) ")
                .AppendLine("                            AND T1.REG_STF_CD = T104.STF_CD(+) ")
                .AppendLine("                            AND T102.DLR_CD = T103.DLR_CD(+) ")
                .AppendLine("                            AND T102.SVC_CD = T103.SVC_CD(+) ")
                .AppendLine("                            AND T1.VCL_ID = :VCL_ID ")
                .AppendLine("                            AND T1.REG_MTD IN (:REG_MTD_1, :REG_MTD_2, :REG_MTD_3, :REG_MTD_4) ")
                .AppendLine("                      UNION ALL ")
                .AppendLine("                         SELECT ")
                .AppendLine("                                0 AS VCL_MILE_ID ")
                .AppendLine("                               ,40 AS MARK_SORT ")
                .AppendLine("                               ,NULL AS DLR_CD ")
                .AppendLine("                               ,0 AS VCL_ID ")
                .AppendLine("                               ,0 AS CST_ID ")
                .AppendLine("                               ,N' ' AS CST_NAME ")
                .AppendLine("                               ,TO_DATE(TO_CHAR(T4.OCCURDATE, 'YYYY/MM/DD'), 'YYYY/MM/DD') AS REG_DATE ")
                .AppendLine("                               ,T4.MILEAGE AS REG_MILE ")
                .AppendLine("                               ,N'0' AS REG_MTD ")
                .AppendLine("                               ,NULL AS REG_STF_CD ")
                .AppendLine("                               ,NULL AS STF_NAME ")
                .AppendLine("                               ,NULL AS DMS_TAKEIN_DATETIME ")
                .AppendLine("                               ,T4.OWNERS_ID ")
                .AppendLine("                               ,T4.VIN ")
                .AppendLine("                               ,T4.RECEIVESEQ ")
                .AppendLine("                               ,T4.SEQNO ")
                .AppendLine("                               ,T4.OCCURDATE ")
                .AppendLine("                               ,T4.WARNINGCODE ")
                .AppendLine("                               ,T4.CREATEDATE ")
                .AppendLine("                               ,T4.UPDATEDATE ")
                .AppendLine("                               ,NULL AS SVC_NAME_MILE ")
                .AppendLine("                           FROM ")
                .AppendLine("                               (SELECT ")
                .AppendLine("                                       T1.OWNERS_ID ")
                .AppendLine("                                      ,T1.VIN ")
                .AppendLine("                                      ,T1.RECEIVESEQ ")
                .AppendLine("                                      ,T2.SEQNO ")
                .AppendLine("                                      ,T1.MILEAGE ")
                .AppendLine("                                      ,T1.OCCURDATE ")
                .AppendLine("                                      ,T2.WARNINGCODE ")
                .AppendLine("                                      ,T1.CREATEDATE ")
                .AppendLine("                                      ,T1.UPDATEDATE ")
                .AppendLine("                                      ,RTRIM(T1.VIN) AS VIN_RTRIM ")
                .AppendLine("                                  FROM ")
                .AppendLine("                                       TBL_TLM_WARNING T1 ")
                .AppendLine("                                      ,TBL_TLM_WARNING_DETAIL T2 ")
                .AppendLine("                                 WHERE ")
                .AppendLine("                                       T1.OWNERS_ID = T2.OWNERS_ID ")
                .AppendLine("                                   AND T1.VIN = T2.VIN ")
                .AppendLine("                                   AND T1.RECEIVESEQ = T2.RECEIVESEQ ")
                .AppendLine("                               ) T4 ")
                .AppendLine("                          WHERE ")
                .AppendLine("                                T4.VIN_RTRIM = :VIN ")
                .AppendLine("                            AND T4.OWNERS_ID = :OWNERS_ID ")
                .AppendLine("                            AND T4.OCCURDATE >= TO_DATE(:OCCURDATE, 'YYYY/MM/DD HH24:MI:SS') ")
                .AppendLine("                      UNION ALL ")
                .AppendLine("                         SELECT ")
                .AppendLine("                                0 AS VCL_MILE_ID ")
                .AppendLine("                               ,10 AS MARK_SORT ")
                .AppendLine("                               ,CAST (T6.DLRCD AS NVARCHAR2(5)) AS DLR_CD ")
                .AppendLine("                               ,T6.VCL_ID ")
                .AppendLine("                               ,T6.CST_ID ")
                .AppendLine("                               ,N' ' AS CST_NAME ")
                .AppendLine("                               ,TO_DATE(TO_CHAR(T6.REGISTDATE, 'YYYY/MM/DD'), 'YYYY/MM/DD') AS REG_DATE ")
                .AppendLine("                               ,T6.MILEAGE AS REG_MILE ")
                .AppendLine("                               ,N'5' AS REG_MTD ")
                .AppendLine("                               ,NULL AS REG_STF_CD ")
                .AppendLine("                               ,NULL AS STF_NAME ")
                .AppendLine("                               ,NULL AS DMS_TAKEIN_DATETIME ")
                .AppendLine("                               ,T6.OWNERS_ID ")
                .AppendLine("                               ,T6.VIN ")
                .AppendLine("                               ,T6.MILEAGESEQ AS RECEIVESEQ ")
                .AppendLine("                               ,0 AS SEQNO ")
                .AppendLine("                               ,NULL AS OCCURDATE ")
                .AppendLine("                               ,N' ' AS ERRORCODE ")
                .AppendLine("                               ,NULL AS CREATEDATE ")
                .AppendLine("                               ,NULL AS UPDATEDATE ")
                .AppendLine("                               ,NULL AS SVC_NAME_MILE ")
                .AppendLine("                           FROM ")
                .AppendLine("                               (SELECT ")
                .AppendLine("                                       T5.DLRCD ")
                .AppendLine("                                      ,T5.VCL_ID ")
                .AppendLine("                                      ,T5.CST_ID ")
                .AppendLine("                                      ,T5.REGISTDATE ")
                .AppendLine("                                      ,T5.MILEAGE ")
                .AppendLine("                                      ,T5.OWNERS_ID ")
                .AppendLine("                                      ,T5.VIN ")
                .AppendLine("                                      ,T5.MILEAGESEQ ")
                .AppendLine("                                  FROM ")
                .AppendLine("                                       TBL_TLM_MILEAGEHIS T5 ")
                .AppendLine("                                 WHERE ")
                .AppendLine("                                       T5.VCL_ID = :VCL_ID ")
                .AppendLine("                                   AND T5.ORIGINALFLG = :ORIGINALFLG ")
                .AppendLine("                                   AND T5.DELFLG = :DELFLG ")
                .AppendLine("                               ) T6 ")
                .AppendLine("                        ) M1 ")
                .AppendLine("               ORDER BY ")
                .AppendLine("                        M1.REG_DATE DESC ")
                .AppendLine("                       ,M1.MARK_SORT DESC ")
                .AppendLine("                       ,M1.RECEIVESEQ DESC ")
                .AppendLine("                ) N1 ")
                .AppendLine("         ) A1 ")
                .AppendLine("   WHERE A1.REG_DATE BETWEEN :STARTDATE AND :ENDDATE ")
                .AppendLine("   ORDER BY  A1.NO ASC ")
            End With

            Using query As New DBSelectQuery(Of SC3240601TelemaInfoDataTable)("SC3240601_007")
                query.CommandText = sql.ToString()

                'バインド変数
                query.AddParameterWithTypeValue("VCL_ID", OracleDbType.Decimal, inVclId)
                query.AddParameterWithTypeValue("REG_MTD_1", OracleDbType.NVarchar2, REG_MTD_1)
                query.AddParameterWithTypeValue("REG_MTD_2", OracleDbType.NVarchar2, REG_MTD_2)
                query.AddParameterWithTypeValue("REG_MTD_3", OracleDbType.NVarchar2, REG_MTD_3)
                query.AddParameterWithTypeValue("REG_MTD_4", OracleDbType.NVarchar2, REG_MTD_4)
                query.AddParameterWithTypeValue("VIN", OracleDbType.NVarchar2, inVin)
                query.AddParameterWithTypeValue("OWNERS_ID", OracleDbType.Char, inOwnersId)
                query.AddParameterWithTypeValue("OCCURDATE", OracleDbType.Date, inOccurdate)
                query.AddParameterWithTypeValue("ORIGINALFLG", OracleDbType.Char, ORIGINALFLG_1)
                query.AddParameterWithTypeValue("DELFLG", OracleDbType.Char, DELFLG_0)
                query.AddParameterWithTypeValue("STARTDATE", OracleDbType.Date, inStartDate)
                query.AddParameterWithTypeValue("ENDDATE", OracleDbType.Date, inEndDate)

                'データ取得
                dt = query.GetData()

            End Using

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END OUT:COUNT = {2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , dt.Rows.Count.ToString(CultureInfo.CurrentCulture)))
            Return dt

        End Function

        ''' <summary>
        ''' SC3240601_002：所有者情報を取得
        ''' </summary>
        ''' <param name="inDlrCD">販売店コード</param>
        ''' <param name="inVclId">車両ID</param>
        ''' <returns>所有者情報Row</returns>
        ''' <remarks></remarks>
        Public Function GetOwnerInfo(ByVal inDlrCD As String, _
                                     ByVal inVclId As Decimal) As SC3240601OwnerInfoRow

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} START inDlrCD:{2};inVclId:{3};" _
                        , Me.GetType.ToString _
                        , MethodBase.GetCurrentMethod.Name _
                        , inDlrCD _
                        , inVclID))

            'データ格納用
            Dim dt As SC3240601OwnerInfoDataTable

            Dim sql As New StringBuilder

            'SQL文作成
            With sql
                .AppendLine(" SELECT /* SC3240601_002 */ ")
                .AppendLine("        T1.VCL_ID ")
                .AppendLine("       ,T4.CST_NAME AS OWNER ")
                .AppendLine("       ,T1.VCL_VIN AS VIN ")
                .AppendLine("       ,NVL(TRIM(T5.MODEL_NAME), T1.NEWCST_MODEL_NAME) AS MODEL_CD ")
                .AppendLine("       ,T2.REG_NUM ")
                .AppendLine("   FROM ")
                .AppendLine("        TB_M_VEHICLE T1 ")
                .AppendLine("       ,TB_M_VEHICLE_DLR T2 ")
                .AppendLine("       ,TB_M_CUSTOMER_VCL T3 ")
                .AppendLine("       ,TB_M_CUSTOMER T4 ")
                .AppendLine("       ,TB_M_MODEL T5 ")
                .AppendLine("  WHERE ")
                .AppendLine("        T3.DLR_CD = T2.DLR_CD ")
                .AppendLine("    AND T1.VCL_ID = T2.VCL_ID(+) ")
                .AppendLine("    AND T1.VCL_ID = T3.VCL_ID(+) ")
                .AppendLine("    AND T3.CST_ID = T4.CST_ID(+) ")
                .AppendLine("    AND T1.MODEL_CD = T5.MODEL_CD(+) ")
                .AppendLine("    AND T3.DLR_CD = :DLR_CD ")
                .AppendLine("    AND T3.CST_VCL_TYPE = :CST_VCL_TYPE ")
                .AppendLine("    AND T1.VCL_ID = :VCL_ID ")
                .AppendLine("  ORDER BY OWNER ")
            End With

            Using query As New DBSelectQuery(Of SC3240601OwnerInfoDataTable)("SC3240601_002")
                query.CommandText = sql.ToString()

                'バインド変数
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, inDlrCD)
                query.AddParameterWithTypeValue("CST_VCL_TYPE", OracleDbType.NVarchar2, CST_VCL_TYPE_1)
                query.AddParameterWithTypeValue("VCL_ID", OracleDbType.Decimal, inVclID)

                'データ取得
                dt = query.GetData()

            End Using

            If (0 < dt.Rows.Count) Then
                '検索情報がある場合、該当情報Rowで返却

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} END OUT:COUNT = {2}" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                            , dt.Rows.Count.ToString(CultureInfo.CurrentCulture)))

                Return dt(0)
            Else
                '検索情報がない場合、Nothingを返却
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} Data NoFound,  Return Nothing END" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return Nothing
            End If

        End Function

        ''' <summary>
        ''' SC3240601_003：オーナーズID取得
        ''' </summary>
        ''' <param name="inVin">VIN</param>
        ''' <param name="inVclId">車両ID</param>
        ''' <returns>オーナーズID</returns>
        ''' <remarks></remarks>
        Public Function GetOwnerId(ByVal inVin As String, _
                                   ByVal inVclId As Decimal) As String

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} START inVin:{2};inVclId:{3};" _
                        , Me.GetType.ToString _
                        , MethodBase.GetCurrentMethod.Name _
                        , inVin _
                        , inVclId))

            'データ格納用
            Dim dt As SC3240601OwnersIdDataTable

            Dim sql As New StringBuilder

            'SQL文作成
            With sql
                .AppendLine(" SELECT /* SC3240601_003 */ ")
                .AppendLine("        T3.OWNERS_ID ")
                .AppendLine("   FROM ")
                .AppendLine("        TB_M_VEHICLE T1 ")
                .AppendLine("       ,TB_M_MEMBER T2 ")
                .AppendLine("       ,TBL_TLM_CONTRACT T3 ")
                .AppendLine("  WHERE ")
                .AppendLine("        T1.VCL_VIN = T2.VCL_VIN ")
                .AppendLine("    AND T2.VCL_VIN = RTRIM(T3.VIN) ")
                .AppendLine("    AND T2.MEM_SYSTEM_ID = TRIM(T3.OWNERS_ID) ")
                .AppendLine("    AND T1.VCL_ID = :VCL_ID ")
                .AppendLine("    AND RTRIM(T3.VIN) = :VCL_VIN ")
                .AppendLine("    AND T3.DELFLG = :DELFLG_0 ")
            End With

            Using query As New DBSelectQuery(Of SC3240601OwnersIdDataTable)("SC3240601_003")
                query.CommandText = sql.ToString()

                'バインド変数
                query.AddParameterWithTypeValue("VCL_ID", OracleDbType.Decimal, inVclId)
                query.AddParameterWithTypeValue("VCL_VIN", OracleDbType.Varchar2, inVin)
                query.AddParameterWithTypeValue("DELFLG_0", OracleDbType.Char, DELFLG_0)

                'データ取得
                dt = query.GetData()

            End Using

            If (0 < dt.Rows.Count) Then
                '検索情報がある場合オーナーズIDを返却する
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} END OUT:OwnersID = {2}" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                            , dt(0).OWNERS_ID))

                Return dt(0).OWNERS_ID
            Else
                '検索情報がない場合、String.Emptyを返却
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} Data NoFound, Return String.Empty END" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return String.Empty
            End If

        End Function

        ''' <summary>
        ''' SC3240601_004：店舗名を取得
        ''' </summary>
        ''' <param name="inDealerCode">販売店コード</param>
        ''' <param name="inBranchCode">店舗コード</param>
        ''' <returns>店舗名称</returns>
        ''' <remarks></remarks>
        Public Function GetBranchName(ByVal inDealerCode As String, _
                                      ByVal inBranchCode As String) As String

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} START inDealerCode:{2};inBranchCode:{3};" _
                        , Me.GetType.ToString _
                        , MethodBase.GetCurrentMethod.Name _
                        , inDealerCode _
                        , inBranchCode))

            'データ格納用
            Dim dt As SC3240601BranchInfoDataTable

            Dim sql As New StringBuilder

            'SQL文作成
            With sql
                .AppendLine(" SELECT /* SC3240601_004 */ ")
                .AppendLine("        NVL(TRIM(T1.BRN_NAME), T1.BRN_NAME_ENG) AS BRN_NAME ")
                .AppendLine("   FROM ")
                .AppendLine("        TB_M_BRANCH T1 ")
                .AppendLine("  WHERE ")
                .AppendLine("        T1.DLR_CD = :DLR_CD ")
                .AppendLine("    AND T1.BRN_CD = :BRN_CD ")
                .AppendLine("    AND T1.INUSE_FLG = :INUSE_FLG_1 ")
            End With

            Using query As New DBSelectQuery(Of SC3240601BranchInfoDataTable)("SC3240601_004")
                query.CommandText = sql.ToString()

                'バインド変数
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, inDealerCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, inBranchCode)
                query.AddParameterWithTypeValue("INUSE_FLG_1", OracleDbType.NVarchar2, INUSE_FLG_1)

                'データ取得
                dt = query.GetData()

            End Using

            If (0 < dt.Rows.Count) Then
                '検索情報がある場合販売店名称を返却する
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} END OUT:DealerName = {2}" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                            , dt(0).BRN_NAME))
                Return dt(0).BRN_NAME
            Else
                '検索情報がない場合、String.Emptyを返却
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} Data NoFound, Return String.Empty END" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return String.Empty
            End If

        End Function

        ''' <summary>
        ''' SC3240601_005：Warning詳細を取得
        ''' </summary>
        ''' <param name="inCntCD">国番号</param>
        ''' <param name="inOwnersId">オーナーズID</param>
        ''' <param name="inVin">VIN</param>
        ''' <param name="inReceiveSeq">受信連番</param>
        ''' <param name="inSeqNo">連番</param>
        ''' <returns>Warning詳細DataTable</returns>
        ''' <remarks></remarks>
        Public Function GetWarningDetail(ByVal inCntCD As String, _
                                         ByVal inOwnersId As String, _
                                         ByVal inVin As String, _
                                         ByVal inReceiveSeq As Long, _
                                         ByVal inSeqNo As Long) As SC3240601WarningDetailDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} START inCntCD:{2};inOwnersId:{3};inVin:{4};inReceiveSeq:{5};inSeqNo:{6};" _
                        , Me.GetType.ToString _
                        , MethodBase.GetCurrentMethod.Name _
                        , inCntCD _
                        , inOwnersId _
                        , inVin _
                        , inReceiveSeq _
                        , inSeqNo))

            'データ格納用
            Dim dt As SC3240601WarningDetailDataTable

            Dim sql As New StringBuilder

            'SQL文作成
            With sql
                .AppendLine(" SELECT /* SC3240601_005 */ ")
                .AppendLine("        T2.OCCURDATE ")
                .AppendLine("       ,T2.MILEAGE ")
                .AppendLine("       ,T3.WARNINGCODE ")
                .AppendLine("       ,NVL(TRIM(T1.WARNINGNAME), T3.WARNINGNAME) AS WARNINGNAME ")
                .AppendLine("       ,T1.INDICATOR_IMGFILE ")
                .AppendLine("       ,T1.EXPLANATION ")
                .AppendLine("   FROM ")
                .AppendLine("        TBL_TLM_WARNING_MST T1 ")
                .AppendLine("       ,TBL_TLM_WARNING T2 ")
                .AppendLine("       ,TBL_TLM_WARNING_DETAIL T3 ")
                .AppendLine("  WHERE ")
                .AppendLine("        T2.OWNERS_ID = T3.OWNERS_ID ")
                .AppendLine("    AND T2.VIN = T3.VIN ")
                .AppendLine("    AND T2.RECEIVESEQ = T3.RECEIVESEQ ")
                .AppendLine("    AND T3.WARNINGCODE = T1.WARNINGCODE ")
                .AppendLine("    AND T1.CNTCD = :CNTCD ")
                .AppendLine("    AND T2.OWNERS_ID = :OWNERS_ID ")
                .AppendLine("    AND T2.VIN = :VIN ")
                .AppendLine("    AND T2.RECEIVESEQ = :RECEIVESEQ ")
                .AppendLine("    AND T3.SEQNO = :SEQNO ")
            End With

            Using query As New DBSelectQuery(Of SC3240601WarningDetailDataTable)("SC3240601_005")
                query.CommandText = sql.ToString()

                'バインド変数
                query.AddParameterWithTypeValue("CNTCD", OracleDbType.NChar, inCntCD)
                query.AddParameterWithTypeValue("OWNERS_ID", OracleDbType.Char, inOwnersId)
                query.AddParameterWithTypeValue("VIN", OracleDbType.Varchar2, inVin)
                query.AddParameterWithTypeValue("RECEIVESEQ", OracleDbType.Long, inReceiveSeq)
                query.AddParameterWithTypeValue("SEQNO", OracleDbType.Long, inSEQNO)

                'データ取得
                dt = query.GetData()

            End Using

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END OUT:COUNT = {2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , dt.Rows.Count.ToString(CultureInfo.CurrentCulture)))

            Return dt

        End Function

#End Region

    End Class

End Namespace

Partial Class SC3240601DataSet
    Partial Class SC3240601GraphJsonDataTable

    End Class

End Class


