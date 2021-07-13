'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'IC3070401BusinessLogic.vb
'─────────────────────────────────────
'機能： オススメ情報取得IF ビジネスロジック
'補足： 
'作成： 2012/03/01 TCS 陳
'更新： 
'─────────────────────────────────────

Imports System.Text
Imports System.Reflection
Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.Estimate.Recommended.DataAccess


Public Class IC3070401BusinessLogic
    Inherits BaseBusinessComponent

#Region "定数"

#Region "終了コード"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks>正常終了</remarks>
    Private Const NOMAL As Integer = 0
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks>販売店コードが未設定</remarks>
    Private Const ERR_DlrCdIsNull As Integer = 2077
    ''' <remarks>シリーズコードが未設定</remarks>
    Private Const ERR_SeriesCdIsNull As Integer = 2078
    ''' <remarks>モデルコードが未設定</remarks>
    Private Const ERR_ModelCdIsNull As Integer = 2079

#End Region

    ''' <summary>
    ''' メソッド名
    ''' </summary>
    ''' <remarks>ログ出力用(メソッド名)</remarks>
    Private Const METHODNAME As String = "GetRecommended "

    ''' <summary>
    ''' 開始ログ    
    ''' </summary>
    ''' <remarks>ログ出力用(開始)</remarks>
    Private Const STARTLOG As String = "START "

    ''' <summary>
    ''' 終了ログ
    ''' </summary>
    ''' <remarks>ログ出力用(終了)</remarks>
    Private Const ENDLOG As String = "END "

#End Region

#Region "メンバ変数"
    ''' <summary>
    ''' 終了コード
    ''' </summary>
    ''' <remarks></remarks>
    Private resultId_ As Integer
#End Region

#Region "プロパティ"
    ''' <summary>
    ''' 終了コード
    ''' </summary>
    ''' <value>終了コード</value>
    ''' <returns>終了コード</returns>
    ''' <remarks>0の場合は正常、それ以外の場合エラー</remarks>
    Public Property ResultId As Integer
        Get
            Return resultId_
        End Get
        Set(value As Integer)
            resultId_ = value
        End Set
    End Property

#End Region

#Region "コンストラクタ"
    ''' <summary>
    ''' 初期化処理
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        resultId_ = 0
    End Sub
#End Region

#Region "001.オススメ情報取得"

    ''' <summary>
    ''' 001.オススメ情報取得
    ''' </summary>
    ''' <param name="dlrCd">販売店コード</param>
    ''' <param name="seriesCd">シリーズコード</param>
    ''' <param name="modelCd">モデルコード</param>
    ''' <returns>オススメ情報</returns>
    ''' <remarks>対象の車両/グレードのオススメオプション情報を取得する。</remarks>
    Public Function GetRecommended(ByVal dlrCD As String,
                                   ByVal seriesCD As String,
                                   ByVal modelCD As String) As IC3070401DataSet

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "[{0}]_[{1}]_Start,[DLRCD:{2},SERIESCD:{3},MODELCD:{4}]",
                                  IC3070401TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name, dlrCD, seriesCD, modelCD),
                                  True)
        ' ======================== ログ出力 終了 ========================

        '結果返却用DataSet作成
        Using retIC3070401DataSet As New IC3070401DataSet

            retIC3070401DataSet.Tables.Clear()

            ' -----------------------------------------------
            ' -- オススメ情報取得処理
            ' -----------------------------------------------

            '取得データ格納用DataTable作成
            Dim retPURCHASERATEDataTbl As IC3070401DataSet.IC3070401PurchaserateDataTable = Nothing
            'オススメ情報取得処理
            Dim adapter As New IC3070401TableAdapter()
            Try
                'オススメ情報取得
                retPURCHASERATEDataTbl = adapter.GetRecommendedInfo(dlrCD,
                                                                    seriesCD,
                                                                    modelCD)
            Finally
                adapter = Nothing
            End Try

            '取得データテーブルをデータセットに格納
            retIC3070401DataSet.Tables.Add(retPURCHASERATEDataTbl)

            '正常終了
            ResultId = NOMAL

            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "[{0}]_[{1}]_End, {2} RowCount:[{3}]",
                                      IC3070401TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name, MethodBase.GetCurrentMethod.Name, retPURCHASERATEDataTbl.Rows.Count),
                                      True)
            ' ======================== ログ出力 終了 ========================

            Return retIC3070401DataSet

        End Using

    End Function

#End Region

End Class
