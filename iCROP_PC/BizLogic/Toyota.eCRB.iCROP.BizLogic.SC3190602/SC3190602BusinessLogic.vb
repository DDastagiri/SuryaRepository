'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3190602BusinessLogic.vb
'─────────────────────────────────────
'機能： B/O部品入力 (ビジネス)
'補足： 
'作成： 2014/08/29 TMEJ M.Asano
'更新： 2015/04/22 TMEJ M.Asano 必須入力チェック外し対応 $01
'─────────────────────────────────────

Imports System.Text
Imports System.Web
Imports System.Reflection
Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Configuration
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.PartsManagement.BoMonitor.DataAccess

Public Class SC3190602BusinessLogic
    Inherits BaseBusinessComponent
    Implements ISC3190602BusinessLogic

#Region "定数"

#Region "ログ関連"

    ''' <summary>
    ''' Logフォーマット
    ''' </summary>
    ''' <remarks></remarks>
    Private Const LOG_FORMAT As String = "{0}_{1} {2} {3}"

    ''' <summary>
    ''' Log文言：開始
    ''' </summary>
    ''' <remarks></remarks>
    Private Const LOG_START As String = "Start"

    ''' <summary>
    ''' Log文言：パラメータ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const LOG_PARAMETER As String = "ParamValue"

    ''' <summary>
    ''' Log文言：終了
    ''' </summary>
    ''' <remarks></remarks>
    Private Const LOG_END As String = "End"

#End Region

#Region "Jsonキー名称"

    ''' <summary>
    ''' jsonKey:B/O ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const JSON_KEY_BO_ID As String = "BoId"

    ''' <summary>
    ''' jsonKey:P/O 番号
    ''' </summary>
    ''' <remarks></remarks>
    Private Const JSON_KEY_PO_NUM As String = "PoNum"

    ''' <summary>
    ''' jsonKey:R/O 番号
    ''' </summary>
    ''' <remarks></remarks>
    Private Const JSON_KEY_RO_NUM As String = "RoNum"

    ''' <summary>
    ''' jsonKey:車両ステータス
    ''' </summary>
    ''' <remarks></remarks>
    Private Const JSON_KEY_VCL_STATUS As String = "VclStatus"

    ''' <summary>
    ''' jsonKey:お客様約束日
    ''' </summary>
    ''' <remarks></remarks>
    Private Const JSON_KEY_CST_APP_DATE As String = "CstAppDate"

    ''' <summary>
    ''' jsonKey:作業情報リスト
    ''' </summary>
    ''' <remarks></remarks>
    Private Const JSON_KEY_JOB_LIST As String = "JobList"

    ''' <summary>
    ''' jsonKey:作業名称
    ''' </summary>
    ''' <remarks></remarks>
    Private Const JSON_KEY_JOB_NAME As String = "JobName"

    ''' <summary>
    ''' jsonKey:部品情報リスト
    ''' </summary>
    ''' <remarks></remarks>
    Private Const JSON_KEY_PARTS_LIST As String = "PartsList"

    ''' <summary>
    ''' jsonKey:部品名称
    ''' </summary>
    ''' <remarks></remarks>
    Private Const JSON_KEY_PARTS_NAME As String = "PartsName"

    ''' <summary>
    ''' jsonKey:部品コード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const JSON_KEY_PARTS_CODE As String = "PartsCode"

    ''' <summary>
    ''' jsonKey:部品数量
    ''' </summary>
    ''' <remarks></remarks>
    Private Const JSON_KEY_PARTS_AMOUNT As String = "PartsAmount"

    ''' <summary>
    ''' jsonKey:発注日
    ''' </summary>
    ''' <remarks></remarks>
    Private Const JSON_KEY_ORDER_DATE As String = "OrderDate"

    ''' <summary>
    ''' jsonKey:到着予定日
    ''' </summary>
    ''' <remarks></remarks>
    Private Const JSON_KEY_ARRIVAL_DATE As String = "ArrivalDate"

