Imports Oracle.DataAccess.Client
Imports System.Text
Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Globalization
Imports System.Reflection

Namespace Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess

    ''' <summary>
    ''' TBL_DLRENVSETTINGからデータを取得する共通クラスです。
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class DlrEnvSettingTableAdapter

        Private Sub New()

        End Sub

        ''' <summary>
        ''' TBL_DLRENVSETTINGから指定データを取得します。
        ''' </summary>
        ''' <param name="dlrCd">販売店コード</param>
        ''' <param name="strCd">店舗コード</param>
        ''' <param name="param">パラメーター</param>
        ''' <returns>DLRENVSETTINGDataTable</returns>
        ''' <remarks>
        ''' TBL_DLRENVSETTINGから指定データを取得します。
        ''' </remarks>
        Public Shared Function GetDlrEnvSettingDataTable(ByVal dlrCD As String,
                                                  ByVal strCD As String,
                                                  ByVal param As String) As DlrEnvSettingDataSet.DLRENVSETTINGDataTable

            ' 2013/06/30 TCS 坂井 2013/10対応版　既存流用 START
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      " {0}_Start, dlrCD:[{1}], strCD:[{2}], param:[{3}]",
                                      MethodBase.GetCurrentMethod.Name,
                                      dlrCD,
                                      strCD,
                                      param))
            ' ======================== ログ出力 終了 ========================
            ' 2013/06/30 TCS 坂井 2013/10対応版　既存流用 END

            Using query As New DBSelectQuery(Of DlrEnvSettingDataSet.DLRENVSETTINGDataTable)("DLRENVSETTING_001")

                Dim sql As New StringBuilder

                '2013/06/30 TCS 坂井 2013/10対応版 既存流用 START
                With sql
                    .Append(" SELECT /* DLRENVSETTING_001 */ ")
                    .Append("     ID ")
                    .Append("   , DLRCD ")
                    .Append("   , STRCD ")
                    .Append("   , PARAMNAME ")
                    .Append("   , PARAMVALUE ")
                    .Append("   , CREATEDATE ")
                    .Append("   , UPDATEDATE ")
                    .Append("   , UPDATEACCOUNT ")
                    .Append(" FROM ")
                    .Append("     TBL_DLRENVSETTING T1 ")
                    .Append(" WHERE ")
                    .Append("     T1.DLRCD = :DLRCD ")
                    .Append(" AND T1.STRCD = :STRCD ")
                    .Append(" AND T1.PARAMNAME = :PARAMNAME ")
                End With

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrCD)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strCD)
                query.AddParameterWithTypeValue("PARAMNAME", OracleDbType.Varchar2, param)

                ' 2013/06/30 TCS 坂井 2013/10対応版　既存流用 START
                Dim dt As DlrEnvSettingDataSet.DLRENVSETTINGDataTable = query.GetData()

                ' ======================== ログ出力 開始 ========================
                Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                          " {0}_End, Return:[{1}]",
                                          MethodBase.GetCurrentMethod.Name, dt.Rows.Count))
                ' ======================== ログ出力 終了 ========================
                Return dt
                '2013/06/30 TCS 坂井 2013/10対応版 既存流用 END
            End Using

        End Function

    End Class

End Namespace