'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3080101.aspx.vb
'─────────────────────────────────────
'機能： 顧客検索一覧
'補足： 
'作成： 2011/11/18 TCS 安田
'更新： 2012/01/26 TCS 安田 【SALES_1B】顧客種別、顧客タイプ　アイコンの名称取得の変更 (不具合対応)
'更新： 2012/01/26 TCS 安田 【SALES_1B】TCVパラメーター変更
'更新： 2012/01/26 TCS 安田 【SALES_1B】次画面より戻るボタン対応
'更新： 2012/04/26 TCS 安田 HTMLエンコード対応
'更新： 2012/05/17 TCS 安田 クルクル対応 
'更新： 2012/08/13 TCS 安田 【SALES_3】顧客編集より顧客検索機能追加
'更新： 2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善
'更新： 2012/11/15 TCS 藤井 【A.STEP2】次世代e-CRB  新車タブレット横展開に向けた機能開発
'更新： 2013/06/30 TCS 趙   【A STEP2】i-CROP新DB適応に向けた機能開発（既存流用）
'更新： 2013/12/03 TCS 森    Aカード情報相互連携開発
'更新： 2015/06/08 TCS 中村 TMT課題対応(#2)
'更新： 2018/06/15 TCS 舩橋 TKM Next Gen e-CRB Project Application development Block B-1
'更新： 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-3
'─────────────────────────────────────

Option Explicit On
Option Strict On

Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.CustomerInfo.Search.BizLogic
Imports Toyota.eCRB.CustomerInfo.Search.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Web.Controls
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
'2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 START
Imports Toyota.eCRB.Common.VisitResult.BizLogic
'2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 END

''' <summary>
''' SC3080101(顧客検索一覧)
''' Webページのプレゼンテーション層
''' </summary>
''' <remarks>顧客検索一覧</remarks>
Partial Class Pages_SC3080101
    Inherits BasePage

#Region "定数"

    ''' <summary>
    ''' セッションキー
    ''' </summary>
    ''' <remarks></remarks>
    Public Const SESSION_KEY_SERCHTYPE As String = "searchType"                  '検索タイプ (1: 顧客名称、2: VIN、3: 車両登録No、4: 電話番号/携帯番号)
    Public Const SESSION_KEY_SERCHSTRING As String = "searchString"              '検索文字列
    Public Const SESSION_KEY_SERCHDIRECTION As String = "searchDirection"        '検索方向 (1:前方一致、2:あいまい検索、3:完全一致)
    ' 2012/08/13 TCS 安田 【SALES_3】顧客編集より顧客検索機能追加 START
    Public Const SESSION_KEY_SERCHFLG As String = "searchTelFlg"                 '電話番号検索フラグ
    ' 2012/08/13 TCS 安田 【SALES_3】顧客編集より顧客検索機能追加 END
    '2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 START
    ''' <summary> セッションキー 来店実績連番</summary>
    Private Const SESSION_KEY_VISITSEQ As String = "SearchKey.VISITSEQ"
    ''' <summary> セッションキー 来店実績の来店人数</summary>
    Private Const SESSION_KEY_WALKINNUM As String = "SearchKey.WALKINNUM"
    '2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 END

    ''' <summary>顧客種別</summary>
    Private Const SESSION_KEY_CSTKIND As String = "SearchKey.CSTKIND"
    ''' <summary>顧客分類</summary>
    Private Const SESSION_KEY_CUSTOMERCLASS As String = "SearchKey.CUSTOMERCLASS"
    ''' <summary>活動先顧客コード</summary>
    Private Const SESSION_KEY_CRCUSTID As String = "SearchKey.CRCUSTID"
    ''' <summary>車両ID</summary>
    Private Const SESSION_KEY_VCLID As String = "SearchKey.VCLID"
    ''' <summary>FOLLOW_UP_BOX</summary>
    Private Const SESSION_KEY_FOLLOW_UP_BOX As String = "SearchKey.FOLLOW_UP_BOX"
    ''' <summary>Follow-upBoxの店舗コード</summary>
    Private Const SESSION_KEY_FLLWUPBOX_STRCD As String = "SearchKey.FLLWUPBOX_STRCD"
    ''' <summary>Follow-upBoxの店舗コード</summary>
    Private Const SESSION_KEY_FLLWUPBOX_STRCD2 As String = "FLLWUPBOX_STRCD"
    ''' <summary>モード 1:顧客検索一覧、2:顧客編集、3:車両編集、4:顧客メモ</summary>
    Private Const SESSION_KEY_MODE As String = "SearchKey.MODE"
    ''' <summary>セールススタッフコード</summary>
    Private Const SESSION_KEY_SALESSTAFFCD As String = "SearchKey.SALESSTAFFCD"

    '2012/01/26 TCS 安田 【SALES_1B】次画面より戻るボタン対応 START
    ''' <summary>ソート方向</summary>
    Private Const SESSION_KEY_SORTORDER As String = "SearchKey.SORTORDER"
    ''' <summary>ソート項目</summary>
    Private Const SESSION_KEY_SORTTYPE As String = "SearchKey.SORTTYPE"
    ''' <summary>ページ番号</summary>
    Private Const SESSION_KEY_CURRENTPAGENO As String = "SearchKey.CURRENTPAGENO"
    '2012/01/26 TCS 安田 【SALES_1B】次画面より戻るボタン対応 END

    ''' <summary>画像登録なし時のアイコン</summary>
    Private Const NO_IMAGE_ICON As String = "../Styles/Images/Nnsc05-01Portraits01.png"

    '2018/06/15 TCS 舩橋 TKM Next Gen e-CRB Project Application development Block B-1 START
    ''' <summary>Loyal Customer（L）アイコン</summary>
    Private Const LOYALCOUSTOMER_L As String = "../Styles/Images/L.png"
    '2018/06/15 TCS 舩橋 TKM Next Gen e-CRB Project Application development Block B-1 END

    ''' <summary>
    ''' 1ページあたりの表示件数
    ''' </summary>
    ''' <remarks></remarks>
    Public Const PAGEMAXLINE As Integer = 50

    ''' <summary>
    ''' 検索結果が0件です。メッセージＩＤ
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ID_ZERO_MESSAGE As Integer = 11

    ''' <summary>
    ''' 次の{0}件を読み込むメッセージＩＤ
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ID_NEXTLINE_MESSAGE As Integer = 9

    ''' <summary>
    ''' 前の{0}件を読み込むメッセージＩＤ
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ID_BEFORM_MESSAGE As Integer = 10

    '2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 START
    ''' <summary>
    ''' 別のスタッフによって顧客情報の登録が行われた場合のメッセージＩＤ
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ID_ALREADYUPDATEDCUSTOMERINFO_MESSAGE As Integer = 15
    ''' <summary>
    ''' 別のスタッフによって顧客情報の登録が行われた
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ALREADYUPDATEDCUSTOMERINFO As Integer = 5004
    '2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 END

    ''' <summary>
    ''' フッター　(メインメニューへ)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MAIN_MENU As Integer = 100

    ''' <summary>
    ''' フッター　(顧客詳細へ)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CUSTOMER_SEARCH As Integer = 200

    ' 2012/02/29 TCS 小野 【SALES_2】 START
    ''' <summary>
    ''' 1フッター（ショールーム）
    ''' </summary>
    Private Const SHOW_ROOM As Integer = 1200
    ' 2012/02/29 TCS 小野 【SALES_2】 END

    ' 2013/12/03 TCS 森    Aカード情報相互連携開発 START
    ''' <summary>
    ''' 組織ID取得用：店舗内全組織ID
    ''' </summary>
    ''' <remarks></remarks>
    Public Const AllOrg As String = "allOrg"

    ''' <summary>
    ''' 組織ID取得用：自チーム組織ID
    ''' </summary>
    ''' <remarks></remarks>
    Public Const TeamOrg As String = "teamOrg"
    ' 2013/12/03 TCS 森    Aカード情報相互連携開発 END

