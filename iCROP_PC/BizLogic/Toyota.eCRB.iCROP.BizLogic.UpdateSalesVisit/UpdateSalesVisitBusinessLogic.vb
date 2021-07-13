'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'UpdateSalesVisitBusinessLogic.vb
'──────────────────────────────────
'機能： セールス来店実績更新
'補足： 
'作成： 2011/12/12 KN k.nagasawa
'更新： 2012/02/13 KN y.nakamura STEP2開発 $01
'更新： 2012/08/23 TMEJ m.okamura 新車受付機能改善 $02
'更新： 2013/02/26 TMEJ t.shimamura 新車タブレット受付画面管理指標の変更対応 $03
'──────────────────────────────────

Option Strict On
Option Explicit On

Imports System.Globalization
Imports System.IO
Imports System.Net
Imports System.Text
Imports Toyota.eCRB.Common.VisitResult.DataAccess.UpdateSalesVisitDataSet
Imports Toyota.eCRB.Common.VisitResult.DataAccess.UpdateSalesVisitDataSetTableAdapters
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.Visit.Api.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Web
' $01 start step2開発
Imports Toyota.eCRB.Visit.Api.DataAccess
' $01 end   step2開発

''' <summary>
''' セールス来店実績更新ビジネスロジックの実装クラス
''' </summary>
''' <remarks></remarks>
Public Class UpdateSalesVisitBusinessLogic

#Region "列挙体"

    ''' <summary>
    ''' メッセージIDの列挙体
    ''' </summary>
    ''' <remarks></remarks>
    Private Enum Message As Integer
        ''' <summary>
        ''' 正常終了
        ''' </summary>
        ''' <remarks></remarks>
        None = 0

        ''' <summary>
        ''' 顧客区分、顧客コードに該当する来店実績情報が存在しない
        ''' </summary>
        ''' <remarks></remarks>
        NotExistsCustomerInfo = 1102

        ''' <summary>
        ''' 顧客区分が未設定
        ''' </summary>
        ''' <remarks></remarks>
        EmptyCustomerSegment = 2002

        ''' <summary>
        ''' 顧客コードが未設定
        ''' </summary>
        ''' <remarks></remarks>
        EmptyCustomerId = 2003

        ''' <summary>
        ''' 商談開始日時が未設定
        ''' </summary>
        ''' <remarks></remarks>
        EmptySalesStart = 2009

        ''' <summary>
        ''' 商談終了日時が未設定
        ''' </summary>
        ''' <remarks></remarks>
        EmptySalesEnd = 2010

        ''' <summary>
        ''' 更新機能IDが未設定
        ''' </summary>
        ''' <remarks></remarks>
        EmptyUpdateId = 2012

        ''' <summary>
        ''' 来店実績連番が未設定
        ''' </summary>
        ''' <remarks></remarks>
        EmptyVisitSeq = 2013

        ''' <summary>
        ''' 顧客担当スタッフコードが未設定
        ''' </summary>
        ''' <remarks></remarks>
        EmptyStaffCode = 2014

        ''' <summary>
        ''' 別のスタッフによって商談が開始された
        ''' </summary>
        ''' <remarks></remarks>
        AlreadyStarted = 5002

        ''' <summary>
        ''' 商談終了できない来店実績情報が指定された
        ''' </summary>
        ''' <remarks></remarks>
        CannotSalesEnd = 5003

        ''' <summary>
        ''' 別のスタッフによって顧客情報の登録が行われた
        ''' </summary>
        ''' <remarks></remarks>
        AlreadyUpdatedCustomerInfo = 5004

        '$03 start 納車作業対応
        ''' <summary>
        ''' 別のスタッフによって納車作業が開始された
        ''' </summary>
        ''' <remarks></remarks>
        AlreadyDeliverlyStarted = 5005

        ''' <summary>
        ''' 商談終了できない来店実績情報が指定された
        ''' </summary>
        ''' <remarks></remarks>
        CannotDeliverlyEnd = 5006
        '$03 start 納車作業対応

        ''' <summary>
        ''' データ更新エラー
        ''' </summary>
        ''' <remarks></remarks>
        UpdateError = 9001
    End Enum

#End Region

#Region "定数"
    ' $03 start 納車作業ステータス対応
#Region "商談開始処理区分"
    ''' <summary>
    ''' 処理区分：商談開始
    ''' </summary>
    ''' <remarks></remarks>
    Public Const LogicStateNegotiationStart As String = "1"
    ''' <summary>
    ''' 処理区分：納車作業開始
    ''' </summary>
    ''' <remarks></remarks>
    Public Const LogicStateDeliverly As String = "3"

#End Region
    ' $03 end   納車作業ステータス対応
    ' $02 start 複数顧客に対する商談平行対応
#Region "商談終了処理区分"

    ''' <summary>
    ''' 処理区分：商談終了
    ''' </summary>
    ''' <remarks></remarks>
    Public Const LogicStateNegotiationFinish As String = "1"

    ''' <summary>
    ''' 処理区分：商談中断
    ''' </summary>
    ''' <remarks></remarks>
    Public Const LogicStateNegotiationStop As String = "2"

    ' $03 start 納車作業ステータス対応
    ''' <summary>
    ''' 処理区分：納車作業終了
    ''' </summary>
    ''' <remarks></remarks>
    Public Const LogicStateDeliverlyFinish As String = "3"
    ' $03 start 納車作業ステータス対応

#End Region
    ' $02 end   複数顧客に対する商談平行対応

#Region "デバッグ用"

#Region "メソッド名"

    ''' <summary>
    ''' メソッド名（来店実績更新_商談または納車作業開始）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MethodNameUpdateVisitSalesStart As String = "UpdateSalesVisitBusinessLogic.UpdateVisitSalesStart"

    ''' <summary>
    ''' メソッド名（来店実績更新_商談または納車作業開始の引数チェック）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MethodNameIsValidUpdateVisitSalesStartParameter As String = "UpdateSalesVisitBusinessLogic.IsValidUpdateVisitSalesStartParameter"

    ''' <summary>
    ''' メソッド名（顧客区分、顧客コードの入力値チェック）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MethodNameIsValidCustomerInfo As String = "UpdateSalesVisitBusinessLogic.IsValidCustomerInfo"

    ''' <summary>
    ''' メソッド名（顧客担当スタッフコードの入力値チェック）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MethodNameIsValidStaffCode As String = "UpdateSalesVisitBusinessLogic.IsValidStaffCode"

    ''' <summary>
    ''' メソッド名（更新機能IDの入力値チェック）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MethodNameIsValidUpdateId As String = "UpdateSalesVisitBusinessLogic.IsValidUpdateId"

    ''' <summary>
    ''' メソッド名（来店日時開始の取得）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MethodNameGetVisitTimestampStart As String = "UpdateSalesVisitBusinessLogic.GetVisitTimestampStart"

    ''' <summary>
    ''' メソッド名（来店日時終了の取得）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MethodNameGetVisitTimestampEnd As String = "UpdateSalesVisitBusinessLogic.GetVisitTimestampEnd"

    ''' <summary>
    ''' メソッド名（Push送信）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MethodNamePush As String = "UpdateSalesVisitBusinessLogic.Push"

    ''' <summary>
    ''' メソッド名（Push対象のアカウント情報の取得）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MethodNameGetPushUser As String = "UpdateSalesVisitBusinessLogic.GetPushUser"

    ' $01 start step2開発
    ''' <summary>
    ''' メソッド名（Push送信）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MethodNameTabletSendPush As String = "UpdateSalesVisitBusinessLogic.TabletSendPush"

    ''' <summary>
    ''' メソッド名（Push送信）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MethodNamePcSendPush As String = "UpdateSalesVisitBusinessLogic.PcSendPush"
    ' $01 end   step2開発

    ''' <summary>
    ''' メソッド名（来店実績更新_商談または納車作業終了）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MethodNameUpdateVisitSalesEnd As String = "UpdateSalesVisitBusinessLogic.UpdateVisitSalesEnd"

    ''' <summary>
    ''' メソッド名（来店実績更新_商談または納車作業終了の引数チェック）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MethodNameIsValidUpdateUpdateVisitSalesEndParameter As String = "UpdateSalesVisitBusinessLogic.IsValidUpdateUpdateVisitSalesEndParameter"

    ''' <summary>
    ''' メソッド名（来店実績更新_顧客登録）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MethodNameUpdateVisitCustomerInfo As String = "UpdateSalesVisitBusinessLogic.UpdateVisitCustomerInfo"

    ''' <summary>
    ''' メソッド名（来店実績更新_顧客登録の引数チェック）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MethodNameIsValidUpdateVisitCustomerInfoParameter As String = "UpdateSalesVisitBusinessLogic.IsValidUpdateVisitCustomerInfoParameter"

    ''' <summary>
    ''' メソッド名（来店実績更新_ログイン）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MethodNameUpdateVisitLogin As String = "UpdateSalesVisitBusinessLogic.UpdateVisitLogin"

    ''' <summary>
    ''' メソッド名（商談開始前の来店実績連番の取得）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MethodNameGetVisitSeqBeforeSalesStart As String = "UpdateSalesVisitBusinessLogic.GetVisitSeqBeforeSalesStart"

#End Region

