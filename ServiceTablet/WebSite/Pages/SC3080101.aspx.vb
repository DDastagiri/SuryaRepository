Option Explicit On
Option Strict On

Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.iCROP.BizLogic.SC3080101
Imports Toyota.eCRB.iCROP.DataAccess.SC3080101
Imports Toyota.eCRB.SystemFrameworks.Web.Controls
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.iCROP.DataAccess.SC3080101.SC3080101DataSetTableAdapters

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
    Public Const SESSION_KEY_SERCHDIRECTION As String = "searchDirection"        '検索方向 (1:前方一致、2:あいまい検索)

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


    ''' <summary>画像登録なし時のアイコン</summary>
    Private Const NO_IMAGE_ICON As String = "../Styles/Images/Nnsc05-01Portraits01.png"


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

#End Region

#Region "イベント"

    ''' <summary>
    ''' ロード次の処理を実施します。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            '以下の機能は、使用しない (全件読み込みとする)
            ''検索開始位置
            'Me.fromNoHidden.Value = "0"
            ''検索終了位置
            'Me.tonoHidden.Value = CType(PAGEMAXLINE, String)
            ''カレントページ
            'Me.currentPageHidden.Value = "1"

            'ソート方向 (1:昇順)
            Me.sortOrderHidden.Value = CType(SC3080101DataTableTableAdapter.IdOrderAsc, String)

            'ソート項目 (1:名称)
            Me.sortTypeHidden.Value = CType(SC3080101DataTableTableAdapter.IdSortName, String)

            '顧客件数取得
            Dim msgID As Integer = 0
            Dim serchTable As SC3080101DataSet.SC3080101SerchDataTable

            '検索条件用のDataTableを作成する
            serchTable = GetSerchTable()

            Dim count As Integer = SC3080101BusinessLogic.GetCountCustomer(serchTable)

            '合計件数を出力
            Dim goukeiStr As New StringBuilder(1000)
            goukeiStr.AppendFormat(WebWordUtility.GetWord(1), count)
            Me.goukeiLabel.Text = goukeiStr.ToString

            '検索結果が0件です。
            If (count = 0) Then
                Me.resultListPanel.Visible = False
                ShowMessageBox(ID_ZERO_MESSAGE)
            Else
                Me.resultListPanel.Visible = True
                '次の{0}件を読み込む
                Dim massageStr As New StringBuilder(1000)
                massageStr.AppendFormat(WebWordUtility.GetWord(ID_NEXTLINE_MESSAGE), PAGEMAXLINE)
                customerRepeater.ForwardPagerLabel = massageStr.ToString

                '前の{0}件を読み込む
                Dim massageStr2 As New StringBuilder(1000)
                massageStr2.AppendFormat(WebWordUtility.GetWord(ID_BEFORM_MESSAGE), PAGEMAXLINE)
                customerRepeater.RewindPagerLabel = massageStr2.ToString
            End If

            'SA SC名取得
            ScLabel.Text = SC3080101BusinessLogic.GetSSName(serchTable)
            SaLabel.Text = SC3080101BusinessLogic.GetSAName(serchTable)


        End If

        'フッターの制御
        InitFooterEvent()

    End Sub

    ''' <summary>
    ''' 顧客検索一覧(CustomerRepeater)のソートイベント。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub sortButton_Click(sender As Object, e As System.EventArgs) Handles sortButton.Click

        '特に処理をしない
        'customerRepeater_ClientCallbackが呼び出される。

    End Sub

    ''' <summary>
    ''' 顧客検索一覧(CustomerRepeater)の顧客選択イベント。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks>顧客詳細画面へ遷移する。</remarks>
    Protected Sub nextButton_Click(sender As Object, e As System.EventArgs) Handles nextButton.Click

        Me.SetValue(ScreenPos.Next, SESSION_KEY_MODE, "1")                                   '1:顧客検索一覧
        Me.SetValue(ScreenPos.Next, SESSION_KEY_CSTKIND, Me.cstkindHidden.Value)             '1：自社客 / 2：未取引客
        Me.SetValue(ScreenPos.Next, SESSION_KEY_CUSTOMERCLASS, "1")                          '1：所有者 / 2：使用者 / 3：その他
        Me.SetValue(ScreenPos.Next, SESSION_KEY_CRCUSTID, Me.crcustidHidden.Value)           'オリジナルID：自社客 / 未取引客連番：未取引客
        Me.SetValue(ScreenPos.Next, SESSION_KEY_VCLID, Me.vclHidden.Value)                   'VIN：自社客 / 車両シーケンスNo.：未取引客
        Me.SetValue(ScreenPos.Next, SESSION_KEY_SALESSTAFFCD, Me.salessStaffcdHidden.Value)  'セールススタッフコード
        'Me.SetValue(ScreenPos.Next, SESSION_KEY_FOLLOW_UP_BOX, "3255")  'SEQNO
        'Me.SetValue(ScreenPos.Next, SESSION_KEY_FLLWUPBOX_STRCD, String.Empty)  'Follow-upBoxの店舗コード
        'Me.SetValue(ScreenPos.Next, SESSION_KEY_FLLWUPBOX_STRCD2, String.Empty)  'Follow-upBoxの店舗コード


        '顧客詳細画面へ遷移
        Me.RedirectNextScreen("SC3080201")

    End Sub

    ''' <summary>
    ''' 顧客検索一覧(CustomerRepeater)の検索イベント。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub customerRepeater_ClientCallback(sender As Object, e As Toyota.eCRB.SystemFrameworks.Web.Controls.ClientCallbackEventArgs) Handles customerRepeater.ClientCallback

        Dim beginRowIndex As Integer = 0
        If (Integer.TryParse(CType(e.Arguments("beginRowIndex"), String), beginRowIndex)) Then

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
            Me.goukeiLabel.Text = goukeiStr.ToString

            If (count > 0) Then
                Me.resultListPanel.Visible = True

                '顔写真の保存先フォルダ(Web向け)取得
                Dim imagePath As String = SC3080101BusinessLogic.GetImagePath()

                '顧客一覧取得
                Dim customerList As SC3080101DataSet.SC3080101CustDataTable = _
                    SC3080101BusinessLogic.GetCustomerList(serchTable)
                Dim customerRow As SC3080101DataSet.SC3080101CustRow

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

                        '＜セールススタッフリーダーまたはセールススタッフ かつ　タップした行の顧客が担当以外の顧客の場合＞
                        '処理を行わない
                        If (customerRow.STAFFCD.Trim().Equals(StaffContext.Current.Account().Trim()) = False) Then

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
                        sstkindnm = "未"
                    Else
                        sstkindnm = "自"
                    End If

                    '顧客タイプ
                    Dim custypenm As String = String.Empty
                    If (customerRow.CUSTYPE.Equals("0")) Then
                        custypenm = "法"
                    End If
                    If (customerRow.CUSTYPE.Equals("1")) Then
                        custypenm = "個"
                    End If

                    'ファイルパス
                    If (String.IsNullOrEmpty(customerRow.IMAGEFILE_S) = True) Then
                        imgpath = NO_IMAGE_ICON
                    Else
                        imgpath = imagePath & customerRow.IMAGEFILE_S
                    End If
                    imgpath = Me.ResolveClientUrl(imgpath)

                    '偶数／奇数行判定
                    Dim flg As Integer
                    flg = i Mod 2

                    'シーケンシャル番号
                    Dim seqno As Long = 0
                    If (customerRow.IsSEQNONull() = True) Then
                        seqno = 0
                    Else
                        seqno = customerRow.SEQNO
                    End If

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
                                      """CUSTYPE"" : ""{13}""," & _
                                      """CSTKIND"" : ""{14}""," & _
                                      """CRCUSTID"" : ""{15}""," & _
                                      """STAFFCD"" : ""{16}""," & _
                                      """SEQNO"" : {17}," & _
                                      """maxrow"" : {18} ," & _
                                      """updateFlg"" : {19} ," & _
                                      """flg"" : {20} }}", _
                                      (i + 1), _
                                      HttpUtility.JavaScriptStringEncode(imgpath), _
                                      HttpUtility.JavaScriptStringEncode(customerRow.NAMETITLE), _
                                      HttpUtility.JavaScriptStringEncode(SpaceToHeifun(name)), _
                                      HttpUtility.JavaScriptStringEncode(SpaceToHeifun(telno)), _
                                      HttpUtility.JavaScriptStringEncode(SpaceToHeifun(mobile)), _
                                      HttpUtility.JavaScriptStringEncode(SpaceToHeifun(customerRow.SERIESNM.Trim)), _
                                      HttpUtility.JavaScriptStringEncode(SpaceToHeifun(customerRow.VCLREGNO.Trim)), _
                                      HttpUtility.JavaScriptStringEncode(SpaceToHeifun(customerRow.VIN.Trim)), _
                                      HttpUtility.JavaScriptStringEncode(SpaceToHeifun(customerRow.SSUSERNAME.Trim)), _
                                      HttpUtility.JavaScriptStringEncode(SpaceToHeifun(customerRow.SAUSERNAME.Trim)), _
                                      HttpUtility.JavaScriptStringEncode(customerRow.STAFFCD.Trim), _
                                      HttpUtility.JavaScriptStringEncode(sstkindnm), _
                                      HttpUtility.JavaScriptStringEncode(custypenm), _
                                      HttpUtility.JavaScriptStringEncode(customerRow.CSTKIND.Trim), _
                                      HttpUtility.JavaScriptStringEncode(customerRow.CRCUSTID.Trim), _
                                      HttpUtility.JavaScriptStringEncode(customerRow.STAFFCD.Trim), _
                                      seqno, _
                                      customerList.Rows.Count, _
                                      updateFlg, _
                                      flg)
                Next

                e.Results("@rows") = "[" & rows.ToString() & "]"

            End If

        Else
            e.Results("@rows") = "[]"
        End If

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
        '検索方向 (1:前方一致、2:あいまい検索)
        Dim serchDirection As Integer = 0
        If (Me.ContainsKey(ScreenPos.Current, SESSION_KEY_SERCHDIRECTION) = True) Then
            serchDirection = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_SERCHDIRECTION, False), Integer)
        Else
            serchDirection = SC3080101DataTableTableAdapter.IdSerchdirectionAfter
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
        If (serchType = SC3080101DataTableTableAdapter.IdSerchTel) Then
            '電話番号時は、ハイフンを取り除く
            SerchRow.SERCHSTRING = serchString.Replace("-", "")             '検索文字列
        Else
            '電話番号以外（名称・VIN・車両登録No）で検索時
            SerchRow.SERCHSTRING = serchString.ToUpper                      '検索文字列 (全角に変換する)
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

        ''TCSとの連携ボタン
        'Dim tcvButton As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.TCV)
        'AddHandler tcvButton.Click, AddressOf tcvButton_Click

    End Sub

    ''' <summary>
    ''' TCSとの連携ボタン
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub tcvButton_Click(sender As Object, e As CommonMasterFooterButtonClickEventArgs)

        Dim context As StaffContext = StaffContext.Current

        'TCV機能に渡す引数を設定
        e.Parameters.Add("DataSource", "none")
        e.Parameters.Add("MenuLockFlag", CType(False, String))
        e.Parameters.Add("CloseCallback", "closeCallbackFunction")
        e.Parameters.Add("StatusCallback", "statusCallbackFunction")
        e.Parameters.Add("Account", context.Account)
        e.Parameters.Add("NewActFlag", CType(False, String))
        e.Parameters.Add("AccountStrCd", context.BrnCD)
        e.Parameters.Add("DlrCd", context.DlrCD)

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

End Class