#End Region

#Region "DB関連"

    ''' <summary>
    ''' オラクルエラータイムアウト
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OracleErrorTimeOut As Integer = 2049

    ''' <summary>
    ''' DB初期値：文字列
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DB_DEFAULT_VALUE_STRING As String = " "

    ''' <summary>
    ''' DB初期値：数値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DB_DEFAULT_VALUE_NUMBER As Decimal = 0

    ''' <summary>
    ''' DB初期値：日付
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DB_DEFAULT_VALUE_DATE As Date = #1/1/1900#

#End Region

#Region "メッセージID"

    ''' <summary>
    ''' 正常
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MessageIdNormal As Integer = 0

    ''' <summary>
    ''' DBタイムアウト
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MessageIdDbTimeOut As Integer = 900

    ''' <summary>
    ''' フォーマット不正
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MessageIdFormatIncorrect As Integer = 903

#End Region

#Region "入力チェック最大文字数"

    ''' <summary>
    ''' 最大文字数：P/O番号
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MaxLengthPoNumber As Integer = 20

    ''' <summary>
    ''' 最大文字数：R/O番号
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MaxLengthRoNumber As Integer = 20

    ''' <summary>
    ''' 最大文字数：作業名称
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MaxLengthJobName As Integer = 30

    ''' <summary>
    ''' 最大文字数：部品名称
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MaxLengthPartsName As Integer = 30

    ''' <summary>
    ''' 最大文字数：部品コード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MaxLengthPartsCode As Integer = 50
#End Region

#Region "その他"

    ' $01 必須入力チェック外し対応 START
    ''' <summary>
    ''' 数量:最小値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const AmountMinValue As Decimal = 1
    ' $01 必須入力チェック外し対応 END

    ''' <summary>
    ''' 数量:最大値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const AmountMaxValue As Decimal = 99

    ''' <summary>
    ''' 日付フォーマット
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DateFormat As String = "dd/MM/yyyy"

#End Region

#End Region

