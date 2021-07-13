'Copyright c2009 Toyota Motor Corporation. All rights reserved. For internal use only.
Imports System
Imports System.Collections.Generic
Imports System.Configuration
Imports System.Globalization
Imports System.Text
Imports System.Web
Imports System.Web.UI
Imports System.Diagnostics.CodeAnalysis

Namespace Toyota.eCRB.SystemFrameworks.Web

    ''' <summary>
    ''' 履歴型サイトマッププロバイダです。
    ''' </summary>
    ''' <remarks>
    ''' 遷移したページの情報をセッションに保持します。
    ''' </remarks>
    Public Class HistorySiteMapProvider
        Inherits System.Web.SiteMapProvider

        ''' <summary>
        ''' 画面遷移履歴リストのSessionキー名
        ''' </summary>
        Public Const SESSION_KEY_PAGE_HISTORY_LIST As String = _
            "Toyota.eCRB.SystemFrameworks.Web.HistorySiteMapProvider.PageHistoryList"
        ''' <summary>
        ''' 次画面引継ぎDictionary(Of String, Object)のSessionキー名
        ''' </summary>
        Public Const SESSION_KEY_NEXT_PAGE_INFO As String = _
            "Toyota.eCRB.SystemFrameworks.Web.HistorySiteMapProvider.NextPageInfo"

        ''' <summary>
        ''' メインメニュー用Session保持データ
        ''' Dictionary(Of String, Object)のSessionキー名
        ''' </summary>
        ''' <remarks></remarks>
        Public Const SESSION_KEY_MAINMENU_CONTEXT As String = _
            "Toyota.eCRB.SystemFrameworks.AppService.BasePage.MainMenuContext"

        ''' <summary>
        ''' 指定した URL のページを表す SiteMapNode オブジェクトを取得します。 
        ''' </summary>
        ''' <param name="rawUrl">SiteMapNode の取得対象ページを示す URL。</param>
        ''' <returns>rawURL で示されるページを表す SiteMapNode。</returns>
        ''' <remarks></remarks>
        Public Overloads Overrides Function FindSiteMapNode( _
            ByVal rawUrl As String) As SiteMapNode

            Dim nodeList As List(Of SerializableSiteMapNode) = _
                HistorySiteMapProvider.SiteMapNodeList

            If nodeList Is Nothing Then
                '指定した URL のページを表す SiteMapNode オブジェクトを取得処理のループを開始
                For i As Integer = nodeList.Count - 1 To 0 Step -1
                    If nodeList(i).Url.Equals(rawUrl) Then
                        Return ConvertSiteMapNode(nodeList(i), i)
                    End If
                Next
                '指定した URL のページを表す SiteMapNode オブジェクトを取得処理のループを終了
            End If

            Return Nothing
        End Function

        ''' <summary>
        ''' 特定の SiteMapNode の子ノードを取得します。
        ''' </summary>
        ''' <param name="node">すべての子ノードを取得する対象の SiteMapNode。</param>
        ''' <returns>指定した SiteMapNode の直接の子ノードが格納されている読み取り専用の SiteMapNodeCollection。</returns>
        ''' <remarks></remarks>
        Public Overrides Function GetChildNodes( _
            ByVal node As System.Web.SiteMapNode) As System.Web.SiteMapNodeCollection

            Dim nodeList As List(Of SerializableSiteMapNode) = _
                HistorySiteMapProvider.SiteMapNodeList
            Dim col As SiteMapNodeCollection = New SiteMapNodeCollection()
            Dim current As Boolean = False

            '特定の SiteMapNode の子ノードを取得処理のループを開始
            For i As Integer = 0 To nodeList.Count - 1
                If current Then
                    col.Add(ConvertSiteMapNode(nodeList(i), i))
                ElseIf nodeList(i).Key.Equals(node.Key) Then
                    current = True
                End If
            Next
            '特定の SiteMapNode の子ノードを取得処理のループを終了

            Return col
        End Function

        ''' <summary>
        ''' 特定の SiteMapNode オブジェクトの親ノードを取得します。 
        ''' </summary>
        ''' <param name="node">親ノードを取得する対象の SiteMapNode。</param>
        ''' <returns>node の親を表す SiteMapNode。</returns>
        ''' <remarks></remarks>
        Public Overrides Function GetParentNode( _
            ByVal node As System.Web.SiteMapNode) As System.Web.SiteMapNode

            Dim nodeList As List(Of SerializableSiteMapNode) = _
                HistorySiteMapProvider.SiteMapNodeList

            '特定の SiteMapNode オブジェクトの親ノードを取得処理のループを開始
            For i As Integer = nodeList.Count - 1 To 1 Step -1
                If nodeList(i).Key.Equals(node.Key) Then
                    Return ConvertSiteMapNode(nodeList(i - 1), i - 1)
                End If
            Next
            '特定の SiteMapNode オブジェクトの親ノードを取得処理のループを終了

            Return Nothing
        End Function

        ''' <summary>
        ''' 現在のプロバイダによって現在管理されている全ノードのルート ノードを取得します。 
        ''' </summary>
        ''' <returns>この処理は使われない為、Nothingを返します。</returns>
        ''' <remarks></remarks>
        Protected Overrides Function GetRootNodeCore() As System.Web.SiteMapNode
            Return Nothing
        End Function

        ''' <summary>
        ''' サイトマップノードのリストを取得します。
        ''' </summary>
        ''' <returns>サイトマップノードのリスト</returns>
        ''' <remarks>
        ''' 遷移したページの情報をセッションに保持します。
        ''' </remarks>
        Public Shared ReadOnly Property SiteMapNodeList() As List(Of SerializableSiteMapNode)
            Get
                Dim nodeList As List(Of SerializableSiteMapNode) _
                    = DirectCast(HttpContext.Current.Session(SESSION_KEY_PAGE_HISTORY_LIST),  _
                        List(Of SerializableSiteMapNode))

                If nodeList Is Nothing Then
                    nodeList = New List(Of SerializableSiteMapNode)
                    HttpContext.Current.Session(SESSION_KEY_PAGE_HISTORY_LIST) = nodeList
                End If

                Return nodeList
            End Get
        End Property

        ''' <summary>
        ''' 引数のページSession情報にて、SerializableSiteMapNodeを生成し、画面遷移履歴リストに追加します。
        ''' </summary>
        ''' <param name="url">画面表示にリクエストされたUrl</param>
        ''' <param name="title">パンくず表示に使用する画面名</param>
        ''' <param name="pageSessionInfo">ページSession情報を格納したDictionary(Of String, Object)</param>
        ''' <remarks></remarks>
        Public Shared Sub AddNewNode( _
            ByVal url As String, _
            ByVal title As String, _
            ByVal pageSessionInfo As Dictionary(Of String, Object))
            Dim nodeList As List(Of SerializableSiteMapNode) = _
                HistorySiteMapProvider.SiteMapNodeList
            nodeList.Add( _
                New SerializableSiteMapNode(Guid.NewGuid.ToString, url, title, pageSessionInfo))
        End Sub

        ''' <summary>
        ''' サイトマップノードのリストを全て削除します。
        ''' </summary>
        ''' <remarks></remarks>
        ''' 
        <SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Scope:="member", _
        Justification:="これは内部でスタテックメンバのみアクセスであるが、_意味的にはスタティックメソッドではないので除外する。")> _
        Public Sub Clear()
            HttpContext.Current.Session.Remove(SESSION_KEY_PAGE_HISTORY_LIST)
        End Sub

        ''' <summary>
        ''' SerializableSiteMapNodeを、ASP.NET標準のSiteMapNodeに変換します。
        ''' </summary>
        ''' <param name="node">変換対象のSerializableSiteMapNode</param>
        ''' <param name="index">変換対象Nodeの画面遷移履歴Listの位置</param>
        ''' <returns>変換されたSiteMapNode</returns>
        ''' <remarks></remarks>
        Private Function ConvertSiteMapNode( _
            ByVal node As SerializableSiteMapNode, _
            ByVal index As Integer) As SiteMapNode
            Dim nodeTitle As String

            nodeTitle = node.Title
            Dim siteMapNode As SiteMapNode = New SiteMapNode(Me, node.Key, node.Url, nodeTitle)
            siteMapNode.Description = index.ToString(CultureInfo.InvariantCulture)
            Return siteMapNode
        End Function


    End Class
End Namespace