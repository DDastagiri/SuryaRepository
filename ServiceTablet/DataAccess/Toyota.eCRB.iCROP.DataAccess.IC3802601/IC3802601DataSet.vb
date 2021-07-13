'-------------------------------------------------------------------------
'IC3802601DataSet.vb
'-------------------------------------------------------------------------
'機能：ステータス送信
'補足：
'作成：2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発
'更新：
'─────────────────────────────────────
Option Strict On
Option Explicit On

Imports Oracle.DataAccess.Client
Imports System.Text
Imports System.Globalization
Imports System.Reflection
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.DMSLinkage.StatusInfo.Api.DataAccess.IC3802601DataSet


Namespace IC3802601DataSetTableAdapters
    Public Class IC3802601DataTableAdapter
        Inherits Global.System.ComponentModel.Component

        ''' <summary>
        ''' チップ情報を取得する
        ''' </summary>
        ''' <param name="svcinId">サービス入庫ID</param>
        ''' <param name="stallUseId">ストール利用ID</param>
        ''' <param name="jobDtlId">作業内容ID</param>
        ''' <returns></returns>
        Public Function GetChipInfo(ByVal svcinId As Decimal, _
                                    ByVal stallUseId As Decimal, _
                                    ByVal jobDtlId As Decimal) As IC3802601ChipInfoDataTable

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}_S. svcinId={1}, stallUseId={2}, jobDtlId={3}", _
                                      MethodBase.GetCurrentMethod.Name, _
                                      svcinId, _
                                      stallUseId, _
                                      jobDtlId))

            Dim sql As New StringBuilder
            With sql
                .AppendLine(" SELECT /* IC3802601_002 */ ")
                .AppendLine("        T1.NEXT_SVCIN_INSPECTION_ADVICE AS INSPECTION_MEMO ")
                .AppendLine("      , T2.DMS_JOB_DTL_ID ")
                .AppendLine("      , T3.RSLT_START_DATETIME AS RSLT_START_DATETIME ")
                .AppendLine("      , T3.RSLT_END_DATETIME AS RSLT_END_DATETIME ")
                .AppendLine("      , T3.SCHE_START_DATETIME AS REZ_TIME ")
                .AppendLine("      , T3.SCHE_WORKTIME AS WORKTIME ")
                .AppendLine("      , (CASE  WHEN T3.REST_FLG=N'1' THEN N'0' WHEN T3.REST_FLG=N'0' THEN N'1' ELSE N'1' END) AS BREAK ")
                .AppendLine("      , T3.STALL_ID  ")
                .AppendLine("      , T4.RSLT_START_DATETIME AS CW_RSLT_START_DATETIME ")
                .AppendLine("      , T4.RSLT_END_DATETIME AS CW_RSLT_END_DATETIME ")
                .AppendLine("      , T5.RSLT_START_DATETIME AS IS_RSLT_START_DATETIME ")
                .AppendLine("      , T5.RSLT_END_DATETIME AS IS_RSLT_END_DATETIME ")
                .AppendLine("      , T6.JOB_DTL_ID AS MANA_JOB_DTL_ID ")
                .AppendLine("      , T6.DMS_JOB_DTL_ID AS MANA_DMS_JOB_DTL_ID ")
                .AppendLine("   FROM ")
                .AppendLine("        TB_T_SERVICEIN T1 ")
                .AppendLine("      , TB_T_JOB_DTL T2 ")
                .AppendLine("      , TB_T_STALL_USE T3 ")
                .AppendLine("      , TB_T_CARWASH_RESULT T4 ")
                .AppendLine("      , TB_T_INSPECTION_RESULT T5 ")
                .AppendLine("      , TB_T_JOB_DTL T6  ")
                .AppendLine("  WHERE ")
                .AppendLine("        T1.SVCIN_ID=T2.SVCIN_ID ")
                .AppendLine("    AND T2.JOB_DTL_ID=T3.JOB_DTL_ID ")
                .AppendLine("    AND T1.SVCIN_ID=T4.SVCIN_ID(+) ")
                .AppendLine("    AND T2.JOB_DTL_ID=T5.JOB_DTL_ID(+) ")
                .AppendLine("    AND T1.SVCIN_ID=T6.SVCIN_ID ")
                .AppendLine("    AND T2.JOB_DTL_ID=:JOB_DTL_ID ")
                .AppendLine("    AND T2.CANCEL_FLG=N'0' ")
                .AppendLine("    AND T3.STALL_USE_ID =:STALL_USE_ID ")
                .AppendLine("    AND T6.CANCEL_FLG =N'0'  ")
                .AppendLine("    AND T6.JOB_DTL_ID =(SELECT MIN(T8.JOB_DTL_ID ) ")
                .AppendLine("                        FROM TB_T_JOB_DTL T8  ")
                .AppendLine("                        WHERE T8.SVCIN_ID=:SVCIN_ID   ")
                .AppendLine("                        AND   T8.CANCEL_FLG= N'0' )  ")
            End With

            Dim getTable As IC3802601ChipInfoDataTable = Nothing

            Using query As New DBSelectQuery(Of IC3802601ChipInfoDataTable)("IC3802601_002")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Long, svcinId)
                query.AddParameterWithTypeValue("JOB_DTL_ID", OracleDbType.Long, jobDtlId)
                query.AddParameterWithTypeValue("STALL_USE_ID", OracleDbType.Long, stallUseId)

                getTable = query.GetData()
            End Using

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                              "{0}_E Count={1}", _
                              MethodBase.GetCurrentMethod.Name, _
                              getTable.Rows.Count))

            Return getTable

        End Function

        ''' <summary>
        ''' リレーションチップス情報(作業内容ID・基幹作業内容ID)を取得する
        ''' </summary>
        ''' <param name="svcinId">サービス入庫ID</param>
        ''' <param name="jobDtlId">作業内容ID</param>
        ''' <returns></returns>
        Public Function GetRelationChipInfo(ByVal svcinId As Decimal, _
                                    ByVal jobDtlId As Decimal) As IC3802601RelationChipInfoDataTable

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}_S. svcinId={1}, jobDtlId={2}", _
                                      MethodBase.GetCurrentMethod.Name, _
                                      svcinId, _
                                      jobDtlId))

            Dim sql As New StringBuilder
            With sql
                .AppendLine(" SELECT /* IC3802601_003 */ ")
                .AppendLine("        T1.JOB_DTL_ID ")
                .AppendLine("      , T1.DMS_JOB_DTL_ID ")
                .AppendLine("   FROM ")
                .AppendLine("        TB_T_JOB_DTL T1 ")
                .AppendLine("  WHERE ")
                .AppendLine("        T1.SVCIN_ID=:SVCIN_ID ")
                .AppendLine("    AND T1.CANCEL_FLG= N'0' ")
                .AppendLine("    AND T1.JOB_DTL_ID<>:JOB_DTL_ID ")
            End With

            Dim getTable As IC3802601RelationChipInfoDataTable = Nothing

            Using query As New DBSelectQuery(Of IC3802601RelationChipInfoDataTable)("IC3802601_003")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Long, svcinId)
                query.AddParameterWithTypeValue("JOB_DTL_ID", OracleDbType.Long, jobDtlId)

                getTable = query.GetData()
            End Using

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                              "{0}_E Count={1}", _
                              MethodBase.GetCurrentMethod.Name, _
                              getTable.Rows.Count))

            Return getTable

        End Function

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
                                        ByVal inCrntStatus As String) As IC3802601LinkSendSettingsDataTable

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
                .AppendLine("   SELECT /* IC3802601_004 */ ")
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

            Dim returnTable As IC3802601LinkSendSettingsDataTable = Nothing

            Using query As New DBSelectQuery(Of IC3802601LinkSendSettingsDataTable)("IC3802601_004")
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
        

    End Class
End Namespace


Partial Class IC3802601DataSet

End Class
