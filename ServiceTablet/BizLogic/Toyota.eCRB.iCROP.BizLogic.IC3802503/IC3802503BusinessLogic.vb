'-------------------------------------------------------------------------
'IC3802503BusinessLogic.vb
'-------------------------------------------------------------------------
'機能：部品ステータス情報取得(ビジネスロジック)
'補足：
'作成：2013/12/25 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発
'更新：2015/06/05 TMEJ 小澤 TMT号口調査 XML解析中にエラーが発生した場合はログを出力するように修正
'更新：2020/01/29 NSK 今泉 TR-SVT-TKM-20191030-001 新車が販売店に来店した際にVIN番号を入力しても検索エラーが表示される
'更新：
'─────────────────────────────────────

Imports System.Globalization
Imports System.Reflection
Imports System.Xml
Imports System.Text
Imports System.IO
Imports System.Net
Imports System.Web
Imports System.Text.RegularExpressions
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.DMSLinkage.PartsInfo.Api.DataAccess.IC3802503DataSet
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.BizLogic
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.DataAccess

Public Class IC3802503BusinessLogic
    Inherits BaseBusinessComponent
    Implements IDisposable

#Region "列挙体"

    ''' <summary>
    ''' IC3802503の返却コード列挙体
    ''' </summary>
    ''' <remarks>
    ''' IC3802503PartsStatusDataTableの
    ''' ResultCodeカラムに設定される値。
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

#Region "定数"

    ''' <summary>
    ''' 本クラスの名前
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MyClassName As String = "IC3802503BusinessLogic"

    ''' <summary>
    ''' XMLの要素内の要素を取得する際の先頭に付ける値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const XmlRootDirectory As String = "//"

    ' ''' <summary>
    ' ''' 日付フォーマット(yyyy/MM/dd HH:mm:ss)
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const yyyyMMddHHmmssDateFormat As String = "yyyy/MM/dd HH:mm:ss"

    ''' <summary>
    ''' 日付フォーマット(dd/MM/yyyy HH:mm:ss)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ddMMyyyyHHmmssDateFormat As String = "dd/MM/yyyy HH:mm:ss"

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
    ''' 基幹連携URL（部品ステータス情報）
    ''' </summary>
    Private Const DlrSysLinkUrlPartsStatus = "LINK_URL_PARTS_STATUS"

#End Region

