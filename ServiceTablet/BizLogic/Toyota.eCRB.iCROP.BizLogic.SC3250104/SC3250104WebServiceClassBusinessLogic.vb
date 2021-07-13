'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3250101WebServiceClassBusinessLogic.vb
'─────────────────────────────────────
'機能： 商品訴求メイン（車両）WEBサービス用ビジネスロジック
'補足： 
'作成： 2014/02/XX NEC 鈴木
'更新： 2014/03/xx NEC 上野
'更新： 2014/04/xx NEC 脇谷
'─────────────────────────────────────

Option Explicit On
Option Strict On

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
Imports Toyota.eCRB.iCROP.BizLogic.SC3250104.SC3250104WebServiceClassBusinessLogic_CreateXml
Imports Toyota.eCRB.iCROP.DataAccess.SC3250104.SC3250104DataSet

Imports System.Runtime.Serialization

Public Class SC3250104WebServiceClassBusinessLogic
    Inherits BaseBusinessComponent
    Implements IDisposable

#Region "IDisposable Support"
    Private disposedValue As Boolean ' 重複する呼び出しを検出するには

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: マネージ状態を破棄します (マネージ オブジェクト)。
            End If

            ' TODO: アンマネージ リソース (アンマネージ オブジェクト) を解放し、下の Finalize() をオーバーライドします。
            ' TODO: 大きなフィールドを null に設定します。
        End If
        Me.disposedValue = True
    End Sub

    ' TODO: 上の Dispose(ByVal disposing As Boolean) にアンマネージ リソースを解放するコードがある場合にのみ、Finalize() をオーバーライドします。
    'Protected Overrides Sub Finalize()
    '    ' このコードを変更しないでください。クリーンアップ コードを上の Dispose(ByVal disposing As Boolean) に記述します。
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' このコードは、破棄可能なパターンを正しく実装できるように Visual Basic によって追加されました。
    Public Sub Dispose() Implements IDisposable.Dispose
        ' このコードを変更しないでください。クリーンアップ コードを上の Dispose(disposing As Boolean) に記述します。
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

