'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3340101DataSet.vb
'─────────────────────────────────────
'機能：洗車マンメインメニュー(CW)のデータセット
'補足： 
'作成：2015/01/05 TMEJ 範  NextSTEPサービス 洗車用端末開発向けた評価用アプリ作成
'更新：2018/07/09 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示
'更新：2019/09/09 NSK 大平 (FS)納車時オペレーションCS向上にむけた評価 Pilot-231574 メインメニュー(CW)でエラーが発生する
'更新：
'─────────────────────────────────────

Option Strict On
Option Explicit On

Imports System.Text
Imports Oracle.DataAccess.Client
Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks.Core

Partial Public Class SC3340101DataSet


End Class

Namespace SC3340101DataSetTableAdapters

    Public Class SC3340101DataTableAdapter
        Inherits Global.System.ComponentModel.Component

#Region "定数"

        ''' <summary>
        ''' 洗車必要フラグ　1:洗車必要
        ''' </summary>
        ''' <remarks></remarks>
        Private Const CARWASH_NEED As String = "1"

#Region "キャンセルフラグ"

        ''' <summary>
        ''' キャンセルフラグ 　0:有効　
        ''' </summary>
        ''' <remarks></remarks>
        Private Const NOT_CANCEL = "0"

#End Region

#Region "サービスステータス"

        ''' <summary>
        ''' サービスステータス：洗車待ち
        ''' </summary>
        ''' <remarks></remarks>
        Private Const SVC_STATUS_CARWASHWAIT As String = "07"

        ''' <summary>
        ''' サービスステータス：洗車中
        ''' </summary>
        ''' <remarks></remarks>
        Private Const SVC_STATUS_CARWASHSTART As String = "08"

#End Region

#Region "日付"
        ''' <summary>
        ''' 最小日付
        ''' </summary>
        ''' <remarks></remarks>
        Private Const MINDATE As String = "1900/01/01 00:00:00"

        ''' <summary>
        ''' 最大日付
        ''' </summary>
        ''' <remarks></remarks>
        Private Const MAXDATE As String = "9999/01/01 00:00:00"

#End Region


#End Region

        '2018/07/09 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
#Region "P/Lマークフラグ"
        ''' <summary>
        ''' P/Lマークフラグ（0：非表示）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const IconFlagOff As String = "0"
#End Region
        '2018/07/09 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END

