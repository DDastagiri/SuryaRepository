
'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'IC3060102BusinessLogic.vb
'─────────────────────────────────────
'機能： 査定依頼取得インタフェースデータアクセス
'補足： 
'作成： 
'更新： 2013/05/27 TMEJ m.asano 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 $01
'─────────────────────────────────────
Imports System.Text
Imports System.Globalization
Imports System.Reflection.MethodBase
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.Assessment.Assessment.DataAccess
Imports Toyota.eCRB.Assessment.Assessment.DataAccess.IC3060102DataSetTableAdapters

''' <summary>
''' IC3060102（査定依頼取得インタフェース）
''' 査定依頼取得ビジネスロジッククラス
''' </summary>
''' <remarks></remarks>
Public NotInheritable Class IC3060102BusinessLogic
    Inherits BaseBusinessComponent

#Region "定数"

#Region "終了コード"

    ''' <summary>終了コード 処理正常</summary>
    Public Const ResultCodeSuccess As Integer = 0

    ''' <summary>終了コード XML Document不正</summary>
    Public Const ResultCodeErrorXml As Integer = -1

    ''' <summary>終了コード 項目必須エラー </summary>
    Public Const ResultCodeErrorMust As Integer = 2000

    ''' <summary>終了コード 項目型エラー </summary>
    Public Const ResultCodeErrorType As Integer = 3000

    ''' <summary>終了コード 項目サイズエラー </summary>
    Public Const ResultCodeErrorSize As Integer = 4000

    ''' <summary>終了コード 値チェックエラー </summary>
    Public Const ResultCodeErrorValue As Integer = 5000

    ''' <summary>終了コード データ存在エラー </summary>
    Public Const ResultCodeErrorExists As Integer = 1100

    ''' <summary>終了コード システムエラー</summary>
    Public Const ResultCodeErrorSystem As Integer = 9999

#End Region

    ''' <summary>
    ''' エラーログ 終了コード見出し
    ''' </summary>
    Public Const LogResultId As String = "IC3060102 ResultId:"

    ''' <summary>実行モード 査定依頼件数取得</summary>
    Public Const ModeAssessmentReqCount As Integer = 0

    ''' <summary>実行モード 査定依頼一覧取得(初回)</summary>
    Public Const ModeAssessmentReqListFirst As Integer = 1

    ''' <summary>実行モード 査定依頼一覧取得(次の10件)</summary>
    Public Const ModeAssessmentReqListNext As Integer = 2

    ''' <summary>実行モード 査定依頼状態確認</summary>
    Public Const ModeAssessmentReqState As Integer = 3

    ''' <summary>
    ''' データテーブル名 対応中査定依頼一覧情報
    ''' </summary>
    Public Const TableNameInProgressAssessmentReqList As String =
        "IC3060102InProgressAssessmentReqListInfo"

    ''' <summary>日付時刻のフォーマット(yyyyMMddHHmmss)</summary>
    Public Const FormatDateTime As String = "yyyyMMddHHmmss"

    ''' <summary>日付のフォーマット(yyyyMMdd)</summary>
    Private Const FORMAT_DATE As String = "yyyyMMdd"

    ''' <summary>日付変換用時刻最小値（00時00分00秒）</summary>
    Private Const FORMAT_DATE_TIME_MIN As String = "000000"
    ''' <summary>日付変換用時刻最小値（23時59分59秒）</summary>
    Private Const FORMAT_DATE_TIME_MAX As String = "235959"

    ''' <summary>システム環境設定データ 敬称位置パラメータ名</summary>
    Private Const SYSTEMENV_PARAM_KEISYO_ZENGO As String = "KEISYO_ZENGO"
    ''' <summary>敬称表示位置 名前の前に敬称（主に英語圏）</summary>
    Private Const NAMETITLE_POSITION_BEFORE As String = "1"
    ''' <summary>敬称表示位置 名前の後ろに敬称（中国など）</summary>
    Private Const NAMETITLE_POSITION_AFTER As String = "2"

    ''' <summary>通知ステータス 依頼</summary>
    Private Const STATUS_REQUEST As String = "1"
    ''' <summary>通知ステータス キャンセル</summary>
    Private Const STATUS_CANCEL As String = "2"
    ''' <summary>通知ステータス 受信</summary>
    Private Const STATUS_RECEPTION As String = "3"
    ''' <summary>通知ステータス 受付</summary>
    Private Const STATUS_RECEPTIONIST As String = "4"

    ''' <summary>
    ''' 保有の値（保有車両）
    ''' </summary>
    Private Const RETENSION_ON As String = "1"

    ''' <summary>顧客種別 自社客</summary>
    Private Const CUSTOMER_KIND_ORIGINAL As String = "1"
    ''' <summary>顧客種別 未取引客</summary>
    Private Const CUSTOMER_KIND_NEW As String = "2"

    ''' <summary>顧客分類 所有者</summary>
    Private Const CUSTOMER_CLASS_OWNER As String = "1"

    ''' <summary>パラメータエラーメッセージ</summary>
    Public Const ErrorMessageParameter As String = " input param error "
    ''' <summary>データ存在エラーメッセージ</summary>
    Public Const ErrorMessageDataNotFound As String = " data not found error "
    ''' <summary>ステータス値エラーメッセージ</summary>
    Private Const ErrorMessageStatus As String = " status value error "

#End Region

#Region "列挙体"

    ''' <summary>
    ''' メッセージID用項目No
    ''' </summary>
    ''' <remarks></remarks>
    Private Enum ItemNo As Integer
        ''' <summary>販売店コード</summary>
        DealerCode = 3
        ''' <summary>店舗コード</summary>
        StoreCode = 4
        ''' <summary>端末ID</summary>
        ClientId = 5
        ''' <summary>依頼ID</summary>
        RequestId = 6
        ''' <summary>取得データ開始位置</summary>
        DataFrom = 7
        ''' <summary>取得データ終了位置</summary>
        DataTo = 8
        ''' <summary>ステータス</summary>
        Status = 9
    End Enum

    ''' <summary>
    ''' 項目桁数
    ''' </summary>
    ''' <remarks></remarks>
    Private Enum ItemLength As Integer
        ''' <summary>販売店コード</summary>
        DealerCode = 5
        ''' <summary>店舗コード</summary>
        StoreCode = 3
        ''' <summary>端末ID</summary>
        ClientId = 20
        ''' <summary>依頼ID</summary>
        RequestId = 10
        ''' <summary>取得データ開始位置</summary>
        DataFrom = 3
        ''' <summary>取得データ終了位置</summary>
        DataTo = 3
    End Enum

    ''' <summary>
    ''' データ存在エラーメッセージID用項目No
    ''' </summary>
    ''' <remarks></remarks>
    Private Enum DataExistsNo As Integer
        ''' <summary>査定依頼状態確認データ</summary>
        AssessmentReqState = 1
    End Enum

#End Region

#Region "コンストラクタ"

    ''' <summary>
    ''' 初期化処理
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
    End Sub

#End Region

#Region "001.査定依頼取得"

    ''' <summary>
    ''' 001.査定依頼取得
    ''' </summary>
    ''' <param name="mode">実行モード</param>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="storeCode">店舗コード</param>
    ''' <param name="clientId">端末ID</param>
    ''' <param name="requestId">依頼ID</param>
    ''' <param name="dataFrom">取得データ開始位置</param>
    ''' <param name="dataTo">取得データ終了位置</param>
    ''' <param name="messageId">メッセージID</param>
    ''' <returns>IC3060102DataSet</returns>
    ''' <remarks>実行モード別に各情報の取得を行う</remarks>
    Public Function GetAssessmentRequest(ByVal mode As Integer, _
                                         ByVal dealerCode As String, _
                                         ByVal storeCode As String, _
                                         ByVal clientId As String, _
                                         ByVal requestId As String, _
                                         ByVal dataFrom As String, _
                                         ByVal dataTo As String, _
                                         ByRef messageId As Integer) As IC3060102DataSet
        '開始ログ出力
        Logger.Info(getLogMethod(GetCurrentMethod.Name, True), True)
        Logger.Info(getLogParam("mode", CStr(mode), False), True)
        Logger.Info(getLogParam("dealerCode", dealerCode, True), True)
        Logger.Info(getLogParam("storeCode", storeCode, True), True)
        Logger.Info(getLogParam("clientId", clientId, True), True)
        Logger.Info(getLogParam("requestId", requestId, True), True)
        Logger.Info(getLogParam("dataFrom", dataFrom, True), True)
        Logger.Info(getLogParam("dataTo", dataTo, True), True)
        Logger.Info(getLogParam("messageId", CStr(messageId), True), True)


        '結果返却用DataSet作成
        Using retIC3060102DataSet As New IC3060102DataSet

            ' -----------------------------------------------
            ' -- 実行モード判定
            ' -----------------------------------------------
            Select Case mode
                Case ModeAssessmentReqCount
                    ' 査定依頼件数取得
                    retIC3060102DataSet.Merge(Me.GetAssessmentRequestInfo(dealerCode, _
                                                                          storeCode, _
                                                                          clientId, _
                                                                          messageId))

                Case ModeAssessmentReqListFirst, _
                     ModeAssessmentReqListNext

                    ' 査定依頼一覧取得
                    retIC3060102DataSet.Merge(Me.GetAssessmentRequestListInfo(mode, _
                                                                              dealerCode, _
                                                                              storeCode, _
                                                                              clientId, _
                                                                              dataFrom, _
                                                                              dataTo, _
                                                                              messageId))

                Case ModeAssessmentReqState
                    ' 査定依頼状態確認
                    retIC3060102DataSet.Merge(Me.GetAssessmentRequestStateInfo(dealerCode, _
                                                                               storeCode, _
                                                                               clientId, _
                                                                               requestId, _
                                                                               messageId))

            End Select

            '終了ログ出力
            Logger.Info(getReturnDataSet(retIC3060102DataSet), True)
            Logger.Info(getLogParam("messageId", CStr(messageId), True), True)
            Logger.Info(getLogMethod(GetCurrentMethod.Name, False), True)

            Return retIC3060102DataSet
        End Using

    End Function

