'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3080203.aspx.vb
'─────────────────────────────────────
'機能： 顧客詳細(活動登録)
'補足： 
'作成：            TCS 河原 【SALES_1A】
'更新： 2012/08/09 TCS 河原 【A STEP2】次世代e-CRB セールスKPI出力開発
'更新： 2012/08/09 TCS 河原 Follow-upBox結果の次回活動日の修正
'更新： 2012/08/09 TCS 安田 【SALES_3】共通.来店実績更新パラメーター追加
'更新： 2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善
'更新： 2012/09/06 TCS 山口 【A STEP2】次世代e-CRB 新車受付機能改善
'更新： 2012/11/05 TCS 神本 【A.STEP2】次世代e-CRB  新車タブレット横展開に向けた機能開発
'更新： 2013/01/10 TCS 上田 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発
'更新： 2013/03/07 TCS 橋本 【A.STEP2】新車タブレット受付画面の管理指標変更対応
'更新： 2013/03/13 TCS 葛西 GL0875対応
'更新： 2013/06/30 TCS 庄   【A STEP2】i-CROP新DB適応に向けた機能開発（既存流用）
'更新： 2013/12/03 TCS 市川 Aカード情報相互連携開発
'更新： 2014/01/29 TCS 松月 【A STEP2】成約活動ID設定漏れ対応（号口切替BTS-15） 
'更新： 2014/04/01 TCS 松月 【A STEP2】TMT不具合対応
'更新： 2014/04/14 TCS 松月 【A STEP2】活動予定接触方法設定不正不具合対応（問連TR-V4-GTMC140211005）
'更新： 2014/05/15 TCS 武田 受注後フォロー機能開発
'更新： 2014/05/23 TCS 安田  TMT不具合対応
'更新： 2014/08/01 TCS 市川 受注後フォロー機能開発(UAT-BTS-212対応)
'更新： 2014/08/20 TCS 森   受注後活動A⇒H移行対応
'更新： 2014/09/01 TCS 松月 【A STEP2】ToDo連携店舗コード変更対応(初期活動店舗)（問連TR-V4-GTMC140807001）
'更新： 2014/10/08 TCS 河原  TMT BTS-193
'更新： 2015/07/06 TCS 藤井  TR-V4-FTMS141028001(FTMS→TMTマージ)
'更新： 2015/12/10 TCS 鈴木 受注後工程蓋閉め対応
'更新： 2015/12/21 TCS 中村 受注後工程蓋閉め対応 IT1対応
'更新： 2019/04/05 TS  河原 見積印刷実績がある状態で活動結果登録を行うとエラーになる件
'更新： 2020/01/28 TS  舩橋 TKM Change request development for Next Gen e-CRB (CR058,CR061) 
'─────────────────────────────────────

Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.CustomerInfo.Details.DataAccess.SC3080203DataSet
Imports Toyota.eCRB.CustomerInfo.Details.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.CustomerInfo.Details.DataAccess.SC3080203DataSetTableAdapters
Imports Toyota.eCRB.iCROP.BizLogic
Imports System.Globalization
Imports Toyota.eCRB.Common.VisitResult.BizLogic
'2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 START
Imports Toyota.eCRB.CommonUtility.DataAccess
'2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 END
'2012/11/05 TCS 神本 【A.STEP2】次世代e-CRB  新車タブレット横展開に向けた機能開発 START
Imports Toyota.eCRB.Estimate.Quotation.BizLogic
'2012/11/05 TCS 神本 【A.STEP2】次世代e-CRB  新車タブレット横展開に向けた機能開発 END
'2013/01/10 TCS 上田 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START
Imports Toyota.eCRB.CommonUtility.BizLogic
'2013/01/10 TCS 上田 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END

Public Class SC3080203BusinessLogic
    Inherits BaseBusinessComponent

