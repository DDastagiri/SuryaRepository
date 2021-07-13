'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'ActivityInfoBusinessLogic.vb
'─────────────────────────────────────
'機能： 顧客詳細共通処理
'補足： 
'作成：  
'更新： 2012/02/27 TCS 安田 【SALES_2】
'更新： 2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善
'─────────────────────────────────────

Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.CustomerInfo.Details.DataAccess
Imports Toyota.eCRB.CommonUtility.DataAccess
Imports Toyota.eCRB.CommonUtility.DataAccess.ActivityInfoDataSet
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.iCROP.BizLogic
Imports System.Globalization
Imports Toyota.eCRB.Common.VisitResult.BizLogic
Imports Toyota.eCRB.CustomerInfo.Details.BizLogic

Public Class ActivityInfoBusinessLogic
    Inherits BaseBusinessComponent

#Region "定数"

    ' 活動結果(Success)
    Public Const CRACTRESULT_SUCCESS As String = "3"

    ' アクションコード(SEQ)
    ' カタログ
    Public Const ACTIONCD_CATALOG As String = "A22"
    ' 試乗
    Public Const ACTIONCD_TESTDRIVE As String = "A26"
    ' 査定
    Public Const ACTIONCD_EVALUATION As String = "A30"
    ' 見積
    Public Const ACTIONCD_QUOTATION As String = "A23"

    ' 受注時
    Public Const SALESAFTER_NO As String = "0"
    ' 受注後
    Public Const SALESAFTER_YES As String = "1"

    ' 受注後工程（振当待ち）
    Public Const WAITINGOBJECT_ALLOCATION = "001"
    ' 受注後工程（入金待ち）
    Public Const WAITINGOBJECT_PAYMENT = "002"
    ' 受注後工程（納車待ち）
    Public Const WAITINGOBJECT_DELIVERY = "005"
    ' 受注後工程（納車済み）
    Public Const WAITINGOBJECT_SUCCESS = "007"


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

#End Region

