'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3190601BusinessLogic.vb
'─────────────────────────────────────
'機能： B/O管理ボード (ビジネス)
'補足： 
'作成： 2014/08/26 TMEJ M.Asano
'更新： 
'─────────────────────────────────────

Imports System.Text
Imports System.Globalization
Imports System.Reflection
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.PartsManagement.BoMonitor.DataAccess

Public Class SC3190601BusinessLogic
    Inherits BaseBusinessComponent

#Region "定数"

    ''' <summary>
    ''' Logフォーマット
    ''' </summary>
    ''' <remarks></remarks>
    Private Const LOG_FORMAT As String = "{0}_{1} {2} {3}"

    ''' <summary>
    ''' Log文言：開始
    ''' </summary>
    ''' <remarks></remarks>
    Private Const LOG_START As String = "Start"

    ''' <summary>
    ''' Log文言：パラメータ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const LOG_PARAMETER As String = "ParamValue"

    ''' <summary>
    ''' Log文言：終了
    ''' </summary>
    ''' <remarks></remarks>
    Private Const LOG_END As String = "End"

#End Region

#Region "公開メソッド"

    ''' <summary>
    ''' 部品情報一覧の取得
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="todayDate">本日日付</param>
    ''' <param name="judgmentDate">直近到着予定部品判定日付</param>
    ''' <returns>BoPartsInfoListDataTable</returns>
    ''' <remarks></remarks>
    Public Function GetPartsInfoList(ByVal dealerCode As String, _
                                     ByVal branchCode As String, _
                                     ByVal todayDate As Date, _
                                     ByVal judgmentDate As Date) _
                                     As SC3190601DataSet.BoPartsInfoListDataTable

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , LOG_FORMAT _
                                , MethodBase.GetCurrentMethod.Name _
                                , LOG_START _
                                , LOG_PARAMETER _
                                , CreateLogWrod(dealerCode, branchCode, todayDate, judgmentDate)))

        ' 部品情報一覧取得
        Dim boPartsInfoList As SC3190601DataSet.BoPartsInfoListDataTable = _
            SC3190601TableAdapter.GetPartsInfoList(dealerCode, branchCode, todayDate, judgmentDate)

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , LOG_FORMAT _
                                , MethodBase.GetCurrentMethod.Name _
                                , LOG_END _
                                , LOG_PARAMETER _
                                , CreateLogWrod(boPartsInfoList)))

        Return boPartsInfoList

    End Function

#End Region

#Region "非公開メソッド"

#Region "ログ文字列作成"

    ''' <summary>
    ''' ログ出力文字列作成
    ''' </summary>
    ''' <param name="parameters">ログに出力する値</param>
    ''' <returns>ログ出力文字列</returns>
    ''' <remarks></remarks>
    Private Function CreateLogWrod(ByVal ParamArray parameters As Object()) As String

        Dim logWord As New StringBuilder()
        With logWord
            Dim lastIndex As Integer = parameters.Length - 1

            ' すべての要素
            For i As Integer = 0 To lastIndex

                ' 最初の要素
                If 0 = i Then
                    .Append("[")

                    ' 最初の要素でない場合
                Else
                    .Append(", ")
                End If

                .Append(parameters(i))

                ' データテーブルの場合
                If TypeOf parameters(i) Is DataTable Then
                    .Append("[Count = ")
                    .Append(DirectCast(parameters(i), DataTable).Rows.Count)
                    .Append("]")
                End If

                ' 最後の要素の場合
                If i = lastIndex Then
                    .Append("]")
                End If
            Next

        End With

        Return logWord.ToString()

    End Function

#End Region

#End Region

End Class
