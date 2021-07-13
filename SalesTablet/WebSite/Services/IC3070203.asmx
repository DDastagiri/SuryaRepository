<%@ WebService Language="VB" Class="Toyota.eCRB.Estimate.Quotation.WebService.IC3070203" %>

Imports System.Xml
Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.Estimate.Quotation.BizLogic
Imports Toyota.eCRB.Estimate.Quotation.DataAccess


Namespace Toyota.eCRB.Estimate.Quotation.WebService

#Region "見積情報登録I/Fクラス"
    ' この Web サービスを、スクリプトから ASP.NET AJAX を使用して呼び出せるようにするには、次の行のコメントを解除します。

    ' <System.Web.Script.Services.ScriptService()> _
    ''' <summary>
    ''' 見積情報登録I/F
    ''' プレゼンテーション層クラス
    ''' </summary>
    ''' <remarks></remarks>
    <WebService(Namespace:="http://tempuri.org/")> _
    <WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
    Public Class IC3070203
        Inherits System.Web.Services.WebService

#Region "定数定義"
        ''' <summary>
        ''' メッセージID
        ''' </summary>
        ''' <remarks>
        ''' メッセージに割り当てられた識別コード:IC3070203（見積情報登録）
        ''' </remarks>
        Private Const MessageId As String = "IC3070203"
    
        ''' <summary>
        ''' 応答結果のメッセージ（成功）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const MessageSuccess As String = "Success"
    
        ''' <summary>
        ''' 応答結果のメッセージ（失敗）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const MessageFailure As String = "Failure"

        ''' <summary>
        ''' 必須項目：チェックなし
        ''' </summary>
        ''' <remarks></remarks>
        Private Const CheckNoRequired As Short = 0
        ''' <summary>
        ''' 必須項目：チェックあり
        ''' </summary>
        ''' <remarks></remarks>
        Private Const CheckRequired As Short = 1

        ''' <summary>
        ''' 属性値：Byteチェック
        ''' </summary>
        ''' <remarks></remarks>
        Private Const AttributeByte As Short = 0
        ''' <summary>
        ''' 属性値：文字数チェック
        ''' </summary>
        ''' <remarks></remarks>
        Private Const AttributeLegth As Short = 1
        ''' <summary>
        ''' 属性値：Numericチェック
        ''' </summary>
        ''' <remarks></remarks>
        Private Const AttributeNum As Short = 2
        ''' <summary>
        ''' 属性値：Dateチェック
        ''' </summary>
        ''' <remarks></remarks>
        Private Const AttributeDate As Short = 3
        ''' <summary>
        ''' 属性値：Datetimeチェック
        ''' </summary>
        ''' <remarks></remarks>
        Private Const AttributeDatetime As Short = 4
        
        ''' <summary>
        ''' エラーコード：処理正常終了(該当データ有）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ErrCodeSuccess As Short = 0
        ''' <summary>
        ''' エラーコード：XML Document不正
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ErrCodeXmlDoc As Short = -1
        ''' <summary>
        ''' エラーコード：項目必須エラー
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ErrCodeItMust As Short = 2000
        ''' <summary>
        ''' エラーコード：項目型エラー
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ErrCodeItType As Short = 3000
        ''' <summary>
        ''' エラーコード：項目サイズエラー
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ErrCodeItSize As Short = 4000
        ''' <summary>
        ''' エラーコード：値チェックエラー
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ErrCodeItValue As Short = 5000
        ''' <summary>
        ''' エラーコード：システムエラー
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ErrCodeSys As Short = 9999
        
        ''' <summary>
        ''' 日付のフォーマット
        ''' </summary>
        ''' <remarks></remarks>
        Private Const FormatDate As String = "dd/MM/yyyy"
        ''' <summary>
        ''' 日付時刻のフォーマット
        ''' </summary>
        ''' <remarks></remarks>
        Private Const FormatDatetime As String = "dd/MM/yyyy HH:mm:ss"
        
        ''' <summary>
        ''' Headerタグ名称
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagHead As String = "Head"
        ''' <summary>
        ''' Commonタグ名称
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagCommon As String = "Common"
        ''' <summary>
        ''' EstimationInfoタグ名称
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagEstimationInfo As String = "EstimationInfo"
        ''' <summary>
        ''' EstVclOptionInfoタグ名称
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagEstVclOptionInfo As String = "EstVcloptionInfo"
        
        ''' <summary>
        ''' Customerタグ名称
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagCustomer As String = "Customer"
        
        ''' <summary>
        ''' Customer_Userタグ名称
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagCustomerUser As String = "Customer_User"
        
        ''' <summary>
        ''' Headerタグ：送信メッセージ
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagHeadMessageID As Short = 2
        ''' <summary>
        ''' Headerタグ：送信日付
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagHeadTransmissionDate As Short = 1

        ''' <summary>
        ''' 見積情報タグ：見積管理ID
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagEstEstimateId As Short = 21
        ''' <summary>
        ''' 見積情報タグ：納車予定日
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagEstDeliDate As Short = 31
        ''' <summary>
        ''' 見積情報タグ：値引き額
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagEstDiscountPrice As Short = 32
        ''' <summary>
        ''' 見積情報タグ：メモ
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagEstMemo As Short = 33
        ''' <summary>
        ''' 見積情報タグ：見積印刷日
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagEstprintDate As Short = 34
        ''' <summary>
        ''' 見積情報タグ：契約書印刷フラグ
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagEstContPrintFlg As Short = 36
        ''' <summary>
        ''' 見積情報タグ：支払方法区分
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagEstPaymentStyle As Short = 63
        ''' <summary>
        ''' 見積情報タグ：頭金
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagEstDeposit As Short = 64
        ''' <summary>
        ''' 見積情報タグ：頭金支払方法区分
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagEstDepositPaymentStyle As Short = 65
        ''' <summary>
        ''' 見積情報タグ：保険区分
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagEstInsurance As Short = 66


        ''' <summary>
        ''' 見積車両オプション情報タグ：オプション区分
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagEstOptionPart As Short = 72
        ''' <summary>
        ''' 見積車両オプション情報タグ：オプションコード
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagEstOptionCode As Short = 73
        ''' <summary>
        ''' 見積車両オプション情報タグ：オプション名
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagEstOptionName As Short = 74
        ''' <summary>
        ''' 見積車両オプション情報タグ：価格
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagEstPrice As Short = 75
        ''' <summary>
        ''' 見積車両オプション情報タグ：取付費用
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagEstInstallCost As Short = 76
        ''' <summary>
        ''' 見積車両オプション情報タグ：削除フラグ
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagEstDeleteDate As Short = 67
        
        ''' <summary>
        ''' 顧客情報タグ：シーケンスNo.
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagSeqNo As Short = 77
        
        ''' <summary>
        ''' 顧客情報タグ：顧客区分
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagCustomerType As Short = 84
        
        ''' <summary>
        ''' 顧客情報タグ：個人法人項目コード
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagSubCustomerType As Short = 85
        
        ''' <summary>
        ''' 顧客情報タグ：国民番号
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagSocialID As Short = 86
        
        ''' <summary>
        ''' 顧客情報タグ：性別区分
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagSex As Short = 87
        
        ''' <summary>
        ''' 顧客情報タグ：顧客誕生日
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagBirthDay As Short = 88
        
        ''' <summary>
        ''' 顧客情報タグ：敬称コード
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagNameTitleCode As Short = 89
        
        ''' <summary>
        ''' 顧客情報タグ：敬称
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagNameTitle As Short = 90
        
        ''' <summary>
        ''' 顧客情報タグ：ファーストネーム
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagName1 As Short = 91
        
        ''' <summary>
        ''' 顧客情報タグ：ミドルネーム
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagName2 As Short = 92
        
        ''' <summary>
        ''' 顧客情報タグ：ラストネーム
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagName3 As Short = 93
        
        ''' <summary>
        ''' 顧客情報タグ：ニックネーム
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagSubName1 As Short = 94
        
        ''' <summary>
        ''' 顧客情報タグ：顧客会社名
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagCompanyName As Short = 96
        
        ''' <summary>
        ''' 顧客情報タグ：法人担当者名
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagEmployeeName As Short = 97
        
        ''' <summary>
        ''' 顧客情報タグ：法人担当者所属部署
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagEmployeeDepartment As Short = 98
        
        ''' <summary>
        ''' 顧客情報タグ：法人担当者役職
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagEmployeePosition As Short = 99
        
        ''' <summary>
        ''' 顧客情報タグ：顧客住所1
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagAddress1 As Short = 300
        
        ''' <summary>
        ''' 顧客情報タグ：顧客住所2
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagAddress2 As Short = 301
        
        ''' <summary>
        ''' 顧客情報タグ：顧客住所3
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagAddress3 As Short = 302
        
        ''' <summary>
        ''' 顧客情報タグ：本籍
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagDomicile As Short = 303
        
        ''' <summary>
        ''' 顧客情報タグ：国籍
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagCountry As Short = 304
        
        ''' <summary>
        ''' 顧客情報タグ：顧客郵便番号 
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagZipCode As Short = 305
        
        ''' <summary>
        ''' 顧客情報タグ：顧客住所（州）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagStateCode As Short = 306
        
        ''' <summary>
        ''' 顧客情報タグ：顧客住所（地区）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagDistrictCode As Short = 307
        
        ''' <summary>
        ''' 顧客情報タグ：顧客住所（市）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagCityCode As Short = 308
        
        ''' <summary>
        ''' 顧客情報タグ：顧客住所（地域）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagLocationCode As Short = 309
        
        ''' <summary>
        ''' 顧客情報タグ：顧客電話番号
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagTelNumber As Short = 310
        
        ''' <summary>
        ''' 顧客情報タグ：顧客FAX番号
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagFaxNumber As Short = 311
        
        ''' <summary>
        ''' 顧客情報タグ：顧客携帯電話番号 
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagMobile As Short = 312
        
        ''' <summary>
        ''' 顧客情報タグ：顧客EMAILアドレス1
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagEMail1 As Short = 313
        
        ''' <summary>
        ''' 顧客情報タグ：顧客EMAILアドレス2
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagEMail2 As Short = 314
        
        ''' <summary>
        ''' 顧客情報タグ：顧客勤め先電話番号 
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagBusinessTelNumber As Short = 315
        
        ''' <summary>
        ''' 顧客情報タグ：顧客収入
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagIncome As Short = 316
        
        ''' <summary>
        ''' 顧客情報タグ：連絡時間帯ID
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagContactTime As Short = 317
        
        ''' <summary>
        ''' 顧客情報タグ：顧客職業ID
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagOccupationID As Short = 318
        
        ''' <summary>
        ''' 顧客情報タグ：顧客職業
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagOccupation As Short = 319
        
        ''' <summary>
        ''' 顧客情報タグ：結婚区分
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagFamily As Short = 320
        
        ''' <summary>
        ''' 顧客情報タグ：デフォルト言語
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagDefaultLang As Short = 321
        
        
        ''' <summary>
        ''' 顧客情報(USER)タグ：シーケンスNo.
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagUseSeqNo As Short = 77
        
        ''' <summary>
        ''' 顧客情報(USE)タグ：顧客区分
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagUseCustomerType As Short = 84
        
        ''' <summary>
        ''' 顧客情報(USE)タグ：個人法人項目コード
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagUseSubCustomerType As Short = 85
        
        ''' <summary>
        ''' 顧客情報(USE)タグ：国民番号
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagUseSocialID As Short = 86
        
        ''' <summary>
        ''' 顧客情報(USE)タグ：性別区分
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagUseSex As Short = 87
        
        ''' <summary>
        ''' 顧客情報(USE)タグ：顧客誕生日
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagUseBirthDay As Short = 88
        
        ''' <summary>
        ''' 顧客情報(USE)タグ：敬称コード
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagUseNameTitleCode As Short = 89
        
        ''' <summary>
        ''' 顧客情報(USE)タグ：敬称
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagUseNameTitle As Short = 90
        
        ''' <summary>
        ''' 顧客情報(USE)タグ：ファーストネーム
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagUseName1 As Short = 91
        
        ''' <summary>
        ''' 顧客情報(USE)タグ：ミドルネーム
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagUseName2 As Short = 92
        
        ''' <summary>
        ''' 顧客情報(USE)タグ：ラストネーム
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagUseName3 As Short = 93
        
        ''' <summary>
        ''' 顧客情報(USE)タグ：ニックネーム
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagUseSubName1 As Short = 94
        
        ''' <summary>
        ''' 顧客情報(USE)タグ：顧客会社名
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagUseCompanyName As Short = 96
        
        ''' <summary>
        ''' 顧客情報(USE)タグ：法人担当者名
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagUseEmployeeName As Short = 97
        
        ''' <summary>
        ''' 顧客情報(USE)タグ：法人担当者所属部署
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagUseEmployeeDepartment As Short = 98
        
        ''' <summary>
        ''' 顧客情報(USE)タグ：法人担当者役職
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagUseEmployeePosition As Short = 99
        
        ''' <summary>
        ''' 顧客情報(USE)タグ：顧客住所1
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagUseAddress1 As Short = 300
        
        ''' <summary>
        ''' 顧客情報(USE)タグ：顧客住所2
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagUseAddress2 As Short = 301
        
        ''' <summary>
        ''' 顧客情報(USE)タグ：顧客住所3
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagUseAddress3 As Short = 302
        
        ''' <summary>
        ''' 顧客情報(USE)タグ：本籍
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagUseDomicile As Short = 303
        
        ''' <summary>
        ''' 顧客情報(USE)タグ：国籍
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagUseCountry As Short = 304
        
        ''' <summary>
        ''' 顧客情報(USE)タグ：顧客郵便番号 
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagUseZipCode As Short = 305
        
        ''' <summary>
        ''' 顧客情報(USE)タグ：顧客住所（州）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagUseStateCode As Short = 306
        
        ''' <summary>
        ''' 顧客情報(USE)タグ：顧客住所（地区）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagUseDistrictCode As Short = 307
        
        ''' <summary>
        ''' 顧客情報(USE)タグ：顧客住所（市）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagUseCityCode As Short = 308
        
        ''' <summary>
        ''' 顧客情報(USE)タグ：顧客住所（地域）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagUseLocationCode As Short = 309
        
        ''' <summary>
        ''' 顧客情報(USE)タグ：顧客電話番号
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagUseTelNumber As Short = 310
        
        ''' <summary>
        ''' 顧客情報(USE)タグ：顧客FAX番号
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagUseFaxNumber As Short = 311
        
        ''' <summary>
        ''' 顧客情報(USE)タグ：顧客携帯電話番号 
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagUseMobile As Short = 312
        
        ''' <summary>
        ''' 顧客情報(USE)タグ：顧客EMAILアドレス1
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagUseEMail1 As Short = 313
        
        ''' <summary>
        ''' 顧客情報(USE)タグ：顧客EMAILアドレス2
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagUseEMail2 As Short = 314
        
        ''' <summary>
        ''' 顧客情報(USE)タグ：顧客勤め先電話番号 
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagUseBusinessTelNumber As Short = 315
        
        ''' <summary>
        ''' 顧客情報(USE)タグ：顧客収入
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagUseIncome As Short = 316
        
        ''' <summary>
        ''' 顧客情報(USE)タグ：連絡時間帯ID
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagUseContactTime As Short = 317
        
        ''' <summary>
        ''' 顧客情報(USE)タグ：顧客職業ID
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagUseOccupationID As Short = 318
        
        ''' <summary>
        ''' 顧客情報(USE)タグ：顧客職業
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagUseOccupation As Short = 319
        
        ''' <summary>
        ''' 顧客情報(USE)タグ：結婚区分
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagUseFamily As Short = 320
        
        ''' <summary>
        ''' 顧客情報(USE)タグ：デフォルト言語
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagUseDefaultLang As Short = 321
        
#End Region
    
#Region "メンバ変数"
        ''' <summary>
        ''' 項目名称
        ''' </summary>
        ''' <remarks>XMLタグの各項目の項目名称を保持する配列</remarks>
        Private Itemname() As String
        
        ''' <summary>
        ''' 項目番号
        ''' </summary>
        ''' <remarks>XMLタグの各項目の項目番号を保持する配列</remarks>
        Private ItemNumber() As Short
        
        ''' <summary>
        ''' 項目必須フラグ
        ''' </summary>
        ''' <remarks>XMLタグの各項目の必須チェック有無を保持する配列</remarks>
        Private Chkrequiredflg() As Short
        
        ''' <summary>
        ''' 項目属性
        ''' </summary>
        ''' <remarks>XMLタグの各項目の項目属性を保持する配列</remarks>
        Private Attribute() As Short
        
        ''' <summary>
        ''' 項目サイズ
        ''' </summary>
        ''' <remarks>XMLタグの各項目の項目サイズを保持する配列</remarks>
        Private Itemsize() As Double
        
        ''' <summary>
        ''' 項目初期値
        ''' </summary>
        ''' <remarks>XMLタグの各項目の初期値を保持する配列</remarks>
        Private DefaultValue() As String
        
        ''' <summary>
        ''' XMLタグのルート要素
        ''' </summary>
        ''' <remarks>受信XMLタグのルート要素</remarks>
        Private RootElement As XmlElement
        
        ''' <summary>
        ''' XMLタグの要素
        ''' </summary>
        ''' <remarks>受信XML各タグの要素</remarks>
        Private NodeElement As XmlElement
        
        ''' <summary>
        ''' 送信日時（Request）
        ''' </summary>
        ''' <remarks>メッセージ送信日時(yyyyMMddhhmmss)</remarks>
        Private TransmissionDate As Date

        ''' <summary>
        ''' 見積情報データテーブル
        ''' </summary>
        ''' <remarks>EstimationInfoタグ情報格納用のデータテーブル</remarks>
        Private EstimationInfoDT As IC3070203DataSet.IC3070203EstimationInfoDataTable
        
        ''' <summary>
        ''' 見積車両オプション情報データテーブル
        ''' </summary>
        ''' <remarks>EstVclOptionInfoタグ情報格納用のデータテーブル</remarks>
        Private EstVclOptionInfoDT As IC3070203DataSet.IC3070203EstVclOptionInfoDataTable
        
        ''' <summary>
        ''' 顧客情報データテーブル
        ''' </summary>
        ''' <remarks></remarks>
        Private EstUpdCustomerDT As IC3070203DataSet.IC3070203EstUpdCustomerDataTable
        
        ''' <summary>
        ''' 見積支払情報データテーブル
        ''' </summary>
        ''' <remarks></remarks>
        Private EstPaymentInfoDT As IC3070203DataSet.IC3070203EstPaymentInfoDataTable
        
        ''' <summary>
        ''' 見積保険情報データテーブル
        ''' </summary>
        ''' <remarks></remarks>
        Private EstInsuranceDT As IC3070203DataSet.IC3070203EstInsuranceInfoDataTable
        
        ''' <summary>
        ''' 見積顧客情報データテーブル
        ''' </summary>
        ''' <remarks></remarks>
        Private EstCustomerInfoDT As IC3070203DataSet.IC3070203EstCustomerInfoDataTable
        
        ''' <summary>
        ''' 見積タグ情報データテーブル
        ''' </summary>
        ''' <remarks></remarks>
        Private EstimationInfoTagPresenceDT As IC3070203DataSet.IC3070203EstimationInfoTagPresenceDataTable
        
        ''' <summary>
        ''' 見積車両オプションタグ情報データテーブル
        ''' </summary>
        ''' <remarks></remarks>
        Private EstVcloptionInfoTagPresenceDT As IC3070203DataSet.IC3070203EstVcloptionInfoTagPresenceDataTable
        
        ''' <summary>
        ''' 顧客タグ情報データテーブル
        ''' </summary>
        ''' <remarks></remarks>
        Private CustomerTagPresenceDT As IC3070203DataSet.IC3070203CustomerTagPresenceDataTable
        
        ''' <summary>
        ''' 終了コード
        ''' </summary>
        ''' <remarks>応答結果のコード（"0"の場合は正常、それ以外の場合エラー）</remarks>
        Private ResultId As Short = ErrCodeSuccess
        
        ''' <summary>
        ''' 見積管理ID
        ''' </summary>
        ''' <remarks></remarks>
        Private EstimateId As Long = 0

        ''' <summary>
        ''' 作成日
        ''' </summary>
        ''' <remarks></remarks>
        Private CreateDate As String = String.Empty
        
        ''' <summary>
        ''' タグの有無
        ''' </summary>
        ''' <remarks></remarks>
        Private TagPresence As String = IC3070203BusinessLogic.TagPresenceNo
#End Region
    
#Region "Publicメソッド"
        ''' <summary>
        ''' 見積情報を登録します。
        ''' </summary>
        ''' <param name="xsData">登録する見積情報のXML</param>
        ''' <returns>見積情報登録結果のXML</returns>
        ''' <remarks></remarks>
        <WebMethod()> _
        Public Function SetEstimation(ByVal xsData As String) As Response
        
            Dim retXml As Response = Nothing            ' 送信XML
            Dim retMessage As String = MessageFailure   ' メッセージ
            Dim receptionDate As String = String.Empty  ' 受信日時
            
            Try
                ' システム日付を取得する
                receptionDate = DateTimeFunc.Now().ToString(FormatDatetime, CultureInfo.InvariantCulture)

                ' 受信XMLをログ出力
                Logger.Info("Request XML : " & xsData, True)
            
                ' 見積情報データセットのインスタンス生成
                Using estInfoDataSet As IC3070203DataSet = New IC3070203DataSet
            
                    ' 受信XMLをプロパティにセット
                    Me.SetData(xsData, estInfoDataSet)
                    
                    ' 見積情報登録処理用
                    Dim bizLogic As IC3070203BusinessLogic = New IC3070203BusinessLogic     ' 見積情報登録I/Fビジネスロジック
                    Dim estResultDT As IC3070203DataSet.IC3070203EstResultDataTable         ' 見積情報登録結果データテーブル
                    
                    Try
                        ' 見積情報登録処理
                        estResultDT = bizLogic.SetEstimationInfo(estInfoDataSet)
                        
                        ' 登録処理結果をセット
                        Me.ResultId = bizLogic.ResultId
                        Me.EstimateId = estResultDT.Item(0).EstimateId
                        Me.CreateDate = estResultDT.Item(0).CreateDate.ToString(FormatDatetime, CultureInfo.InvariantCulture)
                        
                        If Me.ResultId = ErrCodeSuccess Then
                            retMessage = MessageSuccess
                        End If
                        
                    Catch ex As Exception
                        Me.ResultId = bizLogic.ResultId
                            
                        Throw
                    Finally
                        bizLogic = Nothing
                        estResultDT = Nothing
                    End Try

                End Using
                
            Catch ex As Exception
                If Me.ResultId = ErrCodeSuccess Then
                    Me.ResultId = ErrCodeSys
                End If
                Logger.Error(Me.ResultId.ToString(CultureInfo.InvariantCulture), ex)
            Finally
                ' 返却XMLを作成
                retXml = Me.GetResponseXml(receptionDate, retMessage)
                
                ' 終了コードをログ出力
                Logger.Info("ResultId[" & _
                            Me.TransmissionDate.ToString(FormatDatetime, CultureInfo.InvariantCulture) & _
                            "] : " & _
                            Me.ResultId.ToString(CultureInfo.InvariantCulture), _
                            True)
            End Try
        
            ' 結果を返却
            Return retXml
        
        End Function
#End Region
            
#Region "Privateメソッド"
        ''' <summary>
        ''' XMLタグの情報をデータ格納クラスにセットします。
        ''' </summary>
        ''' <param name="xsData">受信XML</param>
        ''' <param name="estInfoDataSet">見積情報データセット</param>
        ''' <remarks></remarks>
        Private Sub SetData(ByVal xsData As String, ByVal estInfoDataSet As IC3070203DataSet)
        
            ' XmlDocument生成
            Dim xdoc As New XmlDocument
            
            Try
                ' XML読み込み
                xdoc.LoadXml(xsData)
            Catch ex As Exception
                Me.ResultId = ErrCodeXmlDoc
                
                Throw
            End Try

            ' メンバ変数を設定
            Me.RootElement = xdoc.DocumentElement                                ' ルート要素
            Me.EstimationInfoDT = estInfoDataSet.IC3070203EstimationInfo         ' 見積情報データテーブル
            Me.EstVclOptionInfoDT = estInfoDataSet.IC3070203EstVclOptionInfo     ' 見積車両オプション情報データテーブル
            Me.EstCustomerInfoDT = estInfoDataSet.IC3070203EstCustomerInfo       ' 見積顧客データテーブル
            Me.EstInsuranceDT = estInfoDataSet.IC3070203EstInsuranceInfo         ' 見積保険情報データテーブル
            Me.EstPaymentInfoDT = estInfoDataSet.IC3070203EstPaymentInfo         ' 見積支払情報データテーブル
            Me.EstUpdCustomerDT = estInfoDataSet.IC3070203EstUpdCustomer         ' 顧客マスタ情報データテーブル

            Me.EstimationInfoTagPresenceDT = estInfoDataSet.IC3070203EstimationInfoTagPresence
            Me.EstVcloptionInfoTagPresenceDT = estInfoDataSet.IC3070203EstVcloptionInfoTagPresence
            Me.CustomerTagPresenceDT = estInfoDataSet.IC3070203CustomerTagPresence
            
            ' Header情報格納
            Me.InitHeader()
            Me.SetHeader()

            ' EstimationInfo情報格納
            Me.InitEstimationInfo()
            Me.SetEstimationInfo()
            
            ' EstVclOptionInfo情報格納
            Me.InitEstVclOptionInfo()
            Me.SetEstVclOptionInfo()
            
            'EstCustomer情報格納
            Me.InitEstCustomer()
            Me.SetEstUpdCustomer()
            
            'EstCustomerUse情報格納
            Me.InitEstCustomerUser()
            Me.SetEstUpdCustomerUser()
            
            xdoc = Nothing
        End Sub

#Region "初期化"
        ''' <summary>
        ''' Headerタグ情報の初期化
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub InitHeader()
        
            ' 項目名称を設定
            Me.Itemname = {"MessageID", "TransmissionDate"}
            
            ' 項目Noを設定
            Me.ItemNumber = {TagHeadMessageID, TagHeadTransmissionDate}
            
            ' 必須必須フラグを設定
            Me.Chkrequiredflg = {CheckRequired, CheckRequired}
            
            ' 項目属性を設定
            Me.Attribute = {AttributeLegth, AttributeDatetime}
            
            ' 項目サイズを設定
            Me.Itemsize = {9, 0}
        End Sub

        ''' <summary>
        ''' EstimationInfoタグ情報の初期化
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub InitEstimationInfo()
        
            ' 項目名称を設定
            Me.Itemname = _
                {"EstimateId", "DeliDate", "DiscountPrice", "Memo", _
                 "EstprintDate", "ContPrintFlg", "PaymentStyle", "Deposit", _
                 "DepositPaymentStyle", "Insurance"}
            
            ' 項目Noを設定
            Me.ItemNumber = _
                {TagEstEstimateId, TagEstDeliDate, TagEstDiscountPrice, TagEstMemo, _
                 TagEstprintDate, TagEstContPrintFlg, TagEstPaymentStyle, TagEstDeposit, _
                 TagEstDepositPaymentStyle, TagEstInsurance}
            
            ' 必須フラグを設定
            Me.Chkrequiredflg = _
                {CheckRequired, CheckNoRequired, CheckNoRequired, CheckNoRequired, _
                 CheckNoRequired, CheckNoRequired, CheckNoRequired, CheckNoRequired, _
                 CheckNoRequired, CheckNoRequired}
            
            ' 項目属性を設定
            Me.Attribute = _
                {AttributeNum, AttributeDatetime, AttributeNum, AttributeLegth, _
                 AttributeDatetime, AttributeLegth, AttributeLegth, AttributeNum, _
                 AttributeLegth, AttributeLegth}
            
            ' 項目サイズを設定
            Me.Itemsize = _
                 {10, 0, 11.2, 1024, _
                 0, 1, 1, 11.2, _
                 1, 1}
                        
        End Sub

        ''' <summary>
        ''' EstVclOptionInfoタグ情報の初期化
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub InitEstVclOptionInfo()
        
            ' 項目名称を設定
            Me.Itemname = _
                {"OptionPart", "OptionCode", "OptionName", _
                 "Price", "InstallCost", "DeleteDate"}
            
            ' 項目Noを設定
            Me.ItemNumber = _
                {TagEstOptionPart, TagEstOptionCode, TagEstOptionName, _
                 TagEstPrice, TagEstInstallCost, TagEstDeleteDate}
            
            ' 必須必須フラグを設定
            Me.Chkrequiredflg = _
                {CheckRequired, CheckRequired, CheckRequired, _
                 CheckRequired, CheckNoRequired, CheckNoRequired}
            
            ' 項目属性を設定
            Me.Attribute = _
                {AttributeLegth, AttributeLegth, AttributeLegth, AttributeNum, _
                 AttributeNum, AttributeDatetime}
            
            ' 項目サイズを設定
            Me.Itemsize = _
                 {1, 10, 64, 11.2, _
                  11.2, 0}
        End Sub
        
        ''' <summary>
        ''' EstUpdCustomerタグ情報の初期化
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub InitEstCustomer()
            
            '項目名称を設定
            Me.Itemname = {"SeqNo", "CustomerType", "SubCustomerType", "SocialID", _
                          "Sex", "BirthDay", "NameTitleCode", "NameTitle", _
                           "Name1", "Name2", "Name3", "SubName1", _
                           "CompanyName", "EmployeeName", "EmployeeDepartment", "EmployeePosition", _
                           "Address1", "Address2", "Address3", "Domicile", _
                           "Country", "ZipCode", "StateCode", "DistrictCode", _
                           "CityCode", "LocationCode", "TelNumber", "FaxNumber", _
                           "Mobile", "EMail1", "EMail2", "BusinessTelNumber", _
                           "Income", "ContactTime", "OccupationID", "Occupation", _
                           "Family", "DefaultLang"}
            
            '項目No.を設定
            Me.ItemNumber = {TagSeqNo, TagCustomerType, TagSubCustomerType, TagSocialID, _
                             TagSex, TagBirthDay, TagNameTitleCode, TagNameTitle, _
                             TagName1, TagName2, TagName3, TagSubName1, _
                             TagCompanyName, TagEmployeeName, TagEmployeeDepartment, TagEmployeePosition, _
                             TagAddress1, TagAddress2, TagAddress3, TagDomicile, _
                             TagCountry, TagZipCode, TagStateCode, TagDistrictCode, _
                             TagCityCode, TagLocationCode, TagTelNumber, TagFaxNumber, _
                             TagMobile, TagEMail1, TagEMail2, TagBusinessTelNumber, _
                             TagIncome, TagContactTime, TagOccupationID, TagOccupation, _
                             TagFamily, TagDefaultLang}
            
            '必須必須フラグを設定
            Me.Chkrequiredflg = {CheckRequired, CheckNoRequired, CheckNoRequired, CheckNoRequired, _
                                 CheckNoRequired, CheckNoRequired, CheckNoRequired, CheckNoRequired, _
                                 CheckNoRequired, CheckNoRequired, CheckNoRequired, CheckNoRequired, _
                                 CheckNoRequired, CheckNoRequired, CheckNoRequired, CheckNoRequired, _
                                 CheckNoRequired, CheckNoRequired, CheckNoRequired, CheckNoRequired, _
                                 CheckNoRequired, CheckNoRequired, CheckNoRequired, CheckNoRequired, _
                                 CheckNoRequired, CheckNoRequired, CheckNoRequired, CheckNoRequired, _
                                 CheckNoRequired, CheckNoRequired, CheckNoRequired, CheckNoRequired, _
                                 CheckNoRequired, CheckNoRequired, CheckNoRequired, CheckNoRequired, _
                                 CheckNoRequired, CheckNoRequired}
            
            '項目属性を設定
            Me.Attribute = {AttributeNum, AttributeLegth, AttributeLegth, AttributeLegth, _
                            AttributeLegth, AttributeDatetime, AttributeLegth, AttributeLegth, _
                            AttributeLegth, AttributeLegth, AttributeLegth, AttributeLegth, _
                            AttributeLegth, AttributeLegth, AttributeLegth, AttributeLegth, _
                            AttributeLegth, AttributeLegth, AttributeLegth, AttributeLegth, _
                            AttributeLegth, AttributeLegth, AttributeLegth, AttributeLegth, _
                            AttributeLegth, AttributeLegth, AttributeLegth, AttributeLegth, _
                            AttributeLegth, AttributeLegth, AttributeLegth, AttributeLegth, _
                            AttributeLegth, AttributeNum, AttributeNum, AttributeLegth, _
                            AttributeLegth, AttributeLegth}
            
            '項目サイズを設定
            Me.Itemsize = {14, 1, 2, 32, _
                           1, 0, 5, 32, _
                           64, 64, 64, 40, _
                           128, 256, 64, 64, _
                           256, 256, 256, 320, _
                           64, 32, 5, 5, _
                           5, 5, 64, 64, _
                           128, 128, 128, 64, _
                           32, 10, 10, 64, _
                           1, 2}
        End Sub
        
        ''' <summary>
        ''' EstUpdCustomerUserタグ情報の初期化
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub InitEstCustomerUser()
            
            '項目名称を設定
            Me.Itemname = {"SeqNo", "CustomerType", "SubCustomerType", "SocialID", _
                          "Sex", "BirthDay", "NameTitleCode", "NameTitle", _
                           "Name1", "Name2", "Name3", "SubName1", _
                           "CompanyName", "EmployeeName", "EmployeeDepartment", "EmployeePosition", _
                           "Address1", "Address2", "Address3", "Domicile", _
                           "Country", "ZipCode", "StateCode", "DistrictCode", _
                           "CityCode", "LocationCode", "TelNumber", "FaxNumber", _
                           "Mobile", "EMail1", "EMail2", "BusinessTelNumber", _
                           "Income", "ContactTime", "OccupationID", "Occupation", _
                           "Family", "DefaultLang"}
            
            '項目No.を設定
            Me.ItemNumber = {TagSeqNo, TagCustomerType, TagSubCustomerType, TagSocialID, _
                             TagSex, TagBirthDay, TagNameTitleCode, TagNameTitle, _
                             TagName1, TagName2, TagName3, TagSubName1, _
                             TagCompanyName, TagEmployeeName, TagEmployeeDepartment, TagEmployeePosition, _
                             TagAddress1, TagAddress2, TagAddress3, TagDomicile, _
                             TagCountry, TagZipCode, TagStateCode, TagDistrictCode, _
                             TagCityCode, TagLocationCode, TagTelNumber, TagFaxNumber, _
                             TagMobile, TagEMail1, TagEMail2, TagBusinessTelNumber, _
                             TagIncome, TagContactTime, TagOccupationID, TagOccupation, _
                             TagFamily, TagDefaultLang}
            
            '必須必須フラグを設定
            Me.Chkrequiredflg = {CheckRequired, CheckNoRequired, CheckNoRequired, CheckNoRequired, _
                                 CheckNoRequired, CheckNoRequired, CheckNoRequired, CheckNoRequired, _
                                 CheckNoRequired, CheckNoRequired, CheckNoRequired, CheckNoRequired, _
                                 CheckNoRequired, CheckNoRequired, CheckNoRequired, CheckNoRequired, _
                                 CheckNoRequired, CheckNoRequired, CheckNoRequired, CheckNoRequired, _
                                 CheckNoRequired, CheckNoRequired, CheckNoRequired, CheckNoRequired, _
                                 CheckNoRequired, CheckNoRequired, CheckNoRequired, CheckNoRequired, _
                                 CheckNoRequired, CheckNoRequired, CheckNoRequired, CheckNoRequired, _
                                 CheckNoRequired, CheckNoRequired, CheckNoRequired, CheckNoRequired, _
                                 CheckNoRequired, CheckNoRequired}
            
            '項目属性を設定
            Me.Attribute = {AttributeNum, AttributeLegth, AttributeLegth, AttributeLegth, _
                            AttributeLegth, AttributeDatetime, AttributeLegth, AttributeLegth, _
                            AttributeLegth, AttributeLegth, AttributeLegth, AttributeLegth, _
                            AttributeLegth, AttributeLegth, AttributeLegth, AttributeLegth, _
                            AttributeLegth, AttributeLegth, AttributeLegth, AttributeLegth, _
                            AttributeLegth, AttributeLegth, AttributeLegth, AttributeLegth, _
                            AttributeLegth, AttributeLegth, AttributeLegth, AttributeLegth, _
                            AttributeLegth, AttributeLegth, AttributeLegth, AttributeLegth, _
                            AttributeLegth, AttributeNum, AttributeNum, AttributeLegth, _
                            AttributeLegth, AttributeLegth}
            
            '項目サイズを設定
            Me.Itemsize = {14, 1, 2, 32, _
                           1, 0, 5, 32, _
                           64, 64, 64, 40, _
                           128, 256, 64, 64, _
                           256, 256, 256, 320, _
                           64, 32, 5, 5, _
                           5, 5, 64, 64, _
                           128, 128, 128, 64, _
                           32, 10, 10, 64, _
                           1, 2}
        End Sub

#End Region

#Region "プロパティーセット"
        ''' <summary>
        ''' Headerタグ情報のプロパティーセット
        ''' </summary>
        ''' <remarks>
        ''' XMLオブジェクトより、プロパティを設定します。
        ''' </remarks>
        Private Sub SetHeader()

            Dim itemNo As Short = 0             ' タグ番号
            Dim nodeList As XmlNodeList         ' XMLノードリスト
            Dim nodeDocument As XmlDocument     ' XML要素
            
            Try
                ' XMLノードリスト取得
                nodeList = Me.RootElement.GetElementsByTagName(TagHead)
                
                ' XML要素を設定
                nodeDocument = New XmlDocument
                nodeDocument.LoadXml(nodeList.ItemOf(0).OuterXml)
                Me.NodeElement = nodeDocument.DocumentElement
            
                ' MessageIdタグのNodeListを取得する
                Dim messageId As String = Me.GetElementValue(itemNo)

                ' TransmissionDateタグのNodeListを取得する
                itemNo += 1
                Me.TransmissionDate = Me.GetElementValue(itemNo)
                
            Catch ex As Exception
                If Me.ResultId = ErrCodeSuccess Then
                    Me.ResultId = ErrCodeItType + Me.ItemNumber(itemNo)
                End If
                Throw
            Finally
                nodeDocument = Nothing
                Me.NodeElement = Nothing
            End Try

        End Sub
        
        ''' <summary>
        ''' EstimationInfoタグ情報のプロパティーセット
        ''' </summary>
        ''' <remarks>
        ''' XMLオブジェクトより、プロパティを設定します。
        ''' </remarks>
        Private Sub SetEstimationInfo()

            Dim itemNo As Short = 0             ' タグ番号
            Dim nodeList As XmlNodeList         ' XMLノードリスト
            Dim nodeDocument As XmlDocument     ' XML要素
            
            ' 見積情報データテーブル行
            Dim estimationInfoRow As IC3070203DataSet.IC3070203EstimationInfoRow
            Dim estimationPaymentRow As IC3070203DataSet.IC3070203EstPaymentInfoRow
            Dim estimationInsuranceRow As IC3070203DataSet.IC3070203EstInsuranceInfoRow
            Dim estimationInfoTagPresenceRow As IC3070203DataSet.IC3070203EstimationInfoTagPresenceRow
            
            
            Try
                ' XMLノードリスト取得
                nodeList = Me.RootElement.GetElementsByTagName(TagEstimationInfo)
                
                ' XML要素を設定
                nodeDocument = New XmlDocument
                nodeDocument.LoadXml(nodeList.ItemOf(0).OuterXml)
                Me.NodeElement = nodeDocument.DocumentElement
                
                ' 見積情報データテーブルの新規行を作成
                estimationInfoRow = Me.EstimationInfoDT.NewRow()
                estimationPaymentRow = Me.EstPaymentInfoDT.NewRow()
                estimationInsuranceRow = Me.EstInsuranceDT.NewRow()
                estimationInfoTagPresenceRow = Me.EstimationInfoTagPresenceDT.NewRow()
                
                ' 編集開始
                estimationInfoRow.BeginEdit()
                estimationPaymentRow.BeginEdit()
                estimationInsuranceRow.BeginEdit()
                estimationInfoTagPresenceRow.BeginEdit()
                
                ' EstimateIdタグのNodeListを取得する
                itemNo = 0
                estimationInfoRow(Me.EstimationInfoDT.ESTIMATEIDColumn) = Me.GetEstimationInfoElementValue(itemNo)
                estimationInfoTagPresenceRow(Me.EstimationInfoTagPresenceDT.EstimateIdColumn) = Me.TagPresence
                
                ' DeliDateタグのNodeListを取得する
                itemNo += 1
                estimationInfoRow(Me.EstimationInfoDT.DELIDATEColumn) = Me.GetEstimationInfoElementValue(itemNo)
                estimationInfoTagPresenceRow(Me.EstimationInfoTagPresenceDT.DeliDateColumn) = Me.TagPresence
                
                ' DiscountPriceタグのNodeListを取得する
                itemNo += 1
                estimationInfoRow(Me.EstimationInfoDT.DISCOUNTPRICEColumn) = Me.GetEstimationInfoElementValue(itemNo)
                estimationInfoTagPresenceRow(Me.EstimationInfoTagPresenceDT.DiscountPriceColumn) = Me.TagPresence
                
                ' MemoタグのNodeListを取得する
                itemNo += 1
                estimationInfoRow(Me.EstimationInfoDT.MEMOColumn) = Me.GetEstimationInfoElementValue(itemNo)
                estimationInfoTagPresenceRow(Me.EstimationInfoTagPresenceDT.MemoColumn) = Me.TagPresence
                
                ' EstprintDateタグのNodeListを取得する
                itemNo += 1
                estimationInfoRow(Me.EstimationInfoDT.ESTPRINTDATEColumn) = Me.GetEstimationInfoElementValue(itemNo)
                estimationInfoTagPresenceRow(Me.EstimationInfoTagPresenceDT.EstprintDateColumn) = Me.TagPresence
                
                ' ContPrintFlgタグのNodeListを取得する
                itemNo += 1
                estimationInfoRow(Me.EstimationInfoDT.CONTPRINTFLGColumn) = Me.GetEstimationInfoElementValue(itemNo)
                estimationInfoTagPresenceRow(Me.EstimationInfoTagPresenceDT.ContPrintFlgColumn) = Me.TagPresence
                
                ' PaymentStyleタグのNodeListを取得する
                itemNo += 1
                estimationPaymentRow(Me.EstPaymentInfoDT.PAYMENTMETHODColumn) = Me.GetEstimationInfoElementValue(itemNo)
                estimationInfoTagPresenceRow(Me.EstimationInfoTagPresenceDT.PaymentStyleColumn) = Me.TagPresence
                
                ' DepositタグのNodeListを取得する
                itemNo += 1
                estimationPaymentRow(Me.EstPaymentInfoDT.DEPOSITColumn) = Me.GetEstimationInfoElementValue(itemNo)
                estimationInfoTagPresenceRow(Me.EstimationInfoTagPresenceDT.DepositColumn) = Me.TagPresence
                
                ' DepositPaymentStyleタグのNodeListを取得する
                itemNo += 1
                estimationPaymentRow(Me.EstPaymentInfoDT.DEPOSITPAYMENTMETHODColumn) = Me.GetEstimationInfoElementValue(itemNo)
                estimationInfoTagPresenceRow(Me.EstimationInfoTagPresenceDT.DepositPaymentStyleColumn) = Me.TagPresence
                
                ' InsuranceタグのNodeListを取得する
                itemNo += 1
                estimationInsuranceRow(Me.EstInsuranceDT.INSUDVSColumn) = Me.GetEstimationInfoElementValue(itemNo)
                estimationInfoTagPresenceRow(Me.EstimationInfoTagPresenceDT.InsuranceColumn) = Me.TagPresence
                
                
                ' 編集終了
                estimationInfoRow.EndEdit()
                estimationPaymentRow.EndEdit()
                estimationInsuranceRow.EndEdit()
                estimationInfoTagPresenceRow.EndEdit()
                
                ' 編集内容を反映
                Me.EstimationInfoDT.AddIC3070203EstimationInfoRow(estimationInfoRow)
                Me.EstPaymentInfoDT.AddIC3070203EstPaymentInfoRow(estimationPaymentRow)
                Me.EstInsuranceDT.AddIC3070203EstInsuranceInfoRow(estimationInsuranceRow)
                Me.EstimationInfoTagPresenceDT.AddIC3070203EstimationInfoTagPresenceRow(estimationInfoTagPresenceRow)
                
            Catch dex As System.Data.DataException
                If Me.ResultId = ErrCodeSuccess Then
                    Me.ResultId = ErrCodeSys
                End If
                Throw
            Catch ex As Exception
                If Me.ResultId = ErrCodeSuccess Then
                    Me.ResultId = ErrCodeItType + Me.ItemNumber(itemNo)
                End If
                Throw
            Finally
                nodeDocument = Nothing
                Me.NodeElement = Nothing
            End Try

        End Sub

        ''' <summary>
        ''' EstVclOptionInfoタグ情報のプロパティーセット
        ''' </summary>
        ''' <remarks>
        ''' XMLオブジェクトより、プロパティを設定します。
        ''' </remarks>
        Private Sub SetEstVclOptionInfo()

            Dim itemNo As Short = 0             ' タグ番号
            Dim nodeList As XmlNodeList         ' XMLノードリスト
            Dim nodeDocument As XmlDocument     ' XML要素
            
            ' 見積車両オプション情報データテーブル行
            Dim estVclOptionInfoRow As IC3070203DataSet.IC3070203EstVclOptionInfoRow
            Dim estVcloptionInfoTagPresenceRow As IC3070203DataSet.IC3070203EstVcloptionInfoTagPresenceRow
            
            Try
                ' XMLノードリスト取得
                nodeList = Me.RootElement.GetElementsByTagName(TagEstVclOptionInfo)
                
                ' XMLノードリスト内のXML要素分実行
                For Each elem As XmlElement In nodeList
                    
                    ' XML要素を設定
                    nodeDocument = New XmlDocument
                    nodeDocument.LoadXml(elem.OuterXml)
                    Me.NodeElement = nodeDocument.DocumentElement
                
                    ' 見積車両オプション情報データテーブルの新規行を作成
                    estVclOptionInfoRow = Me.EstVclOptionInfoDT.NewRow()
                    estVcloptionInfoTagPresenceRow = Me.EstVcloptionInfoTagPresenceDT.NewRow()
                
                    ' 編集開始
                    estVclOptionInfoRow.BeginEdit()
                    estVcloptionInfoTagPresenceRow.BeginEdit()
                                
                    ' OptionPartタグのNodeListを取得する
                    itemNo = 0
                    estVclOptionInfoRow(Me.EstVclOptionInfoDT.OPTIONPARTColumn) = Me.GetEstVclOptionInfoElementValue(itemNo)
                    estVcloptionInfoTagPresenceRow(Me.EstVcloptionInfoTagPresenceDT.OptionPartColumn) = Me.TagPresence
                    
                    ' OptionCodeタグのNodeListを取得する
                    itemNo += 1
                    estVclOptionInfoRow(Me.EstVclOptionInfoDT.OPTIONCODEColumn) = Me.GetEstVclOptionInfoElementValue(itemNo)
                    estVcloptionInfoTagPresenceRow(Me.EstVcloptionInfoTagPresenceDT.OptionCodeColumn) = Me.TagPresence
                
                    ' OptionNameタグのNodeListを取得する
                    itemNo += 1
                    estVclOptionInfoRow(Me.EstVclOptionInfoDT.OPTIONNAMEColumn) = Me.GetEstVclOptionInfoElementValue(itemNo)
                    estVcloptionInfoTagPresenceRow(Me.EstVcloptionInfoTagPresenceDT.OptionNameColumn) = Me.TagPresence
                
                    ' PriceタグのNodeListを取得する
                    itemNo += 1
                    estVclOptionInfoRow(Me.EstVclOptionInfoDT.PRICEColumn) = Me.GetEstVclOptionInfoElementValue(itemNo)
                    estVcloptionInfoTagPresenceRow(Me.EstVcloptionInfoTagPresenceDT.PriceColumn) = Me.TagPresence
                
                    ' InstallCostタグのNodeListを取得する
                    itemNo += 1
                    estVclOptionInfoRow(Me.EstVclOptionInfoDT.INSTALLCOSTColumn) = Me.GetEstVclOptionInfoElementValue(itemNo)
                    estVcloptionInfoTagPresenceRow(Me.EstVcloptionInfoTagPresenceDT.InstallCostColumn) = Me.TagPresence
                
                    ' DeleteDateタグのNodeListを取得する
                    itemNo += 1
                    estVclOptionInfoRow(Me.EstVclOptionInfoDT.DELETEDATEColumn) = Me.GetEstVclOptionInfoElementValue(itemNo)
                    estVcloptionInfoTagPresenceRow(Me.EstVcloptionInfoTagPresenceDT.DeleteDateColumn) = Me.TagPresence
                    
                    ' 編集終了
                    estVclOptionInfoRow.EndEdit()
                    estVcloptionInfoTagPresenceRow.EndEdit()
                
                    ' 編集内容を反映
                    Me.EstVclOptionInfoDT.AddIC3070203EstVclOptionInfoRow(estVclOptionInfoRow)
                    Me.EstVcloptionInfoTagPresenceDT.AddIC3070203EstVcloptionInfoTagPresenceRow(estVcloptionInfoTagPresenceRow)
                    
                    nodeDocument = Nothing
                    Me.NodeElement = Nothing
                Next
                
            Catch dex As System.Data.DataException
                If Me.ResultId = ErrCodeSuccess Then
                    Me.ResultId = ErrCodeSys
                End If
                Throw
            Catch ex As Exception
                If Me.ResultId = ErrCodeSuccess Then
                    Me.ResultId = ErrCodeItType + Me.ItemNumber(itemNo)
                End If
                Throw
            Finally
                nodeDocument = Nothing
                Me.NodeElement = Nothing
            End Try
            
        End Sub
        
        ''' <summary>
        ''' Customerタグ情報のプロパティーセット
        ''' </summary>
        ''' <remarks>
        ''' XMLオブジェクトより、プロパティを設定します。
        ''' </remarks>
        Private Sub SetEstUpdCustomer()
            
            Dim itemNo As Short = 0             ' タグ番号
            Dim nodeList As XmlNodeList         ' XMLノードリスト
            Dim nodeDocument As XmlDocument     ' XML要素
            
            ' 顧客情報データテーブル行
            Dim estUpdCustomerRow As IC3070203DataSet.IC3070203EstUpdCustomerRow
            Dim customerTagPresenceRow As IC3070203DataSet.IC3070203CustomerTagPresenceRow
            
            Try
                ' XMLノードリスト取得
                nodeList = Me.RootElement.GetElementsByTagName(TagCustomer)
                
                ' XMLノードリスト内のXML要素分実行
                For Each elem As XmlElement In nodeList
                    
                    ' XML要素を設定
                    nodeDocument = New XmlDocument
                    nodeDocument.LoadXml(elem.OuterXml)
                    Me.NodeElement = nodeDocument.DocumentElement
                    
                    ' 顧客情報データテーブル新規行作成
                    estUpdCustomerRow = Me.EstUpdCustomerDT.NewRow()
                    customerTagPresenceRow = Me.CustomerTagPresenceDT.NewRow()
                    
                    ' 編集開始
                    estUpdCustomerRow.BeginEdit()
                    customerTagPresenceRow.BeginEdit()
                    
                    ' 項目を設定
                    ' SeqNoタグのNodeListを取得する
                    itemNo = 0
                    Dim seqNo As Long = Me.GetUpdCustomerElementValue(itemNo)
                    customerTagPresenceRow(Me.CustomerTagPresenceDT.SeqNoColumn) = Me.TagPresence
                    
                    ' CustomerTypeタグのNodeListを取得する
                    itemNo += 1
                    estUpdCustomerRow(Me.EstUpdCustomerDT.FLEET_FLGColumn) = Me.GetUpdCustomerElementValue(itemNo)
                    customerTagPresenceRow(Me.CustomerTagPresenceDT.CustomerTypeColumn) = Me.TagPresence
                    
                    ' SubCustomerTypeタグのNodeListを取得する
                    itemNo += 1
                    estUpdCustomerRow(Me.EstUpdCustomerDT.PRIVATE_FLEET_ITEM_CDColumn) = Me.GetUpdCustomerElementValue(itemNo)
                    customerTagPresenceRow(Me.CustomerTagPresenceDT.SubCustomerTypeColumn) = Me.TagPresence
                    
                    ' SocialIDタグのNodeListを取得する
                    itemNo += 1
                    estUpdCustomerRow(Me.EstUpdCustomerDT.CST_SOCIALNUMColumn) = Me.GetUpdCustomerElementValue(itemNo)
                    customerTagPresenceRow(Me.CustomerTagPresenceDT.SocialIDColumn) = Me.TagPresence
                                        
                    ' SexタグのNodeListを取得する
                    itemNo += 1
                    estUpdCustomerRow(Me.EstUpdCustomerDT.CST_GENDERColumn) = Me.GetUpdCustomerElementValue(itemNo)
                    customerTagPresenceRow(Me.CustomerTagPresenceDT.SexColumn) = Me.TagPresence
                    
                    ' BirthDayタグのNodeListを取得する
                    itemNo += 1
                    estUpdCustomerRow(Me.EstUpdCustomerDT.CST_BIRTH_DATEColumn) = Me.GetUpdCustomerElementValue(itemNo)
                    customerTagPresenceRow(Me.CustomerTagPresenceDT.BirthDayColumn) = Me.TagPresence
                    
                    ' NameTitleCodeタグのNodeListを取得する
                    itemNo += 1
                    estUpdCustomerRow(Me.EstUpdCustomerDT.NAMETITLE_CDColumn) = Me.GetUpdCustomerElementValue(itemNo)
                    customerTagPresenceRow(Me.CustomerTagPresenceDT.NameTitleCodeColumn) = Me.TagPresence
                                        
                    ' NameTitleタグのNodeListを取得する
                    itemNo += 1
                    estUpdCustomerRow(Me.EstUpdCustomerDT.NAMETITLE_NAMEColumn) = Me.GetUpdCustomerElementValue(itemNo)
                    customerTagPresenceRow(Me.CustomerTagPresenceDT.NameTitleColumn) = Me.TagPresence
                                        
                    ' Name1タグのNodeListを取得する
                    itemNo += 1
                    estUpdCustomerRow(Me.EstUpdCustomerDT.FIRST_NAMEColumn) = Me.GetUpdCustomerElementValue(itemNo)
                    customerTagPresenceRow(Me.CustomerTagPresenceDT.Name1Column) = Me.TagPresence
                                        
                    ' Name2タグのNodeListを取得する
                    itemNo += 1
                    estUpdCustomerRow(Me.EstUpdCustomerDT.MIDDLE_NAMEColumn) = Me.GetUpdCustomerElementValue(itemNo)
                    customerTagPresenceRow(Me.CustomerTagPresenceDT.Name2Column) = Me.TagPresence
                                        
                    ' Name3タグのNodeListを取得する
                    itemNo += 1
                    estUpdCustomerRow(Me.EstUpdCustomerDT.LAST_NAMEColumn) = Me.GetUpdCustomerElementValue(itemNo)
                    customerTagPresenceRow(Me.CustomerTagPresenceDT.Name3Column) = Me.TagPresence
                                        
                    ' SubName1タグのNodeListを取得する
                    itemNo += 1
                    estUpdCustomerRow(Me.EstUpdCustomerDT.NICK_NAMEColumn) = Me.GetUpdCustomerElementValue(itemNo)
                    customerTagPresenceRow(Me.CustomerTagPresenceDT.SubName1Column) = Me.TagPresence
                    
                    ' CompanyNameタグのNodeListを取得する
                    itemNo += 1
                    estUpdCustomerRow(Me.EstUpdCustomerDT.CST_COMPANY_NAMEColumn) = Me.GetUpdCustomerElementValue(itemNo)
                    customerTagPresenceRow(Me.CustomerTagPresenceDT.CompanyNameColumn) = Me.TagPresence
                    
                    ' EmployeeNameタグのNodeListを取得する
                    itemNo += 1
                    estUpdCustomerRow(Me.EstUpdCustomerDT.FLEET_PIC_NAMEColumn) = Me.GetUpdCustomerElementValue(itemNo)
                    customerTagPresenceRow(Me.CustomerTagPresenceDT.EmployeeNameColumn) = Me.TagPresence
                    
                    ' EmployeeDepartmentタグのNodeListを取得する
                    itemNo += 1
                    estUpdCustomerRow(Me.EstUpdCustomerDT.FLEET_PIC_DEPTColumn) = Me.GetUpdCustomerElementValue(itemNo)
                    customerTagPresenceRow(Me.CustomerTagPresenceDT.EmployeeDepartmentColumn) = Me.TagPresence
                    
                    ' EmployeePositionタグのNodeListを取得する
                    itemNo += 1
                    estUpdCustomerRow(Me.EstUpdCustomerDT.FLEET_PIC_POSITIONColumn) = Me.GetUpdCustomerElementValue(itemNo)
                    customerTagPresenceRow(Me.CustomerTagPresenceDT.EmployeePositionColumn) = Me.TagPresence

                    ' Address1タグのNodeListを取得する
                    itemNo += 1
                    estUpdCustomerRow(Me.EstUpdCustomerDT.CST_ADDRESS_1Column) = Me.GetUpdCustomerElementValue(itemNo)
                    customerTagPresenceRow(Me.CustomerTagPresenceDT.Address1Column) = Me.TagPresence
                                        
                    ' Address2タグのNodeListを取得する
                    itemNo += 1
                    estUpdCustomerRow(Me.EstUpdCustomerDT.CST_ADDRESS_2Column) = Me.GetUpdCustomerElementValue(itemNo)
                    customerTagPresenceRow(Me.CustomerTagPresenceDT.Address2Column) = Me.TagPresence
                                        
                    ' Address3タグのNodeListを取得する
                    itemNo += 1
                    estUpdCustomerRow(Me.EstUpdCustomerDT.CST_ADDRESS_3Column) = Me.GetUpdCustomerElementValue(itemNo)
                    customerTagPresenceRow(Me.CustomerTagPresenceDT.Address3Column) = Me.TagPresence
                                        
                    ' DomicileタグのNodeListを取得する
                    itemNo += 1
                    estUpdCustomerRow(Me.EstUpdCustomerDT.CST_DOMICILEColumn) = Me.GetUpdCustomerElementValue(itemNo)
                    customerTagPresenceRow(Me.CustomerTagPresenceDT.DomicileColumn) = Me.TagPresence
                    
                    ' CountryタグのNodeListを取得する
                    itemNo += 1
                    estUpdCustomerRow(Me.EstUpdCustomerDT.CST_COUNTRYColumn) = Me.GetUpdCustomerElementValue(itemNo)
                    customerTagPresenceRow(Me.CustomerTagPresenceDT.CountryColumn) = Me.TagPresence
                    
                    ' ZipCodeタグのNodeListを取得する
                    itemNo += 1
                    estUpdCustomerRow(Me.EstUpdCustomerDT.CST_ZIPCDColumn) = Me.GetUpdCustomerElementValue(itemNo)
                    customerTagPresenceRow(Me.CustomerTagPresenceDT.ZipCodeColumn) = Me.TagPresence
                                        
                    ' StateCodeタグのNodeListを取得する
                    itemNo += 1
                    estUpdCustomerRow(Me.EstUpdCustomerDT.CST_ADDRESS_STATEColumn) = Me.GetUpdCustomerElementValue(itemNo)
                    customerTagPresenceRow(Me.CustomerTagPresenceDT.StateCodeColumn) = Me.TagPresence
                                        
                    ' DistrictCodeタグのNodeListを取得する
                    itemNo += 1
                    estUpdCustomerRow(Me.EstUpdCustomerDT.CST_ADDRESS_DISTRICTColumn) = Me.GetUpdCustomerElementValue(itemNo)
                    customerTagPresenceRow(Me.CustomerTagPresenceDT.DistrictCodeColumn) = Me.TagPresence
                                        
                    ' CityCodeタグのNodeListを取得する
                    itemNo += 1
                    estUpdCustomerRow(Me.EstUpdCustomerDT.CST_ADDRESS_CITYColumn) = Me.GetUpdCustomerElementValue(itemNo)
                    customerTagPresenceRow(Me.CustomerTagPresenceDT.CityCodeColumn) = Me.TagPresence
                                        
                    ' LocationCodeタグのNodeListを取得する
                    itemNo += 1
                    estUpdCustomerRow(Me.EstUpdCustomerDT.CST_ADDRESS_LOCATIONColumn) = Me.GetUpdCustomerElementValue(itemNo)
                    customerTagPresenceRow(Me.CustomerTagPresenceDT.LocationCodeColumn) = Me.TagPresence
                                        
                    ' TelNumberタグのNodeListを取得する
                    itemNo += 1
                    estUpdCustomerRow(Me.EstUpdCustomerDT.CST_PHONEColumn) = Me.GetUpdCustomerElementValue(itemNo)
                    customerTagPresenceRow(Me.CustomerTagPresenceDT.TelNumberColumn) = Me.TagPresence
                                        
                    ' FaxNumberタグのNodeListを取得する
                    itemNo += 1
                    estUpdCustomerRow(Me.EstUpdCustomerDT.CST_FAXColumn) = Me.GetUpdCustomerElementValue(itemNo)
                    customerTagPresenceRow(Me.CustomerTagPresenceDT.FaxNumberColumn) = Me.TagPresence
                                        
                    ' MobileタグのNodeListを取得する
                    itemNo += 1
                    estUpdCustomerRow(Me.EstUpdCustomerDT.CST_MOBILEColumn) = Me.GetUpdCustomerElementValue(itemNo)
                    customerTagPresenceRow(Me.CustomerTagPresenceDT.MobileColumn) = Me.TagPresence
                                        
                    ' EMail1タグのNodeListを取得する
                    itemNo += 1
                    estUpdCustomerRow(Me.EstUpdCustomerDT.CST_EMAIL_1Column) = Me.GetUpdCustomerElementValue(itemNo)
                    customerTagPresenceRow(Me.CustomerTagPresenceDT.EMail1Column) = Me.TagPresence
                                        
                    ' EMail2タグのNodeListを取得する
                    itemNo += 1
                    estUpdCustomerRow(Me.EstUpdCustomerDT.CST_EMAIL_2Column) = Me.GetUpdCustomerElementValue(itemNo)
                    customerTagPresenceRow(Me.CustomerTagPresenceDT.EMail2Column) = Me.TagPresence
                    
                    ' BusinessTelNumberタグのNodeListを取得する
                    itemNo += 1
                    estUpdCustomerRow(Me.EstUpdCustomerDT.CST_BIZ_PHONEColumn) = Me.GetUpdCustomerElementValue(itemNo)
                    customerTagPresenceRow(Me.CustomerTagPresenceDT.BusinessTelNumberColumn) = Me.TagPresence
                    
                    ' IncomeタグのNodeListを取得する
                    itemNo += 1
                    estUpdCustomerRow(Me.EstUpdCustomerDT.CST_INCOMEColumn) = Me.GetUpdCustomerElementValue(itemNo)
                    customerTagPresenceRow(Me.CustomerTagPresenceDT.IncomeColumn) = Me.TagPresence
                    
                    ' ContactTimeタグのNodeListを取得する
                    itemNo += 1
                    estUpdCustomerRow(Me.EstUpdCustomerDT.CONTACT_TIMESLOTColumn) = Me.GetUpdCustomerElementValue(itemNo)
                    customerTagPresenceRow(Me.CustomerTagPresenceDT.ContactTimeColumn) = Me.TagPresence
                    
                    ' OccupationIDタグのNodeListを取得する
                    itemNo += 1
                    estUpdCustomerRow(Me.EstUpdCustomerDT.CST_OCCUPATION_IDColumn) = Me.GetUpdCustomerElementValue(itemNo)
                    customerTagPresenceRow(Me.CustomerTagPresenceDT.OccupationIDColumn) = Me.TagPresence
                    
                    ' OccupationタグのNodeListを取得する
                    itemNo += 1
                    estUpdCustomerRow(Me.EstUpdCustomerDT.CST_OCCUPATIONColumn) = Me.GetUpdCustomerElementValue(itemNo)
                    customerTagPresenceRow(Me.CustomerTagPresenceDT.OccupationColumn) = Me.TagPresence
                    
                    ' FamilyタグのNodeListを取得する
                    itemNo += 1
                    estUpdCustomerRow(Me.EstUpdCustomerDT.MARITAL_TYPEColumn) = Me.GetUpdCustomerElementValue(itemNo)
                    customerTagPresenceRow(Me.CustomerTagPresenceDT.FamilyColumn) = Me.TagPresence
                    
                    ' DefaultLangタグのNodeListを取得する
                    itemNo += 1
                    estUpdCustomerRow(Me.EstUpdCustomerDT.DEFAULT_LANGColumn) = Me.GetUpdCustomerElementValue(itemNo)
                    customerTagPresenceRow(Me.CustomerTagPresenceDT.DefaultLangColumn) = Me.TagPresence
                    
                    ' 顧客種別を設定
                    estUpdCustomerRow(Me.EstUpdCustomerDT.CONTRACTCUSTTYPEColumn) = IC3070203BusinessLogic.CustTypeOwner
                    customerTagPresenceRow(Me.CustomerTagPresenceDT.CONTRACTCUSTTYPEColumn) = IC3070203BusinessLogic.CustTypeOwner
                    
                    ' 編集終了
                    estUpdCustomerRow.EndEdit()
                    customerTagPresenceRow.EndEdit()
                    
                    ' 編集内容を反映
                    Me.EstUpdCustomerDT.AddIC3070203EstUpdCustomerRow(estUpdCustomerRow)
                    Me.CustomerTagPresenceDT.AddIC3070203CustomerTagPresenceRow(customerTagPresenceRow)
                    
                    nodeDocument = Nothing
                    Me.NodeElement = Nothing
                    
                Next
                
            Catch dex As System.Data.DataException
                If Me.ResultId = ErrCodeSuccess Then
                    Me.ResultId = ErrCodeSys
                End If
                Throw
            Catch ex As Exception
                If Me.ResultId = ErrCodeSuccess Then
                    Me.ResultId = ErrCodeItType + Me.ItemNumber(itemNo)
                End If
                Throw
            Finally
                nodeDocument = Nothing
                Me.NodeElement = Nothing
            End Try
        End Sub
        
        ''' <summary>
        ''' Customer_Userタグ情報のプロパティーセット
        ''' </summary>
        ''' <remarks>
        ''' XMLオブジェクトより、プロパティを設定します。
        ''' </remarks>
        Private Sub SetEstUpdCustomerUser()
            
            
            Dim itemNo As Short = 0             ' タグ番号
            Dim nodeList As XmlNodeList         ' XMLノードリスト
            Dim nodeDocument As XmlDocument     ' XML要素
            
            ' 顧客情報データテーブル行
            Dim estUpdCustomerRow As IC3070203DataSet.IC3070203EstUpdCustomerRow
            Dim customerTagPresenceRow As IC3070203DataSet.IC3070203CustomerTagPresenceRow
            
            Try
                ' XMLノードリスト取得
                nodeList = Me.RootElement.GetElementsByTagName(TagCustomerUser)
                
                ' XMLノードリスト内のXML要素分実行
                For Each elem As XmlElement In nodeList
                    
                    ' XML要素を設定
                    nodeDocument = New XmlDocument
                    nodeDocument.LoadXml(elem.OuterXml)
                    Me.NodeElement = nodeDocument.DocumentElement
                    
                    ' 顧客情報データテーブル新規行作成
                    estUpdCustomerRow = Me.EstUpdCustomerDT.NewRow()
                    customerTagPresenceRow = Me.CustomerTagPresenceDT.NewRow()
                    
                    ' 編集開始
                    estUpdCustomerRow.BeginEdit()
                    customerTagPresenceRow.BeginEdit()
                    
                    ' 項目を設定
                    ' CustomerTypeタグのSeqNoを取得する
                    itemNo = 0
                    Dim seqNo As Long = Me.GetUpdCustomerElementValue(itemNo)
                    customerTagPresenceRow(Me.CustomerTagPresenceDT.SeqNoColumn) = Me.TagPresence
                    
                    ' CustomerTypeタグのNodeListを取得する
                    itemNo += 1
                    estUpdCustomerRow(Me.EstUpdCustomerDT.FLEET_FLGColumn) = Me.GetUpdCustomerElementValue(itemNo)
                    customerTagPresenceRow(Me.CustomerTagPresenceDT.CustomerTypeColumn) = Me.TagPresence
                    
                    ' SubCustomerTypeタグのNodeListを取得する
                    itemNo += 1
                    estUpdCustomerRow(Me.EstUpdCustomerDT.PRIVATE_FLEET_ITEM_CDColumn) = Me.GetUpdCustomerElementValue(itemNo)
                    customerTagPresenceRow(Me.CustomerTagPresenceDT.SubCustomerTypeColumn) = Me.TagPresence
                    
                    ' SocialIDタグのNodeListを取得する
                    itemNo += 1
                    estUpdCustomerRow(Me.EstUpdCustomerDT.CST_SOCIALNUMColumn) = Me.GetUpdCustomerElementValue(itemNo)
                    customerTagPresenceRow(Me.CustomerTagPresenceDT.SocialIDColumn) = Me.TagPresence
                    
                    ' スキップ
                    itemNo += 2
                    
                    ' NameTitleCodeタグのNodeListを取得する
                    itemNo += 1
                    estUpdCustomerRow(Me.EstUpdCustomerDT.NAMETITLE_CDColumn) = Me.GetUpdCustomerElementValue(itemNo)
                    customerTagPresenceRow(Me.CustomerTagPresenceDT.NameTitleCodeColumn) = Me.TagPresence
                    
                    ' NameTitleタグのNodeListを取得する
                    itemNo += 1
                    estUpdCustomerRow(Me.EstUpdCustomerDT.NAMETITLE_NAMEColumn) = Me.GetUpdCustomerElementValue(itemNo)
                    customerTagPresenceRow(Me.CustomerTagPresenceDT.NameTitleColumn) = Me.TagPresence
                    
                    ' Name1タグのNodeListを取得する
                    itemNo += 1
                    estUpdCustomerRow(Me.EstUpdCustomerDT.FIRST_NAMEColumn) = Me.GetUpdCustomerElementValue(itemNo)
                    customerTagPresenceRow(Me.CustomerTagPresenceDT.Name1Column) = Me.TagPresence
                    
                    ' Name2タグのNodeListを取得する
                    itemNo += 1
                    estUpdCustomerRow(Me.EstUpdCustomerDT.MIDDLE_NAMEColumn) = Me.GetUpdCustomerElementValue(itemNo)
                    customerTagPresenceRow(Me.CustomerTagPresenceDT.Name2Column) = Me.TagPresence
                    
                    ' Name3タグのNodeListを取得する
                    itemNo += 1
                    estUpdCustomerRow(Me.EstUpdCustomerDT.LAST_NAMEColumn) = Me.GetUpdCustomerElementValue(itemNo)
                    customerTagPresenceRow(Me.CustomerTagPresenceDT.Name3Column) = Me.TagPresence
                    
                    ' スキップ
                    itemNo += 5
                    
                    ' Address1タグのNodeListを取得する
                    itemNo += 1
                    estUpdCustomerRow(Me.EstUpdCustomerDT.CST_ADDRESS_1Column) = Me.GetUpdCustomerElementValue(itemNo)
                    customerTagPresenceRow(Me.CustomerTagPresenceDT.Address1Column) = Me.TagPresence
                    
                    ' Address2タグのNodeListを取得する
                    itemNo += 1
                    estUpdCustomerRow(Me.EstUpdCustomerDT.CST_ADDRESS_2Column) = Me.GetUpdCustomerElementValue(itemNo)
                    customerTagPresenceRow(Me.CustomerTagPresenceDT.Address2Column) = Me.TagPresence
                    
                    ' Address3タグのNodeListを取得する
                    itemNo += 1
                    estUpdCustomerRow(Me.EstUpdCustomerDT.CST_ADDRESS_3Column) = Me.GetUpdCustomerElementValue(itemNo)
                    customerTagPresenceRow(Me.CustomerTagPresenceDT.Address3Column) = Me.TagPresence

                    ' スキップ
                    itemNo += 2
                    
                    ' ZipCodeタグのNodeListを取得する
                    itemNo += 1
                    estUpdCustomerRow(Me.EstUpdCustomerDT.CST_ZIPCDColumn) = Me.GetUpdCustomerElementValue(itemNo)
                    customerTagPresenceRow(Me.CustomerTagPresenceDT.ZipCodeColumn) = Me.TagPresence
                    
                    ' StateCodeタグのNodeListを取得する
                    itemNo += 1
                    estUpdCustomerRow(Me.EstUpdCustomerDT.CST_ADDRESS_STATEColumn) = Me.GetUpdCustomerElementValue(itemNo)
                    customerTagPresenceRow(Me.CustomerTagPresenceDT.StateCodeColumn) = Me.TagPresence
                    
                    ' DistrictCodeタグのNodeListを取得する
                    itemNo += 1
                    estUpdCustomerRow(Me.EstUpdCustomerDT.CST_ADDRESS_DISTRICTColumn) = Me.GetUpdCustomerElementValue(itemNo)
                    customerTagPresenceRow(Me.CustomerTagPresenceDT.DistrictCodeColumn) = Me.TagPresence
                    
                    ' CityCodeタグのNodeListを取得する
                    itemNo += 1
                    estUpdCustomerRow(Me.EstUpdCustomerDT.CST_ADDRESS_CITYColumn) = Me.GetUpdCustomerElementValue(itemNo)
                    customerTagPresenceRow(Me.CustomerTagPresenceDT.CityCodeColumn) = Me.TagPresence
                    
                    ' LocationCodeタグのNodeListを取得する
                    itemNo += 1
                    estUpdCustomerRow(Me.EstUpdCustomerDT.CST_ADDRESS_LOCATIONColumn) = Me.GetUpdCustomerElementValue(itemNo)
                    customerTagPresenceRow(Me.CustomerTagPresenceDT.LocationCodeColumn) = Me.TagPresence
                    
                    ' TelNumberタグのNodeListを取得する
                    itemNo += 1
                    estUpdCustomerRow(Me.EstUpdCustomerDT.CST_PHONEColumn) = Me.GetUpdCustomerElementValue(itemNo)
                    customerTagPresenceRow(Me.CustomerTagPresenceDT.TelNumberColumn) = Me.TagPresence
                    
                    ' FaxNumberタグのNodeListを取得する
                    itemNo += 1
                    estUpdCustomerRow(Me.EstUpdCustomerDT.CST_FAXColumn) = Me.GetUpdCustomerElementValue(itemNo)
                    customerTagPresenceRow(Me.CustomerTagPresenceDT.FaxNumberColumn) = Me.TagPresence
                    
                    ' MobileタグのNodeListを取得する
                    itemNo += 1
                    estUpdCustomerRow(Me.EstUpdCustomerDT.CST_MOBILEColumn) = Me.GetUpdCustomerElementValue(itemNo)
                    customerTagPresenceRow(Me.CustomerTagPresenceDT.MobileColumn) = Me.TagPresence
                    
                    ' EMail1タグのNodeListを取得する
                    itemNo += 1
                    estUpdCustomerRow(Me.EstUpdCustomerDT.CST_EMAIL_1Column) = Me.GetUpdCustomerElementValue(itemNo)
                    customerTagPresenceRow(Me.CustomerTagPresenceDT.EMail1Column) = Me.TagPresence
                    
                    ' 顧客種別を設定
                    estUpdCustomerRow(Me.EstUpdCustomerDT.CONTRACTCUSTTYPEColumn) = IC3070203BusinessLogic.CustTypeUser
                    customerTagPresenceRow(Me.CustomerTagPresenceDT.CONTRACTCUSTTYPEColumn) = IC3070203BusinessLogic.CustTypeUser
                    
                                        
                    ' 編集終了
                    estUpdCustomerRow.EndEdit()
                    customerTagPresenceRow.EndEdit()
                    
                    ' 編集内容を反映
                    Me.EstUpdCustomerDT.AddIC3070203EstUpdCustomerRow(estUpdCustomerRow)
                    Me.CustomerTagPresenceDT.AddIC3070203CustomerTagPresenceRow(customerTagPresenceRow)
                    
                    nodeDocument = Nothing
                    Me.NodeElement = Nothing
                    
                Next
                
            Catch dex As System.Data.DataException
                If Me.ResultId = ErrCodeSuccess Then
                    Me.ResultId = ErrCodeSys
                End If
                Throw
            Catch ex As Exception
                If Me.ResultId = ErrCodeSuccess Then
                    Me.ResultId = ErrCodeItType + Me.ItemNumber(itemNo)
                End If
                Throw
            Finally
                nodeDocument = Nothing
                Me.NodeElement = Nothing
            End Try

        End Sub
        
#End Region
        
#Region "各タグデータの取得"
        ''' <summary>
        ''' EstimationInfoタグのデータを取得します。
        ''' </summary>
        ''' <param name="No">項目No</param>
        ''' <returns>XMLから取り出した値</returns>
        ''' <remarks>
        ''' XMLからデータを取り出し、必須／属性／サイズチェックを実施します。
        ''' </remarks>
        Private Function GetEstimationInfoElementValue(ByVal no As Short) As Object
            
            ' 返却するオブジェクトを取得
            Dim valueObj As Object = Me.GetElementValue(no)
            
            ' チェック結果
            Dim isValid As Boolean = True
            

            ' 契約書印刷フラグの値チェック
            If Not Me.IsValidContPrintFlg(no, valueObj) Then
                isValid = False
            End If
            
            ' 支払方法区分の値チェック
            If Not Me.IsValidPaymentMethod(no, valueObj) Then
                isValid = False
            End If

            ' 頭金支払方法区分の値チェック
            If Not Me.IsValidDepositPaymentMethod(no, valueObj) Then
                isValid = False
            End If
            
            ' 保険区分の値チェック
            If Not Me.IsValidInsurance(no, valueObj) Then
                isValid = False
            End If
            
            ' チェック結果がNGの場合
            If Not isValid Then
                Me.ResultId = ErrCodeItValue + Me.ItemNumber(no)
                Throw New ArgumentException("", Me.Itemname(no))
            End If
                        
            Return valueObj
            
        End Function
        
        ''' <summary>
        ''' EstVclOptionInfoタグのデータを取得します。
        ''' </summary>
        ''' <param name="No">項目No</param>
        ''' <returns>XMLから取り出した値</returns>
        ''' <remarks>
        ''' XMLからデータを取り出し、必須／属性／サイズチェックを実施します。
        ''' </remarks>
        Private Function GetEstVclOptionInfoElementValue(ByVal no As Short) As Object
            
            ' 返却するオブジェクトを取得
            Dim valueObj As Object = Me.GetElementValue(no)
            
            ' チェック結果
            Dim isValid As Boolean = True
            
            ' 車両オプション区分の値チェック
            If Not Me.IsValidOptionPart(no, valueObj) Then
                isValid = False
            End If
            
            ' チェック結果がNGの場合
            If Not isValid Then
                Me.ResultId = ErrCodeItValue + Me.ItemNumber(no)
                Throw New ArgumentException("", Me.Itemname(no))
            End If
            
            Return valueObj
            
        End Function
        
        ''' <summary>
        ''' Customerタグのデータ取得
        ''' </summary>
        ''' <param name="no"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function GetUpdCustomerElementValue(ByVal no As Short) As Object
            
            ' 返却するオブジェクトを取得
            Dim valueObj As Object = Me.GetElementValue(no)
            
            'チェック結果
            Dim isValid As Boolean = True

            ' 顧客区分の値チェック
            If Not Me.IsValidCustomerType(no, valueObj) Then
                isValid = False
            End If

            ' 性別区分の値チェック
            If Not Me.IsValidSex(no, valueObj) Then
                isValid = False
            End If

            ' チェック結果がNGの場合
            If Not isValid Then
                Me.ResultId = ErrCodeItValue + Me.ItemNumber(no)
                Throw New ArgumentException("", Me.Itemname(no))
            End If
            
            Return valueObj
            
        End Function
        
        ''' <summary>
        ''' Customer_Userタグのデータ取得
        ''' </summary>
        ''' <param name="no"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function GetCustomerUserlementValue(ByVal no As Short) As Object
            
            ' 返却するオブジェクトを取得
            Dim valueObj As Object = Me.GetElementValue(no)
            
            'チェック結果
            Dim isValid As Boolean = True
            
            ' 顧客区分の値チェック
            If Not Me.IsValidCustomerType(no, valueObj) Then
                isValid = False
            End If

            ' 性別区分の値チェック
            If Not Me.IsValidSex(no, valueObj) Then
                isValid = False
            End If
            
            ' チェック結果がNGの場合
            If Not isValid Then
                Me.ResultId = ErrCodeItValue + Me.ItemNumber(no)
                Throw New ArgumentException("", Me.Itemname(no))
            End If
            
            Return valueObj
            
        End Function

#End Region
        
#Region "XML内のデータ取得"
        ''' <summary>
        ''' XML内のデータを取得します。
        ''' </summary>
        ''' <param name="No">項目No</param>
        ''' <returns>XMLから取り出した値</returns>
        ''' <remarks>
        ''' XMLからデータを取り出し、必須／属性／サイズチェックを実施します。
        ''' </remarks>
        Private Function GetElementValue(ByVal no As Short) As Object
            
            ' 返却するオブジェクト
            Dim valueObj As Object = Nothing

            Try
                '指定タグのNodeListを取得する
                Dim node As XmlNodeList = Me.NodeElement.GetElementsByTagName(Itemname(no))

                '指定したタグの存在有無により値をSet
                Dim valueString As String = String.Empty
                If node.Count > 0 Then
                    '指定したタグが存在したのでInnerTextプロパティで値を取得する
                    valueString = RTrim(node.Item(0).InnerText)
                    'タグ有
                    Me.TagPresence = IC3070203BusinessLogic.TagPresenceYes
                Else
                    valueString = ""
                    'タグ無
                    Me.TagPresence = IC3070203BusinessLogic.TagPresenceNo
                End If

                '文字列格納
                valueObj = valueString

                ' 必須項目チェック
                If CheckRequired = Chkrequiredflg(no) Then
                    If valueString.Length = 0 Then
                        Me.ResultId = ErrCodeItMust + Me.ItemNumber(no)
                        Throw New ArgumentException("", Me.Itemname(no))
                    End If
                End If
                
                Dim itemSizeDbl As Double = Me.Itemsize(no)
                Dim itemSizeStr As String = Me.Itemsize(no).ToString(CultureInfo.InvariantCulture)
                    
                ' 属性別のチェック
                Select Case Attribute(no)
                    
                    Case AttributeByte
                        ' 属性：Byteチェック
                        
                        If valueString.Length > 0 Then
                            If Not Validation.IsCorrectByte(valueString, itemSizeDbl) Then
                                Me.ResultId = ErrCodeItSize + Me.ItemNumber(no)
                                Throw New ArgumentException("", Me.Itemname(no))
                            End If
                        End If
                        
                    Case AttributeLegth
                        ' 属性：文字数チェック
                        
                        If valueString.Length > 0 Then
                            If Not Validation.IsCorrectDigit(valueString, itemSizeDbl) Then
                                Me.ResultId = ErrCodeItSize + Me.ItemNumber(no)
                                Throw New ArgumentException("", Me.Itemname(no))
                            End If
                        End If
                        
                    Case AttributeNum
                        ' 属性：Numericチェック
                        
                        ' 空の場合はDBNull値をセット
                        If valueString = "" Then
                            valueObj = Convert.DBNull
                        Else
                            Dim utf8 As New UTF8Encoding
                            
                            ' 全角文字はエラー
                            If valueString.Length <> utf8.GetByteCount(valueString) Then
                                Me.ResultId = ErrCodeItType + Me.ItemNumber(no)
                                Throw New ArgumentException("", Me.Itemname(no))
                            End If
                            ' 数値型
                            If Not IsNumeric(valueString) Then
                                Me.ResultId = ErrCodeItType + Me.ItemNumber(no)
                                Throw New ArgumentException("", Me.Itemname(no))
                            Else
                                ' 小数の桁数を取得
                                Dim dec As Integer
                                If itemSizeStr.IndexOf(".", StringComparison.OrdinalIgnoreCase) > 0 Then
                                    dec = CInt(Mid(itemSizeStr, itemSizeStr.IndexOf(".", StringComparison.OrdinalIgnoreCase) + 2))
                                Else
                                    dec = 0
                                End If

                                ' 整数部分をチェック
                                If Math.Abs(Int(CDec(valueString))).ToString(CultureInfo.InvariantCulture).Length > Int(itemSizeDbl) - dec Then
                                    Me.ResultId = ErrCodeItSize + Me.ItemNumber(no)
                                    Throw New ArgumentException("", Me.Itemname(no))
                                Else
                                    ' 小数チェック存在時に小数部分の桁数をチェック(小数点が存在するときのみ)
                                    If valueString.IndexOf(".", StringComparison.OrdinalIgnoreCase) > 0 Then
                                        ' 小数部分を取得し、小数桁をチェック
                                        If Mid(valueString, valueString.IndexOf(".", StringComparison.OrdinalIgnoreCase) + 2).Length > dec Then
                                            If dec = 0 Then
                                                Me.ResultId = ErrCodeItType + Me.ItemNumber(no)
                                                Throw New ArgumentException("", Me.Itemname(no))
                                            Else
                                                Me.ResultId = ErrCodeItSize + Me.ItemNumber(no)
                                                Throw New ArgumentException("", Me.Itemname(no))
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                            
                            utf8 = Nothing
                        End If
                        
                    Case AttributeDate
                        ' 属性：Dateチェック
                        
                        ' 空の場合はDBNull値をセット
                        If valueString = "" Then
                            valueObj = Convert.DBNull
                        Else
                            ' 指定されたフォーマットの日付書式か
                            valueObj = ConvertDateTime(valueString, FormatDate, ErrCodeItType + Me.ItemNumber(no))
                        End If

                    Case AttributeDatetime
                        ' 属性：DateTimeチェック
                        
                        ' 空の場合はDBNull値をセット
                        If valueString = "" Then
                            valueObj = Convert.DBNull
                        Else
                            ' 指定されたフォーマットの日付時刻書式か
                            valueObj = ConvertDateTime(valueString, FormatDatetime, ErrCodeItType + Me.ItemNumber(no))
                        End If
                        
                    Case Else
                        ' 属性：不明な属性
                        Me.ResultId = ErrCodeSys
                        Throw New ArgumentOutOfRangeException(Me.Itemname(no), valueObj, "Invalid Attribute kind")
                End Select
                
            Catch ex As Exception
                If Me.ResultId = ErrCodeSuccess Then
                    Me.ResultId = ErrCodeSys
                End If
                Throw
            End Try
            
            ' 結果を返却
            Return valueObj
            
        End Function
        
        ''' <summary>
        ''' 日付の書式に合わせて変換を行う。
        ''' </summary>
        ''' <param name="valueString">XMLの取り出し値（Check String）</param>
        ''' <param name="FormatDate">日付/時刻のフォーマット書式</param>
        ''' <param name="ErrNumber">エラーコード</param>
        ''' <returns>XMLから取り出した値</returns>
        ''' <remarks></remarks>
        Private Function ConvertDateTime(ByVal valueString As String, ByVal formatDate As String, ByVal errNumber As Short) As Object
            
            Try
                ' 指定されたフォーマット書式の日付に変換
                Return DateTime.ParseExact(valueString, formatDate, Nothing)
            Catch ex As Exception
                Me.ResultId = errNumber
                Throw
            End Try

        End Function
#End Region

#Region "各タグの値チェック"
        ''' <summary>
        ''' 契約書印刷フラグの値チェック
        ''' </summary>
        ''' <param name="no">項目No</param>
        ''' <param name="valueObj">値</param>
        ''' <returns>True：チェックOK、False：チェックNG</returns>
        ''' <remarks>許容値："0" or "1" or ""</remarks>
        Private Function IsValidContPrintFlg(ByVal no As Short, ByVal valueObj As Object) As Boolean
            
            Dim isValid As Boolean = False

            If Me.ItemNumber(no) <> TagEstContPrintFlg Then
                isValid = True
            ElseIf valueObj.Equals("0") Or valueObj.Equals("1") Or valueObj.Equals("") Then
                isValid = True
            End If

            Return isValid

        End Function

        ''' <summary>
        ''' 支払方法区分の値チェック
        ''' </summary>
        ''' <param name="no">項目No</param>
        ''' <param name="valueObj">値</param>
        ''' <returns>True：チェックOK、False：チェックNG</returns>
        ''' <remarks>許容値："1" or "2" or ""</remarks>
        Private Function IsValidPaymentMethod(ByVal no As Short, ByVal valueObj As Object) As Boolean
            
            Dim isValid As Boolean = False

            If Me.ItemNumber(no) <> TagEstPaymentStyle Then
                isValid = True
            ElseIf valueObj.Equals("1") Or valueObj.Equals("2") Or valueObj.Equals("") Then
                isValid = True
            End If

            Return isValid

        End Function
        
        ''' <summary>
        ''' 頭金支払方法区分の値チェック
        ''' </summary>
        ''' <param name="no">項目No</param>
        ''' <param name="valueObj">値</param>
        ''' <returns>True：チェックOK、False：チェックNG</returns>
        ''' <remarks>許容値："1" or "2" or ""</remarks>
        Private Function IsValidDepositPaymentMethod(ByVal no As Short, ByVal valueObj As Object) As Boolean
            
            Dim isValid As Boolean = False

            If Me.ItemNumber(no) <> TagEstDepositPaymentStyle Then
                isValid = True
            ElseIf valueObj.Equals("1") Or valueObj.Equals("2") Or valueObj.Equals("") Then
                isValid = True
            End If

            Return isValid

        End Function
        
        ''' <summary>
        ''' 保険区分の値チェック
        ''' </summary>
        ''' <param name="no">項目No</param>
        ''' <param name="valueObj">値</param>
        ''' <returns>True：チェックOK、False：チェックNG</returns>
        ''' <remarks>許容値："1" or "2" or ""</remarks>
        Private Function IsValidInsurance(ByVal no As Short, ByVal valueObj As Object) As Boolean
            
            Dim isValid As Boolean = False

            If Me.ItemNumber(no) <> TagEstInsurance Then
                isValid = True
            ElseIf valueObj.Equals("1") Or valueObj.Equals("2") Or valueObj.Equals("") Then
                isValid = True
            End If

            Return isValid

        End Function
        
        ''' <summary>
        ''' オプション区分の値チェック
        ''' </summary>
        ''' <param name="no">項目No</param>
        ''' <param name="valueObj">値</param>
        ''' <returns>True：チェックOK、False：チェックNG</returns>
        ''' <remarks>許容値："1" or "2" or "9"</remarks>
        Private Function IsValidOptionPart(ByVal no As Short, ByVal valueObj As Object) As Boolean
            
            Dim isValid As Boolean = False

            If Me.ItemNumber(no) <> TagEstOptionPart Then
                isValid = True
            ElseIf valueObj.Equals("1") Or valueObj.Equals("2") Or valueObj.Equals("9") Then
                isValid = True
            End If

            Return isValid

        End Function

        ''' <summary>
        ''' 顧客区分の値チェック
        ''' </summary>
        ''' <param name="no">項目No</param>
        ''' <param name="valueObj">値</param>
        ''' <returns>True：チェックOK、False：チェックNG</returns>
        ''' <remarks>許容値："0" or "1" or ""</remarks>
        Private Function IsValidCustomerType(ByVal no As Short, ByVal valueObj As Object) As Boolean
            
            Dim isValid As Boolean = False
            
            If Me.ItemNumber(no) <> TagCustomerType Then
                isValid = True
            ElseIf valueObj.Equals("0") Or valueObj.Equals("1") Or valueObj.Equals("") Then
                isValid = True
            End If

            Return isValid

        End Function

        ''' <summary>
        ''' 性別区分の値チェック
        ''' </summary>
        ''' <param name="no">項目No</param>
        ''' <param name="valueObj">値</param>
        ''' <returns>True：チェックOK、False：チェックNG</returns>
        ''' <remarks>許容値："0" or "1" or "2" or " " or ""</remarks>
        Private Function IsValidSex(ByVal no As Short, ByVal valueObj As Object) As Boolean
            
            Dim isValid As Boolean = False

            If Me.ItemNumber(no) <> TagSex Then
                isValid = True
            ElseIf valueObj.Equals("0") Or valueObj.Equals("1") Or valueObj.Equals("2") Or _
                valueObj.Equals(" ") Or valueObj.Equals("") Then
                isValid = True
            End If

            Return isValid

        End Function


