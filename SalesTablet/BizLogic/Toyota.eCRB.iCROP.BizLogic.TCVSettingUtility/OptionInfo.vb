
''' <summary>
''' オプション情報格納クラス（メーカーオプション、ディーラーオプション共通）
''' </summary>
''' <remarks></remarks>
Public Class OptionInfo

    Dim _OptionId As String
    Dim _OptionName As String
    Dim _Price As String
    Dim _ImageFilePath As String
    Dim _ImageFileName As String
    Dim _GradeConformity As List(Of String)

    ''' <summary>
    ''' オプションIDの設定と取得を行う
    ''' </summary>
    ''' <value>オプションID</value>
    ''' <returns>オプションID</returns>
    ''' <remarks></remarks>
    Public Property OptionId As String
        Get
            Return _OptionId
        End Get
        Set(value As String)
            _OptionId = value
        End Set
    End Property

    ''' <summary>
    ''' オプション名の設定と取得を行う
    ''' </summary>
    ''' <value>オプション名</value>
    ''' <returns>オプション名</returns>
    ''' <remarks></remarks>
    Public Property OptionName As String
        Get
            Return _OptionName
        End Get
        Set(value As String)
            _OptionName = value
        End Set
    End Property

    ''' <summary>
    ''' 価格の設定と取得を行う
    ''' </summary>
    ''' <value>価格</value>
    ''' <returns>価格</returns>
    ''' <remarks></remarks>
    Public Property Price As String
        Get
            Return _Price
        End Get
        Set(value As String)
            _Price = value
        End Set
    End Property

    ''' <summary>
    ''' 画像ファイルパスの設定と取得を行う
    ''' </summary>
    ''' <value>画像ファイルパス</value>
    ''' <returns>画像ファイルパス</returns>
    ''' <remarks></remarks>
    Public Property ImageFilePath As String
        Get
            Return _ImageFilePath
        End Get
        Set(value As String)
            _ImageFilePath = value
        End Set
    End Property

    ''' <summary>
    ''' 画像ファイル名の設定と取得を行う
    ''' </summary>
    ''' <value>画像ファイル名</value>
    ''' <returns>画像ファイル名</returns>
    ''' <remarks></remarks>
    Public Property ImageFileName As String
        Get
            Return _ImageFileName
        End Get
        Set(value As String)
            _ImageFileName = value
        End Set
    End Property

    ''' <summary>
    ''' グレード適合の取得を行う
    ''' </summary>
    ''' <returns>グレード適合</returns>
    ''' <remarks></remarks>
    Public ReadOnly Property GradeConformity As List(Of String)
        Get
            Return _GradeConformity
        End Get
    End Property

    ''' <summary>
    ''' グレード適合の設定を行う
    ''' </summary>
    ''' <param name="gradeConformity">グレード適合</param>
    ''' <remarks></remarks>
    Public Sub SetGradeConformity(gradeConformity As List(Of String))
        _GradeConformity = gradeConformity
    End Sub

End Class
