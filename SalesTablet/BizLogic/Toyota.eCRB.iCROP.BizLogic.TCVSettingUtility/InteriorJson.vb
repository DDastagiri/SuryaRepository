Imports System.Runtime.Serialization

''' <summary>
''' interior JSONファイル インテリア情報格納クラス
''' </summary>
''' <remarks></remarks>
<DataContract()>
Public Class InteriorJson
    Inherits AbstractJson

    Dim _id As String
    Dim _grade As List(Of String)
    Dim _col_i As List(Of String)
    Dim _title As String
    Dim _img_back As String
    Dim _img_list As String

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
    ''' グレード適合の設定と取得を行う
    ''' </summary>
    ''' <value>グレード適合</value>
    ''' <returns>グレード適合</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property grade As List(Of String)
        Get
            Return _grade
        End Get
        Set(value As List(Of String))
            _grade = value
        End Set
    End Property

    ''' <summary>
    ''' 内装カラー適合の設定と取得を行う
    ''' </summary>
    ''' <value>内装カラー適合</value>
    ''' <returns>内装カラー適合</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property col_i As List(Of String)
        Get
            Return _col_i
        End Get
        Set(value As List(Of String))
            _col_i = value
        End Set
    End Property

    ''' <summary>
    ''' タイトルの設定と取得を行う
    ''' </summary>
    ''' <value>タイトル</value>
    ''' <returns>タイトル</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property title As String
        Get
            Return _title
        End Get
        Set(value As String)
            _title = value
        End Set
    End Property

    ''' <summary>
    ''' 背景画像の設定と取得を行う
    ''' </summary>
    ''' <value>背景画像</value>
    ''' <returns>背景画像</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property img_back As String
        Get
            Return _img_back
        End Get
        Set(value As String)
            _img_back = value
        End Set
    End Property

    ''' <summary>
    ''' 一覧画像の設定と取得を行う
    ''' </summary>
    ''' <value>ID</value>
    ''' <returns>ID</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property img_list As String
        Get
            Return _img_list
        End Get
        Set(value As String)
            _img_list = value
        End Set
    End Property

End Class
