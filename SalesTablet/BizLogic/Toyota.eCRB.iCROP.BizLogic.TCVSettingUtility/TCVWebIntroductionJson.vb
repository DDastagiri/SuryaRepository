Imports System.Runtime.Serialization

''' <summary>
''' tcv_web JSONファイル 環境データ格納クラス
''' </summary>
''' <remarks></remarks>
<DataContract()>
Public Class TcvWebIntroductionJson
    Inherits AbstractJson

    Private _resolution As String
    Private _angles As List(Of Integer)
    Private _init_angle As String

    ''' <summary>
    ''' 解像度の設定と取得を行う
    ''' </summary>
    ''' <value>解像度</value>
    ''' <returns>解像度</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property resolution() As String
        Get
            Return _resolution
        End Get
        Set(value As String)
            _resolution = value
        End Set
    End Property

    ''' <summary>
    ''' アングル数の設定と取得を行う
    ''' </summary>
    ''' <value>アングル数</value>
    ''' <returns>アングル数</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property angles() As List(Of Integer)
        Get
            Return _angles
        End Get
        Set(value As List(Of Integer))
            _angles = value
        End Set
    End Property

    ''' <summary>
    ''' 初期アングルの設定と取得を行う
    ''' </summary>
    ''' <value>初期アングル</value>
    ''' <returns>初期アングル</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property init_angle() As String
        Get
            Return _init_angle
        End Get
        Set(value As String)
            _init_angle = value
        End Set
    End Property

End Class
