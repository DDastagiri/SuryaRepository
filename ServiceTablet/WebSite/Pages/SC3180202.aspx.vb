 '------------------------------------------------------------------------------
'SC3180202.aspx.vb
'------------------------------------------------------------------------------
'機能：チェックシートプレビュー
'補足：
'作成： 2013/02/01 工藤
'更新： 2019/12/10 NCN 吉川（FS）次世代サービス業務における車両型式別点検の検証
'------------------------------------------------------------------------------
Option Explicit On
Option Strict On
Imports System
Imports System.Data
Imports System.Globalization
Imports System.Web.Script.Serialization
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.ServerCheck.CheckResult.BizLogic.SC3180202
Imports Toyota.eCRB.ServerCheck.CheckResult.DataAccess.SC3180202.SC3180202DataSet
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.DataAccess.ServiceCommonClassDataSet
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.DataAccess
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.BizLogic

''' <summary>
''' チェックシートプレビュー
''' </summary>
''' <remarks></remarks>
Partial Class Pages_SC3180202
    Inherits BasePage

#Region "変数"
    Private staffInfo As StaffContext
    Private params As New Parameters
    Private dateDispFlg As Boolean
    Private isExistActive As Boolean

    ''' <summary>
    ''' Getパラメーター格納用クラス
    ''' </summary>
    ''' <remarks></remarks>
    Private Class Parameters
        ''' <summary>販売店コード</summary>
        Public dealerCode As String
        ''' <summary>店舗コード</summary>
        Public branchCode As String
        ''' <summary>ログインユーザID</summary>
        Public loginUserID As String
        ''' <summary>SAChipID</summary>
        Public saChipID As String
        ''' <summary>BASREZID</summary>
        Public basrezID As String
        ''' <summary>R/O</summary>
        Public R_O As String
        ''' <summary>SEQ_NO</summary>
        Public seqNo As String
        ''' <summary>VIN_NO</summary>
        Public vinNo As String
        ''' <summary>ViewMode 1=Readonly / 0=Edit</summary>
        Public viewMode As String
        ''' <summary> Format0=R/O Preview(S-SA-16) / 1=Service history(S-SA-16-HIST)</summary>
        Public format As String
        ''' <summary> SVCIN_NUM</summary>
        Public svcInNum As String
        ''' <summary>SVCIN_DealerCode</summary>
        Public svcInDealerCode As String
    End Class

#End Region

#Region "定数"
    '画面ID
    Private Const ApplicationId As String = "SC3180202"
    Private Const ProgramIdPrintPreview As String = "SC3180203"
    Private Const ProgramIdInspection As String = "SC3180204"

    '各種アイコン
    Private Const IconNoProblem As String = "Square1"
    Private Const IconNeedInspection As String = "Square2"
    Private Const IconNeedReplace As String = "Square6"
    Private Const IconNeedFixing As String = "Square8"
    Private Const IconNeedCleaning As String = "Square9"
    Private Const IconNeedSwapping As String = "Square10"
    Private Const IconAlreadyReplace As String = "Square7"
    Private Const IconAlreadyFixed As String = "Square3"
    Private Const IconAlreadyCleaning As String = "Square4"
    Private Const IconAlreadySwapping As String = "Square5"
    Private Const IconNocheck As String = "Square12"

    'レイアウトのパス
    Private Const PathCarModel As String = "..\Styles\SC3180202\Templates\"
    Private Const PathCarModelLxs As String = "..\Styles\SC3180202\LexusTemplates\"
    Private Const PathExtension As String = ".txt"
    Private Const PathDefault As String = "_Default"

    '点検結果入力値(Before, After)のフォーマット(999.99値形式)
    Private Const FormatRsltVal As String = "##0.00"

    'サービス入庫テーブル.納車実績日時初期値(年月日)
    Private Const FormatDbDateTime As String = "1900/01/01"

    ''' <summary>
    ''' フッターイベントの置換用文字列
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FooterButtonClick As String = "FooterButtonClick({0});"

    ''' <summary>
    ''' 電話帳ボタンのイベント
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FooterEventTel As String = "return schedule.appExecute.executeCont();"

    ''' <summary>
    ''' 点検結果
    ''' </summary>
    ''' <remarks></remarks>
    Enum InspecResultCD
        Notselected = 0 '未実施
        NoProblem = 1
        NeedInspection = 2
        NeedReplace = 3
        NeedFixing = 4
        NeedCleaning = 5
        NeedSwapping = 6
        AlreadyReplace = 7
        AlreadyFixed = 8
        AlreadyCleaning = 9
        AlreadySwapped = 10
        NoChecked = 11
    End Enum

    ''' <summary>
    ''' 作業ステータス
    ''' </summary>
    ''' <remarks></remarks>
    Enum OperationStatus
        WorkinProgress = 0  '作業途中
        ApprovalPending = 1 '承認待ち
        Approved = 2        '承認済み
        Remand = 3          '差し戻し
        Other = 9           'その他
    End Enum

    ''' <summary>
    ''' 選択状態
    ''' </summary>
    ''' <remarks></remarks>
    Enum SelectFlg
        CheckOff = 0 '無効
        CheckOn = 1  '有効
    End Enum

#Region "文言ID"
    ''' <summary>
    ''' 文言ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Enum WordId
        ''' <summary>タイトル</summary>
        id000 = 0
        ''' <summary>登録No.</summary>
        id001 = 1
        ''' <summary>来店者名</summary>
        id002 = 2
        ''' <summary>修理日</summary>
        id003 = 3
        ''' <summary>走行距離（入店時）</summary>
        id004 = 4
        ''' <summary>R/O　No</summary>
        id005 = 5
        ''' <summary>担当SA</summary>
        id006 = 6
        ''' <summary>納車日</summary>
        id007 = 7
        ''' <summary>問題なし </summary>
        id008 = 8
        ''' <summary>要検査</summary>
        id009 = 9
        ''' <summary>要交換</summary>
        id010 = 10
        ''' <summary>要調整</summary>
        id011 = 11
        ''' <summary>要クリーニング</summary>
        id012 = 12
        ''' <summary>要スワッピング</summary>
        id013 = 13
        ''' <summary>交換済</summary>
        id014 = 14
        ''' <summary>調整済 </summary>
        id015 = 15
        ''' <summary>清掃済</summary>
        id016 = 16
        ''' <summary>スワップ済</summary>
        id017 = 17
        ''' <summary>作業結果とアドバイス</summary>
        id018 = 18
        ''' <summary>編集</summary>
        id019 = 19
        ''' <summary>印刷</summary>
        id020 = 20
        ''' <summary>R/O</summary>
        id021 = 21
    End Enum
#End Region

#Region "画面遷移"

#Region "スクリプト用画面コード"
    ''' <summary>
    ''' フッターコード：メインメニュー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FooterMainMenu As Integer = 100
    ''' <summary>
    ''' フッターコード：TCメイン
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FooterTecnicianMain As Integer = 200
    ''' <summary>
    ''' フッターコード：FMメイン
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FooterFormanMain As Integer = 300
    ''' <summary>
    ''' フッターコード：来店管理
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FooterVisitManament As Integer = 400
    ''' <summary>
    ''' フッターコード：R/Oボタン
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FooterRo As Integer = 500
    ''' <summary>
    ''' フッターコード：連絡先
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FooterTelDirector As Integer = 600
    ''' <summary>
    ''' フッターコード：顧客詳細
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FooterCustomer As Integer = 700
    ''' <summary>
    ''' フッターコード：商品訴求コンテンツ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FooterContents As Integer = 800
    ''' <summary>
    ''' フッターコード：キャンペーン
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FooterCampaing As Integer = 900
    ''' <summary>
    ''' フッターコード：全体管理
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FooterAllManagment As Integer = 1000
    ''' <summary>
    ''' フッターコード：SMB
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FooterSmb As Integer = 1100
    ''' <summary>
    ''' フッターコード：追加作業ボタン
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FooterAddList As Integer = 1200
#End Region

