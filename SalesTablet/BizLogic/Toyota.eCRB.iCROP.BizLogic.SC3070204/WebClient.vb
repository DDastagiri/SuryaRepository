'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'WebClient.vb
'─────────────────────────────────────
'機能： 見積書・契約書印刷処理
'補足： 
'作成： 2012/11/16 TCS 坪根
'更新： 2013/06/30 TCS 山田 2013/10対応版 既存流用
'更新： 2013/10/25 TCS 葛西 次世代e-CRBセールス機能 新DB適応に向けた機能開発
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
Public NotInheritable Class WebClient
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
    ''' コンテキストタイプ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONTEXT_TYPE As String = "application/xml; charset=UTF-8"
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

    '2013/10/25 TCS 葛西 次世代e-CRBセールス機能 新DB適応に向けた機能開発 ADD START
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
    '2013/10/25 TCS 葛西 次世代e-CRBセールス機能 新DB適応に向けた機能開発 ADD END

#End Region

#Region "Publicメソッド"

    ''' <summary>
    ''' リクエストの送信を行い、レスポンスを受信します。
    ''' </summary>
    ''' <param name="paymentKbn">支払方法区分</param>
    ''' <param name="apiDt">見積情報I/F</param>
    ''' <param name="dealerEnvDt">環境設定のTACT連携PATH</param>
    ''' <param name="staff">StaffContext</param>
    ''' <returns>Dictionary(key:レスポンスの項目名　value:レスポンスの値)</returns>
    ''' <remarks></remarks>
    Public Function RequestHttp(ByVal paymentKbn As String, _
                                ByVal apiDt As IC3070201DataSet, _
                                ByVal dealerEnvDt As DlrEnvSettingDataSet.DLRENVSETTINGRow, _
                                ByVal staff As StaffContext) As Dictionary(Of String, String)

        '送信するxmlを作成する
        Dim xmlValue As String = CreateXml(paymentKbn, _
                                           apiDt, _
                                           staff)

        Dim rtnStrs As New Dictionary(Of String, String)
        If dealerEnvDt Is Nothing Then
            Return rtnStrs
        End If

        Dim tactPath As String = dealerEnvDt.PARAMVALUE

        'POST送信するデータ
        Dim postDataBytes As Byte() = System.Text.Encoding.UTF8.GetBytes(xmlValue)

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
            req.ContentType = CONTEXT_TYPE

            'POST送信するデータの長さを設定
            req.ContentLength = postDataBytes.Length

            'データをPOST送信するためのStream取得
            Dim reqStream As System.IO.Stream = req.GetRequestStream
            '送信するデータを書き込む
            reqStream.Write(postDataBytes, 0, postDataBytes.Length)

            reqStream.Close()

            'サーバからの応答を受信
            Using res As WebResponse = req.GetResponse()
                Using resStream As Stream = res.GetResponseStream()
                    'xml読込み
                    Dim xml As XmlTextReader = New XmlTextReader(resStream)
                    While xml.Read()
                        Select Case xml.NodeType
                            Case XmlNodeType.Element

                                If TAG_RESULT_ID.Equals(xml.Name) Then
                                    '結果
                                    rtnStrs.Add(DIC_KEY_ID, xml.ReadString())
                                ElseIf TAG_MESSAGE.Equals(xml.Name) Then
                                    'メッセージ
                                    rtnStrs.Add(DIC_KEY_MSG, xml.ReadString())
                                ElseIf TAG_CONTRACTNO.Equals(xml.Name) Then
                                    '契約No
                                    rtnStrs.Add(DIC_KEY_NO, xml.ReadString())
                                End If
                        End Select
                    End While
                End Using
            End Using
        Catch ex As WebException
            Return rtnStrs
        End Try

        Return rtnStrs
    End Function

#End Region

