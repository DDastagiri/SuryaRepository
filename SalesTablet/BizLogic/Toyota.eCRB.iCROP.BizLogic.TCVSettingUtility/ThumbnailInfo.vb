
''' <summary>
''' サムネイル情報 格納クラス
''' </summary>
''' <remarks></remarks>
Public Class ThumbnailInfo

    Private _id As String
    Private _thumbnailPath As String
    Private _gridPath As String

    ''' <summary>
    ''' IDの設定と取得を行う
    ''' </summary>
    ''' <value>ID</value>
    ''' <returns>ID</returns>
    ''' <remarks></remarks>
    Public Property Id As String
        Get
            Return _id
        End Get
        Set(value As String)
            _id = value
        End Set
    End Property

    ''' <summary>
    ''' サムネイル画像パスの設定と取得を行う
    ''' </summary>
    ''' <value>サムネイル画像パス</value>
    ''' <returns>サムネイル画像パス</returns>
    ''' <remarks></remarks>
    Public Property ThumbnailPath As String
        Get
            Return _thumbnailPath
        End Get
        Set(value As String)
            _thumbnailPath = value
        End Set
    End Property

    ''' <summary>
    ''' グリッド画像パスの設定と取得を行う
    ''' </summary>
    ''' <value>グリッド画像パス</value>
    ''' <returns>グリッド画像パス</returns>
    ''' <remarks></remarks>
    Public Property GridPath As String
        Get
            Return _gridPath
        End Get
        Set(value As String)
            _gridPath = value
        End Set
    End Property

End Class
