'-------------------------------------------------------------------------
'IC3800708BusinessLogic.vb
'-------------------------------------------------------------------------
'機能：顧客情報取得IF
'補足：
'作成：2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発
'更新：2015/06/05 TMEJ 小澤 TMT号口調査 XML解析中にエラーが発生した場合はログを出力するように修正
'更新：2020/01/29 NSK 今泉 TR-SVT-TKM-20191030-001 新車が販売店に来店した際にVIN番号を入力しても検索エラーが表示される
'更新：2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証
'─────────────────────────────────────
Imports System.Xml
Imports System.Net
Imports System.IO
Imports System.Globalization
Imports System.Reflection
Imports System.Xml.Serialization
Imports System.Text.RegularExpressions
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Text
Imports System.Web
Imports Toyota.eCRB.DMSLinkage.CustomerInfo.Api.DataAccess.IC3800708DataSet
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.BizLogic
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.DataAccess

Public Class IC3800708BusinessLogic
    Inherits BaseBusinessComponent
    Implements IDisposable


#Region "定数"

    ''' <summary>
    ''' 本機能のプログラムID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MY_PROGRAMID As String = "IC3800708"

    ''' <summary>
    ''' XMLの要素内の要素を取得する際の先頭に付ける値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const XmlRootDirectory As String = "//"

    ''' <summary>
    ''' 日付フォーマット
    ''' </summary>
    Private Const DateFormat = "dd/MM/yyyy HH:mm:ss"

#Region "システム設定名"

    ''' <summary>
    ''' 基幹連携送信タイムアウト値
    ''' </summary>
    Private Const SysLinkSendTimeOutVal = "LINK_SEND_TIMEOUT_VAL"

    ''' <summary>
    ''' 国コード
    ''' </summary>
    Private Const SysCountryCode = "DIST_CD"

#End Region

#Region "販売店システム設定名"

    ''' <summary>
    ''' 基幹連携URL（顧客詳細情報）
    ''' </summary>
    Private Const DlrSysLinkUrlCstDtlInfo = "LINK_URL_CST_DETAIL"

#End Region

#Region "送信XML関連"

    ''' <summary>
    ''' メッセージID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RequestCustomerDetailId As String = "IC3A09923"

    ''' <summary>
    ''' エンコード(UTF-8)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const EncodeUtf8 As Integer = 65001

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
    ''' WebService(IC3A09923)メソッド名
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WebServiceMethodName As String = "GetCustomerDetail"

    ''' <summary>
    ''' WebService(IC3A09923)引数名
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WebServiceArgumentName As String = "xsData="

#End Region

#Region "タグ名"

#Region "要求XML関連"

#Region "GetCustomerDetailノード"

    ''' <summary>
    ''' タグ名：GetCustomerDetail
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagGetCustomerDetail As String = "GetCustomerDetail"

#Region "headノード"

    ''' <summary>
    ''' タグ名：head
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagHead As String = "head"

    ''' <summary>
    ''' タグ名：MessageID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagMessageID As String = "MessageID"

    ''' <summary>
    ''' タグ名：CountryCode
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagCountryCode As String = "CountryCode"

    ''' <summary>
    ''' タグ名：LinkSystemCode
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagLinkSystemCode As String = "LinkSystemCode"

    ''' <summary>
    ''' タグ名：TransmissionDate
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagTransmissionDate As String = "TransmissionDate"

#End Region

#Region "Detailノード"

    ''' <summary>
    ''' タグ名：Detail
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagDetail As String = "Detail"

#Region "Commonノード"

    ''' <summary>
    ''' タグ名：Common
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagCommon As String = "Common"

    ''' <summary>
    ''' タグ名：DealerCode
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagDealerCode As String = "DealerCode"

    ''' <summary>
    ''' タグ名：BranchCode
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagBranchCode As String = "BranchCode"

    ''' <summary>
    ''' タグ名：StaffCode
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagStaffCode As String = "StaffCode"

#End Region

#Region "SearchConditionノード"

    ''' <summary>
    ''' タグ名：SearchCondition
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagSearchCondition As String = "SearchCondition"

    ''' <summary>
    ''' タグ名：CustomerCode
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagCustomerCode As String = "CustomerCode"

    '2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証 START
    ''' <summary>
    ''' タグ名：CustomerCode
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagStart As String = "Start"

    ''' <summary>
    ''' タグ名：CustomerCode
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagCount As String = "Count"
    '2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証 END

#End Region

#End Region

#End Region

#End Region

#Region "受信XML関連"

#Region "Resultノード"

    ''' <summary>
    ''' タグ名：Result
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagResult As String = "Result"

    ''' <summary>
    ''' タグ名：ResultCode
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagResultCode As String = "ResultCode"
    '2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証 START
    ''' <summary>
    ''' タグ名：ResultCode
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagAllCount As String = "AllCount"
    '2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証 END
#End Region

#Region "CustInfoノード"

    ''' <summary>
    ''' タグ名：CustInfo
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagCustInfo As String = "CustInfo"

    '※CustomerCodeタグは要求XMLと共通

    ''' <summary>
    ''' タグ名：SocialID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagSocialID As String = "SocialID"

    ''' <summary>
    ''' タグ名：Name1
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagName1 As String = "Name1"

    ''' <summary>
    ''' タグ名：Name2
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagName2 As String = "Name2"

    ''' <summary>
    ''' タグ名：Name3
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagName3 As String = "Name3"

    ''' <summary>
    ''' タグ名：Sex
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagSex As String = "Sex"

    ''' <summary>
    ''' タグ名：NameTitle
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagNameTitle As String = "NameTitle"

    ''' <summary>
    ''' タグ名：VIPFlg
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagVIPFlg As String = "VIPFlg"

    ''' <summary>
    ''' タグ名：CustomerType
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagCustomerType As String = "CustomerType"

    ''' <summary>
    ''' タグ名：SubCustomerType
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagSubCustomerType As String = "SubCustomerType"

    ''' <summary>
    ''' タグ名：CompanyName
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagCompanyName As String = "CompanyName"

    ''' <summary>
    ''' タグ名：EmployeeName
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagEmployeeName As String = "EmployeeName"

    ''' <summary>
    ''' タグ名：EmployeeDepartment
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagEmployeeDepartment As String = "EmployeeDepartment"

    ''' <summary>
    ''' タグ名：EmployeePosition
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagEmployeePosition As String = "EmployeePosition"

    ''' <summary>
    ''' タグ名：TelNumber
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagTelNumber As String = "TelNumber"

    ''' <summary>
    ''' タグ名：FaxNumber
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagFaxNumber As String = "FaxNumber"

    ''' <summary>
    ''' タグ名：Mobile
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagMobile As String = "Mobile"

    ''' <summary>
    ''' タグ名：EMail1
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagEMail1 As String = "EMail1"

    ''' <summary>
    ''' タグ名：EMail2
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagEMail2 As String = "EMail2"

    ''' <summary>
    ''' タグ名：BusinessTelNumber
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagBusinessTelNumber As String = "BusinessTelNumber"

    ''' <summary>
    ''' タグ名：Address1
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagAddress1 As String = "Address1"

    ''' <summary>
    ''' タグ名：Address2
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagAddress2 As String = "Address2"

    ''' <summary>
    ''' タグ名：Address3
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagAddress3 As String = "Address3"

    ''' <summary>
    ''' タグ名：Domicile
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagDomicile As String = "Domicile"

    ''' <summary>
    ''' タグ名：Country
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagCountry As String = "Country"

    ''' <summary>
    ''' タグ名：ZipCode
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagZipCode As String = "ZipCode"

    ''' <summary>
    ''' タグ名：StateCode
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagStateCode As String = "StateCode"

    ''' <summary>
    ''' タグ名：DistrictCode
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagDistrictCode As String = "DistrictCode"

    ''' <summary>
    ''' タグ名：CityCode
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagCityCode As String = "CityCode"

    ''' <summary>
    ''' タグ名：LocationCode
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagLocationCode As String = "LocationCode"

    ''' <summary>
    ''' タグ名：BirthDay
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagBirthDay As String = "BirthDay"

    ''' <summary>
    ''' タグ名：NewcustomerID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagNewcustomerID As String = "NewcustomerID"

    ''' <summary>
    ''' タグ名：VhcInfo
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagVhcInfo As String = "VhcInfo"

    ''' <summary>
    ''' タグ名：MakerCode
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagMakerCode As String = "MakerCode"

    ''' <summary>
    ''' タグ名：Vin
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagVin As String = "Vin"

    ''' <summary>
    ''' タグ名：VehicleRegistrationNumber
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagVehicleRegistrationNumber As String = "VehicleRegistrationNumber"

    ''' <summary>
    ''' タグ名：VehicleAreaCode
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagVehicleAreaCode As String = "VehicleAreaCode"

    ''' <summary>
    ''' タグ名：Grade
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagGrade As String = "Grade"

    ''' <summary>
    ''' タグ名：SERIESCD
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagSeriescd As String = "SERIESCD"

    ''' <summary>
    ''' タグ名：BaseType
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagBaseType As String = "BaseType"

    ''' <summary>
    ''' タグ名：ModelCode
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagModelCode As String = "ModelCode"

    ''' <summary>
    ''' タグ名：FuelDivision
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagFuelDivision As String = "FuelDivision"

    ''' <summary>
    ''' タグ名：BodyColorName
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagBodyColorName As String = "BodyColorName"

    ''' <summary>
    ''' タグ名：EngineNumber
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagEngineNumber As String = "EngineNumber"

    ''' <summary>
    ''' タグ名：Transmission
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagTransmission As String = "Transmission"

    ''' <summary>
    ''' タグ名：NewVehicleDivision
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagNewVehicleDivision As String = "NewVehicleDivision"

    ''' <summary>
    ''' タグ名：VehicleDeliveryDate
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagVehicleDeliveryDate As String = "VehicleDeliveryDate"

    ''' <summary>
    ''' タグ名：VehicleRegistrationDate
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagVehicleRegistrationDate As String = "VehicleRegistrationDate"

    ''' <summary>
    ''' タグ名：Mileage
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagMileage As String = "Mileage"

    ''' <summary>
    ''' タグ名：RegistDate
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagRegistDate As String = "RegistDate"

    ''' <summary>
    ''' タグ名：SalesStaffCode
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagSalesStaffCode As String = "SalesStaffCode"

    ''' <summary>
    ''' タグ名：ServiceAdviserCode
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagServiceAdviserCode As String = "ServiceAdviserCode"

    '※CompanyNameタグはCustInfoのCompanyName共通

    ''' <summary>
    ''' タグ名：InsNo
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagInsNo As String = "InsNo"

    ''' <summary>
    ''' タグ名：StartDate
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagStartDate As String = "StartDate"

    ''' <summary>
    ''' タグ名：EndDate
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagEndDate As String = "EndDate"

    ''' <summary>
    ''' タグ名：JDPFlg
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagJDPFlg As String = "JDPFlg"

#End Region

#End Region

#End Region

#End Region

#Region "エラーコード"

    ''' <summary>
    ''' システム設定不備エラー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ErrorSysEnv As Integer = 1121

    ''' <summary>
    ''' 販売店システム設定不備エラー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ErrorDlrEnv As Integer = 1150

#End Region

#Region "列挙体"

    ''' <summary>
    ''' IC3800708の返却コード列挙体
    ''' </summary>
    ''' <remarks>
    ''' CustomerDetailClassの
    ''' ResultCodeに設定される値。
    ''' </remarks>
    Public Enum Result As Integer

        ''' <summary>
        ''' 成功
        ''' </summary>
        ''' <remarks></remarks>
        Success = 0

        ''' <summary>
        ''' タイムアウトエラー
        ''' </summary>
        ''' <remarks>基幹側WebService呼出時</remarks>
        TimeOutError = 6001

        ''' <summary>
        ''' 基幹側のエラー
        ''' </summary>
        ''' <remarks></remarks>
        DmsError = 6002

        ''' <summary>
        ''' その他のエラー
        ''' </summary>
        ''' <remarks>基本的にiCROP側のエラー全般</remarks>
        OtherError = 6003
    End Enum

