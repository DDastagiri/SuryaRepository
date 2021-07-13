'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3180205BusinessLogic.vb
'─────────────────────────────────────
'機能： 承認者選択画面
'補足： 
'作成： 2014/01/21 TMEJ小澤	初版作成
'更新： 
'─────────────────────────────────────
Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.AddRepair.AddRepairConfirm.DataAccess.SC3180205DataSet
Imports Toyota.eCRB.AddRepair.AddRepairConfirm.DataAccess.SC3180205DataSetTableAdapters

Public Class SC3180205BusinessLogic
    Inherits BaseBusinessComponent
    Implements IDisposable

#Region "定数"

    ''' <summary>
    ''' リターンコード
    ''' </summary>
    Private Enum ReturnCode

        ''' <summary>
        ''' 成功
        ''' </summary>
        Success = 0

        ''' <summary>
        ''' DBタイムアウト
        ''' </summary>
        ErrDBTimeout = 901

    End Enum

#End Region

#Region "メイン処理"

    ''' <summary>
    ''' ユーザー情報取得
    ''' </summary>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inStoreCode">店舗コード</param>
    ''' <returns>ユーザー情報</returns>
    ''' <remarks></remarks>
    Public Function GetUserInfo(ByVal inDealerCode As String, _
                                ByVal inStoreCode As String) As SC3180205UserInfoDataTable
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START IN:inDealerCode = {2},inStoreCode = {3}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , inDealerCode _
                    , inStoreCode))

        Using da As New SC3180205DataTableAdapter
            'ユーザー情報取得
            Dim dt As SC3180205UserInfoDataTable = _
                da.GetUserInfo(inDealerCode, inStoreCode)

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return dt
        End Using
    End Function

#End Region

#Region "IDisposable Support"
    Private disposedValue As Boolean ' 重複する呼び出しを検出するには

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: マネージ状態を破棄します (マネージ オブジェクト)。
            End If

            ' TODO: アンマネージ リソース (アンマネージ オブジェクト) を解放し、下の Finalize() をオーバーライドします。
            ' TODO: 大きなフィールドを null に設定します。
        End If
        Me.disposedValue = True
    End Sub

    ' このコードは、破棄可能なパターンを正しく実装できるように Visual Basic によって追加されました。
    Public Sub Dispose() Implements IDisposable.Dispose
        ' このコードを変更しないでください。クリーンアップ コードを上の Dispose(ByVal disposing As Boolean) に記述します。
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

#End Region

End Class