#Region "送信XML関連"

    ''' <summary>
    ''' メッセージID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RequestPartsStatusId As String = "IC3A09918"

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
    ''' WebService(IC3A09919)メソッド名
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WebServiceMethodName As String = "GetPartsStatus"

    ''' <summary>
    ''' WebService(IC3A09919)引数名
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WebServiceArgumentName As String = "xsData="

#End Region

#Region "タグ名"

#Region "要求XML関連"

#Region "Partsノード"

    ''' <summary>
    ''' タグ名：Parts
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagParts As String = "Parts"

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

#End Region

#Region "PartsSearchConditionノード"

    ''' <summary>
    ''' タグ名：PartsSearchCondition
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagPartsSearchCondition As String = "PartsSearchCondition"

    ''' <summary>
    ''' タグ名：R_O
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagRO As String = "R_O"

    ''' <summary>
    ''' タグ名：R_O_SEQNO
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagROSEQNO As String = "R_O_SEQNO"

#End Region

#End Region

#End Region

#End Region

#Region "受信XML関連"

#Region "Parts_Resultノード"

    ''' <summary>
    ''' タグ名：Parts_Result
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagPartsResult As String = "Parts_Result"

    ''' <summary>
    ''' タグ名：ResultCode
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagResultCode As String = "ResultCode"

#Region "PARTS_STATUSノード"

    ''' <summary>
    ''' タグ名：PARTS_STATUS
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagPartsStatus As String = "PARTS_STATUS"

    ''' <summary>
    ''' タグ名：PARTS_ISSUE_STATUS
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagPartsIssueStatus As String = "PARTS_ISSUE_STATUS"

    ''' <summary>
    ''' タグ名：RequestedStaffID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagRequestedStaffId As String = "RequestedStaffID"

    '※R_OタグとR_O_SEQNOタグは要求XMLと共通

    ''' <summary>
    ''' タグ名：BILL
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagBILL As String = "BILL"

#End Region

#End Region

#End Region

#End Region

#Region "エラーコード"

    ''' <summary>
    ''' システム設定不備エラー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ErrorSystemSetting As String = "1101"

    ''' <summary>
    ''' 販売店システム設定不備エラー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ErrorSystemSettingDlr As String = "1102"

    ''' <summary>
    ''' 基幹コードマップ不備エラー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ErrorDmsCodeMap As String = "1103"

    ''' <summary>
    ''' 引数エラー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ErrorArgument As String = "6101"

#End Region

#Region "部品準備ステータス"

    ''' <summary>
    ''' 部品準備ステータス：8(完了)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PartsIssueStatusIssued As String = "8"

#End Region

#End Region

#Region "Publicメソッド"

    ''' <summary>
    ''' 部品のステータス情報リストを取得する
    ''' </summary>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inBranchCode">店舗コード</param>
    ''' <param name="inRONumInfoTable">RO番号とRO枝番で構成されたテーブル</param>
    ''' <returns>IC3802503PartsStatusDataTable</returns>
    ''' <remarks>
    ''' 基幹側のWebServiceにリクエストし、返却されたXMLのデータをDataTableに設定して返却する。
    ''' 戻り値のDataTable1行目、ResultCodeカラムに0以外が設定されている場合、エラー発生。
    ''' 要求した部品ステータス情報が見つからない場合、Nothingを返却。
    ''' XML解析中にエラーが発生した場合はログを出力するようにする。
    ''' </remarks>
    ''' <history>
    ''' 2015/06/05 TMEJ 小澤 TMT号口調査 XML解析中にエラーが発生した場合はログを出力するように修正
    ''' </history>
    Public Function GetPartsStatusList(ByVal inDealerCode As String, _
                                       ByVal inBranchCode As String, _
                                       ByVal inRONumInfoTable As IC3802503RONumInfoDataTable) As IC3802503PartsStatusDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0} {1}.Start IN:inDealerCode={2}, inBranchCode={3}", _
                                  MyClassName, _
                                  MethodBase.GetCurrentMethod.Name, _
                                  inDealerCode, _
                                  inBranchCode))

        '返却用の部品詳細テーブル
        Dim partsStatusTable As IC3802503PartsStatusDataTable = Nothing

        Try

            '引数チェック
            Dim argumentCheckFlg As Boolean = Me.CheckArgument(inDealerCode, _
                                                               inBranchCode, _
                                                               inRONumInfoTable)

            If Not argumentCheckFlg Then
                '引数チェックエラー時

                'その他のエラーでエラーテーブル作成
                partsStatusTable = Me.CreateErrorTable(Result.otherError)
                Exit Try

            End If

            '引数のinRONumInfoTableの内容をログに出力
            Me.OutPutDataTableLog(inRONumInfoTable, MethodBase.GetCurrentMethod.Name)

            '現在日時取得
            Dim nowDateTime As Date = DateTimeFunc.Now(inDealerCode)

            'システム設定値を取得
            Dim systemSettingsValueRow As IC3802503SystemSettingValueRow _
                = Me.GetSystemSettingValues()

            'システム設定値の取得でエラーがあった場合
            If IsNothing(systemSettingsValueRow) Then

                'その他のエラーでエラーテーブル作成
                partsStatusTable = Me.CreateErrorTable(Result.otherError)
                Exit Try

            End If

            '基幹販売店コード、店舗コードを取得
            Dim dmsDlrBrnTable As ServiceCommonClassDataSet.DmsCodeMapDataTable _
                = Me.GetDmsCode(inDealerCode, inBranchCode)

            '基幹販売店コード、店舗コードの取得でエラーがあった場合
            If IsNothing(dmsDlrBrnTable) Then

                'その他のエラーでエラーテーブル作成
                partsStatusTable = Me.CreateErrorTable(Result.otherError)
                Exit Try

            End If

            '送信XMLの作成
            Dim sendXml As XmlDocument = Me.StructRequestPartsStatusXml(systemSettingsValueRow, _
                                                                        dmsDlrBrnTable.Item(0), _
                                                                        inRONumInfoTable, _
                                                                        nowDateTime)

            'WebServiceのURLを作成
            Dim createUrl As String = String.Concat(systemSettingsValueRow.LINK_URL_PARTS_STATUS, _
                                                    "/", _
                                                    WebServiceMethodName)

            '送信XMLを引数に設定
            Dim sendXmlString As String = String.Concat(WebServiceArgumentName, _
                                                        sendXml.InnerXml)

            'WebService送受信処理
            Dim resultString As String = CallWebServiceSite(sendXmlString, _
                                                            createUrl, _
                                                            systemSettingsValueRow.LINK_SEND_TIMEOUT_VAL)

            If CType(Result.timeOutError, String).Equals(resultString) _
            OrElse CType(Result.otherError, String).Equals(resultString) Then
                '送受信処理でエラー発生時

                '該当エラーでエラーテーブル作成
                partsStatusTable = Me.CreateErrorTable(CType(resultString, Long))
                Exit Try

            End If

            'XML名前空間用の正規表現設定
            Dim regex As Regex = New Regex(" xmln.*=""[^""]*"".")

            'XML名前空間を除去
            Dim editResultString As String = regex.Replace(resultString, Space(0))

            '受信XMLを解析し、部品ステータスDataTableを作成
            partsStatusTable = Me.CreatePartsStatusTable(editResultString)

            '2015/06/05 TMEJ 小澤 TMT号口調査 XML解析中にエラーが発生した場合はログを出力するように修正 START

            '作成したDataTebleのエラーコードチェック
            If Not (IsNothing(partsStatusTable)) AndAlso _
               0 < partsStatusTable.Count AndAlso _
               Not (partsStatusTable(0).IsResultCodeNull) AndAlso _
               partsStatusTable(0).ResultCode <> Result.Success Then
                'データが存在している且つ、「0：成功」以外の場合
                'ログを出力する
                '送信XMLのログ出力
                Logger.Warn(String.Format(CultureInfo.InvariantCulture, _
                                           "{0} {1}.CreatePartsStatusTable Error : SendXML = {2}", _
                                           MyClassName, _
                                           MethodBase.GetCurrentMethod.Name, _
                                           sendXml.InnerXml))
                '受信XMLのログ出力
                Logger.Warn(String.Format(CultureInfo.InvariantCulture, _
                                           "{0} {1}.CreatePartsStatusTable Error : ReceivedXML = {2}", _
                                           MyClassName, _
                                           MethodBase.GetCurrentMethod.Name, _
                                           editResultString))

            End If

            '2015/06/05 TMEJ 小澤 TMT号口調査 XML解析中にエラーが発生した場合はログを出力するように修正 END

        Catch ex As Exception

            Logger.Error(String.Format(CultureInfo.InvariantCulture, _
                                       "{0} {1}.Error ", _
                                       MyClassName, _
                                       MethodBase.GetCurrentMethod.Name), ex)

            'その他のエラーでエラーテーブル作成
            partsStatusTable = Me.CreateErrorTable(Result.otherError)

        End Try

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0} {1}_End", _
                                  MyClassName, _
                                  MethodBase.GetCurrentMethod.Name))

        Return partsStatusTable

    End Function

#End Region

#Region "Privateメソッド"

#Region "取得系"

    ''' <summary>
    ''' システム設定、販売店設定から必要な設定値を取得する
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetSystemSettingValues() As IC3802503SystemSettingValueRow

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0} {1}_Start", _
                                  MyClassName, _
                                  MethodBase.GetCurrentMethod.Name))

        '戻り値
        Dim retRow As IC3802503SystemSettingValueRow = Nothing

        'エラー発生フラグ
        Dim errorFlg As Boolean = False

        Try
            Using svcCommonBiz As New ServiceCommonClassBusinessLogic

                '******************************
                '* システム設定から取得
                '******************************
                '基幹連携送信時タイムアウト値
                Dim linkSendTimeoutVal As String _
                    = svcCommonBiz.GetSystemSettingValueBySettingName(SysLinkSendTimeOutVal)

                If String.IsNullOrEmpty(linkSendTimeoutVal) Then
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                               "{0} {1}.Error ErrorCode:{2}, {3} does not exist.", _
                                               MyClassName, _
                                               MethodBase.GetCurrentMethod.Name, _
                                               ErrorSystemSetting, _
                                               SysLinkSendTimeOutVal))
                    errorFlg = True
                    Exit Try
                End If

                '国コード
                Dim countryCode As String _
                    = svcCommonBiz.GetSystemSettingValueBySettingName(SysCountryCode)

                If String.IsNullOrEmpty(countryCode) Then
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                               "{0} {1}.Error ErrorCode:{2}, {3} does not exist.", _
                                               MyClassName, _
                                               MethodBase.GetCurrentMethod.Name, _
                                               ErrorSystemSetting, _
                                               SysCountryCode))
                    errorFlg = True
                    Exit Try
                End If

                '******************************
                '* 販売店システム設定から取得
                '******************************
                '送信先アドレス
                Dim linkUrlPartsStatusInfo As String _
                    = svcCommonBiz.GetDlrSystemSettingValueBySettingName(DlrSysLinkUrlPartsStatus)

                If String.IsNullOrEmpty(linkUrlPartsStatusInfo) Then
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                               "{0} {1}.Error ErrorCode:{2}, {3} does not exist.", _
                                               MyClassName, _
                                               MethodBase.GetCurrentMethod.Name, _
                                               ErrorSystemSettingDlr, _
                                               DlrSysLinkUrlPartsStatus))
                    errorFlg = True
                    Exit Try
                End If

                Using table As New IC3802503SystemSettingValueDataTable

                    retRow = table.NewIC3802503SystemSettingValueRow

                    With retRow
                        '取得した値を戻り値のデータ行に設定
                        .LINK_SEND_TIMEOUT_VAL = linkSendTimeoutVal
                        .DIST_CD = countryCode
                        .LINK_URL_PARTS_STATUS = linkUrlPartsStatusInfo
                    End With

                End Using

            End Using

        Finally

            If errorFlg Then
                retRow = Nothing
            End If

        End Try

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0} {1}_End", _
                                  MyClassName, _
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
                                  "{0} {1}.Start IN:dealerCode={2}, branchCode={3} ", _
                                  MyClassName, _
                                  MethodBase.GetCurrentMethod.Name, _
                                  dealerCode, _
                                  branchCode))

        '返却用のデータテーブル
        Dim dmsDlrBrnTable As ServiceCommonClassDataSet.DmsCodeMapDataTable

        Using svcCommonBiz As New ServiceCommonClassBusinessLogic

            '**************************************************
            '* 基幹販売店コード、店舗コードを取得
            '**************************************************
            dmsDlrBrnTable = svcCommonBiz.GetIcropToDmsCode(dealerCode, _
                                                            ServiceCommonClassBusinessLogic.DmsCodeType.BranchCode, _
                                                            dealerCode, _
                                                            branchCode, _
                                                            String.Empty)

            If dmsDlrBrnTable.Count <= 0 Then

                'データが取得できない場合はエラー
                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                           "{0} {1}.Error ErrorCode:{2}, Failed to convert key dealer code.(No data found)", _
                                           MyClassName, _
                                           MethodBase.GetCurrentMethod.Name, _
                                           ErrorDmsCodeMap))
                dmsDlrBrnTable = Nothing

            ElseIf 1 < dmsDlrBrnTable.Count Then

                'データが2件以上取得できた場合は一意に決定できないためエラー
                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                           "{0} {1}.Error ErrorCode:{2}, Failed to convert key dealer code.(Non-unique)", _
                                           MyClassName, _
                                           MethodBase.GetCurrentMethod.Name, _
                                           ErrorDmsCodeMap))
                dmsDlrBrnTable = Nothing

            End If

        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0} {1}_End", _
                                  MyClassName, _
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

#End Region

#Region "チェック系"

    ''' <summary>
    ''' 引数のチェックを行う
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="roNumInfoTable">RO番号とRO枝番で構成されたテーブル</param>
    ''' <returns>True:チェックOK/False:チェックNG</returns>
    ''' <remarks></remarks>
    Private Function CheckArgument(ByVal dealerCode As String, _
                                   ByVal branchCode As String, _
                                   ByVal roNumInfoTable As IC3802503RONumInfoDataTable) As Boolean

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0} {1}_Start", _
                                  MyClassName, _
                                  MethodBase.GetCurrentMethod.Name))

        '初期値はチェックOK
        Dim checkFlg As Boolean = True

        '販売店コード存在チェック
        If String.IsNullOrWhiteSpace(dealerCode) Then

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                       "{0} {1}.Error ErrorCode:{2}, dealerCode is nothing.", _
                                       MyClassName, _
                                       MethodBase.GetCurrentMethod.Name, _
                                       ErrorArgument))
            checkFlg = False

        End If

        '店舗コード存在チェック
        If String.IsNullOrWhiteSpace(branchCode) Then

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                       "{0} {1}.Error ErrorCode:{2}, branchCode is nothing.", _
                                       MyClassName, _
                                       MethodBase.GetCurrentMethod.Name, _
                                       ErrorArgument))
            checkFlg = False

        End If

        'RO番号情報テーブル存在チェック
        If IsNothing(roNumInfoTable) _
        OrElse roNumInfoTable.Rows.Count = 0 Then
            'RO番号情報テーブル存在しない場合

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                       "{0} {1}.Error ErrorCode:{2}, IC3802503RONumInfoDataTable is nothing or row count is 0.", _
                                       MyClassName, _
                                       MethodBase.GetCurrentMethod.Name, _
                                       ErrorArgument))

            checkFlg = False

        Else
            'RO番号情報テーブル存在する場合

            'RO番号情報テーブルのRO番号存在チェック
            For Each row As IC3802503RONumInfoRow In roNumInfoTable

                If String.IsNullOrWhiteSpace(row.R_O) Then

                    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                               "{0} {1}.Error ErrorCode:{2}, R_O Value is not set.", _
                                               MyClassName, _
                                               MethodBase.GetCurrentMethod.Name, _
                                               ErrorArgument))

                    checkFlg = False

                    Exit For
                End If

            Next

        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0} {1}.End OUT:RESULT={2}", _
                                  MyClassName, _
                                  MethodBase.GetCurrentMethod.Name, _
                                  checkFlg))

        Return checkFlg

    End Function

    ''' <summary>
    ''' 返却XMLのPARTS_STATUSタグ内必須チェックを行う
    ''' </summary>
    ''' <param name="roNo">RO番号</param>
    ''' <param name="roSeqNo">RO枝番</param>
    ''' <param name="partsIssueStatus">部品ステータス</param>
    ''' <returns>チェックOK：True/チェックNG：False</returns>
    ''' <remarks></remarks>
    Private Function CheckMandatoryPartsStatusTag(ByVal roNo As String, _
                                                  ByVal roSeqNo As String, _
                                                  ByVal partsIssueStatus As String) As Boolean

        'Logger.Info(String.Format(CultureInfo.CurrentCulture, _
        '                          "{0}.Start IN:roNo:{1}, roSeqNo:{2}, partsIssueStatus:{3}", _
        '                          MethodBase.GetCurrentMethod.Name, _
        '                          roNo, _
        '                          roSeqNo, _
        '                          partsIssueStatus))

        Dim retCheckOkFlg As Boolean = True

        If String.IsNullOrEmpty(roNo) Then
            'RO番号が存在しないため、エラー
            Logger.Warn(String.Format(CultureInfo.InvariantCulture, _
                                       "{0} {1}.Error ErrorCode:{2}, R_O Value is not set.", _
                                       MyClassName, _
                                       MethodBase.GetCurrentMethod.Name, _
                                       CType(Result.DmsError, String)))
            retCheckOkFlg = False
        End If

        If String.IsNullOrEmpty(roSeqNo) Then
            'RO枝番が存在しないため、エラー
            Logger.Warn(String.Format(CultureInfo.InvariantCulture, _
                                       "{0} {1}.Error ErrorCode:{2}, R_O_SEQNO Value is not set.", _
                                       MyClassName, _
                                       MethodBase.GetCurrentMethod.Name, _
                                       CType(Result.DmsError, String)))
            retCheckOkFlg = False
        End If

        If String.IsNullOrEmpty(partsIssueStatus) Then
            '部品ステータスが存在しないため、エラー
            Logger.Warn(String.Format(CultureInfo.InvariantCulture, _
                                       "{0} {1}.Error ErrorCode:{2}, PARTS_ISSUE_STATUS Value is not set.", _
                                       MyClassName, _
                                       MethodBase.GetCurrentMethod.Name, _
                                       CType(Result.DmsError, String)))
            retCheckOkFlg = False
        End If

        'Logger.Info(String.Format(CultureInfo.CurrentCulture, _
        '                          "{0}.End OUT:retCheckOkFlg:{1}", _
        '                          MethodBase.GetCurrentMethod.Name, retCheckOkFlg))

        Return retCheckOkFlg

    End Function

    ''' <summary>
    ''' 部品の存在有無を判定する
    ''' </summary>
    ''' <param name="partsIssueStatus">部品ステータス</param>
    ''' <param name="nodeBill">BILLノード</param>
    ''' <returns>部品無し：True/部品有り：False</returns>
    ''' <remarks>
    ''' 部品ステータスが8、かつBILLノードがNothingの場合は
    ''' 部品無しと判断してTrueを返却。
    ''' それ以外はFalseを返却。
    ''' </remarks>
    Private Function IsNothingParts(ByVal partsIssueStatus As String, _
                                    ByVal nodeBill As XmlNode) As Boolean

        'Logger.Info(String.Format(CultureInfo.CurrentCulture, _
        '                          "{0}.Start", _
        '                          MethodBase.GetCurrentMethod.Name))

        Dim isNothingFlg As Boolean = False

        If PartsIssueStatusIssued.Equals(partsIssueStatus) Then
            '部品準備ステータスが8(完了)の場合

            If IsNothing(nodeBill) Then
                'BILLノードがない場合

                '部品無しと判断
                isNothingFlg = True

            End If

        End If

        'Logger.Info(String.Format(CultureInfo.CurrentCulture, _
        '                          "{0}.End OUT:isNothingFlg:{1}", _
        '                          MethodBase.GetCurrentMethod.Name, isNothingFlg))

        Return isNothingFlg

    End Function

