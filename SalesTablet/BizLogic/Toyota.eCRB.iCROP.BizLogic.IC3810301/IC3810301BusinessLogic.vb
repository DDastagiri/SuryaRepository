'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'IC3810301BusinessLogic.vb
'─────────────────────────────────────
'機能： R/O連携ビジネスロジック
'補足： 
'作成： 2012/01/26 KN 瀧
'更新： 
'─────────────────────────────────────

Imports System.Xml
Imports System.Text
Imports System.Web
Imports System.Reflection
Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.iCROP.DataAccess.IC3810301.IC3810301DataSet
Imports Toyota.eCRB.iCROP.DataAccess.IC3810301.IC3810301DataSetTableAdapters

''' <summary>
''' IC3810301
''' </summary>
''' <remarks>R/O連携</remarks>
Public Class IC3810301BusinessLogic
    Inherits BaseBusinessComponent
    Implements IDisposable

    ''' <summary>
    ''' 成功
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ResultSuccess As Long = 0
    ''' <summary>
    ''' エラー:SAコードが異なる
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ResultDiffSACode As Long = 1
    ''' <summary>
    ''' エラー:DBタイムアウト
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ResultDBTimeout As Long = 901
    ''' <summary>
    ''' エラー:該当データなし
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ResultNoMatch As Long = 902

    ''' <summary>
    ''' R/O画面仕掛中反映
    ''' </summary>
    ''' <param name="rowIN">R/O画面仕掛中反映引数</param>
    ''' <returns>登録結果</returns>
    ''' <remarks></remarks>
    ''' 
    ''' <history>
    ''' </history>
    <EnableCommit()>
    Public Function AddOrderSave(ByVal rowIN As IC3810301inOrderSaveRow) As Long
        Try
            ''引数をログに出力
            Dim args As New List(Of String)
            For Each column As DataColumn In rowIN.Table.Columns
                If rowIN.IsNull(column.ColumnName) = True Then
                    args.Add(String.Format(CultureInfo.CurrentCulture, "{0} = NULL", column.ColumnName))
                Else
                    args.Add(String.Format(CultureInfo.CurrentCulture, "{0} = {1}", column.ColumnName, rowIN(column.ColumnName)))
                End If
            Next
            ''開始ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} IN:{2}" _
                , Me.GetType.ToString _
                , MethodBase.GetCurrentMethod.Name _
                , String.Join(", ", args.ToArray())))

            Using da As New IC3810301DataTableAdapter
                ''来店実績番号の入力チェック
                If rowIN.IsVISITSEQNull = False _
                    AndAlso (rowIN.VISITSEQ > 0) Then
                    ''来店実績番号が入力されている場合、修正更新
                    ''サービス来店者キー情報の取得
                    Using dtVisit As IC3810301VisitKeyDataTable = da.GetVisitKey(rowIN)
                        If dtVisit.Rows.Count = 0 Then
                            ''該当データが存在しない場合
                            ''ログの出力
                            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} OUT:RETURNCODE = {2}" _
                                , Me.GetType.ToString _
                                , MethodBase.GetCurrentMethod.Name _
                                , ResultNoMatch))
                            Return ResultNoMatch
                        End If
                        Dim rowVK As IC3810301VisitKeyRow = DirectCast(dtVisit.Rows(0), IC3810301VisitKeyRow)
                        If (rowVK.IsSACODENull = True) _
                            OrElse (String.Compare(rowIN.SACODE, rowVK.SACODE, True, CultureInfo.CurrentCulture) <> 0) Then
                            ''SAコードチェック
                            ''SAコードが異なる場合はエラー
                            ''ログの出力
                            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} OUT:RETURNCODE = {2}" _
                                , Me.GetType.ToString _
                                , MethodBase.GetCurrentMethod.Name _
                                , ResultDiffSACode))
                            Return ResultDiffSACode
                        End If
                    End Using
                    ''修正更新処理
                    da.UpdateVisitOrder(rowIN)
                Else
                    ''来店実績番号が入力されていない場合、新規登録
                    ''新規登録処理
                    rowIN.VISITSEQ = da.InsertVisitOrder(rowIN)
                End If
            End Using
            ''終了ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} OUT:RETURNCODE = {2}" _
                , Me.GetType.ToString _
                , MethodBase.GetCurrentMethod.Name _
                , ResultSuccess))
            Return ResultSuccess
        Catch ex As OracleExceptionEx When ex.Number = 1013
            ''ORACLEのタイムアウトのみ処理
            Me.Rollback = True
            ''終了ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} OUT:RETURNCODE = {2}" _
                , Me.GetType.ToString _
                , MethodBase.GetCurrentMethod.Name _
                , ResultDBTimeout))
            Return ResultDBTimeout
        Catch ex As Exception
            Me.Rollback = True
            ''エラーログの出力
            Logger.Error(ex.Message, ex)
            Throw
        Finally
            ''終了処理

        End Try
    End Function

    ''' <summary>
    ''' R/Oキャンセル
    ''' </summary>
    ''' <param name="rowIN">R/Oキャンセル引数</param>
    ''' <returns>登録結果</returns>
    ''' <remarks></remarks>
    ''' 
    ''' <history>
    ''' </history>
    <EnableCommit()>
    Public Function DeleteOrderSave(ByVal rowIN As IC3810301inOrderSaveRow) As Long
        Try
            ''引数をログに出力
            Dim args As New List(Of String)
            For Each column As DataColumn In rowIN.Table.Columns
                If rowIN.IsNull(column.ColumnName) = True Then
                    args.Add(String.Format(CultureInfo.CurrentCulture, "{0} = NULL", column.ColumnName))
                Else
                    args.Add(String.Format(CultureInfo.CurrentCulture, "{0} = {1}", column.ColumnName, rowIN(column.ColumnName)))
                End If
            Next
            ''開始ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} IN:{2}" _
                , Me.GetType.ToString _
                , MethodBase.GetCurrentMethod.Name _
                , String.Join(", ", args.ToArray())))

            Using da As New IC3810301DataTableAdapter
                ''サービス来店者キー情報の取得
                Using dtVisit As IC3810301VisitKeyDataTable = da.GetVisitKey(rowIN)
                    If dtVisit.Rows.Count = 0 Then
                        ''該当データが存在しない場合
                        ''ログの出力
                        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} OUT:RETURNCODE = {2}" _
                            , Me.GetType.ToString _
                            , MethodBase.GetCurrentMethod.Name _
                            , ResultNoMatch))
                        Return ResultNoMatch
                    End If
                    Dim rowVK As IC3810301VisitKeyRow = DirectCast(dtVisit.Rows(0), IC3810301VisitKeyRow)
                    If (rowVK.IsSACODENull = True) _
                        OrElse (String.Compare(rowIN.SACODE, rowVK.SACODE, True, CultureInfo.CurrentCulture) <> 0) Then
                        ''SAコードチェック
                        ''SAコードが異なる場合はエラー
                        ''ログの出力
                        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} OUT:RETURNCODE = {2}" _
                            , Me.GetType.ToString _
                            , MethodBase.GetCurrentMethod.Name _
                            , ResultDiffSACode))
                        Return ResultDiffSACode
                    End If
                End Using
                ''キャンセル処理
                da.DeleteVisitOrder(rowIN)
            End Using
            ''終了ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} OUT:RETURNCODE = {2}" _
                , Me.GetType.ToString _
                , MethodBase.GetCurrentMethod.Name _
                , ResultSuccess))
            Return ResultSuccess
        Catch ex As OracleExceptionEx When ex.Number = 1013
            ''ORACLEのタイムアウトのみ処理
            Me.Rollback = True
            ''終了ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} OUT:RETURNCODE = {2}" _
                , Me.GetType.ToString _
                , MethodBase.GetCurrentMethod.Name _
                , ResultDBTimeout))
            Return ResultDBTimeout
        Catch ex As Exception
            Me.Rollback = True
            ''エラーログの出力
            Logger.Error(ex.Message, ex)
            Throw
        Finally

        End Try
    End Function

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
