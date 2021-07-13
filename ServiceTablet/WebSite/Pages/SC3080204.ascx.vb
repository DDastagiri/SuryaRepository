'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3080204.ascx.vb
'─────────────────────────────────────
'機能： 顧客メモ
'補足： 
'作成： 2011/12/??  ????
'更新： 2013/07/05 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
'─────────────────────────────────────
Option Explicit On
Option Strict On

Imports System.Globalization

Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.iCROP.BizLogic.SC3080204
Imports Toyota.eCRB.iCROP.DataAccess.SC3080204
Imports Toyota.eCRB.iCROP.BizLogic.SC3080201
Imports Toyota.eCRB.SystemFrameworks.Web.Controls
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.iCROP.DataAccess.SC3080204.SC3080204DataSetTableAdapters

''' <summary>
''' SC3080204(顧客メモ)
''' Webページのプレゼンテーション層
''' </summary>
''' <remarks>顧客メモ</remarks>
Partial Class Pages_SC3080204uc
    Inherits System.Web.UI.UserControl
    Implements ICallbackEventHandler

#Region "定数"
    ''' <summary>
    ''' セッションキー
    ''' </summary>
    ''' <remarks></remarks>
    Public Const SESSION_KEY_CUSTSEGMENT As String = "SearchKey.CUSTSEGMENT"      '顧客区分 (1：自社客 / 2：未取引客)
    Public Const SESSION_KEY_CUSTOMERCLASS As String = "SearchKey.CUSTOMERCLASS"  '顧客分類 (1：所有者 / 2：使用者 / 3：その他)
    Public Const SESSION_KEY_CRCUSTID As String = "SearchKey.CRCUSTID"            '活動先顧客コード
    Public Const SESSION_KEY_CRCUSTNAME As String = "SearchKey.CRCUSTNAME"        '活動先顧客名
    Public Const SESSION_KEY_MEMO_INIT As String = "SearchKey.MEMOINIT"           '9:顧客メモ読み込み完了

    ''' <summary>
    ''' 編集モード
    ''' </summary>
    ''' <remarks></remarks>
    Public Const MODE_APPEND As String = "append"          '追加モード
    Public Const MODE_EDIT As String = "edit"              '編集モード
    Public Const MODE_LOOK As String = "look"              '参照モード
#End Region

#Region "イベント"

    ''' <summary>
    ''' ロード次の処理を実施します。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        '顧客IDがセットされた最初は、顧客メモ一覧の内容を反映する。
        'If (Me.ContainsKey(ScreenPos.Current, SESSION_KEY_CRCUSTID) = True) AndAlso _
        '    Not String.IsNullOrEmpty(DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CRCUSTID, False), String)) Then

        '    '2度処理の制御 (初期起動時と、顧客新規登録してから、画面再読み込み時)
        '    If ((Me.ContainsKey(ScreenPos.Current, SESSION_KEY_MEMO_INIT) = False) Or (Not Page.IsPostBack)) Then

        '        '顧客メモ一覧を取得する
        '        Dim memoListDataTbl As New SC3080204DataSet.SC3080204CustMemoDataTable

        '        '顧客メモ一覧画面描画
        '        memoListDataTbl = SetMemoList()

        '        '読み込み完了
        '        SetValue(ScreenPos.Current, SESSION_KEY_MEMO_INIT, "9")

        '    End If

        'End If

        If (Not Me.IsPostBack) Then
            '初期表示時はダウンロードしない
            CustomerMemoVisiblePanel.Visible = False
        End If

        'コールバックスプリクト登録
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), _
                                            "Callback2", _
                                            String.Format(CultureInfo.InvariantCulture, _
                                                            "callback2.beginCallback = function () {{ {0}; }};", _
                                                            Page.ClientScript.GetCallbackEventReference(Me, _
                                                                                                        "callback2.packedArgument", _
                                                                                                        "callback2.endCallback", _
                                                                                                        "", _
                                                                                                        True)), _
                                                            True)

    End Sub

    ''' <summary>
    ''' 保存ボタン押下時。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2013/07/05 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
    ''' </history>
    Protected Sub SaveMemoButton_Click(sender As Object, e As System.EventArgs) Handles SaveMemoButton.Click

        Dim ret As Integer = 0
        Dim msgID As Integer = 0
        Dim bizClass As New SC3080204BusinessLogic
        Dim memoDataTbl As New SC3080204DataSet.SC3080204CustMemoDataTable

        '画面の値を取得する
        Me.GetDisplayValues(memoDataTbl)

        'スクリプトで顧客メモポップアップを起動
        '2012/04/18 KN 暫定対応
        'JavaScriptUtility.RegisterStartupFunctionCallScript(CType(Me.Parent.Parent.Parent.Parent, BasePage), "PageLoad", "save")
        JavaScriptUtility.RegisterStartupFunctionCallScript(CType(Me.Page, BasePage), "SC3080204PageLoad", "save")
        '2012/04/18 KN 暫定対応

        'バリデーション判定
        If (SC3080204BusinessLogic.CheckValidation(memoDataTbl, msgID) = False) Then
            'エラーメッセージを表示
            ShowMessageBox(msgID)
            Exit Sub
        End If

        If (Me.modeMemo.Value.Equals(MODE_APPEND)) Then
            '顧客メモ新規登録
            ret = bizClass.InsertCustomerMemo(memoDataTbl, msgID)
        Else
            '顧客メモ更新
            ret = bizClass.UpdateCustomerMemo(memoDataTbl, msgID)
        End If

        '顧客メモ一覧画面再描画
        Call SetMemoList()

    End Sub
