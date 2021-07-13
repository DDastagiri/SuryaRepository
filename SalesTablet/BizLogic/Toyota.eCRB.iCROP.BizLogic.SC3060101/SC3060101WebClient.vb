Imports System.Text
Imports Toyota.eCRB.SystemFrameworks.Core

''' <summary>
''' SC3060101(査定チェックシート)
''' Webクライアント
''' </summary>
''' <remarks></remarks>
Public Class SC3060101WebClient

#Region "メンバ変数"

    ''' <summary>販売店コード</summary>
    Private dlrCd_ As String

    ''' <summary>店舗コード</summary>
    Private strCd_ As String

    ''' <summary>依頼ID</summary>
    Private requestId_ As String

    ''' <summary>査定No</summary>
    Private assessmentNo_ As String

#End Region

#Region "定数"

    ''' <summary>要求XML rootノード名</summary>
    Const XML_NODE_UCARINFORMATION As String = "SearchAssessmentInforMation"
    ''' <summary>要求XML  ヘッダーノード名</summary>
    Const XML_NODE_HEAD As String = "Head"
    ''' <summary>要求XML  送信日付タグ名</summary>
    Const XML_LEAF_TRANSMISSIONDATE As String = "TransmissionDate"
    ''' <summary>要求XML  アカウントタグ名</summary>
    Const XML_LEAF_ACCOUNT As String = "Account"
    ''' <summary>要求XML  パスワードタグ名</summary>
    Const XML_LEAF_PASSWORD As String = "Password"

    ''' <summary>要求XML  ボディノード名</summary>
    Const XML_NODE_BODY As String = "Body"
    ''' <summary>要求XML  販売店コードタグ名</summary>
    Const XML_LEAF_DLR_CD As String = "DlrCd"
    ''' <summary>要求XML 店舗コードタグ名</summary>
    Const XML_LEAF_STR_CD As String = "StrCd"
    ''' <summary>要求XML 依頼ＩＤタグ名</summary>
    Const XML_LEAF_REQUEST_ID As String = "RequestId"
    ''' <summary>要求XML 査定Ｎｏタグ名</summary>
    Const XML_LEAF_ASSESSMENT_NO As String = "AssessmentNo"
    ''' <summary>要求XML エンコード</summary>
    Const XML_ENC_CODE As String = "UTF-8"
    ''' <summary>要求XML 区切り文字</summary>
    Const XML_BOUNDRY As String = "--"
    ''' <summary>要求XML 呼び出しメソッド</summary>
    Const XML_METHOD As String = "POST"
    ''' <summary>要求XML マルチパート指定</summary>
    Const XML_CONTENT_TYPE As String = "multipart/form-data; boundary="
    ''' <summary>要求XML コンテント一行目</summary>
    Const XML_CONTENT_ROW1 As String = "Content-Disposition: form-data; name=""XMLFILE""; filename="""
    ''' <summary>要求XML コンテント二行目</summary>
    Const XML_CONTENT_ROW2 As String = "Content-Type: application/octet-stream"
    ''' <summary>要求XML コンテント三行目</summary>
    Const XML_CONTENT_ROW3 As String = "Content-Transfer-Encoding: binary"
    ''' <summary>返却XML 文字コード</summary>
    Const XML_RECIEVE_ENC_CODE As String = "utf-16"

    ''' <summary>開始ログ</summary>
    Private Const STARTLOG As String = "START "

    ''' <summary>終了ログ</summary>
    Private Const ENDLOG As String = "END "

    ''' <summary>終了ログRETURN</summary>
    Private Const ENDLOGRETURN As String = "RETURN "

