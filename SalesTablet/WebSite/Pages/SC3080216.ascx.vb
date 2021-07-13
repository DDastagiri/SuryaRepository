'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3080216.aspx.vb
'─────────────────────────────────────
'機能： 顧客詳細(受注後工程フォロー)
'補足： 
'作成： 2014/02/13 TCS 森 受注後フォロー機能開発
'更新： 2014/03/18 TCS 葛西 切替BTS-210対応
'更新： 2014/04/02 TCS 河原 性能改善
'─────────────────────────────────────

Imports System.Globalization

Imports Toyota.eCRB.SystemFrameworks.Web
Imports System.Reflection.MethodBase
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web.Controls
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess

Imports Toyota.eCRB.CustomerInfo.Details.BizLogic
Imports Toyota.eCRB.CustomerInfo.Details.DataAccess
Imports Toyota.eCRB.CommonUtility.BizLogic
Imports Toyota.eCRB.CommonUtility.DataAccess


''' <summary>
''' 顧客詳細(受注後工程フォロー)
''' Webページのプレゼンテーション層
''' </summary>
''' <remarks>顧客詳細(受注後工程フォロー)</remarks>
Partial Class Pages_SC3080216uc
    Inherits System.Web.UI.UserControl
    Implements ISC3080203Control

#Region " セッションキー "
    ''' <summary>
    '''Follow-upBoxのシーケンスNo.
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONST_FLLWUPBOX_SEQNO As String = "SearchKey.FOLLOW_UP_BOX"

    ''' <summary>
    '''Follow-upBoxの店舗コード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONST_FLLWUPBOX_STRCD As String = "SearchKey.FLLWUPBOX_STRCD"


    ''' <summary>
    '''顧客区分　自社客：1　未取引客：2
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONST_CSTKIND As String = "SearchKey.CSTKIND"

    ''' <summary>
    '''顧客ID　自社客：自社客連番　未取引客：未取引客ユーザID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONST_INSDID As String = "SearchKey.CRCUSTID"

    ''' <summary>
    '''車両ID 自社客：VIN　未取引客：未取引客車両SeqNo
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONST_VCLINFO As String = "VCLINFO"

    ''' <summary>
    '''顧客分類
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_CUSTOMERCLASS As String = "SearchKey.CUSTOMERCLASS"

    ''' <summary>
    '''注文番号
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_ORDER_NO As String = "SearchKey.ORDER_NO"

    ''' <summary>
    '''受注後フラグ（0:受注時、1:受注後）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_SALESAFTER As String = "SearchKey.SALESAFTER"

    ''' <summary>
    '''表示ページ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_DISPPAGE As String = "SearchKey.DISPPAGE"

    ''' <summary>
    ''' メッセージID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MSG_30932 As Integer = 30932

    Private Const MSG_30933 As Integer = 30933
    Private Const MSG_30934 As Integer = 30934

    Private Const MSG_20916 As Integer = 20916

    ''' <summary>商談中Follow-upBox内連番</summary>
    Private Const SESSION_KEY_FOLLOW_UP_BOX_SALES As String = "SearchKey.FOLLOW_UP_BOX_SALES"

    ''' <summary>商談中Follow-upBox店舗コード</summary>
    Private Const SESSION_KEY_FLLWUPBOX_STRCD_SALES As String = "SearchKey.FLLWUPBOX_STRCD_SALES"

    ' 活動ID
    Private Const SESSION_KEY_ACT_ID As String = "SearchKey.ACT_ID"

    ' 用件ID
    Private Const SESSION_KEY_REQ_ID As String = "SearchKey.REQ_ID"

    ' 誘致ID
    Private Const SESSION_KEY_ATT_ID As String = "SearchKey.ATT_ID"

    ' 活動回数
    Private Const SESSION_KEY_COUNT As String = "SearchKey.COUNT"

    ' 用件ロックバージョン
    Private Const SESSION_KEY_REQUEST_LOCK_VERSION As String = "SearchKey.REQUEST_LOCK_VERSION"

    ' 誘致ロックバージョン
    Private Const SESSION_KEY_ATTRACT_LOCK_VERSION As String = "SearchKey.ATTRACT_LOCK_VERSION"

    ' 商談ロックバージョン
    Private Const SESSION_KEY_SALES_LOCK_VERSION As String = "SearchKey.SALES_LOCK_VERSION"

#End Region

#Region " 定数 "
    ''' <summary>
    ''' カタログ用SEQ
    ''' </summary>
    ''' <remarks></remarks>
    Public Const CONTENT_SEQ_CATALOG As Integer = 9

    ''' <summary>
    ''' 試乗用SEQ
    ''' </summary>
    ''' <remarks></remarks>
    Public Const CONTENT_SEQ_TESTDRIVE As Integer = 16

    ''' <summary>
    ''' 査定用SEQ
    ''' </summary>
    ''' <remarks></remarks>
    Public Const CONTENT_SEQ_ASSESSMENT As Integer = 18

    ''' <summary>
    ''' 画面で選択する活動結果
    ''' </summary>
    ''' <remarks></remarks>
    Public Const C_RSLT_WALKIN As String = "1"
    Public Const C_RSLT_PROSPECT As String = "2"
    Public Const C_RSLT_HOT As String = "3"
    Public Const C_RSLT_SUCCESS As String = "4"
    Public Const C_RSLT_GIVEUP As String = "5"

    ''' <summary>
    ''' プログラムＩＤ
    ''' </summary>
    ''' <remarks></remarks>
    Private Shared MY_PROGRAMID As String = "SC3080216"

    ''' <summary>
    ''' 1ページの表示件数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PAGE_LINE As Integer = 19

    ''' <summary>
    ''' メッセージ出力時の日付フォーマット 年月日時分
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DATE_FORMAT_YMDHS_CONVID As Integer = 2

    ''' <summary>
    ''' 契約活動コード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ENVSETTINGKEY_AFTER_ODR_CONTRACT As String = "AFTER_ODR_ACT_CD_CONTRACT" '契約

    ''' <summary>
    ''' 活動方法
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SC308216SELECT_ACT_CONTACT_SIX As String = "6"

    ''' <summary>
    ''' 活動予定/実績時間設定フラグ ON
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DATEORTIME_FLG_SET_TIME_ON As String = "1"

#End Region

#Region " 変数 "
    '日時リスト変数
    Private DaysHeaderShowList As New List(Of String)
    '工程リスト変数
    Private PrcsHeaderShowList As New List(Of String)
#End Region

#Region "イベント類"
    ''' <summary>
    ''' 初期表示
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Logger.Info("Page_Load Start")

        If Me.Visible Then

            If Not Page.IsPostBack Then

                '初期設定
                InitDisplaySetting()

            End If

        End If

        Logger.Info("Page_Load End")

    End Sub

#End Region

#Region "メソット類"
    ''' <summary>
    ''' 初期設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitDisplaySetting()

        Logger.Info("InitDisplaySetting Start")

        'セッション情報取得
        GetSessionValues()

        'TODO:共通後に変更する
        SC308216selectActContact.Value = SC308216SELECT_ACT_CONTACT_SIX

        '文言マスタより文言取得
        SetLabelText()

        '対応SC欄の設定
        SetStaffSelector()

        Dim msgId As Integer = 0
        HeaderTitleDays.Value = ""
        HeaderTitlePrcs.Value = ""

        Using inputTbl As New SC3080216DataSet.SC3080216PlanDataTable

            'セッション値の設定
            SetDataTable(inputTbl)

            '初期表示情報取得
            Dim inputRow As SC3080216DataSet.SC3080216PlanRow = inputTbl.Item(0)
            SC3080216BusinessLogic.GetInitialize(inputRow, msgId)

            '受注日(契約完了日)
            Dim salesDt As Date
            If (inputRow.IsSALESBKGDATENull) Then
                salesDt = Today
            Else
                salesDt = inputRow.SALESBKGDATE
            End If

            '計画アイコン情報をセットする (その２)
            SetIconData2(salesDt, inputRow)

            'プロセス欄表示有無
            If (inputRow.SALESAFTERFLG.Equals(SC3080216BusinessLogic.SalesFlg)) Then
                '受注時 　1:表示
                SC308216processFlg.Value = "1"
            Else
                '受注後 　0:非表示
                SC308216processFlg.Value = "0"
            End If

            '活動内容のユーザーコントロール値設定
            SetActiveInfo()

            '受注後工程が使用可能かを判定する
            If (ActivityInfoBusinessLogic.CheckUsedB2D()) Then
                Me.useB2DPanel.Visible = True
            Else
                Me.useB2DPanel.Visible = False
            End If

            '契約活動コード取得
            ActContractCode.Value = SC3080216BusinessLogic.GetSysEnvSettingValue(ENVSETTINGKEY_AFTER_ODR_CONTRACT)


            '受注後工程活動情報取得(日時別)
            Dim SC3080216_AfterAct_days = SC3080216BusinessLogic.GetBookedafterActivityInfo(inputRow.FLLWUPBOX_SEQNO, 1)

            '日付を MM/DD に変換
            For Each SC3080216_AfterAct_days_rows In SC3080216_AfterAct_days

                '活動予定/実績開始～終了時間が設定されている場合、時分まで表示する
                If SC3080216_AfterAct_days_rows.DATEORTIME_FLG.Equals(DATEORTIME_FLG_SET_TIME_ON) Then
                    '日時指定あり

                    '当日
                    If Date.Today = SC3080216_AfterAct_days_rows.START_DATEORTIME.Date Then

                        SC3080216_AfterAct_days_rows.DATEORTIME = SC3080216_AfterAct_days_rows.START_DATEORTIME.ToString("HH:mm") + " - " +
                        SC3080216_AfterAct_days_rows.END_DATEORTIME.ToString("HH:mm")

                    Else
                        '当日以外
                        If Date.Today.AddDays(-1) = SC3080216_AfterAct_days_rows.START_DATEORTIME.Date Then

                            '昨日
                            SC3080216_AfterAct_days_rows.DATEORTIME =
                                DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, SC3080216_AfterAct_days_rows.START_DATEORTIME, Date.Now, StaffContext.Current.DlrCD) +
                            SC3080216_AfterAct_days_rows.START_DATEORTIME.ToString(" HH:mm") + " - " +
                            SC3080216_AfterAct_days_rows.END_DATEORTIME.ToString("HH:mm")



                        Else

                            'それ以外
                            SC3080216_AfterAct_days_rows.DATEORTIME =
                                SC3080216_AfterAct_days_rows.START_DATEORTIME.ToString("dd/MM HH:mm") + " - " +
                            SC3080216_AfterAct_days_rows.END_DATEORTIME.ToString("HH:mm")

                        End If

                    End If

                Else
                    '日時指定なし

                    If Date.Today = SC3080216_AfterAct_days_rows.START_DATEORTIME.Date Then

                        SC3080216_AfterAct_days_rows.DATEORTIME =
                            DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, SC3080216_AfterAct_days_rows.START_DATEORTIME, Date.Now, StaffContext.Current.DlrCD, False)

                    Else
                        '当日以外
                        If Date.Today.AddDays(-1) = SC3080216_AfterAct_days_rows.START_DATEORTIME.Date Then

                            '昨日
                            SC3080216_AfterAct_days_rows.DATEORTIME =
                                DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, SC3080216_AfterAct_days_rows.START_DATEORTIME, Date.Now, StaffContext.Current.DlrCD, False)

                        Else

                            'それ以外
                            SC3080216_AfterAct_days_rows.DATEORTIME =
                                SC3080216_AfterAct_days_rows.START_DATEORTIME.ToString("dd/MM")

                        End If

                    End If

                End If

                'チェックボックス表示判定
                If SC3080216_AfterAct_days_rows.AFTER_ODR_ACT_INPUT_TYPE.Equals("1") Then
                    SC3080216_AfterAct_days_rows.CHECK_BOX_FLG = "1"

                Else
                    SC3080216_AfterAct_days_rows.CHECK_BOX_FLG = "0"
                End If

                ''ヘッダー部非表示フラグ準備
                If SC3080216_AfterAct_days_rows.COMPLETION_FLG <> "1" Then
                    If DaysHeaderShowList.IndexOf(SC3080216_AfterAct_days_rows.DATEORTIME) < 0 Then
                        '現在の日時が含まれていない場合のみ追加
                        DaysHeaderShowList.Add(SC3080216_AfterAct_days_rows.DATEORTIME)
                    End If
                End If

            Next

            Me.SC3080216_AfterActivityDaysRepeater.DataSource = SC3080216_AfterAct_days
            Me.SC3080216_AfterActivityDaysRepeater.DataBind()

            '受注後工程活動情報取得(工程別)
            Dim SC3080216_AfterAct_prcs = SC3080216BusinessLogic.GetBookedafterActivityInfo(inputRow.FLLWUPBOX_SEQNO, 2)

            '日付を MM/DD に変換
            For Each SC3080216_AfterAct_prcs_rows In SC3080216_AfterAct_prcs

                '活動予定/実績開始～終了時間が設定されている場合、時分まで表示する
                If SC3080216_AfterAct_prcs_rows.DATEORTIME_FLG.Equals(DATEORTIME_FLG_SET_TIME_ON) Then

                    '日時指定あり

                    '当日
                    If Date.Today = SC3080216_AfterAct_prcs_rows.START_DATEORTIME.Date Then

                        SC3080216_AfterAct_prcs_rows.DATEORTIME = SC3080216_AfterAct_prcs_rows.START_DATEORTIME.ToString("HH:mm")

                    Else

                        '当日以外
                        If Date.Today.AddDays(-1) = SC3080216_AfterAct_prcs_rows.START_DATEORTIME.Date Then

                            '昨日
                            SC3080216_AfterAct_prcs_rows.DATEORTIME =
                                DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, SC3080216_AfterAct_prcs_rows.START_DATEORTIME, Date.Now, StaffContext.Current.DlrCD) +
                                    SC3080216_AfterAct_prcs_rows.START_DATEORTIME.ToString(" HH:mm")

                        Else
                            'それ以外
                            SC3080216_AfterAct_prcs_rows.DATEORTIME = SC3080216_AfterAct_prcs_rows.START_DATEORTIME.ToString("dd/MM HH:mm")

                        End If

                    End If

                Else
                    '日時指定なし

                    '当日
                    If Date.Today = SC3080216_AfterAct_prcs_rows.START_DATEORTIME.Date Then

                        SC3080216_AfterAct_prcs_rows.DATEORTIME =
                           DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, SC3080216_AfterAct_prcs_rows.START_DATEORTIME, Date.Now, StaffContext.Current.DlrCD, False)

                    Else

                        '当日以外
                        If Date.Today.AddDays(-1) = SC3080216_AfterAct_prcs_rows.START_DATEORTIME.Date Then

                            '昨日
                            SC3080216_AfterAct_prcs_rows.DATEORTIME =
                                DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, SC3080216_AfterAct_prcs_rows.START_DATEORTIME, Date.Now, StaffContext.Current.DlrCD, False)

                        Else
                            'それ以外
                            SC3080216_AfterAct_prcs_rows.DATEORTIME = SC3080216_AfterAct_prcs_rows.START_DATEORTIME.ToString("dd/MM")

                        End If

                    End If

                End If

                'チェックボックス表示判定
                If SC3080216_AfterAct_prcs_rows.AFTER_ODR_ACT_INPUT_TYPE.Equals("1") Then
                    SC3080216_AfterAct_prcs_rows.CHECK_BOX_FLG = "1"

                Else
                    SC3080216_AfterAct_prcs_rows.CHECK_BOX_FLG = "0"
                End If

                'ヘッダー部非表示フラグ準備
                If SC3080216_AfterAct_prcs_rows.COMPLETION_FLG <> "1" Then
                    If PrcsHeaderShowList.IndexOf(SC3080216_AfterAct_prcs_rows.AFTER_ODR_PRCS_CD) < 0 Then
                        '現在の工程が含まれていない場合のみ追加
                        PrcsHeaderShowList.Add(SC3080216_AfterAct_prcs_rows.AFTER_ODR_PRCS_CD)
                    End If
                End If

            Next

            Me.SC3080216_AfterActivityPrcsRepeater.DataSource = SC3080216_AfterAct_prcs
            Me.SC3080216_AfterActivityPrcsRepeater.DataBind()

        End Using

        '2014/04/02 TCS 河原 性能改善 Start
        Sc3080218Page.SetInit()
        '2014/04/02 TCS 河原 性能改善 End

        Logger.Info("InitDisplaySetting End")

    End Sub


    ''' <summary>
    ''' アイコン情報をセットする (その２)
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetIconData2(ByVal salesDt As Date, ByVal inputRow As SC3080216DataSet.SC3080216PlanRow)

        Logger.Info("SetIconData2 Start")


        '来店人数
        If (inputRow.IsWALKINNUMNull) Then
            SC308216walkinNum.Value = String.Empty
        Else
            SC308216walkinNum.Value = CType(inputRow.WALKINNUM, String)
        End If

        '新規活動フラグ
        SC308216newFllwFlg.Value = inputRow.NEWFLLWUPBOXFLG

        '商談開始時間
        If (Not inputRow.IsSALESSTARTTIMENull()) Then
            SC308216ActTimeFromSelectorWK.Value = inputRow.SALESSTARTTIME
        Else
            '営業活動開始時間
            If (Not inputRow.IsSALESSTARTTIMENull()) Then
                SC308216ActTimeFromSelectorWK.Value = inputRow.EIGYOSTARTTIME
            Else
                SC308216ActTimeFromSelectorWK.Value = Now
            End If
        End If

        '商談終了時間
        If (inputRow.IsSALESENDTIMENull()) Then
            '開始時間＋１時間
            SC308216ActTimeToSelectorWK.Value = SC308216ActTimeFromSelectorWK.Value.Value.AddHours(1)
            '+1hの時は書き込み可否フラグを設定
            Me.SC3080216UpdateRWFlg.Value = "1"
        Else
            SC308216ActTimeToSelectorWK.Value = inputRow.SALESENDTIME
        End If

        Logger.Info("SetIconData2 End")

    End Sub

    ''' <summary>
    ''' 文字列を日付型に変換する
    ''' </summary>
    ''' <remarks></remarks>
    Private Function ChangeDate(ByVal dtString As String) As Date

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
    ''' 文言マスタより文言取得・設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetLabelText()

        Logger.Info("SetLabelText Start")

        SC3080216_TitleLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 18))
        SC3080216_ToDoLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 19))
        SC3080216_AllLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 20))
        SC3080216_TimeLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 21))
        SC3080216_ProcessLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 22))
        ActCheckOffMsg.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 908))

        Logger.Info("SetLabelText End")

    End Sub


    ''' <summary>
    ''' セッションの値をDataRowにセットする。
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub SetDataTable(ByVal dataTbl As SC3080216DataSet.SC3080216PlanDataTable)

        Logger.Info("SetDataTable Start")

        Dim dataRow As SC3080216DataSet.SC3080216PlanRow
        dataRow = dataTbl.NewSC3080216PlanRow()
        dataTbl.AddSC3080216PlanRow(dataRow)

        'ログインユーザー情報取得用
        Dim context As StaffContext = StaffContext.Current

        Dim dlrcd As String = context.DlrCD         '自身の販売店コード
        Dim strcd As String = context.BrnCD         '自身の店舗コード
        Dim account As String = context.Account     '自身のアカウント
        Dim strcdstaff As String = context.BrnCD    '自身の店舗コード (スタッフ店舗コード)
        Dim staffcd As String = context.Account     '自身のアカウント (スタッフコード)

        '販売店コード
        dataRow.DLRCD = dlrcd

        'アカウント
        dataRow.ACCOUNT = account

        'SeqNo取得
        If (String.IsNullOrEmpty(SC308216fllwSeq.Value) = True) Then
            dataRow.FLLWUPBOX_SEQNO = 0
        Else
            dataRow.FLLWUPBOX_SEQNO = CType(SC308216fllwSeq.Value, Long)
        End If

        '店舗コード
        If String.IsNullOrEmpty(SC308216fllwstrcd.Value) Then
            dataRow.STRCD = strcd
        Else
            dataRow.STRCD = SC308216fllwstrcd.Value
        End If

        '顧客区分
        dataRow.CUSTSEGMENT = DirectCast(GetValue(ScreenPos.Current, CONST_CSTKIND, False), String)

        '顧客分類
        dataRow.CUSTOMERCLASS = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CUSTOMERCLASS, False), String)

        '活動先顧客コード
        dataRow.CRCUSTID = DirectCast(GetValue(ScreenPos.Current, CONST_INSDID, False), String)

        '顧客種別
        SC308216cstkind.Value = dataRow.CUSTSEGMENT

        '活動先顧客コード
        SC308216insdid.Value = dataRow.CRCUSTID

        '注文番号
        dataRow.SALESBKGNO = CType(GetValue(ScreenPos.Current, SESSION_KEY_ORDER_NO, False), String)
        '注文番号が空か、ダミーの番号の場合空白にする
        If String.IsNullOrEmpty(dataRow.SALESBKGNO) Or String.Equals(dataRow.SALESBKGNO, "0") Then
            dataRow.SALESBKGNO = " "
        End If

        '受注後フラグ
        dataRow.SALESAFTERFLG = CType(GetValue(ScreenPos.Current, SESSION_KEY_SALESAFTER, False), String)

        Logger.Info("SetDataTable End")

    End Sub


    ''' <summary>
    ''' セッションの値をDataRowに設定する
    ''' </summary>
    ''' <param name="dataRow"></param>
    ''' <remarks></remarks>
    Private Sub SetDataRows(ByVal dataRow As SC3080216DataSet.SC3080216UpdBookdafActiveRow)

        Logger.Info("SetDataRows Start")

        Logger.Info("SC3080216 SetDataRows セッション値設定 Start")

        'ログインユーザー情報取得用
        Dim context As StaffContext = StaffContext.Current

        Dim dlrcd As String = context.DlrCD         '自身の販売店コード
        Dim strcd As String = context.BrnCD         '自身の店舗コード
        Dim account As String = context.Account     '自身のアカウント
        Dim strcdstaff As String = context.BrnCD    '自身の店舗コード (スタッフ店舗コード)
        Dim staffcd As String = context.Account     '自身のアカウント (スタッフコード)

        '販売店コード
        dataRow.RSLT_DLR_CD = dlrcd

        Logger.Info("販売店コード")
        Logger.Info(dataRow.RSLT_DLR_CD)

        Logger.Info("店舗コード")

        '店舗コード
        If String.IsNullOrEmpty(SC308216fllwstrcd.Value) Then
            dataRow.RSLT_BRN_CD = strcd
            Logger.Info("スタッフコンテキスト" + strcd)
        Else
            dataRow.RSLT_BRN_CD = SC308216fllwstrcd.Value
            Logger.Info("セッション値" + SC308216fllwstrcd.Value)
        End If


        'スタッフコード
        dataRow.RSLT_STF_CD = staffcd
        Logger.Info("スタッフコード" + staffcd)

        '組織IDコード
        dataRow.RSLT_ORGNZ_ID = context.TeamCD

        dataRow.ACT_ID = 0
        dataRow.AFTER_ODR_FLLW_SEQ = 0

        Logger.Info("SC3080216 SetDataRows セッション値設定 End")

        Logger.Info("SetDataRows End")

    End Sub


    ''' <summary>
    ''' セッション情報取得
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetSessionValues()

        Logger.Info("GetSessionValues Start")

        'フォローアップボックスSeqNo取得
        If ContainsKey(ScreenPos.Current, CONST_FLLWUPBOX_SEQNO) Then
            SC308216fllwSeq.Value = DirectCast(GetValue(ScreenPos.Current, CONST_FLLWUPBOX_SEQNO, False), String)
        Else
            SC308216fllwSeq.Value = ""
        End If

        'フォローアップボックスの店舗コード
        If ContainsKey(ScreenPos.Current, CONST_FLLWUPBOX_STRCD) Then
            SC308216fllwstrcd.Value = DirectCast(GetValue(ScreenPos.Current, CONST_FLLWUPBOX_STRCD, False), String)
        Else
            SC308216fllwstrcd.Value = ""
        End If

        '1枚目で選んでいる車両
        If ContainsKey(ScreenPos.Current, CONST_VCLINFO) Then
            SC308216Vclseq.Value = DirectCast(GetValue(ScreenPos.Current, CONST_VCLINFO, False), String)
        Else
            SC308216Vclseq.Value = ""
        End If

        Logger.Info("GetSessionValues End")
    End Sub

#End Region

#Region "プロセス関連"

    ''' <summary>
    ''' 対応SC欄のセット
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Function SetStaffSelector() As Boolean
        Logger.Info("setStaffSelector Start")

        Dim userary As String()
        Dim context As StaffContext = StaffContext.Current
        userary = Split(context.Account, "@")
        SC308216selectStaff.Value = userary(0)

        Logger.Info("setStaffSelector End")

        Return True

    End Function

    ''' <summary>
    ''' 活動内容のユーザーコントロール値設定
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Function SetActiveInfo() As Boolean

        Logger.Info("SetActiveInfo Start")

        '活動開始日時
        Sc3080218Page.ActTimeFrom = SC308216ActTimeFromSelectorWK.Value

        '活動終了時間
        If Me.SC3080216UpdateRWFlg.Value = "0" Then
            Sc3080218Page.ActTimeTo = SC308216ActTimeToSelectorWK.Value
        End If

        '対応SC
        Sc3080218Page.SelectStaff = SC308216selectStaff.Value

        '活動方法
        Sc3080218Page.SelectActContact = SC308216selectActContact.Value

        'プロセス有無
        Sc3080218Page.ProcessFlg = SC308216processFlg.Value

        '受注後フラグ
        Dim SalesFlg As String = CType(GetValue(ScreenPos.Current, SESSION_KEY_SALESAFTER, False), String)
        If (SalesFlg.Equals(SC3080216BusinessLogic.SalesFlg)) Then
            Sc3080218Page.BookedFlg = SC3080216BusinessLogic.SalesFlg
        Else
            Sc3080218Page.BookedFlg = String.Empty
        End If

        '再設定処理
        Sc3080218Page.RefreshDisplay()

        Logger.Info("SetActiveInfo End")

        Return True

    End Function

#End Region

#Region " ページクラス処理のバイパス処理 "
    Private Sub SetValue(ByVal pos As Toyota.eCRB.SystemFrameworks.Web.ScreenPos, ByVal key As String, ByVal value As Object)
        GetPageInterface().SetValueBypass(pos, key, value)
    End Sub

    Private Function GetValue(ByVal pos As ScreenPos, ByVal key As String, ByVal removeFlg As Boolean) As Object
        Return GetPageInterface().GetValueBypass(pos, key, removeFlg)
    End Function

    Private Sub ShowMessageBox(ByVal wordNo As Integer, ByVal ParamArray wordParam() As String)
        GetPageInterface().ShowMessageBoxBypass(wordNo, wordParam)
    End Sub

    Private Function ContainsKey(ByVal pos As Toyota.eCRB.SystemFrameworks.Web.ScreenPos, ByVal key As String) As Boolean
        Return GetPageInterface().ContainsKeyBypass(pos, key)
    End Function

    Private Function GetPageInterface() As ICustomerDetailControl
        Return CType(Me.Page, ICustomerDetailControl)
    End Function
#End Region

#Region "活動関係"

    ''' <summary>
    ''' 活動完了イベント
    ''' </summary>
    Public Event SuccessActivity(ByVal sender As Object, ByVal e As System.EventArgs) Implements Toyota.eCRB.CustomerInfo.Details.BizLogic.ISC3080203Control.SuccessActivity

    ''' <summary>
    ''' 活動継続イベント
    ''' </summary>
    Public Event ContinueActivity(ByVal sender As Object, ByVal e As System.EventArgs) Implements Toyota.eCRB.CustomerInfo.Details.BizLogic.ISC3080203Control.ContinueActivity

    ''' <summary>
    ''' 商談画面で活動が変更された際に呼び出されるメソッド
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ChangeFollow() Implements Toyota.eCRB.CustomerInfo.Details.BizLogic.ISC3080203Control.ChangeFollow

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SC3080216 ChangeFollow Start")

        If Me.ContainsKey(ScreenPos.Current, CONST_FLLWUPBOX_SEQNO) AndAlso Me.ContainsKey(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX_SALES) Then
            Dim fllwSeq As String = DirectCast(GetValue(ScreenPos.Current, CONST_FLLWUPBOX_SEQNO, False), String)
            Dim fllwStrcd As String = DirectCast(GetValue(ScreenPos.Current, CONST_FLLWUPBOX_STRCD, False), String)
            Dim salesfllwSeq As String = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX_SALES, False), String)
            Dim salesfllwStrcd As String = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_FLLWUPBOX_STRCD_SALES, False), String)
            '現在表示中の活動と、商談中の活動が一致する場合のみ、初期設定を行う
            If (fllwSeq = salesfllwSeq) Then
                '初期設定
                InitDisplaySetting()
            End If
        End If

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SC3080216 ChangeFollow End")

    End Sub

    ''' <summary>
    ''' 活動結果登録ボタン押下時にコールされるメソッド
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub RegistActivity() Implements Toyota.eCRB.CustomerInfo.Details.BizLogic.ISC3080203Control.RegistActivity

        Logger.Info("SC3080216 RegistActivity Start")


        Dim msgId As Integer = 0
        Dim msgItem0 As String = String.Empty

        'データ行設定
        '共通項目の設定
        Using inputTbl As New SC3080216DataSet.SC3080216PlanDataTable

            SetDataTable(inputTbl)

            Dim inputRow As SC3080216DataSet.SC3080216PlanRow = inputTbl.Item(0)

            '初期値取得

            '対応アカウント
            Dim context As StaffContext = StaffContext.Current
            inputRow.ACTUALACCOUNT = Sc3080218Page.SelectStaff & "@" & context.DlrCD

            '接触方法No
            inputRow.CONTACTNO = CType(Sc3080218Page.SelectActContact, Long)

            '商談開始時間
            inputRow.SALESSTARTTIME = ChangeDate(Sc3080218Page.ActTimeFrom.ToString("yyyy/MM/dd HH:mm", CultureInfo.CurrentCulture) + ":00")

            '終了日時
            inputRow.SALESENDTIME = ChangeDate(Sc3080218Page.ActTimeFrom.ToString("yyyy/MM/dd", CultureInfo.CurrentCulture) + " " + Sc3080218Page.ActTimeTo.ToString("HH:mm", CultureInfo.CurrentCulture) + ":00")

            '来店人数
            If (String.IsNullOrEmpty(SC308216walkinNum.Value)) Then
                inputRow.SetWALKINNUMNull()
            Else
                inputRow.WALKINNUM = CType(SC308216walkinNum.Value, Short)
            End If

            '新規活動フラグ
            inputRow.NEWFLLWUPBOXFLG = CType(SC308216newFllwFlg.Value, Short)

            '前回までの活動終了時間で最新の時間
            Dim actTimeEnd As Date
            If GetLatestActTimeEnd(actTimeEnd) Then
                inputRow.LATEST_TIME_END = actTimeEnd
            End If

            Dim retValue As Boolean = False

            Dim custKind As String = DirectCast(GetValue(ScreenPos.Current, CONST_CSTKIND, False), String)
            Dim custId As String = DirectCast(GetValue(ScreenPos.Current, CONST_INSDID, False), String)

            '未取引客情報取得
            Dim newCustDt As ActivityInfoDataSet.GetNewCustomerDataTable = ActivityInfoBusinessLogic.GetNewcustomer(custId)

            '入力チェック
            If (SC3080216BusinessLogic.IsInputeCheck(inputTbl, newCustDt, custKind, msgId, msgItem0) = False) Then
                SetValue(ScreenPos.Current, SESSION_KEY_DISPPAGE, "3")

                If MSG_30933 = msgId Or MSG_30934 = msgId Then
                    ShowMessageBox(msgId)
                    'スクリプトの登録
                    JavaScriptUtility.RegisterStartupFunctionCallScript(Me.Page, "newCustomerDummyErrorAfter", "after")
                ElseIf MSG_30932 = msgId AndAlso inputRow.IsLATEST_TIME_ENDNull = False Then
                    Dim param As String = DateTimeFunc.FormatDate(DATE_FORMAT_YMDHS_CONVID, inputRow.LATEST_TIME_END)
                    ShowMessageBox(msgId, New String() {param})
                ElseIf MSG_20916 = msgId Then
                    ShowMessageBox(msgId, New String() {msgItem0})
                Else
                    ShowMessageBox(msgId)
                End If

                Return
            End If

            '現在のステータスを取得する
            Dim staffStatus As String = SC3080216BusinessLogic.GetStaffStatus

            If (inputRow.SALESAFTERFLG.Equals(SC3080216BusinessLogic.SalesFlg)) Then

                '受注時、活動結果登録
                Using activDataTbl As New ActivityInfoDataSet.ActivityInfoRegistDataDataTable

                    '登録データ設定
                    SetRegistData(inputTbl, activDataTbl)


                    '2014/03/18 TCS 葛西 切替BTS-210対応 START
                    Dim bizLogic As New SC3080216BusinessLogic
                    retValue = bizLogic.UpdateSales(inputTbl, activDataTbl, msgId)
                    '2014/03/18 TCS 葛西 切替BTS-210対応 END

                End Using

            Else
                '受注後、活動結果登録
                Dim vclid As Decimal
                Dim actid As Decimal
                If (Not String.IsNullOrEmpty(SC308216Vclseq.Value)) Then
                    vclid = CDec(SC308216Vclseq.Value)
                Else
                    vclid = 0
                End If
                If ContainsKey(ScreenPos.Current, SESSION_KEY_ACT_ID) Then
                    actid = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_ACT_ID, False), Decimal)
                Else
                    actid = 0
                End If

                '2014/03/18 TCS 葛西 切替BTS-210対応 START
                Dim bizLogic As New SC3080216BusinessLogic
                retValue = bizLogic.UpdateSalesAfter(inputTbl, vclid, actid)
                '2014/03/18 TCS 葛西 切替BTS-210対応 END
            End If

            If retValue = False Then
                '登録処理エラー
                ShowMessageBox(msgId)
                Return
            End If

            '登録対象データを設定する
            '共通項目の設定
            Dim input As New SC3080216DataSet.SC3080216UpdBookdafActiveDataTable
            Dim rows As SC3080216DataSet.SC3080216UpdBookdafActiveRow
            Dim actCalDavDate As New SC3080216BusinessLogic.CalDavDate

            rows = input.NewSC3080216UpdBookdafActiveRow()
            Dim sales_id As Decimal = DirectCast(GetValue(ScreenPos.Current, CONST_FLLWUPBOX_SEQNO, False), String)

            Logger.Info("SC3080216 RegistActivity sales_id")
            Logger.Info(CType(sales_id, String))

            '受注後フラグ
            Dim salesAfterFlg = CType(GetValue(ScreenPos.Current, SESSION_KEY_SALESAFTER, False), String)

            Logger.Info("SC3080216 RegistActivity salesAfterFlg")
            Logger.Info(salesAfterFlg)

            If Not UpdAfterActCdList.Value.Equals("") Then

                Logger.Info("SC3080216 RegistActivity AFTER_ODR_ACT_ID -  AFTER_ODR_ACT_STATUS")
                Logger.Info(UpdAfterActCdList.Value)

                For Each actCode As String In UpdAfterActCdList.Value.Split(",")
                    rows = input.NewRow()

                    rows.AFTER_ODR_ACT_ID = CType(actCode.Split("-").ElementAt(0), Decimal)
                    rows.AFTER_ODR_ACT_STATUS = actCode.Split("-").ElementAt(1)

                    SetDataRows(rows)

                    rows.RSLT_START_DATEORTIME = inputRow.SALESSTARTTIME
                    rows.RSLT_END_DATEORTIME = inputRow.SALESENDTIME
                    rows.RSLT_CONTACT_MTD = inputRow.CONTACTNO

                    '完了フラグが完了の場合、登録データをセットする
                    If rows.AFTER_ODR_ACT_STATUS.Equals("1") Then
                        rows.RSLT_DATEORTIME_FLG = "1"
                    Else
                        rows.RSLT_DATEORTIME_FLG = "0"
                    End If

                    rows.ROW_UPDATE_ACCOUNT = rows.RSLT_STF_CD
                    rows.ROW_UPDATE_DATETIME = inputRow.SALESENDTIME
                    rows.ROW_UPDATE_FUNCTION = MY_PROGRAMID

                    input.Rows.Add(rows)

                Next

                '商談IDをセット
                actCalDavDate.SalesId = sales_id

                'CalDAV連携情報を設定
                actCalDavDate = SetCalDavTag(actCalDavDate)

                '受注後工程活動更新
                SC3080216BusinessLogic.UpdateBookedafterActivityInfo(sales_id, StaffContext.Current.Account, input, salesAfterFlg,
                                                                     actCalDavDate)

            Else
                '受注後工程フォロー画面で変更はないが、バックオフィス、受注時説明ツールで活動結果登録した場合
                '受注後工程連番を付与する
                SC3080216BusinessLogic.UpdateLinkBookedafterActivityInfoBp(sales_id)

            End If


            '来店実績更新_商談終了時のPush送信
            SC3080216BusinessLogic.PushUpdateVisitSalesEnd(staffStatus)

        End Using

        RaiseEvent SuccessActivity(Me, EventArgs.Empty)

        Logger.Info("SC3080216 RegistActivity End")

    End Sub

    ''' <summary>
    ''' 商談画面で希望車種が変更された場合に呼び出されるメソッド<br/>
    ''' 活動登録画面の希望車種情報を更新します。(商談画面で選択された希望車種にあわせる)
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub UpdateActivityResult() Implements Toyota.eCRB.CustomerInfo.Details.BizLogic.ISC3080203Control.UpdateActivityResult

        '受注以降この処理はない

    End Sub

    ''' <summary>
    ''' 活動登録処理用のDataSetを作成する
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub SetRegistData(ByVal inputTbl As SC3080216DataSet.SC3080216PlanDataTable, ByVal registTbl As ActivityInfoDataSet.ActivityInfoRegistDataDataTable)

        Logger.Info("SetRegistData Start")

        Dim registRw As ActivityInfoDataSet.ActivityInfoRegistDataRow

        '行を作成
        registRw = registTbl.NewActivityInfoRegistDataRow

        Dim inputRw As SC3080216DataSet.SC3080216PlanRow = inputTbl.Item(0)
        registRw.CRCUSTID = inputRw.CRCUSTID
        registRw.CUSTSEGMENT = inputRw.CUSTSEGMENT

        '活動日
        registRw.ACTDAYFROM = String.Empty
        'FROM
        If (Not inputRw.IsSALESSTARTTIMENull) Then
            '開始日時
            registRw.ACTDAYFROM = inputRw.SALESSTARTTIME.ToString("yyyy/MM/dd HH:mm", CultureInfo.CurrentCulture)
        End If
        'TO
        If (Not inputRw.IsSALESENDTIMENull) Then
            '終了日時
            registRw.ACTDAYTO = inputRw.SALESENDTIME.ToString("HH:mm", CultureInfo.CurrentCulture)
        End If

        '前回までの活動終了時間で最新の時間
        Dim actTimeEnd As Date
        If GetLatestActTimeEnd(actTimeEnd) Then
            registRw.LATEST_TIME_END = actTimeEnd
        End If

        'アカウント
        registRw.ACTACCOUNT = Sc3080218Page.SelectStaff
        'コンタクト方法
        registRw.ACTCONTACT = inputRw.CONTACTNO
        'プロセス有無フラグ
        registRw.PROCESSFLG = Sc3080218Page.ProcessFlg

        'プロセス欄
        registRw.SELECTACTCATALOG = Sc3080218Page.selectActCatalog
        registRw.SELECTACTTESTDRIVE = Sc3080218Page.selectActTestDrive

        '画面で入力された結果を格納
        registRw.SELECTACTASSESMENT = Sc3080218Page.selectActAssesment

        registRw.SELECTACTVALUATION = Sc3080218Page.selectActValuation

        '4:受注
        registRw.ACTRESULT = C_RSLT_SUCCESS

        '------------------------
        ' 次回活動
        '------------------------
        registRw.NEXTACTCONTACT = "1"

        'From-Toフラグ
        registRw.NEXTACTDAYTOFLG = "0"

        '時間指定フラグ(デフォルトをFalseにする)
        registRw.NEXTACTTIMEFLG = False

        '--------------------
        '次回活動日設定
        '--------------------
        '翌日の日付を取得
        Dim staffInfo As StaffContext = StaffContext.Current
        Dim dbNow As Date = DateTimeFunc.Now(staffInfo.DlrCD)
        Dim NowDate As Date
        NowDate = DateAdd(DateInterval.Day, 1, dbNow)

        '現在の時間を取得
        Dim NowHour As String = Hour(dbNow).ToString("00")
        '現在の分を取得
        Dim NowMinute As String = Minute(dbNow).ToString("00")

        Dim fromHour As String = NowHour
        Dim toHour As String = (CInt(NowHour) + 1).ToString("00")

        If String.Equals(NowHour, "22") Or String.Equals(NowHour, "23") Then
            '現在が23時台か22時台の場合
            fromHour = "23:00"
            toHour = "23:59"
        Else
            If CInt(NowMinute) <> 0 Then
                fromHour = (CInt(NowHour) + 1).ToString("00")
                toHour = (CInt(NowHour) + 2).ToString("00")
            End If
            fromHour = fromHour & ":00"
            toHour = toHour & ":00"
        End If

        '時間を切り捨て
        Dim truncNow As String = NowDate.ToString("yyyy/MM/dd")

        Dim initStartDate As Date = Date.ParseExact(truncNow & " " & fromHour, "yyyy/MM/dd HH:mm", Nothing)
        Dim initEndDate As Date = Date.ParseExact(truncNow & " " & toHour, "yyyy/MM/dd HH:mm", Nothing)

        'FROM
        registRw.NEXTACTDAYFROM = initStartDate.ToString("yyyy/MM/dd") & " 00:00"

        'TO
        registRw.NEXTACTDAYTO = registRw.ACTDAYTO

        '------------------------
        ' フォロー
        '------------------------

        'フォロー有無しフラグ
        registRw.FOLLOWFLG = "0"

        'コンタクト方法
        registRw.FOLLOWCONTACT = "0"

        'From-Toフラグ
        registRw.FOLLOWDAYTOFLG = "0"

        '時間指定フラグ(デフォルトをFalseにする)
        registRw.FOLLOWTIMEFLG = False

        'None選択の場合は、フォロー無しを設定
        registRw.FOLLOWFLG = "0"

        'FROM
        registRw.FOLLOWDAYFROM = initStartDate.ToString("yyyy/MM/dd HH:mm")

        'TO
        registRw.FOLLOWDAYTO = initEndDate.ToString("HH:mm")

        'アラート
        registRw.FOLLOWALERT = "0"
        registRw.NEXTACTALERT = "0"

        '------------------------
        ' 他
        '------------------------

        '成約車種を選択(1)にする
        Dim sucessSeqno As Long = _
            SC3080216BusinessLogic.GetSuccessSeriesSeqno(inputRw.FLLWUPBOX_SEQNO)
        Dim selectSelSeries As String = Sc3080218Page.selectSelSeries

        Dim successcarary As String()
        successcarary = selectSelSeries.Split(";"c)
        Dim successcararywk As String()
        Dim cnt As Integer = 1

        '希望車種の種類分ループ
        Dim sucessSeries As String = String.Empty
        For i = 0 To successcarary.Length - 2
            successcararywk = successcarary(i).Split(","c)
            If String.Equals(successcararywk(0), CType(sucessSeqno, String)) Then
                successcararywk(1) = "1"
            End If
            sucessSeries = sucessSeries + successcararywk(0) + "," + successcararywk(1) + ";"
        Next

        registRw.SUCCESSSERIES = sucessSeries
        registRw.GIVEUPMAKER = ""
        registRw.GIVEUPMODEL = ""
        registRw.GIVEUPREASON = ""
        registRw.CSTKIND = SC308216cstkind.Value
        registRw.INSDID = SC308216insdid.Value
        registRw.VCLSEQ = SC308216Vclseq.Value
        registRw.FLLWSEQ = SC308216fllwSeq.Value
        registRw.FLLWSTRCD = SC308216fllwstrcd.Value
        If Not String.IsNullOrEmpty(SC308216walkinNum.Value) Then    '来店人数
            registRw.WALKINNUM = SC308216walkinNum.Value
        End If

        If ContainsKey(ScreenPos.Current, SESSION_KEY_ACT_ID) Then
            registRw.ACTID = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_ACT_ID, False), Decimal)
        Else
            registRw.ACTID = 0
        End If

        ' 用件ID
        If ContainsKey(ScreenPos.Current, SESSION_KEY_REQ_ID) Then
            registRw.REQID = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_REQ_ID, False), Decimal)
        Else
            registRw.REQID = 0
        End If

        ' 商談ID
        registRw.SALESID = SC308216fllwSeq.Value

        ' 誘致ID
        If ContainsKey(ScreenPos.Current, SESSION_KEY_ATT_ID) Then
            registRw.ATTID = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_ATT_ID, False), Decimal)
        Else
            registRw.ATTID = 0
        End If

        ' 活動回数
        If ContainsKey(ScreenPos.Current, SESSION_KEY_COUNT) Then
            registRw.ACTCOUNT = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_COUNT, False), Long)
        Else
            registRw.ACTCOUNT = 0
        End If

        ' 用件行ロックバージョン
        If ContainsKey(ScreenPos.Current, SESSION_KEY_REQUEST_LOCK_VERSION) Then
            registRw.REQUESTLOCKVERSION = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_REQUEST_LOCK_VERSION, False), Long)
        Else
            registRw.REQUESTLOCKVERSION = 0
        End If

        ' 誘致ロックバージョン
        If ContainsKey(ScreenPos.Current, SESSION_KEY_ATTRACT_LOCK_VERSION) Then
            registRw.ATTRACTLOCKVERSION = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_ATTRACT_LOCK_VERSION, False), Long)
        Else
            registRw.ATTRACTLOCKVERSION = 0
        End If

        ' 商談行ロックバージョン
        If ContainsKey(ScreenPos.Current, SESSION_KEY_SALES_LOCK_VERSION) Then
            registRw.SALESVERSION = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_SALES_LOCK_VERSION, False), Long)
        Else
            registRw.SALESVERSION = 0
        End If

        ' 顧客ID
        registRw.CSTID = SC308216insdid.Value

        ' 車両ID
        If SC308216Vclseq.Value = "" Then
            registRw.VCLID = 0
        Else
            registRw.VCLID = SC308216Vclseq.Value
        End If

        ' 断念競合車種連番
        registRw.GIVEUPVCLSEQ = 0

        '登録対象のレコードを追加
        registTbl.Rows.Add(registRw)

        Logger.Info("SetRegistData End")

    End Sub

    ''' <summary>
    ''' 前回までの活動終了時間で最新の時間を取得する
    ''' </summary>
    ''' <param name="actTimeEnd"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetLatestActTimeEnd(ByRef actTimeEnd As Date) As Boolean

        Logger.Info("GetLatestActTimeEnd Start")

        Dim staffInfo As StaffContext = StaffContext.Current
        Dim dlrCd As String = staffInfo.DlrCD
        Dim strCD As String = SC308216fllwstrcd.Value
        Dim fllwupboxSeqNo As String = 0
        Dim dtLatestActTime As ActivityInfoDataSet.ActivityInfoLatestActTimeDataTable = Nothing
        Dim ret As Boolean = Long.TryParse(SC308216fllwSeq.Value, fllwupboxSeqNo)
        If (Not ret) Then
            Return ret
        End If
        dtLatestActTime = ActivityInfoBusinessLogic.GetLatestActTimeEnd(dlrCd, strCD, fllwupboxSeqNo)

        If dtLatestActTime IsNot Nothing AndAlso dtLatestActTime.Rows.Count > 0 Then
            Dim drLatestActTime As ActivityInfoDataSet.ActivityInfoLatestActTimeRow = dtLatestActTime(0)
            If Not drLatestActTime.IsLATEST_TIME_ENDNull Then
                '前回までの活動終了時間で最新の時間が取得できた場合
                actTimeEnd = drLatestActTime.LATEST_TIME_END
                Return True
            End If
        End If

        Logger.Info("GetLatestActTimeEnd End")

        Return False

    End Function

    ''' <summary>
    ''' 受注後工程活動の工程名の表示・非表示を調整する
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub SC3080216_AfterActivityPrcsRepeater_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles SC3080216_AfterActivityPrcsRepeater.ItemDataBound
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_Start", GetCurrentMethod.Name), True)

        Dim row As SC3080216DataSet.SC3080216AfterOdracTRow = DirectCast(e.Item.DataItem.row, SC3080216DataSet.SC3080216AfterOdracTRow)
        Dim rightBoxTitle As HtmlGenericControl = DirectCast(e.Item.FindControl("rightBoxTitle"), HtmlGenericControl)
        Dim rightBoxRow As HtmlGenericControl = DirectCast(e.Item.FindControl("rightBoxRow"), HtmlGenericControl)
        Dim checkBorderAreaPrcs As HtmlGenericControl = DirectCast(e.Item.FindControl("CheckBorderAreaPrcs"), HtmlGenericControl)
        Dim checkBorderImageAreaPrcs As HtmlGenericControl = DirectCast(e.Item.FindControl("CheckBorderImageAreaPrcs"), HtmlGenericControl)
        Dim afterOdrActNamePrcs As CustomLabel = DirectCast(e.Item.FindControl("After_Odr_Act_Name_Prcs"), CustomLabel)
        Dim titleFlgPrcs As HiddenField = DirectCast(e.Item.FindControl("SC3080216_Title_Flg_Prcs"), HiddenField)
        Dim afterActNoCheckPrcs As HiddenField = DirectCast(e.Item.FindControl("SC3080216AfterActNoCheckPrcs"), HiddenField)
        Dim afterActCheckMarkFlgPrcs As HiddenField = DirectCast(e.Item.FindControl("SC3080216AfterActCheckMarkFlgPrcs"), HiddenField)
        Dim afterActContractFlg As HiddenField = DirectCast(e.Item.FindControl("SC3080216_save_flg_prcs"), HiddenField)

        '工程別の先頭行を出力するか設定する

        If HeaderTitlePrcs.Value.Equals(row.AFTER_ODR_PRCS_CD) Then

            '工程名を非表示にする
            AddCssClass(rightBoxTitle, "display:none")

        Else
            '工程名を表示する
            HeaderTitlePrcs.Value = row.AFTER_ODR_PRCS_CD

            '活動がすべて完了済みの場合、ヘッダー非表示フラグON
            If PrcsHeaderShowList.IndexOf(row.AFTER_ODR_PRCS_CD) < 0 Then
                titleFlgPrcs.Value = "1"
                AddCssClass(rightBoxTitle, "display:none")
            Else
                AddCssClass(rightBoxTitle, "display:block")
            End If
        End If

        ' 完了済み活動を非表示とする
        If (row.COMPLETION_FLG.Equals("1")) Then
            AddCssClass(rightBoxRow, "display:none")

        End If

        '背景色変更
        If row.COMPLETION_FLG.Equals("1") Then
            afterOdrActNamePrcs.Attributes("class") = "complete"
        ElseIf row.STD_VOLUNTARYINS_ACT_TYPE.Equals("1") Then
            afterOdrActNamePrcs.Attributes("class") = "on"
        End If

        'チェックボックス設定
        If row.CHECK_BOX_FLG.Equals("0") Then
            AddCssClass(checkBorderAreaPrcs, "border: none")
            AddCssClass(checkBorderImageAreaPrcs, "background-image: none")
            afterActNoCheckPrcs.Value = "1"
        End If

        '契約活動チェックボックス固定値設定
        If ActContractCode.Value.Equals(row.AFTER_ODR_ACT_CD) Then
            afterActNoCheckPrcs.Value = "1"
            If row.COMPLETION_FLG.Equals("0") Then
                afterActContractFlg.Value = "1"
            End If
        End If

        '担当外活動は触れられないようにする
        If Not row.AFTER_ODR_ACT_INPUT_TYPE.Equals(DATEORTIME_FLG_SET_TIME_ON) Then
            afterActCheckMarkFlgPrcs.Value = DATEORTIME_FLG_SET_TIME_ON
        End If

    End Sub


    ''' <summary>
    ''' 受注後工程活動の日付の表示・非表示を調整する
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub SC3080216_AfterActivityDaysRepeater_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles SC3080216_AfterActivityDaysRepeater.ItemDataBound
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_Start", GetCurrentMethod.Name), True)

        Dim row As SC3080216DataSet.SC3080216AfterOdracTRow = DirectCast(e.Item.DataItem.row, SC3080216DataSet.SC3080216AfterOdracTRow)
        Dim rightBoxTitle As HtmlGenericControl = DirectCast(e.Item.FindControl("rightBoxTitle"), HtmlGenericControl)
        Dim rightBoxRow As HtmlGenericControl = DirectCast(e.Item.FindControl("rightBoxRow"), HtmlGenericControl)
        Dim checkBorderAreaDays As HtmlGenericControl = DirectCast(e.Item.FindControl("CheckBorderAreaDays"), HtmlGenericControl)
        Dim checkBorderImageAreaDays As HtmlGenericControl = DirectCast(e.Item.FindControl("CheckBorderImageAreaDays"), HtmlGenericControl)
        Dim afterOdrActNameDays As CustomLabel = DirectCast(e.Item.FindControl("After_Odr_Act_Name_Days"), CustomLabel)
        Dim titleFlgDays As HiddenField = DirectCast(e.Item.FindControl("SC3080216_Title_Flg_Days"), HiddenField)
        Dim afterActNoCheckDays As HiddenField = DirectCast(e.Item.FindControl("SC3080216AfterActNoCheckDays"), HiddenField)
        Dim afterActCheckMarkFlgDays As HiddenField = DirectCast(e.Item.FindControl("SC3080216AfterActCheckMarkFlgDays"), HiddenField)
        Dim afterActContractFlg As HiddenField = DirectCast(e.Item.FindControl("SC3080216_save_flg_days"), HiddenField)

        '日付別の先頭行を出力するか設定する
        If HeaderTitleDays.Value.Equals(row.DATEORTIME) Then

            '日付を非表示にする
            AddCssClass(rightBoxTitle, "display:none")

        Else
            '日付を表示する
            HeaderTitleDays.Value = row.DATEORTIME

            '活動がすべて完了済みの場合、ヘッダー非表示フラグON
            If DaysHeaderShowList.IndexOf(row.DATEORTIME) < 0 Then
                titleFlgDays.Value = "1"
                AddCssClass(rightBoxTitle, "display:none")
            Else
                AddCssClass(rightBoxTitle, "display:block")
            End If
        End If


        ' 完了済み活動を非表示とする
        If (row.COMPLETION_FLG.Equals("1")) Then
            AddCssClass(rightBoxRow, "display:none")

        End If

        '背景色変更
        If row.COMPLETION_FLG.Equals("1") Then
            afterOdrActNameDays.Attributes("class") = "complete"
        ElseIf row.STD_VOLUNTARYINS_ACT_TYPE.Equals("1") Then
            afterOdrActNameDays.Attributes("class") = "on"
        End If

        'チェックボックス設定
        If row.CHECK_BOX_FLG.Equals("0") Then
            AddCssClass(checkBorderAreaDays, "border: none")
            AddCssClass(checkBorderImageAreaDays, "background-image: none")
            afterActNoCheckDays.Value = "1"
        End If

        '契約活動チェックボックス固定値設定
        If ActContractCode.Value.Equals(row.AFTER_ODR_ACT_CD) Then
            afterActNoCheckDays.Value = "1"
            If row.COMPLETION_FLG.Equals("0") Then
                afterActContractFlg.Value = "1"
            End If
        End If

        '担当外活動は触れられないようにする
        If Not row.AFTER_ODR_ACT_INPUT_TYPE.Equals(DATEORTIME_FLG_SET_TIME_ON) Then
            afterActCheckMarkFlgDays.Value = DATEORTIME_FLG_SET_TIME_ON
        End If

    End Sub

    ''' <summary>
    ''' スタイル設定
    ''' </summary>
    ''' <param name="element">設定コントロール</param>
    ''' <param name="cssClass">設定クラス名</param>
    ''' <remarks></remarks>
    Private Sub AddCssClass(ByVal element As HtmlGenericControl, ByVal cssClass As String)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_Start", GetCurrentMethod.Name), True)

        If String.IsNullOrEmpty(element.Attributes("style").Trim) Then
            element.Attributes("style") = cssClass
        Else
            element.Attributes("style") = element.Attributes("style") & " " & cssClass
        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name), True)
    End Sub

    ''' <summary>
    ''' CalDAV連携情報を設定する
    ''' </summary>
    ''' <param name="calDavData">CalDAV連携情報格納クラス</param>
    ''' <remarks></remarks>
    Private Function SetCalDavTag(ByVal calDavData As SC3080216BusinessLogic.CalDavDate) As SC3080216BusinessLogic.CalDavDate

        Logger.Info("SetCalDavTag Start")

        Logger.Info("SC3080216 SetCalDavTag CalDAV連携情報設定 Start")

        'ログインユーザ情報取得用
        Dim context As StaffContext = StaffContext.Current

        'CalDAV連携の情報をセットする
        '販売店コード
        calDavData.DealerCode = context.DlrCD
        Logger.Info("販売店コード" + calDavData.DealerCode)

        '店舗コード
        calDavData.BranchCode = context.BrnCD
        Logger.Info("店舗コード" + calDavData.BranchCode)

        'スタッフコード
        calDavData.StaffCode = context.Account
        Logger.Info("スタッフコード" + calDavData.StaffCode)

        '顧客区分
        calDavData.CustomerDiv = DirectCast(GetValue(ScreenPos.Current, CONST_CSTKIND, False), String)
        Logger.Info("顧客区分" + calDavData.CustomerDiv)

        '顧客コード
        calDavData.CustomerCode = DirectCast(GetValue(ScreenPos.Current, CONST_INSDID, False), String)
        Logger.Info("顧客コード" + calDavData.CustomerCode)


        Logger.Info("SC3080216 SetCalDavTag CalDAV連携情報設定 End")

        Return calDavData

        Logger.Info("SetCalDavTag End")

    End Function

#End Region

End Class
