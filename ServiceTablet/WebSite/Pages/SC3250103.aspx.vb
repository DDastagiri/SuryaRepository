'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3250103.aspx.vb
'─────────────────────────────────────
'機能： 部品説明画面 コードビハインド
'補足： 
'作成： 2014/08/XX NEC 上野
'更新： 
'─────────────────────────────────────

Option Explicit On
Option Strict On

Imports System.Data
Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess.SystemEnvSettingDataSet
Imports Toyota.eCRB.iCROP.BizLogic.SC3250103
Imports Toyota.eCRB.iCROP.DataAccess.SC3250103

''' <summary>
''' 部品説明画面
''' </summary>
''' <remarks></remarks>
Partial Class SC3250103
    Inherits BasePage

#Region "メンバ変数"

    ''' <summary>Getパラメーター格納</summary>
    Private Params As New Parameters

#End Region

#Region "定数"

    ''' <summary>
    ''' カートボタン（有効）
    ''' </summary>
    Private Const Cart_Enable As String = "Cart_Enable"

    ''' <summary>
    ''' カートボタン（無効）
    ''' </summary>
    Private Const Cart_Disable As String = "Cart_Disable"

    ''' <summary>
    ''' Log開始用文言
    ''' </summary>
    ''' <remarks></remarks>
    Private Const LOG_START As String = "Start"

    ''' <summary>
    ''' Log終了文言
    ''' </summary>
    ''' <remarks></remarks>
    Private Const LOG_END As String = "End"

    ''' <summary>
    ''' コンテンツボックスのCSSクラス名
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONTENT_CLASS_NAME = "pdcontentBox"

    ''' <summary>
    ''' コンテンツボックス最小化　CSSクラス名
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONTENT_MINIMIZE = "OnlyTitleBar"

    ''' <summary>
    ''' コンテンツボックス最小化　CSSクラス名
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONTENT_SHOW = "ShowContents"

    ''' <summary>
    ''' プログラムID：基幹画面連携用フレーム("SC3010501")
    ''' </summary>
    Private Const APPLICATIONID_FRAMEID As String = "SC3010501"

    ''' <summary>
    ''' プログラムID：（SA）SAメイン画面（SC3140103）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SA_MAINMENUID As String = "SC3140103"

    ''' <summary>
    ''' プログラムID：（SM）全体管理画面 （SC3220201）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const APPLICATIONID_GENERALMANAGER As String = "SC3220201"

    ''' <summary>
    ''' プログラムID：未振当て一覧画面（SC3100401）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const APPLICATIONID_NOASSIGNMENTLIST As String = "SC3100401"

    ''' <summary>
    ''' プログラムID：予約管理画面　（SC3100303）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const APPLICATIONID_VSTMANAGER As String = "SC3100303"

    ''' <summary>
    ''' プログラムID：SMB（SC3240101）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PROCESS_CONTROL_PAGE As String = "SC3240101"

    ''' <summary>
    ''' プログラムID：商品訴求コンテンツ画面（SC3250101）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PGMID_GOOD_SOLICITATION_CONTENTS As String = "SC3250101"

    ''' <summary>
    ''' 編集モードフラグ("1"；リードオンリー) 
    ''' </summary>
    Private Const ReadMode As String = "1"

    ''' <summary>
    ''' 編集モードフラグ("0"；編集) 
    ''' </summary>
    Private Const EditMode As String = "0"

    ''' <summary>
    ''' SessionValue(画面番号)：RO一覧
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONVALUE_RO_LIST As Long = 14

    ''' <summary>
    ''' キャンペーン画面(DISP_NUM:"15")
    ''' </summary>
    Private Const APPLICATIONID_CAMPAIGN As Long = 15

    ''' <summary>
    ''' カート画面(DISP_NUM:"17")
    ''' </summary>
    Private Const APPLICATIONID_CART As Long = 17


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
    ''' 基幹画面連携用フレーム用セッション名("Session.Param10")
    ''' </summary>
    Private Const SessionParam10 As String = "Session.Param10"

    ''' <summary>
    ''' 基幹画面連携用フレーム用セッション名("Session.Param11")
    ''' </summary>
    Private Const SessionParam11 As String = "Session.Param11"

    ''' <summary>
    ''' 基幹画面連携用フレーム用セッション名("Session.Param12")
    ''' </summary>
    Private Const SessionParam12 As String = "Session.Param12"

    ''' <summary>
    ''' 基幹画面連携用フレーム用セッション名("Session.DISP_NUM")
    ''' </summary>
    Private Const SessionDispNum As String = "Session.DISP_NUM"

    ''' <summary>
    ''' セッション名("DealerCode")
    ''' </summary>
    Private Const SessionDealerCode As String = "DealerCode"

    ''' <summary>
    ''' セッション名("BranchCode")
    ''' </summary>
    Private Const SessionBranchCode As String = "BranchCode"

    ''' <summary>
    ''' セッション名("LoginUserID")
    ''' </summary>
    Private Const SessionLoginUserID As String = "LoginUserID"

    ''' <summary>
    ''' セッション名("SAChipID")
    ''' </summary>
    Private Const SessionSAChipID As String = "SAChipID"

    ''' <summary>
    ''' セッション名("BASREZID")
    ''' </summary>
    Private Const SessionBASREZID As String = "BASREZID"

    ''' <summary>
    ''' セッション名("R_O")
    ''' </summary>
    Private Const SessionRO As String = "R_O"

    ''' <summary>
    ''' セッション名("SEQ_NO")
    ''' </summary>
    Private Const SessionSEQNO As String = "SEQ_NO"

    ''' <summary>
    ''' セッション名("VIN_NO")
    ''' </summary>
    Private Const SessionVINNO As String = "VIN_NO"

    ''' <summary>
    ''' セッション名("ViewMode")
    ''' </summary>
    Private Const SessionViewMode As String = "ViewMode"

    ''' <summary>
    ''' セッション名("商品訴求用部位コード")
    ''' </summary>
    Private Const SessionReqPartCD As String = "ReqPartCD"

    ''' <summary>
    ''' セッション名("点検項目コード")
    ''' </summary>
    Private Const SessionInspecItemCD As String = "InspecItemCD"

    ''' <summary>
    ''' SystemEnvSetting名(部品カタログ)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OTHER_LINKAGE_DOMAIN As String = "OTHER_LINKAGE_DOMAIN"

