'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3160218.aspx.vb
'─────────────────────────────────────
'機能： RO作成機能グローバル連携処理
'補足： 外観チェック画面
'作成： 2013/11/25 SKFC 久代 
'更新： 2018/09/26 SKFC 上田 TKM 疎通テストIssue-009(CONN-0009)対応
'更新： 
'─────────────────────────────────────
Option Strict On
Option Explicit On

Imports System
Imports System.Data
Imports System.Web.Script.Serialization
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.iCROP.BizLogic.SC3160218
Imports System.Collections.ObjectModel

''' <summary>
''' コードビハインドクラス
''' </summary>
''' <remarks></remarks>
Partial Class Pages_SC3160218
    Inherits BasePage
    Implements IDisposable

#Region "定数"
    '文言番号
    Private Const CaptionNumTitle As Decimal = 20
    Private Const CaptionNumNoDamage As Decimal = 21
    Private Const CaptionNumCanNotCheck As Decimal = 22
    Private Const WordDictionaryBegin As Decimal = CaptionNumTitle
    Private Const WordDictionaryEnd As Decimal = CaptionNumCanNotCheck

    'MessageWord
    Private Const WordDictionaryMsg As Decimal = 801

    '画面ID
    Private Const CProgramId As String = "SC3160218"
    Private Const CSettingSection As String = "ExteriorCheck"
    Private Const CSettingKeyZoomRate As String = "ZoomRate"
    Private Const CSettingKeySuperDomain As String = "SuperDomain"

    'LoginUserID
    Private Const CLOGINUSERID_MAX_LEN As Integer = 20

    '来店実績連番
    Private Const CVISIT_SEQ_ERR As Decimal = -1
    Private Const CVISIT_SEQ_MAX_LEN As Integer = 10

    'RO番号
    Private Const CR_O_INIT As String = " "
    Private Const CR_O_MAX_LEN As Integer = 10

    'ViewMode
    Private Const CVIEWMODE_MAX_LEN As Integer = 1


    'BASREZID
    Private Const CBASREZID_INIT As String = " "
    Private Const CBASREZID_MAX_LEN As Integer = 10

    'SEQ_NO
    Private Const CSEQ_NO_ERR As Decimal = 0
    Private Const CSEQ_NO_MAX_LEN As Integer = 2

    'VIN_NO
    Private Const CVIN_NO_INIT As String = " "
    Private Const CVIN_NO_MAX_LEN As Integer = 20

    'Getパラメータ一覧
    Private ReadOnly MandatoryArgsParamList As ReadOnlyCollection(Of String) = _
        Array.AsReadOnly(New String() {
                         "DealerCode",
                         "BranchCode",
                         "LoginUserID",
                         "VIN_NO",
                         "ViewMode"
                         })
