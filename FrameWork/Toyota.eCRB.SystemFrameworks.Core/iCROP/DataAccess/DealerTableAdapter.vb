Imports Oracle.DataAccess.Client
Imports System.Text
Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Globalization
Imports System.Reflection

Namespace Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess

    ''' <summary>
    ''' TB_M_DEALERからデータを取得する共通クラスです。
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class DealerTableAdapter

        Private Sub New()

        End Sub

#Region "定数"
        ''2013/06/30 TCS 坂井 2013/10対応版 既存流用 START ADD
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

#Region "GetDealerDataTable"
        ''' <summary>
        ''' TB_M_DEALERから販売店リストを取得します。
        ''' </summary>
        ''' <param name="dlrCd">販売店コード</param>
        ''' <param name="delFlg">削除フラグ</param>
        ''' <returns>DEALERDataTable</returns>
        ''' <remarks>
        ''' TB_M_DEALERから販売店リストを取得します。
        ''' </remarks>
        Public Shared Function GetDealerDataTable(ByVal dlrCD As String, ByVal delFlg As String) As DealerDataSet.DEALERDataTable

            ' 2013/06/30 TCS 坂井 2013/10対応版　既存流用 START
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      " {0}_Start, dlrCD:[{1}], delFlg:[{2}]",
                                      MethodBase.GetCurrentMethod.Name,
                                      dlrCD,
                                      delFlg))
            ' ======================== ログ出力 終了 ========================
            ' 2013/06/30 TCS 坂井 2013/10対応版　既存流用 END

            Using query As New DBSelectQuery(Of DealerDataSet.DEALERDataTable)("DEALER_001")

                Dim sql As New StringBuilder

                '2013/06/30 TCS 坂井 2013/10対応版 既存流用 START
                Dim inUseFlg As String

                With sql
                    .Append(" SELECT /* DEALER_001 */ ")
                    .Append("     DLR_CD AS DLRCD ")
                    .Append("   , DLR_NAME AS DLRNM_LOCAL ")
                    .Append("   , DLR_NAME_ENG AS DLRNM_ENG ")
                    .Append("   , DLR_NAME AS DLRNICNM_LOCAL ")
                    .Append("   , DLR_NAME_ENG AS DLRNICNM_EN ")
                    .Append("   , DLR_ADDRESS_1 AS ADDR1_LOCAL ")
                    .Append("   , DLR_ADDRESS_1_ENG AS ADDR1_ENG ")
                    .Append("   , DLR_ADDRESS_2 AS ADDR2_LOCAL ")
                    .Append("   , DLR_ADDRESS_2_ENG AS ADDR2_ENG ")
                    .Append("   , DLR_ZIPCD AS POSTNO ")
                    .Append("   , DLR_PHONE AS TEL ")
                    .Append("   , DLR_FAX AS FAXNO ")
                    .Append("   , DLR_URL AS DLRURL ")
                    .Append("   , DLR_NAME_PIC_PATH AS DLRNMPICT ")
                    .Append("   , DLR_LOGO_PIC_PATH AS DLRLGPICT ")
                    .Append("   , DUMMY_DLR_FLG AS DUMMYDLRFLG ")
                    .Append("   , INUSE_FLG AS DELFLG ")
                    .Append(" FROM ")
                    .Append("     TB_M_DEALER T1 ")

                    If Not String.IsNullOrEmpty(dlrCD) OrElse Not String.IsNullOrEmpty(delFlg) Then
                        .Append(" WHERE ")
                    End If
                    If Not String.IsNullOrEmpty(dlrCD) Then
                        .Append(" T1.DLR_CD = :DLRCD ")
                        query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrCD)
                    End If
                    If Not String.IsNullOrEmpty(delFlg) Then
                        If Not String.IsNullOrEmpty(dlrCD) Then
                            .Append(" AND ")
                        End If
                        .Append(" T1.INUSE_FLG = :DELFLG ")

                        If delFlg.Equals(flgOne) Then
                            inUseFlg = flgZero
                        ElseIf delFlg.Equals(flgZero) Then
                            inUseFlg = flgOne
                        Else
                            inUseFlg = blank
                        End If
                        query.AddParameterWithTypeValue("DELFLG", OracleDbType.Char, inUseFlg)
                    End If
                End With
                '2013/06/30 TCS 坂井 2013/10対応版 既存流用 END

                query.CommandText = sql.ToString()

                ' 2013/06/30 TCS 坂井 2013/10対応版　既存流用 START
                Dim dt As DealerDataSet.DEALERDataTable = query.GetData()

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

