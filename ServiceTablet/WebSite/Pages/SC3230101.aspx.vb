'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3230101.aspx.vb
'─────────────────────────────────────
'機能： メインメニュー(FM)画面 コードビハインド
'補足： 
'作成： 2014/02/XX NEC 桜井
'更新： 
'更新： 
'─────────────────────────────────────

Option Explicit On
Option Strict On
Imports Toyota.eCRB.Foreman.MainMenu.BizLogic.SC3230101
Imports Toyota.eCRB.Foreman.MainMenu.DataAccess
Imports Toyota.eCRB.Foreman.MainMenu.DataAccess.SC3230101DataSet
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Configuration
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Web.Controls
Imports System.Data
Imports System.Globalization

''' <summary>
''' メインメニュー(FM)画面
''' </summary>
''' <remarks></remarks>
Partial Class Pages_SC3230101
    Inherits BasePage

#Region "定数"

    ''' <summary>
    ''' チップのTOPマージン
    ''' </summary>
    ''' <remarks>チップデータ１行目のTop Margin</remarks>
    Private Const ChipTopMargin As Integer = 16

    ''' <summary>
    ''' チップのLEFTマージン
    ''' </summary>
    ''' <remarks>チップデータ１列目のLeft Margin</remarks>
    Private Const ChipLeftMargin As Integer = 16

    ''' <summary>
    ''' 追加作業承認待ち／完成検査承認待ちエリアの最大表示列数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ChipAreaMaxColumns As Integer = 5

    ''' <summary>
    ''' チップ高さ
    ''' </summary>
    ''' <remarks>チップ項目を表示する際、６件目毎に縦にずらす高さ</remarks>
    Private Const ChipHeight As Integer = 81

    ''' <summary>
    ''' チップ幅
    ''' </summary>
    ''' <remarks>チップ項目を表示する際の横にずらす幅</remarks>
    Private Const ChipWidth As Integer = 93

    ''' <summary>チップ情報用Style属性の設定値</summary>
    Private Const ChipStyle As String = "left:{0}px; top:{1}px; cursor:pointer;"

    ''' <summary>
    ''' パラメータのデリミタ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const Delimiter As Char = ControlChars.Tab

    ''' <summary>
    ''' 追加作業承認画面Url
    ''' </summary>
    ''' <remarks>追加作業承認画面Url</remarks>
    Private Const AddJobApprUrl As String = "SC3170301"

    ''' <summary>
    ''' 追加作業承認画面Urlとパラメータ
    ''' </summary>
    ''' <remarks>追加作業承認画面Urlとパラメータ</remarks>
    Private Const AddJobApprUrlParameter As String = AddJobApprUrl & "{0}" _
                                                   & Delimiter & "{1}" _
                                                   & Delimiter & "{2}" _
                                                   & Delimiter & "{3}" _
                                                   & Delimiter & "{4}" _
                                                   & Delimiter & "{5}" _
                                                   & Delimiter & "{6}" _
                                                   & Delimiter & "{7}" _
                                                   & Delimiter & "{8}"

    ''' <summary>
    ''' 追加作業承認画面遷移用パラメータ項目
    ''' </summary>
    ''' <remarks>追加作業承認画面に遷移際のパラメータ配列のIndex、およびセッションキー名称</remarks>
    Private Enum AddJobApprParam

        ''' <summary>追加作業承認画面用パラメータ：販売店コード</summary>
        DealerCode = 0

        ''' <summary>追加作業承認画面用パラメータ：店舗コード</summary>
        BranchCode

        ''' <summary>追加作業承認画面用パラメータ：ログインユーザID</summary>
        LoginUserID

        ''' <summary>追加作業承認画面用パラメータ：来店実績連番</summary>
        SAChipID

        ''' <summary>追加作業承認画面用パラメータ：予約ID</summary>
        BASREZID

        ''' <summary>追加作業承認画面用パラメータ：R/O番号</summary>
        R_O

        ''' <summary>追加作業承認画面用パラメータ：R/O連番</summary>
        SEQ_NO

        ''' <summary>追加作業承認画面用パラメータ：VIN</summary>
        VIN_NO

        ''' <summary>追加作業承認画面用パラメータ：Viewモード</summary>
        ViewMode

    End Enum

    ''' <summary>
    ''' 完成検査承認画面Url
    ''' </summary>
    ''' <remarks>完成検査承認画面Url</remarks>
    Private Const InsRltApprUrl As String = "SC3180201"

    ''' <summary>
    ''' 完成検査承認画面Urlとパラメータ
    ''' </summary>
    ''' <remarks>完成検査承認画面Urlとパラメータ</remarks>
    Private Const InsRltApprUrlParameter As String = InsRltApprUrl & "{0}" _
                                                   & Delimiter & "{1}" _
                                                   & Delimiter & "{2}" _
                                                   & Delimiter & "{3}" _
                                                   & Delimiter & "{4}" _
                                                   & Delimiter & "{5}" _
                                                   & Delimiter & "{6}" _
                                                   & Delimiter & "{7}" _
                                                   & Delimiter & "{8}" _
                                                   & Delimiter & "{9}"

    ''' <summary>
    ''' 完成検査承認画面遷移用パラメータ項目
    ''' </summary>
    ''' <remarks>完成検査承認画面に遷移際のパラメータ配列のIndex、およびセッションキー名称</remarks>
    Private Enum InsRltApprParam

        ''' <summary>完成検査承認画面用パラメータ：販売店コード</summary>
        DealerCode = 0

        ''' <summary>完成検査承認画面用パラメータ：店舗コード</summary>
        BranchCode

        ''' <summary>完成検査承認画面用パラメータ：ログインユーザID</summary>
        LoginUserID

        ''' <summary>完成検査承認画面用パラメータ：来店実績連番</summary>
        SAChipID

        ''' <summary>完成検査承認画面用パラメータ：予約ID</summary>
        BASREZID

        ''' <summary>完成検査承認画面用パラメータ：R/O番号</summary>
        R_O

        ''' <summary>完成検査承認画面用パラメータ：R/O連番</summary>
        SEQ_NO

        ''' <summary>完成検査承認画面用パラメータ：VIN</summary>
        VIN_NO

        ''' <summary>完成検査承認画面用パラメータ：Viewモード</summary>
        ViewMode

        ''' <summary>完成検査承認画面用パラメータ：作業内容ID</summary>
        JOB_DTL_ID
    End Enum

    ''' <summary>
    ''' 追加作業承認待ち／完成検査承認待ちのアイコンクリック用JavaScriptイベントの置換用文字列
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ChipReplaceEvent As String = "FncRedirectNextScreen('{0}');"

    ''' <summary>
    ''' GET渡しされたクエリ文字列／セッションのViewModeのキー
    ''' </summary>
    ''' <remarks>"1"：ReadOnly ／ "0"：Edit</remarks>
    Private Const KeyVieMode As String = "ViewMode"

    ''' <summary>
    ''' ViewModeモード
    ''' </summary>
    ''' <remarks>0：Edit ／ 1:ReadOnly(ReadOnlyが予約語の為、NoEditで宣言)</remarks>
    Private Enum ViewMode
        ''' <summary>Editモード</summary>
        Edit = 0
        ''' <summary>ReadOnlyモード</summary>
        NoEdit = 1
    End Enum

    ''' <summary>
    ''' 日付変換ID(HH:mm)
    ''' </summary>
    ''' <remarks>Date型の値を"HH:mm"にフォーマットする場合のID：14</remarks>
    Private Const DateFormat_HHmm As Integer = 14

    ''' <summary>
    ''' セッションキー(表示番号14：R/O一覧)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionDataDispNum_ROList As Long = 14

    ''' <summary>
    ''' セッションキー(表示番号22：追加作業一覧)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionDataDispNum_AddList As Long = 22

    ''' <summary>
    ''' セッションキー(表示番号24：追加作業承認画面)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionDataDispNum_FMAddJobAppr As Long = 24

    ''' <summary>
    ''' フッターイベントの置換用文字列
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_REPLACE_EVENT As String = "FooterButtonClick({0});"

    ''' <summary>
    ''' 電話帳ボタンのイベント
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_EVENT_TEL As String = "return schedule.appExecute.executeCont();"

    ''' <summary>
    ''' 基幹画面連携用フレーム用セッション名("Session.Param1")
    ''' </summary>
    Private Const SessionParam01 As String = "Session.Param1"

    ''' <summary>
    ''' 基幹画面連携用フレーム用セッション名("Session.Param2")
    ''' </summary>
    Private Const SessionParam02 As String = "Session.Param2"

    ''' <summary>
    ''' 基幹画面連携用フレーム用セッション名("Session.Param3")
    ''' </summary>
    Private Const SessionParam03 As String = "Session.Param3"

    ''' <summary>
    ''' 基幹画面連携用フレーム用セッション名("Session.Param4")
    ''' </summary>
    Private Const SessionParam04 As String = "Session.Param4"

    ''' <summary>
    ''' 基幹画面連携用フレーム用セッション名("Session.Param5")
    ''' </summary>
    Private Const SessionParam05 As String = "Session.Param5"

    ''' <summary>
    ''' 基幹画面連携用フレーム用セッション名("Session.Param6")
    ''' </summary>
    Private Const SessionParam06 As String = "Session.Param6"

    ''' <summary>
    ''' 基幹画面連携用フレーム用セッション名("Session.Param7")
    ''' </summary>
    Private Const SessionParam07 As String = "Session.Param7"

    ''' <summary>
    ''' 基幹画面連携用フレーム用セッション名("Session.Param8")
    ''' </summary>
    Private Const SessionParam08 As String = "Session.Param8"

    ''' <summary>
    ''' 基幹画面連携用フレーム用セッション名("Session.Param9")
    ''' </summary>
    Private Const SessionParam09 As String = "Session.Param9"

    ''' <summary>
    ''' 基幹画面連携用フレーム用セッション名("Session.DISP_NUM")
    ''' </summary>
    Private Const SessionDispNum As String = "Session.DISP_NUM"

    ''' <summary>
    ''' メインメニュー(TC)画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PROGRAM_ID_MAINMENU_TC As String = "SC3150101"

    ''' <summary>
    ''' メインメニュー(FM)画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PROGRAM_ID_MAINMENU_FM As String = "SC3230101"

    ''' <summary>
    ''' 工程管理画面ID(SMB)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PROGRAM_ID_PROCESS_CONTROL As String = "SC3240101"

    ''' <summary>
    ''' 他システム連携画面画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ProgramIDOtherLinkage As String = "SC3010501"

    ''' <summary>
    ''' メッセージID管理
    ''' </summary>
    ''' <remarks></remarks>
    Private Enum MsgID
        ''' <summary>予期せぬエラー</summary>
        id917 = 917
    End Enum

    ''' <summary>
    ''' Pマークフラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private PMarkFlg As String = "1"

    ''' <summary>
    ''' Lマークフラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private LMarkFlg As String = "2"

#End Region

#Region "イベントハンドラ"

    ''' <summary>
    ''' Page_Loadイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Logger.Error(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} START" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '画面遷移時(ポストバック以外)
        If Not Me.IsPostBack Then

            '追加作業承認待ち／完成宣言承認待ちエリア表示処理
            Me.MainRefresh()

        End If

        'ログインユーザ
        Dim staffInfo As StaffContext = StaffContext.Current

        'フッター設定
        Me.InitFooterButton(staffInfo)

        Logger.Error(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} END" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' 画面遷移用隠しボタンクリックイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>追加作業承認待ち、完成検査承認待ちのチップクリック(タップ)時、Javascriptを介してサーバ処理を執行するためのボタンクリックイベント</remarks>
    Protected Sub hdnBtnNextPage_Click(ByVal sender As Object, ByVal e As EventArgs) Handles hdnBtnNextPage.Click

        Logger.Error(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} START url=[{2}]" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name _
            , Me.hdnUrl.Value))

        'Me.RedirectNextScreen(Me.hdnUrl.Value)

        Dim parameters As String = String.Empty

        '隠し項目の値(Url+パラメータ)の先頭値(Url)によって遷移先を判断
        If Me.hdnUrl.Value.StartsWith(AddJobApprUrl) Then

            '追加作業承認画面に遷移
            Me.RedirectFMAddJobAppr()

        Else

            '完成検査承認画面に遷移
            Me.RedirectFMInspection()

        End If

        Logger.Error(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} END" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' 追加作業承認／完成検査承認待ちエリアリフレッシュ用隠しボタンクリックイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>追加作業承認待ち、完成検査承認待ちのチップクリック(タップ)時、Javascriptを介してサーバ処理を執行するためのボタンクリックイベント</remarks>
    Protected Sub hdnBtnRefreshPage_Click(ByVal sender As Object, ByVal e As EventArgs) Handles hdnBtnRefreshPage.Click

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} START" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '追加作業承認待ち／完成宣言承認待ちエリア表示処理
        Me.MainRefresh()

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} END" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

#End Region

#Region "ヘッダ"
    'Private Const CONTEXTMENU_BUSY = 1   '1～99の範囲内で採番すること
    'Private CommonMaster As CommonMasterPage

    ''' <summary>
    ''' ヘッダ制御
    ''' </summary>
    ''' <param name="pCommonMaster"></param>
    ''' <returns>コンテキストメニューに「ログアウト」を表示する</returns>
    ''' <remarks></remarks>
    Public Overrides Function DeclareCommonMasterContextMenu(ByVal pCommonMaster As CommonMasterPage) As Integer()
        ''コンテキストメニューに「商談開始」「ログアウト」を表示（ログアウトは組み込みメニュー）
        'Me.CommonMaster = pCommonMaster
        'Return New Integer() {CONTEXTMENU_BUSY, CommonMasterContextMenuBuiltinMenuID.LogoutItem}

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} START" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return New Integer() {CommonMasterContextMenuBuiltinMenuID.LogoutItem}

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} END" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Function

    'Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    '    '検索バーの制御（テキストボックスに文字列表示＆無効化）
    '    With Me.CommonMaster.SearchBox
    '        .Enabled = False
    '        .SearchText = "*****"
    '    End With

    '    'コンテキストメニューの初期化
    '    Dim busyItem As CommonMasterContextMenuItem = Me.CommonMaster.ContextMenu.GetMenuItem(CONTEXTMENU_BUSY)
    '    With busyItem
    '        .Text = "商談開始"
    '        .PresenceCategory = "2"
    '        .PresenceDetail = "0"
    '        AddHandler .Click, AddressOf busyItem_Click  'イベントハンドラは、ポストバック時でも常に割り当てる必要があります
    '    End With
    'End Sub

#End Region

#Region "フッター制御"

    ''' <summary>
    ''' ハイライトフッター設定
    ''' </summary>
    ''' <param name="commonMaster">マスターページ</param>
    ''' <param name="category">カテゴリ</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function DeclareCommonMasterFooter(ByVal commonMaster As CommonMasterPage, _
                        ByRef category As FooterMenuCategory) As Integer()

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'フッターボタンのメインメニュー(FM)をハイライト
        category = FooterMenuCategory.ForemanMain

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))


        '（表示・非表示に関わらず）使用するサブメニューボタンを宣言
        Return New Integer() {}

    End Function

    ''' <summary>
    ''' フッターボタンの初期化
    ''' </summary>
    ''' <param name="inStaffInfo">ログインユーザー情報</param>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Private Sub InitFooterButton(ByVal inStaffInfo As StaffContext)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'メインメニューボタンの設定
        Dim mainMenuButton As CommonMasterFooterButton = _
            CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.MainMenu)

        AddHandler mainMenuButton.Click, AddressOf MainMenuButton_Click

        mainMenuButton.OnClientClick = _
            String.Format(CultureInfo.CurrentCulture, _
                          FOOTER_REPLACE_EVENT, _
                          CType(FooterMenuCategory.MainMenu, Integer).ToString(CultureInfo.CurrentCulture))

        '権限チェック
        If inStaffInfo.OpeCD = Operation.FM Then
            'FM権限の場合

            'SMBボタンの設定
            Dim smbButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.SMB)

            AddHandler smbButton.Click, AddressOf SMBButton_Click

            smbButton.OnClientClick = _
                String.Format(CultureInfo.CurrentCulture, _
                              FOOTER_REPLACE_EVENT, _
                              CType(FooterMenuCategory.SMB, Integer).ToString(CultureInfo.CurrentCulture))

            'R/Oボタンの設定
            Dim roButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.RepairOrderList)

            AddHandler roButton.Click, AddressOf RoButton_Click

            roButton.OnClientClick = _
                String.Format(CultureInfo.CurrentCulture, _
                              FOOTER_REPLACE_EVENT, _
                              CType(FooterMenuCategory.RepairOrderList, Integer).ToString(CultureInfo.CurrentCulture))

            '追加作業ボタンの設定
            Dim addListButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.AddWorkList)

            'フレームワークのWeb.dllから追加作業ボタンが取得できない場合の対応
            If addListButton IsNot Nothing Then

                AddHandler addListButton.Click, AddressOf AddListButton_Click

                addListButton.OnClientClick = _
                    String.Format(CultureInfo.CurrentCulture, _
                                  FOOTER_REPLACE_EVENT, _
                                  CType(FooterMenuCategory.AddWorkList, Integer).ToString(CultureInfo.CurrentCulture))

            End If

        ElseIf inStaffInfo.OpeCD = Operation.CHT Then
            'ChT権限の場合

            '2014/07/29 ChTではTCメインボタンは使用不可能に変更(UAT#0117対応)　Start
            'TCメインボタンの設定
            Dim technicianMainButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.TechnicianMain)

            'AddHandler technicianMainButton.Click, AddressOf TechnicianMainButton_Click

            'technicianMainButton.OnClientClick = _
            '    String.Format(CultureInfo.CurrentCulture, _
            '                  FOOTER_REPLACE_EVENT, _
            '                  CType(FooterMenuCategory.TechnicianMain, Integer).ToString(CultureInfo.CurrentCulture))

            technicianMainButton.Enabled = False
            '2014/07/29 ChTではTCメインボタンは使用不可能に変更(UAT#0117対応)　End

            'FMメインボタンの設定
            Dim FormanMainButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.ForemanMain)

            AddHandler FormanMainButton.Click, AddressOf FormanMainButton_Click

            FormanMainButton.OnClientClick = _
                String.Format(CultureInfo.CurrentCulture, _
                              FOOTER_REPLACE_EVENT, _
                              CType(FooterMenuCategory.ForemanMain, Integer).ToString(CultureInfo.CurrentCulture))

            'R/Oボタンの設定
            Dim roButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.RepairOrderList)

            AddHandler roButton.Click, AddressOf RoButton_Click

            roButton.OnClientClick = _
                String.Format(CultureInfo.CurrentCulture, _
                              FOOTER_REPLACE_EVENT, _
                              CType(FooterMenuCategory.RepairOrderList, Integer).ToString(CultureInfo.CurrentCulture))

            '追加作業ボタンの設定
            Dim addListButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.AddWorkList)

            'フレームワークのWeb.dllから追加作業ボタンが取得できない場合の対応
            If addListButton IsNot Nothing Then

                AddHandler addListButton.Click, AddressOf AddListButton_Click

                addListButton.OnClientClick = _
                    String.Format(CultureInfo.CurrentCulture, _
                                  FOOTER_REPLACE_EVENT, _
                                  CType(FooterMenuCategory.AddWorkList, Integer).ToString(CultureInfo.CurrentCulture))

            End If

        End If

        '電話帳ボタンの設定
        Dim telDirectoryButton As CommonMasterFooterButton = _
            CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.Contact)

        telDirectoryButton.OnClientClick = FOOTER_EVENT_TEL

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' メインメニューボタンを押した時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Private Sub MainMenuButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim staffInfo As StaffContext = StaffContext.Current

        Logger.Info(String.Format("OperationCD=[{0}]", staffInfo.OpeCD.ToString()))

        '権限によって遷移先を変える
        If staffInfo.OpeCD = Operation.FM Then
            'メインメニュー(FM)に遷移する
            Me.RedirectNextScreen(PROGRAM_ID_MAINMENU_FM)

        ElseIf staffInfo.OpeCD = Operation.CHT Then
            '工程管理(SMB)に遷移する
            Me.RedirectNextScreen(PROGRAM_ID_PROCESS_CONTROL)

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' SMBボタンを押した時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Private Sub SMBButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '工程管理画面(SMB)に遷移する
        Me.RedirectNextScreen(PROGRAM_ID_PROCESS_CONTROL)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' R/Oボタンを押した時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Private Sub RoButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'R/O一覧画面遷移処理
        Me.RedirectOrderList()

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' R/O一覧画面遷移処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub RedirectOrderList()

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'ログインスタッフ情報取得
        Dim staffInfo As StaffContext = StaffContext.Current

        Using biz As New SC3230101BusinessLogic

            '基幹コードへ変換処理
            Dim rowDmsCodeMap As ServiceCommonClassDataSet.DmsCodeMapRow = biz.ChangeDmsCode(staffInfo)

            '基幹販売店コードチェック
            If String.IsNullOrEmpty(rowDmsCodeMap.CODE1) Then
                '値無し

                'エラーログ
                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                               , "{0}.{1} Err:rowDmsCodeMap.CODE1=NOTHING" _
                               , Me.GetType.ToString _
                               , System.Reflection.MethodBase.GetCurrentMethod.Name))

                'エラーメッセージ表示
                Me.ShowMessageBox(MsgID.id917)

                '処理終了
                Exit Sub

            End If

            '基幹店舗コードチェック
            If String.IsNullOrEmpty(rowDmsCodeMap.CODE2) Then
                '値無し

                'エラーログ
                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                               , "{0}.{1} Err:rowDmsCodeMap.CODE2=NOTHING" _
                               , Me.GetType.ToString _
                               , System.Reflection.MethodBase.GetCurrentMethod.Name))

                'エラーメッセージ表示
                Me.ShowMessageBox(MsgID.id917)

                '処理終了
                Exit Sub

            End If

            '基幹アカウントチェック
            If String.IsNullOrEmpty(rowDmsCodeMap.ACCOUNT) Then
                '値無し

                'エラーログ
                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                               , "{0}.{1} Err:rowDmsCodeMap.ACCOUNT=NOTHING" _
                               , Me.GetType.ToString _
                               , System.Reflection.MethodBase.GetCurrentMethod.Name))

                'エラーメッセージ表示
                Me.ShowMessageBox(MsgID.id917)

                '処理終了
                Exit Sub

            End If

            '次画面遷移パラメータ設定

            '販売店コード
            Me.SetValue(ScreenPos.Next, SessionParam01, rowDmsCodeMap.CODE1)
            '店舗コード
            Me.SetValue(ScreenPos.Next, SessionParam02, rowDmsCodeMap.CODE2)
            'アカウント
            Me.SetValue(ScreenPos.Next, SessionParam03, rowDmsCodeMap.ACCOUNT)
            '来店者実績連番
            Me.SetValue(ScreenPos.Next, SessionParam04, String.Empty)
            'DMS予約ID
            Me.SetValue(ScreenPos.Next, SessionParam05, String.Empty)
            'RO
            Me.SetValue(ScreenPos.Next, SessionParam06, String.Empty)
            'RO_SEQ
            Me.SetValue(ScreenPos.Next, SessionParam07, String.Empty)
            'VIN
            Me.SetValue(ScreenPos.Next, SessionParam08, String.Empty)
            'ViewMode
            Me.SetValue(ScreenPos.Next, SessionParam09, _
                        DirectCast(ViewMode.Edit, Integer).ToString(CultureInfo.CurrentCulture))
            'DISP_NUM
            Me.SetValue(ScreenPos.Next, SessionDispNum, SessionDataDispNum_ROList)

        End Using

        '基幹画面連携用フレーム呼出処理
        Me.ScreenTransition()


        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' 追加作業ボタンを押した時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Private Sub AddListButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '追加作業一覧画面遷移処理
        Me.RedirectAddList()

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' 追加作業一覧画面遷移処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub RedirectAddList()

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'ログインスタッフ情報取得
        Dim staffInfo As StaffContext = StaffContext.Current

        Using biz As New SC3230101BusinessLogic

            '基幹コードへ変換処理
            Dim rowDmsCodeMap As ServiceCommonClassDataSet.DmsCodeMapRow = biz.ChangeDmsCode(staffInfo)

            '基幹販売店コードチェック
            If String.IsNullOrEmpty(rowDmsCodeMap.CODE1) Then
                '値無し

                'エラーログ
                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                               , "{0}.{1} Err:rowDmsCodeMap.CODE1=NOTHING" _
                               , Me.GetType.ToString _
                               , System.Reflection.MethodBase.GetCurrentMethod.Name))

                'エラーメッセージ表示
                Me.ShowMessageBox(MsgID.id917)

                '処理終了
                Exit Sub

            End If

            '基幹店舗コードチェック
            If String.IsNullOrEmpty(rowDmsCodeMap.CODE2) Then
                '値無し

                'エラーログ
                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                               , "{0}.{1} Err:rowDmsCodeMap.CODE2=NOTHING" _
                               , Me.GetType.ToString _
                               , System.Reflection.MethodBase.GetCurrentMethod.Name))

                'エラーメッセージ表示
                Me.ShowMessageBox(MsgID.id917)

                '処理終了
                Exit Sub

            End If

            '基幹アカウントチェック
            If String.IsNullOrEmpty(rowDmsCodeMap.ACCOUNT) Then
                '値無し

                'エラーログ
                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                               , "{0}.{1} Err:rowDmsCodeMap.ACCOUNT=NOTHING" _
                               , Me.GetType.ToString _
                               , System.Reflection.MethodBase.GetCurrentMethod.Name))

                'エラーメッセージ表示
                Me.ShowMessageBox(MsgID.id917)

                '処理終了
                Exit Sub

            End If

            '次画面遷移パラメータ設定

            '販売店コード
            Me.SetValue(ScreenPos.Next, SessionParam01, rowDmsCodeMap.CODE1)
            '店舗コード
            Me.SetValue(ScreenPos.Next, SessionParam02, rowDmsCodeMap.CODE2)
            'アカウント
            Me.SetValue(ScreenPos.Next, SessionParam03, rowDmsCodeMap.ACCOUNT)
            '来店者実績連番
            Me.SetValue(ScreenPos.Next, SessionParam04, String.Empty)
            'DMS予約ID
            Me.SetValue(ScreenPos.Next, SessionParam05, String.Empty)
            'RO
            Me.SetValue(ScreenPos.Next, SessionParam06, String.Empty)
            'RO_SEQ
            Me.SetValue(ScreenPos.Next, SessionParam07, String.Empty)
            'VIN
            Me.SetValue(ScreenPos.Next, SessionParam08, String.Empty)
            'ViewMode
            Me.SetValue(ScreenPos.Next, SessionParam09, _
                        DirectCast(ViewMode.Edit, Integer).ToString(CultureInfo.CurrentCulture))
            'DISP_NUM
            Me.SetValue(ScreenPos.Next, SessionDispNum, SessionDataDispNum_AddList)

        End Using

        '基幹画面連携用フレーム呼出処理
        Me.ScreenTransition()

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' TCメインボタンを押した時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Private Sub TechnicianMainButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'メインメニュー(TC)画面に遷移する
        Me.RedirectNextScreen(PROGRAM_ID_MAINMENU_TC)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' FMメインボタンを押した時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Private Sub FormanMainButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'メインメニュー(FM)画面に遷移する
        Me.RedirectNextScreen(PROGRAM_ID_MAINMENU_FM)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' 基幹画面連携用フレーム(他システム連携画面)呼出処理
    ''' </summary>
    ''' <history>
    ''' </history>
    Private Sub ScreenTransition()

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '基幹画面連携用フレーム呼出
        Me.RedirectNextScreen(ProgramIDOtherLinkage)

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

#End Region

#Region "追加作業承認待ちエリア用"

    ''' <summary>
    ''' 追加作業承認待ちエリア表示処理
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub ShowAddJobApprArea()

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} START" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim biz As SC3230101BusinessLogic = New SC3230101BusinessLogic()

        'ログインユーザID取得
        Dim staffInfo As StaffContext = StaffContext.Current
        Dim userID As String = staffInfo.Account

        '追加作業承認待ちチップ情報取得
        ' BTS256 他販売店・店舗が表示されないよう修正 2015/03/31 start
        'Using dv As DataView = biz.GetAddJobApprData()
        Using dv As DataView = biz.GetAddJobApprData(staffInfo.DlrCD, staffInfo.BrnCD)
            'BTS256 他販売店・店舗が表示されないよう修正 2015/03/31 end

            Dim left As Integer = 0
            Dim top As Integer = 0
            Dim tagChips As HtmlGenericControl = New HtmlGenericControl("DIV")
            Dim tagUL As HtmlGenericControl = New HtmlGenericControl("UL")
            Dim tagLI As HtmlGenericControl
            Dim tagDIV As HtmlGenericControl
            Dim lblRegNum As CustomLabel
            Dim lblTime As CustomLabel

            Dim maxCount = dv.Count

            Dim MarkFlg As String = String.Empty

            For i = 0 To maxCount - 1

                tagLI = New HtmlGenericControl("LI")
                tagDIV = New HtmlGenericControl("DIV")
                lblRegNum = New CustomLabel
                lblTime = New CustomLabel

                tagDIV.Attributes("class") = "Chip"
                lblRegNum.CssClass = "RegNumLabel"
                lblTime.CssClass = "ROCreateTime"

                lblRegNum.Text = dv(i)("REG_NUM").ToString()
                lblTime.Text =
                    DateTimeFunc.FormatDate(DateFormat_HHmm _
                                          , DirectCast(dv(i)("RO_CREATE_DATETIME"), Date))

                'チップ項目を表示する完成検査承認待ちエリア内のロケーションを算出
                top = Me.GetChipLocationTop(i)
                left = Me.GetChipLocationLeft(i)

                If dv(i)("IMP_VCL_FLG").ToString() = PMarkFlg Then
                    MarkFlg = "withPMark"

                ElseIf dv(i)("IMP_VCL_FLG").ToString() = LMarkFlg Then
                    MarkFlg = "withLMark"

                Else
                    MarkFlg = ""

                End If

                tagLI.Attributes("class") = dv(i)("CAR_ICON_CSS").ToString() & MarkFlg
                tagLI.Attributes("style") = String.Format(ChipStyle, left, top)
                tagLI.Attributes("onclick") = Me.GetAddJobApprParam(dv(i), userID)

                tagDIV.Controls.Add(lblRegNum)
                tagDIV.Controls.Add(lblTime)
                tagLI.Controls.Add(tagDIV)
                tagUL.Controls.Add(tagLI)

            Next

            'CType(Master.FindControl("content"), ContentPlaceHolder).FindControl("AddJobApprArea").Controls.Add(tagUL)

            '作成したULタグをDIVタグに追加、DIVタグの高さを動的設定(スクロール用)
            tagChips.Controls.Add(tagUL)
            tagChips.Style(HtmlTextWriterStyle.Height) = (top + ChipHeight).ToString() & "px"

            Logger.Info("AddJobAppr Chips area DIV Height=" & tagChips.Style(HtmlTextWriterStyle.Height))

            '追加作業承認待ちチップ領域にDIVタグを追加
            Me.AddJobApprChips.Controls.Add(tagChips)

            '出力件数をセット
            Me.lblAddJobApprCount.Text = maxCount.ToString()

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} END" _
                , Me.GetType.ToString _
                , System.Reflection.MethodBase.GetCurrentMethod.Name))
        End Using

    End Sub

    ''' <summary>
    ''' 追加作業承認入力画面遷移用クライアントClick値取得
    ''' </summary>
    ''' <param name="row">追加作業承認待ちチップデータ</param>
    ''' <param name="userID">ログインユーザID</param>
    ''' <returns>追加作業承認入力画面遷移用クライアントClick値</returns>
    ''' <remarks>追加作業承認入力画面のUrlにパラメータ値を付与した値を取得する</remarks>
    Protected Function GetAddJobApprParam(ByVal row As DataRowView,
                                          ByVal userID As String) As String

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} START" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'ViewModeを取得
        Dim viewMode As String = Me.GetViewMode()

        'Urlにパラメータを付与
        Dim getParam As String = ""
        getParam = String.Format(AddJobApprUrlParameter, row("DLR_CD").ToString(), _
                                                         row("BRN_CD").ToString(), _
                                                         userID, _
                                                         row("VISITSEQ").ToString(), _
                                                         row("DMS_JOB_DTL_ID").ToString(), _
                                                         row("RO_NUM").ToString(), _
                                                         row("RO_SEQ").ToString(),
                                                         row("VIN").ToString(), _
                                                         viewMode)

        'チップクリックによって実行されるJavaScriptのClickイベントを作成
        Dim rtn As String = ""
        rtn = String.Format(ChipReplaceEvent, getParam)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} END Return=[{2}]" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name _
            , rtn))

        Return rtn

    End Function

    ''' <summary>
    ''' 追加作業承認画面のパラメータをSessionにセットして追加作業承認画面に遷移する
    ''' </summary>
    ''' <remarks>追加作業承認画面のパラメータをSessionにセットして追加作業承認画面に遷移する</remarks>
    Protected Sub RedirectFMAddJobAppr()

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} START hdnUrl.Value=[{2}]" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name _
            , Me.hdnUrl.Value))

        '隠し項目の値(Url+パラメータ)からパラメータを配列で取得
        Dim wk As String = Me.hdnUrl.Value.Substring(AddJobApprUrl.Length)
        Dim params As String() = wk.Split(Delimiter)

        ''販売店コード
        'Me.SetValue(ScreenPos.Next, AddJobApprParam.DealerCode.ToString(),
        '                            params(AddJobApprParam.DealerCode))
        '
        ''店舗コード
        'Me.SetValue(ScreenPos.Next, AddJobApprParam.BranchCode.ToString(),
        '                            params(AddJobApprParam.BranchCode))
        '
        ''ログインユーザID
        'Me.SetValue(ScreenPos.Next, AddJobApprParam.LoginUserID.ToString(),
        '                            params(AddJobApprParam.LoginUserID))
        '
        ''来店実績連番
        'Me.SetValue(ScreenPos.Next, AddJobApprParam.SAChipID.ToString(),
        '                            params(AddJobApprParam.SAChipID))
        '
        ''予約ID
        'Me.SetValue(ScreenPos.Next, AddJobApprParam.BASREZID.ToString(),
        '                            params(AddJobApprParam.BASREZID))
        '
        ''R/O番号
        'Me.SetValue(ScreenPos.Next, AddJobApprParam.R_O.ToString(),
        '                            params(AddJobApprParam.R_O))
        '
        ''R/O連番
        'Me.SetValue(ScreenPos.Next, AddJobApprParam.SEQ_NO.ToString(),
        '                            params(AddJobApprParam.SEQ_NO))
        '
        ''VIN
        'Me.SetValue(ScreenPos.Next, AddJobApprParam.VIN_NO.ToString(),
        '                            params(AddJobApprParam.VIN_NO))
        '
        ''Viewモード
        'Me.SetValue(ScreenPos.Next, AddJobApprParam.ViewMode.ToString(),
        '                            params(AddJobApprParam.ViewMode))
        '
        ''追加作業承認画面へ遷移
        'Me.RedirectNextScreen(AddJobApprUrl)


        'ログインスタッフ情報取得
        Dim staffInfo As StaffContext = StaffContext.Current

        Using biz As New SC3230101BusinessLogic

            '基幹コードへ変換処理
            Dim rowDmsCodeMap As ServiceCommonClassDataSet.DmsCodeMapRow = biz.ChangeDmsCode(staffInfo)

            '基幹販売店コードチェック
            If String.IsNullOrEmpty(rowDmsCodeMap.CODE1) Then
                '値無し

                'エラーログ
                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                               , "{0}.{1} Err:rowDmsCodeMap.CODE1=NOTHING" _
                               , Me.GetType.ToString _
                               , System.Reflection.MethodBase.GetCurrentMethod.Name))

                'エラーメッセージ表示
                Me.ShowMessageBox(MsgID.id917)

                '処理終了
                Exit Sub

            End If

            '基幹店舗コードチェック
            If String.IsNullOrEmpty(rowDmsCodeMap.CODE2) Then
                '値無し

                'エラーログ
                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                               , "{0}.{1} Err:rowDmsCodeMap.CODE2=NOTHING" _
                               , Me.GetType.ToString _
                               , System.Reflection.MethodBase.GetCurrentMethod.Name))

                'エラーメッセージ表示
                Me.ShowMessageBox(MsgID.id917)

                '処理終了
                Exit Sub

            End If

            '基幹アカウントチェック
            If String.IsNullOrEmpty(rowDmsCodeMap.ACCOUNT) Then
                '値無し

                'エラーログ
                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                               , "{0}.{1} Err:rowDmsCodeMap.ACCOUNT=NOTHING" _
                               , Me.GetType.ToString _
                               , System.Reflection.MethodBase.GetCurrentMethod.Name))

                'エラーメッセージ表示
                Me.ShowMessageBox(MsgID.id917)

                '処理終了
                Exit Sub

            End If

            '次画面遷移パラメータ設定

            '販売店コード
            Me.SetValue(ScreenPos.Next, SessionParam01, rowDmsCodeMap.CODE1)
            '店舗コード
            Me.SetValue(ScreenPos.Next, SessionParam02, rowDmsCodeMap.CODE2)
            'アカウント
            Me.SetValue(ScreenPos.Next, SessionParam03, rowDmsCodeMap.ACCOUNT)
            '来店者実績連番
            Me.SetValue(ScreenPos.Next, SessionParam04, params(AddJobApprParam.SAChipID))
            'DMS予約ID
            Me.SetValue(ScreenPos.Next, SessionParam05, params(AddJobApprParam.BASREZID))
            'RO
            Me.SetValue(ScreenPos.Next, SessionParam06, params(AddJobApprParam.R_O))
            'RO_SEQ
            Me.SetValue(ScreenPos.Next, SessionParam07, params(AddJobApprParam.SEQ_NO))
            'VIN
            Me.SetValue(ScreenPos.Next, SessionParam08, params(AddJobApprParam.VIN_NO))
            'ViewMode
            Me.SetValue(ScreenPos.Next, SessionParam09, params(AddJobApprParam.ViewMode))
            'DISP_NUM
            Me.SetValue(ScreenPos.Next, SessionDispNum, SessionDataDispNum_FMAddJobAppr)

        End Using

        '基幹画面連携用フレーム呼出処理
        Me.ScreenTransition()

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} END" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

