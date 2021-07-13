'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3170210.aspx.vb
'─────────────────────────────────────
'機能： 写真選択画面
'補足： RO作成機能グローバル連携処理
'作成： 2013/12/25 SKFC 久代 
'作成： 2018/10/23 SKFC 上田 
'更新： 
'─────────────────────────────────────
Option Strict On
Option Explicit On

Imports System.Collections.ObjectModel
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.iCROP.BizLogic.SC3170210

''' <summary>
''' SC3170210画面クラス
''' </summary>
''' <remarks></remarks>
Partial Class Pages_SC3170210
    Inherits BasePage
    Implements IDisposable

#Region "定数"
    ''' <summary>
    ''' SAChipID最大文字数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const _SACHIPID_MAX_LEN As Integer = 10

    ''' <summary>
    ''' R_O最大文字数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const _R_O_MAX_LEN As Integer = 10

    ''' <summary>
    ''' SEQ_NO最大文字数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const _SEQ_NO_MAX_LEN As Integer = 10

    ''' <summary>
    ''' PictMode最大文字数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const _PICT_MODE_MAX_LEN As Integer = 1

    ''' <summary>
    ''' LinkSysType最大文字数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const _LINK_SYS_TYPE_MAX_LEN As Integer = 1

    ''' <summary>
    ''' LoginUserID最大文字数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const _LOGIN_USER_ID_MAX_LEN As Integer = 20

    ''' <summary>
    ''' BASREZID最大文字数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const _BASREZID_MAX_LEN As Integer = 10

    ''' <summary>
    ''' VIN_NO最大文字数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const _VIN_NO_MAX_LEN As Integer = 20


    '文言番号
    Private Const _CaptionCancelBtn As Decimal = 0
    Private Const _CaptionRegistBtn As Decimal = 1
    Private Const _CaptionDeleteBtn As Decimal = 2
    Private Const _WordDictionaryBegin As Decimal = _CaptionCancelBtn
    Private Const _WordDictionaryEnd As Decimal = _CaptionDeleteBtn
    Private Const _ProgramId As String = "SC3170210"
#End Region

#Region "DTOクラス"
    ''' <summary>
    ''' パラメータクラス
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ArgParam
        ''' <summary>
        ''' 基幹販売店コード
        ''' </summary>
        ''' <remarks></remarks>
        Public DealerCode As String = ""

        ''' <summary>
        ''' 基幹店舗コード
        ''' </summary>
        ''' <remarks></remarks>
        Public BranchCode As String = ""

        ''' <summary>
        ''' 来店実績連番
        ''' </summary>
        ''' <remarks></remarks>
        Public SAChipID As Long = -1

        ''' <summary>
        ''' RO番号
        ''' </summary>
        ''' <remarks></remarks>
        Public R_O As String = ""

        ''' <summary>
        ''' RO枝番
        ''' </summary>
        ''' <remarks></remarks>
        Public SEQ_NO As Long = -1

        ''' <summary>
        ''' 写真区分
        ''' </summary>
        ''' <remarks></remarks>
        Public PictMode As String = ""

        ''' <summary>
        ''' LinkSysType
        ''' </summary>
        ''' <remarks></remarks>
        Public LinkSysType As String = ""

        ''' <summary>
        ''' ログインユーザID
        ''' </summary>
        ''' <remarks></remarks>
        Public LoginUserID As String = ""

        ''' <summary>
        ''' 基幹予約ID
        ''' </summary>
        ''' <remarks></remarks>
        Public BASREZID As String = ""

        ''' <summary>
        ''' VIN
        ''' </summary>
        ''' <remarks></remarks>
        Public VIN_NO As String = ""

        ''' <summary>
        ''' パラメータチェックフラグ
        ''' </summary>
        ''' <remarks>True:OK,False:NG</remarks>
        Public IsCheck As Boolean = True
    End Class
#End Region



    ''' <summary>
    ''' PageLoadインベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Logger.Info("Pages_SC3170210.Page_Load function Begin.")
        If Not IsPostBack Then
            Logger.Info("Request URL=" & Request.RawUrl)

            Dim wordDic As New Dictionary(Of Decimal, String)

            '文言取得(パフォーマンスを考慮しログ出力は行わない)
            For displayId As Decimal = _WordDictionaryBegin To _WordDictionaryEnd
                wordDic.Add(displayId, WebWordUtility.GetWord(_ProgramId, displayId))
            Next

            ' 文言設定
            _localizeWord(wordDic)

            _initializeClient()

        End If
        Logger.Info("Pages_SC3170210.Page_Load function End.")
    End Sub

