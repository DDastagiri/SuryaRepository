'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'IC3070203TableAdapter.vb
'─────────────────────────────────────
'機能： 見積登録I/F
'補足： 
'作成： 2013/12/10 TCS 森
'更新： 2014/05/28 TCS 安田 受注時説明機能開発（受注後工程スケジュール）
'─────────────────────────────────────

Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic

''' <summary>
''' 見積情報登録I/F
''' テーブルアダプタークラス
''' </summary>
''' <remarks></remarks>
Public Class IC3070203TableAdapter

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
    ''' テーブルコード：顧客情報
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TblCodeCustomer As Short = 14
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
    ''' テーブルコード：顧客連絡時間帯
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TblCodeCstContactTimeSlot As Short = 16

    ''' <summary>
    ''' 更新処理区分：0（登録）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const UpdateDvsRegist As Short = 0

    ''' <summary>
    ''' 画面ID：見積登録IF
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MY_PROGRAMID As String = "IC3070203"

    ''' <summary>
    ''' 選択フラグ：未選択
    ''' </summary>
    ''' <remarks></remarks>
    Public Const SelectFlgNotSelect As String = "0"

    ''' <summary>
    ''' 選択フラグ：選択
    ''' </summary>
    ''' <remarks></remarks>
    Public Const SelectFlgSelect As String = "1"

    ''' <summary>
    ''' DB文字列初期値
    ''' </summary>
    ''' <remarks></remarks>
    Public Const StringDefValue As String = " "
    ''' <summary>
    ''' DB文字列初期値
    ''' </summary>
    ''' <remarks></remarks>
    Public Const StringDefValueZero As String = "0"
    ''' <summary>
    ''' DB数値初期値
    ''' </summary>
    ''' <remarks></remarks>
    Public Const NumDefValue As Integer = 0
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
    ''' 顧客連絡時間帯削除
    ''' </summary>
    ''' <param name="cstId">顧客ID</param>
    ''' <returns>処理結果（成功[True]/失敗[False]）</returns>
    ''' <remarks></remarks>
    Public Function DeleteCstContactTimeslot(ByVal cstId As Decimal) As Boolean

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeleteCstContactTimeslot_Start")
        'ログ出力 End *****************************************************************************

        Try
            ' SQL組み立て
            Dim sql As New StringBuilder

            With sql
                .Append(" DELETE /* IC3070203_216 */ ")
                .Append(" FROM ")
                .Append("     TB_M_CST_CONTACT_TIMESLOT ")
                .Append(" WHERE ")
                .Append("     CST_ID = :CST_ID ")                 '顧客ID
                .Append("  AND TIMESLOT_CLASS = :TIMESLOT_CLASS")  '時間帯分類
            End With

            ' DbUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("IC3070203_216")

                query.CommandText = sql.ToString()

                ' SQLパラメータ設定
                query.AddParameterWithTypeValue("CST_ID", OracleDbType.Decimal, cstId)
                query.AddParameterWithTypeValue("TIMESLOT_CLASS", OracleDbType.NVarchar2, "1")

                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeleteCstContactTimeslot_End")
                'ログ出力 End *****************************************************************************
                ' SQL実行（結果を返却）
                If query.Execute() >= 0 Then
                    Return True
                Else
                    Return False
                End If
            End Using
        Catch ex As Exception
            Me.prpResultId = ErrCodeDBUpdate + TblCodeCstContactTimeSlot
            Throw
        End Try

    End Function


    ''' <summary>
    ''' 見積車両オプション削除
    ''' </summary>
    ''' <param name="dr">見積車両オプション情報データテーブル</param>
    ''' <returns>処理結果（成功[True]/失敗[False]）</returns>
    ''' <remarks></remarks>
    Public Function DeleteEstVcloptioninfo(ByVal dr As IC3070203DataSet.IC3070203EstVclOptionInfoRow) As Boolean


        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeleteEstVcloptioninfo_Start")
        'ログ出力 End *****************************************************************************

        Try
            ' SQL組み立て
            Dim sql As New StringBuilder

            With sql
                .Append(" DELETE /* IC3070203_218 */ ")
                .Append(" FROM ")
                .Append("     TBL_EST_VCLOPTIONINFO ")
                .Append(" WHERE ")
                .Append("     ESTIMATEID = :ESTIMATEID ")         '見積管理ID
                .Append("  AND OPTIONPART = :OPTIONPART ")        'オプション区分
                .Append("  AND OPTIONCODE = :OPTIONCODE")         'オプションコード
            End With

            ' DbUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("IC3070203_218")

                query.CommandText = sql.ToString()

                ' SQLパラメータ設定
                query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Int64, dr.ESTIMATEID)        '見積管理ID
                query.AddParameterWithTypeValue("OPTIONPART", OracleDbType.NVarchar2, dr.OPTIONPART)    'オプション区分
                query.AddParameterWithTypeValue("OPTIONCODE", OracleDbType.NVarchar2, dr.OPTIONCODE)    'オプションコード

                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeleteEstVcloptioninfo_End")
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


    End Function

#End Region