#End Region


#Region "Publicメソッド"
    '2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証 START

    ' ''' <summary>
    ' ''' 顧客詳細情報を取得する
    ' ''' </summary>
    ' ''' <param name="inDealerCode">販売店コード</param>
    ' ''' <param name="inBranchCode">店舗コード</param>
    ' ''' <param name="inDmsCstCode">基幹顧客コード</param>
    ' ''' <returns>CustomerDetailClass</returns>
    ' ''' <remarks>
    ' ''' 基幹側のWebServiceにリクエストし、返却されたXMLのデータを
    ' ''' DataTableに設定して返却する。
    ' ''' 戻り値にNothingが返却された場合、エラー発生。
    ' ''' XML解析中にエラーが発生した場合はログを出力するようにする。
    ' ''' </remarks>
    ' ''' <history>
    ' ''' 2015/06/05 TMEJ 小澤 TMT号口調査 XML解析中にエラーが発生した場合はログを出力するように修正
    ' ''' </history>
    'Public Function GetCustomerDtlinfo(ByVal inDealerCode As String, _
    '                                   ByVal inBranchCode As String, _
    '                                   ByVal inDmsCstCode As String) As CustomerDetailClass

    ''' <summary>
    ''' 顧客詳細情報を取得する
    ''' </summary>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inBranchCode">店舗コード</param>
    ''' <param name="inDmsCstCode">基幹顧客コード</param>
    ''' <param name="inVin">Vin</param>
    ''' <param name="inStart">開始位置</param>
    ''' <param name="inCount">指定台数</param>
    ''' <returns>CustomerDetailClass</returns>
    ''' <remarks>
    ''' 基幹側のWebServiceにリクエストし、返却されたXMLのデータを
    ''' DataTableに設定して返却する。
    ''' 戻り値にNothingが返却された場合、エラー発生。
    ''' XML解析中にエラーが発生した場合はログを出力するようにする。
    ''' </remarks>
    ''' <history>
    ''' 2015/06/05 TMEJ 小澤 TMT号口調査 XML解析中にエラーが発生した場合はログを出力するように修正
    ''' 2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証
    ''' </history>
    Public Function GetCustomerDtlinfo(ByVal inDealerCode As String,
                                       ByVal inBranchCode As String, _
                                       ByVal inDmsCstCode As String, _
                                       ByVal inVin As String, _
                                       Optional inStart As Long = -1, _
                                       Optional inCount As Long = -1) As CustomerDetailClass


        '2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証 END
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.Start IN:inDealerCode={1}, inBranchCode={2}, inDmsCstCode={3} ", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  inDealerCode, _
                                  inBranchCode, _
                                  inDmsCstCode))

        '返却用の顧客詳細クラス
        Dim rtCustomerDtlInfo As New CustomerDetailClass

        Try
            '引数チェック
            If String.IsNullOrEmpty(inDealerCode.Trim()) Then
                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                           "{0}.Error:inDealerCode is Null or Empty.", _
                                           MethodBase.GetCurrentMethod.Name))
                rtCustomerDtlInfo.ResultCode = Result.OtherError
                Exit Try

            End If

            If String.IsNullOrEmpty(inBranchCode.Trim()) Then
                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                           "{0}.Error:inBranchCode is Null or Empty.", _
                                           MethodBase.GetCurrentMethod.Name))
                rtCustomerDtlInfo.ResultCode = Result.OtherError
                Exit Try

            End If

            If String.IsNullOrEmpty(inDmsCstCode.Trim()) Then
                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                           "{0}.Error:inDmsCstCode is Null or Empty.", _
                                           MethodBase.GetCurrentMethod.Name))
                rtCustomerDtlInfo.ResultCode = Result.OtherError
                Exit Try

            End If


            '現在日時取得
            Dim nowDateTime As Date = DateTimeFunc.Now(inDealerCode)

            'システム設定値を取得
            Dim systemSettingsValueRow As IC3800708SystemSettingValueRow _
                = Me.GetSystemSettingValues()
            'システム設定値の取得でエラーがあった場合
            If IsNothing(systemSettingsValueRow) Then

                'その他のエラーで作成
                rtCustomerDtlInfo.ResultCode = Result.OtherError
                Exit Try

            End If

            '基幹販売店コード、店舗コードを取得
            Dim dmsDlrBrnTable As ServiceCommonClassDataSet.DmsCodeMapDataTable _
                = Me.GetDmsCode(inDealerCode, inBranchCode)
            '基幹販売店コード、店舗コードの取得でエラーがあった場合
            If IsNothing(dmsDlrBrnTable) Then

                'その他のエラーで作成
                rtCustomerDtlInfo.ResultCode = Result.OtherError
                Exit Try

            End If

            ' 2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証 START

            '送信XMLの作成
            'Dim sendXml As XmlDocument = Me.StructRequestCstDtlXml(systemSettingsValueRow, _
            '                                                            dmsDlrBrnTable.Item(0), _
            '                                                            inDmsCstCode, _
            '                                                            nowDateTime)

            Dim sendXml As XmlDocument = Me.StructRequestCstDtlXml(systemSettingsValueRow, _
                                                                         dmsDlrBrnTable.Item(0), _
                                                                         inDmsCstCode, _
                                                                         nowDateTime, _
                                                                         inVin, _
                                                                         inStart, _
                                                                         inCount)

            ' 2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証 END

            'WebServiceのURLを作成
            Dim createUrl As String = String.Concat(systemSettingsValueRow.LINK_URL_CST_DTL_INFO, _
                                                    "/", _
                                                    WebServiceMethodName)

            '送信XMLを引数に設定
            Dim sendXmlString As String = String.Concat(WebServiceArgumentName, _
                                                        sendXml.InnerXml)

            'WebService送受信処理
            Dim resultString As String = CallWebServiceSite(sendXmlString, _
                                                            createUrl, _
                                                            systemSettingsValueRow.LINK_SEND_TIMEOUT_VAL)


            If CType(Result.TimeOutError, String).Equals(resultString) _
            OrElse CType(Result.OtherError, String).Equals(resultString) Then
                '送受信処理でエラー発生時

                '該当エラーでエラーテーブル作成
                rtCustomerDtlInfo.ResultCode = CType(resultString, Long)
                Exit Try

            End If


            'XML名前空間用の正規表現設定
            Dim regex As Regex = New Regex(" xmln.*=""[^""]*"".")

            'XML名前空間を除去
            Dim editResultString As String = regex.Replace(resultString, Space(0))

            '受信XMLを解析し、顧客詳細情報を作成
            rtCustomerDtlInfo = Me.CreateCustomerDtlInfo(editResultString)

            '2015/06/05 TMEJ 小澤 TMT号口調査 XML解析中にエラーが発生した場合はログを出力するように修正 START

            '作成したDataTebleのエラーコードチェック
            If Not (IsNothing(rtCustomerDtlInfo)) AndAlso _
                rtCustomerDtlInfo.ResultCode <> Result.Success Then
                'データが存在している且つ、「0：成功」以外の場合
                'ログを出力する
                '送信XMLのログ出力
                Logger.Warn(String.Format(CultureInfo.InvariantCulture, _
                                           "{0} {1}.CreateCustomerDtlInfo Error : SendXml = {2}", _
                                           Me.GetType.ToString, _
                                           MethodBase.GetCurrentMethod.Name, _
                                           sendXml.InnerXml))
                '受信XMLのログ出力
                Logger.Warn(String.Format(CultureInfo.InvariantCulture, _
                                           "{0} {1}.CreateCustomerDtlInfo Error : ReceivedXML = {2}", _
                                           Me.GetType.ToString, _
                                           System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                           editResultString))

            End If

            '2015/06/05 TMEJ 小澤 TMT号口調査 XML解析中にエラーが発生した場合はログを出力するように修正 END

        Catch ex As Exception

            Logger.Error(String.Format(CultureInfo.InvariantCulture, _
                                       "{0}.Error ", _
                                       MethodBase.GetCurrentMethod.Name), ex)

            rtCustomerDtlInfo.ResultCode = Result.OtherError

        End Try

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_End", _
                                  MethodBase.GetCurrentMethod.Name))

        Return rtCustomerDtlInfo

    End Function

