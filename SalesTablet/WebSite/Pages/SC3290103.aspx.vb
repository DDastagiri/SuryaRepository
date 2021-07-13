'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3290103.aspx.vb
'──────────────────────────────────
'機能： 異常詳細画面
'補足： 
'作成： 2014/06/12 TMEJ y.gotoh
'──────────────────────────────────

Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Globalization
Imports Toyota.eCRB.SalesManager.IrregularControl.DataAccess
Imports Toyota.eCRB.SalesManager.IrregularControl.BizLogic

Partial Class Pages_SC3290103
    Inherits BasePage

#Region "定数"

    ''' <summary>
    ''' 画面ID
    ''' </summary>
    Private Const DisplayId As String = "SC3290103"

    ''' <summary>
    ''' フォロー完了フラグ：未完了
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CompleteFlgNotcomplete As String = "0"

    ''' <summary>
    ''' フォロー完了フラグ：完了
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CompleteFlgComplete As String = "1"

    ''' <summary>
    ''' 文言：チーム名
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DisplayNoColumNameTeamName As String = "1"

    ''' <summary>
    ''' 文言：スタッフ名
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DisplayNoColumNameStaffName As String = "2"

    ''' <summary>
    ''' 文言：月度目標
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DisplayNoColumNameMonthlyGoal As String = "3"

    ''' <summary>
    ''' 文言：進捗目標
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DisplayNoColumNameProgressGoal As String = "4"

    ''' <summary>
    ''' 文言：実績
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DisplayNoColumNameResult As String = "5"

    ''' <summary>
    ''' 文言：達成率
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DisplayNoColumNameAchievementRate As String = "6"

    ''' <summary>
    ''' 文言：確認
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DisplayNoColumNameConfirmation As String = "7"

    ''' <summary>
    ''' 文言：他のユーザがフォローしています
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DisplayNoAlertMessage As String = "901"

    ''' <summary>
    ''' DB初期値（文字列）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DBDefaultValueString As String = " "

    ''' <summary>
    ''' セッションキー：異常分類コード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const KEY_IRREGULAR_CLASS_CD As String = "IRREGULAR_CLASS_CD"

    ''' <summary>
    ''' セッションキー：異常項目コード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const KEY_IRREGULAR_ITEM_CD As String = "IRREGULAR_ITEM_CD"
#End Region


#Region "非公開変数"
    ''' <summary>
    ''' ページ用マスタページ
    ''' </summary>
    ''' <remarks></remarks>
    Private commonMasterPage As CommonMasterPage

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

        ' フッターの設定
        If Not ScriptManager.IsInAsyncPostBack Then
            Me.InitFooter()
        End If

        ' PostBack時、初期表示処理は行わない。
        If Me.IsPostBack Then
            Logger.Info("Page_Load_End PostBack")
            Return
        End If

        '画面文言の設定
        Me.SetWord()
        'タイトルの設定
        Me.SetTitle()

        'スタッフ情報を取得
        Logger.Info("Page_Load_001 " & "Call_Start StaffContext.Current")
        Dim staffInfo As StaffContext = StaffContext.Current
        Logger.Info("Page_Load_001 " & "Call_End   StaffContext.Current")

        'ログイン中のスタッフコードをhiddenに保持
        Me.LoginStaffCode.Value = staffInfo.Account

        Logger.Info("Page_Load_End")

    End Sub

    ''' <summary>
    ''' スピンアイコン表示時の初期化処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub LoadSpinButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SC3290103_LoadSpinButton.Click

        Logger.Info("SC3290103_LoadSpinButton_Click_Start")

        'スタッフ情報を取得
        Logger.Info("LoadSpinButton_Click_001 " & "Call_Start StaffContext.Current")
        Dim staffInfo As StaffContext = StaffContext.Current
        Logger.Info("LoadSpinButton_Click_001 " & "Call_End   StaffContext.Current")

        Dim irregClassCode As String = DirectCast(Me.GetValue(ScreenPos.Current, KEY_IRREGULAR_CLASS_CD, False), String)
        Dim irregItemCode As String = DirectCast(Me.GetValue(ScreenPos.Current, KEY_IRREGULAR_ITEM_CD, False), String)

        Dim logic As SC3290103BusinessLogic = New SC3290103BusinessLogic
        DateTimeFunc.Now(staffInfo.DlrCD)
        '異常詳細情報一覧の取得
        Dim irregularDetailInfoDataTable As SC3290103DataSet.IrregularDetailInfoDataTable
        irregularDetailInfoDataTable = logic.GetIrregularDetailList(staffInfo.DlrCD, _
                                                                    staffInfo.BrnCD, _
                                                                    irregClassCode, _
                                                                    irregItemCode, _
                                                                    DateTimeFunc.Now(staffInfo.DlrCD))

        Me.IrregularDetailRepeater.DataSource = irregularDetailInfoDataTable
        Me.IrregularDetailRepeater.DataBind()

        ' 件数分表示する
        For i = 0 To IrregularDetailRepeater.Items.Count - 1

            Dim irregularDetail As Control = IrregularDetailRepeater.Items(i)
            Dim irregularDetailInfoRow As SC3290103DataSet.IrregularDetailInfoRow = irregularDetailInfoDataTable.Rows(i)

            '情報を表示する
            Me.ShowIrregularDetailList(irregularDetail, irregularDetailInfoRow)

        Next

        'スクリプトの登録
        JavaScriptUtility.RegisterStartupFunctionCallScript(Me.Page, "showCompleteSC3290103", "startup")

        Logger.Info("SC3290103_LoadSpinButton_Click_End")
    End Sub

    ''' <summary>
    ''' フッターサブメニューの宣言
    ''' </summary>
    ''' <param name="commonMaster">ページ用マスタページ</param>
    ''' <param name="category">自ページの所属メニュー</param>
    ''' <returns>フッターボタンIDの配列</returns>
    ''' <remarks></remarks>
    Public Overrides Function DeclareCommonMasterFooter(ByVal commonMaster As CommonMasterPage, _
                                                        ByRef category As FooterMenuCategory) _
                                                        As Integer()

        Logger.Info("DeclareCommonMasterFooter_Click_Start")

        Me.commonMasterPage = commonMaster

        Logger.Info("DeclareCommonMasterFooter_Click_End")

        Return New Integer() {}

    End Function

