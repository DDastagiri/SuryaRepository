'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3070207WebClient.vb
'─────────────────────────────────────
'機能： 注文承認
'補足： 
'作成： 2013/12/10 TCS 山口  Aカード情報相互連携開発
'更新： 2014/08/01 TCS 山口 NextStep BTS-74
'更新： 2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 
'─────────────────────────────────────

Imports System.Net
Imports System.IO
Imports System.Text
Imports System.Xml
Imports System.Web
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports System.Globalization
Imports Toyota.eCRB.Estimate.Quotation.DataAccess
Imports Toyota.eCRB.Estimate.Quotation.DataAccess.IC3070201DataSet
Imports System.Reflection

''' <summary>
''' Webサービスクライアントクラス
''' </summary>
''' <remarks></remarks>
Public NotInheritable Class SC3070207WebClient
    Inherits BaseBusinessComponent

#Region "コンストラクタ"
    ''' <summary>
    ''' デフォルトコンストラクタ
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        '処理なし
    End Sub

#End Region

#Region "PrivateConst"
    ''' <summary>
    ''' ポスト
    ''' </summary>
    ''' <remarks></remarks>
    Private Const POST_STR As String = "POST"
    ''' <summary>
    ''' コンテキストタイプ(XML)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONTEXT_TYPE_XML As String = "application/xml; charset=UTF-8"
    ''' <summary>
    ''' コンテキストタイプ(Form)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONTEXT_TYPE_FORM As String = "application/x-www-form-urlencoded;"
    ''' <summary>
    ''' タグ名　結果
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TAG_RESULT_ID As String = "ResultId"
    ''' <summary>
    ''' タグ名　メッセージ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TAG_MESSAGE As String = "Message"
    ''' <summary>
    ''' タグ名　契約No
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TAG_CONTRACTNO As String = "ContractNo"
    ''' <summary>
    ''' Dictinay　key 終了コード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DIC_KEY_ID As String = "ID"
    ''' <summary>
    ''' Dictinay　key メッセージ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DIC_KEY_MSG As String = "MSG"
    ''' <summary>
    ''' Dictinay　key 契約書NO
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DIC_KEY_NO As String = "NO"

    ''' <summary>
    ''' シリーズコード　AHV41L-JEXGBC
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SERIES_CD_HV As String = "AHV41L-JEXGBC"

    ''' <summary>
    ''' 車種コード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VEHICLE_NAME_CODE As String = "CMYHV"

    ''' <summary>
    ''' 契約顧客種別 所有者
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONTRACTCUSTTYPE_DEALER As String = "1"

    ''' <summary>
    ''' テーブル名　見積情報(I/F)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IFTBL_ESTINFO = "IC3070201EstimationInfo"

    ''' <summary>
    ''' テーブル名　顧客情報(I/F)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IFTBL_CUSTINFO = "IC3070201CustomerInfo"

    ''' <summary>
    ''' テーブル名　見積保険情報(I/F)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IFTBL_INSINFO = "IC3070201EstInsuranceInfo"

    ''' <summary>
    ''' テーブル名　基幹コード情報(I/F)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IFTBL_DMSCD = "IC3070201DmsCd"

    ''' <summary>
    ''' 郵便番号
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CLM_ZIPCODE = "ZIPCODE"

    ''' <summary>
    ''' 見積情報DBのカラム名　値引き額
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DISCOUNTPRICE As String = "DISCOUNTPRICE"

    ''' <summary>
    ''' 見積車両情報DBのカラム名　本体車両価格
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CLM_BASEPRICE As String = "BASEPRICE"

    ''' <summary>
    ''' 見積車両情報DBのカラム名　外装追加費用
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CLM_EXTAMOUNT As String = "EXTAMOUNT"

    ''' <summary>
    ''' 見積車両情報DBのカラム名　内装追加費用
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CLM_INTAMOUNT As String = "INTAMOUNT"

    ''' <summary>
    ''' 納車予定日
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CLM_DELIDATE = "DELIDATE"

    ''' <summary>
    ''' DMS側の日付書式パラメータ名
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DMS_DATETIMEFORMAT As String = "DMS_DATETIMEFORMAT"

    ''' <summary>
    ''' 変換フォーマット
    ''' </summary>
    ''' <remarks>YYYY/MM/DD</remarks>
    Private Const FORMAT_YYYYMMDD As Integer = 3

    ''' <summary>
    ''' TACT側性別区分　男性："1"
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TACT_SEX_MEN As String = "1"
    ''' <summary>
    ''' TACT側性別区分　女性："2"
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TACT_SEX_FIMALE As String = "2"

#End Region