#End Region

#Region "002.査定依頼件数取得"

    ''' <summary>
    ''' 002.査定依頼件数取得
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="storeCode">店舗コード</param>
    ''' <param name="clientId">端末ID</param>
    ''' <param name="messageId">メッセージID</param>
    ''' <returns>IC3060102DataSet</returns>
    ''' <remarks>
    ''' 販売店コード、店舗コード、端末IDを条件に未対応査定依頼件数、
    ''' 対応中査定依頼情報の取得を行う
    ''' </remarks>
    Private Function GetAssessmentRequestInfo(ByVal dealerCode As String, _
                                              ByVal storeCode As String, _
                                              ByVal clientId As String, _
                                              ByRef messageId As Integer) As IC3060102DataSet
        '開始ログ出力
        Logger.Info(getLogMethod(GetCurrentMethod.Name, True), True)
        Logger.Info(getLogParam("dealerCode", dealerCode, False), True)
        Logger.Info(getLogParam("storeCode", storeCode, True), True)
        Logger.Info(getLogParam("clientId", clientId, True), True)
        Logger.Info(getLogParam("messageId", CStr(messageId), True), True)


        '結果返却用DataSet作成
        Using retIC3060102DataSet As New IC3060102DataSet

            retIC3060102DataSet.Tables.Clear()

            ' -----------------------------------------------
            ' -- 入力チェック
            ' -----------------------------------------------

            '販売店コードチェック
            If IsErrorDealerCode(dealerCode, messageId) Then
                ' 入力チェックでエラーの場合、処理を終了する
                'エラーログ出力
                Logger.Error(LogResultId & CType(messageId, String) & ErrorMessageParameter)
                '終了ログ出力
                Logger.Info(getReturnDataSet(retIC3060102DataSet), True)
                Logger.Info(getLogMethod(GetCurrentMethod.Name, False), True)
                Return retIC3060102DataSet
            End If

            '店舗コードチェック
            If IsErrorStoreCode(storeCode, messageId) Then
                ' 入力チェックでエラーの場合、処理を終了する
                'エラーログ出力
                Logger.Error(LogResultId & CType(messageId, String) & ErrorMessageParameter)
                '終了ログ出力
                Logger.Info(getReturnDataSet(retIC3060102DataSet), True)
                Logger.Info(getLogMethod(GetCurrentMethod.Name, False), True)
                Return retIC3060102DataSet
            End If

            '端末IDチェック
            If IsErrorClientId(clientId, messageId) Then
                ' 入力チェックでエラーの場合、処理を終了する
                'エラーログ出力
                Logger.Error(LogResultId & CType(messageId, String) & ErrorMessageParameter)
                '終了ログ出力
                Logger.Info(getReturnDataSet(retIC3060102DataSet), True)
                Logger.Info(getLogMethod(GetCurrentMethod.Name, False), True)
                Return retIC3060102DataSet
            End If


            '現在日時の取得
            Dim dateNow As Date = DateTimeFunc.Now

            '現在日付（最小時刻）
            Dim nowFrom As Date = _
                Date.ParseExact(dateNow.ToString(FORMAT_DATE, CultureInfo.InvariantCulture) & FORMAT_DATE_TIME_MIN, _
                                FormatDateTime, Nothing)

            '現在日付（最大時刻）
            Dim nowTo As Date = _
                Date.ParseExact(dateNow.ToString(FORMAT_DATE, CultureInfo.InvariantCulture) & FORMAT_DATE_TIME_MAX, _
                                FormatDateTime, Nothing)

            ' -----------------------------------------------
            ' -- 未対応査定依頼件数取得
            ' -----------------------------------------------

            '取得データ格納用DataTable作成
            Dim assessmentReqCount As  _
                IC3060102DataSet.IC3060102AssessmentReqCountDataTable = Nothing
            Dim inProgressAssessmentReqInfo As  _
                IC3060102DataSet.IC3060102InProgressAssessmentReqInfoDataTable = Nothing

            Using da As New IC3060102DataTableTableAdapter

                '未対応査定依頼件数取得
                assessmentReqCount = _
                    da.GetAssessmentReqCountDataTable(dealerCode, _
                                                      storeCode, _
                                                      clientId,
                                                      nowFrom, _
                                                      nowTo)

                '対応中査定依頼情報取得
                inProgressAssessmentReqInfo = _
                    da.GetInProgressAssessmentReqInfoDataTable(dealerCode, _
                                                               storeCode, _
                                                               clientId, _
                                                               nowFrom, _
                                                               nowTo)

            End Using

            '取得データテーブルをデータセットに格納
            retIC3060102DataSet.Tables.Add(assessmentReqCount)
            retIC3060102DataSet.Tables.Add(inProgressAssessmentReqInfo)

            '終了ログ出力
            Logger.Info(getReturnDataSet(retIC3060102DataSet), True)
            Logger.Info(getLogParam("messageId", CStr(messageId), True), True)
            Logger.Info(getLogMethod(GetCurrentMethod.Name, False), True)

            Return retIC3060102DataSet

        End Using

    End Function

#End Region

#Region "003.査定依頼一覧取得"

    ''' <summary>
    ''' 003.査定依頼一覧取得
    ''' </summary>
    ''' <param name="mode">実行モード</param>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="storeCode">店舗コード</param>
    ''' <param name="clientId">端末ID</param>
    ''' <param name="dataFrom">取得データ開始位置</param>
    ''' <param name="dataTo">取得データ終了位置</param>
    ''' <param name="messageId">メッセージID</param>
    ''' <returns>IC3060102DataSet</returns>
    ''' <remarks>
    ''' 販売店コード、店舗コード、端末ID、取得データ開始位置、取得データ終了位置
    ''' を条件に未対応査定依頼一覧、対応中査定依頼一覧の取得を行う
    ''' 　・実行モードが"1"の場合、未対応査定依頼一覧、対応中査定依頼一覧の取得を行う
    ''' 　・実行モードが"2"の場合、未対応査定依頼一覧のみの取得を行う
    ''' </remarks>
    Private Function GetAssessmentRequestListInfo(ByVal mode As Integer, _
                                                  ByVal dealerCode As String, _
                                                  ByVal storeCode As String, _
                                                  ByVal clientId As String, _
                                                  ByVal dataFrom As String, _
                                                  ByVal dataTo As String, _
                                                  ByRef messageId As Integer) As IC3060102DataSet
        '開始ログ出力
        Logger.Info(getLogMethod(GetCurrentMethod.Name, True), True)
        Logger.Info(getLogParam("mode", CStr(mode), False), True)
        Logger.Info(getLogParam("dealerCode", dealerCode, True), True)
        Logger.Info(getLogParam("storeCode", storeCode, True), True)
        Logger.Info(getLogParam("clientId", clientId, True), True)
        Logger.Info(getLogParam("dataFrom", dataFrom, True), True)
        Logger.Info(getLogParam("dataTo", dataTo, True), True)
        Logger.Info(getLogParam("messageId", CStr(messageId), True), True)

        '結果返却用DataSet作成
        Using retIC3060102DataSet As New IC3060102DataSet

            retIC3060102DataSet.Tables.Clear()

            ' -----------------------------------------------
            ' -- 入力チェック
            ' -----------------------------------------------

            '販売店コードチェック
            If IsErrorDealerCode(dealerCode, messageId) Then
                ' 入力チェックでエラーの場合、処理を終了する
                'エラーログ出力
                Logger.Error(LogResultId & CType(messageId, String) & ErrorMessageParameter)
                '終了ログ出力
                Logger.Info(getReturnDataSet(retIC3060102DataSet), True)
                Logger.Info(getLogParam("messageId", CStr(messageId), True), True)
                Logger.Info(getLogMethod(GetCurrentMethod.Name, False), True)
                Return retIC3060102DataSet
            End If

            '店舗コードチェック
            If IsErrorStoreCode(storeCode, messageId) Then
                ' 入力チェックでエラーの場合、処理を終了する
                'エラーログ出力
                Logger.Error(LogResultId & CType(messageId, String) & ErrorMessageParameter)
                '終了ログ出力
                Logger.Info(getReturnDataSet(retIC3060102DataSet), True)
                Logger.Info(getLogParam("messageId", CStr(messageId), True), True)
                Logger.Info(getLogMethod(GetCurrentMethod.Name, False), True)
                Return retIC3060102DataSet
            End If

            '端末IDチェック
            If IsErrorClientId(clientId, messageId) Then
                ' 入力チェックでエラーの場合、処理を終了する
                'エラーログ出力
                Logger.Error(LogResultId & CType(messageId, String) & ErrorMessageParameter)
                '終了ログ出力
                Logger.Info(getReturnDataSet(retIC3060102DataSet), True)
                Logger.Info(getLogParam("messageId", CStr(messageId), True), True)
                Logger.Info(getLogMethod(GetCurrentMethod.Name, False), True)
                Return retIC3060102DataSet
            End If

            '取得データ開始位置チェック
            Dim dataFromValue As Integer = 0
            If IsErrorDataFrom(dataFrom, dataFromValue, messageId) Then
                ' 入力チェックでエラーの場合、処理を終了する
                'エラーログ出力
                Logger.Error(LogResultId & CType(messageId, String) & ErrorMessageParameter)
                '終了ログ出力
                Logger.Info(getReturnDataSet(retIC3060102DataSet), True)
                Logger.Info(getLogParam("messageId", CStr(messageId), True), True)
                Logger.Info(getLogMethod(GetCurrentMethod.Name, False), True)
                Return retIC3060102DataSet
            End If

            '取得データ終了位置チェック
            Dim dataToValue As Integer = 0
            If IsErrorDataTo(dataTo, dataToValue, messageId) Then
                ' 入力チェックでエラーの場合、処理を終了する
                'エラーログ出力
                Logger.Error(LogResultId & CType(messageId, String) & ErrorMessageParameter)
                '終了ログ出力
                Logger.Info(getReturnDataSet(retIC3060102DataSet), True)
                Logger.Info(getLogParam("messageId", CStr(messageId), True), True)
                Logger.Info(getLogMethod(GetCurrentMethod.Name, False), True)
                Return retIC3060102DataSet
            End If

            '取得データ開始・終了位置の相関チェック
            If IsErrorDataFromTo(dataFromValue, dataToValue, messageId) Then
                ' 入力チェックでエラーの場合、処理を終了する
                'エラーログ出力
                Logger.Error(LogResultId & CType(messageId, String) & ErrorMessageParameter)
                '終了ログ出力
                Logger.Info(getReturnDataSet(retIC3060102DataSet), True)
                Logger.Info(getLogParam("messageId", CStr(messageId), True), True)
                Logger.Info(getLogMethod(GetCurrentMethod.Name, False), True)
                Return retIC3060102DataSet
            End If

            '現在日時の取得
            Dim dateNow As Date = DateTimeFunc.Now

            '現在日付（最小時刻）
            Dim nowFrom As Date = _
                Date.ParseExact(dateNow.ToString(FORMAT_DATE, CultureInfo.InvariantCulture) & FORMAT_DATE_TIME_MIN, _
                                FormatDateTime, Nothing)

            '現在日付（最大時刻）
            Dim nowTo As Date = _
                Date.ParseExact(dateNow.ToString(FORMAT_DATE, CultureInfo.InvariantCulture) & FORMAT_DATE_TIME_MAX, _
                                FormatDateTime, Nothing)


            ' -----------------------------------------------
            ' -- 査定依頼一覧取得
            ' -----------------------------------------------

            Using da As New IC3060102DataTableTableAdapter

                If ModeAssessmentReqListFirst.Equals(mode) Then
                    ' 実行モードが査定依頼一覧取得(初回)の場合は、対応中査定依頼一覧を取得する

                    '取得データ格納用DataTable作成
                    Dim inProgressAssessmentReqListInfo As  _
                        IC3060102DataSet.IC3060102AssessmentReqListInfoDataTable = Nothing

                    '対応中査定依頼一覧取得
                    inProgressAssessmentReqListInfo = _
                        da.GetInProgressAssessmentReqListInfoDataTable( _
                                                    dealerCode, _
                                                    storeCode, _
                                                    clientId, _
                                                    nowFrom, _
                                                    nowTo, _
                                                    STATUS_RECEPTION)

                    If 0 < inProgressAssessmentReqListInfo.Count Then

                        '自社客の顧客、車両情報をセット
                        SetAssessmentReqListOrgCustomer(da, inProgressAssessmentReqListInfo)
                        '未取引の顧客、車両情報をセット
                        SetAssessmentReqListNewCustomer(da, inProgressAssessmentReqListInfo)
                        '副顧客の顧客、車両情報をセット
                        SetAssessmentReqListSubCustomer(da, inProgressAssessmentReqListInfo)

                    End If

                    '取得データに査定依頼日時の表示用データを設定する
                    SetAssessmentReqListInfoAddition(dealerCode, _
                                                     inProgressAssessmentReqListInfo)

                    'データテーブルのテーブル名を対応中査定依頼一覧用に変更
                    inProgressAssessmentReqListInfo.TableName = TableNameInProgressAssessmentReqList

                    '取得データテーブルをデータセットに格納
                    retIC3060102DataSet.Tables.Add(inProgressAssessmentReqListInfo)
                End If

                '未対応査定依頼一覧情報 取得データ格納用DataTable作成
                Dim assessmentReqListInfo As  _
                    IC3060102DataSet.IC3060102AssessmentReqListInfoDataTable = Nothing
                Dim assessmentReqListInfoTarget As  _
                    IC3060102DataSet.IC3060102AssessmentReqListInfoDataTable = Nothing

                '未対応査定依頼一覧情報取得
                assessmentReqListInfo = _
                    da.GetAssessmentReqListInfoDataTable( _
                                                dealerCode, _
                                                storeCode, _
                                                clientId, _
                                                nowFrom, _
                                                nowTo, _
                                                STATUS_REQUEST)

                '取得データを対象データレコードのみに絞り込む
                assessmentReqListInfoTarget = GetAssessmentReqListInfoTargetRecord( _
                                                    dataFromValue, _
                                                    dataToValue, _
                                                    assessmentReqListInfo)

                If 0 < assessmentReqListInfoTarget.Count Then

                    '自社客の顧客、車両情報をセット
                    SetAssessmentReqListOrgCustomer(da, assessmentReqListInfoTarget)
                    '未取引の顧客、車両情報をセット
                    SetAssessmentReqListNewCustomer(da, assessmentReqListInfoTarget)
                    '副顧客の顧客、車両情報をセット
                    SetAssessmentReqListSubCustomer(da, assessmentReqListInfoTarget)

                    '取得データに査定依頼日時の表示用データを設定する
                    SetAssessmentReqListInfoAddition(dealerCode, _
                                                     assessmentReqListInfoTarget)

                End If

                retIC3060102DataSet.Tables.Add(assessmentReqListInfoTarget)

            End Using


            '終了ログ出力
            Logger.Info(getReturnDataSet(retIC3060102DataSet), True)
            Logger.Info(getLogParam("messageId", CStr(messageId), True), True)
            Logger.Info(getLogMethod(GetCurrentMethod.Name, False), True)
            Return retIC3060102DataSet

        End Using

    End Function

