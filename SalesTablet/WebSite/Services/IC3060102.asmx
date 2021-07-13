<%@ WebService Language="VB" Class="Toyota.eCRB.Assessment.Assessment.WebService.IC3060102" %>

Option Strict On

Imports System.Web.Services

Imports System.Xml
Imports System.Globalization
Imports System.Reflection.MethodBase
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.Assessment.Assessment.BizLogic
Imports Toyota.eCRB.Assessment.Assessment.DataAccess
Imports System.Data


Namespace Toyota.eCRB.Assessment.Assessment.WebService

    ' この Web サービスを、スクリプトから ASP.NET AJAX を使用して呼び出せるようにするには、次の行のコメントを解除します。
    '<System.Web.Script.Services.ScriptService()> _
    <WebService(Namespace:="http://tempuri.org/")> _
    <WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
    Public Class IC3060102
        Inherits System.Web.Services.WebService
    
#Region "定数"
    
        ''' <summary>
        ''' メッセージID
        ''' </summary>
        ''' <remarks>メッセージ識別コード(IC3060102) 査定依頼取得</remarks>
        Private Const MESSAGEID_CONST As String = "IC3060102"

        ''' <summary>
        ''' メッセージ(成功)
        ''' </summary>
        ''' <remarks>応答結果メッセージ(Success.)</remarks>
        Private Const MESSAGE_SUCCESS_CONST As String = "Success"
    
        ''' <summary>
        ''' メッセージ(失敗)
        ''' </summary>
        ''' <remarks>応答結果メッセージ(Failure.)</remarks>
        Private Const MESSAGE_FAILURE_CONST As String = "Failure"
        
        ''' <summary>XMLエラーメッセージ</summary>
        Private Const ErrorMessageXml As String = " input xml error "

#Region "データテーブル名"
        
        ''' <summary>
        ''' データテーブル名 査定依頼件数情報
        ''' </summary>
        Private Const TABLE_NAME_ASSESSMENT_REQ_COUNT As String = _
            "IC3060102AssessmentReqCount"

        ''' <summary>
        ''' データテーブル名 対応中査定依頼情報
        ''' </summary>
        Private Const TABLE_NAME_INPROGRESS_ASSESSMENT_REQ_INFO As String = _
            "IC3060102InProgressAssessmentReqInfo"

        ''' <summary>
        ''' データテーブル名 未対応査定依頼一覧情報
        ''' </summary>
        Private Const TABLE_NAME_ASSESSMENT_REQ_LIST As String = _
            "IC3060102AssessmentReqListInfo"

        ''' <summary>
        ''' データテーブル名 査定依頼状態情報
        ''' </summary>
        Private Const TABLE_NAME_ASSESSMENT_REQ_STATE_INFO As String = _
            "IC3060102AssessmentReqStateInfo"
        
#End Region

        ''' <summary>XMLタグの開始文字</summary>
        Private Const XML_TAG_START As String = "<"
        ''' <summary>XMLタグの終了文字</summary>
        Private Const XML_TAG_END As String = ">"
        
        ''' <summary>スラッシュ("/")</summary>
        Private Const CHAR_SLASH As String = "/"

        ''' <summary>受信XML宣言文字列</summary>
        Private Const XML_REQUEST_DECLARATION As String = _
            "<?xml version=""1.0"" encoding=""UTF-16LE""?>"
        
        ' XML宣言
        ''' <summary>XMLバージョン</summary>
        Private Const XML_VERSION As String = "1.0"
        ''' <summary>XMLエンコード</summary>
        Private Const XML_ENCODING As String = "UTF-16LE"
        
#Region "XMLタグ名"

        ''' <summary>要求XMLルートタグ名称</summary>
        Private Const XML_GETASSESSMENTREQUEST As String = "GetAssessmentRequest"
        
        ''' <summary>Headerタグ名称</summary>
        Private Const XML_HEAD As String = "Head"
                
        ''' <summary>MessageIDタグ名称</summary>
        Private Const XML_MESSAGEID As String = "MessageId"
        
        ''' <summary>受信日付タグ名称</summary>
        Private Const XML_RECEPTIONDATE As String = "ReceptionDate"

        ''' <summary>送信日付タグ名称</summary>
        Private Const XML_TRANSMISSIONDATE As String = "TransmissionDate"

        ''' <summary>Detailタグ名称</summary>
        Private Const XML_DETAIL As String = "Detail"

        ''' <summary>Commonタグ名称</summary>
        Private Const XML_COMMON As String = "Common"

        ''' <summary>ResultIdタグ名称</summary>
        Private Const XML_RESULTID As String = "ResultId"

        ''' <summary>Messageタグ名称</summary>
        Private Const XML_MESSAGE As String = "Message"

        ''' <summary>実行モードタグ名称</summary>
        Private Const XML_MODE As String = "Mode"

        ''' <summary>販売店コードタグ名称</summary>
        Private Const XML_DEALERCODE As String = "DealerCode"

        ''' <summary>店舗コードタグ名称</summary>
        Private Const XML_STORECODE As String = "StoreCode"

        ''' <summary>端末IDタグ名称</summary>
        Private Const XML_CLIENTID As String = "ClientId"

        ''' <summary>取得データ開始位置タグ名称</summary>
        Private Const XML_DATAFROM As String = "DataFrom"

        ''' <summary>取得データ終了位置タグ名称</summary>
        Private Const XML_DATATO As String = "DataTo"

        ''' <summary>返却XMLルートタグ名称</summary>
        Private Const XML_RESPONSE As String = "Response"

        ''' <summary>査定依頼タグ名称</summary>
        Private Const XML_ASSESSMENTREQUEST As String = "AssessmentRequest"


        ''' <summary>対応中査定依頼情報タグ名称</summary>
        Private Const XML_ASSESSMENTREQCOUNT As String = _
            "AssessmentReqCount"
        
        ''' <summary>対応中査定依頼情報タグ名称</summary>
        Private Const XML_INPROGRESSASSESSMENTREQINFO As String = _
            "InProgressAssessmentReqInfo"
        
        ''' <summary>対応中査定依頼一覧タグ名称</summary>
        Private Const XML_ASSESSMENTREQLISTINFO As String = _
            "AssessmentReqListInfo"
        
        ''' <summary>対応中査定依頼一覧タグ名称</summary>
        Private Const XML_INPROGRESSASSESSMENTREQLISTINFO As String = _
            "InProgressAssessmentReqListInfo"
        
        ''' <summary>査定依頼状態タグ名称</summary>
        Private Const XML_ASSESSMENTREQSTATEINFO As String = _
            "AssessmentReqStateInfo"

        ''' <summary>依頼IDタグ名称</summary>
        Private Const XML_REQUESTID As String = "RequestId"
        ''' <summary>依頼種別IDタグ名称</summary>
        Private Const XML_REQUESTCLASSID As String = "RequestClassId"
        ''' <summary>ステータスタグ名称</summary>
        Private Const XML_STATUS As String = "Status"

        ''' <summary>査定依頼日時タグ名称</summary>
        Private Const XML_ASSESSMENTTIME As String = "AssessmentTime"
        ''' <summary>査定依頼日時表示用タグ名称</summary>
        Private Const XML_ASSESSMENTTIMEDISP As String = "AssessmentTimeDisplay"
        ''' <summary>Vinタグ名称</summary>
        Private Const XML_VIN As String = "Vin"
        ''' <summary>車両登録Noタグ名称</summary>
        Private Const XML_REGISTRATIONNO As String = "RegistrationNo"
        ''' <summary>依頼アカウントタグ名称</summary>
        Private Const XML_REQUESTACCOUNT As String = "RequestAccount"
        ''' <summary>依頼端末IDタグ名称</summary>
        Private Const XML_REQUESTCLIENTID As String = "RequestClientId"
        ''' <summary>依頼セールススタッフ名タグ名称</summary>
        Private Const XML_REQUESTSCNAME As String = "RequestSCName"
        ''' <summary>敬称付きお客様名タグ名称</summary>
        Private Const XML_CUSTOMERNAMETITLE As String = "CustomerNameTitle"
        ''' <summary>携帯電話番号タグ名称</summary>
        Private Const XML_CUSTOMERMOBILE As String = "CustomerMobile"
        ''' <summary>メーカーコードタグ名称</summary>
        Private Const XML_MAKERCD As String = "MakerCd"
        ''' <summary>メーカー名タグ名称</summary>
        Private Const XML_MAKERNAME As String = "MakerName"
        ''' <summary>シリーズコードタグ名称</summary>
        Private Const XML_SERIESCD As String = "SeriesCd"
        ''' <summary>シリーズ名称タグ名称</summary>
        Private Const XML_SERIESNAME As String = "SeriesName"
        ''' <summary>商談テーブルNoタグ名称</summary>
        Private Const XML_SALESTABLENO As String = "SalesTableNo"
        ''' <summary>お客様名IDタグ名称</summary>
        Private Const XML_CUSTOMID As String = "CustomId"
        ''' <summary>顧客分類タグ名称</summary>
        Private Const XML_CUSTOMERCLASS As String = "CustomerClass"
        ''' <summary>顧客種別タグ名称</summary>
        Private Const XML_CUSTOMERKIND As String = "CustomerKind"

        ''' <summary>依頼種別タグ名称</summary>
        Private Const XML_REQUESTCLASS As String = "RequestClass"
        ''' <summary>対応端末IDタグ名称</summary>
        Private Const XML_TOCLIENTID As String = "ToClientId"
        ''' <summary>対応スタッフ名タグ名称</summary>
        Private Const XML_TOACCOUNTNAME As String = "ToAccountName"
        ''' <summary>スタッフコード(送信元)タグ名称</summary>
        Private Const XML_FROMACCOUNT As String = "FromAccount"
        ''' <summary>端末ID(送信元)タグ名称</summary>
        Private Const XML_FROMCLIENTID As String = "FromClientId"
        ''' <summary>お客様名タグ名称</summary>
        Private Const XML_CUSTOMERNAME As String = "CustomerName"

#End Region

#End Region

#Region "列挙体"

        ''' <summary>
        ''' メッセージID用項目No
        ''' </summary>
        ''' <remarks></remarks>
        Private Enum ItemNo As Integer
            ''' <summary>送信日付</summary>
            TransmissionDate = 1
            ''' <summary>実行モード</summary>
            Mode = 2
        End Enum

#End Region

#Region "メンバ変数"
    
        ''' <summary>
        ''' 終了コード
        ''' </summary>
        ''' <remarks></remarks>
        Private ResultId As Integer

        ''' <summary>
        ''' 実行モード
        ''' </summary>
        ''' <remarks></remarks>
        Private _mode As Integer

        ''' <summary>
        ''' 販売店コード
        ''' </summary>
        ''' <remarks></remarks>
        Private _dealerCode As String
        
        ''' <summary>
        ''' 店舗コード
        ''' </summary>
        ''' <remarks></remarks>
        Private _storeCode As String
              
        ''' <summary>
        ''' 端末ID
        ''' </summary>
        ''' <remarks></remarks>
        Private _clientID As String
        
        ''' <summary>
        ''' 依頼ID
        ''' </summary>
        ''' <remarks></remarks>
        Private _requestId As String
              
        ''' <summary>
        ''' 取得データ開始位置
        ''' </summary>
        ''' <remarks></remarks>
        Private _dataFrom As String
        
        ''' <summary>
        ''' 取得データ終了位置
        ''' </summary>
        ''' <remarks></remarks>
        Private _dataTo As String

        ''' <summary>
        ''' 送信日時（Request）
        ''' </summary>
        ''' <remarks>メッセージ送信日時(yyyyMMddHHmmss)</remarks>
        Private _transmissionDate As String

#End Region
    
#Region "プロパティ"

        ''' <summary>
        ''' 実行モードプロパティ
        ''' </summary>
        ''' <value>実行モード</value>
        ''' <returns>実行モード</returns>
        ''' <remarks></remarks>
        Private Property Mode As Integer
            Get
                Return _mode
            End Get
            Set(value As Integer)
                _mode = value
            End Set
        End Property
        
        ''' <summary>
        ''' 販売店コードプロパティ
        ''' </summary>
        ''' <value>販売店コード</value>
        ''' <returns>販売店コード</returns>
        ''' <remarks></remarks>
        Private Property DealerCode As String
            Get
                Return _dealerCode
            End Get
            Set(value As String)
                _dealerCode = value
            End Set
        End Property

        ''' <summary>
        ''' 店舗コードプロパティ
        ''' </summary>
        ''' <value>店舗コード</value>
        ''' <returns>店舗コード</returns>
        ''' <remarks></remarks>
        Private Property StoreCode As String
            Get
                Return _storeCode
            End Get
            Set(value As String)
                _storeCode = value
            End Set
        End Property
    
        ''' <summary>
        ''' 端末IDプロパティ
        ''' </summary>
        ''' <value>端末ID</value>
        ''' <returns>端末ID</returns>
        ''' <remarks></remarks>
        Private Property ClientId As String
            Get
                Return _clientID
            End Get
            Set(value As String)
                _clientID = value
            End Set
        End Property
    
        ''' <summary>
        ''' 依頼IDプロパティ
        ''' </summary>
        ''' <value>依頼ID</value>
        ''' <returns>依頼ID</returns>
        ''' <remarks></remarks>
        Private Property RequestId As String
            Get
                Return _requestId
            End Get
            Set(value As String)
                _requestId = value
            End Set
        End Property
    
        ''' <summary>
        ''' 取得データ開始位置プロパティ
        ''' </summary>
        ''' <value>取得データ開始位置</value>
        ''' <returns>取得データ開始位置</returns>
        ''' <remarks></remarks>
        Private Property DataFrom As String
            Get
                Return _dataFrom
            End Get
            Set(value As String)
                _dataFrom = value
            End Set
        End Property
    
        ''' <summary>
        ''' 取得データ終了位置プロパティ
        ''' </summary>
        ''' <value>取得データ終了位置</value>
        ''' <returns>取得データ終了位置</returns>
        ''' <remarks></remarks>
        Private Property DataTo As String
            Get
                Return _dataTo
            End Get
            Set(value As String)
                _dataTo = value
            End Set
        End Property

        ''' <summary>
        ''' 送信日時プロパティ
        ''' </summary>
        ''' <value>送信日時</value>
        ''' <returns>送信日時</returns>
        ''' <remarks></remarks>
        Private Property TransmissionDate As String
            Get
                Return _transmissionDate
            End Get
            Set(value As String)
                _transmissionDate = value
            End Set
        End Property
                
#End Region
       
#Region "コンストラクタ"
    
        ''' <summary>
        ''' コンストラクタ
        ''' </summary>
        ''' <remarks>初期化処理</remarks>
        Public Sub New()
            Me.ResultId = 0
        End Sub
    
#End Region
    
#Region "査定依頼取得Webサービス"
        
        ''' <summary>
        ''' 査定依頼取得Webサービス
        ''' </summary>
        ''' <remarks></remarks>
        <WebMethod()> _
        Public Sub GetAssessmentRequest()
            '開始ログ出力
            Logger.Info(getLogMethod(GetCurrentMethod.Name, True), True)
            ' 受信XMLをログ出力
            Logger.Info("Request Form:" & _
                        HttpUtility.UrlDecode(Me.Context.Request.Form.ToString()), True)

            Dim returnXml As New XmlDocument

            '返却XML宣言の追加
            Dim xmlDeclaration As XmlDeclaration = _
                returnXml.CreateXmlDeclaration(XML_VERSION, XML_ENCODING, Nothing)
            returnXml.AppendChild(xmlDeclaration)
                
            'Responseルートタグの追加
            Dim responseElement As XmlElement = returnXml.CreateElement(XML_RESPONSE)
            returnXml.AppendChild(responseElement)

            'Headタグの追加
            Dim headElement As XmlElement = returnXml.CreateElement(XML_HEAD)
            responseElement.AppendChild(headElement)

            'MessageIdタグの追加
            Dim messageIdElement As XmlElement = returnXml.CreateElement(XML_MESSAGEID)
            messageIdElement.InnerText = MESSAGEID_CONST
            headElement.AppendChild(messageIdElement)

            '受信日付タグの追加
            Dim receptionDateElement As XmlElement = returnXml.CreateElement(XML_RECEPTIONDATE)
            headElement.AppendChild(receptionDateElement)

            '送信日付タグの追加
            Dim transmissionDateElement As XmlElement = returnXml.CreateElement(XML_TRANSMISSIONDATE)
            transmissionDateElement.InnerText = ""
            headElement.AppendChild(transmissionDateElement)
                
            'Detailタグの追加
            Dim detailElement As XmlElement = returnXml.CreateElement(XML_DETAIL)
            responseElement.AppendChild(detailElement)

            'Commonタグの追加
            Dim commonElement As XmlElement = returnXml.CreateElement(XML_COMMON)
            detailElement.AppendChild(commonElement)

            '終了コードタグの追加
            Dim resultIdElement As XmlElement = returnXml.CreateElement(XML_RESULTID)
            resultIdElement.InnerText = ""
            commonElement.AppendChild(resultIdElement)

            'メッセージタグの追加
            Dim messageElement As XmlElement = returnXml.CreateElement(XML_MESSAGE)
            messageElement.InnerText = MESSAGE_FAILURE_CONST
            commonElement.AppendChild(messageElement)

            'Response情報を設定
            Me.Context.Response.Charset = XML_ENCODING
            Me.Context.Response.ContentEncoding = Encoding.GetEncoding(XML_ENCODING)
            Me.Context.Response.ContentType = "text/xml"
            Me.Context.Response.HeaderEncoding = Encoding.UTF8

            Try
                
                'Inputメッセージ受信日時取得
                Dim resReceptionData As String = _
                    DateTimeFunc.Now.ToString(IC3060102BusinessLogic.FormatDateTime, _
                                              CultureInfo.InvariantCulture)
                '受信日付の設定
                receptionDateElement.InnerText = resReceptionData
                
                'AssessmentRequestタグの追加
                Dim assessmentRequestElement As XmlElement = _
                    returnXml.CreateElement(XML_ASSESSMENTREQUEST)
                detailElement.AppendChild(assessmentRequestElement)

                '受信XMLの取り出し
                Dim xmlData As String = Me.GetRequestXmlString()

                ' 受信XMLをデータ格納用クラスにセット
                Me.SetData(xmlData)
                
                '査定依頼取得処理
                Dim businessClassIC3060102 As New IC3060102BusinessLogic
                
                Dim IC3060102DataSet As IC3060102DataSet = _
                        businessClassIC3060102.GetAssessmentRequest(Me.Mode, _
                                                                    Me.DealerCode, _
                                                                    Me.StoreCode, _
                                                                    Me.ClientId, _
                                                                    Me.RequestId, _
                                                                    Me.DataFrom, _
                                                                    Me.DataTo, _
                                                                    Me.ResultId)
                
                If Me.ResultId.Equals(IC3060102BusinessLogic.ResultCodeSuccess) Then
                    'Responseクラスへの格納処理
                    Me.SetAssessmentRequestXml(returnXml, _
                                               assessmentRequestElement, _
                                               IC3060102DataSet)
                
                    'メッセージを設定
                    messageElement.InnerText = MESSAGE_SUCCESS_CONST
                End If

            Catch ex As Exception
                If Me.ResultId.Equals(IC3060102BusinessLogic.ResultCodeSuccess) Then
                    Me.ResultId = IC3060102BusinessLogic.ResultCodeErrorSystem
                End If
                
                'エラーログ出力
                Logger.Error(IC3060102BusinessLogic.LogResultId & _
                             CStr(Me.ResultId), ex)
                
                
            Finally
                '終了コードを設定
                resultIdElement.InnerText = CType(Me.ResultId, String)

                '送信日時を設定
                transmissionDateElement.InnerText = _
                    DateTimeFunc.Now.ToString(IC3060102BusinessLogic.FormatDateTime,
                                              CultureInfo.InvariantCulture)
                
                ' 送信XMLをログ出力
                Logger.Info("Response XML:" & returnXml.OuterXml, True)
                
                '返却XMLの出力
                Me.Context.Response.Write(returnXml.OuterXml)

                Logger.Info(getLogMethod(GetCurrentMethod.Name, False), True)
            End Try

        End Sub

#End Region
         
#Region "Request XMLの格納処理"

        ''' <summary>
        ''' POSTデータの取得処理
        ''' </summary>
        ''' <returns>XMLデータ文字列</returns>
        ''' <remarks></remarks>
        Private Function GetRequestXmlString() As String
            '開始ログ出力
            Logger.Info(getLogMethod(GetCurrentMethod.Name, True), True)

            ' POSTデータの取得
            Dim formString As String = HttpUtility.UrlDecode(Me.Context.Request.Form.ToString())
            

            Dim utf8Bytes As Byte() = Encoding.UTF8.GetBytes(formString)

            Dim encodeUtf16 As Encoding = Encoding.GetEncoding(XML_ENCODING)

            Dim xmlString As String = encodeUtf16.GetString(utf8Bytes)
         
            Dim searchRootTagStart As New StringBuilder
            searchRootTagStart.Append(XML_TAG_START)
            searchRootTagStart.Append(XML_GETASSESSMENTREQUEST)
            searchRootTagStart.Append(XML_TAG_END)

            Dim rootStartIndex As Integer = _
                xmlString.IndexOf(searchRootTagStart.ToString, _
                                  System.StringComparison.Ordinal)
            If rootStartIndex.Equals(-1) Then
                'ルートタグ(開始)が存在しない場合は、XMLエラー
                Me.ResultId = IC3060102BusinessLogic.ResultCodeErrorXml
                Throw New ArgumentException("", XML_GETASSESSMENTREQUEST & ErrorMessageXml)
            End If
            xmlString = Mid(xmlString, rootStartIndex + 1)

            Dim searchRootTagEnd As New StringBuilder
            searchRootTagEnd.Append(XML_TAG_START)
            searchRootTagEnd.Append(CHAR_SLASH)
            searchRootTagEnd.Append(XML_GETASSESSMENTREQUEST)
            searchRootTagEnd.Append(XML_TAG_END)

            Dim rootEndIndex As Integer = _
                xmlString.IndexOf(searchRootTagEnd.ToString, _
                                  System.StringComparison.Ordinal)
            If rootEndIndex.Equals(-1) Then
                'ルートタグ(終了)が存在しない場合は、XMLエラー
                Me.ResultId = IC3060102BusinessLogic.ResultCodeErrorXml
                Throw New ArgumentException("", XML_GETASSESSMENTREQUEST & ErrorMessageXml)
            End If
            xmlString = Left(xmlString, rootEndIndex + searchRootTagEnd.Length)

            Logger.Info(getReturnParam(xmlString), True)
            Logger.Info(getLogMethod(GetCurrentMethod.Name, False), True)
            Return xmlString

        End Function
        
        ''' <summary>
        ''' XMLタグの情報をデータ格納クラスにセットします。
        ''' </summary>
        ''' <param name="xsData">受信XML</param>
        ''' <remarks></remarks>
        Private Sub SetData(xsData As String)
            '開始ログ出力
            Logger.Info(getLogMethod(GetCurrentMethod.Name, True), True)
            Logger.Info(getLogParam("xsData", xsData, False), True)

            ' XmlDocument生成
            Dim requestXml As New XmlDocument
            
            Try
                ' XML文字列を生成する
                Dim stringXml As New StringBuilder
                stringXml.AppendLine(XML_REQUEST_DECLARATION)
                stringXml.Append(xsData)
                
                ' XML読み込み
                requestXml.LoadXml(stringXml.ToString)

            Catch ex As Exception
                'XML読み込み失敗時は終了コードをセットして処理終了
                Me.ResultId = IC3060102BusinessLogic.ResultCodeErrorXml
                
                'エラーログ出力
                Logger.Error(IC3060102BusinessLogic.LogResultId & _
                             CStr(Me.ResultId) & ErrorMessageXml, ex)
                Throw
            End Try
            
            ' Header情報格納
            Me.InitHead()
            Me.SetHead(requestXml.DocumentElement)
            
            ' AssessmentRequest情報格納
            Me.InitDetail()
            Me.SetDetail(requestXml.DocumentElement)
            
            requestXml = Nothing
        
            '終了ログ出力
            Logger.Info(getLogMethod(GetCurrentMethod.Name, False), True)
        End Sub

#End Region

        ''' <summary>
        ''' 査定依頼取得結果オブジェクトへの格納処理
        ''' </summary>
        ''' <param name="returnXml">返却XMLDocument</param>        
        ''' <param name="assessmentRequestElement">assessmentRequestタグ情報</param>        
        ''' <param name="dsAssessmentInfo">取得結果データセット</param>
        ''' <remarks></remarks>
        Private Sub SetAssessmentRequestXml( _
            ByVal returnXml As XmlDocument, _
            ByVal assessmentRequestElement As XmlElement, _
            ByVal dsAssessmentInfo As IC3060102DataSet)
            '開始ログ出力
            Logger.Info(getLogMethod(GetCurrentMethod.Name, True), True)
            Logger.Info(getLogParam("returnXml", returnXml.OuterXml, False), True)
            Logger.Info(getLogParam("assessmentRequestElement", _
                                    assessmentRequestElement.OuterXml, True), True)
            Dim sb As New StringBuilder
            Dim i As Integer = 0
            For Each dt As DataTable In dsAssessmentInfo.Tables
                If 0 < i Then
                    sb.Append(",")
                End If
                sb.Append(dt.TableName)
                sb.Append(" Count:")
                sb.Append(CStr(dt.Rows.Count))

                i = i + 1
            Next dt
            Logger.Info(getLogParam("dsAssessmentInfo", sb.ToString, True), True)

            
            '未対応査定依頼件数
            If 0 < dsAssessmentInfo.Tables(TABLE_NAME_ASSESSMENT_REQ_COUNT).Rows.Count Then
                    
                'AssessmentReqCountタグの追加
                Dim assessmentReqCount As XmlElement = returnXml.CreateElement(XML_ASSESSMENTREQCOUNT)
                assessmentReqCount.InnerText = _
                    dsAssessmentInfo.Tables(TABLE_NAME_ASSESSMENT_REQ_COUNT).Rows(0). _
                    Item("ASSESSMENTREQCOUNT").ToString()
                assessmentRequestElement.AppendChild(assessmentReqCount)
            End If
            
            '対応中査定依頼情報
            If 0 < dsAssessmentInfo.Tables(TABLE_NAME_INPROGRESS_ASSESSMENT_REQ_INFO).Rows.Count Then

                Me.SetInProgressAssessmentReqInfo( _
                    returnXml, _
                    assessmentRequestElement, _
                    DirectCast(dsAssessmentInfo.Tables(TABLE_NAME_INPROGRESS_ASSESSMENT_REQ_INFO),  _
                               IC3060102DataSet.IC3060102InProgressAssessmentReqInfoDataTable))
            End If
            
            '未対応査定依頼一覧情報
            If 0 < dsAssessmentInfo.Tables(TABLE_NAME_ASSESSMENT_REQ_LIST).Rows.Count Then
                
                Me.SetAssessmentReqListInfo( _
                    returnXml, _
                    assessmentRequestElement, _
                    DirectCast(dsAssessmentInfo.Tables(TABLE_NAME_ASSESSMENT_REQ_LIST),  _
                                IC3060102DataSet.IC3060102AssessmentReqListInfoDataTable))
            End If
            
            '対応中査定依頼一覧情報
            If dsAssessmentInfo.Tables.Contains(IC3060102BusinessLogic.TableNameInProgressAssessmentReqList) Then

                Me.SetInProgressAssessmentReqListInfo( _
                    returnXml, _
                    assessmentRequestElement, _
                    DirectCast(dsAssessmentInfo.Tables(IC3060102BusinessLogic.TableNameInProgressAssessmentReqList),  _
                               IC3060102DataSet.IC3060102AssessmentReqListInfoDataTable))
            End If

            '査定依頼状態情報
            If 0 < dsAssessmentInfo.Tables(TABLE_NAME_ASSESSMENT_REQ_STATE_INFO).Rows.Count Then

                Me.SetAssessmentReqStateInfo( _
                    returnXml, _
                    assessmentRequestElement, _
                    DirectCast(dsAssessmentInfo.Tables(TABLE_NAME_ASSESSMENT_REQ_STATE_INFO),  _
                               IC3060102DataSet.IC3060102AssessmentReqStateInfoDataTable))
            End If
            
            '終了ログ出力
            Logger.Info(getLogMethod(GetCurrentMethod.Name, False), True)
        End Sub
                
        ''' <summary>
        ''' 対応中査定依頼情報格納処理
        ''' </summary>
        ''' <param name="returnXml">返却XMLDocument</param>        
        ''' <param name="assessmentRequestElement">assessmentRequestタグ情報</param>        
        ''' <param name="inProgressAssessmentReqInfo">対応中査定依頼情報データテーブル</param>
        ''' <remarks></remarks>
        Private Sub SetInProgressAssessmentReqInfo( _
            ByVal returnXml As XmlDocument, _
            ByVal assessmentRequestElement As XmlElement, _
            ByVal inProgressAssessmentReqInfo As IC3060102DataSet.IC3060102InProgressAssessmentReqInfoDataTable)
            '開始ログ出力
            Logger.Info(getLogMethod(GetCurrentMethod.Name, True), True)
            Logger.Info(getLogParam("returnXml", returnXml.OuterXml, False), True)
            Logger.Info(getLogParam("assessmentRequestElement", _
                                    assessmentRequestElement.OuterXml, True), True)
            Logger.Info(getLogParam("inProgressAssessmentReqInfo Count", _
                                    CStr(inProgressAssessmentReqInfo.Count), True), True)

            Dim i As Integer = 0
            For Each dr As IC3060102DataSet.IC3060102InProgressAssessmentReqInfoRow In inProgressAssessmentReqInfo.Rows

                Dim inProgressAssessmentReqInfoElement As XmlElement = _
                    returnXml.CreateElement(XML_INPROGRESSASSESSMENTREQINFO)
                assessmentRequestElement.AppendChild(inProgressAssessmentReqInfoElement)
                
                '依頼ID
                Dim requestIdElement As XmlElement = returnXml.CreateElement(XML_REQUESTID)
                requestIdElement.InnerText = CType(dr.NOTICEREQID, String)
                inProgressAssessmentReqInfoElement.AppendChild(requestIdElement)

            Next
        
            '終了ログ出力
            Logger.Info(getLogMethod(GetCurrentMethod.Name, False), True)
        End Sub
        
        ''' <summary>
        ''' 未対応査定依頼一覧情報格納処理
        ''' </summary>
        ''' <param name="returnXml">返却XMLDocument</param>        
        ''' <param name="assessmentRequestElement">assessmentRequestタグ情報</param>        
        ''' <param name="assessmentReqListInfo">未対応査定依頼一覧情報DataTable</param>
        ''' <remarks></remarks>
        Private Sub SetAssessmentReqListInfo( _
            ByVal returnXml As XmlDocument, _
            ByVal assessmentRequestElement As XmlElement, _
            ByVal assessmentReqListInfo As IC3060102DataSet.IC3060102AssessmentReqListInfoDataTable)
            '開始ログ出力
            Logger.Info(getLogMethod(GetCurrentMethod.Name, True), True)
            Logger.Info(getLogParam("returnXml", returnXml.OuterXml, False), True)
            Logger.Info(getLogParam("assessmentRequestElement", _
                                    assessmentRequestElement.OuterXml, True), True)
            Logger.Info(getLogParam("assessmentReqListInfo Count", _
                                    CStr(assessmentReqListInfo.Count), True), True)
            
            For Each dr As IC3060102DataSet.IC3060102AssessmentReqListInfoRow In assessmentReqListInfo
                
                'AssessmentReqListInfoタグの追加
                Dim assessmentReqListInfoElement As XmlElement = returnXml.CreateElement(XML_ASSESSMENTREQLISTINFO)
                assessmentRequestElement.AppendChild(assessmentReqListInfoElement)
                
                'ステータス
                Dim statusElement As XmlElement = returnXml.CreateElement(XML_STATUS)
                statusElement.InnerText = dr.STATUS
                assessmentReqListInfoElement.AppendChild(statusElement)
                
                '依頼ID
                Dim requestIdElement As XmlElement = returnXml.CreateElement(XML_REQUESTID)
                requestIdElement.InnerText = CType(dr.NOTICEREQID, String)
                assessmentReqListInfoElement.AppendChild(requestIdElement)
                
                '査定依頼日時
                Dim assessmentTimeElement As XmlElement = returnXml.CreateElement(XML_ASSESSMENTTIME)
                assessmentTimeElement.InnerText = _
                    dr.SENDDATE.ToString(IC3060102BusinessLogic.FormatDateTime, CultureInfo.InvariantCulture)
                assessmentReqListInfoElement.AppendChild(assessmentTimeElement)
                
                '査定依頼日時表示用
                Dim assessmentTimeDispElement As XmlElement = returnXml.CreateElement(XML_ASSESSMENTTIMEDISP)
                assessmentTimeDispElement.InnerText = dr.SENDDATEDISP
                assessmentReqListInfoElement.AppendChild(assessmentTimeDispElement)
                
                'VIN
                Dim vinElement As XmlElement = returnXml.CreateElement(XML_VIN)
                vinElement.InnerText = dr.VIN
                assessmentReqListInfoElement.AppendChild(vinElement)
                
                '車両登録No
                Dim registrationNoElement As XmlElement = returnXml.CreateElement(XML_REGISTRATIONNO)
                registrationNoElement.InnerText = dr.VCLREGNO
                assessmentReqListInfoElement.AppendChild(registrationNoElement)
                
                '依頼アカウント
                Dim requestAccountElement As XmlElement = returnXml.CreateElement(XML_REQUESTACCOUNT)
                requestAccountElement.InnerText = dr.FROMACCOUNT
                assessmentReqListInfoElement.AppendChild(requestAccountElement)
                
                '依頼端末ID
                Dim requestClientIdElement As XmlElement = returnXml.CreateElement(XML_REQUESTCLIENTID)
                requestClientIdElement.InnerText = "" '現在はiPhoneで空を設定するため、空文字を設定
                assessmentReqListInfoElement.AppendChild(requestClientIdElement)
                
                '依頼セールススタッフ名
                Dim requestScNameElement As XmlElement = returnXml.CreateElement(XML_REQUESTSCNAME)
                requestScNameElement.InnerText = dr.FROMACCOUNTNAME
                assessmentReqListInfoElement.AppendChild(requestScNameElement)
                
                '敬称付き顧客氏名
                Dim customerNameTitleElement As XmlElement = returnXml.CreateElement(XML_CUSTOMERNAMETITLE)
                customerNameTitleElement.InnerText = dr.CUSTOMNAME
                assessmentReqListInfoElement.AppendChild(customerNameTitleElement)
                
                '携帯電話番号
                Dim customerMobileElement As XmlElement = returnXml.CreateElement(XML_CUSTOMERMOBILE)
                customerMobileElement.InnerText = dr.MOBILE
                assessmentReqListInfoElement.AppendChild(customerMobileElement)
                
                'メーカーコード
                Dim MakerCDElement As XmlElement = returnXml.CreateElement(XML_MAKERCD)
                MakerCDElement.InnerText = dr.MAKERCD
                assessmentReqListInfoElement.AppendChild(MakerCDElement)
                
                'メーカー名
                Dim makerNameElement As XmlElement = returnXml.CreateElement(XML_MAKERNAME)
                makerNameElement.InnerText = dr.MAKERNAME
                assessmentReqListInfoElement.AppendChild(makerNameElement)
                
                'シリーズコード
                Dim seriesCDElement As XmlElement = returnXml.CreateElement(XML_SERIESCD)
                seriesCDElement.InnerText = dr.SERIESCD
                assessmentReqListInfoElement.AppendChild(seriesCDElement)
                
                'シリーズ名称
                Dim seriesNmElement As XmlElement = returnXml.CreateElement(XML_SERIESNAME)
                seriesNmElement.InnerText = dr.SERIESNM
                assessmentReqListInfoElement.AppendChild(seriesNmElement)
                
                '商談テーブルNo.
                Dim salesTableNoElement As XmlElement = returnXml.CreateElement(XML_SALESTABLENO)
                salesTableNoElement.InnerText = CType(dr.SALESTABLENO, String)
                assessmentReqListInfoElement.AppendChild(salesTableNoElement)
                
                '依頼種別ID
                Dim requestClassIdElement As XmlElement = returnXml.CreateElement(XML_REQUESTCLASSID)
                requestClassIdElement.InnerText = CType(dr.REQCLASSID, String)
                assessmentReqListInfoElement.AppendChild(requestClassIdElement)
                
                'お客様名ID
                Dim customIdElement As XmlElement = returnXml.CreateElement(XML_CUSTOMID)
                customIdElement.InnerText = dr.CRCUSTID
                assessmentReqListInfoElement.AppendChild(customIdElement)
                
                '顧客分類
                Dim customerClassElement As XmlElement = returnXml.CreateElement(XML_CUSTOMERCLASS)
                customerClassElement.InnerText = dr.CUSTOMERCLASS
                assessmentReqListInfoElement.AppendChild(customerClassElement)
                
                '顧客種別
                Dim cstKindElement As XmlElement = returnXml.CreateElement(XML_CUSTOMERKIND)
                cstKindElement.InnerText = dr.CSTKIND
                assessmentReqListInfoElement.AppendChild(cstKindElement)

            Next dr
                
            '終了ログ出力
            Logger.Info(getLogMethod(GetCurrentMethod.Name, False), True)
        End Sub
        
        ''' <summary>
        ''' 対応中査定依頼一覧情報格納処理
        ''' </summary>
        ''' <param name="returnXml">返却XMLDocument</param>        
        ''' <param name="assessmentRequestElement">assessmentRequestタグ情報</param>        
        ''' <param name="inProgressAssessmentReqListInfo">対応中査定依頼一覧情報DataTable</param>
        ''' <remarks></remarks>
        Private Sub SetInProgressAssessmentReqListInfo( _
            ByVal returnXml As XmlDocument, _
            ByVal assessmentRequestElement As XmlElement, _
            ByVal inProgressAssessmentReqListInfo As IC3060102DataSet.IC3060102AssessmentReqListInfoDataTable)
            '開始ログ出力
            Logger.Info(getLogMethod(GetCurrentMethod.Name, True), True)
            Logger.Info(getLogParam("returnXml", returnXml.OuterXml, False), True)
            Logger.Info(getLogParam("assessmentRequestElement", _
                                    assessmentRequestElement.OuterXml, True), True)
            Logger.Info(getLogParam("inProgressAssessmentReqListInfo Count", _
                                    CStr(inProgressAssessmentReqListInfo.Count), True), True)
            
            '型付けデータテーブルの各項目のプロパティ参照を可能にするため、
            'テーブル名を型付けデータテーブル(査定依頼一覧用データテーブル)と同名に変更する
            Dim assessmentReqListInfo As IC3060102DataSet.IC3060102AssessmentReqListInfoDataTable = Nothing
            assessmentReqListInfo = DirectCast(inProgressAssessmentReqListInfo.Copy(),  _
                            IC3060102DataSet.IC3060102AssessmentReqListInfoDataTable)
            assessmentReqListInfo.TableName = TABLE_NAME_ASSESSMENT_REQ_LIST

            For Each dr As IC3060102DataSet.IC3060102AssessmentReqListInfoRow In assessmentReqListInfo.Rows
                
                'InProgressAssessmentReqListInfoタグの追加
                Dim inProgressssessmentReqListInfoElement As XmlElement = _
                    returnXml.CreateElement(XML_INPROGRESSASSESSMENTREQLISTINFO)
                assessmentRequestElement.AppendChild(inProgressssessmentReqListInfoElement)
                
                'ステータス
                Dim statusElement As XmlElement = returnXml.CreateElement(XML_STATUS)
                statusElement.InnerText = dr.STATUS
                inProgressssessmentReqListInfoElement.AppendChild(statusElement)
                
                '依頼ID
                Dim requestIdElement As XmlElement = returnXml.CreateElement(XML_REQUESTID)
                requestIdElement.InnerText = CType(dr.NOTICEREQID, String)
                inProgressssessmentReqListInfoElement.AppendChild(requestIdElement)
                
                '査定依頼日時
                Dim assessmentTimeElement As XmlElement = returnXml.CreateElement(XML_ASSESSMENTTIME)
                assessmentTimeElement.InnerText = _
                    dr.SENDDATE.ToString(IC3060102BusinessLogic.FormatDateTime, CultureInfo.InvariantCulture)
                inProgressssessmentReqListInfoElement.AppendChild(assessmentTimeElement)
                
                '査定依頼日時表示用
                Dim assessmentTimeDispElement As XmlElement = returnXml.CreateElement(XML_ASSESSMENTTIMEDISP)
                assessmentTimeDispElement.InnerText = dr.SENDDATEDISP
                inProgressssessmentReqListInfoElement.AppendChild(assessmentTimeDispElement)
                
                'VIN
                Dim vinElement As XmlElement = returnXml.CreateElement(XML_VIN)
                vinElement.InnerText = dr.VIN
                inProgressssessmentReqListInfoElement.AppendChild(vinElement)
                
                '車両登録No
                Dim registrationNoElement As XmlElement = returnXml.CreateElement(XML_REGISTRATIONNO)
                registrationNoElement.InnerText = dr.VCLREGNO
                inProgressssessmentReqListInfoElement.AppendChild(registrationNoElement)
                
                '依頼アカウント
                Dim requestAccountElement As XmlElement = returnXml.CreateElement(XML_REQUESTACCOUNT)
                requestAccountElement.InnerText = dr.FROMACCOUNT
                inProgressssessmentReqListInfoElement.AppendChild(requestAccountElement)
                
                '依頼端末ID
                Dim requestClientIdElement As XmlElement = returnXml.CreateElement(XML_REQUESTCLIENTID)
                requestClientIdElement.InnerText = "" '現在はiPhoneで空を設定するため、空文字を設定
                inProgressssessmentReqListInfoElement.AppendChild(requestClientIdElement)

                '依頼セールススタッフ名
                Dim requestScNameElement As XmlElement = returnXml.CreateElement(XML_REQUESTSCNAME)
                requestScNameElement.InnerText = dr.FROMACCOUNTNAME
                inProgressssessmentReqListInfoElement.AppendChild(requestScNameElement)
                
                '敬称付き顧客氏名
                Dim customerNameTitleElement As XmlElement = returnXml.CreateElement(XML_CUSTOMERNAMETITLE)
                customerNameTitleElement.InnerText = dr.CUSTOMNAME
                inProgressssessmentReqListInfoElement.AppendChild(customerNameTitleElement)
                
                '携帯電話番号
                Dim customerMobileElement As XmlElement = returnXml.CreateElement(XML_CUSTOMERMOBILE)
                customerMobileElement.InnerText = dr.MOBILE
                inProgressssessmentReqListInfoElement.AppendChild(customerMobileElement)
                
                'メーカーコード
                Dim MakerCDElement As XmlElement = returnXml.CreateElement(XML_MAKERCD)
                MakerCDElement.InnerText = dr.MAKERCD
                inProgressssessmentReqListInfoElement.AppendChild(MakerCDElement)
                
                'メーカー名
                Dim makerNameElement As XmlElement = returnXml.CreateElement(XML_MAKERNAME)
                makerNameElement.InnerText = dr.MAKERNAME
                inProgressssessmentReqListInfoElement.AppendChild(makerNameElement)
                
                'シリーズコード
                Dim seriesCDElement As XmlElement = returnXml.CreateElement(XML_SERIESCD)
                seriesCDElement.InnerText = dr.SERIESCD
                inProgressssessmentReqListInfoElement.AppendChild(seriesCDElement)
                
                'シリーズ名称
                Dim seriesNmElement As XmlElement = returnXml.CreateElement(XML_SERIESNAME)
                seriesNmElement.InnerText = dr.SERIESNM
                inProgressssessmentReqListInfoElement.AppendChild(seriesNmElement)
                
                '商談テーブルNo.
                Dim salesTableNoElement As XmlElement = returnXml.CreateElement(XML_SALESTABLENO)
                salesTableNoElement.InnerText = CType(dr.SALESTABLENO, String)
                inProgressssessmentReqListInfoElement.AppendChild(salesTableNoElement)
                
                '依頼種別ID
                Dim requestClassIdElement As XmlElement = returnXml.CreateElement(XML_REQUESTCLASSID)
                requestClassIdElement.InnerText = CType(dr.REQCLASSID, String)
                inProgressssessmentReqListInfoElement.AppendChild(requestClassIdElement)
                
                'お客様名ID
                Dim customIdElement As XmlElement = returnXml.CreateElement(XML_CUSTOMID)
                customIdElement.InnerText = dr.CRCUSTID
                inProgressssessmentReqListInfoElement.AppendChild(customIdElement)
                
                '顧客分類
                Dim customerClassElement As XmlElement = returnXml.CreateElement(XML_CUSTOMERCLASS)
                customerClassElement.InnerText = dr.CUSTOMERCLASS
                inProgressssessmentReqListInfoElement.AppendChild(customerClassElement)
                
                '顧客種別
                Dim cstKindElement As XmlElement = returnXml.CreateElement(XML_CUSTOMERKIND)
                cstKindElement.InnerText = dr.CSTKIND
                inProgressssessmentReqListInfoElement.AppendChild(cstKindElement)

            Next dr
            
            '終了ログ出力
            Logger.Info(getLogMethod(GetCurrentMethod.Name, False), True)
        End Sub
        
        ''' <summary>
        ''' 査定依頼状態情報格納処理
        ''' </summary>
        ''' <param name="returnXml">返却XMLDocument</param>        
        ''' <param name="assessmentRequestElement">assessmentRequestタグ情報</param>        
        ''' <param name="assessmentReqStateInfo">査定依頼状態情報データテーブル</param>
        ''' <remarks></remarks>
        Private Sub SetAssessmentReqStateInfo( _
            ByVal returnXml As XmlDocument, _
            ByVal assessmentRequestElement As XmlElement, _
            ByVal assessmentReqStateInfo As IC3060102DataSet.IC3060102AssessmentReqStateInfoDataTable)
            '開始ログ出力
            Logger.Info(getLogMethod(GetCurrentMethod.Name, True), True)
            Logger.Info(getLogParam("returnXml", returnXml.OuterXml, False), True)
            Logger.Info(getLogParam("assessmentRequestElement", _
                                    assessmentRequestElement.OuterXml, True), True)
            Logger.Info(getLogParam("assessmentReqStateInfo Count", _
                                    CStr(assessmentReqStateInfo.Count), True), True)
            
            Dim i As Integer = 0
            For Each dr As IC3060102DataSet.IC3060102AssessmentReqStateInfoRow In assessmentReqStateInfo.Rows
                
                'AssessmentReqStateInfoタグの追加
                Dim assessmentReqStateInfoElement As XmlElement = _
                    returnXml.CreateElement(XML_ASSESSMENTREQSTATEINFO)
                assessmentRequestElement.AppendChild(assessmentReqStateInfoElement)
                
                'ステータス
                Dim statusElement As XmlElement = returnXml.CreateElement(XML_STATUS)
                statusElement.InnerText = dr.STATUS
                assessmentReqStateInfoElement.AppendChild(statusElement)
                '依頼種別
                Dim requestClassElement As XmlElement = returnXml.CreateElement(XML_REQUESTCLASS)
                requestClassElement.InnerText = dr.NOTICEREQCTG
                assessmentReqStateInfoElement.AppendChild(requestClassElement)
                '対応端末ID 
                Dim toClientElement As XmlElement = returnXml.CreateElement(XML_TOCLIENTID)
                toClientElement.InnerText = dr.TOCLIENTID
                assessmentReqStateInfoElement.AppendChild(toClientElement)
                '対応スタッフ名
                Dim toAccountNameElement As XmlElement = returnXml.CreateElement(XML_TOACCOUNTNAME)
                toAccountNameElement.InnerText = CType(dr.TOACCOUNTNAME, String)
                assessmentReqStateInfoElement.AppendChild(toAccountNameElement)
                'お客様名
                Dim customerName As XmlElement = returnXml.CreateElement(XML_CUSTOMERNAME)
                customerName.InnerText = dr.CUSTOMNAME
                assessmentReqStateInfoElement.AppendChild(customerName)
            Next
        
            '終了ログ出力
            Logger.Info(getLogMethod(GetCurrentMethod.Name, False), True)
        End Sub
  
