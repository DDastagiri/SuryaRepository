Imports Oracle.DataAccess.Client
Imports System.Text
Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Globalization
Imports System.Reflection

Namespace Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess

    ''' <summary>
    ''' TB_M_BRANCHからデータを取得する共通クラスです。
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class BranchTableAdapter

        Private Sub New()

        End Sub

        ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
#Region "定数"
        ''' <summary>
        ''' フラグの判定用定数(値:0)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const flgOff As String = "0"
        ''' <summary>
        ''' フラグの判定用定数(値:1)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const flgOn As String = "1"
        ''' <summary>
        ''' フラグの代入用定数(値:ブランク)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const flgBlank As String = " "
#End Region
        ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END

#Region "GetBranchDataTable"
        ''' <summary>
        ''' TB_M_BRANCHから店舗リストを取得します。
        ''' </summary>
        ''' <param name="dlrCd">販売店コード</param>
        ''' <param name="strCd">店舗コード</param>
        ''' <param name="delFlg">削除フラグ</param>
        ''' <returns>BRANCHDataTable</returns>
        ''' <remarks>
        ''' TB_M_BRANCHから店舗リストを取得します。
        ''' </remarks>
        Public Shared Function GetBranchDataTable(ByVal dlrCD As String, ByVal strCD As String, ByVal delFlg As String) As BranchDataSet.BRANCHDataTable

            ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      " {0}_Start, dlrCD:[{1}], strCD:[{2}], delFlg:[{3}]",
                                      MethodBase.GetCurrentMethod.Name,
                                      dlrCD,
                                      strCD,
                                      delFlg))
            ' ======================== ログ出力 終了 ========================
            ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END

            Using query As New DBSelectQuery(Of BranchDataSet.BRANCHDataTable)("BRANCH_001")

                Dim sql As New StringBuilder

                With sql
                    ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
                    .Append(" SELECT /* BRANCH_001 */ ")
                    .Append("        T1.DLR_CD AS DLRCD ")
                    .Append("      , T1.BRN_CD AS STRCD ")
                    .Append("      , T1.BRN_NAME AS STRNM_LOCAL ")
                    .Append("      , T1.BRN_NAME_ENG AS STRNM_ENG ")
                    .Append("      , T2.SALES_PHONE AS SALTEL ")
                    .Append("      , T2.SALES_FAX AS SALFAXNO ")
                    .Append("      , T2.SVC_PHONE AS SRVSTEL ")
                    .Append("      , CASE WHEN T1.INUSE_FLG = '0' THEN '1' ")
                    .Append("             WHEN T1.INUSE_FLG = '1' THEN '0' ")
                    .Append("        END AS DELFLG ")
                    .Append("      , T1.ROW_CREATE_DATETIME AS INSDATE ")
                    .Append("      , T1.ROW_UPDATE_DATETIME AS UPDTTM ")
                    .Append("      , T2.BRN_ADDRESS_1 AS ADDR1_LOCAL ")
                    .Append("   FROM TB_M_BRANCH T1 ")
                    .Append("      , TB_M_BRANCH_DETAIL T2 ")
                    .Append("  WHERE T1.DLR_CD = T2.DLR_CD ")
                    .Append("    AND T1.BRN_CD = T2.BRN_CD ")
                    .Append("    AND T1.DLR_CD = :DLRCD ")
                    query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrCD)
                    If Not String.IsNullOrEmpty(strCD) Then
                        .Append("    AND T1.BRN_CD = :STRCD ")
                        query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strCD)
                    End If
                    If Not String.IsNullOrEmpty(delFlg) Then
                        Dim inUseFlg As String
                        If delFlg.Equals(flgOn) Then
                            inUseFlg = flgOff
                        ElseIf delFlg.Equals(flgOff) Then
                            inUseFlg = flgOn
                        Else
                            inUseFlg = flgBlank
                        End If
                        .Append("    AND T1.INUSE_FLG = :DELFLG ")
                        query.AddParameterWithTypeValue("DELFLG", OracleDbType.Char, inUseFlg)
                    End If
                    ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END
                End With

                query.CommandText = sql.ToString()

                ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
                Dim dt As BranchDataSet.BRANCHDataTable = query.GetData()

                ' ======================== ログ出力 開始 ========================
                Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                          " {0}_End, Return:[{1}]",
                                          MethodBase.GetCurrentMethod.Name, dt.Rows.Count))
                ' ======================== ログ出力 終了 ========================

                Return dt
                ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END

            End Using

        End Function
#End Region

    End Class

End Namespace
