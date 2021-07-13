'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'MC3070401.vb
'─────────────────────────────────────
'機能： オススメ情報集計バッチ
'補足： 
'作成： 2012/03/01 TCS 鈴木(健)
'更新： 
'─────────────────────────────────────

Imports System.Globalization
Imports System.Reflection
Imports Toyota.eCRB.SystemFrameworks.Batch
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.Estimate.Recommended.BizLogic
Imports Toyota.eCRB.Estimate.Recommended.DataAccess

''' <summary>
''' オススメ情報集計バッチ
''' プレゼンテーション層クラス
''' </summary>
''' <remarks></remarks>
Public Class MC3070401
    Implements IBatch

#Region "定数"
    ''' <summary>
    ''' バッチ終了コード：正常終了
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ResultCodeSuccess As Integer = 0
#End Region

#Region "Publicメソッド"
    ''' <summary>
    ''' オススメ情報集計バッチプログラム起動時に最初に呼び出される処理です。
    ''' </summary>
    ''' <param name="args">コマンドライン引数</param>
    ''' <returns>
    ''' バッチプログラムの返却値
    '''   0：正常終了
    '''  99：異常終了（設定ファイル不正）
    ''' </returns>
    ''' <remarks></remarks>
    Public Function Execute(args() As String) As Integer Implements SystemFrameworks.Batch.IBatch.Execute

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0}_Start",
                                  MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        ' ======================== ログ出力 開始 ========================
        Logger.Info(BatchWordUtility.GetWord(901))
        ' ======================== ログ出力 終了 ========================

        ' ビジネスロジック
        Dim bizLogic As MC3070401BusinessLogic
        ' 終了コード
        Dim returnCode As Integer = ResultCodeSuccess

        Try
            ' ビジネスロジックのインスタンスを生成
            bizLogic = New MC3070401BusinessLogic

            ' 購入率集計情報の登録
            returnCode = bizLogic.SetRecommendedSummary()
        Finally
            bizLogic = Nothing
        End Try

        ' ======================== ログ出力 開始 ========================
        Logger.Info(BatchWordUtility.GetWord(906))
        ' ======================== ログ出力 終了 ========================

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0}_End, Return : [{1}]",
                                  MethodBase.GetCurrentMethod.Name, returnCode))
        ' ======================== ログ出力 終了 ========================

        ' 結果の返却
        Return returnCode
    End Function
#End Region

End Class