#Region "挿入クエリ"
    ''' <summary>
    ''' 見積車両オプション情報を追加
    ''' </summary>
    ''' <param name="dr">見積車両オプション情報データテーブル行</param>
    ''' <returns>処理結果（成功[True]/失敗[False]）</returns>
    ''' <remarks></remarks>
    Public Function InsertEstVcloptioninfo(ByVal dr As IC3070203DataSet.IC3070203EstVclOptionInfoRow) As Boolean
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertEstVcloptioninfo_Start")
        'ログ出力 End *****************************************************************************

        Try
            ' データ行のNullチェック
            If dr Is Nothing Then
                Throw New ArgumentNullException("dr")
            End If

            ' SQL組み立て
            Dim sql As New StringBuilder

            With sql
                .Append(" INSERT /* IC3070203_011 */ ")
                .Append(" INTO ")
                .Append("     TBL_EST_VCLOPTIONINFO ")
                .Append(" ( ")
                .Append("     ESTIMATEID ")          '見積管理ID
                .Append("   , OPTIONPART ")          'オプション区分
                .Append("   , OPTIONCODE ")          'オプションコード
                .Append("   , OPTIONNAME ")          'オプション名
                .Append("   , PRICE ")               '価格
                .Append("   , INSTALLCOST ")         '取付費用
                .Append("   , CREATEDATE ")          '作成日
                .Append("   , UPDATEDATE ")          '更新日
                .Append("   , CREATEACCOUNT ")       '作成ユーザアカウント
                .Append("   , UPDATEACCOUNT ")       '更新ユーザアカウント
                .Append("   , CREATEID ")            '作成機能ID
                .Append("   , UPDATEID ")            '更新機能ID
                .Append(" ) ")
                .Append(" VALUES ")
                .Append(" ( ")
                .Append("     :ESTIMATEID ")          '見積管理ID
                .Append("   , :OPTIONPART ")          'オプション区分
                .Append("   , :OPTIONCODE ")          'オプションコード
                .Append("   , :OPTIONNAME ")          'オプション名
                .Append("   , :PRICE ")               '価格
                .Append("   , :INSTALLCOST ")         '取付費用
                .Append("   , SYSDATE ")              '作成日
                .Append("   , SYSDATE ")              '更新日
                .Append("   , :CREATEACCOUNT ")       '作成ユーザアカウント
                .Append("   , :UPDATEACCOUNT ")       '更新ユーザアカウント
                .Append("   , :CREATEID ")            '作成機能ID
                .Append("   , :UPDATEID ")            '更新機能ID
                .Append(" ) ")
            End With

            ' DbUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("IC3070203_011")

                query.CommandText = sql.ToString()

                ' SQLパラメータ設定
                query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Int64, dr.ESTIMATEID)        '見積管理ID
                query.AddParameterWithTypeValue("OPTIONPART", OracleDbType.Char, dr.OPTIONPART)         'オプション区分
                query.AddParameterWithTypeValue("OPTIONCODE", OracleDbType.Varchar2, dr.OPTIONCODE)     'オプションコード
                query.AddParameterWithTypeValue("OPTIONNAME", OracleDbType.NVarchar2, dr.OPTIONNAME)    'オプション名
                query.AddParameterWithTypeValue("PRICE", OracleDbType.Double, dr.PRICE)                 '価格
                '取付費用
                If dr.IsINSTALLCOSTNull Then
                    query.AddParameterWithTypeValue("INSTALLCOST", OracleDbType.Double, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("INSTALLCOST", OracleDbType.Double, dr.INSTALLCOST)
                End If
                query.AddParameterWithTypeValue("CREATEACCOUNT", OracleDbType.Varchar2, StringDefValue)            '作成ユーザアカウント
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, StringDefValue)            '更新ユーザアカウント
                query.AddParameterWithTypeValue("CREATEID", OracleDbType.Varchar2, MY_PROGRAMID)        '作成機能ID
                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, MY_PROGRAMID)        '更新機能ID

                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertEstVcloptioninfo_End")
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

    End Function

    ''' <summary>
    ''' 見積保険情報を追加します。
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <param name="dr">見積保険情報データテーブル行</param>
    ''' <returns>処理結果（成功[True]/失敗[False]）</returns>
    ''' <remarks></remarks>
    Public Function InsertEstInsuranceinfo(ByVal estimateId As Long, _
                                           ByVal dr As IC3070203DataSet.IC3070203EstInsuranceInfoRow) As Boolean
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertEstInsuranceinfo_Start")
        'ログ出力 End *****************************************************************************

        Try
            ' データ行のNullチェック
            If dr Is Nothing Then
                Throw New ArgumentNullException("dr")
            End If

            ' SQL組み立て
            Dim sql As New StringBuilder

            With sql
                .Append(" INSERT /* IC3070203_012 */ ")
                .Append(" INTO ")
                .Append("     TBL_EST_INSURANCEINFO ")
                .Append(" ( ")
                .Append("     ESTIMATEID ")          '見積管理ID
                .Append("   , INSUDVS ")             '保険区分
                .Append("   , INSUCOMCD ")           '保険会社コード
                .Append("   , INSUKIND ")            '保険種別
                .Append("   , AMOUNT ")              '保険金額
                .Append("   , CREATEDATE ")          '作成日
                .Append("   , UPDATEDATE ")          '更新日
                .Append("   , CREATEACCOUNT ")       '作成ユーザアカウント
                .Append("   , UPDATEACCOUNT ")       '更新ユーザアカウント
                .Append("   , CREATEID ")            '作成機能ID
                .Append("   , UPDATEID ")            '更新機能ID
                .Append(" ) ")
                .Append(" VALUES ")
                .Append(" ( ")
                .Append("     :ESTIMATEID ")          '見積管理ID
                .Append("   , :INSUDVS ")             '保険区分
                .Append("   , :INSUCOMCD ")           '保険会社コード
                .Append("   , :INSUKIND ")            '保険種別
                .Append("   , :AMOUNT ")              '保険金額
                .Append("   , SYSDATE ")              '作成日
                .Append("   , SYSDATE ")              '更新日
                .Append("   , :CREATEACCOUNT ")       '作成ユーザアカウント
                .Append("   , :UPDATEACCOUNT ")       '更新ユーザアカウント
                .Append("   , :CREATEID ")            '作成機能ID
                .Append("   , :UPDATEID ")            '更新機能ID
                .Append(" ) ")
            End With

            ' DbUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("IC3070203_012")

                query.CommandText = sql.ToString()

                ' SQLパラメータ設定
                query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Int64, estimateId)   '見積管理ID
                query.AddParameterWithTypeValue("INSUDVS", OracleDbType.Char, dr.INSUDVS)       '保険区分
                query.AddParameterWithTypeValue("INSUCOMCD", OracleDbType.Char, DBNull.Value)   '保険会社コード
                query.AddParameterWithTypeValue("INSUKIND", OracleDbType.Char, DBNull.Value)    '保険種別
                query.AddParameterWithTypeValue("AMOUNT", OracleDbType.Double, DBNull.Value)    '保険金額
                query.AddParameterWithTypeValue("CREATEACCOUNT", OracleDbType.Varchar2, StringDefValue)        '作成ユーザアカウント
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, StringDefValue)        '更新ユーザアカウント
                query.AddParameterWithTypeValue("CREATEID", OracleDbType.Varchar2, MY_PROGRAMID)    '作成機能ID
                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, MY_PROGRAMID)    '更新機能ID

                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertEstInsuranceinfo_End")
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

    End Function

    ''' <summary>
    ''' 見積支払方法情報を追加
    ''' </summary>
    ''' <param name="dr">見積支払い方法情報データテーブル行</param>
    ''' <param name="paymentmethod">支払方法区分</param>
    ''' <param name="selectFlg">選択フラグ</param>
    ''' <returns>処理結果（成功[True]/失敗[False]）</returns>
    ''' <remarks></remarks>
    Public Function InsertEstPaymentinfo(ByVal dr As IC3070203DataSet.IC3070203EstPaymentInfoRow, _
                                         ByVal paymentmethod As String, _
                                         ByVal selectFlg As String) As Boolean
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertEstPaymentinfo_Start")
        'ログ出力 End *****************************************************************************

        Try
            ' データ行のNullチェック
            If dr Is Nothing Then
                Throw New ArgumentNullException("dr")
            End If

            ' SQL組み立て
            Dim sql As New StringBuilder

            With sql
                .Append(" INSERT /* IC3070203_013 */ ")
                .Append(" INTO ")
                .Append("     TBL_EST_PAYMENTINFO ")
                .Append(" ( ")
                .Append("     ESTIMATEID ")             '見積管理ID
                .Append("   , PAYMENTMETHOD ")          '支払方法区分
                .Append("   , FINANCECOMCODE ")         '融資会社コード
                .Append("   , PAYMENTPERIOD ")          '支払期間
                .Append("   , MONTHLYPAYMENT ")         '毎月返済額
                .Append("   , DEPOSIT ")                '頭金
                .Append("   , BONUSPAYMENT ")           'ボーナス時返済額
                .Append("   , DUEDATE ")                '初回支払期限
                .Append("   , DELFLG ")                 '削除フラグ
                .Append("   , CREATEDATE ")             '作成日
                .Append("   , UPDATEDATE ")             '更新日
                .Append("   , CREATEACCOUNT ")          '作成ユーザアカウント
                .Append("   , UPDATEACCOUNT ")          '更新ユーザアカウント
                .Append("   , CREATEID ")               '作成機能ID
                .Append("   , UPDATEID ")               '更新機能ID
                .Append("   , SELECTFLG ")              '選択フラグ
                .Append("   , INTERESTRATE ")           '利率
                .Append("   , DEPOSITPAYMENTMETHOD ")   '頭金支払方法区分
                .Append(" ) ")
                .Append(" VALUES ")
                .Append(" ( ")
                .Append("     :ESTIMATEID ")            '見積管理ID
                .Append("   , :PAYMENTMETHOD ")         '支払方法区分
                .Append("   , :FINANCECOMCODE ")        '融資会社コード
                .Append("   , :PAYMENTPERIOD ")         '支払期間
                .Append("   , :MONTHLYPAYMENT ")        '毎月返済額
                .Append("   , :DEPOSIT ")               '頭金
                .Append("   , :BONUSPAYMENT ")          'ボーナス時返済額
                .Append("   , :DUEDATE ")               '初回支払期限
                .Append("   , :DELFLG ")                '削除フラグ
                .Append("   , SYSDATE ")                '作成日
                .Append("   , SYSDATE ")                '更新日
                .Append("   , :CREATEACCOUNT ")         '作成ユーザアカウント
                .Append("   , :UPDATEACCOUNT ")         '更新ユーザアカウント
                .Append("   , :CREATEID ")              '作成機能ID
                .Append("   , :UPDATEID ")              '更新機能ID
                .Append("   , :SELECTFLG ")             '選択フラグ
                .Append("   , :INTERESTRATE ")          '利率
                .Append("   , :DEPOSITPAYMENTMETHOD ")  '頭金支払方法区分
                .Append(" ) ")
            End With

            ' DbUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("IC3070203_013")

                query.CommandText = sql.ToString()

                ' SQLパラメータ設定
                query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Int64, dr.ESTIMATEID)        '見積管理ID
                query.AddParameterWithTypeValue("PAYMENTMETHOD", OracleDbType.Char, paymentmethod)      '支払方法区分
                query.AddParameterWithTypeValue("FINANCECOMCODE", OracleDbType.Char, DBNull.Value) '融資会社コード
                query.AddParameterWithTypeValue("PAYMENTPERIOD", OracleDbType.Int64, DBNull.Value) '支払期間
                query.AddParameterWithTypeValue("MONTHLYPAYMENT", OracleDbType.Double, DBNull.Value) '毎月返済額
                '頭金
                If dr.IsDEPOSITNull Or SelectFlgNotSelect.Equals(selectFlg) Then
                    query.AddParameterWithTypeValue("DEPOSIT", OracleDbType.Double, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("DEPOSIT", OracleDbType.Double, dr.DEPOSIT)
                End If
                query.AddParameterWithTypeValue("BONUSPAYMENT", OracleDbType.Double, DBNull.Value)  'ボーナス時返済額
                query.AddParameterWithTypeValue("DUEDATE", OracleDbType.Int64, DBNull.Value)        '初回支払期限
                query.AddParameterWithTypeValue("DELFLG", OracleDbType.Char, "0")                   '削除フラグ
                query.AddParameterWithTypeValue("CREATEACCOUNT", OracleDbType.Varchar2, StringDefValue)        '作成ユーザアカウント
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, StringDefValue)        '更新ユーザアカウント
                query.AddParameterWithTypeValue("CREATEID", OracleDbType.Varchar2, MY_PROGRAMID)    '作成機能ID
                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, MY_PROGRAMID)    '更新機能ID
                query.AddParameterWithTypeValue("SELECTFLG", OracleDbType.Char, selectFlg)          '選択フラグ
                query.AddParameterWithTypeValue("INTERESTRATE", OracleDbType.Double, DBNull.Value) '利率
                '頭金支払方法区分
                If dr.IsDEPOSITPAYMENTMETHODNull Or SelectFlgNotSelect.Equals(selectFlg) Then
                    query.AddParameterWithTypeValue("DEPOSITPAYMENTMETHOD", OracleDbType.Char, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("DEPOSITPAYMENTMETHOD", OracleDbType.Char, dr.DEPOSITPAYMENTMETHOD)
                End If

                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertEstPaymentinfo_End")
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

    End Function

    ''' <summary>
    ''' 見積顧客情報の空レコードを追加
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <param name="cstType">顧客種別</param>
    ''' <returns>処理結果（成功[True]/失敗[False]）</returns>
    ''' <remarks></remarks>
    Public Function InsertEstCustomer(ByVal estimateId As Long, _
                                      ByVal cstType As String) As Boolean
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertEstCustomer_Start")
        'ログ出力 End *****************************************************************************

        Try
            ' SQL組み立て
            Dim sql As New StringBuilder

            With sql
                .Append(" INSERT /* IC3070203_014 */ ")
                .Append(" INTO ")
                .Append("     TBL_EST_CUSTOMERINFO ")
                .Append(" ( ")
                .Append("     ESTIMATEID ")          '見積管理ID
                .Append("   , CONTRACTCUSTTYPE ")    '契約顧客種別
                .Append("   , CREATEDATE ")          '作成日
                .Append("   , UPDATEDATE ")          '更新日
                .Append("   , CREATEACCOUNT ")       '作成ユーザアカウント
                .Append("   , UPDATEACCOUNT ")       '更新ユーザアカウント
                .Append("   , CREATEID ")            '作成機能ID
                .Append("   , UPDATEID ")            '更新機能ID
                .Append("   , PRIVATE_FLEET_ITEM_CD ") '個人法人項目コード
                .Append("   , NAMETITLE_CD ")        '敬称コード
                .Append("   , NAMETITLE_NAME ")      '敬称
                .Append("   , FIRST_NAME ")          'ファーストネーム
                .Append("   , MIDDLE_NAME ")         'ミドルネーム
                .Append("   , LAST_NAME ")           'ラストネーム
                .Append("   , CST_ADDRESS_1 ")       '顧客住所1
                .Append("   , CST_ADDRESS_2 ")       '顧客住所2
                .Append("   , CST_ADDRESS_3 ")       '顧客住所3
                .Append("   , CST_ADDRESS_STATE ")   '顧客住所（州）
                .Append("   , CST_ADDRESS_DISTRICT ") '顧客住所（地区）
                .Append("   , CST_ADDRESS_CITY ")    '顧客住所（市）
                .Append("   , CST_ADDRESS_LOCATION ") '顧客住所（地域）
                .Append(" ) ")
                .Append(" VALUES ")
                .Append(" ( ")
                .Append("     :ESTIMATEID ")          '見積管理ID
                .Append("   , :CONTRACTCUSTTYPE ")    '契約顧客種別
                .Append("   , SYSDATE ")              '作成日
                .Append("   , SYSDATE ")              '更新日
                .Append("   , :CREATEACCOUNT ")       '作成ユーザアカウント
                .Append("   , :UPDATEACCOUNT ")       '更新ユーザアカウント
                .Append("   , :CREATEID ")            '作成機能ID
                .Append("   , :UPDATEID ")            '更新機能ID
                .Append("   , :PRIVATE_FLEET_ITEM_CD ") '個人法人項目コード
                .Append("   , :NAMETITLE_CD ")        '敬称コード
                .Append("   , :NAMETITLE_NAME ")      '敬称
                .Append("   , :FIRST_NAME ")          'ファーストネーム
                .Append("   , :MIDDLE_NAME ")         'ミドルネーム
                .Append("   , :LAST_NAME ")           'ラストネーム
                .Append("   , :CST_ADDRESS_1 ")       '顧客住所1
                .Append("   , :CST_ADDRESS_2 ")       '顧客住所2
                .Append("   , :CST_ADDRESS_3 ")       '顧客住所3
                .Append("   , :CST_ADDRESS_STATE ")   '顧客住所（州）
                .Append("   , :CST_ADDRESS_DISTRICT ") '顧客住所（地区）
                .Append("   , :CST_ADDRESS_CITY ")    '顧客住所（市）
                .Append("   , :CST_ADDRESS_LOCATION ") '顧客住所（地域）
                .Append(" ) ")
            End With

            ' DbUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("IC3070203_014")

                query.CommandText = sql.ToString()

                ' SQLパラメータ設定
                query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Int64, estimateId)               '見積管理ID
                query.AddParameterWithTypeValue("CONTRACTCUSTTYPE", OracleDbType.Char, cstType) '契約顧客種別

                query.AddParameterWithTypeValue("CREATEACCOUNT", OracleDbType.Varchar2, StringDefValue)            '作成ユーザアカウント
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, StringDefValue)            '更新ユーザアカウント
                query.AddParameterWithTypeValue("CREATEID", OracleDbType.Varchar2, MY_PROGRAMID)        '作成機能ID
                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, MY_PROGRAMID)        '更新機能ID

                '個人法人項目コード
                query.AddParameterWithTypeValue("PRIVATE_FLEET_ITEM_CD", OracleDbType.NVarchar2, StringDefValue)
                '敬称コード
                query.AddParameterWithTypeValue("NAMETITLE_CD", OracleDbType.NVarchar2, StringDefValue)
                '敬称
                query.AddParameterWithTypeValue("NAMETITLE_NAME", OracleDbType.NVarchar2, StringDefValue)
                'ファーストネーム
                query.AddParameterWithTypeValue("FIRST_NAME", OracleDbType.NVarchar2, StringDefValue)
                'ミドルネーム
                query.AddParameterWithTypeValue("MIDDLE_NAME", OracleDbType.NVarchar2, StringDefValue)
                'ラストネーム
                query.AddParameterWithTypeValue("LAST_NAME", OracleDbType.NVarchar2, StringDefValue)
                '顧客住所1
                query.AddParameterWithTypeValue("CST_ADDRESS_1", OracleDbType.NVarchar2, StringDefValue)
                '顧客住所2
                query.AddParameterWithTypeValue("CST_ADDRESS_2", OracleDbType.NVarchar2, StringDefValue)
                '顧客住所3
                query.AddParameterWithTypeValue("CST_ADDRESS_3", OracleDbType.NVarchar2, StringDefValue)
                '顧客住所（州）
                query.AddParameterWithTypeValue("CST_ADDRESS_STATE", OracleDbType.NVarchar2, StringDefValue)
                '顧客住所（地区）
                query.AddParameterWithTypeValue("CST_ADDRESS_DISTRICT", OracleDbType.NVarchar2, StringDefValue)
                '顧客住所（市）
                query.AddParameterWithTypeValue("CST_ADDRESS_CITY", OracleDbType.NVarchar2, StringDefValue)
                '顧客住所（地域）
                query.AddParameterWithTypeValue("CST_ADDRESS_LOCATION", OracleDbType.NVarchar2, StringDefValue)

                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertEstCustomer_End")
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

    End Function

    ''' <summary>
    ''' 見積顧客情報を追加
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <param name="dr">見積顧客情報データテーブル行</param>
    ''' <returns>処理結果（成功[True]/失敗[False]）</returns>
    ''' <remarks></remarks>
    Public Function InsertEstCustomerinfo(ByVal estimateId As Long, _
                                          ByVal dr As IC3070203DataSet.IC3070203EstCustomerInfoRow) As Boolean
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertEstCustomerinfo_Start")
        'ログ出力 End *****************************************************************************

        Try
            ' データ行のNullチェック
            If dr Is Nothing Then
                Throw New ArgumentNullException("dr")
            End If

            ' SQL組み立て
            Dim sql As New StringBuilder

            With sql
                .Append(" INSERT /* IC3070203_014 */ ")
                .Append(" INTO ")
                .Append("     TBL_EST_CUSTOMERINFO ")
                .Append(" ( ")
                .Append("     ESTIMATEID ")          '見積管理ID
                .Append("   , CONTRACTCUSTTYPE ")    '契約顧客種別
                .Append("   , CUSTPART ")            '顧客区分
                .Append("   , NAME ")                '氏名
                .Append("   , SOCIALID ")            '国民番号
                .Append("   , ZIPCODE ")             '郵便番号
                .Append("   , ADDRESS ")             '住所
                .Append("   , TELNO ")               '電話番号
                .Append("   , MOBILE ")              '携帯電話番号
                .Append("   , FAXNO ")               'FAX番号
                .Append("   , EMAIL ")               'e-MAILアドレス
                .Append("   , CREATEDATE ")          '作成日
                .Append("   , UPDATEDATE ")          '更新日
                .Append("   , CREATEACCOUNT ")       '作成ユーザアカウント
                .Append("   , UPDATEACCOUNT ")       '更新ユーザアカウント
                .Append("   , CREATEID ")            '作成機能ID
                .Append("   , UPDATEID ")            '更新機能ID
                .Append("   , PRIVATE_FLEET_ITEM_CD ") '個人法人項目コード
                .Append("   , NAMETITLE_CD ")        '敬称コード
                .Append("   , NAMETITLE_NAME ")      '敬称
                .Append("   , FIRST_NAME ")          'ファーストネーム
                .Append("   , MIDDLE_NAME ")         'ミドルネーム
                .Append("   , LAST_NAME ")           'ラストネーム
                .Append("   , CST_ADDRESS_1 ")       '顧客住所1
                .Append("   , CST_ADDRESS_2 ")       '顧客住所2
                .Append("   , CST_ADDRESS_3 ")       '顧客住所3
                .Append("   , CST_ADDRESS_STATE ")   '顧客住所（州）
                .Append("   , CST_ADDRESS_DISTRICT ") '顧客住所（地区）
                .Append("   , CST_ADDRESS_CITY ")    '顧客住所（市）
                .Append("   , CST_ADDRESS_LOCATION ") '顧客住所（地域）
                .Append(" ) ")
                .Append(" VALUES ")
                .Append(" ( ")
                .Append("     :ESTIMATEID ")          '見積管理ID
                .Append("   , :CONTRACTCUSTTYPE ")    '契約顧客種別
                .Append("   , :CUSTPART ")            '顧客区分
                .Append("   , :NAME ")                '氏名
                .Append("   , :SOCIALID ")            '国民番号
                .Append("   , :ZIPCODE ")             '郵便番号
                .Append("   , :ADDRESS ")             '住所
                .Append("   , :TELNO ")               '電話番号
                .Append("   , :MOBILE ")              '携帯電話番号
                .Append("   , :FAXNO ")               'FAX番号
                .Append("   , :EMAIL ")               'e-MAILアドレス
                .Append("   , SYSDATE ")              '作成日
                .Append("   , SYSDATE ")              '更新日
                .Append("   , :CREATEACCOUNT ")       '作成ユーザアカウント
                .Append("   , :UPDATEACCOUNT ")       '更新ユーザアカウント
                .Append("   , :CREATEID ")            '作成機能ID
                .Append("   , :UPDATEID ")            '更新機能ID
                .Append("   , :PRIVATE_FLEET_ITEM_CD ") '個人法人項目コード
                .Append("   , :NAMETITLE_CD ")        '敬称コード
                .Append("   , :NAMETITLE_NAME ")      '敬称
                .Append("   , :FIRST_NAME ")          'ファーストネーム
                .Append("   , :MIDDLE_NAME ")         'ミドルネーム
                .Append("   , :LAST_NAME ")           'ラストネーム
                .Append("   , :CST_ADDRESS_1 ")       '顧客住所1
                .Append("   , :CST_ADDRESS_2 ")       '顧客住所2
                .Append("   , :CST_ADDRESS_3 ")       '顧客住所3
                .Append("   , :CST_ADDRESS_STATE ")   '顧客住所（州）
                .Append("   , :CST_ADDRESS_DISTRICT ") '顧客住所（地区）
                .Append("   , :CST_ADDRESS_CITY ")    '顧客住所（市）
                .Append("   , :CST_ADDRESS_LOCATION ") '顧客住所（地域）
                .Append(" ) ")
            End With

            ' DbUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("IC3070203_014")

                query.CommandText = sql.ToString()

                ' SQLパラメータ設定
                query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Int64, estimateId)               '見積管理ID
                query.AddParameterWithTypeValue("CONTRACTCUSTTYPE", OracleDbType.Char, dr.CONTRACTCUSTTYPE) '契約顧客種別
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

                query.AddParameterWithTypeValue("CREATEACCOUNT", OracleDbType.Varchar2, StringDefValue)            '作成ユーザアカウント
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, StringDefValue)            '更新ユーザアカウント
                query.AddParameterWithTypeValue("CREATEID", OracleDbType.Varchar2, MY_PROGRAMID)        '作成機能ID
                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, MY_PROGRAMID)        '更新機能ID

                '個人法人項目コード
                If dr.IsPRIVATE_FLEET_ITEM_CDNull Then
                    query.AddParameterWithTypeValue("PRIVATE_FLEET_ITEM_CD", OracleDbType.NVarchar2, StringDefValue)
                Else
                    query.AddParameterWithTypeValue("PRIVATE_FLEET_ITEM_CD", OracleDbType.NVarchar2, dr.PRIVATE_FLEET_ITEM_CD)
                End If

                '敬称コード
                If dr.IsNAMETITLE_CDNull Then
                    query.AddParameterWithTypeValue("NAMETITLE_CD", OracleDbType.NVarchar2, StringDefValue)
                Else
                    query.AddParameterWithTypeValue("NAMETITLE_CD", OracleDbType.NVarchar2, dr.NAMETITLE_CD)
                End If

                '敬称
                If dr.IsNAMETITLE_NAMENull Then
                    query.AddParameterWithTypeValue("NAMETITLE_NAME", OracleDbType.NVarchar2, StringDefValue)
                Else
                    query.AddParameterWithTypeValue("NAMETITLE_NAME", OracleDbType.NVarchar2, dr.NAMETITLE_NAME)
                End If

                'ファーストネーム
                If dr.IsFIRST_NAMENull Then
                    query.AddParameterWithTypeValue("FIRST_NAME", OracleDbType.NVarchar2, StringDefValue)
                Else
                    query.AddParameterWithTypeValue("FIRST_NAME", OracleDbType.NVarchar2, dr.FIRST_NAME)
                End If

                'ミドルネーム
                If dr.IsMIDDLE_NAMENull Then
                    query.AddParameterWithTypeValue("MIDDLE_NAME", OracleDbType.NVarchar2, StringDefValue)
                Else
                    query.AddParameterWithTypeValue("MIDDLE_NAME", OracleDbType.NVarchar2, dr.MIDDLE_NAME)
                End If

                'ラストネーム
                If dr.IsLAST_NAMENull Then
                    query.AddParameterWithTypeValue("LAST_NAME", OracleDbType.NVarchar2, StringDefValue)
                Else
                    query.AddParameterWithTypeValue("LAST_NAME", OracleDbType.NVarchar2, dr.LAST_NAME)
                End If

                '顧客住所1
                If dr.IsCST_ADDRESS_1Null Then
                    query.AddParameterWithTypeValue("CST_ADDRESS_1", OracleDbType.NVarchar2, StringDefValue)
                Else
                    query.AddParameterWithTypeValue("CST_ADDRESS_1", OracleDbType.NVarchar2, dr.CST_ADDRESS_1)
                End If

                '顧客住所2
                If dr.IsCST_ADDRESS_2Null Then
                    query.AddParameterWithTypeValue("CST_ADDRESS_2", OracleDbType.NVarchar2, StringDefValue)
                Else
                    query.AddParameterWithTypeValue("CST_ADDRESS_2", OracleDbType.NVarchar2, dr.CST_ADDRESS_2)
                End If

                '顧客住所3
                If dr.IsCST_ADDRESS_3Null Then
                    query.AddParameterWithTypeValue("CST_ADDRESS_3", OracleDbType.NVarchar2, StringDefValue)
                Else
                    query.AddParameterWithTypeValue("CST_ADDRESS_3", OracleDbType.NVarchar2, dr.CST_ADDRESS_3)
                End If

                '顧客住所（州）
                If dr.IsCST_ADDRESS_STATENull Then
                    query.AddParameterWithTypeValue("CST_ADDRESS_STATE", OracleDbType.NVarchar2, StringDefValue)
                Else
                    query.AddParameterWithTypeValue("CST_ADDRESS_STATE", OracleDbType.NVarchar2, dr.CST_ADDRESS_STATE)
                End If

                '顧客住所（地区）
                If dr.IsCST_ADDRESS_DISTRICTNull Then
                    query.AddParameterWithTypeValue("CST_ADDRESS_DISTRICT", OracleDbType.NVarchar2, StringDefValue)
                Else
                    query.AddParameterWithTypeValue("CST_ADDRESS_DISTRICT", OracleDbType.NVarchar2, dr.CST_ADDRESS_DISTRICT)
                End If

                '顧客住所（市）
                If dr.IsCST_ADDRESS_CITYNull Then
                    query.AddParameterWithTypeValue("CST_ADDRESS_CITY", OracleDbType.NVarchar2, StringDefValue)
                Else
                    query.AddParameterWithTypeValue("CST_ADDRESS_CITY", OracleDbType.NVarchar2, dr.CST_ADDRESS_CITY)
                End If

                '顧客住所（地域）
                If dr.IsCST_ADDRESS_LOCATIONNull Then
                    query.AddParameterWithTypeValue("CST_ADDRESS_LOCATION", OracleDbType.NVarchar2, StringDefValue)
                Else
                    query.AddParameterWithTypeValue("CST_ADDRESS_LOCATION", OracleDbType.NVarchar2, dr.CST_ADDRESS_LOCATION)
                End If

                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertEstCustomerinfo_End")
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

    End Function

    ''' <summary>
    ''' 顧客連絡時間帯登録
    ''' </summary>
    ''' <param name="cstId">顧客ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function InsertCstContactTimeslot(ByVal cstId As Decimal, _
                                             ByVal contractTime As Long) As Boolean

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertCstContactTimeslot_Start")
        'ログ出力 End *****************************************************************************

        Dim sql As New StringBuilder

        With sql
            .Append(" INSERT INTO /* IC3070203_210 */")
            .Append("   TB_M_CST_CONTACT_TIMESLOT (")
            .Append("     CST_ID,")
            .Append("     TIMESLOT_CLASS,")
            .Append("     CONTACT_TIMESLOT,")
            .Append("     ROW_CREATE_DATETIME,")
            .Append("     ROW_CREATE_ACCOUNT,")
            .Append("     ROW_CREATE_FUNCTION,")
            .Append("     ROW_UPDATE_DATETIME,")
            .Append("     ROW_UPDATE_ACCOUNT,")
            .Append("     ROW_UPDATE_FUNCTION,")
            .Append("     ROW_LOCK_VERSION")
            .Append(" ) VALUES (")
            .Append("     :CST_ID,")
            .Append("     :TIMESLOT_CLASS,")
            .Append("     :CONTACT_TIMESLOT,")
            .Append("     SYSDATE,")
            .Append("     :ROW_CREATE_ACCOUNT,")
            .Append("     :ROW_CREATE_FUNCTION,")
            .Append("     SYSDATE,")
            .Append("     :ROW_UPDATE_ACCOUNT,")
            .Append("     :ROW_UPDATE_FUNCTION,")
            .Append("     :ROW_LOCK_VERSION")
            .Append(" )")
        End With

        Using query As New DBUpdateQuery("IC3070203_210")

            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("CST_ID", OracleDbType.Decimal, cstId)
            query.AddParameterWithTypeValue("TIMESLOT_CLASS", OracleDbType.Char, "1")
            query.AddParameterWithTypeValue("CONTACT_TIMESLOT", OracleDbType.Int64, contractTime)
            query.AddParameterWithTypeValue("ROW_CREATE_ACCOUNT", OracleDbType.NVarchar2, StringDefValue)
            query.AddParameterWithTypeValue("ROW_CREATE_FUNCTION", OracleDbType.NVarchar2, MY_PROGRAMID)
            query.AddParameterWithTypeValue("ROW_UPDATE_ACCOUNT", OracleDbType.NVarchar2, StringDefValue)
            query.AddParameterWithTypeValue("ROW_UPDATE_FUNCTION", OracleDbType.NVarchar2, MY_PROGRAMID)
            query.AddParameterWithTypeValue("ROW_LOCK_VERSION", OracleDbType.Long, 0)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertCstContactTimeslot_End")
            'ログ出力 End *****************************************************************************

            ' SQL実行（結果を返却）
            If query.Execute() > 0 Then
                Return True
            Else
                Return False
            End If
        End Using

    End Function