#End Region

#Region "Privateメソッド"

#Region "取得系"

    ''' <summary>
    ''' システム設定、販売店設定から必要な設定値を取得する
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetSystemSettingValues() As IC3800708SystemSettingValueRow

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_Start", _
                                  MethodBase.GetCurrentMethod.Name))

        '戻り値
        Dim retRow As IC3800708SystemSettingValueRow = Nothing

        'エラー発生フラグ
        Dim errorFlg As Boolean = False


        Try
            Using smbCommonBiz As New ServiceCommonClassBusinessLogic

                '******************************
                '* システム設定から取得
                '******************************
                '基幹連携送信時タイムアウト値
                Dim linkSendTimeoutVal As String _
                    = smbCommonBiz.GetSystemSettingValueBySettingName(SysLinkSendTimeOutVal)

                If String.IsNullOrEmpty(linkSendTimeoutVal) Then
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                               "{0}.Error ErrCode:{1}, LINK_SEND_TIMEOUT_VAL does not exist.", _
                                               MethodBase.GetCurrentMethod.Name, _
                                               ErrorSysEnv))
                    errorFlg = True
                    Exit Try
                End If

                '国コード
                Dim countryCode As String _
                    = smbCommonBiz.GetSystemSettingValueBySettingName(SysCountryCode)

                If String.IsNullOrEmpty(countryCode) Then
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                               "{0}.Error ErrCode:{1}, DIST_CD does not exist.", _
                                               MethodBase.GetCurrentMethod.Name, _
                                               ErrorSysEnv))
                    errorFlg = True
                    Exit Try
                End If


                '******************************
                '* 販売店システム設定から取得
                '******************************
                '送信先アドレス
                Dim linkUrlCstDtlInfo As String _
                    = smbCommonBiz.GetDlrSystemSettingValueBySettingName(DlrSysLinkUrlCstDtlInfo)

                If String.IsNullOrEmpty(linkUrlCstDtlInfo) Then
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                               "{0}.Error ErrCode:{1}, LINK_URL_CST_DTL_INFO does not exist.", _
                                               MethodBase.GetCurrentMethod.Name, _
                                               ErrorDlrEnv))
                    errorFlg = True
                    Exit Try
                End If

                Using table As New IC3800708SystemSettingValueDataTable

                    retRow = table.NewIC3800708SystemSettingValueRow

                    With retRow
                        '取得した値を戻り値のデータ行に設定
                        .LINK_SEND_TIMEOUT_VAL = linkSendTimeoutVal
                        .DIST_CD = countryCode
                        .DATE_FORMAT = DateFormat
                        .LINK_URL_CST_DTL_INFO = linkUrlCstDtlInfo
                    End With

                End Using

            End Using

        Finally

            If errorFlg Then
                retRow = Nothing
            End If

        End Try

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_End", _
                                  MethodBase.GetCurrentMethod.Name))

        Return retRow

    End Function

    ''' <summary>
    ''' 基幹販売店コード、基幹店舗コードの入ったDataTableを取得する
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <returns>SMBCommonClassDataSet.DmsCodeMapDataTable</returns>
    ''' <remarks></remarks>
    Private Function GetDmsCode(ByVal dealerCode As String, _
                                ByVal branchCode As String) As ServiceCommonClassDataSet.DmsCodeMapDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.Start IN:dealerCode={1}, branchCode={2} ", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  dealerCode, _
                                  branchCode))

        '返却用のデータテーブル
        Dim dmsDlrBrnTable As ServiceCommonClassDataSet.DmsCodeMapDataTable

        Using smbCommonBiz As New ServiceCommonClassBusinessLogic

            '**************************************************
            '* 基幹販売店コード、店舗コードを取得
            '**************************************************
            dmsDlrBrnTable = smbCommonBiz.GetIcropToDmsCode(dealerCode, _
                                                            ServiceCommonClassBusinessLogic.DmsCodeType.BranchCode, _
                                                            dealerCode, _
                                                            branchCode, _
                                                            String.Empty)

            If dmsDlrBrnTable.Count <= 0 Then

                'データが取得できない場合はエラー
                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                           "{0}.Error:Failed to convert key dealer code.(No data found)", _
                                           MethodBase.GetCurrentMethod.Name))
                dmsDlrBrnTable = Nothing

            ElseIf 1 < dmsDlrBrnTable.Count Then

                'データが2件以上取得できた場合は一意に決定できないためエラー
                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                           "{0}.Error:Failed to convert key dealer code.(Non-unique)", _
                                           MethodBase.GetCurrentMethod.Name))
                dmsDlrBrnTable = Nothing

            End If

        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_End", _
                                  MethodBase.GetCurrentMethod.Name))

        Return dmsDlrBrnTable

    End Function

    ''' <summary>
    ''' ノード内のタグ情報を取得する
    ''' </summary>
    ''' <param name="node">ノード</param>
    ''' <param name="tagNames">読み込みを行うタグ名の配列</param>
    ''' <returns>タグ名をキーとしたDictionary</returns>
    ''' <remarks></remarks>
    Private Function GetElementsData(ByVal node As XmlNode, _
                                     ByVal tagNames() As String) As Dictionary(Of String, String)

        Dim dictinary As New Dictionary(Of String, String)

        '指定タグ名分ループ
        For Each tagName As String In tagNames
            If 0 < node.SelectNodes(tagName).Count Then
                'タグあり
                dictinary.Add(tagName, node.SelectSingleNode(tagName).InnerText)
            Else
                'タグなし
                dictinary.Add(tagName, String.Empty)
            End If
        Next

        '処理結果返却
        Return dictinary

    End Function

    ''' <summary>
    ''' ノード内のタグ情報を取得する
    ''' </summary>
    ''' <param name="node">ノード</param>
    ''' <param name="tagNamesList">読み込みを行うタグ名のリスト</param>
    ''' <returns>タグ名をキーとしたDictionary</returns>
    ''' <remarks></remarks>
    Private Function GetElementsData(ByVal node As XmlNode, _
                                     ByVal tagNamesList As List(Of String)) As Dictionary(Of String, String)

        Dim dictinary As New Dictionary(Of String, String)

        '指定タグ名分ループ
        For Each tagName As String In tagNamesList
            If 0 < node.SelectNodes(tagName).Count Then
                'タグあり
                dictinary.Add(tagName, node.SelectSingleNode(tagName).InnerText)
            Else
                'タグなし
                dictinary.Add(tagName, String.Empty)
            End If
        Next

        '処理結果返却
        Return dictinary

    End Function

#End Region

#Region "XML作成"
    ' 2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証 START

    ' ''' <summary>
    ' ''' 顧客詳細要求用XMLを構築する
    ' ''' </summary>
    ' ''' <param name="sysValueRow">システム設定値データ行</param>
    ' ''' <param name="dmsDlrBrnCodeRow">基幹コードデータ行</param>
    ' ''' <param name="dmsCstCode">基幹顧客コード</param>
    ' ''' <param name="nowDateTime">現在日時</param>
    ' ''' <returns>構築したXMLドキュメント</returns>
    ' ''' <remarks></remarks>
    'Private Function StructRequestCstDtlXml(ByVal sysValueRow As IC3800708SystemSettingValueRow, _
    '                                             ByVal dmsDlrBrnCodeRow As ServiceCommonClassDataSet.DmsCodeMapRow, _
    '                                             ByVal dmsCstCode As String, _
    '                                             ByVal nowDateTime As Date) As XmlDocument

    ''' <summary>
    ''' 顧客詳細要求用XMLを構築する
    ''' </summary>
    ''' <param name="sysValueRow">システム設定値データ行</param>
    ''' <param name="dmsDlrBrnCodeRow">基幹コードデータ行</param>
    ''' <param name="dmsCstCode">基幹顧客コード</param>
    ''' <param name="nowDateTime">現在日時</param>
    ''' <param name="inVin">Vin</param>
    ''' <param name="inStart">開始位置</param>
    ''' <param name="inCount">指定台数</param>
    ''' <returns>構築したXMLドキュメント</returns>
    ''' <remarks></remarks>
    Private Function StructRequestCstDtlXml(ByVal sysValueRow As IC3800708SystemSettingValueRow, _
                                                 ByVal dmsDlrBrnCodeRow As ServiceCommonClassDataSet.DmsCodeMapRow, _
                                                 ByVal dmsCstCode As String, _
                                                 ByVal nowDateTime As Date, _
                                                 ByVal inVin As String, _
                                                 ByVal inStart As Long, _
                                                 ByVal inCount As Long) As XmlDocument

        ' 2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証 END

        '引数をログに出力
        Dim args As New List(Of String)

        'DataRow内の項目を列挙
        Me.AddLogData(args, sysValueRow)
        Me.AddLogData(args, dmsDlrBrnCodeRow)

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.Start IN:{1}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  String.Join(", ", args.ToArray())))

        '65001がUTF-8
        Dim xmlEncode As Encoding = Encoding.GetEncoding(EncodeUtf8)

        'XMLドキュメント作成
        Dim xmlDocument As New XmlDocument

        'ヘッダ部作成(<?xml version="1.0" encoding="utf-8"?>の部分)
        Dim xmlDeclaration As XmlDeclaration = xmlDocument.CreateXmlDeclaration("1.0", xmlEncode.BodyName, Nothing)

        'ルートタグ(GetCustomerDetailタグ)の作成
        Dim xmlRoot As XmlElement = xmlDocument.CreateElement(TagGetCustomerDetail)

        'headタグの構築
        Dim headTag As XmlElement = Me.StructHeadTag(xmlDocument, _
                                                     sysValueRow.DIST_CD, _
                                                     sysValueRow.DATE_FORMAT, _
                                                     nowDateTime)

        ' 2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証 START

        ''Detailタグの構築
        'Dim detailTag As XmlElement = Me.StructDetailTag(xmlDocument, _
        '                                                 dmsDlrBrnCodeRow.CODE1, _
        '                                                 dmsDlrBrnCodeRow.CODE2, _
        '                                                 dmsCstCode)

        'Detailタグの構築
        Dim detailTag As XmlElement = Me.StructDetailTag(xmlDocument, _
                                                         dmsDlrBrnCodeRow.CODE1, _
                                                         dmsDlrBrnCodeRow.CODE2, _
                                                         dmsCstCode, _
                                                         inVin, _
                                                         inStart, _
                                                         inCount)

        ' 2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証 END

        'GetCustomerDetailタグを構築
        xmlRoot.AppendChild(headTag)
        xmlRoot.AppendChild(detailTag)

        '送信用XMLを構築
        xmlDocument.AppendChild(xmlDeclaration)
        xmlDocument.AppendChild(xmlRoot)

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.End OUT:STRUCTXML = " & vbCrLf & "{1}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  Me.FormatXml(xmlDocument)))

        Return xmlDocument

    End Function

    ''' <summary>
    ''' 顧客詳細要求用XMLのheadタグを構築する
    ''' </summary>
    ''' <param name="xmlDocument">顧客詳細要求用XMLドキュメント</param>
    ''' <param name="countryCode">国コード</param>
    ''' <param name="dateFormat">日付フォーマット</param>
    ''' <param name="nowDateTime">現在日時</param>
    ''' <returns>headタグエレメント</returns>
    ''' <remarks></remarks>
    Private Function StructHeadTag(ByVal xmlDocument As XmlDocument, _
                                   ByVal countryCode As String, _
                                   ByVal dateFormat As String, _
                                   ByVal nowDateTime As Date) As XmlElement

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_Start IN:countryCode={1}, dateFormat={2}, nowDateTime={3}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  countryCode, _
                                  dateFormat, _
                                  nowDateTime))

        'headタグを作成
        Dim headTag As XmlElement = xmlDocument.CreateElement(TagHead)

        'headタグの子要素を作成
        Dim messageIdTag As XmlElement = xmlDocument.CreateElement(TagMessageID)
        Dim countryCodeTag As XmlElement = xmlDocument.CreateElement(TagCountryCode)
        Dim linkSystemCodeTag As XmlElement = xmlDocument.CreateElement(TagLinkSystemCode)
        Dim TransmissionDateTag As XmlElement = xmlDocument.CreateElement(TagTransmissionDate)

        '子要素に値を設定
        messageIdTag.AppendChild(xmlDocument.CreateTextNode(RequestCustomerDetailId))
        countryCodeTag.AppendChild(xmlDocument.CreateTextNode(countryCode))
        linkSystemCodeTag.AppendChild(xmlDocument.CreateTextNode("0"))
        TransmissionDateTag.AppendChild(xmlDocument.CreateTextNode(nowDateTime.ToString(dateFormat, CultureInfo.CurrentCulture)))

        'headタグを構築
        With headTag
            .AppendChild(messageIdTag)
            .AppendChild(countryCodeTag)
            .AppendChild(linkSystemCodeTag)
            .AppendChild(TransmissionDateTag)
        End With

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_End OUT:headTag={1}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  headTag.InnerXml))

        Return headTag

    End Function
    ' 2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証 START

    ' ''' <summary>
    ' ''' 顧客詳細要求用XMLのDetailタグを構築する
    ' ''' </summary>
    ' ''' <param name="xmlDocument">顧客詳細要求用XMLドキュメント</param>
    ' ''' <param name="dmsDealerCode">基幹販売店コード</param>
    ' ''' <param name="dmsBranchCode">基幹店舗コード</param>
    ' ''' <param name="dmsCstCode">基幹顧客コード</param>
    ' ''' <returns>Detailタグエレメント</returns>
    ' ''' <remarks></remarks>
    'Private Function StructDetailTag(ByVal xmlDocument As XmlDocument, _
    '                                 ByVal dmsDealerCode As String, _
    '                                 ByVal dmsBranchCode As String, _
    '                                 ByVal dmsCstCode As String) As XmlElement

    ''' <summary>
    ''' 顧客詳細要求用XMLのDetailタグを構築する
    ''' </summary>
    ''' <param name="xmlDocument">顧客詳細要求用XMLドキュメント</param>
    ''' <param name="dmsDealerCode">基幹販売店コード</param>
    ''' <param name="dmsBranchCode">基幹店舗コード</param>
    ''' <param name="dmsCstCode">基幹顧客コード</param>
    ''' <param name="inVin">Vin</param>
    ''' <param name="inStart">開始位置</param>
    ''' <param name="inCount">指定台数</param>
    ''' <returns>Detailタグエレメント</returns>
    ''' <remarks></remarks>
    Private Function StructDetailTag(ByVal xmlDocument As XmlDocument, _
                                     ByVal dmsDealerCode As String, _
                                     ByVal dmsBranchCode As String, _
                                     ByVal dmsCstCode As String, _
                                     ByVal inVin As String, _
                                     ByVal inStart As Long, _
                                     ByVal inCount As Long) As XmlElement

        ' 2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証 END

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_Start IN:dmsDealerCode={1}, dmsBranchCode={2}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  dmsDealerCode, _
                                  dmsBranchCode))

        'Detailタグを作成
        Dim detailTag As XmlElement = xmlDocument.CreateElement(TagDetail)

        'Commonタグを構築
        Dim commonTag As XmlElement = Me.StructCommonTag(xmlDocument, _
                                                         dmsDealerCode, _
                                                         dmsBranchCode)

        'DetailタグにCommonタグを子要素として追加
        detailTag.AppendChild(commonTag)

        ' 2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証 START

        ''SearchConditionTagタグを構築
        'Dim cstSearchConditionTag As XmlElement = Me.StructCstSearchConditionTag(xmlDocument, _
        '                                                                                 dmsCstCode)

        'SearchConditionTagタグを構築
        Dim cstSearchConditionTag As XmlElement = Me.StructCstSearchConditionTag(xmlDocument, _
                                                                                         dmsCstCode, _
                                                                                         inVin, _
                                                                                         inStart, _
                                                                                         inCount)

        ' 2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証 END

        'DetailタグにSearchConditionTagタグを子要素として追加
        detailTag.AppendChild(cstSearchConditionTag)

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_End OUT:detailTag={1}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  detailTag.InnerXml))

        Return detailTag

    End Function

    ''' <summary>
    ''' 顧客詳細要求用XMLのCommonタグを構築する
    ''' </summary>
    ''' <param name="xmlDocument">顧客詳細要求用XMLドキュメント</param>
    ''' <param name="dmsDealerCode">基幹販売店コード</param>
    ''' <param name="dmsBranchCode">基幹店舗コード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function StructCommonTag(ByVal xmlDocument As XmlDocument, _
                                     ByVal dmsDealerCode As String, _
                                     ByVal dmsBranchCode As String) As XmlElement

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_Start IN:dmsDealerCode={1}, dmsBranchCode={2}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  dmsDealerCode, _
                                  dmsBranchCode))

        'ログインスタッフ情報取得
        Dim userContext As StaffContext = StaffContext.Current
        Dim staffCode As String = userContext.Account.Split(CChar("@"))(0)

        'Commonタグを作成
        Dim commonTag As XmlElement = xmlDocument.CreateElement(TagCommon)

        'Commonタグの子要素を作成
        Dim dealerCodeTag As XmlElement = xmlDocument.CreateElement(TagDealerCode)
        Dim branchCodeTag As XmlElement = xmlDocument.CreateElement(TagBranchCode)
        Dim staffCodeTag As XmlElement = xmlDocument.CreateElement(TagStaffCode)

        '子要素に値を設定
        dealerCodeTag.AppendChild(xmlDocument.CreateTextNode(dmsDealerCode))
        branchCodeTag.AppendChild(xmlDocument.CreateTextNode(dmsBranchCode))
        staffCodeTag.AppendChild(xmlDocument.CreateTextNode(staffCode))

        'Commonタグの子要素を追加
        With commonTag
            .AppendChild(dealerCodeTag)
            .AppendChild(branchCodeTag)
            .AppendChild(staffCodeTag)
        End With

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_End OUT:commonTag={1}", _
                                  MethodBase.GetCurrentMethod.Name, commonTag.InnerXml))

        Return commonTag

    End Function
    ' 2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証 START

    ' ''' <summary>
    ' ''' 顧客詳細要求用XMLのSearchConditionタグを構築する
    ' ''' </summary>
    ' ''' <param name="xmlDocument">顧客詳細要求用XMLドキュメント</param>
    ' ''' <param name="dmsCstCode">基幹顧客コード</param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Private Function StructCstSearchConditionTag(ByVal xmlDocument As XmlDocument, _
    '                                               ByVal dmsCstCode As String) As XmlElement

    ''' <summary>
    ''' 顧客詳細要求用XMLのSearchConditionタグを構築する
    ''' </summary>
    ''' <param name="xmlDocument">顧客詳細要求用XMLドキュメント</param>
    ''' <param name="dmsCstCode">基幹顧客コード</param>
    ''' <param name="inVin">Vin</param>
    ''' <param name="inStart">開始位置</param>
    ''' <param name="inCount">指定台数</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function StructCstSearchConditionTag(ByVal xmlDocument As XmlDocument, _
                                                   ByVal dmsCstCode As String, _
                                                   ByVal inVin As String, _
                                                   ByVal inStart As Long, _
                                                   ByVal inCount As Long) As XmlElement

        ' 2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証 END

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_Start ", _
                                  MethodBase.GetCurrentMethod.Name))

        'SearchConditionタグを作成
        Dim searchConditionTag As XmlElement = xmlDocument.CreateElement(TagSearchCondition)


        'SearchConditionタグの子要素を作成
        Dim customerCodeTag As XmlElement = xmlDocument.CreateElement(TagCustomerCode)

        '子要素に値を設定
        'CustomerCode
        customerCodeTag.AppendChild(xmlDocument.CreateTextNode(dmsCstCode))

        'SearchConditionタグの子要素を追加
        ' 2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証 START
        'With searchConditionTag
        '    .AppendChild(customerCodeTag)
        'End With

        With searchConditionTag
            .AppendChild(customerCodeTag)
        End With

        'Start
        If 0 <= inStart Then
            Dim startTag As XmlElement = xmlDocument.CreateElement(TagStart)
            startTag.AppendChild(xmlDocument.CreateTextNode(CStr(inStart)))
            With searchConditionTag
                .AppendChild(startTag)
            End With
        End If

        'Count
        If 0 < inCount Then
            Dim countTag As XmlElement = xmlDocument.CreateElement(TagCount)
            countTag.AppendChild(xmlDocument.CreateTextNode(CStr(inCount)))
            With searchConditionTag
                .AppendChild(countTag)
            End With
        End If

        'Vin
        If Not String.IsNullOrWhiteSpace(inVin) Then
            Dim vinTag As XmlElement = xmlDocument.CreateElement(TagVin)
            vinTag.AppendChild(xmlDocument.CreateTextNode(inVin))
            With searchConditionTag
                .AppendChild(vinTag)
            End With
        End If
        ' 2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証 END

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_End OUT:SearchConditionTag={1}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  searchConditionTag.InnerXml))

        Return searchConditionTag

    End Function

