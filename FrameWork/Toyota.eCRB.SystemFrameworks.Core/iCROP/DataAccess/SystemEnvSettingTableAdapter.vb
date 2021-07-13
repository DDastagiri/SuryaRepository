Imports Oracle.DataAccess.Client
Imports System.Text
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports System.Globalization
Imports System.Reflection

Namespace Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess


    ''' <summary>
    ''' TBL_SYSTEMENVSETTINGからデータを取得する共通クラスです。
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class SystemEnvSettingTableAdapter

        Private Sub New()

        End Sub

        ''' <summary>
        ''' TBL_SYSTEMENVSETTINGから指定データを取得します。
        ''' </summary>
        ''' <param name="param">パラメーター</param>
        ''' <param name="cntCd">国コード</param>
        ''' <returns>SYSTEMENVSETTINGDataTable</returns>
        ''' <remarks>
        ''' TBL_SYSTEMENVSETTINGから指定データを取得します。
        ''' </remarks>
        Public Shared Function GetSystemEnvSettingDataTable(ByVal param As String,
                                                     ByVal cntCD As String) As SystemEnvSettingDataSet.SYSTEMENVSETTINGDataTable

            ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      " {0}_Start, param:[{1}], cntCD:[{2}]",
                                      MethodBase.GetCurrentMethod.Name,
                                      param,
                                      cntCD))
            ' ======================== ログ出力 終了 ========================
            ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END

            Using query As New DBSelectQuery(Of SystemEnvSettingDataSet.SYSTEMENVSETTINGDataTable)("SYSTEMENVSETTING_001")

                Dim sql As New StringBuilder

                With sql
                    ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
                    .Append(" SELECT /* SYSTEMENVSETTING_001 */ ")
                    .Append("        T1.ID ")
                    .Append("      , T1.CNTCD ")
                    .Append("      , T1.PARAMNAME ")
                    .Append("      , T1.PARAMVALUE ")
                    .Append("      , T1.CREATEDATE ")
                    .Append("      , T1.UPDATEDATE ")
                    .Append(" FROM ")
                    .Append("  TBL_SYSTEMENVSETTING T1 ")
                    .Append(" WHERE ")
                    .Append("     T1.PARAMNAME = :PARAMNAME ")
                    .Append(" AND T1.CNTCD = :CNTCD ")
                    ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END
                End With

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("PARAMNAME", OracleDbType.Char, param)
                query.AddParameterWithTypeValue("CNTCD", OracleDbType.Char, cntCD)

                ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
                Dim dt As SystemEnvSettingDataSet.SYSTEMENVSETTINGDataTable = query.GetData()

                ' ======================== ログ出力 開始 ========================
                Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                          " {0}_End, Return:[{1}]",
                                          MethodBase.GetCurrentMethod.Name, dt.Rows.Count))
                ' ======================== ログ出力 終了 ========================

                Return dt
                ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END
            End Using

        End Function

    End Class

End Namespace

