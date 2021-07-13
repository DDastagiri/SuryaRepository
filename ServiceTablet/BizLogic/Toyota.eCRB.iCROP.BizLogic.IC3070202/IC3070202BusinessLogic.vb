Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.Estimate.Quotation.DataAccess

''' <summary>
''' 見積情報登録I/F
''' ビジネスロジック層クラス
''' </summary>
''' <remarks></remarks>
Public Class IC3070202BusinessLogic
    Inherits BaseBusinessComponent

#Region "定数"
    ''' <summary>
    ''' エラーコード：処理正常終了(該当データ有）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ErrCodeSuccess As Short = 0
    ''' <summary>
    ''' エラーコード：システムエラー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ErrCodeSys As Short = 9999

    ''' <summary>
    ''' 見積情報.契約書印刷フラグ（常に0で登録）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONTPRINTFLG_VALUE As String = "0"
#End Region

#Region "メンバ変数"
    ''' <summary>
    ''' 終了コード
    ''' </summary>
    ''' <remarks></remarks>
    Private prpResultId As Short = 0

    ''' <summary>
    ''' 作成日
    ''' </summary>
    ''' <remarks></remarks>
    Private prpCreateDate As Date = DateTime.MinValue
#End Region

#Region "プロパティ"
    ''' <summary>
    ''' 終了コード
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>0の場合は正常、それ以外の場合エラー</remarks>
    Public ReadOnly Property ResultId() As Short
        Get
            Return prpResultId
        End Get
    End Property
#End Region

#Region "Publicメソッド"
    ''' <summary>
    ''' 見積情報を登録する。
    ''' </summary>
    ''' <param name="estInfoDataSet">見積情報データセット</param>
    ''' <param name="updateDvs">更新区分（0：登録/1：更新/2：削除）</param>
    ''' <param name="mode">実行モード（0：全ての見積関連情報を更新/1：見積情報、見積車両情報、見積車両オプション情報のみ更新）</param>
    ''' <param name="vclOptionUpdateDvs">車両オプション更新区分（0：全ての車両オプションを更新/1：メーカーの車両オプションのみ更新）</param>
    ''' <returns>見積情報登録結果データテーブル</returns>
    ''' <remarks>
    ''' ■見積情報データセット（IC3070202DataSet）
    ''' 　|----見積情報データテーブル（IC3070202EstimationInfo）
    ''' 　|----見積車両オプション情報データテーブル（IC3070202EstVclOptionInfo）
    ''' 　|----見積顧客情報データテーブル（IC3070202EstCustomerInfo）
    ''' 　|----見積諸費用情報データテーブル（IC3070202EstChargeInfo）
    ''' 　|----見積支払方法情報データテーブル（IC3070202EstPaymentInfo）
    ''' 　|----見積下取車両情報データテーブル（IC3070202EstTradeInCarInfo）
    ''' 　|----見積保険情報データテーブル（IC3070202EstInsuranceInfo）
    ''' 　|----見積情報登録結果データテーブル（IC3070202EstResult）
    ''' </remarks>
    <EnableCommit()>
    Public Function SetEstimationInfo(ByVal estInfoDataSet As IC3070202DataSet, _
                                      ByVal updateDvs As Integer, _
                                      ByVal mode As Integer, _
                                      ByVal vclOptionUpdateDvs As Integer) As IC3070202DataSet.IC3070202EstResultDataTable

        ' 引数チェックはプレゼンテーション層で実施する
        If estInfoDataSet Is Nothing Then
            Throw New ArgumentException("Exception Occured", "estInfoDataSet")
        End If

        ' 見積情報登録処理
        Dim adapter As New IC3070202TableAdapter(CShort(updateDvs))

        Try
            ' 見積情報登録結果データテーブル
            Dim estResultDT As IC3070202DataSet.IC3070202EstResultDataTable _
                = estInfoDataSet.IC3070202EstResult

            ' 見積情報登録結果データテーブル行
            Dim estResultRow As IC3070202DataSet.IC3070202EstResultRow _
                = estResultDT.NewIC3070202EstResultRow()

            ' 登録処理結果
            estResultRow.IsSuccess = False
            ' 見積管理ID
            Dim estimateId As Long = 0

            ' 更新区分により、処理分岐
            Select Case updateDvs
                Case 0
                    ' -----------------------------------------------
                    ' -- 更新区分：0（新規登録）
                    ' -----------------------------------------------

                    ' 見積管理IDシーケンス取得
                    estimateId = adapter.SelEstimateId()

                    ' 見積関連情報登録
                    RegistEstimationInfo(CShort(mode), estimateId, estInfoDataSet, adapter)

                Case 1
                    ' -----------------------------------------------
                    ' -- 更新区分：1（更新）
                    ' -----------------------------------------------

                    ' 見積管理IDの取得
                    estimateId = estInfoDataSet.IC3070202EstimationInfo.Item(0).ESTIMATEID

                    ' 見積情報削除
                    adapter.DelEstimateInfoDataTable(estimateId)

                    ' 見積車両情報削除
                    adapter.DelEstVclInfoDataTable(estimateId)

                    ' 見積車両オプション情報削除
                    adapter.DelEstVclOptionInfoDataTable(estimateId, mode, vclOptionUpdateDvs)

                    If mode = 0 Then
                        ' -----------------------------------------------
                        ' -- 実行モード：0（全情報 ）
                        ' -----------------------------------------------

                        ' 見積保険情報削除
                        adapter.DelEstInsuranceInfoDataTable(estimateId)

                        ' 見積支払い方法情報削除
                        adapter.DelEstPaymentInfoDataTable(estimateId)

                        ' 見積顧客情報削除
                        adapter.DelEstCustomerInfoDataTable(estimateId)

                        ' 見積諸費用情報削除
                        adapter.DelEstChargeInfoDataTable(estimateId)

                        ' 見積下取車両情報削除
                        adapter.DelEstTradeInCarInfoDataTable(estimateId)

                    End If

                    ' 見積関連情報登録
                    RegistEstimationInfo(CShort(mode), estimateId, estInfoDataSet, adapter)
                Case 2
                    ' -----------------------------------------------
                    ' -- 更新区分：2（削除）
                    ' -----------------------------------------------

                    ' 見積管理IDの取得
                    estimateId = estInfoDataSet.IC3070202EstimationInfo.Item(0).ESTIMATEID

                    ' 見積情報削除（論理削除）
                    adapter.UpdEstimateInfoDataTable(estimateId)

            End Select

            ' プロパティに結果をセット
            Me.prpResultId = adapter.ResultId

            ' 見積情報登録結果データテーブルに結果をセット
            estResultRow.EstimateId = estimateId
            estResultRow.IsSuccess = True
            estResultRow.CreateDate = Me.prpCreateDate
            estResultDT.AddIC3070202EstResultRow(estResultRow)

            ' 結果を返却
            Return estResultDT

        Catch ex As Exception
            Me.Rollback = True
            If adapter.ResultId <> ErrCodeSuccess Then
                Me.prpResultId = adapter.ResultId
            Else
                Me.prpResultId = ErrCodeSys
            End If
            Logger.Error(Me.prpResultId.ToString(CultureInfo.InvariantCulture), ex)

            Throw
        Finally
            adapter = Nothing
        End Try

    End Function
