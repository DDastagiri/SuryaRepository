Imports System.Web

Namespace Toyota.eCRB.SystemFrameworks.Web

    ''' <summary>
    ''' サーバサイド用Cookie処理クラスです。
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class CookieUtility

        'Cookie格納用キー名
        Private Const SERVER_SIDE_COOKIE_KEY As String = "ServerSideCookie"

        'Cookie格納用区切り文字
        Private Const SERVER_SIDE_COOKIE_DELIMITER As Char = "$"c

        'Cookie格納値の区切り文字
        Private Const SERVER_SIDE_COOKIE_EQUAL As Char = "="c

        ''' <summary>
        ''' コンストラクタです
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub New()
        End Sub


        ''' <summary>
        ''' Cookie格納処理
        ''' </summary>
        ''' <param name="context">Cookie処理対象となるHttpContext</param>
        ''' <param name="key">格納する値のキー名</param>
        ''' <param name="value">格納する値</param>
        ''' <remarks></remarks>
        Public Shared Sub SetCookie(ByVal context As HttpContext, ByVal key As String, ByVal value As String)

            Dim isNew As Boolean = True
            'CookieからサーバサイドCookieを取得
            '区切り文字でSplitして配列化する
            Dim cookieArr As String() = GetCookieArray(context)

            '配列の要素分ループ
            For i As Integer = 0 To cookieArr.Length - 1
                '等号でSplitした先頭の要素が一致する場合
                If String.Equals(cookieArr(i).Split(SERVER_SIDE_COOKIE_EQUAL)(0), key) Then
                    '配列の要素を更新する
                    cookieArr(i) = key & SERVER_SIDE_COOKIE_EQUAL & value
                    isNew = False
                    Exit For
                End If
            Next

            '新規追加の値の場合
            If isNew Then
                '配列に追加する
                Array.Resize(cookieArr, cookieArr.Length + 1)
                cookieArr(cookieArr.Length - 1) = key & SERVER_SIDE_COOKIE_EQUAL & value
            End If

            '配列を区切り文字でJoinし、Cookieに格納
            SetCookieArray(context, cookieArr)

        End Sub

        ''' <summary>
        ''' Cookie取得処理
        ''' </summary>
        ''' <param name="context">Cookie処理対象となるHttpContext</param>
        ''' <param name="key">Cookieから取得する値のキー名</param>
        ''' <returns>Cookieから取得した値</returns>
        ''' <remarks></remarks>
        Public Shared Function GetCookie(ByVal context As HttpContext, ByVal key As String) As String

            'CookieからサーバサイドCookieを取得
            '区切り文字でSplitして配列化する
            Dim cookieArr As String() = GetCookieArray(context)

            '配列の要素分ループ
            For i As Integer = 0 To cookieArr.Length - 1
                '等号でSplitした先頭の要素が一致する場合
                If String.Equals(cookieArr(i).Split(SERVER_SIDE_COOKIE_EQUAL)(0), key) Then
                    '末尾の要素を戻り値として戻す
                    Return cookieArr(i).Split(SERVER_SIDE_COOKIE_EQUAL)(1)
                End If
            Next

            Return Nothing
        End Function

        ''' <summary>
        ''' Cookie削除処理
        ''' </summary>
        ''' <param name="context">Cookie処理対象となるHttpContext</param>
        ''' <param name="key">Cookieから削除する値のキー名</param>
        ''' <remarks></remarks>
        Public Shared Sub RemoveCookie(ByVal context As HttpContext, ByVal key As String)

            'CookieからサーバサイドCookieを取得
            '区切り文字でSplitして配列化する
            Dim cookieArr As String() = GetCookieArray(context)

            '配列をList型に変換する
            Dim cookieList As New List(Of String)
            cookieList.AddRange(cookieArr)

            '削除対象が存在する
            Dim exists As Boolean = False

            '配列の要素分ループ
            For i As Integer = 0 To cookieList.Count - 1
                '等号でSplitした先頭の要素が一致する場合
                If String.Equals(cookieList.Item(i).Split(SERVER_SIDE_COOKIE_EQUAL)(0), key) Then
                    '該当の要素を削除
                    cookieList.RemoveAt(i)
                    exists = True
                    Exit For
                End If
            Next

            '削除対象が存在する場合、Cookieに反映
            If exists Then
                '配列を区切り文字でJoinし、Cookieに格納
                SetCookieArray(context, cookieList.ToArray)
            End If
        End Sub


        ''' <summary>
        ''' Cookie配列取得処理
        ''' </summary>
        ''' <param name="context">Cookie処理対象となるHttpContext</param>
        ''' <returns>Cookieから取得した値の格納された配列</returns>
        ''' <remarks></remarks>
        Public Shared Function GetCookieArray(ByVal context As HttpContext) As String()

            '引数contextがNothingの場合
            If context Is Nothing Then
                '例外としてArgumentNullExceptionをスローする
                Throw New ArgumentNullException("context")
            End If

            Dim serverSideCookie As String

            'CookieからサーバサイドCookieを取得
            'ResposeからCookie取得
            serverSideCookie = context.Server.UrlDecode(context.Response.Cookies(SERVER_SIDE_COOKIE_KEY).Value)

            If String.IsNullOrEmpty(serverSideCookie) Then
                'Resposeに存在しない場合、RequestからResposeに設定
                context.Response.Cookies(SERVER_SIDE_COOKIE_KEY).Value = context.Request.Cookies(SERVER_SIDE_COOKIE_KEY).Value
                serverSideCookie = context.Server.UrlDecode(context.Response.Cookies(SERVER_SIDE_COOKIE_KEY).Value)
            End If

            '区切り文字でSplitして配列化する
            Dim cookieArr As String()
            If Not String.IsNullOrEmpty(serverSideCookie) Then
                cookieArr = serverSideCookie.Split(SERVER_SIDE_COOKIE_DELIMITER)
            Else
                cookieArr = New String() {}
            End If

            Return cookieArr
        End Function


        ''' <summary>
        ''' Cookie配列格納処理
        ''' </summary>
        ''' <param name="context">Cookie処理対象となるHttpContext</param>
        ''' <param name="cookieArr">格納する値の配列</param>
        ''' <remarks></remarks>
        Public Shared Sub SetCookieArray(ByVal context As HttpContext, ByVal cookieArr As String())

            '引数contextがNothingの場合
            If context Is Nothing Then
                '例外としてArgumentNullExceptionをスローする
                Throw New ArgumentNullException("context")
            End If

            '配列を区切り文字でJoinし、Cookieに格納
            Dim setValue As String = String.Join(SERVER_SIDE_COOKIE_DELIMITER, cookieArr)
            context.Response.Cookies(SERVER_SIDE_COOKIE_KEY).Value = context.Server.UrlEncode(setValue)
        End Sub

    End Class

End Namespace