#Region "画面ID"
    ''' <summary>
    ''' メインメニュー(SA)画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ProgramIdMinaMenuSa As String = "SC3140103"
    ''' <summary>
    ''' 全体管理画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ProgramIdAllManagment As String = "SC3220201"
    ''' <summary>
    ''' 工程管理画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ProgramIdProcessControl As String = "SC3240101"
    ''' <summary>
    ''' メインメニュー(TC)画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ProgramIdMainMenuTc As String = "SC3150101"
    ''' <summary>
    ''' メインメニュー(FM)画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ProgramIdMainMenuFm As String = "SC3230101"
    ''' <summary>
    ''' 来店管理画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ProgramIdVisitManagment As String = "SC3100303"
    ''' <summary>
    ''' 未振当一覧画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ProgramIdAssignmentList As String = "SC3100401"
    ''' <summary>
    ''' 商品訴求コンテンツ画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ProgramIdGoodsSolication As String = "SC3250101"
    ''' <summary>
    ''' 他システム連携画面画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ProgramIdOtherLinkage As String = "SC3010501"

#End Region

#Region "セッション関連"
    ''' <summary>
    ''' 編集モードフラグ("0"；編集) 
    ''' </summary>
    Private Const EditMode As String = "0"

    ''' <summary>
    ''' 編集モードフラグ("1"；リードオンリー) 
    ''' </summary>
    Private Const ReadMode As String = "1"

    'R/O Preview(S-SA-16)
    Private Const Preview As String = "0"

    'Service history(S-SA-16-HIST)					
    Private Const ServiceHistory As String = "1"
    ''' <summary>
    ''' セッションキー(表示番号13：R/O参照画面)
    ''' </summary>
    Private Const SessionDataDispNum_OrderOut As String = "13"
    ''' <summary>
    ''' セッションキー(表示番号14：R/O一覧)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionDataDispNum_ROList As String = "14"
    ''' <summary>
    ''' セッションキー(表示番号15：キャンペーン)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionDataDispNum_Campaign As String = "15"
    ''' <summary>
    ''' セッションキー(表示番号22：追加作業一覧)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionDataDispNum_AddList As String = "22"
    ''' <summary>
    '''セッションキー(表示番号25：R/O履歴画面) 
    ''' </summary>
    Private Const SessionDataDispNum_ServiceHistory As String = "25"

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
    ''' セッション名("Format")
    ''' </summary>
    Private Const SessionFormat As String = "Format"

    ''' <summary>
    ''' セッション名("SVCIN_NUM")
    ''' </summary>
    Private Const SessionSVCIN_NUM As String = "SVCIN_NUM"

    ''' <summary>
    ''' セッション名("SVCIN_DealerCode")
    ''' </summary>
    Private Const SessionSVCIN_DealerCode As String = "SVCIN_DealerCode"

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
#End Region

#End Region

#End Region

#Region "プロパティ"
    Private headerScriptData As String
    Private detailScriptData As String
    Private scriptTemplateString As String
    '2014/07/09 タイトルをデザイン固定にするため削除
    'Private detailTitleScriptData As String

    ''' <summary>
    ''' テンプレート文字列
    ''' </summary>
    ''' <value></value>
    ''' <returns>テンプレート文字列</returns>
    ''' <remarks></remarks>
    Public ReadOnly Property TemplateString() As String
        Get
            Return scriptTemplateString
        End Get
    End Property

    ''' <summary>
    ''' ヘッダーデータ
    ''' </summary>
    ''' <returns>ヘッダーデータ</returns>
    ''' <remarks></remarks>
    Public ReadOnly Property HeaderData() As String
        Get
            Return headerScriptData
        End Get
    End Property

    ''' <summary>
    ''' 明細データ
    ''' </summary>
    ''' <returns>明細データ</returns>
    ''' <remarks></remarks>
    Public ReadOnly Property DetailData() As String
        Get
            Return detailScriptData
        End Get
    End Property

    '2014/07/09 タイトルをデザイン固定にするため削除
    ' ''' <summary>
    ' ''' 明細データ
    ' ''' </summary>
    ' ''' <returns>明細データ</returns>
    ' ''' <remarks></remarks>
    'Public ReadOnly Property DetailTitleData() As String
    '    Get
    '        Return detailTitleScriptData
    '    End Get
    'End Property
