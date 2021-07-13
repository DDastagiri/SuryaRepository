'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3070101.ascx.vb
'──────────────────────────────────
'機能： 在庫状況
'補足： 
'作成： 2012/02/20 KN m.asano
'更新： 2013/02/08 TMEJ m.asano FTMS対応 $01
'更新： 2013/12/17 TMEJ t.shimamura 在庫車両連携IF開発 $02
'更新： 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証
'──────────────────────────────────
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.Estimate.Ims.BizLogic
Imports Toyota.eCRB.Estimate.Ims.DataAccess
Imports Toyota.eCRB.Estimate.Ims.DataAccess.SC3070101SearchConditionDataSet
Imports Toyota.eCRB.Estimate.Ims.DataAccess.SC3070101SearchResultDataSet
Imports System.Globalization
Imports System.Data
Imports Toyota.eCRB.iCROP.BizLogic.SC3070201
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess


''' <summary>
''' SC3070101(在庫状況)
''' Webページのプレゼンテーション層
''' </summary>
''' <remarks></remarks>
Partial Class Pages_SC3070101
    Inherits System.Web.UI.UserControl

#Region "定数"

#Region "画面固有"

    ''' <summary>
    ''' 画面ID
    ''' </summary>
    Private Const ApplicationId As String = "SC3070101"

    ''' <summary>
    ''' 画面間パラメータ名：車名
    ''' </summary>
    Private Const SearchKeyCarName As String = "SearchKey.CAR_NAME"

    ''' <summary>
    ''' 画面間パラメータ名：グレード
    ''' </summary>
    Private Const SeatchKeyGrade As String = "SearchKey.GRADE"

    ''' <summary>
    ''' 画面間パラメータ名：SFX
    ''' </summary>
    Private Const SearchKeySfx As String = "SearchKey.SFX"

    ''' <summary>
    ''' 画面間パラメータ名：外装色
    ''' </summary>
    Private Const SearchKeyExteriorColor As String = "SearchKey.COLOR_NAME"

    ''' <summary>
    ''' 画面間パラメータ名：承認モードフラグ
    ''' </summary>
    Private Const SearchKeyApprovalModeFlag As String = "EstimateMode.Approval"

    ''' <summary>
    ''' 画面間パラメータ名：価格相談モードフラグ
    ''' </summary>
    Private Const SearchKeyPriceApprovalModeFlag As String = "EstimateMode.PriceApproval"

    ''' <summary>
    ''' 在庫リスト奇数行背景色
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StockListOddRowBackColor As String = "-webkit-gradient(linear, left top, left bottom, from(white), color-stop(0.51, #FEFEFE), to(#E2E2E2))"

    ''' <summary>
    ''' 在庫リスト奇数行文字色
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StockListOddRowFontColor As String = "#AAA"

    ' $01 start FTMS対応
    ''' <summary>
    ''' 日付デフォルト値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DateDefaultValue As String = "01/01/1900"

    ''' <summary>
    ''' 日付デフォルト値(年)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DateDefaultValueYear As Integer = 1900
    ' $01 end  FTMS対応

    '$02 start システム環境値

    ''' <summary>
    ''' 在庫画面の表示区分
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StockDispKbn As String = "STOCK_DISP_KBN"

    ''' <summary>
    ''' 注文車両の鮮度判定用日数1
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OrderVclJudgementDay1st As String = "ORDERVCLJUDGMENTDAY_1ST"

    ''' <summary>
    ''' 注文車両の鮮度判定用日数2
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OrderVclJudgementDay2nd As String = "ORDERVCLJUDGMENTDAY_2ND"

    ''' <summary>
    ''' 在庫車両の鮮度判定用日数1
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StockVclJudgementDay1st As String = "STOCKVCLJUDGMENTDAY_1ST"

    ''' <summary>
    ''' 在庫車両の鮮度判定用日数2
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StockVclJudgementDay2nd As String = "STOCKVCLJUDGMENTDAY_2ND"

    '$02 end システム環境値


#End Region

#Region "メッセージID"

    ''' <summary>
    ''' メッセージID:成功
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MessageIdSuccess As String = "0"

    ''' <summary>
    ''' メッセージID:引数エラー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MessageIdCarNameError As String = "2001"

    ''' <summary>
    ''' メッセージID:WebServiceエラー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MessageIdWebServiceError As String = "9999"

#End Region

#Region "文言ID"

    ''' <summary>
    '''文言ID：在庫状況
    ''' </summary>
    Private Const WordIdZaikoJokyo As Integer = 1
    ''' <summary>
    '''文言ID：モデル
    ''' </summary>
    Private Const WordIdModel As Integer = 2
    ''' <summary>
    '''文言ID：グレード
    ''' </summary>
    Private Const WordIdGrade As Integer = 3
    ''' <summary>
    '''文言ID：SFX
    ''' </summary>
    Private Const WordIdSuffix As Integer = 4
    ''' <summary>
    '''文言ID：カラー
    ''' </summary>
    Private Const WordIdColor As Integer = 5
    ''' <summary>
    '''文言ID：注文
    ''' </summary>
    Private Const WordIdOrder As Integer = 6
    ''' <summary>
    '''文言ID：店頭在庫
    ''' </summary>
    Private Const WordIdStock As Integer = 7
    ''' <summary>
    '''文言ID：キャンセル
    ''' </summary>
    Private Const WordIdCancel As Integer = 8
    ''' <summary>
    '''文言ID：決定
    ''' </summary>
    Private Const WordIdDecision As Integer = 9
    ''' <summary>
    '''文言ID：グレード選択
    ''' </summary>
    Private Const WordIdGradeSelect As Integer = 10
    ''' <summary>
    '''文言ID：SFX選択
    ''' </summary>
    Private Const WordIdSuffixSelect As Integer = 11
    ''' <summary>
    '''文言ID：外装色選択
    ''' </summary>
    Private Const WordIdColorSelect As Integer = 12
    ''' <summary>
    '''文言ID：未選択
    ''' </summary>
    Private Const WordIdNoSelect As Integer = 13

#End Region

#Region "文字切り桁数"

    ''' <summary>
    '''文字切り：タイトル
    ''' </summary>
    Private Const WordMaxLengthTitle As Integer = 9
    ''' <summary>
    '''文字切り：検索条件：タイトル
    ''' </summary>
    Private Const WordMaxLengthSearchTitle As Integer = 13
    ''' <summary>
    '''文字切り：在庫リストヘッダー：グレード
    ''' </summary>
    Private Const WordMaxLengthStockListHeadGrade As Integer = 14
    ''' <summary>
    '''文字切り：在庫リストヘッダー：SFX
    ''' </summary>
    Private Const WordMaxLengthStockListHeadSuffix As Integer = 8
    '01 start 外装色列追加
    ''' <summary>
    '''文字切り：在庫リストヘッダー：外装色
    ''' </summary>
    Private Const WordMaxLengthStockListHeadColor As Integer = 6
    '01 end 外装色列追加
    ''' <summary>
    '''文字切り：在庫リストヘッダー：注文
    ''' </summary>
    Private Const WordMaxLengthStockListHeadOrder As Integer = 13
    ''' <summary>
    '''文字切り：在庫リストヘッダー：店頭在庫
    ''' </summary>
    Private Const WordMaxLengthStockListHeadStock As Integer = 25
    ''' <summary>
    '''文字切り：検索リスト：ボタン
    ''' </summary>
    Private Const WordMaxLengthSearchListButton As Integer = 6
    ''' <summary>
    '''文字切り：検索リスト：タイトル
    ''' </summary>
    Private Const WordMaxLengthSearchListTitle As Integer = 7

#End Region

#End Region