#End Region

#Region "004.査定依頼状態確認"

    ''' <summary>
    ''' 004.査定依頼状態確認
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="storeCode">店舗コード</param>
    ''' <param name="clientId">端末ID</param>
    ''' <param name="requestId">依頼ID</param>
    ''' <param name="messageId">メッセージID</param>
    ''' <returns>IC3060102DataSet</returns>
    ''' <remarks>
    ''' 販売店コード、店舗コード、依頼IDを条件に査定依頼状態情報を取得する
    ''' </remarks>
    Private Function GetAssessmentRequestStateInfo(ByVal dealerCode As String, _
                                                   ByVal storeCode As String, _
                                                   ByVal clientId As String, _
                                                   ByVal requestId As String, _
                                                   ByRef messageId As Integer) As IC3060102DataSet
        '開始ログ出力
        Logger.Info(getLogMethod(GetCurrentMethod.Name, True), True)
        Logger.Info(getLogParam("dealerCode", dealerCode, False), True)
        Logger.Info(getLogParam("storeCode", storeCode, True), True)
        Logger.Info(getLogParam("requestId", CStr(requestId), True), True)
        Logger.Info(getLogParam("messageId", CStr(messageId), True), True)


        '結果返却用DataSet作成
        Using retIC3060102DataSet As New IC3060102DataSet

            retIC3060102DataSet.Tables.Clear()

            ' -----------------------------------------------
            ' -- 入力チェック
            ' -----------------------------------------------

            '販売店コードチェック
            If IsErrorDealerCode(dealerCode, messageId) Then
                ' 入力チェックでエラーの場合、処理を終了する
                'エラーログ出力
                Logger.Error(LogResultId & CType(messageId, String) & ErrorMessageParameter)
                '終了ログ出力
                Logger.Info(getReturnDataSet(retIC3060102DataSet), True)
                Logger.Info(getLogParam("messageId", CStr(messageId), True), True)
                Logger.Info(getLogMethod(GetCurrentMethod.Name, False), True)
                Return retIC3060102DataSet
            End If

            '店舗コードチェック
            If IsErrorStoreCode(storeCode, messageId) Then
                ' 入力チェックでエラーの場合、処理を終了する
                'エラーログ出力
                Logger.Error(LogResultId & CType(messageId, String) & ErrorMessageParameter)
                '終了ログ出力
                Logger.Info(getReturnDataSet(retIC3060102DataSet), True)
                Logger.Info(getLogParam("messageId", CStr(messageId), True), True)
                Logger.Info(getLogMethod(GetCurrentMethod.Name, False), True)
                Return retIC3060102DataSet
            End If

            '端末IDチェック
            If IsErrorClientId(clientId, messageId) Then
                ' 入力チェックでエラーの場合、処理を終了する
                'エラーログ出力
                Logger.Error(LogResultId & CType(messageId, String) & ErrorMessageParameter)
                '終了ログ出力
                Logger.Info(getReturnDataSet(retIC3060102DataSet), True)
                Logger.Info(getLogParam("messageId", CStr(messageId), True), True)
                Logger.Info(getLogMethod(GetCurrentMethod.Name, False), True)
                Return retIC3060102DataSet
            End If

            '依頼IDチェック
            Dim requestIdValue As Long = 0
            If IsErrorRequestId(requestId, requestIdValue, messageId) Then
                ' 入力チェックでエラーの場合、処理を終了する
                'エラーログ出力
                Logger.Error(LogResultId & CType(messageId, String) & ErrorMessageParameter)
                '終了ログ出力
                Logger.Info(getReturnDataSet(retIC3060102DataSet), True)
                Logger.Info(getLogParam("messageId", CStr(messageId), True), True)
                Logger.Info(getLogMethod(GetCurrentMethod.Name, False), True)
                Return retIC3060102DataSet
            End If

            '現在日時の取得
            Dim dateNow As Date = DateTimeFunc.Now

            '現在日付（最小時刻）
            Dim nowFrom As Date = _
                Date.ParseExact(dateNow.ToString(FORMAT_DATE, CultureInfo.InvariantCulture) & FORMAT_DATE_TIME_MIN, _
                                FormatDateTime, Nothing)

            '現在日付（最大時刻）
            Dim nowTo As Date = _
                Date.ParseExact(dateNow.ToString(FORMAT_DATE, CultureInfo.InvariantCulture) & FORMAT_DATE_TIME_MAX, _
                                FormatDateTime, Nothing)

            ' -----------------------------------------------
            ' -- 査定依頼状態取得
            ' -----------------------------------------------

            '取得データ格納用DataTable作成
            Dim assessmentReqStateInfo As  _
                IC3060102DataSet.IC3060102AssessmentReqStateInfoDataTable = Nothing

            Using da As New IC3060102DataTableTableAdapter

                '査定依頼状態情報取得
                assessmentReqStateInfo = _
                    da.GetAssessmentReqStateInfoDataTable( _
                                                dealerCode, _
                                                storeCode, _
                                                requestIdValue, _
                                                nowFrom, _
                                                nowTo)

                If assessmentReqStateInfo.Count.Equals(0) Then
                    'データが存在しない場合は、該当データなしエラー
                    messageId = ResultCodeErrorExists + DataExistsNo.AssessmentReqState

                    'エラーログ出力
                    Logger.Error(LogResultId & CType(messageId, String) & ErrorMessageDataNotFound)
                    '終了ログ出力
                    Logger.Info(getReturnDataSet(retIC3060102DataSet), True)
                    Logger.Info(getLogParam("messageId", CStr(messageId), True), True)
                    Logger.Info(getLogMethod(GetCurrentMethod.Name, False), True)
                    Return retIC3060102DataSet
                End If

                Dim row As IC3060102DataSet.IC3060102AssessmentReqStateInfoRow = _
                    DirectCast(assessmentReqStateInfo.Rows(0),  _
                               IC3060102DataSet.IC3060102AssessmentReqStateInfoRow)
                If IsErrorStatus(row.STATUS, messageId) Then
                    ' 入力チェックでエラーの場合、処理を終了する
                    'エラーログ出力
                    Logger.Error(LogResultId & CType(messageId, String) & ErrorMessageStatus)
                    '終了ログ出力
                    Logger.Info(getReturnDataSet(retIC3060102DataSet), True)
                    Logger.Info(getLogParam("messageId", CStr(messageId), True), True)
                    Logger.Info(getLogMethod(GetCurrentMethod.Name, False), True)
                    Return retIC3060102DataSet
                End If

                '顧客名、送信元、受信先の設定
                SetAssessmentReqStateAddition(da, _
                                              dealerCode, _
                                              storeCode, _
                                              clientId, _
                                              requestIdValue, _
                                              assessmentReqStateInfo)

            End Using

            retIC3060102DataSet.Tables.Add(assessmentReqStateInfo)


            '終了ログ出力
            Logger.Info(getReturnDataSet(retIC3060102DataSet), True)
            Logger.Info(getLogParam("messageId", CStr(messageId), True), True)
            Logger.Info(getLogMethod(GetCurrentMethod.Name, False), True)
            Return retIC3060102DataSet

        End Using

    End Function

#End Region

#Region "顧客・車両情報、取得設定処理(査定依頼一覧情報)"

    ''' <summary>
    ''' 未対応査定依頼一覧情報から自社客の顧客、車両情報を取得、設定して返却する
    ''' </summary>
    ''' <param name="da">IC3060102DataTableアダプター</param>
    ''' <param name="assessmentReqListDataTable">未対応査定依頼一覧情報DataTable</param>
    ''' <remarks></remarks>
    Private Sub SetAssessmentReqListOrgCustomer( _
        ByVal da As IC3060102DataTableTableAdapter, _
        ByVal assessmentReqListDataTable As IC3060102DataSet.IC3060102AssessmentReqListInfoDataTable)
        '開始ログ出力
        Logger.Info(getLogMethod(GetCurrentMethod.Name, True), True)
        Logger.Info(getLogParam("da", CStr(da.ToString), False), True)
        Logger.Info(getLogParam("assessmentReqListDataTable", CStr(assessmentReqListDataTable.Count), True), True)

        ' 顧客種別が"1"(自社客)で顧客分類が"1"(所有者)のレコードを取得する
        Dim rowsAssessmentReq() As DataRow = _
            assessmentReqListDataTable.Select("CSTKIND='1' AND CUSTOMERCLASS='1'")

        If 0 < rowsAssessmentReq.Length Then
            'データが存在する場合、自社客の顧客、車両情報を取得し、設定する

            '自社客の顧客・車両情報を取得設定する
            For Each rowAssessmentReq As IC3060102DataSet.IC3060102AssessmentReqListInfoRow In rowsAssessmentReq

                '顧客コードが空でない場合のみ、顧客情報を取得、設定する
                If Not String.IsNullOrEmpty(rowAssessmentReq.CRCUSTID) Then

                    '顧客、車両情報の取得
                    Dim retCustomerDataTable As  _
                        IC3060102DataSet.IC3060102CustomerInfoDataTable = _
                            da.GetOrgCustomerInfoDataTable(rowAssessmentReq.CRCUSTID, _
                                                           rowAssessmentReq.VIN)

                    For Each rowCustomer As IC3060102DataSet.IC3060102CustomerInfoRow In retCustomerDataTable.Rows

                        '取得した顧客情報をセット
                        '携帯電話番号がスペースのみの場合、電話番号をセットする
                        If String.IsNullOrEmpty(rowCustomer.MOBILE.Trim) Then
                            rowAssessmentReq.MOBILE = rowCustomer.TELNO.Trim
                        Else
                            rowAssessmentReq.MOBILE = rowCustomer.MOBILE.Trim
                        End If

                        If String.Equals(RETENSION_ON, rowAssessmentReq.RETENTION) Then
                            '保有が"1"(保有車両)の場合、顧客情報から車両情報を設定する
                            rowAssessmentReq.VCLREGNO = rowCustomer.VCLREGNO
                            rowAssessmentReq.MAKERCD = rowCustomer.MAKERCD
                            rowAssessmentReq.MAKERNAME = rowCustomer.MAKERNAME
                            rowAssessmentReq.SERIESCD = rowCustomer.SERIESCD
                            rowAssessmentReq.SERIESNM = rowCustomer.SERIESNM
                        End If

                    Next

                End If

            Next rowAssessmentReq

        End If

        '終了ログ出力
        Logger.Info(getLogMethod(GetCurrentMethod.Name, False), True)
    End Sub

    ''' <summary>
    ''' 未対応査定依頼一覧情報から未取引の顧客、車両情報を取得、設定して返却する
    ''' </summary>
    ''' <param name="da">IC3060102DataTableアダプター</param>
    ''' <param name="assessmentReqListDataTable">未対応査定依頼一覧情報DataTable</param>
    ''' <remarks></remarks>
    Private Sub SetAssessmentReqListNewCustomer( _
        ByVal da As IC3060102DataTableTableAdapter, _
        ByVal assessmentReqListDataTable As IC3060102DataSet.IC3060102AssessmentReqListInfoDataTable)
        '開始ログ出力
        Logger.Info(getLogMethod(GetCurrentMethod.Name, True), True)
        Logger.Info(getLogParam("da", CStr(da.ToString), False), True)
        Logger.Info(getLogParam("assessmentReqListDataTable", CStr(assessmentReqListDataTable.Count), True), True)

        ' 顧客種別が"2"(未取引客)のレコードを取得する
        Dim rowsAssessmentReq() As DataRow = _
            assessmentReqListDataTable.Select("CSTKIND='2'")

        If 0 < rowsAssessmentReq.Length Then
            'データが存在する場合、自社客の顧客、車両情報を取得し、設定する

            '自社客の顧客・車両情報を取得設定する
            For Each rowAssessmentReq As IC3060102DataSet.IC3060102AssessmentReqListInfoRow In rowsAssessmentReq

                '顧客コードが空でない場合のみ、顧客情報を取得、設定する
                If Not String.IsNullOrEmpty(rowAssessmentReq.CRCUSTID) Then

                    '未取引客車両シーケンス番号がNULL値の場合は、シーケンスに-1を指定し
                    '顧客情報のみ取得する
                    '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 START
                    Dim seqNo As Decimal = -1
                    If Not Decimal.TryParse(rowAssessmentReq.NEWCSTVCL_SEQNO, seqNo) Then
                        seqNo = -1
                    End If
                    '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 END
                    '顧客、車両情報の取得
                    Dim retCustomerDataTable As  _
                        IC3060102DataSet.IC3060102CustomerInfoDataTable =
                            da.GetNewCustomerInfoDataTable(rowAssessmentReq.CRCUSTID, _
                                                           seqNo)

                    For Each rowCustomer As IC3060102DataSet.IC3060102CustomerInfoRow In retCustomerDataTable.Rows

                        '取得した顧客情報をセット
                        '携帯電話番号がスペースのみの場合、電話番号をセットする
                        If String.IsNullOrEmpty(rowCustomer.MOBILE.Trim) Then
                            rowAssessmentReq.MOBILE = rowCustomer.TELNO.Trim
                        Else
                            rowAssessmentReq.MOBILE = rowCustomer.MOBILE.Trim
                        End If

                        If String.Equals(RETENSION_ON, rowAssessmentReq.RETENTION) Then
                            '保有が"1"(保有車両)の場合、顧客情報から車両情報を設定する
                            rowAssessmentReq.VIN = rowCustomer.VIN
                            rowAssessmentReq.VCLREGNO = rowCustomer.VCLREGNO
                            rowAssessmentReq.MAKERCD = rowCustomer.MAKERCD
                            rowAssessmentReq.MAKERNAME = rowCustomer.MAKERNAME
                            rowAssessmentReq.SERIESCD = rowCustomer.SERIESCD
                            rowAssessmentReq.SERIESNM = rowCustomer.SERIESNM
                        End If

                    Next

                End If

            Next rowAssessmentReq

        End If

        '終了ログ出力
        Logger.Info(getLogMethod(GetCurrentMethod.Name, False), True)
    End Sub

    ''' <summary>
    ''' 未対応査定依頼一覧情報から副顧客の顧客、車両情報を取得、設定して返却する
    ''' </summary>
    ''' <param name="da">IC3060102DataTableアダプター</param>
    ''' <param name="assessmentReqListDataTable">未対応査定依頼一覧情報DataTable</param>
    ''' <remarks></remarks>
    Private Sub SetAssessmentReqListSubCustomer( _
        ByVal da As IC3060102DataTableTableAdapter, _
        ByVal assessmentReqListDataTable As IC3060102DataSet.IC3060102AssessmentReqListInfoDataTable)
        '開始ログ出力
        Logger.Info(getLogMethod(GetCurrentMethod.Name, True), True)
        Logger.Info(getLogParam("da", CStr(da.ToString), False), True)
        Logger.Info(getLogParam("assessmentReqListDataTable", CStr(assessmentReqListDataTable.Count), True), True)

        ' 顧客種別が"1"(自社客)で顧客分類が"1"(所有者)でない(副顧客)レコードを取得する
        Dim rowsAssessmentReq() As DataRow = _
            assessmentReqListDataTable.Select("CSTKIND='1' AND CUSTOMERCLASS<>'1'")

        If 0 < rowsAssessmentReq.Length Then
            'データが存在する場合、自社客の顧客、車両情報を取得し、設定する

            '自社客の顧客・車両情報を取得設定する
            For Each rowAssessmentReq As IC3060102DataSet.IC3060102AssessmentReqListInfoRow In rowsAssessmentReq

                '顧客コードが空でない場合のみ、顧客情報を取得、設定する
                If Not String.IsNullOrEmpty(rowAssessmentReq.CRCUSTID) Then

                    '顧客、車両情報の取得
                    Dim retCustomerDataTable As  _
                        IC3060102DataSet.IC3060102CustomerInfoDataTable = _
                            da.GetSubCustomerInfoDataTable(rowAssessmentReq.CRCUSTID, _
                                                           rowAssessmentReq.VIN)

                    For Each rowCustomer As IC3060102DataSet.IC3060102CustomerInfoRow In retCustomerDataTable.Rows

                        '取得した顧客情報をセット
                        '携帯電話番号がスペースのみの場合、電話番号をセットする
                        If String.IsNullOrEmpty(rowCustomer.MOBILE.Trim) Then
                            rowAssessmentReq.MOBILE = rowCustomer.TELNO.Trim
                        Else
                            rowAssessmentReq.MOBILE = rowCustomer.MOBILE.Trim
                        End If

                        If String.Equals(RETENSION_ON, rowAssessmentReq.RETENTION) Then
                            '保有が"1"(保有車両)の場合、顧客情報から車両情報を設定する
                            rowAssessmentReq.VCLREGNO = rowCustomer.VCLREGNO
                            rowAssessmentReq.MAKERCD = rowCustomer.MAKERCD
                            rowAssessmentReq.MAKERNAME = rowCustomer.MAKERNAME
                            rowAssessmentReq.SERIESCD = rowCustomer.SERIESCD
                            rowAssessmentReq.SERIESNM = rowCustomer.SERIESNM
                        End If

                    Next

                End If

            Next rowAssessmentReq

        End If

        '終了ログ出力
        Logger.Info(getLogMethod(GetCurrentMethod.Name, False), True)
    End Sub

#End Region

#Region "査定依頼一覧情報のデータ付加処理（査定依頼一覧情報）"

    ''' <summary>
    ''' 査定依頼一覧情報に付加情報を設定する
    ''' 　・査定依頼日時の表示用データの作成
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="assessmentReqListInfo">査定依頼一覧情報DataTable</param>
    ''' <remarks></remarks>
    Private Sub SetAssessmentReqListInfoAddition( _
        ByVal dealerCode As String, _
        ByVal assessmentReqListInfo _
        As IC3060102DataSet.IC3060102AssessmentReqListInfoDataTable)
        '開始ログ出力
        Logger.Info(getLogMethod(GetCurrentMethod.Name, True), True)
        Logger.Info(getLogParam("dealerCode", dealerCode, False), True)
        Logger.Info(getLogParam("assessmentReqListInfo DataTable Count", _
                                CStr(assessmentReqListInfo.Count), True), True)

        For Each row As IC3060102DataSet.IC3060102AssessmentReqListInfoRow In assessmentReqListInfo.Rows

            '査定依頼日時の表示用の作成
            row.SENDDATEDISP = DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Notification, _
                                                              row.SENDDATE, _
                                                              dealerCode)
        Next

        '終了ログ出力
        Logger.Info(getLogMethod(GetCurrentMethod.Name, False), True)
    End Sub

#End Region

#Region "査定依頼一覧情報レコード抽出処理"

    ''' <summary>
    ''' 査定依頼一覧情報から取得データ開始位置、終了位置に該当する
    ''' レコードを抽出する
    ''' </summary>
    ''' <param name="dataFrom">取得データ開始位置</param>
    ''' <param name="dataTo">取得データ終了位置</param>
    ''' <param name="assessmentReqListInfo">査定依頼一覧情報DataTable</param>
    ''' <remarks></remarks>
    Private Function GetAssessmentReqListInfoTargetRecord( _
        ByVal dataFrom As Integer, _
        ByVal dataTo As Integer, _
        ByVal assessmentReqListInfo As IC3060102DataSet.IC3060102AssessmentReqListInfoDataTable) _
        As IC3060102DataSet.IC3060102AssessmentReqListInfoDataTable
        '開始ログ出力
        Logger.Info(getLogMethod(GetCurrentMethod.Name, True), True)
        Logger.Info(getLogParam("dataFrom", CStr(dataFrom), False), True)
        Logger.Info(getLogParam("dataTo", CStr(dataTo), True), True)
        Logger.Info(getLogParam("assessmentReqListInfo DataTable Count", _
                                CStr(assessmentReqListInfo.Count), True), True)

        Using retAssessmentReqList As New IC3060102DataSet.IC3060102AssessmentReqListInfoDataTable

            Dim i As Integer = 1
            For Each row As IC3060102DataSet.IC3060102AssessmentReqListInfoRow In assessmentReqListInfo.Rows

                If dataTo < i Then
                    'データ取得終了位置を以上の場合、ループを終了する
                    Exit For
                End If

                If dataFrom <= i Then
                    'データ取得開始位置より大きいレコード位置の場合、データを設定する
                    retAssessmentReqList.ImportRow(row)
                End If

                i = i + 1
            Next

            '終了ログ出力
            Logger.Info(getReturnParam("DataTable Count:" & CStr(retAssessmentReqList.Count)), True)
            Logger.Info(getLogMethod(GetCurrentMethod.Name, False), True)
            Return retAssessmentReqList

        End Using

    End Function

#End Region

#Region "査定依頼状態、顧客名取得・設定処理"

    ''' <summary>
    ''' 査定依頼状態確認の顧客コード、顧客種別、顧客分類から
    ''' 顧客名を取得し、新規査定、再査定のそれぞれの場合の受付者の設定をする
    ''' </summary>
    ''' <param name="da">IC3060102DataTableアダプター</param>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="storeCode">店舗コード</param>
    ''' <param name="clientId">端末ID</param>
    ''' <param name="requestId">依頼ID</param>
    ''' <param name="assessmentReqStateDataTable">査定依頼状態情報DataTable</param>
    ''' <remarks></remarks>
    Private Sub SetAssessmentReqStateAddition( _
        ByVal da As IC3060102DataTableTableAdapter, _
        ByVal dealerCode As String, _
        ByVal storeCode As String, _
        ByVal clientId As String, _
        ByVal requestId As Long, _
        ByVal assessmentReqStateDataTable As IC3060102DataSet.IC3060102AssessmentReqStateInfoDataTable)
        '開始ログ出力
        Logger.Info(getLogMethod(GetCurrentMethod.Name, True), True)
        Logger.Info(getLogParam("da", CStr(da.ToString), False), True)
        Logger.Info(getLogParam("dealerCode", dealerCode, True), True)
        Logger.Info(getLogParam("storeCode", storeCode, True), True)
        Logger.Info(getLogParam("clientId", clientId, True), True)
        Logger.Info(getLogParam("requestId", CStr(requestId), True), True)
        Logger.Info(getLogParam("assessmentReqStateDataTable", CStr(assessmentReqStateDataTable.Count), True), True)

        For Each dr As IC3060102DataSet.IC3060102AssessmentReqStateInfoRow In assessmentReqStateDataTable.Rows

            Dim customerDataTable As  _
                    IC3060102DataSet.IC3060102CustomerNameDataTable = Nothing

            If CUSTOMER_KIND_ORIGINAL.Equals(dr.CSTKIND) AndAlso _
                CUSTOMER_CLASS_OWNER.Equals(dr.CUSTOMERCLASS) Then
                '顧客が自社客の場合

                '自社客情報の取得
                customerDataTable = da.GetOrgCustomerNameDataTable(dr.CRCUSTID)
            ElseIf CUSTOMER_KIND_NEW.Equals(dr.CSTKIND) Then
                '顧客が未取引客の場合

                '未取引客情報の取得
                customerDataTable = da.GetNewCustomerNameDataTable(dr.CRCUSTID)

            ElseIf CUSTOMER_KIND_ORIGINAL.Equals(dr.CSTKIND) AndAlso _
                Not CUSTOMER_CLASS_OWNER.Equals(dr.CUSTOMERCLASS) Then
                '顧客が副顧客の場合

                '副顧客情報の取得
                customerDataTable = da.GetSubCustomerNameDataTable(dr.CRCUSTID)
            End If

            If 0 < customerDataTable.Count Then
                For Each customerRow As IC3060102DataSet.IC3060102CustomerNameRow In customerDataTable.Rows
                    '顧客名（敬称なし）のセット
                    dr.CUSTOMNAME = customerRow.NAME
                Next customerRow

            End If

            '再査定依頼の場合の状態を設定する
            If Not dr.IsLASTNOTICEIDNull Then
                If Not dr.LASTNOTICEID.Equals(0) Then
                    '最終通知IDが0でない場合

                    Dim targetClientId As String = dr.TOCLIENTID
                    If STATUS_RECEPTION.Equals(dr.STATUS) Then
                        'ステータスが"3"の場合、FROMCLIENTIDの値を使用する
                        targetClientId = dr.FROMCLIENTID
                    End If

                    If Not clientId.Equals(targetClientId) Then
                        '端末IDが自分自身でない場合、完了者の名前を取得する

                        '完了者の名前を取得、設定する
                        Dim assessmentAcountNameDataTable As  _
                            IC3060102DataSet.IC3060102NoticeFromAccountNameDataTable = _
                                da.GetReceptionistAccountNameDataTable( _
                                    dealerCode, _
                                    storeCode, _
                                    requestId)

                        '完了者の名前を設定する
                        '(データが存在しない場合は初回査定データのため、
                        ' 査定依頼状態確認データをそのまま使用する)
                        For Each drNoticeFromAccountName As IC3060102DataSet.IC3060102NoticeFromAccountNameRow In assessmentAcountNameDataTable.Rows
                            'ステータスを"4"(受付)とする
                            dr.STATUS = STATUS_RECEPTIONIST

                            dr.FROMACCOUNTNAME = drNoticeFromAccountName.FROMACCOUNTNAME
                        Next

                    End If
                End If
            End If

            'ステータス別に送信元の値と受信先の値を設定する
            If Not STATUS_REQUEST.Equals(dr.STATUS) And _
                Not STATUS_CANCEL.Equals(dr.STATUS) Then
                'ステータスが'1'(依頼)、または'2'(キャンセル)でない場合
                Dim fromAccountWork As String = dr.FROMACCOUNT
                Dim fromClientId As String = dr.FROMCLIENTID
                Dim fromAccountName As String = dr.FROMACCOUNTNAME

                '送信元の値を受信先の値で入れ替える
                dr.FROMACCOUNT = dr.TOACCOUNT
                dr.FROMCLIENTID = dr.TOCLIENTID
                dr.FROMACCOUNTNAME = dr.TOACCOUNTNAME

                '受信先の値を送信元の値で入れ替える
                dr.TOACCOUNT = fromAccountWork
                dr.TOCLIENTID = fromClientId
                dr.TOACCOUNTNAME = fromAccountName
            End If

        Next dr


        '終了ログ出力
        Logger.Info(getLogMethod(GetCurrentMethod.Name, False), True)
    End Sub

