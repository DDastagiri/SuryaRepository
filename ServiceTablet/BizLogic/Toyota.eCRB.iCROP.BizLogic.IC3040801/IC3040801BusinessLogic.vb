'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'IC3040801BusinessLogic.vb
'─────────────────────────────────────
'機能： 通知登録インターフェース
'補足： 
'作成： 2012/02/01 KN 小澤
'更新： 2012/03/02 KN 佐藤 【SERVICE_1】エラーの戻り値を修正
'更新： 2012/03/05 KN 佐藤 【SERVICE_1】置換用文字列を修正（"%USRE%"→"%USER%"）
'更新： 2012/03/05 KN 佐藤 【SERVICE_1】コメント見直し
'更新： 2012/03/24 KN 小澤 【SALES_1】自分→自分の場合、未読にする修正
'更新： 2012/03/26 KN 小澤 【SALES_2】Push通信する際に表示文字数が256バイトを
'                     超えていた場合は「･･･」または「...」を表示するように修正
'更新： 2012/04/06 KN 小澤 【SALES_2】「･･･」または「...」の文言取得の番号を「37」から「39」に修正
'更新： 2012/05/25 KN 長谷 【SERVICE_1】対応ステータスの追加
'更新： 2012/06/06 KN 彭    コード分析対応（不要な引数、変数を削除）
'更新： 2012/09/05 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.20）
'更新： 2013/12/10 TMEJ 加藤(宏) TMEJ次世代サービス 工程管理機能開発
'更新： 
'─────────────────────────────────────

Imports System.Xml
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.Tool.Notify.Api.DataAccess
Imports System.Globalization
Imports System.IO
Imports System.Text
Imports System.Xml.Serialization
Imports Toyota.eCRB.Visit.Api.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.Tool.Notify.Api.DataAccess.ConstCode

' $01 start step2開発
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.Visit.Api.DataAccess.VisitUtilityDataSet
' $01 end   step2開発

''' <summary>
''' IC3040801
''' 通知DBのAPIクラス
''' </summary>
''' <remarks></remarks>
Public Class IC3040801BusinessLogic
    Inherits BaseBusinessComponent
    Implements IDisposable

#Region "共通変数"

    ''' <summary>
    ''' DACクラス
    ''' </summary>
    Private Property daDataSetTableAdapters As IC3040801DataSetTableAdapters.IC3040801TableAdapters

    ''' <summary>
    ''' エラー内容格納クラス
    ''' </summary>
    Private Property errorInfo As XmlCommon

    ''' <summary>
    ''' 通知依頼情報登録のデータテーブル
    ''' </summary>
    Private Property dtNoticeRequest As IC3040801DataSet.IC3040801NoticeRequestDataTable

    ''' <summary>
    ''' 通知情報登録のデータテーブル
    ''' </summary>
    Private Property dtNoticeInfo As IC3040801DataSet.IC3040801NoticeInfoDataTable

    ''' <summary>
    ''' 通知情報取得のデータテーブル
    ''' </summary>
    Private Property dtSelectNoticeInfo As IC3040801DataSet.IC3040801SelectNoticeInfoDataTable

    ''' <summary>
    ''' 通知履歴取得のデータテーブル
    ''' </summary>
    Private Property dtSelectNoticeRequest As IC3040801DataSet.IC3040801SelectNoticeRequestDataTable

    ''' <summary>
    ''' 受信先情報格納クラス
    ''' </summary>
    Private Property xmlAccountData As XmlAccount

    ''' <summary>
    ''' PushServer情報格納クラス
    ''' </summary>
    Private Property pushInfoData As XmlPushInfo

    ''' <summary>
    ''' 通知情報格納クラス
    ''' </summary>
    Private Property requestNoticeData As XmlRequestNotice

    ''' <summary>
    ''' 通知DBメソッド
    ''' </summary>
    Private Property noticeDBClone As IC3040801BusinessLogic

    ''' <summary>
    ''' Accountチェック(TRUE:受信先情報あり、FALSE:受信先情報なし)
    ''' </summary>
    Private Property accountCheck As Boolean

    ' ''' <summary>
    ' ''' 通知メッセージ
    ' ''' </summary>
    'Private Property noticeMessage As String

#End Region

