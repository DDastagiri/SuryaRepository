Public Class XmlAccount
    Implements IDisposable

    ''' <summary>
    ''' 受信先のアカウント
    ''' </summary>
    ''' <remarks></remarks>
    Private _ToAccount As String

    ''' <summary>
    ''' 受信先の端末ID
    ''' </summary>
    ''' <remarks></remarks>
    Private _ToClientId As String

    ''' <summary>
    ''' 受信者名
    ''' </summary>
    ''' <remarks></remarks>
    Private _ToAccountName As String

    ''' <summary>
    ''' 受信者名
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ToAccountName() As String
        Get
            Return _ToAccountName
        End Get
        Set(ByVal value As String)
            _ToAccountName = value
        End Set
    End Property

    ''' <summary>
    ''' 受信先の端末ID
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ToClientId() As String
        Get
            Return _ToClientId
        End Get
        Set(ByVal value As String)
            _ToClientId = value
        End Set
    End Property

    ''' <summary>
    ''' 受信先のアカウント
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ToAccount() As String
        Get
            Return _ToAccount
        End Get
        Set(ByVal value As String)
            _ToAccount = value
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