#Region "定数"

    ''' <summary>
    ''' 成功
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ResultSuccess As Long = 0

    ''' <summary>
    ''' XML戻り値成功
    ''' </summary>
    ''' <remarks></remarks>
    Public Const XmlSuccess As String = "0"


    ''' <summary>
    ''' XML戻り値解析失敗
    ''' </summary>
    ''' <remarks></remarks>
    Public Const XmlErr As String = "-1"

    ''' <summary>
    ''' WebService(IC3A09922)引数名
    ''' </summary>
    ''' <remarks></remarks>
    Public Const WebServiceArgument As String = "xsData="

    ''' <summary>
    ''' WebService ヘッダーコメント削除置換用文字列
    ''' </summary>
    ''' <remarks></remarks>
    Public Const XmlReplace As String = "<?xml version=""1.0"" encoding=""utf-16""?>"

    ''' <summary>
    ''' エンコード(UTF-8)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const EncodingUTF8 As String = "UTF-8"

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

    Public Class GetServiceItems_Info
        ''' <summary>
        ''' WebService名(IC3A09922)
        ''' </summary>
        ''' <remarks></remarks>
        Public Const WebServiceIDValue As String = "IC3A09920"

        ''' <summary>
        ''' WebService(IC3A09920)メソッド名
        ''' </summary>
        ''' <remarks></remarks>
        Public Const WebServiceMethod As String = "IC3A09920"

        ''' <summary>
        ''' WebServiceURL(IC3A09922)
        ''' </summary>
        ''' <remarks></remarks>
        Public Const WebServiceURL As String = "LINK_URL_SERVICE_ITEMS"


        Public Class Response
            ''' <summary>
            ''' Node名(Response)
            ''' </summary>
            ''' <remarks></remarks>
            Public Const NodeResponse As String = "Response"

            ''' <summary>
            ''' Node名(Head)
            ''' </summary>
            ''' <remarks></remarks>
            Public Const NodeHead As String = "Head"

            ''' <summary>
            ''' Tag名(MessageID)
            ''' </summary>
            ''' <remarks></remarks>
            Public Const TagMessageID As String = "MessageID"

            ''' <summary>
            ''' Tag名(ReceptionDate)
            ''' </summary>
            ''' <remarks></remarks>
            Public Const TagReceptionDate As String = "ReceptionDate"

            ''' <summary>
            ''' Tag名(TransmissionDate)
            ''' </summary>
            ''' <remarks></remarks>
            Public Const TagTransmissionDate As String = "TransmissionDate"

            ''' <summary>
            ''' Node名(Detail)
            ''' </summary>
            ''' <remarks></remarks>
            Public Const NodeDetail As String = "Detail"

            ''' <summary>
            ''' Node名(Common)
            ''' </summary>
            ''' <remarks></remarks>
            Public Const NodeCommon As String = "Common"

            ''' <summary>
            ''' Tag名(TransmissionDate)
            ''' </summary>
            ''' <remarks></remarks>
            Public Const TagResultId As String = "ResultId"

            ''' <summary>
            ''' Tag名(Message)
            ''' </summary>
            ''' <remarks></remarks>
            Public Const TagMessage As String = "Message"
        End Class

    End Class

    Public Class GetMileage_Info
        ''' <summary>
        ''' WebService名(IC3A09921)
        ''' </summary>
        ''' <remarks></remarks>
        Public Const WebServiceIDValue As String = "IC3A09921"

        ''' <summary>
        ''' WebService(IC3A09921)メソッド名
        ''' </summary>
        ''' <remarks></remarks>
        Public Const WebServiceMethod As String = "IC3A09921"


        ''' <summary>
        ''' WebServiceURL(IC3A09922)
        ''' </summary>
        ''' <remarks></remarks>
        Public Const WebServiceURL As String = "LINK_URL_MILE"
        'Public Const WebServiceURL As String = "LINK_URL_MILEAGE_IN"

        Public Class Response

            ''' <summary>
            ''' Node名(Mileage_Result)
            ''' </summary>
            ''' <remarks></remarks>
            Public Const NodeMileage_Result As String = "Mileage_Result"

            ''' <summary>
            ''' Tag名(ResultCode)
            ''' </summary>
            ''' <remarks></remarks>
            Public Const TagResultCode As String = "/Mileage_Result/ResultCode"

            ''' <summary>
            ''' Node名(Output_Mileage)
            ''' </summary>
            ''' <remarks></remarks>
            Public Const NodeOutput_Mileage As String = "/Mileage_Result/Output_Mileage"

            ''' <summary>
            ''' Tag名(DealerCode)
            ''' </summary>
            ''' <remarks></remarks>
            Public Const TagDealerCode As String = "DealerCode"

            ''' <summary>
            ''' Tag名(BranchCode)
            ''' </summary>
            ''' <remarks></remarks>
            Public Const TagBranchCode As String = "BranchCode"

            ''' <summary>
            ''' Tag名(R_O)
            ''' </summary>
            ''' <remarks></remarks>
            Public Const TagR_O As String = "R_O"

            ''' <summary>
            ''' Tag名(BASREZID)
            ''' </summary>
            ''' <remarks></remarks>
            Public Const TagBASREZID As String = "BASREZID"

            ''' <summary>
            ''' Tag名(SAChipID)
            ''' </summary>
            ''' <remarks></remarks>
            Public Const TagSAChipID As String = "SAChipID"

            ''' <summary>
            ''' Tag名(Mileage)
            ''' </summary>
            ''' <remarks></remarks>
            Public Const TagMileage As String = "Mileage"

            ''' <summary>
            ''' Tag名(DealerCode)
            ''' </summary>
            ''' <remarks></remarks>
            Public Const TagMileageSource As String = "MileageSource"

            ''' <summary>
            ''' Tag名(DealerCode)
            ''' </summary>
            ''' <remarks></remarks>
            Public Const TagMileageDate As String = "MileageDate"
        End Class
    End Class

    Public Class GetRoThumbnailCount_Info
        ''' <summary>
        ''' WebService名(IC3A09917)
        ''' </summary>
        ''' <remarks></remarks>
        Public Const WebServiceIDValue As String = "IC3A09917"

        ''' <summary>
        ''' WebService(IC3A09917)メソッド名
        ''' </summary>
        ''' <remarks></remarks>
        Public Const WebServiceMethod As String = "GetRoThumbnailCount" 'GetRoThumbnailCount

        ''' <summary>
        ''' WebServiceURL(IC3A09922)
        ''' </summary>
        ''' <remarks></remarks>
        Public Const WebServiceURL As String = "LINK_URL_IMG_COUNT"
        'Public Const WebServiceURL As String = "LINK_URL_RO_THIMBNAILCOUNT_INFO"


        Public Class Response
            ''' <summary>
            ''' Node名(RoThumbnailCount_Result)
            ''' </summary>
            ''' <remarks></remarks>
            Public Const NodeRoThumbnailCount_Result As String = "RoThumbnailCount_Result"

            ''' <summary>
            ''' Tag名(ResultCode)
            ''' </summary>
            ''' <remarks></remarks>
            Public Const TagResultCode As String = "ResultCode"

            ''' <summary>
            ''' Node名(Output_RoThumbnailCount)
            ''' </summary>
            ''' <remarks></remarks>
            Public Const Output_RoThumbnailCount As String = "Output_RoThumbnailCount"

            ''' <summary>
            ''' Tag名(ReceptionDate)
            ''' </summary>
            ''' <remarks></remarks>
            Public Const TagReceptionDate As String = "ReceptionDate"

            ''' <summary>
            ''' Tag名(TransmissionDate)
            ''' </summary>
            ''' <remarks></remarks>
            Public Const TagRoThumbnailCount As String = "RoThumbnailCount"
        End Class

    End Class

    ''' <summary>
    ''' 返却結果コード
    ''' </summary>
    Private Enum ReturnCode

        ''' <summary>
        ''' 成功
        ''' </summary>
        Success = 0

        ''' <summary>
        ''' DBタイムアウト
        ''' </summary>
        ErrTimeout = 6001

        ''' <summary>
        ''' DMS側エラー発生
        ''' </summary>
        ErrDms = 6002

        ''' <summary>
        ''' その他エラー
        ''' </summary>
        ErrOther = 6003

    End Enum

    'Public ReadOnly Property WebServiceID As String
    '    Get
    '        Return WebServiceIDValue
    '    End Get
    'End Property

#End Region

#Region "Public"

    ''' <summary>
    ''' 写真枚数取得
    ''' </summary>
    ''' <param name="inXmlClass">XML作成用クラス</param>
    ''' <returns>WebService処理結果。Nothingの場合はXML解析エラー発生する原因となる</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Public Function CallGetRoThumbnailCountWebService(ByVal inXmlClass As RoThumbnailCountXmlDocumentClass) As RoThumbnailCountDataTable

        '開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} " _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name))

        'DMSコード転換
        '2014/03/11　DMS変換不要のため、コメントアウト
        'inXmlClass = Me.Change2DMSXMLOfRoThumbnailCount(inXmlClass)

        'XML戻り値用DataTable
        Dim dtWebServiceResult As New RoThumbnailCountDataTable

        'XML戻り値用DataRow
        Dim rowWebServiceResult As RoThumbnailCountRow = DirectCast(dtWebServiceResult.NewRow, RoThumbnailCountRow)

        Try
            'WebServiceURLの取得
            Dim envSettingRow As String = String.Empty
            '2014/05/23 「ServiceCommonClassBusinessLogic」の使用廃止　START　↓↓↓
            'Using biz As New ServiceCommonClassBusinessLogic
            Using biz As New SC3250104BusinessLogic
                envSettingRow = biz.GetDlrSystemSettingValueBySettingName(GetRoThumbnailCount_Info.WebServiceURL)
            End Using
            '2014/05/23 「ServiceCommonClassBusinessLogic」の使用廃止　　END　↑↑↑

            'URLの取得確認
            If envSettingRow Is Nothing Then
                'URL取得失敗

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} DlrEnvSetting == NOTHING OUT:resultXmlValue.ResultCode = {2}" _
                        , Me.GetType.ToString _
                        , MethodBase.GetCurrentMethod.Name _
                        , rowWebServiceResult("ResultCode")))

                Return dtWebServiceResult

            End If

            'WebServiceのURLを作成
            'Dim createUrl As String = String.Concat(envSettingRow, "/", GetRoThumbnailCount_Info.WebServiceMethod)
            Dim createUrl As String = envSettingRow
            'If Not envSettingRow.EndsWith(GetRoThumbnailCount_Info.WebServiceMethod) Then
            '    createUrl += "/" + GetRoThumbnailCount_Info.WebServiceMethod
            '    'Logger.Info("★写真枚数：WebServiceのURLにメソッド追加：" & GetRoThumbnailCount_Info.WebServiceMethod)
            'End If
            Logger.Info(String.Format("RoThumbnailCountWebServiceURL:[{0}]", createUrl))

            'WebService送信用XML作成処理
            'Dim sendXml As String = CreateXmlOfRoThumbnailCount(inXmlClass, GetRoThumbnailCount_Info.WebServiceMethod)
            Dim sendXml As String = CreateXmlOfRoThumbnailCount(inXmlClass, "IC3A09917")

            'XMLのヘッダー部分を削除
            sendXml = sendXml.Replace(XmlReplace, String.Empty)

            '送信XMLをエンコードし引数に指定
            sendXml = String.Concat(WebServiceArgument, HttpUtility.UrlEncode(sendXml))

            'WebService送受信処理
            Dim resultString As String = CallWebServiceSite(sendXml, createUrl)

            '返却された文字列をデコード
            resultString = HttpUtility.HtmlDecode(resultString)

            'XML名前空間用の正規表現設定
            Dim regex As Regex = New Regex(" xmln.*=""[^""]*"".")

            'XML名前空間を除去
            resultString = regex.Replace(resultString, Space(0))

            'WebServiceの戻りXMLを解析し値を取得

            '2014/06/11 応答XMLの戻り値解析追加と文字列で解析　START　↓↓↓
            Dim xml As Xml.XmlDocument = New Xml.XmlDocument
            xml.LoadXml(resultString)
            Dim retCD As String = xml.SelectSingleNode(String.Format("/{0}/{1}" _
                                                                     , GetRoThumbnailCount_Info.Response.NodeRoThumbnailCount_Result _
                                                                     , GetRoThumbnailCount_Info.Response.TagResultCode _
                                                                     )).InnerText
            '正常終了か？
            Dim ReturnCount As String = String.Empty
            If retCD = XmlSuccess Then
                '正常終了　→　Output_RoThumbnailCountタグを解析
                Dim nodes As Xml.XmlNodeList = xml.SelectNodes(String.Format("/{0}/{1}" _
                                                                             , GetRoThumbnailCount_Info.Response.NodeRoThumbnailCount_Result _
                                                                             , GetRoThumbnailCount_Info.Response.Output_RoThumbnailCount _
                                                                             ))
                Dim nd As Xml.XmlNode = nodes(0)
                ReturnCount = nd.SelectSingleNode(GetRoThumbnailCount_Info.Response.TagRoThumbnailCount).InnerText
            Else
                '異常
                ReturnCount = XmlErr
            End If

            rowWebServiceResult("ResultCode") = retCD
            rowWebServiceResult("RoThumbnailCount") = ReturnCount
            'rowWebServiceResult("RoThumbnailCount") = GetRoThumbnailCountFromXMLData(resultString, rowWebServiceResult)
            '2014/06/11 応答XMLの戻り値解析追加と文字列で解析　END　　↑↑↑

            dtWebServiceResult.AddRoThumbnailCountRow(rowWebServiceResult)
            '終了ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:resultXmlValue.RoThumbnailCount = {2}" _
                        , Me.GetType.ToString _
                        , MethodBase.GetCurrentMethod.Name _
                        , rowWebServiceResult("RoThumbnailCount")))

            Return dtWebServiceResult

        Catch ex As System.Net.WebException
            'WebServiceエラー

            Logger.Error(String.Format(CultureInfo.CurrentCulture _
                         , "{0}.{1} OUT:ErrWebService = {2}" _
                         , Me.GetType.ToString _
                         , MethodBase.GetCurrentMethod.Name _
                         , ex.ToString))

            If ex.Status = WebExceptionStatus.Timeout Then

                Dim drTemp As RoThumbnailCountRow = DirectCast(dtWebServiceResult.NewRow, RoThumbnailCountRow)
                drTemp("ResultCode") = ReturnCode.ErrTimeout

                dtWebServiceResult.Rows.Add(drTemp)

            Else

                Dim drTemp As RoThumbnailCountRow = DirectCast(dtWebServiceResult.NewRow, RoThumbnailCountRow)
                drTemp("ResultCode") = ReturnCode.ErrOther

                dtWebServiceResult.Rows.Add(drTemp)

            End If

            Return dtWebServiceResult

        Catch ex2 As System.Exception

            Logger.Error(String.Format(CultureInfo.CurrentCulture _
                         , "{0}.{1} OUT:ErrWebService = {2}" _
                         , Me.GetType.ToString _
                         , MethodBase.GetCurrentMethod.Name _
                         , ex2.ToString))

            Dim drTemp As RoThumbnailCountRow = DirectCast(dtWebServiceResult.NewRow, RoThumbnailCountRow)
            drTemp("ResultCode") = ReturnCode.ErrOther

            dtWebServiceResult.Rows.Add(drTemp)

            Return dtWebServiceResult

        Finally
            '終了ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:RETURNCODE = {2}" _
                        , Me.GetType.ToString _
                        , MethodBase.GetCurrentMethod.Name _
                        , inXmlClass))
        End Try

    End Function

