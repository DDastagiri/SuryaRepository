'-------------------------------------------------------------------------
'IC3040803BusinessLogic.vb
'-------------------------------------------------------------------------
'機能：Push送信Webサービス(ビジネスロジック)
'補足：
'作成：2014/2/12 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発
'更新：2015/07/17 TMEJ 明瀬 タブレットSMB性能改善 通知WebService化
'更新：2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする
'更新：2017/11/13 NSK 竹中(悠) TR-SVT-TMT-20170830-001 CHTが追加ジョブをチップにマージすると部品監視画面が更新されず、アラートがならない
'更新：
'─────────────────────────────────────
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web
Imports System.Text
Imports System.Xml
Imports System.IO
Imports System.Net
Imports System.Web
Imports System.Globalization
Imports System.Reflection
Imports Toyota.eCRB.Visit.Api.BizLogic

'2015/07/17 TMEJ 明瀬 タブレットSMB性能改善 通知WebService化 START
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.BizLogic
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.DataAccess.TabletSMBCommonClassDataSet
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.DataAccess.TabletSMBCommonClassDataSetTableAdapters
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.BizLogic
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.DataAccess
'2015/07/17 TMEJ 明瀬 タブレットSMB性能改善 通知WebService化 END

Public Class IC3040803BusinessLogic
    Inherits BaseBusinessComponent
    Implements IDisposable

#Region "定数"

#Region "XML関連"
    ''' <summary>
    ''' エンコード(UTF-8)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const EncodeUtf8 As Integer = 65001

    ''' <summary>
    ''' XMLの要素内の要素を取得する際の先頭に付ける値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const XmlRootDirectory As String = "//"

    ''' <summary>
    ''' 日付の書式
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DateFormat As String = "yyyy/MM/dd HH:mm:ss"

    '2017/11/13 NSK 竹中(悠) TR-SVT-TMT-20170830-001 CHTが追加ジョブをチップにマージすると部品監視画面が更新されず、アラートがならない START
    ''' <summary>
    ''' 着工指示済み（変更前）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const BeforeWorkOrderFlgOn As String = "1"
    '2017/11/13 NSK 竹中(悠) TR-SVT-TMT-20170830-001 CHTが追加ジョブをチップにマージすると部品監視画面が更新されず、アラートがならない END

#Region "受信XML"

    ''' <summary>
    ''' タグ名：Head
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagHead As String = "Head"
    ''' <summary>
    ''' タグ名：TransmissionDate
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagTransmissionDate As String = "TransmissionDate"
    ''' <summary>
    ''' タグ名：ToAccount
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagToAccount As String = "ToAccount"
    ''' <summary>
    ''' タグ名：Account
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagAccount As String = "Account"
    ''' <summary>
    ''' タグ名：RequestNotice
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagRequestNotice As String = "RequestNotice"
    ''' <summary>
    ''' タグ名：DealerCode
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagDealerCode As String = "DealerCode"
    ''' <summary>
    ''' タグ名：PushInfo
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagPushInfo As String = "PushInfo"
    ''' <summary>
    ''' タグ名：PushCategory
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagPushCategory As String = "PushCategory"
    ''' <summary>
    ''' タグ名：DisplayType
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagDisplayType As String = "DisplayType"
    ''' <summary>
    ''' タグ名：PositionType
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagPositionType As String = "PositionType"
    ''' <summary>
    ''' タグ名：Time
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagTime As String = "Time"
    ''' <summary>
    ''' タグ名：DisplayContents
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagDisplayContents As String = "DisplayContents"
    ''' <summary>
    ''' タグ名：Color 
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagColor As String = "Color"
    ''' <summary>
    ''' タグ名：PopWidth 
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagPopWidth As String = "PopWidth"
    ''' <summary>
    ''' タグ名：PopHeight 
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagPopHeight As String = "PopHeight"
    ''' <summary>
    ''' タグ名：PopX  
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagPopX As String = "PopX"
    ''' <summary>
    ''' タグ名：PopY  
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagPopY As String = "PopY"
    ''' <summary>
    ''' タグ名：DisplayFunction  
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagDisplayFunction As String = "DisplayFunction"
    ''' <summary>
    ''' タグ名：ActionFunction  
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagActionFunction As String = "ActionFunction"

    '2015/07/17 TMEJ 明瀬 タブレットSMB性能改善 通知WebService化 START

    ''' <summary>
    ''' タグ名：Contents
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagContents As String = "Contents"

    ''' <summary>
    ''' タグ名：販売店コード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagDlrCd As String = "DLR_CD"

    ''' <summary>
    ''' タグ名：店舗コード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagBrnCd As String = "BRN_CD"

    ''' <summary>
    ''' タグ名：サービス入庫ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagSvcInId As String = "SVCIN_ID"

    ''' <summary>
    ''' タグ名：作業内容ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagJobDtlId As String = "JOB_DTL_ID"

    ''' <summary>
    ''' タグ名：ストール利用ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagStallUseId As String = "STALL_USE_ID"

    ''' <summary>
    ''' タグ名：ストールID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagStallId As String = "STALL_ID"

    ''' <summary>
    ''' タグ名：スタッフアカウント(ログインアカウント)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagStaffAccount As String = "STAFF_ACCOUNT"

    ''' <summary>
    ''' タグ名：スタッフ名(ログインスタッフ名)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagStaffName As String = "STAFF_NAME"

    '2015/07/17 TMEJ 明瀬 タブレットSMB性能改善 通知WebService化 END

    '2017/11/13 NSK 竹中(悠) TR-SVT-TMT-20170830-001 CHTが追加ジョブをチップにマージすると部品監視画面が更新されず、アラートがならない START
    ''' <summary>
    ''' タグ名：着工指示フラグ（変更前）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagBeforeWorkOrderFlg As String = "BEFORE_WORKORDERFLG"
    '2017/11/13 NSK 竹中(悠) TR-SVT-TMT-20170830-001 CHTが追加ジョブをチップにマージすると部品監視画面が更新されず、アラートがならない END
