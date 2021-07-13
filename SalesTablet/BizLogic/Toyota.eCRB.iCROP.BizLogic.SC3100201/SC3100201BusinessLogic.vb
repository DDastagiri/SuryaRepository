'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3100201BusinessLogic.vb
'──────────────────────────────────
'機能： 未対応来店客
'補足： 
'作成： yyyy/MM/dd KN  x.xxxxxx
'更新： 2012/02/14 KN  y.nakamura STEP2開発 $01
'更新： 2013/03/04 TMEJ t.shimamura 新車タブレット受付画面管理指標変更対応 $02
'更新： 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証
'──────────────────────────────────

Imports Oracle.DataAccess.Client
Imports System.IO
Imports System.Net
Imports System.Text
Imports Toyota.eCRB.Visit.Api.BizLogic
Imports Toyota.eCRB.Visit.NotDealCustomer.DataAccess
Imports Toyota.eCRB.Visit.NotDealCustomer.DataAccess.SC3100201DataSetTableAdapters
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
' $01 start step2開発
Imports Toyota.eCRB.Visit.Api.DataAccess.VisitUtilityDataSet
' $01 end   step2開発

''' <summary>
''' 未対応来店客のビジネスロジッククラスです。
''' </summary>
''' <remarks></remarks>
Public Class SC3100201BusinessLogic
    Inherits BaseBusinessComponent
    Implements ISC3100201BusinessLogic

#Region "定数"

    ''' <summary>
    ''' メッセージID - 正常終了
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MessageSuccess As Integer = 0

    ''' <summary>
    ''' 文言ID - メッセージ：SCの返答に対するデータ更新処理時のDBタイムアウト
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MessageDBTimeout As Integer = 902

    ''' <summary>
    ''' 文言ID - メッセージ：SCの返答に対するデータ更新処理時の排他エラー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MessageConcurrencyViolation As Integer = 903

    ''' <summary>
    ''' システム管理マスタ.パラメータ名:敬称表示位置
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SystemEnvKeisyoZengo As String = "KEISYO_ZENGO"

    ''' <summary>
    ''' 敬称表示位置:前
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SystemEnvKeisyoZengoMae As String = "1"

    ''' <summary>
    ''' 敬称表示位置:後
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SystemEnvKeisyoZengoUshiro As String = "2"

    ''' <summary>
    ''' 顧客区分 - 自社客
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SegmentCustomer As String = "1"

    ''' <summary>
    ''' 顧客区分 - 未取引客
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SegmentNewCustomer As String = "2"

    ''' <summary>
    ''' 更新機能ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const UpdateId As String = "SC3100201"

    ''' <summary>
    ''' 来店実績ステータス - フリー
    ''' </summary>
    Private Const VisitStatusFree As String = "01"

    ''' <summary>
    ''' 来店実績ステータス - 確定(ブロードキャスト)
    ''' </summary>
    Private Const VisitStatusDefinitionBroud As String = "04"

    ''' <summary>
    ''' 来店実績ステータス - 確定
    ''' </summary>
    Private Const VisitStatusDefinition As String = "05"

    ''' <summary>
    ''' 来店実績ステータス - 待ち
    ''' </summary>
    Private Const VisitStatusWait As String = "06"

    ''' <summary>
    ''' 送信元（未対応来店客画面）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SourceSC3100201 As String = "02"

    ''' <summary>
    ''' 送信時処理（「了」ボタンタップ（お客様が来店状態））
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ProcessNotWaitToDecision As String = "01"

    ''' <summary>
    ''' 送信時処理（「了」ボタンタップ（お客様が待ち状態））
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ProcessWaitToDecision As String = "02"

    ''' <summary>
    ''' 送信時処理（「待」ボタンタップ）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ProcessNotWaitToWait As String = "03"

    ''' <summary>
    ''' 送信時処理（「不」ボタンタップ（お客様が来店状態））
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ProcessNotWaitToFree As String = "04"

    ''' <summary>
    ''' 送信時処理（「不」ボタンタップ（お客様が待ち状態））
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ProcessWaitToFree As String = "05"

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

    ''' <summary>
    ''' 操作権限コード（セールスマネージャ）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OperationCodeSalesStaffManager As Decimal = 7D

    ' $01 start step2開発
    ' $02 start SSV廃止
    '''' <summary>
    '''' 操作権限コード（SSV）
    '''' </summary>
    '''' <remarks></remarks>
    'Private Const OperationCodeSsv As Decimal = 53D
    ' $02 end   SSV廃止
    ' $01 end   step2開発

    ''' <summary>
    ''' オラクルエラーコード:タイムアウト
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ErrorCodeOra2049 As Integer = 2049
#End Region