#End Region

#Region "デフォルト値設定"
        ''' <summary>
        ''' 値が空の場合、初期値を設定します。
        ''' </summary>
        ''' <param name="no">項目No</param>
        ''' <param name="valueObj">値</param>
        ''' <returns>デフォルト値</returns>
        ''' <remarks></remarks>
        Private Function SetDefaultValue(ByVal no As Short, ByVal valueObj As Object) As Object
            
            If Not String.IsNullOrEmpty(Me.DefaultValue(no)) Then
                If String.IsNullOrEmpty(valueObj) Then
                    valueObj = Me.DefaultValue(no)
                End If
            End If
            
            Return valueObj
        End Function
#End Region
        
#Region "応答用XML作成"
        ''' <summary>
        ''' 応答用インターフェイスを返却します。
        ''' </summary>
        ''' <param name="receptionDate">受信日時</param>
        ''' <param name="retMessage">メッセージ</param>
        ''' <returns>応答用インターフェイス</returns>
        ''' <remarks></remarks>
        Private Function GetResponseXml(ByVal receptionDate As String, ByVal retMessage As String) As Response
            
            ' システム日付を取得する
            Dim transmissionDate As String = DateTimeFunc.Now().ToString(FormatDatetime, CultureInfo.InvariantCulture)
            
            ' Responseクラス生成
            Dim iResponse As Response = New Response()
            
            ' Headerクラスに値をセット
            Dim iRespHead As Response.Root_Head = New Response.Root_Head()
            iRespHead.MessageID = MessageId
            iRespHead.ReceptionDate = receptionDate
            iRespHead.TransmissionDate = transmissionDate
            
            ' Detailクラス生成
            Dim iRespDetail As Response.Root_Detail = New Response.Root_Detail()
            
            ' Commonクラスに値をセット
            Dim iRespCommon As Response.Root_Detail.Detail_Common = New Response.Root_Detail.Detail_Common()
            iRespCommon.ResultId = Me.ResultId.ToString(CultureInfo.InvariantCulture)
            iRespCommon.Message = retMessage
                
            ' EstimationInfoクラスに値をセット
            Dim iRespEst As Response.Root_Detail.Detail_EstimationInfo = New Response.Root_Detail.Detail_EstimationInfo()
            iRespEst.EstimateId = Me.EstimateId.ToString(CultureInfo.InvariantCulture)
                
            ' DetailクラスにCommon、EstimationInfoをセット
            iRespDetail.Common = iRespCommon
            iRespDetail.EstimationInfo = iRespEst
            
            ' ResponseクラスにHeader、Detailをセット
            iResponse.Head = iRespHead
            iResponse.Detail = iRespDetail
            
            Return iResponse

        End Function