#Region "共通定数"

    ''' <summary>機能ID</summary>
    Private Const C_SYSTEM As String = "IC3040801"

    ''' <summary>Response(成功)</summary>
    Private Const RESULTID_SUCCESS_CONST As String = "000000"
    Private Const MESSAGE_SUCCESS_CONST As String = "Success"

    ''' <summary>Response(失敗)</summary>
    Private Const RESULTID_FAILURE_CONST As String = "009999"
    Private Const MESSAGE_FAILURE_CONST As String = "Failure"

    ''' <summary>文字列</summary>
    Private Const PushSubJavaScript As String = "4"

    ''' <summary>Long初期値</summary>
    Private Const InitLong As Long = 0

    ''' <summary>Integer数値</summary>
    Private Const InitInteger As Integer = 0

    ''' <summary>XMLの要素内の要素を取得する際の先頭につけるもの</summary>
    Private Const XmlRootDirectory As String = "//"

    ' XML要素名一覧
    ''' <summary>RequestRegist要素</summary>
    Private Const XmlDataRequestRegist As String = "RequestRegist"

    ''' <summary>Head要素</summary>
    Private Const XmlDataHead As String = "Head"

    ''' <summary>TransmissionDate要素</summary>
    Private Const XmlDataTransmissionDate As String = "TransmissionDate"

    ''' <summary>ReceiveAccount要素</summary>
    Private Const XmlDataReceiveAccount As String = "ReceiveAccount"

    ''' <summary>Account要素</summary>
    Private Const XmlDataAccount As String = "Account"

    ''' <summary>ToAccount要素</summary>
    Private Const XmlDataToAccount As String = "ToAccount"

    ''' <summary>ToClientID要素</summary>
    Private Const XmlDataToClientId As String = "ToClientId"

    ''' <summary>ToAccountName要素</summary>
    Private Const XmlDataToAccountName As String = "ToAccountName"

    ''' <summary>Detail要素</summary>
    Private Const XmlDataDetail As String = "Detail"

    ''' <summary>RequestNotice要素</summary>
    Private Const XmlDataRequestNotice As String = "RequestNotice"

    ''' <summary>DealerCode要素</summary>
    Private Const XmlDataDealerCode As String = "DealerCode"

    ''' <summary>StoreCode要素</summary>
    Private Const XmlDataStoreCode As String = "StoreCode"

    ''' <summary>RequestClass要素</summary>
    Private Const XmlDataRequestClass As String = "RequestClass"

    ''' <summary>Status要素</summary>
    Private Const XmlDataStatus As String = "Status"

    ''' <summary>RequestID要素</summary>
    Private Const XmlDataRequestId As String = "RequestId"

    ''' <summary>RequestClassID要素</summary>
    Private Const XmlDataRequestClassId As String = "RequestClassId"

    ''' <summary>FromAccount要素</summary>
    Private Const XmlDataFromAccount As String = "FromAccount"

    ''' <summary>FromClientID要素</summary>
    Private Const XmlDataFromClientId As String = "FromClientId"

    ''' <summary>FromAccountName要素</summary>
    Private Const XmlDataFromAccountName As String = "FromAccountName"

    ''' <summary>CustomID要素</summary>
    Private Const XmlDataCustomId As String = "CustomId"

    ''' <summary>CustomName要素</summary>
    Private Const XmlDataCustomName As String = "CustomName"

    ''' <summary>CustomerClass要素</summary>
    Private Const XmlDataCustomerClass As String = "CustomerClass"

    ''' <summary>CustomerKind要素</summary>
    Private Const XmlDataCustomerKind As String = "CustomerKind"

    ''' <summary>Message要素</summary>
    Private Const XmlDataMessage As String = "Message"

    ''' <summary>SessionValue要素</summary>
    Private Const XmlDataSessionValue As String = "SessionValue"

    ''' <summary>SalesStaffCode要素</summary>
    Private Const XmlDataSalesStaffCode As String = "SalesStaffCode"

    ''' <summary>VehicleSequenceNumber要素</summary>
    Private Const XmlDataVehicleSequenceNumber As String = "VehicleSequenceNumber"

    ''' <summary>FollowUpBoxStoreCode要素</summary>
    Private Const XmlDataFollowUpBoxStoreCode As String = "FollowUpBoxStoreCode"

    ''' <summary>FollowUpBoxNumber要素</summary>
    Private Const XmlDataFollowUpBoxNumber As String = "FollowUpBoxNumber"
    
    ' $01 start step2開発
    ''' <summary>CSPaperName要素</summary>
    Private Const XmlDataCSPaperName As String = "CSPaperName"
    ' $01 end   step2開発

    ''' <summary>PushInfo要素</summary>
    Private Const XmlDataPushInfo As String = "PushInfo"

    ''' <summary>PuchCategory要素</summary>
    Private Const XmlDataPushCategory As String = "PushCategory"

    ''' <summary>PositionType要素</summary>
    Private Const XmlDataPositionType As String = "PositionType"

    ''' <summary>Time要素</summary>
    Private Const XmlDataTime As String = "Time"

    ''' <summary>DispType要素</summary>
    Private Const XmlDataDisplayType As String = "DisplayType"

    ''' <summary>DispContents要素</summary>
    Private Const XmlDataDisplayContents As String = "DisplayContents"

    ''' <summary>Color要素</summary>
    Private Const XmlDataColor As String = "Color"

    ''' <summary>PopWidth要素</summary>
    Private Const XmlDataPopWidth As String = "PopWidth"

    ''' <summary>PopHeight要素</summary>
    Private Const XmlDataPopHeight As String = "PopHeight"

    ''' <summary>PopX要素</summary>
    Private Const XmlDataPopX As String = "PopX"

    ''' <summary>PopY要素</summary>
    Private Const XmlDataPopY As String = "PopY"

    ''' <summary>DispFunction要素</summary>
    Private Const XmlDataDisplayFunction As String = "DisplayFunction"

    ''' <summary>ActionFunction要素</summary>
    Private Const XmlDataActionFunction As String = "ActionFunction"

    ''' <summary>
    ''' ステータス
    ''' </summary>
    ''' <remarks></remarks>
    Private Enum Status As Integer

        ''' <summary>None</summary>
        None = 0

        ''' <summary>依頼</summary>
        RequestStatus = 1

        ''' <summary>キャンセル</summary>
        CancelStatus = 2

        ''' <summary>受信</summary>
        GetStatus = 3

        ''' <summary>受付</summary>
        AcceptanceStatus = 4

    End Enum

    ''' <summary>
    ''' XMLの要素のデータの割り当てを指定します。
    ''' </summary>
    ''' <remarks></remarks>
    Private Enum DataAssignment As Integer

        ''' <summary>必須項目</summary>
        ModeMandatory

        ''' <summary>オプション項目</summary>
        ModeOptional

    End Enum

    ''' <summary>
    ''' 文字チェックを行う際の型を指します
    ''' </summary>
    ''' <remarks></remarks>
    Private Enum TypeConversion As Integer

        ''' <summary>チェックをしない</summary>
        None

        ''' <summary>文字列チェックを行う</summary>
        StringType

        ''' <summary>数値チェックを行う</summary>
        IntegerType

        ''' <summary>日付チェックを行う</summary>
        DateType

    End Enum


    ''' <summary>通知依頼種別：査定</summary>
    Private Const NoticeClassAssessment As String = "01"
    ''' <summary>通知依頼種別：価格相談</summary>
    Private Const NoticeClassPriceConsultation As String = "02"
    ''' <summary>通知依頼種別：ヘルプ</summary>
    Private Const NoticeClassHelp As String = "03"
    ''' <summary>通知依頼種別：来店</summary>
    Private Const NoticeClassComingStore As String = "04"

    ' $01 start step2開発
    ''' <summary>通知依頼種別：苦情</summary>
    Private Const NoticeClassClaim As String = "05"
    ''' <summary>通知依頼種別：CSSurvey</summary>
    Private Const NoticeClassCSSurvey As String = "06"
    ' $01 end   step2開発

    ''' <summary>
    ''' エラー番号
    ''' </summary>
    ''' <remarks></remarks>
    Private Enum ReturnCode As Integer

        ' グループエラー、要素の値に＋して使用します　

        ''' <summary>項目必須エラー</summary>
        NotXmlElementError = 2000

        ''' <summary>項目型エラー</summary>
        XmlParseError = 3000

        ''' <summary>項目サイズエラー</summary>
        XmlMaximumOfDigitError = 4000

        ''' <summary>値チェックエラー</summary>
        XmlValueCheckError = 5000

        ''' <summary>DBタイムアウトエラー</summary>
        TimeOutError = 6000

        ''' <summary>DB更新エラー</summary>
        UpdateError = 6010

        ''' <summary>DBエラー</summary>
        DatabaseError = 9000

        ''' <summary>正常終了</summary>
        Successful = 0

        ''' <summary>XMLタグ不正エラー</summary>
        XmlIncorrect = -1

        ''' <summary>スタッフコードチェックエラー</summary>
        StaffCodeError = 1

        ''' <summary>エラーコード：システムエラー</summary>
        ErrCodeSys = 9999

    End Enum

    ''' <summary>
    ''' タグ番号
    ''' </summary>
    ''' <remarks></remarks>
    Private Enum ElementName As Integer

        ''' <summary>None</summary>
        None = 0

        ''' <summary>RequestRegist要素</summary>
        RequestRegist = 1001

        ''' <summary>Head要素</summary>
        Head = 1002

        ''' <summary>TransmissionDate要素</summary>
        TransmissionDate = 1

        ''' <summary>ReceiveAccount要素</summary>
        ReceiveAccount = 1003

        ''' <summary>Account要素</summary>
        Account = 1004

        ''' <summary>ToAccount要素</summary>
        ToAccount = 101

        ''' <summary>ToClientId要素</summary>
        ToClientId = 102

        ''' <summary>ToAccountName要素</summary>
        ToAccountName = 103

        ''' <summary>Detail要素</summary>
        Detail = 1005

        ''' <summary>RequestNotice要素</summary>
        RequestNotice = 1006

        ''' <summary>DealerCode要素</summary>
        DealerCode = 201

        ''' <summary>StoreCode要素</summary>
        StoreCode = 202

        ''' <summary>RequestClass要素</summary>
        RequestClass = 203

        ''' <summary>Status要素</summary>
        Status = 204

        ''' <summary>RequestId要素</summary>
        RequestId = 205

        ''' <summary>RequestClassId要素</summary>
        RequestClassId = 206

        ''' <summary>FromAccount要素</summary>
        FromAccount = 207

        ''' <summary>FromClientId要素</summary>
        FromClientId = 208

        ''' <summary>FromAccountName要素</summary>
        FromAccountName = 209

        ''' <summary>CustomId要素</summary>
        CustomId = 210

        ''' <summary>CustomName要素</summary>
        CustomName = 211

        ''' <summary>CustomerClass要素</summary>
        CustomerClass = 212

        ''' <summary>CustomerKind要素</summary>
        CustomerKind = 213

        ''' <summary>Message要素</summary>
        Message = 214

        ''' <summary>SessionValue要素</summary>
        SessionValue = 215

        ''' <summary>SalesStaffCode要素</summary>
        SalesStaffCode = 216

        ''' <summary>VehicleSequenceNumber要素</summary>
        VehicleSequenceNumber = 217

        ''' <summary>FollowUpBoxStoreCode要素</summary>
        FollowUpBoxStoreCode = 218

        ''' <summary>FollowUpBoxNumber要素</summary>
        FollowUpBoxNumber = 219

        ' $01 start step2開発
        ''' <summary>CS PaperName要素</summary>
        CSPaperName = 220
        ' $01 end   step2開発

        ''' <summary>PushInfo要素</summary>
        PushInfo = 1007

        ''' <summary>PuchCategory要素</summary>
        PushCategory = 301

        ''' <summary>PositionType要素</summary>
        PositionType = 302

        ''' <summary>Time要素</summary>
        Time = 303

        ''' <summary>DisplayType要素</summary>
        DisplayType = 304

        ''' <summary>DisplayContents要素</summary>
        DisplayContents = 305

        ''' <summary>Color要素</summary>
        Color = 306

        ''' <summary>PopWidth要素</summary>
        PopWidth = 307

        ''' <summary>PopHeight要素</summary>
        PopHeight = 308

        ''' <summary>PopX要素</summary>
        PopX = 309

        ''' <summary>PopY要素</summary>
        PopY = 310

        ''' <summary>DisplayFunction要素</summary>
        DisplayFunction = 311

        ''' <summary>ActionFunction要素</summary>
        ActionFunction = 312

    End Enum

    ''' <summary>
    ''' cat
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PushConstCategoryText As String = "cat="
    Private Enum PushConstCategory As Integer

        ''' <summary>None</summary>
        none = 0
        ''' <summary>Popup</summary>
        popup = 1
        ''' <summary>Action</summary>
        action = 2

    End Enum

    ''' <summary>
    ''' type
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PushConstTypeText As String = "&type="
    Private Enum PushConstType As Integer

        ''' <summary>Main</summary>
        main = 0
        ''' <summary>Header</summary>
        header = 1
        ''' <summary>Bottom</summary>
        bottom = 2
        ''' <summary>Left</summary>
        left = 3
        ''' <summary>Right</summary>
        right = 4
        ''' <summary>Inside</summary>
        inside = 5

    End Enum

    ''' <summary>
    ''' sub
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PushConstSubText As String = "&sub="
    Private Enum PushConstSub As Integer

        ''' <summary>None</summary>
        none = 0
        ''' <summary>text</summary>
        text = 1
        ''' <summary>url</summary>
        url = 2
        ''' <summary>local</summary>
        local = 3
        ''' <summary>js</summary>
        js = 4

    End Enum

    ''' <summary>
    ''' uid
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PushConstUserIdText As String = "&uid="

    ''' <summary>
    ''' time
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PushConstTimeText As String = "&time="

    ''' <summary>
    ''' color
    ''' </summary>
    ''' <remarks></remarks>
    Private Enum PushConstColor As Integer

        ''' <summary>None</summary>
        none = 0
        ''' <summary>yellow</summary>
        yellow = 1
        ''' <summary>blue</summary>
        blue = 2

    End Enum
    ''' <summary>
    ''' 薄い黄色(249:237:190)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PushConstColorYellow As String = "&color=F9EDBE64"
    ''' <summary>
    ''' 薄い青色(203:232:255)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PushConstColorBlue As String = "&color=CBE8FF64"

    ''' <summary>
    ''' height
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PushConstHeightText As String = "&height="

    ''' <summary>
    ''' width
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PushConstWidthText As String = "&width="

    ''' <summary>
    ''' pox
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PushConstPositionXText As String = "&pox="

    ''' <summary>
    ''' poy
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PushConstPositionYText As String = "&poy="

    ''' <summary>
    ''' Contents
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PushConstContentsMessage As String = "&msg="
    Private Const PushConstContentsUrl As String = "&url="
    Private Const PushConstContentsFileName As String = "&fname="

    ''' <summary>
    ''' js1
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PushConstJavaScript1Text As String = "&js1="

    ''' <summary>
    ''' js2
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PushConstJavaScript2Text As String = "&js2="

    ' 2012/03/05 KN 佐藤 【SERVICE_1】置換用文字列を修正（"%USRE%"→"%USER%"） START
    ''' <summary>
    ''' 置換用(account)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ReplaceAccount As String = "%USER%"
    'Private Const ReplaceAccount As String = "%USRE%"
    ' 2012/03/05 KN 佐藤 【SERVICE_1】置換用文字列を修正（"%USRE%"→"%USER%"） END

    ''' <summary>
    ''' 置換用(message)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ReplaceMessage As String = "%MESSAGE%"

    ''' <summary>
    ''' 置換用(顧客)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PermutationCustNothing As String = "<a href="""" onClick=""return SalesLinkClick(event,1)"">%CUST%</a>"

    '2012/09/05 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.20） START
    ''' <summary>
    ''' 置換用(JS2)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ReplaceJavascript2 As String = "%JS2%"
    ''' <summary>
    ''' 置換用文字列カンマ無(JS2)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ReplaceJavascript2After As String = "icropScript.ui.openNoticeDialog()"
    ''' <summary>
    ''' 置換用文字列カンマ有(JS2)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ReplaceJavascript2AfterComma As String = ",icropScript.ui.openNoticeDialog()"
    '2012/09/05 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.20） END

    ''' <summary>
    ''' 既読フラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Enum Read As Integer

        ''' <summary>未読</summary>
        Unread = 0

        ''' <summary>既読</summary>
        Read = 1

    End Enum
    ' $01 start step2開発
#Region "顧客情報画面のPush送信用定数"

    ''' <summary>
    ''' JavaScript用のPushコマンド(cat)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PUSH_COMMAND_CAT = "cat=action"

    ''' <summary>
    ''' JavaScript用のPushコマンド(type)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PUSH_COMMAND_TYPE = "&type=main"

    ''' <summary>
    ''' JavaScript用のPushコマンド(sub)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PUSH_COMMAND_SUB = "&sub=js"

    ''' <summary>
    ''' JavaScript用のPushコマンド(uid)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PUSH_COMMAND_UID = "&uid="

    ''' <summary>
    ''' JavaScript用のPushコマンド(time)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PUSH_COMMAND_TIME = "&time=0"

    ''' <summary>
    ''' JavaScript用のPushコマンド(JS1)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PUSH_COMMAND_JS1 = "&js1="

    ''' <summary>
    ''' JavaScript用のPushコマンド(JS1)の置換用文字列
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PUSH_COMMAND_JS1_REPLACE = "%JS1_CMD%"

    ''' <summary>
    ''' 受付用JavaScript用のPushコマンド(JS1)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PUSH_COMMAND_JS1_RECEPTION = "SC3100101Update"

    ''' <summary>
    ''' SSV用JavaScript用のPushコマンド(JS1)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PUSH_COMMAND_JS1_SSV = "SC3210201Update"

    ''' <summary>
    ''' 査定、価格相談、ヘルプのキャンセル送信時のJavaScriptの1つ目の引数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PUSH2_CANCEL1 As String = "99"

    ''' <summary>
    ''' 査定、価格相談、ヘルプのキャンセル送信時のJavaScriptの2つ目引数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PUSH2_CANCEL2 As String = "01"

    ''' <summary>
    ''' 査定、価格相談、ヘルプのキャンセル以外送信時のJavaScriptの1つ目の引数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PUSH2_NOT_CANCEL1 As String = "03"

    ''' <summary>
    '''査定の送信時のJavaScriptの2つ目の引数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PUSH2_SEND_ASSESSMENT As String = "03"

    ''' <summary>
    '''査定の受信時のJavaScriptの2つ目の引数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PUSH2_RECEIVE_ASSESSMENT As String = "04"

    ''' <summary>
    '''価格相談の送信時のJavaScriptの2つ目の引数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PUSH2_SEND_PRICECONSULTATION As String = "05"

    ''' <summary>
    '''価格相談の回答時のJavaScriptの2つ目の引数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PUSH2_REPLY_PRICECONSULTATION As String = "06"

    ''' <summary>
    '''ヘルプの送信時のJavaScriptの2つ目の引数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PUSH2_SEND_HELP As String = "07"

    ''' <summary>
    '''ヘルプの受信時のJavaScriptの2つ目の引数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PUSH2_REPLY_HELP As String = "08"

    ''' <summary>
    ''' 操作権限コード（受付）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OperationCodeReception As Decimal = 51D

    ''' <summary>
    ''' 操作権限コード（セールスマネージャ）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OperationCodeSalesStaffManager As Decimal = 7D

    ''' <summary>
    ''' 操作権限コード（SSV）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OperationCodeSSV As Decimal = 53D

    ''' <summary>
    ''' 削除フラグ（未削除）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DeleteFlgNone As String = "0"

    ''' <summary>
    ''' ユーザマスタテーブルの在籍状態(大分類)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PresenceCategory As String = "PRESENCECATEGORY"

    ''' <summary>
    ''' ユーザマスタテーブルの在籍状態(大分類)が商談中のときの値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NEGOCICATING As String = "2"


#End Region
    ' $01 end   step2開発

#End Region

#Region "デフォルトコンストラクタ処理"

    ''' <summary>
    ''' デフォルトコンストラクタ処理
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        '処理なし
        Me.daDataSetTableAdapters = New IC3040801DataSetTableAdapters.IC3040801TableAdapters
        Me.errorInfo = New XmlCommon
        Me.dtNoticeRequest = New IC3040801DataSet.IC3040801NoticeRequestDataTable
        Me.dtNoticeInfo = New IC3040801DataSet.IC3040801NoticeInfoDataTable
        Me.dtSelectNoticeInfo = New IC3040801DataSet.IC3040801SelectNoticeInfoDataTable
        Me.dtSelectNoticeRequest = New IC3040801DataSet.IC3040801SelectNoticeRequestDataTable
        Me.xmlAccountData = New XmlAccount
        Me.pushInfoData = New XmlPushInfo
        Me.requestNoticeData = New XmlRequestNotice
        Me.accountCheck = True
    End Sub

#End Region

#Region "通知DB API"

    ''' <summary>
    ''' WebService用のメイン処理
    ''' </summary>
    ''' <param name="getXml">解析XML</param>
    ''' <param name="noticeDisposalMode">固有、汎用フラグ</param>
    ''' <returns>XMLを格納したResponse</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2012/09/05 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.20）
    ''' </history>
    Public Function Notice(ByVal getXml As String,
                           ByVal noticeDisposalMode As NoticeDisposal) As Response
        Logger.Info(GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, True))
        Logger.Info(GetLogParam("getXml", getXml, False) &
                    GetLogParam("noticeDisposalMode", CStr(noticeDisposalMode), True))

        Dim returnXml As New Response()

        Using xmlDataClass As New XmlNoticeData
            Try
                'XML解析処理
                GetXMLData(getXml, xmlDataClass, noticeDisposalMode)

                '通知DB処理
                Me.noticeDBClone = New IC3040801BusinessLogic
                If Me.accountCheck OrElse xmlDataClass.AccountList.Count <> 0 Then
                    Me.noticeDBClone.RegistsNoticeDB(xmlDataClass, noticeDisposalMode)
                Else
                    Me.noticeDBClone.RegistsNoticeDBNoAccount(xmlDataClass)
                End If

                'PushServer処理
                '2012/09/05 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.20） START
                'SendPushServer(xmlDataClass)
                Me.SendPushServer(xmlDataClass)
                '2012/09/05 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.20） END

                ' $01 start step2開発
                '固有の場合は、受付メイン画面更新用のPushServer処理を実施する
                If noticeDisposalMode = NoticeDisposal.Peculiar Then

                    SendPushServerForRefresh(xmlDataClass.RequestNotice.RequestClass,
                                   xmlDataClass.RequestNotice.Status,
                                   xmlDataClass.RequestNotice.DealerCode,
                                   xmlDataClass.RequestNotice.StoreCode,
                                   xmlDataClass.RequestNotice.FromAccount,
                                    xmlDataClass.AccountList)
                End If
                ' $01 end   step2開発

                '成功情報を格納
                Me.errorInfo.ResultId = RESULTID_SUCCESS_CONST
                Me.errorInfo.Message = MESSAGE_SUCCESS_CONST

            Catch ex As ArgumentException
                Logger.Error(ex.Message, ex)
                '失敗情報を格納
                Me.errorInfo.Message = MESSAGE_FAILURE_CONST

            Catch ex As OracleExceptionEx
                Logger.Error(ex.Message, ex)
                '失敗情報を格納
                Me.errorInfo.ResultId = Me.noticeDBClone.errorInfo.ResultId
                Me.errorInfo.Message = MESSAGE_FAILURE_CONST

            Catch ex As Exception
                Logger.Error(ex.Message, ex)
                '失敗情報を格納
                Me.errorInfo.ResultId = RESULTID_FAILURE_CONST
                Me.errorInfo.Message = MESSAGE_FAILURE_CONST

            Finally
                'XML作成
                Me.errorInfo.NoticeRequestId = xmlDataClass.RequestNotice.RequestId
                returnXml = CreateReturnXml()
                'ログ出力
                Using writer As New StringWriter(CultureInfo.InvariantCulture())
                    Dim outXml As New XmlSerializer(GetType(Response))
                    outXml.Serialize(writer, returnXml)
                    If RESULTID_SUCCESS_CONST.Equals(Me.errorInfo.ResultId) Then
                        '成功
                        Logger.Info(writer.ToString)
                    Else
                        '失敗
                        Logger.Error(getXml)
                        Logger.Error(writer.ToString)
                    End If
                End Using
            End Try
        End Using

        Logger.Info(GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, False))

        Return returnXml
    End Function

    ''' <summary>
    ''' 画面用のメイン処理
    ''' </summary>
    ''' <param name="xmlDataClass">通知情報</param>
    ''' <param name="noticeDisposalMode">固有、汎用フラグ</param>
    ''' <returns>戻り値情報</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2012/09/05 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.20）
    ''' </history>
    Public Function NoticeDisplay(ByVal xmlDataClass As XmlNoticeData,
                                  ByVal noticeDisposalMode As NoticeDisposal) As XmlCommon
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} START:noticeDisposalMode={2} " _
                , Me.GetType.ToString _
                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                , noticeDisposalMode))

        'Logger.Info(GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, True))
        'Logger.Info(LogNoticeData(xmlDataClass) &
        '            GetLogParam("noticeDisposalMode", CStr(noticeDisposalMode), True))

        Dim returnXml As New Response()

        Try
            '値チェック
            CheckXmlDataClass(xmlDataClass, noticeDisposalMode)

            '通知DB処理
            Me.noticeDBClone = New IC3040801BusinessLogic
            If Me.accountCheck OrElse xmlDataClass.AccountList.Count <> 0 Then
                Me.noticeDBClone.RegistsNoticeDB(xmlDataClass, noticeDisposalMode)
            Else
                Me.noticeDBClone.RegistsNoticeDBNoAccount(xmlDataClass)
            End If

            'PushServer処理
            '2012/09/05 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.20） START
            'SendPushServer(xmlDataClass)
            Me.SendPushServer(xmlDataClass)
            '2012/09/05 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.20） END

            ' $01 start step2開発
            '固有の場合は、受付メイン画面更新用のPushServer処理を実施する
            If noticeDisposalMode = NoticeDisposal.Peculiar Then

                SendPushServerForRefresh(xmlDataClass.RequestNotice.RequestClass,
                               xmlDataClass.RequestNotice.Status,
                               xmlDataClass.RequestNotice.DealerCode,
                               xmlDataClass.RequestNotice.StoreCode,
                               xmlDataClass.RequestNotice.FromAccount,
                            xmlDataClass.AccountList)
            End If
            ' $01 end   step2開発

            '成功情報を格納
            Me.errorInfo.ResultId = RESULTID_SUCCESS_CONST
            Me.errorInfo.Message = MESSAGE_SUCCESS_CONST

        Catch ex As ArgumentException
            Logger.Error(ex.Message, ex)
            '失敗情報を格納
            Me.errorInfo.Message = MESSAGE_FAILURE_CONST
            Throw

        Catch ex As OracleExceptionEx
            Logger.Error(ex.Message, ex)
            '失敗情報を格納
            Me.errorInfo.ResultId = Me.noticeDBClone.errorInfo.ResultId
            Me.errorInfo.Message = MESSAGE_FAILURE_CONST
            Throw

        Catch ex As Exception
            Logger.Error(ex.Message, ex)
            '失敗情報を格納
            Me.errorInfo.ResultId = RESULTID_FAILURE_CONST
            Me.errorInfo.Message = MESSAGE_FAILURE_CONST
            Throw

        Finally
            'XML作成
            Me.errorInfo.NoticeRequestId = xmlDataClass.RequestNotice.RequestId
            returnXml = CreateReturnXml()
            'ログ出力
            Using writer As New StringWriter(CultureInfo.InvariantCulture())
                Dim outXml As New XmlSerializer(GetType(Response))
                outXml.Serialize(writer, returnXml)
                If RESULTID_SUCCESS_CONST.Equals(Me.errorInfo.ResultId) Then
                    '成功
                    Logger.Info(writer.ToString)
                Else
                    '失敗
                    Logger.Error(writer.ToString)
                End If
            End Using
        End Try

        'Logger.Info(GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, False))
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} END " _
                , Me.GetType.ToString _
                , System.Reflection.MethodBase.GetCurrentMethod.Name))
        Return Me.errorInfo
    End Function

#End Region

#Region "XML解析処理"

    ''' <summary>
    ''' XML解析処理
    ''' </summary>
    ''' <param name="getXml">解析XML</param>
    ''' <param name="xmlDataClass">格納クラス</param>
    ''' <param name="noticeDisposalMode">固有、汎用表示フラグ</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2013/12/10 TMEJ 加藤(宏) TMEJ次世代サービス 工程管理機能開発
    ''' </history>
    Private Sub GetXMLData(ByVal getXml As String,
                           ByVal xmlDataClass As XmlNoticeData,
                           ByVal noticeDisposalMode As NoticeDisposal)
        Logger.Info(GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, True))
        'Logger.Info(GetLogParam("getXml", getXml, False) & _
        '            GetLogParam("xmlDataClass", xmlDataClass.ToString, True) & _
        '            GetLogParam("noticeDisposalMode", CStr(noticeDisposalMode), True))

        Try
            Dim reqestXmlDocument As New XmlDocument()

            '2013/12/10 TMEJ 加藤(宏) TMEJ次世代サービス 工程管理機能開発 START
            reqestXmlDocument.PreserveWhitespace = True
            '2013/12/10 TMEJ 加藤(宏) TMEJ次世代サービス 工程管理機能開発 END

            reqestXmlDocument.LoadXml(getXml)

            'RequestRegist配下の情報を取得する
            Dim requestRegistClone As XmlNode =
                GetChildNode(reqestXmlDocument,
                             XmlDataRequestRegist,
                             DataAssignment.ModeMandatory,
                             ElementName.RequestRegist).CloneNode(True)

            GetRegistElementValue(requestRegistClone, xmlDataClass, noticeDisposalMode)

        Finally
            'Logger.Info(GetReturnParam(xmlDataClass.ToString))
            Logger.Info(GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, False))
        End Try

    End Sub

    ''' <summary>
    ''' 子ノードを取得します
    ''' </summary>
    ''' <param name="parentsNode">親ノード</param>
    ''' <param name="childNodeName">子ノード名</param>
    ''' <param name="dataAssignmentMode">要素の割り当て状態</param>
    ''' <param name="elementCode">エラー出力用の要素コード</param>
    ''' <returns>子ノード</returns>
    ''' <remarks></remarks>
    Private Function GetChildNode(ByVal parentsNode As XmlNode,
                                  ByVal childNodeName As String,
                                  ByVal dataAssignmentMode As DataAssignment,
                                  ByVal elementCode As ElementName) As XmlNode
        'Logger.Info(GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, True))
        'Logger.Info(GetLogParam("parentsNode", parentsNode.Value, False) & _
        '            GetLogParam("childNodeName", childNodeName, True) & _
        '            GetLogParam("dataAssignmentMode", CStr(dataAssignmentMode), True) & _
        '            GetLogParam("elementCode", CStr(elementCode), True))

        '子ノード配下の情報を取得する
        Dim getChildNodes As XmlNodeList =
            GetChildNodeInfo(parentsNode,
                         childNodeName,
                         dataAssignmentMode,
                         False,
                         elementCode)

        'Logger.Info(GetReturnParam(getChildNodes.ToString))
        'Logger.Info(GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, False))
        Return getChildNodes.Item(0)
    End Function

    ''' <summary>
    ''' 子ノードの情報を取得します
    ''' </summary>
    ''' <param name="parentsNode">親ノード</param>
    ''' <param name="childNodeName">子ノード名</param>
    ''' <param name="dataAssignmentMode">要素の割り当て状態</param>
    ''' <param name="canMultiple">同名の子ノードを複数を許す場合はTrueとします</param>
    ''' <param name="elementCode">エラー出力用の要素コード</param>
    ''' <returns>子ノードの情報</returns>
    ''' <remarks></remarks>
    Private Function GetChildNodeInfo(ByVal parentsNode As XmlNode,
                                  ByVal childNodeName As String,
                                  ByVal dataAssignmentMode As DataAssignment,
                                  ByVal canMultiple As Boolean,
                                  ByVal elementCode As ElementName) As XmlNodeList
        'Logger.Info(GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, True))
        'Logger.Info(GetLogParam("parentsNode", parentsNode.Value, False) & _
        '            GetLogParam("childNodeName", childNodeName, True) & _
        '            GetLogParam("dataAssignmentMode", CStr(dataAssignmentMode), True) & _
        '            GetLogParam("canMultiple", canMultiple.ToString, True) & _
        '            GetLogParam("elementCode", CStr(elementCode), True))

        '子ノードが存在するか確認します。
        Dim childNodesCount As Integer =
            parentsNode.SelectNodes(XmlRootDirectory + childNodeName).Count
        Dim xmlNodeListData As XmlNodeList

        Dim errorResultId As String = String.Empty
        If childNodesCount = 0 Then
            '子ノードが存在しなかった場合
            Select Case dataAssignmentMode
                Case DataAssignment.ModeMandatory
                    '必須項目に対して子ノードが存在しないのでエラー
                    errorResultId = CreateResultId(ReturnCode.NotXmlElementError, elementCode)
                    Me.errorInfo.ResultId = errorResultId
                    Throw New ArgumentException()
                Case DataAssignment.ModeOptional
                    'オプション項目なので子ノードが存在しなくても問題ない
                    xmlNodeListData = parentsNode.SelectNodes(XmlRootDirectory + childNodeName)
                Case Else
                    '想定外の値が設定された場合、オプション項目として扱います。
                    xmlNodeListData = parentsNode.SelectNodes(XmlRootDirectory + childNodeName)
            End Select
        ElseIf childNodesCount = 1 Then
            '子ノードが存在する場合、要素を取得します。
            xmlNodeListData = parentsNode.SelectNodes(XmlRootDirectory + childNodeName)
        Else
            '子ノードが複数あるのが許される場合
            If canMultiple Then
                xmlNodeListData = parentsNode.SelectNodes(XmlRootDirectory + childNodeName)
            Else
                errorResultId = CStr(ReturnCode.XmlIncorrect)
                Me.errorInfo.ResultId = errorResultId
                Throw New ArgumentException()
            End If
        End If

        'Logger.Info(GetReturnParam(xmlNodeListData.ToString))
        'Logger.Info(GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, False))
        Return xmlNodeListData
    End Function


    ' 2012/06/06 KN 彭 コード分析対応 START

    ''' <summary>
    ''' RequestRegist要素内にある要素を専用のクラスに格納します。
    ''' </summary>
    ''' <param name="requestRegistClone">親タグ配下の情報</param>
    ''' <param name="xmlNoticeDataClass">格納クラス</param>
    ''' <param name="noticeDisposalMode">固有、汎用表示フラグ</param>
    ''' <remarks></remarks>
    Private Sub GetRegistElementValue(ByVal requestRegistClone As XmlNode,
                                      ByVal xmlNoticeDataClass As XmlNoticeData,
                                      ByVal noticeDisposalMode As NoticeDisposal)
        'Logger.Info(GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, True))
        'Logger.Info(GetLogParam("requestRegistClone", requestRegistClone.Value, False) & _
        '            GetLogParam("xmlNoticeDataClass", xmlNoticeDataClass.ToString, True) & _
        '            GetLogParam("noticeDisposalMode", CStr(noticeDisposalMode), True))

        'Head要素内を取得します。
        GetHeadElementValue(requestRegistClone, xmlNoticeDataClass)

        'Detail要素内を取得します。
        Dim detailClone As XmlNode = GetChildNode(requestRegistClone,
                                                  XmlDataDetail,
                                                  DataAssignment.ModeMandatory,
                                                  ElementName.Detail).CloneNode(True)

        'RequestNotice要素内を取得します。
        GetRequestNoticeElementValue(detailClone,
                                     xmlNoticeDataClass,
                                     noticeDisposalMode)

        'PushInfo要素内を取得します。
        GetPushInfoElementValue(detailClone, xmlNoticeDataClass)

        '「依頼種別=01(査定) And ステータス=4(受付)」or「依頼種別=02(価格相談) And ステータス<>1(依頼)」の場合は
        'Accountチェックしない
        Dim accountDataAssignment As DataAssignment
        If (NoticeClassAssessment.Equals(Me.requestNoticeData.RequestClass) AndAlso
           CStr(Status.AcceptanceStatus).Equals(Me.requestNoticeData.Status)) OrElse
           (NoticeClassPriceConsultation.Equals(Me.requestNoticeData.RequestClass) AndAlso
           Not CStr(Status.RequestStatus).Equals(Me.requestNoticeData.Status)) Then
            Me.accountCheck = False
            accountDataAssignment = DataAssignment.ModeOptional
        Else
            accountDataAssignment = DataAssignment.ModeMandatory
        End If

        'ReceiveAccount要素内を取得します。
        Dim receiveAccountClone As XmlNode =
            GetChildNode(requestRegistClone,
                         XmlDataReceiveAccount,
                         accountDataAssignment,
                         ElementName.RequestRegist)
        If Not IsNothing(receiveAccountClone) Then
            'ReceiveAccount要素は複数ある場合があるので、ForEach文で回してListに格納する。
            For Each accountXmlClone As XmlNode In GetChildNodeInfo(receiveAccountClone,
                                                                XmlDataAccount,
                                                                accountDataAssignment,
                                                                True,
                                                                ElementName.ReceiveAccount)
                '複数ノードを許可する
                Dim accounClone As XmlNode = accountXmlClone.CloneNode(True)
                'Account要素内を取得します。
                GetAccountElementValue(accounClone,
                                       xmlNoticeDataClass)
            Next
        End If

        'Logger.Info(GetReturnParam(xmlNoticeDataClass.ToString))
        'Logger.Info(GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, False))
    End Sub

    ''' <summary>
    ''' Head要素内にある要素を専用のクラスに格納します。
    ''' </summary>
    ''' <param name="requestRegistClone">親タグ配下の情報</param>
    ''' <param name="xmlDataClass">取得した要素を格納したクラス</param>
    ''' <remarks></remarks>
    Private Sub GetHeadElementValue(ByVal requestRegistClone As XmlNode,
                                    ByVal xmlDataClass As XmlNoticeData)
        'Logger.Info(GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, True))
        'Logger.Info(GetLogParam("requestRegistClone", requestRegistClone.Value, False) & _
        '            GetLogParam("xmlDataClass", xmlDataClass.ToString, True))

        'Head要素の子ノードを取得します。
        Dim headXml As XmlNode = GetChildNode(requestRegistClone,
                                              XmlDataHead,
                                              DataAssignment.ModeMandatory,
                                              ElementName.Head).CloneNode(True)
        '送信日付
        Dim transmissionDateText As String = GetNodeInnerText(headXml,
                                             XmlDataTransmissionDate,
                                             DataAssignment.ModeMandatory,
                                             19,
                                             TypeConversion.DateType,
                                             ElementName.TransmissionDate)
        xmlDataClass.TransmissionDate = Date.Parse(transmissionDateText,
                                                   CultureInfo.InvariantCulture())

        'Logger.Info(GetReturnParam(xmlDataClass.ToString))
        'Logger.Info(GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, False))
    End Sub

    ''' <summary>
    ''' RequestNotice要素内にある要素を専用のクラスに格納します。
    ''' </summary>
    ''' <param name="detailClone">detail情報</param>
    ''' <param name="xmlDataClass">取得した要素を格納したクラス</param>
    ''' <param name="noticeDisposalMode">固有、汎用表示フラグ</param>
    ''' <remarks></remarks>
    Private Sub GetRequestNoticeElementValue(ByVal detailClone As XmlNode,
                                             ByVal xmlDataClass As XmlNoticeData,
                                             ByVal noticeDisposalMode As NoticeDisposal)
        'Logger.Info(GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, True))
        'Logger.Info(GetLogParam("detailClone", detailClone.Value, False) & _
        '            GetLogParam("xmlDataClass", xmlDataClass.ToString, True) & _
        '            GetLogParam("noticeDisposalMode", CStr(noticeDisposalMode), True))

        'RequestNotice要素の子ノードを取得します。
        Dim requestNoticeXml As XmlNode =
            GetChildNode(detailClone,
                         XmlDataRequestNotice,
                         DataAssignment.ModeMandatory,
                         ElementName.RequestNotice).CloneNode(True)
        Me.requestNoticeData = New XmlRequestNotice

        '共通項目の取得
        '販売店コード
        Me.requestNoticeData.DealerCode = GetNodeInnerText(requestNoticeXml,
                                                        XmlDataDealerCode,
                                                        DataAssignment.ModeMandatory,
                                                        5,
                                                        TypeConversion.StringType,
                                                        ElementName.DealerCode)
        '店舗コード
        Me.requestNoticeData.StoreCode = GetNodeInnerText(requestNoticeXml,
                                                       XmlDataStoreCode,
                                                       DataAssignment.ModeMandatory,
                                                       3,
                                                       TypeConversion.StringType,
                                                       ElementName.StoreCode)
        'スタッフコード(送信元)
        Dim fromAccount As String = GetNodeInnerText(requestNoticeXml,
                                                     XmlDataFromAccount,
                                                     DataAssignment.ModeOptional,
                                                     26,
                                                     TypeConversion.StringType,
                                                     ElementName.FromAccount)
        '端末ID(送信元)
        Dim fromClientID As String = GetNodeInnerText(requestNoticeXml,
                                                      XmlDataFromClientId,
                                                      DataAssignment.ModeOptional,
                                                      20,
                                                      TypeConversion.StringType,
                                                      ElementName.FromClientId)
        '送信元情報のチェック
        If String.IsNullOrEmpty(fromAccount) And String.IsNullOrEmpty(fromClientID) Then
            Dim errorResultId As String = CreateResultId(ReturnCode.NotXmlElementError,
                                                         ElementName.FromAccount)
            Me.errorInfo.ResultId = errorResultId
            Throw New ArgumentException()
        End If
        Me.requestNoticeData.FromAccount = fromAccount
        Me.requestNoticeData.FromClientId = fromClientID

        'スタッフ名(送信元)
        Me.requestNoticeData.FromAccountName = GetNodeInnerText(requestNoticeXml,
                                                             XmlDataFromAccountName,
                                                             DataAssignment.ModeMandatory,
                                                             256,
                                                             TypeConversion.StringType,
                                                             ElementName.FromAccountName)

        '固有、汎用項目の取得
        Select Case noticeDisposalMode
            Case NoticeDisposal.Peculiar
                '依頼種別
                Me.requestNoticeData.RequestClass = GetNodeInnerText(requestNoticeXml,
                                                                  XmlDataRequestClass,
                                                                  DataAssignment.ModeMandatory,
                                                                  2,
                                                                  TypeConversion.StringType,
                                                                  ElementName.RequestClass)
                'ステータス
                Me.requestNoticeData.Status = GetNodeInnerText(requestNoticeXml,
                                                            XmlDataStatus,
                                                            DataAssignment.ModeMandatory,
                                                            1,
                                                            TypeConversion.StringType,
                                                            ElementName.Status)

                Dim requestIdText As String = String.Empty
                Dim requestIdDataAssignment As DataAssignment
                ' $01 start step2開発
                '通知依頼ID： 依頼種別が01(査定),02(価格相談),03(ヘルプ)の場合、ステータスが1(依頼)以外は必須
                If Not CStr(Status.RequestStatus).Equals(requestNoticeData.Status) _
                    And (NoticeClassAssessment.Equals(requestNoticeData.RequestClass) _
                         OrElse NoticeClassPriceConsultation.Equals(requestNoticeData.RequestClass) _
                         OrElse NoticeClassHelp.Equals(requestNoticeData.RequestClass)) Then
                    requestIdDataAssignment = DataAssignment.ModeMandatory
                Else
                    requestIdDataAssignment = DataAssignment.ModeOptional
                End If
                ' $01 end   step2開発

                '依頼ID
                requestIdText = GetNodeInnerText(requestNoticeXml,
                                                 XmlDataRequestId,
                                                 requestIdDataAssignment,
                                                 10,
                                                 TypeConversion.IntegerType,
                                                 ElementName.RequestId)
                Me.requestNoticeData.RequestId = CLng(requestIdText)

                Dim requestClassIdText As String = String.Empty
                Dim requestClassIdDataAssignment As DataAssignment

                ' $01 start step2開発
                '依頼種別IDは「依頼種別=01(査定) or 依頼種別=02(価格相談) or 06(CS Survey)」の場合、必須

                If NoticeClassAssessment.Equals(Me.requestNoticeData.RequestClass) OrElse
                   NoticeClassPriceConsultation.Equals(Me.requestNoticeData.RequestClass) OrElse
                   NoticeClassCSSurvey.Equals(Me.requestNoticeData.RequestClass) Then
                    ' $01 end   step2開発
                    requestClassIdDataAssignment = DataAssignment.ModeMandatory
                Else
                    requestClassIdDataAssignment = DataAssignment.ModeOptional
                End If
                '依頼種別ID
                requestClassIdText = GetNodeInnerText(requestNoticeXml,
                                                      XmlDataRequestClassId,
                                                      requestClassIdDataAssignment,
                                                      10,
                                                      TypeConversion.IntegerType,
                                                      ElementName.RequestClassId)
                Me.requestNoticeData.RequestClassId = CLng(requestClassIdText)

                ' $01 start step2開発                
                'お客様名ID、お客様名、顧客分類、顧客種別は「ステータス=1(依頼)」の場合は必須
                '依頼種別が05(苦情),06(CS Survey)の場合、ステータス値に関わらず必須
                Dim custDataAssignment As DataAssignment
                If CStr(Status.RequestStatus).Equals(Me.requestNoticeData.Status) _
                   OrElse NoticeClassClaim.Equals(requestNoticeData.RequestClass) _
                   OrElse NoticeClassCSSurvey.Equals(requestNoticeData.RequestClass) Then
                    ' $01 end   step2開発

                    custDataAssignment = DataAssignment.ModeMandatory
                Else
                    custDataAssignment = DataAssignment.ModeOptional
                End If
                'お客様ID
                Me.requestNoticeData.CustomId = GetNodeInnerText(requestNoticeXml,
                                                              XmlDataCustomId,
                                                              custDataAssignment,
                                                              20,
                                                              TypeConversion.StringType,
                                                              ElementName.CustomId)
                'お客様名
                Me.requestNoticeData.CustomName = GetNodeInnerText(requestNoticeXml,
                                                                XmlDataCustomName,
                                                                custDataAssignment,
                                                                256,
                                                                TypeConversion.StringType,
                                                                ElementName.CustomName)
                '顧客分類
                Me.requestNoticeData.CustomerClass = GetNodeInnerText(requestNoticeXml,
                                                                   XmlDataCustomerClass,
                                                                   custDataAssignment,
                                                                   1,
                                                                   TypeConversion.StringType,
                                                                   ElementName.CustomerClass)
                '顧客分類
                Me.requestNoticeData.CustomerKind = GetNodeInnerText(requestNoticeXml,
                                                                  XmlDataCustomerKind,
                                                                  custDataAssignment,
                                                                  1,
                                                                  TypeConversion.StringType,
                                                                  ElementName.CustomerKind)
                '顧客担当セールススタッフコード
                Me.requestNoticeData.SalesStaffCode = GetNodeInnerText(requestNoticeXml,
                                                                    XmlDataSalesStaffCode,
                                                                    DataAssignment.ModeOptional,
                                                                    20,
                                                                    TypeConversion.StringType,
                                                                    ElementName.SalesStaffCode)
                '車両シーケンス№
                Me.requestNoticeData.VehicleSequenceNumber =
                    GetNodeInnerText(requestNoticeXml,
                                     XmlDataVehicleSequenceNumber,
                                     DataAssignment.ModeOptional,
                                     128,
                                     TypeConversion.StringType,
                                     ElementName.VehicleSequenceNumber)
                'Follow-up Box店舗コード
                Me.requestNoticeData.FollowUpBoxStoreCode =
                    GetNodeInnerText(requestNoticeXml,
                                     XmlDataFollowUpBoxStoreCode,
                                     DataAssignment.ModeOptional,
                                     3,
                                     TypeConversion.StringType,
                                     ElementName.FollowUpBoxStoreCode)
                'Follow-up Box番号
                Dim followUpBoxNumberText As String =
                    GetNodeInnerText(requestNoticeXml,
                                     XmlDataFollowUpBoxNumber,
                                     DataAssignment.ModeOptional,
                                     10,
                                     TypeConversion.IntegerType,
                                     ElementName.FollowUpBoxNumber)
                Me.requestNoticeData.FollowUpBoxNumber = CLng(followUpBoxNumberText)

                ' $01 start step2開発
                ' 用紙名: 依頼種別が06(CS Survey)の場合は必須
                Dim CSPaperNameDataAssignment As DataAssignment
                If NoticeClassCSSurvey.Equals(requestNoticeData.RequestClass) Then
                    CSPaperNameDataAssignment = DataAssignment.ModeMandatory
                Else
                    CSPaperNameDataAssignment = DataAssignment.ModeOptional
                End If

                Me.requestNoticeData.CSPaperName = GetNodeInnerText(requestNoticeXml,
                                                XmlDataCSPaperName,
                                                CSPaperNameDataAssignment,
                                                64,
                                                TypeConversion.StringType,
                                                ElementName.CSPaperName)
                ' $01 end   step2開発
                Me.requestNoticeData.Message = String.Empty
                Me.requestNoticeData.SessionValue = String.Empty

            Case NoticeDisposal.GeneralPurpose
                '表示内容
                Me.requestNoticeData.Message = GetNodeInnerText(requestNoticeXml,
                                                             XmlDataMessage,
                                                             DataAssignment.ModeMandatory,
                                                             2000,
                                                             TypeConversion.StringType,
                                                             ElementName.Message)
                'セッション設定値
                Me.requestNoticeData.SessionValue = GetNodeInnerText(requestNoticeXml,
                                                                  XmlDataSessionValue,
                                                                  DataAssignment.ModeOptional,
                                                                  2000,
                                                                  TypeConversion.StringType,
                                                                  ElementName.SessionValue)

                Me.requestNoticeData.RequestClass = String.Empty
                Me.requestNoticeData.Status = String.Empty
                Me.requestNoticeData.RequestId = InitLong
                Me.requestNoticeData.RequestClassId = InitLong
                Me.requestNoticeData.CustomId = String.Empty
                Me.requestNoticeData.CustomName = String.Empty
                Me.requestNoticeData.CustomerClass = String.Empty
                Me.requestNoticeData.CustomerKind = String.Empty
                Me.requestNoticeData.SalesStaffCode = String.Empty
                Me.requestNoticeData.VehicleSequenceNumber = String.Empty
                Me.requestNoticeData.FollowUpBoxStoreCode = String.Empty
                Me.requestNoticeData.FollowUpBoxNumber = InitLong
                ' $01 start step2開発
                Me.requestNoticeData.CSPaperName = String.Empty
                ' $01 end   step2開発

        End Select

        xmlDataClass.RequestNotice = Me.requestNoticeData

        'Logger.Info(GetReturnParam(xmlDataClass.ToString))
        'Logger.Info(GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, False))
    End Sub

    ''' <summary>
    ''' Account要素内にある要素を専用のクラスに格納します。
    ''' </summary>
    ''' <param name="accounClone">Account情報</param>
    ''' <param name="xmlDataClass">取得した要素を格納したクラス</param>
    ''' <remarks></remarks>
    Private Sub GetAccountElementValue(ByVal accounClone As XmlNode,
                                       ByVal xmlDataClass As XmlNoticeData)
        'Logger.Info(GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, True))
        'Logger.Info(GetLogParam("accounClone", accounClone.Value, False) & _
        '            GetLogParam("xmlDataClass", xmlDataClass.ToString, True))

        Me.xmlAccountData = New XmlAccount

        'スタッフコード(受信先)
        Dim toAccount As String = GetNodeInnerText(accounClone,
                                                   XmlDataToAccount,
                                                   DataAssignment.ModeOptional,
                                                   26,
                                                   TypeConversion.StringType,
                                                   ElementName.ToAccount)
        '端末ID(受信先)
        Dim toClientID As String = GetNodeInnerText(accounClone,
                                                    XmlDataToClientId,
                                                    DataAssignment.ModeOptional,
                                                    20,
                                                    TypeConversion.StringType,
                                                    ElementName.ToClientId)
        '受信先情報のチェック
        If Me.accountCheck Then
            If String.IsNullOrEmpty(toAccount) And String.IsNullOrEmpty(toClientID) Then
                Dim errorResultId As String = CreateResultId(ReturnCode.NotXmlElementError,
                                                             ElementName.ToAccount)
                Me.errorInfo.ResultId = errorResultId
                Throw New ArgumentException()
            End If
        End If
        Me.xmlAccountData.ToAccount = toAccount
        Me.xmlAccountData.ToClientId = toClientID

        'スタッフ名(受信先)
        Me.xmlAccountData.ToAccountName = GetNodeInnerText(accounClone,
                                                        XmlDataToAccountName,
                                                        DataAssignment.ModeOptional,
                                                        256,
                                                        TypeConversion.StringType,
                                                        ElementName.ToAccountName)

        xmlDataClass.AccountList.Add(Me.xmlAccountData)

        'Logger.Info(GetReturnParam(xmlDataClass.ToString))
        'Logger.Info(GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, False))
    End Sub
    ' 2012/06/06 KN 彭 コード分析対応 END

    ''' <summary>
    ''' PushInfo要素内にある要素を専用のクラスに格納します。
    ''' </summary>
    ''' <param name="detailClone">detail情報</param>
    ''' <param name="xmlDataClass">取得した要素を格納したクラス</param>
    ''' <remarks></remarks>
    Private Sub GetPushInfoElementValue(ByVal detailClone As XmlNode,
                                        ByVal xmlDataClass As XmlNoticeData)
        'Logger.Info(GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, True))
        'Logger.Info(GetLogParam("detailClone", detailClone.ToString, False) & _
        '            GetLogParam("xmlDataClass", "格納クラス", True))

        'PushInfo要素の子ノートを取得します。
        Dim pushInfoXml As XmlNode = GetChildNode(detailClone,
                                                  XmlDataPushInfo,
                                                  DataAssignment.ModeMandatory,
                                                  ElementName.PushInfo).CloneNode(True)
        Me.pushInfoData = New XmlPushInfo

        'カテゴリータイプ
        Me.pushInfoData.PushCategory = GetNodeInnerText(pushInfoXml,
                                                     XmlDataPushCategory,
                                                     DataAssignment.ModeMandatory,
                                                     1,
                                                     TypeConversion.StringType,
                                                     ElementName.PushCategory)
        '表示位置
        Me.pushInfoData.PositionType = GetNodeInnerText(pushInfoXml,
                                                     XmlDataPositionType,
                                                     DataAssignment.ModeMandatory,
                                                     1,
                                                     TypeConversion.StringType,
                                                     ElementName.PositionType)
        '表示時間
        Me.pushInfoData.Time = CLng(GetNodeInnerText(pushInfoXml,
                                                  XmlDataTime,
                                                  DataAssignment.ModeOptional,
                                                  3,
                                                  TypeConversion.IntegerType,
                                                  ElementName.Time))
        '表示タイプ
        Me.pushInfoData.DisplayType = GetNodeInnerText(pushInfoXml,
                                                    XmlDataDisplayType,
                                                    DataAssignment.ModeMandatory,
                                                    1,
                                                    TypeConversion.StringType,
                                                    ElementName.DisplayType)
        '表示内容
        Me.pushInfoData.DisplayContents = GetNodeInnerText(pushInfoXml,
                                                        XmlDataDisplayContents,
                                                        DataAssignment.ModeOptional,
                                                        256,
                                                        TypeConversion.StringType,
                                                        ElementName.DisplayContents)
        '色
        Me.pushInfoData.Color = GetNodeInnerText(pushInfoXml,
                                              XmlDataColor,
                                              DataAssignment.ModeOptional,
                                              1,
                                              TypeConversion.StringType,
                                              ElementName.Color)
        '幅
        Me.pushInfoData.PopWidth = CLng(GetNodeInnerText(pushInfoXml,
                                                      XmlDataPopWidth,
                                                      DataAssignment.ModeOptional,
                                                      5,
                                                      TypeConversion.IntegerType,
                                                      ElementName.PopWidth))
        '高さ
        Me.pushInfoData.PopHeight = CLng(GetNodeInnerText(pushInfoXml,
                                                       XmlDataPopHeight,
                                                       DataAssignment.ModeOptional,
                                                       5,
                                                       TypeConversion.IntegerType,
                                                       ElementName.PopHeight))
        'X座標
        Me.pushInfoData.PopX = CLng(GetNodeInnerText(pushInfoXml,
                                                  XmlDataPopX,
                                                  DataAssignment.ModeOptional,
                                                  5,
                                                  TypeConversion.IntegerType,
                                                  ElementName.PopX))
        'Y座標
        Me.pushInfoData.PopY = CLng(GetNodeInnerText(pushInfoXml,
                                                  XmlDataPopY,
                                                  DataAssignment.ModeOptional,
                                                  5,
                                                  TypeConversion.IntegerType,
                                                  ElementName.PopY))
        '表示時関数
        Me.pushInfoData.DisplayFunction = GetNodeInnerText(pushInfoXml,
                                                        XmlDataDisplayFunction,
                                                        DataAssignment.ModeOptional,
                                                        256,
                                                        TypeConversion.StringType,
                                                        ElementName.DisplayFunction)
        'アクション時関数
        Me.pushInfoData.ActionFunction = GetNodeInnerText(pushInfoXml,
                                                       XmlDataActionFunction,
                                                       DataAssignment.ModeOptional,
                                                       256,
                                                       TypeConversion.StringType,
                                                       ElementName.ActionFunction)
        xmlDataClass.PushInfo = Me.pushInfoData

        'Logger.Info(GetReturnParam(xmlDataClass.ToString))
        'Logger.Info(GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, False))
    End Sub

    ''' <summary>
    ''' 子ノードの要素を取得します。
    ''' </summary>
    ''' <param name="parentsNode">親ノード</param>
    ''' <param name="childNodeName">子ノード名</param>
    ''' <param name="maximumOfDigit">子ノードの要素の最大桁数</param>
    ''' <param name="dataAssignmentMode">要素の割り当て状態</param>
    ''' <param name="type">入力チェックの形式</param>
    ''' <param name="elementCode">エラー出力用の要素コード</param>
    ''' <returns>要素</returns>
    ''' <remarks></remarks>
    Private Function GetNodeInnerText(ByVal parentsNode As XmlNode,
                                      ByVal childNodeName As String,
                                      ByVal dataAssignmentMode As DataAssignment,
                                      ByVal maximumOfDigit As Integer,
                                      ByVal type As TypeConversion,
                                      ByVal elementCode As ElementName) As String
        'Logger.Info(GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, True))
        'Logger.Info(GetLogParam("parentsNode", parentsNode.ToString, False) & _
        '            GetLogParam("childNodeName", childNodeName, True) & _
        '            GetLogParam("dataAssignmentMode", CStr(dataAssignmentMode), True) & _
        '            GetLogParam("maximumOfDigit", CStr(maximumOfDigit), True) & _
        '            GetLogParam("type", CStr(type), True) & _
        '            GetLogParam("elementCode", CStr(elementCode), True))

        ' 要素を取得します。
        Dim childNode As XmlNode = GetChildNode(parentsNode,
                                                childNodeName,
                                                dataAssignmentMode,
                                                elementCode)
        If childNode IsNot Nothing Then
            ' 必須項目で、尚且つ要素内が空の場合は必須項目がないのでエラーとなる
            If Validation.Equals(childNode.InnerText, String.Empty) And
               dataAssignmentMode = DataAssignment.ModeMandatory Then
                ' 必須項目に対してノードが存在しないのでエラー
                Dim errorResultId As String = CreateResultId(ReturnCode.NotXmlElementError,
                                                             elementCode)
                Me.errorInfo.ResultId = errorResultId
                Throw New ArgumentException()
            End If

            ' 取得した要素をチェックします。
            If Validation.Equals(childNode.InnerText, String.Empty) Then
                ' 要素内が空の場合、タイプ別に値を返します
                Select Case type
                    Case TypeConversion.DateType
                        Return Nothing
                    Case TypeConversion.IntegerType
                        Return CStr(InitInteger)
                    Case TypeConversion.None
                        Return Nothing
                    Case TypeConversion.StringType
                        Return String.Empty
                    Case Else
                        Return Nothing
                End Select
            End If

            ' 最大桁数および型をチェックします
            Dim isCheck As Boolean = IsCheckElement(childNode.InnerText,
                                                    maximumOfDigit,
                                                    type,
                                                    elementCode)
            If isCheck Then
                'Logger.Info(GetReturnParam(childNode.InnerXml))
                'Logger.Info(GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, False))
                Return childNode.InnerXml
            End If
        End If
        Return Nothing
    End Function

    ''' <summary>
    ''' 最大桁数チェックと、型チェックを行います
    ''' </summary>
    ''' <param name="target">チェック対象の文字列</param>
    ''' <param name="maximumOfDigit">最大桁数</param>
    ''' <param name="type">型チェックを行う型</param>
    ''' <param name="elementCode">エラー出力用の要素コード</param>
    ''' <returns>True:エラーなし</returns>
    ''' <remarks></remarks>
    Private Function IsCheckElement(ByVal target As String,
                                    ByVal maximumOfDigit As Integer,
                                    ByVal type As TypeConversion,
                                    ByVal elementCode As ElementName) As Boolean
        'Logger.Info(GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, True))
        'Logger.Info(GetLogParam("target", target, False) & _
        '            GetLogParam("maximumOfDigit", CStr(maximumOfDigit), True) & _
        '            GetLogParam("type", CStr(type), True) & _
        '            GetLogParam("elementCode", CStr(elementCode), True))

        ' 最大桁数チェックを行います
        Dim errorResultId As String = String.Empty
        If Validation.IsCorrectDigit(target, maximumOfDigit) Then

            Dim isCheck As Boolean = False
            Select Case type
                Case TypeConversion.None
                    ' noneはチェックしない
                    isCheck = True
                Case TypeConversion.StringType
                    ' 元々文字列型なのでチェックの必要性はない
                    isCheck = True
                Case TypeConversion.IntegerType
                    ' 整数型のチェックをします
                    isCheck = Integer.TryParse(target, 0)
                Case TypeConversion.DateType
                    ' 日付型のチェックをします
                    Try
                        If Len(target) = 10 Then
                            DateTimeFunc.FormatString("yyyy/MM/dd", target)
                        Else
                            DateTimeFunc.FormatString("yyyy/MM/dd HH:mm:ss", target)
                        End If
                        isCheck = True
                    Catch ex As FormatException
                    End Try
            End Select

            If isCheck Then
                'Logger.Info(GetReturnParam(isCheck.ToString))
                'Logger.Info(GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, False))
                Return isCheck
            End If
            ' 値チェックに失敗した場合、エラーとします。
            errorResultId = CreateResultId(ReturnCode.XmlParseError, elementCode)
            Me.errorInfo.ResultId = errorResultId
            Throw New ArgumentException()
        Else
            ' 桁チェックに失敗した場合、エラーとします。
            errorResultId = CreateResultId(ReturnCode.XmlMaximumOfDigitError, elementCode)
            Me.errorInfo.ResultId = errorResultId
            Throw New ArgumentException()
        End If
    End Function

#End Region

#Region "通知DB処理"

    ''' <summary>
    ''' 通知DB処理(受信先があるもの)
    ''' </summary>
    ''' <param name="xmlDataClass">XML解析内容</param>
    ''' <param name="noticeDisposalMode">固有、汎用フラグ</param>
    ''' <remarks></remarks>
    ''' 
    ''' <history>
    ''' 2012/03/02 KN 佐藤 【SERVICE_1】エラーの戻り値を修正
    ''' </history>
    <EnableCommit()>
    Private Sub RegistsNoticeDB(ByVal xmlDataClass As XmlNoticeData,
                         ByVal noticeDisposalMode As NoticeDisposal)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} START:noticeDisposalMode={2} " _
                , Me.GetType.ToString _
                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                , noticeDisposalMode))
        'Logger.Info(GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, True))
        'Logger.Info(GetLogParam("xmlDataClass", xmlDataClass.ToString, False) & _
        '            GetLogParam("noticeDisposalMode", CStr(noticeDisposalMode), True))

        Dim errorResultId As String = String.Empty
        Dim noticeId As Long = InitLong
        Dim noticeRequestDisposalCount As Long = InitLong

        Try
            Using Me.daDataSetTableAdapters
                'Push情報生成
                '通知履歴からのキャンセルの場合
                If IsNothing(xmlDataClass.PushInfo) Then
                    'Push情報と通知マスタのキャンセル文言を取得する
                    Me.dtSelectNoticeRequest = Me.daDataSetTableAdapters.SelectPushInfo(xmlDataClass.RequestNotice.RequestId, CStr(Status.CancelStatus))
                    xmlDataClass.RequestNotice.PushInfo = Me.dtSelectNoticeRequest(0).PUSHINFO
                    xmlDataClass.RequestNotice.NoticeMessage = Me.dtSelectNoticeRequest(0).NOTICEMSG
                Else
                    Dim sbPushInfo As New StringBuilder
                    With sbPushInfo
                        'ヘッダー作成(cat：type：sub)
                        .Append(GetCategory(xmlDataClass.PushInfo.PushCategory))
                        .Append(GetPushType(xmlDataClass.PushInfo.PositionType))
                        .Append(GetSub(xmlDataClass.PushInfo.DisplayType))
                        'ユーザー情報作成(uid)
                        .Append(GetUserId(ReplaceAccount))
                        'フッター作成(time：color：width：height：pox：poy：msg、url、fname：js1：js2)
                        .Append(GetTime(CStr(xmlDataClass.PushInfo.Time)))
                        .Append(GetColor(xmlDataClass.PushInfo.Color))
                        .Append(GetWidth(CStr(xmlDataClass.PushInfo.PopWidth)))
                        .Append(GetHeight(CStr(xmlDataClass.PushInfo.PopHeight)))
                        .Append(GetPositionX(CStr(xmlDataClass.PushInfo.PopX)))
                        .Append(GetPositionY(CStr(xmlDataClass.PushInfo.PopY)))
                        .Append(GetDisplayContents(xmlDataClass.PushInfo.DisplayType, ReplaceMessage))
                        .Append(GetJavaScript1(xmlDataClass.PushInfo.DisplayFunction))
                        .Append(GetJavaScript2(xmlDataClass.PushInfo.ActionFunction))
                    End With
                    xmlDataClass.RequestNotice.PushInfo = sbPushInfo.ToString
                End If

                '受信者が1件の場合は通知IDを取得する
                If xmlDataClass.AccountList.Count = 1 Then
                    noticeId = Me.daDataSetTableAdapters.SelectNoticeId()
                End If

                '通知依頼情報を設定
                Me.dtNoticeRequest = New IC3040801DataSet.IC3040801NoticeRequestDataTable
                Dim drRequest As IC3040801DataSet.IC3040801NoticeRequestRow
                drRequest = CType(Me.dtNoticeRequest.NewRow, 
                                  IC3040801DataSet.IC3040801NoticeRequestRow)

                drRequest.NOTICEREQID = xmlDataClass.RequestNotice.RequestId
                drRequest.NOTICEREQCTG = xmlDataClass.RequestNotice.RequestClass
                drRequest.REQCLASSID = xmlDataClass.RequestNotice.RequestClassId
                drRequest.DLRCD = xmlDataClass.RequestNotice.DealerCode
                drRequest.STRCD = xmlDataClass.RequestNotice.StoreCode
                drRequest.CRCUSTID = xmlDataClass.RequestNotice.CustomId
                drRequest.CUSTOMERCLASS = xmlDataClass.RequestNotice.CustomerClass
                drRequest.LASTNOTICEID = noticeId
                drRequest.CSTKIND = xmlDataClass.RequestNotice.CustomerKind
                drRequest.STATUS = xmlDataClass.RequestNotice.Status
                drRequest.CUSTOMNAME = xmlDataClass.RequestNotice.CustomName
                drRequest.SALESSTAFFCD = xmlDataClass.RequestNotice.SalesStaffCode
                drRequest.VCLID = xmlDataClass.RequestNotice.VehicleSequenceNumber
                drRequest.FLLWUPBOXSTRCD = xmlDataClass.RequestNotice.FollowUpBoxStoreCode
                drRequest.FLLWUPBOX = xmlDataClass.RequestNotice.FollowUpBoxNumber
                ' $01 start step2開発
                drRequest.CSPAPERNAME = xmlDataClass.RequestNotice.CSPaperName
                ' $01 end   step2開発

                '送信者のアカウントがある場合
                If Not String.IsNullOrEmpty(xmlDataClass.RequestNotice.FromAccount) Then
                    drRequest.ACCOUNT = xmlDataClass.RequestNotice.FromAccount
                Else
                    drRequest.ACCOUNT = xmlDataClass.RequestNotice.FromClientId
                End If
                drRequest.SYSTEM = C_SYSTEM

                If xmlDataClass.RequestNotice.RequestId = InitLong OrElse
                   IsNothing(xmlDataClass.RequestNotice.RequestId) Then
                    '新規登録時には通知依頼IDを取得する
                    Dim seqNoticeReqId As Long = Me.daDataSetTableAdapters.SelectNoticeRequestId()
                    xmlDataClass.RequestNotice.RequestId = seqNoticeReqId
                    drRequest.NOTICEREQID = seqNoticeReqId
                    drRequest.PUSHINFO = xmlDataClass.RequestNotice.PushInfo
                    '通知依頼情報の登録処理
                    noticeRequestDisposalCount = Me.daDataSetTableAdapters.InsertNoticeRequest(drRequest)
                    '登録が失敗していたらロールバック
                    If noticeRequestDisposalCount <> 1 Then
                        Throw New OracleExceptionEx()
                    End If
                Else
                    '通知依頼情報の更新処理
                    noticeRequestDisposalCount =
                        Me.daDataSetTableAdapters.UpdateNoticeRequest(drRequest)
                    '更新が失敗していたらロールバック
                    If noticeRequestDisposalCount <> 1 Then
                        Me.Rollback = True
                        '「通知依頼種別=01(査定) And ステータス=3(受付)」の場合のみ、エラー番号に「006010」を入れる
                        If NoticeClassAssessment.Equals(drRequest.NOTICEREQCTG) And
                           CStr(Status.GetStatus).Equals(drRequest.STATUS) Then
                            errorResultId = CreateResultId(ReturnCode.UpdateError, InitInteger)
                            Me.errorInfo.ResultId = errorResultId
                        End If
                        Throw New OracleExceptionEx()
                    End If
                End If

                '通知情報を設定
                Dim sendDate As Date = DateTimeFunc.Now(xmlDataClass.RequestNotice.DealerCode)
                Dim drInfo As IC3040801DataSet.IC3040801NoticeInfoRow
                drInfo = CType(Me.dtNoticeInfo.NewRow, IC3040801DataSet.IC3040801NoticeInfoRow)

                drInfo.NOTICEREQID = drRequest.NOTICEREQID
                drInfo.FROMACCOUNT = xmlDataClass.RequestNotice.FromAccount
                drInfo.FROMCLIENTID = xmlDataClass.RequestNotice.FromClientId
                drInfo.FROMACCOUNTNAME = xmlDataClass.RequestNotice.FromAccountName
                drInfo.SENDDATE = sendDate
                drInfo.READFLG = CStr(Read.Unread)
                drInfo.STATUS = drRequest.STATUS
                drInfo.MESSAGE = xmlDataClass.RequestNotice.Message
                drInfo.SESSIONVALUE = xmlDataClass.RequestNotice.SessionValue

                '送信者のアカウントがある場合
                If Not String.IsNullOrEmpty(xmlDataClass.RequestNotice.FromAccount) Then
                    drInfo.ACCOUNT = xmlDataClass.RequestNotice.FromAccount
                Else
                    drInfo.ACCOUNT = xmlDataClass.RequestNotice.FromClientId
                End If
                drInfo.SYSTEM = C_SYSTEM

                Dim toAccountName As String = xmlDataClass.AccountList(0).ToAccountName
                For Each xmlAccountList As XmlAccount In xmlDataClass.AccountList
                    drInfo.TOACCOUNT = xmlAccountList.ToAccount
                    drInfo.TOCLIENTID = xmlAccountList.ToClientId
                    drInfo.TOACCOUNTNAME = xmlAccountList.ToAccountName
                    '通知情報の登録処理
                    noticeRequestDisposalCount = Me.daDataSetTableAdapters.InsertNoticeInfo(drInfo, noticeId)
                    '登録が失敗していたらロールバック
                    If noticeRequestDisposalCount <> 1 Then
                        Throw New OracleExceptionEx()
                    End If
                Next

                '2012/03/24 KN 小澤 【SALES_1】自分→自分の場合、未読にする修正 START
                '自分→自分追加判断用にFromを取得する
                Dim fromStaff As String = xmlDataClass.RequestNotice.FromAccount
                If String.IsNullOrEmpty(fromStaff) Then
                    fromStaff = xmlDataClass.RequestNotice.FromClientId
                End If

                '自分→自分追加判断　Toの中にFromと同一のアカウントもしくは端末IDがある場合、
                '自分→自分を追加しない
                Dim selfAddFlag As Boolean _
                    = (From ac As Api.DataAccess.XmlAccount In xmlDataClass.AccountList _
                    Where New String() {ac.ToAccount, ac.ToClientId}.Contains(fromStaff)).Count() > 0

                '固有の場合は「自分→自分」の情報を登録する

                'If  noticeDisposalMode = NoticeDisposal.Peculiar Then
                '2012/03/24 KN 小澤 【SALES_1】自分→自分の場合、未読にする修正 START

                If selfAddFlag = False _
                    AndAlso noticeDisposalMode = NoticeDisposal.Peculiar Then
                    drInfo.FROMACCOUNT = xmlDataClass.RequestNotice.FromAccount
                    drInfo.FROMCLIENTID = xmlDataClass.RequestNotice.FromClientId
                    drInfo.FROMACCOUNTNAME = xmlDataClass.RequestNotice.FromAccountName
                    drInfo.TOACCOUNT = xmlDataClass.RequestNotice.FromAccount
                    drInfo.TOCLIENTID = xmlDataClass.RequestNotice.FromClientId
                    drInfo.TOACCOUNTNAME = toAccountName
                    drInfo.READFLG = CStr(Read.Read)
                    If Not String.IsNullOrEmpty(xmlDataClass.RequestNotice.FromAccount) Then
                        drInfo.ACCOUNT = xmlDataClass.RequestNotice.FromAccount
                    Else
                        drInfo.ACCOUNT = xmlDataClass.RequestNotice.FromClientId
                    End If

                    drInfo.SYSTEM = C_SYSTEM

                    '通知情報の登録処理
                    noticeRequestDisposalCount =
                    Me.daDataSetTableAdapters.InsertNoticeInfo(drInfo, InitLong)
                    '登録が失敗していたらロールバック
                    If noticeRequestDisposalCount <> 1 Then
                        Throw New OracleExceptionEx()
                    End If
                End If
            End Using
        Catch ex As OracleExceptionEx When ex.Number = 1014
            Me.Rollback = True
            xmlDataClass.RequestNotice.RequestId = InitLong
            errorResultId = CreateResultId(ReturnCode.TimeOutError, InitInteger)
            Me.errorInfo.ResultId = errorResultId
            Logger.Error(ex.ToString, ex)
            Throw
        Catch ex As OracleExceptionEx
            Me.Rollback = True
            xmlDataClass.RequestNotice.RequestId = InitLong
            'ResultIdが入っていない場合はDBエラー(9000)を入れる
            If String.IsNullOrEmpty(Me.errorInfo.ResultId) Then
                errorResultId = CreateResultId(ReturnCode.DatabaseError, InitInteger)
                Me.errorInfo.ResultId = errorResultId
            End If
            Logger.Error(ex.ToString, ex)
            Throw
        Catch ex As Exception
            Me.Rollback = True
            xmlDataClass.RequestNotice.RequestId = InitLong
            Logger.Error(ex.ToString, ex)
            Throw
        Finally
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END " _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
        End Try
    End Sub

    ''' <summary>
    ''' 通知DB処理(受信先がないもの)
    ''' </summary>
    ''' <param name="xmlDataClass">XML解析内容</param>
    ''' <returns>成功：0</returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Private Function RegistsNoticeDBNoAccount(ByVal xmlDataClass As XmlNoticeData) As Integer
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} START " _
                , Me.GetType.ToString _
                , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim errorResultId As String = String.Empty
        Dim noticeId As Long = InitLong
        Try
            Using Me.daDataSetTableAdapters

                'Push情報生成
                Dim sbPushInfo As New StringBuilder
                With sbPushInfo
                    'ヘッダー作成(cat：type：sub)
                    .Append(GetCategory(xmlDataClass.PushInfo.PushCategory))
                    .Append(GetPushType(xmlDataClass.PushInfo.PositionType))
                    .Append(GetSub(xmlDataClass.PushInfo.DisplayType))
                    'ユーザー情報作成(uid)
                    .Append(GetUserId(ReplaceAccount))
                    'フッター作成(time：color：width：height：pox：poy：msg、url、fname：js1：js2)
                    .Append(GetTime(CStr(xmlDataClass.PushInfo.Time)))
                    .Append(GetColor(xmlDataClass.PushInfo.Color))
                    .Append(GetWidth(CStr(xmlDataClass.PushInfo.PopWidth)))
                    .Append(GetHeight(CStr(xmlDataClass.PushInfo.PopHeight)))
                    .Append(GetPositionX(CStr(xmlDataClass.PushInfo.PopX)))
                    .Append(GetPositionY(CStr(xmlDataClass.PushInfo.PopY)))
                    .Append(GetDisplayContents(xmlDataClass.PushInfo.DisplayType, ReplaceMessage))
                    .Append(GetJavaScript1(xmlDataClass.PushInfo.DisplayFunction))
                    .Append(GetJavaScript2(xmlDataClass.PushInfo.ActionFunction))
                End With
                xmlDataClass.RequestNotice.PushInfo = sbPushInfo.ToString

                '現在通知のやり取りをしている情報を取得する
                Me.dtSelectNoticeInfo = Me.daDataSetTableAdapters.SelectNoticeInfo(xmlDataClass.RequestNotice.RequestId)

                '受信者が1件(自分込みで2件)の場合は通知IDを取得する
                If Me.dtSelectNoticeInfo.Rows.Count = 2 Then
                    noticeId = Me.daDataSetTableAdapters.SelectNoticeId()
                End If

                '通知依頼情報を設定
                Me.dtNoticeRequest = New IC3040801DataSet.IC3040801NoticeRequestDataTable
                Dim drNoticeRequest As IC3040801DataSet.IC3040801NoticeRequestRow
                drNoticeRequest =
                    CType(Me.dtNoticeRequest.NewRow, IC3040801DataSet.IC3040801NoticeRequestRow)
                drNoticeRequest.NOTICEREQID = xmlDataClass.RequestNotice.RequestId
                drNoticeRequest.NOTICEREQCTG = xmlDataClass.RequestNotice.RequestClass
                drNoticeRequest.LASTNOTICEID = noticeId
                drNoticeRequest.STATUS = xmlDataClass.RequestNotice.Status
                If Not String.IsNullOrEmpty(xmlDataClass.RequestNotice.FromAccount) Then
                    drNoticeRequest.ACCOUNT = xmlDataClass.RequestNotice.FromAccount
                Else
                    drNoticeRequest.ACCOUNT = xmlDataClass.RequestNotice.FromClientId
                End If
                drNoticeRequest.SYSTEM = C_SYSTEM

                '通知依頼情報の更新処理
                Dim noticeRequestDisposalCount As Long =
                    Me.daDataSetTableAdapters.UpdateNoticeRequest(drNoticeRequest)
                '更新が失敗していたらロールバック
                If noticeRequestDisposalCount <> 1 Then
                    Me.Rollback = True
                    '「通知依頼種別=01(査定) And ステータス=3(受付)」の場合のみ、エラー番号に「006010」を入れる
                    If NoticeClassAssessment.Equals(drNoticeRequest.NOTICEREQCTG) And
                       CStr(Status.GetStatus).Equals(drNoticeRequest.STATUS) Then
                        errorResultId = CreateResultId(ReturnCode.UpdateError, InitInteger)
                        Me.errorInfo.ResultId = errorResultId
                    End If
                    Throw New OracleExceptionEx()
                End If

                '取得した通知情報をソートする
                Dim dtSelectNoticeInfoOrder As IEnumerable(Of IC3040801DataSet.IC3040801SelectNoticeInfoRow)
                '「通知依頼種別=02(価格相談) And ステータス=3(受信)」の場合は降順、それ以外は昇順でソートする
                If NoticeClassPriceConsultation.Equals(xmlDataClass.RequestNotice.RequestClass) AndAlso
                   CStr(Status.GetStatus).Equals(xmlDataClass.RequestNotice.Status) Then
                    dtSelectNoticeInfoOrder =
                        From fromTableDescending As IC3040801DataSet.IC3040801SelectNoticeInfoRow In Me.dtSelectNoticeInfo
                        Order By fromTableDescending.NOTICEID Descending
                Else
                    dtSelectNoticeInfoOrder =
                        From fromTableAscending As IC3040801DataSet.IC3040801SelectNoticeInfoRow In Me.dtSelectNoticeInfo
                        Order By fromTableAscending.NOTICEID Ascending
                End If

                '通知情報を設定
                Dim sendDate As Date = DateTimeFunc.Now(xmlDataClass.RequestNotice.DealerCode)
                Dim fromAccountName As String = dtSelectNoticeInfoOrder(0).FROMACCOUNTNAME
                Dim toAccountName As String = dtSelectNoticeInfoOrder(0).TOACCOUNTNAME
                Dim drInfo As IC3040801DataSet.IC3040801NoticeInfoRow
                drInfo = CType(Me.dtNoticeInfo.NewRow, IC3040801DataSet.IC3040801NoticeInfoRow)
                drInfo.NOTICEREQID = xmlDataClass.RequestNotice.RequestId
                drInfo.FROMACCOUNT = xmlDataClass.RequestNotice.FromAccount
                drInfo.FROMCLIENTID = xmlDataClass.RequestNotice.FromClientId
                drInfo.FROMACCOUNTNAME = xmlDataClass.RequestNotice.FromAccountName
                drInfo.SENDDATE = sendDate
                For Each drSelectNoticeInfo As IC3040801DataSet.IC3040801SelectNoticeInfoRow In dtSelectNoticeInfoOrder
                    Me.xmlAccountData = New XmlAccount

                    '「通知依頼種別=01(査定) And 送信者=受信者」の場合は自分→自分のデータを入れる
                    If NoticeClassAssessment.Equals(xmlDataClass.RequestNotice.RequestClass) AndAlso
                       drSelectNoticeInfo.FROMACCOUNT.Equals(drSelectNoticeInfo.TOACCOUNT) Then
                        drInfo.TOACCOUNT = xmlDataClass.RequestNotice.FromAccount
                        drInfo.TOCLIENTID = xmlDataClass.RequestNotice.FromClientId
                        drInfo.TOACCOUNTNAME = toAccountName
                        drInfo.READFLG = CStr(Read.Read)
                    Else
                        drInfo.TOACCOUNT = drSelectNoticeInfo.TOACCOUNT
                        drInfo.TOCLIENTID = drSelectNoticeInfo.TOCLIENTID
                        drInfo.TOACCOUNTNAME = drSelectNoticeInfo.TOACCOUNTNAME
                        drInfo.READFLG = CStr(Read.Unread)

                        'Push用のリストを作成(自分→自分以外のデータのみ)
                        If Not drSelectNoticeInfo.FROMACCOUNT.Equals(drSelectNoticeInfo.TOACCOUNT) Then
                            Me.xmlAccountData.ToAccount = drSelectNoticeInfo.TOACCOUNT
                            Me.xmlAccountData.ToClientId = drSelectNoticeInfo.TOCLIENTID
                            Me.xmlAccountData.ToAccountName = drSelectNoticeInfo.TOACCOUNTNAME
                            xmlDataClass.AccountList.Add(Me.xmlAccountData)
                        End If
                    End If
                    '「ステータス=3(受信)」の場合は受信者の名前を変更する
                    If CStr(Status.GetStatus).Equals(xmlDataClass.RequestNotice.Status) Then
                        drInfo.TOACCOUNTNAME = fromAccountName
                    End If

                    drInfo.STATUS = xmlDataClass.RequestNotice.Status
                    drInfo.MESSAGE = String.Empty
                    drInfo.SESSIONVALUE = String.Empty
                    If Not String.IsNullOrEmpty(xmlDataClass.RequestNotice.FromAccount) Then
                        drInfo.ACCOUNT = xmlDataClass.RequestNotice.FromAccount
                    Else
                        drInfo.ACCOUNT = xmlDataClass.RequestNotice.FromClientId
                    End If
                    drInfo.SYSTEM = C_SYSTEM

                    '通知情報の登録処理
                    noticeRequestDisposalCount = Me.daDataSetTableAdapters.InsertNoticeInfo(drInfo, noticeId)
                    '登録が失敗していたらロールバック
                    If noticeRequestDisposalCount <> 1 Then
                        Throw New OracleExceptionEx()
                    End If

                    '通知ID初期化
                    noticeId = InitLong
                Next
            End Using
        Catch ex As OracleExceptionEx When ex.Number = 1014
            Me.Rollback = True
            xmlDataClass.RequestNotice.RequestId = InitLong
            errorResultId = CreateResultId(ReturnCode.TimeOutError, InitInteger)
            Me.errorInfo.ResultId = errorResultId
            Logger.Error(ex.ToString, ex)
            Throw
        Catch ex As OracleExceptionEx
            Me.Rollback = True
            xmlDataClass.RequestNotice.RequestId = InitLong
            'ResultIdが入っていない場合はDBエラー(9000)を入れる
            If String.IsNullOrEmpty(Me.errorInfo.ResultId) Then
                errorResultId = CreateResultId(ReturnCode.DatabaseError, InitInteger)
                Me.errorInfo.ResultId = errorResultId
            End If
            Logger.Error(ex.ToString, ex)
            Throw
        Catch ex As Exception
            Me.Rollback = True
            xmlDataClass.RequestNotice.RequestId = InitLong
            Logger.Error(ex.ToString, ex)
            Throw
        End Try

        'Logger.Info(GetReturnParam(CStr(InitLong)))
        'Logger.Info(GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, False))

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} END " _
                , Me.GetType.ToString _
                , System.Reflection.MethodBase.GetCurrentMethod.Name))
        Return InitLong
    End Function

