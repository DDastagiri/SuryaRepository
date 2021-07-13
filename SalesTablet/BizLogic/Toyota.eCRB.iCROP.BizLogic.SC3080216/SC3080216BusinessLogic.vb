'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3080216BusinessLogic.vb
'─────────────────────────────────────
'機能： 顧客詳細(受注後工程フォロー)
'補足： 
'作成： 2014/02/13 TCS 森 受注後フォロー機能開発
'更新： 2014/03/18 TCS 葛西 切替BTS-210対応
'更新： 2014/05/30 TCS 市川 TMT不具合対応
'更新： 2014/08/01 TCS 市川 受注後フォロー機能開発(UAT-BTS-212対応)
'更新： 2014/08/20 TCS 森   受注後活動A⇒H移行対応
'更新： 2014/12/25 TCS 外崎 車両ステイタス連携機能各国展開に向けた追加機能開発
'更新： 2019/01/25 TCS 中村(拓) TKM-UAT-0644
'更新： 2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展
'─────────────────────────────────────
Imports System.Xml
Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.CommonUtility.BizLogic
Imports Toyota.eCRB.CommonUtility.DataAccess
Imports Toyota.eCRB.CustomerInfo.Details.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.iCROP.DataAccess.CalenderXmlCreateClass
Imports Toyota.eCRB.iCROP.BizLogic.CalenderXmlCreateClass.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess.DlrEnvSettingDataSet
Imports Toyota.eCRB.Common.VisitResult.BizLogic

''' <summary>
''' SC3080216(顧客詳細(受注後工程フォロー))
''' Webページで使用するビジネスロジック
''' </summary>
''' <remarks></remarks>
''' 
Public NotInheritable Class SC3080216BusinessLogic
    Inherits BaseBusinessComponent

#Region "定数"

    ''' <summary>
    ''' 受注時フラグ（0:受注時）
    ''' </summary>
    ''' <remarks></remarks>
    Public Const SalesFlg As String = "0"

    ''' <summary>
    ''' 受注時フラグ（1:受注後工程フォロー）
    ''' </summary>
    ''' <remarks></remarks>
    Public Const SalesAfterFlg As String = "1"

    ''' <summary>
    ''' 画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONTENT_MODULEID As String = "SC3080216"

    ''' <summary>
    ''' 在席状態：商談中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STAFF_STATUS_NEGOTIATION As String = "20"

    ''' <summary>
    ''' 在席状態：納車作業中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STAFF_STATUS_DELIVERY As String = "22"

    ''' <summary>
    ''' 登録時エラーＩＤ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DB_ERR_MSGID As Integer = 30931

    ''' <summary>
    ''' 2:新規活動結果登録処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NEW_FOLLOW As String = "2"

    ''' <summary>
    ''' 3:新規活動結果更新処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Const UPD_FOLLOW As String = "3"

    ' ''' <summary>
    ' ''' 自社客/未取引客フラグ (1：自社客)
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Public Const Orgcustflg As String = "1"

    ''' <summary>
    ''' 自社客/未取引客フラグ (2：未取引客)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const Newcustflg As String = "2"

    ''' <summary>
    ''' ダミー名称フラグ  正式名称:0　ダミー名称:1
    ''' </summary>
    Private Const DummyNameFlgOfficial As String = "0"
    Private Const DummyNameFlgDummy As String = "1"

    ' ''' <summary>
    ' ''' 自社客/未取引客フラグ (0：自社客) カルタブ連携用
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const ORGCUSTFLG_RENKEI As String = "0"

    ''' <summary>
    ''' ログのフォーマット（OracleExceptionEx 発生）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FormatOracleExceptionExLog As String = "An exception occurred during the operation of the database. messageId = {0}"

    ''' <summary>
    ''' 受注後工程コード 契約
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ENVSETTINGKEY_AFTER_ODR_CONTRACT As String = "AFTER_ODR_ACT_CD_CONTRACT" '契約

    ''' <summary>
    ''' CalDAV連携 処理区分 登録
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CALDAV_TODO_INS As String = "1"

    ''' <summary>
    ''' CalDAV連携 処理区分 更新
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CALDAV_TODO_UPD As String = "2"

    ''' <summary>
    ''' CalDAV連携 処理区分 削除
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CALDAV_TODO_DEL As String = "4"

    ''' <summary>
    ''' CalDAV連携 スケジュール区分 ToDo+Event
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CALDAV_SCHEDULE_TODOEVENT As String = "1"

    ''' <summary>
    ''' CalDAV連携 スケジュール区分 ToDo
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CALDAV_SCHEDULE_TODO As String = "2"


    ''' <summary>
    ''' CalDAV連携 接触方法 来店
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONTACT_MTD_WALK_IN As String = "11"

    ''' <summary>
    ''' 日付初期値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const INIT_DATATIME As String = "1900/01/01 00:00:00"

    ''' <summary>
    ''' 'CalDAV連携用URL
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_CALDAV_WEBSERVICE_URL As String = "CALDAV_WEBSERVICE_URL"

    ''' <summary>
    ''' スケジュール登録 処理区分 2:受注後(基本活動未完了なし)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CALDAV_AFTER_KBN As String = "2"

    ''' <summary>
    ''' 書類回収活動コード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ENVSETTINGKEY_AFTER_ODR_DOC_COLLECT As String = "AFTER_ODR_ACT_CD_DOCUMENTS_RECOVERY" '書類回収活動

    '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 START
    ''' <summary>
    ''' システム設定の指定パラメータ 受注後工程活動情報取得用
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_TDIS_INTRODUCTION_FLG As String = "TDIS_INTRODUCTION_FLG"
    '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 END

#End Region

#Region "構造体"

    Public Structure CalDavDate

        'Commonタグ用項目
        '販売店コード
        Private _DealerCode As String

        Public Property DealerCode As String

            Get
                Return _DealerCode
            End Get
            Set(value As String)
                _DealerCode = value
            End Set
        End Property


        '店舗コード
        Private _BranchCode As String

        Public Property BranchCode As String

            Get
                Return _BranchCode
            End Get
            Set(value As String)
                _BranchCode = value
            End Set
        End Property

        'ScheduleInfoタグ用項目
        '顧客区分
        Private _CustomerDiv As String

        Public Property CustomerDiv As String

            Get
                Return _CustomerDiv
            End Get
            Set(value As String)
                _CustomerDiv = value
            End Set
        End Property

        '顧客コード
        Private _CustomerCode As String

        Public Property CustomerCode As String

            Get
                Return _CustomerCode
            End Get
            Set(value As String)
                _CustomerCode = value
            End Set
        End Property

        '商談ID
        Private _SalesId As Decimal

        Public Property SalesId As Decimal

            Get
                Return _SalesId
            End Get
            Set(value As Decimal)
                _SalesId = value
            End Set
        End Property

        'スタッフコード
        Private _StaffCode As String

        Public Property StaffCode As String

            Get
                Return _StaffCode
            End Get
            Set(value As String)
                _StaffCode = value
            End Set
        End Property

        Public Overrides Function Equals(obj As Object) As Boolean

            If (obj Is Nothing) OrElse (Not Me.GetType() Is obj.GetType()) Then
                Return False
            End If

            Dim s As CalDavDate = CType(obj, CalDavDate)

            Return (Me.DealerCode = s.DealerCode) And (Me.BranchCode = s.BranchCode) And (Me.CustomerDiv =
                    s.CustomerDiv) And (Me.CustomerCode = s.CustomerCode) And (Me.SalesId = s.SalesId) And
                (Me.StaffCode = s.StaffCode)

        End Function

        Public Overrides Function GetHashCode() As Integer

            Return CInt(DealerCode.GetHashCode() Xor BranchCode.GetHashCode() Xor CustomerDiv.GetHashCode() Xor
                CustomerCode.GetHashCode() Xor CLng(SalesId) Xor StaffCode.GetHashCode())
        End Function

    End Structure