#End Region


#Region "返信XML"
    ''' <summary>
    ''' タグ名：Response  
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagResponse As String = "Response"
    ''' <summary>
    ''' タグ名：Detail  
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagDetail As String = "Detail"
    ''' <summary>
    ''' タグ名：Common  
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagCommon As String = "Common"
    ''' <summary>
    ''' タグ名：ResultId  
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagResultId As String = "ResultId"
    ''' <summary>
    ''' タグ名：Message  
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagMessage As String = "Message"

#End Region


#End Region

#Region "カテゴリータイプ"
    Private Const Popup As String = "1"
    Private Const Action As String = "2"
    Private Const PushCategory_Popup As String = "popup"
    Private Const PushCategory_Action As String = "action"
#End Region

#Region "表示位置"
    Private Const Main As String = "0"
    Private Const Header As String = "1"
    Private Const Bottom As String = "2"
    Private Const Left As String = "3"
    Private Const Right As String = "4"
    Private Const Inside As String = "5"
    Private Const PositionType_Main As String = "main"
    Private Const PositionType_Header As String = "header"
    Private Const PositionType_Bottom As String = "bottom"
    Private Const PositionType_Left As String = "left"
    Private Const PositionType_Right As String = "right"
    Private Const PositionType_Inside As String = "inside"
#End Region

#Region "表示タイプ"
    Private Const Text As String = "1"
    Private Const Url As String = "2"
    Private Const Local As String = "3"
    Private Const Js As String = "4"
    Private Const DisplayType_Text As String = "text"
    Private Const DisplayType_Url As String = "url"
    Private Const DisplayType_Local As String = "local"
    Private Const DisplayType_Js As String = "js"
#End Region

#Region "色"
    Private Const Yellow As String = "1"
    Private Const Blue As String = "2"
    Private Const Color_Yellow As String = "F9EDBE64"
    Private Const Color_Blue As String = "CBE8FF64"
#End Region

#Region "エラーコード"

    '2015/07/17 TMEJ 明瀬 タブレットSMB性能改善 通知WebService化 START

    ''' <summary>
    ''' 成功
    ''' </summary>
    ''' <remarks></remarks>
    Private Const Success As Integer = 0

    '2015/07/17 TMEJ 明瀬 タブレットSMB性能改善 通知WebService化 END

    ''' <summary>
    ''' 必須項目がない
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ArgumentError As Integer = -1

    ''' <summary>
    ''' 予期せぬエラー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ExceptionError As Integer = 9999

#End Region

#End Region

    '2015/07/17 TMEJ 明瀬 タブレットSMB性能改善 通知WebService化 START

    Private LogServiceCommonBiz As New ServiceCommonClassBusinessLogic(True)

    '2015/07/17 TMEJ 明瀬 タブレットSMB性能改善 通知WebService化 END

