Imports System.Runtime.Serialization

''' <summary>
''' tcv_web JSONファイル 深度情報データ格納クラス
''' </summary>
''' <remarks></remarks>
<DataContract()>
Public Class TcvWebInitDepthJson
    Inherits AbstractJson

    Private _id As String
    Private _depth As String

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
    ''' 深度の設定と取得を行う
    ''' </summary>
    ''' <value>深度</value>
    ''' <returns>深度</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property depth As String
        Get
            Return _depth
        End Get
        Set(value As String)
            _depth = value
        End Set
    End Property

End Class
