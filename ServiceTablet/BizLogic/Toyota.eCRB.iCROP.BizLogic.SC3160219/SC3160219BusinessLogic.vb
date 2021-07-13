'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3160219BusinessLogic.vb
'──────────────────────────────────
'機能： RO損傷登録画面
'補足： 
'作成： 2013/11/19 SKFC 橋本
'更新： 2018/03/29 SKFC 横田　REQ-SVT-TMT-20170809-001　損傷写真複数対応
'    ： 2019/09/20 SKFC 二村 TR-V4-TKM-20190813-003横展
'──────────────────────────────────

Imports System.Text
Imports System.IO
Imports System.Xml
Imports System.Web
Imports System.Net
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.iCROP.DataAccess.SC3160219
Imports Toyota.eCRB.SystemFrameworks.Web
Imports System.Globalization

Public Class SC3160219BusinessLogic
    Inherits BaseBusinessComponent

#Region "定数"

    ''' <summary>
    ''' 機能ID：RO損傷登録画面
    ''' </summary>
    Private Const C_FUNCTION_ID As String = "SC3160219"

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
    ''' 画像情報連携I/Fの返却XMLタグ：中サイズイメージ
    ''' </summary>
    Private Const C_IMAGESIZE_XMLTAG As String = "MiddleImage"

    ''' <summary>
    ''' ファイル拡張子の設定名
    ''' </summary>
    Private Const C_FILE_EXTENSION As String = "FILE_UPLOAD_EXTENSION"


#End Region

