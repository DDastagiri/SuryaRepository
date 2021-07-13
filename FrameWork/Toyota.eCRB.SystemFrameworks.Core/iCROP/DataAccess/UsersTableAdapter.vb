Imports Oracle.DataAccess.Client
Imports System.Text
Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Globalization
Imports System.Reflection

Namespace Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess

    ''' <summary>
    ''' TBL_USERSのデータを取得する共通クラスです。
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class UsersTableAdapter

        Private Sub New()

        End Sub

        ''' <summary>
        ''' TBL_USERSから一覧を取得します。
        ''' </summary>
        ''' <param name="dlrCd">販売店コード</param>
        ''' <param name="strCd">店舗コード</param>
        ''' <param name="operationCdList">オペレーションコード</param>
        ''' <param name="delFlg">削除フラグ</param>
        ''' <returns>USERSDataTable</returns>
        ''' <remarks>
        ''' TBL_USERSから一覧を取得します。
        ''' </remarks>
        Public Shared Function GetUsersDataTable(ByVal dlrCD As String,
                                          ByVal strCD As String,
                                          ByVal operationCDList As List(Of Decimal),
                                          ByVal delFlg As String) As UsersDataSet.USERSDataTable

            ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
            ' ======================== ログ出力 開始 ========================
            Dim ope As String
            If operationCDList IsNot Nothing Then
                ope = String.Join(", ", operationCDList.ToArray())
            Else
                ope = ""
            End If
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      " {0}_Start, dlrCD:[{1}], dlrCD:[{2}], operationCDList:[{3}], delFlg:[{4}]",
                                      MethodBase.GetCurrentMethod.Name,
                                      dlrCD,
                                      strCD,
                                      ope,
                                      delFlg))
            ' ======================== ログ出力 終了 ========================
            ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END

            Using query As New DBSelectQuery(Of UsersDataSet.USERSDataTable)("USERS_001")

                Dim sql As New StringBuilder

                With sql
                    ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
                    .Append(" SELECT /* USERS_001 */ ")
                    .Append("        ACCOUNT ")
                    .Append("      , PASSWORD ")
                    .Append("      , DLRCD ")
                    .Append("      , STRCD ")
                    .Append("      , PERMISSION ")
                    .Append("      , OPERATIONCODE ")
                    .Append("      , USERNAME ")
                    .Append("      , USERKANA ")
                    .Append("      , USERCOMMENT ")
                    .Append("      , ORG_IMGFILE ")
                    .Append("      , SYS_IMGFILE ")
                    .Append("      , DELFLG ")
                    .Append("      , CREATEDATE ")
                    .Append("      , UPDATEDATE ")
                    .Append("      , UPDATEACCOUNT ")
                    .Append("      , INBOUNDFLG ")
                    .Append("      , ORG_SIGNFILE ")
                    .Append("      , SYS_SIGNFILE ")
                    .Append("      , TEAMCD ")
                    .Append("      , LEADERFLG ")
                    .Append("      , CLMCHRGTYP ")
                    .Append("      , PRESENCECATEGORY ")
                    .Append("      , PRESENCEDETAIL ")
                    .Append("      , PRESENCEUPDATEDATE ")
                    .Append("      , PRESENCECATEGORYDATE ")
                    .Append(" FROM ")
                    .Append("  TBL_USERS T1 ")
                    .Append(" WHERE ")
                    .Append("     T1.DLRCD = :DLRCD ")
                    query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrCD)
                    If Not String.IsNullOrEmpty(strCD) Then
                        .Append(" AND T1.STRCD = :STRCD ")
                        query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strCD)
                    End If
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
                    If Not String.IsNullOrEmpty(delFlg) Then
                        .Append(" AND T1.DELFLG = :DELFLG ")
                        query.AddParameterWithTypeValue("DELFLG", OracleDbType.Char, delFlg)
                    End If
                    ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END
                End With

                query.CommandText = sql.ToString()

                ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
                Dim dt As UsersDataSet.USERSDataTable = query.GetData()

                ' ======================== ログ出力 開始 ========================
                Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                          " {0}_End, Return:[{1}]",
                                          MethodBase.GetCurrentMethod.Name, dt.Rows.Count))
                ' ======================== ログ出力 終了 ========================

                Return dt
                ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END

            End Using

        End Function

        ''' <summary>
        ''' TBL_USERSから指定ユーザーを取得します。
        ''' </summary>
        ''' <param name="account">アカウント</param>
        ''' <param name="delFlg">削除フラグ</param>
        ''' <returns>USERSDataTable</returns>
        ''' <remarks>
        ''' TBL_USERSから指定ユーザーを取得します。
        ''' </remarks>
        Public Shared Function GetUsersDataTable(ByVal account As String _
                               , ByVal delFlg As String) As UsersDataSet.USERSDataTable

            ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      " {0}_Start, account:[{1}], delFlg:[{2}]",
                                      MethodBase.GetCurrentMethod.Name,
                                      account,
                                      delFlg))
            ' ======================== ログ出力 終了 ========================
            ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END

            Using query As New DBSelectQuery(Of UsersDataSet.USERSDataTable)("USERS_002")

                Dim sql As New StringBuilder

                With sql
                    ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
                    .Append(" SELECT /* USERS_002 */ ")
                    .Append("        ACCOUNT ")
                    .Append("      , PASSWORD ")
                    .Append("      , DLRCD ")
                    .Append("      , STRCD ")
                    .Append("      , PERMISSION ")
                    .Append("      , OPERATIONCODE ")
                    .Append("      , USERNAME ")
                    .Append("      , USERKANA ")
                    .Append("      , USERCOMMENT ")
                    .Append("      , ORG_IMGFILE ")
                    .Append("      , SYS_IMGFILE ")
                    .Append("      , DELFLG ")
                    .Append("      , CREATEDATE ")
                    .Append("      , UPDATEDATE ")
                    .Append("      , UPDATEACCOUNT ")
                    .Append("      , INBOUNDFLG ")
                    .Append("      , ORG_SIGNFILE ")
                    .Append("      , SYS_SIGNFILE ")
                    .Append("      , TEAMCD ")
                    .Append("      , LEADERFLG ")
                    .Append("      , CLMCHRGTYP ")
                    .Append("      , PRESENCECATEGORY ")
                    .Append("      , PRESENCEDETAIL ")
                    .Append("      , PRESENCEUPDATEDATE ")
                    .Append("      , PRESENCECATEGORYDATE ")
                    .Append(" FROM ")
                    .Append("  TBL_USERS A ")
                    .Append(" WHERE ")
                    .Append("     A.ACCOUNT = :ACCOUNT ")
                    query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.Varchar2, account)
                    If Not String.IsNullOrEmpty(delFlg) Then
                        .Append(" AND A.DELFLG = :DELFLG ")
                        query.AddParameterWithTypeValue("DELFLG", OracleDbType.Char, delFlg)
                    End If
                    ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END
                End With

                query.CommandText = sql.ToString()

                ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
                Dim dt As UsersDataSet.USERSDataTable = query.GetData()

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