#End Region

    
#Region "イベント"

    ''' <summary>
    ''' Page_Load
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        staffInfo = StaffContext.Current

        InitProc()

        If Not IsPostBack Then
            ''業務フッター設定
            'ButtonEdit.Text = WebWordUtility.GetWord(ApplicationId, WordId.id019)
            'ButtonPrint.Text = WebWordUtility.GetWord(ApplicationId, WordId.id020)
            'ButtonRO.Text = WebWordUtility.GetWord(ApplicationId, WordId.id021)

            ''権限ごとに使用可能ボタン変更
            'Select Case staffInfo.OpeCD
            '    Case Operation.SVR
            '        ButtonEdit.Visible = False
            '    Case Operation.TEC
            '        ButtonEdit.Visible = False
            '        ButtonRO.Visible = False
            'End Select


            '2019/07/05　TKM要件:型式対応　START　↓↓↓
            '車種ごとのテンプレート取得
            Dim fileName As String = String.Empty

            Using biz As New SC3180202BusinessLogic

                '2017/01/24　ライフサイクル対応追加　START　↓↓↓
                isExistActive = biz.IsExistServiceinActive(params.dealerCode, params.branchCode, params.R_O)
                '2017/01/24　ライフサイクル対応追加　END　↑↑↑

                System.Environment.CurrentDirectory = Server.MapPath("./")

                Dim katashiki_exist As Boolean = biz.GetKatashikiExist(params.R_O, params.dealerCode, params.branchCode)

                If katashiki_exist Then
                    Dim katashiki As String = Trim(biz.GetKatashiki(params.dealerCode, params.branchCode, params.R_O, isExistActive))
                    If Not katashiki = String.Empty Then
                        fileName = ApplicationId & "_" & katashiki
                    End If
                End If

                If fileName = String.Empty Then
                    Dim modelCode As String = Trim(biz.GetModelCode(params.dealerCode, params.branchCode, params.R_O, isExistActive))
                    If Not modelCode = String.Empty Then
                        fileName = ApplicationId & "_" & modelCode
                    End If
                End If

                If fileName = String.Empty Then
                    'モデルが未登録の場合はデフォルトテンプレート
                    fileName = ApplicationId & PathDefault
                End If
                '2019/07/05　TKM要件:型式対応　END　↑↑↑

                'レクサスの場合は専用のテンプレートを取得
                Dim isLxs As Boolean = biz.isLexus()
                Dim templetePath As String = String.Empty

                'テンプレートファイルのパス決定
                If isLxs Then
                    templetePath = PathCarModelLxs
                Else
                    templetePath = PathCarModel
                End If

                'テンプレート取得
                scriptTemplateString = biz.GetTemplateFile(System.IO.Path.GetFullPath(templetePath & fileName & PathExtension)).Trim

                'テンプレートが取れないときはデフォルトを再取得
                If scriptTemplateString = String.Empty Then
                    fileName = ApplicationId & PathDefault
                    scriptTemplateString = biz.GetTemplateFile(System.IO.Path.GetFullPath(templetePath & fileName & PathExtension)).Trim
                End If

            End Using

            ''基盤フッター設定
            'Me.InitFooterButton(staffInfo)

            'ヘッダー情報設定
            InitHeader()

            '明細情報設定
            InitDetail()
        End If

        '基盤フッター設定
        Me.InitFooterButton(staffInfo)

        '業務フッター設定
        'ButtonEdit.Text = WebWordUtility.GetWord(ApplicationId, WordId.id019)
        'ButtonPrint.Text = WebWordUtility.GetWord(ApplicationId, WordId.id020)
        'ButtonRO.Text = WebWordUtility.GetWord(ApplicationId, WordId.id021)
        divEdit.InnerHtml = WebWordUtility.GetWord(ApplicationId, WordId.id019)
        divPrint.InnerHtml = WebWordUtility.GetWord(ApplicationId, WordId.id020)
        divRO.InnerHtml = WebWordUtility.GetWord(ApplicationId, WordId.id021)
        'ButtonPrint.Attributes.Add("onclick", String.Format("ShowUrlSchemePopup('{0}');return false;", "icrop:iurl:16::8::984::740::-1::http://172.16.101.211:8025/i-CROP_Service/Pages/SC3180203.aspx"))
        'Logger.Error(Request.Url.ToString.Replace("SC3180202.aspx", "SC3180203.aspx"))
        ''ButtonPrint.Attributes.Add("onclick", String.Format("ShowUrlSchemePopup('{0}');return false;", "icrop:iurl:16::8::984::740::-1::" & Request.Url.ToString.Replace("SC3180202.aspx", "SC3180203.aspx")))
        ButtonPrint.Attributes.Add("onclick", String.Format("ShowUrlSchemePopup('{0}');return false;", "icrop:iurl:16::8::984::740::-1::SC3180203.aspx"))
        '権限ごとに使用可能ボタン変更
        Select Case staffInfo.OpeCD
            Case Operation.SVR
                ButtonEdit.Visible = False
                divEdit.Visible = False
            Case Operation.TEC
                ButtonEdit.Visible = False
                ButtonRO.Visible = False
                divEdit.Visible = False
                divRO.Visible = False
        End Select

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' パラメータ設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitProc()
        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        staffInfo = StaffContext.Current

        params.dealerCode = String.Empty
        params.branchCode = String.Empty
        params.loginUserID = String.Empty
        params.saChipID = String.Empty
        params.basrezID = String.Empty
        params.R_O = String.Empty
        params.seqNo = String.Empty
        params.vinNo = String.Empty
        params.viewMode = String.Empty
        params.format = Preview
        params.svcInNum = String.Empty
        params.svcInDealerCode = String.Empty

        '販売店コード
        If Me.ContainsKey(ScreenPos.Current, SessionDealerCode) = True Then
            params.dealerCode = DirectCast(GetValue(ScreenPos.Current, SessionDealerCode, False), String)
        End If

        '店舗コード
        If Me.ContainsKey(ScreenPos.Current, SessionBranchCode) = True Then
            params.branchCode = DirectCast(GetValue(ScreenPos.Current, SessionBranchCode, False), String)
        End If

        '2014/07/28　DMS→ICROP変換処理追加　START　↓↓↓
        'ログインID(account)
        If Me.ContainsKey(ScreenPos.Current, SessionLoginUserID) Then
            params.loginUserID = DirectCast(GetValue(ScreenPos.Current, SessionLoginUserID, False), String)
        End If

        '2019/07/05　TKM要件:型式対応　START　↓↓↓
        Using biz As New SC3180202BusinessLogic
            '2019/07/05　TKM要件:型式対応　END　↑↑↑

            'DMS→ICROP変換処理
            biz.GetDmsToIcropCode(params.dealerCode, params.branchCode)

        End Using
        '2014/07/28　DMS→ICROP変換処理追加　END　　↑↑↑

        'RO
        If Me.ContainsKey(ScreenPos.Current, SessionRO) = True Then
            params.R_O = DirectCast(GetValue(ScreenPos.Current, SessionRO, False), String)
        End If

        'VIN
        If Me.ContainsKey(ScreenPos.Current, SessionVINNO) = True Then
            params.vinNo = DirectCast(GetValue(ScreenPos.Current, SessionVINNO, False), String)
        End If

        'ViewMode
        If Me.ContainsKey(ScreenPos.Current, SessionViewMode) = True Then
            params.viewMode = DirectCast(GetValue(ScreenPos.Current, SessionViewMode, False), String)
        End If

        '来店者実績連番
        If Me.ContainsKey(ScreenPos.Current, SessionSAChipID) = True Then
            params.saChipID = GetValue(ScreenPos.Current, SessionSAChipID, False).ToString
        End If

        'DMS予約ID
        If Me.ContainsKey(ScreenPos.Current, SessionBASREZID) = True Then
            params.basrezID = DirectCast(GetValue(ScreenPos.Current, SessionBASREZID, False), String)
        End If

        'RO_JOB_SEQ(親のRO_JOB_SEQ = 0)
        If Me.ContainsKey(ScreenPos.Current, SessionSEQNO) = True Then
            params.seqNo = DirectCast(GetValue(ScreenPos.Current, SessionSEQNO, False), String)
        End If

        'Format 
        If Me.ContainsKey(ScreenPos.Current, SessionFormat) = True Then
            params.format = DirectCast(GetValue(ScreenPos.Current, SessionFormat, False), String)
        End If

        'SVCIN_NUM 
        If Me.ContainsKey(ScreenPos.Current, SessionSVCIN_NUM) = True Then
            params.svcInNum = DirectCast(GetValue(ScreenPos.Current, SessionSVCIN_NUM, False), String)
        End If

        'SVCIN_DealerCode 
        If Me.ContainsKey(ScreenPos.Current, SessionSVCIN_DealerCode) = True Then
            params.svcInDealerCode = DirectCast(GetValue(ScreenPos.Current, SessionSVCIN_DealerCode, False), String)
        End If

        If params.dealerCode.Trim.Length = 0 Then
            params.dealerCode = staffInfo.DlrCD         '販売店コード
        End If

        If params.branchCode.Trim.Length = 0 Then
            params.branchCode = staffInfo.BrnCD         '店舗コード
        End If

        If params.loginUserID.Trim.Length = 0 Then
            params.loginUserID = staffInfo.Account      'ユーザーID
        End If

        'ユーザーIDに@が無ければ、「スタッフ識別文字列 + "@" + 販売店コード」の形にする
        If Not params.loginUserID.Contains("@") Then
            params.loginUserID = String.Format("{0}@{1}", params.loginUserID, staffInfo.DlrCD)
        End If

        If params.format.Trim.Length = 0 Then
            params.format = Preview
        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture, "SessionDealerCode={0} ", params.dealerCode))
        Logger.Info(String.Format(CultureInfo.CurrentCulture, "SessionBranchCode={0} ", params.branchCode))
        Logger.Info(String.Format(CultureInfo.CurrentCulture, "SessionloginUserID={0} ", params.loginUserID))
        Logger.Info(String.Format(CultureInfo.CurrentCulture, "SessionRO={0} ", params.R_O))
        Logger.Info(String.Format(CultureInfo.CurrentCulture, "SessionVINNO={0} ", params.vinNo))
        Logger.Info(String.Format(CultureInfo.CurrentCulture, "SessionViewMode={0} ", params.viewMode))
        Logger.Info(String.Format(CultureInfo.CurrentCulture, "SessionSAChipID={0} ", params.saChipID))
        Logger.Info(String.Format(CultureInfo.CurrentCulture, "SessionBASREZID={0} ", params.basrezID))
        Logger.Info(String.Format(CultureInfo.CurrentCulture, "SessionSEQNO={0} ", params.seqNo))
        Logger.Info(String.Format(CultureInfo.CurrentCulture, "SessionFormat={0} ", params.format))
        Logger.Info(String.Format(CultureInfo.CurrentCulture, "SessionSVCIN_NUM={0} ", params.svcInNum))
        Logger.Info(String.Format(CultureInfo.CurrentCulture, "SessionSVCIN_DealerCode={0} ", params.svcInDealerCode))

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

