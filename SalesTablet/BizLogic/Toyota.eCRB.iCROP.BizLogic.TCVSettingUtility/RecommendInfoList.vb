
''' <summary>
''' リコメンド情報 全データ格納クラス
''' </summary>
''' <remarks></remarks>
Public Class RecommendInfoList

    Private _Root As List(Of RecommendInfo)
    Private _TimeStamp As String

    ''' <summary>
    ''' コンストラクタ
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        _Root = New List(Of RecommendInfo)
    End Sub

    ''' <summary>
    ''' リコメンド情報の設定と取得を行う
    ''' </summary>
    ''' <value>リコメンド情報</value>
    ''' <returns>リコメンド情報</returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Root As List(Of RecommendInfo)
        Get
            Return _Root
        End Get
    End Property

    ''' <summary>
    ''' ファイル更新日付の設定と取得を行う
    ''' </summary>
    ''' <value>ファイル更新日付</value>
    ''' <returns>ファイル更新日付</returns>
    ''' <remarks></remarks>
    Public Property TimeStamp As String
        Get
            Return _TimeStamp
        End Get
        Set(value As String)
            _TimeStamp = value
        End Set
    End Property
End Class