#End Region

#Region "列挙体"

    ''' <summary>
    ''' コンテンツ表示設定フラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Enum Content
        Box1 = 0
        Box2 = 1
        Box3 = 2
        Box4 = 3
        Box5 = 4

        None = 0
        Show = 1
    End Enum

#End Region

#Region "クラス"

    ''' <summary>
    ''' Getパラメーター格納用クラス
    ''' </summary>
    ''' <remarks></remarks>
    Private Class Parameters
        ''' <summary>販売店コード</summary>
        Public DealerCode As String
        ''' <summary>店舗コード</summary>
        Public BranchCode As String
        ''' <summary>ログインユーザID</summary>
        Public LoginUserID As String
        ''' <summary>SAChipID</summary>
        Public SAChipID As String
        ''' <summary>BASREZID</summary>
        Public BASREZID As String
        ''' <summary>R/O</summary>
        Public R_O As String
        ''' <summary>SEQ_NO</summary>
        Public SEQ_NO As String
        ''' <summary>VIN_NO</summary>
        Public VIN_NO As String
        ''' <summary>ViewMode 1=Readonly / 0=Edit</summary>
        Public ViewMode As String
        ''' <summary>ReqPartCD（商品訴求用部位コード）</summary>
        Public ReqPartCD As String
        ''' <summary>InspecItemCD（点検項目コード）</summary>
        Public InspecItemCD As String

    End Class

#End Region

#Region "イベントハンドラ"

    ''' <summary>
    ''' Page_Loadイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        '開始ログの記録
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2} " _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START))

        '初期化
        InitProc()

        '初期表示設定
        InitViewProc()

        '終了ログの記録
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))

    End Sub

    ''' <summary>
    ''' カート画面遷移隠しボタン　クリックイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ButtonCart_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonCart.Click
        '開始ログの記録
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2} " _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START))

        Using Biz As New SC3250103BusinessLogic

            '基幹コードへ変換処理
            Dim staffInfo As StaffContext = StaffContext.Current
            Dim rowDmsCodeMap As SC3250103DataSet.DmsCodeMapRow = Biz.ChangeDmsCode(staffInfo)

            '基幹販売店コードチェック
            If String.Empty.Equals(rowDmsCodeMap.CODE1) Then
                '値無し
                rowDmsCodeMap.CODE1 = Params.DealerCode
            End If

            '基幹店舗コードチェック
            If String.Empty.Equals(rowDmsCodeMap.CODE2) Then
                '値無し
                rowDmsCodeMap.CODE2 = Params.BranchCode
            End If

            '販売店コード
            Me.SetValue(ScreenPos.Next, SessionParam01, rowDmsCodeMap.CODE1)
            '店舗コード
            Me.SetValue(ScreenPos.Next, SessionParam02, rowDmsCodeMap.CODE2)
            'アカウント
            'ログインユーザーの「＠」以降を削除する
            Dim UserName As String
            If Params.LoginUserID.Contains("@") Then
                'ユーザーに「＠」が含まれている
                UserName = Params.LoginUserID.Substring(0, Params.LoginUserID.IndexOf("@"))
            Else
                UserName = Params.LoginUserID
            End If
            Logger.Info("UserName = " & UserName)
            Me.SetValue(ScreenPos.Next, SessionParam03, UserName)
            'Me.SetValue(ScreenPos.Next, SessionParam03, Params.LoginUserID)
            '来店者実績連番
            Me.SetValue(ScreenPos.Next, SessionParam04, Params.SAChipID)
            'DMS予約ID
            Me.SetValue(ScreenPos.Next, SessionParam05, Params.BASREZID)
            'RO
            Me.SetValue(ScreenPos.Next, SessionParam06, Params.R_O)
            'RO_JOB_SEQ
            Me.SetValue(ScreenPos.Next, SessionParam07, Params.SEQ_NO)
            'VIN
            Me.SetValue(ScreenPos.Next, SessionParam08, Params.VIN_NO)
            'ViewMode
            Me.SetValue(ScreenPos.Next, SessionParam09, Params.ViewMode)
            'DISP_NUM
            Me.SetValue(ScreenPos.Next, SessionDispNum, APPLICATIONID_CART)

        End Using

        '基幹画面連携用フレーム呼出処理
        Me.ScreenTransition()

        '終了ログの記録
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))

    End Sub

#End Region

#Region "フッター制御"

    ' ''' <summary>
    ' ''' ハイライトフッター設定
    ' ''' </summary>
    ' ''' <param name="commonMaster">マスターページ</param>
    ' ''' <param name="category">カテゴリ</param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Overrides Function DeclareCommonMasterFooter(ByVal commonMaster As CommonMasterPage, _
    '                    ByRef category As FooterMenuCategory) As Integer()

    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                , "{0}.{1} START" _
    '                , Me.GetType.ToString _
    '                , System.Reflection.MethodBase.GetCurrentMethod.Name))

    '    'フッターボタンの商品訴求をハイライト
    '    category = FooterMenuCategory.GoodsSolicitationContents

    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                , "{0}.{1} END" _
    '                , Me.GetType.ToString _
    '                , System.Reflection.MethodBase.GetCurrentMethod.Name))

    '    Return New Integer() {}

    'End Function

    ''' <summary>
    ''' フッターボタンの初期化
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitFooterEvent()

        '開始ログの記録
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2} " _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START))

        'メインメニュー
        Dim mainMenuButton As CommonMasterFooterButton = _
        DirectCast(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.MainMenu)
        AddHandler mainMenuButton.Click, AddressOf mainMenuButton_Click
        mainMenuButton.OnClientClick = "return FooterButtonControl();"

        '連絡先
        Dim telephoneBookButton As CommonMasterFooterButton = _
        DirectCast(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.Contact)
        telephoneBookButton.OnClientClick = "return schedule.appExecute.executeCont();"

        'SMBボタンの設定
        Dim smbButton As CommonMasterFooterButton = _
        DirectCast(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.SMB)
        AddHandler smbButton.Click, AddressOf SMBButton_Click
        smbButton.OnClientClick = "return FooterButtonControl();"

        '商品訴求
        Dim goodsSolicitationContentsButton As CommonMasterFooterButton = _
        DirectCast(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.GoodsSolicitationContents)
        AddHandler goodsSolicitationContentsButton.Click, AddressOf goodsSolicitationContentsButton_Click
        goodsSolicitationContentsButton.OnClientClick = "return FooterButtonControl();"

        'キャンペーン
        Dim campaignButton As CommonMasterFooterButton = _
        DirectCast(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.Campaign)
        AddHandler campaignButton.Click, AddressOf campaignButton_Click
        campaignButton.OnClientClick = "return FooterButtonControl();"

        '顧客詳細ボタンの設定(ヘッダー顧客検索機能へフォーカス)
        Dim customerButton As CommonMasterFooterButton = _
        DirectCast(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.CustomerDetail)
        customerButton.OnClientClick = "FooterButtonclick(" & FooterMenuCategory.CustomerDetail & ");"

        'R/O作成
        Dim roMakeButton As CommonMasterFooterButton = _
        DirectCast(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.RepairOrderList)
        AddHandler roMakeButton.Click, AddressOf roMakeButton_Click
        roMakeButton.OnClientClick = "return FooterButtonControl();"

        '来店管理
        Dim reserveManagementButton As CommonMasterFooterButton = _
        DirectCast(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.ReserveManagement)
        AddHandler reserveManagementButton.Click, AddressOf reserveManagementButton_Click
        reserveManagementButton.OnClientClick = "return FooterButtonControl();"

        '終了ログの記録
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))

    End Sub

    ''' <summary>
    ''' メインメニューボタンを押した時の処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Private Sub mainMenuButton_Click(ByVal sender As Object, ByVal e As CommonMasterFooterButtonClickEventArgs)

        '開始ログの記録
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2} " _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START))

        Dim strMainMenuId As String
        Dim staffInfo As StaffContext = StaffContext.Current

        '権限により、別々の画面へ遷移する
        If staffInfo.OpeCD = iCROP.BizLogic.Operation.SM Then
            'Service Manager : 全体管理
            strMainMenuId = APPLICATIONID_GENERALMANAGER
        ElseIf staffInfo.OpeCD = iCROP.BizLogic.Operation.SA Then
            'Service Advisor : SAメイン
            strMainMenuId = SA_MAINMENUID
        Else
            strMainMenuId = APPLICATIONID_NOASSIGNMENTLIST
        End If

        '終了ログの記録
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}, MainMenuId:[{3}]" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END _
                   , strMainMenuId))

        ' メイン画面に遷移する
        Me.RedirectNextScreen(strMainMenuId)

    End Sub

    ''' <summary>
    ''' フッター「R/Oボタン」クリック時の処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks>
    ''' 　R/O一覧画面に遷移します。
    ''' </remarks>
    Private Sub roMakeButton_Click(ByVal sender As Object, ByVal e As CommonMasterFooterButtonClickEventArgs)

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                 , "{0}.{1} {2}" _
                                 , Me.GetType.ToString _
                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                 , LOG_START))


        Using Biz As New SC3250103BusinessLogic

            '基幹コードへ変換処理
            Dim staffInfo As StaffContext = StaffContext.Current
            Dim rowDmsCodeMap As SC3250103DataSet.DmsCodeMapRow = Biz.ChangeDmsCode(staffInfo)

            '基幹販売店コードチェック
            If String.Empty.Equals(rowDmsCodeMap.CODE1) Then
                '値無し
                rowDmsCodeMap.CODE1 = Params.DealerCode
            End If

            '基幹店舗コードチェック
            If String.Empty.Equals(rowDmsCodeMap.CODE2) Then
                '値無し
                rowDmsCodeMap.CODE2 = Params.BranchCode
            End If

            '基幹アカウントチェック
            If String.Empty.Equals(rowDmsCodeMap.ACCOUNT) Then
                '値無し
                rowDmsCodeMap.ACCOUNT = Params.LoginUserID
            End If

            '販売店コード
            Me.SetValue(ScreenPos.Next, SessionParam01, rowDmsCodeMap.CODE1)
            '店舗コード
            Me.SetValue(ScreenPos.Next, SessionParam02, rowDmsCodeMap.CODE2)
            'アカウント
            Me.SetValue(ScreenPos.Next, SessionParam03, rowDmsCodeMap.ACCOUNT)
            '来店者実績連番
            Me.SetValue(ScreenPos.Next, SessionParam04, Params.SAChipID)
            'DMS予約ID
            Me.SetValue(ScreenPos.Next, SessionParam05, Params.BASREZID)
            'RO
            Me.SetValue(ScreenPos.Next, SessionParam06, Params.R_O)
            'RO_JOB_SEQ
            Me.SetValue(ScreenPos.Next, SessionParam07, Params.SEQ_NO)
            'VIN
            Me.SetValue(ScreenPos.Next, SessionParam08, Params.VIN_NO)
            'ViewMode
            Me.SetValue(ScreenPos.Next, SessionParam09, ReadMode)
            'DISP_NUM
            Me.SetValue(ScreenPos.Next, SessionDispNum, SESSIONVALUE_RO_LIST)

        End Using

        '基幹画面連携用フレーム呼出処理
        Me.ScreenTransition()

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))

    End Sub

    ''' <summary>
    ''' SMBボタンを押した時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
    ''' </history>
    Private Sub SMBButton_Click(ByVal sender As Object, ByVal e As CommonMasterFooterButtonClickEventArgs)

        '開始ログの記録
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2} " _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START))

        '工程管理画面に遷移する
        Me.RedirectNextScreen(PROCESS_CONTROL_PAGE)

        '終了ログの記録
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))

    End Sub

    ''' <summary>
    ''' 商品訴求ボタンを押した時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Sub goodsSolicitationContentsButton_Click(ByVal sender As Object, ByVal e As CommonMasterFooterButtonClickEventArgs)

        '開始ログの記録
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2} " _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START))

        '次画面遷移パラメータ設定

        '販売店コード
        Me.SetValue(ScreenPos.Next, SessionDealerCode, Params.DealerCode)
        '店舗コード
        Me.SetValue(ScreenPos.Next, SessionBranchCode, Params.BranchCode)
        'アカウント
        Me.SetValue(ScreenPos.Next, SessionLoginUserID, Params.LoginUserID)
        '来店者実績連番
        Me.SetValue(ScreenPos.Next, SessionSAChipID, Params.SAChipID)
        'DMS予約ID
        Me.SetValue(ScreenPos.Next, SessionBASREZID, Params.BASREZID)
        'RO
        Me.SetValue(ScreenPos.Next, SessionRO, Params.R_O)
        'RO_JOB_SEQ           
        Me.SetValue(ScreenPos.Next, SessionSEQNO, Params.SEQ_NO)
        'VIN
        Me.SetValue(ScreenPos.Next, SessionVINNO, Params.VIN_NO)
        'ViewMode
        Me.SetValue(ScreenPos.Next, SessionViewMode, Params.ViewMode)

        '商品訴求コンテンツ画面遷移
        Me.RedirectNextScreen(PGMID_GOOD_SOLICITATION_CONTENTS)

        '終了ログの記録
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))


    End Sub

    ''' <summary>
    ''' キャンペーンボタンタップイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub campaignButton_Click(ByVal sender As Object, ByVal e As CommonMasterFooterButtonClickEventArgs)

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                 , "{0}.{1} {2}" _
                                 , Me.GetType.ToString _
                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                 , LOG_START))

        Using Biz As New SC3250103BusinessLogic

            '基幹コードへ変換処理
            Dim staffInfo As StaffContext = StaffContext.Current
            Dim rowDmsCodeMap As SC3250103DataSet.DmsCodeMapRow = Biz.ChangeDmsCode(staffInfo)

            '基幹販売店コードチェック
            If String.Empty.Equals(rowDmsCodeMap.CODE1) Then
                '値無し
                rowDmsCodeMap.CODE1 = Params.DealerCode
            End If

            '基幹店舗コードチェック
            If String.Empty.Equals(rowDmsCodeMap.CODE2) Then
                '値無し
                rowDmsCodeMap.CODE2 = Params.BranchCode
            End If

            '販売店コード
            Me.SetValue(ScreenPos.Next, SessionParam01, rowDmsCodeMap.CODE1)
            '店舗コード
            Me.SetValue(ScreenPos.Next, SessionParam02, rowDmsCodeMap.CODE2)
            'アカウント
            Me.SetValue(ScreenPos.Next, SessionParam03, Params.LoginUserID)
            '来店者実績連番
            Me.SetValue(ScreenPos.Next, SessionParam04, Params.SAChipID)
            'DMS予約ID
            Me.SetValue(ScreenPos.Next, SessionParam05, Params.BASREZID)
            'RO
            Me.SetValue(ScreenPos.Next, SessionParam06, Params.R_O)
            'RO_JOB_SEQ
            Me.SetValue(ScreenPos.Next, SessionParam07, Params.SEQ_NO)
            'VIN
            Me.SetValue(ScreenPos.Next, SessionParam08, Params.VIN_NO)
            'ViewMode
            If String.IsNullOrWhiteSpace(Params.VIN_NO) OrElse String.IsNullOrWhiteSpace(Params.SAChipID) Then
                'VIN又は来店者実績連番が空白ならReadMode
                Me.SetValue(ScreenPos.Next, SessionParam09, ReadMode)
            Else
                Me.SetValue(ScreenPos.Next, SessionParam09, EditMode)
            End If
            'DISP_NUM
            Me.SetValue(ScreenPos.Next, SessionDispNum, APPLICATIONID_CAMPAIGN)

        End Using

        '基幹画面連携用フレーム呼出処理
        Me.ScreenTransition()

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))

    End Sub

    ''' <summary>
    ''' 予約管理ボタンタップイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' 
    Private Sub reserveManagementButton_Click(ByVal sender As Object, ByVal e As CommonMasterFooterButtonClickEventArgs)

        '開始ログの記録
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2} " _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START))

        '来店管理画面に遷移する
        Me.RedirectNextScreen(APPLICATIONID_VSTMANAGER)

        '終了ログの記録
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))

    End Sub

    ''' <summary>
    ''' 基幹画面連携用フレーム(他システム連携画面)呼出処理
    ''' </summary>
    ''' <history>
    ''' </history>
    Private Sub ScreenTransition()

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2} " _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START))

        '基幹画面連携用フレーム呼出(SC3010501)
        Me.RedirectNextScreen(APPLICATIONID_FRAMEID)

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))

    End Sub

