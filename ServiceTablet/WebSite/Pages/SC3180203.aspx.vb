'------------------------------------------------------------------------------
'SC3180203.aspx.vb
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
Imports Toyota.eCRB.ServerCheck.CheckResult.BizLogic.SC3180203
Imports Toyota.eCRB.ServerCheck.CheckResult.DataAccess.SC3180203.SC3180203DataSet
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.DataAccess.ServiceCommonClassDataSet
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.DataAccess
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.BizLogic

''' <summary>
''' チェックシートプレビュー
''' </summary>
''' <remarks></remarks>
Partial Class Pages_SC3180203
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
    Private Const ApplicationId As String = "SC3180203"
    Private Const ProgramIdPrintPreview As String = "SC3180202"
 
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
    Private Const PathCarModel As String = "..\Styles\SC3180203\Templates\"
    Private Const PathCarModelLxs As String = "..\Styles\SC3180203\LexusTemplates\"
    Private Const PathExtension As String = ".txt"
    Private Const PathDefault As String = "_Default"

    '点検結果入力値(Before, After)のフォーマット(999.99値形式)
    Private Const FormatRsltVal As String = "##0.00"

    'サービス入庫テーブル.納車実績日時初期値(年月日)
    Private Const FormatDbDateTime As String = "1900/01/01"

    'R/O Preview(S-SA-16)
    Private Const Preview As String = "0"

    ''' <summary>
    ''' 点検結果
    ''' </summary>
    ''' <remarks></remarks>
    Enum INSPEC_RESULT_CD
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
    Enum OPERATION_STATUS
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
     
    End Enum
#End Region

#Region "セッション関連"

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

            '車種ごとのテンプレート取得
            Dim fileName As String = String.Empty

            Using biz As New SC3180203BusinessLogic

                '2017/01/24　ライフサイクル対応追加　START　↓↓↓
                isExistActive = biz.IsExistServiceinActive(params.dealerCode, params.branchCode, params.R_O)
                '2017/01/24　ライフサイクル対応追加　END　↑↑↑

                System.Environment.CurrentDirectory = Server.MapPath("./")
                '2020/02/12　TKM要件:型式対応　START　↓↓↓
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

                'レクサスの場合は専用のテンプレートを取得
                Dim isLxs As Boolean = biz.isLexus()
                Dim templetePath As String = String.Empty

                '2019/07/05　TKM要件:型式対応　END　↑↑↑
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

            'ヘッダー情報設定
            InitHeader()

            '明細情報設定
            InitDetail()
        End If

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} : {2} END" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name,scriptTemplateString))

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

        Using biz As New SC3180203BusinessLogic

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

            'params.saChipID = DirectCast(GetValue(ScreenPos.Current, SessionSAChipID, False), String)
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
              , "{0}.{1} SAChipID=[{2}]" _
              , Me.GetType.ToString _
              , System.Reflection.MethodBase.GetCurrentMethod.Name _
              , GetValue(ScreenPos.Current, SessionSAChipID, False).ToString))

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

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

