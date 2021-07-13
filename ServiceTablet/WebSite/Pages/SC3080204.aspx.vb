'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3080204.aspx.vb
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
Imports Toyota.eCRB.SystemFrameworks.Web.Controls
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.iCROP.DataAccess.SC3080204.SC3080204DataSetTableAdapters

''' <summary>
''' SC3080204(顧客メモ)
''' Webページのプレゼンテーション層
''' </summary>
''' <remarks>顧客メモ</remarks>
Partial Class Pages_SC3080204
    Inherits BasePage
    Implements ICallbackEventHandler


    ''' <summary>
    ''' セッションキー
    ''' </summary>
    ''' <remarks></remarks>
    Public Const SESSION_KEY_CUSTSEGMENT As String = "SearchKey.CUSTSEGMENT"      '顧客区分 (1：自社客 / 2：未取引客)
    Public Const SESSION_KEY_CUSTOMERCLASS As String = "SearchKey.CUSTOMERCLASS"  '顧客分類 (1：所有者 / 2：使用者 / 3：その他)
    Public Const SESSION_KEY_CRCUSTID As String = "SearchKey.CRCUSTID"            '活動先顧客コード
    Public Const SESSION_KEY_CRCUSTNAME As String = "SearchKey.CRCUSTNAME"        '活動先顧客名
    'Public Const SESSION_KEY_DLRCD As String = "dlrcd"                  '販売店コード
    'Public Const SESSION_KEY_STRCD As String = "strcd"                  '店舗コード
    'Public Const SESSION_KEY_ACCOUNT As String = "account"              'アカウント

    ''' <summary>
    ''' 編集モード
    ''' </summary>
    ''' <remarks></remarks>
    Public Const MODE_APPEND As String = "append"          '追加モード
    Public Const MODE_EDIT As String = "edit"              '編集モード
    Public Const MODE_LOOK As String = "look"              '参照モード

    ''' <summary>
    ''' ロード次の処理を実施します。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            '顧客メモ一覧を取得する
            Dim memoListDataTbl As New SC3080204DataSet.SC3080204CustMemoDataTable

            '顧客メモ一覧画面描画
            memoListDataTbl = SetMemoList()

        End If

        'コールバックスプリクト登録
        ClientScript.RegisterStartupScript(Me.GetType(), _
                                           "Callback", _
                                           String.Format(CultureInfo.InvariantCulture, _
                                                         "callback.beginCallback = function () {{ {0}; }};", _
                                                         Page.ClientScript.GetCallbackEventReference(Me, _
                                                                                                     "callback.packedArgument", _
                                                                                                     "callback.endCallback", _
                                                                                                     "", _
                                                                                                     True)), _
                                                         True)
    End Sub

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
    ''' メモ選択時。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub memoSelectButton_Click(sender As Object, e As System.EventArgs) Handles memoSelectButton.Click

        Dim msgID As Integer = 0
        Dim bizClass As New SC3080204BusinessLogic
        Dim memoDataTbl As New SC3080204DataSet.SC3080204CustMemoDataTable
        Dim memoDataRow As SC3080204DataSet.SC3080204CustMemoRow
        memoDataRow = memoDataTbl.NewSC3080204CustMemoRow
        SetSessionValue(memoDataRow)            'セッション値の取得
        memoDataTbl.Rows.Add(memoDataRow)       '追加する

        '顧客メモ一覧を取得する
        Dim memoListDataTbl As New SC3080204DataSet.SC3080204CustMemoDataTable
        Dim memoListDataRow As SC3080204DataSet.SC3080204CustMemoRow
        memoListDataTbl = SC3080204BusinessLogic.GetCustomerMemo(memoDataTbl, msgID)
        For Each memoListDataRow In memoListDataTbl
            If (Me.activeSEQNO.Value.Equals(CType(memoListDataRow.CUSTMEMOHIS_SEQNO, String)) = True) Then

                Me.activeSEQNO.Value = CType(memoListDataRow.CUSTMEMOHIS_SEQNO, String)
                Me.titleLabel.Text = memoListDataRow.FIRSTMEMO
                Me.dateLabel.Text = Format(memoListDataRow.UPDATEDATE, "yyyy/MM/dd")
                Me.timeLabel.Text = Format(memoListDataRow.UPDATEDATE, "HH:mm")

                Exit For
            End If
        Next

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
    Protected Sub saveButton_Click(sender As Object, e As System.EventArgs) Handles saveButton.Click

        Dim ret As Integer = 0
        Dim msgID As Integer = 0
        Dim bizClass As New SC3080204BusinessLogic
        Dim memoDataTbl As New SC3080204DataSet.SC3080204CustMemoDataTable

        '画面の値を取得する
        Me.GetDisplayValues(memoDataTbl)

        'バリデーション判定
        If (SC3080204BusinessLogic.CheckValidation(memoDataTbl, msgID) = False) Then
            'エラーメッセージを表示
            ShowMessageBox(msgID)
            Exit Sub
        End If

        If (Me.mode.Value.Equals(MODE_APPEND)) Then
            '顧客メモ新規登録
            ret = bizClass.InsertCustomerMemo(memoDataTbl, msgID)
        Else
            '顧客メモ更新
            ret = bizClass.UpdateCustomerMemo(memoDataTbl, msgID)
        End If

        '顧客メモ一覧画面再描画
        Call SetMemoList()

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

            Me.activeSEQNO.Value = CType(memoDataRow.CUSTMEMOHIS_SEQNO, String)     'SEQNo
            Me.titleLabel.Text = HttpUtility.HtmlEncode(memoDataRow.FIRSTMEMO)      'タイトル
            Me.dateLabel.Text = Format(memoDataRow.UPDATEDATE, "yyyy/MM/dd")        '日付
            Me.timeLabel.Text = Format(memoDataRow.UPDATEDATE, "HH:mm")             '時間
            Me.memoTextBox.Text = HttpUtility.HtmlEncode(memoDataRow.MEMO)          'メモ内容

            '2013/07/05 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

            Me.activeCSTMemoLockVersionHidden.Value = CType(memoDataRow.ROW_LOCK_VERSION, String)
            Me.cstLockVersionHidden.Value = CStr(memoDataRow.CST_ROW_LOCK_VERSION)

            '2013/07/05 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

            Me.countLabel.Text = String.Format(CultureInfo.CurrentCulture, HttpUtility.HtmlEncode(WebWordUtility.GetWord(70002)), memoListDataTbl.Rows.Count)   '件数

            '初期モード:参照モード
            Me.mode.Value = MODE_LOOK

        Else
            Me.activeSEQNO.Value = String.Empty            'SEQNo
            Me.titleLabel.Text = String.Empty              'タイトル
            Me.dateLabel.Text = String.Empty               '日付
            Me.timeLabel.Text = String.Empty               '時間
            Me.memoTextBox.Text = String.Empty             'メモ内容
            Me.countLabel.Text = String.Empty              '0件の場合は、件数を表示ない 

            '初期モード:追加モード
            Me.mode.Value = MODE_APPEND

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
        If (Not Me.mode.Value.Equals(MODE_APPEND)) Then '追加モード以外
            memoDataRow.CUSTMEMOHIS_SEQNO = CType(Me.activeSEQNO.Value, Long)

            '2013/07/05 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
            memoDataRow.ROW_LOCK_VERSION = CType(Me.activeCSTMemoLockVersionHidden.Value, Long)
            memoDataRow.CST_ROW_LOCK_VERSION = CType(Me.cstLockVersionHidden.Value, Long)
            '2013/07/05 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        End If
        memoDataRow.MEMO = Me.memoTextBox.Text

        '追加する
        memoDataTbl.AddSC3080204CustMemoRow(memoDataRow)

    End Sub


    ' ''' <summary>
    ' ''' 削除ボタン押下時。
    ' ''' </summary>
    ' ''' <param name="sender">イベント発生元</param>
    ' ''' <param name="e">イベントデータ</param>
    ' ''' <remarks></remarks>
    'Protected Sub deleteButton_Click(sender As Object, e As System.EventArgs) Handles deleteButton.Click

    '    Dim ret As Integer = 0
    '    Dim msgID As Integer = 0
    '    Dim bizClass As New SC3080204BusinessLogic
    '    Dim memoDataTbl As New SC3080204DataSet.SC3080204CustMemoDataTable

    '    '画面の値を取得する
    '    Me.GetDisplayValues(memoDataTbl)

    '    '顧客メモ削除
    '    ret = bizClass.DeleteCustomerMemo(memoDataTbl, msgID)

    '    '件数
    '    Me.listCountHidden.Value = CType(CType(Me.listCountHidden.Value, Integer) - 1, String)
    '    Me.countLabel.Text = String.Format(WebWordUtility.GetWord(2), Me.listCountHidden.Value)

    '    '現在選択行の次の行を選択する
    '    '最後の明細の場合は、ひつと前の行を選択する
    '    Dim seqnoList As New List(Of String)
    '    Dim seqno As String = String.Empty

    '    'SEQNo配列を設定する
    '    Dim i As Integer
    '    For i = 0 To Me.memoRepeater.Items.Count - 1
    '        If (Me.memoRepeater.Items(i).Visible = True) Then
    '            seqno = CType(Me.memoRepeater.Items(i).FindControl("seqnoHidden"), HiddenField).Value
    '            seqnoList.Add(seqno)

    '            '削除行を非表示にする
    '            If (Me.activeSEQNO.Value = seqno) Then
    '                Me.memoRepeater.Items(i).Visible = False
    '            End If
    '        End If
    '    Next

    '    '１件になった場合はすべてをクリアする
    '    If (seqnoList.Count = 1) Then
    '        Call SetMemoList()
    '        Exit Sub
    '    End If

    '    '表示されている前の行を更新する
    '    Dim beforeSeqno As String = String.Empty
    '    For i = seqnoList.Count - 1 To 0 Step -1
    '        seqno = CType(seqnoList.Item(i), String)

    '        If (Me.activeSEQNO.Value = seqno) Then
    '            If (String.IsNullOrEmpty(beforeSeqno) = True) Then
    '                '一番したのメモが選択された場合
    '                If (i <> 0) Then
    '                    beforeSeqno = CType(seqnoList.Item(i - 1), String)
    '                End If
    '            End If

    '            Me.activeSEQNO.Value = beforeSeqno

    '            Exit For
    '        End If

    '        beforeSeqno = seqno
    '    Next

    '    '現在時の設定
    '    Me.todayHidden.Value = Format(Now(), "yyyy/MM/dd")
    '    Me.nowTimeHidden.Value = Format(Now(), "HH:mm")

    'End Sub

    Private _callbackResult As String

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


            '顧客メモ入力チェック
            If (method.Equals(MethodInputCheck)) Then

                Dim msgID As Integer = 0

                'メモ内容をセットする

                '2013/07/05 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
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
                '2013/07/05 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

            End If

        Catch ex As Exception

            _callbackResult = ErrorText + "," + ex.Message

        End Try

    End Sub

    ''' <summary>
    ''' 顧客メモ削除。
    ''' </summary>
    ''' <remarks></remarks>
    Protected Function deleteMemo(ByVal seqno As Long, ByVal linecount As Integer) As String

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
        Me.countLabel.Text = String.Format(WebWordUtility.GetWord(2), Me.listCountHidden.Value)
        resultString = resultString & Me.listCountHidden.Value & ","
        resultString = resultString & Me.countLabel.Text & ","
        resultString = resultString & "OK"

        ''現在選択行の次の行を選択する
        ''最後の明細の場合は、ひつと前の行を選択する
        'Dim seqnoList As New List(Of String)
        'Dim focusSeqno As String = String.Empty

        ''SEQNo配列を設定する
        'Dim i As Integer
        'Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("start **********************")
        'For i = 0 To Me.memoRepeater.Items.Count - 1
        '    If (Me.memoRepeater.Items(i).Visible = True) Then
        '        focusSeqno = CType(Me.memoRepeater.Items(i).FindControl("seqnoHidden"), HiddenField).Value
        '        seqnoList.Add(focusSeqno)

        '        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("focusSeqno=" + focusSeqno)

        '        '削除行を非表示にする
        '        If (focusSeqno.Equals(CType(seqno, String)) = True) Then
        '            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("Visilbe=FALSE=" + focusSeqno)
        '            Me.memoRepeater.Items(i).Visible = False

        '        End If
        '    End If
        'Next
        'Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("end **********************")

        ''１件になった場合はすべてをクリアする
        'If (seqnoList.Count = 1) Then
        '    resultString = resultString & "ZERO"
        '    Return resultString
        'End If

        ''表示されている前の行を更新する
        'Dim beforeSeqno As String = String.Empty
        'For i = seqnoList.Count - 1 To 0 Step -1
        '    focusSeqno = CType(seqnoList.Item(i), String)

        '    If (focusSeqno.Equals(CType(seqno, String)) = True) Then
        '        If (String.IsNullOrEmpty(beforeSeqno) = True) Then
        '            '一番したのメモが選択された場合
        '            If (i <> 0) Then
        '                beforeSeqno = CType(seqnoList.Item(i - 1), String)
        '            End If
        '        End If

        '        resultString = resultString & beforeSeqno

        '        Exit For
        '    End If

        '    beforeSeqno = focusSeqno
        'Next

        Return resultString

    End Function

End Class
