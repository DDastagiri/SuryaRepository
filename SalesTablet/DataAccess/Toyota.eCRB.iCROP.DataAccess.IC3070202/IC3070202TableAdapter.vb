'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'IC3070202TableAdapter.vb
'─────────────────────────────────────
'機能： 見積登録I/F
'補足： 
'作成： 2011/12/01 TCS 鈴木(健)
'更新： 2012/02/07 TCS 明瀬【SALES_1B】
'       2012/03/02 TCS 劉  【SALES_2】
'更新： 2012/03/16 TCS 陳【SALES_2】
'更新： 2012/03/28 TCS 李【SALES_2】
'更新： 2013/01/18 TCS 上田  GL0871対応
'更新： 2013/02/04 TCS 橋本 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発
'更新： 2013/03/12 TCS 神本【A STEP2】新車タブレット見積り画面機能拡充対応
'更新： 2013/06/30 TCS 内藤【A STEP2】i-CROP新DB適応に向けた機能開発（既存流用）
'更新： 2013/12/12 TCS 森 Aカード情報相互連携開発
'更新： 2014/03/07 TCS 各務 再構築不具合対応マージ版
'更新： 2014/04/01 TCS 松月 【A STEP2】TMT不具合対応
'更新： 2014/05/28 TCS 安田 受注時説明機能開発（受注後工程スケジュール）
'更新： 2017/11/20 TCS 河原 TKM独自機能開発
'更新： 2018/04/17 TCS 河原 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証
'削除： 2020/01/06 TS  重松 [TMTレスポンススロー] SLT基盤への横展
'─────────────────────────────────────

Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Globalization
' 2014/03/07 TCS 各務 再構築不具合対応マージ版 START
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
' 2014/03/07 TCS 各務 再構築不具合対応マージ版 END

