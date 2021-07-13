'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3070204BusinessLogic.vb
'─────────────────────────────────────
'機能： 見積書・契約書印刷処理
'補足： 
'作成： 2012/11/16 TCS 坪根
'更新： 2013/01/10 TCS 橋本 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発
'更新： 2013/03/08 TCS 山田 【A.STEP2】新車タブレット受付画面の管理指標変更対応
'更新： 2013/06/30 TCS 山田 2013/10対応版 既存流用
'更新： 2013/10/25 TCS 葛西 次世代e-CRBセールス機能 新DB適応に向けた機能開発
'更新： 2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展
'─────────────────────────────────────
Imports System.Text
Imports System.IO
Imports System.Xml
Imports System.Globalization
Imports System.Web

Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.Estimate.Quotation.DataAccess
Imports Toyota.eCRB.Estimate.Quotation.DataAccess.IC3070201DataSet
Imports Toyota.eCRB.Tool.Notify.Api.BizLogic
Imports Toyota.eCRB.Tool.Notify.Api.DataAccess
'2013/01/10 TCS 橋本 【A.STEP2】Add Start
Imports Toyota.eCRB.CommonUtility.BizLogic
Imports System.Reflection

'2013/01/10 TCS 橋本 【A.STEP2】Add End

Public Class SC3070204BusinessLogic
    Inherits BaseBusinessComponent
    Implements ISC3070204BusinessLogic

    ''' <summary>
    ''' デフォルトコンストラクタ
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        Me._msgId = 0
    End Sub

#Region "定数"

    ''' <summary>
    ''' プログラムID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PROGRAM_ID As String = "SC3070204"

    ''' <summary>
    ''' メッセージID (ID:0)
    ''' </summary>
    ''' <remarks>
    ''' 正常
    ''' </remarks>
    Private Const MESSAGE_ID_SUCCESS As Integer = 0

    ''' <summary>
    ''' メッセージID (ID:907)
    ''' </summary>
    ''' <remarks>
    ''' 見積書は他の方により削除されている可能性があります。
    ''' </remarks>
    Private Const MESSAGE_ID_907 As Integer = 907

    '2013/03/08 TCS 山田 【A.STEP2】新車タブレット受付画面の管理指標変更対応 START
    ''' <summary>
    ''' メッセージID (ID:911)
    ''' </summary>
    ''' <remarks>
    ''' 注文書は他の方により削除されている可能性があります。
    ''' </remarks>
    Private Const MESSAGE_ID_911 As Integer = 911
    '2013/03/08 TCS 山田 【A.STEP2】新車タブレット受付画面の管理指標変更対応 END

    ''' <summary>
    ''' メッセージID (ID:908)
    ''' </summary>
    ''' <remarks>
    ''' 契約書は他の方により削除されている可能性があります。
    ''' </remarks>
    Private Const MESSAGE_ID_908 As Integer = 908

    ''' <summary>
    ''' メッセージID (ID:909)
    ''' </summary>
    ''' <remarks>
    ''' DMSとの連携に失敗しました。
    ''' </remarks>
    Private Const MESSAGE_ID_909 As Integer = 909

    ''' <summary>
    ''' メッセージID (ID:910)
    ''' </summary>
    ''' <remarks>
    ''' 依頼がキャンセルされました。
    ''' </remarks>
    Private Const MESSAGE_ID_910 As Integer = 910

    ''' <summary>
    ''' 印刷モード　見積書
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PRINT_MODE_ESTIMATION As String = "1"

    '2013/03/08 TCS 山田 【A.STEP2】新車タブレット受付画面の管理指標変更対応 START
    ''' <summary>
    ''' 印刷モード　注文書
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PRINT_MODE_ORDER As String = "3"
    '2013/03/08 TCS 山田 【A.STEP2】新車タブレット受付画面の管理指標変更対応 END

    ''' <summary>
    ''' 印刷モード　契約書
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PRINT_MODE_CONTRACT As String = "2"

    ''' <summary>
    ''' 印刷先のアプリケーションID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PRINT_APP_ID As String = "MC3B40002"

    ''' <summary>
    ''' 契約状況フラグ　契約済み
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONTRACTFLG_CONTRACT As String = "1"

    ''' <summary>
    ''' 契約状況フラグ　キャンセル
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONTRACTFLG_CANCEL As String = "2"

    ''' <summary>
    ''' 契約顧客種別 所有者
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONTRACTCUSTTYPE_DEALER As String = "1"

    ''' <summary>
    ''' 費用項目コード　手続き費用
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ITEM_CODE_PURCHASE As String = "1"

    ''' <summary>
    ''' 費用項目コード　購入税
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ITEM_CODE_REGISTRAION As String = "2"

    ''' <summary>
    ''' 支払区分　現金
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PAYMENTMETHOD_MONEY As String = "1"

    ''' <summary>
    ''' 支払区分　ローン
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PAYMENTMETHOD_LOAN As String = "2"

    ''' <summary>
    ''' TBL_DLRENVSETTINGマスタのパラメータ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DLRENVSETTING_TACT As String = "TACT_ORDER_PATH"

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
    ''' ログ出力メッセージ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ERROR_MSG As String = "Tact Error : ReturnId = "

    ''' <summary>
    ''' 敬称位置のパラメータ名
    ''' </summary>
    ''' <remarks>
    ''' 1: 名前の前に敬称(主に英語圏)、2: 名前の後ろに敬称(中国など)
    ''' </remarks>
    Private Const PARAM_NAME_TITLE_POS As String = "KEISYO_ZENGO"

    ''' <summary>
    ''' 敬称のパラメータ名
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PARAM_NAME_TITLE As String = "HONORIFIC_TITLE"

    ''' <summary>
    ''' 変換フォーマット
    ''' </summary>
    ''' <remarks>YYYY/MM/DD HH:MM:SS</remarks>
    Private Const FORMAT_YYYYMMDDHHMMSS As Integer = 1

    ''' <summary>
    ''' 変換フォーマット
    ''' </summary>
    ''' <remarks>YYYY/MM/DD</remarks>
    Private Const FORMAT_YYYYMMDD As Integer = 3

    ''' <summary>
    ''' 実行モード　見積情報取得用
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ESTIMATION_MODE_ALL As Integer = 0

    '2013/06/30 TCS 山田 2013/10対応版 既存流用 START
    ''' <summary>
    ''' 変換モード　見積情報取得用
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CHANGE_MODE_NOT_TCV As Integer = 0
    '2013/06/30 TCS 山田 2013/10対応版 既存流用 END

    ''' <summary>
    ''' 列名　販売店コード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CLM_DLRCD = "DLRCD"

    ''' <summary>
    ''' 列名　保険会社コード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CLM_INSUCOMCD = "INSUCOMCD"

    ''' <summary>
    ''' 列名　保険種別
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CLM_INSUKIND = "INSUKIND"

    ''' <summary>
    ''' 列名　デフォルト敬称
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CLM_DFLTNAMETITLE = "DEFOLTNAMETITLE"

    ''' <summary>
    ''' 列名　敬称位置
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CLM_NAMETITLEPOSITION = "NAMETITLEPOSITION"

    ''' <summary>
    ''' 納車予定日
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CLM_DELIDATE = "DELIDATE"

    ''' <summary>
    ''' 列名　融資会社コード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CLM_FINANCECOMCODE = "FINANCECOMCODE"

    ''' <summary>
    ''' 列名　支払い方法
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CLM_PAYMENTMETHOD = "PAYMENTMETHOD"

    ''' <summary>
    ''' テーブル名　見積保険情報(I/F)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IFTBL_INSINFO = "IC3070201EstInsuranceInfo"

    ''' <summary>
    ''' テーブル名　支払い情報(I/F)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IFTBL_PAYINFO = "IC3070201PaymentInfo"

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
    ''' テーブル名　車両オプション情報(I/F)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IFTBL_VCLOPTIONINFO = "IC3070201VclOptionInfo"

    ''' <summary>
    ''' テーブル名　下取り車両情報(I/F)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IFTBL_TRADEINCARINFO = "IC3070201TradeincarInfo"

    ''' <summary>
    ''' テーブル名　諸費用情報(I/F)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IFTBL_CHARGEINFO = "IC3070201ChargeInfo"

    ''' <summary>
    ''' テーブル名　保険会社情報
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TBL_INSINFO = "SC3070204InsKindMast"

    ''' <summary>
    ''' テーブル名　融資会社情報
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TBL_FINANCEINFO = "SC3070204FinanceComMast"

    ''' <summary>
    ''' テーブル名　システム環境情報
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TBL_SYSTEMINFO = "SC3070204SystemEnvSetting"

    ''' <summary>
    ''' テーブル名　印刷情報(基本)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TBL_PRINTINFO_BASIC = "SC3070204PrintInfoBasic"

    ''' <summary>
    ''' テーブル名　印刷情報(オプション)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TBL_PRINTINFO_OPTION = "SC3070204PrintInfoOption"

    ''' <summary>
    ''' テーブル名　印刷情報(下取車両情報)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TBL_PRINTINFO_TRADEINCAR = "SC3070204PrintInfoTradeInCar"

    '2013/03/08 TCS 山田 【A.STEP2】新車タブレット受付画面の管理指標変更対応 START
    ''' <summary>
    ''' テーブル名　印刷情報(諸費用情報)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TBL_PRINTINFO_CHARGE = "SC3070204PrintInfoCharge"
    '2013/03/08 TCS 山田 【A.STEP2】新車タブレット受付画面の管理指標変更対応 END

#End Region

#Region "メンバ変数"

    ''' <summary>
    ''' メッセージID
    ''' </summary>
    ''' <remarks></remarks>
    Private _msgId As Integer = 0

#End Region

#Region "プロパティ"

    ''' <summary>
    ''' メッセージID
    ''' </summary>
    ''' <value>メッセージID</value>
    ''' <returns></returns>
    ''' <remarks>0の場合は正常、それ以外の場合エラー</remarks>
    Public ReadOnly Property MsgId() As Integer
        Get
            Return Me._msgId
        End Get
    End Property

#End Region

