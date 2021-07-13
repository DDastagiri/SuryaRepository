'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'IC3080204DataSet.vb
'─────────────────────────────────────
'機能： 自社客個情報を取得する
'補足： 基幹顧客ID(DMSID)から自社客連番(ORIGINALID)を取得する
'作成： 2012/02/15 KN 佐藤（真）
'更新： 
'─────────────────────────────────────

Imports System.Text
Imports System.Reflection
Imports System.Globalization
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core

Namespace IC3080204DataSetTableAdapters

    Public Class IC3080204DataTableTableAdapter
        Inherits Global.System.ComponentModel.Component

        ''' <summary>
        ''' 自社客個人情報取得
        ''' </summary>
        ''' <param name="dealerCD">販売店コード</param>
        ''' <param name="storeCD">店舗コード</param>
        ''' <param name="basicCustomerId">基幹顧客ID(DMSID)</param>
        ''' <returns>自社客個人情報データテーブル</returns>
        ''' <remarks>基幹顧客ID(DMSID)から自社客連番(ORIGINALID)を取得</remarks>
        ''' <History>
        ''' </History>
        Public Function GetMyCustomer(ByVal dealerCD As String _
                                      , ByVal storeCD As String _
                                      , ByVal basicCustomerId As String) As IC3080204DataSet.IC3080204CustomerDataTable

            '引数を編集
            Dim args As New List(Of String)
            args.Add(String.Format(CultureInfo.InvariantCulture, "{0} = {1}", MethodBase.GetCurrentMethod.GetParameters(0).Name, dealerCD))
            args.Add(String.Format(CultureInfo.InvariantCulture, "{0} = {1}", MethodBase.GetCurrentMethod.GetParameters(1).Name, storeCD))
            args.Add(String.Format(CultureInfo.InvariantCulture, "{0} = {1}", MethodBase.GetCurrentMethod.GetParameters(2).Name, basicCustomerId))
            '開始ログを出力
            OutPutStartLog(MethodBase.GetCurrentMethod.Name, args)

            Dim sql As New StringBuilder
            With sql
                .Append(" SELECT /* IC3080204_001 */ ")
                .Append("        ORIGINALID ")
                .Append("   FROM TBLORG_CUSTOMER ")
                .Append("  WHERE DLRCD = :DLRCD ")
                .Append("    AND STRCD = :STRCD ")
                .Append("    AND CUSTCD = :DMSID ")
            End With

            Using query As New DBSelectQuery(Of IC3080204DataSet.IC3080204CustomerDataTable)("IC3080204_001")
                'パラメータ設定
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCD)               '販売店コード
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, storeCD)                '店舗コード
                query.AddParameterWithTypeValue("DMSID", OracleDbType.NVarchar2, basicCustomerId)   'DMSID
                'SQL実行
                Using dt As IC3080204DataSet.IC3080204CustomerDataTable = query.GetData()
                    '終了ログを出力
                    OutPutEndLog(MethodBase.GetCurrentMethod.Name, dt)
                    Return dt
                End Using
            End Using

        End Function

        ''' <summary>
        ''' 開始ログ出力
        ''' </summary>
        ''' <param name="methodName">メソッド名</param>
        ''' <param name="args">引数</param>
        ''' <remarks></remarks>
        ''' 
        ''' <History>
        ''' </History>
        Private Sub OutPutStartLog(ByVal methodName As String, ByVal args As List(Of String))

            '引数をログに出力
            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                "{0}.{1} IN:{2}" _
                , Me.GetType.ToString _
                , methodName _
                , String.Join(", ", args.ToArray())))

        End Sub

        ''' <summary>
        ''' 終了ログ出力
        ''' </summary>
        ''' <param name="methodName">メソッド名</param>
        ''' <param name="dt">取得データ</param>
        ''' <remarks></remarks>
        ''' 
        ''' <History>
        ''' </History>
        Private Sub OutPutEndLog(ByVal methodName As String, ByVal dt As DataTable)

            '取得件数をログに出力
            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                "{0}.{1} OUT:ROWSCOUNT = {2}" _
                , Me.GetType.ToString _
                , methodName _
                , dt.Rows.Count))

        End Sub

    End Class

End Namespace

Partial Class IC3080204DataSet

End Class