#Region "Publicメッソド"
    ''' <summary>
    ''' プッシュ
    ''' </summary>
    ''' <param name="xml">受信のXML</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function WebServicePush(ByVal xml As String) As String

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                          "{0}.Start IN:receiveXml={1} ", _
                          MethodBase.GetCurrentMethod.Name, _
                          xml))
        '引数チェック
        If String.IsNullOrWhiteSpace(xml) Then
            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
               "{0}.Error, xml is not set.", _
               MethodBase.GetCurrentMethod.Name))
            Return Me.CreateRexml(ArgumentError)
        End If

        Dim rtXml As String = String.Empty

        Dim resultCode As Long
        'XmlDocument
        Dim receiveXmlDocument As New XmlDocument

        '受信した文字列をXML化
        receiveXmlDocument.LoadXml(xml)

        Dim transmissionDateNode As XmlNode = receiveXmlDocument.SelectSingleNode(XmlRootDirectory & TagHead)
        '送信日付の値を取得する
        Dim transmissionDateDictinary As Dictionary(Of String, String) _
            = Me.GetElementsData(transmissionDateNode, {TagTransmissionDate})
        Dim transmissionDate As String = transmissionDateDictinary.Item(TagTransmissionDate)
        '必須項目チェック
        If String.IsNullOrWhiteSpace(transmissionDate) Then
            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                           "{0}.Error, transmissionDate Value is not set.", _
                           MethodBase.GetCurrentMethod.Name))
            Return Me.CreateRexml(ArgumentError)
        End If

        Dim dealerCodeNode As XmlNode = receiveXmlDocument.SelectSingleNode(XmlRootDirectory & TagRequestNotice)
        '店舗コードの値を取得する
        Dim dealerCodeDictinary As Dictionary(Of String, String) _
            = Me.GetElementsData(dealerCodeNode, {TagDealerCode})
        Dim dealerCode As String = dealerCodeDictinary.Item(TagDealerCode)
        '必須項目チェック
        If String.IsNullOrWhiteSpace(dealerCode) Then
            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                           "{0}.Error, dealerCode Value is not set.", _
                           MethodBase.GetCurrentMethod.Name))
            Return Me.CreateRexml(ArgumentError)
        End If

        'accountタグ分繰り返し
        Dim accountNodeList As XmlNodeList = receiveXmlDocument.SelectNodes(XmlRootDirectory & TagAccount)
        Dim toAccountList As New List(Of String)

        For Each accountNode As XmlNode In accountNodeList
            '受信先の値を取得する
            Dim toAccountDictinary As Dictionary(Of String, String) _
                = Me.GetElementsData(accountNode, {TagToAccount})
            Dim toAccount As String = toAccountDictinary.Item(TagToAccount)
            If String.IsNullOrWhiteSpace(toAccount) Then
                Continue For
            End If
            toAccountList.Add(toAccount)
        Next

        If toAccountList.Count > 0 Then
            Dim pushInfoNode As XmlNode = receiveXmlDocument.SelectSingleNode(XmlRootDirectory & TagPushInfo)
            Dim pushInfo As PushInfoClass = Me.GetPushInfo(pushInfoNode)
            If pushInfo.Reslut = 0 Then
                pushInfo.DealerCode = dealerCode
                Me.DoPush(toAccountList, pushInfo)
            Else
                resultCode = pushInfo.Reslut
                Return Me.CreateRexml(resultCode)
            End If
        Else
            resultCode = ArgumentError
            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                           "{0}.Error, ToAccount Value is not set.", _
                                           MethodBase.GetCurrentMethod.Name))
            Return Me.CreateRexml(resultCode)
        End If

        rtXml = Me.CreateRexml(resultCode, dealerCode)
        Return rtXml

    End Function

    ''' <summary>
    ''' 返信XMLを作成
    ''' </summary>
    ''' <param name="reslutCode">結果コード</param>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CreateRexml(ByVal reslutCode As Long, _
                                 Optional ByVal inDealerCode As String = "") As String
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                          "{0}.Start, IN{1} ", _
                          MethodBase.GetCurrentMethod.Name, _
                          reslutCode))
        '65001がUTF-8
        Dim xmlEncode As Encoding = Encoding.GetEncoding(EncodeUtf8)
        'XMLドキュメント作成
        Dim xmlDocument As New XmlDocument
        'ヘッダ部作成(<?xml version="1.0" encoding="utf-8"?>の部分)
        Dim xmlDeclaration As XmlDeclaration = _
            xmlDocument.CreateXmlDeclaration("1.0", xmlEncode.BodyName, Nothing)

        'ルートタグ(Responseタグ)の作成
        Dim xmlRoot As XmlElement = xmlDocument.CreateElement(TagResponse)

        'Headタグ(Responseタグ)の作成
        Dim xmlHead As XmlElement = xmlDocument.CreateElement(TagHead)
        'Headタグの子要素を作成
        Dim transmissionDateTag As XmlElement = xmlDocument.CreateElement(TagTransmissionDate)
        '子要素に値を設定
        '現在日時取得
        If Not String.IsNullOrWhiteSpace(inDealerCode) Then
            Dim nowDateTime As String = _
                DateTimeFunc.Now(inDealerCode).ToString(DateFormat, CultureInfo.CurrentCulture)
            transmissionDateTag.AppendChild(xmlDocument.CreateTextNode(nowDateTime))
        Else
            '販売店コードがない場合からに設定する
            transmissionDateTag.AppendChild(xmlDocument.CreateTextNode(""))
        End If
        xmlHead.AppendChild(transmissionDateTag)

        'Detailタグ(Detailタグ)の作成
        Dim xmlDetail As XmlElement = xmlDocument.CreateElement(TagDetail)

        'Commonタグ(Commonタグ)の作成
        Dim xmlCommon As XmlElement = xmlDocument.CreateElement(TagCommon)

        'Commonタグの子要素を作成
        Dim resultIdTag As XmlElement = xmlDocument.CreateElement(TagResultId)
        Dim messageTag As XmlElement = xmlDocument.CreateElement(TagMessage)

        '子要素に値を設定
        resultIdTag.AppendChild(xmlDocument.CreateTextNode(reslutCode.ToString(CultureInfo.CurrentCulture)))
        messageTag.AppendChild(xmlDocument.CreateTextNode(""))


        With xmlCommon
            .AppendChild(resultIdTag)
            .AppendChild(messageTag)
        End With

        xmlDetail.AppendChild(xmlCommon)
        xmlRoot.AppendChild(xmlHead)
        xmlRoot.AppendChild(xmlDetail)


        With xmlDocument
            .AppendChild(xmlDeclaration)
            .AppendChild(xmlRoot)
        End With

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                  "{0}_End, OUT{1} ", _
                  MethodBase.GetCurrentMethod.Name, _
                  xmlDocument.InnerXml))

        Return xmlDocument.InnerXml

    End Function

    '2015/07/17 TMEJ 明瀬 タブレットSMB性能改善 通知WebService化 START

    ''' <summary>
    ''' 着工指示の通知とPushを送信する
    ''' </summary>
    ''' <param name="inXmlString">受信XML文字列</param>
    ''' <returns>返却XML文字列</returns>
    ''' <remarks></remarks>
    Public Function SendNoticePushJobInstruct(ByVal inXmlString As String) As String

        Logger.Info(String.Format( _
                    CultureInfo.InvariantCulture, _
                    "{0}.{1} Start[inXmlString={2}]", _
                    Me.GetType(), _
                    MethodBase.GetCurrentMethod.Name, _
                    inXmlString))

        '返却XML文字列
        Dim outXmlString As String = String.Empty

        Try

            '受信XML文字列の解析
            Dim receiveXmlDocument As New XmlDocument

            receiveXmlDocument.LoadXml(inXmlString)

            'XMLの必須タグチェック
            If Me.ExistsEmptyMandatoryTagNoticePushJobInstruct(receiveXmlDocument) Then

                '返却XMLの作成
                outXmlString = Me.CreateResponseXml(ArgumentError)

                'エラーログの出力
                Logger.Info(String.Format( _
                             CultureInfo.InvariantCulture, _
                             "{0}.{1} End[inXmlString={2}, outXmlString={3}]", _
                             Me.GetType(), _
                             MethodBase.GetCurrentMethod.Name, _
                             inXmlString, _
                             outXmlString))

                Return outXmlString

            End If

            'XMLタグの値チェック(特定のタグ値が半角数字のみかどうか)
            If Me.ExistsNotNumberValueTagNoticePushJobInstruct(receiveXmlDocument) Then

                '返却XMLの作成
                outXmlString = Me.CreateResponseXml(ArgumentError)

                'エラーログの出力
                Logger.Info(String.Format( _
                             CultureInfo.InvariantCulture, _
                             "{0}.{1} End[inXmlString={2}, outXmlString={3}]", _
                             Me.GetType(), _
                             MethodBase.GetCurrentMethod.Name, _
                             inXmlString, _
                             outXmlString))

                Return outXmlString

            End If

            '受信XMLのサービス入庫ID
            Dim serviceInId As Decimal = _
                CType(Me.GetXmlDocumentTagValue(receiveXmlDocument, TagSvcInId), Decimal)

            '受信XMLの作業内容ID
            Dim jobDetailId As Decimal = _
                CType(Me.GetXmlDocumentTagValue(receiveXmlDocument, TagJobDtlId), Decimal)

            '受信XMLのストール利用ID
            Dim stallUseId As Decimal = _
                CType(Me.GetXmlDocumentTagValue(receiveXmlDocument, TagStallUseId), Decimal)

            '受信XMLの販売店コード
            Dim dealerCode As String = _
                Me.GetXmlDocumentTagValue(receiveXmlDocument, TagDlrCd)

            '受信XMLの店舗コード
            Dim branchCode As String = _
                Me.GetXmlDocumentTagValue(receiveXmlDocument, TagBrnCd)

            '通知用の情報テーブル
            Dim dtNoticeInfo As TabletSmbCommonClassNoticeInfoDataTable = Nothing

            'チップの情報テーブル
            Dim dtChipInfo As TabletSmbCommonClassChipEntityDataTable = Nothing

            Using clsTabletSMBCommonClassDataAdpter As New TabletSMBCommonClassDataAdapter

                LogServiceCommonBiz.OutputLog(0, "●■● 1.1 TABLETSMBCOMMONCLASS_055 START")

                '通知用の情報取得
                dtNoticeInfo = clsTabletSMBCommonClassDataAdpter.GetNoticeInfo(serviceInId, _
                                                                               dealerCode, _
                                                                               branchCode, _
                                                                               jobDetailId)

                LogServiceCommonBiz.OutputLog(0, "●■● 1.1 TABLETSMBCOMMONCLASS_055 END")

                'チップの情報を取得
                dtChipInfo = clsTabletSMBCommonClassDataAdpter.GetChipEntity(stallUseId, 0)

            End Using

            'チップの予定開始日時
            Dim serviceWorkStartDateTime As Date = Date.MinValue

            'チップの予定終了日時
            Dim serviceWorkEndDateTime As Date = Date.MinValue

            'チップの情報が取得できている場合(取得できていないことはあり得ない)
            If 0 < dtChipInfo.Count Then

                serviceWorkStartDateTime = dtChipInfo(0).SCHE_START_DATETIME
                serviceWorkEndDateTime = dtChipInfo(0).SCHE_END_DATETIME

            End If

            Using clsTabletSmbCommonBiz As New TabletSMBCommonClassBusinessLogic

                '受信XMLのストールID
                Dim stallId As Decimal = _
                    CType(Me.GetXmlDocumentTagValue(receiveXmlDocument, TagStallId), Decimal)

                '受信XMLのスタッフアカウント
                Dim staffAccount As String = _
                    Me.GetXmlDocumentTagValue(receiveXmlDocument, TagStaffAccount)

                '受信XMLのスタッフ名
                Dim staffName As String = _
                    Me.GetXmlDocumentTagValue(receiveXmlDocument, TagStaffName)

                '2017/11/13 NSK 竹中(悠) TR-SVT-TMT-20170830-001 CHTが追加ジョブをチップにマージすると部品監視画面が更新されず、アラートがならない START
                ' Push完了フラグ
                Dim pushCompleteFlg As Boolean = False
                '2017/11/13 NSK 竹中(悠) TR-SVT-TMT-20170830-001 CHTが追加ジョブをチップにマージすると部品監視画面が更新されず、アラートがならない END

                '通知用の情報が取得できている場合
                If 0 < dtNoticeInfo.Count Then

                    Dim drNoticeInfo As TabletSmbCommonClassNoticeInfoRow _
                        = dtNoticeInfo(0)

                    '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする START

                    'LogServiceCommonBiz.OutputLog(8, "●■● 1.4 着工指示通知処理 START")

                    ''着工指示通知処理
                    'clsTabletSmbCommonBiz.JobInstructNotice(drNoticeInfo, _
                    '                                        serviceWorkStartDateTime, _
                    '                                        serviceWorkEndDateTime, _
                    '                                        stallId, _
                    '                                        dealerCode, _
                    '                                        branchCode, _
                    '                                        staffAccount, _
                    '                                        staffName)

                    'LogServiceCommonBiz.OutputLog(8, "●■● 1.4 着工指示通知処理 END")

                    '2017/11/13 NSK 竹中(悠) TR-SVT-TMT-20170830-001 CHTが追加ジョブをチップにマージすると部品監視画面が更新されず、アラートがならない START
                    '受信XMLの着工指示フラグ（変更前）
                    'タグ値が存在しない場合はbeforeWorkOrderFlgに空白が入る
                    Dim beforeWorkOrderFlg As String = _
                        Me.GetXmlDocumentTagValue(receiveXmlDocument, TagBeforeWorkOrderFlg)

                    '着工指示（変更前）の状態が未着工の場合
                    If Not BeforeWorkOrderFlgOn.Equals(beforeWorkOrderFlg) Then
                        '2017/11/13 NSK 竹中(悠) TR-SVT-TMT-20170830-001 CHTが追加ジョブをチップにマージすると部品監視画面が更新されず、アラートがならない END
                        Dim jobInstructTable As TabletSmbCommonClassJobInstructDataTable = Nothing

                        'チップと紐付くRO枝番を取得する
                        Using ta As New TabletSMBCommonClassDataAdapter
                            jobInstructTable = ta.GetROJobSeqByJobDtlId(jobDetailId)
                        End Using

                        '着工指示が行われた場合、通知を行う
                        If jobInstructTable.Rows.Count >= 1 Then

                            LogServiceCommonBiz.OutputLog(8, "●■● 1.4 着工指示通知処理 START")

                            '着工指示通知処理
                            clsTabletSmbCommonBiz.JobInstructNotice(drNoticeInfo, _
                                                                    serviceWorkStartDateTime, _
                                                                    serviceWorkEndDateTime, _
                                                                    stallId, _
                                                                    dealerCode, _
                                                                    branchCode, _
                                                                    staffAccount, _
                                                                    staffName)

                            LogServiceCommonBiz.OutputLog(8, "●■● 1.4 着工指示通知処理 END")

                            LogServiceCommonBiz.OutputLog(18, "●■● 1.5 着工指示Push処理① START")

                            '通知履歴作成対象にPush
                            clsTabletSmbCommonBiz.NoticeAccountPush(dealerCode, _
                                                                    branchCode, _
                                                                    staffAccount, _
                                                                    stallId)

                            LogServiceCommonBiz.OutputLog(18, "●■● 1.5 着工指示Push処理① END")

                            '2017/11/13 NSK 竹中(悠) TR-SVT-TMT-20170830-001 CHTが追加ジョブをチップにマージすると部品監視画面が更新されず、アラートがならない START
                            'Push完了フラグをTrueにする
                            pushCompleteFlg = True
                            '2017/11/13 NSK 竹中(悠) TR-SVT-TMT-20170830-001 CHTが追加ジョブをチップにマージすると部品監視画面が更新されず、アラートがならない END
                        End If

                        '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする END

                        '2017/11/13 NSK 竹中(悠) TR-SVT-TMT-20170830-001 CHTが追加ジョブをチップにマージすると部品監視画面が更新されず、アラートがならない START
                    End If
                    '2017/11/13 NSK 竹中(悠) TR-SVT-TMT-20170830-001 CHTが追加ジョブをチップにマージすると部品監視画面が更新されず、アラートがならない END
                End If

                LogServiceCommonBiz.OutputLog(23, "●■● 1.6 着工指示Push処理② START")

                '通知履歴作成対象外にPush
                '2017/11/13 NSK 竹中(悠) TR-SVT-TMT-20170830-001 CHTが追加ジョブをチップにマージすると部品監視画面が更新されず、アラートがならない START
                'clsTabletSmbCommonBiz.JobInstructPush(dealerCode, _
                '                                      branchCode, _
                '                                      staffAccount, _
                '                                      stallId)
                clsTabletSmbCommonBiz.JobInstructPush(dealerCode, _
                                                      branchCode, _
                                                      staffAccount, _
                                                      stallId, _
                                                      pushCompleteFlg)
                '2017/11/13 NSK 竹中(悠) TR-SVT-TMT-20170830-001 CHTが追加ジョブをチップにマージすると部品監視画面が更新されず、アラートがならない END

                LogServiceCommonBiz.OutputLog(23, "●■● 1.6 着工指示Push処理② END")

            End Using

            '返却XMLの作成
            outXmlString = Me.CreateResponseXml(Success)

        Catch ex As Exception

            '返却XMLの作成
            outXmlString = Me.CreateResponseXml(ExceptionError)

            'エラーログの出力
            Logger.Error(String.Format( _
                         CultureInfo.InvariantCulture, _
                         "{0}.{1} End[inXmlString={2}, outXmlString={3}]", _
                         Me.GetType(), _
                         MethodBase.GetCurrentMethod.Name, _
                         inXmlString, _
                         outXmlString), ex)

        End Try

        Logger.Info(String.Format( _
                    CultureInfo.InvariantCulture, _
                    "{0}.{1} End[outXmlString={2}]", _
                    Me.GetType(), _
                    MethodBase.GetCurrentMethod.Name, _
                    outXmlString))

        Return outXmlString

    End Function

    '2015/07/17 TMEJ 明瀬 タブレットSMB性能改善 通知WebService化 END

