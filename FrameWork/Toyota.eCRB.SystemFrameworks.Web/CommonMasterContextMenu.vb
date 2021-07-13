Imports Toyota.eCRB.SystemFrameworks.Web.Controls
Imports System.Web.UI.WebControls
Imports Toyota.eCRB.SystemFrameworks.Core

Namespace Toyota.eCRB.SystemFrameworks.Web

    ''' <summary>
    ''' ページ用マスターページのコンテキストメニューを表すクラスです。ヘッダ制御APIを提供します。
    ''' </summary>
    Public Class CommonMasterContextMenu

        ''' <summary>
        ''' コンテキストメニューの状態（有効／無効）を取得または設定します。
        ''' </summary>
        Public Property Enabled As Boolean
            Get
                Return _trigger.Enabled
            End Get
            Set(value As Boolean)
                _trigger.Enabled = value
            End Set
        End Property

        ''' <summary>
        ''' コンテキストメニューの開く／閉じる操作時に、ポストバックを発生させるかどうかを取得または設定します。
        ''' </summary>
        Public Property AutoPostBack As Boolean
            Get
                Return (_owner.Attributes("data-AutoPostBack") IsNot Nothing)
            End Get
            Set(value As Boolean)
                If (value) Then
                    _owner.Attributes("data-AutoPostBack") = "true"
                Else
                    _owner.Attributes.Remove("data-AutoPostBack")
                End If
            End Set
        End Property

        ''' <summary>
        ''' コンテキストメニューを開いた状態で、画面を初期表示するどうかを取得または設定します。
        ''' </summary>
        Public Property UseAutoOpening As Boolean
            Get
                Return (_owner.Attributes("data-UseAutoOpening") IsNot Nothing)
            End Get
            Set(value As Boolean)
                If (value) Then
                    _owner.Attributes("data-UseAutoOpening") = "true"
                Else
                    _owner.Attributes.Remove("data-UseAutoOpening")
                End If
            End Set
        End Property

        ''' <summary>
        ''' コンテキストメニューに保持されているコンテキストメニュー項目を取得します。
        ''' </summary>
        ''' <param name="id">コンテキストメニュー項目ID</param>
        ''' <returns>Idで指定されたコンテキストメニュー項目　（存在しない場合はNothing）</returns>
        Public Function GetMenuItem(ByVal id As Integer) As CommonMasterContextMenuItem
            If (_contextMenuItems.ContainsKey(id)) Then
                Return _contextMenuItems(id)
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' コンテキストメニューを開いた時に発生します。
        ''' </summary>
        ''' <remarks>AutoPostBackプロパティがTrueかつUseAutoOpeningプロパティがFalseになっている必要があります</remarks>
        Public Event Open As EventHandler

        ''' <summary>
        ''' コンテキストメニューを閉じた時に発生します。
        ''' </summary>
        ''' <remarks>AutoPostBackプロパティがTrueかつUseAutoOpeningプロパティがTrueになっている必要があります</remarks>
        Public Event Close As EventHandler



        Public Sub New(ByVal owner As PopOver, ByVal trigger As ImageButton)
            _trigger = trigger
            _owner = owner
        End Sub

        Public Sub AddMenuItem(ByVal item As CommonMasterContextMenuItem)
            _contextMenuItems.Add(item.ID, item)
            AddHandler item.Owner.Click, AddressOf menuItem_Click
        End Sub

        Public ReadOnly Property Owner As PopOver
            Get
                Return _owner
            End Get
        End Property

        Public ReadOnly Property ContextMenuItems As Dictionary(Of Integer, CommonMasterContextMenuItem)
            Get
                Return _contextMenuItems
            End Get
        End Property

        Public Sub OnOpen()
            RaiseEvent Open(Me, New EventArgs())
        End Sub

        Public Sub OnClose()
            RaiseEvent Close(Me, New EventArgs())
        End Sub

        Private Sub menuItem_Click(sender As Object, e As EventArgs)
            ''押されたボタンのイベントを発生させます。
            Dim id As String = CType(sender, WebControl).ID
            id = id.Substring(id.LastIndexOf("_", StringComparison.OrdinalIgnoreCase) + 1)

            Dim target As CommonMasterContextMenuItem = _contextMenuItems(CInt(id))

            ''在席状態を更新
            Try
                Dim staff As StaffContext = StaffContext.Current()
                staff.UpdatePresence(target.PresenceCategory, target.PresenceDetail)
            Catch ex As Exception
                Logger.Error("Login user presence couldn't be updated.", ex)
            End Try

            target.OnClick()
        End Sub

        Private _trigger As ImageButton
        Private _owner As PopOver
        Private _contextMenuItems As New Dictionary(Of Integer, CommonMasterContextMenuItem)    ''ヘッダー管理配列

    End Class
End Namespace