#End Region

#Region "メソット"

    ''' <summary>
    ''' セッションの値をDataRowにセットする。
    ''' </summary>
    ''' <param name="memoDataRow">顧客メモDataRow</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2013/07/05 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
    ''' </history>
    Protected Sub SetSessionValue(ByVal memoDataRow As SC3080204DataSet.SC3080204CustMemoRow)

        '販売店コード
        Dim dlrcd As String = Nothing
        dlrcd = StaffContext.Current.DlrCD
        '店舗コード
        Dim strcd As String = Nothing
        strcd = StaffContext.Current.BrnCD
        'アカウント
        Dim account As String = Nothing
        account = StaffContext.Current.Account

        '顧客区分 (1：自社客 / 2：未取引客)
        Dim custsegment As String = Nothing
        custsegment = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CUSTSEGMENT, False), String)
        '顧客分類 (1：所有者 / 2：使用者 / 3：その他)
        Dim customerclass As String = Nothing
        customerclass = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CUSTOMERCLASS, False), String)
        '活動先顧客コード
        Dim crcustid As String = Nothing
        crcustid = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CRCUSTID, False), String)
        '活動先顧客名
        Dim crcustname As String = Nothing
        crcustname = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CRCUSTNAME, False), String)

        '販売店コード
        memoDataRow.DLRCD = dlrcd
        '店舗コード
        memoDataRow.STRCD = strcd
        '更新アカウント
        memoDataRow.ACCOUNT = account

        '顧客区分 (1：自社客 / 2：未取引客)
        memoDataRow.CUSTSEGMENT = custsegment
        '顧客分類 (1：所有者 / 2：使用者 / 3：その他)
        memoDataRow.CUSTOMERCLASS = customerclass

        '2013/07/05 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        '活動先顧客コード
        'memoDataRow.CRCUSTID = crcustid

        memoDataRow.CRCUSTID = 0

        If Not Decimal.TryParse(crcustid, memoDataRow.CRCUSTID) Then

            memoDataRow.CRCUSTID = -1

        End If

        '2013/07/05 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        '活動先顧客名
        memoDataRow.CRCUSTNAME = crcustname

    End Sub


    ''' <summary>
    ''' 顧客メモ一覧画面の値を設定する
    ''' </summary>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2013/07/05 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
    ''' </history>
    Protected Function SetMemoList() As SC3080204DataSet.SC3080204CustMemoDataTable

        '顧客メモ一覧を取得する
        Dim msgID As Integer = 0
        Dim bizClass As New SC3080204BusinessLogic
        Dim memoDataTbl As New SC3080204DataSet.SC3080204CustMemoDataTable
        Dim memoDataRow As SC3080204DataSet.SC3080204CustMemoRow
        Dim memoListDataTbl As New SC3080204DataSet.SC3080204CustMemoDataTable

        memoDataRow = memoDataTbl.NewSC3080204CustMemoRow
        SetSessionValue(memoDataRow)            'セッション値の取得
        memoDataTbl.Rows.Add(memoDataRow)       '追加する

        memoListDataTbl = SC3080204BusinessLogic.GetCustomerMemo(memoDataTbl, msgID)
        memoRepeater.DataSource = memoListDataTbl
        memoRepeater.DataBind()

        If (memoListDataTbl.Rows.Count > 0) Then
            memoDataRow = memoListDataTbl.Item(0)

            Me.activeSEQNOMemo.Value = CType(memoDataRow.CUSTMEMOHIS_SEQNO, String)     'SEQNo
            Me.titleLabelMemo.Text = memoDataRow.FIRSTMEMO                              'タイトル
            Me.dateLabel.Text = Format(memoDataRow.UPDATEDATE, "yyyy/MM/dd")            '日付
            Me.timeLabel.Text = Format(memoDataRow.UPDATEDATE, "HH:mm")                 '時間
            Me.memoTextBox.Text = memoDataRow.MEMO                                      'メモ内容

            '2013/07/05 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

            Me.activeCSTMemoLockVersionHidden.Value = CType(memoDataRow.ROW_LOCK_VERSION, String)
            Me.cstLockVersionHidden.Value = CStr(memoDataRow.CST_ROW_LOCK_VERSION)

            '2013/07/05 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

            Me.countLabel.Text = String.Format(CultureInfo.CurrentCulture, WebWordUtility.GetWord(70002), memoListDataTbl.Rows.Count)   '件数

            '初期モード:参照モード
            Me.modeMemo.Value = MODE_LOOK

        Else
            Me.activeSEQNOMemo.Value = String.Empty            'SEQNo
            Me.titleLabelMemo.Text = String.Empty              'タイトル
            Me.dateLabel.Text = String.Empty               '日付
            Me.timeLabel.Text = String.Empty               '時間
            Me.memoTextBox.Text = String.Empty             'メモ内容
            Me.countLabel.Text = String.Empty              '0件の場合は、件数を表示ない 

            '初期モード:追加モード
            Me.modeMemo.Value = MODE_APPEND

        End If

        '現在時の設定
        Me.todayHidden.Value = Format(Now(), "yyyy/MM/dd")
        Me.nowTimeHidden.Value = Format(Now(), "HH:mm")

        '件数
        Me.listCountHidden.Value = CType(memoListDataTbl.Rows.Count, String)

        Return memoListDataTbl

    End Function

    ''' <summary>
    ''' 画面の値を取得する
    ''' </summary>
    ''' <param name="memoDataTbl">顧客メモ情報DataTable</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2013/07/05 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
    ''' </history>
    Protected Sub GetDisplayValues(ByVal memoDataTbl As SC3080204DataSet.SC3080204CustMemoDataTable)

        Dim memoDataRow As SC3080204DataSet.SC3080204CustMemoRow = memoDataTbl.NewSC3080204CustMemoRow

        'セッション内の値をセットする
        Me.SetSessionValue(memoDataRow)

        '値を設定する
        If (Not Me.modeMemo.Value.Equals(MODE_APPEND)) Then '追加モード以外
            memoDataRow.CUSTMEMOHIS_SEQNO = CType(Me.activeSEQNOMemo.Value, Long)

            '2013/07/05 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
            memoDataRow.ROW_LOCK_VERSION = CType(Me.activeCSTMemoLockVersionHidden.Value, Long)
            memoDataRow.CST_ROW_LOCK_VERSION = CType(Me.cstLockVersionHidden.Value, Long)
            '2013/07/05 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        End If
        memoDataRow.MEMO = Me.memoTextBox.Text

        '追加する
        memoDataTbl.AddSC3080204CustMemoRow(memoDataRow)

    End Sub

    ''' <summary>
    ''' 顧客メモ削除。
    ''' </summary>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2013/07/05 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
    ''' </history>
    Protected Function DeleteMemo(ByVal seqno As Long, ByVal linecount As Integer) As String

        Dim ret As Integer = 0
        Dim msgID As Integer = 0
        Dim bizClass As New SC3080204BusinessLogic
        Dim memoDataTbl As New SC3080204DataSet.SC3080204CustMemoDataTable

        '更新用DataTableの作成
        Dim memoDataRow As SC3080204DataSet.SC3080204CustMemoRow = memoDataTbl.NewSC3080204CustMemoRow
        memoDataTbl.AddSC3080204CustMemoRow(memoDataRow)

        'セッション内の値をセットする
        Me.SetSessionValue(memoDataRow)

        '2013/07/05 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
        memoDataRow.CST_ROW_LOCK_VERSION = CType(Me.cstLockVersionHidden.Value, Long)
        '2013/07/05 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        memoDataRow.CUSTMEMOHIS_SEQNO = seqno

        '顧客メモ削除
        ret = bizClass.DeleteCustomerMemo(memoDataTbl, msgID)

        Dim resultString As String = String.Empty

        '件数
        Me.listCountHidden.Value = CType(linecount - 1, String)
        Me.countLabel.Text = String.Format(CultureInfo.CurrentCulture, WebWordUtility.GetWord(70002), Me.listCountHidden.Value)
        resultString = resultString & Me.listCountHidden.Value & ","
        resultString = resultString & Me.countLabel.Text & ","
        resultString = resultString & "OK"

        Return resultString

    End Function