#Region "Publicメソッド"
    ''' <summary>
    ''' リクエストの送信を行い、レスポンスを受信します。
    ''' </summary>
    ''' <param name="aCardNum">A-Card番号</param>
    ''' <param name="drPaymentInfo">見積支払情報</param>
    ''' <param name="apidt">見積情報I/F</param>
    ''' <param name="dealerEnvDt">環境設定のTACT連携PATH</param>
    ''' <param name="staff">StaffContext</param>
    ''' <param name="useXmlPost">True：XML送信/False：Form送信</param>
    ''' <returns>Dictionary(key:レスポンスの項目名　value:レスポンスの値)</returns>
    ''' <remarks></remarks>
    Public Function RequestHttp(ByVal aCardNum As String, _
                                ByVal drPaymentInfo As SC3070207DataSet.SC3070207PaymentInfoRow, _
                                ByVal apidt As IC3070201DataSet, _
                                ByVal dealerEnvdt As DlrEnvSettingDataSet.DLRENVSETTINGRow, _
                                ByVal staff As StaffContext, _
                                ByVal useXmlPost As Boolean) As Dictionary(Of String, String)

        '送信するxmlを作成する
        Dim xmlValue As String = CreateXml(aCardNum, _
                                           drPaymentInfo, _
                                           apidt, _
                                           staff)
        If Not useXmlPost Then xmlValue = "xsData=" + HttpUtility.UrlEncode(xmlValue) + ""
        Logger.Warn(xmlValue)

        Dim rtnStrs As New Dictionary(Of String, String)
        If dealerEnvdt Is Nothing Then
            Return rtnStrs
        End If

        Dim tactPath As String = dealerEnvdt.PARAMVALUE
        Logger.Info(tactPath)


        'POST送信するデータ
        Dim postDataBytes As Byte()
        If useXmlPost Then
            postDataBytes = Encoding.UTF8.GetBytes(xmlValue)
        Else
            postDataBytes = Encoding.ASCII.GetBytes(xmlValue)
        End If

        '----------------TEST LOGIC-----------------
        ''結果
        'rtnStrs.Add(DIC_KEY_ID, "0")
        ''メッセージ
        'rtnStrs.Add(DIC_KEY_MSG, "AAAA")
        ''契約No
        'rtnStrs.Add(DIC_KEY_NO, "1234567890")
        '----------------TEST LOGIC-----------------

        'webリクエストの作成
        Try
            Dim path As String = New Uri(tactPath).ToString
            Dim req As HttpWebRequest = CType(WebRequest.Create(New Uri(path)), HttpWebRequest)

            'メソッドをPOSTに設定
            req.Method = POST_STR

            'ContentTypeの設定
            If useXmlPost Then
                req.ContentType = CONTEXT_TYPE_XML
            Else
                req.ContentType = CONTEXT_TYPE_FORM
            End If
            Logger.Info("ContentType:" & req.ContentType)

            'POST送信するデータの長さを設定
            req.ContentLength = postDataBytes.Length

            'データをPOST送信するためのStream取得
            Dim reqStream As System.IO.Stream = req.GetRequestStream
            '送信するデータを書き込む
            reqStream.Write(postDataBytes, 0, postDataBytes.Length)

            reqStream.Close()

            Dim returnString As String = String.Empty

            'サーバからの応答を受信
            Logger.Info("SA04_Start", True)
            Using res As WebResponse = req.GetResponse() 'サーバーからの応答を受信するためのWebResponseを取得
                Logger.Info("GetResponse End")
                Using resStream As Stream = res.GetResponseStream() '応答データを受信するためのStreamを取得
                    Logger.Info("GetResponseStream End")
                    '受信文字コード:UTF-8
                    Using sr As New StreamReader(resStream, Encoding.UTF8) '受信
                        Logger.Info("ReadStream End")
                        '返却文字列を取得
                        returnString = sr.ReadToEnd()
                        Logger.Info("GetReturnString End")
                        '閉じる
                        sr.Close()
                    End Using
                End Using
            End Using

            'xml読込み
            Logger.Info("ReadXml Start")
            Logger.Warn(xmlValue)

            Using xml As XmlTextReader = New XmlTextReader((New StringReader(returnString)))
                Logger.Info("MakeXML")
                While xml.Read()
                    Logger.Info("ReadXML")
                    Select Case xml.NodeType
                        Case XmlNodeType.Element

                            If TAG_RESULT_ID.Equals(xml.Name) Then
                                '結果
                                rtnStrs.Add(DIC_KEY_ID, xml.ReadString())
                                Logger.Info(String.Format(CultureInfo.InvariantCulture, "ResultId {0}", xml.ReadString()), True)
                            ElseIf TAG_MESSAGE.Equals(xml.Name) Then
                                'メッセージ
                                rtnStrs.Add(DIC_KEY_MSG, xml.ReadString())
                                Logger.Info(String.Format(CultureInfo.InvariantCulture, "Message {0}", xml.ReadString()), True)
                            ElseIf TAG_CONTRACTNO.Equals(xml.Name) Then
                                '契約No
                                rtnStrs.Add(DIC_KEY_NO, xml.ReadString())
                                Logger.Info(String.Format(CultureInfo.InvariantCulture, "ContractNo {0}", xml.ReadString()), True)
                            End If
                    End Select
                End While
            End Using
            Logger.Info("ReadXml End")

            Logger.Info("SA04_End", True)
        Catch ex As WebException
            '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 START
            Logger.Error("Exception", ex)
            '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 END
            Return rtnStrs
        End Try

        Return rtnStrs
    End Function

#End Region

#Region "Privateメソッド"
    ''' <summary>
    ''' I/F用のxmlを作成します。
    ''' </summary>
    ''' <param name="aCardNum">A-Card番号</param>
    ''' <param name="drPaymentInfo">見積支払情報</param>
    ''' <param name="apiDt">見積情報I/F</param>
    ''' <param name="staff">StaffContext</param>
    ''' <returns>xmlデータ</returns>
    ''' <remarks></remarks>
    Private Function CreateXml(ByVal aCardNum As String, _
                               ByVal drPaymentInfo As SC3070207DataSet.SC3070207PaymentInfoRow, _
                               ByVal apiDt As IC3070201DataSet, _
                               ByVal staff As StaffContext) As String

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  " {0}_Start",
                                  MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        Using writer As StringWriter = New StringWriter(CultureInfo.InvariantCulture)
            Using xmlWriter As XmlTextWriter = New XmlTextWriter(writer)
                '見積情報取得I/Fで取得したデータ
                Dim apiDtEstimateRow As IC3070201EstimationInfoRow = Nothing
                apiDtEstimateRow = CType(apiDt.Tables(IFTBL_ESTINFO).Rows(0), IC3070201EstimationInfoRow)
                '見積情報取得I/Fで取得した基幹コード(販売店コード、店舗コード)
                Dim apiDtDmsCdRow As IC3070201DmsCdRow = Nothing
                apiDtDmsCdRow = CType(apiDt.Tables(IFTBL_DMSCD).Rows(0), IC3070201DmsCdRow)

                xmlWriter.WriteStartDocument()
                xmlWriter.WriteStartElement("CreateOrder")
                xmlWriter.WriteStartElement("Head")
                'メッセージID
                xmlWriter.WriteElementString("MessageID", "IC3800201")
                '国コード
                xmlWriter.WriteElementString("CountryCode", EnvironmentSetting.CountryCode)
                '基幹SYSTEM識別コード
                xmlWriter.WriteElementString("LinkSystemCode", "0")
                'メッセージ送信日時

                'DMS側の日付書式取得
                Dim strDateFormat As String = Me.GetDmsDateFormat(DMS_DATETIMEFORMAT)
                Dim dNow As Date = DateTimeFunc.Now

                '年月日までしかない為、年月日と時分秒を分けて書式設定
                Dim strTransmissionDate As String = dNow.ToString(strDateFormat, CultureInfo.InvariantCulture) & " " & _
                                                    dNow.ToString("HH:mm:ss", CultureInfo.InvariantCulture)

                xmlWriter.WriteElementString("TransmissionDate", strTransmissionDate)

                xmlWriter.WriteEndElement() 'Head

                xmlWriter.WriteStartElement("Detail")
                xmlWriter.WriteStartElement("Common")

                '販売店コード
                'xmlWriter.WriteElementString("DealerCode", staff.DlrCD)
                xmlWriter.WriteElementString("DealerCode", Me.GetDmsCdDtCol(apiDtDmsCdRow, "DMS_CD_1"))
                '店舗コード
                'xmlWriter.WriteElementString("BranchCode", staff.BrnCD)
                xmlWriter.WriteElementString("BranchCode", Me.GetDmsCdDtCol(apiDtDmsCdRow, "DMS_CD_2"))

                xmlWriter.WriteEndElement() 'Common

                xmlWriter.WriteStartElement("OrderInfo")

                'シーケンスナンバー
                xmlWriter.WriteElementString("SeqNo", DateTimeFunc.Now.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture))

                'A-Card番号
                xmlWriter.WriteElementString("FollowUpID", aCardNum)

                'Follow-up Box内連番
                xmlWriter.WriteElementString("FollowUpNo", Me.GetApiDtCol(apiDtEstimateRow, "FLLWUPBOX_SEQNO"))

                '契約番号
                Dim contractNo As String = Me.GetApiDtCol(apiDtEstimateRow, "CONTRACTNO")
                If Not String.IsNullOrEmpty(contractNo) Then
                    xmlWriter.WriteElementString("ContractNo", contractNo)
                End If

                '見積管理ID
                xmlWriter.WriteElementString("EstimateId", Me.GetApiDtCol(apiDtEstimateRow, "ESTIMATEID"))

                '売上区分   
                xmlWriter.WriteElementString("SalesPart", "1")

                '顧客情報
                For Each apiDtCustomerRow As IC3070201CustomerInfoRow In apiDt.Tables(IFTBL_CUSTINFO).Rows()
                    If CONTRACTCUSTTYPE_DEALER.Equals(apiDtCustomerRow.Item("CONTRACTCUSTTYPE")) Then
                        '所有者のみ設定
                        '買主区分 
                        xmlWriter.WriteElementString("BuyerPart", Me.GetCustomerDtCol(apiDtCustomerRow, "CUSTPART"))
                        '2014/08/01 TCS 山口 NextStep BTS-74 START
                        'Buyer Sub Part
                        xmlWriter.WriteElementString("BuyerPart2", Me.GetCustomerDtCol(apiDtCustomerRow, "PRIVATE_FLEET_ITEM_CD"))
                        'Buyer Name Title Code
                        xmlWriter.WriteElementString("BuyerNameTitleCode", Me.GetCustomerDtCol(apiDtCustomerRow, "NAMETITLE_CD"))
                        'Buyer Name Title
                        xmlWriter.WriteElementString("BuyerNameTitle", Me.GetCustomerDtCol(apiDtCustomerRow, "NAMETITLE_NAME"))
                        '2014/08/01 TCS 山口 NextStep BTS-74 END
                        '買主名
                        xmlWriter.WriteElementString("BuyerName", Me.GetCustomerDtCol(apiDtCustomerRow, "NAME"))
                        '2014/08/01 TCS 山口 NextStep BTS-74 START
                        'Buyer First Name
                        xmlWriter.WriteElementString("BuyerFirstName", Me.GetCustomerDtCol(apiDtCustomerRow, "FIRST_NAME"))
                        'Buyer Middle Name
                        xmlWriter.WriteElementString("BuyerMiddleName", Me.GetCustomerDtCol(apiDtCustomerRow, "MIDDLE_NAME"))
                        'Buyer Last Name
                        xmlWriter.WriteElementString("BuyerLastName", Me.GetCustomerDtCol(apiDtCustomerRow, "LAST_NAME"))
                        '2014/08/01 TCS 山口 NextStep BTS-74 END
                        '買主ID
                        xmlWriter.WriteElementString("BuyerID", Me.GetCustomerDtCol(apiDtCustomerRow, "SOCIALID"))
                        '買主郵便番号
                        xmlWriter.WriteElementString("BuyerZIP", Me.GetCustomerDtCol(apiDtCustomerRow, "ZIPCODE"))
                        '買主住所
                        xmlWriter.WriteElementString("BuyerAddress", Me.GetCustomerDtCol(apiDtCustomerRow, "ADDRESS"))
                        '2014/08/01 TCS 山口 NextStep BTS-74 START
                        'Buyer Address1
                        xmlWriter.WriteElementString("BuyerAddress1", Me.GetCustomerDtCol(apiDtCustomerRow, "CST_ADDRESS_1"))
                        'Buyer Address2
                        xmlWriter.WriteElementString("BuyerAddress2", Me.GetCustomerDtCol(apiDtCustomerRow, "CST_ADDRESS_2"))
                        'Buyer Address3
                        xmlWriter.WriteElementString("BuyerAddress3", Me.GetCustomerDtCol(apiDtCustomerRow, "CST_ADDRESS_3"))
                        'Buyer State Code
                        xmlWriter.WriteElementString("BuyerStateCode", Me.GetCustomerDtCol(apiDtCustomerRow, "CST_ADDRESS_STATE"))
                        'Buyer District Code
                        xmlWriter.WriteElementString("BuyerDistrictCode", Me.GetCustomerDtCol(apiDtCustomerRow, "CST_ADDRESS_DISTRICT"))
                        'Buyer City Code
                        xmlWriter.WriteElementString("BuyerCityCode", Me.GetCustomerDtCol(apiDtCustomerRow, "CST_ADDRESS_CITY"))
                        'Buyer Location Code
                        xmlWriter.WriteElementString("BuyerLocationCode", Me.GetCustomerDtCol(apiDtCustomerRow, "CST_ADDRESS_LOCATION"))
                        '2014/08/01 TCS 山口 NextStep BTS-74 END

                        Dim telno As String = ""
                        Dim mobileNo As String = ""
                        If Not String.IsNullOrEmpty(Me.GetCustomerDtCol(apiDtCustomerRow, "TELNO")) Then
                            telno = Me.GetCustomerDtCol(apiDtCustomerRow, "TELNO")
                            mobileNo = Me.GetCustomerDtCol(apiDtCustomerRow, "MOBILE")
                        Else
                            telno = Me.GetCustomerDtCol(apiDtCustomerRow, "MOBILE")
                        End If

                        '買主電話１
                        xmlWriter.WriteElementString("BuyerTEL1", telno)
                        '買主電話2
                        xmlWriter.WriteElementString("BuyerTEL2", mobileNo)
                        '買主ＦＡＸ
                        xmlWriter.WriteElementString("BuyerFAX", Me.GetCustomerDtCol(apiDtCustomerRow, "FAXNO"))
                        '買主E-MAIL
                        xmlWriter.WriteElementString("BuyerEMail", Me.GetCustomerDtCol(apiDtCustomerRow, "EMAIL"))
                    Else
                        '使用者のみ設定
                        '名義人区分
                        xmlWriter.WriteElementString("NomineePart", Me.GetCustomerDtCol(apiDtCustomerRow, "CUSTPART"))
                        '2014/08/01 TCS 山口 NextStep BTS-74 START
                        'Nominee Sub Part
                        xmlWriter.WriteElementString("NomineePart2", Me.GetCustomerDtCol(apiDtCustomerRow, "PRIVATE_FLEET_ITEM_CD"))
                        'Nominee Name Title Code
                        xmlWriter.WriteElementString("NomineeNameTitleCode", Me.GetCustomerDtCol(apiDtCustomerRow, "NAMETITLE_CD"))
                        'Nominee Name Title
                        xmlWriter.WriteElementString("NomineeNameTitle", Me.GetCustomerDtCol(apiDtCustomerRow, "NAMETITLE_NAME"))
                        '2014/08/01 TCS 山口 NextStep BTS-74 END
                        '名義人名
                        xmlWriter.WriteElementString("Nominee", Me.GetCustomerDtCol(apiDtCustomerRow, "NAME"))
                        '2014/08/01 TCS 山口 NextStep BTS-74 START
                        'Nominee First Name
                        xmlWriter.WriteElementString("NomineeFirstName", Me.GetCustomerDtCol(apiDtCustomerRow, "FIRST_NAME"))
                        'Nominee Middle Name
                        xmlWriter.WriteElementString("NomineeMiddleName", Me.GetCustomerDtCol(apiDtCustomerRow, "MIDDLE_NAME"))
                        'Nominee Last Name
                        xmlWriter.WriteElementString("NomineeLastName", Me.GetCustomerDtCol(apiDtCustomerRow, "LAST_NAME"))
                        '2014/08/01 TCS 山口 NextStep BTS-74 END
                        '名義人ID
                        xmlWriter.WriteElementString("NomineeID", Me.GetCustomerDtCol(apiDtCustomerRow, "SOCIALID"))
                        '名義人郵便番号
                        xmlWriter.WriteElementString("NomineeZIP", Me.GetCustomerDtCol(apiDtCustomerRow, "ZIPCODE"))
                        '名義人住所
                        xmlWriter.WriteElementString("NomineeAddress", Me.GetCustomerDtCol(apiDtCustomerRow, "ADDRESS"))
                        '2014/08/01 TCS 山口 NextStep BTS-74 START
                        'Nominee Address1
                        xmlWriter.WriteElementString("NomineeAddress1", Me.GetCustomerDtCol(apiDtCustomerRow, "CST_ADDRESS_1"))
                        'Nominee Address2
                        xmlWriter.WriteElementString("NomineeAddress2", Me.GetCustomerDtCol(apiDtCustomerRow, "CST_ADDRESS_2"))
                        'Nominee Address3
                        xmlWriter.WriteElementString("NomineeAddress3", Me.GetCustomerDtCol(apiDtCustomerRow, "CST_ADDRESS_3"))
                        'Nominee State Code
                        xmlWriter.WriteElementString("NomineeStateCode", Me.GetCustomerDtCol(apiDtCustomerRow, "CST_ADDRESS_STATE"))
                        'Nominee District Code
                        xmlWriter.WriteElementString("NomineeDistrictCode", Me.GetCustomerDtCol(apiDtCustomerRow, "CST_ADDRESS_DISTRICT"))
                        'Nominee City Code
                        xmlWriter.WriteElementString("NomineeCityCode", Me.GetCustomerDtCol(apiDtCustomerRow, "CST_ADDRESS_CITY"))
                        'Nominee Location Code
                        xmlWriter.WriteElementString("NomineeLocationCode", Me.GetCustomerDtCol(apiDtCustomerRow, "CST_ADDRESS_LOCATION"))
                        '2014/08/01 TCS 山口 NextStep BTS-74 END

                        Dim holTelno As String = ""
                        Dim holMobileNo As String = ""
                        If Not String.IsNullOrEmpty(Me.GetCustomerDtCol(apiDtCustomerRow, "TELNO")) Then
                            holTelno = Me.GetCustomerDtCol(apiDtCustomerRow, "TELNO")
                            holMobileNo = Me.GetCustomerDtCol(apiDtCustomerRow, "MOBILE")
                        Else
                            holTelno = Me.GetCustomerDtCol(apiDtCustomerRow, "MOBILE")
                        End If

                        '名義人電話１
                        xmlWriter.WriteElementString("NomineeTEL1", holTelno)
                        '名義人電話2
                        xmlWriter.WriteElementString("NomineeTEL2", holMobileNo)
                        '名義人ＦＡＸ
                        xmlWriter.WriteElementString("NomineeFAX", Me.GetCustomerDtCol(apiDtCustomerRow, "FAXNO"))
                        '名義人EーMAIL
                        xmlWriter.WriteElementString("NomineeEMail", Me.GetCustomerDtCol(apiDtCustomerRow, "EMAIL"))
                    End If
                Next

                Dim strModelNumber As String = Me.GetApiDtCol(apiDtEstimateRow, "MODELNUMBER")      '車両型号
                Dim strSeriesCode As String = Me.GetApiDtCol(apiDtEstimateRow, "SERIESCD")          'シリーズコード

                'セールスコード
                If (String.IsNullOrEmpty(staff.Account.Trim())) Then
                    xmlWriter.WriteElementString("SalesCode", String.Empty)
                Else
                    Dim staffcodes = staff.Account.Split("@"c)
                    xmlWriter.WriteElementString("SalesCode", staffcodes(0))
                End If

                '納車希望日
                xmlWriter.WriteElementString("DeliveryHopeDate", Me.GetApiDtCol(apiDtEstimateRow, "DELIDATE"))
                '型式
                xmlWriter.WriteElementString("Model", strModelNumber)
                'SFX
                xmlWriter.WriteElementString("SFX", Me.GetApiDtCol(apiDtEstimateRow, "SUFFIXCD"))

                '外装色コード取得
                Dim colorDt As SC3070207DataSet.SC3070207MstextEriorDataTable = SC3070207TableAdapter.GetColorCode(Me.GetApiDtCol(apiDtEstimateRow, "MODELCD"), _
                                                                                                Me.GetApiDtCol(apiDtEstimateRow, "EXTCOLORCD"))

                Dim strColorCode As String = String.Empty
                If colorDt.Count = 0 Then
                    strColorCode = String.Empty
                Else
                    strColorCode = CType(colorDt.Rows(0).Item("COLOR_CD"), String)
                End If
                colorDt = Nothing

                '外装色コード
                xmlWriter.WriteElementString("ColorCD", strColorCode)

                '車名コード
                xmlWriter.WriteElementString("VehicleNameCode", strSeriesCode)

                '車両本体価格（車両価格　+　外装追加費用　＋　内装追加費用)
                Dim dblBasePrice As Double = CType(Me.GetApiDtCol(apiDtEstimateRow, CLM_BASEPRICE), Double)     '本体車両価格
                Dim dblExtAmount As Double = CType(Me.GetApiDtCol(apiDtEstimateRow, CLM_EXTAMOUNT), Double)     '外装追加費用
                Dim dblIntAmount As Double = CType(Me.GetApiDtCol(apiDtEstimateRow, CLM_INTAMOUNT), Double)     '内装追加費用

                Dim bodyPrice As Double = dblBasePrice + dblExtAmount + dblIntAmount
                xmlWriter.WriteElementString("VhcBodyPrice", bodyPrice.ToString("0.00", CultureInfo.CurrentCulture))

                Dim dblDisCountPrice As Double = CType(Me.GetApiDtCol(apiDtEstimateRow, DISCOUNTPRICE), Double)     '値引き
                '本体値引き
                xmlWriter.WriteElementString("VhcBodyCut", dblDisCountPrice.ToString("0.00", CultureInfo.CurrentCulture))
                '販売価格（車両本体価格　-　値引き額)
                Dim payPrice As Double = bodyPrice - dblDisCountPrice
                xmlWriter.WriteElementString("VhcBodyPay", payPrice.ToString("0.00", CultureInfo.CurrentCulture))
                '納車前手続き
                xmlWriter.WriteElementString("NeedProcedure", "2")
                '緊急納車
                xmlWriter.WriteElementString("Urgency", "2")
                '車両メモ
                xmlWriter.WriteElementString("VhcMemo", Me.GetApiDtCol(apiDtEstimateRow, "MEMO"))
                '支払方式区分
                xmlWriter.WriteElementString("PaymentStyle", drPaymentInfo.PAYMENTMETHOD)
                '頭金
                If drPaymentInfo.IsDEPOSITNull = False Then
                    xmlWriter.WriteElementString("Deposit", drPaymentInfo.DEPOSIT.ToString("0.00", CultureInfo.CurrentCulture))
                End If
                '頭金支払方法区分
                xmlWriter.WriteElementString("DepositPaymentStyle", drPaymentInfo.DEPOSITPAYMENTMETHOD)

                Dim apiDtInsuranceRow As IC3070201EstInsuranceInfoRow = Nothing
                apiDtInsuranceRow = CType(apiDt.Tables(IFTBL_INSINFO).Rows(0), IC3070201EstInsuranceInfoRow)

                '保険区分
                xmlWriter.WriteElementString("Insurance", Me.GetInsuranceDtCol(apiDtInsuranceRow, "INSUDVS"))
                '台数
                xmlWriter.WriteElementString("VhcCount", "1")
                '未取引客ID
                xmlWriter.WriteElementString("CustId", Trim(Me.GetApiDtCol(apiDtEstimateRow, "CRCUSTID")))

                Dim buyerSex As String = String.Empty
                Dim buyerBirthday As String = String.Empty
                '2014/08/01 TCS 山口 NextStep BTS-74 START
                Dim domicile As String = String.Empty
                Dim country As String = String.Empty
                '2014/08/01 TCS 山口 NextStep BTS-74 END

                '顧客情報の取得
                Dim customerDt As SC3070207DataSet.SC3070207CustomerInfoDataTable _
                                    = SC3070207TableAdapter.GetCustomerInfo(apiDtEstimateRow.DLRCD, CType(Trim(apiDtEstimateRow.CRCUSTID), Decimal))

                If Not customerDt Is Nothing AndAlso Not customerDt.Rows.Count <= 0 Then
                    If Not String.IsNullOrEmpty(customerDt(0).SEX) Then
                        buyerSex = customerDt(0).SEX
                    End If
                    Try
                        If Not IsDBNull(customerDt(0).BIRTHDAY) Then
                            'DMSの日付書式を取得
                            Dim sysEnv As New SystemEnvSetting
                            Dim sysEnvRow As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow = sysEnv.GetSystemEnvSetting(DMS_DATETIMEFORMAT)
                            Dim dmsDatetimeFormt As String = sysEnvRow.PARAMVALUE
                            dmsDatetimeFormt = dmsDatetimeFormt.Substring(0, 10)
                            '日付書式指定
                            buyerBirthday = customerDt(0).BIRTHDAY.ToString(dmsDatetimeFormt, CultureInfo.InvariantCulture)
                        End If
                    Catch e As StrongTypingException
                        'DBNullはスルー
                    End Try

                    '2014/08/01 TCS 山口 NextStep BTS-74 START
                    If Not String.IsNullOrEmpty(customerDt(0).CST_DOMICILE) Then
                        domicile = customerDt(0).CST_DOMICILE
                    End If
                    If Not String.IsNullOrEmpty(customerDt(0).CST_COUNTRY) Then
                        country = customerDt(0).CST_COUNTRY
                    End If
                    '2014/08/01 TCS 山口 NextStep BTS-74 END

                End If

                '買主性別
                'TACTに連携する性別は、1:男性, 2:女性 の２種類のみ
                Select Case buyerSex
                    Case "0"    '男性
                        xmlWriter.WriteElementString("BuyerSex", TACT_SEX_MEN)
                    Case "1"    '女性
                        xmlWriter.WriteElementString("BuyerSex", TACT_SEX_FIMALE)
                    Case Else
                        xmlWriter.WriteElementString("BuyerSex", String.Empty)
                End Select

                '買主生年月日
                xmlWriter.WriteElementString("BuyerBirthday", buyerBirthday)

                '2014/08/01 TCS 山口 NextStep BTS-74 START
                'Buyer Domicile
                xmlWriter.WriteElementString("BuyerDomicile", domicile)

                'Buyer Nationality
                xmlWriter.WriteElementString("BuyerCountry", country)
                '2014/08/01 TCS 山口 NextStep BTS-74 END

                '顧客種別
                xmlWriter.WriteElementString("CustomerSegment", customerDt(0).CST_TYPE)

                If customerDt(0).CST_TYPE.Equals("1") Then
                    '自社客コード 
                    xmlWriter.WriteElementString("CustomerCode", Trim(customerDt(0).DMS_CST_CD_DISP))
                Else
                    '未取引客コード 
                    'xmlWriter.WriteElementString("NewCustomerCode", Trim(customerDt(0).NEWCST_CD))
                    xmlWriter.WriteElementString("NewCustomerCode", Trim(Me.GetApiDtCol(apiDtEstimateRow, "CRCUSTID")))
                End If

                xmlWriter.WriteEndElement() 'OrderInfo
                xmlWriter.WriteEndElement() 'Detail
                xmlWriter.WriteEndElement() 'CreateOrder

                apiDtEstimateRow = Nothing
                apiDtInsuranceRow = Nothing
            End Using

            Dim dataXml As String = writer.GetStringBuilder.ToString()
            Dim num As Integer = dataXml.IndexOf(">", StringComparison.CurrentCulture)
            dataXml = dataXml.Remove(0, num + 1)

            ' ログ書き出し
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      " {0}_End, Return:[{1}]",
                                      MethodBase.GetCurrentMethod.Name, dataXml))
            ' ======================== ログ出力 終了 ========================
            Return dataXml
        End Using

    End Function

    ''' <summary>
    ''' 契約情報DtカラムのDBNull判定処理を行います。
    ''' </summary>
    ''' <param name="apiDtRow">契約情報Dt</param>
    ''' <param name="colName">カラム名</param>
    ''' <returns>DBNullの場合は""を返却</returns>
    ''' <remarks></remarks>
    Private Function GetApiDtCol(ByVal apiDtRow As IC3070201EstimationInfoRow,
                                 ByVal colName As String) As String
        'DBNullの場合　""を返却
        If IsDBNull(apiDtRow.Item(colName)) Then
            If (DISCOUNTPRICE.Equals(colName)) OrElse (CLM_BASEPRICE.Equals(colName)) OrElse _
                    (CLM_EXTAMOUNT.Equals(colName)) OrElse (CLM_INTAMOUNT.Equals(colName)) Then
                Return "0"
            End If

            Return String.Empty
        Else
            If CLM_DELIDATE.Equals(colName) Then
                'DMS側の日付書式取得
                Dim strDateFormat As String = Me.GetDmsDateFormat(DMS_DATETIMEFORMAT)

                Return apiDtRow.DELIDATE.ToString(strDateFormat, CultureInfo.CurrentCulture)
            Else
                Return CType(apiDtRow.Item(colName), String)
            End If
        End If
    End Function

    ''' <summary>
    ''' 顧客情報DtカラムのDBNull判定処理を行います。
    ''' </summary>
    ''' <param name="apiCustomerDtRow">顧客情報のカラム</param>
    ''' <param name="colName">カラム名</param>
    ''' <returns>DBNullの場合は""を返却</returns>
    ''' <remarks></remarks>
    Private Function GetCustomerDtCol(ByVal apiCustomerDtRow As IC3070201CustomerInfoRow,
                                      ByVal colName As String) As String
        Dim strZipCode As String = String.Empty

        'DBNullの場合　""を返却
        If IsDBNull(apiCustomerDtRow.Item(colName)) Then
            Return strZipCode
        ElseIf colName = CLM_ZIPCODE Then
            strZipCode = CType(apiCustomerDtRow.Item(colName), String)
            '"-"排除
            strZipCode = strZipCode.Replace("-", "")
        Else
            strZipCode = CType(apiCustomerDtRow.Item(colName), String)
        End If

        Return strZipCode

    End Function

    ''' <summary>
    ''' 保険情報DtのDBNull判定処理を行います。
    ''' </summary>
    ''' <param name="apiInsuranceDtRow">保険情報のカラム</param>
    ''' <param name="colName">カラム名</param>
    ''' <returns>DBNullの場合は""を返却</returns>
    ''' <remarks></remarks>
    Private Function GetInsuranceDtCol(ByVal apiInsuranceDtRow As IC3070201EstInsuranceInfoRow,
                                     ByVal colName As String) As String
        'DBNullの場合　""を返却
        If IsDBNull(apiInsuranceDtRow.Item(colName)) Then
            Return String.Empty
        End If

        Return CStr(apiInsuranceDtRow.Item(colName))
    End Function

    ''' <summary>
    ''' 基幹コードDtのDBNull判定処理を行います。
    ''' </summary>
    ''' <param name="apiDmsCdDtRow">基幹コードのカラム</param>
    ''' <param name="colName">カラム名</param>
    ''' <returns>DBNullの場合は""を返却</returns>
    ''' <remarks></remarks>
    Private Function GetDmsCdDtCol(ByVal apiDmsCdDtRow As IC3070201DmsCdRow,
                                     ByVal colName As String) As String
        'DBNullの場合　""を返却
        If IsDBNull(apiDmsCdDtRow.Item(colName)) Then
            Return String.Empty
        End If

        Return CStr(apiDmsCdDtRow.Item(colName))
    End Function

    ''' <summary>
    ''' 日付書式を取得します。
    ''' </summary>
    ''' <param name="datetimeFormat"></param>
    ''' <returns>指定した日付書式</returns>
    ''' <remarks></remarks>
    Private Function GetDmsDateFormat(ByVal datetimeFormat As String) As String
        'DMSの日付書式を取得
        Dim sysEnv As New SystemEnvSetting
        Dim sysEnvRow As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow
        sysEnvRow = sysEnv.GetSystemEnvSetting(datetimeFormat)
        Dim dmsDatetimeFormt As String = sysEnvRow.PARAMVALUE

        sysEnvRow = Nothing
        sysEnv = Nothing

        Return dmsDatetimeFormt
    End Function

#End Region

End Class
