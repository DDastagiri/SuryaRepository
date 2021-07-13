'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3290104.ascx.vb
'──────────────────────────────────
'機能： フォロー設定
'補足： 
'作成： 2014/06/11 TMEJ t.mizumoto
'──────────────────────────────────

Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SalesManager.IrregularControl.BizLogic
Imports Toyota.eCRB.SalesManager.IrregularControl.DataAccess
Imports System.Globalization

''' <summary>
''' フォロー設定
''' </summary>
''' <remarks></remarks>
Partial Class Pages_SC3290104_Control
    Inherits System.Web.UI.UserControl


#Region "定数"

    ''' <summary>
    ''' 機能ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const AppId As String = "SC3290104"

    ''' <summary>
    ''' DB初期値（数値）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DBDefaultValueNumber As Integer = 0

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
    ''' フォロー完了フラグ：完了
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FllwCompleteFlgComplete As String = "1"

    ''' <summary>
    ''' フォロー完了フラグ：未完了
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FllwCompleteFlgNotComplete As String = "0"

    ''' <summary>
    ''' 処理タイプ：処理なし
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ActionTypeNothing As String = "0"

    ''' <summary>
    ''' フォローメモの最大文字数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FllwMemoMaxLength As Integer = 1024

    ''' <summary>
    ''' 文言：タイトル
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdTitle As Integer = 1

    ''' <summary>
    ''' 文言：キャンセルボタン
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdCancelButton As Integer = 2

    ''' <summary>
    ''' 文言：登録ボタン
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdRegistButton As Integer = 3

    ''' <summary>
    ''' 文言：フォロー設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdFllwSetting As Integer = 4

    ''' <summary>
    ''' 文言：次回フォロー日
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdFllwExprDateTitle As Integer = 5

    ''' <summary>
    ''' 文言：メモタイトル
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdFllwMemoTitle As Integer = 6

    ''' <summary>
    ''' 文言：メモ消去ボタン
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdClearMemo As Integer = 7

    ''' <summary>
    ''' 文言：メモ送信タイトル
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdSendMemo As Integer = 8

    ''' <summary>
    ''' 文言：通知ボタン
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdNotice As Integer = 9

    ''' <summary>
    ''' 文言：メールボタン
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdClearMail As Integer = 10

    ''' <summary>
    ''' 文言：フォロー完了タイトル
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdFllwCompleteFlgTitle As Integer = 11

    ''' <summary>
    ''' 文言：フォロー完了ボタン
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdFllwComplete As Integer = 12

    ''' <summary>
    ''' 文言：フォロー未完了ボタン
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdFllwNotComplete As Integer = 13

    ''' <summary>
    ''' 文言：メールタイトル
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdMailTitle As Integer = 14

    ''' <summary>
    ''' 文言：既にフォローが完了している場合
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdFllwAlreadyComplate As Integer = 901

    ''' <summary>
    ''' 文言：他のユーザがフォローしている場合
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdFllwOtherStaff As Integer = 902

    ''' <summary>
    ''' 文言：次回フォロー日が未入力の場合
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdRequiredFllwExprDate As Integer = 903

    ''' <summary>
    ''' 文言：次回フォロー日が過去日付の場合
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdPastFllwExprDate As Integer = 904

    ''' <summary>
    ''' 文言：フォローメモの最大文字数を超えていた場合
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdFllwMemoMaxLength As Integer = 905

    ''' <summary>
    ''' 正常
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MessageIdNormal As Integer = 0

#End Region


#Region "イベント定義"

    ''' <summary>
    ''' フォームロード時のイベント
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

        ' 文言設定
        Me.SetWord()

        Logger.Info("Page_Load_End")

    End Sub

    ''' <summary>
    ''' スピンアイコン表示時の初期化処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub LoadSpinButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SC3290104_LoadSpinButton.Click

        Logger.Info("SC3290104_LoadSpinButton_Click_Start")

        ' ログイン情報チェック
        Logger.Info("LoadSpinButton_Click_001 " & "Call_Start StaffContext.Current")
        Dim loginStaff As StaffContext = StaffContext.Current
        Logger.Info("LoadSpinButton_Click_001 " & "Call_End   StaffContext.Current")

        ' 現在日付の取得
        Logger.Info("LoadSpinButton_Click_002 Call_Start DateTimeFunc.Now param[" & loginStaff.DlrCD & "]")
        Dim nowDate As Date = DateTimeFunc.Now(loginStaff.DlrCD)
        Logger.Info("LoadSpinButton_Click_002 Call_End DateTimeFunc.Now Ret[" & nowDate & "]")


        ' 異常項目フォローの取得
        Dim logic As New SC3290104BusinessLogic
        Dim row As SC3290104DataSet.SC3290104IrregFllwRow = _
            logic.GetIrregularFollowInfo(Me.SC3290104_IrregFllwId.Value, Me.SC3290104_IrregClassCd.Value, Me.SC3290104_IrregItemCd.Value, _
                                         Me.SC3290104_StfCd.Value, nowDate)

        ' 値の初期化
        Me.SC3290104_ActionType.Value = ActionTypeNothing
        Me.SC3290104_IrregFllwId.Value = DBDefaultValueNumber
        Me.SC3290104_NowDate.Value = nowDate.Date
        Me.SC3290104_FllwFlg.Checked = False
        Me.SC3290104_FllwExprDate.Value = Nothing
        Me.SC3290104_FllwExprDateDummy.Value = String.Empty
        Me.SC3290104_FllwMemo.Value = String.Empty
        Me.SC3290104_FllwCompleteFlg.Value = FllwCompleteFlgNotComplete
        Me.SC3290104_FllwCompleteFlgButton.Text = Me.SC3290104_FllwNotCompleteWord.Value

        ' 異常項目フォローをチェックする
        Dim wordId = Me.IsErrorFllwRow(row, loginStaff.Account)

        If Not wordId Is Nothing Then

            ' エラーメッセージを出力し排他エラー処理を行う
            JavaScriptUtility.RegisterStartupFunctionCallScript( _
                Me.Page, "showMessageBoxAndConcurrencySC3290104", "startup", WebWordUtility.GetWord(AppId, wordId))

            Logger.Info("SC3290104_LoadSpinButton_Click_End Error:" & wordId)

            Return
        End If

        ' 異常項目フォローが存在する場合
        If Not row Is Nothing Then

            Me.SC3290104_FllwFlg.Checked = True
            Me.SC3290104_IrregFllwId.Value = row.IRREG_FLLW_ID

            Me.SC3290104_FllwCompleteFlg.Value = row.FLLW_COMPLETE_FLG

            If row.FLLW_COMPLETE_FLG = FllwCompleteFlgComplete Then
                Me.SC3290104_FllwCompleteFlgButton.Text = Me.SC3290104_FllwCompleteWord.Value
            Else
                If row.FLLW_EXPR_DATE.Date <> DBDefaultValueDate Then
                    Me.SC3290104_FllwExprDate.Value = row.FLLW_EXPR_DATE.Date
                End If
            End If

            If Not String.IsNullOrEmpty(row.FLLW_MEMO) AndAlso row.FLLW_MEMO <> DBDefaultValueString Then
                Me.SC3290104_FllwMemo.Value = row.FLLW_MEMO
            End If

        End If

        ' 表示完了処理
        JavaScriptUtility.RegisterStartupFunctionCallScript(Me.Page, "initDisplaySC3290104", "startup")

        Logger.Info("SC3290104_LoadSpinButton_Click_End")
    End Sub

    ''' <summary>
    ''' 登録ボタン押下処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub RegisterButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SC3290104_RegisterButton.Click

        Logger.Info("SC3290104_RegisterButton_Start")

        ' フォロー設定がOff、かつフォロー完了フラグが未完了の場合は登録処理を行わない
        If Not Me.SC3290104_FllwFlg.Checked AndAlso Me.SC3290104_FllwCompleteFlg.Value = FllwCompleteFlgNotComplete Then

            ' 登録完了処理
            JavaScriptUtility.RegisterStartupFunctionCallScript(Me.Page, "registerCompleteSC3290104", "startup")

            Logger.Info("SC3290104_RegisterButton_End Follow Off")
            Return

        End If

        ' ログイン情報チェック
        Logger.Info("RegisterButton_Click_001 " & "Call_Start StaffContext.Current")
        Dim loginStaff As StaffContext = StaffContext.Current
        Logger.Info("RegisterButton_Click_001 " & "Call_End   StaffContext.Current")

        ' 現在日付の取得
        Logger.Info("RegisterButton_Click_002 Call_Start DateTimeFunc.Now param[" & loginStaff.DlrCD & "]")
        Dim nowDate As Date = DateTimeFunc.Now(loginStaff.DlrCD)
        Logger.Info("RegisterButton_Click_002 Call_End DateTimeFunc.Now Ret[" & nowDate & "]")

        If FllwMemoMaxLength < Me.SC3290104_FllwMemo.Value.Length Then

            Dim message As String = _
                String.Format(CultureInfo.InvariantCulture _
                              , WebWordUtility.GetWord(AppId, WordIdFllwMemoMaxLength) _
                              , WebWordUtility.GetWord(AppId, WordIdFllwMemoTitle) _
                              , FllwMemoMaxLength)

            ' エラーメッセージを出力
            JavaScriptUtility.RegisterStartupFunctionCallScript( _
                Me.Page, "showMessageBoxSC3290104", "startup", message)

            Logger.Info("SC3290104_RegisterButton_End Error:" & WordIdFllwMemoMaxLength)

            Return

        End If

        Dim fllwExprDate As Date? = Nothing

        ' フォロー期日が入力ありの場合
        If Not String.IsNullOrEmpty(Me.SC3290104_FllwExprDateDummy.Value) Then

            Dim parsedValue As DateTime
            If (DateTime.TryParse(Me.SC3290104_FllwExprDateDummy.Value, parsedValue)) Then
                fllwExprDate = parsedValue
            End If

        End If

        ' フォロー完了フラグが未完了の場合
        If Me.SC3290104_FllwCompleteFlg.Value = FllwCompleteFlgNotComplete Then

            ' フォロー期日が未入力
            If fllwExprDate Is Nothing Then

                ' エラーメッセージを出力
                JavaScriptUtility.RegisterStartupFunctionCallScript( _
                    Me.Page, "showMessageBoxSC3290104", "startup", WebWordUtility.GetWord(AppId, WordIdRequiredFllwExprDate))

                Logger.Info("SC3290104_RegisterButton_End Error:" & WordIdRequiredFllwExprDate)

                Return

            Else

                ' フォロー期日が過去日付
                If fllwExprDate.Value < nowDate.Date Then

                    ' エラーメッセージを出力
                    JavaScriptUtility.RegisterStartupFunctionCallScript( _
                        Me.Page, "showMessageBoxSC3290104", "startup", WebWordUtility.GetWord(AppId, WordIdPastFllwExprDate))

                    Logger.Info("SC3290104_RegisterButton_End Error:" & WordIdPastFllwExprDate)

                    Return

                End If

            End If

        End If

        ' 異常項目フォローの取得
        Dim logic As New SC3290104BusinessLogic
        Dim row As SC3290104DataSet.SC3290104IrregFllwRow = _
            logic.GetIrregularFollowInfo(Me.SC3290104_IrregFllwId.Value, Me.SC3290104_IrregClassCd.Value, Me.SC3290104_IrregItemCd.Value, _
                                         Me.SC3290104_StfCd.Value, nowDate)

        ' 異常項目フォローをチェックする
        Dim wordId = Me.IsErrorFllwRow(row, loginStaff.Account)

        If Not wordId Is Nothing Then

            ' エラーメッセージを出力し排他エラー処理を行う
            JavaScriptUtility.RegisterStartupFunctionCallScript( _
                Me.Page, "showMessageBoxAndConcurrencySC3290104", "startup", WebWordUtility.GetWord(AppId, wordId))

            Logger.Info("SC3290104_RegisterButton_End Error:" & wordId)

            Return

        End If

        ' 異常項目フォローが存在しない場合
        If row Is Nothing Then

            Dim dataDable As New SC3290104DataSet.SC3290104IrregFllwDataTable
            row = dataDable.NewRow
            row.IRREG_FLLW_ID = DBDefaultValueNumber
            row.IRREG_CLASS_CD = Me.SC3290104_IrregClassCd.Value
            row.IRREG_ITEM_CD = Me.SC3290104_IrregItemCd.Value
            row.STF_CD = Me.SC3290104_StfCd.Value
        End If

        row.FLLW_PIC_STF_CD = loginStaff.Account
        row.FLLW_COMPLETE_FLG = Me.SC3290104_FllwCompleteFlg.Value

        If fllwExprDate Is Nothing Then
            row.FLLW_EXPR_DATE = DBDefaultValueDate
        Else
            row.FLLW_EXPR_DATE = fllwExprDate.Value
        End If

        If Not String.IsNullOrEmpty(Me.SC3290104_FllwMemo.Value) Then
            row.FLLW_MEMO = Me.SC3290104_FllwMemo.Value
        Else
            row.FLLW_MEMO = DBDefaultValueString
        End If

        ' 異常項目フォローの設定
        wordId = logic.SetIrregularFollowInfo(row, loginStaff.Account, nowDate)

        If wordId <> MessageIdNormal Then

            ' エラーメッセージを出力
            JavaScriptUtility.RegisterStartupFunctionCallScript( _
                Me.Page, "showMessageBoxSC3290104", "startup", WebWordUtility.GetWord(AppId, wordId))

            Logger.Info("SC3290104_RegisterButton_End Error:" & wordId)

            Return

        End If

        ' 登録完了処理
        JavaScriptUtility.RegisterStartupFunctionCallScript(Me.Page, "registerCompleteSC3290104", "startup")

        Logger.Info("SC3290104_RegisterButton_End")

    End Sub

#End Region


#Region "非公開メソッド"

    ''' <summary>
    ''' 文言をセットする
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetWord()

        Logger.Info("SetWord_Start")

        Me.SC3290104_Title.Text = Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordIdTitle))
        Me.SC3290104_CancelButton.Text = Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordIdCancelButton))
        Me.SC3290104_RegistButton.Text = Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordIdRegistButton))
        Me.SC3290104_FllwSetting.Text = Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordIdFllwSetting))
        Me.SC3290104_FllwExprDateTitle.Text = Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordIdFllwExprDateTitle))
        Me.SC3290104_FllwMemoTitle.Text = Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordIdFllwMemoTitle))
        Me.SC3290104_ClearMemo.Text = Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordIdClearMemo))
        Me.SC3290104_SendMemo.Text = Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordIdSendMemo))
        Me.SC3290104_Notice.Text = Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordIdNotice))
        Me.SC3290104_ClearMail.Text = Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordIdClearMail))
        Me.SC3290104_FllwCompleteFlgTitle.Text = Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordIdFllwCompleteFlgTitle))

        ' Hiddenフィールドに保持する文言はHTMLエンコードが自動で行われるためここでは処理しない
        Me.SC3290104_FllwCompleteWord.Value = WebWordUtility.GetWord(AppId, WordIdFllwComplete)
        Me.SC3290104_FllwNotCompleteWord.Value = WebWordUtility.GetWord(AppId, WordIdFllwNotComplete)
        Me.SC3290104_MailTitle.Value = WebWordUtility.GetWord(AppId, WordIdMailTitle)

        Logger.Info("SetWord_End")
    End Sub

    ''' <summary>
    ''' 異常項目フォローをチェックする
    ''' </summary>
    ''' <param name="row">異常項目フォローの行</param>
    ''' <param name="stfCd">スタッフコード</param>
    ''' <returns>エラーが存在する場合は文言ID。それ以外はNothingを返却する。</returns>
    ''' <remarks></remarks>
    Private Function IsErrorFllwRow(ByVal row As SC3290104DataSet.SC3290104IrregFllwRow, ByVal stfCd As String) As String

        Dim startLog As New StringBuilder
        With startLog
            .Append("IsErrorFllwRow_Start ")
            .Append("row[")
            .Append(IsNothing(row))
            .Append("]")
            .Append(",stfCd[" & stfCd & "]")
        End With
        Logger.Info(startLog.ToString)

        If row Is Nothing Then
            Logger.Info("IsErrorFllwRow_End:[]")
            Return Nothing
        End If

        Dim wordId As String = Nothing

        ' 別のスタッフがフォローしている場合
        If DBDefaultValueString <> row.FLLW_PIC_STF_CD AndAlso row.FLLW_PIC_STF_CD <> stfCd Then

            ' フォロー完了している場合
            If row.FLLW_COMPLETE_FLG = FllwCompleteFlgComplete Then

                wordId = WordIdFllwAlreadyComplate
            Else
                wordId = WordIdFllwOtherStaff
            End If

        End If

        '結果返却
        Dim endLog As New StringBuilder
        With endLog
            .Append("IsErrorFllwRow_End Ret:[")
            .Append(wordId)
            .Append("] ")
        End With
        Logger.Info(endLog.ToString)

        Return wordId

    End Function

#End Region

End Class
