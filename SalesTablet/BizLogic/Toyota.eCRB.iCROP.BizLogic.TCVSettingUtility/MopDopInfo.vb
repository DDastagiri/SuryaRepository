''' <summary>
''' MOP/DOP情報データ格納クラス
''' </summary>
''' <remarks></remarks>
Public Class MopDopInfo

    Private _optionId As String
    Private _optionKind As String
    Private _optionKindName As String
    Private _optionName As String
    Private _attribute As String
    Private _attributeName As String
    Private _order As Integer
    Private _maxOrderInAttr As Integer
    Private _countInAttr As Integer

    ''' <summary>
    ''' オプションIDの取得と設定を行います。
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property OptionId As String
        Get
            Return Me._optionId
        End Get
        Set(value As String)
            Me._optionId = value
        End Set
    End Property

    ''' <summary>
    ''' オプション種別の取得と設定を行います。
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property OptionKind As String
        Get
            Return Me._optionKind
        End Get
        Set(value As String)
            Me._optionKind = value
        End Set
    End Property

    ''' <summary>
    ''' オプション種別名の取得と設定を行います。
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property OptionKindName As String
        Get
            Return Me._optionKindName
        End Get
        Set(value As String)
            Me._optionKindName = value
        End Set
    End Property

    ''' <summary>
    ''' オプション名の取得と設定を行います。
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property OptionName As String
        Get
            Return Me._optionName
        End Get
        Set(value As String)
            Me._optionName = value
        End Set
    End Property

    ''' <summary>
    ''' 属性の取得と設定を行います。
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Attribute As String
        Get
            Return Me._attribute
        End Get
        Set(value As String)
            Me._attribute = value
        End Set
    End Property

    ''' <summary>
    ''' 属性名の取得と設定を行います。
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property AttributeName As String
        Get
            Return Me._attributeName
        End Get
        Set(value As String)
            Me._attributeName = value
        End Set
    End Property

    ''' <summary>
    ''' 表示順の取得と設定を行います。
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Order As Integer
        Get
            Return Me._order
        End Get
        Set(value As Integer)
            Me._order = value
        End Set
    End Property

    ''' <summary>
    ''' 属性毎の表示順の最大値の取得と設定を行います。
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property MaxOrderInAttr As Integer
        Get
            Return Me._maxOrderInAttr
        End Get
        Set(value As Integer)
            Me._maxOrderInAttr = value
        End Set
    End Property

    ''' <summary>
    ''' 属性毎の件数の取得と設定を行います。
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CountInAttr As Integer
        Get
            Return Me._countInAttr
        End Get
        Set(value As Integer)
            Me._countInAttr = value
        End Set
    End Property

End Class
