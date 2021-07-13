Imports Oracle.DataAccess.Client
Imports System.Text
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports System.Globalization
Imports System.Reflection

Namespace Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess


    ''' <summary>
    ''' TB_M_SYSTEM_SETTING_DLRからデータを取得する共通クラスです。
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class SystemSettingDlrTableAdapter

        Private Sub New()

        End Sub

        ''' <summary>
        ''' TB_M_SYSTEM_SETTING_DLRから指定データを取得します。
        ''' </summary>
        ''' <param name="dlrCd">販売店コード</param>
        ''' <param name="strCd">店舗コード</param>
        ''' <param name="param">パラメーター</param>
        ''' <returns>TB_M_SYSTEM_SETTING_DLRDataTable</returns>
        ''' <remarks>
        ''' TB_M_SYSTEM_SETTING_DLRから指定データを取得します。
        ''' </remarks>
        Public Shared Function GetSystemSettingDlrDataTable(ByVal dlrCD As String,
                                                  ByVal strCD As String,
                                                  ByVal param As String) As SystemSettingDlrDataSet.TB_M_SYSTEM_SETTING_DLRDataTable

            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      " {0}_Start, dlrCD:[{1}], strCD:[{2}], param:[{3}]",
                                      MethodBase.GetCurrentMethod.Name,
                                      dlrCD,
                                      strCD,
                                      param))
            ' ======================== ログ出力 終了 ========================

            Using query As New DBSelectQuery(Of SystemSettingDlrDataSet.TB_M_SYSTEM_SETTING_DLRDataTable)("TB_M_SYSTEM_SETTING_DLR_001")

                Dim sql As New StringBuilder

                With sql
                    .Append(" SELECT /* TB_M_SYSTEM_SETTING_DLR_001 */ ")
                    .Append("     DLR_CD ")
                    .Append("   , BRN_CD ")
                    .Append("   , SETTING_NAME ")
                    .Append("   , SETTING_VAL ")
                    .Append(" FROM ")
                    .Append("     TB_M_SYSTEM_SETTING_DLR T1 ")
                    .Append(" WHERE ")
                    .Append("     T1.DLR_CD = :DLRCD ")
                    .Append(" AND T1.BRN_CD = :STRCD ")
                    .Append(" AND T1.SETTING_NAME = :PARAMNAME ")
                End With

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrCD)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strCD)
                query.AddParameterWithTypeValue("PARAMNAME", OracleDbType.Varchar2, param)

                Dim dt As SystemSettingDlrDataSet.TB_M_SYSTEM_SETTING_DLRDataTable = query.GetData()

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

