Imports System.Runtime.Serialization

''' <summary>
''' footer JSONファイル 画像ファイル情報格納クラス
''' </summary>
''' <remarks></remarks>
<DataContract()>
Public Class FooterImageJson
    Inherits AbstractJson

    Private _normal As String
    Private _on As String
    Private _disable As String

    ''' <summary>
    ''' ファイルの設定と取得を行う
    ''' </summary>
    ''' <value>normal</value>
    ''' <returns>normal</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property normal As String
        Get
            Return _normal
        End Get
        Set(value As String)
            _normal = value
        End Set
    End Property

    ''' <summary>
    ''' 選択時ファイルの設定と取得を行う
    ''' </summary>
    ''' <value>ID</value>
    ''' <returns>ID</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property [on] As String
        Get
            Return _on
        End Get
        Set(value As String)
            _on = value
        End Set
    End Property

    ''' <summary>
    ''' 選択不可時ファイルの設定と取得を行う
    ''' </summary>
    ''' <value>disable</value>
    ''' <returns>disable</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property disable As String
        Get
            Return _disable
        End Get
        Set(value As String)
            _disable = value
        End Set
    End Property

End Class
