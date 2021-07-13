'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3290102.ascx.vb
'─────────────────────────────────────
'機能： リマインダー
'補足： 
'作成： 2014/05/30 TMEJ t.nagata
'─────────────────────────────────────

Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SalesManager.IrregularControl.BizLogic
Imports Toyota.eCRB.SalesManager.IrregularControl.DataAccess
Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.SalesManager.IrregularControl.DataAccess.SC3290102DataSet

''' <summary>
''' リマインダー
''' </summary>
''' <remarks></remarks>
Partial Class Pages_SC3290102
    Inherits UserControl


#Region "定数"

    ''' <summary>
    ''' 画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const AppId As String = "SC3290102"

    ''' <summary>
    ''' システム環境設定パラメータ（表示最大件数(N件)）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MaxShowItemsParam As String = "MAX_DISPLAY_ITEMS"

    ''' <summary>
    ''' DB初期値（文字列）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DBDefaultValueString As String = " "

    ''' <summary>
    ''' DB初期値（日付）
    ''' </summary>
    ''' <remarks></remarks>
    Private ReadOnly DBDefaultValueDate As Date = New Date(1900, 1, 1)

    ''' <summary>
    ''' 文言：タイトル
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdLabel_TitleName As String = "1"

    ''' <summary>
    ''' 文言：期限
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdExpirationTitleName As String = "2"

    ''' <summary>
    ''' 文言：スタッフ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdStaffNameTitleName As String = "3"

    ''' <summary>
    ''' 文言：項目
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdIrregularItemName As String = "4"

    ''' <summary>
    ''' 文言：前のN件
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdPreButtonName As String = "5"

    ''' <summary>
    ''' 文言：次のN件
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdNextButtonName As String = "6"

    ''' <summary>
    ''' 文言：読み込み中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdLoading As String = "7"

    ''' <summary>
    ''' 文言：データなし
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdItemNotingLabelName As String = "8"

#End Region


#Region "イベント定義"

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
            Logger.Info("Page_Load_End PostBack")
            Return
        End If

        Dim sysEnvSet As New SystemEnvSetting

        ' 表示最大件数をシステム環境設定より取得し、セッションに保持する
        Dim sysEnvSetMaxItemsRow As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow = Nothing
        Logger.Info("Page_Load_001" & "Call_Start GetSystemEnvSetting Param[" & MaxShowItemsParam & "]")
        sysEnvSetMaxItemsRow = sysEnvSet.GetSystemEnvSetting(MaxShowItemsParam)
        Logger.Info("Page_Load_001" & "Call_Start GetSystemEnvSetting Param[" & IsDBNull(sysEnvSetMaxItemsRow) & "]")
        Me.SC3290102_MaxItemsField.Value = sysEnvSetMaxItemsRow.PARAMVALUE

        ' 文言設定
        Me.SetWord()

        '取得開始行と終了行の初期設定
        Me.SC3290102_GetBeginLineField.Value = "1"
        Me.SC3290102_GetEndLineField.Value = Me.SC3290102_MaxItemsField.Value

        Logger.Info("Page_Load_End")

    End Sub

    ''' <summary>
    ''' スピンアイコン表示時の初期化処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub LoadSpinButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) _
        Handles SC3290102_LoadSpinButton.Click

        Logger.Info("LoadSpinButton_Click Start")

        'フォロー一覧の表示()

        Me.ShowIrregularFollowList()

        Logger.Info("LoadSpinButton_Click End")

    End Sub

    ''' <summary>
    ''' 前のN件ボタン
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub HidePreButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) _
        Handles SC3290102_HidePreButton.Click

        Logger.Info("HidePreButton_Click Start")

        ' 取得開始／終了行を変更
        Me.SC3290102_GetBeginLineField.Value = (Integer.Parse(SC3290102_GetBeginLineField.Value) - Integer.Parse(SC3290102_MaxItemsField.Value)).ToString
        Me.SC3290102_GetEndLineField.Value = (Integer.Parse(SC3290102_GetEndLineField.Value) - Integer.Parse(SC3290102_MaxItemsField.Value)).ToString

        'フォロー一覧の表示
        Me.ShowIrregularFollowList()

        Logger.Info("HidePreButton_Click End")

    End Sub

    ''' <summary>
    ''' 次のN件ボタン
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub HideNextButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) _
        Handles SC3290102_HideNextButton.Click

        Logger.Info("HideNextButton_Click Start")

        ' 取得開始／終了行を変更
        Me.SC3290102_GetBeginLineField.Value = (Integer.Parse(SC3290102_GetBeginLineField.Value) + Integer.Parse(SC3290102_MaxItemsField.Value)).ToString
        Me.SC3290102_GetEndLineField.Value = (Integer.Parse(SC3290102_GetEndLineField.Value) + Integer.Parse(SC3290102_MaxItemsField.Value)).ToString

        'フォロー一覧の表示
        Me.ShowIrregularFollowList()

        Logger.Info("HideNextButton_Clic End")

    End Sub