#End Region

#Region "コールバック"

    Private _callbackResult As String

    ''' <summary>
    ''' コールバック用文字列を返す
    ''' </summary>
    ''' <remarks></remarks>
    Public Function GetCallbackResult() As String Implements System.Web.UI.ICallbackEventHandler.GetCallbackResult

        Return _callbackResult

    End Function

    Private Const OKText As String = "1"

    Private Const ErrorText As String = "999"

    '顧客メモ削除
    Private Const MethodDeleteMemo As String = "DeleteMemo"

    '顧客メモ入力チェック
    Private Const MethodInputCheck As String = "InputCheckMemo"

    ''' <summary>
    ''' コールバックイベントハンドリング
    ''' </summary>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2013/07/05 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
    ''' </history>
    Public Sub RaiseCallbackEvent(eventArgument As String) Implements System.Web.UI.ICallbackEventHandler.RaiseCallbackEvent

        Try

            Dim tokens As String() = eventArgument.Split(New Char() {","c})
            Dim method As String = tokens(0)
            Dim argument As String = tokens(1)
            Dim resultString As String = String.Empty

            '顧客メモ削除
            If (method.Equals(MethodDeleteMemo)) Then

                'SEQ番号
                Dim seqno As Long = CType(tokens(1), Long)
                '行数
                Dim linecount As Integer = CType(tokens(2), Integer)

                _callbackResult = deleteMemo(seqno, linecount)

            End If


            '2013/07/05 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
            '顧客メモ入力チェック
            If (method.Equals(MethodInputCheck)) Then

                Dim msgID As Integer = 0

                'メモ内容をセットする

                Using memoDataTbl2 As New SC3080204DataSet.SC3080204CustMemoDataTable

                    Dim memoDataRow As SC3080204DataSet.SC3080204CustMemoRow = memoDataTbl2.NewSC3080204CustMemoRow
                    memoDataRow.MEMO = HttpUtility.UrlDecode(tokens(1))
                    memoDataTbl2.AddSC3080204CustMemoRow(memoDataRow)

                    'バリデーション判定
                    If (SC3080204BusinessLogic.CheckValidation(memoDataTbl2, msgID) = False) Then
                        'エラーメッセージを表示
                        _callbackResult = ErrorText + "," + HttpUtility.HtmlEncode(WebWordUtility.GetWord(msgID))

                        Exit Sub
                    End If

                    _callbackResult = OKText

                End Using

            End If
            '22013/07/05 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計　既存流用 END

        Catch ex As Exception

            _callbackResult = ErrorText + "," + ex.Message

        End Try

    End Sub
