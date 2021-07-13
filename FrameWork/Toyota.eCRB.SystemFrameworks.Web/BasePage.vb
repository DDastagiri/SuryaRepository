'Copyright c2009 Toyota Motor Corporation. All rights reserved. For internal use only.
Imports System.ComponentModel
Imports System.Globalization
Imports System.IO
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Web
Imports System.Web.HttpContext
Imports System.Web.UI
Imports System.Text
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Configuration

Namespace Toyota.eCRB.SystemFrameworks.Web

    ''' <summary>
    ''' ダイアログ表示のエフェクトを表す列挙型
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum DialogEffect As Integer
        ''' <summary>
        ''' 中央にフェードイン
        ''' </summary>
        FadeIn = 0
        ''' <summary>
        ''' 左からのスライドイン
        ''' </summary>
        Left = 1
        ''' <summary>
        ''' 右からのスライドイン
        ''' </summary>
        Right = 2
        ''' <summary>
        ''' 上からのスライドイン
        ''' </summary>
        Top = 3
        ''' <summary>
        ''' 下からのスライドイン
        ''' </summary>
        Bottom = 4
    End Enum

    ''' <summary>
    ''' 画面遷移履歴での位置を表す列挙型
    ''' Prev    :現在表示している画面の１つ前にある画面位置
    ''' Current :現在表示している画面位置
    ''' [Next]  :現在表示している画面の次に表示しようとしている画面位置
    ''' Last    :履歴上にある現在の画面と同一で、直近の画面位置
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum ScreenPos As Integer
        Prev = 0
        Current = 1
        [Next] = 2
        Last = 3
    End Enum

    ''' <summary>
    ''' 基底プレゼンテーションクラスです。共通で使用する機能を提供します。
    ''' </summary>
    ''' <remarks>
    ''' アプリケーションでは Web ページクラスを作成するとき
    ''' <see cref="System.Web.UI.Page"/> ではなく、
    ''' このクラスを基底クラスとしてください。
    ''' </remarks>
    Public MustInherit Class BasePage
        Inherits System.Web.UI.Page

        ''' <summary>
        ''' 共通基盤管理用トップページURLのセッションキー
        ''' </summary>
        ''' <remarks></remarks>
        Private Const SESSION_TOPPAGE As String = "Toyota.eCRB.SystemFrameworks.Web.BasePage.TopPage"

        Private ReadOnly Property CommonMaster As CommonMasterPage
            Get
                Dim m As MasterPage = Me.Master
                Do While (m IsNot Nothing)
                    If (TypeOf m Is CommonMasterPage) Then
                        Return CType(m, CommonMasterPage)
                    End If
                    m = m.Master
                Loop
                Return Nothing
            End Get
        End Property

        ''' <summary>
        ''' 表示するフッターボタンを宣言します。
        ''' </summary>
        ''' <param name="commonMaster"></param>
        ''' <param name="category">ページが属するメニューカテゴリ（派生クラスが設定します）</param>
        ''' <returns>フッターボタンIDの配列</returns>
        ''' <remarks>このメソッドは、派生クラスがオーバーライドする必要があります。</remarks>
        Public Overridable Function DeclareCommonMasterFooter(ByVal commonMaster As CommonMasterPage, ByRef category As FooterMenuCategory) As Integer()
            Return New Integer() {}
        End Function

        ''' <summary>
        ''' 表示するコンテキストメニュー項目を宣言します。
        ''' </summary>
        ''' <param name="commonMaster"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Function DeclareCommonMasterContextMenu(ByVal commonMaster As CommonMasterPage) As Integer()
            Return New Integer() {CommonMasterContextMenuBuiltinMenuID.StandByItem, CommonMasterContextMenuBuiltinMenuID.SuspendItem, CommonMasterContextMenuBuiltinMenuID.LogoutItem}
        End Function

        Protected Overrides Sub OnPreRender(ByVal e As System.EventArgs)
            MyBase.OnPreRender(e)

            'icropScript.ui.account を設定
            Dim account As String = ""
            Try
                account = StaffContext.Current.Account
            Catch ex As InvalidOperationException
                '未ログイン
            End Try

            Dim script As String = String.Format(CultureInfo.InvariantCulture, "icropScript.ui.account = '{0}';", account)
            ClientScript.RegisterStartupScript(GetType(BasePage), "icropScript.ui.account", script, True)

        End Sub

