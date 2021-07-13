Imports System.Runtime.Serialization

''' <summary>
''' tcv_web JSONファイル 追加画像情報データ格納クラス
''' </summary>
''' <remarks></remarks>
<DataContract()>
Public Class TcvWebAddImgJson
    Inherits AbstractJson

    Private _id As String
    Private _id_a As String
    Private _id_b As String
    Private _img_0 As String
    Private _img_1 As String

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
    Public Property id_b As String
        Get
            Return _id_b
        End Get
        Set(value As String)
            _id_b = value
        End Set
    End Property

    ''' <summary>
    ''' 画像0（着色済）の設定と取得を行う
    ''' </summary>
    ''' <value>画像0（着色済）</value>
    ''' <returns>画像0（着色済）</returns>
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
    ''' 画像1（非着色）の設定と取得を行う
    ''' </summary>
    ''' <value>画像1（非着色）</value>
    ''' <returns>画像1（非着色）</returns>
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

End Class