#Region "ログのフォーマット"

    ''' <summary>
    ''' ログのフォーマット（引数が Nothing または空文字）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FormatIsNullOrEmptyParameterLog As String = "Invalid parameter: {0} is null or empty. messageId = {1}"

    ''' <summary>
    ''' ログのフォーマット（引数が空）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FormatIsEmptyParameterLog As String = "Invalid parameter: {0} is empty. messageId = {1}"

    ''' <summary>
    ''' ログのフォーマット（来店実績ステータスが「商談中」の場合）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FormatAlreadyStartedLog As String = "Business discussion has already started. messageId = {0}"

    '$03 start 納車作業対応
    ''' <summary>
    ''' ログのフォーマット（来店実績ステータスが「商談中」の場合）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FormatAlreadyDeliverlyStartedLog As String = "Deliverly has already started. messageId = {0}"
    '$03 end   納車作業対応

    ''' <summary>
    ''' ログのフォーマット（登録・更新レコード0件）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FormatConcurrencyViolationLog As String = "Processed out 0 of the expected 1 or more records. messageId = {0}"

    ''' <summary>
    ''' ログのフォーマット（OracleExceptionEx 発生）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FormatOracleExceptionExLog As String = "An exception occurred during the operation of the database. messageId = {0}"

    ''' <summary>
    ''' ログのフォーマット（顧客情報に該当する来店実績情報が存在しない）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FormatNotExistsCustomerInfoLog As String = "There is no record corresponding to the CustomerInfo. messageId = {0}"

    ''' <summary>
    ''' ログのフォーマット（対応担当アカウントとログインアカウントが一致しない）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FormatCannotSalesEndLog As String = "ACCOUNT is not consistent with the login account. messageId = {0}"


#End Region

#Region "配列の名前"

    ''' <summary>
    ''' 配列の名前（引数）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ArrayNameParameters As String = "Param"

    ''' <summary>
    ''' 配列の名前（戻り値）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ArrayNameReturnValues As String = "Ret"

#End Region

#Region "マーカー名"

    ''' <summary>
    ''' マーカー名（UpdateSalesVisitBusinessLogic.UpdateVisitSalesStart-外部メソッドの呼び出し）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MarkerNameUpdateVisitSalesStart003 As String = "UpdateSalesVisitBusinessLogic.UpdateVisitSalesStart_003"

    ''' <summary>
    ''' マーカー名（UpdateSalesVisitBusinessLogic.GetVisitTimestampStart-外部メソッドの呼び出し）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MarkerNameGetVisitTimestampStart001 As String = "UpdateSalesVisitBusinessLogic.GetVisitTimestampStart_001"

    ''' <summary>
    ''' マーカー名（UpdateSalesVisitBusinessLogic.Push-外部メソッドの呼び出し）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MarkerNamePush001 As String = "UpdateSalesVisitBusinessLogic.Push_003"

    ''' <summary>
    ''' マーカー名（UpdateSalesVisitBusinessLogic.GetPushUser-外部メソッドの呼び出し）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MarkerNameGetPushUser001 As String = "UpdateSalesVisitBusinessLogic.GetPushUser_001"

    ''' <summary>
    ''' マーカー名（UpdateSalesVisitBusinessLogic.UpdateVisitSalesEnd-外部メソッドの呼び出し）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MarkerNameUpdateVisitSalesEnd003 As String = "UpdateSalesVisitBusinessLogic.UpdateVisitSalesEnd_003"

    ''' <summary>
    ''' マーカー名（UpdateSalesVisitBusinessLogic.UpdateVisitCustomerInfo-外部メソッドの呼び出し）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MarkerNameGetVisitUpdateVisitCustomerInfo003 As String = "UpdateSalesVisitBusinessLogic.UpdateVisitCustomerInfo_003"

    ''' <summary>
    ''' マーカー名（UpdateSalesVisitBusinessLogic.UpdateVisitLogin-外部メソッドの呼び出し）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MarkerNameUpdateVisitLogin003 As String = "UpdateSalesVisitBusinessLogic.UpdateVisitLogin_003"

    ''' <summary>
    ''' マーカー名（UpdateSalesVisitBusinessLogic.GetVisitSeqBeforeSalesStart-外部メソッドの呼び出し）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MarkerNameGetVisitSeqBeforeSalesStart003 As String = "UpdateSalesVisitBusinessLogic.GetVisitSeqBeforeSalesStart_003"

#End Region

#Region "外部メソッド名"

    ''' <summary>
    ''' 外部メソッド名（StaffContext.Current）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MethodNameStaffContextCurrent As String = "StaffContext.Current"

    ''' <summary>
    ''' 外部メソッド名（DateTimeFunc.Now）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MethodNameDateTimeFuncNow As String = "DateTimeFunc.Now"

    ''' <summary>
    ''' 外部メソッド名（Users.GetAllUser）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MethodNameUsersGetAllUser As String = "Users.GetAllUser"

#End Region

#End Region

#Region "来店実績ステータス"

    ''' <summary>
    ''' 来店実績ステータス（07:商談中）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VisitStatusSalesStart As String = "07"

    ' $03 start 納車作業ステータス対応
    ''' <summary>
    ''' 来店実績ステータス（11:納車作業中）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VisitStatusDeliverlyStart As String = "11"
    ' $03 end 納車作業ステータス対応

#End Region

#Region "Date操作"

    ''' <summary>
    ''' 加算する日数（翌日）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NextDayValue As Double = 1.0

    ''' <summary>
    ''' 加算するミリ秒（1ミリ秒前）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OneMsBeforeValue As Double = -1.0

#End Region

#Region "Push送信"

    ''' <summary>
    ''' 送信元（顧客情報画面）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SourceCustomerInfo As String = "03"

    ''' <summary>
    ''' 送信時処理（商談開始）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ProcessSalesStart As String = "01"

    ''' <summary>
    ''' 送信時処理（商談終了）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ProcessSalesEnd As String = "02"

    ' $02 start 複数顧客に対する商談平行対応
    ''' <summary>
    ''' 送信時処理（商談中断）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ProcessSalesStop As String = "09"
    ' $02 end   複数顧客に対する商談平行対応

    ''' <summary>
    ''' 削除フラグ（未削除）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DeleteFlgNone As String = "0"

    ''' <summary>
    ''' 操作権限コード（受付）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OperationCodeReception As Decimal = 51D

    ' $01 start step2開発
    ''' <summary>
    ''' 操作権限コード（セールスマネージャ）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OperationCodeSalesStaffManager As Decimal = 7D

    '$03 start SSV 廃止
    ' ''' <summary>
    ' ''' 操作権限コード（SSV）
    ' ''' </summary>
    ' ''' <remarks></remarks>
    ' Private Const OperationCodeSsv As Decimal = 53D
    ' $03 end SSV 廃止
    ' $01 end   step2開発

#End Region

#End Region

#Region "メソッド"