#Region " プロパティ "
        ''' <summary>
        ''' ページプロパティの返答値を上書きし、本クラスのインスタンス自身をプロパティ値とします。
        ''' </summary>
        ''' <value></value>
        ''' <returns>本クラスのインスタンス自身をプロパティ値として戻します。</returns>
        ''' <remarks></remarks>
        Public Shadows ReadOnly Property Page() As BasePage
            Get
                Return Me
            End Get
        End Property

        ''' <summary>
        ''' 自画面が操作ロック中かどうかを返します。自画面がICustomerFormインタフェースを実装していない場合は、常にFalseです。
        ''' </summary>
        ''' <returns>True：ロック中　False：ロック中でない</returns>
        ''' <remarks></remarks>
        Protected ReadOnly Property OperationLocked() As Boolean
            Get
                If TypeOf Me Is ICustomerForm Then
                    'ICustomerFormを実装している画面
                    Dim master As CommonMasterPage = Me.CommonMaster
                    If (master IsNot Nothing) Then
                        '画面ロックのチェック状態を返却
                        Return master.OperationLocked.Value.Equals("1")
                    Else
                        'CommonMasterPage以外
                        Return False
                    End If
                Else
                    'ICustomerFormを実装していない画面
                    Return False
                End If
            End Get
        End Property
#End Region

#Region "Session操作"
        Private _nextPageInfo As Dictionary(Of String, Object)
        Private _prevPageInfo As Dictionary(Of String, Object)
        Private _lastPageInfo As Dictionary(Of String, Object)
        Private _currentPageInfo As Dictionary(Of String, Object)

        ''' <summary>
        ''' Sessionへ検索条件等のデータを格納します。
        ''' </summary>
        ''' <param name="pos">画面位置を表す列挙型。</param>
        ''' <param name="key">Sessionに格納するObjectのキー名。</param>
        ''' <param name="value">Sessionに格納するOblect</param>
        ''' <remarks></remarks>
        ''' <exception cref="ArgumentNullException">
        ''' 引数「key」にNothingを指定した場合にスローされます。
        ''' </exception>
        ''' <exception cref=" InvalidOperationException">
        ''' 遷移元が存在しない場合にscreenPosに「Prev」「Last」を指定した場合にスローされます。
        ''' </exception>
        Protected Sub SetValue( _
            ByVal pos As ScreenPos, _
            ByVal key As String, _
            ByVal value As Object)

            '引数keyがNothingの場合
            If key Is Nothing Then
                '例外としてArgumentNullExceptionをスローする
                Throw New ArgumentNullException("key")
            End If

            Dim pageInfo As Dictionary(Of String, Object)

            '引数screenPos判定
            Select Case pos
                Case ScreenPos.Next    'screenPosがNext(次画面）の場合
                    pageInfo = Me.NextPageInfo

                Case ScreenPos.Current 'screenPosがCurrent(自画面）の場合
                    pageInfo = Me.CurrentPageInfo

                Case ScreenPos.Prev    'screenPosがPrev（前画面）の場合
                    pageInfo = Me.PrevPageInfo

                Case Else   'screenPosがLast（直近の同一画面）の場合
                    pageInfo = Me.LastPageInfo

                    '直近同一画面引渡しDictionary(Of String, Object)がNothingの場合
                    If pageInfo Is Nothing Then
                        Throw New InvalidOperationException
                    End If

            End Select

            pageInfo.Item(key) = value

        End Sub

        ''' <summary>
        ''' Sessionから検索条件等のデータを取得します。
        ''' </summary>
        ''' <param name="pos">画面位置を表す列挙型。</param>
        ''' <param name="key">Sessionに格納されているObjectのキー名。</param>
        ''' <param name="removeFlg">Sessionに格納されているデータを取り出した後、
        ''' 削除したい場合はTrue、それ以外はFalseを指定。</param>
        ''' <returns>Sessionに格納されているObject</returns>
        ''' <remarks></remarks>
        ''' <exception cref="ArgumentNullException">
        ''' 引数「key」にNothingを指定した場合にスローされます。
        ''' </exception>
        ''' <exception cref=" InvalidOperationException">
        ''' 遷移元が存在しない場合にscreenPosに「Prev」を指定した場合にスローされます。
        ''' </exception>
        Protected Function GetValue( _
            ByVal pos As ScreenPos, _
            ByVal key As String, _
            ByVal removeFlg As Boolean) As Object

            '引数keyがNothingの場合
            If key Is Nothing Then
                '例外としてArgumentNullExceptionをスローする
                Throw New ArgumentNullException("key")
            End If

            Dim pageInfo As Dictionary(Of String, Object)

            '引数screenPos判定
            Select Case pos
                Case ScreenPos.Current 'screenPosがCurrent(自画面）の場合
                    pageInfo = Me.CurrentPageInfo

                Case ScreenPos.Prev    'screenPosがPrev（前画面）の場合
                    pageInfo = Me.PrevPageInfo

                Case ScreenPos.Last    'screenPosがLast（直近の同一画面）の場合
                    pageInfo = Me.LastPageInfo

                    '直近同一画面引渡しDictionary(Of String, Object)がNothingの場合
                    If pageInfo Is Nothing Then
                        Return Nothing
                    End If

                Case Else   'screenPosがNext(次画面）の場合
                    pageInfo = Me.NextPageInfo

            End Select

            '返り値格納用オブジェクトにセッションに格納されているObjectを格納
            Dim returnObject As Object = pageInfo.Item(key)

            'removeFlgがTrueの場合
            If removeFlg Then
                '引数のkeyをキーとして、ローカル変数pageInfoのremoveメソッドにて
                '対象のObjectを削除する
                pageInfo.Remove(key)

                ''メインメニューSession退避Cookie処理
                'RemoveMainMenuCookie(screenPos, key)
            End If

            Return returnObject
        End Function

        ''' <summary>
        ''' Sessionから検索条件等のデータを削除します。
        ''' </summary>
        ''' <param name="pos">画面位置を表す列挙型。</param>
        ''' <param name="key">Sessionから削除するObjectのキー名。</param>
        ''' <remarks></remarks>
        ''' <exception cref="ArgumentNullException">
        ''' 引数「key」にNothingを指定した場合にスローされます。
        ''' </exception>
        ''' <exception cref=" InvalidOperationException">
        ''' 遷移元が存在しない場合にscreenPosに「Prev」「Last」を指定した場合にスローされます。
        ''' </exception>
        Protected Sub RemoveValue( _
            ByVal pos As ScreenPos, _
            ByVal key As String)

            '引数keyがNothingの場合
            If key Is Nothing Then
                '例外としてArgumentNullExceptionをスローする
                Throw New ArgumentNullException("key")
            End If

            Dim pageInfo As Dictionary(Of String, Object)

            '引数screenPos判定
            Select Case pos
                Case ScreenPos.Current 'screenPosがCurrent(自画面）の場合
                    pageInfo = Me.CurrentPageInfo

                Case ScreenPos.Prev    'screenPosがPrev（前画面）の場合
                    pageInfo = Me.PrevPageInfo

                Case ScreenPos.Last    'screenPosがLast（直近の同一画面）の場合
                    pageInfo = Me.LastPageInfo

                    '直近同一画面引渡しDictionary(Of String, Object)がNothingの場合
                    If pageInfo Is Nothing Then
                        Throw New InvalidOperationException
                    End If

                Case Else   'screenPosがNext(次画面）の場合
                    pageInfo = Me.NextPageInfo

            End Select

            '引数のkeyをキーとして、ローカル変数pageInfoのremoveメソッドにて対象のObjectを削除する
            pageInfo.Remove(key)

            ''メインメニューSession退避Cookie処理
            'RemoveMainMenuCookie(screenPos, key)

        End Sub

        ''' <summary>
        ''' 指定したキー名のObjectがSessionに存在するか確認します。
        ''' </summary>
        ''' <param name="pos">存在確認する画面位置を表す列挙型。</param>
        ''' <param name="key">Sessionに存在するか確認するObjectのキー名。</param>
        ''' <returns>True：指定したデータがSessionに存在する。False：存在しない。</returns>
        ''' <remarks></remarks>
        ''' <exception cref="ArgumentNullException">
        ''' 引数「key」にNothingを指定した場合にスローされます。
        ''' </exception>
        ''' <exception cref=" InvalidOperationException">
        ''' 遷移元が存在しない場合にscreenPosに「Prev」「Last」を指定した場合にスローされます。
        ''' </exception>
        Protected Function ContainsKey( _
            ByVal pos As ScreenPos, _
            ByVal key As String) As Boolean

            '引数keyがNothingの場合
            If key Is Nothing Then
                '例外としてArgumentNullExceptionをスローする
                Throw New ArgumentNullException("key")
            End If

            Dim pageInfo As Dictionary(Of String, Object)

            '引数screenPos判定
            Select Case pos
                Case ScreenPos.Current 'screenPosがCurrent(自画面）の場合
                    pageInfo = Me.CurrentPageInfo

                Case ScreenPos.Prev    'screenPosがPrev（前画面）の場合
                    pageInfo = Me.PrevPageInfo

                Case ScreenPos.Last    'screenPosがLast（直近の同一画面）の場合
                    pageInfo = Me.LastPageInfo

                    '直近同一画面引渡しDictionary(Of String, Object)がNothingの場合
                    If pageInfo Is Nothing Then
                        Return False
                    End If

                Case Else    'screenPosがNext(次画面）の場合
                    pageInfo = Me.NextPageInfo

            End Select

            'ローカル変数pageInfoのContainsKeyメソッドを引数keyを指定して実行し、
            '結果を戻り値として戻す。
            Return pageInfo.ContainsKey(key)

        End Function

        ''' <summary>
        ''' 画面遷移履歴内にて、カレント画面から前位置の画面のIDを取得します。
        ''' </summary>
        ''' <returns>前位置の画面ID</returns>
        ''' <remarks></remarks>
        ''' <exception cref=" InvalidOperationException">
        ''' 遷移元の画面が存在しない場合にスローされます。
        ''' </exception>
        Protected ReadOnly Property GetPrevScreenId() As String
            Get
                Dim nodeList As List(Of SerializableSiteMapNode)

                'HistorySiteMapProviderのSiteMapNodeListプロパティにてSessionから
                '画面遷移履歴Listを取得
                nodeList = HistorySiteMapProvider.SiteMapNodeList

                '画面遷移履歴Listのサイズが１以下の場合
                If nodeList.Count <= 1 Then


                    '画面遷移履歴Listのサイズが１かつ、メインメニューSessionが存在する場合
                    If nodeList.Count = 1 Then
                        'メインメニューのアプリＩＤを返す
                        Return CStr(Session(SESSION_TOPPAGE))
                    Else
                        'Nothingを返す
                        Return Nothing
                    End If
                    'End If

                End If

                Dim node As SerializableSiteMapNode

                '画面遷移履歴Listの最後尾から１つ前の位置の履歴を取得
                node = nodeList(nodeList.Count - 2)

                '取得した履歴のURLプロパティを取得
                Dim prevUrl As String = node.Url

                '＜クエリ文字列が含まれる場合＞
                If 0 < prevUrl.IndexOf("?", StringComparison.OrdinalIgnoreCase) Then
                    'クエリ文字列を削除
                    prevUrl = prevUrl.Remove(prevUrl.IndexOf("?", StringComparison.OrdinalIgnoreCase))
                End If

                '画面IDの取得
                Dim screenId As String = prevUrl.Substring(prevUrl.LastIndexOf("/", StringComparison.OrdinalIgnoreCase) + 1)
                screenId = screenId.Remove(screenId.LastIndexOf(".", StringComparison.OrdinalIgnoreCase))

                Return screenId
            End Get
        End Property



        ''' <summary>
        ''' 履歴最大保持数および最大サイズを超えた場合に、サイトマップをリサイズします。
        ''' </summary>
        ''' <remarks></remarks>
        Private Shared Sub resizeSiteMapNoneList()

            Dim siteMapNodeList As List(Of SerializableSiteMapNode) = HistorySiteMapProvider.SiteMapNodeList

            '履歴表示上限件数を取得
            Dim maxHistoryCount As Integer = EnvironmentSetting.MaxHistoryCount

            '画面遷移履歴Listの件数と上限値を比較
            If maxHistoryCount < siteMapNodeList.Count Then

                '超過分を削除
                siteMapNodeList.RemoveRange(0, siteMapNodeList.Count - maxHistoryCount)
            End If

            '履歴Sessionサイズ上限を取得
            Dim maxHistorySize As Integer = EnvironmentSetting.MaxHistorySize * 1024

            '画面遷移履歴Listのシリアライズしたサイズを取得
            Dim nodeListSize As Long = CalculateSize(siteMapNodeList)

            '画面遷移履歴Listのシリアライズしたサイズと上限値を比較
            If maxHistorySize < nodeListSize Then

                'Nodeサイズ合計用のローカル変数nodeSizeを宣言する
                Dim nodeSize As Long = 0

                '削除件数用のローカル変数delCountを宣言する
                Dim delCount As Integer

                '画面遷移履歴Listの先頭から最後尾の要素までループ処理を開始
                For Each serializableSiteMapNode As SerializableSiteMapNode In siteMapNodeList

                    '画面遷移履歴のシリアライズしたサイズを加算
                    nodeSize += CalculateSize(serializableSiteMapNode)

                    '削除件数delCountをインクリメント
                    delCount += 1

                    If (nodeListSize - nodeSize) < maxHistorySize Then
                        'ループを抜ける
                        Exit For
                    End If

                Next

                '画面遷移履歴Listの先頭から最後尾の要素までループ処理を終了
                '画面遷移履歴Listの先頭から、delCountの件数分の履歴を削除
                siteMapNodeList.RemoveRange(0, delCount)

            End If

        End Sub



        ''' <summary>
        ''' Sessionにて次画面に引渡す情報を格納したDictionary(Of String, Object)を取得します。
        ''' </summary>
        ''' <returns>
        ''' Sessionにて次画面に引渡す情報を格納したDictionary(Of String, Object)
        ''' </returns>
        Friend ReadOnly Property NextPageInfo() As Dictionary(Of String, Object)
            Get
                If Me._nextPageInfo Is Nothing Then
                    'インスタンス変数_nextPageInfoがNothingの場合、以下の処理を実行

                    '次画面引渡しDictionary(Of String, Object)
                    Dim nextPage As Dictionary(Of String, Object) = Nothing

                    'Sessionより次画面引渡しDictionary(Of String, Object)を取得
                    nextPage = DirectCast( _
                        Current.Session(HistorySiteMapProvider.SESSION_KEY_NEXT_PAGE_INFO),  _
                        Dictionary(Of String, Object))

                    If nextPage Is Nothing Then

                        '次画面引渡しDictionary(Of String, Object)を生成
                        nextPage = New Dictionary(Of String, Object)

                        '生成したDictionary(Of String, Object)をHistorySiteMapProvider.SESSION_KEY_NEXT_PAGE_INFOを
                        'キーとしてSessionに格納
                        Current.Session(HistorySiteMapProvider.SESSION_KEY_NEXT_PAGE_INFO) = nextPage
                    End If

                    'Dictionary(Of String, Object)をインスタンス変数_nextPageInfoに設定
                    Me._nextPageInfo = nextPage
                End If

                '戻り値として、インスタンス変数_nextPageInfoを戻す。
                Return Me._nextPageInfo
            End Get
        End Property

        ''' <summary>
        ''' Sessionにて前画面に引渡す情報を格納したDictionary(Of String, Object)を取得します。
        ''' </summary>
        ''' <returns>
        ''' Sessionにて前画面に引渡す情報を格納したDictionary(Of String, Object)
        ''' </returns>
        ''' <exception cref="InvalidOperationException">
        ''' 遷移元の画面が存在しない場合にスローされます。
        ''' </exception>
        Private ReadOnly Property PrevPageInfo() As Dictionary(Of String, Object)
            Get
                If Me._prevPageInfo Is Nothing Then
                    'インスタンス変数_prevPageInfoがNothingの場合、以下の処理を実行

                    Dim nodeList As List(Of SerializableSiteMapNode)

                    '前画面引渡しHasTable
                    Dim prevPage As Dictionary(Of String, Object) = Nothing

                    'HistorySiteMapProviderのSiteMapNodeListプロパティにて
                    'Sessionから画面遷移履歴Listを取得
                    nodeList = HistorySiteMapProvider.SiteMapNodeList

                    '画面遷移履歴Listのサイズが１以下の場合
                    If nodeList.Count <= 1 Then
                        Throw New InvalidOperationException
                    End If

                    Dim node As SerializableSiteMapNode

                    '画面遷移履歴Listの最後尾から１つ前の位置の履歴を取得
                    node = nodeList(nodeList.Count - 2)

                    '履歴のPageSessionInfoプロパティにて前画面引渡しDictionary(Of String, Object)を取得
                    prevPage = node.PageSessionInfo

                    'Dictionary(Of String, Object)をインスタンス変数_prevPageInfoに設定
                    Me._prevPageInfo = prevPage
                End If

                '戻り値として、インスタンス変数_prevPageInfoを戻す。
                Return Me._prevPageInfo
            End Get
        End Property

        ''' <summary>
        ''' Sessionにて直近の同一画面に引渡す情報を格納したDictionary(Of String, Object)を取得します。
        ''' 画面遷移履歴に該当する履歴が存在しない場合、および前位置に画面遷移履歴がない場合は
        ''' 例外をスローせず、Nothingを戻します。
        ''' </summary>
        ''' <returns>
        ''' Sessionにて直近の同一画面に引渡す情報を格納したDictionary(Of String, Object)
        ''' </returns>
        Private ReadOnly Property LastPageInfo() As Dictionary(Of String, Object)
            Get
                If Me._lastPageInfo Is Nothing Then
                    'インスタンス変数_lastPageInfoがNothingの場合、以下の処理を実行
                    Dim nodeList As List(Of SerializableSiteMapNode)

                    'HistorySiteMapProviderのSiteMapNodeListプロパティにてSessionから
                    '画面遷移履歴Listを取得
                    nodeList = HistorySiteMapProvider.SiteMapNodeList

                    '画面遷移履歴Listのサイズが１以下の場合
                    If nodeList.Count <= 1 Then
                        Me._lastPageInfo = Nothing
                    Else
                        '引渡しHasTable
                        Dim lastPage As Dictionary(Of String, Object) = Nothing

                        For i As Integer = nodeList.Count - 2 To 0 Step -1
                            If nodeList(i).Url.Equals(Current.Request.RawUrl) Then
                                lastPage = nodeList(i).PageSessionInfo
                                Exit For
                            End If
                        Next i

                        'Dictionary(Of String, Object)をインスタンス変数_lastPageInfoに設定
                        Me._lastPageInfo = lastPage
                    End If

                End If

                '戻り値として、インスタンス変数_lastPageInfoを戻す。
                Return Me._lastPageInfo
            End Get
        End Property

        ''' <summary>
        ''' 自画面のSession情報を格納したDictionary(Of String, Object)を取得します。
        ''' </summary>
        ''' <returns>
        ''' 自画面のSession情報を格納したDictionary(Of String, Object)
        ''' </returns>
        Private ReadOnly Property CurrentPageInfo() As Dictionary(Of String, Object)
            Get
                If Me._currentPageInfo Is Nothing Then
                    'インスタンス変数_currentPageInfoがNothingの場合、以下の処理を実行

                    '次画面引渡しHasTable
                    Dim currentPage As Dictionary(Of String, Object) = Nothing

                    Dim nodeList As List(Of SerializableSiteMapNode)

                    'HistorySiteMapProviderのSiteMapNodeListプロパティにてSessionから
                    '画面遷移履歴Listを取得
                    nodeList = HistorySiteMapProvider.SiteMapNodeList

                    '画面遷移履歴Listが０件の場合は遷移履歴を作成
                    'メインメニューの場合は独自のKeyで管理
                    If (nodeList.Count = 0) Then
                        If Session(HistorySiteMapProvider.SESSION_KEY_MAINMENU_CONTEXT) Is Nothing Then
                            currentPage = New Dictionary(Of String, Object)
                            Session(HistorySiteMapProvider.SESSION_KEY_MAINMENU_CONTEXT) = currentPage
                        Else
                            currentPage = CType(Session(HistorySiteMapProvider.SESSION_KEY_MAINMENU_CONTEXT), Dictionary(Of String, Object))
                        End If
                    Else
                        '画面遷移履歴Listの最後尾の位置から履歴を取得
                        Dim node As SerializableSiteMapNode = nodeList(nodeList.Count - 1)

                        '履歴のPageSessionInfoプロパティにて画面引渡しDictionary(Of String, Object)を取得
                        currentPage = node.PageSessionInfo
                    End If
                    ''画面遷移履歴Listが０件の場合は遷移履歴を作成
                    'If (nodeList.Count = 0) Then
                    '    HistorySiteMapProvider.AddNewNode(Context.Request.Url.ToString(), "", New Dictionary(Of String, Object))
                    'End If

                    ''画面遷移履歴Listの最後尾の位置から履歴を取得
                    'Dim node As SerializableSiteMapNode = nodeList(nodeList.Count - 1)

                    ''履歴のPageSessionInfoプロパティにて画面引渡しDictionary(Of String, Object)を取得
                    'currentPage = node.PageSessionInfo

                    'Dictionary(Of String, Object)をインスタンス変数_currentPageInfoに設定
                    Me._currentPageInfo = currentPage
                End If

                '戻り値として、インスタンス変数_currentPageInfoを戻す。
                Return Me._currentPageInfo
            End Get
        End Property

        ''' <summary>
        ''' 指定したObjectのシリアライズしたサイズをバイト単位で返します。
        ''' </summary>
        ''' <param name="obj">サイズ求めるObject</param>
        ''' <remarks>
        ''' </remarks>
        Friend Shared Function CalculateSize(ByVal obj As Object) As Long

            Dim returnSize As Long = 0

            Using stream As MemoryStream = New MemoryStream()
                Dim writer As BinaryWriter = New BinaryWriter(stream)
                Dim formatter As New BinaryFormatter
                formatter.Serialize(writer.BaseStream, obj)

                writer.Flush()
                returnSize = stream.Length
            End Using

            Return returnSize
        End Function
#End Region

#Region " 画面遷移及び画面操作 "

        ''' <summary>
        ''' ドメイン名格納用セッション名
        ''' </summary>
        ''' <remarks></remarks>
        Private Const SESSION_DOMAIN As String = "Toyota.eCRB.SystemFrameworks.Web."

        ''' <summary>
        ''' バリデーション結果を通知するためのポップアップダイアログを表示したい時に使用します
        ''' </summary>
        ''' <param name="wordNo">表示メッセージ（文言No）</param>
        ''' <param name="wordParam">表示メッセージ（置換文字列）</param>
        ''' <remarks></remarks>
        Protected Sub ShowMessageBox(ByVal wordNo As Integer, ByVal ParamArray wordParam As String())
            Dim word As String = WebWordUtility.GetWord(wordNo)
            If wordParam IsNot Nothing AndAlso wordParam.Length > 0 Then
                word = String.Format(CultureInfo.InvariantCulture, word, wordParam)
            End If
            JavaScriptUtility.RegisterAlertMessege(Me, "", "", word)
        End Sub

        ''' <summary>
        ''' バリデーション結果を通知するためのポップアップダイアログを表示したい時に使用します
        ''' </summary>
        ''' <param name="code">エラーコード</param>
        ''' <param name="detail">障害解析用文字列</param>
        ''' <param name="wordNo">表示メッセージ（文言No）</param>
        ''' <param name="wordParam">表示メッセージ（置換文字列）</param>
        ''' <remarks></remarks>
        Protected Sub ShowMessageBox(ByVal code As String, ByVal detail As String, ByVal wordNo As Integer, ByVal ParamArray wordParam As String())
            Dim word As String = WebWordUtility.GetWord(wordNo)
            If wordParam IsNot Nothing AndAlso wordParam.Length > 0 Then
                word = String.Format(CultureInfo.InvariantCulture, word, wordParam)
            End If
            JavaScriptUtility.RegisterAlertMessege(Me, code, detail, word)
        End Sub

        ''' <summary>
        ''' 次画面に遷移します。
        ''' </summary>
        ''' <param name="appId">画面ID</param>
        ''' <remarks></remarks>
        Public Sub RedirectNextScreen(ByVal appId As String)

            '画面遷移履歴Listを取得
            'Dim siteMapNodeList As List(Of SerializableSiteMapNode) = HistorySiteMapProvider.SiteMapNodeList
            Dim aspxFileName As String = appId & ".aspx"

            '次画面引渡しDictionary(Of String, Object)を取得
            Dim nextPageInfo As Dictionary(Of String, Object) = Me.NextPageInfo

            '次画面引渡しDictionary(Of String, Object)が存在する場合
            If Session(HistorySiteMapProvider.SESSION_KEY_NEXT_PAGE_INFO) IsNot Nothing Then
                'Sessionより次画面引渡しDictionary(Of String, Object)を削除
                Session.Remove(HistorySiteMapProvider.SESSION_KEY_NEXT_PAGE_INFO)
            End If

            '画面遷移履歴を追加
            Dim canonicalUrl As String = Me.ResolveUrl("~/Pages/" & aspxFileName)
            'Dim hisMapProvider As New HistorySiteMapProvider
            HistorySiteMapProvider.AddNewNode(canonicalUrl, "", nextPageInfo)

            'サイトマップのリサイズ
            resizeSiteMapNoneList()

            '遷移先画面のドメイン名取得
            Dim config As ClassSection = SystemConfiguration.Current.Manager.DocumentDomain
            If config IsNot Nothing Then
                Dim domain As String = Nothing
                If config IsNot Nothing Then
                    Dim setting As Setting = config.GetSetting(String.Empty)
                    If (setting IsNot Nothing) Then
                        domain = DirectCast(setting.GetValue(appId), String)
                        Session(SESSION_DOMAIN) = domain
                    End If
                End If
            End If

            '画面遷移
            Logger.Debug(String.Format(CultureInfo.InvariantCulture, "BasePage.RedirectNextScreen: {0}", canonicalUrl))
            Me.Response.Redirect(canonicalUrl)
        End Sub

        ''' <summary>
        ''' 不要なSession（自画面、次画面）を削除し、画面遷移履歴から指定画面数前の画面にリダイレクトします。
        ''' </summary>
        ''' <param name="prev">戻る画面数</param>
        ''' <remarks></remarks>
        ''' <exception cref=" InvalidOperationException">
        ''' 遷移元の画面が存在しない場合、引数がマイナスの場合にスローされます。
        ''' </exception>
        Public Sub RedirectPrevScreen(ByVal prev As Integer)

            If prev < 1 Then
                '例外としてInvalidOperationExceptionをスローする
                Throw New InvalidOperationException
            End If

            Dim nodeList As List(Of SerializableSiteMapNode)

            'HistorySiteMapProviderのSiteMapNodeListプロパティにてSessionから画面遷移履歴Listを取得
            nodeList = HistorySiteMapProvider.SiteMapNodeList

            Dim prevUrl As String = Nothing
            If nodeList.Count <= 1 Then
                ''履歴が1つの時戻れるのはトップページのみ
                prevUrl = ResolveUrl("~/Pages/" & CStr(Session(SESSION_TOPPAGE)) & ".aspx")
            Else
                Dim node As SerializableSiteMapNode
                '画面遷移履歴Listの最後尾から１つ前の位置の履歴を取得
                node = nodeList(nodeList.Count - (1 + prev))

                '取得した履歴のURLプロパティを取得
                prevUrl = node.Url
            End If

            '画面遷移履歴Listの最後尾の履歴を削除
            If (0 < nodeList.Count) Then
                nodeList.RemoveRange(nodeList.Count - prev, prev)
            End If

            '定数SESSION_KEY_NEXT_PAGE_INFOをキーとして、Sessionより次画面引渡しDictionary(Of String, Object)を削除
            Current.Session.Remove(HistorySiteMapProvider.SESSION_KEY_NEXT_PAGE_INFO)

            '遷移先画面のドメイン名取得
            Dim AppIdAry As String()
            AppIdAry = Split(prevUrl, "/")
            Dim AppId As String
            AppId = AppIdAry(AppIdAry.Length - 1)
            AppId = Replace(AppId, ".aspx", "")
            Dim config As Configuration.ClassSection = SystemConfiguration.Current.Manager.DocumentDomain
            If config IsNot Nothing Then
                Dim domain As String = Nothing
                If config IsNot Nothing Then
                    Dim setting As Configuration.Setting = config.GetSetting(String.Empty)
                    If (setting IsNot Nothing) Then
                        domain = DirectCast(setting.GetValue(AppId), String)
                        Session(SESSION_DOMAIN) = domain
                    End If
                End If
            End If

            '取得した履歴のURLへリダイレクト
            Logger.Debug(String.Format(CultureInfo.InvariantCulture, "BasePage.RedirectPrevScreen: {0}", prevUrl))
            Response.Redirect(prevUrl)

        End Sub

        ''' <summary>
        ''' 不要なSession（自画面、次画面）を削除し、画面遷移履歴の前位置の画面にリダイレクトします。
        ''' </summary>
        ''' <remarks></remarks>
        ''' <exception cref=" InvalidOperationException">
        ''' 遷移元の画面が存在しない場合にスローされます。
        ''' </exception>
        Public Sub RedirectPrevScreen()

            RedirectPrevScreen(1)

        End Sub
#End Region

#Region "ダイアログ操作"
        ''' <summary>
        ''' 引数「appId」で指定された画面をダイアログ表示します。
        ''' </summary>
        ''' <param name="appId">子ダイアログを表す機能ID</param>
        ''' <param name="effect">ダイアログのエフェクト</param>
        ''' <remarks></remarks>
        Public Sub OpenDialog(ByVal appId As String, ByVal effect As DialogEffect)
            Dim paramEffect As String = "fadeIn"
            Select Case effect
                Case DialogEffect.Left
                    paramEffect = "left"
                Case DialogEffect.Right
                    paramEffect = "right"
                Case DialogEffect.Top
                    paramEffect = "top"
                Case DialogEffect.Bottom
                    paramEffect = "bottom"
            End Select

            Dim sb As New StringBuilder
            sb.Append("<script type='text/javascript'>").Append(vbCrLf)
            sb.Append("    (function(window) {").Append(vbCrLf)
            sb.Append("         icropScript.ui.openDialog('").Append(HttpUtility.JavaScriptStringEncode(appId & ".aspx"))
            sb.Append("', '").Append(paramEffect).Append("', ")
            sb.Append("function() { ").Append(Me.ClientScript.GetPostBackEventReference(Me, "")).Append("; });")
            sb.Append("    })(window);").Append(vbCrLf)
            sb.Append("</script>" & vbCrLf)
            JavaScriptUtility.RegisterStartupScript(Me, sb.ToString, "icropScript.ui.openDialog")

        End Sub

        ''' <summary>
        ''' ダイアログを閉じます。
        ''' このメソッドはダイアログで表示されている子画面でのみ機能します。
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub CloseDialog()
            'Dim sb As New StringBuilder
            JavaScriptUtility.RegisterStartupFunctionCallScript(Me, "icropScript.ui.closeDialog", "icropScript.ui.closeDialog")
        End Sub
