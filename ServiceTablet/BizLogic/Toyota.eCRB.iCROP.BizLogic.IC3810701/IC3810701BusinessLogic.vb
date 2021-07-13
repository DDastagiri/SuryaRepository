'-------------------------------------------------------------------------
'IC3810701BusinessLogic.vb
'-------------------------------------------------------------------------
'機能：サービス標準LT取得API
'補足：
'作成： 2012/05/11 KN 河原
'─────────────────────────────────────
Imports System.Text
Imports System.Net
Imports System.IO
Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SMBLinkage.GetServiceLT.Api.DataAccess

Public Class IC3810701BusinessLogic
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
    ''' サービス標準LT取得
    ''' </summary>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inStoreCode">店舗コード</param>
    ''' <returns>標準LT</returns>
    ''' <remarks></remarks>
    Public Function GetStandardLTList(ByVal inDealerCode As String, _
                                      ByVal inStoreCode As String) _
                                      As IC3810701DataSet.StandardLTListDataTable

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                 , "{0}.{1} P1:{2} P2:{3} " _
                                 , Me.GetType.ToString _
                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                 , inDealerCode, inStoreCode))
        'リターンコード
        Dim returnCode As String
        'データテーブル
        Dim dt As IC3810701DataSet.StandardLTListDataTable
        'サービス標準LT取得
        Using dataSet As New IC3810701DataSetTableAdapters.IC3810701TableAdapter
            dt = dataSet.GetDBStandardLTList(inDealerCode, inStoreCode)

            'セレクト結果の確認
            If dt.Rows.Count = 0 Then
                '取得0件
                returnCode = ReturnCodeNothing
            Else
                ' 成功
                returnCode = ReturnCodeSuccess
            End If

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} RETURNCODE = {2} DT.COUNT = {3}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , returnCode, dt.Rows.Count))

            Return dt
        End Using
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