#End Region

#Region "値チェック"

    ''' <summary>
    ''' 値チェック処理
    ''' </summary>
    ''' <param name="xmlDataClass">通知情報</param>
    ''' <param name="noticeDisposalMode">固有、汎用フラグ</param>
    ''' <remarks></remarks>
    Private Sub CheckXmlDataClass(ByVal xmlDataClass As XmlNoticeData,
                                  ByVal noticeDisposalMode As NoticeDisposal)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} START:noticeDisposalMode={2} " _
                , Me.GetType.ToString _
                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                , noticeDisposalMode))
        'Logger.Info(GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, True))
        'Logger.Info(GetLogParam("xmlDataClass", xmlDataClass.ToString, False) & _
        '            GetLogParam("noticeDisposalMode", CStr(noticeDisposalMode), True))

        '「通知依頼種別=01(査定) And ステータス=4(受付)」or「通知依頼種別=02(価格相談) And ステータス<>1(依頼)」の場合は、
        'Accountチェックしない
        If (NoticeClassAssessment.Equals(xmlDataClass.RequestNotice.RequestClass) And
           CStr(Status.AcceptanceStatus).Equals(xmlDataClass.RequestNotice.Status)) Or
           (NoticeClassPriceConsultation.Equals(xmlDataClass.RequestNotice.RequestClass) And
           Not CStr(Status.RequestStatus).Equals(xmlDataClass.RequestNotice.Status)) Then
            Me.accountCheck = False
        End If

        Dim errorResultId As String = String.Empty
        'Account要素のチェック
        '受信先情報がない場合はエラー
        If Me.accountCheck And xmlDataClass.AccountList.Count = 0 Then
            errorResultId =
                CreateResultId(ReturnCode.NotXmlElementError, ElementName.ToAccount)
            Me.errorInfo.ResultId = errorResultId
            Throw New ArgumentException()
        End If
        '受信先のアカウントおよび受信先の端末IDがない場合はエラー
        For Each xmlAccountList As XmlAccount In xmlDataClass.AccountList
            If Me.accountCheck Then
                If String.IsNullOrEmpty(xmlAccountList.ToAccount) And
                   String.IsNullOrEmpty(xmlAccountList.ToClientId) Then
                    errorResultId =
                        CreateResultId(ReturnCode.NotXmlElementError, ElementName.ToAccount)
                    Me.errorInfo.ResultId = errorResultId
                    Throw New ArgumentException()
                End If
            End If
        Next
        'RequestNotice要素のチェック
        '販売店コード
        If String.IsNullOrEmpty(xmlDataClass.RequestNotice.DealerCode) Then
            errorResultId = CreateResultId(ReturnCode.NotXmlElementError, ElementName.DealerCode)
            Me.errorInfo.ResultId = errorResultId
            Throw New ArgumentException()
        End If
        '店舗コード
        If String.IsNullOrEmpty(xmlDataClass.RequestNotice.StoreCode) Then
            errorResultId = CreateResultId(ReturnCode.NotXmlElementError, ElementName.StoreCode)
            Me.errorInfo.ResultId = errorResultId
            Throw New ArgumentException()
        End If
        '固有の場合は必須チェック
        If noticeDisposalMode = NoticeDisposal.Peculiar Then
            '依頼種別
            If String.IsNullOrEmpty(xmlDataClass.RequestNotice.RequestClass) Then
                errorResultId =
                    CreateResultId(ReturnCode.NotXmlElementError, ElementName.RequestClass)
                Me.errorInfo.ResultId = errorResultId
                Throw New ArgumentException()
            End If
            'ステータス
            If String.IsNullOrEmpty(xmlDataClass.RequestNotice.Status) Then
                errorResultId = CreateResultId(ReturnCode.NotXmlElementError, ElementName.Status)
                Me.errorInfo.ResultId = errorResultId
                Throw New ArgumentException()
            End If
            '依頼ID：「ステータス<>1(依頼) And 通知依頼種別<>04(来店)」の場合は必須
            ' $01 start step2開発
            '通知依頼ID： 依頼種別が01(査定),02(価格相談),03(ヘルプ)の場合、ステータスが1(依頼)以外は必須
            If Not CStr(Status.RequestStatus).Equals(xmlDataClass.RequestNotice.Status) _
                And (CStr(NoticeClassAssessment).Equals(xmlDataClass.RequestNotice.RequestClass) _
                     OrElse CStr(NoticeClassPriceConsultation).Equals(xmlDataClass.RequestNotice.RequestClass) _
                     OrElse CStr(NoticeClassHelp).Equals(requestNoticeData.RequestClass)) Then
                ' $01 end   step2開発

                If xmlDataClass.RequestNotice.RequestId = InitLong Then
                    errorResultId =
                        CreateResultId(ReturnCode.NotXmlElementError, ElementName.RequestId)
                    Me.errorInfo.ResultId = errorResultId
                    Throw New ArgumentException()
                End If
            End If

            ' $01 start step2開発
            '  ''依頼種別ID：「通知依頼種別=01(査定) or 依頼種別=02(価格相談)」の場合は必須
            '依頼種別IDは「依頼種別=01(査定) or 依頼種別=02(価格相談) or 06(CS Survey)」の場合、必須
            If NoticeClassAssessment.Equals(xmlDataClass.RequestNotice.RequestClass) Or
               NoticeClassPriceConsultation.Equals(xmlDataClass.RequestNotice.RequestClass) Or
               NoticeClassCSSurvey.Equals(xmlDataClass.RequestNotice.RequestClass) Then
                ' $01 end   step2開発
                If xmlDataClass.RequestNotice.RequestClassId = InitLong Then
                    errorResultId =
                        CreateResultId(ReturnCode.NotXmlElementError, ElementName.RequestClassId)
                    Me.errorInfo.ResultId = errorResultId
                    Throw New ArgumentException()
                End If
            End If

            ' $01 start step2開発 
            'お客様名ID、お客様名、顧客分類、顧客種別
            '「ステータス=1(依頼)」の場合は必須
            ' 依頼種別が05(苦情),06(CS Survey)の場合、ステータス値に関わらず必須
            If CStr(Status.RequestStatus).Equals(xmlDataClass.RequestNotice.Status) _
                OrElse NoticeClassClaim.Equals(xmlDataClass.RequestNotice.RequestClass) _
                OrElse NoticeClassCSSurvey.Equals(xmlDataClass.RequestNotice.RequestClass) Then
                ' $01 end   step2開発

                'お客様名ID
                If String.IsNullOrEmpty(xmlDataClass.RequestNotice.CustomId) Then
                    errorResultId =
                        CreateResultId(ReturnCode.NotXmlElementError, ElementName.CustomId)
                    Me.errorInfo.ResultId = errorResultId
                    Throw New ArgumentException()
                End If
                'お客様名
                If String.IsNullOrEmpty(xmlDataClass.RequestNotice.CustomName) Then
                    errorResultId =
                        CreateResultId(ReturnCode.NotXmlElementError, ElementName.CustomName)
                    Me.errorInfo.ResultId = errorResultId
                    Throw New ArgumentException()
                End If
                '顧客分類
                If String.IsNullOrEmpty(xmlDataClass.RequestNotice.CustomerClass) Then
                    errorResultId =
                        CreateResultId(ReturnCode.NotXmlElementError, ElementName.CustomerClass)
                    Me.errorInfo.ResultId = errorResultId
                    Throw New ArgumentException()
                End If
                '顧客種別
                If String.IsNullOrEmpty(xmlDataClass.RequestNotice.CustomerKind) Then
                    errorResultId =
                        CreateResultId(ReturnCode.NotXmlElementError, ElementName.CustomerKind)
                    Me.errorInfo.ResultId = errorResultId
                    Throw New ArgumentException()
                End If
            End If
        End If

        ' $01 start step2開発 
        '用紙名
        '依頼種別が06(CS Survey)の場合は必須
        If NoticeClassCSSurvey.Equals(xmlDataClass.RequestNotice.RequestClass) Then

            '用紙名
            If String.IsNullOrEmpty(xmlDataClass.RequestNotice.CSPaperName) Then
                errorResultId =
                    CreateResultId(ReturnCode.NotXmlElementError, ElementName.CSPaperName)
                Me.errorInfo.ResultId = errorResultId
                Throw New ArgumentException()
            End If
        End If
        ' $01 end   step2開発

        'スタッフコード(送信先)、端末ID(送信先)
        If String.IsNullOrEmpty(xmlDataClass.RequestNotice.FromAccount) And
           String.IsNullOrEmpty(xmlDataClass.RequestNotice.FromClientId) Then
            errorResultId = CreateResultId(ReturnCode.NotXmlElementError, ElementName.FromAccount)
            Me.errorInfo.ResultId = errorResultId
            Throw New ArgumentException()
        End If
        'PushInfo要素のチェック
        '通知履歴の場合はチェックなし
        If Not IsNothing(xmlDataClass.PushInfo) Then
            'カテゴリータイプ
            If String.IsNullOrEmpty(xmlDataClass.PushInfo.PushCategory) Then
                errorResultId = CreateResultId(ReturnCode.NotXmlElementError, ElementName.PushCategory)
                Me.errorInfo.ResultId = errorResultId
                Throw New ArgumentException()
            End If
            '表示位置
            If String.IsNullOrEmpty(xmlDataClass.PushInfo.PositionType) Then
                errorResultId = CreateResultId(ReturnCode.NotXmlElementError, ElementName.PositionType)
                Me.errorInfo.ResultId = errorResultId
                Throw New ArgumentException()
            End If
            '表示タイプ
            If String.IsNullOrEmpty(xmlDataClass.PushInfo.DisplayType) Then
                errorResultId = CreateResultId(ReturnCode.NotXmlElementError, ElementName.DisplayType)
                Me.errorInfo.ResultId = errorResultId
                Throw New ArgumentException()
            End If
        End If
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} END " _
                , Me.GetType.ToString _
                , System.Reflection.MethodBase.GetCurrentMethod.Name ))
    End Sub

