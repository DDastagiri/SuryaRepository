Imports System.Runtime.Serialization

''' <summary>
''' tcv_web JSONファイル パーツ分類情報データ格納クラス
''' </summary>
''' <remarks></remarks>
<DataContract()>
Public Class TcvWebPartsBunruiJson
    Inherits AbstractJson

    Private _name As String
    Private _id As String
    Private _type As String
    Private _img As String
    Private _def As String

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
    ''' デフォルトの設定と取得を行う
    ''' </summary>
    ''' <value>デフォルト</value>
    ''' <returns>デフォルト</returns>
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

End Class
