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
'Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.BizLogic
'Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.DataAccess
Imports Toyota.eCRB.iCROP.BizLogic.SC3250101.SC3250101WebServiceClassBusinessLogic_CreateXml
Imports Toyota.eCRB.iCROP.DataAccess.SC3250101.SC3250101DataSet

Imports System.Runtime.Serialization

Public Class SC3250101WebServiceClassBusinessLogic
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
    ''' サービスアイテム取得
    ''' </summary>
    ''' <param name="inXmlClass">XML作成用クラス</param>
    ''' <returns>WebService処理結果。Nothingの場合はXML解析エラー発生する原因となる</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Public Function CallGetServiceItemsWebService(ByVal inXmlClass As ServiceItemsXmlDocumentClass) As String
        Dim myInfo As New GetServiceItems_Info

        ''開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} " _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name))

        'DMSコード転換
        inXmlClass = Me.Change2DMSXMLOfServiceItems(inXmlClass)

        'XML戻り値用DataTable
        Dim dtWebServiceResult As New ServiceItemsResultDataTable

        'XML戻り値用DataRow
        Dim rowWebServiceResult As ServiceItemsResultRow = DirectCast(dtWebServiceResult.NewRow, ServiceItemsResultRow)

        Try
            'WebServiceURLの取得
            Dim envSettingRow As String = String.Empty
            Using biz As New SC3250101BusinessLogic
                envSettingRow = biz.GetDlrSystemSettingValueBySettingName(GetServiceItems_Info.WebServiceURL)
            End Using

            'URLの取得確認
            If envSettingRow Is Nothing Then
                'URL取得失敗

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} DlrEnvSetting == NOTHING OUT:resultXmlValue.ResultCode = {2}" _
                        , Me.GetType.ToString _
                        , MethodBase.GetCurrentMethod.Name _
                        , rowWebServiceResult("ResultId")))

                Return String.Empty

            End If

            'WebServiceのURLを作成
            'Dim createUrl As String = String.Concat(envSettingRow, "/", GetServiceItems_Info.WebServiceIDValue, "/", GetServiceItems_Info.WebServiceMethod)
            '2014/03/07　WebServiceのURLにIDとメソッドが追加されているが、メソッドのみなので変更する
            'Dim createUrl As String = envSettingRow
            'If Not envSettingRow.EndsWith(GetServiceItems_Info.WebServiceIDValue + "/" + GetServiceItems_Info.WebServiceMethod) Then
            '    '「URL/ID/メソッド」になっていない場合
            '    If envSettingRow.EndsWith(GetServiceItems_Info.WebServiceIDValue) Then
            '        'IDのみ　ついていた　→　メソッドのみ追加
            '        createUrl += "/" + GetServiceItems_Info.WebServiceMethod
            '    Else
            '        'IDもメソッドもついていない　→　IDとメソッドを追加
            '        createUrl += "/" + GetServiceItems_Info.WebServiceIDValue + "/" + GetServiceItems_Info.WebServiceMethod
            '    End If
            'End If

            Dim createUrl As String = envSettingRow
            If Not envSettingRow.EndsWith(GetServiceItems_Info.WebServiceMethod) Then
                'Logger.Info("★サービスアイテム：WebServiceのURLにメソッド追加：" & GetServiceItems_Info.WebServiceMethod)
                createUrl += "/" + GetServiceItems_Info.WebServiceMethod
            End If
            Logger.Info(String.Format("ServiceItemsWebServiceURL:[{0}]", createUrl))

            'WebService送信用XML作成処理
            Dim sendXml As String = CreateXmlOfServiceItems(inXmlClass, GetServiceItems_Info.WebServiceMethod)

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

            '2014/06/03 戻りXMLを文字列で解析　START　↓↓↓

            '取得XMLの解析
            Dim xml As Xml.XmlDocument = New Xml.XmlDocument
            Dim Res As GetServiceItems_Info.Response = New GetServiceItems_Info.Response

            xml.LoadXml(resultString)
            Dim nodes As Xml.XmlNodeList = xml.SelectNodes(String.Format("/{0}/{1}/{2}", GetServiceItems_Info.Response.NodeResponse _
                                                                         , GetServiceItems_Info.Response.NodeDetail _
                                                                         , GetServiceItems_Info.Response.NodeCommon))
            Dim nd As Xml.XmlNode = nodes(0)
            Dim ReturnCode As String = nd.SelectSingleNode(GetServiceItems_Info.Response.TagResultId).InnerText

            'dtWebServiceResult = GetXMLDataOfServiceItems(resultString, rowWebServiceResult)

            '2014/06/03 戻りXMLを文字列で解析　END　　↑↑↑

            '終了ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:resultXmlValue.ResultCode = {2}" _
                        , Me.GetType.ToString _
                        , MethodBase.GetCurrentMethod.Name _
                        , ReturnCode))

            Return ReturnCode

        Catch ex As System.Net.WebException
            'WebServiceエラー
            Logger.Error(String.Format(CultureInfo.CurrentCulture _
                         , "{0}.{1} OUT:ErrWebService = {2}" _
                         , Me.GetType.ToString _
                         , MethodBase.GetCurrentMethod.Name _
                         , ex.ToString))

            If ex.Status = WebExceptionStatus.Timeout Then

                Dim drTemp As ServiceItemsResultRow = DirectCast(dtWebServiceResult.NewRow, ServiceItemsResultRow)
                drTemp("ResultId") = ReturnCode.ErrTimeout

                dtWebServiceResult.Rows.Add(drTemp)

            Else

                Dim drTemp As ServiceItemsResultRow = DirectCast(dtWebServiceResult.NewRow, ServiceItemsResultRow)
                drTemp("ResultId") = ReturnCode.ErrOther

                dtWebServiceResult.Rows.Add(drTemp)

            End If

            Return String.Empty

        Catch ex2 As System.Exception

            Logger.Error(String.Format(CultureInfo.CurrentCulture _
                         , "{0}.{1} OUT:ErrWebService = {2}" _
                         , Me.GetType.ToString _
                         , MethodBase.GetCurrentMethod.Name _
                         , ex2.ToString))

            Dim drTemp As ServiceItemsResultRow = DirectCast(dtWebServiceResult.NewRow, ServiceItemsResultRow)
            drTemp("ResultId") = ReturnCode.ErrOther

            dtWebServiceResult.Rows.Add(drTemp)

            Return String.Empty

        Finally
            ''終了ログの出力
            'Logger.Info(String.Format(CultureInfo.CurrentCulture _
            '            , "{0}.{1} OUT:RETURNCODE = {2}" _
            '            , Me.GetType.ToString _
            '            , MethodBase.GetCurrentMethod.Name _
            '            , inXmlClass))
        End Try

    End Function

    ''' <summary>
    ''' マイレージ取得
    ''' </summary>
    ''' <param name="inXmlClass">XML作成用クラス</param>
    ''' <returns>WebService処理結果。Nothingの場合はXML解析エラー発生する原因となる</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Public Function CallGetMileageWebService(ByVal inXmlClass As Request_MileageXmlDocumentClass) As MileageDataTable
        Dim myInfo As New GetMileage_Info

        '開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} " _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name))

        'DMSコード転換
        inXmlClass = Me.Change2DMSXMLOfMileage(inXmlClass)

        'XML戻り値用DataTable
        Dim dtWebServiceResult As New MileageDataTable

        'XML戻り値用DataRow
        Dim rowWebServiceResult As MileageRow = DirectCast(dtWebServiceResult.NewRow, MileageRow)


        'XML戻り値用
        Dim ret As New GetMileage_Info.Response

        Try
            'WebServiceURLの取得
            Dim envSettingRow As String = String.Empty
            '2014/05/23 「ServiceCommonClassBusinessLogic」の使用廃止　START　↓↓↓
            'Using biz As New ServiceCommonClassBusinessLogic
            Using biz As New SC3250101BusinessLogic
                envSettingRow = biz.GetDlrSystemSettingValueBySettingName(GetMileage_Info.WebServiceURL)
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
            Dim createUrl As String = envSettingRow
            If Not envSettingRow.EndsWith(GetMileage_Info.WebServiceMethod) Then
                'Logger.Info("★マイレージ：WebServiceのURLにメソッド追加：" & GetMileage_Info.WebServiceMethod)
                'createUrl = String.Concat(envSettingRow, "/", GetMileage_Info.WebServiceMethod)
                createUrl += "/" + GetMileage_Info.WebServiceMethod
            End If
            Logger.Info(String.Format("MileageWebServiceURL:[{0}]", createUrl))

            'WebService送信用XML作成処理
            Dim sendXml As String = CreateXmlOfMileage(inXmlClass, GetMileage_Info.WebServiceMethod)

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
            rowWebServiceResult("Mileage") = GetMileageFromXMLData(resultString, rowWebServiceResult)

            dtWebServiceResult.AddMileageRow(rowWebServiceResult)
            '終了ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:resultXmlValue.ResultCode = {2}" _
                        , Me.GetType.ToString _
                        , MethodBase.GetCurrentMethod.Name _
                        , rowWebServiceResult("ResultCode")))

            Return dtWebServiceResult

        Catch ex As System.Net.WebException
            'WebServiceエラー

            Logger.Error(String.Format(CultureInfo.CurrentCulture _
                         , "{0}.{1} OUT:ErrWebService = {2}" _
                         , Me.GetType.ToString _
                         , MethodBase.GetCurrentMethod.Name _
                         , ex.ToString))

            If ex.Status = WebExceptionStatus.Timeout Then

                Dim drTemp As MileageRow = DirectCast(dtWebServiceResult.NewRow, MileageRow)
                drTemp("ResultCode") = ReturnCode.ErrTimeout

                dtWebServiceResult.Rows.Add(drTemp)

            Else

                Dim drTemp As MileageRow = DirectCast(dtWebServiceResult.NewRow, MileageRow)
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

            Dim drTemp As MileageRow = DirectCast(dtWebServiceResult.NewRow, MileageRow)
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
            Using biz As New SC3250101BusinessLogic
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
    ''' サービスアイテム：XML作成(メイン)
    ''' </summary>
    ''' <param name="inXmlClass">XML作成用クラス</param>
    ''' <returns>XMLString</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Function CreateXmlOfServiceItems(ByVal inXmlClass As ServiceItemsXmlDocumentClass, ByVal WebServiceID As String) As String

        ''開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} " _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name))


        'XMLのHeadTagの作成処理
        inXmlClass = CreateHeadTagOfServiceItems(inXmlClass, WebServiceID)

        'テキストWriter
        Using writer As New StringWriter(CultureInfo.InvariantCulture)

            'XMLシリアライザー型の設定
            Dim serializer As New XmlSerializer(GetType(ServiceItemsXmlDocumentClass))

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
    ''' マイレージ：XML作成(メイン)
    ''' </summary>
    ''' <param name="inXmlClass">XML作成用クラス</param>
    ''' <returns>XMLString</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Function CreateXmlOfMileage(ByVal inXmlClass As Request_MileageXmlDocumentClass, ByVal WebServiceID As String) As String

        ''開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} " _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name))


        'XMLのHeadTagの作成処理
        inXmlClass = CreateHeadTagOfMileage(inXmlClass, WebServiceID)

        'テキストWriter
        Using writer As New StringWriter(CultureInfo.InvariantCulture)

            'XMLシリアライザー型の設定
            Dim serializer As New XmlSerializer(GetType(Request_MileageXmlDocumentClass))

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
    ''' サービスアイテム：XML作成(HeadTag)
    ''' </summary>
    ''' <param name="inXmlClass">XML作成用クラス</param>
    ''' <returns>XML作成用クラス</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Function CreateHeadTagOfServiceItems(ByVal inXmlClass As ServiceItemsXmlDocumentClass, ByVal WebServiceID As String) As ServiceItemsXmlDocumentClass

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

    ''' <summary>
    ''' マイレージ：XML作成(HeadTag)
    ''' </summary>
    ''' <param name="inXmlClass">XML作成用クラス</param>
    ''' <returns>XML作成用クラス</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Function CreateHeadTagOfMileage(ByVal inXmlClass As Request_MileageXmlDocumentClass, ByVal WebServiceID As String) As Request_MileageXmlDocumentClass

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

