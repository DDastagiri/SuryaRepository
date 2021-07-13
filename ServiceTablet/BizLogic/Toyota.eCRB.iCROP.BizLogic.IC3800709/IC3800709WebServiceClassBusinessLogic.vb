'-------------------------------------------------------------------------
'IC3800709WebServiceClassBusinessLogic.vb
'-------------------------------------------------------------------------
'機能：顧客検索用情報取得WebService送受信用関数
'補足：GetCustomerSearchInfo
'作成：2013/12/27 TMEJ 陳　 TMEJ次世代サービス 工程管理機能開発
'更新：2015/06/05 TMEJ 小澤 TMT号口調査 XML解析中にエラーが発生した場合はログを出力するように修正
'更新：2020/01/29 NSK 今泉 TR-SVT-TKM-20191030-001 新車が販売店に来店した際にVIN番号を入力しても検索エラーが表示される
'更新：
'─────────────────────────────────────

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
Imports Toyota.eCRB.DMSLinkage.CustomerInfo.Api.DataAccess
Imports Toyota.eCRB.DMSLinkage.CustomerInfo.Api.DataAccess.IC3800709DataSet
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.BizLogic
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.DataAccess
Imports Toyota.eCRB.CommonUtility.SMBCommonClass.Api.BizLogic

Public Class IC3800709BusinessLogic
    Inherits BaseBusinessComponent
    Implements IDisposable

