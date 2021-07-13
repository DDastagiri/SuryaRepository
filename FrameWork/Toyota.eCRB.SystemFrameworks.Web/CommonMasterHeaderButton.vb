Imports System.Web.UI.WebControls
Imports Toyota.eCRB.SystemFrameworks.Web.Controls

Namespace Toyota.eCRB.SystemFrameworks.Web
    ''' <summary>
    ''' ログアウトボタンクラスです。
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CommonMasterHeaderButton

        Private _owner As WebControl

        ''' <summary>
        ''' ログアウトボタンのハイパーリンクインスタンスを指定して、<br/>
        ''' 当クラスのインスタンスを生成します。
        ''' </summary>
        ''' <param name="owner"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal owner As WebControl)
            _owner = owner
        End Sub

        ''' <summary>
        ''' ハイパーリンクがクリックされた再に実行されるクライアント側スクリプトを取得または設定します。
        ''' </summary>
        ''' <remarks></remarks>
        Public Property OnClientClick As String
            Get
                'If TypeOf _owner Is CustomHyperLink Then
                '    '拡張コントロール
                '    Return CType(_owner, CustomHyperLink).OnClientClick
                'ElseIf TypeOf _owner Is LinkButton Then
                '    '基本コントロール
                '    Return CType(_owner, LinkButton).OnClientClick
                'Else
                '    '不明
                '    Throw New ArgumentException()
                'End If
                If TypeOf _owner Is CustomHyperLink Then
                    '拡張コントロール
                    Return CType(_owner, CustomHyperLink).OnClientClick
                Else
                    '基本コントロール
                    Return CType(_owner, LinkButton).OnClientClick
                End If
            End Get
            Set(value As String)
                '_owner.OnClientClick = value
                If TypeOf _owner Is CustomHyperLink Then
                    '拡張コントロール
                    CType(_owner, CustomHyperLink).OnClientClick = value
                ElseIf TypeOf _owner Is LinkButton Then
                    '基本コントロール
                    CType(_owner, LinkButton).OnClientClick = value
                Else
                    '不明
                    Throw New ArgumentException()
                End If
            End Set
        End Property

    End Class
End Namespace