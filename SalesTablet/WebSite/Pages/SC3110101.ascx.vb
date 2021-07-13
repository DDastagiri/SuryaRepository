Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core

Partial Class Pages_SC3110101_Control
    Inherits System.Web.UI.UserControl

#Region "定数"

    ''' <summary>
    ''' 機能ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const AppId As String = "SC3110101"

#Region "文言ID(項目)"

    ''' <summary>
    ''' 試乗車空き状況
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
    Private Const WordIdSubmit As Integer = 3

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

#Region "初期表示"

    ''' <summary>
    ''' フォームロード時のイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Logger.Info("Page_Load_Start")

        'トリガーになる値を設定する
        Me.TestDrivePopOverForm.TriggerClientId = TriggerClientId

        ' PostBack時、初期表示処理は行わない。
        If Me.IsPostBack Then

            Logger.Info("Page_Load_End PostBack")
            Return
        End If

        'ログイン情報チェック
        Logger.Info("Page_Load_001 " & "Call_Start StaffContext.Current")
        Dim loginStaff As StaffContext = StaffContext.Current
        Logger.Info("Page_Load_001 " & "Call_End   StaffContext.Current")

        'この情報を使って権限の指定をする
        Me.opeCd.Value = loginStaff.OpeCD

        '文言を設定する
        Me.SetWord()

        Logger.Info("Page_Load_End")

    End Sub

#End Region

#Region "文言セット"

    ''' <summary>
    ''' 文言をセットする
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetWord()

        Logger.Info("SetWord_Start")

        'タイトル、キャンセルボタン、登録ボタンの設定
        Me.wordTitle.Value = WebWordUtility.GetWord(AppId, WordIdTitle)
        Me.wordCancel.Value = WebWordUtility.GetWord(AppId, WordIdCancel)
        Me.wordSubmit.Value = WebWordUtility.GetWord(AppId, WordIdSubmit)

        Logger.Info("SetWord_End")
    End Sub

#End Region

End Class