#Region "トレースログ"

    ''' <summary>
    ''' SendGatePush
    ''' </summary>
    ''' <remarks>ログ出力用(push機能呼び出し)</remarks>
    Private Const CallSendPush As String = "SendPush CALL "

    ''' <summary>
    ''' 開始ログ    
    ''' </summary>
    ''' <remarks>ログ出力用(開始)</remarks>
    Private Const StartLog As String = "START "

    ''' <summary>
    ''' POSTパラメーターログ    
    ''' </summary>
    ''' <remarks>ログ出力用(POSTパラメーター)</remarks>
    Private Const ParameterLog As String = "Send Parameter "

    ''' <summary>
    ''' 終了ログ
    ''' </summary>
    ''' <remarks>ログ出力用(終了)</remarks>
    Private Const EndLog As String = "END "
#End Region

#Region "Public関数"

    ''' <summary>
    ''' 未対応来店客一覧を取得する。
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="storeCode">店舗コード</param>
    ''' <param name="account">ログインアカウント</param>
    ''' <param name="nowDate">現在日時</param>
    ''' <param name="complaintDateCount">苦情表示日数</param>
    ''' <returns>未対応来店客一覧</returns>
    ''' <remarks></remarks>
    Public Function GetNotDealVisitCustomer(ByVal dealerCode As String, _
                                            ByVal storeCode As String, _
                                            ByVal account As String, _
                                            ByVal nowDate As Date, _
                                            ByVal complaintDateCount As Integer) _
                                        As SC3100201DataSet.NotDealVisitDataTable
        ' $01 start step2開発
        Logger.Info("GetNotDealVisitCustomer_Start Param[" & dealerCode & _
                     ", " & storeCode & _
                     ", " & account & _
                     ", " & nowDate & _
                     ", " & complaintDateCount & _
                     "]")
        ' $01 end   step2開発

        ' 取得開始日時と取得終了日時の作成
        Dim dateSt As New Date(nowDate.Year, nowDate.Month, nowDate.Day, 0, 0, 0)
        Dim dateEd As New Date(nowDate.Year, nowDate.Month, nowDate.Day, 23, 59, 59)

        ' 未対応来店客一覧
        Dim visitDt As SC3100201DataSet.NotDealVisitDataTable = Nothing

        Using adapter As New SC3100201TableAdapter

            ' ブロードキャスト対応来店実績一覧の取得
            Dim visitBroudcastDt As SC3100201DataSet.NotDealVisitDataTable = _
                adapter.GetVisitBroadcast(dealerCode, storeCode, account, dateSt, dateEd)

            ' セールススタッフ指定対応来店実績一覧の取得
            Dim visitStaffSpecifyDt As SC3100201DataSet.NotDealVisitDataTable = _
                adapter.GetVisitStaffSpecify(dealerCode, storeCode, account, dateSt, dateEd)

            ' 取得した来店実績一覧のマージ
            visitDt = visitStaffSpecifyDt
            visitDt.Merge(visitBroudcastDt)

            ' 件数チェック
            If 0 >= visitDt.Count Then

                Logger.Info("GetNotDealVisitCustomer_End Ret[Nothing]")
                Return Nothing
            End If

            ' $01 start step2開発
            ' 顧客情報の取得
            Me.SettingDetail(adapter, visitDt, nowDate, complaintDateCount)
            ' $01 end   step2開発

            ' 来店実績一覧のソート(ソート済みのため、不要)

        End Using

        Logger.Info("GetNotDealVisitCustomer_End Ret[" & visitDt.Count & "]")
        Return visitDt
    End Function

    ''' <summary>
    ''' 参考情報一覧を取得する。
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="storeCode">店舗コード</param>
    ''' <param name="account">ログインアカウント</param>
    ''' <param name="nowDate">現在日時</param>
    ''' <param name="complaintDateCount">苦情表示日数</param>
    ''' <returns>参考情報一覧</returns>
    ''' <remarks></remarks>
    Public Function GetReferenceVisitCustomer(ByVal dealerCode As String, _
                                              ByVal storeCode As String, _
                                              ByVal account As String, _
                                              ByVal nowDate As Date, _
                                              ByVal complaintDateCount As Integer) _
                                         As SC3100201DataSet.NotDealVisitDataTable
        ' $01 start step2開発
        Logger.Info("GetReferenceVisitCustomer_Start Param[" & dealerCode & _
                     ", " & storeCode & _
                     ", " & account & _
                     ", " & nowDate & _
                     ", " & complaintDateCount & _
                     "]")
        ' $01 end   step2開発

        ' 取得開始日時と取得終了日時の作成
        Dim dateSt As New Date(nowDate.Year, nowDate.Month, nowDate.Day, 0, 0, 0)
        Dim dateEd As New Date(nowDate.Year, nowDate.Month, nowDate.Day, 23, 59, 59)

        ' 参考情報一覧
        Dim visitDt As SC3100201DataSet.NotDealVisitDataTable = New SC3100201DataSet.NotDealVisitDataTable

        Using adapter As New SC3100201TableAdapter

            ' 顧客担当来店実績一覧の取得
            Dim visitCustomerStaffDt As SC3100201DataSet.NotDealVisitDataTable = _
                adapter.GetVisitCustomerStaff(dealerCode, storeCode, account, dateSt, dateEd)

            ' 案内通知対応来店実績一覧の取得
            Dim visitReciveNoticeDt As SC3100201DataSet.NotDealVisitDataTable = _
                adapter.GetVisitReceiveNotice(dealerCode, storeCode, account, dateSt, dateEd)

            ' 取得した来店実績一覧のマージ
            Dim mergeDt As SC3100201DataSet.NotDealVisitDataTable = visitReciveNoticeDt
            mergeDt.Merge(visitCustomerStaffDt)

            ' 件数チェック
            If 0 >= mergeDt.Count Then

                Logger.Info("GetReferenceVisitCustomer_End Ret[Nothing]")
                Return Nothing
            End If

            ' $01 start step2開発
            ' 顧客情報の取得
            Me.SettingDetail(adapter, mergeDt, nowDate, complaintDateCount)
            ' $01 end   step2開発

            ' 来店実績一覧のソート
            Using dv As DataView = New DataView(mergeDt)

                dv.Sort = "VISITTIMESTAMP"

                For Each drv As DataRowView In dv
                    visitDt.ImportRow(drv.Row)
                Next
            End Using

        End Using

        Logger.Info("GetReferenceVisitCustomer_End Ret[" & visitDt.Count & "]")
        Return visitDt

    End Function

    ''' <summary>
    ''' 来店実績情報を取得する。
    ''' </summary>
    ''' <param name="visitSeq">来店実績連番</param>
    ''' <returns>来店実績</returns>
    ''' <remarks></remarks>
    Public Function GetVisit(ByVal visitSeq As Long) As SC3100201DataSet.VisitSalesRow
        Logger.Info("GetVisit_Start Param[" & visitSeq & "]")

        ' 来店実績
        Dim dt As SC3100201DataSet.VisitSalesDataTable = Nothing

        Using adapter As New SC3100201TableAdapter

            ' 来店実績情報の取得
            dt = adapter.GetVisit(visitSeq)

            ' 件数チェック
            If 0 >= dt.Count Then

                Logger.Info("GetVisit_End Ret[Nothing]")
                Return Nothing
            End If

        End Using

        Logger.Info("GetVisit_End Ret[" & dt.Count & "]")
        Return dt.Item(0)

    End Function

    ''' <summary>
    ''' 対応依頼通知の存在有無を取得する。
    ''' </summary>
    ''' <param name="visitSeq">来店実績連番</param>
    ''' <param name="account">対象アカウント</param>
    ''' <returns>存在有無</returns>
    ''' <remarks></remarks>
    Public Function ExistsVisitDealRequestNotice(ByVal visitSeq As Long, _
                             ByVal account As String) As Boolean
        Logger.Info("ExistsVisitDealRequestNotice_Start Param[" & visitSeq & _
                     ", " & account & _
                     "]")

        ' 存在有無
        Dim isExists As Boolean = False

        Using adapter As New SC3100201TableAdapter

            ' 対応依頼通知の存在有無の取得
            isExists = adapter.ExistsVisitDealRequestNotice(visitSeq, account)

        End Using

        Logger.Info("ExistsVisitDealRequestNotice_End Ret[" & isExists & "]")
        Return isExists

    End Function

    ''' <summary>
    ''' 来店客の対応
    ''' </summary>
    ''' <param name="visitSeq">来店実績連番</param>
    ''' <param name="visitStatus">来店実績ステータス</param>
    ''' <param name="isUpdateDealAccount">対応担当アカウントの更新有無</param>
    ''' <param name="dealAccount">対応担当アカウント</param>
    ''' <param name="updateAccount">更新アカウント</param>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="storeCode">店舗コード</param>
    ''' <param name="updateDate">取得更新日時</param>
    ''' <returns>メッセージID</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証
    ''' </history>
    <EnableCommit()> _
    Public Function UpdateVisitCustomer( _
            ByVal visitSeq As Long, ByVal visitStatus As String, _
            ByVal isUpdateDealAccount As Boolean, ByVal dealAccount As String, _
            ByVal updateAccount As String, ByVal dealerCode As String, ByVal storeCode As String, _
            ByVal updateDate As String) As Integer _
            Implements ISC3100201BusinessLogic.UpdateVisitCustomer

        Logger.Info(New StringBuilder("UpdateVisitCustomer_Start Param[").Append(visitSeq).Append( _
                ", ").Append(visitStatus).Append(", ").Append(isUpdateDealAccount).Append( _
                ", ").Append(dealAccount).Append(", ").Append(updateAccount).Append(", ").Append( _
                dealerCode).Append(", ").Append(storeCode).Append(", ").Append(updateDate).Append( _
                "]").ToString())

        Try
            Using adapter As New SC3100201TableAdapter

                ' 来店実績データの来店客対応更新
                Dim isSuccessUpdateVisit As Boolean = adapter.UpdateVisit(visitSeq, _
                        visitStatus, isUpdateDealAccount, dealAccount, updateDate, _
                        updateAccount, UpdateId)

                ' 更新結果が失敗の場合
                If Not isSuccessUpdateVisit Then
                    ' Logger.Debug("UpdateVisitCustomer_001 " & "Not isSuccessUpdateVisit")

                    ' ロールバックを設定
                    Me.Rollback = True

                    ' 終了
                    Logger.Error("UpdateVisitCustomer ResultId:" & CStr(MessageConcurrencyViolation))
                    Logger.Error("UpdateVisitCustomer_Start Ret[" & MessageConcurrencyViolation & "]")
                    Return MessageConcurrencyViolation
                End If

                ' Logger.Debug("UpdateVisitCustomer_002 " & "isSuccessUpdateVisit")

                ' 対応依頼通知データの論理削除更新
                adapter.UpdateVisitDealRequestNotice(visitSeq, updateAccount, UpdateId)

            End Using

        Catch oraEx As OracleExceptionEx
            ' データベースの操作中に例外が発生した場合
            ' Logger.Debug("UpdateVisitCustomer_003 " & "Catch OracleExceptionEx")

            If oraEx.Number = ErrorCodeOra2049 Then
                'DBタイムアウトエラー時
                ' Logger.Debug("UpdateVisitCustomer_004 " & "oraEx.Number = ErrorCodeOra2049")

                ' ロールバックを設定
                Me.Rollback = True

                'ログ出力
                Logger.Error("UpdateVisitCustomer ResultId:" & CStr(MessageDBTimeout), oraEx)
                Logger.Error("UpdateVisitCustomer_End Ret[" & MessageDBTimeout & "]")
                Return MessageDBTimeout

            Else
                ' Logger.Debug("UpdateVisitCustomer_005 " & "oraEx.Number <> ErrorCodeOra2049")

                '上記以外のエラーは基盤側で制御
                Logger.Error("UpdateVisitCustomer_End Ret[Throw OracleExceptionEx]")
                Throw
            End If
        End Try

        ' Logger.Debug("UpdateVisitCustomer_006")

        ' メッセージIDを返却
        Logger.Info("UpdateVisitCustomer_End Ret[" & MessageSuccess & "]")
        Return MessageSuccess

    End Function

    ''' <summary>
    ''' Push送信
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="storeCode">店舗コード</param>
    ''' <param name="beforeVisitStatus">更新前来店実績ステータス</param>
    ''' <param name="afterVisitStatus">更新後来店実績ステータス</param>
    ''' <remarks></remarks>
    Public Sub SendPush( _
            ByVal dealerCode As String, ByVal storeCode As String, _
            ByVal beforeVisitStatus As String, ByVal afterVisitStatus As String)

        Logger.Info(New StringBuilder("SendPush_Start Param[").Append(dealerCode).Append( _
                ", ").Append(storeCode).Append(", ").Append(beforeVisitStatus).Append( _
                ", ").Append(afterVisitStatus).Append("]").ToString())

        ' js1の第２引数を取得
        Dim js1Arg2 As String = GetJs1Arg2(beforeVisitStatus, afterVisitStatus)

        ' $01 start step2開発
        ' 操作権限コードのリスト（受付メイン画面再描画）
        Dim tabletOperationCdList As New List(Of Decimal)
        tabletOperationCdList.Add(OperationCodeReception)
        tabletOperationCdList.Add(OperationCodeSalesStaffManager)

        ' Push対象のアカウント情報の取得（受付メイン画面再描画）
        Using tabletUsersDataTable As VisitUtilityUsersDataTable = GetPushUser(dealerCode, storeCode, tabletOperationCdList)
            ' すべてのデータ行
            For Each dr As VisitUtilityUsersRow In tabletUsersDataTable
                ' Push送信
                SendTabletPushUser(dr.ACCOUNT, js1Arg2)
            Next dr
        End Using

        ' $02 start SSV廃止
        ' 操作権限コードのリスト（SSV画面再描画）
        ' Dim pcOperationCdList As New List(Of Decimal)
        ' pcOperationCdList.Add(OperationCodeSsv)

        ' Push対象のアカウント情報の取得（SSV画面再描画）
        ' Using pcUsersDataTable As VisitUtilityUsersDataTable = GetPushUser(dealerCode, storeCode, pcOperationCdList)
        ' すべてのデータ行
        ' For Each dr As VisitUtilityUsersRow In pcUsersDataTable
        ' Push送信
        ' SendPcPushUser(dr.ACCOUNT, js1Arg2)
        ' Next dr
        ' End Using
        ' $01 end   step2開発
        ' $02 end   SSV廃止

        Logger.Info("SendPush_End")

    End Sub

#End Region

#Region "Private関数"

    ''' <summary>
    ''' 顧客情報の取得と詳細設定を行う。
    ''' </summary>
    ''' <param name="adapter">アダプター</param>
    ''' <param name="visitDt">対象の来店実績一覧</param>
    ''' <param name="nowDate">現在日時</param>
    ''' <param name="complaintDateCount">苦情表示日数</param>
    ''' <remarks></remarks>
    ''' 
    Private Sub SettingDetail(ByVal adapter As SC3100201TableAdapter, _
                              ByRef visitDt As SC3100201DataSet.NotDealVisitDataTable, _
                              ByVal nowDate As Date, _
                              ByVal complaintDateCount As Integer)
        ' $01 start step2開発
        ' Logger.Debug(New StringBuilder("SettingDetail_Start Param[SC3100201TableAdapter, SC3100201DataSet.NotDealVisitDataTable, ") _
        '            .Append(nowDate).Append(", ").Append(complaintDateCount).Append("]").ToString)
        ' $01 end   step2開発

        ' 1行毎に処理
        For Each visitRow As SC3100201DataSet.NotDealVisitRow In visitDt

            ' 顧客情報の設定
            ' 顧客区分、または、顧客IDが設定されていない場合
            If visitRow.IsCUSTSEGMENTNull OrElse visitRow.IsCUSTIDNull Then
                Continue For
            End If

            ' 顧客情報
            Dim customerDt As SC3100201DataSet.CustomerDataTable = Nothing

            ' 顧客区分が自社客の場合
            If visitRow.CUSTSEGMENT.Equals(SegmentCustomer) Then

                ' Logger.Debug("SettingDetail_S001 SC3100201TableAdapter.GetOrgCustomer")
                customerDt = adapter.GetOrgCustomer(visitRow.CUSTID)

                ' 顧客区分が未取引客の場合
            ElseIf visitRow.CUSTSEGMENT.Equals(SegmentNewCustomer) Then

                ' Logger.Debug("SettingDetail_S002 SC3100201TableAdapter.GetNewCustomer")
                customerDt = adapter.GetNewCustomer(visitRow.CUSTID)

            End If

            ' 顧客情報が取得できなかった場合
            If customerDt Is Nothing OrElse 0 >= customerDt.Count Then
                Continue For
            End If

            ' 顧客情報を移行
            Dim customerRow As SC3100201DataSet.CustomerRow = customerDt.Item(0)
            visitRow.CUSTOMERNAME = customerRow.NAME
            visitRow.CUSTOMERNAMETITLE = customerRow.NAMETITLE
            visitRow.CUSTOMERIMAGEFILE = customerRow.IMAGEFILE

            ' $01 start step2開発
            '苦情件数の取得
            Dim utility As New VisitUtilityBusinessLogic

            If utility.HasClaimInfo(visitRow.CUSTSEGMENT, visitRow.CUSTID, nowDate, complaintDateCount) Then
                visitRow.CLAIMINFO = True
            Else
                visitRow.CLAIMINFO = False
            End If

            utility = Nothing
            ' $01 end   step2開発
        Next

        ' Logger.Debug("SettingDetail_End")
    End Sub

    ''' <summary>
    ''' js1の第２引数を取得する
    ''' </summary>
    ''' <param name="beforeVisitStatus">更新前来店実績ステータス</param>
    ''' <param name="afterVisitStatus">更新後来店実績ステータス</param>
    ''' <returns>js1の第２引数</returns>
    ''' <remarks></remarks>
    Private Function GetJs1Arg2(ByVal beforeVisitStatus As String, _
                                       ByVal afterVisitStatus As String) As String
        ' Logger.Debug("GetJs1Arg2_Start Param[" & beforeVisitStatus & _
        '             ", " & afterVisitStatus & "]")

        ' js1の第２引数
        Dim js1Arg2 As String = Nothing

        If VisitStatusWait.Equals(beforeVisitStatus) Then
            ' 更新前来店実績ステータスが待ちの場合

            If VisitStatusDefinition.Equals(afterVisitStatus) _
                OrElse VisitStatusDefinitionBroud.Equals(afterVisitStatus) Then
                ' 更新後来店実績ステータスが了解の場合

                ' js1の第２引数に了解ボタンタップ（お客様が待ち状態）を設定
                js1Arg2 = ProcessWaitToDecision

            ElseIf VisitStatusFree.Equals(afterVisitStatus) Then
                ' 更新後来店実績ステータスがフリーの場合

                ' js1の第２引数に不可ボタンタップ（お客様が待ち状態）を設定
                js1Arg2 = ProcessWaitToFree
            End If

        Else
            ' 更新前来店実績ステータスが待ち以外の場合

            If VisitStatusDefinition.Equals(afterVisitStatus) _
                OrElse VisitStatusDefinitionBroud.Equals(afterVisitStatus) Then
                ' 更新後来店実績ステータスが了解の場合

                ' js1の第２引数に了解ボタンタップ（お客様が来店状態）を設定
                js1Arg2 = ProcessNotWaitToDecision

            ElseIf VisitStatusWait.Equals(afterVisitStatus) Then
                ' 更新後来店実績ステータスが待ちの場合

                ' js1の第２引数に待ちボタンタップを設定
                js1Arg2 = ProcessNotWaitToWait

            ElseIf VisitStatusFree.Equals(afterVisitStatus) Then
                ' 更新後来店実績ステータスがフリーの場合

                ' js1の第２引数に不可ボタンタップ（お客様が来店状態）を設定
                js1Arg2 = ProcessNotWaitToFree
            End If
        End If

        ' 戻り値にjs1の第２引数を設定
        ' Logger.Debug("GetJs1Arg2_End Ret[" & js1Arg2 & "]")
        Return js1Arg2

    End Function

#Region "Push送信"

    ''' <summary>
    ''' Push対象のアカウント情報の取得
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="storeCode">店舗コード</param>
    ''' <param name="operationCodeList">権限コードリスト</param>
    ''' <returns>ユーザー情報のデータテーブル</returns>
    ''' <remarks></remarks>
    Private Function GetPushUser(ByVal dealerCode As String, _
                                 ByVal storeCode As String, _
                                 ByVal operationCodeList As List(Of Decimal)
                                        ) As VisitUtilityUsersDataTable
        ' Logger.Debug("GetPushUser_Start Param[" & dealerCode & _
        '             ", " & storeCode & "," & IsNothing(operationCodeList).ToString & "]")

        ' $01 start step2開発
        ' オンラインユーザー情報を取得する
        Dim utility As New VisitUtilityBusinessLogic
        Dim sendPushUsers As VisitUtilityUsersDataTable = _
            utility.GetOnlineUsers(dealerCode, storeCode, operationCodeList)
        utility = Nothing
        ' $01 end   step2開発

        ' 戻り値にユーザー情報のデータテーブルを設定
        ' Logger.Debug("GetPushUser_End Ret[" & (sendPushUsers IsNot Nothing) & "]")
        Return sendPushUsers

    End Function

    ''' <summary>
    ''' Push送信（タブレット）
    ''' </summary>
    ''' <param name="uid">uidキーの値</param>
    ''' <param name="js1Arg2">js1キーの第二引数</param>
    ''' <remarks></remarks>
    Private Sub SendTabletPushUser(ByVal uid As String, ByVal js1Arg2 As String)
        ' Logger.Debug("SendTabletPushUser_Start Param[" & uid & _
        '             ", " & js1Arg2 & "]")

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
            .Append(SourceSC3100201)
            .Append("', '")
            .Append(js1Arg2)
            .Append("')")

        End With

        ' Push送信（タブレット）
        Dim visitUtility As New VisitUtility
        visitUtility.SendPush(postMsg.ToString)

        ' Logger.Debug("SendTabletPushUser_End")
    End Sub

    ' $01 start step2開発
    ''' <summary>
    ''' Push送信（PC）
    ''' </summary>
    ''' <param name="uid">uidキーの値</param>
    ''' <param name="js1Arg2">js1キーの第二引数</param>
    ''' <remarks></remarks>
    Private Sub SendPcPushUser(ByVal uid As String, ByVal js1Arg2 As String)
        ' Logger.Debug("SendPcPushUser_Start Param[" & uid & _
        '             ", " & js1Arg2 & "]")

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
            .Append(SourceSC3100201)
            .Append("', '")
            .Append(js1Arg2)
            .Append("')")

        End With

        ' Push送信（PC）
        Dim visitUtility As New VisitUtility
        visitUtility.SendPushPC(postMsg.ToString)

        ' Logger.Debug("SendPcPushUser_End")
    End Sub
    ' $01 end   step2開発

#End Region

#End Region

End Class