Imports System.Runtime.Serialization

''' <summary>
''' tcv_web JSONファイル ファイル情報データ格納クラス
''' </summary>
''' <remarks></remarks>
<DataContract()>
Public Class TCVWebFileInfoJson
    Inherits AbstractJson

    Private _version As String
    Private _timestamp As String

    ''' <summary>
    ''' バージョンの設定と取得を行う
    ''' </summary>
    ''' <value>バージョン</value>
    ''' <returns>バージョン</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property version() As String
        Get
            Return _version
        End Get
        Set(value As String)
            _version = value
        End Set
    End Property

    ''' <summary>
    ''' タイムスタンプの設定と取得を行う
    ''' </summary>
    ''' <value>タイムスタンプ</value>
    ''' <returns>タイムスタンプ</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property Timestamp() As String
        Get
            Return _timestamp
        End Get
        Set(value As String)
            _timestamp = value
        End Set
    End Property

End Class