#Region "プロパティ"
    ''' <summary>
    ''' 希望車：車名
    ''' </summary>
    Private SeletedCarNameValue As String

    ''' <summary>
    ''' 希望車：グレード
    ''' </summary>
    Private SeletedCarGradeValue As String

    ''' <summary>
    ''' 希望車：SFX
    ''' </summary>
    Private SeletedCarSfxValue As String

    ''' <summary>
    ''' 希望車：外装色
    ''' </summary>
    Private SeletedCarExteriorValue As String

    ''' <summary>
    ''' 文言未選択
    ''' </summary>
    Private WordNoSelect As String

    ''' <summary>
    ''' 承認モード
    ''' </summary>
    Private CurrentApprovalModeFlagValue As String

    ''' <summary>
    ''' 価格相談モード
    ''' </summary>
    Private CurrentPriceApprovalMode As String

    ' $01 start 在庫状況取得引数追加
    ''' <summary>
    ''' 注文車両鮮度判定日数1
    ''' </summary>
    Private orderVclFreshThreshold1st As String

    ''' <summary>
    ''' 注文車両鮮度判定日数2
    ''' </summary>
    Private orderVclFreshThreshold2nd As String

    ''' <summary>
    ''' 在庫車両鮮度判定日数1
    ''' </summary>
    Private stockVclFreshThreshold1st As String

    ''' <summary>
    ''' 在庫車両鮮度判定日数2
    ''' </summary>
    Private stockVclFreshThreshold2nd As String
    ' $01 end 在庫状況取得引数追加

#End Region

#Region "イベント"

#Region "ページロード"

    ''' <summary>
    ''' 在庫状況画面ページロード
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub SC3070101_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Logger.Info("Page_Load_Start")

        'PostBack時、初期表示処理は行わない。
        If IsPostBack = True Then

            Logger.Info("Page_Load_001 IsPostBack")
            Return
        End If

        'セッション情報取得
        GetSessionValue()

        '初期表示
        PageInit()

        ' 以下の場合に初期表示時に在庫車両リストエリアを表示する。
        ' 契約承認依頼の回答時
        ' 価格相談の回答時
        Dim zaikoAriaName As String = "zaiko_GL1"
        If Me.DisplayClassValue.Value.Equals("1") Then
            zaikoAriaName = "zaiko_GL2"
        End If

        If CurrentApprovalModeFlagValue.Equals("1") Or CurrentPriceApprovalMode.Equals("1") Then
            CType(Me.FindControl(zaikoAriaName), HtmlGenericControl).Attributes("style") = "display:block;"
        Else
            CType(Me.FindControl(zaikoAriaName), HtmlGenericControl).Attributes("style") = "display:none;"
        End If

        Logger.Info("Page_Load_End")
    End Sub

#End Region

#Region "非同期メソッド"

    ''' <summary>
    ''' 初期検索処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub LoadButton_Click(sender As Object, e As System.EventArgs) Handles LoadButton.Click

        Logger.Info("LoadButtonClick_Strat")

        ' 更新タイプを初期検索にする
        Me.UpDateTypeValue.Value = "0"

        ' セッション情報取得
        GetSessionValue()

        ' システム環境値取得
        Dim sysEnvSet As New SystemEnvSetting
        Dim sysEnvSetTitlePosRow As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow = Nothing
        ' 表示区分
        Me.DisplayClassValue.Value = sysEnvSet.GetSystemEnvSetting(StockDispKbn).PARAMVALUE

        GetJudgementDay(Me.DisplayClassValue.Value)

        try
            Dim sc3070101Biz As SC3070101BusinessLogic = New SC3070101BusinessLogic

            ' 車名コード及びグレードが設定されている場合のみ検索を行う。
            ' 初期検索非同期対応
            If (Not String.IsNullOrEmpty(SeletedCarNameValue) AndAlso _
                Not String.IsNullOrEmpty(SeletedCarGradeValue)) Then

                '車両在庫状況確認(TACT連携)
                ViewState("ResultDataTable") = _
                    sc3070101Biz.GetStockStatus(GetSeriesCode(), SeletedCarGradeValue, SeletedCarSfxValue, SeletedCarExteriorValue, Me.DisplayClassValue.Value, _
                                                orderVclFreshThreshold1st, orderVclFreshThreshold2nd, stockVclFreshThreshold1st, stockVclFreshThreshold2nd)

                '在庫リスト表示処理
                If Me.DisplayClassValue.Value.Equals("0") Then
                    StockListUpDate(SeletedCarGradeValue, SeletedCarSfxValue, String.Empty)
                Else
                    StockListUpDateGL2(SeletedCarGradeValue, SeletedCarSfxValue, String.Empty)
                End If

            End If
        Catch ex As ApplicationException
            Me.ShowMessageBox(CInt(MessageIdWebServiceError))
        End Try
        Logger.Info("LoadButtonClick_End")
    End Sub

    ''' <summary>
    ''' 在庫リストを更新します。(SFX・カラー変更時)
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub ListUpdateButtonClick(sender As Object, e As System.EventArgs) Handles ListUpdateButton.Click

        Logger.Info("ListUpdateButton_Strat")

        ' 更新タイプをリスト選択にする
        Me.UpDateTypeValue.Value = "1"
        Me.LoadingFlg.Value = "1"

        WordNoSelect = Server.HtmlEncode(WebWordUtility.GetWord(ApplicationId, WordIdNoSelect))

        ' 検索条件取得
        '"未選択"だった場合は空文字とする。
        Dim gradeCodeValue As String = IIf(String.Equals(Me.GradeSearchValue.Value, WordNoSelect), String.Empty, Me.GradeCodeSearchValue.Value)
        Dim gradeValue As String = IIf(String.Equals(Me.GradeSearchValue.Value, WordNoSelect), String.Empty, Me.GradeSearchValue.Value)
        Dim suffixValue As String = IIf(String.Equals(Me.SuffixSearchValue.Value, WordNoSelect), String.Empty, Me.SuffixSearchValue.Value)
        Dim colorCodeValue As String = IIf(String.Equals(Me.ColorSearchValue.Value, WordNoSelect), String.Empty, Me.ColorCodeSearchValue.Value)
        Dim colorValue As String = IIf(String.Equals(Me.ColorSearchValue.Value, WordNoSelect), String.Empty, Me.ColorSearchValue.Value)

        'グレードが未選択の場合は以降の処理は行わない。
        If String.IsNullOrEmpty(gradeValue) Then
            Logger.Info("ListUpdateButton_001 Grade Not Select")
            Me.LoadingFlg.Value = "0"
            Return
        End If

        ' セッション情報取得
        GetSessionValue()

        'SFXが変更された場合
        If (String.Equals(Me.SuffixSearchValueChange.Value, "1")) Then

            If Me.DisplayClassValue.Value.Equals("0") Then
                ' リストの更新(グレード、SFX)
                StockListUpDate(gradeCodeValue, suffixValue, String.Empty)
            Else
                StockListUpDateGL2(gradeCodeValue, suffixValue, String.Empty)
            End If

            If (String.Equals(Me.SuffixSearchValue.Value, WordNoSelect)) Then

                'SFXが未選択へ変更された場合,グレードにてカラーリストを取得
                SetExteriorList(gradeCodeValue, String.Empty)

            Else

                'SFXが未選択以外へ変更された場合、グレード、SFXにてカラーリストを取得
                SetExteriorList(gradeCodeValue, suffixValue)
            End If

            Me.SuffixSearchValueChange.Value = "0"
            colorCodeValue = String.Empty
            colorValue = String.Empty

        Else

            ' カラーが変更された場合
            ' リストの更新(グレード、SFX、カラー)
            If Me.DisplayClassValue.Value.Equals("0") Then
                StockListUpDate(gradeCodeValue, suffixValue, colorCodeValue)
            Else
                StockListUpDateGL2(gradeCodeValue, suffixValue, colorCodeValue)
            End If

        End If

        ' 表示を戻す
        If String.IsNullOrEmpty(gradeValue) Then
            Me.Lable_GradeSearch.Text = Server.HtmlEncode(WordNoSelect)
            Me.GradeSearchValue.Value = Server.HtmlEncode(WordNoSelect)
            Me.GradeCodeSearchValue.Value = Server.HtmlEncode(WordNoSelect)
        Else

            If String.IsNullOrEmpty(suffixValue) Then
                Me.Lable_GradeSearch.Text = Server.HtmlEncode(gradeValue)
                Me.GradeSearchValue.Value = Server.HtmlEncode(gradeValue)
                Me.GradeCodeSearchValue.Value = Server.HtmlEncode(gradeCodeValue)
            Else
                ' グレードリストを取得
                Dim gradeDataTable As SC3070101SearchConditionDataSet.GradeConditionDataTableDataTable = _
                        CType(ViewState("GradeDataTable"), SC3070101SearchConditionDataSet.GradeConditionDataTableDataTable)
                Me.Lable_GradeSearch.Text = Server.HtmlEncode(GetGradeName(gradeCodeValue, suffixValue, gradeDataTable))
                Me.GradeSearchValue.Value = Me.Lable_GradeSearch.Text
                Me.GradeCodeSearchValue.Value = Server.HtmlEncode(gradeCodeValue)
            End If
        End If

        If Me.DisplayClassValue.Value.Equals("0") Then
            Me.Lable_SuffixSearch.Text = Server.HtmlEncode(IIf(String.IsNullOrEmpty(suffixValue), WordNoSelect, suffixValue))
            Me.Lable_ColorSearch.Text = Server.HtmlEncode(IIf(String.IsNullOrEmpty(colorValue), WordNoSelect, colorValue))
        Else
            Me.Lable_SuffixSearchGL2.Text = Server.HtmlEncode(IIf(String.IsNullOrEmpty(suffixValue), WordNoSelect, suffixValue))
            Me.Lable_ColorSearchGL2.Text = Server.HtmlEncode(IIf(String.IsNullOrEmpty(colorValue), WordNoSelect, colorValue))
        End If

        Me.SuffixSearchValue.Value = Server.HtmlEncode(IIf(String.IsNullOrEmpty(suffixValue), WordNoSelect, suffixValue))
        Me.ColorSearchValue.Value = Server.HtmlEncode(IIf(String.IsNullOrEmpty(colorValue), WordNoSelect, colorValue))
        Me.ColorCodeSearchValue.Value = Server.HtmlEncode(IIf(String.IsNullOrEmpty(colorValue), String.Empty, colorCodeValue))

        Me.LoadingFlg.Value = "0"

        ' リストの更新
        UpdateAreaStock.Update()

        Logger.Info("ListUpdateButton_End")
    End Sub

    ''' <summary>
    ''' 在庫リストを更新します。(グレード変更時)
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub ListUpdateGradeButtonClick(sender As Object, e As System.EventArgs) Handles ListUpdateGradeButton.Click

        Logger.Info("ListUpdateGradeButtonClick_Strat")

        ' 更新タイプをリスト選択にする
        Me.UpDateTypeValue.Value = "1"
        Me.LoadingFlg.Value = "1"

        WordNoSelect = Server.HtmlEncode(WebWordUtility.GetWord(ApplicationId, WordIdNoSelect))

        ' 検索条件取得
        ' グレードが変更された場合は、SFX・カラーは未選択とする。
        Dim gradeCodeValue As String = IIf(String.Equals(Me.GradeSearchValue.Value, WordNoSelect), String.Empty, Me.GradeCodeSearchValue.Value)
        Dim gradeSuffixValue As String = IIf(String.Equals(Me.GradeSearchValue.Value, WordNoSelect), String.Empty, Me.GradeSuffixSearchValue.Value)
        Dim gradeValue As String = IIf(String.Equals(Me.GradeSearchValue.Value, WordNoSelect), String.Empty, Me.GradeSearchValue.Value)

        ' セッション情報取得
        GetSessionValue()

        ' 設定値取得
        GetJudgementDay(Me.DisplayClassValue.Value)

        Try
            ' 車両在庫状況確認(TACT連携)
            Dim sc3070101Biz As SC3070101BusinessLogic = New SC3070101BusinessLogic
            ViewState("ResultDataTable") = _
                sc3070101Biz.GetStockStatus(GetSeriesCode(), gradeCodeValue, gradeSuffixValue, String.Empty, Me.DisplayClassValue.Value, _
                                            orderVclFreshThreshold1st, orderVclFreshThreshold2nd, stockVclFreshThreshold1st, stockVclFreshThreshold2nd)
            ' リストの更新
            If Me.DisplayClassValue.Value.Equals("0") Then

                StockListUpDate(gradeCodeValue, String.Empty, String.Empty)
            Else
                StockListUpDateGL2(gradeCodeValue, String.Empty, String.Empty)

            End If

        Catch ex As ApplicationException

            If String.Equals(ex.Message, MessageIdCarNameError) Then

                Me.ShowMessageBox(CInt(MessageIdCarNameError))

            ElseIf String.Equals(ex.Message, MessageIdWebServiceError) Then

                ' DMS連携に失敗した場合は、画面リストをクリアしておく。
                Me.RepStockListBox.DataSource = Nothing
                Me.RepStockListBox.DataBind()
                Me.RepStockListBoxGL2.DataSource = Nothing
                Me.RepStockListBoxGL2.DataBind()
                Me.ShowMessageBox(CInt(MessageIdWebServiceError))

            End If

        Finally
            ' 未選択文言設定
            If Me.DisplayClassValue.Value.Equals("0") Then
                Me.Lable_GradeSearch.Text = Server.HtmlEncode(IIf(String.IsNullOrEmpty(gradeValue), WordNoSelect, gradeValue))
                Me.Lable_SuffixSearch.Text = Server.HtmlEncode(WordNoSelect)
                Me.Lable_ColorSearch.Text = Server.HtmlEncode(WordNoSelect)
            Else
                Me.Lable_GradeSearchGL2.Text = Server.HtmlEncode(IIf(String.IsNullOrEmpty(gradeValue), WordNoSelect, gradeValue))
                Me.Lable_SuffixSearchGL2.Text = Server.HtmlEncode(WordNoSelect)
                Me.Lable_ColorSearchGL2.Text = Server.HtmlEncode(WordNoSelect)
            End If

            ' 表示を戻す
            Me.GradeCodeSearchValue.Value = Server.HtmlEncode(IIf(String.IsNullOrEmpty(gradeValue), WordNoSelect, gradeCodeValue))
            Me.SuffixSearchValue.Value = Server.HtmlEncode(WordNoSelect)
            Me.ColorSearchValue.Value = Server.HtmlEncode(WordNoSelect)
            Me.ColorCodeSearchValue.Value = String.Empty

            ' SFX及びカラーリストを再設定
            'サフィックス検索条件取得
            SetSuffixList(gradeCodeValue)

            '外装色検索条件取得
            SetExteriorList(gradeCodeValue, String.Empty)

            Me.LoadingFlg.Value = "0"

            ' リストの更新
            UpdateAreaStock.Update()
        End Try

        Logger.Info("ListUpdateGradeButtonClick_End")
    End Sub

#End Region

#Region "非同期通信時のエラー処理"
    ''' <summary>
    ''' 非同期通信時のエラー処理を行う
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証
    ''' </history>
    Protected Sub SendErrorMessageButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SendErrorMessageButton.Click
        Logger.Error("SendErrorMessageButton_Click Param[" & sender.ToString & "," & e.ToString & "]")

        Logger.Error("SendErrorMessageButton_Click SendError[" & ErrorMessage.Value & "]")

        Logger.Error("SendErrorMessageButton_Click Ret[]")
    End Sub
