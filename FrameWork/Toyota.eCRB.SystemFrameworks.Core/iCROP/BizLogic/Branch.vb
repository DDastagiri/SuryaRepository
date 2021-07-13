Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess

Namespace Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic

    Public Class Branch
        Inherits BaseBusinessComponent

#Region "GetAllBranch"
        ''' <summary>
        ''' 全店舗情報を取得します。
        ''' </summary>
        ''' <param name="dlrCd">販売店コード</param>
        ''' <param name="delFlg">削除フラグ</param>
        ''' <returns>BRANCHDataTable</returns>
        ''' <remarks>
        ''' 全店舗情報を取得します。
        ''' データが0件のとき、0件のDataTableを返却します。
        ''' </remarks>
        Public Function GetAllBranch(ByVal dlrCD As String,
                                     Optional ByVal delFlg As String = Nothing) As BranchDataSet.BRANCHDataTable

            If String.IsNullOrEmpty(dlrCD) Then
                Return New BranchDataSet.BRANCHDataTable
            End If

            Return BranchTableAdapter.GetBranchDataTable(dlrCD, Nothing, delFlg)

        End Function
#End Region

#Region "GetBranch"
        ''' <summary>
        ''' 指定店舗情報を取得します。
        ''' </summary>
        ''' <param name="dlrCd">販売店コード</param>
        ''' <param name="strCd">店舗コード</param>
        ''' <param name="delFlg">削除フラグ</param>
        ''' <returns>BRANCHRow</returns>
        ''' <remarks>
        ''' 指定店舗情報を取得します。
        ''' データが0件のとき、Nothingを返却します。
        ''' </remarks>
        Public Function GetBranch(ByVal dlrCD As String,
                                        ByVal strCD As String,
                                        Optional ByVal delFlg As String = Nothing) As BranchDataSet.BRANCHRow

            If String.IsNullOrEmpty(dlrCD) OrElse String.IsNullOrEmpty(strCD) Then
                Return Nothing
            End If

            Dim braDt As BranchDataSet.BRANCHDataTable
            braDt = BranchTableAdapter.GetBranchDataTable(dlrCD, strCD, delFlg)

            If braDt.Rows.Count = 0 Then
                Return Nothing
            End If

            Return DirectCast(braDt.Rows(0), BranchDataSet.BRANCHRow)

        End Function
#End Region

    End Class

End Namespace