#Region "未使用メソッド"
    ' ''' <summary>
    ' ''' WebServiceの戻りXMLを解析し値を取得
    ' ''' </summary>
    ' ''' <param name="resultString">送信XML文字列</param>
    ' ''' <param name="rowWebServiceResult">XML戻り値用DataRow</param>
    ' ''' <returns>WebService結果</returns>
    ' ''' <remarks></remarks>
    'Private Function GetXMLDataOfServiceItems(ByVal resultString As String, _
    '                            ByVal rowWebServiceResult As ServiceItemsResultRow) As ServiceItemsResultDataTable

    '    ''開始ログの出力
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                , "{0}.{1} RESULTXML:{2}" _
    '                , Me.GetType.ToString _
    '                , MethodBase.GetCurrentMethod.Name _
    '                , resultString))

    '    '返却DataTable
    '    Dim retDataTable As New ServiceItemsResultDataTable

    '    Try

    '        ''XmlDocument
    '        'Dim resultXmlDocument As New XmlDocument

    '        ''返却された文字列をXML化
    '        'resultXmlDocument.LoadXml(resultString)

    '        ''XmlElementを取得
    '        'Dim resultXmlElement As XmlElement = resultXmlDocument.DocumentElement

    '        ''XmlElementの確認
    '        'If resultXmlElement Is Nothing Then
    '        '    '取得失敗

    '        '    'エラーログの出力
    '        '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '        '                , "{0}.{1} OUT:Err XmlDocument.DocumentElement = Nothing" _
    '        '                , Me.GetType.ToString _
    '        '                , MethodBase.GetCurrentMethod.Name))

    '        '    Return Nothing

    '        'End If

    '        ''Resultノード取得開始
    '        ''子ノードリストの取得
    '        'Dim resultXmlNodeList As XmlNodeList = resultXmlElement.GetElementsByTagName(NodeResponse)

    '        ''子ノードリストの確認
    '        'If resultXmlNodeList Is Nothing OrElse resultXmlNodeList.Count = 0 Then
    '        '    '取得失敗

    '        '    'エラーログの出力
    '        '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '        '                , "{0}.{1} OUT:Err Update_Reserve = Nothing" _
    '        '                , Me.GetType.ToString _
    '        '                , MethodBase.GetCurrentMethod.Name))

    '        '    Return Nothing

    '        'End If


    '        ''子ノードの取得
    '        'Dim resultXmlNode As XmlNode = resultXmlNodeList.Item(0)

    '        ''解析したXMLから設定されている値の取得
    '        'rowWebServiceResult = GetXmlResultNodeValue(rowWebServiceResult, resultXmlNode)

    '        'If rowWebServiceResult Is Nothing Then
    '        '    Return Nothing
    '        'End If
    '        ''Resultノード取得終了



    '        ''終了ログの出力
    '        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '        '            , "{0}.{1} OUT:RETURNSTRING = {2}" _
    '        '            , Me.GetType.ToString _
    '        '            , MethodBase.GetCurrentMethod.Name _
    '        '            , resultString))

    '        Return retDataTable


    '    Catch ex As XmlException

    '        'エラーログの出力
    '        Logger.Error(String.Format(CultureInfo.CurrentCulture _
    '                     , "{0}.{1} OUT:Err XmlException = {2}" _
    '                     , Me.GetType.ToString _
    '                     , MethodBase.GetCurrentMethod.Name _
    '                     , ex.ToString))

    '        Return Nothing

    '    End Try

    'End Function
