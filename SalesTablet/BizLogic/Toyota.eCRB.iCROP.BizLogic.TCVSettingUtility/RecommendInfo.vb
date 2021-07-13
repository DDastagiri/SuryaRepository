
''' <summary>
''' リコメンド情報格納クラス（メーカーオプション、ディーラーオプション共通）
''' </summary>
''' <remarks></remarks>
Public Class RecommendInfo

    Dim _RecommendId As String
    Dim _RecommendName As String
    Dim _RecommendCheck As Boolean

    ''' <summary>
    ''' 属性の設定と取得を行う
    ''' </summary>
    ''' <value>属性</value>
    ''' <returns>属性</returns>
    ''' <remarks></remarks>
    Public Property RecommendId As String
        Get
            Return _RecommendId
        End Get
        Set(value As String)
            _RecommendId = value
        End Set
    End Property

    ''' <summary>
    ''' 属性名の設定と取得を行う
    ''' </summary>
    ''' <value>属性名</value>
    ''' <returns>属性名</returns>
    ''' <remarks></remarks>
    Public Property RecommendName As String
        Get
            Return _RecommendName
        End Get
        Set(value As String)
            _RecommendName = value
        End Set
    End Property

    ''' <summary>
    ''' 属性チェックの設定と取得を行う
    ''' </summary>
    ''' <value>属性チェック</value>
    ''' <returns>属性チェック</returns>
    ''' <remarks></remarks>
    Public Property RecommendCheck As Boolean
        Get
            Return _RecommendCheck
        End Get
        Set(value As Boolean)
            _RecommendCheck = value
        End Set
    End Property

End Class