#End Region

#Region "PushServer処理"

    ''' <summary>
    ''' PushServer処理
    ''' </summary>
    ''' <param name="xmlDataClass">通知情報</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2012/09/05 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.20）
    ''' </history>
    Private Sub SendPushServer(ByVal xmlDataClass As XmlNoticeData)
        Logger.Info(GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, True))
        Logger.Info(GetLogParam("xmlDataClass", xmlDataClass.ToString, False))

        '通知履歴キャンセルでPush情報がない場合は処理しない
        If String.IsNullOrEmpty(xmlDataClass.RequestNotice.PushInfo) Then
            Return
        End If

        Dim visitUtility As New VisitUtility
        Dim pushMessage As String = String.Empty

        '通知履歴キャンセルの場合は通知マスタから取得したデータを置換する
        If IsNothing(xmlDataClass.PushInfo) Then
            pushMessage = Replace(xmlDataClass.RequestNotice.NoticeMessage,
                                 PermutationCustNothing,
                                 xmlDataClass.RequestNotice.CustomName)
        Else
            pushMessage = xmlDataClass.PushInfo.DisplayContents
        End If

        '256バイトを超えていた場合は語尾を置き換える
        If Not Validation.IsCorrectByte(pushMessage, 256) Then
            '「･･･」または「...」の取得
            '2012/04/06 KN 小澤 START
            'Dim rep As String = WebWordUtility.GetWord("SC3040801", 37)
            Dim rep As String = WebWordUtility.GetWord("SC3040801", 39)
            '2012/04/06 KN 小澤 END
            Dim subPushMessage As New StringBuilder

            Dim utf8Encoding As System.Text.Encoding = System.Text.Encoding.GetEncoding("utf-8")
            Dim checkCnt1 As Integer = 256 - utf8Encoding.GetByteCount(rep)

            Dim utf8Byte() As Byte = Encoding.UTF8.GetBytes(pushMessage)
            Dim checkCnt2 As Integer = utf8Encoding.GetCharCount(utf8Byte, 0, checkCnt1)

            With subPushMessage
                If Validation.IsCorrectByte(pushMessage.Substring(0, checkCnt2), checkCnt1) Then
                    subPushMessage.Append(pushMessage.Substring(0, checkCnt2))
                Else
                    subPushMessage.Append(pushMessage.Substring(0, checkCnt2 - 1))
                End If
                subPushMessage.Append(rep)
            End With
            pushMessage = subPushMessage.ToString
        End If

        'MESSAGEを置換する
        xmlDataClass.RequestNotice.PushInfo =
            Replace(xmlDataClass.RequestNotice.PushInfo,
                    ReplaceMessage,
                    pushMessage)

        '2012/09/05 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.20） START
        '権限情報を取得する
        Dim sendOperationList As String() = Nothing
        Dim drSystemEnvSetting As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow = _
            (New SystemEnvSetting).GetSystemEnvSetting("SEND_OPERATION")
        '情報が取得できれば配列に格納
        If Not (IsNothing(drSystemEnvSetting)) AndAlso Not (String.IsNullOrEmpty(drSystemEnvSetting.PARAMVALUE)) Then
            sendOperationList = Split(drSystemEnvSetting.PARAMVALUE, ",")
        End If
        '2012/09/05 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.20） END
        'Account分をPushする
        For Each sendAccountList As XmlAccount In xmlDataClass.AccountList
            Dim sendPushInfo = xmlDataClass.RequestNotice.PushInfo
            'USERを置換する
            Dim account As String
            If Not String.IsNullOrEmpty(sendAccountList.ToAccount) Then
                account = sendAccountList.ToAccount
            Else
                account = sendAccountList.ToClientId
            End If
            sendPushInfo = Replace(sendPushInfo, ReplaceAccount, account)

            '2012/09/05 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.20） START
            'JS2を置換するかチェックする
            If Me.isJavaScript2Replace(account, sendOperationList) Then
                '置換対象の場合
                If String.IsNullOrEmpty(xmlDataClass.PushInfo.ActionFunction) Then
                    sendPushInfo = Replace(sendPushInfo, ReplaceJavascript2, ReplaceJavascript2After)
                Else
                    sendPushInfo = Replace(sendPushInfo, ReplaceJavascript2, ReplaceJavascript2AfterComma)
                End If
            Else
                '置換対象でない場合
                sendPushInfo = Replace(sendPushInfo, ReplaceJavascript2, String.Empty)
            End If
            '2012/09/05 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.20） END

            visitUtility.SendPush(sendPushInfo, xmlDataClass.RequestNotice.DealerCode)
        Next
        Logger.Info(GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, False))
    End Sub

    ''' <summary>
    ''' Push情報生成(カテゴリータイプ)
    ''' </summary>
    ''' <param name="category">カテゴリータイプ</param>
    ''' <returns>「1：cat=popup」「2：cat=action」</returns>
    ''' <remarks></remarks>
    Private Function GetCategory(ByVal category As String) As String
        'Logger.Info(GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, True))
        'Logger.Info(GetLogParam("category", category, False))

        Dim returnCategory As New StringBuilder
        With returnCategory
            .Append(PushConstCategoryText)
            .Append(System.Enum.GetName(GetType(PushConstCategory), CInt(category)))
        End With

        'Logger.Info(GetReturnParam(returnCategory.ToString))
        'Logger.Info(GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, False))
        Return returnCategory.ToString
    End Function

    ''' <summary>
    ''' Push情報生成(表示位置)
    ''' </summary>
    ''' <param name="type">表示位置</param>
    ''' <returns>「0：type=main」「1：type=header」「2：type=bottom」「3：type=left」「4：type=right」「5：type=inside」</returns>
    ''' <remarks></remarks>
    Private Function GetPushType(ByVal type As String) As String
        'Logger.Info(GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, True))
        'Logger.Info(GetLogParam("type", type, False))

        Dim returnPushType As New StringBuilder
        With returnPushType
            .Append(PushConstTypeText)
            .Append(System.Enum.GetName(GetType(PushConstType), CInt(type)))
        End With

        'Logger.Info(GetReturnParam(returnPushType.ToString))
        'Logger.Info(GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, False))
        Return returnPushType.ToString
    End Function

    ''' <summary>
    ''' Push情報生成(表示タイプ)
    ''' </summary>
    ''' <param name="pushSub">表示タイプ</param>
    ''' <returns>「1：sub=Text」「2：sub=URL」「3：sub=Local」「4：sub=JavaScript」</returns>
    ''' <remarks></remarks>
    Private Function GetSub(ByVal pushSub As String) As String
        'Logger.Info(GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, True))
        'Logger.Info(GetLogParam("pushSub", pushSub, False))

        Dim returnSub As New StringBuilder
        With returnSub
            .Append(PushConstSubText)
            .Append(System.Enum.GetName(GetType(PushConstSub), CInt(pushSub)))
        End With

        'Logger.Info(GetReturnParam(returnSub.ToString))
        'Logger.Info(GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, False))
        Return returnSub.ToString
    End Function

    ''' <summary>
    ''' Push情報生成(受信者アカウント)
    ''' </summary>
    ''' <param name="userId">受信者アカウント</param>
    ''' <returns>uid=ユーザーアカウント</returns>
    ''' <remarks></remarks>
    Private Function GetUserId(ByVal userId As String) As String
        'Logger.Info(GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, True))
        'Logger.Info(GetLogParam("userId", userId, False))

        Dim returnUserId As New StringBuilder
        With returnUserId
            .Append(PushConstUserIdText)
            .Append(userId)
        End With

        'Logger.Info(GetReturnParam(returnUserId.ToString))
        'Logger.Info(GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, False))
        Return returnUserId.ToString
    End Function

    ''' <summary>
    ''' Push情報生成(表示時間)
    ''' </summary>
    ''' <param name="time">表示時間</param>
    ''' <returns>time=時間</returns>
    ''' <remarks></remarks>
    Private Function GetTime(ByVal time As String) As String
        'Logger.Info(GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, True))
        'Logger.Info(GetLogParam("time", time, False))

        Dim returnTime As New StringBuilder
        With returnTime
            .Append(PushConstTimeText)
            .Append(time)
        End With

        'Logger.Info(GetReturnParam(returnTime.ToString))
        'Logger.Info(GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, False))
        Return returnTime.ToString
    End Function

    ''' <summary>
    ''' Push情報生成(色)
    ''' </summary>
    ''' <param name="color">色</param>
    ''' <returns>色「1：color=F9EDBE64」「2：color=CBE8FF64」</returns>
    ''' <remarks></remarks>
    Private Function GetColor(ByVal color As String) As String
        'Logger.Info(GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, True))
        'Logger.Info(GetLogParam("color", color, False))

        If String.IsNullOrEmpty(color) Then
            Return String.Empty
        End If
        Dim returnColor As String = String.Empty
        Select Case (CInt(color))
            Case PushConstColor.yellow
                returnColor = PushConstColorYellow
            Case PushConstColor.blue
                returnColor = PushConstColorBlue
        End Select

        'Logger.Info(GetReturnParam(returnColor.ToString))
        'Logger.Info(GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, False))
        Return returnColor
    End Function

    ''' <summary>
    ''' Push情報生成(高さ)
    ''' </summary>
    ''' <param name="height">高さ</param>
    ''' <returns>height=高さ</returns>
    ''' <remarks></remarks>
    Private Function GetHeight(ByVal height As String) As String
        'Logger.Info(GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, True))
        'Logger.Info(GetLogParam("height", height, False))

        If String.IsNullOrEmpty(height) Or CInt(height) = 0 Then
            Return String.Empty
        End If
        Dim returnHeight As New StringBuilder
        With returnHeight
            .Append(PushConstHeightText)
            .Append(height)
        End With

        'Logger.Info(GetReturnParam(returnHeight.ToString))
        'Logger.Info(GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, False))
        Return returnHeight.ToString
    End Function

    ''' <summary>
    ''' Push情報生成(幅)
    ''' </summary>
    ''' <param name="width">幅</param>
    ''' <returns>width=幅</returns>
    ''' <remarks></remarks>
    Private Function GetWidth(ByVal width As String) As String
        'Logger.Info(GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, True))
        'Logger.Info(GetLogParam("width", width, False))

        If String.IsNullOrEmpty(width) Or CInt(width) = 0 Then
            Return String.Empty
        End If
        Dim returnWidth As New StringBuilder
        With returnWidth
            .Append(PushConstWidthText)
            .Append(width)
        End With

        'Logger.Info(GetReturnParam(returnWidth.ToString))
        'Logger.Info(GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, False))
        Return returnWidth.ToString
    End Function

    ''' <summary>
    ''' Push情報生成(X座標)
    ''' </summary>
    ''' <param name="positionX">X座標</param>
    ''' <returns>pox=X座標</returns>
    ''' <remarks></remarks>
    Private Function GetPositionX(ByVal positionX As String) As String
        'Logger.Info(GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, True))
        'Logger.Info(GetLogParam("positionX", positionX, False))

        If String.IsNullOrEmpty(positionX) Or CInt(positionX) = 0 Then
            Return String.Empty
        End If
        Dim returnPositionX As New StringBuilder
        With returnPositionX
            .Append(PushConstPositionXText)
            .Append(positionX)
        End With

        'Logger.Info(GetReturnParam(returnPositionX.ToString))
        'Logger.Info(GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, False))
        Return returnPositionX.ToString
    End Function

    ''' <summary>
    ''' Push情報生成(Y座標)
    ''' </summary>
    ''' <param name="positionY">Y座標</param>
    ''' <returns>poy=Y座標</returns>
    ''' <remarks></remarks>
    Private Function GetPositionY(ByVal positionY As String) As String
        'Logger.Info(GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, True))
        'Logger.Info(GetLogParam("positionY", positionY, False))

        If String.IsNullOrEmpty(positionY) Or CInt(positionY) = 0 Then
            Return String.Empty
        End If
        Dim returnPositionY As New StringBuilder
        With returnPositionY
            .Append(PushConstPositionYText)
            .Append(positionY)
        End With

        'Logger.Info(GetReturnParam(returnPositionY.ToString))
        'Logger.Info(GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, False))
        Return returnPositionY.ToString
    End Function

    ''' <summary>
    ''' Push情報生成(表示内容)
    ''' </summary>
    ''' <param name="pushSub">表示タイプ</param>
    ''' <param name="dispContents">表示内容</param>
    ''' <returns>「1：msg=表示内容」「2：url=表示内容」「3：fname=表示内容」</returns>
    ''' <remarks></remarks>
    Private Function GetDisplayContents(ByVal pushSub As String,
                                        ByVal dispContents As String) As String
        'Logger.Info(GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, True))
        'Logger.Info(GetLogParam("pushSub", pushSub, False))
        'Logger.Info(GetLogParam("dispContents", dispContents, True))

        If PushSubJavaScript.Equals(pushSub) Then
            Return String.Empty
        End If
        Dim returnDispContents As New StringBuilder
        With returnDispContents
            Select Case (CInt(pushSub))
                Case PushConstSub.text
                    .Append(PushConstContentsMessage)
                Case PushConstSub.url
                    .Append(PushConstContentsUrl)
                Case PushConstSub.local
                    .Append(PushConstContentsFileName)
            End Select
            .Append(dispContents)
        End With

        'Logger.Info(GetReturnParam(returnDispContents.ToString))
        'Logger.Info(GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, False))
        Return returnDispContents.ToString
    End Function

    ''' <summary>
    ''' Push情報生成(表示時関数)
    ''' </summary>
    ''' <param name="javaScript1">表示時関数</param>
    ''' <returns>js1=表示時関数</returns>
    ''' <remarks></remarks>
    Private Function GetJavaScript1(ByVal javaScript1 As String) As String
        'Logger.Info(GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, True))
        'Logger.Info(GetLogParam("javaScript1", javaScript1, False))

        If String.IsNullOrEmpty(javaScript1) Then
            Return String.Empty
        End If
        Dim returnJavaScript1 As New StringBuilder
        With returnJavaScript1
            .Append(PushConstJavaScript1Text)
            .Append(javaScript1)
        End With

        'Logger.Info(GetReturnParam(returnJavaScript1.ToString))
        'Logger.Info(GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, False))
        Return returnJavaScript1.ToString
    End Function

    ''' <summary>
    ''' Push情報生成(アクション時関数)
    ''' </summary>
    ''' <param name="javaScript2">アクション時関数</param>
    ''' <returns>js2=アクション時関数</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2012/09/05 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.20）
    ''' </history>
    Private Function GetJavaScript2(ByVal javaScript2 As String) As String
        'Logger.Info(GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, True))
        'Logger.Info(GetLogParam("javaScript2", javaScript2, False))

        '2012/09/05 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.20） START
        'If String.IsNullOrEmpty(javaScript2) Then
        '    Return String.Empty
        'End If
        'Dim returnJavaScript2 As New StringBuilder
        'With returnJavaScript2
        '    .Append(PushConstJavaScript2Text)
        '    .Append(javaScript2)
        'End With
        Dim returnJavaScript2 As New StringBuilder
        With returnJavaScript2
            If String.IsNullOrEmpty(javaScript2) Then
                .Append(PushConstJavaScript2Text)
                returnJavaScript2.Append(ReplaceJavascript2)
            Else
                .Append(PushConstJavaScript2Text)
                .Append(javaScript2)
                .Append(ReplaceJavascript2)
            End If
        End With
        '2012/09/05 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.20） END

        'Logger.Info(GetReturnParam(returnJavaScript2.ToString))
        'Logger.Info(GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, False))
        Return returnJavaScript2.ToString
    End Function

    '2012/09/05 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.20） START
    ''' <summary>
    ''' JS2置換有無
    ''' </summary>
    ''' <param name="account">受信先アカウントID</param>
    ''' <param name="sendOperationList">権限リスト</param>
    ''' <returns>JS2置換有無</returns>
    ''' <remarks></remarks>
    Private Function isJavaScript2Replace(ByVal account As String,
                                          ByVal sendOperationList As String()) As Boolean
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '          , "{0}.{1} START account={2} operationList.COUNT={3}" _
        '          , Me.GetType.ToString _
        '          , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '          , account, sendOperationList.Count.ToString(CultureInfo.CurrentCulture)))

        '戻り値
        Dim returnReplaceCondition As Boolean = False

        'ユーザー情報取得
        Dim drUsers As UsersDataSet.USERSRow = (New Users).GetUser(account)
        '権限がリストと同じの場合は戻り値をTrueにする
        If Not (IsNothing(drUsers)) AndAlso _
           sendOperationList.Contains(drUsers.OPERATIONCODE.ToString(CultureInfo.CurrentCulture)) Then
            returnReplaceCondition = True
        End If

        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '          , "{0}.{1} END" _
        '          , Me.GetType.ToString _
        '          , System.Reflection.MethodBase.GetCurrentMethod.Name))
        Return returnReplaceCondition
    End Function
    '2012/09/05 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.20） END

#End Region

    ' $01 start step2開発
#Region "受付メイン・画面更新用のPushServer処理のメソッド"

    ''' <summary>
    ''' 受付メイン画面更新用のPushServer処理
    ''' </summary>
    ''' <param name="requestClass">通知依頼種別</param>
    ''' <param name="currentStatus">ステータス</param>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="storeCode">店舗コード</param>
    ''' <remarks></remarks>
    Private Sub SendPushServerForRefresh(ByVal requestClass As String, ByVal currentStatus As String,
                               ByVal dealerCode As String, ByVal storeCode As String,
                               ByVal fromAccount As String, ByVal toAccount As List(Of XmlAccount))
        Logger.Info(GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, True))
        Logger.Info(GetLogParam("requestClass", requestClass, False))
        Logger.Info(GetLogParam("currentStatus", currentStatus, False))
        Logger.Info(GetLogParam("dealerCode", dealerCode, False))
        Logger.Info(GetLogParam("storeCode", storeCode, False))

        ' JavaScript用の引数を取得する。空の場合、PushServerへの送信なし
        Dim jsargument As String
        jsargument = GetJavaScript1Argument(requestClass, currentStatus)

        If String.IsNullOrEmpty(jsargument) Then
            Logger.Info("No Push in this stuation")
            Return
        End If

        If (CStr(Status.RequestStatus).Equals(currentStatus) Or CStr(Status.CancelStatus).Equals(currentStatus)) Then

            ' ステータスが 1:依頼 2:キャンセル かつ スタッフコード(送信元)が商談中以外なら、送信せず
            Logger.Info("Status: Request or Cancel")
            If Not isNegociating(fromAccount) Then
                Logger.Info("No push: isNegociating = false")
                Return
            End If
        Else
            ' ステータスが 3:受信 4:回答 かつ 受信先のアカウントが商談中以外なら、送信せず
            Logger.Info("Status: Receive or Reply")

            If toAccount.Count = 0 Then
                Logger.Info("No push: toAccount.Count = 0")
                Return
            End If

            If Not isNegociating(toAccount(0).ToAccount) Then
                Logger.Info("No push: isNegociating = false")
                Return
            End If

        End If

        'pushメッセージを取得する
        Dim bufMessage As New StringBuilder
        With bufMessage
            .Append(PUSH_COMMAND_CAT)
            .Append(PUSH_COMMAND_TYPE)
            .Append(PUSH_COMMAND_SUB)
            .Append(PUSH_COMMAND_UID).Append(ReplaceAccount)
            .Append(PUSH_COMMAND_TIME)
            .Append(PUSH_COMMAND_JS1).Append(PUSH_COMMAND_JS1_REPLACE)
            .Append(jsargument)
        End With

        ' Push送信用のユーティリティを作成
        Dim visitUtility As New VisitUtility

        ' 受付用Push対象のアカウント情報にPush送信する
        ' 操作権限コードのリスト
        Dim operationCdListForReception As New List(Of Decimal)
        operationCdListForReception.Add(OperationCodeReception)
        operationCdListForReception.Add(OperationCodeSalesStaffManager)

        Using usersDt As VisitUtilityUsersDataTable = GetPushUser(dealerCode, storeCode, operationCdListForReception)

            ' JavaScriptのコマンド名を置換する
            Dim pushMessage As String
            pushMessage = bufMessage.ToString().Replace(PUSH_COMMAND_JS1_REPLACE, PUSH_COMMAND_JS1_RECEPTION)

            ' Pushする対象がいない場合、ログを出力する。
            If usersDt.Count = 0 Then
                Logger.Info("No target for reception push")
            End If

            'Account分をPushする
            For Each dr As VisitUtilityUsersRow In usersDt
                'USERを置換して、Push送信する
                visitUtility.SendPush(Replace(pushMessage, ReplaceAccount, dr.ACCOUNT), dealerCode)
            Next dr
        End Using

        ' SSV用Push対象のアカウント情報にPush送信する
        ' 操作権限コードのリスト
        Dim operationCdListForSSV As New List(Of Decimal)
        operationCdListForSSV.Add(OperationCodeSSV)

        Using usersDt As VisitUtilityUsersDataTable = GetPushUser(dealerCode, storeCode, operationCdListForSSV)

            ' JavaScriptのコマンド名を置換する
            Dim pushMessageForSSV As String
            pushMessageForSSV = bufMessage.ToString().Replace(PUSH_COMMAND_JS1_REPLACE, PUSH_COMMAND_JS1_SSV)

            ' Pushする対象がいない場合、ログを出力する。
            If usersDt.Count = 0 Then
                Logger.Info("No target for SSV push")
            End If

            'Account分をPushする
            For Each dr As VisitUtilityUsersRow In usersDt
                'USERを置換して、Push送信する
                visitUtility.SendPushPC(Replace(pushMessageForSSV, ReplaceAccount, dr.ACCOUNT))
            Next dr
        End Using


        Logger.Info(GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, False))

    End Sub


    ''' <summary>
    ''' js1の2つ目の引数取得処理
    ''' </summary>
    ''' <param name="requestClass">通知依頼種別</param>
    ''' <param name="currentStatus">ステータス</param>
    ''' <returns>push用JavaScriptの2つ目の引数</returns>
    ''' <remarks></remarks>
    Private Function GetJavaScript1Argument(ByVal requestClass As String, ByVal currentStatus As String) As String

        Logger.Info(GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, True))
        Logger.Info(GetLogParam("requestClass", requestClass, False))
        Logger.Info(GetLogParam("status", currentStatus, False))

        Dim arg1 As String = String.Empty
        Dim arg2 As String = String.Empty

        ' 通知依頼種別とステータスによって、値を返す
        If NoticeClassAssessment.Equals(requestClass) Then

            ' 査定 送信時:03 受信時:04 
            If CStr(Status.RequestStatus).Equals(currentStatus) Then
                arg2 = PUSH2_SEND_ASSESSMENT
            ElseIf CStr(Status.GetStatus).Equals(currentStatus) Then
                arg2 = PUSH2_RECEIVE_ASSESSMENT
            End If

            arg1 = PUSH2_NOT_CANCEL1

        ElseIf NoticeClassPriceConsultation.Equals(requestClass) Then

            ' 価格相談 送信時:05 回答時:06 
            If CStr(Status.RequestStatus).Equals(currentStatus) Then
                arg2 = PUSH2_SEND_PRICECONSULTATION
            ElseIf CStr(Status.AcceptanceStatus).Equals(currentStatus) Then
                arg2 = PUSH2_REPLY_PRICECONSULTATION
            End If

            arg1 = PUSH2_NOT_CANCEL1

        ElseIf NoticeClassHelp.Equals(requestClass) Then

            ' ヘルプ 送信時:07 回答時:08
            If CStr(Status.RequestStatus).Equals(currentStatus) Then
                arg2 = PUSH2_SEND_HELP
            ElseIf CStr(Status.AcceptanceStatus).Equals(currentStatus) Then
                arg2 = PUSH2_REPLY_HELP
            End If

            arg1 = PUSH2_NOT_CANCEL1

        End If

        If CStr(Status.CancelStatus).Equals(currentStatus) Then
            ' 依頼種別が査定、価格相談、ヘルプにて、ステータスがキャンセルのとき
            If NoticeClassAssessment.Equals(requestClass) _
            OrElse NoticeClassPriceConsultation.Equals(requestClass) _
            OrElse NoticeClassHelp.Equals(requestClass) Then

                arg1 = PUSH2_CANCEL1
                arg2 = PUSH2_CANCEL2

            End If
        End If

        '引数箇所を取得する
        Dim bufArg As New StringBuilder
        If arg2.Length > 0 Then

            With bufArg
                .Append("('")
                .Append(arg1)
                .Append("','")
                .Append(arg2)
                .Append("')")
            End With
        End If

        Dim result As String
        result = bufArg.ToString

        Logger.Info(GetReturnParam(result))
        Logger.Info(GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, False))
        Return result

    End Function

    ''' <summary>
    ''' Push対象のアカウント情報の取得
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="storeCode">店舗コード</param>
    ''' <param name="operationCdList">操作権限コードのリスト</param>
    ''' <returns>オンラインのユーザー情報のデータテーブル</returns>
    ''' <remarks>
    ''' 本メソッドでは、以下のメソッドも利用しているため、例外情報に関しては、下記、メソッドを参照してください。
    ''' <seealso cref="Users.GetAllUser" />
    ''' <seealso cref="VisitUtility.GetOnlineUsers" />
    ''' </remarks>
    Private Function GetPushUser( _
            ByVal dealerCode As String,
            ByVal storeCode As String,
            ByVal operationCdList As List(Of Decimal)) As VisitUtilityUsersDataTable

        Logger.Info(GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, True))
        Logger.Info(GetLogParam("dealerCode", dealerCode, False))
        Logger.Info(GetLogParam("storeCode", storeCode, False))
        Logger.Info(GetLogParam("operationCdList", operationCdList.ToArray.ToString(), False))


        ' ユーザー情報を取得する
        Dim visitUtil As New VisitUtilityBusinessLogic
        Dim dt As VisitUtilityUsersDataTable
        dt = visitUtil.GetOnlineUsers(dealerCode, storeCode, operationCdList)

        Logger.Info(GetReturnParam(dt.ToString))
        Logger.Info(GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, False))

        ' 戻り値にユーザー情報のデータテーブルを設定
        Return dt

    End Function

    ''' <summary>
    ''' 指定のアカウントが商談中か判断する
    ''' </summary>
    ''' <param name="accountId">アカウントのid</param>
    ''' <returns>True: 商談中 False: 商談中でない</returns>
    ''' <remarks>
    ''' </remarks>
    Private Function isNegociating(ByVal accountId As String) As Boolean

        Logger.Info(GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, True))
        Logger.Info(GetLogParam("accountId", accountId, False))

        If String.IsNullOrEmpty(accountId) Then
            Logger.Info("accountId is Null or Empty")
            Return False
        End If

        ' ユーザー情報を取得する
        Dim users As New Users
        Dim userDataSetRow As UsersDataSet.USERSRow = users.GetUser(accountId)

        Dim result As Boolean = False

        If IsNothing(userDataSetRow) Then
            ' 該当のユーザが見つからなかった場合は、false
            Logger.Info("Not found the user")
            result = False
        Else
            Logger.Info("Found the user")

            ' 該当のユーザの「在席状態(大分類))」を取得する
            Dim status As String
            status = userDataSetRow(PresenceCategory).ToString

            result = NEGOCICATING.Equals(status)
        End If
        Logger.Info(GetReturnParam(result.ToString))
        Logger.Info(GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, False))

        ' 戻り値に商談中かどうかの判断
        Return result

    End Function
