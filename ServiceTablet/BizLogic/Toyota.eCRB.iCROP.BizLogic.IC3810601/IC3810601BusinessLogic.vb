'-------------------------------------------------------------------------
'IC3810601BusinessLogic.vb
'-------------------------------------------------------------------------
'機能：ユーザーステータス取得API
'補足：
'作成： 2012/07/04 TMEJ 河原
'─────────────────────────────────────
Imports System.Text
Imports System.Net
Imports System.IO
Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SMBLinkage.GetUserList.Api.DataAccess

Public Class IC3810601BusinessLogic
    Inherits BaseBusinessComponent
    Implements IDisposable

#Region "定数"

    ''' <summary>
    ''' リターンCD(該当無し)
    ''' </summary>
    Private Const ReturnCodeNothing As String = "902"
    ''' <summary>
    ''' リターンCD(成功)
    ''' </summary>
    Private Const ReturnCodeSuccess As String = "0"

#End Region

#Region "メソッド"

    ''' <summary>
    ''' ユーザーステータスを取得
    ''' </summary>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inStoreCode">店舗コード</param>
    ''' <param name="inOperationCodeList">操作権限コードリスト</param>
    ''' <returns>ユーザのスタッフ情報</returns>
    ''' <remarks></remarks>
    Public Function GetAcknowledgeStaffList(ByVal inDealerCode As String, _
                                            ByVal inStoreCode As String, _
                                            ByVal inOperationCodeList As List(Of Long)) _
                                            As IC3810601DataSet.AcknowledgeStaffListDataTable

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                 , "{0}.{1} P1:{2} P2:{3} " _
                                 , Me.GetType.ToString _
                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                 , inDealerCode, inStoreCode))
        'リターンコード
        Dim returnCode As String

        'データテーブル
        Dim dt As IC3810601DataSet.AcknowledgeStaffListDataTable = Nothing
        Try
            '値チェック(操作権限コード)
            If inOperationCodeList IsNot Nothing _
            AndAlso Not inOperationCodeList.Count = 0 Then
                'ユーザのスタッフ情報取得
                Using dataSet As New IC3810601DataSetTableAdapters.IC3810601TableAdapter
                    dt = dataSet.GetDBAcknowledgeStaffList(inDealerCode, inStoreCode, inOperationCodeList)

                    'セレクト結果の確認
                    If dt.Rows.Count = 0 Then
                        '取得0件
                        returnCode = ReturnCodeNothing
                    Else
                        '成功
                        returnCode = ReturnCodeSuccess
                    End If
                End Using
            Else '引数エラー
                'コード902
                returnCode = ReturnCodeNothing
                '0件データテーブル
                dt = New IC3810601DataSet.AcknowledgeStaffListDataTable
            End If
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} RETURNCODE = {2} DT.COUNT = {3}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , returnCode, dt.Rows.Count))
            Return dt
        Finally '最終処理
            If (Not dt Is Nothing) Then dt.Dispose()
        End Try
    End Function

#End Region

    ''' <summary>
    ''' IDisposable.Dispoase
    ''' </summary>
    ''' <remarks></remarks>
    Public Overloads Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
    Protected Overridable Overloads Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
        End If
    End Sub
End Class
