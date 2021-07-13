Imports System.Runtime.Serialization

''' <summary>
''' footer JSONファイル フッター情報格納クラス
''' </summary>
''' <remarks></remarks>
<DataContract()>
Public Class FooterListJson
    Inherits AbstractJson

    Private _timeStamp As String
    Private _default_URI_scheme As String
    Private _footerMap As List(Of FooterJson)

    ''' <summary>
    ''' 更新日時の取得と設定を行います。
    ''' </summary>
    ''' <value>更新日時</value>
    ''' <returns>更新日時</returns>
    ''' <remarks></remarks>
    Public Property TimeStamp As String
        Get
            Return _timeStamp
        End Get
        Set(value As String)
            _timeStamp = value
        End Set
    End Property

    ''' <summary>
    ''' デフォルトURLスキーマの取得と設定を行います。
    ''' </summary>
    ''' <value>デフォルトURLスキーマ</value>
    ''' <returns>デフォルトURLスキーマ</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property default_URI_scheme As String
        Get
            Return _default_URI_scheme
        End Get
        Set(value As String)
            _default_URI_scheme = value
        End Set
    End Property

    ''' <summary>
    ''' フッターマップ情報の設定と取得を行う
    ''' </summary>
    ''' <value>フッターマップ情報</value>
    ''' <returns>フッターマップ情報</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property footerMap As List(Of FooterJson)
        Get
            If IsNothing(_footerMap) Then
                Return New List(Of FooterJson)
            End If
            Return _footerMap
        End Get
        Set(value As List(Of FooterJson))
            _footerMap = value
        End Set
    End Property

End Class