#End Region

#Region " 編集状態保存 "

        ''' <summary>
        ''' 画面の入力中データを取得します。
        ''' </summary>
        ''' <returns>自画面が、ISafeInputForm.SaveFormStateメソッドにて退避した入力中データ</returns>
        ''' <remarks></remarks>
        Public Function GetFormState() As Dictionary(Of String, ISerializable)

            Dim state As Dictionary(Of String, Dictionary(Of String, ISerializable)) = GetSessionFormState()
            Dim appId As String = GetCurrentAppID()

            If Not state.ContainsKey(appId) Then
                'なければ追加
                state(appId) = New Dictionary(Of String, ISerializable)
            End If

            Return state(appId)
        End Function

        ''' <summary>
        ''' 自画面の入力中データを保持しているSessionをクリアします。
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub ClearFormState()
            Dim state As Dictionary(Of String, Dictionary(Of String, ISerializable)) = GetSessionFormState()
            Dim appId As String = GetCurrentAppID()
            If state.ContainsKey(appId) Then
                state.Remove(appId)
            End If
        End Sub

        ''' <summary>
        ''' 画面入力情報保持のSessionキー名
        ''' </summary>
        Private Const SESSION_KEY_FORM_STATE As String = "Toyota.eCRB.SystemFrameworks.Web.BasePage.FormState"

        ''' <summary>
        ''' セッションより入力情報を取得します。
        ''' </summary>
        ''' <returns>画面ID毎で、入力項目毎のキーとその値のDictionary</returns>
        ''' <remarks></remarks>
        Private Function GetSessionFormState() As Dictionary(Of String, Dictionary(Of String, ISerializable))

            Dim state As Dictionary(Of String, Dictionary(Of String, ISerializable)) = Nothing

            'セッションより画面入力情報取得
            state = DirectCast(Session(SESSION_KEY_FORM_STATE), Dictionary(Of String, Dictionary(Of String, ISerializable)))
            If state Is Nothing Then
                '新規作成
                state = New Dictionary(Of String, Dictionary(Of String, ISerializable))
                'セッション領域に保存
                Current.Session(SESSION_KEY_FORM_STATE) = state
            End If

            '返却
            Return state
        End Function

        ''' <summary>
        ''' 現在リクエストされている画面のIDを取得します。
        ''' </summary>
        ''' <returns>画面ID</returns>
        ''' <remarks></remarks>
        Private Shared Function GetCurrentAppID() As String
            Dim path As String = Current.Request.AppRelativeCurrentExecutionFilePath
            path = path.Substring(path.LastIndexOf("/", StringComparison.OrdinalIgnoreCase) + 1)
            path = path.Remove(path.LastIndexOf(".", StringComparison.OrdinalIgnoreCase))
            Return path.ToUpper(CultureInfo.InvariantCulture)
        End Function

