Imports Oracle.DataAccess.Client
Imports System.Text
Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Globalization
Imports System.Reflection

Namespace Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess

    ''' <summary>
    ''' TBL_FUNCTIONSETTINGからデータを取得する共通クラスです。
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class FunctionSettingTableAdapter

        Private Sub New()

        End Sub

#Region "GetFunctionSettingDataTable"
        ''' <summary>
        ''' TBL_FUNCTIONSETTINGから指定データを取得します。
        ''' </summary>
        ''' <param name="dlrCd">販売店コード</param>
        ''' <param name="param">パラメーター</param>
        ''' <returns>FUNCTIONSETTINGDataTable</returns>
        ''' <remarks>
        ''' TBL_FUNCTIONSETTINGから指定データを取得します。
        ''' </remarks>
        Public Shared Function GetFunctionSettingDataTable(ByVal dlrCD As String,
                                                    ByVal param As String) As FunctionSettingDataSet.FUNCTIONSETTINGDataTable

            ' 2013/06/30 TCS 山田 2013/10対応版　既存流用 START
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      " {0}_Start",
                                      MethodBase.GetCurrentMethod.Name))
            ' ======================== ログ出力 終了 ========================
            ' 2013/06/30 TCS 山田 2013/10対応版　既存流用 END

            Using query As New DBSelectQuery(Of FunctionSettingDataSet.FUNCTIONSETTINGDataTable)("FUNCTIONSETTING_001")

                Dim sql As New StringBuilder

                '2013/06/30 TCS 山田 2013/10対応版 既存流用 START
                With sql
                    .Append(" SELECT /* FUNCTIONSETTING_001 */ ")
                    .Append("     T1.DLRCD ")
                    .Append("   , T1.FUNCPARAM ")
                    .Append("   , T1.FUNCSTATUS ")
                    .Append("   , T1.CHANGEFLG ")
                    .Append("   , T1.NAMEWORDNO ")
                    .Append("   , T1.DETAILWORDNO ")
                    .Append("   , T1.WARNINGFLG ")
                    .Append("   , T1.WARNINGWORDNO ")
                    .Append("   , T1.SEQNO ")
                    .Append("   , T1.CREATEDATE ")
                    .Append("   , T1.UPDATEDATE ")
                    .Append("   , T1.UPDATEACCOUNT ")
                    .Append(" FROM ")
                    .Append("  TBL_FUNCTIONSETTING T1 ")
                    .Append(" WHERE ")
                    .Append("     T1.DLRCD = :DLRCD ")
                    .Append(" AND T1.FUNCPARAM = :FUNCPARAM ")
                End With
                '2013/06/30 TCS 山田 2013/10対応版 既存流用 END

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrCD)
                query.AddParameterWithTypeValue("FUNCPARAM", OracleDbType.Varchar2, param)

                ' 2013/06/30 TCS 山田 2013/10対応版　既存流用 START
                Dim dt As FunctionSettingDataSet.FUNCTIONSETTINGDataTable = query.GetData()

                ' ======================== ログ出力 開始 ========================
                Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                          " {0}_End, Return:[{1}]",
                                          MethodBase.GetCurrentMethod.Name, dt.Rows.Count))
                ' ======================== ログ出力 終了 ========================

                Return dt
                ' 2013/06/30 TCS 山田 2013/10対応版　既存流用 END

            End Using

        End Function
#End Region

    End Class


End Namespace

