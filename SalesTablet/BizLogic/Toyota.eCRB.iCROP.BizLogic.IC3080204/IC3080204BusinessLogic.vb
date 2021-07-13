'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'IC3080204BusinessLogic.vb
'─────────────────────────────────────
'機能： 顧客コードを変換する
'補足： 基幹顧客ID(DMSID)から自社客連番(ORIGINALID)を取得する
'作成： 2012/02/15 KN 佐藤（真）
'更新： 
'─────────────────────────────────────

Imports System.Reflection
Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.iCROP.DataAccess.IC3080204

''' <summary>
''' IC3080204
''' Webページで使用するビジネスロジック
''' </summary>
''' <remarks></remarks>
Public Class IC3080204BusinessLogic
    Inherits BaseBusinessComponent
    Implements IDisposable

#Region " 定数 "

    ''' <summary>
    ''' 成功
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ConstReturnSuccess As Long = 0

    ''' <summary>
    ''' 該当データなし
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ConstReturnNoMatch As Long = 902

#End Region

    ''' <summary>
    ''' 顧客詳細(顧客コード変換)
    ''' </summary>
    ''' <param name="dealerCD">販売店コード</param>
    ''' <param name="storeCD">店舗コード</param>
    ''' <param name="basicCustomerId">基幹顧客ID(DMSID)</param>
    ''' <returns>自社客個人情報データテーブル</returns>
    ''' <remarks>基幹顧客ID(DMSID)から自社客連番(ORIGINALID)を取得</remarks>
    ''' 
    ''' <History>
    ''' </History>
    Public Function GetMyCustomerInfo(ByVal dealerCD As String _
                                      , ByVal storeCD As String _
                                      , ByVal basicCustomerId As String) As IC3080204DataSet.IC3080204CustomerDataTable

        '引数を編集
        Dim args As New List(Of String)
        args.Add(String.Format(CultureInfo.InvariantCulture, "{0} = {1}", MethodBase.GetCurrentMethod.GetParameters(0).Name, basicCustomerId))
        '開始ログを出力
        OutPutStartLog(MethodBase.GetCurrentMethod.Name, args)

        Using da As New IC3080204DataSetTableAdapters.IC3080204DataTableTableAdapter
            ' 基幹顧客IDを取得
            ' 自社客個情報を取得
            Using dt As IC3080204DataSet.IC3080204CustomerDataTable = da.GetMyCustomer(dealerCD, storeCD, basicCustomerId)
                ' 終了ログを出力
                If dt.Rows.Count = 0 Then
                    ' データなし
                    OutPutEndLog(MethodBase.GetCurrentMethod.Name, ConstReturnNoMatch)
                Else
                    ' データあり
                    OutPutEndLog(MethodBase.GetCurrentMethod.Name, ConstReturnSuccess)
                End If
                Return dt
            End Using
        End Using

    End Function

    ''' <summary>
    ''' 開始ログ出力
    ''' </summary>
    ''' <param name="methodName">メソッド名</param>
    ''' <param name="args">引数</param>
    ''' <remarks></remarks>
    ''' 
    ''' <History>
    ''' </History>
    Private Sub OutPutStartLog(ByVal methodName As String, ByVal args As List(Of String))

        '引数をログに出力
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
            "{0}.{1} IN:{2}" _
            , Me.GetType.ToString _
            , methodName _
            , String.Join(", ", args.ToArray())))

    End Sub

    ''' <summary>
    ''' 終了ログ出力
    ''' </summary>
    ''' <param name="methodName">メソッド名</param>
    ''' <param name="returnCD">リターンコード</param>
    ''' <remarks></remarks>
    ''' 
    ''' <History>
    ''' </History>
    Private Sub OutPutEndLog(ByVal methodName As String, ByVal returnCD As Long)

        'ログに出力
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
            "{0}.{1} OUT:RETURNCODE = {2}" _
            , Me.GetType.ToString _
            , methodName _
            , returnCD))

    End Sub

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

    ' TODO: 上の Dispose(ByVal disposing As Boolean) にアンマネージ リソースを解放するコードがある場合にのみ、Finalize() をオーバーライドします。
    'Protected Overrides Sub Finalize()
    '    ' このコードを変更しないでください。クリーンアップ コードを上の Dispose(ByVal disposing As Boolean) に記述します。
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' このコードは、破棄可能なパターンを正しく実装できるように Visual Basic によって追加されました。
    Public Sub Dispose() Implements IDisposable.Dispose
        ' このコードを変更しないでください。クリーンアップ コードを上の Dispose(ByVal disposing As Boolean) に記述します。
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class
