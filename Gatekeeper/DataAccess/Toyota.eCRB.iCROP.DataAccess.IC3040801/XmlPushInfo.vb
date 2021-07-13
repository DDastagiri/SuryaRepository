Public Class XmlPushInfo
    Implements IDisposable

    ''' <summary>
    ''' カテゴリータイプ
    ''' </summary>
    ''' <remarks></remarks>
    Private _PushCategory As String

    ''' <summary>
    ''' 表示位置
    ''' </summary>
    ''' <remarks></remarks>
    Private _PositionType As String

    ''' <summary>
    ''' 表示時間
    ''' </summary>
    ''' <remarks></remarks>
    Private _Time As Long

    ''' <summary>
    ''' 表示タイプ
    ''' </summary>
    ''' <remarks></remarks>
    Private _DisplayType As String

    ''' <summary>
    ''' 表示内容
    ''' </summary>
    ''' <remarks></remarks>
    Private _DisplayContents As String

    ''' <summary>
    ''' 色
    ''' </summary>
    ''' <remarks></remarks>
    Private _Color As String

    ''' <summary>
    ''' 幅
    ''' </summary>
    ''' <remarks></remarks>
    Private _PopWidth As Long

    ''' <summary>
    ''' 高さ
    ''' </summary>
    ''' <remarks></remarks>
    Private _PopHeight As Long

    ''' <summary>
    ''' X座標
    ''' </summary>
    ''' <remarks></remarks>
    Private _PopX As Long

    ''' <summary>
    ''' Y座標
    ''' </summary>
    ''' <remarks></remarks>
    Private _PopY As Long

    ''' <summary>
    ''' 表示時関数
    ''' </summary>
    ''' <remarks></remarks>
    Private _DisplayFunction As String

    ''' <summary>
    ''' アクンション時関数
    ''' </summary>
    ''' <remarks></remarks>
    Private _ActionFunction As String

    ''' <summary>
    ''' アクンション時関数
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ActionFunction() As String
        Get
            Return _ActionFunction
        End Get
        Set(ByVal value As String)
            _ActionFunction = value
        End Set
    End Property

    ''' <summary>
    ''' 表示時関数
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property DisplayFunction() As String
        Get
            Return _DisplayFunction
        End Get
        Set(ByVal value As String)
            _DisplayFunction = value
        End Set
    End Property

    ''' <summary>
    ''' Y座標
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property PopY() As Long
        Get
            Return _PopY
        End Get
        Set(ByVal value As Long)
            _PopY = value
        End Set
    End Property

    ''' <summary>
    ''' X座標
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property PopX() As Long
        Get
            Return _PopX
        End Get
        Set(ByVal value As Long)
            _PopX = value
        End Set
    End Property

    ''' <summary>
    ''' 高さ
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property PopHeight() As Long
        Get
            Return _PopHeight
        End Get
        Set(ByVal value As Long)
            _PopHeight = value
        End Set
    End Property

    ''' <summary>
    ''' 幅
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property PopWidth() As Long
        Get
            Return _PopWidth
        End Get
        Set(ByVal value As Long)
            _PopWidth = value
        End Set
    End Property

    ''' <summary>
    ''' 色
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Color() As String
        Get
            Return _Color
        End Get
        Set(ByVal value As String)
            _Color = value
        End Set
    End Property

    ''' <summary>
    ''' 表示内容
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property DisplayContents() As String
        Get
            Return _DisplayContents
        End Get
        Set(ByVal value As String)
            _DisplayContents = value
        End Set
    End Property

    ''' <summary>
    ''' 表示タイプ
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property DisplayType() As String
        Get
            Return _DisplayType
        End Get
        Set(ByVal value As String)
            _DisplayType = value
        End Set
    End Property

    ''' <summary>
    ''' 表示時間
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Time() As Long
        Get
            Return _Time
        End Get
        Set(ByVal value As Long)
            _Time = value
        End Set
    End Property

    ''' <summary>
    ''' 表示位置
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property PositionType() As String
        Get
            Return _PositionType
        End Get
        Set(ByVal value As String)
            _PositionType = value
        End Set
    End Property

    ''' <summary>
    ''' カテゴリータイプ
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property PushCategory() As String
        Get
            Return _PushCategory
        End Get
        Set(ByVal value As String)
            _PushCategory = value
        End Set
    End Property

    ''' <summary>
    ''' Disposeメソッド
    ''' </summary>
    ''' <remarks></remarks>
    Public Overloads Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
    Protected Overridable Overloads Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
        End If
    End Sub
End Class