#End Region

#Region "Privateメソッド"
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

    ''' <summary>
    ''' 受信XMLからPush情報を取得する
    ''' </summary>
    ''' <param name="pushInfoXml"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetPushInfo(ByVal pushInfoXml As XmlNode) As PushInfoClass
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                          "{0}.Start IN:receiveXml={1} ", _
                          MethodBase.GetCurrentMethod.Name, _
                          pushInfoXml.InnerXml))

        Using clsPushInfo As New PushInfoClass
            Dim tagPushInfoList As List(Of String) = Me.CreateListofPushInfoTag()
            'VhcInfoタグの子要素設定値を取得する
            Dim pushInfoDictinary As Dictionary(Of String, String) _
                    = Me.GetElementsData(pushInfoXml, tagPushInfoList)
            '必須項目チェック
            If String.IsNullOrEmpty(pushInfoDictinary.Item(TagPushCategory)) OrElse _
                String.IsNullOrEmpty(pushInfoDictinary.Item(TagPositionType)) OrElse _
                String.IsNullOrEmpty(pushInfoDictinary.Item(TagDisplayType)) Then
                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                           "{0}.Error, Mandatory Item Value is not set.", _
                           MethodBase.GetCurrentMethod.Name))
                clsPushInfo.Reslut = ArgumentError
                Return clsPushInfo
            End If

            With clsPushInfo
                .Reslut = 0
                .PushCategory = pushInfoDictinary.Item(TagPushCategory)
                .PositionType = pushInfoDictinary.Item(TagPositionType)

                If pushInfoDictinary.ContainsKey(TagTime) Then
                    .Time = pushInfoDictinary.Item(TagTime)
                Else
                    .Time = ""
                End If

                If pushInfoDictinary.ContainsKey(TagDisplayType) Then
                    .DisplayType = pushInfoDictinary.Item(TagDisplayType)
                Else
                    .DisplayType = ""
                End If

                If pushInfoDictinary.ContainsKey(TagDisplayContents) Then
                    .DisplayContents = pushInfoDictinary.Item(TagDisplayContents)
                Else
                    .DisplayContents = ""
                End If

                If pushInfoDictinary.ContainsKey(TagColor) Then
                    .Color = pushInfoDictinary.Item(TagColor)
                Else
                    .Color = ""
                End If

                If pushInfoDictinary.ContainsKey(TagPopWidth) Then
                    .PopWidth = pushInfoDictinary.Item(TagPopWidth)
                Else
                    .PopWidth = ""
                End If

                If pushInfoDictinary.ContainsKey(TagPopHeight) Then
                    .PopHeight = pushInfoDictinary.Item(TagPopHeight)
                Else
                    .PopHeight = ""
                End If

                If pushInfoDictinary.ContainsKey(TagPopX) Then
                    .PopX = pushInfoDictinary.Item(TagPopX)
                Else
                    .PopX = ""
                End If

                If pushInfoDictinary.ContainsKey(TagPopY) Then
                    .PopY = pushInfoDictinary.Item(TagPopY)
                Else
                    .PopY = ""
                End If

                If pushInfoDictinary.ContainsKey(TagDisplayFunction) Then
                    .DisplayFunction = pushInfoDictinary.Item(TagDisplayFunction)
                Else
                    .DisplayFunction = ""
                End If

                If pushInfoDictinary.ContainsKey(TagActionFunction) Then
                    .ActionFunction = pushInfoDictinary.Item(TagActionFunction)
                Else
                    .ActionFunction = ""
                End If
            End With

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                  "{0}.End ", _
                  MethodBase.GetCurrentMethod.Name))
            Return clsPushInfo
        End Using

    End Function

    ''' <summary>
    ''' PushInfo全てのタグをリスト化
    ''' </summary>
    ''' <returns>VhcInfoタグのリスト</returns>
    ''' <remarks></remarks>
    Private Function CreateListofPushInfoTag() As List(Of String)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                          "{0}.Start ", _
                          MethodBase.GetCurrentMethod.Name))
        Dim tagPushInfoList As New List(Of String)
        With tagPushInfoList
            .Add(TagPushCategory)
            .Add(TagPositionType)
            .Add(TagTime)
            .Add(TagDisplayType)
            .Add(TagDisplayContents)
            .Add(TagColor)
            .Add(TagPopWidth)
            .Add(TagPopHeight)
            .Add(TagPopX)
            .Add(TagPopY)
            .Add(TagDisplayFunction)
            .Add(TagActionFunction)
        End With

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                          "{0}_End, OUT:{1}", _
                          MethodBase.GetCurrentMethod.Name, _
                          String.Join(", ", tagPushInfoList.ToArray())))
        Return tagPushInfoList
    End Function

    ''' <summary>
    ''' Push実行
    ''' </summary>
    ''' <param name="toAccountList">送信先リスト</param>
    ''' <param name="inPushInfo">Push情報</param>
    ''' <remarks></remarks>
    Private Sub DoPush(ByVal toAccountList As List(Of String), ByVal inPushInfo As PushInfoClass)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                          "{0}.Start ", _
                          MethodBase.GetCurrentMethod.Name))

        For Each toAccount As String In toAccountList
            'POST送信メッセージの作成
            Dim postSendMessage As New StringBuilder

            'catの値を設定
            Select Case inPushInfo.PushCategory
                Case Action
                    postSendMessage.Append("cat=" & PushCategory_Action)
                Case Popup
                    postSendMessage.Append("cat=" & PushCategory_Popup)
            End Select

            'typeの値を設定
            Select Case inPushInfo.PositionType
                Case Bottom
                    postSendMessage.Append("&type=" & PositionType_Bottom)
                Case Header
                    postSendMessage.Append("&type=" & PositionType_Header)
                Case Inside
                    postSendMessage.Append("&type=" & PositionType_Inside)
                Case Left
                    postSendMessage.Append("&type=" & PositionType_Left)
                Case Main
                    postSendMessage.Append("&type=" & PositionType_Main)
                Case Right
                    postSendMessage.Append("&type=" & PositionType_Right)
            End Select

            'subの値を設定
            Select Case inPushInfo.DisplayType
                Case Js
                    postSendMessage.Append("&sub=" & DisplayType_Js)
                Case Text
                    postSendMessage.Append("&sub=" & DisplayType_Text)
                    postSendMessage.Append("&msg=" & inPushInfo.DisplayContents)
                Case Url
                    postSendMessage.Append("&sub=" & DisplayType_Url)
                    postSendMessage.Append("&url=" & inPushInfo.DisplayContents)
                Case Local
                    postSendMessage.Append("&sub=" & DisplayType_Local)
                    postSendMessage.Append("&fname=" & inPushInfo.DisplayContents)
            End Select

            'colorの値を設定
            Select Case inPushInfo.Color
                Case Yellow
                    postSendMessage.Append("color=" & Color_Yellow)
                Case Blue
                    postSendMessage.Append("color=" & Color_Blue)
            End Select

            postSendMessage.Append("&uid=" & toAccount)
            If String.IsNullOrWhiteSpace(inPushInfo.Time) Then
                postSendMessage.Append("&time=0")
            Else
                postSendMessage.Append("&time=" & inPushInfo.Time)
            End If

            postSendMessage.Append("&width=" & inPushInfo.PopWidth)
            postSendMessage.Append("&height=" & inPushInfo.PopHeight)
            postSendMessage.Append("&pox=" & inPushInfo.PopX)
            postSendMessage.Append("&poy=" & inPushInfo.PopY)
            postSendMessage.Append("&js1=" & inPushInfo.DisplayFunction)
            postSendMessage.Append("&js2=" & inPushInfo.ActionFunction)

            '送信処理
            Dim visitUtility As New VisitUtility
            visitUtility.SendPush(postSendMessage.ToString, inPushInfo.DealerCode)
        Next

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                  "{0}_End ", _
                  MethodBase.GetCurrentMethod.Name))
    End Sub

    '2015/07/17 TMEJ 明瀬 タブレットSMB性能改善 通知WebService化 START

    ''' <summary>
    ''' NoticePushJobInstructの必須タグ値存在確認
    ''' </summary>
    ''' <param name="inXmlDocument">受信XMLドキュメント</param>
    ''' <returns>True：必須タグに空の値有り / False：必須タグに空の値無し</returns>
    ''' <remarks></remarks>
    Private Function ExistsEmptyMandatoryTagNoticePushJobInstruct(ByVal inXmlDocument As XmlDocument) As Boolean

        Logger.Info(String.Format( _
                    CultureInfo.InvariantCulture, _
                    "{0}.{1} Start", _
                    Me.GetType(), _
                    MethodBase.GetCurrentMethod.Name))

        '返却値
        Dim returnValue As Boolean = False

        '販売店コード「DLR_CD」
        If Me.ExistsEmptyMandatoryTag(inXmlDocument, TagDlrCd) Then returnValue = True

        '店舗コード「BRN_CD」
        If Me.ExistsEmptyMandatoryTag(inXmlDocument, TagBrnCd) Then returnValue = True

        'サービス入庫ID「SVCIN_ID」
        If Me.ExistsEmptyMandatoryTag(inXmlDocument, TagSvcInId) Then returnValue = True

        '作業内容ID「JOB_DTL_ID」
        If Me.ExistsEmptyMandatoryTag(inXmlDocument, TagJobDtlId) Then returnValue = True

        'ストール利用ID「STALL_USE_ID」
        If Me.ExistsEmptyMandatoryTag(inXmlDocument, TagStallUseId) Then returnValue = True

        'ストールID「STALL_ID」
        If Me.ExistsEmptyMandatoryTag(inXmlDocument, TagStallId) Then returnValue = True

        'スタッフアカウント「STAFF_ACCOUNT」
        If Me.ExistsEmptyMandatoryTag(inXmlDocument, TagStaffAccount) Then returnValue = True

        'スタッフ名「STAFF_NAME」
        If Me.ExistsEmptyMandatoryTag(inXmlDocument, TagStaffName) Then returnValue = True

        Logger.Info(String.Format( _
                    CultureInfo.InvariantCulture, _
                    "{0}.{1} End[returnValue={2}]", _
                    Me.GetType(), _
                    MethodBase.GetCurrentMethod.Name, _
                    returnValue))

        Return returnValue

    End Function

    ''' <summary>
    ''' タグ値存在確認
    ''' </summary>
    ''' <param name="inXmlDocument">受信XMLドキュメント</param>
    ''' <param name="tagName">タグ名称</param>
    ''' <returns>True：引数のタグ名称のタグ値が空 / False：引数のタグ名称のタグ値が空でない</returns>
    ''' <remarks></remarks>
    Private Function ExistsEmptyMandatoryTag(ByVal inXmlDocument As XmlDocument, _
                                             ByVal tagName As String) As Boolean

        Logger.Info(String.Format( _
                    CultureInfo.InvariantCulture, _
                    "{0}.{1} Start", _
                    Me.GetType(), _
                    MethodBase.GetCurrentMethod.Name))

        '返却値
        Dim returnValue As Boolean = False

        'タグに設定された値を取得
        Dim tagValue As String = Me.GetXmlDocumentTagValue(inXmlDocument, _
                                                           tagName)

        '必須チェック
        If String.IsNullOrWhiteSpace(tagValue) Then

            Logger.Info(String.Format( _
                         CultureInfo.InvariantCulture, _
                         "{0}.{1} End[Value of tag({2}) is empty.]", _
                         Me.GetType(), _
                         MethodBase.GetCurrentMethod.Name, _
                         tagName))

            returnValue = True

        End If

        Logger.Info(String.Format( _
                    CultureInfo.InvariantCulture, _
                    "{0}.{1} End[returnValue={2}]", _
                    Me.GetType(), _
                    MethodBase.GetCurrentMethod.Name, _
                    returnValue))

        Return returnValue

    End Function

    ''' <summary>
    ''' タグ値存在確認
    ''' </summary>
    ''' <param name="inXmlDocument">受信XMLドキュメント</param>
    ''' <param name="tagName">タグ名称</param>
    ''' <returns>タグに設定された値(タグがなければ空文字)</returns>
    ''' <remarks></remarks>
    Private Function GetXmlDocumentTagValue(ByVal inXmlDocument As XmlDocument, _
                                            ByVal tagName As String) As String

        Logger.Info(String.Format( _
                    CultureInfo.InvariantCulture, _
                    "{0}.{1} Start", _
                    Me.GetType(), _
                    MethodBase.GetCurrentMethod.Name))

        '返却値
        Dim returnValue As String = String.Empty

        'XMLタグ
        Dim node As XmlNode = inXmlDocument.SelectSingleNode(XmlRootDirectory & tagName)

        If Not IsNothing(node) Then

            returnValue = node.InnerText

        End If

        Logger.Info(String.Format( _
                    CultureInfo.InvariantCulture, _
                    "{0}.{1} End[returnValue={2}]", _
                    Me.GetType(), _
                    MethodBase.GetCurrentMethod.Name, _
                    returnValue))

        Return returnValue

    End Function

    ''' <summary>
    ''' NoticePushJobInstructタグで半角数値指定のタグ値が半角数値のみかどうか確認
    ''' </summary>
    ''' <param name="inXmlDocument">受信XMLドキュメント</param>
    ''' <returns>True：半角数値指定のタグ値が半角数値以外有り / False：半角数値指定のタグ値が半角数値のみ</returns>
    ''' <remarks></remarks>
    Private Function ExistsNotNumberValueTagNoticePushJobInstruct(ByVal inXmlDocument As XmlDocument) As Boolean

        Logger.Info(String.Format( _
                    CultureInfo.InvariantCulture, _
                    "{0}.{1} Start", _
                    Me.GetType(), _
                    MethodBase.GetCurrentMethod.Name))

        '返却値
        Dim returnValue As Boolean = False

        'サービス入庫ID「SVCIN_ID」
        If Not Me.IsNumberValueTag(inXmlDocument, TagSvcInId) Then returnValue = True

        '作業内容ID「JOB_DTL_ID」
        If Not Me.IsNumberValueTag(inXmlDocument, TagJobDtlId) Then returnValue = True

        'ストール利用ID「STALL_USE_ID」
        If Not Me.IsNumberValueTag(inXmlDocument, TagStallUseId) Then returnValue = True

        'ストールID「STALL_ID」
        If Not Me.IsNumberValueTag(inXmlDocument, TagStallId) Then returnValue = True

        Logger.Info(String.Format( _
                    CultureInfo.InvariantCulture, _
                    "{0}.{1} End[returnValue={2}]", _
                    Me.GetType(), _
                    MethodBase.GetCurrentMethod.Name, _
                    returnValue))

        Return returnValue

    End Function

    ''' <summary>
    ''' タグ値が半角数値のみかどうか確認
    ''' </summary>
    ''' <param name="inXmlDocument">受信XMLドキュメント</param>
    ''' <param name="tagName">タグ名称</param>
    ''' <returns>True：引数のタグ名称のタグ値が半角数値のみ / False：引数のタグ名称のタグ値が半角数値以外有り</returns>
    ''' <remarks></remarks>
    Private Function IsNumberValueTag(ByVal inXmlDocument As XmlDocument, _
                                      ByVal tagName As String) As Boolean

        Logger.Info(String.Format( _
                    CultureInfo.InvariantCulture, _
                    "{0}.{1} Start", _
                    Me.GetType(), _
                    MethodBase.GetCurrentMethod.Name))

        '返却値
        Dim returnValue As Boolean = True

        'タグに設定された値を取得
        Dim tagValue As String = Me.GetXmlDocumentTagValue(inXmlDocument, _
                                                           tagName)

        '半角数値のみチェック
        If Not Validation.IsHankakuNumber(tagValue) Then

            Logger.Info(String.Format( _
                         CultureInfo.InvariantCulture, _
                         "{0}.{1} End[Value of tag({2}) is not only number.]", _
                         Me.GetType(), _
                         MethodBase.GetCurrentMethod.Name, _
                         tagName))

            returnValue = False

        End If

        Logger.Info(String.Format( _
                    CultureInfo.InvariantCulture, _
                    "{0}.{1} End[returnValue={2}]", _
                    Me.GetType(), _
                    MethodBase.GetCurrentMethod.Name, _
                    returnValue))

        Return returnValue

    End Function

    ''' <summary>
    ''' 返信XMLを作成
    ''' </summary>
    ''' <param name="resultCode">結果コード</param>
    ''' <returns>返信XML</returns>
    ''' <remarks></remarks>
    Private Function CreateResponseXml(ByVal resultCode As Integer) As String

        Logger.Info(String.Format( _
                    CultureInfo.InvariantCulture, _
                    "{0}.{1} Start[resultCode={2}]", _
                    Me.GetType(), _
                    MethodBase.GetCurrentMethod.Name, _
                    resultCode))

        '65001がUTF-8
        Dim xmlEncode As Encoding = Encoding.GetEncoding(EncodeUtf8)

        'XMLドキュメント作成
        Dim xmlDocument As New XmlDocument

        'ヘッダ部作成(<?xml version="1.0" encoding="utf-8"?>の部分)
        Dim xmlDeclaration As XmlDeclaration = _
            xmlDocument.CreateXmlDeclaration("1.0", xmlEncode.BodyName, Nothing)

        'ルートタグ(Responseタグ)の作成
        Dim xmlRoot As XmlElement = xmlDocument.CreateElement(TagResponse)

        'ルートタグの子要素(ResultIdタグ)を作成
        Dim resultIdTag As XmlElement = xmlDocument.CreateElement(TagResultId)

        'ResultIdタグに値を設定
        resultIdTag.AppendChild( _
            xmlDocument.CreateTextNode(resultCode.ToString(CultureInfo.CurrentCulture)))

        'ルートタグに子要素を設定
        xmlRoot.AppendChild(resultIdTag)

        'XMLドキュメントに要素を設定
        With xmlDocument
            .AppendChild(xmlDeclaration)
            .AppendChild(xmlRoot)
        End With

        Logger.Info(String.Format( _
                    CultureInfo.InvariantCulture, _
                    "{0}.{1} End[responseXml={2}]", _
                    Me.GetType(), _
                    MethodBase.GetCurrentMethod.Name, _
                    xmlDocument.InnerXml))

        Return xmlDocument.InnerXml

    End Function

    '2015/07/17 TMEJ 明瀬 タブレットSMB性能改善 通知WebService化 END

#End Region

#Region "Privateクラス"
    ''' <summary>
    ''' Push情報クラス
    ''' </summary>
    ''' <remarks></remarks>
    Private Class PushInfoClass
        Implements IDisposable
        Public Property Reslut As Long
        Public Property DealerCode As String
        Public Property PushCategory As String
        Public Property PositionType As String
        Public Property Time As String
        Public Property DisplayType As String
        Public Property DisplayContents As String
        Public Property Color As String
        Public Property PopWidth As String
        Public Property PopHeight As String
        Public Property PopX As String
        Public Property PopY As String
        Public Property DisplayFunction As String
        Public Property ActionFunction As String

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
