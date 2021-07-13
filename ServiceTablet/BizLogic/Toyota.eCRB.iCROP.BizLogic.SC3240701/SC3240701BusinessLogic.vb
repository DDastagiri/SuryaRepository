'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3240701BusinessLogic.vb
'─────────────────────────────────────
'機能： ストール使用不可設定（ビジネスロジック）
'補足： 
'作成： 2017/08/30 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加
'更新： 
'─────────────────────────────────────
Option Explicit On
Option Strict On

Imports System.Text
Imports System.Reflection
Imports System.Globalization
Imports System.Web.Script.Serialization
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.BizLogic.TabletSMBCommonClassBusinessLogic
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.BizLogic
Imports Toyota.eCRB.iCROP.DataAccess.SC3240701

Public Class SC3240701BusinessLogic
    Inherits BaseBusinessComponent
    Implements IDisposable

#Region "定数"
    ''' <summary>
    ''' 自画面のプログラムID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const UNAVAILABLE_PROGRAMID As String = "SC3240701"

    ''' <summary>
    ''' 使用不可チップ メモ欄
    ''' デフォルト値　
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_UNAVAILABLE_MEMO_DEFAULT = " "
#End Region


#Region "列挙体"

    ''' <summary>
    ''' エラーメッセージ一覧
    ''' </summary>
    ''' <remarks></remarks>
    Private Enum ResultCode As Integer

        ''' <summary>
        ''' 成功
        ''' </summary>
        ''' <remarks></remarks>
        Success = 0

        ''' <summary>
        ''' DBタイムアウトエラー
        ''' </summary>
        ''' <remarks></remarks>
        DBTimeOut = 901

        ''' <summary>
        ''' 予期せぬエラー
        ''' </summary>
        ''' <remarks></remarks>
        Exception = 902

        ''' <summary>
        ''' 他チップとの重複エラー
        ''' </summary>
        ''' <remarks></remarks>
        CollisionError = 904

        ''' <summary>
        ''' 行ロックバージョンエラー
        ''' </summary>
        ''' <remarks></remarks>
        RowLockVersionError = 905

        ''' <summary>
        ''' データ存在チェックエラー
        ''' </summary>
        ''' <remarks></remarks>
        OtherDeleteError = 906

    End Enum
#End Region


#Region "パブリックメソッド"
    ''' <summary>
    ''' ストール使用不可チップ作成
    ''' </summary>
    ''' <param name="stallId">ストールID</param>
    ''' <param name="startIdleDateTime">開始日時</param>
    ''' <param name="endIdleDateTime">終了日時</param>
    ''' <param name="idleMemo">メモ</param>
    ''' <param name="nowDate">現在日時</param>
    ''' <param name="objStaffContext">スタッフ情報</param>
    ''' <param name="stallIdleId">ストール非稼働ID</param>
    ''' <returns>結果コード</returns>
    ''' <remarks></remarks>
    ''' 
    <EnableCommit()>
    Public Function CreateUnavailableChip(ByVal stallId As Decimal, _
                                   ByVal startIdleDateTime As Date, _
                                   ByVal endIdleDateTime As Date, _
                                   ByVal idleMemo As String, _
                                   ByVal nowDate As Date, _
                                   ByVal objStaffContext As StaffContext, _
                                   ByRef stallIdleId As Decimal) As Integer

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.{1} START IN:stallId={2}, idleStartDatetime={3}, idleEndDatetime={4}, idleMemo={5}, nowDate={6}, objStaffContext={7} ", _
                                  Me.GetType.ToString, _
                                  MethodBase.GetCurrentMethod.Name, _
                                  stallId, _
                                  startIdleDateTime, _
                                  endIdleDateTime, _
                                  idleMemo, _
                                  nowDate, _
                                  objStaffContext))

        '結果コード
        Dim result As Integer

        Try
            Using tabletSMBCommons As New TabletSMBCommonClassBusinessLogic
                '使用不可チップ作成
                result = tabletSMBCommons.CreateStallUnavailable(stallId, startIdleDateTime, endIdleDateTime, idleMemo, nowDate, objStaffContext, 0, UNAVAILABLE_PROGRAMID, stallIdleId)
            End Using

        Catch ex As OracleExceptionEx When ex.Number = 1013
            'DBタイムアウト
            Logger.Error(ex.ToString, ex)
            Me.Rollback = True
            Return ResultCode.DBTimeOut

        Catch exception As Exception
            '予期せぬエラー
            Logger.Error(exception.ToString, exception)
            Me.Rollback = True
            Return ResultCode.Exception
        End Try

        If result = ActionResult.OverlapError Then
            '他チップとの重複エラー
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Error:CollisionError.", System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return ResultCode.CollisionError
        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", System.Reflection.MethodBase.GetCurrentMethod.Name))
        Return ResultCode.Success
    End Function

    ''' <summary>
    ''' ストール使用不可チップ更新
    ''' </summary>
    ''' <param name="stallIdleId">ストール非稼働ID</param>
    ''' <param name="stallId">ストールID</param>
    ''' <param name="startIdleDateTime">開始日時</param>
    ''' <param name="endIdleDateTime">開始日時</param>
    ''' <param name="idleMemo">メモ</param>
    ''' <param name="nowDate">現在日時、更新日時</param>
    ''' <param name="objStaffContext">スタッフ情報</param>
    ''' <param name="rowLockVersion">行ロックバージョン</param>
    ''' <returns>結果コード</returns>
    ''' <remarks></remarks>
    ''' 
    <EnableCommit()>
    Public Function UpdateUnavailableChip(ByVal stallIdleId As Decimal, _
                                   ByVal stallId As Decimal, _
                                   ByVal startIdleDateTime As Date, _
                                   ByVal endIdleDateTime As Date, _
                                   ByVal idleMemo As String, _
                                   ByVal nowDate As Date, _
                                   ByVal objStaffContext As StaffContext, _
                                   ByVal rowLockVersion As Long) As Integer
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.{1} START IN:stallIdleId={2}, stallId={3}, idleStartDatetime={4}, idleEndDatetime={5}, idleMemo={6}, nowDate={7}, objStaffContext={8}, ,rowLockVersion={9} ", _
                                  Me.GetType.ToString, _
                                  MethodBase.GetCurrentMethod.Name, _
                                  stallIdleId, _
                                  stallId, _
                                  startIdleDateTime, _
                                  endIdleDateTime, _
                                  idleMemo, _
                                  nowDate, _
                                  objStaffContext, _
                                  rowLockVersion))

        Dim result As Long = CLng(0)
        Dim convertIdleMemo = idleMemo
        Try
            Using tabletSMBCommons As New TabletSMBCommonClassBusinessLogic


                'メモ欄が空の場合、空白で更新するための準備
                If String.IsNullOrEmpty(idleMemo) Then

                    convertIdleMemo = C_UNAVAILABLE_MEMO_DEFAULT
                End If

                '使用不可チップ更新
                result = tabletSMBCommons.UpdateStallUnavailable(stallIdleId, stallId, startIdleDateTime, endIdleDateTime, convertIdleMemo, nowDate, nowDate, objStaffContext, UNAVAILABLE_PROGRAMID, rowLockVersion)
            End Using

        Catch ex As OracleExceptionEx When ex.Number = 1013
            'DBタイムアウト
            Logger.Error(ex.ToString, ex)
            Me.Rollback = True
            Return ResultCode.DBTimeOut

        Catch exception As Exception
            '予期せぬエラー
            Logger.Error(exception.ToString, exception)
            Me.Rollback = True
            Return ResultCode.Exception
        End Try

        If result = ActionResult.OverlapUnavailableError Then
            '他チップとの重複エラー
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Error:CollisionError.", System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return ResultCode.CollisionError
        End If

        If result = ActionResult.RowLockVersionError Then
            '排他チェックエラー
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Error:RowLockVersionError.", System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return ResultCode.RowLockVersionError
        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
        Return ResultCode.Success
    End Function

    ''' <summary>
    '''登録済み使用不可チップの情報取得
    ''' </summary>
    ''' <param name="stallIdleId">ストール非稼働ID</param>
    ''' <remarks></remarks>
    Public Function GetInitInfo(ByVal stallIdleId As Decimal) As SC3240701DataSet.StallIdleInfoDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. stallIdleId={1}" _
                , MethodBase.GetCurrentMethod.Name, stallIdleId))

        Dim ta As New SC3240701DataSetTableAdapters.SC3240701DataAdapter
        'ストール使用不可情報取得
        Dim unavailableTable As SC3240701DataSet.StallIdleInfoDataTable = ta.GetStallUnavailableChipInfo(stallIdleId)

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))

        '結果が存在しない場合
        If unavailableTable.Rows.Count = 0 Then

            'Nothing返却
            Return Nothing
        End If

        'ストール使用不可情報返却
        Return unavailableTable
    End Function
#End Region


#Region "変換処理"
    ''' <summary>
    '''   DataTableをJSON文字列に変換する
    ''' </summary>
    ''' <param name="dataTable">変換対象 DataSet</param>
    ''' <returns>JSON文字列</returns>
    Public Function DataTableToJson(ByVal dataTable As DataTable) As String
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S.", System.Reflection.MethodBase.GetCurrentMethod.Name))
        Dim resultMain As New Dictionary(Of String, Object)
        Dim JSerializer As New JavaScriptSerializer

        'ディレクショナリ型をJSON形式に変換
        If dataTable Is Nothing Then
            Return JSerializer.Serialize(resultMain)
        End If

        'データテーブルの行分ループ
        For Each dr As DataRow In dataTable.Rows
            Dim result As New Dictionary(Of String, Object)

            'データテーブルの列分ループ
            For Each dc As DataColumn In dataTable.Columns
                'データテーブルの列名と文字列に変換した値を取得
                result.Add(dc.ColumnName, dr(dc).ToString)
            Next

            'ディレクショナリ型のキーの要素に取得した値を設定
            resultMain.Add("Key" + CType(resultMain.Count + 1, String), result)
        Next
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", System.Reflection.MethodBase.GetCurrentMethod.Name))
        Return JSerializer.Serialize(resultMain)
    End Function


#End Region

#Region "IDisposable Support"
    Private disposedValue As Boolean ' 重複する呼び出しを検出するには

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
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
