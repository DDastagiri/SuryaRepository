'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'ProgramSettingV4TableAdapter.vb
'─────────────────────────────────────
'機能： ProgramSettingV4
'補足： 
'作成： 2016/04/26 TCS 山口　（トライ店システム評価）他システム連携における複数店舗コード変換対応
'─────────────────────────────────────
Imports Oracle.DataAccess.Client
Imports System.Text
Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Globalization
Imports System.Reflection

Namespace Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess

    ''' <summary>
    ''' TB_M_PROGRAM_SETTINGから設定値を取得する共通クラスです。
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class ProgramSettingV4TableAdapter

        Private Sub New()

        End Sub

#Region "GetProgramSettingV4DataTable"
        ''' <summary>
        ''' TB_M_PROGRAM_SETTINGから設定値を取得。
        ''' </summary>
        ''' <param name="programCd">プログラムコード</param>
        ''' <param name="settingSection">設定セクション</param>
        ''' <param name="settingKey">設定キー</param>
        ''' <returns>PROGRAMSETTINGV4DataTable</returns>
        ''' <remarks>
        ''' TB_M_PROGRAM_SETTINGから設定値を取得します。
        ''' </remarks>
        Public Shared Function GetProgramSettingV4DataTable(ByVal programCd As String, ByVal settingSection As String,
                                                            ByVal settingKey As String) As ProgramSettingV4DataSet.PROGRAMSETTINGV4DataTable
            Using query As New DBSelectQuery(Of ProgramSettingV4DataSet.PROGRAMSETTINGV4DataTable)("PROGRAMSETTINGV4_001")

                Dim sql As New StringBuilder

                With sql
                    .Append("SELECT /* PROGRAMSETTINGV4_001 */ ")
                    .Append("    SETTING_VAL ")
                    .Append("FROM ")
                    .Append("    TB_M_PROGRAM_SETTING ")
                    .Append("WHERE ")
                    .Append("        PROGRAM_CD = :PROGRAM_CD ")
                    .Append("    AND SETTING_SECTION = :SETTING_SECTION ")
                    .Append("    AND SETTING_KEY = :SETTING_KEY ")
                End With

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("PROGRAM_CD", OracleDbType.NVarchar2, programCd)
                query.AddParameterWithTypeValue("SETTING_SECTION", OracleDbType.NVarchar2, settingSection)
                query.AddParameterWithTypeValue("SETTING_KEY", OracleDbType.NVarchar2, settingKey)

                Dim dt As ProgramSettingV4DataSet.PROGRAMSETTINGV4DataTable = query.GetData()
                ' ======================== ログ出力 開始 ========================
                Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                          " {0}_End, Return:[{1}]",
                                          MethodBase.GetCurrentMethod.Name, dt.Rows.Count))
                ' ======================== ログ出力 終了 ========================

                Return dt

            End Using

        End Function
#End Region

    End Class

End Namespace