#End Region

#Region "ページ関連処理"

    ''' <summary>
    ''' 初期処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitProc()

        '開始ログの記録
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2} " _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START))

        'パラメータを取得する
        Me.GetParams()

        'フッター初期化
        Me.InitFooterEvent()

        '終了ログの記録
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))

    End Sub

    ''' <summary>
    ''' パラメータを取得する
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetParams()

        '開始ログの記録
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2} " _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START))


        '販売店コード、店舗コード、ログインIDは基盤から取得するためコメント化
        ''販売店コード(DealerCode)
        'If Me.ContainsKey(ScreenPos.Current, SessionDealerCode) Then
        '    Params.DealerCode = DirectCast(GetValue(ScreenPos.Current, SessionDealerCode, False), String)
        'End If

        ''店舗コード(BranchCode)
        'If Me.ContainsKey(ScreenPos.Current, SessionBranchCode) Then
        '    Params.BranchCode = DirectCast(GetValue(ScreenPos.Current, SessionBranchCode, False), String)
        'End If

        ''ログインID(LoginUserID)
        'If Me.ContainsKey(ScreenPos.Current, SessionLoginUserID) Then
        '    Params.LoginUserID = DirectCast(GetValue(ScreenPos.Current, SessionLoginUserID, False), String)
        'End If

        '来店実績連番(SAChipID)
        If Me.ContainsKey(ScreenPos.Current, SessionSAChipID) Then
            Params.SAChipID = DirectCast(GetValue(ScreenPos.Current, SessionSAChipID, False), String)
        End If

        'DMS予約ID（BASREZID）
        If Me.ContainsKey(ScreenPos.Current, SessionBASREZID) Then
            Params.BASREZID = DirectCast(GetValue(ScreenPos.Current, SessionBASREZID, False), String)
        End If

        'RO番号（R_O）
        If Me.ContainsKey(ScreenPos.Current, SessionRO) Then
            Params.R_O = DirectCast(GetValue(ScreenPos.Current, SessionRO, False), String)
        End If

        'RO作業連番（SEQ_NO）
        If Me.ContainsKey(ScreenPos.Current, SessionSEQNO) Then
            Params.SEQ_NO = DirectCast(GetValue(ScreenPos.Current, SessionSEQNO, False), String)
        End If

        'VIN（VIN_NO）
        If Me.ContainsKey(ScreenPos.Current, SessionVINNO) Then
            Params.VIN_NO = DirectCast(GetValue(ScreenPos.Current, SessionVINNO, False), String)
        End If

        '編集モード（ViewMode）
        If Me.ContainsKey(ScreenPos.Current, SessionViewMode) Then
            Params.ViewMode = DirectCast(GetValue(ScreenPos.Current, SessionViewMode, False), String)
        End If

        '商品訴求用部位コード（ReqPartCD）
        If Me.ContainsKey(ScreenPos.Current, SessionReqPartCD) Then
            Params.ReqPartCD = DirectCast(GetValue(ScreenPos.Current, SessionReqPartCD, False), String)
        End If

        '点検項目コード（InspecItemCD）
        If Me.ContainsKey(ScreenPos.Current, SessionInspecItemCD) Then
            Params.InspecItemCD = DirectCast(GetValue(ScreenPos.Current, SessionInspecItemCD, False), String)
        End If

        '販売店コード、店舗コード、店舗コードは基盤から情報を取得する

        Dim staffInfo As StaffContext = StaffContext.Current

        If String.IsNullOrWhiteSpace(Params.DealerCode) Then
            Params.DealerCode = staffInfo.DlrCD
        End If
        If String.IsNullOrWhiteSpace(Params.BranchCode) Then
            Params.BranchCode = staffInfo.BrnCD
        End If
        If String.IsNullOrWhiteSpace(Params.LoginUserID) Then
            Params.LoginUserID = staffInfo.Account
        End If

        'ユーザーIDに@が無ければ、「スタッフ識別文字列 + "@" + 販売店コード」の形にする
        If Not Params.LoginUserID.Contains("@") Then
            Params.LoginUserID = String.Format("{0}@{1}", Params.LoginUserID, Params.DealerCode)
        End If


        '***取得したパラメータ情報をログに記録
        Logger.Error(String.Format("Params:DealerCode:[{0}], BranchCode:[{1}], LoginUserID:[{2}], SAChipID:[{3}], BASREZID:[{4}], R_O:[{5}], SEQ_NO:[{6}], VIN_NO:[{7}], ViewMode:[{8}], ReqPartCD:[{9}], InspecItemCD:[{10}]", _
                                  Params.DealerCode, _
                                  Params.BranchCode, _
                                  Params.LoginUserID, _
                                  Params.SAChipID, _
                                  Params.BASREZID, _
                                  Params.R_O, _
                                  Params.SEQ_NO, _
                                  Params.VIN_NO, _
                                  Params.ViewMode, _
                                  Params.ReqPartCD, _
                                  Params.InspecItemCD))

        '終了ログの記録
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))

    End Sub

    ''' <summary>
    ''' 表示関係の初期設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitViewProc()

        '開始ログの記録
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2} " _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START))

        '***部品説明エリアを表示
        ContentBox1.Attributes.Add("class", String.Format("{0} {1}{2}", CONTENT_CLASS_NAME, CONTENT_SHOW, "1Before"))

        '***部品説明エリア以外のエリアは非表示（タイトルのみ表示）
        Dim ContentsBoxMinimize As String = String.Format("{0} {1}", CONTENT_CLASS_NAME, CONTENT_MINIMIZE)
        ContentBox2.Attributes.Add("class", ContentsBoxMinimize)
        ContentBox3.Attributes.Add("class", ContentsBoxMinimize)
        ContentBox4.Attributes.Add("class", ContentsBoxMinimize)
        ContentBox5.Attributes.Add("class", ContentsBoxMinimize)

        'コンテンツ表示フラグの初期化
        Dim DisplayFlag() As Integer = {Content.Show _
                                       , Content.Show _
                                       , Content.Show _
                                       , Content.Show _
                                       , Content.Show}

        '***VINが無い場合、交換部品情報（エリア３）と残量グラフエリア（エリア４）をタイトルも含め非表示にする
        If String.IsNullOrWhiteSpace(Params.VIN_NO) Then
            'ContentBox3.Style.Add("display", "none")
            DisplayFlag(Content.Box3) = Content.None
            'iFrame3.Attributes.Remove("src")

            'ContentBox4.Style.Add("display", "none")
            DisplayFlag(Content.Box4) = Content.None
            'iFrame4.Attributes.Remove("src")
        End If

        Using Biz As New SC3250103BusinessLogic
            '***各コンテンツ表示に必要な情報を取得する
            Dim dtInspecItemInfo As New SC3250103DataSet.InspecItemInfoDataTable
            dtInspecItemInfo = Biz.GetInspecItemInfoData(Params.InspecItemCD)

            If dtInspecItemInfo IsNot Nothing AndAlso 0 < dtInspecItemInfo.Count Then

                Dim InspecItemInfo As SC3250103DataSet.InspecItemInfoRow = dtInspecItemInfo(0)

                '***大タイトル部分の表示
                'サブ点検項目のセット（中央部分）
                InspecItemName2.InnerHtml = InspecItemInfo.SUB_INSPEC_ITEM_NAME

                '***コンテンツエリア１．部品説明
                '表示する部品説明ページのURLを取得
                'iFrame1.Attributes.Remove("src")
                If Not String.IsNullOrWhiteSpace(InspecItemInfo.PARTS_AREA_URL) Then
                    Dim PartsDetailURL As String = String.Empty
                    '「http:」より前はカットして、表示するURLを取得
                    If InspecItemInfo.PARTS_AREA_URL.Contains("http:") Then
                        PartsDetailURL = InspecItemInfo.PARTS_AREA_URL.Substring(InspecItemInfo.PARTS_AREA_URL.IndexOf("http:"))
                    Else
                        PartsDetailURL = InspecItemInfo.PARTS_AREA_URL
                    End If

                    'URLにページIDがあった場合、ページIDを取り出す
                    If PartsDetailURL.Contains("#") Then
                        Dim SplitURL() As String = PartsDetailURL.Split("#"c)

                        ''ページIDを取り除いたURLをセット
                        'PartsDetailURL = SplitURL(0)

                        'ページIDを隠しフィールドに入れておく
                        hdnContent1_PageId.Value = SplitURL(1)
                    End If

                    'iFrameにセットする
                    'Logger.Info("★★" & PartsDetailURL)
                    iFrame1.Attributes.Add("src", PartsDetailURL)
                Else
                    'URLが取得できなかった
                    'iFrame1.Attributes.Remove("src")
                    'クルクルを消す
                    ServerProcessIcon1.Style.Add("display", "none")
                End If

                '***コンテンツエリア２．新旧コンテンツ表示
                'コンテンツエリア２に表示するページURLをHiddenフィールドに設定する
                hdnContent2URL.Value = ResolveClientUrl("SC3250104.aspx")

                '表示する点検項目にNewパーツの写真が登録されていなかったら、タイトルを含め非表示にする
                If String.IsNullOrWhiteSpace(InspecItemInfo.NEW_PARTS_FILE_NAME) Then
                    'ContentBox2.Style.Add("display", "none")
                    DisplayFlag(Content.Box2) = Content.None
                    'iFrame2.Attributes.Remove("src")
                End If

                '***コンテンツエリア３．新旧コンテンツ表示
                'コンテンツエリア３に表示するページURLをHiddenフィールドに設定する
                hdnContent3URL.Value = ResolveClientUrl("SC3250105.aspx")

                '***コンテンツエリア４．残量グラフ表示
                'コンテンツエリア４に表示するページURLをHiddenフィールドに設定する
                hdnContent4URL.Value = ResolveClientUrl("SC3250106.aspx")

                '表示する点検項目に対し、
                '　①グループコードが設定されていなかったら、タイトルを含め非表示にする
                '　②グループコードが設定されていても、完成検査結果データなかったら、タイトルを含め非表示にする
                'If Biz.ChkPartsGroupCd(Params.InspecItemCD) = False Then
                If Biz.ChkDisplayGraphArea(Params.DealerCode, Params.BranchCode, Params.VIN_NO, Params.InspecItemCD) = False Then
                    'ContentBox4.Style.Add("display", "none")
                    DisplayFlag(Content.Box4) = Content.None
                    'iFrame4.Attributes.Remove("src")
                End If

                '***コンテンツエリア５．部品カタログの設定
                'タイトルをセットする
                Content5_Title.Text = WebWordUtility.GetWord(6).Replace("{0}", InspecItemInfo.SUB_INSPEC_ITEM_NAME)

                'IFrameにURLをセットする
                If String.IsNullOrWhiteSpace(InspecItemInfo.PRIMARY_INSPEC_ITEM_CD) Then
                    'プライマリ点検項目コードが見つからなかったら、点検項目コードを入れる
                    InspecItemInfo.PRIMARY_INSPEC_ITEM_CD = Params.InspecItemCD
                End If
                'iFrame5.Attributes("src") = Me.CreatePartsCatalogURL(InspecItemInfo.PRIMARY_INSPEC_ITEM_CD)
                'コンテンツエリア５に表示するページURLをHiddenフィールドに設定する
                hdnContent5URL.Value = Me.CreatePartsCatalogURL(InspecItemInfo.PRIMARY_INSPEC_ITEM_CD)

            End If

        End Using

        'コンテンツの表示設定を隠しフィールドに入れる
        '（各コンテンツの表示・非表示処理はJavaScriptにて行う）
        hdnDisplayFlag.Value = String.Format("{0},{1},{2},{3},{4}" _
                                            , DisplayFlag(Content.Box1) _
                                            , DisplayFlag(Content.Box2) _
                                            , DisplayFlag(Content.Box3) _
                                            , DisplayFlag(Content.Box4) _
                                            , DisplayFlag(Content.Box5))

        '終了ログの記録
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))

    End Sub

    ''' <summary>
    ''' コンテンツエリア５（部品カタログ）画面のURLを作成する
    ''' </summary>
    ''' <param name="strPRIMARY_INSPEC_ITEM_CD">点検項目コード</param>
    ''' <remarks></remarks>
    Private Function CreatePartsCatalogURL(ByVal strPRIMARY_INSPEC_ITEM_CD As String) As String

        '開始ログの記録
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2} " _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START))

        Using Biz As New SC3250103BusinessLogic

            '基幹コードへ変換処理
            Dim staffInfo As StaffContext = StaffContext.Current
            Dim rowDmsCodeMap As SC3250103DataSet.DmsCodeMapRow = Biz.ChangeDmsCode(staffInfo)

            '基幹販売店コードチェック
            If String.Empty.Equals(rowDmsCodeMap.CODE1) Then
                '値無し
                rowDmsCodeMap.CODE1 = Params.DealerCode
            End If

            '基幹店舗コードチェック
            If String.Empty.Equals(rowDmsCodeMap.CODE2) Then
                '値無し
                rowDmsCodeMap.CODE2 = Params.BranchCode
            End If

            'ログインユーザーの「＠」以降を削除する
            Dim UserName As String
            If Params.LoginUserID.Contains("@") Then
                'ユーザーに「＠」が含まれている
                UserName = Params.LoginUserID.Substring(0, Params.LoginUserID.IndexOf("@"))
            Else
                UserName = Params.LoginUserID
            End If

            'パラメータ作成
            Dim parameterList As New List(Of String)
            parameterList.Add(rowDmsCodeMap.CODE1)
            parameterList.Add(rowDmsCodeMap.CODE2)
            parameterList.Add(UserName)
            parameterList.Add(Params.SAChipID)
            parameterList.Add(Params.BASREZID)
            parameterList.Add(Params.R_O)
            parameterList.Add(Params.SEQ_NO)
            parameterList.Add(Params.VIN_NO)
            parameterList.Add(Params.ViewMode)
            parameterList.Add(strPRIMARY_INSPEC_ITEM_CD)

            'TBL_SYSTEMENVからドメイン名を取得
            Dim systemEnv As New SystemEnvSetting
            Dim systemEnvParam As String = String.Empty
            Dim drSystemEnvSetting As SYSTEMENVSETTINGRow = _
                systemEnv.GetSystemEnvSetting(OTHER_LINKAGE_DOMAIN)

            '取得できた場合のみ設定する
            If Not (IsNothing(drSystemEnvSetting)) Then
                systemEnvParam = drSystemEnvSetting.PARAMVALUE
            End If

            '表示番号とパラメータとドメインからIFrameに表示するURLを作成
            Dim url As String = Biz.CreateURL(18, parameterList, systemEnvParam)

            '終了ログの記録
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} {2}, Return[{3}]" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , LOG_END _
                       , url))

            Return url

        End Using

    End Function

#End Region

End Class
