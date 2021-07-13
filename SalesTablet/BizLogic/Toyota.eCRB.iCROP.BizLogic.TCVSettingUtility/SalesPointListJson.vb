Imports System.Runtime.Serialization

''' <summary>
''' sales_point JSONファイル 全データ格納クラス
''' </summary>
''' <remarks></remarks>
<DataContract()>
<KnownType(GetType(SalesPointJson))>
Public Class SalesPointListJson
    Inherits AbstractJson

    Private _sales_point As List(Of SalesPointJson)
    Private _targetId As String
    Private _targetNo As String
    Private _timeStamp As String

    ''' <summary>
    ''' コンストラクタ
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        'リストの初期化
        _sales_point = New List(Of SalesPointJson)
    End Sub

    ''' <summary>
    ''' セールスポイント情報の設定と取得を行う
    ''' </summary>
    ''' <value>セールスポイント情報</value>
    ''' <returns>セールスポイント情報</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property sales_point As List(Of SalesPointJson)
        Get
            Return _sales_point
        End Get
        Set(value As List(Of SalesPointJson))
            _sales_point = value
        End Set
    End Property

    ''' <summary>
    ''' 対象セールスポイント番号の設定と取得を行う
    ''' </summary>
    ''' <value>対象セールスポイント番号(新規はブランク)</value>
    ''' <returns>対象セールスポイント番号(新規はブランク)</returns>
    ''' <remarks></remarks>
    Public Property TargetNo As String
        Get
            Return _targetNo
        End Get
        Set(value As String)
            _targetNo = value
        End Set
    End Property

    ''' <summary>
    ''' 対象セールスポイントIDの設定と取得を行う
    ''' </summary>
    ''' <value>対象セールスポイントID(新規はブランク)</value>
    ''' <returns>対象セールスポイントID(新規はブランク)</returns>
    ''' <remarks></remarks>
    Public Property TargetId As String
        Get
            Return _targetId
        End Get
        Set(value As String)
            _targetId = value
        End Set
    End Property

    ''' <summary>
    ''' ファイル更新日時の設定と取得を行う
    ''' </summary>
    ''' <value>ファイル更新日時</value>
    ''' <returns>ファイル更新日時</returns>
    ''' <remarks></remarks>
    Public Property TimeStamp As String
        Get
            Return _timeStamp
        End Get
        Set(value As String)
            _timeStamp = value
        End Set
    End Property

End Class
