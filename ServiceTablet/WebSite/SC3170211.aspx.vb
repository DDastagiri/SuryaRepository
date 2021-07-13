'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3170211.aspx.vb
'─────────────────────────────────────
'機能： 写真画像ポップアップ表示処理
'補足： 
'作成： 2014/01/29 SKFC 疋田 
'更新： 
'─────────────────────────────────────
Option Strict On
Option Explicit On

Imports System.Globalization
Imports System.Collections.ObjectModel
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.iCROP.BizLogic.SC3170211

''' <summary>
''' SC3170211画面クラス
''' </summary>
''' <remarks></remarks>
Partial Class Pages_SC3170211
    Inherits BasePage
    Implements IDisposable

#Region "定数"

    ''' <summary>
    ''' 画面ID
    ''' </summary>
    Private Const C_FUNCTION_ID As String = "SC3170211"

    ''' <summary>
    ''' 元画面からの引数のキー名：画像ファイルのURL
    ''' </summary>
    Private Const C_REQ_PICT_URL As String = "PictureURL"

    ''' <summary>
    ''' 元画面からの引数のキー名：表示タイトル文字列
    ''' </summary>
    Private Const C_REQ_TITLE_STRING As String = "TitleString"

    ''' <summary>
    ''' 元画面からの引数のキー名：モード（参照モード:0(規定値), 編集モード:1）
    ''' </summary>
    Private Const C_REQ_MODE As String = "Mode"

    ''' <summary>
    ''' 画面モード：参照モード
    ''' </summary>
    Private Const C_MODE_VIEW As String = "0"

    ''' <summary>
    ''' 画面モード：登録モード
    ''' </summary>
    Private Const C_MODE_EDIT As String = "1"

    ''' <summary>
    ''' 閉じるボタン文言番号
    ''' </summary>
    ''' <remarks></remarks>
    Private Const _CaptionCloseBtn As Decimal = 0

#End Region


    ''' <summary>
    ''' PageLoadイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} Start",
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))

        If Not IsPostBack Then
            Logger.Info("Request URL=" & Request.RawUrl)

            Dim pictURL As String = String.Empty
            Dim pictURL2 As String = String.Empty
            Dim mode As String = String.Empty

            If Not GetRequestValue(pictURL, mode) Then
                Throw New ApplicationException("パラメータエラー")
            End If

            'URLからファイル名を取得　「_O.png」も消す
            pictURL2 = pictURL.Remove(0, pictURL.LastIndexOf("/") + 1)
            If 0 <= pictURL2.LastIndexOf("_") Then
                Me.Hidden_FileName.Value = pictURL2.Remove(pictURL2.LastIndexOf("_"))
            ElseIf 0 <= pictURL2.LastIndexOf(".") Then
                Me.Hidden_FileName.Value = pictURL2.Remove(pictURL2.LastIndexOf("."))
            Else
                Me.Hidden_FileName.Value = pictURL2
            End If

            ' パスを抽出
            Dim imagePath As String = SC3170211BusinessLogic.GetImagePath()
            Dim cameraPath As String = pictURL.Replace(imagePath, "")
            Me.Hidden_CameraFilePath.Value = cameraPath.Replace(pictURL2, "")

            '写真表示
            Me.Img_Photo.Src = pictURL

            'タイトル表示
            TitleLabel.Width = 290
            TitleLabel.Text = Hidden_Title.Value

            Dim wordDic As New Dictionary(Of Decimal, String)

            ' 文言設定
            _localizeWord()

        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} End",
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub


#Region "非公開メソッド"

    ''' <summary>
    ''' 呼び出し元からリクエストを取得
    ''' </summary>
    ''' <param name="pictURL">画像ファイルのURL</param>
    ''' <param name="mode">モード</param>
    ''' <remarks></remarks>
    Private Function GetRequestValue(ByRef pictURL As String, ByRef mode As String) As Boolean

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} Start",
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))

        '画像ファイルのURL
        If Not String.IsNullOrWhiteSpace(Request(C_REQ_PICT_URL)) Then
            pictURL = Request(C_REQ_PICT_URL).Trim()
            Me.Hidden_PictURL.Value = pictURL
        Else
            Logger.Error(C_REQ_PICT_URL & ":" & Request(C_REQ_PICT_URL))
            Return False
        End If

        '表示タイトル文字列
        Dim titele As String = String.Empty
        If Not String.IsNullOrWhiteSpace(Request(C_REQ_TITLE_STRING)) Then
            titele = Request(C_REQ_TITLE_STRING).Trim()
            Me.Hidden_Title.Value = titele
        End If

        ' モード
        If Not String.IsNullOrWhiteSpace(Request(C_REQ_MODE)) Then
            mode = Request(C_REQ_MODE).Trim()

            '値がある場合は0か1のみ
            If mode.Equals(C_MODE_VIEW) Or mode.Equals(C_MODE_EDIT) Then
                Me.Hidden_Mode.Value = mode

            Else
                Logger.Error(C_REQ_PICT_URL & ":" & Request(C_REQ_MODE))
                Return False
            End If
        Else
            Me.Hidden_Mode.Value = C_MODE_VIEW
        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} End [PictureURL:{1}][TitleString:{2}][Mode:{3}]",
                                  System.Reflection.MethodBase.GetCurrentMethod.Name,
                                  pictURL, titele, mode))
        Return True

    End Function

    ''' <summary>
    ''' 文言管理にDB登録を行い文言番号より取得する
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub _localizeWord()
        Logger.Info("Pages_SC3160218._localizeWord function begin.")

        ' 閉じるボタンキャプション
        StaticCaptionCloseButton.Text = Server.HtmlEncode(WebWordUtility.GetWord(C_FUNCTION_ID, _CaptionCloseBtn))

        Logger.Info("Pages_SC3160218._localizeWord function end.")
    End Sub
#End Region

End Class
