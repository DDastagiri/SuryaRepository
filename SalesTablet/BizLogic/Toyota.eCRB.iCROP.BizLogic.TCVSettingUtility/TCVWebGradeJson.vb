Imports System.Runtime.Serialization

''' <summary>
''' tcv_web JSONファイル グレード情報データ格納クラス
''' </summary>
''' <remarks></remarks>
<DataContract()>
Public Class TcvWebGradeJson
    Inherits AbstractJson

    Private _id As String
    Private _type As String
    Private _model As String
    Private _sinkyu As String
    Private _drive As String
    Private _capacity As String
    Private _engine As String
    Private _mission As String
    Private _name As String
    Private _price As String
    Private _tax As String
    Private _def As String
    Private _base_img_0 As String
    Private _base_img_1 As String
    Private _car_type_cd As String
    Private _spec_seq As String
    Private _estsim_flg As String

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
    ''' 型式の設定と取得を行う
    ''' </summary>
    ''' <value>型式</value>
    ''' <returns>型式</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property model As String
        Get
            Return _model
        End Get
        Set(value As String)
            _model = value
        End Set
    End Property

    ''' <summary>
    ''' 新旧区分の設定と取得を行う
    ''' </summary>
    ''' <value>新旧区分</value>
    ''' <returns>新旧区分</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property sinkyu As String
        Get
            Return _sinkyu
        End Get
        Set(value As String)
            _sinkyu = value
        End Set
    End Property

    ''' <summary>
    ''' 駆動の設定と取得を行う
    ''' </summary>
    ''' <value>駆動</value>
    ''' <returns>駆動</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property drive As String
        Get
            Return _drive
        End Get
        Set(value As String)
            _drive = value
        End Set
    End Property

    ''' <summary>
    ''' 定員の設定と取得を行う
    ''' </summary>
    ''' <value>定員</value>
    ''' <returns>定員</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property capacity As String
        Get
            Return _capacity
        End Get
        Set(value As String)
            _capacity = value
        End Set
    End Property

    ''' <summary>
    ''' 排気量の設定と取得を行う
    ''' </summary>
    ''' <value>排気量</value>
    ''' <returns>排気量</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property engine As String
        Get
            Return _engine
        End Get
        Set(value As String)
            _engine = value
        End Set
    End Property

    ''' <summary>
    ''' ミッションの設定と取得を行う
    ''' </summary>
    ''' <value>ミッション</value>
    ''' <returns>ミッション</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property mission As String
        Get
            Return _mission
        End Get
        Set(value As String)
            _mission = value
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
    ''' 車種本体価格の設定と取得を行う
    ''' </summary>
    ''' <value>車種本体価格</value>
    ''' <returns>車種本体価格</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property price As String
        Get
            Return _price
        End Get
        Set(value As String)
            _price = value
        End Set
    End Property

    ''' <summary>
    ''' 課税タイプの設定と取得を行う
    ''' </summary>
    ''' <value>課税タイプ</value>
    ''' <returns>課税タイプ</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property tax As String
        Get
            Return _tax
        End Get
        Set(value As String)
            _tax = value
        End Set
    End Property

    ''' <summary>
    ''' マスグレードの設定と取得を行う
    ''' </summary>
    ''' <value>マスグレード</value>
    ''' <returns>マスグレード</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property def As String
        Get
            Return _def
        End Get
        Set(value As String)
            _def = value
        End Set
    End Property

    ''' <summary>
    ''' ベース車種画像0（着色済）の設定と取得を行う
    ''' </summary>
    ''' <value>ベース車種画像0（着色済）</value>
    ''' <returns>ベース車種画像0（着色済）</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property base_img_0 As String
        Get
            Return _base_img_0
        End Get
        Set(value As String)
            _base_img_0 = value
        End Set
    End Property

    ''' <summary>
    ''' ベース車種画像1（非着色）の設定と取得を行う
    ''' </summary>
    ''' <value>ベース車種画像1（非着色）</value>
    ''' <returns>ベース車種画像1（非着色）</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property base_img_1 As String
        Get
            Return _base_img_1
        End Get
        Set(value As String)
            _base_img_1 = value
        End Set
    End Property

    ''' <summary>
    ''' 車種タイプの設定と取得を行う
    ''' </summary>
    ''' <value>車種タイプ</value>
    ''' <returns>車種タイプ</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property car_type_cd As String
        Get
            Return _car_type_cd
        End Get
        Set(value As String)
            _car_type_cd = value
        End Set
    End Property

    ''' <summary>
    ''' スペックシーケンスの設定と取得を行う
    ''' </summary>
    ''' <value>スペックシーケンス</value>
    ''' <returns>スペックシーケンス</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property spec_seq As String
        Get
            Return _spec_seq
        End Get
        Set(value As String)
            _spec_seq = value
        End Set
    End Property

    ''' <summary>
    ''' 見積シミュレーション有効フラグの設定と取得を行う
    ''' </summary>
    ''' <value>見積シミュレーション有効フラグ</value>
    ''' <returns>見積シミュレーション有効フラグ</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property estsim_flg As String
        Get
            Return _estsim_flg
        End Get
        Set(value As String)
            _estsim_flg = value
        End Set
    End Property

End Class