#Region "来店実績更新_商談または納車作業開始"

    ''' <summary>
    ''' 来店実績更新_商談または納車作業開始
    ''' </summary>
    ''' <param name="customerSegment">顧客区分</param>
    ''' <param name="customerId">顧客コード</param>
    ''' <param name="staffCode">顧客担当スタッフコード</param>
    ''' <param name="followUpBoxDealerCode">Follow-up Box販売店コード</param>
    ''' <param name="followUpBoxStoreCode">Follow-up Box店舗コード</param>
    ''' <param name="followUpBoxSeqNo">Follow-up Box内連番</param>
    ''' <param name="salesStart">商談開始日時</param>
    ''' <param name="updateId">更新機能ID</param>
    ''' <param name="messageId">メッセージID</param>
    ''' <param name="statusClass">ステータス区分</param>
    ''' <remarks>
    ''' 商談または納車作業開始時に必要な来店実績データの更新・登録を行う。
    ''' 本メソッドでは、以下のメソッドも利用しているため、例外情報に関しては、下記、メソッドを参照してください。
    ''' <seealso cref="UpdateSalesVisitTableAdapter.GetVisitSalesStart" />
    ''' <seealso cref="UpdateSalesVisitTableAdapter.UpdateVisitSalesStart" />
    ''' <seealso cref="UpdateSalesVisitTableAdapter.GetVisitSalesSeqNextValue" />
    ''' <seealso cref="UpdateSalesVisitTableAdapter.InsertVisitSales" />
    ''' </remarks>
    Public Sub UpdateVisitSalesStart( _
            ByVal customerSegment As String, ByVal customerId As String,
            ByVal staffCode As String, ByVal followUpBoxDealerCode As String, _
            ByVal followUpBoxStoreCode As String, ByVal followUpBoxSeqNo As Decimal, _
            ByVal salesStart As Date, ByVal updateId As String, ByRef messageId As Integer, _
            ByVal statusClass As String)

        OutputStartLog(MethodNameUpdateVisitSalesStart, customerSegment, customerId, _
                staffCode, followUpBoxDealerCode, followUpBoxStoreCode, followUpBoxSeqNo, _
                salesStart, updateId, messageId)

        ' 引数チェック
        Dim isValidParameter As Boolean = IsValidUpdateVisitSalesStartParameter(customerSegment, _
                customerId, staffCode, salesStart, updateId, messageId)

        ' 引数チェックが異常の場合は、処理を終了する
        If Not isValidParameter Then
            Logger.Info("UpdateSalesVisitBusinessLogic.UpdateVisitSalesStart_001")
            OutputEndLog(MethodNameUpdateVisitSalesStart, messageId)
            Return
        End If

        Logger.Info("UpdateSalesVisitBusinessLogic.UpdateVisitSalesStart_002")

        ' ログイン情報を取得
        OutputCallStartLog(MarkerNameUpdateVisitSalesStart003, MethodNameStaffContextCurrent)
        Dim staffInfo As StaffContext = StaffContext.Current()
        OutputCallEndLog(MarkerNameUpdateVisitSalesStart003, MethodNameStaffContextCurrent, _
                staffInfo)

        ' 日付管理（現在日付の取得）
        Dim visitTimestampStart As Date = GetVisitTimestampStart(staffInfo.DlrCD)
        Dim visitTimestampEnd As Date = GetVisitTimestampEnd(visitTimestampStart)

        Try
            Dim isSuccess As Boolean = False

            Using ta As New UpdateSalesVisitTableAdapter
                ' 商談開始時の来店実績の取得
                Using dt As UpdateSalesVisitDataTable = ta.GetVisitSalesStart(staffInfo.DlrCD, _
                        staffInfo.BrnCD, customerSegment, customerId, visitTimestampStart, _
                        visitTimestampEnd)

                    ' 来店実績情報が取得できた場合
                    If 0 < dt.Count Then
                        Logger.Info("UpdateSalesVisitBusinessLogic.UpdateVisitSalesStart_004")

                        Dim dr As UpdateSalesVisitRow = dt.Item(0)

                        ' 来店実績ステータスが「商談中」の場合
                        If dr.VISITSTATUS.Equals(VisitStatusSalesStart.PadRight( _
                                dr.VISITSTATUS.Length)) Then
                            Logger.Info("UpdateSalesVisitBusinessLogic.UpdateVisitSalesStart_005a")

                            ' メッセージIDを設定
                            messageId = Message.AlreadyStarted

                            Logger.Debug(String.Format(CultureInfo.InvariantCulture, _
                                    FormatAlreadyStartedLog, messageId))
                            OutputEndLog(MethodNameUpdateVisitSalesStart, messageId)
                            Return

                            ' $03 start 来店実績ステータスが「納車作業中」の場合
                        ElseIf dr.VISITSTATUS.Equals(VisitStatusDeliverlyStart.PadRight( _
                                dr.VISITSTATUS.Length)) Then
                            Logger.Info("UpdateSalesVisitBusinessLogic.UpdateVisitSalesStart_005b")

                            ' メッセージIDを設定
                            messageId = Message.AlreadyDeliverlyStarted

                            Logger.Debug(String.Format(CultureInfo.InvariantCulture, _
                                    FormatAlreadyDeliverlyStartedLog, messageId))
                            OutputEndLog(MethodNameUpdateVisitSalesStart, messageId)
                            Return
                            ' $03 end   来店実績ステータスが「納車作業中」の場合
                        End If

                        Logger.Info("UpdateSalesVisitBusinessLogic.UpdateVisitSalesStart_006")

                        If statusClass = LogicStateNegotiationStart Then
                            ' 商談開始時の来店実績の更新
                            isSuccess = ta.UpdateVisitSalesStart(dr.VISITSEQ, staffInfo.Account, _
                                    followUpBoxDealerCode, followUpBoxStoreCode, followUpBoxSeqNo, _
                                    salesStart, staffInfo.Account, updateId, LogicStateNegotiationStart)
                        Else
                            ' 納車作業開始時の来店実績の更新
                            isSuccess = ta.UpdateVisitSalesStart(dr.VISITSEQ, staffInfo.Account, _
                                    followUpBoxDealerCode, followUpBoxStoreCode, followUpBoxSeqNo, _
                                    salesStart, staffInfo.Account, updateId, LogicStateDeliverly)
                        End If

                        ' 来店実績情報が取得できない場合
                    Else
                        Logger.Info("UpdateSalesVisitBusinessLogic.UpdateVisitSalesStart_007")

                        ' 来店実績シーケンスの取得
                        Dim newVisitseq As Long = ta.GetVisitSalesSeqNextValue()
                        If statusClass = LogicStateNegotiationFinish Then
                            ' 来店実績の登録
                            isSuccess = ta.InsertVisitSales(newVisitseq, staffInfo.DlrCD, _
                                    staffInfo.BrnCD, customerSegment, customerId, staffCode, _
                                    staffInfo.Account, followUpBoxDealerCode, followUpBoxStoreCode, _
                                    followUpBoxSeqNo, salesStart, staffInfo.Account, updateId, LogicStateNegotiationFinish)
                        Else
                            isSuccess = ta.InsertVisitSales(newVisitseq, staffInfo.DlrCD, _
                                    staffInfo.BrnCD, customerSegment, customerId, staffCode, _
                                    staffInfo.Account, followUpBoxDealerCode, followUpBoxStoreCode, _
                                    followUpBoxSeqNo, salesStart, staffInfo.Account, updateId, LogicStateDeliverlyFinish)
                        End If
                    End If
                End Using
            End Using

            staffInfo = Nothing

            ' 処理結果が失敗の場合は、処理を中断
            If Not isSuccess Then
                Logger.Info("UpdateSalesVisitBusinessLogic.UpdateVisitSalesStart_008")

                ' メッセージIDを設定
                messageId = Message.UpdateError

                Logger.Debug(String.Format(CultureInfo.InvariantCulture, _
                        FormatConcurrencyViolationLog, messageId))
                OutputEndLog(MethodNameUpdateVisitSalesStart, messageId)
                Return
            End If

            Logger.Info("UpdateSalesVisitBusinessLogic.UpdateVisitSalesStart_009")

            ' データベースの操作中に例外が発生した場合
        Catch ex As OracleExceptionEx
            Logger.Info("UpdateSalesVisitBusinessLogic.UpdateVisitSalesStart_010")

            ' メッセージIDを設定
            messageId = Message.UpdateError

            Logger.Error(String.Format(CultureInfo.InvariantCulture, FormatOracleExceptionExLog, _
                    messageId), ex)
            OutputExLog(MethodNameUpdateVisitSalesStart, ex, messageId)
            Throw
        End Try

        Logger.Info("UpdateSalesVisitBusinessLogic.UpdateVisitSalesStart_011")

        ' メッセージIDを設定
        messageId = Message.None

        OutputEndLog(MethodNameUpdateVisitSalesStart, messageId)

    End Sub

    ''' <summary>
    ''' 来店実績更新_商談または納車作業開始時のPush送信
    ''' </summary>
    ''' <remarks>
    ''' 商談または納車作業開始の処理が全て終了した後に呼び出され、Push送信を行う
    ''' </remarks>
    Public Sub PushUpdateVisitSalesStart()

        Logger.Info("UpdateSalesVisitBusinessLogic.PushUpdateVisitSalesStart_Start")

        ' Push送信（受付メイン画面へ「商談開始」のリフレッシュ命令）
        Push(ProcessSalesStart)

        Logger.Info("UpdateSalesVisitBusinessLogic.PushUpdateVisitSalesStart_End")


    End Sub

#Region "来店実績更新_商談または納車作業開始用 Privateメソッド"

    ''' <summary>
    ''' 来店実績更新_商談または納車作業開始の引数チェック
    ''' </summary>
    ''' <param name="customerSegment">顧客区分</param>
    ''' <param name="customerId">顧客コード</param>
    ''' <param name="staffCode">顧客担当スタッフコード</param>
    ''' <param name="salesStart">商談開始日時</param>
    ''' <param name="updateId">更新機能ID</param>
    ''' <param name="messageId">メッセージID</param>
    ''' <returns>検証結果（True:正常 / False:異常）</returns>
    ''' <remarks></remarks>
    Private Function IsValidUpdateVisitSalesStartParameter( _
            ByVal customerSegment As String, ByVal customerId As String, _
            ByVal staffCode As String, ByVal salesStart As Date, ByVal updateId As String, _
            ByRef messageId As Integer) As Boolean

        OutputStartLog(MethodNameIsValidUpdateVisitSalesStartParameter, customerSegment, _
                customerId, staffCode, salesStart, updateId, messageId)

        ' 検証結果
        Dim isValid As Boolean = False

        ' 顧客区分、顧客コードの入力値チェックが異常の場合
        If Not IsValidCustomerInfo(customerSegment, customerId, messageId) Then
            Logger.Info("UpdateSalesVisitBusinessLogic.IsValidUpdateVisitSalesStartParameter_001")
            OutputEndLog(MethodNameIsValidUpdateVisitSalesStartParameter, isValid, messageId)

            ' 戻り値に検証結果を設定
            Return isValid
        End If

        Logger.Info("UpdateSalesVisitBusinessLogic.IsValidUpdateVisitSalesStartParameter_002")

        ' 顧客担当スタッフコードの入力値チェックが未設定の場合
        If Not IsValidStaffCode(staffCode, messageId) Then
            Logger.Info("UpdateSalesVisitBusinessLogic.IsValidUpdateVisitSalesStartParameter_003")
            OutputEndLog(MethodNameIsValidUpdateVisitSalesStartParameter, isValid, messageId)

            ' 戻り値に検証結果を設定
            Return isValid
        End If

        Logger.Info("UpdateSalesVisitBusinessLogic.IsValidUpdateVisitSalesStartParameter_004")

        ' 商談開始日時が未設定の場合
        If 0 = Date.MinValue.CompareTo(salesStart) Then
            Logger.Info("UpdateSalesVisitBusinessLogic.IsValidUpdateVisitSalesStartParameter_005")

            ' メッセージIDを設定
            messageId = Message.EmptySalesStart

            Logger.Debug(String.Format(CultureInfo.InvariantCulture, FormatIsEmptyParameterLog, _
                    "salesStart", messageId))
            OutputEndLog(MethodNameIsValidUpdateVisitSalesStartParameter, isValid, messageId)

            ' 戻り値に検証結果を設定
            Return isValid
        End If

        Logger.Info("UpdateSalesVisitBusinessLogic.IsValidUpdateVisitSalesStartParameter_006")

        ' 更新機能IDの入力値チェックが異常の場合
        If Not IsValidUpdateId(updateId, messageId) Then
            Logger.Info("UpdateSalesVisitBusinessLogic.IsValidUpdateVisitSalesStartParameter_007")
            OutputEndLog(MethodNameIsValidUpdateVisitSalesStartParameter, isValid, messageId)

            ' 戻り値に検証結果を設定
            Return isValid
        End If

        Logger.Info("UpdateSalesVisitBusinessLogic.IsValidUpdateVisitSalesStartParameter_008")

        ' 検証結果に正常を設定
        isValid = True

        OutputEndLog(MethodNameIsValidUpdateVisitSalesStartParameter, isValid, messageId)

        ' 戻り値に検証結果を設定
        Return isValid

    End Function

    ''' <summary>
    ''' 来店日時開始の取得
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <returns>来店日時開始</returns>
    ''' <remarks></remarks>
    Private Function GetVisitTimestampStart(ByVal dealerCode As String) As Date

        OutputStartLog(MethodNameGetVisitTimestampStart, dealerCode)

        ' 来店日時開始の取得
        OutputCallStartLog(MarkerNameGetVisitTimestampStart001, MethodNameDateTimeFuncNow, _
                dealerCode)
        Dim now As Date = DateTimeFunc.Now(dealerCode)
        OutputCallEndLog(MarkerNameGetVisitTimestampStart001, MethodNameDateTimeFuncNow, now)

        Dim visitTimestampStart As Date = now.Date

        OutputEndLog(MethodNameGetVisitTimestampStart, visitTimestampStart)

        ' 戻り値に来店日時開始を設定
        Return visitTimestampStart

    End Function

    ''' <summary>
    ''' 来店日時終了の取得
    ''' </summary>
    ''' <param name="visitTimestampStart">来店日時開始</param>
    ''' <returns>来店日時終了</returns>
    ''' <remarks></remarks>
    Private Function GetVisitTimestampEnd(ByVal visitTimestampStart As Date) As Date

        OutputStartLog(MethodNameGetVisitTimestampEnd, visitTimestampStart)

        ' 来店日時終了の取得
        Dim visitTimestampStartNextDay As Date = visitTimestampStart.AddDays(NextDayValue)
        Dim visitTimestampEnd As Date = visitTimestampStartNextDay.AddMilliseconds(OneMsBeforeValue)

        OutputEndLog(MethodNameGetVisitTimestampEnd, visitTimestampEnd)

        ' 戻り値に来店日時終了を設定
        Return visitTimestampEnd

    End Function

#End Region

#End Region

#Region "ログ出力"

    ''' <summary>
    ''' 開始ログを出力する
    ''' </summary>
    ''' <param name="name">メソッド名</param>
    ''' <param name="parameters">メソッドの引数</param>
    ''' <remarks></remarks>
    Private Sub OutputStartLog(ByVal name As String, ByVal ParamArray parameters As Object())

        Dim sb As New StringBuilder(name)
        sb.Append("_Start")
        AppendArray(sb, ArrayNameParameters, parameters)
        Logger.Info(sb.ToString())
        sb = Nothing

    End Sub

    ''' <summary>
    ''' 配列の内容を StringBuilder の末尾に追加する
    ''' </summary>
    ''' <param name="sb">StringBuilder</param>
    ''' <param name="arrayName">配列の名前</param>
    ''' <param name="array">配列</param>
    ''' <remarks></remarks>
    Private Sub AppendArray( _
            ByVal sb As StringBuilder, ByVal arrayName As String, ByVal array As Object())

        With sb
            Dim lastIndex As Integer = array.Length - 1

            ' すべての要素
            For i As Integer = 0 To lastIndex
                ' 最初の要素
                If 0 = i Then
                    .Append(" ")
                    .Append(arrayName)
                    .Append("[")

                    ' 最初の要素でない場合
                Else
                    .Append(", ")
                End If

                .Append(array(i))

                ' データテーブルの場合
                If TypeOf array(i) Is DataTable Then
                    .Append("[Count = ")
                    .Append(DirectCast(array(i), DataTable).Rows.Count)
                    .Append("]")
                End If

                ' 最後の要素の場合
                If i = lastIndex Then
                    .Append("]")
                End If
            Next i
        End With

    End Sub

    ''' <summary>
    ''' 終了ログを出力する
    ''' </summary>
    ''' <param name="name">メソッド名</param>
    ''' <param name="returnValues">メソッドの戻り値</param>
    ''' <remarks></remarks>
    Private Sub OutputEndLog(ByVal name As String, ByVal ParamArray returnValues As Object())

        Dim sb As New StringBuilder(name)
        sb.Append("_End")
        AppendArray(sb, ArrayNameReturnValues, returnValues)
        Logger.Info(sb.ToString())
        sb = Nothing

    End Sub

    ''' <summary>
    ''' 外部メソッド呼び出しの開始ログを出力する
    ''' </summary>
    ''' <param name="markerName">マーカーの名前</param>
    ''' <param name="methodName">外部メソッドの名前</param>
    ''' <param name="parameters">外部メソッドの引数</param>
    ''' <remarks></remarks>
    Private Sub OutputCallStartLog( _
            ByVal markerName As String, ByVal methodName As String, _
            ByVal ParamArray parameters As Object())

        Dim sb As New StringBuilder(markerName)
        sb.Append(" Call_Start ")
        sb.Append(methodName)
        AppendArray(sb, ArrayNameParameters, parameters)
        Logger.Info(sb.ToString())
        sb = Nothing

    End Sub

    ''' <summary>
    ''' 外部メソッド呼び出しの終了ログを出力する
    ''' </summary>
    ''' <param name="markerName">マーカーの名前</param>
    ''' <param name="methodName">外部メソッドの名前</param>
    ''' <param name="returnValues">外部メソッドの戻り値</param>
    ''' <remarks></remarks>
    Private Sub OutputCallEndLog( _
            ByVal markerName As String, ByVal methodName As String, _
            ByVal ParamArray returnValues As Object())

        Dim sb As New StringBuilder(markerName)
        sb.Append(" Call_End ")
        sb.Append(methodName)
        AppendArray(sb, ArrayNameReturnValues, returnValues)
        Logger.Info(sb.ToString())
        sb = Nothing

    End Sub

    ''' <summary>
    ''' 例外ログを出力する
    ''' </summary>
    ''' <param name="name">メソッド名</param>
    ''' <param name="returnValues">メソッドの戻り値</param>
    ''' <remarks></remarks>
    Private Sub OutputExLog(ByVal name As String, ByVal ParamArray returnValues As Object())

        Dim sb As New StringBuilder(name)
        sb.Append("_Ex")
        AppendArray(sb, ArrayNameReturnValues, returnValues)
        Logger.Info(sb.ToString())
        sb = Nothing

    End Sub

