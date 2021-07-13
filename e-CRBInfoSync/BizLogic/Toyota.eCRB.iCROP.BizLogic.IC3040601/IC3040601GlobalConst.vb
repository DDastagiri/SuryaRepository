
Namespace IC3040601.BizLogic

    ''' <summary>
    ''' 定数値
    ''' </summary>
    ''' <remarks>
    ''' 定数定義のみ
    ''' </remarks>
    Class GlobalConst

        '通信ステータス
        Public Const HTTP_STAT_200 As Integer = 200 'OK
        Public Const HTTP_STAT_207 As Integer = 207 'MultiStatus
        Public Const HTTP_STAT_401 As Integer = 401 'Unauthorized
        Public Const HTTP_STAT_403 As Integer = 403 'Forbidden
        Public Const HTTP_STAT_405 As Integer = 405 'Not Found
        Public Const HTTP_STAT_500 As Integer = 500 'Internal Server Error
        Public Const HTTP_STAT_501 As Integer = 501 'Not implemented
        Public Const HTTP_STAT_422 As Integer = 422 'Unprocessible Entity

        '''Public Const HEAD_DAV_01 As String = "1, 2, 3, access-control, calendar-access, calendar-schedule"
        '''Public Const HEAD_DAV_02 As String = "extended-mkcol, calendar-proxy, bind, addressbook, calendar-auto-schedule"
        Public Const HEAD_DAV_01 As String = "addressbook"
        '''Public Const HEAD_ALLOW As String = "OPTIONS, PROPFIND, REPORT, DELETE, LOCK, UNLOCK, MOVE, GET, HEAD, MKCOL, MKCALENDAR, PROPPATCH, BIND, ACL"
        Public Const HEAD_ALLOW As String = "OPTIONS, PROPFIND, REPORT"
        Public Const HEAD_SERVER As String = "IIS/7.5 (Windows)"

        Public Const CARDDAV_PROGRM_ID As String = "IC3040601"

        'レスポンスの文字列群
        '全体のヘッダ　String.Formatでパスを入れること{0}
        Public Const HTTP_RES_HEAD As String = _
            "<?xml version=""1.0"" encoding=""utf-8"" ?>" & vbCrLf & _
            "<multistatus xmlns=""DAV:"" xmlns:C=""urn:ietf:params:xml:ns:caldav"">" & vbCrLf & _
            "<response>" & vbCrLf & _
            "<href>{0}</href>" & vbCrLf

        'ボディのヘッダ（OK／エラー共通）
        Public Const HTTP_RES_BODY_HEAD As String = _
            "<propstat>" & vbCrLf & _
            "<prop>" & vbCrLf

        'OKの場合のフッタ
        Public Const HTTP_RES_FOOT_PROP_200 As String = _
            "</prop>" & vbCrLf & _
            "<status>HTTP/1.1 200 OK</status>" & vbCrLf & _
            "</propstat>" & vbCrLf

        'エラーの場合のフッタ
        Public Const HTTP_RES_FOOT_PROP_404 As String = _
            "</prop>" & vbCrLf & _
            "<status>HTTP/1.1 404 Not Found</status>" & vbCrLf & _
            "</propstat>" & vbCrLf

        '全体のフッタ
        Public Const HTTP_RES_FOOT As String = _
            "</response>" & vbCrLf & _
            "</multistatus>" & vbCrLf

        'PROPFIND １回目のレスポンス
        Public Const HTTP_RES_PROPFIND1 As String =
            "<?xml version=""1.0"" encoding=""utf-8"" ?>" & vbCrLf & _
            "<multistatus xmlns=""DAV:"" >" & vbCrLf & _
            " <response>" & vbCrLf & _
            "  <href>{0}</href>" & vbCrLf & _
            "  <propstat>" & vbCrLf & _
            "   <prop>" & vbCrLf & _
            "    <principal-URL>" & vbCrLf & _
            "     <href>{0}</href>" & vbCrLf & _
            "    </principal-URL>" & vbCrLf & _
            "    <resourcetype>" & vbCrLf & _
            "     <collection/>" & vbCrLf & _
            "     <addressbook/>" & vbCrLf & _
            "    </resourcetype>" & vbCrLf & _
            "   </prop>" & vbCrLf & _
            "   <status>HTTP/1.1 200 OK</status>" & vbCrLf & _
            "  </propstat>" & vbCrLf & _
            "  <propstat>" & vbCrLf & _
            "      <prop>" & vbCrLf & _
            "          <current-user-principal/>" & vbCrLf & _
            "      </prop>" & vbCrLf & _
            "      <status>HTTP/1.1 404 Not Found</status>" & vbCrLf & _
            "  </propstat>" & vbCrLf & _
            " </response>" & vbCrLf & _
            "</multistatus>" & vbCrLf

        'PROPFIND ２回目のレスポンス
        Public Const HTTP_RES_PROPFIND2 As String =
            "<?xml version=""1.0"" encoding=""utf-8"" ?>" & vbCrLf & _
            "<multistatus xmlns=""DAV:"" xmlns:C=""urn:ietf:params:xml:ns:carddav"" xmlns:C1=""http://calendarserver.org/ns/"">" & vbCrLf & _
            " <response>" & vbCrLf & _
            "  <href>{0}</href>" & vbCrLf & _
            "  <propstat>" & vbCrLf & _
            "   <prop>" & vbCrLf & _
            "    <C:addressbook-home-set>" & vbCrLf & _
            "     <href>{0}</href>" & vbCrLf & _
            "    </C:addressbook-home-set>" & vbCrLf & _
            "    <displayname>Toyota-sales</displayname>" & vbCrLf & _
            "    <principal-collection-set>" & vbCrLf & _
            "     <href>{0}</href>" & vbCrLf & _
            "    </principal-collection-set>" & vbCrLf & _
            "    <principal-URL>" & vbCrLf & _
            "     <href>{0}</href>" & vbCrLf & _
            "    </principal-URL>" & vbCrLf & _
            "    <resource-id>" & vbCrLf & _
            "     <href>{0}.resources/1001</href>" & vbCrLf & _
            "    </resource-id>" & vbCrLf & _
            "    <supported-report-set>" & vbCrLf & _
            "     <supported-report>" & vbCrLf & _
            "      <report>" & vbCrLf & _
            "       <principal-property-search/>" & vbCrLf & _
            "      </report>" & vbCrLf & _
            "     </supported-report>" & vbCrLf & _
            "     <supported-report>" & vbCrLf & _
            "      <report>" & vbCrLf & _
            "       <principal-search-property-set/>" & vbCrLf & _
            "      </report>" & vbCrLf & _
            "     </supported-report>" & vbCrLf & _
            "     <supported-report>" & vbCrLf & _
            "      <report>" & vbCrLf & _
            "       <expand-property/>" & vbCrLf & _
            "      </report>" & vbCrLf & _
            "     </supported-report>" & vbCrLf & _
            "     <supported-report>" & vbCrLf & _
            "      <report>" & vbCrLf & _
            "       <sync-collection/>" & vbCrLf & _
            "      </report>" & vbCrLf & _
            "     </supported-report>" & vbCrLf & _
            "    </supported-report-set>" & vbCrLf & _
            "   </prop>" & vbCrLf & _
            "   <status>HTTP/1.1 200 OK</status>" & vbCrLf & _
            "  </propstat>" & vbCrLf & _
            "  <propstat>" & vbCrLf & _
            "   <prop>" & vbCrLf & _
            "    <C:directory-gateway/>" & vbCrLf & _
            "    <C1:email-address-set/>" & vbCrLf & _
            "   </prop>" & vbCrLf & _
            "   <status>HTTP/1.1 404 Not Found</status>" & vbCrLf & _
            "  </propstat>" & vbCrLf & _
            " </response>" & vbCrLf & _
            "</multistatus>" & vbCrLf

        'PROPFIND ３回目のレスポンス
        Public Const HTTP_RES_PROPFIND3 As String =
            "<?xml version=""1.0"" encoding=""utf-8"" ?>" & vbCrLf & _
            "<multistatus xmlns=""DAV:"" xmlns:C=""urn:ietf:params:xml:ns:carddav"">" & vbCrLf & _
            " <response>" & vbCrLf & _
            "  <href>{0}</href>" & vbCrLf & _
            "  <propstat>" & vbCrLf & _
            "   <prop>" & vbCrLf & _
            "    <C:addressbook-home-set>{0}</C:addressbook-home-set>" & vbCrLf & _
            "   </prop>" & vbCrLf & _
            "   <status>HTTP/1.1 200 OK</status>" & vbCrLf & _
            "  </propstat>" & vbCrLf & _
            " </response>" & vbCrLf & _
            "</multistatus>" & vbCrLf

        'PROPFIND ４回目のレスポンス
        Public Const HTTP_RES_PROPFIND4 As String =
            "<?xml version=""1.0"" encoding=""utf-8"" ?>" & vbCrLf & _
            "<multistatus xmlns=""DAV:"" xmlns:M=""http://me.com/_namespace/"" xmlns:C=""urn:ietf:params:xml:ns:carddav"" xmlns:C1=""http://calendarserver.org/ns/"" xmlns:C2=""urn:ietf:params:xml:ns:caldav"">" & vbCrLf & _
            " <response>" & vbCrLf & _
            "  <href>{0}</href>" & vbCrLf & _
            "  <propstat>" & vbCrLf & _
            "   <prop>" & vbCrLf & _
            "    <current-user-privilege-set>" & vbCrLf & _
            "     <privilege>" & vbCrLf & _
            "      <all/>" & vbCrLf & _
            "     </privilege>" & vbCrLf & _
            "     <privilege>" & vbCrLf & _
            "      <read/>" & vbCrLf & _
            "     </privilege>" & vbCrLf & _
            "     <privilege>" & vbCrLf & _
            "      <unlock/>" & vbCrLf & _
            "     </privilege>" & vbCrLf & _
            "     <privilege>" & vbCrLf & _
            "      <read-acl/>" & vbCrLf & _
            "     </privilege>" & vbCrLf & _
            "     <privilege>" & vbCrLf & _
            "      <read-current-user-privilege-set/>" & vbCrLf & _
            "     </privilege>" & vbCrLf & _
            "     <privilege>" & vbCrLf & _
            "      <write-acl/>" & vbCrLf & _
            "     </privilege>" & vbCrLf & _
            "     <privilege>" & vbCrLf & _
            "      <write/>" & vbCrLf & _
            "     </privilege>" & vbCrLf & _
            "     <privilege>" & vbCrLf & _
            "      <write-properties/>" & vbCrLf & _
            "     </privilege>" & vbCrLf & _
            "     <privilege>" & vbCrLf & _
            "      <write-content/>" & vbCrLf & _
            "     </privilege>" & vbCrLf & _
            "     <privilege>" & vbCrLf & _
            "      <bind/>" & vbCrLf & _
            "     </privilege>" & vbCrLf & _
            "     <privilege>" & vbCrLf & _
            "      <unbind/>" & vbCrLf & _
            "     </privilege>" & vbCrLf & _
            "    </current-user-privilege-set>" & vbCrLf & _
            "    <displayname>Toyota-sales addressbook</displayname>" & vbCrLf & _
            "    <C:max-resource-size>65500</C:max-resource-size>" & vbCrLf & _
            "    <owner>" & vbCrLf & _
            "     <href>{0}</href>" & vbCrLf & _
            "    </owner>" & vbCrLf & _
            "    <resource-id>" & vbCrLf & _
            "     <href>{0}.resources/1001</href>" & vbCrLf & _
            "    </resource-id>" & vbCrLf & _
            "    <resourcetype>" & vbCrLf & _
            "     <collection/>" & vbCrLf & _
            "     <C:addressbook/>" & vbCrLf & _
            "    </resourcetype>" & vbCrLf & _
            "    <supported-report-set>" & vbCrLf & _
            "     <supported-report>" & vbCrLf & _
            "      <report>" & vbCrLf & _
            "       <principal-property-search/>" & vbCrLf & _
            "      </report>" & vbCrLf & _
            "     </supported-report>" & vbCrLf & _
            "     <supported-report>" & vbCrLf & _
            "      <report>" & vbCrLf & _
            "       <principal-search-property-set/>" & vbCrLf & _
            "      </report>" & vbCrLf & _
            "     </supported-report>" & vbCrLf & _
            "     <supported-report>" & vbCrLf & _
            "      <report>" & vbCrLf & _
            "       <expand-property/>" & vbCrLf & _
            "      </report>" & vbCrLf & _
            "     </supported-report>" & vbCrLf & _
            "     <supported-report>" & vbCrLf & _
            "      <report>" & vbCrLf & _
            "       <sync-collection/>" & vbCrLf & _
            "      </report>" & vbCrLf & _
            "     </supported-report>" & vbCrLf & _
            "     <supported-report>" & vbCrLf & _
            "      <report>" & vbCrLf & _
            "       <C:addressbook-query/>" & vbCrLf & _
            "      </report>" & vbCrLf & _
            "     </supported-report>" & vbCrLf & _
            "     <supported-report>" & vbCrLf & _
            "      <report>" & vbCrLf & _
            "       <C:addressbook-multiget/>" & vbCrLf & _
            "      </report>" & vbCrLf & _
            "     </supported-report>" & vbCrLf & _
            "    </supported-report-set>" & vbCrLf & _
            "   </prop>" & vbCrLf & _
            "   <status>HTTP/1.1 200 OK</status>" & vbCrLf & _
            "  </propstat>" & vbCrLf & _
            "  <propstat>" & vbCrLf & _
            "   <prop>" & vbCrLf & _
            "    <add-member/>" & vbCrLf & _
            "    <M:bulk-requests/>" & vbCrLf & _
            "    <C:max-image-size/>" & vbCrLf & _
            "    <C1:me-card/>" & vbCrLf & _
            "    <C1:push-transports/>" & vbCrLf & _
            "    <C1:pushkey/>" & vbCrLf & _
            "    <quota-available-bytes/>" & vbCrLf & _
            "    <quota-used-bytes/>" & vbCrLf & _
            "    <sync-token/>" & vbCrLf & _
            "   </prop>" & vbCrLf & _
            "   <status>HTTP/1.1 404 Not Found</status>" & vbCrLf & _
            "  </propstat>" & vbCrLf & _
            " </response>" & vbCrLf & _
            "</multistatus>" & vbCrLf

        'PROPFIND ５回目のレスポンス(getctag)
        Public Const HTTP_RES_PROPFIND5 As String =
            "<?xml version=""1.0"" encoding=""utf-8"" ?>" & vbCrLf & _
            "<multistatus xmlns=""DAV:"" xmlns:C=""http://calendarserver.org/ns/"">" & vbCrLf & _
            " <response>" & vbCrLf & _
            "  <href>{0}</href>" & vbCrLf & _
            "  <propstat>" & vbCrLf & _
            "   <prop>" & vbCrLf & _
            "    <C:getctag>"""

        Public Const HTTP_RES_PROPFIND5_2 As String =
            """</C:getctag>" & vbCrLf & _
            "   </prop>" & vbCrLf & _
            "   <status>HTTP/1.1 200 OK</status>" & vbCrLf & _
            "  </propstat>" & vbCrLf & _
            "  <propstat>" & vbCrLf & _
            "   <prop>" & vbCrLf & _
            "    <sync-token/>" & vbCrLf & _
            "   </prop>" & vbCrLf & _
            "   <status>HTTP/1.1 404 Not Found</status>" & vbCrLf & _
            "  </propstat>" & vbCrLf & _
            " </response>" & vbCrLf & _
            "</multistatus>" & vbCrLf


        'PROPFIND ６回目のレスポンス(getetag)
        Public Const HTTP_RES_PROPFIND6 As String =
            "<?xml version=""1.0"" encoding=""utf-8"" ?>" & vbCrLf & _
            "<multistatus xmlns=""DAV:"">" & vbCrLf & _
            " <response>" & vbCrLf & _
            "  <href>{0}</href>" & vbCrLf & _
            "  <propstat>" & vbCrLf & _
            "   <prop/>" & vbCrLf & _
            "   <status>HTTP/1.1 200 OK</status>" & vbCrLf & _
            "  </propstat>" & vbCrLf & _
            "  <propstat>" & vbCrLf & _
            "   <prop>" & vbCrLf & _
            "    <getetag/>" & vbCrLf & _
            "   </prop>" & vbCrLf & _
            "   <status>HTTP/1.1 404 Not Found</status>" & vbCrLf & _
            "  </propstat>" & vbCrLf & _
            " </response>" & vbCrLf

        Public Const HTTP_RES_PROPFIND6_vcf_s As String =
            " <response>" & vbCrLf & _
            "  <href>{0}"

        Public Const HTTP_RES_PROPFIND6_vcf_e As String =
            ".vcf</href>" & vbCrLf & _
            "  <propstat>" & vbCrLf

        Public Const HTTP_RES_PROPFIND6_etag_s As String =
            "   <prop>" & vbCrLf & _
            "    <getetag>"""

        Public Const HTTP_RES_PROPFIND6_etag_e As String =
            """</getetag>" & vbCrLf & _
            "   </prop>" & vbCrLf & _
            "   <status>HTTP/1.1 200 OK</status>" & vbCrLf & _
            "  </propstat>" & vbCrLf & _
            " </response>" & vbCrLf

        Public Const HTTP_RES_PROPFIND6_END As String =
            "</multistatus>" & vbCrLf


        Private Sub New()
            'dummy
        End Sub

    End Class

End Namespace