#Region "定数"

    ''' <summary>
    ''' 画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONTENT_MODULEID As String = "SC3080203"

    ''' <summary>
    ''' カタログ用SEQ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONTENT_SEQ_CATALOG As Integer = 9

    ''' <summary>
    ''' 試乗用SEQ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONTENT_SEQ_TESTDRIVE As Integer = 16

    ''' <summary>
    ''' 査定用SEQ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONTENT_SEQ_ASSESSMENT As Integer = 18

    ''' <summary>
    ''' 見積り用SEQ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONTENT_SEQ_VALUATION As Integer = 10

    ''' <summary>
    ''' HotのときのActionCD
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONTENT_HOT_ACTIONCD As String = "D06"

    ''' <summary>
    ''' ProspectのときのActionCD
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONTENT_PROSPECT_ACTIONCD As String = "D05"

    ''' <summary>
    ''' SuccessのときのActionCD
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONTENT_SUCCESS_ACTIONCD As String = "D01"

    ''' <summary>
    ''' Give-upのときのActionCD
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONTENT_GIVEUP_ACTIONCD As String = "D02"

    ''' <summary>
    ''' 成約時のCR活動結果ID取得用
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONTENT_SUCCESS_CRRSLTID As String = "SUCCESS_CRRSLTID"

    ''' <summary>
    ''' 継続時のCR活動結果ID取得用
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONTENT_CONTINUE_CRRSLTID As String = "CONTINUE_CRRSLTID"

    ''' <summary>
    ''' 断念時のCR活動結果ID取得用
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONTENT_GIVEUP_CRRSLTID As String = "GIVEUP_CRRSLTID"

    ''' <summary>
    ''' Hot・Procpect時のCR活動結果ID取得用
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONTENT_HOTPROSPECT_CRRSLTID As String = "HOTPROSPECT_CRRSLTID"

    ''' <summary>
    ''' Walk-in時のCR活動結果ID取得用
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONTENT_WALKINREQUEST_CRRSLTID As String = "WALKINREQUEST_CRRSLTID"

    '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 用件ソース(1st）コード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const USED_FLG_SOURCE1EDIT As String = "USED_FLG_SOURCE1EDIT"
    '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

    ''' <summary>
    ''' 来店区分取得用
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONTENT_WALKIN_WICID As String = "WALKIN_WICID"

    ''' <summary>
    ''' 敬称前後取得用
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONTENT_KEISYO_ZENGO As String = "KEISYO_ZENGO"

    '2012/03/02 Version1.01 Yasuda 【A.STEP2】代理商談入力機能開発 Start
    ''' <summary>
    ''' 受注後フラグ（0:受注時）
    ''' </summary>
    ''' <remarks></remarks>
    Public Const SalesFlg As String = "0"
    '2012/03/02 Version1.01 Yasuda 【A.STEP2】代理商談入力機能開発 End

    ''' <summary>
    ''' Follow-upBoxのCR活動スタータス
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_FLLWUP_HOT = "1"
    Private Const C_FLLWUP_PROSPECT = "2"
    Private Const C_FLLWUP_REPUCHASE = "3"
    Private Const C_FLLWUP_PERIODICAL = "4"
    Private Const C_FLLWUP_PROMOTION = "5"
    Private Const C_FLLWUP_REQUEST = "6"
    Private Const C_FLLWUP_WALKIN = "7"


    ''' <summary>
    ''' Follow-upBoxの活動結果
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_CRACTRSLT_HOT As String = "1"
    Private Const C_CRACTRSLT_PROSPECT As String = "2"
    Private Const C_CRACTRSLT_SUCCESS As String = "3"
    Private Const C_CRACTRSLT_CONTINUE As String = "4"
    Private Const C_CRACTRSLT_GIVEUP As String = "5"

    ''' <summary>
    ''' 画面で選択する活動結果
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_RSLT_WALKIN As String = "1"
    Private Const C_RSLT_PROSPECT As String = "2"
    Private Const C_RSLT_HOT As String = "3"
    Private Const C_RSLT_SUCCESS As String = "4"
    Private Const C_RSLT_GIVEUP As String = "5"

    '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
    Public Const CRACTRESULT_SUCCESS As String = "31"
    Private Const CRACTRESULT_HOT As String = "21"
    Private Const CRACTRESULT_GIVEUP As String = "32"
    Private Const CRACTRESULT_PROSPECT As String = "2"
    Private Const CRACTRESULT_WALKIN As String = "0"

    Private Const CRACTSTATUS_HOT As String = "30"
    Private Const CRACTSTATUS_PROSPECT As String = "20"
    Private Const CRACTSTATUS_WALKIN As String = "10"
    '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

    Private Const C_CRACTRESULT_HOT As String = "1"
    Private Const C_CRACTRESULT_PROSPECT As String = "2"
    Private Const C_CRACTRESULT_NOTACT As String = "0"
    Private Const C_CRACTRESULT_CONTINUE As String = "4"


    '1：Periodical Inspection  2：Repurchase Follow-up  3：Others  4:Birthday

    Private Const C_CRACTCATEGORY_DEFFULT As String = "0"
    Private Const C_CRACTCATEGORY_PERIODICAL As String = "1"
    Private Const C_CRACTCATEGORY_REPURCHASE As String = "2"
    Private Const C_CRACTCATEGORY_OTHERS As String = "3"
    Private Const C_CRACTCATEGORY_BIRTHDAY As String = "4"

    '1：Walk-in  2：Call-in  3：RMM  4：Request
    Private Const C_REQCATEGORY_WALKIN As String = "1"
    Private Const C_REQCATEGORY_CALLIN As String = "2"
    Private Const C_REQCATEGORY_RMM As String = "3"
    Private Const C_REQCATEGORY_REQUEST As String = "4"


    Private Const C_DONECAT_HOT = "6"
    Private Const C_DONECAT_PROSPECT = "7"
    Private Const C_DONECAT_REPURCHASE = "2"
    Private Const C_DONECAT_PERIODICAL = "1"
    Private Const C_DONECAT_PROMOTION = "3"
    Private Const C_DONECAT_REQUEST = "4"
    Private Const C_DONECAT_WALKIN = "5"

    'Sales Staff権限の権限コード
    Private Const C_SALESSTAFFOPECD As String = "8"

    'CalDAV連携用URL
    Private Const C_CALDAV_WEBSERVICE_URL As String = "CALDAV_WEBSERVICE_URL"

    ''' <summary>
    ''' 在席状態：商談中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STAFF_STATUS_NEGOTIATION As String = "20"

    '2015/12/21 TCS 中村 受注後工程蓋閉め対応 IT1対応 ADD START
    ''' <summary>
    ''' 在席状態：納車作業中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STAFF_STATUS_DELIVERY As String = "22"
    '2015/12/21 TCS 中村 受注後工程蓋閉め対応 IT1対応 ADD END

    ''' <summary>
    ''' メッセージＩＤ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MSG_30932 As Integer = 30932

    '2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 START
    ''' <summary>
    ''' 顧客区分　自社客：1　未取引客：2
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CSTKIND_ORG As String = "1"
    Private Const CSTKIND_NEW As String = "2"

    ''' <summary>
    ''' ダミー名称フラグ  正式名称:0　ダミー名称:1
    ''' </summary>
    Public Const DummyNameFlgOfficial As String = "0"
    Public Const DummyNameFlgDummy As String = "1"
    '2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 END

    '2013/03/07 TCS 橋本 【A.STEP2】新車タブレット受付画面の管理指標変更対応 ADD START
    ''' <summary>
    ''' 査定実績判定(査定実績なし)
    ''' </summary>
    Private Const C_ASMACTSTATUS_NASI As String = "0"

    ''' <summary>
    ''' 査定実績判定(査定実績あり＆査定回答済)
    ''' </summary>
    Private Const C_ASMACTSTATUS_ARI As String = "1"

    ''' <summary>
    ''' 査定実績判定(査定実績あり＆査定未回答)
    ''' </summary>
    Private Const C_ASMACTSTATUS_MIKAITOU As String = "2"

    ''' <summary>
    ''' 通知依頼情報．最終ステータス(キャンセル)
    ''' </summary>
    Private Const C_NOTICEREQSTATUS_CANCEL As String = "2"
    '2013/03/07 TCS 橋本 【A.STEP2】新車タブレット受付画面の管理指標変更対応 ADD END


    '2013/12/03 TCS 市川 Aカード情報相互連携開発 START
    'システム環境設定・販売店環境設置のKEY
    Private Const ENVSETTINGKEY_GIVEUP_REASON_OTHER As String = "OTHER_GIVEUP_REASON_ACT_RSLT_ID" '断念理由手入力時に選択する活動結果ID
    Private Const ENVSETTINGKEY_MANUAL_SUCCESSABLE As String = "TABLET_SUCCESS_FLG"                '注文承認を経ない手動Successの許可

    Private Const ENVSETTINGVALUE_MANUAL_SUCCESSABLE As String = "1" '手動Success可
    '2013/12/03 TCS 市川 Aカード情報相互連携開発 END

    ''' <summary>
    ''' ブランク/V4画面へ遷移後セッションの店舗コードがNullとなるため、セッションから取得できない場合はブランクを設定するように修正
    ''' </summary>
    ''' <remarks></remarks>
    Private Const BLANK = " "

#End Region

#Region "処理"

    ''' <summary>
    ''' デフォルトコンストラクタ
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        '処理なし
    End Sub

    ''' <summary>
    ''' 担当SC一覧取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetUsers() As SC3080203DataSet.SC3080203UsersDataTable
        Dim context As StaffContext = StaffContext.Current
        Return SC3080203TableAdapter.GetUsers(context.DlrCD, context.BrnCD)
    End Function


    '2013/06/30 TCS 松月 2013/10対応版　既存流用 START DEL
    '2013/06/30 TCS 松月 2013/10対応版　既存流用 END


    '2013/06/30 TCS 松月 2013/10対応版　既存流用 START DEL
    '2013/06/30 TCS 松月 2013/10対応版　既存流用 END

    '2013/06/30 TCS 松月 2013/10対応版　既存流用 START DEL
    '2013/06/30 TCS 松月 2013/10対応版　既存流用 END

    ''' <summary>
    ''' 活動方法取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetActContact() As SC3080203DataSet.SC3080203ActContactDataTable
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
        Return SC3080203TableAdapter.GetActContact()
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 END
    End Function


    ''' <summary>
    ''' 次回活動方法取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetNextActContact() As SC3080203DataSet.SC3080203NextActContactDataTable
        Return SC3080203TableAdapter.GetNextActContact()
    End Function


    ''' <summary>
    ''' フォロー方法取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetFollowContact() As SC3080203DataSet.SC3080203FollowContactDataTable
        Return SC3080203TableAdapter.GetFollowContact()
    End Function


    ''' <summary>
    ''' Follow-upBoxのスタータス取得
    ''' </summary>
    ''' <param name="serchdt"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetFollowCractstatus(ByVal serchdt As SC3080203DlrStrFollwDataTable) As SC3080203DataSet.SC3080201FollowStatusDataTable
        Dim Serchrw As SC3080203DlrStrFollwRow
        Serchrw = CType(serchdt.Rows(0), SC3080203DlrStrFollwRow)
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
        Return SC3080203TableAdapter.GetFollowCractstatus(Serchrw.FLLWUPBOX_SEQNO)
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 END
    End Function


    ''' <summary>
    ''' 文言取得
    ''' </summary>
    ''' <param name="serchdt"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetContentWord(ByVal serchdt As SC3080203SeqDataTable) As SC3080203DataSet.SC3080203ContentWordDataTable
        Dim Serchrw As SC3080203SeqRow
        Serchrw = CType(serchdt.Rows(0), SC3080203SeqRow)
        Return SC3080203TableAdapter.GetContentWord(Serchrw.SEQNO)
    End Function


    ''' <summary>
    ''' 競合メーカー取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetCompetitionMakermaster() As SC3080203DataSet.SC3080203CompetitionMakermasterDataTable
        Return SC3080203TableAdapter.GetCompetitionMakermaster()
    End Function


    ''' <summary>
    ''' 競合車種取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetCompetitorMaster() As SC3080203DataSet.SC3080203CompetitorMasterDataTable
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
        Dim dlrcd As String = StaffContext.Current.DlrCD
        Return SC3080203TableAdapter.GetCompetitorMaster(dlrcd)
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 END
    End Function


    ''' <summary>
    ''' 時分選択有のアラートマスタ取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetAlertSel() As SC3080203DataSet.SC3080203AlarmMasterDataTable
        Dim dt As SC3080203DataSet.SC3080203AlarmMasterDataTable = SC3080203TableAdapter.GetAlarmMaster("1")
        SetAlertTitle(dt)
        AddNoneAlertItem(dt)
        Return dt
    End Function


    ''' <summary>
    ''' 時分選択無のアラートマスタ取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetAlertNonSel() As SC3080203DataSet.SC3080203AlarmMasterDataTable
        '--デバッグログ---------------------------------------------------
        Logger.Info("GetAlertNonSel Start")
        '-----------------------------------------------------------------
        Dim dt As SC3080203DataSet.SC3080203AlarmMasterDataTable = SC3080203TableAdapter.GetAlarmMaster("2")
        SetAlertTitle(dt)
        AddNoneAlertItem(dt)
        '--デバッグログ---------------------------------------------------
        Logger.Info("GetAlertNonSel End")
        '-----------------------------------------------------------------
        Return dt
    End Function

    Private Shared Sub AddNoneAlertItem(ByVal dt As SC3080203DataSet.SC3080203AlarmMasterDataTable)
        '--デバッグログ---------------------------------------------------
        Logger.Info("AddNoneAlertItem Start")
        '-----------------------------------------------------------------

        Dim dr As SC3080203DataSet.SC3080203AlarmMasterRow = dt.NewSC3080203AlarmMasterRow()
        With dr
            .TITLE = WebWordUtility.GetWord(30333)
            .ALARMNO = 0L
            .TIME = 0S
            .UNIT = String.Empty
        End With
        dt.Rows.InsertAt(dr, 0)

        '--デバッグログ---------------------------------------------------
        Logger.Info("AddNoneAlertItem End")
        '-----------------------------------------------------------------
    End Sub

    ''' <summary>
    ''' アラートの
    ''' </summary>
    ''' <param name="dt"></param>
    ''' <remarks></remarks>
    Private Shared Sub SetAlertTitle(ByVal dt As SC3080203DataSet.SC3080203AlarmMasterDataTable)
        '--デバッグログ---------------------------------------------------
        Logger.Info("SetAlertTitle Start")
        '-----------------------------------------------------------------

        For Each dr As SC3080203DataSet.SC3080203AlarmMasterRow In dt.Rows
            Dim title As String = dr.TIME.ToString(CultureInfo.CurrentCulture())
            If dr.UNIT.Equals("0") Then
                title = WebWordUtility.GetWord(30334)
            End If
            If dr.UNIT.Equals("1") Then
                title = title & WebWordUtility.GetWord(30335)
            End If
            If dr.UNIT.Equals("2") Then
                title = title & WebWordUtility.GetWord(30336)
            End If
            If dr.UNIT.Equals("3") Then
                title = title & WebWordUtility.GetWord(30337)
            End If
            If dr.UNIT.Equals("4") Then
                title = title & WebWordUtility.GetWord(30338)
            End If
            dr.TITLE = title
        Next
        '--デバッグログ---------------------------------------------------
        Logger.Info("SetAlertTitle End")
        '-----------------------------------------------------------------
    End Sub


    ''' <summary>
    ''' 日付フォーマット取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetDateFormat() As SC3080203DataSet.SC3080203DateFormatDataTable
        Return SC3080203TableAdapter.GetDateFormat()
    End Function


    ''' <summary>
    ''' アイコンのパス取得
    ''' </summary>
    ''' <param name="seqno"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetContentIconPath(ByVal seqno As Integer) As SC3080203DataSet.SC3080203ContentIconPathDataTable
        Return SC3080203TableAdapter.GetContentIconPath(seqno)
    End Function

    '2013/06/30 TCS 庄 2013/10対応版　既存流用 START DEL
    '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

    '2013/06/30 TCS 庄 2013/10対応版　既存流用 START DEL
    '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

    '2013/03/07 TCS 橋本 【A.STEP2】新車タブレット受付画面の管理指標変更対応 MOD START
    'Shared Function InsertHistory(ByVal dlrcd As String, ByVal fllwstrcd As String, ByVal fllwupboxseqno As Long,
    '                                 ByVal selcar As String, ByVal catalog As String, ByVal testdrive As String,
    '                                 ByVal assessment As String, ByVal valuation As String, ByVal account As String, ByVal actdate As String) As Boolean

    '2013/06/30 TCS 庄 2013/10対応版　既存流用 START DEL
    '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

    '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 活動結果を登録する
    ''' </summary>
    ''' <param name="registdt"></param>
    ''' <param name="resistFlg">1:新規活動登録　2:新規活動登録からの即Success、Give-up　3:既存の活動に対する活動結果</param>
    ''' <param name="fllwStatus"></param>
    ''' <param name="contractStatusFlg" >契約状況フラグ（0:未契約 1:契約済 2:キャンセル）</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Function InsertActivityResult(ByVal registdt As SC3080203RegistDataDataTable, ByVal resistFlg As String, ByVal fllwStatus As String, ByVal contractStatusFlg As String) As Boolean
        '--デバッグログ---------------------------------------------------
        Logger.Info("InsertActivityResult Start")
        '-----------------------------------------------------------------

        '既存活動の場合
        '要件の場合

        Dim RegistRw As SC3080203DataSet.SC3080203RegistDataRow
        RegistRw = CType(registdt.Rows(0), SC3080203RegistDataRow)
        Dim fllwstrcd As String = RegistRw.FLLWSTRCD
        Dim fllwupbox_seqno As Decimal = RegistRw.FLLWSEQ
        '用件ID
        Dim reqid As Decimal = RegistRw.REQID
        '用件行ロックバージョン
        Dim requestLockversion As Long = RegistRw.REQUESTLOCKVERSION
        '誘致行ロックバージョン
        Dim attractLockversion As Long = RegistRw.ATTRACTLOCKVERSION
        '商談行ロックバージョン
        Dim salesLockversion As Long = RegistRw.SALESLOCKVERSION
        '誘致ID
        Dim attid As Decimal = RegistRw.ATTID
        '活動ID
        Dim actid As Decimal = RegistRw.ACTID
        '商談ID
        Dim salesid As Decimal = fllwupbox_seqno
        'ログインユーザー情報取得用
        Dim context As StaffContext = StaffContext.Current
        '店舗コード
        Dim dlrcd As String = context.DlrCD
        '販売店コード
        Dim brncd As String = context.BrnCD
        '活動結果
        Dim actresult As String = RegistRw.ACTRESULT
        'モデルコード
        Dim modelcode As String = RegistRw.GIVEUPMODEL

        '既存活動の場合
        If actid <> 0 Then

            '用件の場合
            If attid = 0 Then
                '用件データロック
                SC3080203TableAdapter.GetRequestLock(reqid, requestLockversion)
            Else
                '誘致の場合
                SC3080203TableAdapter.GetAttractLock(attid, attractLockversion)
            End If

            '商談データロック
            SC3080203TableAdapter.GetSalesLock(salesid, salesLockversion)

            'follow-up Box商談データロック
            SC3080203TableAdapter.GetFollowupSalesLock(dlrcd, brncd, fllwupbox_seqno)

        Else
            '2013/12/03 TCS 市川 Aカード情報相互連携開発 START
            '新規活動の場合

            '商談一時情報ロック
            If SC3080203TableAdapter.LockSalesTemp(fllwupbox_seqno) <> 1 Then
                Logger.Error("SC3080203BusinessLogic.InsertActivityResult - Internal Error: SC3080203TableAdapter.LockSalesTemp (SALES_ID:" & fllwupbox_seqno & ")")
                Me.Rollback = True
                Return False
            End If

            '2013/12/03 TCS 市川 Aカード情報相互連携開発 END
        End If

        '未存在希望車種登録
        InsertNotRegSelectedSeries(registdt)

        'GIVEUPの場合
        If (String.Equals(actresult, C_RSLT_GIVEUP)) And (Not String.IsNullOrEmpty(modelcode)) Then
            Dim compnum As Integer = SC3080203TableAdapter.GetCompvalseq(CType(salesid, String), modelcode)
            If (compnum.Equals(0)) Then
                '2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
                '競合車種連番を取得
                compnum = SC3080203TableAdapter.GetMaxcompvalseq(CType(salesid, String))
                '2013/06/30 TCS 小幡 2013/10対応版　既存流用 END
                '競合車種登録
                SC3080203TableAdapter.SetCompetitorvcl(CType(salesid, String), CType(compnum + 1, String), modelcode, StaffContext.Current.Account)

                ' 2014/05/23 TCS 安田 TMT不具合対応 START
                compnum = compnum + 1
                ' 2014/05/23 TCS 安田 TMT不具合対応 END

            Else
                ' 2014/05/23 TCS 安田 TMT不具合対応 START
                '登録されている、断念競合車種連番を取得する
                compnum = SC3080203TableAdapter.GetModelCompvalseq(salesid, modelcode)
                ' 2014/05/23 TCS 安田 TMT不具合対応 END

            End If

            ' 2014/05/23 TCS 安田 TMT不具合対応 START
            '断念競合車種連番をセットする
            RegistRw.GIVEUPVCLSEQ = compnum
            ' 2014/05/23 TCS 安田 TMT不具合対応 END
        End If

        Dim staffcdplan As String = context.Account

        If actid <> 0 Then
            '既存活動の場合

            Dim retCnt As Integer
            retCnt = SC3080203TableAdapter.GetCountStaffPlan(reqid, attid)
            If retCnt > 0 Then
                staffcdplan = SC3080203TableAdapter.GetStaffPlan(reqid, attid)
            End If

            '用件ID
            reqid = RegistRw.REQID
            '誘致ID
            attid = RegistRw.ATTID

            '用件の場合
            If attid = 0 Then
                '用件の場合
                If Not UpdateRequest(registdt, resistFlg, fllwStatus) Then
                    Logger.Error("SC3080203BusinessLogic.InsertActivityResult - Internal Error: UpdateRequest (REQ_ID:" & reqid & ")")
                    Me.Rollback = True
                    Return False
                End If
            Else
                '誘致の場合
                If Not UpdateAttract(registdt, resistFlg, fllwStatus) Then
                    Logger.Error("SC3080203BusinessLogic.InsertActivityResult - Internal Error: UpdateAttract (ATT_ID:" & attid & ")")
                    Me.Rollback = True
                    Return False
                End If
            End If

            '商談活動追加
            '2015/12/11 TCS 鈴木 受注後工程蓋閉め対応 START
            If (Not InsertSalesAct(registdt, resistFlg, fllwStatus, contractStatusFlg)) Then
                '2015/12/11 TCS 鈴木 受注後工程蓋閉め対応 END
                Logger.Error("SC3080203BusinessLogic.InsertActivityResult - Internal Error: InsertSalesAct")
                Me.Rollback = True
                Return False
            End If
        Else
            '新規活動の場合
            '2015/12/11 TCS 鈴木 受注後工程蓋閉め対応 MOD START
            InsertNewRequest(registdt, resistFlg, fllwStatus, contractStatusFlg)
            '2015/12/11 TCS 鈴木 受注後工程蓋閉め対応 MOD END


        End If

        '2015/07/06 TCS 藤井  TR-V4-FTMS141028001(FTMS→TMTマージ) START

        '断念の場合、断念理由を顧客メモに登録
        If String.Equals(actresult, C_RSLT_GIVEUP) Then
            InsertCustomerMemo(RegistRw.CRCUSTID, RegistRw.GIVEUPREASON)
        End If

        '2015/07/06 TCS 藤井  TR-V4-FTMS141028001(FTMS→TMTマージ) END

        '活動結果がcold,worm,hotの場合
        '活動回数
        Dim count As Long = RegistRw.COUNT + 1
        RegistRw.COUNT = count
        Dim schedatetime As Date = Date.ParseExact(RegistRw.NEXTACTDAYFROM, "yyyy/MM/dd HH:mm", Nothing)
        Dim walkinschestart As Date = Date.ParseExact("1900/01/01 00:00", "yyyy/MM/dd HH:mm", Nothing)
        Dim walkinscheend As Date = Date.ParseExact("1900/01/01 00:00", "yyyy/MM/dd HH:mm", Nothing)
        Dim dlrcdplan As String = context.DlrCD
        Dim brncdplan As String = context.BrnCD

        Dim schecontactmtd As String = RegistRw.NEXTACTCONTACT
        Dim rsltflg As String = RegistRw.FOLLOWFLG
        Dim rsltdate As Date = Date.ParseExact("1900/01/01 00:00", "yyyy/MM/dd HH:mm", Nothing)
        Dim rsltstaffcd As String = context.Account
        Dim cractstatus As String = Nothing

        Dim sysEnv As New SystemEnvSetting

        Dim rsltid As String
        'CR活動結果IDを取得
        If String.Equals(actresult, C_RSLT_SUCCESS) Then
            rsltid = sysEnv.GetSystemEnvSetting(CONTENT_SUCCESS_CRRSLTID).PARAMVALUE
            cractstatus = CRACTRESULT_SUCCESS
        ElseIf String.Equals(actresult, C_RSLT_GIVEUP) Then
            '2013/12/03 TCS 市川 Aカード情報相互連携開発 START
            '断念時は断念理由ID(画面入力)を登録する。
            rsltid = RegistRw.GIVEUP_REASON_ID.ToString()
            '2013/12/03 TCS 市川 Aカード情報相互連携開発 END
            cractstatus = CRACTRESULT_GIVEUP
        ElseIf String.Equals(actresult, C_RSLT_WALKIN) Then
            rsltid = sysEnv.GetSystemEnvSetting(CONTENT_WALKINREQUEST_CRRSLTID).PARAMVALUE
            cractstatus = CRACTRESULT_HOT
        ElseIf String.Equals(actresult, C_RSLT_PROSPECT) Then
            If String.Equals(actresult, C_RSLT_WALKIN) Then
                rsltid = sysEnv.GetSystemEnvSetting(CONTENT_WALKINREQUEST_CRRSLTID).PARAMVALUE
            Else
                rsltid = sysEnv.GetSystemEnvSetting(CONTENT_HOTPROSPECT_CRRSLTID).PARAMVALUE
            End If
            cractstatus = CRACTRESULT_HOT
        Else
            If String.Equals(actresult, C_RSLT_WALKIN) Then
                rsltid = sysEnv.GetSystemEnvSetting(CONTENT_WALKINREQUEST_CRRSLTID).PARAMVALUE
            Else
                rsltid = sysEnv.GetSystemEnvSetting(CONTENT_HOTPROSPECT_CRRSLTID).PARAMVALUE
            End If
            cractstatus = CRACTRESULT_HOT
        End If
        Dim acount As String = context.Account
        Dim rowfunction As String = CONTENT_MODULEID
        reqid = RegistRw.REQID
        attid = RegistRw.ATTID

        '2013/12/03 TCS 市川 Aカード情報相互連携開発 START
        '活動結果がgive-up,success以外の場合
        If Not String.Equals(RegistRw.ACTRESULT, C_RSLT_GIVEUP) _
            AndAlso Not String.Equals(RegistRw.ACTRESULT, C_RSLT_SUCCESS) Then

            Dim WI_DateTime_Flg As Integer
            WI_DateTime_Flg = 0
            If RegistRw.NEXTACTCONTACT = "11" Then
                ' 2014/04/14 TCS 松月 【A STEP2】活動予定接触方法設定不正不具合対応（問連TR-V4-GTMC140211005）START
                'schedatetime = Date.ParseExact(RegistRw.FOLLOWDAYFROM, "yyyy/MM/dd HH:mm", Nothing)
                ' 2014/04/14 TCS 松月 【A STEP2】活動予定接触方法設定不正不具合対応（問連TR-V4-GTMC140211005）END
                walkinschestart = Date.ParseExact(RegistRw.NEXTACTDAYFROM, "yyyy/MM/dd HH:mm", Nothing)
                walkinscheend = Date.ParseExact(RegistRw.NEXTACTDAYFROM.Substring(0, 10) & " " & RegistRw.NEXTACTDAYTO, "yyyy/MM/dd HH:mm", Nothing)
                WI_DateTime_Flg = 1
            Else
                walkinschestart = Date.ParseExact("1900/01/01 00:00", "yyyy/MM/dd HH:mm", Nothing)
                walkinscheend = Date.ParseExact("1900/01/01 00:00", "yyyy/MM/dd HH:mm", Nothing)
            End If

            ' 2014/04/14 TCS 松月 【A STEP2】活動予定接触方法設定不正不具合対応（問連TR-V4-GTMC140211005）START
            Dim DateTime_Flg As Integer
            DateTime_Flg = 0
            If (RegistRw.NEXTACTCONTACT = "11" Or RegistRw.NEXTACTCONTACT = "21") And (RegistRw.FOLLOWFLG <> "1") Then
                DateTime_Flg = 1
            End If
            If RegistRw.FOLLOWFLG = "1" Then
                schedatetime = Date.ParseExact(RegistRw.FOLLOWDAYFROM, "yyyy/MM/dd HH:mm", Nothing)
                schecontactmtd = RegistRw.FOLLOWCONTACT
            End If
            ' 2014/04/14 TCS 松月 【A STEP2】活動予定接触方法設定不正不具合対応（問連TR-V4-GTMC140211005）END

            '活動Id
            actid = SC3080203TableAdapter.GetSqActId()
            '2013/06/30 TCS 小幡 2013/10対応版　既存流用 START

            Dim orgnzid As Decimal
            orgnzid = 0
            ' 2014/04/01 TCS 松月 TMT不具合対応 Modify Start
            Dim orgnzidplan As Decimal
            orgnzidplan = SC3080203TableAdapter.GetorgnzId(staffcdplan)
            ' 2014/04/01 TCS 松月 TMT不具合対応 Modify End
            '活動(予定)追加
            ' 2014/04/14 TCS 松月 【A STEP2】活動予定接触方法設定不正不具合対応（問連TR-V4-GTMC140211005）START
            SC3080203TableAdapter.InsertActivity(actid, reqid, attid,
                                                       count, schedatetime, walkinschestart,
                                                       walkinscheend, dlrcdplan, brncdplan,
                                                       staffcdplan, schecontactmtd, "0",
                                                       rsltdate, " ", " ",
                                                       " ", " ", " ",
                                                       rsltid,
                                                       acount, rowfunction, " ", orgnzid, orgnzidplan, DateTime_Flg, WI_DateTime_Flg)
            ' 2014/04/14 TCS 松月 【A STEP2】活動予定接触方法設定不正不具合対応（問連TR-V4-GTMC140211005）END
            '2013/06/30 TCS 小幡 2013/10対応版　既存流用 END
        End If

        Dim staffInfo As StaffContext = StaffContext.Current
        Dim actFromDate As Date = Date.ParseExact(registdt(0).ACTDAYFROM, "yyyy/MM/dd HH:mm", Nothing)
        Dim actdayto As String = registdt(0).ACTDAYFROM.Substring(0, 10) & " " & registdt(0).ACTDAYTO
        Dim actToDate As Date = Date.ParseExact(actdayto, "yyyy/MM/dd HH:mm", Nothing)
        Dim actaccount As String            '活動実施者(画面で入力した値)
        actaccount = RegistRw.ACTACCOUNT & "@" & context.DlrCD       '活動実施者(画面で入力した値)
        'FLLWUPBOX 商談を更新
        SC3080203DataSetTableAdapters.SC3080203TableAdapter.UpdateFllwupboxSales(registdt(0).FLLWSEQ, _
                                                                                    actaccount, _
                                                                                    actFromDate, _
                                                                                    actToDate, _
                                                                                    staffInfo.Account, _
                                                                                    rowfunction)

        'SA01連携(TMT向け機能)（※次回活動予定登録後、History移動前、FllwupboxSales更新後に呼び出す）
        If Not ActivityInfoBusinessLogic.SyncDmsSA01(salesid, StaffContext.Current.Account) Then
            Logger.Error("SC3080203BusinessLogic.InsertActivityResult - Internal Error: ActivityInfoBusinessLogic.SyncDmsSA01")
            Me.Rollback = True
            Return False
        End If

        '活動結果がgive-up,successの場合
        If String.Equals(RegistRw.ACTRESULT, C_RSLT_GIVEUP) _
            Or String.Equals(RegistRw.ACTRESULT, C_RSLT_SUCCESS) Then
            '商談の場合
            If reqid <> 0 Then
                If Not MoveHistory(True, reqid, salesid) Then
                    Logger.Error("SC3080203BusinessLogic.InsertActivityResult - Internal Error: MoveHistory (REQ_ID:" & reqid & ")")
                    Me.Rollback = True
                    Return False
                End If
            Else
                If SC3080203TableAdapter.AttractStatusCheck(attid) = 0 Then
                    If Not MoveHistory(False, attid, salesid) Then
                        Logger.Error("SC3080203BusinessLogic.InsertActivityResult - Internal Error: MoveHistory (ATT_ID:" & attid & ")")
                        Me.Rollback = True
                        Return False
                    End If
                End If
            End If

            '2014/08/20 TCS 森 受注後活動A⇒H移行対応 START
            '受注後活動の計画を削除する
            ActivityInfoBusinessLogic.MoveAfterOrderProcInfo(salesid, acount, CONTENT_MODULEID)
            '2014/08/20 TCS 森 受注後活動A⇒H移行対応 END

        End If
        '2013/12/03 TCS 市川 Aカード情報相互連携開発 END

        Dim account As String = context.Account     '自身のアカウント

        '査定実績フラグのクリア更新
        ActivityInfoBusinessLogic.UpdateActAsmFlg(dlrcd, _
                                                  fllwstrcd, _
                                                  fllwupbox_seqno, _
                                                  context.Account, _
                                                  rowfunction)

        '見積実績フラグのクリア更新
        ActivityInfoBusinessLogic.UpdateActEstFlg(dlrcd, _
                                                  fllwstrcd, _
                                                  fllwupbox_seqno, _
                                                  context.Account, _
                                                  rowfunction)

        '2013/12/03 TCS 市川 Aカード情報相互連携開発 DELETE

        '来店実績更新
        Dim msgid As Integer = 0
        Dim staffStatus As String = staffInfo.PresenceCategory & staffInfo.PresenceDetail
        Dim UpdateVisitSales As New UpdateSalesVisitBusinessLogic

        '2015/12/21 TCS 中村 受注後工程蓋閉め対応 IT1対応 MOD START
        If String.Equals(staffStatus, STAFF_STATUS_NEGOTIATION) Or String.Equals(staffStatus, STAFF_STATUS_DELIVERY) Then

            Dim endDate As Date = DateTimeFunc.Now(staffInfo.DlrCD)

            If String.Equals(staffStatus, STAFF_STATUS_NEGOTIATION) Then '20：商談中
                UpdateVisitSales.UpdateVisitSalesEnd(registdt(0).CUSTSEGMENT, registdt(0).CRCUSTID.ToString, endDate, rowfunction, msgid, UpdateSalesVisitBusinessLogic.LogicStateNegotiationFinish)
            ElseIf String.Equals(staffStatus, STAFF_STATUS_DELIVERY) Then '22：納車作業中
                UpdateVisitSales.UpdateVisitSalesEnd(registdt(0).CUSTSEGMENT, registdt(0).CRCUSTID.ToString, endDate, rowfunction, msgid, UpdateSalesVisitBusinessLogic.LogicStateDeliverlyFinish)
            End If
            '2015/12/21 TCS 中村 受注後工程蓋閉め対応 IT1対応 MOD END

            If msgid <> 0 Then
                Logger.Error("SC3080203BusinessLogic.InsertActivityResult - Internal Error: UpdateSalesVisitBusinessLogic.UpdateVisitSalesEnd (msgid:" & msgid & ")")
                Me.Rollback = True
                Return False
            End If
        End If

        ' 2020/01/28 TS 舩橋 TKM Change request development for Next Gen e-CRB (CR058,CR061) Start
        ActivityInfoTableAdapter.UpdateSourceChgPossibleFlg(salesid, StaffContext.Current.Account, CONTENT_MODULEID)
        ' 2020/01/28 TS 舩橋 TKM Change request development for Next Gen e-CRB (CR058,CR061) End

        'ステータスを「スタンバイ」に更新
        staffInfo.UpdatePresence("1", "0")

        'CalDAV連携実施
        SetToDo(registdt, fllwStatus)

        '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

        '--デバッグログ---------------------------------------------------
        Logger.Info("InsertActivityResult End")
        '-----------------------------------------------------------------
        Return True
    End Function

    '''2013/06/30 TCS 庄 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 活動新規登録
    ''' </summary>
    ''' <param name="registdt"></param>
    ''' <param name="resistFlg">1:新規活動登録　2:新規活動登録からの即Success、Give-up　3:既存の活動に対する活動結果</param>
    ''' <param name="fllwStatus"></param>
    ''' <param name="contractStatusFlg"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Function InsertNewRequest(ByVal registdt As SC3080203RegistDataDataTable, ByVal resistFlg As String, ByVal fllwStatus As String, ByVal contractStatusFlg As String) As Boolean

        '--デバッグログ---------------------------------------------------
        Logger.Info("InsertNewRequest Start")
        '-----------------------------------------------------------------

        Dim RegistRw As SC3080203DataSet.SC3080203RegistDataRow
        RegistRw = CType(registdt.Rows(0), SC3080203RegistDataRow)
        Dim fllwstrcd As String = RegistRw.FLLWSTRCD
        Dim fllwupboxseqno As Decimal = RegistRw.FLLWSEQ

        Dim sysEnv As New SystemEnvSetting
        Dim sysEnvRow As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow
        sysEnvRow = sysEnv.GetSystemEnvSetting(CONTENT_SUCCESS_CRRSLTID)

        Dim context As StaffContext = StaffContext.Current
        Dim dlrcd As String = context.DlrCD
        '受付顧客車両区分	‘1’ （所有者）
        Dim customerclass As String = "1"

        Dim crsuctid As Decimal = RegistRw.CRCUSTID
        Dim vclid As Decimal = RegistRw.VCLSEQ


        sysEnvRow = sysEnv.GetSystemEnvSetting(CONTENT_WALKIN_WICID)
        Dim source1cd As String = sysEnvRow.PARAMVALUE

        Dim cractrslt As String = Nothing
        '1: Hot  2: Prospect(Warm)  0: Walk-in(Cold)
        If String.Equals(RegistRw.ACTRESULT, C_RSLT_GIVEUP) Then
            cractrslt = CRACTRESULT_GIVEUP
        ElseIf String.Equals(RegistRw.ACTRESULT, C_RSLT_SUCCESS) Then
            cractrslt = CRACTRESULT_SUCCESS
        Else
            cractrslt = CRACTRESULT_HOT
        End If
        Dim lastactdatetime As Date = Date.ParseExact(RegistRw.ACTDAYFROM.Substring(0, 10) & " " & RegistRw.ACTDAYTO, "yyyy/MM/dd HH:mm", Nothing)

        Dim recldatetime As Date = Date.ParseExact(RegistRw.ACTDAYFROM, "yyyy/MM/dd HH:mm", Nothing)

        Dim recdatetime As Date = recldatetime

        Dim brncd As String = context.BrnCD
        Dim staffcd As String = context.Account

        Dim acount As String = context.Account
        Dim rowfunction As String = CONTENT_MODULEID

        Dim attid As Decimal = 0
        Dim count As Long = 1
        RegistRw.COUNT = count
        Dim schedatetime As Date
        schedatetime = Date.ParseExact(RegistRw.ACTDAYFROM, "yyyy/MM/dd HH:mm", Nothing)

        Dim walkinschestart As Date = Date.ParseExact(RegistRw.ACTDAYFROM, "yyyy/MM/dd HH:mm", Nothing)
        Dim walkinscheend As Date = Date.ParseExact(RegistRw.ACTDAYFROM.Substring(0, 10) & " " & RegistRw.ACTDAYTO, "yyyy/MM/dd HH:mm", Nothing)
        Dim dlrcdplan As String = context.DlrCD
        Dim brncdplan As String = context.BrnCD
        Dim staffcdplan As String = context.Account
        Dim schecontactmtd As String = RegistRw.ACTCONTACT
        Dim rsltflg As String = "1"
        Dim rsltdate As Date = walkinschestart
        Dim rsltstaffcd As String = RegistRw.ACTACCOUNT & "@" & context.DlrCD
        Dim rsltcontactmtd As String = RegistRw.ACTCONTACT
        '活動結果ID
        Dim actresult As String = RegistRw.ACTRESULT
        Dim rsltid As String
        'CR活動結果IDを取得
        If String.Equals(actresult, C_RSLT_SUCCESS) Then
            rsltid = sysEnv.GetSystemEnvSetting(CONTENT_SUCCESS_CRRSLTID).PARAMVALUE
        ElseIf String.Equals(actresult, C_RSLT_GIVEUP) Then
            '2013/12/03 TCS 市川 Aカード情報相互連携開発 START
            '断念時は断念理由ID(画面入力)を登録する。
            rsltid = RegistRw.GIVEUP_REASON_ID.ToString()
            '2013/12/03 TCS 市川 Aカード情報相互連携開発 END
        ElseIf String.Equals(actresult, C_RSLT_WALKIN) Then
            rsltid = sysEnv.GetSystemEnvSetting(CONTENT_WALKINREQUEST_CRRSLTID).PARAMVALUE
        ElseIf String.Equals(actresult, C_RSLT_PROSPECT) Then
            If String.Equals(actresult, C_RSLT_WALKIN) Then
                rsltid = sysEnv.GetSystemEnvSetting(CONTENT_WALKINREQUEST_CRRSLTID).PARAMVALUE
            Else
                rsltid = sysEnv.GetSystemEnvSetting(CONTENT_HOTPROSPECT_CRRSLTID).PARAMVALUE
            End If
        Else
            If String.Equals(actresult, C_RSLT_WALKIN) Then
                rsltid = sysEnv.GetSystemEnvSetting(CONTENT_WALKINREQUEST_CRRSLTID).PARAMVALUE
            Else
                rsltid = sysEnv.GetSystemEnvSetting(CONTENT_HOTPROSPECT_CRRSLTID).PARAMVALUE
            End If
        End If

        Dim cstid As Decimal = RegistRw.CRCUSTID
        Dim prospectcd As String = " "
        If String.Equals(RegistRw.ACTRESULT, C_RSLT_HOT) Then
            prospectcd = CRACTSTATUS_HOT
        ElseIf String.Equals(RegistRw.ACTRESULT, C_RSLT_PROSPECT) Then
            prospectcd = CRACTSTATUS_PROSPECT
        ElseIf String.Equals(RegistRw.ACTRESULT, C_RSLT_WALKIN) Then
            prospectcd = CRACTSTATUS_WALKIN
            '2013/06/30 TCS 三宅 2013/10対応版　既存流用 START
        ElseIf String.Equals(RegistRw.ACTRESULT, C_RSLT_SUCCESS) Then
            prospectcd = CRACTSTATUS_HOT
        ElseIf String.Equals(RegistRw.ACTRESULT, C_RSLT_GIVEUP) Then
            prospectcd = CRACTSTATUS_WALKIN
            '2013/06/30 TCS 三宅 2013/10対応版　既存流用 END
        End If
        '商談完了フラグ
        Dim compflg As String = "0"
        If String.Equals(RegistRw.ACTRESULT, C_RSLT_SUCCESS) Or String.Equals(RegistRw.ACTRESULT, C_RSLT_GIVEUP) Then
            compflg = "1"
        Else
            compflg = "0"
        End If

        '2013/06/30 TCS 黄 2013/10対応版　既存流用 START
        Dim giveupvclseq As Long = RegistRw.GIVEUPVCLSEQ
        '2013/06/30 TCS 黄 2013/10対応版　既存流用 END

        '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
        Dim giveupreason As String = " "
        If Not String.IsNullOrEmpty(RegistRw.GIVEUPREASON) Then
            giveupreason = RegistRw.GIVEUPREASON
        End If
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 END

        '用件IDシーケンス取得
        Dim reqid As Decimal = SC3080203TableAdapter.GetSqReqId()

        '活動IDシーケンス取得
        Dim SqActId As Decimal = SC3080203TableAdapter.GetSqActId()

        RegistRw.ATTID = 0

        RegistRw.REQID = reqid

        RegistRw.ACTID = SqActId


        '2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
        Dim orgnzid As Decimal
        orgnzid = SC3080203TableAdapter.GetorgnzId(rsltstaffcd)
        ' 2014/04/01 TCS 松月 TMT不具合対応 Modify Start
        '要件追加
        '2013/12/03 TCS 市川 Aカード情報相互連携開発 START
        SC3080203TableAdapter.InsertRequest(reqid, crsuctid, vclid, customerclass, source1cd, cractrslt,
                                                       lastactdatetime, rsltid, SqActId,
                                                       recldatetime, recdatetime, dlrcd,
                                                       brncd, staffcd, rsltcontactmtd, SqActId,
                                                       acount, rowfunction, fllwupboxseqno, orgnzid)
        '2013/12/03 TCS 市川 Aカード情報相互連携開発 END

        '2013/06/30 TCS 小幡 2013/10対応版　既存流用 START

        ' 2014/04/01 TCS 松月 TMT不具合対応 Modify End
        '活動(結果)追加
        ' 2014/04/01 TCS 松月 TMT不具合対応 Modify Start
        Dim orgnzidplan As Decimal
        orgnzidplan = SC3080203TableAdapter.GetorgnzId(staffcdplan)

        ' 2014/04/14 TCS 松月 【A STEP2】活動予定接触方法設定不正不具合対応（問連TR-V4-GTMC140211005）START
        SC3080203TableAdapter.InsertActivity(SqActId, reqid, RegistRw.ATTID,
                                                       count, schedatetime, walkinschestart,
                                                       walkinscheend, dlrcdplan, brncdplan,
                                                       staffcdplan, schecontactmtd, rsltflg,
                                                       rsltdate, dlrcd, brncd,
                                                       rsltstaffcd, rsltcontactmtd, cractrslt,
                                                       rsltid,
                                                       acount, rowfunction, prospectcd, orgnzid, orgnzidplan, 1, 1)
        ' 2014/04/14 TCS 松月 【A STEP2】活動予定接触方法設定不正不具合対応（問連TR-V4-GTMC140211005）END
        ' 2014/04/01 TCS 松月 TMT不具合対応 Modify End
        '2013/06/30 TCS 小幡 2013/10対応版　既存流用 END

        '2014/05/15 TCS 武田 受注後フォロー機能開発 START
        '商談追加
        SC3080203TableAdapter.InsertSales(fllwupboxseqno, dlrcd, brncd,
                                                       cstid, prospectcd, reqid,
                                                       compflg, giveupvclseq, giveupreason,
                                                       acount, rowfunction, SqActId)
        '2014/05/15 TCS 武田 受注後フォロー機能開発 END

        '2013/12/03 TCS 市川 Aカード情報相互連携開発 START
        '商談一時情報削除
        If SC3080203TableAdapter.MoveSalesTemp(fllwupboxseqno) Then
            If Not SC3080203TableAdapter.DeleteSalesTemp(fllwupboxseqno) Then
                Me.Rollback = True
                Return False
            End If
        Else
            Me.Rollback = True
            Return False
        End If
        '2013/12/03 TCS 市川 Aカード情報相互連携開発 END

        '商談活動追加
        '2015/12/16 TCS 鈴木 受注後工程蓋閉め対応 MOD START
        InsertSalesAct(registdt, resistFlg, fllwStatus, contractStatusFlg)
        '2015/12/16 TCS 鈴木 受注後工程蓋閉め対応 MOD END

        '--デバッグログ---------------------------------------------------
        Logger.Info("InsertNewRequest End")
        '-----------------------------------------------------------------
        Return True
    End Function
    '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

    '''2013/06/30 TCS 庄 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 用件情報更新
    ''' </summary>
    ''' <param name="registdt"></param>
    ''' <param name="resistFlg">1:新規活動登録　2:新規活動登録からの即Success、Give-up　3:既存の活動に対する活動結果</param>
    ''' <param name="fllwStatus"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Function UpdateRequest(ByVal registdt As SC3080203RegistDataDataTable, ByVal resistFlg As String, ByVal fllwStatus As String) As Boolean
        '--デバッグログ---------------------------------------------------
        Logger.Info("UpdateRequest Start")
        '-----------------------------------------------------------------
        Dim RegistRw As SC3080203DataSet.SC3080203RegistDataRow
        RegistRw = CType(registdt.Rows(0), SC3080203RegistDataRow)

        Dim cractrslt As String = Nothing
        '1: Hot  2: Prospect(Warm)  0: Walk-in(Cold)
        If String.Equals(RegistRw.ACTRESULT, C_RSLT_GIVEUP) Then
            cractrslt = CRACTRESULT_GIVEUP
        ElseIf String.Equals(RegistRw.ACTRESULT, C_RSLT_SUCCESS) Then
            cractrslt = CRACTRESULT_SUCCESS
        Else
            cractrslt = CRACTRESULT_HOT
        End If

        Dim context As StaffContext = StaffContext.Current

        Dim fllwupboxseqno As Decimal = RegistRw.FLLWSEQ
        Dim completeflg As String
        If String.Equals(RegistRw.ACTRESULT, C_RSLT_SUCCESS) Or String.Equals(RegistRw.ACTRESULT, C_RSLT_GIVEUP) Then
            completeflg = "1"
        Else
            completeflg = "0"
        End If

        Dim giveupvclseq As Long = RegistRw.GIVEUPVCLSEQ

        Dim account As String = context.Account
        Dim rowuodatefunction As String = CONTENT_MODULEID
        Dim requestlockversion As Long = RegistRw.REQUESTLOCKVERSION
        Dim saleslockversion As Long = RegistRw.SALESLOCKVERSION

        Dim rsltdate As Date
        If Not String.IsNullOrEmpty(RegistRw.ACTDAYFROM) Then
            rsltdate = Convert.ToDateTime(RegistRw.ACTDAYFROM)
        End If
        Dim rsltdatetime As Date = rsltdate
        Dim dlrcd As String = context.DlrCD
        Dim brncd As String = context.BrnCD
        Dim staffcd As String = RegistRw.ACTACCOUNT & "@" & context.DlrCD
        Dim rsltcontactmthd As String = RegistRw.ACTCONTACT

        Dim sysEnv As New SystemEnvSetting
        Dim actresult As String = RegistRw.ACTRESULT
        Dim rsltid As String
        Dim prospectcd As String = " "

        'CR活動結果IDを取得
        '2013/06/30 TCS 三宅 2013/10対応版　既存流用 START
        If String.Equals(actresult, C_RSLT_SUCCESS) Then
            rsltid = sysEnv.GetSystemEnvSetting(CONTENT_SUCCESS_CRRSLTID).PARAMVALUE
            prospectcd = CRACTSTATUS_HOT
        ElseIf String.Equals(actresult, C_RSLT_GIVEUP) Then
            '2013/12/03 TCS 市川 Aカード情報相互連携開発 START
            '断念時は断念理由ID(画面入力)を登録する。
            rsltid = RegistRw.GIVEUP_REASON_ID.ToString()
            '2013/12/03 TCS 市川 Aカード情報相互連携開発 END
            prospectcd = CRACTSTATUS_WALKIN
            '2013/06/30 TCS 三宅 2013/10対応版　既存流用 END
        ElseIf String.Equals(actresult, C_RSLT_WALKIN) Then
            rsltid = sysEnv.GetSystemEnvSetting(CONTENT_WALKINREQUEST_CRRSLTID).PARAMVALUE
            prospectcd = CRACTSTATUS_WALKIN
        ElseIf String.Equals(actresult, C_RSLT_PROSPECT) Then
            rsltid = sysEnv.GetSystemEnvSetting(CONTENT_HOTPROSPECT_CRRSLTID).PARAMVALUE
            prospectcd = CRACTSTATUS_PROSPECT
        Else
            rsltid = sysEnv.GetSystemEnvSetting(CONTENT_HOTPROSPECT_CRRSLTID).PARAMVALUE
            prospectcd = CRACTSTATUS_HOT
        End If

        Dim reqid As Decimal = RegistRw.REQID
        Dim lastactdatetime As Date = Date.ParseExact(RegistRw.ACTDAYFROM.Substring(0, 10) & " " & RegistRw.ACTDAYTO, "yyyy/MM/dd HH:mm", Nothing)
        Dim count As Long = RegistRw.COUNT + 1
        RegistRw.COUNT = count
        Dim giveupreason As String = " "

        If Not String.IsNullOrEmpty(RegistRw.GIVEUPREASON) Then
            giveupreason = RegistRw.GIVEUPREASON
        End If

        '予定活動ID取得
        Dim dt As SC3080203DataSet.SC3080203GetScheDataDataTable = SC3080203TableAdapter.GetScheSqActId(reqid, 0)
        Dim GetScheDataRw As SC3080203DataSet.SC3080203GetScheDataRow = CType(dt.Rows(0), SC3080203GetScheDataRow)
        Dim actid As Decimal = GetScheDataRw.ACT_ID
        Dim actrockverrion As Long = GetScheDataRw.ROW_LOCK_VERSION
        RegistRw.ACTID = actid
        Dim ret As Integer

        '要件更新
        ret = SC3080203TableAdapter.UpdateRequest(reqid, cractrslt, lastactdatetime, count,
                                                       CLng(rsltid), actid,
                                                       account, rowuodatefunction, requestlockversion)
        If ret = 0 Then
            Me.Rollback = True
            Return False
        End If

        '活動(結果)更新
        '2013/06/30 TCS 小幡 2013/10対応版　既存流用 STAR
        Dim orgnzid As Decimal
        orgnzid = SC3080203TableAdapter.GetorgnzId(staffcd)
        ret = SC3080203TableAdapter.UpdateActivity(actid, rsltdate, rsltdatetime, dlrcd,
                                                       brncd, staffcd, rsltcontactmthd,
                                                       cractrslt, rsltid,
                                                       account, rowuodatefunction, actrockverrion,
                                                       prospectcd, orgnzid)
        '2013/06/30 TCS 小幡 2013/10対応版　既存流用 END
        If ret = 0 Then
            Me.Rollback = True
            Return False
        End If

        '商談更新
        '2014/05/15 TCS 武田 受注後フォロー機能開発 START
        ret = SC3080203TableAdapter.UpdateSales(fllwupboxseqno, prospectcd, completeflg, giveupvclseq, giveupreason,
                                                       account, rowuodatefunction, saleslockversion, 0)
        '2014/05/15 TCS 武田 受注後フォロー機能開発 END
        If ret = 0 Then
            Me.Rollback = True
            Return False
        End If

        '--デバッグログ---------------------------------------------------
        Logger.Info("UpdateRequest End")
        '-----------------------------------------------------------------
        Return True
    End Function
    '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

    '''2013/06/30 TCS 庄 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 誘致情報更新
    ''' </summary>
    ''' <param name="registdt"></param>
    ''' <param name="resistFlg">1:新規活動登録　2:新規活動登録からの即Success、Give-up　3:既存の活動に対する活動結果</param>
    ''' <param name="fllwStatus"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Function UpdateAttract(ByVal registdt As SC3080203RegistDataDataTable, ByVal resistFlg As String, ByVal fllwStatus As String) As Boolean
        '--デバッグログ---------------------------------------------------
        Logger.Info("UpdateAttract Start")
        '-----------------------------------------------------------------

        Dim context As StaffContext = StaffContext.Current
        Dim RegistRw As SC3080203DataSet.SC3080203RegistDataRow
        RegistRw = CType(registdt.Rows(0), SC3080203RegistDataRow)
        Dim attid As Decimal = RegistRw.ATTID
        Dim dateto As String = RegistRw.ACTDAYFROM.Substring(0, 10) & " " & RegistRw.ACTDAYTO
        Dim lastactdate As Date = Date.ParseExact(RegistRw.ACTDAYFROM.Substring(0, 10), "yyyy/MM/dd", Nothing)
        Dim lastactdatetime As Date = Date.ParseExact(dateto, "yyyy/MM/dd HH:mm", Nothing)
        Dim lastrsltid As String = RegistRw.ACTRESULT

        Dim count As Long = RegistRw.COUNT + 1
        RegistRw.COUNT = count

        Dim account As String = context.Account
        Dim rowuodatefunction As String = CONTENT_MODULEID
        Dim attractlockversion As Long = RegistRw.ATTRACTLOCKVERSION
        Dim saleslockversion As Long = RegistRw.SALESLOCKVERSION

        Dim rsltdate As Date
        '2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
        If Not String.IsNullOrEmpty(RegistRw.ACTDAYFROM) Then
            rsltdate = Convert.ToDateTime(RegistRw.ACTDAYFROM)
        End If
        '2013/06/30 TCS 小幡 2013/10対応版　既存流用 END
        Dim rsltdatetime As Date = rsltdate
        Dim dlrcd As String = context.DlrCD
        Dim brncd As String = context.BrnCD
        Dim staffcd As String = RegistRw.ACTACCOUNT & "@" & context.DlrCD
        Dim rsltcontactmthd As String = RegistRw.ACTCONTACT
        Dim thistimecractstatus As String = RegistRw.ACTRESULT

        Dim sysEnv As New SystemEnvSetting
        Dim actresult As String = RegistRw.ACTRESULT
        Dim rsltid As String
        Dim prospectcd As String = " "
        'CR活動結果IDを取得
        '2013/06/30 TCS 三宅 2013/10対応版　既存流用 START
        If String.Equals(actresult, C_RSLT_SUCCESS) Then
            rsltid = sysEnv.GetSystemEnvSetting(CONTENT_SUCCESS_CRRSLTID).PARAMVALUE
            prospectcd = CRACTSTATUS_HOT
        ElseIf String.Equals(actresult, C_RSLT_GIVEUP) Then
            '2013/12/03 TCS 市川 Aカード情報相互連携開発 START
            '断念時は断念理由ID(画面入力)を登録する。
            rsltid = RegistRw.GIVEUP_REASON_ID.ToString()
            '2013/12/03 TCS 市川 Aカード情報相互連携開発 END
            prospectcd = CRACTSTATUS_WALKIN
            '2013/06/30 TCS 三宅 2013/10対応版　既存流用 END
        ElseIf String.Equals(actresult, C_RSLT_WALKIN) Then
            rsltid = sysEnv.GetSystemEnvSetting(CONTENT_WALKINREQUEST_CRRSLTID).PARAMVALUE
            prospectcd = CRACTSTATUS_WALKIN
        ElseIf String.Equals(actresult, C_RSLT_PROSPECT) Then
            rsltid = sysEnv.GetSystemEnvSetting(CONTENT_HOTPROSPECT_CRRSLTID).PARAMVALUE
            prospectcd = CRACTSTATUS_PROSPECT
        Else
            rsltid = sysEnv.GetSystemEnvSetting(CONTENT_HOTPROSPECT_CRRSLTID).PARAMVALUE
            prospectcd = CRACTSTATUS_HOT
        End If

        '2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
        Dim cractrslt As String = Nothing
        '1: Hot  2: Prospect(Warm)  0: Walk-in(Cold)
        If String.Equals(RegistRw.ACTRESULT, C_RSLT_GIVEUP) Then
            cractrslt = CRACTRESULT_GIVEUP
        ElseIf String.Equals(RegistRw.ACTRESULT, C_RSLT_SUCCESS) Then
            cractrslt = CRACTRESULT_SUCCESS
        Else
            cractrslt = CRACTRESULT_HOT
        End If
        '2013/06/30 TCS 小幡 2013/10対応版　既存流用 END

        '予定活動ID取得
        Dim dt As SC3080203DataSet.SC3080203GetScheDataDataTable = SC3080203TableAdapter.GetScheSqActId(0, attid)
        Dim GetScheDataRw As SC3080203DataSet.SC3080203GetScheDataRow = CType(dt.Rows(0), SC3080203GetScheDataRow)
        Dim actid As Decimal = GetScheDataRw.ACT_ID
        Dim actrockverrion As Long = GetScheDataRw.ROW_LOCK_VERSION
        RegistRw.ACTID = actid
        Dim ret As Integer

        Dim giveupreason As String = " "
        If Not String.IsNullOrEmpty(RegistRw.GIVEUPREASON) Then
            giveupreason = RegistRw.GIVEUPREASON
        End If

        '2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
        '誘致更新
        ret = SC3080203TableAdapter.UpdateAttract(attid, cractrslt, lastactdate, count,
                                                     lastrsltid, actid,
                                                     account, rowuodatefunction, attractlockversion)
        '2013/06/30 TCS 小幡 2013/10対応版　既存流用 END
        If ret = 0 Then
            Me.Rollback = True
            Return False
        End If

        '活動(結果)更新
        '2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
        Dim orgnzid As Decimal
        orgnzid = SC3080203TableAdapter.GetorgnzId(staffcd)
        ret = SC3080203TableAdapter.UpdateActivity(actid, rsltdate, rsltdatetime, dlrcd,
                                                       brncd, staffcd, rsltcontactmthd,
                                                       cractrslt, rsltid,
                                                       account, rowuodatefunction, actrockverrion,
                                                       prospectcd, orgnzid)
        '2013/06/30 TCS 小幡 2013/10対応版　既存流用 END
        If ret = 0 Then
            Me.Rollback = True
            Return False
        End If

        '商談更新
        Dim fllwupboxseqno As Decimal = RegistRw.FLLWSEQ
        Dim completeflg As String
        If String.Equals(RegistRw.ACTRESULT, C_RSLT_SUCCESS) Or String.Equals(RegistRw.ACTRESULT, C_RSLT_GIVEUP) Then
            completeflg = "1"
        Else
            completeflg = "0"
        End If

        '2014/05/15 TCS 武田 受注後フォロー機能開発 START
        '初回商談活動判定
        Dim fstSalesDt As SC3080203DataSet.SC3080203FstSalesActMasterDataTable = SC3080203TableAdapter.GetFstSalesAct(fllwupboxseqno)
        Dim fstSalesRw As SC3080203DataSet.SC3080203FstSalesActMasterRow = CType(fstSalesDt.Rows(0), SC3080203FstSalesActMasterRow)
        Dim firstSalesActId As Decimal

        '初回商談活動IDが設定されていない場合
        If fstSalesRw.FIRST_SALES_ACT_ID = 0 Then
            '初回商談活動IDを取得
            firstSalesActId = GetFirstSalesActId(attid)
        End If

        Dim giveupvclseq As Long = RegistRw.GIVEUPVCLSEQ
        '商談更新
        ret = SC3080203TableAdapter.UpdateSales(fllwupboxseqno, prospectcd, completeflg, giveupvclseq, giveupreason,
                                                       account, rowuodatefunction, saleslockversion, firstSalesActId)

        '2014/05/15 TCS 武田 受注後フォロー機能開発 END
        If ret = 0 Then
            Me.Rollback = True
            Return False
        End If


        '--デバッグログ---------------------------------------------------
        Logger.Info("UpdateAttract End")
        '-----------------------------------------------------------------
        Return True
    End Function
    '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

    '''2013/06/30 TCS 庄 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 商談活動追加
    ''' </summary>
    ''' <param name="registdt"></param>
    ''' <param name="resistFlg">1:新規活動登録　2:新規活動登録からの即Success、Give-up　3:既存の活動に対する活動結果</param>
    ''' <param name="fllwStatus"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Function InsertSalesAct(ByVal registdt As SC3080203RegistDataDataTable, ByVal resistFlg As String, ByVal fllwStatus As String, ByVal contractStatusFlg As String) As Boolean

        '■Function InsertSalesAct(ByVal registdt As SC3080203RegistDataDataTable, ByVal resistFlg As String, ByVal fllwStatus As String) As Boolean
        ' <param name="contractStatusFlg">契約状況フラグ</param>

        '--デバッグログ---------------------------------------------------
        Logger.Info("InsertSalesAct Start")
        '-----------------------------------------------------------------
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

        Dim RegistRw As SC3080203DataSet.SC3080203RegistDataRow
        RegistRw = CType(registdt.Rows(0), SC3080203RegistDataRow)

        'ログインユーザー情報取得用
        Dim context As StaffContext = StaffContext.Current
        '変数の宣言
        'ログイン情報系
        Dim dlrcd As String = context.DlrCD         '自身の店舗コード
        Dim strcd As String = context.BrnCD         '自身の販売店コード
        Dim fllwstrcd As String = RegistRw.FLLWSTRCD
        Dim account As String = context.Account     '自身のアカウント
        Dim actaccount As String = RegistRw.ACTACCOUNT & "@" & context.DlrCD       '活動実施者(画面で入力した値)
        ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
        Dim success_series As String() = RegistRw.SUCCESSSERIES.Split(";"c)
        ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 END

        Dim rowfunction As String = CONTENT_MODULEID

        ' 2014/04/01 TCS 松月 TMT不具合対応 Modify Start
        Dim orgnzid As Decimal
        orgnzid = SC3080203TableAdapter.GetorgnzId(actaccount)
        ' 2014/04/01 TCS 松月 TMT不具合対応 Modify End



        Dim cntcd As String = Nothing       '国コード
        Dim cstkind As String = Nothing     '顧客区分 1:自社客 2:未取引客
        Dim cstid As String = Nothing       '未取引客ID
        Dim cstkindwk As Long = Nothing     '未取引客IDワーク
        Dim originalid As String = " "      '自社客連番
        Dim vin As String = " "             'VIN

        Dim newcustcarseq As Nullable(Of Long) = Nothing      '未取引客車両連番

        Dim seriescd As String = Nothing    'シリーズコード
        Dim makername As String = Nothing   'メーカー名

        Dim walkinidwk As Long = Nothing 'ウォークインIDワーク
        Dim walkinid As String = Nothing    'ウォークインID

        Dim actresult As String = RegistRw.ACTRESULT    '活動結果(画面で入力した値)

        'Dim seriescode As String = Nothing      '
        Dim seriesname As String = Nothing      '
        Dim registrationtype As String = Nothing '1:Staff Follow-up Box,3:Success/Give-up

        Dim wicid As String = Nothing            '来店区分
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
        Dim fllwupbox_seqno As Decimal = RegistRw.FLLWSEQ   'Follow-upBox内連番
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

        Dim crcustid As String = Nothing        '未取引顧客ID、自社客ID、副顧客IDのいずれかを設定
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
        Dim carid As Decimal = Nothing           'Vin or 未取車両seq
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

        Dim totalhisseq As Long = Nothing
        Dim service_nm As String = ""

        Dim customerclass As String = "1"       '1:所有者、2:使用者、3:その他

        Dim catalog As String = ""
        Dim testdrive As String = ""
        Dim assessment As String = ""
        Dim valuation As String = ""
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
        Dim lockversion As String = ""
        Dim valuationPrice As String = ""
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

        Dim appointtimeflg As String = "1"       '次回活動時間時分指定フラグ 0:なし、1:あり(あり固定で投入)

        cstkind = RegistRw.cstkind               '顧客区分 1:自社客 2:未取引客
        crcustid = RegistRw.INSDID
        carid = RegistRw.VCLSEQ
        cntcd = EnvironmentSetting.CountryCode


        If String.Equals(RegistRw.ACTRESULT, C_RSLT_SUCCESS) Or String.Equals(RegistRw.ACTRESULT, C_RSLT_GIVEUP) Then
            registrationtype = "3"
        Else
            registrationtype = "1"
        End If

        'Toは時分しか持っていないためFrom側から日付をセット
        Dim actdayto As String = RegistRw.ACTDAYFROM.Substring(0, 10) & " " & RegistRw.ACTDAYTO
        Dim actDayToDate As Date = Date.ParseExact(actdayto, "yyyy/MM/dd HH:mm", Nothing)

        Dim actdate As Date                    '活動日(画面で入力した値)
        actdate = actDayToDate

        Dim cractivedate As Date = Nothing          '次回活動日
        If String.Equals(RegistRw.FOLLOWFLG, "1") Then
            If String.Equals(RegistRw.FOLLOWDAYTOFLG, "1") Then
                'appointtimeflg = "1"
                cractivedate = Date.ParseExact(RegistRw.FOLLOWDAYFROM.Substring(0, 10) & " " & RegistRw.FOLLOWDAYTO, "yyyy/MM/dd HH:mm", Nothing)
            Else
                cractivedate = Date.ParseExact(RegistRw.FOLLOWDAYFROM, "yyyy/MM/dd HH:mm", Nothing)
                'appointtimeflg = "0"
            End If
            appointtimeflg = If(RegistRw.FOLLOWTIMEFLG, "1", "0")
        Else
            If String.Equals(RegistRw.NEXTACTDAYTOFLG, "1") Then
                'appointtimeflg = "1"
                cractivedate = Date.ParseExact(RegistRw.NEXTACTDAYFROM.Substring(0, 10) & " " & RegistRw.NEXTACTDAYTO, "yyyy/MM/dd HH:mm", Nothing)
            Else
                'appointtimeflg = "0"
                cractivedate = Date.ParseExact(RegistRw.NEXTACTDAYFROM, "yyyy/MM/dd HH:mm", Nothing)
            End If
            appointtimeflg = If(RegistRw.NEXTACTTIMEFLG, "1", "0")
        End If

        Dim prospect_date As Nullable(Of Date) = Nothing
        Dim hot_date As Nullable(Of Date) = Nothing
        If String.Equals(actresult, C_RSLT_HOT) Then
            hot_date = Now()
        ElseIf String.Equals(actresult, C_RSLT_PROSPECT) Then
            prospect_date = Now()
        End If


        '配列作成
        Dim wkary As String()
        Dim tempary As String()
        Dim seqdt As SC3080203SeqDataTable
        Dim seqrw As SC3080203SeqRow
        '全希望車種のSEQのリストを作成
        Dim selcar As String = ""
        ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
        Dim selockversion As String = ""
        ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 END
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
        seqdt = SC3080203TableAdapter.GetActHisCarSeq(fllwupbox_seqno)
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 END
        For j As Integer = 0 To seqdt.Count - 1
            seqrw = CType(seqdt.Rows(j), SC3080203SeqRow)
            selcar = selcar & seqrw.SEQNO & ","
            ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
            selockversion = selockversion & seqrw.LOCKVERSION & ","
            ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 END
        Next
        'カタログ実績がある希望車種のSEQのリストを作成
        catalog = ""
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
        Dim fllwupboxseqno As Decimal = RegistRw.FLLWSEQ
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 END
        wkary = RegistRw.SELECTACTCATALOG.Split(";"c)
        For i = 0 To wkary.Length - 2
            tempary = wkary(i).Split(","c)
            If String.Equals(tempary(1), "1") Then
                '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
                seqdt = SC3080203TableAdapter.GetActHisSelCarSeq(fllwupboxseqno, CDec(tempary(0)))
                '2013/06/30 TCS 庄 2013/10対応版　既存流用 END
                For j = 0 To seqdt.Count - 1
                    seqrw = CType(seqdt.Rows(j), SC3080203SeqRow)
                    catalog = catalog & seqrw.SEQNO & ","
                Next

            End If
        Next

        '試乗実績がある希望車種のSEQのリストを作成
        testdrive = ""
        wkary = RegistRw.SELECTACTTESTDRIVE.Split(";"c)
        For i = 0 To wkary.Length - 2
            tempary = wkary(i).Split(","c)
            If String.Equals(tempary(1), "1") Then
                '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
                seqdt = SC3080203TableAdapter.GetActHisSelCarSeq(fllwupboxseqno, CDec(tempary(0)))
                '2013/06/30 TCS 庄 2013/10対応版　既存流用 END
                For j = 0 To seqdt.Count - 1
                    seqrw = CType(seqdt.Rows(j), SC3080203SeqRow)
                    testdrive = testdrive & seqrw.SEQNO & ","
                Next
            End If
        Next

        '2013/03/07 TCS 橋本 【A.STEP2】新車タブレット受付画面の管理指標変更対応 MOD START
        '見積り実績(見積りは全部に対して有り、無しの2択)
        assessment = RegistRw.SELECTACTASSESMENT

        Dim assessmentNo As Long = 0    '査定No


        '査定実績(査定は全希望車種に対して同じ区分を適用)
        assessment = GetRegActAsmStatus(RegistRw.SELECTACTASSESMENT, dlrcd, fllwstrcd, fllwupbox_seqno, assessmentNo)


        '2013/03/07 TCS 橋本 【A.STEP2】新車タブレット受付画面の管理指標変更対応 MOD END

        '査定実績がある希望車種のSEQのリストを作成
        valuation = ""
        lockversion = ""
        valuationPrice = ""
        wkary = RegistRw.SELECTACTVALUATION.Split(";"c)

        '2015/12/15 TCS 鈴木 受注後工程蓋閉め対応 START
        '希望車種のSEQリスト
        Dim selectactvaluation As String()
        selectactvaluation = wkary
        '2015/12/15 TCS 鈴木 受注後工程蓋閉め対応 END

        '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
        For i = 0 To wkary.Length - 2
            tempary = wkary(i).Split(","c)
            If String.Equals(tempary(1), "1") Then
                seqdt = SC3080203TableAdapter.GetActHisSelCarSeq(fllwupboxseqno, CDec(tempary(0)))
                For j = 0 To seqdt.Count - 1
                    seqrw = CType(seqdt.Rows(j), SC3080203SeqRow)
                    valuation = valuation & seqrw.SEQNO & ","
                    lockversion = lockversion & seqrw.LOCKVERSION & ","
                Next
                valuationPrice = valuationPrice & tempary(2) & ","
            End If
        Next
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

        '各活動実績の登録
        '登録に必要なデータ取得
        '各シーケンスNo用DataTable

        '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
        Dim actdateDt As Date = actdate

        '実績登録用にフォローアップボックスのデータを取得
        Dim ActHisFllwdt As SC3080203DataSet.SC3080203ActHisFllwDataTable
        Dim ActHisFllwrw As SC3080203DataSet.SC3080203ActHisFllwRow = Nothing
        ActHisFllwdt = SC3080203TableAdapter.GetActHisFllw(fllwupboxseqno, dlrcd)
        Dim ActHisFllwCrplan_id As Nullable(Of Long)
        Dim ActHisFllwPromotion_id As Nullable(Of Long)
        If ActHisFllwdt.Rows.Count > 0 Then
            ActHisFllwrw = CType(ActHisFllwdt.Rows(0), SC3080203ActHisFllwRow)
            If ActHisFllwrw.IsCRPLAN_IDNull Then
                ActHisFllwCrplan_id = Nothing
            Else
                ActHisFllwCrplan_id = ActHisFllwrw.CRPLAN_ID
            End If
            If ActHisFllwrw.IsPROMOTION_IDNull Then
                ActHisFllwPromotion_id = Nothing
            Else
                ActHisFllwPromotion_id = ActHisFllwrw.PROMOTION_ID
            End If
        End If
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

        'カタログのマスタ情報取得
        Dim ActHisCatalogdt As SC3080203DataSet.SC3080203ActHisContentDataTable
        Dim ActHisCatalogrw As SC3080203DataSet.SC3080203ActHisContentRow
        ActHisCatalogdt = SC3080203TableAdapter.GetActHisContent(CONTENT_SEQ_CATALOG)
        ActHisCatalogrw = CType(ActHisCatalogdt.Rows(0), SC3080203ActHisContentRow)

        Dim ActHisCatalogCategorydvsid As Nullable(Of Long)
        If ActHisCatalogrw.IsCATEGORYDVSIDNull Then
            ActHisCatalogCategorydvsid = Nothing
        Else
            ActHisCatalogCategorydvsid = ActHisCatalogrw.CATEGORYDVSID
        End If

        '試乗のマスタ情報取得()
        Dim ActHisTestDrivedt As SC3080203DataSet.SC3080203ActHisContentDataTable
        Dim ActHisTestDriverw As SC3080203DataSet.SC3080203ActHisContentRow
        ActHisTestDrivedt = SC3080203TableAdapter.GetActHisContent(CONTENT_SEQ_TESTDRIVE)
        ActHisTestDriverw = CType(ActHisTestDrivedt.Rows(0), SC3080203ActHisContentRow)

        Dim ActHisTestDriveCategorydvsid As Nullable(Of Long)
        If ActHisTestDriverw.IsCATEGORYDVSIDNull Then
            ActHisTestDriveCategorydvsid = Nothing
        Else
            ActHisTestDriveCategorydvsid = ActHisTestDriverw.CATEGORYDVSID
        End If

        '査定のマスタ情報取得
        Dim ActHisAssessmentdt As SC3080203DataSet.SC3080203ActHisContentDataTable
        Dim ActHisAssessmentrw As SC3080203DataSet.SC3080203ActHisContentRow
        ActHisAssessmentdt = SC3080203TableAdapter.GetActHisContent(CONTENT_SEQ_ASSESSMENT)
        ActHisAssessmentrw = CType(ActHisAssessmentdt.Rows(0), SC3080203ActHisContentRow)

        Dim ActHisAssessmentCategorydvsid As Nullable(Of Long)
        If ActHisAssessmentrw.IsCATEGORYDVSIDNull Then
            ActHisAssessmentCategorydvsid = Nothing
        Else
            ActHisAssessmentCategorydvsid = ActHisAssessmentrw.CATEGORYDVSID
        End If

        '見積りのマスタ情報取得
        Dim ActHisValuationdt As SC3080203DataSet.SC3080203ActHisContentDataTable
        Dim ActHisValuationrw As SC3080203DataSet.SC3080203ActHisContentRow
        ActHisValuationdt = SC3080203TableAdapter.GetActHisContent(CONTENT_SEQ_VALUATION)
        ActHisValuationrw = CType(ActHisValuationdt.Rows(0), SC3080203ActHisContentRow)

        Dim ActHisValuationCategorydvsid As Nullable(Of Long)
        If ActHisValuationrw.IsCATEGORYDVSIDNull Then
            ActHisValuationCategorydvsid = Nothing
        Else
            ActHisValuationCategorydvsid = ActHisValuationrw.CATEGORYDVSID
        End If

        Dim selcarary As String() = selcar.Split(","c)
        ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
        Dim seVersion As String() = selockversion.Split(","c)
        ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 END
        Dim catalogary As String() = catalog.Split(","c)
        Dim testdriveary As String() = testdrive.Split(","c)
        Dim valuationary As String() = valuation.Split(","c)
        Dim valuationaryPrice As String() = valuationPrice.Split(","c)
        Dim valVersion As String() = lockversion.Split(","c)
        cntcd = EnvironmentSetting.CountryCode

        '実績がある場合
        Dim salesid As Decimal = fllwupbox_seqno
        Dim rsltdate As Date

        rsltdate = Date.ParseExact(RegistRw.ACTDAYFROM.Substring(0, 10) & " " & RegistRw.ACTDAYTO, "yyyy/MM/dd HH:mm", Nothing)

        Dim staffcd As String = context.Account
        Dim rsltcontactmthd As String = RegistRw.ACTCONTACT
        Dim ActHisSelCardt As SC3080203DataSet.SC3080203ActHisSelCarDataTable
        Dim ActHisSelCarrw As SC3080203DataSet.SC3080203ActHisSelCarRow
        Dim acount As String = context.Account
        Dim actid As Decimal = RegistRw.ACTID
        If Not String.IsNullOrEmpty(catalogary(0)) Then
            SC3080203TableAdapter.DeleteBrochure(salesid)
        End If
        Dim rsltsalescat As String = " "
        Dim createctactresult As String = " "
        If String.Equals(RegistRw.ACTRESULT, C_RSLT_WALKIN) Then
            createctactresult = CRACTSTATUS_WALKIN
        ElseIf String.Equals(RegistRw.ACTRESULT, C_RSLT_PROSPECT) Then
            createctactresult = CRACTSTATUS_PROSPECT
        ElseIf String.Equals(RegistRw.ACTRESULT, C_RSLT_HOT) Then
            createctactresult = CRACTSTATUS_HOT
        End If
        '2013/06/30 TCS 宋 2013/10対応版　既存流用 START
        '査定処理フラグ
        Dim sateflg As Boolean = True
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
        For i = 0 To selcarary.Length - 2
            ActHisSelCardt = SC3080203TableAdapter.GetActHisCarSeq(fllwupboxseqno, CDec(selcarary(i)), dlrcd)
            Dim ActHisSelCaVclmodel_Name As String = " "
            Dim ActHisSelCardisp_Bdy_Color As String = " "
            Dim modelcd As String = " "
            ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
            Dim successSeries As String() = success_series(i).Split(","c)
            Dim cractrslt As String = " "
            Dim lock_ver As Long = CLng(seVersion(i))
            Dim req As Integer
            ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 END

            If ActHisSelCardt.Rows.Count > 0 Then
                ActHisSelCarrw = CType(ActHisSelCardt.Rows(0), SC3080203ActHisSelCarRow)

                If ActHisSelCarrw.IsVCLMODEL_NAMENull Then
                    ActHisSelCaVclmodel_Name = " "
                Else
                    ActHisSelCaVclmodel_Name = ActHisSelCarrw.VCLMODEL_NAME
                End If

                If ActHisSelCarrw.IsDISP_BDY_COLORNull Then
                    ActHisSelCardisp_Bdy_Color = " "
                Else
                    ActHisSelCardisp_Bdy_Color = ActHisSelCarrw.DISP_BDY_COLOR
                End If

                If Not String.IsNullOrEmpty(ActHisSelCarrw.SERIESNM) Then
                    modelcd = ActHisSelCarrw.SERIESNM
                End If


                Dim modelcode As String = modelcd
                Dim modelname As String = ActHisSelCaVclmodel_Name


                'カタログ実績確認
                For j = 0 To catalogary.Length - 2
                    If selcarary(i) = catalogary(j) Then
                        rsltsalescat = "2"
                        '商談活動IDシーケンス取得
                        Dim salesactid As Decimal = SC3080203TableAdapter.GetSqSalesActId()
                        '商談活動追加
                        SC3080203TableAdapter.InsertSalesActivity(salesactid, fllwupboxseqno, actid,
                                                                       rsltsalescat, createctactresult, modelcode,
                                                                       " ",
                                                                       acount, rowfunction)
                        ' 2014/04/01 TCS 松月 TMT不具合対応 Modify Start
                        SC3080203TableAdapter.InsertBrochure(salesid, modelcd, rsltdate,
                                                                       staffcd, rsltcontactmthd,
                                                                       acount, rowfunction, orgnzid)
                        ' 2014/04/01 TCS 松月 TMT不具合対応 Modify End
                    End If
                Next

                '試乗実績確認
                For j = 0 To testdriveary.Length - 2
                    If selcarary(i) = testdriveary(j) Then
                        rsltsalescat = "4"
                        '商談活動IDシーケンス取得
                        Dim salesactid As Decimal = SC3080203TableAdapter.GetSqSalesActId()
                        '商談活動追加
                        SC3080203TableAdapter.InsertSalesActivity(salesactid, fllwupboxseqno, actid,
                                                                       rsltsalescat, createctactresult, modelcode,
                                                                       " ",
                                                                       acount, rowfunction)
                        ' 2013/06/30 TCS 王 2013/10対応版　既存流用 START
                        '試乗予約ID取得
                        Dim testdriveid As Decimal = SC3080203TableAdapter.GetReqTestDriveId()
                        Dim actDayfromDate As Date = Date.ParseExact(RegistRw.ACTDAYFROM, "yyyy/MM/dd HH:mm", Nothing)
                        '試乗予約追加
                        ' 2014/04/01 TCS 松月 TMT不具合対応 Modify Start
                        SC3080203TableAdapter.InsertTestDrive(testdriveid, dlrcd, strcd,
                                                              modelcd, modelname, RegistRw.CRCUSTID, fllwupboxseqno,
                                                              rsltdate, actDayfromDate, rsltdate,
                                                              staffcd, acount, rowfunction, orgnzid)
                        ' 2014/04/01 TCS 松月 TMT不具合対応 Modify End
                        ' 2013/06/30 TCS 王 2013/10対応版　既存流用 END
                    End If
                Next

                '査定実績確認
                If String.Equals(assessment, "1") Then
                    rsltsalescat = "7"
                    '査定情報取得
                    Dim vclname As String = " "
                    Dim actasminfo As SC3080203DataSet.ActAsmInfoDataTable
                    Dim actasmrow As SC3080203DataSet.ActAsmInfoRow
                    Dim iCount As Integer
                    Dim asmseq As Long

                    '2013/06/30 TCS 宋 2013/10対応版　既存流用 START
                    '査定処理フラグはTRUEの場合のみ実施
                    If sateflg Then
                        actasminfo = SC3080203TableAdapter.GetActAsmInfo(fllwupboxseqno)
                        If actasminfo.Rows.Count > 0 Then
                            actasmrow = CType(actasminfo.Rows(0), SC3080203DataSet.ActAsmInfoRow)
                            asmseq = actasmrow.ASSMNTSEQ
                            For iCount = 0 To actasminfo.Rows.Count - 1
                                actasmrow = CType(actasminfo.Rows(iCount), SC3080203DataSet.ActAsmInfoRow)

                                '査定実績登録
                                ' 2014/04/01 TCS 松月 TMT不具合対応 Modify Start
                                SC3080203TableAdapter.SetAssessmentAct(fllwupboxseqno, asmseq + iCount,
                                                                               actasmrow.VEHICLENAME, staffcd,
                                                                               actasmrow.APPRISAL_PRICE,
                                                                               acount, rowfunction, orgnzid)
                                ' 2014/04/01 TCS 松月 TMT不具合対応 Modify End
                                vclname = actasmrow.VEHICLENAME

                                '商談活動IDシーケンス取得
                                Dim salesactid As Decimal = SC3080203TableAdapter.GetSqSalesActId()
                                '商談活動追加
                                SC3080203TableAdapter.InsertSalesActivity(salesactid, fllwupboxseqno, actid,
                                                                               rsltsalescat, createctactresult, " ",
                                                                               vclname,
                                                                               acount, rowfunction)
                            Next
                        Else
                            '商談活動IDシーケンス取得
                            Dim salesactid As Decimal = SC3080203TableAdapter.GetSqSalesActId()
                            '商談活動追加
                            SC3080203TableAdapter.InsertSalesActivity(salesactid, fllwupboxseqno, actid,
                                                                           rsltsalescat, createctactresult, " ",
                                                                           vclname,
                                                                           acount, rowfunction)
                        End If

                        sateflg = False
                    End If
                    '2013/06/30 TCS 宋 2013/10対応版　既存流用 END
                End If

                '2019/04/05 TS  河原 見積印刷実績がある状態で活動結果登録を行うとエラーになる件 START
                '見積実績確認
                If Not ActivityInfoBusinessLogic.UpdateEstimateAmount(salesid, RegistRw.SALESLOCKVERSION, CInt(selcarary(i)), rsltcontactmthd, staffcd, account, rowfunction, lock_ver, orgnzid, actid, createctactresult, modelcd, modelname) Then
                    Me.Rollback = True
                    Return False
                End If
                '2019/04/05 TS  河原 見積印刷実績がある状態で活動結果登録を行うとエラーになる件 END

                If String.Equals(assessment, C_ASMACTSTATUS_MIKAITOU) Then
                    Dim bfafdvs As String = " "
                    If Not ActHisFllwrw Is Nothing Then
                        bfafdvs = ActHisFllwrw.BFAFDVS
                    End If
                    '査定依頼機能を使用している場合、且つ査定実績がある場合、且つ査定依頼未回答の場合
                    ActivityInfoTableAdapter.InsertFllwupBoxCrHisAsm(assessmentNo, dlrcd, fllwstrcd, fllwupboxseqno, actid, ActHisFllwCrplan_id, bfafdvs,
                                                                     ActHisFllwrw.CRDVSID, ActHisFllwrw.INSDID, ActHisFllwrw.SERIESCODE, ActHisFllwrw.SERIESNAME,
                                                                     account, ActHisFllwrw.VCLREGNO, ActHisFllwrw.SUBCTGCODE, ActHisFllwrw.SERVICECD,
                                                                     ActHisFllwrw.SUBCTGORGNAME, ActHisFllwrw.SUBCTGORGNAME_EX, ActHisFllwPromotion_id,
                                                                     ActHisFllwrw.CRACTRESULT, ActHisFllwrw.PLANDVS, actdateDt, ActHisAssessmentrw.METHOD,
                                                                     ActHisAssessmentrw.ACTION, ActHisAssessmentrw.ACTIONTYPE, ActHisFllwrw.ACCOUNT_PLAN,
                                                                     ActHisAssessmentrw.ACTIONCD, CONTENT_SEQ_ASSESSMENT,
                                                                     Long.Parse(selcarary(i), CultureInfo.CurrentCulture()), ActHisSelCarrw.SERIESNM,
                                                                     ActHisSelCaVclmodel_Name, ActHisSelCardisp_Bdy_Color, ActHisSelCarrw.QUANTITY, _
                                                                     salesid,
                                                                     ActHisAssessmentrw.CATEGORYID.ToString(CultureInfo.CurrentCulture()),
                                                                     ActHisAssessmentCategorydvsid, ActHisFllwrw.VIN, ActHisFllwrw.CUSTCHRGSTAFFNM,
                                                                     ActHisFllwrw.CRCUSTID, ActHisFllwrw.CUSTOMERCLASS)
                End If
                ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
                If String.Equals(RegistRw.ACTRESULT, C_RSLT_SUCCESS) Or String.Equals(RegistRw.ACTRESULT, C_RSLT_GIVEUP) Then
                    '成約車両に選択した場合

                    ' 2015/12/11 TCS 鈴木 受注後工程蓋閉め対応 START
                    '商談行ロック
                    Dim salesversion As Long = RegistRw.SALESLOCKVERSION

                    If Not String.Equals(contractStatusFlg, "1") Then
                        '契約済み以外の場合
                        ' 2015/12/11 TCS 鈴木 受注後工程蓋閉め対応 END

                        If (successSeries(1).Equals("1")) Then
                            cractrslt = CRACTRESULT_SUCCESS
                            '見積の成約フラグを更新
                            SC3080203TableAdapter.GetEstimateLock(salesid, selcarary(i))
                            req = SC3080203TableAdapter.UpdateSuccessFlag(salesid, selcarary(i), acount, rowfunction)
                        Else
                            cractrslt = CRACTRESULT_GIVEUP
                        End If

                        '希望車のステータスを更新
                        '2014/01/29 TCS 松月 【A STEP2】成約活動ID設定漏れ対応（号口切替BTS-15）START
                        req = SC3080203TableAdapter.UpdateSalesstatus(salesid, selcarary(i), cractrslt, acount, rowfunction, lock_ver, actid)
                        '2014/01/29 TCS 松月 【A STEP2】成約活動ID設定漏れ対応（号口切替BTS-15）END
                        If req = 0 Then
                            Me.Rollback = True
                            Return False
                        End If

                        ' 2015/12/11 TCS 鈴木 受注後工程蓋閉め対応 START
                    ElseIf String.Equals(contractStatusFlg, "1") Then
                        ' 2015/12/11 TCS 鈴木 受注後工程蓋閉め対応 END
                        '契約済みの場合
                        If (Not ActivityInfoBusinessLogic.InsertSalesActContracted(actid, salesid, acount, rsltcontactmthd, salesversion, assessment, actdayto, selectactvaluation)) Then
                            Return False
                        End If
                    End If
                End If
                ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 END
            End If
        Next

        '2013/12/03 TCS 市川 Aカード情報相互連携開発 START
        If String.Equals(RegistRw.ACTRESULT, C_RSLT_SUCCESS) Then
            '成約結果登録時、成約以外の見積を削除する
            SC3080203TableAdapter.GetEstimateDelLock(salesid)
            SC3080203TableAdapter.UpdateEstimateDel(salesid, acount, rowfunction)
        End If
        '2013/12/03 TCS 市川 Aカード情報相互連携開発 END

        'Follow-up Box商談メモ追加
        Dim crcstid As Decimal = RegistRw.CRCUSTID
        SC3080203TableAdapter.InsertFllwupboxSalesmemo(fllwupboxseqno, crcstid, carid, actid, acount)

        '2013/06/30 TCS 松月 2013/10対応版　既存流用 START

        'Follow-up Box商談メモWK削除
        SC3080203TableAdapter.DeleteFllwupboxSalesmemowk(fllwupbox_seqno)
        '2013/06/30 TCS 松月 2013/10対応版　既存流用 END

        '一発Success、Give-up時に2回プロセス実績が登録されないように空にする
        RegistRw.SELECTACTCATALOG = ""
        RegistRw.SELECTACTTESTDRIVE = ""
        RegistRw.SELECTACTVALUATION = ""
        RegistRw.SELECTACTASSESMENT = ""

        '--デバッグログ---------------------------------------------------
        Logger.Info("InsertSalesAct End")
        '-----------------------------------------------------------------
        Return True
    End Function
    '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

    '''2013/06/30 TCS 庄 2013/10対応版　既存流用 START
    ''' <summary>
    ''' History移動
    ''' </summary>
    ''' <param name="isrequest"></param>
    ''' <param name="reqid"></param>
    ''' <param name="salesid"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Function MoveHistory(ByVal isrequest As Boolean, ByVal reqid As Decimal, ByVal salesid As Decimal) As Boolean
        '--デバッグログ---------------------------------------------------
        Logger.Info("MoveHistory Start")
        '-----------------------------------------------------------------

        Dim req As SC3080203DataSet.SC3080203ActionidDataTable
        '用件の場合
        If isrequest Then
            SC3080203TableAdapter.MoveRequest(reqid)
            SC3080203TableAdapter.MoveActivity(reqid, 0)
            req = SC3080203TableAdapter.SelectActionID(reqid, 0)
        Else
            SC3080203TableAdapter.MoveAttract(reqid)
            SC3080203TableAdapter.MoveAttractCall(reqid)
            SC3080203TableAdapter.MoveAttractDM(reqid)
            SC3080203TableAdapter.MoveAttractRMM(reqid)
            SC3080203TableAdapter.MoveActivity(0, reqid)
            req = SC3080203TableAdapter.SelectActionID(0, reqid)
        End If

        Dim seqrw As SC3080203ActionidRow
        For i = 0 To req.Rows.Count - 1
            seqrw = CType(req.Rows(i), SC3080203ActionidRow)
            SC3080203TableAdapter.MoveActivityMemo(seqrw.ACT_ID)
        Next
        SC3080203TableAdapter.MoveSales(salesid)
        SC3080203TableAdapter.MoveSalesAct(salesid)
        SC3080203TableAdapter.MovePreferVcl(salesid)
        SC3080203TableAdapter.MoveCompetitorVcl(salesid)
        SC3080203TableAdapter.MoveBrochure(salesid)
        SC3080203TableAdapter.MoveTestDrive(salesid)
        '2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
        SC3080203TableAdapter.MoveAssessmentAct(salesid)
        '2013/06/30 TCS 小幡 2013/10対応版　既存流用 END

        '用件の場合
        If isrequest Then
            SC3080203TableAdapter.DeleteRequest(reqid)
            SC3080203TableAdapter.DeleteActivity(reqid, 0)
        Else
            SC3080203TableAdapter.DeleteAttract(reqid)
            SC3080203TableAdapter.DeleteAttractCall(reqid)
            SC3080203TableAdapter.DeleteAttractDM(reqid)
            SC3080203TableAdapter.DeleteAttractRMM(reqid)
            SC3080203TableAdapter.DeleteActivity(0, reqid)
        End If

        For i = 0 To req.Rows.Count - 1
            seqrw = CType(req.Rows(i), SC3080203ActionidRow)
            SC3080203TableAdapter.DeleteActivityMemo(seqrw.ACT_ID)
        Next
        SC3080203TableAdapter.DeleteSales(salesid)
        SC3080203TableAdapter.DeleteSalesAct(salesid)
        SC3080203TableAdapter.DeletePreferVcl(salesid)
        SC3080203TableAdapter.DeleteCompetitorVcl(salesid)
        SC3080203TableAdapter.DeleteBrochure(salesid)
        SC3080203TableAdapter.DeleteTestDrive(salesid)
        '2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
        SC3080203TableAdapter.DeleteAssessmentAct(salesid)
        '2013/06/30 TCS 小幡 2013/10対応版　既存流用 END
        '--デバッグログ---------------------------------------------------
        Logger.Info("MoveHistory End")
        '-----------------------------------------------------------------
        Return True
    End Function
    '2013/06/30 TCS 庄 2013/10対応版　既存流用 END


    ' 2012/03/13 TCS 安田 【SALES_2】 START
    ''' <summary>
    ''' ステータスの取得
    ''' </summary>
    ''' <remarks></remarks>
    Public Function GetStaffStatus() As String

        Dim staffInfo As StaffContext = StaffContext.Current
        Dim staffStatus As String = staffInfo.PresenceCategory & staffInfo.PresenceDetail

        Return staffStatus

    End Function

    ''' <summary>
    ''' 来店実績更新_商談終了時のPush送信
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub PushUpdateVisitSalesEnd(ByVal staffStatus As String)

        If String.Equals(staffStatus, STAFF_STATUS_NEGOTIATION) Then
            '活動登録処理完了時、
            '来店実績更新_商談終了時のPush送信
            Dim UpdateVisitSales As New UpdateSalesVisitBusinessLogic
            '2012/09/06 TCS 山口 【A STEP2】次世代e-CRB 新車受付機能改善 START
            '処理区分に1:商談終了を設定して呼び出し
            UpdateVisitSales.PushUpdateVisitSalesEnd(UpdateSalesVisitBusinessLogic.LogicStateNegotiationFinish)
            '2012/09/06 TCS 山口 【A STEP2】次世代e-CRB 新車受付機能改善 END
        End If

    End Sub
    ' 2012/03/13 TCS 安田 【SALES_2】 END

    ''' <summary>
    ''' 活動名を生成する
    ''' </summary>
    ''' <param name="servicename"></param>
    ''' <param name="promoname"></param>
    ''' <param name="condition"></param>
    ''' <param name="planid"></param>
    ''' <param name="actcategory"></param>
    ''' <param name="promoid"></param>
    ''' <param name="reqcategory"></param>
    ''' <param name="reqname"></param>
    ''' <param name="actresult"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetActName(ByVal servicename As String, ByVal promoname As String, ByVal condition As String, ByVal planid As Nullable(Of Long),
                               ByVal actcategory As String, ByVal promoid As Nullable(Of Long), ByVal reqcategory As String,
                               ByVal reqname As String, ByVal actresult As String) As String

        '--デバッグログ---------------------------------------------------
        Logger.Info("GetActName Start")
        '-----------------------------------------------------------------
        Const C_ACTCTG_PERIODICAL = "1"         ' CR活動カテゴリ／Periodical Inspection
        Const C_ACTCTG_REPURCHASE = "2"         ' CR活動カテゴリ／Repurchase Follow-up
        Const C_ACTCTG_BIRTHDAY = "4"           ' CR活動カテゴリ／Birthday
        Const C_CONDITION_ONE = "1"             ' 計画外管理-作成状態／One Time
        Const C_CONDITION_EVERY = "2"           ' 計画外管理-作成状態／Every Month
        Const C_REQCTG_WALKIN = "1"             ' リクエストカテゴリ／Walk-in
        Const C_REQCTG_CALLIN = "2"             ' リクエストカテゴリ／Call-in
        Const C_REQCTG_RMM = "3"                ' リクエストカテゴリ／RMM
        Const C_REQCTG_REQUEST = "4"            ' リクエストカテゴリ／Request
        Const C_CRRESULT_HOT = "1"              ' CR活動結果／Hot
        Const C_CRRESULT_PROSPECT = "2"         ' CR活動結果／Prospect

        Dim recactname As String                       ' 編集済活動名
        Dim prmonth As String '付属年月

        recactname = ""
        prmonth = ""

        ' Follow-up Box
        Select Case actcategory
            Case C_ACTCTG_PERIODICAL, C_ACTCTG_REPURCHASE, C_ACTCTG_BIRTHDAY
                ' 1:Periodical Inspection 2:Repurchase Follow-up 4:Birthday
                recactname = servicename
            Case Else
                If Not promoid Is Nothing Then
                    ' プロモーションIDがNULLでない
                    recactname = promoname
                    If String.IsNullOrEmpty(condition) = False Then
                        Select Case condition
                            Case C_CONDITION_ONE    ' One Time
                                prmonth = ""
                            Case C_CONDITION_EVERY  ' Every Month
                                prmonth = Mid(planid & "", 1, 8)
                                Dim prmonthwk As Date
                                prmonthwk = CDate(prmonth)
                                prmonth = DateTimeFunc.FormatDate(12, prmonthwk)
                        End Select
                    End If

                    recactname = recactname & prmonth
                Else
                    ' プロモーションIDがNULL
                    Select Case reqcategory
                        Case C_REQCTG_CALLIN, C_REQCTG_RMM, C_REQCTG_REQUEST
                            ' 2:Call-in 3:RMM 4:Request
                            If String.IsNullOrEmpty(reqname) = False Then
                                recactname = WebWordUtility.GetWord(30351) & " (" & reqname & ")"      'Request Follow-up
                            Else
                                recactname = WebWordUtility.GetWord(30351)                             'Request Follow-up
                            End If
                        Case C_REQCTG_WALKIN
                            ' Walk-in
                            recactname = WebWordUtility.GetWord(30352)                                'Walk-in Follow-up
                        Case Else
                            recactname = ""
                    End Select
                End If
        End Select

        ' CR活動結果

        If String.IsNullOrEmpty(actresult) = False Then
            Select Case actresult
                Case C_CRRESULT_HOT         ' Hot
                    If String.IsNullOrEmpty(Trim(recactname)) = False Then
                        recactname = WebWordUtility.GetWord(30353) & " (" & recactname & ")"           'Hot
                    Else
                        recactname = WebWordUtility.GetWord(30353)                                  'Hot
                    End If
                Case C_CRRESULT_PROSPECT    ' Prospect
                    If String.IsNullOrEmpty(Trim(recactname)) = False Then
                        recactname = WebWordUtility.GetWord(30354) & " (" & recactname & ")"           'Prospect
                    Else
                        recactname = WebWordUtility.GetWord(30354)                                   'Prospect
                    End If
                Case Else
                    ' そのまま出力
            End Select
        Else
            ' そのまま出力
        End If
        '--デバッグログ---------------------------------------------------
        Logger.Info("GetActName End")
        '-----------------------------------------------------------------
        Return recactname
    End Function

    '2013/12/24 TCS 市川 Aカード情報相互連携開発 追加要望 START
    ''' <summary>
    ''' 入力チェック処理
    ''' </summary>
    ''' <param name="registdt"></param>
    ''' <param name="newCustDt"></param>
    ''' <param name="custKind"></param>
    ''' <param name="msgid"></param>
    ''' <param name="msgItem0"></param>
    ''' <param name="contractflg">契約状況フラグ</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function IsInputeCheck(ByVal registdt As SC3080203RegistDataDataTable, _
                                  ByVal newCustDt As ActivityInfoDataSet.GetNewCustomerDataTable, _
                                  ByVal custKind As String, _
                                  ByRef msgid As Integer, _
                                  ByRef msgItem0 As String, _
                                  ByVal contractflg As String) As Boolean
        '2013/12/24 TCS 市川 Aカード情報相互連携開発 追加要望 END

        '--デバッグログ---------------------------------------------------
        Logger.Info("IsInputeCheck Start")
        '-----------------------------------------------------------------

        '2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 START
        '未取引客の場合のみダミー名称、電話番号のチェックを行う
        If CSTKIND_NEW.Equals(custKind) Then
            Dim newCustRw As ActivityInfoDataSet.GetNewCustomerRow = CType(newCustDt.Rows(0), ActivityInfoDataSet.GetNewCustomerRow)

            'ダミー名称の場合
            If DummyNameFlgDummy.Equals(newCustRw.DUMMYNAMEFLG) Then
                msgid = 30933
                Return False
            End If

            '2013/12/03 TCS 市川 Aカード情報相互連携開発 DELETE
        End If
        '2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 END

        Dim Registrw As SC3080203RegistDataRow
        Registrw = CType(registdt.Rows(0), SC3080203RegistDataRow)

        '2013/12/03 TCS 市川 Aカード情報相互連携開発 START
        '必須入力チェック
        '2013/12/24 TCS 市川 Aカード情報相互連携開発 追加要望 START
        '2014/08/01 TCS 市川 受注後フォロー機能開発(UAT-BTS-212対応) START
        If Not ActivityInfoBusinessLogic.MandatoryCheck(StaffContext.Current.DlrCD, StaffContext.Current.BrnCD, Registrw.CRCUSTID, Registrw.FLLWSEQ, msgid, msgItem0, False) Then
            '2014/08/01 TCS 市川 受注後フォロー機能開発(UAT-BTS-212対応) END
            '2013/12/24 TCS 市川 Aカード情報相互連携開発 追加要望 END
            Return False
        End If
        '2013/12/03 TCS 市川 Aカード情報相互連携開発 END

        '開始時間[活動内容]を入力してください
        If String.IsNullOrEmpty(Registrw.ACTDAYFROM) Then
            msgid = 30901
            Return False
        End If

        If IsDate(Registrw.ACTDAYFROM) = False Then
            msgid = 30902
            Return False
        End If

        '終了時間[活動内容]を入力してください
        If String.IsNullOrEmpty(Registrw.ACTDAYTO) Then
            msgid = 30903
            Return False
        End If

        '終了時間[活動内容]を日付形式で入力してください
        If Validation.IsDate(14, Registrw.ACTDAYTO) = False Then
            msgid = 30904
            Return False
        End If

        '終了時間[活動内容]を開始時間[活動内容]より未来の時間で入力してください
        If Registrw.ACTDAYFROM.Substring(11, 5) > Registrw.ACTDAYTO Then
            msgid = 30905
            Return False
        End If

        '日付[活動内容]を現在より過去の日時で入力してください
        'If CDate(Registrw.ACTDAYFROM) > Now() Then
        '    msgid = 30906
        '    Return False
        'End If
        Dim actdayto As String = Registrw.ACTDAYFROM.Substring(0, 10) & " " & Registrw.ACTDAYTO '活動開始の年月日＋活動終了時間
        Dim actDayToDate As Date = Date.ParseExact(actdayto, "yyyy/MM/dd HH:mm", Nothing)
        '2013/03/13 TCS 葛西 GL0875対応 START
        Dim actdayfrom As String = Registrw.ACTDAYFROM.Substring(0, 10)
        Dim actDayFromDate As Date = Date.ParseExact(actdayfrom, "yyyy/MM/dd", Nothing)
        If actDayFromDate > Today() Then
            msgid = 30906
            Return False
        End If
        '2013/03/13 TCS 葛西 GL0875対応 END

        '終了時間を前回の活動終了時間{0}より未来の時間で入力してください
        If Registrw.IsLATEST_TIME_ENDNull = False Then
            'Dim actdayto As String = Registrw.ACTDAYFROM.Substring(0, 10) & " " & Registrw.ACTDAYTO '活動開始の年月日＋活動終了時間
            'Dim actDayToDate As Date = Date.ParseExact(actdayto, "yyyy/MM/dd HH:mm", Nothing)
            If actDayToDate <= Registrw.LATEST_TIME_END Then
                msgid = MSG_30932
                Return False
            End If
        End If

        '対応SCを選択してください
        If String.IsNullOrEmpty(Registrw.ACTACCOUNT) Then
            msgid = 30907
            Return False
        End If

        '分類[活動内容]を選択してください
        If String.IsNullOrEmpty(Registrw.ACTCONTACT) Then
            msgid = 30908
            Return False
        End If

        '活動結果を選択してください
        If String.IsNullOrEmpty(Registrw.ACTRESULT) Then
            msgid = 30909
            Return False
        End If

        '分類[次回活動]を選択してください
        If Registrw.ACTRESULT = C_RSLT_WALKIN Or Registrw.ACTRESULT = C_RSLT_PROSPECT Or Registrw.ACTRESULT = C_RSLT_HOT Then
            If String.IsNullOrEmpty(Registrw.NEXTACTCONTACT) Then
                msgid = 30910
                Return False
            End If
        End If

        '期限[次回活動]を入力してください
        If Registrw.ACTRESULT = C_RSLT_WALKIN Or Registrw.ACTRESULT = C_RSLT_PROSPECT Or Registrw.ACTRESULT = C_RSLT_HOT Then
            If String.Equals(Registrw.NEXTACTDAYTOFLG, "0") Then
                If String.IsNullOrEmpty(Registrw.NEXTACTDAYFROM) Then
                    msgid = 30911
                    Return False
                End If
            End If
        End If

        '開始時間[次回活動]を入力してください
        If Registrw.ACTRESULT = C_RSLT_WALKIN Or Registrw.ACTRESULT = C_RSLT_PROSPECT Or Registrw.ACTRESULT = C_RSLT_HOT Then
            If String.Equals(Registrw.NEXTACTDAYTOFLG, "1") And String.IsNullOrEmpty(Registrw.NEXTACTDAYFROM) Then
                msgid = 30913
                Return False
            End If
        End If

        '終了時間[次回活動]を入力してください
        If Registrw.ACTRESULT = C_RSLT_WALKIN Or Registrw.ACTRESULT = C_RSLT_PROSPECT Or Registrw.ACTRESULT = C_RSLT_HOT Then
            If String.Equals(Registrw.NEXTACTDAYTOFLG, "1") Then
                If String.IsNullOrEmpty(Registrw.NEXTACTDAYTO) Then
                    msgid = 30915
                    Return False
                End If
            End If
        End If

        '終了時間[次回活動]を開始時間[次回活動]より未来の時間で入力してください
        If Registrw.ACTRESULT = C_RSLT_WALKIN Or Registrw.ACTRESULT = C_RSLT_PROSPECT Or Registrw.ACTRESULT = C_RSLT_HOT Then
            If String.Equals(Registrw.NEXTACTDAYTOFLG, "1") Then
                If Registrw.NEXTACTDAYFROM.Substring(11, 5) > Registrw.NEXTACTDAYTO Then
                    msgid = 30917
                    Return False
                End If
            End If
        End If

        '予約フォローを選択してください
        If Registrw.ACTRESULT = C_RSLT_WALKIN Or Registrw.ACTRESULT = C_RSLT_PROSPECT Or Registrw.ACTRESULT = C_RSLT_HOT Then
            If String.Equals(Registrw.FOLLOWFLG, "1") Then
                If String.IsNullOrEmpty(Registrw.FOLLOWCONTACT) Then
                    msgid = 30918
                    Return False
                End If
            End If
        End If

        '期限[予約フォロー]を入力してください
        If Registrw.ACTRESULT = C_RSLT_WALKIN Or Registrw.ACTRESULT = C_RSLT_PROSPECT Or Registrw.ACTRESULT = C_RSLT_HOT Then
            If String.Equals(Registrw.FOLLOWFLG, "1") Then
                If String.Equals(Registrw.FOLLOWDAYTOFLG, "0") Then
                    If String.IsNullOrEmpty(Registrw.FOLLOWDAYFROM) Then
                        msgid = 30919
                        Return False
                    End If
                End If
            End If
        End If

        '開始時間[予約フォロー]を入力してください
        If Registrw.ACTRESULT = C_RSLT_WALKIN Or Registrw.ACTRESULT = C_RSLT_PROSPECT Or Registrw.ACTRESULT = C_RSLT_HOT Then
            If String.Equals(Registrw.FOLLOWFLG, "1") Then
                If String.Equals(Registrw.FOLLOWDAYTOFLG, "1") Then
                    If String.IsNullOrEmpty(Registrw.FOLLOWDAYFROM) Then
                        msgid = 30921
                        Return False
                    End If
                End If
            End If
        End If

        '終了時間[予約フォロー]を入力してください
        If Registrw.ACTRESULT = C_RSLT_WALKIN Or Registrw.ACTRESULT = C_RSLT_PROSPECT Or Registrw.ACTRESULT = C_RSLT_HOT Then
            If String.Equals(Registrw.FOLLOWFLG, "1") Then
                If String.Equals(Registrw.FOLLOWDAYTOFLG, "1") Then
                    If String.IsNullOrEmpty(Registrw.FOLLOWDAYTO) Then
                        msgid = 30923
                        Return False
                    End If
                End If
            End If
        End If

        '終了時間[予約フォロー]を開始時間[予約フォロー]より未来の時間で入力してください
        If Registrw.ACTRESULT = C_RSLT_WALKIN Or Registrw.ACTRESULT = C_RSLT_PROSPECT Or Registrw.ACTRESULT = C_RSLT_HOT Then
            If String.Equals(Registrw.FOLLOWFLG, "1") Then
                If String.Equals(Registrw.FOLLOWDAYTOFLG, "1") Then
                    If Registrw.FOLLOWDAYFROM.Substring(11, 5) > Registrw.FOLLOWDAYTO Then
                        msgid = 30925
                        Return False
                    End If
                End If
            End If
        End If

        '日付[予約フォロー]を日付[次回活動]より未来の日時で入力してください
        If Registrw.ACTRESULT = C_RSLT_WALKIN Or Registrw.ACTRESULT = C_RSLT_PROSPECT Or Registrw.ACTRESULT = C_RSLT_HOT Then
            If String.Equals(Registrw.FOLLOWFLG, "1") Then
                Dim nextActDayFromWK As Date
                If Registrw.NEXTACTDAYFROM.Length = 10 Then
                    nextActDayFromWK = DateTimeFunc.FormatString("yyyy/MM/dd HH:mm", Registrw.NEXTACTDAYFROM & " 23:59")
                Else
                    nextActDayFromWK = DateTimeFunc.FormatString("yyyy/MM/dd HH:mm", Registrw.NEXTACTDAYFROM)
                End If
                Dim followDayFromWK As Date
                If Registrw.FOLLOWDAYFROM.Length = 10 Then
                    followDayFromWK = DateTimeFunc.FormatString("yyyy/MM/dd HH:mm", Registrw.FOLLOWDAYFROM & " 00:00")
                Else
                    followDayFromWK = DateTimeFunc.FormatString("yyyy/MM/dd HH:mm", Registrw.FOLLOWDAYFROM)
                End If
                If nextActDayFromWK < followDayFromWK Then
                    msgid = 30926
                    Return False
                End If
            End If
        End If

        '2015/12/11 TCS 鈴木 受注後工程蓋閉め対応 START
        If Not String.Equals(contractflg, "1") Then
            '契約済以外の場合、必須チェクを外す
            '2015/12/11 TCS 鈴木 受注後工程蓋閉め対応 END
            '受注車両を選択してください
            If Registrw.ACTRESULT = C_RSLT_SUCCESS Then
                Dim flg As Boolean = False
                Dim SuccessSeriesSet As String()
                SuccessSeriesSet = Registrw.SUCCESSSERIES.Split(";"c)
                Dim SuccessSeries As String()
                For i = 0 To SuccessSeriesSet.Length - 2
                    SuccessSeries = SuccessSeriesSet(i).Split(","c)
                    If String.Equals(SuccessSeries(1), "1") Then
                        flg = True
                    End If
                Next
                If flg = False Then
                    msgid = 30927
                    Return False
                End If
            End If
        End If

        '2013/12/03 TCS 市川 Aカード情報相互連携開発 START
        If Registrw.ACTRESULT = C_RSLT_GIVEUP Then
            If Registrw.IsGIVEUP_REASON_IDNull OrElse Registrw.GIVEUP_REASON_ID = 0 Then
                '断念理由(選択)
                msgid = 30929
                Return False

            Else
                Dim otherReasonId As Decimal = 0
                If Decimal.TryParse(GetBrnEnvSettingValue(StaffContext.Current.DlrCD, StaffContext.Current.BrnCD, ENVSETTINGKEY_GIVEUP_REASON_OTHER), otherReasonId) _
                    AndAlso Registrw.GIVEUP_REASON_ID = otherReasonId Then

                    '断念理由(その他：手書き)
                    '文字数
                    If (Validation.IsCorrectDigit(Registrw.GIVEUPREASON, 256) = False) Then
                        msgid = 30928
                        Return False
                    End If
                    '禁則文字チェック
                    If Not Validation.IsValidString(Registrw.GIVEUPREASON) Then
                        msgid = 30930
                        Return False
                    End If
                End If
            End If

        End If
        '2013/12/03 TCS 市川 Aカード情報相互連携開発 END

        '--デバッグログ---------------------------------------------------
        Logger.Info("IsInputeCheck End OK")
        '-----------------------------------------------------------------
        Return True
    End Function

    ''' <summary>
    ''' ToDoリスト登録
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function SetToDo(ByVal registdt As SC3080203RegistDataDataTable, ByVal fllwstatsu As String) As Boolean
        '--デバッグログ---------------------------------------------------
        Logger.Info("SetToDo Start")
        '-----------------------------------------------------------------

        'ログインユーザー情報取得用
        Dim context As StaffContext = StaffContext.Current
        Dim sysEnv As New SystemEnvSetting
        Dim sysEnvRow As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow
        sysEnvRow = sysEnv.GetSystemEnvSetting(CONTENT_KEISYO_ZENGO)
        Dim nmtitledt As SC3080203NameTitleDataTable
        Dim RegistRw As SC3080203DataSet.SC3080203RegistDataRow
        RegistRw = CType(registdt.Rows(0), SC3080203RegistDataRow)
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
        nmtitledt = SC3080203TableAdapter.GetOrgNameTitle(RegistRw.INSDID)
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
        Dim nmtitlerw As SC3080203NameTitleRow
        nmtitlerw = CType(nmtitledt.Rows(0), SC3080203NameTitleRow)
        Using sendObj As New IC3040401.IC3040401BusinessLogic
            '共通設定項目作成
            sendObj.CreateCommon()
            '親レコード設定
            sendObj.ActionType = "0"
            sendObj.DealerCode = context.DlrCD

            '2014/09/01 TCS 松月 問連TR-V4-GTMC140807001対応 START
            sendObj.BranchCode = ActivityInfoTableAdapter.GetPreBrnCd(RegistRw.FLLWSEQ)
            '2014/09/01 TCS 松月 問連TR-V4-GTMC140807001対応 END

            sendObj.ScheduleDivision = "0"
            sendObj.ScheduleId = RegistRw.FLLWSEQ.ToString(CultureInfo.CurrentCulture())
            sendObj.ActivityCreateStaffCode = context.Account

            If String.Equals(RegistRw.ACTRESULT, C_RSLT_SUCCESS) Or String.Equals(RegistRw.ACTRESULT, C_RSLT_GIVEUP) Then
                'SuccessかGive-upの場合
                sendObj.CompleteFlg = "3"
                sendObj.CompletionDate = Format(Now, "yyyy/MM/dd HH:mm:ss")
            Else
                If String.IsNullOrEmpty(fllwstatsu) Then
                    sendObj.CompleteFlg = "1"
                Else
                    sendObj.CompleteFlg = "2"
                End If
                If String.Equals(RegistRw.cstkind, "1") Then
                    sendObj.CustomerDivision = "0"
                Else
                    sendObj.CustomerDivision = "2"
                End If
                sendObj.CustomerId = RegistRw.INSDID
                sendObj.CustomerName = nmtitlerw.NAME
                If nmtitlerw.IsNAMETITLENull Then
                    sendObj.NameTitle = ""
                Else
                    sendObj.NameTitle = nmtitlerw.NAMETITLE
                End If
                sendObj.NameTitlePosition = sysEnvRow.PARAMVALUE
            End If

            If String.Equals(RegistRw.ACTRESULT, C_RSLT_WALKIN) Or String.Equals(RegistRw.ACTRESULT, C_RSLT_PROSPECT) Or String.Equals(RegistRw.ACTRESULT, C_RSLT_HOT) Then
                '子レコード作成
                sendObj.CreateScheduleInfo()
                '子レコードプロパティ設定
                sendObj.ActivityStaffBranchCode(0) = context.BrnCD
                sendObj.ActivityStaffCode(0) = context.Account
                Dim cntnmdt As SC3080203GetContactNmDataTable
                Dim cntnmrw As SC3080203GetContactNmRow
                Dim clrdt As SC3080203TodoColorDataTable
                Dim clrrw As SC3080203TodoColorRow

                If String.Equals(RegistRw.FOLLOWFLG, "1") Then
                    'フォロー有(2レコード作るパターン)
                    If String.Equals(RegistRw.FOLLOWDAYTOFLG, "1") Then
                        'From-To
                        sendObj.StartTime(0) = RegistRw.FOLLOWDAYFROM
                        sendObj.EndTime(0) = RegistRw.FOLLOWDAYFROM.Substring(0, 10) & " " & RegistRw.FOLLOWDAYTO
                    Else
                        '納期のみ
                        If RegistRw.FOLLOWTIMEFLG Then
                            '時間指定あり
                            sendObj.EndTime(0) = RegistRw.FOLLOWDAYFROM
                        Else
                            '時間指定なし(日付のみセット)
                            sendObj.EndTime(0) = RegistRw.FOLLOWDAYFROM.Substring(0, 10)
                        End If
                    End If

                    sendObj.AlarmNo(0) = RegistRw.FOLLOWALERT
                    sendObj.ContactNo(0) = RegistRw.FOLLOWCONTACT
                    cntnmdt = SC3080203TableAdapter.GetContactNM(Long.Parse(RegistRw.FOLLOWCONTACT, CultureInfo.CurrentCulture()))
                    cntnmrw = CType(cntnmdt.Rows(0), SC3080203GetContactNmRow)
                    sendObj.ContactName(0) = cntnmrw.CONTACT
                    sendObj.ComingFollowName(0) = WebWordUtility.GetWord("SCHEDULE", 1)
                    '色取得
                    clrdt = SC3080203TableAdapter.GetToDoColor("XXXXX", "1", "0", "1", Long.Parse(RegistRw.FOLLOWCONTACT, CultureInfo.CurrentCulture()))
                    clrrw = CType(clrdt.Rows(0), SC3080203TodoColorRow)
                    sendObj.BackgroundColor(0) = clrrw.BACKGROUNDCOLOR
                    '子レコード作成
                    sendObj.CreateScheduleInfo()
                    sendObj.ActivityStaffBranchCode(1) = context.BrnCD
                    sendObj.ActivityStaffCode(1) = context.Account

                    '次回活動のチップ
                    If String.Equals(RegistRw.NEXTACTDAYTOFLG, "1") Then
                        'From-To
                        sendObj.StartTime(1) = RegistRw.NEXTACTDAYFROM
                        sendObj.EndTime(1) = RegistRw.NEXTACTDAYFROM.Substring(0, 10) & " " & RegistRw.NEXTACTDAYTO
                    Else
                        '納期のみ
                        If RegistRw.NEXTACTTIMEFLG Then
                            '時間指定あり
                            sendObj.EndTime(1) = RegistRw.NEXTACTDAYFROM
                        Else
                            '時間指定なし(日付のみセット)
                            sendObj.EndTime(1) = RegistRw.NEXTACTDAYFROM.Substring(0, 10)
                        End If
                    End If

                    sendObj.AlarmNo(1) = RegistRw.NEXTACTALERT
                    sendObj.ContactNo(1) = RegistRw.NEXTACTCONTACT
                    cntnmdt = SC3080203TableAdapter.GetContactNM(Long.Parse(RegistRw.NEXTACTCONTACT, CultureInfo.CurrentCulture()))
                    cntnmrw = CType(cntnmdt.Rows(0), SC3080203GetContactNmRow)
                    sendObj.ContactName(1) = cntnmrw.CONTACT
                    '色取得
                    clrdt = SC3080203TableAdapter.GetToDoColor("XXXXX", "1", "0", "0", Long.Parse(RegistRw.NEXTACTCONTACT, CultureInfo.CurrentCulture()))
                    clrrw = CType(clrdt.Rows(0), SC3080203TodoColorRow)
                    sendObj.BackgroundColor(1) = clrrw.BACKGROUNDCOLOR
                Else
                    'フォロー無
                    If String.Equals(RegistRw.NEXTACTDAYTOFLG, "1") Then
                        'From-To
                        sendObj.StartTime(0) = RegistRw.NEXTACTDAYFROM
                        sendObj.EndTime(0) = RegistRw.NEXTACTDAYFROM.Substring(0, 10) & " " & RegistRw.NEXTACTDAYTO
                    Else
                        '納期のみ
                        If RegistRw.NEXTACTTIMEFLG Then
                            '時間指定あり
                            sendObj.EndTime(0) = RegistRw.NEXTACTDAYFROM
                        Else
                            '時間指定なし(日付のみセット)
                            sendObj.EndTime(0) = RegistRw.NEXTACTDAYFROM.Substring(0, 10)
                        End If

                    End If
                    sendObj.AlarmNo(0) = RegistRw.NEXTACTALERT
                    sendObj.ContactNo(0) = RegistRw.NEXTACTCONTACT
                    cntnmdt = SC3080203TableAdapter.GetContactNM(Long.Parse(RegistRw.NEXTACTCONTACT, CultureInfo.CurrentCulture()))
                    cntnmrw = CType(cntnmdt.Rows(0), SC3080203GetContactNmRow)
                    sendObj.ContactName(0) = cntnmrw.CONTACT
                    '色取得
                    clrdt = SC3080203TableAdapter.GetToDoColor("XXXXX", "1", "0", "0", Long.Parse(RegistRw.NEXTACTCONTACT, CultureInfo.CurrentCulture()))
                    '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
                    If clrdt.Rows.Count > 0 Then
                        clrrw = CType(clrdt.Rows(0), SC3080203TodoColorRow)
                        sendObj.BackgroundColor(0) = clrrw.BACKGROUNDCOLOR
                    End If
                    '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
                End If
            End If
            'Webサービス連携を実施:引数は対象URL
            Dim errCd As String
            Dim dlrenvdt As New DealerEnvSetting
            Dim dlrenvrw As DlrEnvSettingDataSet.DLRENVSETTINGRow
            dlrenvrw = dlrenvdt.GetEnvSetting("XXXXX", C_CALDAV_WEBSERVICE_URL)
            '対象URLはDLRENVSETTINGより取得する
            errCd = sendObj.SendScheduleInfo(dlrenvrw.PARAMVALUE)
            'errCd = 1
            If String.Equals(errCd, "0") = False Then
                'エラー処理
                Logger.Error("SC3080203BusinessLogic.SetToDo - Internal Error: IC3040401BusinessLogic.SendScheduleInfo(" & dlrenvrw.PARAMVALUE & ") (errCd:" & errCd & ")")
                Return False
            End If
        End Using
        '--デバッグログ---------------------------------------------------
        Logger.Info("SetToDo End")
        '-----------------------------------------------------------------
        Return True
    End Function


    ''' <summary>
    ''' tbl_FLLWUPBOXRSLT_DONEで使用するカテゴリの設定
    ''' </summary>
    ''' <param name="fllwuptyp"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function getFllwupDoneCategory(ByVal fllwuptyp As String) As String
        '--デバッグログ---------------------------------------------------
        Logger.Info("getFllwupDoneCategory Start")
        '-----------------------------------------------------------------
        Dim doneCategory As String = ""
        'カテゴリ設定
        Select Case fllwuptyp
            Case C_FLLWUP_HOT
                doneCategory = C_DONECAT_HOT
            Case C_FLLWUP_PROSPECT
                doneCategory = C_DONECAT_PROSPECT
            Case C_FLLWUP_REPUCHASE
                doneCategory = C_DONECAT_REPURCHASE
            Case C_FLLWUP_PERIODICAL
                doneCategory = C_DONECAT_PERIODICAL
            Case C_FLLWUP_PROMOTION
                doneCategory = C_DONECAT_PROMOTION
            Case C_FLLWUP_REQUEST
                doneCategory = C_DONECAT_REQUEST
            Case C_FLLWUP_WALKIN
                doneCategory = C_DONECAT_WALKIN
        End Select
        '--デバッグログ---------------------------------------------------
        Logger.Info("getFllwupDoneCategory End")
        '-----------------------------------------------------------------
        Return doneCategory
    End Function


    ''' <summary>
    ''' Follow-up Box種別取得
    ''' </summary>
    ''' <param name="cractresult"></param>
    ''' <param name="promotionid"></param>
    ''' <param name="cractcategory"></param>
    ''' <param name="reqcategory"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function getFllwupBoxType(ByVal cractresult As String, ByVal promotionid As Nullable(Of Long), ByVal cractcategory As String,
                                     ByVal reqcategory As String) As String
        '--デバッグログ---------------------------------------------------
        Logger.Info("getFllwupBoxType Start")
        '-----------------------------------------------------------------
        Dim fllwupBoxType As String = ""
        Select Case cractresult
            Case C_CRACTRESULT_HOT    'Hot
                fllwupBoxType = C_FLLWUP_HOT
            Case C_CRACTRESULT_PROSPECT    'Prospect
                fllwupBoxType = C_FLLWUP_PROSPECT
            Case C_CRACTRESULT_NOTACT, C_CRACTRESULT_CONTINUE
                If Not promotionid Is Nothing Then  'Promotion
                    fllwupBoxType = C_FLLWUP_PROMOTION
                Else
                    Select Case cractcategory
                        Case C_CRACTCATEGORY_REPURCHASE    'Repurchase
                            fllwupBoxType = C_FLLWUP_REPUCHASE
                        Case C_CRACTCATEGORY_PERIODICAL, C_CRACTCATEGORY_OTHERS, C_CRACTCATEGORY_BIRTHDAY 'Periodical
                            fllwupBoxType = C_FLLWUP_PERIODICAL
                        Case C_CRACTCATEGORY_DEFFULT
                            Select Case reqcategory
                                Case C_REQCATEGORY_WALKIN    'Walk-in
                                    fllwupBoxType = C_FLLWUP_WALKIN
                                Case C_REQCATEGORY_CALLIN, C_REQCATEGORY_RMM, C_REQCATEGORY_REQUEST    'Request
                                    fllwupBoxType = C_FLLWUP_REQUEST
                            End Select
                    End Select
                End If
        End Select
        '--デバッグログ---------------------------------------------------
        Logger.Info("getFllwupBoxType End")
        '-----------------------------------------------------------------
        Return fllwupBoxType
    End Function


    ''' <summary>
    ''' 競合車種取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetNoCompetitionMakermaster() As SC3080203DataSet.SC3080203CompetitionMakermasterDataTable
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
        Dim dlrcd As String = StaffContext.Current.DlrCD
        Return SC3080203TableAdapter.GetNoCompetitionMakermaster(dlrcd)
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 END
    End Function

    '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
    ''' <summary>
    ''' Follow-up Box商談取得 (開始時間、来店人数取得)
    ''' </summary>
    ''' <param name="dlrCd"></param>
    ''' <param name="strCd"></param>
    ''' <param name="fllwupboxSeqNo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetFllwupboxSales(ByVal dlrCD As String, ByVal strCD As String, ByVal fllwupboxSeqNo As Decimal) As SC3080203DataSet.SC3080203FllwupboxSalesDataTable
        Return SC3080203TableAdapter.GetFllwupboxSales(fllwupboxSeqNo)
    End Function
    '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

    '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 最大の活動終了時間を取得
    ''' </summary>
    ''' <param name="fllwupboxSeqNo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetLatestActTimeEnd(ByVal fllwupboxSeqNo As Decimal) As SC3080203DataSet.SC3080203LatestActTimeDataTable
        Return SC3080203TableAdapter.GetLatestActTimeEnd(fllwupboxSeqNo)
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 END
    End Function


    ''' <summary>
    ''' 活動分類名を取得
    ''' </summary>
    ''' <param name="contactNo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetNextActContactTitle(ByVal contactNo As Long) As SC3080203DataSet.SC3080203NextActContactDataTable
        Return SC3080203TableAdapter.GetActContactTitle(contactNo)
    End Function

    '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
    '2012/11/05 TCS 神本 【A.STEP2】次世代e-CRB  新車タブレット横展開に向けた機能開発 START
    ''' <summary>
    ''' 希望車種(見積車種＋選択車種)の取得
    ''' </summary>
    ''' <param name="fllwupboxseqno"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetPreferredCar(ByVal fllwStrcd As String, ByVal fllwupboxseqno As Decimal) As SC3080203DataSet.SC3080203PreferredCarDataTable
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 END
        Dim context As StaffContext = StaffContext.Current
        Dim estimateCar As SC3080203PreferredCarDataTable '見積車種リスト
        Dim selectedCar As SC3080203PreferredCarDataTable '選択車種リスト
        Dim modelName As SC3080203ModelNameDataTable
        Dim exteriorColor As SC3080203ExteriorColorDataTable
        Dim estimateBiz As New IC3070201BusinessLogic()
        Dim totalPrice As Double

        '見積車種を取得する
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
        estimateCar = SC3080203TableAdapter.GetEstimateCar(fllwupboxseqno)
        '選択車種を取得する
        selectedCar = SC3080203TableAdapter.GetSelectedCar(context.DlrCD, fllwStrcd, fllwupboxseqno)
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

        '支払い総額を取得する
        For Each row In estimateCar
            totalPrice = 0

            '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
            totalPrice = estimateBiz.GetTotalPrice(row.ESTIMATEID, 0)
            '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

            '表示用支払い総額設定
            row.DISPLAY_PRICE = totalPrice.ToString("0.00", Globalization.CultureInfo.InvariantCulture)
        Next

        '見積車種と選択車種をマージする
        estimateCar.Merge(selectedCar)


        For Each row As SC3080203PreferredCarRow In estimateCar



            '型式名設定
            modelName = SC3080203TableAdapter.GetModelName(row)
            If modelName.Count > 0 Then
                row.VCLMODEL_NAME = modelName(0).VCLMODEL_NAME
            End If

            '外装色名設定
            exteriorColor = SC3080203TableAdapter.GetExteriorColor(row)
            If exteriorColor.Count > 0 Then
                row.DISP_BDY_COLOR = exteriorColor(0).DISP_BDY_COLOR
            End If
        Next
        Dim dv As DataView = estimateCar.DefaultView
        dv.Sort = "IS_EXISTS_SELECTED_SERIES DESC,SEQNO ASC,ESTIMATEID ASC"

        '2013/01/10 TCS 上田 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START
        'Dim returnPreferredCar As New SC3080203DataSet.SC3080203PreferredCarDataTable()


        'For Each row As DataRow In dv.ToTable.Rows
        '    returnPreferredCar.ImportRow(row)
        'Next

        'Return returnPreferredCar

        Using returnPreferredCar As New SC3080203DataSet.SC3080203PreferredCarDataTable()
            For Each row As DataRow In dv.ToTable.Rows
                returnPreferredCar.ImportRow(row)
            Next

            Return returnPreferredCar
        End Using
        '2013/01/10 TCS 上田 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 ENDT
    End Function
    '2012/11/05 TCS 神本 【A.STEP2】次世代e-CRB  新車タブレット横展開に向けた機能開発 END

    '2013/01/10 TCS 上田 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START
    '2013/03/07 TCS 橋本 【A.STEP2】新車タブレット受付画面の管理指標変更対応 DEL START
    ' ''' <summary>
    ' ''' 査定依頼機能使用可否判定
    ' ''' </summary>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Shared Function IsRegAsm() As Boolean
    '    '--デバッグログ---------------------------------------------------
    '    Logger.Info("IsRegAsm Start")
    '    '-----------------------------------------------------------------
    '    Dim context As StaffContext = StaffContext.Current

    '    Return ActivityInfoBusinessLogic.IsRegAsm(context.DlrCD)
    '    '--デバッグログ---------------------------------------------------
    '    Logger.Info("IsRegAsm End")
    '    '-----------------------------------------------------------------
    'End Function

    ' ''' <summary>
    ' ''' 査定実績判定
    ' ''' </summary>
    ' ''' <param name="FllwStrcd"></param>
    ' ''' <param name="fllwupboxseqno"></param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Shared Function IsActAsm(ByVal fllwStrcd As String, ByVal fllwupboxseqno As Long) As Boolean
    '    '--デバッグログ---------------------------------------------------
    '    Logger.Info("IsActAsm Start")
    '    '-----------------------------------------------------------------
    '    Dim context As StaffContext = StaffContext.Current

    '    Return ActivityInfoBusinessLogic.IsActAsm(context.DlrCD, fllwStrcd, fllwupboxseqno, assessmentNo)

    '    '--デバッグログ---------------------------------------------------
    '    Logger.Info("IsActAsm End")
    '    '-----------------------------------------------------------------
    'End Function
    '2013/03/07 TCS 橋本 【A.STEP2】新車タブレット受付画面の管理指標変更対応 DEL END

    ''' <summary>
    ''' 未存在選択車種登録
    ''' </summary>
    ''' <param name="registdt"></param>
    ''' <remarks></remarks>
    Private Sub InsertNotRegSelectedSeries(ByRef registdt As SC3080203RegistDataDataTable)
        '--デバッグログ---------------------------------------------------
        Logger.Info("InsertNotRegSelectedSeries Start")
        '-----------------------------------------------------------------

        Dim RegistRw As SC3080203DataSet.SC3080203RegistDataRow
        RegistRw = CType(registdt.Rows(0), SC3080203RegistDataRow)

        '配列作成
        Dim wkary As String()
        Dim tempary As String()
        Dim estimateId As Long
        Dim seqno As Long
        Dim carList As String
        Dim context As StaffContext = StaffContext.Current
        Dim account As String = context.Account     '自身のアカウント
        Dim cntCd As String = EnvironmentSetting.CountryCode

        '見積作成実績
        carList = ""
        wkary = RegistRw.SELECTACTVALUATION.Split(";"c)
        For i = 0 To wkary.Length - 2
            tempary = wkary(i).Split(","c)
            '見積実績で選択された車種のうち、選択車種レコードがが存在しない場合
            If String.Equals(tempary(1), "1") And String.Equals(tempary(2), "0") Then
                '見積管理ID
                estimateId = CType(tempary(4), Long)

                '選択車種の登録
                '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
                seqno = ActivityInfoBusinessLogic.InsertNotRegSelectedSeries(estimateId, account, RegistRw.FLLWSEQ)
                '2013/06/30 TCS 庄 2013/10対応版　既存流用 END
                tempary(0) = seqno.ToString(CultureInfo.CurrentCulture())
            End If
            carList = carList & tempary(0) & "," & tempary(1) & "," & tempary(5) & ";"
        Next

        '結果を反映
        RegistRw.SELECTACTVALUATION = carList


        '成約車種
        carList = ""
        wkary = RegistRw.SUCCESSSERIES.Split(";"c)
        For i = 0 To wkary.Length - 2
            tempary = wkary(i).Split(","c)
            '成約車種で選択された車種のうち、選択車種レコードがが存在しない場合
            If String.Equals(tempary(1), "1") And String.Equals(tempary(2), "0") Then
                '見積管理ID
                estimateId = CType(tempary(4), Long)

                '選択車種の登録
                '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
                seqno = ActivityInfoBusinessLogic.InsertNotRegSelectedSeries(estimateId, account, RegistRw.FLLWSEQ)
                '2013/06/30 TCS 庄 2013/10対応版　既存流用 END
                tempary(5) = seqno.ToString(CultureInfo.CurrentCulture())
            End If
            '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
            carList = carList & tempary(5) & "," & tempary(1) & "," & tempary(2) & "," & tempary(3) & "," & tempary(4) & ";"
            '2013/06/30 TCS 庄 2013/10対応版　既存流用 END
        Next

        '結果を反映
        RegistRw.SUCCESSSERIES = carList

        '--デバッグログ---------------------------------------------------
        Logger.Info("InsertNotRegSelectedSeries End")
        '-----------------------------------------------------------------
    End Sub
    '2013/01/10 TCS 上田 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END

    '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
    '2013/03/07 TCS 橋本 【A.STEP2】新車タブレット受付画面の管理指標変更対応 ADD START
    ''' <summary>
    ''' 査定実績登録区分判定
    ''' </summary>
    ''' <param name="selectActAsmBottom">査定実績ボタンの押下状態</param>
    ''' <param name="dlrCd">販売店コード</param>
    ''' <param name="strCd">店舗コード</param>
    ''' <param name="fllwUpBoxSeqNo">Follow-up Box内連番</param>
    ''' <param name="assessmentNo">依頼No</param>
    ''' <returns>0:活動履歴登録無し/1:活動履歴登録有り/2:活動履歴退避登録</returns>
    ''' <remarks>査定依頼機能使用可否判定と査定実績判定から、査定の活動履歴を登録する区分を設定します。</remarks>
    Private Shared Function GetRegActAsmStatus(ByVal selectActAsmBottom As String, ByVal dlrCd As String, ByVal strCd As String, ByVal fllwUpBoxSeqNo As Decimal,
                                               ByRef assessmentNo As Long
                                               ) As String
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 END
        Dim retCd As String = C_ASMACTSTATUS_NASI

        '査定依頼機能使用可否判定
        If ActivityInfoBusinessLogic.IsRegAsm(dlrCd) Then
            '査定実績判定
            retCd = ActivityInfoBusinessLogic.IsActAsm(dlrCd, strCd, fllwUpBoxSeqNo, assessmentNo, True)
        Else
            '査定実績ボタンの押下状態を反映
            retCd = selectActAsmBottom
        End If

        Return retCd
    End Function
    '2013/03/07 TCS 橋本 【A.STEP2】新車タブレット受付画面の管理指標変更対応 ADD END

#Region "Aカード情報相互連携開発"
    '2013/12/03 TCS 市川 Aカード情報相互連携開発 START
    ''' <summary>
    ''' 断念理由マスタ取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>断念理由マスタを取得する。</remarks>
    Public Shared Function GetGiveUpReasonMaster() As SC3080203DataSet.SC3080203GiveupReasonMasterDataTable

        Dim dlrCd As String = String.Empty
        Dim brnCd As String = String.Empty
        Dim otherActRsltId As Decimal = 0

        'セッションからログイン販売店コード・店舗コードを取得する。
        dlrCd = StaffContext.Current.DlrCD
        brnCd = StaffContext.Current.BrnCD

        '環境設定からコンタクト方法・その他活動結果(手入力活動結果)を取得する。
        Decimal.TryParse(GetBrnEnvSettingValue(dlrCd, brnCd, ENVSETTINGKEY_GIVEUP_REASON_OTHER), otherActRsltId)

        Return SC3080203TableAdapter.GetGiveupReasonMaster(dlrCd, brnCd, otherActRsltId)
    End Function

    ''' <summary>
    ''' 手動成約許可設定取得
    ''' </summary>
    ''' <returns>True：手動成約可／False：手動成約不可（注文承認による自動成約のみ可）</returns>
    ''' <remarks>手動成約可否（販売店毎）設定を取得する。</remarks>
    Public Shared Function GetSetting_AllowUISuccess() As Boolean
        Dim envValue As String = GetDlrEnvSettingValue(StaffContext.Current.DlrCD, ENVSETTINGKEY_MANUAL_SUCCESSABLE)
        Return String.Equals(envValue, ENVSETTINGVALUE_MANUAL_SUCCESSABLE)
    End Function

    ''' <summary>
    ''' 販売店別設定値を取得
    ''' </summary>
    ''' <param name="dlrCd"></param>
    ''' <param name="dlrEnvName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetDlrEnvSettingValue(ByVal dlrCd As String, ByVal dlrEnvName As String) As String
        Dim dr As DlrEnvSettingDataSet.DLRENVSETTINGRow = Nothing
        Dim env As DealerEnvSetting = Nothing
        Try
            env = New DealerEnvSetting()
            dr = env.GetEnvSetting(dlrCd, dlrEnvName)
            If Not dr Is Nothing Then
                Return dr.PARAMVALUE.Trim()
            End If
        Catch ex As Exception
            Logger.Error("GetEnvSettingErr", ex)
        Finally
            env = Nothing
        End Try

        Return String.Empty
    End Function
    ''' <summary>
    ''' 店舗別設定値を取得
    ''' </summary>
    ''' <param name="dlrCd"></param>
    ''' <param name="brnCd"></param>
    ''' <param name="dlrEnvName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetBrnEnvSettingValue(ByVal dlrCd As String, ByVal brnCd As String, ByVal dlrEnvName As String) As String
        Dim dr As DlrEnvSettingDataSet.DLRENVSETTINGRow = Nothing
        Dim env As BranchEnvSetting = Nothing
        Try
            env = New BranchEnvSetting()
            dr = env.GetEnvSetting(dlrCd, brnCd, dlrEnvName)
            If Not dr Is Nothing Then
                Return dr.PARAMVALUE.Trim()
            End If
        Catch ex As Exception
            Logger.Error("GetEnvSettingErr", ex)
        Finally
            env = Nothing
        End Try

        Return String.Empty
    End Function

    '2013/12/03 TCS 市川 Aカード情報相互連携開発 END
#End Region

    '2014/05/15 TCS 武田 受注後フォロー機能開発 START
    ''' <summary>
    ''' 初回商談活動ID取得
    ''' </summary>
    ''' <param name="attId">誘致ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetFirstSalesActId(ByVal attId As Decimal) As Decimal
        '初回商談活動IDを取得
        Dim fstSalesActId As Decimal = 0
        Dim fstSalesActIdDt As SC3080203DataSet.SC3080203FstSalesActIdMasterDataTable = SC3080203TableAdapter.GetFstSalesActId(attId)
        Dim fstSalesActIdRw As SC3080203DataSet.SC3080203FstSalesActIdMasterRow = CType(fstSalesActIdDt.Rows(0), SC3080203FstSalesActIdMasterRow)
        If Not IsDBNull(fstSalesActIdRw.Item("ACT_ID")) Then
            fstSalesActId = fstSalesActIdRw.ACT_ID
        End If

        Return fstSalesActId
    End Function
    '2014/05/15 TCS 武田 受注後フォロー機能開発 END

    '2015/07/06 TCS 藤井  TR-V4-FTMS141028001(FTMS→TMTマージ) START
    ''' <summary>
    ''' 顧客メモ新規登録処理
    ''' </summary>
    ''' <param name="memoDataTbl">データセット (インプット)</param>
    ''' <param name="msgId">メッセージID</param>
    ''' <returns>処理結果</returns>
    ''' <remarks>顧客情報を新規登録する。</remarks>
    Public Function InsertCustomerMemo(ByVal cst_id As Decimal, ByVal giveup_reason As String) As Integer

        Dim ret As Integer = 1

        '顧客メモ連番采番
        Dim seqno As Long

        seqno = SC3080203TableAdapter.GetCustmemoseq(StaffContext.Current.DlrCD, cst_id)

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetCustmemoseq_Start")
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetCustmemoseq seqno = " + CType(seqno, String))
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetCustmemoseq memoDataRow.MEMO = " + giveup_reason)
        'ログ出力 End *****************************************************************************

        '顧客メモ追加
        ret = SC3080203TableAdapter.InsertCustomerMemo(StaffContext.Current.DlrCD,
                                                       cst_id,
                                                       seqno,
                                                       giveup_reason,
                                                       StaffContext.Current.Account)

        'ログ出力 Start ***************************************************************************  
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetCustmemoseq_End")
        'ログ出力 End *****************************************************************************
        Return ret

    End Function
    '2015/07/06 TCS 藤井  TR-V4-FTMS141028001(FTMS→TMTマージ) END

#End Region

End Class
