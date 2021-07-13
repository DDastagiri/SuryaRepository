'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3080203.aspx.vb
'─────────────────────────────────────
'機能： 顧客詳細(活動登録)
'補足： 
'作成：            TCS 河原 【SALES_1A】
'更新： 2012/08/09 TCS 河原 【A STEP2】次世代e-CRB セールスKPI出力開発
'更新： 2012/08/09 TCS 河原 Follow-upBox結果の次回活動日の修正
'更新： 2012/08/09 TCS 安田 【SALES_3】共通.来店実績更新パラメーター追加
'更新： 2012/09/06 TCS 山口 【A STEP2】次世代e-CRB 新車受付機能改善
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


    Private Const CRACTRESULT_HOT As String = "1"
    Private Const CRACTRESULT_PROSPECT As String = "2"
    Private Const CRACTRESULT_WALKIN As String = "0"


    Private Const CRACTSTATUS_HOT As String = "1"
    Private Const CRACTSTATUS_PROSPECT As String = "2"
    Private Const CRACTSTATUS_WALKIN As String = "7"


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

    ''' <summary>
    ''' メッセージＩＤ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MSG_30932 As Integer = 30932
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


    ''' <summary>
    ''' シリーズ単位の希望車種取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetFllwSeries(ByVal FllwStrcd As String, ByVal fllwupboxseqno As Long) As SC3080203DataSet.SC3080203FllwSeriesDataTable
        Dim context As StaffContext = StaffContext.Current
        Return (SC3080203TableAdapter.GetFllwSeries(context.DlrCD, FllwStrcd, EnvironmentSetting.CountryCode, fllwupboxseqno))
    End Function


    ''' <summary>
    ''' グレード単位の希望車種の取得
    ''' </summary>
    ''' <param name="fllwupboxseqno"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetFllwModel(ByVal FllwStrcd As String, ByVal fllwupboxseqno As Long) As SC3080203DataSet.SC3080203FllwModelDataTable
        Dim context As StaffContext = StaffContext.Current
        Return SC3080203TableAdapter.GetFllwModel(context.DlrCD, FllwStrcd, EnvironmentSetting.CountryCode, fllwupboxseqno)
    End Function


    ''' <summary>
    ''' カラー単位の希望車種の取得
    ''' </summary>
    ''' <param name="fllwupboxseqno"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetFllwColor(ByVal FllwStrcd As String, ByVal fllwupboxseqno As Long) As SC3080203DataSet.SC3080203FllwColorDataTable
        Dim context As StaffContext = StaffContext.Current
        Return SC3080203TableAdapter.GetFllwColor(context.DlrCD, FllwStrcd, EnvironmentSetting.CountryCode, fllwupboxseqno)
    End Function

    ''' <summary>
    ''' 活動方法取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetActContact() As SC3080203DataSet.SC3080203ActContactDataTable
        '2012/03/02 Version1.01 Yasuda 【A.STEP2】代理商談入力機能開発 Start
        'Return SC3080203TableAdapter.GetActContact() 
        Return SC3080203TableAdapter.GetActContact(SalesFlg)
        '2012/03/02 Version1.01 Yasuda 【A.STEP2】代理商談入力機能開発 End
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
        Return SC3080203TableAdapter.GetFollowCractstatus(Serchrw.DLRCD, Serchrw.STRCD, Serchrw.FLLWUPBOX_SEQNO)
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
        Return SC3080203TableAdapter.GetCompetitorMaster()
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
        Logger.Debug("GetAlertNonSel Start")
        '-----------------------------------------------------------------
        Dim dt As SC3080203DataSet.SC3080203AlarmMasterDataTable = SC3080203TableAdapter.GetAlarmMaster("2")
        SetAlertTitle(dt)
        AddNoneAlertItem(dt)
        '--デバッグログ---------------------------------------------------
        Logger.Debug("GetAlertNonSel End")
        '-----------------------------------------------------------------
        Return dt
    End Function

    Private Shared Sub AddNoneAlertItem(ByVal dt As SC3080203DataSet.SC3080203AlarmMasterDataTable)
        '--デバッグログ---------------------------------------------------
        Logger.Debug("AddNoneAlertItem Start")
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
        Logger.Debug("AddNoneAlertItem End")
        '-----------------------------------------------------------------
    End Sub

    ''' <summary>
    ''' アラートの
    ''' </summary>
    ''' <param name="dt"></param>
    ''' <remarks></remarks>
    Private Shared Sub SetAlertTitle(ByVal dt As SC3080203DataSet.SC3080203AlarmMasterDataTable)
        '--デバッグログ---------------------------------------------------
        Logger.Debug("SetAlertTitle Start")
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
        Logger.Debug("SetAlertTitle End")
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


    ''' <summary>
    ''' 新規登録処理
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function InsertActivityData(ByVal registdt As SC3080203RegistDataDataTable) As Boolean
        '--デバッグログ---------------------------------------------------
        Logger.Debug("InsertActivityData Start")
        '-----------------------------------------------------------------

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
        Dim fllwupbox_seqno As Long = RegistRw.FLLWSEQ   'Follow-upBox内連番

        Dim crcustid As String = Nothing        '未取引顧客ID、自社客ID、副顧客IDのいずれかを設定
        Dim carid As String = Nothing           'Vin or 未取車両seq

        Dim totalhisseq As Long = Nothing
        Dim service_nm As String = ""

        Dim customerclass As String = "1"       '1:所有者、2:使用者、3:その他

        Dim catalog As String = ""
        Dim testdrive As String = ""
        Dim assessment As String = ""
        Dim valuation As String = ""

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
        seqdt = SC3080203TableAdapter.GetActHisCarSeq(dlrcd, fllwstrcd, fllwupbox_seqno)
        For j As Integer = 0 To seqdt.Count - 1
            seqrw = CType(seqdt.Rows(j), SC3080203SeqRow)
            selcar = selcar & seqrw.SEQNO & ","
        Next

        'カタログ実績がある希望車種のSEQのリストを作成
        catalog = ""
        wkary = RegistRw.SELECTACTCATALOG.Split(";"c)
        For i = 0 To wkary.Length - 2
            tempary = wkary(i).Split(","c)
            If String.Equals(tempary(1), "1") Then
                seqdt = SC3080203TableAdapter.GetActHisSelCarSeq(dlrcd, fllwstrcd, fllwupbox_seqno, tempary(0), "1")
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
                seqdt = SC3080203TableAdapter.GetActHisSelCarSeq(dlrcd, fllwstrcd, fllwupbox_seqno, tempary(0), "2")
                For j = 0 To seqdt.Count - 1
                    seqrw = CType(seqdt.Rows(j), SC3080203SeqRow)
                    testdrive = testdrive & seqrw.SEQNO & ","
                Next
            End If
        Next

        '見積り実績(見積りは全部に対して有り、無しの2択)
        assessment = RegistRw.SELECTACTASSESMENT

        '査定実績がある希望車種のSEQのリストを作成
        valuation = ""
        wkary = RegistRw.SELECTACTVALUATION.Split(";"c)
        For i = 0 To wkary.Length - 2
            tempary = wkary(i).Split(","c)
            If String.Equals(tempary(1), "1") Then
                seqdt = SC3080203TableAdapter.GetActHisSelCarSeq(dlrcd, fllwstrcd, fllwupbox_seqno, tempary(0), "4")
                For j = 0 To seqdt.Count - 1
                    seqrw = CType(seqdt.Rows(j), SC3080203SeqRow)
                    valuation = valuation & seqrw.SEQNO & ","
                Next
            End If
        Next

        'シリーズコード、シリーズ名取得
        Dim Srsdt As SC3080203SeriesDataTable
        Dim Srsrw As SC3080203SeriesRow
        If String.Equals(cstkind, "1") Then
            '自社の場合
            originalid = crcustid

            vin = carid
            Srsdt = SC3080203TableAdapter.GetVclinfo(originalid, vin)
            If Srsdt.Count > 0 Then
                Srsrw = CType(Srsdt.Rows(0), SC3080203SeriesRow)
                seriescd = Srsrw.SERIESCD
                seriesname = Srsrw.SERIESNM
            Else
                seriescd = " "
                seriesname = " "
            End If
        Else
            '未取の場合
            cstid = crcustid

            If String.IsNullOrEmpty(carid) = False Then
                newcustcarseq = CInt(carid)
            Else
                newcustcarseq = Nothing
            End If

            If newcustcarseq IsNot Nothing Then
                Srsdt = SC3080203TableAdapter.GetNewcustVclinfo(cstid, newcustcarseq.Value)
                If Srsdt.Count > 0 Then
                    Srsrw = CType(Srsdt.Rows(0), SC3080203SeriesRow)
                    seriescd = Srsrw.SERIESCD
                    seriesname = Srsrw.SERIESNM
                Else
                    seriescd = " "
                    seriesname = " "
                End If
            Else
                seriescd = " "
                seriesname = " "
            End If
        End If

        If String.Equals(cstkind, "1") Then
            Dim newcustiddt As SC3080203DataSet.SC3080203NewCustIDDataTable
            Dim newcustidrw As SC3080203DataSet.SC3080203NewCustIDRow
            newcustiddt = SC3080203TableAdapter.GetNewCustID(crcustid)

            If newcustiddt.Count > 0 Then
                newcustidrw = CType(newcustiddt.Rows(0), SC3080203NewCustIDRow)
                cstid = newcustidrw.CSTID

                Dim newvcliddt As SC3080203DataSet.SC3080203NewVclIDDataTable
                Dim newvclidrw As SC3080203DataSet.SC3080203NewVclIDRow
                newvcliddt = SC3080203TableAdapter.GetNewVclID(cstid, vin)

                If newvcliddt.Count > 0 Then
                    newvclidrw = CType(newvcliddt.Rows(0), SC3080203NewVclIDRow)
                    newcustcarseq = newvclidrw.SEQNO
                Else
                    newcustcarseq = Nothing
                End If
            Else
                cstid = ""
            End If
        End If

        Dim sequencedt As SC3080203DataSet.SC3080203SequenceDataTable
        Dim sequencerw As SC3080203DataSet.SC3080203SequenceRow
        '自社客で未取引客情報未登録の場合
        If String.Equals(cstkind, "1") Then
            If String.IsNullOrEmpty(cstid) Then
                sequencedt = SC3080203TableAdapter.GetSeqNewcustomerCstId()
                sequencerw = CType(sequencedt.Rows(0), SC3080203SequenceRow)
                cstkindwk = sequencerw.Seq
                '10桁にゼロ埋めして未取引客IDを作成
                cstid = "NCST" & CStr(cstkindwk).PadLeft(10, "0"c)
                '034.未取引客個人情報追加
                SC3080203TableAdapter.InsertNweCustomer(cstid, originalid, vin)
            End If
            If newcustcarseq Is Nothing Then
                '056.メーカー名取得
                Dim makerdt As SC3080201MakernameDataTable
                Dim makerrw As SC3080201MakernameRow
                makerdt = SC3080203TableAdapter.GetMakername(dlrcd, cntcd, seriescd)

                If makerdt.Count > 0 Then
                    makerrw = CType(makerdt.Rows(0), SC3080201MakernameRow)
                    makername = makerrw.MAKERNAME
                Else
                    makername = " "
                End If
                '054.未取引客車両情報追加SeqNo取得
                sequencedt = SC3080203TableAdapter.GetSeqNewcustomerVclreSeqno()
                sequencerw = CType(sequencedt.Rows(0), SC3080203SequenceRow)
                newcustcarseq = sequencerw.Seq
                '035.未取引客車両情報追加
                SC3080203TableAdapter.InsertNweCustomerVclre(cstid, newcustcarseq.Value, makername, originalid, vin)
            End If
        End If

        '051.Walk-in Person SeqNo取得
        sequencedt = SC3080203TableAdapter.GetSeqWalkInPersonWalkInId()
        sequencerw = CType(sequencedt.Rows(0), SC3080203SequenceRow)
        walkinidwk = sequencerw.Seq

        walkinid = "WID" & CStr(walkinidwk).PadLeft(10, "0"c)

        'WICID取得
        Dim sysEnv As New SystemEnvSetting
        Dim sysEnvRow As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow
        sysEnvRow = sysEnv.GetSystemEnvSetting(CONTENT_WALKIN_WICID)
        wicid = sysEnvRow.PARAMVALUE

        '来店人数
        Dim walkinNum As Nullable(Of Integer) = Nothing '来店人数
        If Not RegistRw.IsWALKINNUMNull Then
            walkinNum = RegistRw.WALKINNUM
        End If

        '044.ウォークイン要件情報追加
        SC3080203TableAdapter.InsertWalkInPerson(walkinid, cstid, newcustcarseq, dlrcd, strcd, actDayToDate, seriescd, seriesname,
                                   registrationtype, actDayToDate, Integer.Parse(wicid, CultureInfo.CurrentCulture()), fllwupbox_seqno, crcustid,
                                   originalid, account, context.UserName, Long.Parse(RegistRw.ACTCONTACT, CultureInfo.CurrentCulture()), RegistRw.ACTDAYFROM, actdayto, actaccount, walkinNum)


        '045.ウォークイン要件メモ追加
        SC3080203TableAdapter.InsertWalkInPersonMemo(walkinid, dlrcd)

        '057.TotalHisSqeNo取得
        sequencedt = SC3080203TableAdapter.GetSeqTotalhisSeqno()
        sequencerw = CType(sequencedt.Rows(0), SC3080203SequenceRow)
        totalhisseq = sequencerw.Seq

        'Completed文言取得
        Dim thisstatus As String
        thisstatus = WebWordUtility.GetWord(30356)

        '042.Total履歴追加(Walk-in)
        SC3080203TableAdapter.InsertTotalHis(dlrcd, strcd, crcustid, totalhisseq, actDayToDate, "7", "0", vin, seriesname,
                               thisstatus, service_nm, actaccount, crcustid)

        Dim cractresult As String = Nothing         '1: Hot  2: Prospect(Warm)  0: Walk-in(Cold)
        Dim cractstatus As String = Nothing         '1: Hot  2: Prospect(Warm)  7: Walk-in(Cold)

        If String.Equals(RegistRw.ACTRESULT, C_RSLT_HOT) Then
            cractresult = CRACTRESULT_HOT
            cractstatus = CRACTSTATUS_HOT
        ElseIf String.Equals(RegistRw.ACTRESULT, C_RSLT_PROSPECT) Then
            cractresult = CRACTRESULT_PROSPECT
            cractstatus = CRACTSTATUS_PROSPECT
        Else
            cractresult = CRACTRESULT_WALKIN
            cractstatus = CRACTSTATUS_WALKIN
        End If

        '005.関連情報取得
        Dim relaifdt As SC3080203DataSet.SC3080203SequenceDataTable
        Dim relaifrw As SC3080203DataSet.SC3080203SequenceRow
        relaifdt = SC3080203TableAdapter.GetRelatedInfo(vin)
        relaifrw = CType(relaifdt.Rows(0), SC3080203SequenceRow)
        Dim relatedinfoflg As String = Nothing
        If relaifrw.Seq > 0 Then
            relatedinfoflg = "1"
        Else
            relatedinfoflg = "0"
        End If

        Dim nextcractivedate As Nullable(Of Date) = Nothing
        If String.Equals(cstkind, "1") Then
            '自社客走行距離履歴よりサービススタッフ情報を取得
            Dim srvstaffdt As SC3080203DataSet.SC3080203ServiceStaffDataTable
            Dim srvstaffrw As SC3080203DataSet.SC3080203ServiceStaffRow
            srvstaffdt = SC3080203TableAdapter.GetServiceStaff(originalid, vin)
            Dim servicestaffcd As String
            Dim servicestaffnm As String

            If srvstaffdt.Count > 0 Then
                srvstaffrw = CType(srvstaffdt.Rows(0), SC3080203ServiceStaffRow)
                servicestaffcd = srvstaffrw.SERVICESTAFFCD
                servicestaffnm = srvstaffrw.SERVICESTAFFNM
            Else
                servicestaffcd = " "
                servicestaffnm = " "
            End If

            '007.Follow-up Box追加(自社客)
            '次回活動担当は自分固定
            SC3080203TableAdapter.InsertFllwupbox(dlrcd, strcd, fllwupbox_seqno, cractivedate, appointtimeflg, cractivedate, cstid, newcustcarseq.ToString, cractresult, strcd,
                                    account, originalid, vin, relatedinfoflg, nextcractivedate, account, cractresult, prospect_date, hot_date, Long.Parse(wicid, CultureInfo.CurrentCulture()),
                                    cractstatus, cractstatus, strcd, account, cractivedate, cractivedate, crcustid, account, originalid, servicestaffcd, servicestaffnm)
        Else
            '007.Follow-up Box追加(未取引客)
            SC3080203TableAdapter.InsertNewCustFllwupbox(dlrcd, strcd, fllwupbox_seqno, cractivedate, appointtimeflg, cractivedate, cstid, newcustcarseq, cractresult, strcd,
                                           account, relatedinfoflg, nextcractivedate, account, cractresult, prospect_date, hot_date, Long.Parse(wicid, CultureInfo.CurrentCulture()),
                                           cractstatus, cractstatus, strcd, account, cractivedate, cractivedate, crcustid, account, cstid)
        End If

        '058.来店区分取得
        Dim wicdt As SC3080203DataSet.SC3080203WinclassDataTable
        Dim wicrw As SC3080203DataSet.SC3080203WinclassRow
        wicdt = SC3080203TableAdapter.GetWinclass(wicid)
        wicrw = CType(wicdt.Rows(0), SC3080203WinclassRow)

        '018.Follow-up Box活動履歴追加(Walk-in受付用)
        Dim action As String
        Dim actioncd As String
        action = wicrw.WICNAME
        actioncd = wicrw.ACTIONCD

        Dim calldate As Nullable(Of Date) = Nothing
        Dim callaccount As String = Nothing
        Dim crdvs As Nullable(Of Long) = Nothing
        Dim actualtime_end As Nullable(Of Date) = Nothing
        Dim method As String = Nothing
        Dim actiontype As String = Nothing
        Dim brnchaccount As String = Nothing
        Dim ctntseqno As Nullable(Of Long) = Nothing

        Dim select_series_seqno As Nullable(Of Long) = Nothing
        Dim seriesnm As String = Nothing
        Dim vclmodel_name As String = Nothing
        Dim disp_bdy_color As String = Nothing
        Dim quantity As Nullable(Of Integer) = Nothing

        Dim fllwupboxrslt_seqno As Integer = Nothing

        callaccount = " "
        method = " "
        actiontype = "0"
        brnchaccount = actaccount
        fllwupboxrslt_seqno = 0

        SC3080203TableAdapter.InsertFllwupboxCRHis(calldate, callaccount, crdvs, actualtime_end, actDayToDate, method, action, actiontype, brnchaccount, actioncd, ctntseqno,
                                     select_series_seqno, seriesnm, vclmodel_name, disp_bdy_color, quantity, fllwupboxrslt_seqno, dlrcd, strcd, fllwupbox_seqno)

        '活動結果がProspectの場合
        If String.Equals(actresult, "2") Then
            actioncd = CONTENT_HOT_ACTIONCD
            action = "Prospect"
            SC3080203TableAdapter.InsertFllwupboxCRHis(calldate, callaccount, crdvs, actualtime_end, actDayToDate, method, action, actiontype, brnchaccount, actioncd, ctntseqno,
                                         select_series_seqno, seriesnm, vclmodel_name, disp_bdy_color, quantity, fllwupboxrslt_seqno, dlrcd, strcd, fllwupbox_seqno)
        End If

        '活動結果がProspectの場合
        If String.Equals(actresult, "3") Then
            actioncd = CONTENT_PROSPECT_ACTIONCD
            action = "Hot"
            SC3080203TableAdapter.InsertFllwupboxCRHis(calldate, callaccount, crdvs, actualtime_end, actDayToDate, method, action, actiontype, brnchaccount, actioncd, ctntseqno,
                                        select_series_seqno, seriesnm, vclmodel_name, disp_bdy_color, quantity, fllwupboxrslt_seqno, dlrcd, strcd, fllwupbox_seqno)
        End If

        '希望車種に対する活動実績を入力する
        If String.Equals(RegistRw.PROCESSFLG, "1") Then
            InsertHistory(dlrcd, fllwstrcd, fllwupbox_seqno, selcar, catalog, testdrive, assessment, valuation, account, actdate.ToString("yyyy/MM/dd HH:mm", CultureInfo.CurrentCulture()))
        End If

        '活動結果がHot、Prospectの場合、それに対応したTotal履歴を作成
        If String.Equals(actresult, C_RSLT_PROSPECT) Or String.Equals(actresult, C_RSLT_HOT) Then
            Dim totalHisStatus As String
            If String.Equals(actresult, C_RSLT_PROSPECT) Then
                totalHisStatus = WebWordUtility.GetWord(30384)
            Else
                totalHisStatus = WebWordUtility.GetWord(30385)
            End If

            '057.TotalHisSqeNo取得
            sequencedt = SC3080203TableAdapter.GetSeqTotalhisSeqno()
            sequencerw = CType(sequencedt.Rows(0), SC3080203SequenceRow)
            totalhisseq = sequencerw.Seq

            'TotalHisの登録(Hot/Prospectの履歴)
            SC3080203TableAdapter.InsertTotalHis(dlrcd, strcd, crcustid, totalhisseq, actDayToDate, "7", "", vin, seriesname, totalHisStatus, service_nm, actaccount, crcustid)
        End If

        '057.TotalHisSqeNo取得
        sequencedt = SC3080203TableAdapter.GetSeqTotalhisSeqno()
        sequencerw = CType(sequencedt.Rows(0), SC3080203SequenceRow)
        totalhisseq = sequencerw.Seq

        '042.Total履歴追加(Follow-upBox)
        SC3080203TableAdapter.InsertTotalHis(dlrcd, strcd, crcustid, totalhisseq, actDayToDate, "4", "", vin, seriesname, "", service_nm, actaccount, crcustid)

        '023.Follow-up Box未取引客情報追加
        SC3080203TableAdapter.InsertFllwupboxNewcst(dlrcd, strcd, fllwupbox_seqno, account, cstid, newcustcarseq)

        '自社客の場合Follow-up Box詳細を作成
        If String.Equals(cstkind, "1") Then
            Dim lastserviceindate As String = Nothing
            Dim last_subctgcode As String = Nothing
            Dim last_servicecd As String = Nothing
            Dim last_servicename As String = Nothing
            Dim lastmileage As String = Nothing
            Dim lastserviceinbranch As String = Nothing

            'VIN取得
            Dim vindt As SC3080203VinDataTable
            Dim vinrw As SC3080203VinRow
            vindt = SC3080203TableAdapter.GetVin(originalid)

            For i = 0 To vindt.Count - 1
                vinrw = CType(vindt.Rows(i), SC3080203VinRow)

                last_subctgcode = " "
                last_servicecd = " "
                last_servicename = " "

                '048.自社客走行距離履歴取得
                Dim milehisdt As SC3080201MileageHisDataTable
                Dim milehisrw As SC3080201MileageHisRow
                milehisdt = SC3080203TableAdapter.GetMileageHis(originalid, vinrw.VIN)
                If milehisdt.Count > 0 Then
                    '取得できた場合変数に値をセット
                    milehisrw = CType(milehisdt.Rows(0), SC3080201MileageHisRow)

                    If milehisrw.IsREGISTDATENull() Then
                        lastserviceindate = Nothing
                    Else
                        lastserviceindate = milehisrw.REGISTDATE
                    End If

                    lastmileage = milehisrw.MILEAGE.ToString(CultureInfo.CurrentCulture())
                    lastserviceinbranch = milehisrw.STRCD

                    '049.自社客点検履歴取得
                    Dim srvhisdt As SC3080201ServiceHisDataTable
                    Dim srvhisrw As SC3080201ServiceHisRow
                    srvhisdt = SC3080203TableAdapter.GetServiceHis(milehisrw.DLRCD, milehisrw.JOBNO)
                    If srvhisdt.Count > 0 Then
                        srvhisrw = CType(srvhisdt.Rows(0), SC3080201ServiceHisRow)

                        '040.サービスマスタ取得
                        Dim srvmstdt As SC3080203ServiceMasterDataTable
                        Dim srvmstrw As SC3080203ServiceMasterRow
                        srvmstdt = SC3080203TableAdapter.GetServiceMaster(srvhisrw.SERVICECD, dlrcd)
                        If srvmstdt.Count > 0 Then
                            srvmstrw = CType(srvmstdt.Rows(0), SC3080203ServiceMasterRow)
                            last_servicecd = srvmstrw.SERVICECD
                            last_servicename = srvmstrw.SERVICENAME

                            '041.中項目マスタ取得
                            Dim subctgdt As SC3080203SubCategoryDataTable
                            Dim subctgrw As SC3080203SubCategoryRow
                            subctgdt = SC3080203TableAdapter.GetSubCategory(srvmstrw.SERVICECD)
                            If subctgdt.Count > 0 Then
                                subctgrw = CType(subctgdt.Rows(0), SC3080203SubCategoryRow)
                                last_subctgcode = subctgrw.SUBCTGCODE
                            End If
                        End If
                    End If
                End If

                '019.Follow-up Box詳細追加
                SC3080203TableAdapter.InserFllwupboxDetail(dlrcd, strcd, fllwupbox_seqno, lastserviceindate, last_subctgcode, last_servicecd, last_servicename,
                                             lastmileage, lastserviceinbranch, originalid, vinrw.VIN)

                If milehisdt.Count > 0 Then
                    '092.Follow-up Box走行距離履歴の追加
                    milehisrw = CType(milehisdt.Rows(0), SC3080201MileageHisRow)
                    SC3080203TableAdapter.insertFllwupboxMilehis(dlrcd, strcd, fllwupbox_seqno, originalid, vinrw.VIN, milehisrw.MILEAGESEQ, milehisrw.REGISTDATE, milehisrw.MILEAGE, "1", milehisrw.JOBNO)
                End If

            Next

        End If

        '037.その他計画(Follow-up)追加
        SC3080203TableAdapter.InsertOtherPlanFllw(CONTENT_MODULEID, account, CONTENT_MODULEID, account, dlrcd, strcd, fllwupbox_seqno)

        '031.Follow-up Box商談メモ追加
        SC3080203TableAdapter.InsertFllwupboxSalesmemo(dlrcd, strcd, fllwupbox_seqno, cstkind, customerclass, crcustid, account, CONTENT_MODULEID)

        '055.Follow-up Box商談メモWK削除
        SC3080203TableAdapter.DeleteFllwupboxSalesmemowk(dlrcd, strcd, fllwupbox_seqno)

        '一発Success、Give-up時に2回プロセス実績が登録されないように空にする
        RegistRw.SELECTACTCATALOG = ""
        RegistRw.SELECTACTTESTDRIVE = ""
        RegistRw.SELECTACTVALUATION = ""
        RegistRw.SELECTACTASSESMENT = ""

        '--デバッグログ---------------------------------------------------
        Logger.Debug("InsertActivityData End")
        '-----------------------------------------------------------------
        'Me.Rollback = True
        Return True
    End Function


    ''' <summary>
    ''' 活動結果登録処理
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function UpdateActivityData(ByVal registdt As SC3080203RegistDataDataTable) As Boolean
        '--デバッグログ---------------------------------------------------
        Logger.Debug("UpdateActivityData Start")
        '-----------------------------------------------------------------

        Dim RegistRw As SC3080203DataSet.SC3080203RegistDataRow
        RegistRw = CType(registdt.Rows(0), SC3080203DataSet.SC3080203RegistDataRow)

        'ログインユーザー情報取得用
        Dim context As StaffContext = StaffContext.Current

        Dim dlrcd As String = context.DlrCD         '自身の店舗コード
        Dim strcd As String = context.BrnCD         '自身の販売店コード
        Dim fllwstrcd As String = RegistRw.FLLWSTRCD
        Dim account As String = context.Account     '自身のアカウント

        Dim fllwupbox_seqno As Long

        Dim cstkind As String               '顧客区分 1:自社客 2:未取引客
        Dim vin As String                   '活動の車両のVIN
        Dim seriesname As String
        Dim customerclass As String = "1"   '現状では1(所有者)のみ
        Dim crcustid As String              '未取引顧客ID、自社客ID、副顧客IDのいずれかを設定

        Dim actendflg As String             '0:継続中(Continue) 1:活動終了 2:GiveUp

        Dim actresult As String             '活動結果(画面で選択した値) 1:Walk-in(Cold)、2:Prospect(Warm)、3:Hot、4:Success、5:Give-up
        Dim actdate As String               '活動日(画面で入力した値)
        Dim actaccount As String            '活動実施者(画面で入力した値)
        Dim successcar As String            '成約車種(画面で入力した値)
        Dim nextactdate As String           '次回活動予定日(画面で入力した値)
        Dim memo As String = RegistRw.GIVEUPREASON                 '断念理由(画面で入力した値)

        Dim catalog As String
        Dim testdrive As String
        Dim assessment As String
        Dim valuation As String

        Dim appointtimeflg As String = "1"       '次回活動時間時分指定フラグ 0:なし、1:あり

        actresult = RegistRw.ACTRESULT
        cstkind = RegistRw.cstkind

        actaccount = RegistRw.ACTACCOUNT & "@" & context.DlrCD       '活動実施者(画面で入力した値)
        fllwupbox_seqno = RegistRw.FLLWSEQ

        actdate = RegistRw.ACTDAYFROM

        successcar = RegistRw.SUCCESSSERIES

        If String.Equals(RegistRw.ACTRESULT, C_RSLT_SUCCESS) Then
            actendflg = "1"
        ElseIf String.Equals(RegistRw.ACTRESULT, C_RSLT_GIVEUP) Then
            actendflg = "2"
        Else
            actendflg = "0"
        End If

        'Toは時分しか持っていないためFrom側から日付をセット
        Dim actdayto As String = RegistRw.ACTDAYFROM.Substring(0, 10) & " " & RegistRw.ACTDAYTO
        Dim actDayToDate As Date = Date.ParseExact(actdayto, "yyyy/MM/dd HH:mm", Nothing)


        If String.Equals(RegistRw.FOLLOWFLG, "1") Then
            If String.Equals(RegistRw.FOLLOWDAYTOFLG, "1") Then
                nextactdate = RegistRw.FOLLOWDAYFROM.Substring(0, 10) & " " & RegistRw.FOLLOWDAYTO
            Else
                nextactdate = RegistRw.FOLLOWDAYFROM
            End If
            appointtimeflg = If(RegistRw.FOLLOWTIMEFLG, "1", "0")
        Else
            If String.Equals(RegistRw.NEXTACTDAYTOFLG, "1") Then
                nextactdate = RegistRw.NEXTACTDAYFROM.Substring(0, 10) & " " & RegistRw.NEXTACTDAYTO
            Else
                nextactdate = RegistRw.NEXTACTDAYFROM
            End If
            appointtimeflg = If(RegistRw.NEXTACTTIMEFLG, "1", "0")
        End If

        '配列作成
        Dim wkary As String()
        Dim tempary As String()
        Dim seqdt As SC3080203SeqDataTable
        Dim seqrw As SC3080203SeqRow

        '全希望車種のSEQのリストを作成
        Dim selcar As String = ""
        seqdt = SC3080203TableAdapter.GetActHisCarSeq(dlrcd, fllwstrcd, fllwupbox_seqno)
        For j = 0 To seqdt.Count - 1
            seqrw = CType(seqdt.Rows(j), SC3080203SeqRow)
            selcar = selcar & seqrw.SEQNO & ","
        Next

        'カタログ実績がある希望車種のSEQのリストを作成
        catalog = ""
        wkary = RegistRw.SELECTACTCATALOG.Split(";"c)
        For i = 0 To wkary.Length - 2
            tempary = wkary(i).Split(","c)
            If String.Equals(tempary(1), "1") Then
                seqdt = SC3080203TableAdapter.GetActHisSelCarSeq(dlrcd, fllwstrcd, fllwupbox_seqno, tempary(0), "1")
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
                seqdt = SC3080203TableAdapter.GetActHisSelCarSeq(dlrcd, fllwstrcd, fllwupbox_seqno, tempary(0), "2")
                For j = 0 To seqdt.Count - 1
                    seqrw = CType(seqdt.Rows(j), SC3080203SeqRow)
                    testdrive = testdrive & seqrw.SEQNO & ","
                Next
            End If
        Next

        '見積り実績(見積りは全部に対して有り、無しの2択)
        assessment = RegistRw.SELECTACTASSESMENT

        '査定実績がある希望車種のSEQのリストを作成
        valuation = ""
        wkary = RegistRw.SELECTACTVALUATION.Split(";"c)
        For i = 0 To wkary.Length - 2
            tempary = wkary(i).Split(","c)
            If String.Equals(tempary(1), "1") Then
                seqdt = SC3080203TableAdapter.GetActHisSelCarSeq(dlrcd, fllwstrcd, fllwupbox_seqno, tempary(0), "4")
                For j = 0 To seqdt.Count - 1
                    seqrw = CType(seqdt.Rows(j), SC3080203SeqRow)
                    valuation = valuation & seqrw.SEQNO & ","
                Next
            End If
        Next

        'Follow-up Boxを取得
        Dim fllwboxdt As SC3080203DataSet.SC3080203FllwupBoxDataTable
        Dim fllwboxrw As SC3080203DataSet.SC3080203FllwupBoxRow
        fllwboxdt = SC3080203TableAdapter.GetFllwupBox(dlrcd, fllwstrcd, fllwupbox_seqno)
        fllwboxrw = CType(fllwboxdt.Rows(0), SC3080203FllwupBoxRow)
        Dim fllwcrplan_id As Nullable(Of Long)
        If fllwboxrw.IsCRPLAN_IDNull Then
            fllwcrplan_id = Nothing
        Else
            fllwcrplan_id = fllwboxrw.CRPLAN_ID
        End If
        Dim fllwpromotion_id As Nullable(Of Long)
        If fllwboxrw.IsPROMOTION_IDNull Then
            fllwpromotion_id = Nothing
        Else
            fllwpromotion_id = fllwboxrw.PROMOTION_ID
        End If
        Dim fllwvclseqno As Nullable(Of Long)
        If fllwboxrw.IsVCLSEQNONull Then
            fllwvclseqno = Nothing
        Else
            fllwvclseqno = fllwboxrw.VCLSEQNO
        End If
        vin = fllwboxrw.VIN
        Dim fllwcractlimitdate As String
        If fllwboxrw.IsCRACTLIMITDATENull Then
            fllwcractlimitdate = Nothing
        Else
            fllwcractlimitdate = fllwboxrw.CRACTLIMITDATE
        End If
        Dim fllwpromotionname As String
        If fllwboxrw.IsPROMOTIONNAMENull Then
            fllwpromotionname = Nothing
        Else
            fllwpromotionname = fllwboxrw.PROMOTIONNAME
        End If
        Dim fllwcondition As String
        If fllwboxrw.IsCONDITIONNull Then
            fllwcondition = Nothing
        Else
            fllwcondition = fllwboxrw.CONDITION
        End If
        Dim fllwrequestnm As String
        If fllwboxrw.IsREQUESTNMNull Then
            fllwrequestnm = Nothing
        Else
            fllwrequestnm = fllwboxrw.REQUESTNM
        End If

        '活動名を取得
        Dim cractname As String
        cractname = GetActName(fllwboxrw.SUBCTGORGNAME, fllwpromotionname, fllwcondition, fllwcrplan_id, fllwboxrw.CRACTCATEGORY,
                               fllwpromotion_id, fllwboxrw.REQCATEGORY, fllwrequestnm, fllwboxrw.CRACTRESULT)

        'Follow-up Box種別取得
        Dim fllwuptyp As String = "0"
        fllwuptyp = getFllwupBoxType(fllwboxrw.CRACTRESULT, fllwpromotion_id, fllwboxrw.CRACTCATEGORY, fllwboxrw.REQCATEGORY)

        If String.Equals(fllwboxrw.MEMKIND, "3") Then
            '会員種別が3(未取引客)の場合
            crcustid = fllwboxrw.UNTRADEDCSTID
        Else
            '会員種別が1,2(自社客)の場合
            crcustid = fllwboxrw.INSDID
        End If

        seriesname = fllwboxrw.SERIESNAME
        customerclass = "1"

        Dim cractlimitdate As String
        cractlimitdate = fllwcractlimitdate

        Dim thistime_cractresult As String = ""

        '選んだ活動結果がSuccess
        If String.Equals(actresult, C_RSLT_SUCCESS) Then
            thistime_cractresult = C_CRACTRSLT_SUCCESS
            '選んだ活動結果がGive-up
        ElseIf String.Equals(actresult, C_RSLT_GIVEUP) Then
            thistime_cractresult = C_CRACTRSLT_GIVEUP
            '選んだ活動結果がWalk-in
        ElseIf String.Equals(actresult, C_RSLT_WALKIN) Then
            thistime_cractresult = C_CRACTRSLT_CONTINUE
            '選んだ活動結果がProspect
        ElseIf String.Equals(actresult, C_RSLT_PROSPECT) Then
            If String.Equals(fllwboxrw.CRACTSTATUS, C_FLLWUP_PROSPECT) Then
                thistime_cractresult = C_CRACTRSLT_CONTINUE
            Else
                thistime_cractresult = C_CRACTRSLT_PROSPECT
            End If
        End If

        '選んだ活動結果がHot
        If String.Equals(actresult, C_RSLT_HOT) Then
            If String.Equals(fllwboxrw.CRACTSTATUS, C_FLLWUP_HOT) Then
                thistime_cractresult = C_CRACTRSLT_CONTINUE
            Else
                thistime_cractresult = C_CRACTRSLT_HOT
            End If
        End If

        Dim strselecteddvs As String                '活動結果
        strselecteddvs = actresult

        Dim thistime_cractstatus As String = ""     '今回の活動ステータス
        If String.Equals(actresult, C_RSLT_SUCCESS) Or String.Equals(actresult, C_RSLT_GIVEUP) Then
            thistime_cractstatus = fllwboxrw.CRACTSTATUS
        ElseIf String.Equals(actresult, C_RSLT_WALKIN) Then
            thistime_cractstatus = C_FLLWUP_WALKIN
        ElseIf String.Equals(actresult, C_RSLT_PROSPECT) Then
            thistime_cractstatus = C_FLLWUP_PROSPECT
        ElseIf String.Equals(actresult, C_RSLT_HOT) Then
            thistime_cractstatus = C_FLLWUP_HOT
        End If

        '各シーケンスNo用DataTable
        Dim sequencedt As SC3080203DataSet.SC3080203SequenceDataTable
        Dim sequencerw As SC3080203DataSet.SC3080203SequenceRow

        'Follow-up Boxを更新
        SC3080203TableAdapter.UpdateFllwupbox(thistime_cractresult, strselecteddvs, cractlimitdate, nextactdate, fllwboxrw.CRDVSID, account,
                                 thistime_cractstatus, dlrcd, fllwstrcd, fllwupbox_seqno, fllwuptyp, appointtimeflg)

        'Follow-up Box結果を取得
        Dim fllwrsltdt As SC3080203DataSet.SC3080203FllwupboxRsltDataTable
        Dim fllwrsltrw As SC3080203DataSet.SC3080203FllwupboxRsltRow
        fllwrsltdt = SC3080203TableAdapter.GetFllwupboxRslt(dlrcd, fllwstrcd, fllwupbox_seqno)
        fllwrsltrw = CType(fllwrsltdt.Rows(0), SC3080203FllwupboxRsltRow)

        'Follow-up Box結果を登録
        If String.Equals(actresult, C_RSLT_GIVEUP) Then
            'Give-upの場合断念理由を設定
            memo = RegistRw.GIVEUPREASON
        Else
            'それ以外のときは空
            memo = ""
        End If

        Dim successdate As String = actdate.Substring(0, 10)
        Dim giveupdate As String = actdate.Substring(0, 10)

        Dim strstall_reserveid As Nullable(Of Long) = Nothing
        Dim strstall_dlrcd As String = Nothing
        Dim strstall_strcd As String = Nothing

        Dim strrecid As Nullable(Of Long) = Nothing
        Dim strlogicflg As Integer = 9
        Dim strcmshislinkid As Nullable(Of Long) = Nothing



        Dim strpurchasedmakerno As String = RegistRw.GIVEUPMAKER
        Dim strpurchasedmakername As String = ""
        Dim strpurchasedmodelcd As String = RegistRw.GIVEUPMODEL
        Dim strpurchasedmodelname As String = ""

        Dim compdt As SC3080203DataSet.SC3080203CompetitionDataTable
        Dim comprw As SC3080203DataSet.SC3080203CompetitionRow

        compdt = SC3080203TableAdapter.GetCompetition(strpurchasedmakerno, strpurchasedmodelcd)
        If compdt.Count > 0 Then
            comprw = CType(compdt.Rows(0), SC3080203CompetitionRow)
            If comprw.IsCOMPETITIONMAKERNull Then
                strpurchasedmakername = ""
            Else
                strpurchasedmakername = comprw.COMPETITIONMAKER
            End If
            If comprw.IsCOMPETITORNMNull Then
                strpurchasedmodelname = ""
            Else
                strpurchasedmodelname = comprw.COMPETITORNM
            End If
        End If

        Dim strsuccesskind As String = "1"              '1(New Car)のみ

        Dim sysEnv As New SystemEnvSetting
        Dim sysEnvRow As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow
        Dim crrsltid As String                          'CR活動結果ID

        'CR活動結果IDを取得
        If String.Equals(actresult, C_RSLT_SUCCESS) Then
            sysEnvRow = sysEnv.GetSystemEnvSetting(CONTENT_SUCCESS_CRRSLTID)
        ElseIf String.Equals(actresult, C_RSLT_GIVEUP) Then
            sysEnvRow = sysEnv.GetSystemEnvSetting(CONTENT_GIVEUP_CRRSLTID)
        ElseIf String.Equals(actresult, C_RSLT_WALKIN) Then
            If String.Equals(fllwboxrw.CRACTSTATUS, C_FLLWUP_WALKIN) Then
                sysEnvRow = sysEnv.GetSystemEnvSetting(CONTENT_CONTINUE_CRRSLTID)
            Else
                sysEnvRow = sysEnv.GetSystemEnvSetting(CONTENT_WALKINREQUEST_CRRSLTID)
            End If
        ElseIf String.Equals(actresult, C_RSLT_PROSPECT) Then
            If String.Equals(fllwboxrw.CRACTSTATUS, C_FLLWUP_PROSPECT) Then
                sysEnvRow = sysEnv.GetSystemEnvSetting(CONTENT_CONTINUE_CRRSLTID)
            ElseIf String.Equals(actresult, C_RSLT_WALKIN) Then
                sysEnvRow = sysEnv.GetSystemEnvSetting(CONTENT_WALKINREQUEST_CRRSLTID)
            Else
                sysEnvRow = sysEnv.GetSystemEnvSetting(CONTENT_HOTPROSPECT_CRRSLTID)
            End If
        Else
            If String.Equals(fllwboxrw.CRACTSTATUS, C_FLLWUP_HOT) Then
                sysEnvRow = sysEnv.GetSystemEnvSetting(CONTENT_CONTINUE_CRRSLTID)
            ElseIf String.Equals(actresult, C_RSLT_WALKIN) Then
                sysEnvRow = sysEnv.GetSystemEnvSetting(CONTENT_WALKINREQUEST_CRRSLTID)
            Else
                sysEnvRow = sysEnv.GetSystemEnvSetting(CONTENT_HOTPROSPECT_CRRSLTID)
            End If
        End If
        crrsltid = sysEnvRow.PARAMVALUE

        Dim talkingtime As Long = 0L
        Dim strcalltime As String = ""

        Dim stractualtime_start As String = actdate         '活動を行った開始時間
        Dim stractualtime_end As String = actdayto

        '来店人数
        Dim walkinNum As Nullable(Of Integer) = Nothing
        If Not RegistRw.IsWALKINNUMNull Then
            walkinNum = RegistRw.WALKINNUM
        End If


        '2012/08/09 TCS 河原 Follow-upBox結果の次回活動日の修正 START
        SC3080203TableAdapter.InsertFllwupboxRslt(fllwboxrw.MEMKIND, thistime_cractresult, stractualtime_start, strcalltime, nextactdate,
                                                  fllwboxrw.INSURANCEFLG, memo, strpurchasedmakername, strpurchasedmodelcd, strpurchasedmodelname,
                                                  dlrcd, fllwupbox_seqno.ToString(CultureInfo.CurrentCulture()), fllwrsltrw.SEQNO.ToString(CultureInfo.CurrentCulture()),
                                                  fllwcrplan_id, fllwboxrw.BFAFDVS, fllwboxrw.CRDVSID.ToString(CultureInfo.CurrentCulture()),
                                                  fllwboxrw.PLANDVS, fllwboxrw.INSDID, fllwboxrw.UNTRADEDCSTID, fllwboxrw.VIN, crrsltid, account,
                                                  talkingtime.ToString(CultureInfo.CurrentCulture()), fllwboxrw.SUBCTGCODE, fllwpromotion_id,
                                                  successdate, strsuccesskind, fllwboxrw.SERIESCODE, fllwboxrw.SERIESNAME, giveupdate,
                                                  CType(context.OpeCD, String), fllwboxrw.SERVICECD, cractname, fllwboxrw.SUBCTGORGNAME_EX,
                                                  strstall_reserveid, strstall_dlrcd, strstall_strcd, fllwstrcd, strrecid, strlogicflg,
                                                  strcmshislinkid, thistime_cractstatus, fllwboxrw.CRACTSTATUS, strpurchasedmakerno, crcustid,
                                                  customerclass, fllwuptyp, appointtimeflg, Long.Parse(RegistRw.ACTCONTACT, CultureInfo.CurrentCulture()),
                                                  RegistRw.ACTDAYFROM, actdayto, stractualtime_end, fllwboxrw.ACCOUNT_PLAN, actaccount, context.BrnCD, walkinNum)
        '2012/08/09 TCS 河原 Follow-upBox結果の次回活動日の修正 END

        'Follow-up Box活動履歴を登録
        '活動内容設定
        Dim action As String = ""
        Select Case thistime_cractresult
            Case C_CRACTRSLT_HOT
                action = WebWordUtility.GetWord(30381)
            Case C_CRACTRSLT_PROSPECT
                action = WebWordUtility.GetWord(30380)
            Case C_CRACTRSLT_SUCCESS
                action = WebWordUtility.GetWord(30382)
            Case C_CRACTRSLT_CONTINUE
                action = WebWordUtility.GetWord(30379)
            Case C_CRACTRSLT_GIVEUP
                action = WebWordUtility.GetWord(30383)
        End Select

        SC3080203TableAdapter.InsertFllwupboxCRHisRslt(fllwboxrw.INSURANCEFLG, stractualtime_start, thistime_cractresult, strselecteddvs, dlrcd, fllwstrcd,
                                                                fllwupbox_seqno, fllwcrplan_id, fllwboxrw.BFAFDVS, fllwboxrw.CRDVSID.ToString(CultureInfo.CurrentCulture()), fllwboxrw.INSDID, fllwboxrw.SERIESCODE,
                                                                fllwboxrw.SERIESNAME, context.Account, fllwboxrw.VCLREGNO, fllwboxrw.SUBCTGCODE, fllwpromotion_id, crrsltid,
                                                                fllwboxrw.PLANDVS, actdate.Substring(0, 10), action, context.Account, fllwboxrw.SERVICECD, cractname,
                                                                fllwboxrw.SUBCTGORGNAME_EX, strstall_reserveid, strstall_dlrcd, strstall_strcd, strrecid, strcmshislinkid,
                                                                fllwrsltrw.SEQNO, actdayto)

        'Follow-up Box活動更新を存在確認
        Dim fllwentrdt As SC3080203DataSet.SC3080203CountDataTable
        fllwentrdt = SC3080203TableAdapter.GetFllwupboxEntry(dlrcd, fllwstrcd, fllwupbox_seqno)

        If fllwentrdt.Count > 0 Then
            'Follow-up Box活動更新を更新
            SC3080203TableAdapter.UpdateFllwupboxEntry(context.DlrCD, fllwstrcd, fllwupbox_seqno, context.Account)
        Else
            'Follow-up Box活動更新を登録
            SC3080203TableAdapter.InsertFllwupboxEntry(context.DlrCD, fllwstrcd, fllwupbox_seqno, context.Account)
        End If


        'メモが入力されている場合メモの登録を行う
        If String.IsNullOrEmpty(memo) = False Then
            Dim vclinforegistflg As String
            If String.Equals(cstkind, "1") Then
                Select Case fllwboxrw.CRACTRESULT
                    Case "", "0"
                        vclinforegistflg = "1"
                    Case "1", "2"
                        vclinforegistflg = "0"
                    Case Else
                        vclinforegistflg = "1"
                End Select
            Else
                vclinforegistflg = "0"
            End If
            SC3080203TableAdapter.InsertCustMemohis(customerclass, cractname, vclinforegistflg, cstkind, crcustid, context.DlrCD,
                                                             context.BrnCD, context.Account, memo, fllwboxrw.INSDID, fllwboxrw.VIN)
        End If

        'Follow-up Box活動実施のカテゴリ取得
        Dim doneCategory As String
        doneCategory = getFllwupDoneCategory(fllwuptyp)

        'Follow-up Box活動実施の存在確認
        Dim fllwdndt As SC3080203DataSet.SC3080203FllwupboxrsltDoneDataTable
        Dim fllwdnrw As SC3080203DataSet.SC3080203FllwupboxrsltDoneRow
        fllwdndt = SC3080203TableAdapter.GetFllwupboxrsltDone(dlrcd, fllwstrcd, fllwupbox_seqno, doneCategory)

        If fllwdndt.Count = 0 Then
            Select Case fllwuptyp
                Case C_FLLWUP_HOT
                    If thistime_cractresult <> C_CRACTRSLT_PROSPECT Then
                        'Follow-up Box活動実施の登録
                        SC3080203TableAdapter.InsertFllwupboxrsltDone(dlrcd, fllwstrcd, fllwupbox_seqno, doneCategory, actendflg,
                                                                               strcd, fllwboxrw.ACCOUNT_PLAN, C_SALESSTAFFOPECD, account)
                    End If
                Case C_FLLWUP_PROSPECT
                    If thistime_cractresult <> C_CRACTRSLT_HOT Then
                        'Follow-up Box活動実施の登録
                        SC3080203TableAdapter.InsertFllwupboxrsltDone(dlrcd, fllwstrcd, fllwupbox_seqno, doneCategory, actendflg,
                                                                               strcd, fllwboxrw.ACCOUNT_PLAN, C_SALESSTAFFOPECD, account)
                    End If
                Case Else
                    'Follow-up Box活動実施の登録
                    SC3080203TableAdapter.InsertFllwupboxrsltDone(dlrcd, fllwstrcd, fllwupbox_seqno, doneCategory, actendflg, strcd,
                                                                           fllwboxrw.ACCOUNT_PLAN, C_SALESSTAFFOPECD, account)
            End Select
        Else
            fllwdnrw = CType(fllwdndt.Rows(0), SC3080203FllwupboxrsltDoneRow)
            If String.Equals(fllwdnrw.ACTENDFLG, "0") And thistime_cractresult <> C_CRACTRSLT_CONTINUE Then
                'Follow-up Box活動実施の更新
                SC3080203TableAdapter.UpdateFllwupboxrsltDone(dlrcd, fllwstrcd, fllwupbox_seqno, account, doneCategory, actendflg)
            End If
        End If

        'Total履歴の登録
        Dim totalstatus As String = ""
        Select Case thistime_cractresult
            Case C_CRACTRSLT_HOT
                totalstatus = WebWordUtility.GetWord(30381)
            Case C_CRACTRSLT_PROSPECT
                totalstatus = WebWordUtility.GetWord(30380)
            Case C_CRACTRSLT_SUCCESS
                totalstatus = WebWordUtility.GetWord(30382)
            Case C_CRACTRSLT_CONTINUE
                totalstatus = WebWordUtility.GetWord(30379)
            Case C_CRACTRSLT_GIVEUP
                totalstatus = WebWordUtility.GetWord(30383)
        End Select

        SC3080203TableAdapter.InsertTotalHisRslt(fllwboxrw.INSDID, dlrcd, fllwstrcd, fllwboxrw.UNTRADEDCSTID, vin, seriesname, totalstatus,
                                                          fllwpromotion_id, cractname, actaccount)

        'TBL_FLREQUEST追加
        SC3080203TableAdapter.UpdateFLRequest(thistime_cractresult, cractlimitdate, nextactdate, fllwboxrw.INSURANCEFLG, fllwboxrw.SERIESCODE,
                                                       strsuccesskind, fllwboxrw.SERIESNAME, actdate, memo, dlrcd, fllwstrcd, fllwupbox_seqno, fllwuptyp,
                                                       giveupdate)

        '未取引客で活動結果がSuccessかGive-upの場合InReserveInfoに登録
        If String.Equals(cstkind, "2") And (String.Equals(actresult, C_RSLT_SUCCESS) Or String.Equals(actresult, C_RSLT_GIVEUP)) Then
            '052.非活動対象要件情報SeqNo取得
            Dim inreserveidwk As Long
            sequencedt = SC3080203TableAdapter.GetSeqInreserveInfoInreserveId()
            sequencerw = CType(sequencedt.Rows(0), SC3080203SequenceRow)
            inreserveidwk = sequencerw.Seq

            '10桁にゼロ埋めして未取引客IDを作成
            Dim inreserveid As String
            inreserveid = "INR" & CStr(inreserveidwk).PadLeft(10, "0"c)

            'Successの場合活動日を、Give-upの場合は空を設定
            Dim inreserveactdate As String
            If String.Equals(actresult, C_RSLT_SUCCESS) Then
                'inreserveactdate = actdate 
                inreserveactdate = actdayto
            Else
                inreserveactdate = ""
            End If

            '非活動対象要件情報を登録
            SC3080203TableAdapter.InsertInreserveInfo(inreserveid, dlrcd, strcd, account, fllwupbox_seqno, fllwboxrw.REQUESTID,
                                                               fllwboxrw.UNTRADEDCSTID, fllwvclseqno, inreserveactdate, actaccount, strcd, actaccount)
        End If

        '活動結果がSuccessの場合、成約車種に登録
        If String.Equals(actresult, C_RSLT_SUCCESS) Then
            Dim successcarary As String()
            successcarary = successcar.Split(";"c)
            Dim successcararywk As String()
            Dim cnt As Integer = 1

            '希望車種の種類分ループ
            For i = 0 To successcarary.Length - 2
                successcararywk = successcarary(i).Split(","c)
                If String.Equals(successcararywk(1), "1") Then
                    '希望車種より台数を取得
                    Dim carnumdt As SC3080203DataSet.SC3080203SelectedCarNumDataTable
                    Dim carnumrw As SC3080203DataSet.SC3080203SelectedCarNumRow
                    carnumdt = SC3080203TableAdapter.GetSelectedCarNum(dlrcd, fllwstrcd, fllwupbox_seqno, successcararywk(0))
                    carnumrw = CType(carnumdt.Rows(0), SC3080203SelectedCarNumRow)

                    For j = 1 To carnumrw.QUANTITY
                        'Follow-up Box成約車種の登録
                        SC3080203TableAdapter.InsertFllwupboxSuccessSeries(cnt, account, dlrcd, fllwstrcd, fllwupbox_seqno, successcararywk(0))
                        cnt = cnt + 1
                    Next
                End If
            Next
        End If

        'その他計画(Follow-up)の存在確認
        Dim othrplndt As SC3080203DataSet.SC3080203CountDataTable
        othrplndt = SC3080203TableAdapter.GetOtherPlanFllw(dlrcd, fllwstrcd, fllwupbox_seqno)

        'Dim crresult As String

        Dim cractresult As String = thistime_cractresult

        Dim cractcategory As String
        cractcategory = fllwboxrw.CRACTCATEGORY

        Dim categorywk As String = Nothing

        If String.Equals(cractresult, "1") Then
            categorywk = "1"
        ElseIf String.Equals(cractresult, "2") Then
            categorywk = "2"
        Else
            If String.Equals(cractresult, "0") Or String.Equals(cractresult, "4") Then
                If String.Equals(cractcategory, "1") Or String.Equals(cractcategory, "4") Then
                    categorywk = "3"
                ElseIf String.Equals(cractcategory, "2") Then
                    categorywk = "4"
                Else
                    If Not fllwpromotion_id Is Nothing Then
                        categorywk = "5"
                    Else
                        If String.Equals(fllwboxrw.REQCATEGORY, "1") Then
                            categorywk = "7"
                        Else
                            categorywk = "6"
                        End If
                    End If
                End If
            End If
        End If

        Dim planstatus As String            '活動継続なら0、活動終了なら1
        If String.Equals(actresult, C_RSLT_WALKIN) Or String.Equals(actresult, C_RSLT_PROSPECT) Or String.Equals(actresult, C_RSLT_HOT) Then
            planstatus = "0"
        Else
            planstatus = "1"
        End If

        Dim cractresultflg As String        'CR活動結果更新可否
        If String.Equals(actresult, C_RSLT_WALKIN) Then
            cractresultflg = "1"
        Else
            cractresultflg = "0"
        End If

        Dim crenddate As Date
        crenddate = actDayToDate

        'その他計画が存在するか
        If othrplndt.Count = 0 Then
            'その他計画(Follow-up)の登録
            SC3080203TableAdapter.InsertOtherPlanFllwRslt(categorywk, crrsltid, planstatus, categorywk, actDayToDate, strcd,
                                                                   actaccount, planstatus, CONTENT_MODULEID, account, CONTENT_MODULEID, account,
                                                                   dlrcd, fllwstrcd, fllwupbox_seqno)
        Else
            'その他計画(Follow-up)の更新
            SC3080203TableAdapter.UpdateOtherPlanFllwRslt(planstatus, cractresult, fllwboxrw.CRACTRESULT, cractresultflg, categorywk,
                                                                   crenddate, crrsltid, fllwboxrw.CRDVSID, strcd, actaccount, account, nextactdate,
                                                                   appointtimeflg, CONTENT_MODULEID, dlrcd, fllwstrcd, fllwupbox_seqno)
        End If

        '希望車種に対する活動実績を入力する
        If RegistRw.PROCESSFLG.Equals("1") Then
            InsertHistory(dlrcd, fllwstrcd, fllwupbox_seqno, selcar, catalog, testdrive, assessment, valuation, account, actdayto)
        End If

        '031.Follow-up Box商談メモ追加
        SC3080203TableAdapter.InsertFllwupboxSalesmemo(dlrcd, fllwstrcd, fllwupbox_seqno, cstkind, customerclass, crcustid, account, CONTENT_MODULEID)

        '055.Follow-up Box商談メモWK削除
        SC3080203TableAdapter.DeleteFllwupboxSalesmemowk(dlrcd, fllwstrcd, fllwupbox_seqno)

        '--デバッグログ---------------------------------------------------
        Logger.Debug("UpdateActivityData End")
        '-----------------------------------------------------------------
        Return True
    End Function


    ''' <summary>
    ''' 希望車種に対する活動実績を入力する
    ''' </summary>
    ''' <param name="dlrcd"></param>
    ''' <param name="fllwstrcd"></param>
    ''' <param name="fllwupboxseqno"></param>
    ''' <param name="selcar"></param>
    ''' <param name="catalog"></param>
    ''' <param name="testdrive"></param>
    ''' <param name="assessment"></param>
    ''' <param name="valuation"></param>
    ''' <param name="account"></param>
    ''' <param name="actdate"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function InsertHistory(ByVal dlrcd As String, ByVal fllwstrcd As String, ByVal fllwupboxseqno As Long,
                                     ByVal selcar As String, ByVal catalog As String, ByVal testdrive As String,
                                     ByVal assessment As String, ByVal valuation As String, ByVal account As String, ByVal actdate As String) As Boolean

        '--デバッグログ---------------------------------------------------
        Logger.Debug("InsertHistory Start")
        '-----------------------------------------------------------------
        '各活動実績の登録
        '登録に必要なデータ取得
        '各シーケンスNo用DataTable
        Dim sequencedt As SC3080203DataSet.SC3080203SequenceDataTable
        Dim sequencerw As SC3080203DataSet.SC3080203SequenceRow

        Dim actdateDt As Date = Date.ParseExact(actdate, "yyyy/MM/dd HH:mm", Nothing)

        sequencedt = SC3080203TableAdapter.GetFllwRsltSeq(dlrcd, fllwstrcd, fllwupboxseqno)

        Dim fllwupboxrslt_seqno As Long
        If sequencedt.Count = 0 Then
            fllwupboxrslt_seqno = 0
        Else
            sequencerw = CType(sequencedt.Rows(0), SC3080203SequenceRow)
            fllwupboxrslt_seqno = sequencerw.Seq - 1
        End If

        '実績登録用にフォローアップボックスのデータを取得
        Dim ActHisFllwdt As SC3080203DataSet.SC3080203ActHisFllwDataTable
        Dim ActHisFllwrw As SC3080203DataSet.SC3080203ActHisFllwRow
        ActHisFllwdt = SC3080203TableAdapter.GetActHisFllw(dlrcd, fllwstrcd, fllwupboxseqno)
        ActHisFllwrw = CType(ActHisFllwdt.Rows(0), SC3080203ActHisFllwRow)

        Dim ActHisFllwCrplan_id As Nullable(Of Long)
        If ActHisFllwrw.IsCRPLAN_IDNull Then
            ActHisFllwCrplan_id = Nothing
        Else
            ActHisFllwCrplan_id = ActHisFllwrw.CRPLAN_ID
        End If

        Dim ActHisFllwPromotion_id As Nullable(Of Long)
        If ActHisFllwrw.IsPROMOTION_IDNull Then
            ActHisFllwPromotion_id = Nothing
        Else
            ActHisFllwPromotion_id = ActHisFllwrw.PROMOTION_ID
        End If

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

        Dim catalogary As String() = catalog.Split(","c)
        Dim testdriveary As String() = testdrive.Split(","c)
        Dim valuationary As String() = valuation.Split(","c)

        Dim ActHisSelCardt As SC3080203DataSet.SC3080203ActHisSelCarDataTable
        Dim ActHisSelCarrw As SC3080203DataSet.SC3080203ActHisSelCarRow

        Dim cntcd As String = EnvironmentSetting.CountryCode

        '希望車種の台数分ループ
        For i = 0 To selcarary.Length - 2
            '希望車種の情報取得
            ActHisSelCardt = SC3080203TableAdapter.GetActHisCarSeq(dlrcd, fllwstrcd, fllwupboxseqno, Long.Parse(selcarary(i), CultureInfo.CurrentCulture()), cntcd)
            ActHisSelCarrw = CType(ActHisSelCardt.Rows(0), SC3080203ActHisSelCarRow)

            Dim ActHisSelCaVclmodel_Name As String
            If ActHisSelCarrw.IsVCLMODEL_NAMENull Then
                ActHisSelCaVclmodel_Name = Nothing
            Else
                ActHisSelCaVclmodel_Name = ActHisSelCarrw.VCLMODEL_NAME
            End If

            Dim ActHisSelCardisp_Bdy_Color As String
            If ActHisSelCarrw.IsDISP_BDY_COLORNull Then
                ActHisSelCardisp_Bdy_Color = Nothing
            Else
                ActHisSelCardisp_Bdy_Color = ActHisSelCarrw.DISP_BDY_COLOR
            End If

            'カタログの実績確認
            For j = 0 To catalogary.Length - 2
                If selcarary(i) = catalogary(j) Then
                    SC3080203TableAdapter.InserActHisFllwCrHis(dlrcd, fllwstrcd, fllwupboxseqno, ActHisFllwCrplan_id,
                                                                        ActHisFllwrw.BFAFDVS, ActHisFllwrw.CRDVSID, ActHisFllwrw.INSDID,
                                                                        ActHisFllwrw.SERIESCODE, ActHisFllwrw.SERIESNAME, account,
                                                                        ActHisFllwrw.VCLREGNO, ActHisFllwrw.SUBCTGCODE, ActHisFllwrw.SERVICECD,
                                                                        ActHisFllwrw.SUBCTGORGNAME, ActHisFllwrw.SUBCTGORGNAME_EX,
                                                                        ActHisFllwPromotion_id, ActHisFllwrw.CRACTRESULT, ActHisFllwrw.PLANDVS,
                                                                        actdateDt, ActHisCatalogrw.METHOD, ActHisCatalogrw.ACTION,
                                                                        ActHisCatalogrw.ACTIONTYPE, ActHisFllwrw.ACCOUNT_PLAN,
                                                                        ActHisCatalogrw.ACTIONCD, CONTENT_SEQ_CATALOG, Long.Parse(selcarary(i), CultureInfo.CurrentCulture()),
                                                                        ActHisSelCarrw.SERIESNM, ActHisSelCaVclmodel_Name, ActHisSelCardisp_Bdy_Color,
                                                                        ActHisSelCarrw.QUANTITY, fllwupboxrslt_seqno)

                    SC3080203TableAdapter.InserActHisFllwTotalHis(dlrcd, fllwstrcd, ActHisFllwrw.INSDID, actdateDt, ActHisCatalogrw.CATEGORYID.ToString(CultureInfo.CurrentCulture()),
                                                                           ActHisCatalogCategorydvsid, ActHisFllwrw.VIN, ActHisFllwrw.SERIESNAME,
                                                                           ActHisFllwrw.CUSTCHRGSTAFFNM, ActHisFllwrw.CRCUSTID, ActHisFllwrw.CUSTOMERCLASS)
                End If
            Next

            '試乗実績確認
            For j = 0 To testdriveary.Length - 2
                If selcarary(i) = testdriveary(j) Then
                    SC3080203TableAdapter.InserActHisFllwCrHis(dlrcd, fllwstrcd, fllwupboxseqno, ActHisFllwCrplan_id, ActHisFllwrw.BFAFDVS,
                                                                        ActHisFllwrw.CRDVSID, ActHisFllwrw.INSDID, ActHisFllwrw.SERIESCODE, ActHisFllwrw.SERIESNAME,
                                                                        account, ActHisFllwrw.VCLREGNO, ActHisFllwrw.SUBCTGCODE, ActHisFllwrw.SERVICECD,
                                                                        ActHisFllwrw.SUBCTGORGNAME, ActHisFllwrw.SUBCTGORGNAME_EX, ActHisFllwPromotion_id,
                                                                        ActHisFllwrw.CRACTRESULT, ActHisFllwrw.PLANDVS, actdateDt, ActHisTestDriverw.METHOD,
                                                                        ActHisTestDriverw.ACTION, ActHisTestDriverw.ACTIONTYPE, ActHisFllwrw.ACCOUNT_PLAN,
                                                                        ActHisTestDriverw.ACTIONCD, CONTENT_SEQ_TESTDRIVE, Long.Parse(selcarary(i), CultureInfo.CurrentCulture()), ActHisSelCarrw.SERIESNM,
                                                                        ActHisSelCaVclmodel_Name, ActHisSelCardisp_Bdy_Color, ActHisSelCarrw.QUANTITY,
                                                                        fllwupboxrslt_seqno)

                    SC3080203TableAdapter.InserActHisFllwTotalHis(dlrcd, fllwstrcd, ActHisFllwrw.INSDID, actdateDt, ActHisTestDriverw.CATEGORYID.ToString(CultureInfo.CurrentCulture()),
                                                                           ActHisTestDriveCategorydvsid, ActHisFllwrw.VIN, ActHisFllwrw.SERIESNAME,
                                                                           ActHisFllwrw.CUSTCHRGSTAFFNM, ActHisFllwrw.CRCUSTID, ActHisFllwrw.CUSTOMERCLASS)
                End If
            Next

            '査定実績確認
            If String.Equals(assessment, "1") Then
                SC3080203TableAdapter.InserActHisFllwCrHis(dlrcd, fllwstrcd, fllwupboxseqno, ActHisFllwCrplan_id, ActHisFllwrw.BFAFDVS,
                                            ActHisFllwrw.CRDVSID, ActHisFllwrw.INSDID, ActHisFllwrw.SERIESCODE, ActHisFllwrw.SERIESNAME,
                                            account, ActHisFllwrw.VCLREGNO, ActHisFllwrw.SUBCTGCODE, ActHisFllwrw.SERVICECD,
                                            ActHisFllwrw.SUBCTGORGNAME, ActHisFllwrw.SUBCTGORGNAME_EX, ActHisFllwPromotion_id,
                                            ActHisFllwrw.CRACTRESULT, ActHisFllwrw.PLANDVS, actdateDt, ActHisAssessmentrw.METHOD,
                                            ActHisAssessmentrw.ACTION, ActHisAssessmentrw.ACTIONTYPE, ActHisFllwrw.ACCOUNT_PLAN,
                                            ActHisAssessmentrw.ACTIONCD, CONTENT_SEQ_ASSESSMENT, Long.Parse(selcarary(i), CultureInfo.CurrentCulture()), ActHisSelCarrw.SERIESNM,
                                            ActHisSelCaVclmodel_Name, ActHisSelCardisp_Bdy_Color, ActHisSelCarrw.QUANTITY,
                                            fllwupboxrslt_seqno)

                SC3080203TableAdapter.InserActHisFllwTotalHis(dlrcd, fllwstrcd, ActHisFllwrw.INSDID, actdateDt, ActHisAssessmentrw.CATEGORYID.ToString(CultureInfo.CurrentCulture()),
                                                                       ActHisAssessmentCategorydvsid, ActHisFllwrw.VIN, ActHisFllwrw.SERIESNAME,
                                                                       ActHisFllwrw.CUSTCHRGSTAFFNM, ActHisFllwrw.CRCUSTID, ActHisFllwrw.CUSTOMERCLASS)
            End If

            '見積り実績確認
            For j = 0 To valuationary.Length - 2
                If selcarary(i) = valuationary(j) Then
                    SC3080203TableAdapter.InserActHisFllwCrHis(dlrcd, fllwstrcd, fllwupboxseqno, ActHisFllwCrplan_id, ActHisFllwrw.BFAFDVS,
                                                ActHisFllwrw.CRDVSID, ActHisFllwrw.INSDID, ActHisFllwrw.SERIESCODE, ActHisFllwrw.SERIESNAME,
                                                account, ActHisFllwrw.VCLREGNO, ActHisFllwrw.SUBCTGCODE, ActHisFllwrw.SERVICECD,
                                                ActHisFllwrw.SUBCTGORGNAME, ActHisFllwrw.SUBCTGORGNAME_EX, ActHisFllwPromotion_id,
                                                ActHisFllwrw.CRACTRESULT, ActHisFllwrw.PLANDVS, actdateDt, ActHisValuationrw.METHOD,
                                                ActHisValuationrw.ACTION, ActHisValuationrw.ACTIONTYPE, ActHisFllwrw.ACCOUNT_PLAN,
                                                ActHisValuationrw.ACTIONCD, CONTENT_SEQ_VALUATION, Long.Parse(selcarary(i), CultureInfo.CurrentCulture()), ActHisSelCarrw.SERIESNM,
                                                ActHisSelCaVclmodel_Name, ActHisSelCardisp_Bdy_Color, ActHisSelCarrw.QUANTITY,
                                                fllwupboxrslt_seqno)

                    SC3080203TableAdapter.InserActHisFllwTotalHis(dlrcd, fllwstrcd, ActHisFllwrw.INSDID, actdateDt, ActHisValuationrw.CATEGORYID.ToString(CultureInfo.CurrentCulture()),
                                                                           ActHisValuationCategorydvsid, ActHisFllwrw.VIN, ActHisFllwrw.SERIESNAME,
                                                                           ActHisFllwrw.CUSTCHRGSTAFFNM, ActHisFllwrw.CRCUSTID, ActHisFllwrw.CUSTOMERCLASS)
                End If
            Next
        Next

        '--デバッグログ---------------------------------------------------
        Logger.Debug("InsertHistory End")
        '-----------------------------------------------------------------
        Return True
    End Function


    ''' <summary>
    ''' 活動結果を登録する
    ''' </summary>
    ''' <param name="registdt"></param>
    ''' <param name="resistFlg">1:新規活動登録　2:新規活動登録からの即Success、Give-up　3:既存の活動に対する活動結果</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Function InsertActivityResult(ByVal registdt As SC3080203RegistDataDataTable, ByVal resistFlg As String, ByVal fllwStatus As String) As Boolean
        '--デバッグログ---------------------------------------------------
        Logger.Debug("InsertActivityResult Start")
        '-----------------------------------------------------------------

        If String.Equals(resistFlg, "1") Or String.Equals(resistFlg, "2") Then
            InsertActivityData(registdt)
        End If

        If String.Equals(resistFlg, "2") Or String.Equals(resistFlg, "3") Then
            UpdateActivityData(registdt)
        End If

        Dim staffInfo As StaffContext = StaffContext.Current
        Dim msgid As Integer = 0
        Dim staffStatus As String = staffInfo.PresenceCategory & staffInfo.PresenceDetail
        Dim UpdateVisitSales As New UpdateSalesVisitBusinessLogic

        If String.Equals(staffStatus, STAFF_STATUS_NEGOTIATION) Then

            '2012/08/09 TCS 河原 【A STEP2】次世代e-CRB セールスKPI出力開発 START
            '来店実績更新_商談終了
            Dim endDate As Date = DateTimeFunc.Now(staffInfo.DlrCD)
            '2012/08/09 TCS 河原 【A STEP2】次世代e-CRB セールスKPI出力開発 END

            '一時的にコメントアウトする
            'UpdateVisitSales.UpdateVisitSalesEnd(registdt(0).ACTACCOUNT, staffInfo.DlrCD, registdt(0).FLLWSTRCD, registdt(0).FLLWSEQ, endDate, staffInfo.Account, CONTENT_MODULEID, msgid)
            ' 2012/08/09 TCS 安田 【SALES_3】 START
            'UpdateVisitSales.UpdateVisitSalesEnd(registdt(0).CUSTSEGMENT, registdt(0).CRCUSTID, endDate, CONTENT_MODULEID, msgid)
            UpdateVisitSales.UpdateVisitSalesEnd(registdt(0).CUSTSEGMENT, registdt(0).CRCUSTID, endDate, CONTENT_MODULEID, msgid, UpdateSalesVisitBusinessLogic.LogicStateNegotiationFinish)
            ' 2012/08/09 TCS 安田 【SALES_3】 END

            If msgid <> 0 Then
                'ロールバック
                Rollback = True
                '--デバッグログ---------------------------------------------------
                Logger.Debug("来店実績更新_商談終了 失敗")
                '-----------------------------------------------------------------
                Return False
            End If
        End If

        'Toは時分しか持っていないためFrom側から日付をセット
        Dim actaccount As String = registdt(0).ACTACCOUNT & "@" & staffInfo.DlrCD       '活動実施者(画面で入力した値)
        Dim actFromDate As Date = Date.ParseExact(registdt(0).ACTDAYFROM, "yyyy/MM/dd HH:mm", Nothing)
        Dim actdayto As String = registdt(0).ACTDAYFROM.Substring(0, 10) & " " & registdt(0).ACTDAYTO
        Dim actToDate As Date = Date.ParseExact(actdayto, "yyyy/MM/dd HH:mm", Nothing)

        'FLLWUPBOX 商談を更新
        SC3080203DataSetTableAdapters.SC3080203TableAdapter.UpdateFllwupboxSales(staffInfo.DlrCD, _
                                                                                    registdt(0).FLLWSTRCD, _
                                                                                    registdt(0).FLLWSEQ, _
                                                                                    actaccount, _
                                                                                    actFromDate, _
                                                                                    actToDate, _
                                                                                    staffInfo.Account, _
                                                                                    CONTENT_MODULEID)


        'ステータスを「スタンバイ」に更新
        staffInfo.UpdatePresence("1", "0")

        'CalDAV連携実施
        SetToDo(registdt, fllwStatus)

        ' 2012/03/13 TCS 安田 【SALES_2】 START
        'If String.Equals(staffStatus, STAFF_STATUS_NEGOTIATION) Then
        '    '活動登録処理完了時、
        '    '来店実績更新_商談終了時のPush送信
        '    UpdateVisitSales.PushUpdateVisitSalesEnd()
        'End If
        ' 2012/03/13 TCS 安田 【SALES_2】 END

        '--デバッグログ---------------------------------------------------
        Logger.Debug("InsertActivityResult End")
        '-----------------------------------------------------------------
        Return True
    End Function

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
        Logger.Debug("GetActName Start")
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
        Logger.Debug("GetActName End")
        '-----------------------------------------------------------------
        Return recactname
    End Function


    ''' <summary>
    ''' 入力チェック処理
    ''' </summary>
    ''' <param name="Registdt"></param>
    ''' <param name="msgid"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function IsInputeCheck(ByVal registdt As SC3080203RegistDataDataTable, ByRef msgid As Integer) As Boolean
        '--デバッグログ---------------------------------------------------
        Logger.Debug("IsInputeCheck Start")
        '-----------------------------------------------------------------
        Dim Registrw As SC3080203RegistDataRow
        Registrw = CType(registdt.Rows(0), SC3080203RegistDataRow)

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
        If actDayToDate > Now() Then
            msgid = 30906
            Return False
        End If

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

        '詳細を256文字以内で入力してください
        If Registrw.ACTRESULT = C_RSLT_GIVEUP Then
            If (Validation.IsCorrectDigit(Registrw.GIVEUPREASON, 256) = False) Then
                msgid = 30928
                Return False
            End If
        End If

        '禁則文字
        If Registrw.ACTRESULT = C_RSLT_GIVEUP Then
            '禁則文字チェック
            If Not Validation.IsValidString(Registrw.GIVEUPREASON) Then
                msgid = 30930
                Return False
            End If
        End If

        '--デバッグログ---------------------------------------------------
        Logger.Debug("IsInputeCheck End OK")
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
        Logger.Debug("SetToDo Start")
        '-----------------------------------------------------------------

        'ログインユーザー情報取得用
        Dim context As StaffContext = StaffContext.Current
        Dim sysEnv As New SystemEnvSetting
        Dim sysEnvRow As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow
        sysEnvRow = sysEnv.GetSystemEnvSetting(CONTENT_KEISYO_ZENGO)
        Dim nmtitledt As SC3080203NameTitleDataTable
        Dim RegistRw As SC3080203DataSet.SC3080203RegistDataRow
        RegistRw = CType(registdt.Rows(0), SC3080203RegistDataRow)
        If String.Equals(RegistRw.cstkind, "1") Then
            nmtitledt = SC3080203TableAdapter.GetOrgNameTitle(RegistRw.INSDID)
        Else
            nmtitledt = SC3080203TableAdapter.GetNewNameTitle(RegistRw.INSDID)
        End If
        Dim nmtitlerw As SC3080203NameTitleRow
        nmtitlerw = CType(nmtitledt.Rows(0), SC3080203NameTitleRow)
        Using sendObj As New IC3040401.IC3040401BusinessLogic
            '共通設定項目作成
            sendObj.CreateCommon()
            '親レコード設定
            sendObj.ActionType = "0"
            sendObj.DealerCode = context.DlrCD
            sendObj.BranchCode = RegistRw.FLLWSTRCD
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
                    clrrw = CType(clrdt.Rows(0), SC3080203TodoColorRow)
                    sendObj.BackgroundColor(0) = clrrw.BACKGROUNDCOLOR
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
                '--デバッグログ---------------------------------------------------
                Logger.Debug("Webサービス連携 失敗")
                '-----------------------------------------------------------------
                Return False
            End If
        End Using
        '--デバッグログ---------------------------------------------------
        Logger.Debug("SetToDo End")
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
        Logger.Debug("getFllwupDoneCategory Start")
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
        Logger.Debug("getFllwupDoneCategory End")
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
        Logger.Debug("getFllwupBoxType Start")
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
        Logger.Debug("getFllwupBoxType End")
        '-----------------------------------------------------------------
        Return fllwupBoxType
    End Function


    ''' <summary>
    ''' 競合車種取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetNoCompetitionMakermaster() As SC3080203DataSet.SC3080203CompetitionMakermasterDataTable
        Return SC3080203TableAdapter.GetNoCompetitionMakermaster()
    End Function


    ''' <summary>
    ''' Follow-up Box商談取得 (開始時間、来店人数取得)
    ''' </summary>
    ''' <param name="dlrCd"></param>
    ''' <param name="strCd"></param>
    ''' <param name="fllwupboxSeqNo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetFllwupboxSales(dlrCD As String, strCD As String, fllwupboxSeqNo As Long) As SC3080203DataSet.SC3080203FllwupboxSalesDataTable
        Return SC3080203TableAdapter.GetFllwupboxSales(dlrCD, strCD, fllwupboxSeqNo)
    End Function


    ''' <summary>
    ''' 最大の活動終了時間を取得
    ''' </summary>
    ''' <param name="dlrCD"></param>
    ''' <param name="strCD"></param>
    ''' <param name="fllwupboxSeqNo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetLatestActTimeEnd(dlrCD As String, strCD As String, fllwupboxSeqNo As Long) As SC3080203DataSet.SC3080203LatestActTimeDataTable
        Return SC3080203TableAdapter.GetLatestActTimeEnd(dlrCD, strCD, fllwupboxSeqNo)
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










#End Region

End Class