#End Region

    ''' <summary>
    ''' マイレージ：WebServiceの戻りXMLを解析する
    ''' </summary>
    ''' <param name="resultString"></param>
    ''' <param name="rowWebServiceResult"></param>
    ''' <returns>走行距離</returns>
    ''' <remarks></remarks>
    Private Function GetMileageFromXMLData(ByVal resultString As String, _
                                ByVal rowWebServiceResult As MileageRow) As String

        ''開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} RESULTXML:{2}" _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name _
                    , resultString))

        '返却DataTable
        Dim retDataTable As New MileageDataTable

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

                Return Nothing

            End If

            'Resultノード取得開始
            '子ノードリストの取得
            'Dim resultXmlNodeList As XmlNodeList = resultXmlElement.GetElementsByTagName(GetMileage_Info.Response.NodeMileage_Result)
            Dim resultXmlNode As XmlNode = resultXmlElement.SelectSingleNode("//Mileage_Result/Output_Mileage")
            ''子ノードリストの確認
            'If resultXmlNodeList Is Nothing OrElse resultXmlNodeList.Count = 0 Then
            '    '取得失敗

            '    'エラーログの出力
            '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
            '                , "{0}.{1} OUT:Err Update_Reserve = Nothing" _
            '                , Me.GetType.ToString _
            '                , MethodBase.GetCurrentMethod.Name))

            '    Return Nothing

            'End If


            ''子ノードの取得
            'Dim resultXmlNode As XmlNode = resultXmlNodeList.Item(0)

            Return GetTagValue(resultXmlNode, GetMileage_Info.Response.TagMileage)

        Catch ex As XmlException

            'エラーログの出力
            Logger.Error(String.Format(CultureInfo.CurrentCulture _
                         , "{0}.{1} OUT:Err XmlException = {2}" _
                         , Me.GetType.ToString _
                         , MethodBase.GetCurrentMethod.Name _
                         , ex.tostring))

            Return Nothing

        End Try

    End Function

#Region "未使用メソッド"
    'Private Function GetRoThumbnailCountFromXMLData(ByVal resultString As String, _
    '                        ByVal rowWebServiceResult As RoThumbnailCountRow) As String

    '    ''開始ログの出力
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                , "{0}.{1} RESULTXML:{2}" _
    '                , Me.GetType.ToString _
    '                , MethodBase.GetCurrentMethod.Name _
    '                , resultString))

    '    '返却DataTable
    '    Dim retDataTable As New RoThumbnailCountDataTable

    '    Try

    '        'XmlDocument
    '        Dim resultXmlDocument As New XmlDocument

    '        '返却された文字列をXML化
    '        resultXmlDocument.LoadXml(resultString)

    '        'XmlElementを取得
    '        Dim resultXmlElement As XmlElement = resultXmlDocument.DocumentElement

    '        'XmlElementの確認
    '        If resultXmlElement Is Nothing Then
    '            '取得失敗

    '            'エラーログの出力
    '            Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                        , "{0}.{1} OUT:Err XmlDocument.DocumentElement = Nothing" _
    '                        , Me.GetType.ToString _
    '                        , MethodBase.GetCurrentMethod.Name))

    '            Return Nothing

    '        End If

    '        'Resultノード取得開始
    '        '子ノードリストの取得
    '        'Dim resultXmlNodeList As XmlNodeList = resultXmlElement.GetElementsByTagName(GetMileage_Info.Response.NodeMileage_Result)
    '        Dim resultXmlNode As XmlNode = resultXmlElement.SelectSingleNode("//RoThumbnailCount_Result/Output_RoThumbnailCount")

    '        '子ノードリストの確認
    '        'If resultXmlNodeList Is Nothing OrElse resultXmlNodeList.Count = 0 Then
    '        '    '取得失敗

    '        '    'エラーログの出力
    '        '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '        '                , "{0}.{1} OUT:Err Update_Reserve = Nothing" _
    '        '                , Me.GetType.ToString _
    '        '                , MethodBase.GetCurrentMethod.Name))

    '        '    Return Nothing

    '        'End If


    '        ''子ノードの取得
    '        'Dim resultXmlNode As XmlNode = resultXmlNodeList.Item(0)

    '        Return GetTagValue(resultXmlNode, GetRoThumbnailCount_Info.Response.TagRoThumbnailCount)

    '    Catch ex As XmlException

    '        'エラーログの出力
    '        Logger.Error(String.Format(CultureInfo.CurrentCulture _
    '                     , "{0}.{1} OUT:Err XmlException = {2}" _
    '                     , Me.GetType.ToString _
    '                     , MethodBase.GetCurrentMethod.Name _
    '                     , ex.tostring))

    '        Return Nothing

    '    End Try

    'End Function

#End Region

    ''' <summary>
    ''' Tagから値を取得
    ''' </summary>
    ''' <param name="resultXmlNode">受信XMLノード</param>
    ''' <param name="tagName">Tag名</param>
    ''' <returns>結果XML</returns>
    ''' <remarks></remarks>
    Private Function GetTagValue(ByVal resultXmlNode As XmlNode, _
                                 ByVal tagName As String) As String

        '開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} IN:TAGNAME = {2}" _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name _
                    , tagName))

        '処理結果
        Dim resultValue As String = XmlErr

        'タグの取得
        Dim selectNodeList As XmlNodeList = resultXmlNode.SelectNodes(tagName)

        'タグの確認
        If selectNodeList Is Nothing OrElse selectNodeList.Count = 0 Then
            '取得失敗

            'エラーログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:Err GET {2} VALUE = Nothing" _
                        , Me.GetType.ToString _
                        , MethodBase.GetCurrentMethod.Name _
                        , tagName))

            'コードに-1を設定
            Return XmlErr

        End If

        '値の取得
        resultValue = selectNodeList.Item(0).InnerText.Trim

        '終了ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} OUT:RESULTVALUE = {2}" _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name _
                    , resultValue))

        Return resultValue

    End Function

    ''' <summary>
    ''' サービスアイテム：XMLの店舗コードと販売店コードをDMSに転換
    ''' </summary>
    ''' <param name="inXmlClass">XML作成用クラス</param>
    ''' <returns>転換後XML</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Function Change2DMSXMLOfServiceItems(ByVal inXmlClass As ServiceItemsXmlDocumentClass) As ServiceItemsXmlDocumentClass

        '2014/05/23 「ServiceCommonClassBusinessLogic」の使用廃止　START　↓↓↓
        'Using biz As New ServiceCommonClassBusinessLogic
        Using biz As New SC3250101BusinessLogic

            Dim dtResult As DmsCodeMapDataTable = biz.GetIcropToDmsCode(inXmlClass.Detail.Common.DealerCode, _
                                  SC3250101BusinessLogic.DmsCodeType.BranchCode, _
                                  inXmlClass.Detail.Common.DealerCode, _
                                  inXmlClass.Detail.Common.BranchCode, _
                                  String.Empty, _
                                  String.Empty)
            'Dim dtResult As ServiceCommonClassDataSet.DmsCodeMapDataTable = biz.GetIcropToDmsCode(inXmlClass.Detail.Common.DealerCode, _
            '          ServiceCommonClassBusinessLogic.DmsCodeType.BranchCode, _
            '          inXmlClass.Detail.Common.DealerCode, _
            '          inXmlClass.Detail.Common.BranchCode, _
            '          String.Empty, _
            '          String.Empty)


            If dtResult.Count > 0 Then
                inXmlClass.Detail.Common.DealerCode = dtResult(0).CODE1
                inXmlClass.Detail.Common.BranchCode = dtResult(0).CODE2
            End If

        End Using
        '2014/05/23 「ServiceCommonClassBusinessLogic」の使用廃止　　END　↑↑↑

        Return inXmlClass

    End Function

    ''' <summary>
    ''' マイレージ：XMLの店舗コードと販売店コードをDMSに転換
    ''' </summary>
    ''' <param name="inXmlClass">XML作成用クラス</param>
    ''' <returns>転換後XML</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Function Change2DMSXMLOfMileage(ByVal inXmlClass As Request_MileageXmlDocumentClass) As Request_MileageXmlDocumentClass

        '2014/05/23 「ServiceCommonClassBusinessLogic」の使用廃止　START　↓↓↓
        'Using biz As New ServiceCommonClassBusinessLogic
        Using biz As New SC3250101BusinessLogic

            Dim dtResult As DmsCodeMapDataTable = biz.GetIcropToDmsCode(inXmlClass.Detail.Common.DealerCode, _
                                  SC3250101BusinessLogic.DmsCodeType.BranchCode, _
                                  inXmlClass.Detail.Common.DealerCode, _
                                  inXmlClass.Detail.Common.BranchCode, _
                                  String.Empty, _
                                  String.Empty)

            'Dim dtResult As ServiceCommonClassDataSet.DmsCodeMapDataTable = biz.GetIcropToDmsCode(inXmlClass.Detail.Common.DealerCode, _
            '                      ServiceCommonClassBusinessLogic.DmsCodeType.BranchCode, _
            '                      inXmlClass.Detail.Common.DealerCode, _
            '                      inXmlClass.Detail.Common.BranchCode, _
            '                      String.Empty, _
            '                      String.Empty)

            If dtResult.Count > 0 Then
                inXmlClass.Detail.Common.DealerCode = dtResult(0).CODE1
                inXmlClass.Detail.Common.BranchCode = dtResult(0).CODE2
            End If

        End Using
        '2014/05/23 「ServiceCommonClassBusinessLogic」の使用廃止　　END　↑↑↑

        Return inXmlClass

    End Function