#Region "初期化"

        ''' <summary>
        ''' Headerタグ情報の初期化
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub InitHead()
            '開始ログ出力
            Logger.Info(getLogMethod(GetCurrentMethod.Name, True), True)

            Me.TransmissionDate = ""

            '終了ログ出力
            Logger.Info(getLogMethod(GetCurrentMethod.Name, False), True)
        End Sub

        ''' <summary>
        ''' Detailタグ情報の初期化
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub InitDetail()
            '開始ログ出力
            Logger.Info(getLogMethod(GetCurrentMethod.Name, True), True)

            Me.Mode = -1
            Me.DealerCode = ""
            Me.StoreCode = ""
            Me.ClientId = ""
            Me.RequestId = ""
            Me.DataFrom = ""
            Me.DataTo = ""

            '終了ログ出力
            Logger.Info(getLogMethod(GetCurrentMethod.Name, False), True)
        End Sub

#End Region
        
#Region "要求情報セット"
        
        ''' <summary>
        ''' Headerタグ情報のプロパティーセット
        ''' </summary>
        ''' <param name="rootElement">ルートElement情報</param>        
        ''' <remarks>
        ''' XMLオブジェクトより、プロパティを設定します。
        ''' </remarks>
        Private Sub SetHead(ByVal rootElement As XmlElement)
            '開始ログ出力
            Logger.Info(getLogMethod(GetCurrentMethod.Name, True), True)
            Logger.Info(getLogParam("rootElement", rootElement.OuterXml, False), True)

            ' XMLノードリスト取得(<Head><TransmissionDate>タグ)
            Dim nodeTransmissionDate As XmlNode = _
                rootElement.SelectSingleNode(CHAR_SLASH & XML_GETASSESSMENTREQUEST & _
                                             CHAR_SLASH & XML_HEAD & _
                                             CHAR_SLASH & XML_TRANSMISSIONDATE)
            If nodeTransmissionDate Is Nothing Then
                'TransmissionDateタグが存在しない場合、XMLエラー
                Me.ResultId = IC3060102BusinessLogic.ResultCodeErrorXml
                Throw New ArgumentException("", XML_TRANSMISSIONDATE & ErrorMessageXml)
            End If
            ' TransmissionDateタグの値を取得する
            Me.TransmissionDate = nodeTransmissionDate.InnerText

            If String.IsNullOrEmpty(Me.TransmissionDate) Then
                '送信日付が空の場合は、必須エラー
                Me.ResultId = IC3060102BusinessLogic.ResultCodeErrorMust + ItemNo.TransmissionDate
                Throw New ArgumentException("", XML_TRANSMISSIONDATE & IC3060102BusinessLogic.ErrorMessageParameter)
            End If
            
            '日付型チェック
            Dim transmissionDateValue As Date = _
                ConvertDateTime(Me.TransmissionDate, _
                                IC3060102BusinessLogic.FormatDateTime, _
                                IC3060102BusinessLogic.ResultCodeErrorType + ItemNo.TransmissionDate)

            '終了ログ出力
            Logger.Info(getLogMethod(GetCurrentMethod.Name, False), True)
        End Sub

        ''' <summary>
        ''' AssessmentRequestタグ情報のプロパティーセット
        ''' </summary>
        ''' <param name="rootElement">ルートElement情報</param>        
        ''' <remarks>
        ''' XMLオブジェクトより、プロパティを設定します。
        ''' </remarks>
        Private Sub SetDetail(ByVal rootElement As XmlElement)
            '開始ログ出力
            Logger.Info(getLogMethod(GetCurrentMethod.Name, True), True)
            Logger.Info(getLogParam("rootElement", rootElement.OuterXml, False), True)

            ' Detail XMLノード取得（<Detail>タグ）
            Dim nodeDetail As XmlNode = _
                rootElement.SelectSingleNode(CHAR_SLASH & XML_GETASSESSMENTREQUEST & _
                                             CHAR_SLASH & XML_DETAIL)
            If nodeDetail Is Nothing Then
                'Detailタグが存在しない場合、XMLエラー
                Me.ResultId = IC3060102BusinessLogic.ResultCodeErrorXml
                Throw New ArgumentException("", XML_DETAIL & ErrorMessageXml)
            End If
            
            ' Mode XMLノード取得（<Detail><Mode>タグ）
            Dim nodeMode As XmlNode = nodeDetail.SelectSingleNode(XML_MODE)
            If nodeMode Is Nothing Then
                'Detailタグが存在しない場合、XMLエラー
                Me.ResultId = IC3060102BusinessLogic.ResultCodeErrorXml
                Throw New ArgumentException("", XML_MODE & ErrorMessageXml)
            End If

            If String.IsNullOrEmpty(nodeMode.InnerText) Then
                '実行モードが空の場合は、必須エラー
                Me.ResultId = IC3060102BusinessLogic.ResultCodeErrorMust + ItemNo.Mode
                Throw New ArgumentException("", XML_MODE & IC3060102BusinessLogic.ErrorMessageParameter)
            End If

            If Not Integer.TryParse(nodeMode.InnerText, Me.Mode) Then
                '実行モードが数値型でない場合、型エラー
                Me.ResultId = IC3060102BusinessLogic.ResultCodeErrorType + ItemNo.Mode
                Throw New ArgumentException("", XML_MODE & IC3060102BusinessLogic.ErrorMessageParameter)
            End If

            If Not IC3060102BusinessLogic.ModeAssessmentReqCount.Equals(Me.Mode) AndAlso _
               Not IC3060102BusinessLogic.ModeAssessmentReqListFirst.Equals(Me.Mode) AndAlso _
               Not IC3060102BusinessLogic.ModeAssessmentReqListNext.Equals(Me.Mode) AndAlso _
               Not IC3060102BusinessLogic.ModeAssessmentReqState.Equals(Me.Mode) Then
                '実行モードが0、1、2、3でない場合、値エラー
                Me.ResultId = IC3060102BusinessLogic.ResultCodeErrorValue + ItemNo.Mode
                Throw New ArgumentException("", XML_MODE & IC3060102BusinessLogic.ErrorMessageParameter)
            End If

            ' 販売店コード XMLノード取得（<Detail><DealerCode>タグ）
            Dim nodeDealerCode As XmlNode = nodeDetail.SelectSingleNode(XML_DEALERCODE)
            If nodeDealerCode Is Nothing Then
                '販売店コードノードが空の場合は、XMLエラー
                Me.ResultId = IC3060102BusinessLogic.ResultCodeErrorXml
                Throw New ArgumentException("", XML_DEALERCODE & ErrorMessageXml)
            End If
            Me.DealerCode = nodeDealerCode.InnerText

            ' 店舗コード XMLノード取得（<Detail><StoreCode>タグ）
            Dim nodeStoreCode As XmlNode = nodeDetail.SelectSingleNode(XML_STORECODE)
            If nodeStoreCode Is Nothing Then
                '店舗コードノードが空の場合は、XMLエラー
                Me.ResultId = IC3060102BusinessLogic.ResultCodeErrorXml
                Throw New ArgumentException("", XML_STORECODE & ErrorMessageXml)
            End If
            Me.StoreCode = nodeStoreCode.InnerText

            ' 端末ID XMLノード取得（<Detail><ClientID>タグ）
            Dim nodeClientId As XmlNode = nodeDetail.SelectSingleNode(XML_CLIENTID)
            If nodeClientId Is Nothing Then
                '端末IDノードが空の場合は、XMLエラー
                Me.ResultId = IC3060102BusinessLogic.ResultCodeErrorXml
                Throw New ArgumentException("", XML_CLIENTID & ErrorMessageXml)
            End If
            Me.ClientId = nodeClientId.InnerText

            ' 依頼ID XMLノード取得（<Detail><RequestId>タグ）
            If IC3060102BusinessLogic.ModeAssessmentReqState.Equals(Mode) Then
                '実行モードが3の場合、取得する
                Dim nodeRequestId As XmlNode = nodeDetail.SelectSingleNode(XML_REQUESTID)
                If nodeRequestId Is Nothing Then
                    '依頼IDノードが空の場合は、XMLエラー
                    Me.ResultId = IC3060102BusinessLogic.ResultCodeErrorXml
                    Throw New ArgumentException("", XML_REQUESTID & ErrorMessageXml)
                End If
                Me.RequestId = nodeRequestId.InnerText
            End If

            If IC3060102BusinessLogic.ModeAssessmentReqListFirst.Equals(Mode) OrElse _
                IC3060102BusinessLogic.ModeAssessmentReqListNext.Equals(Mode) Then
                '実行モードが1、2の場合、取得する

                ' 取得データ開始位置 XMLノード取得（<Detail><DataFrom>タグ）
                Dim nodeDataFrom As XmlNode = nodeDetail.SelectSingleNode(XML_DATAFROM)
                If nodeDataFrom Is Nothing Then
                    '取得データ開始位置ノードが空の場合は、XMLエラー
                    Me.ResultId = IC3060102BusinessLogic.ResultCodeErrorXml
                    Throw New ArgumentException("", XML_DATAFROM & ErrorMessageXml)
                End If
                Me.DataFrom = nodeDataFrom.InnerText

                ' 取得データ終了位置 XMLノード取得（<Detail><DataTo>タグ）
                Dim nodeDataTo As XmlNode = nodeDetail.SelectSingleNode(XML_DATATO)
                If nodeDataTo Is Nothing Then
                    '取得データ終了位置ノードが空の場合は、XMLエラー
                    Me.ResultId = IC3060102BusinessLogic.ResultCodeErrorXml
                    Throw New ArgumentException("", XML_DATATO & ErrorMessageXml)
                End If
                Me.DataTo = nodeDataTo.InnerText
            End If

            '終了ログ出力
            Logger.Info(getLogMethod(GetCurrentMethod.Name, False), True)
        End Sub

