
Namespace IC3040404.BizLogic


    ''' <summary>
    ''' propfindのデータ   2012/12/12
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class GlobalResText

        'TEST DATA
        Public Const GetData As String = _
          "BEGIN:VCALENDAR" & vbCrLf _
        & "METHOD:PUBLISH" & vbCrLf _
        & "BEGIN:VEVENT" & vbCrLf _
        & "SEQUENCE:5" & vbCrLf _
        & "TRANSP:OPAQUE" & vbCrLf _
        & "UID:479DEF6F-05E3-4042-9A80-13BC493567C7" & vbCrLf _
        & "DTSTART:20111209T120000" & vbCrLf _
        & "DTSTAMP:20090721T011308Z" & vbCrLf _
        & "SUMMARY:Event Day" & vbCrLf _
        & "CREATED:20090720T234935Z" & vbCrLf _
        & "DTEND:20111209T120030" & vbCrLf _
        & "RRULE:FREQ=YEARLY;INTERVAL=1;BYMONTH=7;BYDAY=3MO" & vbCrLf _
        & "END:VEVENT" & vbCrLf _
        & "BEGIN:VEVENT" & vbCrLf _
        & "SEQUENCE:2" & vbCrLf _
        & "TRANSP:OPAQUE" & vbCrLf _
        & "UID:E245FD75-0DF3-4DE2-846F-A80C3D3C0144" & vbCrLf _
        & "DTSTART;VALUE=DATE:20111208" & vbCrLf _
        & "DTSTAMP:20090721T003801Z" & vbCrLf _
        & "SUMMARY:TOYOTAの日 (TOYOTA Day)" & vbCrLf _
        & "CREATED:20090720T234935Z" & vbCrLf _
        & "DTEND;VALUE=DATE:20111208" & vbCrLf _
        & "RRULE:FREQ=YEARLY;INTERVAL=1;BYMONTH=4" & vbCrLf _
        & "END:VEVENT" & vbCrLf _
        & "END:VCALENDAR" & vbCrLf


        ' 2012/10/23 SKFC 浦野【iOS6対応】未知のタグに対応 START
        Public Const PropData As String = "" & vbCrLf _
        & " <response>" & vbCrLf _
        & "  <href>{1}</href>" & vbCrLf _
        & "  <propstat>" & vbCrLf _
        & "   <prop>" & vbCrLf _
        & "    <C1:calendar-free-busy-set>" & vbCrLf _
        & "     <href>{2}</href>" & vbCrLf _
        & "    </C1:calendar-free-busy-set>" & vbCrLf _
        & "    <current-user-privilege-set>" & vbCrLf _
        & "     <privilege>" & vbCrLf _
        & "      <all/>" & vbCrLf _
        & "     </privilege>" & vbCrLf _
        & "     <privilege>" & vbCrLf _
        & "      <read/>" & vbCrLf _
        & "     </privilege>" & vbCrLf _
        & "     <privilege>" & vbCrLf _
        & "      <unlock/>" & vbCrLf _
        & "     </privilege>" & vbCrLf _
        & "     <privilege>" & vbCrLf _
        & "      <read-acl/>" & vbCrLf _
        & "     </privilege>" & vbCrLf _
        & "     <privilege>" & vbCrLf _
        & "      <read-current-user-privilege-set/>" & vbCrLf _
        & "     </privilege>" & vbCrLf _
        & "     <privilege>" & vbCrLf _
        & "      <write-acl/>" & vbCrLf _
        & "     </privilege>" & vbCrLf _
        & "     <privilege>" & vbCrLf _
        & "      <write/>" & vbCrLf _
        & "     </privilege>" & vbCrLf _
        & "     <privilege>" & vbCrLf _
        & "      <write-properties/>" & vbCrLf _
        & "     </privilege>" & vbCrLf _
        & "     <privilege>" & vbCrLf _
        & "      <write-content/>" & vbCrLf _
        & "     </privilege>" & vbCrLf _
        & "     <privilege>" & vbCrLf _
        & "      <bind/>" & vbCrLf _
        & "     </privilege>" & vbCrLf _
        & "     <privilege>" & vbCrLf _
        & "      <unbind/>" & vbCrLf _
        & "     </privilege>" & vbCrLf _
        & "    </current-user-privilege-set>" & vbCrLf _
        & "    <displayname>{3}</displayname>" & vbCrLf _
        & "    <C:getctag>""df9e0b95470f1fdc545b494b1d588a4e""</C:getctag>" & vbCrLf _
        & "    <owner>" & vbCrLf _
        & "     <href>{1}</href>" & vbCrLf _
        & "    </owner>" & vbCrLf _
        & "    <resource-id>" & vbCrLf _
        & "     <href>{0}.resources/1038</href>" & vbCrLf _
        & "    </resource-id>" & vbCrLf _
        & "    <resourcetype>" & vbCrLf _
        & "     <collection/>" & vbCrLf _
        & "     <principal/>" & vbCrLf _
        & "    </resourcetype>" & vbCrLf _
        & "    <C1:schedule-default-calendar-URL>" & vbCrLf _
        & "     <href>{4}</href>" & vbCrLf _
        & "    </C1:schedule-default-calendar-URL>" & vbCrLf _
        & "    <supported-report-set>" & vbCrLf _
        & "     <supported-report>" & vbCrLf _
        & "      <report>" & vbCrLf _
        & "       <principal-property-search/>" & vbCrLf _
        & "      </report>" & vbCrLf _
        & "     </supported-report>" & vbCrLf _
        & "     <supported-report>" & vbCrLf _
        & "      <report>" & vbCrLf _
        & "       <principal-search-property-set/>" & vbCrLf _
        & "      </report>" & vbCrLf _
        & "     </supported-report>" & vbCrLf _
        & "     <supported-report>" & vbCrLf _
        & "      <report>" & vbCrLf _
        & "       <expand-property/>" & vbCrLf _
        & "      </report>" & vbCrLf _
        & "     </supported-report>" & vbCrLf _
        & "     <supported-report>" & vbCrLf _
        & "      <report>" & vbCrLf _
        & "       <sync-collection/>" & vbCrLf _
        & "      </report>" & vbCrLf _
        & "     </supported-report>" & vbCrLf _
        & "    </supported-report-set>" & vbCrLf _
        & "   </prop>" & vbCrLf _
        & "   <status>HTTP/1.1 200 OK</status>" & vbCrLf _
        & "  </propstat>" & vbCrLf _
        & "  <propstat>" & vbCrLf _
        & "   <prop>" & vbCrLf _
        & "    <add-member/>" & vbCrLf _
        & "    <C:allowed-sharing-modes/>" & vbCrLf _
        & "    <M:bulk-requests/>" & vbCrLf _
        & "    <A:calendar-color/>" & vbCrLf _
        & "    <C1:calendar-description/>" & vbCrLf _
        & "    <A:calendar-order/>" & vbCrLf _
        & "    <C1:calendar-timezone/>" & vbCrLf _
        & "    <C2:max-image-size/>" & vbCrLf _
        & "    <C2:max-resource-size/>" & vbCrLf _
        & "    <C:me-card/>" & vbCrLf _
        & "    <C1:default-alarm-vevent-date/>" & vbCrLf _
        & "    <C1:default-alarm-vevent-datetime/>" & vbCrLf _
        & "    <C:pre-publish-url/>" & vbCrLf _
        & "    <C:publish-url/>" & vbCrLf _
        & "    <C:push-transports/>" & vbCrLf _
        & "    <C:pushkey/>" & vbCrLf _
        & "    <quota-avaiable-bytes/>" & vbCrLf _
        & "    <quota-used-bytes/>" & vbCrLf _
        & "    <A:refreshrate/>" & vbCrLf _
        & "    <C1:schedule-calendar-transp/>" & vbCrLf _
        & "    <C:source/>" & vbCrLf _
        & "    <C:subscribed-strip-alarms/>" & vbCrLf _
        & "    <C:subscribed-strip-attachments/>" & vbCrLf _
        & "    <C:subscribed-strip-todos/>" & vbCrLf _
        & "    <C1:supported-calendar-component-set/>" & vbCrLf _
        & "    <C1:supported-calendar-component-sets/>" & vbCrLf _
        & "    <sync-token/>" & vbCrLf _
        & "    <C:xmpp-server/>" & vbCrLf _
        & "    <C:xmpp-uri/>" & vbCrLf _
        & "   </prop>" & vbCrLf _
        & "   <status>HTTP/1.1 404 Not Found</status>" & vbCrLf _
        & "  </propstat>" & vbCrLf _
        & " </response>" & vbCrLf _
        & " <response>" & vbCrLf _
        & "  <href>{2}</href>" & vbCrLf _
        & "  <propstat>" & vbCrLf _
        & "   <prop>" & vbCrLf _
        & "    <A:calendar-color>#F64F00</A:calendar-color>" & vbCrLf _
        & "    <C1:calendar-free-busy-set>" & vbCrLf _
        & "     <href>{2}</href>" & vbCrLf _
        & "    </C1:calendar-free-busy-set>" & vbCrLf _
        & "    <A:calendar-order>0</A:calendar-order>" & vbCrLf _
        & "    <current-user-privilege-set>" & vbCrLf _
        & "     <privilege>" & vbCrLf _
        & "      <all/>" & vbCrLf _
        & "     </privilege>" & vbCrLf _
        & "     <privilege>" & vbCrLf _
        & "      <read/>" & vbCrLf _
        & "     </privilege>" & vbCrLf _
        & "     <privilege>" & vbCrLf _
        & "      <unlock/>" & vbCrLf _
        & "     </privilege>" & vbCrLf _
        & "     <privilege>" & vbCrLf _
        & "      <read-acl/>" & vbCrLf _
        & "     </privilege>" & vbCrLf _
        & "     <privilege>" & vbCrLf _
        & "      <read-current-user-privilege-set/>" & vbCrLf _
        & "     </privilege>" & vbCrLf _
        & "     <privilege>" & vbCrLf _
        & "      <write-acl/>" & vbCrLf _
        & "     </privilege>" & vbCrLf _
        & "     <privilege>" & vbCrLf _
        & "      <C1:read-free-busy/>" & vbCrLf _
        & "     </privilege>" & vbCrLf _
        & "     <privilege>" & vbCrLf _
        & "      <write/>" & vbCrLf _
        & "     </privilege>" & vbCrLf _
        & "     <privilege>" & vbCrLf _
        & "      <write-properties/>" & vbCrLf _
        & "     </privilege>" & vbCrLf _
        & "     <privilege>" & vbCrLf _
        & "      <write-content/>" & vbCrLf _
        & "     </privilege>" & vbCrLf _
        & "     <privilege>" & vbCrLf _
        & "      <bind/>" & vbCrLf _
        & "     </privilege>" & vbCrLf _
        & "     <privilege>" & vbCrLf _
        & "      <unbind/>" & vbCrLf _
        & "     </privilege>" & vbCrLf _
        & "    </current-user-privilege-set>" & vbCrLf _
        & "    <displayname>{3}</displayname>" & vbCrLf _
        & "    <C:getctag>{5}</C:getctag>" & vbCrLf _
        & "    <owner>" & vbCrLf _
        & "     <href>{1}</href>" & vbCrLf _
        & "    </owner>" & vbCrLf _
        & "    <resource-id>" & vbCrLf _
        & "     <href>{0}.resources/1038</href>" & vbCrLf _
        & "    </resource-id>" & vbCrLf _
        & "    <resourcetype>" & vbCrLf _
        & "     <collection/>" & vbCrLf _
        & "     <C1:calendar/>" & vbCrLf _
        & "    </resourcetype>" & vbCrLf _
        & "    <C1:schedule-default-calendar-URL>" & vbCrLf _
        & "     <href>{4}</href>" & vbCrLf _
        & "    </C1:schedule-default-calendar-URL>" & vbCrLf _
        & "    <C1:supported-calendar-component-set>" & vbCrLf _
        & "     <C1:comp name=""VEVENT""/>" & vbCrLf _
        & "    </C1:supported-calendar-component-set>" & vbCrLf _
        & "    <supported-report-set>" & vbCrLf _
        & "     <supported-report>" & vbCrLf _
        & "      <report>" & vbCrLf _
        & "       <principal-property-search/>" & vbCrLf _
        & "      </report>" & vbCrLf _
        & "     </supported-report>" & vbCrLf _
        & "     <supported-report>" & vbCrLf _
        & "      <report>" & vbCrLf _
        & "       <principal-search-property-set/>" & vbCrLf _
        & "      </report>" & vbCrLf _
        & "     </supported-report>" & vbCrLf _
        & "     <supported-report>" & vbCrLf _
        & "      <report>" & vbCrLf _
        & "       <expand-property/>" & vbCrLf _
        & "      </report>" & vbCrLf _
        & "     </supported-report>" & vbCrLf _
        & "     <supported-report>" & vbCrLf _
        & "      <report>" & vbCrLf _
        & "       <sync-collection/>" & vbCrLf _
        & "      </report>" & vbCrLf _
        & "     </supported-report>" & vbCrLf _
        & "     <supported-report>" & vbCrLf _
        & "      <report>" & vbCrLf _
        & "       <C1:calendar-query/>" & vbCrLf _
        & "      </report>" & vbCrLf _
        & "     </supported-report>" & vbCrLf _
        & "     <supported-report>" & vbCrLf _
        & "      <report>" & vbCrLf _
        & "       <C1:calendar-multiget/>" & vbCrLf _
        & "      </report>" & vbCrLf _
        & "     </supported-report>" & vbCrLf _
        & "     <supported-report>" & vbCrLf _
        & "      <report>" & vbCrLf _
        & "       <C1:free-busy-query/>" & vbCrLf _
        & "      </report>" & vbCrLf _
        & "     </supported-report>" & vbCrLf _
        & "    </supported-report-set>" & vbCrLf _
        & "   </prop>" & vbCrLf _
        & "   <status>HTTP/1.1 200 OK</status>" & vbCrLf _
        & "  </propstat>" & vbCrLf _
        & "  <propstat>" & vbCrLf _
        & "   <prop>" & vbCrLf _
        & "    <add-member/>" & vbCrLf _
        & "    <C:allowed-sharing-modes/>" & vbCrLf _
        & "    <M:bulk-requests/>" & vbCrLf _
        & "    <C1:calendar-description/>" & vbCrLf _
        & "    <C1:calendar-timezone/>" & vbCrLf _
        & "    <C1:comp name=""VTODO""/>" & vbCrLf _
        & "    <C2:max-image-size/>" & vbCrLf _
        & "    <C2:max-resource-size/>" & vbCrLf _
        & "    <C:me-card/>" & vbCrLf _
        & "    <C:publish-url/>" & vbCrLf _
        & "    <C:push-transports/>" & vbCrLf _
        & "    <C:pushkey/>" & vbCrLf _
        & "    <quota-available-bytes/>" & vbCrLf _
        & "    <quota-used-bytes/>" & vbCrLf _
        & "    <A:refreshrate/>" & vbCrLf _
        & "    <C1:schedule-calendar-transp/>" & vbCrLf _
        & "    <C:source/>" & vbCrLf _
        & "    <C:subscribed-strip-alarms/>" & vbCrLf _
        & "    <C:subscribed-strip-attachments/>" & vbCrLf _
        & "    <C:subscribed-strip-todos/>" & vbCrLf _
        & "    <sync-token/>" & vbCrLf _
        & "    <C:xmpp-server/>" & vbCrLf _
        & "    <C:xmpp-uri/>" & vbCrLf _
        & "   </prop>" & vbCrLf _
        & "   <status>HTTP/1.1 404 Not Found</status>" & vbCrLf _
        & "  </propstat>" & vbCrLf
        '& " </response>" & vbCrLf _
        '& "</multistatus>" & vbCrLf _

        '& "     <C1:comp name=""VEVENT""/>" & vbCrLf _
        '& "     <C1:comp name=""VTODO""/>" & vbCrLf _
        '& "     <C1:comp name=""VJOURNAL""/>" & vbCrLf _
        '& "     <C1:comp name=""VTIMEZONE""/>" & vbCrLf _
        '& "     <C1:comp name=""VFREEBUSY""/>" & vbCrLf _
        ' 2012/10/23 SKFC 浦野【iOS6対応】未知のタグに対応 END

        Private Sub New()
            'dummy
        End Sub
    End Class

End Namespace