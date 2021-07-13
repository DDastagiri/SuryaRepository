'------------------------------------------------------------------------------
'SC3150201.aspx.vb
'------------------------------------------------------------------------------
'機能：TCステータスモニター
'補足：
'作成：2013/02/21 TMEJ　成澤
'更新：2013/12/12 TMEJ 成澤　【開発】IT9611_次世代サービス 工程管理機能開発
'更新：2016/03/30 NSK  小牟禮 アクティビティインジケータが消えない問題
'更新：2018/05/02 NSK  井本 車両番号のシングルクォーテーションによるエラー対応
'------------------------------------------------------------------------------
Option Strict On
Option Explicit On

Imports System
Imports System.Data
Imports System.Globalization
Imports System.Web.Script.Serialization
Imports Toyota.eCRB.iCROP.BizLogic.SC3150201
Imports Toyota.eCRB.iCROP.DataAccess.SC3150201
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic

Partial Class Pages_SC3150201
    Inherits BasePage

    ''' セッションキー
    Public Const SESSION_KEY_STALL_ID As String = "SessionKey.StallId"  'ストールID

#Region "定数"

    ''' <summary>
    ''' 画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const APPLICATION_ID As String = "SC3150201"

    ''' <summary>
    ''' SC3150101の画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const REDIRECT_ID As String = "SC3150101"

    ''' <summary>
    ''' DateTimeFuncにて、"yyyy/MM/dd"形式をコンバートするためのID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DATE_CONVERT_ID_YYYY_MM_DD As Integer = 21

    ''' <summary>
    ''' DateTimeFuncにて、"yyyy/MM/dd HH:mm"形式をコンバートするためのID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DATE_CONVERT_ID_YYYYMMDDHHMM As Integer = 2

    ''' <summary>
    ''' リフレッショタイムの初期値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const Default_Refresh_Time As Integer = 60
    ' 2013/12/12　TMEJ 成澤　IT9611_次世代サービス 工程管理機能開発 START
    ''' <summary>
    ''' オペレーションコード：チーフテクニシャン
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OPERATION_CODE_CHT As Integer = 62
    ''' <summary>
    '''日付最小値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MINDATE As String = "1900/01/01 0:00:00"
    ' 2013/12/12　TMEJ 成澤　IT9611_次世代サービス 工程管理機能開発 END
#End Region

#Region "メンバ変数"

    ''' <summary>
    ''' ユーザ情報（セッションより）
    ''' </summary>
    ''' <remarks></remarks>
    Private objStaffContext As StaffContext

    ''' <summary>
    ''' ビジネスロジック
    ''' </summary>
    ''' <remarks></remarks>
    Private businessLogic As New SC3150201BusinessLogic

    '2013/12/12 TMEJ 成澤　【開発】IT9611_次世代サービス 工程管理機能開発 START
    ''' <summary>
    ''' ログイン中のストールID
    ''' </summary>
    ''' <remarks></remarks>
    Private stallId As Decimal
    'Private stallId As Integer
    '2013/12/12 TMEJ 成澤　【開発】IT9611_次世代サービス 工程管理機能開発 END

    ''' <summary>
    ''' ストールの稼動開始時間
    ''' </summary>
    ''' <remarks></remarks>
    Private stallActualStartTime As Date

    ''' <summary>
    ''' ストールの稼動終了時間
    ''' </summary>
    ''' <remarks></remarks>
    Private stallActualEndTime As Date

#End Region

#Region "イベント"

    ''' <summary>
    ''' ページロード時の処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    ''' 
    ''' <History>
    ''' 2013/12/12　TMEJ 成澤　IT9611_次世代サービス 工程管理機能開発
    ''' </History>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' 2013/12/12　TMEJ 成澤　IT9611_次世代サービス 工程管理機能開発 START
        'セッションキーのチェック
        Dim sessionKeyValue As Boolean = Me.ContainsKey(ScreenPos.Current, SESSION_KEY_STALL_ID)
        'キーがあり
        If sessionKeyValue Then
            'セッションの値を取得
            Me.stallId = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_STALL_ID, False), Decimal)
        End If
        ' 2013/12/12　TMEJ 成澤　IT9611_次世代サービス 工程管理機能開発 END

        '最初のアクセス時にのみ行う
        If Not IsPostBack Then

            Logger.Info("Page_Load.S")

            'ストール情報を取得
            SetStallInfo()
            '予約・実績チップ情報取得
            GetCandidateChipInfo()
            'リフレッシュタイム格納
            HiddenFieldValue()

            Logger.Info("Page_Load.E")
        End If
    End Sub

    ''' <summary>
    ''' HiddenButtonRefreshtSC3150201ボタンクリックイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub HiddenButtonRefreshtSC3150201_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles HiddenButtonRefreshtSC3150201.Click

        Logger.Info("HiddenButtonRefreshtSC3150201_Click.S")

        'ストール情報を取得
        SetStallInfo()
        '予約・実績チップ情報取得
        GetCandidateChipInfo()

        ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", " LoadProcessHide();", True)

        Logger.Info("HiddenButtonRefreshtSC3150201_Click.E")

    End Sub

    ''' <summary>
    ''' HiddenButtonRedirectSC3150101ボタンクリックイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub HiddenButtonRedirectSC3150101_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles HiddenButtonRedirectSC3150101.Click
        Logger.Info("HiddenButtonRedirectSC3150101_Click.S")

        ' 2013/12/12　TMEJ 成澤　IT9611_次世代サービス 工程管理機能開発 START
        'ユーザ情報の取得.
        objStaffContext = StaffContext.Current

        If OPERATION_CODE_CHT = objStaffContext.OpeCD Then
            Me.SetValue(ScreenPos.Next, "SC3150101.StallId", Me.stallId)
        End If
        ' 2013/12/12　TMEJ 成澤　IT9611_次世代サービス 工程管理機能開発 END
        ' TCメインメニューへ遷移
        Me.RedirectNextScreen(REDIRECT_ID)

        Logger.Info("HiddenButtonRedirectSC3150101_Click.E")

    End Sub

    ''' <summary>
    ''' HiddenButtonRefreshボタンクリックイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub HiddenButtonRefresh_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles HiddenButtonRefresh.Click
        Logger.Info("HiddenButtonRefresh_Click.S")

        ' リロード
        Me.RedirectNextScreen(APPLICATION_ID)

        Logger.Info("HiddenButtonRefresh_Click.E")
    End Sub

#End Region

#Region "メソッド"

    ''' <summary>
    ''' スクリーンセイバーの内容を設定する
    ''' </summary>
    ''' <param name="inpEntryNo">登録No</param>
    ''' <param name="inpEstimatedTime">納車予定時刻</param>
    ''' <param name=" inpConstructionPlan">着工計画</param>
    ''' <param name="inpConstructionResults">着工実績</param>
    ''' <param name="inpCompleted">完了計画</param>
    ''' <remarks></remarks>
    Private Sub ScreenSaverValue(ByVal inpEntryNo As String, _
                                   ByVal inpEstimatedTime As String, _
                                   ByVal inpConstructionPlan As String, _
                                   ByVal inpConstructionResults As String, _
                                   ByVal inpCompleted As String)

        Logger.Info("ScreenSaverValue.S")

        '変数宣言
        Dim entryNoWord As String = "登録No."
        Dim estimatedTimeWord As String = "納車予定時刻"
        Dim constructionPlanWord As String = "着工計画"
        Dim constructionResultsWord As String = "着工実績"
        Dim completedWord As String = "完了計画"

        '項目名文字列取得
        entryNoWord = WebWordUtility.GetWord(1)
        estimatedTimeWord = WebWordUtility.GetWord(2)
        constructionPlanWord = WebWordUtility.GetWord(3)
        constructionResultsWord = WebWordUtility.GetWord(5)
        completedWord = WebWordUtility.GetWord(4)


        Dim javaScriptWord As StringBuilder = New StringBuilder

        ' 2018/05/02 NSK  井本 車両番号のシングルクォーテーションによるエラー対応 START

        '登録Noの設定をするjavaScript
        'javaScriptWord.Append(" var temp = document.getElementById('EntryNo');" _
        '+ "temp.innerHTML = '<strong>" + entryNoWord + "</strong><p>" + inpEntryNo + "</p>';")
        javaScriptWord.Append(" var temp = document.getElementById('EntryNo');" _
                            + "temp.innerHTML = '<strong>" + Server.HtmlEncode(entryNoWord) + "</strong><p>" + Server.HtmlEncode(inpEntryNo) + "</p>';")

        '納車予定時刻の設定をするjavaScript
        'javaScriptWord.Append(" var temp = document.getElementById('EstimatedTime');" _
        '+ "temp.innerHTML = '<strong>" + estimatedTimeWord + "</strong>" + inpEstimatedTime + "';")
        javaScriptWord.Append(" var temp = document.getElementById('EstimatedTime');" _
                            + "temp.innerHTML = '<strong>" + Server.HtmlEncode(estimatedTimeWord) + "</strong>" + Server.HtmlEncode(inpEstimatedTime) + "';")

        '着工計画の設定をするjavaScript
        'javaScriptWord.Append(" var temp = document.getElementById('ConstructionPlan');" _
        '+ "temp.innerHTML = '<strong>" + constructionPlanWord + "</strong>" + inpConstructionPlan + "';")
        javaScriptWord.Append(" var temp = document.getElementById('ConstructionPlan');" _
                            + "temp.innerHTML = '<strong>" + Server.HtmlEncode(constructionPlanWord) + "</strong>" + Server.HtmlEncode(inpConstructionPlan) + "';")

        '着工実績の設定をするjavaScript
        'javaScriptWord.Append(" var temp3 = document.getElementById('ConstructionResults');" _
        '+ "temp3.innerHTML = '<strong>" + constructionResultsWord + "</strong>" + inpConstructionResults + "';")
        javaScriptWord.Append(" var temp3 = document.getElementById('ConstructionResults');" _
                              + "temp3.innerHTML = '<strong>" + Server.HtmlEncode(constructionResultsWord) + "</strong>" + Server.HtmlEncode(inpConstructionResults) + "';")

        '着工実績の設定をするjavaScript
        'javaScriptWord.Append(" var temp4 = document.getElementById('Completed');" _
        '+ "temp4.innerHTML = '<strong>" + Server.HtmlEncode(completedWord) + "</strong>" + Server.HtmlEncode(inpCompleted) + "';")
        javaScriptWord.Append(" var temp4 = document.getElementById('Completed');" _
                               + "temp4.innerHTML = '<strong>" + Server.HtmlEncode(completedWord) + "</strong>" + Server.HtmlEncode(inpCompleted) + "';")
        ' 2018/05/02 NSK  井本 車両番号のシングルクォーテーションによるエラー対応 END

        'javaScript実行
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "value", javaScriptWord.ToString, True)

        Logger.Info("ScreenSaverValue.E")

    End Sub

    ''' <summary>
    ''' スクリーンセイバーの背景を変更する
    ''' </summary>
    ''' <param name="inpCompleted">完了計画</param>
    ''' <param name="inpconstructionPlan">着工計画</param>
    ''' <param name="inpConstructionResults">着工実績</param>
    ''' <param name="inpInstruct">着工指示</param>
    ''' <remarks></remarks>
    Private Sub BackGroundChange(ByVal inpCompleted As Date, _
                                 ByVal inpconstructionPlan As Date, _
                                 ByVal inpConstructionResults As String, _
                                 ByVal inpInstruct As String)

        Logger.Info("BackGroundChange.S")

        '変数宣言、初期値は「standby」
        Dim bgimage As String = "standby"

        '「完了計画」がnullの時
        If Not inpCompleted = Date.ParseExact(MINDATE, "yyyy/MM/dd H:mm:ss", CultureInfo.CurrentCulture()) Then

            '現在の時刻取得
            Dim dtNow As DateTime
            dtNow = DateTime.Now

            ' 2013/12/12　TMEJ 成澤　IT9611_次世代サービス 工程管理機能開発 START
            'Dim intCompleted As Date
            'Dim intConstructionPlan As Date

            '「completed」をDate型にキャスト
            'intCompleted = Date.Parse(inpCompleted, CultureInfo.CurrentCulture())
            'intConstructionPlan = Date.Parse(inpconstructionPlan, CultureInfo.CurrentCulture())

            ' 2013/12/12　TMEJ 成澤　IT9611_次世代サービス 工程管理機能開発 END

            '完了計画が現在の時刻を越えている場合
            If (dtNow > inpCompleted) Then

                '背景をlagに
                bgimage = "lag"

                '着工指示が"2"で、
                '着工計画が現在の時刻を越えており、
                '着工実績がない場合
            ElseIf (inpInstruct.Equals("2")) And _
                   (inpConstructionResults.Equals("--:--")) Then

                If (inpconstructionPlan < dtNow) Then

                    '背景をwaitに
                    bgimage = "wait"
                Else
                    '背景をstandbyに
                    bgimage = "standby"
                End If

            Else

                '背景をnormalに
                bgimage = "normal"

            End If

            '着工指示が"0"場合
        ElseIf inpInstruct.Equals("0") Then
            '背景をstandbyに
            bgimage = "standby"

        End If

        '背景のイメージを変更するjavaScript
        ScriptManager.RegisterStartupScript(Me, _
                                            Me.GetType, _
                                            "client", _
                                            " document.getElementById('bgImages').className = '" + bgimage + "';", _
                                            True)

        Logger.Info("BackGroundChange.E")

    End Sub

    ''' <summary>
    ''' 隠しフィールドに値を格納
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub HiddenFieldValue()

        Logger.Info("HiddenFieldValue.S")

        '変数宣言
        Dim refreshTime As Integer
        'リフレッシュタイム格納
        refreshTime = SetRefreshTime()
        '隠しフィールドに格納
        Me.HiddenRefreshTime.Value = refreshTime.ToString(CultureInfo.CurrentCulture())

        Dim sysEnv As New SystemEnvSetting
        '時間取得
        Me.MstPG_RefreshTimerTime.Value = sysEnv.GetSystemEnvSetting("REFRESH_TIMER_TIME").PARAMVALUE

        'メッセージ取得
        Me.MstPG_RefreshTimerMessage1.Value = WebWordUtility.GetWord("MASTERPAGEMAIN", 21)


        Logger.Info("HiddenFieldValue.E")

    End Sub

    ''' <summary>
    ''' ログインアカウントよりストール情報を取得し、メンバ変数に格納する
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetStallInfo()

        Logger.Info("SetStallInfo.S")

        'ストール情報の取得.
        Dim stallDataTable As SC3150201DataSet.SC3150201StallInfoDataTable
        stallDataTable = businessLogic.GetStallData(stallId)
        'ストール時間に初期値を格納する.
        Me.stallActualStartTime = DateTimeFunc.Now().Date
        Me.stallActualEndTime = Me.stallActualStartTime.AddDays(1)

        'ストール情報を設定.
        For Each eachStallData As DataRow In stallDataTable

            '2013/12/12 TMEJ 成澤　【開発】IT9611_次世代サービス 工程管理機能開発 START
            'Me.stallId = CType(eachStallData("STALLID"), Integer) 'ストールID
            Me.stallId = CType(eachStallData("STALLID"), Decimal) 'ストールID
            '2013/12/12 TMEJ 成澤　【開発】IT9611_次世代サービス 工程管理機能開発 END

            Me.stallActualStartTime = ExchangeStallHourToDate(CType(eachStallData("PSTARTTIME"), String)) '作業開始時間
            Me.stallActualEndTime = ExchangeStallHourToDate(CType(eachStallData("PENDTIME"), String)) '作業終了時間

            'ストール時間が、開始時間より終了時間が小さくなってしまう場合、終了時間に1日加算する.
            If (Me.stallActualEndTime < Me.stallActualStartTime) Then

                Me.stallActualEndTime = Me.stallActualEndTime.AddDays(1)
            End If

        Next
        Logger.Info("SetStallInfo.E")

    End Sub

    ''' <summary>
    ''' ストール時間を取得し、Date型に変換する
    ''' </summary>
    ''' <param name="stallHour">5桁の（HH:mm）形式の時間</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ExchangeStallHourToDate(ByVal stallHour As String) As Date

        Logger.Info("ExchangeStallHourToDate.S param1:" + stallHour)

        '2013/12/12 TMEJ 成澤　【開発】IT9611_次世代サービス 工程管理機能開発 START
        ''返す値の初期値として、当日の0時を設定する.
        'Dim stallDate As Date = DateTime.Now 'DateTimeFunc.Now(objStaffContext.DlrCD).Date

        'Dim stallDateString As New System.Text.StringBuilder

        ''当日日付を追加
        'stallDateString.Append(DateTimeFunc.FormatDate(DATE_CONVERT_ID_YYYY_MM_DD, stallDate))
        'stallDateString.Append(" ")
        'stallDateString.Append(stallHour.Substring(0, 5))

        ''生成した文字列を使用して、日付型データを取得する.
        'stallDate = DateTimeFunc.FormatString("yyyy/MM/dd HH:mm", stallDateString.ToString())

        objStaffContext = StaffContext.Current

        '時間と分に分割する
        Dim hourUnit As String() = stallHour.Split(":"c)

        '返す値の初期値として、当日の0時を設定する.
        Dim stallDate As Date = DateTimeFunc.Now(objStaffContext.DlrCD).Date

        '分割した時間と分を本日の日付に足す
        stallDate = stallDate.AddHours(Double.Parse(hourUnit(0)))
        stallDate.AddMinutes(Double.Parse(hourUnit(1)))
        '2013/12/12 TMEJ 成澤　【開発】IT9611_次世代サービス 工程管理機能開発 END

        Logger.Info("ExchangeStallHourToDate.E return" + DateTimeFunc.FormatDate(DATE_CONVERT_ID_YYYYMMDDHHMM, stallDate))
        Return stallDate

    End Function

    ''' <summary>
    ''' ログインユーザーのストールIDを元に、チップの実績情報を取得
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetCandidateChipInfo()
        Logger.Info("GetCandidateChipInfo.S")

        '変数宣言
        Dim entryNo As String = ""
        Dim estimatedTime As String = ""
        Dim constructionPlan As String = ""
        Dim constructionResults As String = ""
        Dim completed As String = ""
        Dim instruct As String = ""
        ' 2013/12/12　TMEJ 成澤　IT9611_次世代サービス 工程管理機能開発 START
        Dim resultEndTime As Date = Date.ParseExact(MINDATE, "yyyy/MM/dd H:mm:ss", CultureInfo.CurrentCulture())
        Dim scheduleStartTime As Date = Date.ParseExact(MINDATE, "yyyy/MM/dd H:mm:ss", CultureInfo.CurrentCulture())
        ' 2013/12/12　TMEJ 成澤　IT9611_次世代サービス 工程管理機能開発 END

        '予約・実績チップデータセットを取得する.
        Dim dtChipInfo As SC3150201DataSet.SC3150201ChipInfoDataTable
        dtChipInfo = businessLogic.GetWorkData(Me.stallId, Me.stallActualStartTime)

        '予約・実績チップ情報がない場合
        If dtChipInfo.Rows.Count = 0 Then
            entryNo = ""
            estimatedTime = "--:--"
            constructionPlan = "--:--"
            constructionResults = "--:--"
            completed = "--:--"
            instruct = ""
        Else
            ''取得した予約・実績チップのデータセットをループ処理する.
            'For Each eachData As SC3150201DataSet.SC3150201ChipInfoRow In dtChipInfo.Rows

            '    '----------------------------------------------------------------
            '    '「登録No」が" "の場合
            '    If eachData("VCLREGNO").Equals(" ") Then
            '        '空白を格納
            '        entryNo = ""
            '    Else
            '        '「登録No」格納
            '        entryNo = CType(eachData("VCLREGNO"), String)
            '    End If
            '    '----------------------------------------------------------------

            '    '----------------------------------------------------------------
            '    '「着工指示区分」が空文字列の場合
            '    If eachData("INSTRUCT").Equals(String.Empty) Then
            '        '"0"を格納
            '        instruct = "0"
            '    Else
            '        '「着工指示区分」格納
            '        instruct = CType(eachData("INSTRUCT"), String)
            '    End If
            '    '----------------------------------------------------------------

            '    '----------------------------------------------------------------
            '    '「納車予定時刻」が空文字列の場合
            '    If eachData("REZ_DELI_DATE").Equals(String.Empty) Then
            '        '「"--:--"」を格納
            '        estimatedTime = "--:--"
            '    Else
            '        '「納車予定時刻」取得
            '        Dim strEstimatedTime As String = CType(eachData("REZ_DELI_DATE"), String)
            '        Dim dateEstimatedTime As Date

            '        'String型をDate型にキャスト
            '        dateEstimatedTime = DateTime.ParseExact(strEstimatedTime, "yyyyMMddHHmm", Nothing)
            '        '「納車予定時刻」格納
            '        estimatedTime = ExchangeTime(dateEstimatedTime)
            '    End If
            '    '----------------------------------------------------------------

            '    '----------------------------------------------------------------
            '    '「着工実績」がDBNullの場合
            '    If IsDBNull(eachData("RESULT_START_TIME")) Then
            '        '「"--:--"」を格納
            '        constructionResults = "--:--"
            '    Else
            '        '「着工実績」取得
            '        Dim strConstructionResults As String = CType(eachData("RESULT_START_TIME"), String)
            '        Dim dateConstructionResults As Date

            '        'String型をDate型にキャスト
            '        dateConstructionResults = DateTime.ParseExact(strConstructionResults, "yyyyMMddHHmm", Nothing)
            '        '「着工実績」格納
            '        constructionResults = ExchangeTime(dateConstructionResults)
            '    End If
            '    '----------------------------------------------------------------

            '    '----------------------------------------------------------------
            '    '「着工計画」がDBNullの場合
            '    If IsDBNull(eachData("STARTTIME")) Then
            '        '「"--:--"」を格納
            '        constructionPlan = "--:--"
            '    Else
            '        '「着工計画」取得
            '        Dim dateConstructionPlan As Date = CType(eachData("STARTTIME"), Date)
            '        '「着工計画」格納
            '        constructionPlan = ExchangeTime(dateConstructionPlan)
            '    End If
            '    '----------------------------------------------------------------

            '    '----------------------------------------------------------------
            '    '「完了計画」がDBNullの場合
            '    If IsDBNull(("ENDTIME")) Then
            '        '「"--:--"」を格納
            '        completed = "--:--"
            '    Else
            '        '「完了計画」取得
            '        Dim dateCompleted As Date = CType(eachData("ENDTIME"), Date)
            '        '「完了計画」格納
            '        completed = ExchangeTime(dateCompleted)
            '    End If
            '    '----------------------------------------------------------------

            'Next

            Dim drChipInfo As SC3150201DataSet.SC3150201ChipInfoRow = _
                DirectCast(dtChipInfo.Rows(0), SC3150201DataSet.SC3150201ChipInfoRow)

            '取得した予約・実績チップのデータを格納する
            '----------------------------------------------------------------
            '「登録No」が" "の場合
            If String.IsNullOrEmpty(drChipInfo.VCLREGNO) OrElse _
                drChipInfo.VCLREGNO.Equals(" ") Then
                '空白を格納
                entryNo = ""
            Else
                '「登録No」格納
                entryNo = drChipInfo.VCLREGNO
            End If
            '----------------------------------------------------------------

            '----------------------------------------------------------------
            '「着工指示区分」が空文字列の場合
            If String.IsNullOrEmpty(drChipInfo.INSTRUCT) Then
                '"0"を格納
                instruct = "0"
            Else
                '「着工指示区分」格納
                instruct = drChipInfo.INSTRUCT
            End If
            '----------------------------------------------------------------

            '----------------------------------------------------------------
            '「納車予定時刻」が空文字列の場合
            If (drChipInfo.IsREZ_DELI_DATENull) OrElse _
                (drChipInfo.REZ_DELI_DATE = Date.MinValue) Then
                '「"--:--"」を格納
                estimatedTime = "--:--"
            Else
                '「納車予定時刻」格納
                estimatedTime = ExchangeTime(drChipInfo.REZ_DELI_DATE)
            End If
            '----------------------------------------------------------------

            '----------------------------------------------------------------
            '「着工実績」がDBNullの場合
            If (drChipInfo.IsRESULT_START_TIMENull) OrElse _
                (drChipInfo.RESULT_START_TIME = Date.MinValue) Then
                '「"--:--"」を格納
                constructionResults = "--:--"
            Else
                '「着工実績」格納
                constructionResults = ExchangeTime(drChipInfo.RESULT_START_TIME)
            End If
            '----------------------------------------------------------------

            '----------------------------------------------------------------
            '「着工計画」がDBNullの場合
            If (drChipInfo.IsSTARTTIMENull) OrElse _
                (drChipInfo.STARTTIME = Date.ParseExact(MINDATE, "yyyy/MM/dd H:mm:ss", CultureInfo.CurrentCulture())) Then
                '「"--:--"」を格納
                constructionPlan = "--:--"
            Else
                '「着工計画」格納
                constructionPlan = ExchangeTime(drChipInfo.STARTTIME)
                scheduleStartTime = drChipInfo.STARTTIME
            End If
            '----------------------------------------------------------------

            '----------------------------------------------------------------
            '「完了計画」がDBNullの場合
            If (drChipInfo.IsENDTIMENull) OrElse _
                (drChipInfo.ENDTIME = Date.ParseExact(MINDATE, "yyyy/MM/dd H:mm:ss", CultureInfo.CurrentCulture())) Then
                '「"--:--"」を格納
                completed = "--:--"
            Else
                '「完了計画」格納
                completed = ExchangeTime(drChipInfo.ENDTIME)
                resultEndTime = drChipInfo.ENDTIME
            End If
            '----------------------------------------------------------------
        End If

        '着工指示区分が"0"で、着工実績が"--:--"の場合
        If (instruct.Equals("0")) And (constructionResults.Equals("--:--")) Then

            '「"--:--"」を格納
            constructionPlan = "--:--"
            constructionResults = "--:--"
            completed = "--:--"

            resultEndTime = Date.ParseExact(MINDATE, "yyyy/MM/dd H:mm:ss", CultureInfo.CurrentCulture())
            scheduleStartTime = Date.ParseExact(MINDATE, "yyyy/MM/dd H:mm:ss", CultureInfo.CurrentCulture())
        End If

        ScreenSaverValue(entryNo, estimatedTime, constructionPlan, constructionResults, completed)

        ' 2013/12/12　TMEJ 成澤　IT9611_次世代サービス 工程管理機能開発 START
        'BackGroundChange(completed, constructionPlan, constructionResults, instruct)
        BackGroundChange(resultEndTime, scheduleStartTime, constructionResults, instruct)
        ' 2013/12/12　TMEJ 成澤　IT9611_次世代サービス 工程管理機能開発 END

        Logger.Info("GetSelectedChipInfo.E")

    End Sub

    ''' <summary>
    ''' 引数に応じて、日付か時刻に変換し、String型で返す
    ''' </summary>
    ''' <param name="inpParam">Date型時刻</param>
    ''' <returns>String型時刻</returns>
    ''' <remarks></remarks>
    Private Function ExchangeTime(ByVal inpParam As Date) As String

        Logger.Info("ExchangeTime.S")
        '変数宣言
        Dim dtNow As DateTime
        Dim outParam As String

        '現在の日付を取得する
        dtNow = DateTime.Today

        '引数の日付が現在の日付と同日なら
        If dtNow = inpParam.Date Then
            '時刻を表示
            outParam = DateTimeFunc.FormatDate(14, inpParam, Nothing)
        Else
            '日付を表示
            outParam = DateTimeFunc.FormatDate(11, inpParam, Nothing)

        End If
        Logger.Info("ExchangeTime.E")
        Return outParam

    End Function

    ''' <summary>
    ''' 画面リフレッシュ時間取得
    ''' </summary>
    ''' <returns>リフレッシュタイム</returns>
    ''' <remarks></remarks>
    Private Function SetRefreshTime() As Integer

        Logger.Info("SetRefreshTime.S")

        '変数宣言、初期値は60秒
        Dim refreshTime As Integer = Default_Refresh_Time

        'リフレッシュタイムのデータセットを取得する
        Dim refreshTimeDataTable As SC3150201DataSet.SC3150201RefreshTimeDataTable
        refreshTimeDataTable = businessLogic.GetRefreshData()

        'DBのカラム数が０ではない場合
        If Not refreshTimeDataTable.Count = &H0 Then
            'DBNULLではない場合
            If Not IsDBNull(refreshTimeDataTable(0)("TCSTATUS_REFRESH_TIME")) Then
                'データセットの内容をキャストして変数に格納
                refreshTime = CType(refreshTimeDataTable(0)("TCSTATUS_REFRESH_TIME"), Integer)
            End If
        End If
        Logger.Info("SetRefreshTime.E")
        Return refreshTime

    End Function

#End Region

End Class

