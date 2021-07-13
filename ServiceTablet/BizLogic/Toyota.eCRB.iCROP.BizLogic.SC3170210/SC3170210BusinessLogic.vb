'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3170210BusinessLogic.vb
'─────────────────────────────────────
'機能： RO作成機能グローバル連携処理
'補足： 追加作業サムネイル(追加作業)
'作成： 2013/12/25 SKFC 久代
'更新： 
'─────────────────────────────────────
Option Strict On
Option Explicit On

Imports System.Text
Imports System.Net
Imports System.Xml
Imports System.IO
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.iCROP.DataAccess.SC3170210
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic

''' <summary>
''' ビジネスロジッククラス
''' </summary>
''' <remarks></remarks>
Public Class SC3170210BusinessLogic
    Inherits BaseBusinessComponent

#Region "定数"
    ''' <summary>
    ''' POST要求
    ''' </summary>
    ''' <remarks></remarks>
    Private Const _POST_METHOD As String = "POST"

    ''' <summary>
    ''' POST要求のcontent-type
    ''' </summary>
    ''' <remarks></remarks>
    Private Const _CONTENT_TYPE As String = "application/x-www-form-urlencoded"

    ''' <summary>
    ''' LinkSysTypeの基幹コード使用
    ''' </summary>
    ''' <remarks></remarks>
    Private Const _LINK_SYS_TYPE_DMS As String = "0"

    ''' <summary>
    ''' プログラム設定セクション値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const _PROGRAM_SETTING_SECTION As String = "ThumbnalSelect"

    ''' <summary>
    ''' プログラム設定キー値:画像URLパス
    ''' </summary>
    ''' <remarks></remarks>
    Private Const _PROGRAM_SETTING_KEY_IMGURL As String = "OpenDirectoryURL"

    ''' <summary>
    ''' プログラム設定キー値：WebServiceドメイン名
    ''' </summary>
    ''' <remarks></remarks>
    Private Const _PROGRAM_SETTING_KEY_DOMAIN_URL As String = "WebServiceDomainURL"

    ''' <summary>
    ''' 画像情報連携I/F(ドメイン名無し)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const _WEB_SERVICE_GET_IMAGE_INFO As String = "A0/IC3A09924.asmx/GetImageInfo"

    ''' <summary>
    ''' ステータスコード:正常
    ''' </summary>
    ''' <remarks></remarks>
    Private Const _RESULT_STATUS_OK As String = "0"

    ''' <summary>
    ''' 画像情報連携I/F 戻り値タグ名
    ''' </summary>
    ''' <remarks></remarks>
    Private Const _XML_NODE_RESULT_CODE As String = "ResultCode"

    ''' <summary>
    ''' 画像情報連携I/F 出力パスデータノード名
    ''' </summary>
    ''' <remarks></remarks>
    Private Const _XML_NODE_OUTPUT_IMAGEPATH As String = "Output_ImagePath"

    ''' <summary>
    ''' 画像情報連携I/F 出力パス戻り値タグ名
    ''' </summary>
    ''' <remarks></remarks>
    Private Const _XML_NODE_IMAGE_RESULT_CODE As String = "ImageResultCode"

    ''' <summary>
    ''' 画像情報連携I/F 画像パス(入力値)タグ名
    ''' </summary>
    ''' <remarks></remarks>
    Private Const _XML_NODE_IMAGE_PATH As String = "ImagePath"

    ''' <summary>
    ''' 画像情報連携I/F オリジナル画像ファイルタグ名
    ''' </summary>
    ''' <remarks></remarks>
    Private Const _XML_NODE_ORIGNAL_IMAGE_PATH As String = "OriginalImage"

    ''' <summary>
    ''' 画像情報連携I/F ラージ画像ファイルタグ名
    ''' </summary>
    ''' <remarks></remarks>
    Private Const _XML_NODE_LARGE_IMAGE_PATH As String = "LargeImage"

    ''' <summary>
    ''' 画像情報連携I/F ミドル画像ファイルタグ名
    ''' </summary>
    ''' <remarks></remarks>
    Private Const _XML_NODE_MIDDLE_IMAGE_PATH As String = "MiddleImage"

    ''' <summary>
    ''' 画像情報連携I/F スモール画像ファイルタグ名
    ''' </summary>
    ''' <remarks></remarks>
    Private Const _XML_NODE_SMALL_IMAGE_PATH As String = "SmallImage"

    Private Const _RO_EXTERIOR_PROGRAM_ID As String = "SC3160219"
