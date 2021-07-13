'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3100103.ascx.vb
'──────────────────────────────────
'機能： スタンバイスタッフ並び順変更
'補足： 
'作成： 2012/08/17 TMEJ m.okamura
'──────────────────────────────────

Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core

''' <summary>
''' SC3100103
''' スタンバイスタッフ並び順変更 プレゼンテーション層クラス
''' </summary>
''' <remarks></remarks>
Partial Class Pages_SC3100103_Control
    Inherits System.Web.UI.UserControl

#Region "定数"

    ''' <summary>
    ''' 機能ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const AppId As String = "SC3100103"

#Region "文言ID(項目)"

    ''' <summary>
    ''' スタンバイスタッフ並び順変更
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdTitle As Integer = 1

    ''' <summary>
    ''' キャンセル
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdCancel As Integer = 2

    ''' <summary>
    ''' 登録
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdRegister As Integer = 3

#End Region

#End Region

#Region "プロパティ"

    ''' <summary>
    ''' UCにセットした情報を格納
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Property TriggerClientId_ As String

    ''' <summary>
    ''' キックする値のプロパティ
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property TriggerClientId() As String

        Get
            Return TriggerClientId_
        End Get

        Set(ByVal value As String)
            TriggerClientId_ = value
        End Set

    End Property

#End Region

#Region "イベント処理"

#Region "ページロード時の処理"

    ''' <summary>
    ''' ページロード時の処理です。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Logger.Info("Page_Load_Start")

        'トリガーになる値を設定する
        Me.StandByStaffPopOverForm.TriggerClientId = TriggerClientId

        ' PostBack時、初期表示処理は行わない。
        If Me.IsPostBack Then

            Logger.Info("Page_Load_End PostBack")
            Return

        End If

        '文言を設定する
        Me.InitWord()

        Logger.Info("Page_Load_End")

    End Sub

#End Region

#End Region

#Region "非公開メソッド"

#Region "文言管理"

    ''' <summary>
    ''' 文言管理にDB登録を行い文言番号より取得する
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitWord()

        Logger.Info("InitWord_Start")

        'タイトル、キャンセルボタン、登録ボタンの設定
        Me.StandByStaffWordTitle.Value = Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordIdTitle))
        Me.StandByStaffWordCancel.Value = Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordIdCancel))
        Me.StandByStaffWordRegister.Value = Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordIdRegister))

        Logger.Info("InitWord_End")

    End Sub

#End Region

#End Region

End Class
