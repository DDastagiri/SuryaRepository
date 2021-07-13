
''' <summary>
''' 履歴ファイル 全データ格納クラス
''' </summary>
''' <remarks></remarks>
Public Class ReplicationFileRoot

    Private _Root As List(Of ReplicationFileInfo)

    ''' <summary>
    ''' コンストラクタ
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        _Root = New List(Of ReplicationFileInfo)
    End Sub

    ''' <summary>
    ''' 操作区分の設定と取得を行う
    ''' </summary>
    ''' <value>操作区分</value>
    ''' <returns>操作区分</returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Root As List(Of ReplicationFileInfo)
        Get
            If IsNothing(_Root) Then
                Return New List(Of ReplicationFileInfo)
            End If
            Return _Root
        End Get
    End Property

End Class