#Region "Privateメソッド"

    ''' <summary>
    ''' I/F用のxmlを作成します。
    ''' </summary>
    ''' <param name="paymentKbn">支払方法区分</param>
    ''' <param name="apiDt">見積情報I/F</param>
    ''' <param name="staff">StaffContext</param>
    ''' <returns>xmlデータ</returns>
    ''' <remarks></remarks>
    Private Function CreateXml(ByVal paymentKbn As String, _
                               ByVal apiDt As IC3070201DataSet, _
                               ByVal staff As StaffContext) As String

        '2013/06/30 TCS 山田 2013/10対応版 既存流用 START
        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  " {0}_Start",
                                  MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================
        '2013/06/30 TCS 山田 2013/10対応版 既存流用 END

        Using writer As StringWriter = New StringWriter(CultureInfo.InvariantCulture)
            Using xmlWriter As XmlTextWriter = New XmlTextWriter(writer)
                '見積情報取得I/Fで取得したデータ
                Dim apiDtEstimateRow As IC3070201EstimationInfoRow = Nothing
                apiDtEstimateRow = CType(apiDt.Tables(IFTBL_ESTINFO).Rows(0), IC3070201EstimationInfoRow)

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
                xmlWriter.WriteElementString("DealerCode", staff.DlrCD)
                '店舗コード
                xmlWriter.WriteElementString("BranchCode", staff.BrnCD + " ")

                xmlWriter.WriteEndElement() 'Common

                xmlWriter.WriteStartElement("OrderInfo")

                'シーケンスナンバー
                xmlWriter.WriteElementString("SeqNo", DateTimeFunc.Now.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture))
                '売上区分
                xmlWriter.WriteElementString("SalesPart", "1")

                '顧客情報
                For Each apiDtCustomerRow As IC3070201CustomerInfoRow In apiDt.Tables(IFTBL_CUSTINFO).Rows()
                    If CONTRACTCUSTTYPE_DEALER.Equals(apiDtCustomerRow.Item("CONTRACTCUSTTYPE")) Then
                        '所有者のみ設定
                        '買主区分 
                        xmlWriter.WriteElementString("BuyerPart", Me.GetCustomerDtCol(apiDtCustomerRow, "CUSTPART"))
                        '買主名
                        xmlWriter.WriteElementString("BuyerName", Me.GetCustomerDtCol(apiDtCustomerRow, "NAME"))
                        '買主ID
                        xmlWriter.WriteElementString("BuyerID", Me.GetCustomerDtCol(apiDtCustomerRow, "SOCIALID"))
                        '買主郵便番号
                        xmlWriter.WriteElementString("BuyerZIP", Me.GetCustomerDtCol(apiDtCustomerRow, "ZIPCODE"))
                        '買主住所
                        xmlWriter.WriteElementString("BuyerAddress", Me.GetCustomerDtCol(apiDtCustomerRow, "ADDRESS"))

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
                        '名義人名
                        xmlWriter.WriteElementString("Nominee", Me.GetCustomerDtCol(apiDtCustomerRow, "NAME"))
                        '名義人ID
                        xmlWriter.WriteElementString("NomineeID", Me.GetCustomerDtCol(apiDtCustomerRow, "SOCIALID"))
                        '名義人郵便番号
                        xmlWriter.WriteElementString("NomineeZIP", Me.GetCustomerDtCol(apiDtCustomerRow, "ZIPCODE"))
                        '名義人住所
                        xmlWriter.WriteElementString("NomineeAddress", Me.GetCustomerDtCol(apiDtCustomerRow, "ADDRESS"))

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
                xmlWriter.WriteElementString("SalesCode", staff.Account.Substring(0, 6))
                '納車希望日
                xmlWriter.WriteElementString("DeliveryHopeDate", Me.GetApiDtCol(apiDtEstimateRow, "DELIDATE"))
                '型式
                xmlWriter.WriteElementString("Model", strModelNumber)
                'SFX
                xmlWriter.WriteElementString("SFX", Me.GetApiDtCol(apiDtEstimateRow, "SUFFIXCD"))

                '外装色コード取得
                Dim da As New SC3070204TableAdapter
                Dim colorDt As SC3070204DataSet.SC3070204MstextEriorDataTable = da.GetColorCode(Me.GetApiDtCol(apiDtEstimateRow, "MODELCD"), _
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

                '2013/06/30 TCS 山田 2013/10対応版 既存流用 START
                '車名コード
                xmlWriter.WriteElementString("VehicleNameCode", strSeriesCode)
                '2013/06/30 TCS 山田 2013/10対応版 既存流用 END

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
                xmlWriter.WriteElementString("PaymentStyle", paymentKbn)

                Dim apiDtInsuranceRow As IC3070201EstInsuranceInfoRow = Nothing
                apiDtInsuranceRow = CType(apiDt.Tables(IFTBL_INSINFO).Rows(0), IC3070201EstInsuranceInfoRow)

                '保険区分
                xmlWriter.WriteElementString("Insurance", Me.GetInsuranceDtCol(apiDtInsuranceRow, "INSUDVS"))
                '台数
                xmlWriter.WriteElementString("VhcCount", "1")
                '未取引客ID
                xmlWriter.WriteElementString("CustId", Me.GetApiDtCol(apiDtEstimateRow, "CRCUSTID"))

                '2013/10/25 TCS 葛西 次世代e-CRBセールス機能 新DB適応に向けた機能開発 ADD START
                Dim buyerSex As String = String.Empty
                Dim buyerBirthday As String = String.Empty

                '顧客情報の取得
                Dim customerDt As SC3070204DataSet.SC3070204CustomerInfoDataTable _
                                    = SC3070204TableAdapter.GetCustomerInfo(apiDtEstimateRow.CRCUSTID)

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
                '2013/10/25 TCS 葛西 次世代e-CRBセールス機能 新DB適応に向けた機能開発 ADD END

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
            '2013/06/30 TCS 山田 2013/10対応版 既存流用 START
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      " {0}_End, Return:[{1}]",
                                      MethodBase.GetCurrentMethod.Name, dataXml))
            ' ======================== ログ出力 終了 ========================
            '2013/06/30 TCS 山田 2013/10対応版 既存流用 END
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