#End Region

    ''' <summary>
    ''' コンストラクタ
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="storeCode">店舗コード</param>
    ''' <param name="requestId">依頼ID</param>
    ''' <param name="assessmentNo">査定No</param>
    ''' <remarks>
    ''' </remarks>
    Public Sub New(ByVal dealerCode As String, ByVal storeCode As String, ByVal requestId As String, ByVal assessmentNo As String)
        Me.dlrCd_ = dealerCode
        Me.strCd_ = storeCode
        Me.requestId_ = requestId
        Me.assessmentNo_ = assessmentNo
    End Sub

    ''' <summary>
    ''' 中古車IF呼び出し
    ''' </summary>
    ''' <param name="url">IFのURL</param>
    ''' <param name="userId">ユーザーID</param>
    ''' <param name="password">パスワード</param>
    ''' <remarks>
    ''' </remarks>
    Public Function SendRequest(ByVal url As System.Uri, ByVal userId As String, ByVal password As String, ByVal sendDate As String) As String

        Const METHODNAME As String = "SendRequest "

        'デバッグログ(開始)
        '開始ログ出力
        Dim startLogInfo As New StringBuilder
        startLogInfo.Append(METHODNAME)
        startLogInfo.Append(STARTLOG)
        Logger.Info(startLogInfo.ToString())

        '====================================
        ' 送信するXMLの生成
        '====================================
        Dim xmlString As String = GetUcarRequestXml(userId, password, sendDate).Replace(vbCrLf, "")

        'インフォーメーションログを出力
        Logger.Info(xmlString)

        '文字コード
        Dim enc As System.Text.Encoding = _
            System.Text.Encoding.GetEncoding(XML_ENC_CODE)
        '区切り文字列
        Dim tick As String = CStr(System.Environment.TickCount)

        Dim encoding As New System.Text.UnicodeEncoding
        Dim bytArray() As Byte = encoding.GetBytes(xmlString)

        Dim ret As String = ""

        If bytArray.Length > 0 Then
            '------------------------------------
            '           HTTPXML通信
            '------------------------------------
            Dim objXmlHttp As System.Net.HttpWebRequest     'ServerXMLHttpオブジェクト

            ' XMLHTTPオブジェクトの生成
            objXmlHttp = _
                CType(System.Net.WebRequest.Create(url),  _
                    System.Net.HttpWebRequest)

            ' タイムアウト設定
            objXmlHttp.Timeout = 225000

            ' サーバー接続（同期）
            'メソッドにPOSTを指定
            objXmlHttp.Method = XML_METHOD
            'ContentTypeを設定
            objXmlHttp.ContentType = XML_CONTENT_TYPE + tick

            Dim boundary As Byte() = enc.GetBytes(XML_BOUNDRY + tick)
            Dim crlf As Byte() = enc.GetBytes(vbCrLf)

            ' リクエストストリーム
            Try
                Dim contentLen As Long

                ' ヘッダ
                Dim headerXML As String
                '  XMLのヘッダをセット
                headerXML = XML_CONTENT_ROW1 + vbCrLf + _
                            XML_CONTENT_ROW2 + vbCrLf + _
                            XML_CONTENT_ROW3 + vbCrLf + vbCrLf

                contentLen += enc.GetBytes(headerXML).Length + crlf.Length
                contentLen += bytArray.Length + crlf.Length + boundary.Length
                '全体のデータサイズ
                objXmlHttp.ContentLength = contentLen + boundary.Length + boundary.Length

                Dim reqStream As System.IO.Stream = objXmlHttp.GetRequestStream()

                '  XMLのセット
                reqStream.Write(boundary, 0, boundary.Length)
                reqStream.Write(crlf, 0, crlf.Length)
                reqStream.Write(enc.GetBytes(headerXML), 0, enc.GetBytes(headerXML).Length)
                reqStream.Write(bytArray, 0, bytArray.Length)
                reqStream.Write(crlf, 0, crlf.Length)

                reqStream.Write(boundary, 0, boundary.Length)
                reqStream.Write(boundary, 0, boundary.Length)
                reqStream.Close()

                ' 戻り値の取得
                Dim res As System.Net.WebResponse = objXmlHttp.GetResponse()
                Dim st As System.IO.Stream = res.GetResponseStream()
                Dim sr As New System.IO.StreamReader(st, System.Text.Encoding.GetEncoding(XML_RECIEVE_ENC_CODE))

                ' 返された値をXMLに変換する
                Dim receiveXML = New System.Xml.XmlDocument
                receiveXML.LoadXml(sr.ReadToEnd)

                ret = receiveXML.InnerXml

                sr.Close()
                sr = Nothing

            Finally
                objXmlHttp.Abort()
                objXmlHttp = Nothing

            End Try

        End If

        'デバッグログ(終了)
        '終了ログ出力
        Dim endLogInfo As New StringBuilder
        endLogInfo.Append(METHODNAME)
        endLogInfo.Append(ENDLOG)
        endLogInfo.Append(ENDLOGRETURN)
        endLogInfo.Append(ret)
        Logger.Info(endLogInfo.ToString())

        Return ret

    End Function

    ''' <summary>
    ''' 中古車IF呼び出し用要求XML作成
    ''' </summary>
    ''' <returns>要求XML</returns>
    ''' <remarks>
    ''' </remarks>
    Private Function GetUcarRequestXml(ByVal userid As String, ByVal password As String, ByVal sendDate As String) As String

        Const METHODNAME As String = "GetUcarRequestXml "

        'デバッグログ(開始)
        '開始ログ出力
        Dim startLogInfo As New StringBuilder
        startLogInfo.Append(METHODNAME)
        startLogInfo.Append(STARTLOG)
        Logger.Info(startLogInfo.ToString())

        Dim xmlDoc As System.Xml.XmlDocument

        xmlDoc = New System.Xml.XmlDocument()

        Dim pXml As System.Xml.XmlProcessingInstruction = xmlDoc.CreateProcessingInstruction("xml", "version=""1.0"" encoding=""utf-8""")
        xmlDoc.AppendChild(pXml)

        Dim rootElement As System.Xml.XmlElement = xmlDoc.CreateElement(XML_NODE_UCARINFORMATION)

        'ヘッダーノードを生成
        Dim headerElement As System.Xml.XmlElement = xmlDoc.CreateElement(XML_NODE_HEAD)

        '送信日付
        Dim sendDateElm As System.Xml.XmlElement = xmlDoc.CreateElement(XML_LEAF_TRANSMISSIONDATE)
        sendDateElm.InnerText = sendDate
        headerElement.AppendChild(sendDateElm)

        'アカウント
        Dim accountElm As System.Xml.XmlElement = xmlDoc.CreateElement(XML_LEAF_ACCOUNT)
        accountElm.InnerText = userid ' "ygueW1iuhIw6u2Ehniqw"
        headerElement.AppendChild(accountElm)

        'パスワード
        Dim passwordElm As System.Xml.XmlElement = xmlDoc.CreateElement(XML_LEAF_PASSWORD)
        passwordElm.InnerText = password '"9hcK7snC2iwvgyzQlx83"
        headerElement.AppendChild(passwordElm)
        'ヘッダーノードを追加
        rootElement.AppendChild(headerElement)

        'ボディノードを生成
        Dim bodyElement As System.Xml.XmlElement = xmlDoc.CreateElement(XML_NODE_BODY)

        '販売店コード
        Dim dlrCd As System.Xml.XmlElement = xmlDoc.CreateElement(XML_LEAF_DLR_CD)
        dlrCd.InnerText = Me.dlrCd_
        bodyElement.AppendChild(dlrCd)

        '店舗コード
        Dim strCd As System.Xml.XmlElement = xmlDoc.CreateElement(XML_LEAF_STR_CD)
        strCd.InnerText = Me.strCd_
        bodyElement.AppendChild(strCd)

        '依頼ＩＤ
        Dim requestId As System.Xml.XmlElement = xmlDoc.CreateElement(XML_LEAF_REQUEST_ID)
        requestId.InnerText = Me.requestId_
        bodyElement.AppendChild(requestId)

        '査定Ｎｏ
        Dim assessmentNo As System.Xml.XmlElement = xmlDoc.CreateElement(XML_LEAF_ASSESSMENT_NO)
        assessmentNo.InnerText = Me.assessmentNo_
        bodyElement.AppendChild(assessmentNo)
        'ボディノードを追加
        rootElement.AppendChild(bodyElement)

        'ルートノードを追加
        xmlDoc.AppendChild(rootElement)

        'デバッグログ(終了)
        '終了ログ出力
        Dim endLogInfo As New StringBuilder
        endLogInfo.Append(METHODNAME)
        endLogInfo.Append(ENDLOG)
        endLogInfo.Append(ENDLOGRETURN)
        endLogInfo.Append(xmlDoc.OuterXml & vbCrLf)
        Logger.Info(endLogInfo.ToString())


        Return xmlDoc.OuterXml & vbCrLf

    End Function


End Class
