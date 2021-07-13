'-------------------------------------------------------------------------
'SMBCommonWebServiceClassBusinessLogic.vb
'-------------------------------------------------------------------------
'機能：WebService送受信用関数
'補足：
'作成：2013/08/20 TMEJ 河原 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発
'更新：2014/02/14 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発
'更新：2014/3/5 　TMEJ 陳 　TMEJ次世代サービス 工程管理機能開発
'─────────────────────────────────────

Imports System.Xml
Imports System.Text
Imports System.Web
Imports System.Net
Imports System.IO
Imports System.Reflection
Imports System.Globalization
Imports System.Text.RegularExpressions
Imports System.Xml.Serialization
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.CommonUtility.SMBCommonClass.Api.DataAccess
Imports Toyota.eCRB.CommonUtility.SMBCommonClass.Api.DataAccess.SMBCommonClassDataSet

Partial Class SMBCommonClassBusinessLogic

#Region "定数"

    ''' <summary>
    ''' 成功
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ResultSuccess As Long = 0

    ''' <summary>
    ''' XML戻り値解析失敗
    ''' </summary>
    ''' <remarks></remarks>
    Public Const XmlErr As Long = -1

    ''' <summary>
    ''' WebService名(IC3B30504)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WebServiceID As String = "IC3B30504"

    ''' <summary>
    ''' WebService(IC3B30504)メソッド名
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WebServiceMethod As String = "UpdateStallReserveInfo"

    ''' <summary>
    ''' WebService(IC3B30504)引数名
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WebServiceArgument As String = "s="

    ''' <summary>
    ''' WebServiceURL(IC3B30504)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WebServiceURL As String = "WEBSERVICE_URL_IC3B30504"

    ''' <summary>
    ''' WebService ヘッダーコメント削除置換用文字列
    ''' </summary>
    ''' <remarks></remarks>
    Private Const XmlReplace As String = "<?xml version=""1.0"" encoding=""utf-16""?>"

    ''' <summary>
    ''' エンコード(UTF-8)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const EncodingUTF8 As String = "UTF-8"

    ''' <summary>
    ''' 送信方法(POST)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const Post As String = "POST"

    ''' <summary>
    ''' ContentType
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ContentTypeString As String = "application/x-www-form-urlencoded"

    ''' <summary>
    ''' Node名(Result)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NodeResult As String = "Result"

    ''' <summary>
    ''' Tag名(ResultCode)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagResultCode As String = "ResultCode"

    ''' <summary>
    ''' Tag名(SVCIN_ID)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagSVCINID As String = "SVCIN_ID"

    ''' <summary>
    ''' Tag名(JOB_DTL_ID)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagJOBDTLID As String = "JOB_DTL_ID"

    ''' <summary>
    ''' Tag名(STALL_USE_ID)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagSTALLUSEID As String = "STALL_USE_ID"

    ''' <summary>
    ''' Tag名(CSTID)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagCSTID As String = "CSTID"

    ''' <summary>
    ''' Tag名(VCLID)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagVCLID As String = "VCLID"

    ''' <summary>
    ''' Tag名(ROW_LOCK_VERSION)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagROWLOCKVERSION As String = "ROW_LOCK_VERSION"

    '更新：2014/3/5 　TMEJ 陳 　TMEJ次世代サービス 工程管理機能開発 START 
    ''' <summary>
    ''' 基幹連携送信タイムアウト値
    ''' </summary>
    Private Const SysLinkSendTimeOutVal = "LINK_SEND_TIMEOUT_VAL"

    '更新：2014/3/5 　TMEJ 陳 　TMEJ次世代サービス 工程管理機能開発 END 

#End Region


#Region "Public"

    ''' <summary>
    ''' 予約更新登録WebService呼出処理
    ''' </summary>
    ''' <param name="inXmlClass">XML作成用クラス</param>
    ''' <returns>WebService処理結果</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Public Function CallReserveWebService(ByVal inXmlClass As XmlDocumentClass) As WebServiceResultRow

        ''開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} " _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name))

        'XML戻り値用DataTable
        Using dtWebServiceResult As New WebServiceResultDataTable

            'XML戻り値用DataRow
            Dim rowWebServiceResult As WebServiceResultRow = dtWebServiceResult.NewWebServiceResultRow

            '初期値設定(-1)
            rowWebServiceResult.RESULTCODE = XmlErr
            rowWebServiceResult.SVCIN_ID = XmlErr
            rowWebServiceResult.JOB_DTL_ID = XmlErr
            rowWebServiceResult.STALL_USE_ID = XmlErr
            rowWebServiceResult.CSTID = XmlErr
            rowWebServiceResult.VCLID = XmlErr
            rowWebServiceResult.ROW_LOCK_VERSION = XmlErr

            Try

                Dim systemEnvSetting As New SystemEnvSetting

                'WebServiceURLの取得
                Dim envSettingRow As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow = systemEnvSetting.GetSystemEnvSetting(WebServiceURL)

                'URLの取得確認
                If envSettingRow Is Nothing Then
                    'URL取得失敗

                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} DlrEnvSetting == NOTHING OUT:resultXmlValue.ResultCode = {2}" _
                            , Me.GetType.ToString _
                            , MethodBase.GetCurrentMethod.Name _
                            , rowWebServiceResult.RESULTCODE))

                    Return rowWebServiceResult

                End If

                'WebServiceのURLを作成
                Dim createUrl As String = String.Concat(envSettingRow.PARAMVALUE, "/", WebServiceMethod)

                'WebService送信用XML作成処理
                Dim sendXml As String = CreateXml(inXmlClass)

                'XMLのヘッダー部分を削除
                sendXml = sendXml.Replace(XmlReplace, String.Empty)

                '送信XMLをエンコードし引数に指定
                sendXml = String.Concat(WebServiceArgument, HttpUtility.UrlEncode(sendXml))

                'WebService送受信処理
                Dim resultString As String = CallWebServiceSite(sendXml, createUrl)
                '2014/3/5 　TMEJ 陳 　TMEJ次世代サービス 工程管理機能開発 START
                If String.IsNullOrEmpty(resultString) Then

                    Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                                 , "{0}.{1} TimeOutValue is EmptyOrNull OUT:ErrWebService " _
                                 , Me.GetType.ToString _
                                 , MethodBase.GetCurrentMethod.Name))

                    rowWebServiceResult.RESULTCODE = XmlErr

                    Return rowWebServiceResult
                End If
                '2014/3/5 　TMEJ 陳 　TMEJ次世代サービス 工程管理機能開発 END

                '返却された文字列をデコード
                resultString = HttpUtility.HtmlDecode(resultString)

                'XML名前空間用の正規表現設定
                Dim regex As Regex = New Regex(" xmln.*=""[^""]*"".")

                'XML名前空間を除去
                resultString = regex.Replace(resultString, Space(0))

                'WebServiceの戻りXMLを解析し値を取得
                rowWebServiceResult = GetXMLData(resultString, rowWebServiceResult)

                '終了ログの出力
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} OUT:resultXmlValue.ResultCode = {2}" _
                            , Me.GetType.ToString _
                            , MethodBase.GetCurrentMethod.Name _
                            , rowWebServiceResult.RESULTCODE))

                Return rowWebServiceResult

            Catch ex As System.Net.WebException
                'WebServiceエラー

                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                             , "{0}.{1} OUT:ErrWebService = {2}" _
                             , Me.GetType.ToString _
                             , MethodBase.GetCurrentMethod.Name _
                             , ex.Message))

                rowWebServiceResult.RESULTCODE = XmlErr

                Return rowWebServiceResult

            End Try

        End Using

    End Function

