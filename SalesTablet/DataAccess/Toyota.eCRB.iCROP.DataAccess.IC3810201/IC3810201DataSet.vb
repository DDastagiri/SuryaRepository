'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'IC3810201DataSet.vb
'─────────────────────────────────────
'機能： 顧客連携データアクセス
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

Partial Class IC3810201DataSet
End Class

Namespace IC3810201DataSetTableAdapters
    Public Class IC3810201DataTableAdapter
        Inherits Global.System.ComponentModel.Component

        ''' <summary>
        ''' サービス来店者管理テーブルの存在チェック
        ''' </summary>
        ''' <param name="rowIN">顧客登録結果反引数</param>
        ''' <returns>サービス来店者キー情報</returns>
        ''' <remarks></remarks>
        ''' 
        ''' <history>
        ''' </history>
        Public Function GetVisitKey(ByVal rowIN As IC3810201DataSet.IC3810201inCustomerSaveRow) As IC3810201DataSet.IC3810201VisitKeyDataTable
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

                Using query As New DBSelectQuery(Of IC3810201DataSet.IC3810201VisitKeyDataTable)("IC3810201_001")
                    ''SQLの設定
                    Dim sql As New StringBuilder
                    sql.AppendLine("SELECT /* IC3810201_001 */")
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
                    Using dt As IC3810201DataSet.IC3810201VisitKeyDataTable = query.GetData()
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
        ''' 顧客登録結果反映(新規追加)
        ''' </summary>
        ''' <param name="rowIN">顧客登録結果反引数</param>
        ''' <returns>来店実績連番</returns>
        ''' <remarks></remarks>
        ''' 
        ''' <history>
        ''' </history>
        Public Function InsertVisitCustomer(ByVal rowIN As IC3810201DataSet.IC3810201inCustomerSaveRow) As Long
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
                Using query As New DBSelectQuery(Of DataTable)("IC3810201_101")
                    ''SQLの設定
                    Dim sqlNextVal As New StringBuilder
                    sqlNextVal.AppendLine("SELECT /* IC3810201_101 */")
                    sqlNextVal.AppendLine("       SEQ_SERVICE_VISIT_MANAGEMENT.NEXTVAL AS VISITSEQ")
                    sqlNextVal.AppendLine("  FROM DUAL")
                    query.CommandText = sqlNextVal.ToString()
                    Using dt As DataTable = query.GetData()
                        visitseq = CType(dt.Rows(0)("VISITSEQ"), Long)
                    End Using
                End Using
                Try
                    ''SQLの設定
                    Dim sqlInsert As New StringBuilder
                    sqlInsert.AppendLine("INSERT /* IC3810201_102 */")
                    sqlInsert.AppendLine("  INTO TBL_SERVICE_VISIT_MANAGEMENT (")
                    sqlInsert.AppendLine("       VISITSEQ")
                    sqlInsert.AppendLine("     , DLRCD")
                    sqlInsert.AppendLine("     , STRCD")
                    sqlInsert.AppendLine("     , VCLREGNO")
                    sqlInsert.AppendLine("     , CUSTSEGMENT")
                    sqlInsert.AppendLine("     , CUSTID")
                    sqlInsert.AppendLine("     , VIN")

                    ''2012/01/06 追加 サービス来店者情報に基幹顧客ID,モデルコード,電話番号,携帯番号項目の追加
                    sqlInsert.AppendLine("     , DMSID")
                    sqlInsert.AppendLine("     , MODELCODE")
                    sqlInsert.AppendLine("     , TELNO")
                    sqlInsert.AppendLine("     , MOBILE")

                    sqlInsert.AppendLine("     , NAME")
                    sqlInsert.AppendLine("     , SACODE")
                    sqlInsert.AppendLine("     , REGISTKIND")
                    sqlInsert.AppendLine("     , CREATEDATE")
                    sqlInsert.AppendLine("     , UPDATEDATE")
                    sqlInsert.AppendLine("     , CREATEACCOUNT")
                    sqlInsert.AppendLine("     , UPDATEACCOUNT")
                    sqlInsert.AppendLine("     , CREATEID")
                    sqlInsert.AppendLine("     , UPDATEID")
                    sqlInsert.AppendLine(") ")
                    sqlInsert.AppendLine("VALUES (")
                    sqlInsert.AppendLine("       :VISITSEQ")
                    sqlInsert.AppendLine("     , :DLRCD")
                    sqlInsert.AppendLine("     , :STRCD")
                    sqlInsert.AppendLine("     , :VCLREGNO")
                    sqlInsert.AppendLine("     , :CUSTSEGMENT")
                    sqlInsert.AppendLine("     , :CUSTID")
                    sqlInsert.AppendLine("     , :VIN")

                    ''2012/01/06 追加 サービス来店者情報に基幹顧客ID,モデルコード,電話番号,携帯番号項目の追加
                    sqlInsert.AppendLine("     , :DMSID")
                    sqlInsert.AppendLine("     , :MODELCODE")
                    sqlInsert.AppendLine("     , :TELNO")
                    sqlInsert.AppendLine("     , :MOBILE")

                    sqlInsert.AppendLine("     , :NAME")
                    sqlInsert.AppendLine("     , :SACODE")
                    sqlInsert.AppendLine("     , :REGISTKIND")
                    sqlInsert.AppendLine("     , SYSDATE")
                    sqlInsert.AppendLine("     , SYSDATE")
                    sqlInsert.AppendLine("     , :CREATEACCOUNT")
                    sqlInsert.AppendLine("     , :UPDATEACCOUNT")
                    sqlInsert.AppendLine("     , :CREATEID")
                    sqlInsert.AppendLine("     , :UPDATEID")
                    sqlInsert.AppendLine(")")
                    Using query As New DBUpdateQuery("IC3810201_102")
                        query.CommandText = sqlInsert.ToString()
                        ''パラメータの設定
                        query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Decimal, visitseq)
                        query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, rowIN.DLRCD)
                        query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, rowIN.STRCD)
                        If (rowIN.IsVCLREGNONull = True) Then
                            query.AddParameterWithTypeValue("VCLREGNO", OracleDbType.NVarchar2, DBNull.Value)
                        Else
                            query.AddParameterWithTypeValue("VCLREGNO", OracleDbType.NVarchar2, rowIN.VCLREGNO)
                        End If
                        query.AddParameterWithTypeValue("CUSTSEGMENT", OracleDbType.Char, "1")
                        If (rowIN.IsCUSTOMERCODENull = True) Then
                            query.AddParameterWithTypeValue("CUSTID", OracleDbType.Char, DBNull.Value)
                        Else
                            query.AddParameterWithTypeValue("CUSTID", OracleDbType.Char, rowIN.CUSTOMERCODE)
                        End If
                        If (rowIN.IsVINNull = True) Then
                            query.AddParameterWithTypeValue("VIN", OracleDbType.Varchar2, DBNull.Value)
                        Else
                            query.AddParameterWithTypeValue("VIN", OracleDbType.Varchar2, rowIN.VIN)
                        End If

                        ''2012/01/06 追加 サービス来店者情報に基幹顧客ID,モデルコード,電話番号,携帯番号項目の追加
                        If (rowIN.IsDMSIDNull = True) Then
                            query.AddParameterWithTypeValue("DMSID", OracleDbType.NVarchar2, DBNull.Value)
                        Else
                            query.AddParameterWithTypeValue("DMSID", OracleDbType.NVarchar2, rowIN.DMSID)
                        End If
                        If (rowIN.IsMODELCODENull = True) Then
                            query.AddParameterWithTypeValue("MODELCODE", OracleDbType.NVarchar2, DBNull.Value)
                        Else
                            query.AddParameterWithTypeValue("MODELCODE", OracleDbType.NVarchar2, rowIN.MODELCODE)
                        End If
                        If (rowIN.IsTELNONull = True) Then
                            query.AddParameterWithTypeValue("TELNO", OracleDbType.NVarchar2, DBNull.Value)
                        Else
                            query.AddParameterWithTypeValue("TELNO", OracleDbType.NVarchar2, rowIN.TELNO)
                        End If
                        If (rowIN.IsMOBILENull = True) Then
                            query.AddParameterWithTypeValue("MOBILE", OracleDbType.NVarchar2, DBNull.Value)
                        Else
                            query.AddParameterWithTypeValue("MOBILE", OracleDbType.NVarchar2, rowIN.MOBILE)
                        End If

                        If (rowIN.IsCUSTOMERNAMENull = True) Then
                            query.AddParameterWithTypeValue("NAME", OracleDbType.NVarchar2, DBNull.Value)
                        Else
                            query.AddParameterWithTypeValue("NAME", OracleDbType.NVarchar2, rowIN.CUSTOMERNAME)
                        End If
                        query.AddParameterWithTypeValue("SACODE", OracleDbType.Varchar2, rowIN.SACODE)
                        query.AddParameterWithTypeValue("REGISTKIND", OracleDbType.Char, "1")
                        query.AddParameterWithTypeValue("CREATEACCOUNT", OracleDbType.Varchar2, rowIN.ACCOUNT)
                        query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, rowIN.ACCOUNT)
                        query.AddParameterWithTypeValue("CREATEID", OracleDbType.Varchar2, rowIN.SYSTEM)
                        query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, rowIN.SYSTEM)
                        ''SQLの実行
                        query.Execute()
                    End Using
                Finally
                End Try
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
        ''' 顧客登録結果反映(修正更新)
        ''' </summary>
        ''' <param name="rowIN">顧客登録結果反引数</param>
        ''' <returns>更新件数</returns>
        ''' <remarks></remarks>
        ''' 
        ''' <history>
        ''' </history>
        Public Function UpdateVisitCustomer(ByVal rowIN As IC3810201DataSet.IC3810201inCustomerSaveRow) As Long
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

                Using query As New DBUpdateQuery("IC3810201_103")
                    ''SQLの設定
                    Dim sql As New StringBuilder
                    sql.AppendLine("UPDATE /* IC3810201_103 */")
                    sql.AppendLine("       TBL_SERVICE_VISIT_MANAGEMENT")
                    sql.AppendLine("   SET VCLREGNO = :VCLREGNO")
                    sql.AppendLine("     , CUSTSEGMENT = :CUSTSEGMENT")
                    sql.AppendLine("     , CUSTID = :CUSTID")
                    sql.AppendLine("     , VIN = :VIN")

                    ''2012/01/06 追加 サービス来店者情報に基幹顧客ID,モデルコード,電話番号,携帯番号項目の追加
                    sql.AppendLine("     , DMSID = :DMSID")
                    sql.AppendLine("     , MODELCODE = :MODELCODE")
                    sql.AppendLine("     , TELNO = :TELNO")
                    sql.AppendLine("     , MOBILE = :MOBILE")

                    sql.AppendLine("     , NAME = :NAME")
                    sql.AppendLine("     , UPDATEDATE = SYSDATE")
                    sql.AppendLine("     , UPDATEACCOUNT = :UPDATEACCOUNT")
                    sql.AppendLine("     , UPDATEID = :UPDATEID")
                    sql.AppendLine(" WHERE VISITSEQ = :VISITSEQ")
                    sql.AppendLine("   AND DLRCD = :DLRCD")
                    sql.AppendLine("   AND STRCD = :STRCD")
                    sql.AppendLine("   AND SACODE = :SACODE")
                    query.CommandText = sql.ToString()
                    ''パラメータの設定
                    query.AddParameterWithTypeValue("VCLREGNO", OracleDbType.NVarchar2, rowIN.VCLREGNO)
                    query.AddParameterWithTypeValue("CUSTSEGMENT", OracleDbType.Char, "1")
                    If (rowIN.IsCUSTOMERCODENull = True) Then
                        query.AddParameterWithTypeValue("CUSTID", OracleDbType.Char, DBNull.Value)
                    Else
                        query.AddParameterWithTypeValue("CUSTID", OracleDbType.Char, rowIN.CUSTOMERCODE)
                    End If
                    If (rowIN.IsVINNull = True) Then
                        query.AddParameterWithTypeValue("VIN", OracleDbType.Varchar2, DBNull.Value)
                    Else
                        query.AddParameterWithTypeValue("VIN", OracleDbType.Varchar2, rowIN.VIN)
                    End If

                    ''2012/01/06 追加 サービス来店者情報に基幹顧客ID,モデルコード,電話番号,携帯番号項目の追加
                    If (rowIN.IsDMSIDNull = True) Then
                        query.AddParameterWithTypeValue("DMSID", OracleDbType.NVarchar2, DBNull.Value)
                    Else
                        query.AddParameterWithTypeValue("DMSID", OracleDbType.NVarchar2, rowIN.DMSID)
                    End If
                    If (rowIN.IsMODELCODENull = True) Then
                        query.AddParameterWithTypeValue("MODELCODE", OracleDbType.NVarchar2, DBNull.Value)
                    Else
                        query.AddParameterWithTypeValue("MODELCODE", OracleDbType.NVarchar2, rowIN.MODELCODE)
                    End If
                    If (rowIN.IsTELNONull = True) Then
                        query.AddParameterWithTypeValue("TELNO", OracleDbType.NVarchar2, DBNull.Value)
                    Else
                        query.AddParameterWithTypeValue("TELNO", OracleDbType.NVarchar2, rowIN.TELNO)
                    End If
                    If (rowIN.IsMOBILENull = True) Then
                        query.AddParameterWithTypeValue("MOBILE", OracleDbType.NVarchar2, DBNull.Value)
                    Else
                        query.AddParameterWithTypeValue("MOBILE", OracleDbType.NVarchar2, rowIN.MOBILE)
                    End If

                    If (rowIN.IsCUSTOMERNAMENull = True) Then
                        query.AddParameterWithTypeValue("NAME", OracleDbType.NVarchar2, DBNull.Value)
                    Else
                        query.AddParameterWithTypeValue("NAME", OracleDbType.NVarchar2, rowIN.CUSTOMERNAME)
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

    End Class

End Namespace