#End Region

#End Region

#Region "プライベートメソッド"

#Region "初期表示"

    ''' <summary>
    ''' 初期表示処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub PageInit()

        Logger.Info("PageInit_Strat")

        ' $02 start システム環境値の取得
        ' システム環境値取得
        Dim sysEnvSet As New SystemEnvSetting
        Dim sysEnvSetTitlePosRow As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow = Nothing
        ' 表示区分
        Me.DisplayClassValue.Value = sysEnvSet.GetSystemEnvSetting(StockDispKbn).PARAMVALUE

        GetJudgementDay(Me.DisplayClassValue.Value)
        ' $02 end システム環境値の取得

        Logger.Info("PageInit_000 orderVclFreshThreshold1st = " & orderVclFreshThreshold1st & ", orderVclFreshThreshold2nd = " & orderVclFreshThreshold2nd _
                    & ", stockVclFreshThreshold1st = " & stockVclFreshThreshold1st & ", stockVclFreshThreshold2nd = " & stockVclFreshThreshold2nd)

        '文言設定
        SetWord(Me.DisplayClassValue.Value)

        '検索条件（プルダウン）の取得
        Dim sc3070101Biz As SC3070101BusinessLogic = New SC3070101BusinessLogic

        Try
            ' $02 start GL版により表示内容を選択
            If Me.DisplayClassValue.Value.Equals("0") Then

                zaiko_GL2.Visible = False
                zaiko_GL1.Visible = True

                'グレード検索条件取得
                Dim gradeTable As GradeConditionDataTableDataTable = _
                    sc3070101Biz.GetGradeConditionList(SeletedCarNameValue)
                Me.RepGradeListBox.DataSource = gradeTable
                Me.RepGradeListBox.DataBind()
                ViewState("GradeDataTable") = gradeTable
                Me.GradeSerchNumber.Value = CStr(gradeTable.Count)

                'サフィックス検索条件取得
                SetSuffixList(SeletedCarGradeValue)

                '外装色検索条件取得
                SetExteriorList(SeletedCarGradeValue, IIf(String.IsNullOrEmpty(SeletedCarSfxValue), String.Empty, SeletedCarSfxValue))

                ' 検索条件設定
                ' 車名コード
                Me.Lable_ModelSearch.Text = Server.HtmlEncode(SeletedCarNameValue)
                ' グレード
                If String.IsNullOrEmpty(SeletedCarGradeValue) Then

                    Logger.Info("PageInit_001 Grade Is Empty")

                    ' セッションからの取得値が未設定の場合は、"未選択"を表示
                    Me.Lable_GradeSearch.Text = WordNoSelect
                Else

                    Logger.Info("PageInit_002 Grade Is Not Empty")

                    ' セッションからの取得値が設定されている場合は、該当するグレード名を表示
                    Dim dispGradeName As String = GetGradeName(SeletedCarGradeValue, SeletedCarSfxValue, gradeTable)
                    Me.Lable_GradeSearch.Text = dispGradeName
                    Me.GradeCodeSearchValue.Value = SeletedCarGradeValue
                End If

                ' SFX
                Me.Lable_SuffixSearch.Text = _
                    Server.HtmlEncode(IIf(String.IsNullOrEmpty(SeletedCarSfxValue), WordNoSelect, SeletedCarSfxValue))
                Me.SuffixSearchValue.Value = _
                    Server.HtmlEncode(IIf(String.IsNullOrEmpty(SeletedCarSfxValue), WordNoSelect, SeletedCarSfxValue))
                ' 外装色
                Me.Lable_ColorSearch.Text = WordNoSelect
                Me.ColorSearchValue.Value = WordNoSelect
                Me.ColorCodeSearchValue.Value = String.Empty

                ' GL2版
            Else

                zaiko_GL2.Visible = True
                zaiko_GL1.Visible = False

                'グレード検索条件取得
                Dim gradeTable As GradeConditionDataTableDataTable = _
                    sc3070101Biz.GetGradeConditionList(SeletedCarNameValue)
                Me.RepGradeListBoxGL2.DataSource = gradeTable
                Me.RepGradeListBoxGL2.DataBind()
                ViewState("GradeDataTable") = gradeTable
                Me.GradeSerchNumber.Value = CStr(gradeTable.Count)
                'サフィックス検索条件取得
                SetSuffixList(SeletedCarGradeValue)

                '外装色検索条件取得
                SetExteriorList(SeletedCarGradeValue, IIf(String.IsNullOrEmpty(SeletedCarSfxValue), String.Empty, SeletedCarSfxValue))

                ' 検索条件設定
                ' 車名コード
                Me.Lable_ModelSearchGL2.Text = Server.HtmlEncode(SeletedCarNameValue)
                ' グレード
                If String.IsNullOrEmpty(SeletedCarGradeValue) Then

                    Logger.Info("PageInit_001 Grade Is Empty")

                    ' セッションからの取得値が未設定の場合は、"未選択"を表示
                    Me.Lable_GradeSearchGL2.Text = WordNoSelect
                Else

                    Logger.Info("PageInit_002 Grade Is Not Empty")

                    ' セッションからの取得値が設定されている場合は、該当するグレード名を表示
                    Dim dispGradeName As String = GetGradeName(SeletedCarGradeValue, SeletedCarSfxValue, gradeTable)
                    Me.Lable_GradeSearchGL2.Text = dispGradeName
                    Me.GradeCodeSearchValue.Value = SeletedCarGradeValue
                End If

                ' SFX
                Me.Lable_SuffixSearchGL2.Text = _
                    Server.HtmlEncode(IIf(String.IsNullOrEmpty(SeletedCarSfxValue), WordNoSelect, SeletedCarSfxValue))
                Me.SuffixSearchValue.Value = _
                    Server.HtmlEncode(IIf(String.IsNullOrEmpty(SeletedCarSfxValue), WordNoSelect, SeletedCarSfxValue))
                ' 外装色
                Me.Lable_ColorSearchGL2.Text = WordNoSelect
                Me.ColorSearchValue.Value = WordNoSelect
                Me.ColorCodeSearchValue.Value = String.Empty

            End If
            ' $02 end GL版により表示内容を選択

            ScriptManager.RegisterStartupScript(Me, _
                        Me.GetType, _
                        "PageLoad", _
                        "setTimeout('LoadingScreen();', 300);", _
                        True)

        Catch ex As ApplicationException

            If String.Equals(ex.Message, MessageIdCarNameError) Then

                Me.ShowMessageBox(CInt(MessageIdCarNameError))

            ElseIf String.Equals(ex.Message, MessageIdWebServiceError) Then

                Me.ShowMessageBox(CInt(MessageIdWebServiceError))

            End If

        End Try

        Logger.Info("PageInit_End")

    End Sub

