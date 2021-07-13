'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'IC3810401BusinessLogic.vb
'─────────────────────────────────────
'機能： R/O,REZ連携ビジネスロジック
'補足： 
'作成： 2012/01/26 KN 瀧
'更新： 2012/02/16 KN 瀧 【SERVICE_1】サービス来店者管理テーブルの更新方法を変更
'更新： 2012/02/17 KN 瀧 【SERVICE_1】ストール予約テーブルの登録項目を追加
'更新： 2012/02/23 KN 瀧 【SERVICE_1】ストップフラグの変更(TEMPorWALKINの判定を追加)
'更新： 2012/02/24 KN 瀧 【SERVICE_1】ストール予約テーブルの更新方法を変更
'更新： 2012/02/27 KN 瀧 【SERVICE_1】ストール予約テーブルの登録項目を追加
'更新： 2012/03/03 KN 瀧 【SERVICE_1】ストール予約テーブルの登録項目を追加２
'更新： 2012/03/03 KN 瀧 【SERVICE_1】引数に車名、モデルコードを追加
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
Imports Toyota.eCRB.iCROP.DataAccess.IC3810401.IC3810401DataSet
Imports Toyota.eCRB.iCROP.DataAccess.IC3810401.IC3810401DataSetTableAdapters

''' <summary>
''' IC3810401
''' </summary>
''' <remarks>R/O,REZ連携</remarks>
Public Class IC3810401BusinessLogic
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
    ''' エラー:整備受注Noが未入力
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ResultRequiredOrderNo As Long = 2
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
    ''' 予約情報更新
    ''' </summary>
    ''' <param name="rowIN">予約情報更新引数</param>
    ''' <returns>登録結果</returns>
    ''' <remarks></remarks>
    ''' 
    ''' <history>
    ''' 2012/02/16 KN 瀧 【SERVICE_1】サービス来店者管理テーブルの更新方法を変更
    ''' 2012/02/24 KN 瀧 【SERVICE_1】ストール予約テーブルの更新方法を変更
    ''' 2012/02/27 KN 瀧 【SERVICE_1】ストール予約テーブルの登録項目を追加
    ''' </history>
    <EnableCommit()>
    Public Function UpdateOrderSave(ByVal rowIN As IC3810401InOrderSaveRow) As Long
        ''引数をログに出力
        Dim args As New List(Of String)
        ' DataRow内の項目を列挙
        Me.AddLogData(args, rowIN)
        ''開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} IN:{2}" _
            , Me.GetType.ToString _
            , MethodBase.GetCurrentMethod.Name _
            , String.Join(", ", args.ToArray())))

        '2012/02/16 KN 瀧 【SERVICE_1】サービス来店者管理テーブルの更新方法を変更 START
        ''入力チェック
        ''整備受注No
        If rowIN.IsORDERNONull = True _
            OrElse rowIN.ORDERNO.Trim.Length = 0 Then
            ''整備受注Noが未入力の場合
            ''ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} OUT:RETURNCODE = {2}" _
                , Me.GetType.ToString _
                , MethodBase.GetCurrentMethod.Name _
                , ResultRequiredOrderNo))
            Return ResultRequiredOrderNo
        End If
        '2012/02/16 KN 瀧 【SERVICE_1】サービス来店者管理テーブルの更新方法を変更 END
        Try
            Using da As New IC3810401DataTableAdapter
                ''サービス来店者管理情報の取得
                Using dtVisit As IC3810401VisitKeyDataTable = da.GetVisitKey(rowIN)
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
                    Dim rowVK As IC3810401VisitKeyRow = DirectCast(dtVisit.Rows(0), IC3810401VisitKeyRow)
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

                '2012/02/24 KN 瀧 【SERVICE_1】ストール予約テーブルの更新方法を変更 START
                Using dtStall As IC3810401StallKeyDataTable = da.GetStallKey(rowIN)
                    If rowIN.IsREZIDNull = False _
                        AndAlso dtStall.Rows.Count = 0 Then
                        ''引数に予約IDが存在し、該当データが存在しない場合、エラー処理
                        ''ログの出力
                        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} OUT:RETURNCODE = {2}" _
                            , Me.GetType.ToString _
                            , MethodBase.GetCurrentMethod.Name _
                            , ResultNoMatch))
                        Return ResultNoMatch
                    ElseIf rowIN.IsREZIDNull = True _
                        AndAlso dtStall.Rows.Count > 0 Then
                        ''引数に予約IDが存在せず、該当データが存在する場合、引数に予約IDを設定
                        rowIN.REZID = DirectCast(dtStall.Rows(0), IC3810401StallKeyRow).REZID
                    End If
                End Using
                '2012/02/24 KN 瀧 【SERVICE_1】ストール予約テーブルの更新方法を変更 END
                ''予約IDの入力チェック
                If rowIN.IsREZIDNull = False Then
                    '2012/02/24 KN 瀧 【SERVICE_1】ストール予約テーブルの更新方法を変更 START
                    'Using dtStall As IC3810401StallKeyDataTable = da.GetStallKey(rowIN)
                    '    If dtStall.Rows.Count = 0 Then
                    '        ''該当データが存在しない場合
                    '        ''ログの出力
                    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    '            , "{0}.{1} OUT:RETURNCODE = {2}" _
                    '            , Me.GetType.ToString _
                    '            , MethodBase.GetCurrentMethod.Name _
                    '            , ResultNoMatch))
                    '        Return ResultNoMatch
                    '    End If
                    'End Using
                    '2012/02/24 KN 瀧 【SERVICE_1】ストール予約テーブルの更新方法を変更 END

                    ''修正更新処理
                    Dim ret As Long = da.UpdateStallOrder(rowIN)
                    ''2012/01/06 追加 ストール予約履歴テーブルのレコード追加処理の追加
                    ''ストール予約履歴登録処理
                    If ret > 0 Then
                        da.InsertStallHis(rowIN, False)
                    End If
                Else
                    ''予約IDが入力されていない場合、新規登録

                    '2012/02/27 KN 瀧 【SERVICE_1】ストール予約テーブルの登録項目を追加 START
                    ''自社客個人情報の取得
                    Dim dtCI As IC3810401CustomerInfoDataTable = da.GetCustomerInfo(rowIN)
                    Dim rowCI As IC3810401CustomerInfoRow
                    If dtCI.Rows.Count > 0 Then
                        rowCI = DirectCast(dtCI.Rows(0), IC3810401CustomerInfoRow)
                    Else
                        rowCI = dtCI.NewIC3810401CustomerInfoRow
                    End If
                    ''自社客車両情報の取得
                    Dim dtVI As IC3810401VehicleInfoDataTable = da.GetVehicleInfo(rowIN)
                    Dim rowVI As IC3810401VehicleInfoRow
                    If dtVI.Rows.Count > 0 Then
                        rowVI = DirectCast(dtVI.Rows(0), IC3810401VehicleInfoRow)
                    Else
                        rowVI = dtVI.NewIC3810401VehicleInfoRow
                    End If
                    ''サービス情報の取得
                    Dim dtSI As IC3810401ServiceInfoDataTable = da.GetServiceInfo(rowIN, rowVI)
                    Dim rowSI As IC3810401ServiceInfoRow
                    If dtSI.Rows.Count > 0 Then
                        rowSI = DirectCast(dtSI.Rows(0), IC3810401ServiceInfoRow)
                    Else
                        rowSI = dtSI.NewIC3810401ServiceInfoRow
                    End If
                    '2012/02/27 KN 瀧 【SERVICE_1】ストール予約テーブルの登録項目を追加 END

                    ''新規登録処理
                    rowIN.REZID = da.InsertStallOrder(rowIN, rowCI, rowVI, rowSI)
                    ''2012/01/06 追加 ストール予約履歴テーブルのレコード追加処理の追加
                    ''ストール予約履歴登録処理
                    da.InsertStallHis(rowIN, True)
                End If
                ''サービス予約情報の修正更新処理
                da.UpdateVisitOrder(rowIN)
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
    ''' DataRow内の項目を列挙(ログ出力用)
    ''' </summary>
    ''' <param name="args">ログ項目のコレクション</param>
    ''' <param name="row">対象となるDataRow</param>
    ''' <remarks></remarks>
    Private Sub AddLogData(ByVal args As List(Of String), ByVal row As DataRow)
        For Each column As DataColumn In row.Table.Columns
            If row.IsNull(column.ColumnName) = True Then
                args.Add(String.Format(CultureInfo.CurrentCulture, "{0} = NULL", column.ColumnName))
            Else
                args.Add(String.Format(CultureInfo.CurrentCulture, "{0} = {1}", column.ColumnName, row(column.ColumnName)))
            End If
        Next
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
