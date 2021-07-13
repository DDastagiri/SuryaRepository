Imports System.Runtime.Serialization

''' <summary>
''' tcv_web JSONファイル 排他情報データ格納クラス
''' </summary>
''' <remarks></remarks>
<DataContract()>
Public Class TcvWebHaitaJson
    Inherits AbstractJson

    Private _id_a As String
    Private _id_b As List(Of String)
    Private _type As String
    Private _mes_type As String
    Private _mes As String
    Private _grd As List(Of String)
    Private _col_e As List(Of String())
    Private _prt As List(Of String())
    Private _prt_jyogai As String

    ''' <summary>
    ''' ID_Aの設定と取得を行う
    ''' </summary>
    ''' <value>ID_A</value>
    ''' <returns>ID_A</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property id_a As String
        Get
            Return _id_a
        End Get
        Set(value As String)
            _id_a = value
        End Set
    End Property

    ''' <summary>
    ''' ID_Bの設定と取得を行う
    ''' </summary>
    ''' <value>ID_B</value>
    ''' <returns>ID_B</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property id_b As List(Of String)
        Get
            Return _id_b
        End Get
        Set(value As List(Of String))
            _id_b = value
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
    ''' メッセージ種別の設定と取得を行う
    ''' </summary>
    ''' <value>メッセージ種別</value>
    ''' <returns>メッセージ種別</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property mes_type As String
        Get
            Return _mes_type
        End Get
        Set(value As String)
            _mes_type = value
        End Set
    End Property

    ''' <summary>
    ''' メッセージの設定と取得を行う
    ''' </summary>
    ''' <value>メッセージ</value>
    ''' <returns>メッセージ</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property mes As String
        Get
            Return _mes
        End Get
        Set(value As String)
            _mes = value
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
    ''' パーツ適合の設定と取得を行う
    ''' </summary>
    ''' <value>パーツ適合</value>
    ''' <returns>パーツ適合</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property prt As List(Of String())
        Get
            Return _prt
        End Get
        Set(value As List(Of String()))
            _prt = value
        End Set
    End Property

    ''' <summary>
    ''' 除外パーツの設定と取得を行う
    ''' </summary>
    ''' <value>除外パーツ</value>
    ''' <returns>除外パーツ</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property prt_jyogai As String
        Get
            Return _prt_jyogai
        End Get
        Set(value As String)
            _prt_jyogai = value
        End Set
    End Property

End Class
