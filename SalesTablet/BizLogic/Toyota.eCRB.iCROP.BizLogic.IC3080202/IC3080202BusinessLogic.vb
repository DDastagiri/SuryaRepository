'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'IC3080202BusinessLogic.vb
'─────────────────────────────────────
'機能： 顧客詳細(車両ロゴ)
'補足： 
'作成： 2012/01/27 KN 佐藤（真）
'更新： 
'─────────────────────────────────────

Imports System.Text
Imports System.Reflection
Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.iCROP.DataAccess.IC3080202

''' <summary>
''' IC3080202()
''' Webページで使用するビジネスロジック
''' </summary>
''' <remarks></remarks>
Public Class IC3080202BusinessLogic
    Inherits BaseBusinessComponent

#Region " 定数 "

    ''' <summary>
    ''' 成功
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ConstReturnSuccess As Long = 0

    ''' <summary>
    ''' エラー:該当データなし
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ConstReturnNoMatch As Long = 902

#End Region

    ''' <summary>
    ''' 自社客車両取得（モデルロゴ）
    ''' </summary>
    ''' <param name="inModelLogoDataTable">データセット (インプット)</param>
    ''' <returns>データセット (アウトプット)</returns>
    ''' <remarks>自社客車両のモデルロゴを取得する処理</remarks>
    ''' 
    ''' <History>
    ''' </History>
    Public Function GetModelLogoData(ByVal inModelLogoDataTable As IC3080202DataSet.IC3080202ParameterDataTable) As IC3080202DataSet.IC3080202ModelLogoDataTable

        '引数を編集
        Dim args As New List(Of String)
        For Each dr As IC3080202DataSet.IC3080202ParameterRow In inModelLogoDataTable.Rows
            For Each column As DataColumn In inModelLogoDataTable.Columns
                If dr.IsNull(column.ColumnName) = True Then
                    args.Add(String.Format(CultureInfo.InvariantCulture, "{0} = NULL", column.ColumnName))
                Else
                    args.Add(String.Format(CultureInfo.InvariantCulture, "{0} = {1}", column.ColumnName, dr(column.ColumnName)))
                End If
            Next
        Next
        '開始ログを出力
        OutPutStartLog(MethodBase.GetCurrentMethod.Name, args)

        '検索条件を取得
        Dim seriesCd As String = Nothing
        Using da As New IC3080202DataSetTableAdapters.IC3080202DataTableTableAdapter
            '検索条件（シリーズコード）取得
            seriesCd = da.GetSeriesCD(inModelLogoDataTable(0).VIN, inModelLogoDataTable(0).VCLREGNO)
        End Using

        '検索条件の取得判定
        If String.IsNullOrEmpty(seriesCd) Then
            '該当データなしを出力
            OutPutEndLog(MethodBase.GetCurrentMethod.Name, ConstReturnNoMatch)

            Return New IC3080202DataSet.IC3080202ModelLogoDataTable
        End If

        '自社客車両情報を取得
        Using da As New IC3080202DataSetTableAdapters.IC3080202DataTableTableAdapter
            '自社客車両情報（モデルロゴ）取得
            Using outModelLogoDataTable As IC3080202DataSet.IC3080202ModelLogoDataTable = da.GetModelLogo(inModelLogoDataTable(0).DLRCD, _
                                                                                                        seriesCd, _
                                                                                                        inModelLogoDataTable(0).SELECTIONDVS)
                '自社客車両情報の取得判定
                If outModelLogoDataTable.Count = 0 Then
                    '該当データなしを出力
                    OutPutEndLog(MethodBase.GetCurrentMethod.Name, ConstReturnNoMatch)

                    Return outModelLogoDataTable
                End If

                '終了ログを出力
                OutPutEndLog(MethodBase.GetCurrentMethod.Name, ConstReturnSuccess)

                Return outModelLogoDataTable
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

End Class