#End Region

#Region "入力チェック"

    ''' <summary>
    ''' 販売店コード入力チェック
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="messageId">メッセージID</param>
    ''' <returns>True:エラーあり,False:エラーなし</returns>
    ''' <remarks></remarks>
    Private Function IsErrorDealerCode(ByVal dealerCode As String, _
                                       ByRef messageId As Integer) As Boolean
        '開始ログ出力
        Logger.Info(getLogMethod(GetCurrentMethod.Name, True), True)
        Logger.Info(getLogParam("dealerCode", dealerCode, False), True)
        Logger.Info(getLogParam("messageId", CStr(messageId), True), True)

        ' 必須項目チェック
        If String.IsNullOrEmpty(dealerCode) Then
            messageId = ResultCodeErrorMust + ItemNo.DealerCode
            'エラーログ出力
            Logger.Error(LogResultId & CType(messageId, String) & ErrorMessageParameter)

            '終了ログ出力
            Logger.Info(getReturnParam(True.ToString), True)
            Logger.Info(getLogParam("messageId", CStr(messageId), True), True)
            Logger.Info(getLogMethod(GetCurrentMethod.Name, False), True)
            Return True
        End If

        ' 桁チェック
        If Not Validation.IsCorrectDigit(dealerCode, ItemLength.DealerCode) Then
            messageId = ResultCodeErrorSize + ItemNo.DealerCode
            'エラーログ出力
            Logger.Error(LogResultId & CType(messageId, String) & ErrorMessageParameter)

            '終了ログ出力
            Logger.Info(getReturnParam(True.ToString), True)
            Logger.Info(getLogParam("messageId", CStr(messageId), True), True)
            Logger.Info(getLogMethod(GetCurrentMethod.Name, False), True)
            Return True
        End If

        '終了ログ出力
        Logger.Info(getReturnParam(False.ToString), True)
        Logger.Info(getLogParam("messageId", CStr(messageId), True), True)
        Logger.Info(getLogMethod(GetCurrentMethod.Name, False), True)
        Return False
    End Function

    ''' <summary>
    ''' 店舗コード入力チェック
    ''' </summary>
    ''' <param name="storeCode">店舗コード</param>
    ''' <param name="messageId">メッセージID</param>
    ''' <returns>True:エラーあり,False:エラーなし</returns>
    ''' <remarks></remarks>
    Private Function IsErrorStoreCode(ByVal storeCode As String, _
                                      ByRef messageId As Integer) As Boolean
        '開始ログ出力
        Logger.Info(getLogMethod(GetCurrentMethod.Name, True), True)
        Logger.Info(getLogParam("storeCode", storeCode, False), True)
        Logger.Info(getLogParam("messageId", CStr(messageId), True), True)

        ' 必須項目チェック
        If String.IsNullOrEmpty(storeCode) Then
            messageId = ResultCodeErrorMust + ItemNo.StoreCode
            'エラーログ出力
            Logger.Error(LogResultId & CType(messageId, String) & ErrorMessageParameter)

            '終了ログ出力
            Logger.Info(getReturnParam(True.ToString), True)
            Logger.Info(getLogParam("messageId", CStr(messageId), True), True)
            Logger.Info(getLogMethod(GetCurrentMethod.Name, False), True)
            Return True
        End If

        ' 桁チェック
        If Not Validation.IsCorrectDigit(storeCode, ItemLength.StoreCode) Then
            messageId = ResultCodeErrorSize + ItemNo.StoreCode
            'エラーログ出力
            Logger.Error(LogResultId & CType(messageId, String) & ErrorMessageParameter)

            '終了ログ出力
            Logger.Info(getReturnParam(True.ToString), True)
            Logger.Info(getLogParam("messageId", CStr(messageId), True), True)
            Logger.Info(getLogMethod(GetCurrentMethod.Name, False), True)
            Return True
        End If

        '終了ログ出力
        Logger.Info(getReturnParam(False.ToString), True)
        Logger.Info(getLogParam("messageId", CStr(messageId), True), True)
        Logger.Info(getLogMethod(GetCurrentMethod.Name, False), True)
        Return False
    End Function

    ''' <summary>
    ''' 端末ID入力チェック
    ''' </summary>
    ''' <param name="clientId">端末ID</param>
    ''' <param name="messageId">メッセージID</param>
    ''' <returns>True:エラーあり,False:エラーなし</returns>
    ''' <remarks></remarks>
    Private Function IsErrorClientId(ByVal clientId As String, _
                                     ByRef messageId As Integer) As Boolean
        '開始ログ出力
        Logger.Info(getLogMethod(GetCurrentMethod.Name, True), True)
        Logger.Info(getLogParam("clientId", clientId, False), True)
        Logger.Info(getLogParam("messageId", CStr(messageId), True), True)

        ' 必須項目チェック
        If String.IsNullOrEmpty(clientId) Then
            messageId = ResultCodeErrorMust + ItemNo.ClientId
            'エラーログ出力
            Logger.Error(LogResultId & CType(messageId, String) & ErrorMessageParameter)

            '終了ログ出力
            Logger.Info(getReturnParam(True.ToString), True)
            Logger.Info(getLogParam("messageId", CStr(messageId), True), True)
            Logger.Info(getLogMethod(GetCurrentMethod.Name, False), True)
            Return True
        End If

        ' 桁チェック
        If Not Validation.IsCorrectDigit(clientId, ItemLength.ClientId) Then
            messageId = ResultCodeErrorSize + ItemNo.ClientId
            'エラーログ出力
            Logger.Error(LogResultId & CType(messageId, String) & ErrorMessageParameter)

            '終了ログ出力
            Logger.Info(getReturnParam(True.ToString), True)
            Logger.Info(getLogParam("messageId", CStr(messageId), True), True)
            Logger.Info(getLogMethod(GetCurrentMethod.Name, False), True)
            Return True
        End If

        '終了ログ出力
        Logger.Info(getReturnParam(False.ToString), True)
        Logger.Info(getLogParam("messageId", CStr(messageId), True), True)
        Logger.Info(getLogMethod(GetCurrentMethod.Name, False), True)
        Return False
    End Function

    ''' <summary>
    ''' 依頼ID入力チェック
    ''' </summary>
    ''' <param name="requestId">依頼ID</param>
    ''' <param name="messageId">メッセージID</param>
    ''' <returns>True:エラーあり,False:エラーなし</returns>
    ''' <remarks></remarks>
    Private Function IsErrorRequestId(ByVal requestId As String,
                                      ByRef requestIdValue As Long, _
                                      ByRef messageId As Integer) As Boolean
        '開始ログ出力
        Logger.Info(getLogMethod(GetCurrentMethod.Name, True), True)
        Logger.Info(getLogParam("requestId", requestId, False), True)
        Logger.Info(getLogParam("requestIdValue", CStr(requestIdValue), True), True)
        Logger.Info(getLogParam("messageId", CStr(messageId), True), True)

        requestIdValue = 0

        ' 必須項目チェック
        If String.IsNullOrEmpty(requestId) Then
            messageId = ResultCodeErrorMust + ItemNo.RequestId
            'エラーログ出力
            Logger.Error(LogResultId & CType(messageId, String) & ErrorMessageParameter)

            '終了ログ出力
            Logger.Info(getReturnParam(True.ToString), True)
            Logger.Info(getLogParam("messageId", CStr(messageId), True), True)
            Logger.Info(getLogMethod(GetCurrentMethod.Name, False), True)
            Return True
        End If

        ' 数値チェック
        If Not Long.TryParse(requestId, requestIdValue) Then
            messageId = ResultCodeErrorType + ItemNo.RequestId
            'エラーログ出力
            Logger.Error(LogResultId & CType(messageId, String) & ErrorMessageParameter)

            '終了ログ出力
            Logger.Info(getReturnParam(True.ToString), True)
            Logger.Info(getLogParam("messageId", CStr(messageId), True), True)
            Logger.Info(getLogMethod(GetCurrentMethod.Name, False), True)
            Return True
        End If

        ' 桁チェック
        If Not Validation.IsCorrectDigit(requestId, ItemLength.RequestId) Then
            messageId = ResultCodeErrorSize + ItemNo.RequestId
            'エラーログ出力
            Logger.Error(LogResultId & CType(messageId, String) & ErrorMessageParameter)

            '終了ログ出力
            Logger.Info(getReturnParam(True.ToString), True)
            Logger.Info(getLogParam("messageId", CStr(messageId), True), True)
            Logger.Info(getLogMethod(GetCurrentMethod.Name, False), True)
            Return True
        End If

        '終了ログ出力
        Logger.Info(getReturnParam(False.ToString), True)
        Logger.Info(getLogParam("messageId", CStr(messageId), True), True)
        Logger.Info(getLogMethod(GetCurrentMethod.Name, False), True)
        Return False
    End Function

    ''' <summary>
    ''' 取得データ開始位置入力チェック
    ''' </summary>
    ''' <param name="dataFrom">取得データ開始位置</param>
    ''' <param name="dataFromValue">取得データ開始位置(数値型)</param>
    ''' <param name="messageId">メッセージID</param>
    ''' <returns>True:エラーあり,False:エラーなし</returns>
    ''' <remarks></remarks>
    Private Function IsErrorDataFrom(ByVal dataFrom As String, _
                                     ByRef dataFromValue As Integer, _
                                     ByRef messageId As Integer) As Boolean
        '開始ログ出力
        Logger.Info(getLogMethod(GetCurrentMethod.Name, True), True)
        Logger.Info(getLogParam("dataFrom", dataFrom, False), True)
        Logger.Info(getLogParam("dataFromValue", CStr(dataFromValue), True), True)
        Logger.Info(getLogParam("messageId", CStr(messageId), True), True)

        dataFromValue = 0

        ' 必須項目チェック
        If String.IsNullOrEmpty(dataFrom) Then
            messageId = ResultCodeErrorMust + ItemNo.DataFrom
            'エラーログ出力
            Logger.Error(LogResultId & CType(messageId, String) & ErrorMessageParameter)

            '終了ログ出力
            Logger.Info(getReturnParam(True.ToString), True)
            Logger.Info(getLogParam("messageId", CStr(messageId), True), True)
            Logger.Info(getLogMethod(GetCurrentMethod.Name, False), True)
            Return True
        End If

        ' 数値チェック
        If Not Integer.TryParse(dataFrom, dataFromValue) Then
            messageId = ResultCodeErrorType + ItemNo.DataFrom
            'エラーログ出力
            Logger.Error(LogResultId & CType(messageId, String) & ErrorMessageParameter)

            '終了ログ出力
            Logger.Info(getReturnParam(True.ToString), True)
            Logger.Info(getLogParam("messageId", CStr(messageId), True), True)
            Logger.Info(getLogMethod(GetCurrentMethod.Name, False), True)
            Return True
        End If

        ' 桁チェック
        If Not Validation.IsCorrectDigit(dataFrom, ItemLength.DataFrom) Then
            messageId = ResultCodeErrorSize + ItemNo.DataFrom
            'エラーログ出力
            Logger.Error(LogResultId & CType(messageId, String) & ErrorMessageParameter)

            '終了ログ出力
            Logger.Info(getReturnParam(True.ToString), True)
            Logger.Info(getLogParam("messageId", CStr(messageId), True), True)
            Logger.Info(getLogMethod(GetCurrentMethod.Name, False), True)
            Return True
        End If

        '終了ログ出力
        Logger.Info(getReturnParam(False.ToString), True)
        Logger.Info(getLogParam("messageId", CStr(messageId), True), True)
        Logger.Info(getLogMethod(GetCurrentMethod.Name, False), True)
        Return False
    End Function

    ''' <summary>
    ''' 取得データ終了位置入力チェック
    ''' </summary>
    ''' <param name="dataTo">取得データ終了位置</param>
    ''' <param name="dataToValue">取得データ終了位置(数値型)</param>
    ''' <param name="messageId">メッセージID</param>
    ''' <returns>True:エラーあり,False:エラーなし</returns>
    ''' <remarks></remarks>
    Private Function IsErrorDataTo(ByVal dataTo As String, _
                                   ByRef dataToValue As Integer, _
                                   ByRef messageId As Integer) As Boolean
        '開始ログ出力
        Logger.Info(getLogMethod(GetCurrentMethod.Name, True), True)
        Logger.Info(getLogParam("dataTo", dataTo, False), True)
        Logger.Info(getLogParam("dataToValue", CStr(dataToValue), True), True)
        Logger.Info(getLogParam("messageId", CStr(messageId), True), True)

        dataToValue = 0

        ' 必須項目チェック
        If String.IsNullOrEmpty(dataTo) Then
            messageId = ResultCodeErrorMust + ItemNo.DataTo
            'エラーログ出力
            Logger.Error(LogResultId & CType(messageId, String) & ErrorMessageParameter)

            '終了ログ出力
            Logger.Info(getReturnParam(True.ToString), True)
            Logger.Info(getLogParam("messageId", CStr(messageId), True), True)
            Logger.Info(getLogMethod(GetCurrentMethod.Name, False), True)
            Return True
        End If

        ' 数値チェック
        If Not Integer.TryParse(dataTo, dataToValue) Then
            messageId = ResultCodeErrorType + ItemNo.DataTo
            'エラーログ出力
            Logger.Error(LogResultId & CType(messageId, String) & ErrorMessageParameter)

            '終了ログ出力
            Logger.Info(getReturnParam(True.ToString), True)
            Logger.Info(getLogParam("messageId", CStr(messageId), True), True)
            Logger.Info(getLogMethod(GetCurrentMethod.Name, False), True)
            Return True
        End If

        ' 桁チェック
        If Not Validation.IsCorrectDigit(dataTo, ItemLength.DataTo) Then
            messageId = ResultCodeErrorSize + ItemNo.DataTo
            'エラーログ出力
            Logger.Error(LogResultId & CType(messageId, String) & ErrorMessageParameter)

            '終了ログ出力
            Logger.Info(getReturnParam(True.ToString), True)
            Logger.Info(getLogParam("messageId", CStr(messageId), True), True)
            Logger.Info(getLogMethod(GetCurrentMethod.Name, False), True)
            Return True
        End If

        '終了ログ出力
        Logger.Info(getReturnParam(False.ToString), True)
        Logger.Info(getLogParam("messageId", CStr(messageId), True), True)
        Logger.Info(getLogMethod(GetCurrentMethod.Name, False), True)
        Return False
    End Function

    ''' <summary>
    ''' 取得データ位置相関入力チェック
    ''' </summary>
    ''' <param name="dataFrom">取得データ開始位置</param>
    ''' <param name="dataTo">取得データ終了位置</param>
    ''' <param name="messageId">メッセージID</param>
    ''' <returns>True:エラーあり,False:エラーなし</returns>
    ''' <remarks></remarks>
    Private Function IsErrorDataFromTo(ByVal dataFrom As Integer, _
                                       ByVal dataTo As Integer, _
                                       ByRef messageId As Integer) As Boolean
        '開始ログ出力
        Logger.Info(getLogMethod(GetCurrentMethod.Name, True), True)
        Logger.Info(getLogParam("dataFrom", CStr(dataFrom), False), True)
        Logger.Info(getLogParam("dataTo", CStr(dataTo), True), True)
        Logger.Info(getLogParam("messageId", CStr(messageId), True), True)

        ' 必須項目チェック
        If 0 < dataFrom.CompareTo(dataTo) Then
            messageId = ResultCodeErrorValue + ItemNo.DataTo
            'エラーログ出力
            Logger.Error(LogResultId & CType(messageId, String) & ErrorMessageParameter)

            '終了ログ出力
            Logger.Info(getReturnParam(True.ToString), True)
            Logger.Info(getLogParam("messageId", CStr(messageId), True), True)
            Logger.Info(getLogMethod(GetCurrentMethod.Name, False), True)
            Return True
        End If

        '終了ログ出力
        Logger.Info(getReturnParam(False.ToString), True)
        Logger.Info(getLogParam("messageId", CStr(messageId), True), True)
        Logger.Info(getLogMethod(GetCurrentMethod.Name, False), True)
        Return False
    End Function

    ''' <summary>
    ''' ステータスチェック
    ''' 　ステータスが"1"(依頼)、"2"(キャンセル)、"3"(受信)、"4"(受付)の
    ''' 　いずれかでない場合、エラー
    ''' </summary>
    ''' <param name="status">ステータス</param>
    ''' <param name="messageId">メッセージID</param>
    ''' <returns>True:エラーあり,False:エラーなし</returns>
    ''' <remarks></remarks>
    Private Function IsErrorStatus(ByVal status As String, _
                                   ByRef messageId As Integer) As Boolean
        '開始ログ出力
        Logger.Info(getLogMethod(GetCurrentMethod.Name, True), True)
        Logger.Info(getLogParam("status", status, False), True)
        Logger.Info(getLogParam("messageId", CStr(messageId), True), True)

        Dim isError As Boolean = True

        Select Case status
            Case STATUS_REQUEST, _
                 STATUS_CANCEL, _
                 STATUS_RECEPTION,
                 STATUS_RECEPTIONIST
                'ステータスが"1"(依頼)、"2"(キャンセル)、"3"(受信)、"4"(受付)の
                'いずれかの場合
                isError = False

            Case Else
                'ステータスが"1"(依頼)、"2"(キャンセル)、"3"(受信)、"4"(受付)の
                'いずれかでない場合
                isError = True
                messageId = ResultCodeErrorValue + ItemNo.Status
                'エラーログ出力
                Logger.Info(getLogParam("messageId", CStr(messageId), True), True)
                Logger.Error(LogResultId & CType(messageId, String) & ErrorMessageStatus)

        End Select

        '終了ログ出力
        Logger.Info(getReturnParam(isError.ToString), True)
        Logger.Info(getLogParam("messageId", CStr(messageId), True), True)
        Logger.Info(getLogMethod(GetCurrentMethod.Name, False), True)

        Return isError
    End Function