#Region "定数"

    ''' <summary>
    ''' 成功
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ResultSuccess As Long = 0

    ''' <summary>
    ''' XML戻り値解析失敗
    ''' </summary>
    ''' <remarks></remarks>
    Public Const XmlErr As String = "-1"
    ''' <summary>
    ''' WebService名(IC3A09922)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WebServiceID As String = "IC3A09922"

    ''' <summary>
    ''' WebServiceメソッド名
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WebServiceMethod As String = "GetCustomerList"

    ''' <summary>
    ''' WebService引数名
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WebServiceArgument As String = "xsData="
    ''' <summary>
    ''' WebServiceURL
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WebServiceURL As String = "LINK_URL_CST_SEARCH"

    ''' <summary>
    ''' WebService ヘッダーコメント削除置換用文字列
    ''' </summary>
    ''' <remarks></remarks>
    Private Const XmlReplace As String = "<?xml version=""1.0"" encoding=""utf-16""?>"

    ''' <summary>
    ''' エンコード(UTF-8)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const EncodingUTF8 As String = "UTF-8"

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

    ''' <summary>
    ''' Node名(Result)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NodeResult As String = "Result"

    ''' <summary>
    ''' Tag名(DealerCode)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagDealerCode As String = "DealerCode"

    ''' <summary>
    ''' Tag名(BranchCode)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagBranchCode As String = "BranchCode"

    ''' <summary>
    ''' Tag名(ResultCode)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagResultCode As String = "ResultCode"

    ''' <summary>
    ''' Tag名(AllCount)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagAllCount As String = "AllCount"

    ''' <summary>
    ''' Tag名(Count)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagCount As String = "Count"

    ''' <summary>
    ''' Tag名(Start)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagStart As String = "Start"

    ''' <summary>
    ''' Node名(CustInfo)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NodeCustInfo As String = "CustInfo"

    ''' <summary>
    ''' Tag名(CustomerCode)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagCustomerCode As String = "CustomerCode"

    ''' <summary>
    ''' Tag名(SocialID)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagSocialID As String = "SocialID"

    ''' <summary>
    ''' Tag名(CustomerName)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagCustomerName As String = "CustomerName"

    ''' <summary>
    ''' Tag名(TelNumber)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagTelNumber As String = "TelNumber"

    ''' <summary>
    ''' Tag名(Mobile)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagMobile As String = "Mobile"

    ''' <summary>
    ''' Tag名(VIPFlg)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagVIPFlg As String = "VIPFlg"

    ''' <summary>
    ''' Tag名(CustomerType)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagCustomerType As String = "CustomerType"

    ''' <summary>
    ''' Tag名(NewcustomerID)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagNewcustomerID As String = "NewcustomerID"

    ''' <summary>
    ''' Node名(VhcInfo)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NodeVhcInfo As String = "VhcInfo"

    ''' <summary>
    ''' Tag名(VehicleRegistrationNumber)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagVehicleRegistrationNumber As String = "VehicleRegistrationNumber"

    ''' <summary>
    ''' Tag名(Vin)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagVin As String = "Vin"

    ''' <summary>
    ''' Tag名(ModelCode)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagModelCode As String = "ModelCode"

    ''' <summary>
    ''' Tag名(VehicleAreaCode)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagVehicleAreaCode As String = "VehicleAreaCode"

    ''' <summary>
    ''' Tag名(Customer_Flag)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagCustomer_Flag As String = "Customer_Flag"

    ''' <summary>
    ''' Tag名(SalesStaffCode)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagSalesStaffCode As String = "SalesStaffCode"

    ''' <summary>
    ''' Tag名(ServiceAdviserCode)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagServiceAdviserCode As String = "ServiceAdviserCode"

    ''' <summary>
    ''' Tag名(SeriesName)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagSeriesName As String = "SeriesName"

    ''' <summary>
    ''' 基幹連携送信タイムアウト値
    ''' </summary>
    Private Const SysLinkSendTimeOutVal = "LINK_SEND_TIMEOUT_VAL"

    ''' <summary>
    ''' システム設定不備エラー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ErrorSysEnv As Integer = 1121

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

    ''' <summary>
    ''' タグ必須チェック制御
    ''' </summary>
    Private Enum TagCheckType

        ''' <summary>
        ''' 必須タグ
        ''' </summary>
        MandatoryTag = 0

        ''' <summary>
        ''' 必須なしタグ
        ''' </summary>
        OptionalTag = 1

        ''' <summary>
        ''' 条件必須タグ
        ''' </summary>
        ConditionalMandatoryTag = 2

    End Enum

#End Region

#Region "プロパティ"

    ''' <summary>
    ''' 販売店コードを保持
    ''' </summary>
    ''' <remarks></remarks>
    Private CurrentDlrCdValue As String
    Private Property CurrentDlrCd As String

        Set(ByVal value As String)

            CurrentDlrCdValue = value

        End Set

        Get

            Return CurrentDlrCdValue

        End Get

    End Property

#End Region

#Region "Public"

    ''' <summary>
    ''' 顧客検索用情報取得WebService呼出処理
    ''' </summary>
    ''' <param name="inXmlClass">XML作成用クラス</param>
    ''' <returns>WebService処理結果。Nothingの場合はXML解析エラー発生する原因となる</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2015/06/05 TMEJ 小澤 TMT号口調査 XML解析中にエラーが発生した場合はログを出力するように修正
    ''' </history>
    Public Function CallGetCustomerSearchInfoWebService(ByVal inXmlClass As CustomerSearchXmlDocumentClass) As CustomerSearchResultDataTable

        ''開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START " _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name))

        '販売店コード保持
        Me.CurrentDlrCd = inXmlClass.Detail.Common.DealerCode

        'DMSコード転換
        inXmlClass = Me.Change2DMSXML(inXmlClass)

        'XML戻り値用DataTable
        Dim dtWebServiceResult As New CustomerSearchResultDataTable

        'XML必須チェック
        If Not Me.XmlCheck(inXmlClass) Then

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} XML Err : Mandatory value is empty" _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name))

            dtWebServiceResult = New CustomerSearchResultDataTable

            Dim drTemp As IC3800709DataSet.CustomerSearchResultRow = dtWebServiceResult.NewCustomerSearchResultRow
            drTemp.ResultCode = ReturnCode.ErrOther

            dtWebServiceResult.AddCustomerSearchResultRow(drTemp)

            Return dtWebServiceResult

        End If

        'XML戻り値用DataRow
        Dim rowWebServiceResult As CustomerSearchResultRow = dtWebServiceResult.NewCustomerSearchResultRow

        Try
            'WebServiceURLの取得
            Dim envSettingRow As String = String.Empty
            Using biz As New ServiceCommonClassBusinessLogic
                envSettingRow = biz.GetDlrSystemSettingValueBySettingName(WebServiceURL)
            End Using

            'URLの取得確認
            If String.IsNullOrEmpty(envSettingRow) Then
                'URL取得失敗

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} DlrEnvSetting == NOTHING OUT:resultXmlValue.ResultCode = {2}" _
                        , Me.GetType.ToString _
                        , MethodBase.GetCurrentMethod.Name _
                        , ReturnCode.ErrOther.ToString()))

                Dim drTemp As IC3800709DataSet.CustomerSearchResultRow = dtWebServiceResult.NewCustomerSearchResultRow
                drTemp.ResultCode = ReturnCode.ErrOther

                dtWebServiceResult.AddCustomerSearchResultRow(drTemp)

                Return dtWebServiceResult

            End If

            'WebServiceのURLを作成
            Dim createUrl As String = String.Concat(envSettingRow, "/", WebServiceMethod)

            'WebService送信用XML作成処理
            Dim sendXml As String = CreateXml(inXmlClass)

            'XMLのヘッダー部分を削除
            sendXml = sendXml.Replace(XmlReplace, String.Empty)

            '送信XMLをエンコードし引数に指定
            sendXml = String.Concat(WebServiceArgument, HttpUtility.UrlEncode(sendXml))

            'WebService送受信処理
            Dim resultString As String = CallWebServiceSite(sendXml, createUrl)
            If String.IsNullOrEmpty(resultString) Then

                Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} CallWebServiceSite == EmptyOrNull OUT:resultXmlValue.ResultCode = {2}" _
                        , Me.GetType.ToString _
                        , MethodBase.GetCurrentMethod.Name _
                        , ReturnCode.ErrOther.ToString()))

                Dim drTemp As IC3800709DataSet.CustomerSearchResultRow = dtWebServiceResult.NewCustomerSearchResultRow
                drTemp.ResultCode = ReturnCode.ErrOther

                dtWebServiceResult.AddCustomerSearchResultRow(drTemp)

                Return dtWebServiceResult

            End If

            '2020/01/29 NSK 今泉 TR-SVT-TKM-20191030-001 新車が販売店に来店した際にVIN番号を入力しても検索エラーが表示される START
            '返却された文字列をデコード
            'resultString = HttpUtility.HtmlDecode(resultString)
            '2020/01/29 NSK 今泉 TR-SVT-TKM-20191030-001 新車が販売店に来店した際にVIN番号を入力しても検索エラーが表示される END

            'XML名前空間用の正規表現設定
            Dim regex As Regex = New Regex(" xmln.*=""[^""]*"".")

            'XML名前空間を除去
            resultString = regex.Replace(resultString, Space(0))

            'WebServiceの戻りXMLを解析し値を取得
            dtWebServiceResult = GetXMLData(resultString, rowWebServiceResult)

            '2015/06/05 TMEJ 小澤 TMT号口調査 XML解析中にエラーが発生した場合はログを出力するように修正 START

            ''Nothingを返却する場合
            'If IsNothing(dtWebServiceResult) Then

            '    dtWebServiceResult = New CustomerSearchResultDataTable

            '    Dim drTemp As IC3800709DataSet.CustomerSearchResultRow = dtWebServiceResult.NewCustomerSearchResultRow
            '    drTemp.ResultCode = ReturnCode.ErrOther

            '    dtWebServiceResult.AddCustomerSearchResultRow(drTemp)

            '    Logger.Error(String.Format(CultureInfo.CurrentCulture _
            '                 , "{0}.{1} OUT:ErrWebService ResultCode = {2}" _
            '                 , Me.GetType.ToString _
            '                 , MethodBase.GetCurrentMethod.Name _
            '                 , ReturnCode.ErrOther.ToString()))

            '    Return dtWebServiceResult

            'End If

            '2015/06/05 TMEJ 小澤 TMT号口調査 XML解析中にエラーが発生した場合はログを出力するように修正 START

            '作成したDataTebleのエラーコードチェック
            If Not (IsNothing(dtWebServiceResult)) AndAlso _
               0 < dtWebServiceResult.Count AndAlso _
               Not (dtWebServiceResult(0).IsResultCodeNull) AndAlso _
               dtWebServiceResult(0).ResultCode <> ReturnCode.Success Then
                'データが存在している且つ、「0：成功」以外の場合
                'ログを出力する
                '送信XMLのログ出力
                Logger.Warn(String.Format(CultureInfo.InvariantCulture, _
                                           "{0} {1}.GetXMLData Error : SendXml = {2}", _
                                           Me.GetType.ToString, _
                                           MethodBase.GetCurrentMethod.Name, _
                                           sendXml))
                '受信XMLのログ出力
                Logger.Warn(String.Format(CultureInfo.InvariantCulture, _
                                           "{0} {1}.GetXMLData Error : ReceivedXML = {2}", _
                                           Me.GetType.ToString, _
                                           System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                           resultString))

            End If

            '2015/06/05 TMEJ 小澤 TMT号口調査 XML解析中にエラーが発生した場合はログを出力するように修正 END

            '終了ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:resultXmlValue.ResultCode = {2}" _
                        , Me.GetType.ToString _
                        , MethodBase.GetCurrentMethod.Name _
                        , rowWebServiceResult.ResultCode))

            Return dtWebServiceResult

        Catch ex As System.Net.WebException
            'WebServiceエラー

            Dim resultCode As String = String.Empty

            If IsNothing(dtWebServiceResult) Then
                dtWebServiceResult = New CustomerSearchResultDataTable
            End If

            If ex.Status = WebExceptionStatus.Timeout Then

                Dim drTemp As IC3800709DataSet.CustomerSearchResultRow = dtWebServiceResult.NewCustomerSearchResultRow
                drTemp.ResultCode = ReturnCode.ErrTimeout
                resultCode = ReturnCode.ErrTimeout.ToString()

                dtWebServiceResult.AddCustomerSearchResultRow(drTemp)

            Else

                Dim drTemp As IC3800709DataSet.CustomerSearchResultRow = dtWebServiceResult.NewCustomerSearchResultRow
                drTemp.ResultCode = ReturnCode.ErrOther
                resultCode = ReturnCode.ErrTimeout.ToString()

                dtWebServiceResult.AddCustomerSearchResultRow(drTemp)

            End If

            Logger.Error(String.Format(CultureInfo.CurrentCulture _
                         , "{0}.{1} OUT:ErrWebService ResultCode = {2}" _
                         , Me.GetType.ToString _
                         , MethodBase.GetCurrentMethod.Name _
                         , resultCode))

            Return dtWebServiceResult

        Catch ex2 As System.Exception

            If IsNothing(dtWebServiceResult) Then
                dtWebServiceResult = New CustomerSearchResultDataTable
            End If

            Dim drTemp As IC3800709DataSet.CustomerSearchResultRow = dtWebServiceResult.NewCustomerSearchResultRow
            drTemp.ResultCode = ReturnCode.ErrOther

            dtWebServiceResult.AddCustomerSearchResultRow(drTemp)

            Logger.Error(String.Format(CultureInfo.CurrentCulture _
                         , "{0}.{1} OUT:ErrWebService ResultCode = {2}" _
                         , Me.GetType.ToString _
                         , MethodBase.GetCurrentMethod.Name _
                         , ReturnCode.ErrOther.ToString()))

            Return dtWebServiceResult

        End Try

    End Function

#End Region

#Region "必須チェック"
    ''' <summary>
    ''' XML必須チェック
    ''' </summary>
    ''' <param name="inXmlClass">XML作成用クラス</param>
    ''' <remarks></remarks>
    Private Function XmlCheck(ByVal inXmlClass As CustomerSearchXmlDocumentClass) As Boolean

        '開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} " _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name))

        If String.IsNullOrEmpty(inXmlClass.Head.TransmissionDate) Then

            '終了ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:TransmissionDate: NullOrEmpty; Return: {2}" _
                        , Me.GetType.ToString _
                        , MethodBase.GetCurrentMethod.Name _
                        , False))

            Return False
        End If
        If String.IsNullOrEmpty(inXmlClass.Detail.Common.DealerCode) Then

            '終了ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:DealerCode: NullOrEmpty; Return: {2}" _
                        , Me.GetType.ToString _
                        , MethodBase.GetCurrentMethod.Name _
                        , False))

            Return False
        End If
        If String.IsNullOrEmpty(inXmlClass.Detail.Common.BranchCode) Then

            '終了ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:BranchCode: NullOrEmpty; Return: {2}" _
                        , Me.GetType.ToString _
                        , MethodBase.GetCurrentMethod.Name _
                        , False))

            Return False
        End If
        If String.IsNullOrEmpty(inXmlClass.Detail.Common.StaffCode) Then

            '終了ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:StaffCode: NullOrEmpty; Return: {2}" _
                        , Me.GetType.ToString _
                        , MethodBase.GetCurrentMethod.Name _
                        , False))

            Return False
        End If
        If String.IsNullOrEmpty(inXmlClass.Detail.SearchCondition.Start) Then

            '終了ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:Start: NullOrEmpty; Return: {2}" _
                        , Me.GetType.ToString _
                        , MethodBase.GetCurrentMethod.Name _
                        , False))

            Return False
        End If
        If String.IsNullOrEmpty(inXmlClass.Detail.SearchCondition.Count) Then

            '終了ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:Count: NullOrEmpty; Return: {2}" _
                        , Me.GetType.ToString _
                        , MethodBase.GetCurrentMethod.Name _
                        , False))

            Return False
        End If
        If String.IsNullOrEmpty(inXmlClass.Detail.SearchCondition.Sort1) Then

            '終了ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:Sort1: NullOrEmpty; Return: {2}" _
                        , Me.GetType.ToString _
                        , MethodBase.GetCurrentMethod.Name _
                        , False))

            Return False
        End If
        If Not String.IsNullOrEmpty(inXmlClass.Detail.SearchCondition.VclRegNo) Then
            If String.IsNullOrEmpty(inXmlClass.Detail.SearchCondition.VclRegNo_MatchType) Then

                '終了ログの出力
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} OUT:VclRegNo And VclRegNo_MatchType: NullOrEmpty; Return: {2}" _
                            , Me.GetType.ToString _
                            , MethodBase.GetCurrentMethod.Name _
                            , False))

                Return False
            End If
        End If
        If Not String.IsNullOrEmpty(inXmlClass.Detail.SearchCondition.CustomerName) Then
            If String.IsNullOrEmpty(inXmlClass.Detail.SearchCondition.CustomerName_MatchType) Then

                '終了ログの出力
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} OUT:CustomerName And CustomerName_MatchType: NullOrEmpty; Return: {2}" _
                            , Me.GetType.ToString _
                            , MethodBase.GetCurrentMethod.Name _
                            , False))

                Return False
            End If
        End If
        If Not String.IsNullOrEmpty(inXmlClass.Detail.SearchCondition.Vin) Then
            If String.IsNullOrEmpty(inXmlClass.Detail.SearchCondition.Vin_MatchType) Then

                '終了ログの出力
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} OUT:Vin And Vin_MatchType: NullOrEmpty; Return: {2}" _
                            , Me.GetType.ToString _
                            , MethodBase.GetCurrentMethod.Name _
                            , False))

                Return False
            End If
        End If
        If Not String.IsNullOrEmpty(inXmlClass.Detail.SearchCondition.BasRezid) Then
            If String.IsNullOrEmpty(inXmlClass.Detail.SearchCondition.BasRezid_MatchType) Then

                '終了ログの出力
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} OUT:BasRezid And BasRezid_MatchType: NullOrEmpty; Return: {2}" _
                            , Me.GetType.ToString _
                            , MethodBase.GetCurrentMethod.Name _
                            , False))

                Return False
            End If
        End If
        If Not String.IsNullOrEmpty(inXmlClass.Detail.SearchCondition.R_O) Then
            If String.IsNullOrEmpty(inXmlClass.Detail.SearchCondition.R_O_MatchType) Then

                '終了ログの出力
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} OUT:R_O And R_O_MatchType: NullOrEmpty; Return: {2}" _
                            , Me.GetType.ToString _
                            , MethodBase.GetCurrentMethod.Name _
                            , False))

                Return False
            End If
        End If
        If Not String.IsNullOrEmpty(inXmlClass.Detail.SearchCondition.TelNumber) Then
            If String.IsNullOrEmpty(inXmlClass.Detail.SearchCondition.TelNumber_MatchType) Then

                '終了ログの出力
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} OUT:TelNumber And TelNumber_MatchType: NullOrEmpty; Return: {2}" _
                            , Me.GetType.ToString _
                            , MethodBase.GetCurrentMethod.Name _
                            , False))

                Return False
            End If
        End If
        If String.IsNullOrEmpty(inXmlClass.Detail.SearchCondition.VclRegNo) And _
            String.IsNullOrEmpty(inXmlClass.Detail.SearchCondition.CustomerName) And _
            String.IsNullOrEmpty(inXmlClass.Detail.SearchCondition.Vin) And _
            String.IsNullOrEmpty(inXmlClass.Detail.SearchCondition.BasRezid) And _
            String.IsNullOrEmpty(inXmlClass.Detail.SearchCondition.R_O) And _
            String.IsNullOrEmpty(inXmlClass.Detail.SearchCondition.TelNumber) Then

            '終了ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:no ConditionTag be setted; Return: {2}" _
                        , Me.GetType.ToString _
                        , MethodBase.GetCurrentMethod.Name _
                        , False))

            Return False
        End If

        '終了ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} OUT:RETURN = {2}" _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name _
                    , True))

        Return True

    End Function