#Region "未使用メソッド"
    ' ''' <summary>
    ' ''' Request XMLの店舗コードと販売店コードをDMSに転換
    ' ''' </summary>
    ' ''' <param name="inXmlClass">XML作成用クラス</param>
    ' ''' <returns>転換後XML</returns>
    ' ''' <remarks></remarks>
    ' ''' <history>
    ' ''' </history>
    'Private Function Change2DMSXMLOfRoThumbnailCount(ByVal inXmlClass As RoThumbnailCountXmlDocumentClass) As RoThumbnailCountXmlDocumentClass

    '    'Using biz As New ServiceCommonClassBusinessLogic
    '    Using biz As New SC3250101BusinessLogic

    '        Dim dtResult As DmsCodeMapDataTable = biz.GetIcropToDmsCode(inXmlClass.Detail.Common.DealerCode, _
    '                              SC3250101BusinessLogic.DmsCodeType.BranchCode, _
    '                              inXmlClass.Detail.Common.DealerCode, _
    '                              inXmlClass.Detail.Common.BranchCode, _
    '                              String.Empty, _
    '                              String.Empty)

    '        'Dim dtResult As ServiceCommonClassDataSet.DmsCodeMapDataTable = biz.GetIcropToDmsCode(inXmlClass.Detail.Common.DealerCode, _
    '        '                      ServiceCommonClassBusinessLogic.DmsCodeType.BranchCode, _
    '        '                      inXmlClass.Detail.Common.DealerCode, _
    '        '                      inXmlClass.Detail.Common.BranchCode, _
    '        '                      String.Empty, _
    '        '                      String.Empty)

    '        If dtResult.Count > 0 Then
    '            inXmlClass.Detail.Common.DealerCode = dtResult(0).CODE1
    '            inXmlClass.Detail.Common.BranchCode = dtResult(0).CODE2
    '        End If

    '    End Using

    '    Return inXmlClass

    'End Function

