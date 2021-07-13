'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'MC3070401BusinessLogic.vb
'─────────────────────────────────────
'機能： オススメ情報集計バッチ ビジネスロジック
'補足： 
'作成： 2012/02/29 TCS 鈴木(健)
'更新： 2013/06/30 TCS 武田 2013/10対応版　既存流用
'─────────────────────────────────────

Imports System.Globalization
Imports System.Reflection
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Batch
Imports Toyota.eCRB.Estimate.Recommended.DataAccess

''' <summary>
''' オススメ情報集計バッチ
''' ビジネスロジック層クラス
''' </summary>
''' <remarks></remarks>
Public Class MC3070401BusinessLogic
    Inherits BaseBusinessComponent

#Region "定数"
    ''' <summary>
    ''' バッチ終了コード：正常終了
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ResultCodeSuccess As Integer = 0

    ''' <summary>
    ''' 設定ファイルから集計日数を取得するためのKEY値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const KeySummaryDateCount As String = "SummaryDateCount"

    ''' <summary>
    ''' 集計日数の初期値
    ''' </summary>
    ''' <remarks>設定ファイルから値を取得できなかった場合のみ使用</remarks>
    Private Const DefValSummaryDateCount As String = "180"
#End Region

#Region "メンバ変数"
    ''' <summary>
    ''' ログ出力用文言：Displayno（902）
    ''' </summary>
    ''' <remarks></remarks>
    Private OutputWord902 As String = BatchWordUtility.GetWord(902)

    ''' <summary>
    ''' ログ出力用文言：Displayno（903）
    ''' </summary>
    ''' <remarks></remarks>
    Private OutputWord903 As String = BatchWordUtility.GetWord(903)

    ''' <summary>
    ''' ログ出力用文言：Displayno（904）
    ''' </summary>
    ''' <remarks></remarks>
    Private OutputWord904 As String = BatchWordUtility.GetWord(904)

    ''' <summary>
    ''' ログ出力用文言：Displayno（905）
    ''' </summary>
    ''' <remarks></remarks>
    Private OutputWord905 As String = BatchWordUtility.GetWord(905)
#End Region

#Region "Publicメソッド"
    ''' <summary>
    ''' 購入率集計情報を登録します。
    ''' </summary>
    ''' <returns>
    ''' 処理結果（0：正常終了 / -1：異常終了（DBアクセスエラー）
    ''' </returns>
    ''' <remarks></remarks>
    Public Function SetRecommendedSummary() As Integer

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  " {0}_Start",
                                  MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        ' バッチ終了コード
        Dim returnCode As Integer = ResultCodeSuccess

        ' 集計日数の取得
        Dim summaryDateCount As Double = Me.GetSummaryDateCount()
        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, Me.OutputWord902, summaryDateCount))
        ' ======================== ログ出力 終了 ========================

        ' バッチ実行日の取得
        Dim today As Date = DateTimeFunc.Now().Date

        ' 集計期間の取得
        Dim startDate As Date = today.AddDays(-summaryDateCount)
        Dim endDate As Date = today
        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  Me.OutputWord903,
                                  startDate.ToString(MC3070401TableAdapter.FormatDatetime, CultureInfo.InvariantCulture),
                                  endDate.ToString(MC3070401TableAdapter.FormatDatetime, CultureInfo.InvariantCulture)))
        ' ======================== ログ出力 終了 ========================

        ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START

        ' 購入率集計情報のロックを取得
        MC3070401TableAdapter.GetPurchaseRateLock()

        ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END

        ' 購入率集計情報のTRUNCATE
        MC3070401TableAdapter.TranRecommendedSummary()
        ' ======================== ログ出力 開始 ========================
        Logger.Info(Me.OutputWord904)
        ' ======================== ログ出力 終了 ========================

        '' 購入率集計情報の登録
        returnCode = Me.RegistRecommendedSummary(startDate, endDate)

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  " {0}_End, Return : [{1}]",
                                  MethodBase.GetCurrentMethod.Name, returnCode))
        ' ======================== ログ出力 終了 ========================

        ' 結果の返却
        Return returnCode
    End Function
#End Region

#Region "Privateメソッド"
    ''' <summary>
    ''' 集計日数を取得します。
    ''' </summary>
    ''' <returns>集計日数</returns>
    ''' <remarks></remarks>
    Private Function GetSummaryDateCount() As Double

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  " {0}_Start",
                                  MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        ' 返却結果
        Dim result As Double

        ' 集計日数の取得
        Dim retDate As String = BatchSetting.GetValue(MC3070401TableAdapter.FunctionId, KeySummaryDateCount, DefValSummaryDateCount)

        ' Double型に変換（数値チェック含む）
        Try
            result = Double.Parse(retDate, CultureInfo.InvariantCulture)
        Catch ex As System.FormatException
            ' ======================== ログ出力 開始 ========================
            Logger.Error(String.Format(CultureInfo.InvariantCulture,
                                      " SummaryDateCount : [{0}] is not Numeric.",
                                      retDate))
            ' ======================== ログ出力 終了 ========================

            Throw
        End Try

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  " {0}_End, Return : [{1}]",
                                  MethodBase.GetCurrentMethod.Name, result))
        ' ======================== ログ出力 終了 ========================

        ' 結果を返却
        Return result
    End Function

    ''' <summary>
    ''' 購入率集計情報を登録します。
    ''' </summary>
    ''' <param name="startDate">集計開始日時</param>
    ''' <param name="endDate">集計終了日時</param>
    ''' <returns>
    ''' 処理結果（0：正常終了 / -1：異常終了（DBアクセスエラー））
    ''' </returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Private Function RegistRecommendedSummary(startDate As Date, endDate As Date) As Integer

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  " {0}_Start",
                                  MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        ' 処理結果
        Dim returnCode As Integer = ResultCodeSuccess

        ' 購入率集計情報の挿入
        Dim retCount As Integer = MC3070401TableAdapter.InsRecommendedSummary(startDate, endDate)
        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, Me.OutputWord905, retCount))
        ' ======================== ログ出力 終了 ========================

        ' バッチ終了コードの設定
        If retCount < 0 Then
            returnCode = retCount
        End If

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  " {0}_End, Return : [{1}]",
                                  MethodBase.GetCurrentMethod.Name, returnCode))
        ' ======================== ログ出力 終了 ========================

        ' 結果を返却
        Return returnCode
    End Function
#End Region

End Class
