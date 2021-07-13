Imports System.Runtime.Serialization

''' <summary>
''' car_lineup JSONファイル 車種リストデータ格納クラス
''' </summary>
''' <remarks></remarks>
<DataContract()>
Public Class CarLineupCarListJson
    Inherits AbstractJson

    Private _id As String
    Private _series As String
    Private _name As String
    Private _popupImage As String
    Private _priceMin As String
    Private _priceMax As String
    Private _imageurlA As String
    Private _imageurlB As String
    Private _imageurlMirrorA As String
    Private _imageurlMirrorB As String
    Private _logourl As String
    Private _carselectVisivle As Boolean
    Private _libraryVisible As Boolean
    Private _introductionExists As Boolean
    Private _specificationsExists As Boolean
    Private _comparisonExists As Boolean
    Private _libraryExists As Boolean
    Private _recommendCarRateVisible As Boolean

    ''' <summary>
    ''' IDの設定と取得を行う
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
    ''' シリーズの設定と取得を行う
    ''' </summary>
    ''' <value>シリーズ</value>
    ''' <returns>シリーズ</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property series As String
        Get
            Return _series
        End Get
        Set(value As String)
            _series = value
        End Set
    End Property

    ''' <summary>
    ''' 車種名称の設定と取得を行う
    ''' </summary>
    ''' <value>車種名称</value>
    ''' <returns>車種名称</returns>
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
    ''' ポップアップ画像の設定と取得を行う
    ''' </summary>
    ''' <value>ポップアップ画像</value>
    ''' <returns>ポップアップ画像</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property popupImage As String
        Get
            Return _popupImage
        End Get
        Set(value As String)
            _popupImage = value
        End Set
    End Property

    ''' <summary>
    ''' 最低価格の設定と取得を行う
    ''' </summary>
    ''' <value>最低価格</value>
    ''' <returns>最低価格</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property priceMin As String
        Get
            Return _priceMin
        End Get
        Set(value As String)
            _priceMin = value
        End Set
    End Property

    ''' <summary>
    ''' 最高価格の設定と取得を行う
    ''' </summary>
    ''' <value>最高価格</value>
    ''' <returns>最高価格</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property priceMax As String
        Get
            Return _priceMax
        End Get
        Set(value As String)
            _priceMax = value
        End Set
    End Property

    ''' <summary>
    ''' 車両紹介画面画像Aの設定と取得を行う
    ''' </summary>
    ''' <value>車両紹介画面画像A</value>
    ''' <returns>車両紹介画面画像A</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property imageurlA As String
        Get
            Return _imageurlA
        End Get
        Set(value As String)
            _imageurlA = value
        End Set
    End Property

    ''' <summary>
    ''' 車両紹介画面画像Bの設定と取得を行う
    ''' </summary>
    ''' <value>車両紹介画面画像B</value>
    ''' <returns>車両紹介画面画像B</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property imageurlB As String
        Get
            Return _imageurlB
        End Get
        Set(value As String)
            _imageurlB = value
        End Set
    End Property

    ''' <summary>
    ''' 鏡面画像画像Aの設定と取得を行う
    ''' </summary>
    ''' <value>鏡面画像画像A</value>
    ''' <returns>鏡面画像画像A</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property imageurlMirrorA As String
        Get
            Return _imageurlMirrorA
        End Get
        Set(value As String)
            _imageurlMirrorA = value
        End Set
    End Property

    ''' <summary>
    ''' 鏡面画像画像Bの設定と取得を行う
    ''' </summary>
    ''' <value>鏡面画像画像B</value>
    ''' <returns>鏡面画像画像B</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property imageurlMirrorB As String
        Get
            Return _imageurlMirrorB
        End Get
        Set(value As String)
            _imageurlMirrorB = value
        End Set
    End Property

    ''' <summary>
    ''' ロゴ画像の設定と取得を行う
    ''' </summary>
    ''' <value>ロゴ画像</value>
    ''' <returns>ロゴ画像</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property logourl As String
        Get
            Return _logourl
        End Get
        Set(value As String)
            _logourl = value
        End Set
    End Property

    ''' <summary>
    ''' 車種選択可視フラグの設定と取得を行う
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property carselectVisivle As Boolean
        Get
            Return _carselectVisivle
        End Get
        Set(value As Boolean)
            _carselectVisivle = value
        End Set
    End Property

    ''' <summary>
    ''' ライブラリ可視フラグの設定と取得を行う
    ''' </summary>
    ''' <value>ライブラリ可視フラグ</value>
    ''' <returns>ライブラリ可視フラグ</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property libraryVisible As Boolean
        Get
            Return _libraryVisible
        End Get
        Set(value As Boolean)
            _libraryVisible = value
        End Set
    End Property

    ''' <summary>
    ''' 車両紹介存在フラグの設定と取得を行う
    ''' </summary>
    ''' <value>車両紹介存在フラグ</value>
    ''' <returns>車両紹介存在フラグ</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property introductionExists As Boolean
        Get
            Return _introductionExists
        End Get
        Set(value As Boolean)
            _introductionExists = value
        End Set
    End Property

    ''' <summary>
    ''' 諸元表存在フラグの設定と取得を行う
    ''' </summary>
    ''' <value>諸元表存在フラグ</value>
    ''' <returns>諸元表存在フラグ</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property specificationsExists As Boolean
        Get
            Return _specificationsExists
        End Get
        Set(value As Boolean)
            _specificationsExists = value
        End Set
    End Property

    ''' <summary>
    ''' 競合車比較存在フラグの設定と取得を行う
    ''' </summary>
    ''' <value>競合車比較存在フラグ</value>
    ''' <returns>競合車比較存在フラグ</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property comparisonExists As Boolean
        Get
            Return _comparisonExists
        End Get
        Set(value As Boolean)
            _comparisonExists = value
        End Set
    End Property

    ''' <summary>
    ''' ライブラリ存在フラグの設定と取得を行う
    ''' </summary>
    ''' <value>ライブラリ存在フラグ</value>
    ''' <returns>ライブラリ存在フラグ</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property libraryExists As Boolean
        Get
            Return _libraryExists
        End Get
        Set(value As Boolean)
            _libraryExists = value
        End Set
    End Property

    ''' <summary>
    ''' リコメンド機能存在フラグの設定と取得を行う
    ''' </summary>
    ''' <value>リコメンド機能存在フラグ</value>
    ''' <returns>リコメンド機能存在フラグ</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property recommendCarRateVisible As Boolean
        Get
            Return _recommendCarRateVisible
        End Get
        Set(value As Boolean)
            _recommendCarRateVisible = value
        End Set
    End Property

End Class
