'------------------------------------------------------------------------------
'SC3010501.aspx.vb
'------------------------------------------------------------------------------
'機能： サービス用共通関数処理
'補足： 
'作成： 2013/12/16 TMEJ小澤	初版作成
'更新： 2014/07/01 TMEJ 丁　 TMT_UAT対応
'更新： 2016/11/24 NSK 竹中 サブエリアのTCメインフッターのDisable対応
'更新： 
'------------------------------------------------------------------------------
Option Strict On
Option Explicit On

Imports Toyota.eCRB.SMB.ChipSearch.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Web.Controls
Imports Toyota.eCRB.SMB.ChipSearch.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.Common.OtherLinkage.BizLogic
Imports Toyota.eCRB.Common.OtherLinkage.DataAccess.SC3010501DataSet
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess.SystemEnvSettingDataSet
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.BizLogic
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.DataAccess.ServiceCommonClassDataSet

Public Class SC3010501
    Inherits BasePage

#Region "定数"

    ''' <summary>
    ''' 画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const APPLICATION_ID As String = "SC3010501"

    ''' <summary>
    ''' フッターコード：メインメニュー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_MAINMENU As Integer = 100
    ''' <summary>
    ''' フッターコード：TCメイン
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_TECHNICIAN_MAIN As Integer = 200
    ''' <summary>
    ''' フッターコード：FMメイン
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_FORMAN_MAIN As Integer = 300
    ''' <summary>
    ''' フッターコード：来店管理
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_VISIT_MANAMENT As Integer = 400
    ''' <summary>
    ''' フッターコード：R/Oボタン
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_RO As Integer = 500
    ''' <summary>
    ''' フッターコード：連絡先
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_TEL_DIRECTORY As Integer = 600
    ''' <summary>
    ''' フッターコード：顧客詳細
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_CUSTOMER As Integer = 700
    ''' <summary>
    ''' フッターコード：商品訴求コンテンツ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_CONTENTS As Integer = 800
    ''' <summary>
    ''' フッターコード：キャンペーン
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_CAMPAIGN As Integer = 900
    ''' <summary>
    ''' フッターコード：全体管理
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_ALL_MANAGMENT As Integer = 1000
    ''' <summary>
    ''' フッターコード：SMB
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_SMB As Integer = 1100
    ''' <summary>
    ''' フッターコード：追加作業ボタン
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_ADD_LIST As Integer = 1200
    ''' <summary>
    ''' フッターイベントの置換用文字列
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_REPLACE_EVENT As String = "FooterButtonClick({0});"

    ''' <summary>
    ''' メインメニュー(SA)画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PROGRAM_ID_MAINMENU_SA As String = "SC3140103"
    ''' <summary>
    ''' 全体管理画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PROGRAM_ID_ALL_MANAGMENT As String = "SC3220201"
    ''' <summary>
    ''' 工程管理画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PROGRAM_ID_PROCESS_CONTROL As String = "SC3240101"
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
    ''' 来店管理画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PROGRAM_ID_VISIT_MANAGMENT As String = "SC3100303"
    ''' <summary>
    ''' 未振当一覧画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PROGRAM_ID_ASSIGNMENT_LIST As String = "SC3100401"
    ''' <summary>
    ''' 商品訴求コンテンツ画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PROGRAM_ID_GOODS_SOLICITATION_CONTENTS As String = "SC3250101"
    ''' <summary>
    ''' 他システム連携画面画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PROGRAM_ID_OTHER_LINKAGE As String = "SC3010501"
    ''' <summary>
    ''' 電話帳ボタンのイベント
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_EVENT_TEL As String = "return schedule.appExecute.executeCont();"

    ''' <summary>
    ''' セッションキー(表示番号)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_DISP_NUM As String = "Session.DISP_NUM"
    ''' <summary>
    ''' セッションキー(表示番号14：R/O一覧)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_DATA_DISP_NUM_RO_LIST As Long = 14
    ''' <summary>
    ''' セッションキー(表示番号22：追加作業一覧)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_DATA_DISP_NUM_ADD_LIST As Long = 22
    ''' <summary>
    ''' セッションキー(表示番号15：キャンペーン)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_DATA_DISP_NUM_CAMPAIGN As Long = 15

    ''' <summary>
    ''' 他システム連携画面遷移セッションキー(パラメーター1)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_LINKAGE_PARAM1 As String = "Session.Param1"
    ''' <summary>
    ''' 他システム連携画面遷移セッションキー(パラメーター2)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_LINKAGE_PARAM2 As String = "Session.Param2"
    ''' <summary>
    ''' 他システム連携画面遷移セッションキー(パラメーター3)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_LINKAGE_PARAM3 As String = "Session.Param3"
    ''' <summary>
    ''' 他システム連携画面遷移セッションキー(パラメーター4)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_LINKAGE_PARAM4 As String = "Session.Param4"
    ''' <summary>
    ''' 他システム連携画面遷移セッションキー(パラメーター5)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_LINKAGE_PARAM5 As String = "Session.Param5"
    ''' <summary>
    ''' 他システム連携画面遷移セッションキー(パラメーター6)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_LINKAGE_PARAM6 As String = "Session.Param6"
    ''' <summary>
    ''' 他システム連携画面遷移セッションキー(パラメーター7)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_LINKAGE_PARAM7 As String = "Session.Param7"
    ''' <summary>
    ''' 他システム連携画面遷移セッションキー(パラメーター8)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_LINKAGE_PARAM8 As String = "Session.Param8"
    ''' <summary>
    ''' 他システム連携画面遷移セッションキー(パラメーター9)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_LINKAGE_PARAM9 As String = "Session.Param9"

    ''' <summary>
    ''' 商品訴求コンテンツ画面遷移セッションキー(DMS販売店コード)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_GOODS_CONTENTS_DEARLERCODE As String = "DealerCode"
    ''' <summary>
    ''' 商品訴求コンテンツ画面遷移セッションキー(DMS店舗コード)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_GOODS_CONTENTS_BRANCHCODE As String = "BranchCode"
    ''' <summary>
    ''' 商品訴求コンテンツ画面遷移セッションキー(アカウント)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_GOODS_CONTENTS_ACCOUNT As String = "LoginUserID"
    ''' <summary>
    ''' 商品訴求コンテンツ画面遷移セッションキー(来店実績連番)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_GOODS_CONTENTS_VISITSEQUENCE As String = "SAChipID"
    ''' <summary>
    ''' 商品訴求コンテンツ画面遷移セッションキー(DMS予約ID)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_GOODS_CONTENTS_RESERVEID As String = "BASREZID"
    ''' <summary>
    ''' 商品訴求コンテンツ画面遷移セッションキー(RO番号)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_GOODS_CONTENTS_REPAIRORDER As String = "R_O"
    ''' <summary>
    ''' 商品訴求コンテンツ画面遷移セッションキー(RO作業連番)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_GOODS_CONTENTS_REPAIRORDER_SEQUENCE As String = "SEQ_NO"
    ''' <summary>
    ''' 商品訴求コンテンツ画面遷移セッションキー(VIN)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_GOODS_CONTENTS_VIN As String = "VIN_NO"
    ''' <summary>
    ''' 商品訴求コンテンツ画面遷移セッションキー(編集モード)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_GOODS_CONTENTS_VIEWMODE As String = "ViewMode"

    ''' <summary>
    ''' セッションキデータ(編集モード(0：編集))
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_DATA_VIEWMODE_EDIT As String = "0"
    ''' <summary>
    ''' セッションキデータ(編集モード(1：プレビュー))
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_DATA_VIEWMODE_PREVIEW As String = "1"

    ''' <summary>
    ''' セッションキー(パラメーター)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_PARAMETER As String = "Session.Param"

    ''' <summary>
    ''' SystemEnvSetting名(他システムドメイン名)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SYSTEMENV_OTHER_LINKAGE_DOMAIN As String = "OTHER_LINKAGE_DOMAIN"

    ''' <summary>
    ''' 文言ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Enum WordId
        ''' <summary>なし</summary>
        id000 = 0
        ''' <summary>データベースとの接続でタイムアウトが発生しました。再度処理を行ってください。</summary>
        id901 = 901
    End Enum

#End Region

#Region "初期処理"

    ''' <summary>
    ''' 初期処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim staffInfo As StaffContext = StaffContext.Current

        '初回読み込み時
        If Not IsPostBack Then

        End If

        'フッター設定
        Me.InitFooterButton(staffInfo)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

#End Region

#Region "フッター制御"

    ''' <summary>
    ''' フッター制御
    ''' </summary>
    ''' <param name="commonMaster">マスターページ</param>
    ''' <param name="category">カテゴリ</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Public Overrides Function DeclareCommonMasterFooter(ByVal commonMaster As CommonMasterPage, _
                                                        ByRef category As FooterMenuCategory) As Integer()
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
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
            CType(Me.Master, CommonMasterPage).GetFooterButton(FOOTER_MAINMENU)
        AddHandler mainMenuButton.Click, AddressOf MainMenuButton_Click
        mainMenuButton.OnClientClick = _
            String.Format(CultureInfo.CurrentCulture, _
                          FOOTER_REPLACE_EVENT, _
                          FOOTER_MAINMENU.ToString(CultureInfo.CurrentCulture))

        '権限チェック
        If inStaffInfo.OpeCD = Operation.SA OrElse inStaffInfo.OpeCD = Operation.SM Then
            'SA権限、SM権限の場合
            '顧客詳細ボタンの設定
            Dim customerButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FOOTER_CUSTOMER)
            customerButton.OnClientClick = _
                String.Format(CultureInfo.CurrentCulture, _
                              FOOTER_REPLACE_EVENT, _
                              FOOTER_CUSTOMER.ToString(CultureInfo.CurrentCulture))

            'R/Oボタンの設定
            Dim roButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FOOTER_RO)
            AddHandler roButton.Click, AddressOf RoButton_Click
            roButton.OnClientClick = _
                String.Format(CultureInfo.CurrentCulture, _
                              FOOTER_REPLACE_EVENT, _
                              FOOTER_RO.ToString(CultureInfo.CurrentCulture))

            '商品訴求コンテンツボタンの設定
            Dim goodsSolicitationContentsButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FOOTER_CONTENTS)
            AddHandler goodsSolicitationContentsButton.Click, AddressOf GoodsSolicitationContentsButton_Click
            goodsSolicitationContentsButton.OnClientClick = _
                String.Format(CultureInfo.CurrentCulture, _
                              FOOTER_REPLACE_EVENT, _
                              FOOTER_CONTENTS.ToString(CultureInfo.CurrentCulture))

            'キャンペーンボタンの設定
            Dim campaignButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FOOTER_CAMPAIGN)
            AddHandler campaignButton.Click, AddressOf CampaignButton_Click
            campaignButton.OnClientClick = _
                String.Format(CultureInfo.CurrentCulture, _
                              FOOTER_REPLACE_EVENT, _
                              FOOTER_CAMPAIGN.ToString(CultureInfo.CurrentCulture))

            '来店管理ボタンの設定
            Dim visitManagmentButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FOOTER_VISIT_MANAMENT)
            AddHandler visitManagmentButton.Click, AddressOf VisitManagmentButton_Click
            visitManagmentButton.OnClientClick = _
                String.Format(CultureInfo.CurrentCulture, _
                              FOOTER_REPLACE_EVENT, _
                              FOOTER_VISIT_MANAMENT.ToString(CultureInfo.CurrentCulture))

            'SMBボタンの設定
            Dim smbButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FOOTER_SMB)
            AddHandler smbButton.Click, AddressOf SMBButton_Click
            smbButton.OnClientClick = _
                String.Format(CultureInfo.CurrentCulture, _
                              FOOTER_REPLACE_EVENT, _
                              FOOTER_SMB.ToString(CultureInfo.CurrentCulture))

        ElseIf inStaffInfo.OpeCD = Operation.CT OrElse inStaffInfo.OpeCD = Operation.TEC Then
            'CT権限、TC権限の場合
            'R/Oボタンの設定
            Dim roButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FOOTER_RO)
            AddHandler roButton.Click, AddressOf RoButton_Click
            roButton.OnClientClick = _
                String.Format(CultureInfo.CurrentCulture, _
                              FOOTER_REPLACE_EVENT, _
                              FOOTER_RO.ToString(CultureInfo.CurrentCulture))

            ''追加作業ボタンの設定
            Dim addListButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FOOTER_ADD_LIST)
            AddHandler addListButton.Click, AddressOf AddListButton_Click
            addListButton.OnClientClick = _
                String.Format(CultureInfo.CurrentCulture, _
                              FOOTER_REPLACE_EVENT, _
                              FOOTER_ADD_LIST.ToString(CultureInfo.CurrentCulture))

        ElseIf inStaffInfo.OpeCD = Operation.CHT Then
            'ChT権限の場合
            'TCメインボタンの設定
            Dim technicianMainButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FOOTER_TECHNICIAN_MAIN)
            technicianMainButton.OnClientClick = _
                String.Format(CultureInfo.CurrentCulture, _
                              FOOTER_REPLACE_EVENT, _
                              FOOTER_TECHNICIAN_MAIN.ToString(CultureInfo.CurrentCulture))
            '2016/11/24 NSK 竹中 サブエリアのTCメインフッターのDisable対応 START
            technicianMainButton.Enabled = False
            '2016/11/24 NSK 竹中 サブエリアのTCメインフッターのDisable対応 END

            'FMメインボタンの設定
            Dim FormanMainButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FOOTER_FORMAN_MAIN)
            AddHandler FormanMainButton.Click, AddressOf FormanMainButton_Click
            FormanMainButton.OnClientClick = _
                String.Format(CultureInfo.CurrentCulture, _
                              FOOTER_REPLACE_EVENT, _
                              FOOTER_FORMAN_MAIN.ToString(CultureInfo.CurrentCulture))

            'R/Oボタンの設定
            Dim roButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FOOTER_RO)
            AddHandler roButton.Click, AddressOf RoButton_Click
            roButton.OnClientClick = _
                String.Format(CultureInfo.CurrentCulture, _
                              FOOTER_REPLACE_EVENT, _
                              FOOTER_RO.ToString(CultureInfo.CurrentCulture))

            ''追加作業ボタンの設定
            Dim addListButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FOOTER_ADD_LIST)
            AddHandler addListButton.Click, AddressOf AddListButton_Click
            addListButton.OnClientClick = _
                String.Format(CultureInfo.CurrentCulture, _
                              FOOTER_REPLACE_EVENT, _
                              FOOTER_ADD_LIST.ToString(CultureInfo.CurrentCulture))

        ElseIf inStaffInfo.OpeCD = Operation.FM Then
            'FM権限の場合
            'SMBボタンの設定
            Dim smbButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FOOTER_SMB)
            AddHandler smbButton.Click, AddressOf SMBButton_Click
            smbButton.OnClientClick = _
                String.Format(CultureInfo.CurrentCulture, _
                              FOOTER_REPLACE_EVENT, _
                              FOOTER_SMB.ToString(CultureInfo.CurrentCulture))

            'R/Oボタンの設定
            Dim roButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FOOTER_RO)
            AddHandler roButton.Click, AddressOf RoButton_Click
            roButton.OnClientClick = _
                String.Format(CultureInfo.CurrentCulture, _
                              FOOTER_REPLACE_EVENT, _
                              FOOTER_RO.ToString(CultureInfo.CurrentCulture))

            ''追加作業ボタンの設定
            Dim addListButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FOOTER_ADD_LIST)
            AddHandler addListButton.Click, AddressOf AddListButton_Click
            addListButton.OnClientClick = _
                String.Format(CultureInfo.CurrentCulture, _
                              FOOTER_REPLACE_EVENT, _
                              FOOTER_ADD_LIST.ToString(CultureInfo.CurrentCulture))

        ElseIf inStaffInfo.OpeCD = Operation.SVR Then
            'SVR権限の場合

            '来店管理ボタンの設定
            Dim visitManagmentButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FOOTER_VISIT_MANAMENT)
            AddHandler visitManagmentButton.Click, AddressOf VisitManagmentButton_Click
            visitManagmentButton.OnClientClick = _
                String.Format(CultureInfo.CurrentCulture, _
                              FOOTER_REPLACE_EVENT, _
                              FOOTER_VISIT_MANAMENT.ToString(CultureInfo.CurrentCulture))

            'R/Oボタンの設定
            Dim roButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FOOTER_RO)
            AddHandler roButton.Click, AddressOf RoButton_Click
            roButton.OnClientClick = _
                String.Format(CultureInfo.CurrentCulture, _
                              FOOTER_REPLACE_EVENT, _
                              FOOTER_RO.ToString(CultureInfo.CurrentCulture))

            '全体管理ボタンの設定
            Dim allManagmentButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FOOTER_ALL_MANAGMENT)
            AddHandler allManagmentButton.Click, AddressOf AllManagmentButtonButton_Click
            allManagmentButton.OnClientClick = _
                String.Format(CultureInfo.CurrentCulture, _
                              FOOTER_REPLACE_EVENT, _
                              FOOTER_ALL_MANAGMENT.ToString(CultureInfo.CurrentCulture))

        End If

        '電話帳ボタンの設定
        Dim telDirectoryButton As CommonMasterFooterButton = _
            CType(Me.Master, CommonMasterPage).GetFooterButton(FOOTER_TEL_DIRECTORY)
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

        '権限によって遷移先を変える
        If staffInfo.OpeCD = Operation.SA Then
            'メインメニュー(SA)に遷移する
            Me.RedirectNextScreen(PROGRAM_ID_MAINMENU_SA)

        ElseIf staffInfo.OpeCD = Operation.SM Then
            '全体管理に遷移する
            Me.RedirectNextScreen(PROGRAM_ID_ALL_MANAGMENT)

        ElseIf staffInfo.OpeCD = Operation.CT OrElse staffInfo.OpeCD = Operation.CHT Then
            '工程管理に遷移する
            Me.RedirectNextScreen(PROGRAM_ID_PROCESS_CONTROL)

        ElseIf staffInfo.OpeCD = Operation.TEC Then
            'メインメニュー(TC)に遷移する
            Me.RedirectNextScreen(PROGRAM_ID_MAINMENU_TC)

        ElseIf staffInfo.OpeCD = Operation.FM Then
            'メインメニュー(FM)に遷移する
            Me.RedirectNextScreen(PROGRAM_ID_MAINMENU_FM)

        ElseIf staffInfo.OpeCD = Operation.SVR Then
            '未振当一覧に遷移する
            Me.RedirectNextScreen(PROGRAM_ID_ASSIGNMENT_LIST)

        End If

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
    ''' 来店管理ボタンを押した時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Private Sub VisitManagmentButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '来店管理画面に遷移する
        Me.RedirectNextScreen(PROGRAM_ID_VISIT_MANAGMENT)

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

        'スタッフ情報取得
        Dim staffInfo As StaffContext = StaffContext.Current

        Using biz As New SC3010501BusinessLogic

            Try
                'DMS情報取得
                Dim dtDmsCodeMapDataTable As DmsCodeMapDataTable = biz.GetDmsDealerData(staffInfo)

                'DMS情報のチェック
                If Not (IsNothing(dtDmsCodeMapDataTable)) Then
                    '取得できた場合
                    '画面間パラメータを設定
                    '表示番号
                    Me.SetValue(ScreenPos.Next, "Session.DISP_NUM", SESSION_DATA_DISP_NUM_RO_LIST)

                    'DMS販売店コード
                    Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM1, dtDmsCodeMapDataTable(0).CODE1)

                    'DMS店舗コード
                    Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM2, dtDmsCodeMapDataTable(0).CODE2)

                    'アカウント
                    Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM3, dtDmsCodeMapDataTable(0).ACCOUNT)

                    '来店実績連番
                    Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM4, String.Empty)

                    'DMS予約ID
                    Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM5, String.Empty)

                    'RO番号
                    Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM6, String.Empty)

                    'RO作業連番
                    Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM7, String.Empty)

                    'VIN
                    Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM8, String.Empty)

                    '編集モード
                    Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM9, SESSION_DATA_VIEWMODE_EDIT)

                    '追加作業画面(枠)に遷移する
                    Me.RedirectNextScreen(PROGRAM_ID_OTHER_LINKAGE)

                Else
                    '取得できなかった場合
                    'エラー
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                             , "{0}.{1} ERROR " _
                                             , Me.GetType.ToString _
                                             , System.Reflection.MethodBase.GetCurrentMethod.Name))

                End If

            Catch ex As OracleExceptionEx When ex.Number = 1013
                'ORACLEのタイムアウト処理
                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                                         , "{0}.{1} DB TIMEOUT:{2}" _
                                         , Me.GetType.ToString _
                                         , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                         , ex.Message))

                'DBタイムアウトのメッセージ表示
                Me.ShowMessageBox(WordId.id901)

            End Try

        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' 商品訴求コンテンツボタンを押した時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Private Sub GoodsSolicitationContentsButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '画面間パラメータを設定
        'DMS販売店コード
        Me.SetValue(ScreenPos.Next, SESSION_KEY_GOODS_CONTENTS_DEARLERCODE, Space(1))

        'DMS店舗コード
        Me.SetValue(ScreenPos.Next, SESSION_KEY_GOODS_CONTENTS_BRANCHCODE, Space(1))

        'アカウント
        Me.SetValue(ScreenPos.Next, SESSION_KEY_GOODS_CONTENTS_ACCOUNT, Space(1))

        '来店実績連番
        Me.SetValue(ScreenPos.Next, SESSION_KEY_GOODS_CONTENTS_VISITSEQUENCE, String.Empty)

        'DMS予約ID
        Me.SetValue(ScreenPos.Next, SESSION_KEY_GOODS_CONTENTS_RESERVEID, String.Empty)

        'RO番号
        Me.SetValue(ScreenPos.Next, SESSION_KEY_GOODS_CONTENTS_REPAIRORDER, String.Empty)

        'RO作業連番
        Me.SetValue(ScreenPos.Next, SESSION_KEY_GOODS_CONTENTS_REPAIRORDER_SEQUENCE, String.Empty)

        'VIN
        Me.SetValue(ScreenPos.Next, SESSION_KEY_GOODS_CONTENTS_VIN, String.Empty)

        '編集モード
        Me.SetValue(ScreenPos.Next, SESSION_KEY_GOODS_CONTENTS_VIEWMODE, SESSION_DATA_VIEWMODE_PREVIEW)

        '商品訴求コンテンツ画面に遷移する
        Me.RedirectNextScreen(PROGRAM_ID_GOODS_SOLICITATION_CONTENTS)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' キャンペーンボタンを押した時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Private Sub CampaignButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'スタッフ情報取得
        Dim staffInfo As StaffContext = StaffContext.Current

        Using biz As New SC3010501BusinessLogic

            Try
                'DMS情報取得
                Dim dtDmsCodeMapDataTable As DmsCodeMapDataTable = biz.GetDmsDealerData(staffInfo)

                'DMS情報のチェック
                If Not (IsNothing(dtDmsCodeMapDataTable)) Then
                    '取得できた場合
                    '画面間パラメータを設定
                    '表示番号
                    Me.SetValue(ScreenPos.Next, SESSION_KEY_DISP_NUM, SESSION_DATA_DISP_NUM_CAMPAIGN)

                    'DMS販売店コード
                    Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM1, dtDmsCodeMapDataTable(0).CODE1)

                    'DMS店舗コード
                    Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM2, dtDmsCodeMapDataTable(0).CODE2)

                    'アカウント
                    Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM3, dtDmsCodeMapDataTable(0).ACCOUNT)

                    '来店実績連番
                    Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM4, String.Empty)

                    'DMS予約ID
                    Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM5, String.Empty)

                    'RO番号
                    Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM6, String.Empty)

                    'RO作業連番
                    Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM7, String.Empty)

                    'VIN
                    Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM8, String.Empty)

                    '編集モード
                    '2014/07/01 TMEJ 丁　 TMT_UAT対応 START
                    'Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM9, SESSION_DATA_VIEWMODE_EDIT)
                    Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM9, SESSION_DATA_VIEWMODE_PREVIEW)
                    '2014/07/01 TMEJ 丁　 TMT_UAT対応 END

                    '追加作業画面(枠)に遷移する
                    Me.RedirectNextScreen(PROGRAM_ID_OTHER_LINKAGE)

                Else
                    '取得できなかった場合
                    'エラー
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                             , "{0}.{1} ERROR " _
                                             , Me.GetType.ToString _
                                             , System.Reflection.MethodBase.GetCurrentMethod.Name))

                End If

            Catch ex As OracleExceptionEx When ex.Number = 1013
                'ORACLEのタイムアウト処理
                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                                         , "{0}.{1} DB TIMEOUT:{2}" _
                                         , Me.GetType.ToString _
                                         , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                         , ex.Message))

                'DBタイムアウトのメッセージ表示
                Me.ShowMessageBox(WordId.id901)

            End Try

        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' 全体管理ボタンを押した時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Private Sub AllManagmentButtonButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '全体管理（枠）画面に遷移する
        Me.RedirectNextScreen(PROGRAM_ID_ALL_MANAGMENT)

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

        '工程管理画面に遷移する
        Me.RedirectNextScreen(PROGRAM_ID_PROCESS_CONTROL)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
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

        'スタッフ情報取得
        Dim staffInfo As StaffContext = StaffContext.Current

        Using biz As New SC3010501BusinessLogic

            Try
                'DMS情報取得
                Dim dtDmsCodeMapDataTable As DmsCodeMapDataTable = biz.GetDmsDealerData(staffInfo)

                'DMS情報のチェック
                If Not (IsNothing(dtDmsCodeMapDataTable)) Then
                    '取得できた場合
                    '画面間パラメータを設定
                    '表示番号
                    Me.SetValue(ScreenPos.Next, "Session.DISP_NUM", SESSION_DATA_DISP_NUM_ADD_LIST)

                    'DMS販売店コード
                    Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM1, dtDmsCodeMapDataTable(0).CODE1)

                    'DMS店舗コード
                    Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM2, dtDmsCodeMapDataTable(0).CODE2)

                    'アカウント
                    Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM3, staffInfo.Account.Substring(0, staffInfo.Account.IndexOf("@")))

                    '来店実績連番
                    Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM4, String.Empty)

                    'DMS予約ID
                    Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM5, String.Empty)

                    'RO番号
                    Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM6, String.Empty)

                    'RO作業連番
                    Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM7, String.Empty)

                    'VIN
                    Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM8, String.Empty)

                    '編集モード
                    Me.SetValue(ScreenPos.Next, SESSION_KEY_LINKAGE_PARAM9, SESSION_DATA_VIEWMODE_EDIT)

                    '追加作業画面(枠)に遷移する
                    Me.RedirectNextScreen(PROGRAM_ID_OTHER_LINKAGE)

                Else
                    '取得できなかった場合
                    'エラー
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                             , "{0}.{1} ERROR " _
                                             , Me.GetType.ToString _
                                             , System.Reflection.MethodBase.GetCurrentMethod.Name))

                End If

            Catch ex As OracleExceptionEx When ex.Number = 1013
                'ORACLEのタイムアウト処理
                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                                         , "{0}.{1} DB TIMEOUT:{2}" _
                                         , Me.GetType.ToString _
                                         , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                         , ex.Message))

                'DBタイムアウトのメッセージ表示
                Me.ShowMessageBox(WordId.id901)

            End Try

        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

#End Region

#Region "イベント"

    ''' <summary>
    ''' 初期表示用
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Protected Sub MainAreaReload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles MainAreaReload.Click
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '表示番号チェック取得
        If Me.ContainsKey(ScreenPos.Current, SESSION_KEY_DISP_NUM) AndAlso _
           Not (String.IsNullOrEmpty(CType(GetValue(ScreenPos.Current, SESSION_KEY_DISP_NUM, False), String))) Then
            '表示番号が存在する場合
            '表示番号チェック取得
            Dim displayNumber As Long = CType(GetValue(ScreenPos.Current, SESSION_KEY_DISP_NUM, False), Long)

            '置換用の文字列を取得して配列に入れる
            Dim parameterList As New List(Of String)
            Dim parameterType As Boolean = True
            Dim parameterIndex As Long = 1
            While parameterType
                'Sessionキー作成
                Dim key As String = String.Concat(SESSION_KEY_PARAMETER, _
                                                  parameterIndex.ToString(CultureInfo.CurrentCulture))

                'Session情報確認
                If Me.ContainsKey(ScreenPos.Current, key) Then
                    'Session情報がある場合
                    'パラメータを配列に格納
                    parameterList.Add(CType(GetValue(ScreenPos.Current, key, False), String))
                    parameterIndex += 1

                Else
                    'Session情報がある場合
                    'ループ終了
                    parameterType = False

                End If

            End While

            'TBL_SYSTEMENVからドメイン名を取得
            Dim systemEnv As New SystemEnvSetting
            Dim systemEnvParam As String = String.Empty
            Dim drSystemEnvSetting As SYSTEMENVSETTINGRow = _
                systemEnv.GetSystemEnvSetting(SYSTEMENV_OTHER_LINKAGE_DOMAIN)

            '取得できた場合のみ設定する
            If Not (IsNothing(drSystemEnvSetting)) Then
                systemEnvParam = drSystemEnvSetting.PARAMVALUE

            End If

            '表示番号とパラメータとドメインからIFrameに表示するURLを作成
            Dim url As String = Me.CreateURL(displayNumber, parameterList, systemEnvParam)

            'IFrameにURLを設定
            Me.HiddenFieldIFrameURL.Value = url

            'エリア更新
            Me.MainAreaPanel.Update()

        Else
            '存在しない場合
            'エラーを出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} ERROR:[Session.DISP_NUM] is Nothing" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' URL作成処理
    ''' </summary>
    ''' <param name="inDisplayNumber">表示番号</param>
    ''' <param name="inParameterList">置換データリスト</param>
    ''' <returns>URL</returns>
    ''' <remarks></remarks>
    Private Function CreateURL(ByVal inDisplayNumber As Long, _
                               ByVal inParameterList As List(Of String), _
                               ByVal inDomain As String) As String
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '戻り値宣言
        Dim returnURL As String = String.Empty

        Using biz As New SC3010501BusinessLogic
            Try
                'URL取得
                Dim dtDisplayRelation As SC3010501DisplayRelationDataTable = biz.GetDisplayUrl(inDisplayNumber)

                'URL取得確認
                If 0 < dtDisplayRelation.Count Then
                    '取得できた場合
                    '戻り値に設定
                    returnURL = dtDisplayRelation(0).DMS_DISP_URL


                    'ドメイン名を置換する
                    returnURL = returnURL.Replace("{0}", inDomain)

                    'パラメーターを置換する
                    Dim replaceType As Boolean = True
                    Dim replacecount As Integer = 1
                    While replaceType
                        '置換対象の文字列作成
                        Dim replaceWord As String = String.Concat("{", (replacecount).ToString(CultureInfo.CurrentCulture), "}")

                        '置換対象する文字列の存在チェック
                        If 0 <= returnURL.IndexOf(replaceWord) Then
                            '存在する場合
                            '置換するデータの確認
                            If replacecount <= inParameterList.Count Then
                                '存在する場合
                                '対象データに置換する
                                returnURL = returnURL.Replace(replaceWord, inParameterList(replacecount - 1))

                            Else
                                '存在しない場合
                                '空文字列に置換する
                                returnURL = returnURL.Replace(replaceWord, String.Empty)

                            End If
                        Else
                            '存在しない場合
                            'ループ終了
                            replaceType = False

                        End If

                        replacecount += 1
                    End While

                Else
                    '取得できなかった場合
                    'ログ出力
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} ERROR:TB_M_DMS_CODE_MAP is nothing" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name))

                End If

            Catch ex As OracleExceptionEx When ex.Number = 1013
                'ORACLEのタイムアウト処理
                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                                         , "{0}.{1} DB TIMEOUT:{2}" _
                                         , Me.GetType.ToString _
                                         , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                         , ex.Message))

                'DBタイムアウトのメッセージ表示
                Me.ShowMessageBox(WordId.id901)

            End Try

        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END URL:{2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , returnURL))
        Return returnURL
    End Function

#End Region

#Region "画面遷移処理"

#End Region

End Class