#End Region

#Region "更新クエリ"

    ''' <summary>
    ''' 顧客情報更新
    ''' </summary>
    ''' <param name="updCst">更新情報</param>
    ''' <returns>処理結果（成功[True]/失敗[False]）</returns>
    ''' <remarks></remarks>
    Public Function UpdateCustomer(ByVal updCst As IC3070203DataSet.IC3070203CustomerRow) As Boolean

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateCustomer_Start")
        'ログ出力 End *****************************************************************************

        'SQL生成
        Dim sql As New StringBuilder
        With sql

            .Append(" UPDATE /* IC3070203_208 */ ")
            .Append("     TB_M_CUSTOMER ")
            .Append(" SET ")
            .Append("     FLEET_FLG = :FLEET_FLG,")
            .Append("     CST_SOCIALNUM = :CST_SOCIALNUM,")
            .Append("     CST_GENDER = :CST_GENDER,")
            .Append("     CST_BIRTH_DATE = :CST_BIRTH_DATE,")
            .Append("     NAMETITLE_CD = :NAMETITLE_CD,")
            .Append("     NAMETITLE_NAME = :NAMETITLE_NAME,")
            .Append("     CST_NAME = :CST_NAME,")
            .Append("     FIRST_NAME = :FIRST_NAME,")
            .Append("     MIDDLE_NAME = :MIDDLE_NAME,")
            .Append("     LAST_NAME = :LAST_NAME,")
            .Append("     NICK_NAME = :NICK_NAME,")
            .Append("     CST_COMPANY_NAME = :CST_COMPANY_NAME,")
            .Append("     FLEET_PIC_NAME = :FLEET_PIC_NAME,")
            .Append("     FLEET_PIC_DEPT = :FLEET_PIC_DEPT,")
            .Append("     FLEET_PIC_POSITION = :FLEET_PIC_POSITION,")
            .Append("     CST_ADDRESS = :CST_ADDRESS,")
            .Append("     CST_ADDRESS_1 = :CST_ADDRESS_1,")
            .Append("     CST_ADDRESS_2 = :CST_ADDRESS_2,")
            .Append("     CST_ADDRESS_3 = :CST_ADDRESS_3,")
            .Append("     CST_DOMICILE = :CST_DOMICILE,")
            .Append("     CST_COUNTRY = :CST_COUNTRY,")
            .Append("     CST_ZIPCD = :CST_ZIPCD,")
            .Append("     CST_ADDRESS_STATE = :CST_ADDRESS_STATE,")
            .Append("     CST_ADDRESS_DISTRICT = :CST_ADDRESS_DISTRICT,")
            .Append("     CST_ADDRESS_CITY = :CST_ADDRESS_CITY,")
            .Append("     CST_ADDRESS_LOCATION = :CST_ADDRESS_LOCATION,")
            .Append("     CST_PHONE = :CST_PHONE,")
            .Append("     CST_FAX = :CST_FAX,")
            .Append("     CST_MOBILE = :CST_MOBILE,")
            .Append("     CST_EMAIL_1 = :CST_EMAIL_1,")
            .Append("     CST_EMAIL_2 = :CST_EMAIL_2,")
            .Append("     CST_BIZ_PHONE = :CST_BIZ_PHONE,")
            .Append("     CST_INCOME = :CST_INCOME,")
            .Append("     CST_OCCUPATION_ID = :CST_OCCUPATION_ID,")
            .Append("     CST_OCCUPATION = :CST_OCCUPATION,")
            .Append("     MARITAL_TYPE = :MARITAL_TYPE,")
            .Append("     ENG_FLG = :ENG_FLG,")
            .Append("     DEFAULT_LANG = :DEFAULT_LANG,")
            .Append("     ROW_UPDATE_DATETIME = SYSDATE,")
            .Append("     ROW_UPDATE_ACCOUNT = :ROW_UPDATE_ACCOUNT,")
            .Append("     ROW_UPDATE_FUNCTION = :ROW_UPDATE_FUNCTION,")
            .Append("     ROW_LOCK_VERSION = ROW_LOCK_VERSION + 1,")
            .Append("     PRIVATE_FLEET_ITEM_CD = :PRIVATE_FLEET_ITEM_CD")
            .Append(" WHERE ")
            .Append("     CST_ID = :CST_ID ")

        End With

        ' DbUpdateQueryインスタンス生成
        Using query As New DBUpdateQuery("IC3070203_208")

            query.CommandText = sql.ToString()

            ' SQLパラメータ設定
            query.AddParameterWithTypeValue("FLEET_FLG", OracleDbType.NVarchar2, updCst.FLEET_FLG)
            query.AddParameterWithTypeValue("CST_SOCIALNUM", OracleDbType.NVarchar2, updCst.CST_SOCIALNUM)
            query.AddParameterWithTypeValue("CST_GENDER", OracleDbType.NVarchar2, updCst.CST_GENDER)
            query.AddParameterWithTypeValue("CST_BIRTH_DATE", OracleDbType.Date, updCst.CST_BIRTH_DATE)
            query.AddParameterWithTypeValue("NAMETITLE_CD", OracleDbType.NVarchar2, updCst.NAMETITLE_CD)
            query.AddParameterWithTypeValue("NAMETITLE_NAME", OracleDbType.NVarchar2, updCst.NAMETITLE_NAME)
            query.AddParameterWithTypeValue("CST_NAME", OracleDbType.NVarchar2, updCst.CST_NAME)
            query.AddParameterWithTypeValue("FIRST_NAME", OracleDbType.NVarchar2, updCst.FIRST_NAME)
            query.AddParameterWithTypeValue("MIDDLE_NAME", OracleDbType.NVarchar2, updCst.MIDDLE_NAME)
            query.AddParameterWithTypeValue("LAST_NAME", OracleDbType.NVarchar2, updCst.LAST_NAME)
            query.AddParameterWithTypeValue("NICK_NAME", OracleDbType.NVarchar2, updCst.NICK_NAME)
            query.AddParameterWithTypeValue("CST_COMPANY_NAME", OracleDbType.NVarchar2, updCst.CST_COMPANY_NAME)
            query.AddParameterWithTypeValue("FLEET_PIC_NAME", OracleDbType.NVarchar2, updCst.FLEET_PIC_NAME)
            query.AddParameterWithTypeValue("FLEET_PIC_DEPT", OracleDbType.NVarchar2, updCst.FLEET_PIC_DEPT)
            query.AddParameterWithTypeValue("FLEET_PIC_POSITION", OracleDbType.NVarchar2, updCst.FLEET_PIC_POSITION)
            query.AddParameterWithTypeValue("CST_ADDRESS", OracleDbType.NVarchar2, updCst.CST_ADDRESS)
            query.AddParameterWithTypeValue("CST_ADDRESS_1", OracleDbType.NVarchar2, updCst.CST_ADDRESS_1)
            query.AddParameterWithTypeValue("CST_ADDRESS_2", OracleDbType.NVarchar2, updCst.CST_ADDRESS_2)
            query.AddParameterWithTypeValue("CST_ADDRESS_3", OracleDbType.NVarchar2, updCst.CST_ADDRESS_3)
            query.AddParameterWithTypeValue("CST_DOMICILE", OracleDbType.NVarchar2, updCst.CST_DOMICILE)
            query.AddParameterWithTypeValue("CST_COUNTRY", OracleDbType.NVarchar2, updCst.CST_COUNTRY)
            query.AddParameterWithTypeValue("CST_ZIPCD", OracleDbType.NVarchar2, updCst.CST_ZIPCD)
            query.AddParameterWithTypeValue("CST_ADDRESS_STATE", OracleDbType.NVarchar2, updCst.CST_ADDRESS_STATE)
            query.AddParameterWithTypeValue("CST_ADDRESS_DISTRICT", OracleDbType.NVarchar2, updCst.CST_ADDRESS_DISTRICT)
            query.AddParameterWithTypeValue("CST_ADDRESS_CITY", OracleDbType.NVarchar2, updCst.CST_ADDRESS_CITY)
            query.AddParameterWithTypeValue("CST_ADDRESS_LOCATION", OracleDbType.NVarchar2, updCst.CST_ADDRESS_LOCATION)
            query.AddParameterWithTypeValue("CST_PHONE", OracleDbType.NVarchar2, updCst.CST_PHONE)
            query.AddParameterWithTypeValue("CST_FAX", OracleDbType.NVarchar2, updCst.CST_FAX)
            query.AddParameterWithTypeValue("CST_MOBILE", OracleDbType.NVarchar2, updCst.CST_MOBILE)
            query.AddParameterWithTypeValue("CST_EMAIL_1", OracleDbType.NVarchar2, updCst.CST_EMAIL_1)
            query.AddParameterWithTypeValue("CST_EMAIL_2", OracleDbType.NVarchar2, updCst.CST_EMAIL_2)
            query.AddParameterWithTypeValue("CST_BIZ_PHONE", OracleDbType.NVarchar2, updCst.CST_BIZ_PHONE)
            query.AddParameterWithTypeValue("CST_INCOME", OracleDbType.NVarchar2, updCst.CST_INCOME)
            query.AddParameterWithTypeValue("CST_OCCUPATION_ID", OracleDbType.Int64, updCst.CST_OCCUPATION_ID)
            query.AddParameterWithTypeValue("CST_OCCUPATION", OracleDbType.NVarchar2, updCst.CST_OCCUPATION)
            query.AddParameterWithTypeValue("MARITAL_TYPE", OracleDbType.NVarchar2, updCst.MARITAL_TYPE)
            query.AddParameterWithTypeValue("ENG_FLG", OracleDbType.NVarchar2, updCst.ENG_FLG)
            query.AddParameterWithTypeValue("DEFAULT_LANG", OracleDbType.NVarchar2, updCst.DEFAULT_LANG)
            query.AddParameterWithTypeValue("ROW_UPDATE_ACCOUNT", OracleDbType.NVarchar2, StringDefValue)
            query.AddParameterWithTypeValue("ROW_UPDATE_FUNCTION", OracleDbType.NVarchar2, MY_PROGRAMID)
            query.AddParameterWithTypeValue("PRIVATE_FLEET_ITEM_CD", OracleDbType.NVarchar2, updCst.PRIVATE_FLEET_ITEM_CD)
            query.AddParameterWithTypeValue("CST_ID", OracleDbType.Decimal, updCst.CST_ID)


            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateCustomer_End")
            'ログ出力 End *****************************************************************************
            ' SQL実行（結果を返却）TblCodeMstcarName
            If query.Execute() > 0 Then
                Return True
            Else
                Me.prpResultId = ErrCodeDBUpdate + TblCodeCustomer
                Throw New ArgumentException("", "customer_id")
            End If
        End Using

    End Function

    ''' <summary>
    ''' 見積顧客更新
    ''' </summary>
    ''' <param name="dr">見積顧客情報データテーブル</param>
    ''' <returns>処理結果（成功[True]/失敗[False]）</returns>
    ''' <remarks></remarks>
    Public Function UpdateEstCustomerinfo(ByVal dr As IC3070203DataSet.IC3070203EstCustomerInfoRow) As Boolean

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateEstCustomerinfo_Start")
        'ログ出力 End *****************************************************************************

        'SQL生成
        Dim sql As New StringBuilder
        With sql

            .Append(" UPDATE /* IC3070203_209 */ ")
            .Append("     TBL_EST_CUSTOMERINFO ")
            .Append(" SET ")
            .Append("     CUSTPART = :CUSTPART,")                            '顧客区分
            .Append("     NAME = :NAME,")                                    '氏名
            .Append("     SOCIALID = :SOCIALID,")                            '国民番号
            .Append("     ZIPCODE = :ZIPCODE,")                              '郵便番号
            .Append("     ADDRESS = :ADDRESS,")                              '住所
            .Append("     TELNO = :TELNO,")                                  '電話番号
            .Append("     MOBILE = :MOBILE,")                                '携帯電話番号
            .Append("     FAXNO = :FAXNO,")                                  'FAX番号
            .Append("     EMAIL = :EMAIL,")                                  'e-MAILアドレス
            .Append("     UPDATEDATE = SYSDATE,")                            '更新日
            .Append("     UPDATEACCOUNT = :UPDATEACCOUNT,")                  '更新ユーザアカウント
            .Append("     UPDATEID = :UPDATEID,")                            '更新機能ID
            .Append("     PRIVATE_FLEET_ITEM_CD = :PRIVATE_FLEET_ITEM_CD,")  '個人法人項目コード
            .Append("     NAMETITLE_CD = :NAMETITLE_CD,")                    '敬称コード
            .Append("     NAMETITLE_NAME = :NAMETITLE_NAME,")                '敬称
            .Append("     FIRST_NAME = :FIRST_NAME,")                        'ファーストネーム
            .Append("     MIDDLE_NAME = :MIDDLE_NAME,")                      'ミドルネーム
            .Append("     LAST_NAME = :LAST_NAME,")                          'ラストネーム
            .Append("     CST_ADDRESS_1 = :CST_ADDRESS_1,")                  '顧客住所1
            .Append("     CST_ADDRESS_2 = :CST_ADDRESS_2,")                  '顧客住所2
            .Append("     CST_ADDRESS_3 = :CST_ADDRESS_3,")                  '顧客住所3
            .Append("     CST_ADDRESS_STATE = :CST_ADDRESS_STATE,")          '顧客住所（州）
            .Append("     CST_ADDRESS_DISTRICT = :CST_ADDRESS_DISTRICT,")    '顧客住所（地区）
            .Append("     CST_ADDRESS_CITY = :CST_ADDRESS_CITY,")            '顧客住所（市）
            .Append("     CST_ADDRESS_LOCATION = :CST_ADDRESS_LOCATION")     '顧客住所（地域）
            .Append(" WHERE ")
            .Append("     ESTIMATEID = :ESTIMATEID ")                        '見積管理ID
            .Append(" AND CONTRACTCUSTTYPE = :CONTRACTCUSTTYPE")             '契約顧客種別

        End With

        ' DbUpdateQueryインスタンス生成
        Using query As New DBUpdateQuery("IC3070203_209")

            query.CommandText = sql.ToString()

            ' SQLパラメータ設定
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

            query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, StringDefValue)          '更新ユーザアカウント
            query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, MY_PROGRAMID)      '更新機能ID

            query.AddParameterWithTypeValue("PRIVATE_FLEET_ITEM_CD", OracleDbType.NVarchar2, dr.PRIVATE_FLEET_ITEM_CD)    ' 個人法人項目コード
            query.AddParameterWithTypeValue("NAMETITLE_CD", OracleDbType.NVarchar2, dr.NAMETITLE_CD)                      '敬称コード
            query.AddParameterWithTypeValue("NAMETITLE_NAME", OracleDbType.NVarchar2, dr.NAMETITLE_NAME)                  '敬称
            query.AddParameterWithTypeValue("FIRST_NAME", OracleDbType.NVarchar2, dr.FIRST_NAME)                          'ファーストネーム
            query.AddParameterWithTypeValue("MIDDLE_NAME", OracleDbType.NVarchar2, dr.MIDDLE_NAME)                        'ミドルネーム
            query.AddParameterWithTypeValue("LAST_NAME", OracleDbType.NVarchar2, dr.LAST_NAME)                            'ラストネーム
            query.AddParameterWithTypeValue("CST_ADDRESS_1", OracleDbType.NVarchar2, dr.CST_ADDRESS_1)                    '顧客住所1
            query.AddParameterWithTypeValue("CST_ADDRESS_2", OracleDbType.NVarchar2, dr.CST_ADDRESS_2)                    '顧客住所2
            query.AddParameterWithTypeValue("CST_ADDRESS_3", OracleDbType.NVarchar2, dr.CST_ADDRESS_3)                    '顧客住所3
            query.AddParameterWithTypeValue("CST_ADDRESS_STATE", OracleDbType.NVarchar2, dr.CST_ADDRESS_STATE)            '顧客住所（州）
            query.AddParameterWithTypeValue("CST_ADDRESS_DISTRICT", OracleDbType.NVarchar2, dr.CST_ADDRESS_DISTRICT)      '顧客住所（州）
            query.AddParameterWithTypeValue("CST_ADDRESS_CITY", OracleDbType.NVarchar2, dr.CST_ADDRESS_CITY)              '顧客住所（市）
            query.AddParameterWithTypeValue("CST_ADDRESS_LOCATION", OracleDbType.NVarchar2, dr.CST_ADDRESS_LOCATION)      '顧客住所（地域）
            query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Int64, dr.ESTIMATEID)            '見積管理ID
            query.AddParameterWithTypeValue("CONTRACTCUSTTYPE", OracleDbType.Char, dr.CONTRACTCUSTTYPE) '契約顧客種別

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateEstCustomerinfo_End")
            'ログ出力 End *****************************************************************************
            ' SQL実行（結果を返却）
            If query.Execute() > 0 Then
                Return True
            Else
                Me.prpResultId = ErrCodeDBUpdate + TblCodeEstCustomerInfo
                Throw New ArgumentException("", "ESTIMATEID")
            End If
        End Using

    End Function


    ''' <summary>
    ''' 見積情報更新
    ''' </summary>
    ''' <returns>処理結果（成功[True]/失敗[False]）</returns>
    ''' <remarks></remarks>
    Public Function UpdateEstimateinfo(ByVal estimateId As Long, _
                                       ByVal delDate As Nullable(Of Date), _
                                       ByVal discountPrice As Nullable(Of Double), _
                                       ByVal memo As String, _
                                       ByVal estPrintDate As Nullable(Of Date), _
                                       ByVal cntPrintFlg As String, _
                                       ByVal estActFlg As String, _
                                       ByVal estChgFlg As String) As Boolean

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateEstimateinfo_Start")
        'ログ出力 End *****************************************************************************

        'SQL生成
        Dim sql As New StringBuilder
        With sql

            .Append(" UPDATE /* IC3070203_221 */ ")
            .Append("     TBL_ESTIMATEINFO ")
            .Append(" SET ")
            .Append("     DELIDATE = :DELIDATE,")                            '納車予定日
            .Append("     DISCOUNTPRICE = :DISCOUNTPRICE,")                  '値引き額
            .Append("     MEMO = :MEMO,")                                    'メモ
            .Append("     ESTPRINTDATE = :ESTPRINTDATE,")                    '見積印刷日
            .Append("     CONTPRINTFLG = :CONTPRINTFLG,")                    '契約書印刷フラグ
            .Append("     UPDATEDATE = SYSDATE, ")                           '更新日
            .Append("     UPDATEACCOUNT = :UPDATEACCOUNT,")                  '更新ユーザアカウント
            .Append("     UPDATEID = :UPDATEID, ")                           '更新機能ID
            .Append("     EST_ACT_FLG = :EST_ACT_FLG, ")                     '見積実績フラグ
            '更新： 2014/05/28 TCS 安田 受注時説明機能開発（受注後工程スケジュール） START
            .Append("    CONTRACT_COND_CHG_FLG = :CONTRACT_COND_CHG_FLG ")   '契約条件変更フラグ
            '更新： 2014/05/28 TCS 安田 受注時説明機能開発（受注後工程スケジュール） END
            .Append(" WHERE ")
            .Append("     ESTIMATEID = :ESTIMATEID ")                        '見積管理ID

        End With

        ' DbUpdateQueryインスタンス生成
        Using query As New DBUpdateQuery("IC3070203_221")

            query.CommandText = sql.ToString()

            ' SQLパラメータ設定
            '納車予定日
            If delDate Is Nothing Then
                query.AddParameterWithTypeValue("DELIDATE", OracleDbType.Date, DBNull.Value)
            Else
                query.AddParameterWithTypeValue("DELIDATE", OracleDbType.Date, delDate)
            End If
            '値引き額
            If discountPrice Is Nothing Then
                query.AddParameterWithTypeValue("DISCOUNTPRICE", OracleDbType.Double, DBNull.Value)
            Else
                query.AddParameterWithTypeValue("DISCOUNTPRICE", OracleDbType.Double, discountPrice)
            End If
            'メモ
            If String.IsNullOrEmpty(memo) Then
                query.AddParameterWithTypeValue("MEMO", OracleDbType.NVarchar2, DBNull.Value)
            Else
                query.AddParameterWithTypeValue("MEMO", OracleDbType.NVarchar2, memo)
            End If
            '見積印刷日
            If estPrintDate Is Nothing Then
                query.AddParameterWithTypeValue("ESTPRINTDATE", OracleDbType.Date, DBNull.Value)
            Else
                query.AddParameterWithTypeValue("ESTPRINTDATE", OracleDbType.Date, estPrintDate)
            End If
            '契約書印刷フラグ
            query.AddParameterWithTypeValue("CONTPRINTFLG", OracleDbType.Char, cntPrintFlg)

            query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, StringDefValue)        '更新ユーザアカウント
            query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, MY_PROGRAMID)    '更新機能ID
            query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Int64, estimateId)       '見積管理ID

            '見積実績フラグ
            query.AddParameterWithTypeValue("EST_ACT_FLG", OracleDbType.Char, estActFlg)

            '更新： 2014/05/28 TCS 安田 受注時説明機能開発（受注後工程スケジュール） START
            '契約条件変更フラグ
            query.AddParameterWithTypeValue("CONTRACT_COND_CHG_FLG", OracleDbType.NVarchar2, estChgFlg)
            '更新： 2014/05/28 TCS 安田 受注時説明機能開発（受注後工程スケジュール） END

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateEstimateinfo_End")
            'ログ出力 End *****************************************************************************
            ' SQL実行（結果を返却）
            If query.Execute() > 0 Then
                Return True
            Else
                Me.prpResultId = ErrCodeDBUpdate + TblCodeEstimateInfo
                Throw New ArgumentException("", "ESTIMATEID")
            End If
        End Using

    End Function


    ''' <summary>
    ''' 見積保険情報更新
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <param name="insudvs">保険区分</param>
    ''' <returns>処理結果（成功[True]/失敗[False]）</returns>
    ''' <remarks></remarks>
    Public Function UpdateEstInsuranceinfo(ByVal estimateId As Long, _
                                           ByVal insudvs As String) As Boolean

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateEstInsuranceinfo_Start")
        'ログ出力 End *****************************************************************************

        'SQL生成
        Dim sql As New StringBuilder
        With sql

            .Append(" UPDATE /* IC3070203_213 */ ")
            .Append("     TBL_EST_INSURANCEINFO ")
            .Append(" SET ")
            .Append("     INSUDVS = :INSUDVS, ")                           '保険区分
            .Append("     UPDATEDATE = SYSDATE,")                          '更新日
            .Append("     UPDATEACCOUNT = :UPDATEACCOUNT,")                '更新ユーザアカウント
            .Append("     UPDATEID = :UPDATEID ")                          '更新機能ID
            .Append(" WHERE ")
            .Append("     ESTIMATEID = :ESTIMATEID ")                      '見積管理ID

        End With

        ' DbUpdateQueryインスタンス生成
        Using query As New DBUpdateQuery("IC3070203_213")

            query.CommandText = sql.ToString()

            ' SQLパラメータ設定
            query.AddParameterWithTypeValue("INSUDVS", OracleDbType.Char, insudvs)              '保険区分
            query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, StringDefValue)        '更新ユーザアカウント
            query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, MY_PROGRAMID)    '更新機能ID
            query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Int64, estimateId)       '見積管理ID

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateEstInsuranceinfo_End")
            'ログ出力 End *****************************************************************************
            ' SQL実行（結果を返却）
            If query.Execute() > 0 Then
                Return True
            Else
                Me.prpResultId = ErrCodeDBUpdate + TblCodeEstInsuranceInfo
                Throw New ArgumentException("", "ESTIMATEID")
            End If
        End Using


    End Function


    ''' <summary>
    ''' 見積車両オプション更新
    ''' </summary>
    ''' <param name="dr">見積車両オプション情報データテーブル</param>
    ''' <returns>処理結果（成功[True]/失敗[False]）</returns>
    ''' <remarks></remarks>
    Public Function UpdVcloptionInfo(ByVal dr As IC3070203DataSet.IC3070203EstVclOptionInfoRow) As Boolean

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdVcloptionInfo_Start")
        'ログ出力 End *****************************************************************************

        'SQL生成
        Dim sql As New StringBuilder
        With sql

            .Append(" UPDATE /* IC3070203_005 */ ")
            .Append("     TBL_EST_VCLOPTIONINFO ")
            .Append(" SET ")
            .Append("     OPTIONNAME = :OPTIONNAME,")                        'オプション名
            .Append("     PRICE = :PRICE,")                                  '価格
            .Append("     INSTALLCOST = :INSTALLCOST,")                      '取付費用
            .Append("     UPDATEDATE = SYSDATE,")                            '更新日
            .Append("     UPDATEACCOUNT = :UPDATEACCOUNT,")                  '更新ユーザアカウント
            .Append("     UPDATEID = :UPDATEID ")                            '更新機能ID
            .Append(" WHERE ")
            .Append("     ESTIMATEID = :ESTIMATEID ")                        '見積管理ID
            .Append(" AND OPTIONPART = :OPTIONPART ")                        'オプション区分
            .Append(" AND OPTIONCODE = :OPTIONCODE ")                        'オプションコード

        End With

        ' DbUpdateQueryインスタンス生成
        Using query As New DBUpdateQuery("IC3070203_005")

            query.CommandText = sql.ToString()

            ' SQLパラメータ設定
            query.AddParameterWithTypeValue("OPTIONNAME", OracleDbType.Char, dr.OPTIONNAME)        'オプション名
            query.AddParameterWithTypeValue("PRICE", OracleDbType.Double, dr.PRICE)             '価格
            '取付費用
            If dr.IsINSTALLCOSTNull Then
                query.AddParameterWithTypeValue("INSTALLCOST", OracleDbType.Double, DBNull.Value)
            Else
                query.AddParameterWithTypeValue("INSTALLCOST", OracleDbType.Double, dr.INSTALLCOST)
            End If
            query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, StringDefValue)            '更新ユーザアカウント
            query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, MY_PROGRAMID)        '更新機能ID
            query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Int64, dr.ESTIMATEID)        '見積管理ID
            query.AddParameterWithTypeValue("OPTIONPART", OracleDbType.NVarchar2, dr.OPTIONPART)    'オプション区分
            query.AddParameterWithTypeValue("OPTIONCODE", OracleDbType.NVarchar2, dr.OPTIONCODE)    'オプションコード

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateEst_Insuranceinfo_End")
            'ログ出力 End *****************************************************************************
            ' SQL実行（結果を返却）
            If query.Execute() > 0 Then
                Return True
            Else
                Me.prpResultId = ErrCodeDBUpdate + TblCodeEstVclOptionInfo
                Throw New ArgumentException("", "ESTIMATEID")
            End If
        End Using

    End Function

    ''' <summary>
    ''' 見積支払情報更新(選択フラグを0にする)
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <returns>処理結果（成功[True]/失敗[False]）</returns>
    ''' <remarks></remarks>
    Public Function UpdateEstPaymentinfoSelectFlg(ByVal estimateId As Long) As Boolean

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateEstPaymentinfoSelectFlg_Start")
        'ログ出力 End *****************************************************************************

        'SQL生成
        Dim sql As New StringBuilder
        With sql

            .Append(" UPDATE /* IC3070203_215 */ ")
            .Append("     TBL_EST_PAYMENTINFO ")
            .Append(" SET ")
            .Append("     UPDATEDATE = SYSDATE, ")              '更新日
            .Append("     UPDATEACCOUNT = :UPDATEACCOUNT, ")    '更新ユーザアカウント
            .Append("     UPDATEID = :UPDATEID, ")              '更新機能ID
            .Append("     SELECTFLG = :SELECTFLG ")             '選択フラグ
            .Append(" WHERE ")
            .Append("     ESTIMATEID = :ESTIMATEID")            '見積管理ID

        End With

        ' DbUpdateQueryインスタンス生成
        Using query As New DBUpdateQuery("IC3070203_215")

            query.CommandText = sql.ToString()

            ' SQLパラメータ設定
            query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, StringDefValue)            '更新ユーザアカウント
            query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, MY_PROGRAMID)        '更新機能ID
            query.AddParameterWithTypeValue("SELECTFLG", OracleDbType.Char, SelectFlgNotSelect)     '選択フラグ
            query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Int64, estimateId)           '見積管理ID

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateEstPaymentinfoSelectFlg_End")
            'ログ出力 End *****************************************************************************
            ' SQL実行（結果を返却）
            If query.Execute() > 0 Then
                Return True
            Else
                Me.prpResultId = ErrCodeDBUpdate + TblCodeEstPaymentInfo
                Throw New ArgumentException("", "ESTIMATEID")
            End If
        End Using

    End Function

    ''' <summary>
    ''' 見積支払情報更新
    ''' </summary>
    ''' <param name="dr">見積支払情報データテーブル</param>
    ''' <param name="selectFlg">選択フラグ</param>
    ''' <returns>処理結果（成功[True]/失敗[False]）</returns>
    ''' <remarks></remarks>
    Public Function UpdateEstPaymentinfo(ByVal dr As IC3070203DataSet.IC3070203EstPaymentInfoRow, _
                                         ByVal selectFlg As String) As Boolean

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateEstPaymentinfo_Start")
        'ログ出力 End *****************************************************************************

        'SQL生成
        Dim sql As New StringBuilder
        With sql

            .Append(" UPDATE /* IC3070203_215 */ ")
            .Append("     TBL_EST_PAYMENTINFO ")
            .Append(" SET ")
            .Append("     DEPOSIT = :DEPOSIT, ")                            '頭金
            .Append("     DEPOSITPAYMENTMETHOD = :DEPOSITPAYMENTMETHOD, ")  '頭金支払方法区分
            .Append("     UPDATEDATE = SYSDATE, ")                          '更新日
            .Append("     UPDATEACCOUNT = :UPDATEACCOUNT, ")                '更新ユーザアカウント
            .Append("     UPDATEID = :UPDATEID, ")                          '更新機能ID
            .Append("     SELECTFLG = :SELECTFLG ")                         '選択フラグ
            .Append(" WHERE ")
            .Append("     ESTIMATEID = :ESTIMATEID ")                       '見積管理ID
            .Append(" AND PAYMENTMETHOD = :PAYMENTMETHOD ")                 '支払方法区分

        End With

        ' DbUpdateQueryインスタンス生成
        Using query As New DBUpdateQuery("IC3070203_215")

            query.CommandText = sql.ToString()

            ' SQLパラメータ設定
            '頭金
            If dr.IsDEPOSITNull Then
                query.AddParameterWithTypeValue("DEPOSIT", OracleDbType.Char, DBNull.Value)
            Else
                query.AddParameterWithTypeValue("DEPOSIT", OracleDbType.Char, dr.DEPOSIT)
            End If
            '頭金支払方法区分
            If dr.IsDEPOSITPAYMENTMETHODNull Then
                query.AddParameterWithTypeValue("DEPOSITPAYMENTMETHOD", OracleDbType.NVarchar2, DBNull.Value)
            Else
                query.AddParameterWithTypeValue("DEPOSITPAYMENTMETHOD", OracleDbType.NVarchar2, dr.DEPOSITPAYMENTMETHOD)
            End If
            query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, StringDefValue)                '更新ユーザアカウント
            query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, MY_PROGRAMID)            '更新機能ID
            query.AddParameterWithTypeValue("SELECTFLG", OracleDbType.Char, selectFlg)                  '選択フラグ
            query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Int64, dr.ESTIMATEID)            '見積管理ID
            query.AddParameterWithTypeValue("PAYMENTMETHOD", OracleDbType.NVarchar2, dr.PAYMENTMETHOD)  '支払方法区分

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateEstPaymentinfo_End")
            'ログ出力 End *****************************************************************************
            ' SQL実行（結果を返却）
            If query.Execute() > 0 Then
                Return True
            Else
                Me.prpResultId = ErrCodeDBUpdate + TblCodeEstPaymentInfo
                Throw New ArgumentException("", "ESTIMATEID")
            End If
        End Using

    End Function


#End Region

#Region "選択クエリ"
    ''' <summary>
    ''' 見積管理IDTBLから更新対象の見積管理IDのロックを取得します。
    ''' </summary>
    ''' <remarks></remarks>
    Public Function GetEstimateinfoLock(ByVal estimateId As Long) As Boolean

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetEstimateinfoLock_Start")
        'ログ出力 End *****************************************************************************

        Try
            Dim env As New SystemEnvSetting
            Dim sqlForUpdate As String = " FOR UPDATE WAIT " + env.GetLockWaitTime()

            ' SQL組み立て
            Dim sql As New StringBuilder
            With sql
                .Append(" SELECT ")
                .Append("   /* IC3070203_206 */ ")
                .Append("  1 ")
                .Append(" FROM ")
                .Append("   TBL_ESTIMATEINFO ")
                .Append(" WHERE ")
                .Append("   ESTIMATEID = :ESTIMATEID ")
                .Append(sqlForUpdate)
            End With

            ' DbSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of IC3070203DataSet.IC3070203EstSeqDataTable)("IC3070203_206")

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Int64, estimateId)

                ' SQL実行
                Dim retDT As IC3070203DataSet.IC3070203EstSeqDataTable = query.GetData()

                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetEstimateinfoLock_End")
                'ログ出力 End *****************************************************************************
                ' 結果を返却
                If retDT.Rows.Count > 0 Then
                    Return True
                Else
                    Return False
                End If
            End Using
        Catch ex As Exception
            Me.prpResultId = ErrCodeDBOverlap + TblCodeEstimateInfo
            Throw
        End Try

    End Function

    ''' <summary>
    ''' 顧客TBLから更新対象の顧客IDのロックを取得します。
    ''' </summary>
    ''' <remarks></remarks>
    Public Function SelCstIdLock(ByVal cstId As Decimal) As Boolean

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SelCstIdLock_Start")
        'ログ出力 End *****************************************************************************

        Try
            Dim env As New SystemEnvSetting
            Dim sqlForUpdate As String = " FOR UPDATE WAIT " + env.GetLockWaitTime()

            ' SQL組み立て
            Dim sql As New StringBuilder
            With sql
                .Append(" SELECT ")
                .Append("   /* IC3070203_207 */ ")
                .Append("  1 ")
                .Append(" FROM ")
                .Append("   TB_M_CUSTOMER ")
                .Append(" WHERE ")
                .Append("   CST_ID = :CST_ID ")
                .Append(sqlForUpdate)
            End With

            ' DbSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of IC3070203DataSet.IC3070203EstSeqDataTable)("IC3070203_207")

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("CST_ID", OracleDbType.Decimal, cstId)

                ' SQL実行
                Dim retDT As IC3070203DataSet.IC3070203EstSeqDataTable = query.GetData()

                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SelCstIdLock_End")
                'ログ出力 End *****************************************************************************
                ' 結果を返却
                If retDT.Rows.Count > 0 Then
                    Return True
                Else
                    Return False
                End If
            End Using
        Catch ex As Exception
            Me.prpResultId = ErrCodeDBOverlap + TblCodeEstimateInfo
            Throw
        End Try

    End Function


    ''' <summary>
    ''' 顧客マスタ取得
    ''' </summary>
    ''' <param name="cstId">顧客ID</param>
    ''' <returns>取得結果</returns>
    ''' <remarks>更新対象のデータを取得する</remarks>
    Public Function SelCustomer(ByVal cstId As Decimal) As IC3070203DataSet.IC3070203CustomerDataTable

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SelCustomer_Start")
        'ログ出力 End *****************************************************************************

        Try
            ' SQL組み立て
            Dim sql As New StringBuilder
            With sql
                .Append(" SELECT /* IC3070203_218 */ ")
                .Append("     CST_ID, ")              '顧客ID
                .Append("     DMS_CST_CD, ")          '基幹顧客コード 
                .Append("     DMS_CST_CD_DISP,")      '基幹顧客コード（表示用）
                .Append("     NEWCST_CD,")            '未取引客コード 
                .Append("     ORGCST_CD,")            '自社客コード 
                .Append("     FLEET_FLG,")            '法人フラグ 
                .Append("     FLEET_PIC_NAME,")       '法人担当者名
                .Append("     FLEET_PIC_DEPT,")       '法人担当者所属部署
                .Append("     FLEET_PIC_POSITION,")   '法人担当者役職
                .Append("     CST_SOCIALNUM_TYPE,")   '顧客識別番号区分
                .Append("     CST_SOCIALNUM,")        '顧客識別番号 
                .Append("     CST_NAME,")             '顧客氏名 
                .Append("     CST_NAME_SEARCH,")      '顧客氏名 （検索用）
                .Append("     CST_NAME_SEARCH_REV,")  '顧客氏名 （検索用・逆順）
                .Append("     NAMETITLE_CD,")         '敬称コード
                .Append("     NAMETITLE_NAME,")       '敬称
                .Append("     FIRST_NAME,")           'ファーストネーム
                .Append("     MIDDLE_NAME,")          'ミドルネーム
                .Append("     LAST_NAME,")            'ラストネーム
                .Append("     FIRST_NAME_KANA,")      'お客様カナ名
                .Append("     LAST_NAME_KANA,")       'お客様カナ姓
                .Append("     NICK_NAME,")            'ニックネーム
                .Append("     CST_GENDER,")           '性別区分
                .Append("     CST_DOMICILE,")         '本籍
                .Append("     CST_COUNTRY,")          '国籍
                .Append("     CST_ZIPCD,")            '顧客郵便番号 
                .Append("     CST_ZIPCD_SEARCH,")     '顧客郵便番号 （検索用）
                .Append("     CST_ADDRESS,")          '顧客住所 
                .Append("     CST_ADDRESS_1,")        '顧客住所1 
                .Append("     CST_ADDRESS_2,")        '顧客住所2 
                .Append("     CST_ADDRESS_3,")        '顧客住所3 
                .Append("     CST_ADDRESS_STATE,")    '顧客住所（州）
                .Append("     CST_ADDRESS_DISTRICT,") '顧客住所（地区）
                .Append("     CST_ADDRESS_CITY,")     '顧客住所（市）
                .Append("     CST_ADDRESS_LOCATION,") '顧客住所（地域）
                .Append("     CST_PHONE,")            '顧客電話番号 
                .Append("     CST_PHONE_SEARCH,")     '顧客電話番号 (検索用）
                .Append("     CST_PHONE_SEARCH_REV,") '顧客電話番号 （検索用・逆順）
                .Append("     CST_MOBILE,")             '顧客携帯電話番号 
                .Append("     CST_MOBILE_SEARCH,")      '顧客携帯電話番号 (検索用）
                .Append("     CST_MOBILE_SEARCH_REV, ") '顧客携帯電話番号 (検索用・逆順）
                .Append("     CST_FAX,")                '顧客FAX番号 
                .Append("     CST_COMPANY_NAME,")       '顧客会社名
                .Append("     CST_BIZ_PHONE,")       '顧客勤め先電話番号 
                .Append("     CST_EMAIL_1,")         '顧客EMAILアドレス1
                .Append("     CST_EMAIL_2,")         '顧客EMAILアドレス2 
                .Append("     CST_BIRTH_DATE,")      '顧客誕生日
                .Append("     CST_INCOME,")          '顧客収入
                .Append("     CST_OCCUPATION_ID,")   '顧客職業ID
                .Append("     CST_OCCUPATION,")      '顧客職業
                .Append("     MARITAL_TYPE,")        '結婚区分
                .Append("     ENG_FLG,")             '英語フラグ
                .Append("     DMS_TYPE,")            '基幹識別区分
                .Append("     DMS_TAKEIN_DATETIME,") '基幹取込日時
                .Append("     UPDATE_FUNCTION_JUDGE,") '更新機能判定
                .Append("     CST_BIRTH_DATE_SEARCH,") '顧客誕生日 (検索用）
                .Append("     CST_SOCIALNUM_SEARCH,")  '顧客識別番号 (検索用）
                .Append("     DEFAULT_LANG,")          'デフォルト言語
                .Append("     CST_REG_STATUS,")        '顧客登録状態
                .Append("     PRIVATE_FLEET_ITEM_CD")  '個人法人項目コード
                .Append(" ")
                .Append(" FROM ")
                .Append("     TB_M_CUSTOMER ")
                .Append(" WHERE ")
                .Append("     CST_ID = :CST_ID ")       '顧客ID
            End With

            ' DbSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of IC3070203DataSet.IC3070203CustomerDataTable)("IC3070203_218")

                query.CommandText = sql.ToString()

                ' SQLパラメータ設定
                query.AddParameterWithTypeValue("CST_ID", OracleDbType.Decimal, cstId)       '顧客ID

                ' SQL実行
                Dim retDT As IC3070203DataSet.IC3070203CustomerDataTable = query.GetData()

                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SelCustomer_End")
                'ログ出力 End *****************************************************************************
                ' 結果を返却
                Return retDT

            End Using
        Catch ex As Exception
            Me.prpResultId = ErrCodeDBOverlap + TblCodeEstimateInfo
            Throw
        End Try

    End Function

    ''' <summary>
    ''' 見積車両オプション存在チェック
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <param name="optionPart">オプション区分</param>
    ''' <param name="optionCode">オプションコード</param>
    ''' <returns>処理結果（存在する[True]/存在しない[False]）</returns>
    ''' <remarks></remarks>
    Public Function SelectEstVcloptioninfo(ByVal estimateId As Long, _
                                           ByVal optionPart As String, _
                                           ByVal optionCode As String) As IC3070203DataSet.IC3070203EstVclOptionInfoDataTable

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SelectEstVcloptioninfo_Start")
        'ログ出力 End *****************************************************************************

        Try
            ' SQL組み立て
            Dim sql As New StringBuilder
            With sql
                .Append(" SELECT /* IC3070203_220 */ ")
                .Append("     ESTIMATEID, ")                    '見積管理ID
                .Append("     OPTIONPART,")                     'オプション区分
                .Append("     OPTIONCODE,")                     'オプションコード
                .Append("     OPTIONNAME,")                     'オプション名
                .Append("     PRICE,")                          '価格
                .Append("     INSTALLCOST")                     '取付費用
                .Append(" FROM ")
                .Append("     TBL_EST_VCLOPTIONINFO ")
                .Append(" WHERE ")
                .Append("     ESTIMATEID = :ESTIMATEID ")       '見積管理ID
                .Append("  AND OPTIONPART = :OPTIONPART ")       'オプション区分
                .Append("  AND OPTIONCODE = :OPTIONCODE ")       'オプションコード
            End With

            ' DbSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of IC3070203DataSet.IC3070203EstVclOptionInfoDataTable)("IC3070203_220")

                query.CommandText = sql.ToString()

                ' SQLパラメータ設定
                query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Int64, estimateId)       '見積管理ID
                query.AddParameterWithTypeValue("OPTIONPART", OracleDbType.Char, optionPart)            'オプション区分
                query.AddParameterWithTypeValue("OPTIONCODE", OracleDbType.NVarchar2, optionCode)       'オプションコード

                ' SQL実行
                Dim retDT As IC3070203DataSet.IC3070203EstVclOptionInfoDataTable = query.GetData()

                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SelectEstVcloptioninfo_End")
                'ログ出力 End *****************************************************************************
                ' 結果を返却
                Return retDT

            End Using
        Catch ex As Exception
            Me.prpResultId = ErrCodeDBOverlap + TblCodeEstimateInfo
            Throw
        End Try

    End Function


    ''' <summary>
    ''' 見積顧客存在チェック
    ''' </summary>
    ''' <param name="estimateId">見積顧客ID</param>
    ''' <param name="contractCustType">契約顧客種別</param>
    ''' <returns>処理結果（存在する[True]/存在しない[False]）</returns>
    ''' <remarks></remarks>
    Public Function SelectEstCustomerinfo(ByVal estimateId As Long, _
                                          ByVal contractCustType As String) As IC3070203DataSet.IC3070203EstCustomerInfoDataTable

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SelectEstCustomerinfo_Start")
        'ログ出力 End *****************************************************************************

        Try
            ' SQL組み立て
            Dim sql As New StringBuilder
            With sql
                .Append(" SELECT /* IC3070203_221 */ ")
                .Append("     ESTIMATEID,")       '見積管理ID
                .Append("     CONTRACTCUSTTYPE,") '契約顧客種別
                .Append("     CUSTPART,")         '顧客区分
                .Append("     NAME,")             '氏名
                .Append("     SOCIALID,")         '国民番号
                .Append("     ZIPCODE,")          '郵便番号
                .Append("     ADDRESS,")          '住所
                .Append("     TELNO,")            '電話番号
                .Append("     MOBILE,")           '携帯電話番号
                .Append("     FAXNO,")            'FAX番号
                .Append("     EMAIL,")            'e-MAILアドレス
                .Append("     CREATEDATE,")       '作成日
                .Append("     UPDATEDATE,")       '更新日
                .Append("     CREATEACCOUNT,")    '作成ユーザアカウント
                .Append("     UPDATEACCOUNT,")    '更新ユーザアカウント
                .Append("     CREATEID,")         '作成機能ID
                .Append("     UPDATEID,")         '更新機能ID
                .Append("     PRIVATE_FLEET_ITEM_CD,") '個人法人項目コード
                .Append("     NAMETITLE_CD,")     '敬称コード
                .Append("     NAMETITLE_NAME,")   '敬称
                .Append("     FIRST_NAME,")       'ファーストネーム
                .Append("     MIDDLE_NAME,")      'ミドルネーム
                .Append("     LAST_NAME,")        'ラストネーム
                .Append("     CST_ADDRESS_1,")    '顧客住所1
                .Append("     CST_ADDRESS_2,")    '顧客住所2 
                .Append("     CST_ADDRESS_3,")    '顧客住所3 
                .Append("     CST_ADDRESS_STATE,") '顧客住所（州）
                .Append("     CST_ADDRESS_DISTRICT,") '顧客住所（地区）
                .Append("     CST_ADDRESS_CITY,")  '顧客住所（市）
                .Append("     CST_ADDRESS_LOCATION ")  '顧客住所（地域）
                .Append(" FROM ")
                .Append("     TBL_EST_CUSTOMERINFO ")
                .Append(" WHERE ")
                .Append("     ESTIMATEID = :ESTIMATEID ")              '見積管理ID
                .Append("  AND CONTRACTCUSTTYPE = :CONTRACTCUSTTYPE ")  '契約顧客種別
            End With

            ' DbSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of IC3070203DataSet.IC3070203EstCustomerInfoDataTable)("IC3070203_221")

                query.CommandText = sql.ToString()

                ' SQLパラメータ設定
                query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Int64, estimateId)         '見積管理ID
                query.AddParameterWithTypeValue("CONTRACTCUSTTYPE", OracleDbType.Char, contractCustType)  '契約顧客種別

                ' SQL実行
                Dim retDT As IC3070203DataSet.IC3070203EstCustomerInfoDataTable = query.GetData()

                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SelectEstCustomerinfo_End")
                'ログ出力 End *****************************************************************************
                ' 結果を返却
                Return retDT

            End Using
        Catch ex As Exception
            Me.prpResultId = ErrCodeDBOverlap + TblCodeEstimateInfo
            Throw
        End Try


    End Function


    ''' <summary>
    ''' 見積保険情報存在チェック
    ''' </summary>
    ''' <param name="estimateId">,見積管理ID</param>
    ''' <returns>処理結果（存在する[True]/存在しない[False]）</returns>
    ''' <remarks></remarks>
    Public Function SelectEstInsuranceinfo(ByVal estimateId As Long) As IC3070203DataSet.IC3070203EstInsuranceInfoDataTable

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SelectEstInsuranceinfo_Start")
        'ログ出力 End *****************************************************************************

        Try
            ' SQL組み立て
            Dim sql As New StringBuilder
            With sql
                .Append(" SELECT /* IC3070203_222 */ ")
                .Append("     ESTIMATEID,")       '見積管理ID
                .Append("     INSUDVS,")          ' 保険区分
                .Append("     INSUCOMCD,")        '保険会社コード
                .Append("     INSUKIND,")         '保険種別
                .Append("     AMOUNT")           '保険金額
                .Append(" FROM ")
                .Append("     TBL_EST_INSURANCEINFO ")
                .Append(" WHERE ")
                .Append("     ESTIMATEID = :ESTIMATEID ")              '見積管理ID
            End With

            ' DbSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of IC3070203DataSet.IC3070203EstInsuranceInfoDataTable)("IC3070203_222")

                query.CommandText = sql.ToString()

                ' SQLパラメータ設定
                query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Int64, estimateId)         '見積管理ID

                ' SQL実行
                Dim retDT As IC3070203DataSet.IC3070203EstInsuranceInfoDataTable = query.GetData()

                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SelectEstInsuranceinfo_End")
                'ログ出力 End *****************************************************************************
                ' 結果を返却
                Return retDT

            End Using
        Catch ex As Exception
            Me.prpResultId = ErrCodeDBOverlap + TblCodeEstimateInfo
            Throw
        End Try


    End Function


    ''' <summary>
    ''' 見積支払情報存在チェック
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <param name="paymentMethod">支払方法区分</param>
    ''' <returns>処理結果（存在する[True]/存在しない[False]）</returns>
    ''' <remarks></remarks>
    Public Function SelectEstPaymentinfo(ByVal estimateId As Long, _
                                   ByVal paymentMethod As String) As IC3070203DataSet.IC3070203EstPaymentInfoDataTable

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SelectEstPaymentinfo_Start")
        'ログ出力 End *****************************************************************************

        Try
            ' SQL組み立て
            Dim sql As New StringBuilder
            With sql
                .Append(" SELECT /* IC3070203_223 */ ")
                .Append("     ESTIMATEID,")       '見積管理ID
                .Append("     PAYMENTMETHOD,")    '支払方法区分
                .Append("     FINANCECOMCODE,")   '融資会社コード
                .Append("     PAYMENTPERIOD,")    '支払期間
                .Append("     MONTHLYPAYMENT,")   '毎月返済額
                .Append("     DEPOSIT,")          '頭金
                .Append("     BONUSPAYMENT,")     'ボーナス時返済額
                .Append("     DUEDATE,")          '初回支払期限
                .Append("     DELFLG,")           '削除フラグ
                .Append("     SELECTFLG,")        '選択フラグ
                .Append("     INTERESTRATE,")     '利率
                .Append("     DEPOSITPAYMENTMETHOD")  '頭金支払方法区分
                .Append(" FROM ")
                .Append("     TBL_EST_PAYMENTINFO ")
                .Append(" WHERE ")
                .Append("     ESTIMATEID = :ESTIMATEID ")              '見積管理ID
                .Append("  AND PAYMENTMETHOD = :PAYMENTMETHOD")        '支払方法区分
            End With

            ' DbSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of IC3070203DataSet.IC3070203EstPaymentInfoDataTable)("IC3070203_223")

                query.CommandText = sql.ToString()

                ' SQLパラメータ設定
                query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Int64, estimateId)         '見積管理ID
                query.AddParameterWithTypeValue("PAYMENTMETHOD", OracleDbType.Char, paymentMethod)        '支払方法区分

                ' SQL実行
                Dim retDT As IC3070203DataSet.IC3070203EstPaymentInfoDataTable = query.GetData()

                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SelectEstPaymentinfo_End")
                'ログ出力 End *****************************************************************************
                ' 結果を返却
                Return retDT

            End Using
        Catch ex As Exception
            Me.prpResultId = ErrCodeDBOverlap + TblCodeEstimateInfo
            Throw
        End Try


    End Function

    ''' <summary>
    ''' i-CROPコード取得(顧客職業ID)
    ''' </summary>
    ''' <param name="OccupationId">顧客職業ID</param>
    ''' <returns>取得結果</returns>
    ''' <remarks></remarks>
    Public Function GetIcropCdOccupationId(ByVal OccupationId As String) As String
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetIcropCdOccupationId_Start")
        'ログ出力 End *****************************************************************************

        Using query As New DBSelectQuery(Of DataTable)("IC3070203_224")
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* IC3070203_224 */ ")
                .Append("  ICROP_CD_1 ")
                .Append("  FROM TB_M_DMS_CODE_MAP ")
                .Append("  WHERE DLR_CD = 'XXXXX' ")
                .Append("    AND DMS_CD_TYPE = '12' ")
                .Append("    AND DMS_CD_1 = :DMS_CD_1 ")
                .Append("  ORDER BY ROW_UPDATE_DATETIME ")
            End With
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DMS_CD_1", OracleDbType.NVarchar2, Trim(OccupationId))

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetIcropCdOccupationId_End")
            'ログ出力 End *****************************************************************************
            Dim ResultSet As DataTable = query.GetData()

            If ResultSet.Rows.Count > 0 Then
                Return CStr(ResultSet.Rows(0).Item("ICROP_CD_1"))
            Else
                Return String.Empty
            End If

        End Using
    End Function


#Region "契約変更情報取得"

    '更新： 2014/05/28 TCS 安田 受注時説明機能開発（受注後工程スケジュール） START
    ''' <summary>
    ''' 注文承認されているかチェックする
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <returns>True:注文承認後／False:注文承認前</returns>
    ''' <remarks></remarks>
    Public Function CheckBookAfter(ByVal estimateId As Long) As Boolean

        Dim sql As New StringBuilder
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("CheckBookAfter_Start")
        'ログ出力 End *****************************************************************************
        With sql
            .Append("SELECT ")
            .Append("  /* IC3070203_225 */ ")
            .Append("  COUNT(1) AS CNT ")
            .Append("FROM ")
            .Append("  TBL_ESTIMATEINFO ")
            .Append(" WHERE ESTIMATEID = :ESTIMATEID ")
            .Append("   AND CONTRACTNO IS NOT NULL ")
            .Append("   AND DELFLG = '0' ")
        End With

        Using query As New DBSelectQuery(Of IC3070203DataSet.IC3070203CountDataTable)("IC3070203_214")
            query.CommandText = sql.ToString()

            ' SQLパラメータ設定
            query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Long, estimateId)       '見積管理ID

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
    Public Function GetEstBeforeChangeInfo(ByVal estimateId As Long) As IC3070203DataSet.IC3070203EstChangeInfoDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture, System.Reflection.MethodBase.GetCurrentMethod.Name & "_Start[estimateId:{0}]", estimateId))
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetEstBeforeChangeInfo_Start")
        'ログ出力 End *****************************************************************************

        Try
            ' SQL組み立て
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* IC3070203_226 */ ")
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
            Using query As New DBSelectQuery(Of IC3070203DataSet.IC3070203EstChangeInfoDataTable)("IC3070203_215")

                query.CommandText = sql.ToString()

                ' SQLパラメータ設定
                query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Long, estimateId)       '見積管理ID

                ' SQL実行
                Dim retDT As IC3070203DataSet.IC3070203EstChangeInfoDataTable = query.GetData()

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

#End Region

#End Region

#Region "コンストラクタ"
    ''' <summary>
    ''' コンストラクタ
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()

    End Sub
#End Region

End Class
