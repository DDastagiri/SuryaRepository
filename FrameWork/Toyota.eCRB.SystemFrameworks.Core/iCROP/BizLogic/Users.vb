Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess

Namespace Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic

    ''' <summary>
    ''' TBL_USERSのデータを取得する共通クラスです。
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Users
        Inherits BaseBusinessComponent

#Region "GetAllUsers"
        ''' <summary>
        ''' 全ユーザー情報を取得します。
        ''' </summary>
        ''' <param name="dlrCd">販売店コード</param>
        ''' <param name="strCd">店舗コード</param>
        ''' <param name="operationCdList">オペレーションコード</param>
        ''' <param name="delFlg">削除フラグ</param>
        ''' <returns>USERSDataTable</returns>
        ''' <remarks>
        ''' 全ユーザー情報を取得します。
        ''' データが0件のとき、0件のDataTableを返却します。
        ''' </remarks>
        Public Function GetAllUser(ByVal dlrCD As String,
                                    Optional ByVal strCD As String = Nothing,
                                    Optional ByVal operationCDList As List(Of Decimal) = Nothing,
                                    Optional ByVal delFlg As String = Nothing) As UsersDataSet.USERSDataTable

            If String.IsNullOrEmpty(dlrCD) Then
                Return New UsersDataSet.USERSDataTable
            End If

            Return UsersTableAdapter.GetUsersDataTable(dlrCD, strCD, operationCDList, delFlg)

        End Function
#End Region

#Region "GetUser"
        ''' <summary>
        ''' 指定ユーザー情報を取得します。
        ''' </summary>
        ''' <param name="account">アカウント</param>
        ''' <param name="delFlg">削除フラグ</param>
        ''' <returns>USERSRow</returns>
        ''' <remarks>
        ''' 指定ユーザー情報を取得します。
        ''' データが0件のとき、Nothingを返却します。
        ''' </remarks>
        Public Function GetUser(ByVal account As String,
                                       Optional ByVal delFlg As String = Nothing) As UsersDataSet.USERSRow

            If String.IsNullOrEmpty(account) Then
                Return Nothing
            End If

            Dim userDt As UsersDataSet.USERSDataTable
            userDt = UsersTableAdapter.GetUsersDataTable(account, delFlg)

            If userDt.Rows.Count = 0 Then
                Return Nothing
            End If

            Return DirectCast(userDt.Rows(0), UsersDataSet.USERSRow)

        End Function
#End Region

    End Class

End Namespace

