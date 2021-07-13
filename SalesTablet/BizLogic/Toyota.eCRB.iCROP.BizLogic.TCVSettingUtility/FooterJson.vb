Imports System.Runtime.Serialization
Imports System.Web

''' <summary>
''' footer JSONファイル フッターマップ情報格納クラス
''' </summary>
''' <remarks></remarks>
<DataContract()>
Public Class FooterJson
    Inherits AbstractJson

    Private _id As String                   'json
    Private _url_name As String             'json
    Private _url As String                  'json
    Private _name As String                 'json
    Private _exists As Boolean              'json
    Private _imageFile As FooterImageJson   'json
    Private _iconPath As String             'original
    Private _iconNameNew As String          'original
    Private _iconNameOld As String          'original
    Private _order As Integer               'original
    Private _postedFile As HttpPostedFile   'original

    ''' <summary>
    ''' IDの取得と設定を行います。
    ''' </summary>
    ''' <value>ID</value>
    ''' <returns>ID</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property id As String
        Get
            Return _id
        End Get
        Set(value As String)
            _id = value
        End Set
    End Property

    ''' <summary>
    ''' 名称の取得と設定を行います。
    ''' </summary>
    ''' <value>名称</value>
    ''' <returns>名称</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property url_name As String
        Get
            Return _url_name
        End Get
        Set(value As String)
            _url_name = value
        End Set
    End Property

    ''' <summary>
    ''' リンク先URLの取得と設定を行います。
    ''' </summary>
    ''' <value>リンク先URL</value>
    ''' <returns>リンク先URL</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property url As String
        Get
            Return _url
        End Get
        Set(value As String)
            _url = value
        End Set
    End Property

    ''' <summary>
    ''' 表示用文言の取得と設定を行います。
    ''' </summary>
    ''' <value>表示用文言</value>
    ''' <returns>表示用文言</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property name As String
        Get
            Return _name
        End Get
        Set(value As String)
            _name = value
        End Set
    End Property

    ''' <summary>
    ''' 有無フラグの取得と設定を行います。
    ''' </summary>
    ''' <value>有無フラグ</value>
    ''' <returns>有無フラグ</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property exists As Boolean
        Get
            Return _exists
        End Get
        Set(value As Boolean)
            _exists = value
        End Set
    End Property

    ''' <summary>
    ''' 画像ファイル情報の取得と設定を行います。
    ''' </summary>
    ''' <value>画像ファイル情報</value>
    ''' <returns>画像ファイル情報</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property imageFile As FooterImageJson
        Get
            If IsNothing(_imageFile) Then
                _imageFile = New FooterImageJson
            End If
            Return _imageFile
        End Get
        Set(value As FooterImageJson)
            _imageFile = value
        End Set
    End Property

    ''' <summary>
    ''' アイコン画像パスの取得と設定を行います。
    ''' </summary>
    ''' <value>アイコン画像パス</value>
    ''' <returns>アイコン画像パス</returns>
    ''' <remarks></remarks>
    Public Property IconPath As String
        Get
            Return _iconPath
        End Get
        Set(value As String)
            _iconPath = value
        End Set
    End Property

    ''' <summary>
    ''' 新アイコン画像名の取得と設定を行います。
    ''' </summary>
    ''' <value>新アイコン画像名</value>
    ''' <returns>新アイコン画像名</returns>
    ''' <remarks></remarks>
    Public Property IconNameNew As String
        Get
            Return _iconNameNew
        End Get
        Set(value As String)
            _iconNameNew = value
        End Set
    End Property

    ''' <summary>
    ''' 旧アイコン画像名の取得と設定を行います。
    ''' </summary>
    ''' <value>旧アイコン画像名</value>
    ''' <returns>旧アイコン画像名</returns>
    ''' <remarks></remarks>
    Public Property IconNameOld As String
        Get
            Return _iconNameOld
        End Get
        Set(value As String)
            _iconNameOld = value
        End Set
    End Property

    ''' <summary>
    ''' 表示順の取得と設定を行います。
    ''' </summary>
    ''' <value>表示順</value>
    ''' <returns>表示順</returns>
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
    ''' アップロードファイルの取得と設定を行います。
    ''' </summary>
    ''' <value>アップロードファイル</value>
    ''' <returns>アップロードファイル</returns>
    ''' <remarks></remarks>
    Public Property PostedFile As HttpPostedFile
        Get
            Return Me._postedFile
        End Get
        Set(value As HttpPostedFile)
            Me._postedFile = value
        End Set
    End Property

End Class
