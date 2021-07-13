Imports System.Web.UI.WebControls
Imports Toyota.eCRB.SystemFrameworks.Web.Controls

Namespace Toyota.eCRB.SystemFrameworks.Web

    ''' <summary>
    ''' ページ用マスターページの検索バーを表すクラスです。ヘッダ制御APIを提供します。
    ''' </summary>
    Public Class CommonMasterSearchBox
        ''' <summary>
        ''' 検索バーの状態（有効／無効）を取得または設定します。
        ''' </summary>
        Public Property Enabled As Boolean
            Get
                Return _textBox.Enabled
            End Get
            Set(value As Boolean)
                _textBox.Enabled = value
                _icon.Enabled = value
                If (value) Then
                    _frame.CssClass = ""
                Else
                    _frame.CssClass = "disabled"
                End If
            End Set
        End Property

        ''' <summary>
        ''' 検索バーの状態（有効／無効）を取得または設定します。
        ''' </summary>
        Public Property Visible As Boolean
            Get
                If (_frame.Style("display") IsNot Nothing) Then
                    Return Not _frame.Style("display").Equals("none")
                Else
                    Return True
                End If
            End Get
            Set(value As Boolean)
                If (value) Then
                    _frame.Style.Remove("display")
                Else
                    _frame.Style("display") = "none"
                End If
            End Set
        End Property

        ''' <summary>
        ''' 検索バーの中の検索文字列を取得または設定します。
        ''' </summary>
        Public Property SearchText As String
            Get
                Return _textBox.Text
            End Get
            Set(value As String)
                _textBox.Text = value
            End Set
        End Property


        Public Sub New(ByVal frame As Panel, ByVal textBox As CustomTextBox, ByVal icon As ImageButton)
            _frame = frame
            _textBox = textBox
            _icon = icon
        End Sub

        Private _frame As Panel
        Private _textBox As CustomTextBox
        Private _icon As ImageButton
    End Class
End Namespace