#End Region


#Region "XML作成"

    ''' <summary>
    ''' XML作成(メイン)
    ''' </summary>
    ''' <param name="inXmlClass">XML作成用クラス</param>
    ''' <returns>XMLString</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Function CreateXml(ByVal inXmlClass As XmlDocumentClass) As String

        ''開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} " _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name))


        'XMLのHeadTagの作成処理
        inXmlClass = CreateHeadTag(inXmlClass)

        'テキストWriter
        Using writer As New StringWriter(CultureInfo.InvariantCulture)

            'XMLシリアライザー型の設定
            Dim serializer As New XmlSerializer(GetType(XmlDocumentClass))

            'XmlDocumentClassをXML化
            serializer.Serialize(writer, inXmlClass)

            'XML名前空間用の正規表現設定
            Dim regex As Regex = New Regex(" xmlns:xs.=""[^""]*""")

            'XML名前空間を除去
            Dim stringXml As String = regex.Replace(writer.ToString, Space(0))

            '終了ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:RETURNCODE = {2}" _
                        , Me.GetType.ToString _
                        , MethodBase.GetCurrentMethod.Name _
                        , stringXml))

            Return stringXml

        End Using

    End Function

    ''' <summary>
    ''' XML作成(HeadTag)
    ''' </summary>
    ''' <param name="inXmlClass">XML作成用クラス</param>
    ''' <returns>XML作成用クラス</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Function CreateHeadTag(ByVal inXmlClass As XmlDocumentClass) As XmlDocumentClass

        ''開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} " _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name))

        'メッセージID
        inXmlClass.Head.MessageId = WebServiceID

        '国コード
        inXmlClass.Head.CountryCode = EnvironmentSetting.CountryCode

        '基幹SYSTEM識別コード(0固定)
        inXmlClass.Head.LinkSystemCode = "0"

        '終了ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} OUT:RETURNCODE = {2}" _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name _
                    , inXmlClass))

        Return inXmlClass


    End Function