#Region "ボタン"
    ''' <summary>
    ''' btnClose_Click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        ''販売店コード
        'Me.SetValue(ScreenPos.Next, SessionDealerCode, params.DealerCode)
        ''店舗コード
        'Me.SetValue(ScreenPos.Next, SessionBranchCode, params.BranchCode)
        ''アカウント
        'Me.SetValue(ScreenPos.Next, SessionLoginUserID, params.LoginUserID)
        ''来店者実績連番
        'Me.SetValue(ScreenPos.Next, SessionSAChipID, params.SAChipID)
        ''DMS予約ID
        'Me.SetValue(ScreenPos.Next, SessionBASREZID, params.BASREZID)
        ''RO
        'Me.SetValue(ScreenPos.Next, SessionRO, params.R_O)
        ''RO_JOB_SEQ(親のRO_JOB_SEQ = 0)            
        'Me.SetValue(ScreenPos.Next, SessionSEQNO, params.seqNo)
        ''VIN
        'Me.SetValue(ScreenPos.Next, SessionVINNO, params.vinNo)
        ''ViewMode
        'Me.SetValue(ScreenPos.Next, SessionViewMode, params.ViewMode)
        ''Format
        'Me.SetValue(ScreenPos.Next, SessionFormat, params.Format)
        ''SVCIN_NUM
        'Me.SetValue(ScreenPos.Next, SessionSVCIN_NUM, params.svcInNum)
        ''SVCIN_DealerCode
        'Me.SetValue(ScreenPos.Next, SessionSVCIN_DealerCode, params.svcInDealerCode)

        'Me.RedirectNextScreen(ProgramIdPrintPreview)
        'ScriptManager.RegisterStartupScript(Me, Me.GetType, "PrintURL", "ClosePopup();", True)
        Me.RedirectPrevScreen()

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} END" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

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

        Dim SC3180203Bis As New SC3180203BusinessLogic()
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

        Using biz As New SC3180203BusinessLogic
            Dim dt As New SC3180203HeaderDataDataTable
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

            '2016/11/08 「1900/01/01」を表示させない　Start
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
            '2016/11/08 「1900/01/01」を表示させない　END

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
        'Using biz As New SC3180203BusinessLogic
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

		'2019/07/05　TKM要件:型式対応　START　↓↓↓
        Using biz As New SC3180203BusinessLogic
            '2019/07/05　TKM要件:型式対応　END　↑↑↑
            Dim dt As New SC3180203DetailDataDataTable
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
    Private Function CreateDetailDataScript(ByVal dt As SC3180203DetailDataDataTable) As String
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

                '2016/11/08 点検項目がない場合でもアドバイスを表示させる　Start
                'アドバイス
                advice = .Item("ADVICE_CONTENT").ToString
                '2016/11/08 点検項目がない場合でもアドバイスを表示させる　End

                'If .Item("HTML_INSPEC_ITEM_CD").ToString.Trim.Length = 0 Then
                If .Item("INSPEC_ITEM_CD").ToString.Trim.Length = 0 Then
                    Continue For
                End If

                ''アイテムコード
                'itemCode = .Item("HTML_INSPEC_ITEM_CD").ToString
                itemCode = .Item("INSPEC_ITEM_CD").ToString

                '点検項目
                headItemName = .Item("INSPEC_ITEM_NAME").ToString

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

                '2016/11/08 点検項目がない場合でもアドバイスを表示させる　Start
                'アドバイス
                'advice = .Item("ADVICE_CONTENT").ToString
                '2016/11/08 点検項目がない場合でもアドバイスを表示させる　End

                '点検結果アイコンの設定
                iconIndex = CInt(.Item("INSPEC_RSLT_CD"))

                '2014/06/27 不具合修正　Start
                '作業内容アイコンの設定
                'If CInt(.Item("OPERATION_RSLT_ALREADY_REPLACE")) = SelectFlg.CheckOn Then
                '    iconIndex = INSPEC_RESULT_CD.AlreadyReplace
                'End If

                'If CInt(.Item("OPERATION_RSLT_ALREADY_FIX")) = SelectFlg.CheckOn Then
                '    iconIndex = INSPEC_RESULT_CD.AlreadyFixed
                'End If

                'If CInt(.Item("OPERATION_RSLT_ALREADY_CLEAN")) = SelectFlg.CheckOn Then
                '    iconIndex = INSPEC_RESULT_CD.AlreadyCleaning
                'End If

                'If CInt(.Item("OPERATION_RSLT_ALREADY_SWAP")) = SelectFlg.CheckOn Then
                '    iconIndex = INSPEC_RESULT_CD.AlreadySwapped
                'End If

                '作業内容アイコンの設定
                If iconIndex <> 7 Then
                	'2019/07/05　TKM要件:型式対応　START　↓↓↓
                    Using biz As New SC3180203BusinessLogic
                        '2019/07/05　TKM要件:型式対応　END　↑↑↑
                        Dim itemCodeData As New DataTable
                        itemCodeData = biz.GetItemCodeOrder(itemCode)
                        If itemCodeData.Rows.Count <> 0 Then
                            For k As Integer = 0 To itemCodeData.Rows.Count - 1
                                Dim wkName As String = itemCodeData.Rows(k).Item("DISP_NAME").ToString

                                '作業内容アイコンの設定
                                If CInt(.Item("OPERATION_RSLT_ALREADY_" & wkName)) = SelectFlg.CheckOn Then
                                    Select Case wkName
                                        Case "REPLACE"
                                            iconIndex = INSPEC_RESULT_CD.AlreadyReplace
                                            Exit For
                                        Case "FIX"
                                            iconIndex = INSPEC_RESULT_CD.AlreadyFixed
                                            Exit For
                                        Case "CLEAN"
                                            iconIndex = INSPEC_RESULT_CD.AlreadyCleaning
                                            Exit For
                                        Case "SWAP"
                                            iconIndex = INSPEC_RESULT_CD.AlreadySwapped
                                            Exit For
                                    End Select
                                End If
                            Next
                        End If
                    End Using
                    '2014/06/27 不具合修正　End
                Else
                    iconIndex = INSPEC_RESULT_CD.NoChecked
                End If
            End With

            Dim strIcon As String = String.Empty

            ''チェック・作業内容実施マークは、承認済の点検に対して表示する。　承認前の点検は、チェック部分を表示しない。（白くする）
            Select Case CInt(dt.Rows(i).Item("APPROVAL_STATUS"))
                Case OPERATION_STATUS.Approved
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
                '    .AppendLine("LabelsSetting(""" & itemCode & """,""<p>" & headItemName & itemName & "</p>"", """ & inputValue & """, """ & strIcon & """);")
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

End Class

