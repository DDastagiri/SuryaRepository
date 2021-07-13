Public Class XmlNoticeData
    Implements IDisposable

    ''' <summary>
    ''' 送信日付
    ''' </summary>
    ''' <remarks></remarks>
    Private _TransmissionDate As Date

    ''' <summary>
    ''' 受信者
    ''' </summary>
    ''' <remarks></remarks>
    Private ReadOnly _AccountList As New List(Of XmlAccount)

    ''' <summary>
    ''' 通知情報
    ''' </summary>
    ''' <remarks></remarks>
    Private _RequestNotice As New XmlRequestNotice

    ''' <summary>
    ''' PushServer情報
    ''' </summary>
    ''' <remarks></remarks>
    Private _PushInfo As New XmlPushInfo

    ''' <summary>
    ''' PushServer情報
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property PushInfo() As XmlPushInfo
        Get
            Return _PushInfo
        End Get
        Set(ByVal value As XmlPushInfo)
            _PushInfo = value
        End Set
    End Property

    ''' <summary>
    ''' 通知情報
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property RequestNotice() As XmlRequestNotice
        Get
            Return _RequestNotice
        End Get
        Set(ByVal value As XmlRequestNotice)
            _RequestNotice = value
        End Set
    End Property

    ''' <summary>
    ''' 送信日付
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property TransmissionDate() As Date
        Get
            Return _TransmissionDate
        End Get
        Set(ByVal value As Date)
            _TransmissionDate = value
        End Set
    End Property

    ''' <summary>
    ''' 受信者
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property AccountList() As List(Of XmlAccount)
        Get
            Return _AccountList
        End Get
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
            _RequestNotice.Dispose()
            If Not IsNothing(_PushInfo) Then
                _PushInfo.Dispose()
            End If

            _RequestNotice = Nothing
            _PushInfo = Nothing
        End If
    End Sub
End Class