#End Region


#Region "非公開メソッド"

    ''' <summary>
    ''' 文言をセットする
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetWord()

        Logger.Info("SetWord_Start")

        ' 文言の設定
        Me.SC3290102_Label_Title.Text = Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordIdLabel_TitleName))
        Me.SC3290102_ExpirationTitle.Text = Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordIdExpirationTitleName))
        Me.SC3290102_StaffNameTitle.Text = Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordIdStaffNameTitleName))
        Me.SC3290102_IrregularItemNameTitle.Text = Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordIdIrregularItemName))
        Me.SC3290102_PreButtonName.Text = Server.HtmlEncode(String.Format(CultureInfo.InvariantCulture, _
                                                                          WebWordUtility.GetWord(AppId, WordIdPreButtonName), Me.SC3290102_MaxItemsField.Value))
        Me.SC3290102_NextButtonName.Text = Server.HtmlEncode(String.Format(CultureInfo.InvariantCulture, _
                                                                          WebWordUtility.GetWord(AppId, WordIdNextButtonName), Me.SC3290102_MaxItemsField.Value))
        Me.SC3290102_LoadingName1.Text = Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordIdLoading))
        Me.SC3290102_LoadingName2.Text = Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordIdLoading))
        Me.SC3290102_ItemNotingLabel.Text = Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordIdItemNotingLabelName))

        Logger.Info("SetWord_End")

    End Sub

    ''' <summary>
    ''' フォロー一覧の表示
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ShowIrregularFollowList()

        Logger.Info("ShowIrregularFollowList_Start")

        ' ログインスタッフ情報
        Logger.Info("ShowIrregularFollowList_001 " & "Call_Start StaffContext.Current")
        Dim loginStaff As StaffContext = StaffContext.Current
        Logger.Info("ShowIrregularFollowList_001 " & "Call_End   StaffContext.Current")

        'ビジネスロジックインスタンス
        Dim businessLogic = New SC3290102BusinessLogic

        Dim targetFollowListCountRow As SC3290102FollowListCountRow

        Using dataSet As SC3290102FollowListCountDataTable _
            = businessLogic.GetIrregularFollowListCount(loginStaff.DlrCD, _
                                                        loginStaff.BrnCD, _
                                                        loginStaff.Account)
            'フォロー一覧項目数を取得
            targetFollowListCountRow = dataSet.Rows(0)
            Me.SC3290102_ItemsField.Value = targetFollowListCountRow.FOLLOWLISTCOUNT.ToString()

        End Using

        If 0 < targetFollowListCountRow.FOLLOWLISTCOUNT Then

            'ページ番号で最終ページの考慮
            If targetFollowListCountRow.FOLLOWLISTCOUNT < SC3290102_GetBeginLineField.Value And Not Integer.Parse(SC3290102_GetBeginLineField.Value) = 1 Then
                ' 取得開始／終了行を変更
                Me.SC3290102_GetBeginLineField.Value = (Integer.Parse(SC3290102_GetBeginLineField.Value) - Integer.Parse(SC3290102_MaxItemsField.Value)).ToString
                Me.SC3290102_GetEndLineField.Value = (Integer.Parse(SC3290102_GetEndLineField.Value) - Integer.Parse(SC3290102_MaxItemsField.Value)).ToString
            End If

            ' フォロー一覧の取得
            Using dataSet As SC3290102FollowListDataTable _
                = businessLogic.GetIrregularFollowList(loginStaff.DlrCD, loginStaff.BrnCD, _
                                                       loginStaff.Account, Integer.Parse(SC3290102_GetBeginLineField.Value), _
                                                       Integer.Parse(SC3290102_GetEndLineField.Value))
                'リピーターに情報をセット
                Me.SC3290102_FollwListRepeater.DataSource = dataSet
                Me.SC3290102_FollwListRepeater.DataBind()

                ' 指定された開始行と終了行を表示する
                For i = 0 To SC3290102_FollwListRepeater.Items.Count - 1

                    Dim follow As Control = SC3290102_FollwListRepeater.Items(i)
                    Dim targetFollowListRow As SC3290102FollowListRow = dataSet.Rows(i)

                    '情報を表示する
                    Me.ShowIrregularFollowListRow(follow, targetFollowListRow)
                Next

            End Using

        End If

        ' 表示完了処理
        JavaScriptUtility.RegisterStartupFunctionCallScript(Me.Page, "initDisplaySC3290102", "startup")

        Logger.Info("ShowIrregularFollowList_End")

    End Sub

    ''' <summary>
    ''' フォロー一覧の行表示処理
    ''' </summary>
    ''' <param name="followListControl">HTMLコントロール</param>
    ''' <param name="targetFollowListRow">フォロー一覧の行</param>
    ''' <remarks></remarks>
    Private Sub ShowIrregularFollowListRow(ByVal followListControl As Control, ByVal targetFollowListRow As SC3290102FollowListRow)

        ' 期限
        Dim expiration As String = "-"
        ' スタッフ名
        Dim staffName As String = "-"
        ' 異常項目名
        Dim irregularItemName As String = "-"

        If targetFollowListRow.FLLW_EXPR_DATE <> DBDefaultValueDate Then
            expiration = DateTimeFunc.FormatDate(11, targetFollowListRow.FLLW_EXPR_DATE)
        End If

        If Not String.IsNullOrEmpty(targetFollowListRow.STF_NAME) AndAlso targetFollowListRow.STF_NAME <> DBDefaultValueString Then
            staffName = targetFollowListRow.STF_NAME
        End If

        If Not String.IsNullOrEmpty(targetFollowListRow.IRREG_ITEM_NAME) AndAlso targetFollowListRow.IRREG_ITEM_NAME <> DBDefaultValueString Then
            irregularItemName = targetFollowListRow.IRREG_ITEM_NAME
        End If

        ' フォロー期日遅れの場合の文字色
        If Date.Compare(targetFollowListRow.FLLW_EXPR_DATE, DateTimeFunc.Now.ToShortDateString()) = -1 Then
            CType(followListControl.FindControl("SC3290102_ExpirationLiteral"), Label).Text = "<span class=""ExpirationRed"">" + Server.HtmlEncode(expiration) + "</span>"
        Else
            CType(followListControl.FindControl("SC3290102_ExpirationLiteral"), Label).Text = "<span class=""ExpirationBlack"">" + Server.HtmlEncode(expiration) + "</span>"
        End If

        CType(followListControl.FindControl("SC3290102_StaffNameLiteral"), Label).Text = Server.HtmlEncode(staffName)
        CType(followListControl.FindControl("SC3290102_IrregularItemNameLiteral"), Label).Text = Server.HtmlEncode(irregularItemName)

        ' 隠し項目の設定
        CType(followListControl.FindControl("IrregFllwId"), HiddenField).Value = targetFollowListRow.IRREG_FLLW_ID


    End Sub

#End Region

End Class