#Region "Publicメソッド"

    ''' <summary>
    ''' 印刷情報取得
    ''' </summary>
    ''' <param name="estimateid">見積管理ID</param>
    ''' <param name="paymentKbn">支払方法区分</param>
    ''' <returns>データセット(SC3070204アウトプット)</returns>
    ''' <remarks></remarks>
    Public Function GetDataPrintInfo(ByVal estimateid As Long, _
                                     ByVal paymentKbn As String) As SC3070204DataSet

        '2013/06/30 TCS 山田 2013/10対応版 既存流用 START
        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  " {0}_Start",
                                  MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================
        '2013/06/30 TCS 山田 2013/10対応版 既存流用 END

        Dim retDtSC3070204 As SC3070204DataSet = Nothing

        'メッセージIDの初期設定
        Me._msgId = MESSAGE_ID_SUCCESS

        '見積情報取得 
        Dim apiBiz As New IC3070201BusinessLogic
        Dim apiDt As IC3070201DataSet
        '2013/06/30 TCS 山田 2013/10対応版 既存流用 START
        apiDt = apiBiz.GetEstimationInfo(estimateid, ESTIMATION_MODE_ALL, CHANGE_MODE_NOT_TCV)
        '2013/06/30 TCS 山田 2013/10対応版 既存流用 END

        '自画面データセット生成（返却用ではない）
        Using estDtSC3070204 As New SC3070204DataSet

            'DataSetの中のテーブルを全て削除(I/Fのやり方で統一)
            estDtSC3070204.Tables.Clear()

            '保険情報取得
            Dim insuranceDt As SC3070204DataSet.SC3070204InsKindMastDataTable = estDtSC3070204.SC3070204InsKindMast
            If 0 < apiDt.Tables(IFTBL_INSINFO).Rows.Count Then
                insuranceDt = Me.GetInsuranceCompanyInfo(apiDt, estDtSC3070204)
            End If

            '支払い方法
            Dim payMethod As String = paymentKbn.ToString(CultureInfo.CurrentCulture)

            '融資情報取得
            Dim financeDt As SC3070204DataSet.SC3070204FinanceComMastDataTable = estDtSC3070204.SC3070204FinanceComMast
            If 0 < apiDt.Tables(IFTBL_PAYINFO).Rows.Count Then
                financeDt = Me.GetFinanceCompanyInfo(apiDt, estDtSC3070204, payMethod)
            End If

            '販売店情報取得
            Dim nameTitleDt As SC3070204DataSet.SC3070204SystemEnvSettingDataTable = Me.GetNameTitleInfo()

            'データセットにテーブル追加
            estDtSC3070204.Tables.Add(insuranceDt)
            estDtSC3070204.Tables.Add(financeDt)
            estDtSC3070204.Tables.Add(nameTitleDt)

            '店舗情報取得
            Dim branchBiz As New Branch
            Dim staff As StaffContext = StaffContext.Current
            Dim branchDt As BranchDataSet.BRANCHRow = branchBiz.GetBranch(staff.DlrCD, staff.BrnCD)

            '印刷情報をデータセットに設定
            retDtSC3070204 = Me.SetDataPrintInfo(apiDt, _
                                                 branchDt, _
                                                 estDtSC3070204, _
                                                 paymentKbn)

            '2013/06/30 TCS 山田 2013/10対応版 既存流用 START
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      " {0}_End, Return:[{1}]",
                                      MethodBase.GetCurrentMethod.Name,
                                      retDtSC3070204.Tables(TBL_PRINTINFO_BASIC).Rows.Count))
            ' ======================== ログ出力 終了 ========================
            '2013/06/30 TCS 山田 2013/10対応版 既存流用 END

            Return retDtSC3070204
        End Using

    End Function

    '2013/03/08 TCS 山田 【A.STEP2】新車タブレット受付画面の管理指標変更対応 START
    ''' <summary>
    ''' 印刷情報のXMLを作成
    ''' </summary>
    ''' <param name="dtSC3070204">SC3070204データセット(インプット)</param>
    ''' <param name="printMode">印刷モード(1:見積書印刷, 2:契約書印刷, 3:注文書印刷)</param>
    ''' <returns>印刷情報XML</returns>
    ''' <remarks></remarks>
    Public Function GetXmlPrintInfo(ByVal dtSC3070204 As SC3070204DataSet,
                                    ByVal printMode As String) As String
        '2013/03/08 TCS 山田 【A.STEP2】新車タブレット受付画面の管理指標変更対応 END

        '印刷情報(基本)
        Dim printDataBasicRow As SC3070204DataSet.SC3070204PrintInfoBasicRow
        printDataBasicRow = CType(dtSC3070204.Tables(TBL_PRINTINFO_BASIC).Rows(0), SC3070204DataSet.SC3070204PrintInfoBasicRow)

        Using writer As New StringWriter(CultureInfo.InvariantCulture)
            Using xmlWriter As XmlTextWriter = New XmlTextWriter(writer)
                Logger.Info("GetXmlPrintInfo Start")

                '▼共通部分▼
                xmlWriter.WriteStartElement("CommonCommunication")      'CommonCommunicationタグ (Start)
                xmlWriter.WriteStartElement("Head")                     '    Headタグ (Start)
                xmlWriter.WriteStartElement("Destination")              '        Destinationタグ (Start)

                'アプリケーションID
                xmlWriter.WriteElementString("ApplicationID", PRINT_APP_ID)

                xmlWriter.WriteEndElement()                             '        Destinationタグ (End)
                xmlWriter.WriteEndElement()                             '    Headタグ (End)

                '▼個別部分▼
                xmlWriter.WriteStartElement("Detail")                   '    Detailタグ (Start)
                xmlWriter.WriteStartElement("Request")                  '        Requestタグ (Start)
                xmlWriter.WriteStartElement("Head")                     '            Headタグ (Start)

                '送信日時
                xmlWriter.WriteElementString("TransmissionDate", _
                                             DateTimeFunc.FormatDate(FORMAT_YYYYMMDDHHMMSS, DateTimeFunc.Now))

                xmlWriter.WriteEndElement()                             '            Headタグ (End)
                xmlWriter.WriteStartElement("Detail")                   '            Detailタグ (Start)
                xmlWriter.WriteStartElement("Common")                   '                Commonタグ (Start)

                '印刷モード
                xmlWriter.WriteElementString("PrintMode", printMode)

                xmlWriter.WriteEndElement()                             '                Commonタグ (End)
                xmlWriter.WriteStartElement("PrintInfo")                '                PrintInfoタグ (Start)

                '契約書No
                xmlWriter.WriteElementString("ContractNo", printDataBasicRow.CONTRACT_NO)
                '契約顧客区分
                xmlWriter.WriteElementString("ContractCustType", printDataBasicRow.CONTRACT_CUST_TYPE)
                '顧客区分
                xmlWriter.WriteElementString("CustomerPart", printDataBasicRow.CUSTOMER_PART)
                'お客様氏名
                xmlWriter.WriteElementString("CustomerNm", printDataBasicRow.CUSTOMER_NM)
                '敬称
                xmlWriter.WriteElementString("CustomerNmTitle", printDataBasicRow.CUSTOMER_NM_TITLE)
                '敬称位置
                xmlWriter.WriteElementString("CustomerNmTitlePos", printDataBasicRow.CUSTOMER_NM_TITLE_POS)
                'お客様携帯番号
                xmlWriter.WriteElementString("CustomerMobile", printDataBasicRow.CUSTOMER_MOBILE)
                'お客様住所
                xmlWriter.WriteElementString("CustomerAddress", printDataBasicRow.CUSTOMER_ADDRESS)
                'お客様郵便番号
                xmlWriter.WriteElementString("CustomerZipCd", printDataBasicRow.CUSTOMER_ZIPCD)
                'お客様電話番号
                xmlWriter.WriteElementString("CustomerTelNo", printDataBasicRow.CUSTOMER_TELNO)
                'お客様FAX
                xmlWriter.WriteElementString("CustomerFax", printDataBasicRow.CUSTOMER_FAX)
                '国民番号
                xmlWriter.WriteElementString("CustomerSocialId", printDataBasicRow.CUSTOMER_SOCIALID)
                'お客様EMail
                xmlWriter.WriteElementString("CustomerMail", printDataBasicRow.CUSTOMER_MAIL)

                '2013/10/25 TCS 葛西 次世代e-CRBセールス機能 新DB適応に向けた機能開発 ADD START
                '顧客区分（使用者）
                xmlWriter.WriteElementString("UserPart", printDataBasicRow.USER_PART)
                'お客様氏名（使用者）
                xmlWriter.WriteElementString("UserNm", printDataBasicRow.USER_NM)
                'お客様携帯番号（使用者）
                xmlWriter.WriteElementString("UserMobile", printDataBasicRow.USER_MOBILE)
                'お客様住所（使用者）
                xmlWriter.WriteElementString("UserAddress", printDataBasicRow.USER_ADDRESS)
                'お客様郵便番号（使用者）
                xmlWriter.WriteElementString("UserZipCd", printDataBasicRow.USER_ZIPCD)
                'お客様電話番号（使用者）
                xmlWriter.WriteElementString("UserTelNo", printDataBasicRow.USER_TELNO)
                'お客様FAX（使用者）
                xmlWriter.WriteElementString("UserFax", printDataBasicRow.USER_FAX)
                '国民番号（使用者）
                xmlWriter.WriteElementString("UserSocialId", printDataBasicRow.USER_SOCIALID)
                'お客様EMail（使用者）
                xmlWriter.WriteElementString("UserMail", printDataBasicRow.USER_MAIL)
                '2013/10/25 TCS 葛西 次世代e-CRBセールス機能 新DB適応に向けた機能開発 ADD END

                '本日日付
                xmlWriter.WriteElementString("Today", DateTimeFunc.FormatDate(FORMAT_YYYYMMDD, DateTimeFunc.Now) & " 00:00:00")
                '販売店名称
                xmlWriter.WriteElementString("SellesNm", printDataBasicRow.SELLES_NM)
                '販売店住所
                xmlWriter.WriteElementString("SellesAddress", printDataBasicRow.SELLES_ADDRESS)
                '販売店電話番号
                xmlWriter.WriteElementString("SellesTelNo", printDataBasicRow.SELLES_TELNO)
                '販売店FAX
                xmlWriter.WriteElementString("SellesFax", printDataBasicRow.SELLES_FAX)
                'サービス電話番号
                xmlWriter.WriteElementString("ServiceTelNo", printDataBasicRow.SERVICE_TELNO)
                'スタッフ名称
                xmlWriter.WriteElementString("StaffNm", printDataBasicRow.STAFF_NM)
                'シリーズ名称
                xmlWriter.WriteElementString("SeriesNm", printDataBasicRow.SERIES_NM)
                'モデル名称
                xmlWriter.WriteElementString("ModelNm", printDataBasicRow.MODEL_NM)
                'ボディータイプ
                xmlWriter.WriteElementString("BodyType", printDataBasicRow.BODY_TYPE)
                '排気量
                xmlWriter.WriteElementString("Displacement", printDataBasicRow.DISPLACEMENT)
                '駆動方式
                xmlWriter.WriteElementString("DriveSystem", printDataBasicRow.DRIVESYSTEM)
                'トランスミッション
                xmlWriter.WriteElementString("TransMission", printDataBasicRow.TRANSMISSION)
                '外装色名称
                xmlWriter.WriteElementString("ExtColorNm", printDataBasicRow.EXTCOLORNM)
                '内装色名称
                xmlWriter.WriteElementString("IntColorNm", printDataBasicRow.INTCOLORNM)
                '車両番号
                xmlWriter.WriteElementString("ModelNumber", printDataBasicRow.MODELNUMBER)
                'サフィックス
                xmlWriter.WriteElementString("SuffixCd", printDataBasicRow.SUFFIXCD)
                '本体車両価格
                xmlWriter.WriteElementString("BasePrice", printDataBasicRow.BASEPRICE)
                '値引き
                xmlWriter.WriteElementString("DisCountPrice", printDataBasicRow.DISCOUNTPRICE)
                '外装追加費用
                xmlWriter.WriteElementString("ExtAmount", printDataBasicRow.EXTAMOUNT)
                '内装追加費用
                xmlWriter.WriteElementString("IntAmount", printDataBasicRow.INTAMOUNT)
                '車両購入税
                xmlWriter.WriteElementString("BasePurchaseTax", printDataBasicRow.BASEPURCHASETAX)
                '登録費用
                xmlWriter.WriteElementString("RegistAmount", printDataBasicRow.REGISTAMOUNT)
                '保険会社名称
                xmlWriter.WriteElementString("InsuranceCompany", printDataBasicRow.INSURANCE_COMPANY)
                '保険種別名称
                xmlWriter.WriteElementString("InsuranceKindNm", printDataBasicRow.INSURANCE_KINDNM)
                '保険費用
                xmlWriter.WriteElementString("InsuranceAmount", printDataBasicRow.INSURANCE_AMOUNT)
                '融資会社名称
                xmlWriter.WriteElementString("FinanceCompanyNm", printDataBasicRow.FINANCE_COMPANYNM)
                '支払方法区分
                xmlWriter.WriteElementString("PaymentMethod", printDataBasicRow.PAYMENT_METHOD)
                '支払期間
                xmlWriter.WriteElementString("PaymentPeriod", printDataBasicRow.PAYMENT_PERIOD)
                '毎月返済額
                xmlWriter.WriteElementString("MonthlyPayment", printDataBasicRow.MONTHLYPAYMENT)
                '頭金
                xmlWriter.WriteElementString("Deposit", printDataBasicRow.DEPOSIT)
                'ボーナス時返済額
                xmlWriter.WriteElementString("BonusPayment", printDataBasicRow.BONUSPAYMENT)
                '初回支払期限
                xmlWriter.WriteElementString("DueDate", printDataBasicRow.DUEDATE)
                '2013/03/08 TCS 山田 【A.STEP2】新車タブレット受付画面の管理指標変更対応 START
                '利率
                xmlWriter.WriteElementString("InterestRate", printDataBasicRow.INTERESTRATE)
                '2013/03/08 TCS 山田 【A.STEP2】新車タブレット受付画面の管理指標変更対応 END
                '納車予定日
                xmlWriter.WriteElementString("DeliDate", printDataBasicRow.DELIDATE)
                'メモ
                xmlWriter.WriteElementString("Memo", printDataBasicRow.MEMO)

                printDataBasicRow = Nothing

                'オプション情報
                Dim dtPrintDataOption As SC3070204DataSet.SC3070204PrintInfoOptionDataTable = Nothing
                dtPrintDataOption = CType(dtSC3070204.Tables(TBL_PRINTINFO_OPTION), SC3070204DataSet.SC3070204PrintInfoOptionDataTable)

                For Each printDataOptionRow As SC3070204DataSet.SC3070204PrintInfoOptionRow In dtPrintDataOption.Rows()
                    xmlWriter.WriteStartElement("OptionInfo")               '                    OptionInfoタグ (Start)

                    'オプション名称
                    xmlWriter.WriteElementString("Name", printDataOptionRow.NAME)
                    'オプション価格
                    xmlWriter.WriteElementString("Price", printDataOptionRow.PRICE)
                    'オプション取付費用
                    xmlWriter.WriteElementString("InstallCost", printDataOptionRow.INSTALLCOST)

                    xmlWriter.WriteEndElement()                             '                    OptionInfoタグ (End)
                Next
                dtPrintDataOption.Dispose()
                dtPrintDataOption = Nothing

                '下取車両情報
                Dim dtPrintDataTradeinCar As SC3070204DataSet.SC3070204PrintInfoTradeInCarDataTable = Nothing
                dtPrintDataTradeinCar = CType(dtSC3070204.Tables(TBL_PRINTINFO_TRADEINCAR), SC3070204DataSet.SC3070204PrintInfoTradeInCarDataTable)

                For Each printDataTradeinCarRow As SC3070204DataSet.SC3070204PrintInfoTradeInCarRow In dtPrintDataTradeinCar.Rows()
                    xmlWriter.WriteStartElement("TradeincarInfo")           '                    TradeincarInfoタグ (Start)

                    'オプション名称
                    xmlWriter.WriteElementString("Name", printDataTradeinCarRow.NAME)
                    'オプション価格
                    xmlWriter.WriteElementString("Price", printDataTradeinCarRow.PRICE)

                    xmlWriter.WriteEndElement()                             '                    TradeincarInfoタグ (End)
                Next
                dtPrintDataTradeinCar = Nothing

                '2013/03/08 TCS 山田 【A.STEP2】新車タブレット受付画面の管理指標変更対応 START
                '諸費用情報
                Dim dtPrintDataCharge As SC3070204DataSet.SC3070204PrintInfoChargeDataTable = Nothing
                dtPrintDataCharge = CType(dtSC3070204.Tables(TBL_PRINTINFO_CHARGE), SC3070204DataSet.SC3070204PrintInfoChargeDataTable)

                For Each printDataChargeRow As SC3070204DataSet.SC3070204PrintInfoChargeRow In dtPrintDataCharge.Rows()
                    xmlWriter.WriteStartElement("ChargeInfo")           '                    ChargeInfoタグ (Start)

                    '費用項目名
                    xmlWriter.WriteElementString("Name", printDataChargeRow.NAME)
                    '価格
                    xmlWriter.WriteElementString("Price", printDataChargeRow.PRICE)

                    xmlWriter.WriteEndElement()                             '                    ChargeInfoタグ (End)
                Next
                dtPrintDataCharge = Nothing
                '2013/03/08 TCS 山田 【A.STEP2】新車タブレット受付画面の管理指標変更対応 END

                xmlWriter.WriteEndElement()                             '                PrintInfoタグ (End)
                xmlWriter.WriteEndElement()                             '            Detailタグ (End)
                xmlWriter.WriteEndElement()                             '        Requestタグ (End)
                xmlWriter.WriteEndElement()                             '    Detailタグ (End)
                xmlWriter.WriteEndElement()                             'CommonCommunicationタグ (End)

                Dim dataXml As String = writer.GetStringBuilder.ToString

                '先頭にエンコードタグを挿入
                dataXml = dataXml.Insert(0, "<?xml version=""1.0"" encoding=""utf-8"" ?>")

                ' ログ書き出し
                Logger.Info("Xml PrintData = " & dataXml, True)
                Logger.Info("GetXmlPrintInfo End")

                Return dataXml
            End Using
        End Using

    End Function

    ''' <summary>
    ''' 見積印刷日更新
    ''' </summary>
    ''' <param name="estimateid">見積管理ID</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Function UpdateEstimatePrintDate(ByVal estimateid As Long) As Boolean Implements ISC3070204BusinessLogic.UpdateEstimatePrintDate

        '2013/06/30 TCS 山田 2013/10対応版 既存流用 START
        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  " {0}_Start",
                                  MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================
        '2013/06/30 TCS 山田 2013/10対応版 既存流用 END

        Dim staff As StaffContext = StaffContext.Current
        Dim ret As Integer = 1

        Dim da As New SC3070204TableAdapter

        'メッセージIDの初期設定
        Me._msgId = MESSAGE_ID_SUCCESS

        '2013/06/30 TCS 山田 2013/10対応版 既存流用 START
        '見積情報更新ロック取得
        Try
            da.GetEstimateinfoLock(estimateid)
        Catch ex As OracleExceptionEx

            Me._msgId = MESSAGE_ID_907

            '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 START
            ' ======================== ログ出力 開始 ========================
            Logger.Error(String.Format(CultureInfo.InvariantCulture,
                                      " {0}_End, Return:[{1}]",
                                      MethodBase.GetCurrentMethod.Name, "False"))
            ' ======================== ログ出力 終了 ========================
            '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 END

            Return False
        End Try
        '2013/06/30 TCS 山田 2013/10対応版 既存流用 END

        '更新に失敗していたらメッセージを表示
        Try
            '見積印刷日更新
            ret = da.UpdateEstimatePrintDate(estimateid, _
                                             staff.Account, _
                                             PROGRAM_ID)
            '更新に失敗していたらロールバック
            If ret = 0 Then
                'エラー
                Me.Rollback = True

                Me._msgId = MESSAGE_ID_907

                '2013/06/30 TCS 山田 2013/10対応版 既存流用 START
                '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 START
                ' ======================== ログ出力 開始 ========================
                Logger.Error(String.Format(CultureInfo.InvariantCulture,
                                           " {0}_End, Return:[{1}]",
                                          MethodBase.GetCurrentMethod.Name, "False"))
                ' ======================== ログ出力 終了 ========================
                '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 END
                '2013/06/30 TCS 山田 2013/10対応版 既存流用 END
                Return False
            End If
        Catch ex As OracleExceptionEx
            'エラー
            Me.Rollback = True

            Me._msgId = MESSAGE_ID_907

            '2013/06/30 TCS 山田 2013/10対応版 既存流用 START
            '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 START
            ' ======================== ログ出力 開始 ========================
            Logger.Error(String.Format(CultureInfo.InvariantCulture,
                                       " {0}_End, Return:[{1}]",
                                      MethodBase.GetCurrentMethod.Name, "False"))
            ' ======================== ログ出力 終了 ========================
            '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 END
            '2013/06/30 TCS 山田 2013/10対応版 既存流用 END
            Return False
        End Try

        '2013/06/30 TCS 山田 2013/10対応版 既存流用 START
        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  " {0}_End, Return:[{1}]",
                                  MethodBase.GetCurrentMethod.Name, "True"))
        ' ======================== ログ出力 終了 ========================
        '2013/06/30 TCS 山田 2013/10対応版 既存流用 END
        Return True

    End Function

    '2013/03/08 TCS 山田 【A.STEP2】新車タブレット受付画面の管理指標変更対応 START
    ''' <summary>
    ''' 契約書印刷フラグ更新
    ''' </summary>
    ''' <param name="estimateid">見積管理ID</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Function UpdateContractPrintFlg(ByVal estimateid As Long, _
                                           ByVal method As String) As Boolean Implements ISC3070204BusinessLogic.UpdateContractPrintFlg

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  " {0}_Start",
                                  MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================
        '2013/06/30 TCS 山田 2013/10対応版 既存流用 END

        Dim staff As StaffContext = StaffContext.Current
        Dim ret As Integer = 1

        Dim da As New SC3070204TableAdapter

        'メッセージIDの初期設定
        Me._msgId = MESSAGE_ID_SUCCESS

        '2013/06/30 TCS 山田 2013/10対応版 既存流用 START
        '見積情報更新ロック取得
        Try
            da.GetEstimateinfoLock(estimateid)
        Catch ex As OracleExceptionEx

            If method = "OrderUpdateContractPrintFlg" Then
                Me._msgId = MESSAGE_ID_911
            Else
                Me._msgId = MESSAGE_ID_908
            End If

            '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 START
            ' ======================== ログ出力 開始 ========================
            Logger.Error(String.Format(CultureInfo.InvariantCulture,
                                       " {0}_End, Return:[{1}]",
                                      MethodBase.GetCurrentMethod.Name, "False"))
            ' ======================== ログ出力 終了 ========================
            '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 END
            Return False
        End Try
        '2013/06/30 TCS 山田 2013/10対応版 既存流用 END

        '更新に失敗していたらメッセージを表示
        Try
            '契約書印刷フラグ更新
            ret = da.UpdateContractPrintFlg(estimateid, _
                                            staff.Account, _
                                            PROGRAM_ID)
            '更新に失敗していたらロールバック
            If ret = 0 Then
                'エラー
                Me.Rollback = True

                '2013/03/08 TCS 山田 【A.STEP2】新車タブレット受付画面の管理指標変更対応 START
                If method = "OrderUpdateContractPrintFlg" Then
                    Me._msgId = MESSAGE_ID_911
                Else
                    Me._msgId = MESSAGE_ID_908
                End If
                '2013/03/08 TCS 山田 【A.STEP2】新車タブレット受付画面の管理指標変更対応 END

                '2013/06/30 TCS 山田 2013/10対応版 既存流用 START
                '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 START
                ' ======================== ログ出力 開始 ========================
                Logger.Error(String.Format(CultureInfo.InvariantCulture,
                                           " {0}_End, Return:[{1}]",
                                          MethodBase.GetCurrentMethod.Name, "False"))
                ' ======================== ログ出力 終了 ========================
                '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 END
                '2013/06/30 TCS 山田 2013/10対応版 既存流用 END
                Return False
            End If
        Catch ex As OracleExceptionEx
            'エラー
            Me.Rollback = True

            '2013/03/08 TCS 山田 【A.STEP2】新車タブレット受付画面の管理指標変更対応 START
            If method = "OrderUpdateContractPrintFlg" Then
                Me._msgId = MESSAGE_ID_911
            Else
                Me._msgId = MESSAGE_ID_908
            End If
            '2013/03/08 TCS 山田 【A.STEP2】新車タブレット受付画面の管理指標変更対応 END

            '2013/06/30 TCS 山田 2013/10対応版 既存流用 START
            '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 START
            ' ======================== ログ出力 開始 ========================
            Logger.Error(String.Format(CultureInfo.InvariantCulture,
                                      " {0}_End, Return:[{1}]",
                                      MethodBase.GetCurrentMethod.Name, "False"))
            ' ======================== ログ出力 終了 ========================
            '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 END
            '2013/06/30 TCS 山田 2013/10対応版 既存流用 END
            Return False
        End Try

        '2013/06/30 TCS 山田 2013/10対応版 既存流用 START
        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  " {0}_End, Return:[{1}]",
                                  MethodBase.GetCurrentMethod.Name, "True"))
        ' ======================== ログ出力 終了 ========================
        '2013/06/30 TCS 山田 2013/10対応版 既存流用 END
        Return True

    End Function

    ''' <summary>
    ''' 契約情報更新(確定時)
    ''' </summary>
    ''' <param name="estimateid">見積管理ID</param>
    ''' <param name="paymentKbn">支払方法区分</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Function UpdateContractInfoByDecide(ByVal estimateid As Long, _
                                               ByVal paymentKbn As String) As Boolean Implements ISC3070204BusinessLogic.UpdateContractInfoByDecide

        '2013/06/30 TCS 山田 2013/10対応版 既存流用 START
        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  " {0}_Start",
                                  MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================
        '2013/06/30 TCS 山田 2013/10対応版 既存流用 END

        '見積情報取得 
        Dim apiBiz As New IC3070201BusinessLogic
        Dim apiDt As IC3070201DataSet

        'メッセージIDの初期設定
        Me._msgId = MESSAGE_ID_SUCCESS

        '2013/06/30 TCS 山田 2013/10対応版 既存流用 START
        Dim da As New SC3070204TableAdapter

        '見積情報更新ロック取得
        Try
            da.GetEstimateinfoLock(estimateid)
        Catch ex As OracleExceptionEx

            Me._msgId = MESSAGE_ID_908

            '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 START
            ' ======================== ログ出力 開始 ========================
            Logger.Error(String.Format(CultureInfo.InvariantCulture,
                                      " {0}_End, Return:[{1}]",
                                      MethodBase.GetCurrentMethod.Name, "False"))
            '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 END
            ' ======================== ログ出力 終了 ========================

            Return False
        End Try

        apiDt = apiBiz.GetEstimationInfo(estimateid, ESTIMATION_MODE_ALL, CHANGE_MODE_NOT_TCV)
        '2013/06/30 TCS 山田 2013/10対応版 既存流用 END


        Dim staff As StaffContext = StaffContext.Current

        '環境設定の取得
        Dim dealerEnvBiz As New DealerEnvSetting
        Dim dealerEnvDt As DlrEnvSettingDataSet.DLRENVSETTINGRow = dealerEnvBiz.GetEnvSetting("XXXXX", DLRENVSETTING_TACT)

        'TACT連携
        Dim clsWebClient As New WebClient
        Dim res As Dictionary(Of String, String) = clsWebClient.RequestHttp(paymentKbn, _
                                                                            apiDt, _
                                                                            dealerEnvDt, _
                                                                            staff)
        clsWebClient = Nothing

        Dim constractNo As String = String.Empty

        If res.ContainsKey(DIC_KEY_ID) Then
            If Not "0".Equals(res.Item(DIC_KEY_ID)) Then
                Me._msgId = MESSAGE_ID_909
                Logger.Error(ERROR_MSG & res.Item(DIC_KEY_ID))
                '2013/06/30 TCS 山田 2013/10対応版 既存流用 START
                '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 START
                ' ======================== ログ出力 開始 ========================
                Logger.Error(String.Format(CultureInfo.InvariantCulture,
                                          " {0}_End, Return:[{1}]",
                                          MethodBase.GetCurrentMethod.Name, "False"))
                ' ======================== ログ出力 終了 ========================
                '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 END
                '2013/06/30 TCS 山田 2013/10対応版 既存流用 END
                Return False
            Else
                If res.ContainsKey(DIC_KEY_NO) Then
                    constractNo = res.Item(DIC_KEY_NO)
                End If
            End If
        Else
            Me._msgId = MESSAGE_ID_909
            '2013/06/30 TCS 山田 2013/10対応版 既存流用 START
            '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 START
            ' ======================== ログ出力 開始 ========================
            Logger.Error(String.Format(CultureInfo.InvariantCulture,
                                       " {0}_End, Return:[{1}]",
                                      MethodBase.GetCurrentMethod.Name, "False"))
            ' ======================== ログ出力 終了 ========================
            '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 END
            '2013/06/30 TCS 山田 2013/10対応版 既存流用 END
            Return False
        End If

        '見積作成画面上で選択されていない支払方法を設定
        Dim payment As String = String.Empty
        If PAYMENTMETHOD_MONEY.Equals(paymentKbn) Then
            payment = PAYMENTMETHOD_LOAN
        Else
            payment = PAYMENTMETHOD_MONEY
        End If

        ' 2013/06/30 TCS 山田 2013/10対応版　既存流用 START DEL
        ' 2013/06/30 TCS 山田 2013/10対応版　既存流用 END
        Dim dateNow As Date = Date.Now()
        Dim ret As Integer = 1

        Try
            '更新処理 
            '支払方法の削除フラグ更新
            ret = da.UpdateDelFlg(estimateid, _
                                  payment, _
                                  staff.Account, _
                                  PROGRAM_ID)

            '更新に失敗していたらロールバック
            If ret = 0 Then
                'エラー
                Me.Rollback = True

                Me._msgId = MESSAGE_ID_908
                '2013/06/30 TCS 山田 2013/10対応版 既存流用 START
                '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 START
                ' ======================== ログ出力 開始 ========================
                Logger.Error(String.Format(CultureInfo.InvariantCulture,
                                          " {0}_End, Return:[{1}]",
                                          MethodBase.GetCurrentMethod.Name, "False"))
                ' ======================== ログ出力 終了 ========================
                '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 END
                '2013/06/30 TCS 山田 2013/10対応版 既存流用 END
                Return False
            End If

            '契約情報の更新
            ret = da.UpdateContractInfo(estimateid, _
                                         CONTRACTFLG_CONTRACT, _
                                         dateNow, _
                                         constractNo, _
                                         staff.Account, _
                                         PROGRAM_ID)

            '更新に失敗していたらロールバック
            If ret = 0 Then
                'エラー
                Me.Rollback = True

                Me._msgId = MESSAGE_ID_908

                '2013/06/30 TCS 山田 2013/10対応版 既存流用 START
                '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 START
                ' ======================== ログ出力 開始 ========================
                Logger.Error(String.Format(CultureInfo.InvariantCulture,
                                          " {0}_End, Return:[{1}]",
                                          MethodBase.GetCurrentMethod.Name, "False"))
                ' ======================== ログ出力 終了 ========================
                '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 END
                '2013/06/30 TCS 山田 2013/10対応版 既存流用 END
                Return False
            End If

            '2013/01/10 TCS 橋本 【A.STEP2】Add Start
            Dim retInsSS As Long
            '2013/06/30 TCS 山田 2013/10対応版 既存流用 START
            Dim apiDtEstimateRow As IC3070201EstimationInfoRow = Nothing
            Dim salesid As Decimal = 0

            '商談ID取得
            apiDtEstimateRow = CType(apiDt.Tables(IFTBL_ESTINFO).Rows(0), IC3070201EstimationInfoRow)
            If Not IsDBNull(apiDtEstimateRow.Item("FLLWUPBOX_SEQNO")) Then
                salesid = CType(apiDtEstimateRow.Item("FLLWUPBOX_SEQNO"), Decimal)
            End If
            apiDtEstimateRow = Nothing
            '未存在希望車種の登録
            retInsSS = ActivityInfoBusinessLogic.InsertNotRegSelectedSeries(estimateid,
                                                                            staff.Account,
                                                                            salesid
                                                                            )
            '2013/06/30 TCS 山田 2013/10対応版 既存流用 END

            '登録に失敗していたらロールバック
            If retInsSS < 0 Then
                'エラー
                Me.Rollback = True

                Me._msgId = MESSAGE_ID_908

                '2013/06/30 TCS 山田 2013/10対応版 既存流用 START
                '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 START
                ' ======================== ログ出力 開始 ========================
                Logger.Error(String.Format(CultureInfo.InvariantCulture,
                                          " {0}_End, Return:[{1}]",
                                          MethodBase.GetCurrentMethod.Name, "False"))
                ' ======================== ログ出力 終了 ========================
                '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 END
                '2013/06/30 TCS 山田 2013/10対応版 既存流用 END
                Return False
            End If
            '2013/01/10 TCS 橋本 【A.STEP2】Add End

            ' 2013/06/30 TCS 山田 2013/10対応版　既存流用 START DEL
            ' 2013/06/30 TCS 山田 2013/10対応版　既存流用 END

            '通知キャンセル
            If Not UpdateNoticeRequest(salesid) Then
                '2013/06/30 TCS 山田 2013/10対応版 既存流用 END
                'エラー
                Me.Rollback = True

                Me._msgId = MESSAGE_ID_908

                '2013/06/30 TCS 山田 2013/10対応版 既存流用 START
                '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 START
                ' ======================== ログ出力 開始 ========================
                Logger.Error(String.Format(CultureInfo.InvariantCulture,
                                          " {0}_End, Return:[{1}]",
                                          MethodBase.GetCurrentMethod.Name, "False"))
                ' ======================== ログ出力 終了 ========================
                '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 END
                '2013/06/30 TCS 山田 2013/10対応版 既存流用 END
                Return False
            End If
        Catch ex As OracleExceptionEx
            'エラー
            Me.Rollback = True

            Me._msgId = MESSAGE_ID_908

            '2013/06/30 TCS 山田 2013/10対応版 既存流用 START
            '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 START
            ' ======================== ログ出力 開始 ========================
            Logger.Error(String.Format(CultureInfo.InvariantCulture,
                                      " {0}_End, Return:[{1}]",
                                      MethodBase.GetCurrentMethod.Name, "False"))
            ' ======================== ログ出力 終了 ========================
            '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 END
            '2013/06/30 TCS 山田 2013/10対応版 既存流用 END
            Return False
        End Try

        '2013/06/30 TCS 山田 2013/10対応版 既存流用 START
        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  " {0}_End, Return:[{1}]",
                                  MethodBase.GetCurrentMethod.Name, "True"))
        ' ======================== ログ出力 終了 ========================
        '2013/06/30 TCS 山田 2013/10対応版 既存流用 END
        Return True

    End Function

    ''' <summary>
    ''' 契約情報更新(キャンセル時)
    ''' </summary>
    ''' <param name="estimateid">見積管理ID</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Function UpdateContractInfoByCancel(ByVal estimateid As Long) As Boolean Implements ISC3070204BusinessLogic.UpdateContractInfoByCancel

        '2013/06/30 TCS 山田 2013/10対応版 既存流用 START
        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  " {0}_Start",
                                  MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================
        '2013/06/30 TCS 山田 2013/10対応版 既存流用 END

        Dim staff As StaffContext = StaffContext.Current
        Dim ret As Integer = 1
        Dim da As New SC3070204TableAdapter

        'メッセージIDの初期設定
        Me._msgId = MESSAGE_ID_SUCCESS

        '2013/06/30 TCS 山田 2013/10対応版 既存流用 START
        '見積情報更新ロック取得
        Try
            da.GetEstimateinfoLock(estimateid)
        Catch ex As OracleExceptionEx

            Me._msgId = MESSAGE_ID_908

            '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 START
            ' ======================== ログ出力 開始 ========================
            Logger.Error(String.Format(CultureInfo.InvariantCulture,
                                      " {0}_End, Return:[{1}]",
                                      MethodBase.GetCurrentMethod.Name, "False"))
            '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 END
            ' ======================== ログ出力 終了 ========================

            Return False
        End Try
        '2013/06/30 TCS 山田 2013/10対応版 既存流用 END

        '更新に失敗していたらメッセージを表示
        Try
            '契約情報のキャンセル
            ret = da.UpdateContractInfo(estimateid, _
                                        CONTRACTFLG_CANCEL, _
                                        Date.MinValue, _
                                        Nothing, _
                                        staff.Account, _
                                        PROGRAM_ID)

            '更新に失敗していたらロールバック
            If ret = 0 Then
                'エラー
                Me.Rollback = True

                Me._msgId = MESSAGE_ID_908

                '2013/06/30 TCS 山田 2013/10対応版 既存流用 START
                '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 START
                ' ======================== ログ出力 開始 ========================
                Logger.Error(String.Format(CultureInfo.InvariantCulture,
                                          " {0}_End, Return:[{1}]",
                                          MethodBase.GetCurrentMethod.Name, "False"))
                ' ======================== ログ出力 終了 ========================
                '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 END
                '2013/06/30 TCS 山田 2013/10対応版 既存流用 END
                Return False
            End If
        Catch ex As OracleExceptionEx
            'エラー
            Me.Rollback = True

            Me._msgId = MESSAGE_ID_908

            '2013/06/30 TCS 山田 2013/10対応版 既存流用 START
            '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 START
            ' ======================== ログ出力 開始 ========================
            Logger.Error(String.Format(CultureInfo.InvariantCulture,
                                      " {0}_End, Return:[{1}]",
                                      MethodBase.GetCurrentMethod.Name, "False"))
            ' ======================== ログ出力 終了 ========================
            '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 END
            '2013/06/30 TCS 山田 2013/10対応版 既存流用 END
            Return False
        End Try

        '2013/06/30 TCS 山田 2013/10対応版 既存流用 START
        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  " {0}_End, Return:[{1}]",
                                  MethodBase.GetCurrentMethod.Name, "True"))
        ' ======================== ログ出力 終了 ========================
        '2013/06/30 TCS 山田 2013/10対応版 既存流用 END
        Return True

    End Function


#End Region

#Region "Privateメソッド"

    ''' <summary>
    ''' 保険情報取得
    ''' </summary>
    ''' <param name="estDtIC3070201">見積情報I/Fデータセット</param>
    ''' <param name="estDtSC3070204">SC3070204データセット</param>
    ''' <returns>保険情報データテーブル情報</returns>
    ''' <remarks></remarks>
    Private Function GetInsuranceCompanyInfo(ByVal estDtIC3070201 As IC3070201DataSet, _
                                             ByVal estDtSC3070204 As SC3070204DataSet) As SC3070204DataSet.SC3070204InsKindMastDataTable

        '販売店コード
        Dim dlrCd As String = CStr(estDtIC3070201.Tables(IFTBL_ESTINFO).Rows(0).Item(CLM_DLRCD))
        '保険会社コード
        Dim insuranceComCd As String = String.Empty
        '保険種別
        Dim insuranceKind As String = String.Empty

        If IsDBNull(estDtIC3070201.Tables(IFTBL_INSINFO).Rows(0).Item(CLM_INSUCOMCD)) Then
            '保険会社コードがNULLの場合は、SQLを発行しない
            Return estDtSC3070204.SC3070204InsKindMast

        ElseIf IsDBNull(estDtIC3070201.Tables(IFTBL_INSINFO).Rows(0).Item(CLM_INSUKIND)) Then
            '保険種別のみがNULLの場合は、SQLを発行する
            insuranceComCd = CStr(estDtIC3070201.Tables(IFTBL_INSINFO).Rows(0).Item(CLM_INSUCOMCD))
            insuranceKind = String.Empty
        Else
            '両方ある場合は、SQLを発行する
            insuranceComCd = CStr(estDtIC3070201.Tables(IFTBL_INSINFO).Rows(0).Item(CLM_INSUCOMCD))
            insuranceKind = CStr(estDtIC3070201.Tables(IFTBL_INSINFO).Rows(0).Item(CLM_INSUKIND))
        End If

        Dim da As New SC3070204TableAdapter
        '検索処理
        Return da.GetInsuranceCompanyInfo(dlrCd, insuranceComCd, insuranceKind)

    End Function

    ''' <summary>
    ''' 融資情報取得
    ''' </summary>
    ''' <param name="estDtIC3070201">見積情報I/Fデータセット</param>
    ''' <param name="estDtSC3070204">SC3070204データセット</param>
    ''' <param name="payMethod">支払方法区分</param>
    ''' <returns>融資情報データテーブル情報</returns>
    ''' <remarks></remarks>
    Private Function GetFinanceCompanyInfo(ByVal estDtIC3070201 As IC3070201DataSet, _
                                           ByVal estDtSC3070204 As SC3070204DataSet, _
                                           ByVal payMethod As String) As SC3070204DataSet.SC3070204FinanceComMastDataTable

        '販売店コード
        Dim dlrCd As String = CStr(estDtIC3070201.Tables(IFTBL_ESTINFO).Rows(0).Item(CLM_DLRCD))
        '融資会社コード
        Dim financeCd As String = String.Empty

        '支払い方法データテーブルの行ループ
        For Each paymentRow As IC3070201PaymentInfoRow In estDtIC3070201.Tables(IFTBL_PAYINFO).Rows()
            '現金のレコードを使用するか、ローンのレコードを使用するかをセッションの支払い方法で判別
            If payMethod.Equals(paymentRow.Item(CLM_PAYMENTMETHOD)) Then
                If IsDBNull(paymentRow.Item(CLM_FINANCECOMCODE)) Then
                    '融資会社コードがNULLの場合、SQLを発行しない
                    Return estDtSC3070204.SC3070204FinanceComMast
                Else
                    '融資会社コードがある場合、SQLを発行
                    financeCd = CStr(paymentRow.Item(CLM_FINANCECOMCODE))
                End If
            End If
        Next

        Dim da As New SC3070204TableAdapter
        '検索処理
        Return da.GetFinanceCompanyInfo(dlrCd, financeCd)

    End Function

    ''' <summary>
    ''' 敬称情報取得
    ''' </summary>
    ''' <returns>システム環境設定データテーブル情報</returns>
    ''' <remarks></remarks>
    Private Function GetNameTitleInfo() As SC3070204DataSet.SC3070204SystemEnvSettingDataTable

        Dim sysenvDataRow As SC3070204DataSet.SC3070204SystemEnvSettingRow

        Using sysenvDataTbl As New SC3070204DataSet.SC3070204SystemEnvSettingDataTable
            sysenvDataRow = sysenvDataTbl.NewSC3070204SystemEnvSettingRow

            Dim sys As New SystemEnvSetting
            Dim sysPosition As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow = sys.GetSystemEnvSetting(PARAM_NAME_TITLE_POS)
            Dim sysDefoltTitle As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow = sys.GetSystemEnvSetting(PARAM_NAME_TITLE)

            '敬称の位置を取得
            If (sysPosition Is Nothing) Then
                sysenvDataRow.NAMETITLEPOSITION = "1"
            Else
                sysenvDataRow.NAMETITLEPOSITION = sysPosition.PARAMVALUE
            End If

            '敬称のデフォルト値を取得
            If (sysDefoltTitle Is Nothing) Then
                sysenvDataRow.DEFOLTNAMETITLE = ""
            Else
                sysenvDataRow.DEFOLTNAMETITLE = sysDefoltTitle.PARAMVALUE
            End If

            sysenvDataTbl.AddSC3070204SystemEnvSettingRow(sysenvDataRow)

            Return sysenvDataTbl
        End Using
    End Function

    ''' <summary>
    ''' 印刷情報設定
    ''' </summary>
    ''' <param name="apiDt">見積情報I/Fデータセット</param>
    ''' <param name="branchDt">店舗情報行情報</param>
    ''' <param name="estDtSC3070204">SC3070204データセット</param>
    ''' <param name="paymentKbn">支払方法区分</param>
    ''' <returns>SC3070204データセット(アウトプット)</returns>
    ''' <remarks></remarks>
    Private Function SetDataPrintInfo(ByVal apiDt As IC3070201DataSet,
                                      ByVal branchDt As BranchDataSet.BRANCHRow,
                                      ByVal estDtSC3070204 As SC3070204DataSet,
                                      ByVal paymentKbn As String) As SC3070204DataSet

        Using retDtSC3070204 As New SC3070204DataSet
            '初期化
            retDtSC3070204.Tables.Clear()

            '見積情報取得I/Fで取得したデータ
            Dim apiDtEstimateRow As IC3070201EstimationInfoRow = Nothing
            apiDtEstimateRow = CType(apiDt.Tables(IFTBL_ESTINFO).Rows(0), IC3070201EstimationInfoRow)

            '▼印刷情報(基本)▼
            Using dtPrintDataBasic As New SC3070204DataSet.SC3070204PrintInfoBasicDataTable
                Dim printDataBasicRow As SC3070204DataSet.SC3070204PrintInfoBasicRow

                '印刷基本情報を初期化
                printDataBasicRow = Me.GetInitPrintDataBasic(dtPrintDataBasic)

                '顧客情報
                For Each apiDtCustomerRow As IC3070201CustomerInfoRow In apiDt.Tables(IFTBL_CUSTINFO).Rows()
                    '所有者のみ設定
                    If CONTRACTCUSTTYPE_DEALER.Equals(apiDtCustomerRow.Item("CONTRACTCUSTTYPE")) Then
                        '契約顧客区分
                        printDataBasicRow.CONTRACT_CUST_TYPE = Me.GetCustomerDtCol(apiDtCustomerRow, "CONTRACTCUSTTYPE")
                        '顧客区分
                        printDataBasicRow.CUSTOMER_PART = Me.GetCustomerDtCol(apiDtCustomerRow, "CUSTPART")
                        'お客様氏名
                        printDataBasicRow.CUSTOMER_NM = Me.GetCustomerDtCol(apiDtCustomerRow, "NAME")
                        'お客様携帯番号
                        printDataBasicRow.CUSTOMER_MOBILE = Me.GetCustomerDtCol(apiDtCustomerRow, "MOBILE")
                        'お客様住所
                        printDataBasicRow.CUSTOMER_ADDRESS = Me.GetCustomerDtCol(apiDtCustomerRow, "ADDRESS")
                        'お客様郵便番号
                        printDataBasicRow.CUSTOMER_ZIPCD = Me.GetCustomerDtCol(apiDtCustomerRow, "ZIPCODE")
                        'お客様電話番号
                        printDataBasicRow.CUSTOMER_TELNO = Me.GetCustomerDtCol(apiDtCustomerRow, "TELNO")
                        'お客様FAX
                        printDataBasicRow.CUSTOMER_FAX = Me.GetCustomerDtCol(apiDtCustomerRow, "FAXNO")
                        '国民番号
                        printDataBasicRow.CUSTOMER_SOCIALID = Me.GetCustomerDtCol(apiDtCustomerRow, "SOCIALID")
                        'お客様EMail
                        printDataBasicRow.CUSTOMER_MAIL = Me.GetCustomerDtCol(apiDtCustomerRow, "EMAIL")

                        '2013/10/25 TCS 葛西 次世代e-CRBセールス機能 新DB適応に向けた機能開発 ADD START
                        '使用者の設定
                    Else
                        '顧客区分（使用者）
                        printDataBasicRow.USER_PART = Me.GetCustomerDtCol(apiDtCustomerRow, "CUSTPART")
                        'お客様氏名（使用者）
                        printDataBasicRow.USER_NM = Me.GetCustomerDtCol(apiDtCustomerRow, "NAME")
                        'お客様携帯番号（使用者）
                        printDataBasicRow.USER_MOBILE = Me.GetCustomerDtCol(apiDtCustomerRow, "MOBILE")
                        'お客様住所（使用者）
                        printDataBasicRow.USER_ADDRESS = Me.GetCustomerDtCol(apiDtCustomerRow, "ADDRESS")
                        'お客様郵便番号（使用者）
                        printDataBasicRow.USER_ZIPCD = Me.GetCustomerDtCol(apiDtCustomerRow, "ZIPCODE")
                        'お客様電話番号（使用者）
                        printDataBasicRow.USER_TELNO = Me.GetCustomerDtCol(apiDtCustomerRow, "TELNO")
                        'お客様FAX（使用者）
                        printDataBasicRow.USER_FAX = Me.GetCustomerDtCol(apiDtCustomerRow, "FAXNO")
                        '国民番号（使用者）
                        printDataBasicRow.USER_SOCIALID = Me.GetCustomerDtCol(apiDtCustomerRow, "SOCIALID")
                        'お客様EMail（使用者）
                        printDataBasicRow.USER_MAIL = Me.GetCustomerDtCol(apiDtCustomerRow, "EMAIL")
                        '2013/10/25 TCS 葛西 次世代e-CRBセールス機能 新DB適応に向けた機能開発 ADD END

                    End If
                Next

                '本日日付
                printDataBasicRow.TODAY = DateTimeFunc.FormatDate(FORMAT_YYYYMMDD, DateTimeFunc.Now) & " 00:00:00"
                '販売店名称
                printDataBasicRow.SELLES_NM = branchDt.STRNM_LOCAL
                '販売店住所
                printDataBasicRow.SELLES_ADDRESS = branchDt.ADDR1_LOCAL
                '販売店電話番号
                printDataBasicRow.SELLES_TELNO = branchDt.SALTEL
                '販売店FAX
                printDataBasicRow.SELLES_FAX = branchDt.SALFAXNO
                'サービス電話番号
                printDataBasicRow.SERVICE_TELNO = branchDt.SRVSTEL
                'スタッフ名称
                Dim staff As StaffContext = StaffContext.Current
                printDataBasicRow.STAFF_NM = staff.UserName
                staff = Nothing

                '契約書No
                printDataBasicRow.CONTRACT_NO = Me.GetApiDtCol(apiDtEstimateRow, "CONTRACTNO")
                'シリーズ名称
                printDataBasicRow.SERIES_NM = Me.GetApiDtCol(apiDtEstimateRow, "SERIESNM")
                'モデル名称
                printDataBasicRow.MODEL_NM = Me.GetApiDtCol(apiDtEstimateRow, "MODELNM")
                'ボディータイプ
                printDataBasicRow.BODY_TYPE = Me.GetApiDtCol(apiDtEstimateRow, "BODYTYPE")
                '排気量
                printDataBasicRow.DISPLACEMENT = Me.GetApiDtCol(apiDtEstimateRow, "DISPLACEMENT")
                '駆動方式
                printDataBasicRow.DRIVESYSTEM = Me.GetApiDtCol(apiDtEstimateRow, "DRIVESYSTEM")
                'トランスミッション
                printDataBasicRow.TRANSMISSION = Me.GetApiDtCol(apiDtEstimateRow, "TRANSMISSION")
                '外装色名称
                printDataBasicRow.EXTCOLORNM = Me.GetApiDtCol(apiDtEstimateRow, "EXTCOLOR")
                '内装色名称
                printDataBasicRow.INTCOLORNM = Me.GetApiDtCol(apiDtEstimateRow, "INTCOLOR")
                '車両番号
                printDataBasicRow.MODELNUMBER = Me.GetApiDtCol(apiDtEstimateRow, "MODELNUMBER")
                'サフィックス
                printDataBasicRow.SUFFIXCD = Me.GetApiDtCol(apiDtEstimateRow, "SUFFIXCD")
                '本体車両価格
                printDataBasicRow.BASEPRICE = Me.GetApiDtCol(apiDtEstimateRow, "BASEPRICE")
                '値引き
                printDataBasicRow.DISCOUNTPRICE = Me.GetApiDtCol(apiDtEstimateRow, "DISCOUNTPRICE")
                '外装追加費用
                printDataBasicRow.EXTAMOUNT = Me.GetApiDtCol(apiDtEstimateRow, "EXTAMOUNT")
                '内装追加費用
                printDataBasicRow.INTAMOUNT = Me.GetApiDtCol(apiDtEstimateRow, "INTAMOUNT")

                '諸費用情報
                For Each apiDtChangeRow As IC3070201ChargeInfoRow In apiDt.Tables(IFTBL_CHARGEINFO).Rows()
                    If ITEM_CODE_PURCHASE.Equals(apiDtChangeRow.ITEMCODE) Then
                        '車両購入税
                        printDataBasicRow.BASEPURCHASETAX = Me.GetChangeDtCol(apiDtChangeRow, "PRICE")
                    ElseIf ITEM_CODE_REGISTRAION.Equals(apiDtChangeRow.ITEMCODE) Then
                        '登録費用
                        printDataBasicRow.REGISTAMOUNT = Me.GetChangeDtCol(apiDtChangeRow, "PRICE")
                    End If
                Next

                '保険情報が存在した場合のみ処理
                If 0 < apiDt.Tables(IFTBL_INSINFO).Rows.Count Then
                    Dim apiDtInsuranceRow As IC3070201EstInsuranceInfoRow = Nothing
                    apiDtInsuranceRow = CType(apiDt.Tables(IFTBL_INSINFO).Rows(0), IC3070201EstInsuranceInfoRow)

                    '保険費用
                    printDataBasicRow.INSURANCE_AMOUNT = Me.GetInsuranceDtCol(apiDtInsuranceRow, "AMOUNT")

                    apiDtInsuranceRow = Nothing
                End If

                '保険マスタ情報が存在した場合のみ処理
                If 0 < estDtSC3070204.Tables(TBL_INSINFO).Rows.Count Then
                    Dim insuranceRow As SC3070204DataSet.SC3070204InsKindMastRow = Nothing
                    insuranceRow = CType(estDtSC3070204.Tables(TBL_INSINFO).Rows(0), SC3070204DataSet.SC3070204InsKindMastRow)

                    '保険会社名称
                    printDataBasicRow.INSURANCE_COMPANY = Me.GetInsKindMastDtCol(insuranceRow, "INSUCOMNM")
                    '保険種別名称
                    printDataBasicRow.INSURANCE_KINDNM = Me.GetInsKindMastDtCol(insuranceRow, "INSUKINDNM")

                    insuranceRow = Nothing
                End If

                '融資マスタ情報が存在した場合のみ処理
                If 0 < estDtSC3070204.Tables(TBL_FINANCEINFO).Rows.Count Then
                    Dim financeRow As SC3070204DataSet.SC3070204FinanceComMastRow = Nothing
                    financeRow = CType(estDtSC3070204.Tables(TBL_FINANCEINFO).Rows(0), SC3070204DataSet.SC3070204FinanceComMastRow)

                    '融資会社名称
                    printDataBasicRow.FINANCE_COMPANYNM = Me.GetFinanceComMastDtCol(financeRow, "FINANCECOMNAME")

                    financeRow = Nothing
                End If

                '保険情報が存在した場合のみ処理
                If 0 < estDtSC3070204.Tables(TBL_SYSTEMINFO).Rows.Count Then
                    Dim nameTitleRow As SC3070204DataSet.SC3070204SystemEnvSettingRow = Nothing
                    nameTitleRow = CType(estDtSC3070204.Tables(TBL_SYSTEMINFO).Rows(0), SC3070204DataSet.SC3070204SystemEnvSettingRow)

                    '敬称
                    printDataBasicRow.CUSTOMER_NM_TITLE = Me.GetSystemEnvDtCol(nameTitleRow, "DEFOLTNAMETITLE")
                    '敬称位置
                    printDataBasicRow.CUSTOMER_NM_TITLE_POS = Me.GetSystemEnvDtCol(nameTitleRow, "NAMETITLEPOSITION")

                    nameTitleRow = Nothing
                End If

                For Each apiDtPaymentRow As IC3070201PaymentInfoRow In apiDt.Tables(IFTBL_PAYINFO).Rows()
                    '見積作成画面で選択された支払方法区分に紐づくデータを設定
                    If paymentKbn.Equals(apiDtPaymentRow.PAYMENTMETHOD) Then
                        '支払情報
                        printDataBasicRow.PAYMENT_METHOD = Me.GetPaymentDtCol(apiDtPaymentRow, "PAYMENTMETHOD")
                        '支払期間
                        printDataBasicRow.PAYMENT_PERIOD = Me.GetPaymentDtCol(apiDtPaymentRow, "PAYMENTPERIOD")
                        '毎月返済額
                        printDataBasicRow.MONTHLYPAYMENT = Me.GetPaymentDtCol(apiDtPaymentRow, "MONTHLYPAYMENT")
                        '頭金
                        printDataBasicRow.DEPOSIT = Me.GetPaymentDtCol(apiDtPaymentRow, "DEPOSIT")
                        'ボーナス時返済額
                        printDataBasicRow.BONUSPAYMENT = Me.GetPaymentDtCol(apiDtPaymentRow, "BONUSPAYMENT")
                        '初回支払期限
                        printDataBasicRow.DUEDATE = Me.GetPaymentDtCol(apiDtPaymentRow, "DUEDATE")
                        '2013/03/08 TCS 山田 【A.STEP2】新車タブレット受付画面の管理指標変更対応 START
                        '利率
                        printDataBasicRow.INTERESTRATE = Me.GetPaymentDtCol(apiDtPaymentRow, "INTERESTRATE")
                        '2013/03/08 TCS 山田 【A.STEP2】新車タブレット受付画面の管理指標変更対応 END
                    End If
                Next

                '納車予定日
                printDataBasicRow.DELIDATE = Me.GetApiDtCol(apiDtEstimateRow, "DELIDATE")
                'メモ
                printDataBasicRow.MEMO = Me.GetApiDtCol(apiDtEstimateRow, "MEMO")

                '契約書印刷フラグ
                printDataBasicRow.CONTRACT_PRINTFLG = Me.GetApiDtCol(apiDtEstimateRow, "CONTPRINTFLG")
                '契約書状況フラグ
                printDataBasicRow.CONTRACT_STATUS_FLG = Me.GetApiDtCol(apiDtEstimateRow, "CONTRACTFLG")
                'FollowupBox
                printDataBasicRow.FLLWUPBOX_SEQNO = Me.GetApiDtCol(apiDtEstimateRow, "FLLWUPBOX_SEQNO")

                'テーブル変数に追加
                dtPrintDataBasic.Rows.Add(printDataBasicRow)
                printDataBasicRow = Nothing

                'データセットに追加
                retDtSC3070204.Tables.Add(dtPrintDataBasic)
            End Using

            '▼印刷情報(オプション)▼
            Using dtPrintDataOption As New SC3070204DataSet.SC3070204PrintInfoOptionDataTable
                Dim printDataOptionRow As SC3070204DataSet.SC3070204PrintInfoOptionRow
                For Each apiDtVclOptionRow As IC3070201VclOptionInfoRow In apiDt.Tables(IFTBL_VCLOPTIONINFO).Rows()
                    printDataOptionRow = CType(dtPrintDataOption.NewRow, SC3070204DataSet.SC3070204PrintInfoOptionRow)

                    'オプション名称
                    printDataOptionRow.NAME = Me.GetOptionDtCol(apiDtVclOptionRow, "OPTIONNAME")
                    'オプション価格
                    printDataOptionRow.PRICE = Me.GetOptionDtCol(apiDtVclOptionRow, "PRICE")
                    'オプション取付費用
                    printDataOptionRow.INSTALLCOST = Me.GetOptionDtCol(apiDtVclOptionRow, "INSTALLCOST")

                    'テーブル変数に追加
                    dtPrintDataOption.Rows.Add(printDataOptionRow)
                Next
                printDataOptionRow = Nothing

                'データセットに追加
                retDtSC3070204.Tables.Add(dtPrintDataOption)
            End Using

            '▼印刷情報(下取車両情報)▼
            Using dtPrintDataTradeinCar As New SC3070204DataSet.SC3070204PrintInfoTradeInCarDataTable
                Dim printDataTradeinCarRow As SC3070204DataSet.SC3070204PrintInfoTradeInCarRow
                For Each apiDtTradeincarRow As IC3070201TradeincarInfoRow In apiDt.Tables(IFTBL_TRADEINCARINFO).Rows()
                    printDataTradeinCarRow = CType(dtPrintDataTradeinCar.NewRow, SC3070204DataSet.SC3070204PrintInfoTradeInCarRow)

                    '車名
                    printDataTradeinCarRow.NAME = Me.GetTradeincarDtCol(apiDtTradeincarRow, "VEHICLENAME")
                    '下取価格
                    printDataTradeinCarRow.PRICE = Me.GetTradeincarDtCol(apiDtTradeincarRow, "ASSESSEDPRICE")

                    'テーブル変数に追加
                    dtPrintDataTradeinCar.Rows.Add(printDataTradeinCarRow)
                Next
                printDataTradeinCarRow = Nothing

                'データセットに追加
                retDtSC3070204.Tables.Add(dtPrintDataTradeinCar)
            End Using

            '2013/03/08 TCS 山田 【A.STEP2】新車タブレット受付画面の管理指標変更対応 START
            '▼印刷情報(諸費用情報)▼
            Using dtPrintDataCharge As New SC3070204DataSet.SC3070204PrintInfoChargeDataTable
                Dim printDataChargeRow As SC3070204DataSet.SC3070204PrintInfoChargeRow
                For Each apiDtChargeRow As IC3070201ChargeInfoRow In apiDt.Tables(IFTBL_CHARGEINFO).Rows()
                    printDataChargeRow = CType(dtPrintDataCharge.NewRow, SC3070204DataSet.SC3070204PrintInfoChargeRow)

                    If Not ITEM_CODE_PURCHASE.Equals(apiDtChargeRow.ITEMCODE) AndAlso _
                        Not ITEM_CODE_REGISTRAION.Equals(apiDtChargeRow.ITEMCODE) Then
                        '費用項目名
                        printDataChargeRow.NAME = Me.GetChangeDtCol(apiDtChargeRow, "ITEMNAME")
                        '価格
                        printDataChargeRow.PRICE = Me.GetChangeDtCol(apiDtChargeRow, "PRICE")
                        'テーブル変数に追加
                        dtPrintDataCharge.Rows.Add(printDataChargeRow)
                    End If

                Next
                printDataChargeRow = Nothing

                'データセットに追加
                retDtSC3070204.Tables.Add(dtPrintDataCharge)
            End Using
            '2013/03/08 TCS 山田 【A.STEP2】新車タブレット受付画面の管理指標変更対応 END

            Return retDtSC3070204
        End Using

    End Function

    ''' <summary>
    ''' 印刷基本情報を初期化して返却
    ''' </summary>
    ''' <param name="dtPrintDataBasic">印刷基本情報テーブル</param>
    ''' <returns>印刷基本情報行</returns>
    ''' <remarks></remarks>
    Private Function GetInitPrintDataBasic(ByVal dtPrintDataBasic As SC3070204DataSet.SC3070204PrintInfoBasicDataTable) As SC3070204DataSet.SC3070204PrintInfoBasicRow
        Dim printDataBasicRow = CType(dtPrintDataBasic.NewRow, SC3070204DataSet.SC3070204PrintInfoBasicRow)

        '契約顧客区分
        printDataBasicRow.CONTRACT_CUST_TYPE = String.Empty
        '顧客区分
        printDataBasicRow.CUSTOMER_PART = String.Empty
        'お客様氏名
        printDataBasicRow.CUSTOMER_NM = String.Empty
        'お客様携帯番号
        printDataBasicRow.CUSTOMER_MOBILE = String.Empty
        'お客様住所
        printDataBasicRow.CUSTOMER_ADDRESS = String.Empty
        'お客様郵便番号
        printDataBasicRow.CUSTOMER_ZIPCD = String.Empty
        'お客様電話番号
        printDataBasicRow.CUSTOMER_TELNO = String.Empty
        'お客様FAX
        printDataBasicRow.CUSTOMER_FAX = String.Empty
        '国民番号
        printDataBasicRow.CUSTOMER_SOCIALID = String.Empty
        'お客様EMail
        printDataBasicRow.CUSTOMER_MAIL = String.Empty
        '2013/10/25 TCS 葛西 次世代e-CRBセールス機能 新DB適応に向けた機能開発 ADD START
        '顧客区分
        printDataBasicRow.USER_PART = String.Empty
        'お客様氏名
        printDataBasicRow.USER_NM = String.Empty
        'お客様携帯番号
        printDataBasicRow.USER_MOBILE = String.Empty
        'お客様住所
        printDataBasicRow.USER_ADDRESS = String.Empty
        'お客様郵便番号
        printDataBasicRow.USER_ZIPCD = String.Empty
        'お客様電話番号
        printDataBasicRow.USER_TELNO = String.Empty
        'お客様FAX
        printDataBasicRow.USER_FAX = String.Empty
        '国民番号
        printDataBasicRow.USER_SOCIALID = String.Empty
        'お客様EMail
        printDataBasicRow.USER_MAIL = String.Empty
        '2013/10/25 TCS 葛西 次世代e-CRBセールス機能 新DB適応に向けた機能開発 ADD END
        '本日日付
        printDataBasicRow.TODAY = String.Empty
        '販売店名称
        printDataBasicRow.SELLES_NM = String.Empty
        '販売店住所
        printDataBasicRow.SELLES_ADDRESS = String.Empty
        '販売店電話番号
        printDataBasicRow.SELLES_TELNO = String.Empty
        '販売店FAX
        printDataBasicRow.SELLES_FAX = String.Empty
        'サービス電話番号
        printDataBasicRow.SERVICE_TELNO = String.Empty
        'スタッフ名称
        printDataBasicRow.STAFF_NM = String.Empty

        '契約書No
        printDataBasicRow.CONTRACT_NO = String.Empty
        'シリーズ名称
        printDataBasicRow.SERIES_NM = String.Empty
        'モデル名称
        printDataBasicRow.MODEL_NM = String.Empty
        'ボディータイプ
        printDataBasicRow.BODY_TYPE = String.Empty
        '排気量
        printDataBasicRow.DISPLACEMENT = String.Empty
        '駆動方式
        printDataBasicRow.DRIVESYSTEM = String.Empty
        'トランスミッション
        printDataBasicRow.TRANSMISSION = String.Empty
        '外装色名称
        printDataBasicRow.EXTCOLORNM = String.Empty
        '内装色名称
        printDataBasicRow.INTCOLORNM = String.Empty
        '車両番号
        printDataBasicRow.MODELNUMBER = String.Empty
        'サフィックス
        printDataBasicRow.SUFFIXCD = String.Empty
        '本体車両価格
        printDataBasicRow.BASEPRICE = String.Empty
        '値引き
        printDataBasicRow.DISCOUNTPRICE = String.Empty
        '外装追加費用
        printDataBasicRow.EXTAMOUNT = String.Empty
        '内装追加費用
        printDataBasicRow.INTAMOUNT = String.Empty
        '車両購入税
        printDataBasicRow.BASEPURCHASETAX = String.Empty
        '登録費用
        printDataBasicRow.REGISTAMOUNT = String.Empty
        '保険費用
        printDataBasicRow.INSURANCE_AMOUNT = String.Empty
        '保険会社名称
        printDataBasicRow.INSURANCE_COMPANY = String.Empty
        '保険種別名称
        printDataBasicRow.INSURANCE_KINDNM = String.Empty
        '融資会社名称
        printDataBasicRow.FINANCE_COMPANYNM = String.Empty
        '敬称
        printDataBasicRow.CUSTOMER_NM_TITLE = String.Empty
        '敬称位置
        printDataBasicRow.CUSTOMER_NM_TITLE_POS = String.Empty
        '支払情報
        printDataBasicRow.PAYMENT_METHOD = String.Empty
        '支払期間
        printDataBasicRow.PAYMENT_PERIOD = String.Empty
        '毎月返済額
        printDataBasicRow.MONTHLYPAYMENT = String.Empty
        '頭金
        printDataBasicRow.DEPOSIT = String.Empty
        'ボーナス時返済額
        printDataBasicRow.BONUSPAYMENT = String.Empty
        '初回支払期限
        printDataBasicRow.DUEDATE = String.Empty
        '納車予定日
        printDataBasicRow.DELIDATE = String.Empty
        'メモ
        printDataBasicRow.MEMO = String.Empty
        '2013/03/08 TCS 山田 【A.STEP2】新車タブレット受付画面の管理指標変更対応 START
        '利率
        printDataBasicRow.INTERESTRATE = String.Empty
        '2013/03/08 TCS 山田 【A.STEP2】新車タブレット受付画面の管理指標変更対応 END
        '契約書印刷フラグ
        printDataBasicRow.CONTRACT_PRINTFLG = String.Empty
        '契約書状況フラグ
        printDataBasicRow.CONTRACT_STATUS_FLG = String.Empty
        'FollowupBox
        printDataBasicRow.FLLWUPBOX_SEQNO = String.Empty

        Return printDataBasicRow
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
            Return String.Empty
        Else
            If CLM_DELIDATE.Equals(colName) Then
                Return DateTimeFunc.FormatDate(FORMAT_YYYYMMDD, apiDtRow.DELIDATE) & " 00:00:00"
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
        'DBNullの場合　""を返却
        If IsDBNull(apiCustomerDtRow.Item(colName)) Then
            Return String.Empty
        End If

        Return CType(apiCustomerDtRow.Item(colName), String)
    End Function

    ''' <summary>
    ''' 諸費用情報DtのDBNull判定処理を行います。
    ''' </summary>
    ''' <param name="apiChangeDtRow">諸費用情報のカラム</param>
    ''' <param name="colName">カラム名</param>
    ''' <returns>DBNullの場合は""を返却</returns>
    ''' <remarks></remarks>
    Private Function GetChangeDtCol(ByVal apiChangeDtRow As IC3070201ChargeInfoRow,
                                     ByVal colName As String) As String
        'DBNullの場合　""を返却
        If IsDBNull(apiChangeDtRow.Item(colName)) Then
            Return String.Empty
        End If

        Return CStr(apiChangeDtRow.Item(colName))
    End Function

    ''' <summary>
    ''' 支払情報DtのDBNull判定処理を行います。
    ''' </summary>
    ''' <param name="apiPaymentDtRow">支払情報のカラム</param>
    ''' <param name="colName">カラム名</param>
    ''' <returns>DBNullの場合は""を返却</returns>
    ''' <remarks></remarks>
    Private Function GetPaymentDtCol(ByVal apiPaymentDtRow As IC3070201PaymentInfoRow,
                                     ByVal colName As String) As String
        'DBNullの場合　""を返却
        If IsDBNull(apiPaymentDtRow.Item(colName)) Then
            Return String.Empty
        End If

        Return CStr(apiPaymentDtRow.Item(colName))
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
    ''' オプション情報DtのDBNull判定処理を行います。
    ''' </summary>
    ''' <param name="apiOptionDtRow">オプション情報のカラム</param>
    ''' <param name="colName">カラム名</param>
    ''' <returns>DBNullの場合は""を返却</returns>
    ''' <remarks></remarks>
    Private Function GetOptionDtCol(ByVal apiOptionDtRow As IC3070201VclOptionInfoRow,
                                     ByVal colName As String) As String
        'DBNullの場合　""を返却
        If IsDBNull(apiOptionDtRow.Item(colName)) Then
            Return String.Empty
        End If

        Return CStr(apiOptionDtRow.Item(colName))
    End Function

    ''' <summary>
    ''' 下取車両情報DtのDBNull判定処理を行います。
    ''' </summary>
    ''' <param name="apiTradeincarDtRow">下取車両情報のカラム</param>
    ''' <param name="colName">カラム名</param>
    ''' <returns>DBNullの場合は""を返却</returns>
    ''' <remarks></remarks>
    Private Function GetTradeincarDtCol(ByVal apiTradeincarDtRow As IC3070201TradeincarInfoRow,
                                     ByVal colName As String) As String
        'DBNullの場合　""を返却
        If IsDBNull(apiTradeincarDtRow.Item(colName)) Then
            Return String.Empty
        End If

        Return CStr(apiTradeincarDtRow.Item(colName))
    End Function

    ''' <summary>
    ''' システム環境設定DtカラムのDBNull判定処理を行います。
    ''' </summary>
    ''' <param name="systemEnvRow">システム環境設定のカラム</param>
    ''' <param name="colName">カラム名</param>
    ''' <returns>DBNullの場合は""を返却</returns>
    ''' <remarks></remarks>
    Private Function GetSystemEnvDtCol(ByVal systemEnvRow As SC3070204DataSet.SC3070204SystemEnvSettingRow,
                                     ByVal colName As String) As String
        'DBNullの場合　""を返却
        If IsDBNull(systemEnvRow.Item(colName)) Then
            Return String.Empty
        End If

        Return CType(systemEnvRow.Item(colName), String)
    End Function

    ''' <summary>
    ''' 保険会社情報DtカラムのDBNull判定処理を行います。
    ''' </summary>
    ''' <param name="insKindMastRow">保険会社情報のカラム</param>
    ''' <param name="colName">カラム名</param>
    ''' <returns>DBNullの場合は""を返却</returns>
    ''' <remarks></remarks>
    Private Function GetInsKindMastDtCol(ByVal insKindMastRow As SC3070204DataSet.SC3070204InsKindMastRow,
                                     ByVal colName As String) As String
        'DBNullの場合　""を返却
        If IsDBNull(insKindMastRow.Item(colName)) Then
            Return String.Empty
        End If

        Return CType(insKindMastRow.Item(colName), String)
    End Function

    ''' <summary>
    ''' 融資会社情報DtカラムのDBNull判定処理を行います。
    ''' </summary>
    ''' <param name="financeComMastRow">融資会社情報のカラム</param>
    ''' <param name="colName">カラム名</param>
    ''' <returns>DBNullの場合は""を返却</returns>
    ''' <remarks></remarks>
    Private Function GetFinanceComMastDtCol(ByVal financeComMastRow As SC3070204DataSet.SC3070204FinanceComMastRow,
                                     ByVal colName As String) As String
        'DBNullの場合　""を返却
        If IsDBNull(financeComMastRow.Item(colName)) Then
            Return String.Empty
        End If

        Return CType(financeComMastRow.Item(colName), String)
    End Function

    '2013/06/30 TCS 山田 2013/10対応版 既存流用 START
    ''' <summary>
    ''' 通知キャンセル処理
    ''' </summary>
    ''' <param name="fllwupBoxSeqno">Follow-up Box内連番</param>
    ''' <returns>処理結果</returns>
    ''' <remarks>通知キャンセル処理</remarks>
    Private Function UpdateNoticeRequest(ByVal fllwupBoxSeqno As Decimal) As Boolean
        '2013/06/30 TCS 山田 2013/10対応版 既存流用 END

        Logger.Info("UpdateNoticeRequest Start")

        'キャンセル対象件取得
        Dim da As New SC3070204TableAdapter

        Dim NoticeRequestDt As SC3070204DataSet.SC3070204NoticeRequestDataTable
        '2013/06/30 TCS 山田 2013/10対応版 既存流用 START
        NoticeRequestDt = da.GetNoticeRequest(fllwupBoxSeqno)
        '2013/06/30 TCS 山田 2013/10対応版 既存流用 END

        da = Nothing

        If NoticeRequestDt.Count > 0 Then
            'Follow-upに紐づく見積り分、通知する
            For Each noticeRequestRow As SC3070204DataSet.SC3070204NoticeRequestRow In NoticeRequestDt.Rows
                Dim rsltId As Integer
                Dim returnXmlNotice As XmlCommon

                Dim reqClassId As Nullable(Of Long) = Nothing
                If Not noticeRequestRow.IsREQCLASSIDNull Then
                    reqClassId = noticeRequestRow.REQCLASSID
                End If

                Dim toAccount As String = ""
                If Not noticeRequestRow.IsTOACCOUNTNull Then
                    toAccount = noticeRequestRow.TOACCOUNT
                End If

                '通知登録API呼び出し
                returnXmlNotice = SetNoticeInfo(noticeRequestRow.NOTICEREQID, reqClassId, toAccount)

                '処理結果が0以外の場合、処理を終了する
                rsltId = CInt(returnXmlNotice.ResultId)
                If rsltId <> 0 Then
                    Return False
                End If
            Next
        End If

        Logger.Info("UpdateNoticeRequest End")

        Return True
    End Function

    ''' <summary>
    ''' 通知登録API呼び出し
    ''' </summary>
    ''' <param name="noticereqId">通知依頼ID</param>
    ''' <param name="requestClassId">依頼ID</param>
    ''' <param name="toAccount">スタッフコード(受信先)</param>
    ''' <returns>処理結果</returns>
    ''' <remarks>通知登録API呼び出し</remarks>
    Private Function SetNoticeInfo(ByVal noticereqId As Long, _
                                   ByVal requestClassId As Nullable(Of Long), _
                                   ByVal toAccount As String) As XmlCommon

        Logger.Info("SetNoticeInfo Start")

        Dim returnXmlNotice As XmlCommon

        Dim dlrcd As String = StaffContext.Current.DlrCD
        Dim strcd As String = StaffContext.Current.BrnCD

        Using noticeData As New XmlNoticeData
            '送信日付
            noticeData.TransmissionDate = DateTimeFunc.Now()

            Using xmlAccount As New XmlAccount
                '査定以外の場合
                xmlAccount.ToAccount = toAccount
                noticeData.AccountList.Add(xmlAccount)

                '通知用の送信XMLを出力
                Logger.Info("[Send] Xml Account = " & xmlAccount.ToString)
            End Using

            Dim UserName As String = StaffContext.Current.UserName
            Dim Account As String = StaffContext.Current.Account

            Using requestNotice As New XmlRequestNotice
                requestNotice.DealerCode = dlrcd                                        '販売店コード
                requestNotice.StoreCode = strcd                                         '店舗コード
                requestNotice.RequestClass = "02"                                       '依頼種別
                requestNotice.Status = "2"                                              'ステータス
                requestNotice.RequestId = noticereqId                                   '依頼種別ID

                If Not IsNothing(requestClassId) Then
                    requestNotice.RequestClassId = CLng(requestClassId)                 '依頼ID
                End If

                requestNotice.FromAccount = Account                                     'スタッフコード（送信元）
                requestNotice.FromAccountName = UserName                                'スタッフ名（送信元）
                noticeData.RequestNotice = requestNotice

                '通知用の送信XMLを出力
                Logger.Info("[Send] Xml RequestNotice = " & requestNotice.ToString)
            End Using

            Using pushInfo As New XmlPushInfo
                pushInfo.PushCategory = "1"                                             'カテゴリータイプ
                pushInfo.PositionType = "1"                                             '表示位置
                pushInfo.Time = 3                                                       '表示時間
                pushInfo.DisplayType = "1"                                              '表示タイプ
                pushInfo.Color = "1"                                                    '色
                pushInfo.DisplayContents = WebWordUtility.GetWord(MESSAGE_ID_910)       '表示内容
                pushInfo.DisplayFunction = "icropScript.ui.openNoticeList()"            '表示時間数
                pushInfo.ActionFunction = "icropScript.ui.openNoticeList()"             'アクション時間数

                noticeData.PushInfo = pushInfo

                '通知用の送信XMLを出力
                Logger.Info("[Send] Xml pushInfo = " & pushInfo.ToString)
            End Using

            Using noticeInfo As New IC3040801BusinessLogic
                returnXmlNotice = noticeInfo.NoticeDisplay(noticeData, ConstCode.NoticeDisposal.Peculiar)
            End Using

            '通知用の受信XMLを出力
            Logger.Info("[Recv] Xml pushInfo = " & returnXmlNotice.ToString)

            Logger.Info("SetNoticeInfo End")

            Return returnXmlNotice

        End Using

        Logger.Info("SetNoticeInfo End")

    End Function

#End Region

End Class
