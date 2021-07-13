'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3240701DataSet.vb
'─────────────────────────────────────
'機能： ストール使用不可設定（データアクセス）
'補足： 
'作成： 2017/08/30 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加
'更新：
'─────────────────────────────────────
Option Strict On
Option Explicit On

Imports System.Text
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Oracle.DataAccess.Client
Imports System.Globalization
Imports Toyota.eCRB.iCROP.DataAccess.SC3240701.SC3240701DataSet
Namespace SC3240701DataSetTableAdapters
    Public Class SC3240701DataAdapter

#Region "定数"


        ''' <summary>
        ''' キャンセルフラグ（0：有効）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const CANCEL_0 As String = "0"

        ''' <summary>
        ''' 非稼動区分（2：ストール使用不可）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const IDLE_TYPE_2 As String = "2"
#End Region

#Region "ストール使用不可情報取得"
        ''' <summary>
        ''' ストール使用不可情報取得
        ''' </summary>
        ''' <param name="stallIdleId">非稼働ストールID</param>
        ''' <returns>ストール非稼働マスタ</returns>
        ''' <remarks></remarks>
        Public Function GetStallUnavailableChipInfo(ByVal stallIdleId As Decimal) As StallIdleInfoDataTable

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. stallId={1}" _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name, stallIdleId))

            Dim sql As New StringBuilder
            With sql
                .AppendLine("   SELECT /* SC3240701_001 */ ")
                .AppendLine("          STALL_IDLE_ID  ")
                .AppendLine("        , STALL_ID")
                .AppendLine("        , IDLE_START_DATETIME")
                .AppendLine("        , IDLE_END_DATETIME")
                .AppendLine("        , IDLE_MEMO ")
                .AppendLine("        , ROW_LOCK_VERSION ")
                .AppendLine("     FROM ")
                .AppendLine("          TB_M_STALL_IDLE ")
                .AppendLine("    WHERE ")
                .AppendLine("          STALL_IDLE_ID = :STALL_IDLE_ID")
                .AppendLine("      AND CANCEL_FLG = :CANCEL_0")
                .AppendLine("      AND IDLE_TYPE = :IDLE_TYPE_2")
            End With

            Using query As New DBSelectQuery(Of StallIdleInfoDataTable)("SC3240701_001")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("STALL_IDLE_ID", OracleDbType.Decimal, stallIdleId)
                query.AddParameterWithTypeValue("CANCEL_0", OracleDbType.NVarchar2, CANCEL_0)
                query.AddParameterWithTypeValue("IDLE_TYPE_2", OracleDbType.NVarchar2, IDLE_TYPE_2)
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E", System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return query.GetData()
            End Using
        End Function
#End Region

    End Class
End Namespace

Partial Class SC3240701DataSet
End Class