#End Region

    ' $01 end   step2開発


#Region "戻り値用XML作成"

    ''' <summary>
    ''' 戻り値のXMLを作成します。
    ''' </summary>
    ''' <returns>XMLを格納したResponse</returns>
    ''' <remarks></remarks>
    Private Function CreateReturnXml() As Response
        Logger.Info(GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, True))

        ' Responseクラス生成
        Dim returnXml As Response = New Response()

        ' Headerクラスに値をセット
        Dim createRespHead As Response.RootHead = New Response.RootHead()
        createRespHead.TransmissionDate = CStr(DateTimeFunc.Now())

        ' Detailクラス生成
        Dim createRespDetail As Response.RootDetail = New Response.RootDetail()

        ' Commonクラスに値をセット
        Dim createRespCommon As Response.RootDetail.DetailCommon =
            New Response.RootDetail.DetailCommon()
        createRespCommon.NoticeRequestId = CStr(Me.errorInfo.NoticeRequestId)
        createRespCommon.ResultId = Me.errorInfo.ResultId
        createRespCommon.Message = Me.errorInfo.Message

        'Commonにセットした値をDetailに反映
        createRespDetail.Common = createRespCommon

        'Header、Detailにセットした値をResponseに反映
        returnXml.Head = createRespHead
        returnXml.Detail = createRespDetail

        Logger.Info(GetReturnParam(returnXml.ToString))
        Logger.Info(GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, False))
        Return returnXml
    End Function

#End Region

#Region "ログデータ加工処理"

    ''' <summary>
    ''' ログデータ（メソッド）
    ''' </summary>
    ''' <param name="methodName">メソッド名</param>
    ''' <param name="startEndFlag">True：「method start」を表示、False：「method end」を表示</param>
    ''' <returns>加工した文字列</returns>
    ''' <remarks></remarks>
    Private Function GetLogMethod(ByVal methodName As String,
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
    Private Function GetLogParam(ByVal paramName As String,
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
    Private Function GetReturnParam(ByVal paramData As String) As String
        Dim sb As New StringBuilder
        With sb
            .Append("Return=")
            .Append(paramData)
        End With
        Return sb.ToString
    End Function

#End Region

#Region "エラーコード作成処理"

    ''' <summary>
    ''' エラーコード作成処理
    ''' </summary>
    ''' <param name="errorCode">エラーコード</param>
    ''' <param name="elementCode">項目番号</param>
    ''' <returns>作成した文字列</returns>
    ''' <remarks></remarks>
    Private Function CreateResultId(ByVal errorCode As Integer,
                                    ByVal elementCode As Integer) As String
        Dim returnErrorCode As New StringBuilder
        With returnErrorCode
            .Append("00")
            .Append(CStr(errorCode + elementCode))
        End With
        Return returnErrorCode.ToString
    End Function

#End Region

#Region "XmlNoticeDataログデータ加工処理"

    ''' <summary>
    ''' XmlNoticeDataログデータ加工処理
    ''' </summary>
    ''' <param name="xmlNoticeData">XmlNoticeDataクラス</param>
    ''' <returns>ログ情報</returns>
    ''' <remarks></remarks>
    Private Function LogNoticeData(ByVal xmlNoticeData As XmlNoticeData) As String
        Dim log As New StringBuilder
        With log
            '見やすくするために改行
            .AppendLine("")
            .AppendLine("000･･･")
            .AppendLine("<TransmissionDate>" & CStr(xmlNoticeData.TransmissionDate))
            'AccountList
            .AppendLine("100･･･")
            For Each accountData In xmlNoticeData.AccountList
                .AppendLine("<ToAccount>" & accountData.ToAccount)
                .AppendLine("<ToClientID>" & accountData.ToClientId)
                .AppendLine("<ToAccountName>" & accountData.ToAccountName)
            Next
            .AppendLine("200･･･")
            .AppendLine("<DealerCode>" & xmlNoticeData.RequestNotice.DealerCode)
            .AppendLine("<StoreCode>" & xmlNoticeData.RequestNotice.StoreCode)
            .AppendLine("<RequestClass>" & xmlNoticeData.RequestNotice.RequestClass)
            .AppendLine("<Status>" & xmlNoticeData.RequestNotice.Status)
            .AppendLine("<RequestID>" & xmlNoticeData.RequestNotice.RequestId)
            .AppendLine("<RequestClassID>" & xmlNoticeData.RequestNotice.RequestClassId)
            .AppendLine("<FromAccount>" & xmlNoticeData.RequestNotice.FromAccount)
            .AppendLine("<FromClientID>" & xmlNoticeData.RequestNotice.FromClientId)
            .AppendLine("<FromAccountName>" & xmlNoticeData.RequestNotice.FromAccountName)
            .AppendLine("<CustomID>" & xmlNoticeData.RequestNotice.CustomId)
            .AppendLine("<CustomName>" & xmlNoticeData.RequestNotice.CustomName)
            .AppendLine("<CustomerClass>" & xmlNoticeData.RequestNotice.CustomerClass)
            .AppendLine("<CstKind>" & xmlNoticeData.RequestNotice.CustomerKind)
            .AppendLine("<Message>" & xmlNoticeData.RequestNotice.Message)
            .AppendLine("<SessionValue>" & xmlNoticeData.RequestNotice.SessionValue)
            .AppendLine("<SalesStaffCode>" & xmlNoticeData.RequestNotice.SalesStaffCode)
            .AppendLine("<VehicleSequenceNumber>" & xmlNoticeData.RequestNotice.VehicleSequenceNumber)
            .AppendLine("<FollowUpBoxStoreCode>" & xmlNoticeData.RequestNotice.FollowUpBoxStoreCode)
            .AppendLine("<FollowUpBoxNumber>" & xmlNoticeData.RequestNotice.FollowUpBoxNumber)
            ' $01 start step2開発
            .AppendLine("<CSPaperName>" & xmlNoticeData.RequestNotice.CSPaperName)
            ' $01 end   step2開発
            .AppendLine("300･･･")
            If Not IsNothing(xmlNoticeData.PushInfo) Then
                .AppendLine("<PushCategory>" & xmlNoticeData.PushInfo.PushCategory)
                .AppendLine("<PositionType>" & xmlNoticeData.PushInfo.PositionType)
                .AppendLine("<Time>" & xmlNoticeData.PushInfo.Time)
                .AppendLine("<DisplayType>" & xmlNoticeData.PushInfo.DisplayType)
                .AppendLine("<DisplayContents>" & xmlNoticeData.PushInfo.DisplayContents)
                .AppendLine("<Color>" & xmlNoticeData.PushInfo.Color)
                .AppendLine("<PopWidth>" & xmlNoticeData.PushInfo.PopWidth)
                .AppendLine("<PopHeight>" & xmlNoticeData.PushInfo.PopHeight)
                .AppendLine("<PopX>" & xmlNoticeData.PushInfo.PopX)
                .AppendLine("<PopY>" & xmlNoticeData.PushInfo.PopY)
                .AppendLine("<DisplayFunction>" & xmlNoticeData.PushInfo.DisplayFunction)
                .AppendLine("<ActionFunction>" & xmlNoticeData.PushInfo.ActionFunction)
            End If
        End With
        Return log.ToString
    End Function

#End Region

#Region "Responseクラス"
    ''' <summary>
    ''' Responseクラス(応答用XMLのIFクラス)
    ''' </summary>
    ''' <remarks>応答用のXML情報を格納するクラス</remarks>
    <System.Xml.Serialization.XmlRoot("Response", Namespace:="http://tempuri.org/Response.xsd")> _
    Public Class Response

        ''' <summary>
        ''' Headタグの定義
        ''' </summary>
        ''' <remarks></remarks>
        <System.Xml.Serialization.XmlElementAttribute(ElementName:="Head", IsNullable:=False)> _
        Private outHead As RootHead

        ''' <summary>
        ''' Detailタグの定義
        ''' </summary>
        ''' <remarks></remarks>
        <System.Xml.Serialization.XmlElementAttribute(ElementName:="Detail", IsNullable:=False)> _
        Private outDetail As RootDetail

        ''' <summary>
        ''' Headerタグ用プロパティ
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Head() As RootHead
            Set(ByVal value As RootHead)
                outHead = value
            End Set
            Get
                Return outHead
            End Get
        End Property

        ''' <summary>
        ''' Detailタグ用プロパティ
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Detail() As RootDetail
            Set(ByVal value As RootDetail)
                outDetail = value
            End Set
            Get
                Return outDetail
            End Get
        End Property

        ''' <summary>
        ''' Headタグ用クラス
        ''' </summary>
        ''' <remarks></remarks>
        Public Class RootHead

            ''' <summary>
            ''' TransmissionDateタグの定義
            ''' </summary>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="TransmissionDate",
                                                          IsNullable:=False)> _
            Private outTransmissionDate As String

            ''' <summary>
            ''' TransmissionDateタグ用のプロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="TransmissionDate",
                                                          IsNullable:=False)> _
            Public Property TransmissionDate As String
                Get
                    Return outTransmissionDate
                End Get
                Set(ByVal value As String)
                    outTransmissionDate = value
                End Set
            End Property
        End Class

        ''' <summary>
        ''' Detailタグ用クラス
        ''' </summary>
        ''' <remarks></remarks>
        Public Class RootDetail

            ''' <summary>
            ''' Commonタグの定義
            ''' </summary>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="Common",
                                                          IsNullable:=False)> _
            Private outCommon As DetailCommon

            ''' <summary>
            ''' Commonタグ用のプロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Property Common() As DetailCommon
                Set(ByVal value As DetailCommon)
                    outCommon = value
                End Set
                Get
                    Return outCommon
                End Get
            End Property

            ''' <summary>
            ''' Commonタグ用クラス
            ''' </summary>
            ''' <remarks></remarks>
            Public Class DetailCommon

                ''' <summary>
                ''' NoticeRequestIdタグの定義
                ''' </summary>
                ''' <remarks></remarks>
                <System.Xml.Serialization.XmlElementAttribute(ElementName:="NoticeRequestId",
                                                              IsNullable:=False)> _
                Private outNoticeRequestId As String

                ''' <summary>
                ''' ResultIdタグの定義
                ''' </summary>
                ''' <remarks></remarks>
                <System.Xml.Serialization.XmlElementAttribute(ElementName:="ResultId",
                                                              IsNullable:=False)> _
                Private outResultId As String

                ''' <summary>
                ''' Messageタグの定義
                ''' </summary>
                ''' <remarks></remarks>
                <System.Xml.Serialization.XmlElementAttribute(ElementName:="Message",
                                                              IsNullable:=False)> _
                Private outMessage As String

                ''' <summary>
                ''' NoticeRequestIdタグ用のプロパティ
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                Public Property NoticeRequestId() As String
                    Set(ByVal value As String)
                        outNoticeRequestId = value
                    End Set
                    Get
                        Return outNoticeRequestId
                    End Get
                End Property

                ''' <summary>
                ''' ResultIdタグ用のプロパティ
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                Public Property ResultId() As String
                    Set(ByVal value As String)
                        outResultId = value
                    End Set
                    Get
                        Return outResultId
                    End Get
                End Property

                ''' <summary>
                ''' Messageタグ用のプロパティ
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                Public Property Message() As String
                    Set(ByVal value As String)
                        outMessage = value
                    End Set
                    Get
                        Return outMessage
                    End Get
                End Property
            End Class
        End Class
    End Class
#End Region

#Region "IDisposableインターフェイス"
    ''' <summary>
    ''' IDisposableインターフェイス.Dispoase
    ''' </summary>
    ''' <remarks></remarks>
    Public Overloads Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
    Protected Overridable Overloads Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            Me.daDataSetTableAdapters.Dispose()
            Me.dtNoticeRequest.Dispose()
            Me.dtNoticeInfo.Dispose()
            Me.dtSelectNoticeInfo.Dispose()
            Me.dtSelectNoticeRequest.Dispose()
            Me.xmlAccountData.Dispose()
            Me.pushInfoData.Dispose()
            Me.requestNoticeData.Dispose()
            If Not IsNothing(Me.noticeDBClone) Then
                Me.noticeDBClone.Dispose()
            End If

            Me.daDataSetTableAdapters = Nothing
            Me.dtNoticeRequest = Nothing
            Me.dtNoticeInfo = Nothing
            Me.dtSelectNoticeInfo = Nothing
            Me.dtSelectNoticeRequest = Nothing
            Me.xmlAccountData = Nothing
            Me.pushInfoData = Nothing
            Me.requestNoticeData = Nothing
            Me.noticeDBClone = Nothing
        End If
    End Sub
#End Region

End Class