#End Region

#Region "在庫リスト表示処理"

    ''' <summary>
    ''' 在庫リスト表示処理
    ''' </summary>
    ''' <param name="Model">MODEL</param>
    ''' <param name="Suffix">SFX</param>
    ''' <param name="ExteriorColor">外装色</param>
    ''' <remarks></remarks>
    Private Sub StockListUpDate(ByVal model As String,
                    ByVal suffix As String, ByVal exteriorColor As String)

        Logger.Info("StockListUpDatet_Strat Rram[" & model & suffix & exteriorColor & "]")

        Dim resultDataTable As SC3070101SearchResultDataSet.ResultDataTableDataTable = _
            CType(ViewState("ResultDataTable"), SC3070101SearchResultDataSet.ResultDataTableDataTable)

        ' データテーブルが存在しない場合は、以降の処理は行わない。
        If resultDataTable Is Nothing Then

            Logger.Info("StockListUpDatet_End StockInfo Nothing")
            Return
        End If

        ' 検索条件作成
        Dim wherePram As New StringBuilder
        If (Not String.IsNullOrEmpty(model)) Then
            Logger.Info("StockListUpDatet_001 Add SearchParm Model")
            wherePram.Append("Model='" & model & "' ")
        End If
        If (Not String.IsNullOrEmpty(suffix)) Then
            Logger.Info("StockListUpDatet_002 Add SearchParm Suffix")
            wherePram.Append("and Suffix='" & suffix & "' ")
        End If
        If (Not String.IsNullOrEmpty(exteriorColor)) Then
            Logger.Info("StockListUpDatet_003 Add SearchParm Color")
            wherePram.Append("and Color='" & exteriorColor & "'")
        End If

        Dim parm As String = wherePram.ToString()
        If (parm.StartsWith("and", StringComparison.CurrentCulture)) Then
            parm = parm.Substring(3)
        End If

        Logger.Info("StockListUpDatet_004 SearchParm=[" & parm & "]")

        ' データテーブルより検索条件に一致したデータを取得
        Dim selectRow() As DataRow = resultDataTable.Select(parm)
        wherePram = Nothing

        ' データバインド
        Me.RepStockListBox.DataSource = CreateStockListDataSet(selectRow)
        Me.RepStockListBox.DataBind()

        ' 表示調整
        Dim repStockListIndex As Integer
        Dim staffInfo As StaffContext = StaffContext.Current
        Dim nowDate As Date = DateTimeFunc.Now(staffInfo.DlrCD)
        Dim baseDate As New Date(nowDate.Year, nowDate.Month, nowDate.Day)
        Dim gradeName As String = String.Empty
        Dim suffixName As String = String.Empty
        Dim suffixCount As Integer = 0
        Dim firstGradeControl As HtmlTableCell = Nothing
        Dim checkGradeName As String = String.Empty

        ' グレードリストを取得
        Dim gradeDataTable As SC3070101SearchConditionDataSet.GradeConditionDataTableDataTable = _
            CType(ViewState("GradeDataTable"), SC3070101SearchConditionDataSet.GradeConditionDataTableDataTable)

        For repStockListIndex = 0 To Me.RepStockListBox.Items.Count - 1
            'Sfxを取得
            suffixName = CType(Me.RepStockListBox.Items(repStockListIndex).FindControl("Lable_SfxContent"), Label).Text
            checkGradeName = CType(Me.RepStockListBox.Items(repStockListIndex).FindControl("Lable_GradeContent"), Label).Text

            CType(Me.RepStockListBox.Items(repStockListIndex).FindControl("Lable_GradeContent"), Label).Text = _
                GetGradeName(CType(Me.RepStockListBox.Items(repStockListIndex).FindControl("Lable_GradeContent"), Label).Text, suffixName, gradeDataTable)

            'グレード名を取得
            If (Not String.Equals(gradeName, CType(Me.RepStockListBox.Items(repStockListIndex).FindControl("Lable_GradeContent"), Label).Text)) Then

                ' １行目のグレードタグを追加
                gradeName = CType(Me.RepStockListBox.Items(repStockListIndex).FindControl("Lable_GradeContent"), Label).Text
                firstGradeControl = CType(Me.RepStockListBox.Items(repStockListIndex).FindControl("GradeContent"), HtmlTableCell)
                suffixCount = 0

            Else

                ' ２行目以降のグレード名は表示させない。
                CType(Me.RepStockListBox.Items(repStockListIndex).FindControl("GradeContent"), HtmlTableCell).Visible = False
            End If

            'グレード列を連結
            suffixCount = suffixCount + 1
            firstGradeControl.Attributes("rowspan") = suffixCount

            ''奇数行の場合、背景色を変更する。
            If (repStockListIndex Mod 2 <> 0) Then
                CType(Me.RepStockListBox.Items(repStockListIndex).FindControl("GradeContent"), HtmlTableCell).Style("background") = _
                    StockListOddRowBackColor
                CType(Me.RepStockListBox.Items(repStockListIndex).FindControl("GradeContent"), HtmlTableCell).Style("color") = _
                    StockListOddRowFontColor
                CType(Me.RepStockListBox.Items(repStockListIndex).FindControl("SfxContent"), HtmlTableCell).Style("background") = _
                    StockListOddRowBackColor
                CType(Me.RepStockListBox.Items(repStockListIndex).FindControl("SfxContent"), HtmlTableCell).Style("color") = _
                    StockListOddRowFontColor
                CType(Me.RepStockListBox.Items(repStockListIndex).FindControl("OrderContent"), HtmlTableCell).Style("background") = _
                    StockListOddRowBackColor
                CType(Me.RepStockListBox.Items(repStockListIndex).FindControl("OrderContent"), HtmlTableCell).Style("color") = _
                    StockListOddRowFontColor
                CType(Me.RepStockListBox.Items(repStockListIndex).FindControl("StockContent"), HtmlTableCell).Style("background") = _
                     StockListOddRowBackColor
                CType(Me.RepStockListBox.Items(repStockListIndex).FindControl("StockContent"), HtmlTableCell).Style("color") = _
                    StockListOddRowFontColor
            End If

            Dim repOrderIndex As Integer
            Dim repOrderContent As Repeater = Me.RepStockListBox.Items(repStockListIndex).FindControl("RepOrderContent")
            For repOrderIndex = 0 To repOrderContent.Items.Count - 1

                'チップの色は販売店入庫予定日を基準に以下を表示。
                ' 0～29日前は青色、30～59日前は橙色、60～日前は赤色
                Dim orderColorCode As String = CType(repOrderContent.Items(repOrderIndex).FindControl("TipValueColor"), Label).Text
                Dim orderDateValue As String = CType(repOrderContent.Items(repOrderIndex).FindControl("DateValue"), HiddenField).Value
                Dim newlyDeliveryDate As Date = Date.Parse(orderDateValue, CultureInfo.InvariantCulture())
                CType(repOrderContent.Items(repOrderIndex).FindControl("TipBlock"), HtmlGenericControl).Attributes("class") = _
                    GetCssName(newlyDeliveryDate, baseDate, IsVehicleTypeOfChoice(checkGradeName, suffixName, orderColorCode))

                ' $01 start FTMS対応
                ' 日付にデフォルト値(1900/01/01)が設定されている場合は、空にする。
                If Date.Parse(orderDateValue).Year = DateDefaultValueYear Then
                    CType(repOrderContent.Items(repOrderIndex).FindControl("TipValueDate"), Label).Text = String.Empty
                End If
                ' $01 end  FTMS対応
            Next

            Dim repStockIndex As Integer
            Dim repStockContent As Repeater = Me.RepStockListBox.Items(repStockListIndex).FindControl("RepStockContent")
            For repStockIndex = 0 To repStockContent.Items.Count - 1

                'チップの色は販売店入庫日を基準に以下を表示。
                ' 0～29日前は青色、30～59日前は橙色、60～日前は赤色
                Dim stockColorCode As String = CType(repStockContent.Items(repStockIndex).FindControl("TipValueColor"), Label).Text
                Dim stockDateValue As String = CType(repStockContent.Items(repStockIndex).FindControl("DateValue"), HiddenField).Value
                Dim acceptDate As Date = Date.Parse(stockDateValue, CultureInfo.InvariantCulture())
                CType(repStockContent.Items(repStockIndex).FindControl("TipBlock"), HtmlGenericControl).Attributes("class") = _
                    GetCssName(acceptDate, baseDate, IsVehicleTypeOfChoice(checkGradeName, suffixName, stockColorCode))
            Next

        Next

        Logger.Info("StockListUpDatet_End")
    End Sub

    '02 start GL2版メソッド
    ''' <summary>
    ''' 在庫リスト表示処理 GL2版
    ''' </summary>
    ''' <param name="Model">Grade</param>
    ''' <param name="Suffix">SFX</param>
    ''' <param name="ExteriorColor">外装色</param>
    ''' <remarks></remarks>
    Private Sub StockListUpDateGL2(ByVal model As String,
                    ByVal suffix As String, ByVal exteriorColor As String)

        Logger.Info("StockListUpDatet_Strat Rram[" & model & suffix & exteriorColor & "]")

        Dim resultDataTable As SC3070101SearchResultDataSet.ResultDataTableDataTable = _
            CType(ViewState("ResultDataTable"), SC3070101SearchResultDataSet.ResultDataTableDataTable)

        ' データテーブルが存在しない場合は、以降の処理は行わない。
        If resultDataTable Is Nothing Then

            Logger.Info("StockListUpDatet_End StockInfo Nothing")
            Return
        End If

        ' 検索条件作成
        Dim wherePram As New StringBuilder
        If (Not String.IsNullOrEmpty(model)) Then
            Logger.Info("StockListUpDatet_001 Add SearchParm Model")
            wherePram.Append("Model='" & model & "' ")
        End If
        If (Not String.IsNullOrEmpty(suffix)) Then
            Logger.Info("StockListUpDatet_002 Add SearchParm Suffix")
            wherePram.Append("and Suffix='" & suffix & "' ")
        End If
        If (Not String.IsNullOrEmpty(exteriorColor)) Then
            Logger.Info("StockListUpDatet_003 Add SearchParm Color")
            wherePram.Append("and Color='" & exteriorColor & "'")
        End If

        Dim parm As String = wherePram.ToString()
        If (parm.StartsWith("and", StringComparison.CurrentCulture)) Then
            parm = parm.Substring(3)
        End If

        Logger.Info("StockListUpDatet_004 SearchParm=[" & parm & "]")

        ' データテーブルより検索条件に一致したデータを取得
        Dim selectRow() As DataRow = resultDataTable.Select(parm)
        wherePram = Nothing

        ' データバインド
        ' Me.RepStockListBoxGL2.DataSource = CreateStockListDataSet(selectRow)
        Me.RepStockListBoxGL2.DataSource = selectRow
        Me.RepStockListBoxGL2.DataBind()

        ' 表示調整
        Dim repStockListIndex As Integer
        Dim staffInfo As StaffContext = StaffContext.Current
        Dim nowDate As Date = DateTimeFunc.Now(staffInfo.DlrCD)
        Dim baseDate As New Date(nowDate.Year, nowDate.Month, nowDate.Day)
        Dim gradeName As String = String.Empty
        Dim suffixName As String = String.Empty
        Dim suffixCount As Integer = 0
        Dim checkGradeName As String = String.Empty
        Dim colorCount As Integer = 0
        Dim preGradeName As String = String.Empty

        ' グレードリストを取得
        Dim gradeDataTable As SC3070101SearchConditionDataSet.GradeConditionDataTableDataTable = _
            CType(ViewState("GradeDataTable"), SC3070101SearchConditionDataSet.GradeConditionDataTableDataTable)

        
        For repStockListIndex = 0 To Me.RepStockListBoxGL2.Items.Count - 1
            suffixName = CType(Me.RepStockListBoxGL2.Items(repStockListIndex).FindControl("Lable_SfxContentGL2"), Label).Text
            checkGradeName = CType(Me.RepStockListBoxGL2.Items(repStockListIndex).FindControl("Lable_GradeContentGL2"), Label).Text

            CType(Me.RepStockListBoxGL2.Items(repStockListIndex).FindControl("Lable_GradeContentGL2"), Label).Text = _
                GetGradeName(CType(Me.RepStockListBoxGL2.Items(repStockListIndex).FindControl("Lable_GradeContentGL2"), Label).Text, suffixName, gradeDataTable)

            'グレード名を取得
            If (Not String.Equals(gradeName, CType(Me.RepStockListBoxGL2.Items(repStockListIndex).FindControl("Lable_GradeContentGL2"), Label).Text)) Then

                ' １行目のグレードタグを追加
                preGradeName = gradeName
                gradeName = CType(Me.RepStockListBoxGL2.Items(repStockListIndex).FindControl("Lable_GradeContentGL2"), Label).Text

            Else

                ' ２行目以降のグレード名は表示させない。
                CType(Me.RepStockListBoxGL2.Items(repStockListIndex).FindControl("Lable_GradeContentGL2"), Label).Text = ""
                'Suffix取得
                If (Not String.Equals(suffixName, CType(Me.RepStockListBoxGL2.Items(repStockListIndex).FindControl("Lable_SfxContentGL2"), Label).Text)) Then
                    suffixName = CType(Me.RepStockListBoxGL2.Items(repStockListIndex).FindControl("Lable_SfxContentGL2"), Label).Text
                Else
                    ' ２行目以降のSuffix名は表示させない。
                    CType(Me.RepStockListBoxGL2.Items(repStockListIndex).FindControl("Lable_SfxContentGL2"), Label).Text = ""

                End If

            End If

            If String.IsNullOrEmpty(CType(Me.RepStockListBoxGL2.Items(repStockListIndex).FindControl("TipValueNumberOrder1"), Label).Text) Then
                CType(Me.RepStockListBoxGL2.Items(repStockListIndex).FindControl("OrderTipBlockBlue"), HtmlGenericControl).Attributes("style") = "opacity:0;"
            End If

            If String.IsNullOrEmpty(CType(Me.RepStockListBoxGL2.Items(repStockListIndex).FindControl("TipValueNumberOrder2"), Label).Text) Then
                CType(Me.RepStockListBoxGL2.Items(repStockListIndex).FindControl("OrderTipBlockYellow"), HtmlGenericControl).Attributes("style") = "opacity:0;"
            End If

            If String.IsNullOrEmpty(CType(Me.RepStockListBoxGL2.Items(repStockListIndex).FindControl("TipValueNumberOrder3"), Label).Text) Then
                CType(Me.RepStockListBoxGL2.Items(repStockListIndex).FindControl("OrderTipBlockRed"), HtmlGenericControl).Attributes("style") = "opacity:0;"
            End If

            If String.IsNullOrEmpty(CType(Me.RepStockListBoxGL2.Items(repStockListIndex).FindControl("TipValueColorStock1"), Label).Text) Then
                CType(Me.RepStockListBoxGL2.Items(repStockListIndex).FindControl("StockTipBlockBlue"), HtmlGenericControl).Attributes("style") = "opacity:0;"
            End If

            If String.IsNullOrEmpty(CType(Me.RepStockListBoxGL2.Items(repStockListIndex).FindControl("TipValueColorStock2"), Label).Text) Then
                CType(Me.RepStockListBoxGL2.Items(repStockListIndex).FindControl("StockTipBlockYellow"), HtmlGenericControl).Attributes("style") = "opacity:0;"
            End If

            If String.IsNullOrEmpty(CType(Me.RepStockListBoxGL2.Items(repStockListIndex).FindControl("TipValueColorStock3"), Label).Text) Then
                CType(Me.RepStockListBoxGL2.Items(repStockListIndex).FindControl("StockTipBlockRed"), HtmlGenericControl).Attributes("style") = "opacity:0;"
            End If

        Next

        Logger.Info("StockListUpDatet_End")
    End Sub
    '02 end GL2版メソッド

    ''' <summary>
    ''' グレードリストよりグレード名を取得します。
    ''' </summary>
    ''' <param name="gradeCode">検索用グレードコード</param>
    ''' <param name="suffix">サフィックス</param>
    ''' <param name="gradeDataTable">グレードリスト</param>
    ''' <returns>グレード名</returns>
    ''' <remarks></remarks>
    Private Function GetGradeName(ByVal gradeCode As String, ByVal suffix As String, _
                    ByVal gradeDataTable As SC3070101SearchConditionDataSet.GradeConditionDataTableDataTable) As String

        Logger.Info("GetGradeName_Start Rram[" & gradeCode & "]")

        ' 検索条件作成
        Dim wherePram As New StringBuilder
        wherePram.Append("ModelCode='" & gradeCode & "' ")
        If (Not String.IsNullOrEmpty(suffix)) Then
            Logger.Info("GetGradeName_Start_01 Add SearchParm Suffix")
            wherePram.Append("and Suffix='" & suffix & "' ")
        End If

        Dim selectRow() As DataRow = gradeDataTable.Select(wherePram.ToString())

        If selectRow.Length = 0 Then

            Logger.Info("GetGradeName_END Rram[" & gradeCode & "]")
            Return gradeCode
        Else

            Dim returnValue As String = CType(selectRow(0), SC3070101SearchConditionDataSet.GradeConditionDataTableRow).GradeName
            Logger.Info("GetGradeName_END Rram[" & returnValue & "]")
            Return returnValue
        End If

    End Function

