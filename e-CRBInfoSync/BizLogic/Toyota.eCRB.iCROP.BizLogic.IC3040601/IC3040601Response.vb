Imports System.Web
Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Xml
Imports System.Text
Imports Toyota.eCRB.iCROP.DataAccess.IC3040601

Namespace IC3040601.BizLogic

    ''' <summary>
    ''' レスポンス処理
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Response

        Private _Response As HttpResponse
        Private _Request As HttpRequest
        Private _HeaderInfo As RequestInfo
        Private _Global As GlobalValue
        Private _strMethod As String
        Private _xml As XmlDocument
        Private _nPropfindNo As Integer = 0   'PropFindメソッド連文番号
        Private _strxmlName(100) As String    'xml Name文字列
        Private _nxmlNameCount As Integer = 0 'xml Name数

        Private _DLRCD As String = ""
        Private _STRCD As String = ""

        Private Const CrLF As String = vbCrLf '改行文字
        Private Const VCardDataArrayMaxCounts As Integer = 1000 '送信VCardData 最大数

        '行区切りの文字列
        Private Const separator As String = vbCrLf

        Private Const STATUS_OK As Integer = 200
        Private Const STATUS_NG As Integer = 404

        Property NPropfindNo As Integer
            Get
                Return _nPropfindNo
            End Get
            Set(ByVal value As Integer)
                _nPropfindNo = value
            End Set
        End Property

        Property StrxmlName(ByVal index As Integer) As String
            Get
                Return _strxmlName(index)
            End Get
            Set(ByVal value As String)
                _strxmlName(index) = value
            End Set
        End Property

        Property NxmlNameCount As Integer
            Get
                Return _nxmlNameCount
            End Get
            Set(ByVal value As Integer)
                _nxmlNameCount = value
            End Set
        End Property

        Property Dlrcd As String
            Get
                Return _DLRCD
            End Get
            Set(ByVal value As String)
                _DLRCD = value
            End Set
        End Property

        Property Strcd As String
            Get
                Return _STRCD
            End Get
            Set(ByVal value As String)
                _STRCD = value
            End Set
        End Property


        ''' <summary>
        ''' Setter Getter
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property StrMethod As String
            Get
                Return _strMethod
            End Get
            Set(ByVal value As String)
                _strMethod = value
            End Set
        End Property

        ''' <summary>
        ''' コンストラクタ
        ''' </summary>
        ''' <param name="Response"></param>
        ''' <param name="Request"></param>
        ''' <remarks></remarks>
        Sub New(ByRef response As HttpResponse, ByRef request As HttpRequest, _
                ByVal clsHeaderInfo As RequestInfo, ByVal clsGlobal As GlobalValue, ByVal xml As XmlDocument)
            Logger.Debug("[IC3040601Response:New(constructor)] Start")

            _Response = response
            _Request = request
            _HeaderInfo = clsHeaderInfo
            _Global = clsGlobal
            _xml = xml      '事前に読み込んでおく（_Requestから読めないので)
            _strMethod = request.HttpMethod.ToString     'ヘッダのリクエスト

            Logger.Debug(" [IC3040601Response:New(constructor)] exit")
        End Sub

        ''' <summary>
        ''' ひとつのエレメントを作成
        ''' </summary>
        ''' <param name="res">レスポンス文字列</param>
        ''' <param name="sSpace">名前空間</param>
        ''' <param name="elm">エレメント名</param>
        ''' <param name="NotFound">TrueのときNotFound用のデータを作成する。
        ''' 　　　　この場合、 <tagName/> だけのデータとなる。既定値はFalse
        ''' </param>
        ''' <returns>作成したエレメント情報の文字列</returns>
        ''' <remarks></remarks>
        Function MakeOneElement(ByVal res As String, ByVal sSpace As String, ByVal elm As String, Optional ByVal notFound As Boolean = False) As String
            Dim out As New StringBuilder
            out.Length = 0

            '
            Dim element As String
            If String.IsNullOrEmpty(sSpace) Then
                element = elm
            Else
                element = sSpace & ":" & elm
            End If

            If notFound Then
                out.Append("<" & element & "/>" & separator)
            Else
                out.Append("<" & element & ">" & separator)
                out.Append(res & separator)     'Separatorは vbCrLf
                out.Append("</" & element & ">" & separator)
            End If

            Return out.ToString
        End Function

        ''' <summary>
        ''' xmlツリー
        ''' </summary>
        Private Sub getXmlTree(ByVal xnode As XmlNode)
            If Not IsNothing(xnode) Then xmlFormat(xnode)

            If xnode.HasChildNodes Then
                xnode = xnode.FirstChild
                While Not IsNothing(xnode)
                    getXmlTree(xnode)
                    xnode = xnode.NextSibling
                End While
            End If
        End Sub

        'xml Format the output.
        Private Sub xmlFormat(ByVal xnode As XmlNode)
            If Not xnode.HasChildNodes Then
                Logger.Debug(Strings.Chr(9) & xnode.Name & "<" & xnode.Value & ">")
                Debug.Print(Strings.Chr(9) & xnode.Name & "<" & xnode.Value & ">")
                If NxmlNameCount < 100 Then
                    StrxmlName(NxmlNameCount) = xnode.Name     'xml Name文字列
                    NxmlNameCount = NxmlNameCount + 1          'xml Name数
                End If
            Else
                'xnode.GetNamespaceOfPrefix()
                'xnode.GetPrefixOfNamespace()
                Debug.Print("BaseURI:" & xnode.BaseURI)
                Debug.Print("InnerXml:" & xnode.InnerXml)
                Debug.Print("InnerText:" & xnode.InnerText)

                Logger.Debug(xnode.Name)
                Debug.Print(xnode.Name)
                If NxmlNameCount < 100 Then
                    StrxmlName(NxmlNameCount) = xnode.Name     'xml Name文字列
                    NxmlNameCount = NxmlNameCount + 1          'xml Name数
                End If
                If XmlNodeType.Element = xnode.NodeType Then
                    Dim map As XmlNamedNodeMap = xnode.Attributes
                    Dim attrnode As XmlNode
                    For Each attrnode In map
                        Logger.Debug(" " & attrnode.Name & "<" & attrnode.Value & "> ")
                        Debug.Print(" " & attrnode.Name & "<" & attrnode.Value & "> ")
                        If NxmlNameCount < 100 Then
                            StrxmlName(NxmlNameCount) = attrnode.Name       'xml Name文字列
                            NxmlNameCount = NxmlNameCount + 1                               'xml Name数
                        End If
                    Next
                End If
                Logger.Debug("")
                Debug.Print("")
            End If
        End Sub

        ''' <summary>
        ''' PROPFINDメソッド
        ''' </summary>
        ''' <remarks>
        ''' このメソッドが呼ばれるときは、認証が終了している
        ''' </remarks>
        Sub ResPropFind()
            Logger.Info("[IC3040601Response:ResPropFind] Start  User:" & _HeaderInfo.GetUser)

            WriteBasicHeader(_Response)

            'XmlNodeクラスでノードを操作する（表示）
            Dim xmlnode As XmlNode
            xmlnode = _xml.DocumentElement
            NxmlNameCount = 0    'xml Name数クリア
            getXmlTree(xmlnode)

            'xml出力
            Dim rootpath As String = _Global.CardDavRootUrl
            Dim xml_root As XmlElement = _xml.DocumentElement
            If IsNothing(xml_root) Then
                'Request Bodyがない場合エラーを返す
                _Response.StatusCode = GlobalConst.HTTP_STAT_422 'Unprocessible Entity
            Else
                '正常時　MultiStatusを返す                MakeXmlVcard("skfcs", "skf")

                'xmlを出力
                Dim XmlData As New StringBuilder '200 OKデータ格納場所
                XmlData.Length = 0

                Debug.Print("ContentLength:" & _Request.ContentLength)
                Debug.Print("ContentType:" & _Request.ContentType)
                Debug.Print("ContentEncoding:" & _Request.ContentEncoding.ToString)
                Logger.Debug("ContentLength:" & _Request.ContentLength)
                Logger.Debug("ContentType:" & _Request.ContentType)
                Logger.Debug("ContentEncoding:" & _Request.ContentEncoding.ToString)

                '空行を出力
                XmlData.Append("")
                If _Request.ContentLength = 121 Then
                    'PROPFIND ６回目のレスポンス(getetag)  getetagの問い合わせ
                    Dim CARDIDArray(VCardDataArrayMaxCounts) As String
                    Dim CARDIDSetCount As Integer = 0
                    Dim CARDDateArray(VCardDataArrayMaxCounts) As DateTime

                    ' 販売店コードと店舗コードからデータ数（複数）のCARDIDデータを取得する
                    CARDIDSetCount = GetCardId(CARDIDArray, CARDDateArray, Dlrcd, Strcd)
                    'ヘッダに追加
                    _Response.Headers.Add("ETag", MakeEtag())

                    XmlData.Append(String.Format(Globalization.CultureInfo.CurrentCulture(), GlobalConst.HTTP_RES_PROPFIND6, rootpath))
                    For i = 0 To CARDIDSetCount - 1
                        XmlData.Append(String.Format(Globalization.CultureInfo.CurrentCulture(), GlobalConst.HTTP_RES_PROPFIND6_vcf_s, rootpath))
                        XmlData.Append(CARDIDArray(i))
                        XmlData.Append(GlobalConst.HTTP_RES_PROPFIND6_vcf_e)
                        XmlData.Append(GlobalConst.HTTP_RES_PROPFIND6_etag_s)
                        XmlData.Append(MakeFileEtag(CARDIDArray(i), CARDDateArray(i)))
                        XmlData.Append(GlobalConst.HTTP_RES_PROPFIND6_etag_e)
                    Next
                    XmlData.Append(GlobalConst.HTTP_RES_PROPFIND6_END)
                ElseIf _Request.ContentLength < 180 Then
                    'PROPFIND ３回目のレスポンス:addressbook-home-setの問い合わせ
                    XmlData.Append(String.Format(Globalization.CultureInfo.CurrentCulture(), GlobalConst.HTTP_RES_PROPFIND3, rootpath))
                ElseIf _Request.ContentLength = 181 And NxmlNameCount <= 5 Then
                    'PROPFIND ５回目のレスポンス(getctag)  ctagの問い合わせ
                    XmlData.Append(String.Format(Globalization.CultureInfo.CurrentCulture(), GlobalConst.HTTP_RES_PROPFIND5, rootpath))
                    XmlData.Append(MakeCtag())
                    XmlData.Append(GlobalConst.HTTP_RES_PROPFIND5_2)
                ElseIf _Request.ContentLength < 190 Then
                    'PROPFIND １回目のレスポンス:URLの問い合わせ
                    XmlData.Append(String.Format(Globalization.CultureInfo.CurrentCulture(), GlobalConst.HTTP_RES_PROPFIND1, rootpath))
                ElseIf _Request.ContentLength < 500 Then
                    'PROPFIND ２回目のレスポンス:セット（set） URLの問い合わせ
                    XmlData.Append(String.Format(Globalization.CultureInfo.CurrentCulture(), GlobalConst.HTTP_RES_PROPFIND2, rootpath))
                Else
                    'PROPFIND ４回目のレスポンス:特権セット（set）の問い合わせ
                    XmlData.Append(String.Format(Globalization.CultureInfo.CurrentCulture(), GlobalConst.HTTP_RES_PROPFIND4, rootpath))
                End If
                'xmlを出力
                OutRespXml(XmlData.ToString)

                _Response.StatusCode = GlobalConst.HTTP_STAT_207 'MultiStatus
            End If

            Logger.Info(" [IC3040601Response:ResPropFind] Exit")
        End Sub

        ''' <summary>
        ''' CTAGを生成し返す
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function MakeCtag(Optional ByVal optPath As String = "") As String

            Dim path As String = "/" & _HeaderInfo.GetUser & "/" & optPath
            Dim d As Date = Nothing
            'ファイルからctagの日付を得る
            d = _HeaderInfo.GetCtagDate()

            '日付型を書式で編集
            Dim dat As String = Format(d, "yyyy/MM/dd HH:mm:ss")

            Dim md5 As String = CreateMd5(path, dat)

            Logger.Debug(" MakeCtag:path=" & path & ":date=" & d & ":MD5=" & md5)

            Return md5

        End Function


        ''' <summary>
        ''' Etagを作成
        ''' </summary>
        ''' <param name="kind"></param>
        ''' <returns></returns>
        ''' <remarks>
        '''デフォルトはカレンダー用のEtag
        ''' 引数kindを指定すると、そのTagになる
        ''' 作成されるTagは　/200005@42A10/calendar/ と　日付から
        ''' 生成するMD5データとなる
        ''' </remarks>
        Function MakeEtag(Optional ByVal kind As String = "calendar/") As String

            '作成方法はctagと同じ
            Dim md5 As String = MakeCtag(kind)

            Return md5

        End Function

        ''' <summary>
        ''' FileEtagを作成
        ''' </summary>
        ''' <param name="cardid">カードID</param>
        ''' <param name="carddate">カードIDのデータUPDATE日付</param>
        ''' <returns></returns>
        ''' <remarks>
        '''デフォルトはカレンダー用のFileEtag
        ''' 引数kindを指定すると、そのTagになる
        ''' 作成されるTagは　cardidと日付(carddate)から
        ''' 生成するMD5データとなる
        ''' </remarks>
        Function MakeFileEtag(ByVal cardid As String, ByVal carddate As DateTime) As String
            '日付型を書式で編集
            Dim dat As String = Format(carddate, "yyyy/MM/dd HH:mm:ss")

            '作成方法はctagと同じ
            Dim md5 As String = CreateMd5(cardid, dat)

            Logger.Debug(" MakeFileEtag:cardid=" & cardid & ":date=" & carddate & ":MD5=" & md5)

            Return md5

        End Function

        ''' <summary>
        ''' Bodyを出力
        ''' </summary>
        ''' <param name="str"></param>
        ''' <remarks>
        ''' string builderで作成した文字列をそのまま
        ''' 出力すると改行が消えるのでこの関数を実装
        ''' </remarks>
        Private Sub OutRespXml(ByVal str As String)

            Dim work() As String = str.Split(separator)

            For Each s As String In work
                WriteExec(s, "")
            Next
            Logger.Debug("RESPONSE:" & str)

        End Sub


        ''' <summary>
        ''' レスポンスのボディを返す
        ''' </summary>
        ''' <param name="str"></param>
        ''' <param name="lf">既定値改行=vbLf オプション=""で無改行</param>
        ''' <remarks></remarks>
        Sub WriteExec(ByVal str As String, Optional ByVal lf As String = vbLf)
            _Response.Write(str & lf)
        End Sub


        ''' <summary>
        ''' 基本ヘッダを出力
        ''' </summary>
        ''' <remarks>
        ''' 全体の処理で普遍のヘッダをレスポンス出力
        ''' </remarks>
        Public Sub WriteBasicHeader(ByRef response As HttpResponse)

            response.ClearHeaders()
            response.Charset = "Content-Type"
            response.AppendHeader("DAV", GlobalConst.HEAD_DAV_01)
            response.AppendHeader("Content-Type", "text/xml")
            response.ContentEncoding = System.Text.Encoding.UTF8
            response.ContentType = "text/xml"

        End Sub

        ''' <summary>
        ''' ステータスをメンバー変数に設定
        ''' </summary>
        ''' <param name="status"></param>
        ''' <remarks></remarks>
        Sub SetStatus(ByVal status As Integer)
            _Response.StatusCode = status
        End Sub

        ''' <summary>
        ''' BASIC認証を要求
        ''' </summary>
        ''' <remarks>
        ''' BASIC認証なしにHTTPメソッドが呼ばれたときにBASIC認証の要求を返す
        ''' </remarks>
        Public Sub RequestCertify()
            WriteBasicHeader(_Response)
