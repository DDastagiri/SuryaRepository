'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3100103.aspx.vb
'──────────────────────────────────
'機能： スタンバイスタッフ並び順変更
'補足： 
'作成： 2012/08/17 TMEJ m.okamura
'──────────────────────────────────

Imports Toyota.eCRB.Visit.ReceptionistMain.BizLogic
Imports Toyota.eCRB.Visit.ReceptionistMain.DataAccess.SC3100103DataSet
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Globalization

''' <summary>
''' SC3100103
''' スタンバイスタッフ並び順変更 プレゼンテーション層クラス
''' </summary>
''' <remarks></remarks>
Partial Class Pages_SC3100103
    Inherits BasePage

#Region "定数"

    ''' <summary>
    ''' 機能ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const AppId As String = "SC3100103"

    ''' <summary>
    ''' セッションキー（スタッフ写真用パス）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyStaffPhotoPath As String = "staffPhotoPath"

    ''' <summary>
    ''' スタッフ写真用パスの先頭に設定する文字列
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StaffPhotoPathPrefix As String = "~/"

    ''' <summary>
    ''' デフォルトアイコン（顧客、スタッフ）のファイルパス
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DefaultIcon As String = "Styles/Images/VisitCommon/silhouette_person01.png"

    ''' <summary>
    ''' 値がない場合の設定値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DataNull As String = "-"

    ''' <summary>
    ''' 時間の値が存在しない場合の設定値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NothingDate As String = "--:--"

    ''' <summary>
    ''' 当日実績組数がない場合の設定値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DefaultResultCount As Short = 0

#Region "表示文言"

    ''' <summary>
    ''' スタッフ名ヘッダ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdStaffNameHead As Integer = 4

    ''' <summary>
    ''' スタッフの当日商談実績組数ヘッダ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdResultCountHead As Integer = 5

    ''' <summary>
    ''' スタッフの当日商談終了時間ヘッダ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdMaxSalesEndHead As Integer = 6

    ''' <summary>
    ''' スタッフの当日商談実績組数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdPer As Integer = 7

    ''' <summary>
    ''' スタンバイ中のスタッフが存在しない時のメッセージ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NotExistStandByStaff As Integer = 8

#End Region

#End Region

#Region "イベント処理"

#Region "ページロード時の処理"

    ''' <summary>
    ''' ページロード時の処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Logger.Info("Page_Load_Start")

        ' PostBack時、初期表示処理は行わない。
        If Me.IsPostBack Then

            Me.LoadSpinPanel.Visible = False
            Logger.Info("Page_Load_End PostBack")
            Return

        End If

        Me.LoadSpinPanel.Visible = True
        Me.NotStandByStaffPanel.Visible = False
        Me.StandByStaffPanel.Visible = False

        Me.StandByStaffErrorMessage.Value = "0"

        Logger.Info("Page_Load_End")

    End Sub

#End Region

#Region "スピンアイコン表示時の初期化処理"

    ''' <summary>
    ''' スピンアイコン表示時の初期化処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub LoadSpinButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LoadSpinButton.Click

        Logger.Info("LoadSpinButton_Click_Start")

        Me.PageInit()

        Logger.Info("LoadSpinButton_Click_End")

    End Sub

#End Region

#Region "登録ボタン押下イベント"

    ''' <summary>
    ''' 登録ボタン押下処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub RegisterButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RegisterButton.Click

        Logger.Info("RegisterButton_Click_Start")

        'ログイン情報チェック
        Logger.Info("RegisterButton_Click_001 " & "Call_Start StaffContext.Current")
        Dim loginStaff As StaffContext = StaffContext.Current
        Logger.Info("RegisterButton_Click_001 " & "Call_End   StaffContext.Current")

        Using insertStandByStaffSort As New SC3100103StandByStaffSortDataTable

            Dim i As Integer = 0

            '登録データを追加する
            For Each item As RepeaterItem In Me.StandByStaffRepeater.Items

                'レコードを追加する
                Dim insertRows As SC3100103StandByStaffSortRow = insertStandByStaffSort.NewSC3100103StandByStaffSortRow
                insertRows.DLRCD = loginStaff.DlrCD
                insertRows.STRCD = loginStaff.BrnCD
                insertRows.ACCOUNT = CType(item.FindControl("Account"), HiddenField).Value
                insertRows.PRESENCECATEGORYDATE = CDate(CType(item.FindControl("PresenceCategoryDate"), HiddenField).Value)
                insertRows.SORTNO = i

                insertStandByStaffSort.Rows.Add(insertRows)

                Logger.Info("RegisterButton_Click_002 " & "Next")
                i += 1

            Next

            'スタッフ並び順の登録
            Dim businessLogic As New SC3100103BusinessLogic
            Me.StandByStaffErrorMessage.Value = businessLogic.RegistStaffSort(loginStaff.DlrCD, loginStaff.BrnCD, loginStaff.Account, insertStandByStaffSort)

        End Using

        ' 返却メッセージID
        Dim messageId As Integer = CInt(Me.StandByStaffErrorMessage.Value)

        '結果を返却
        If messageId <> 0 Then

            Logger.Info("RegisterButton_Click_003  Me.MessageId <> 0 ")

            '対応のエラーメッセージを表示し、画面を再描画
            Me.ShowMessageBox(messageId, WebWordUtility.GetWord(AppId, messageId))
            Me.PageInit()

        End If

        Logger.Info("RegisterButton_Click_End")

    End Sub

#End Region

#End Region

#Region "非公開メソッド"

#Region "初期化処理"

    ''' <summary>
    ''' 初期化処理
    ''' </summary>
    ''' <remarks>件数をチェックし、表示する内容を決める</remarks>
    Private Sub PageInit()

        Logger.Info("PageInit_Start")

        'ログイン情報チェック
        Logger.Info("PageInit_001 " & "Call_Start StaffContext.Current")
        Dim loginStaff As StaffContext = StaffContext.Current
        Logger.Info("PageInit_001 " & "Call_End   StaffContext.Current")

        ' 日付管理(現在日付の取得)
        Logger.Debug("PageInit_002 " & "Call_Start DateTimeFunc.Now Param[" & loginStaff.DlrCD & "]")
        Dim now As Date = DateTimeFunc.Now(loginStaff.DlrCD)
        Logger.Debug("PageInit_002 " & "Call_End   DateTimeFunc.Now Ret[" & now & "]")

        '初期表示データ取得
        Dim businessLogic As New SC3100103BusinessLogic

        '表示に使う情報を設定するDataTable
        Using dataSet As SC3100103TargetStaffDataTable = businessLogic.GetTargetStaff(loginStaff.DlrCD, loginStaff.BrnCD, now)

            '結果の件数チェック
            If dataSet Is Nothing OrElse dataSet.Count <= 0 Then

                '0件の場合のパネルを表示
                Logger.Info("PageInit_003 Count = 0")
                Me.NotStandByStaffStatus.Text = Server.HtmlEncode(WebWordUtility.GetWord(AppId, NotExistStandByStaff))
                Me.NotStandByStaffPanel.Visible = True
                Me.StandByStaffPanel.Visible = False

                Return

            End If

            Logger.Info("PageInit_004 Count > 0")

            ' ヘッダ文言を取得
            Me.UserNameHeadLiteral.Text = Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordIdStaffNameHead))
            Me.ResultCountHeadLiteral.Text = Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordIdResultCountHead))
            Me.MaxSalesEndHeadLiteral.Text = Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordIdMaxSalesEndHead))

            Dim wordPer As String = Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordIdPer))

            'スタッフ写真用のパスを取得
            Logger.Info("PageInit_005 " & "Call_Start MyBase.GetValue Param[" & _
                         ScreenPos.Current & "," & SessionKeyStaffPhotoPath & "," & False & "]")
            Dim staffPhotoPath As String = CType(MyBase.GetValue(ScreenPos.Current, SessionKeyStaffPhotoPath, False), String)
            Logger.Info("PageInit_005 " & "Call_End MyBase.GetValue Ret[" & staffPhotoPath & "]")

            'リピーターに情報をセット
            Me.StandByStaffRepeater.DataSource = dataSet
            Me.StandByStaffRepeater.DataBind()

            ' 件数分表示する
            For i = 0 To StandByStaffRepeater.Items.Count - 1

                Dim staff As Control = StandByStaffRepeater.Items(i)

                If i Mod 2 <> 0 Then

                    CType(staff.FindControl("StaffChipRow"), HtmlGenericControl).Attributes("Class") += " SecondRow"

                End If

                Dim targetStaffRow As SC3100103TargetStaffRow = dataSet.Rows(i)

                '情報を表示する
                Me.ShowStandByStaff(staffPhotoPath, wordPer, staff, targetStaffRow)

            Next

        End Using

        businessLogic = Nothing

        '件数パネルの表示
        Me.NotStandByStaffPanel.Visible = False
        Me.StandByStaffPanel.Visible = True

        Logger.Info("PageInit_End")

    End Sub

#End Region

#Region "初期化処理"

    ''' <summary>
    ''' 初期化処理
    ''' </summary>
    ''' <remarks>表示</remarks>
    Private Sub ShowStandByStaff(ByVal staffPhotoPath As String, _
                                 ByVal wordPer As String, _
                                 ByVal staffControl As Control, _
                                 ByVal targetStaffRow As SC3100103TargetStaffRow)

        Logger.Info("SetStandByStaff_Start")

        ' スタッフ写真ファイル名
        Dim orgImgFile As String = If(targetStaffRow.IsORG_IMGFILENull(), String.Empty, targetStaffRow.ORG_IMGFILE)
        ' スタッフ名
        Dim userName As String = If(targetStaffRow.IsUSERNAMENull(), String.Empty, targetStaffRow.USERNAME)
        ' 当日実績組数
        Dim resultCount As Short = If(targetStaffRow.IsRESULTCOUNTNull(), DefaultResultCount, targetStaffRow.RESULTCOUNT)
        ' 当日最終商談終了日時
        Dim maxSalesEnd As String = If(targetStaffRow.IsMAXSALESENDNull(), NothingDate, targetStaffRow.MAXSALESEND.ToString("HH:mm", CultureInfo.CurrentCulture()))

        ' スタッフ写真の表示
        If String.IsNullOrEmpty(orgImgFile.Trim()) Then
            CType(staffControl.FindControl("OrgImgFileImage"), Image).ImageUrl = StaffPhotoPathPrefix & DefaultIcon
        Else
            CType(staffControl.FindControl("OrgImgFileImage"), Image).ImageUrl = StaffPhotoPathPrefix & staffPhotoPath & orgImgFile
        End If

        ' スタッフ名の表示
        If String.IsNullOrEmpty(userName.Trim()) Then
            CType(staffControl.FindControl("UserNameLiteral"), Literal).Text = DataNull
        Else
            CType(staffControl.FindControl("UserNameLiteral"), Literal).Text = Server.HtmlEncode(userName)
        End If

        ' 当日実績組数の表示
        CType(staffControl.FindControl("ResultCountLiteral"), Literal).Text = CStr(resultCount) & wordPer

        ' 当日最終商談終了日時の表示
        CType(staffControl.FindControl("MaxSalesEndLiteral"), Literal).Text = maxSalesEnd

        Logger.Info("SetStandByStaff_End")

    End Sub

#End Region

#End Region

End Class
