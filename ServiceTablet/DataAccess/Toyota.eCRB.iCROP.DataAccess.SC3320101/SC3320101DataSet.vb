'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3320101DataSet.vb
'─────────────────────────────────────
'機能：メインメニュー(ASA)のデータセット
'補足： 
'作成：2014/08/14 TMEJ 丁 NextSTEPサービス 作業進捗管理に向けたシステム構想検討
'更新：2015/03/02 TMEJ 小澤 DMS連携版タブレット 来店情報異常時 チップ表示制御追加開発
'更新；2015/11/11 TM 皆川 TR-SVT-TMT-20150929-001対応 車両登録番号を販売店車両から優先して取得する
'更新： 
'─────────────────────────────────────

Option Strict On
Option Explicit On

Imports System.Text
Imports Oracle.DataAccess.Client
Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks.Core

Partial Public Class SC3320101DataSet
    Partial Class SC3320101VisitInfoDataTable

    End Class

End Class

Namespace SC3320101DataSetTableAdapters

    Public Class SC3320101DataTableAdapter
        Inherits Global.System.ComponentModel.Component

#Region "定数"

        ''' <summary>
        ''' 自画面のプログラムID
        ''' </summary>
        ''' <remarks></remarks>
        Private Const MY_PROGRAMID As String = "SC3320101"

        '2015/03/02 TMEJ 小澤 DMS連携版タブレット 来店情報異常時 チップ表示制御追加開発 START

        ''' <summary>
        ''' 振当ステータス 0：未振当
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ASSIGNSTATUS_NONEASSIGN As String = "0"

        ''' <summary>
        ''' 振当ステータス 1：受付待
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ASSIGNSTATUS_ASSIGNWAIT As String = "1"

        '2015/03/02 TMEJ 小澤 DMS連携版タブレット 来店情報異常時 チップ表示制御追加開発 END

        ''' <summary>
        ''' 振当ステータス 2：SA振当済
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ASSIGNSTATUS_ASSIGNED As String = "2"

        ''' <summary>
        ''' ROステータス 50：顧客承認済
        ''' </summary>
        ''' <remarks></remarks>
        Private Const RO_STATUS_CONFRIMED As String = "50"


#End Region


#Region "Select"