#End Region

#Region "完成検査承認待ちエリア用"

    ''' <summary>
    ''' 完成検査承認待ちエリア表示処理
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub ShowInsRltApprArea()

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} START" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim biz As SC3230101BusinessLogic = New SC3230101BusinessLogic()

        ' BTS256 他販売店・店舗が表示されないよう修正 2015/03/31 start
        'ログインユーザID取得
        Dim staffInfo As StaffContext = StaffContext.Current

        '完成検査承認待ちチップ情報取得
        'Using dv As DataView = biz.GetInsRltApprData()
        Using dv As DataView = biz.GetInsRltApprData(staffInfo.DlrCD, staffInfo.BrnCD)
            ' BTS256 他販売店・店舗が表示されないよう修正 2015/03/31 end

            Dim left As Integer = 0
            Dim top As Integer = 0
            Dim tagChips As HtmlGenericControl = New HtmlGenericControl("DIV")
            Dim tagUL As HtmlGenericControl = New HtmlGenericControl("UL")
            Dim tagLI As HtmlGenericControl
            Dim tagDIV As HtmlGenericControl
            Dim lblRegNum As CustomLabel
            Dim lblStall As CustomLabel
            Dim lblTime As CustomLabel

            Dim maxCount = dv.Count

            Dim MarkFlg As String = String.Empty

            For i = 0 To maxCount - 1

                tagLI = New HtmlGenericControl("LI")
                tagDIV = New HtmlGenericControl("DIV")
                lblRegNum = New CustomLabel
                lblStall = New CustomLabel
                lblTime = New CustomLabel

                tagDIV.Attributes("class") = "Chip"
                lblRegNum.CssClass = "RegNumLabel"
                lblStall.CssClass = "StallName"
                lblTime.CssClass = "RSLTEndTime"

                lblRegNum.Text = dv(i)("REG_NUM").ToString()
                lblStall.Text = Me.GetStallName(dv, i)
                lblTime.Text =
                    DateTimeFunc.FormatDate(DateFormat_HHmm _
                                          , DirectCast(dv(i)("RSLT_END_DATETIME"), Date))

                'チップ項目を表示する完成検査承認待ちエリア内のロケーションを算出
                top = Me.GetChipLocationTop(i)
                left = Me.GetChipLocationLeft(i)

                If dv(i)("IMP_VCL_FLG").ToString() = PMarkFlg Then
                    MarkFlg = "withPMark"

                ElseIf dv(i)("IMP_VCL_FLG").ToString() = LMarkFlg Then
                    MarkFlg = "withLMark"

                Else
                    MarkFlg = ""

                End If
                tagLI.Attributes("class") = dv(i)("CAR_ICON_CSS").ToString() & MarkFlg
                tagLI.Attributes("style") = String.Format(ChipStyle, left, top)
                tagLI.Attributes("onclick") = Me.GetInsRltApprParam(dv(i))

                tagDIV.Controls.Add(lblRegNum)
                tagDIV.Controls.Add(lblStall)
                tagDIV.Controls.Add(lblTime)
                tagLI.Controls.Add(tagDIV)
                tagUL.Controls.Add(tagLI)

            Next

            'CType(Master.FindControl("content"), ContentPlaceHolder).FindControl("InsRltApprArea").Controls.Add(tagUL)

            '作成したULタグをDIVタグに追加、DIVタグの高さを動的設定(スクロール用)
            tagChips.Controls.Add(tagUL)
            tagChips.Style(HtmlTextWriterStyle.Height) = (top + ChipHeight).ToString() & "px"

            Logger.Info("InsRltAppr Chips area DIV Height=" & tagChips.Style(HtmlTextWriterStyle.Height))

            '完成検査承認待ちチップ領域にDIVタグを追加
            Me.InsRltApprChips.Controls.Add(tagChips)

            '出力件数をセット
            Me.lblInsRltApprCount.Text = maxCount.ToString()

        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} END" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' 完成検査承認待ちエリアのストール名称取得処理
    ''' </summary>
    ''' <param name="dv">完成検査承認待ちの全チップデータ</param>
    ''' <param name="index">現在の処理対象となるDataViewのIndex</param>
    ''' <returns>ストール名称</returns>
    ''' <remarks>重複した車両登録番号が存在する場合、ストール名称を出力し、そうでない場合はブランクを返す。</remarks>
    Protected Function GetStallName(ByVal dv As DataView, ByVal index As Integer) As String

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} START" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim rtn As String = String.Empty

        '同一の車両登録番号が複数存在する場合のみ、ストール名称を表示する
        dv.RowFilter = String.Format("REG_NUM = '{0}'", dv(index)("REG_NUM").ToString().Replace("'", "''"))
        If 1 < dv.Count Then
            dv.RowFilter = String.Empty
            rtn = dv(index)("STALL_NAME_SHORT").ToString()
        End If
        dv.RowFilter = String.Empty

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} END" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return rtn

    End Function

    ''' <summary>
    ''' 完成検査承認入力画面遷移用クライアントClick値取得
    ''' </summary>
    ''' <param name="row">完成検査承認待ちチップデータ</param>
    ''' <returns>完成検査承認入力画面遷移用クライアントClick値</returns>
    ''' <remarks>パラメータをGet渡しする完成検査承認入力画面のUrlを作成し、それをパラメータとするクライアントClick値を取得する</remarks>
    Protected Function GetInsRltApprParam(ByVal row As DataRowView) As String

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} START" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'ViewModeを取得
        Dim viewMode As String = Me.GetViewMode()
        'TR-SVT-TMT-20160512-001 サービス来店者管理が存在しない場合にもFMメインを有効とする start
        'DBNULL回避のため、NULLであれば-2が入っている。（未指定などでは-1が入るため-2とする）
        '値がなければ空文字となる
        Dim tmpREZID As String = String.Empty
        If row("REZID").ToString <> "-2" Then
            tmpREZID = row("REZID").ToString
        End If
        'TR-SVT-TMT-20160512-001 サービス来店者管理が存在しない場合にもFMメインを有効とする end

        'Urlにパラメータを付与
        Dim getParam As String = ""
        getParam = String.Format(InsRltApprUrlParameter, String.Empty, _
                                                         String.Empty, _
                                                         String.Empty, _
                                                         row("VISITSEQ").ToString(), _
                                                         tmpREZID, _
                                                         row("RO_NUM").ToString(), _
                                                         row("RO_SEQ").ToString(),
                                                         row("VIN").ToString(), _
                                                         viewMode, _
                                                         row("JOB_DTL_ID").ToString())

        'チップクリックによって実行されるJavaScriptのClickイベントを作成
        Dim rtn As String = ""
        rtn = String.Format(ChipReplaceEvent, getParam)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} END Return=[{2}]" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name _
            , rtn))

        Return rtn

    End Function

    ''' <summary>
    ''' 完成検査承認画面のパラメータをSessionにセットして完成検査承認画面に遷移する
    ''' </summary>
    ''' <remarks>完成検査承認画面のパラメータをSessionにセットして完成検査承認画面に遷移する</remarks>
    Protected Sub RedirectFMInspection()

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} START hdnUrl.Value=[{2}]" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name _
            , Me.hdnUrl.Value))

        '隠し項目の値(Url+パラメータ)からパラメータを配列で取得
        Dim wk As String = Me.hdnUrl.Value.Substring(InsRltApprUrl.Length)
        Dim params As String() = wk.Split(Delimiter)

        '販売店コード
        Me.SetValue(ScreenPos.Next, InsRltApprParam.DealerCode.ToString(),
                                    params(InsRltApprParam.DealerCode))

        '店舗コード
        Me.SetValue(ScreenPos.Next, InsRltApprParam.BranchCode.ToString(),
                                    params(InsRltApprParam.BranchCode))

        'ログインユーザID
        Me.SetValue(ScreenPos.Next, InsRltApprParam.LoginUserID.ToString(),
                                    params(InsRltApprParam.LoginUserID))

        '来店実績連番
        Me.SetValue(ScreenPos.Next, InsRltApprParam.SAChipID.ToString(),
                                    params(InsRltApprParam.SAChipID))

        '予約ID
        Me.SetValue(ScreenPos.Next, InsRltApprParam.BASREZID.ToString(),
                                    params(InsRltApprParam.BASREZID))

        'R/O番号
        Me.SetValue(ScreenPos.Next, InsRltApprParam.R_O.ToString(),
                                    params(InsRltApprParam.R_O))

        'R/O連番
        Me.SetValue(ScreenPos.Next, InsRltApprParam.SEQ_NO.ToString(),
                                    params(InsRltApprParam.SEQ_NO))

        'VIN
        Me.SetValue(ScreenPos.Next, InsRltApprParam.VIN_NO.ToString(),
                                    params(InsRltApprParam.VIN_NO))

        'Viewモード
        Me.SetValue(ScreenPos.Next, InsRltApprParam.ViewMode.ToString(),
                                    params(InsRltApprParam.ViewMode))

        '作業内容ID
        Me.SetValue(ScreenPos.Next, InsRltApprParam.JOB_DTL_ID.ToString(),
                                    params(InsRltApprParam.JOB_DTL_ID))

        '完成検査承認画面へ遷移
        Me.RedirectNextScreen(InsRltApprUrl)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} END" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

#End Region

#Region "追加作業承認待ち／完成検査承認待ちの両エリア共通処理"

    ''' <summary>
    ''' 追加作業承認待ち／完成宣言承認待ちエリア表示処理
    ''' </summary>
    ''' <remarks>追加作業承認待ち／完成宣言承認待ちエリアをリフレッシュする</remarks>
    Protected Sub MainRefresh()

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} START" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Me.ShowAddJobApprArea()
        Me.ShowInsRltApprArea()

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} END" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' 追加作業承認待ち／完成検査承認待ちエリアのチップ項目Top座標取得処理
    ''' </summary>
    ''' <param name="i">チップ項目のカウンタ(ゼロオリジンとして、何件目か)</param>
    ''' <returns>チップ項目のTop座標値</returns>
    ''' <remarks>出力対象となるチップ項目のカウンタを元に、表示エリア内のTOP座標を算出する</remarks>
    Protected Function GetChipLocationTop(ByVal i As Integer) As Integer

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} START" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim top As Integer

        top = ChipTopMargin + (i \ ChipAreaMaxColumns) * ChipHeight

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} END Top={2}" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name _
            , top.ToString()))

        Return top

    End Function

    ''' <summary>
    ''' 追加作業承認待ち／完成検査承認待ちエリアのチップ項目Left座標取得処理
    ''' </summary>
    ''' <param name="i">チップ項目のカウンタ(ゼロオリジンとして、何件目か)</param>
    ''' <returns>チップ項目のLeft座標値</returns>
    ''' <remarks>出力対象となるチップ項目のカウンタを元に、表示エリア内のLEFT座標を算出する</remarks>
    Protected Function GetChipLocationLeft(ByVal i As Integer) As Integer

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} START" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim Left As Integer

        Left = ChipLeftMargin + (i Mod ChipAreaMaxColumns) * ChipWidth

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} END" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return Left

    End Function

    ''' <summary>
    ''' ViewMode取得処理
    ''' </summary>
    ''' <returns>"0"：Edit ／ "1"：ReadOnly</returns>
    ''' <remarks>ViewModeの値を取得する。Sessionから取得できなかった場合は"0"(=Edit)モードを返す。</remarks>
    Protected Function GetViewMode() As String

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} START" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim rtn As String = String.Empty

        'If Request.QueryString.AllKeys.Contains(KeyVieMode) Then
        '    'Get渡しされたKeyVieModeがあれば値を取得する
        '    rtn = Request.QueryString(KeyVieMode)
        '
        'ElseIf Me.ContainsKey(ScreenPos.Current, KeyVieMode) Then
        '
        '    'SessionにKeyVieModeがあれば値を取得する
        '    rtn = Me.GetValue(ScreenPos.Current, KeyVieMode, False)
        '
        'End If

        'SessionにKeyVieModeがあれば値を取得する
        If Me.ContainsKey(ScreenPos.Current, KeyVieMode) Then

            rtn = CType(Me.GetValue(ScreenPos.Current, KeyVieMode, False), String)

        End If

        'ViewModeが取得できなかった場合(含む取得値 = "")、Editモードとする
        If String.IsNullOrEmpty(rtn) Then
            rtn = DirectCast(ViewMode.Edit, Integer).ToString(CultureInfo.CurrentCulture)
        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} END" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return rtn

    End Function

#End Region

End Class
