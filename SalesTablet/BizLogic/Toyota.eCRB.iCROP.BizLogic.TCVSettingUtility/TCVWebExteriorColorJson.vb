Imports System.Runtime.Serialization

''' <summary>
''' tcv_web JSONファイル ボディカラー情報データ格納クラス
''' </summary>
''' <remarks></remarks>
<DataContract()>
Public Class TcvWebExteriorColorJson
    Inherits AbstractJson

    Private _id As String
    Private _type As String
    Private _name As String
    Private _cd As String
    Private _speckbn As String
    Private _div As String
    Private _price_t As String
    Private _price_f As String
    Private _img As String
    Private _grd As List(Of String)
    Private _color As String
    Private _baseBn As String
    Private _baseCon As String

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
    ''' 種別の設定と取得を行う
    ''' </summary>
    ''' <value>種別</value>
    ''' <returns>種別</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property type As String
        Get
            Return _type
        End Get
        Set(value As String)
            _type = value
        End Set
    End Property

    ''' <summary>
    ''' 名称の設定と取得を行う
    ''' </summary>
    ''' <value>名称</value>
    ''' <returns>名称</returns>
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
    ''' カラーコードの設定と取得を行う
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property cd As String
        Get
            Return _cd
        End Get
        Set(value As String)
            _cd = value
        End Set
    End Property

    ''' <summary>
    ''' スペック区分の設定と取得を行う
    ''' </summary>
    ''' <value>スペック区分</value>
    ''' <returns>スペック区分</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property speckbn As String
        Get
            Return _speckbn
        End Get
        Set(value As String)
            _speckbn = value
        End Set
    End Property

    ''' <summary>
    ''' 区分の設定と取得を行う
    ''' </summary>
    ''' <value>区分</value>
    ''' <returns>区分</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property div As String
        Get
            Return _div
        End Get
        Set(value As String)
            _div = value
        End Set
    End Property

    ''' <summary>
    ''' 税込価格の設定と取得を行う
    ''' </summary>
    ''' <value>税込価格</value>
    ''' <returns>税込価格</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property price_t As String
        Get
            Return _price_t
        End Get
        Set(value As String)
            _price_t = value
        End Set
    End Property

    ''' <summary>
    ''' 税抜価格の設定と取得を行う
    ''' </summary>
    ''' <value>税抜価格</value>
    ''' <returns>税抜価格</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property price_f As String
        Get
            Return _price_f
        End Get
        Set(value As String)
            _price_f = value
        End Set
    End Property

    ''' <summary>
    ''' ボタン画像の設定と取得を行う
    ''' </summary>
    ''' <value>ボタン画像</value>
    ''' <returns>ボタン画像</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property img As String
        Get
            Return _img
        End Get
        Set(value As String)
            _img = value
        End Set
    End Property

    ''' <summary>
    ''' グレード適合の設定と取得を行う
    ''' </summary>
    ''' <value>グレード適合</value>
    ''' <returns>グレード適合</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property grd As List(Of String)
        Get
            Return _grd
        End Get
        Set(value As List(Of String))
            _grd = value
        End Set
    End Property

    ''' <summary>
    ''' RGBの設定と取得を行う
    ''' </summary>
    ''' <value>RGB</value>
    ''' <returns>RGB</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property color As String
        Get
            Return _color
        End Get
        Set(value As String)
            _color = value
        End Set
    End Property

    ''' <summary>
    ''' 明度の設定と取得を行う
    ''' </summary>
    ''' <value>明度</value>
    ''' <returns>明度</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property baseBn As String
        Get
            Return _baseBn
        End Get
        Set(value As String)
            _baseBn = value
        End Set
    End Property

    ''' <summary>
    ''' コントラストの設定と取得を行う
    ''' </summary>
    ''' <value>コントラスト</value>
    ''' <returns>コントラスト</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property baseCon As String
        Get
            Return _baseCon
        End Get
        Set(value As String)
            _baseCon = value
        End Set
    End Property

End Class
