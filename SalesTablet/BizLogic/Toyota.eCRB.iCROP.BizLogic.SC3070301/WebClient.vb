'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'WebClient.vb
'─────────────────────────────────────
'機能： 契約書印刷
'補足： 
'作成： 2011/12/01 TCS 相田
'更新： 2012/02/29 TCS 藤井 【SALES_1A】TACT連携時に正しいシリーズコードが連携できない不具合修正
'─────────────────────────────────────

Imports System.Net
Imports System.IO
Imports System.Text
Imports System.Xml
Imports System.Web
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.Estimate.Order.DataAccess
Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess

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
    Private Sub New()
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

#End Region

    ''' <summary>
    ''' リクエストの送信を行い、レスポンスを受信します。
    ''' </summary>
    ''' <param name="staff">StaffContext</param>
    ''' <param name="tblRow">セッションデータセットの行</param>
    ''' <param name="constractDb">契約情報データテーブル</param>
    ''' <param name="dealerEnvDt">環境設定のTACT連携PATH</param>
    ''' <returns>Dictionary(key:レスポンスの項目名　value:レスポンスの値)</returns>
    ''' <remarks></remarks>
    Shared Function RequestHttp(ByVal staff As StaffContext,
                                ByVal tblRow As SC3070301DataSet.SessionRow,
                                ByVal constractDB As SC3070301DataSet.ConstractInfoDataTable,
                                ByVal dealerEnvDT As DlrEnvSettingDataSet.DLRENVSETTINGRow) As Dictionary(Of String, String)

        '送信するxmlを作成する
        Dim xmlValue As String = CreateXml(staff, tblRow, constractDB)

        Dim rtnStrs As New Dictionary(Of String, String)
        If dealerEnvDT Is Nothing Then
            Return rtnStrs
        End If

        Dim tactPath As String = dealerEnvDT.PARAMVALUE

        'POST送信するデータ
        Dim postDataBytes As Byte() = System.Text.Encoding.UTF8.GetBytes(xmlValue)

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

    ''' <summary>
    ''' I/F用のxmlを作成します。
    ''' </summary>
    ''' <param name="staff">StaffContext</param>
    ''' <param name="tblRow">セッションデータセットの行</param>
    ''' <param name="constractDb">契約情報データテーブル</param>
    ''' <returns>xmlデータ</returns>
    ''' <remarks></remarks>
    ''' <History>
    ''' 2012/02/29 TCS 藤井 【SALES_1A】TACT連携時に正しいシリーズコードが連携できない不具合修正
    ''' </History>
    Private Shared Function CreateXml(ByVal staff As StaffContext,
                               ByVal tblRow As SC3070301DataSet.SessionRow,
                               ByVal constractDb As SC3070301DataSet.ConstractInfoDataTable) As String

        Using writer As StringWriter = New StringWriter(CultureInfo.InvariantCulture)
            Using xmlWriter As XmlTextWriter = New XmlTextWriter(writer)

                If constractDb Is Nothing Or staff Is Nothing Or tblRow Is Nothing Then
                    Return String.Empty
                End If

                Dim row As SC3070301DataSet.ConstractInfoRow
                row = CType(constractDb.Rows(0), SC3070301DataSet.ConstractInfoRow)

                xmlWriter.WriteStartDocument()
                xmlWriter.WriteStartElement("CreateOrder")
                xmlWriter.WriteStartElement("Head")
                'メッセージID
                xmlWriter.WriteElementString("MessageID", "IC3800201")
                '国コード
                xmlWriter.WriteElementString("CountryCode", "GZ")
                '基幹SYSTEM識別コード
                xmlWriter.WriteElementString("LinkSystemCode", "0")
                'メッセージ送信日時
                xmlWriter.WriteElementString("TransmissionDate", Date.Now().ToString("dd/MM/yyyy hh:mm:ss", CultureInfo.InvariantCulture))

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
                xmlWriter.WriteElementString("SeqNo", Date.Now().ToString("yyyyMMddhhmmss", CultureInfo.InvariantCulture))
                '売上区分
                xmlWriter.WriteElementString("SalesPart", "1")
                '買主区分 
                xmlWriter.WriteElementString("BuyerPart", row.BUYERCUSTPART)
                '買主名
                xmlWriter.WriteElementString("BuyerName", row.BUYERNAME)
                '買主ID
                xmlWriter.WriteElementString("BuyerID", row.BUYERSOCIALID)
                '買主郵便番号
                xmlWriter.WriteElementString("BuyerZIP", row.BUYERZIPCODE.Replace("-", ""))
                '買主住所
                xmlWriter.WriteElementString("BuyerAddress", row.BUYERADDRESS)

                Dim telno As String = ""
                Dim mobileNo As String = ""
                If Not String.IsNullOrEmpty(row.BUYERTELNO) Then
                    telno = row.BUYERTELNO
                    mobileNo = row.BUYERMOBILE
                Else
                    telno = row.BUYERMOBILE
                End If

                '買主電話１
                xmlWriter.WriteElementString("BuyerTEL1", telno)
                '買主電話2
                xmlWriter.WriteElementString("BuyerTEL2", mobileNo)
                '買主ＦＡＸ
                xmlWriter.WriteElementString("BuyerFAX", row.BUYERFAXNO)
                '買主E-MAIL
                xmlWriter.WriteElementString("BuyerEMail", row.BUYERMAIL)
                '名義人区分
                xmlWriter.WriteElementString("NomineePart", row.HOLDERCUSTPART)
                '名義人名
                xmlWriter.WriteElementString("Nominee", row.HOLDERNAME)
                '名義人ID
                xmlWriter.WriteElementString("NomineeID", row.HOLDERSOCIALID)
                '名義人郵便番号
                xmlWriter.WriteElementString("NomineeZIP", row.HOLDERZIPCODE.Replace("-", ""))
                '名義人住所
                xmlWriter.WriteElementString("NomineeAddress", row.HOLDERADDRESS)

                Dim holTelno As String = ""
                Dim holMobileNo As String = ""
                If Not String.IsNullOrEmpty(row.HOLDERTELNO) Then
                    holTelno = row.HOLDERTELNO
                    holMobileNo = row.HOLDERMOBILE
                Else
                    holTelno = row.HOLDERMOBILE
                End If

                '名義人電話１
                xmlWriter.WriteElementString("NomineeTEL1", holTelno)
                '名義人電話2
                xmlWriter.WriteElementString("NomineeTEL2", holMobileNo)
                '名義人ＦＡＸ
                xmlWriter.WriteElementString("NomineeFAX", row.HOLDERFAXNO)
                '名義人EーMAIL
                xmlWriter.WriteElementString("NomineeEMail", row.HOLDERMAIL)
                'セールスコード
                xmlWriter.WriteElementString("SalesCode", staff.Account.Substring(0, 6))
                '納車希望日
                xmlWriter.WriteElementString("DeliveryHopeDate", row.DELIDATE)
                '型式
                xmlWriter.WriteElementString("Model", row.MODELNUMBER)
                'SFX
                xmlWriter.WriteElementString("SFX", row.SUFFIXCD)
                '外装色コード
                xmlWriter.WriteElementString("ColorCD", row.EXTCOLORCD)
                '車名コード

                Dim code As String = String.Empty
                If SERIES_CD_HV.Equals(row.MODELNUMBER) Then
                    code = VEHICLE_NAME_CODE
                Else
                    '2012/02/29 TCS 藤井 【SALES_1A】TACT連携時に正しいシリーズコードが連携できない不具合修正 Modify Start
                    'code = row.SERIESCD
                    ' シリーズコードの基幹車名コードへの変換
                    Dim seriesCdDt As SC3070301DataSet.SeriesCodeDataTable = SC3070301TableAdapter.GetCarNameCode(row.SERIESCD)

                    If Not seriesCdDt.Rows.Count < 0 AndAlso Not String.IsNullOrEmpty(seriesCdDt(0).CAR_NAME_CD_AI21) Then
                        code = seriesCdDt(0).CAR_NAME_CD_AI21
                    Else
                        code = String.Empty
                    End If
                    '2012/02/29 TCS 藤井 【SALES_1A】TACT連携時に正しいシリーズコードが連携できない不具合修正 Modify End
                End If
                xmlWriter.WriteElementString("VehicleNameCode", code)
                '車両本体価格（車両価格　+　外装追加費用　＋　内装追加費用)
                Dim bodyPrice As Double = row.BASEPRICE + row.EXTAMOUNT + row.INTAMOUNT
                xmlWriter.WriteElementString("VhcBodyPrice", bodyPrice.ToString("0.00", CultureInfo.CurrentCulture))
                '本体値引き
                xmlWriter.WriteElementString("VhcBodyCut", row.DISCOUNTPRICE.ToString("0.00", CultureInfo.CurrentCulture))
                '販売価格（車両本体価格　-　値引き額)
                Dim payPrice As Double = bodyPrice - row.DISCOUNTPRICE
                xmlWriter.WriteElementString("VhcBodyPay", payPrice.ToString("0.00", CultureInfo.CurrentCulture))
                '納車前手続き
                xmlWriter.WriteElementString("NeedProcedure", "2")
                '緊急納車
                xmlWriter.WriteElementString("Urgency", "2")
                '車両メモ
                xmlWriter.WriteElementString("VhcMemo", row.MEMO)
                '支払方式区分
                xmlWriter.WriteElementString("PaymentStyle", tblRow.PAYMENTMETHOD)
                '保険会社種類
                xmlWriter.WriteElementString("Insurance", row.INSUKIND)
                '台数
                xmlWriter.WriteElementString("VhcCount", "1")
                '未取引客ID
                xmlWriter.WriteElementString("CustId", row.CRCUSTID)

                xmlWriter.WriteEndElement() 'OrderInfo
                xmlWriter.WriteEndElement() 'Detail
                xmlWriter.WriteEndElement() 'CreateOrder


            End Using

            Dim dataXml As String = writer.GetStringBuilder.ToString()
            Dim num As Integer = dataXml.IndexOf(">", StringComparison.CurrentCulture)
            dataXml = dataXml.Remove(0, num + 1)

            ' ログ書き出し
            Logger.Info(dataXml, True)

            Return dataXml
        End Using
    End Function
End Class