#End Region

#Region "SFX選択リスト更新処理"

    ''' <summary>
    ''' SFX選択リスト更新処理
    ''' </summary>
    ''' <param name="grade">検索条件：グレード</param>
    ''' <remarks></remarks>
    Private Sub SetSuffixList(ByVal grade As String)

        Logger.Info("SetSuffixList_Start Prm[" & grade & "]")

        Dim sc3070101Biz As SC3070101BusinessLogic = New SC3070101BusinessLogic

        Dim suffixTable As SuffixConditionDataTableDataTable = _
            sc3070101Biz.GetSuffixConditionList(SeletedCarNameValue, grade)
        If DisplayClassValue.Value.Equals("0") Then
            Me.RepSfxListBox.DataSource = suffixTable
            Me.RepSfxListBox.DataBind()
        Else
            Me.RepSfxListBoxGL2.DataSource = suffixTable
            Me.RepSfxListBoxGL2.DataBind()

        End If

        Logger.Info("SetSuffixList_End")
    End Sub

#End Region

#Region "カラー選択リスト更新処理"

    ''' <summary>
    ''' カラー選択リスト更新処理
    ''' </summary>
    ''' <param name="grade">検索条件：グレード</param>
    ''' <param name="suffix">検索条件：サフィックス</param>
    ''' <remarks></remarks>
    Private Sub SetExteriorList(ByVal grade As String, ByVal suffix As String)

        Logger.Info("SetExteriorList_Start Prm[" & grade & "," & suffix & "]")
        Dim sc3070101Biz As SC3070101BusinessLogic = New SC3070101BusinessLogic

        Dim exteriorColorTable As ExteriorConditionDataTableDataTable = _
            sc3070101Biz.GetExteriorColorConditionList(SeletedCarNameValue, grade, suffix, String.Empty)
        If DisplayClassValue.Value.Equals("0") Then
            Me.RepExteriorListBox.DataSource = exteriorColorTable
            Me.RepExteriorListBox.DataBind()
        Else
            Me.RepExteriorListBoxGL2.DataSource = exteriorColorTable
            Me.RepExteriorListBoxGL2.DataBind()
        End If

        Logger.Info("SetExteriorList_End")
    End Sub

#End Region

#Region "画面在庫リスト表示用DataSet作成"

    ''' <summary>
    ''' 画面在庫リスト表示用DataSet作成します。
    ''' </summary>
    ''' <param name="targetRow">リスト表示用DataRow配列</param>
    ''' <returns>画面在庫リスト表示用DataSet</returns>
    ''' <remarks></remarks>
    Private Function CreateStockListDataSet(ByVal targetRow() As DataRow) As SC3070101StockListDataSet

        Logger.Info("CreateStockListDataSet_Strat Rram[" & (targetRow IsNot Nothing) & "]")

        Dim returnDataSet As SC3070101StockListDataSet = New SC3070101StockListDataSet
        Dim commonTable As SC3070101StockListDataSet.CommonDataTableDataTable = returnDataSet.CommonDataTable
        Dim orderTable As SC3070101StockListDataSet.OrderDataTableDataTable = returnDataSet.OrderDataTable
        Dim stockTable As SC3070101StockListDataSet.StockDataTableDataTable = returnDataSet.StockDataTable

        ' 在庫数分ループ
        Dim model As String = String.Empty
        Dim suffix As String = String.Empty
        Dim keyCode As Decimal = 0
        Using orderTableAdd As SC3070101StockListDataSet.OrderDataTableDataTable = New SC3070101StockListDataSet.OrderDataTableDataTable
            Using stockTableAdd As SC3070101StockListDataSet.StockDataTableDataTable = New SC3070101StockListDataSet.StockDataTableDataTable
                For Each resultRow As ResultDataTableRow In targetRow

                    'グレード・サフィックスのどちらかでも変更されたら行追加
                    If ((Not String.Equals(model, resultRow.Model)) OrElse
                        (Not String.Equals(suffix, resultRow.Suffix))) Then

                        Logger.Info("CreateStockListDataSet_001 RowChange")

                        keyCode = keyCode + 1
                        model = resultRow.Model
                        suffix = resultRow.Suffix

                        commonTable.AddCommonDataTableRow(keyCode, _
                                                          resultRow.Model, _
                                                          resultRow.Suffix)
                    End If

                    ' $01 start FTMS対応
                    '注文か在庫か判断
                    If Not (String.IsNullOrEmpty(resultRow.AcceptDate)) Then

                        Logger.Info("CreateStockListDataSet_002 AddTip Stock")
                        'If DateTimeFunc.FormatDateSD(3, resultRow.AcceptDate).HasValue Then
                        '    Dim a As Date = DateTimeFunc.FormatDateSD(3, resultRow.AcceptDate)
                        'End If

                        '店頭在庫列へ表示
                        stockTableAdd.AddStockDataTableRow(keyCode, _
                                                           resultRow.Color, _
                                                           DateTimeFunc.FormatString("dd/MM/yyyy", resultRow.AcceptDate))

                    ElseIf Not (String.IsNullOrEmpty(resultRow.NewlyDeliveryDate)) Then

                        Logger.Info("CreateStockListDataSet_003 AddTip Order")

                        '注文列へ表示
                        orderTableAdd.AddOrderDataTableRow(keyCode, _
                                                           resultRow.Color, _
                                                          DateTimeFunc.FormatString("dd/MM/yyyy", resultRow.NewlyDeliveryDate))
                    Else
                        ' 入庫日及び入庫予定日が共に設定されていない場合は、注文列へ表示する。
                        ' ソート用に日付は1900/01/01を設定
                        orderTableAdd.AddOrderDataTableRow(keyCode, _
                                                           resultRow.Color, _
                                                          DateTimeFunc.FormatString("dd/MM/yyyy", DateDefaultValue))
                    End If
                    ' $01 end  FTMS対応
                Next

                ' 注文を販売店入庫予定日にて昇順にソート
                Using orderView As DataView = New DataView(orderTableAdd)
                    orderView.Sort = "NewlyDeliveryDate asc"
                    For Each orderRowView As DataRowView In orderView
                        orderTable.ImportRow(orderRowView.Row)
                    Next
                End Using

                ' 店頭在庫を販売店入庫日にて降順にソート
                Using stockView As DataView = New DataView(stockTableAdd)
                    stockView.Sort = "AcceptDate desc"
                    For Each stockRowView As DataRowView In stockView
                        stockTable.ImportRow(stockRowView.Row)
                    Next
                End Using
            End Using
        End Using

        'リレーションを設定
        returnDataSet.Relations.Add("relationOrder", _
                           returnDataSet.Tables("CommonDataTable").Columns("KyeCode"), _
                           returnDataSet.Tables("OrderDataTable").Columns("KyeCode"))

        'リレーションを設定
        returnDataSet.Relations.Add("relationStock", _
                           returnDataSet.Tables("CommonDataTable").Columns("KyeCode"), _
                           returnDataSet.Tables("StockDataTable").Columns("KyeCode"))

        Logger.Info("CreateStockListDataSet_End Rram[" & (returnDataSet IsNot Nothing) & "]")
        Return returnDataSet

    End Function

#End Region

#Region "グレードリストよりシリーズコードを取得"

    ''' <summary>
    ''' グレードリストよりシリーズコードを取得します。
    ''' </summary>
    ''' <returns>シリーズコード</returns>
    ''' <remarks></remarks>
    Private Function GetSeriesCode() As String

        Logger.Info("GetSeriesCode_Start")

        Dim gradeDataTable As SC3070101SearchConditionDataSet.GradeConditionDataTableDataTable = _
            CType(ViewState("GradeDataTable"), SC3070101SearchConditionDataSet.GradeConditionDataTableDataTable)

        Dim returnValse As String = gradeDataTable.Item(0).VclSeriesCode
        Logger.Info("GetSeriesCode_END Ret[" & returnValse & "]")
        Return returnValse

    End Function