#End Region

#Region "引数チェック"

    ''' <summary>
    ''' 顧客区分、顧客コードの入力値チェック
    ''' </summary>
    ''' <param name="customerSegment">顧客区分</param>
    ''' <param name="customerId">顧客コード</param>
    ''' <param name="messageId">メッセージID</param>
    ''' <returns>検証結果（True:正常 / False:異常）</returns>
    ''' <remarks></remarks>
    Private Function IsValidCustomerInfo( _
            ByVal customerSegment As String, ByVal customerId As String, _
            ByRef messageId As Integer) As Boolean

        OutputStartLog(MethodNameIsValidCustomerInfo, customerSegment, customerId, messageId)

        ' 検証結果
        Dim isValid As Boolean = False

        ' 顧客区分が未設定の場合
        If String.IsNullOrEmpty(customerSegment) Then
            Logger.Info("UpdateSalesVisitBusinessLogic.IsValidCustomerInfo_001")

            ' メッセージIDを設定
            messageId = Message.EmptyCustomerSegment

            Logger.Debug(String.Format(CultureInfo.InvariantCulture, _
                    FormatIsNullOrEmptyParameterLog, "customerSegment", messageId))
            OutputEndLog(MethodNameIsValidCustomerInfo, isValid, messageId)

            ' 戻り値に検証結果を設定
            Return isValid
        End If

        Logger.Info("UpdateSalesVisitBusinessLogic.IsValidCustomerInfo_002")

        ' 顧客コードが未設定の場合
        If String.IsNullOrEmpty(customerId) Then
            Logger.Info("UpdateSalesVisitBusinessLogic.IsValidCustomerInfo_003")

            ' メッセージIDを設定
            messageId = Message.EmptyCustomerId

            Logger.Debug(String.Format(CultureInfo.InvariantCulture, _
                    FormatIsNullOrEmptyParameterLog, "customerId", messageId))
            OutputEndLog(MethodNameIsValidCustomerInfo, isValid, messageId)

            ' 戻り値に検証結果を設定
            Return isValid
        End If

        Logger.Info("UpdateSalesVisitBusinessLogic.IsValidCustomerInfo_004")

        ' 検証結果に正常を設定
        isValid = True

        OutputEndLog(MethodNameIsValidCustomerInfo, isValid, messageId)

        ' 戻り値に検証結果を設定
        Return isValid

    End Function

    ''' <summary>
    ''' 顧客担当スタッフコードの入力値チェック
    ''' </summary>
    ''' <param name="staffCode">顧客担当スタッフコード</param>
    ''' <param name="messageId">メッセージID</param>
    ''' <returns>検証結果（True:正常 / False:異常）</returns>
    ''' <remarks></remarks>
    Private Function IsValidStaffCode( _
            ByVal staffCode As String, ByRef messageId As Integer) As Boolean

        OutputStartLog(MethodNameIsValidStaffCode, staffCode, messageId)

        ' 検証結果
        Dim isValid As Boolean = False

        ' 顧客担当スタッフコードが未設定の場合
        If String.IsNullOrEmpty(staffCode) Then
            Logger.Info("UpdateSalesVisitBusinessLogic.IsValidStaffCode_001")

            ' メッセージIDを設定
            messageId = Message.EmptyStaffCode

            Logger.Debug(String.Format(CultureInfo.InvariantCulture, _
                    FormatIsNullOrEmptyParameterLog, "staffCode", messageId))
            OutputEndLog(MethodNameIsValidStaffCode, isValid, messageId)

            ' 戻り値に検証結果を設定
            Return isValid
        End If

        Logger.Info("UpdateSalesVisitBusinessLogic.IsValidStaffCode_002")

        ' 検証結果に正常を設定
        isValid = True

        OutputEndLog(MethodNameIsValidStaffCode, isValid, messageId)

        ' 戻り値に検証結果を設定
        Return isValid

    End Function

    ''' <summary>
    ''' 更新機能IDの入力値チェック
    ''' </summary>
    ''' <param name="updateId">更新機能ID</param>
    ''' <param name="messageId">メッセージID</param>
    ''' <returns>検証結果（True:正常 / False:異常）</returns>
    ''' <remarks></remarks>
    Private Function IsValidUpdateId( _
            ByVal updateId As String, ByRef messageId As Integer) _
            As Boolean

        OutputStartLog(MethodNameIsValidUpdateId, updateId, messageId)

        ' 検証結果
        Dim isValid As Boolean = False

        ' 更新機能IDが未設定の場合
        If String.IsNullOrEmpty(updateId) Then
            Logger.Info("UpdateSalesVisitBusinessLogic.IsValidUpdateId_001")

            ' メッセージIDを設定
            messageId = Message.EmptyUpdateId

            Logger.Debug(String.Format(CultureInfo.InvariantCulture, _
                    FormatIsNullOrEmptyParameterLog, "updateId", messageId))
            OutputEndLog(MethodNameIsValidUpdateId, isValid, messageId)

            ' 戻り値に検証結果を設定
            Return isValid
        End If

        Logger.Info("UpdateSalesVisitBusinessLogic.IsValidUpdateId_002")

        ' 検証結果に正常を設定
        isValid = True

        OutputEndLog(MethodNameIsValidUpdateId, isValid, messageId)

        ' 戻り値に検証結果を設定
        Return isValid

    End Function