#End Region


    '2014/03/18 TCS 葛西 切替BTS-210対応 START
    '2014/03/18 TCS 葛西 切替BTS-210対応 END

    ''' <summary>
    ''' 初期表示情報取得
    ''' </summary>
    ''' <param name="inputRow">データセット (インプット)</param>
    ''' <param name="msgId">メッセージID</param>
    ''' <remarks>初期表示表示用情報を取得する。</remarks>
    ''' <history></history>
    Public Shared Sub GetInitialize(ByVal inputRow As SC3080216DataSet.SC3080216PlanRow, ByRef msgId As Integer)

        Logger.Info("GetInitialize Start")

        msgId = 0
        'Follow-up Box商談取得
        Dim fllwSalesTbl As SC3080216DataSet.SC3080216FllwSalesDataTable
        fllwSalesTbl = SC3080216TableAdapter.GetFllwupboxSales(inputRow.FLLWUPBOX_SEQNO)

        Dim fllwSalesRow As SC3080216DataSet.SC3080216FllwSalesRow
        fllwSalesRow = fllwSalesTbl.Item(0)

        '出力用データセット作成
        inputRow.CUSTOMERCLASS = fllwSalesRow.CUSTOMERCLASS       '顧客分類
        inputRow.CRCUSTID = fllwSalesRow.CRCUSTID                 '活動先顧客コード
        inputRow.ACTUALACCOUNT = fllwSalesRow.ACTUALACCOUNT       '対応アカウント
        If (fllwSalesRow.IsWALKINNUMNull) Then
            inputRow.SetWALKINNUMNull()                           '来店人数
        Else
            inputRow.WALKINNUM = fllwSalesRow.WALKINNUM           '来店人数
        End If
        inputRow.NEWFLLWUPBOXFLG = fllwSalesRow.NEWFLLWUPBOXFLG   '新規活動フラグ
        inputRow.REGISTFLG = fllwSalesRow.REGISTFLG               '登録フラグ


        '商談開始時間
        If (Not fllwSalesRow.IsSTARTTIMENull()) Then
            inputRow.SALESSTARTTIME = fllwSalesRow.STARTTIME
        End If

        '商談終了時間
        If (Not fllwSalesRow.IsENDTIMENull()) Then
            inputRow.SALESENDTIME = fllwSalesRow.ENDTIME
        End If

        '活動終了日時
        If (Not fllwSalesRow.IsEIGYOSTARTTIMENull()) Then
            inputRow.EIGYOSTARTTIME = fllwSalesRow.EIGYOSTARTTIME
        End If

        fllwSalesTbl.Dispose()

        Logger.Info("GetInitialize End")

    End Sub

    ''' <summary>
    ''' 入力チェック処理
    ''' </summary>
    ''' <param name="inputDataTbl"></param>
    ''' <param name="newCustDt"></param>
    ''' <param name="custKind"></param>
    ''' <param name="msgid"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function IsInputeCheck(ByVal inputDataTbl As SC3080216DataSet.SC3080216PlanDataTable, _
                                         ByVal newCustDt As ActivityInfoDataSet.GetNewCustomerDataTable, _
                                         ByVal custKind As String, _
                                         ByRef msgid As Integer, _
                                         ByRef msgItem0 As String) As Boolean

        '--デバッグログ---------------------------------------------------
        Logger.Info("IsInputeCheck Start")
        '-----------------------------------------------------------------

        '未取引客の場合のみダミー名称、電話番号のチェックを行う
        If Newcustflg.Equals(custKind) Then
            Dim newCustRw As ActivityInfoDataSet.GetNewCustomerRow = CType(newCustDt.Rows(0), ActivityInfoDataSet.GetNewCustomerRow)

            'ダミー名称の場合
            If DummyNameFlgDummy.Equals(newCustRw.DUMMYNAMEFLG) Then
                msgid = 30933
                Return False
            End If

        End If

        Dim Registrw As SC3080216DataSet.SC3080216PlanRow
        Registrw = inputDataTbl.Item(0)

        '必須入力チェック
        '2014/08/01 TCS 市川 受注後フォロー機能開発(UAT-BTS-212対応) START
        If Not ActivityInfoBusinessLogic.MandatoryCheck(StaffContext.Current.DlrCD, StaffContext.Current.BrnCD, Decimal.Parse(Registrw.CRCUSTID, CultureInfo.InvariantCulture), Registrw.FLLWUPBOX_SEQNO, msgid, msgItem0, True) Then
            '2014/08/01 TCS 市川 受注後フォロー機能開発(UAT-BTS-212対応) END
            Return False
        End If

        '開始時間[活動内容]を入力してください
        If Registrw.IsSALESSTARTTIMENull Then
            msgid = 30901
            Return False
        End If

        '終了時間[活動内容]を入力してください
        If Registrw.IsSALESENDTIMENull Then
            msgid = 30903
            Return False
        End If

        '終了時間[活動内容]を開始時間[活動内容]より未来の時間で入力してください
        If Registrw.SALESSTARTTIME.ToString("HH:mm", CultureInfo.CurrentCulture) > Registrw.SALESENDTIME.ToString("HH:mm", CultureInfo.CurrentCulture) Then
            msgid = 30905
            Return False
        End If

        '日付[活動内容]を現在より過去の日時で入力してください
        Dim actdayto As String = Registrw.SALESSTARTTIME.ToString("yyyy/MM/dd", CultureInfo.CurrentCulture) & " " & Registrw.SALESENDTIME.ToString("HH:mm", CultureInfo.CurrentCulture) '活動開始の年月日＋活動終了時間
        Dim actDayToDate As Date = Date.ParseExact(actdayto, "yyyy/MM/dd HH:mm", Nothing)

        Dim actdayfrom As String = Registrw.SALESSTARTTIME.ToString("yyyy/MM/dd", CultureInfo.CurrentCulture)
        Dim actdayFromDate As Date = Date.ParseExact(actdayfrom, "yyyy/MM/dd", Nothing)
        If actdayFromDate > Today() Then
            msgid = 30906
            Return False
        End If


        '終了時間を前回の活動終了時間{0}より未来の時間で入力してください
        If Registrw.IsLATEST_TIME_ENDNull = False Then
            If actDayToDate <= Registrw.LATEST_TIME_END Then
                msgid = 30932
                Return False
            End If
        End If

        '対応SCを選択してください
        If String.IsNullOrEmpty(Registrw.ACTUALACCOUNT) Then
            msgid = 30907
            Return False
        End If

        '分類[活動内容]を選択してください
        If Registrw.IsCONTACTNONull Then
            msgid = 30908
            Return False
        End If

        Return True

    End Function

    '2014/03/18 TCS 葛西 切替BTS-210対応 START
    ''' <summary>
    ''' 受注時、活動結果登録	受注時、活動結果登録
    ''' </summary>
    ''' <param name="inputDataTbl">データセット (インプット)</param>
    ''' <param name="msgId">メッセージID</param>
    ''' <returns>更新結果</returns>
    ''' <remarks>受注時、活動結果登録	受注時、活動結果登録を実施する。</remarks>
    <EnableCommit()>
    Public Function UpdateSales(ByVal inputDataTbl As SC3080216DataSet.SC3080216PlanDataTable, _
                                       ByVal activDataTbl As ActivityInfoDataSet.ActivityInfoRegistDataDataTable, _
                                                                              ByRef msgId As Integer) As Boolean
        '2014/03/18 TCS 葛西 切替BTS-210対応 END

        Logger.Info("UpdateSales Start")

        msgId = 0

        Dim row As SC3080216DataSet.SC3080216PlanRow
        row = inputDataTbl.Item(0)

        '新規活動フラグ＝1:新規
        Dim resistFlg As String = String.Empty
        If (row.NEWFLLWUPBOXFLG.Equals("1") = True) Then
            '新規活動結果登録処理
            resistFlg = NEW_FOLLOW
        Else
            '新規活動結果更新処理
            resistFlg = UPD_FOLLOW
        End If

        ' ステータス取得
        Dim datatableStatus As SC3080216DataSet.SC3080216CrstatusDataTable
        datatableStatus = SC3080216TableAdapter.GetFollowCractstatus(row.FLLWUPBOX_SEQNO)


        Dim fllwStatus As String = String.Empty
        If (datatableStatus.Rows.Count > 0) Then
            fllwStatus = datatableStatus.Item(0).CRACTSTATUS
        End If

        '活動結果を登録する　(3:SUCESS)
        If (Not InsertActivityResult(activDataTbl, resistFlg, fllwStatus)) Then
            msgId = 901
            Logger.Error("SC3080216BusinessLogic.UpdateSales - Internal Error: InsertActivityResult")
            Me.Rollback = True
            Return False
        End If

        Logger.Info("UpdateSales End")

        Return True

    End Function


    '2014/03/18 TCS 葛西 切替BTS-210対応 START
    ''' <summary>
    ''' 受注後、活動結果登録	受注後、活動結果登録
    ''' </summary>
    ''' <param name="inputDataTbl">データセット (インプット)</param>
    ''' <returns>更新結果</returns>
    ''' <remarks>受注後、活動結果登録	受注後、活動結果登録を実施する。</remarks>
    <EnableCommit()>
    Public Function UpdateSalesAfter(ByVal inputDataTbl As SC3080216DataSet.SC3080216PlanDataTable,
                                            ByVal vclid As Decimal, ByVal actid As Decimal) As Boolean

        '2014/03/18 TCS 葛西 切替BTS-210対応 END
        Dim msgId As Integer

        Logger.Info("UpdateSalesAfter Start")

        msgId = 0

        '活動内容、活動結果登録	活動内容、活動結果登録
        If (Not UpdateFllwupboxSales(inputDataTbl, msgId)) Then
            Logger.Error("SC3080216BusinessLogic.UpdateSalesAfter - Internal Error: UpdateFllwupboxSales (msgId:" & msgId & ")")
            msgId = DB_ERR_MSGID
            Me.Rollback = True
            Return False
        End If

        'Dim cnt As Integer
        Dim row As SC3080216DataSet.SC3080216PlanRow
        row = inputDataTbl.Item(0)

        '実績連番の取得
        row.SEQNO = SC3080216TableAdapter.GetBookedafterFollowrsltMax()

        '受注後工程フォロー結果追加
        SC3080216TableAdapter.InsertBookedafterFollowrslt(row, CONTENT_MODULEID)
        '031.Follow-up Box商談メモ追加
        Dim crcustId As Decimal
        If (Not String.IsNullOrEmpty(row.CRCUSTID)) Then
            crcustId = CDec(row.CRCUSTID)
        Else
            crcustId = 0
        End If
        If actid = 0 Then
            actid = SC3080216TableAdapter.GetMaxActId(row.FLLWUPBOX_SEQNO)
        End If
        SC3080216TableAdapter.InsertFllwupboxSalesmemoHis(row.FLLWUPBOX_SEQNO, crcustId, vclid, actid, row.ACCOUNT)
        '055.Follow-up Box商談メモWK削除
        ActivityInfoTableAdapter.DeleteFllwupboxSalesmemowk(row.FLLWUPBOX_SEQNO)

        Dim staffInfo As StaffContext = StaffContext.Current
        Dim staffStatus As String = staffInfo.PresenceCategory & staffInfo.PresenceDetail
        Dim UpdateVisitSales As New UpdateSalesVisitBusinessLogic

        '20:商談中
        If String.Equals(staffStatus, STAFF_STATUS_NEGOTIATION) Or String.Equals(staffStatus, STAFF_STATUS_DELIVERY) Then

            '来店実績更新_商談終了
            Dim endDate As Date = ChangeDate(row.SALESSTARTTIME.ToString("yyyy/MM/dd", CultureInfo.CurrentCulture) & " " & row.SALESENDTIME.ToString("HH:mm", CultureInfo.CurrentCulture))

            '20：商談中
            If String.Equals(staffStatus, STAFF_STATUS_NEGOTIATION) Then
                UpdateVisitSales.UpdateVisitSalesEnd(row.CUSTSEGMENT, row.CRCUSTID, endDate, CONTENT_MODULEID, msgId, UpdateSalesVisitBusinessLogic.LogicStateNegotiationFinish)

                '22：納車作業中
            ElseIf String.Equals(staffStatus, STAFF_STATUS_DELIVERY) Then
                ' 納車作業終了を追加
                UpdateVisitSales.UpdateVisitSalesEnd(row.CUSTSEGMENT, row.CRCUSTID, endDate, CONTENT_MODULEID, msgId, UpdateSalesVisitBusinessLogic.LogicStateDeliverlyFinish)
            End If

            If msgId <> 0 Then
                Logger.Error("SC3080216BusinessLogic.UpdateSalesAfter - Internal Error: UpdateSalesVisitBusinessLogic.UpdateVisitSalesEnd (msgid:" & msgId & ")")
                Me.Rollback = True
                Return False
            End If


        End If

        'ステータスを「スタンバイ」に更新
        staffInfo.UpdatePresence("1", "0")

        Logger.Info("UpdateSalesAfter End")

        Return True

    End Function

    ''' <summary>
    ''' 活動内容、活動結果登録	活動内容、活動結果登録
    ''' </summary>
    ''' <param name="inputDataTbl">データセット (インプット)</param>
    ''' <param name="msgId">メッセージID</param>
    ''' <returns>更新結果</returns>
    ''' <remarks>活動内容、活動結果登録	活動内容、活動結果登録を実施する。</remarks>
    Public Shared Function UpdateFllwupboxSales(ByVal inputDataTbl As SC3080216DataSet.SC3080216PlanDataTable, ByRef msgId As Integer) As Boolean

        Logger.Info("UpdateFllwupboxSales Start")

        msgId = 0
        Dim ret As Integer = 1

        Dim row As SC3080216DataSet.SC3080216PlanRow
        row = inputDataTbl.Item(0)

        Logger.Info(row.FLLWUPBOX_SEQNO.ToString())
        'Follow-up Box商談の更新
        ret = ActivityInfoBusinessLogic.UpdateFllwupboxSales(row.FLLWUPBOX_SEQNO, _
                                                       row.ACTUALACCOUNT, _
                                                       row.SALESSTARTTIME, _
                                                       row.SALESENDTIME, _
                                                       row.ACCOUNT, _
                                                       CONTENT_MODULEID)


        Logger.Info("UpdateFllwupboxSales End")

        If (ret < 0) Then
            Return False
        Else
            Return True
        End If

    End Function


    ''' <summary>
    ''' 活動結果を登録する
    ''' </summary>
    ''' <param name="registdt"></param>
    ''' <param name="resistFlg">1:新規活動登録　2:新規活動登録からの即Success、Give-up　3:既存の活動に対する活動結果</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function InsertActivityResult(ByVal registdt As ActivityInfoDataSet.ActivityInfoRegistDataDataTable, ByVal resistFlg As String, ByVal fllwStatus As String) As Boolean

        Logger.Info("InsertActivityResult Start")

        If String.Equals(resistFlg, "2") Then
            ActivityInfoBusinessLogic.InsertActivityData(registdt)
        End If

        If String.Equals(resistFlg, "3") Then
            If (Not ActivityInfoBusinessLogic.UpdateActivityData(registdt)) Then
                Logger.Error("SC3080216BusinessLogic.InsertActivityResult - Internal Error: ActivityInfoBusinessLogic.UpdateActivityData")
                Return False
            End If
        End If

        Dim staffInfo As StaffContext = StaffContext.Current
        Dim msgid As Integer = 0
        Dim staffStatus As String = staffInfo.PresenceCategory & staffInfo.PresenceDetail
        Dim UpdateVisitSales As New UpdateSalesVisitBusinessLogic

        '20:商談中,22:納車作業中
        If String.Equals(staffStatus, STAFF_STATUS_NEGOTIATION) Or String.Equals(staffStatus, STAFF_STATUS_DELIVERY) Then

            '来店実績更新_商談終了
            Dim endDate As Date = ChangeDate(registdt(0).ACTDAYFROM.Substring(0, 10) & " " & registdt(0).ACTDAYTO)

            '20：商談中
            If String.Equals(staffStatus, STAFF_STATUS_NEGOTIATION) Then
                UpdateVisitSales.UpdateVisitSalesEnd(registdt(0).CUSTSEGMENT, registdt(0).CRCUSTID, endDate, CONTENT_MODULEID, msgid, UpdateSalesVisitBusinessLogic.LogicStateNegotiationFinish)
                '22：納車作業中
            ElseIf String.Equals(staffStatus, STAFF_STATUS_DELIVERY) Then
                ' 納品作業終了を追加
                UpdateVisitSales.UpdateVisitSalesEnd(registdt(0).CUSTSEGMENT, registdt(0).CRCUSTID, endDate, CONTENT_MODULEID, msgid, UpdateSalesVisitBusinessLogic.LogicStateDeliverlyFinish)
            End If

            If msgid <> 0 Then
                Logger.Error("SC3080216BusinessLogic.InsertActivityResult - Internal Error: UpdateSalesVisitBusinessLogic.UpdateVisitSalesEnd (msgid:" & msgid & ")")
                Return False
            End If

        End If

        'ステータスを「スタンバイ」に更新
        staffInfo.UpdatePresence("1", "0")

        'CalDAV連携実施
        ActivityInfoBusinessLogic.SetToDo(registdt, fllwStatus)

        Logger.Info("InsertActivityResult End")
        Return True
    End Function

    ''' <summary>
    ''' ステータスの取得
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared ReadOnly Property GetStaffStatus() As String

        Get
            Dim staffInfo As StaffContext = StaffContext.Current
            Dim staffStatus As String = staffInfo.PresenceCategory & staffInfo.PresenceDetail

            Return staffStatus

        End Get

    End Property

    ''' <summary>
    ''' 来店実績更新_商談終了時のPush送信
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Sub PushUpdateVisitSalesEnd(ByVal staffStatus As String)

        If String.Equals(staffStatus, STAFF_STATUS_NEGOTIATION) Or String.Equals(staffStatus, STAFF_STATUS_DELIVERY) Then

            '活動登録処理完了時、
            '来店実績更新_商談終了時のPush送信
            Dim UpdateVisitSales As New UpdateSalesVisitBusinessLogic

            '処理区分に1:商談終了を設定して呼び出し
            UpdateVisitSales.PushUpdateVisitSalesEnd(UpdateSalesVisitBusinessLogic.LogicStateNegotiationFinish)
        End If

    End Sub


    ''' <summary>
    ''' 文字列を日付型に変換する
    ''' </summary>
    ''' <remarks></remarks>
    Private Shared Function ChangeDate(ByVal dtString As String) As Date

        If (dtString.Length <= 10) Then
            Return Date.ParseExact(dtString, "yyyy/MM/dd", Nothing)
        Else
            If (dtString.Length <= 16) Then
                Return Date.ParseExact(dtString, "yyyy/MM/dd HH:mm", Nothing)
            Else
                Return Date.ParseExact(dtString, "yyyy/MM/dd HH:mm:ss", Nothing)

            End If
        End If

    End Function


    ''' <summary>
    ''' 受注後工程活動情報取得
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <param name="mode">取得モード(1:日付別、2:工程別</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetBookedafterActivityInfo(ByVal salesId As Decimal, ByVal mode As String) As SC3080216DataSet.SC3080216AfterOdracTDataTable

        Logger.Info("GetBookedafterActivityInfo Strat")

        '2014/12/25 TCS 外崎 車両ステイタス連携機能各国展開に向けた追加機能開発 START
        'Return SC3080216TableAdapter.GetBookedafterActivityInfo(salesId, mode)

        Dim resultTable = SC3080216TableAdapter.GetBookedafterActivityInfo(salesId, mode)

        '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 START
        Dim TDISIntroductionFlg As String
        Dim systemBiz As New SystemSettingDlr

        '①販売店≠'XXXXX'、店舗＝'XXX'販売店（販売店コードのみ該当）
        '②販売店＝'XXXXX'、店舗＝'XXX'（販売店コード・店舗コードいずれも該当なし(デフォルト値)  
        Dim drSettingDlr As SystemSettingDlrDataSet.TB_M_SYSTEM_SETTING_DLRRow = systemBiz.GetEnvSetting(StaffContext.Current.DlrCD, ConstantBranchCD.AllBranchCD, C_TDIS_INTRODUCTION_FLG)

        'データが存在しない場合は取得条件を変更して実行、データが存在していればその値を格納
        If drSettingDlr Is Nothing Then
            'NULLが返ってきた場合は0にする
            TDISIntroductionFlg = "0"
        Else
            TDISIntroductionFlg = drSettingDlr.SETTING_VAL
        End If
        '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 END

        If (TDISIntroductionFlg = "0") Then
            For Each row In resultTable
                If (row.AFTER_ODR_ACT_INPUT_TYPE = "3") Then
                    row.Delete()
                End If
            Next
            resultTable.AcceptChanges()
        End If

        Return resultTable
        '2014/12/25 TCS 外崎 車両ステイタス連携機能各国展開に向けた追加機能開発 END

        Logger.Info("GetBookedafterActivityInfo End")
    End Function


    ''' <summary>
    ''' 受注後工程活動情報更新
    ''' </summary>
    ''' <param name="input">更新データ</param>
    ''' <param name="salesId">商談ID</param>
    ''' <param name="account">活動担当スタッフ</param>
    ''' <param name="salesAfterFlg">受注後判定フラグ</param>
    ''' <param name="actCalDavInfo">CalDAV連携用情報</param>
    ''' <returns>更新完了件数</returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Shared Function UpdateBookedafterActivityInfo(ByVal salesId As Decimal, ByVal account As String, ByVal input As SC3080216DataSet.SC3080216UpdBookdafActiveDataTable,
                                                         ByVal salesAfterFlg As String, ByVal actCalDavInfo As SC3080216BusinessLogic.CalDavDate) As Integer

        Logger.Info("UpdateBookedafterActivityInfo Start")

        Logger.Info("SC3080216 UpdateBookedafterActivityInfo Start")

        Dim updCount As Integer = 0
        Dim BizClass As New SC3080216BusinessLogic
        Dim ActIdTable As New SC3080216DataSet.SC3080216ActIdDataTable

        '活動実施終了日
        Dim AfterOdrRsltEndDateorTime As String = ""

        '受注後工程情報
        Dim AfterOdrProc As New SC3080216DataSet.SC3080216AfterOdrProcDataTable

        '書類回収活動処理用変数
        Dim AfterOdrActId As Decimal = 0
        Dim UpdAfterOdrDocumentTable As New SC3080216DataSet.SC3080216UpdAfterOdrDocumentDataTable
        Dim UpdAfterOdrDocumentRow As SC3080216DataSet.SC3080216UpdAfterOdrDocumentRow =
        UpdAfterOdrDocumentTable.NewSC3080216UpdAfterOdrDocumentRow()

        UpdAfterOdrDocumentTable.AddSC3080216UpdAfterOdrDocumentRow(UpdAfterOdrDocumentRow)

        Try

            ' 受注時の場合活動IDを取得する
            If salesAfterFlg.Equals("0") Then
                ActIdTable = SC3080216TableAdapter.GetActId(salesId)
            End If

            '受注後工程活動登録処理を行う
            For Each UpdAfterActRow In input
                '完了フラグが完了済みの場合
                If UpdAfterActRow.AFTER_ODR_ACT_STATUS.Equals("1") Then

                    UpdAfterActRow.RSLT_DATEORTIME_FLG = "1"

                    If salesAfterFlg.Equals("0") Then
                        '受注時の場合活動IDを設定する
                        UpdAfterActRow.ACT_ID = ActIdTable.Item(0).ACT_ID
                        Logger.Info("salesAfterFlg = 0　Set ACT_ID:" + CType(UpdAfterActRow.ACT_ID, String))
                    Else
                        '受注後の場合受注後工程フォロー連番を設定する
                        UpdAfterActRow.AFTER_ODR_FLLW_SEQ =
                            SC3080216TableAdapter.GetBookedAfterFollowSeqNoMax(UpdAfterActRow.RSLT_DLR_CD, salesId)
                        Logger.Info("salesAfterFlg = 1  Set AFTER_ODR_FLLW_SEQ:" + CType(UpdAfterActRow.AFTER_ODR_FLLW_SEQ, String))

                    End If

                    '実施終了日を保持する ※CalDAV連携削除処理で使用
                    If Not UpdAfterActRow.IsRSLT_END_DATEORTIMENull Then
                        AfterOdrRsltEndDateorTime = UpdAfterActRow.RSLT_END_DATEORTIME.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture)
                    End If
                End If

                Logger.Info("UpdateBookedafterActivityInfo　Call Start")
                updCount = updCount + SC3080216TableAdapter.UpdateBookedafterActivityInfo(UpdAfterActRow)
                Logger.Info("UpdateBookedafterActivityInfo　Call End")

                '更新処理を行った受注後活動IDをKEYに書類回収活動かチェックを行う
                AfterOdrProc = SC3080216TableAdapter.GetAfterOrderProc(UpdAfterActRow.AFTER_ODR_ACT_ID)

                If AfterOdrProc.Item(0).AFTER_ODR_ACT_CD.Equals(SC3080216BusinessLogic.GetSysEnvSettingValue(ENVSETTINGKEY_AFTER_ODR_DOC_COLLECT)) Then

                    ''書類回収活動を更新した場合

                    AfterOdrActId = UpdAfterActRow.AFTER_ODR_ACT_ID

                    UpdAfterOdrDocumentRow.AFTER_ODR_ACT_ID = AfterOdrActId

                    UpdAfterOdrDocumentRow.AFTER_ODR_ACT_CD = SC3080216BusinessLogic.GetSysEnvSettingValue(ENVSETTINGKEY_AFTER_ODR_DOC_COLLECT)

                    UpdAfterOdrDocumentRow.AFTER_ODR_ACT_STATUS = CType(UpdAfterActRow.AFTER_ODR_ACT_STATUS, Decimal)

                    UpdAfterOdrDocumentRow.SCHE_SEND_DATE = AfterOdrProc.Item(0).SCHE_START_DATEORTIME.Date

                    UpdAfterOdrDocumentRow.RSLT_SEND_DATE = UpdAfterActRow.RSLT_END_DATEORTIME.Date

                    UpdAfterOdrDocumentRow.ROW_UPDATE_DATETIME = UpdAfterActRow.RSLT_END_DATEORTIME

                    UpdAfterOdrDocumentRow.ROW_UPDATE_ACCOUNT = account

                    UpdAfterOdrDocumentRow.ROW_UPDATE_FUNCTION = CONTENT_MODULEID

                End If
            Next

            If AfterOdrActId <> 0 Then

                '書類回収活動を更新した場合
                Logger.Info("UpdAfterOdrDocument　Call")
                SC3080216TableAdapter.UpdAfterOdrDocument(UpdAfterOdrDocumentTable)

            End If


            Logger.Info("RegistMySchedule　Call Start")
            'CalDAV連携処理呼び出し
            RegistMySchedule(input, actCalDavInfo, salesAfterFlg, AfterOdrRsltEndDateorTime)

            Logger.Info("RegistMySchedule　Call End")

            '受注後工程活動紐付更新
            UpdateLinkBookedafterActivityInfo(salesId, input)

            '受注後活動情報退避
            MoveHistoryAfterOdr(salesId, account, input, actCalDavInfo, AfterOdrRsltEndDateorTime)

        Catch ex As OracleExceptionEx

            ' メッセージIDを設定
            Dim messageId = DB_ERR_MSGID

            BizClass.Rollback = True
            Logger.Error(String.Format(CultureInfo.InvariantCulture, FormatOracleExceptionExLog, _
                    messageId), ex)
            Throw

        Finally
            ActIdTable.Dispose()
            AfterOdrProc.Dispose()
        End Try

        Logger.Info("UpdateBookedafterActivityInfo End")

        Return updCount

    End Function


    ''' <summary>
    ''' 受注後工程活動紐付更新
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <param name="input">更新対象</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Shared Function UpdateLinkBookedafterActivityInfo(ByVal salesId As Decimal, ByVal input As SC3080216DataSet.SC3080216UpdBookdafActiveDataTable) As Integer

        Logger.Info("UpdateLinkBookedafterActivityInfo Strat")

        Dim updCount As Integer = 0

        '受注後工程活動紐付け処理を行う
        Dim UpdAfterActRow As SC3080216DataSet.SC3080216UpdBookdafActiveRow = input.NewSC3080216UpdBookdafActiveRow

        UpdAfterActRow = input.Item(0)

        '完了フラグが完了済みの場合
        If UpdAfterActRow.AFTER_ODR_ACT_STATUS.Equals("1") Then

            UpdAfterActRow.RSLT_DATEORTIME_FLG = "1"

            '受注後の場合
            If UpdAfterActRow.ACT_ID.Equals("0") Then
                UpdAfterActRow.AFTER_ODR_FLLW_SEQ =
                    SC3080216TableAdapter.GetBookedAfterFollowSeqNoMax(UpdAfterActRow.RSLT_DLR_CD, salesId)
            End If
        End If

        updCount = updCount + SC3080216TableAdapter.UpdateLinkBookedafterActivityInfo(salesId, UpdAfterActRow, GetSysEnvSettingValue(ENVSETTINGKEY_AFTER_ODR_CONTRACT))

        Logger.Info("UpdateLinkBookedafterActivityInfo End")

        Return updCount


    End Function

    ''' <summary>
    ''' 受注後工程活動紐付更新(バックオフィス・受注時説明ツールのみ更新時)
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Shared Sub UpdateLinkBookedafterActivityInfoBp(ByVal salesId As Decimal)

        Logger.Info("UpdateLinkBookedafterActivityInfoBp Strat")

        Dim updCount As Integer = 0

        '受注後工程活動紐付け処理を行う
        Dim UpdAfterActRowTable As New SC3080216DataSet.SC3080216UpdBookdafActiveDataTable
        Dim UpdAfterActRow As SC3080216DataSet.SC3080216UpdBookdafActiveRow = UpdAfterActRowTable.NewSC3080216UpdBookdafActiveRow

        '受注後工程連番を設定する
        UpdAfterActRow.AFTER_ODR_FLLW_SEQ =
            SC3080216TableAdapter.GetBookedAfterFollowSeqNoMax(StaffContext.Current.DlrCD, salesId)

        UpdAfterActRow.ACT_ID = 0

        UpdAfterActRow.ROW_UPDATE_ACCOUNT = StaffContext.Current.Account
        UpdAfterActRow.ROW_UPDATE_FUNCTION = CONTENT_MODULEID

        updCount = updCount + SC3080216TableAdapter.UpdateLinkBookedafterActivityInfo(salesId, UpdAfterActRow, GetSysEnvSettingValue(ENVSETTINGKEY_AFTER_ODR_CONTRACT))


        Logger.Info("UpdateLinkBookedafterActivityInfoBp End")


    End Sub

    ''' <summary>
    ''' 受注後活動情報退避
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <param name="account">アカウント</param>
    ''' <returns>処理結果 成功 : True / 失敗 : False</returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Shared Function MoveHistoryAfterOdr(ByVal salesId As Decimal, ByVal account As String,
                                               ByVal input As SC3080216DataSet.SC3080216UpdBookdafActiveDataTable,
                                               ByVal actCalDavInfo As SC3080216BusinessLogic.CalDavDate,
                                               ByVal afterOdrRsltEndDateorTime As String) As Boolean

        Logger.Info("MoveHistoryAfterOdr Strat")

        '受注後工程必須活動未完了件数取得
        Dim flg As Decimal = SC3080216TableAdapter.GetCountMandatoryBookedAfterProcess(salesId)

        '未完了活動が1件以上の場合、処理終了
        If flg > 0 Then
            Return True
        End If

        'CalDAV連携
        RegistMySchedule(input, actCalDavInfo, CALDAV_AFTER_KBN, afterOdrRsltEndDateorTime)

        '2014/08/20 TCS 森 受注後活動A⇒H移行対応 START
        '受注後活動退避処理
        ActivityInfoBusinessLogic.MoveAfterOrderProcInfo(salesId, account, CONTENT_MODULEID)

        '2014/08/20 TCS 森 受注後活動A⇒H移行対応 END

        Logger.Info("MoveHistoryAfterOdr End")

        Return True

    End Function


    ''' <summary>
    ''' 活動コード取得
    ''' </summary>
    ''' <param name="sysEnvName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetSysEnvSettingValue(ByVal sysEnvName As String) As String
        Dim dr As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow = Nothing
        Dim env As SystemEnvSetting = Nothing

        env = New SystemEnvSetting()
        dr = env.GetSystemEnvSetting(sysEnvName)
        Dim ret As String = String.Empty

        If Not dr Is Nothing Then
            ret = dr.PARAMVALUE.Trim()
        End If

        Return ret
    End Function


    ''' <summary>
    ''' スケジュール登録
    ''' </summary>
    ''' <param name="input">更新データ</param>
    ''' <param name="actCalDavInfo">CalDAV連携情報</param>
    ''' <param name="salesAfterFlg">処理区分 0:受注時 1:受注後(基本活動未完了あり) 2:受注後(基本活動未完了なし)</param>
    ''' <param name="AfterOdrRsltEndDateorTime">活動実施終了日</param>
    ''' <remarks></remarks>
    Public Shared Sub RegistMySchedule(ByVal input As SC3080216DataSet.SC3080216UpdBookdafActiveDataTable,
                                 ByVal actCalDavInfo As SC3080216BusinessLogic.CalDavDate,
                                 ByVal salesAfterFlg As String,
                                 ByVal afterOdrRsltEndDateorTime As String)

        Logger.Info("RegistMySchedule Strat")
        Logger.Info("SC3080216 RegistMySchedule Start")

        'Commonタグ用クラス
        Dim calDavCommonTag As New iCROP.BizLogic.IC3040401.RegistAfterOrderCommon

        'ScheduleInfoタグ用クラス
        Dim calDavScheduleInfoTag As New iCROP.BizLogic.IC3040401.RegistAfterOrderScheduleInfo

        'Scheduleタグ用クラス
        Dim calDavScheduleTag As iCROP.BizLogic.IC3040401.RegistAfterOrderSchedule

        'Detailタグ用クラス
        Dim calDavDetailTag As New iCROP.BizLogic.IC3040401.RegistAfterOrderDetail

        'CalDAV連携引数用クラス
        Dim calDavOrder As New iCROP.BizLogic.IC3040401.RegistAfterOrder

        '受注後活動CalDAV用情報取得TBL
        Dim calDavAfterOdrProc As New SC3080216DataSet.SC3080216AfterOdrProcDataTable

        'コンタクト名取得TBL
        Dim calDavAfterOdrContactName As New SC3080216DataSet.SC3080216AfterOdrContactDataTable

        'ToDoチップ色取得TBL
        Dim calDavAfterOdrToDoColor As New SC3080216DataSet.SC3080216AfterOdrToDoColorDataTable

        'CalDAV連携URL取得用
        Dim dlrenvdt As New DealerEnvSetting
        Dim dlrenvrw As DlrEnvSettingDataSet.DLRENVSETTINGRow


        '処理区分に応じてデータを設定し、CalDAV連携をCallする
        If salesAfterFlg.Equals("0") Then

            Logger.Info("CalDAV SC3080216　salesAfterFlg = 0　Set Param")
            '処理区分が'0'(受注時)
            'Commonタグに情報を設定する
            Logger.Info("CalDAV　salesAfterFlg = 0 CommonTag Set Param Start")
            '販売店コード
            calDavCommonTag.DealerCode = actCalDavInfo.DealerCode
            Logger.Info("CalDAV　salesAfterFlg = 0 CommonTag　Set DealerCode:" + calDavCommonTag.DealerCode)

            '店舗コード
            calDavCommonTag.BranchCode = actCalDavInfo.BranchCode
            Logger.Info("CalDAV　salesAfterFlg = 0 CommonTag　Set BranchCode:" + calDavCommonTag.BranchCode)

            'スケジュールID
            calDavCommonTag.ScheduleId = CType(actCalDavInfo.SalesId, String)
            Logger.Info("CalDAV　salesAfterFlg = 0 CommonTag　Set ScheduleId(SalesId):" + calDavCommonTag.ScheduleId)

            '処理区分
            calDavCommonTag.ActionType = CALDAV_TODO_INS
            Logger.Info("CalDAV　salesAfterFlg = 0 CommonTag　Set ActionType:" + calDavCommonTag.ActionType)

            '活動作成スタッフコード
            calDavCommonTag.ActivityCreateStaff = actCalDavInfo.StaffCode
            Logger.Info("CalDAV　salesAfterFlg = 0 CommonTag　Set ActivityCreateStaff:" + calDavCommonTag.ActivityCreateStaff)

            Logger.Info("CalDAV　salesAfterFlg = 0 CommonTag Set Param End")

            'ScheduleInfoタグに情報を設定する
            Logger.Info("CalDAV　salesAfterFlg = 0 ScheduleInfoTag Set Param Start")

            '顧客区分
            If String.Equals(actCalDavInfo.CustomerDiv, "1") Then
                actCalDavInfo.CustomerDiv = "0"
            Else
                actCalDavInfo.CustomerDiv = "2"
            End If
            calDavScheduleInfoTag.CustomerDiv = actCalDavInfo.CustomerDiv
            Logger.Info("CalDAV　salesAfterFlg = 0 ScheduleInfoTag　Set CustomerDiv:" + calDavScheduleInfoTag.CustomerDiv)

            '顧客コード
            calDavScheduleInfoTag.CustomerCode = actCalDavInfo.CustomerCode
            Logger.Info("CalDAV　salesAfterFlg = 0 ScheduleInfoTag　Set CustomerCode:" + calDavScheduleInfoTag.CustomerCode)

            '顧客名称情報を取得する
            Dim calDavCstNameInfo As SC3080216DataSet.SC3080216AfterOdrCstInfoDataTable =
                SC3080216TableAdapter.GetCstName(CType(actCalDavInfo.CustomerCode, Decimal))

            If calDavCstNameInfo.Count <> 0 Then

                'DMSID
                calDavScheduleInfoTag.DmsId = calDavCstNameInfo.Item(0).DMS_CST_CD
                Logger.Info("CalDAV　salesAfterFlg = 0 ScheduleInfotag　Set DMSID:" + calDavScheduleInfoTag.DmsId)

                '顧客名称
                calDavScheduleInfoTag.CustomerName = calDavCstNameInfo.Item(0).CST_NAME
                Logger.Info("CalDAV　salesAfterFlg = 0 ScheduleInfoTag　Set CustomerName:" + calDavScheduleInfoTag.CustomerName)

            Else
                'DMSID
                calDavScheduleInfoTag.DmsId = ""
                Logger.Info("CalDAV　salesAfterFlg = 0 ScheduleInfotag　Set DMSID(Blank)" + calDavScheduleInfoTag.DmsId)

                '顧客名称
                calDavScheduleInfoTag.CustomerName = ""
                Logger.Info("CalDAV　salesAfterFlg = 0 ScheduleInfoTag　Set CustomerName(Blank)" + calDavScheduleInfoTag.CustomerName)

            End If
            Logger.Info("CalDAV　salesAfterFlg = 0 ScheduleInfoTag Set Param End")

            'Scheduleタグに情報を設定する
            Logger.Info("CalDAV　salesAfterFlg = 0 ScheduleTag Set Param Start")

            '未完了の受注後活動を取得する
            Dim calDavAfterOdrActList As SC3080216DataSet.SC3080216AfterOdrActCalDAVDataTable =
                SC3080216TableAdapter.GetAfterActCalDav(actCalDavInfo.SalesId)


            For Each calDavAfterOdrAct As SC3080216DataSet.SC3080216AfterOdrActCalDAVRow In calDavAfterOdrActList

                '受注後活動CalDAV用情報取得
                calDavAfterOdrProc = SC3080216TableAdapter.GetAfterOrderProc(calDavAfterOdrAct.AFTER_ODR_ACT_ID)

                Logger.Info("CalDAV　salesAfterFlg = 0 ScheduleTag GetAfterOrderProc")

                '2014/12/25 TCS 外崎 車両ステイタス連携機能各国展開に向けた追加機能開発 START
                If Not ((calDavAfterOdrProc.Item(0).SC_RSLT_INPUT_FLG = "1") OrElse ((calDavAfterOdrProc.Item(0).STD_VOLUNTARYINS_ACT_TYPE = "2") Or (calDavAfterOdrProc.Item(0).STD_VOLUNTARYINS_ACT_TYPE = "3"))) Then
                    'SC実績入力区分が「1:SCによる実績登録可」もしくは
                    '受注後基本任意活動区分が「2:任意活動、3:事前フォロー」の場合のみ、TODOチップを作成する
                    Continue For
                End If
                '2014/12/25 TCS 外崎 車両ステイタス連携機能各国展開に向けた追加機能開発 END

                'コンタクト名取得
                calDavAfterOdrContactName = SC3080216TableAdapter.GetContactName(calDavAfterOdrProc.Item(0).SCHE_CONTACT_MTD)

                Logger.Info("CalDAV　salesAfterFlg = 0 ScheduleTag calDavAfterOdrContactName")

                'ToDoチップ色取得
                calDavAfterOdrToDoColor = SC3080216TableAdapter.GetToDoColor(calDavAfterOdrProc.Item(0).AFTER_ODR_PRCS_CD)

                Logger.Info("CalDAV　salesAfterFlg = 0 ScheduleTag calDavAfterOdrToDoColor")

                'Scheduleタグ生成
                calDavScheduleTag = New iCROP.BizLogic.IC3040401.RegistAfterOrderSchedule

                Logger.Info("CalDAV　salesAfterFlg = 0 ScheduleTag Create")

                'スケジュール作成区分
                '接触方法が来店の場合
                'ToDoと合わせてスケジュールの登録も行う
                If calDavAfterOdrProc.Item(0).SCHE_CONTACT_MTD.Equals(CONTACT_MTD_WALK_IN) Then

                    calDavScheduleTag.CreateScheduleDiv = CALDAV_SCHEDULE_TODOEVENT

                Else

                    calDavScheduleTag.CreateScheduleDiv = CALDAV_SCHEDULE_TODO

                End If
                Logger.Info("CalDAV　salesAfterFlg = 0 ScheduleTag CreateScheduleDiv:" + calDavScheduleTag.CreateScheduleDiv)

                '活動担当スタッフ店舗コード
                calDavScheduleTag.ActivityStaffBranchCode = calDavAfterOdrProc.Item(0).SCHE_BRN_CD
                Logger.Info("CalDAV　salesAfterFlg = 0 ScheduleTag ActivityStaffBranchCode:" + calDavScheduleTag.ActivityStaffBranchCode)

                '活動スタッフコード
                calDavScheduleTag.ActivityStaffCode = calDavAfterOdrProc.Item(0).SCHE_STF_CD
                Logger.Info("CalDAV　salesAfterFlg = 0 ScheduleTag ActivityStaffCode:" + calDavScheduleTag.ActivityStaffCode)

                '受付担当スタッフ店舗コード
                calDavScheduleTag.ReceptionStaffBranchCode = ""
                Logger.Info("CalDAV　salesAfterFlg = 0 ScheduleTag ReceptionStaffBranchCode(Blank)" + calDavScheduleTag.ReceptionStaffBranchCode)

                '受付担当スタッフコード
                calDavScheduleTag.ReceptionStaffCode = ""
                Logger.Info("CalDAV　salesAfterFlg = 0 ScheduleTag ReceptionStaffCode(Blank)" + calDavScheduleTag.ReceptionStaffCode)

                '接触方法No.
                calDavScheduleTag.ContactNo = calDavAfterOdrProc.Item(0).SCHE_CONTACT_MTD
                Logger.Info("CalDAV　salesAfterFlg = 0 ScheduleTag ContactNo:" + calDavScheduleTag.ContactNo)

                '接触方法名
                calDavScheduleTag.ContactName = calDavAfterOdrContactName.Item(0).CONTACT_NAME
                Logger.Info("CalDAV　salesAfterFlg = 0 ScheduleTag ContactName:" + calDavScheduleTag.ContactName)

                '受注後活動名称
                calDavScheduleTag.ActOdrName = calDavAfterOdrProc.Item(0).AFTER_ODR_ACT_NAME
                Logger.Info("CalDAV　salesAfterFlg = 0 ScheduleTag ActOdrName:" + calDavScheduleTag.ActOdrName)

                'タイトル
                calDavScheduleTag.Summary = calDavCstNameInfo.Item(0).CST_NAME + " " +
                    calDavAfterOdrContactName.Item(0).CONTACT_NAME + " (" +
                    calDavAfterOdrProc.Item(0).AFTER_ODR_ACT_NAME + ") "

                Logger.Info("CalDAV　salesAfterFlg = 0 ScheduleTag Summary:" + calDavScheduleTag.Summary)

                '開始日時
                '書式を指定してから代入する必要あり
                If calDavAfterOdrProc.Item(0).SCHE_DATEORTIME_FLG.Equals("1") Then
                    '開始日時
                    calDavScheduleTag.StartTime = Format(calDavAfterOdrProc.Item(0).SCHE_START_DATEORTIME, "yyyy/MM/dd HH:mm:ss")
                    Logger.Info("CalDAV　salesAfterFlg = 0 ScheduleTag Set StartTime:" + calDavScheduleTag.StartTime)

                    '終了日時
                    '開始日時と同じ
                    calDavScheduleTag.EndTime = Format(calDavAfterOdrProc.Item(0).SCHE_END_DATEORTIME, "yyyy/MM/dd HH:mm:ss")
                    Logger.Info("CalDAV　salesAfterFlg = 0 ScheduleTag Set EndTime:" + calDavScheduleTag.EndTime)

                Else
                    '開始日時
                    calDavScheduleTag.StartTime = Format(calDavAfterOdrProc.Item(0).SCHE_START_DATEORTIME, "yyyy/MM/dd")
                    Logger.Info("CalDAV　salesAfterFlg = 0 ScheduleTag Set Blank StartTime" + calDavScheduleTag.StartTime)

                    '終了日時
                    '開始日時と同じ
                    calDavScheduleTag.EndTime = Format(calDavAfterOdrProc.Item(0).SCHE_END_DATEORTIME, "yyyy/MM/dd")
                    Logger.Info("CalDAV　salesAfterFlg = 0 ScheduleTag Set Blank EndTime" + calDavScheduleTag.EndTime)

                End If

                '説明(メモ)
                calDavScheduleTag.Memo = ""
                Logger.Info("CalDAV　salesAfterFlg = 0 ScheduleTag Memo(Blank)" + calDavScheduleTag.Memo)

                '色設定
                calDavScheduleTag.XIcropColor = calDavAfterOdrToDoColor.Item(0).BACKGROUNDCOLOR
                Logger.Info("CalDAV　salesAfterFlg = 0 ScheduleTag XIcropColor:" + calDavScheduleTag.XIcropColor)

                '受注区分
                If calDavAfterOdrProc.Item(0).AFTER_ODR_PRCS_TYPE.Equals("0") Then

                    '納車前工程
                    calDavScheduleTag.OdrDiv = "1"
                    Logger.Info("CalDAV　salesAfterFlg = 0 Scheduletag OdrDiv:" + calDavScheduleTag.OdrDiv)
                Else

                    '納車後工程
                    calDavScheduleTag.OdrDiv = "2"
                    Logger.Info("CalDAV　salesAfterFlg = 0 ScheduleTag OdrDiv:" + calDavScheduleTag.OdrDiv)
                End If

                '受注後活動ID
                calDavScheduleTag.AfterOdrActId = CType(calDavAfterOdrAct.AFTER_ODR_ACT_ID, String)
                Logger.Info("CalDAV　salesAfterFlg = 0 ScheduleTag AfterOdrActId:" + calDavScheduleTag.AfterOdrActId)

                'ToDoID
                calDavScheduleTag.TodoId = ""
                Logger.Info("CalDAV　salesAfterFlg = 0 ScheduleTag ToDoID(Blank)" + calDavScheduleTag.TodoId)

                '工程区分
                calDavScheduleTag.ProcessDiv = calDavAfterOdrProc.Item(0).AFTER_ODR_PRCS_CD
                Logger.Info("CalDAV　salesAfterFlg = 0 ScheduleTag ProcessDiv:" + calDavScheduleTag.ProcessDiv)

                'ScheduleListにScheduleタグ情報を追加する
                calDavDetailTag.ScheduleList.Add(calDavScheduleTag)
            Next

        ElseIf salesAfterFlg.Equals("1") Then

            '処理区分が'1'(受注後:基本活動未完了あり)
            Logger.Info("CalDAV　salesAfterFlg = 1 Set Param")

            'Commonタグに情報を設定する
            Logger.Info("CalDAV　salesAfterFlg = 1 CommonTag Set Param")
            '販売店コード
            calDavCommonTag.DealerCode = actCalDavInfo.DealerCode
            Logger.Info("CalDAV　salesAfterFlg = 1 CommonTag DealerCode:" + calDavCommonTag.DealerCode)

            '店舗コード
            calDavCommonTag.BranchCode = actCalDavInfo.BranchCode
            Logger.Info("CalDAV　salesAfterFlg = 1 CommonTag BranchCode:" + calDavCommonTag.BranchCode)

            'スケジュールID
            calDavCommonTag.ScheduleId = CType(actCalDavInfo.SalesId, String)
            Logger.Info("CalDAV　salesAfterFlg = 1 CommonTag ScheduleId:" + calDavCommonTag.ScheduleId)

            '処理区分
            calDavCommonTag.ActionType = CALDAV_TODO_UPD
            Logger.Info("CalDAV　salesAfterFlg = 1 CommonTag ActionType:" + calDavCommonTag.ActionType)

            '活動作成スタッフコード
            calDavCommonTag.ActivityCreateStaff = actCalDavInfo.StaffCode
            Logger.Info("CalDAV　salesAfterFlg = 1 CommonTag ActivityCreateStaff:" + calDavCommonTag.ActivityCreateStaff)

            'ScheduleInfoタグに情報を設定する
            Logger.Info("CalDAV　salesAfterFlg = 1 ScheduleInfoTag Set Param")
            '顧客区分
            If String.Equals(actCalDavInfo.CustomerDiv, "1") Then
                actCalDavInfo.CustomerDiv = "0"
            Else
                actCalDavInfo.CustomerDiv = "2"
            End If
            calDavScheduleInfoTag.CustomerDiv = actCalDavInfo.CustomerDiv
            Logger.Info("CalDAV　salesAfterFlg = 1 ScheduleInfoTag CustomerDiv:" + calDavScheduleInfoTag.CustomerDiv)

            '顧客コード
            calDavScheduleInfoTag.CustomerCode = actCalDavInfo.CustomerCode
            Logger.Info("CalDAV　salesAfterFlg = 1 ScheduleInfoTag CustomerCode:" + calDavScheduleInfoTag.CustomerCode)

            '更新データの件数分、Scheduleタグにデータを設定する
            For Each setData As SC3080216DataSet.SC3080216UpdBookdafActiveRow In input

                '受注後活動CalDAV用情報取得
                calDavAfterOdrProc = SC3080216TableAdapter.GetAfterOrderProc(setData.AFTER_ODR_ACT_ID)
                Logger.Info("CalDAV　salesAfterFlg = 1 ScheduleInfoTag GetAfterOrderProc")

                '2014/12/25 TCS 外崎 車両ステイタス連携機能各国展開に向けた追加機能開発 START
                '2019/01/25 TCS 中村(拓) TKM-UAT-0644 START
                If Not ((calDavAfterOdrProc.Item(0).SC_RSLT_INPUT_FLG = "1") OrElse ((calDavAfterOdrProc.Item(0).STD_VOLUNTARYINS_ACT_TYPE = "2") Or (calDavAfterOdrProc.Item(0).STD_VOLUNTARYINS_ACT_TYPE = "3"))) Then
                    '2019/01/25 TCS 中村(拓) TKM-UAT-0644 END
                    'SCによる実績登録可能の受注後活動についてのみ、TODOチップを作成する
                    Continue For
                End If
                '2014/12/25 TCS 外崎 車両ステイタス連携機能各国展開に向けた追加機能開発 END

                'Scheduleタグ生成
                calDavScheduleTag = New iCROP.BizLogic.IC3040401.RegistAfterOrderSchedule
                Logger.Info("CalDAV　salesAfterFlg = 1 ScheduleTag Set Param")

                'スケジュール作成区分
                calDavScheduleTag.CreateScheduleDiv = CALDAV_SCHEDULE_TODO
                Logger.Info("CalDAV　salesAfterFlg = 1 ScheduleTag CreateScheduleDiv:" + calDavScheduleTag.CreateScheduleDiv)

                '受注区分
                If calDavAfterOdrProc.Item(0).AFTER_ODR_PRCS_TYPE.Equals("0") Then

                    '納車前工程
                    calDavScheduleTag.OdrDiv = "1"
                    Logger.Info("CalDAV　salesAfterFlg = 1 ScheduleTag OdrDiv:" + calDavScheduleTag.OdrDiv)
                Else

                    '納車後工程
                    calDavScheduleTag.OdrDiv = "2"
                    Logger.Info("CalDAV　salesAfterFlg = 1 ScheduleTag OdrDiv:" + calDavScheduleTag.OdrDiv)
                End If

                '受注後活動ID
                calDavScheduleTag.AfterOdrActId = CType(setData.AFTER_ODR_ACT_ID, String)
                Logger.Info("CalDAV　salesAfterFlg = 1 ScheduleTag AfterOdrActId:" + calDavScheduleTag.AfterOdrActId)

                '工程区分
                calDavScheduleTag.ProcessDiv = calDavAfterOdrProc.Item(0).AFTER_ODR_PRCS_CD
                Logger.Info("CalDAV　salesAfterFlg = 1 ScheduleTag ProcessDiv:" + calDavScheduleTag.ProcessDiv)

                '実績日
                '活動が完了しているものは実施終了日時を
                '活動を未完了に戻すものは初期値(1900/01/01 00:00:00)を設定する
                If setData.AFTER_ODR_ACT_STATUS.Equals("1") Then
                    '時間指定あり・なしでフォーマットを変更する
                    If setData.RSLT_DATEORTIME_FLG.Equals("1") Then
                        calDavScheduleTag.ResultDate = Format(setData.RSLT_END_DATEORTIME, "yyyy/MM/dd HH:mm:ss")
                        Logger.Info("CalDAV　salesAfterFlg = 1 ScheduleTag RSLT_DATEORTIME_FLG = 1  ResultDate:" + calDavScheduleTag.ResultDate)

                    Else
                        calDavScheduleTag.ResultDate = Format(setData.RSLT_END_DATEORTIME, "yyyy/MM/dd")
                        Logger.Info("CalDAV　salesAfterFlg = 1 ScheduleTag RSLT_DATEORTIME_FLG = 0  ResultDate:" + calDavScheduleTag.ResultDate)
                    End If

                Else
                    calDavScheduleTag.ResultDate = INIT_DATATIME
                    Logger.Info("CalDAV　salesAfterFlg = 1 ScheduleTag Reset ResultDate:" + calDavScheduleTag.ResultDate)
                End If

                'ScheduleListにScheduleタグ情報を追加する
                calDavDetailTag.ScheduleList.Add(calDavScheduleTag)
            Next

        Else

            '処理区分が'2'(受注後:基本活動未完了なし)
            Logger.Info("CalDAV　salesAfterFlg = 2 Set Param")

            'Commonタグに情報を設定する
            Logger.Info("CalDAV　salesAfterFlg = 2 CommonTag Set Param")
            '販売店コード
            calDavCommonTag.DealerCode = actCalDavInfo.DealerCode
            Logger.Info("CalDAV　salesAfterFlg = 2 CommonTag DealerCode:" + calDavCommonTag.DealerCode)

            '店舗コード
            calDavCommonTag.BranchCode = actCalDavInfo.BranchCode
            Logger.Info("CalDAV　salesAfterFlg = 2 CommonTag BranchCode:" + calDavCommonTag.BranchCode)

            'スケジュールID
            calDavCommonTag.ScheduleId = CType(actCalDavInfo.SalesId, String)
            Logger.Info("CalDAV　salesAfterFlg = 2 CommonTag ScheduleId:" + calDavCommonTag.ScheduleId)

            '処理区分
            calDavCommonTag.ActionType = CALDAV_TODO_DEL
            Logger.Info("CalDAV　salesAfterFlg = 2 CommonTag ActionType:" + calDavCommonTag.ActionType)

            '活動作成スタッフコード
            calDavCommonTag.ActivityCreateStaff = actCalDavInfo.StaffCode
            Logger.Info("CalDAV　salesAfterFlg = 2 CommonTag ActivityCreateStaff:" + calDavCommonTag.ActivityCreateStaff)

            'ScheduleInfoタグに削除日を設定する
            calDavScheduleInfoTag.DeleteDate = afterOdrRsltEndDateorTime

        End If

        'CalDAV連携実施
        'Commonタグ設定
        calDavDetailTag.Common = calDavCommonTag
        Logger.Info("CalDAV Set CommonTag")

        'ScheduleInfoタグ設定
        calDavDetailTag.ScheduleInfo = calDavScheduleInfoTag
        Logger.Info("CalDAV Set ScheduleInfoTag")

        'Detailタグ設定
        calDavOrder.DetailList.Add(calDavDetailTag)
        Logger.Info("CalDAV Set DetailTag")

        'CalDAVWeb参照URL取得
        dlrenvrw = dlrenvdt.GetEnvSetting("XXXXX", C_CALDAV_WEBSERVICE_URL)
        Logger.Info("CalDAV Get URL:" + dlrenvrw.PARAMVALUE)

        Dim errCd As String



        'CalDAV連携実施
        Using calDavBiz As New iCROP.BizLogic.IC3040401.IC3040401BusinessLogic

            Logger.Info("SendAfterProcessScheduleInfo Call Start")
            errCd = calDavBiz.SendAfterProcessScheduleInfo(dlrenvrw.PARAMVALUE, calDavOrder)
            Logger.Info("SendAfterProcessScheduleInfo Call End")
            Logger.Info("SendAfterProcessScheduleInfo errCd: " + errCd)

        End Using

        If String.Equals(errCd, "0") = False Then
            'エラー処理
            Logger.Error("SC3080216BusinessLogic.RegistMySchedule - Internal Error: RegistMySchedule (return:" & errCd & ")")

            Logger.Info("CalDAV Error")
        End If

        calDavAfterOdrProc.Dispose()
        calDavAfterOdrContactName.Dispose()
        calDavAfterOdrToDoColor.Dispose()

        Logger.Info("CalDAV End")

        Logger.Info("RegistMySchedule End")

    End Sub

    ''' <summary>
    ''' 成約車種SeqNo取得
    ''' </summary>
    ''' <param name="fllwupboxSeqno"></param>
    ''' <returns>成約車種SeqNo</returns>
    ''' <remarks></remarks>
    Public Shared Function GetSuccessSeriesSeqno(ByVal fllwupboxSeqno As Decimal) As Long
        ' 成約車種取得
        Dim datatableSelectedSeries As ActivityInfoDataSet.ActivityInfoGetSelectedSeriesDataTable

        datatableSelectedSeries =
            ActivityInfoTableAdapter.GetSuccessSeries(fllwupboxSeqno)

        If (datatableSelectedSeries.Rows.Count > 0) Then
            If (datatableSelectedSeries.Item(0).IsSEQNONull) Then
                Return 0
            Else
                Return datatableSelectedSeries.Item(0).SEQNO
            End If
        End If

        Return 0

    End Function

End Class