#End Region

#Region "非公開メソッド"

    ''' <summary>
    ''' タイトルの設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetTitle()

        Logger.Info("SetTitle_Start")

        Dim irregClassCode As String = DirectCast(Me.GetValue(ScreenPos.Current, KEY_IRREGULAR_CLASS_CD, False), String)
        Dim irregItemCode As String = DirectCast(Me.GetValue(ScreenPos.Current, KEY_IRREGULAR_ITEM_CD, False), String)

        Dim logic As SC3290103BusinessLogic = New SC3290103BusinessLogic

        '異常項目名表示名称取得
        Dim irregularItemDisplayName As String = logic.GetIrregularItemDisplayName(irregClassCode, irregItemCode)


        If Not String.IsNullOrEmpty(irregularItemDisplayName) AndAlso _
            Not DBDefaultValueString.Equals(irregularItemDisplayName) Then

            Me.ColumNameTitle.Text = Server.HtmlEncode(irregularItemDisplayName)
        Else
            Me.ColumNameTitle.Text = "-"
        End If

        Logger.Info("SetTitle_End")
    End Sub


    ''' <summary>
    ''' 文言の設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetWord()

        Logger.Info("SetWord_Start")

        Me.ColumNameTeamName.Text = Server.HtmlEncode(WebWordUtility.GetWord(DisplayId, DisplayNoColumNameTeamName))
        Me.ColumNameStaffName.Text = Server.HtmlEncode(WebWordUtility.GetWord(DisplayId, DisplayNoColumNameStaffName))
        Me.ColumNameMonthlyGoal.Text = Server.HtmlEncode(WebWordUtility.GetWord(DisplayId, DisplayNoColumNameMonthlyGoal))
        Me.ColumNameProgressGoal.Text = Server.HtmlEncode(WebWordUtility.GetWord(DisplayId, DisplayNoColumNameProgressGoal))
        Me.ColumNameResult.Text = Server.HtmlEncode(WebWordUtility.GetWord(DisplayId, DisplayNoColumNameResult))
        Me.ColumNameAchievementRate.Text = Server.HtmlEncode(WebWordUtility.GetWord(DisplayId, DisplayNoColumNameAchievementRate))
        Me.ColumNameConfirmation.Text = Server.HtmlEncode(WebWordUtility.GetWord(DisplayId, DisplayNoColumNameConfirmation))

        ' Hiddenフィールドに保持する文言はHTMLエンコードが自動で行われるためここでは処理しない
        Me.AlertMessage.Value = WebWordUtility.GetWord(DisplayId, DisplayNoAlertMessage)

        Logger.Info("SetWord_End")
    End Sub

    ''' <summary>
    ''' 異常詳細リストの作成
    ''' </summary>
    ''' <param name="irregularDetailControl">HTMLコントロール</param>
    ''' <param name="irregularDetailInfoRow">異常詳細情報</param>
    ''' <remarks>表示</remarks>
    Private Sub ShowIrregularDetailList(ByVal irregularDetailControl As Control, _
                                 ByVal irregularDetailInfoRow As SC3290103DataSet.IrregularDetailInfoRow)

        'チーム名
        Dim orgnzName As String = irregularDetailInfoRow.ORGNZ_NAME.Trim
        If String.IsNullOrEmpty(orgnzName) Then
            orgnzName = "-"
        End If

        'スタッフ名
        Dim stfName As String = irregularDetailInfoRow.STF_NAME.Trim
        If String.IsNullOrEmpty(stfName) Then
            stfName = "-"
        End If

        '月度目標
        Dim monthTarget As String
        If irregularDetailInfoRow.MONTH_TARGET < 0 Then
            monthTarget = "-"
        Else
            monthTarget = irregularDetailInfoRow.MONTH_TARGET.ToString(CultureInfo.CurrentCulture)
        End If

        '進捗目標
        Dim progressTarget As String
        If irregularDetailInfoRow.PROGRESS_TARGET < 0 Then
            progressTarget = "-"
        Else
            progressTarget = irregularDetailInfoRow.PROGRESS_TARGET.ToString(CultureInfo.CurrentCulture)
        End If

        '実績数
        Dim rsltCount As String
        If irregularDetailInfoRow.RSLT_COUNT < 0 Then
            rsltCount = "-"
        Else
            rsltCount = irregularDetailInfoRow.RSLT_COUNT.ToString(CultureInfo.CurrentCulture)
        End If

        '達成率
        Dim achieveRate As String
        If irregularDetailInfoRow.ACHIEVE_RATE < 0 Then
            achieveRate = "-"
        Else
            achieveRate = irregularDetailInfoRow.ACHIEVE_RATE.ToString(CultureInfo.CurrentCulture) + "%"
        End If

        CType(irregularDetailControl.FindControl("LabelTeamName"), Label).Text = Server.HtmlEncode(orgnzName)
        CType(irregularDetailControl.FindControl("LabelStaffName"), Label).Text = Server.HtmlEncode(stfName)
        CType(irregularDetailControl.FindControl("LabelMonthlyGoal"), Label).Text = Server.HtmlEncode(monthTarget)
        CType(irregularDetailControl.FindControl("LabelProgressGoal"), Label).Text = Server.HtmlEncode(progressTarget)
        CType(irregularDetailControl.FindControl("LabelResult"), Label).Text = Server.HtmlEncode(rsltCount)
        CType(irregularDetailControl.FindControl("LabelAchievementRate"), Label).Text = Server.HtmlEncode(achieveRate)

        '確認
        Dim fllwCompleteFlg As String = irregularDetailInfoRow.FLLW_COMPLETE_FLG

        'フォロー完了していて、猶予期日を過ぎている異常はチェックボックスを表示する
        If CompleteFlgNotcomplete.Equals(fllwCompleteFlg) Then

            Dim fllwExprDateConvert As String = DateTimeFunc.FormatDate(11, irregularDetailInfoRow.FLLW_EXPR_DATE.Date)
            irregularDetailControl.FindControl("MgrCheck").Visible = False
            CType(irregularDetailControl.FindControl("FollowDate"), Label).Text = Server.HtmlEncode(fllwExprDateConvert)

            Dim mgrButton01 As HtmlGenericControl = irregularDetailControl.FindControl("MgrButton01")

            'フォロー期日によって背景色を変更する
            If irregularDetailInfoRow.FLLW_EXPR_DATE < Now.Date Then

                mgrButton01.Attributes("class") = "MGR_Button01_Red"
            Else
                mgrButton01.Attributes("class") = "MGR_Button01"
            End If


        Else
            irregularDetailControl.FindControl("MgrButton01").Visible = False
        End If

        'hiddenの設定
        Dim irregClassCode As String = irregularDetailInfoRow.IRREG_CLASS_CD
        Dim irregItemCode As String = irregularDetailInfoRow.IRREG_ITEM_CD
        Dim staffCode As String = irregularDetailInfoRow.STF_CD
        Dim FllwPicStfCode As String = irregularDetailInfoRow.FLLW_PIC_STF_CD

        CType(irregularDetailControl.FindControl("IrregClassCd"), HiddenField).Value = irregClassCode
        CType(irregularDetailControl.FindControl("IrregItemCd"), HiddenField).Value = irregItemCode
        CType(irregularDetailControl.FindControl("StfCd"), HiddenField).Value = staffCode
        CType(irregularDetailControl.FindControl("FllwPicStfCd"), HiddenField).Value = FllwPicStfCode

    End Sub

    ''' <summary>
    ''' フッターエリアの初期化
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitFooter()

        Logger.Info("InitFooter_Start")

        ' メニューボタン
        Logger.Info("InitFooter_001 " & "Call_Start GetFooterButton Param[" & FooterMenuCategory.MainMenu & "]")
        Dim mainManuButton As CommonMasterFooterButton = commonMasterPage.GetFooterButton(FooterMenuCategory.MainMenu)
        Logger.Info("InitFooter_001 " & "Call_End GetFooterButton Ret[" & mainManuButton.ToString & "]")

        AddHandler mainManuButton.Click, _
          Sub()
              Logger.Info("MainManuButton_Click_Start")

              ' SCメインに遷移
              Logger.Info("MainManuButton_Click_001 " & "Call_Start Me.RedirectNextScreen Param[SC3010203]")


              Logger.Info("MainManuButton_Click_End")

              Me.RedirectNextScreen("SC3010203")
          End Sub

        ' ショールームステータスボタン
        Logger.Info("InitFooter_002 " & "Call_Start GetFooterButton Param[" & FooterMenuCategory.ShowRoomStatus & "]")
        Dim submenuShowRoomLink As CommonMasterFooterButton = commonMasterPage.GetFooterButton(FooterMenuCategory.ShowRoomStatus)
        Logger.Info("InitFooter_002 " & "Call_End GetFooterButton Ret[" & submenuShowRoomLink.ToString & "]")

        AddHandler submenuShowRoomLink.Click, _
            Sub()
                Logger.Info("submenuShowRoomLink_Click_Start")

                ' 受付メインに遷移
                Logger.Info("SubmenuShowRoomLink_Click_001 " & "Call_Start Me.RedirectNextScreen Param[SC3100101]")

                Logger.Info("SubmenuShowRoomLink_Click_End")
                Me.RedirectNextScreen("SC3100101")
            End Sub

        ' 顧客ボタン
        Logger.Info("InitFooter_003 " & "Call_Start GetFooterButton Param[" & FooterMenuCategory.Customer & "]")
        Dim customerButton As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.Customer)
        Logger.Info("InitFooter_003 " & "Call_End GetFooterButton Ret[" & customerButton.ToString & "]")

        '非表示にする
        customerButton.Visible = False

        ' TCVボタン
        Logger.Info("InitFooter_004 " & "Call_Start GetFooterButton Param[" & FooterMenuCategory.TCV & "]")
        Dim tcvButton As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.TCV)
        Logger.Info("InitFooter_004 " & "Call_End GetFooterButton Ret[" & tcvButton.ToString & "]")
        AddHandler tcvButton.Click, AddressOf tcvButton_Click

        Logger.Info("InitFooter_End")

        '納車時説明ボタン
        Logger.Info("InitFooter_005 " & "Call_Start GetFooterButton Param[" & FooterMenuCategory.NewCarExplain & "]")
        Dim newCarExplainButton As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.NewCarExplain)
        Logger.Info("InitFooter_005 " & "Call_End GetFooterButton Ret[" & newCarExplainButton.ToString & "]")

        '非表示にする
        newCarExplainButton.Visible = False

    End Sub

    ''' <summary>
    ''' TCSとの連携ボタン
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub tcvButton_Click(ByVal sender As Object, ByVal e As CommonMasterFooterButtonClickEventArgs)

        Logger.Info("tcvButton_Click_Click_Start")

        'スタッフ情報を取得
        Logger.Info("tcvButton_Click_001 " & "Call_Start StaffContext.Current")
        Dim context As StaffContext = StaffContext.Current
        Logger.Info("tcvButton_Click_001 " & "Call_End   StaffContext.Current")

        'TCV機能に渡す引数を設定
        e.Parameters.Add("DataSource", "none")
        Logger.Info("tcvButton_Click Parameters DataSource[" & "none" & "]")
        e.Parameters.Add("MenuLockFlag", False)
        Logger.Info("tcvButton_Click Parameters MenuLockFlag[" & "False" & "]")
        e.Parameters.Add("Account", context.Account)
        Logger.Info("tcvButton_Click Parameters Account[" & context.Account & "]")
        e.Parameters.Add("AccountStrCd", context.BrnCD)
        Logger.Info("tcvButton_Click Parameters AccountStrCd[" & context.BrnCD & "]")
        e.Parameters.Add("DlrCd", context.DlrCD)
        Logger.Info("tcvButton_Click Parameters DlrCd[" & context.DlrCD & "]")
        e.Parameters.Add("StrCd", String.Empty)
        Logger.Info("tcvButton_Click Parameters StrCd[" & String.Empty & "]")
        e.Parameters.Add("FollowupBox_SeqNo", String.Empty)
        Logger.Info("tcvButton_Click Parameters FollowupBox_SeqNo[" & String.Empty & "]")
        e.Parameters.Add("CstKind", String.Empty)
        Logger.Info("tcvButton_Click Parameters CstKind[" & String.Empty & "]")
        e.Parameters.Add("CustomerClass", String.Empty)
        Logger.Info("tcvButton_Click Parameters CustomerClass[" & String.Empty & "]")
        e.Parameters.Add("CRCustId", String.Empty)
        Logger.Info("tcvButton_Click Parameters CRCustId[" & String.Empty & "]")
        e.Parameters.Add("OperationCode", context.OpeCD)
        Logger.Info("tcvButton_Click Parameters OperationCode[" & context.OpeCD & "]")
        e.Parameters.Add("BusinessFlg", False)
        Logger.Info("tcvButton_Click Parameters BusinessFlg[" & "False" & "]")
        e.Parameters.Add("ReadOnlyFlg", False)
        Logger.Info("tcvButton_Click Parameters ReadOnlyFlg[" & "False" & "]")

        Logger.Info("tcvButton_Click_Click_End")

    End Sub

#End Region

End Class
