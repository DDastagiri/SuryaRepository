'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3010401BusinessLogic.vb
'─────────────────────────────────────
'機能： TODO一覧 (ビジネスロジック)
'補足： 
'作成： 2012/02/26 TCS 竹内
'更新： 2012/09/28 TCS 渡邊   【SALES_Step3】GTMC120924022の不具合修正
'更新： 2013/01/11 TCS 橋本   【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発
'更新： 2013/06/30 TCS 武田   2013/10対応版　既存流用
'更新： 2014/02/17 TCS 山田   受注後フォロー機能開発
'更新： 2014/07/30 TCS 武田   受注後活動性能改善
'更新： 2015/12/08 TCS 中村   (ﾄﾗｲ店ｼｽﾃﾑ評価)新車ﾀﾌﾞﾚｯﾄ(受注後)の管理機能開発
'更新： 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-3
'更新： 2020/01/06 TS  重松 [TMTレスポンススロー] SLT基盤への横展
'─────────────────────────────────────
Option Explicit On
Option Strict On

Imports System.Web
Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.CustomerInfo.ToDoList.BizLogic
Imports Toyota.eCRB.CustomerInfo.ToDoList.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess.DlrEnvSettingDataSet
Imports System.Threading.Tasks
Imports System.Reflection

Public Class SC3010401BusinessLogic
    Inherits BaseBusinessComponent

#Region " コンストラクタ "
    ''' <summary>
    ''' デフォルトコンストラクタ
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        context = StaffContext.Current     'スタッフ情報
    End Sub
#End Region

#Region "定数"
    ''' <summary>
    ''' 顔写真の保存先フォルダ(Web向け)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const FacePicUploadurl As String = "FACEPIC_UPLOADURL"

    ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
    ''' <summary>
    ''' fllwupbox受注状態：３１
    ''' </summary>
    ''' <remarks></remarks>
    Public Const FllwStatusSuccess As String = "31"
    ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END

    ''' <summary>
    ''' 苦情有無判定日数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const dspComplaintDay As String = "COMPLAINT_DISPLAYDATE"

    Private Const zeroClear As Integer = 0     '0clear

    ''' <summary>
    ''' Caldav情報抽出処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Const caldavDelayData As Integer = 0     '過去データ
    Private Const caldavDueData As Integer = 1       '当日データ
    Private Const caldavFutureData As Integer = 2    '未来データ

    Private Const caldavReserv As String = "0"     '来店予約
    Private Const caldavSuccsess As String = "2"      '受注後工程

    Private Const caldavProcAllocation As String = "001"     '振当て
    Private Const caldavProcPay As String = "002"       '入金
    Private Const caldavProcDeli As String = "005"      '納車

    ''' <summary>
    ''' 注文情報集計時：受注後の状態
    ''' </summary>
    ''' <remarks></remarks>
    Private Const tallyStatusAllocation As String = "001"      '受注状態
    Private Const tallyStatusPayment As String = "002"   '振当状態
    Private Const tallyStatusCarDeli As String = "005"     '入金：Delivery

    ''' <summary>
    ''' 受注後：コンタクトアイコン状態
    ''' </summary>
    ''' <remarks></remarks>
    Private Const contactStatusAlloc As String = "7"  '振当
    Private Const contactStatusPayment As String = "8"  '入金
    Private Const contactStatusDeli As String = "9"  '納車

    ''' <summary>
    ''' コンタクトアイコン取得時KEY
    ''' </summary>
    ''' <remarks></remarks>
    Private Const contactIconKeySuccess As String = "001"      '受注Icon
    Private Const contactIconKeyAllocation As String = "002"   '振当Icon
    Private Const contactIconKeyPayment As String = "005"     '入金Icon

    ''' <summary>
    ''' クレームの有無
    ''' </summary>
    ''' <remarks></remarks>
    Private Const clmExist As String = "1"
    Private Const clmNoExist As String = "0"

    ''' <summary>
    ''' 画面での条件有無
    ''' </summary>
    ''' <remarks></remarks>
    Private Const extractOff = 0        '条件なし
    Private Const extractOn = 1         '条件あり

    ''' <summary>
    ''' 受注後のステイタスを細分化するための値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const sortNoStatusSuccess As Integer = 0
    Private Const sortNoStatusAllocation As Integer = 10
    Private Const sortNoStatusPayment As Integer = 20
    Private Const sortNoStatusDelivery As Integer = 30

    ''' <summary>
    ''' 自社客/motirhiki
    ''' </summary>
    ''' <remarks></remarks>
    Private Const custmerOrg As String = "1"
    Private Const custmerNew As String = "2"

    '   時刻無しデータ
    Private Const NOTIME_FLG As String = "0"
    '   終日データ
    Private Const ALLDAY_FLG As String = "1"

    Private Const DAYSDATA As String = "1"
    Private Const NODAYSDATA As String = "0"

    Private Const STS_SUCCESS As String = "2"
    Private Const STS_PROSPECT As String = "0"

    ' 2014/02/17 TCS 山田 受注後フォロー機能開発 START
    '   受注後予定工程アイコンパス用2NDキー
    Private Const PLAN_AFTER_ODR_PROC As String = "31"
    ' 2014/02/17 TCS 山田 受注後フォロー機能開発 END

    '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 START
    ''' <summary>
    ''' システム設定の指定パラメータ 受注後工程利用フラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_USE_AFTER_ODR_PROC_FLG As String = "USE_AFTER_ODR_PROC_FLG"
    '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 END

#End Region

#Region "Private 変数"
    Private context As StaffContext