#Region "ボタン"
    ''' <summary>
    ''' ButtonPrint_Click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ButtonPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonPrint.Click
        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '販売店コード
        Me.SetValue(ScreenPos.Next, SessionDealerCode, params.dealerCode)
        '店舗コード
        Me.SetValue(ScreenPos.Next, SessionBranchCode, params.branchCode)
        'アカウント
        Me.SetValue(ScreenPos.Next, SessionLoginUserID, params.loginUserID)
        '来店者実績連番
        Me.SetValue(ScreenPos.Next, SessionSAChipID, params.saChipID)
        'DMS予約ID
        Me.SetValue(ScreenPos.Next, SessionBASREZID, params.basrezID)
        'RO
        Me.SetValue(ScreenPos.Next, SessionRO, params.R_O)
        'RO_JOB_SEQ(親のRO_JOB_SEQ = 0)            
        Me.SetValue(ScreenPos.Next, SessionSEQNO, params.seqNo)
        'VIN
        Me.SetValue(ScreenPos.Next, SessionVINNO, params.vinNo)
        'ViewMode
        Me.SetValue(ScreenPos.Next, SessionViewMode, params.viewMode)
        'Format
        Me.SetValue(ScreenPos.Next, SessionFormat, params.format)
        'SVCIN_NUM
        Me.SetValue(ScreenPos.Next, SessionSVCIN_NUM, params.svcInNum)
        'SVCIN_DealerCode
        Me.SetValue(ScreenPos.Next, SessionSVCIN_DealerCode, params.svcInDealerCode)

        'Me.RedirectNextScreen(ProgramIdPrintPreview)
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "PrintURL", String.Format("ShowUrlSchemePopup('{0}');", "../Pages/SC3180203.aspx"), True)

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' ButtonEdit_Click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ButtonEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonEdit.Click
        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                 , "DealerCode={0}&BranchCode={1}&R_O={2}&VIN_NO={3}&ViewMode={4}" _
                                 , params.dealerCode _
                                 , params.branchCode _
                                 , params.R_O _
                                 , params.vinNo _
                                 , params.viewMode))

        Me.SetValue(ScreenPos.Next, SessionDealerCode, params.dealerCode)
        Me.SetValue(ScreenPos.Next, SessionBranchCode, params.branchCode)
        Me.SetValue(ScreenPos.Next, SessionRO, params.R_O)
        Me.SetValue(ScreenPos.Next, SessionSAChipID, params.saChipID)
        Me.SetValue(ScreenPos.Next, SessionVINNO, params.vinNo)
        Me.SetValue(ScreenPos.Next, SessionViewMode, params.viewMode)
        Me.RedirectNextScreen(ProgramIdInspection)

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' ButtonRO_Click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ButtonRO_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonRO.Click
        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'R/O参照画面遷移処理
        Me.RedirectOrderDisp()

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' R/O参照画面遷移処理(パラメータ設定)
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub RedirectOrderDisp()

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START SC3180202 " _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '次画面遷移パラメータ設定
        Dim staffInfo As StaffContext = StaffContext.Current

        '2019/07/05　TKM要件:型式対応　START　↓↓↓
        Using bis As New SC3180202BusinessLogic
            '2019/07/05　TKM要件:型式対応　END　↑↑↑

            '基幹コードへ変換処理
            Dim rowDmsCodeMap As ServiceCommonClassDataSet.DmsCodeMapRow = bis.ChangeDmsCode(staffInfo)

            '基幹販売店コードチェック
            If String.IsNullOrEmpty(rowDmsCodeMap.CODE1) Then
                '値無し

                'エラーログ
                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                               , "{0}.{1} Err:rowDmsCodeMap.CODE1=NOTHING" _
                               , Me.GetType.ToString _
                               , System.Reflection.MethodBase.GetCurrentMethod.Name))

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
            Me.SetValue(ScreenPos.Next, SessionParam04, params.saChipID)
            'DMS予約ID
            Me.SetValue(ScreenPos.Next, SessionParam05, params.basrezID)
            'RO
            Me.SetValue(ScreenPos.Next, SessionParam06, params.R_O)
            'RO_JOB_SEQ
            Me.SetValue(ScreenPos.Next, SessionParam07, params.seqNo)
            'VIN
            Me.SetValue(ScreenPos.Next, SessionParam08, params.vinNo)
            'ViewMode
            Me.SetValue(ScreenPos.Next, SessionParam09, params.viewMode)
            'Format
            Me.SetValue(ScreenPos.Next, SessionParam10, params.format)

            Select Case params.format
                Case Preview
                    'DISP_NUM
                    Me.SetValue(ScreenPos.Next, SessionDispNum, SessionDataDispNum_OrderOut)
                Case ServiceHistory
                    'SVCIN_NUM
                    Me.SetValue(ScreenPos.Next, SessionParam11, params.svcInNum)
                    'SVCIN_DealerCode
                    Me.SetValue(ScreenPos.Next, SessionParam12, params.svcInDealerCode)
                    'DISP_NUM
                    Me.SetValue(ScreenPos.Next, SessionDispNum, SessionDataDispNum_ServiceHistory)
                Case Else
                    'DISP_NUM
                    Me.SetValue(ScreenPos.Next, SessionDispNum, SessionDataDispNum_OrderOut)
            End Select

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
             , "R/O Prev DealerCode={0}&BranchCode={1}&LoginUserID={2}&SAChipID={3}&BASREZID={4}&R_O={5}&SEQ_NO={6}&VIN_NO={7}&ViewMode={8}&Format={9}&SVCIN_NUM={10}&SVCIN_DealerCode={11}" _
             , rowDmsCodeMap.CODE1 _
             , rowDmsCodeMap.CODE2 _
             , rowDmsCodeMap.ACCOUNT _
             , params.saChipID _
             , params.basrezID _
             , params.R_O _
             , params.seqNo _
             , params.vinNo _
             , params.viewMode _
             , params.format _
             , params.svcInNum _
             , params.svcInDealerCode))

        End Using

        '基幹画面連携用フレーム呼出処理
        Me.ScreenTransition()

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

#Region "全画面共通(基幹画面連携用フレーム呼出処理)"

    ''' <summary>
    ''' 基幹画面連携用フレーム呼出処理
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
        Me.RedirectNextScreen(ProgramIdOtherLinkage)

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

#End Region

#End Region

#End Region

