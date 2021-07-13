Imports Oracle.DataAccess.Client
Imports System.Text
Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Globalization
Imports System.Reflection

Namespace Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess

    Public NotInheritable Class OperationTypeTableAdapter

        Private Sub New()

        End Sub

#Region "定数"
        '2013/06/30 TCS 坂井 2013/10対応版 既存流用 START
        ''' <summary>
        ''' 引数_削除フラグの判定用定数(値:0)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const flgZero As String = "0"
        ''' <summary>
        ''' 引数_削除フラグの判定用定数(値:1)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const flgOne As String = "1"
        ''' <summary>
        ''' 引数_削除フラグの代入用定数(値:ブランク)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const blank As String = " "
        '2013/06/30 TCS 坂井 2013/10対応版 既存流用 END
#End Region


#Region "GetOperationTypeDataTable"
        ''' <summary>
        ''' TBL_OPERATIONTYPEから権限一覧を取得します。
        ''' </summary>
        ''' <param name="dlrCd">販売店コード</param>
        ''' <param name="strCd">店舗コード</param>
        ''' <param name="operationCdList">オペレーションコード</param>
        ''' <param name="delFlg">削除フラグ</param>
        ''' <returns>OPERATIONTYPEDataTable</returns>
        ''' <remarks>
        ''' TBL_OPERATIONTYPEから権限一覧を取得します。
        ''' </remarks>
        Public Shared Function GetOperationTypeDataTable(ByVal dlrCD As String,
                                                  ByVal strCD As String,
                                                  ByVal operationCDList As List(Of Decimal),
                                                  ByVal delFlg As String) As OperationTypeDataSet.OPERATIONTYPEDataTable

            ' 2013/06/30 TCS 坂井 2013/10対応版　既存流用 START
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      " {0}_Start, dlrCD:[{1}], strCD:[{2}], operationCDList:[{3}], delFlg:[{4}]",
                                      MethodBase.GetCurrentMethod.Name,
                                      dlrCD,
                                      strCD,
                                      operationCDList,
                                      delFlg))
            ' ======================== ログ出力 終了 ========================
            ' 2013/06/30 TCS 坂井 2013/10対応版　既存流用 END

            Using query As New DBSelectQuery(Of OperationTypeDataSet.OPERATIONTYPEDataTable)("OPERATIONTYPE_001")

                Dim sql As New StringBuilder

                '2013/06/30 TCS 坂井 2013/10対応版 既存流用 START
                With sql
                    .Append(" SELECT /* OPERATIONTYPE_001 */ ")
                    .Append("     OPERATIONCODE ")
                    .Append("   , dlrCD ")
                    .Append("   , strCD ")
                    .Append("   , OPERATIONNAME ")
                    .Append("   , OPERATIONNAME_DFL ")
                    .Append("   , OPERATIONCOMMENT ")
                    .Append("   , RECUSEFLG ")
                    .Append("   , delFlg ")
                    .Append("   , CREATEDATE ")
                    .Append("   , UPDATEDATE ")
                    .Append("   , UPDATEACCOUNT ")
                    .Append("   , LOGIN_STARTTIME ")
                    .Append("   , LOGIN_ENDTIME ")
                    .Append("   , ICON_IMGFILE ")
                    .Append(" FROM ")
                    .Append("     TBL_OPERATIONTYPE T1 ")
                    .Append(" WHERE ")
                    .Append("     T1.DLRCD = :DLRCD ")
                    query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrCD)
                    If operationCDList IsNot Nothing Then
                        .Append(" AND T1.OPERATIONCODE IN (")
                        Dim i As Integer = 1
                        For Each operationCd As Decimal In operationCDList
                            .Append(" :OPERATIONCODE" & i)
                            query.AddParameterWithTypeValue("OPERATIONCODE" & i, OracleDbType.Decimal, operationCd)
                            If Not operationCDList.Count() = i Then
                                .Append(",")
                            End If
                            i = i + 1
                        Next
                        .Append(" ) ")
                    End If
                    .Append(" AND T1.STRCD = :STRCD ")
                    query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strCD)
                    If Not String.IsNullOrEmpty(delFlg) Then
                        .Append("AND T1.DELFLG = :DELFLG ")
                        query.AddParameterWithTypeValue("DELFLG", OracleDbType.Char, delFlg)
                    End If
                End With
                '2013/06/30 TCS 坂井 2013/10対応版 既存流用 END
                query.CommandText = sql.ToString()

                ' 2013/06/30 TCS 坂井 2013/10対応版　既存流用 START
                Dim dt As OperationTypeDataSet.OPERATIONTYPEDataTable = query.GetData()

                ' ======================== ログ出力 開始 ========================
                Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                          " {0}_End, Return:[{1}]",
                                          MethodBase.GetCurrentMethod.Name, dt.Rows.Count))
                ' ======================== ログ出力 終了 ========================

                Return dt
                ' 2013/06/30 TCS 坂井 2013/10対応版　既存流用 END

            End Using

        End Function
#End Region

    End Class

End Namespace