#End Region
 
        
        ''' <summary>
        ''' 日付の書式に合わせて変換を行う。
        ''' </summary>
        ''' <param name="valueString">XMLの取り出し値（Check String）</param>
        ''' <param name="FormatDate">日付/時刻のフォーマット書式</param>
        ''' <param name="ErrNumber">エラーコード</param>
        ''' <returns>XMLから取り出した値</returns>
        ''' <remarks></remarks>
        Private Function ConvertDateTime(ByVal valueString As String, _
                                         ByVal formatDate As String, _
                                         ByVal errNumber As Short) As Date
            '開始ログ出力
            Logger.Info(getLogMethod(GetCurrentMethod.Name, True), True)
            Logger.Info(getLogParam("valueString", valueString, False), True)
            Logger.Info(getLogParam("formatDate", formatDate, True), True)
            Logger.Info(getLogParam("errNumber", CStr(errNumber), True), True)
            
            Try
                ' 指定されたフォーマット書式の日付に変換

                '終了ログ出力
                Logger.Info(getReturnParam(CStr(DateTime.ParseExact(valueString, formatDate, Nothing))), True)
                Logger.Info(getLogMethod(GetCurrentMethod.Name, False), True)
                Return DateTime.ParseExact(valueString, formatDate, Nothing)
            Catch ex As Exception
                Me.ResultId = errNumber
                
                'エラーログ出力
                Logger.Error(IC3060102BusinessLogic.LogResultId & _
                             CStr(Me.ResultId), ex)
                Throw
            End Try

        End Function