#Region "メソット"

    ''' <summary>
    ''' デフォルトコンストラクタ
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        '処理なし
    End Sub

    ''' <summary>
    ''' 項目内の情報をコピーする (未使用)
    ''' </summary>
    ''' <param name="copyRow">コピー先データ (アウトプット)</param>
    ''' <param name="sendRow">コピー元データ (インプット)</param>
    ''' <remarks></remarks>
    Private Shared Sub CopyColumus(ByVal copyRow As DataRow, ByVal sendRow As DataRow)

        Dim i As Integer = 0
        For i = 0 To sendRow.Table.Columns.Count - 1
            copyRow(sendRow.Table.Columns(i).ColumnName) = sendRow(sendRow.Table.Columns(i).ColumnName)
        Next

    End Sub

    ''' <summary>
    ''' データテーブルをコピーする (未使用)
    ''' </summary>
    ''' <param name="copyTbl">コピー先データテーブル (アウトプット)</param>
    ''' <param name="sendTbl">コピー元データテーブル (インプット)</param>
    ''' <remarks></remarks>
    Private Shared Sub CopyTable(ByVal copyTbl As DataTable, ByVal sendTbl As DataTable)

        Dim newRow As DataRow
        For i = 0 To sendTbl.Rows.Count - 1
            newRow = copyTbl.NewRow
            CopyColumus(newRow, sendTbl.Rows(i))
            copyTbl.Rows.Add(newRow)
        Next
    End Sub


    ''' <summary>
    ''' 担当SC一覧取得
    ''' </summary>
    ''' <returns>担当SC一覧データテーブル</returns>
    ''' <remarks></remarks>
    Shared Function GetUsers() As ActivityInfoDataSet.ActivityInfoUsersDataTable

        Dim context As StaffContext = StaffContext.Current
        Return ActivityInfoTableAdapter.GetUsers(context.DlrCD, context.BrnCD)

    End Function

    ''' <summary>
    ''' シリーズ単位の希望車種取得
    ''' </summary>
    ''' <param name="fllwStrcd">Follow-up Box店舗コード</param>
    ''' <param name="fllwupboxSeqNo">Follow-up Box内連番</param>
    ''' <returns>シリーズ単位の希望車種データテーブル</returns>
    ''' <remarks></remarks>
    Shared Function GetFllwSeries(ByVal fllwStrcd As String, ByVal fllwupboxseqno As Long) As ActivityInfoDataSet.ActivityInfoFllwSeriesDataTable

        Dim context As StaffContext = StaffContext.Current
        Return (ActivityInfoTableAdapter.GetFllwSeries(context.DlrCD, fllwStrcd, EnvironmentSetting.CountryCode, fllwupboxseqno))

    End Function

    ''' <summary>
    ''' グレード単位の希望車種の取得
    ''' </summary>
    ''' <param name="fllwStrcd">Follow-up Box店舗コード</param>
    ''' <param name="fllwupboxSeqNo">Follow-up Box内連番</param>
    ''' <returns>グレード単位の希望車種データテーブル</returns>
    ''' <remarks></remarks>
    Shared Function GetFllwModel(ByVal fllwStrcd As String, ByVal fllwupboxseqno As Long) As ActivityInfoDataSet.ActivityInfoFllwModelDataTable

        Dim context As StaffContext = StaffContext.Current
        Return ActivityInfoTableAdapter.GetFllwModel(context.DlrCD, fllwStrcd, EnvironmentSetting.CountryCode, fllwupboxseqno)

    End Function

    ''' <summary>
    ''' カラー単位の希望車種の取得
    ''' </summary>
    ''' <param name="fllwStrcd">Follow-up Box店舗コード</param>
    ''' <param name="fllwupboxSeqNo">Follow-up Box内連番</param>
    ''' <returns>カラー単位の希望車種データテーブル</returns>
    ''' <remarks></remarks>
    Shared Function GetFllwColor(ByVal fllwStrcd As String, ByVal fllwupboxseqno As Long) As ActivityInfoDataSet.ActivityInfoFllwColorDataTable

        Dim context As StaffContext = StaffContext.Current
        Return ActivityInfoTableAdapter.GetFllwColor(context.DlrCD, fllwStrcd, EnvironmentSetting.CountryCode, fllwupboxseqno)

    End Function


    ''' <summary>
    ''' 活動方法取得
    ''' </summary>
    ''' <param name="bookedafterflg">受注後フラグ (指定がなければ全件検索)</param>
    ''' <returns>活動方法データテーブル</returns>
    ''' <remarks></remarks>
    Shared Function GetActContact(ByVal bookedafterflg As String) As ActivityInfoDataSet.ActivityInfoActContactDataTable

        Return ActivityInfoTableAdapter.GetActContact(bookedafterflg)

    End Function


    ''' <summary>
    ''' 文言取得
    ''' </summary>
    ''' <param name="serchdt"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetContentWord(ByVal serchdt As ActivityInfoDataSet.ActivityInfoSeqDataTable) As ActivityInfoDataSet.ActivityInfoContentWordDataTable

        Dim Serchrw As ActivityInfoDataSet.ActivityInfoSeqRow
        Serchrw = CType(serchdt.Rows(0), ActivityInfoDataSet.ActivityInfoSeqRow)
        Return ActivityInfoTableAdapter.GetContentWord(Serchrw.SEQNO)

    End Function

    ''' <summary>
    ''' 日付フォーマット取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetDateFormat() As ActivityInfoDataSet.ActivityInfoDateFormatDataTable

        Return ActivityInfoTableAdapter.GetDateFormat()

    End Function

    ''' <summary>
    ''' アイコンのパス取得
    ''' </summary>
    ''' <param name="seqno"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetContentIconPath(ByVal seqno As Integer) As ActivityInfoDataSet.ActivityInfoContentIconPathDataTable

        Return ActivityInfoTableAdapter.GetContentIconPath(seqno)

    End Function

    ''' <summary>
    ''' 新規登録処理
    ''' </summary>
    ''' <param name="registdt">データテーブル (インプット)</param>
    ''' <param name="programId">更新プログラムID</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Shared Function InsertActivityData(ByVal registdt As ActivityInfoDataSet.ActivityInfoRegistDataDataTable, _
                                       ByVal programId As String) As Boolean

        '--デバッグログ---------------------------------------------------
        Logger.Debug("InsertActivityData Start")
        '-----------------------------------------------------------------

        Dim RegistRw As ActivityInfoDataSet.ActivityInfoRegistDataRow
        RegistRw = CType(registdt.Rows(0), ActivityInfoRegistDataRow)

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

        cstkind = RegistRw.CSTKIND               '顧客区分 1:自社客 2:未取引客
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
        Dim seqdt As ActivityInfoSeqDataTable
        Dim seqrw As ActivityInfoSeqRow
        '全希望車種のSEQのリストを作成
        Dim selcar As String = ""
        seqdt = ActivityInfoTableAdapter.GetActHisCarSeq(dlrcd, fllwstrcd, fllwupbox_seqno)
        For j As Integer = 0 To seqdt.Count - 1
            seqrw = CType(seqdt.Rows(j), ActivityInfoSeqRow)
            selcar = selcar & seqrw.SEQNO & ","
        Next

        'カタログ実績がある希望車種のSEQのリストを作成
        catalog = ""
        wkary = RegistRw.SELECTACTCATALOG.Split(";"c)
        For i = 0 To wkary.Length - 2
            tempary = wkary(i).Split(","c)
            If String.Equals(tempary(1), "1") Then
                seqdt = ActivityInfoTableAdapter.GetActHisSelCarSeq(dlrcd, fllwstrcd, fllwupbox_seqno, tempary(0), "1")
                For j = 0 To seqdt.Count - 1
                    seqrw = CType(seqdt.Rows(j), ActivityInfoSeqRow)
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
                seqdt = ActivityInfoTableAdapter.GetActHisSelCarSeq(dlrcd, fllwstrcd, fllwupbox_seqno, tempary(0), "2")
                For j = 0 To seqdt.Count - 1
                    seqrw = CType(seqdt.Rows(j), ActivityInfoSeqRow)
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
                seqdt = ActivityInfoTableAdapter.GetActHisSelCarSeq(dlrcd, fllwstrcd, fllwupbox_seqno, tempary(0), "4")
                For j = 0 To seqdt.Count - 1
                    seqrw = CType(seqdt.Rows(j), ActivityInfoSeqRow)
                    valuation = valuation & seqrw.SEQNO & ","
                Next
            End If
        Next

        'シリーズコード、シリーズ名取得
        Dim Srsdt As ActivityInfoSeriesDataTable
        Dim Srsrw As ActivityInfoSeriesRow
        If String.Equals(cstkind, "1") Then
            '自社の場合
            originalid = crcustid

            vin = carid
            Srsdt = ActivityInfoTableAdapter.GetVclinfo(originalid, vin)
            If Srsdt.Count > 0 Then
                Srsrw = CType(Srsdt.Rows(0), ActivityInfoSeriesRow)
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
                Srsdt = ActivityInfoTableAdapter.GetNewcustVclinfo(cstid, newcustcarseq.Value)
                If Srsdt.Count > 0 Then
                    Srsrw = CType(Srsdt.Rows(0), ActivityInfoSeriesRow)
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
            Dim newcustiddt As ActivityInfoDataSet.ActivityInfoNewCustIDDataTable
            Dim newcustidrw As ActivityInfoDataSet.ActivityInfoNewCustIDRow
            newcustiddt = ActivityInfoTableAdapter.GetNewCustID(crcustid)

            If newcustiddt.Count > 0 Then
                newcustidrw = CType(newcustiddt.Rows(0), ActivityInfoNewCustIDRow)
                cstid = newcustidrw.CSTID

                Dim newvcliddt As ActivityInfoDataSet.ActivityInfoNewVclIDDataTable
                Dim newvclidrw As ActivityInfoDataSet.ActivityInfoNewVclIDRow
                newvcliddt = ActivityInfoTableAdapter.GetNewVclID(cstid, vin)

                If newvcliddt.Count > 0 Then
                    newvclidrw = CType(newvcliddt.Rows(0), ActivityInfoNewVclIDRow)
                    newcustcarseq = newvclidrw.SEQNO
                Else
                    newcustcarseq = Nothing
                End If
            Else
                cstid = ""
            End If
        End If

        Dim sequencedt As ActivityInfoDataSet.ActivityInfoSequenceDataTable
        Dim sequencerw As ActivityInfoDataSet.ActivityInfoSequenceRow
        '自社客で未取引客情報未登録の場合
        If String.Equals(cstkind, "1") Then
            If String.IsNullOrEmpty(cstid) Then
                sequencedt = ActivityInfoTableAdapter.GetSeqNewcustomerCstId()
                sequencerw = CType(sequencedt.Rows(0), ActivityInfoSequenceRow)
                cstkindwk = sequencerw.SEQ
                '10桁にゼロ埋めして未取引客IDを作成
                cstid = "NCST" & CStr(cstkindwk).PadLeft(10, "0"c)
                '034.未取引客個人情報追加
                ActivityInfoTableAdapter.InsertNweCustomer(cstid, originalid, vin)
            End If
            If newcustcarseq Is Nothing Then
                '056.メーカー名取得
                Dim makerdt As ActivityInfoMakernameDataTable
                Dim makerrw As ActivityInfoMakernameRow
                makerdt = ActivityInfoTableAdapter.GetMakername(dlrcd, cntcd, seriescd)

                If makerdt.Count > 0 Then
                    makerrw = CType(makerdt.Rows(0), ActivityInfoMakernameRow)
                    makername = makerrw.MAKERNAME
                Else
                    makername = " "
                End If
                '054.未取引客車両情報追加SeqNo取得
                sequencedt = ActivityInfoTableAdapter.GetSeqNewcustomerVclreSeqno()
                sequencerw = CType(sequencedt.Rows(0), ActivityInfoSequenceRow)
                newcustcarseq = sequencerw.SEQ
                '035.未取引客車両情報追加
                ActivityInfoTableAdapter.InsertNweCustomerVclre(cstid, newcustcarseq.Value, makername, originalid, vin)
            End If
        End If

        '051.Walk-in Person SeqNo取得
        sequencedt = ActivityInfoTableAdapter.GetSeqWalkInPersonWalkInId()
        sequencerw = CType(sequencedt.Rows(0), ActivityInfoSequenceRow)
        walkinidwk = sequencerw.SEQ

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
        ActivityInfoTableAdapter.InsertWalkInPerson(walkinid, cstid, newcustcarseq, dlrcd, strcd, actDayToDate, seriescd, seriesname,
                                   registrationtype, actDayToDate, Integer.Parse(wicid, CultureInfo.CurrentCulture()), fllwupbox_seqno, crcustid,
                                   originalid, account, context.UserName, Long.Parse(RegistRw.ACTCONTACT, CultureInfo.CurrentCulture()), RegistRw.ACTDAYFROM, actdayto, actaccount, walkinNum)


        '045.ウォークイン要件メモ追加
        ActivityInfoTableAdapter.InsertWalkInPersonMemo(walkinid, dlrcd)

        '057.TotalHisSqeNo取得
        sequencedt = ActivityInfoTableAdapter.GetSeqTotalhisSeqno()
        sequencerw = CType(sequencedt.Rows(0), ActivityInfoSequenceRow)
        totalhisseq = sequencerw.SEQ

        'Completed文言取得
        Dim thisstatus As String
        thisstatus = WebWordUtility.GetWord(30356)

        '042.Total履歴追加(Walk-in)
        ActivityInfoTableAdapter.InsertTotalHis(dlrcd, strcd, crcustid, totalhisseq, actDayToDate, "7", "0", vin, seriesname,
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
        Dim relaifdt As ActivityInfoDataSet.ActivityInfoSequenceDataTable
        Dim relaifrw As ActivityInfoDataSet.ActivityInfoSequenceRow
        relaifdt = ActivityInfoTableAdapter.GetRelatedInfo(vin)
        relaifrw = CType(relaifdt.Rows(0), ActivityInfoSequenceRow)
        Dim relatedinfoflg As String = Nothing
        If relaifrw.SEQ > 0 Then
            relatedinfoflg = "1"
        Else
            relatedinfoflg = "0"
        End If

        Dim nextcractivedate As Nullable(Of Date) = Nothing
        If String.Equals(cstkind, "1") Then
            '自社客走行距離履歴よりサービススタッフ情報を取得
            Dim srvstaffdt As ActivityInfoDataSet.ActivityInfoServiceStaffDataTable
            Dim srvstaffrw As ActivityInfoDataSet.ActivityInfoServiceStaffRow
            srvstaffdt = ActivityInfoTableAdapter.GetServiceStaff(originalid, vin)
            Dim servicestaffcd As String
            Dim servicestaffnm As String

            If srvstaffdt.Count > 0 Then
                srvstaffrw = CType(srvstaffdt.Rows(0), ActivityInfoServiceStaffRow)
                servicestaffcd = srvstaffrw.SERVICESTAFFCD
                servicestaffnm = srvstaffrw.SERVICESTAFFNM
            Else
                servicestaffcd = " "
                servicestaffnm = " "
            End If

            '007.Follow-up Box追加(自社客)
            '次回活動担当は自分固定
            ActivityInfoTableAdapter.InsertFllwupbox(dlrcd, strcd, fllwupbox_seqno, cractivedate, appointtimeflg, cractivedate, cstid, newcustcarseq.ToString, cractresult, strcd,
                                    account, originalid, vin, relatedinfoflg, nextcractivedate, account, cractresult, prospect_date, hot_date, Long.Parse(wicid, CultureInfo.CurrentCulture()),
                                    cractstatus, cractstatus, strcd, account, cractivedate, cractivedate, crcustid, account, originalid, servicestaffcd, servicestaffnm)
        Else
            '007.Follow-up Box追加(未取引客)
            ActivityInfoTableAdapter.InsertNewCustFllwupbox(dlrcd, strcd, fllwupbox_seqno, cractivedate, appointtimeflg, cractivedate, cstid, newcustcarseq, cractresult, strcd,
                                           account, relatedinfoflg, nextcractivedate, account, cractresult, prospect_date, hot_date, Long.Parse(wicid, CultureInfo.CurrentCulture()),
                                           cractstatus, cractstatus, strcd, account, cractivedate, cractivedate, crcustid, account, cstid)
        End If

        '058.来店区分取得
        Dim wicdt As ActivityInfoDataSet.ActivityInfoWinclassDataTable
        Dim wicrw As ActivityInfoDataSet.ActivityInfoWinclassRow
        wicdt = ActivityInfoTableAdapter.GetWinclass(wicid)
        wicrw = CType(wicdt.Rows(0), ActivityInfoWinclassRow)

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

        ActivityInfoTableAdapter.InsertFllwupboxCRHis(calldate, callaccount, crdvs, actualtime_end, actDayToDate, method, action, actiontype, brnchaccount, actioncd, ctntseqno,
                                     select_series_seqno, seriesnm, vclmodel_name, disp_bdy_color, quantity, fllwupboxrslt_seqno, dlrcd, strcd, fllwupbox_seqno)

        '活動結果がProspectの場合
        If String.Equals(actresult, "2") Then
            actioncd = CONTENT_HOT_ACTIONCD
            action = "Prospect"
            ActivityInfoTableAdapter.InsertFllwupboxCRHis(calldate, callaccount, crdvs, actualtime_end, actDayToDate, method, action, actiontype, brnchaccount, actioncd, ctntseqno,
                                         select_series_seqno, seriesnm, vclmodel_name, disp_bdy_color, quantity, fllwupboxrslt_seqno, dlrcd, strcd, fllwupbox_seqno)
        End If

        '活動結果がProspectの場合
        If String.Equals(actresult, "3") Then
            actioncd = CONTENT_PROSPECT_ACTIONCD
            action = "Hot"
            ActivityInfoTableAdapter.InsertFllwupboxCRHis(calldate, callaccount, crdvs, actualtime_end, actDayToDate, method, action, actiontype, brnchaccount, actioncd, ctntseqno,
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
            sequencedt = ActivityInfoTableAdapter.GetSeqTotalhisSeqno()
            sequencerw = CType(sequencedt.Rows(0), ActivityInfoSequenceRow)
            totalhisseq = sequencerw.SEQ

            'TotalHisの登録(Hot/Prospectの履歴)
            ActivityInfoTableAdapter.InsertTotalHis(dlrcd, strcd, crcustid, totalhisseq, actDayToDate, "7", "", vin, seriesname, totalHisStatus, service_nm, actaccount, crcustid)
        End If

        '057.TotalHisSqeNo取得
        sequencedt = ActivityInfoTableAdapter.GetSeqTotalhisSeqno()
        sequencerw = CType(sequencedt.Rows(0), ActivityInfoSequenceRow)
        totalhisseq = sequencerw.SEQ

        '042.Total履歴追加(Follow-upBox)
        ActivityInfoTableAdapter.InsertTotalHis(dlrcd, strcd, crcustid, totalhisseq, actDayToDate, "4", "", vin, seriesname, "", service_nm, actaccount, crcustid)

        '023.Follow-up Box未取引客情報追加
        ActivityInfoTableAdapter.InsertFllwupboxNewcst(dlrcd, strcd, fllwupbox_seqno, account, cstid, newcustcarseq)

        '自社客の場合Follow-up Box詳細を作成
        If String.Equals(cstkind, "1") Then
            Dim lastserviceindate As String = Nothing
            Dim last_subctgcode As String = Nothing
            Dim last_servicecd As String = Nothing
            Dim last_servicename As String = Nothing
            Dim lastmileage As String = Nothing
            Dim lastserviceinbranch As String = Nothing

            'VIN取得
            Dim vindt As ActivityInfoVinDataTable
            Dim vinrw As ActivityInfoVinRow
            vindt = ActivityInfoTableAdapter.GetVin(originalid)

            For i = 0 To vindt.Count - 1
                vinrw = CType(vindt.Rows(i), ActivityInfoVinRow)

                last_subctgcode = " "
                last_servicecd = " "
                last_servicename = " "

                '048.自社客走行距離履歴取得
                Dim milehisdt As ActivityInfoMileageHisDataTable
                Dim milehisrw As ActivityInfoMileageHisRow
                milehisdt = ActivityInfoTableAdapter.GetMileageHis(originalid, vinrw.VIN)
                If milehisdt.Count > 0 Then
                    '取得できた場合変数に値をセット
                    milehisrw = CType(milehisdt.Rows(0), ActivityInfoMileageHisRow)

                    If milehisrw.IsREGISTDATENull() Then
                        lastserviceindate = Nothing
                    Else
                        lastserviceindate = milehisrw.REGISTDATE
                    End If

                    lastmileage = milehisrw.MILEAGE.ToString(CultureInfo.CurrentCulture())
                    lastserviceinbranch = milehisrw.STRCD

                    '049.自社客点検履歴取得
                    Dim srvhisdt As ActivityInfoServiceHisDataTable
                    Dim srvhisrw As ActivityInfoServiceHisRow
                    srvhisdt = ActivityInfoTableAdapter.GetServiceHis(milehisrw.DLRCD, milehisrw.JOBNO)
                    If srvhisdt.Count > 0 Then
                        srvhisrw = CType(srvhisdt.Rows(0), ActivityInfoServiceHisRow)

                        '040.サービスマスタ取得
                        Dim srvmstdt As ActivityInfoServiceMasterDataTable
                        Dim srvmstrw As ActivityInfoServiceMasterRow
                        srvmstdt = ActivityInfoTableAdapter.GetServiceMaster(srvhisrw.SERVICECD, dlrcd)
                        If srvmstdt.Count > 0 Then
                            srvmstrw = CType(srvmstdt.Rows(0), ActivityInfoServiceMasterRow)
                            last_servicecd = srvmstrw.SERVICECD
                            last_servicename = srvmstrw.SERVICENAME

                            '041.中項目マスタ取得
                            Dim subctgdt As ActivityInfoSubCategoryDataTable
                            Dim subctgrw As ActivityInfoSubCategoryRow
                            subctgdt = ActivityInfoTableAdapter.GetSubCategory(srvmstrw.SERVICECD)
                            If subctgdt.Count > 0 Then
                                subctgrw = CType(subctgdt.Rows(0), ActivityInfoSubCategoryRow)
                                last_subctgcode = subctgrw.SUBCTGCODE
                            End If
                        End If
                    End If
                End If

                '019.Follow-up Box詳細追加
                ActivityInfoTableAdapter.InserFllwupboxDetail(dlrcd, strcd, fllwupbox_seqno, lastserviceindate, last_subctgcode, last_servicecd, last_servicename,
                                             lastmileage, lastserviceinbranch, originalid, vinrw.VIN)

                If milehisdt.Count > 0 Then
                    '092.Follow-up Box走行距離履歴の追加
                    milehisrw = CType(milehisdt.Rows(0), ActivityInfoMileageHisRow)
                    ActivityInfoTableAdapter.insertFllwupboxMilehis(dlrcd, strcd, fllwupbox_seqno, originalid, vinrw.VIN, milehisrw.MILEAGESEQ, milehisrw.REGISTDATE, milehisrw.MILEAGE, "1", milehisrw.JOBNO)
                End If

            Next

        End If

        '037.その他計画(Follow-up)追加
        ActivityInfoTableAdapter.InsertOtherPlanFllw(programId, account, programId, account, dlrcd, strcd, fllwupbox_seqno)

        '031.Follow-up Box商談メモ追加
        ActivityInfoTableAdapter.InsertFllwupboxSalesmemo(dlrcd, strcd, fllwupbox_seqno, cstkind, customerclass, crcustid, account, programId)

        '055.Follow-up Box商談メモWK削除
        ActivityInfoTableAdapter.DeleteFllwupboxSalesmemowk(dlrcd, strcd, fllwupbox_seqno)

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
        Dim sequencedt As ActivityInfoDataSet.ActivityInfoSequenceDataTable
        Dim sequencerw As ActivityInfoDataSet.ActivityInfoSequenceRow

        Dim actdateDt As Date = Date.ParseExact(actdate, "yyyy/MM/dd HH:mm", Nothing)

        sequencedt = ActivityInfoTableAdapter.GetFllwRsltSeq(dlrcd, fllwstrcd, fllwupboxseqno)

        Dim fllwupboxrslt_seqno As Long
        If sequencedt.Count = 0 Then
            fllwupboxrslt_seqno = 0
        Else
            sequencerw = CType(sequencedt.Rows(0), ActivityInfoSequenceRow)
            fllwupboxrslt_seqno = sequencerw.SEQ - 1
        End If

        '実績登録用にフォローアップボックスのデータを取得
        Dim ActHisFllwdt As ActivityInfoDataSet.ActivityInfoActHisFllwDataTable
        Dim ActHisFllwrw As ActivityInfoDataSet.ActivityInfoActHisFllwRow
        ActHisFllwdt = ActivityInfoTableAdapter.GetActHisFllw(dlrcd, fllwstrcd, fllwupboxseqno)
        ActHisFllwrw = CType(ActHisFllwdt.Rows(0), ActivityInfoActHisFllwRow)

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
        Dim ActHisCatalogdt As ActivityInfoDataSet.ActivityInfoActHisContentDataTable
        Dim ActHisCatalogrw As ActivityInfoDataSet.ActivityInfoActHisContentRow
        ActHisCatalogdt = ActivityInfoTableAdapter.GetActHisContent(CONTENT_SEQ_CATALOG)
        ActHisCatalogrw = CType(ActHisCatalogdt.Rows(0), ActivityInfoActHisContentRow)

        Dim ActHisCatalogCategorydvsid As Nullable(Of Long)
        If ActHisCatalogrw.IsCATEGORYDVSIDNull Then
            ActHisCatalogCategorydvsid = Nothing
        Else
            ActHisCatalogCategorydvsid = ActHisCatalogrw.CATEGORYDVSID
        End If

        '試乗のマスタ情報取得()
        Dim ActHisTestDrivedt As ActivityInfoDataSet.ActivityInfoActHisContentDataTable
        Dim ActHisTestDriverw As ActivityInfoDataSet.ActivityInfoActHisContentRow
        ActHisTestDrivedt = ActivityInfoTableAdapter.GetActHisContent(CONTENT_SEQ_TESTDRIVE)
        ActHisTestDriverw = CType(ActHisTestDrivedt.Rows(0), ActivityInfoActHisContentRow)

        Dim ActHisTestDriveCategorydvsid As Nullable(Of Long)
        If ActHisTestDriverw.IsCATEGORYDVSIDNull Then
            ActHisTestDriveCategorydvsid = Nothing
        Else
            ActHisTestDriveCategorydvsid = ActHisTestDriverw.CATEGORYDVSID
        End If

        '査定のマスタ情報取得
        Dim ActHisAssessmentdt As ActivityInfoDataSet.ActivityInfoActHisContentDataTable
        Dim ActHisAssessmentrw As ActivityInfoDataSet.ActivityInfoActHisContentRow
        ActHisAssessmentdt = ActivityInfoTableAdapter.GetActHisContent(CONTENT_SEQ_ASSESSMENT)
        ActHisAssessmentrw = CType(ActHisAssessmentdt.Rows(0), ActivityInfoActHisContentRow)

        Dim ActHisAssessmentCategorydvsid As Nullable(Of Long)
        If ActHisAssessmentrw.IsCATEGORYDVSIDNull Then
            ActHisAssessmentCategorydvsid = Nothing
        Else
            ActHisAssessmentCategorydvsid = ActHisAssessmentrw.CATEGORYDVSID
        End If

        '見積りのマスタ情報取得
        Dim ActHisValuationdt As ActivityInfoDataSet.ActivityInfoActHisContentDataTable
        Dim ActHisValuationrw As ActivityInfoDataSet.ActivityInfoActHisContentRow
        ActHisValuationdt = ActivityInfoTableAdapter.GetActHisContent(CONTENT_SEQ_VALUATION)
        ActHisValuationrw = CType(ActHisValuationdt.Rows(0), ActivityInfoActHisContentRow)

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

        Dim ActHisSelCardt As ActivityInfoDataSet.ActivityInfoActHisSelCarDataTable
        Dim ActHisSelCarrw As ActivityInfoDataSet.ActivityInfoActHisSelCarRow

        Dim cntcd As String = EnvironmentSetting.CountryCode

        '希望車種の台数分ループ
        For i = 0 To selcarary.Length - 2
            '希望車種の情報取得
            ActHisSelCardt = ActivityInfoTableAdapter.GetActHisCarSeq(dlrcd, fllwstrcd, fllwupboxseqno, Long.Parse(selcarary(i), CultureInfo.CurrentCulture()), cntcd)
            ActHisSelCarrw = CType(ActHisSelCardt.Rows(0), ActivityInfoActHisSelCarRow)

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
                    ActivityInfoTableAdapter.InserActHisFllwCrHis(dlrcd, fllwstrcd, fllwupboxseqno, ActHisFllwCrplan_id,
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

                    ActivityInfoTableAdapter.InserActHisFllwTotalHis(dlrcd, fllwstrcd, ActHisFllwrw.INSDID, actdateDt, ActHisCatalogrw.CATEGORYID.ToString(CultureInfo.CurrentCulture()),
                                                                           ActHisCatalogCategorydvsid, ActHisFllwrw.VIN, ActHisFllwrw.SERIESNAME,
                                                                           ActHisFllwrw.CUSTCHRGSTAFFNM, ActHisFllwrw.CRCUSTID, ActHisFllwrw.CUSTOMERCLASS)
                End If
            Next

            '試乗実績確認
            For j = 0 To testdriveary.Length - 2
                If selcarary(i) = testdriveary(j) Then
                    ActivityInfoTableAdapter.InserActHisFllwCrHis(dlrcd, fllwstrcd, fllwupboxseqno, ActHisFllwCrplan_id, ActHisFllwrw.BFAFDVS,
                                                                        ActHisFllwrw.CRDVSID, ActHisFllwrw.INSDID, ActHisFllwrw.SERIESCODE, ActHisFllwrw.SERIESNAME,
                                                                        account, ActHisFllwrw.VCLREGNO, ActHisFllwrw.SUBCTGCODE, ActHisFllwrw.SERVICECD,
                                                                        ActHisFllwrw.SUBCTGORGNAME, ActHisFllwrw.SUBCTGORGNAME_EX, ActHisFllwPromotion_id,
                                                                        ActHisFllwrw.CRACTRESULT, ActHisFllwrw.PLANDVS, actdateDt, ActHisTestDriverw.METHOD,
                                                                        ActHisTestDriverw.ACTION, ActHisTestDriverw.ACTIONTYPE, ActHisFllwrw.ACCOUNT_PLAN,
                                                                        ActHisTestDriverw.ACTIONCD, CONTENT_SEQ_TESTDRIVE, Long.Parse(selcarary(i), CultureInfo.CurrentCulture()), ActHisSelCarrw.SERIESNM,
                                                                        ActHisSelCaVclmodel_Name, ActHisSelCardisp_Bdy_Color, ActHisSelCarrw.QUANTITY,
                                                                        fllwupboxrslt_seqno)

                    ActivityInfoTableAdapter.InserActHisFllwTotalHis(dlrcd, fllwstrcd, ActHisFllwrw.INSDID, actdateDt, ActHisTestDriverw.CATEGORYID.ToString(CultureInfo.CurrentCulture()),
                                                                           ActHisTestDriveCategorydvsid, ActHisFllwrw.VIN, ActHisFllwrw.SERIESNAME,
                                                                           ActHisFllwrw.CUSTCHRGSTAFFNM, ActHisFllwrw.CRCUSTID, ActHisFllwrw.CUSTOMERCLASS)
                End If
            Next

            '査定実績確認
            If String.Equals(assessment, "1") Then
                ActivityInfoTableAdapter.InserActHisFllwCrHis(dlrcd, fllwstrcd, fllwupboxseqno, ActHisFllwCrplan_id, ActHisFllwrw.BFAFDVS,
                                            ActHisFllwrw.CRDVSID, ActHisFllwrw.INSDID, ActHisFllwrw.SERIESCODE, ActHisFllwrw.SERIESNAME,
                                            account, ActHisFllwrw.VCLREGNO, ActHisFllwrw.SUBCTGCODE, ActHisFllwrw.SERVICECD,
                                            ActHisFllwrw.SUBCTGORGNAME, ActHisFllwrw.SUBCTGORGNAME_EX, ActHisFllwPromotion_id,
                                            ActHisFllwrw.CRACTRESULT, ActHisFllwrw.PLANDVS, actdateDt, ActHisAssessmentrw.METHOD,
                                            ActHisAssessmentrw.ACTION, ActHisAssessmentrw.ACTIONTYPE, ActHisFllwrw.ACCOUNT_PLAN,
                                            ActHisAssessmentrw.ACTIONCD, CONTENT_SEQ_ASSESSMENT, Long.Parse(selcarary(i), CultureInfo.CurrentCulture()), ActHisSelCarrw.SERIESNM,
                                            ActHisSelCaVclmodel_Name, ActHisSelCardisp_Bdy_Color, ActHisSelCarrw.QUANTITY,
                                            fllwupboxrslt_seqno)

                ActivityInfoTableAdapter.InserActHisFllwTotalHis(dlrcd, fllwstrcd, ActHisFllwrw.INSDID, actdateDt, ActHisAssessmentrw.CATEGORYID.ToString(CultureInfo.CurrentCulture()),
                                                                       ActHisAssessmentCategorydvsid, ActHisFllwrw.VIN, ActHisFllwrw.SERIESNAME,
                                                                       ActHisFllwrw.CUSTCHRGSTAFFNM, ActHisFllwrw.CRCUSTID, ActHisFllwrw.CUSTOMERCLASS)
            End If

            '見積り実績確認
            For j = 0 To valuationary.Length - 2
                If selcarary(i) = valuationary(j) Then
                    ActivityInfoTableAdapter.InserActHisFllwCrHis(dlrcd, fllwstrcd, fllwupboxseqno, ActHisFllwCrplan_id, ActHisFllwrw.BFAFDVS,
                                                ActHisFllwrw.CRDVSID, ActHisFllwrw.INSDID, ActHisFllwrw.SERIESCODE, ActHisFllwrw.SERIESNAME,
                                                account, ActHisFllwrw.VCLREGNO, ActHisFllwrw.SUBCTGCODE, ActHisFllwrw.SERVICECD,
                                                ActHisFllwrw.SUBCTGORGNAME, ActHisFllwrw.SUBCTGORGNAME_EX, ActHisFllwPromotion_id,
                                                ActHisFllwrw.CRACTRESULT, ActHisFllwrw.PLANDVS, actdateDt, ActHisValuationrw.METHOD,
                                                ActHisValuationrw.ACTION, ActHisValuationrw.ACTIONTYPE, ActHisFllwrw.ACCOUNT_PLAN,
                                                ActHisValuationrw.ACTIONCD, CONTENT_SEQ_VALUATION, Long.Parse(selcarary(i), CultureInfo.CurrentCulture()), ActHisSelCarrw.SERIESNM,
                                                ActHisSelCaVclmodel_Name, ActHisSelCardisp_Bdy_Color, ActHisSelCarrw.QUANTITY,
                                                fllwupboxrslt_seqno)

                    ActivityInfoTableAdapter.InserActHisFllwTotalHis(dlrcd, fllwstrcd, ActHisFllwrw.INSDID, actdateDt, ActHisValuationrw.CATEGORYID.ToString(CultureInfo.CurrentCulture()),
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
    ''' 活動結果更新処理
    ''' </summary>
    ''' <param name="registdt">データテーブル (インプット)</param>
    ''' <param name="programId">更新プログラムID</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Shared Function UpdateActivityData(ByVal registdt As ActivityInfoDataSet.ActivityInfoRegistDataDataTable, _
                                       ByVal programId As String) As Boolean

        '--デバッグログ---------------------------------------------------
        Logger.Debug("UpdateActivityData Start")
        '-----------------------------------------------------------------

        Dim RegistRw As ActivityInfoDataSet.ActivityInfoRegistDataRow
        RegistRw = CType(registdt.Rows(0), ActivityInfoDataSet.ActivityInfoRegistDataRow)

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
        cstkind = RegistRw.CSTKIND

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
        Dim seqdt As ActivityInfoSeqDataTable
        Dim seqrw As ActivityInfoSeqRow

        '全希望車種のSEQのリストを作成
        Dim selcar As String = ""
        seqdt = ActivityInfoTableAdapter.GetActHisCarSeq(dlrcd, fllwstrcd, fllwupbox_seqno)
        For j = 0 To seqdt.Count - 1
            seqrw = CType(seqdt.Rows(j), ActivityInfoSeqRow)
            selcar = selcar & seqrw.SEQNO & ","
        Next

        'カタログ実績がある希望車種のSEQのリストを作成
        catalog = ""
        wkary = RegistRw.SELECTACTCATALOG.Split(";"c)
        For i = 0 To wkary.Length - 2
            tempary = wkary(i).Split(","c)
            If String.Equals(tempary(1), "1") Then
                seqdt = ActivityInfoTableAdapter.GetActHisSelCarSeq(dlrcd, fllwstrcd, fllwupbox_seqno, tempary(0), "1")
                For j = 0 To seqdt.Count - 1
                    seqrw = CType(seqdt.Rows(j), ActivityInfoSeqRow)
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
                seqdt = ActivityInfoTableAdapter.GetActHisSelCarSeq(dlrcd, fllwstrcd, fllwupbox_seqno, tempary(0), "2")
                For j = 0 To seqdt.Count - 1
                    seqrw = CType(seqdt.Rows(j), ActivityInfoSeqRow)
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
                seqdt = ActivityInfoTableAdapter.GetActHisSelCarSeq(dlrcd, fllwstrcd, fllwupbox_seqno, tempary(0), "4")
                For j = 0 To seqdt.Count - 1
                    seqrw = CType(seqdt.Rows(j), ActivityInfoSeqRow)
                    valuation = valuation & seqrw.SEQNO & ","
                Next
            End If
        Next

        'Follow-up Boxを取得
        Dim fllwboxdt As ActivityInfoDataSet.ActivityInfoFllwupBoxDataTable
        Dim fllwboxrw As ActivityInfoDataSet.ActivityInfoFllwupBoxRow
        fllwboxdt = ActivityInfoTableAdapter.GetFllwupBox(dlrcd, fllwstrcd, fllwupbox_seqno)
        fllwboxrw = CType(fllwboxdt.Rows(0), ActivityInfoFllwupBoxRow)
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
        fllwuptyp = GetFllwupBoxType(fllwboxrw.CRACTRESULT, fllwpromotion_id, fllwboxrw.CRACTCATEGORY, fllwboxrw.REQCATEGORY)

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
        Dim sequencedt As ActivityInfoDataSet.ActivityInfoSequenceDataTable
        Dim sequencerw As ActivityInfoDataSet.ActivityInfoSequenceRow

        'Follow-up Boxを更新
        ActivityInfoTableAdapter.UpdateFllwupbox(thistime_cractresult, strselecteddvs, cractlimitdate, nextactdate, fllwboxrw.CRDVSID, account,
                                 thistime_cractstatus, dlrcd, fllwstrcd, fllwupbox_seqno, fllwuptyp, appointtimeflg)

        'Follow-up Box結果を取得
        Dim fllwrsltdt As ActivityInfoDataSet.ActivityInfoFllwupboxRsltDataTable
        Dim fllwrsltrw As ActivityInfoDataSet.ActivityInfoFllwupboxRsltRow
        fllwrsltdt = ActivityInfoTableAdapter.GetFllwupboxRslt(dlrcd, fllwstrcd, fllwupbox_seqno)
        fllwrsltrw = CType(fllwrsltdt.Rows(0), ActivityInfoFllwupboxRsltRow)

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

        Dim compdt As ActivityInfoDataSet.ActivityInfoCompetitionDataTable
        Dim comprw As ActivityInfoDataSet.ActivityInfoCompetitionRow

        compdt = ActivityInfoTableAdapter.GetCompetition(strpurchasedmakerno, strpurchasedmodelcd)
        If compdt.Count > 0 Then
            comprw = CType(compdt.Rows(0), ActivityInfoCompetitionRow)
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

        ActivityInfoTableAdapter.InsertFllwupboxRslt(fllwboxrw.MEMKIND, thistime_cractresult, stractualtime_start, strcalltime, actdayto,
                                                           fllwboxrw.INSURANCEFLG, memo, strpurchasedmakername, strpurchasedmodelcd, strpurchasedmodelname,
                                                           dlrcd, fllwupbox_seqno.ToString(CultureInfo.CurrentCulture()), fllwrsltrw.SEQNO.ToString(CultureInfo.CurrentCulture()), fllwcrplan_id, fllwboxrw.BFAFDVS, fllwboxrw.CRDVSID.ToString(CultureInfo.CurrentCulture()),
                                                           fllwboxrw.PLANDVS, fllwboxrw.INSDID, fllwboxrw.UNTRADEDCSTID, fllwboxrw.VIN, crrsltid, account,
                                                           talkingtime.ToString(CultureInfo.CurrentCulture()), fllwboxrw.SUBCTGCODE, fllwpromotion_id, successdate, strsuccesskind, fllwboxrw.SERIESCODE,
                                                           fllwboxrw.SERIESNAME, giveupdate, CType(context.OpeCD, String), fllwboxrw.SERVICECD, cractname,
                                                           fllwboxrw.SUBCTGORGNAME_EX, strstall_reserveid, strstall_dlrcd, strstall_strcd, fllwstrcd, strrecid,
                                                           strlogicflg, strcmshislinkid, thistime_cractstatus, fllwboxrw.CRACTSTATUS, strpurchasedmakerno,
                                                           crcustid, customerclass, fllwuptyp, appointtimeflg, Long.Parse(RegistRw.ACTCONTACT, CultureInfo.CurrentCulture()), RegistRw.ACTDAYFROM, actdayto,
                                                           stractualtime_end, fllwboxrw.ACCOUNT_PLAN, actaccount, context.BrnCD, walkinNum)

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

        ActivityInfoTableAdapter.InsertFllwupboxCRHisRslt(fllwboxrw.INSURANCEFLG, stractualtime_start, thistime_cractresult, strselecteddvs, dlrcd, fllwstrcd,
                                                                fllwupbox_seqno, fllwcrplan_id, fllwboxrw.BFAFDVS, fllwboxrw.CRDVSID.ToString(CultureInfo.CurrentCulture()), fllwboxrw.INSDID, fllwboxrw.SERIESCODE,
                                                                fllwboxrw.SERIESNAME, context.Account, fllwboxrw.VCLREGNO, fllwboxrw.SUBCTGCODE, fllwpromotion_id, crrsltid,
                                                                fllwboxrw.PLANDVS, actdate.Substring(0, 10), action, context.Account, fllwboxrw.SERVICECD, cractname,
                                                                fllwboxrw.SUBCTGORGNAME_EX, strstall_reserveid, strstall_dlrcd, strstall_strcd, strrecid, strcmshislinkid,
                                                                fllwrsltrw.SEQNO, actdayto)

        'Follow-up Box活動更新を存在確認
        Dim fllwentrdt As ActivityInfoDataSet.ActivityInfoCountDataTable
        fllwentrdt = ActivityInfoTableAdapter.GetFllwupboxEntry(dlrcd, fllwstrcd, fllwupbox_seqno)

        If fllwentrdt.Count > 0 Then
            'Follow-up Box活動更新を更新
            ActivityInfoTableAdapter.UpdateFllwupboxEntry(context.DlrCD, fllwstrcd, fllwupbox_seqno, context.Account)
        Else
            'Follow-up Box活動更新を登録
            ActivityInfoTableAdapter.InsertFllwupboxEntry(context.DlrCD, fllwstrcd, fllwupbox_seqno, context.Account)
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
            ActivityInfoTableAdapter.InsertCustMemohis(customerclass, cractname, vclinforegistflg, cstkind, crcustid, context.DlrCD,
                                                             context.BrnCD, context.Account, memo, fllwboxrw.INSDID, fllwboxrw.VIN)
        End If

        'Follow-up Box活動実施のカテゴリ取得
        Dim doneCategory As String
        doneCategory = GetFllwupDoneCategory(fllwuptyp)

        'Follow-up Box活動実施の存在確認
        Dim fllwdndt As ActivityInfoDataSet.ActivityInfoFllwupboxrsltDoneDataTable
        Dim fllwdnrw As ActivityInfoDataSet.ActivityInfoFllwupboxrsltDoneRow
        fllwdndt = ActivityInfoTableAdapter.GetFllwupboxrsltDone(dlrcd, fllwstrcd, fllwupbox_seqno, doneCategory)

        If fllwdndt.Count = 0 Then
            Select Case fllwuptyp
                Case C_FLLWUP_HOT
                    If thistime_cractresult <> C_CRACTRSLT_PROSPECT Then
                        'Follow-up Box活動実施の登録
                        ActivityInfoTableAdapter.InsertFllwupboxrsltDone(dlrcd, fllwstrcd, fllwupbox_seqno, doneCategory, actendflg,
                                                                               strcd, fllwboxrw.ACCOUNT_PLAN, C_SALESSTAFFOPECD, account)
                    End If
                Case C_FLLWUP_PROSPECT
                    If thistime_cractresult <> C_CRACTRSLT_HOT Then
                        'Follow-up Box活動実施の登録
                        ActivityInfoTableAdapter.InsertFllwupboxrsltDone(dlrcd, fllwstrcd, fllwupbox_seqno, doneCategory, actendflg,
                                                                               strcd, fllwboxrw.ACCOUNT_PLAN, C_SALESSTAFFOPECD, account)
                    End If
                Case Else
                    'Follow-up Box活動実施の登録
                    ActivityInfoTableAdapter.InsertFllwupboxrsltDone(dlrcd, fllwstrcd, fllwupbox_seqno, doneCategory, actendflg, strcd,
                                                                           fllwboxrw.ACCOUNT_PLAN, C_SALESSTAFFOPECD, account)
            End Select
        Else
            fllwdnrw = CType(fllwdndt.Rows(0), ActivityInfoFllwupboxrsltDoneRow)
            If String.Equals(fllwdnrw.ACTENDFLG, "0") And thistime_cractresult <> C_CRACTRSLT_CONTINUE Then
                'Follow-up Box活動実施の更新
                ActivityInfoTableAdapter.UpdateFllwupboxrsltDone(dlrcd, fllwstrcd, fllwupbox_seqno, account, doneCategory, actendflg)
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

        ActivityInfoTableAdapter.InsertTotalHisRslt(fllwboxrw.INSDID, dlrcd, fllwstrcd, fllwboxrw.UNTRADEDCSTID, vin, seriesname, totalstatus,
                                                          fllwpromotion_id, cractname, actaccount)

        'TBL_FLREQUEST追加
        ActivityInfoTableAdapter.UpdateFLRequest(thistime_cractresult, cractlimitdate, nextactdate, fllwboxrw.INSURANCEFLG, fllwboxrw.SERIESCODE,
                                                       strsuccesskind, fllwboxrw.SERIESNAME, actdate, memo, dlrcd, fllwstrcd, fllwupbox_seqno, fllwuptyp,
                                                       giveupdate)

        '未取引客で活動結果がSuccessかGive-upの場合InReserveInfoに登録
        If String.Equals(cstkind, "2") And (String.Equals(actresult, C_RSLT_SUCCESS) Or String.Equals(actresult, C_RSLT_GIVEUP)) Then
            '052.非活動対象要件情報SeqNo取得
            Dim inreserveidwk As Long
            sequencedt = ActivityInfoTableAdapter.GetSeqInreserveInfoInreserveId()
            sequencerw = CType(sequencedt.Rows(0), ActivityInfoSequenceRow)
            inreserveidwk = sequencerw.SEQ

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
            ActivityInfoTableAdapter.InsertInreserveInfo(inreserveid, dlrcd, strcd, account, fllwupbox_seqno, fllwboxrw.REQUESTID,
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
                    Dim carnumdt As ActivityInfoDataSet.ActivityInfoSelectedCarNumDataTable
                    Dim carnumrw As ActivityInfoDataSet.ActivityInfoSelectedCarNumRow
                    carnumdt = ActivityInfoTableAdapter.GetSelectedCarNum(dlrcd, fllwstrcd, fllwupbox_seqno, successcararywk(0))
                    carnumrw = CType(carnumdt.Rows(0), ActivityInfoSelectedCarNumRow)

                    For j = 1 To carnumrw.QUANTITY
                        'Follow-up Box成約車種の登録
                        ActivityInfoTableAdapter.InsertFllwupboxSuccessSeries(cnt, account, dlrcd, fllwstrcd, fllwupbox_seqno, successcararywk(0))
                        cnt = cnt + 1
                    Next
                End If
            Next
        End If

        'その他計画(Follow-up)の存在確認
        Dim othrplndt As ActivityInfoDataSet.ActivityInfoCountDataTable
        othrplndt = ActivityInfoTableAdapter.GetOtherPlanFllw(dlrcd, fllwstrcd, fllwupbox_seqno)

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
            ActivityInfoTableAdapter.InsertOtherPlanFllwRslt(categorywk, crrsltid, planstatus, categorywk, actDayToDate, strcd,
                                                                   actaccount, planstatus, programId, account, programId, account,
                                                                   dlrcd, fllwstrcd, fllwupbox_seqno)
        Else
            'その他計画(Follow-up)の更新
            ActivityInfoTableAdapter.UpdateOtherPlanFllwRslt(planstatus, cractresult, fllwboxrw.CRACTRESULT, cractresultflg, categorywk,
                                                                   crenddate, crrsltid, fllwboxrw.CRDVSID, strcd, actaccount, account, nextactdate,
                                                                   appointtimeflg, programId, dlrcd, fllwstrcd, fllwupbox_seqno)
        End If

        '希望車種に対する活動実績を入力する
        If RegistRw.PROCESSFLG.Equals("1") Then
            InsertHistory(dlrcd, fllwstrcd, fllwupbox_seqno, selcar, catalog, testdrive, assessment, valuation, account, actdayto)
        End If

        '031.Follow-up Box商談メモ追加
        ActivityInfoTableAdapter.InsertFllwupboxSalesmemo(dlrcd, fllwstrcd, fllwupbox_seqno, cstkind, customerclass, crcustid, account, programId)

        '055.Follow-up Box商談メモWK削除
        ActivityInfoTableAdapter.DeleteFllwupboxSalesmemowk(dlrcd, fllwstrcd, fllwupbox_seqno)

        '--デバッグログ---------------------------------------------------
        Logger.Debug("UpdateActivityData End")
        '-----------------------------------------------------------------
        Return True


    End Function

    ''' <summary>
    ''' tbl_FLLWUPBOXRSLT_DONEで使用するカテゴリの設定
    ''' </summary>
    ''' <param name="fllwuptyp"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetFllwupDoneCategory(ByVal fllwuptyp As String) As String
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
    ''' Follow-up Box種別取得
    ''' </summary>
    ''' <param name="cractresult"></param>
    ''' <param name="promotionid"></param>
    ''' <param name="cractcategory"></param>
    ''' <param name="reqcategory"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetFllwupBoxType(ByVal cractresult As String, ByVal promotionid As Nullable(Of Long), ByVal cractcategory As String,
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
    ''' ToDoリスト登録
    ''' </summary>
    ''' <param name="registdt">データテーブル (インプット)</param>
    ''' <param name="fllwstatus">CR活動ステータス</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Shared Function SetToDo(ByVal registdt As ActivityInfoDataSet.ActivityInfoRegistDataDataTable, ByVal fllwstatus As String) As Boolean

        '--デバッグログ---------------------------------------------------
        Logger.Debug("SetToDo Start")
        '-----------------------------------------------------------------

        'ログインユーザー情報取得用
        Dim context As StaffContext = StaffContext.Current
        Dim sysEnv As New SystemEnvSetting
        Dim sysEnvRow As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow
        sysEnvRow = sysEnv.GetSystemEnvSetting(CONTENT_KEISYO_ZENGO)
        Dim nmtitledt As ActivityInfoNameTitleDataTable
        Dim RegistRw As ActivityInfoDataSet.ActivityInfoRegistDataRow
        RegistRw = CType(registdt.Rows(0), ActivityInfoRegistDataRow)
        If String.Equals(RegistRw.CSTKIND, "1") Then
            nmtitledt = ActivityInfoTableAdapter.GetOrgNameTitle(RegistRw.INSDID)
        Else
            nmtitledt = ActivityInfoTableAdapter.GetNewNameTitle(RegistRw.INSDID)
        End If
        Dim nmtitlerw As ActivityInfoNameTitleRow
        nmtitlerw = CType(nmtitledt.Rows(0), ActivityInfoNameTitleRow)
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
                If String.IsNullOrEmpty(fllwstatus) Then
                    sendObj.CompleteFlg = "1"
                Else
                    sendObj.CompleteFlg = "2"
                End If
                If String.Equals(RegistRw.CSTKIND, "1") Then
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
                Dim cntnmdt As ActivityInfoGetContactNmDataTable
                Dim cntnmrw As ActivityInfoGetContactNmRow
                Dim clrdt As ActivityInfoTodoColorDataTable
                Dim clrrw As ActivityInfoTodoColorRow

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
                    cntnmdt = ActivityInfoTableAdapter.GetContactNM(Long.Parse(RegistRw.FOLLOWCONTACT, CultureInfo.CurrentCulture()))
                    cntnmrw = CType(cntnmdt.Rows(0), ActivityInfoGetContactNmRow)
                    sendObj.ContactName(0) = cntnmrw.CONTACT
                    sendObj.ComingFollowName(0) = WebWordUtility.GetWord("SCHEDULE", 1)
                    '色取得
                    clrdt = ActivityInfoTableAdapter.GetToDoColor("XXXXX", "1", "0", "1", Long.Parse(RegistRw.FOLLOWCONTACT, CultureInfo.CurrentCulture()))
                    clrrw = CType(clrdt.Rows(0), ActivityInfoTodoColorRow)
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
                    cntnmdt = ActivityInfoTableAdapter.GetContactNM(Long.Parse(RegistRw.NEXTACTCONTACT, CultureInfo.CurrentCulture()))
                    cntnmrw = CType(cntnmdt.Rows(0), ActivityInfoGetContactNmRow)
                    sendObj.ContactName(1) = cntnmrw.CONTACT
                    '色取得
                    clrdt = ActivityInfoTableAdapter.GetToDoColor("XXXXX", "1", "0", "0", Long.Parse(RegistRw.NEXTACTCONTACT, CultureInfo.CurrentCulture()))
                    clrrw = CType(clrdt.Rows(0), ActivityInfoTodoColorRow)
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
                    cntnmdt = ActivityInfoTableAdapter.GetContactNM(Long.Parse(RegistRw.NEXTACTCONTACT, CultureInfo.CurrentCulture()))
                    cntnmrw = CType(cntnmdt.Rows(0), ActivityInfoGetContactNmRow)
                    sendObj.ContactName(0) = cntnmrw.CONTACT
                    '色取得
                    clrdt = ActivityInfoTableAdapter.GetToDoColor("XXXXX", "1", "0", "0", Long.Parse(RegistRw.NEXTACTCONTACT, CultureInfo.CurrentCulture()))
                    clrrw = CType(clrdt.Rows(0), ActivityInfoTodoColorRow)
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
    ''' Follow-up Box商談 を更新
    ''' </summary>
    ''' <param name="dlrCD"></param>
    ''' <param name="strCD"></param>
    ''' <param name="fllwupbox_seqno"></param>
    ''' <param name="actualaccount"></param>
    ''' <param name="salesstarttime"></param>
    ''' <param name="salesendtime"></param>
    ''' <param name="account"></param>
    ''' <param name="updateid"></param>
    ''' <returns>処理件数</returns>
    ''' <remarks></remarks>
    Public Shared Function UpdateFllwupboxSales(ByVal dlrcd As String, _
                            ByVal strcd As String, _
                            ByVal fllwupbox_seqno As Long, _
                            ByVal actualaccount As String, _
                            ByVal salesstarttime As Date, _
                            ByVal salesendtime As Date, _
                            ByVal account As String, _
                            ByVal updateid As String) As Integer

        'FLLWUPBOX 商談を更新
        Return ActivityInfoTableAdapter.UpdateFllwupboxSales(dlrcd, _
                                                            strcd, _
                                                            fllwupbox_seqno, _
                                                            actualaccount, _
                                                            salesstarttime, _
                                                            salesendtime, _
                                                            account, _
                                                            updateid)

    End Function

    ''' <summary>
    ''' 最大の活動終了時間を取得
    ''' </summary>
    ''' <param name="dlrCD"></param>
    ''' <param name="strCD"></param>
    ''' <param name="fllwupboxSeqNo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetLatestActTimeEnd(dlrCD As String, strCD As String, fllwupboxSeqNo As Long) As ActivityInfoDataSet.ActivityInfoLatestActTimeDataTable
        Return ActivityInfoTableAdapter.GetLatestActTimeEnd(dlrCD, strCD, fllwupboxSeqNo)
    End Function

    ''' <summary>
    ''' 希望車種リスト取得
    ''' </summary>
    ''' <param name="datatableFrom">
    ''' [DataTable]
    ''' DLRCD:販売店コード
    ''' STRCD:店舗コード
    ''' CNTCD:国コード
    ''' FLLWUPBOX_SEQNO：FollowupboxシーケンスNo
    ''' </param>
    ''' <returns>
    ''' [DataTable]
    ''' SERIESCD:シリーズコード
    ''' SERIESNM:シリーズ名
    ''' MODELCD:モデルコード
    ''' VCLMODEL_NAME:モデル名
    ''' COLORCD:カラーコード
    ''' DISP_BDY_COLOR:カラー名
    ''' PICIMAGE:モデル写真
    ''' LOGOIMAGE:モデルロゴ
    ''' QUANTITY:台数
    ''' SEQNO:希望車種シーケンスNo
    ''' </returns>
    ''' <remarks></remarks>
    Public Shared Function GetSelectedSeriesList(ByVal datatableFrom As ActivityInfoDataSet.ActivityInfoGetSelectedSeriesListFromDataTable) As ActivityInfoDataSet.ActivityInfoGetSelectedSeriesListToDataTable

        Logger.Debug("GetSelectedSeriesList Start")

        ' 変数
        Dim dlrcd As String                         '販売店コード
        Dim strcd As String                         '店舗コード
        Dim cntcd As String                         '国コード
        Dim fllwupbox_seqno As Long                 'FollowupBox連番
        Dim datatableSelectedSeries As ActivityInfoDataSet.ActivityInfoGetSelectedSeriesDataTable
        Dim datarowFrom As ActivityInfoDataSet.ActivityInfoGetSelectedSeriesListFromRow

        ' 引数取得
        datarowFrom = datatableFrom.Rows(0)
        dlrcd = datarowFrom.DLRCD
        strcd = datarowFrom.STRCD
        cntcd = datarowFrom.CNTCD
        fllwupbox_seqno = datarowFrom.FLLWUPBOX_SEQNO

        ' 希望車種取得
        datatableSelectedSeries =
            ActivityInfoTableAdapter.GetSelectedSeries(dlrcd, strcd, cntcd, fllwupbox_seqno)

        ' DataTableに格納
        Using datatableTo As New ActivityInfoDataSet.ActivityInfoGetSelectedSeriesListToDataTable
            For Each dt As ActivityInfoDataSet.ActivityInfoGetSelectedSeriesRow In datatableSelectedSeries
                Dim datarowTo As ActivityInfoDataSet.ActivityInfoGetSelectedSeriesListToRow
                datarowTo = datatableTo.NewActivityInfoGetSelectedSeriesListToRow
                datarowTo.SERIESCD = dt.SERIESCD
                datarowTo.SERIESNM = dt.SERIESNM
                If dt.IsMODELCDNull Then
                    datarowTo.MODELCD = String.Empty
                Else
                    datarowTo.MODELCD = dt.MODELCD
                End If
                If dt.IsVCLMODEL_NAMENull Then
                    datarowTo.VCLMODEL_NAME = String.Empty
                Else
                    datarowTo.VCLMODEL_NAME = dt.VCLMODEL_NAME
                End If
                If dt.IsCOLORCDNull Then
                    datarowTo.COLORCD = String.Empty
                Else
                    datarowTo.COLORCD = dt.COLORCD
                End If
                If dt.IsDISP_BDY_COLORNull Then
                    datarowTo.DISP_BDY_COLOR = String.Empty
                Else
                    datarowTo.DISP_BDY_COLOR = dt.DISP_BDY_COLOR
                End If
                If dt.IsPICIMAGENull Then
                    datarowTo.PICIMAGE = String.Empty
                Else
                    datarowTo.PICIMAGE = dt.PICIMAGE
                End If
                If dt.IsLOGOIMAGENull Then
                    datarowTo.LOGOIMAGE = String.Empty
                Else
                    datarowTo.LOGOIMAGE = dt.LOGOIMAGE
                End If
                datarowTo.QUANTITY = dt.QUANTITY
                datarowTo.SEQNO = dt.SEQNO
                datatableTo.Rows.Add(datarowTo)
            Next

            Return datatableTo
        End Using

        Logger.Debug("GetSelectedSeriesList End")

    End Function

    ''' <summary>
    ''' 成約車種リスト取得
    ''' </summary>
    ''' <param name="datatableFrom">
    ''' [DataTable]
    ''' DLRCD:販売店コード
    ''' STRCD:店舗コード
    ''' FLLWUPBOX_SEQNO：FollowupboxシーケンスNo
    ''' </param>
    ''' <returns>
    ''' [DataTable]
    ''' SERIESCD:シリーズコード
    ''' SERIESNM:シリーズ名
    ''' MODELCD:モデルコード
    ''' VCLMODEL_NAME:モデル名
    ''' COLORCD:カラーコード
    ''' DISP_BDY_COLOR:カラー名
    ''' PICIMAGE:モデル写真
    ''' LOGOIMAGE:モデルロゴ
    ''' QUANTITY:台数
    ''' SEQNO:希望車種シーケンスNo
    ''' </returns>
    ''' <remarks></remarks>
    Public Shared Function GetSuccessSeriesList(ByVal datatableFrom As ActivityInfoDataSet.ActivityInfoGetSelectedSeriesListFromDataTable) As ActivityInfoDataSet.ActivityInfoGetSelectedSeriesListToDataTable
        Logger.Debug("GetSuccessSeriesList Start")

        ' 変数
        Dim dlrcd As String                         '販売店コード
        Dim strcd As String                         '店舗コード
        Dim fllwupbox_seqno As Long                 'FollowupBox連番
        Dim datatableSelectedSeries As ActivityInfoDataSet.ActivityInfoGetSelectedSeriesDataTable
        Dim datarowFrom As ActivityInfoDataSet.ActivityInfoGetSelectedSeriesListFromRow

        ' 引数取得
        datarowFrom = datatableFrom.Rows(0)
        dlrcd = datarowFrom.DLRCD
        strcd = datarowFrom.STRCD
        fllwupbox_seqno = datarowFrom.FLLWUPBOX_SEQNO

        ' 成約車種取得
        datatableSelectedSeries =
            ActivityInfoTableAdapter.GetSuccessSeries(dlrcd, strcd, fllwupbox_seqno)

        ' DataTableに格納
        Using datatableTo As New ActivityInfoDataSet.ActivityInfoGetSelectedSeriesListToDataTable
            For Each dt As ActivityInfoDataSet.ActivityInfoGetSelectedSeriesRow In datatableSelectedSeries
                Dim datarowTo As ActivityInfoDataSet.ActivityInfoGetSelectedSeriesListToRow
                datarowTo = datatableTo.NewActivityInfoGetSelectedSeriesListToRow
                datarowTo.SERIESCD = dt.SERIESCD
                datarowTo.SERIESNM = dt.SERIESNM
                If dt.IsMODELCDNull Then
                    datarowTo.MODELCD = String.Empty
                Else
                    datarowTo.MODELCD = dt.MODELCD
                End If
                If dt.IsVCLMODEL_NAMENull Then
                    datarowTo.VCLMODEL_NAME = String.Empty
                Else
                    datarowTo.VCLMODEL_NAME = dt.VCLMODEL_NAME
                End If
                If dt.IsCOLORCDNull Then
                    datarowTo.COLORCD = String.Empty
                Else
                    datarowTo.COLORCD = dt.COLORCD
                End If
                If dt.IsDISP_BDY_COLORNull Then
                    datarowTo.DISP_BDY_COLOR = String.Empty
                Else
                    datarowTo.DISP_BDY_COLOR = dt.DISP_BDY_COLOR
                End If
                If dt.IsPICIMAGENull Then
                    datarowTo.PICIMAGE = String.Empty
                Else
                    datarowTo.PICIMAGE = dt.PICIMAGE
                End If
                If dt.IsLOGOIMAGENull Then
                    datarowTo.LOGOIMAGE = String.Empty
                Else
                    datarowTo.LOGOIMAGE = dt.LOGOIMAGE
                End If
                datarowTo.QUANTITY = dt.QUANTITY
                datarowTo.SEQNO = dt.SEQNO
                datatableTo.Rows.Add(datarowTo)
            Next

            Return datatableTo
        End Using

        Logger.Debug("GetSuccessSeriesList End")
    End Function

    ''' <summary>
    ''' プロセス取得
    ''' </summary>
    ''' <param name="datatableFrom">
    ''' [DataTable]
    ''' DLRCD:販売店コード
    ''' STRCD:店舗コード
    ''' FLLWUPBOX_SEQNO：FollowupboxシーケンスNo
    ''' </param>
    ''' <returns>
    ''' [DataTable]
    ''' SEQNO:希望車種シーケンスNo
    ''' CATALOGDATE:カタログ実施日
    ''' TESTDRIVEDATE:試乗実施日
    ''' EVALUATIONDATE:査定実施日
    ''' QUOTATIONDATE:見積実施日
    ''' SALESBKGDATE:受注日
    ''' VCLASIDATE:振当日
    ''' SALESDATE:入金日
    ''' VCLDELIDATE:納車日
    ''' </returns>
    ''' <remarks></remarks>
    Public Shared Function GetProcess(ByVal datatableFrom As ActivityInfoDataSet.ActivityInfoGetProcessFromDataTable) As ActivityInfoDataSet.ActivityInfoGetProcessToDataTable
        Logger.Debug("GetProcess Start")

        ' 変数
        Dim dlrcd As String                         '販売店コード
        Dim strcd As String                         '店舗コード
        Dim fllwupbox_seqno As Long                 'FollowupBox連番
        Dim salesbkgno As String                    '受注No
        Dim datatableProcess As ActivityInfoDataSet.ActivityInfoGetProcessDataTable
        Dim datatableProcessAfter As ActivityInfoDataSet.ActivityInfoGetProcessAfterDataTable
        Dim datarowProcess As ActivityInfoDataSet.ActivityInfoGetProcessRow
        Dim datarowFrom As ActivityInfoDataSet.ActivityInfoGetProcessFromRow
        Dim tempSeqno As Long

        ' 引数取得
        datarowFrom = datatableFrom.Rows(0)
        dlrcd = datarowFrom.DLRCD
        strcd = datarowFrom.STRCD
        fllwupbox_seqno = datarowFrom.FLLWUPBOX_SEQNO
        salesbkgno = datarowFrom.SALESBKGNO

        ' プロセス取得
        datatableProcess = ActivityInfoTableAdapter.GetProcess(dlrcd, strcd, fllwupbox_seqno)
        ' 受注後プロセス取得
        If String.IsNullOrEmpty(salesbkgno) Then
            datatableProcessAfter = Nothing
        Else
            datatableProcessAfter = ActivityInfoTableAdapter.GetProcessAfter(dlrcd, salesbkgno)
        End If

        ' DataTableに格納
        If datatableProcess.Rows.Count > 0 Then
            datarowProcess = CType(datatableProcess.Rows(0), ActivityInfoDataSet.ActivityInfoGetProcessRow)
            tempSeqno = datarowProcess.SEQNO
        End If

        Using datatableTo As New ActivityInfoDataSet.ActivityInfoGetProcessToDataTable
            Dim datarowTo As ActivityInfoDataSet.ActivityInfoGetProcessToRow
            datarowTo = datatableTo.NewActivityInfoGetProcessToRow
            Dim scDlrCd As String = StaffContext.Current.DlrCD
            For Each dt As ActivityInfoDataSet.ActivityInfoGetProcessRow In datatableProcess
                If tempSeqno <> dt.SEQNO Then
                    datatableTo.Rows.Add(datarowTo)
                    datarowTo = datatableTo.NewActivityInfoGetProcessToRow
                    tempSeqno = dt.SEQNO
                End If

                Select Case dt.ACTIONCD
                    Case ACTIONCD_CATALOG
                        datarowTo.CATALOGDATE = DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, dt.LASTACTDATE, scDlrCd)
                    Case ACTIONCD_TESTDRIVE
                        datarowTo.TESTDRIVEDATE = DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, dt.LASTACTDATE, scDlrCd)
                    Case ACTIONCD_EVALUATION
                        datarowTo.EVALUATIONDATE = DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, dt.LASTACTDATE, scDlrCd)
                    Case ACTIONCD_QUOTATION
                        datarowTo.QUOTATIONDATE = DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, dt.LASTACTDATE, scDlrCd)
                End Select

                datarowTo.SEQNO = dt.SEQNO
            Next

            If datatableProcessAfter Is Nothing Then
                If datatableProcess.Rows.Count > 0 Then
                    datatableTo.Rows.Add(datarowTo)
                End If
            Else
                If datatableProcess.Rows.Count > 0 Or datatableProcessAfter.Rows.Count > 0 Then
                    datatableTo.Rows.Add(datarowTo)
                End If
            End If

            If Not String.IsNullOrEmpty(salesbkgno) Then

                '受注前も、受注後も両方データがなかった場合に追加する
                If datatableTo.Rows.Count = 0 Then
                    datatableTo.Rows.Add(datarowTo)
                End If

                For Each dr As ActivityInfoDataSet.ActivityInfoGetProcessToRow In datatableTo

                    If datatableProcessAfter.Rows.Count > 0 Then
                        If datatableProcessAfter(0).IsSALESBKGDATENull Then
                        Else
                            dr.SALESBKGDATE = DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, datatableProcessAfter(0).SALESBKGDATE, DateTimeFunc.Now, scDlrCd, False)
                        End If
                        If datatableProcessAfter(0).IsVCLASIDATENull Then
                        Else
                            dr.VCLASIDATE = DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, datatableProcessAfter(0).VCLASIDATE, DateTimeFunc.Now, scDlrCd, False)
                        End If
                        If datatableProcessAfter(0).IsSALESDATENull Then
                        Else
                            dr.SALESDATE = DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, datatableProcessAfter(0).SALESDATE, DateTimeFunc.Now, scDlrCd, False)
                        End If
                        If datatableProcessAfter(0).IsVCLDELIDATENull Then
                        Else
                            dr.VCLDELIDATE = DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, datatableProcessAfter(0).VCLDELIDATE, DateTimeFunc.Now, scDlrCd, False)
                        End If
                    Else
                        '注文日が取得できなかった場合
                        If datatableProcessAfter.Rows.Count > 0 Then
                            If (datatableProcessAfter(0).IsSALESBKGDATENull) Then
                                '見積情報テーブル情報取得
                                Dim estimateTbl As New ActivityInfoDataSet.ActivityInfoGetContractDateDataTable
                                estimateTbl = ActivityInfoTableAdapter.GetContractDate(datarowFrom.DLRCD, datarowFrom.STRCD, datarowFrom.FLLWUPBOX_SEQNO)

                                '契約完了日にする
                                If (estimateTbl.Rows.Count > 0) Then
                                    dr.SALESBKGDATE = DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, estimateTbl.Item(0).CONTRACTDATE, scDlrCd)
                                End If
                            End If
                        Else
                            '見積情報テーブル情報取得
                            Dim estimateTbl As New ActivityInfoDataSet.ActivityInfoGetContractDateDataTable
                            estimateTbl = ActivityInfoTableAdapter.GetContractDate(datarowFrom.DLRCD, datarowFrom.STRCD, datarowFrom.FLLWUPBOX_SEQNO)

                            '契約完了日にする
                            If (estimateTbl.Rows.Count > 0) Then
                                dr.SALESBKGDATE = DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, estimateTbl.Item(0).CONTRACTDATE, scDlrCd)
                            End If
                        End If
                    End If
                Next
            End If


            Return datatableTo
        End Using

        Logger.Debug("GetProcess End")
    End Function

    ''' <summary>
    ''' ステータス取得
    ''' </summary>
    ''' <param name="datatableFrom">
    ''' [DataTable]
    ''' DLRCD:販売店コード
    ''' STRCD:店舗コード
    ''' FLLWUPBOX_SEQNO：FollowupboxシーケンスNo
    ''' </param>
    ''' <returns>
    ''' [DataTable]
    ''' CRACTRESULT:活動結果(1:Hot,2:Warm,3:Success,4:Cold,5:Give-up)
    ''' </returns>
    ''' <remarks></remarks>
    Public Shared Function GetStatus(ByVal datatableFrom As ActivityInfoDataSet.ActivityInfoGetStatusFromDataTable) As ActivityInfoDataSet.ActivityInfoGetStatusToDataTable
        Logger.Debug("GetStatus Start")

        ' 変数
        Dim dlrcd As String                         '販売店コード
        Dim strcd As String                         '店舗コード
        Dim fllwupbox_seqno As Long                 'FollowupBox連番
        Dim datatableStatus As ActivityInfoDataSet.ActivityInfoGetStatusDataTable
        Dim datarowFrom As ActivityInfoDataSet.ActivityInfoGetStatusFromRow

        ' 引数取得
        datarowFrom = datatableFrom.Rows(0)
        dlrcd = datarowFrom.DLRCD
        strcd = datarowFrom.STRCD
        fllwupbox_seqno = datarowFrom.FLLWUPBOX_SEQNO

        ' ステータス取得
        datatableStatus = ActivityInfoTableAdapter.GetStatus(dlrcd, strcd, fllwupbox_seqno)

        ' DataTableに格納
        Using datatableTo As New ActivityInfoDataSet.ActivityInfoGetStatusToDataTable
            If datatableStatus.Rows.Count > 0 Then
                Dim datarowTo As ActivityInfoDataSet.ActivityInfoGetStatusToRow
                datarowTo = datatableTo.NewActivityInfoGetStatusToRow
                Dim dt As ActivityInfoDataSet.ActivityInfoGetStatusRow =
                    CType(datatableStatus.Rows(0), ActivityInfoDataSet.ActivityInfoGetStatusRow)
                datarowTo.CRACTRESULT = dt.CRACTRESULT
                datatableTo.Rows.Add(datarowTo)
            End If

            Return datatableTo
        End Using
        Logger.Debug("GetStatus End")
    End Function

    ''' <summary>
    ''' CR活動成功のデータ存在判定
    ''' </summary>
    ''' <param name="datatableFrom">
    ''' [DataTable]
    ''' DLRCD:販売店コード
    ''' STRCD:店舗コード
    ''' FLLWUPBOX_SEQNO：FollowupboxシーケンスNo
    ''' </param>
    ''' <returns>
    ''' 判定結果:(0:受注時,1:受注後)
    ''' </returns>
    ''' <remarks>CR活動成功のデータが存在するか判定</remarks>
    Public Shared Function CountFllwupboxRslt(ByVal datatableFrom As ActivityInfoDataSet.ActivityInfoCountFromDataTable) As String

        Logger.Debug("CountFllwupboxRslt Start")

        Dim dlrcd As String = datatableFrom(0).DLRCD
        Dim strcd As String = datatableFrom(0).STRCD
        Dim fllwupboxseqno As Long = datatableFrom(0).FLLWUPBOX_SEQNO
        Dim rslt As String

        Dim cnt As Integer = ActivityInfoTableAdapter.CountFllwupboxRslt(dlrcd, strcd, fllwupboxseqno, CRACTRESULT_SUCCESS)
        If cnt > 0 Then
            '活動が1件以上存在する
            rslt = SALESAFTER_YES
        Else
            '存在しない
            rslt = SALESAFTER_NO
        End If

        Logger.Debug("CountFllwupboxRslt End")
        Return rslt
    End Function

    ''' <summary>
    ''' 契約書No取得
    ''' </summary>
    ''' <param name="datatableFrom">
    ''' [DataTable]
    ''' DLRCD:販売店コード
    ''' STRCD:店舗コード
    ''' FLLWUPBOX_SEQNO：FollowupboxシーケンスNo
    ''' </param>
    ''' <returns>契約書No</returns>
    ''' <remarks>契約書No取得</remarks>
    Public Shared Function GetContractNo(ByVal datatableFrom As ActivityInfoDataSet.ActivityInfoContractNoFromDataTable) As String

        Logger.Debug("GetContractInfo Start")

        Dim dlrcd As String = datatableFrom(0).DLRCD
        Dim strcd As String = datatableFrom(0).STRCD
        Dim fllwupboxseqno As Long = datatableFrom(0).FLLWUPBOX_SEQNO
        Dim rslt As String

        '契約書Noを取得
        Dim dataSet As ActivityInfoDataSet.ActivityInfoContractNoDataTable =
            ActivityInfoTableAdapter.GetContractNo(dlrcd, strcd, fllwupboxseqno)
        If dataSet.Rows.Count > 0 Then
            If (dataSet(0).IsCONTRACTNONull) Then
                rslt = String.Empty
            Else
                rslt = CStr(dataSet(0).CONTRACTNO)
            End If
        Else
            rslt = String.Empty
        End If

        Logger.Debug("GetContractInfo End")
        Return rslt
    End Function

    ''' <summary>
    ''' 受注後活動状態取得
    ''' </summary>
    ''' <param name="datatableFrom">
    ''' [DataTable]
    ''' DLRCD:販売店コード
    ''' SALESBKGNO:受注No
    ''' </param>
    ''' <returns>WaitingObject(001:振当待ち、002:入金待ち、005:納車待ち、007:納車済)</returns>
    ''' <remarks>受注後活動状態取得</remarks>
    Public Shared Function GetWaitingObject(ByVal datatableFrom As ActivityInfoDataSet.ActivityInfoWaitingObjectFromDataTable) As String

        Logger.Debug("GetContractInfo Start")

        Dim dlrcd As String = datatableFrom(0).DLRCD
        Dim salesbkgno As String = datatableFrom(0).SALESBKGNO
        Dim rslt As String

        '契約書Noを取得
        Dim dataSet As ActivityInfoDataSet.ActivityInfoWaitingObjectDataTable =
            ActivityInfoTableAdapter.GetWaitingObject(dlrcd, salesbkgno)
        If dataSet.Rows.Count > 0 Then
            rslt = CStr(dataSet(0).WAITINGOBJECT)
        Else
            rslt = WAITINGOBJECT_ALLOCATION
        End If

        Logger.Debug("GetContractInfo End")
        Return rslt
    End Function

    ' 2012/02/29 TCS 安田 【SALES_2】 START
    ''' <summary>
    ''' キャンセル区分取得
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="salesbkgno">受注No</param>
    ''' <returns>True:注文キャンセル False:それ以外</returns>
    ''' <remarks>注文キャンセルされているか判定する</remarks>
    Public Shared Function GetSalesCancel(ByVal dlrcd As String,
                                            ByVal salesbkgno As String) As Boolean

        Logger.Debug("GetSalesCancel Start")

        Dim rslt As Boolean = False

        'キャンセル区分取得
        Dim dataSet As ActivityInfoDataSet.ActivityInfoGetCancelStatusDataTable =
            ActivityInfoTableAdapter.GetSalesCancel(dlrcd, salesbkgno)
        If dataSet.Rows.Count > 0 Then
            'キャンセル区分が1:注文キャンセル時
            If (Not dataSet(0).IsCANCELFLGNull AndAlso dataSet(0).CANCELFLG.Equals("1")) Then
                rslt = True
            End If
        End If

        Logger.Debug("GetSalesCancel End")

        Return rslt

    End Function

    ''' <summary>
    ''' 活動中リスト取得
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="strcd">店舗コード</param>
    ''' <param name="insdid">未取引客ID／自社客連番</param>
    ''' <param name="cstkind">未取引客:2／自社客種別:1</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetSalesActiveList(ByVal dlrcd As String,
                                       ByVal strcd As String,
                                       ByVal insdid As String,
                                       ByVal cstkind As String,
                                       ByVal newcustid As String) As ActivityInfoDataSet.ActivityInfoSalesActiveListDataTable

        Logger.Debug("GetSalesActiveList Start")

        Dim dataSet As ActivityInfoDataSet.ActivityInfoSalesActiveListDataTable =
            ActivityInfoTableAdapter.GetSalesActiveList(dlrcd, strcd, insdid, cstkind, newcustid)

        Return dataSet

        Logger.Debug("GetSalesActiveList End")

    End Function
    ' 2012/02/29 TCS 安田 【SALES_2】 END

    ''' <summary>
    ''' 契約日取得
    ''' </summary>
    ''' <param name="datatableFrom">
    ''' [DataTable]
    ''' DLRCD:販売店コード
    ''' STRCD:店舗コード
    ''' FLLWUPBOX_SEQNO：FollowupboxシーケンスNo
    ''' </param>
    ''' <returns>契約日</returns>
    ''' <remarks></remarks>
    Public Shared Function GetEstimateDate(ByVal datatableFrom As ActivityInfoDataSet.ActivityInfoGetContractDateFromDataTable) As ActivityInfoDataSet.ActivityInfoGetContractDateDataTable

        Logger.Debug("GetEstimateDate Start")

        '契約日を取得
        Dim estimateTbl As New ActivityInfoDataSet.ActivityInfoGetContractDateDataTable
        estimateTbl = ActivityInfoTableAdapter.GetContractDate(datatableFrom(0).DLRCD, datatableFrom(0).STRCD, datatableFrom(0).FLLWUPBOX_SEQNO)

        Logger.Debug("GetEstimateDate End")
        Return estimateTbl
    End Function

    ''' <summary>
    ''' 活動方法初期値取得
    ''' </summary>
    ''' <param name="bookedafterflg">受注後フラグ (指定がなければ全件検索)</param>
    ''' <returns>活動方法データテーブル</returns>
    ''' <remarks></remarks>
    Shared Function GetInitActContact(ByVal bookedafterflg As String) As ActivityInfoDataSet.ActivityInfoActContactRow

        Dim contactNo As Integer = Nothing
        Dim dt As ActivityInfoDataSet.ActivityInfoActContactDataTable
        dt = ActivityInfoTableAdapter.GetActContact(bookedafterflg)
        Dim rw As ActivityInfoDataSet.ActivityInfoActContactRow = Nothing

        '現在のステータス
        Dim staffStatus As String = StaffContext.Current.PresenceCategory & StaffContext.Current.PresenceDetail

        '初期選択値を探す
        For Each dr As ActivityInfoDataSet.ActivityInfoActContactRow In dt.Rows
            If String.Equals(staffStatus, STAFF_STATUS_NEGOTIATION) AndAlso String.Equals(dr.FIRSTSELECT_WALKIN, "1") Then
                '商談中の場合、初期選択(商談)のレコードを探す
                rw = dr
                Exit For
            ElseIf Not String.Equals(staffStatus, STAFF_STATUS_NEGOTIATION) AndAlso String.Equals(dr.FIRSTSELECT_NOTWALKIN, "1") Then
                '商談中以外の場合、初期選択(営業活動)のレコードを探す
                rw = dr
                Exit For
            End If
        Next

        'Return CStr(rw.CONTACTNO) & "," & rw.PROCESS
        Return rw
    End Function

    ''' <summary>
    ''' 担当スタッフ取得
    ''' </summary>
    ''' <param name="account">アカウント</param>
    ''' <returns>担当スタッフデータテーブル</returns>
    ''' <remarks></remarks>
    Shared Function GetStaff(ByVal account As String) As ActivityInfoDataSet.ActivityInfoUsersDataTable

        Dim context As StaffContext = StaffContext.Current
        Return ActivityInfoTableAdapter.GetStaff(context.DlrCD, context.BrnCD, account)

    End Function

    ''' <summary>
    ''' 今回活動分類タイトル取得
    ''' </summary>
    ''' <param name="contactNo">分類コード</param>
    ''' <returns>活動分類タイトル</returns>
    ''' <remarks></remarks>
    Shared Function GetInitActContactTitle(ByVal contactNo As String) As String

        'Dim contactNo As Integer = Nothing
        Dim dt As ActivityInfoDataSet.ActivityInfoGetContactNmDataTable
        dt = ActivityInfoTableAdapter.GetContactNM(contactNo)
        Dim rw As ActivityInfoDataSet.ActivityInfoGetContactNmRow = Nothing

        rw = dt.Rows(0)

        ''現在のステータス
        'Dim staffStatus As String = StaffContext.Current.PresenceCategory & StaffContext.Current.PresenceDetail
        '
        ''初期選択値を探す
        'For Each dr As ActivityInfoDataSet.ActivityInfoActContactRow In dt.Rows
        '    If String.Equals(staffStatus, STAFF_STATUS_NEGOTIATION) AndAlso String.Equals(dr.FIRSTSELECT_WALKIN, "1") Then
        '        '商談中の場合、初期選択(商談)のレコードを探す
        '        rw = dr
        '        Exit For
        '    ElseIf Not String.Equals(staffStatus, STAFF_STATUS_NEGOTIATION) AndAlso String.Equals(dr.FIRSTSELECT_NOTWALKIN, "1") Then
        '        '商談中以外の場合、初期選択(営業活動)のレコードを探す
        '        rw = dr
        '        Exit For
        '    End If
        'Next

        'Return CStr(rw.CONTACTNO) & "," & rw.PROCESS
        Return rw.CONTACT

    End Function

    '2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 START
    ''' <summary>
    ''' 未取引客個人情報取得
    ''' </summary>
    ''' <param name="custId">未取引客ユーザーID</param>
    ''' <returns>GetNewCustomerDataTable</returns>
    ''' <remarks></remarks>
    Shared Function GetNewcustomer(ByVal custId As String) As ActivityInfoDataSet.GetNewCustomerDataTable
        Dim dt As ActivityInfoDataSet.GetNewCustomerDataTable = ActivityInfoTableAdapter.GetNewCustomer(custId)

        Return dt
    End Function
    '2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 END

#End Region

End Class
