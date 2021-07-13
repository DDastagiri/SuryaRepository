Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.Estimate.Quotation.DataAccess
Imports System.Text

Public Class IC3070201BusinessLogic
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
    ''' <remarks>見積管理IDが未設定</remarks>
    Private Const ERR_EstimateIdIsNull As Integer = 2021
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks>見積管理IDが数値以外</remarks>
    Private Const ERR_EstimateIdIsNotNumeric As Integer = 3021
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks>見積管理IDが10桁以上</remarks>
    Private Const ERR_EstimateIdSizeOver As Integer = 4021
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks>実行モードが未設定</remarks>
    Private Const ERR_ModeIsNull As Integer = 2011
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks>実行モードが不正値</remarks>
    Private Const ERR_ModeIsNotCorrect As Integer = 5011
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks>対象見積情報無し</remarks>
    Private Const ERR_EstInfoNothing As Integer = 6001
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks>システムエラー</remarks>
    Private Const ERR_SysErr As Integer = 9999

#End Region

    ''' <summary>
    ''' メソッド名
    ''' </summary>
    ''' <remarks>ログ出力用(メソッド名)</remarks>
    Private Const METHODNAME As String = "GetEstimationInfo "

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

#Region "001.見積情報取得"

    ''' <summary>
    ''' 001.見積情報取得
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <param name="mode">実行モード 0:全情報取得、1:車両情報のみ取得</param>
    ''' <returns>IC3070201DataSet</returns>
    ''' <remarks>見積管理IDを条件に見積情報の取得を行う</remarks>
    Public Function GetEstimationInfo(ByVal estimateId As Long, _
                                      ByVal mode As Integer) As IC3070201DataSet

        'デバッグログ(開始)
        '開始ログ出力
        Dim startLogInfo As New StringBuilder
        startLogInfo.Append(METHODNAME)
        startLogInfo.Append(STARTLOG)
        Logger.Debug(startLogInfo.ToString())

        '結果返却用DataSet作成
        Using retIC3070201DataSet As New IC3070201DataSet

            retIC3070201DataSet.Tables.Clear()

            ' -----------------------------------------------
            ' -- 入力チェック
            ' -----------------------------------------------

            '見積管理IDチェック
            If (IsNothing(estimateId)) Then
                '見積管理IDが未設定
                ResultId = ERR_EstimateIdIsNull

                'ログ出力
                Logger.Error("ResultId : " & CType(ResultId, String))

                Return retIC3070201DataSet

            ElseIf Not Validation.IsHankakuNumber(CType(estimateId, String)) Then
                '見積管理IDが半角数値以外
                ResultId = ERR_EstimateIdIsNotNumeric

                'ログ出力
                Logger.Error("ResultId : " & CType(ResultId, String))

                Return retIC3070201DataSet

            ElseIf CType(estimateId, String).Length > 10 Then
                '見積管理IDが10桁以上
                ResultId = ERR_EstimateIdSizeOver

                'ログ出力
                Logger.Error("ResultId : " & CType(ResultId, String))

                Return retIC3070201DataSet

            End If

            '実行モードチェック
            If (IsNothing(mode)) Then
                '実行モードが未設定
                ResultId = ERR_ModeIsNull

                'ログ出力
                Logger.Error("ResultId : " & CType(ResultId, String))

                Return retIC3070201DataSet

            ElseIf Not ((mode.Equals(0) Or mode.Equals(1))) Then
                '実行モードが不正値
                ResultId = ERR_ModeIsNotCorrect

                'ログ出力
                Logger.Error("ResultId : " & CType(ResultId, String))

                Return retIC3070201DataSet

            End If


            ' -----------------------------------------------
            ' -- 見積情報取得処理
            ' -----------------------------------------------

            '取得データ格納用DataTable作成
            Dim retESTIMATIONINFODataTbl As IC3070201DataSet.IC3070201EstimationInfoDataTable = Nothing
            Dim retEST_VCLOPTIONINFODataTbl As IC3070201DataSet.IC3070201VclOptionInfoDataTable = Nothing
            Dim retEST_CUSTOMERINFODataTbl As IC3070201DataSet.IC3070201CustomerInfoDataTable = Nothing
            Dim retEST_CHARGEINFODataTbl As IC3070201DataSet.IC3070201ChargeInfoDataTable = Nothing
            Dim retEST_PAYMENTINFODataTbl As IC3070201DataSet.IC3070201PaymentInfoDataTable = Nothing
            Dim retEST_TRADEINCARINFODataTbl As IC3070201DataSet.IC3070201TradeincarInfoDataTable = Nothing
            Dim retEST_INSURANCEINFODataTbl As IC3070201DataSet.IC3070201EstInsuranceInfoDataTable = Nothing


            ' 見積情報登録処理
            Dim adapter As New IC3070201TableAdapter(mode)

            Try
                '見積情報取得
                retESTIMATIONINFODataTbl = adapter.GetEstimationInfoDataTable(estimateId)

                '見積車両オプション情報取得
                retEST_VCLOPTIONINFODataTbl = adapter.GetVclOptionInfoDataTable(estimateId)

                '実行モードが0の場合、見積顧客/見積諸費用/見積支払方法/見積下取車両の情報も取得する
                If (mode.Equals(0)) Then

                    '見積顧客情報取得
                    retEST_CUSTOMERINFODataTbl = adapter.GetCustomerInfoDataTable(estimateId)

                    '見積諸費用情報取得
                    retEST_CHARGEINFODataTbl = adapter.GetChargeInfoDataTable(estimateId)

                    '見積支払方法情報取得
                    retEST_PAYMENTINFODataTbl = adapter.GetPaymentInfoDataTable(estimateId)

                    '見積下取車両情報取得
                    retEST_TRADEINCARINFODataTbl = adapter.GetTradeincarInfoDataTable(estimateId)

                    '見積保険情報取得
                    retEST_INSURANCEINFODataTbl = adapter.GetInsuranceInfoDataTable(estimateId)
                End If

            Catch oex As OracleExceptionEx
                ResultId = ERR_SysErr
                Logger.Error("ResultId : " & CType(ERR_SysErr, String), oex)

                Throw
            Finally
                adapter = Nothing
            End Try

            '取得データテーブルをデータセットに格納
            retIC3070201DataSet.Tables.Add(retESTIMATIONINFODataTbl)
            retIC3070201DataSet.Tables.Add(retEST_VCLOPTIONINFODataTbl)

            '実行モードが0の場合
            If (mode.Equals(0)) Then
                retIC3070201DataSet.Tables.Add(retEST_CUSTOMERINFODataTbl)
                retIC3070201DataSet.Tables.Add(retEST_CHARGEINFODataTbl)
                retIC3070201DataSet.Tables.Add(retEST_PAYMENTINFODataTbl)
                retIC3070201DataSet.Tables.Add(retEST_TRADEINCARINFODataTbl)
                retIC3070201DataSet.Tables.Add(retEST_INSURANCEINFODataTbl)
            End If

            '見積情報の取得件数を確認
            If retESTIMATIONINFODataTbl.Rows.Count.Equals(1) Then
                '正常終了
                ResultId = NOMAL
            Else
                '対象データが無し
                ResultId = ERR_EstInfoNothing
                Logger.Error("ResultId : " & CType(ResultId, String))
            End If


            'デバッグログ(終了)
            '終了ログ出力
            Dim endLogInfo As New StringBuilder
            endLogInfo.Append(METHODNAME)
            endLogInfo.Append(ENDLOG)
            Logger.Debug(endLogInfo.ToString())

            Return retIC3070201DataSet

        End Using

    End Function

#End Region

End Class