#Region "Publicメソット"

    ''' <summary>
    ''' RO外装ダメージマスタ情報取得
    ''' </summary>
    ''' <param name="partsType">部位種別</param>
    ''' <returns>RO外装ダメージマスタ情報データテーブル</returns>
    ''' <remarks></remarks>
    Public Function GetRoExteriorDamageMaster(ByVal partsType As String) As SC3160219DataSet.RoExteriorDamageMasterDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} Start",
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim dt As SC3160219DataSet.RoExteriorDamageMasterDataTable
        Dim clsTblAdapter As New SC3160219TableAdapter

        'RO外装ダメージマスタ情報を取得
        dt = clsTblAdapter.GetRoExteriorDamageMaster(partsType)

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} End [RowCount:{1}]",
                                  System.Reflection.MethodBase.GetCurrentMethod.Name,
                                  dt.Rows.Count))

        '処理結果返却
        Return dt

    End Function


    ''' <summary>
    ''' RO外装ダメージ情報取得
    ''' </summary>
    ''' <param name="roExteriorId">RO外装ID</param>
    ''' <param name="partsType">部位種別</param>
    ''' <returns>RO外装ダメージ情報データテーブル</returns>
    ''' <remarks></remarks>
    Public Function GetRoExteriorDamageInfo(ByVal roExteriorId As Decimal, ByVal partsType As String) As SC3160219DataSet.RoExteriorDamageInfoDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} Start",
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim dt As SC3160219DataSet.RoExteriorDamageInfoDataTable
        Dim clsTblAdapter As New SC3160219TableAdapter

        'RO外装ダメージ情報を取得
        dt = clsTblAdapter.GetRoExteriorDamageInfo(roExteriorId, partsType)

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} End [RowCount:{1}]",
                                  System.Reflection.MethodBase.GetCurrentMethod.Name,
                                  dt.Rows.Count))

        '処理結果返却
        Return dt

    End Function



    ''' <summary>
    ''' RO外装ダメージ情報取得
    ''' </summary>
    ''' <param name="roExteriorId">RO外装ID</param>
    ''' <param name="partsType">部位種別</param>
    ''' <returns>RO外装ダメージ情報データテーブル</returns>
    ''' <remarks></remarks>
    Public Function GetRothumbnailInfo(ByVal roExteriorId As Decimal, ByVal partsType As String) As SC3160219DataSet.RoExteriorDamageInfoDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} Start",
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim dt As SC3160219DataSet.RoExteriorDamageInfoDataTable
        Dim clsTblAdapter As New SC3160219TableAdapter

        'RO外装ダメージ情報を取得
        dt = clsTblAdapter.GetRothumbnailInfo(roExteriorId, partsType)

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} End [RowCount:{1}]",
                                  System.Reflection.MethodBase.GetCurrentMethod.Name,
                                  dt.Rows.Count))

        '処理結果返却
        Return dt

    End Function

    ''' <summary>
    ''' RO外装ダメージ情報登録
    ''' </summary>
    ''' <param name="roExteriorId">RO外装ID</param>
    ''' <param name="partsType">部位種別</param>
    ''' <param name="damageTypeExists">ダメージ有無</param>
    ''' <param name="memo">メモ</param>
    ''' <param name="roThumbnailId"></param>
    ''' <param name="rothumbnailImgPath"></param>
    ''' <param name="rothumbnailImgPathOrg"></param>
    ''' <param name="account">アカウント</param>
    ''' <returns>処理結果  成功:True/失敗:False</returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Function SetROExteriorDamageInfo(ByVal roExteriorId As Decimal,
                                            ByVal partsType As String,
                                            ByVal damageTypeExists As String,
                                            ByVal memo As String,
                                            ByVal roThumbnailId As String,
                                            ByVal roThumbnailImgPath As String,
                                            ByVal roThumbnailImgPathOrg As String,
                                            ByVal account As String) As Boolean


        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} Start [roExteriorId:{1}][partsType:{2}][damageTypeExists:{3}][memo:{4}][thumbnailImgPath:{5}][thumbnailImgPathOrg:{6}][account:{7}]",
                                  System.Reflection.MethodBase.GetCurrentMethod.Name,
                                  roExteriorId, partsType, damageTypeExists, memo, roThumbnailId, roThumbnailImgPath, roThumbnailImgPathOrg,
                                  account))

        ' カンマ区切りで分割して配列に格納する
        Dim roThumbnailIdData As String() = roThumbnailId.Split(","c)
        Dim roThumbnailImgPathData As String() = roThumbnailImgPath.Split(","c)
        Dim roThumbnailImgPathOrgData As String() = roThumbnailImgPathOrg.Split(","c)

        Dim clsTblAdapter As New SC3160219TableAdapter


        '処理結果
        Dim isSuccess As Boolean = True
        Dim roThumbnailIdNew As Decimal = -1

        '削除処理
        For l As Integer = 0 To 4

            Dim index1 As Integer = 0

            'roThumbnailImgPathDataの位置を取得する
            index1 = Array.IndexOf(roThumbnailImgPathData, roThumbnailImgPathOrgData(l))

            If index1 < 0 AndAlso Not String.IsNullOrEmpty(roThumbnailImgPathOrgData(l)) Then
                isSuccess = Me.SetDispOffROThumbnailInfo(roThumbnailIdData(l))
            Else
                If Not String.IsNullOrEmpty(roThumbnailIdData(l)) Then
                    roThumbnailIdNew = CDec(roThumbnailIdData(l))
                End If
            End If

        Next

        '登録処理
        For k As Integer = 0 To 4

            Dim index1 As Integer = 0

            'roThumbnailImgPathDataの位置を取得する
            index1 = Array.IndexOf(roThumbnailImgPathOrgData, roThumbnailImgPathData(k))
            If index1 < 0 AndAlso Not String.IsNullOrEmpty(roThumbnailImgPathData(k)) Then

                'ROサムネイルIDの取得
                roThumbnailIdNew = clsTblAdapter.GetRoThumbnailId()

                '取得成功時
                If roThumbnailIdNew >= 0 Then

                    'ROサムネイル画像登録
                    isSuccess = clsTblAdapter.SetROThumbnailImgPath(roThumbnailIdNew,
                                                                    roExteriorId,
                                                                    roThumbnailImgPathData(k),
                                                                    account,
                                                                    partsType
                                                                    )
                End If

            End If
        Next


        'If String.IsNullOrEmpty(roThumbnailImgPath1) AndAlso Not String.IsNullOrEmpty(roThumbnailImgPathOrg1) Then
        '    '表示中ファイルパスがなくて、表示時にはあった場合

        '    'ROサムネイル画像物理削除
        '    isSuccess = Me.SetDispOffROThumbnailInfo(roThumbnailId)

        '    '削除されたのでROサムネイルIDをクリア
        '    roThumbnailIdNew = -1

        'Else
        '    '変わらない場合はそのまま更新
        '    roThumbnailIdNew = roThumbnailId

        '    isSuccess = True

        'End If


        '登録成功時
        If isSuccess Then

            'ROサムネイル画像登録
            isSuccess = clsTblAdapter.SetROExteriorDamageInfo(roExteriorId,
                                                              partsType,
                                                              damageTypeExists,
                                                              memo,
                                                              roThumbnailIdNew,
                                                              account)

        End If

        'いずれかが失敗時
        If Not isSuccess Then
            'ロールバック
            Me.Rollback = True

        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} End [IsSuccess:{1}]",
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, isSuccess))

        '終了
        Return isSuccess

    End Function


    ''' <summary>
    ''' ROサムネイル画像論理削除
    ''' </summary>
    ''' <param name="roThumbnailId">ROサムネイルID</param>
    ''' <returns>処理結果  成功:True/失敗:False</returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Function SetDispOffROThumbnailInfo(ByVal roThumbnailId As String) As Boolean

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} Start [roThumbnailId:{1}]",
                                  System.Reflection.MethodBase.GetCurrentMethod.Name,
                                  roThumbnailId))

        Dim clsTblAdapter As New SC3160219TableAdapter

        '処理結果
        Dim isSuccess As Boolean = False

        'ROサムネイル画像論理削除
        isSuccess = clsTblAdapter.SetROThumbnailInfo(roThumbnailId)

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} End [IsSuccess:{1}]",
                                  System.Reflection.MethodBase.GetCurrentMethod.Name,
                                  isSuccess))

        '終了
        Return isSuccess

    End Function


    ''' <summary>
    ''' 写真相対パスの取得
    ''' </summary>
    ''' <param name="filePath">対象画像ファイルパス</param>
    ''' <param name="largeFilePath">(out)大きい画像のファイルパス</param>
    ''' <returns>対象画像ファイル相対パス</returns>
    ''' <remarks></remarks>
    Public Function GetImageFileRelativePath(ByVal filePath As String, ByRef largeFilePath As String) As String

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} Start [filePath:{1}] [largeFilePath:{2}]",
                                  System.Reflection.MethodBase.GetCurrentMethod.Name,
                                  filePath, largeFilePath))

        Dim retRelativePath As String = String.Empty

        Dim clsTblAdapter As New SC3160219TableAdapter

        '送信XML作成
        Dim imageInfoXml As String = CreateSendXML(filePath)
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

            Dim endPath As String = String.Empty    'テスト時：filePath

            '画像ファイルのパスを取得
            For Each imageNode As XmlNode In rootElement.GetElementsByTagName(C_IMAGESIZE_XMLTAG)
                endPath = imageNode.InnerText
            Next

            '公開フォルダのURLはProgramSettingより取得
            Dim openDlr As String = String.Empty
            Dim dtOpenDlr As DataTable = clsTblAdapter.GetProgramSettingValues(C_FUNCTION_ID, , C_OPEN_DIRECTORY_URL)

            If Not IsDBNull(dtOpenDlr) Then openDlr = dtOpenDlr.Rows(0).Item("SETTING_VAL").ToString

            'パスを合成
            retRelativePath = Path.Combine(openDlr, endPath)

            ' 大きい画像パス生成
            Dim largeImageElement As XmlNodeList = rootElement.GetElementsByTagName("OriginalImage")
            largeFilePath = Path.Combine(openDlr, largeImageElement.Item(0).InnerText)

        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} End [retRelativePath :{1}]",
                                  System.Reflection.MethodBase.GetCurrentMethod.Name,
                                  retRelativePath))

        Return retRelativePath

    End Function


    ''' <summary>
    ''' 新ファイルパスを作成
    ''' </summary>
    ''' <param name="roExteriorId">RO外装ID</param>
    ''' <param name="partsType">部位種別</param>
    ''' <param name="imageSeq">画像連番</param>
    ''' <returns>新ファイルパス</returns>
    ''' <remarks>公開フォルダ以下の保存先ディレクトリと拡張子を付与した新連番ファイルパスを作成する</remarks>
    Public Function CleateNewFilePath(ByVal roExteriorId As String, ByVal partsType As String, ByVal imageSeq As String) As String

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} Start [roExteriorId:{1}][partsType:{2}][imageSeq:{3}]",
                                  System.Reflection.MethodBase.GetCurrentMethod.Name,
                                  roExteriorId, partsType, CStr(imageSeq)))

        Dim filePath As String = String.Empty

        '連番を１つ進める
        Dim nextImageSeq As Integer
        If String.IsNullOrEmpty(imageSeq) Then
            nextImageSeq = 1
        Else
            nextImageSeq = CInt(imageSeq) + 1
        End If

        '連番を進めてファイル名を作成
        Dim fileName As String = roExteriorId & "_" & partsType & "_" & CStr(nextImageSeq)

        '保存先ディレクトリと拡張子を含んだフォーマットをProgramSettingから取得
        Dim clsTblAdapter As New SC3160219TableAdapter
        'Dim dt As DataTable = clsTblAdapter.GetProgramSettingValues(C_FUNCTION_ID, , C_FILE_NAME_FORMAT)
        Dim dt As DataTable = clsTblAdapter.GetSystemSettingValues(C_FILE_EXTENSION)
        Dim replaceTarget As String = "{0}"

        If Not IsDBNull(dt) Then
            filePath = String.Format(dt.Rows(0).Item("SETTING_VAL").ToString, fileName)
            filePath = filePath.Replace(replaceTarget, "")
            Dim dealerCode = clsTblAdapter.GetDealerCode(roExteriorId)
            If (Not String.IsNullOrWhiteSpace(dealerCode)) Then
                filePath = String.Format("{0}/{1}/{2}", dealerCode, Now.ToString("yyyyMMdd"), filePath)
            End If
        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} End [filePath :{1}]",
                                  System.Reflection.MethodBase.GetCurrentMethod.Name,
                                  filePath))

        Return filePath

    End Function