#End Region


#Region "XML送受信"

    ''' <summary>
    ''' WebServiceのサイトを呼出
    ''' WebServiceを送信し結果を受信する
    ''' </summary>
    ''' <param name="sendXml">送信XML文字列</param>
    ''' <param name="WebServiceUrl">送信先URL</param>
    ''' <returns>結果XML</returns>
    ''' <remarks></remarks>
    Private Function CallWebServiceSite(ByVal sendXml As String, ByVal webServiceUrl As String) As String

        ''開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} SENDXML:{2} URL:{3}" _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name _
                    , sendXml, webServiceUrl))


        'タイムアウト設定値を取得
        Dim timeOut As String = Me.GetTimeOutValues()

        'システム設定値の取得でエラーがあった場合
        If String.IsNullOrEmpty(timeOut) Then

            '終了ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} TimeOutValue error,  OUT:RETURNSTRING = Empty" _
                        , Me.GetType.ToString _
                        , MethodBase.GetCurrentMethod.Name))

            Return String.Empty
        End If

        '文字コードを指定する
        Dim enc As System.Text.Encoding = _
            System.Text.Encoding.GetEncoding(EncodingUTF8)

        'バイト型配列に変換
        Dim postDataBytes As Byte() = _
            System.Text.Encoding.UTF8.GetBytes(sendXml)

        'WebRequestの作成
        Dim req As WebRequest = WebRequest.Create(webServiceUrl)

        'メソッドにPOSTを指定
        req.Method = Post

        'ContentType指定(固定)
        req.ContentType = ContentTypeString

        'POST送信するデータの長さを指定
        req.ContentLength = postDataBytes.Length

        '2014/3/5 　TMEJ 陳 　TMEJ次世代サービス 工程管理機能開発 START
        '送信タイムアウト設定(10秒)
        'req.Timeout = 10000
        '送信タイムアウト設定
        req.Timeout = CType(timeOut, Integer)
        '2014/3/5 　TMEJ 陳 　TMEJ次世代サービス 工程管理機能開発 END

        'データをPOST送信するためのStreamを取得
        Using reqStream As Stream = req.GetRequestStream()

            '送信するデータを書き込む
            reqStream.Write(postDataBytes, 0, postDataBytes.Length)

        End Using

        'サーバーからの応答を受信するためのWebResponseを取得
        Dim resultResponse As WebResponse = req.GetResponse()

        '応答データを受信するためのStreamを取得
        Dim resultStream As Stream = resultResponse.GetResponseStream()

        '返却文字列
        Dim resultString As String

        '受信して表示
        Using resultReader As New StreamReader(resultStream, enc)

            '返却文字列を取得
            resultString = resultReader.ReadToEnd()

        End Using

        '終了ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} OUT:RETURNSTRING = {2}" _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name _
                    , resultString))

        Return resultString

    End Function

    ''' <summary>
    ''' WebServiceの戻りXMLを解析し値を取得
    ''' </summary>
    ''' <param name="resultString">送信XML文字列</param>
    ''' <param name="rowWebServiceResult">XML戻り値用DataRow</param>
    ''' <returns>WebService結果</returns>
    ''' <remarks></remarks>
    Private Function GetXMLData(ByVal resultString As String, _
                                ByVal rowWebServiceResult As WebServiceResultRow) As WebServiceResultRow

        ''開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} RESULTXML:{2}" _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name _
                    , resultString))

        Try

            'XmlDocument
            Dim resultXmlDocument As New XmlDocument

            '返却された文字列をXML化
            resultXmlDocument.LoadXml(resultString)

            'XmlElementを取得
            Dim resultXmlElement As XmlElement = resultXmlDocument.DocumentElement

            'XmlElementの確認
            If resultXmlElement Is Nothing Then
                '取得失敗

                'エラーログの出力
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} OUT:Err XmlDocument.DocumentElement = Nothing" _
                            , Me.GetType.ToString _
                            , MethodBase.GetCurrentMethod.Name))

                'エラーコード設定
                rowWebServiceResult.RESULTCODE = XmlErr

                Return rowWebServiceResult

            End If

            '子ノードリストの取得
            Dim resultXmlNodeList As XmlNodeList = resultXmlElement.GetElementsByTagName(NodeResult)

            '子ノードリストの確認
            If resultXmlNodeList Is Nothing OrElse resultXmlNodeList.Count = 0 Then
                '取得失敗

                'エラーログの出力
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} OUT:Err Update_Reserve = Nothing" _
                            , Me.GetType.ToString _
                            , MethodBase.GetCurrentMethod.Name))

                'エラーコード設定
                rowWebServiceResult.RESULTCODE = XmlErr

                Return rowWebServiceResult

            End If


            '子ノードの取得
            Dim resultXmlNode As XmlNode = resultXmlNodeList.Item(0)

            '解析したXMLから設定されている値の取得
            rowWebServiceResult = GetXmlNodeValue(rowWebServiceResult, resultXmlNode)


            '終了ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:RETURNSTRING = {2}" _
                        , Me.GetType.ToString _
                        , MethodBase.GetCurrentMethod.Name _
                        , resultString))

            Return rowWebServiceResult


        Catch ex As XmlException

            'エラーログの出力
            Logger.Error(String.Format(CultureInfo.CurrentCulture _
                         , "{0}.{1} OUT:Err XmlException = {2}" _
                         , Me.GetType.ToString _
                         , MethodBase.GetCurrentMethod.Name _
                         , ex.Message))

            'エラーコード設定
            rowWebServiceResult.RESULTCODE = XmlErr

            Return rowWebServiceResult

        End Try

    End Function

    ''' <summary>
    ''' 戻りXMLから設定されている値を取得
    ''' </summary>
    ''' <param name="rowWebServiceResultRow">XML戻り値用DataRow</param>
    ''' <param name="resultXmlNode">受信XMLノード</param>
    ''' <returns>XML戻り値用DataRow</returns>
    ''' <remarks></remarks>
    Private Function GetXmlNodeValue(ByVal rowWebServiceResultRow As WebServiceResultRow, _
                                     ByVal resultXmlNode As XmlNode) As WebServiceResultRow

        '開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} " _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name))


        'ResultCodeタグの値取得
        rowWebServiceResultRow.RESULTCODE = GetTagValue(resultXmlNode, TagResultCode)

        'WEBServiceの処理結果確認
        If rowWebServiceResultRow.RESULTCODE <> ResultSuccess Then
            '処理結果が失敗

            '終了ログの出力
            Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                         , "{0}.{1} OUT:Err ResultCode = {2}" _
                         , Me.GetType.ToString _
                         , MethodBase.GetCurrentMethod.Name _
                         , rowWebServiceResultRow.RESULTCODE))

            Return rowWebServiceResultRow

        End If


        'SVCIN_IDタグの値取得
        '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
        'rowWebServiceResultRow.SVCIN_ID = GetTagValue(resultXmlNode, TagSVCINID)
        rowWebServiceResultRow.SVCIN_ID = GetTagDecimalValue(resultXmlNode, TagSVCINID)
        '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

        'SVCIN_IDタグの値取得確認
        If rowWebServiceResultRow.SVCIN_ID = XmlErr Then
            '処理結果が失敗

            '終了ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:Err TagSVCIN_ID = NOTHING" _
                        , Me.GetType.ToString _
                        , MethodBase.GetCurrentMethod.Name))

            'エラーコードに-1を設定
            rowWebServiceResultRow.RESULTCODE = XmlErr

            Return rowWebServiceResultRow

        End If


        'JOB_DTL_IDタグの値取得
        '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
        'rowWebServiceResultRow.JOB_DTL_ID = GetTagValue(resultXmlNode, TagJOBDTLID)
        rowWebServiceResultRow.JOB_DTL_ID = GetTagDecimalValue(resultXmlNode, TagJOBDTLID)
        '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

        'JOB_DTL_IDタグの値取得確認
        If rowWebServiceResultRow.JOB_DTL_ID = XmlErr Then
            '処理結果が失敗

            '終了ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:Err TagJOB_DTL_ID = NOTHING" _
                        , Me.GetType.ToString _
                        , MethodBase.GetCurrentMethod.Name))

            'エラーコードに-1を設定
            rowWebServiceResultRow.RESULTCODE = XmlErr

            Return rowWebServiceResultRow

        End If


        'STALL_USE_IDタグの値取得
        '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
        'rowWebServiceResultRow.STALL_USE_ID = GetTagValue(resultXmlNode, TagSTALLUSEID)
        rowWebServiceResultRow.STALL_USE_ID = GetTagDecimalValue(resultXmlNode, TagSTALLUSEID)
        '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

        'STALL_USE_IDタグの値取得確認
        If rowWebServiceResultRow.STALL_USE_ID = XmlErr Then
            '処理結果が失敗

            '終了ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:Err TagSTALL_USE_ID = NOTHING" _
                        , Me.GetType.ToString _
                        , MethodBase.GetCurrentMethod.Name))

            'エラーコードに-1を設定
            rowWebServiceResultRow.RESULTCODE = XmlErr

            Return rowWebServiceResultRow

        End If


        'CSTIDタグの値取得
        '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
        'rowWebServiceResultRow.CSTID = GetTagValue(resultXmlNode, TagCSTID)
        rowWebServiceResultRow.CSTID = GetTagDecimalValue(resultXmlNode, TagCSTID)
        '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

        'CSTIDタグの値取得確認
        If rowWebServiceResultRow.CSTID = XmlErr Then
            '処理結果が失敗

            '終了ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:Err TagCSTID = NOTHING" _
                        , Me.GetType.ToString _
                        , MethodBase.GetCurrentMethod.Name))

            'エラーコードに-1を設定
            rowWebServiceResultRow.RESULTCODE = XmlErr

            Return rowWebServiceResultRow

        End If


        'VCLIDタグの値取得
        '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
        'rowWebServiceResultRow.VCLID = GetTagValue(resultXmlNode, TagVCLID)
        rowWebServiceResultRow.VCLID = GetTagDecimalValue(resultXmlNode, TagVCLID)
        '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

        'VCLIDタグの値取得確認
        If rowWebServiceResultRow.VCLID = XmlErr Then
            '処理結果が失敗

            '終了ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:Err TagVCLID = NOTHING" _
                        , Me.GetType.ToString _
                        , MethodBase.GetCurrentMethod.Name))

            'エラーコードに-1を設定
            rowWebServiceResultRow.RESULTCODE = XmlErr

            Return rowWebServiceResultRow

        End If


        'ROW_LOCK_VERSIONタグの値取得
        rowWebServiceResultRow.ROW_LOCK_VERSION = GetTagValue(resultXmlNode, TagROWLOCKVERSION)

        'ROW_LOCK_VERSIONタグの値取得確認
        If rowWebServiceResultRow.ROW_LOCK_VERSION = XmlErr Then
            '処理結果が失敗

            '終了ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:Err TagROW_LOCK_VERSION = NOTHING" _
                        , Me.GetType.ToString _
                        , MethodBase.GetCurrentMethod.Name))

            'エラーコードに-1を設定
            rowWebServiceResultRow.RESULTCODE = XmlErr

            Return rowWebServiceResultRow

        End If


        '終了ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} OUT:RETURNCODE = {2}" _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name _
                    , rowWebServiceResultRow.RESULTCODE))

        Return rowWebServiceResultRow

    End Function

    ''' <summary>
    ''' Tagから値を取得
    ''' </summary>
    ''' <param name="resultXmlNode">受信XMLノード</param>
    ''' <param name="tagName">Tag名</param>
    ''' <returns>結果XML</returns>
    ''' <remarks></remarks>
    Private Function GetTagValue(ByVal resultXmlNode As XmlNode, _
                                 ByVal tagName As String) As Long

        '開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} IN:TAGNAME = {2}" _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name _
                    , tagName))

        '処理結果
        Dim resultValue As Long = XmlErr

        'タグの取得
        Dim selectNodeList As XmlNodeList = resultXmlNode.SelectNodes(tagName)

        'タグの確認
        If selectNodeList Is Nothing OrElse selectNodeList.Count = 0 Then
            '取得失敗

            'エラーログの出力
            Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:Err GET {2} VALUE = Nothing" _
                        , Me.GetType.ToString _
                        , MethodBase.GetCurrentMethod.Name _
                        , tagName))

            'コードに-1を設定
            Return XmlErr

        End If

        '値の取得
        Dim tagValue As String = selectNodeList.Item(0).InnerText.Trim

        '取得した値をLongに変換
        If Not Long.TryParse(tagValue, resultValue) Then
            'Longに変換できなかった場合

            'エラーログの出力
            Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:Err GET {2} = {3}" _
                        , Me.GetType.ToString _
                        , MethodBase.GetCurrentMethod.Name _
                        , tagName _
                        , tagValue))

            'コードに-1を設定
            Return XmlErr

        End If

        '終了ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} OUT:RESULTVALUE = {2}" _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name _
                    , resultValue))

        Return resultValue

    End Function

    '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
    ''' <summary>
    ''' Tagから値を取得(Decimal)
    ''' </summary>
    ''' <param name="resultXmlNode">受信XMLノード</param>
    ''' <param name="tagName">Tag名</param>
    ''' <returns>結果XML</returns>
    ''' <remarks></remarks>
    Private Function GetTagDecimalValue(ByVal resultXmlNode As XmlNode, _
                                 ByVal tagName As String) As Decimal

        '開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} IN:TAGNAME = {2}" _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name _
                    , tagName))

        '処理結果
        Dim resultValue As Decimal = -1

        'タグの取得
        Dim selectNodeList As XmlNodeList = resultXmlNode.SelectNodes(tagName)

        'タグの確認
        If selectNodeList Is Nothing OrElse selectNodeList.Count = 0 Then
            '取得失敗

            'エラーログの出力
            Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:Err GET {2} VALUE = Nothing" _
                        , Me.GetType.ToString _
                        , MethodBase.GetCurrentMethod.Name _
                        , tagName))

            'コードに-1を設定
            Return -1

        End If

        '値の取得
        Dim tagValue As String = selectNodeList.Item(0).InnerText.Trim

        '取得した値をDecimalに変換
        If Not Decimal.TryParse(tagValue, resultValue) Then
            'Longに変換できなかった場合

            'エラーログの出力
            Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:Err GET {2} = {3}" _
                        , Me.GetType.ToString _
                        , MethodBase.GetCurrentMethod.Name _
                        , tagName _
                        , tagValue))

            'コードに-1を設定
            Return -1

        End If

        '終了ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} OUT:RESULTVALUE = {2}" _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name _
                    , resultValue))

        Return resultValue

    End Function
    '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END


    '2014/3/5 TMEJ 陳 TMEJ次世代サービス 工程管理機能開発 START
    ''' <summary>
    ''' タイムアウト設定値を取得する
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetTimeOutValues() As String

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_Start", _
                                  MethodBase.GetCurrentMethod.Name))

        '戻り値
        Dim retValue As String = String.Empty

        'エラー発生フラグ
        Dim errorFlg As Boolean = False


        Try
            Using smbCommonBiz As New ServiceCommonClass.Api.BizLogic.ServiceCommonClassBusinessLogic

                '******************************
                '* システム設定から取得
                '******************************
                '基幹連携送信時タイムアウト値
                retValue = smbCommonBiz.GetSystemSettingValueBySettingName(SysLinkSendTimeOutVal)

                If String.IsNullOrEmpty(retValue) Then
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                               "{0}.Error , LINK_SEND_TIMEOUT_VAL does not exist.", _
                                               MethodBase.GetCurrentMethod.Name))
                    errorFlg = True
                    Exit Try
                End If

            End Using

        Finally

            If errorFlg Then
                retValue = String.Empty
            End If

        End Try

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_End", _
                                  MethodBase.GetCurrentMethod.Name))

        Return retValue

    End Function

    '2014/3/5 TMEJ 陳 TMEJ次世代サービス 工程管理機能開発 END

#End Region

End Class