#End Region


    ''' <summary>
    ''' データセットインスタンス作成
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CreateDataSet() As SC3010401DataSet

        CreateDataSet = New SC3010401DataSet
        Return (CreateDataSet)

    End Function

    ''' <summary>
    '''検索用共通データテーブルの作成 
    ''' </summary>
    ''' <param name="xmltext">CALDAVからのXML文字列</param>
    ''' <param name="ds">共有データセット</param>
    Public Sub CreateSearch(xmltext As String, ByVal ds As SC3010401DataSet, serchDelay As Integer, serchToday As Integer, serchFuture As Integer)
        Try
            ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "{0}_Start",
                                      MethodBase.GetCurrentMethod.Name))
            ' ======================== ログ出力 終了 ========================
            ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END
            Dim xml As New XmlDocument
            Logger.Info("ClassLibraryBusinessLogic.GetCalender value=" & xmltext)
            xml.LoadXml(xmltext)

            Dim dr As SC3010401DataSet.SC3010401SearchRow
            Dim nowDate As Date = DateTimeFunc.Now(context.DlrCD)   '現在日時
            Dim truncNow As Date = New Date(nowDate.Year, nowDate.Month, nowDate.Day)
            Dim endDate As Date = truncNow.AddDays(1).AddSeconds(-1)
            'Logger.Info("comparison datetime=" & nowDate.ToString)

            For Each detailNode As XmlNode In xml.SelectNodes("//Common")       'XML全文から"Common"タグ分繰り返し
                Dim xDealerCode As String
                Dim xBranchCode As String
                Dim xScheduleID As String
                Dim WSEQ As Short = zeroClear
                Dim xScheduleDiv As String
                Dim xTimeFlg As String          '時刻ありデータかどうか
                Dim xAllDayFlg As String        '終日データかどうか
                Dim wDaysDataFlg As String         '日またぎデータかどうか

                'CALDAVにデータＫＥＹ項目がない場合、処理しない	 かつ、ｽｹｼﾞｭｰﾙ区分:0[来店予約] :2[受注後工程] を対象とする
                If detailNode.SelectNodes("DealerCode").Count >= 1 AndAlso detailNode.SelectNodes("BranchCode").Count >= 1 AndAlso _
                detailNode.SelectNodes("ScheduleID").Count >= 1 AndAlso detailNode.SelectNodes("ScheduleDiv").Count >= 1 AndAlso _
                (detailNode.SelectSingleNode("ScheduleDiv").InnerText.Equals(caldavReserv) Or detailNode.SelectSingleNode("ScheduleDiv").InnerText.Equals(caldavSuccsess)) Then

                    xDealerCode = detailNode.SelectSingleNode("DealerCode").InnerText
                    xBranchCode = detailNode.SelectSingleNode("BranchCode").InnerText
                    xScheduleID = detailNode.SelectSingleNode("ScheduleID").InnerText
                    xScheduleDiv = detailNode.SelectSingleNode("ScheduleDiv").InnerText
                    ' 2014/02/17 TCS 山田 受注後フォロー機能開発 START
                    Dim xCustomerName As String
                    xCustomerName = detailNode.SelectSingleNode("../ScheduleInfo/CustomerName").InnerText
                    ' 2014/02/17 TCS 山田 受注後フォロー機能開発 END

                    For Each todoNode As XmlNode In detailNode.SelectNodes("../VTodo")
                        Dim xDue As String = todoNode.SelectSingleNode("Due").InnerText
                        '2013/01/11 TCS 橋本 【A.STEP2】Add Start
                        Dim xCompFlg As String = todoNode.SelectSingleNode("CompFlg").InnerText
                        '2013/01/11 TCS 橋本 【A.STEP2】Add End
                        Dim xDueDatetime As Date = Date.ParseExact(xDue, "yyyy/MM/dd HH:mm:ss", Nothing)
                        Dim xDueDate As Date = New Date(xDueDatetime.Year, xDueDatetime.Month, xDueDatetime.Day)
                        Dim xDueDateEnd As Date = New DateTime(xDueDatetime.Year, xDueDatetime.Month, xDueDatetime.Day, 23, 59, 59)

                        '日またぎデータフラグ：開始日が過去(今日)日付かつ、期限日が未来日のもの
                        If todoNode.SelectNodes("DtStart").Count >= 1 Then
                            wDaysDataFlg = setDaysDataFlg(todoNode.SelectSingleNode("DtStart").InnerText, xDueDate, truncNow)
                        Else
                            wDaysDataFlg = NODAYSDATA
                        End If

                        xTimeFlg = todoNode.SelectSingleNode("TimeFlg").InnerText
                        xAllDayFlg = todoNode.SelectSingleNode("AllDayFlg").InnerText
                        If xTimeFlg.Equals(NOTIME_FLG) Or xAllDayFlg.Equals(ALLDAY_FLG) Then    '時刻無しデータか終日データの場合→23:59:59にして遅れの切り分け基準の変更
                            xDueDatetime = xDueDateEnd
                        End If

                        Dim flgday As Integer = setFlgDay(xDueDatetime, nowDate, xDueDate, truncNow)  '該当データの遅れ/今日/未来 の切り分け
                        Dim flgProc As Boolean = setDspJdg(serchDelay, serchToday, serchFuture, flgday, wDaysDataFlg)   '表示有無判定

                        ' 2014/02/17 TCS 山田 受注後フォロー機能開発 START
                        If flgProc And xCompFlg <> "1" Then         '表示対象時
                            Dim xProcessID As String = "00"           '受注後の工程コード
                            ' 2014/02/17 TCS 山田 受注後フォロー機能開発 END
                            If todoNode.SelectNodes("ProcessDiv").Count > 0 Then
                                xProcessID = todoNode.SelectSingleNode("ProcessDiv").InnerText
                            End If
                            Dim xContactNo As String = "0"                                      '接触№
                            If todoNode.SelectNodes("ContactNo").Count >= 1 Then
                                xContactNo = todoNode.SelectSingleNode("ContactNo").InnerText
                            End If
                            Dim flgStatus As String = STS_PROSPECT
                            If xScheduleDiv = caldavSuccsess Then               '受注後のCONTACTNOは再設定
                                ' 2014/02/17 TCS 山田 受注後フォロー機能開発 START DEL
                                ' 2014/02/17 TCS 山田 受注後フォロー機能開発 END
                                flgStatus = STS_SUCCESS
                            End If
                            dr = ds.SC3010401Search.NewSC3010401SearchRow           '検索用テーブルへ項目を転送
                            With dr
                                .DLRCD = xDealerCode
                                .STRCD = Left(xBranchCode & "   ", 3)
                                ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
                                .FLLWUPBOX_SEQNO = CType(xScheduleID, Decimal)
                                ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END
                                .TODOSEQ = WSEQ
                                .CONTACTNO = xContactNo
                                .TODONAME = todoNode.SelectSingleNode("Summary").InnerText      'TODO名称
                                .ALLDAYFLG = xAllDayFlg                  '終日フラグ
                                ' 2014/02/17 TCS 山田 受注後フォロー機能開発 START
                                .PROCESSID = Trim(xProcessID)                  '予定工程区分
                                .CUSTOMERNAME = xCustomerName
                                .CONTACTNAME = todoNode.SelectSingleNode("ContactName").InnerText '接触方法名
                                .ACTODRNAME = todoNode.SelectSingleNode("ActOdrName").InnerText '受注後活動名称
                                ' 2014/02/17 TCS 山田 受注後フォロー機能開発 END
                                .TIMEFLG = xTimeFlg                      '時刻有無
                                .CONTACTDATE = xDue
                                .ACTUALDATE = endDate                       '基盤からのシステム日時(苦情が取消される日の算出のため)
                                .SUCCESSFLG = flgStatus     '受注:2 or 見込:0
                                '2013/01/11 TCS 橋本 【A.STEP2】Add Start
                                .COMPFLG = xCompFlg         '完了フラグ　0:未完了/1:完了
                                '2013/01/11 TCS 橋本 【A.STEP2】Add End
                            End With

                            ds.SC3010401Search.Rows.Add(dr)
                            WSEQ = CShort(WSEQ + 1) '連番加算
                        End If
                    Next
                End If
            Next
            ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "{0}_End",
                                      MethodBase.GetCurrentMethod.Name))
            ' ======================== ログ出力 終了 ========================
            ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END
        Catch ex As Exception
            Logger.Error(ex.Message, ex)
            Throw
        End Try
    End Sub

    ''' <summary>
    ''' 該当データの遅れ/今日/未来 の切り分け
    ''' </summary>
    ''' <param name="xduedatetime">作業日時(DATE)</param>
    ''' <param name="nowdate">現在日時(DATE)</param>
    ''' <returns>0:遅れ 1:Due 2:未来</returns>
    ''' <remarks></remarks>
    Private Function setFlgDay(xduedatetime As Date, nowdate As Date, xDueDate As Date, truncNow As Date) As Integer
        Select Case DateTime.Compare(xduedatetime, nowdate)
            Case -1
                setFlgDay = caldavDelayData      '遅れデータ	0　遅れ判定は時刻を含めて判定
            Case 0
                setFlgDay = caldavDueData      '今日データ 1
            Case Else
                setFlgDay = caldavFutureData      '未来データ 2
                Select Case DateTime.Compare(xDueDate, truncNow) '時刻を含めて判定させて未来の場合は日付のみで判定する
                    Case 0
                        setFlgDay = caldavDueData      '今日データ 1
                End Select
        End Select
    End Function

    ''' <summary>
    ''' 表示有無判定
    ''' </summary>
    ''' <param name="serchDelay">遅れ表示(画面チェック)</param>
    ''' <param name="serchToday">今日表示(画面チェック)</param>
    ''' <param name="serchFuture">未来表示(画面チェック)</param>
    ''' <param name="flgDay">該当データが遅れか今日か未来かFLG</param>
    ''' <param name="wDaysDataFlg">日またぎデータFLG</param>
    ''' <returns>表示有無FLG</returns>
    ''' <remarks></remarks>
    Private Function setDspJdg(serchDelay As Integer, serchToday As Integer, serchFuture As Integer, flgDay As Integer, wDaysDataFlg As String) As Boolean

        setDspJdg = True

        '遅れチェックなしかつ遅れデータは処理しない
        If serchDelay = extractOff And flgDay = caldavDelayData Then
            setDspJdg = False
            '今日チェックなしかつ今日データは処理しない
        ElseIf serchToday = extractOff And flgDay = caldavDueData Then
            setDspJdg = False
            '遅れのみにチェックなら遅れデータ以外処理しない
        ElseIf serchDelay = extractOn And serchToday = extractOff And serchFuture = extractOff And flgDay <> caldavDelayData Then
            setDspJdg = False
        End If

        '実行中の日またぎデータは、今日チェック以外では表示しない
        If serchToday <> extractOn And wDaysDataFlg = DAYSDATA Then
            setDspJdg = False
        End If

    End Function

    ''' <summary>
    ''' 日またぎデータ判定
    ''' </summary>
    ''' <param name="xDtStart">開始日時</param>
    ''' <param name="xDueDate">完了予定日時</param>
    ''' <param name="truncNow">現在日時</param>
    ''' <returns>日またぎデータか否か</returns>
    ''' <remarks></remarks>
    Private Function setDaysDataFlg(xDtStart As String, xDueDate As Date, truncNow As Date) As String

        Dim frDatePastFlg As Integer
        Dim toDatePastFlg As Integer
        Dim xDtStarttime As Date
        Dim xDtStartDate As Date

        xDtStarttime = Date.ParseExact(xDtStart, "yyyy/MM/dd HH:mm:ss", Nothing)
        xDtStartDate = New Date(xDtStarttime.Year, xDtStarttime.Month, xDtStarttime.Day)

        frDatePastFlg = Date.Compare(xDtStartDate, truncNow)
        toDatePastFlg = Date.Compare(xDueDate, truncNow)

        If (frDatePastFlg = -1 And toDatePastFlg = 0) Or (frDatePastFlg = 0 And toDatePastFlg = 1) Or (frDatePastFlg = -1 And toDatePastFlg = 1) Then
            setDaysDataFlg = DAYSDATA
        Else
            setDaysDataFlg = NODAYSDATA
        End If

    End Function

    ' 2014/02/17 TCS 山田 受注後フォロー機能開発 START DEL
    ' 2014/02/17 TCS 山田 受注後フォロー機能開発 END

    ''' <summary>
    '''検索用テーブルから、情報収集しList(表示用)テーブルを作成する 
    ''' </summary>
    ''' <param name="ds">データセット</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' <para>2012/09/28 TCS 渡邊 【SALES_Step3】GTMC120924022の不具合修正</para>
    ''' </history>
    Public Sub SetListData(ByVal ds As SC3010401DataSet)

        Try
            ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "{0}_Start",
                                      MethodBase.GetCurrentMethod.Name))
            ' ======================== ログ出力 終了 ========================
            ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END

            Dim searchDataTbl As SC3010401DataSet.SC3010401SearchDataTable = ds.SC3010401Search     '検索用テーブル
            Dim dr As SC3010401DataSet.SC3010401ListAllRow                          '全件取込用テーブル
            Dim MainRow As SC3010401DataSet.SC3010401GetMainRow                     'FLLWUPBOX
            Dim SeriesRow As SC3010401DataSet.SC3010401GetSeriesCDRow               '車種ＣＤ取得 
            Dim SeriesNameRow As SC3010401DataSet.SC3010401GetSelectedSeriesRow     '車種名取得
            Dim CustomerRow As SC3010401DataSet.SC3010401GetCustRow                 '顧客情報
            Dim ClaimRow As SC3010401DataSet.SC3010401GetComplaintRow               '苦情情報取得
            Dim contactIconPathList As SC3010401DataSet.SC3010401GetContactIconpathDataTable = GetContactIconPath()
            ' 2014/02/17 TCS 山田 受注後フォロー機能開発 START
            Dim afterOdrPrcsIconPathList As SC3010401DataSet.SC3010401GetAfterOdrProcIconPathDataTable = GetAfterOdrProcIconPath(PLAN_AFTER_ODR_PROC)
            ' 2014/02/17 TCS 山田 受注後フォロー機能開発 END
            ' 2014/07/30 TCS 武田 受注後活動性能改善 START
            Dim salesId As Decimal = 0                                                           '商談ID
            Dim MainRowPool As SC3010401DataSet.SC3010401GetMainRow = Nothing                    'FLLWUPBOXプール
            Dim SeriesRowPool As SC3010401DataSet.SC3010401GetSeriesCDRow = Nothing              '車種ＣＤ取得プール 
            Dim SeriesNameRowPool As SC3010401DataSet.SC3010401GetSelectedSeriesRow = Nothing    '車種名取得プール
            Dim CustomerRowPool As SC3010401DataSet.SC3010401GetCustRow = Nothing                '顧客情報プール
            Dim ClaimRowPool As SC3010401DataSet.SC3010401GetComplaintRow = Nothing              '苦情情報取得プール
            Dim BookingNoRowPool As SC3010401DataSet.SC3010401GetBookingNoRow = Nothing          '注文情報取得プール
            ' 2014/07/30 TCS 武田 受注後活動性能改善 END

            For Each srchRow As SC3010401DataSet.SC3010401SearchRow In searchDataTbl
                Dim SortPlus As Int16 = 0

                'tbl_FLLWUPBOXの取得
                ' 2014/07/30 TCS 武田 受注後活動性能改善 START
                If salesId <> srchRow.FLLWUPBOX_SEQNO Then
                    MainRowPool = GetMain(srchRow)
                End If
                MainRow = MainRowPool
                ' 2014/07/30 TCS 武田 受注後活動性能改善 END

                If Not IsNothing(MainRow) Then  'tbl_fllwupboxは必須

                    'tbl_FLLWUPBOXから、検索用テーブルを更新(情報を補足)
                    '必須
                    With srchRow
                        '.BeginEdit()
                        .Item("INSDID") = MainRow("INSDID")
                        ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START DEL
                        ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END
                        .Item("CUSTSEGMENT") = MainRow("CUSTSEGMENT")
                        .Item("CRACTRESULT") = MainRow("CRACTRESULT")
                        ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
                        .Item("DLRCD") = MainRow("DLRCD")
                        ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END
                        '.EndEdit()
                    End With

                    '車種ＣＤを取得し、検索用テーブルを更新(情報を補足)
                    ' 2014/07/30 TCS 武田 受注後活動性能改善 START
                    If salesId <> srchRow.FLLWUPBOX_SEQNO Then
                        SeriesRowPool = GetSeriesCD(srchRow)             '車種ＣＤ取得
                    End If
                    SeriesRow = SeriesRowPool
                    ' 2014/07/30 TCS 武田 受注後活動性能改善 END
                    With srchRow
                        '.BeginEdit()
                        If IsNothing(SeriesRow) Then
                            .SERIESCD = ""
                            .MODELCD = ""
                        Else
                            .SERIESCD = SeriesRow.SERIESCD
                            .MODELCD = SeriesRow.MODELCD
                        End If
                        '.EndEdit()
                    End With

                    '各テーブルからの情報取得
                    ' 2014/07/30 TCS 武田 受注後活動性能改善 START
                    If salesId <> srchRow.FLLWUPBOX_SEQNO Then
                        SeriesNameRowPool = GetSelectedSeries(srchRow)   '車名取得
                    End If
                    SeriesNameRow = SeriesNameRowPool

                    If salesId <> srchRow.FLLWUPBOX_SEQNO Then
                        CustomerRowPool = GetCust(srchRow)               '顧客情報取得
                    End If
                    CustomerRow = CustomerRowPool
                    ' 2014/07/30 TCS 武田 受注後活動性能改善 END

                    '受注後工程状態取得と算出(受注:3のとき)
                    ' 2014/02/17 TCS 山田 受注後フォロー機能開発 START
                    Dim beginRowIndex As Integer = 0
                    Dim afterOdrIconPath As String = String.Empty
                    If MainRow("CRACTRESULT").ToString = FllwStatusSuccess Then
                        For i As Integer = beginRowIndex To afterOdrPrcsIconPathList.Rows.Count - 1
                            If srchRow.PROCESSID = afterOdrPrcsIconPathList(i).AFTER_ODR_PRCS_CD Then
                                SortPlus = CShort(i + 1)
                                If afterOdrPrcsIconPathList(i).IsICON_PATHNull Then
                                    afterOdrIconPath = String.Empty
                                Else
                                    afterOdrIconPath = afterOdrPrcsIconPathList(i).ICON_PATH
                                End If
                                Exit For
                            End If
                        Next
                        ' 2014/02/17 TCS 山田 受注後フォロー機能開発 END
                    End If

                    'ListAll(全件データテーブル)項目へ転送
                    dr = ds.SC3010401ListAll.NewSC3010401ListAllRow

                    '検索用テーブルの項目から転送
                    With srchRow
                        dr.DLRCD = .DLRCD
                        dr.STRCD = .STRCD
                        dr.FLLWUPBOX_SEQNO = .FLLWUPBOX_SEQNO
                        dr.TODONAME = .TODONAME
                        dr.TODOSEQ = .TODOSEQ
                        '顧客区分 1:自 2:未は個人情報TBL取得時に転送する
                        '→　2:未でも自客に結びつけば1:自とするため。
                        'dr.CUSTSEGMENT = .CUSTSEGMENT
                        dr.CONTACTNO = .CONTACTNO
                        dr.CONTACTDATE = Date.ParseExact(.CONTACTDATE, "yyyy/MM/dd HH:mm:ss", Nothing)
                        If .TIMEFLG.Equals(NOTIME_FLG) Or .ALLDAYFLG.Equals(ALLDAY_FLG) Then
                            dr.CONTACTDATE = New DateTime(dr.CONTACTDATE.Year, dr.CONTACTDATE.Month, dr.CONTACTDATE.Day, 23, 59, 59)
                        End If
                        dr.TIMEFLG = .TIMEFLG
                        dr.ALLDAYFLG = .ALLDAYFLG
                        dr.CRACTRESULT = .CRACTRESULT
                        '2013/01/11 TCS 橋本 【A.STEP2】Add Start
                        dr.COMPFLG = .COMPFLG
                        '2013/01/11 TCS 橋本 【A.STEP2】Add End
                        ' 2014/02/17 TCS 山田 受注後フォロー機能開発 START
                        dr.CUSTOMERNAME = .CUSTOMERNAME
                        dr.CONTACTNAME = .CONTACTNAME
                        dr.ACTODRNAME = .ACTODRNAME
                        ' 2014/02/17 TCS 山田 受注後フォロー機能開発 END
                    End With

                    '顧客情報から転送
                    If IsNothing(CustomerRow) Then
                        dr.IMAGEFILE_S = ""
                        dr.STAFFCD = ""
                        dr.KOKYAKUID = ""
                    Else
                        dr.IMAGEFILE_S = CustomerRow.IMAGEFILE_S
                        dr.STAFFCD = CustomerRow.STAFFCD

                        ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
                        dr.KOKYAKUID = CustomerRow.KOKYAKUID.ToString(CultureInfo.InvariantCulture)
                        ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END
                        dr.CUSTSEGMENT = CustomerRow.KOKYAKUKBN
                        ' 2014/02/17 TCS 山田 受注後フォロー機能開発 START
                        dr.NAMETITLE = CustomerRow.NAMETITLE_NAME
                        ' 2014/02/17 TCS 山田 受注後フォロー機能開発 END
                        ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-3 START
                        dr.CSTJOINTYPE = CustomerRow.CSTJOINTYPE
                        ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-3 END
                    End If

                    ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
                    ' 2014/07/30 TCS 武田 受注後活動性能改善 START
                    If salesId <> srchRow.FLLWUPBOX_SEQNO Then
                        ClaimRowPool = GetComplaint(srchRow)          '苦情情報取得
                    End If
                    ClaimRow = ClaimRowPool
                    ' 2014/07/30 TCS 武田 受注後活動性能改善 END
                    ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END

                    '苦情情報から転送
                    dr.CLMFLG = CStr(IIf(ClaimRow.CMPCNT > 0, clmExist, clmNoExist))

                    '車両情報から転送
                    If IsNothing(SeriesRow) Then
                        dr.SERIESCD = ""
                        dr.MODELCD = ""
                    Else
                        dr.SERIESCD = SeriesRow.SERIESCD
                        dr.MODELCD = SeriesRow.MODELCD
                    End If
                    If IsNothing(SeriesNameRow) Then
                        dr.SERIESNM = ""
                        dr.VCLMODEL_NAME = ""
                    Else
                        dr.SERIESNM = SeriesNameRow.SERIESNM
                        '2012/09/28 TCS 渡邊 【SALES_Step3】GTMC120924022の不具合修正 START
                        If Not String.IsNullOrEmpty(SeriesNameRow.VCLMODEL_NAME) Then
                            dr.VCLMODEL_NAME = SeriesNameRow.VCLMODEL_NAME
                        Else
                            dr.VCLMODEL_NAME = ""
                        End If
                        '2012/09/28 TCS 渡邊 【SALES_Step3】GTMC120924022の不具合修正 END
                    End If

                    'FLLWUPBOXから転送
                    ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
                    dr.CRCUSTID = MainRow.INSDID.ToString(CultureInfo.InvariantCulture)
                    ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END
                    dr.CUSTOMERCLASS = MainRow.CUSTOMERCLASS

                    'ステイタス用ソート項目算出
                    'CR活動結果(hot:100,prospect:200,Cold:300,受:400,断:500) ＋ 受注後工程(400 + TB_M_AFTER_ODR_PROC.SORT_ORDER + 1)
                    dr.CRRESULTSORT = MainRow.CRRESULTSORT + SortPlus
                    ' 2014/02/17 TCS 山田 受注後フォロー機能開発 START
                    dr.AFTERODRICONPATH = afterOdrIconPath
                    ' 2014/02/17 TCS 山田 受注後フォロー機能開発 END

                    '接触方法アイコン取得
                    dr.CONTACTICONPATH = ""
                    'If dr.CRACTRESULT = FllwStatusSuccess Then
                    ' 2014/02/17 TCS 山田 受注後フォロー機能開発 START
                    '受注前、受注後
                    Dim intContactNo As Integer = 0
                    If Integer.TryParse(dr.CONTACTNO, intContactNo) Then

                        dr.CONTACTICONPATH = (
                         From n In contactIconPathList
                         Where n.CONTACTNO = dr.CONTACTNO
                         Select n.CONTACTICONPATH
                         ).FirstOrDefault

                        If dr.IsCONTACTICONPATHNull Then
                            dr.CONTACTICONPATH = String.Empty
                        Else
                            dr.CONTACTICONPATH = VirtualPathUtility.ToAbsolute(dr.CONTACTICONPATH)
                        End If
                    End If

                    '注文番号取得
                    dr.BOOKINGNO = ""
                    Dim BookingNoRow As SC3010401DataSet.SC3010401GetBookingNoRow
                    ' 2014/07/30 TCS 武田 受注後活動性能改善 START
                    If salesId <> srchRow.FLLWUPBOX_SEQNO Then
                        BookingNoRowPool = GetBookingNo(srchRow)          '注文番号取得
                    End If
                    BookingNoRow = BookingNoRowPool
                    ' 2014/07/30 TCS 武田 受注後活動性能改善 END

                    '車両情報から転送
                    If IsNothing(BookingNoRow) Then
                        dr.BOOKINGNO = ""
                    Else
                        dr.BOOKINGNO = BookingNoRow.BOOKINGNO
                    End If
                    ' 2014/02/17 TCS 山田 受注後フォロー機能開発 END

                    'ListAllデータテーブルへ追加
                    ds.SC3010401ListAll.Rows.Add(dr)

                    ' 2014/07/30 TCS 武田 受注後活動性能改善 START
                    '検索用テーブルから商談IDをセット
                    salesId = srchRow.FLLWUPBOX_SEQNO
                    ' 2014/07/30 TCS 武田 受注後活動性能改善 END
                End If  'Not IsNothing(MainRow)

            Next

            ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "{0}_End",
                                      MethodBase.GetCurrentMethod.Name))
            ' ======================== ログ出力 終了 ========================
            ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END

        Catch ex As Exception
            Logger.Error(ex.Message, ex)
            Throw
        End Try

    End Sub

    ''' <summary>
    ''' 抽出条件、ソート条件の反映
    ''' </summary>
    ''' <param name="ds">データセット</param>
    ''' <param name="selectStr">抽出条件</param>
    ''' <param name="sortStr">ソート条件</param>
    ''' <remarks></remarks>
    Public Sub SetListFilter(ByVal ds As SC3010401DataSet, ByVal selectStr As String, ByVal sortStr As String)

        Try
            Logger.Info("SetListFilter Start")

            For Each dtrow As DataRow In ds.SC3010401ListAll.Select(selectStr, sortStr)
                ds.SC3010401ListFilter.ImportRow(dtrow)
            Next

            Logger.Info("SetListFilter End")

        Catch ex As Exception
            Logger.Error(ex.Message, ex)
            Throw
        End Try

    End Sub

    ''' <summary>
    ''' 主情報取得
    ''' </summary>
    ''' <param name="searchDataRow">データセット</param>
    ''' <returns>主情報</returns>
    ''' <remarks></remarks>
    Private Function GetMain(ByVal searchDataRow As SC3010401DataSet.SC3010401SearchRow) As SC3010401DataSet.SC3010401GetMainRow

        ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0}_Start",
                                  MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        '主情報（商談情報）取得
        Dim MainSalesInfo As SC3010401DataSet.SC3010401GetMainSaleDataTable
        MainSalesInfo = SC3010401DataTableTableAdapter.GetMainSale(searchDataRow.FLLWUPBOX_SEQNO)

        If MainSalesInfo.Count = 0 Then
            '主情報（商談情報）がない場合
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "{0}_End, Return:[{1}]",
                                      MethodBase.GetCurrentMethod.Name,
                                      "Nothing"))
            ' ======================== ログ出力 終了 ========================
            Return Nothing
        Else
            Dim MainSalesInfoRow As SC3010401DataSet.SC3010401GetMainSaleRow
            MainSalesInfoRow = MainSalesInfo.Item(0)

            If MainSalesInfoRow.REQ_ID <> 0 And MainSalesInfoRow.VCL_ID <> 0 Then
                '主情報（用件）取得
                Dim RequestInfo As SC3010401DataSet.SC3010401GetMainDataTable
                RequestInfo = SC3010401DataTableTableAdapter.GetMainRequest(searchDataRow.FLLWUPBOX_SEQNO)

                If RequestInfo.Count = 0 Then
                    ' ======================== ログ出力 開始 ========================
                    Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                              "{0}_End, Return:[{1}]",
                                              MethodBase.GetCurrentMethod.Name,
                                              "Nothing"))
                    ' ======================== ログ出力 終了 ========================
                    Return Nothing
                Else
                    ' ======================== ログ出力 開始 ========================
                    Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                              "{0}_End, Return:[{1}]",
                                              MethodBase.GetCurrentMethod.Name,
                                              RequestInfo.Item(0).ToString))
                    ' ======================== ログ出力 終了 ========================
                    Return RequestInfo.Item(0)
                End If

            ElseIf MainSalesInfoRow.REQ_ID <> 0 And MainSalesInfoRow.VCL_ID = 0 Then
                '主情報（車両なし用件）取得
                Dim RequestInfoNoVcl As SC3010401DataSet.SC3010401GetMainDataTable
                RequestInfoNoVcl = SC3010401DataTableTableAdapter.GetMainRequestNoVcl(searchDataRow.FLLWUPBOX_SEQNO, MainSalesInfoRow.CST_ID)

                If RequestInfoNoVcl.Count = 0 Then
                    ' ======================== ログ出力 開始 ========================
                    Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                              "{0}_End, Return:[{1}]",
                                              MethodBase.GetCurrentMethod.Name,
                                              "Nothing"))
                    ' ======================== ログ出力 終了 ========================
                    Return Nothing
                Else
                    ' ======================== ログ出力 開始 ========================
                    Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                              "{0}_End, Return:[{1}]",
                                              MethodBase.GetCurrentMethod.Name,
                                              RequestInfoNoVcl.Item(0).ToString))
                    ' ======================== ログ出力 終了 ========================
                    Return RequestInfoNoVcl.Item(0)
                End If

            ElseIf MainSalesInfoRow.ATT_ID <> 0 And MainSalesInfoRow.VCL_ID <> 0 Then
                '主情報（誘致）取得
                Dim AttractInfo As SC3010401DataSet.SC3010401GetMainDataTable
                AttractInfo = SC3010401DataTableTableAdapter.GetMainAttract(searchDataRow.FLLWUPBOX_SEQNO)

                If AttractInfo.Count = 0 Then
                    ' ======================== ログ出力 開始 ========================
                    Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                              "{0}_End, Return:[{1}]",
                                              MethodBase.GetCurrentMethod.Name,
                                              "Nothing"))
                    ' ======================== ログ出力 終了 ========================
                    Return Nothing
                Else
                    ' ======================== ログ出力 開始 ========================
                    Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                              "{0}_End, Return:[{1}]",
                                              MethodBase.GetCurrentMethod.Name,
                                              AttractInfo.Item(0).ToString))
                    ' ======================== ログ出力 終了 ========================
                    Return AttractInfo.Item(0)
                End If
            Else
                '主情報（車両なし誘致）取得
                Dim AttractInfoNoVcl As SC3010401DataSet.SC3010401GetMainDataTable
                AttractInfoNoVcl = SC3010401DataTableTableAdapter.GetMainAttractNoVcl(searchDataRow.FLLWUPBOX_SEQNO, MainSalesInfoRow.CST_ID)

                If AttractInfoNoVcl.Count = 0 Then
                    ' ======================== ログ出力 開始 ========================
                    Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                              "{0}_End, Return:[{1}]",
                                              MethodBase.GetCurrentMethod.Name,
                                              "Nothing"))
                    ' ======================== ログ出力 終了 ========================
                    Return Nothing
                Else
                    ' ======================== ログ出力 開始 ========================
                    Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                              "{0}_End, Return:[{1}]",
                                              MethodBase.GetCurrentMethod.Name,
                                              AttractInfoNoVcl.Item(0).ToString))
                    ' ======================== ログ出力 終了 ========================
                    Return AttractInfoNoVcl.Item(0)
                End If
            End If

        End If

    End Function

    ''' <summary>
    ''' 車種ＣＤ取得
    ''' </summary>
    ''' <param name="searchDataRow">データセット</param>
    ''' <returns>車種ＣＤ</returns>
    ''' <remarks></remarks>
    Private Function GetSeriesCD(ByVal searchDataRow As SC3010401DataSet.SC3010401SearchRow) As SC3010401DataSet.SC3010401GetSeriesCDRow

        ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0}_Start",
                                  MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================
        ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END

        Dim SeriesCD As SC3010401DataSet.SC3010401GetSeriesCDDataTable

        ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
        SeriesCD = SC3010401DataTableTableAdapter.GetSeriesCD(searchDataRow.FLLWUPBOX_SEQNO, searchDataRow.CRACTRESULT)
        ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END

        If SeriesCD.Count = 0 Then
            ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "{0}_End, Return:[{1}]",
                                      MethodBase.GetCurrentMethod.Name,
                                      "Nothing"))
            ' ======================== ログ出力 終了 ========================
            ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END
            Return Nothing
        Else
            ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "{0}_End, Return:[{1}]",
                                      MethodBase.GetCurrentMethod.Name,
                                      SeriesCD.Item(0).ToString))
            ' ======================== ログ出力 終了 ========================
            ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END
            Return SeriesCD.Item(0)
        End If

        ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START DEL
        ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END

    End Function

    ''' <summary>
    ''' 顧客情報取得
    ''' </summary>
    ''' <param name="searchDataRow">データセット</param>
    ''' <returns>顧客情報</returns>
    ''' <remarks></remarks>
    Private Function GetCust(ByVal searchDataRow As SC3010401DataSet.SC3010401SearchRow) As SC3010401DataSet.SC3010401GetCustRow

        ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0}_Start",
                                  MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================
        ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END

        Dim CustInfo As SC3010401DataSet.SC3010401GetCustDataTable
        ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
        CustInfo = SC3010401DataTableTableAdapter.GetCustomer(searchDataRow.INSDID, searchDataRow.DLRCD)
        ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END

        If CustInfo.Count = 0 Then
            ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "{0}_End, Return:[{1}]",
                                      MethodBase.GetCurrentMethod.Name,
                                      "Nothing"))
            ' ======================== ログ出力 終了 ========================
            ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END
            Return Nothing
        Else
            ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "{0}_End, Return:[{1}]",
                                      MethodBase.GetCurrentMethod.Name,
                                      CustInfo.Item(0).ToString))
            ' ======================== ログ出力 終了 ========================
            ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END
            Return CustInfo.Item(0)
        End If

        ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START DEL
        ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END

    End Function

    ''' <summary>
    ''' 車種名称取得
    ''' </summary>
    ''' <param name="searchDataRow">データセット</param>
    ''' <returns>車種名称</returns>
    ''' <remarks></remarks>
    Private Function GetSelectedSeries(ByVal searchDataRow As SC3010401DataSet.SC3010401SearchRow) As SC3010401DataSet.SC3010401GetSelectedSeriesRow

        ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0}_Start",
                                  MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================
        ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END

        Dim SeriesName As SC3010401DataSet.SC3010401GetSelectedSeriesDataTable
        ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
        SeriesName = SC3010401DataTableTableAdapter.GetSelectedSeries(searchDataRow.SERIESCD, searchDataRow.MODELCD)
        ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END

        If SeriesName.Rows.Count = 0 Then
            ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "{0}_End, Return:[{1}]",
                                      MethodBase.GetCurrentMethod.Name,
                                      "Nothing"))
            ' ======================== ログ出力 終了 ========================
            ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END
            Return Nothing
        Else
            ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "{0}_End, Return:[{1}]",
                                      MethodBase.GetCurrentMethod.Name,
                                      SeriesName.Item(0).ToString))
            ' ======================== ログ出力 終了 ========================
            ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END
            Return SeriesName.Item(0)
        End If

        ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START DEL
        ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END

    End Function

    ' 2014/02/17 TCS 山田 受注後フォロー機能開発 START END
    ' 2014/02/17 TCS 山田 受注後フォロー機能開発 END

    ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 苦情情報件数取得
    ''' </summary>
    ''' <param name="searchDataRow">データセット</param>
    ''' <returns>苦情情報</returns>
    ''' <remarks></remarks>
    Private Function GetComplaint(ByVal searchDataRow As SC3010401DataSet.SC3010401SearchRow) As SC3010401DataSet.SC3010401GetComplaintRow

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0}_Start",
                                  MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================
        ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END

        '苦情を表示させる日数をシステムより取得
        Dim sysEnv As New SystemEnvSetting
        Dim PreDays As Int16

        If sysEnv.GetSystemEnvSetting(dspComplaintDay) Is Nothing Then
            PreDays = 0 '仮
        Else
            PreDays = CShort(sysEnv.GetSystemEnvSetting(dspComplaintDay).PARAMVALUE)
        End If

        Dim paramactualdate As DateTime = searchDataRow.ACTUALDATE.AddDays(PreDays * -1)

        ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
        Dim Complaint As SC3010401DataSet.SC3010401GetComplaintDataTable
        Complaint = SC3010401DataTableTableAdapter.GetComplaint(paramactualdate, searchDataRow.INSDID)

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0}_End, Return:[{1}]",
                                  MethodBase.GetCurrentMethod.Name,
                                  Complaint.Item(0).ToString))
        ' ======================== ログ出力 終了 ========================

        Return Complaint.Item(0)

        ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END

    End Function

    ''' <summary>
    ''' 顔写真の保存先フォルダ(Web向け)取得
    ''' </summary>
    ''' <returns>顔写真の保存先フォルダ(Web向け)</returns>
    ''' <remarks>顔写真の保存先フォルダ(Web向け)取得</remarks>
    ReadOnly Property GetImagePath As String
        Get
            '顔写真の保存先フォルダ(Web向け)取得
            Dim sysEnv As New SystemEnvSetting
            Dim sysEnvRow As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow
            sysEnvRow = sysEnv.GetSystemEnvSetting(FacePicUploadurl)

            Return sysEnvRow.PARAMVALUE
        End Get
    End Property

    ''' <summary>
    ''' コンタクトアイコンパスの取得
    ''' </summary>
    ''' <returns>コンタクトアイコンパス</returns>
    ''' <remarks></remarks>
    Private Function GetContactIconPath() As SC3010401DataSet.SC3010401GetContactIconpathDataTable

        Logger.Info("GetContactIconPath Start")

        Dim IconPath As SC3010401DataSet.SC3010401GetContactIconpathDataTable
        ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
        IconPath = SC3010401DataTableTableAdapter.GetContactIconPath(context.DlrCD)
        ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END
        If IsNothing(IconPath) Then
            ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "{0}_End, Return:[{1}]",
                                      MethodBase.GetCurrentMethod.Name,
                                      "Nothing"))
            ' ======================== ログ出力 終了 ========================
            ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END
            Return Nothing
        Else
            ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "{0}_End, Return RowCount:[{1}]",
                                      MethodBase.GetCurrentMethod.Name, IconPath.Rows.Count))
            ' ======================== ログ出力 終了 ========================
            ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END
            Return IconPath
        End If

        ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START DEL
        ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END

    End Function

    ' 2014/02/17 TCS 山田 受注後フォロー機能開発 START DEL
    ' 2014/02/17 TCS 山田 受注後フォロー機能開発 END

    ' 2014/02/17 TCS 山田 受注後フォロー機能開発 START
    ''' <summary>
    ''' 顧客検索条件項目取得
    ''' </summary>
    ''' <returns>顧客検索条件項目情報</returns>
    ''' <remarks></remarks>
    Public Function GetCstSearchCond() As SC3010401DataSet.SC3010401GetCstSearchCondDataTable

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0}_Start",
                                  MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        '顧客検索条件項目取得
        Dim todoSearchTypeList As SC3010401DataSet.SC3010401GetCstSearchCondDataTable
        todoSearchTypeList = SC3010401DataTableTableAdapter.GetCstSearchCond()

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0}_End, Return RowCount:[{1}]",
                                  MethodBase.GetCurrentMethod.Name,
                                  todoSearchTypeList.Rows.Count))
        ' ======================== ログ出力 終了 ========================

        Return todoSearchTypeList

    End Function

    ''' <summary>
    ''' 受注後工程アイコンパス取得
    ''' </summary>
    ''' <returns>受注後工程アイコンパス情報</returns>
    ''' <remarks></remarks>
    Public Function GetAfterOdrProcIconPath(ByVal secondKey As String) As SC3010401DataSet.SC3010401GetAfterOdrProcIconPathDataTable

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0}_Start",
                                  MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        '顧客検索条件項目取得
        Dim afterOdrPrcsIconPathList As SC3010401DataSet.SC3010401GetAfterOdrProcIconPathDataTable
        afterOdrPrcsIconPathList = SC3010401DataTableTableAdapter.GetAfterOdrProcIconPath(context.DlrCD, secondKey)

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0}_End, Return RowCount:[{1}]",
                                  MethodBase.GetCurrentMethod.Name,
                                  afterOdrPrcsIconPathList.Rows.Count))
        ' ======================== ログ出力 終了 ========================

        Return afterOdrPrcsIconPathList

    End Function

    ''' <summary>
    ''' 顧客一覧作成
    ''' </summary>
    ''' <returns>顧客一覧</returns>
    ''' <remarks></remarks>
    Public Function GetCustomerList(ByVal searchDirection As Integer, ByVal searchValue As String,
      ByVal searchType As String) As SC3010401DataSet.SC3010401GetCustomerListDataTable

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0}_Start",
                                  MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        '顧客検索条件項目取得
        Dim customerList As SC3010401DataSet.SC3010401GetCustomerListDataTable
        customerList = SC3010401DataTableTableAdapter.GetCustomerList(context.DlrCD, searchDirection, searchValue, searchType, context.Account)

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0}_End, Return RowCount:[{1}]",
                                  MethodBase.GetCurrentMethod.Name,
                                  customerList.Rows.Count))
        ' ======================== ログ出力 終了 ========================

        Return customerList

    End Function

    ''' <summary>
    ''' 注文番号取得
    ''' </summary>
    ''' <param name="searchDataRow">データセット</param>
    ''' <returns>注文番号</returns>
    ''' <remarks></remarks>
    Private Function GetBookingNo(ByVal searchDataRow As SC3010401DataSet.SC3010401SearchRow) As SC3010401DataSet.SC3010401GetBookingNoRow

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0}_Start",
                                  MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        Dim BookingNo As SC3010401DataSet.SC3010401GetBookingNoDataTable
        BookingNo = SC3010401DataTableTableAdapter.GetBookingNo(searchDataRow.DLRCD, searchDataRow.FLLWUPBOX_SEQNO)

        If BookingNo.Count = 0 Then
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "{0}_End, Return:[{1}]",
                                      MethodBase.GetCurrentMethod.Name,
                                      "Nothing"))
            ' ======================== ログ出力 終了 ========================
            Return Nothing
        Else
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "{0}_End, Return:[{1}]",
                                      MethodBase.GetCurrentMethod.Name,
                                      BookingNo.Item(0).ToString))
            ' ======================== ログ出力 終了 ========================
            Return BookingNo.Item(0)
        End If

    End Function
    ' 2014/02/17 TCS 山田 受注後フォロー機能開発 END

    '2015/12/08 TCS 中村 MOD (ﾄﾗｲ店ｼｽﾃﾑ評価)新車ﾀﾌﾞﾚｯﾄ(受注後)の管理機能開発 START
    ''' <summary>
    ''' 受注後工程利用フラグ取得
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="brncd">店舗コード</param>
    ''' <returns>受注後工程利用フラグ(0:利用しない、1:利用する)</returns>
    ''' <remarks></remarks>
    Public Shared Function GetAfterOdrProcFlg(ByVal dlrcd As String, ByVal brncd As String) As String
        Logger.Info("GetAfterOdrProcFlg Start")

       '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 START
        '①販売店≠'XXXXX'、店舗≠'XXX'（販売店コード・店舗コード該当）
        '②①実行でデータがなければ販売店≠'XXXXX'、店舗＝'XXX'販売店（販売店コードのみ該当）
        '③①②実行でデータがなければ販売店＝'XXXXX'、店舗＝'XXX'（販売店コード・店舗コードいずれも該当なし(デフォルト値)  
        Dim systemBiz As New SystemSettingDlr
        Dim drSettingDlr As SystemSettingDlrDataSet.TB_M_SYSTEM_SETTING_DLRRow = systemBiz.GetEnvSetting(dlrcd, brncd, C_USE_AFTER_ODR_PROC_FLG)

        'データ取得できない場合も、デフォルト設定値がない場合もエラー
        If drSettingDlr Is Nothing Then
            Return Nothing
        End If

        Logger.Info("GetAfterOdrProcFlg End")
        Return drSettingDlr.SETTING_VAL
        '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 END
    End Function
    '2015/12/08 TCS 中村 MOD (ﾄﾗｲ店ｼｽﾃﾑ評価)新車ﾀﾌﾞﾚｯﾄ(受注後)の管理機能開発 END
End Class