#End Region

#Region "ログデータ加工処理"
    ''' <summary>
    ''' ログデータ（メソッド）
    ''' </summary>
    ''' <param name="methodName">メソッド名</param>
    ''' <param name="startEndFlag">True：「method start」を表示、False：「method end」を表示</param>
    ''' <returns>加工した文字列</returns>
    ''' <remarks></remarks>
    Private Function getLogMethod(ByVal methodName As String,
                                ByVal startEndFlag As Boolean) As String
        Dim sb As New StringBuilder
        With sb
            .Append("[")
            .Append(methodName)
            .Append("]")
            If startEndFlag Then
                .Append(" method start")
            Else
                .Append(" method end")
            End If
        End With
        Return sb.ToString
    End Function

    ''' <summary>
    ''' ログデータ（引数）
    ''' </summary>
    ''' <param name="paramName">引数名</param>
    ''' <param name="paramData">引数値</param>
    ''' <param name="kanmaFlag">True：引数名の前に「,」を表示、False：特になし</param>
    ''' <returns>加工した文字列</returns>
    ''' <remarks></remarks>
    Private Function getLogParam(ByVal paramName As String,
                                 ByVal paramData As String,
                                 ByVal kanmaFlag As Boolean) As String
        Dim sb As New StringBuilder
        With sb
            If kanmaFlag Then
                .Append(",")
            End If
            .Append(paramName)
            .Append("=")
            .Append(paramData)
        End With
        Return sb.ToString
    End Function

    ''' <summary>
    ''' ログデータ（戻り値）
    ''' </summary>
    ''' <param name="paramData">引数値</param>
    ''' <returns>加工した文字列</returns>
    ''' <remarks></remarks>
    Private Function getReturnParam(ByVal paramData As String) As String
        Dim sb As New StringBuilder
        With sb
            .Append("Return=")
            .Append(paramData)
        End With
        Return sb.ToString
    End Function

    ''' <summary>
    ''' ログデータ（DataSet）
    ''' 　データセット内部のデータテーブルの件数からログ出力用文字列を作成する
    ''' </summary>
    ''' <param name="paramDataSet">引数値</param>
    ''' <returns>加工した文字列</returns>
    ''' <remarks></remarks>
    Private Function getReturnDataSet(ByVal paramDataSet As IC3060102DataSet) As String
        Dim sb As New StringBuilder
        With sb
            .Append("Return DataSet =")
            Dim i As Integer = 0
            For Each dt As DataTable In paramDataSet.Tables
                If 0 < i Then
                    .Append(",")
                End If
                .Append(dt.TableName)
                .Append(" Count:")
                .Append(CStr(dt.Rows.Count))

                i = i + 1
            Next dt
        End With
        Return sb.ToString
    End Function
#End Region

End Class
