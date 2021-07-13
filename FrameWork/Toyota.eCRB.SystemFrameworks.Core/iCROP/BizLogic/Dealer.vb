Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess

Namespace Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic

    ''' <summary>
    ''' TBLM_DEALERのデータを取得する共通クラスです。
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Dealer
        Inherits BaseBusinessComponent

#Region "全販売店を取得"
        ''' <summary>
        ''' 全販売店情報を取得します。
        ''' </summary>
        ''' <param name="delFlg">削除フラグ</param>
        ''' <returns>DEALERDataTable</returns>
        ''' <remarks>
        ''' 全販売店情報を取得します。
        ''' データが0件のとき、0件のDataTableを返却します。
        ''' </remarks>
        Public Function GetAllDealer(Optional ByVal delFlg As String = Nothing) As DealerDataSet.DEALERDataTable

            Dim dlrDt As DealerDataSet.DEALERDataTable
            dlrDt = DealerTableAdapter.GetDealerDataTable(Nothing, delFlg)

            Return dlrDt

        End Function
#End Region

#Region "指定販売店"
        ''' <summary>
        ''' 指定販売店情報を取得します。
        ''' </summary>
        ''' <param name="dlrCd">販売店コード</param>
        ''' <param name="delFlg">削除フラグ</param>
        ''' <returns>DEALERRow</returns>
        ''' <remarks>
        ''' 指定販売店情報を取得します。
        ''' データが0件のとき、Nothingを返却します。
        ''' </remarks>
        Public Function GetDealer(ByVal dlrCD As String, Optional ByVal delFlg As String = Nothing) As DealerDataSet.DEALERRow

            If String.IsNullOrEmpty(dlrCD) Then
                Return Nothing
            End If

            Dim dlrDt As DealerDataSet.DEALERDataTable
            dlrDt = DealerTableAdapter.GetDealerDataTable(dlrCD, delFlg)

            If dlrDt.Rows.Count = 0 Then
                Return Nothing
            End If

            Return DirectCast(dlrDt.Rows(0), DealerDataSet.DEALERRow)

        End Function
#End Region

    End Class


End Namespace
