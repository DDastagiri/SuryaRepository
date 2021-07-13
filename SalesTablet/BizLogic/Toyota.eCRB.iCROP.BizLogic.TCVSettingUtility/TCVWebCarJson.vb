Imports System.Runtime.Serialization

''' <summary>
''' tcv_web JSONファイル 車種情報データ格納クラス
''' </summary>
''' <remarks></remarks>
<DataContract()>
Public Class TcvWebCarJson
    Inherits AbstractJson

    Private _name As String
    Private _name_en As String
    Private _car_name_cd As String
    Private _echoice_no As String
    Private _catalog_year As String
    Private _catalog_month As String

    ''' <summary>
    ''' 車種名称（英語）の設定と取得を行う
    ''' </summary>
    ''' <value>車種名称（英語）</value>
    ''' <returns>車種名称（英語）</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property name() As String
        Get
            Return _name
        End Get
        Set(value As String)
            _name = value
        End Set
    End Property

    ''' <summary>
    ''' 車種名称（英語）の設定と取得を行う
    ''' </summary>
    ''' <value>車種名称（英語）</value>
    ''' <returns>車種名称（英語）</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property name_en() As String
        Get
            Return _name_en
        End Get
        Set(value As String)
            _name_en = value
        End Set
    End Property

    ''' <summary>
    ''' 車種名称コードの設定と取得を行う
    ''' </summary>
    ''' <value>車種名称コード</value>
    ''' <returns>車種名称コード</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property car_name_cd() As String
        Get
            Return _car_name_cd
        End Get
        Set(value As String)
            _car_name_cd = value
        End Set
    End Property

    ''' <summary>
    ''' E-CHOICE番号の設定と取得を行う
    ''' </summary>
    ''' <value>E-CHOICE番号</value>
    ''' <returns>E-CHOICE番号</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property echoice_no() As String
        Get
            Return _echoice_no
        End Get
        Set(value As String)
            _echoice_no = value
        End Set
    End Property

    ''' <summary>
    ''' カタログ年の設定と取得を行う
    ''' </summary>
    ''' <value>カタログ年</value>
    ''' <returns>カタログ年</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property catalog_year() As String
        Get
            Return _catalog_year
        End Get
        Set(value As String)
            _catalog_year = value
        End Set
    End Property

    ''' <summary>
    ''' カタログ月の設定と取得を行う
    ''' </summary>
    ''' <value>カタログ月</value>
    ''' <returns>カタログ月</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property catalog_month() As String
        Get
            Return _catalog_month
        End Get
        Set(value As String)
            _catalog_month = value
        End Set
    End Property

End Class
