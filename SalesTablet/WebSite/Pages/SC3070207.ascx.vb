'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3070207.ascx.vb
'─────────────────────────────────────
'機能： 注文承認
'補足： 
'作成： 2013/12/09 TCS 山口  Aカード情報相互連携開発
'─────────────────────────────────────

Option Explicit On

Imports Toyota.eCRB.Estimate.Quotation.BizLogic
Imports Toyota.eCRB.Estimate.Quotation.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Web.Controls
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.iCROP.BizLogic.SC3070201
Imports System.Globalization
Imports System.Reflection.MethodBase

Partial Class Pages_SC3070207
    Inherits System.Web.UI.UserControl

#Region "定数/列挙値"
    ''' <summary>
    ''' セッションキー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SeatchKeyEstimateId As String = "EstimateId"
    Private Const SeatchKeySelectedEstimateIndex As String = "SelectedEstimateIndex"
    Private Const SeatchKeyNoticeReqId As String = "NoticeReqId"

    '2015/03/16 TCS 鈴木【TMT課題対応(#72 セールスタブレットの価格相談履歴表示)】ADD START
    Private _estimateIds As String = String.Empty
    '2015/03/16 TCS 鈴木【TMT課題対応(#72 セールスタブレットの価格相談履歴表示)】ADD END

    ''' <summary>
    ''' 注文承認を表示中フラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SeatchKeyApprovalActive As String = "ApprovalActive"

    ''' <summary>
    '''マネージャーコメント桁数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MANAGER_MEMO_CNT As String = "128"

#End Region

#Region "イベント"
    ''' <summary>
    ''' ページロード
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Pages_SC3070207_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_Start", GetCurrentMethod.Name), True)

        If Not Me.IsPostBack Then
            
        End If

        ErrorFlg.Value = String.Empty

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name), True)
    End Sub

