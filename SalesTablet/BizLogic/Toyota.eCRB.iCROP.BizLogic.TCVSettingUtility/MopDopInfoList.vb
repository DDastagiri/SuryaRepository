''' <summary>
''' MOP/DOP情報データ格納クラス
''' </summary>
''' <remarks></remarks>
Public Class MopDopInfoList

    Private _mopDopInfoList As List(Of MopDopInfo)
    Private _timeStamp As String

    ''' <summary>
    ''' MOP/DOP一覧情報の取得を行います。
    ''' </summary>
    ''' <value>MOP/DOP一覧情報</value>
    ''' <returns>MOP/DOP一覧情報</returns>
    ''' <remarks></remarks>
    Public ReadOnly Property MopDopInfoList As List(Of MopDopInfo)
        Get
            If IsNothing(Me._mopDopInfoList) Then
                Return New List(Of MopDopInfo)
            End If
            Return Me._mopDopInfoList
        End Get
    End Property

    ''' <summary>
    ''' ファイルの更新日時の取得と設定を行います。
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

    ''' <summary>
    ''' MOP/DOP一覧情報の設定を行います。
    ''' </summary>
    ''' <param name="mopDopInfoList">MOP/DOP一覧情報</param>
    ''' <remarks>CI対策</remarks>
    Public Sub SetMopDopInfoList(ByVal mopDopInfoList As List(Of MopDopInfo))
        Me._mopDopInfoList = mopDopInfoList
    End Sub

End Class
