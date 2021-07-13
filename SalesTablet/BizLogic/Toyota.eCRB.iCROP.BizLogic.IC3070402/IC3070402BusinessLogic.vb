'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'IC3070402BusinessLogic.vb
'─────────────────────────────────────
'機能： 顧客属性取得IF ビジネスロジック
'補足： 
'作成： 2012/03/07 TCS 陳
'更新： 2013/06/30 TCS 武田 2013/10対応版　既存流用
'─────────────────────────────────────

Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.Estimate.Recommended.DataAccess
Imports System.Text
Imports System.Globalization
Imports System.Reflection

Public Class IC3070402BusinessLogic
    Inherits BaseBusinessComponent

#Region "定数"

#Region "終了コード"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks>正常終了</remarks>
    Private Const NOMAL As Integer = 0

#End Region

    ''' <summary>
    ''' メソッド名
    ''' </summary>
    ''' <remarks>ログ出力用(メソッド名)</remarks>
    Private Const METHODNAME As String = "GetCustAttribute "

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

#Region "001.顧客属性取得"

    ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 001.顧客属性取得
    ''' </summary>
    ''' <param name="crCustID">活動先顧客コード</param>
    ''' <returns>顧客属性</returns>
    ''' <remarks>顧客コードを条件に、顧客属性を取得する</remarks>
    Public Function GetCustAttribute(ByVal crCustId As String) As IC3070402DataSet
        ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "[{0}]_[{1}]_Start,[crCustId:{2}]",
                                  IC3070402TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name, crCustId),
                                  True)
        ' ======================== ログ出力 終了 ========================

        '結果返却用DataSet作成
        Using retIC3070402DataSet As New IC3070402DataSet

            retIC3070402DataSet.Tables.Clear()

            ' -----------------------------------------------
            ' -- 顧客属性取得処理
            ' -----------------------------------------------

            '取得データ格納用DataTable作成
            '顧客名データー
            Dim retCstNameDataTable As IC3070402DataSet.IC3070402CstNameDataTable = Nothing
            '顧客職業データー
            Dim retCstoccupationDataTable As IC3070402DataSet.IC3070402CstoccupationDataTable = Nothing
            '顧客家族構成データー
            Dim retCstfamilyDataTable As IC3070402DataSet.IC3070402CstfamilyDataTable = Nothing
            '顧客趣味データー
            Dim retCsthobbyDataTable As IC3070402DataSet.IC3070402CsthobbyDataTable = Nothing

            '顧客属性取得
            Dim adapter As New IC3070402TableAdapter()
            Try
                ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
                '顧客名取得
                retCstNameDataTable = adapter.GetCstName(crCustId)
                ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END
                '顧客職業取得
                retCstoccupationDataTable = adapter.GetCstOccupation(crCustId)
                '顧客家族構成取得
                retCstfamilyDataTable = adapter.GetCstFamily(crCustId)
                '顧客趣味取得
                retCsthobbyDataTable = adapter.GetCstHobby(crCustId)

            Finally
                adapter = Nothing
            End Try

            '取得データテーブルをデータセットに格納
            retIC3070402DataSet.Tables.Add(retCstNameDataTable)
            retIC3070402DataSet.Tables.Add(retCstoccupationDataTable)
            retIC3070402DataSet.Tables.Add(retCstfamilyDataTable)
            retIC3070402DataSet.Tables.Add(retCsthobbyDataTable)

            '正常終了
            ResultId = NOMAL

            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "[{0}]_[{1}]_End, {2} IC3070402DataSet:[{3}]",
                                      IC3070402TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name, MethodBase.GetCurrentMethod.Name, retIC3070402DataSet),
                                      True)
            ' ======================== ログ出力 終了 ========================

            Return retIC3070402DataSet

        End Using

    End Function

#End Region

End Class