#Region "公開メソッド"

    ''' <summary>
    ''' B/O情報の取得
    ''' </summary>
    ''' <param name="boId">B/O ID</param>
    ''' <returns>SC3190602BoInfoDataSet</returns>
    ''' <remarks></remarks>
    Public Function GetBoInfo(ByVal boId As Decimal) As SC3190602BoInfoDataSet

        ' 開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , LOG_FORMAT _
                                , MethodBase.GetCurrentMethod.Name _
                                , LOG_START _
                                , LOG_PARAMETER _
                                , CreateLogWrod(boId)))

        Dim boPartsInfo As SC3190602DataSet.BoInfoDataTable

        ' B/O情報
        If boId = 0 Then
            ' 新規
            boPartsInfo = New SC3190602DataSet.BoInfoDataTable
        Else
            ' 更新
            boPartsInfo = SC3190602TableAdapter.GetBoPartsInfo(boId)

            ' 取得件数ログ出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                    , LOG_FORMAT _
                                    , MethodBase.GetCurrentMethod.Name _
                                    , String.Empty _
                                    , LOG_PARAMETER _
                                    , CreateLogWrod(boPartsInfo)))
        End If

        ' 最後に空のレコードを追加する
        boPartsInfo.AddBoInfoRow(DB_DEFAULT_VALUE_NUMBER _
                               , DB_DEFAULT_VALUE_STRING _
                               , DB_DEFAULT_VALUE_STRING _
                               , DB_DEFAULT_VALUE_STRING _
                               , DB_DEFAULT_VALUE_DATE _
                               , DB_DEFAULT_VALUE_STRING _
                               , DB_DEFAULT_VALUE_STRING _
                               , DB_DEFAULT_VALUE_STRING _
                               , DB_DEFAULT_VALUE_NUMBER _
                               , DB_DEFAULT_VALUE_DATE _
                               , DB_DEFAULT_VALUE_DATE _
                               , DB_DEFAULT_VALUE_NUMBER)

        ' 画面描画用のSC3190602BoInfoDataSetへ変換
        Dim boInfoDataSet As SC3190602BoInfoDataSet = CreateReturnDataSet(boPartsInfo)

        ' 終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , LOG_FORMAT _
                                , MethodBase.GetCurrentMethod.Name _
                                , LOG_END _
                                , LOG_PARAMETER _
                                , CreateLogWrod(boInfoDataSet)))

        Return boInfoDataSet

    End Function

    ''' <summary>
    ''' 入力値のフォーマットチェック
    ''' </summary>
    ''' <param name="boInfo">B/O情報</param>
    ''' <returns>メッセージID</returns>
    ''' <remarks></remarks>
    Public Function CheckInputValue(ByVal boInfo As Dictionary(Of String, Object)) As Integer

        ' PO番号の桁数チェック
        If boInfo(JSON_KEY_PO_NUM).ToString.Length > MaxLengthPoNumber Then
            Return MessageIdFormatIncorrect
        End If

        ' RO番号の桁数チェック
        If boInfo(JSON_KEY_RO_NUM).ToString.Length > MaxLengthRoNumber Then
            Return MessageIdFormatIncorrect
        End If

        ' 入力値のフォーマットチェック
        ' お客様約束日
        If Not CheckInputValueToDate(boInfo(JSON_KEY_CST_APP_DATE).ToString) Then
            Return MessageIdFormatIncorrect
        End If

        ' 作業リスト取得
        Dim jobList As ArrayList = CType(boInfo(JSON_KEY_JOB_LIST), ArrayList)

        ' 作業分繰り返し
        For jobIndex = 0 To jobList.Count - 1
            ' 作業情報取得
            Dim jobInfo As Dictionary(Of String, Object) = CType(jobList(jobIndex), Dictionary(Of String, Object))

            ' 作業名称の桁数チェック
            If jobInfo(JSON_KEY_JOB_NAME).ToString.Length > MaxLengthJobName Then
                Return MessageIdFormatIncorrect
            End If

            ' Partsリスト取得
            Dim partsList As ArrayList = CType(jobInfo(JSON_KEY_PARTS_LIST), ArrayList)

            ' Parts分繰り返し
            For partsIndex = 0 To partsList.Count - 1
                ' Parts情報取得
                Dim partsInfo As Dictionary(Of String, Object) = CType(partsList(partsIndex), Dictionary(Of String, Object))

                ' 部品名称の桁数チェック
                If partsInfo(JSON_KEY_PARTS_NAME).ToString.Length > MaxLengthPartsName Then
                    Return MessageIdFormatIncorrect
                End If

                ' 部品コードの桁数チェック
                If partsInfo(JSON_KEY_PARTS_CODE).ToString.Length > MaxLengthPartsCode Then
                    Return MessageIdFormatIncorrect
                End If

                ' 数量
                If Not CheckInputValueToNumber(CStr(partsInfo(JSON_KEY_PARTS_AMOUNT))) Then
                    Return MessageIdFormatIncorrect
                End If

                ' 注文日
                If Not CheckInputValueToDate(CStr(partsInfo(JSON_KEY_ORDER_DATE))) Then
                    Return MessageIdFormatIncorrect
                End If

                ' 到着予定日
                If Not CheckInputValueToDate(CStr(partsInfo(JSON_KEY_ARRIVAL_DATE))) Then
                    Return MessageIdFormatIncorrect
                End If
            Next
        Next

        Return MessageIdNormal

    End Function

    ''' <summary>
    ''' B/O情報の登録
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="nowDate">本日日時</param>
    ''' <param name="account">アカウント</param>
    ''' <param name="boInfo">B/O情報</param>
    ''' <returns>メッセージID</returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Function RegisterBoInfo(ByVal dealerCode As String _
                                 , ByVal branchCode As String _
                                 , ByVal nowDate As Date _
                                 , ByVal account As String _
                                 , ByVal boInfo As Dictionary(Of String, Object)) _
                                   As Integer _
                                   Implements ISC3190602BusinessLogic.RegisterBoInfo

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , LOG_FORMAT _
                                , MethodBase.GetCurrentMethod.Name _
                                , LOG_START _
                                , LOG_PARAMETER _
                                , CreateLogWrod(dealerCode, branchCode, nowDate, account, boInfo)))

        Try
            Dim boId As Decimal = 0
            Decimal.TryParse(boInfo(JSON_KEY_BO_ID).ToString, boId)

            If boId = 0 Then
                ' 新規登録
                ' B/O IDの取得
                boId = SC3190602TableAdapter.GetBoIdNextValue()

                ' B/O 管理情報テーブルの作成
                InsertBoManagementInfo(dealerCode, branchCode, boId, nowDate, account, boInfo)
            Else
                ' 更新
                ' B/O部品情報の削除
                Dim partsInfoDeleteCunto = SC3190602TableAdapter.DeletePartsInfo(boId)
                Logger.Info("TB_T_BO_PARTS_INFO Delete Count[" & partsInfoDeleteCunto & "]")

                ' B/O作業情報の削除
                Dim jobInfoDeleteCunto = SC3190602TableAdapter.DeleteBoJobInfo(boId)
                Logger.Info("TB_T_BO_JOB_INFO Delete Count[" & jobInfoDeleteCunto & "]")

                ' B/O 管理情報テーブルの更新
                UpdateBoManagementInfo(boId, nowDate, account, boInfo)
            End If

            ' 作業及び部品情報の登録
            RegisterOperationAndPartsInfo(boId, nowDate, account, boInfo)

        Catch ex As OracleExceptionEx

            If ex.Number = OracleErrorTimeOut Then

                ' DBタイムアウト時は、アプリ側で復旧
                Me.Rollback = True
                Logger.Error(CType(MessageIdDbTimeOut, String), ex)

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                        , LOG_FORMAT _
                                        , MethodBase.GetCurrentMethod.Name _
                                        , LOG_END _
                                        , LOG_PARAMETER _
                                        , CreateLogWrod(MessageIdDbTimeOut)))
                Return MessageIdDbTimeOut

            Else
                ' データベースの操作中に例外が発生した場合
                Logger.Info("RegisterBoInfo_001 " & "Catch OracleExceptionEx")
                Logger.Info("RegisterBoInfo_End RetValue[Throw OracleExceptionEx]")
                Logger.Error("ErrorID:" & CStr(ex.Number) & "Exception:" & ex.Message)

                Throw
            End If

        End Try

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , LOG_FORMAT _
                                , MethodBase.GetCurrentMethod.Name _
                                , LOG_END _
                                , LOG_PARAMETER _
                                , CreateLogWrod(MessageIdNormal)))

        Return MessageIdNormal

    End Function

#End Region

#Region "非公開メソッド"

#Region "返却用DataSet作成"

    ''' <summary>
    ''' 返却用DataSet作成
    ''' </summary>
    ''' <param name="boInfo">B/O 情報</param>
    ''' <returns>SC3190602BoInfoDataSet</returns>
    ''' <remarks></remarks>
    Private Function CreateReturnDataSet(ByVal boInfo As SC3190602DataSet.BoInfoDataTable) As SC3190602BoInfoDataSet

        Dim returnDataSet As SC3190602BoInfoDataSet = New SC3190602BoInfoDataSet
        Dim jobId As Decimal = Decimal.Zero

        For Each resultRow As SC3190602DataSet.BoInfoRow In boInfo

            ' 作業IDが変更された場合
            If jobId = 0 OrElse jobId <> resultRow.BO_JOB_ID Then

                ' 作業IDの更新
                jobId = resultRow.BO_JOB_ID

                ' 作業テーブルへレコード追加
                returnDataSet.JobInfo.AddJobInfoRow(ProcessingDisplayValue(jobId) _
                                                  , ProcessingDisplayValue(resultRow.JOB_NAME) _
                                                  , ProcessingDisplayValue(resultRow.BO_ID) _
                                                  , ProcessingDisplayValue(resultRow.PO_NUM) _
                                                  , ProcessingDisplayValue(resultRow.RO_NUM) _
                                                  , ProcessingDisplayValue(resultRow.VCL_PARTAKE_FLG) _
                                                  , ProcessingDisplayValue(resultRow.CST_APPOINTMENT_DATE))
            End If

            ' 部品情報テーブルへレコード追加
            returnDataSet.PartsInfo.AddPartsInfoRow(ProcessingDisplayValue(jobId) _
                                                  , ProcessingDisplayValue(resultRow.PARTS_NAME) _
                                                  , ProcessingDisplayValue(resultRow.PARTS_CD) _
                                                  , ProcessingDisplayValue(resultRow.PARTS_AMOUNT) _
                                                  , ProcessingDisplayValue(resultRow.ODR_DATE) _
                                                  , ProcessingDisplayValue(resultRow.ARRIVAL_SCHE_DATE))
        Next

        'リレーションを設定
        returnDataSet.Relations.Add("relationJob" _
                                  , returnDataSet.Tables("JobInfo").Columns("BO_JOB_ID") _
                                  , returnDataSet.Tables("PartsInfo").Columns("BO_JOB_ID"))

        Return returnDataSet
    End Function

#End Region

#Region "B/O 管理情報"

    ''' <summary>
    ''' B/O 管理情報登録
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="boId">B/O ID</param>
    ''' <param name="nowDate">本日日時</param>
    ''' <param name="account">アカウント</param>
    ''' <param name="boInfo">B/O情報</param>
    ''' <remarks></remarks>
    Private Sub InsertBoManagementInfo(ByVal dealerCode As String _
                                     , ByVal branchCode As String _
                                     , ByVal boId As Decimal _
                                     , ByVal nowDate As Date _
                                     , ByVal account As String _
                                     , ByVal boInfo As Dictionary(Of String, Object))

        Using boMngInfoDataTable As New SC3190602DataSet.BoMngInfoDataTable

            Dim boInfoRow As SC3190602DataSet.BoMngInfoRow = boMngInfoDataTable.NewBoMngInfoRow

            '引数の情報をセット
            boInfoRow.BO_ID = boId
            boInfoRow.DLR_CD = dealerCode
            boInfoRow.BRN_CD = branchCode
            boInfoRow.PO_NUM = HttpUtility.HtmlDecode(ChangeDbRegisterValueToString(boInfo(JSON_KEY_PO_NUM).ToString))
            boInfoRow.RO_NUM = HttpUtility.HtmlDecode(ChangeDbRegisterValueToString(boInfo(JSON_KEY_RO_NUM).ToString))
            boInfoRow.VCL_PARTAKE_FLG = ChangeDbRegisterValueToString(boInfo(JSON_KEY_VCL_STATUS).ToString)
            boInfoRow.CST_APPOINTMENT_DATE = ChangeDbRegisterValueToDate(boInfo(JSON_KEY_CST_APP_DATE).ToString)
            boInfoRow.NOW_DATE = nowDate
            boInfoRow.ACCOUNT = account

            ' 登録
            SC3190602TableAdapter.InsertBoMngInfo(boInfoRow)
        End Using

    End Sub

    ''' <summary>
    ''' B/O 管理情報更新
    ''' </summary>
    ''' <param name="boId">B/O ID</param>
    ''' <param name="nowDate">本日日時</param>
    ''' <param name="account">アカウント</param>
    ''' <param name="boInfo">B/O情報</param>
    ''' <remarks></remarks>
    Private Sub UpdateBoManagementInfo(ByVal boId As Decimal _
                                     , ByVal nowDate As Date _
                                     , ByVal account As String _
                                     , ByVal boInfo As Dictionary(Of String, Object))

        Using boMngInfoDataTable As New SC3190602DataSet.BoMngInfoDataTable

            Dim boInfoRow As SC3190602DataSet.BoMngInfoRow = boMngInfoDataTable.NewBoMngInfoRow

            '引数の情報をセット
            boInfoRow.BO_ID = boId
            boInfoRow.PO_NUM = HttpUtility.HtmlDecode(ChangeDbRegisterValueToString(boInfo(JSON_KEY_PO_NUM).ToString))
            boInfoRow.RO_NUM = HttpUtility.HtmlDecode(ChangeDbRegisterValueToString(boInfo(JSON_KEY_RO_NUM).ToString))
            boInfoRow.VCL_PARTAKE_FLG = ChangeDbRegisterValueToString(boInfo(JSON_KEY_VCL_STATUS).ToString)
            boInfoRow.CST_APPOINTMENT_DATE = ChangeDbRegisterValueToDate(boInfo(JSON_KEY_CST_APP_DATE).ToString)
            boInfoRow.NOW_DATE = nowDate
            boInfoRow.ACCOUNT = account

            ' 更新
            SC3190602TableAdapter.UpdateBoMngInfo(boInfoRow)
        End Using

    End Sub

#End Region

#Region "作業及び部品の登録"

    ''' <summary>
    ''' 作業及び部品の登録
    ''' </summary>
    ''' <param name="boId">B/O ID</param>
    ''' <param name="nowDate">本日日時</param>
    ''' <param name="account">アカウント</param>
    ''' <param name="boInfo">B/O情報</param>
    ''' <returns>メッセージID</returns>
    ''' <remarks></remarks>
    Private Function RegisterOperationAndPartsInfo(ByVal boId As Decimal _
                                                 , ByVal nowDate As Date _
                                                 , ByVal account As String _
                                                 , ByVal boInfo As Dictionary(Of String, Object)) As Decimal

        ' 作業リスト取得
        Dim jobList As ArrayList = CType(boInfo(JSON_KEY_JOB_LIST), ArrayList)

        Using boJobInfoDataTable As New SC3190602DataSet.BoJobInfoDataTable
            Using boPartsInfoDataTable As New SC3190602DataSet.BoPartsInfoDataTable

                ' 作業分繰り返し
                For jobIndex = 0 To jobList.Count - 1

                    ' 作業情報取得
                    Dim jobInfo As Dictionary(Of String, Object) = CType(jobList(jobIndex), Dictionary(Of String, Object))

                    ' 作業IDの取得
                    Dim jobId = SC3190602TableAdapter.GetBoJobIdNextValue()

                    ' 作業情報の引数作成
                    Dim jobInfoRow As SC3190602DataSet.BoJobInfoRow = boJobInfoDataTable.NewBoJobInfoRow
                    jobInfoRow.BO_ID = boId
                    jobInfoRow.BO_JOB_ID = jobId
                    jobInfoRow.JOB_NAME = HttpUtility.HtmlDecode(ChangeDbRegisterValueToString(CStr(jobInfo(JSON_KEY_JOB_NAME))))
                    jobInfoRow.NOW_DATE = nowDate
                    jobInfoRow.ACCOUNT = account

                    ' 作業情報の登録
                    SC3190602TableAdapter.InsertBoJobInfo(jobInfoRow)

                    ' Partsリスト取得
                    Dim partsList As ArrayList = CType(jobInfo(JSON_KEY_PARTS_LIST), ArrayList)

                    ' Parts分繰り返し
                    For partsIndex = 0 To partsList.Count - 1

                        ' Parts情報取得
                        Dim partsInfo As Dictionary(Of String, Object) = CType(partsList(partsIndex), Dictionary(Of String, Object))

                        ' Parts情報の引数作成
                        Dim partsInfoRow As SC3190602DataSet.BoPartsInfoRow = boPartsInfoDataTable.NewBoPartsInfoRow
                        partsInfoRow.BO_JOB_ID = jobId
                        partsInfoRow.PARTS_NAME = HttpUtility.HtmlDecode(ChangeDbRegisterValueToString(CStr(partsInfo(JSON_KEY_PARTS_NAME))))
                        partsInfoRow.PARTS_CD = HttpUtility.HtmlDecode(ChangeDbRegisterValueToString(CStr(partsInfo(JSON_KEY_PARTS_CODE))))
                        partsInfoRow.PARTS_AMOUNT = ChangeDbRegisterValueToNumber(CStr(partsInfo(JSON_KEY_PARTS_AMOUNT)))
                        partsInfoRow.ODR_DATE = ChangeDbRegisterValueToDate(CStr(partsInfo(JSON_KEY_ORDER_DATE)))
                        partsInfoRow.ARRIVAL_SCHE_DATE = ChangeDbRegisterValueToDate(CStr(partsInfo(JSON_KEY_ARRIVAL_DATE)))
                        partsInfoRow.NOW_DATE = nowDate
                        partsInfoRow.ACCOUNT = account

                        ' Parts作業情報の登録
                        SC3190602TableAdapter.InsertPartsInfo(partsInfoRow)
                    Next
                Next
            End Using
        End Using

        Return 0

    End Function