#Region "公開メソッド"
    ''' <summary>
    ''' サムネイル情報取得
    ''' </summary>
    ''' <param name="DealerCode">販売店コード</param>
    ''' <param name="BranchCode">店舗コード</param>
    ''' <param name="SAChipID">来店実績連番</param>
    ''' <param name="R_O">RO番号</param>
    ''' <param name="SEQ_NO">RO枝番</param>
    ''' <param name="PictMode">写真区分</param>
    ''' <param name="LinkSysType">LinkSysType</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.Web.Services.WebMethod()> _
    Public Shared Function GetRoThumbnail(ByVal DealerCode As String,
                                          ByVal BranchCode As String,
                                          ByVal SAChipID As String,
                                          ByVal R_O As String,
                                          ByVal SEQ_NO As String,
                                          ByVal VIN_NO As String,
                                          ByVal PictMode As String,
                                          ByVal ViewMode As String,
                                          ByVal BASREZID As String,
                                          ByVal LinkSysType As String,
                                          ByVal LoginUserID As String) As String
        Logger.Info("Pages_SC3170210.GetRoThumbnail function Begin.")

        ' パラメータチェック
        Dim param As ArgParam
        '2018/01/24 Mod Start【iOS11 課題№13】
        param = _GetArgParam(DealerCode,
                             BranchCode,
                             SAChipID,
                             R_O,
                             SEQ_NO,
                             HttpUtility.UrlDecode(VIN_NO),
                             PictMode,
                             ViewMode,
                             BASREZID,
                             LinkSysType,
                             LoginUserID)
        '2018/01/24 Mod End
        If Not param.IsCheck Then
            Throw New ApplicationException("パラメータ不正")
        End If

        ' サムネイル情報取得
        Dim thumbList As ArrayList
        thumbList = SC3170210BusinessLogic.GetRoThumbnail(param.DealerCode,
                                                          param.BranchCode,
                                                          param.SAChipID,
                                                          param.R_O,
                                                          param.SEQ_NO,
                                                          param.PictMode,
                                                          param.LinkSysType)
        ' サムネイル情報をJSON文字列化
        Dim resultJson As StringBuilder = New StringBuilder
        resultJson.Append("{ ""data"": [")
        For Each row As SC3170210BusinessLogic.ThumbnailData In thumbList
            With resultJson
                .Append("{""id"": """ & row.id.ToString & """,")
                .Append(" ""title"": """ & row.partsTitle & """,")
                .Append(" ""dbImgPath"": """ & row.dbImgPath & """,")
                .Append(" ""orignalImgPath"": """ & row.orignalImgPath & """,")
                .Append(" ""largeImgPath"": """ & row.largeImgPath & """,")
                .Append(" ""middleImgPath"": """ & row.middleImgPath & """,")
                .Append(" ""smallImgPath"": """ & row.smallImgPath & """}")
            End With

            If thumbList.IndexOf(row) < (thumbList.Count - 1) Then
                resultJson.Append(",")
            End If
        Next
        resultJson.Append("]}")

        Logger.Info("Pages_SC3170210.GetRoThumbnail function End.")
        Return resultJson.ToString
    End Function

    ''' <summary>
    ''' サムネイル削除
    ''' </summary>
    ''' <param name="id">サムネイル情報ID</param>
    ''' <param name="LoginUserID">更新ユーザ</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.Web.Services.WebMethod()> _
    Public Shared Function DeleteRoThumbnail(ByVal id As String,
                                             ByVal LoginUserID As String) As Integer
        Logger.Info("Pages_SC3170210.DeleteRoThumbnail function Begin.")
        Dim result As Integer = -1

        ' サムネイル情報IDをDecimal変換
        Dim RO_THUMBNAIL_ID As Decimal
        If Decimal.TryParse(id, RO_THUMBNAIL_ID) Then
            ' サムネイル情報削除
            result = SC3170210BusinessLogic.DeleteRoThubnail(RO_THUMBNAIL_ID, LoginUserID)
        End If


        Logger.Info("Pages_SC3170210.DeleteRoThumbnail function End.")
        Return result
    End Function
#End Region

#Region "非公開メソッド"
    ''' <summary>
    ''' パラメータチェック
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function _checkParam() As ArgParam
        Logger.Info("Pages_SC3170210._checkParam function Begin.")

        Dim result As ArgParam
        result = _GetArgParam(Request("DealerCode"),
                           Request("BranchCode"),
                           Request("SAChipID"),
                           Request("R_O"),
                           Request("SEQ_NO"),
                           Request("VIN_NO"),
                           Request("PictMode"),
                           Request("ViewMode"),
                           Request("BASREZID"),
                           Request("LinkSysType"),
                           Request("LoginUserID")
                           )

        Logger.Info("Pages_SC3170210._checkParam function End.")

        Return result
    End Function

    ''' <summary>
    ''' パラメーラDTO取得(パラメータチェック付き)
    ''' </summary>
    ''' <param name="DealerCode">基幹販売店コード</param>
    ''' <param name="BranchCode">基幹店舗コード</param>
    ''' <param name="SAChipID">来店実績連番</param>
    ''' <param name="R_O">RO番号</param>
    ''' <param name="SEQ_NO">RO枝番</param>
    ''' <param name="PictMode">写真区分</param>
    ''' <param name="LinkSysType">LinkSysType</param>
    ''' <param name="LoginUserID">ログインユーザID</param>
    ''' <param name="BASREZID">基幹予約ID</param>
    ''' <param name="VIN_NO">VIN</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Shared Function _GetArgParam(ByVal DealerCode As String,
                                         ByVal BranchCode As String,
                                         ByVal SAChipID As String,
                                         ByVal R_O As String,
                                         ByVal SEQ_NO As String,
                                         Optional ByVal VIN_NO As String = " ",
                                         Optional ByVal PictMode As String = "1",
                                         Optional ByVal ViewMode As String = "0",
                                         Optional ByVal BASREZID As String = " ",
                                         Optional ByVal LinkSysType As String = "0",
                                         Optional ByVal LoginUserID As String = " "
                                         ) As ArgParam
        Logger.Info("Pages_SC3170210._GetArgParam function Begin.")
        Dim result As ArgParam = New ArgParam

        ' 必須項目チェック
        ' 販売店コード
        If String.IsNullOrEmpty(DealerCode) Then
            Logger.Info("Mandatory parameter DealerCode not found.")
            result.IsCheck = False
        End If
        result.DealerCode = DealerCode

        ' 店舗コード
        If String.IsNullOrEmpty(BranchCode) Then
            Logger.Info("Mandatory parameter BranchCode not found.")
            result.IsCheck = False
        End If
        result.BranchCode = BranchCode

        ' 来店実績連番かRO番号どちらかが無い場合はエラー
        If String.IsNullOrEmpty(SAChipID) And String.IsNullOrEmpty(R_O) Then
            Logger.Info("SAChipID and R_O not found.")
            result.IsCheck = False
        End If


        ' 来店実績連番
        If Not String.IsNullOrEmpty(SAChipID) Then
            If Not Long.TryParse(SAChipID, result.SAChipID) Then
                Logger.Info(String.Format("SAChipID Parse error {0}.", SAChipID))
                result.IsCheck = False
            ElseIf _SACHIPID_MAX_LEN < SAChipID.Length Then
                Logger.Info("SAChipID over length.")
                result.IsCheck = False
            End If
        End If

        ' RO番号
        If _R_O_MAX_LEN < R_O.Length Then
            Logger.Info("R_O over length.")
            result.IsCheck = False
        End If
        result.R_O = R_O

        ' RO枝番
        If Not String.IsNullOrEmpty(SEQ_NO) Then
            If Not Long.TryParse(SEQ_NO, result.SEQ_NO) Then
                Logger.Info(String.Format("SEQ_NO Parse error {0}.", SEQ_NO))
                result.IsCheck = False
            ElseIf _SEQ_NO_MAX_LEN < SEQ_NO.Length Then
                Logger.Info("SEQ_NO over length.")
                result.IsCheck = False
            End If
        End If

        ' 写真区分
        If _PICT_MODE_MAX_LEN < PictMode.Length Then
            Logger.Info("PictMode over length.")
            result.IsCheck = False
        End If
        result.PictMode = PictMode

        ' LinkSysType
        If _LINK_SYS_TYPE_MAX_LEN < LinkSysType.Length Then
            Logger.Info("LinkSysType over length.")
            result.IsCheck = False
        End If
        result.LinkSysType = LinkSysType

        ' ログインユーザ
        If _LOGIN_USER_ID_MAX_LEN < LoginUserID.Length Then
            Logger.Info("LoginUserID over length.")
            result.IsCheck = False
        End If
        result.LoginUserID = LoginUserID

        ' 予約ID
        If _BASREZID_MAX_LEN < BASREZID.Length Then
            Logger.Info("BASREZID over length.")
            result.IsCheck = False
        End If
        result.BASREZID = BASREZID

        ' VIN
        If _VIN_NO_MAX_LEN < VIN_NO.Length Then
            Logger.Info("VIN_NO over length.")
            result.IsCheck = False
        End If
        result.VIN_NO = VIN_NO

        Logger.Info("Pages_SC3170210._GetArgParam function End.")
        Return result
    End Function

    ''' <summary>
    ''' クライアント側の初期化
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub _initializeClient()
        Logger.Info("Pages_SC3170210._initializeClient function Begin.")
        Dim initilizeScript As StringBuilder = New StringBuilder

        With initilizeScript
            .Append("<script type='text/javascript'>")
            .Append("</script>")
        End With

        Dim cs As ClientScriptManager = Page.ClientScript
        cs.RegisterStartupScript(Me.GetType, "init", initilizeScript.ToString)

        Logger.Info("Pages_SC3170210._initializeClient function End.")
    End Sub

    ''' <summary>
    ''' 文言管理にDB登録を行い文言番号より取得する
    ''' </summary>
    ''' <param name="dic">文言管理配列</param>
    ''' <remarks></remarks>
    Private Sub _localizeWord(ByVal dic As Dictionary(Of Decimal, String))
        Logger.Info("Pages_SC3160218._localizeWord function begin.")

        ' 解除/削除ボタンキャプション
        StaticCancelBtn.Text = Server.HtmlEncode(dic(_CaptionCancelBtn))
        ' 登録キャプション
        StaticRegistrationBtn.Text = Server.HtmlEncode(dic(_CaptionRegistBtn))
        ' 削除ボタンキャプション
        StaticDeleteBtn.Text = Server.HtmlEncode(dic(_CaptionDeleteBtn))

        Logger.Info("Pages_SC3160218._localizeWord function end.")
    End Sub
#End Region

End Class
