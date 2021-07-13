Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.Visit.Api.DataAccess
Imports Toyota.eCRB.Visit.Api.DataAccess.VisitUtilityDataSet
Imports Toyota.eCRB.Visit.Api.DataAccess.VisitUtilityDataSetTableAdapters
Imports System.Text

''' <summary>
''' 共通ロジック
''' </summary>
''' <remarks></remarks>
Public Class VisitUtilityBusinessLogic
    Inherits BaseBusinessComponent

#Region "定数"

    ''' <summary>
    ''' 翌日
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NextDay As Double = 1.0

    ''' <summary>
    ''' 1ミリ秒前
    ''' </summary>
    ''' <remarks></remarks>
    Private Const BeforMillisecond As Double = -1.0

    ''' <summary>
    ''' 在席状態(大分類)：スタンバイ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PresenceCategoryStandby As String = "1"

    ''' <summary>
    ''' 在席状態(大分類)：商談中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PresenceCategoryNegotiate As String = "2"

    ''' <summary>
    ''' 在席状態(大分類)：退席中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PresenceCategoryLeaving As String = "3"

    ''' <summary>
    ''' 削除フラグ（未削除）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DeleteFlagNone As String = "0"

#End Region

#Region "苦情情報有無の取得"

    ''' <summary>
    ''' 苦情情報有無の取得
    ''' </summary>
    ''' <param name="customerKind">顧客種別</param>
    ''' <param name="customerCode">顧客コード</param>
    ''' <param name="nowDate">現在日時</param>
    ''' <param name="complaintDateCount">苦情表示日数</param>
    ''' <returns>苦情情報有無</returns>
    ''' <remarks></remarks>
    Public Function HasClaimInfo(ByVal customerKind As String, ByVal customerCode As String, _
                                  ByVal nowDate As Date, ByVal complaintDateCount As Integer) As Boolean
        Logger.Info("HasClaimInfo_Start " & _
                    "Param[" & customerKind & ", " & customerCode & _
                    ", " & nowDate & ", " & complaintDateCount & "]")

        '当日の00:00:00を格納
        Dim startDate As Date = nowDate.Date
        'Logger.Info("HasClaimInfo_001 startDate[" & startDate & "]")

        '苦情表示期間の設定
        Dim completeDate As Date = startDate.AddDays(-complaintDateCount)
        'Logger.Info("HasClaimInfo_002 completeDate[" & completeDate & "]")

        Dim hasClaim As Boolean = False

        Using dataAdapter As New VisitUtilityDataSetTableAdapter

            '苦情情報有無の取得
            hasClaim = dataAdapter.HasClaimInfo(customerKind, customerCode, completeDate)
        End Using

        Logger.Info("HasClaimInfo_End Ret[" & hasClaim & "]")
        Return hasClaim
    End Function
#End Region

#Region "オンラインユーザー情報の取得"

    ''' <summary>
    ''' 全オンラインユーザー情報を取得します。
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="storeCode">店舗コード</param>
    ''' <param name="operationCodeList">オペレーションコード</param>
    ''' <returns>VisitUtilityUsersDataTable</returns>
    ''' <remarks>
    ''' 在席状態がオンラインの全ユーザー情報を取得します。
    ''' データが0件のとき、0件のDataTableを返却します。
    ''' </remarks>
    Public Function GetOnlineUsers(ByVal dealerCode As String,
                                   Optional ByVal storeCode As String = Nothing,
                                   Optional ByVal operationCodeList As List(Of Decimal) = Nothing) As VisitUtilityUsersDataTable
        ' Logger.Info("GetOnlineUsers_Start Param[" & dealerCode & ", " _
        '             & "storeCode is Nothing:" & IsNothing(storeCode).ToString & ", " _
        '             & "operationCDList is Nothing:" & IsNothing(operationCodeList).ToString & "]")

        ' オンラインの在籍状態リストを生成
        Dim onlineList As New List(Of String)
        onlineList.Add(PresenceCategoryStandby)
        onlineList.Add(PresenceCategoryNegotiate)
        onlineList.Add(PresenceCategoryLeaving)

        Dim usersDataTable As VisitUtilityUsersDataTable
        usersDataTable = Me.GetUsers(dealerCode, storeCode, operationCodeList, onlineList, DeleteFlagNone)

        ' Logger.Info("GetOnlineUsers_End Ret[usersDataTable[Count = " & usersDataTable.Count & "]]")
        Return usersDataTable

    End Function
#End Region

#Region "ユーザー情報の取得"
    ''' <summary>
    ''' 全ユーザー情報を取得します。
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="storeCode">店舗コード</param>
    ''' <param name="operationCodeList">オペレーションコード</param>
    ''' <param name="presenceCategoryList">在席状態</param>
    ''' <param name="deleteCode">削除フラグ</param>
    ''' <param name="account">アカウント</param>
    ''' <returns>VisitUtilityUsersDataTable</returns>
    ''' <remarks>
    ''' 全ユーザー情報を取得します。
    ''' データが0件のとき、0件のDataTableを返却します。
    ''' </remarks>
    Public Function GetUsers(ByVal dealerCode As String,
                             Optional ByVal storeCode As String = Nothing,
                             Optional ByVal operationCodeList As List(Of Decimal) = Nothing,
                             Optional ByVal presenceCategoryList As List(Of String) = Nothing,
                             Optional ByVal deleteCode As String = Nothing,
                             Optional ByVal account As String = Nothing) As VisitUtilityUsersDataTable
        Logger.Info("GetUsers_Start Param[" & dealerCode & ", " _
                     & "storeCode is Nothing:" & IsNothing(storeCode).ToString & ", " _
                     & "operationCDList is Nothing:" & IsNothing(operationCodeList).ToString & ", " _
                     & "presenceCategoryList is Nothing:" & IsNothing(presenceCategoryList).ToString & ", " _
                     & "deleteFlag is Nothing:" & IsNothing(deleteCode).ToString & "," _
                     & "account is Nothing:" & IsNothing(account).ToString & "]")

        If String.IsNullOrEmpty(dealerCode) Then
            Logger.Info("GetUsers_001 dealerCode is Nothing")
            Logger.Info("GetUsers_End Ret[usersDataTable[Count = 0]]")
            Return New VisitUtilityUsersDataTable
        End If

        Dim usersDataTable As VisitUtilityUsersDataTable
        usersDataTable = VisitUtilityDataSetTableAdapter.GetUsers(dealerCode, storeCode, operationCodeList, presenceCategoryList, deleteCode, account)

        Logger.Info("GetUsers_End Ret[usersDataTable[Count = " & usersDataTable.Count & "]]")
        Return usersDataTable

    End Function
#End Region

End Class