#End Region

#Region "XML送受信"

    ''' <summary>
    ''' WebServiceのサイトを呼出
    ''' WebServiceを送信し結果を受信する
    ''' </summary>
    ''' <param name="sendXml">送信XML文字列</param>
    ''' <param name="webServiceUrl">送信先URL</param>
    ''' <param name="timeOutValue">タイムアウト設定値</param>
    ''' <returns>結果XML</returns>
    ''' <remarks></remarks>
    Private Function CallWebServiceSite(ByVal sendXml As String, _
                                        ByVal webServiceUrl As String, _
                                        ByVal timeOutValue As String) As String

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.Start IN:sendXml={1}, webServiceUrl={2} ", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  sendXml, _
                                  webServiceUrl))

        '文字コードを指定する
        Dim enc As System.Text.Encoding = _
            System.Text.Encoding.GetEncoding(EncodeUtf8)

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

        '送信タイムアウト値設定
        req.Timeout = CType(timeOutValue, Integer)

        'データをPOST送信するためのStreamを取得
        Using reqStream As Stream = req.GetRequestStream()

            '送信するデータを書き込む
            reqStream.Write(postDataBytes, 0, postDataBytes.Length)

        End Using

        '返却文字列(応答XML文字列をHTMLデコード)
        Dim decodeString As String = String.Empty

        Try
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

            '2020/01/29 NSK 今泉 TR-SVT-TKM-20191030-001 新車が販売店に来店した際にVIN番号を入力しても検索エラーが表示される START
            ''返却文字列をHTMLデコードする
            'decodeString = HttpUtility.HtmlDecode(resultString)
            'responseのXMLをデコードしないように修正
            decodeString = resultString
            '2020/01/29 NSK 今泉 TR-SVT-TKM-20191030-001 新車が販売店に来店した際にVIN番号を入力しても検索エラーが表示される END

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}.End OUT:resultString={1} ", _
                                      MethodBase.GetCurrentMethod.Name, _
                                      decodeString))


        Catch webEx As WebException
            If webEx.Status = WebExceptionStatus.Timeout Then
                'タイムアウトが発生した場合
                Logger.Error(String.Format(CultureInfo.InvariantCulture, _
                                           "{0}_Error ErrorCode:{1}, Timeout error occurred.", _
                                           MethodBase.GetCurrentMethod.Name, _
                                           CType(Result.TimeOutError, String)), webEx)

                decodeString = CType(Result.TimeOutError, String)
            Else
                'それ以外のネットワークエラー
                Logger.Error(String.Format(CultureInfo.InvariantCulture, _
                                           "{0}_Error ErrorCode:{1}", _
                                           MethodBase.GetCurrentMethod.Name, _
                                           CType(Result.OtherError, String)), webEx)

                decodeString = CType(Result.OtherError, String)
            End If

            '返却文字列を返却
            Return decodeString
        End Try

        '返却文字列をHTMLデコードして返却
        Return decodeString

    End Function

#End Region

#Region "XML解析"

    ''' <summary>
    ''' 返却用の顧客詳細クラスを作成する
    ''' </summary>
    ''' <param name="resultXml">受信XML文字列</param>
    ''' <returns>顧客詳細クラス</returns>
    ''' <remarks></remarks>
    Private Function CreateCustomerDtlInfo(ByVal resultXml As String) As CustomerDetailClass

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.Start IN:resultXml={1} ", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  resultXml))

        '返却用顧客詳細クラスのインスタンス生成
        Dim clCstDtlInfo As New CustomerDetailClass

        Try

            'XmlDocument
            Dim resultXmlDocument As New XmlDocument

            '返却された文字列をXML化
            resultXmlDocument.LoadXml(resultXml)

            'Resultノードを取得
            Dim resultNode As XmlNode _
                = resultXmlDocument.SelectSingleNode(XmlRootDirectory & TagResult)

            'ResultCodeの取得
            Dim resultCodeDictionary As Dictionary(Of String, String) _
                    = Me.GetElementsData(resultNode, {TagResultCode})

            Dim resultCode As String = resultCodeDictionary.Item(TagResultCode)

            '2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証 START
            'AllCountの取得
            Dim AllCountDictionary As Dictionary(Of String, String) _
                     = Me.GetElementsData(resultNode, {TagAllCount})

            Dim allCount As String = AllCountDictionary.Item(TagAllCount)

            ' 2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証 END
            'ResultCodeの値が0以外の場合
            If Not resultCode.Equals("0") Then
                Logger.Warn(String.Format(CultureInfo.InvariantCulture, _
                                           "{0}.Error OUT:ResultCode={1} ", _
                                           MethodBase.GetCurrentMethod.Name, _
                                           resultCode))
                clCstDtlInfo.ResultCode = Result.DmsError
                Return clCstDtlInfo
            End If

            'CustInfoタグの子要素を取得する
            Dim custInfo As XmlNode = _
                resultXmlDocument.SelectSingleNode(String.Concat(XmlRootDirectory, TagCustInfo))

            If IsNothing(custInfo) Then
                Logger.Warn(String.Format(CultureInfo.InvariantCulture, _
                           "{0}.Error OUT:ResultCode={1} ", _
                           MethodBase.GetCurrentMethod.Name, _
                           Result.DmsError))
                clCstDtlInfo.ResultCode = Result.DmsError
                Return clCstDtlInfo
            End If

            'CustInfoタグの子要素設定値を取得する
            Dim tagCustInfoList As List(Of String) = Me.CreateListofCustInfoTag()
            Dim custInfoDictionary As Dictionary(Of String, String) _
                    = Me.GetElementsData(custInfo, tagCustInfoList)

            Dim CustomerCode As String = custInfoDictionary.Item(TagCustomerCode)
            Dim Name1 As String = custInfoDictionary.Item(TagName1)

            '必須チェック
            If Not CheckMandatoryCustInfoTag(CustomerCode, Name1) Then
                clCstDtlInfo.ResultCode = Result.DmsError
                Return clCstDtlInfo
            End If

            'VhcInfoタグの子要素を取得する
            Dim vhcInfoXmlNodeList As XmlNodeList = custInfo.SelectNodes(XmlRootDirectory & TagVhcInfo)

            '顧客車両データテーブルを作成
            Dim dtVhcInfo As IC3800708CustomerVhcInfoDataTable = _
                Me.CreateVhcInfoDataTable(vhcInfoXmlNodeList)

            If IsNothing(dtVhcInfo) Then
                Logger.Warn(String.Format(CultureInfo.InvariantCulture, _
                                           "{0}.Error ErrorCode:{1}, vhcInfo is not set.", _
                                           MethodBase.GetCurrentMethod.Name, _
                                           CType(Result.DmsError, String)))
                clCstDtlInfo.ResultCode = Result.DmsError
                Return clCstDtlInfo
            End If

            '2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証 START
            '返却データを作成する
            'clCstDtlInfo = SetCustInfo(custInfoDictionary, dtVhcInfo)

            clCstDtlInfo = SetCustInfo(custInfoDictionary, dtVhcInfo, allCount)
            '2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証 END

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}_End", _
                                      MethodBase.GetCurrentMethod.Name))

            Return clCstDtlInfo
        Finally
            clCstDtlInfo = Nothing
        End Try

    End Function

    ''' <summary>
    ''' CustInfo全てのタグをリスト化
    ''' </summary>
    ''' <returns>CustInfoタグのリスト</returns>
    ''' <remarks></remarks>
    Private Function CreateListofCustInfoTag() As List(Of String)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                          "{0}.Start ", _
                          MethodBase.GetCurrentMethod.Name))
        Dim tagCustInfoList As New List(Of String)
        With tagCustInfoList
            .Add(TagCustomerCode)
            .Add(TagSocialID)
            .Add(TagName1)
            .Add(TagName2)
            .Add(TagName3)
            .Add(TagSex)
            .Add(TagNameTitle)
            .Add(TagVIPFlg)
            .Add(TagCustomerType)
            .Add(TagSubCustomerType)
            .Add(TagCompanyName)
            .Add(TagEmployeeName)
            .Add(TagEmployeeDepartment)
            .Add(TagEmployeePosition)
            .Add(TagTelNumber)
            .Add(TagFaxNumber)
            .Add(TagMobile)
            .Add(TagEMail1)
            .Add(TagEMail2)
            .Add(TagBusinessTelNumber)
            .Add(TagAddress1)
            .Add(TagAddress2)
            .Add(TagAddress3)
            .Add(TagDomicile)
            .Add(TagCountry)
            .Add(TagZipCode)
            .Add(TagStateCode)
            .Add(TagDistrictCode)
            .Add(TagCityCode)
            .Add(TagLocationCode)
            .Add(TagBirthDay)
            .Add(TagNewcustomerID)
        End With

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                  "{0}_End, OUT:{1}", _
                  MethodBase.GetCurrentMethod.Name, _
                  String.Join(", ", tagCustInfoList.ToArray())))
        Return tagCustInfoList
    End Function

    ''' <summary>
    ''' VhcInfo全てのタグをリスト化
    ''' </summary>
    ''' <returns>VhcInfoタグのリスト</returns>
    ''' <remarks></remarks>
    Private Function CreateListofVhcInfoTag() As List(Of String)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                          "{0}.Start ", _
                          MethodBase.GetCurrentMethod.Name))
        Dim tagVhcInfoList As New List(Of String)
        With tagVhcInfoList
            .Add(TagMakerCode)
            .Add(TagVin)
            .Add(TagVehicleRegistrationNumber)
            .Add(TagVehicleAreaCode)
            .Add(TagGrade)
            .Add(TagSeriescd)
            .Add(TagBaseType)
            .Add(TagModelCode)
            .Add(TagFuelDivision)
            .Add(TagBodyColorName)
            .Add(TagEngineNumber)
            .Add(TagTransmission)
            .Add(TagNewVehicleDivision)
            .Add(TagVehicleDeliveryDate)
            .Add(TagVehicleRegistrationDate)
            .Add(TagMileage)
            .Add(TagRegistDate)
            .Add(TagSalesStaffCode)
            .Add(TagServiceAdviserCode)
            .Add(TagCompanyName)
            .Add(TagInsNo)
            .Add(TagStartDate)
            .Add(TagEndDate)
            .Add(TagJDPFlg)
        End With

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                          "{0}_End, OUT:{1}", _
                          MethodBase.GetCurrentMethod.Name, _
                          String.Join(", ", tagVhcInfoList.ToArray())))
        Return tagVhcInfoList
    End Function
    '2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証 START

    ' ''' <summary>
    ' ''' 顧客詳細データをクラス化
    ' ''' </summary>
    ' ''' <param name="custDtlData">顧客詳細データ</param>
    ' ''' <param name="drVhcInfos">顧客車両データ行(複数可能)</param>
    ' ''' <returns>顧客詳細クラス(データ入り)</returns>
    ' ''' <remarks></remarks>
    'Private Function SetCustInfo(ByVal custDtlData As Dictionary(Of String, String), _
    '                                 ByVal drVhcInfos As IC3800708CustomerVhcInfoDataTable) As CustomerDetailClass

    ''' <summary>
    ''' 顧客詳細データをクラス化
    ''' </summary>
    ''' <param name="custDtlData">顧客詳細データ</param>
    ''' <param name="drVhcInfos">顧客車両データ行(複数可能)</param>
    ''' <param name="allCount">顧客車両件数</param>
    ''' <returns>顧客詳細クラス(データ入り)</returns>
    ''' <remarks></remarks>
    Private Function SetCustInfo(ByVal custDtlData As Dictionary(Of String, String), _
                                     ByVal drVhcInfos As IC3800708CustomerVhcInfoDataTable, _
                                     ByVal allCount As String) As CustomerDetailClass

        '2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証 END
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                  "{0}.Start ", _
                  MethodBase.GetCurrentMethod.Name))

        Dim rtClCustDtlInfo As New CustomerDetailClass

        With rtClCustDtlInfo
            .ResultCode = Result.Success
            .CustomerCode = custDtlData.Item(TagCustomerCode)
            .SocialId = custDtlData.Item(TagSocialID)
            .Name1 = custDtlData.Item(TagName1)
            .Name2 = custDtlData.Item(TagName2)
            .Name3 = custDtlData.Item(TagName3)
            .Sex = custDtlData.Item(TagSex)
            .NameTitle = custDtlData.Item(TagNameTitle)
            .VipFlg = custDtlData.Item(TagVIPFlg)
            .CustomerType = custDtlData.Item(TagCustomerType)
            .SubCustomerType = custDtlData.Item(TagSubCustomerType)
            .CompanyName = custDtlData.Item(TagCompanyName)
            .EmployeeName = custDtlData.Item(TagEmployeeName)
            .EmployeeDepartment = custDtlData.Item(TagEmployeeDepartment)
            .EmployeePosition = custDtlData.Item(TagEmployeePosition)
            .TelNumber = custDtlData.Item(TagTelNumber)
            .FaxNumber = custDtlData.Item(TagFaxNumber)
            .Mobile = custDtlData.Item(TagMobile)
            .EMail1 = custDtlData.Item(TagEMail1)
            .EMail2 = custDtlData.Item(TagEMail2)
            .BusinessTelNumber = custDtlData.Item(TagBusinessTelNumber)
            .Address1 = custDtlData.Item(TagAddress1)
            .Address2 = custDtlData.Item(TagAddress2)
            .Address3 = custDtlData.Item(TagAddress3)
            .Domicile = custDtlData.Item(TagDomicile)
            .Country = custDtlData.Item(TagCountry)
            .ZipCode = custDtlData.Item(TagZipCode)
            .StateCode = custDtlData.Item(TagStateCode)
            .DistrictCode = custDtlData.Item(TagDistrictCode)
            .CityCode = custDtlData.Item(TagCityCode)
            .LocationCode = custDtlData.Item(TagLocationCode)
            .BirthDay = custDtlData.Item(TagBirthDay)
            .NewcustomerId = custDtlData.Item(TagNewcustomerID)
            '2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証 START
            .AllCount = allCount
            '2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証 START
            .VhcInfo = drVhcInfos
        End With

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                          "{0}_End", _
                          MethodBase.GetCurrentMethod.Name))
        Return rtClCustDtlInfo
    End Function

    ''' <summary>
    ''' 顧客車両データテーブルを作成
    ''' </summary>
    ''' <param name="vhcInfoXmlNodeList">顧客車両XmlNodeList</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CreateVhcInfoDataTable(ByVal vhcInfoXmlNodeList As XmlNodeList) As IC3800708CustomerVhcInfoDataTable
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
          "{0}.Start ", _
          MethodBase.GetCurrentMethod.Name))

        Using rtDtVhcInfo As New IC3800708CustomerVhcInfoDataTable
            Dim tagVhcInfoList As List(Of String) = Me.CreateListofVhcInfoTag()

            For Each vhcInfoXmlNode As XmlNode In vhcInfoXmlNodeList
                'VhcInfoタグの子要素設定値を取得する
                Dim vhcInfoDictinary As Dictionary(Of String, String) _
                        = Me.GetElementsData(vhcInfoXmlNode, tagVhcInfoList)

                '必須項目チェック
                If String.IsNullOrEmpty(vhcInfoDictinary.Item(TagVin)) AndAlso _
                    String.IsNullOrEmpty(vhcInfoDictinary.Item(TagVehicleRegistrationNumber)) Then

                    '車両登録番号・VIN両方が入力されないため、エラー
                    Logger.Warn(String.Format(CultureInfo.InvariantCulture, _
                                               "{0}.Error ErrorCode:{1}, Vin and VehicleRegistrationNumber Value is not set.", _
                                               MethodBase.GetCurrentMethod.Name, _
                                               CType(Result.DmsError, String)))
                    Return Nothing
                End If

                '顧客車両データ行を作成
                Dim drVhcInfo As IC3800708CustomerVhcInfoRow = rtDtVhcInfo.NewIC3800708CustomerVhcInfoRow
                With drVhcInfo
                    .MakerCode = vhcInfoDictinary.Item(TagMakerCode)
                    .Vin = vhcInfoDictinary.Item(TagVin)
                    .VehicleRegistrationNumber = vhcInfoDictinary.Item(TagVehicleRegistrationNumber)
                    .VehicleAreaCode = vhcInfoDictinary.Item(TagVehicleAreaCode)
                    .Grade = vhcInfoDictinary.Item(TagGrade)
                    .SERIESCD = vhcInfoDictinary.Item(TagSeriescd)
                    .BaseType = vhcInfoDictinary.Item(TagBaseType)
                    .ModelCode = vhcInfoDictinary.Item(TagModelCode)
                    .FuelDivision = vhcInfoDictinary.Item(TagFuelDivision)
                    .BodyColorName = vhcInfoDictinary.Item(TagBodyColorName)
                    .EngineNumber = vhcInfoDictinary.Item(TagEngineNumber)
                    .Transmission = vhcInfoDictinary.Item(TagTransmission)
                    .NewVehicleDivision = vhcInfoDictinary.Item(TagNewVehicleDivision)
                    .VehicleDeliveryDate = vhcInfoDictinary.Item(TagVehicleDeliveryDate)
                    .VehicleRegistrationDate = vhcInfoDictinary.Item(TagVehicleRegistrationDate)
                    .Mileage = vhcInfoDictinary.Item(TagMileage)
                    .RegistDate = vhcInfoDictinary.Item(TagRegistDate)
                    .SalesStaffCode = vhcInfoDictinary.Item(TagSalesStaffCode)
                    .ServiceAdviserCode = vhcInfoDictinary.Item(TagServiceAdviserCode)
                    .CompanyName = vhcInfoDictinary.Item(TagCompanyName)
                    .InsNo = vhcInfoDictinary.Item(TagInsNo)
                    .StartDate = vhcInfoDictinary.Item(TagStartDate)
                    .EndDate = vhcInfoDictinary.Item(TagEndDate)
                    .JDPFlg = vhcInfoDictinary.Item(TagJDPFlg)
                End With

                '顧客車両データテーブルにデータ行を追加
                rtDtVhcInfo.AddIC3800708CustomerVhcInfoRow(drVhcInfo)
            Next

            If Not IsNothing(rtDtVhcInfo) AndAlso 0 < rtDtVhcInfo.Rows.Count Then
                '作成した顧客車両データテーブルが1行でもあればログに内容出力
                Me.OutPutDataTableLog(rtDtVhcInfo, MethodBase.GetCurrentMethod.Name)
            End If
            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                          "{0}_End", _
                          MethodBase.GetCurrentMethod.Name))

            Return rtDtVhcInfo
        End Using
    End Function

    ''' <summary>
    ''' 返却XMLの内CustInfo必須チェックを行う
    ''' </summary>
    ''' <param name="customerCode">顧客コード</param>
    ''' <param name="Name1">顧客の個人名</param>
    ''' <returns>チェックOK：True/チェックNG：False</returns>
    ''' <remarks></remarks>
    Private Function CheckMandatoryCustInfoTag(ByVal customerCode As String, _
                                                  ByVal Name1 As String) As Boolean

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.Start IN:customerCode:{1}, Name1:{2}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  customerCode, _
                                  Name1))

        Dim retCheckOkFlg As Boolean = True

        If String.IsNullOrEmpty(customerCode) Then
            '顧客コードが存在しないため、エラー
            Logger.Warn(String.Format(CultureInfo.InvariantCulture, _
                                       "{0}.Error ErrorCode:{1}, CustomerCode Value is not set.", _
                                       MethodBase.GetCurrentMethod.Name, _
                                       CType(Result.DmsError, String)))
            retCheckOkFlg = False
        End If

        If String.IsNullOrEmpty(Name1) Then
            '顧客の個人名が存在しないため、エラー
            Logger.Warn(String.Format(CultureInfo.InvariantCulture, _
                                       "{0}.Error ErrorCode:{1}, Name1 Value is not set.", _
                                       MethodBase.GetCurrentMethod.Name, _
                                       CType(Result.DmsError, String)))
            retCheckOkFlg = False
        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.End OUT:retCheckOkFlg:{1}", _
                                  MethodBase.GetCurrentMethod.Name, retCheckOkFlg))

        Return retCheckOkFlg

    End Function

#End Region

#Region "ログ出力用"

    ''' <summary>
    ''' DataRow内の項目を列挙(ログ出力用)
    ''' </summary>
    ''' <param name="args">ログ項目のコレクション</param>
    ''' <param name="row">対象となるDataRow</param>
    ''' <remarks></remarks>
    Private Sub AddLogData(ByVal args As List(Of String), ByVal row As DataRow)
        For Each column As DataColumn In row.Table.Columns
            If row.IsNull(column.ColumnName) Then
                args.Add(String.Format(CultureInfo.CurrentCulture, "{0} = NULL", column.ColumnName))
            Else
                args.Add(String.Format(CultureInfo.CurrentCulture, "{0} = {1}", column.ColumnName, row(column.ColumnName)))
            End If
        Next
    End Sub

    ''' <summary>
    ''' ログ出力(DataTable用)
    ''' </summary>
    ''' <param name="dt">戻り値(DataTable)</param>
    ''' <param name="methodName">メソッド名</param>
    ''' <remarks></remarks>
    Private Sub OutPutDataTableLog(ByVal dt As DataTable, ByVal methodName As String)

        If dt Is Nothing Then
            Return
        End If

        Logger.Info(MY_PROGRAMID & Space(1) & methodName & _
                    " LOG START " & " OutPutCount: " & _
                    (dt.Rows.Count).ToString(CultureInfo.InvariantCulture))

        Dim log As New Text.StringBuilder

        For j = 0 To dt.Rows.Count - 1

            log = New Text.StringBuilder()
            Dim dr As DataRow = dt.Rows(j)

            log.Append("RowNum: " + (j + 1).ToString(CultureInfo.InvariantCulture) + " -- ")

            For i = 0 To dt.Columns.Count - 1
                log.Append(dt.Columns(i).Caption)
                If IsDBNull(dr(i)) Then
                    log.Append(" IS NULL")
                Else
                    log.Append(" = ")
                    log.Append(dr(i).ToString)
                End If

                If i <= dt.Columns.Count - 2 Then
                    log.Append(", ")
                End If
            Next

            Logger.Info(log.ToString)
        Next

        Logger.Info(MY_PROGRAMID & Space(1) & methodName & " LOG END ")

    End Sub

    ''' <summary>
    ''' XMLをインデントを付加して整形する(ログ出力用)
    ''' </summary>
    ''' <param name="xmlDoc">XMLドキュメント</param>
    ''' <returns>整形後XML文字列</returns>
    ''' <remarks></remarks>
    Private Function FormatXml(ByVal xmlDoc As XmlDocument) As String

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_Start", _
                                  MethodBase.GetCurrentMethod.Name))

        Using textWriter As New StringWriter(CultureInfo.InvariantCulture)

            Dim xmlWriter As XmlTextWriter

            Try
                xmlWriter = New XmlTextWriter(textWriter)

                'インデントを2でフォーマット
                xmlWriter.Formatting = Formatting.Indented
                xmlWriter.Indentation = 2

                'XmlTextWriterにXMLを出力
                xmlDoc.WriteTo(xmlWriter)

                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                          "{0}_End", _
                                          MethodBase.GetCurrentMethod.Name))

                Return textWriter.ToString()

            Finally
                xmlWriter = Nothing
            End Try

        End Using

    End Function

#End Region

#Region "Publicクラス"

    ''' <summary>
    ''' 作業詳細
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CustomerDetailClass
        Public Property ResultCode As Long
        Public Property CustomerCode As String
        Public Property SocialId As String
        Public Property Name1 As String
        Public Property Name2 As String
        Public Property Name3 As String
        Public Property Sex As String
        Public Property NameTitle As String
        Public Property VipFlg As String
        Public Property CustomerType As String
        Public Property SubCustomerType As String
        Public Property CompanyName As String
        Public Property EmployeeName As String
        Public Property EmployeeDepartment As String
        Public Property EmployeePosition As String
        Public Property TelNumber As String
        Public Property FaxNumber As String
        Public Property Mobile As String
        Public Property EMail1 As String
        Public Property EMail2 As String
        Public Property BusinessTelNumber As String
        Public Property Address1 As String
        Public Property Address2 As String
        Public Property Address3 As String
        Public Property Domicile As String
        Public Property Country As String
        Public Property ZipCode As String
        Public Property StateCode As String
        Public Property DistrictCode As String
        Public Property CityCode As String
        Public Property LocationCode As String
        Public Property BirthDay As String
        Public Property NewcustomerId As String
        '2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証 START
        Public Property AllCount As String
        '2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証 START
        Public Property VhcInfo As IC3800708CustomerVhcInfoDataTable
    End Class

#End Region

#End Region

#Region "IDisposable Support"
    Private disposedValue As Boolean ' 重複する呼び出しを検出するには

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
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
        ' このコードを変更しないでください。クリーンアップ コードを上の Dispose(ByVal disposing As Boolean) に記述します。
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class