#End Region

#Region "Push送信"

    ''' <summary>
    ''' Push送信
    ''' </summary>
    ''' <param name="js1Parameter2">js1キーの第二引数</param>
    ''' <remarks>
    ''' 本メソッドでは、以下のメソッドも利用しているため、例外情報に関しては、下記、メソッドを参照してください。
    ''' <seealso cref="Push" />
    ''' </remarks>
    Private Sub Push(ByVal js1Parameter2 As String)

        OutputStartLog(MethodNamePush, js1Parameter2)

        ' ログイン情報を取得
        OutputCallStartLog(MarkerNamePush001, MethodNameStaffContextCurrent)
        Dim staffInfo As StaffContext = StaffContext.Current()
        OutputCallEndLog(MarkerNamePush001, MethodNameStaffContextCurrent, staffInfo)

        ' $01 start step2開発
        ' 操作権限コードのリスト（受付メイン画面再描画）
        Dim tabletOperationCdList As New List(Of Decimal)
        tabletOperationCdList.Add(OperationCodeReception)
        tabletOperationCdList.Add(OperationCodeSalesStaffManager)

        ' Push対象のアカウント情報の取得
        Using usersDt As VisitUtilityDataSet.VisitUtilityUsersDataTable = GetPushUser(staffInfo.DlrCD, staffInfo.BrnCD, tabletOperationCdList)

            ' すべてのデータ行
            For Each dr As VisitUtilityDataSet.VisitUtilityUsersRow In usersDt
                ' Push送信
                TabletSendPush(dr.ACCOUNT, js1Parameter2)
            Next dr

        End Using

        '$03 start SSV廃止
        ' 操作権限コードのリスト（SSV画面再描画）
        ' Dim pcOperationCdList As New List(Of Decimal)
        ' pcOperationCdList.Add(OperationCodeSsv)

        ' Push対象のアカウント情報の取得
        ' Using usersDt As VisitUtilityDataSet.VisitUtilityUsersDataTable = GetPushUser(staffInfo.DlrCD, staffInfo.BrnCD, pcOperationCdList)

        ' すべてのデータ行
        ' For Each dr As VisitUtilityDataSet.VisitUtilityUsersRow In usersDt
        ' Push送信
        ' PcSendPush(dr.ACCOUNT, js1Parameter2)
        ' Next dr

        ' End Using
        ' $03 end SSV廃止
        ' $01 end   step2開発

        staffInfo = Nothing

        OutputEndLog(MethodNamePush)

    End Sub

    ''' <summary>
    ''' Push対象のアカウント情報の取得
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="storeCode">店舗コード</param>
    ''' <param name="operationCodeList">権限コードリスト</param>
    ''' <returns>ユーザー情報のデータテーブル</returns>
    ''' <remarks>
    ''' </remarks>
    Private Function GetPushUser(ByVal dealerCode As String, _
                                 ByVal storeCode As String, _
                                 ByVal operationCodeList As List(Of Decimal)) _
                                 As VisitUtilityDataSet.VisitUtilityUsersDataTable

        OutputStartLog(MethodNameGetPushUser, dealerCode, storeCode, operationCodeList)

        ' $01 start step2開発
        'オンラインユーザー情報を取得する
        Dim utility As New VisitUtilityBusinessLogic
        Dim sendPushUsers As VisitUtilityDataSet.VisitUtilityUsersDataTable = _
            utility.GetOnlineUsers(dealerCode, storeCode, operationCodeList)
        utility = Nothing

        OutputEndLog(MethodNameGetPushUser, sendPushUsers)

        ' 戻り値にユーザー情報のデータテーブルを設定
        Return sendPushUsers
        ' $01 end   step2開発

    End Function

    ''' <summary>
    ''' Push送信
    ''' </summary>
    ''' <param name="uid">uidキーの値</param>
    ''' <param name="js1Parameter2">js1キーの第二引数</param>
    ''' <exception cref="WebException">Push送信処理中に例外が発生した場合</exception>
    ''' <remarks></remarks>
    Private Sub TabletSendPush(ByVal uid As String, ByVal js1Parameter2 As String)

        OutputStartLog(MethodNameTabletSendPush, uid, js1Parameter2)

        ' POST送信する文字列
        Dim postMsg As New StringBuilder

        With postMsg

            ' cat
            .Append("cat=action")
            ' type
            .Append("&type=main")
            ' type
            .Append("&sub=js")
            ' uid
            .Append("&uid=")
            .Append(uid)
            ' time
            .Append("&time=0")
            ' js1
            .Append("&js1=SC3100101Update('")
            .Append(SourceCustomerInfo)
            .Append("', '")
            .Append(js1Parameter2)
            .Append("')")

        End With

        ' ログ出力
        Dim sb As New StringBuilder
        sb.Append("Send push parameter: ")
        sb.Append(postMsg)
        Logger.Debug(sb.ToString())
        sb = Nothing

        ' Push送信
        Dim v As New VisitUtility
        v.SendPush(postMsg.ToString())
        v = Nothing

        OutputEndLog(MethodNameTabletSendPush)

    End Sub

    ''' <summary>
    ''' Push送信
    ''' </summary>
    ''' <param name="uid">uidキーの値</param>
    ''' <param name="js1Parameter2">js1キーの第二引数</param>
    ''' <exception cref="WebException">Push送信処理中に例外が発生した場合</exception>
    ''' <remarks></remarks>
    Private Sub PcSendPush(ByVal uid As String, ByVal js1Parameter2 As String)

        OutputStartLog(MethodNamePcSendPush, uid, js1Parameter2)

        ' POST送信する文字列
        Dim postMsg As New StringBuilder

        With postMsg

            ' cat
            .Append("cat=action")
            ' type
            .Append("&type=main")
            ' type
            .Append("&sub=js")
            ' uid
            .Append("&uid=")
            .Append(uid)
            ' time
            .Append("&time=0")
            ' js1
            .Append("&js1=SC3210201Update('")
            .Append(SourceCustomerInfo)
            .Append("', '")
            .Append(js1Parameter2)
            .Append("')")

        End With

        ' ログ出力
        Dim sb As New StringBuilder
        sb.Append("Send push parameter: ")
        sb.Append(postMsg)
        Logger.Debug(sb.ToString())
        sb = Nothing

        ' Push送信
        Dim v As New VisitUtility
        v.SendPushPC(postMsg.ToString())
        v = Nothing

        OutputEndLog(MethodNamePcSendPush)

    End Sub

#End Region