#End Region

#Region "文言の設定"

    ''' <summary>
    ''' 文言の設定
    ''' </summary>
    ''' <param name="ParentDisplayClassValue">画面表示区分</param>
    ''' <remarks></remarks>
    Private Sub SetWord(ByVal ParentDisplayClassValue As String)

        Logger.Info("SetWord_Strat ")

        Me.Lable_ZaikoJokyo.Text = WordFormat(WebWordUtility.GetWord(ApplicationId, WordIdZaikoJokyo), WordMaxLengthTitle)

        If ParentDisplayClassValue.Equals("0") Then
            Me.Lable_Model.Text = WordFormat(WebWordUtility.GetWord(ApplicationId, WordIdModel), WordMaxLengthSearchTitle)
            Me.Lable_Grade.Text = WordFormat(WebWordUtility.GetWord(ApplicationId, WordIdGrade), WordMaxLengthSearchTitle)
            Me.Lable_Suffix.Text = WordFormat(WebWordUtility.GetWord(ApplicationId, WordIdSuffix), WordMaxLengthSearchTitle)
            Me.Lable_Color.Text = WordFormat(WebWordUtility.GetWord(ApplicationId, WordIdColor), WordMaxLengthSearchTitle)

            Me.Lable_GradeTitle.Text = WordFormat(WebWordUtility.GetWord(ApplicationId, WordIdGrade), WordMaxLengthStockListHeadGrade)
            Me.Lable_SfxTitle.Text = WordFormat(WebWordUtility.GetWord(ApplicationId, WordIdSuffix), WordMaxLengthStockListHeadSuffix)
            Me.Lable_OrderTitle.Text = WordFormat(WebWordUtility.GetWord(ApplicationId, WordIdOrder), WordMaxLengthStockListHeadOrder)
            Me.Lable_StockTitle.Text = WordFormat(WebWordUtility.GetWord(ApplicationId, WordIdStock), WordMaxLengthStockListHeadStock)

            Me.Lable_GradeSelect.Text = WordFormat(WebWordUtility.GetWord(ApplicationId, WordIdGradeSelect), WordMaxLengthSearchListTitle)
            Me.Lable_SuffixSelect.Text = WordFormat(WebWordUtility.GetWord(ApplicationId, WordIdSuffixSelect), WordMaxLengthSearchListTitle)
            Me.Lable_ColorSelect.Text = WordFormat(WebWordUtility.GetWord(ApplicationId, WordIdColorSelect), WordMaxLengthSearchListTitle)

            WordNoSelect = Server.HtmlEncode(WebWordUtility.GetWord(ApplicationId, WordIdNoSelect))

            Me.ExteriorListNoSelect.Text = WordNoSelect
            Me.SuffixListNoSelect.Text = WordNoSelect

        Else

            Me.Lable_ModelGL2.Text = WordFormat(WebWordUtility.GetWord(ApplicationId, WordIdModel), WordMaxLengthSearchTitle)
            Me.Lable_GradeGL2.Text = WordFormat(WebWordUtility.GetWord(ApplicationId, WordIdGrade), WordMaxLengthSearchTitle)
            Me.Lable_SuffixGL2.Text = WordFormat(WebWordUtility.GetWord(ApplicationId, WordIdSuffix), WordMaxLengthSearchTitle)
            Me.Lable_ColorGL2.Text = WordFormat(WebWordUtility.GetWord(ApplicationId, WordIdColor), WordMaxLengthSearchTitle)

            Me.Lable_GradeTitleGL2.Text = WordFormat(WebWordUtility.GetWord(ApplicationId, WordIdGrade), WordMaxLengthStockListHeadGrade)
            Me.Lable_SfxTitleGL2.Text = WordFormat(WebWordUtility.GetWord(ApplicationId, WordIdSuffix), WordMaxLengthStockListHeadSuffix)
            Me.Lable_ColorTitleGL2.Text = WordFormat(WebWordUtility.GetWord(ApplicationId, WordIdColor), WordMaxLengthStockListHeadColor)
            Me.Lable_OrderTitleGL2.Text = WordFormat(WebWordUtility.GetWord(ApplicationId, WordIdOrder), WordMaxLengthStockListHeadOrder)
            Me.Lable_StockTitleGL2.Text = WordFormat(WebWordUtility.GetWord(ApplicationId, WordIdStock), WordMaxLengthStockListHeadStock)

            Me.Lable_GradeSelectGL2.Text = WordFormat(WebWordUtility.GetWord(ApplicationId, WordIdGradeSelect), WordMaxLengthSearchListTitle)
            Me.Lable_SuffixSelectGL2.Text = WordFormat(WebWordUtility.GetWord(ApplicationId, WordIdSuffixSelect), WordMaxLengthSearchListTitle)
            Me.Lable_ColorSelectGL2.Text = WordFormat(WebWordUtility.GetWord(ApplicationId, WordIdColorSelect), WordMaxLengthSearchListTitle)

            WordNoSelect = Server.HtmlEncode(WebWordUtility.GetWord(ApplicationId, WordIdNoSelect))

            Me.ExteriorListNoSelectGL2.Text = WordNoSelect
            Me.SuffixListNoSelectGL2.Text = WordNoSelect
        End If

        Logger.Info("SetWord_End")
    End Sub

#End Region

#Region "セッション情報取得"

    ''' <summary>
    ''' セッション情報取得
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetSessionValue()

        Logger.Info("GetSessionValue_Strat ")

        '画面間情報の取得
        SeletedCarNameValue = GetValue(ScreenPos.Current, SearchKeyCarName, False) '車名
        SeletedCarGradeValue = GetValue(ScreenPos.Current, SeatchKeyGrade, False) 'グレード
        SeletedCarSfxValue = GetValue(ScreenPos.Current, SearchKeySfx, False) 'SFX
        SeletedCarExteriorValue = GetValue(ScreenPos.Current, SearchKeyExteriorColor, False) '外装色
        ' $01 start 見積画面のモード取得
        '承認モード
        If ContainsKey(ScreenPos.Current, SearchKeyApprovalModeFlag) Then
            CurrentApprovalModeFlagValue = GetValue(ScreenPos.Current, SearchKeyApprovalModeFlag, False)
        Else
            CurrentApprovalModeFlagValue = String.Empty
        End If

        '価格相談モード
        If ContainsKey(ScreenPos.Current, SearchKeyPriceApprovalModeFlag) Then
            CurrentPriceApprovalMode = GetValue(ScreenPos.Current, SearchKeyPriceApprovalModeFlag, False)
        Else
            CurrentPriceApprovalMode = String.Empty
        End If
        ' $01 end 見積画面のモード取得

        Logger.Info("GetSessionValue_001 SearchKey.CAR_NAME=[" & SeletedCarNameValue & "]")
        Logger.Info("GetSessionValue_002 SearchKey.GRADE=[" & SeletedCarGradeValue & "]")
        Logger.Info("GetSessionValue_003 SearchKey.SFX=[" & SeletedCarSfxValue & "]")
        Logger.Info("GetSessionValue_004 SearchKey.COLOR_NAME=[" & SeletedCarExteriorValue & "]")
        Logger.Info("GetSessionValue_005 EstimateMode.Approval=[" & CurrentApprovalModeFlagValue & "]")
        Logger.Info("GetSessionValue_006 EstimateMode.PriceApproval=[" & CurrentPriceApprovalMode & "]")

        ' カラーコードに関しては、ICROPのカラーコードからTACT用のカラーコードに変換する。
        Dim sc3070101Biz As SC3070101BusinessLogic = New SC3070101BusinessLogic
        Dim exteriorColorTable As ExteriorConditionDataTableDataTable = _
            sc3070101Biz.GetExteriorColorConditionList(SeletedCarNameValue, SeletedCarGradeValue, SeletedCarSfxValue, SeletedCarExteriorValue)

        If (exteriorColorTable.Rows.Count > 0) Then
            SeletedCarExteriorValue = exteriorColorTable.Item(0).ExteriorColorCode
        End If

        Logger.Info("GetSessionValue_End")
    End Sub


#End Region

#Region "期間チェック"

    ''' <summary>
    ''' 基準日と対象日の期間を算出し、画面のチップのCSS名を取得します。
    ''' </summary>
    ''' <param name="targetDate">対象日</param>
    ''' <param name="baseDate">基準日</param>
    ''' <param name="isHighlight">ハイライト表示の有無</param>
    ''' <returns>Css名</returns>
    ''' <remarks></remarks>
    Private Function GetCssName(ByVal targetDate As Date, ByVal baseDate As Date, ByVal isHighlight As Boolean) As String

        Logger.Info("GetCssName_Strat ")
        ' 期間を算出
        Dim period As Long = System.Math.Abs(DateDiff(DateInterval.Day, targetDate, baseDate))

        ' $01 start FTMS対応
        If (0 <= period AndAlso period < 30) Or targetDate.Year = DateDefaultValueYear Then

            ' チップの色を青
            Logger.Info("GetCssName_End Ret[" & IIf(isHighlight, "icn01b", "icn01") & "]")
            Return IIf(isHighlight, "icn01b", "icn01")

        ElseIf (30 <= period AndAlso period < 60) Then

            ' チップの色を橙
            Logger.Info("GetCssName_End Ret[" & IIf(isHighlight, "icn02b", "icn02") & "]")
            Return IIf(isHighlight, "icn02b", "icn02")
        Else

            ' チップの色を赤
            Logger.Info("GetCssName_End Ret[" & IIf(isHighlight, "icn03b", "icn03") & "]")
            Return IIf(isHighlight, "icn03b", "icn03")
        End If
        ' $01 end  FTMS対応

    End Function

#End Region

#Region "希望車種チェック"

    ''' <summary>
    ''' 希望車種チェック
    ''' </summary>
    ''' <param name="Model">MODEL</param>
    ''' <param name="Suffix">SFX</param>
    ''' <param name="ExteriorColor">外装色</param>
    ''' <returns>希望車種かどうか</returns>
    ''' <remarks></remarks>
    Private Function IsVehicleTypeOfChoice(ByVal model As String,
                    ByVal suffix As String, ByVal exteriorColor As String) As Boolean

        Logger.Info("IsVehicleTypeOfChoice_Strat Pram[" & model & "," & suffix & "," & exteriorColor & "]")

        ' グレードチェック
        If ((Not String.IsNullOrEmpty(SeletedCarGradeValue)) AndAlso
            (Not String.Equals(Trim(model), SeletedCarGradeValue))) Then

            Logger.Info("IsVehicleTypeOfChoice_End Ret[false]")
            Return False
        End If

        ' SFXチェック
        If ((Not String.IsNullOrEmpty(SeletedCarSfxValue)) AndAlso
            (Not String.Equals(Trim(suffix), SeletedCarSfxValue))) Then

            Logger.Info("IsVehicleTypeOfChoice_End Ret[false]")
            Return False
        End If

        ' 外装色チェック
        If ((Not String.IsNullOrEmpty(SeletedCarExteriorValue)) AndAlso
            (Not String.Equals(Trim(exteriorColor), SeletedCarExteriorValue))) Then

            Logger.Info("IsVehicleTypeOfChoice_End Ret[false]")
            Return False
        End If

        ' 条件に一致
        Logger.Info("IsVehicleTypeOfChoice_End Ret[True]")
        Return True
    End Function

#End Region

#Region "文言の文字切り"

    ''' <summary>
    ''' 文言の文字切りを行います。
    ''' </summary>
    ''' <param name="wordVal">対象文字列</param>
    ''' <param name="length">桁数</param>
    ''' <returns>文字切り後の値</returns>
    ''' <remarks></remarks>
    Private Function WordFormat(ByVal wordVal As String, ByVal length As Integer) As String

        Logger.Debug("WordFormat_Start Pram[" & wordVal & "," & length & "]")

        '空文字か判定
        If String.IsNullOrEmpty(wordVal) Then

            Logger.Debug("WordFormat_001 Value is NullOrEmpty")
            Logger.Debug("WordFormat_End Ret[" & wordVal & "]")
            Return Server.HtmlEncode(wordVal)

        End If

        '最大文字数以内であればそのまま返却
        If wordVal.Length <= length Then

            Logger.Debug("WordFormat_002 Value is smallLength")
            Logger.Debug("WordFormat_End Ret[" & wordVal & "]")
            Return Server.HtmlEncode(wordVal)

        End If

        Dim retVal As String = Left(wordVal, length)

        Logger.Debug("WordFormat_End Ret[" & retVal & "]")
        Return Server.HtmlEncode(retVal)

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
    Private Function GetValue(pos As ScreenPos, key As String, removeFlg As Boolean) As Object
        Logger.Info("GetValue_Strat ")
        Logger.Info("GetValue_End ")
        Return GetPageInterface().GetValueBypass(pos, key, removeFlg)
    End Function

    ''' <summary>
    ''' ContainsKey関数のバイパス
    ''' </summary>
    ''' <param name="pos">ポジジョン</param>
    ''' <param name="key">検索キー</param>
    ''' <returns>値</returns>
    ''' <remarks></remarks>
    Private Function ContainsKey(pos As ScreenPos, key As String) As Boolean
        Return GetPageInterface().ContainsKeyBypass(pos, key)
    End Function

    ''' <summary>
    ''' ShowMessageBox関数のバイパス
    ''' </summary>
    ''' <param name="wordNo">文言No</param>
    ''' <param name="wordParam">パラメータ</param>
    ''' <remarks></remarks>
    Private Sub ShowMessageBox(wordNo As Integer, ParamArray wordParam() As String)
        Logger.Info("ShowMessageBox_Strat ")
        Logger.Info("ShowMessageBox_End ")
        GetPageInterface().ShowMessageBoxBypass(wordNo, wordParam)
    End Sub

    ''' <summary>
    ''' 親ページのインターフェース取得
    ''' </summary>
    ''' <returns>親ページのIEstimateInfoControl</returns>
    ''' <remarks></remarks>
    Private Function GetPageInterface() As IEstimateInfoControl
        Logger.Info("GetPageInterface_Strat ")
        Logger.Info("GetPageInterface_End ")
        Return CType(Me.Page, IEstimateInfoControl)
    End Function

