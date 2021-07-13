'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'IC3070202BusinessLogic.vb
'─────────────────────────────────────
'機能： 見積登録I/F
'補足： 
'作成： 2011/12/01 TCS 鈴木(健)
'更新： 2012/02/07 TCS 明瀬【SALES_1B】
'       2012/03/02 TCS 劉  【SALES_2】
'       2013/03/12 TCS 神本【A STEP2】新車タブレット見積り画面機能拡充対応
'更新： 2013/06/30 TCS 趙   【A STEP2】i-CROP新DB適応に向けた機能開発（既存流用）
'更新： 2013/12/12 TCS 森 Aカード情報相互連携開発
'更新： 2014/05/28 TCS 安田 受注時説明機能開発（受注後工程スケジュール）
'更新： 2017/11/20 TCS 河原 TKM独自機能開発
'更新： 2018/04/17 TCS 河原 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証
'更新： 2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展
'更新： 2020/01/06 TS  重松 [TMTレスポンススロー] SLT基盤への横展
'─────────────────────────────────────

Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.Estimate.Quotation.DataAccess
Imports Toyota.eCRB.Tool.Notify.Api.DataAccess
Imports Toyota.eCRB.Tool.Notify.Api.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic

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

    '2012/02/07 TCS 明瀬【SALES_1B】START

    ''' <summary>
    ''' 依頼種別ID 価格相談
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NOTICEREQ_DISCOUNTAPPROVAL As String = "02"

    ''' <summary>
    ''' 通知ステータス キャンセル
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STATUS_CANCEL As String = "2"

    ''' <summary>
    ''' 通知I/F処理結果コード　成功
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NOTICEIF_SUCCESS As String = "000000"

    ''' <summary>
    ''' 通知I/F処理結果コード　DBタイムアウト
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NOTICEIF_DBTIMEOUT As String = "006000"

    ''' <summary>
    ''' I/Fパラメータ　カテゴリータイプ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IFPARAM_CATEGORY As String = "1"

    ''' <summary>
    ''' I/Fパラメータ　表示位置
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IFPARAM_DISPPOSITION As String = "1"

    ''' <summary>
    ''' I/Fパラメータ　表示時間
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IFPARAM_DISPTIME As Long = 3

    ''' <summary>
    ''' I/Fパラメータ　表示タイプ(テキスト)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IFPARAM_DISPTYPE As String = "1"

    ''' <summary>
    ''' I/Fパラメータ　色(薄い黄色)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IFPARAM_COLOR As String = "1"

    ''' <summary>
    ''' I/Fパラメータ　表示時関数(MG画面でMG通知を開く関数)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IFPARAM_DISPFUNCTION As String = "icropScript.ui.openNoticeList()"

    ''' <summary>
    ''' エラーコード：通知I/Fエラー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ErrCodeNoticeIF As Short = 6000

    ''' <summary>
    ''' 画面ID：見積登録IF
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MY_PROGRAMID As String = "IC3070202"

    '2012/02/07 TCS 明瀬【SALES_1B】END

    ' 2013/12/12 TCS 森 Aカード情報相互連携開発 START

    ''' <summary>
    ''' 希望者の商談見込み度コード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ENVSETTINGKEY_MOST_PREFERRED_PROSPECT_CD As String = "MOST_PREFERRED_PROSPECT_CD"

    ''' <summary>
    ''' 外装飾コード：先頭の空白除去して渡す処理実行フラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ENVSETTINGKEY_USE_UNTRIMMED_COLOR_CD As String = "USE_UNTRIMMED_COLOR_CD"

    ''' <summary>
    ''' 外装飾コードが３桁の場合のみ空白を追加する
    ''' </summary>
    ''' <remarks></remarks>
    Private Const EXTCOLORCD_CONV_APPEND_LENGTH As Integer = 3
    ' 2013/12/12 TCS 森 Aカード情報相互連携開発 END

    '更新： 2014/05/28 TCS 安田 受注時説明機能開発（受注後工程スケジュール） START

    ''' <summary>
    ''' 契約条件変更フラグがON
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONTRACT_COND_CHG_FLG_ON As String = "1"

    ''' <summary>
    ''' 契約条件変更フラグがOFF
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONTRACT_COND_CHG_FLG_OFF As String = "0"

    '更新： 2014/05/28 TCS 安田 受注時説明機能開発（受注後工程スケジュール） END

    '2018/04/17 TCS 河原 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 START
    ''' <summary>
    ''' サフィックス使用可否フラグ名称
    ''' </summary>
    ''' <remarks></remarks>
    Private Const USE_FLG_SUFFIX As String = "USE_FLG_SUFFIX"

    ''' <summary>
    ''' 内装色使用可否フラグ名称
    ''' </summary>
    ''' <remarks></remarks>
    Private Const USE_FLG_INTERIORCLR As String = "USE_FLG_INTERIORCLR"
    '2018/04/17 TCS 河原 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 END

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

    '2012/02/07 TCS 明瀬【SALES_1B】START
    ''' <summary>
    ''' 通知登録I/Fのエラーフラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private prpNoticeIfErr As Boolean = False

    ''' <summary>
    ''' 通知登録I/FのDBタイムアウト発生フラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private prpNoticeIfDbTimeOut As Boolean = False
    '2012/02/07 TCS 明瀬【SALES_1B】END
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
    '2013/06/30 TCS 趙 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 見積情報を登録する。
    ''' </summary>
    ''' <param name="estInfoDataSet">見積情報データセット</param>
    ''' <param name="updateDvs">更新区分（0：登録/1：更新/2：削除）</param>
    ''' <param name="mode">実行モード（0：全ての見積関連情報を更新/1：見積情報、見積車両情報、見積車両オプション情報のみ更新）</param>
    ''' <param name="vclOptionUpdateDvs">車両オプション更新区分（0：全ての車両オプションを更新/1：メーカーの車両オプションのみ更新）</param>
    ''' <param name="changemode">TCVフラグ 0:ＴＣＶ以外、1:ＴＣＶ</param>
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
                                      ByVal vclOptionUpdateDvs As Integer, _
                                      ByVal changemode As Integer) As IC3070202DataSet.IC3070202EstResultDataTable
    '2013/06/30 TCS 趙 2013/10対応版　既存流用 END
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

            '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 START
            '更新： 2014/05/28 TCS 安田 受注時説明機能開発（受注後工程スケジュール） START

            '契約承認後に、前回の保険・支払が見積保存時から変更された。もしくは、TCVで新規車両選択時
            '契約条件変更フラグをONにする
            Dim estChgFlg As String = CONTRACT_COND_CHG_FLG_OFF
            If ((updateDvs = 0) OrElse (updateDvs = 1)) AndAlso
                (Not estInfoDataSet.IC3070202EstimationInfo.Item(0).IsFLLWUPBOX_SEQNONull) Then

                '注文承認されているかチェックする
                Dim ckBookAfter As Boolean = adapter.CheckBookAfter(estInfoDataSet.IC3070202EstimationInfo.Item(0))

                '受注後の場合チェックする
                If (ckBookAfter = True) Then

                    Logger.Info("1.受注後データ" & changemode)

                    If (changemode <> 0) Then

                        Logger.Info("20.TCV")

                        'TCV
                        estChgFlg = CONTRACT_COND_CHG_FLG_ON

                    Else
                        'TCV以外
                        estChgFlg = GetOdrConfChangFlg(adapter, estInfoDataSet, estChgFlg)

                        Logger.Info("10.TCV以外")

                    End If
                End If

            End If
            '更新： 2014/05/28 TCS 安田 受注時説明機能開発（受注後工程スケジュール） END
            '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 END

            ' 更新区分により、処理分岐
            Select Case updateDvs
                Case 0
                    ' -----------------------------------------------
                    ' -- 更新区分：0（新規登録）
                    ' -----------------------------------------------

                    ' 見積管理IDシーケンス取得
                    estimateId = adapter.SelEstimateId()
                    '2013/06/30 TCS 趙 2013/10対応版　既存流用 START
                    ' 見積関連情報登録
                    RegistEstimationInfo(CShort(mode), estimateId, estInfoDataSet, adapter, changemode, estChgFlg)
                    '2013/06/30 TCS 趙 2013/10対応版　既存流用 END

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

                    '2013/03/12 TCS 神本【A STEP2】新車タブレット見積り画面機能拡充対応 START
                    'TCVとi-CROPで同じメーカーオプションを選択していれば、i-CROPで設定したメーカーオプションの価格を引き継ぐ
                    'i-CROPで変更したメーカーオプション価格を取得する。
                    If vclOptionUpdateDvs = 1 Then
                        '車両オプション更新区分が"1"(車両オプションをメーカーオプションのみ更新)の場合のみ、
                        'i-CROPで変更したメーカーオプション価格を取得する
                        Dim bizIC3070201 As New IC3070201BusinessLogic
                        Dim dsIC3070201DataSet As IC3070201DataSet
                        '2013/06/30 TCS 趙 2013/10対応版　既存流用 START
                        '見積情報を取得する。
                        dsIC3070201DataSet = bizIC3070201.GetEstimationInfo(estimateId, 1, changemode)
                        '2013/06/30 TCS 趙 2013/10対応版　既存流用 END
                        'i-CROPで設定したメーカーオプションを取得する。
                        Dim makerOption_iCROP As IC3070201DataSet.IC3070201VclOptionInfoDataTable =
                            DirectCast(dsIC3070201DataSet.Tables("IC3070201VclOptionInfo"), IC3070201DataSet.IC3070201VclOptionInfoDataTable)

                        'TCVで設定したメーカーオプションを取得する。
                        Dim makerOption_TCV As IC3070202DataSet.IC3070202EstVclOptionInfoDataTable = estInfoDataSet.IC3070202EstVclOptionInfo

                        'i-CROPで設定したメーカーオプション価格引継ぎ
                        Me.UpdateTCVMakerOption(makerOption_TCV, makerOption_iCROP)

                    End If
                    '2013/03/12 TCS 神本【A STEP2】新車タブレット見積り画面機能拡充対応 END

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

                        '2012/02/07 TCS 明瀬【SALES_1B】START
                        '2013/06/30 TCS 趙 2013/10対応版　既存流用 START
                        ' 見積関連情報登録
                        RegistEstimationInfo(CShort(mode), estimateId, estInfoDataSet, adapter, changemode, estChgFlg)
                        '2013/06/30 TCS 趙 2013/10対応版　既存流用 END

                    Else
                        ' -----------------------------------------------
                        ' -- 実行モード：1（一部の情報 ）
                        ' -----------------------------------------------
                        If vclOptionUpdateDvs = 0 Then

                            ' 見積保険情報削除
                            adapter.DelEstInsuranceInfoDataTable(estimateId)

                            ' 見積支払い方法情報削除
                            adapter.DelEstPaymentInfoDataTable(estimateId)

                            ' 見積諸費用情報削除
                            adapter.DelEstChargeInfoDataTable(estimateId)

                            '値引き額を削除
                            estInfoDataSet.IC3070202EstimationInfo.Item(0).SetDISCOUNTPRICENull()
                            '2013/06/30 TCS 趙 2013/10対応版　既存流用 START
                            ' 見積関連情報登録
                            RegistEstimationInfo(CShort(mode), estimateId, estInfoDataSet, adapter, changemode, estChgFlg)
                            '2013/06/30 TCS 趙 2013/10対応版　既存流用 END

                            '価格相談中の通知情報を取得する
                            Dim priceConsultationDT As IC3070202DataSet.IC3070202PriceConsultationDataTable
                            priceConsultationDT = adapter.SelPriceConsultationInfo(estimateId)

                            '価格相談中の通知情報があればキャンセルする
                            If Not priceConsultationDT.Rows.Count = 0 Then
                                '通知登録I/Fを呼び出す
                                Dim rtnXml As XmlCommon = Me.NoticeRequest(estInfoDataSet.IC3070202EstimationInfo.Item(0), priceConsultationDT)

                                If Not rtnXml.ResultId.Equals(NOTICEIF_SUCCESS) Then
                                    prpNoticeIfDbTimeOut = True
                                    Throw New OracleExceptionEx
                                End If
                            End If
                        Else
                            '2013/06/30 TCS 趙 2013/10対応版　既存流用 START
                            ' 見積関連情報登録
                            RegistEstimationInfo(CShort(mode), estimateId, estInfoDataSet, adapter, changemode, estChgFlg)
                            '2013/06/30 TCS 趙 2013/10対応版　既存流用 END
                            '2012/02/07 TCS 明瀬【SALES_1B】END
                        End If
                        '2012/02/07 TCS 明瀬【SALES_1B】START
                    End If
                    ' 見積関連情報登録
                    'RegistEstimationInfo(CShort(mode), estimateId, estInfoDataSet, adapter)
                    '2012/02/07 TCS 明瀬【SALES_1B】END
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

                '2012/02/07 TCS 明瀬【SALES_1B】START
            ElseIf prpNoticeIfDbTimeOut Then
                '通知I/FからDBタイムアウトで返却された場合は結果ID：6000で返却
                Me.prpResultId = ErrCodeNoticeIF

            ElseIf prpNoticeIfErr Then
                '通知I/F内で例外が発生した場合は結果ID：6001で返却
                Me.prpResultId = ErrCodeNoticeIF + 1
                '2012/02/07 TCS 明瀬【SALES_1B】END
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
    '2013/06/30 TCS 趙 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 見積関連情報を登録する。
    ''' </summary>
    ''' <param name="mode">実行モード（0：全ての見積関連情報を更新/1：見積情報、見積車両情報、見積車両オプション情報のみ更新）</param>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <param name="estInfoDataSet">見積情報データセット</param>
    ''' <param name="adapter">データアダプタのインスタンス</param>
    ''' <param name="changemode">TCVフラグ 0:ＴＣＶ以外、1:ＴＣＶ</param>
    ''' <param name="estChgFlg">契約条件変更フラグ</param>
    ''' <remarks></remarks>
    Private Sub RegistEstimationInfo(ByVal mode As Short, _
                                            ByVal estimateId As Long, _
                                            ByVal estInfoDataSet As IC3070202DataSet, _
                                            ByVal adapter As IC3070202TableAdapter, _
                                            ByVal changemode As Integer, _
                                            ByVal estChgFlg As String)

        '2013/06/30 TCS 趙 2013/10対応版　既存流用 END
        Dim estimationInfoRow As IC3070202DataSet.IC3070202EstimationInfoRow = _
            estInfoDataSet.IC3070202EstimationInfo.Item(0)

        ' TCV時に、外装飾コード：先頭に空白を追加する
        Me.ChangeExtColor(changemode, estimationInfoRow)

        '更新： 2014/05/28 TCS 安田 受注時説明機能開発（受注後工程スケジュール） START
        ' 見積情報登録
        adapter.InsEstimateInfoDataTable(estimateId, CONTPRINTFLG_VALUE, estChgFlg, estimationInfoRow)
        '更新： 2014/05/28 TCS 安田 受注時説明機能開発（受注後工程スケジュール） END

        ' 作成日取得
        Me.prpCreateDate = adapter.SelCreateDate(estimateId)
        '2013/06/30 TCS 趙 2013/10対応版　既存流用 START
        ' 見積車両情報登録
        If changemode = 0 Then
            adapter.InsEstVclInfoDataTable(estimateId, estimationInfoRow)
        Else
            ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
            Dim carNameCD As String = adapter.SelCarNameCD(estimationInfoRow)
            If String.IsNullOrEmpty(carNameCD) Then
                carNameCD = estimationInfoRow.SERIESCD
            End If
            adapter.InsCamry(estimateId, estimationInfoRow, Me.prpCreateDate, carNameCD)
            ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 END
        End If
        '2013/06/30 TCS 趙 2013/10対応版　既存流用 END
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
            InsEstCustomerInfo(estInfoDataSet, adapter, estimationInfoRow, estimateId)

            ' 見積諸費用情報登録
            For Each estChargeInfoDataRow As IC3070202DataSet.IC3070202EstChargeInfoRow In estInfoDataSet.IC3070202EstChargeInfo
                adapter.InsEstChargeInfoDataTable(estimateId, estChargeInfoDataRow, estimationInfoRow)
            Next

            ' 見積下取車両情報登録
            For Each estTradeInCarInfoDataRow As IC3070202DataSet.IC3070202EstTradeInCarInfoRow In estInfoDataSet.IC3070202EstTradeInCarInfo
                adapter.InsEstTradeInCarInfoDataTable(estimateId, estTradeInCarInfoDataRow, estimationInfoRow)
            Next

            '2012/03/02 TCS 劉【SALES_2】ADD START
        ElseIf mode = 1 Then
            ' -----------------------------------------------
            ' -- 実行モード：1（車両情報のみ ）
            ' -----------------------------------------------

            '2018/04/17 TCS 河原 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 START
            '使用可否フラグの取得

            '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 START
            'サフィックス使用可否フラグ()
            Dim use_suffix As String
            Dim use_interiorcolor As String

            Dim systemBiz As New SystemSetting
            Dim dataRow As SystemSettingDataSet.TB_M_SYSTEM_SETTINGRow
            dataRow = systemBiz.GetSystemSetting(USE_FLG_SUFFIX)

            If IsNothing(dataRow) Then
                use_suffix = "0"
            Else
                use_suffix = dataRow.SETTING_VAL
            End If

            '内装色使用可否フラグ()
            Dim dataRowclr As SystemSettingDataSet.TB_M_SYSTEM_SETTINGRow
            dataRowclr = systemBiz.GetSystemSetting(USE_FLG_INTERIORCLR)

            If IsNothing(dataRowclr) Then
                use_interiorcolor = "0"
            Else
                use_interiorcolor = dataRowclr.SETTING_VAL
            End If
            '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 END

            '2018/04/17 TCS 河原 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 END

            ' 選択希望車種情報登録
            If (estimationInfoRow.IsFLLWUPBOX_SEQNONull = False) Then

                '商談がHistoryテーブルに移行されているかチェックする
                Dim salesHisFlg As Boolean
                salesHisFlg = adapter.CheckSalesHistory(estimationInfoRow.FLLWUPBOX_SEQNO)


                ' 2013/12/12 TCS 森 Aカード情報相互連携開発 START
                '商談見込み度コード既定値設定
                Dim mostPerfCd As String
                mostPerfCd = GetSysEnvSettingValue(ENVSETTINGKEY_MOST_PREFERRED_PROSPECT_CD)
                ' 2013/12/12 TCS 森 Aカード情報相互連携開発 END

                ' 選択希望車種取得
                Dim fllwupboxSelectedSeriesDT As IC3070202DataSet.IC3070202FllwupboxSelectedSeriesDataTable
                fllwupboxSelectedSeriesDT = adapter.GetPriferdModel(estimationInfoRow, salesHisFlg, use_suffix, use_interiorcolor)

                'Dim i As Integer = 0
                'Dim flgCheckColorcd As Boolean = False            ' 対象データはDBに存在するか

                ' 対象データ存在しない、登録
                If fllwupboxSelectedSeriesDT.Rows.Count = 0 Then

                    ' 2013/12/12 TCS 森 Aカード情報相互連携開発 START
                    ' 一押し希望車をクリアする
                    adapter.UpdateMostPreferred(estimationInfoRow.FLLWUPBOX_SEQNO, estimationInfoRow.UPDATEACCOUNT, mostPerfCd, salesHisFlg)
                    ' 2013/12/12 TCS 森 Aカード情報相互連携開発 END

                    ' シーケンスNo取得
                    '2013/06/30 TCS 趙 2013/10対応版　既存流用 START
                    Dim seqno As Long = CLng(adapter.SelSeqno(estimationInfoRow, salesHisFlg))
                    '2013/06/30 TCS 趙 2013/10対応版　既存流用 END

                    ' 車種コード取得
                    Dim carNameCD As String = adapter.SelCarNameCD(estimationInfoRow)

                    ' 選択希望車種登録
                    '2013/06/30 TCS 趙 2013/10対応版　既存流用 START
                    adapter.InsPriferdModel(estimationInfoRow, seqno, carNameCD, mostPerfCd, salesHisFlg)
                    '2013/06/30 TCS 趙 2013/10対応版　既存流用 END

                Else

                    '2018/04/17 TCS 河原 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 START
                    '2017/11/20 TCS 河原 TKM独自機能開発 START
                    If (Trim(fllwupboxSelectedSeriesDT.Item(0).MODELCD).Length = 0 And Trim(estimationInfoRow.MODELCD).Length > 0) Then
                        'グレードコード未選択の希望車とマッチした場合
                        '既存の希望車種のグレード以下を更新
                        adapter.UpdatePreferVcl(1, fllwupboxSelectedSeriesDT.Item(0).SEQNO, estimationInfoRow, salesHisFlg)
                    ElseIf String.Equals(use_suffix, "1") And (Trim(fllwupboxSelectedSeriesDT.Item(0).SUFFIX_CD).Length = 0 And Trim(estimationInfoRow.SUFFIXCD).Length > 0) Then
                        'サフィックスコード未選択の希望車とマッチした場合
                        '既存の希望車種のサフィックス以下を更新
                        adapter.UpdatePreferVcl(2, fllwupboxSelectedSeriesDT.Item(0).SEQNO, estimationInfoRow, salesHisFlg)
                    ElseIf (Trim(fllwupboxSelectedSeriesDT.Item(0).COLORCD).Length = 0 And Trim(estimationInfoRow.EXTCOLORCD).Length > 0) Then
                        If String.Equals(use_suffix, "0") Then
                            '外装色コード未選択の希望車とマッチした場合
                            '既存の希望車種の外装色以下を更新
                            adapter.UpdatePreferVcl(2, fllwupboxSelectedSeriesDT.Item(0).SEQNO, estimationInfoRow, salesHisFlg)
                        Else
                            '外装色コード未選択の希望車とマッチした場合
                            '既存の希望車種の外装色以下を更新
                            adapter.UpdatePreferVcl(3, fllwupboxSelectedSeriesDT.Item(0).SEQNO, estimationInfoRow, salesHisFlg)
                        End If
                    ElseIf String.Equals(use_interiorcolor, "1") And (Trim(fllwupboxSelectedSeriesDT.Item(0).INTERIORCLR_CD).Length = 0 And Trim(estimationInfoRow.INTCOLORCD).Length > 0) Then
                        '内装色コード未選択の希望車とマッチした場合
                        '既存の希望車種の内装色以下を更新
                        adapter.UpdatePreferVcl(4, fllwupboxSelectedSeriesDT.Item(0).SEQNO, estimationInfoRow, salesHisFlg)
                    End If
                    '2017/11/20 TCS 河原 TKM独自機能開発 END
                    '2018/04/17 TCS 河原 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 END

                End If
            End If

            '2012/03/02 TCS 劉【SALES_2】ADD END
        End If

    End Sub

    '2012/02/07 TCS 明瀬【SALES_1B】START
    ''' <summary>
    ''' 通知登録IF呼び出し
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>通知登録IFを呼び出す。</remarks>
    Private Function NoticeRequest(ByVal estimateInfoRow As IC3070202DataSet.IC3070202EstimationInfoRow, _
                                   ByVal priceConsultationDT As IC3070202DataSet.IC3070202PriceConsultationDataTable) As XmlCommon

        Logger.Info(String.Format(CultureInfo.InvariantCulture, System.Reflection.MethodBase.GetCurrentMethod.Name & "_Start"))

        Try

            Using noticeData As New XmlNoticeData

                '送信日付
                noticeData.TransmissionDate = DateTimeFunc.Now(estimateInfoRow.DLRCD)

                Dim managerAccount As String = priceConsultationDT.Item(0).MANAGERACCOUNT
                Dim managerName As String = priceConsultationDT.Item(0).MANAGERNAME
                Dim staffAccount As String = priceConsultationDT.Item(0).STAFFACCOUNT
                Dim staffName As String = priceConsultationDT.Item(0).STAFFNAME
                Dim requestId As Long = priceConsultationDT.Item(0).NOTICEREQID

                'accountにデータを格納
                Using account As New XmlAccount

                    account.ToAccount = managerAccount      'スタッフコード（受信先）
                    account.ToAccountName = managerName     '受信者名（受信先）

                    '格納したデータを親クラスに格納
                    noticeData.AccountList.Add(account)
                End Using

                'requestNoticeにデータを格納
                Using requestNotice As New XmlRequestNotice

                    requestNotice.DealerCode = estimateInfoRow.DLRCD                '販売店コード
                    requestNotice.StoreCode = estimateInfoRow.STRCD                 '店舗コード
                    requestNotice.RequestClass = NOTICEREQ_DISCOUNTAPPROVAL         '依頼種別(02:価格相談)
                    requestNotice.Status = STATUS_CANCEL                            'ステータス(2:キャンセル)
                    requestNotice.RequestId = requestId                             '依頼ID
                    requestNotice.RequestClassId = estimateInfoRow.ESTIMATEID       '依頼種別ID(見積管理ID)
                    requestNotice.FromAccount = staffAccount                        'スタッフコード（送信元：通知の返却者）
                    requestNotice.FromAccountName = staffName                       'スタッフ名（送信元：通知の返却者）

                    '格納したデータを親クラスに格納
                    noticeData.RequestNotice = requestNotice
                End Using

                'pushInfoにデータを格納
                Using pushInfo As New XmlPushInfo

                    pushInfo.PushCategory = IFPARAM_CATEGORY                                        'カテゴリータイプ
                    pushInfo.PositionType = IFPARAM_DISPPOSITION                                    '表示位置
                    pushInfo.Time = IFPARAM_DISPTIME                                                '表示時間
                    pushInfo.Color = IFPARAM_COLOR                                                  '色
                    pushInfo.DisplayType = IFPARAM_DISPTYPE                                         '表示タイプ
                    pushInfo.DisplayContents = WebWordUtility.GetWord(MY_PROGRAMID, 9000)           '表示内容
                    pushInfo.DisplayFunction = IFPARAM_DISPFUNCTION                                 '表示時関数
                    pushInfo.ActionFunction = IFPARAM_DISPFUNCTION                                  'アクション時関数

                    '格納したデータを親クラスに格納
                    noticeData.PushInfo = pushInfo

                End Using

                'ロジックを呼ぶ
                Using apiBiz As New IC3040801BusinessLogic

                    'i-CROPへ送信
                    Dim rtnXml As XmlCommon = apiBiz.NoticeDisplay(noticeData, ConstCode.NoticeDisposal.Peculiar)

                    Logger.Info(String.Format(CultureInfo.InvariantCulture, System.Reflection.MethodBase.GetCurrentMethod.Name & _
                                      "_End[Message:{0}][NoticeRequestId:{1}][ResultId:{2}]", rtnXml.Message, rtnXml.NoticeRequestId.ToString(CultureInfo.CurrentCulture), rtnXml.ResultId))

                    Return rtnXml
                End Using

            End Using

        Catch ex As Exception
            prpNoticeIfErr = True
            Throw
        End Try

    End Function
    '2012/02/07 TCS 明瀬【SALES_1B】END

    '2013/03/12 TCS 神本【A STEP2】新車タブレット見積り画面機能拡充対応 START
    ''' <summary>
    ''' i-CROPで設定したメーカーオプション価格引継ぎ処理
    ''' </summary>
    ''' <param name="makerOption_TCV">TCVで設定したメーカーオプション</param>
    ''' <param name="makerOption_iCROP">i-CROPで管理しているメーカーオプション</param>
    ''' <remarks></remarks>
    Private Sub UpdateTCVMakerOption(ByVal makerOption_TCV As IC3070202DataSet.IC3070202EstVclOptionInfoDataTable,
                                     ByVal makerOption_iCROP As IC3070201DataSet.IC3070201VclOptionInfoDataTable)

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_Start", System.Reflection.MethodBase.GetCurrentMethod.Name))

        For Each tcvOption In makerOption_TCV
            Dim optionCode As String = tcvOption.OPTIONCODE

            Dim iCROPOptionPrice = (From iCROPOption In makerOption_iCROP
                    Where iCROPOption.OPTIONCODE = optionCode AndAlso
                            iCROPOption.OPTIONPART <> "9"
                    Select iCROPOption.PRICE)


            If iCROPOptionPrice.LongCount > 0 Then
                'i-CROPで設定したメーカーオプション価格をTCVのメーカーオプション価格へ反映する
                Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                              "Update TCV_MakerOption_Price[{0}] to i-CROP_MakerOption_Price[{1}] in MakerOptionCode=[{2}]",
                                             tcvOption.PRICE,
                                             iCROPOptionPrice.FirstOrDefault,
                                             optionCode))
                tcvOption.PRICE = iCROPOptionPrice.FirstOrDefault
            End If
        Next

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_End", System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub
    '2013/03/12 TCS 神本【A STEP2】新車タブレット見積り画面機能拡充対応 END

    ' 2013/12/12 TCS 森 Aカード情報相互連携開発 START
    ''' <summary>
    ''' 見積顧客情報登録
    ''' </summary>
    ''' <param name="estInfoDataSet">見積情報データセット</param>
    ''' <param name="adapter">データアダプタのインスタンス</param>
    ''' <param name="estimationInfoRow">見積情報DataRow</param>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <remarks></remarks>
    Private Sub InsEstCustomerInfo(ByVal estInfoDataSet As IC3070202DataSet, _
                                   ByVal adapter As IC3070202TableAdapter, _
                                   ByVal estimationInfoRow As IC3070202DataSet.IC3070202EstimationInfoRow, _
                                   ByVal estimateId As Long)
        ' 見積顧客情報登録
        For Each estCustomerDataRow As IC3070202DataSet.IC3070202EstCustomerInfoRow In estInfoDataSet.IC3070202EstCustomerInfo
            adapter.InsEstCustomerInfoDataTable(estimateId, estCustomerDataRow, estimationInfoRow)
            If Not estimationInfoRow.IsCRCUSTIDNull() AndAlso "1".Equals(estCustomerDataRow.CONTRACTCUSTTYPE) Then
                '所有者のみ
                adapter.SyncCustomerInfo(estCustomerDataRow, estimationInfoRow)
            End If
        Next
    End Sub

    ''' <summary>
    ''' システム設定値を取得する
    ''' </summary>
    ''' <param name="sysEnvName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetSysEnvSettingValue(ByVal sysEnvName As String) As String
        Dim dr As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow = Nothing
        Dim env As SystemEnvSetting = Nothing

        env = New SystemEnvSetting()
        dr = env.GetSystemEnvSetting(sysEnvName)
        If Not dr Is Nothing Then
            Return dr.PARAMVALUE.Trim()
        End If

        Return String.Empty

    End Function

    ' 2013/12/12 TCS 森 Aカード情報相互連携開発 START
    ''' <summary>
    ''' TCV時に、外装飾コード：先頭に空白を追加する
    ''' </summary>
    ''' <param name="changemode">TCVフラグ 0:ＴＣＶ以外、1:ＴＣＶ</param>
    ''' <param name="estimateRow">IC3070201DataSet.IC3070202EstimationInfoRow</param>
    ''' <remarks>見積管理IDを条件に見積情報の取得を行う</remarks>
    Public Sub ChangeExtColor(ByVal changemode As Integer,
                                ByVal estimateRow As IC3070202DataSet.IC3070202EstimationInfoRow)

        'TCV時(changemode <> 0)に、設定がONの場合に先頭に空白を追加する
        Logger.Info("ChangeEXTCOLORCD2 changemode = " & changemode)
        Dim sysEnvVal As String = GetSysEnvSettingValue(ENVSETTINGKEY_USE_UNTRIMMED_COLOR_CD)
        If ((changemode <> 0) AndAlso "1".Equals(sysEnvVal)) Then

            Logger.Info("estimateRow.EXTCOLORCD2 = " & estimateRow.EXTCOLORCD)
            If estimateRow.EXTCOLORCD.Length = EXTCOLORCD_CONV_APPEND_LENGTH Then
                Logger.Info("estimateRow.EXTCOLORCD Before = " & estimateRow.EXTCOLORCD)
                estimateRow.EXTCOLORCD = " " & estimateRow.EXTCOLORCD
                Logger.Info("estimateRow.EXTCOLORCD After = " & estimateRow.EXTCOLORCD)
            End If

        End If

    End Sub
    ' 2013/12/12 TCS 森 Aカード情報相互連携開発 END

    '更新： 2014/05/28 TCS 安田 受注時説明機能開発（受注後工程スケジュール） START
    ''' <summary>
    ''' 契約情報に変更がないかを確認する
    ''' </summary>
    ''' <param name="adapter"></param>
    ''' <param name="estInfoDataSet"></param>
    ''' <param name="estChgFlg"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetOdrConfChangFlg(ByVal adapter As IC3070202TableAdapter, ByVal estInfoDataSet As IC3070202DataSet, ByVal estChgFlg As String) As String

        'TCV以外
        Dim dtBeforeEstIfo As IC3070202DataSet.IC3070202EstChangeInfoDataTable =
            adapter.GetEstBeforeChangeInfo(estInfoDataSet.IC3070202EstimationInfo.Item(0).ESTIMATEID)

        '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 START
        Logger.Info("10.CONTRACT_COND_CHG_FLG" & dtBeforeEstIfo.Item(0).CONTRACT_COND_CHG_FLG)
        '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 END

        '変更前の契約条件変更フラグ
        estChgFlg = dtBeforeEstIfo.Item(0).CONTRACT_COND_CHG_FLG

        '契約条件変更フラグが、まだ、OFFの場合に、契約条件が変わっていないか判定する
        If (estChgFlg.Equals(CONTRACT_COND_CHG_FLG_OFF)) Then

            ' 見積保険情報
            Dim estInsuranceInfoDataRow As IC3070202DataSet.IC3070202EstInsuranceInfoRow = estInfoDataSet.IC3070202EstInsuranceInfo.NewIC3070202EstInsuranceInfoRow
            For Each estPaymentInfoDataRow2 As IC3070202DataSet.IC3070202EstInsuranceInfoRow In estInfoDataSet.IC3070202EstInsuranceInfo
                estInsuranceInfoDataRow = estPaymentInfoDataRow2
            Next

            ' 見積支払い方法情報
            Dim estPaymentInfoDataRow As IC3070202DataSet.IC3070202EstPaymentInfoRow = estInfoDataSet.IC3070202EstPaymentInfo.NewIC3070202EstPaymentInfoRow
            For Each estPaymentInfoDataRow2 As IC3070202DataSet.IC3070202EstPaymentInfoRow In estInfoDataSet.IC3070202EstPaymentInfo
                If (estPaymentInfoDataRow2.SELECTFLG.Equals(CONTRACT_COND_CHG_FLG_ON)) Then
                    estPaymentInfoDataRow = estPaymentInfoDataRow2
                End If
            Next

            '入力値の頭金支払方法区分
            Dim inputDepMtd As String
            If (dtBeforeEstIfo.Item(0).IsDEPOSITPAYMENTMETHODNull()) Then
                inputDepMtd = String.Empty
            Else
                inputDepMtd = dtBeforeEstIfo.Item(0).DEPOSITPAYMENTMETHOD.TrimEnd
            End If

            '見積の頭金支払方法区分
            Dim estDepMtd As String
            If (estPaymentInfoDataRow.IsDEPOSITPAYMENTMETHODNull()) Then
                estDepMtd = String.Empty
            Else
                estDepMtd = estPaymentInfoDataRow.DEPOSITPAYMENTMETHOD.TrimEnd
            End If

            '見積情報が変更されたかをチェックする
            '見積保険情報.保険区分
            '見積支払情報.支払方法区分
            '見積支払情報.頭金支払方法区分
            If ((Not dtBeforeEstIfo.Item(0).INSUDVS.Equals(estInsuranceInfoDataRow.INSUDVS)) OrElse
                (Not dtBeforeEstIfo.Item(0).PAYMENTMETHOD.Equals(estPaymentInfoDataRow.PAYMENTMETHOD)) OrElse
                (Not inputDepMtd.Equals(estDepMtd))) Then

                estChgFlg = CONTRACT_COND_CHG_FLG_ON
            End If
            '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 START
            Logger.Info("11.INSUDVS=" & dtBeforeEstIfo.Item(0).INSUDVS)
            Logger.Info("12.INSUDVS=" & estInsuranceInfoDataRow.INSUDVS)
            Logger.Info("13.PAYMENTMETHOD=" & dtBeforeEstIfo.Item(0).PAYMENTMETHOD)
            Logger.Info("14.PAYMENTMETHOD=" & estPaymentInfoDataRow.PAYMENTMETHOD)
            Logger.Info("15.DEPOSITPAYMENTMETHOD=" & inputDepMtd)
            Logger.Info("16.DEPOSITPAYMENTMETHOD=" & estDepMtd)
            '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 END
        End If

        Return estChgFlg

    End Function
    '更新： 2014/05/28 TCS 安田 受注時説明機能開発（受注後工程スケジュール） END

#End Region

End Class