#End Region

#Region "XML作成"

    ''' <summary>
    ''' XML作成(メイン)
    ''' </summary>
    ''' <param name="inXmlClass">XML作成用クラス</param>
    ''' <returns>XMLString</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Function CreateXml(ByVal inXmlClass As CustomerSearchXmlDocumentClass) As String

        ''開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} " _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name))


        'XMLのHeadTagの作成処理
        inXmlClass = CreateHeadTag(inXmlClass)

        'テキストWriter
        Using writer As New StringWriter(CultureInfo.InvariantCulture)

            'XMLシリアライザー型の設定
            Dim serializer As New XmlSerializer(GetType(CustomerSearchXmlDocumentClass))

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
    ''' XML作成(HeadTag)
    ''' </summary>
    ''' <param name="inXmlClass">XML作成用クラス</param>
    ''' <returns>XML作成用クラス</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Function CreateHeadTag(ByVal inXmlClass As CustomerSearchXmlDocumentClass) As CustomerSearchXmlDocumentClass

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

        '終了ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} OUT:RETURNCODE = {2}" _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name _
                    , inXmlClass))

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


        'タイムアウト設定値を取得
        Dim timeOut As String = Me.GetTimeOutValues()

        'システム設定値の取得でエラーがあった場合
        If String.IsNullOrEmpty(timeOut) Then
            
            '終了ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} TimeOutValue error,  OUT:RETURNSTRING = Empty" _
                        , Me.GetType.ToString _
                        , MethodBase.GetCurrentMethod.Name))

            Return String.Empty
        End If


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

        '送信タイムアウト設定
        req.Timeout = CType(timeOut, Integer)

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

    ''' <summary>
    ''' WebServiceの戻りXMLを解析し値を取得
    ''' </summary>
    ''' <param name="resultString">送信XML文字列</param>
    ''' <param name="rowWebServiceResult">XML戻り値用DataRow</param>
    ''' <returns>WebService結果</returns>
    ''' <remarks></remarks>
    Private Function GetXMLData(ByVal resultString As String, _
                                ByVal rowWebServiceResult As CustomerSearchResultRow) As CustomerSearchResultDataTable

        ''開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} RESULTXML:{2}" _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name _
                    , resultString))

        '返却DataTable
        Dim retDataTable As New CustomerSearchResultDataTable

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
                Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} OUT:Err XmlDocument.DocumentElement = Nothing" _
                            , Me.GetType.ToString _
                            , MethodBase.GetCurrentMethod.Name))

                Return Me.SetErrReturnDataTable(ReturnCode.ErrDms)

            End If

            'Resultノード取得開始
            '子ノードリストの取得
            Dim resultXmlNodeList As XmlNodeList = resultXmlElement.GetElementsByTagName(NodeResult)

            '子ノードリストの確認
            If resultXmlNodeList Is Nothing OrElse resultXmlNodeList.Count = 0 Then
                '取得失敗

                'エラーログの出力
                Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} OUT:Err Update_Reserve = Nothing" _
                            , Me.GetType.ToString _
                            , MethodBase.GetCurrentMethod.Name))

                Return Me.SetErrReturnDataTable(ReturnCode.ErrDms)

            End If


            '子ノードの取得
            Dim resultXmlNode As XmlNode = resultXmlNodeList.Item(0)

            '解析したXMLから設定されている値の取得
            rowWebServiceResult = GetXmlResultNodeValue(rowWebServiceResult, resultXmlNode)

            If rowWebServiceResult Is Nothing Then
                Return Me.SetErrReturnDataTable(ReturnCode.ErrDms)
            End If

            '返却顧客件数ない場合
            If rowWebServiceResult.AllCount = "0" Then
                Return retDataTable
            End If

            'Resultノード取得終了

            'CustInfoノード取得開始
            '子ノードリストの取得
            Dim custInfoXmlNodeList As XmlNodeList = resultXmlElement.GetElementsByTagName(NodeCustInfo)

            '子ノードリストの確認
            If custInfoXmlNodeList Is Nothing OrElse custInfoXmlNodeList.Count = 0 Then
                '取得失敗

                'エラーログの出力
                Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} OUT:Err Update_Reserve = Nothing" _
                            , Me.GetType.ToString _
                            , MethodBase.GetCurrentMethod.Name))

                Return Me.SetErrReturnDataTable(ReturnCode.ErrDms)

            End If

            '解析したXMLから設定されている値の取得
            retDataTable = GetXmlCustInfoNodeValue(rowWebServiceResult, custInfoXmlNodeList)


            '終了ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:RETURNSTRING = {2}" _
                        , Me.GetType.ToString _
                        , MethodBase.GetCurrentMethod.Name _
                        , resultString))

            Return retDataTable


        Catch ex As XmlException

            'エラーログの出力
            Logger.Error(String.Format(CultureInfo.CurrentCulture _
                         , "{0}.{1} OUT:Err XmlException = {2}" _
                         , Me.GetType.ToString _
                         , MethodBase.GetCurrentMethod.Name _
                         , ex.Message))

            Return Nothing

        End Try

    End Function

    ''' <summary>
    ''' エラー返却テーブルを作り
    ''' </summary>
    ''' <param name="errCode">エラーコード</param>
    ''' <returns>WebService結果</returns>
    ''' <remarks></remarks>
    Private Function SetErrReturnDataTable(ByVal errCode As ReturnCode) As CustomerSearchResultDataTable

        Dim dtWebServiceResult As CustomerSearchResultDataTable = New CustomerSearchResultDataTable()
        Dim drTemp As IC3800709DataSet.CustomerSearchResultRow = dtWebServiceResult.NewCustomerSearchResultRow
        drTemp.ResultCode = errCode

        dtWebServiceResult.AddCustomerSearchResultRow(drTemp)

        Return dtWebServiceResult

    End Function

    ''' <summary>
    ''' 戻りXMLから設定されている値を取得(Result Node)
    ''' </summary>
    ''' <param name="rowWebServiceResultRow">XML戻り値用DataRow</param>
    ''' <param name="resultXmlNode">受信XMLノード</param>
    ''' <returns>XML戻り値用DataRow</returns>
    ''' <remarks></remarks>
    Private Function GetXmlResultNodeValue(ByVal rowWebServiceResultRow As CustomerSearchResultRow, _
                                     ByVal resultXmlNode As XmlNode) As CustomerSearchResultRow

        '開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} " _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name))

        'ResultCodeタグの値取得
        Dim strResultCode As String = GetTagValue(resultXmlNode, TagResultCode, TagCheckType.MandatoryTag)
        rowWebServiceResultRow.ResultCode = CType(strResultCode, Long)

        'WEBServiceの処理結果確認
        If rowWebServiceResultRow.ResultCode <> ResultSuccess Then
            '処理結果が失敗

            '終了ログの出力
            Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                         , "{0}.{1} OUT:Err ResultCode = {2}" _
                         , Me.GetType.ToString _
                         , MethodBase.GetCurrentMethod.Name _
                         , rowWebServiceResultRow.ResultCode))

            Return rowWebServiceResultRow

        End If


        'DealerCodeタグの値取得
        rowWebServiceResultRow.DealerCode = GetTagValue(resultXmlNode, TagDealerCode, TagCheckType.MandatoryTag)

        'DealerCodeタグの値取得確認
        If rowWebServiceResultRow.DealerCode = XmlErr Then
            '処理結果が失敗

            '終了ログの出力
            Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:Err TagDealerCode = NOTHING" _
                        , Me.GetType.ToString _
                        , MethodBase.GetCurrentMethod.Name))

            Return Nothing

        End If


        'BranchCodeタグの値取得
        rowWebServiceResultRow.BranchCode = GetTagValue(resultXmlNode, TagBranchCode, TagCheckType.MandatoryTag)

        'BranchCodeタグの値取得確認
        If rowWebServiceResultRow.BranchCode = XmlErr Then
            '処理結果が失敗

            '終了ログの出力
            Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:Err TagBranchCode = NOTHING" _
                        , Me.GetType.ToString _
                        , MethodBase.GetCurrentMethod.Name))

            Return Nothing

        End If


        'AllCountタグの値取得
        rowWebServiceResultRow.AllCount = GetTagValue(resultXmlNode, TagAllCount, TagCheckType.MandatoryTag)

        'AllCountタグの値取得確認
        If rowWebServiceResultRow.AllCount = XmlErr Then
            '処理結果が失敗

            '終了ログの出力
            Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:Err TagAllCount = NOTHING" _
                        , Me.GetType.ToString _
                        , MethodBase.GetCurrentMethod.Name))

            Return Nothing

        End If


        'Countタグの値取得
        rowWebServiceResultRow.Count = GetTagValue(resultXmlNode, TagCount, TagCheckType.MandatoryTag)

        'Countタグの値取得確認
        If rowWebServiceResultRow.Count = XmlErr Then
            '処理結果が失敗

            '終了ログの出力
            Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:Err TagCount = NOTHING" _
                        , Me.GetType.ToString _
                        , MethodBase.GetCurrentMethod.Name))

            Return Nothing

        End If


        'Startタグの値取得
        rowWebServiceResultRow.Start = GetTagValue(resultXmlNode, TagStart, TagCheckType.MandatoryTag)

        'Startタグの値取得確認
        If rowWebServiceResultRow.Start = XmlErr Then
            '処理結果が失敗

            '終了ログの出力
            Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:Err TagStart = NOTHING" _
                        , Me.GetType.ToString _
                        , MethodBase.GetCurrentMethod.Name))

            Return Nothing

        End If

        '終了ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} OUT:RETURNCODE = {2}" _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name _
                    , rowWebServiceResultRow.ResultCode))

        Return rowWebServiceResultRow

    End Function

    ''' <summary>
    ''' 戻りXMLから設定されている値を取得(CustInfo Node)
    ''' </summary>
    ''' <param name="rowWebServiceResultRow">XML戻り値用DataRow</param>
    ''' <param name="custInfoXmlNodeList">受信XMLノードリスト</param>
    ''' <returns>XML戻り値用DataTable</returns>
    ''' <remarks></remarks>
    Private Function GetXmlCustInfoNodeValue(ByVal rowWebServiceResultRow As CustomerSearchResultRow, _
                                     ByVal custInfoXmlNodeList As XmlNodeList) As CustomerSearchResultDataTable

        '開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} " _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name))

        Dim retCustomerSearchResultDataTable As New CustomerSearchResultDataTable

        For Each custInfoNode As XmlNode In custInfoXmlNodeList

            'VhcInfoタグの値取得
            Dim vhcInfoInnerText As String = GetTagValue(custInfoNode, NodeVhcInfo, TagCheckType.MandatoryTag)

            'VhcInfoタグの値取得確認
            If vhcInfoInnerText = XmlErr Then
                '処理結果が失敗

                '終了ログの出力
                Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} OUT:Err TagVhcInfo = NOTHING" _
                            , Me.GetType.ToString _
                            , MethodBase.GetCurrentMethod.Name))

                Return Me.SetErrReturnDataTable(ReturnCode.ErrDms)

            End If

            'CustInfo Node値を取得
            Dim vhcInfoXmlNodeList As XmlNodeList = custInfoNode.SelectNodes(NodeVhcInfo)

            Dim vchInfoIndex = 0
            Do While vchInfoIndex < vhcInfoXmlNodeList.Count

                Dim rowTemp As CustomerSearchResultRow = retCustomerSearchResultDataTable.NewCustomerSearchResultRow()

                'Result Node値を取得
                rowTemp.ItemArray = rowWebServiceResultRow.ItemArray

                'CustomerCodeタグの値取得
                rowTemp.CustomerCode = GetTagValue(custInfoNode, TagCustomerCode, TagCheckType.MandatoryTag)

                'CustomerCodeタグの値取得確認
                If rowTemp.CustomerCode = XmlErr Then
                    '処理結果が失敗

                    '終了ログの出力
                    Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} OUT:Err TagCustomerCode = NOTHING" _
                                , Me.GetType.ToString _
                                , MethodBase.GetCurrentMethod.Name))

                    Return Me.SetErrReturnDataTable(ReturnCode.ErrDms)

                End If

                Using biz As New SMBCommonClassBusinessLogic
                    'DMSCSTID変換
                    rowTemp.CustomerCode = biz.ReplaceBaseCustomerCode(Me.CurrentDlrCd, rowTemp.CustomerCode)
                End Using

                'SocialIDタグの値取得
                rowTemp.SocialID = GetTagValue(custInfoNode, TagSocialID, TagCheckType.OptionalTag)

                'CustomerNameタグの値取得
                rowTemp.CustomerName = GetTagValue(custInfoNode, TagCustomerName, TagCheckType.MandatoryTag)

                'CustomerNameタグの値取得確認
                If rowTemp.CustomerName = XmlErr Then
                    '処理結果が失敗

                    '終了ログの出力
                    Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} OUT:Err TagCustomerName = NOTHING" _
                                , Me.GetType.ToString _
                                , MethodBase.GetCurrentMethod.Name))

                    Return Me.SetErrReturnDataTable(ReturnCode.ErrDms)

                End If

                'TelNumberタグの値取得
                rowTemp.TelNumber = GetTagValue(custInfoNode, TagTelNumber, TagCheckType.OptionalTag)


                'Mobileタグの値取得
                rowTemp.Mobile = GetTagValue(custInfoNode, TagMobile, TagCheckType.OptionalTag)


                'VIPFlgタグの値取得
                rowTemp.VIPFlg = GetTagValue(custInfoNode, TagVIPFlg, TagCheckType.OptionalTag)


                'CustomerTypeタグの値取得
                rowTemp.CustomerType = GetTagValue(custInfoNode, TagCustomerType, TagCheckType.OptionalTag)


                'NewcustomerIDタグの値取得
                rowTemp.NewcustomerID = GetTagValue(custInfoNode, TagNewcustomerID, TagCheckType.OptionalTag)


                Dim vhcInfoXmlNode As XmlNode = vhcInfoXmlNodeList.Item(vchInfoIndex)

                'VehicleRegistrationNumberタグの値取得
                rowTemp.VehicleRegistrationNumber = GetTagValue(vhcInfoXmlNode, TagVehicleRegistrationNumber, TagCheckType.ConditionalMandatoryTag)

                'Vinタグの値取得
                rowTemp.Vin = GetTagValue(vhcInfoXmlNode, TagVin, TagCheckType.ConditionalMandatoryTag)

                If (String.IsNullOrEmpty(rowTemp.VehicleRegistrationNumber)) _
                    AndAlso (String.IsNullOrEmpty(rowTemp.Vin)) Then
                    'VclRegNoとVin全部値なしの場合、エラーになる
                    '終了ログの出力
                    Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} OUT:Err VehicleRegistrationNumber and Vin = NOTHING" _
                                , Me.GetType.ToString _
                                , MethodBase.GetCurrentMethod.Name))

                    Return Me.SetErrReturnDataTable(ReturnCode.ErrDms)
                End If


                'ModelCodeタグの値取得
                rowTemp.ModelCode = GetTagValue(vhcInfoXmlNode, TagModelCode, TagCheckType.OptionalTag)


                'VehicleAreaCodeタグの値取得
                rowTemp.VehicleAreaCode = GetTagValue(vhcInfoXmlNode, TagVehicleAreaCode, TagCheckType.OptionalTag)


                'Customer_Flagタグの値取得
                rowTemp.Customer_Flag = GetTagValue(vhcInfoXmlNode, TagCustomer_Flag, TagCheckType.OptionalTag)


                'SalesStraffCodeタグの値取得
                rowTemp.SalesStaffCode = GetTagValue(vhcInfoXmlNode, TagSalesStaffCode, TagCheckType.OptionalTag)


                'ServiceAdviserCodeタグの値取得
                rowTemp.ServiceAdviserCode = GetTagValue(vhcInfoXmlNode, TagServiceAdviserCode, TagCheckType.OptionalTag)


                'SeriesNameタグの値取得
                rowTemp.SeriesName = GetTagValue(vhcInfoXmlNode, TagSeriesName, TagCheckType.OptionalTag)


                '結果テーブルにデータ行追加
                retCustomerSearchResultDataTable.AddCustomerSearchResultRow(rowTemp)

                '次の車両情報索引
                vchInfoIndex = vchInfoIndex + 1

            Loop

        Next

        '終了ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} OUT:RETURNCODE = {2}" _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name _
                    , rowWebServiceResultRow.ResultCode))

        Return retCustomerSearchResultDataTable

    End Function

    ''' <summary>
    ''' Tagから値を取得
    ''' </summary>
    ''' <param name="resultXmlNode">受信XMLノード</param>
    ''' <param name="tagName">Tag名</param>
    ''' <returns>結果XML</returns>
    ''' <remarks></remarks>
    Private Function GetTagValue(ByVal resultXmlNode As XmlNode, _
                                 ByVal tagName As String, _
                                 ByVal tagCheckFlg As TagCheckType) As String

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

            If tagCheckFlg = TagCheckType.MandatoryTag Then
                '必須タグ
                'コードに-1を設定
                Return XmlErr
            Else
                Return String.Empty
            End If

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
    ''' Request XMLの店舗コードと販売店コードをDMSに転換
    ''' </summary>
    ''' <param name="inXmlClass">XML作成用クラス</param>
    ''' <returns>転換後XML</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Function Change2DMSXML(ByVal inXmlClass As CustomerSearchXmlDocumentClass) As CustomerSearchXmlDocumentClass

        '開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name))

        Using biz As New ServiceCommonClassBusinessLogic

            Dim dtResult As ServiceCommonClassDataSet.DmsCodeMapDataTable = biz.GetIcropToDmsCode(inXmlClass.Detail.Common.DealerCode, _
                                  ServiceCommonClassBusinessLogic.DmsCodeType.BranchCode, _
                                  inXmlClass.Detail.Common.DealerCode, _
                                  inXmlClass.Detail.Common.BranchCode, _
                                  String.Empty, _
                                  inXmlClass.Detail.Common.StaffCode)

            If dtResult.Count > 0 Then
                inXmlClass.Detail.Common.DealerCode = dtResult(0).CODE1
                inXmlClass.Detail.Common.BranchCode = dtResult(0).CODE2
                inXmlClass.Detail.Common.StaffCode = dtResult(0).ACCOUNT
            End If

        End Using

        '終了ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name))

        Return inXmlClass

    End Function


    ''' <summary>
    ''' タイムアウト設定値を取得する
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetTimeOutValues() As String

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_Start", _
                                  MethodBase.GetCurrentMethod.Name))

        '戻り値
        Dim retValue As String = String.Empty

        'エラー発生フラグ
        Dim errorFlg As Boolean = False


        Try
            Using smbCommonBiz As New ServiceCommonClassBusinessLogic

                '******************************
                '* システム設定から取得
                '******************************
                '基幹連携送信時タイムアウト値
                retValue = smbCommonBiz.GetSystemSettingValueBySettingName(SysLinkSendTimeOutVal)

                If String.IsNullOrEmpty(retValue) Then
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                               "{0}.Error ErrCode:{1}, LINK_SEND_TIMEOUT_VAL does not exist.", _
                                               MethodBase.GetCurrentMethod.Name, _
                                               ErrorSysEnv))
                    errorFlg = True
                    Exit Try
                End If

            End Using

        Finally

            If errorFlg Then
                retValue = String.Empty
            End If

        End Try

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_End", _
                                  MethodBase.GetCurrentMethod.Name))

        Return retValue

    End Function

#End Region

    ''' <summary>
    ''' IDisposable.Dispoase
    ''' </summary>
    ''' <remarks></remarks>
    Public Overloads Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
    Protected Overridable Overloads Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
        End If
    End Sub

End Class