#End Region
#Region "DTOクラス"
    ''' <summary>
    ''' DTOクラス
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ThumbnailData
        ''' <summary>
        ''' RO_THUMBNAIL_ID
        ''' </summary>
        ''' <remarks></remarks>
        Public id As Decimal = 0

        ''' <summary>
        ''' 画像への参照パス(DB値)
        ''' </summary>
        ''' <remarks></remarks>
        Public dbImgPath As String = ""

        ''' <summary>
        ''' オリジナル画像パス
        ''' </summary>
        ''' <remarks></remarks>
        Public orignalImgPath As String = ""

        ''' <summary>
        ''' ラージー画像パス
        ''' </summary>
        ''' <remarks></remarks>
        Public largeImgPath As String = ""

        ''' <summary>
        ''' ミドル画像パス
        ''' </summary>
        ''' <remarks></remarks>
        Public middleImgPath As String = ""

        ''' <summary>
        ''' スモール画像パス
        ''' </summary>
        ''' <remarks></remarks>
        Public smallImgPath As String = ""

        ''' <summary>
        ''' 部位名称
        ''' </summary>
        ''' <remarks></remarks>
        Public partsTitle As String = ""
    End Class
#End Region

#Region "公開メソッド"
    ''' <summary>
    ''' ROサムネイル情報データ取得
    ''' </summary>
    ''' <param name="DealerCode">販売店コード</param>
    ''' <param name="BranchCode">店舗コード</param>
    ''' <param name="SAChipId">来店実績連番</param>
    ''' <param name="R_O">RO番号</param>
    ''' <param name="SEQ_NO">RO枝番</param>
    ''' <param name="PictMode">写真区分</param>
    ''' <param name="LinkSysType">LinkSysType</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetRoThumbnail(ByVal DealerCode As String,
                                          ByVal BranchCode As String,
                                          ByVal SAChipId As Long,
                                          ByVal R_O As String,
                                          ByVal SEQ_NO As Long,
                                          ByVal PictMode As String,
                                          ByVal LinkSysType As String) As ArrayList
        Logger.Info("SC3170210BusinessLogic.GetRoThumbnail Function Start.")
        Dim resultList As ArrayList = New ArrayList
        Dim dlrCd As String = DealerCode
        Dim strCd As String = BranchCode

        '基幹コード変換
        If LinkSysType.Equals(_LINK_SYS_TYPE_DMS) Then
            Dim resultIcropCd As SC3170210DataSet.TB_M_DMS_CODE_MAPDataTable
            resultIcropCd = SC3170210TableAdapter.ChangeDlrStrCodeToICROP(DealerCode, BranchCode)
            If 1 <> resultIcropCd.Count Then
                Logger.Info("DLR_CD,STR_CD change faild.")
                Logger.Info("SC3170210TableAdapter.GetRoThumbnail function End.")
                Return resultList
            End If

            dlrCd = resultIcropCd.Item(0).ICROP_CD_1
            strCd = resultIcropCd.Item(0).ICROP_CD_2
        End If

        Dim ds As SC3170210DataSet.TB_T_RO_THUMBNAILDataTable
        ds = SC3170210TableAdapter.GetRoThumbnail(dlrCd, strCd, SAChipId, R_O, SEQ_NO, PictMode)

        If 0 < ds.Count Then
            For i As Integer = 0 To ds.Count - 1
                Dim data As ThumbnailData = New ThumbnailData

                ' サムネイルID
                data.id = ds.Item(i).RO_THUMBNAIL_ID
                ' パス
                data.dbImgPath = ds.Item(i).THUMBNAIL_IMG_PATH

                '部品名称
                data.partsTitle = ""
                Dim partWordNum As Decimal
                partWordNum = SC3170210TableAdapter.GetPartsTitle(ds.Item(i).RO_THUMBNAIL_ID)
                If 0 <= partWordNum Then
                    data.partsTitle = WebWordUtility.GetWord(_RO_EXTERIOR_PROGRAM_ID, partWordNum)
                End If

                resultList.Add(data)
            Next
        End If

        '画像情報連携I/Fで画像パス取得
        Dim reqDomain As String = SC3170210TableAdapter.GetProgramSetting(_PROGRAM_SETTING_SECTION, _PROGRAM_SETTING_KEY_DOMAIN_URL)
        Dim xmlString As String = _CreateRequestXmlString(resultList)

        Dim responseXml As XmlDocument = New XmlDocument
        responseXml.LoadXml(_CallWebService(xmlString, reqDomain & _WEB_SERVICE_GET_IMAGE_INFO))

        'パス生成
        _GenerateImagePath(responseXml, resultList)

        Logger.Info("SC3170210BusinessLogic.GetRoThumbnail Function End.")
        Return resultList
    End Function

    ''' <summary>
    ''' ROサムネイル情報データ削除
    ''' </summary>
    ''' <param name="RO_THUMBNAIL_ID">ROサムネイルID</param>
    ''' <param name="LoginUserID">更新ユーザ</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function DeleteRoThubnail(ByVal RO_THUMBNAIL_ID As Decimal,
                                            ByVal LoginUserID As String) As Integer
        Logger.Info("SC3170210BusinessLogic.DeleteRoThubnail Function Start.")

        Dim result As Integer = SC3170210TableAdapter.DeleteRoThumbnail(RO_THUMBNAIL_ID, LoginUserID)

        Logger.Info("SC3170210BusinessLogic.DeleteRoThubnail Function End.")
        Return result
    End Function
#End Region

#Region "非公開メソッド"
    ''' <summary>
    ''' 画像情報連携I/F送信用XML文字列作成
    ''' </summary>
    ''' <param name="list"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Shared Function _CreateRequestXmlString(ByVal list As ArrayList) As String
        Logger.Info("SC3170210BusinessLogic._CreateRequestXmlString Function Start.")

        Dim xmlString As StringBuilder = New StringBuilder()
        xmlString.Append("<?xml version='1.0' encoding='UTF-8'?>")
        xmlString.Append("<ImageInfo>")
        xmlString.Append("  <Head>")
        xmlString.Append("    <MessageID>IC3A09924</MessageID>")
        xmlString.Append("    <CountryCode>" & EnvironmentSetting.CountryCode & "</CountryCode>")
        xmlString.Append("    <LinkSystemCode>0</LinkSystemCode>")
        xmlString.Append("    <TransmissionDate>" & DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") & "</TransmissionDate>")
        xmlString.Append("  </Head>")
        xmlString.Append("  <Detail>")
        xmlString.Append("    <Common>")
        For Each dat As ThumbnailData In list
            xmlString.Append("      <Image_Path>" & dat.dbImgPath & "</Image_Path>")
        Next
        xmlString.Append("    </Common>")
        xmlString.Append("  </Detail>")
        xmlString.Append("</ImageInfo>")

        ' XMLドキュメントを生成
        Dim xml As XmlDocument = New XmlDocument()
        xml.LoadXml(xmlString.ToString)

        ' XMLドキュメントをString型に置き換えます。
        Dim result As String = "xsData=" & xml.DocumentElement.OuterXml

        Logger.Info("SC3170210BusinessLogic._CreateRequestXmlString Function End.")

        Return result
    End Function

    ''' <summary>
    ''' WebServiceのサイト呼び出し
    ''' </summary>
    ''' <param name="postData">送信文字列</param>
    ''' <param name="WebServiceUrl">送信先アドレス</param>
    ''' <returns>返却XML文字列</returns>
    ''' <remarks></remarks>
    Private Shared Function _CallWebService(ByVal postData As String,
                                            ByVal WebServiceUrl As String) As String
        Logger.Info("SC3170210BusinessLogic._CallWebService Function Start.")
        Logger.Info(postData)
        Logger.Info(WebServiceUrl)

        '文字コードを指定する
        Dim enc As System.Text.Encoding = Encoding.GetEncoding("UTF-8")

        'バイト型配列に変換
        Dim postDataBytes As Byte() = Encoding.UTF8.GetBytes(postData)

        'WebRequestの作成
        Dim req As WebRequest = WebRequest.Create(WebServiceUrl)

        'メソッドにPOSTを指定
        req.Method = _POST_METHOD

        'ContentTypeを"application/x-www-form-urlencoded"にする
        req.ContentType = _CONTENT_TYPE

        'POST送信するデータの長さを指定
        req.ContentLength = postDataBytes.Length

        'データをPOST送信するためのStreamを取得
        Dim reqStream As System.IO.Stream = req.GetRequestStream()

        '送信するデータを書き込む
        reqStream.Write(postDataBytes, 0, postDataBytes.Length)
        reqStream.Close()

        'サーバーからの応答を受信するためのWebResponseを取得
        Dim res As WebResponse
        Try
            res = req.GetResponse()
        Catch e As WebException
            Logger.Info(e.Message)
            Logger.Info("SC3170210BusinessLogic._CallWebService Function End.")
            Return ""
        End Try

        '応答データを受信するためのStreamを取得
        Dim resStream As System.IO.Stream = res.GetResponseStream()

        '受信して表示
        Dim returnString As String = ""
        Using response As New System.IO.StreamReader(resStream, enc)

            '返却文字列を取得
            returnString = WebUtility.HtmlDecode(response.ReadToEnd())
        End Using

        Logger.Info("SC3170210BusinessLogic._CallWebService Function End.")

        Return returnString

    End Function

    ''' <summary>
    ''' 画像へのパス生成
    ''' </summary>
    ''' <param name="resXml"></param>
    ''' <param name="pathList"></param>
    ''' <remarks></remarks>
    Private Shared Sub _GenerateImagePath(ByVal resXml As XmlDocument,
                                          ByRef pathList As ArrayList)
        Logger.Info("SC3170210BusinessLogic._GenerateImagePath Function Start.")
        'ルート要素を取得する
        Dim rootNode As XmlElement = resXml.DocumentElement

        'リザルトコードを判定
        If rootNode.GetElementsByTagName(_XML_NODE_RESULT_CODE).Item(0).InnerText.Equals(_RESULT_STATUS_OK) Then

            '公開フォルダのURLはProgramSettingより取得
            Dim imageUrlPath As String = SC3170210TableAdapter.GetProgramSetting(_PROGRAM_SETTING_SECTION,
                                                                                 _PROGRAM_SETTING_KEY_IMGURL)

            '画像ファイルのパスを取得
            For Each node As XmlNode In rootNode.GetElementsByTagName(_XML_NODE_OUTPUT_IMAGEPATH)
                If node(_XML_NODE_IMAGE_RESULT_CODE).InnerText.Equals(_RESULT_STATUS_OK) Then
                    For Each data As ThumbnailData In pathList
                        If node(_XML_NODE_IMAGE_PATH).InnerText.Equals(data.dbImgPath) Then
                            data.orignalImgPath = imageUrlPath & node(_XML_NODE_ORIGNAL_IMAGE_PATH).InnerText
                            data.largeImgPath = imageUrlPath & node(_XML_NODE_LARGE_IMAGE_PATH).InnerText
                            data.middleImgPath = imageUrlPath & node(_XML_NODE_MIDDLE_IMAGE_PATH).InnerText
                            data.smallImgPath = imageUrlPath & node(_XML_NODE_SMALL_IMAGE_PATH).InnerText
                            Exit For
                        End If
                    Next
                End If
            Next
        End If

        Logger.Info("SC3170210BusinessLogic._GenerateImagePath Function End.")
    End Sub
#End Region

End Class
