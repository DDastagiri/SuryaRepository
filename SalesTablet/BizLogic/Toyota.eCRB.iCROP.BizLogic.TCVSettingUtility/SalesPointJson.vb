Imports System.Runtime.Serialization

''' <summary>
''' sales_point JSONファイル セールスポイント情報データ格納クラス
''' </summary>
''' <remarks></remarks>
<DataContract()>
Public Class SalesPointJson
    Inherits AbstractJson

    Private _sortNo As Integer
    Private _no As String
    Private _id As String
    Private _type As String
    Private _viewtype As String
    Private _angle As List(Of String)
    Private _interiorid As List(Of String)
    Private _grd As List(Of String)
    Private _title As String
    Private _contents As String
    Private _top As List(Of String)
    Private _left As List(Of String)
    Private _overviewtitle As String
    Private _overviewcontents As String
    Private _overviewtop As List(Of String)
    Private _overviewleft As List(Of String)
    Private _overviewimg As String
    Private _popuptype As String
    Private _popuptitle As String
    Private _popupcontents As String
    Private _popupsrc As String
    Private _fullscreenpopupsrc As String
    Private _introductionVisible As Boolean
    Private _overviewFile As String
    Private _popupFile As String
    Private _fullscreenPopupFile As String

    ''' <summary>
    ''' ソートNoの設定と取得を行う
    ''' </summary>
    ''' <value>ソートNo</value>
    ''' <returns>ソートNo</returns>
    ''' <remarks></remarks>
    Public Property SortNo() As Integer
        Get
            Return _sortNo
        End Get
        Set(value As Integer)
            _sortNo = value
        End Set
    End Property

    ''' <summary>
    ''' Noの設定と取得を行う
    ''' </summary>
    ''' <value>No</value>
    ''' <returns>No</returns>
    ''' <remarks></remarks>
    Public Property No() As String
        Get
            Return _no
        End Get
        Set(value As String)
            _no = value
        End Set
    End Property

    ''' <summary>
    ''' IDの設定と取得を行う
    ''' </summary>
    ''' <value>ID</value>
    ''' <returns>ID</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property id() As String
        Get
            Return _id
        End Get
        Set(value As String)
            _id = value
        End Set
    End Property

    ''' <summary>
    ''' 外装/内装の設定と取得を行う
    ''' </summary>
    ''' <value>外装/内装</value>
    ''' <returns>外装/内装</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property type() As String
        Get
            Return _type
        End Get
        Set(value As String)
            _type = value
        End Set
    End Property

    ''' <summary>
    ''' タイプの設定と取得を行う
    ''' </summary>
    ''' <value>タイプ</value>
    ''' <returns>タイプ</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property viewtype() As String
        Get
            Return _viewtype
        End Get
        Set(value As String)
            _viewtype = value
        End Set
    End Property

    ''' <summary>
    ''' 外装アングルの設定と取得を行う
    ''' </summary>
    ''' <value>外装アングル</value>
    ''' <returns>外装アングル</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property angle() As List(Of String)
        Get
            Return _angle
        End Get
        Set(value As List(Of String))
            _angle = value
        End Set
    End Property

    ''' <summary>
    ''' 内装画面IDの設定と取得を行う
    ''' </summary>
    ''' <value>内装画面ID</value>
    ''' <returns>内装画面ID</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property interiorid() As List(Of String)
        Get
            Return _interiorid
        End Get
        Set(value As List(Of String))
            _interiorid = value
        End Set
    End Property

    ''' <summary>
    ''' グレード適合の設定と取得を行う
    ''' </summary>
    ''' <value>グレード適合</value>
    ''' <returns>グレード適合</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property grd() As List(Of String)
        Get
            Return _grd
        End Get
        Set(value As List(Of String))
            _grd = value
        End Set
    End Property

    ''' <summary>
    ''' タイトルの設定と取得を行う
    ''' </summary>
    ''' <value>タイトル</value>
    ''' <returns>タイトル</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property title() As String
        Get
            Return _title
        End Get
        Set(value As String)
            _title = value
        End Set
    End Property

    ''' <summary>
    ''' 説明文の設定と取得を行う
    ''' </summary>
    ''' <value>説明文</value>
    ''' <returns>説明文</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property contents() As String
        Get
            Return _contents
        End Get
        Set(value As String)
            _contents = value
        End Set
    End Property

    ''' <summary>
    ''' 指示ポイント（トップ）の設定と取得を行う
    ''' </summary>
    ''' <value>指示ポイント（トップ）</value>
    ''' <returns>指示ポイント（トップ）</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property top() As List(Of String)
        Get
            Return _top
        End Get
        Set(value As List(Of String))
            _top = value
        End Set
    End Property

    ''' <summary>
    ''' 指示ポイント（レフト）の設定と取得を行う
    ''' </summary>
    ''' <value>指示ポイント（レフト）</value>
    ''' <returns>指示ポイント（レフト）</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property left() As List(Of String)
        Get
            Return _left
        End Get
        Set(value As List(Of String))
            _left = value
        End Set
    End Property

    ''' <summary>
    ''' オーバーレイ（タイトル）の設定と取得を行う
    ''' </summary>
    ''' <value>オーバーレイ（タイトル）</value>
    ''' <returns>オーバーレイ（タイトル）</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property overviewtitle() As String
        Get
            Return _overviewtitle
        End Get
        Set(value As String)
            _overviewtitle = value
        End Set
    End Property

    ''' <summary>
    ''' オーバーレイ（説明文）の設定と取得を行う
    ''' </summary>
    ''' <value>オーバーレイ（説明文）</value>
    ''' <returns>オーバーレイ（説明文）</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property overviewcontents() As String
        Get
            Return _overviewcontents
        End Get
        Set(value As String)
            _overviewcontents = value
        End Set
    End Property

    ''' <summary>
    ''' オーバーレイ（トップ）の設定と取得を行う
    ''' </summary>
    ''' <value>オーバーレイ（トップ）</value>
    ''' <returns>オーバーレイ（トップ）</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property overviewtop() As List(Of String)
        Get
            Return _overviewtop
        End Get
        Set(value As List(Of String))
            _overviewtop = value
        End Set
    End Property

    ''' <summary>
    ''' オーバーレイ（レフト）の設定と取得を行う
    ''' </summary>
    ''' <value>オーバーレイ（レフト）</value>
    ''' <returns>オーバーレイ（レフト）</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property overviewleft() As List(Of String)
        Get
            Return _overviewleft
        End Get
        Set(value As List(Of String))
            _overviewleft = value
        End Set
    End Property

    ''' <summary>
    ''' オーバーレイ（画像）の設定と取得を行う
    ''' </summary>
    ''' <value>オーバーレイ（画像）</value>
    ''' <returns>オーバーレイ（画像）</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property overviewimg() As String
        Get
            Return _overviewimg
        End Get
        Set(value As String)
            _overviewimg = value
        End Set
    End Property

    ''' <summary>
    ''' ポップアップ（タイプ）の設定と取得を行う
    ''' </summary>
    ''' <value>ポップアップ（タイプ）</value>
    ''' <returns>ポップアップ（タイプ）</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property popuptype() As String
        Get
            Return _popuptype
        End Get
        Set(value As String)
            _popuptype = value
        End Set
    End Property

    ''' <summary>
    ''' ポップアップ（タイトル）の設定と取得を行う
    ''' </summary>
    ''' <value>ポップアップ（タイトル）</value>
    ''' <returns>ポップアップ（タイトル）</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property popuptitle() As String
        Get
            Return _popuptitle
        End Get
        Set(value As String)
            _popuptitle = value
        End Set
    End Property

    ''' <summary>
    ''' ポップアップ（コンテンツ）の設定と取得を行う
    ''' </summary>
    ''' <value>ポップアップ（コンテンツ）</value>
    ''' <returns>ポップアップ（コンテンツ）</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property popupcontents() As String
        Get
            Return _popupcontents
        End Get
        Set(value As String)
            _popupcontents = value
        End Set
    End Property

    ''' <summary>
    ''' ポップアップ（画像）の設定と取得を行う
    ''' </summary>
    ''' <value>ポップアップ（画像）</value>
    ''' <returns>ポップアップ（画像）</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property popupsrc() As String
        Get
            Return _popupsrc
        End Get
        Set(value As String)
            _popupsrc = value
        End Set
    End Property

    ''' <summary>
    ''' フルスクリーンポップアップ（画像）の設定と取得を行う
    ''' </summary>
    ''' <value>フルスクリーンポップアップ（画像）</value>
    ''' <returns>フルスクリーンポップアップ（画像）</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property fullscreenpopupsrc() As String
        Get
            Return _fullscreenpopupsrc
        End Get
        Set(value As String)
            _fullscreenpopupsrc = value
        End Set
    End Property

    ''' <summary>
    ''' セールスポイント有効フラグの設定と取得を行う
    ''' </summary>
    ''' <value>セールスポイント有効フラグ</value>
    ''' <returns>セールスポイント有効フラグ</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property introductionVisible() As Boolean
        Get
            Return _introductionVisible
        End Get
        Set(value As Boolean)
            _introductionVisible = value
        End Set
    End Property

    ''' <summary>
    ''' オーバーレイファイル名の設定と取得を行う
    ''' </summary>
    ''' <value>オーバーレイファイル名</value>
    ''' <returns>オーバーレイファイル名</returns>
    ''' <remarks></remarks>
    Public Property OverviewFile() As String
        Get
            Return _overviewFile
        End Get
        Set(value As String)
            _overviewFile = value
        End Set
    End Property

    ''' <summary>
    ''' ポップアップファイル名の設定と取得を行う
    ''' </summary>
    ''' <value>ポップアップファイル名</value>
    ''' <returns>ポップアップファイル名</returns>
    ''' <remarks></remarks>
    Public Property PopupFile() As String
        Get
            Return _popupFile
        End Get
        Set(value As String)
            _popupFile = value
        End Set
    End Property

    ''' <summary>
    ''' フルスクリーンポップアップファイル名の設定と取得を行う
    ''' </summary>
    ''' <value>フルスクリーンポップアップファイル名</value>
    ''' <returns>フルスクリーンポップアップファイル名</returns>
    ''' <remarks></remarks>
    Public Property FullscreenPopupFile() As String
        Get
            Return _fullscreenPopupFile
        End Get
        Set(value As String)
            _fullscreenPopupFile = value
        End Set
    End Property

End Class
