Imports System.Text
Imports System.ComponentModel
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.Script.Serialization
Imports System.Web.Configuration
Imports System.Runtime.Serialization
Imports Toyota.eCRB.SystemFrameworks.Web.Controls.Design
Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Globalization

<Assembly: WebResource("CustomGridView.js", "application/x-javascript")> 
Namespace Toyota.eCRB.SystemFrameworks.Web.Controls
    ''' <summary>
    ''' 以下のテーブル表示表示機能を提供するコントロールです。
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CustomGridView
        Inherits WebControl
        Implements ICallbackEventHandler, IPostBackEventHandler

#Region " イベント宣言 "

        ''' <summary>
        ''' <see cref="CustomGridView.Search"/>メソッドが呼ばれた場合又は、 クライアントサイド側でページング処理を行った場合に発生するイベントです。
        ''' </summary>
        Public Event Paging As EventHandler(Of CustomGridViewPagingEventArgs)

        ''' <summary>
        ''' 行を選択した際に発生するイベントです。<br/>
        ''' </summary>
        Public Event SelectedRow As EventHandler(Of CustomGridViewSelectedRowEventArgs)
#End Region

#Region " 定数 "
        ''' <summary>コールバックパラメータ名-コントロール名</summary>
        Private Const C_CALLBACK_ARGS_CTRLNAME As String = "CTRLNAME"
        ''' <summary>コールバックパラメータ名-表示対象ページ</summary>
        Private Const C_CALLBACK_ARGS_PAGEINDEX As String = "PAGEINDEX"
        ''' <summary>コールバックパラメータ名-アクション名</summary>
        Private Const C_CALLBACK_ARGS_ACTION As String = "ACTION"
        ''' <summary>コールバックパラメータ名-ソート列インデックス</summary>
        Private Const C_CALLBACK_ARGS_SORTCOL As String = "SORTCOLUMN"
        ''' <summary>コールバックパラメータ名-ソート列方向</summary>
        Private Const C_CALLBACK_ARGS_SORTDVS As String = "SORTDVS"

        ''' <summary>前ページに戻る際のアクション名</summary>
        Private Const C_ACTIONTYPE_NEXTPAGE As String = "NEXT"
        ''' <summary>次ページに戻る際のアクション名</summary>
        Private Const C_ACTIONTYPE_PREVPAGE As String = "PREV"

        ''' <summary>ポストバック関数に渡す引数</summary>
        Private Const C_POSTBACK_ARGS As String = "SELECT-ROW"

#End Region

#Region " プロパティ "

        ''' <summary>
        ''' 行が選択された時に実行されるクライアント側スクリプトを取得または設定します。
        ''' </summary>
        ''' <returns>行が選択された時に実行されるクライアント側スクリプト。</returns>
        ''' <remarks></remarks>
        Public Property OnClientSelectedRow As String
            Get
                If ViewState("OnClientSelectedRow") Is Nothing Then
                    Return String.Empty
                Else
                    Return DirectCast(ViewState("OnClientSelectedRow"), String)
                End If
            End Get
            Set(value As String)
                ViewState("OnClientSelectedRow") = value
            End Set
        End Property

        ''' <summary>
        ''' クライアント側でデータキャッシュとして保存するページ数を取得又は設定。<br/>
        ''' このプロパティに2以上を指定すると、先行フェッチが行われます。
        ''' </summary>
        ''' <returns>キャッシュするレコード数</returns>
        ''' <remarks></remarks>
        <DefaultValue(5), Bindable(False)> _
        Public Property CachePageCount As Integer
            Get
                If ViewState("CachePageCount") Is Nothing Then
                    Return 5
                Else
                    Return DirectCast(ViewState("CachePageCount"), Integer)
                End If
            End Get
            Set(value As Integer)
                ViewState("CachePageCount") = value
            End Set
        End Property

        ''' <summary>
        ''' １回のページング処理で表示する最大レコード数を取得又は設定します。
        ''' </summary>
        ''' <returns>レコード数</returns>
        ''' <remarks></remarks>
        <DefaultValue(30), Bindable(False)> _
        Public Property PageSize As Integer
            Get
                If ViewState("PageSize") Is Nothing Then
                    Return 30
                Else
                    Return DirectCast(ViewState("PageSize"), Integer)
                End If
            End Get
            Set(value As Integer)
                ViewState("PageSize") = value
            End Set
        End Property

        ''' <summary>
        ''' <see cref="CustomGridView.Paging"/>イベントが発生した際に<see cref="CustomGridViewPagingEventArgs.Data"/>プロパティに設定する
        ''' データソースのキーとなるフィールド名又はプロパティ名を１つ又は複数指定します。<br/>
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Bindable(False), PersistenceMode(PersistenceMode.Attribute), TypeConverter(GetType(CommaSeparateConverter))> _
        Public Property DataKeyFieldNames() As String()
            Get
                If ViewState("DataKeyFieldNames") Is Nothing Then
                    Return New String() {}
                Else
                    Return DirectCast(DirectCast(ViewState("DataKeyFieldNames"), String()).Clone, String())
                End If
            End Get
            Set(value As String())
                ViewState("DataKeyFieldNames") = value
            End Set
        End Property

        ''' <summary>
        ''' テーブルに表示する列定義を取得又は設定します。
        ''' </summary>
        ''' <returns>列定義リスト</returns>
        ''' <remarks></remarks>
        <PersistenceMode(PersistenceMode.InnerProperty), Bindable(False), Description("テーブルに表示する列定義を取得又は設定します。")> _
        Public ReadOnly Property Columns As List(Of BaseCustomGridViewColumn)
            Get
                If ViewState("Columns") Is Nothing Then
                    ViewState("Columns") = New List(Of BaseCustomGridViewColumn)
                End If
                Return DirectCast(ViewState("Columns"), List(Of BaseCustomGridViewColumn))
            End Get
        End Property