#End Region
        
#Region "応答用XMLインターフェイス"
        ''' <summary>
        ''' 応答用XMLのインターーフェイスクラス
        ''' </summary>
        ''' <remarks></remarks>
        <System.Xml.Serialization.XmlRoot("Response", Namespace:="http://tempuri.org/Response.xsd")> _
        Public Class Response

            ''' <summary>
            ''' Headタグ
            ''' </summary>
            ''' <remarks></remarks>
            Public Class Root_Head
                <System.Xml.Serialization.XmlElementAttribute(ElementName:="MessageID", IsNullable:=False)> _
                Private prpMessageID As String
                <System.Xml.Serialization.XmlElementAttribute(ElementName:="ReceptionDate", IsNullable:=False)> _
                Private prpReceptionDate As String
                <System.Xml.Serialization.XmlElementAttribute(ElementName:="TransmissionDate", IsNullable:=False)> _
                Private prpTransmissionDate As String
                
                ''' <summary>
                ''' メッセージID
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                Public Property MessageID() As String
                    Set(ByVal value As String)
                        prpMessageID = value
                    End Set
                    Get
                        Return prpMessageID
                    End Get
                End Property
                
                ''' <summary>
                ''' 受信日時
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                Public Property ReceptionDate() As String
                    Set(ByVal value As String)
                        prpReceptionDate = value
                    End Set
                    Get
                        Return prpReceptionDate
                    End Get
                End Property
                
                ''' <summary>
                ''' 送信日時
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                Public Property TransmissionDate() As String
                    Set(ByVal value As String)
                        prpTransmissionDate = value
                    End Set
                    Get
                        Return prpTransmissionDate
                    End Get
                End Property
                    
            End Class

            ''' <summary>
            ''' Detailタグ
            ''' </summary>
            ''' <remarks></remarks>
            Public Class Root_Detail

                ''' <summary>
                ''' Commonタグ
                ''' </summary>
                ''' <remarks></remarks>
                Public Class Detail_Common
                    <System.Xml.Serialization.XmlElementAttribute(ElementName:="ResultId", IsNullable:=False)> _
                    Private prpResultId As String
                    <System.Xml.Serialization.XmlElementAttribute(ElementName:="Message", IsNullable:=False)> _
                    Private prpMessage As String
                    
                    ''' <summary>
                    ''' 終了コード
                    ''' </summary>
                    ''' <value></value>
                    ''' <returns></returns>
                    ''' <remarks></remarks>
                    Public Property ResultId() As String
                        Set(ByVal value As String)
                            prpResultId = value
                        End Set
                        Get
                            Return prpResultId
                        End Get
                    End Property

                    ''' <summary>
                    ''' メッセージ
                    ''' </summary>
                    ''' <value></value>
                    ''' <returns></returns>
                    ''' <remarks></remarks>
                    Public Property Message() As String
                        Set(ByVal value As String)
                            prpMessage = value
                        End Set
                        Get
                            Return prpMessage
                        End Get
                    End Property
                End Class

                ''' <summary>
                ''' EstimationInfoタグ
                ''' </summary>
                ''' <remarks></remarks>
                Public Class Detail_EstimationInfo
                    <System.Xml.Serialization.XmlElementAttribute(ElementName:="EstimateId", IsNullable:=False)> _
                    Private prpEstimateId As String

                    ''' <summary>
                    ''' 見積管理ID
                    ''' </summary>
                    ''' <value></value>
                    ''' <returns></returns>
                    ''' <remarks></remarks>
                    Public Property EstimateId() As String
                        Set(ByVal value As String)
                            prpEstimateId = value
                        End Set
                        Get
                            Return prpEstimateId
                        End Get
                    End Property
                End Class

                <System.Xml.Serialization.XmlElementAttribute(ElementName:="Common", IsNullable:=False)> _
                Private prpCommon As Detail_Common
                <System.Xml.Serialization.XmlElementAttribute(ElementName:="EstimationInfo", IsNullable:=False)> _
                Private prpEstimationInfo As Detail_EstimationInfo

                ''' <summary>
                ''' Commonタグ
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                Public Property Common() As Detail_Common
                    Set(ByVal value As Detail_Common)
                        prpCommon = value
                    End Set
                    Get
                        Return prpCommon
                    End Get
                End Property

                ''' <summary>
                ''' Estimationタグ
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                Public Property EstimationInfo() As Detail_EstimationInfo
                    Set(ByVal value As Detail_EstimationInfo)
                        prpEstimationInfo = value
                    End Set
                    Get
                        Return prpEstimationInfo
                    End Get
                End Property
            End Class

            <System.Xml.Serialization.XmlElementAttribute(ElementName:="Head", IsNullable:=False)> _
            Private prpHead As Root_Head
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="Detail", IsNullable:=False)> _
            Private prpDetail As Root_Detail

            
            ''' <summary>
            ''' Headerタグ
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Property Head() As Root_Head
                Set(ByVal value As Root_Head)
                    prpHead = value
                End Set
                Get
                    Return prpHead
                End Get
            End Property
            
            ''' <summary>
            ''' Detailタグ
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Property Detail() As Root_Detail
                Set(ByVal value As Root_Detail)
                    prpDetail = value
                End Set
                Get
                    Return prpDetail
                End Get
            End Property
        End Class
#End Region
        
#End Region

    End Class
#End Region

End Namespace