#Region "承認処理"
    ''' <summary>
    ''' 依頼されている注文承認を承認する
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub SC3070207_ApprovalButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SC3070207_ApprovalButton.Click
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_Start", GetCurrentMethod.Name), True)

        '入力チェック
        If Not CheckInputFormat() Then
            Exit Sub
        End If

        'セッション情報取得
        Dim dtParameter As SC3070207DataSet.SC3070207ParameterDataTable
        dtParameter = GetSession()

        'コメント設定
        dtParameter(0).DISPLAYCONTENTS = Me.SC3070207_CommentTextBox.Text

        '承認処理
        Dim bizLogicSC3070207 As New SC3070207BusinessLogic()

        Dim result As Boolean = bizLogicSC3070207.InsertApproval(dtParameter)
 
        'エラー有の場合
        If result = False Then

            Toyota.eCRB.SystemFrameworks.Core.Logger.Error("Error Code=" & bizLogicSC3070207.MsgId)

            If (SC3070207BusinessLogic.MsgId901 = bizLogicSC3070207.MsgId) Then

                '承認依頼がキャンセルされています。
                ScriptManager.RegisterStartupScript(Me.Page, _
                                                   Me.GetType, _
                                                    "PageLoad", _
                                                    "dispLoading();alert(SC3070201HTMLDecode(""" + HttpUtility.HtmlEncode(WebWordUtility.GetWord("SC3070201", bizLogicSC3070207.MsgId)) + """));this_form.actionModeHiddenField.value = ""2"";this_form.submit();", _
                                                    True)

                ErrorFlg.Value = bizLogicSC3070207.MsgId
            Else
                '通知失敗、SA01,SA04連携失敗、見積テーブルロック失敗時。
                ShowMessageBox(bizLogicSC3070207.MsgId)
            End If

        Else
            '注文承認を非表示にする
            Me.Visible = False
            Me.RemoveValueBypass(ScreenPos.Current, SeatchKeyApprovalActive)
        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name), True)
    End Sub
#End Region

#Region "否認処理"
    ''' <summary>
    ''' 依頼されている注文承認を否認する
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub SC3070207_DenialButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SC3070207_DenialButton.Click
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_Start", GetCurrentMethod.Name), True)

        '入力チェック
        If Not CheckInputFormat() Then
            Exit Sub
        End If

        'セッション情報取得
        Dim dtParameter As SC3070207DataSet.SC3070207ParameterDataTable
        dtParameter = GetSession()

        'コメント設定
        dtParameter(0).DISPLAYCONTENTS = Me.SC3070207_CommentTextBox.Text

        '否認処理
        Dim bizLogicSC3070207 As New SC3070207BusinessLogic()

        Dim result As Boolean = bizLogicSC3070207.InsertDenial(dtParameter)
 
        'エラー有の場合
        If result = False Then

            Toyota.eCRB.SystemFrameworks.Core.Logger.Error("Error Code=" & bizLogicSC3070207.MsgId)

            If (SC3070207BusinessLogic.MsgId901 = bizLogicSC3070207.MsgId) Then

                '承認依頼がキャンセルされています。
                ScriptManager.RegisterStartupScript(Me, _
                                                    Me.GetType, _
                                                    "PageLoad", _
                                                    "dispLoading();alert(SC3070201HTMLDecode(""" + HttpUtility.HtmlEncode(WebWordUtility.GetWord("SC3070201", bizLogicSC3070207.MsgId)) + """));this_form.actionModeHiddenField.value = ""2"";this_form.submit();", _
                                                    True)

                ErrorFlg.Value = bizLogicSC3070207.MsgId
            Else
                '通知失敗、見積テーブルロック失敗時。
                ShowMessageBox(bizLogicSC3070207.MsgId)
            End If
        Else
            '注文承認を非表示にする
            Me.Visible = False
            Me.RemoveValueBypass(ScreenPos.Current, SeatchKeyApprovalActive)
        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name), True)
    End Sub
#End Region
#End Region

#Region "Private"
    '''' <summary>
    '''' 入力チェックを実施する
    '''' </summary>
    '''' <remarks></remarks>
    Private Function CheckInputFormat() As Boolean
        If Not Validation.IsCorrectDigit(Me.SC3070207_CommentTextBox.Text, MANAGER_MEMO_CNT) And _
            Not String.IsNullOrEmpty(Me.SC3070207_CommentTextBox.Text) Then
            'マネージャーコメントが128桁以上の場合
            ShowMessageBox(SC3070207BusinessLogic.MsgId904)

            Return False

        ElseIf (Validation.IsValidString(Me.SC3070207_CommentTextBox.Text) = False) And _
            Not String.IsNullOrEmpty(Me.SC3070207_CommentTextBox.Text) Then
            'マネージャーコメントに禁則文字が含まれている場合
            ShowMessageBox(SC3070207BusinessLogic.MsgId905)

            Return False

        End If

        Return True
    End Function

    ''' <summary>
    ''' セッション情報を取得する
    ''' </summary>
    ''' <returns>パラメータDataTable</returns>
    ''' <remarks></remarks>
    Private Function GetSession() As SC3070207DataSet.SC3070207ParameterDataTable
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_Start", GetCurrentMethod.Name), True)

        Using dtParameter As New SC3070207DataSet.SC3070207ParameterDataTable
            Dim drParameter As SC3070207DataSet.SC3070207ParameterRow
            drParameter = dtParameter.NewSC3070207ParameterRow

            '見積管理ID
            Dim estimateId As String = String.Empty
            If Me.ContainsKey(ScreenPos.Current, SeatchKeyEstimateId) Then
                estimateId = CType(Me.GetValue(ScreenPos.Current, SeatchKeyEstimateId, False), String)
            End If

            '選択している見積IDのIndex
            Dim selectedEstimateIndex As Long = 0
            If Me.ContainsKey(ScreenPos.Current, SeatchKeySelectedEstimateIndex) Then
                selectedEstimateIndex = CType(Me.GetValue(ScreenPos.Current, SeatchKeySelectedEstimateIndex, False), Long)
            End If

            '選択している見積ID
            drParameter.ESTIMATEID = 0
            drParameter.ESTIMATEID = CType(GetSelectedEstimateId(estimateId, selectedEstimateIndex), Long)

            '通知依頼ID
            drParameter.NOTICEREQID = 0
            If Me.ContainsKey(ScreenPos.Current, SeatchKeyNoticeReqId) Then
                drParameter.NOTICEREQID = CType(Me.GetValue(ScreenPos.Current, SeatchKeyNoticeReqId, False), Long)
            End If

            'ログインユーザー情報
            drParameter.DLR_CD = StaffContext.Current.DlrCD
            drParameter.BRN_CD = StaffContext.Current.BrnCD
            drParameter.ACCOUNT = StaffContext.Current.Account
            drParameter.ACCOUNTNAME = StaffContext.Current.UserName

            dtParameter.Rows.Add(drParameter)

            Return dtParameter
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name), True)
        End Using
    End Function


    ''' <summary>
    ''' 対象見積管理ID取得
    ''' </summary>
    ''' <param name="allEstimeId">見積管理ID(カンマ区切り)</param>
    ''' <param name="Index">対象Index番号</param>
    ''' <returns>見積管理ID</returns>
    ''' <remarks>Indexに該当する見積管理IDを返す</remarks>
    Private Function GetSelectedEstimateId(ByVal allEstimeId As String, ByVal Index As Long) As Long
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_Start", GetCurrentMethod.Name), True)

        Dim estimetaId = allEstimeId.Split(","c)

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name), True)

        Return CType(estimetaId(Index), Long)

    End Function











#End Region

#Region " ページクラス処理のバイパス処理 "

    ''' <summary>
    ''' GetValue関数のバイパス
    ''' </summary>
    ''' <param name="pos">ポジジョン</param>
    ''' <param name="key">検索キー</param>
    ''' <param name="removeFlg">削除フラグ</param>
    ''' <returns>値</returns>
    ''' <remarks></remarks>
    Private Function GetValue(ByVal pos As ScreenPos, ByVal key As String, ByVal removeFlg As Boolean) As Object
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_Start", GetCurrentMethod.Name), True)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name), True)
        Return GetPageInterface().GetValueBypass(pos, key, removeFlg)
    End Function

    ''' <summary>
    ''' ShowMessageBox関数のバイパス
    ''' </summary>
    ''' <param name="wordNo">文言No</param>
    ''' <param name="wordParam">パラメータ</param>
    ''' <remarks></remarks>
    Private Sub ShowMessageBox(ByVal wordNo As Integer, ByVal ParamArray wordParam() As String)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_Start", GetCurrentMethod.Name), True)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name), True)
        GetPageInterface().ShowMessageBoxBypass(wordNo, wordParam)
    End Sub

    ''' <summary>
    ''' ContainsKey関数のバイパス
    ''' </summary>
    ''' <param name="pos">ポジジョン</param>
    ''' <param name="key">検索キー</param>
    ''' <returns>値</returns>
    ''' <remarks></remarks>
    Private Function ContainsKey(ByVal pos As ScreenPos, ByVal key As String) As Object
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_Start", GetCurrentMethod.Name), True)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name), True)
        Return GetPageInterface().ContainsKeyBypass(pos, key)
    End Function

    ''' <summary>
    ''' RemoveValueBypass関数のバイパス
    ''' </summary>
    ''' <param name="pos">ポジジョン</param>
    ''' <param name="key">検索キー</param>
    ''' <remarks></remarks>
    Private Sub RemoveValueBypass(pos As Toyota.eCRB.SystemFrameworks.Web.ScreenPos, key As String)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_Start", GetCurrentMethod.Name), True)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name), True)
        GetPageInterface().RemoveValueBypass(pos, key)
    End Sub

    ''' <summary>
    ''' 親ページのインターフェース取得
    ''' </summary>
    ''' <returns>親ページのIEstimateInfoControl</returns>
    ''' <remarks></remarks>
    Private Function GetPageInterface() As IEstimateInfoControl
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_Start", GetCurrentMethod.Name), True)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name), True)
        Return CType(Me.Page, IEstimateInfoControl)
    End Function

#End Region
End Class