#End Region

#End Region

#Region "未使用メソッド"
    ' ''' <summary>
    ' ''' 基幹コードへ変換処理
    ' ''' 販売店コード・店舗コード・アカウントをそれぞれ
    ' ''' 基幹販売店コード・基幹店舗コード・基幹アカウントに変換
    ' ''' </summary>
    ' ''' <param name="inStaffInfo">スタッフ情報</param>
    ' ''' <remarks>基幹コード情報ROW</remarks>
    ' ''' <history>
    ' ''' </history>
    'Public Function ChangeDmsCode(ByVal inStaffInfo As StaffContext) _
    '                              As Request_MileageXmlDocumentClass

    '    '開始ログ
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '              , "{0}.{1} {2} IN:DLRCD = {3} STRCD = {4} ACCOUNT = {5} " _
    '              , Me.GetType.ToString _
    '              , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '              , "Start" _
    '              , inStaffInfo.DlrCD, inStaffInfo.BrnCD, inStaffInfo.Account))

    '    '基幹コードへ変換処理
    '    Dim biz As New ServiceCommonClassBusinessLogic



    '    Using dtDmsCodeMap As ServiceCommonClassDataSet.DmsCodeMapDataTable = _
    '        biz.GetIcropToDmsCode(inStaffInfo.DlrCD, _
    '                              ServiceCommonClassBusinessLogic.DmsCodeType.BranchCode, _
    '                                    inStaffInfo.DlrCD, _
    '                                    inStaffInfo.BrnCD, _
    '                                    String.Empty, _
    '                                    inStaffInfo.Account)
    '        '基幹コード情報Row
    '        Dim rowDmsCodeMap As IC3190402DataSet.DmsCodeMapRow

    '        '基幹コードへ変換処理結果チェック
    '        If dtDmsCodeMap IsNot Nothing AndAlso 0 < dtDmsCodeMap.Rows.Count Then
    '            '基幹コードへ変換処理成功

    '            'Rowに変換
    '            rowDmsCodeMap = CType(dtDmsCodeMap.Rows(0), IC3190402DataSet.DmsCodeMapRow)

    '            '基幹アカウントチェック
    '            If rowDmsCodeMap.IsACCOUNTNull Then
    '                '値無し

    '                '空文字を設定する
    '                '基幹アカウント
    '                rowDmsCodeMap.ACCOUNT = String.Empty

    '            End If

    '            '基幹販売店コードチェック
    '            If rowDmsCodeMap.IsCODE1Null Then
    '                '値無し

    '                '空文字を設定する
    '                '基幹販売店コード
    '                rowDmsCodeMap.CODE1 = String.Empty

    '            End If

    '            '基幹店舗コードチェック
    '            If rowDmsCodeMap.IsCODE2Null Then
    '                '値無し

    '                '空文字を設定する
    '                '基幹店舗コード
    '                rowDmsCodeMap.CODE2 = String.Empty

    '            End If

    '        Else
    '            '基幹コードへ変換処理成功失敗

    '            '新しいRowを作成
    '            rowDmsCodeMap = CType(dtDmsCodeMap.NewDmsCodeMapRow, IC3190402DataSet.DmsCodeMapRow)

    '            '空文字を設定する
    '            '基幹アカウント
    '            rowDmsCodeMap.ACCOUNT = String.Empty
    '            '基幹販売店コード
    '            rowDmsCodeMap.CODE1 = String.Empty
    '            '基幹店舗コード
    '            rowDmsCodeMap.CODE2 = String.Empty

    '        End If

    '        '終了ログ
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                   , "{0}.{1} {2} dtDmsCodeMap:COUNT = {3}" _
    '                   , Me.GetType.ToString _
    '                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                   , "End" _
    '                   , dtDmsCodeMap.Count))

    '        '結果返却
    '        Return rowDmsCodeMap
    '    End Using

    'End Function
#End Region

End Class