''' <summary>
''' 見積情報登録I/F
''' テーブルアダプタークラス
''' </summary>
''' <remarks></remarks>
Public Class IC3070202TableAdapter

#Region "定数"
    ''' <summary>
    ''' エラーコード：処理正常終了(該当データ有）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ErrCodeSuccess As Short = 0
    ''' <summary>
    ''' エラーコード：データ存在エラー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ErrCodeDBNothing As Short = 1100
    ''' <summary>
    ''' エラーコード：データ更新エラー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ErrCodeDBUpdate As Short = 9000
    ''' <summary>
    ''' エラーコード：データ重複エラー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ErrCodeDBOverlap As Short = 9100
    ''' <summary>
    ''' エラーコード：システムエラー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ErrCodeSys As Short = 9999

    ''' <summary>
    ''' テーブルコード：見積情報
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TblCodeEstimateInfo As Short = 1
    ''' <summary>
    ''' テーブルコード：見積顧客情報
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TblCodeEstCustomerInfo As Short = 2
    ''' <summary>
    ''' テーブルコード：見積車両情報
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TblCodeEstVclInfo As Short = 3
    ''' <summary>
    ''' テーブルコード：見積車両オプション情報
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TblCodeEstVclOptionInfo As Short = 4
    ''' <summary>
    ''' テーブルコード：見積諸費用情報
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TblCodeEstChargeInfo As Short = 5
    ''' <summary>
    ''' テーブルコード：見積保険情報
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TblCodeEstInsuranceInfo As Short = 6
    ''' <summary>
    ''' テーブルコード：見積支払方法情報
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TblCodeEstPaymentInfo As Short = 7
    ''' <summary>
    ''' テーブルコード：見積下取車両情報
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TblCodeEstTradeInCarInfo As Short = 8
    ''' <summary>
    ''' テーブルコード：見積価格相談
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TblCodeEstDiscountApproval As Short = 9
    ''' <summary>
    ''' テーブルコード：見積費用項目マスタ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TblCodeEstChargeMast As Short = 10
    ''' <summary>
    ''' テーブルコード：融資会社マスタ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TblCodeEstFinanceComMast As Short = 11

    '2012/02/07 TCS 明瀬【SALES_1B】START
    ''' <summary>
    ''' テーブルコード：価格相談情報
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TblCodePriceConsultation As Short = 12
    '2012/02/07 TCS 明瀬【SALES_1B】END

    '2012/03/02 TCS 劉【SALES_2】ADD START
    ''' <summary>
    ''' テーブルコード：Follow-up Box選択車種
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TblCodeFllwupboxSelectedSeries As Short = 13
    ''' <summary>
    ''' テーブルコード：mst車名マスタ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TblCodeMstcarName As Short = 14
    '2012/03/02 TCS 劉【SALES_2】ADD END

    ''' <summary>
    ''' 更新処理区分：0（登録）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const UpdateDvsRegist As Short = 0

    '2012/02/07 TCS 明瀬【SALES_1B】START
    ''' <summary>
    ''' 通知ステータス キャンセル
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STATUS_CANCEL As String = "2"
    '2012/02/07 TCS 明瀬【SALES_1B】END

    '2012/03/28 TCS 李【SALES_2】ADD START
    ''' <summary>
    ''' モデルコード　AHV41L-JEXGBC
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MODEL_CD_HV As String = "AHV41L-JEXGBC"
    ''' <summary>
    ''' シリーズコード　CAMRY
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SERIES_CODE_CAMRY As String = "CAMRY"
    ''' <summary>
    ''' シリーズコード　CMYHV
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SERIES_CODE_CMYHV As String = "CMYHV"
    '2012/03/28 TCS 李【SALES_2】ADD END

    ''' <summary>
    ''' Stringデフォルト値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DEFAULT_STRING_VALUE As String = " "

    ''' <summary>
    ''' 契約承認ステータス 0: 未承認
    ''' </summary>
    ''' <remarks></remarks>
    Public Const STATUS_ANAPPROVED As String = "0"

    ' 2014/03/07 TCS 各務 再構築不具合対応マージ版 START
    ''' <summary>
    ''' 外版色コードを前3桁だけで比較するか否かフラグ 1: 前3桁で比較 0: 全桁で比較
    ''' </summary>
    ''' <remarks></remarks>
    Private Const EXTERIOR_COLOR_3_FLG As String = "EXTERIOR_COLOR_3_FLG"
    ' 2014/03/07 TCS 各務 再構築不具合対応マージ版 END

#End Region

#Region "メンバ変数"
    ''' <summary>
    ''' 更新区分（0：登録/1：更新/2：削除）
    ''' </summary>
    ''' <remarks></remarks>
    Private prpUpdDvs As Short

    ''' <summary>
    ''' 終了コード
    ''' </summary>
    ''' <remarks></remarks>
    Private prpResultId As Short = ErrCodeSuccess
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

#Region "メソッド"

#Region "削除クエリ"
    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 見積情報を削除します。
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <returns>処理結果（成功[True]/失敗[False]）</returns>
    ''' <remarks></remarks>
    Public Function DelEstimateInfoDataTable(ByVal estimateId As Long) As Boolean
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DelEstimateInfoDataTable_Start")
        'ログ出力 End *****************************************************************************

        Try
            ' SQL組み立て
            Dim sql As New StringBuilder

            With sql
                .Append("DELETE /* IC3070202_001 */ ")
                .Append("FROM ")
                .Append("    TBL_ESTIMATEINFO ")
                .Append("WHERE ")
                .Append("    ESTIMATEID = :ESTIMATEID ")
            End With

            ' DbUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("IC3070202_001")

                query.CommandText = sql.ToString()

                ' SQLパラメータ設定
                query.AddParameterWithTypeValue("ESTIMATEID", CType(OracleDbType.Long, OracleDbType), estimateId)

                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DelEstimateInfoDataTable_End")
                'ログ出力 End *****************************************************************************
                ' SQL実行（結果を返却）
                If query.Execute() > 0 Then
                    Return True
                Else
                    Me.prpResultId = ErrCodeDBNothing + TblCodeEstimateInfo
                    Throw New ArgumentException("", "estimateId")
                End If
            End Using
        Catch ex As Exception
            If Me.prpResultId = ErrCodeSuccess Then
                Me.prpResultId = ErrCodeDBUpdate + TblCodeEstimateInfo
            End If
            Throw
        End Try
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

    End Function

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 見積車両情報を削除します。
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <returns>処理結果（成功[True]/失敗[False]）</returns>
    ''' <remarks></remarks>
    Public Function DelEstVclInfoDataTable(ByVal estimateId As Long) As Boolean
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DelEstVclInfoDataTable_Start")
        'ログ出力 End *****************************************************************************

        Try
            ' SQL組み立て
            Dim sql As New StringBuilder

            With sql
                .Append("DELETE /* IC3070202_002 */ ")
                .Append("FROM ")
                .Append("    TBL_EST_VCLINFO ")
                .Append("WHERE ")
                .Append("    ESTIMATEID = :ESTIMATEID ")
            End With

            ' DbUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("IC3070202_002")

                query.CommandText = sql.ToString()

                ' SQLパラメータ設定
                query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Long, estimateId)

                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DelEstVclInfoDataTable_End")
                'ログ出力 End *****************************************************************************
                ' SQL実行（結果を返却）
                If query.Execute() > 0 Then
                    Return True
                Else
                    Me.prpResultId = ErrCodeDBNothing + TblCodeEstVclInfo
                    Throw New ArgumentException("", "estimateId")
                End If
            End Using
        Catch ex As Exception
            If Me.prpResultId = ErrCodeSuccess Then
                Me.prpResultId = ErrCodeDBUpdate + TblCodeEstVclInfo
            End If
            Throw
        End Try
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

    End Function

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 見積車両オプション情報を削除します。
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <param name="mode">実行モード（0：見積の全情報を更新　1：見積の車両情報のみ更新）</param>
    ''' <param name="vclOptionUpdateDvs">車両オプション更新区分（0：車両オプションを全て更新　1：車両オプションをメーカーオプションのみ更新）</param>
    ''' <returns>処理結果（成功[True]/失敗[False]</returns>
    ''' <remarks></remarks>
    Public Function DelEstVclOptionInfoDataTable(ByVal estimateId As Long,
                                                 ByVal mode As Integer,
                                                 ByVal vclOptionUpdateDvs As Integer) As Boolean
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DelEstVclOptionInfoDataTable_Start")
        'ログ出力 End *****************************************************************************

        Try
            ' SQL組み立て
            Dim sql As New StringBuilder

            With sql
                .Append("DELETE /* IC3070202_003 */ ")
                .Append("FROM ")
                .Append("    TBL_EST_VCLOPTIONINFO ")
                .Append("WHERE ")
                .Append("    ESTIMATEID = :ESTIMATEID ")

                ' 実行モードが1、かつ、車両オプション更新区分が1の場合のみ
                ' 削除条件にオプション区分＝メーカーのみの条件を指定
                If mode = 1 And vclOptionUpdateDvs = 1 Then
                    '2012/03/16 TCS 陳【SALES_2】EDIT START
                    '.Append("  AND OPTIONPART = '1' ")
                    .Append("  AND OPTIONPART IN ('1','2') ")
                    '2012/03/16 TCS 陳【SALES_2】EDIT END

                End If
            End With

            ' DbUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("IC3070202_003")

                query.CommandText = sql.ToString()

                ' SQLパラメータ設定
                query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Long, estimateId)

                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DelEstVclOptionInfoDataTable_End")
                'ログ出力 End *****************************************************************************
                ' SQL実行（結果を返却）
                If query.Execute() >= 0 Then
                    Return True
                Else
                    Return False
                End If
            End Using
        Catch ex As Exception
            Me.prpResultId = ErrCodeDBUpdate + TblCodeEstVclOptionInfo
            Throw
        End Try
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

    End Function

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 見積保険情報を削除します。
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <returns>処理結果（成功[True]/失敗[False]）</returns>
    ''' <remarks></remarks>
    Public Function DelEstInsuranceInfoDataTable(ByVal estimateId As Long) As Boolean
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DelEstInsuranceInfoDataTable_Start")
        'ログ出力 End *****************************************************************************

        Try
            ' SQL組み立て
            Dim sql As New StringBuilder

            With sql
                .Append("DELETE /* IC3070202_004 */ ")
                .Append("FROM ")
                .Append("    TBL_EST_INSURANCEINFO ")
                .Append("WHERE ")
                .Append("    ESTIMATEID = :ESTIMATEID ")
            End With

            ' DbUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("IC3070202_004")

                query.CommandText = sql.ToString()

                ' SQLパラメータ設定
                query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Long, estimateId)

                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DelEstInsuranceInfoDataTable_End")
                'ログ出力 End *****************************************************************************
                ' SQL実行（結果を返却）
                If query.Execute() >= 0 Then
                    Return True
                Else
                    Return False
                End If
            End Using
        Catch ex As Exception
            Me.prpResultId = ErrCodeDBUpdate + TblCodeEstInsuranceInfo
            Throw
        End Try
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

    End Function

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 見積支払い方法情報を削除します。
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <returns>処理結果（成功[True]/失敗[False]）</returns>
    ''' <remarks></remarks>
    Public Function DelEstPaymentInfoDataTable(ByVal estimateId As Long) As Boolean
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DelEstPaymentInfoDataTable_Start")
        'ログ出力 End *****************************************************************************

        Try
            ' SQL組み立て
            Dim sql As New StringBuilder
            With sql
                .Append("DELETE /* IC3070202_005 */ ")
                .Append("FROM ")
                .Append("    TBL_EST_PAYMENTINFO ")
                .Append("WHERE ")
                .Append("    ESTIMATEID = :ESTIMATEID ")
            End With

            ' DbUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("IC3070202_005")

                query.CommandText = sql.ToString()

                ' SQLパラメータ設定
                query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Long, estimateId)

                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DelEstPaymentInfoDataTable_End")
                'ログ出力 End *****************************************************************************
                ' SQL実行（結果を返却）
                If query.Execute() >= 0 Then
                    Return True
                Else
                    Return False
                End If
            End Using
        Catch ex As Exception
            Me.prpResultId = ErrCodeDBUpdate + TblCodeEstPaymentInfo
            Throw
        End Try
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

    End Function

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 見積顧客情報を削除します。
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <returns>処理結果（成功[True]/失敗[False]）</returns>
    ''' <remarks></remarks>
    Public Function DelEstCustomerInfoDataTable(ByVal estimateId As Long) As Boolean
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DelEstCustomerInfoDataTable_Start")
        'ログ出力 End *****************************************************************************

        Try
            ' SQL組み立て
            Dim sql As New StringBuilder

            With sql
                .Append("DELETE /* IC3070202_006 */ ")
                .Append("FROM ")
                .Append("    TBL_EST_CUSTOMERINFO ")
                .Append("WHERE ")
                .Append("    ESTIMATEID = :ESTIMATEID ")
            End With

            ' DbUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("IC3070202_006")

                query.CommandText = sql.ToString()

                ' SQLパラメータ設定
                query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Long, estimateId)

                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DelEstCustomerInfoDataTable_End")
                'ログ出力 End *****************************************************************************
                ' SQL実行（結果を返却）
                If query.Execute() >= 0 Then
                    Return True
                Else
                    Return False
                End If
            End Using
        Catch ex As Exception
            Me.prpResultId = ErrCodeDBUpdate + TblCodeEstCustomerInfo
            Throw
        End Try
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

    End Function

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 見積諸費用情報を削除します。
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <returns>処理結果（成功[True]/失敗[False]）</returns>
    ''' <remarks></remarks>
    Public Function DelEstChargeInfoDataTable(ByVal estimateId As Long) As Boolean
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DelEstChargeInfoDataTable_Start")
        'ログ出力 End *****************************************************************************

        Try
            ' SQL組み立て
            Dim sql As New StringBuilder

            With sql
                .Append("DELETE /* IC3070202_007 */ ")
                .Append("FROM ")
                .Append("    TBL_EST_CHARGEINFO ")
                .Append("WHERE ")
                .Append("    ESTIMATEID = :ESTIMATEID ")
            End With

            ' DbUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("IC3070202_007")

                query.CommandText = sql.ToString()

                ' SQLパラメータ設定
                query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Long, estimateId)

                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DelEstChargeInfoDataTable_End")
                'ログ出力 End *****************************************************************************
                ' SQL実行（結果を返却）
                If query.Execute() >= 0 Then
                    Return True
                Else
                    Return False
                End If
            End Using
        Catch ex As Exception
            Me.prpResultId = ErrCodeDBUpdate + TblCodeEstChargeInfo
            Throw
        End Try
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

    End Function

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 見積下取車両情報を削除します。
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <returns>処理結果（成功[True]/失敗[False]）</returns>
    ''' <remarks></remarks>
    Public Function DelEstTradeInCarInfoDataTable(ByVal estimateId As Long) As Boolean
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DelEstTradeInCarInfoDataTable_Start")
        'ログ出力 End *****************************************************************************

        Try
            ' SQL組み立て
            Dim sql As New StringBuilder

            With sql
                .Append("DELETE /* IC3070202_008 */ ")
                .Append("FROM ")
                .Append("    TBL_EST_TRADEINCARINFO ")
                .Append("WHERE ")
                .Append("    ESTIMATEID = :ESTIMATEID ")
            End With

            ' DbUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("IC3070202_008")

                query.CommandText = sql.ToString()

                ' SQLパラメータ設定
                query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Long, estimateId)

                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DelEstTradeInCarInfoDataTable_End")
                'ログ出力 End *****************************************************************************
                ' SQL実行（結果を返却）
                If query.Execute() >= 0 Then
                    Return True
                Else
                    Return False
                End If
            End Using
        Catch ex As Exception
            Me.prpResultId = ErrCodeDBUpdate + TblCodeEstTradeInCarInfo
            Throw
        End Try
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

    End Function
#End Region

#Region "挿入クエリ"
    '2013/12/12 TCS 森 Aカード情報相互連携開発 START
    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 見積情報を挿入します。
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <param name="contPrintFlg">契約書印刷フラグ</param>
    ''' <param name="estChgFlg">契約条件変更フラグ</param>
    ''' <param name="dr">見積情報データテーブル行</param>
    ''' <returns>処理結果（成功[True]/失敗[False]）</returns>
    ''' <remarks></remarks>
    Public Function InsEstimateInfoDataTable(ByVal estimateId As Long, _
                                             ByVal contPrintFlg As String, _
                                             ByVal estChgFlg As String, _
                                             ByVal dr As IC3070202DataSet.IC3070202EstimationInfoRow) As Boolean
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsEstimateInfoDataTable_Start")
        'ログ出力 End *****************************************************************************

        Try
            ' データ行のNullチェック
            If dr Is Nothing Then
                Throw New ArgumentNullException("dr")
            End If

            ' SQL組み立て
            Dim sql As New StringBuilder

            With sql
                .Append("INSERT /* IC3070202_009 */ ")
                .Append("INTO ")
                .Append("    TBL_ESTIMATEINFO ")
                .Append("( ")
                .Append("    ESTIMATEID ")      '見積管理ID
                .Append("  , DLRCD ")           '販売店コード
                .Append("  , STRCD ")           '店舗コード
                .Append("  , FLLWUPBOX_SEQNO ") 'Follow-up Box内連番
                .Append("  , CNT_STRCD ")       '契約店舗コード
                .Append("  , CNT_STAFF ")       '契約スタッフ
                .Append("  , CSTKIND ")         '顧客種別
                .Append("  , CUSTOMERCLASS ")   '顧客分類
                .Append("  , CRCUSTID ")        '活動先顧客コード
                .Append("  , CUSTID ")          '基幹お客様コード
                .Append("  , DELIDATE ")        '納車予定日
                .Append("  , DISCOUNTPRICE ")   '値引き額
                .Append("  , MEMO ")            'メモ
                .Append("  , ESTPRINTDATE ")    '見積印刷日
                .Append("  , CONTRACTNO ")      '契約書No.
                .Append("  , CONTPRINTFLG ")    '契約書印刷フラグ
                .Append("  , CONTRACTFLG ")     '契約状況フラグ
                .Append("  , CONTRACTDATE ")    '契約完了日
                .Append("  , DELFLG ")          '削除フラグ
                .Append("  , CREATEDATE ")      '作成日
                .Append("  , UPDATEDATE ")      '更新日
                .Append("  , CREATEACCOUNT ")   '作成ユーザアカウント
                .Append("  , UPDATEACCOUNT ")   '更新ユーザアカウント
                .Append("  , CREATEID ")        '作成機能ID
                .Append("  , UPDATEID ")        '更新機能ID
                .Append("  , TCVVERSION ")      'TCVバージョン
                '2013/02/04 TCS 橋本 【A.STEP2】Add Start
                .Append("  , EST_ACT_FLG ")     '見積実績フラグ
                '2013/02/04 TCS 橋本 【A.STEP2】Add End
                .Append("  , CONTRACT_APPROVAL_STATUS ")        '契約承認ステータス
                .Append("  , CONTRACT_APPROVAL_STAFF ")         '契約承認スタッフ
                .Append("  , CONTRACT_APPROVAL_REQUESTDATE ")   '契約承認依頼日時
                .Append("  , CONTRACT_APPROVAL_REQUESTSTAFF ")  '契約承認依頼スタッフ
                '更新： 2014/05/28 TCS 安田 受注時説明機能開発（受注後工程スケジュール） START
                .Append("  , CONTRACT_COND_CHG_FLG ")                     '契約条件変更フラグ
                '更新： 2014/05/28 TCS 安田 受注時説明機能開発（受注後工程スケジュール） END
                .Append(") ")
                .Append("VALUES ")
                .Append("( ")
                .Append("    :ESTIMATEID ")      '見積管理ID
                .Append("  , :DLRCD ")           '販売店コード
                .Append("  , :STRCD ")           '店舗コード
                .Append("  , :FLLWUPBOX_SEQNO ") 'Follow-up Box内連番
                .Append("  , :CNT_STRCD ")       '契約店舗コード
                .Append("  , :CNT_STAFF ")       '契約スタッフ
                .Append("  , :CSTKIND ")         '顧客種別
                .Append("  , :CUSTOMERCLASS ")   '顧客分類
                .Append("  , :CRCUSTID ")        '活動先顧客コード
                .Append("  , :CUSTID ")          '基幹お客様コード
                .Append("  , :DELIDATE ")        '納車予定日
                .Append("  , :DISCOUNTPRICE ")   '値引き額
                .Append("  , :MEMO ")            'メモ
                .Append("  , :ESTPRINTDATE ")    '見積印刷日
                .Append("  , :CONTRACTNO ")      '契約書No.
                .Append("  , :CONTPRINTFLG ")    '契約書印刷フラグ
                .Append("  , :CONTRACTFLG ")     '契約状況フラグ
                .Append("  , :CONTRACTDATE ")    '契約完了日
                .Append("  , :DELFLG ")          '削除フラグ
                ' 作成日
                If dr.IsCREATEDATENull Then
                    .Append("  , SYSDATE ")
                Else
                    .Append("  , :CREATEDATE ")
                End If
                .Append("  , SYSDATE ")          '更新日
                .Append("  , :CREATEACCOUNT ")   '作成ユーザアカウント
                .Append("  , :UPDATEACCOUNT ")   '更新ユーザアカウント
                .Append("  , :CREATEID ")        '作成機能ID
                .Append("  , :UPDATEID ")        '更新機能ID
                .Append("  , :TCVVERSION ")      'TCVバージョン
                '2013/02/04 TCS 橋本 【A.STEP2】Add Start
                .Append("  , :EST_ACT_FLG ")     '見積実績フラグ
                '2013/02/04 TCS 橋本 【A.STEP2】Add End
                .Append("  , :CONTRACT_APPROVAL_STATUS ")       '契約承認ステータス
                .Append("  , :CONTRACT_APPROVAL_STAFF ")        '契約承認スタッフ
                .Append("  , :CONTRACT_APPROVAL_REQUESTDATE ")  '契約承認依頼日時
                .Append("  , :CONTRACT_APPROVAL_REQUESTSTAFF ") '契約承認依頼スタッフ
                '更新： 2014/05/28 TCS 安田 受注時説明機能開発（受注後工程スケジュール） START
                .Append("  , :CONTRACT_COND_CHG_FLG ")                    '契約条件変更フラグ
                '更新： 2014/05/28 TCS 安田 受注時説明機能開発（受注後工程スケジュール） END
                .Append(") ")
            End With

            ' DbUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("IC3070202_009")

                query.CommandText = sql.ToString()

                ' SQLパラメータ設定
                query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Long, estimateId)               '見積管理ID
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dr.DLRCD)                      '販売店コード
                '店舗コード
                If dr.IsSTRCDNull Then
                    query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, dr.STRCD)
                End If
                'Follow-up Box内連番

                If dr.IsFLLWUPBOX_SEQNONull Then
                    query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, dr.FLLWUPBOX_SEQNO)
                End If

                '契約店舗コード
                If dr.IsCNT_STRCDNull Then
                    query.AddParameterWithTypeValue("CNT_STRCD", OracleDbType.Char, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("CNT_STRCD", OracleDbType.Char, dr.CNT_STRCD)
                End If
                '契約スタッフ
                If dr.IsCNT_STAFFNull Then
                    query.AddParameterWithTypeValue("CNT_STAFF", OracleDbType.Varchar2, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("CNT_STAFF", OracleDbType.Varchar2, dr.CNT_STAFF)
                End If
                '顧客種別
                If dr.IsCSTKINDNull Then
                    query.AddParameterWithTypeValue("CSTKIND", OracleDbType.Char, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("CSTKIND", OracleDbType.Char, dr.CSTKIND)
                End If
                '顧客分類
                If dr.IsCUSTOMERCLASSNull Then
                    query.AddParameterWithTypeValue("CUSTOMERCLASS", OracleDbType.Char, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("CUSTOMERCLASS", OracleDbType.Char, dr.CUSTOMERCLASS)
                End If
                '活動先顧客コード
                If dr.IsCRCUSTIDNull Then
                    query.AddParameterWithTypeValue("CRCUSTID", OracleDbType.Char, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("CRCUSTID", OracleDbType.Char, dr.CRCUSTID)
                End If
                '基幹お客様コード
                If dr.IsCUSTIDNull Then
                    query.AddParameterWithTypeValue("CUSTID", OracleDbType.NVarchar2, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("CUSTID", OracleDbType.NVarchar2, dr.CUSTID)
                End If
                '納車予定日
                If dr.IsDELIDATENull Then
                    query.AddParameterWithTypeValue("DELIDATE", OracleDbType.Date, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("DELIDATE", OracleDbType.Date, dr.DELIDATE)
                End If
                '値引き額
                If dr.IsDISCOUNTPRICENull Then
                    query.AddParameterWithTypeValue("DISCOUNTPRICE", OracleDbType.Double, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("DISCOUNTPRICE", OracleDbType.Double, dr.DISCOUNTPRICE)
                End If
                'メモ
                If dr.IsMEMONull Then
                    query.AddParameterWithTypeValue("MEMO", OracleDbType.NVarchar2, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("MEMO", OracleDbType.NVarchar2, dr.MEMO)
                End If
                '見積印刷日
                If dr.IsESTPRINTDATENull Then
                    query.AddParameterWithTypeValue("ESTPRINTDATE", OracleDbType.Date, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("ESTPRINTDATE", OracleDbType.Date, dr.ESTPRINTDATE)
                End If
                '契約書No.
                If dr.IsCONTRACTNONull Then
                    query.AddParameterWithTypeValue("CONTRACTNO", OracleDbType.Char, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("CONTRACTNO", OracleDbType.Char, dr.CONTRACTNO)
                End If
                query.AddParameterWithTypeValue("CONTPRINTFLG", OracleDbType.Char, contPrintFlg)           '契約書印刷フラグ
                query.AddParameterWithTypeValue("CONTRACTFLG", OracleDbType.Char, dr.CONTRACTFLG)          '契約状況フラグ
                '契約完了日
                If dr.IsCONTRACTDATENull Then
                    query.AddParameterWithTypeValue("CONTRACTDATE", OracleDbType.Date, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("CONTRACTDATE", OracleDbType.Date, dr.CONTRACTDATE)
                End If
                query.AddParameterWithTypeValue("DELFLG", OracleDbType.Char, dr.DELFLG)                    '削除フラグ
                query.AddParameterWithTypeValue("TCVVERSION", OracleDbType.NVarchar2, dr.TCVVERSION)       'TCVバージョン
                '作成日
                If Not dr.IsCREATEDATENull Then
                    query.AddParameterWithTypeValue("CREATEDATE", OracleDbType.Date, dr.CREATEDATE)
                End If
                query.AddParameterWithTypeValue("CREATEACCOUNT", OracleDbType.Varchar2, dr.CREATEACCOUNT)  '作成ユーザアカウント
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, dr.UPDATEACCOUNT)  '更新ユーザアカウント
                query.AddParameterWithTypeValue("CREATEID", OracleDbType.Varchar2, dr.CREATEID)            '作成機能ID
                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, dr.UPDATEID)            '更新機能ID
                '2013/02/04 TCS 橋本 【A.STEP2】Add Start
                query.AddParameterWithTypeValue("EST_ACT_FLG", OracleDbType.Char, dr.EST_ACT_FLG)          '見積実績フラグ
                '2013/02/04 TCS 橋本 【A.STEP2】Add End
                '契約承認ステータス
                If dr.IsCONTRACT_APPROVAL_STATUSNull Then
                    query.AddParameterWithTypeValue("CONTRACT_APPROVAL_STATUS", OracleDbType.NVarchar2, STATUS_ANAPPROVED)
                Else
                    query.AddParameterWithTypeValue("CONTRACT_APPROVAL_STATUS", OracleDbType.NVarchar2, dr.CONTRACT_APPROVAL_STATUS)
                End If
                '契約承認スタッフ
                If dr.IsCONTRACT_APPROVAL_STAFFNull Then
                    query.AddParameterWithTypeValue("CONTRACT_APPROVAL_STAFF", OracleDbType.Varchar2, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("CONTRACT_APPROVAL_STAFF", OracleDbType.Varchar2, dr.CONTRACT_APPROVAL_STAFF)
                End If
                '契約承認依頼日時
                If dr.IsCONTRACT_APPROVAL_REQUESTDATENull Then
                    query.AddParameterWithTypeValue("CONTRACT_APPROVAL_REQUESTDATE", OracleDbType.Date, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("CONTRACT_APPROVAL_REQUESTDATE", OracleDbType.Date, dr.CONTRACT_APPROVAL_REQUESTDATE)
                End If
                '契約承認依頼スタッフ
                If dr.IsCONTRACT_APPROVAL_REQUESTSTAFFNull Then
                    query.AddParameterWithTypeValue("CONTRACT_APPROVAL_REQUESTSTAFF", OracleDbType.Varchar2, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("CONTRACT_APPROVAL_REQUESTSTAFF", OracleDbType.Varchar2, dr.CONTRACT_APPROVAL_REQUESTSTAFF)
                End If
                '更新： 2014/05/28 TCS 安田 受注時説明機能開発（受注後工程スケジュール） START
                '契約条件変更フラグ
                query.AddParameterWithTypeValue("CONTRACT_COND_CHG_FLG", OracleDbType.NVarchar2, estChgFlg)
                '更新： 2014/05/28 TCS 安田 受注時説明機能開発（受注後工程スケジュール） END

                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsEstimateInfoDataTable_End")
                'ログ出力 End *****************************************************************************
                ' SQL実行（結果を返却）
                If query.Execute() > 0 Then
                    Return True
                Else
                    Return False
                End If
            End Using
        Catch ex As Exception
            If Me.prpUpdDvs = UpdateDvsRegist Then
                Me.prpResultId = ErrCodeDBOverlap + TblCodeEstimateInfo
            Else
                Me.prpResultId = ErrCodeDBUpdate + TblCodeEstimateInfo
            End If
            Throw
        End Try
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END
    End Function

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    '2013/12/12 TCS 森 Aカード情報相互連携開発 END
    ''' <summary>
    ''' 見積車両情報を挿入します。
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <param name="dr">見積情報データテーブル行</param>
    ''' <returns>処理結果（成功[True]/失敗[False]）</returns>
    ''' <remarks></remarks>
    Public Function InsEstVclInfoDataTable(ByVal estimateId As Long, _
                                           ByVal dr As IC3070202DataSet.IC3070202EstimationInfoRow) As Boolean
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsEstVclInfoDataTable_Start")
        'ログ出力 End *****************************************************************************

        Try
            ' データ行のNullチェック
            If dr Is Nothing Then
                Throw New ArgumentNullException("dr")
            End If

            ' SQL組み立て
            Dim sql As New StringBuilder

            With sql
                .Append("INSERT /* IC3070202_010 */ ")
                .Append("INTO ")
                .Append("    TBL_EST_VCLINFO ")
                .Append("( ")
                .Append("    ESTIMATEID ")          '見積管理ID
                .Append("  , SERIESCD ")            'シリーズコード
                .Append("  , SERIESNM ")            'シリーズ名称
                .Append("  , MODELCD ")             'モデルコード
                .Append("  , MODELNM ")             'モデル名称
                .Append("  , BODYTYPE ")            'ボディータイプ
                .Append("  , DRIVESYSTEM ")         '駆動方式
                .Append("  , DISPLACEMENT ")        '排気量
                .Append("  , TRANSMISSION ")        'ミッションタイプ
                .Append("  , SUFFIXCD ")            'サフィックス
                .Append("  , EXTCOLORCD ")          '外装色コード
                .Append("  , EXTCOLOR ")            '外装色名称
                .Append("  , EXTAMOUNT ")           '外装追加費用
                .Append("  , INTCOLORCD ")          '内装色コード
                .Append("  , INTCOLOR ")            '内装色名称
                .Append("  , INTAMOUNT ")           '内装追加費用
                .Append("  , MODELNUMBER ")         '車両型号
                .Append("  , BASEPRICE ")           '車両価格
                .Append("  , CREATEDATE ")          '作成日
                .Append("  , UPDATEDATE ")          '更新日
                .Append("  , CREATEACCOUNT ")       '作成ユーザアカウント
                .Append("  , UPDATEACCOUNT ")       '更新ユーザアカウント
                .Append("  , CREATEID ")            '作成機能ID
                .Append("  , UPDATEID ")            '更新機能ID
                .Append(") ")
                .Append("VALUES ")
                .Append("( ")
                .Append("    :ESTIMATEID ")          '見積管理ID
                .Append("  , :SERIESCD ")            'シリーズコード
                .Append("  , :SERIESNM ")            'シリーズ名称
                .Append("  , :MODELCD ")             'モデルコード
                .Append("  , :MODELNM ")             'モデル名称
                .Append("  , :BODYTYPE ")            'ボディータイプ
                .Append("  , :DRIVESYSTEM ")         '駆動方式
                .Append("  , :DISPLACEMENT ")        '排気量
                .Append("  , :TRANSMISSION ")        'ミッションタイプ
                .Append("  , :SUFFIXCD ")            'サフィックス
                .Append("  , :EXTCOLORCD ")          '外装色コード
                .Append("  , :EXTCOLOR ")            '外装色名称
                .Append("  , :EXTAMOUNT ")           '外装追加費用
                .Append("  , :INTCOLORCD ")          '内装色コード
                .Append("  , :INTCOLOR ")            '内装色名称
                .Append("  , :INTAMOUNT ")           '内装追加費用
                .Append("  , :MODELNUMBER ")         '車両型号
                .Append("  , :BASEPRICE ")           '車両価格
                ' 作成日
                If dr.IsCREATEDATENull Then
                    .Append("  , SYSDATE ")
                Else
                    .Append("  , :CREATEDATE ")
                End If
                .Append("  , SYSDATE ")              '更新日
                .Append("  , :CREATEACCOUNT ")       '作成ユーザアカウント
                .Append("  , :UPDATEACCOUNT ")       '更新ユーザアカウント
                .Append("  , :CREATEID ")            '作成機能ID
                .Append("  , :UPDATEID ")            '更新機能ID
                .Append(") ")
            End With

            ' DbUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("IC3070202_010")

                query.CommandText = sql.ToString()

                ' SQLパラメータ設定
                query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Long, estimateId)                           '見積管理ID
                query.AddParameterWithTypeValue("SERIESCD", OracleDbType.NVarchar2, dr.SERIESCD)                       'シリーズコード
                query.AddParameterWithTypeValue("SERIESNM", OracleDbType.NVarchar2, dr.SERIESNM)                       'シリーズ名称
                query.AddParameterWithTypeValue("MODELCD", OracleDbType.NVarchar2, dr.MODELCD)                         'モデルコード
                query.AddParameterWithTypeValue("MODELNM", OracleDbType.NVarchar2, dr.MODELNM)                         'モデル名称
                query.AddParameterWithTypeValue("BODYTYPE", OracleDbType.NVarchar2, dr.BODYTYPE)                       'ボディータイプ
                query.AddParameterWithTypeValue("DRIVESYSTEM", OracleDbType.NVarchar2, dr.DRIVESYSTEM)                 '駆動方式
                query.AddParameterWithTypeValue("DISPLACEMENT", OracleDbType.NVarchar2, dr.DISPLACEMENT)               '排気量
                query.AddParameterWithTypeValue("TRANSMISSION", OracleDbType.NVarchar2, dr.TRANSMISSION)               'ミッションタイプ
                query.AddParameterWithTypeValue("SUFFIXCD", OracleDbType.Varchar2, dr.SUFFIXCD)                        'サフィックス
                query.AddParameterWithTypeValue("EXTCOLORCD", OracleDbType.Varchar2, dr.EXTCOLORCD)                    '外装色コード
                query.AddParameterWithTypeValue("EXTCOLOR", OracleDbType.NVarchar2, dr.EXTCOLOR)                       '外装色名称
                query.AddParameterWithTypeValue("EXTAMOUNT", OracleDbType.Double, dr.EXTAMOUNT)                        '外装追加費用
                query.AddParameterWithTypeValue("INTCOLORCD", OracleDbType.Varchar2, dr.INTCOLORCD)                    '内装色コード
                query.AddParameterWithTypeValue("INTCOLOR", OracleDbType.NVarchar2, dr.INTCOLOR)                       '内装色名称
                query.AddParameterWithTypeValue("INTAMOUNT", OracleDbType.Double, dr.INTAMOUNT)                        '内装追加費用
                query.AddParameterWithTypeValue("MODELNUMBER", OracleDbType.NVarchar2, dr.MODELNUMBER)                 '車両型号
                query.AddParameterWithTypeValue("BASEPRICE", OracleDbType.Double, dr.BASEPRICE)                        '車両価格
                '作成日
                If Not dr.IsCREATEDATENull Then
                    query.AddParameterWithTypeValue("CREATEDATE", OracleDbType.Date, dr.CREATEDATE)
                End If
                query.AddParameterWithTypeValue("CREATEACCOUNT", OracleDbType.Varchar2, dr.CREATEACCOUNT)              '作成ユーザアカウント
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, dr.UPDATEACCOUNT)              '更新ユーザアカウント
                query.AddParameterWithTypeValue("CREATEID", OracleDbType.Varchar2, dr.CREATEID)                        '作成機能ID
                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, dr.UPDATEID)                        '更新機能ID

                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsEstVclInfoDataTable_End")
                'ログ出力 End *****************************************************************************
                ' SQL実行（結果を返却）
                If query.Execute() > 0 Then
                    Return True
                Else
                    Return False
                End If
            End Using
        Catch ex As Exception
            If Me.prpUpdDvs = UpdateDvsRegist Then
                Me.prpResultId = ErrCodeDBOverlap + TblCodeEstVclInfo
            Else
                Me.prpResultId = ErrCodeDBUpdate + TblCodeEstVclInfo
            End If
            Throw
        End Try
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

    End Function

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 見積車両オプション情報を挿入します。
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <param name="dr">見積車両オプション情報データテーブル行</param>
    ''' <param name="drBase">見積情報データテーブル行</param>
    ''' <returns>処理結果（成功[True]/失敗[False]）</returns>
    ''' <remarks></remarks>
    Public Function InsEstVclOptionInfoDataTable(ByVal estimateId As Long, _
                                                 ByVal dr As IC3070202DataSet.IC3070202EstVclOptionInfoRow, _
                                                 ByVal drBase As IC3070202DataSet.IC3070202EstimationInfoRow) As Boolean
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsEstVclOptionInfoDataTable_Start")
        'ログ出力 End *****************************************************************************

        Try
            ' データ行のNullチェック
            If dr Is Nothing Then
                Throw New ArgumentNullException("dr")
            End If

            If drBase Is Nothing Then
                Throw New ArgumentNullException("drBase")
            End If

            ' SQL組み立て
            Dim sql As New StringBuilder

            With sql
                .Append("INSERT /* IC3070202_011 */ ")
                .Append("INTO ")
                .Append("    TBL_EST_VCLOPTIONINFO ")
                .Append("( ")
                .Append("    ESTIMATEID ")          '見積管理ID
                .Append("  , OPTIONPART ")          'オプション区分
                .Append("  , OPTIONCODE ")          'オプションコード
                .Append("  , OPTIONNAME ")          'オプション名
                .Append("  , PRICE ")               '価格
                .Append("  , INSTALLCOST ")         '取付費用
                .Append("  , CREATEDATE ")          '作成日
                .Append("  , UPDATEDATE ")          '更新日
                .Append("  , CREATEACCOUNT ")       '作成ユーザアカウント
                .Append("  , UPDATEACCOUNT ")       '更新ユーザアカウント
                .Append("  , CREATEID ")            '作成機能ID
                .Append("  , UPDATEID ")            '更新機能ID
                .Append(") ")
                .Append("VALUES ")
                .Append("( ")
                .Append("    :ESTIMATEID ")          '見積管理ID
                .Append("  , :OPTIONPART ")          'オプション区分
                .Append("  , :OPTIONCODE ")          'オプションコード
                .Append("  , :OPTIONNAME ")          'オプション名
                .Append("  , :PRICE ")               '価格
                .Append("  , :INSTALLCOST ")         '取付費用
                .Append("  , SYSDATE ")              '作成日
                .Append("  , SYSDATE ")              '更新日
                .Append("  , :CREATEACCOUNT ")       '作成ユーザアカウント
                .Append("  , :UPDATEACCOUNT ")       '更新ユーザアカウント
                .Append("  , :CREATEID ")            '作成機能ID
                .Append("  , :UPDATEID ")            '更新機能ID
                .Append(") ")
            End With

            ' DbUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("IC3070202_011")

                query.CommandText = sql.ToString()

                ' SQLパラメータ設定
                query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Long, estimateId)                           '見積管理ID
                query.AddParameterWithTypeValue("OPTIONPART", OracleDbType.Char, dr.OPTIONPART)                        'オプション区分
                query.AddParameterWithTypeValue("OPTIONCODE", OracleDbType.Varchar2, dr.OPTIONCODE)                    'オプションコード
                query.AddParameterWithTypeValue("OPTIONNAME", OracleDbType.NVarchar2, dr.OPTIONNAME)                   'オプション名
                query.AddParameterWithTypeValue("PRICE", OracleDbType.Double, dr.PRICE)                                '価格
                '取付費用
                If dr.IsINSTALLCOSTNull Then
                    query.AddParameterWithTypeValue("INSTALLCOST", OracleDbType.Double, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("INSTALLCOST", OracleDbType.Double, dr.INSTALLCOST)
                End If
                query.AddParameterWithTypeValue("CREATEACCOUNT", OracleDbType.Varchar2, drBase.UPDATEACCOUNT)          '作成ユーザアカウント
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, drBase.UPDATEACCOUNT)          '更新ユーザアカウント
                query.AddParameterWithTypeValue("CREATEID", OracleDbType.Varchar2, drBase.UPDATEID)                    '作成機能ID
                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, drBase.UPDATEID)                    '更新機能ID

                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsEstVclOptionInfoDataTable_End")
                'ログ出力 End *****************************************************************************
                ' SQL実行（結果を返却）
                If query.Execute() > 0 Then
                    Return True
                Else
                    Return False
                End If
            End Using
        Catch ex As Exception
            If Me.prpUpdDvs = UpdateDvsRegist Then
                Me.prpResultId = ErrCodeDBOverlap + TblCodeEstVclOptionInfo
            Else
                Me.prpResultId = ErrCodeDBUpdate + TblCodeEstVclOptionInfo
            End If
            Throw
        End Try
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

    End Function

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 見積保険情報を挿入します。
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <param name="dr">見積保険情報データテーブル行</param>
    ''' <param name="drBase">見積情報データテーブル行</param>
    ''' <returns>処理結果（成功[True]/失敗[False]）</returns>
    ''' <remarks></remarks>
    Public Function InsEstInsuranceInfoDataTable(ByVal estimateId As Long, _
                                                 ByVal dr As IC3070202DataSet.IC3070202EstInsuranceInfoRow, _
                                                 ByVal drBase As IC3070202DataSet.IC3070202EstimationInfoRow) As Boolean
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsEstInsuranceInfoDataTable_Start")
        'ログ出力 End *****************************************************************************

        Try
            ' データ行のNullチェック
            If dr Is Nothing Then
                Throw New ArgumentNullException("dr")
            End If

            If drBase Is Nothing Then
                Throw New ArgumentNullException("drBase")
            End If

            ' SQL組み立て
            Dim sql As New StringBuilder

            With sql
                .Append("INSERT /* IC3070202_012 */ ")
                .Append("INTO ")
                .Append("    TBL_EST_INSURANCEINFO ")
                .Append("( ")
                .Append("    ESTIMATEID ")          '見積管理ID
                .Append("  , INSUDVS ")             '保険区分
                .Append("  , INSUCOMCD ")           '保険会社コード
                .Append("  , INSUKIND ")            '保険種別
                .Append("  , AMOUNT ")              '保険金額
                .Append("  , CREATEDATE ")          '作成日
                .Append("  , UPDATEDATE ")          '更新日
                .Append("  , CREATEACCOUNT ")       '作成ユーザアカウント
                .Append("  , UPDATEACCOUNT ")       '更新ユーザアカウント
                .Append("  , CREATEID ")            '作成機能ID
                .Append("  , UPDATEID ")            '更新機能ID
                .Append(") ")
                .Append("VALUES ")
                .Append("( ")
                .Append("    :ESTIMATEID ")          '見積管理ID
                .Append("  , :INSUDVS ")             '保険区分
                .Append("  , :INSUCOMCD ")           '保険会社コード
                .Append("  , :INSUKIND ")            '保険種別
                .Append("  , :AMOUNT ")              '保険金額
                .Append("  , SYSDATE ")              '作成日
                .Append("  , SYSDATE ")              '更新日
                .Append("  , :CREATEACCOUNT ")       '作成ユーザアカウント
                .Append("  , :UPDATEACCOUNT ")       '更新ユーザアカウント
                .Append("  , :CREATEID ")            '作成機能ID
                .Append("  , :UPDATEID ")            '更新機能ID
                .Append(") ")
            End With

            ' DbUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("IC3070202_012")

                query.CommandText = sql.ToString()

                ' SQLパラメータ設定
                query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Long, estimateId)                           '見積管理ID
                query.AddParameterWithTypeValue("INSUDVS", OracleDbType.Char, dr.INSUDVS)                              '保険区分オプション区分
                '保険会社コードオプション区分
                If dr.IsINSUCOMCDNull Then
                    query.AddParameterWithTypeValue("INSUCOMCD", OracleDbType.Char, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("INSUCOMCD", OracleDbType.Char, dr.INSUCOMCD)
                End If
                '保険種別オプション区分
                If dr.IsINSUKINDNull Then
                    query.AddParameterWithTypeValue("INSUKIND", OracleDbType.Char, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("INSUKIND", OracleDbType.Char, dr.INSUKIND)
                End If
                '保険金額取付費用
                If dr.IsAMOUNTNull Then
                    query.AddParameterWithTypeValue("AMOUNT", OracleDbType.Double, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("AMOUNT", OracleDbType.Double, dr.AMOUNT)
                End If
                query.AddParameterWithTypeValue("CREATEACCOUNT", OracleDbType.Varchar2, drBase.UPDATEACCOUNT)          '作成ユーザアカウント
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, drBase.UPDATEACCOUNT)          '更新ユーザアカウント
                query.AddParameterWithTypeValue("CREATEID", OracleDbType.Varchar2, drBase.UPDATEID)                    '作成機能ID
                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, drBase.UPDATEID)                    '更新機能ID

                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsEstInsuranceInfoDataTable_End")
                'ログ出力 End *****************************************************************************
                ' SQL実行（結果を返却）
                If query.Execute() > 0 Then
                    Return True
                Else
                    Return False
                End If
            End Using
        Catch ex As Exception
            If Me.prpUpdDvs = UpdateDvsRegist Then
                Me.prpResultId = ErrCodeDBOverlap + TblCodeEstInsuranceInfo
            Else
                Me.prpResultId = ErrCodeDBUpdate + TblCodeEstInsuranceInfo
            End If
            Throw
        End Try
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

    End Function

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 見積支払い方法情報を挿入します。
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <param name="dr">見積支払い方法情報データテーブル行</param>
    ''' <param name="drBase">見積情報データテーブル行</param>
    ''' <returns>処理結果（成功[True]/失敗[False]）</returns>
    ''' <remarks></remarks>
    ''' <History>
    '''  2013/01/18 TCS 上田  GL0871対応
    ''' </History>
    Public Function InsEstPaymentInfoDataTable(ByVal estimateId As Long, _
                                               ByVal dr As IC3070202DataSet.IC3070202EstPaymentInfoRow, _
                                               ByVal drBase As IC3070202DataSet.IC3070202EstimationInfoRow) As Boolean
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsEstPaymentInfoDataTable_Start")
        'ログ出力 End *****************************************************************************

        Try
            ' データ行のNullチェック
            If dr Is Nothing Then
                Throw New ArgumentNullException("dr")
            End If

            If drBase Is Nothing Then
                Throw New ArgumentNullException("drBase")
            End If

            ' SQL組み立て
            Dim sql As New StringBuilder

            With sql
                .Append("INSERT /* IC3070202_013 */ ")
                .Append("INTO ")
                .Append("    TBL_EST_PAYMENTINFO ")
                .Append("( ")
                .Append("    ESTIMATEID ")          '見積管理ID
                .Append("  , PAYMENTMETHOD ")       '支払方法区分
                .Append("  , FINANCECOMCODE ")      '融資会社コード
                .Append("  , PAYMENTPERIOD ")       '支払期間
                .Append("  , MONTHLYPAYMENT ")      '毎月返済額
                .Append("  , DEPOSIT ")             '頭金
                .Append("  , BONUSPAYMENT ")        'ボーナス時返済額
                .Append("  , DUEDATE ")             '初回支払期限
                .Append("  , DELFLG ")              '削除フラグ
                .Append("  , CREATEDATE ")          '作成日
                .Append("  , UPDATEDATE ")          '更新日
                .Append("  , CREATEACCOUNT ")       '作成ユーザアカウント
                .Append("  , UPDATEACCOUNT ")       '更新ユーザアカウント
                .Append("  , CREATEID ")            '作成機能ID
                .Append("  , UPDATEID ")            '更新機能ID
                ' 2013/01/18 TCS 上田 GL0871対応 START
                .Append("  , SELECTFLG ")            '選択フラグ
                ' 2013/01/18 TCS 上田 GL0871対応 END
                '2013/03/12 TCS 神本【A STEP2】新車タブレット見積り画面機能拡充対応 START
                .Append("  , INTERESTRATE ")            '利率
                '2013/03/12 TCS 神本【A STEP2】新車タブレット見積り画面機能拡充対応 END
                .Append("  , DEPOSITPAYMENTMETHOD ") '頭金支払方法区分
                .Append(") ")
                .Append("VALUES ")
                .Append("( ")
                .Append("    :ESTIMATEID ")          '見積管理ID
                .Append("  , :PAYMENTMETHOD ")       '支払方法区分
                .Append("  , :FINANCECOMCODE ")      '融資会社コード
                .Append("  , :PAYMENTPERIOD ")       '支払期間
                .Append("  , :MONTHLYPAYMENT ")      '毎月返済額
                .Append("  , :DEPOSIT ")             '頭金
                .Append("  , :BONUSPAYMENT ")        'ボーナス時返済額
                .Append("  , :DUEDATE ")             '初回支払期限
                .Append("  , :DELFLG ")              '削除フラグ
                .Append("  , SYSDATE ")              '作成日
                .Append("  , SYSDATE ")              '更新日
                .Append("  , :CREATEACCOUNT ")       '作成ユーザアカウント
                .Append("  , :UPDATEACCOUNT ")       '更新ユーザアカウント
                .Append("  , :CREATEID ")            '作成機能ID
                .Append("  , :UPDATEID ")            '更新機能ID
                ' 2013/01/18 TCS 上田 GL0871対応 START
                .Append("  , :SELECTFLG ")            '選択フラグ
                ' 2013/01/18 TCS 上田 GL0871対応 END
                '2013/03/12 TCS 神本【A STEP2】新車タブレット見積り画面機能拡充対応 START
                .Append("  , :INTERESTRATE ")            '利率
                '2013/03/12 TCS 神本【A STEP2】新車タブレット見積り画面機能拡充対応 END
                .Append("  , :DEPOSITPAYMENTMETHOD ") '頭金支払方法区分
                .Append(") ")
            End With

            ' DbUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("IC3070202_013")

                query.CommandText = sql.ToString()

                ' SQLパラメータ設定
                query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Long, estimateId)                           '見積管理ID
                query.AddParameterWithTypeValue("PAYMENTMETHOD", OracleDbType.Char, dr.PAYMENTMETHOD)                  '支払方法区分
                '融資会社コード
                If dr.IsFINANCECOMCODENull Then
                    query.AddParameterWithTypeValue("FINANCECOMCODE", OracleDbType.Char, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("FINANCECOMCODE", OracleDbType.Char, dr.FINANCECOMCODE)
                End If
                '支払期間
                If dr.IsPAYMENTPERIODNull Then
                    query.AddParameterWithTypeValue("PAYMENTPERIOD", OracleDbType.Int64, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("PAYMENTPERIOD", OracleDbType.Int64, dr.PAYMENTPERIOD)
                End If
                '毎月返済額
                If dr.IsMONTHLYPAYMENTNull Then
                    query.AddParameterWithTypeValue("MONTHLYPAYMENT", OracleDbType.Double, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("MONTHLYPAYMENT", OracleDbType.Double, dr.MONTHLYPAYMENT)
                End If
                '頭金
                If dr.IsDEPOSITNull Then
                    query.AddParameterWithTypeValue("DEPOSIT", OracleDbType.Double, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("DEPOSIT", OracleDbType.Double, dr.DEPOSIT)
                End If
                'ボーナス時返済額
                If dr.IsBONUSPAYMENTNull Then
                    query.AddParameterWithTypeValue("BONUSPAYMENT", OracleDbType.Double, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("BONUSPAYMENT", OracleDbType.Double, dr.BONUSPAYMENT)
                End If
                '初回支払期限
                If dr.IsDUEDATENull Then
                    query.AddParameterWithTypeValue("DUEDATE", OracleDbType.Int64, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("DUEDATE", OracleDbType.Int64, dr.DUEDATE)
                End If
                query.AddParameterWithTypeValue("DELFLG", OracleDbType.Char, dr.DELFLG)                                '削除フラグ
                query.AddParameterWithTypeValue("CREATEACCOUNT", OracleDbType.Varchar2, drBase.UPDATEACCOUNT)          '作成ユーザアカウント
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, drBase.UPDATEACCOUNT)          '更新ユーザアカウント
                query.AddParameterWithTypeValue("CREATEID", OracleDbType.Varchar2, drBase.UPDATEID)                    '作成機能ID
                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, drBase.UPDATEID)                    '更新機能ID
                ' 2013/01/18 TCS 上田 GL0871対応 START
                query.AddParameterWithTypeValue("SELECTFLG", OracleDbType.Char, dr.SELECTFLG)                          '選択フラグ
                ' 2013/01/18 TCS 上田 GL0871対応 END
                '2013/03/12 TCS 神本【A STEP2】新車タブレット見積り画面機能拡充対応 START
                '利率
                If dr.IsINTERESTRATENull Then
                    query.AddParameterWithTypeValue("INTERESTRATE", OracleDbType.Double, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("INTERESTRATE", OracleDbType.Double, dr.INTERESTRATE)
                End If
                '2013/03/12 TCS 神本【A STEP2】新車タブレット見積り画面機能拡充対応 END
                '頭金支払方法区分
                If dr.IsDEPOSITPAYMENTMETHODNull Then
                    query.AddParameterWithTypeValue("DEPOSITPAYMENTMETHOD", OracleDbType.Char, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("DEPOSITPAYMENTMETHOD", OracleDbType.Char, dr.DEPOSITPAYMENTMETHOD)
                End If
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsEstPaymentInfoDataTable_End")
                'ログ出力 End *****************************************************************************
                ' SQL実行（結果を返却）
                If query.Execute() > 0 Then
                    Return True
                Else
                    Return False
                End If
            End Using
        Catch ex As Exception
            If Me.prpUpdDvs = UpdateDvsRegist Then
                Me.prpResultId = ErrCodeDBOverlap + TblCodeEstPaymentInfo
            Else
                Me.prpResultId = ErrCodeDBUpdate + TblCodeEstPaymentInfo
            End If
            Throw
        End Try
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

    End Function


    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 見積顧客情報を挿入します。
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <param name="dr">見積顧客情報データテーブル行</param>
    ''' <param name="drBase">見積情報データテーブル行</param>
    ''' <returns>処理結果（成功[True]/失敗[False]）</returns>
    ''' <remarks></remarks>
    Public Function InsEstCustomerInfoDataTable(ByVal estimateId As Long, _
                                                ByVal dr As IC3070202DataSet.IC3070202EstCustomerInfoRow, _
                                                ByVal drBase As IC3070202DataSet.IC3070202EstimationInfoRow) As Boolean
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsEstCustomerInfoDataTable_Start")
        'ログ出力 End *****************************************************************************

        Try
            ' データ行のNullチェック
            If dr Is Nothing Then
                Throw New ArgumentNullException("dr")
            End If

            If drBase Is Nothing Then
                Throw New ArgumentNullException("drBase")
            End If

            ' SQL組み立て
            Dim sql As New StringBuilder

            With sql
                .Append("INSERT /* IC3070202_014 */ ")
                .Append("INTO ")
                .Append("    TBL_EST_CUSTOMERINFO ")
                .Append("( ")
                .Append("    ESTIMATEID ")          '見積管理ID
                .Append("  , CONTRACTCUSTTYPE ")    '契約顧客種別
                .Append("  , CUSTPART ")            '顧客区分
                .Append("  , NAME ")                '氏名
                .Append("  , SOCIALID ")            '国民番号
                .Append("  , ZIPCODE ")             '郵便番号
                .Append("  , ADDRESS ")             '住所
                .Append("  , TELNO ")               '電話番号
                .Append("  , MOBILE ")              '携帯電話番号
                .Append("  , FAXNO ")               'FAX番号
                .Append("  , EMAIL ")               'e-MAILアドレス
                .Append("  , CREATEDATE ")          '作成日
                .Append("  , UPDATEDATE ")          '更新日
                .Append("  , CREATEACCOUNT ")       '作成ユーザアカウント
                .Append("  , UPDATEACCOUNT ")       '更新ユーザアカウント
                .Append("  , CREATEID ")            '作成機能ID
                .Append("  , UPDATEID ")            '更新機能ID
                .Append("  , PRIVATE_FLEET_ITEM_CD ")   '個人法人項目コード
                .Append("  , NAMETITLE_CD ")            '敬称コード
                .Append("  , NAMETITLE_NAME ")          '敬称
                .Append("  , FIRST_NAME ")              'ファーストネーム
                .Append("  , MIDDLE_NAME ")             'ミドルネーム
                .Append("  , LAST_NAME ")               'ラストネーム
                .Append("  , CST_ADDRESS_1 ")           '顧客住所1
                .Append("  , CST_ADDRESS_2 ")           '顧客住所2 
                .Append("  , CST_ADDRESS_3 ")           '顧客住所3 
                .Append("  , CST_ADDRESS_STATE ")       '顧客住所（州）
                .Append("  , CST_ADDRESS_DISTRICT ")    '顧客住所（地区）
                .Append("  , CST_ADDRESS_CITY ")        '顧客住所（市）
                .Append("  , CST_ADDRESS_LOCATION ")    '顧客住所（地域）
                .Append(") ")
                .Append("VALUES ")
                .Append("( ")
                .Append("    :ESTIMATEID ")          '見積管理ID
                .Append("  , :CONTRACTCUSTTYPE ")    '契約顧客種別
                .Append("  , :CUSTPART ")            '顧客区分
                .Append("  , :NAME ")                '氏名
                .Append("  , :SOCIALID ")            '国民番号
                .Append("  , :ZIPCODE ")             '郵便番号
                .Append("  , :ADDRESS ")             '住所
                .Append("  , :TELNO ")               '電話番号
                .Append("  , :MOBILE ")              '携帯電話番号
                .Append("  , :FAXNO ")               'FAX番号
                .Append("  , :EMAIL ")               'e-MAILアドレス
                .Append("  , SYSDATE ")              '作成日
                .Append("  , SYSDATE ")              '更新日
                .Append("  , :CREATEACCOUNT ")       '作成ユーザアカウント
                .Append("  , :UPDATEACCOUNT ")       '更新ユーザアカウント
                .Append("  , :CREATEID ")            '作成機能ID
                .Append("  , :UPDATEID ")            '更新機能ID
                .Append("  , :PRIVATE_FLEET_ITEM_CD ")   '個人法人項目コード
                .Append("  , :NAMETITLE_CD ")            '敬称コード
                .Append("  , :NAMETITLE_NAME ")          '敬称
                .Append("  , :FIRST_NAME ")              'ファーストネーム
                .Append("  , :MIDDLE_NAME ")             'ミドルネーム
                .Append("  , :LAST_NAME ")               'ラストネーム
                .Append("  , :CST_ADDRESS_1 ")           '顧客住所1
                .Append("  , :CST_ADDRESS_2 ")           '顧客住所2 
                .Append("  , :CST_ADDRESS_3 ")           '顧客住所3 
                .Append("  , :CST_ADDRESS_STATE ")       '顧客住所（州）
                .Append("  , :CST_ADDRESS_DISTRICT ")    '顧客住所（地区）
                .Append("  , :CST_ADDRESS_CITY ")        '顧客住所（市）
                .Append("  , :CST_ADDRESS_LOCATION ")    '顧客住所（地域）
                .Append(") ")
            End With

            ' DbUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("IC3070202_014")

                query.CommandText = sql.ToString()

                ' SQLパラメータ設定
                query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Long, estimateId)                           '見積管理ID
                query.AddParameterWithTypeValue("CONTRACTCUSTTYPE", OracleDbType.Char, dr.CONTRACTCUSTTYPE)            '契約顧客種別
                '顧客区分
                If dr.IsCUSTPARTNull Then
                    query.AddParameterWithTypeValue("CUSTPART", OracleDbType.Char, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("CUSTPART", OracleDbType.Char, dr.CUSTPART)
                End If
                '氏名
                If dr.IsNAMENull Then
                    query.AddParameterWithTypeValue("NAME", OracleDbType.NVarchar2, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("NAME", OracleDbType.NVarchar2, dr.NAME)
                End If
                '国民番号
                If dr.IsSOCIALIDNull Then
                    query.AddParameterWithTypeValue("SOCIALID", OracleDbType.NVarchar2, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("SOCIALID", OracleDbType.NVarchar2, dr.SOCIALID)
                End If
                '郵便番号
                If dr.IsZIPCODENull Then
                    query.AddParameterWithTypeValue("ZIPCODE", OracleDbType.NVarchar2, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("ZIPCODE", OracleDbType.NVarchar2, dr.ZIPCODE)
                End If
                '住所
                If dr.IsADDRESSNull Then
                    query.AddParameterWithTypeValue("ADDRESS", OracleDbType.NVarchar2, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("ADDRESS", OracleDbType.NVarchar2, dr.ADDRESS)
                End If
                '電話番号
                If dr.IsTELNONull Then
                    query.AddParameterWithTypeValue("TELNO", OracleDbType.NVarchar2, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("TELNO", OracleDbType.NVarchar2, dr.TELNO)
                End If
                '携帯電話番号
                If dr.IsMOBILENull Then
                    query.AddParameterWithTypeValue("MOBILE", OracleDbType.NVarchar2, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("MOBILE", OracleDbType.NVarchar2, dr.MOBILE)
                End If
                'FAX番号
                If dr.IsFAXNONull Then
                    query.AddParameterWithTypeValue("FAXNO", OracleDbType.NVarchar2, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("FAXNO", OracleDbType.NVarchar2, dr.FAXNO)
                End If
                'e-MAILアドレス
                If dr.IsEMAILNull Then
                    query.AddParameterWithTypeValue("EMAIL", OracleDbType.NVarchar2, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("EMAIL", OracleDbType.NVarchar2, dr.EMAIL)
                End If
                query.AddParameterWithTypeValue("CREATEACCOUNT", OracleDbType.Varchar2, drBase.UPDATEACCOUNT)   '作成ユーザアカウント
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, drBase.UPDATEACCOUNT)   '更新ユーザアカウント
                query.AddParameterWithTypeValue("CREATEID", OracleDbType.Varchar2, drBase.UPDATEID)             '作成機能ID
                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, drBase.UPDATEID)             '更新機能ID

                '個人法人項目コード
                If dr.IsPRIVATE_FLEET_ITEM_CDNull Then
                    query.AddParameterWithTypeValue("PRIVATE_FLEET_ITEM_CD", OracleDbType.NVarchar2, DEFAULT_STRING_VALUE)
                Else
                    query.AddParameterWithTypeValue("PRIVATE_FLEET_ITEM_CD", OracleDbType.NVarchar2, dr.PRIVATE_FLEET_ITEM_CD)
                End If
                '敬称コード
                If dr.IsNAMETITLE_CDNull Then
                    query.AddParameterWithTypeValue("NAMETITLE_CD", OracleDbType.NVarchar2, DEFAULT_STRING_VALUE)
                Else
                    query.AddParameterWithTypeValue("NAMETITLE_CD", OracleDbType.NVarchar2, dr.NAMETITLE_CD)
                End If
                '敬称
                If dr.IsNAMETITLE_NAMENull Then
                    query.AddParameterWithTypeValue("NAMETITLE_NAME", OracleDbType.NVarchar2, DEFAULT_STRING_VALUE)
                Else
                    query.AddParameterWithTypeValue("NAMETITLE_NAME", OracleDbType.NVarchar2, dr.NAMETITLE_NAME)
                End If
                'ファーストネーム
                If dr.IsFIRST_NAMENull Then
                    query.AddParameterWithTypeValue("FIRST_NAME", OracleDbType.NVarchar2, DEFAULT_STRING_VALUE)
                Else
                    query.AddParameterWithTypeValue("FIRST_NAME", OracleDbType.NVarchar2, dr.FIRST_NAME)
                End If
                'ミドルネーム
                If dr.IsMIDDLE_NAMENull Then
                    query.AddParameterWithTypeValue("MIDDLE_NAME", OracleDbType.NVarchar2, DEFAULT_STRING_VALUE)
                Else
                    query.AddParameterWithTypeValue("MIDDLE_NAME", OracleDbType.NVarchar2, dr.MIDDLE_NAME)
                End If
                'ラストネーム
                If dr.IsLAST_NAMENull Then
                    query.AddParameterWithTypeValue("LAST_NAME", OracleDbType.NVarchar2, DEFAULT_STRING_VALUE)
                Else
                    query.AddParameterWithTypeValue("LAST_NAME", OracleDbType.NVarchar2, dr.LAST_NAME)
                End If
                '顧客住所1
                If dr.IsCST_ADDRESS_1Null Then
                    query.AddParameterWithTypeValue("CST_ADDRESS_1", OracleDbType.NVarchar2, DEFAULT_STRING_VALUE)
                Else
                    query.AddParameterWithTypeValue("CST_ADDRESS_1", OracleDbType.NVarchar2, dr.CST_ADDRESS_1)
                End If
                '顧客住所2 
                If dr.IsCST_ADDRESS_2Null Then
                    query.AddParameterWithTypeValue("CST_ADDRESS_2", OracleDbType.NVarchar2, DEFAULT_STRING_VALUE)
                Else
                    query.AddParameterWithTypeValue("CST_ADDRESS_2", OracleDbType.NVarchar2, dr.CST_ADDRESS_2)
                End If
                '顧客住所3 
                If dr.IsCST_ADDRESS_3Null Then
                    query.AddParameterWithTypeValue("CST_ADDRESS_3", OracleDbType.NVarchar2, DEFAULT_STRING_VALUE)
                Else
                    query.AddParameterWithTypeValue("CST_ADDRESS_3", OracleDbType.NVarchar2, dr.CST_ADDRESS_3)

                End If
                '顧客住所（州）
                If dr.IsCST_ADDRESS_STATENull Then
                    query.AddParameterWithTypeValue("CST_ADDRESS_STATE", OracleDbType.NVarchar2, DEFAULT_STRING_VALUE)
                Else
                    query.AddParameterWithTypeValue("CST_ADDRESS_STATE", OracleDbType.NVarchar2, dr.CST_ADDRESS_STATE)

                End If
                '顧客住所（地区）
                If dr.IsCST_ADDRESS_DISTRICTNull Then
                    query.AddParameterWithTypeValue("CST_ADDRESS_DISTRICT", OracleDbType.NVarchar2, DEFAULT_STRING_VALUE)
                Else
                    query.AddParameterWithTypeValue("CST_ADDRESS_DISTRICT", OracleDbType.NVarchar2, dr.CST_ADDRESS_DISTRICT)

                End If
                '顧客住所（市）
                If dr.IsCST_ADDRESS_CITYNull Then
                    query.AddParameterWithTypeValue("CST_ADDRESS_CITY", OracleDbType.NVarchar2, DEFAULT_STRING_VALUE)
                Else
                    query.AddParameterWithTypeValue("CST_ADDRESS_CITY", OracleDbType.NVarchar2, dr.CST_ADDRESS_CITY)

                End If
                '顧客住所（地域）
                If dr.IsCST_ADDRESS_LOCATIONNull Then
                    query.AddParameterWithTypeValue("CST_ADDRESS_LOCATION", OracleDbType.NVarchar2, DEFAULT_STRING_VALUE)
                Else
                    query.AddParameterWithTypeValue("CST_ADDRESS_LOCATION", OracleDbType.NVarchar2, dr.CST_ADDRESS_LOCATION)

                End If

                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsEstCustomerInfoDataTable_End")
                'ログ出力 End *****************************************************************************
                ' SQL実行（結果を返却）
                If query.Execute() > 0 Then
                    Return True
                Else
                    Return False
                End If
            End Using
        Catch ex As Exception
            If Me.prpUpdDvs = UpdateDvsRegist Then
                Me.prpResultId = ErrCodeDBOverlap + TblCodeEstCustomerInfo
            Else
                Me.prpResultId = ErrCodeDBUpdate + TblCodeEstCustomerInfo
            End If
            Throw
        End Try
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

    End Function

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 見積諸費用情報を挿入します。
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <param name="dr">見積諸費用情報データテーブル行</param>
    ''' <param name="drBase">見積情報データテーブル行</param>
    ''' <returns>処理結果（成功[True]/失敗[False]）</returns>
    ''' <remarks></remarks>
    Public Function InsEstChargeInfoDataTable(ByVal estimateId As Long, _
                                              ByVal dr As IC3070202DataSet.IC3070202EstChargeInfoRow, _
                                              ByVal drBase As IC3070202DataSet.IC3070202EstimationInfoRow) As Boolean
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsEstChargeInfoDataTable_Start")
        'ログ出力 End *****************************************************************************

        Try
            ' データ行のNullチェック
            If dr Is Nothing Then
                Throw New ArgumentNullException("dr")
            End If

            If drBase Is Nothing Then
                Throw New ArgumentNullException("drBase")
            End If

            ' SQL組み立て
            Dim sql As New StringBuilder

            With sql
                .Append("INSERT /* IC3070202_015 */ ")
                .Append("INTO ")
                .Append("    TBL_EST_CHARGEINFO ")
                .Append("( ")
                .Append("    ESTIMATEID ")          '見積管理ID
                .Append("  , ITEMCODE ")            '費用項目コード
                .Append("  , ITEMNAME ")            '費用項目名
                .Append("  , PRICE ")               '価格
                .Append("  , CREATEDATE ")          '作成日
                .Append("  , UPDATEDATE ")          '更新日
                .Append("  , CREATEACCOUNT ")       '作成ユーザアカウント
                .Append("  , UPDATEACCOUNT ")       '更新ユーザアカウント
                .Append("  , CREATEID ")            '作成機能ID
                .Append("  , UPDATEID ")            '更新機能ID
                .Append("  , CHARGEDVS ")           '諸費用区分
                .Append(") ")
                .Append("VALUES ")
                .Append("( ")
                .Append("    :ESTIMATEID ")          '見積管理ID
                .Append("  , :ITEMCODE ")            '費用項目コード
                .Append("  , :ITEMNAME ")            '費用項目名
                .Append("  , :PRICE ")               '価格
                .Append("  , SYSDATE ")              '作成日
                .Append("  , SYSDATE ")              '更新日
                .Append("  , :CREATEACCOUNT ")       '作成ユーザアカウント
                .Append("  , :UPDATEACCOUNT ")       '更新ユーザアカウント
                .Append("  , :CREATEID ")            '作成機能ID
                .Append("  , :UPDATEID ")            '更新機能ID
                .Append("  , :CHARGEDVS ")           '諸費用区分
                .Append(") ")
            End With

            ' DbUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("IC3070202_015")

                query.CommandText = sql.ToString()

                ' SQLパラメータ設定
                query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Long, estimateId)                           '見積管理ID
                query.AddParameterWithTypeValue("ITEMCODE", OracleDbType.Varchar2, dr.ITEMCODE)                        '費用項目コード
                query.AddParameterWithTypeValue("ITEMNAME", OracleDbType.NVarchar2, dr.ITEMNAME)                       '費用項目名
                '価格
                If dr.IsPRICENull Then
                    query.AddParameterWithTypeValue("PRICE", OracleDbType.Double, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("PRICE", OracleDbType.Double, dr.PRICE)
                End If
                query.AddParameterWithTypeValue("CREATEACCOUNT", OracleDbType.Varchar2, drBase.UPDATEACCOUNT)          '作成ユーザアカウント
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, drBase.UPDATEACCOUNT)          '更新ユーザアカウント
                query.AddParameterWithTypeValue("CREATEID", OracleDbType.Varchar2, drBase.UPDATEID)                    '作成機能ID
                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, drBase.UPDATEID)                    '更新機能ID
                query.AddParameterWithTypeValue("CHARGEDVS", OracleDbType.Char, dr.CHARGEDVS)                          '諸費用区分

                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsEstChargeInfoDataTable_End")
                'ログ出力 End *****************************************************************************
                ' SQL実行（結果を返却）
                If query.Execute() > 0 Then
                    Return True
                Else
                    Return False
                End If
            End Using
        Catch ex As Exception
            If Me.prpUpdDvs = UpdateDvsRegist Then
                Me.prpResultId = ErrCodeDBOverlap + TblCodeEstChargeInfo
            Else
                Me.prpResultId = ErrCodeDBUpdate + TblCodeEstChargeInfo
            End If
            Throw
        End Try
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

    End Function

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 見積下取車両情報を挿入します。
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <param name="dr">見積下取車両情報データテーブル行</param>
    ''' <param name="drBase">見積情報データテーブル行</param>
    ''' <returns>処理結果（成功[True]/失敗[False]）</returns>
    ''' <remarks></remarks>
    Public Function InsEstTradeInCarInfoDataTable(ByVal estimateId As Long, _
                                                  ByVal dr As IC3070202DataSet.IC3070202EstTradeInCarInfoRow, _
                                                  ByVal drBase As IC3070202DataSet.IC3070202EstimationInfoRow) As Boolean
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsEstTradeInCarInfoDataTable_Start")
        'ログ出力 End *****************************************************************************

        Try
            ' データ行のNullチェック
            If dr Is Nothing Then
                Throw New ArgumentNullException("dr")
            End If

            If drBase Is Nothing Then
                Throw New ArgumentNullException("drBase")
            End If

            ' SQL組み立て
            Dim sql As New StringBuilder

            With sql
                .Append("INSERT /* IC3070202_016 */ ")
                .Append("INTO ")
                .Append("    TBL_EST_TRADEINCARINFO ")
                .Append("( ")
                .Append("    ESTIMATEID ")          '見積管理ID
                .Append("  , SEQNO ")               '連番
                .Append("  , ASSESSMENTNO ")        '査定No
                .Append("  , VEHICLENAME ")         '車名
                .Append("  , ASSESSEDPRICE ")       '提示価格
                .Append("  , CREATEDATE ")          '作成日
                .Append("  , UPDATEDATE ")          '更新日
                .Append("  , CREATEACCOUNT ")       '作成ユーザアカウント
                .Append("  , UPDATEACCOUNT ")       '更新ユーザアカウント
                .Append("  , CREATEID ")            '作成機能ID
                .Append("  , UPDATEID ")            '更新機能ID
                .Append(") ")
                .Append("VALUES ")
                .Append("( ")
                .Append("    :ESTIMATEID ")          '見積管理ID
                .Append("  , :SEQNO ")               '連番
                .Append("  , :ASSESSMENTNO ")        '査定No
                .Append("  , :VEHICLENAME ")         '車名
                .Append("  , :ASSESSEDPRICE ")       '提示価格
                .Append("  , SYSDATE ")              '作成日
                .Append("  , SYSDATE ")              '更新日
                .Append("  , :CREATEACCOUNT ")       '作成ユーザアカウント
                .Append("  , :UPDATEACCOUNT ")       '更新ユーザアカウント
                .Append("  , :CREATEID ")            '作成機能ID
                .Append("  , :UPDATEID ")            '更新機能ID
                .Append(") ")
            End With

            ' DbUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("IC3070202_016")

                query.CommandText = sql.ToString()

                ' SQLパラメータ設定
                query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Long, estimateId)                           '見積管理ID
                query.AddParameterWithTypeValue("SEQNO", OracleDbType.Int64, dr.SEQNO)                                 '連番
                '査定No
                If dr.IsASSESSMENTNONull Then
                    query.AddParameterWithTypeValue("ASSESSMENTNO", OracleDbType.Int64, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("ASSESSMENTNO", OracleDbType.Int64, dr.ASSESSMENTNO)
                End If
                query.AddParameterWithTypeValue("VEHICLENAME", OracleDbType.NVarchar2, dr.VEHICLENAME)                 '車名
                query.AddParameterWithTypeValue("ASSESSEDPRICE", OracleDbType.Double, dr.ASSESSEDPRICE)                '提示価格
                query.AddParameterWithTypeValue("CREATEACCOUNT", OracleDbType.Varchar2, drBase.UPDATEACCOUNT)          '作成ユーザアカウント
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, drBase.UPDATEACCOUNT)          '更新ユーザアカウント
                query.AddParameterWithTypeValue("CREATEID", OracleDbType.Varchar2, drBase.UPDATEID)                    '作成機能ID
                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, drBase.UPDATEID)                    '更新機能ID

                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsEstTradeInCarInfoDataTable_End")
                'ログ出力 End *****************************************************************************
                ' SQL実行（結果を返却）
                If query.Execute() > 0 Then
                    Return True
                Else
                    Return False
                End If
            End Using
        Catch ex As Exception
            If Me.prpUpdDvs = UpdateDvsRegist Then
                Me.prpResultId = ErrCodeDBOverlap + TblCodeEstTradeInCarInfo
            Else
                Me.prpResultId = ErrCodeDBUpdate + TblCodeEstTradeInCarInfo
            End If
            Throw
        End Try
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

    End Function

    '2017/11/20 TCS 河原 TKM独自機能開発 START
    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 希望車種情報を挿入します。
    ''' </summary>
    ''' <param name="estimateInfoRow">見積情報</param>
    ''' <param name="seqno">希望車連番</param>
    ''' <param name="seriescd">モデルコード</param> 
    ''' <param name="mostPerfcd">希望車の商談見込み度コード</param> 
    ''' <param name="salesHisFlg">商談Histroyフラグ</param> 
    ''' <returns>処理結果（成功[True]/失敗[False]）</returns>
    ''' <remarks></remarks>
    Public Function InsPriferdModel(ByVal estimateInfoRow As IC3070202DataSet.IC3070202EstimationInfoRow,
                                    ByVal seqno As Long,
                                    ByVal seriescd As String,
                                    ByVal mostPerfcd As String,
                                    ByVal salesHisFlg As Boolean) As Boolean
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsPriferdModel_Start")
        'ログ出力 End *****************************************************************************
        Try

            ' 2014/03/07 TCS 各務 再構築不具合対応マージ版 START
            Dim env As New SystemEnvSetting
            ' 外版色コードを前3桁だけで比較するか否かフラグ
            Dim extColor3Flg As String = String.Empty
            Dim sysEnvRow As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow
            sysEnvRow = env.GetSystemEnvSetting(EXTERIOR_COLOR_3_FLG)
            If IsNothing(sysEnvRow) Then
                '取得できなかった場合、"0"を設定
                extColor3Flg = "0"
            Else
                extColor3Flg = sysEnvRow.PARAMVALUE
            End If
            ' 2014/03/07 TCS 各務 再構築不具合対応マージ版 END

            ' SQL組み立て
            Dim sql As New StringBuilder
            With sql
                .Append("INSERT ")
                .Append("    /* IC3070202_203 */ ")
                ' 2013/12/12 TCS 森 Aカード情報相互連携開発 START
                If (salesHisFlg = True) Then
                    .Append("    INTO TB_H_PREFER_VCL ( ")
                Else
                    .Append("    INTO TB_T_PREFER_VCL ( ")
                End If
                ' 2013/12/12 TCS 森 Aカード情報相互連携開発 END
                .Append("    SALES_ID , ")
                .Append("    PREF_VCL_SEQ , ")
                .Append("    SALES_STATUS , ")
                .Append("    MODEL_CD , ")
                .Append("    GRADE_CD , ")
                .Append("    SUFFIX_CD , ")
                .Append("    BODYCLR_CD , ")
                .Append("    INTERIORCLR_CD , ")
                .Append("    PREF_AMOUNT , ")
                .Append("    EST_PREF_DATE , ")                 '見積希望日
                .Append("    EST_RSLT_DATE , ")                 '見積実施日  
                .Append("    EST_RSLT_CONTACT_MTD , ")
                .Append("    EST_AMOUNT , ")
                .Append("    EST_RSLT_FLG , ")
                .Append("    EST_RSLT_STF_CD , ")
                .Append("    EST_RSLT_DEPT_ID , ")
                .Append("    SALESBKG_ACT_ID , ")
                .Append("    SALESBKG_NUM , ")
                .Append("    DMS_TAKEIN_DATETIME , ")           '基幹取込日時
                .Append("    ROW_CREATE_DATETIME , ")
                .Append("    ROW_CREATE_ACCOUNT , ")
                .Append("    ROW_CREATE_FUNCTION , ")
                .Append("    ROW_UPDATE_DATETIME , ")
                .Append("    ROW_UPDATE_ACCOUNT , ")
                .Append("    ROW_UPDATE_FUNCTION , ")
                .Append("    ROW_LOCK_VERSION, ")
                ' 2013/12/12 TCS 森 Aカード情報相互連携開発 START
                .Append("    SALES_PROSPECT_CD ")
                ' 2013/12/12 TCS 森 Aカード情報相互連携開発 END
                .Append(") ")
                .Append("VALUES ( ")
                .Append("    :FLLWUPBOX_SEQNO , ")
                .Append("    :SEQNO , ")
                .Append("    '21' , ")
                .Append("    :SERIESCD , ")
                .Append("    :MODELCD , ")
                .Append("    :SUFFIXCD , ")
                .Append("    :COLORCD , ")
                .Append("    :INTCOLORCD , ")
                .Append("    1 , ")
                .Append("    :DEFAULTDATE , ")
                .Append("    :DEFAULTDATE , ")
                ' 2014/04/01 TCS 松月 TMT不具合対応 Modify Start
                .Append("    ' ' , ")
                .Append("    0 , ")
                .Append("    '0' , ")
                ' 2014/04/01 TCS 松月 TMT不具合対応 Modify End
                .Append("    ' ' , ")
                .Append("    0 , ")
                .Append("    0 , ")
                ' 2014/04/01 TCS 松月 TMT不具合対応 Modify Start
                .Append("    ' ' , ")
                ' 2014/04/01 TCS 松月 TMT不具合対応 Modify End
                .Append("    :DEFAULTDATE , ")
                .Append("    SYSDATE , ")
                .Append("    :ACCOUNT , ")
                .Append("    'IC3070202', ")
                .Append("    SYSDATE , ")
                .Append("    :ACCOUNT , ")
                .Append("    'IC3070202', ")
                .Append("    0, ")
                ' 2013/12/12 TCS 森 Aカード情報相互連携開発 START
                .Append("    :SALES_PROSPECT_CD ")
                ' 2013/12/12 TCS 森 Aカード情報相互連携開発 END
                .Append(") ")
            End With
            ' DbUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("IC3070202_203")

                query.CommandText = sql.ToString()

                ' SQLパラメータ設定
                query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, estimateInfoRow.FLLWUPBOX_SEQNO)  '商談ID
                query.AddParameterWithTypeValue("SEQNO", OracleDbType.Long, seqno)                                      '希望車連番
                query.AddParameterWithTypeValue("SERIESCD", OracleDbType.NVarchar2, seriescd)                      'モデルコード
                query.AddParameterWithTypeValue("MODELCD", OracleDbType.NVarchar2, estimateInfoRow.MODELCD)              'グレードコード
                ' 2014/03/07 TCS 各務 再構築不具合対応マージ版 START
                'query.AddParameterWithTypeValue("COLORCD", OracleDbType.NVarchar2, Left(estimateInfoRow.EXTCOLORCD, 3))           '外鈑色コード
                If (extColor3Flg = "1") Then
                    query.AddParameterWithTypeValue("COLORCD", OracleDbType.NVarchar2, Left(estimateInfoRow.EXTCOLORCD, 3))        '外鈑色コード(前3桁)
                Else
                    query.AddParameterWithTypeValue("COLORCD", OracleDbType.NVarchar2, estimateInfoRow.EXTCOLORCD)        '外鈑色コード(全部)
                End If
                ' 2014/03/07 TCS 各務 再構築不具合対応マージ版 END
                query.AddParameterWithTypeValue("SUFFIXCD", OracleDbType.Varchar2, estimateInfoRow.SUFFIXCD)                        'サフィックス
                query.AddParameterWithTypeValue("INTCOLORCD", OracleDbType.Varchar2, estimateInfoRow.INTCOLORCD)                    '内装色コード
                query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, estimateInfoRow.UPDATEACCOUNT)  '更新アカウントと作成アカウント
                query.AddParameterWithTypeValue("DEFAULTDATE", OracleDbType.Date, Date.ParseExact("1900/01/01 00:00:00", "yyyy/MM/dd HH:mm:ss", Nothing))
                ' 2013/12/12 TCS 森 Aカード情報相互連携開発 START
                query.AddParameterWithTypeValue("SALES_PROSPECT_CD", OracleDbType.NVarchar2, mostPerfcd)           '商談見込み度コード
                ' 2013/12/12 TCS 森 Aカード情報相互連携開発 END

                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsPriferdModel_End")
                'ログ出力 End *****************************************************************************
                ' SQL実行（結果を返却）
                If query.Execute() > 0 Then
                    Return True
                Else
                    Return False
                End If
            End Using
        Catch ex As Exception
            If Me.prpUpdDvs = UpdateDvsRegist Then
                Me.prpResultId = ErrCodeDBOverlap + TblCodeFllwupboxSelectedSeries
            Else
                Me.prpResultId = ErrCodeDBUpdate + TblCodeFllwupboxSelectedSeries
            End If
            Throw
        End Try
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

    End Function
    '2017/11/20 TCS 河原 TKM独自機能開発 END

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START 
    ''' <summary>
    ''' 特定車種の登録
    ''' </summary>
    ''' <param name="estimateId">見積ID</param>
    ''' <param name="estimateInfoRow">見積情報</param>
    ''' <param name="createdate">作成日時</param>
    ''' <param name="carNameCD">モデルコード</param>
    ''' <returns>処理結果（成功[True]/失敗[False]）</returns>
    ''' <remarks></remarks>
    Public Function InsCamry(ByVal estimateId As Long,
                             ByVal estimateInfoRow As IC3070202DataSet.IC3070202EstimationInfoRow,
                             ByVal createdate As Date, ByVal carNameCD As String) As Boolean
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("estimateInfoRow_Start")
        'ログ出力 End *****************************************************************************

        Dim sql As New StringBuilder

        With sql
            .Append("INSERT ")
            .Append("    /* IC3070202_201 */ ")
            .Append("INTO TBL_EST_VCLINFO ( ")
            .Append("    ESTIMATEID , ")
            .Append("    SERIESCD , ")
            .Append("    SERIESNM , ")
            .Append("    MODELCD , ")
            .Append("    MODELNM , ")
            .Append("    BODYTYPE , ")
            .Append("    DRIVESYSTEM , ")
            .Append("    DISPLACEMENT , ")
            .Append("    TRANSMISSION , ")
            .Append("    SUFFIXCD , ")
            .Append("    EXTCOLORCD , ")
            .Append("    EXTCOLOR , ")
            .Append("    EXTAMOUNT , ")
            .Append("    INTCOLORCD , ")
            .Append("    INTCOLOR , ")
            .Append("    INTAMOUNT , ")
            .Append("    MODELNUMBER , ")
            .Append("    BASEPRICE , ")
            .Append("    CREATEDATE , ")
            .Append("    UPDATEDATE , ")
            .Append("    CREATEACCOUNT , ")
            .Append("    UPDATEACCOUNT , ")
            .Append("    CREATEID , ")
            .Append("    UPDATEID ")
            .Append(") ")
            .Append("VALUES ")
            .Append("  ( ")
            .Append("    :ESTIMATEID , ")
            .Append("    :SERIESCD  , ")
            .Append("      :SERIESNM , ")
            .Append("      :MODELCD , ")
            .Append("      :MODELNM , ")
            .Append("      :BODYTYPE , ")
            .Append("      :DRIVESYSTEM , ")
            .Append("      :DISPLACEMENT , ")
            .Append("      :TRANSMISSION , ")
            .Append("      :SUFFIXCD , ")
            .Append("      :EXTCOLORCD , ")
            .Append("        :EXTCOLOR , ")
            .Append("        :EXTAMOUNT , ")
            .Append("        :INTCOLORCD , ")
            .Append("        :INTCOLOR , ")
            .Append("        :INTAMOUNT , ")
            .Append("        :MODELNUMBER , ")
            .Append("        :BASEPRICE , ")
            .Append("        :CREATEDATE , ")
            .Append("        SYSDATE , ")
            .Append("        :CREATEACCOUNT , ")
            .Append("        :UPDATEACCOUNT , ")
            .Append("        'IC3070202' , ")
            .Append("        'IC3070202' ")
            .Append("  ) ")
        End With

        Using query As New DBUpdateQuery("IC3070202_201")

            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Long, estimateId)
            query.AddParameterWithTypeValue("SERIESNM", OracleDbType.NVarchar2, estimateInfoRow.SERIESNM)
            query.AddParameterWithTypeValue("MODELCD", OracleDbType.NVarchar2, estimateInfoRow.MODELCD)
            query.AddParameterWithTypeValue("MODELNM", OracleDbType.NVarchar2, estimateInfoRow.MODELNM)
            query.AddParameterWithTypeValue("BODYTYPE", OracleDbType.NVarchar2, estimateInfoRow.BODYTYPE)
            query.AddParameterWithTypeValue("DRIVESYSTEM", OracleDbType.NVarchar2, estimateInfoRow.DRIVESYSTEM)
            query.AddParameterWithTypeValue("DISPLACEMENT", OracleDbType.NVarchar2, estimateInfoRow.DISPLACEMENT)
            query.AddParameterWithTypeValue("TRANSMISSION", OracleDbType.NVarchar2, estimateInfoRow.TRANSMISSION)
            query.AddParameterWithTypeValue("SUFFIXCD", OracleDbType.Varchar2, estimateInfoRow.SUFFIXCD)
            query.AddParameterWithTypeValue("EXTCOLORCD", OracleDbType.Varchar2, estimateInfoRow.EXTCOLORCD)
            query.AddParameterWithTypeValue("EXTCOLOR", OracleDbType.NVarchar2, estimateInfoRow.EXTCOLOR)
            query.AddParameterWithTypeValue("EXTAMOUNT", OracleDbType.Long, estimateInfoRow.EXTAMOUNT)
            query.AddParameterWithTypeValue("INTCOLORCD", OracleDbType.Varchar2, estimateInfoRow.INTCOLORCD)
            query.AddParameterWithTypeValue("INTCOLOR", OracleDbType.NVarchar2, estimateInfoRow.INTCOLOR)
            query.AddParameterWithTypeValue("INTAMOUNT", OracleDbType.Long, estimateInfoRow.INTAMOUNT)
            query.AddParameterWithTypeValue("MODELNUMBER", OracleDbType.NVarchar2, estimateInfoRow.MODELNUMBER)
            query.AddParameterWithTypeValue("BASEPRICE", OracleDbType.Long, estimateInfoRow.BASEPRICE)
            query.AddParameterWithTypeValue("CREATEDATE", OracleDbType.Date, createdate)
            query.AddParameterWithTypeValue("CREATEACCOUNT", OracleDbType.Varchar2, estimateInfoRow.CREATEACCOUNT)
            query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, estimateInfoRow.UPDATEACCOUNT)
            query.AddParameterWithTypeValue("SERIESCD", OracleDbType.NVarchar2, carNameCD)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("estimateInfoRow_End")
            'ログ出力 End *****************************************************************************

            Return query.Execute()

        End Using
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END 

    End Function

#End Region

#Region "更新クエリ"

    ''' <summary>
    ''' 顧客区分（２：法人)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STR_CUSTPART_HOJIN As String = "2"

    ''' <summary>
    ''' 顧客マスタを見積顧客情報の内容と同期させます。
    ''' </summary>
    ''' <param name="estCustomerDataRow">見積顧客情報データテーブル行</param>
    ''' <param name="estEstimationRow">見積情報データテーブル行</param>
    ''' <returns>処理結果（成功[True]/失敗[False]）</returns>
    ''' <remarks></remarks>
    Public Function SyncCustomerInfo(ByVal estCustomerDataRow As DataRow, _
                                     ByVal estEstimationRow As IC3070202DataSet.IC3070202EstimationInfoRow) As Boolean

        Dim customerId As Long = 0
        If (Long.TryParse(estEstimationRow.CRCUSTID, customerId) = False OrElse customerId = 0) Then
            Return False
        End If

        Try
            ' SQL組み立て
            Dim sql As New StringBuilder


            ' DbUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("IC3070202_213")

                With sql
                    .Append("UPDATE /* IC3070202_213 */ ")
                    .Append("    TB_M_CUSTOMER ")
                    .Append("SET ")
                    .Append("    FLEET_FLG = :FLEET_FLG,")
                    .Append("    CST_SOCIALNUM = :CST_SOCIALNUM,")
                    .Append("    CST_NAME = :CST_NAME,")
                    .Append("    CST_ADDRESS = :CST_ADDRESS,")
                    .Append("    CST_ZIPCD = :CST_ZIPCD,")
                    .Append("    CST_PHONE = :CST_PHONE,")
                    .Append("    CST_MOBILE = :CST_MOBILE,")
                    .Append("    CST_EMAIL_1 = :CST_EMAIL_1,")
                    .Append("    ROW_UPDATE_DATETIME = SYSDATE,")
                    .Append("    ROW_UPDATE_ACCOUNT = :ROW_UPDATE_ACCOUNT,")
                    .Append("    ROW_UPDATE_FUNCTION = :ROW_UPDATE_FUNCTION,")
                    .Append("    ROW_LOCK_VERSION = ROW_LOCK_VERSION + 1 ")
                    .Append("WHERE ")
                    .Append("    CST_ID = :CST_ID ")
                End With
                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("CST_ID", OracleDbType.Long, customerId)

                ' 法人フラグ （0:個人 1:法人）
                If String.Equals(estCustomerDataRow.Item("CUSTPART"), STR_CUSTPART_HOJIN) Then
                    '法人
                    query.AddParameterWithTypeValue("FLEET_FLG", OracleDbType.NVarchar2, "1")
                Else
                    '個人
                    query.AddParameterWithTypeValue("FLEET_FLG", OracleDbType.NVarchar2, "0")
                End If
                query.AddParameterWithTypeValue("CST_SOCIALNUM", OracleDbType.NVarchar2, GetDbStringParameter(estCustomerDataRow("SOCIALID")))
                query.AddParameterWithTypeValue("CST_NAME", OracleDbType.NVarchar2, GetDbStringParameter(estCustomerDataRow("NAME")))
                query.AddParameterWithTypeValue("CST_ADDRESS", OracleDbType.NVarchar2, GetDbStringParameter(estCustomerDataRow("ADDRESS")))
                query.AddParameterWithTypeValue("CST_ZIPCD", OracleDbType.NVarchar2, GetDbStringParameter(estCustomerDataRow("ZIPCODE")))
                query.AddParameterWithTypeValue("CST_PHONE", OracleDbType.NVarchar2, GetDbStringParameter(estCustomerDataRow("TELNO")))
                query.AddParameterWithTypeValue("CST_MOBILE", OracleDbType.NVarchar2, GetDbStringParameter(estCustomerDataRow("MOBILE")))
                query.AddParameterWithTypeValue("CST_EMAIL_1", OracleDbType.NVarchar2, GetDbStringParameter(estCustomerDataRow("EMAIL")))
                query.AddParameterWithTypeValue("ROW_UPDATE_FUNCTION", OracleDbType.NVarchar2, estEstimationRow.UPDATEID)
                query.AddParameterWithTypeValue("ROW_UPDATE_ACCOUNT", OracleDbType.NVarchar2, estEstimationRow.UPDATEACCOUNT)

                query.Execute()
            End Using
        Catch ex As Exception
            If Me.prpResultId = ErrCodeSuccess Then
                Me.prpResultId = ErrCodeDBUpdate + TblCodeEstCustomerInfo
            End If
            Throw
        End Try

        Return True

    End Function

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 見積情報を更新（論理削除）します。
    ''' </summary>
    ''' <param name="ESTIMATEID">見積管理ID</param>
    ''' <returns>処理結果（成功[True]/失敗[False]）</returns>
    ''' <remarks></remarks>
    Public Function UpdEstimateInfoDataTable(ByVal estimateId As Long) As Boolean
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdEstimateInfoDataTable_Start")
        'ログ出力 End *****************************************************************************

        Try
            ' SQL組み立て
            Dim sql As New StringBuilder

            With sql
                .Append("UPDATE /* IC3070202_017 */ ")
                .Append("    TBL_ESTIMATEINFO ")
                .Append("SET ")
                .Append("    DELFLG = '1' ")                '削除フラグ
                .Append("  , UPDATEDATE = SYSDATE ")        '更新日
                .Append("WHERE ")
                .Append("    ESTIMATEID = :ESTIMATEID ")
            End With

            ' DbUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("IC3070202_017")

                query.CommandText = sql.ToString()

                ' SQLパラメータ設定
                query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Long, estimateId)       '見積管理ID

                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdEstimateInfoDataTable_End")
                'ログ出力 End *****************************************************************************
                ' SQL実行（結果を返却）
                If query.Execute() > 0 Then
                    Return True
                Else
                    Me.prpResultId = ErrCodeDBNothing + TblCodeEstimateInfo
                    Throw New ArgumentException("", "estimateId")
                End If
            End Using
        Catch ex As Exception
            If Me.prpResultId = ErrCodeSuccess Then
                Me.prpResultId = ErrCodeDBUpdate + TblCodeEstimateInfo
            End If
            Throw
        End Try
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

    End Function

    '2012/03/02 TCS 劉【SALES_2】ADD START
    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    ''' <summary>
    ''' Follow-up Box選択車種情報を更新します。
    ''' </summary>
    ''' <param name="seqno">希望車連番</param>
    ''' <param name="estimateInfoRow">見積情報</param>
    ''' <param name="salesHisFlg">商談Histroyフラグ</param> 
    ''' <returns>処理結果（成功[True]/失敗[False]）</returns>
    ''' <remarks></remarks>
    Public Function UpPriferdModel(ByVal chgmode As Integer,
                                   ByVal seqno As Decimal,
                                   ByVal estimateInfoRow As IC3070202DataSet.IC3070202EstimationInfoRow,
                                   ByVal salesHisFlg As Boolean) As Boolean
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpPriferdModel_Start")
        'ログ出力 End *****************************************************************************

        Try
            ' 2014/03/07 TCS 各務 再構築不具合対応マージ版 START
            Dim env As New SystemEnvSetting
            ' 外版色コードを前3桁だけで比較するか否かフラグ
            Dim extColor3Flg As String = String.Empty
            Dim sysEnvRow As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow
            sysEnvRow = env.GetSystemEnvSetting(EXTERIOR_COLOR_3_FLG)
            If IsNothing(sysEnvRow) Then
                '取得できなかった場合、"0"を設定
                extColor3Flg = "0"
            Else
                extColor3Flg = sysEnvRow.PARAMVALUE
            End If
            ' 2014/03/07 TCS 各務 再構築不具合対応マージ版 END

            ' SQL組み立て
            Dim sql As New StringBuilder
            With sql

                If chgmode = 0 Then
                    .Append("UPDATE ")
                    .Append("    /* IC3070202_023 */ ")
                    If (salesHisFlg = True) Then
                        .Append("    TB_H_PREFER_VCL ")
                    Else
                        .Append("    TB_T_PREFER_VCL ")
                    End If
                    .Append("SET ")
                    .Append("    BODYCLR_CD = :COLORCD , ")
                    .Append("    ROW_UPDATE_DATETIME = SYSDATE , ")
                    .Append("    ROW_UPDATE_ACCOUNT = :UPDATEACCOUNT , ")
                    .Append("    ROW_LOCK_VERSION = :LOCKVR + 1 ")
                    .Append("WHERE ")
                    .Append("        SALES_ID = :FLLWUPBOX_SEQNO ")
                    .Append("    AND PREF_VCL_SEQ = :SEQNO ")
                    .Append("    AND MODEL_CD = SERIESCD ")
                    .Append("    AND GRADE_CD = :MODELCD ")
                    .Append("    AND ROW_LOCK_VERSION = :LOCKVR ")
                Else
                    .Append("UPDATE ")
                    .Append("    /* IC3070202_023 */ ")
                    If (salesHisFlg = True) Then
                        .Append("    TB_H_PREFER_VCL ")
                    Else
                        .Append("    TB_T_PREFER_VCL ")
                    End If
                    .Append("SET ")
                    .Append("    BODYCLR_CD = :COLORCD , ")
                    .Append("    ROW_UPDATE_DATETIME = SYSDATE , ")
                    .Append("    ROW_UPDATE_ACCOUNT = :UPDATEACCOUNT , ")
                    .Append("    ROW_LOCK_VERSION = :LOCKVR + 1 ")
                    .Append("WHERE ")
                    .Append("        SALES_ID = :FLLWUPBOX_SEQNO ")
                    .Append("    AND PREF_VCL_SEQ = :SEQNO ")
                    .Append("    AND MODEL_CD = ( ")
                    .Append("        SELECT ")
                    .Append("            CAR_NAME_CD_AI21 ")
                    .Append("        FROM ")
                    .Append("            ( ")
                    .Append("            SELECT ")
                    .Append("                CAR_NAME_CD_AI21 ")
                    .Append("            FROM ")
                    .Append("                TBL_MSTCARNAME ")
                    .Append("            WHERE ")
                    .Append("                    VCLSERIES_CD = :SERIESCD ")
                    .Append("                AND DELETE_FLAG IS NULL ")
                    .Append("            ORDER BY ")
                    .Append("                VCLCLASS_GENE DESC ")
                    .Append("            ) ")
                    .Append("        WHERE ")
                    .Append("            ROWNUM = 1 ")
                    .Append("                   ) ")
                    .Append("    AND GRADE_CD = :MODELCD ")
                    .Append("    AND ROW_LOCK_VERSION = :LOCKVR ")
                End If

            End With

            ' DbUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("IC3070202_023")

                query.CommandText = sql.ToString()

                ' SQLパラメータ設定
                ' 2014/03/07 TCS 各務 再構築不具合対応マージ版 START
                'query.AddParameterWithTypeValue("COLORCD", OracleDbType.NVarchar2, Left(estimateInfoRow.EXTCOLORCD, 3))             '外鈑色コード
                If (extColor3Flg = "1") Then
                    query.AddParameterWithTypeValue("COLORCD", OracleDbType.NVarchar2, Left(estimateInfoRow.EXTCOLORCD, 3))        '外鈑色コード(前3桁)
                Else
                    query.AddParameterWithTypeValue("COLORCD", OracleDbType.NVarchar2, estimateInfoRow.EXTCOLORCD)        '外鈑色コード(全部)
                End If
                ' 2014/03/07 TCS 各務 再構築不具合対応マージ版 END
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.NVarchar2, estimateInfoRow.UPDATEACCOUNT)    '更新ユーザアカウント
                query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, estimateInfoRow.FLLWUPBOX_SEQNO)    '商談ID
                query.AddParameterWithTypeValue("SEQNO", OracleDbType.Long, seqno)                                        '希望車連番
                query.AddParameterWithTypeValue("SERIESCD", OracleDbType.NVarchar2, estimateInfoRow.SERIESCD)             'モデルコード
                query.AddParameterWithTypeValue("MODELCD", OracleDbType.NVarchar2, estimateInfoRow.MODELCD)                'グレードコード
                query.AddParameterWithTypeValue("LOCKVR", OracleDbType.Int64, estimateInfoRow.ROWLOCKVERSION)                '

                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpPriferdModel_End")
                'ログ出力 End *****************************************************************************
                ' SQL実行（結果を返却）
                If query.Execute() > 0 Then
                    Return True
                Else
                    Me.prpResultId = ErrCodeDBNothing + TblCodeFllwupboxSelectedSeries
                    Throw New ArgumentException("", "fllwupbox_seqno")
                End If
            End Using
        Catch ex As Exception
            If Me.prpResultId = ErrCodeSuccess Then
                Me.prpResultId = ErrCodeDBUpdate + TblCodeFllwupboxSelectedSeries
            End If
            Throw
        End Try
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

    End Function
    '2012/03/02 TCS 劉【SALES_2】ADD END

    ' 2013/12/12 TCS 森 Aカード情報相互連携開発 START

    ''' <summary>
    ''' 一押し希望車種更新
    ''' </summary>
    ''' <param name="SalesId">商談ID</param>
    ''' <param name="UpdateAccount">更新ユーザーアカウント</param>
    ''' <param name="mostPerfcd">希望車の商談見込み度コード</param>
    ''' <param name="salesHisFlg">商談Histroyフラグ</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateMostPreferred(ByVal salesId As Decimal, _
                                        ByVal updateAccount As String, _
                                        ByVal mostPerfcd As String, _
                                        ByVal salesHisFlg As Boolean) As Integer

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateMostPreferred_Start")
        'ログ出力 End *****************************************************************************

        Try
            ' SQL組み立て
            Dim sql As New StringBuilder
            With sql

                .Append("UPDATE ")
                .Append("    /* IC3070202_212 */ ")
                ' 2013/12/12 TCS 森 Aカード情報相互連携開発 START
                If (salesHisFlg = True) Then
                    .Append("    TB_H_PREFER_VCL ")
                Else
                    .Append("    TB_T_PREFER_VCL ")
                End If
                ' 2013/12/12 TCS 森 Aカード情報相互連携開発 END
                .Append("SET ")
                ' 2013/12/12 TCS 森 Aカード情報相互連携開発 START
                '.Append("    SALES_PROSPECT_CD = '0' , ")
                .Append("    SALES_PROSPECT_CD = ' ' , ")
                ' 2013/12/12 TCS 森 Aカード情報相互連携開発 END
                .Append("    ROW_UPDATE_DATETIME = SYSDATE , ")
                .Append("    ROW_UPDATE_ACCOUNT = :UPDATEACCOUNT , ")
                .Append("    ROW_LOCK_VERSION = ROW_LOCK_VERSION + 1 ")
                .Append("WHERE ")
                .Append("        SALES_ID = :FLLWUPBOX_SEQNO ")
                ' 2013/12/12 TCS 森 Aカード情報相互連携開発 START
                '.Append(" AND SALES_PROSPECT_CD = '1'")
                .Append(" AND SALES_PROSPECT_CD = :SALES_PROSPECT_CD")
                ' 2013/12/12 TCS 森 Aカード情報相互連携開発 END

            End With

            ' DbUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("IC3070202_212")

                query.CommandText = sql.ToString()

                ' SQLパラメータ設定
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.NVarchar2, updateAccount)             '更新ユーザアカウント
                query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.NVarchar2, salesId)                 '商談ID
                ' 2013/12/12 TCS 森 Aカード情報相互連携開発 START
                query.AddParameterWithTypeValue("SALES_PROSPECT_CD", OracleDbType.NVarchar2, mostPerfcd)            '希望車の商談見込み度コード
                ' 2013/12/12 TCS 森 Aカード情報相互連携開発 END

                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateMostPreferred_End")
                'ログ出力 End *****************************************************************************
                ' SQL実行（結果を返却）
                Return query.Execute()
            End Using
        Catch ex As Exception
            If Me.prpResultId = ErrCodeSuccess Then
                Me.prpResultId = ErrCodeDBUpdate + TblCodeFllwupboxSelectedSeries
            End If
            Throw
        End Try

    End Function

    ''' <summary>
    ''' 見積情報に合わせて希望車種情報を更新
    ''' </summary>
    ''' <param name="seqno">希望車連番</param>
    ''' <param name="estimateInfoRow">見積情報</param>
    ''' <param name="salesHisFlg">商談Histroyフラグ</param>
    ''' <returns>処理結果(成功[True]/失敗[False])</returns>
    ''' <remarks></remarks>
    Public Function UpdatePreferVcl(ByVal mode As Integer,
                                    ByVal seqno As Decimal,
                                    ByVal estimateInfoRow As IC3070202DataSet.IC3070202EstimationInfoRow,
                                    ByVal salesHisFlg As Boolean) As Boolean

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdatePreferVcl_Start")
        'ログ出力 End *****************************************************************************

        Try
            Dim env As New SystemEnvSetting
            ' 外版色コードを前3桁だけで比較するか否かフラグ
            Dim extColor3Flg As String = String.Empty
            Dim sysEnvRow As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow
            sysEnvRow = env.GetSystemEnvSetting(EXTERIOR_COLOR_3_FLG)
            If IsNothing(sysEnvRow) Then
                '取得できなかった場合、"0"を設定
                extColor3Flg = "0"
            Else
                extColor3Flg = sysEnvRow.PARAMVALUE
            End If

            ' SQL組み立て
            Dim sql As New StringBuilder
            With sql
                .Append("UPDATE ")
                .Append("    /* IC3070202_024 */ ")
                If (salesHisFlg = True) Then
                    .Append("    TB_H_PREFER_VCL ")
                Else
                    .Append("    TB_T_PREFER_VCL ")
                End If
                .Append("SET ")

                Select Case mode
                    Case 1
                        'グレード以下を更新
                        .Append("    GRADE_CD = :GRADE_CD , ")
                        .Append("    SUFFIX_CD = :SUFFIX_CD , ")
                        .Append("    BODYCLR_CD = :BODYCLR_CD , ")
                        .Append("    INTERIORCLR_CD = :INTERIORCLR_CD , ")
                    Case 2
                        'サフィックス以下を更新
                        .Append("    SUFFIX_CD = :SUFFIX_CD , ")
                        .Append("    BODYCLR_CD = :BODYCLR_CD , ")
                        .Append("    INTERIORCLR_CD = :INTERIORCLR_CD , ")
                    Case 3
                        '外装色以下を更新
                        .Append("    BODYCLR_CD = :BODYCLR_CD , ")
                        .Append("    INTERIORCLR_CD = :INTERIORCLR_CD , ")
                    Case 4
                        '内装色以下を更新
                        .Append("    INTERIORCLR_CD = :INTERIORCLR_CD , ")
                End Select

                .Append("    ROW_UPDATE_DATETIME = SYSDATE , ")
                .Append("    ROW_UPDATE_ACCOUNT = :UPDATEACCOUNT , ")
                .Append("    ROW_LOCK_VERSION = ROW_LOCK_VERSION + 1 ")
                .Append("WHERE ")
                .Append("        SALES_ID = :FLLWUPBOX_SEQNO ")
                .Append("    AND PREF_VCL_SEQ = :SEQNO ")
            End With

            ' DbUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("IC3070202_024")
                query.CommandText = sql.ToString()

                'SQLパラメータ設定
                Select Case mode
                    Case 1
                        query.AddParameterWithTypeValue("GRADE_CD", OracleDbType.NVarchar2, estimateInfoRow.MODELCD)                    'グレード
                        query.AddParameterWithTypeValue("SUFFIX_CD", OracleDbType.NVarchar2, estimateInfoRow.SUFFIXCD)                  'サフィックス
                        If (extColor3Flg = "1") Then
                            query.AddParameterWithTypeValue("BODYCLR_CD", OracleDbType.NVarchar2, Left(estimateInfoRow.EXTCOLORCD, 3))  '外鈑色コード(前3桁)
                        Else
                            query.AddParameterWithTypeValue("BODYCLR_CD", OracleDbType.NVarchar2, estimateInfoRow.EXTCOLORCD)           '外鈑色コード(全部)
                        End If
                        query.AddParameterWithTypeValue("INTERIORCLR_CD", OracleDbType.NVarchar2, estimateInfoRow.INTCOLORCD)           '内装色
                    Case 2
                        query.AddParameterWithTypeValue("SUFFIX_CD", OracleDbType.NVarchar2, estimateInfoRow.SUFFIXCD)                  'サフィックス
                        If (extColor3Flg = "1") Then
                            query.AddParameterWithTypeValue("BODYCLR_CD", OracleDbType.NVarchar2, Left(estimateInfoRow.EXTCOLORCD, 3))  '外鈑色コード(前3桁)
                        Else
                            query.AddParameterWithTypeValue("BODYCLR_CD", OracleDbType.NVarchar2, estimateInfoRow.EXTCOLORCD)           '外鈑色コード(全部)
                        End If
                        query.AddParameterWithTypeValue("INTERIORCLR_CD", OracleDbType.NVarchar2, estimateInfoRow.INTCOLORCD)           '内装色
                    Case 3
                        If (extColor3Flg = "1") Then
                            query.AddParameterWithTypeValue("BODYCLR_CD", OracleDbType.NVarchar2, Left(estimateInfoRow.EXTCOLORCD, 3))  '外鈑色コード(前3桁)
                        Else
                            query.AddParameterWithTypeValue("BODYCLR_CD", OracleDbType.NVarchar2, estimateInfoRow.EXTCOLORCD)           '外鈑色コード(全部)
                        End If
                        query.AddParameterWithTypeValue("INTERIORCLR_CD", OracleDbType.NVarchar2, estimateInfoRow.INTCOLORCD)           '内装色
                    Case 4
                        query.AddParameterWithTypeValue("INTERIORCLR_CD", OracleDbType.NVarchar2, estimateInfoRow.INTCOLORCD)           '内装色
                End Select

                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.NVarchar2, estimateInfoRow.UPDATEACCOUNT)                 '更新ユーザアカウント
                query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, estimateInfoRow.FLLWUPBOX_SEQNO)               '商談ID
                query.AddParameterWithTypeValue("SEQNO", OracleDbType.Long, seqno)                                                      '希望車連番

                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdatePreferVcl_End")
                'ログ出力 End *****************************************************************************

                ' SQL実行（結果を返却）
                If query.Execute() > 0 Then
                    Return True
                Else
                    Me.prpResultId = ErrCodeDBNothing + TblCodeFllwupboxSelectedSeries
                    Throw New ArgumentException("", "fllwupbox_seqno")
                End If

            End Using

        Catch ex As Exception
            If Me.prpResultId = ErrCodeSuccess Then
                Me.prpResultId = ErrCodeDBUpdate + TblCodeFllwupboxSelectedSeries
            End If
            Throw
        End Try
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

    End Function


#End Region

#Region "選択クエリ"
    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 見積管理IDシーケンスから見積管理IDを取得します。
    ''' </summary>
    ''' <returns>見積管理ID</returns>
    ''' <remarks>取得不可の場合は-1を返却します。</remarks>
    Public Function SelEstimateId() As Long
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SelEstimateId_Start")
        'ログ出力 End *****************************************************************************

        Try
            ' SQL組み立て
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT ")
                .Append("  /* IC3070202_202 */ ")
                .Append("  SEQ_ESTIMATEINFO_ESTIMATEID.NEXTVAL AS ESTNO ")
                .Append("FROM ")
                .Append("  DUAL ")
            End With

            ' DbSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of IC3070202DataSet.IC3070202EstSeqDataTable)("IC3070202_202")

                query.CommandText = sql.ToString()

                ' SQL実行
                Dim retDT As IC3070202DataSet.IC3070202EstSeqDataTable = query.GetData()

                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SelEstimateId_End")
                'ログ出力 End *****************************************************************************
                ' 結果を返却
                Return Convert.ToInt64(retDT.Item(0).ESTNO)
            End Using
        Catch ex As Exception
            Me.prpResultId = ErrCodeDBOverlap + TblCodeEstimateInfo
            Throw
        End Try
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

    End Function

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 見積情報テーブルから作成日を取得します。
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <returns>作成日</returns>
    ''' <remarks>取得不可の場合はDateTime.MinValueを返却します。</remarks>
    Public Function SelCreateDate(ByVal estimateId As Long) As Date
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SelCreateDate_Start")
        'ログ出力 End *****************************************************************************

        Try
            ' SQL組み立て
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* IC3070202_019 */ ")
                .Append("    CREATEDATE ")           '見積管理IDシーケンス
                .Append("FROM ")
                .Append("    TBL_ESTIMATEINFO ")
                .Append("WHERE ")
                .Append("    ESTIMATEID = :ESTIMATEID ")
            End With

            ' DbSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of IC3070202DataSet.IC3070202CreateDateDataTable)("IC3070202_019")

                query.CommandText = sql.ToString()

                ' SQLパラメータ設定
                query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Long, estimateId)       '見積管理ID

                ' SQL実行
                Dim retDT As IC3070202DataSet.IC3070202CreateDateDataTable = query.GetData()

                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SelCreateDate_End")
                'ログ出力 End *****************************************************************************
                ' 結果を返却
                If retDT.Rows.Count > 0 Then
                    Return retDT.Item(0).CreateDate
                Else
                    Return DateTime.MinValue
                End If
            End Using
        Catch ex As Exception
            Me.prpResultId = ErrCodeDBOverlap + TblCodeEstimateInfo
            Throw
        End Try
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

    End Function

    '2012/02/07 TCS 明瀬【SALES_1B】START
    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 価格相談に関する情報を取得します。
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SelPriceConsultationInfo(ByVal estimateId As Long) As IC3070202DataSet.IC3070202PriceConsultationDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture, System.Reflection.MethodBase.GetCurrentMethod.Name & "_Start[estimateId:{0}]", estimateId))
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SelPriceConsultationInfo_Start")
        'ログ出力 End *****************************************************************************

        Try
            ' SQL組み立て
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* IC3070202_020 */ ")
                .Append("     A.MANAGERACCOUNT ")
                .Append("   , C.USERNAME MANAGERNAME ")
                .Append("   , A.STAFFACCOUNT ")
                .Append("   , D.USERNAME STAFFNAME ")
                .Append("   , B.NOTICEREQID ")
                .Append("FROM ")
                .Append("     TBL_EST_DISCOUNTAPPROVAL A ")
                .Append("   , TBL_NOTICEREQUEST B ")
                .Append("   , TBL_USERS C ")
                .Append("   , TBL_USERS D ")
                .Append("WHERE ")
                .Append("    A.NOTICEREQID = B.NOTICEREQID ")
                .Append("AND A.MANAGERACCOUNT = C.ACCOUNT ")
                .Append("AND A.STAFFACCOUNT = D.ACCOUNT ")
                .Append("AND A.ESTIMATEID = :ESTIMATEID ")
                .Append("AND B.STATUS <> :STATUS ")
                .Append("ORDER BY ")
                .Append("A.REQUESTPRICE DESC ")
            End With

            ' DbSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of IC3070202DataSet.IC3070202PriceConsultationDataTable)("IC3070202_020")

                query.CommandText = sql.ToString()

                ' SQLパラメータ設定
                query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Long, estimateId)       '見積管理ID
                query.AddParameterWithTypeValue("STATUS", OracleDbType.Char, STATUS_CANCEL)         '通知ステータス(キャンセル)

                ' SQL実行
                Dim retDT As IC3070202DataSet.IC3070202PriceConsultationDataTable = query.GetData()

                Logger.Info(String.Format(CultureInfo.InvariantCulture, System.Reflection.MethodBase.GetCurrentMethod.Name & "_End[GetCount:{0}]", retDT.Count.ToString(CultureInfo.CurrentCulture)))

                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SelPriceConsultationInfo_End")
                'ログ出力 End *****************************************************************************
                Return retDT
            End Using
        Catch ex As Exception
            Me.prpResultId = ErrCodeDBOverlap + TblCodePriceConsultation
            Throw
        End Try
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

    End Function
    '2012/02/07 TCS 明瀬【SALES_1B】END


    '2012/03/02 TCS 劉【SALES_2】ADD START
    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 車種コードを取得します。
    ''' </summary>
    ''' <param name="estimateInfoRow">見積情報</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SelCarNameCD(ByVal estimateInfoRow As IC3070202DataSet.IC3070202EstimationInfoRow) As String
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SelCarNameCD_Start")
        'ログ出力 End *****************************************************************************

        Try
            ' SQL組み立て
            Dim sql As New StringBuilder
            Dim retDT As IC3070202DataSet.IC3070202MstcarNameDataTable

            With sql
                .Append("SELECT /* IC3070202_021 */ ")
                .Append("     CAR_NAME_CD_AI21 ")
                .Append("FROM ( ")
                .Append("    SELECT  ")
                .Append("        CAR_NAME_CD_AI21 ")
                .Append("    FROM ")
                .Append("        TBL_MSTCARNAME ")
                .Append("    WHERE ")
                .Append("        VCLSERIES_CD = :SERIESCD  ")
                .Append("    AND DELETE_FLAG IS NULL ")
                .Append("    ORDER BY VCLCLASS_GENE DESC ) ")
                .Append("WHERE ")
                .Append("    ROWNUM = 1 ")

            End With

            ' DbSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of IC3070202DataSet.IC3070202MstcarNameDataTable)("IC3070202_021")

                query.CommandText = sql.ToString()

                ' SQLパラメータ設定
                '2012/03/28 TCS 李【SALES_2】EDIT START
                'query.AddParameterWithTypeValue("SERIESCD", OracleDbType.NVarchar2, estimateInfoRow.SERIESCD)                    'シリーズコード
                ' シリーズコードが"CAMRY"で、モデルコードに"AHV41L-JEXGBC"が含まれている場合
                If estimateInfoRow.SERIESCD = SERIES_CODE_CAMRY And Not String.IsNullOrEmpty(estimateInfoRow.MODELCD) And _
                   estimateInfoRow.MODELCD.ToString.Contains(MODEL_CD_HV) = True Then
                    query.AddParameterWithTypeValue("SERIESCD", OracleDbType.NVarchar2, SERIES_CODE_CMYHV)                    'シリーズコードCMYHV
                Else
                    query.AddParameterWithTypeValue("SERIESCD", OracleDbType.NVarchar2, estimateInfoRow.SERIESCD)                    'シリーズコードCAMRY
                End If
                '2012/03/28 TCS 李【SALES_2】EDIT END
                ' SQL実行
                retDT = query.GetData()

                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SelCarNameCD_End")
                'ログ出力 End *****************************************************************************
                Return retDT.Item(0).CAR_NAME_CD_AI21
            End Using
        Catch ex As Exception
            Me.prpResultId = ErrCodeDBNothing + TblCodeMstcarName
            Throw
        End Try
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

    End Function

    ' 2013/12/12 TCS 森 Aカード情報相互連携開発 START
    ''' <summary>
    ''' 商談がHistoryテーブルに移行されているかチェックする
    ''' </summary>
    ''' <param name="salesid">商談ID</param>
    ''' <returns>Follow-up Box選択車種情報シーケンスNoのデータセット</returns>
    ''' <remarks></remarks>
    Public Function CheckSalesHistory(ByVal salesid As Decimal) As Boolean

        Dim sql As New StringBuilder
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("CheckSalesHistory_Start")
        'ログ出力 End *****************************************************************************
        With sql
            .Append("SELECT ")
            .Append("  /* IC3070202_207 */ ")
            .Append("  COUNT(1) AS CNT ")
            .Append("FROM ")
            .Append("  TB_H_SALES ")
            .Append("WHERE ")
            .Append("  SALES_ID = :SALES_ID ")
        End With

        Using query As New DBSelectQuery(Of IC3070202DataSet.IC3070202CountDataTable)("IC3070202_207")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, salesid)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("CheckSalesHistory_End")
            'ログ出力 End *****************************************************************************
            ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END

            Return (query.GetCount() > 0)
        End Using

    End Function
    ' 2013/12/12 TCS 森 Aカード情報相互連携開発 END

    '2017/11/20 TCS 河原 TKM独自機能開発 START
    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    '    ''' <summary>
    '    ''' 希望車種を取得します。
    '    ''' </summary>
    '    ''' <param name="estimateInfoRow">見積情報</param>
    '    ''' <param name="salesHisFlg">商談histroyフラグ</param>
    '    ''' <returns></returns>
    '    ''' <remarks></remarks>
    '    Public Function GetPriferdModel(ByVal estimateInfoRow As IC3070202DataSet.IC3070202EstimationInfoRow, ByVal salesHisFlg As Boolean) As IC3070202DataSet.IC3070202FllwupboxSelectedSeriesDataTable
    '        'ログ出力 Start ***************************************************************************
    '        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetPriferdModel_Start")
    '        'ログ出力 End *****************************************************************************
    '
    '        Try
    '            ' SQL組み立て
    '            Dim sql As New StringBuilder
    '            With sql
    '                .Append("SELECT ")
    '                .Append("  /* IC3070202_205 */ ")
    '                .Append("  T1.PREF_VCL_SEQ AS SEQNO , ")
    '                .Append("  T1.MODEL_CD AS SERIESCD , ")
    '                .Append("  T1.GRADE_CD AS MODELCD , ")
    '                .Append("  T1.SUFFIX_CD AS SUFFIX_CD , ")
    '                .Append("  NVL(T3.BODYCLR_CD,' ') AS COLORCD, ")
    '                .Append("  T1.INTERIORCLR_CD AS INTERIORCLR_CD , ")
    '                .Append("  T1.ROW_LOCK_VERSION AS ROWLOCKVERSION ")
    '                .Append("FROM ")
    '                '2013/12/12 TCS 森 Aカード情報相互連携開発 START
    '                If (salesHisFlg = True) Then
    '                    .Append("  TB_H_PREFER_VCL T1 , ")
    '                Else
    '                    .Append("  TB_T_PREFER_VCL T1 , ")
    '                End If
    '                '2013/12/12 TCS 森 Aカード情報相互連携開発 END
    '                .Append("  TBL_MSTCARNAME T2 , ")
    '                .Append("  TBL_MSTEXTERIOR T3 ")
    '                .Append("WHERE ")
    '                .Append("      T1.MODEL_CD = T2.CAR_NAME_CD_AI21 ")
    '                .Append("  AND T1.GRADE_CD = T3.VCLMODEL_CODE(+) ")
    '                .Append("  AND T1.BODYCLR_CD = T3.COLOR_CD(+) ")
    '                .Append("  AND T1.SALES_ID = :FLLWUPBOX_SEQNO ")
    '                .Append("  AND T1.GRADE_CD = :MODELCD ")
    '                .Append("  AND T2.VCLSERIES_CD = :SERIESCD ")
    '            End With
    '
    '            ' DbSelectQueryインスタンス生成
    '            Using query As New DBSelectQuery(Of IC3070202DataSet.IC3070202FllwupboxSelectedSeriesDataTable)("IC3070202_205")
    '
    '                query.CommandText = sql.ToString()
    '
    '                ' SQLパラメータ設定
    '                query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, estimateInfoRow.FLLWUPBOX_SEQNO)       '商談ID
    '                '2012/03/28 TCS 李【SALES_2】EDIT START
    '                'query.AddParameterWithTypeValue("SERIESCD", OracleDbType.NVarchar2, estimateInfoRow.SERIESCD)                'モデルコード
    '                ' シリーズコードが"CAMRY"で、モデルコードに"AHV41L-JEXGBC"が含まれている場合
    '                If estimateInfoRow.SERIESCD = SERIES_CODE_CAMRY And Not String.IsNullOrEmpty(estimateInfoRow.MODELCD) And _
    '                   estimateInfoRow.MODELCD.ToString.Contains(MODEL_CD_HV) = True Then
    '                    query.AddParameterWithTypeValue("SERIESCD", OracleDbType.NVarchar2, SERIES_CODE_CMYHV)                    'モデルコードCMYHV
    '                Else
    '                    query.AddParameterWithTypeValue("SERIESCD", OracleDbType.NVarchar2, estimateInfoRow.SERIESCD)                'モデルコードCAMRY
    '                End If
    '                '2012/03/28 TCS 李【SALES_2】EDIT END
    '                query.AddParameterWithTypeValue("MODELCD", OracleDbType.Varchar2, estimateInfoRow.MODELCD)                   'グレードコード
    '
    '                ' SQL実行
    '                Dim retDT As IC3070202DataSet.IC3070202FllwupboxSelectedSeriesDataTable = query.GetData()
    '
    '                'ログ出力 Start ***************************************************************************
    '                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetPriferdModel_End")
    '                'ログ出力 End *****************************************************************************
    '                Return retDT
    '            End Using
    '        Catch ex As Exception
    '            Me.prpResultId = ErrCodeDBNothing + TblCodeFllwupboxSelectedSeries
    '            Throw
    '        End Try
    '        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END
    '
    '    End Function

    '2018/04/17 TCS 河原 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 START
    ''' <summary>
    ''' 希望車種を取得します。
    ''' </summary>
    ''' <param name="estimateInfoRow">見積情報</param>
    ''' <param name="salesHisFlg">商談histroyフラグ</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetPriferdModel(ByVal estimateInfoRow As IC3070202DataSet.IC3070202EstimationInfoRow,
                                    ByVal salesHisFlg As Boolean,
                                    ByVal use_suffix As String,
                                    ByVal use_interiorcolor As String) As IC3070202DataSet.IC3070202FllwupboxSelectedSeriesDataTable

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetPriferdModel_Start")
        'ログ出力 End *****************************************************************************

        Try
            ' SQL組み立て
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT ")
                .Append("  DISTINCT ")
                .Append("  T1.PREF_VCL_SEQ AS SEQNO, ")
                .Append("  T1.MODEL_CD AS SERIESCD, ")
                .Append("  T1.GRADE_CD AS MODELCD, ")
                .Append("  T1.SUFFIX_CD AS SUFFIX_CD, ")
                .Append("  T1.BODYCLR_CD AS COLORCD, ")
                .Append("  T1.INTERIORCLR_CD AS INTERIORCLR_CD, ")
                .Append("  T1.ROW_LOCK_VERSION AS ROWLOCKVERSION ")
                .Append("FROM ")
                If (salesHisFlg = True) Then
                    .Append("  TB_H_PREFER_VCL T1 , ")
                Else
                    .Append("  TB_T_PREFER_VCL T1 , ")
                End If
                .Append("  TBL_MSTCARNAME T2 ")
                .Append("WHERE ")
                .Append("      T1.SALES_ID = :SALES_ID ")
                .Append("  AND T2.VCLSERIES_CD = :MODEL_CD ")
                .Append("  AND T1.MODEL_CD = T2.CAR_NAME_CD_AI21 ")
                .Append("  AND (T1.GRADE_CD = :GRADE_CD OR TRIM(T1.GRADE_CD) IS NULL) ")
                If String.Equals(use_suffix, "1") Then
                    .Append("  AND (T1.SUFFIX_CD = :SUFFIX_CD OR TRIM(T1.SUFFIX_CD) IS NULL) ")
                End If
                .Append("  AND (T1.BODYCLR_CD = :BODYCLR_CD OR TRIM(T1.BODYCLR_CD) IS NULL) ")
                If String.Equals(use_interiorcolor, "1") Then
                    .Append("  AND (T1.INTERIORCLR_CD = :INTERIORCLR_CD OR TRIM(T1.INTERIORCLR_CD) IS NULL) ")
                End If
                .Append("ORDER BY TRIM(INTERIORCLR_CD), TRIM(BODYCLR_CD), TRIM(SUFFIX_CD), TRIM(GRADE_CD), TRIM(MODEL_CD)")
            End With

            ' DbSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of IC3070202DataSet.IC3070202FllwupboxSelectedSeriesDataTable)("IC3070202_205")

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, estimateInfoRow.FLLWUPBOX_SEQNO)       '商談ID

                If estimateInfoRow.SERIESCD = SERIES_CODE_CAMRY And Not String.IsNullOrEmpty(estimateInfoRow.MODELCD) And estimateInfoRow.MODELCD.ToString.Contains(MODEL_CD_HV) = True Then
                    query.AddParameterWithTypeValue("MODEL_CD", OracleDbType.NVarchar2, SERIES_CODE_CMYHV)               'モデルコードCMYHV
                Else
                    query.AddParameterWithTypeValue("MODEL_CD", OracleDbType.NVarchar2, estimateInfoRow.SERIESCD)        'モデルコードCAMRY
                End If

                query.AddParameterWithTypeValue("GRADE_CD", OracleDbType.Varchar2, estimateInfoRow.MODELCD)              'グレードコード

                If String.Equals(use_suffix, "1") Then
                    query.AddParameterWithTypeValue("SUFFIX_CD", OracleDbType.Varchar2, estimateInfoRow.SUFFIXCD)        'サフィックスコード
                End If

                query.AddParameterWithTypeValue("BODYCLR_CD", OracleDbType.Varchar2, estimateInfoRow.EXTCOLORCD)         '外装色コード

                If String.Equals(use_interiorcolor, "1") Then
                    query.AddParameterWithTypeValue("INTERIORCLR_CD", OracleDbType.Varchar2, estimateInfoRow.INTCOLORCD) '内装色コード
                End If

                ' SQL実行
                Dim retDT As IC3070202DataSet.IC3070202FllwupboxSelectedSeriesDataTable = query.GetData()

                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetPriferdModel_End")
                'ログ出力 End *****************************************************************************

                Return retDT
            End Using
        Catch ex As Exception
            Me.prpResultId = ErrCodeDBNothing + TblCodeFllwupboxSelectedSeries
            Throw
        End Try

    End Function
    '2017/11/20 TCS 河原 TKM独自機能開発 END
    '2018/04/17 TCS 河原 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 END

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    ''' <summary>
    ''' Follow-up Box選択車種シーケンスNoシーケンスからシーケンスNoを取得します。
    ''' </summary>
    ''' <param name="estimateInfoRow">見積情報</param>
    ''' <param name="salesHisFlg">商談histroyフラグ</param>
    ''' <remarks>取得不可の場合は-1を返却します。</remarks>
    Public Function SelSeqno(ByVal estimateInfoRow As IC3070202DataSet.IC3070202EstimationInfoRow, ByVal salesHisFlg As Boolean)
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SelSeqno_Start")
        'ログ出力 End *****************************************************************************
        Try
            ' SQL組み立て
            Dim sql As New StringBuilder
            With sql
                .Append("  SELECT /* IC3070202_206 */ ")
                .Append("  NVL2(MAX(PREF_VCL_SEQ), MAX(PREF_VCL_SEQ) + 1, 1) AS SEQNO ")
                .Append("FROM ")
                '2013/12/12 TCS 森 Aカード情報相互連携開発 START
                If (salesHisFlg = True) Then
                    .Append("  TB_H_PREFER_VCL ")
                Else
                    .Append("  TB_T_PREFER_VCL ")
                End If
                .Append("WHERE ")
                .Append("  SALES_ID = :FLLWUPBOX_SEQNO ")
            End With

            ' DbSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of IC3070202DataSet.IC3070202PrefVclSeqDataTable)("IC3070202_206")

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, estimateInfoRow.FLLWUPBOX_SEQNO)       '希望車連番

                ' SQL実行
                Dim retDT As IC3070202DataSet.IC3070202PrefVclSeqDataTable = query.GetData()

                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SelSeqno_End")
                'ログ出力 End *****************************************************************************
                ' 結果を返却
                Return Convert.ToInt64(retDT.Item(0).SEQNO)
            End Using
        Catch ex As Exception
            Me.prpResultId = ErrCodeDBNothing + TblCodeFllwupboxSelectedSeries
            Throw
        End Try
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

    End Function
    '2012/03/02 TCS 劉【SALES_2】ADD END
#End Region

    Private Function GetDbStringParameter(ByVal value As Object) As String
        If (value Is DBNull.Value) Then
            Return " "
        End If

        Dim strValue = value.ToString()
        If (strValue.Length = 0) Then
            Return " "
        End If
        Return strValue
    End Function

    '2018/04/17 TCS 河原 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 START
    '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 DEL
    '2018/04/17 TCS 河原 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 END

#End Region

#Region "契約変更情報取得"

    '更新： 2014/05/28 TCS 安田 受注時説明機能開発（受注後工程スケジュール） START
    ''' <summary>
    ''' 注文承認されているかチェックする
    ''' </summary>
    ''' <param name="estimateInfoRow">見積情報</param>
    ''' <returns>True:注文承認後／False:注文承認前</returns>
    ''' <remarks></remarks>
    Public Function CheckBookAfter(ByVal estimateInfoRow As IC3070202DataSet.IC3070202EstimationInfoRow) As Boolean

        Dim sql As New StringBuilder
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("CheckBookAfter_Start")
        'ログ出力 End *****************************************************************************
        With sql
            .Append("SELECT ")
            .Append("  /* IC3070202_214 */ ")
            .Append("  COUNT(1) AS CNT ")
            .Append("FROM ")
            .Append("  TBL_ESTIMATEINFO ")
            .Append(" WHERE DLRCD = :DLRCD ")
            .Append("   AND FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO ")
            .Append("   AND CONTRACTNO IS NOT NULL ")
            .Append("   AND DELFLG = '0' ")
        End With

        Using query As New DBSelectQuery(Of IC3070202DataSet.IC3070202CountDataTable)("IC3070202_214")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, estimateInfoRow.DLRCD)                          '販売店コード
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, estimateInfoRow.FLLWUPBOX_SEQNO)   'Follow-up Box内連番

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("CheckBookAfter_End")
            'ログ出力 End *****************************************************************************

            Return (query.GetCount() > 0)
        End Using

    End Function

    ''' <summary>
    ''' 見積変更前情報の取得
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetEstBeforeChangeInfo(ByVal estimateId As Long) As IC3070202DataSet.IC3070202EstChangeInfoDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture, System.Reflection.MethodBase.GetCurrentMethod.Name & "_Start[estimateId:{0}]", estimateId))
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetEstBeforeChangeInfo_Start")
        'ログ出力 End *****************************************************************************

        Try
            ' SQL組み立て
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* IC3070202_215 */ ")
                .Append("       A.CONTRACT_COND_CHG_FLG,   ")
                .Append("       NVL(B.INSUDVS, ' ') AS INSUDVS,   ")
                .Append("       NVL(C.PAYMENTMETHOD, ' ') AS PAYMENTMETHOD ,  ")
                .Append("       NVL(C.DEPOSITPAYMENTMETHOD, ' ') AS DEPOSITPAYMENTMETHOD ")
                .Append("  FROM TBL_ESTIMATEINFO A, TBL_EST_INSURANCEINFO B, TBL_EST_PAYMENTINFO C  ")
                .Append(" WHERE A.ESTIMATEID = :ESTIMATEID  ")
                .Append("   AND A.ESTIMATEID = B.ESTIMATEID(+)  ")
                .Append("   AND A.ESTIMATEID = C.ESTIMATEID(+)  ")
                .Append("   AND C.SELECTFLG(+) = '1'  ")
            End With

            ' DbSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of IC3070202DataSet.IC3070202EstChangeInfoDataTable)("IC3070202_215")

                query.CommandText = sql.ToString()

                ' SQLパラメータ設定
                query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Long, estimateId)       '見積管理ID

                ' SQL実行
                Dim retDT As IC3070202DataSet.IC3070202EstChangeInfoDataTable = query.GetData()

                Logger.Info(String.Format(CultureInfo.InvariantCulture, System.Reflection.MethodBase.GetCurrentMethod.Name & "_End[GetCount:{0}]", retDT.Count.ToString(CultureInfo.CurrentCulture)))

                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetEstBeforeChangeInfo_End")
                'ログ出力 End *****************************************************************************
                Return retDT
            End Using
        Catch ex As Exception
            Me.prpResultId = ErrCodeDBOverlap + TblCodeEstimateInfo
            Throw
        End Try
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

    End Function

    '更新： 2014/05/28 TCS 安田 受注時説明機能開発（受注後工程スケジュール） END
#End Region

#Region "コンストラクタ"
    ''' <summary>
    ''' コンストラクタ
    ''' </summary>
    ''' <param name="updateDvs">更新処理区分（0：登録/1：更新/2：削除）</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal updateDvs As Short)

        ' 更新処理区分を設定
        Me.prpUpdDvs = updateDvs

    End Sub
#End Region


End Class
