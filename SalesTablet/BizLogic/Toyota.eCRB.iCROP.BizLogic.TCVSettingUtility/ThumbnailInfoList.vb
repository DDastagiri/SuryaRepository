
''' <summary>
''' サムネイル情報 全データ格納クラス
''' </summary>
''' <remarks></remarks>
Public Class ThumbnailInfoList

    Private _thumbnailInfo As List(Of ThumbnailInfo)

    ''' <summary>
    ''' コンストラクタ
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        'リストの初期化
        _thumbnailInfo = New List(Of ThumbnailInfo)
    End Sub

    ''' <summary>
    ''' サムネイル情報の取得を行う
    ''' </summary>
    ''' <returns>サムネイル情報</returns>
    ''' <remarks></remarks>
    Public ReadOnly Property ThumbnailInfo As List(Of ThumbnailInfo)
        Get
            Return _thumbnailInfo
        End Get
    End Property

End Class
