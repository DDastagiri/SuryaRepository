Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess

Namespace Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic

    ''' <summary>
    ''' TBL_OPERATIONTYPEのデータを取得する共通クラスです。
    ''' </summary>
    ''' <remarks></remarks>
    Public Class OperationType
        Inherits BaseBusinessComponent

#Region "GetAllOperationType"
        ''' <summary>
        ''' 全権限情報を取得します。
        ''' </summary>
        ''' <param name="dlrCd">販売店コード</param>
        ''' <param name="operationCdList">オペレーションコード</param>
        ''' <param name="delFlg">削除フラグ</param>
        ''' <returns>OPERATIONTYPEDataTable</returns>
        ''' <remarks>
        ''' 全権限情報を取得します。
        ''' データが0件のとき、0件のDataTableを返却します。
        ''' </remarks>
        Public Function GetAllOperationType(ByVal dlrCD As String,
                                            Optional ByVal operationCDList As List(Of Decimal) = Nothing,
                                            Optional ByVal delFlg As String = Nothing) As OperationTypeDataSet.OPERATIONTYPEDataTable

            If String.IsNullOrEmpty(dlrCD) Then
                Return New OperationTypeDataSet.OPERATIONTYPEDataTable
            End If

            Return OperationTypeTableAdapter.GetOperationTypeDataTable(dlrCD, ConstantBranchCD.BranchHO, operationCDList, delFlg)

        End Function
#End Region

#Region "GetSelectOperationType"
        ''' <summary>
        ''' 指定権限情報を取得します。
        ''' </summary>
        ''' <param name="dlrCd">販売店コード</param>
        ''' <param name="operationCd">オペレーションコード</param>
        ''' <param name="delFlg">削除フラグ</param>
        ''' <returns>OPERATIONTYPERow</returns>
        ''' <remarks>
        ''' 指定権限情報を取得します。
        ''' データが0件のとき、Nothingを返却します。
        ''' </remarks>
        Public Function GetOperationType(ByVal dlrCD As String,
                                               ByVal operationCD As Decimal,
                                               Optional ByVal delFlg As String = Nothing) As OperationTypeDataSet.OPERATIONTYPERow

            If String.IsNullOrEmpty(dlrCD) Then
                Return Nothing
            End If

            Dim operationCdList As New List(Of Decimal)
            operationCdList.Add(operationCD)

            Dim opeDt As OperationTypeDataSet.OPERATIONTYPEDataTable = OperationTypeTableAdapter.GetOperationTypeDataTable(dlrCD, ConstantBranchCD.BranchHO, operationCdList, delFlg)

            If opeDt.Rows.Count() = 0 Then
                Return Nothing
            End If

            Return DirectCast(opeDt.Rows(0), OperationTypeDataSet.OPERATIONTYPERow)

        End Function
#End Region
    End Class

End Namespace