#Region "メイン画面表示するための来店者情報取得"
        ''' <summary>
        ''' メイン画面表示するための来店者情報取得
        ''' </summary>
        ''' <param name="settingVal">N日分前のデータを取得システム設定値</param>
        ''' <param name="inDlrCode">販売店コード</param>
        ''' <param name="inBrnCode">店舗コード</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' 2015/03/02 TMEJ 小澤 DMS連携版タブレット 来店情報異常時 チップ表示制御追加開発
        ''' </history>
        Public Function GetServiceVisitInfoForDisplay(ByVal settingVal As String, _
                                              ByVal inDlrCode As String, _
                                              ByVal inBrnCode As String) As SC3320101DataSet.SC3320101VisitInfoDataTable

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}_S.　settingVal={1}, inDlrCode={2}, inBrnCode={3}", _
                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                      settingVal, _
                                      inDlrCode, _
                                      inBrnCode))

            Dim sql As New StringBuilder

            ' SQL文の作成
            With sql
                .AppendLine(" SELECT ")
                .AppendLine("  /* SC3320101_001 */ ")
                .AppendLine("     T14.VISITSEQ")
                .AppendLine("   , T14.ASSIGNTIMESTAMP ")
                .AppendLine("   , T14.VCLREGNO ")
                .AppendLine("   , T14.PARKINGCODE ")
                .AppendLine("   , T14.VISITTIMESTAMP ")
                .AppendLine("   , T14.USERNAME ")
                .AppendLine("   , T14.MODEL_NAME ")
                .AppendLine("   FROM ")
                .AppendLine("        ( ")
                .AppendLine("           SELECT ")
                .AppendLine("               T1.VISITSEQ ")
                .AppendLine("             , T1.ASSIGNTIMESTAMP ")
                '2015/11/11 TM 皆川 TR-SVT-TMT-20150929-001対応 車両登録番号を販売店車両から優先して取得する START
                '.AppendLine("             , T1.VCLREGNO ")
                .AppendLine("             , NVL(TRIM(S7.REG_NUM), T1.VCLREGNO) AS VCLREGNO ")
                '2015/11/11 TM 皆川 TR-SVT-TMT-20150929-001対応 車両登録番号を販売店車両から優先して取得する END
                .AppendLine("             , NVL(T1.PARKINGCODE, ' ') AS PARKINGCODE ")
                .AppendLine("             , T1.VISITTIMESTAMP ")
                .AppendLine("             , T2.USERNAME ")
                .AppendLine("             , NVL(NVL(TRIM(S3.MODEL_NAME), TRIM(T4.MODEL_NAME)), TRIM(S2.NEWCST_MODEL_NAME)) AS MODEL_NAME ")
                .AppendLine("             FROM ")
                .AppendLine("               TBL_SERVICE_VISIT_MANAGEMENT T1 ")
                .AppendLine("             , TBL_USERS T2 ")
                .AppendLine("             , TB_M_KATASHIKI T3 ")
                .AppendLine("             , TB_M_MODEL T4 ")
                .AppendLine("             , TB_T_SERVICEIN S1 ")
                .AppendLine("             , TB_M_VEHICLE S2 ")
                .AppendLine("             , TB_M_MODEL S3 ")
                '2015/11/11 TM 皆川 TR-SVT-TMT-20150929-001対応 車両登録番号を販売店車両から優先して取得する START
                .AppendLine("             , TB_M_VEHICLE_DLR S7 ")
                '2015/11/11 TM 皆川 TR-SVT-TMT-20150929-001対応 車両登録番号を販売店車両から優先して取得する END
                .AppendLine("            WHERE ")
                .AppendLine("               T1.SACODE=T2.ACCOUNT(+) ")
                '2015/11/11 TM 皆川 TR-SVT-TMT-20150929-001対応 車両登録番号を販売店車両から優先して取得する START
                .AppendLine("           AND T1.VCL_ID = S7.VCL_ID(+)  ")
                '2015/11/11 TM 皆川 TR-SVT-TMT-20150929-001対応 車両登録番号を販売店車両から優先して取得する END
                .AppendLine("           AND T1.MODELCODE = T3.VCL_KATASHIKI(+)  ")
                .AppendLine("           AND T3.MODEL_CD = T4.MODEL_CD(+) ")
                .AppendLine("           AND T1.FREZID = S1.SVCIN_ID(+) ")
                .AppendLine("           AND S1.VCL_ID = S2.VCL_ID(+)  ")
                .AppendLine("           AND S2.MODEL_CD = S3.MODEL_CD(+) ")
                .AppendLine("           AND T1.DLRCD =:DLRCD ")
                .AppendLine("           AND T1.STRCD =:STRCD ")
                .AppendLine("           AND S1.DLR_CD(+) = :DLRCD  ")
                .AppendLine("           AND S1.BRN_CD(+) = :STRCD ")
                .AppendLine("           AND T1.VISITTIMESTAMP>= TRUNC(SYSDATE)-:NDAYS ")

                '2015/03/02 TMEJ 小澤 DMS連携版タブレット 来店情報異常時 チップ表示制御追加開発 START
                '.AppendLine("           AND T1.ASSIGNSTATUS= :ASSIGNSTATUS ")
                .AppendLine("           AND T1.ASSIGNSTATUS IN (:ASSIGNSTATUS_0, :ASSIGNSTATUS_1, :ASSIGNSTATUS_2) ")
                '2015/03/02 TMEJ 小澤 DMS連携版タブレット 来店情報異常時 チップ表示制御追加開発 END

                .AppendLine("           AND NOT EXISTS( ")

                '2015/03/02 TMEJ 小澤 DMS連携版タブレット 来店情報異常時 チップ表示制御追加開発 START
                '.AppendLine("                       SELECT 1 ")
                '.AppendLine("                         FROM ")
                '.AppendLine("                           TB_T_SERVICEIN T4 ")
                '.AppendLine("                         , TB_T_JOB_DTL T5 ")
                '.AppendLine("                         , TB_T_JOB_RESULT T6 ")
                '.AppendLine("                        WHERE ")
                '.AppendLine("                           T1.FREZID = T4.SVCIN_ID ")
                '.AppendLine("                       AND T4.SVCIN_ID = T5.SVCIN_ID  ")
                '.AppendLine("                       AND T5.JOB_DTL_ID = T6.JOB_DTL_ID  ")
                '.AppendLine("                       AND T5.DLR_CD =:DLRCD  ")
                '.AppendLine("                       AND T5.BRN_CD =:STRCD  ")

                .AppendLine("                       SELECT 1 ")
                .AppendLine("                         FROM ")
                .AppendLine("                           TB_T_JOB_DTL T4 ")
                .AppendLine("                         , TB_T_JOB_RESULT T5 ")
                .AppendLine("                        WHERE ")
                .AppendLine("                           T4.JOB_DTL_ID = T5.JOB_DTL_ID  ")
                .AppendLine("                       AND T4.SVCIN_ID = T1.FREZID ")
                .AppendLine("                       AND T4.DLR_CD =:DLRCD  ")
                .AppendLine("                       AND T4.BRN_CD =:STRCD  ")
                '2015/03/02 TMEJ 小澤 DMS連携版タブレット 来店情報異常時 チップ表示制御追加開発 END

                .AppendLine("                   ) ")
                '2015/11/11 TM 皆川 TR-SVT-TMT-20150929-001対応 車両登録番号を販売店車両から優先して取得する START
                .AppendLine("           AND S7.DLR_CD(+) = :DLRCD  ")
                '2015/11/11 TM 皆川 TR-SVT-TMT-20150929-001対応 車両登録番号を販売店車両から優先して取得する END
                .AppendLine("            UNION ")
                .AppendLine("           SELECT ")
                .AppendLine("               T7.VISITSEQ ")
                .AppendLine("             , T7.ASSIGNTIMESTAMP ")
                '2015/11/11 TM 皆川 TR-SVT-TMT-20150929-001対応 車両登録番号を販売店車両から優先して取得する START
                '.AppendLine("             , T7.VCLREGNO ")
                .AppendLine("             , NVL(TRIM(S8.REG_NUM), T7.VCLREGNO) AS VCLREGNO ")
                '2015/11/11 TM 皆川 TR-SVT-TMT-20150929-001対応 車両登録番号を販売店車両から優先して取得する END
                .AppendLine("             , NVL(T7.PARKINGCODE, ' ') AS PARKINGCODE ")
                .AppendLine("             , T7.VISITTIMESTAMP ")
                .AppendLine("             , T8.USERNAME ")
                .AppendLine("             , NVL(NVL(TRIM(S6.MODEL_NAME), TRIM(T11.MODEL_NAME)), TRIM(S5.NEWCST_MODEL_NAME)) AS MODEL_NAME ")
                .AppendLine("             FROM ")
                .AppendLine("               TBL_SERVICE_VISIT_MANAGEMENT T7 ")
                .AppendLine("             , TBL_USERS T8 ")
                .AppendLine("             , TB_T_RO_INFO T9 ")
                .AppendLine("             , TB_M_KATASHIKI T10 ")
                .AppendLine("             , TB_M_MODEL T11 ")
                .AppendLine("             , TB_T_SERVICEIN S4 ")
                .AppendLine("             , TB_M_VEHICLE S5 ")
                .AppendLine("             , TB_M_MODEL S6 ")
                '2015/11/11 TM 皆川 TR-SVT-TMT-20150929-001対応 車両登録番号を販売店車両から優先して取得する START
                .AppendLine("             , TB_M_VEHICLE_DLR S8 ")
                '2015/11/11 TM 皆川 TR-SVT-TMT-20150929-001対応 車両登録番号を販売店車両から優先して取得する END
                .AppendLine("            WHERE ")
                .AppendLine("               T7.SACODE=T8.ACCOUNT(+) ")
                '2015/11/11 TM 皆川 TR-SVT-TMT-20150929-001対応 車両登録番号を販売店車両から優先して取得する START
                .AppendLine("           AND T7.VCL_ID = S8.VCL_ID(+)  ")
                '2015/11/11 TM 皆川 TR-SVT-TMT-20150929-001対応 車両登録番号を販売店車両から優先して取得する END
                .AppendLine("           AND T7.MODELCODE=T10.VCL_KATASHIKI(+) ")
                .AppendLine("           AND T10.MODEL_CD = T11.MODEL_CD(+)  ")
                .AppendLine("           AND T7.FREZID = S4.SVCIN_ID(+) ")
                .AppendLine("           AND S4.VCL_ID = S5.VCL_ID(+) ")
                .AppendLine("           AND S5.MODEL_CD = S6.MODEL_CD(+) ")
                .AppendLine("           AND T7.VISITSEQ = T9.VISIT_ID ")
                .AppendLine("           AND T7.DLRCD =:DLRCD ")
                .AppendLine("           AND T7.STRCD =:STRCD ")
                .AppendLine("           AND T9.DLR_CD =:DLRCD ")
                .AppendLine("           AND T9.BRN_CD =:STRCD ")
                .AppendLine("           AND S4.DLR_CD(+) =:DLRCD ")
                .AppendLine("           AND S4.BRN_CD(+) =:STRCD ")
                .AppendLine("           AND T7.VISITTIMESTAMP< TRUNC(SYSDATE)-:NDAYS ")

                '2015/03/02 TMEJ 小澤 DMS連携版タブレット 来店情報異常時 チップ表示制御追加開発 START
                '.AppendLine("           AND T7.ASSIGNSTATUS= :ASSIGNSTATUS ")
                .AppendLine("           AND T7.ASSIGNSTATUS= :ASSIGNSTATUS_2 ")
                '2015/03/02 TMEJ 小澤 DMS連携版タブレット 来店情報異常時 チップ表示制御追加開発 END

                .AppendLine("           AND T9.RO_STATUS =:RO_STATUS ")

                .AppendLine("           AND NOT EXISTS( ")

                '2015/03/02 TMEJ 小澤 DMS連携版タブレット 来店情報異常時 チップ表示制御追加開発 START
                '.AppendLine("                       SELECT 1 ")
                '.AppendLine("                         FROM ")
                '.AppendLine("                           TB_T_SERVICEIN T11 ")
                '.AppendLine("                         , TB_T_JOB_DTL T12 ")
                '.AppendLine("                         , TB_T_JOB_RESULT T13 ")
                '.AppendLine("                        WHERE ")
                '.AppendLine("                           T7.FREZID = T11.SVCIN_ID ")
                '.AppendLine("                       AND T11.SVCIN_ID = T12.SVCIN_ID  ")
                '.AppendLine("                       AND T12.JOB_DTL_ID = T13.JOB_DTL_ID  ")
                '.AppendLine("                       AND T12.DLR_CD =:DLRCD  ")
                '.AppendLine("                       AND T12.BRN_CD =:STRCD  ")

                .AppendLine("                       SELECT 1 ")
                .AppendLine("                         FROM ")
                .AppendLine("                           TB_T_JOB_DTL T11 ")
                .AppendLine("                         , TB_T_JOB_RESULT T12 ")
                .AppendLine("                        WHERE ")
                .AppendLine("                           T11.JOB_DTL_ID = T12.JOB_DTL_ID  ")
                .AppendLine("                       AND T11.SVCIN_ID = T7.FREZID ")
                .AppendLine("                       AND T11.DLR_CD =:DLRCD  ")
                .AppendLine("                       AND T11.BRN_CD =:STRCD  ")
                '2015/03/02 TMEJ 小澤 DMS連携版タブレット 来店情報異常時 チップ表示制御追加開発 END

                .AppendLine("                   ) ")
                '2015/11/11 TM 皆川 TR-SVT-TMT-20150929-001対応 車両登録番号を販売店車両から優先して取得する START
                .AppendLine("           AND S8.DLR_CD(+) = :DLRCD  ")
                '2015/11/11 TM 皆川 TR-SVT-TMT-20150929-001対応 車両登録番号を販売店車両から優先して取得する END
                .AppendLine("         GROUP BY ")
                .AppendLine("               T7.VISITSEQ ")
                .AppendLine("           ,   T7.ASSIGNTIMESTAMP ")

                '2015/11/11 TM 皆川 TR-SVT-TMT-20150929-001対応 車両登録番号を販売店車両から優先して取得する START
                '.AppendLine("           ,   T7.VCLREGNO ")
                .AppendLine("           ,   NVL(TRIM(S8.REG_NUM), T7.VCLREGNO) ")
                '2015/11/11 TM 皆川 TR-SVT-TMT-20150929-001対応 車両登録番号を販売店車両から優先して取得する END

                .AppendLine("           ,   T7.PARKINGCODE ")
                .AppendLine("           ,   T7.VISITTIMESTAMP ")
                .AppendLine("           ,   T8.USERNAME ")
                .AppendLine("           ,   NVL(NVL(TRIM(S6.MODEL_NAME), TRIM(T11.MODEL_NAME)), TRIM(S5.NEWCST_MODEL_NAME)) ")
                .AppendLine("         ) T14  ")
                .AppendLine(" ORDER BY  ")
                .AppendLine("       DECODE(PARKINGCODE, ' ', '0', '1') ASC  ")

                '2015/03/02 TMEJ 小澤 DMS連携版タブレット 来店情報異常時 チップ表示制御追加開発 START
                '.AppendLine("     , ASSIGNTIMESTAMP ASC  ")

                .AppendLine("     , VISITTIMESTAMP ASC  ")
                '2015/03/02 TMEJ 小澤 DMS連携版タブレット 来店情報異常時 チップ表示制御追加開発 END

            End With

            Dim dt As SC3320101DataSet.SC3320101VisitInfoDataTable = Nothing
            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3320101DataSet.SC3320101VisitInfoDataTable)("SC3320101_001")

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, inDlrCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, inBrnCode)
                query.AddParameterWithTypeValue("NDAYS", OracleDbType.NVarchar2, settingVal)

                '2015/03/02 TMEJ 小澤 DMS連携版タブレット 来店情報異常時 チップ表示制御追加開発 START
                'query.AddParameterWithTypeValue("ASSIGNSTATUS", OracleDbType.NVarchar2, ASSIGNSTATUS_ASSIGNED)
                query.AddParameterWithTypeValue("ASSIGNSTATUS_0", OracleDbType.NVarchar2, ASSIGNSTATUS_NONEASSIGN)
                query.AddParameterWithTypeValue("ASSIGNSTATUS_1", OracleDbType.NVarchar2, ASSIGNSTATUS_ASSIGNWAIT)
                query.AddParameterWithTypeValue("ASSIGNSTATUS_2", OracleDbType.NVarchar2, ASSIGNSTATUS_ASSIGNED)
                '2015/03/02 TMEJ 小澤 DMS連携版タブレット 来店情報異常時 チップ表示制御追加開発 END

                query.AddParameterWithTypeValue("RO_STATUS", OracleDbType.NVarchar2, RO_STATUS_CONFRIMED)

                dt = query.GetData()
                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                          "{0}_E, COUNT={1}", _
                                          System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                          dt.Count))

                ' 検索結果の返却
                Return dt

            End Using

        End Function

#End Region

        '#Region "システム設定値取得"
        '        ''' <summary>
        '        ''' システム設定から設定値を取得する
        '        ''' </summary>
        '        ''' <param name="settingName">システム設定名</param>
        '        ''' <param name="inDlrCode">販売店コード</param>
        '        ''' <param name="inBrnCode">店舗コード</param>
        '        ''' <returns></returns>
        '        ''' <remarks></remarks>
        '        Public Function GetSystemSettingValue(ByVal settingName As String, _
        '                                              ByVal inDlrCode As String, _
        '                                              ByVal inBrnCode As String) As SC3320101DataSet.SC3320101SystemSettingDataTable

        '            Logger.Info(String.Format(CultureInfo.CurrentCulture, _
        '                                      "{0}.{1} P1:{2} ", _
        '                                      Me.GetType.ToString, _
        '                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
        '                                      settingName))

        '            Dim sql As New StringBuilder
        '            With sql
        '                .AppendLine(" SELECT /* SC3320101_002 */ ")
        '                .AppendLine(" 		 PARAMVALUE AS SETTING_VAL")
        '                .AppendLine("   FROM ")
        '                .AppendLine(" 		 TBL_DLRENVSETTING ")
        '                .AppendLine("　WHERE STRCD IN (:DLR_CD, N'XXXXX') ")
        '                .AppendLine("    AND DLRCD IN (:BRN_CD, N'XXX') ")
        '                .AppendLine(" 	 AND PARAMNAME = :SETTING_NAME ")
        '            End With

        '            Dim dt As SC3320101DataSet.SC3320101SystemSettingDataTable = Nothing

        '            Using query As New DBSelectQuery(Of SC3320101DataSet.SC3320101SystemSettingDataTable)("SC3320101_002")
        '                query.CommandText = sql.ToString()
        '                query.AddParameterWithTypeValue("SETTING_NAME", OracleDbType.NVarchar2, settingName)
        '                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, inDlrCode)
        '                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, inBrnCode)

        '                dt = query.GetData()
        '            End Using

        '            Logger.Info(String.Format(CultureInfo.CurrentCulture, _
        '                                      "{0}.{1} COUNT = {2}", _
        '                                      Me.GetType.ToString, _
        '                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
        '                                      dt.Count))

        '            Return dt

        '        End Function

        '#End Region

