'-------------------------------------------------------------------------
'IC3802701TableAdapter.vb
'-------------------------------------------------------------------------
'機能：Jobdispatch実績送信
'補足：
'作成：2013/12/26 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発
'更新：
'─────────────────────────────────────

Option Strict On
Option Explicit On

Imports Oracle.DataAccess.Client
Imports System.Text
Imports System.Globalization
Imports System.Reflection
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.DMSLinkage.JobDispatchResult.Api.DataAccess.IC3802701DataSet


Namespace IC3802701DataSetTableAdapters
    Public Class IC3802701DataTableAdapter
        Inherits Global.System.ComponentModel.Component

#Region "定数"
        ''' <summary>
        ''' 関連チップ送信フラグ：送信する
        ''' </summary>
        ''' <remarks></remarks>
        Private Const SendRelationChipFlg_Send As String = "0"

        ''' <summary>
        ''' 関連チップ送信フラグ：送信しない
        ''' </summary>
        ''' <remarks></remarks>
        Private Const SendRelationChipFlg_NotSend As String = "1"
#End Region

        ''' <summary>
        ''' サービス基幹連携送信設定から送信フラグを取得する
        ''' </summary>
        ''' <param name="inDealerCD">販売店コード</param>
        ''' <param name="inBranchCD">店舗コード</param>
        ''' <param name="inAllDealerCD">全販売店を示すコード</param>
        ''' <param name="inAllBranchCD">全店舗を示すコード</param>
        ''' <param name="inInterfaceType">インターフェース区分(1:予約送信/2:ステータス送信/3:作業実績送信)</param>
        ''' <param name="inPrevStatus">更新前サービス連携ステータス</param>
        ''' <param name="inCrntStatus">更新後サービス連携ステータス</param>
        ''' <returns>0:送信しない/1:送信する/Empty:取得できなかった</returns>
        ''' <remarks></remarks>
        Public Function GetLinkSettings(ByVal inDealerCD As String, _
                                        ByVal inBranchCD As String, _
                                        ByVal inAllDealerCD As String, _
                                        ByVal inAllBranchCD As String, _
                                        ByVal inInterfaceType As String, _
                                        ByVal inPrevStatus As String, _
                                        ByVal inCrntStatus As String) As IC3802701LinkSendSettingsDataTable

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}_S inDealerCD={1}, inBranchCD={2}, inAllDealerCD={3}, inAllBranchCD={4}, inInterfaceType={5}, inPrevStatus={6}, inCrntStatus={7}", _
                                      MethodBase.GetCurrentMethod.Name, _
                                      inDealerCD, _
                                      inBranchCD, _
                                      inAllDealerCD, _
                                      inAllBranchCD, _
                                      inInterfaceType, _
                                      inPrevStatus, _
                                      inCrntStatus))

            Dim sql As New StringBuilder
            With sql
                .AppendLine("   SELECT /* IC3802701_001 */ ")
                .AppendLine(" 		   SEND_FLG ")
                .AppendLine(" 		 , DLR_CD ")
                .AppendLine(" 		 , BRN_CD ")
                .AppendLine("     FROM ")
                .AppendLine(" 		   TB_M_SVC_LINK_SEND_SETTING ")
                .AppendLine("    WHERE ")
                .AppendLine(" 		   DLR_CD IN (:DLR_CD, :ALL_DLR_CD) ")
                .AppendLine(" 	   AND BRN_CD IN (:BRN_CD, :ALL_BRN_CD) ")
                .AppendLine(" 	   AND INTERFACE_TYPE = :INTERFACE_TYPE ")
                .AppendLine(" 	   AND BEFORE_SVC_LINK_STATUS = :BEFORE_SVC_LINK_STATUS ")
                .AppendLine(" 	   AND AFTER_SVC_LINK_STATUS = :AFTER_SVC_LINK_STATUS ")
                .AppendLine(" ORDER BY ")
                .AppendLine("          DLR_CD ASC, BRN_CD ASC ")
            End With

            Dim returnTable As IC3802701LinkSendSettingsDataTable = Nothing

            Using query As New DBSelectQuery(Of IC3802701LinkSendSettingsDataTable)("IC3802701_001")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, inDealerCD)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, inBranchCD)
                query.AddParameterWithTypeValue("ALL_DLR_CD", OracleDbType.NVarchar2, inAllDealerCD)
                query.AddParameterWithTypeValue("ALL_BRN_CD", OracleDbType.NVarchar2, inAllBranchCD)
                query.AddParameterWithTypeValue("INTERFACE_TYPE", OracleDbType.NVarchar2, inInterfaceType)
                query.AddParameterWithTypeValue("BEFORE_SVC_LINK_STATUS", OracleDbType.NVarchar2, inPrevStatus)
                query.AddParameterWithTypeValue("AFTER_SVC_LINK_STATUS", OracleDbType.NVarchar2, inCrntStatus)

                returnTable = query.GetData()
            End Using

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}_E RowCount={1}", _
                                      MethodBase.GetCurrentMethod.Name, _
                                      returnTable.Rows.Count))

            Return returnTable

        End Function

        ''' <summary>
        ''' 指定サービス入庫IDのDispatchInfoタブ情報を取得する
        ''' </summary>
        ''' <param name="inSvcinId">サービス入庫ID</param>
        ''' <returns>DispatchInfo情報</returns>
        ''' <remarks></remarks>
        Public Function GetDispatchInfoBySvcinId(ByVal inSvcinId As Decimal) As IC3802701DispatchInfoDataTable

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}_S inSvcinId={1}", _
                                      MethodBase.GetCurrentMethod.Name, _
                                      inSvcinId))

            Dim sql As New StringBuilder
            With sql
                .AppendLine("   SELECT /* IC3802701_002 */ ")
                .AppendLine(" 		   T1.RO_NUM  ")
                .AppendLine(" 		 , T2.JOB_DTL_ID  ")
                .AppendLine(" 		 , T2.DMS_JOB_DTL_ID  ")
                .AppendLine(" 		 , T3.VCL_VIN  ")
                .AppendLine("     FROM ")
                .AppendLine(" 		   TB_T_SERVICEIN T1 ")
                .AppendLine(" 		 , TB_T_JOB_DTL T2 ")
                .AppendLine(" 		 , TB_M_VEHICLE T3 ")
                .AppendLine("    WHERE ")
                .AppendLine(" 		   T1.SVCIN_ID = T2.SVCIN_ID  ")
                .AppendLine(" 	   AND T1.VCL_ID = T3.VCL_ID(+)  ")
                .AppendLine(" 	   AND T2.CANCEL_FLG = N'0'  ")
                .AppendLine(" 	   AND T1.SVCIN_ID = :SVCIN_ID  ")
                .AppendLine(" 	 ORDER BY   ")
                .AppendLine(" 	       JOB_DTL_ID   ")
            End With

            Dim getTable As IC3802701DispatchInfoDataTable = Nothing

            Using query As New DBSelectQuery(Of IC3802701DispatchInfoDataTable)("IC3802701_002")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, inSvcinId)

                getTable = query.GetData()
            End Using

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}_E RowCount={1}", _
                                      MethodBase.GetCurrentMethod.Name, _
                                      getTable.Rows.Count))

            Return getTable

        End Function


        ''' <summary>
        ''' 指定作業内容IDのDetailタブの情報を取得する
        ''' </summary>
        ''' <param name="inJobDtlId">作業内容ID</param>
        ''' <returns>作業詳細情報</returns>
        ''' <remarks></remarks>
        Public Function GetJobDetailByJobDtlId(ByVal inJobDtlId As Decimal) As IC3802701JobDetailDataTable

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}_S inJobDtlId={1}", _
                                      MethodBase.GetCurrentMethod.Name, _
                                      inJobDtlId))

            Dim sql As New StringBuilder
            With sql
                .AppendLine("   SELECT /* IC3802701_003 */ ")
                .AppendLine(" 		   T1.JOB_DTL_ID  ")
                .AppendLine(" 		 , T1.JOB_INSTRUCT_ID  ")
                .AppendLine(" 		 , T1.JOB_INSTRUCT_SEQ  ")
                .AppendLine(" 		 , T1.JOB_CD  ")
                .AppendLine(" 		 , T1.JOB_STF_GROUP_ID  ")
                .AppendLine(" 		 , T2.JOB_STATUS  ")
                .AppendLine(" 		 , T2.RSLT_START_DATETIME  ")
                .AppendLine(" 		 , T2.RSLT_END_DATETIME  ")
                .AppendLine(" 		 , T2.STOP_REASON_TYPE  ")
                .AppendLine(" 		 , T2.STALL_ID  ")
                .AppendLine(" 		 , T3.INSPECTION_APPROVAL_STF_CD  ")
                .AppendLine(" 		 , T3.INSPECTION_NEED_FLG  ")
                .AppendLine("     FROM ")
                .AppendLine(" 		   TB_T_JOB_INSTRUCT T1 ")
                .AppendLine(" 		 , TB_T_JOB_RESULT T2 ")
                .AppendLine(" 		 , TB_T_JOB_DTL T3 ")
                .AppendLine("    WHERE ")
                .AppendLine(" 		   T1.JOB_DTL_ID = T2.JOB_DTL_ID(+)  ")
                .AppendLine(" 	   AND T1.JOB_INSTRUCT_ID = T2.JOB_INSTRUCT_ID(+)  ")
                .AppendLine(" 	   AND T1.JOB_INSTRUCT_SEQ = T2.JOB_INSTRUCT_SEQ(+)  ")
                .AppendLine(" 	   AND T3.JOB_DTL_ID = T1.JOB_DTL_ID  ")
                .AppendLine(" 	   AND T3.JOB_DTL_ID = :JOB_DTL_ID  ")
                .AppendLine(" 	 ORDER BY   ")
                .AppendLine(" 		   JOB_INSTRUCT_ID  ")
                .AppendLine(" 		 , JOB_INSTRUCT_SEQ  ")
                .AppendLine(" 		 , RSLT_START_DATETIME  ")
                .AppendLine(" 		 , RSLT_END_DATETIME  ")
            End With

            Dim getTable As IC3802701JobDetailDataTable = Nothing

            Using query As New DBSelectQuery(Of IC3802701JobDetailDataTable)("IC3802701_003")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("JOB_DTL_ID", OracleDbType.Decimal, inJobDtlId)
                getTable = query.GetData()
            End Using

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}_E RowCount={1}", _
                                      MethodBase.GetCurrentMethod.Name, _
                                      getTable.Rows.Count))
            Return getTable

        End Function

        ''' <summary>
        ''' 指定サービス入庫IDのDetailタブの情報を取得する
        ''' </summary>
        ''' <param name="inSvcinId">サービス入庫ID</param>
        ''' <returns>作業詳細情報</returns>
        ''' <remarks></remarks>
        Public Function GetJobDetailBySvcinId(ByVal inSvcinId As Decimal) As IC3802701JobDetailDataTable

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}_S inSvcinId={1}", _
                                      MethodBase.GetCurrentMethod.Name, _
                                      inSvcinId))

            Dim sql As New StringBuilder
            With sql
                .AppendLine("   SELECT /* IC3802701_004 */ ")
                .AppendLine(" 		   T1.JOB_DTL_ID  ")
                .AppendLine(" 		 , T1.JOB_INSTRUCT_ID  ")
                .AppendLine(" 		 , T1.JOB_INSTRUCT_SEQ  ")
                .AppendLine(" 		 , T1.JOB_CD  ")
                .AppendLine(" 		 , T1.JOB_STF_GROUP_ID  ")
                .AppendLine(" 		 , T2.JOB_STATUS  ")
                .AppendLine(" 		 , T2.RSLT_START_DATETIME  ")
                .AppendLine(" 		 , T2.RSLT_END_DATETIME  ")
                .AppendLine(" 		 , T2.STOP_REASON_TYPE  ")
                .AppendLine(" 		 , T2.STALL_ID  ")
                .AppendLine(" 		 , T4.INSPECTION_APPROVAL_STF_CD  ")
                .AppendLine(" 		 , T4.INSPECTION_NEED_FLG  ")
                .AppendLine("     FROM ")
                .AppendLine(" 		   TB_T_JOB_INSTRUCT T1 ")
                .AppendLine(" 		 , TB_T_JOB_RESULT T2 ")
                .AppendLine(" 		 , TB_T_SERVICEIN T3 ")
                .AppendLine(" 		 , TB_T_JOB_DTL T4 ")
                .AppendLine("    WHERE ")
                .AppendLine(" 		   T1.JOB_DTL_ID = T2.JOB_DTL_ID(+)  ")
                .AppendLine(" 	   AND T1.JOB_INSTRUCT_ID = T2.JOB_INSTRUCT_ID(+)  ")
                .AppendLine(" 	   AND T1.JOB_INSTRUCT_SEQ = T2.JOB_INSTRUCT_SEQ(+)  ")
                .AppendLine(" 	   AND T3.SVCIN_ID = T4.SVCIN_ID  ")
                .AppendLine(" 	   AND T4.JOB_DTL_ID = T1.JOB_DTL_ID  ")
                .AppendLine("      AND T4.CANCEL_FLG = N'0' ")
                .AppendLine(" 	   AND T3.SVCIN_ID = :SVCIN_ID  ")
                .AppendLine(" 	 ORDER BY   ")
                .AppendLine(" 		   JOB_INSTRUCT_ID  ")
                .AppendLine(" 		 , JOB_INSTRUCT_SEQ  ")
                .AppendLine(" 		 , RSLT_START_DATETIME  ")
                .AppendLine(" 		 , RSLT_END_DATETIME  ")
            End With

            Dim getTable As IC3802701JobDetailDataTable = Nothing

            Using query As New DBSelectQuery(Of IC3802701JobDetailDataTable)("IC3802701_004")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, inSvcinId)
                getTable = query.GetData()
            End Using

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}_E RowCount={1}", _
                                      MethodBase.GetCurrentMethod.Name, _
                                      getTable.Rows.Count))
            Return getTable

        End Function

        ''' <summary>
        ''' スタッフ作業テーブルから指定作業IDのスタッフコードを取得する
        ''' </summary>
        ''' <param name="inJobId">作業ID</param>
        ''' <returns>作業詳細情報</returns>
        ''' <remarks></remarks>
        Public Function GetTechnicianIdByJobId(ByVal inJobId As Decimal) As IC3802701StaffJobDataTable

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}_S inJobId={1}", _
                                      MethodBase.GetCurrentMethod.Name, _
                                      inJobId))

            Dim sql As New StringBuilder
            With sql
                .AppendLine("   SELECT /* IC3802701_005 */ ")
                .AppendLine("          STF_CD   ")
                .AppendLine("     FROM ")
                .AppendLine("          TB_T_STAFF_JOB ")
                .AppendLine("    WHERE ")
                .AppendLine("          JOB_ID = :JOB_ID ")
            End With

            Dim getTable As IC3802701StaffJobDataTable = Nothing

            Using query As New DBSelectQuery(Of IC3802701StaffJobDataTable)("IC3802701_005")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("JOB_ID", OracleDbType.Decimal, inJobId)
                getTable = query.GetData()
            End Using

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}_E RowCount={1}", _
                                      MethodBase.GetCurrentMethod.Name, _
                                      getTable.Rows.Count))
            Return getTable

        End Function

        ''' <summary>
        ''' 指定ストールの非稼働情報を取得する
        ''' </summary>
        ''' <param name="inStallId">ストールID</param>
        ''' <param name="inRsltStartTime">チップ実績開始時間</param>
        ''' <param name="inRsltEndTime">チップ実績終了時間</param>
        ''' <returns>非稼働情報</returns>
        ''' <remarks></remarks>
        Public Function GetIdleByStallId(ByVal inStallId As Decimal, _
                                         ByVal inRsltStartTime As Date, _
                                         ByVal inRsltEndTime As Date) As IC3802701StallIdleDataTable

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}_S inStallId={1}, inRsltStartTime={2}, inRsltEndTime={3}", _
                                      MethodBase.GetCurrentMethod.Name, _
                                      inStallId, _
                                      inRsltStartTime, _
                                      inRsltEndTime))

            Dim sql As New StringBuilder
            With sql
                .AppendLine("   SELECT /* IC3802701_006 */ ")
                .AppendLine("        STALL_ID ")
                .AppendLine("      , IDLE_START_TIME ")
                .AppendLine("      , IDLE_END_TIME ")
                .AppendLine("      , IDLE_START_DATETIME ")
                .AppendLine("      , IDLE_END_DATETIME ")
                .AppendLine("      , IDLE_TYPE ")
                .AppendLine("   FROM ")
                .AppendLine("        TB_M_STALL_IDLE ")
                .AppendLine("  WHERE ")
                .AppendLine("        CANCEL_FLG = N'0' ")
                .AppendLine("    AND STALL_ID = :STALL_ID ")
                .AppendLine("    AND ( ")
                .AppendLine("             IDLE_TYPE = N'1' ")                   '休憩エリア
                .AppendLine("             AND SETTING_UNIT_TYPE = N'1' ")
                .AppendLine("             AND TO_CHAR(IDLE_START_TIME, 'HH24MI') < :END_TIME ")
                .AppendLine("             AND TO_CHAR(IDLE_END_TIME, 'HH24MI') > :START_TIME ")
                .AppendLine("        ) ")
            End With

            Dim getTable As IC3802701StallIdleDataTable = Nothing

            Using query As New DBSelectQuery(Of IC3802701StallIdleDataTable)("IC3802701_006")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("STALL_ID", OracleDbType.Decimal, inStallId)
                query.AddParameterWithTypeValue("START_TIME", OracleDbType.Varchar2, inRsltStartTime.ToString("HHmm", CultureInfo.InvariantCulture()))
                query.AddParameterWithTypeValue("END_TIME", OracleDbType.Varchar2, inRsltEndTime.ToString("HHmm", CultureInfo.InvariantCulture()))
                getTable = query.GetData()
            End Using

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}_E RowCount={1}", _
                                      MethodBase.GetCurrentMethod.Name, _
                                      getTable.Rows.Count))
            Return getTable

        End Function

        ''' <summary>
        ''' 指定サービス入庫IDの部分ストール利用情報を取得する
        ''' </summary>
        ''' <param name="inSvcinId">サービス入庫ID</param>
        ''' <param name="inJobDtlId">作業内容ID</param>
        ''' <returns>ストール利用情報</returns>
        ''' <remarks></remarks>
        Public Function GetResultStallUseInfo(ByVal inSvcinId As Decimal, _
                                              Optional ByVal inJobDtlId As Decimal = 0) As IC3802701StallInformationDataTable

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}_S inSvcinId={1}, inJobDtlId={2}", _
                                      MethodBase.GetCurrentMethod.Name, _
                                      inSvcinId, _
                                      inJobDtlId))

            Dim sql As New StringBuilder
            With sql
                .AppendLine("   SELECT /* IC3802701_007 */ ")
                .AppendLine(" 		   T2.STALL_USE_ID  ")
                .AppendLine(" 		 , T2.STALL_ID  ")
                .AppendLine(" 		 , T2.JOB_ID  ")
                .AppendLine(" 		 , T2.RSLT_START_DATETIME  ")
                .AppendLine(" 		 , T2.RSLT_END_DATETIME  ")
                .AppendLine(" 		 , T2.PRMS_END_DATETIME  ")
                .AppendLine(" 		 , T2.STALL_USE_STATUS  ")
                .AppendLine("     FROM ")
                .AppendLine(" 		   TB_T_JOB_DTL T1 ")
                .AppendLine(" 		 , TB_T_STALL_USE T2 ")
                .AppendLine("    WHERE ")
                .AppendLine(" 		   T1.JOB_DTL_ID = T2.JOB_DTL_ID  ")
                .AppendLine(" 	   AND T1.CANCEL_FLG = N'0' ")
                .AppendLine(" 	   AND T2.RSLT_START_DATETIME <> TO_DATE('19000101000000','YYYYMMDDHH24MISS') ") '実績チップ
                If inJobDtlId = 0 Then
                    .AppendLine("  AND T1.SVCIN_ID = :SVCIN_ID  ")
                Else
                    .AppendLine("  AND T1.JOB_DTL_ID = :JOB_DTL_ID  ")
                End If
                .AppendLine(" 	 ORDER BY   ")
                .AppendLine(" 		   RSLT_START_DATETIME  ")
            End With

            Dim getTable As IC3802701StallInformationDataTable = Nothing

            Using query As New DBSelectQuery(Of IC3802701StallInformationDataTable)("IC3802701_007")
                query.CommandText = sql.ToString()

                If inJobDtlId = 0 Then
                    '関連チップ全部送信の場合
                    query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, inSvcinId)
                Else
                    '操作チップ送信の場合
                    query.AddParameterWithTypeValue("JOB_DTL_ID", OracleDbType.Decimal, inJobDtlId)
                End If

                getTable = query.GetData()
            End Using

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}_E RowCount={1}", _
                                      MethodBase.GetCurrentMethod.Name, _
                                      getTable.Rows.Count))
            Return getTable

        End Function

    End Class
End Namespace


Partial Class IC3802701DataSet

End Class
