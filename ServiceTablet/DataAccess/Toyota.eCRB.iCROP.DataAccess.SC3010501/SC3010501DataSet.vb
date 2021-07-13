'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3010501DataSet.vb
'─────────────────────────────────────
'機能： 他システム連携画面 データセット
'補足： 
'作成： 2013/12/16 TMEJ小澤	初版作成
'更新： 
'─────────────────────────────────────

Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web
Imports System.Globalization
Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.Common.OtherLinkage.DataAccess.SC3010501DataSet


Namespace SC3010501DataSetTableAdapters
    Public Class SC3010501DataTableAdapter
        Inherits Global.System.ComponentModel.Component

#Region "定数"

#End Region

#Region "メイン"

        ''' <summary>
        ''' 画面URL情報取得
        ''' </summary>
        ''' <param name="inDisplayNumber">表示番号</param>
        ''' <returns>URL情報</returns>
        ''' <remarks></remarks>
        Public Function GetDisplayUrl(ByVal inDisplayNumber As Long) As SC3010501DisplayRelationDataTable
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} START IN:inDisplayNum = {2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , inDisplayNumber.ToString(CultureInfo.CurrentCulture)))

            'データ格納用
            Dim dt As SC3010501DisplayRelationDataTable

            Dim sql As New StringBuilder

            'SQL文作成
            With sql
                .AppendLine("SELECT /* SC301050_0011 */ ")
                .AppendLine("       T1.DMS_DISP_ID ")
                .AppendLine("      ,T1.DMS_DISP_URL ")
                .AppendLine("  FROM ")
                .AppendLine("       TB_M_DISP_RELATION T1 ")
                .AppendLine(" WHERE ")
                .AppendLine("       T1.DMS_DISP_ID = :DMS_DISP_ID ")

            End With

            Using query As New DBSelectQuery(Of SC3010501DisplayRelationDataTable)("SC3010501_001")
                query.CommandText = sql.ToString()
                'バインド変数
                query.AddParameterWithTypeValue("DMS_DISP_ID", OracleDbType.Long, inDisplayNumber)

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

Partial Class SC3010501DataSet
End Class