#End Region


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Logger.Info("Pages_SC3160218.Page_Load function Begin.")

        If Not IsPostBack Then
            Logger.Info("Request URL=" & Request.RawUrl)

            Dim argParam As New SC3160218BusinessLogic.ParamInfo
            Dim wordDic As New Dictionary(Of Decimal, String)
            Dim exteriorId As Decimal = -1

            '文言取得(パフォーマンスを考慮しログ出力は行わない)
            For displayId As Decimal = WordDictionaryBegin To WordDictionaryEnd
                wordDic.Add(displayId, WebWordUtility.GetWord(CProgramId, displayId))
            Next
            wordDic.Add(WordDictionaryMsg, WebWordUtility.GetWord(CProgramId, WordDictionaryMsg))

            ' 文言設定
            _localizeWord(wordDic)

            If Not _checkParam(Request, argParam) Then
                Throw New ApplicationException("パラメータエラー")
            End If

            exteriorId = SC3160218BusinessLogic.GetExteriorId(argParam)
            If 0 > exteriorId And argParam.ViewMode = 0 Then
                '新規データ(既存データなし)
                exteriorId = SC3160218BusinessLogic.AddExteriorDamageInfo(argParam)
            End If

            'クライアント側の初期化処理
            _initializeClient(exteriorId, wordDic)

        End If

        Logger.Info("Pages_SC3160218.Page_Load function end.")
    End Sub

    ''' <summary>
    ''' 凡例データ取得
    ''' </summary>
    ''' <returns>凡例データ(JSON文字列)</returns>
    ''' <remarks></remarks>
    <System.Web.Services.WebMethod()> _
    Public Shared Function getExplanation() As String
        Logger.Info("Pages_SC3160218.GetExplanation function Begin.")
        Dim result As String = ""

        Dim explanationList As ArrayList
        explanationList = SC3160218BusinessLogic.GetExplanationInfo()

        Dim json As New StringBuilder
        json.Append("[")
        For Each info As SC3160218BusinessLogic.ExplanationInfo In explanationList
            With json
                .Append("{""type"":""" & info.type & """,")
                .Append("""title"":""" & WebWordUtility.GetWord(CProgramId, info.word_num) & """,")
                .Append("""from"":""" & info.fromColor & """,")
                .Append("""to"":""" & info.toColor & """}")
            End With

            If explanationList.IndexOf(info) < (explanationList.Count - 1) Then
                json.Append(",")
            End If
        Next
        json.Append("]")

        result = json.ToString

        Logger.Info("Pages_SC3160218.GetExplanation function end.")
        Return result
    End Function

    ''' <summary>
    ''' ダメージ情報取得
    ''' </summary>
    ''' <param name="id">RO外装ID</param>
    ''' <returns>ダメージ情報(JSON文字列)</returns>
    ''' <remarks></remarks>
    <System.Web.Services.WebMethod()> _
    Public Shared Function GetDamageInfo(ByVal id As String) As String
        Logger.Info("Pages_SC3160218.GetDamageInfo function Begin.")
        Dim result As String = "{}"
        Dim damageInfo As New SC3160218BusinessLogic.DamageInfo

        Dim bFound As Boolean = SC3160218BusinessLogic.GetExteriorDamageInfo(Decimal.Parse(id), damageInfo)
        If bFound Then
            result = _getDamageInfoJson(damageInfo)
        End If
        Logger.Info(result)

        Logger.Info("Pages_SC3160218.GetDamageInfo function end.")
        Return result
    End Function

    ''' <summary>
    ''' NoDamageCheckフラグ設定処理
    ''' </summary>
    ''' <param name="id">外装ID</param>
    ''' <param name="value">NoDamageフラグ</param>
    ''' <param name="userId">更新者</param>
    ''' <remarks></remarks>
    <System.Web.Services.WebMethod()> _
    Public Shared Sub SetNoDamage(ByVal id As String,
                                  ByVal value As String,
                                  ByVal userId As String)
        Logger.Info("Pages_SC3160218.SetNoDamage function Begin.")

        SC3160218BusinessLogic.UpdateNoDamage(Decimal.Parse(id), Boolean.Parse(value), userId)

        Logger.Info("Pages_SC3160218.SetNoDamage function end.")
    End Sub

    ''' <summary>
    ''' Can't checkフラグ設定処理
    ''' </summary>
    ''' <param name="id">外装ID</param>
    ''' <param name="value">Can't checkフラグ</param>
    ''' <param name="userId">更新者</param>
    ''' <remarks></remarks>
    <System.Web.Services.WebMethod()> _
    Public Shared Sub SetCanNotCheck(ByVal id As String,
                                     ByVal value As String,
                                     ByVal userId As String)
        Logger.Info("Pages_SC3160218.SetCanNotCheck function Begin.")

        SC3160218BusinessLogic.UpdateCanNotCheck(Decimal.Parse(id), Boolean.Parse(value), userId)

        Logger.Info("Pages_SC3160218.SetCanNotCheck function end.")
    End Sub

    ''' <summary>
    ''' パラメータチェック
    ''' </summary>
    ''' <param name="args">リクエストパラメータ</param>
    ''' <param name="outArgParam">チェック済みパラメータデータ</param>
    ''' <returns>true:正常,false:異常</returns>
    ''' <remarks></remarks>
    Private Function _checkParam(ByRef args As HttpRequest,
                                 ByRef outArgParam As SC3160218BusinessLogic.ParamInfo) As Boolean
        Logger.Info("Pages_SC3160218._checkParam function begin.")
        Dim tmp As Long
        Dim tmpDecimal As Decimal
        Dim icropDlrCd As String = ""
        Dim icropStrCd As String = ""
        Dim bCondition As Boolean = False

        'パラメータの必須項目チェック
        For Each column As String In MandatoryArgsParamList
            If String.IsNullOrWhiteSpace(args(column)) Then
                Logger.Info("Pages_SC3160218._checkParam not found=" & column & ".")
                Logger.Info("Pages_SC3160218._checkParam function end.")
                Return False
            End If
        Next

        'ビューモード
        If CVIEWMODE_MAX_LEN >= args("ViewMode").Length Then
            If Long.TryParse(args("ViewMode"), tmp) Then
                ' 0でも1でもない場合
                If 0 <> tmp And 1 <> tmp Then
                    Logger.Info("Pages_SC3160218._checkParam ViewMode faild=" & tmp & ".")
                    ' Readonlyで動いてもらう
                    tmp = 1
                End If
                outArgParam.ViewMode = tmp
            Else
                ' パラメータ不足
                Logger.Info("Pages_SC3160218._checkParam ViewMode?=" & args("ViewMode") & ".")
                Return False
            End If
        Else
            Logger.Info("Pages_SC3160218._checkParam ViewMode orver len=" & args("ViewMode") & ".")
            Return False
        End If

        '販売店/店舗コード変換
        bCondition = SC3160218BusinessLogic.ChangeDealerOrg2Icrop(args("DealerCode"),
                                                                  args("BranchCode"),
                                                                  icropDlrCd,
                                                                  icropStrCd)
        If Not bCondition Then
            ' 販売店/店舗コード変換失敗
            Logger.Info("Pages_SC3160218._checkParam change error dlr=" & args("DealerCode") & ", str=" & args("BranchCode") & ".")
            ' Viewモードが0の時、エラー終了
            If outArgParam.ViewMode = 0 Then
                Return False
            End If
        End If
        outArgParam.DealerCode = icropDlrCd
        outArgParam.BranchCode = icropStrCd

        'LoginUserID
        If CLOGINUSERID_MAX_LEN < args("LoginUserID").Length Then
            Logger.Info("Pages_SC3160218._checkParam LoginUserID orver len=" & args("LoginUserID") & ".")
            Return False
        End If
        outArgParam.LoginUserID = args("LoginUserID")

        '来店実績連番
        outArgParam.VISIT_SEQ = CVISIT_SEQ_ERR
        Dim len = args("SAChipID").Length
        '空文字チェック
        If 0 <> len Then
            '桁数チェック
            If CVISIT_SEQ_MAX_LEN >= len Then
                '数値変換チェック
                If Decimal.TryParse(args("SAChipID"), tmpDecimal) Then
                    If CVISIT_SEQ_ERR < tmpDecimal Then
                        ' 正常値
                        outArgParam.VISIT_SEQ = tmpDecimal
                    End If
                Else
                    '数値以外はNG
                    Logger.Info("Pages_SC3160218._checkParam SAChipID=" & args("SAChipID") & ".")
                    Return False
                End If
            Else
                '桁溢れはNG
                Logger.Info("Pages_SC3160218._checkParam SAChipID=" & args("SAChipID") & ".")
                Return False
            End If
        End If
        If CVISIT_SEQ_ERR = outArgParam.VISIT_SEQ Then
            ' 来店実績連番が付かない場合が存在するためログ出力に留める
            Logger.Info("Pages_SC3160218._checkParam SAChipID warn=" & args("SAChipID") & ".")
        End If

        ' RO番号
        outArgParam.R_O = CR_O_INIT
        '文字数チェック
        If Not String.IsNullOrWhiteSpace(args("R_O")) Then
            If CR_O_MAX_LEN >= args("R_O").Length Then
                outArgParam.R_O = args("R_O")
            Else
                Logger.Info("Pages_SC3160218._checkParam R_O=" & args("R_O") & ".")
                Return False
            End If
        End If
        If CR_O_INIT = outArgParam.R_O Then
            ' RO番号が付かない場合が存在するためログ出力に留める
            Logger.Info("Pages_SC3160218._checkParam R_O warn=" & args("R_O") & ".")
        End If

        'パラメータの必須項目チェック
        If CVISIT_SEQ_ERR = outArgParam.VISIT_SEQ And CR_O_INIT = outArgParam.R_O Then
            Logger.Info("Pages_SC3160218._checkParam SAChipID=" & args("SAChipID") & ", R_O=" & args("R_O") & ".")
            Return False
        End If

        '凡例表示フラグ
        If Not String.IsNullOrWhiteSpace(args("LegendDisp")) Then
            If Long.TryParse(args("LegendDisp"), tmp) Then
                If 0 <> tmp And 1 <> tmp Then
                    Logger.Info("Pages_SC3160218._checkParam LegendDisp faild=" & tmp & ".")
                    ' 表示で動いてもらう
                    outArgParam.LegendDisp = 0
                End If
                outArgParam.LegendDisp = tmp
            Else
                ' 表示で動いてもらう
                outArgParam.LegendDisp = 0
            End If
        Else
            ' 省略時
            outArgParam.LegendDisp = 0
        End If

        'チェックボックス表示フラグ
        If Not String.IsNullOrWhiteSpace(args("CheckboxDisp")) Then
            If Long.TryParse(args("CheckboxDisp"), tmp) Then
                If 0 <> tmp And 1 <> tmp Then
                    Logger.Info("Pages_SC3160218._checkParam CheckboxDisp faild=" & tmp & ".")
                    ' 表示で動いてもらう
                    tmp = 0
                End If
                outArgParam.CheckboxDisp = tmp
            Else
                ' 表示で動いてもらう
                outArgParam.CheckboxDisp = 0
            End If
        Else
            ' 省略時
            outArgParam.CheckboxDisp = 0
        End If

        'スケールモード
        If Not String.IsNullOrWhiteSpace(args("ScaleMode")) Then
            If Long.TryParse(args("ScaleMode"), tmp) Then
                If 0 <> tmp And 1 <> tmp Then
                    Logger.Info("Pages_SC3160218._checkParam ScaleMode faild=" & tmp & ".")
                    ' デフォルトで動いてもらう
                    tmp = 0
                End If
                outArgParam.ScaleMode = tmp
            Else
                ' デフォルトで動いてもらう
                outArgParam.ScaleMode = 0
            End If
        Else
            ' 省略時
            outArgParam.ScaleMode = 0
        End If

        'BASREZID
        outArgParam.BASREZID = CBASREZID_INIT
        If Not String.IsNullOrWhiteSpace(args("BASREZID")) Then
            If CBASREZID_MAX_LEN >= args("BASREZID").Length Then
                outArgParam.BASREZID = args("BASREZID")
            End If
        End If

        'SEQ_NO(外観図ではRO枝番号は必ず親番号「0」とする)
        outArgParam.SEQ_NO = CSEQ_NO_ERR

        'VIN_NO
        outArgParam.VIN_NO = CVIN_NO_INIT
        If Not String.IsNullOrWhiteSpace(args("VIN_NO")) Then
            If CVIN_NO_MAX_LEN >= args("VIN_NO").Length Then
                outArgParam.VIN_NO = args("VIN_NO")
            Else
                Logger.Info("Pages_SC3160218._checkParam error VIN_NO=" & args("VIN_NO") & ".")
                Return False
            End If
        End If

        Logger.Info("Pages_SC3160218._checkParam function end.")
        Return True
    End Function

    ''' <summary>
    ''' クライアント初期化
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub _initializeClient(ByVal exteriorId As Decimal,
                                  ByVal dic As Dictionary(Of Decimal, String))
        Logger.Info("Pages_SC3160218._initializeClient function begin.")
        Dim initilizeScript As New StringBuilder
        Dim confirmMsg As String = Server.HtmlEncode(dic(WordDictionaryMsg))

        Dim zoomRate As String = SC3160218BusinessLogic.GetProgramSetting(CProgramId, CSettingSection, CSettingKeyZoomRate)
        Dim superDomain As String = SC3160218BusinessLogic.GetProgramSetting(CProgramId, CSettingSection, CSettingKeySuperDomain)

        With initilizeScript
            ' JS生成
            .Append("<script type='text/javascript'>")
            ' 確認メッセージ
            .Append(" var C_ConfirmMsg = """ & confirmMsg & """;")
            ' 外装ID設定
            .Append(" var C_ExteriorId = " & exteriorId & ";")
            ' 外観チェックズーム率
            .Append(" var C_ZOOM_RATE = " & zoomRate & ";")
            ' スーパードメイン設定
            .Append(" document.domain = """ & superDomain & """;")
            .Append("</script>")
        End With

        Dim cs As ClientScriptManager = Page.ClientScript
        cs.RegisterStartupScript(Me.GetType, "init", initilizeScript.ToString)

        Logger.Info("Pages_SC3160218._initializeClient function end.")
    End Sub

    ''' <summary>
    ''' ダメージ情報をJson化する
    ''' </summary>
    ''' <param name="damageInfo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Shared Function _getDamageInfoJson(ByVal damageInfo As SC3160218BusinessLogic.DamageInfo) As String
        Logger.Info("Pages_SC3160218._getDamageInfoJson function begin.")
        Dim resultJson As New StringBuilder

        With resultJson
            ' ダメージ情報設定
            .Append("{""NO_DAMAGE_FLG"": """ & damageInfo.NO_DAMAGE_FLG & """,")
            .Append(" ""CANNOT_CHECK_FLG"": """ & damageInfo.CANNOT_CHECK_FLG & """,")
            .Append(" ""data"": [")
        End With

        ''損傷データ
        For Each row As SC3160218BusinessLogic.DamageData In damageInfo.data
            With resultJson
                .Append("{""PARTS_TYPE"": """ & row.PARTS_TYPE & """,")
                .Append(" ""RO_THUMBNAIL_ID"": """ & row.RO_THUMBNAIL_ID & """,")
                .Append(" ""DAMAGE_TYPE_1"": """ & row.DAMAGE_TYPE_1 & """,")
                .Append(" ""DAMAGE_TYPE_2"": """ & row.DAMAGE_TYPE_2 & """}")
            End With

            If damageInfo.data.IndexOf(row) < (damageInfo.data.Count - 1) Then
                resultJson.Append(",")
            End If
        Next
        resultJson.Append("]}")

        Logger.Info("Pages_SC3160218._getDamageInfoJson function end.")
        Return resultJson.ToString
    End Function

    ''' <summary>
    ''' 文言管理にDB登録を行い文言番号より取得する
    ''' </summary>
    ''' <param name="dic">文言管理配列</param>
    ''' <remarks></remarks>
    Private Sub _localizeWord(ByVal dic As Dictionary(Of Decimal, String))
        Logger.Info("Pages_SC3160218._localizeWord function begin.")

        ' タイトル(Exterior Check)
        StaticPageTitle.Text = Server.HtmlEncode(dic(CaptionNumTitle))
        ' NoDamageチェックボックスキャプション
        StaticNoDamage.Text = Server.HtmlEncode(dic(CaptionNumNoDamage))
        ' Can'tCheckチェックボックスキャプション
        StaticCanNotCheck.Text = Server.HtmlEncode(dic(CaptionNumCanNotCheck))

        Logger.Info("Pages_SC3160218._localizeWord function end.")
    End Sub

End Class