#End Region

#Region " HTMLレンダリング処理 "
        ''' <summary>
        ''' コントロールの内容を指定したライターに出力します。
        ''' </summary>
        ''' <param name="writer">HTML コンテンツをクライアントに表示する出力ストリームを表す <see cref="HtmlTextWriter"/>。</param>
        ''' <remarks></remarks>
        Protected Overrides Sub RenderContents(writer As System.Web.UI.HtmlTextWriter)

            '外枠となるDiv
            Dim wrapPanel As Panel = CreateWrapPnael()
            'データを表示するテーブル
            Dim dataTable As Table = CreateTable()

            '---------------------------------------
            ' 行(TR)の作成
            '---------------------------------------
            dataTable.Rows.Add(CreateHeaderRow)                                'ヘッダー行(TR)の作成
            'dataTable.Rows.Add(CreatePagingRow("prePagingRow", "Prev"))     '前ページ行(TR)の作成
            dataTable.Rows.Add(CreateDataTemplate)                              'データテンプレート行(TR)の作成
            dataTable.Rows.Add(CreatePagingRow("nextPagingRow", "Next"))    '次ページ行(TR)の作成
            '---------------------------------------

            Dim hidden As New HiddenField
            hidden.ID = GetHiddenID()
            wrapPanel.Controls.Add(hidden)
            If Not DesignMode Then
                'デザインモード以外
                hidden.Value = Page.Request.Form(hidden.UniqueID) & ""
            End If

            '外枠のパネルにテーブル追加
            wrapPanel.Controls.Add(dataTable)

            '上記処理で作成したコントロールをライタに書き込み
            wrapPanel.RenderControl(writer)

        End Sub

        ''' <summary>
        ''' 隠し項目のIDを取得
        ''' </summary>
        ''' <remarks></remarks>
        Private Function GetHiddenID() As String
            Return Me.ID & "__" & "HIDDEN"
        End Function

        ''' <summary>
        ''' テーブルを作成
        ''' </summary>
        ''' <returns>テーブルオブジェクト</returns>
        ''' <remarks></remarks>
        Private Function CreateTable() As Table

            'データを表示するテーブル
            Dim dataTable As New Table

            'ＩＤを設定
            dataTable.ID = Me.ID
            dataTable.CssClass = Me.CssClass

            '１ページに表示する行数
            dataTable.Attributes("PagingRowCount") = PageSize.ToString(CultureInfo.InvariantCulture)
            'キャッシュするページ数
            dataTable.Attributes("CachePageCount") = CachePageCount.ToString(CultureInfo.InvariantCulture)
            'テーブルの高さ
            If Not Me.Height.Equals(Unit.Empty) Then
                dataTable.Attributes("MaxHeight") = Me.Height.ToString(CultureInfo.InvariantCulture)
            End If
            'クライアントサイドイベント
            dataTable.Attributes("onClientSelectedRow") = OnClientSelectedRow
            'テーブルの幅
            dataTable.Width = Me.Width

            Return dataTable
        End Function

        ''' <summary>
        ''' ヘッダー行の作成
        ''' </summary>
        ''' <returns>ヘッダー行</returns>
        ''' <remarks></remarks>
        Private Function CreateHeaderRow() As TableRow

            Dim tr As New TableRow
            Dim cnt As Integer = 0

            'クライアントサイドで使用する属性
            tr.Attributes("rowType") = "headerRow"

            'ヘッダーのセルを追加する
            For Each column As BaseCustomGridViewColumn In Columns

                Dim cell As New TableHeaderCell
                cell.Width = column.Width
                cell.Attributes("columnIndex") = cnt.ToString(CultureInfo.InvariantCulture)
                cell.Attributes("sortDirection") = CType(CustomGridViewSortDirection.None, Integer).ToString(CultureInfo.InvariantCulture)
                If Me.DesignMode Then
                    cell.Text = "COLUMN[WORDNO=" & column.HeaderTextWordNo & "]"
                Else
                    cell.Text = WebWordUtility.GetWord(column.HeaderTextWordNo)
                End If

                tr.Cells.Add(cell)
                cnt += 1
            Next

            Return tr
        End Function

        ''' <summary>
        ''' データテンプレート行の作成
        ''' </summary>
        ''' <returns>データテンプレート行</returns>
        ''' <remarks></remarks>
        Private Function CreateDataTemplate() As TableRow
            Dim tr As New TableRow
            'クライアントサイドで使用する属性
            tr.Attributes("rowType") = "dataTemplate"
            If Not Me.DesignMode Then
                tr.Style("display") = "none"
            End If

            'ヘッダーのセルを追加する
            For Each column As BaseCustomGridViewColumn In Columns

                Dim cell As New TableCell
                Dim col As CustomGridViewColumn = TryCast(column, CustomGridViewColumn)
                If col IsNot Nothing Then
                    If Not String.IsNullOrEmpty(col.DataFieldName) Then
                        'バインドするフィールド名を設定
                        cell.Attributes("bindField") = col.DataFieldName
                        If Me.DesignMode Then
                            cell.Text = col.DataFieldName
                        End If
                    End If
                Else
                    Dim templateCol As CustomGridViewTemplateColumn = TryCast(column, CustomGridViewTemplateColumn)
                    If templateCol IsNot Nothing Then
                        'テンプレートタイプのセル
                        templateCol.DataCellTemplate.InstantiateIn(cell)
                    End If
                End If

                tr.Cells.Add(cell)
            Next
            'For Each column As BaseCustomGridViewColumn In Columns

            '    Dim cell As New TableCell
            '    If TypeOf column Is CustomGridViewColumn Then
            '        Dim filed As CustomGridViewColumn = CType(column, CustomGridViewColumn)
            '        If Not String.IsNullOrEmpty(CType(column, CustomGridViewColumn).DataFieldName) Then
            '            'バインドするフィールド名を設定
            '            cell.Attributes("bindField") = filed.DataFieldName
            '            If Me.DesignMode Then
            '                cell.Text = filed.DataFieldName
            '            End If
            '        End If
            '    ElseIf TypeOf column Is CustomGridViewTemplateColumn Then
            '        'テンプレートタイプのセル
            '        CType(column, CustomGridViewTemplateColumn).DataCellTemplate.InstantiateIn(cell)
            '    End If

            '    tr.Cells.Add(cell)
            'Next

            Return tr
        End Function

        ''' <summary>
        ''' ページング行の作成
        ''' </summary>
        ''' <param name="rowType"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function CreatePagingRow(ByVal rowType As String, ByVal cellText As String) As TableRow
            Dim tr As New TableRow

            'クライアントサイドで使用する属性
            tr.Attributes("rowType") = rowType
            tr.Style("display") = "none"
            Dim cell As New TableCell
            cell.ColumnSpan = Columns.Count
            cell.Text = cellText
            tr.Cells.Add(cell)
            Return tr
        End Function

        ''' <summary>
        ''' 外枠のパネルを作成する
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function CreateWrapPnael() As Panel
            Dim wrapPanel As New Panel
            wrapPanel.Height = Me.Height
            wrapPanel.Width = Me.Width
            Return wrapPanel
        End Function

        ''' <summary>
        ''' 不要なタグを出さない為のオーバーライド
        ''' </summary>
        Public Overrides Sub RenderBeginTag(writer As System.Web.UI.HtmlTextWriter)
            '処理を行わない
        End Sub

        ''' <summary>
        ''' 不要なタグを出さない為のオーバーライド
        ''' </summary>
        Public Overrides Sub RenderEndTag(writer As System.Web.UI.HtmlTextWriter)
            '処理を行わない
        End Sub