#Region "ヘッダー"

    ''' <summary>
    ''' ヘッダー部初期化
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitHeader()
        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))
        '2019/07/05　TKM要件:型式対応　START　↓↓↓
        Dim sc3180202Bis As New SC3180202BusinessLogic
        '2019/07/05　TKM要件:型式対応　END　↑↑↑
        Dim script As New StringBuilder
        Dim registrationNo As String = String.Empty
        Dim customer As String = String.Empty
        Dim inspectionDate As String = String.Empty
        Dim mileage As String = String.Empty
        Dim roNo As String = String.Empty
        Dim sa As String = String.Empty
        Dim deliveryDate As String = String.Empty
        Dim tel As String = String.Empty

        headerScriptData = String.Empty

        '2019/07/05　TKM要件:型式対応　START　↓↓↓
        Using biz As New SC3180202BusinessLogic
            '2019/07/05　TKM要件:型式対応　END　↑↑↑
            Dim dt As New SC3180202HeaderDataDataTable
            dt = biz.GetHeaderData(params.dealerCode, params.branchCode, params.R_O, isExistActive)

            If dt.Rows.Count <> 0 Then
                With dt.Rows(0)
                    registrationNo = .Item("VCLREGNO").ToString

                    customer = .Item("NAME").ToString
                    If customer.StartsWith(.Item("NAMETITLE_NAME").ToString) Then
                        customer = customer.Substring(.Item("NAMETITLE_NAME").ToString.Length)
                    End If
                    If customer.EndsWith(.Item("NAMETITLE_NAME").ToString) Then
                        customer = customer.Substring(0, customer.Length - .Item("NAMETITLE_NAME").ToString.Length)
                    End If

                    If .Item("POSITION_TYPE").ToString = "1" Then '名称の前
                        customer = customer & " " & .Item("NAMETITLE_NAME").ToString
                    ElseIf .Item("POSITION_TYPE").ToString = "2" Then '名称の後
                        customer = .Item("NAMETITLE_NAME").ToString & " " & customer
                    End If

                    If IsDate(.Item("RSLT_SVCIN_DATETIME")) Then
                        inspectionDate = DateTimeFunc.FormatDate(3, CDate(.Item("RSLT_SVCIN_DATETIME")))
                    End If

                    mileage = .Item("SVCIN_MILE").ToString
                    roNo = .Item("RO_NUM").ToString
                    sa = .Item("USERNAME").ToString

                    dateDispFlg = False
                    If IsDate(.Item("RSLT_DELI_DATETIME")) Then
                        If (Date.Compare(Date.Parse(.Item("RSLT_DELI_DATETIME").ToString, CultureInfo.InvariantCulture), Date.Parse(FormatDbDateTime, CultureInfo.InvariantCulture)) = 0) Then
                            deliveryDate = DateTimeFunc.FormatDate(3, CDate(FormatDbDateTime))
                            dateDispFlg = False
                        Else
                            deliveryDate = DateTimeFunc.FormatDate(3, CDate(.Item("RSLT_DELI_DATETIME")))
                            dateDispFlg = True
                        End If
                    End If

                    tel = .Item("TELNO").ToString

                End With
            End If

        End Using

        With script
            'Page Title
            .AppendLine("document.getElementById(""_HD_TITLE"").innerHTML = """ & WebWordUtility.GetWord(ApplicationId, WordId.id000) & """;")

            'Registration no.(Caption)
            .AppendLine("document.getElementById(""_HD_REGNO_C"").innerHTML = """ & WebWordUtility.GetWord(ApplicationId, WordId.id001) & """;")

            'Registration no.(Value)
            .AppendLine("document.getElementById(""_HD_REGNO_V"").innerHTML = """ & registrationNo & """;")

            'R/O No.(Caption)
            .AppendLine("document.getElementById(""_HD_RONO_C"").innerHTML = """ & WebWordUtility.GetWord(ApplicationId, WordId.id005) & """;")

            'R/O No.(Value)
            .AppendLine("document.getElementById(""_HD_RONO_V"").innerHTML = """ & roNo & """;")

            'Customer(Caption)
            .AppendLine("document.getElementById(""_HD_CUST_C"").innerHTML = """ & WebWordUtility.GetWord(ApplicationId, WordId.id002) & """;")

            'Customer(Value)
            .AppendLine("document.getElementById(""_HD_CUST_V"").innerHTML = """ & customer & """;")

            'SA(Caption)
            .AppendLine("document.getElementById(""_HD_SA_C"").innerHTML = """ & WebWordUtility.GetWord(ApplicationId, WordId.id006) & """;")

            'SA(Value)
            .AppendLine("document.getElementById(""_HD_SA_V"").innerHTML = """ & sa & """;")

            'Inspection date(Caption)
            .AppendLine("document.getElementById(""_HD_IDATE_C"").innerHTML = """ & WebWordUtility.GetWord(ApplicationId, WordId.id003) & """;")

            'Inspection date(Value)
            .AppendLine("document.getElementById(""_HD_IDATE_V"").innerHTML = """ & inspectionDate & """;")

            '2016/11/02 「1900/01/01」を表示させない　Start
            If (dateDispFlg) Then
                'Delivery Date(Caption)
                .AppendLine("document.getElementById(""_HD_DDATE_C"").innerHTML = """ & WebWordUtility.GetWord(ApplicationId, WordId.id007) & """;")

                'Delivery Date(Value)
                .AppendLine("document.getElementById(""_HD_DDATE_V"").innerHTML = """ & deliveryDate & """;")
            Else
                'Delivery Date(Caption)
                .AppendLine("document.getElementById(""_HD_DDATE_C"").innerHTML = """";")

                'Delivery Date(Value)
                .AppendLine("document.getElementById(""_HD_DDATE_V"").innerHTML = """";")
            End If
            '2016/11/02 「1900/01/01」を表示させない　END

            'Mileage(Caption)
            .AppendLine("document.getElementById(""_HD_MILE_C"").innerHTML = """ & WebWordUtility.GetWord(ApplicationId, WordId.id004) & """;")

            'Mileage(Value)
            If mileage <> String.Empty Then
                mileage = mileage & "Km"
            End If

            .AppendLine("document.getElementById(""_HD_MILE_V"").innerHTML = """ & mileage & """;")

            'Contact(Value)
            .AppendLine("document.getElementById(""_HD_CONT_V"").innerHTML = """ & tel & """;")

            'アドバイス
            .AppendLine("document.getElementById(""_AD_TITLE"").innerHTML = """ & WebWordUtility.GetWord(ApplicationId, WordId.id018) & """;")
        End With

        headerScriptData = script.ToString

        '2014/07/09 タイトルをデザイン固定にするため削除
        'Using biz As New SC3180202BusinessLogic
        '    Dim dt As New DataTable
        '    dt = biz.GetTitleName(params.dealerCode, params.branchCode)
        '    If dt.Rows.Count <> 0 Then
        '        Dim wkScript As New StringBuilder
        '        Dim itemCode As String = String.Empty
        '        Dim headItemName As String = String.Empty

        '        For i As Integer = 0 To dt.Rows.Count - 1
        '            With dt.Rows(i)
        '                '各見出し
        '                'アイテムコード
        '                'itemCode = .Item("HTML_INSPEC_ITEM_CD").ToString
        '                itemCode = .Item("INSPEC_ITEM_CD").ToString

        '                '点検項目
        '                headItemName = .Item("INSPEC_ITEM_NAME").ToString

        '                wkScript.AppendLine("TitleLabelSetting(""" & itemCode & """,""" & headItemName & """);")
        '            End With
        '        Next

        '        detailTitleScriptData = wkScript.ToString
        '    End If
        'End Using

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub
#End Region

#Region "明細部"
    ''' <summary>
    ''' 明細部作成
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitDetail()
        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        detailScriptData = String.Empty

        Using biz As New SC3180202BusinessLogic
            Dim dt As New SC3180202DetailDataDataTable
            dt = biz.GetDetailData(params.dealerCode, params.branchCode, params.R_O, isExistActive)
            If dt.Rows.Count <> 0 Then
                detailScriptData = CreateDetailDataScript(dt)
            End If
        End Using

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' データ設定
    ''' </summary>
    ''' <remarks>車種データのJavaスクリプトを設定する</remarks>
    Private Function CreateDetailDataScript(ByVal dt As SC3180202DetailDataDataTable) As String
        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))


        Dim wkScript As New StringBuilder
        Dim wkAdvice As New StringBuilder
        Dim itemCode As String = String.Empty
        Dim itemName As String = String.Empty
        Dim headItemName As String = String.Empty
        Dim inputValue As String = String.Empty
        Dim icon(11) As String
        Dim iconIndex As Integer = 0
        Dim advice As String = String.Empty
        Dim rsltVal As Decimal = 0

        'アイコンのパスを設定
        icon(1) = IconNoProblem
        icon(2) = IconNeedInspection
        icon(3) = IconNeedReplace
        icon(4) = IconNeedFixing
        icon(5) = IconNeedCleaning
        icon(6) = IconNeedSwapping

        icon(7) = IconAlreadyReplace
        icon(8) = IconAlreadyFixed
        icon(9) = IconAlreadyCleaning
        icon(10) = IconAlreadySwapping
        icon(11) = IconNoCheck

        For i As Integer = 0 To dt.Rows.Count - 1

            With dt.Rows(i)

                '2016/11/02 点検項目がない場合でもアドバイスを表示させる　Start
                'アドバイス
                advice = .Item("ADVICE_CONTENT").ToString
                '2016/11/02 点検項目がない場合でもアドバイスを表示させる　End

                'If .Item("HTML_INSPEC_ITEM_CD").ToString.Trim.Length = 0 Then
                If .Item("INSPEC_ITEM_CD").ToString.Trim.Length = 0 Then
                    Continue For
                End If

                'アイテムコード
                'itemCode = .Item("HTML_INSPEC_ITEM_CD").ToString
                itemCode = .Item("INSPEC_ITEM_CD").ToString

                '点検項目
                'headItemName = .Item("INSPEC_ITEM_NAME").ToString

                'サブ点検項目

                '2014/07/09 サブ点検項目名称の取得元を変更
                itemName = .Item("PRINT_INSPEC_ITEM_NAME").ToString
                'itemName = .Item("SUB_INSPEC_ITEM_NAME").ToString

                '入力値+単位
                'BEFOREとAFTERの数値が両方入っている場合⇒AFTERの数値をセット
                'BEFOREの数値のみが入っている場合       ⇒BEFOREの数値をセット
                '値が-1⇒未入力とみなす
                If Decimal.TryParse(.Item("RSLT_VAL_AFTER").ToString, rsltVal) AndAlso
                   0 <= rsltVal Then
                    inputValue = rsltVal.ToString(FormatRsltVal) & .Item("DISP_TEXT_UNIT").ToString
                Else
                    If Decimal.TryParse(.Item("RSLT_VAL_BEFORE").ToString, rsltVal) AndAlso
                       0 <= rsltVal Then
                        inputValue = rsltVal.ToString(FormatRsltVal) & .Item("DISP_TEXT_UNIT").ToString
                    Else
                        inputValue = ""
                    End If
                End If

                '2016/11/02 点検項目がない場合でもアドバイスを表示させる　Start
                'アドバイス
                'advice = .Item("ADVICE_CONTENT").ToString
                '2016/11/02 点検項目がない場合でもアドバイスを表示させる　End

                '点検結果アイコンの設定
                iconIndex = CInt(.Item("INSPEC_RSLT_CD"))

                '2014/06/27 不具合修正　Start
                ''作業内容アイコンの設定
                'If CInt(.Item("OPERATION_RSLT_ALREADY_REPLACE")) = SelectFlg.CheckOn Then
                '    iconIndex = InspecResultCD.AlreadyReplace
                'End If

                'If CInt(.Item("OPERATION_RSLT_ALREADY_FIX")) = SelectFlg.CheckOn Then
                '    iconIndex = InspecResultCD.AlreadyFixed
                'End If

                'If CInt(.Item("OPERATION_RSLT_ALREADY_CLEAN")) = SelectFlg.CheckOn Then
                '    iconIndex = InspecResultCD.AlreadyCleaning
                'End If

                'If CInt(.Item("OPERATION_RSLT_ALREADY_SWAP")) = SelectFlg.CheckOn Then
                '    iconIndex = InspecResultCD.AlreadySwapped
                'End If

                '作業内容アイコンの設定
                If iconIndex <> 7 Then
                    Using biz As New SC3180202BusinessLogic
                        Dim itemCodeData As New DataTable
                        itemCodeData = biz.GetItemCodeOrder(itemCode)
                        If itemCodeData.Rows.Count <> 0 Then
                            For k As Integer = 0 To itemCodeData.Rows.Count - 1
                                Dim wkName As String = itemCodeData.Rows(k).Item("DISP_NAME").ToString

                                '作業内容アイコンの設定
                                If CInt(.Item("OPERATION_RSLT_ALREADY_" & wkName)) = SelectFlg.CheckOn Then
                                    Select Case wkName
                                        Case "REPLACE"
                                            iconIndex = InspecResultCD.AlreadyReplace
                                            Exit For
                                        Case "FIX"
                                            iconIndex = InspecResultCD.AlreadyFixed
                                            Exit For
                                        Case "CLEAN"
                                            iconIndex = InspecResultCD.AlreadyCleaning
                                            Exit For
                                        Case "SWAP"
                                            iconIndex = InspecResultCD.AlreadySwapped
                                            Exit For
                                    End Select
                                End If
                            Next
                        End If
                    End Using
                    '2014/06/27 不具合修正　End
                Else
                    iconIndex = InspecResultCD.NoChecked
                End If
            End With

            Dim strIcon As String = String.Empty

            ''チェック・作業内容実施マークは、承認済の点検に対して表示する。　承認前の点検は、チェック部分を表示しない。（白くする）
            Select Case CInt(dt.Rows(i).Item("APPROVAL_STATUS"))
                Case OperationStatus.Approved
                    strIcon = icon(iconIndex)
                Case Else
                    '承認前の点検は、チェック部分を表示しない。（白くする）
                    strIcon = String.Empty
            End Select

            With wkScript
                '各見出し
                '2014/07/09 背景色のセット方法を変更
                '.AppendLine("TitleSetting(""" & itemCode & """);")

                'サブアイテムの項目
                '2014/09/22 Itemセット方法修正
                'If itemName.IndexOf("(") = -1 Then
                .AppendLine("LabelsSetting(""" & itemCode & """,""<p>" & itemName & "</p>"", """ & inputValue & """, """ & strIcon & """);")
                'Else
                '.AppendLine("LabelsSetting(""" & itemCode & """,""<p>" & headItemName & itemName & "</p>"", """ & inputValue & """, """ & strIcon & """);")
                'End If
                '2014/07/09 背景色のセット方法を変更
                .AppendLine("TitleBackGroundSetting();")

            End With
        Next

        'Advice title
        wkAdvice.AppendLine("document.getElementById(""_AD_VAL"").innerHTML = """ & ChengeValueEscape(advice) & """;")

        headerScriptData = headerScriptData & vbCrLf & wkAdvice.ToString

        Return wkScript.ToString

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Function

    ''' <summary>
    ''' エスケープ処理
    ''' </summary>
    ''' <param name="advice">アドバイス</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ChengeValueEscape(ByVal advice As String) As String
        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim wkAdvice As String = String.Empty

        wkAdvice = Replace(Server.HtmlEncode(advice), "\", "\\")
        wkAdvice = Replace(wkAdvice, """", "\""")
        wkAdvice = Replace(wkAdvice, vbCrLf, "<br/>")
        Return wkAdvice

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Function

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
            CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMainMenu)
        AddHandler mainMenuButton.Click, AddressOf MainMenuButton_Click
        mainMenuButton.OnClientClick = _
            String.Format(CultureInfo.CurrentCulture, _
                          FooterButtonClick, _
                          FooterMainMenu.ToString(CultureInfo.CurrentCulture))

        '権限チェック
        If inStaffInfo.OpeCD = Operation.SA OrElse inStaffInfo.OpeCD = Operation.SM Then
            'SA権限、SM権限の場合
            '顧客詳細ボタンの設定
            Dim customerButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FooterCustomer)
            customerButton.OnClientClick = _
                String.Format(CultureInfo.CurrentCulture, _
                              FooterButtonClick, _
                              FooterCustomer.ToString(CultureInfo.CurrentCulture))

            'R/Oボタンの設定
            Dim roButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FooterRo)
            AddHandler roButton.Click, AddressOf RoListButton_Click
            roButton.OnClientClick = _
                String.Format(CultureInfo.CurrentCulture, _
                              FooterButtonClick, _
                              FooterRo.ToString(CultureInfo.CurrentCulture))

            '商品訴求コンテンツボタンの設定
            Dim goodsSolicitationContentsButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FooterContents)
            AddHandler goodsSolicitationContentsButton.Click, AddressOf GoodsSolicitationContentsButton_Click
            goodsSolicitationContentsButton.OnClientClick = _
                String.Format(CultureInfo.CurrentCulture, _
                              FooterButtonClick, _
                              FooterContents.ToString(CultureInfo.CurrentCulture))

            'キャンペーンボタンの設定
            Dim campaignButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FooterCampaing)
            AddHandler campaignButton.Click, AddressOf CampaignButton_Click
            campaignButton.OnClientClick = _
                String.Format(CultureInfo.CurrentCulture, _
                              FooterButtonClick, _
                              FooterCampaing.ToString(CultureInfo.CurrentCulture))

            '来店管理ボタンの設定
            Dim visitManagmentButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FooterVisitManament)
            AddHandler visitManagmentButton.Click, AddressOf VisitManagmentButton_Click
            visitManagmentButton.OnClientClick = _
                String.Format(CultureInfo.CurrentCulture, _
                              FooterButtonClick, _
                              FooterVisitManament.ToString(CultureInfo.CurrentCulture))

            'SMBボタンの設定
            Dim smbButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FooterSmb)
            AddHandler smbButton.Click, AddressOf SMBButton_Click
            smbButton.OnClientClick = _
                String.Format(CultureInfo.CurrentCulture, _
                              FooterButtonClick, _
                              FooterSmb.ToString(CultureInfo.CurrentCulture))

        ElseIf inStaffInfo.OpeCD = Operation.CT OrElse inStaffInfo.OpeCD = Operation.TEC Then
            'CT権限、TC権限の場合
            'R/Oボタンの設定
            Dim roButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FooterRo)
            AddHandler roButton.Click, AddressOf RoListButton_Click
            roButton.OnClientClick = _
                String.Format(CultureInfo.CurrentCulture, _
                              FooterButtonClick, _
                              FooterRo.ToString(CultureInfo.CurrentCulture))

            ''追加作業ボタンの設定
            Dim addListButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FooterAddList)
            AddHandler addListButton.Click, AddressOf AddListButton_Click
            addListButton.OnClientClick = _
                String.Format(CultureInfo.CurrentCulture, _
                              FooterButtonClick, _
                              FooterAddList.ToString(CultureInfo.CurrentCulture))

        ElseIf inStaffInfo.OpeCD = Operation.CHT Then
            'ChT権限の場合
            'TCメインボタンの設定
            Dim technicianMainButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FooterTecnicianMain)
            technicianMainButton.OnClientClick = _
                String.Format(CultureInfo.CurrentCulture, _
                              FooterButtonClick, _
                              FooterTecnicianMain.ToString(CultureInfo.CurrentCulture))

            'FMメインボタンの設定
            Dim FormanMainButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FooterFormanMain)
            AddHandler FormanMainButton.Click, AddressOf FormanMainButton_Click
            FormanMainButton.OnClientClick = _
                String.Format(CultureInfo.CurrentCulture, _
                              FooterButtonClick, _
                              FooterFormanMain.ToString(CultureInfo.CurrentCulture))

            'R/Oボタンの設定
            Dim roButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FooterRo)
            AddHandler roButton.Click, AddressOf RoListButton_Click
            roButton.OnClientClick = _
                String.Format(CultureInfo.CurrentCulture, _
                              FooterButtonClick, _
                              FooterRo.ToString(CultureInfo.CurrentCulture))

            ''追加作業ボタンの設定
            Dim addListButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FooterAddList)
            AddHandler addListButton.Click, AddressOf AddListButton_Click
            addListButton.OnClientClick = _
                String.Format(CultureInfo.CurrentCulture, _
                              FooterButtonClick, _
                              FooterAddList.ToString(CultureInfo.CurrentCulture))

        ElseIf inStaffInfo.OpeCD = Operation.FM Then
            'FM権限の場合
            'SMBボタンの設定
            Dim smbButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FooterSmb)
            AddHandler smbButton.Click, AddressOf SMBButton_Click
            smbButton.OnClientClick = _
                String.Format(CultureInfo.CurrentCulture, _
                              FooterButtonClick, _
                              FooterSmb.ToString(CultureInfo.CurrentCulture))

            'R/Oボタンの設定
            Dim roButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FooterRo)
            AddHandler roButton.Click, AddressOf RoListButton_Click
            roButton.OnClientClick = _
                String.Format(CultureInfo.CurrentCulture, _
                              FooterButtonClick, _
                              FooterRo.ToString(CultureInfo.CurrentCulture))

            ''追加作業ボタンの設定
            Dim addListButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FooterAddList)
            AddHandler addListButton.Click, AddressOf AddListButton_Click
            addListButton.OnClientClick = _
                String.Format(CultureInfo.CurrentCulture, _
                              FooterButtonClick, _
                              FooterAddList.ToString(CultureInfo.CurrentCulture))

        ElseIf inStaffInfo.OpeCD = Operation.SVR Then
            'SVR権限の場合

            '来店管理ボタンの設定
            Dim visitManagmentButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FooterVisitManament)
            AddHandler visitManagmentButton.Click, AddressOf VisitManagmentButton_Click
            visitManagmentButton.OnClientClick = _
                String.Format(CultureInfo.CurrentCulture, _
                              FooterButtonClick, _
                              FooterVisitManament.ToString(CultureInfo.CurrentCulture))

            'R/Oボタンの設定
            Dim roButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FooterRo)
            AddHandler roButton.Click, AddressOf RoListButton_Click
            roButton.OnClientClick = _
                String.Format(CultureInfo.CurrentCulture, _
                              FooterButtonClick, _
                              FooterRo.ToString(CultureInfo.CurrentCulture))

            '全体管理ボタンの設定
            Dim allManagmentButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FooterAllManagment)
            AddHandler allManagmentButton.Click, AddressOf AllManagmentButtonButton_Click
            allManagmentButton.OnClientClick = _
                String.Format(CultureInfo.CurrentCulture, _
                              FooterButtonClick, _
                              FooterAllManagment.ToString(CultureInfo.CurrentCulture))

        End If

        '電話帳ボタンの設定
        Dim telDirectoryButton As CommonMasterFooterButton = _
            CType(Me.Master, CommonMasterPage).GetFooterButton(FooterTelDirector)
       
        telDirectoryButton.OnClientClick = FooterEventTel

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
            Me.RedirectNextScreen(ProgramIdMinaMenuSa)

        ElseIf staffInfo.OpeCD = Operation.SM Then
            '全体管理に遷移する
            Me.RedirectNextScreen(ProgramIdAllManagment)

        ElseIf staffInfo.OpeCD = Operation.CT OrElse staffInfo.OpeCD = Operation.CHT Then
            '工程管理に遷移する
            Me.RedirectNextScreen(ProgramIdProcessControl)

        ElseIf staffInfo.OpeCD = Operation.TEC Then
            'メインメニュー(TC)に遷移する
            Me.RedirectNextScreen(ProgramIdMainMenuTc)

        ElseIf staffInfo.OpeCD = Operation.FM Then
            'メインメニュー(FM)に遷移する
            Me.RedirectNextScreen(ProgramIdMainMenuFm)

        ElseIf staffInfo.OpeCD = Operation.SVR Then
            '未振当一覧に遷移する
            Me.RedirectNextScreen(ProgramIdAssignmentList)

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
        Me.RedirectNextScreen(ProgramIdMainMenuFm)

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
        Me.RedirectNextScreen(ProgramIdVisitManagment)

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
    Private Sub RoListButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'スタッフ情報取得
        Dim staffInfo As StaffContext = StaffContext.Current

        Using biz As New SC3180202BusinessLogic

            'DMS情報取得
            Dim dtDmsCodeMapDataTable As DmsCodeMapDataTable = biz.GetDmsDealerData(staffInfo)

            'DMS情報のチェック
            If Not (IsNothing(dtDmsCodeMapDataTable)) Then
                '取得できた場合
                '画面間パラメータを設定
                '表示番号
                Me.SetValue(ScreenPos.Next, SessionDispNum, SessionDataDispNum_ROList)

                'DMS販売店コード
                Me.SetValue(ScreenPos.Next, SessionParam01, dtDmsCodeMapDataTable(0).CODE1)

                'DMS店舗コード
                Me.SetValue(ScreenPos.Next, SessionParam02, dtDmsCodeMapDataTable(0).CODE2)

                'アカウント
                Me.SetValue(ScreenPos.Next, SessionParam03, dtDmsCodeMapDataTable(0).ACCOUNT)

                '来店実績連番
                Me.SetValue(ScreenPos.Next, SessionParam04, params.saChipID)

                'DMS予約ID
                Me.SetValue(ScreenPos.Next, SessionParam05, params.basrezID)

                'RO番号
                Me.SetValue(ScreenPos.Next, SessionParam06, params.R_O)

                'RO作業連番
                Me.SetValue(ScreenPos.Next, SessionParam07, params.seqNo)

                'VIN
                Me.SetValue(ScreenPos.Next, SessionParam08, params.vinNo)

                '編集モード
                Me.SetValue(ScreenPos.Next, SessionParam09, EditMode)

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                         , "R/O List DealerCode={0}&BranchCode={1}&LoginUserID={2}&SAChipID={3}&BASREZID={4}&R_O={5}&SEQ_NO={6}&VIN_NO={7}&ViewMode={8}" _
                                         , dtDmsCodeMapDataTable(0).CODE1 _
                                         , dtDmsCodeMapDataTable(0).CODE2 _
                                         , dtDmsCodeMapDataTable(0).ACCOUNT _
                                         , params.saChipID _
                                         , params.basrezID _
                                         , params.R_O _
                                         , params.seqNo _
                                         , params.vinNo _
                                         , EditMode))

                '追加作業画面(枠)に遷移する
                Me.RedirectNextScreen(ProgramIdOtherLinkage)

            Else
                '取得できなかった場合
                'エラー
                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                                         , "{0}.{1} ERROR " _
                                         , Me.GetType.ToString _
                                         , System.Reflection.MethodBase.GetCurrentMethod.Name))

            End If

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
        Me.SetValue(ScreenPos.Next, SessionDealerCode, Space(1))

        'DMS店舗コード
        Me.SetValue(ScreenPos.Next, SessionBranchCode, Space(1))

        'アカウント
        Me.SetValue(ScreenPos.Next, SessionLoginUserID, Space(1))

        '来店実績連番
        Me.SetValue(ScreenPos.Next, SessionSAChipID, params.saChipID)

        'DMS予約ID
        Me.SetValue(ScreenPos.Next, SessionBASREZID, params.basrezID)

        'RO番号
        Me.SetValue(ScreenPos.Next, SessionRO, params.R_O)

        'RO作業連番
        Me.SetValue(ScreenPos.Next, SessionSEQNO, params.seqNo)

        'VIN
        Me.SetValue(ScreenPos.Next, SessionVINNO, params.vinNo)

        '編集モード
        Me.SetValue(ScreenPos.Next, SessionViewMode, ReadMode)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                         , "GoodsSolicitation DealerCode={0}&BranchCode={1}&LoginUserID={2}&SAChipID={3}&BASREZID={4}&R_O={5}&SEQ_NO={6}&VIN_NO={7}&ViewMode={8}" _
                         , Space(1) _
                         , Space(1) _
                         , Space(1) _
                         , params.saChipID _
                         , params.basrezID _
                         , params.R_O _
                         , params.seqNo _
                         , params.vinNo _
                         , ReadMode))

        '商品訴求コンテンツ画面に遷移する
        Me.RedirectNextScreen(ProgramIdGoodsSolication)

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

        Using biz As New SC3180202BusinessLogic

            'DMS情報取得
            Dim dtDmsCodeMapDataTable As DmsCodeMapDataTable = biz.GetDmsDealerData(staffInfo)

            'DMS情報のチェック
            If Not (IsNothing(dtDmsCodeMapDataTable)) Then
                '取得できた場合
                '画面間パラメータを設定
                '表示番号
                Me.SetValue(ScreenPos.Next, SessionDispNum, SessionDataDispNum_Campaign)

                'DMS販売店コード
                Me.SetValue(ScreenPos.Next, SessionParam01, dtDmsCodeMapDataTable(0).CODE1)

                'DMS店舗コード
                Me.SetValue(ScreenPos.Next, SessionParam02, dtDmsCodeMapDataTable(0).CODE2)

                'アカウント
                Me.SetValue(ScreenPos.Next, SessionParam03, dtDmsCodeMapDataTable(0).ACCOUNT)

                '来店実績連番
                Me.SetValue(ScreenPos.Next, SessionParam04, params.saChipID)

                'DMS予約ID
                Me.SetValue(ScreenPos.Next, SessionParam05, params.basrezID)

                'RO番号
                Me.SetValue(ScreenPos.Next, SessionParam06, params.R_O)

                'RO作業連番
                Me.SetValue(ScreenPos.Next, SessionParam07, params.seqNo)

                'VIN
                Me.SetValue(ScreenPos.Next, SessionParam08, params.vinNo)

                '編集モード
                Me.SetValue(ScreenPos.Next, SessionParam09, EditMode)

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                     , "R/O List DealerCode={0}&BranchCode={1}&LoginUserID={2}&SAChipID={3}&BASREZID={4}&R_O={5}&SEQ_NO={6}&VIN_NO={7}&ViewMode={8}" _
                     , dtDmsCodeMapDataTable(0).CODE1 _
                     , dtDmsCodeMapDataTable(0).CODE2 _
                     , dtDmsCodeMapDataTable(0).ACCOUNT _
                     , params.saChipID _
                     , params.basrezID _
                     , params.R_O _
                     , params.seqNo _
                     , params.vinNo _
                     , EditMode))

                '追加作業画面(枠)に遷移する
                Me.RedirectNextScreen(ProgramIdOtherLinkage)

            Else
                '取得できなかった場合
                'エラー
                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                                         , "{0}.{1} ERROR " _
                                         , Me.GetType.ToString _
                                         , System.Reflection.MethodBase.GetCurrentMethod.Name))

            End If

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
        Me.RedirectNextScreen(ProgramIdAllManagment)

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
        Me.RedirectNextScreen(ProgramIdProcessControl)

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

        Using biz As New SC3180202BusinessLogic

            'DMS情報取得
            Dim dtDmsCodeMapDataTable As DmsCodeMapDataTable = biz.GetDmsDealerData(staffInfo)

            'DMS情報のチェック
            If Not (IsNothing(dtDmsCodeMapDataTable)) Then
                '取得できた場合
                '画面間パラメータを設定
                '表示番号
                Me.SetValue(ScreenPos.Next, SessionDispNum, SessionDataDispNum_AddList)

                'DMS販売店コード
                Me.SetValue(ScreenPos.Next, SessionParam01, dtDmsCodeMapDataTable(0).CODE1)

                'DMS店舗コード
                Me.SetValue(ScreenPos.Next, SessionParam02, dtDmsCodeMapDataTable(0).CODE2)

                'アカウント
                Me.SetValue(ScreenPos.Next, SessionParam03, staffInfo.Account.Substring(0, staffInfo.Account.IndexOf("@")))

                '来店実績連番
                Me.SetValue(ScreenPos.Next, SessionParam04, params.saChipID)

                'DMS予約ID
                Me.SetValue(ScreenPos.Next, SessionParam05, params.basrezID)

                'RO番号
                Me.SetValue(ScreenPos.Next, SessionParam06, params.R_O)

                'RO作業連番
                Me.SetValue(ScreenPos.Next, SessionParam07, params.seqNo)

                'VIN
                Me.SetValue(ScreenPos.Next, SessionParam08, params.vinNo)

                '編集モード
                Me.SetValue(ScreenPos.Next, SessionParam09, EditMode)

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                     , "AddList DealerCode={0}&BranchCode={1}&LoginUserID={2}&SAChipID={3}&BASREZID={4}&R_O={5}&SEQ_NO={6}&VIN_NO={7}&ViewMode={8}" _
                     , dtDmsCodeMapDataTable(0).CODE1 _
                     , dtDmsCodeMapDataTable(0).CODE2 _
                     , staffInfo.Account.Substring(0, staffInfo.Account.IndexOf("@")) _
                     , params.saChipID _
                     , params.basrezID _
                     , params.R_O _
                     , params.seqNo _
                     , params.vinNo _
                     , EditMode))

                '追加作業画面(枠)に遷移する
                Me.RedirectNextScreen(ProgramIdOtherLinkage)

            Else
                '取得できなかった場合
                'エラー
                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                                         , "{0}.{1} ERROR " _
                                         , Me.GetType.ToString _
                                         , System.Reflection.MethodBase.GetCurrentMethod.Name))
            End If

        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

#End Region

End Class