#Region "来店実績更新_商談または納車作業終了"

    ' $02 start 複数顧客に対する商談平行対応
    ''' <summary>
    ''' 来店実績更新_商談または納車作業終了
    ''' </summary>
    ''' <param name="customerSegment">顧客区分</param>
    ''' <param name="customerId">顧客コード</param>
    ''' <param name="salesEnd">商談終了日時</param>
    ''' <param name="updateId">更新機能ID</param>
    ''' <param name="messageId">メッセージID</param>
    ''' <param name="logicStateNegotiation">処理区分</param>
    ''' <remarks>
    ''' 商談または納車作業終了時に必要な来店実績データの更新を行う。
    ''' 本メソッドでは、以下のメソッドも利用しているため、例外情報に関しては、下記、メソッドを参照してください。
    ''' <seealso cref="UpdateSalesVisitTableAdapter.GetVisitSalesFollowUp" />
    ''' <seealso cref="UpdateSalesVisitTableAdapter.UpdateVisitSalesEnd" />
    ''' <seealso cref="Push" />
    ''' </remarks>
    Public Sub UpdateVisitSalesEnd( _
            ByVal customerSegment As String, ByVal customerId As String, ByVal salesEnd As Date, _
            ByVal updateId As String, ByRef messageId As Integer, ByVal logicStateNegotiation As String)
        ' $02 end   複数顧客に対する商談平行対応

        OutputStartLog(MethodNameUpdateVisitSalesEnd, customerSegment, customerId, _
                salesEnd, updateId, messageId, logicStateNegotiation)

        ' 引数チェック
        Dim isValidParameter As Boolean = IsValidUpdateUpdateVisitSalesEndParameter( _
                customerSegment, customerId, salesEnd, updateId, messageId)

        ' 引数チェックが異常の場合は、処理を中断
        If Not isValidParameter Then
            Logger.Info("UpdateSalesVisitBusinessLogic.UpdateVisitSalesEnd_001")
            OutputEndLog(MethodNameUpdateVisitSalesEnd, messageId)
            Return
        End If

        Logger.Info("UpdateSalesVisitBusinessLogic.UpdateVisitSalesEnd_002")

        ' ログイン情報を取得
        OutputCallStartLog(MarkerNameUpdateVisitSalesEnd003, MethodNameStaffContextCurrent)
        Dim staffInfo As StaffContext = StaffContext.Current()
        OutputCallEndLog(MarkerNameUpdateVisitSalesEnd003, MethodNameStaffContextCurrent, _
                staffInfo)

        Try
            Dim isSuccess As Boolean = False

            Using ta As New UpdateSalesVisitTableAdapter
                ' 顧客指定の来店実績情報の取得
                Using dt As UpdateSalesVisitDataTable = ta.GetVisitSalesFollowUp( _
                        staffInfo.DlrCD, staffInfo.BrnCD, customerSegment, customerId)
                    ' 来店実績情報が取得できない場合は、処理を中断
                    If 0 = dt.Count Then
                        Logger.Info("UpdateSalesVisitBusinessLogic.UpdateVisitSalesEnd_004")

                        ' メッセージIDを設定
                        messageId = Message.NotExistsCustomerInfo

                        Logger.Debug(String.Format(CultureInfo.InvariantCulture, _
                                FormatNotExistsCustomerInfoLog, messageId))
                        OutputEndLog(MethodNameUpdateVisitSalesEnd, messageId)
                        Return
                    End If

                    Logger.Info("UpdateSalesVisitBusinessLogic.UpdateVisitSalesEnd_005")

                    ' データ行を取得
                    Dim dr As UpdateSalesVisitRow = dt.Item(0)

                    ' 対応担当アカウントとログインアカウントが一致しない場合は、処理を中断
                    If dr.IsACCOUNTNull _
                            OrElse Not dr.ACCOUNT.Equals(staffInfo.Account) Then
                        Logger.Info("UpdateSalesVisitBusinessLogic.UpdateVisitSalesEnd_006")
                        ' $03 start 納車作業終了対応
                        If logicStateNegotiation = LogicStateNegotiationFinish Then
                            ' メッセージIDを設定(商談終了不可)
                            messageId = Message.CannotSalesEnd
                        Else
                            ' メッセージIDを設定(納車作業終了不可)
                            messageId = Message.CannotDeliverlyEnd
                        End If
                        ' $03 end   納車作業終了対応
                        Logger.Debug(String.Format(CultureInfo.InvariantCulture, _
                                FormatCannotSalesEndLog, messageId))
                        OutputEndLog(MethodNameUpdateVisitSalesEnd, messageId)
                        Return

                        Logger.Info("UpdateSalesVisitBusinessLogic.UpdateVisitSalesEnd_006")

                    End If

                    Logger.Info("UpdateSalesVisitBusinessLogic.UpdateVisitSalesEnd_007")

                    ' $03 start 納車作業終了対応
                    If logicStateNegotiation = LogicStateNegotiationFinish Or _
                        logicStateNegotiation = LogicStateNegotiationStop Then
                        ' 商談終了時の来店実績の更新
                        isSuccess = ta.UpdateVisitSalesEnd(dr.VISITSEQ, salesEnd, staffInfo.Account, _
                                updateId, LogicStateNegotiationFinish)
                    ElseIf logicStateNegotiation = LogicStateDeliverlyFinish Then
                        ' 納車作業終了時の来店実績の更新
                        isSuccess = ta.UpdateVisitSalesEnd(dr.VISITSEQ, salesEnd, staffInfo.Account, _
                                 updateId, LogicStateDeliverlyFinish)
                    End If
                    ' $03 end   納車作業終了対応

                    ' $02 start 複数顧客に対する商談平行対応

                    ' 処理結果が失敗の場合は、処理を中断
                    If Not isSuccess Then
                        Logger.Info("UpdateSalesVisitBusinessLogic.UpdateVisitSalesEnd_008")

                        ' メッセージIDを設定
                        messageId = Message.UpdateError

                        Logger.Debug(String.Format(CultureInfo.InvariantCulture, _
                                FormatConcurrencyViolationLog, messageId))
                        OutputEndLog(MethodNameUpdateVisitSalesEnd, messageId)
                        Return
                    End If

                    Logger.Info("UpdateSalesVisitBusinessLogic.UpdateVisitSalesEnd_009")

                    ' 処理区分が商談中断の場合
                    If logicStateNegotiation = LogicStateNegotiationStop Then
                        Logger.Info("UpdateSalesVisitBusinessLogic.UpdateVisitSalesEnd_010")

                        ' 商談中断時の来店実績の作成
                        isSuccess = ta.CopyVisitSalesStop(dr.VISITSEQ, salesEnd, staffInfo.Account, _
                                updateId)
                    End If

                    ' $02 end   複数顧客に対する商談平行対応

                End Using
            End Using

            staffInfo = Nothing

            ' 処理結果が失敗の場合は、処理を中断
            If Not isSuccess Then
                Logger.Info("UpdateSalesVisitBusinessLogic.UpdateVisitSalesEnd_011")

                ' メッセージIDを設定
                messageId = Message.UpdateError

                Logger.Debug(String.Format(CultureInfo.InvariantCulture, _
                        FormatConcurrencyViolationLog, messageId))
                OutputEndLog(MethodNameUpdateVisitSalesEnd, messageId)
                Return
            End If

            Logger.Info("UpdateSalesVisitBusinessLogic.UpdateVisitSalesEnd_012")

            ' データベースの操作中に例外が発生した場合
        Catch ex As OracleExceptionEx
            Logger.Info("UpdateSalesVisitBusinessLogic.UpdateVisitSalesEnd_013")

            ' メッセージIDを設定
            messageId = Message.UpdateError

            Logger.Error(String.Format(CultureInfo.InvariantCulture, FormatOracleExceptionExLog, _
                    messageId), ex)
            OutputExLog(MethodNameUpdateVisitSalesEnd, ex, messageId)
            Throw
        End Try

        Logger.Info("UpdateSalesVisitBusinessLogic.UpdateVisitSalesEnd_014")

        ' メッセージIDを設定
        messageId = Message.None

        OutputEndLog(MethodNameUpdateVisitSalesEnd, messageId)

    End Sub

    ' $02 start 複数顧客に対する商談平行対応
    ''' <summary>
    ''' 来店実績更新_商談終了時のPush送信
    ''' </summary>
    ''' <param name="logicStateNegotiation">処理区分</param>
    ''' <remarks>
    ''' 商談終了の処理が全て終了した後に呼び出され、Push送信を行う
    ''' </remarks>
    Public Sub PushUpdateVisitSalesEnd(ByVal logicStateNegotiation As String)

        Logger.Info("UpdateSalesVisitBusinessLogic.PushUpdateVisitSalesEnd_Start")

        If logicStateNegotiation = LogicStateNegotiationStop Then

            ' 処理区分が商談中断の場合
            Logger.Info("UpdateSalesVisitBusinessLogic.PushUpdateVisitSalesEnd_001")
            Push(ProcessSalesStop)
        ElseIf logicStateNegotiation = LogicStateNegotiationStart Then
            ' 処理区分が商談終了の場合
            Logger.Info("UpdateSalesVisitBusinessLogic.PushUpdateVisitSalesEnd_002")
            Push(ProcessSalesEnd)
        End If
        ' $02 end   複数顧客に対する商談平行対応

        Logger.Info("UpdateSalesVisitBusinessLogic.PushUpdateVisitSalesEnd_End")

    End Sub

#Region "来店実績更新_商談または納車作業終了用 Privateメソッド"

    ''' <summary>
    ''' 来店実績更新_商談または納車作業終了の引数チェック
    ''' </summary>
    ''' <param name="customerSegment">顧客区分</param>
    ''' <param name="customerId">顧客コード</param>
    ''' <param name="salesEnd">商談終了日時</param>
    ''' <param name="updateId">更新機能ID</param>
    ''' <param name="messageId">メッセージID</param>
    ''' <returns>検証結果（True:正常 / False:異常）</returns>
    ''' <remarks></remarks>
    Private Function IsValidUpdateUpdateVisitSalesEndParameter( _
            ByVal customerSegment As String, ByVal customerId As String, _
            ByVal salesEnd As Date, ByVal updateId As String, _
            ByRef messageId As Integer) As Boolean

        OutputStartLog(MethodNameIsValidUpdateUpdateVisitSalesEndParameter, _
                customerSegment, customerId, salesEnd, updateId, messageId)

        ' 検証結果
        Dim isValid As Boolean = False

        ' 顧客区分、顧客コードの入力値チェックが異常の場合
        If Not IsValidCustomerInfo(customerSegment, customerId, messageId) Then
            Logger.Info("UpdateSalesVisitBusinessLogic.IsValidUpdateUpdateVisitSalesEndParameter_001")
            OutputEndLog(MethodNameIsValidUpdateUpdateVisitSalesEndParameter, isValid, messageId)

            ' 戻り値に検証結果を設定
            Return isValid
        End If

        Logger.Info("UpdateSalesVisitBusinessLogic.IsValidUpdateUpdateVisitSalesEndParameter_002")

        ' 商談終了日時が未設定
        If 0 = Date.MinValue.CompareTo(salesEnd) Then
            Logger.Info("UpdateSalesVisitBusinessLogic.IsValidUpdateUpdateVisitSalesEndParameter_003")

            ' メッセージIDを設定
            messageId = Message.EmptySalesEnd

            Logger.Debug(String.Format(CultureInfo.InvariantCulture, FormatIsEmptyParameterLog, _
                    "salesEnd", messageId))
            OutputEndLog(MethodNameIsValidUpdateUpdateVisitSalesEndParameter, isValid, messageId)

            ' 戻り値に検証結果を設定
            Return isValid
        End If

        Logger.Info("UpdateSalesVisitBusinessLogic.IsValidUpdateUpdateVisitSalesEndParameter_004")

        ' 更新機能IDの入力値チェックが異常の場合
        If Not IsValidUpdateId(updateId, messageId) Then
            Logger.Info("UpdateSalesVisitBusinessLogic.IsValidUpdateUpdateVisitSalesEndParameter_005")
            OutputEndLog(MethodNameIsValidUpdateUpdateVisitSalesEndParameter, isValid, messageId)

            ' 戻り値に検証結果を設定
            Return isValid
        End If

        Logger.Info("UpdateSalesVisitBusinessLogic.IsValidUpdateUpdateVisitSalesEndParameter_006")

        ' 検証結果に正常を設定
        isValid = True

        OutputEndLog(MethodNameIsValidUpdateUpdateVisitSalesEndParameter, isValid, messageId)

        ' 戻り値に検証結果を設定
        Return isValid

    End Function

#End Region

#End Region

#Region "来店実績更新_顧客登録"

    ''' <summary>
    ''' 来店実績更新_顧客登録
    ''' </summary>
    ''' <param name="visitSeq">来店実績連番</param>
    ''' <param name="customerSegment">顧客区分</param>
    ''' <param name="customerId">顧客コード</param>
    ''' <param name="staffCode">顧客担当アカウント</param>
    ''' <param name="updateId">更新機能ID</param>
    ''' <param name="messageId">メッセージID</param>
    ''' <remarks>
    ''' 顧客登録時に必要な来店実績データの更新を行う。
    ''' 本メソッドでは、以下のメソッドも利用しているため、例外情報に関しては、下記、メソッドを参照してください。
    ''' <seealso cref="UpdateSalesVisitTableAdapter.UpdateVisitSalesCustomer" />
    ''' </remarks>
    Public Sub UpdateVisitCustomerInfo( _
            ByVal visitSeq As Long, ByVal customerSegment As String, ByVal customerId As String, _
            ByVal staffCode As String, ByVal updateId As String, ByRef messageId As Integer)

        OutputStartLog(MethodNameUpdateVisitCustomerInfo, visitSeq, customerSegment, customerId, _
                staffCode, updateId, messageId)

        ' 引数チェック
        Dim isValidParameter As Boolean = IsValidUpdateVisitCustomerInfoParameter(visitSeq, _
                customerSegment, customerId, staffCode, updateId, messageId)

        ' 引数チェックが異常の場合は、処理を中断
        If Not isValidParameter Then
            Logger.Info("UpdateSalesVisitBusinessLogic.UpdateVisitCustomerInfo_001")
            OutputEndLog(MethodNameUpdateVisitCustomerInfo, messageId)
            Return
        End If

        Logger.Info("UpdateSalesVisitBusinessLogic.UpdateVisitCustomerInfo_002")

        ' ログイン情報を取得
        OutputCallStartLog(MarkerNameGetVisitUpdateVisitCustomerInfo003, _
                MethodNameStaffContextCurrent)
        Dim staffInfo As StaffContext = StaffContext.Current()
        OutputCallEndLog(MarkerNameGetVisitUpdateVisitCustomerInfo003, _
                MethodNameStaffContextCurrent, staffInfo)

        Try
            Dim isSuccess As Boolean = False

            Using ta As New UpdateSalesVisitTableAdapter
                ' 顧客登録時の来店実績の更新
                isSuccess = ta.UpdateVisitSalesCustomer(visitSeq, customerSegment, _
                        customerId, staffCode, staffInfo.Account, updateId)
            End Using

            staffInfo = Nothing

            ' 処理結果が失敗の場合は、処理を中断
            If Not isSuccess Then
                Logger.Info("UpdateSalesVisitBusinessLogic.UpdateVisitCustomerInfo_003")

                ' メッセージIDを設定
                messageId = Message.AlreadyUpdatedCustomerInfo

                Logger.Debug(String.Format(CultureInfo.InvariantCulture, _
                        FormatConcurrencyViolationLog, messageId))
                OutputEndLog(MethodNameUpdateVisitCustomerInfo, messageId)
                Return
            End If

            Logger.Info("UpdateSalesVisitBusinessLogic.UpdateVisitCustomerInfo_004")

            '    ' データベースの操作中に例外が発生した場合
        Catch ex As OracleExceptionEx
            Logger.Info("UpdateSalesVisitBusinessLogic.UpdateVisitCustomerInfo_005")

            ' メッセージIDを設定
            messageId = Message.UpdateError

            Logger.Error(String.Format(CultureInfo.InvariantCulture, FormatOracleExceptionExLog, _
                    messageId), ex)
            OutputExLog(MethodNameUpdateVisitCustomerInfo, ex, messageId)
            Throw
        End Try

        Logger.Info("UpdateSalesVisitBusinessLogic.UpdateVisitCustomerInfo_006")

        ' メッセージIDを設定
        messageId = Message.None

        OutputEndLog(MethodNameUpdateVisitCustomerInfo, messageId)

    End Sub

#Region "来店実績更新_顧客登録用 Privateメソッド"

    ''' <summary>
    ''' 来店実績更新_顧客登録の引数チェック
    ''' </summary>
    ''' <param name="visitSeq">来店実績連番</param>
    ''' <param name="customerSegment">顧客区分</param>
    ''' <param name="customerId">顧客コード</param>
    ''' <param name="staffCode">顧客担当アカウント</param>
    ''' <param name="updateId">更新機能ID</param>
    ''' <param name="messageId">メッセージID</param>
    ''' <returns>検証結果（True:正常 / False:異常）</returns>
    ''' <remarks></remarks>
    Private Function IsValidUpdateVisitCustomerInfoParameter( _
            ByVal visitSeq As Long, ByVal customerSegment As String, ByVal customerId As String, _
            ByVal staffCode As String, ByVal updateId As String, ByRef messageId As Integer) _
            As Boolean

        OutputStartLog(MethodNameIsValidUpdateVisitCustomerInfoParameter, visitSeq, _
                customerSegment, customerId, staffCode, updateId, messageId)

        ' 検証結果
        Dim isValid As Boolean = False

        ' 来店実績連番が未設定の場合
        If 0L = visitSeq Then
            Logger.Info("UpdateSalesVisitBusinessLogic.IsValidUpdateVisitCustomerInfoParameter_001")

            ' メッセージIDを設定
            messageId = Message.EmptyVisitSeq

            Logger.Debug(String.Format(CultureInfo.InvariantCulture, FormatIsEmptyParameterLog, _
                    "visitSeq", messageId))
            OutputEndLog(MethodNameIsValidUpdateVisitCustomerInfoParameter, isValid, messageId)

            ' 戻り値に検証結果を設定
            Return isValid
        End If

        Logger.Info("UpdateSalesVisitBusinessLogic.IsValidUpdateVisitCustomerInfoParameter_002")

        ' 顧客区分、顧客コードの入力値チェックが異常の場合
        If Not IsValidCustomerInfo(customerSegment, customerId, messageId) Then
            Logger.Info("UpdateSalesVisitBusinessLogic.IsValidUpdateVisitCustomerInfoParameter_003")
            OutputEndLog(MethodNameIsValidUpdateVisitCustomerInfoParameter, isValid, messageId)

            ' 戻り値に検証結果を設定
            Return isValid
        End If

        Logger.Info("UpdateSalesVisitBusinessLogic.IsValidUpdateVisitCustomerInfoParameter_004")

        ' 顧客担当スタッフコードが未設定の場合
        If Not IsValidStaffCode(staffCode, messageId) Then
            Logger.Info("UpdateSalesVisitBusinessLogic.IsValidUpdateVisitCustomerInfoParameter_005")
            OutputEndLog(MethodNameIsValidUpdateVisitCustomerInfoParameter, isValid, messageId)

            ' 戻り値に検証結果を設定
            Return isValid
        End If

        Logger.Info("UpdateSalesVisitBusinessLogic.IsValidUpdateVisitCustomerInfoParameter_006")

        ' 更新機能IDの入力値チェックが異常の場合
        If Not IsValidUpdateId(updateId, messageId) Then
            Logger.Info("UpdateSalesVisitBusinessLogic.IsValidUpdateVisitCustomerInfoParameter_007")
            OutputEndLog(MethodNameIsValidUpdateVisitCustomerInfoParameter, isValid, messageId)

            ' 戻り値に検証結果を設定
            Return isValid
        End If

        Logger.Info("UpdateSalesVisitBusinessLogic.IsValidUpdateVisitCustomerInfoParameter_008")

        ' 検証結果に正常を設定
        isValid = True

        OutputEndLog(MethodNameIsValidUpdateVisitCustomerInfoParameter, isValid, messageId)

        ' 戻り値に検証結果を設定
        Return isValid

    End Function

