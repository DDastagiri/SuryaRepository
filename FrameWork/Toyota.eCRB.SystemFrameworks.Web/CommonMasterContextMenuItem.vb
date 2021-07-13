Imports System.Web.UI.WebControls
Imports Toyota.eCRB.SystemFrameworks.Web.Controls
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic

Namespace Toyota.eCRB.SystemFrameworks.Web

    ''' <summary>
    ''' 組み込みコンテキストメニュー項目のID
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum CommonMasterContextMenuBuiltinMenuID
        ''' <summary>
        ''' 既定値
        ''' </summary>
        None = 0
        ''' <summary>
        ''' ログアウト
        ''' </summary>
        LogoutItem = 100
        ''' <summary>
        ''' スタンバイ
        ''' </summary>
        StandByItem = 101
        ''' <summary>
        ''' 一時退席
        ''' </summary>
        SuspendItem = 102
    End Enum

    ''' <summary>
    ''' ページ用マスターページのコンテキストメニュー項目を表すクラスです。
    ''' </summary>
    Public Class CommonMasterContextMenuItem

        ''' <summary>
        ''' コンテキストメニュー項目の識別子を取得します。
        ''' </summary>
        Public ReadOnly Property ID As Integer
            Get
                Return _id
            End Get
        End Property

        ''' <summary>
        ''' コンテキストメニュー項目の表示名を取得または設定します。
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Text As String
            Get
                Return _owner.Text
            End Get
            Set(value As String)
                _owner.Text = value
            End Set
        End Property

        ''' <summary>
        ''' コンテキストメニュー項目の状態（表示／非表示）を取得または設定します。
        ''' </summary>
        Public Property Visible As Boolean
            Get
                Return _owner.Visible
            End Get
            Set(value As Boolean)
                _owner.Visible = value
            End Set
        End Property

        ''' <summary>
        ''' コンテキストメニュー項目の状態（有効／無効）を取得または設定します。
        ''' </summary>
        Public Property Enabled As Boolean
            Get
                Return _owner.Enabled
            End Get
            Set(value As Boolean)
                _owner.Enabled = value
            End Set
        End Property

        ''' <summary>
        ''' コンテキストメニュー項目の選択時に呼び出されるJavaScript関数名を取得または設定します。
        ''' </summary>
        Public Property OnClientClick As String
            Get
                Return _owner.OnClientClick
            End Get
            Set(value As String)
                _owner.OnClientClick = value
            End Set
        End Property

        ''' <summary>
        ''' コンテキストメニュー項目選択時に、基盤によって更新される「在席状態（大分類）」を取得または設定します。
        ''' </summary>
        Public Property PresenceCategory As String
            Get
                Return _owner.Attributes("data-presenceCategory")
            End Get
            Set(value As String)
                _owner.Attributes("data-presenceCategory") = value
            End Set
        End Property

        ''' <summary>
        ''' コンテキストメニュー項目選択時に、基盤によって更新される「在席状態（小分類）」を取得または設定します。
        ''' </summary>
        Public Property PresenceDetail As String
            Get
                Return Owner.Attributes("data-presenceDetail")
            End Get
            Set(value As String)
                Owner.Attributes("data-presenceDetail") = value
            End Set
        End Property

        ''' <summary>
        ''' コンテキストメニュー項目選択時に発生します。
        ''' </summary>
        Public Event Click As EventHandler



        Public Sub New(ByVal owner As CustomHyperLink, ByVal id As Integer)
            _owner = owner
            _id = id
        End Sub

        Public ReadOnly Property Owner As CustomHyperLink
            Get
                Return _owner
            End Get
        End Property

        Public Sub OnClick()
            RaiseEvent Click(Me, New EventArgs())
        End Sub



        Private _owner As CustomHyperLink
        Private _id As Integer
    End Class
End Namespace

