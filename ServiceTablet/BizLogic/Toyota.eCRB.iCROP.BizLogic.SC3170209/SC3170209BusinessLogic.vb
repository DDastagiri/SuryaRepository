'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3170209BusinessLogic.vb
'─────────────────────────────────────
'機能： RO作成機能グローバル連携処理
'補足： 追加作業サムネイル(追加作業)
'作成： 2013/12/03 SKFC 久代 橋本
'更新： 
'─────────────────────────────────────
Option Strict On
Option Explicit On

Imports System.Text
Imports System.IO
Imports System.Xml
Imports System.Web
Imports System.Net
Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Web.Controls
Imports Toyota.eCRB.iCROP.DataAccess.SC3170209

''' <summary>
''' SC3170209ビジネスロジッククラス
''' </summary>
''' <remarks></remarks>
Public Class SC3170209BusinessLogic
    Inherits BaseBusinessComponent

#Region "定数"

    ''' <summary>
    ''' 機能ID：ROサムネイル画像
    ''' </summary>
    Private Const C_FUNCTION_ID As String = "SC3170209"

    ''' <summary>
    ''' 機能ID：画像情報連携IF
    ''' </summary>
    Private Const C_WEBSERVICE_IMGINFO_FUNCTION_ID As String = "IC3A09924"

    ''' <summary>
    ''' 画像情報連携I/FのURLのキー値
    ''' </summary>
    Private Const C_IMAGEINFO_WEBSERVICE_URL As String = "ImageInfoWebServiceURL"

    ''' <summary>
    ''' 公開フォルダのURLのキー値
    ''' </summary>
    Private Const C_OPEN_DIRECTORY_URL As String = "OpenDirectoryURL"

    ''' <summary>
    ''' ファイル名フォーマットのキー値
    ''' </summary>
    Private Const C_FILE_NAME_FORMAT As String = "FileNameFormat"

    ''' <summary>
    ''' 画像情報連携I/Fの返却XMLタグ：小サイズイメージ
    ''' </summary>
    Private Const C_IMAGESIZE_XMLTAG As String = "SmallImage"

    ''' <summary>
    ''' ファイル拡張子の設定名
    ''' </summary>
    Private Const C_FILE_EXTENSION As String = "FILE_UPLOAD_EXTENSION"

#End Region

#Region "公開メソッド"

    ''' <summary>
    ''' ROサムネイル画像取得
    ''' </summary>
    ''' <param name="dlrCd">販売店コード</param>
    ''' <param name="brnCd">店舗コード</param>
    ''' <param name="visitSeq">来店実績連番</param>
    ''' <param name="basrezId">基幹予約ID</param>
    ''' <param name="roNum">R/ONO</param>
    ''' <param name="roSeqNum">R/O枝番</param>
    ''' <param name="vinNum">VIN</param>
    ''' <param name="pictureGroup">写真区分</param>
    ''' <param name="linkSysType"></param>
    ''' <returns>ROサムネイル画像データテーブル</returns>
    ''' <remarks></remarks>
    Public Function GetRoThumbnailImgInfo(ByVal dlrCd As String,
                                          ByVal brnCd As String,
                                          ByVal visitSeq As Long,
                                          ByVal basrezId As String,
                                          ByVal roNum As String,
                                          ByVal roSeqNum As Long,
                                          ByVal vinNum As String,
                                          ByVal pictureGroup As String,
                                          ByVal linkSysType As String) As SC3170209DataSet.TB_T_RO_THUMBNAILDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} Start [dlrCd:{1}][brnCd:{2}][visitSeq:{3}][basrezId:{4}][roNum:{5}]" & _
                                  "[roSeqNum:{6}][vinNum:{7}][pictureGroup:{8}][linkSysType:{9}]",
                                  System.Reflection.MethodBase.GetCurrentMethod.Name,
                                  dlrCd, brnCd, visitSeq, basrezId, roNum,
                                  roSeqNum, vinNum, pictureGroup, linkSysType))

        Dim dt As SC3170209DataSet.TB_T_RO_THUMBNAILDataTable

        ' ROサムネイル画像取得
        dt = SC3170209TableAdapter.getRoThumbnailImgInfo(dlrCd,
                                                         brnCd,
                                                         visitSeq,
                                                         basrezId,
                                                         roNum,
                                                         roSeqNum,
                                                         vinNum,
                                                         pictureGroup)

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} End [RowCount:{1}]",
                                  System.Reflection.MethodBase.GetCurrentMethod.Name,
                                  dt.Rows.Count))

        Return dt

    End Function


    ''' <summary>
    ''' ROサムネイル画像登録
    ''' </summary>
    ''' <param name="dlrCd"></param>
    ''' <param name="brnCd"></param>
    ''' <param name="visitSeq"></param>
    ''' <param name="basrezId"></param>
    ''' <param name="roNum"></param>
    ''' <param name="roSeqNum"></param>
    ''' <param name="vinNum"></param>
    ''' <param name="pictureGroup"></param>
    ''' <param name="loginUserId"></param>
    ''' <param name="thumbnailImgPath"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Function SetRoThumbnailImgInfo(ByVal roThumbnailId As Decimal,
                                          ByVal dlrCd As String,
                                          ByVal brnCd As String,
                                          ByVal visitSeq As Long,
                                          ByVal basrezId As String,
                                          ByVal roNum As String,
                                          ByVal roSeqNum As Long,
                                          ByVal vinNum As String,
                                          ByVal pictureGroup As String,
                                          ByVal loginUserId As String,
                                          ByVal thumbnailImgPath As String) As Boolean

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} Start [roThumbnailId:{1}][dlrCd:{2}][brnCd:{3}][visitSeq:{4}][basrezId:{5}][roNum:{6}]" & _
                                  "[roSeqNum:{7}][vinNum:{8}][pictureGroup:{9}][loginUserId:{10}][thumbnailImgPath:{11}]",
                                  System.Reflection.MethodBase.GetCurrentMethod.Name,
                                  roThumbnailId, dlrCd, brnCd, visitSeq, basrezId, roNum,
                                  roSeqNum, vinNum, pictureGroup, loginUserId, thumbnailImgPath))

        Dim result As Boolean

        'ROサムネイル画像登録
        result = SC3170209TableAdapter.setRoThumbnailImgInfo(roThumbnailId,
                                                             dlrCd,
                                                             brnCd,
                                                             visitSeq,
                                                             basrezId,
                                                             roNum,
                                                             roSeqNum,
                                                             vinNum,
                                                             pictureGroup,
                                                             thumbnailImgPath,
                                                             loginUserId)
        'いずれかが失敗時
        If Not result Then
            'ロールバック
            Me.Rollback = True

        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} End [IsSuccess:{1}]",
                                  System.Reflection.MethodBase.GetCurrentMethod.Name,
                                  result))

        Return result

    End Function


    ''' <summary>
    ''' 新ファイルパスを作成
    ''' </summary>
    ''' <returns>新ファイルパス</returns>
    ''' <remarks>公開フォルダ以下の保存先ディレクトリと拡張子を付与した新連番ファイルパスを作成する</remarks>
    Public Function CleateNewFilePath(ByRef roThumbnailId As Decimal) As String

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} Start",
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim filePath As String = String.Empty

        Dim clsTblAdapter As New SC3170209TableAdapter

        'ROサムネイルID取得
        roThumbnailId = SC3170209TableAdapter.getRoThumbnailId

        '連番を進めてファイル名を作成
        Dim fileName As String = CStr(roThumbnailId) & "_" & DateTime.Now.ToString("yyyyMMddHHmmssfff")

        '保存先ディレクトリと拡張子を含んだフォーマットをProgramSettingから取得
        'Dim format As String = clsTblAdapter.GetProgramSettingValue(C_FUNCTION_ID, , C_FILE_NAME_FORMAT)

        '保存先ディレクトリと拡張子を含んだフォーマットをSystemSettingから取得
        Dim format As String = clsTblAdapter.GetSystemSettingValue(C_FILE_EXTENSION)

        filePath = String.Format(format, fileName)

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} End [filePath :{1}]",
                                  System.Reflection.MethodBase.GetCurrentMethod.Name,
                                  filePath))

        Return filePath

    End Function


    ''' <summary>
    ''' 写真相対パスの取得
    ''' </summary>
    ''' <param name="aryFilePath">対象画像ファイルパス</param>
    ''' <remarks></remarks>
    Public Sub GetImageFileRelativePath(ByRef aryFilePath As ArrayList)

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} Start [filePathCount:{1}]",
                                  System.Reflection.MethodBase.GetCurrentMethod.Name,
                                  aryFilePath.Count))

        Dim aryRelativePath As New ArrayList

        Dim clsTblAdapter As New SC3170209TableAdapter

        '送信XML作成
        Dim imageInfoXml As String = CreateSendXML(aryFilePath)
        imageInfoXml = "xsData=" & imageInfoXml

        '画像情報連携I/FのURLはProgramSettingより取得
        Dim imageInfoURL As String = String.Empty
        Dim dtImageInfoURL As DataTable = clsTblAdapter.GetProgramSettingValues(C_FUNCTION_ID, , C_IMAGEINFO_WEBSERVICE_URL)

        If Not IsDBNull(dtImageInfoURL) Then imageInfoURL = dtImageInfoURL.Rows(0).Item("SETTING_VAL").ToString

        '画像情報連携I/Fを呼び出して返却XMLを取得
        Dim resultXmlDoc As XmlDocument = New XmlDocument
        resultXmlDoc.LoadXml(CallWebServiceSite(imageInfoXml, imageInfoURL))

        'ルート要素を取得する
        Dim rootElement As XmlElement = resultXmlDoc.DocumentElement

        'リザルトコードを判定
        If rootElement.GetElementsByTagName("ResultCode").Item(0).InnerText.Equals("0") Then

            '公開フォルダのURLはProgramSettingより取得
            Dim dtOpenDlr As DataTable = clsTblAdapter.GetProgramSettingValues(C_FUNCTION_ID, , C_OPEN_DIRECTORY_URL)
            Dim openDlr As String = dtOpenDlr.Rows(0).Item("SETTING_VAL").ToString

            '画像ファイルのパスを取得
            For Each imageNode As XmlNode In rootElement.GetElementsByTagName(C_IMAGESIZE_XMLTAG)
                'パスを合成して取得
                aryRelativePath.Add(Path.Combine(openDlr, imageNode.InnerText))
            Next

        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} End [retRelativePathCount:{1}]",
                                  System.Reflection.MethodBase.GetCurrentMethod.Name,
                                  aryRelativePath.Count))

        aryFilePath = aryRelativePath

    End Sub


    ''' <summary>
    ''' 基幹コード変換
    ''' </summary>
    ''' <param name="dlrCd"></param>
    ''' <param name="strCd"></param>
    ''' <remarks></remarks>
    Public Sub ChangeDmsCode(ByRef dlrCd As String, ByRef strCd As String)

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                          "{0} Start [dlrCd:{1}][strCd:{2}]",
                          System.Reflection.MethodBase.GetCurrentMethod.Name,
                          dlrCd, strCd))

        Dim clsTblAdapter As New SC3170209TableAdapter
        Dim dt As SC3170209DataSet.TB_M_DMS_CODE_MAPDataTable

        dt = clsTblAdapter.ChangeDlrStrCodeToICROP(dlrCd, strCd)

        For Each dr As SC3170209DataSet.TB_M_DMS_CODE_MAPRow In dt
            If Not String.IsNullOrEmpty(dr.ICROP_CD_2) Then
                dlrCd = dr.ICROP_CD_1
                strCd = dr.ICROP_CD_2
                Exit For
            End If
        Next

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                          "{0} End [dlrCd:{1}][strCd:{2}]",
                          System.Reflection.MethodBase.GetCurrentMethod.Name,
                          dlrCd, strCd))
    End Sub

    ''' <summary>
    ''' システム設定値取得
    ''' </summary>
    ''' <returns>セッティング値データ</returns>
    ''' <remarks>システム設定値を取得をする</remarks>
    Public Function GetSystemSettingValue() As String

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} Start",
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))


        Dim clsTblAdapter As New SC3170209TableAdapter


        Dim format As String = clsTblAdapter.GetSystemSettingValue(C_FILE_EXTENSION)


        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} End [format :{1}]",
                                  System.Reflection.MethodBase.GetCurrentMethod.Name,
                                  format))

        Return format

    End Function

#End Region

#Region "Privateメソット"

    ''' <summary>
    ''' 画像情報連携I/F送信用XML作成
    ''' </summary>
    ''' <param name="aryFilePath"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CreateSendXML(ByVal aryFilePath As ArrayList) As String

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} Start [filePathCount:{1}]",
                                  System.Reflection.MethodBase.GetCurrentMethod.Name,
                                  aryFilePath.Count))

        Dim xmlString As StringBuilder = New StringBuilder()

        xmlString.Append("<?xml version='1.0' encoding='UTF-8'?>")
        xmlString.Append("<ImageInfo>")
        xmlString.Append("  <Head>")
        xmlString.Append("      <MessageID>" & C_WEBSERVICE_IMGINFO_FUNCTION_ID & "</MessageID>")
        xmlString.Append("      <CountryCode>" & EnvironmentSetting.CountryCode & "</CountryCode>")
        xmlString.Append("      <LinkSystemCode>0</LinkSystemCode>")
        xmlString.Append("      <TransmissionDate>" & DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") & "</TransmissionDate>")
        xmlString.Append("  </Head>")
        xmlString.Append("  <Detail>")
        xmlString.Append("      <Common>")
        For Each filePath As String In aryFilePath
            xmlString.Append("          <Image_Path>" & filePath & "</Image_Path>")
        Next
        xmlString.Append("      </Common>")
        xmlString.Append("  </Detail>")
        xmlString.Append("</ImageInfo>")

        ' XMLドキュメントを生成
        Dim imageInfoXml As XmlDocument = New XmlDocument()

        imageInfoXml.LoadXml(xmlString.ToString)

        ' XMLドキュメントをString型に置き換えます。
        Dim imageInfoElement As XmlElement = imageInfoXml.DocumentElement
        Dim imageInfoString As String = imageInfoElement.OuterXml

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} End [imageInfoXml:{1}]",
                                  System.Reflection.MethodBase.GetCurrentMethod.Name,
                                  imageInfoString))

        Return imageInfoString

    End Function


    ''' <summary>
    ''' WebServiceのサイト呼び出し
    ''' </summary>
    ''' <param name="postData">送信文字列</param>
    ''' <param name="WebServiceUrl">送信先アドレス</param>
    ''' <returns>返却XML文字列</returns>
    ''' <remarks></remarks>
    Private Function CallWebServiceSite(ByVal postData As String, ByVal WebServiceUrl As String) As String

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} Start [WebServiceUrl:{1}]",
                                  System.Reflection.MethodBase.GetCurrentMethod.Name,
                                  WebServiceUrl))

        '文字コードを指定する
        Dim enc As System.Text.Encoding = System.Text.Encoding.GetEncoding("UTF-8")

        'バイト型配列に変換
        Dim postDataBytes As Byte() = System.Text.Encoding.UTF8.GetBytes(postData)

        'WebRequestの作成
        Dim req As System.Net.WebRequest = System.Net.WebRequest.Create(WebServiceUrl)

        'メソッドにPOSTを指定
        req.Method = "POST"

        'ContentTypeを"application/x-www-form-urlencoded"にする
        req.ContentType = "application/x-www-form-urlencoded"

        'POST送信するデータの長さを指定
        req.ContentLength = postDataBytes.Length

        'データをPOST送信するためのStreamを取得
        Dim reqStream As System.IO.Stream = req.GetRequestStream()

        '送信するデータを書き込む
        reqStream.Write(postDataBytes, 0, postDataBytes.Length)
        reqStream.Close()

        'サーバーからの応答を受信するためのWebResponseを取得
        Dim res As System.Net.WebResponse = req.GetResponse()

        '応答データを受信するためのStreamを取得
        Dim resStream As System.IO.Stream = res.GetResponseStream()

        '受信して表示
        Dim sr As New System.IO.StreamReader(resStream, enc)

        '返却文字列を取得
        Dim returnString As String = WebUtility.HtmlDecode(sr.ReadToEnd())

        '閉じる
        sr.Close()

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} End [returnString:{1}]",
                                  System.Reflection.MethodBase.GetCurrentMethod.Name,
                                  returnString))

        Return returnString

    End Function

#End Region

End Class