#Region "Select"

        ''' <summary>
        ''' 洗車バナー情報一覧の取得
        ''' </summary>
        ''' <param name="inDealerCode">販売店コード</param>
        ''' <param name="inBranchCode">店舗コード</param>
        ''' <param name="inSelectCount">取得件数</param>
        ''' <returns>洗車バナー情報テーブル</returns>
        ''' <remarks></remarks>
        Public Function GetCarWashInfo(ByVal inDealerCode As String, _
                                       ByVal inBranchCode As String, _
                                       ByVal inSelectCount As Long) As SC3340101DataSet.SC3340101CarWashInfoDataTable

            'ログ出力
            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}_Start. inDealerCode={1}, inBranchCode={2}, inSelectCount={3}", _
                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                      inDealerCode, _
                                      inBranchCode, _
                                      inSelectCount))

            Dim sql As New StringBuilder

            ' SQL文の作成
            With sql
                .AppendLine(" SELECT ")
                .AppendLine("        /* SC3340101_001 */ ")
                .AppendLine("        T7.SVCIN_ID  ")
                .AppendLine("      , TRIM(T7.SVC_STATUS) AS SVC_STATUS ")
                .AppendLine("      , TRIM(T7.RO_NUM) AS RO_NUM ")
                .AppendLine("      , TRIM(T7.PICK_DELI_TYPE) AS PICK_DELI_TYPE ")
                .AppendLine("      , TRIM(T7.ACCEPTANCE_TYPE) AS ACCEPTANCE_TYPE ")
                .AppendLine("      , T7.SCHE_DELI_DATETIME  ")
                .AppendLine("      , T7.ROW_LOCK_VERSION  ")
                .AppendLine("      , T7.JOB_DTL_ID ")
                .AppendLine("      , T7.STALL_USE_ID ")
                .AppendLine("      , T7.RSLT_END_DATETIME  ")
                .AppendLine("      , TRIM(T7.MODEL_CD) AS MODEL_CD ")
                .AppendLine("      , TRIM(NVL(T7.MODEL_NAME, T7.NEWCST_MODEL_NAME)) AS MODEL_NAME  ")
                .AppendLine("      , TRIM(T7.REG_NUM) AS REG_NUM    ")
                .AppendLine("      , T7.RSLT_START_DATETIME  ")
                '2018/07/09 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
                .AppendLine("      , T7.IMP_VCL_FLG ")
                '2018/07/09 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
                .AppendLine("   FROM ")
                .AppendLine("        ( ")
                .AppendLine("           SELECT ")
                .AppendLine("                   T1.SVCIN_ID ")
                .AppendLine("                 , T1.DLR_CD ")
                .AppendLine("                 , T1.BRN_CD ")
                .AppendLine("                 , T6.JOB_DTL_ID ")
                .AppendLine("                 , T6.STALL_USE_ID ")
                .AppendLine("                 , T6.RSLT_END_DATETIME ")
                .AppendLine("                 , T1.RO_NUM  ")
                .AppendLine("                 , T1.CST_ID ")
                .AppendLine("                 , T1.VCL_ID  ")
                .AppendLine("                 , T1.PICK_DELI_TYPE  ")
                .AppendLine("                 , T1.ACCEPTANCE_TYPE  ")
                .AppendLine("                 , T1.SVC_STATUS  ")
                .AppendLine("                 , T1.SCHE_DELI_DATETIME ")
                .AppendLine("                 , T1.ROW_LOCK_VERSION  ")
                .AppendLine("                 , T2.MODEL_CD ")
                .AppendLine("                 , T2.NEWCST_MODEL_NAME  ")
                .AppendLine("                 , T3.MODEL_NAME  ")
                .AppendLine("                 , T4.REG_NUM ")
                .AppendLine("                 , T5.RSLT_START_DATETIME ")
                '2018/07/09 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
                .AppendLine("                 , NVL(TRIM(T4.IMP_VCL_FLG), :ICON_FLAG_OFF) AS IMP_VCL_FLG ")
                '2018/07/09 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
                .AppendLine("             FROM ")
                .AppendLine("                   TB_T_SERVICEIN T1 ")
                .AppendLine("                 , TB_M_VEHICLE T2 ")
                .AppendLine("                 , TB_M_MODEL T3  ")
                .AppendLine("                 , TB_M_VEHICLE_DLR T4 ")
                .AppendLine("                 , TB_T_CARWASH_RESULT T5 ")
                .AppendLine("                 , ( ")
                .AppendLine("                       SELECT ")
                .AppendLine("                               A1.SVCIN_ID  ")
                .AppendLine("                             , A1.DLR_CD  ")
                .AppendLine("                             , A1.BRN_CD   ")
                '2019/09/09 NSK 大平 Pilot-231574 メインメニュー(CW)でエラーが発生する START
                '.AppendLine("                             , MAX(A2.JOB_DTL_ID) AS  JOB_DTL_ID   ")
                '.AppendLine("                             , MAX(A3.STALL_USE_ID) AS STALL_USE_ID   ")
                '.AppendLine("                             , MAX(A3.RSLT_END_DATETIME) AS RSLT_END_DATETIME   ")
                .AppendLine("                             , A2.JOB_DTL_ID AS  JOB_DTL_ID   ")
                .AppendLine("                             , A3.STALL_USE_ID AS STALL_USE_ID   ")
                .AppendLine("                             , A3.RSLT_END_DATETIME AS RSLT_END_DATETIME   ")
                '2019/09/09 NSK 大平 Pilot-231574 メインメニュー(CW)でエラーが発生する END
                .AppendLine("                         FROM   ")
                .AppendLine("                               TB_T_SERVICEIN A1   ")
                .AppendLine("                             , TB_T_JOB_DTL A2   ")
                .AppendLine("                             , TB_T_STALL_USE A3   ")
                .AppendLine("                        WHERE   ")
                .AppendLine("                               A1.SVCIN_ID = A2.SVCIN_ID   ")
                .AppendLine("                          AND  A2.JOB_DTL_ID = A3.JOB_DTL_ID   ")
                .AppendLine("                          AND  A1.DLR_CD = :DLRCD   ")
                .AppendLine("                          AND  A1.BRN_CD = :STRCD   ")
                .AppendLine("                          AND  A2.DLR_CD = :DLRCD    ")
                .AppendLine("                          AND  A2.BRN_CD = :STRCD   ")
                .AppendLine("                          AND  A1.SVC_STATUS IN (:SVC_STATUS1,:SVC_STATUS2)   ")
                .AppendLine("                          AND  A1.CARWASH_NEED_FLG = :CARWASH_NEED_FLG   ")
                .AppendLine("                          AND  A2.CANCEL_FLG = :CANCEL_FLG   ")
                '2019/09/02 NSK 大平 Pilot-231574 メインメニュー(CW)でエラーが発生する START
                .AppendLine("                          AND ( A2.SVCIN_ID, A3.RSLT_END_DATETIME ) = ( ")
                .AppendLine("                              SELECT ")
                .AppendLine("                                  A4.SVCIN_ID, MAX(A5.RSLT_END_DATETIME) ")
                .AppendLine("                              FROM ")
                .AppendLine("                                  TB_T_SERVICEIN A4, TB_T_JOB_DTL A7, TB_T_STALL_USE A5 ")
                .AppendLine("                              WHERE ")
                .AppendLine("                                      A4.SVCIN_ID = A7.SVCIN_ID ")
                .AppendLine("                                  AND A7.JOB_DTL_ID = A5.JOB_DTL_ID ")
                .AppendLine("                                  AND A4.SVCIN_ID = A1.SVCIN_ID ")
                .AppendLine("                                  AND A7.CANCEL_FLG=:CANCEL_FLG ")
                .AppendLine("                              GROUP BY A4.SVCIN_ID ")
                .AppendLine("                              ) ")
                .AppendLine("                          AND A3.STALL_USE_ID=( ")
                .AppendLine("                              SELECT ")
                .AppendLine("                                  MAX(STALL_USE_ID) ")
                .AppendLine("                              FROM ")
                .AppendLine("                                  TB_T_STALL_USE A6 ")
                .AppendLine("                              WHERE ")
                .AppendLine("                                  A6.JOB_DTL_ID=A2.JOB_DTL_ID ")
                .AppendLine("                              ) ")
                '.AppendLine("                     GROUP BY   ")
                '.AppendLine("                               A1.SVCIN_ID   ")
                '.AppendLine("                             , A1.DLR_CD   ")
                '.AppendLine("                             , A1.BRN_CD    ")
                '2019/09/02 NSK 大平 Pilot-231574 メインメニュー(CW)でエラーが発生する END
                .AppendLine("                   ) T6 ")
                .AppendLine("            WHERE ")
                .AppendLine(" 					T1.SVCIN_ID = T6.SVCIN_ID  ")
                .AppendLine(" 			   AND  T1.VCL_ID   = T2.VCL_ID   ")
                .AppendLine("              AND  T2.MODEL_CD = T3.MODEL_CD (+)   ")
                .AppendLine("              AND  T1.VCL_ID   = T4.VCL_ID  ")
                .AppendLine("              AND  T1.DLR_CD   = T4.DLR_CD  ")
                .AppendLine("              AND  T1.SVCIN_ID = T5.SVCIN_ID(+)  ")
                .AppendLine("         ORDER BY   ")
                .AppendLine("                   (    ") 'デフォルトの日付が一番下
                .AppendLine("                    CASE   ")
                .AppendLine("                    WHEN  T1.SCHE_DELI_DATETIME = :MINDATE THEN :MAXDATE   ")
                .AppendLine("                    ELSE  T1.SCHE_DELI_DATETIME ")
                .AppendLine("                     END ")
                .AppendLine("                   )  ASC ")
                .AppendLine("                 , T1.ACCEPTANCE_TYPE ASC   ")
                .AppendLine("                 ,    ")
                .AppendLine("                   (    ")
                .AppendLine("                    CASE   ")
                .AppendLine("                    WHEN  T1.PICK_DELI_TYPE = '0' THEN '01'   ")
                .AppendLine("                    WHEN  T1.PICK_DELI_TYPE = '4' THEN '02'   ")
                .AppendLine("                     END ")
                .AppendLine("                   )  ASC ")
                .AppendLine("                 , T6.RSLT_END_DATETIME ASC   ")
                .AppendLine("         ) T7 ")
                .AppendLine("    WHERE ")
                .AppendLine("         ROWNUM <= :ROW_NUM  ")

            End With

            Dim carwashDatatable As SC3340101DataSet.SC3340101CarWashInfoDataTable = Nothing

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3340101DataSet.SC3340101CarWashInfoDataTable)("SC3340101_001")

                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, inDealerCode)                     ' 販売店コード
                query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, inBranchCode)                     ' 店舗コード
                query.AddParameterWithTypeValue("CANCEL_FLG", OracleDbType.NVarchar2, NOT_CANCEL)                     ' キャンセルフラグ
                query.AddParameterWithTypeValue("SVC_STATUS1", OracleDbType.NVarchar2, SVC_STATUS_CARWASHWAIT)                     ' 洗車待ち
                query.AddParameterWithTypeValue("SVC_STATUS2", OracleDbType.NVarchar2, SVC_STATUS_CARWASHSTART)                     ' 洗車中
                query.AddParameterWithTypeValue("CARWASH_NEED_FLG", OracleDbType.NVarchar2, CARWASH_NEED)                     ' 洗車必要フラグ
                '2018/07/09 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
                query.AddParameterWithTypeValue("ICON_FLAG_OFF", OracleDbType.NVarchar2, IconFlagOff)                     ' アイコンの非表示フラグ
                '2018/07/09 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
                query.AddParameterWithTypeValue("ROW_NUM", OracleDbType.Long, inSelectCount)
                query.AddParameterWithTypeValue("MINDATE", OracleDbType.Date, Date.Parse(MINDATE, CultureInfo.InvariantCulture))
                query.AddParameterWithTypeValue("MAXDATE", OracleDbType.Date, Date.Parse(MAXDATE, CultureInfo.InvariantCulture))

                carwashDatatable = query.GetData()

                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                          "{0}_End.Query count is {1}.", _
                                          System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                          carwashDatatable.Count))

                '検索結果の返却()
                Return carwashDatatable

            End Using

        End Function

        ''' <summary>
        ''' 洗車情報件数取得
        ''' </summary>
        ''' <param name="inDealerCode">販売店コード</param>
        ''' <param name="inBranchCode">店舗コード</param>
        ''' <returns>洗車情報件数情報</returns>
        ''' <remarks></remarks>
        Public Function GetCarWashInfoCount(ByVal inDealerCode As String, _
                                            ByVal inBranchCode As String) As SC3340101DataSet.SC3340101CarWashCountDataTable

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                          "{0}_Start. inDealerCode={1}, inBranchCode={2}", _
                          System.Reflection.MethodBase.GetCurrentMethod.Name, _
                          inDealerCode, _
                          inBranchCode))

            Dim sql As New StringBuilder

            ' SQL文の作成
            With sql
                .AppendLine(" SELECT ")
                .AppendLine("          /* SC3340101_002 */ ")
                .AppendLine("          COUNT(1) AS CAR_WASH_COUNT ")
                .AppendLine("   FROM ")
                .AppendLine("          TB_T_SERVICEIN T1 ")
                .AppendLine("        , TB_T_JOB_DTL T2 ")
                .AppendLine("        , TB_T_STALL_USE T3 ")
                .AppendLine("        , TB_M_VEHICLE T4 ")
                .AppendLine("        , TB_M_VEHICLE_DLR T5 ")
                .AppendLine("   WHERE ")
                .AppendLine("          T1.SVCIN_ID = T2.SVCIN_ID ")
                .AppendLine("     AND  T2.JOB_DTL_ID = T3.JOB_DTL_ID ")
                .AppendLine("     AND  T1.VCL_ID = T4.VCL_ID  ")
                .AppendLine("     AND  T1.VCL_ID = T5.VCL_ID  ")
                .AppendLine("     AND  T1.DLR_CD = T5.DLR_CD   ")
                .AppendLine("     AND  T1.DLR_CD = :DLRCD ")
                .AppendLine("     AND  T1.BRN_CD = :STRCD ")
                .AppendLine("     AND  T2.DLR_CD = :DLRCD ")
                .AppendLine("     AND  T2.BRN_CD = :STRCD ")
                .AppendLine("     AND  T1.SVC_STATUS IN (:SVC_STATUS1,:SVC_STATUS2) ")
                .AppendLine("     AND  T1.CARWASH_NEED_FLG = :CARWASH_NEED_FLG ")
                .AppendLine("     AND  T2.JOB_DTL_ID =   ( ")
                .AppendLine("                               SELECT ")
                .AppendLine("                                       MAX(JOB_DTL_ID) ")
                .AppendLine("                                 FROM ")
                .AppendLine("                                       TB_T_JOB_DTL T6  ")
                .AppendLine("                                WHERE ")
                .AppendLine("                                       T6.SVCIN_ID = T1.SVCIN_ID ")
                .AppendLine("                                  AND  T6.CANCEL_FLG = :CANCEL_FLG ")
                .AppendLine("                            )  ")
                .AppendLine("     AND  T3.STALL_USE_ID = ( ")
                .AppendLine("                               SELECT ")
                .AppendLine("                                       MAX(STALL_USE_ID)  ")
                .AppendLine("                                 FROM ")
                .AppendLine("                                       TB_T_STALL_USE T7   ")
                .AppendLine("                                WHERE ")
                .AppendLine("                                       T7.JOB_DTL_ID=T2.JOB_DTL_ID ")
                .AppendLine("                            )  ")

            End With

            Dim carWashCountDataTable As SC3340101DataSet.SC3340101CarWashCountDataTable = Nothing

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3340101DataSet.SC3340101CarWashCountDataTable)("SC3340101_002")

                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, inDealerCode)                     ' 販売店コード
                query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, inBranchCode)                     ' 店舗コード
                query.AddParameterWithTypeValue("CANCEL_FLG", OracleDbType.NVarchar2, NOT_CANCEL)                     ' キャンセルフラグ
                query.AddParameterWithTypeValue("SVC_STATUS1", OracleDbType.NVarchar2, SVC_STATUS_CARWASHWAIT)                     ' 洗車待ち
                query.AddParameterWithTypeValue("SVC_STATUS2", OracleDbType.NVarchar2, SVC_STATUS_CARWASHSTART)                     ' 洗車中
                query.AddParameterWithTypeValue("CARWASH_NEED_FLG", OracleDbType.NVarchar2, CARWASH_NEED)                     ' 洗車必要フラグ

                carWashCountDataTable = query.GetData()

                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                          "{0}_End.Query count is {1}.", _
                                          System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                          carWashCountDataTable.Count))

                '検索結果の返却()
                Return carWashCountDataTable

            End Using

        End Function

#End Region

    End Class

End Namespace