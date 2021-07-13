'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3170211TableAdapter.vb
'─────────────────────────────────────
'機能： 商品紹介機能開発(RO)
'補足： 写真表示ポップアップ
'作成： 2014/02/18 SKFC 久代
'更新： 
'─────────────────────────────────────
Option Strict On
Option Explicit On

Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core

Public Class SC3170211TableAdapter

#Region "定数"
    ''' <summary>
    ''' プログラムID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const _PROGRAM_ID As String = "SC3170211"
#End Region

#Region "公開メソッド"
    ''' <summary>
    ''' プログラム設定取得
    ''' </summary>
    ''' <param name="SETTING_SECTION"></param>
    ''' <param name="SETTING_KEY"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetProgramSetting(ByVal SETTING_SECTION As String,
                                             ByVal SETTING_KEY As String) As String
        Logger.Info("SC3170211TableAdapter.GetProgramSetting function Begin.")

        Dim result As String = ""
        Dim ds As SC3170211DataSet.TB_M_PROGRAM_SETTINGDataTable

        Using query As New DBSelectQuery(Of SC3170211DataSet.TB_M_PROGRAM_SETTINGDataTable)("SC3170211_001")
            Dim sql As New StringBuilder
            With sql
                .Append(" SELECT /* SC3170211_001 */ ")
                .Append("        SETTING_VAL ")
                .Append(" FROM   TB_M_PROGRAM_SETTING ")
                .Append(" WHERE  PROGRAM_CD      = :PROGRAM_CD ")
                .Append(" AND    SETTING_SECTION = :SETTING_SECTION ")
                .Append(" AND    SETTING_KEY     = :SETTING_KEY ")
            End With

            query.CommandText = sql.ToString()
            'バインド変数
            query.AddParameterWithTypeValue("PROGRAM_CD", OracleDbType.NVarchar2, _PROGRAM_ID)
            query.AddParameterWithTypeValue("SETTING_SECTION", OracleDbType.NVarchar2, SETTING_SECTION)
            query.AddParameterWithTypeValue("SETTING_KEY", OracleDbType.NVarchar2, SETTING_KEY)

            ds = query.GetData()
            If 1 = ds.Count Then
                result = ds.Item(0).SETTING_VAL
            End If
        End Using

        Logger.Info("SC3170211TableAdapter.GetProgramSetting function End.")

        Return result
    End Function
#End Region

End Class