#End Region

#Region "XML作成"

    ''' <summary>
    ''' 写真枚数：XML作成(メイン)
    ''' </summary>
    ''' <param name="inXmlClass">XML作成用クラス</param>
    ''' <returns>XMLString</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Function CreateXmlOfRoThumbnailCount(ByVal inXmlClass As RoThumbnailCountXmlDocumentClass, ByVal WebServiceID As String) As String

        ''開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} " _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name))


        'XMLのHeadTagの作成処理
        inXmlClass = CreateHeadTagOfRoThumbnailCount(inXmlClass, WebServiceID)

        'テキストWriter
        Using writer As New StringWriter(CultureInfo.InvariantCulture)

            'XMLシリアライザー型の設定
            Dim serializer As New XmlSerializer(GetType(RoThumbnailCountXmlDocumentClass))

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
    ''' 写真枚数：XML作成(HeadTag)
    ''' </summary>
    ''' <param name="inXmlClass">XML作成用クラス</param>
    ''' <returns>XML作成用クラス</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Function CreateHeadTagOfRoThumbnailCount(ByVal inXmlClass As RoThumbnailCountXmlDocumentClass, ByVal WebServiceID As String) As RoThumbnailCountXmlDocumentClass

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

        '送信タイムアウト設定(10秒)
        req.Timeout = 10000

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

#End Region

End Class
