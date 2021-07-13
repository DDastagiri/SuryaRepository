Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core

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

    ''' <summary>
    ''' 更新処理区分：0（登録）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const UpdateDvsRegist As Short = 0
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
    ''' <summary>
    ''' 見積情報を削除します。
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <returns>処理結果（成功[True]/失敗[False]）</returns>
    ''' <remarks></remarks>
    Public Function DelEstimateInfoDataTable(ByVal estimateId As Long) As Boolean

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
                query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Long, estimateId)

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

    End Function

    ''' <summary>
    ''' 見積車両情報を削除します。
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <returns>処理結果（成功[True]/失敗[False]）</returns>
    ''' <remarks></remarks>
    Public Function DelEstVclInfoDataTable(ByVal estimateId As Long) As Boolean

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

    End Function

    ''' <summary>
    ''' 見積車両オプション情報を削除します。
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <param name="mode">実行モード（0：見積の全情報を更新　1：見積の車両情報のみ更新）</param>
    ''' <param name="vclOptionUpdateDvs">車両オプション更新区分（0：車両オプションを全て更新　1：車両オプションをメーカーオプションのみ更新）</param>
    ''' <returns>処理結果（成功[True]/失敗[False]</returns>
    ''' <remarks></remarks>
    Public Function DelEstVclOptionInfoDataTable(ByVal estimateId As Long, ByVal mode As Integer, ByVal vclOptionUpdateDvs As Integer) As Boolean

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
                    .Append("  AND OPTIONPART = '1' ")
                End If
            End With

            ' DbUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("IC3070202_003")

                query.CommandText = sql.ToString()

                ' SQLパラメータ設定
                query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Long, estimateId)

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

    End Function

    ''' <summary>
    ''' 見積保険情報を削除します。
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <returns>処理結果（成功[True]/失敗[False]）</returns>
    ''' <remarks></remarks>
    Public Function DelEstInsuranceInfoDataTable(ByVal estimateId As Long) As Boolean

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

    End Function

    ''' <summary>
    ''' 見積支払い方法情報を削除します。
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <returns>処理結果（成功[True]/失敗[False]）</returns>
    ''' <remarks></remarks>
    Public Function DelEstPaymentInfoDataTable(ByVal estimateId As Long) As Boolean

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

    End Function

    ''' <summary>
    ''' 見積顧客情報を削除します。
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <returns>処理結果（成功[True]/失敗[False]）</returns>
    ''' <remarks></remarks>
    Public Function DelEstCustomerInfoDataTable(ByVal estimateId As Long) As Boolean

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

    End Function

    ''' <summary>
    ''' 見積諸費用情報を削除します。
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <returns>処理結果（成功[True]/失敗[False]）</returns>
    ''' <remarks></remarks>
    Public Function DelEstChargeInfoDataTable(ByVal estimateId As Long) As Boolean

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

    End Function

    ''' <summary>
    ''' 見積下取車両情報を削除します。
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <returns>処理結果（成功[True]/失敗[False]）</returns>
    ''' <remarks></remarks>
    Public Function DelEstTradeInCarInfoDataTable(ByVal estimateId As Long) As Boolean

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

    End Function
#End Region

#Region "挿入クエリ"
    ''' <summary>
    ''' 見積情報を挿入します。
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <param name="contPrintFlg">契約書印刷フラグ</param>
    ''' <param name="dr">見積情報データテーブル行</param>
    ''' <returns>処理結果（成功[True]/失敗[False]）</returns>
    ''' <remarks></remarks>
    Public Function InsEstimateInfoDataTable(ByVal estimateId As Long, _
                                             ByVal contPrintFlg As String, _
                                             ByVal dr As IC3070202DataSet.IC3070202EstimationInfoRow) As Boolean

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
                    query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Long, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Long, dr.FLLWUPBOX_SEQNO)
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
    End Function

    ''' <summary>
    ''' 見積車両情報を挿入します。
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <param name="dr">見積情報データテーブル行</param>
    ''' <returns>処理結果（成功[True]/失敗[False]）</returns>
    ''' <remarks></remarks>
    Public Function InsEstVclInfoDataTable(ByVal estimateId As Long, _
                                           ByVal dr As IC3070202DataSet.IC3070202EstimationInfoRow) As Boolean

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

    End Function

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

    End Function

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

    End Function

    ''' <summary>
    ''' 見積支払い方法情報を挿入します。
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <param name="dr">見積支払い方法情報データテーブル行</param>
    ''' <param name="drBase">見積情報データテーブル行</param>
    ''' <returns>処理結果（成功[True]/失敗[False]）</returns>
    ''' <remarks></remarks>
    Public Function InsEstPaymentInfoDataTable(ByVal estimateId As Long, _
                                               ByVal dr As IC3070202DataSet.IC3070202EstPaymentInfoRow, _
                                               ByVal drBase As IC3070202DataSet.IC3070202EstimationInfoRow) As Boolean

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

    End Function

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
                query.AddParameterWithTypeValue("CREATEACCOUNT", OracleDbType.Varchar2, drBase.UPDATEACCOUNT)          '作成ユーザアカウント
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, drBase.UPDATEACCOUNT)          '更新ユーザアカウント
                query.AddParameterWithTypeValue("CREATEID", OracleDbType.Varchar2, drBase.UPDATEID)                    '作成機能ID
                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, drBase.UPDATEID)                    '更新機能ID

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

    End Function

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

    End Function

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

    End Function
#End Region

#Region "更新クエリ"

    ''' <summary>
    ''' 見積情報を更新（論理削除）します。
    ''' </summary>
    ''' <param name="ESTIMATEID">見積管理ID</param>
    ''' <returns>処理結果（成功[True]/失敗[False]）</returns>
    ''' <remarks></remarks>
    Public Function UpdEstimateInfoDataTable(ByVal estimateId As Long) As Boolean

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
                query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Int64, estimateId)       '見積管理ID

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

    End Function
#End Region

#Region "選択クエリ"
    ''' <summary>
    ''' 見積管理IDシーケンスから見積管理IDを取得します。
    ''' </summary>
    ''' <returns>見積管理ID</returns>
    ''' <remarks>取得不可の場合は-1を返却します。</remarks>
    Public Function SelEstimateId() As Long

        Try
            ' SQL組み立て
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* IC3070202_018 */ ")
                .Append("    SEQ_ESTIMATEINFO_ESTIMATEID.NEXTVAL AS ESTIMATEID ")           '見積管理IDシーケンス
                .Append("FROM ")
                .Append("    DUAL ")
            End With

            ' DbSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of IC3070202DataSet.IC3070202EstimateIdDataTable)("IC3070202_018")

                query.CommandText = sql.ToString()

                ' SQL実行
                Dim retDT As IC3070202DataSet.IC3070202EstimateIdDataTable = query.GetData()

                ' 結果を返却
                Return Convert.ToInt64(retDT.Item(0).ESTIMATEID)
            End Using
        Catch ex As Exception
            Me.prpResultId = ErrCodeDBOverlap + TblCodeEstimateInfo
            Throw
        End Try

    End Function

    ''' <summary>
    ''' 見積情報テーブルから作成日を取得します。
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <returns>作成日</returns>
    ''' <remarks>取得不可の場合はDateTime.MinValueを返却します。</remarks>
    Public Function SelCreateDate(ByVal estimateId As Long) As Date

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
                query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Int64, estimateId)       '見積管理ID

                ' SQL実行
                Dim retDT As IC3070202DataSet.IC3070202CreateDateDataTable = query.GetData()

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

    End Function

#End Region

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