#End Region

#Region "入力チェック"

    ''' <summary>
    ''' 入力チェック処理(数値)
    ''' </summary>
    ''' <param name="targetValue">チェック対象値</param>
    ''' <returns>True:正常 False:異常</returns>
    ''' <remarks></remarks>
    Private Function CheckInputValueToNumber(ByVal targetValue As String) As Boolean

        ' 値無し場合は、Ture
        If String.IsNullOrEmpty(Trim(targetValue)) Then

            Return True
        End If

        ' 数値に変換できない場合は、フォーマットエラー
        Dim retrunValue As Decimal
        If Not Decimal.TryParse(targetValue, retrunValue) Then

            Return False
        End If

        ' 入力値が指定範囲でなければエラー
        If AmountMinValue > retrunValue OrElse retrunValue > AmountMaxValue Then
            Return False
        End If

        ' そのまま返す
        Return True

    End Function

    ''' <summary>
    ''' 入力チェック処理(日付)
    ''' </summary>
    ''' <param name="targetValue">チェック対象値</param>
    ''' <returns>True:正常 False:異常</returns>
    ''' <remarks></remarks>
    Private Function CheckInputValueToDate(ByVal targetValue As String) As Boolean

        ' 値無し場合は、DB初期値を返却
        If String.IsNullOrEmpty(Trim(targetValue)) Then
            Return True
        End If

        ' 日付に変換できない場合は、DB初期値を返却
        Dim retrunValue As Date
        If Not Date.TryParseExact(targetValue, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, retrunValue) Then
            Return False
        End If

        ' そのまま返す
        Return True

    End Function