#End Region

#End Region

#Region "来店実績更新_ログイン"

    ''' <summary>
    ''' 来店実績更新_ログイン
    ''' </summary>
    ''' <param name="updateId">更新機能ID</param>
    ''' <param name="messageId">メッセージID</param>
    ''' <return>更新件数</return>
    ''' <remarks>
    ''' ログイン時に必要な来店実績データの更新を行う。
    ''' 本メソッドでは、以下のメソッドも利用しているため、例外情報に関しては、下記、メソッドを参照してください。
    ''' <seealso cref="UpdateSalesVisitTableAdapter.UpdateVisitLogin" />
    ''' <seealso cref="Push" />
    ''' </remarks>
    Public Function UpdateVisitLogin( _
            ByVal updateId As String, ByRef messageId As Integer) As Integer

        OutputStartLog(MethodNameUpdateVisitLogin, updateId, messageId)

        ' 更新対象レコード件数
        Dim record As Integer = 0

        ' 更新機能IDの入力値チェックが異常の場合
        If Not IsValidUpdateId(updateId, messageId) Then
            Logger.Info("UpdateSalesVisitBusinessLogic.UpdateVisitLogin_001")
            OutputEndLog(MethodNameUpdateVisitLogin, record, messageId)

            ' 戻り値に更新対象レコード件数を設定
            Return record
        End If

        Logger.Info("UpdateSalesVisitBusinessLogic.UpdateVisitLogin_002")

        ' ログイン情報を取得
        OutputCallStartLog(MarkerNameUpdateVisitLogin003, MethodNameStaffContextCurrent)
        Dim staffInfo As StaffContext = StaffContext.Current()
        OutputCallEndLog(MarkerNameUpdateVisitLogin003, MethodNameStaffContextCurrent, staffInfo)

        Try
            Using ta As New UpdateSalesVisitTableAdapter
                ' ログイン時の来店実績情報の更新
                record = ta.UpdateVisitLogin(staffInfo.Account, staffInfo.DlrCD, staffInfo.BrnCD, _
                        staffInfo.Account, updateId)
            End Using

            staffInfo = Nothing

            ' データベースの操作中に例外が発生した場合
        Catch ex As OracleExceptionEx
            Logger.Info("UpdateSalesVisitBusinessLogic.UpdateVisitLogin_004")

            ' メッセージIDを設定
            messageId = Message.UpdateError

            Logger.Error(String.Format(CultureInfo.InvariantCulture, FormatOracleExceptionExLog, _
                    messageId), ex)
            OutputExLog(MethodNameUpdateVisitLogin, ex, messageId)
            Throw
        End Try

        Logger.Info("UpdateSalesVisitBusinessLogic.UpdateVisitLogin_005")

        ' メッセージIDを設定
        messageId = Message.None

        OutputEndLog(MethodNameUpdateVisitLogin, record, messageId)

        ' 戻り値に更新対象レコード件数を設定
        Return record

    End Function

    ''' <summary>
    ''' 来店実績更新_ログイン時のPush送信
    ''' </summary>
    ''' <remarks>
    ''' ログインの処理が全て終了した後に呼び出され、Push送信を行う
    ''' </remarks>
    Public Sub PushUpdateVisitLogin()


            Logger.Info("UpdateSalesVisitBusinessLogic.PushUpdateVisitLogin_Start")

            ' Push送信（受付メイン画面へ「商談終了」のリフレッシュ命令）
            Push(ProcessSalesEnd)

            Logger.Info("UpdateSalesVisitBusinessLogic.PushUpdateVisitLogin_End")
    End Sub