#End Region

#Region "Privateメソット"

    ''' <summary>
    ''' 画像情報連携I/F送信用XML作成
    ''' </summary>
    ''' <param name="filePath"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CreateSendXML(ByVal filePath As String) As String

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} Start [filePath:{1}]",
                                  System.Reflection.MethodBase.GetCurrentMethod.Name,
                                  filePath))

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
        xmlString.Append("          <Image_Path>" & filePath & "</Image_Path>")
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
                                  "{0} Start [postData:{1}][WebServiceUrl:{2}]",
                                  System.Reflection.MethodBase.GetCurrentMethod.Name,
                                  postData, WebServiceUrl))

        '文字コードを指定する
        Dim enc As System.Text.Encoding = Encoding.GetEncoding("UTF-8")

        'バイト型配列に変換
        Dim postDataBytes As Byte() = Encoding.UTF8.GetBytes(postData)

        'WebRequestの作成
        Dim req As WebRequest = WebRequest.Create(WebServiceUrl)

        'メソッドにPOSTを指定
        req.Method = "POST"

        'ContentTypeを"application/x-www-form-urlencoded"にする
        req.ContentType = "application/x-www-form-urlencoded"

        'POST送信するデータの長さを指定
        req.ContentLength = postDataBytes.Length

        'データをPOST送信するためのStreamを取得
        Dim reqStream As Stream = req.GetRequestStream()

        '送信するデータを書き込む
        reqStream.Write(postDataBytes, 0, postDataBytes.Length)
        reqStream.Close()

        'サーバーからの応答を受信するためのWebResponseを取得
        Dim res As WebResponse = req.GetResponse()

        '応答データを受信するためのStreamを取得
        Dim resStream As Stream = res.GetResponseStream()

        '受信して表示
        Dim sr As New StreamReader(resStream, enc)

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