#End Region

#Region "鮮度判定値取得"

    ''' <summary>
    ''' 鮮度判定値取得
    ''' </summary>
    ''' <param name="dispType">表示タイプ(0：GL1、1：GL2)</param>
    ''' <remarks></remarks>
    Private Sub GetJudgementDay(ByVal dispType As String)

        Logger.Info("GetJudgementDay_Start Pram[dispType=" & dispType & "]")

        If "0".Equals(dispType) Then

            ' GL1の場合は取得不要の為、0(初期値を指定)
            ' 注文鮮度判定日数
            orderVclFreshThreshold1st = "0"
            orderVclFreshThreshold2nd = "0"

            ' 在庫鮮度判定日数
            stockVclFreshThreshold1st = "0"
            stockVclFreshThreshold2nd = "0"

            Logger.Info("GetJudgementDay_End DispTyep is GL1")
            Return
        End If

        ' GL2の場合は取得
        Dim branchEnvSet As New BranchEnvSetting
        Dim context As StaffContext = StaffContext.Current

        ' 注文鮮度判定日数
        orderVclFreshThreshold1st = branchEnvSet.GetEnvSetting(context.DlrCD, context.BrnCD, OrderVclJudgementDay1st).PARAMVALUE
        orderVclFreshThreshold2nd = branchEnvSet.GetEnvSetting(context.DlrCD, context.BrnCD, OrderVclJudgementDay2nd).PARAMVALUE

        ' 在庫鮮度判定日数
        stockVclFreshThreshold1st = branchEnvSet.GetEnvSetting(context.DlrCD, context.BrnCD, StockVclJudgementDay1st).PARAMVALUE
        stockVclFreshThreshold2nd = branchEnvSet.GetEnvSetting(context.DlrCD, context.BrnCD, StockVclJudgementDay2nd).PARAMVALUE

        Logger.Info("GetJudgementDay_End Pram[Order1st=" & orderVclFreshThreshold1st & "Order2st=" & orderVclFreshThreshold2nd & "Stock1st=" & stockVclFreshThreshold1st & "Stock2st=" & stockVclFreshThreshold2nd & "]")
    End Sub

#End Region

#End Region

End Class
