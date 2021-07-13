'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'IC3040401BusinessLogic.vb
'─────────────────────────────────────
'機能： CalDAV連携バッチ
'補足： 
'作成： 2011/12/01 KN 田中
'更新： 2012/02/14 KN 田中 【SALES_1A】号口(課題No.51)対応
'       2012/02/14 KN 田中 GTMC130118132対応
'       2014/04/25 TMEJ 水本 受注後フォロー機能開発（スタッフ活動KPI）に向けたシステム設計
'─────────────────────────────────────
Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Xml
Imports System.IO
Imports System.Configuration
Imports System.Globalization
Imports System.Text.RegularExpressions
Imports System.Web
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports System.Xml.XPath


Namespace Toyota.eCRB.iCROP.BizLogic.IC3040401

    ''' <summary>
    ''' SC30404011(CalDAV連携)
    ''' 連携で使用するビジネスロジック
    ''' </summary>
    ''' <remarks></remarks>
    Public Class IC3040401BusinessLogic
        Implements IDisposable

        Private MyMessage As String  'メッセージ詳細

        '最終送信Xml
        Private lastSendXmlValue As String

        Private SendDs As RegistSchedule

        '入力プロパティ(共通情報)を格納するデータテーブル
        Private P_CommonDt As RegistSchedule.P_CommonDataTable
        Private P_CommonDr As RegistSchedule.P_CommonRow

        '入力プロパティ(スケジュール情報)を格納する
        Private P_ScheduleInfoDt As RegistSchedule.P_ScheduleDataTable

#Region "定数"
        '顧客区分・副顧客
        Private Const CUSTOMER_DIV_SUB As String = "1"

        'メッセージID
        Private Const MESSAGE_ID = "IC3040403"

        'SYSTEM識別コード
        Private Const LINK_SYSTEM_CODE As String = "0"

        '予約フォローレコード定数(予約レコードは2レコード目と決まっている)
        Private Const CREATE_FOLLOW_COUNT As Integer = 2

        '来店予約
        Private Const RSV_RAITEN As String = "0"

        '入庫予約
        Private Const RSV_NYUKO As String = "1"

        '受注後工程
        Private Const RSV_SALESBKGTALLY As String = "2"

        '処理区分：新規登録
        Private Const ACTION_TYPE_INSERT As String = "1"

        '処理区分：登録
        Private Const ACTION_TYPE_UPDATE As String = "2"

        '処理区分：イベント追加
        Private Const ACTION_TYPE_INSERT_EVENT As String = "3"

        ' 2014/04/25 TMEJ 水本 受注後フォロー機能開発（スタッフ活動KPI）に向けたシステム設計 START
        '受注区分：受注前
        Private Const ODR_DIV_BEFORE As String = "0"
        ' 2014/04/25 TMEJ 水本 受注後フォロー機能開発（スタッフ活動KPI）に向けたシステム設計 END

#End Region


#Region "getter/setter"
        ''' <summary>
        ''' Commonタグの処理区分に対してのアクセッサ
        ''' </summary>
        ''' <value>処理区分</value>
        Public Property ActionType() As String
            Get
                Return P_CommonDr.Item("ActionType").ToString()
            End Get
            Set(ByVal Value As String)
                P_CommonDr.Item("ActionType") = Value
            End Set
        End Property


        ''' <summary>
        ''' Commonタグの販売店コードに対してのアクセッサ
        ''' </summary>
        ''' <value>販売店コード</value>
        Public Property DealerCode() As String
            Get
                Return P_CommonDr.Item("DealerCode").ToString()
            End Get
            Set(ByVal Value As String)
                P_CommonDr.Item("DealerCode") = Value
            End Set
        End Property

        ''' <summary>
        ''' Commonタグの店舗コードに対してのアクセッサ
        ''' </summary>
        ''' <value>店舗コード</value>
        Public Property BranchCode() As String
            Get
                Return P_CommonDr.Item("BranchCode").ToString()
            End Get
            Set(ByVal Value As String)
                P_CommonDr.Item("BranchCode") = Value
            End Set
        End Property

        ''' <summary>
        ''' Commonタグのスケジュール区分に対してのアクセッサ
        ''' </summary>
        ''' <value>スケジュール区分</value>
        Public Property ScheduleDivision() As String
            Get
                Return P_CommonDr.Item("ScheduleDivision").ToString()
            End Get
            Set(ByVal Value As String)
                P_CommonDr.Item("ScheduleDivision") = Value
            End Set
        End Property

        ''' <summary>
        ''' CommonタグのスケジュールIDに対してのアクセッサ
        ''' </summary>
        ''' <value>スケジュールID</value>
        Public Property ScheduleId() As String
            Get
                Return P_CommonDr.Item("ScheduleId").ToString()
            End Get
            Set(ByVal Value As String)
                P_CommonDr.Item("ScheduleId") = Value
            End Set
        End Property

        ''' <summary>
        ''' Commonタグの活動作成スタッフコードに対してのアクセッサ
        ''' </summary>
        ''' <value>活動作成スタッフコード</value>
        Public Property ActivityCreateStaffCode() As String
            Get
                Return P_CommonDr.Item("ActivityCreateStaffCode").ToString()
            End Get
            Set(ByVal Value As String)
                P_CommonDr.Item("ActivityCreateStaffCode") = Value
            End Set
        End Property


        ''' <summary>
        ''' Commonタグの完了フラグに対してのアクセッサ
        ''' </summary>
        ''' <value>終了日時</value>
        Public Property CompleteFlg As String
            Get
                Return P_CommonDr.Item("CompleteFlg").ToString()
            End Get
            Set(ByVal Value As String)
                P_CommonDr.Item("CompleteFlg") = Value
            End Set
        End Property

        ''' <summary>
        ''' Commonタグの完了日に対してのアクセッサ
        ''' </summary>
        ''' <value>完了日</value>
        Public Property CompletionDate() As String
            Get
                Return P_CommonDr.Item("CompletionDate").ToString()
            End Get
            Set(ByVal Value As String)
                P_CommonDr.Item("CompletionDate") = Value
            End Set
        End Property

        ''' <summary>
        ''' Commonタグの顧客区分に対してのアクセッサ
        ''' </summary>
        ''' <value>顧客区分</value>
        Public Property CustomerDivision() As String
            Get
                Return P_CommonDr.Item("CustomerDivision").ToString()
            End Get
            Set(ByVal Value As String)
                P_CommonDr.Item("CustomerDivision") = Value
            End Set
        End Property

        ''' <summary>
        ''' Commonタグの顧客コードに対してのアクセッサ
        ''' </summary>
        ''' <value>顧客コード</value>
        Public Property CustomerId() As String
            Get
                Return P_CommonDr.Item("CustomerId").ToString()
            End Get
            Set(ByVal Value As String)
                P_CommonDr.Item("CustomerId") = Value
            End Set
        End Property

        ''' <summary>
        ''' Commonタグの顧客名に対してのアクセッサ
        ''' </summary>
        ''' <value>顧客名</value>
        Public Property CustomerName() As String
            Get
                Return P_CommonDr.Item("CustomerName").ToString()
            End Get
            Set(ByVal Value As String)
                P_CommonDr.Item("CustomerName") = Value
            End Set
        End Property

        ''' <summary>
        ''' Commonタグの敬称名称に対してのアクセッサ
        ''' </summary>
        ''' <value>敬称</value>
        Public Property NameTitle() As String
            Get
                Return P_CommonDr.Item("NameTitle").ToString()
            End Get
            Set(ByVal Value As String)
                P_CommonDr.Item("NameTitle") = Value
            End Set
        End Property


        ''' <summary>
        ''' commonタグの敬称表示位置に対してのアクセッサ
        ''' </summary>
        ''' <value>敬称表示位置</value>
        Public Property NameTitlePosition() As String
            Get
                Return P_CommonDr.Item("NameTitlePosition").ToString()
            End Get
            Set(ByVal Value As String)
                P_CommonDr.Item("NameTitlePosition") = Value
            End Set
        End Property

        ''' <summary>
        ''' CommonタグのDMSIDに対してのアクセッサ
        ''' </summary>
        ''' <value>顧客コード</value>
        Public Property DmsId() As String
            Get
                Return P_CommonDr.Item("DmsId").ToString()
            End Get
            Set(ByVal Value As String)
                P_CommonDr.Item("DmsId") = Value
            End Set
        End Property

        ''' <summary>
        ''' Commonタグの受付納車区分に対してのアクセッサ
        ''' </summary>
        ''' <value>受付納車区分</value>
        Public Property ReceptionDivision() As String
            Get
                Return P_CommonDr.Item("ReceptionDivision").ToString()
            End Get
            Set(ByVal Value As String)
                P_CommonDr.Item("ReceptionDivision") = Value
            End Set
        End Property

        ''' <summary>
        ''' Commonタグのサービスコードに対してのアクセッサ
        ''' </summary>
        ''' <value>サービスコード</value>
        Public Property ServiceCode() As String
            Get
                Return P_CommonDr.Item("ServiceCode").ToString()
            End Get
            Set(ByVal Value As String)
                P_CommonDr.Item("ServiceCode") = Value
            End Set
        End Property

        ''' <summary>
        ''' Commonタグのサービス名に対してのアクセッサ
        ''' </summary>
        ''' <value>サービス名</value>
        Public Property ServiceName() As String
            Get
                Return P_CommonDr.Item("ServiceName").ToString()
            End Get
            Set(ByVal Value As String)
                P_CommonDr.Item("ServiceName") = Value
            End Set
        End Property

        ''' <summary>
        ''' Commonタグの商品コードに対してのアクセッサ
        ''' </summary>
        ''' <value>商品コード</value>
        Public Property MerchandiseCode() As String
            Get
                Return P_CommonDr.Item("MerchandiseCode").ToString()
            End Get
            Set(ByVal Value As String)
                P_CommonDr.Item("MerchandiseCode") = Value
            End Set
        End Property

        ''' <summary>
        ''' Commonタグの商品名に対してのアクセッサ
        ''' </summary>
        ''' <value>商品名</value>
        Public Property MerchandiseName() As String
            Get
                Return P_CommonDr.Item("MerchandiseName").ToString()
            End Get
            Set(ByVal Value As String)
                P_CommonDr.Item("MerchandiseName") = Value
            End Set
        End Property


        ''' <summary>
        ''' Commonタグの入庫ステータスに対してのアクセッサ
        ''' </summary>
        ''' <value>入庫ステータス</value>
        Public Property StoreStatus() As String
            Get
                Return P_CommonDr.Item("StoreStatus").ToString()
            End Get
            Set(ByVal Value As String)
                P_CommonDr.Item("StoreStatus") = Value
            End Set
        End Property

        ''' <summary>
        ''' Commonタグの予約ステータスに対してのアクセッサ
        ''' </summary>
        ''' <value>予約ステータス</value>
        Public Property ReservationStatus() As String
            Get
                Return P_CommonDr.Item("ReservationStatus").ToString()
            End Get
            Set(ByVal Value As String)
                P_CommonDr.Item("ReservationStatus") = Value
            End Set
        End Property

        ''' <summary>
        ''' Commonタグの予約ステータス名に対してのアクセッサ
        ''' </summary>
        ''' <value>予約ステータス</value>
        Public Property ReservationStatusName() As String
            Get
                Return P_CommonDr.Item("ReservationStatusName").ToString()
            End Get
            Set(ByVal Value As String)
                P_CommonDr.Item("ReservationStatusName") = Value
            End Set
        End Property


        ''' <summary>
        ''' ScheduleInfoタグの活動担当スタッフ店舗コードに対してのアクセッサ
        ''' </summary>
        ''' <value>活動担当スタッフ店舗コード</value>
        Public Property ActivityStaffBranchCode(ByVal index As Integer) As String
            Get
                Return P_ScheduleInfoDt.Rows(index).Item("ActivityStaffBranchCode").ToString()
            End Get
            Set(ByVal Value As String)
                P_ScheduleInfoDt.Rows(index).Item("ActivityStaffBranchCode") = Value
            End Set
        End Property


        ''' <summary>
        ''' Scheduleタグの活動担当スタッフコードに対してのアクセッサ
        ''' </summary>
        ''' <value>セールススタッフコード</value>
        Public Property ActivityStaffCode(ByVal index As Integer) As String
            Get
                Return P_ScheduleInfoDt.Rows(index).Item("ActivityStaffCode").ToString()
            End Get
            Set(ByVal Value As String)
                P_ScheduleInfoDt.Rows(index).Item("ActivityStaffCode") = Value
            End Set
        End Property

        ''' <summary>
        ''' ScheduleInfoタグの受付担当スタッフ店舗コードに対してのアクセッサ
        ''' </summary>
        ''' <value>活動担当スタッフ店舗コード</value>
        Public Property ReceptionStaffBranchCode(ByVal index As Integer) As String
            Get
                Return P_ScheduleInfoDt.Rows(index).Item("ReceptionStaffBranchCode").ToString()
            End Get
            Set(ByVal Value As String)
                P_ScheduleInfoDt.Rows(index).Item("ReceptionStaffBranchCode") = Value
            End Set
        End Property


        ''' <summary>
        ''' Scheduleタグの受付担当スタッフコードに対してのアクセッサ
        ''' </summary>
        ''' <value>セールススタッフコード</value>
        Public Property ReceptionStaffCode(ByVal index As Integer) As String
            Get
                Return P_ScheduleInfoDt.Rows(index).Item("ReceptionStaffCode").ToString()
            End Get
            Set(ByVal Value As String)
                P_ScheduleInfoDt.Rows(index).Item("ReceptionStaffCode") = Value
            End Set
        End Property

        ''' <summary>
        ''' Scheduleタグの開始日時に対してのアクセッサ
        ''' </summary>
        ''' <value>開始日時</value>
        Public Property StartTime(ByVal index As Integer) As String
            Get
                Return P_ScheduleInfoDt.Rows(index).Item("StartTime").ToString()
            End Get
            Set(ByVal Value As String)
                P_ScheduleInfoDt.Rows(index).Item("StartTime") = Value
            End Set
        End Property

        ''' <summary>
        ''' Scheduleタグの終了日時に対してのアクセッサ
        ''' </summary>
        ''' <value>終了日時</value>
        Public Property EndTime(ByVal index As Integer) As String
            Get
                Return P_ScheduleInfoDt.Rows(index).Item("EndTime").ToString()
            End Get
            Set(ByVal Value As String)
                P_ScheduleInfoDt.Rows(index).Item("EndTime") = Value
            End Set
        End Property

        ''' <summary>
        ''' Scheduleタグのメモに対してのアクセッサ
        ''' </summary>
        ''' <value>説明</value>
        Public Property Memo(ByVal index As Integer) As String
            Get
                Return P_ScheduleInfoDt.Rows(index).Item("Memo").ToString()
            End Get
            Set(ByVal Value As String)
                P_ScheduleInfoDt.Rows(index).Item("Memo") = Value
            End Set
        End Property

        ''' <summary>
        ''' ScheduleタグのアラームNoに対してのアクセッサ
        ''' </summary>
        ''' <value>アラームNo</value>
        Public Property AlarmNo(ByVal index As Integer) As String
            Get
                Return P_ScheduleInfoDt.Rows(index).Item("AlarmNo").ToString()
            End Get
            Set(ByVal Value As String)
                P_ScheduleInfoDt.Rows(index).Item("AlarmNo") = Value
            End Set
        End Property

        ''' <summary>
        ''' Scheduleタグの接触方法Noに対してのアクセッサ
        ''' </summary>
        ''' <value>アラームNo</value>
        Public Property ContactNo(ByVal index As Integer) As String
            Get
                Return P_ScheduleInfoDt.Rows(index).Item("ContactNo").ToString()
            End Get
            Set(ByVal Value As String)
                P_ScheduleInfoDt.Rows(index).Item("ContactNo") = Value
            End Set
        End Property

        ''' <summary>
        ''' Scheduleタグの接触方法名に対してのアクセッサ
        ''' </summary>
        ''' <value>アラームNo</value>
        Public Property ContactName(ByVal index As Integer) As String
            Get
                Return P_ScheduleInfoDt.Rows(index).Item("ContactName").ToString()
            End Get
            Set(ByVal Value As String)
                P_ScheduleInfoDt.Rows(index).Item("ContactName") = Value
            End Set
        End Property

        ''' <summary>
        ''' Scheduleタグの来客フォロー名に対してのアクセッサ
        ''' </summary>
        ''' <value>アラームNo</value>
        Public Property ComingFollowName(ByVal index As Integer) As String
            Get
                Return P_ScheduleInfoDt.Rows(index).Item("ComingFollowName").ToString()
            End Get
            Set(ByVal Value As String)
                P_ScheduleInfoDt.Rows(index).Item("ComingFollowName") = Value
            End Set
        End Property



        ''' <summary>
        ''' ScheduleタグのTodo背景色に対してのアクセッサ
        ''' </summary>
        ''' <value>Todo背景色</value>
        Public Property BackgroundColor(ByVal index As Integer) As String
            Get
                Return P_ScheduleInfoDt.Rows(index).Item("BackGroundColor").ToString()
            End Get
            Set(ByVal Value As String)
                P_ScheduleInfoDt.Rows(index).Item("BackGroundColor") = Value
            End Set
        End Property


        ''' <summary>
        ''' ScheduleタグのTodoIdに対してのアクセッサ
        ''' </summary>
        ''' <value>アラームNo</value>
        Public Property TodoId(ByVal index As Integer) As String
            Get
                Return P_ScheduleInfoDt.Rows(index).Item("TodoId").ToString()
            End Get
            Set(ByVal Value As String)
                P_ScheduleInfoDt.Rows(index).Item("TodoId") = Value
            End Set
        End Property

        ''' <summary>
        ''' ScheduleタグのProcessDivisionに対してのアクセッサ
        ''' </summary>
        ''' <value>工程区分</value>
        Public Property ProcessDivision(ByVal index As Integer) As String
            Get
                Return P_ScheduleInfoDt.Rows(index).Item("ProcessDivision").ToString()
            End Get
            Set(ByVal Value As String)
                P_ScheduleInfoDt.Rows(index).Item("ProcessDivision") = Value
            End Set
        End Property

        ''' <summary>
        ''' ScheduleタグのProcessNameに対してのアクセッサ
        ''' </summary>
        ''' <value>工程名称</value>
        Public Property ProcessName(ByVal index As Integer) As String
            Get
                Return P_ScheduleInfoDt.Rows(index).Item("ProcessName").ToString()
            End Get
            Set(ByVal Value As String)
                P_ScheduleInfoDt.Rows(index).Item("ProcessName") = Value
            End Set
        End Property

        ''' <summary>
        ''' ScheduleタグのResultDateに対してのアクセッサ
        ''' </summary>
        ''' <value>実績日</value>
        Public Property ResultDate(ByVal index As Integer) As String
            Get
                Return P_ScheduleInfoDt.Rows(index).Item("ResultDate").ToString()
            End Get
            Set(ByVal Value As String)
                P_ScheduleInfoDt.Rows(index).Item("ResultDate") = Value
            End Set
        End Property

        ''' <summary>
        ''' commonタグのMessageに対してのアクセッサ
        ''' </summary>
        ''' <value>Message</value>
        Public ReadOnly Property GetResultDetail() As String
            Get
                Return MyMessage
            End Get
        End Property


        ''' <summary>
        ''' Commonタグの処理区分に対してのアクセッサ
        ''' </summary>
        ''' <value>処理区分</value>
        Public Property LastSendXml() As String
            Get
                Return lastSendXmlValue
            End Get
            Set(ByVal Value As String)
                lastSendXmlValue = Value
            End Set
        End Property

#End Region


        ''' <summary>
        ''' コンストラクタ
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()

            MyMessage = "OK"

            SendDs = New RegistSchedule

            P_CommonDt = New RegistSchedule.P_CommonDataTable()
            P_ScheduleInfoDt = New RegistSchedule.P_ScheduleDataTable()

        End Sub

        ''' <summary>
        ''' 共通タグ作成
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub CreateCommon()

            P_CommonDr = P_CommonDt.NewP_CommonRow
            P_CommonDt.Rows.Add(P_CommonDr)

        End Sub

        ''' <summary>
        ''' スケジュール情報タグ作成
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub CreateScheduleInfo()

            'ScheduleInfoDr = ScheduleInfoDt.NewRow()
            P_ScheduleInfoDt.Rows.Add(P_ScheduleInfoDt.NewRow())

        End Sub

        ''' <summary>
        ''' Webサービス呼び出し
        ''' </summary>
        ''' <param name="WebServiceAddress">WebService参照先url</param>
        ''' <returns>0:正常終了 1:エラー発生</returns>
        ''' <remarks>
        ''' </remarks>
        Public Function SendScheduleInfo(ByVal webServiceAddress As String) As String

            Try
                '入力データをすべて出力する
                writeInputValue()

                '顧客区分が副顧客の場合は情報を送信しない
                If Not P_CommonDr.IsCustomerDivisionNull AndAlso _
                    P_CommonDr.CustomerDivision.Equals(CUSTOMER_DIV_SUB) Then
                    Logger.Info("CustomerDivision Is 1. Do Not Send ScheduleInfo.")
                    Return "0"
                End If

                'プロパティテーブルから送信スケジュール情報を生成
                Using scheduleInfoDataSet As RegistSchedule = New RegistSchedule

                    '各種テーブルを準備
                    '最終的に形成するXMLの概要は以下
                    '<RegistSchedule>
                    '	<Head></Head>
                    '	<Detail>+
                    '		<Common></Common>
                    '		<ScheduleInfo>?
                    '		</ScheduleInfo>
                    '		<Schedule>*	
                    '			<Alarm>*
                    '		</Schedule>
                    '	</Detail>
                    '</RegistSchedule>
                    Dim headTable As RegistSchedule.HEADDataTable = scheduleInfoDataSet.HEAD
                    Dim detailTable As RegistSchedule.DetailDataTable = scheduleInfoDataSet.Detail
                    Dim commonTable As RegistSchedule.CommonDataTable = scheduleInfoDataSet.Common
                    Dim scheduleInfoTable As RegistSchedule.ScheduleInfoDataTable = scheduleInfoDataSet.ScheduleInfo
                    Dim scheduleTable As RegistSchedule.ScheduleDataTable = scheduleInfoDataSet.Schedule
                    Dim alermTable As RegistSchedule.AlarmDataTable = scheduleInfoDataSet.Alarm

                    '親レコード作成(RegistScheduleタグに相当)
                    Dim RegistScheduleKey As Integer = 0

                    'HEADテーブルの生成(Headタグに相当)
                    Dim headRow As RegistSchedule.HEADRow = SetHeadTable(headTable.NewHEADRow(), RegistScheduleKey)
                    headTable.Rows.Add(headRow)

                    'Detailレコード作成(Detailタグに相当)
                    Dim detailKey As Integer = 0
                    Dim detailRow As RegistSchedule.DetailRow = detailTable.NewDetailRow
                    detailRow.RegistScheduleID = RegistScheduleKey
                    detailRow.DetailID = detailKey

                    'Commonレコード作成(Commonタグに相当)
                    Dim commonRow As RegistSchedule.CommonRow = SetCommonTable(commonTable.NewCommonRow(), detailKey)

                    commonRow.SetParentRow(detailRow)
                    commonTable.Rows.Add(commonRow)

                    'ScheduleInfoレコードの作成(ScheduleInfoタグに相当)
                    Dim scheduleInfoRow As RegistSchedule.ScheduleInfoRow = SetScheduleInfoTable(scheduleInfoTable.NewScheduleInfoRow, detailKey)

                    scheduleInfoRow.SetParentRow(detailRow)
                    scheduleInfoTable.Rows.Add(scheduleInfoRow)

                    'Scheduleレコードの作成(Scheduleタグに)
                    Dim scheduleKey As Integer = 0
                    For Each p_scheduleRow As RegistSchedule.P_ScheduleRow In P_ScheduleInfoDt.Rows
                        Dim scheduleRow As RegistSchedule.ScheduleRow = scheduleTable.NewScheduleRow

                        'Request Follow-upで活動結果登録の場合
                        '(来店区分が0 AND 接触方法Noが無い AND 終了日時が入ってる場合)
                        'スケジュール情報タグを生成しない。
                        '1レコード目の内容で判断する
                        If Not String.IsNullOrEmpty(P_CommonDr.CompleteFlg) AndAlso P_CommonDr.ScheduleDivision.Equals("0") And _
                            String.IsNullOrEmpty(p_scheduleRow.ContactNo) And Not String.IsNullOrEmpty(p_scheduleRow.EndTime) Then

                            'スケジュール情報を生成しない
                            Exit For
                        End If

                        'TODO_IDが入ってたら3:Event
                        '終了日時が入っていたら1:Todo+Event
                        '他（開始日時のみ)の場合は2:Todo
                        If Not String.IsNullOrEmpty(p_scheduleRow.TodoId) Then
                            scheduleRow.CreateScheduleDiv = "3"
                        ElseIf Not String.IsNullOrEmpty(p_scheduleRow.StartTime) Then
                            scheduleRow.CreateScheduleDiv = "1"
                        Else
                            scheduleRow.CreateScheduleDiv = "2"
                        End If

                        '処理区分が1の場合は親子区分を設定する
                        If Not commonRow.IsActionTypeNull AndAlso _
                            commonRow.ActionType.Equals("1") Then

                            If scheduleKey.Equals(0) Then
                                '1レコード目の場合は1(親)を設定
                                scheduleRow.ParentDiv = "1"
                            Else
                                '2レコード目以降は2(子)を設定
                                scheduleRow.ParentDiv = "2"
                            End If

                        End If

                        scheduleRow.ActivityStaffBranchCode = p_scheduleRow.ActivityStaffBranchCode
                        scheduleRow.ActivityStaffCode = p_scheduleRow.ActivityStaffCode
                        scheduleRow.ReceptionStaffBranchCode = p_scheduleRow.ReceptionStaffBranchCode
                        scheduleRow.ReceptionStaffCode = p_scheduleRow.ReceptionStaffCode
                        scheduleRow.ContactNo = p_scheduleRow.ContactNo
                        scheduleRow.Summary = createSummary(p_scheduleRow, commonRow, scheduleKey)
                        scheduleRow.StartTime = AddSecond(p_scheduleRow.StartTime)
                        scheduleRow.EndTime = AddSecond(p_scheduleRow.EndTime)
                        scheduleRow.Memo = p_scheduleRow.Memo

                        ' 2014/04/25 TMEJ 水本 受注後フォロー機能開発（スタッフ活動KPI）に向けたシステム設計 START
                        scheduleRow.ContactName = p_scheduleRow.ContactName
                        scheduleRow.OdrDiv = ODR_DIV_BEFORE
                        ' 2014/04/25 TMEJ 水本 受注後フォロー機能開発（スタッフ活動KPI）に向けたシステム設計 END

                        scheduleRow.ProcessDiv = p_scheduleRow.ProcessDivision
                        scheduleRow.ResultDate = p_scheduleRow.ResultDate

                        If Not String.IsNullOrEmpty(p_scheduleRow.BackGroundColor) Then
                            scheduleRow.XiCropColor = """" + p_scheduleRow.BackGroundColor + """"
                        End If

                        'alarmレコードの設定
                        scheduleRow.AlermID = scheduleKey
                        Dim alermRow As RegistSchedule.AlarmRow = alermTable.NewAlarmRow
                        alermRow.ID = scheduleKey
                        If p_scheduleRow.AlarmNo.Equals("0") Then
                            alermRow.SetTriggerNull()
                        Else
                            alermRow.Trigger = p_scheduleRow.AlarmNo
                        End If

                        alermRow.SetParentRow(scheduleRow)
                        alermTable.Rows.Add(alermRow)

                        scheduleRow.TodoID = p_scheduleRow.TodoId
                        scheduleKey = scheduleKey + 1

                        scheduleRow.SetParentRow(detailRow)
                        scheduleTable.Rows.Add(scheduleRow)
                    Next

                    detailTable.Rows.Add(detailRow)

                    'WebService.SetXmlを実行、エラーコードを返却
                    Return SendXmlByWebService(scheduleInfoDataSet, webServiceAddress)
                End Using

            Catch e As System.Net.WebException
                Logger.Error("WebServiceError!!", e)
                Return "9999"
            Catch e As Exception
                Logger.Error("Error!!", e)
                Throw
            End Try

        End Function

        ' 2014/04/25 TMEJ 水本 受注後フォロー機能開発（スタッフ活動KPI）に向けたシステム設計 START
        ''' <summary>
        ''' Webサービス呼び出し(受注後工程)
        ''' </summary>
        ''' <param name="webServiceAddress">WebService参照先URL</param>
        ''' <param name="registAfterOrderSchedule">受注後活動スケジュール情報</param>
        ''' <returns>0:正常終了 0以外:エラー発生</returns>
        ''' <remarks>
        ''' </remarks>
        Public Function SendAfterProcessScheduleInfo(ByVal webServiceAddress As String, _
                                                     ByVal registAfterOrderSchedule As RegistAfterOrder) As String

            Try

                If registAfterOrderSchedule Is Nothing Then
                    Return "0"
                End If

                '受注後工程のXMLオブジェクトを作成する
                Dim document As XmlDocument = GetAfterProcessXml(registAfterOrderSchedule)

                If document Is Nothing Then
                    Return "0"
                End If

                lastSendXmlValue = document.OuterXml

                '送信XMLをログに出力
                Logger.Info(lastSendXmlValue)

                '送信XMLを記録
                lastSendXmlValue = "xsData=" + HttpUtility.UrlEncode(lastSendXmlValue) + ""

                Dim resultXmlDoc As XmlDocument = New XmlDocument
                resultXmlDoc.LoadXml(CallWebServiceSite(lastSendXmlValue, webServiceAddress & "/RegistAfterOrderSchedule"))

                '結果XmlのRequestIDに0以外のコード（エラー）が含まれていた場合
                'エラー結果を返却する
                For Each resultIdNode As XmlNode In resultXmlDoc.SelectNodes("descendant::ResultId")

                    If Not resultIdNode.InnerXml.Equals("0") Then
                        'エラーコードが6001の場合
                        If Not resultIdNode.InnerXml.Equals("6001") Then
                            Return resultIdNode.InnerXml
                        Else
                            Logger.Info("It ignored, although the standpoint of the error 6001 was carried out. ")
                        End If
                    End If
                Next

                Return "0"

            Catch e As System.Net.WebException
                Logger.Error("WebServiceError!!", e)
                Return "9999"
            Catch e As Exception
                Logger.Error("Error!!", e)
                Throw
            End Try

        End Function

        ''' <summary>
        ''' 受注後工程のXMLオブジェクトを作成する
        ''' </summary>
        ''' <param name="registAfterOrderSchedule">受注後活動スケジュール情報</param>
        ''' <returns>XMLオブジェクト</returns>
        ''' <remarks>
        ''' </remarks>
        Public Function GetAfterProcessXml(ByVal registAfterOrderSchedule As RegistAfterOrder) As IXPathNavigable

            Dim document As New XmlDocument()

            Dim rootElement As XmlElement = document.CreateElement("RegistAfterOrderSchedule")
            document.AppendChild(rootElement)

            'Headタグ
            Dim headElement As XmlElement = document.CreateElement("Head")
            rootElement.AppendChild(headElement)

            AppendChildElement(headElement, "MessageID", MESSAGE_ID)
            AppendChildElement(headElement, "CountryCode", EnvironmentSetting.CountryCode)
            AppendChildElement(headElement, "LinkSystemCode", LINK_SYSTEM_CODE)
            AppendChildElement(headElement, "TransmissionDate", GetNow())

            If registAfterOrderSchedule.DetailList Is Nothing Then
                Return Nothing
            End If

            Dim isFindDetail As Boolean = False

            For Each xmlDetail In registAfterOrderSchedule.DetailList

                'Detailタグ
                Dim detailElement As XmlElement = document.CreateElement("Detail")

                'Commonタグ
                If xmlDetail.Common IsNot Nothing Then

                    Dim commonElement As XmlElement = document.CreateElement("Common")
                    detailElement.AppendChild(commonElement)

                    AppendChildElement(commonElement, "DealerCode", xmlDetail.Common.DealerCode)
                    AppendChildElement(commonElement, "BranchCode", xmlDetail.Common.BranchCode)
                    AppendChildElement(commonElement, "ScheduleID", xmlDetail.Common.ScheduleId)
                    AppendChildElement(commonElement, "ActionType", xmlDetail.Common.ActionType)
                    AppendChildElement(commonElement, "ActivityCreateStaff", xmlDetail.Common.ActivityCreateStaff)

                End If

                'ScheduleInfoタグ
                If xmlDetail.ScheduleInfo IsNot Nothing Then

                    ' 顧客区分が副顧客の場合は情報を送信しない
                    If CUSTOMER_DIV_SUB.Equals(xmlDetail.ScheduleInfo.CustomerDiv) Then
                        Continue For
                    End If

                    isFindDetail = True

                    Dim scheduleInfoElement As XmlElement = document.CreateElement("ScheduleInfo")
                    detailElement.AppendChild(scheduleInfoElement)

                    AppendChildElement(scheduleInfoElement, "CustomerDiv", xmlDetail.ScheduleInfo.CustomerDiv)
                    AppendChildElement(scheduleInfoElement, "CustomerCode", xmlDetail.ScheduleInfo.CustomerCode)
                    AppendChildElement(scheduleInfoElement, "DmsID", xmlDetail.ScheduleInfo.DmsId)
                    AppendChildElement(scheduleInfoElement, "CustomerName", xmlDetail.ScheduleInfo.CustomerName)
                    AppendChildElement(scheduleInfoElement, "DeleteDate", xmlDetail.ScheduleInfo.DeleteDate)

                End If

                'Scheduleタグ
                If xmlDetail.ScheduleList IsNot Nothing Then

                    For Each xmlSchedule In xmlDetail.ScheduleList

                        Dim scheduleElement As XmlElement = document.CreateElement("Schedule")
                        detailElement.AppendChild(scheduleElement)

                        AppendChildElement(scheduleElement, "CreateScheduleDiv", xmlSchedule.CreateScheduleDiv)
                        AppendChildElement(scheduleElement, "ActivityStaffBranchCode", xmlSchedule.ActivityStaffBranchCode)
                        AppendChildElement(scheduleElement, "ActivityStaffCode", xmlSchedule.ActivityStaffCode)
                        AppendChildElement(scheduleElement, "ReceptionStaffBranchCode", xmlSchedule.ReceptionStaffBranchCode)
                        AppendChildElement(scheduleElement, "ReceptionStaffCode", xmlSchedule.ReceptionStaffCode)
                        AppendChildElement(scheduleElement, "ContactNo", xmlSchedule.ContactNo)
                        AppendChildElement(scheduleElement, "ContactName", xmlSchedule.ContactName)
                        AppendChildElement(scheduleElement, "ActOdrName", xmlSchedule.ActOdrName)
                        AppendChildElement(scheduleElement, "Summary", xmlSchedule.Summary)
                        AppendChildElement(scheduleElement, "StartTime", xmlSchedule.StartTime)
                        AppendChildElement(scheduleElement, "EndTime", xmlSchedule.EndTime)
                        AppendChildElement(scheduleElement, "Memo", xmlSchedule.Memo)
                        AppendChildElement(scheduleElement, "XiCropColor", xmlSchedule.XIcropColor)

                        'Alarmタグ
                        If xmlSchedule.AlarmTriggerList IsNot Nothing Then

                            For Each alarmTrigger In xmlSchedule.AlarmTriggerList

                                Dim alarmElement As XmlElement = document.CreateElement("Alarm")
                                scheduleElement.AppendChild(alarmElement)

                                AppendChildElement(alarmElement, "Trigger", alarmTrigger)

                            Next

                        End If

                        AppendChildElement(scheduleElement, "OdrDiv", xmlSchedule.OdrDiv)
                        AppendChildElement(scheduleElement, "AfterOdrActID", xmlSchedule.AfterOdrActId)
                        AppendChildElement(scheduleElement, "TodoID", xmlSchedule.TodoId)
                        AppendChildElement(scheduleElement, "ProcessDiv", xmlSchedule.ProcessDiv)
                        AppendChildElement(scheduleElement, "ResultDate", xmlSchedule.ResultDate)

                    Next

                End If

                rootElement.AppendChild(detailElement)

            Next

            If Not isFindDetail Then
                Return Nothing
            End If

            Return document

        End Function

        ''' <summary>
        ''' XMLの子要素を設定する
        ''' </summary>
        ''' <param name="parentElement">親タグ</param>
        ''' <param name="name">タグ名</param>
        ''' <param name="value">値</param>
        ''' <remarks>
        ''' </remarks>
        Public Sub AppendChildElement(ByVal parentElement As IXPathNavigable, ByVal name As String, ByVal value As String)

            If value IsNot Nothing Then
                Dim parentElementXml As XmlElement = DirectCast(parentElement, XmlElement)
                Dim childElement As XmlElement = parentElementXml.OwnerDocument.CreateElement(name)
                parentElementXml.AppendChild(childElement)
                childElement.InnerText = value
            End If

        End Sub
        ' 2014/04/25 TMEJ 水本 受注後フォロー機能開発（スタッフ活動KPI）に向けたシステム設計 END

        ''' <summary>
        ''' Headテーブルに値を設定する
        ''' </summary>
        ''' <param name="headRow">設定対象列</param>
        ''' <param name="RegistScheduleKey">シーケンスID</param>
        ''' <returns>値設定済み列</returns>
        ''' <remarks></remarks>
        Private Function SetHeadTable(ByVal headRow As RegistSchedule.HEADRow, ByVal RegistScheduleKey As Integer) As RegistSchedule.HEADRow
            headRow.MESSAGEID = MESSAGE_ID
            ' 2014/04/25 TMEJ 水本 受注後フォロー機能開発（スタッフ活動KPI）に向けたシステム設計 START
            headRow.COUNTRYCODE = EnvironmentSetting.CountryCode
            ' 2014/04/25 TMEJ 水本 受注後フォロー機能開発（スタッフ活動KPI）に向けたシステム設計 END
            headRow.LINKSYSTEMCODE = LINK_SYSTEM_CODE
            headRow.TRANSMISSIONDATE = GetNow()
            headRow.RegistScheduleID = RegistScheduleKey

            Return headRow
        End Function


        ''' <summary>
        ''' Commonテーブルに値を設定する
        ''' </summary>
        ''' <param name="commonRow">設定対象列</param>
        ''' <param name="detailKey">シーケンスID</param>
        ''' <returns>値設定済み列</returns>
        ''' <remarks></remarks>
        Private Function SetCommonTable(ByVal commonRow As RegistSchedule.CommonRow, ByVal detailKey As Integer) As RegistSchedule.CommonRow
            commonRow.DealerCode = P_CommonDr.DealerCode
            commonRow.BranchCode = P_CommonDr.BranchCode
            commonRow.ScheduleDiv = P_CommonDr.ScheduleDivision
            commonRow.ScheduleID = P_CommonDr.SCHEDULEID

            If P_CommonDr.IsActionTypeNull = False Then
                If P_ScheduleInfoDt.Rows.Count >= 1 Then
                    'スケジュールテーブルがあるため、TODOIDを取得
                    'TODOIDは1行目から取得されるものを使用
                    Dim todoDr As RegistSchedule.P_ScheduleRow = P_ScheduleInfoDt.Rows(0)
                    commonRow.ActionType = convertSendActionType(P_CommonDr.ActionType, todoDr.TodoId)
                Else
                    'スケジュールテーブルがないため、TODOIDを空で設定
                    commonRow.ActionType = convertSendActionType(P_CommonDr.ActionType, Nothing)
                End If
            End If

            commonRow.ActivityCreateStaff = P_CommonDr.ActivityCreateStaffCode
            commonRow.DetailID = detailKey
            Return commonRow
        End Function

        ''' <summary>
        ''' ScheduleInfoテーブルに値を設定する
        ''' </summary>
        ''' <param name="scheduleInfoRow">設定対象列</param>
        ''' <param name="detailKey">シーケンスID</param>
        ''' <returns>値設定済み列</returns>
        ''' <remarks></remarks>
        Private Function SetScheduleInfoTable(ByVal scheduleInfoRow As RegistSchedule.ScheduleInfoRow, ByVal detailKey As Integer) As RegistSchedule.ScheduleInfoRow
            scheduleInfoRow.CustomerDiv = P_CommonDr.CustomerDivision
            scheduleInfoRow.CustomerCode = P_CommonDr.CustomerId
            scheduleInfoRow.DmsID = P_CommonDr.DmsId
            scheduleInfoRow.CustomerName = P_CommonDr.CustomerName
            scheduleInfoRow.ReceptionDiv = P_CommonDr.ReceptionDivision
            scheduleInfoRow.ServiceCode = P_CommonDr.ServiceCode
            scheduleInfoRow.MerchandiseCd = P_CommonDr.MerchandiseCode
            scheduleInfoRow.StrStatus = P_CommonDr.StoreStatus
            scheduleInfoRow.RezStatus = P_CommonDr.ReservationStatus
            scheduleInfoRow.CompletionDiv = P_CommonDr.CompleteFlg

            '完了フラグが2または3の場合、完了日付を設定
            If P_CommonDr.CompleteFlg.Equals("2") Or P_CommonDr.CompleteFlg.Equals("3") Then
                '完了日付が無ければ現在日時を設定
                If String.IsNullOrEmpty(P_CommonDr.CompletionDate) Then
                    scheduleInfoRow.CompletionDate = GetNow()
                Else
                    scheduleInfoRow.CompletionDate = AddSecond(P_CommonDr.CompletionDate)
                End If
            End If

            '処理区分が2(キャンセル)のときのみ設定
            If P_CommonDr.ActionType.Equals("2") Then
                scheduleInfoRow.DeleteDate = GetNow()
            End If

            scheduleInfoRow.DetailID = detailKey

            Return scheduleInfoRow
        End Function

        ''' <summary>
        ''' WebServiceへXMLの送信を行う
        ''' </summary>
        ''' <param name="scheduleInfoDataSet">送信データテーブル</param>
        ''' <param name="WebServiceUrl">送信先WebServieURL</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <History>
        '''  2012/02/14 KN 田中 【SALES_1A】号口(課題No.51)対応
        ''' </History>
        Private Function SendXmlByWebService(ByVal scheduleInfoDataSet As RegistSchedule, ByVal WebServiceUrl As String) As String
            Dim culture As CultureInfo = New CultureInfo("")
            Using sw As StringWriter = New StringWriter(culture)

                '入力のないタグを削除
                scheduleInfoDataSet = DeleteEmptyTag(scheduleInfoDataSet)

                Dim xtr As XmlTextWriter = New XmlTextWriter(sw)
                scheduleInfoDataSet.WriteXml(xtr, System.Data.XmlWriteMode.IgnoreSchema)

                'XML名前空間を除去
                Dim regex As Regex = New Regex(" xmlns=""[^""]*""")
                lastSendXmlValue = regex.Replace(sw.ToString, Space(0))


                ' 2012/02/14 KN 田中 GTMC130118132対応 MODIFY ログ出力をDebug→Error START
                '送信XMLをログに出力
                Logger.Info(lastSendXmlValue)
                ' 2012/02/14 KN 田中 GTMC130118132対応 MODIFY ログ出力をDebug→Error End

                '送信XMLを記録
                lastSendXmlValue = "xsData=" + HttpUtility.UrlEncode(lastSendXmlValue) + ""


                ' 2012/02/14 KN 田中 【SALES_1A】号口(課題No.51)対応 ADD START
                'Logger.Error("Before Call IC3040403.")
                ' 2012/02/14 KN 田中 【SALES_1A】号口(課題No.51)対応 ADD END

                Dim resultXmlDoc As XmlDocument = New XmlDocument
                resultXmlDoc.LoadXml(CallWebServiceSite(lastSendXmlValue, WebServiceUrl & "/RegistSchedule"))

                ' 2012/02/14 KN 田中 【SALES_1A】号口(課題No.51)対応 ADD START
                'Logger.Error("After Call IC3040403.")
                ' 2012/02/14 KN 田中 【SALES_1A】号口(課題No.51)対応 ADD END

                '結果XmlのRequestIDに0以外のコード（エラー）が含まれたいた場合
                'エラー結果を返却する
                For Each resultIdNode As XmlNode In resultXmlDoc.SelectNodes("descendant::ResultId")
                    ' 2012/02/14 KN 田中 【SALES_1A】号口(課題No.51)対応 ADD START
                    'Logger.Error("IC3040403WebService Response No(ResultId):" + resultIdNode.InnerXml)
                    ' 2012/02/14 KN 田中 【SALES_1A】号口(課題No.51)対応 ADD END

                    If Not resultIdNode.InnerXml.Equals("0") Then
                        'エラーコードが6001の場合
                        If Not resultIdNode.InnerXml.Equals("6001") Then
                            'エラーコード発生
                            ' 2012/02/14 KN 田中 【SALES_1A】号口(課題No.51)対応 ADD START
                            'Logger.Error("ReturnCd:" + resultIdNode.InnerXml)
                            ' 2012/02/14 KN 田中 【SALES_1A】号口(課題No.51)対応 ADD END
                            Return resultIdNode.InnerXml
                        Else
                            Logger.Info("It ignored, although the standpoint of the error 6001 was carried out. ")
                        End If
                    End If
                Next
            End Using


            ' 2012/02/14 KN 田中 【SALES_1A】号口(課題No.51)対応 ADD START
            ' Logger.Error("ReturnCd:0")
            ' 2012/02/14 KN 田中 【SALES_1A】号口(課題No.51)対応 ADD END
            Return "0"
        End Function

        ''' <summary>
        ''' WebServiceのサイトを呼び出す
        ''' </summary>
        ''' <param name="postData">送信文字列</param>
        ''' <param name="WebServiceUrl">送信先アドレス</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function CallWebServiceSite(ByVal postData As String, ByVal WebServiceUrl As String) As String
            '文字コードを指定する
            Dim enc As System.Text.Encoding = _
                System.Text.Encoding.GetEncoding("UTF-8")

            'バイト型配列に変換
            Dim postDataBytes As Byte() = _
                System.Text.Encoding.ASCII.GetBytes(postData)

            'WebRequestの作成
            Dim req As System.Net.WebRequest = _
                System.Net.WebRequest.Create(WebServiceUrl)
            'メソッドにPOSTを指定
            req.Method = "POST"
            'ContentTypeを"application/x-www-form-urlencoded"にする
            req.ContentType = "application/x-www-form-urlencoded"
            'POST送信するデータの長さを指定
            req.ContentLength = postDataBytes.Length

            'データをPOST送信するためのStreamを取得
            Dim reqStream As System.IO.Stream = req.GetRequestStream()
            '送信するデータを書き込む
            reqStream.Write(postDataBytes, 0, postDataBytes.Length)
            reqStream.Close()

            'サーバーからの応答を受信するためのWebResponseを取得
            Dim res As System.Net.WebResponse = req.GetResponse()
            '応答データを受信するためのStreamを取得
            Dim resStream As System.IO.Stream = res.GetResponseStream()
            '受信して表示
            Dim sr As New System.IO.StreamReader(resStream, enc)

            '返却文字列を取得
            Dim returnString As String = sr.ReadToEnd()

            '閉じる
            sr.Close()

            Return returnString
        End Function


        ''' <summary>
        ''' 画面用の処理区分から送信用の処理区分に変換
        ''' </summary>
        ''' <param name="value">画面パラメータ：処理区分</param>
        ''' <param name="todoId">画面パラメータ：TODOID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function convertSendActionType(ByVal value As String, ByVal todoId As String) As String
            Dim ActionType As Integer

            '数値に変換
            If Not Integer.TryParse(value, ActionType) Then
                '変換できない値。そのまま返却
                Return value
            End If

            '処理区分の判断
            If ActionType.Equals(0) Then
                If Not todoId Is Nothing AndAlso todoId.Length > 0 Then
                    Return ACTION_TYPE_INSERT_EVENT
                Else
                    '新規作成
                    Return ACTION_TYPE_INSERT
                End If

            ElseIf ActionType.Equals(1) Then
                '更新
                Return ACTION_TYPE_UPDATE
            ElseIf ActionType.Equals(2) Then
                'キャンセル
                Return ACTION_TYPE_UPDATE
            Else
                'すべて
                Return value
            End If

        End Function

        ''' <summary>
        ''' 送信情報タイトル列の生成
        ''' </summary>
        ''' <param name="p_scheduleRow">画面パラメータテーブル：送信情報</param>
        ''' <param name="commonRow">送信情報テーブル：Common</param>
        ''' <param name="recordCount">送信情報テーブルレコード数</param>
        ''' <returns>送信情報タイトル文字列</returns>
        ''' <remarks></remarks>
        Private Function createSummary(ByVal p_scheduleRow As RegistSchedule.P_ScheduleRow, ByVal commonRow As RegistSchedule.CommonRow, ByVal recordCount As Integer) As String

            'タイトルの生成
            If commonRow.ScheduleDiv.Equals("0") Then
                '来店区分
                If P_ScheduleInfoDt.Rows.Count > 1 And recordCount.Equals(0) Then
                    'ScheduleInfoが2レコード以上存在し、
                    '且つ1レコード目の場合
                    If P_CommonDr.NameTitlePosition.Equals("1") Then
                        Return P_CommonDr.NameTitle + P_CommonDr.CustomerName + Space(1) + p_scheduleRow.ComingFollowName
                    Else
                        Return P_CommonDr.CustomerName + P_CommonDr.NameTitle + Space(1) + p_scheduleRow.ComingFollowName
                    End If
                Else
                    '1レコードしかない場合　または
                    'ScheduleInfoが2レコード以上存在し、
                    '且つ2レコード目以降の場合
                    If P_CommonDr.NameTitlePosition.Equals("1") Then
                        Return P_CommonDr.NameTitle + P_CommonDr.CustomerName + Space(1) + p_scheduleRow.ContactName
                    Else
                        Return P_CommonDr.CustomerName + P_CommonDr.NameTitle + Space(1) + p_scheduleRow.ContactName
                    End If
                End If

            ElseIf commonRow.ScheduleDiv.Equals("1") Then
                '入庫予約
                Dim initialString As String = String.Empty
                If P_CommonDr.ReservationStatus.Equals("2") Then
                    'タイトルの頭文字に"仮"を付加
                    initialString = P_CommonDr.ReservationStatusName + Space(1)
                End If


                If P_CommonDr.NameTitlePosition.Equals("1") Then
                    Return initialString + P_CommonDr.NameTitle + P_CommonDr.CustomerName + Space(1) + P_CommonDr.MerchandiseName + Space(1) + P_CommonDr.ServiceName
                Else
                    Return initialString + P_CommonDr.CustomerName + P_CommonDr.NameTitle + Space(1) + P_CommonDr.MerchandiseName + Space(1) + P_CommonDr.ServiceName
                End If

            Else
                '受注後工程

                If P_CommonDr.NameTitlePosition.Equals("1") Then
                    Return P_CommonDr.NameTitle + P_CommonDr.CustomerName + Space(1) + p_scheduleRow.ProcessName
                Else
                    Return P_CommonDr.CustomerName + P_CommonDr.NameTitle + Space(1) + p_scheduleRow.ProcessName
                End If

            End If
        End Function


        ''' <summary>
        ''' 入力内容のないタグを削除する
        ''' </summary>
        ''' <remarks>顧客名、メモは入力内容がなくとも削除しない</remarks>
        Private Function DeleteEmptyTag(ByVal scheduleInfoDataSet As RegistSchedule) As RegistSchedule

            'commonテーブルのタグ削除
            Dim commonTable As RegistSchedule.CommonDataTable = scheduleInfoDataSet.Common
            commonTable = DeleteCommonTableEmptyTag(commonTable)


            'scheduleInfoテーブルのタグ削除
            Dim scheduleInfoTable As RegistSchedule.ScheduleInfoDataTable = scheduleInfoDataSet.ScheduleInfo
            scheduleInfoTable = DeleteScheduleInfoTableEmptyTag(scheduleInfoTable)


            'scheduleテーブルのタグ削除
            Dim scheduleTable As RegistSchedule.ScheduleDataTable = scheduleInfoDataSet.Schedule
            scheduleTable = DeleteScheduleTableEmptyTag(scheduleTable)


            'scheduleテーブルのタグ削除
            Dim alermTable As RegistSchedule.AlarmDataTable = scheduleInfoDataSet.Alarm
            alermTable = DeleteAlermTableEmptyTag(alermTable)

            Return scheduleInfoDataSet
        End Function


        ''' <summary>
        ''' 現在日付をYYYYMMDDの形式で取得する。
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function GetNow() As String
            Dim culture As CultureInfo = New CultureInfo("")
            Return String.Format(culture, "{0:0000}/{1:00}/{2:00} {3:00}:{4:00}:{5:00}", Now.Year, Now.Month, Now.Day, Now.Hour, Now.Minute, Now.Second)
        End Function

        ''' <summary>
        ''' 日付形式の文字列に秒がない場合、秒を加える
        ''' </summary>
        ''' <param name="value">日付形式の文字列</param>
        ''' <returns>秒を負荷した文字列</returns>
        ''' <remarks></remarks>
        Private Function AddSecond(ByVal value As String) As String
            Dim dateValue As Date

            If Not String.IsNullOrEmpty(value) AndAlso Date.TryParse(value, dateValue) Then
                '日付形式の文字列が入力された
                '11文字以下の場合は時間が入力されていないと判断
                If value.Length < 11 Then
                    'yyyy/mm/ddで返却する
                    Return String.Format(CultureInfo.InvariantCulture, "{0:0000}/{1:00}/{2:00}", dateValue.Year, dateValue.Month, dateValue.Day)
                Else
                    '時間分(0:0:0)の場合、時刻以降
                    If dateValue.Hour.Equals(0) And dateValue.Minute.Equals(0) And dateValue.Second.Equals(0) Then
                        'yyyy/mm/dd hh24:mi:ssで返却する
                        Return String.Format(CultureInfo.InvariantCulture, "{0:0000}/{1:00}/{2:00}", dateValue.Year, dateValue.Month, dateValue.Day)
                    Else
                        'yyyy/mm/dd hh24:mi:ssで返却する
                        Return String.Format(CultureInfo.InvariantCulture, "{0:0000}/{1:00}/{2:00} {3:00}:{4:00}:{5:00}", dateValue.Year, dateValue.Month, dateValue.Day, dateValue.Hour, dateValue.Minute, dateValue.Second)
                    End If
                End If

                Return value
            Else
                '日付形式に変換できない。
                '入力値をそのまま返却する
                Return value
            End If
        End Function
        ''' <summary>
        ''' IDisposableインターフェイス.Dispoase
        ''' </summary>
        ''' <remarks></remarks>
        Public Overloads Sub Dispose() Implements IDisposable.Dispose
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub

        Protected Overridable Overloads Sub Dispose(ByVal disposing As Boolean)

            If disposing Then
                SendDs.Dispose()
                P_CommonDt.Dispose()
                P_ScheduleInfoDt.Dispose()

                SendDs = Nothing
                P_CommonDt = Nothing
                P_ScheduleInfoDt = Nothing
            End If

        End Sub

        ''' <summary>
        ''' Commonテーブルの空タグを削除する
        ''' </summary>
        ''' <param name="commonTable">commonテーブル</param>
        ''' <returns>空タグ削除後のcommonテーブル</returns>
        ''' <remarks></remarks>
        Private Function DeleteCommonTableEmptyTag(ByVal commonTable As RegistSchedule.CommonDataTable) As RegistSchedule.CommonDataTable

            For Each commonRow As RegistSchedule.CommonRow In commonTable.Rows
                '処理区分
                If String.IsNullOrEmpty(commonRow.ActionType) Then
                    commonRow.SetActionTypeNull()
                End If

                '活動作成スタッフコード
                If String.IsNullOrEmpty(commonRow.ActivityCreateStaff) Then
                    commonRow.SetActivityCreateStaffNull()
                End If
            Next

            Return commonTable
        End Function


        ''' <summary>
        ''' ScheduleInfoテーブルの空タグを削除する
        ''' </summary>
        ''' <param name="scheduleInfoTable">scheduleInfoTableテーブル</param>
        ''' <returns>空タグ削除後のscheduleInfoTableテーブル</returns>
        ''' <remarks>顧客名タグは削除しない</remarks>
        Private Function DeleteScheduleInfoTableEmptyTag(ByVal scheduleInfoTable As RegistSchedule.ScheduleInfoDataTable) As RegistSchedule.ScheduleInfoDataTable
            For Each scheduleInfoRow As RegistSchedule.ScheduleInfoRow In scheduleInfoTable.Rows

                '顧客区分
                If String.IsNullOrEmpty(scheduleInfoRow.CustomerDiv) Then
                    scheduleInfoRow.SetCustomerDivNull()
                End If

                '顧客コード
                If String.IsNullOrEmpty(scheduleInfoRow.CustomerCode) Then
                    scheduleInfoRow.SetCustomerCodeNull()
                End If


                '顧客名
                If String.IsNullOrEmpty(scheduleInfoRow.CustomerName) Then
                    scheduleInfoRow.SetCustomerNameNull()
                End If

                'DMSID
                If String.IsNullOrEmpty(scheduleInfoRow.DmsID) Then
                    scheduleInfoRow.SetDmsIDNull()
                End If

                '受付納車区分
                If String.IsNullOrEmpty(scheduleInfoRow.ReceptionDiv) Then
                    scheduleInfoRow.SetReceptionDivNull()
                End If

                'サービスコード
                If String.IsNullOrEmpty(scheduleInfoRow.ServiceCode) Then
                    scheduleInfoRow.SetServiceCodeNull()
                End If

                '商品コード
                If String.IsNullOrEmpty(scheduleInfoRow.MerchandiseCd) Then
                    scheduleInfoRow.SetMerchandiseCdNull()
                End If

                '入庫ステータス
                If String.IsNullOrEmpty(scheduleInfoRow.StrStatus) Then
                    scheduleInfoRow.SetStrStatusNull()
                End If

                '予約ステータス
                If String.IsNullOrEmpty(scheduleInfoRow.RezStatus) Then
                    scheduleInfoRow.SetRezStatusNull()
                End If

                '完了区分
                If String.IsNullOrEmpty(scheduleInfoRow.CompletionDiv) Then
                    scheduleInfoRow.SetCompletionDivNull()
                End If

                '完了日
                If String.IsNullOrEmpty(scheduleInfoRow.CompletionDate) Then
                    scheduleInfoRow.SetCompletionDateNull()
                End If

                '削除日
                If String.IsNullOrEmpty(scheduleInfoRow.DeleteDate) Then
                    scheduleInfoRow.SetDeleteDateNull()
                End If

            Next

            Return scheduleInfoTable
        End Function

        ''' <summary>
        ''' Scheduleテーブルの空タグを削除する
        ''' </summary>
        ''' <param name="scheduleTable">scheduleTableテーブル</param>
        ''' <returns>空タグ削除後のscheduleTableテーブル</returns>
        ''' <remarks>メモタグは削除しない</remarks>
        Private Function DeleteScheduleTableEmptyTag(ByVal scheduleTable As RegistSchedule.ScheduleDataTable) As RegistSchedule.ScheduleDataTable
            For Each scheduleRow As RegistSchedule.ScheduleRow In scheduleTable.Rows
                'スケジュール作成区分
                If String.IsNullOrEmpty(scheduleRow.CreateScheduleDiv) Then
                    scheduleRow.SetCreateScheduleDivNull()
                End If

                '親子区分
                If String.IsNullOrEmpty(scheduleRow.ParentDiv) Then
                    scheduleRow.SetParentDivNull()
                End If

                '活動担当スタッフ店舗コード
                If String.IsNullOrEmpty(scheduleRow.ActivityStaffBranchCode) Then
                    scheduleRow.SetActivityStaffBranchCodeNull()
                End If

                '活動スタッフコード
                If String.IsNullOrEmpty(scheduleRow.ActivityStaffCode) Then
                    scheduleRow.SetActivityStaffCodeNull()
                End If

                '受付担当スタッフ店舗コード
                If String.IsNullOrEmpty(scheduleRow.ReceptionStaffBranchCode) Then
                    scheduleRow.SetReceptionStaffBranchCodeNull()
                End If

                '受付担当スタッフコード
                If String.IsNullOrEmpty(scheduleRow.ReceptionStaffCode) Then
                    scheduleRow.SetReceptionStaffCodeNull()
                End If

                '接触方法No
                If String.IsNullOrEmpty(scheduleRow.ContactNo) Then
                    scheduleRow.SetContactNoNull()
                End If

                ' 2014/04/25 TMEJ 水本 受注後フォロー機能開発（スタッフ活動KPI）に向けたシステム設計 START
                '接触方法名
                If String.IsNullOrEmpty(scheduleRow.ContactName) Then
                    scheduleRow.SetContactNameNull()
                End If
                ' 2014/04/25 TMEJ 水本 受注後フォロー機能開発（スタッフ活動KPI）に向けたシステム設計 END

                'タイトル
                If String.IsNullOrEmpty(scheduleRow.Summary) Then
                    scheduleRow.SetSummaryNull()
                End If

                '開始日時
                If String.IsNullOrEmpty(scheduleRow.StartTime) Then
                    scheduleRow.SetStartTimeNull()
                End If

                '終了日時
                If String.IsNullOrEmpty(scheduleRow.EndTime) Then
                    scheduleRow.SetEndTimeNull()
                End If

                '色設定
                If String.IsNullOrEmpty(scheduleRow.XiCropColor) Then
                    scheduleRow.SetXiCropColorNull()
                End If

                'TodoID
                If String.IsNullOrEmpty(scheduleRow.TodoID) Then
                    scheduleRow.SetTodoIDNull()
                End If

                '工程区分
                If String.IsNullOrEmpty(scheduleRow.ProcessDiv) Then
                    scheduleRow.SetProcessDivNull()
                End If

                '実績日
                If String.IsNullOrEmpty(scheduleRow.ResultDate) Then
                    scheduleRow.SetResultDateNull()
                End If

            Next

            Return scheduleTable
        End Function


        ''' <summary>
        ''' alermテーブルの空タグを削除する
        ''' </summary>
        ''' <param name="alermTable">alermTableテーブル</param>
        ''' <returns>空タグ削除後のalermTableテーブル</returns>
        ''' <remarks>メモタグは削除しない</remarks>
        Private Function DeleteAlermTableEmptyTag(ByVal alermTable As RegistSchedule.AlarmDataTable) As RegistSchedule.AlarmDataTable
            For Each alermRow As RegistSchedule.AlarmRow In alermTable.Rows
                'アラーム起動タイミング
                If String.IsNullOrEmpty(alermRow.Trigger) Then
                    alermRow.SetTriggerNull()
                End If
            Next
            Return alermTable
        End Function

        ''' <summary>
        ''' 入力項目をすべてログ出力する
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub writeInputValue()

            ' 2012/02/14 KN 田中 GTMC130118132対応 MODIFY ログ出力をDebug→Error START
            'Common
            Logger.Info("ActionType:""" + P_CommonDr.ActionType + """")
            Logger.Info("DealerCode:""" + P_CommonDr.DealerCode + """")
            Logger.Info("BranchCode:""" + P_CommonDr.BranchCode + """")
            Logger.Info("ScheduleId:""" + P_CommonDr.SCHEDULEID + """")
            Logger.Info("ScheduleDivision:""" + P_CommonDr.ScheduleDivision + """")
            Logger.Info("ActivityCreateStaffCode:""" + P_CommonDr.ActivityCreateStaffCode + """")
            Logger.Info("CompleteFlg:""" + P_CommonDr.CompleteFlg + """")
            Logger.Info("CompletionDate:""" + P_CommonDr.CompleteFlg + """")
            Logger.Info("CustomerDivision:""" + P_CommonDr.CustomerDivision + """")
            Logger.Info("CustomerId:""" + P_CommonDr.CustomerId + """")
            Logger.Info("CustomerName:""" + P_CommonDr.CustomerName + """")
            Logger.Info("NameTitle:""" + P_CommonDr.NameTitle + """")
            Logger.Info("NameTitlePosition:""" + P_CommonDr.NameTitlePosition + """")
            Logger.Info("DmsId:""" + P_CommonDr.DmsId + """")
            Logger.Info("ReceptionDivision:""" + P_CommonDr.ReceptionDivision + """")
            Logger.Info("ServiceCode:""" + P_CommonDr.ServiceCode + """")
            Logger.Info("ServiceName:""" + P_CommonDr.ServiceName + """")
            Logger.Info("MerchandiseCode:""" + P_CommonDr.MerchandiseCode + """")
            Logger.Info("MerchandiseName:""" + P_CommonDr.MerchandiseName + """")
            Logger.Info("StoreStatus:""" + P_CommonDr.StoreStatus + """")
            Logger.Info("ReservationStatus:""" + P_CommonDr.ReservationStatus + """")
            Logger.Info("ReservationStatusName:""" + P_CommonDr.ReservationStatus + """")

            'Schedule
            For Each p_scheduleRow As RegistSchedule.P_ScheduleRow In P_ScheduleInfoDt.Rows
                Logger.Info("ActivityStaffBranchCode:""" + p_scheduleRow.ActivityStaffBranchCode + """")
                Logger.Info("ActivityStaffCode:""" + p_scheduleRow.ActivityStaffCode + """")
                Logger.Info("ReceptionStaffBranchCode:""" + p_scheduleRow.ReceptionStaffBranchCode + """")
                Logger.Info("ReceptionStaffCode:""" + p_scheduleRow.ReceptionStaffCode + """")
                Logger.Info("StartTime:""" + p_scheduleRow.StartTime + """")
                Logger.Info("EndTime:""" + p_scheduleRow.EndTime + """")
                Logger.Info("Memo:""" + p_scheduleRow.Memo + """")
                Logger.Info("AlarmNo:""" + p_scheduleRow.AlarmNo + """")
                Logger.Info("ContactNo:""" + p_scheduleRow.ContactNo + """")
                Logger.Info("ContactName:""" + p_scheduleRow.ContactName + """")
                Logger.Info("ComingFollowName:""" + p_scheduleRow.ComingFollowName + """")
                Logger.Info("BackgroundColor:""" + p_scheduleRow.BackGroundColor + """")
                Logger.Info("TodoId:""" + p_scheduleRow.TodoId + """")
                Logger.Info("ProcessDivision:""" + p_scheduleRow.ProcessDivision + """")
                Logger.Info("ProcessName:""" + p_scheduleRow.ProcessName + """")
                Logger.Info("ResultDate:""" + p_scheduleRow.ResultDate + """")
            Next
            ' 2012/02/14 KN 田中 GTMC130118132対応 MODIFY ログ出力をDebug→Error END

        End Sub

    End Class
End Namespace
