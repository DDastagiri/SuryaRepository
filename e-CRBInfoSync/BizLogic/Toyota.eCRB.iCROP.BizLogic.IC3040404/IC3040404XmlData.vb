Imports Toyota.eCRB.SystemFrameworks.Core

Namespace IC3040404.BizLogic

    Class XmlData

        ' 2012/10/4 SKFC 浦野【iOS6対応】未知のタグに対応 START
        ' 以下の4つのタグの応答を追加
        'default-alarm-vevent-date
        'default-alarm-vevent-datetime 
        'pre-publish-url
        'supported-calendar-component-sets

        ''' <summary>
        ''' XmlData.ResTableのラッパ関数
        ''' </summary>
        ''' <param name="key"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' 新規関数：辞書引きをTryCatchし存在しない場合、"404::"　を返す
        ''' </remarks>
        ''' <History>
        ''' 2012/10/3 SKFC 浦野【ios6対応】未知のタグに対応
        ''' </History>
        Public Shared Function GetResTable(ByVal key As String) As String
            Dim value As String = ""

            If Not ResTable.TryGetValue(key, value) Then
                value = "404::"
                Logger.Warn("[IC3040404XmlData:GetResTable] UndefinedTag  key:" & key)
            End If

            Return value

        End Function

        ''' <summary>
        ''' VCALENDARのデータ   2012/12/12
        ''' </summary>
        ''' <remarks>
        ''' moduleをclassに変更
        ''' </remarks>
        Public Shared ResVcalendar As Dictionary(Of String, String) = XmlDefine(Of String, String).Build() _
            .Response("<C:calendar-data>BEGIN:VCALENDAR", "99::") _
            .Response("PRODID", "100::-//TMC//TMC Calendar 1.00//JP") _
            .Response("X-WR-CALNAME", "98::{0}") _
            .Response("VERSION", "100::2.0") _
            .Response("BEGIN:VTIMEZONE", "0::") _
            .Response("TZID", "102::{0}") _
            .Response("X-LIC-LOCATION", "102::{0}") _
            .Response("BEGIN:STANDARD", "0::") _
            .Response("TZOFFSETFROM", "103::{0}") _
            .Response("TZOFFSETTO", "104::{0}") _
            .Response("TZNAME", "105::{0}") _
            .Response("DTSTART", "106::{0}") _
            .Response("END:STANDARD", "0::") _
            .Response("END:VTIMEZONE", "0::")


        '.Response("calendar-user-address-set", "2002:C1:<href>{0}</href>" & vbCrLf & "<href>{1}</href>") _
        '.Response("schedule-inbox-URL", "2101:C1:<href>{0}.in/</href>") _
        '.Response("schedule-outbox-URL", "2101:C1:<href>{0}.out/</href>") _

        ''' <summary>
        ''' レスポンスの変換テーブル
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ResTable As Dictionary(Of String, String) = XmlDefine(Of String, String).Build() _
            .Response("current-user-principal", "2101::<href>{0}</href>") _
            .Response("principal-URL", "2101::<href>{0}</href>") _
            .Response("resourcetype", "2000::<collection/>" & vbCrLf & "<C:calendar/>") _
            .Response("allowed-calendar-component-set", "404:C:") _
            .Response("calendar-home-set", "2101:C1:<href>{0}</href>") _
            .Response("calendar-user-address-set", "404:C:") _
 _
            .Response("displayname", "2103::{0}") _
            .Response("dropbox-home-URL", "2101:C:<href>{0}.drop/</href>") _
            .Response("email-address-set", "404:C:") _
            .Response("notification-URL", "404:C:") _
            .Response("principal-collection-set", "2100::<href>{0}</href>") _
            .Response("resource-id", "2100::<href>{0}.resources/1039</href>") _
            .Response("schedule-inbox-URL", "2101:C1:<href>{0}.in/</href>") _
            .Response("schedule-outbox-URL", "2101:C1:<href>{0}.out/</href>") _
            .Response("supported-report-set", "2104::     <supported-report>" & vbCrLf _
                                            & "     <href>{0}</href>" & vbCrLf _
                                            & "     <report>" & vbCrLf _
                                            & "      <principal-property-search/>" & vbCrLf _
                                            & "     </report>" & vbCrLf _
                                            & "     </supported-report>" & vbCrLf _
                                            & "     <supported-report>" & vbCrLf _
                                            & "     <report>" & vbCrLf _
                                            & "       <principal-search-property-set/>" & vbCrLf _
                                            & "     </report>" & vbCrLf _
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
                                            & "   </report>" & vbCrLf _
                                            & " </supported-report>") _
 _
            .Response("add-member", "2010::") _
            .Response("allowed-sharing-modes", "404:C:") _
            .Response("bulk-requests", "404:M:") _
            .Response("calendar-color", "404:A:") _
            .Response("calendar-description", "404:C1:") _
            .Response("calendar-free-busy-set", "2102:C1:<href>{0}</href>") _
            .Response("calendar-order", "2000:A:0") _
            .Response("calendar-timezone", "404:C1:") _
 _
            .Response("current-user-privilege-set", "2000:: <privilege>" & vbCrLf _
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
                                         & "     </privilege>") _
            .Response("getctag", "2004:C:{0}") _
            .Response("max-image-size", "404:C2:") _
            .Response("max-resource-size", "404:C2:") _
            .Response("me-card", "404:C:") _
            .Response("owner", "2101::<href>{0}</href>") _
            .Response("default-alarm-vevent-date", "404::") _
            .Response("default-alarm-vevent-datetime", "404::") _
            .Response("pre-publish-url", "404:C:") _
            .Response("publish-url", "404:C:") _
            .Response("push-transports", "404:C:") _
            .Response("pushkey", "404:C:") _
            .Response("quota-available-bytes", "404::") _
            .Response("quota-used-bytes", "404::") _
            .Response("refreshrate", "404:A:") _
            .Response("schedule-calendar-transp", "404:C1:") _
            .Response("schedule-default-calendar-URL", "2104:C1:<href>{0}</href>") _
            .Response("source", "404:C:") _
            .Response("subscribed-strip-alarms", "404:C:") _
            .Response("subscribed-strip-attachments", "404:C:") _
            .Response("subscribed-strip-todos", "404:C:") _
            .Response("supported-calendar-component-set", "2000::<C1:comp name=""VEVENT""/>") _
            .Response("supported-calendar-component-sets", "404::") _
            .Response("sync-token", "404::") _
            .Response("xmpp-server", "404:C:") _
            .Response("xmpp-uri", "404:C:")


        '.Response("supported-calendar-component-set", "2000::<C1:comp name=""VEVENT""/>" & vbCrLf _
        '                                & "<C1:comp name=""VTODO""/>" & vbCrLf _
        '                                & "<C1:comp name=""VJOURNAL""/>" & vbCrLf _
        '                                & "<C1:comp name=""VTIMEZONE""/>" & vbCrLf _
        '                                & "<C1:comp name=""VFREEBUSY""/>") _


        ''' <summary>
        ''' PROPFINDのrootディレクトリのレスポンス
        ''' </summary>
        ''' <remarks>
        ''' tagのadd-memberをtriggerにする
        ''' 2011/12/4 機能追加
        ''' </remarks>
        Public Shared ResTableRoot As Dictionary(Of String, String) = XmlDefine(Of String, String).Build() _
        .Response("propstat", "2010::  <C1:calendar-free-busy-set> " & vbCrLf _
   & "       <href>{1}</href>" & vbCrLf _
   & "      </C1:calendar-free-busy-set>" & vbCrLf _
   & "       <href>{2}</href>" & vbCrLf _
   & "      <current-user-privilege-set>" & vbCrLf _
   & "       <privilege>" & vbCrLf _
   & "        <all/>" & vbCrLf _
   & "       </privilege>" & vbCrLf _
   & "       <privilege>" & vbCrLf _
   & "        <read/>" & vbCrLf _
   & "       </privilege>" & vbCrLf _
   & "       <privilege>" & vbCrLf _
   & "        <unlock/>" & vbCrLf _
   & "       </privilege>" & vbCrLf _
   & "       <privilege>" & vbCrLf _
   & "        <read-acl/>" & vbCrLf _
   & "       </privilege>" & vbCrLf _
   & "       <privilege>" & vbCrLf _
   & "        <read-current-user-privilege-set/>" & vbCrLf _
   & "       </privilege>" & vbCrLf _
   & "       <privilege>" & vbCrLf _
   & "        <write-acl/>" & vbCrLf _
   & "       </privilege>" & vbCrLf _
   & "       <privilege>" & vbCrLf _
   & "        <write/>" & vbCrLf _
   & "       </privilege>" & vbCrLf _
   & "       <privilege>" & vbCrLf _
   & "        <write-properties/>" & vbCrLf _
   & "       </privilege>" & vbCrLf _
   & "       <privilege>" & vbCrLf _
   & "        <write-content/>" & vbCrLf _
   & "       </privilege>" & vbCrLf _
   & "       <privilege>" & vbCrLf _
   & "        <bind/>" & vbCrLf _
   & "       </privilege>" & vbCrLf _
   & "       <privilege>" & vbCrLf _
   & "        <unbind/>" & vbCrLf _
   & "       </privilege>" & vbCrLf _
   & "      </current-user-privilege-set>" & vbCrLf _
   & "      <displayname>{3}</displayname>" & vbCrLf _
   & "      <C:getctag>""df9e0b95470f1fdc545b494b1d588a4e""</C:getctag>" & vbCrLf _
   & "      <owner>" & vbCrLf _
   & "       <href>{1}</href>" & vbCrLf _
   & "      </owner>" & vbCrLf _
   & "      <resource-id>" & vbCrLf _
   & "       <href>{0}.resources/1038</href>" & vbCrLf _
   & "      </resource-id>" & vbCrLf _
   & "      <resourcetype>" & vbCrLf _
   & "       <collection/>" & vbCrLf _
   & "       <principal/>" & vbCrLf _
   & "      </resourcetype>" & vbCrLf _
   & "      <C1:schedule-default-calendar-URL>" & vbCrLf _
   & "       <href>{4}</href>" & vbCrLf _
   & "      </C1:schedule-default-calendar-URL>" & vbCrLf _
   & "      <supported-report-set>" & vbCrLf _
   & "       <supported-report>" & vbCrLf _
   & "        <report>" & vbCrLf _
   & "         <principal-property-search/>" & vbCrLf _
   & "        </report>" & vbCrLf _
   & "       </supported-report>" & vbCrLf _
   & "       <supported-report>" & vbCrLf _
   & "        <report>" & vbCrLf _
   & "         <principal-search-property-set/>" & vbCrLf _
   & "        </report>" & vbCrLf _
   & "       </supported-report>" & vbCrLf _
   & "       <supported-report>" & vbCrLf _
   & "        <report>" & vbCrLf _
   & "         <expand-property/>" & vbCrLf _
   & "        </report>" & vbCrLf _
   & "       </supported-report>" & vbCrLf _
   & "       <supported-report>" & vbCrLf _
   & "        <report>" & vbCrLf _
   & "         <sync-collection/>" & vbCrLf _
   & "        </report>" & vbCrLf _
   & "       </supported-report>" & vbCrLf _
   & "      </supported-report-set>" & vbCrLf _
   & "     </prop>" & vbCrLf _
   & "     <status>HTTP/1.1 200 OK</status>" & vbCrLf _
   & "    </propstat>" & vbCrLf _
   & "    <propstat>" & vbCrLf _
   & "     <prop>" & vbCrLf _
   & "      <add-member/>" & vbCrLf _
   & "      <C:allowed-sharing-modes/>" & vbCrLf _
   & "      <M:bulk-requests/>" & vbCrLf _
   & "      <A:calendar-color/>" & vbCrLf _
   & "      <C1:calendar-description/>" & vbCrLf _
   & "      <A:calendar-order/>" & vbCrLf _
   & "      <C1:calendar-timezone/>" & vbCrLf _
   & "      <C2:max-image-size/>" & vbCrLf _
   & "      <C2:max-resource-size/>" & vbCrLf _
   & "      <C:me-card/>" & vbCrLf _
   & "      <C1:default-alarm-vevent-date/>" & vbCrLf _
   & "      <C1:default-alarm-vevent-datetime/>" & vbCrLf _
   & "      <C:pre-publish-url/>" & vbCrLf _
   & "      <C:publish-url/>" & vbCrLf _
   & "      <C:push-transports/>" & vbCrLf _
   & "      <C:pushkey/>" & vbCrLf _
   & "      <quota-available-bytes/>" & vbCrLf _
   & "      <quota-used-bytes/>" & vbCrLf _
   & "      <A:refreshrate/>" & vbCrLf _
   & "      <C1:schedule-calendar-transp/>" & vbCrLf _
   & "      <C:source/>" & vbCrLf _
   & "      <C:subscribed-strip-alarms/>" & vbCrLf _
   & "      <C:subscribed-strip-attachments/>" & vbCrLf _
   & "      <C:subscribed-strip-todos/>" & vbCrLf _
   & "      <C1:supported-calendar-component-set/>" & vbCrLf _
   & "      <C1:supported-calendar-component-sets/>" & vbCrLf _
   & "      <sync-token/>" & vbCrLf _
   & "      <C:xmpp-server/>" & vbCrLf _
   & "      <C:xmpp-uri/>" & vbCrLf _
   & "     </prop>" & vbCrLf _
   & "     <status>HTTP/1.1 404 Not Found</status>" & vbCrLf _
   & "    </propstat>" & vbCrLf _
   & "   </response>" & vbCrLf _
   & "   <response>" & vbCrLf _
   & "   <href>{2}</href>" & vbCrLf _
   & "<propstat>" & vbCrLf _
   & "<prop>")

        ' 2012/10/3 SKFC 浦野【iOS6対応】未知のタグに対応 END

        ''' <summary>
        ''' デフォルトコンストラクタ
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub New()
            'Dummy
        End Sub

    End Class

End Namespace
