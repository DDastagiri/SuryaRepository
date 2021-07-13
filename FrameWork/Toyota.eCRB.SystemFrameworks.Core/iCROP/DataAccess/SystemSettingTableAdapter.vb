Imports Oracle.DataAccess.Client
Imports System.Text
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports System.Globalization
Imports System.Reflection

Namespace Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess


    ''' <summary>
    ''' TB_M_SYSTEM_SETTINGからデータを取得する共通クラスです。
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class SystemSettingTableAdapter

        Private Sub New()

        End Sub

        ''' <summary>
        ''' TB_M_SYSTEM_SETTINGから指定データを取得します。
        ''' </summary>
        ''' <param name="param">パラメーター</param>
        ''' <returns>TB_M_SYSTEM_SETTINGDataTable</returns>
        ''' <remarks>
        ''' TB_M_SYSTEM_SETTINGから指定データを取得します。
        ''' </remarks>
        Public Shared Function GetSystemSettingDataTable(
                    ByVal param As String) As SystemSettingDataSet.TB_M_SYSTEM_SETTINGDataTable

            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      " {0}_Start, param:[{1}]",
                                      MethodBase.GetCurrentMethod.Name,
                                      param))
            ' ======================== ログ出力 終了 ========================

            Using query As New DBSelectQuery(Of SystemSettingDataSet.TB_M_SYSTEM_SETTINGDataTable)("TB_M_SYSTEM_SETTING_001")

                Dim sql As New StringBuilder

                With sql
                    .Append(" SELECT /* TB_M_SYSTEM_SETTING_001 */ ")
                    .Append("     SETTING_NAME")
                    .Append("   , SETTING_VAL")
                    .Append(" FROM ")
                    .Append("     TB_M_SYSTEM_SETTING T1 ")
                    .Append(" WHERE ")
                    .Append("     T1.SETTING_NAME = :PARAMNAME ")
                End With

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("PARAMNAME", OracleDbType.Varchar2, param)

                Dim dt As SystemSettingDataSet.TB_M_SYSTEM_SETTINGDataTable = query.GetData()

                ' ======================== ログ出力 開始 ========================
                Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                          " {0}_End, Return:[{1}]",
                                          MethodBase.GetCurrentMethod.Name, dt.Rows.Count))
                ' ======================== ログ出力 終了 ========================
                Return dt
            End Using

        End Function


    End Class

End Namespace

