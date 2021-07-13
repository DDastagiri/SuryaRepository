Imports System.Runtime.Serialization

''' <summary>
''' tcv_web JSONファイル パーツ情報データ格納クラス
''' </summary>
''' <remarks></remarks>
<DataContract()>
Public Class TcvWebPartsJson
    Inherits AbstractJson

    Private _id As String
    Private _type As String
    Private _name As String
    Private _cd_t As String
    Private _cd_s As String
    Private _speckbn As String
    Private _div As String
    Private _price_tt As String
    Private _price_tf As String
    Private _price_st As String
    Private _price_sf As String
    Private _cd3 As String
    Private _img_t As String
    Private _img_s As String
    Private _pb_id As String
    Private _grd As List(Of String)
    Private _col_e As List(Of String())
    Private _grp As String
    Private _set As List(Of String)
    Private _col_e_t As List(Of String)
    Private _btn_flg As String
    Private _est_flg As String
    Private _img_0 As String
    Private _img_1 As String
    Private _asc As String

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
    ''' 追工コード（塗装済）の設定と取得を行う
    ''' </summary>
    ''' <value>追工コード（塗装済）</value>
    ''' <returns>追工コード（塗装済）</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property cd_t As String
        Get
            Return _cd_t
        End Get
        Set(value As String)
            _cd_t = value
        End Set
    End Property

    ''' <summary>
    ''' 追工コード（素地）の設定と取得を行う
    ''' </summary>
    ''' <value>追工コード（素地）</value>
    ''' <returns>追工コード（素地）</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property cd_s As String
        Get
            Return _cd_s
        End Get
        Set(value As String)
            _cd_s = value
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
    ''' 税込塗装済価格の設定と取得を行う
    ''' </summary>
    ''' <value>税込塗装済価格</value>
    ''' <returns>税込塗装済価格</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property price_tt As String
        Get
            Return _price_tt
        End Get
        Set(value As String)
            _price_tt = value
        End Set
    End Property

    ''' <summary>
    ''' 税抜塗装済価格の設定と取得を行う
    ''' </summary>
    ''' <value>税抜塗装済価格</value>
    ''' <returns>税抜塗装済価格</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property price_tf As String
        Get
            Return _price_tf
        End Get
        Set(value As String)
            _price_tf = value
        End Set
    End Property

    ''' <summary>
    ''' 税込素地価格の設定と取得を行う
    ''' </summary>
    ''' <value>税込素地価格</value>
    ''' <returns>税込素地価格</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property price_st As String
        Get
            Return _price_st
        End Get
        Set(value As String)
            _price_st = value
        End Set
    End Property

    ''' <summary>
    ''' 税抜素地価格の設定と取得を行う
    ''' </summary>
    ''' <value>税抜素地価格</value>
    ''' <returns>税抜素地価格</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property price_sf As String
        Get
            Return _price_sf
        End Get
        Set(value As String)
            _price_sf = value
        End Set
    End Property

    ''' <summary>
    ''' 3Dオプション画像の設定と取得を行う
    ''' </summary>
    ''' <value>3Dオプション画像</value>
    ''' <returns>3Dオプション画像</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property cd3 As String
        Get
            Return _cd3
        End Get
        Set(value As String)
            _cd3 = value
        End Set
    End Property

    ''' <summary>
    ''' 塗装済ボタン画像の設定と取得を行う
    ''' </summary>
    ''' <value>塗装済ボタン画像</value>
    ''' <returns>塗装済ボタン画像</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property img_t As String
        Get
            Return _img_t
        End Get
        Set(value As String)
            _img_t = value
        End Set
    End Property

    ''' <summary>
    ''' 素地ボタン画像の設定と取得を行う
    ''' </summary>
    ''' <value>素地ボタン画像</value>
    ''' <returns>素地ボタン画像</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property img_s As String
        Get
            Return _img_s
        End Get
        Set(value As String)
            _img_s = value
        End Set
    End Property

    ''' <summary>
    ''' パーツ分類IDの設定と取得を行う
    ''' </summary>
    ''' <value>パーツ分類ID</value>
    ''' <returns>パーツ分類ID</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property pb_id As String
        Get
            Return _pb_id
        End Get
        Set(value As String)
            _pb_id = value
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
    ''' ボディカラー適合の設定と取得を行う
    ''' </summary>
    ''' <value>ボディカラー適合</value>
    ''' <returns>ボディカラー適合</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property col_e As List(Of String())
        Get
            Return _col_e
        End Get
        Set(value As List(Of String()))
            _col_e = value
        End Set
    End Property

    ''' <summary>
    ''' グループの設定と取得を行う
    ''' </summary>
    ''' <value>グループ</value>
    ''' <returns>グループ</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property grp As String
        Get
            Return _grp
        End Get
        Set(value As String)
            _grp = value
        End Set
    End Property

    ''' <summary>
    ''' セットパーツの設定と取得を行う
    ''' </summary>
    ''' <value>セットパーツ</value>
    ''' <returns>セットパーツ</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property [set] As List(Of String)
        Get
            Return _set
        End Get
        Set(value As List(Of String))
            _set = value
        End Set
    End Property

    ''' <summary>
    ''' 素地カラーの設定と取得を行う
    ''' </summary>
    ''' <value>素地カラー</value>
    ''' <returns>素地カラー</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property col_e_t As List(Of String)
        Get
            Return _col_e_t
        End Get
        Set(value As List(Of String))
            _col_e_t = value
        End Set
    End Property

    ''' <summary>
    ''' 表示フラグの設定と取得を行う
    ''' </summary>
    ''' <value>表示フラグ</value>
    ''' <returns>表示フラグ</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property btn_flg As String
        Get
            Return _btn_flg
        End Get
        Set(value As String)
            _btn_flg = value
        End Set
    End Property

    ''' <summary>
    ''' 見積明細表示フラグの設定と取得を行う
    ''' </summary>
    ''' <value>見積明細表示フラグ</value>
    ''' <returns>見積明細表示フラグ</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property est_flg As String
        Get
            Return _est_flg
        End Get
        Set(value As String)
            _est_flg = value
        End Set
    End Property

    ''' <summary>
    ''' パーツ画像0（着色済）の設定と取得を行う
    ''' </summary>
    ''' <value>パーツ画像0（着色済）</value>
    ''' <returns>パーツ画像0（着色済）</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property img_0 As String
        Get
            Return _img_0
        End Get
        Set(value As String)
            _img_0 = value
        End Set
    End Property

    ''' <summary>
    ''' パーツ画像1（非着色）の設定と取得を行う
    ''' </summary>
    ''' <value>パーツ画像1（非着色）</value>
    ''' <returns>パーツ画像1（非着色）</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property img_1 As String
        Get
            Return _img_1
        End Get
        Set(value As String)
            _img_1 = value
        End Set
    End Property

    ''' <summary>
    ''' 重ね合わせ順序の設定と取得を行う
    ''' </summary>
    ''' <value>重ね合わせ順序</value>
    ''' <returns>重ね合わせ順序</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property asc As String
        Get
            Return _asc
        End Get
        Set(value As String)
            _asc = value
        End Set
    End Property

End Class