#Region "ログデータ加工処理"
        ''' <summary>
        ''' ログデータ（メソッド）
        ''' </summary>
        ''' <param name="methodName">メソッド名</param>
        ''' <param name="startEndFlag">True：「method start」を表示、False：「method end」を表示</param>
        ''' <returns>加工した文字列</returns>
        ''' <remarks></remarks>
        Private Function getLogMethod(ByVal methodName As String,
                                      ByVal startEndFlag As Boolean) As String
            Dim sb As New StringBuilder
            With sb
                .Append("[")
                .Append(methodName)
                .Append("]")
                If startEndFlag Then
                    .Append(" method start")
                Else
                    .Append(" method end")
                End If
            End With
            Return sb.ToString
        End Function

        ''' <summary>
        ''' ログデータ（引数）
        ''' </summary>
        ''' <param name="paramName">引数名</param>
        ''' <param name="paramData">引数値</param>
        ''' <param name="kanmaFlag">True：引数名の前に「,」を表示、False：特になし</param>
        ''' <returns>加工した文字列</returns>
        ''' <remarks></remarks>
        Private Function getLogParam(ByVal paramName As String,
                                     ByVal paramData As String,
                                     ByVal kanmaFlag As Boolean) As String
            Dim sb As New StringBuilder
            With sb
                If kanmaFlag Then
                    .Append(",")
                End If
                .Append(paramName)
                .Append("=")
                .Append(paramData)
            End With
            Return sb.ToString
        End Function

        ''' <summary>
        ''' ログデータ（戻り値）
        ''' </summary>
        ''' <param name="paramData">引数値</param>
        ''' <returns>加工した文字列</returns>
        ''' <remarks></remarks>
        Private Function getReturnParam(ByVal paramData As String) As String
            Dim sb As New StringBuilder
            With sb
                .Append("Return=")
                .Append(paramData)
            End With
            Return sb.ToString
        End Function
#End Region

    End Class
    
End Namespace