#End Region

#Region "DB登録用の値加工処理"

    ''' <summary>
    ''' DB登録用の値加工処理(文字列)
    ''' </summary>
    ''' <param name="targetValue">変換対象値</param>
    ''' <returns>DB登録用値</returns>
    ''' <remarks></remarks>
    Private Function ChangeDbRegisterValueToString(ByVal targetValue As String) As String

        ' 値無し場合は、DB初期値を返却
        If String.IsNullOrEmpty(Trim(targetValue)) Then

            Return DB_DEFAULT_VALUE_STRING
        End If

        ' そのまま返す
        Return targetValue

    End Function

    ''' <summary>
    ''' DB登録用の値加工処理(数値)
    ''' </summary>
    ''' <param name="targetValue">変換対象値</param>
    ''' <returns>DB登録用値</returns>
    ''' <remarks></remarks>
    Private Function ChangeDbRegisterValueToNumber(ByVal targetValue As String) As Decimal

        ' 値無し場合は、DB初期値を返却
        If String.IsNullOrEmpty(Trim(targetValue)) Then

            Return DB_DEFAULT_VALUE_NUMBER
        End If

        ' 数値に変換できない場合は、DB初期値を返却
        Dim retrunValue As Decimal
        If Not Decimal.TryParse(targetValue, retrunValue) Then

            Return DB_DEFAULT_VALUE_NUMBER
        End If

        ' そのまま返す
        Return retrunValue

    End Function

    ''' <summary>
    ''' DB登録用の値加工処理(日付)
    ''' </summary>
    ''' <param name="targetValue">変換対象値</param>
    ''' <returns>DB登録用値</returns>
    ''' <remarks></remarks>
    Private Function ChangeDbRegisterValueToDate(ByVal targetValue As String) As Date

        ' 値無し場合は、DB初期値を返却
        If String.IsNullOrEmpty(Trim(targetValue)) Then
            Return DB_DEFAULT_VALUE_DATE
        End If


        ' 日付に変換できない場合は、DB初期値を返却
        Dim retrunValue As Date
        If Not Date.TryParseExact(targetValue, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, retrunValue) Then
            Return DB_DEFAULT_VALUE_DATE
        End If

        ' そのまま返す
        Return retrunValue

    End Function

