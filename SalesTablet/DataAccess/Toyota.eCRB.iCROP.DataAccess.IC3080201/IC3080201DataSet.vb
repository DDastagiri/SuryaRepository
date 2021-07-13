'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'IC3080201DataSet.vb
'─────────────────────────────────────
'機能： 顧客詳細(写真)
'補足： 
'作成： 2012/01/27 KN 佐藤（真）
'更新： 
'─────────────────────────────────────

Imports System.Text
Imports System.Reflection
Imports System.Globalization
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core

Namespace IC3080201DataSetTableAdapters

    Public Class IC3080201DataTableTableAdapter
        Inherits Global.System.ComponentModel.Component

        ''' <summary>
        ''' 自社客連番取得
        ''' </summary>
        ''' <param name="basicCustomerId">基幹顧客ID(DMSID)</param>
        ''' <returns>自社客連番</returns>
        ''' <remarks>最新の自社客連番を取得</remarks>
        ''' 
        ''' <History>
        ''' </History>
        Public Function GetOriginalId(ByVal basicCustomerId As String) As String

            '引数を編集
            Dim args As New List(Of String)
            args.Add(String.Format(CultureInfo.InvariantCulture, "{0} = {1}", MethodBase.GetCurrentMethod.GetParameters(0).Name, basicCustomerId))
            '開始ログを出力
            OutPutStartLog(MethodBase.GetCurrentMethod.Name, args)

            Dim sql As New StringBuilder
            With sql
                .Append(" SELECT  /* IC3080201_001 */ ")
                .Append("         ORIGINALID ")
                .Append("   FROM  TBLORG_VCLINFO ")
                .Append("  WHERE  CUSTCD = :DMSID ")
                .Append("  ORDER  BY UPDATEDATE DESC ")
            End With

            Using query As New DBSelectQuery(Of IC3080201DataSet.IC3080201OrgPictureDataTable)("IC3080201_001")
                'パラメータ設定
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DMSID", OracleDbType.NVarchar2, basicCustomerId) 'DMSID

                'SQL実行
                Using dt As DataTable = query.GetData()
                    '終了ログを出力
                    OutPutEndLog(MethodBase.GetCurrentMethod.Name, dt)

                    If dt.Rows.Count = 0 Then
                        Return Nothing
                    Else
                        Return dt.Rows(0)("ORIGINALID").ToString
                    End If
                End Using
            End Using

        End Function

        ''' <summary>
        ''' 自社客取得（顔写真）
        ''' </summary>
        ''' <param name="originalId">自社客連番</param>
        ''' <returns>自社客情報</returns>
        ''' <remarks></remarks>
        ''' 
        ''' <History>
        ''' </History>
        Public Function GetPicture(ByVal originalId As String) As IC3080201DataSet.IC3080201OrgPictureDataTable

            '引数を編集
            Dim args As New List(Of String)
            args.Add(String.Format(CultureInfo.InvariantCulture, "{0} = {1}", MethodBase.GetCurrentMethod.GetParameters(0).Name, originalId))
            '開始ログを出力
            OutPutStartLog(MethodBase.GetCurrentMethod.Name, args)

            Dim sql As New StringBuilder
            With sql
                .Append(" SELECT  /* IC3080201_002 */ ")
                .Append("         IMAGEFILE_S ")
                .Append("   FROM  TBLORG_CUSTOMER_APPEND ")
                .Append("  WHERE  ORIGINALID = :ORIGINALID ")
            End With

            Using query As New DBSelectQuery(Of IC3080201DataSet.IC3080201OrgPictureDataTable)("IC3080201_002")
                'パラメータ設定
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("ORIGINALID", OracleDbType.Char, originalId) '自社客連番

                'SQL実行
                Using dt As IC3080201DataSet.IC3080201OrgPictureDataTable = query.GetData()
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

Partial Class IC3080201DataSet

End Class