#End Region

#Region "Update"

#Region "ロケーションコード更新"
        ''' <summary>
        ''' 作業内容を更新する
        ''' </summary>
        ''' <param name="drVisitInfo">来店情報データ行</param>
        ''' <param name="updateDate">登録用更新日時</param>
        ''' <param name="updateAccount">更新アカウント</param>
        ''' <returns>更新件数</returns>
        ''' <remarks></remarks>
        Public Function UpdParkingInfo(ByVal drVisitInfo As SC3320101DataSet.SC3320101VisitInfoRow, _
                                       ByVal updateDate As Date, _
                                       ByVal updateAccount As String) As Integer

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                          "{0}_S. VISITSEQ={1}, updateDate={2}, updateDate={3}", _
                                          System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                          drVisitInfo.VISITSEQ, _
                                          updateDate, _
                                          updateAccount))

            'SQL組み立て
            Dim sql As New StringBuilder
            With sql
                .Append(" UPDATE /* SC3320101_101 */ ")
                .Append("        TBL_SERVICE_VISIT_MANAGEMENT ")                  '来店サービス管理テーブル
                .Append("    SET ")
                .Append("        PARKINGCODE = :PARKINGCODE ")               'ロケーションコード
                .Append("      , UPDATEDATE = :UPDATEDATE ")                       '更新日時
                .Append("      , UPDATEACCOUNT = :UPDATEACCOUNT ")            '更新アカウント
                .Append("      , UPDATEID = :UPDATEID ")           '更新プログラムID
                .Append("  WHERE ")
                .Append("        VISITSEQ = :VISITSEQ ")            '来店シーケンス
            End With

            Using query As New DBUpdateQuery("SC3320101_101")
                query.CommandText = sql.ToString()
                '表示サービス分類ID
                query.AddParameterWithTypeValue("PARKINGCODE", OracleDbType.NVarchar2, drVisitInfo.PARKINGCODE)             'ロケーションコード
                query.AddParameterWithTypeValue("UPDATEDATE", OracleDbType.Date, updateDate)             '更新日時
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.NVarchar2, updateAccount)             '更新アカウント
                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.NVarchar2, MY_PROGRAMID)             '更新プログラムID
                query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Long, drVisitInfo.VISITSEQ)             '来店シーケンス

                'SQL実行
                Dim queryCount As Integer = query.Execute()

                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E", System.Reflection.MethodBase.GetCurrentMethod.Name))

                Return queryCount

            End Using

        End Function
#End Region

#End Region

    End Class

End Namespace