#End Region

#Region "検索処理"
        ''' <summary>
        ''' 検索処理
        ''' </summary>
        ''' <param name="searchString"></param>
        ''' <param name="searchType"></param>
        ''' <remarks></remarks>
        Friend Sub CustomerSearch_Click(ByVal searchString As String, ByVal searchType As Integer)
            '入力された検索値を保持
            SetValue(ScreenPos.Next, "searchString", searchString)
            SetValue(ScreenPos.Next, "searchType", searchType)

            'ログインユーザがセールスかサービスか判断
            Dim type As String = String.Empty
            Try
                Dim staff As StaffContext = StaffContext.Current
                Dim config As Configuration.ClassSection = SystemConfiguration.Current.Manager.StaffDivision
                If config IsNot Nothing Then
                    Dim setting As Configuration.Setting = config.GetSetting(String.Empty)
                    If (setting IsNot Nothing) Then
                        type = DirectCast(setting.GetValue(CStr(staff.OpeCD)), String)
                    End If
                End If
            Catch ex As InvalidOperationException
                '未ログイン
            End Try

            '対象画面へ遷移
            If String.IsNullOrEmpty(type) Then type = String.Empty
            If (type.Equals("Service")) Then
                RedirectNextScreen("SC3080102")
            Else
                RedirectNextScreen("SC3080101")
            End If

        End Sub
        ''' <summary>
        ''' 検索処理
        ''' </summary>
        ''' <param name="searchString"></param>
        ''' <param name="searchType"></param>
        ''' <remarks></remarks>
        Friend Sub CustomerSearch_Click(ByVal searchString As String, ByVal searchType As Integer, ByVal chipType As Integer)
            '2013/06/03 TMEJ 小澤 【A.STEP2】i-CROP業務機能要件定義（既存流用機能） START
            'Friend Sub CustomerSearch_Click(ByVal searchString As String, ByVal searchType As Integer, ByVal chipType As Integer)
            '2013/06/03 TMEJ 小澤 【A.STEP2】i-CROP業務機能要件定義（既存流用機能） END
            '入力された検索値を保持
            SetValue(ScreenPos.Next, "searchString", searchString)
            SetValue(ScreenPos.Next, "searchType", searchType)

            'ログインユーザがセールスかサービスか判断
            Dim type As String = String.Empty
            Try
                Dim staff As StaffContext = StaffContext.Current
                Dim config As Configuration.ClassSection = SystemConfiguration.Current.Manager.StaffDivision
                If config IsNot Nothing Then
                    Dim setting As Configuration.Setting = config.GetSetting(String.Empty)
                    If (setting IsNot Nothing) Then
                        type = DirectCast(setting.GetValue(CStr(staff.OpeCD)), String)
                    End If
                End If
            Catch ex As InvalidOperationException
                '未ログイン
            End Try

            '対象画面へ遷移
            If String.IsNullOrEmpty(type) Then type = String.Empty
            If (type.Equals("Service")) Then
                '2013/06/03 TMEJ 小澤 【A.STEP2】i-CROP業務機能要件定義（既存流用機能） START
                'RedirectNextScreen("SC3080102")
                If chipType = 1 Then
                    RedirectNextScreen("SC3080103")
                Else
                    RedirectNextScreen("SC3240401")
                End If
                '2013/06/03 TMEJ 小澤 【A.STEP2】i-CROP業務機能要件定義（既存流用機能） END
            Else
                RedirectNextScreen("SC3080101")
            End If

        End Sub
