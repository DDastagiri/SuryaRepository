'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3070101BusinessLogic.vb
'──────────────────────────────────
'機能： 在庫状況
'補足： 
'作成： 
'更新： 2016/05/11 NSK 中村 （トライ店システム評価）他システム連携における複数店舗コード変換対応 $01
'──────────────────────────────────

Imports System.Xml
Imports System.IO
Imports System.Text
Imports System.Web
Imports System.Net
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.Estimate.Ims.DataAccess.SC3070101SearchConditionDataSet
Imports Toyota.eCRB.Estimate.Ims.DataAccess.SC3070101SearchResultDataSet
Imports Toyota.eCRB.Estimate.Ims.DataAccess.SC3070101SearchConditionDataSetTableAdapters
Imports System.Globalization

''' <summary>
''' SC3070101(在庫状況)
''' ビジネスロジック層
''' </summary>
''' <remarks></remarks>
Public Class SC3070101BusinessLogic
    Inherits BaseBusinessComponent

    '$01 他システム連携における複数店舗コード変換対応 start 
    Private GlDmsCodeMapUseColumn As String
    '$01 他システム連携における複数店舗コード変換対応 end 

#Region "定数"

    Public Const CstStrPGID As String = "SC3070101"

#Region "XML関連"

#Region "XMLタグ名"

    ''' <summary>
    '''XMLタグ名：GetVhcInStock
    ''' </summary>
    Private Const XmlTagNameGetVhcInStock As String = "GetVhcInStock"
    ''' <summary>
    '''XMLタグ名：Head
    ''' </summary>
    Private Const XmlTagNameHead As String = "Head"
    ''' <summary>
    '''XMLタグ名：MessageID
    ''' </summary>
    Private Const XmlTagNameMessageID As String = "MessageID"
    ''' <summary>
    '''XMLタグ名：CountryCode
    ''' </summary>
    Private Const XmlTagNameCountryCode As String = "CountryCode"
    ''' <summary>
    '''XMLタグ名：LinkSystemCode
    ''' </summary>
    Private Const XmlTagNameLinkSystemCode As String = "LinkSystemCode"
    ''' <summary>
    '''XMLタグ名：TransmissionDate
    ''' </summary>
    Private Const XmlTagNameTransmissionDate As String = "TransmissionDate"
    ''' <summary>
    '''XMLタグ名：Detail
    ''' </summary>
    Private Const XmlTagNameDetail As String = "Detail"
    ''' <summary>
    '''XMLタグ名：Common
    ''' </summary>
    Private Const XmlTagNameCommon As String = "Common"
    ''' <summary>
    '''XMLタグ名：DealerCode
    ''' </summary>
    Private Const XmlTagNameDealerCode As String = "DealerCode"
    ''' <summary>
    '''XMLタグ名：BranchCode
    ''' </summary>
    Private Const XmlTagNameBranchCode As String = "BranchCode"
    ''' <summary>
    '''XMLタグ名：VhcType
    ''' </summary>
    Private Const XmlTagNameVhcType As String = "VhcType"
    ''' <summary>
    '''XMLタグ名：SeqNo
    ''' </summary>
    Private Const XmlTagNameSeqNo As String = "SeqNo"
    ''' <summary>
    '''XMLタグ名：VhcNameCode
    ''' </summary>
    Private Const XmlTagNameVhcNameCode As String = "VhcNameCode"
    ''' <summary>
    '''XMLタグ名：Model
    ''' </summary>
    Private Const XmlTagNameModel As String = "Model"
    ''' <summary>
    '''XMLタグ名：SFX
    ''' </summary>
    Private Const XmlTagNameSFX As String = "SFX"
    ''' <summary>
    '''XMLタグ名：Color
    ''' </summary>
    Private Const XmlTagNameColor As String = "Color"
    ''' <summary>
    '''XMLタグ名：DistributeFlag
    ''' </summary>
    Private Const XmlTagNameDistributeFlag As String = "DistributeFlag"
    ''' <summary>
    '''XMLタグ名：DeliveryFlg
    ''' </summary>
    Private Const XmlTagNameDeliveryFlg As String = "DeliveryFlg"

    ''' <summary>
    '''XMLタグ名：Response
    ''' </summary>
    Private Const XmlTagNameResponse As String = "Response"
    ''' <summary>
    '''XMLタグ名：ReceptionDate
    ''' </summary>
    Private Const XmlTagNameReceptionDate As String = "ReceptionDate"
    ''' <summary>
    '''XMLタグ名：ResultId
    ''' </summary>
    Private Const XmlTagNameResultId As String = "ResultId"
    ''' <summary>
    '''XMLタグ名：Message
    ''' </summary>
    Private Const XmlTagNameMessage As String = "Message"
    ''' <summary>
    '''XMLタグ名：SearchResult
    ''' </summary>
    Private Const XmlTagNameSearchResult As String = "SearchResult"
    ''' <summary>
    '''XMLタグ名：VhcInStockInfo
    ''' </summary>
    Private Const XmlTagNameVhcInStockInfo As String = "VhcInStockInfo"
    ''' <summary>
    '''XMLタグ名：OrderNo
    ''' </summary>
    Private Const XmlTagNameOrderNo As String = "OrderNo"
    ''' <summary>
    '''XMLタグ名：URN
    ''' </summary>
    Private Const XmlTagNameURN As String = "URN"
    ''' <summary>
    '''XMLタグ名：FrameNo
    ''' </summary>
    Private Const XmlTagNameFrameNo As String = "FrameNo"
    ''' <summary>
    '''XMLタグ名：AcceptDate
    ''' </summary>
    Private Const XmlTagNameAcceptDate As String = "AcceptDate"
    ''' <summary>
    '''XMLタグ名：NewlyDeliveryDate
    ''' </summary>
    Private Const XmlTagNameNewlyDeliveryDate As String = "NewlyDeliveryDate"
    ''' <summary>
    '''XMLタグ名：DistributeDate
    ''' </summary>
    Private Const XmlTagNameDistributeDate As String = "DistributeDate"
    ''' <summary>
    '''XMLタグ名：ContractNo
    ''' </summary>
    Private Const XmlTagNameContractNo As String = "ContractNo"
    ''' <summary>
    '''XMLタグ名：DeliveryHopeDate
    ''' </summary>
    Private Const XmlTagNameDeliveryHopeDate As String = "DeliveryHopeDate"

    '$01 start GL2版対応
    ''' <summary>
    '''XMLタグ名：ModelCode
    ''' </summary>
    Private Const XmlTagNameModelCode As String = "ModelCode"

    ''' <summary>
    '''XMLタグ名：GradeCode
    ''' </summary>
    Private Const XmlTagNameGradeCode As String = "GradeCode"

    ''' <summary>
    '''XMLタグ名：Suffix
    ''' </summary>
    Private Const XmlTagNameSuffix As String = "Suffix"

    ''' <summary>
    '''XMLタグ名：BodyColorCode
    ''' </summary>
    Private Const XmlTagNameBodyColorCode As String = "BodyColorCode"

    ''' <summary>
    '''XMLタグ名：OrderVclJudgementDay1st
    ''' </summary>
    Private Const XmlTagNameOrderVclJudgementDay1st As String = "OrderVclJudgmentDay1st"

    ''' <summary>
    '''XMLタグ名：OrderVclJudgementDay2nd
    ''' </summary>
    Private Const XmlTagNameOrderVclJudgementDay2nd As String = "OrderVclJudgmentDay2nd"

    ''' <summary>
    '''XMLタグ名：StockVclJudgementDay1st
    ''' </summary>
    Private Const XmlTagNameStockVclJudgementDay1st As String = "StockVclJudgmentDay1st"

    ''' <summary>
    '''XMLタグ名：XmlTagNameStockVclJudgementDay2nd
    ''' </summary>
    Private Const XmlTagNameStockVclJudgementDay2nd As String = "StockVclJudgmentDay2nd"

    ''' <summary>
    '''XMLタグ名：OrderNumber1st
    ''' </summary>
    Private Const XmlTagNameOrderNumber1st As String = "OrderNumber1st"

    ''' <summary>
    '''XMLタグ名：OrderNumber2nd
    ''' </summary>
    Private Const XmlTagNameOrderNumber2nd As String = "OrderNumber2nd"

    ''' <summary>
    '''XMLタグ名：OrderNumber3rd
    ''' </summary>
    Private Const XmlTagNameOrderNumber3rd As String = "OrderNumber3rd"

    ''' <summary>
    '''XMLタグ名：StockNumber1st
    ''' </summary>
    Private Const XmlTagNameStockNumber1st As String = "StockNumber1st"

    ''' <summary>
    '''XMLタグ名：XmlTagNameStockVclJudgementDay2nd
    ''' </summary>
    Private Const XmlTagNameStockNumber2nd As String = "StockNumber2nd"

    ''' <summary>
    '''XMLタグ名：XmlTagNameStockNumber3rd
    ''' </summary>
    Private Const XmlTagNameStockNumber3rd As String = "StockNumber3rd"

    '$01 end GL2版対応

#End Region

#End Region

#Region "Application固有"

    '$01 他システム連携における複数店舗コード変換対応 start 
    ''' <summary>
    ''' 基幹コードマップ　デフォルト基幹コードカラム
    ''' </summary>
    ''' <remarks></remarks>
    Public Const DmsCodeMapDmsColumnDefault As String = "DMS_CD_2"
    '$01 他システム連携における複数店舗コード変換対応 end 

#Region "メッセージID"

    ''' <summary>
    ''' メッセージID:成功
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MessageIdSuccess As String = "0"

    ''' <summary>
    ''' メッセージID:引数エラー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MessageIdCarNameError As String = "2001"

    ''' <summary>
    ''' メッセージID:WebServiceエラー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MessageIdWebServiceError As String = "9999"

#End Region

#Region "DB関連"

    ''' <summary>
    ''' システム管理マスタ.パラメータ名:DMSの在庫状況取得インターフェースのURL
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DmsStockStatusUrl As String = "DMS_STOCKSTATUS_URL"

    '$01 他システム連携における複数店舗コード変換対応 start 
    ''' <summary>
    ''' プログラム設定．キー名：使用基幹コードカラム
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DmsCodeMapBrnCdKey As String = "DMS_CODE_MAP_BRN_CD"
    '$01 他システム連携における複数店舗コード変換対応 end 

#End Region

#End Region

#End Region

#Region "在庫状況取得処理"

    ''' <summary>
    ''' 在庫状況取得処理
    ''' </summary>
    ''' <param name="carName">車名</param>
    ''' <param name="model">MODEL</param>
    ''' <param name="suffix">SFX</param>
    ''' <param name="exteriorColor">外装色</param>
    ''' <param name="stockdisplayClass">GL版</param>
    ''' <param name="orderVclFreshThreshold1st">注文車両の鮮度判定用日数(1st)</param>
    ''' <param name="orderVclFreshThreshold2nd">注文車両の鮮度判定用日数(2nd)</param>
    ''' <param name="stockVclFreshThreshold1st">在庫車両の鮮度判定用日数(1st)</param>
    ''' <param name="stockVclFreshThreshold2nd">在庫車両の鮮度判定用日数(2nd)</param>
    ''' <returns>データテーブル</returns>
    ''' <remarks></remarks>
    Public Function GetStockStatus(ByVal carName As String, ByVal model As String,
                    ByVal suffix As String, ByVal exteriorColor As String, _
                    ByVal stockDisplayClass As String, _
                    ByVal orderVclFreshThreshold1st As Integer, ByVal orderVclFreshThreshold2nd As Integer, _
                    ByVal stockVclFreshThreshold1st As Integer, ByVal stockVclFreshThreshold2nd As Integer) As ResultDataTableDataTable
        Logger.Info("GetStockStatus_Start Pram[" & carName & ", " & model & ", " & suffix & ", " & exteriorColor & ", " & _
                    orderVclFreshThreshold1st & ", " & orderVclFreshThreshold2nd & ", " & stockVclFreshThreshold1st & _
                    ", " & stockVclFreshThreshold2nd & "]")

        ' 引数チェック
        If String.IsNullOrEmpty(Trim(carName)) Then
            Throw New ApplicationException(MessageIdCarNameError)
        End If

        ' WebServiceURLを取得
        Dim sysEnvSet As New SystemEnvSetting
        Dim sysEnvSetRow As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow
        Dim webServiceUrl As String = String.Empty
        Logger.Info("GetStockStatus_001 " & "Call_Start SystemEnvSetting.GetSystemEnvSetting Pram[" & DmsStockStatusUrl & "]")
        sysEnvSetRow = sysEnvSet.GetSystemEnvSetting(DmsStockStatusUrl)
        Logger.Info("GetStockStatus_001 " & "Call_End   SystemEnvSetting.GetSystemEnvSetting Ret[" & (sysEnvSetRow IsNot Nothing) & "]")
        webServiceUrl = sysEnvSetRow.PARAMVALUE

        '$01 他システム連携における複数店舗コード変換対応 start 
        Dim prgSettingV4 As ProgramSettingV4 = New ProgramSettingV4()
        '基幹コードマップ（販売店コード取得）にて使用するカラムを取得
        Dim prgSettingRow = prgSettingV4.GetProgramSettingV4(CstStrPGID, CstStrPGID, DmsCodeMapBrnCdKey)
        If (prgSettingRow Is Nothing) Then
            'プログラム設定が取得できない場合はデフォルト値を設定
            GlDmsCodeMapUseColumn = DmsCodeMapDmsColumnDefault
        Else
            GlDmsCodeMapUseColumn = prgSettingRow.SETTING_VAL
        End If
        '$01 他システム連携における複数店舗コード変換対応 end

        Dim sendXml As String = String.Empty

        If stockDisplayClass.Equals("0") Then
            ' XMLを生成
            ' SFX、外装色での絞込みはしない。
            sendXml = CreateXml(carName, model, String.Empty, String.Empty)
            sendXml = "input=" & sendXml
        Else
            Dim grade As String = model
            If Not String.IsNullOrEmpty(Trim(suffix)) Then
                grade = grade & "/" & suffix
            End If
            sendXml = CreateXmlGL2(carName, grade, String.Empty, String.Empty, _
                                   orderVclFreshThreshold1st, orderVclFreshThreshold2nd, _
                                   stockVclFreshThreshold1st, stockVclFreshThreshold2nd)
            sendXml = "xsData=" + HttpUtility.UrlEncode(sendXml) + ""
        End If

        '送信XMLをログに出力
        Logger.Info(sendXml)

        ' WebService呼び出し
        Dim resultXmlDoc As XmlDocument = New XmlDocument
        Dim resultString As String = CallWebServiceSite(sendXml, webServiceUrl)
        resultXmlDoc.LoadXml(resultString)

        ' 返却コード解析
        For Each resultIdNode As XmlNode In resultXmlDoc.SelectNodes("descendant::ResultId")
            If Not resultIdNode.InnerXml.Equals("0") Then

                Logger.Error("GetStockStatus_002 WebServiceSite Is Error")
                Logger.Error("SendXml")
                Logger.Error(sendXml)
                Logger.Error("ReceiveXml")
                Logger.Error(resultString)

                'エラーコード発生
                Throw New ApplicationException(MessageIdWebServiceError)
            End If
        Next

        ' 返却XML解析
        Dim returnTable As ResultDataTableDataTable = New ResultDataTableDataTable
        If stockDisplayClass.Equals("0") Then
            returnTable = ReadXml(resultXmlDoc)
        Else
            returnTable = ReadXmlGL2(resultXmlDoc)
        End If


        Logger.Info("GetXmlValue_End Ret[" & (returnTable IsNot Nothing) & "]")
        Return returnTable

    End Function

    ''' <summary>
    ''' WebService呼び出し用XMLを作成する。(GL1)
    ''' </summary>
    ''' <param name="carName">車名</param>
    ''' <param name="model">MODEL</param>
    ''' <param name="suffix">SFX</param>
    ''' <param name="exteriorColor">外装色</param>
    ''' <returns>WebService呼び出し用XML</returns>
    ''' <remarks></remarks>
    Private Function CreateXml(ByVal carName As String, ByVal model As String,
                    ByVal suffix As String, ByVal exteriorColor As String) As String

        Logger.Info("CreateXml_Start Pram[" & carName & ", " & model & ", " & suffix & ", " & exteriorColor & "]")

        ' XML生成
        Using stringWriter As StringWriter = New StringWriter(CultureInfo.CurrentCulture())
            Dim xmlWriter As XmlTextWriter = New XmlTextWriter(stringWriter)

            ' スタッフ情報取得
            Dim staffInfo As StaffContext = StaffContext.Current

            'XML開始
            xmlWriter.WriteStartDocument()

            'GetVhcInStockタグ開始
            xmlWriter.WriteStartElement(XmlTagNameGetVhcInStock)

            'Headタグ開始
            xmlWriter.WriteStartElement(XmlTagNameHead)

            xmlWriter.WriteElementString(XmlTagNameMessageID, "IC3800101")
            xmlWriter.WriteElementString(XmlTagNameCountryCode, EnvironmentSetting.CountryCode)
            xmlWriter.WriteElementString(XmlTagNameLinkSystemCode, "0")
            xmlWriter.WriteElementString(XmlTagNameTransmissionDate, DateTimeFunc.FormatDate(1, Date.Now()))

            'Headタグ終了
            xmlWriter.WriteEndElement()

            'Detailタグ開始
            xmlWriter.WriteStartElement(XmlTagNameDetail)

            'Commonタグ開始
            xmlWriter.WriteStartElement(XmlTagNameCommon)

            xmlWriter.WriteElementString(XmlTagNameDealerCode, staffInfo.DlrCD)
            xmlWriter.WriteElementString(XmlTagNameBranchCode, staffInfo.BrnCD.PadRight(3))

            'Commonタグ終了
            xmlWriter.WriteEndElement()

            'VhcTypeタグ開始
            xmlWriter.WriteStartElement(XmlTagNameVhcType)

            xmlWriter.WriteElementString(XmlTagNameSeqNo, Date.Now().ToString("yyyyMMddhhmmss", CultureInfo.InvariantCulture) & "01")
            xmlWriter.WriteElementString(XmlTagNameVhcNameCode, carName)

            ' model
            If Not String.IsNullOrEmpty(Trim(model)) Then
                Logger.Info("CreateXml_002 model NotIs Nothing")
                ' 値が設定されている場合のみタグ追加
                xmlWriter.WriteElementString(XmlTagNameModel, model)
            End If

            ' suffix
            If Not String.IsNullOrEmpty(Trim(suffix)) Then
                Logger.Info("CreateXml_003 Suffix NotIs Nothing")
                ' 値が設定されている場合のみタグ追加
                xmlWriter.WriteElementString(XmlTagNameSFX, suffix)
            End If

            ' exteriorColor
            If Not String.IsNullOrEmpty(Trim(exteriorColor)) Then
                Logger.Info("CreateXml_004 ExteriorColor NotIs Nothing")
                ' 値が設定されている場合のみタグ追加
                xmlWriter.WriteElementString(XmlTagNameColor, exteriorColor)
            End If

            xmlWriter.WriteElementString(XmlTagNameDistributeFlag, "0")

            'VhcTypeタグ終了
            xmlWriter.WriteEndElement()

            'Detailタグ終了
            xmlWriter.WriteEndElement()

            'GetVhcInStockタグ終了
            xmlWriter.WriteEndElement()

            'XML終了
            xmlWriter.WriteEndDocument()

            Dim dataXml As String = stringWriter.GetStringBuilder.ToString()
            Dim num As Integer = dataXml.IndexOf(">", StringComparison.CurrentCulture)
            dataXml = dataXml.Remove(0, num + 1)

            Logger.Info("CreateXml_End Ret[" & dataXml & "]")
            Return dataXml
        End Using

    End Function

    ' $01 start GL2版対応
    ''' <summary>
    ''' WebService呼び出し用XMLを作成する。(GL2)
    ''' </summary>
    ''' <param name="carName">モデルコード</param>
    ''' <param name="model">グレードコード</param>
    ''' <param name="suffix">SFX</param>
    ''' <param name="exteriorColor">外装色</param>
    ''' <returns>WebService呼び出し用XML</returns>
    ''' <remarks></remarks>
    Private Function CreateXmlGL2(ByVal carName As String, ByVal model As String,
                    ByVal suffix As String, ByVal exteriorColor As String, _
                    ByVal orderVclFreshThreshold1st As Integer, ByVal orderVclFreshThreshold2nd As Integer, _
                    ByVal stockVclFreshThreshold1st As Integer, ByVal stockVclFreshThreshold2nd As Integer) As String

        Logger.Info("CreateXml_Start Pram[" & carName & ", " & model & ", " & suffix & ", " & exteriorColor & "]")

        ' XML生成
        Using stringWriter As StringWriter = New StringWriter(CultureInfo.CurrentCulture())
            Dim xmlWriter As XmlTextWriter = New XmlTextWriter(stringWriter)

            ' スタッフ情報取得
            Dim staffInfo As StaffContext = StaffContext.Current

            'XML開始
            xmlWriter.WriteStartDocument()

            'GetVhcInStockタグ開始
            xmlWriter.WriteStartElement(XmlTagNameGetVhcInStock)

            'Headタグ開始
            xmlWriter.WriteStartElement(XmlTagNameHead)

            xmlWriter.WriteElementString(XmlTagNameMessageID, "IC3800102")
            xmlWriter.WriteElementString(XmlTagNameCountryCode, EnvironmentSetting.CountryCode)
            xmlWriter.WriteElementString(XmlTagNameLinkSystemCode, "0")
            xmlWriter.WriteElementString(XmlTagNameTransmissionDate, DateTimeFunc.FormatDate(1, Date.Now()))

            'Headタグ終了
            xmlWriter.WriteEndElement()

            'Detailタグ開始
            xmlWriter.WriteStartElement(XmlTagNameDetail)

            'Commonタグ開始
            xmlWriter.WriteStartElement(XmlTagNameCommon)

            ' 基幹販売店コードの取得
            Dim changedDealerCdRow As SC3070101DmsCodeMapRow
            Using adapter As New SC3070101SearchConditionTableAdapter
                changedDealerCdRow = CType(adapter.GetDmsCd1("1", staffInfo.DlrCD).Rows(0), SC3070101DmsCodeMapRow)
            End Using
            xmlWriter.WriteElementString(XmlTagNameDealerCode, changedDealerCdRow.DMS_CD_1)

            ' 基幹店舗コードの取得
            Dim changedBranchCdRow As SC3070101DmsCodeMapRow
            Using adapter As New SC3070101SearchConditionTableAdapter
                changedBranchCdRow = CType(adapter.GetDmsCd2("2", staffInfo.DlrCD, staffInfo.BrnCD).Rows(0), SC3070101DmsCodeMapRow)
            End Using
            xmlWriter.WriteElementString(XmlTagNameBranchCode, changedBranchCdRow(GlDmsCodeMapUseColumn).ToString)

            'Commonタグ終了
            xmlWriter.WriteEndElement()

            'VhcTypeタグ開始
            xmlWriter.WriteStartElement(XmlTagNameVhcType)

            ' モデルコード
            xmlWriter.WriteElementString(XmlTagNameModelCode, carName)
            ' グレードコード
            xmlWriter.WriteElementString(XmlTagNameGradeCode, model)

            ' モデルサフィックス
            If Not String.IsNullOrEmpty(Trim(suffix)) Then
                Logger.Info("CreateXml_003 Suffix NotIs Nothing")
                ' 値が設定されている場合のみタグ追加
                xmlWriter.WriteElementString(XmlTagNameSuffix, suffix)
            End If

            ' 外装色コード
            If Not String.IsNullOrEmpty(Trim(exteriorColor)) Then
                Logger.Info("CreateXml_004 ExteriorColor NotIs Nothing")
                ' 値が設定されている場合のみタグ追加
                xmlWriter.WriteElementString(XmlTagNameBodyColorCode, exteriorColor)
            End If

            ' 当日の日付を取得
            Dim today As Date = Date.Now().Date

            ' 注文車両鮮度判定日(1st)
            xmlWriter.WriteElementString(XmlTagNameOrderVclJudgementDay1st, DateTimeFunc.FormatDate(3, today.AddDays(orderVclFreshThreshold1st)))
            ' 注文車両鮮度判定日(2nd)
            xmlWriter.WriteElementString(XmlTagNameOrderVclJudgementDay2nd, DateTimeFunc.FormatDate(3, today.AddDays(orderVclFreshThreshold2nd)))
            ' 在庫車両鮮度判定日(1st)
            xmlWriter.WriteElementString(XmlTagNameStockVclJudgementDay1st, DateTimeFunc.FormatDate(3, today.AddDays(stockVclFreshThreshold1st * -1)))
            ' 在庫車両鮮度判定日(2nd)
            xmlWriter.WriteElementString(XmlTagNameStockVclJudgementDay2nd, DateTimeFunc.FormatDate(3, today.AddDays(stockVclFreshThreshold2nd * -1)))

            'VhcTypeタグ終了
            xmlWriter.WriteEndElement()

            'Detailタグ終了
            xmlWriter.WriteEndElement()

            'GetVhcInStockタグ終了
            xmlWriter.WriteEndElement()

            'XML終了
            xmlWriter.WriteEndDocument()

            Dim dataXml As String = stringWriter.GetStringBuilder.ToString()
            Dim num As Integer = dataXml.IndexOf(">", StringComparison.CurrentCulture)
            dataXml = dataXml.Remove(0, num + 1)

            Logger.Info("CreateXml_End Ret[" & dataXml & "]")
            Return dataXml
        End Using

    End Function
    ' $01 end GL2版対応


    ''' <summary>
    ''' WebServiceのサイトを呼び出す
    ''' </summary>
    ''' <param name="postData">送信文字列</param>
    ''' <param name="WebServiceUrl">送信先アドレス</param>
    ''' <returns>返却XML</returns>
    ''' <remarks></remarks>
    Private Function CallWebServiceSite(ByVal postData As String, ByVal WebServiceUrl As String) As String

        Logger.Info("CallWebServiceSite_Start Pram[" & postData & ", " & WebServiceUrl & "]")

        Dim returnString As String

        '文字コードを指定する
        Dim enc As System.Text.Encoding = Encoding.GetEncoding("UTF-8")

        'POST送信するデータ
        Dim postDataBytes As Byte() = Encoding.ASCII.GetBytes(postData)

        Try

            Dim path As String = New Uri(WebServiceUrl).ToString
            Dim req As HttpWebRequest = CType(WebRequest.Create(New Uri(path)), HttpWebRequest)

            'メソッドをPOSTに設定
            req.Method = "POST"

            'ContentTypeの設定
            req.ContentType = "application/x-www-form-urlencoded;"

            'POST送信するデータの長さを設定
            req.ContentLength = postDataBytes.Length

            'データをPOST送信するためのStream取得
            Dim reqStream As Stream = req.GetRequestStream

            '送信するデータを書き込む
            reqStream.Write(postDataBytes, 0, postDataBytes.Length)

            reqStream.Close()

            'サーバーからの応答を受信するためのWebResponseを取得
            Dim res As WebResponse = req.GetResponse()
            '応答データを受信するためのStreamを取得
            Dim resStream As Stream = res.GetResponseStream()
            '受信
            Dim sr As New StreamReader(resStream, enc)

            '返却文字列を取得
            returnString = sr.ReadToEnd()

            '閉じる
            sr.Close()

        Catch ex As WebException

            Throw New ApplicationException(MessageIdWebServiceError)

        End Try

        Logger.Info("CallWebServiceSite_End Ret[" & returnString & "]")
        Return returnString

    End Function

    ''' <summary>
    ''' WebServiceからの返却XMLを解析する。
    ''' </summary>
    ''' <param name="xml">解析対象XML</param>
    ''' <returns>解析した値を保持したResultDataTableDataTable</returns>
    ''' <remarks></remarks>
    Private Function ReadXml(ByVal xml As XmlDocument) As ResultDataTableDataTable

        Logger.Info("ReadXml_Start Pram[" & (xml IsNot Nothing) & "]")

        '返却用DataSet
        Dim returnTable As ResultDataTableDataTable = New ResultDataTableDataTable

        '各NodeList
        Dim xmlDetailList As XmlNodeList
        Dim xmlSearchResultList As XmlNodeList
        Dim xmlVhcInStockInfoList As XmlNodeList

        'Detailタグ情報取得
        xmlDetailList = xml.GetElementsByTagName(XmlTagNameDetail)

        For Each xmlDetailElement As XmlElement In xmlDetailList

            'SearchResulタグ情報取得
            xmlSearchResultList = xmlDetailElement.GetElementsByTagName(XmlTagNameSearchResult)

            For Each xmlSearchResultElement As XmlElement In xmlSearchResultList

                'VhcInStockInfoタグ情報取得
                xmlVhcInStockInfoList = xmlSearchResultElement.GetElementsByTagName(XmlTagNameVhcInStockInfo)

                If xmlVhcInStockInfoList.Count > 0 Then
                    For Each xmlVhcInStockElement As XmlElement In xmlVhcInStockInfoList

                        ' 値を取得しDataSetへ設定
                        Dim resultDataRow As ResultDataTableRow = returnTable.NewResultDataTableRow

                        resultDataRow.VhcNameCode = GetXmlValue(xmlVhcInStockElement.GetElementsByTagName(XmlTagNameVhcNameCode))
                        resultDataRow.Model = GetXmlValue(xmlVhcInStockElement.GetElementsByTagName(XmlTagNameModel))
                        resultDataRow.Suffix = GetXmlValue(xmlVhcInStockElement.GetElementsByTagName(XmlTagNameSFX))
                        resultDataRow.Color = GetXmlValue(xmlVhcInStockElement.GetElementsByTagName(XmlTagNameColor))
                        resultDataRow.AcceptDate = GetXmlValue(xmlVhcInStockElement.GetElementsByTagName(XmlTagNameAcceptDate))
                        resultDataRow.NewlyDeliveryDate = GetXmlValue(xmlVhcInStockElement.GetElementsByTagName(XmlTagNameNewlyDeliveryDate))

                        returnTable.AddResultDataTableRow(resultDataRow)

                    Next
                End If
            Next
        Next

        Logger.Info("ReadXml_End Ret[" & (returnTable IsNot Nothing) & "]")
        Return returnTable

    End Function
    ' $02 start GL2版対応
    ''' <summary>
    ''' WebServiceからの返却XMLを解析する。
    ''' </summary>
    ''' <param name="xml">解析対象XML</param>
    ''' <returns>解析した値を保持したResultDataTableDataTable</returns>
    ''' <remarks></remarks>
    Private Function ReadXmlGL2(ByVal xml As XmlDocument) As ResultDataTableDataTable

        Logger.Info("ReadXml_Start Pram[" & (xml IsNot Nothing) & "]")

        '返却用DataSet
        Dim returnTable As ResultDataTableDataTable = New ResultDataTableDataTable

        '各NodeList
        Dim xmlDetailList As XmlNodeList
        Dim xmlSearchResultList As XmlNodeList
        Dim xmlVhcInStockInfoList As XmlNodeList

        'Detailタグ情報取得
        xmlDetailList = xml.GetElementsByTagName(XmlTagNameDetail)

        For Each xmlDetailElement As XmlElement In xmlDetailList

            'SearchResulタグ情報取得
            xmlSearchResultList = xmlDetailElement.GetElementsByTagName(XmlTagNameSearchResult)

            For Each xmlSearchResultElement As XmlElement In xmlSearchResultList

                'VhcInStockInfoタグ情報取得
                xmlVhcInStockInfoList = xmlSearchResultElement.GetElementsByTagName(XmlTagNameVhcInStockInfo)

                If xmlVhcInStockInfoList.Count > 0 Then
                    For Each xmlVhcInStockElement As XmlElement In xmlVhcInStockInfoList

                        ' 値を取得しDataSetへ設定
                        Dim resultDataRow As ResultDataTableRow = returnTable.NewResultDataTableRow

                        resultDataRow.VhcNameCode = GetXmlValue(xmlVhcInStockElement.GetElementsByTagName(XmlTagNameModelCode))

                        'グレードは変換
                        resultDataRow.Model = GetXmlValue(xmlVhcInStockElement.GetElementsByTagName(XmlTagNameGradeCode))
                        If resultDataRow.Model.IndexOf("/") > 1 Then
                            resultDataRow.Model = resultDataRow.Model.Substring(0, resultDataRow.Model.IndexOf("/"))

                        End If

                        resultDataRow.Suffix = GetXmlValue(xmlVhcInStockElement.GetElementsByTagName(XmlTagNameSuffix))
                        resultDataRow.Color = GetXmlValue(xmlVhcInStockElement.GetElementsByTagName(XmlTagNameBodyColorCode))
                        resultDataRow.OrderNumber1st = GetXmlValue(xmlVhcInStockElement.GetElementsByTagName(XmlTagNameOrderNumber1st))
                        resultDataRow.OrderNumber2nd = GetXmlValue(xmlVhcInStockElement.GetElementsByTagName(XmlTagNameOrderNumber2nd))
                        resultDataRow.OrderNumber3rd = GetXmlValue(xmlVhcInStockElement.GetElementsByTagName(XmlTagNameOrderNumber3rd))
                        resultDataRow.StockNumber1st = GetXmlValue(xmlVhcInStockElement.GetElementsByTagName(XmlTagNameStockNumber1st))
                        resultDataRow.StockNumber2nd = GetXmlValue(xmlVhcInStockElement.GetElementsByTagName(XmlTagNameStockNumber2nd))
                        resultDataRow.StockNumber3rd = GetXmlValue(xmlVhcInStockElement.GetElementsByTagName(XmlTagNameStockNumber3rd))

                        returnTable.AddResultDataTableRow(resultDataRow)

                    Next
                End If
            Next
        Next

        Logger.Info("ReadXml_End Ret[" & (returnTable IsNot Nothing) & "]")
        Return returnTable

    End Function
    ' $02 end GL2版対応

    ''' <summary>
    ''' XMLタグより値を取得する。
    ''' </summary>
    ''' <param name="xmlList">取得対象XMLNodeList</param>
    ''' <returns>取得した値</returns>
    ''' <remarks></remarks>
    Public Function GetXmlValue(ByVal xmlList As XmlNodeList) As String

        Logger.Info("GetXmlValue_Start Pram[" & (xmlList IsNot Nothing) & "]")
        Dim xmlValue As String = String.Empty

        If Not xmlList Is Nothing Then
            If Not xmlList(0) Is Nothing AndAlso Not xmlList(0).FirstChild Is Nothing Then
                xmlValue = xmlList(0).FirstChild.Value
            End If
        End If

        Logger.Info("GetXmlValue_End Ret[" & xmlValue & "]")
        Return xmlValue

    End Function
