Public Class XmlCommon

    ''' <summary>
    ''' 通知依頼ID
    ''' </summary>
    ''' <remarks></remarks>
    Private _NoticeRequestId As Long

    ''' <summary>
    ''' 応答結果
    ''' </summary>
    ''' <remarks></remarks>
    Private _ResultId As String

    ''' <summary>
    ''' メッセージ
    ''' </summary>
    ''' <remarks></remarks>
    Private _Message As String

    ''' <summary>
    ''' メッセージ
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Message() As String
        Get
            Return _Message
        End Get
        Set(ByVal value As String)
            _Message = value
        End Set
    End Property

    ''' <summary>
    ''' 通知依頼ID
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property NoticeRequestId() As Long
        Get
            Return _NoticeRequestId
        End Get
        Set(ByVal value As Long)
            _NoticeRequestId = value
        End Set
    End Property

    ''' <summary>
    ''' 応答結果
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ResultId() As String
        Get
            Return _ResultId
        End Get
        Set(ByVal value As String)
            _ResultId = value
        End Set
    End Property

End Class