#End Region

#Region "XML作成"

    ''' <summary>
    ''' 部品ステータス要求用XMLを構築する
    ''' </summary>
    ''' <param name="sysValueRow">システム設定値データ行</param>
    ''' <param name="dmsDlrBrnCodeRow">基幹コードデータ行</param>
    ''' <param name="roNumInfoTable">RO番号情報データテーブル</param>
    ''' <param name="nowDateTime">現在日時</param>
    ''' <returns>構築したXMLドキュメント</returns>
    ''' <remarks></remarks>
    Private Function StructRequestPartsStatusXml(ByVal sysValueRow As IC3802503SystemSettingValueRow, _
                                                 ByVal dmsDlrBrnCodeRow As ServiceCommonClassDataSet.DmsCodeMapRow, _
                                                 ByVal roNumInfoTable As IC3802503RONumInfoDataTable, _
                                                 ByVal nowDateTime As Date) As XmlDocument

        '引数をログに出力
        Dim args As New List(Of String)

        'DataRow内の項目を列挙
        Me.AddLogData(args, sysValueRow)
        Me.AddLogData(args, dmsDlrBrnCodeRow)

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0} {1}.Start IN:{2}", _
                                  MyClassName, _
                                  MethodBase.GetCurrentMethod.Name, _
                                  String.Join(", ", args.ToArray())))

        '65001がUTF-8
        Dim xmlEncode As Encoding = Encoding.GetEncoding(EncodeUtf8)

        'XMLドキュメント作成
        Dim xmlDocument As New XmlDocument

        'ヘッダ部作成(<?xml version="1.0" encoding="utf-8"?>の部分)
        Dim xmlDeclaration As XmlDeclaration = xmlDocument.CreateXmlDeclaration("1.0", xmlEncode.BodyName, Nothing)

        'ルートタグ(Partsタグ)の作成
        Dim xmlRoot As XmlElement = xmlDocument.CreateElement(TagParts)

        'headタグの構築
        Dim headTag As XmlElement = Me.StructHeadTag(xmlDocument, _
                                                     sysValueRow.DIST_CD, _
                                                     nowDateTime)

        'Detailタグの構築
        Dim detailTag As XmlElement = Me.StructDetailTag(xmlDocument, _
                                                         dmsDlrBrnCodeRow.CODE1, _
                                                         dmsDlrBrnCodeRow.CODE2, _
                                                         roNumInfoTable)

        'Partsタグを構築
        xmlRoot.AppendChild(headTag)
        xmlRoot.AppendChild(detailTag)

        '送信用XMLを構築
        xmlDocument.AppendChild(xmlDeclaration)
        xmlDocument.AppendChild(xmlRoot)

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0} {1}.End OUT:STRUCTXML = " & vbCrLf & "{2}", _
                                  MyClassName, _
                                  MethodBase.GetCurrentMethod.Name, _
                                  Me.FormatXml(xmlDocument)))

        Return xmlDocument

    End Function

    ''' <summary>
    ''' 部品ステータス要求用XMLのheadタグを構築する
    ''' </summary>
    ''' <param name="xmlDocument">部品ステータス要求用XMLドキュメント</param>
    ''' <param name="countryCode">国コード</param>
    ''' <param name="nowDateTime">現在日時</param>
    ''' <returns>headタグエレメント</returns>
    ''' <remarks></remarks>
    Private Function StructHeadTag(ByVal xmlDocument As XmlDocument, _
                                   ByVal countryCode As String, _
                                   ByVal nowDateTime As Date) As XmlElement

        'Logger.Info(String.Format(CultureInfo.InvariantCulture, _
        '                          "{0}_Start IN:countryCode={1}, nowDateTime={2}", _
        '                          MethodBase.GetCurrentMethod.Name, _
        '                          countryCode, _
        '                          nowDateTime))

        'headタグを作成
        Dim headTag As XmlElement = xmlDocument.CreateElement(TagHead)

        'headタグの子要素を作成
        Dim messageIdTag As XmlElement = xmlDocument.CreateElement(TagMessageID)
        Dim countryCodeTag As XmlElement = xmlDocument.CreateElement(TagCountryCode)
        Dim linkSystemCodeTag As XmlElement = xmlDocument.CreateElement(TagLinkSystemCode)
        Dim TransmissionDateTag As XmlElement = xmlDocument.CreateElement(TagTransmissionDate)

        '子要素に値を設定
        messageIdTag.AppendChild(xmlDocument.CreateTextNode(RequestPartsStatusId))
        countryCodeTag.AppendChild(xmlDocument.CreateTextNode(countryCode))
        linkSystemCodeTag.AppendChild(xmlDocument.CreateTextNode("0"))
        'TransmissionDateTag.AppendChild(xmlDocument.CreateTextNode(nowDateTime.ToString(yyyyMMddHHmmssDateFormat, CultureInfo.CurrentCulture)))
        TransmissionDateTag.AppendChild(xmlDocument.CreateTextNode(nowDateTime.ToString(ddMMyyyyHHmmssDateFormat, CultureInfo.CurrentCulture)))

        'headタグを構築
        With headTag
            .AppendChild(messageIdTag)
            .AppendChild(countryCodeTag)
            .AppendChild(linkSystemCodeTag)
            .AppendChild(TransmissionDateTag)
        End With

        'Logger.Info(String.Format(CultureInfo.InvariantCulture, _
        '                          "{0}_End OUT:headTag={1}", _
        '                          MethodBase.GetCurrentMethod.Name, _
        '                          headTag.InnerXml))

        Return headTag

    End Function

    ''' <summary>
    ''' 部品ステータス要求用XMLのDetailタグを構築する
    ''' </summary>
    ''' <param name="xmlDocument">部品ステータス要求用XMLドキュメント</param>
    ''' <param name="dmsDealerCode">基幹販売店コード</param>
    ''' <param name="dmsBranchCode">基幹店舗コード</param>
    ''' <param name="roNumInfoTable">RO番号情報データテーブル</param>
    ''' <returns>Detailタグエレメント</returns>
    ''' <remarks></remarks>
    Private Function StructDetailTag(ByVal xmlDocument As XmlDocument, _
                                     ByVal dmsDealerCode As String, _
                                     ByVal dmsBranchCode As String, _
                                     ByVal roNumInfoTable As IC3802503RONumInfoDataTable) As XmlElement

        'Logger.Info(String.Format(CultureInfo.InvariantCulture, _
        '                          "{0}_Start IN:dmsDealerCode={1}, dmsBranchCode={2}", _
        '                          MethodBase.GetCurrentMethod.Name, _
        '                          dmsDealerCode, _
        '                          dmsBranchCode))

        'Detailタグを作成
        Dim detailTag As XmlElement = xmlDocument.CreateElement(TagDetail)

        'Commonタグを構築
        Dim commonTag As XmlElement = Me.StructCommonTag(xmlDocument, _
                                                         dmsDealerCode, _
                                                         dmsBranchCode)

        'DetailタグにCommonタグを子要素として追加
        detailTag.AppendChild(commonTag)

        For Each row As IC3802503RONumInfoRow In roNumInfoTable
            'PartsSearchConditionタグを構築
            Dim partsSearchConditionTag As XmlElement = Me.StructPartsSearchConditionTag(xmlDocument, _
                                                                                         row)

            'DetailタグにPartsSearchConditionタグを子要素として追加
            detailTag.AppendChild(partsSearchConditionTag)
        Next

        'Logger.Info(String.Format(CultureInfo.InvariantCulture, _
        '                          "{0}_End OUT:detailTag={1}", _
        '                          MethodBase.GetCurrentMethod.Name, _
        '                          detailTag.InnerXml))

        Return detailTag

    End Function

    ''' <summary>
    ''' 部品ステータス要求用XMLのCommonタグを構築する
    ''' </summary>
    ''' <param name="xmlDocument">部品ステータス要求用XMLドキュメント</param>
    ''' <param name="dmsDealerCode">基幹販売店コード</param>
    ''' <param name="dmsBranchCode">基幹店舗コード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function StructCommonTag(ByVal xmlDocument As XmlDocument, _
                                     ByVal dmsDealerCode As String, _
                                     ByVal dmsBranchCode As String) As XmlElement

        'Logger.Info(String.Format(CultureInfo.InvariantCulture, _
        '                          "{0}_Start IN:dmsDealerCode={1}, dmsBranchCode={2}", _
        '                          MethodBase.GetCurrentMethod.Name, _
        '                          dmsDealerCode, _
        '                          dmsBranchCode))

        'Commonタグを作成
        Dim commonTag As XmlElement = xmlDocument.CreateElement(TagCommon)

        'Commonタグの子要素を作成
        Dim dealerCodeTag As XmlElement = xmlDocument.CreateElement(TagDealerCode)
        Dim branchCodeTag As XmlElement = xmlDocument.CreateElement(TagBranchCode)

        '子要素に値を設定
        dealerCodeTag.AppendChild(xmlDocument.CreateTextNode(dmsDealerCode))
        branchCodeTag.AppendChild(xmlDocument.CreateTextNode(dmsBranchCode))

        'Commonタグの子要素を追加
        With commonTag
            .AppendChild(dealerCodeTag)
            .AppendChild(branchCodeTag)
        End With

        'Logger.Info(String.Format(CultureInfo.InvariantCulture, _
        '                          "{0}_End OUT:commonTag={1}", _
        '                          MethodBase.GetCurrentMethod.Name, commonTag.InnerXml))

        Return commonTag

    End Function

    ''' <summary>
    ''' 部品ステータス要求用XMLのPartsSearchConditionタグを構築する
    ''' </summary>
    ''' <param name="xmlDocument">部品ステータス要求用XMLドキュメント</param>
    ''' <param name="roNumInfoRow">RO番号情報データ行</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function StructPartsSearchConditionTag(ByVal xmlDocument As XmlDocument, _
                                                   ByVal roNumInfoRow As IC3802503RONumInfoRow) As XmlElement

        'Logger.Info(String.Format(CultureInfo.InvariantCulture, _
        '                          "{0}_Start ", _
        '                          MethodBase.GetCurrentMethod.Name))

        'PartsSearchConditionタグを作成
        Dim partsSearchConditionTag As XmlElement = xmlDocument.CreateElement(TagPartsSearchCondition)


        'PartsSearchConditionタグの子要素を作成
        Dim roTag As XmlElement = xmlDocument.CreateElement(TagRO)
        Dim roSeqNoTag As XmlElement = xmlDocument.CreateElement(TagROSEQNO)

        '子要素に値を設定
        'R_O
        roTag.AppendChild(xmlDocument.CreateTextNode(roNumInfoRow.R_O))

        'R_O_SEQNO
        If roNumInfoRow.IsR_O_SEQNONull _
        OrElse String.IsNullOrEmpty(roNumInfoRow.R_O_SEQNO) Then
            '空文字を設定
            roSeqNoTag.AppendChild(xmlDocument.CreateTextNode(String.Empty))
        Else
            roSeqNoTag.AppendChild(xmlDocument.CreateTextNode(roNumInfoRow.R_O_SEQNO))
        End If

        'PartsSearchConditionタグの子要素を追加
        With partsSearchConditionTag
            .AppendChild(roTag)
            .AppendChild(roSeqNoTag)
        End With

        'Logger.Info(String.Format(CultureInfo.InvariantCulture, _
        '                          "{0}_End OUT:partsSearchConditionTag={1}", _
        '                          MethodBase.GetCurrentMethod.Name, _
        '                          partsSearchConditionTag.InnerXml))

        Return partsSearchConditionTag

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
                                  "{0} {1}.Start IN:sendXml={2}, webServiceUrl={3} ", _
                                  MyClassName, _
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

        With req
            req.Method = Post                           'メソッドにPOSTを指定
            req.ContentType = ContentTypeString         'ContentType指定(固定)
            req.ContentLength = postDataBytes.Length    'POST送信するデータの長さを指定
            req.Timeout = CType(timeOutValue, Integer)  '送信タイムアウト値設定
        End With

        'データをPOST送信するためのStreamを取得
        Using reqStream As Stream = req.GetRequestStream()

            '送信するデータを書き込む
            reqStream.Write(postDataBytes, 0, postDataBytes.Length)

        End Using

        '応答XML文字列
        Dim responseString As String = String.Empty

        '返却文字列(応答XML文字列をHTMLデコード)
        Dim retString As String = String.Empty

        Try
            'サーバーからの応答を受信するためのWebResponseを取得
            Dim resultResponse As WebResponse = req.GetResponse()

            '応答データを受信するためのStreamを取得
            Dim resultStream As Stream = resultResponse.GetResponseStream()

            '受信して表示
            Using resultReader As New StreamReader(resultStream, enc)

                '応答XML文字列を取得
                responseString = resultReader.ReadToEnd()

            End Using

            '2020/01/29 NSK 今泉 TR-SVT-TKM-20191030-001 新車が販売店に来店した際にVIN番号を入力しても検索エラーが表示される START
            ''応答XML文字列をHTMLデコードする
            'retString = HttpUtility.HtmlDecode(responseString)
            'responseのXMLをデコードしないように修正
            retString = responseString
            '2020/01/29 NSK 今泉 TR-SVT-TKM-20191030-001 新車が販売店に来店した際にVIN番号を入力しても検索エラーが表示される END

        Catch wex As WebException

            If wex.Status = WebExceptionStatus.Timeout Then
                'タイムアウトが発生した場合
                Logger.Error(String.Format(CultureInfo.InvariantCulture, _
                                           "{0} {1}_Error ErrorCode:{2}, Timeout error occurred.", _
                                           MyClassName, _
                                           MethodBase.GetCurrentMethod.Name, _
                                           CType(Result.TimeOutError, String)), wex)

                retString = CType(Result.timeOutError, String)
            Else
                'それ以外のネットワークエラー
                Logger.Error(String.Format(CultureInfo.InvariantCulture, _
                                           "{0} {1}_Error ErrorCode:{2}", _
                                           MyClassName, _
                                           MethodBase.GetCurrentMethod.Name, _
                                           CType(Result.OtherError, String)), wex)

                retString = CType(Result.otherError, String)
            End If

        End Try

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0} {1}.End OUT:retString={2} ", _
                                  MyClassName, _
                                  MethodBase.GetCurrentMethod.Name, _
                                  retString))

        '返却文字列を返却
        Return retString

    End Function

#End Region

#Region "XML解析"

    ''' <summary>
    ''' 返却用の部品ステータスDataTableを作成する
    ''' </summary>
    ''' <param name="resultXml">受信XML文字列</param>
    ''' <returns>部品ステータスDataTable</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2015/06/05 TMEJ 小澤 TMT号口調査 XML解析中にエラーが発生した場合はログを出力するように修正
    ''' </history>
    Private Function CreatePartsStatusTable(ByVal resultXml As String) As IC3802503PartsStatusDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0} {1}.Start IN:resultXml={2} ", _
                                  MyClassName, _
                                  MethodBase.GetCurrentMethod.Name, _
                                  resultXml))

        '返却用DataTableのインスタンス生成
        Dim dt As New IC3802503PartsStatusDataTable

        Try

            'XmlDocument
            Dim resultXmlDocument As New XmlDocument

            '返却された文字列をXML化
            resultXmlDocument.LoadXml(resultXml)

            'Parts_Resultノードを取得
            Dim partsResultNode As XmlNode _
                = resultXmlDocument.SelectSingleNode(XmlRootDirectory & TagPartsResult)

            'ResultCodeの取得
            Dim resultCodeDictionary As Dictionary(Of String, String) _
                    = Me.GetElementsData(partsResultNode, {TagResultCode})

            Dim resultCode As String = resultCodeDictionary.Item(TagResultCode)

            'ResultCodeの値が0以外の場合
            If Not "0".Equals(resultCode) Then
                Logger.Warn(String.Format(CultureInfo.InvariantCulture, _
                                           "{0} {1}.Error ErrorCode:{2}, ResultCode of received xml is {3}.", _
                                           MyClassName, _
                                           MethodBase.GetCurrentMethod.Name, _
                                           CType(Result.DmsError, String), _
                                           resultCode))
                
                '2015/06/05 TMEJ 小澤 TMT号口調査 XML解析中にエラーが発生した場合はログを出力するように修正 START

                ''受信XMLのログ出力
                'Logger.Error(String.Format(CultureInfo.InvariantCulture, _
                '                           "{0}.{1}_Error ReceivedXML = {2}", _
                '                           Me.GetType.ToString(), _
                '                           MethodBase.GetCurrentMethod.Name, _
                '                           resultXml))

                '2015/06/05 TMEJ 小澤 TMT号口調査 XML解析中にエラーが発生した場合はログを出力するように修正 START

                '基幹側のエラーでエラーテーブル作成
                dt = Me.CreateErrorTable(Result.dmsError)

            Else

                'PARTS_STATUSタグのリストを取り出す
                Dim partsStatusNodeList As XmlNodeList _
                    = resultXmlDocument.SelectNodes(XmlRootDirectory & TagPartsStatus)

                If 0 < partsStatusNodeList.Count Then
                    'PARTS_STATUSタグがある場合

                    'PARTS_STATUSタグ分繰り返し
                    For Each partsStatusNode As XmlNode In partsStatusNodeList

                        'PARTS_STATUSタグの子要素設定値を取得する
                        '※但し、BILLノードは不要なため設定値の取得は行わないが、
                        '　「PARTS_ISSUE_STATUSが8、かつBILLノードがない」場合は
                        '　部品無しと判断するため、存在確認のみ行う
                        Dim partsStatusInfoDictinary As Dictionary(Of String, String) _
                            = Me.GetElementsData(partsStatusNode, {TagRO, TagROSEQNO, _
                                                                   TagPartsIssueStatus, TagRequestedStaffId})

                        Dim roNo As String = partsStatusInfoDictinary.Item(TagRO)
                        Dim roSeq As String = partsStatusInfoDictinary.Item(TagROSEQNO)
                        Dim partsIssueStatus As String = partsStatusInfoDictinary.Item(TagPartsIssueStatus)
                        Dim requestedStaffId As String = partsStatusInfoDictinary.Item(TagRequestedStaffId)

                        If Me.CheckMandatoryPartsStatusTag(roNo, roSeq, partsIssueStatus) Then
                            '必須チェックがOKの場合

                            If Not Me.IsNothingParts(partsIssueStatus, _
                                                     partsStatusNode.SelectSingleNode(TagBILL)) Then
                                '部品有りと判断した場合

                                '部品ステータスデータ行インスタンス作成
                                Dim partsStatusRow As IC3802503PartsStatusRow = dt.NewIC3802503PartsStatusRow

                                '部品ステータスデータ行に設定
                                With partsStatusRow
                                    .ResultCode = Result.Success            'ResultCode
                                    .R_O = roNo                             'R_O
                                    .R_O_SEQNO = roSeq                      'R_O_SEQ
                                    .PARTS_ISSUE_STATUS = partsIssueStatus  'PARTS_ISSUE_STATUS
                                    .RequestedStaffID = requestedStaffId    'RequestedStaffID
                                End With

                                '返却用DataTableにデータ行を追加
                                dt.AddIC3802503PartsStatusRow(partsStatusRow)

                            End If

                        Else
                            '必須チェックがNGの場合

                            '基幹側のエラーでエラーテーブル作成
                            dt = Me.CreateErrorTable(Result.DmsError)
                            Exit For

                        End If

                    Next

                    If dt.Count = 0 Then
                        'PARTS_STATUSタグがあったが、全て部品無しと判断された場合はNothingを返却
                        dt.Dispose()
                        dt = Nothing

                    End If

                Else
                    'PARTS_STATUSタグがない場合はNothingを返却
                    dt.Dispose()
                    dt = Nothing

                End If

            End If

            If Not IsNothing(dt) AndAlso 0 < dt.Rows.Count Then
                '作成した部品ステータステーブルが1行でもあればログに内容出力
                Me.OutPutDataTableLog(dt, MethodBase.GetCurrentMethod.Name)
            End If

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0} {1}_End", _
                                      MyClassName, _
                                      MethodBase.GetCurrentMethod.Name))

            Return dt

        Catch wex As XmlException

            '受信XMLのログ出力
            Logger.Error(String.Format(CultureInfo.InvariantCulture, _
                                       "{0}.{1}_Error ReceivedXML = {2}", _
                                       Me.GetType.ToString(), _
                                       MethodBase.GetCurrentMethod.Name, _
                                       resultXml), wex)

            'その他のエラーでエラーテーブル作成
            dt = Me.CreateErrorTable(Result.OtherError)

            Return dt

        Finally

            If Not IsNothing(dt) Then
                dt.Dispose()
                dt = Nothing
            End If

        End Try

    End Function

#End Region

#Region "エラーテーブル作成"

    ''' <summary>
    ''' エラー発生時用戻り値DataTableを作成する
    ''' </summary>
    ''' <param name="resultCode">結果コード</param>
    ''' <returns>IC3802503PartsStatusDataTable</returns>
    ''' <remarks>
    ''' 作成されるデータ行は1行のみ。
    ''' 作成されたデータ行は、ResultCodeカラムのみデータが入っている。
    ''' ResultCodeカラムに設定される値はエラーコード。
    ''' </remarks>
    Private Function CreateErrorTable(ByVal resultCode As Long) As IC3802503PartsStatusDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0} {1}.Start IN:resultCode={2}", _
                                  MyClassName, _
                                  MethodBase.GetCurrentMethod.Name, _
                                  resultCode))

        Using dt As New IC3802503PartsStatusDataTable

            Dim dr As IC3802503PartsStatusRow = dt.NewIC3802503PartsStatusRow

            dr.ResultCode = resultCode

            dt.AddIC3802503PartsStatusRow(dr)

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0} {1}_End", _
                                      MyClassName, _
                                      MethodBase.GetCurrentMethod.Name))

            Return dt

        End Using


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

        Logger.Info(MyClassName & Space(1) & methodName & _
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

        Logger.Info(MyClassName & Space(1) & methodName & " LOG END ")

    End Sub

    ''' <summary>
    ''' XMLをインデントを付加して整形する(ログ出力用)
    ''' </summary>
    ''' <param name="xmlDoc">XMLドキュメント</param>
    ''' <returns>整形後XML文字列</returns>
    ''' <remarks></remarks>
    Private Function FormatXml(ByVal xmlDoc As XmlDocument) As String

        'Logger.Info(String.Format(CultureInfo.InvariantCulture, _
        '                          "{0}_Start", _
        '                          MethodBase.GetCurrentMethod.Name))

        Using textWriter As New StringWriter(CultureInfo.InvariantCulture)

            Dim xmlWriter As XmlTextWriter

            Try
                xmlWriter = New XmlTextWriter(textWriter)

                'インデントを2でフォーマット
                xmlWriter.Formatting = Formatting.Indented
                xmlWriter.Indentation = 2

                'XmlTextWriterにXMLを出力
                xmlDoc.WriteTo(xmlWriter)

                'Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                '                          "{0}_End", _
                '                          MethodBase.GetCurrentMethod.Name))

                Return textWriter.ToString()

            Finally
                xmlWriter = Nothing
            End Try

        End Using

    End Function

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