#End Region

#Region "グレード検索条件取得"

    ''' <summary>
    ''' グレード検索条件取得処理
    ''' </summary>
    ''' <param name="carName">車名</param>
    ''' <returns>グレードデータテーブル</returns>
    ''' <remarks></remarks>
    Public Function GetGradeConditionList(ByVal carName As String) As GradeConditionDataTableDataTable

        Logger.Info("GetGradeConditionList_Start Pram[" & carName & "]")

        ' 引数チェック
        If String.IsNullOrEmpty(carName) Then
            Throw New ApplicationException(MessageIdCarNameError)
        End If

        ' 検索処理
        Dim returnTable As GradeConditionDataTableDataTable = Nothing
        Using adapter As New SC3070101SearchConditionTableAdapter

            ' グレード検索条件取得
            returnTable = adapter.GetGradeList(carName)

        End Using

        Logger.Info("GetGradeConditionList_End Pram[" & (returnTable IsNot Nothing) & "]")
        Return returnTable
    End Function

#End Region

#Region "サフィックス検索条件取得"

    ''' <summary>
    ''' サフィックス検索条件取得処理
    ''' </summary>
    ''' <param name="carName">車名</param>
    ''' <param name="grade">グレード</param>
    ''' <returns>サフィックスデータテーブル</returns>
    ''' <remarks></remarks>
    Public Function GetSuffixConditionList(ByVal carName As String, ByVal grade As String) As SuffixConditionDataTableDataTable

        Logger.Info("GetSuffixConditionList_Start Pram[" & carName & "," & grade & "]")

        ' 引数チェック
        If String.IsNullOrEmpty(carName) Then
            Throw New ApplicationException(MessageIdCarNameError)
        End If

        ' 検索処理
        Dim returnTable As SuffixConditionDataTableDataTable = Nothing
        Using adapter As New SC3070101SearchConditionTableAdapter

            ' サフィックス検索条件取得
            returnTable = adapter.GetSuffixList(carName, grade)

        End Using

        Logger.Info("GetSuffixConditionList_End Pram[" & (returnTable IsNot Nothing) & "]")
        Return returnTable
    End Function

#End Region

#Region "外装色検索条件取得"

    ''' <summary>
    ''' 外装色検索条件取得処理
    ''' </summary>
    ''' <param name="carName">車名</param>
    ''' <param name="grade">グレード</param>
    ''' <param name="suffix">サフィックス</param>
    ''' <returns>外装色データテーブル</returns>
    ''' <remarks></remarks>
    Public Function GetExteriorColorConditionList(ByVal carName As String, ByVal grade As String, _
                                                  ByVal suffix As String, ByVal colorCode As String) As ExteriorConditionDataTableDataTable

        Logger.Info("GetExteriorColorConditionList_Start Pram[" & carName & "," & grade & "," & suffix & "," & colorCode & "]")

        ' 引数チェック
        If String.IsNullOrEmpty(carName) Then
            Throw New ApplicationException(MessageIdCarNameError)
        End If

        ' 検索処理
        Dim returnTable As ExteriorConditionDataTableDataTable = Nothing
        Using adapter As New SC3070101SearchConditionTableAdapter

            ' 外装色検索条件取得
            returnTable = adapter.GetBodyColorList(carName, grade, suffix, colorCode)

        End Using

        Logger.Info("GetExteriorColorConditionList_End Pram[" & (returnTable IsNot Nothing) & "]")
        Return returnTable
    End Function
#End Region

End Class