#End Region

#Region " 初期化処理 "
        ''' <summary>
        ''' Init イベントを発生させます。
        ''' </summary>
        ''' <param name="e">イベント データを格納している EventArgs オブジェクト。</param>
        ''' <remarks></remarks>
        Protected Overrides Sub OnInit(e As System.EventArgs)
            MyBase.OnInit(e)

            'コールバック時は処理を行わない
            If Page.IsCallback Then
                Return
            End If

            'スクリプトリソースを登録
            RegisterScriptResource()

            '初期化スクリプト登録
            Me.Page.ClientScript.RegisterStartupScript(Me.GetType(), Me.UniqueID, CreateStartUpScript(), True)

            '隠し項目
            If Not Me.Page.IsPostBack Then


            End If

        End Sub

        ''' <summary>
        ''' コールバック用スクリプトの作成
        ''' </summary>
        ''' <returns>コールバックスクリプト</returns>
        ''' <remarks></remarks>
        Private Function CreateStartUpScript() As String

            Dim sbStartUp As New StringBuilder
            Dim param As New Dictionary(Of String, String)
            Dim callBackFunc As String = "eCRB.customGridView.clientCallBack"
            Dim errCallBack As String = "eCRB.customGridView.errorCallBack"
            Dim argsFunc As String = "eCRB.customGridView.createCallBackArgs('" & Me.ClientID & "')"
            'コールバック用のスクリプト参照を作成
            Dim callBackScript As String = Page.ClientScript.GetCallbackEventReference(Me, argsFunc, callBackFunc, """" & Me.ClientID & """", errCallBack, True)
            Dim postBackScript As String = Page.ClientScript.GetPostBackEventReference(Me, C_POSTBACK_ARGS)

            'Function化
            callBackScript = "function() {" & callBackScript & "; }"
            postBackScript = "function() {" & postBackScript & "; }"

            '初期化時に渡すパラメータ作成
            param("id") = "'" & Me.ClientID & "'"
            param("keyNames") = CreateKeyNamesArray()
            param("serverCallBack") = callBackScript
            param("serverPostBack") = postBackScript
            param("hiddenId") = "'" & GetHiddenID() & "'"

            '初期化スクリプト作成
            sbStartUp.Append("eCRB.customGridView.init(").Append(CreateJsonString(param, True)).Append(");")
            Return sbStartUp.ToString
        End Function

        ''' <summary>
        ''' 「DataKeyFieldNames」のJavaScript配列を作成
        ''' </summary>
        ''' <returns>JavaScript配列</returns>
        ''' <remarks></remarks>
        Private Function CreateKeyNamesArray() As String

            Dim keyNames As String

            keyNames = "["
            For i = 0 To DataKeyFieldNames.Length - 1
                If i > 0 Then keyNames &= ","
                keyNames &= """" & DataKeyFieldNames(i) & """"
            Next
            keyNames &= "]"

            Return keyNames
        End Function

        ''' <summary>
        ''' 指定した System.Web.UI.HtmlTextWriter に表示する必要のある HTML 属性およびスタイルを追加します。
        ''' </summary>
        ''' <param name="writer">クライアントに HTML のコンテンツを表示する出力ストリーム。</param>
        ''' <remarks></remarks>
        Protected Overrides Sub AddAttributesToRender(writer As System.Web.UI.HtmlTextWriter)



            MyBase.AddAttributesToRender(writer)
        End Sub
#End Region

#Region " コールバック結果格納データクラス "
        ''' <summary>
        ''' コールバック結果を保持するデータクラス。<br/>
        ''' このクラスの内容をJSON形式に変換し、クライアントに返却します。
        ''' </summary>
        ''' <remarks></remarks>
        Private Class CallBackResultData
            ''' <summary>
            ''' エラーメッセージ
            ''' </summary>
            <ScriptIgnore()> _
            Public Property CallBackErrorMessege As String = String.Empty

            ''' <summary>
            ''' エラーフラグ
            ''' </summary>
            <ScriptIgnore()> _
            Public Property CallBackError As Boolean = False

            ''' <summary>
            ''' 総レコード数
            ''' </summary>
            Public Property TotalCount As Integer

            ''' <summary>
            ''' 結果データ
            ''' </summary>
            Public Property Data As Object = Nothing

            ''' <summary>
            ''' データ取得開始ページインデックス
            ''' </summary>
            Public Property PageIndexFrom As Integer

            ''' <summary>
            ''' データ取得終了ページインデックス
            ''' </summary>
            Public Property PageIndexTo As Integer

            ''' <summary>
            ''' 要求ページインデックス
            ''' </summary>
            Public Property RequestPageIndex As Integer

        End Class
#End Region

#Region " コールバック要求の処理 "
        Private _callBackResult As CallBackResultData

        ''' <summary>
        ''' コントロールを対象とするコールバック イベントの結果を返します。
        ''' </summary>
        ''' <returns>コールバックの結果。(検索結果となるデータソース)</returns>
        ''' <remarks></remarks>
        Public Function GetCallbackResult() As String Implements System.Web.UI.ICallbackEventHandler.GetCallbackResult

            Dim serializer As New JavaScriptSerializer

            If _callBackResult IsNot Nothing AndAlso Not _callBackResult.CallBackError Then
                'コールバック処理にてエラーなし(正常終了)
                If _callBackResult.Data Is Nothing Then
                    'データなし
                    _callBackResult.Data = New Object() {}
                End If

                'カスタムJSON変換クラスを追加する
                Dim customConvert As New List(Of JavaScriptConverter)
                customConvert.Add(New DataTableJsonConvert)
                serializer.RegisterConverters(customConvert)

                'JSON形式に変換した文字列を戻り値として返却
                Return serializer.Serialize(_callBackResult)
            Else
                'コールバック処理にてエラーが発生している場合
                Return CreateAsyncSearchTableErrorJSONData(_callBackResult.CallBackErrorMessege, GetErrorPageUrl)
            End If


        End Function

        ''' <summary>
        ''' 検索処理を実施します。
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Search()
            '検索用のJavaScriptを呼ぶ
            JavaScriptUtility.RegisterStartupFunctionCallScript(Me.Page, "eCRB.customGridView.search", "eCRB.customGridView.search", Me.ClientID)

        End Sub

        ''' <summary>
        ''' コントロールを対象とするコールバック イベントを処理します。
        ''' </summary>
        ''' <param name="eventArgument">イベント ハンドラーに渡されるイベント引数を表す文字列。</param>
        ''' <remarks></remarks>
        Public Sub RaiseCallbackEvent(eventArgument As String) Implements System.Web.UI.ICallbackEventHandler.RaiseCallbackEvent

            _callBackResult = New CallBackResultData

            Try

                Dim serializer As New JavaScriptSerializer
                Dim args As Dictionary(Of String, Object)

                'JSON形式の文字列を変換
                args = serializer.Deserialize(Of Dictionary(Of String, Object))(eventArgument)

                'パラメータチェック
                If args Is Nothing Then
                    Throw New ArgumentException("parameter error!", "eventArgument")
                End If

                '現在クライアントサイドで表示されているページインデックスを取得
                Dim pageIndex As Integer
                If Not Integer.TryParse(CType(args(C_CALLBACK_ARGS_PAGEINDEX), String), pageIndex) Then
                    'エラー
                    Throw New ArgumentException("parameter error!", "eventArgument: " & C_CALLBACK_ARGS_PAGEINDEX)
                End If

                'ソート列インデックス
                Dim sortColIndex As Integer
                If Not Integer.TryParse(CType(args(C_CALLBACK_ARGS_SORTCOL), String), sortColIndex) Then
                    'エラー
                    Throw New ArgumentException("parameter error!", "eventArgument: " & C_CALLBACK_ARGS_SORTCOL)
                End If

                'ソート列インデックス
                Dim sortDvs As CustomGridViewSortDirection
                If Not [Enum].TryParse(Of CustomGridViewSortDirection)(CType(args(C_CALLBACK_ARGS_SORTDVS), String), sortDvs) Then
                    Throw New ArgumentException("parameter error!", "eventArgument: " & C_CALLBACK_ARGS_SORTDVS)
                End If

                'コールバックアクションを格納
                Dim strAction As String = CType(args(C_CALLBACK_ARGS_ACTION), String)

                'データ取得範囲となるページインデックスを設定
                Dim pageIndexFrom As Integer
                Dim pageIndexTo As Integer

                If strAction.Equals(C_ACTIONTYPE_NEXTPAGE) Then
                    '次ページ
                    pageIndexFrom = pageIndex + 1
                    pageIndexTo = pageIndexFrom + (CachePageCount - 1)
                    '要求ページインデックス
                    _callBackResult.RequestPageIndex = pageIndexFrom
                Else
                    'パラメータエラー
                    Throw New System.ArgumentException("parameter error!", "eventArgument")
                End If

                '取得開始-終了範囲を設定
                _callBackResult.PageIndexFrom = pageIndexFrom
                _callBackResult.PageIndexTo = pageIndexTo

                'データ取得イベントを発生させる
                Dim eventObj As New CustomGridViewPagingEventArgs(pageIndexFrom, pageIndexTo, PageSize, sortColIndex, sortDvs)
                'イベント実行
                RaiseEvent Paging(Me, eventObj)

                '検索結果を保存
                _callBackResult.Data = eventObj.Data
                _callBackResult.TotalCount = eventObj.TotalCount

            Catch ex As Exception
                'エラーフラグON()
                _callBackResult.CallBackError = True
                _callBackResult.CallBackErrorMessege = ex.Message
                'コールバックでは、BaseHttpApplication.BaseHttpApplication_Errorが発生しないため、
                'ここで独自にログ出力する。
                Logger.Error(ex.Message, ex)
            End Try


        End Sub
#End Region

#Region " リソース管理・JavaScript作成 "

        ''' <summary>
        ''' リソースよりスクリプトをページオブジェクトに登録します。
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub RegisterScriptResource()
            Me.Page.ClientScript.RegisterClientScriptResource(Me.GetType, "CustomGridView.js")
        End Sub

#End Region

#Region " 内部処理 "

        ''' <summary>
        ''' DictionaryよりJSON形式の文字列を取得します。
        ''' </summary>
        ''' <param name="args">変換対象なるデータのディクショナリ</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Shared Function CreateJsonString(ByVal args As Dictionary(Of String, String), ByVal noStringFlag As Boolean) As String
            Dim sb As New StringBuilder
            Dim cnt As Integer = 0
            '開始文字列
            sb.Append("{")

            'JSON形式の文字列作成
            For Each val As KeyValuePair(Of String, String) In args
                If cnt > 0 Then
                    '２番目以降のフィールド
                    sb.Append(",")
                End If

                '名称
                sb.Append("'").Append(val.Key).Append("'")
                '区切り
                sb.Append(": ")
                If noStringFlag Then
                    'そのまま文字列にせずに登録
                    sb.Append(val.Value)
                Else
                    '値(エスケープはしない)
                    sb.Append("'").Append(val.Value).Append("'")
                End If

                cnt += 1 'カウンタ
            Next

            '終了文字列
            sb.Append("}")

            Return sb.ToString
        End Function


        ''' <summary>
        ''' エラー通知用JSONデータ形式の文字列を取得します。
        ''' </summary>
        ''' <param name="errorMsg">メッセージ</param>
        ''' <param name="redirectUrl">遷移先画面</param>
        ''' <returns>JSON形式のエラー通知データ</returns>
        ''' <remarks></remarks>
        Friend Shared Function CreateAsyncSearchTableErrorJSONData(ByVal errorMsg As String, ByVal redirectUrl As String) As String

            Dim jsonString As New System.Text.StringBuilder

            With jsonString
                .Append("{ ")
                .Append("""errorMessege"": """)
                .Append(HttpUtility.JavaScriptStringEncode(errorMsg)).Append(""",")
                .Append("""redirectUrl"": """)
                .Append(HttpUtility.JavaScriptStringEncode(redirectUrl)).Append("""")
                .Append("}")
            End With

            Return jsonString.ToString()
        End Function

        ''' <summary>
        ''' 500エラー
        ''' </summary>
        Private Const WEB_500ERROR As String = "500"

        ''' <summary>
        ''' システムエラー(500エラー)が発生した際に遷移する画面のURLを取得します。
        ''' </summary>
        ''' <returns>遷移先画面URL</returns>
        ''' <remarks>
        ''' ※コールバック処理では、カスタムエラー設定によっての自動リダイレクトが使用できないため、独自実装するしかないため。
        ''' </remarks>
        Private Function GetErrorPageUrl() As String

            Dim query As String = "?aspxerrorpath=" & HttpUtility.UrlEncode(Me.Page.Request.Url.AbsolutePath)

            'web.configからカスタムエラーページの情報を取得する
            Dim section As Object = WebConfigurationManager.GetSection("system.web/customErrors")
            If section Is Nothing Then
                '空文字列を返却
                Return Me.ResolveClientUrl("~/Error/SC3010301.aspx") & query
            End If

            Dim errorSection As CustomErrorsSection = DirectCast(section, CustomErrorsSection)
            Dim redirectUrl As String

            '500エラー用のカスタムエラーページが設定されているか判定
            If errorSection.Errors.Item(WEB_500ERROR) IsNot Nothing Then
                '設定あり
                redirectUrl = errorSection.Errors.Item(WEB_500ERROR).Redirect
            Else
                '設定なし
                redirectUrl = errorSection.DefaultRedirect
            End If

            'クライアントサイドで使用できるURLに変換
            redirectUrl = Me.ResolveClientUrl(redirectUrl) & query

            '変換したURLを返却
            Return redirectUrl
        End Function

#End Region

#Region " ポストバック処理 "

        ''' <summary>
        ''' ポストバックイベントを処理します。
        ''' </summary>
        ''' <param name="eventArgument">イベントの引数</param>
        ''' <remarks>行選択時に<see cref="CustomGridView.SelectedRow"/>イベントを発生させます。</remarks>
        Public Sub RaisePostBackEvent(eventArgument As String) Implements System.Web.UI.IPostBackEventHandler.RaisePostBackEvent

            If C_POSTBACK_ARGS.Equals(eventArgument) Then

                '行選択の場合
                Dim param As String = Page.Request.Form(GetHiddenID) & ""
                Dim serializer As New JavaScriptSerializer
                Dim args As Dictionary(Of String, Object)

                'JSON形式の文字列を変換
                args = serializer.Deserialize(Of Dictionary(Of String, Object))(param)
                'イベント引数を作成
                Dim eventArgs As New CustomGridViewSelectedRowEventArgs
                Dim keys As Dictionary(Of String, Object) = CType(args("SELKEY"), Dictionary(Of String, Object))
                eventArgs.DataKeys = New Dictionary(Of String, String)
                For Each item As KeyValuePair(Of String, Object) In keys
                    '値を格納
                    eventArgs.DataKeys(item.Key) = item.Value.ToString
                Next

                'イベントを発生させる
                RaiseEvent SelectedRow(Me, eventArgs)

            End If

        End Sub

#End Region

    End Class

#Region " 列クラス "

    ''' <summary>
    ''' <see cref="CustomGridView"/> の列を表す基本クラス（マーカークラス）です。
    ''' </summary>
    ''' <remarks></remarks>
    Public MustInherit Class BaseCustomGridViewColumn

        Private _headerTextWordNo As Decimal
        Private _width As Unit = Unit.Empty
        Private _dataCssClass As String = String.Empty

        ''' <summary>
        ''' ヘッダーに表示するテキストの文言Noを取得又は設定します。
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property HeaderTextWordNo As Decimal
            Get
                Return _headerTextWordNo
            End Get
            Set(value As Decimal)
                _headerTextWordNo = value
            End Set
        End Property


        ''' <summary>
        ''' 列幅を取得又は設定します。
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Width As Unit
            Get
                Return _width
            End Get
            Set(value As Unit)
                _width = value
            End Set
        End Property

        ''' <summary>
        ''' データセルに適用するCSSクラス名を取得又は設定します。
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <CssClassProperty()> _
        Public Property DataCssClass As String
            Get
                Return _dataCssClass
            End Get
            Set(value As String)
                _dataCssClass = value
            End Set
        End Property

    End Class

    ''' <summary>
    ''' 列クラス
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable()> _
    Public Class CustomGridViewColumn
        Inherits BaseCustomGridViewColumn

        Private _dataFieldName As String = String.Empty

        ''' <summary>
        ''' バインドするテーブルのフィールド名を取得又は設定します。
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataFieldName As String
            Get
                Return _dataFieldName
            End Get
            Set(value As String)
                _dataFieldName = value
            End Set
        End Property

    End Class

    ''' <summary>
    ''' テンプレート列クラス
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable()> _
    Friend Class CustomGridViewTemplateColumn
        Inherits BaseCustomGridViewColumn
        Implements INamingContainer

        Private _dataCellTemplate As ITemplate

        ''' <summary>
        ''' データ行のテンプレートです。
        ''' </summary>
        <TemplateContainer(GetType(CustomGridViewTemplateColumn)), PersistenceMode(PersistenceMode.InnerProperty)> _
        Public Property DataCellTemplate As ITemplate
            Get
                Return _dataCellTemplate
            End Get
            Set(value As ITemplate)
                _dataCellTemplate = value
            End Set
        End Property

    End Class
#End Region

 

#Region " イベント引数のクラス宣言 "
    ''' <summary>
    ''' <see cref="CustomGridView"/> コントロールで行を選択した際のイベントのデータ。
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CustomGridViewSelectedRowEventArgs
        Inherits EventArgs

        Private _dataKeys As Dictionary(Of String, String)

        ''' <summary>
        ''' <see cref="CustomGridView.DataKeyFieldNames"/> プロパティで指定したフィード名に対応する選択した行のキー値を取得します。
        ''' </summary>
        ''' <returns><see cref="CustomGridView.DataKeyFieldNames"/> プロパティで指定したフィード名に対応する選択した行のキー値</returns>
        ''' <remarks></remarks>
        Public Property DataKeys As Dictionary(Of String, String)
            Get
                Return _dataKeys
            End Get
            Set(value As Dictionary(Of String, String))
                _dataKeys = value
            End Set
        End Property
    End Class

    ''' <summary>
    ''' 非同期データ取得のサーバーコールバックが発生した場合に呼ばれるイベントのデータ。
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CustomGridViewPagingEventArgs
        Inherits EventArgs

        ''' <summary>
        ''' ページインデックス(開始)
        ''' </summary>
        Private _pageIndexFrom As Integer
        ''' <summary>
        ''' ページインデックス(終了)
        ''' </summary>
        Private _pageIndexTo As Integer
        ''' <summary>
        ''' 検索結果
        ''' </summary>
        Private _data As Object
        ''' <summary>
        ''' 1ページの行数
        ''' </summary>
        ''' <remarks></remarks>
        Private _pagingRowCount As Integer
        ''' <summary>
        ''' データ総件数
        ''' </summary>
        Private _totalCount As Integer
        ''' <summary>
        ''' ソート列番号
        ''' </summary>
        Private _sortColumnIndex As Integer = -1
        ''' <summary>
        ''' ソート方向
        ''' </summary>
        Private _sortDirection As CustomGridViewSortDirection

        ''' <summary>
        ''' ０から始まるページインデックス(開始)を取得します。
        ''' </summary>
        Public ReadOnly Property PageIndexFrom As Integer
            Get
                Return _pageIndexFrom
            End Get
        End Property

        ''' <summary>
        ''' ０から始まるページインデックス(終了)を取得します。
        ''' </summary>
        Public ReadOnly Property PageIndexTo As Integer
            Get
                Return _pageIndexTo
            End Get
        End Property

        ''' <summary>
        ''' １ページ（１回のデータ表示）に表示する行数を取得します。
        ''' </summary>
        Public ReadOnly Property PagingRowCount As Integer
            Get
                Return _pagingRowCount
            End Get
        End Property

        ''' <summary>
        ''' データ取得の開始インデックスを取得します。<br/>
        ''' ０から始まる連番です。
        ''' </summary>
        Public ReadOnly Property StartRowIndex As Integer
            Get
                Return PageIndexFrom * PagingRowCount
            End Get
        End Property

        ''' <summary>
        ''' データ取得の終了インデックスを取得します。<br/>
        ''' ０から始まる連番です。
        ''' </summary>
        Public ReadOnly Property EndRowIndex As Integer
            Get
                Return (_pageIndexTo * PagingRowCount) + PagingRowCount - 1
            End Get
        End Property

        ''' <summary>
        ''' ソート列インデックスを取得します。<br/>
        ''' ソート列が指定されていない場合は、-1が設定されます。
        ''' </summary>
        Public ReadOnly Property SortColumnIndex As Integer
            Get
                Return _sortColumnIndex
            End Get
        End Property

        ''' <summary>
        ''' ソート方向を取得します。
        ''' </summary>
        Public ReadOnly Property SortDirection As CustomGridViewSortDirection
            Get
                Return _sortDirection
            End Get
        End Property

        ''' <summary>
        ''' <see cref="StartRowIndex"/>から<see cref="EndRowIndex"/>に対応する検索結果データを取得又は設定します。
        ''' </summary>
        Public Property Data As Object
            Get
                Return _data
            End Get
            Set(value As Object)
                _data = value
            End Set
        End Property

        ''' <summary>
        ''' データ総件数を取得又は設定します。
        ''' </summary>
        ''' <remarks>
        ''' このプロパティは主にページングの制御に使用します。
        ''' </remarks>
        Public Property TotalCount As Integer
            Get
                Return _totalCount
            End Get
            Set(value As Integer)
                _totalCount = value
            End Set
        End Property

        ''' <summary>
        ''' ページ番号を指定して当クラスのインスタンスを生成します。
        ''' </summary>
        ''' <param name="pageIndexFromParam">０から始まるページインデックス(開始)</param>
        ''' <param name="pageIndexToParam">０から始まるページインデックス(終了)</param>
        ''' <param name="pagingRowCountParam"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal pageIndexFromParam As Integer _
                     , ByVal pageIndexToParam As Integer _
                     , ByVal pagingRowCountParam As Integer _
                     , ByVal sortColumnIndexParam As Integer _
                     , ByVal sortDirectionParam As CustomGridViewSortDirection)
            _pageIndexFrom = pageIndexFromParam
            _pageIndexTo = pageIndexToParam
            _pagingRowCount = pagingRowCountParam
            _sortColumnIndex = sortColumnIndexParam
            _sortDirection = sortDirectionParam
        End Sub
    End Class

#End Region

#Region " ソート方向の列挙型 "
    ''' <summary>
    ''' ソート方向の列挙型
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum CustomGridViewSortDirection As Integer
        ''' <summary>
        ''' ソート指定なし
        ''' </summary>
        None = 0
        ''' <summary>
        ''' 昇順
        ''' </summary>
        Asc = 1
        ''' <summary>
        ''' 降順
        ''' </summary>
        Desc = 2
    End Enum
#End Region

End Namespace