#End Region

#Region "表示文言の加工処理"

    ''' <summary>
    ''' 表示文言の加工処理(文字列)
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Overloads Function ProcessingDisplayValue(ByVal targetValue As String) As String

        ' DB初期値の場合は、未登録用文言を表示
        If String.IsNullOrEmpty(Trim(targetValue)) Then

            Return String.Empty

        End If

        ' DBに登録済みの場合はそのまま返す
        Return targetValue

    End Function

    ''' <summary>
    ''' 表示文言の加工処理(数値)
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Overloads Function ProcessingDisplayValue(ByVal targetValue As Decimal) As String

        ' DB初期値の場合は、未登録用文言を表示
        If DB_DEFAULT_VALUE_NUMBER = targetValue Then

            Return String.Empty

        End If

        ' DBに登録済みの場合はそのまま返す
        Return targetValue.ToString

    End Function

    ''' <summary>
    ''' 表示文言の加工処理(日付)
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Overloads Function ProcessingDisplayValue(ByVal targetValue As Date) As String

        ' DB初期値の場合は、未登録用文言を表示
        If DB_DEFAULT_VALUE_DATE = targetValue Then

            Return String.Empty

        End If

        ' DBに登録済みの場合は、フォーマット変換し返す
        Return targetValue.ToString(DateFormat, CultureInfo.InvariantCulture)

    End Function
#End Region

#Region "ログ文字列作成"

    ''' <summary>
    ''' ログ出力文字列作成
    ''' </summary>
    ''' <param name="parameters">ログに出力する値</param>
    ''' <returns>ログ出力文字列</returns>
    ''' <remarks></remarks>
    Private Function CreateLogWrod(ByVal ParamArray parameters As Object()) As String

        Dim logWord As New StringBuilder()
        With logWord
            Dim lastIndex As Integer = parameters.Length - 1

            ' すべての要素
            For i As Integer = 0 To lastIndex

                ' 最初の要素
                If 0 = i Then
                    .Append("[")

                    ' 最初の要素でない場合
                Else
                    .Append(", ")
                End If

                .Append(parameters(i))

                ' データテーブルの場合
                If TypeOf parameters(i) Is DataTable Then
                    .Append("[Count = ")
                    .Append(DirectCast(parameters(i), DataTable).Rows.Count)
                    .Append("]")
                End If

                ' 最後の要素の場合
                If i = lastIndex Then
                    .Append("]")
                End If
            Next

        End With

        Return logWord.ToString()

    End Function

#End Region

#End Region

End Class