#If Not Debug Then
            Dim sHostIP As String = _Request.ServerVariables("SERVER_NAME")  'ホストのIP取得
            _Response.Headers.Add("WWW-Authenticate", "Basic realm=" & sHostIP)
            _Response.ContentType = "text/html"
            _Response.StatusCode = GlobalConst.HTTP_STAT_401 '401 Authenticate要求
#End If

        End Sub


        ''' <summary>
        ''' オプション
        ''' </summary>
        ''' <remarks></remarks>
        Sub ResOptions()
            Logger.Info("[IC3040601Response:ResOptions] Start  User:" & _HeaderInfo.GetUser)

            WriteBasicHeader(_Response)

            _Response.Headers.Add("Allow", GlobalConst.HEAD_ALLOW)
            _Response.StatusCode = GlobalConst.HTTP_STAT_200  'OK

            Logger.Info(" [IC3040601Response:ResOptions] Exit")
        End Sub


        ''' <summary>
        ''' レポート処理
        ''' </summary>
        ''' <remarks></remarks>
        Sub ResReport()
            Logger.Info("[IC3040601Response:ResReport] Start  User:" & _HeaderInfo.GetUser)

#If Not Debug Then
            _Response.ContentEncoding = System.Text.Encoding.UTF8
#End If

            Dim root As XmlElement = _xml.DocumentElement

            If IsNothing(root) Then
                'Request Bodyがない
                _Response.StatusCode = GlobalConst.HTTP_STAT_422 'Unprocessible Entity

            Else
                _Response.Headers.Add("ETag", MakeEtag())

                'VCARDのデータを作成
                MakeXmlVcard(Dlrcd, Strcd)

                _Response.StatusCode = GlobalConst.HTTP_STAT_207 'MultiStatus
            End If

            Logger.Info(" [IC3040601Response:ResReport] Exit")
        End Sub

        ''' <summary>
        ''' xml vcardの情報を作成する
        ''' </summary>
        ''' <remarks></remarks>
        Sub MakeXmlVcard(ByVal dlrcd As String, ByVal strcd As String)
            Dim VCardDataArraySetCount As Integer = 0
            Dim VCardDataArray(VCardDataArrayMaxCounts) As String '最大データ件数
            Dim CardIDArray(VCardDataArrayMaxCounts) As String 'CARDID 最大データ件数
            Dim CardDateArray(VCardDataArrayMaxCounts) As DateTime
            Dim XmlVCardData As New StringBuilder '200 OKデータ格納場所
            XmlVCardData.Length = 0

            ' 販売店コードと店舗コードからデータ数（複数）のVCARDデータを作成する
            MakeVCard(CardIDArray, CardDateArray, VCardDataArray, VCardDataArraySetCount, dlrcd, strcd)

            ' xmlヘッダデータセット
            XmlVCardData.Append("<?xml version=""1.0"" encoding=""utf-8"" ?>" & CrLF)
            XmlVCardData.Append("<multistatus xmlns=""DAV:"" xmlns:VC=""urn:ietf:params:xml:ns:carddav"">" & CrLF)

            ' VCARDデータセット
            For i = 0 To VCardDataArraySetCount - 1
                XmlVCardData.Append(" <response>" & CrLF)
                XmlVCardData.Append("  <href>/e-CRBInfoSync/DAV/CalDAV/")
                XmlVCardData.Append(CardIDArray(i))
                XmlVCardData.Append(".vcf</href>" & CrLF)
                XmlVCardData.Append("  <propstat>" & CrLF)
                XmlVCardData.Append("   <prop>" & CrLF)
                XmlVCardData.Append("    <getetag>""")
                XmlVCardData.Append(MakeFileEtag(CardIDArray(i), CardDateArray(i)))
                XmlVCardData.Append("""</getetag>" & CrLF)
                XmlVCardData.Append("    <VC:address-data>")

                XmlVCardData.Append(VCardDataArray(i))

                XmlVCardData.Append("    </VC:address-data>" & CrLF)
                XmlVCardData.Append("   </prop>" & CrLF)
                XmlVCardData.Append("   <status>HTTP/1.1 200 OK</status>" & CrLF)
                XmlVCardData.Append("  </propstat>" & CrLF)
                XmlVCardData.Append(" </response>" & CrLF)
            Next

            'multistatus終了
            XmlVCardData.Append("</multistatus>" & CrLF)

            'xmlを出力
            OutRespXml(XmlVCardData.ToString)

        End Sub

        ''' <summary>
        ''' 販売店コードと店舗コードからデータ数（複数）のCARDIDデータを取得する
        ''' </summary>
        ''' <param name="CARDIDArray">CARDIDデータ配列（複数）のVCARDデータ</param>
        ''' <param name="CARDDateArray">CARDDateデータ配列（複数）のVCARDデータ</param>
        ''' <param name="dlrcd">販売店コード</param>
        ''' <param name="strcd">店舗コード</param>
        ''' <returns>CARDIDSetCount"作成するデータ数（複数）のVCARDデータ</returns>
        ''' <remarks>
        ''' </remarks>
        Function GetCardId(ByVal cardidArray() As String, ByVal carddateArray() As DateTime, ByVal dlrcd As String, ByVal strcd As String) As Integer
            Dim CARDIDSetCount As Integer = 0
            Using cardInfo As New DataAccess.IC3040601.IC3040601.Api.DataAccess.TblCardInfo
                Dim ret As IC3040601DataSet.TblCardInfoDataTable = _
                            cardInfo.GetSelectTable(dlrcd, strcd)
                Dim uid As String = String.Empty
                Dim newUid As String = String.Empty
                CARDIDSetCount = 0

                If Not IsNothing(ret) Then
                    'レコード1件ごとに処理を行う
                    For Each row As DataRow In ret
                        If CARDIDSetCount < VCardDataArrayMaxCounts Then '送信VCardData 最大数
                            newUid = row.Item("CARDID")
                            If uid <> newUid Then
                                cardidArray(CARDIDSetCount) = newUid
                                carddateArray(CARDIDSetCount) = row.Item("UPDATEDATE")
                                CARDIDSetCount = CARDIDSetCount + 1
                            End If
                            uid = newUid
                        End If
                    Next
                End If
            End Using
            Return CARDIDSetCount
        End Function

        ''' <summary>
        ''' 販売店コードと店舗コードからデータ数（複数）のVCARDデータを作成する
        ''' </summary>
        ''' <param name="cardidArray">CARDIDデータ配列（複数）のVCARDデータ</param>
        ''' <param name="carddateArray">CARDDateデータ配列（複数）のVCARDデータ</param>
        ''' <param name="VCardDataArray">作成するデータ配列（複数）のVCARDデータ</param>
        ''' <param name="VCardDataArraySetCount">作成するデータ数（複数）のVCARDデータ</param>
        ''' <param name="dlrcd">販売店コード</param>
        ''' <param name="strcd">店舗コード</param>
        ''' <remarks>
        ''' ・VCARDデータ内容
        ''' ----------------
        ''' BEGIN:VCARD
        ''' VERSION:3.0
        ''' PRODID:-//Apple Inc.//iOS 5.0.1//EN
        ''' 
        ''' N:%LASTNAME%;%FIRSTNAME%;;;
        ''' FN:%LASTNAME% %FIRSTNAME%
        ''' X-PHONETIC-FIRST-NAME:%FIRSTNAMEKANA%
        ''' X-PHONETIC-LAST-NAME:%LASTNAMEKANA%
        ''' ORG:%ORGANIZATION%      ※"Kobaruto;Busyo"
        ''' TITLE:%TITLE%           ※"Yaku"
        ''' URL;%URL%               ※"type=HOME:jwwww"、"type=WORK:kwwww"
        ''' NOTE:%MEMO%             ※"Memo"
        ''' 
        ''' EMAIL;%MAILTYPE%:%EMAIL% ※%MAILTYPE%="type=INTERNET;type=HOME;type=pref"、
        '''                                      "type=INTERNET;type=WORK"
        ''' item1.EMAIL;type=INTERNET:%EMAIL%
        ''' ～itemN.　Nを昇順番号にして追加する
        ''' 
        ''' TEL;%TELTYPE%:%TEL%     ※%TELTYPE% ="type=CELL;type=VOICE;type=pref"、
        '''                                     "type=IPHONE;type=CELL;type=VOICE"、
        '''                                     "type=HOME;type=VOICE"、
        '''                                     "type=WORK;type=VOICE"、
        '''                                     "type=MAIN"、
        '''                                     "type=HOME;type=FAX"、
        '''                                     "type=WORK;type=FAX"、
        '''                                     "type=OTHER;type=FAX"、
        '''                                     "type=PAGER"、
        '''                                     "type=OTHER;type=VOICE"
        ''' ～
        ''' 
        ''' item1.ADR;%ADDRESSTYPE%:※%ADDRESS%
        '''                        ※%ADDRESSTYPE% ="type=HOME;type=pref"
        '''                        ※%ADDRESS%     =";;番地(Banchi\nBan\nB123\nB456);市(City);州(Syu);郵便番号(123);"
        ''' item1.X-ABADR:%X_ABADR% ※"cn"=中国、"jp"=日本
        ''' ～itemN.　Nを昇順番号にして追加する
        ''' 
        ''' REV:%REV%               ※"YYYY-MM-DDTHH:MM:SSZ" (例)"2011-11-22T07:38:06Z"
        '''                        ※tbl_CARD_INFOのUPDATEDATE（更新日）
        ''' 
        ''' UID:%UID%               ※tbl_CARD_INFOのUPDATEID（更新機能ID）
        ''' 
        ''' END:VCARD
        ''' ----------------
        ''' </remarks>
        Sub MakeVCard(ByRef cardidArray() As String, ByRef carddateArray() As DateTime, ByRef vCardDataArray() As String, ByRef vCardDataArraySetCount As Integer, ByVal dlrcd As String, ByVal strcd As String)

            Using cardInfo As New DataAccess.IC3040601.IC3040601.Api.DataAccess.TblCardInfo
                Dim ret As IC3040601DataSet.TblCardInfoDataTable = _
                            cardInfo.GetSelectTable(dlrcd, strcd)
                Dim VCardData As New StringBuilder
                Dim uid As String = String.Empty
                Dim newUid As String = String.Empty
                Dim item_count As Integer = 1
                vCardDataArraySetCount = 0

                If Not IsNothing(ret) Then
                    'レコード1件ごとに処理を行う
                    For Each row As DataRow In ret
                        If vCardDataArraySetCount < VCardDataArrayMaxCounts Then '送信VCardData 最大数
                            newUid = row.Item("CARDID")
                            If uid <> newUid Then
                                cardidArray(vCardDataArraySetCount) = newUid
                                carddateArray(vCardDataArraySetCount) = row.Item("UPDATEDATE")
                                VCardData.Clear()
                                VCardData.Length = 0
                                VCardData.Append(MakeVCardBody(row))
                                VCardData.Append(MakeVCardEmail(newUid, item_count))
                                VCardData.Append(MakeVCardTel(newUid))
                                VCardData.Append(MakeVCardAddress(newUid, item_count))
                                VCardData.Append(MakeVCardEnd(row))
                                vCardDataArray(vCardDataArraySetCount) = VCardData.ToString
                                vCardDataArraySetCount = vCardDataArraySetCount + 1
                            End If
                            uid = newUid
                            item_count = 1
                        End If
                    Next
                End If
            End Using
        End Sub

        ''' <summary>
        ''' VCARD BODY を rowから作成する
        ''' </summary>
        ''' <param name="row">1件分のレコード</param>
        ''' <returns>作成したBODY文字列</returns>
        ''' <remarks>
        ''' BEGIN:VCARDから始まる
        ''' END:VCARDは別関数
        ''' </remarks>
        Function MakeVCardBody(ByVal row As DataRow) As String
            Dim VCardData As New StringBuilder
            VCardData.Length = 0

            With VCardData
                .Append("BEGIN:VCARD" & CrLF)
                .Append("VERSION:3.0" & CrLF)
                .Append("PRODID:-//Apple Inc.//iOS 5.0.1//EN" & CrLF)
                .Append("N:" & row.Item("LASTNAME") & ";" & row.Item("FIRSTNAME") & ";;;" & CrLF)
                .Append("FN:" & row.Item("LASTNAME") & " " & row.Item("FIRSTNAME") & CrLF)
                .Append("X-PHONETIC-FIRST-NAME:" & row.Item("FIRSTNAMEKANA") & CrLF)
                .Append("X-PHONETIC-LAST-NAME:" & row.Item("LASTNAMEKANA") & CrLF)
                .Append("ORG:" & row.Item("ORGANIZATION") & CrLF)
                .Append("TITLE:" & row.Item("TITLE") & CrLF)
                .Append("URL;" & row.Item("URL") & CrLF)
                .Append("NOTE:" & row.Item("MEMO") & CrLF)
            End With

            Return VCardData.ToString

        End Function

        ''' <summary>
        ''' VCARD EMAIL を カードIDから作成する
        ''' </summary>
        ''' <param name="CARDID">カードID</param>
        ''' <param name="itemCount">追加カウントデータ(ByRef Integer)</param>
        ''' <returns>作成したEMAIL文字列(複数件分)</returns>
        ''' <remarks>
        ''' EMAIL;%MAILTYPE%:%EMAIL% ※%MAILTYPE%="type=INTERNET;type=HOME;type=pref"、
        '''                                       "type=INTERNET;type=WORK"
        ''' item1.EMAIL;type=INTERNET:%EMAIL%
        ''' ～itemN.　Nを昇順番号にして追加する
        ''' </remarks>
        Function MakeVCardEmail(ByVal cardid As String, ByRef itemCount As Integer) As String
            Dim VCardData As New StringBuilder
            VCardData.Length = 0

            Using cardInfo As New DataAccess.IC3040601.IC3040601.Api.DataAccess.TblCardMail
                Dim ret As IC3040601DataSet.TblCardMailDataTable = _
                            cardInfo.GetSelectTable(cardid)
                Dim uid As String = String.Empty
                Dim newUid As String = String.Empty

                If Not IsNothing(ret) Then
                    'レコード1件ごとに処理を行う
                    For Each row As DataRow In ret
                        newUid = row.Item("SEQNO")
                        If uid <> newUid Then
                            With VCardData
                                If String.Equals("type=INTERNET", row.Item("MAILTYPE")) Then
                                    .Append("item" & itemCount & ".EMAIL;")
                                    itemCount = itemCount + 1
                                Else
                                    .Append("EMAIL;")
                                End If
                                .Append(row.Item("MAILTYPE") & ":")
                                .Append(row.Item("EMAIL") & CrLF)
                            End With
                        End If
                        uid = newUid
                    Next
                End If
            End Using

            Return VCardData.ToString

        End Function

        ''' <summary>
        ''' VCARD TEL を カードIDから作成する
        ''' </summary>
        ''' <param name="CARDID">カードID</param>
        ''' <returns>作成したTEL文字列(複数件分)</returns>
        ''' <remarks>
        ''' TEL;%TELTYPE%:%TEL%     ※%TELTYPE% ="type=CELL;type=VOICE;type=pref"、
        '''                          "type=IPHONE;type=CELL;type=VOICE"、
        '''                          "type=HOME;type=VOICE"、
        '''                          "type=WORK;type=VOICE"、
        '''                          "type=MAIN"、
        '''                          "type=HOME;type=FAX"、
        '''                          "type=WORK;type=FAX"、
        '''                          "type=OTHER;type=FAX"、
        '''                          "type=PAGER"、
        '''                          "type=OTHER;type=VOICE"
        ''' ～
        ''' </remarks>
        Function MakeVCardTel(ByVal cardid As String) As String
            Dim VCardData As New StringBuilder
            VCardData.Length = 0

            Using cardInfo As New DataAccess.IC3040601.IC3040601.Api.DataAccess.TblCardTel
                Dim ret As IC3040601DataSet.TblCardTelDataTable = _
                            cardInfo.GetSelectTable(cardid)
                Dim uid As String = String.Empty
                Dim newUid As String = String.Empty

                If Not IsNothing(ret) Then
                    'レコード1件ごとに処理を行う
                    For Each row As DataRow In ret
                        newUid = row.Item("SEQNO")
                        If uid <> newUid Then
                            With VCardData
                                .Append("TEL;" & row.Item("TELTYPE") & ":" & row.Item("TEL") & CrLF)
                            End With
                        End If
                        uid = newUid
                    Next
                End If
            End Using

            Return VCardData.ToString

        End Function

        ''' <summary>
        ''' VCARD ADDRESS を カードIDから作成する
        ''' </summary>
        ''' <param name="CARDID">カードID</param>
        ''' <param name="itemCount">追加カウントデータ(ByRef Integer)</param>
        ''' <returns>作成したADDRESS文字列(複数件分)</returns>
        ''' <remarks>
        ''' item1.ADR;%ADDRESSTYPE%:※%ADDRESS%
        '''                         ※%ADDRESSTYPE% ="type=HOME;type=pref"
        '''                         ※%ADDRESS%     =";;番地(Banchi\nBan\nB123\nB456);市(City);州(Syu);郵便番号(123);"
        ''' item1.X-ABADR:%X_ABADR% ※"cn"=中国、"jp"=日本
        ''' ～itemN.　Nを昇順番号にして追加する
        ''' </remarks>
        Function MakeVCardAddress(ByVal cardid As String, ByRef itemCount As Integer) As String
            Dim VCardData As New StringBuilder
            VCardData.Length = 0

            Using cardInfo As New DataAccess.IC3040601.IC3040601.Api.DataAccess.TblCardAddress
                Dim ret As IC3040601DataSet.TblCardAddressDataTable = _
                            cardInfo.GetSelectTable(cardid)
                Dim uid As String = String.Empty
                Dim newUid As String = String.Empty

                If Not IsNothing(ret) Then
                    'レコード1件ごとに処理を行う
                    For Each row As DataRow In ret
                        newUid = row.Item("SEQNO")
                        If uid <> newUid Then
                            With VCardData
                                .Append("item" & itemCount & ".ADR;" & row.Item("ADDRESSTYPE") & ":" & row.Item("ADDRESS") & CrLF)
                                .Append("item" & itemCount & ".X-ABADR:" & row.Item("X_ABADR") & CrLF)
                                itemCount = itemCount + 1
                            End With
                        End If
                        uid = newUid
                    Next
                End If
            End Using

            Return VCardData.ToString

        End Function

        ''' <summary>
        ''' VCARD END を rowから作成する
        ''' </summary>
        ''' <param name="row">1件分のレコード</param>
        ''' <returns>作成したEND文字列</returns>
        ''' <remarks>
        ''' REV:%REV%       ※"YYYY-MM-DDTHH:MM:SSZ" (例)"2011-11-22T07:38:06Z"
        '''                 ※tbl_CARD_INFOのUPDATEDATE（更新日）
        ''' UID:%UID%       ※tbl_CARD_INFOのUPDATEID（更新機能ID）
        ''' END:VCARD
        ''' </remarks>
        Private Function MakeVCardEnd(ByVal row As DataRow) As String
            Dim VCardData As New StringBuilder
            Dim VCDate As Date
            VCardData.Length = 0

            With VCardData
                VCDate = row.Item("UPDATEDATE")
                '.Append("REV:2011-11-22T07:38:06Z" & CrLF)
                .Append("REV:" & VCDate.ToUniversalTime().ToString("yyyy-MM-ddThh:mm:ssZ", Globalization.CultureInfo.CurrentCulture()) & CrLF)
                .Append("UID:" & row.Item("UPDATEID") & CrLF)
                .Append("END:VCARD" & CrLF)
            End With

            Return VCardData.ToString

        End Function
#If 0 Then
        ''' <summary>
        ''' 文字を数値に変換
        ''' </summary>
        ''' <param name="obj"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' DBNullのときもゼロを返す
        ''' </remarks>
        Private Function CNullInt(ByVal obj As Object) As Integer
            Dim ret As Integer = 0
            Try
                If Not IsDBNull(obj) Then
                    ret = CInt(obj)
                End If
            Catch ex As ApplicationException
                ret = 0
            End Try

            Return ret
        End Function
#End If
        ''' <summary>
        ''' MD5 に変換した文字列を作成する
        ''' </summary>
        ''' <param name="path">変換元のパス</param>
        ''' <param name="dat">変換元の日付の文字列</param>
        ''' <returns>MD5に変換した文字列</returns>
        ''' <remarks></remarks>
        Private Shared Function CreateMd5(ByVal path As String, ByVal dat As String) As String

            'パスと日付を連結する
            Dim str As New StringBuilder(path)
            str.Append(dat)

            '文字列をbyte型配列に変換する
            Dim byteArray As Byte() = System.Text.Encoding.UTF8.GetBytes(str.ToString)

            'MD5CryptoServiceProviderオブジェクトを作成
            Using md5 As New System.Security.Cryptography.MD5CryptoServiceProvider()
                'または、次のようにもできる
                'Dim md5 As System.Security.Cryptography.MD5 = _
                '    System.Security.Cryptography.MD5.Create()

                'ハッシュ値を計算する
                Dim bs As Byte() = md5.ComputeHash(byteArray)

                'byte型配列を16進数の文字列に変換
                Dim result As New System.Text.StringBuilder()
                Dim b As Byte
                For Each b In bs
                    result.Append(b.ToString("x2", Globalization.CultureInfo.CurrentCulture()))
                Next b

                Return result.ToString
            End Using
        End Function

    End Class

End Namespace
