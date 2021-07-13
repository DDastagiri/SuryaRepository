'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'IC3810301DataSet.vb
'─────────────────────────────────────
'機能： R/O連携データアクセス
'補足： 
'作成： 2012/01/26 KN 瀧
'更新： 
'─────────────────────────────────────

Imports System.Text
Imports System.Reflection
Imports System.Globalization
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic

Partial Class IC3810301DataSet
End Class

Namespace IC3810301DataSetTableAdapters
    Public Class IC3810301DataTableAdapter
        Inherits Global.System.ComponentModel.Component

        ''' <summary>
        ''' サービス来店者管理テーブルの存在チェック
        ''' </summary>
        ''' <param name="rowIN">R/O画面仕掛中反映/R/Oキャンセル引数</param>
        ''' <returns>サービス来店者管理キー情報</returns>
        ''' <remarks></remarks>
        ''' 
        ''' <history>
        ''' </history>
        Public Function GetVisitKey(ByVal rowIN As IC3810301DataSet.IC3810301inOrderSaveRow) As IC3810301DataSet.IC3810301VisitKeyDataTable
            Try
                ''引数をログに出力
                Dim args As New List(Of String)
                For Each column As DataColumn In rowIN.Table.Columns
                    If rowIN.IsNull(column.ColumnName) = True Then
                        args.Add(String.Format(CultureInfo.CurrentCulture, "{0} = NULL", column.ColumnName))
                    Else
                        args.Add(String.Format(CultureInfo.CurrentCulture, "{0} = {1}", column.ColumnName, rowIN(column.ColumnName)))
                    End If
                Next
                ''開始ログの出力
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} IN:{2}" _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name _
                    , String.Join(", ", args.ToArray())))

                Using query As New DBSelectQuery(Of IC3810301DataSet.IC3810301VisitKeyDataTable)("IC3810301_001")
                    ''SQLの設定
                    Dim sql As New StringBuilder
                    sql.AppendLine("SELECT /* IC3810301_001 */")
                    sql.AppendLine("       SACODE")
                    sql.AppendLine("  FROM TBL_SERVICE_VISIT_MANAGEMENT")
                    sql.AppendLine(" WHERE VISITSEQ = :VISITSEQ")
                    sql.AppendLine("   AND DLRCD = :DLRCD")
                    sql.AppendLine("   AND STRCD = :STRCD")
                    query.CommandText = sql.ToString()
                    ''パラメータの設定
                    query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Decimal, rowIN.VISITSEQ)
                    query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, rowIN.DLRCD)
                    query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, rowIN.STRCD)
                    ''SQLの実行
                    Using dt As IC3810301DataSet.IC3810301VisitKeyDataTable = query.GetData()
                        ''終了ログの出力
                        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} OUT:ROWSCOUNT = {2}" _
                            , Me.GetType.ToString _
                            , MethodBase.GetCurrentMethod.Name _
                            , dt.Rows.Count))
                        Return dt
                    End Using
                End Using
            Finally

            End Try
        End Function

        ''' <summary>
        ''' R/O画面仕掛中反映(新規追加)
        ''' </summary>
        ''' <param name="rowIN">R/O画面仕掛中反映</param>
        ''' <returns>来店実績連番</returns>
        ''' <remarks></remarks>
        ''' 
        ''' <history>
        ''' </history>
        Public Function InsertVisitOrder(ByVal rowIN As IC3810301DataSet.IC3810301inOrderSaveRow) As Long
            Try
                ''引数をログに出力
                Dim args As New List(Of String)
                For Each column As DataColumn In rowIN.Table.Columns
                    If rowIN.IsNull(column.ColumnName) = True Then
                        args.Add(String.Format(CultureInfo.CurrentCulture, "{0} = NULL", column.ColumnName))
                    Else
                        args.Add(String.Format(CultureInfo.CurrentCulture, "{0} = {1}", column.ColumnName, rowIN(column.ColumnName)))
                    End If
                Next
                ''開始ログの出力
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} IN:{2}" _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name _
                    , String.Join(", ", args.ToArray())))

                Dim visitseq As Long = 0
                Using query As New DBSelectQuery(Of DataTable)("IC3810301_101")
                    ''SQLの設定
                    Dim sqlNextVal As New StringBuilder
                    sqlNextVal.AppendLine("SELECT /* IC3810301_101 */")
                    sqlNextVal.AppendLine("       SEQ_SERVICE_VISIT_MANAGEMENT.NEXTVAL AS VISITSEQ")
                    sqlNextVal.AppendLine("  FROM DUAL")
                    query.CommandText = sqlNextVal.ToString()
                    Using dt As DataTable = query.GetData()
                        visitseq = CType(dt.Rows(0)("VISITSEQ"), Long)
                    End Using
                End Using
                Using query As New DBUpdateQuery("IC3810301_102")
                    ''SQLの設定
                    Dim sqlInsert As New StringBuilder
                    sqlInsert.AppendLine("INSERT /* IC3810301_102 */")
                    sqlInsert.AppendLine("       INTO TBL_SERVICE_VISIT_MANAGEMENT (")
                    sqlInsert.AppendLine("       VISITSEQ")
                    sqlInsert.AppendLine("     , DLRCD")
                    sqlInsert.AppendLine("     , STRCD")
                    sqlInsert.AppendLine("     , SACODE")
                    sqlInsert.AppendLine("     , ORDERNO")
                    sqlInsert.AppendLine("     , REGISTKIND")
                    sqlInsert.AppendLine("     , CREATEDATE")
                    sqlInsert.AppendLine("     , UPDATEDATE")
                    sqlInsert.AppendLine("     , CREATEACCOUNT")
                    sqlInsert.AppendLine("     , UPDATEACCOUNT")
                    sqlInsert.AppendLine("     , CREATEID")
                    sqlInsert.AppendLine("     , UPDATEID")
                    sqlInsert.AppendLine(")")
                    sqlInsert.AppendLine("VALUES (")
                    sqlInsert.AppendLine("       :VISITSEQ")
                    sqlInsert.AppendLine("     , :DLRCD")
                    sqlInsert.AppendLine("     , :STRCD")
                    sqlInsert.AppendLine("     , :SACODE")
                    sqlInsert.AppendLine("     , :ORDERNO")
                    sqlInsert.AppendLine("     , :REGISTKIND")
                    sqlInsert.AppendLine("     , SYSDATE")
                    sqlInsert.AppendLine("     , SYSDATE")
                    sqlInsert.AppendLine("     , :CREATEACCOUNT")
                    sqlInsert.AppendLine("     , :UPDATEACCOUNT")
                    sqlInsert.AppendLine("     , :CREATEID")
                    sqlInsert.AppendLine("     , :UPDATEID")
                    sqlInsert.AppendLine(")")
                    query.CommandText = sqlInsert.ToString()
                    ''パラメータの設定
                    query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Decimal, visitseq)
                    query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, rowIN.DLRCD)
                    query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, rowIN.STRCD)
                    query.AddParameterWithTypeValue("SACODE", OracleDbType.Varchar2, rowIN.SACODE)
                    If (rowIN.IsORDERNONull = True) Then
                        query.AddParameterWithTypeValue("ORDERNO", OracleDbType.Char, DBNull.Value)
                    Else
                        query.AddParameterWithTypeValue("ORDERNO", OracleDbType.Char, rowIN.ORDERNO)
                    End If
                    query.AddParameterWithTypeValue("REGISTKIND", OracleDbType.Char, "1")
                    query.AddParameterWithTypeValue("CREATEACCOUNT", OracleDbType.Varchar2, rowIN.ACCOUNT)
                    query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, rowIN.ACCOUNT)
                    query.AddParameterWithTypeValue("CREATEID", OracleDbType.Varchar2, rowIN.SYSTEM)
                    query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, rowIN.SYSTEM)
                    ''SQLの実行
                    query.Execute()
                End Using
                ''終了ログの出力
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} OUT:VISITSEQ = {2}" _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name _
                    , visitseq))
                Return visitseq
            Finally

            End Try
        End Function

        ''' <summary>
        ''' R/O画面仕掛中反映(修正更新)
        ''' </summary>
        ''' <param name="rowIN">R/O画面仕掛中反映</param>
        ''' <returns>更新件数</returns>
        ''' <remarks></remarks>
        ''' 
        ''' <history>
        ''' </history>
        Public Overloads Function UpdateVisitOrder(ByVal rowIN As IC3810301DataSet.IC3810301inOrderSaveRow) As Long
            Try
                ''引数をログに出力
                Dim args As New List(Of String)
                For Each column As DataColumn In rowIN.Table.Columns
                    If rowIN.IsNull(column.ColumnName) = True Then
                        args.Add(String.Format(CultureInfo.CurrentCulture, "{0} = NULL", column.ColumnName))
                    Else
                        args.Add(String.Format(CultureInfo.CurrentCulture, "{0} = {1}", column.ColumnName, rowIN(column.ColumnName)))
                    End If
                Next
                ''開始ログの出力
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} IN:{2}" _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name _
                    , String.Join(", ", args.ToArray())))

                Using query As New DBUpdateQuery("IC3810301_103")
                    ''SQLの設定
                    Dim sql As New StringBuilder
                    sql.AppendLine("UPDATE /* IC3810301_103 */")
                    sql.AppendLine("       TBL_SERVICE_VISIT_MANAGEMENT")
                    sql.AppendLine("   SET ORDERNO = :ORDERNO")
                    sql.AppendLine("     , UPDATEDATE = SYSDATE")
                    sql.AppendLine("     , UPDATEACCOUNT = :UPDATEACCOUNT")
                    sql.AppendLine("     , UPDATEID = :UPDATEID")
                    sql.AppendLine(" WHERE VISITSEQ = :VISITSEQ")
                    sql.AppendLine("   AND DLRCD = :DLRCD")
                    sql.AppendLine("   AND STRCD = :STRCD")
                    sql.AppendLine("   AND SACODE = :SACODE")
                    query.CommandText = sql.ToString()
                    ''パラメータの設定
                    If (rowIN.IsORDERNONull = True) Then
                        query.AddParameterWithTypeValue("ORDERNO", OracleDbType.Char, DBNull.Value)
                    Else
                        query.AddParameterWithTypeValue("ORDERNO", OracleDbType.Char, rowIN.ORDERNO)
                    End If
                    query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, rowIN.ACCOUNT)
                    query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, rowIN.SYSTEM)

                    query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Decimal, rowIN.VISITSEQ)
                    query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, rowIN.DLRCD)
                    query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, rowIN.STRCD)
                    query.AddParameterWithTypeValue("SACODE", OracleDbType.Varchar2, rowIN.SACODE)
                    ''SQLの実行
                    Dim ret As Integer = query.Execute()
                    ''終了ログの出力
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:ROWSCOUNT = {2}" _
                        , Me.GetType.ToString _
                        , MethodBase.GetCurrentMethod.Name _
                        , ret))
                    Return ret
                End Using
            Finally

            End Try
        End Function

        ''' <summary>
        ''' R/Oキャンセル
        ''' </summary>
        ''' <param name="rowIN">R/Oキャンセル引数</param>
        ''' <returns>削除件数</returns>
        ''' <remarks></remarks>
        ''' 
        ''' <history>
        ''' </history>
        Public Function DeleteVisitOrder(ByVal rowIN As IC3810301DataSet.IC3810301inOrderSaveRow) As Long
            Try
                ''引数をログに出力
                Dim args As New List(Of String)
                For Each column As DataColumn In rowIN.Table.Columns
                    If rowIN.IsNull(column.ColumnName) = True Then
                        args.Add(String.Format(CultureInfo.CurrentCulture, "{0} = NULL", column.ColumnName))
                    Else
                        args.Add(String.Format(CultureInfo.CurrentCulture, "{0} = {1}", column.ColumnName, rowIN(column.ColumnName)))
                    End If
                Next
                ''開始ログの出力
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} IN:{2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , String.Join(", ", args.ToArray())))

                ''サービス来店管理の修正更新
                ''整理受注NoのNULLクリア
                Using query As New DBUpdateQuery("IC3810301_201")
                    ''SQLの設定
                    Dim sql As New StringBuilder
                    sql.AppendLine("UPDATE /* IC3810301_201 */")
                    sql.AppendLine("       TBL_SERVICE_VISIT_MANAGEMENT")
                    sql.AppendLine("   SET ORDERNO = NULL")
                    sql.AppendLine("     , SACODE = DECODE(REGISTKIND, '1', NULL, SACODE)")
                    sql.AppendLine("     , ASSIGNSTATUS = DECODE(REGISTKIND, '1', '4', ASSIGNSTATUS)")
                    sql.AppendLine("     , UPDATEDATE = SYSDATE")
                    sql.AppendLine("     , UPDATEACCOUNT = :UPDATEACCOUNT")
                    sql.AppendLine("     , UPDATEID = :UPDATEID")
                    sql.AppendLine(" WHERE VISITSEQ = :VISITSEQ")
                    sql.AppendLine("   AND DLRCD = :DLRCD")
                    sql.AppendLine("   AND STRCD = :STRCD")
                    sql.AppendLine("   AND SACODE = :SACODE")
                    query.CommandText = sql.ToString()
                    ''パラメータの設定
                    query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, rowIN.ACCOUNT)
                    query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, rowIN.SYSTEM)

                    query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Decimal, rowIN.VISITSEQ)
                    query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, rowIN.DLRCD)
                    query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, rowIN.STRCD)
                    query.AddParameterWithTypeValue("SACODE", OracleDbType.Varchar2, rowIN.SACODE)
                    ''SQLの実行
                    Dim ret As Integer = query.Execute()
                    ''終了ログの出力
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:ROWSCOUNT = {2}" _
                        , Me.GetType.ToString _
                        , MethodBase.GetCurrentMethod.Name _
                        , ret))
                    Return ret
                End Using
            Finally

            End Try
        End Function

    End Class

End Namespace