#End Region

#Region "イベント"

    Protected Sub PageInitialize()

        '以下の機能は、使用しない (全件読み込みとする)
        ''検索開始位置
        'Me.fromNoHidden.Value = "0"
        ''検索終了位置
        'Me.tonoHidden.Value = CType(PAGEMAXLINE, String)
        ''カレントページ
        'Me.currentPageHidden.Value = "1"

        '2012/01/26 TCS 安田 【SALES_1B】次画面より戻るボタン対応 START
        'ソート方向 
        If (ContainsKey(ScreenPos.Current, SESSION_KEY_SORTORDER) = False) Then
            SetValue(ScreenPos.Current, SESSION_KEY_SORTORDER, SC3080101TableAdapter.IdOrderAsc)
        End If
        'ソート方向 (1:昇順)
        Me.sortOrderHidden.Value = CType(GetValue(ScreenPos.Current, SESSION_KEY_SORTORDER, False), String)

        'ソート項目 
        If (ContainsKey(ScreenPos.Current, SESSION_KEY_SORTTYPE) = False) Then
            SetValue(ScreenPos.Current, SESSION_KEY_SORTTYPE, SC3080101TableAdapter.IdSortName)
        End If
        'ソート項目 (1:名称)
        Me.sortTypeHidden.Value = CType(GetValue(ScreenPos.Current, SESSION_KEY_SORTTYPE, False), String)

        'ページ番号
        If (ContainsKey(ScreenPos.Current, SESSION_KEY_CURRENTPAGENO) = False) Then
            SetValue(ScreenPos.Current, SESSION_KEY_CURRENTPAGENO, 1)
        End If
        '2012/01/26 TCS 安田 【SALES_1B】次画面より戻るボタン対応 END

        ' 2012/08/13 TCS 安田 【SALES_3】顧客編集より顧客検索機能追加 START
        Dim serchFlg As Integer = 0
        If (Me.ContainsKey(ScreenPos.Current, SESSION_KEY_SERCHFLG) = True) Then

            '電話番号検索フラグ＝1:顧客編集より電話番号検索
            serchFlg = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_SERCHFLG, False), Integer)
            If (serchFlg = 1) Then

                '検索窓を非活性にする
                CType(Me.Master, CommonMasterPage).SearchBox.Enabled = False

                '検索窓を長いバージョン→短いバージョンにする
                '顧客検索一覧は、CommonMasterPage.Master.vbで、長いバージョンにしてしまうため
                JavaScriptUtility.RegisterStartupScript(Me, " <script type='text/javascript'>" & "$(function () { custSearchSize = 'L'; });" & "</script>", "smallCustInput")

            End If
        End If
        Me.searchFlgHidden.Value = serchFlg.ToString

        '顧客選択時の確認メッセージ取得
        Me.selectConfirmHidden.Value = WebWordUtility.GetWord(14)
        ' 2012/08/13 TCS 安田 【SALES_3】顧客編集より顧客検索機能追加 END

        ' 2013/12/03 TCS 森    Aカード情報相互連携開発 START
        Dim orgList As String

        orgList = SC3080101BusinessLogic.GetMyTeamId(StaffContext.Current.TeamCD)
        Me.searchOrgTeamList.Value = orgList

        ' 2013/12/03 TCS 森    Aカード情報相互連携開発 END

        '顧客件数取得
        Dim msgID As Integer = 0
        Dim serchTable As SC3080101DataSet.SC3080101SerchDataTable

        '検索条件用のDataTableを作成する
        serchTable = GetSerchTable()

        Dim count As Integer = SC3080101BusinessLogic.GetCountCustomer(serchTable)

        '合計件数を出力
        Dim goukeiStr As New StringBuilder(1000)
        goukeiStr.AppendFormat(WebWordUtility.GetWord(1), count)
        Me.goukeiLabel.Text = HttpUtility.HtmlEncode(goukeiStr.ToString)

        '検索結果が0件です。
        If (count = 0) Then
            Me.resultListPanel.Visible = False
            ShowMessageBox(ID_ZERO_MESSAGE)

            ' 2012/08/13 TCS 安田 【SALES_3】顧客編集より顧客検索機能追加 START
            '電話番号検索フラグ＝1:顧客編集の電話番号検索ボタンより遷移時
            '戻るフラグをONにする
            If (serchFlg = 1) Then
                Me.backFlgHidden.Value = "1"
            End If
            ' 2012/08/13 TCS 安田 【SALES_3】顧客編集より顧客検索機能追加 END

        Else
            Me.resultListPanel.Visible = True
            '次の{0}件を読み込む
            Dim massageStr As New StringBuilder(1000)
            massageStr.AppendFormat(HttpUtility.HtmlEncode(WebWordUtility.GetWord(ID_NEXTLINE_MESSAGE)), PAGEMAXLINE)
            customerRepeater.ForwardPagerLabel = (massageStr.ToString)

            Me.nextMessageHidden.Value = (massageStr.ToString)
            Me.nextLastMessageHidden.Value = (massageStr.ToString)

            '前の{0}件を読み込む
            Dim massageStr2 As New StringBuilder(1000)
            massageStr2.AppendFormat(HttpUtility.HtmlEncode(WebWordUtility.GetWord(ID_BEFORM_MESSAGE)), PAGEMAXLINE)
            customerRepeater.RewindPagerLabel = (massageStr2.ToString)

            Me.forwordMessageHidden.Value = (massageStr2.ToString)
            Me.forwordFirstMessageHidden.Value = (massageStr2.ToString)

            '2012/01/26 TCS 安田 【SALES_1B】次画面より戻るボタン対応 START
            'ページ番号
            customerRepeater.CurrentPage = CType(Me.GetValue(ScreenPos.Current, SESSION_KEY_CURRENTPAGENO, False), Integer)
            '2012/01/26 TCS 安田 【SALES_1B】次画面より戻るボタン対応 END

            'この処理はなくす
            'Dim lastCount As Integer = 0
            'lastCount = count Mod PAGEMAXLINE
            'If (count > PAGEMAXLINE And lastCount > 0) Then
            '    massageStr.Clear()
            '    massageStr.AppendFormat(WebWordUtility.GetWord(ID_NEXTLINE_MESSAGE), lastCount)
            '    Me.nextLastMessageHidden.Value = massageStr.ToString

            '    massageStr2.Clear()
            '    massageStr2.AppendFormat(WebWordUtility.GetWord(ID_BEFORM_MESSAGE), lastCount)
            '    Me.forwordFirstMessageHidden.Value = massageStr2.ToString
            'End If

        End If

        'SA SC名取得
        ScLabel.Text = HttpUtility.HtmlEncode(SC3080101BusinessLogic.GetSSName(serchTable))
        SaLabel.Text = HttpUtility.HtmlEncode(SC3080101BusinessLogic.GetSAName(serchTable))
    End Sub
    ''' <summary>
    ''' ロード次の処理を実施します。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        '2012/04/26 TCS 安田 HTMLエンコード対応 START
        If Not Page.IsPostBack Then

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("Page_Load Not Page.IsPostBack Start")
            'ログ出力 End *****************************************************************************

            PageInitialize()

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("Page_Load Not Page.IsPostBack End")
            'ログ出力 End *****************************************************************************

        End If
        '2012/04/26 TCS 安田 HTMLエンコード対応 END

        'フッターの制御
        InitFooterEvent()

    End Sub

    ''' <summary>
    ''' 顧客検索一覧(CustomerRepeater)のソートイベント。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub sortButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles sortButton.Click

        '特に処理をしない
        'customerRepeater_ClientCallbackが呼び出される。

    End Sub

    ''' <summary>
    ''' 顧客検索一覧(CustomerRepeater)の顧客選択イベント。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks>顧客詳細画面へ遷移する。</remarks>
    Protected Sub nextButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles nextButton.Click

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("nextButton_Click Start")
        'ログ出力 End *****************************************************************************

        '2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 START
        '来店実績連番が存在する場合、選択された顧客で来店実績を登録
        If Not "1".Equals(Me.updateVisitCustomerInfoFlg.Value) AndAlso Me.ContainsKey(ScreenPos.Current, SESSION_KEY_VISITSEQ) = True Then
            Dim visitSeq As Long = CType(GetValue(ScreenPos.Current, SESSION_KEY_VISITSEQ, False), Long)
            Dim returnID As Integer = 0
            Dim biz As New UpdateSalesVisitBusinessLogic
            biz.UpdateVisitCustomerInfo(visitSeq, Me.cstkindHidden.Value, _
                                        Me.crcustidHidden.Value, Me.salessStaffcdHidden.Value, _
                                        "SC3080101", returnID)
            If (returnID = ALREADYUPDATEDCUSTOMERINFO) Then
                ShowMessageBox(ID_ALREADYUPDATEDCUSTOMERINFO_MESSAGE)
                '顧客が登録済みフラグ
                Me.updateVisitCustomerInfoFlg.Value = "1"
                Return
            Else
                '来店実績連番をセッションにセット
                Me.SetValue(ScreenPos.Next, SESSION_KEY_VISITSEQ, visitSeq)
                '来店人数をセッションにセット
                If Me.ContainsKey(ScreenPos.Current, SESSION_KEY_WALKINNUM) = True Then
                    Dim walkinName As Integer = CType(GetValue(ScreenPos.Current, SESSION_KEY_WALKINNUM, False), Integer)
                    SetValue(ScreenPos.Next, SESSION_KEY_WALKINNUM, walkinName)
                End If
            End If
        End If
        '2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 END

        Me.SetValue(ScreenPos.Next, SESSION_KEY_MODE, "1")                                   '1:顧客検索一覧
        Me.SetValue(ScreenPos.Next, SESSION_KEY_CSTKIND, Me.cstkindHidden.Value)             '1：自社客 / 2：未取引客
        Me.SetValue(ScreenPos.Next, SESSION_KEY_CUSTOMERCLASS, "1")                          '1：所有者 / 2：使用者 / 3：その他
        Me.SetValue(ScreenPos.Next, SESSION_KEY_CRCUSTID, Me.crcustidHidden.Value)           'オリジナルID：自社客 / 未取引客連番：未取引客
        Me.SetValue(ScreenPos.Next, SESSION_KEY_VCLID, Me.vclHidden.Value)                   'VIN：自社客 / 車両シーケンスNo.：未取引客
        Me.SetValue(ScreenPos.Next, SESSION_KEY_SALESSTAFFCD, Me.salessStaffcdHidden.Value)  'セールススタッフコード
        'Me.SetValue(ScreenPos.Next, SESSION_KEY_FOLLOW_UP_BOX, "3255")  'SEQNO
        'Me.SetValue(ScreenPos.Next, SESSION_KEY_FLLWUPBOX_STRCD, String.Empty)  'Follow-upBoxの店舗コード
        'Me.SetValue(ScreenPos.Next, SESSION_KEY_FLLWUPBOX_STRCD2, String.Empty)  'Follow-upBoxの店舗コード

        '2012/01/26 TCS 安田 【SALES_1B】次画面より戻るボタン対応 START
        Me.SetValue(ScreenPos.Current, SESSION_KEY_SORTORDER, Me.sortOrderHidden.Value)
        Me.SetValue(ScreenPos.Current, SESSION_KEY_SORTTYPE, Me.sortTypeHidden.Value)
        Me.SetValue(ScreenPos.Current, SESSION_KEY_CURRENTPAGENO, customerRepeater.CurrentPage)
        '2012/01/26 TCS 安田 【SALES_1B】次画面より戻るボタン対応 END

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("nextButton_Click End")
        'ログ出力 End *****************************************************************************

        '顧客詳細画面へ遷移
        Me.RedirectNextScreen("SC3080201")

    End Sub

    ''' <summary>
    ''' 顧客検索一覧(CustomerRepeater)の検索イベント。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub customerRepeater_ClientCallback(ByVal sender As Object, ByVal e As Toyota.eCRB.SystemFrameworks.Web.Controls.ClientCallbackEventArgs) Handles customerRepeater.ClientCallback

        '2012/04/26 TCS 安田 HTMLエンコード対応 START
        Dim beginRowIndex As Integer = 0
        If (Integer.TryParse(CType(e.Arguments("beginRowIndex"), String), beginRowIndex)) Then

            ' 2012/01/26 TCS 安田 【SALES_1B】顧客種別、顧客タイプ　アイコンの名称取得の変更 (不具合対応) START
            Dim icoJisya As String = WebWordUtility.GetWord(5)      '自
            Dim icoMikokyaku As String = WebWordUtility.GetWord(6)  '未
            Dim icoKojin As String = WebWordUtility.GetWord(7)      '個
            Dim icoHojin As String = WebWordUtility.GetWord(8)      '法
            ' 2012/01/26 TCS 安田 【SALES_1B】顧客種別、顧客タイプ　アイコンの名称取得の変更 (不具合対応) END

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("customerRepeater_ClientCallback Start")
            'ログ出力 End *****************************************************************************

            Dim rows As New StringBuilder(1000)
            Dim firstElement As Boolean = True

            Dim msgID As Integer = 0
            Dim serchTable As SC3080101DataSet.SC3080101SerchDataTable

            '検索条件用のDataTableを作成する
            serchTable = GetSerchTable()

            '顧客件数取得
            Dim count As Integer = SC3080101BusinessLogic.GetCountCustomer(serchTable)

            '合計件数をセットする
            Dim goukeiStr As New StringBuilder(1000)
            goukeiStr.AppendFormat(WebWordUtility.GetWord(1), count)
            Me.goukeiLabel.Text = HttpUtility.HtmlEncode(goukeiStr.ToString)

            If (count > 0) Then
                Me.resultListPanel.Visible = True

                '顔写真の保存先フォルダ(Web向け)取得
                Dim imagePath As String = SC3080101BusinessLogic.GetImagePath()

                '顧客一覧取得
                Dim customerList As SC3080101DataSet.SC3080101CustDataTable = _
                    SC3080101BusinessLogic.GetCustomerList(serchTable)
                Dim customerRow As SC3080101DataSet.SC3080101CustRow

                '2013/12/03 TCS 高橋    Aカード情報相互連携開発 START
                Dim searchDataRow As SC3080101DataSet.SC3080101SerchRow = serchTable.Item(0)
                '顧客詳細から遷移してきたか
                Dim fromCustomerDetail As Boolean = (searchDataRow.SERCHFLG = 1)
                '2013/12/03 TCS 高橋    Aカード情報相互連携開発 END

                For i As Integer = beginRowIndex To customerList.Rows.Count - 1

                    customerRow = customerList.Item(i)

                    Dim updateFlg As Integer = 1
                    Dim name As String = customerRow.NAME.Trim      'お客様名
                    Dim telno As String = customerRow.TELNO.Trim    '電話番号
                    Dim mobile As String = customerRow.MOBILE.Trim  '携帯番号

                    If (customerRow.NAMETITLE.Trim.Length > 0) Then
                        name = name + " " + customerRow.NAMETITLE.Trim
                    End If

                    If StaffContext.Current.OpeCD = Operation.SSF Then

                        '2013/12/03 TCS 高橋    Aカード情報相互連携開発 START
                        '・セールススタッフ　かつ
                        '・チームリーダーが顧客詳細で検索した場合、またはチームリーダーでない
                        '　タップした行の顧客が担当以外の顧客
                        '処理を行わない
                        If ((fromCustomerDetail And StaffContext.Current.TeamLeader) _
                                OrElse StaffContext.Current.TeamLeader = False) _
                            AndAlso customerRow.STAFFCD.Trim().Equals(StaffContext.Current.Account().Trim()) = False Then
                            '2013/12/03 TCS 高橋    Aカード情報相互連携開発 END

                            'セールススタッフ　：　担当以外の顧客は電話番号マスク、顧客詳細(顧客情報)への遷移無し
                            '※マスクについて
                            '・下4桁を通常表示し、下4桁以外を「*」表示する
                            If (telno.Length <= 4) Then
                                telno = "****"
                            Else
                                telno = telno.Substring(0, telno.Length - 4) + "****"
                            End If

                            If (mobile.Length <= 4) Then
                                mobile = "****"
                            Else
                                mobile = mobile.Substring(0, mobile.Length - 4) + "****"
                            End If

                            updateFlg = 0
                        Else

                            updateFlg = 1
                        End If
                    End If

                    If (firstElement) Then
                        firstElement = False
                    Else
                        rows.Append(",")
                    End If
                    Dim imgpath As String

                    '顧客種別
                    Dim sstkindnm As String = String.Empty
                    If (customerRow.CSTKIND.Equals("2")) Then
                        sstkindnm = icoMikokyaku
                    Else
                        sstkindnm = icoJisya
                    End If

                    '顧客タイプ
                    Dim custypenm As String = String.Empty
                    If (customerRow.CUSTYPE.Equals("0")) Then
                        custypenm = icoHojin
                    End If
                    If (customerRow.CUSTYPE.Equals("1")) Then
                        custypenm = icoKojin
                    End If

                    'ファイルパス
                    If (String.IsNullOrEmpty(Trim(customerRow.IMAGEFILE_S)) = True) Then
                        imgpath = NO_IMAGE_ICON
                    Else
                        imgpath = imagePath & customerRow.IMAGEFILE_S
                    End If
                    imgpath = Me.ResolveClientUrl(imgpath)

                    '偶数／奇数行判定
                    Dim flg As Integer
                    flg = i Mod 2

                    'シーケンシャル番号
                    '2013/06/30 TCS 趙 2013/10対応版 既存流用 START
                    Dim seqno As Decimal = 0
                    '2013/06/30 TCS 趙 2013/10対応版 既存流用 END
                    If (customerRow.IsSEQNONull() = True) Then
                        seqno = 0
                    Else
                        seqno = customerRow.SEQNO
                    End If

                    Dim lastPageFlg As Integer

                    Dim lastCount As Integer = 0
                    lastCount = ((count \ PAGEMAXLINE) - 1) * PAGEMAXLINE
                    If (lastCount < i) Then
                        lastPageFlg = 1
                    Else
                        lastPageFlg = 0
                    End If

                    '2013/12/03 TCS 舩橋 TKM Next Gen e-CRB Project Application development Block B-1 START
                    Dim imgLoyalCustomerL As String
                    If (customerRow.IMP_VCL_FLG.Equals("2")) Then
                        imgLoyalCustomerL = Me.ResolveClientUrl(LOYALCOUSTOMER_L)
                    Else
                        imgLoyalCustomerL = String.Empty
                    End If
                    '2012/11/15 TCS 舩橋 TKM Next Gen e-CRB Project Application development Block B-1 END

                    ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-3 START
                    '顧客情報のパラメーター作成
                    rows.AppendFormat("{{ ""NO"" : {0}, " & _
                                      """IMAGEPATH"" : ""{1}"", " & _
                                      """NAMETITLE"" : ""{2}""," & _
                                      """NAME"" : ""{3}""," & _
                                      """TELNO"" : ""{4}""," & _
                                      """MOBILE"" : ""{5}""," & _
                                      """SERIESNM"" : ""{6}""," & _
                                      """VCLREGNO"" : ""{7}""," & _
                                      """VIN"" : ""{8}""," & _
                                      """SSUSERNAME"" : ""{9}""," & _
                                      """SAUSERNAME"" : ""{10}""," & _
                                      """STAFFCD"" : ""{11}""," & _
                                      """CSTKINDNM"" : ""{12}""," & _
                                      """CSTKIND"" : ""{13}""," & _
                                      """CRCUSTID"" : ""{14}""," & _
                                      """STAFFCD"" : ""{15}""," & _
                                      """CST_SOCIALNUM"" : ""{16}""," & _
                                      """SEQNO"" : {17}," & _
                                      """maxrow"" : {18} ," & _
                                      """updateFlg"" : {19} ," & _
                                      """flg"" : {20} ," & _
                                      """lastPageFlg"" : {21} ," & _
                                      """imgLoyalCustomerL"" : ""{22}"" ," & _
                                      """joinType"" : ""{23}"" }}", _
                                      (i + 1), _
                                      HttpUtility.JavaScriptStringEncode(imgpath), _
                                      HttpUtility.JavaScriptStringEncode(HttpUtility.HtmlEncode(customerRow.NAMETITLE)), _
                                      HttpUtility.JavaScriptStringEncode(HttpUtility.HtmlEncode(SpaceToHeifun(name))), _
                                      HttpUtility.JavaScriptStringEncode(HttpUtility.HtmlEncode(SpaceToHeifun(telno))), _
                                      HttpUtility.JavaScriptStringEncode(HttpUtility.HtmlEncode(SpaceToHeifun(mobile))), _
                                      HttpUtility.JavaScriptStringEncode(HttpUtility.HtmlEncode(SpaceToHeifun(customerRow.SERIESNM.Trim))), _
                                      HttpUtility.JavaScriptStringEncode(HttpUtility.HtmlEncode(SpaceToHeifun(customerRow.VCLREGNO.Trim))), _
                                      HttpUtility.JavaScriptStringEncode(HttpUtility.HtmlEncode(SpaceToHeifun(customerRow.VIN.Trim))), _
                                      HttpUtility.JavaScriptStringEncode(HttpUtility.HtmlEncode(SpaceToHeifun(customerRow.SSUSERNAME.Trim))), _
                                      HttpUtility.JavaScriptStringEncode(HttpUtility.HtmlEncode(SpaceToHeifun(customerRow.SAUSERNAME.Trim))), _
                                      HttpUtility.JavaScriptStringEncode(HttpUtility.HtmlEncode(customerRow.STAFFCD.Trim)), _
                                      HttpUtility.JavaScriptStringEncode(HttpUtility.HtmlEncode(sstkindnm)), _
                                      HttpUtility.JavaScriptStringEncode(customerRow.CSTKIND.Trim), _
                                      HttpUtility.JavaScriptStringEncode(customerRow.CRCUSTID.Trim), _
                                      HttpUtility.JavaScriptStringEncode(customerRow.STAFFCD.Trim), _
                                      HttpUtility.JavaScriptStringEncode(customerRow.CST_SOCIALNUM.Trim), _
                                      seqno, _
                                      customerList.Rows.Count, _
                                      updateFlg, _
                                      flg, _
                                      lastPageFlg,
                                      HttpUtility.JavaScriptStringEncode(imgLoyalCustomerL), _
                                      HttpUtility.JavaScriptStringEncode(customerRow.CSTJOINTYPE))
                    ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-3 END
                Next

                e.Results("@rows") = "[" & rows.ToString() & "]"

            End If

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("customerRepeater_ClientCallback End")
            'ログ出力 End *****************************************************************************

        Else
            e.Results("@rows") = "[]"
        End If
        '2012/04/26 TCS 安田 HTMLエンコード対応 END

    End Sub

#End Region

#Region "メソット"
    '検索条件用のDataTableを作成する
    Protected Function GetSerchTable() As SC3080101DataSet.SC3080101SerchDataTable

        'セッション情報の取得
        '検索タイプ (1: 顧客名称、2: VIN、3: 車両登録No、4: 電話番号/携帯番号)
        Dim serchType As Integer = 0
        serchType = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_SERCHTYPE, False), Integer)
        '検索文字列
        Dim serchString As String = Nothing
        serchString = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_SERCHSTRING, False), String)

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSerchTable serchString = " + serchString)
        'ログ出力 End *****************************************************************************

        '検索方向 (1:前方一致、2:あいまい検索、3:完全一致))
        Dim serchDirection As Integer = 0
        If (Me.ContainsKey(ScreenPos.Current, SESSION_KEY_SERCHDIRECTION) = True) Then
            serchDirection = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_SERCHDIRECTION, False), Integer)
        Else
            serchDirection = SC3080101TableAdapter.IdSerchdirectionAfter
        End If

        'ログインユーザー情報取得用
        Dim context As StaffContext = StaffContext.Current

        Dim dlrcd As String = context.DlrCD         '自身の販売店コード
        Dim strcd As String = context.BrnCD         '自身の店舗コード
        Dim account As String = context.Account     '自身のアカウント

        'データテーブルに値をセットする
        Dim SerchDataTbl As New SC3080101DataSet.SC3080101SerchDataTable
        Dim SerchRow As SC3080101DataSet.SC3080101SerchRow = _
                                     SerchDataTbl.NewSC3080101SerchRow

        '2015/06/08 TCS 中村 TMT課題対応(#2) START
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSerchTable SerchRow.SERCHSTRING_Start = " + SerchRow.SERCHSTRING)
        '2015/06/08 TCS 中村 TMT課題対応(#2) END

        '検索条件のセット
        SerchRow.DLRCD = dlrcd                                          '販売店コード
        SerchRow.STRCD = strcd                                          '店舗コード
        SerchRow.SERCHTYPE = serchType                                  '検索タイプ

        'この方法はなくす　
        '必ず前方一致で検索する
        '最初１文字が*ならば、前方後方一致とする
        'If (serchString.Length > 0 AndAlso serchString.Substring(0, 1).Equals("*")) Then
        '    serchDirection = SC3080101DataTableTableAdapter.IdSerchdirectionAll
        '    serchString = serchString.Substring(1)
        'End If

        ' 2013/12/03 TCS 森    Aカード情報相互連携開発 START
        SerchRow.ORGNZ_ID = CType(Me.searchOrgTeamList.Value, String)   '自組織(及び配下組織)
        ' 2013/12/03 TCS 森    Aカード情報相互連携開発 END

        If (serchType = SC3080101TableAdapter.IdSerchTel) Then
            '電話番号時は、ハイフンを取り除く
            SerchRow.SERCHSTRING = serchString.Replace("-", "")             '検索文字列
        Else
            '電話番号以外（名称・VIN・車両登録No）で検索時
            SerchRow.SERCHSTRING = serchString.ToUpper                      '検索文字列 (大文字に変換する)
        End If
        SerchRow.SERCHSTRING = SerchRow.SERCHSTRING.Replace("*", "%")            '* → % であいまい検索にする

        SerchRow.SERCHDIRECTION = serchDirection                        '検索方向
        SerchRow.SORTTYPE = CType(Me.sortTypeHidden.Value, Integer)     'ソート項目
        SerchRow.SORTORDER = CType(Me.sortOrderHidden.Value, Integer)   'ソート方向
        '以下の機能は、使用しない (全件読み込みとする)
        SerchRow.FROMNO = 0
        SerchRow.TONO = 0
        'SerchRow.FROMNO = CType(Me.fromNoHidden.Value, Integer)         '検索開始位置
        'SerchRow.TONO = CType(Me.tonoHidden.Value, Integer)             '検索終了位置
        '2012/11/15 TCS 藤井 【A.STEP2】次世代e-CRB  新車タブレット横展開に向けた機能開発 START
        SerchRow.SERCHFLG = CType(Me.searchFlgHidden.Value, Integer)     '電話番号検索フラグ
        '2012/11/15 TCS 藤井 【A.STEP2】次世代e-CRB  新車タブレット横展開に向けた機能開発 END

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSerchTable SerchRow.DLRCD = " + SerchRow.DLRCD)
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSerchTable SerchRow.STRCD = " + SerchRow.STRCD)
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSerchTable SerchRow.SERCHSTRING = " + SerchRow.SERCHSTRING)
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSerchTable SerchRow.SERCHDIRECTION = " & SerchRow.SERCHDIRECTION)
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSerchTable sortTypeHidden = " + Me.sortTypeHidden.Value)
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSerchTable sortOrderHidden = " + Me.sortOrderHidden.Value)
        'ログ出力 End *****************************************************************************

        SerchDataTbl.AddSC3080101SerchRow(SerchRow)

        Return SerchDataTbl

    End Function

    ''' <summary>
    ''' フッターボタンの制御
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitFooterEvent()

        'メニュー
        Dim menuButton As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(MAIN_MENU)
        AddHandler menuButton.Click, _
            Sub()
                'メニューに遷移
                Me.RedirectNextScreen("SC3010203")
            End Sub

        '顧客詳細
        Dim custSearchButton As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(CUSTOMER_SEARCH)
        AddHandler custSearchButton.Click, _
            Sub()
                '顧客詳細に遷移
                Me.RedirectNextScreen("SC3080201")
            End Sub

        ' 2012/02/29 TCS 小野 【SALES_2】 START
        'ショールーム
        Dim ssvButton As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(SHOW_ROOM)
        If ssvButton IsNot Nothing Then
            AddHandler ssvButton.Click, _
            Sub()
                '受付メインに遷移
                Me.RedirectNextScreen("SC3100101")
            End Sub
        End If
        ' 2012/02/29 TCS 小野 【SALES_2】 END

        'TCSとの連携ボタン
        Dim tcvButton As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.TCV)
        AddHandler tcvButton.Click, AddressOf tcvButton_Click

        'ログイン権限がセールスマネージャ、ブランチマネージャ権限場合、顧客ボタンを非表示
        Dim OpeCD As Integer = StaffContext.Current.OpeCD
        Dim SSM As Integer = Operation.SSM
        Dim BM As Integer = Operation.BM
        If OpeCD = SSM Or OpeCD = BM Then
            CType(Me.Master, CommonMasterPage).GetFooterButton(CUSTOMER_SEARCH).Visible = False
        End If

    End Sub

    ''' <summary>
    ''' TCSとの連携ボタン
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub tcvButton_Click(sender As Object, e As CommonMasterFooterButtonClickEventArgs)

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("tcvButton_Click Start")
        'ログ出力 End *****************************************************************************

        Dim context As StaffContext = StaffContext.Current

        'TCV機能に渡す引数を設定
        e.Parameters.Add("DataSource", "none")
        e.Parameters.Add("MenuLockFlag", CType(False, String))
        e.Parameters.Add("CloseCallback", "closeCallbackFunction")
        e.Parameters.Add("StatusCallback", "statusCallbackFunction")
        e.Parameters.Add("Account", context.Account)
        e.Parameters.Add("AccountStrCd", context.BrnCD)
        e.Parameters.Add("DlrCd", context.DlrCD)

        '2012/01/26 TCS 安田 【SALES_1B】TCVパラメーター変更
        'e.Parameters.Add("NewActFlag", CType(False, String))
        e.Parameters.Add("OperationCode", context.OpeCD)
        e.Parameters.Add("BusinessFlg", False)
        e.Parameters.Add("ReadOnlyFlg", False)
        '2012/01/26 TCS 安田 【SALES_1B】TCVパラメーター変更

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("tcvButton_Click End")
        'ログ出力 End *****************************************************************************

    End Sub

    ''' <summary>
    ''' 空文字の場合にハイフンを返す
    ''' </summary>
    ''' <param name="val"></param>
    ''' <remarks></remarks>
    Private Function SpaceToHeifun(ByVal val As String) As String
        If (val.Length = 0) Then
            Return "-"
        Else
            Return val
        End If
    End Function

#End Region

    '2012/05/17 TCS 安田 クルクル対応 START

    ''' <summary>
    ''' 再表示ボタン(隠しボタン)押下時
    ''' </summary>
    ''' <param name="sender">ページオブジェクト</param>
    ''' <param name="e">イベント引数</param>
    ''' <remarks></remarks>
    Protected Sub refreshButton_Click(sender As Object, e As System.EventArgs) Handles refreshButton.Click

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("refreshButton_Click Start")
        'ログ出力 End *****************************************************************************

        If (Me.ContainsKey(ScreenPos.Current, SESSION_KEY_SERCHSTRING) = True) Then

            'パターン１：顧客を選択する前から、wifiが切れていたパターン
            '           →サーバーサイドの処理が実行されていない。 (ScreenPos.Currentに、セッション情報が存在する)
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("Me.PageInitialize()")

            '初期処理
            Me.PageInitialize()

        Else

            'パターン２：顧客を選択して、サーバーサイドの処理をしてから、wifiが切れていたパターン
            '           →すでに、サーバーサイドの処理が実行されている。 (ScreenPos.Currentに、セッション情報が存在しない)
            '           →内部的には、戻るボタンを押されたときと同じ処理をする。
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("Me.RedirectPrevScreen()")

            '前画面へ戻る
            Me.RedirectPrevScreen()

        End If

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("refreshButton_Click End")
        'ログ出力 End *****************************************************************************

    End Sub
    '2012/05/17 TCS 安田 クルクル対応 END


    '2012/08/13 TCS 安田 【SALES_3】顧客編集より顧客検索機能追加 START
    ''' <summary>
    ''' 電話番号検索フラグ＝1:顧客編集の電話番号検索ボタンより遷移時に前画面(顧客編集)へ戻る
    ''' </summary>
    ''' <param name="sender">ページオブジェクト</param>
    ''' <param name="e">イベント引数</param>
    ''' <remarks></remarks>
    Protected Sub backButton_Click(sender As Object, e As System.EventArgs) Handles backButton.Click

        '前画面へ戻る
        Me.RedirectPrevScreen()

    End Sub
    '2012/08/13 TCS 安田 【SALES_3】顧客編集より顧客検索機能追加 END

End Class
