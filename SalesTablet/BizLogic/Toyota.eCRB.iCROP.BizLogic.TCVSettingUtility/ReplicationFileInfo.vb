
''' <summary>
''' 履歴ファイル 履歴情報格納クラス
''' </summary>
''' <remarks></remarks>
Public Class ReplicationFileInfo

    Dim _FileAccess As String
    Dim _FilePath As String

    ''' <summary>
    ''' ファイルアクセス区分の設定と取得を行う
    ''' </summary>
    ''' <value>ファイルアクセス区分</value>
    ''' <returns>ファイルアクセス区分</returns>
    ''' <remarks></remarks>
    Public Property FileAccess As String
        Get
            Return _FileAccess
        End Get
        Set(value As String)
            _FileAccess = value
        End Set
    End Property

    ''' <summary>
    ''' ファイルパスの設定と取得を行う
    ''' </summary>
    ''' <value>ファイルパス</value>
    ''' <returns>ファイルパス</returns>
    ''' <remarks></remarks>
    Public Property FilePath As String
        Get
            Return _FilePath
        End Get
        Set(value As String)
            _FilePath = value
        End Set
    End Property

End Class