#End Region

#Region " ページクラス処理のバイパス処理 "
    Private Sub SetValue(pos As ScreenPos, key As String, value As Object)
        GetPageInterface().SetValueBypass(pos, key, value)
    End Sub

    Private Function GetValue(pos As ScreenPos, key As String, removeFlg As Boolean) As Object
        Return GetPageInterface().GetValueBypass(pos, key, removeFlg)
    End Function

    Private Sub ShowMessageBox(wordNo As Integer, ParamArray wordParam() As String)
        GetPageInterface().ShowMessageBoxBypass(wordNo, wordParam)
    End Sub

    Private Function ContainsKey(pos As Toyota.eCRB.SystemFrameworks.Web.ScreenPos, key As String) As Boolean
        Return GetPageInterface().ContainsKeyBypass(pos, key)
    End Function

    Private Function GetPageInterface() As ICustomerDetailControl
        Return CType(Me.Page, ICustomerDetailControl)
    End Function

    'Public Sub RegistActivityAfter() Implements Toyota.eCRB.iCROP.BizLogic.SC3080201.ISC3080201Control.RegistActivityAfter

    'End Sub
#End Region

    ''' <summary>
    ''' 顧客メモオープンボタン時 (顧客詳細より移行)
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub CustomerMemoEditOpenButton_Click(sender As Object, e As System.EventArgs) Handles CustomerMemoEditOpenButton.Click

        '顧客IDがセットされた最初は、顧客メモ一覧の内容を反映する。
        If (Me.ContainsKey(ScreenPos.Current, SESSION_KEY_CRCUSTID) = True) AndAlso _
            Not String.IsNullOrEmpty(DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CRCUSTID, False), String)) Then

            ''2度処理の制御 (初期起動時と、顧客新規登録してから、画面再読み込み時)
            'If (Me.ContainsKey(ScreenPos.Current, SESSION_KEY_MEMO_INIT) = False) Then

            '顧客メモ一覧を取得する
            Dim memoListDataTbl As New SC3080204DataSet.SC3080204CustMemoDataTable

            '顧客メモ一覧画面描画
            memoListDataTbl = SetMemoList()

            '読み込み完了
            SetValue(ScreenPos.Current, SESSION_KEY_MEMO_INIT, "9")

            '顧客メモ欄を表示する
            CustomerMemoVisiblePanel.Visible = True
            customerMemoPanel.Update()

            'スクリプトで顧客メモの初期設定処理をする
            JavaScriptUtility.RegisterStartupFunctionCallScript(CType(Me.Page, BasePage), "SC3080204PageLoad", "SC3080204PageLoad")
            'End If

        End If

        'スクリプトで顧客メモポップアップを起動
        JavaScriptUtility.RegisterStartupFunctionCallScript(CType(Me.Page, BasePage), "commitCompleteOpenCustomerMemoEdit", "SC3080204Open")

    End Sub

End Class