#End Region

#Region "商談または納車作業開始前の来店実績連番の取得"

    ''' <summary>
    ''' 商談または納車作業開始前の来店実績連番の取得
    ''' </summary>
    ''' <param name="customerSegment">顧客区分</param>
    ''' <param name="customerId">顧客コード</param>
    ''' <returns>来店実績連番</returns>
    ''' <remarks>
    ''' 顧客情報に紐づく商談または納車作業開始前の来店実績連番を取得する
    ''' <exception cref="ArgumentException">顧客区分または顧客コードが設定されていない場合</exception>
    ''' 本メソッドでは、以下のメソッドも利用しているため、例外情報に関しては、下記、メソッドを参照してください。
    ''' <seealso cref="UpdateSalesVisitTableAdapter.GetVisitSalesStart" />
    ''' </remarks>
    Public Function GetVisitSeqBeforeSalesStart( _
            ByVal customerSegment As String, ByVal customerId As String) As Long

        OutputStartLog(MethodNameGetVisitSeqBeforeSalesStart, customerSegment, customerId)

        ' 更新対象レコード件数
        Dim visitSeq As Long = 0L

        ' 顧客区分が未設定の場合
        If String.IsNullOrEmpty(customerSegment) Then
            Logger.Info("UpdateSalesVisitBusinessLogic.GetVisitSeqBeforeSalesStart_001")
            Throw New ArgumentException("Invalid parameter: customerSegment is null or empty.")
        End If

        Logger.Info("UpdateSalesVisitBusinessLogic.GetVisitSeqBeforeSalesStart_002")

        ' 顧客コードが未設定の場合
        If String.IsNullOrEmpty(customerId) Then
            Logger.Info("UpdateSalesVisitBusinessLogic.GetVisitSeqBeforeSalesStart_003")
            Throw New ArgumentException("Invalid parameter: customerId is null or empty.")
        End If

        Logger.Info("UpdateSalesVisitBusinessLogic.GetVisitSeqBeforeSalesStart_004")

        ' ログイン情報を取得
        OutputCallStartLog(MarkerNameGetVisitSeqBeforeSalesStart003, MethodNameStaffContextCurrent)
        Dim staffInfo As StaffContext = StaffContext.Current()
        OutputCallEndLog(MarkerNameGetVisitSeqBeforeSalesStart003, MethodNameStaffContextCurrent, _
                staffInfo)

        ' 日付管理（現在日付の取得）
        Dim visitTimestampStart As Date = GetVisitTimestampStart(staffInfo.DlrCD)
        Dim visitTimestampEnd As Date = GetVisitTimestampEnd(visitTimestampStart)

        Try
            Using ta As New UpdateSalesVisitTableAdapter
                ' 商談開始時の来店実績の取得
                Using dt As UpdateSalesVisitDataTable = ta.GetVisitSalesStart(staffInfo.DlrCD, _
                        staffInfo.BrnCD, customerSegment, customerId, visitTimestampStart, _
                        visitTimestampEnd)

                    ' 来店実績情報が取得できた場合
                    If 0 < dt.Count Then
                        Logger.Info("UpdateSalesVisitBusinessLogic.GetVisitSeqBeforeSalesStart_005")

                        Dim dr As UpdateSalesVisitRow = dt.Item(0)

                        ' $03 start 納車作業ステータス対応
                        ' 来店実績ステータスが「商談中」の場合
                        If dr.VISITSTATUS.Equals(VisitStatusSalesStart.PadRight( _
                                dr.VISITSTATUS.Length)) Then
                            Logger.Info("UpdateSalesVisitBusinessLogic.GetVisitSeqBeforeSalesStart_006a")
                            Logger.Debug("Business discussion has already started.")
                            OutputEndLog(MethodNameGetVisitSeqBeforeSalesStart, visitSeq)

                            ' 戻り値に来店実績連番を設定
                            Return visitSeq
                            ' 来店実績ステータスが「納車作業中」の場合
                        ElseIf dr.VISITSTATUS.Equals(VisitStatusDeliverlyStart.PadRight( _
                                dr.VISITSTATUS.Length)) Then
                            Logger.Info("UpdateSalesVisitBusinessLogic.GetVisitSeqBeforeSalesStart_006b")
                            Logger.Debug("Deliverly has already started.")
                            OutputEndLog(MethodNameGetVisitSeqBeforeSalesStart, visitSeq)

                            ' 戻り値に来店実績連番を設定
                            Return visitSeq
                        End If
                        ' $03 end   納車作業ステータス対応

                        Logger.Info("UpdateSalesVisitBusinessLogic.GetVisitSeqBeforeSalesStart_007")
                        visitSeq = dr.VISITSEQ
                    End If
                End Using
            End Using

            staffInfo = Nothing

            Logger.Info("UpdateSalesVisitBusinessLogic.GetVisitSeqBeforeSalesStart_008")

            ' データベースの操作中に例外が発生した場合
        Catch ex As OracleExceptionEx
            Logger.Info("UpdateSalesVisitBusinessLogic.GetVisitSeqBeforeSalesStart_009")
            Logger.Error("An exception occurred during the operation of the database.", ex)
            OutputExLog(MethodNameGetVisitSeqBeforeSalesStart, ex)
            Throw
        End Try

        Logger.Info("UpdateSalesVisitBusinessLogic.GetVisitSeqBeforeSalesStart_010")

        OutputEndLog(MethodNameGetVisitSeqBeforeSalesStart, visitSeq)

        ' 戻り値に来店実績連番を設定
        Return visitSeq

    End Function

#End Region

#End Region

End Class
