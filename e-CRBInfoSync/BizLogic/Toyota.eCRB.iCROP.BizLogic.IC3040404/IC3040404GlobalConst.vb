
Namespace IC3040404.BizLogic

    ''' <summary>
    ''' 定数値  2012/12/12
    ''' </summary>
    ''' <remarks>
    ''' 定数定義のみ
    ''' </remarks>
    Class GlobalConst

        Public Const CrLf As String = vbCrLf '改行文字

        '通信ステータス
        Public Const HttpStat200 As Integer = 200 'OK
        Public Const HttpStat201 As Integer = 201 'Created       for Put:Insert Add 2011/12/26
        Public Const HttpStat204 As Integer = 204 'No Content    for Put:Update
        Public Const HttpStat207 As Integer = 207 'MultiStatus
        Public Const HttpStat401 As Integer = 401 'Unauthorized
        Public Const HttpStat403 As Integer = 403 'Forbidden
        Public Const HttpStat405 As Integer = 405 'Not Found
        Public Const HttpStat500 As Integer = 500 'Internal Server Error
        Public Const HttpStat501 As Integer = 501 'Not implemented
        Public Const HttpStat422 As Integer = 422 'Unprocessible Entity

        Public Const Culture As Integer = StringComparison.CurrentCulture
 
        'これらは不要
        'Public Const HEAD_DAV_01 As String = "1, 2, 3, access-control, calendar-access, calendar-schedule"
        'Public Const HEAD_DAV_02 As String = "extended-mkcol, calendar-proxy, bind, addressbook, calendar-auto-schedule"
        'Public Const HEAD_ALLOW As String = "OPTIONS, PROPFIND, REPORT, DELETE, LOCK, UNLOCK, MOVE, GET, HEAD, MKCOL, MKCALENDAR, PUT, PROPPATCH, BIND, ACL"
        'Public Const HEAD_ALLOW As String = "OPTIONS, PROPFIND, REPORT, DELETE, HEAD, PUT, PROPPATCH"

        Public Const HeadDav As String = "1, 2, 3, calendar-access"
        Public Const HeadAllow As String = "OPTIONS, PROPFIND, REPORT, DELETE, HEAD, PUT"
        Public Const HeadServer As String = "IIS/7.5 (Windows)"

        Public Const CalDavProgramId As String = "IC3040404"

        Public Const DateLowValue As DateTime = #12:00:00 AM#             '0001/01/01 00:00:01
        Public Const DateHighValue As DateTime = #12/31/9999 11:59:59 PM# '9999/12/29 23:59:59

        'DATETIMEにNothingを設定すると 0001/1/1 0:0:00になるので回避策 0001/1/1 1:1:1
        'この定数はデータアクセス層のwrapAddParameterWithTypeValueも変更すること
        Public Const DateNothingValue As DateTime = #1:01:01 AM#             '0001/01/01 01:01:01

        'path取得用
        Public Const RootPath As Integer = 0
        Public Const HomePath As Integer = 1
        Public Const CalendarPath As Integer = 2
        Public Const DisplayName As Integer = 3
        Public Const CalendarRelPath As Integer = 4

        'レスポンスの文字列
        'propfind 全体ヘッダ
        Public Const HttpHead1 As String = _
            "<?xml version=""1.0"" encoding=""utf-8"" ?>" & CrLf & _
            "<multistatus xmlns=""DAV:"" xmlns:C=""urn:ietf:params:xml:ns:caldav"">" & CrLf

        'propfind 全体ヘッダ calendar-free-busy-set　があるときはこのパターン
        Public Const HttpHead2 As String = _
            "<?xml version=""1.0"" encoding=""utf-8"" ?>" & CrLf & _
            "<multistatus xmlns=""DAV:"" xmlns:C=""http://calendarserver.org/ns/""" _
            & " xmlns:M=""http://me.com/_namespace/"" xmlns:A=""http://apple.com/ns/ical/""" _
            & " xmlns:C1=""urn:ietf:params:xml:ns:caldav"" xmlns:C2=""urn:ietf:params:xml:ns:carddav"">"

        'propfind 全体ヘッダ calendar-home-set　があるときはこのパターン
        Public Const HttpHead3 As String = _
            "<?xml version=""1.0"" encoding=""utf-8"" ?>" & CrLf & _
            "<multistatus xmlns=""DAV:"" xmlns:C=""http://calendarserver.org/ns/""" _
            & " xmlns:C1=""urn:ietf:params:xml:ns:caldav"">" & CrLf

        'report 標準ヘッダ
        Public Const HttpHeadStd As String = _
            "<?xml version=""1.0"" encoding=""utf-8"" ?>" & CrLf & _
            "<multistatus xmlns=""DAV:"">" & CrLf

        'reprot 名前空間Cのみあり
        Public Const HttpHead12 As String = _
            "<?xml version=""1.0"" encoding=""utf-8"" ?>" & CrLf & _
            "<multistatus xmlns=""DAV:"" xmlns:C=""urn:ietf:params:xml:ns:caldav"">" & CrLf

        'レスポンスヘッダ　String.Formatでパスを入れること{0}
        Public Const HttpResHead As String = _
            "<response>" & CrLf & _
            "<href>{0}</href>" & CrLf

        'ボディのヘッダ（OK／エラー共通）
        Public Const HttpResBodyHead As String = _
            "<propstat>" & CrLf & _
            "<prop>" & CrLf

        'OKの場合のフッタ
        Public Const HttpResFootProp200 As String = _
            "</prop>" & CrLf & _
            "<status>HTTP/1.1 200 OK</status>" & CrLf & _
            "</propstat>" & CrLf

        'エラーの場合のフッタ
        Public Const HttpResFootProp404 As String = _
            "</prop>" & CrLf & _
            "<status>HTTP/1.1 404 Not Found</status>" & CrLf & _
            "</propstat>" & CrLf

        '全体のフッタ
        Public Const HttpResFoot As String = _
            "</response>" & CrLf & _
            "</multistatus>" & CrLf

        '全体のフッタ
        Public Const HttpResFoot2 As String = _
            "</multistatus>" & CrLf

        Private Sub New()
            'dummy
        End Sub

    End Class

End Namespace