#End Region

        '2013/12/16 TMEJ 小澤 次世代サービス 工程管理機能開発 START
#Region "画面遷移処理"

        ''' <summary>
        ''' 画面遷移処理
        ''' </summary>
        ''' <param name="inProgramId">遷移先画面ID</param>
        ''' <param name="inSessionKey">Sessionキー</param>
        ''' <param name="inSessionData">Sessionデータ</param>
        ''' <remarks></remarks>
        Friend Sub RedirectNextScreenButton_Click(ByVal inProgramId As String, _
                                                  ByVal inSessionKey As String, _
                                                  ByVal inSessionData As String)
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} START" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))

            '画面間引数の確認
            If (Not (String.IsNullOrEmpty(inSessionKey)) AndAlso 0 < inSessionKey.Length) Then

                '引数が存在する場合
                'データをカンマ区切りで配列にする
                Dim sessionKeyList As String() = inSessionKey.Split(CType(",", Char))
                Dim sessionDataList As String() = inSessionData.Split(CType(",", Char))

                '画面遷移先をチェック
                If inProgramId.Equals("SC3010501") Then
                    '他システム連携画面の場合
                    '表示番号をSessionにデータ格納
                    Me.SetValue(ScreenPos.Next, "Session.DISP_NUM", sessionDataList(0))

                    '他システム画面表示に必要なデータを格納
                    For i As Integer = 1 To sessionKeyList.Count - 1
                        Me.SetValue(ScreenPos.Next, String.Concat("Session.Param", i), sessionDataList(i))
                    Next

                Else
                    'i-CROP画面の場合
                    'Sessionにデータ格納
                    For i As Integer = 0 To sessionKeyList.Count - 1
                        Me.SetValue(ScreenPos.Next, sessionKeyList(i), sessionDataList(i))
                    Next
                End If

            End If

            '画面遷移処理
            RedirectNextScreen(inProgramId)

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))
        End Sub
#End Region
        '2013/12/16 TMEJ 小澤 次世代サービス 工程管理機能開発 END

#Region "TCV戻り処理"
        Friend Sub TCVCallBack(ByVal params As Dictionary(Of String, Object))

            For Each param In params
                SetValue(ScreenPos.Next, param.Key, param.Value)
            Next param

            RedirectNextScreen(CStr(params("StartPageId")))

        End Sub
#End Region

    End Class
End Namespace