#End Region

#Region "Privateメソッド"
    ''' <summary>
    ''' 見積関連情報を登録する。
    ''' </summary>
    ''' <param name="mode">実行モード（0：全ての見積関連情報を更新/1：見積情報、見積車両情報、見積車両オプション情報のみ更新）</param>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <param name="estInfoDataSet">見積情報データセット</param>
    ''' <param name="adapter">データアダプタのインスタンス</param>
    ''' <remarks></remarks>
    Private Sub RegistEstimationInfo(ByVal mode As Short, _
                                            ByVal estimateId As Long, _
                                            ByVal estInfoDataSet As IC3070202DataSet, _
                                            ByVal adapter As IC3070202TableAdapter)

        Dim estimationInfoRow As IC3070202DataSet.IC3070202EstimationInfoRow = _
            estInfoDataSet.IC3070202EstimationInfo.Item(0)

        ' 見積情報登録
        adapter.InsEstimateInfoDataTable(estimateId, CONTPRINTFLG_VALUE, estimationInfoRow)

        ' 作成日取得
        Me.prpCreateDate = adapter.SelCreateDate(estimateId)

        ' 見積車両情報登録
        adapter.InsEstVclInfoDataTable(estimateId, estimationInfoRow)

        ' 見積車両オプション情報登録
        For Each estVclOptionInfoDataRow As IC3070202DataSet.IC3070202EstVclOptionInfoRow In estInfoDataSet.IC3070202EstVclOptionInfo
            adapter.InsEstVclOptionInfoDataTable(estimateId, estVclOptionInfoDataRow, estimationInfoRow)
        Next

        If mode = 0 Then
            ' -----------------------------------------------
            ' -- 実行モード：0（全情報 ）
            ' -----------------------------------------------

            ' 見積保険情報登録
            For Each estInsuranceInfoDataRow As IC3070202DataSet.IC3070202EstInsuranceInfoRow In estInfoDataSet.IC3070202EstInsuranceInfo
                adapter.InsEstInsuranceInfoDataTable(estimateId, estInsuranceInfoDataRow, estimationInfoRow)
            Next

            ' 見積支払い方法情報登録
            For Each estPaymentInfoDataRow As IC3070202DataSet.IC3070202EstPaymentInfoRow In estInfoDataSet.IC3070202EstPaymentInfo
                adapter.InsEstPaymentInfoDataTable(estimateId, estPaymentInfoDataRow, estimationInfoRow)
            Next

            ' 見積顧客情報登録
            For Each estCustomerDataRow As IC3070202DataSet.IC3070202EstCustomerInfoRow In estInfoDataSet.IC3070202EstCustomerInfo
                adapter.InsEstCustomerInfoDataTable(estimateId, estCustomerDataRow, estimationInfoRow)
            Next

            ' 見積諸費用情報登録
            For Each estChargeInfoDataRow As IC3070202DataSet.IC3070202EstChargeInfoRow In estInfoDataSet.IC3070202EstChargeInfo
                adapter.InsEstChargeInfoDataTable(estimateId, estChargeInfoDataRow, estimationInfoRow)
            Next

            ' 見積下取車両情報登録
            For Each estTradeInCarInfoDataRow As IC3070202DataSet.IC3070202EstTradeInCarInfoRow In estInfoDataSet.IC3070202EstTradeInCarInfo
                adapter.InsEstTradeInCarInfoDataTable(estimateId, estTradeInCarInfoDataRow, estimationInfoRow)
            Next

        End If

    End Sub
#End Region

End Class
