'-------------------------------------------------------------------------
'IC3800903BusinessLogic.vb
'-------------------------------------------------------------------------
'機能：予約情報送信(ビジネスロジック)
'補足：
'作成：2013/11/21 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発
'更新：2014/04/23 TMEJ 明瀬 サービスタブレットDMS連携コード変換対応
'更新：2015/04/23 TMEJ 明瀬 DMS連携版サービスタブレット強制納車機能追加開発
'更新：2015/07/17 TMEJ 明瀬 タブレットSMB性能改善 ログ出力強化対応
'更新：2015/10/08 TM 皆川 タブレットSMBのチップ移動時の連携送信メッセージ詳細化
'更新：2016/08/24 NSK 竹中　TR-SVT-TMT-20150817-001 予約詳細をTopservで確認するとき、どうしてチップにRO Noが無いのでしょうか。
'更新：
'─────────────────────────────────────

Imports System.Xml
Imports System.Net
Imports System.Web
Imports System.IO
Imports System.Text
Imports System.Globalization
Imports System.Reflection
Imports System.Xml.Serialization
Imports System.Text.RegularExpressions
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.DMSLinkage.Reserve.Api.BizLogic.IC46201CN
Imports Toyota.eCRB.DMSLinkage.Reserve.Api.DataAccess
Imports Toyota.eCRB.DMSLinkage.Reserve.Api.DataAccess.IC3800903DataSet
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.BizLogic
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.DataAccess

Public Class IC3800903BusinessLogic
    Inherits BaseBusinessComponent
    Implements IDisposable

    '2015/07/17 TMEJ 明瀬 タブレットSMB性能改善 ログ出力強化対応 START

    ''' <summary>
    ''' 受付チップ配置フラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private ReceptonChipPlacementFlg As Boolean = False

    '2015/07/17 TMEJ 明瀬 タブレットSMB性能改善 ログ出力強化対応 END

#Region "Publicメソッド"

    '2015/07/17 TMEJ 明瀬 タブレットSMB性能改善 ログ出力強化対応 START

    ' ''' <summary>
    ' ''' 基幹連携(予約送信処理)を行う(メイン)
    ' ''' </summary>
    ' ''' <param name="inSvcInId">サービス入庫ID</param>
    ' ''' <param name="inJobDtlId">作業内容ID</param>
    ' ''' <param name="inStallUseId">ストール利用ID</param>
    ' ''' <param name="inPrevStatus">変更前チップステータス(基幹連携独自の定義)</param>
    ' ''' <param name="inCrntStatus">変更後チップステータス(基幹連携独自の定義)</param>
    ' ''' <param name="inPrevResvStatus">変更前予約ステータス</param>
    ' ''' <param name="inFunctionId">機能ID</param>
    ' ''' <param name="inPrevCancelJobDtlIdList">更新前からキャンセル状態の作業内容IDリスト(規定値はNothing)</param>
    ' ''' <param name="inCallFromChipDetailFlg">チップ詳細から呼出フラグ(規定値はFalse(チップ詳細以外から呼出))</param>
    ' ''' <returns>予約送信処理結果コード(0:正常終了/9999:異常終了)</returns>
    ' ''' <remarks></remarks>
    'Public Function SendReserveInfo(ByVal inSvcInId As Decimal, _
    '                                ByVal inJobDtlId As Decimal, _
    '                                ByVal inStallUseId As Decimal, _
    '                                ByVal inPrevStatus As String, _
    '                                ByVal inCrntStatus As String, _
    '                                ByVal inPrevResvStatus As String, _
    '                                ByVal inFunctionId As String, _
    '                                Optional ByVal inPrevCancelJobDtlIdList As List(Of Decimal) = Nothing, _
    '                                Optional ByVal inCallFromChipDetailFlg As Boolean = False) As Integer

    ''' <summary>
    ''' 基幹連携(予約送信処理)を行う(メイン)
    ''' </summary>
    ''' <param name="inSvcInId">サービス入庫ID</param>
    ''' <param name="inJobDtlId">作業内容ID</param>
    ''' <param name="inStallUseId">ストール利用ID</param>
    ''' <param name="inPrevStatus">変更前チップステータス(基幹連携独自の定義)</param>
    ''' <param name="inCrntStatus">変更後チップステータス(基幹連携独自の定義)</param>
    ''' <param name="inPrevResvStatus">変更前予約ステータス</param>
    ''' <param name="inFunctionId">機能ID</param>
    ''' <param name="inPrevCancelJobDtlIdList">更新前からキャンセル状態の作業内容IDリスト(規定値はNothing)</param>
    ''' <param name="inCallFromChipDetailFlg">チップ詳細から呼出フラグ(規定値はFalse(チップ詳細以外から呼出))</param>
    ''' <param name="inReceptonChipPlacementFlg">受付チップ配置で呼出フラグ(規定値はFalse)</param>
    ''' <returns>予約送信処理結果コード(0:正常終了/9999:異常終了)</returns>
    ''' <remarks></remarks>
    Public Function SendReserveInfo(ByVal inSvcInId As Decimal, _
                                    ByVal inJobDtlId As Decimal, _
                                    ByVal inStallUseId As Decimal, _
                                    ByVal inPrevStatus As String, _
                                    ByVal inCrntStatus As String, _
                                    ByVal inPrevResvStatus As String, _
                                    ByVal inFunctionId As String, _
                                    Optional ByVal inPrevCancelJobDtlIdList As List(Of Decimal) = Nothing, _
                                    Optional ByVal inCallFromChipDetailFlg As Boolean = False, _
                                    Optional ByVal inReceptonChipPlacementFlg As Boolean = False) As Integer

        ReceptonChipPlacementFlg = inReceptonChipPlacementFlg

        '2015/07/17 TMEJ 明瀬 タブレットSMB性能改善 ログ出力強化対応 END

        'ログ用にリストを文字列加工
        Dim sbPrevCancelJobDtlId As New StringBuilder

        If Not IsNothing(inPrevCancelJobDtlIdList) Then
            For Each id In inPrevCancelJobDtlIdList
                sbPrevCancelJobDtlId.Append(id.ToString(CultureInfo.CurrentCulture))
                sbPrevCancelJobDtlId.Append(Space(1))
            Next
        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.S IN:inSvcInId={1}, inJobDtlId={2}, inStallUseId={3}, inPrevStatus={4}, inCrntStatus={5}, inPrevResvStatus={6}, inFunctionId={7}, inPrevCancelJobDtlIdList={8}, inCallFromChipDetailFlg={9} ", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  inSvcInId, _
                                  inJobDtlId, _
                                  inStallUseId, _
                                  inPrevStatus, _
                                  inCrntStatus, _
                                  inPrevResvStatus, _
                                  inFunctionId, _
                                  sbPrevCancelJobDtlId.ToString(), _
                                  inCallFromChipDetailFlg))
        '戻り値
        Dim retValue As Integer = Success

        Try

            '予約送信するかしないかのフラグを取得
            Dim sendFlg As String = Me.GetSendReserveFlg(inPrevStatus, _
                                                         inCrntStatus, _
                                                         inSvcInId, _
                                                         inJobDtlId, _
                                                         inPrevResvStatus)

            If SendReserve.Equals(sendFlg) Then
                '送信する
                retValue = Me.SendReserveInfoDetails(inSvcInId, _
                                                     inJobDtlId, _
                                                     inCrntStatus, _
                                                     inPrevResvStatus, _
                                                     inFunctionId, _
                                                     inPrevCancelJobDtlIdList, _
                                                     inCallFromChipDetailFlg)
            Else
                '送信しない(正常終了)
                retValue = Success
            End If

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}.E OUT:retValue={1}", _
                                      MethodBase.GetCurrentMethod.Name, _
                                      retValue))

        Catch oex As OracleExceptionEx

            'DBエラー
            Logger.Error(String.Format(CultureInfo.InvariantCulture, _
                                       "{0}.Error ErrorCode:{1}", _
                                       MethodBase.GetCurrentMethod.Name, _
                                       ErrorDBConnect), oex)
            retValue = ErrorDBConnect

        Catch ex As Exception

            '異常終了
            Logger.Error(String.Format(CultureInfo.InvariantCulture, _
                                       "{0}.Error ErrorCode:{1}", _
                                       MethodBase.GetCurrentMethod.Name, _
                                       ErrorSystem), ex)
            retValue = ErrorSystem

        End Try

        Return retValue

    End Function

#End Region

#Region "Privateメソッド"

    ''' <summary>
    ''' 予約送信を実行する
    ''' </summary>
    ''' <param name="inSvcInId">サービス入庫ID</param>
    ''' <param name="inJobDtlId">作業内容ID</param>
    ''' <param name="inCrntStatus">変更後チップステータス(基幹連携独自の定義)</param>
    ''' <param name="inPrevResvStatus">変更前予約ステータス</param>
    ''' <param name="inFunctionId">機能ID</param>
    ''' <param name="inPrevCancelJobDtlIdList">更新前からキャンセル状態の作業内容IDリスト</param>
    ''' <param name="inCallFromChipDetailFlg">チップ詳細から呼出フラグ</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SendReserveInfoDetails(ByVal inSvcInId As Decimal, _
                                            ByVal inJobDtlId As Decimal, _
                                            ByVal inCrntStatus As String, _
                                            ByVal inPrevResvStatus As String, _
                                            ByVal inFunctionId As String, _
                                            ByVal inPrevCancelJobDtlIdList As List(Of Decimal), _
                                            ByVal inCallFromChipDetailFlg As Boolean) As Integer

        'Logger.Info(String.Format(CultureInfo.InvariantCulture, _
        '                          "{0}_S IN:inSvcInId={1}, inJobDtlId={2}, inStallUseId={3}, inPrevStatus={4}, inCrntStatus={5}, inPrevResvStatus={6}, inFunctionId={7}, inCallFromChipDetailFlg={8}", _
        '                          MethodBase.GetCurrentMethod.Name, _
        '                          inSvcInId, _
        '                          inJobDtlId, _
        '                          inStallUseId, _
        '                          inPrevStatus, _
        '                          inCrntStatus, _
        '                          inPrevResvStatus, _
        '                          inFunctionId, _
        '                          inCallFromChipDetailFlg))

        '戻り値を初期化(正常終了)
        Dim retValue As Integer = Success

        '2015/04/23 TMEJ 明瀬 DMS連携版サービスタブレット強制納車機能追加開発 START

        'DMS除外エラーの警告フラグ(True：除外対象あり / False：除外対象のエラーなし、またはエラー自体なし)
        Dim warningOmitDmsErrorFlg As Boolean = False

        '2015/04/23 TMEJ 明瀬 DMS連携版サービスタブレット強制納車機能追加開発 END

        Try
            'ログインスタッフ情報取得
            Dim userContext As StaffContext = StaffContext.Current

            '現在日時取得
            Dim nowDateTime As Date = DateTimeFunc.Now(userContext.DlrCD)

            '**************************************************
            '* システム設定値を取得
            '**************************************************
            Dim systemSettingsValueRow As IC3800903SystemSettingValueRow _
                = Me.GetSystemSettingValues()

            '必要なシステム設定値が一つでも取得できない場合はエラー
            If IsNothing(systemSettingsValueRow) Then

                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                           "{0}.Error Not setting value is present in the system settings.", _
                                           MethodBase.GetCurrentMethod.Name))
                retValue = ErrorSystem
                Exit Try

            End If

            Dim dmsDlrBrnTable As ServiceCommonClassDataSet.DmsCodeMapDataTable = Nothing

            Using svcCommonBiz As New ServiceCommonClassBusinessLogic

                '**************************************************
                '* 基幹販売店コード、店舗コードを取得
                '**************************************************
                dmsDlrBrnTable = svcCommonBiz.GetIcropToDmsCode(userContext.DlrCD, _
                                                                ServiceCommonClassBusinessLogic.DmsCodeType.BranchCode, _
                                                                userContext.DlrCD, _
                                                                userContext.BrnCD, _
                                                                String.Empty)

                If dmsDlrBrnTable.Count <= 0 Then

                    'データが取得できない場合はエラー
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                               "{0}.Error ErrCode:{1}, Failed to convert key dealer code.(No data found)", _
                                               MethodBase.GetCurrentMethod.Name, _
                                               ErrorDmsCodeMap))
                    retValue = ErrorSystem
                    Exit Try

                ElseIf 1 < dmsDlrBrnTable.Count Then

                    'データが2件以上取得できた場合は一意に決定できないためエラー
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                               "{0}.Error ErrCode:{1}, Failed to convert key dealer code.(Non-unique)", _
                                               MethodBase.GetCurrentMethod.Name, _
                                               ErrorDmsCodeMap))
                    retValue = ErrorSystem
                    Exit Try

                End If

            End Using

            Dim cstVclInfoTable As IC3800903DmsSendCstVclInfoDataTable = Nothing

            '自分のテーブルアダプタークラスインスタンスを生成
            Using myTableAdapter As New IC3800903TableAdapter

                '**************************************************
                '* 顧客、車両、販売店車両、販売店顧客車両情報を取得
                '**************************************************
                cstVclInfoTable = myTableAdapter.GetSendDmsCstVclInfo(inSvcInId, _
                                                                      userContext.DlrCD)

                '取得件数が0件の場合はエラー
                If cstVclInfoTable.Count <= 0 Then
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                               "{0}.Error ErrCode:{1}, Failed to get a customer, vehicle information.", _
                                               MethodBase.GetCurrentMethod.Name, _
                                               ErrorCust))
                    retValue = ErrorSystem
                    Exit Try
                End If

            End Using

            '上記処理で取得したデータテーブルからデータ行抜き出し
            Dim dmsDlrBrnRow As ServiceCommonClassDataSet.DmsCodeMapRow = dmsDlrBrnTable.Item(0)
            Dim cstVclInfoRow As IC3800903DmsSendCstVclInfoRow = cstVclInfoTable.Item(0)

            '**************************************************
            '* 送信対象のチップ情報を全て取得
            '**************************************************
            Dim sendTargetChipInfoRow As IC3800903DmsSendReserveInfoRow() _
                = Me.GetSendTargetChipInfo(inSvcInId, _
                                           inJobDtlId, _
                                           inPrevResvStatus, _
                                           inPrevCancelJobDtlIdList, _
                                           systemSettingsValueRow.SEND_RELATION_STATUS, _
                                           inCallFromChipDetailFlg)

            '送信対象のチップ件数分の予約送信を行う
            For Each sendTargetChip In sendTargetChipInfoRow

                'ストールID(xml設定用)
                Dim dmsStallId As String = Me.GetDmsStallId(userContext.DlrCD, _
                                                            userContext.BrnCD, _
                                                            sendTargetChip.STALL_ID.ToString(CultureInfo.CurrentCulture))

                '基幹ストールIDへの変換に失敗した場合
                If String.IsNullOrEmpty(dmsStallId) Then
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                               "{0}.Error ErrCode:{1}, Failed to convert key stallId.", _
                                               MethodBase.GetCurrentMethod.Name, _
                                               ErrorDmsCodeMap))
                    retValue = ErrorSystem
                    Exit Try
                End If

                'ストールIDを送信不可ならストールID(xml設定用)は空文字にする
                If Not Me.CanSendStallId(sendTargetChip, _
                                         systemSettingsValueRow.SUBAREA_STALL_ID_SEND_FLG, _
                                         inCrntStatus) Then
                    dmsStallId = String.Empty
                End If

                '管理作業内容ID(関連チップ内の作業内容IDの最小値)を取得する
                Dim PRezId As String = Me.GetManageJobDtlId(inSvcInId, _
                                                            sendTargetChip.JOB_DTL_ID, _
                                                            systemSettingsValueRow.MNG_JOB_DTL_ID_SEND_FLG, _
                                                            inPrevCancelJobDtlIdList)

                '子予約連番を取得する
                Dim PRezChildNo As String = Me.GetRezChildNo(inSvcInId, _
                                                             sendTargetChip.JOB_DTL_ID, _
                                                             PRezId, _
                                                             inPrevCancelJobDtlIdList)

                'キャンセルフラグの設定値を取得する(既にキャンセルフラグが立っている場合は1:キャンセルを設定)
                Dim cancelFlgValue As String = Me.GetCancelFlgSettingValue(sendTargetChip.CANCEL_FLG, _
                                                                           sendTargetChip.RESV_STATUS, _
                                                                           inPrevResvStatus, _
                                                                           systemSettingsValueRow.TENTATIVE_UPDATE_CANCEL_FLG)

                Dim otherInfoRow As IC3800903DmsSendOtherInfoRow

                'データ行に入っていない情報をまとめる
                Using otherInfoTable As New IC3800903DmsSendOtherInfoDataTable

                    otherInfoRow = otherInfoTable.NewIC3800903DmsSendOtherInfoRow

                    With otherInfoRow
                        .DMS_DLR_CD = dmsDlrBrnRow.CODE1        '基幹販売店コード
                        .DMS_BRN_CD = dmsDlrBrnRow.CODE2        '基幹店舗コード
                        .DMS_STALLID = dmsStallId               '基幹ストールID
                        .PREZID = PRezId                        '管理作業内容ID
                        .REZ_CHILDNO = PRezChildNo              '子予約連番
                        .CANCELFLGVALUE = cancelFlgValue        'キャンセルフラグ
                    End With

                End Using

                '**************************************************
                '* 送信XMLの作成
                '**************************************************
                Dim sendXml As XmlDocument = Me.StructSendReserveXml(systemSettingsValueRow, _
                                                                     cstVclInfoRow, _
                                                                     sendTargetChip, _
                                                                     otherInfoRow, _
                                                                     nowDateTime)
                If IsNothing(sendXml) Then
                    '送信XMLの構築に失敗
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                               "{0}.Error Err:Failed to build XML for transmission.", _
                                               MethodBase.GetCurrentMethod.Name))
                    retValue = ErrorSystem
                    Exit Try
                End If

                '**************************************************
                '* XMLの送受信処理
                '**************************************************
                Dim resultString As String = Me.ExecuteSendReserveXml(sendXml.InnerXml, _
                                                                      systemSettingsValueRow.LINK_URL_RESV_INFO, _
                                                                      systemSettingsValueRow.LINK_SEND_TIMEOUT_VAL, _
                                                                      systemSettingsValueRow.LINK_SOAP_VERSION, _
                                                                      systemSettingsValueRow.DIST_CD)

                '結果文字列が空文字の場合、エラーとする(送受信処理でエラー発生)
                If String.IsNullOrEmpty(resultString) Then
                    Logger.Warn(String.Format(CultureInfo.InvariantCulture, _
                                               "{0}.Error Err:Received XML is empty.", _
                                               MethodBase.GetCurrentMethod.Name))
                    retValue = ErrorSystem
                    Exit Try
                End If

                '受信XMLから必要な値を取得する
                Dim resultValueList As Dictionary(Of String, String) = Me.GetSendReserveXmlResultData(resultString)

                '受信XMLから必要な値を取得できない場合、エラーとする
                If IsNothing(resultValueList) Then
                    Logger.Warn(String.Format(CultureInfo.InvariantCulture, _
                                               "{0}.Error ErrCode:{1}, Received XML is error.", _
                                               MethodBase.GetCurrentMethod.Name, _
                                               ErrorDms))
                    retValue = ErrorSystem
                    Exit Try
                End If

                '結果コードが0以外の場合、エラーとする
                If Not resultValueList.Item(TagResultId).Equals("0") Then

                    Logger.Warn(String.Format(CultureInfo.InvariantCulture, _
                                               "{0}.{1}_Error ErrorCode:{2}, ReceivedXmlContents:ResultId={3}, Message={4}", _
                                               Me.GetType.ToString(), _
                                               MethodBase.GetCurrentMethod.Name, _
                                               ErrorDms, _
                                               resultValueList.Item(TagResultId), _
                                               resultValueList.Item(TagMessage)))

                    '送信XMLのログ出力
                    Logger.Warn(String.Format(CultureInfo.InvariantCulture, _
                                               "{0}.{1}_Error SentXML = {2}", _
                                               Me.GetType.ToString(), _
                                               MethodBase.GetCurrentMethod.Name, _
                                               sendXml.InnerXml))

                    '受信XMLのログ出力
                    Logger.Warn(String.Format(CultureInfo.InvariantCulture, _
                                               "{0}.{1}_Error ReceivedXML = {2}", _
                                               Me.GetType.ToString(), _
                                               MethodBase.GetCurrentMethod.Name, _
                                               resultString))

                    '2015/04/23 TMEJ 明瀬 DMS連携版サービスタブレット強制納車機能追加開発 START

                    'retValue = ErrorSystem
                    'Exit Try

                    Using svcCommonBiz As New ServiceCommonClassBusinessLogic

                        'DMSから返却されたエラーコードが除外対象かどうかを確認
                        If svcCommonBiz.IsOmitDmsErrorCode(InterfaceTypeSendReserve, _
                                                           resultValueList.Item(TagResultId)) Then

                            '除外対象であった場合
                            'DMS除外エラーの警告フラグをTrueに設定しておく
                            warningOmitDmsErrorFlg = True

                        Else
                            '除外対象でなかった場合
                            '9999(異常終了)を返却値に設定
                            retValue = ErrorSystem

                            '2015/10/08 TM 皆川 タブレットSMBのチップ移動時の連携送信メッセージ詳細化 Start
                            '文言コード取得
                            Dim wordCd As Integer = svcCommonBiz.ParticularDmsErrorCodeToWordCode(TypeCodeRsltIdSendResvTablet, _
                                                                                       resultValueList.Item(TagResultId))
                            '取得文言コードが-1以外の場合
                            If wordCd <> -1 Then

                                '返却値に取得文言コードを設定
                                retValue = wordCd

                            End If
                            '2015/10/08 TM 皆川 タブレットSMBのチップ移動時の連携送信メッセージ詳細化 End

                            Exit Try

                        End If

                    End Using

                    '2015/04/23 TMEJ 明瀬 DMS連携版サービスタブレット強制納車機能追加開発 END

                End If

                '**************************************************
                '* 基幹作業内容IDの更新
                '**************************************************
                retValue = Me.UpdateBasRezId(sendTargetChip.JOB_DTL_ID, _
                                             sendTargetChip.DMS_JOB_DTL_ID, _
                                             resultValueList.Item(TagBasRezId), _
                                             nowDateTime, _
                                             inFunctionId)

            Next

            '2015/04/23 TMEJ 明瀬 DMS連携版サービスタブレット強制納車機能追加開発 START

            If retValue <> ErrorSystem Then
                '全ての処理を終えた時点で、返却値が9999(システムエラー)でない

                If warningOmitDmsErrorFlg Then
                    'DMS除外エラーの警告フラグがTrueである

                    '-9000(DMS除外エラーの警告)を返却値に設定
                    retValue = WarningOmitDmsError

                End If

            End If

            '2015/04/23 TMEJ 明瀬 DMS連携版サービスタブレット強制納車機能追加開発 END

        Finally
            'Logger.Info(String.Format(CultureInfo.InvariantCulture, _
            '                          "{0}.E OUT:retValue={1}", _
            '                          MethodBase.GetCurrentMethod.Name, retValue))
        End Try

        Return retValue

    End Function

#Region "判定系"

    ''' <summary>
    ''' WalkIn予約を送信するか否かを判断する
    ''' </summary>
    ''' <param name="walkInSendFlg">来店送信フラグ</param>
    ''' <param name="acceptanceType">受付区分</param>
    ''' <returns>True:送信する/False:送信しない</returns>
    ''' <remarks></remarks>
    Private Function JudgeWalkInSend(ByVal walkInSendFlg As String, _
                                     ByVal acceptanceType As String) As Boolean

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_S walkInSendFlg={1}, acceptanceType={2}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  walkInSendFlg, _
                                  acceptanceType))

        '返却値(初期値はTrue(送信する))
        Dim judgeValue As Boolean = True

        If NotSendWalkIn.Equals(walkInSendFlg) Then
            '来店送信フラグが0(WalkInは送信しない)の場合

            If AcceptanceTypeWalkin.Equals(acceptanceType) Then
                '自分がWalk-inの場合

                '送信しない
                judgeValue = False

            End If

        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_E judgeValue={1}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  judgeValue))

        Return judgeValue

    End Function

    ''' <summary>
    ''' PDS予約を送信するか否かを判断する
    ''' </summary>
    ''' <param name="pdsSendFlg">PDS送信フラグ</param>
    ''' <param name="svcClassType">サービス分類区分</param>
    ''' <returns>True:送信する/False:送信しない</returns>
    ''' <remarks></remarks>
    Private Function JudgePdsSend(ByVal pdsSendFlg As String, _
                                  ByVal svcClassType As String) As Boolean

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_S pdsSendFlg={1}, svcClassType={2}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  pdsSendFlg, _
                                  svcClassType))

        '返却値(初期値はTrue(送信する))
        Dim judgeValue As Boolean = True

        If NotSendPds.Equals(pdsSendFlg) Then
            'PDS送信フラグが0(PDSは送信しない)の場合

            If ServiceClassTypePds.Equals(svcClassType) Then
                '自分の整備種類がPDSの場合

                '送信しない
                judgeValue = False

            End If

        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_E judgeValue={1}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  judgeValue))

        Return judgeValue

    End Function

    ''' <summary>
    ''' 自分が子チップかどうかの確認をする
    ''' </summary>
    ''' <param name="svcInId">サービス入庫</param>
    ''' <param name="jobDtlId">作業内容ID</param>
    ''' <param name="prevCancelJobDtlIdList">元々キャンセルだった作業内容IDリスト</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function IsChildChip(ByVal svcInId As Decimal, _
                                 ByVal jobDtlId As Decimal, _
                                 ByVal prevCancelJobDtlIdList As List(Of Decimal)) As Boolean

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_S IN:svcInId={1}, jobDtlId={2}", _
                                  MethodBase.GetCurrentMethod.Name, svcInId, jobDtlId))

        Dim retValue As Boolean = False

        Dim getTable As IC3800903NumberValueDataTable = Nothing

        '自分のテーブルアダプタークラスインスタンスを生成
        Using myTableAdapter As New IC3800903TableAdapter
            '自分が子チップかどうかを判断するための情報を取得
            getTable = myTableAdapter.GetJudgeChildChip(svcInId, _
                                                        jobDtlId, _
                                                        prevCancelJobDtlIdList)
        End Using

        If 0 < getTable.Count Then
            '同じサービス入庫IDで、自分の作業内容IDより小さい作業内容IDが存在する場合は子チップ
            retValue = True
        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_E OUT:retValue={1}", _
                                  MethodBase.GetCurrentMethod.Name, retValue))

        Return retValue

    End Function

    ''' <summary>
    ''' 関連チップ送信が可能かどうかを判定
    ''' </summary>
    ''' <param name="sendRelationStatus">関連チップ送信フラグ</param>
    ''' <param name="svcInId">サービス入庫ID</param>
    ''' <param name="jobDtlId">作業内容ID</param>
    ''' <param name="prevCancelJobDtlIdList">元々キャンセルだった作業内容IDリスト</param>
    ''' <returns>True:可能/False:不可能</returns>
    ''' <remarks></remarks>
    Private Function CanSendRelationStatus(ByVal sendRelationStatus As String, _
                                           ByVal svcInId As Decimal, _
                                           ByVal jobDtlId As Decimal, _
                                           ByVal prevCancelJobDtlIdList As List(Of Decimal)) As Boolean

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_S IN:sendRelationStatus={1}, svcInId={2}, jobDtlId={3}", _
                                  MethodBase.GetCurrentMethod.Name, sendRelationStatus, svcInId, jobDtlId))

        Dim retValue As Boolean = False

        '関連チップ送信フラグが「1:送信する」の場合、無条件に送信する
        If sendRelationStatus.Equals("1") Then
            retValue = True
        Else

            If Me.IsChildChip(svcInId, jobDtlId, prevCancelJobDtlIdList) Then
                '子チップなら送信しない
                retValue = False
            Else
                '親チップなら関連チップ送信フラグに関係なく送信する
                retValue = True
            End If
        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_E OUT:retValue={1}", _
                                  MethodBase.GetCurrentMethod.Name, retValue))

        Return retValue

    End Function

    ''' <summary>
    ''' ストールIDの送信可否を判定
    ''' </summary>
    ''' <param name="reserveInfoRow">予約情報データ行</param>
    ''' <param name="subareaStallIdSendFlg">サブエリアストールID送信フラグ</param>
    ''' <param name="crntStatus">変換後ステータス</param>
    ''' <returns>True:可能/False:不可能</returns>
    ''' <remarks></remarks>
    Private Function CanSendStallId(ByVal reserveInfoRow As IC3800903DmsSendReserveInfoRow, _
                                    ByVal subareaStallIdSendFlg As String, _
                                    ByVal crntStatus As String) As Boolean

        '引数をログに出力
        Dim args As New List(Of String)

        'DataRow内の項目を列挙
        Me.AddLogData(args, reserveInfoRow)

        '開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.S IN:{1}, subareaStallIdSendFlg={2}, crntStatus={3}" _
                    , MethodBase.GetCurrentMethod.Name _
                    , String.Join(", ", args.ToArray()) _
                    , subareaStallIdSendFlg, crntStatus))

        Dim retValue As Boolean = True

        Try
            'サブエリアストールID送信フラグが「1:送信する」の場合、無条件に送信する
            If subareaStallIdSendFlg.Equals("1") Then
                retValue = True
                Exit Try
            Else

                '？→サブエリアの更新の場合、ストールIDを送らない
                If Me.IsNotSendSubareaStallIdStatus(crntStatus) Then
                    retValue = False
                    Exit Try
                End If

                '*****現状のTabletSMBではありえない*****
                'Dim scheStartDateTime As Date = reserveInfoRow.SCHE_START_DATETIME  '予定開始日時
                'Dim scheEndDateTime As Date = reserveInfoRow.SCHE_END_DATETIME      '予定終了日時
                '時間が確定していない状態(日別SMBのストール名上にある)では送信しない。作業開始前の直後が該当。
                'If scheStartDateTime.Equals(Date.MinValue) _
                'AndAlso scheEndDateTime.Equals(Date.MinValue) Then
                '    retValue = False
                'End If

                If reserveInfoRow.STALL_USE_STATUS.Equals(StallUseStatusJobStop) _
                OrElse reserveInfoRow.STALL_USE_STATUS.Equals(StallUseStatusNoShow) Then
                    'ストール利用ステータスが05:中断、07:未来店客の場合、ストールIDを送らない

                    retValue = False
                    Exit Try

                ElseIf reserveInfoRow.STALL_USE_STATUS.Equals(StallUseStatusWorkOrderWait) _
                OrElse reserveInfoRow.STALL_USE_STATUS.Equals(StallUseStatusStartWait) Then
                    'ストール利用ステータスが00:着工指示待ち、01:作業開始待ちの場合

                    If reserveInfoRow.STALL_ID = 0 Then
                        'ストールIDがDB初期値(0)であればストールIDを送らない
                        retValue = False
                        Exit Try
                    End If

                    If reserveInfoRow.TEMP_FLG.Equals("1") Then
                        '仮置きフラグが1であればストールIDを送らない
                        retValue = False
                        Exit Try
                    End If
                End If

                'ここまでTryを抜けなければストールIDを送る
                retValue = True

            End If

        Finally

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                      "{0}_E OUT:retValue={1}", _
                      MethodBase.GetCurrentMethod.Name, retValue))

        End Try

        Return retValue

    End Function

    ''' <summary>
    ''' 変更後のチップステータスでストールIDを送信可否を判定
    ''' </summary>
    ''' <param name="status">変更後のチップステータス</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function IsNotSendSubareaStallIdStatus(ByVal status As String) As Boolean

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_S IN:status={1}", _
                                  MethodBase.GetCurrentMethod.Name, status))

        Dim retValue As Boolean = False

        '変更後ステータスが中断、仮置き、No Showの場合はストールIDを送らない
        If status.Equals(ChipStatusStopForPartsStockout) _
        OrElse status.Equals(ChipStatusStopForWaitCustomer) _
        OrElse status.Equals(ChipStatusStopForWaitStall) _
        OrElse status.Equals(ChipStatusStopForOtherReason) _
        OrElse status.Equals(ChipStatusStopForInspection) _
        OrElse status.Equals(ChipStatusTemp) _
        OrElse status.Equals(ChipStatusNoshow) Then
            retValue = True
        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_E IN:retValue={1}", _
                                  MethodBase.GetCurrentMethod.Name, retValue))

        Return retValue

    End Function

    ''' <summary>
    ''' 関連チップが存在するか否か判定
    ''' </summary>
    ''' <param name="svcinId">サービス入庫ID</param>
    ''' <param name="prevCancelJobDtlIdList">元々キャンセルだった作業内容IDリスト</param>
    ''' <returns>True：存在する/False：存在しない</returns>
    ''' <remarks></remarks>
    Private Function IsExistRelationChip(ByVal svcinId As Decimal, _
                                         ByVal prevCancelJobDtlIdList As List(Of Decimal)) As Boolean

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_S. svcinId={1}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  svcinId))

        Dim returnValue As Boolean

        '自分のテーブルアダプタークラスインスタンスを生成
        Using myTableAdapter As New IC3800903TableAdapter

            Dim numberValueTable As IC3800903NumberValueDataTable _
                = myTableAdapter.GetJudgeExistRelationChipInfo(svcinId, _
                                                               prevCancelJobDtlIdList)

            If 1 < numberValueTable.Rows.Count Then
                '関連チップ有り
                returnValue = True
            Else
                '関連チップ無し
                returnValue = False
            End If

        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_E OUT:ReturnValue={1}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  returnValue))

        Return returnValue

    End Function

#End Region

#Region "取得系"

    ''' <summary>
    ''' 予約送信を行うかどうかを決定するフラグを取得する
    ''' </summary>
    ''' <param name="prevStatus">変更前チップステータス(基幹連携独自の定義)</param>
    ''' <param name="crntStatus">変更後チップステータス(基幹連携独自の定義)</param>
    ''' <param name="svcinId">サービス入庫ID</param>
    ''' <param name="jobDtlId">作業内容ID</param>
    ''' <param name="prevResvStatus">更新前予約ステータス</param>
    ''' <returns>"1":送信する/それ以外:送信しない</returns>
    ''' <remarks>
    ''' ・送信フラグ(TB_M_SVC_LINK_SEND_SETTING)が0の場合、送信しない
    ''' ・送信フラグが1、かつ来店送信フラグが1の場合、かつPDS送信フラグが1の場合、送信する
    ''' ・送信フラグが1、かつ来店送信フラグが0の場合は、
    ''' 　・自分が予約客なら送信する
    ''' 　・自分がWalk-inなら送信しない
    ''' ・送信フラグが1、かつPDS送信フラグが0の場合は、
    ''' 　・自分の整備種類がPDS以外なら送信する
    ''' 　・自分の整備種類がPDSなら送信しない
    ''' </remarks>
    Private Function GetSendReserveFlg(ByVal prevStatus As String, _
                                       ByVal crntStatus As String, _
                                       ByVal svcinId As Decimal, _
                                       ByVal jobDtlId As Decimal, _
                                       ByVal prevResvStatus As String) As String

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_S prevStatus={1}, crntStatus={2}, svcinId={3}, jobDtlId={4}, prevResvStatus={5}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  prevStatus, _
                                  crntStatus, _
                                  svcinId, _
                                  jobDtlId, _
                                  prevResvStatus))

        'ログインユーザー情報取得
        Dim userContext As StaffContext = StaffContext.Current

        '送信フラグ(戻り値)
        Dim sendFlg As String = String.Empty

        '自分のテーブルアダプタークラスインスタンスを生成
        Using myTableAdapter As New IC3800903TableAdapter

            '予約送信するか否かを判断する情報を取得(送信フラグ以外)
            Dim judgeSendReserveTable As IC3800903JudgeSendReserveDataTable = _
                myTableAdapter.GetJudgeSendReserveData(svcinId, jobDtlId)

            If 0 < judgeSendReserveTable.Count Then
                'サービス入庫のレコードが取得できた場合

                '予約ステータス
                Dim crntReserveStatus As String = judgeSendReserveTable(0).RESV_STATUS

                '受付区分
                Dim crntAcceptanceType As String = judgeSendReserveTable(0).ACCEPTANCE_TYPE

                'サービス分類区分
                Dim svcClassType As String = judgeSendReserveTable(0).SVC_CLASS_TYPE

                '予約送信するかしないかのフラグを取得
                Dim linkSendTable As IC3800903LinkSendSettingsDataTable = _
                    myTableAdapter.GetLinkSettings(userContext.DlrCD, _
                                                   userContext.BrnCD, _
                                                   AllDealerCode, _
                                                   AllBranchCode, _
                                                   "1", _
                                                   Me.ConvertStatus(prevStatus, prevResvStatus), _
                                                   Me.ConvertStatus(crntStatus, crntReserveStatus))

                'ログ出力用販売店コード、店舗コード
                Dim dealerCode As String = String.Empty
                Dim branchCode As String = String.Empty

                '来店送信フラグ
                Dim walkInSendFlg As String = String.Empty

                'PDS送信フラグ
                Dim pdsSendFlg As String = String.Empty

                Dim getFirstRow As IC3800903LinkSendSettingsRow

                If 0 < linkSendTable.Count Then

                    '取得データの1行目（最優先レコード）のみを取得
                    getFirstRow = linkSendTable.Item(0)

                    '最優先レコードの送信フラグ、販売店コード、店舗コード、
                    '来店送信フラグ、PDS送信フラグを取得
                    sendFlg = getFirstRow.SEND_FLG
                    dealerCode = getFirstRow.DLR_CD
                    branchCode = getFirstRow.BRN_CD
                    walkInSendFlg = getFirstRow.WALKIN_SEND_FLG
                    pdsSendFlg = getFirstRow.PDS_SEND_FLG

                    If SendReserve.Equals(sendFlg) Then
                        '送信フラグが1(送信する)の場合

                        If Not Me.JudgeWalkInSend(walkInSendFlg, crntAcceptanceType) Then
                            'Walk-in予約は送信しないと判断した場合

                            sendFlg = NotSendReserve

                        Else
                            If Not Me.JudgePdsSend(pdsSendFlg, svcClassType) Then
                                'PDSは送信しないと判断した場合

                                sendFlg = NotSendReserve

                            End If
                        End If

                    End If

                End If

                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                          "{0}_E SEND_FLG={1}, DLR_CD={2}, BRN_CD={3}, WALKIN_SEND_FLG={4}", _
                                          MethodBase.GetCurrentMethod.Name, _
                                          sendFlg, _
                                          dealerCode, _
                                          branchCode, _
                                          walkInSendFlg))

            Else
                'サービス入庫のレコードが取得できない場合(データ異常)
                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                           "{0}_Error TB_T_SERVICEIN Record does not exist.", _
                                           MethodBase.GetCurrentMethod.Name))
            End If

        End Using

        Return sendFlg

    End Function

    ''' <summary>
    ''' 予約ステータスでチップステータスを変換する
    ''' </summary>
    ''' <param name="chipStatus">チップステータス</param>
    ''' <param name="reserveStatus">予約ステータス</param>
    '''<returns>
    '''予約ステータスが「1：本予約」の場合、
    '''またはチップステータスが「5：仮置き」「6：未来店客」以外の場合はチップステータスを変換せずに返却する。
    '''予約ステータスが「0：仮予約」、かつチップステータスが「5：仮置き」「6：未来店客」の場合、以下の２通りに分岐する。
    '''チップステータスが「5：仮置き」の場合、「23：仮置き(仮予約)」を返却する。
    '''チップステータスが「6：未来店客」の場合、「24：未来店客(仮予約)」を返却する。
    ''' </returns>
    ''' <remarks></remarks>
    Private Function ConvertStatus(ByVal chipStatus As String, ByVal reserveStatus As String) As String

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_S IN:chipStatus={1}, reserveStatus={2}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  chipStatus, _
                                  reserveStatus))

        '以下の条件に当てはまらなければ変換せずに返却
        Dim returnStatus As String = chipStatus

        If ResvStatusTentative.Equals(reserveStatus) Then
            '予約ステータスが仮予約の場合

            If ChipStatusNoshow.Equals(chipStatus) Then
                'チップステータス【6：未来店客】→【24：未来店客(仮予約)】に変換
                returnStatus = ChipStatusTentativeNoShow

            ElseIf ChipStatusTemp.Equals(chipStatus) Then
                'チップステータス【5：仮置き】→【23：仮置き(仮予約)】に変換
                returnStatus = ChipStatusTentativeTemp

            End If
        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.E OUT:returnStatus={1}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  returnStatus))

        Return returnStatus

    End Function

    ''' <summary>
    ''' システム設定、販売店設定から予約送信に必要な設定値を取得する
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetSystemSettingValues() As IC3800903SystemSettingValueRow

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_S", _
                                  MethodBase.GetCurrentMethod.Name))

        '戻り値
        Dim retRow As IC3800903SystemSettingValueRow = Nothing

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
                                               "{0}.Error ErrCode:{1}, {2} does not exist.", _
                                               MethodBase.GetCurrentMethod.Name, _
                                               ErrorSysEnv, _
                                               SysLinkSendTimeOutVal))
                    errorFlg = True
                    Exit Try
                End If

                '国コード
                Dim countryCode As String _
                    = svcCommonBiz.GetSystemSettingValueBySettingName(SysCountryCode)

                If String.IsNullOrEmpty(countryCode) Then
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                               "{0}.Error ErrCode:{1}, {2} does not exist.", _
                                               MethodBase.GetCurrentMethod.Name, _
                                               ErrorSysEnv, _
                                               SysCountryCode))
                    errorFlg = True
                    Exit Try
                End If

                '日付フォーマット
                Dim dateFormat As String _
                    = svcCommonBiz.GetSystemSettingValueBySettingName(SysDateFormat)

                If String.IsNullOrEmpty(dateFormat) Then
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                               "{0}.Error ErrCode:{1}, {2} does not exist.", _
                                               MethodBase.GetCurrentMethod.Name, _
                                               ErrorSysEnv, _
                                               SysDateFormat))
                    errorFlg = True
                    Exit Try
                End If

                '仮予約更新キャンセル
                Dim tentativeUpdateCancelFlg As String _
                    = svcCommonBiz.GetSystemSettingValueBySettingName(SysTentativeUpdateCancelFlg)

                If String.IsNullOrEmpty(tentativeUpdateCancelFlg) Then
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                               "{0}.Error ErrCode:{1}, {2} does not exist.", _
                                               MethodBase.GetCurrentMethod.Name, _
                                               ErrorSysEnv, _
                                               SysTentativeUpdateCancelFlg))
                    errorFlg = True
                    Exit Try
                End If

                '管理作業内容ID送信フラグ
                Dim mngResvIdSendFlg As String _
                    = svcCommonBiz.GetSystemSettingValueBySettingName(SysManageJobDtlIdSendFlg)

                If String.IsNullOrEmpty(mngResvIdSendFlg) Then
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                               "{0}.Error ErrCode:{1}, {2} does not exist.", _
                                               MethodBase.GetCurrentMethod.Name, _
                                               ErrorSysEnv, _
                                               SysManageJobDtlIdSendFlg))
                    errorFlg = True
                    Exit Try
                End If

                '関連チップ送信フラグ
                Dim sendRelationStatus As String _
                    = svcCommonBiz.GetSystemSettingValueBySettingName(SysSendRelationStatus)

                If String.IsNullOrEmpty(sendRelationStatus) Then
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                               "{0}.Error ErrCode:{1}, {2} does not exist.", _
                                               MethodBase.GetCurrentMethod.Name, _
                                               ErrorSysEnv, _
                                               SysSendRelationStatus))
                    errorFlg = True
                    Exit Try
                End If

                'サブエリアストールID送信フラグ
                Dim subareaStallIdSendFlg As String _
                    = svcCommonBiz.GetSystemSettingValueBySettingName(SysSubAreaStallIdSendFlg)

                If String.IsNullOrEmpty(subareaStallIdSendFlg) Then
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                               "{0}.Error ErrCode:{1}, {2} does not exist.", _
                                               MethodBase.GetCurrentMethod.Name, _
                                               ErrorSysEnv, _
                                               SysSubAreaStallIdSendFlg))
                    errorFlg = True
                    Exit Try
                End If

                'SOAPバージョン判定値
                Dim soapVersion As String _
                    = svcCommonBiz.GetSystemSettingValueBySettingName(SysSoapVersion)

                If String.IsNullOrEmpty(soapVersion) Then
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                               "{0}.Error ErrCode:{1}, {2} does not exist.", _
                                               MethodBase.GetCurrentMethod.Name, _
                                               ErrorSysEnv, _
                                               SysSoapVersion))
                    errorFlg = True
                    Exit Try
                End If

                'CDATA付与フラグ
                Dim cdataApdFlg As String _
                    = svcCommonBiz.GetSystemSettingValueBySettingName(SysCDataApdFlg)

                If String.IsNullOrEmpty(cdataApdFlg) Then
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                               "{0}.Error ErrCode:{1}, {2} does not exist.", _
                                               MethodBase.GetCurrentMethod.Name, _
                                               ErrorSysEnv, _
                                               SysCDataApdFlg))
                    errorFlg = True
                    Exit Try
                End If

                '******************************
                '* 販売店システム設定から取得
                '******************************
                '送信先アドレス
                Dim linkUrlResvInfo As String _
                    = svcCommonBiz.GetDlrSystemSettingValueBySettingName(DlrSysLinkUrlResvInfo)

                If String.IsNullOrEmpty(linkUrlResvInfo) Then
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                               "{0}.Error ErrCode:{1}, {2} does not exist.", _
                                               MethodBase.GetCurrentMethod.Name, _
                                               ErrorDlrEnv, _
                                               DlrSysLinkUrlResvInfo))
                    errorFlg = True
                    Exit Try
                End If

                Using table As New IC3800903SystemSettingValueDataTable

                    retRow = table.NewIC3800903SystemSettingValueRow

                    With retRow
                        '取得した値を戻り値のデータ行に設定
                        .LINK_SEND_TIMEOUT_VAL = linkSendTimeoutVal
                        .DIST_CD = countryCode
                        .DATE_FORMAT = dateFormat
                        .TENTATIVE_UPDATE_CANCEL_FLG = tentativeUpdateCancelFlg
                        .MNG_JOB_DTL_ID_SEND_FLG = mngResvIdSendFlg
                        .SEND_RELATION_STATUS = sendRelationStatus
                        .SUBAREA_STALL_ID_SEND_FLG = subareaStallIdSendFlg
                        .LINK_URL_RESV_INFO = linkUrlResvInfo
                        .LINK_SOAP_VERSION = soapVersion
                        .CDATA_APD_FLG = cdataApdFlg
                    End With

                End Using

            End Using

        Finally

            If errorFlg Then
                retRow = Nothing
            End If

        End Try

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_E", _
                                  MethodBase.GetCurrentMethod.Name))

        Return retRow

    End Function

    ''' <summary>
    ''' 予約送信対象のチップ情報を取得する
    ''' </summary>
    ''' <param name="svcInId">サービス入庫ID</param>
    ''' <param name="jobDtlId">作業内容ID</param>
    ''' <param name="prevResvStatus">更新前予約ステータス</param>
    ''' <param name="prevCancelJobDtlIdList">元々キャンセルだった作業内容IDリスト</param>
    ''' <param name="sendRelationStatusFlg">関連チップ送信フラグ</param>
    ''' <param name="callFromChipDetailFlg">チップ詳細から呼出フラグ</param>
    ''' <returns>予約送信対象チップ情報行</returns>
    ''' <remarks></remarks>
    Private Function GetSendTargetChipInfo(ByVal svcInId As Decimal, _
                                           ByVal jobDtlId As Decimal, _
                                           ByVal prevResvStatus As String, _
                                           ByVal prevCancelJobDtlIdList As List(Of Decimal), _
                                           ByVal sendRelationStatusFlg As String, _
                                           ByVal callFromChipDetailFlg As Boolean) As IC3800903DmsSendReserveInfoRow()

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_S IN:svcInId={1}, jobDtlId={2}, prevResvStatus={3}, callFromChipDetailFlg={4}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  svcInId, _
                                  jobDtlId, _
                                  prevResvStatus, _
                                  callFromChipDetailFlg))

        '返却する予約送信対象チップ情報行
        Dim returnReserveInfoRow As IC3800903DmsSendReserveInfoRow() = Nothing

        Dim reserveInfoDataTable As IC3800903DmsSendReserveInfoDataTable = Nothing

        '自分のテーブルアダプタークラスインスタンスを生成
        Using myTableAdapter As New IC3800903TableAdapter

            '関連チップも含めて予約情報を取得する
            reserveInfoDataTable = myTableAdapter.GetSendDmsReserveInfo(svcInId, _
                                                                        prevCancelJobDtlIdList)

        End Using

        '予約情報の絞り込み文字列を作成する
        Dim selectString As New StringBuilder

        If callFromChipDetailFlg Then
            'チップ詳細から呼出された場合

            If sendRelationStatusFlg.Equals("0") Then
                '関連チップ送信フラグが0(送信しない)の場合

                '親だけ送信する
                selectString.Append("JOB_DTL_ID = MIN(JOB_DTL_ID)")
            Else
                '有効な予約だけ送信する
                selectString.Append("CANCEL_FLG = '0'")
            End If

        Else
            'チップ詳細以外から呼出された場合

            '自分の予約情報を抽出する
            Dim myReserveInfoRow As IC3800903DmsSendReserveInfoRow() _
                = DirectCast(reserveInfoDataTable.Select("JOB_DTL_ID = " & jobDtlId),  _
                             IC3800903DmsSendReserveInfoRow())

            If myReserveInfoRow(0).RESV_STATUS.Equals(prevResvStatus) Then
                '予約ステータスが更新前後で変更なしの場合

                If Me.CanSendRelationStatus(sendRelationStatusFlg, _
                                            svcInId, _
                                            jobDtlId, _
                                            prevCancelJobDtlIdList) Then
                    '自分が親、または自分が子で関連チップ送信フラグが1(送信する)の場合

                    If myReserveInfoRow(0).CANCEL_FLG.Equals("0") Then
                        'キャンセル操作以外を行った場合

                        '自分だけに絞り込む
                        selectString.Append("JOB_DTL_ID = " & jobDtlId)
                    Else
                        'キャンセル操作を行った場合

                        'キャンセルされたチップのみに絞り込む
                        '※自分が親で関連チップも含めて全てキャンセルされていたら、関連チップも抽出される
                        '　自分が親で関連チップはキャンセルされていなければ、自分だけが抽出される
                        '　自分が子ならば自分だけが抽出される
                        selectString.Append("CANCEL_FLG = '1'")

                        If sendRelationStatusFlg.Equals("0") Then
                            '関連チップ送信フラグが0(送信しない)の場合
                            '※この条件に入るということは、自分が子で関連チップ送信フラグが1(送信する)という条件を
                            '　満たしていないため、自分が親であることが決定

                            '自分だけに絞り込む
                            selectString.Append(" AND JOB_DTL_ID = " & jobDtlId)
                        End If

                    End If
                Else
                    'それ以外の場合は送信対象なしとする
                    selectString.Append("JOB_DTL_ID = -1")
                End If
            Else
                '予約ステータスが更新前後で変更ありの場合

                If sendRelationStatusFlg.Equals("0") Then
                    '関連チップ送信フラグが0(送信しない)の場合

                    '親だけ送信する
                    selectString.Append("JOB_DTL_ID = MIN(JOB_DTL_ID)")
                Else
                    '有効な予約だけ送信する
                    selectString.Append("CANCEL_FLG = '0'")
                End If
            End If
        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0} selectString={1}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  selectString.ToString()))

        '送信対象チップを絞り込む
        returnReserveInfoRow = DirectCast(reserveInfoDataTable.Select(selectString.ToString()),  _
                                          IC3800903DmsSendReserveInfoRow())

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_E OUT:targetReserveInfoRowCount:{1}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  returnReserveInfoRow.Count()))

        Return returnReserveInfoRow

    End Function

    ''' <summary>
    ''' 基幹ストールIDを取得する
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="stallId">ストールID</param>
    ''' <returns>基幹ストールID</returns>
    ''' <remarks>変換に失敗した場合は、空文字を返却する</remarks>
    Private Function GetDmsStallId(ByVal dealerCode As String, _
                                   ByVal branchCode As String, _
                                   ByVal stallId As String) As String

        'Logger.Info(String.Format(CultureInfo.InvariantCulture, _
        '                          "{0}_S IN:dealerCode={1}, branchCode={2}, stallId={3}", _
        '                          MethodBase.GetCurrentMethod.Name, _
        '                          dealerCode, _
        '                          branchCode, _
        '                          stallId))

        Dim dmsStallIdTable As ServiceCommonClassDataSet.DmsCodeMapDataTable = Nothing

        Using svcCommonBiz As New ServiceCommonClassBusinessLogic

            'ストールIDを基幹ストールIDに変換
            dmsStallIdTable = svcCommonBiz.GetIcropToDmsCode(dealerCode, _
                                                             ServiceCommonClassBusinessLogic.DmsCodeType.StallId, _
                                                             dealerCode, _
                                                             branchCode, _
                                                             stallId)

        End Using

        'ストールID(xml設定用)
        Dim dmsStallId As String

        If dmsStallIdTable.Count <= 0 Then
            'データが取得できない場合は変換無し
            dmsStallId = stallId

        ElseIf dmsStallIdTable.Count = 1 Then
            'データが1件取得できた場合はDB値を設定(DMS_CD_3)
            dmsStallId = dmsStallIdTable.Item(0).CODE3

        Else
            'データが2件以上取得できた場合は一意に決定できないためエラー
            dmsStallId = String.Empty

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                       "{0}.Error ErrCode:{1}, Failed to convert key stallId.(Non-unique)", _
                                       MethodBase.GetCurrentMethod.Name, _
                                       ErrorDmsCodeMap))

        End If

        'Logger.Info(String.Format(CultureInfo.InvariantCulture, _
        '                          "{0}_E OUT:dmsStallId:{1}", _
        '                          MethodBase.GetCurrentMethod.Name, _
        '                          dmsStallId))
        Return dmsStallId

    End Function

    ''' <summary>
    ''' 管理作業内容IDを取得
    ''' </summary>
    ''' <param name="svcInId">サービス入庫ID</param>
    ''' <param name="jobDtlId">作業内容ID</param>
    ''' <param name="mngResvIdSendFlg">管理作業内容ID送信フラグ</param>
    ''' <param name="prevCancelJobDtlIdList">元々キャンセルだった作業内容IDリスト</param>
    ''' <returns>管理作業内容ID</returns>
    ''' <remarks></remarks>
    Private Function GetManageJobDtlId(ByVal svcInId As Decimal, _
                                       ByVal jobDtlId As Decimal, _
                                       ByVal mngResvIdSendFlg As String, _
                                       ByVal prevCancelJobDtlIdList As List(Of Decimal)) As String

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_S IN:svcInId={1}, jobDtlId={2}, mngResvIdSendFlg={3}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  svcInId, _
                                  jobDtlId, _
                                  mngResvIdSendFlg))

        Dim retPRezId As String = String.Empty

        If Me.IsExistRelationChip(svcInId, prevCancelJobDtlIdList) Then
            '関連チップがある場合
            '関連チップ内の最小作業内容IDを管理作業内容IDとする

            '自分のテーブルアダプタークラスインスタンスを生成
            Using myTableAdapter As New IC3800903TableAdapter

                '最小作業内容IDを取得
                Dim numberValueTable As IC3800903NumberValueDataTable _
                    = myTableAdapter.GetMinimumJobDtlId(svcInId, _
                                                        prevCancelJobDtlIdList)

                Dim minJobDetailId As Decimal = -1

                If 0 < numberValueTable.Count Then
                    minJobDetailId = numberValueTable.Item(0).COL1
                End If

                If minJobDetailId < 0 Then
                    '最小作業内容IDが取得できない場合
                    retPRezId = String.Empty
                Else
                    '最小作業内容IDが取得できた場合
                    retPRezId = minJobDetailId.ToString(CultureInfo.CurrentCulture)
                End If

            End Using

        Else

            '関連チップがない場合
            If mngResvIdSendFlg.Equals("1") Then
                '管理作業内容ID送信フラグが「1:送信する」の場合は、関連チップがない場合でも
                '自分の作業内容IDを管理作業内容IDとする
                retPRezId = jobDtlId.ToString(CultureInfo.CurrentCulture)
            End If

        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_E OUT:retPRezId={1}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  retPRezId))

        Return retPRezId

    End Function

    ''' <summary>
    ''' 子予約連番を取得
    ''' </summary>
    ''' <param name="svcInId">サービス入庫ID</param>
    ''' <param name="jobDtlId">作業内容ID</param>
    ''' <param name="mngJobDtlId">管理作業内容ID</param>
    ''' <param name="prevCancelJobDtlIdList">元々キャンセルだった作業内容IDリスト</param>
    ''' <returns>子予約連番</returns>
    ''' <remarks></remarks>
    Private Function GetRezChildNo(ByVal svcInId As Decimal, _
                                   ByVal jobDtlId As Decimal, _
                                   ByVal mngJobDtlId As String, _
                                   ByVal prevCancelJobDtlIdList As List(Of Decimal)) As String

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_S IN:svcInId={1}, jobDtlId={2}, mngJobDtlId={3}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  svcInId, _
                                  jobDtlId, _
                                  mngJobDtlId))

        Dim retRezChildNo As String

        If Not String.IsNullOrEmpty(mngJobDtlId) Then
            '管理作業内容IDがある場合

            '自分のテーブルアダプタークラスインスタンスを生成
            Using myTableAdapter As New IC3800903TableAdapter

                '子予約連番を取得
                Dim numberValueTable As IC3800903NumberValueDataTable _
                    = myTableAdapter.GetRezChildNo(svcInId, _
                                                   jobDtlId, _
                                                   prevCancelJobDtlIdList)

                Dim rezChildNo As Decimal = -1

                If 0 < numberValueTable.Count Then
                    rezChildNo = numberValueTable.Item(0).COL1
                End If

                If rezChildNo < 0 Then
                    '子予約連番が取得できない場合
                    retRezChildNo = String.Empty
                Else
                    '子予約連番が取得できた場合
                    retRezChildNo = rezChildNo.ToString(CultureInfo.CurrentCulture)
                End If

            End Using
        Else
            '管理作業内容IDがない場合
            retRezChildNo = String.Empty
        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_E OUT:retRezChildNo={1}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  retRezChildNo))

        Return retRezChildNo

    End Function

    ''' <summary>
    ''' キャンセルフラグの設定値を取得する
    ''' </summary>
    ''' <param name="cancelFlg">予約情報(更新後の予約).キャンセルフラグ</param>
    ''' <param name="reserveStatus">予約情報(更新後の予約).予約ステータス</param>
    ''' <param name="prevReserveStatus">更新前予約ステータス</param>
    ''' <param name="tentativeUpdateCancelFlg">仮予約時キャンセル送信フラグ</param>
    ''' <returns>送信用XMLに設定するキャンセルフラグ</returns>
    ''' <remarks>
    ''' 以下の条件を全て満たす場合、キャンセルフラグに「1:キャンセル」を設定する。
    ''' システム設定.仮予約時キャンセル送信フラグの設定値が「1：送信する」
    ''' 入力データ.変更前予約ステータスが「1：本予約」
    ''' 予約情報(更新後の予約).予約ステータスが「0：仮予約」
    ''' </remarks>
    Private Function GetCancelFlgSettingValue(ByVal cancelFlg As String, _
                                              ByVal reserveStatus As String, _
                                              ByVal prevReserveStatus As String, _
                                              ByVal tentativeUpdateCancelFlg As String) As String

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_S IN:cancelFlg={1}, reserveStatus={2}, prevReserveStatus={3}, tentativeUpdateCancelFlg={4}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  cancelFlg, _
                                  reserveStatus, _
                                  prevReserveStatus, _
                                  tentativeUpdateCancelFlg))

        '戻り値の初期化(0:キャンセルでない)
        Dim retCancelValue As String = "0"

        If cancelFlg.Equals("1") Then
            '既にキャンセルフラグが立っている場合は「1:キャンセル」を設定
            retCancelValue = "1"
        Else
            '以下の条件を満たす場合、「1:キャンセル」を設定(remarks参照)
            If tentativeUpdateCancelFlg.Equals("1") _
            AndAlso prevReserveStatus.Equals(ResvStatusConfirmed) _
            AndAlso reserveStatus.Equals(ResvStatusTentative) Then
                retCancelValue = "1"
            End If
        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_E OUT:retCancelValue={1}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  retCancelValue))

        Return retCancelValue

    End Function

    ''' <summary>
    ''' VINを取得する
    ''' </summary>
    ''' <param name="vclMstVin">車両マスタのVIN</param>
    ''' <param name="regNo">車両登録番号</param>
    ''' <param name="salesBookingVin">注文マスタのVIN</param>
    ''' <returns>
    ''' 車両マスタからVINが取得できる場合、または登録番号が設定されている場合、車両マスタのVINを返却する。
    ''' (注文番号が設定され、VINが未設定の場合は空で設定する)
    ''' 取得できない場合は、注文マスタのVINを返却する。
    ''' </returns>
    ''' <remarks></remarks>
    Private Function GetVin(ByVal vclMstVin As String, _
                            ByVal regNo As String, _
                            ByVal salesBookingVin As String) As String

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_S IN:vclMstVin={1}, salesBookingVin={2}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  vclMstVin, _
                                  salesBookingVin))

        Dim retVin As String = String.Empty

        '車両登録番号、VIN(車両マスタ)のどちらか一方があるか
        If Not String.IsNullOrWhiteSpace(vclMstVin) _
        OrElse Not String.IsNullOrWhiteSpace(regNo) Then
            '車両マスタのVINを返却
            retVin = vclMstVin.Trim()
        Else
            '注文マスタのVINを返却
            retVin = salesBookingVin.Trim()
        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_E OUT:retVin={1}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  retVin))

        Return retVin

    End Function

    ''' <summary>
    ''' WebServiceの戻りXMLを解析し値を取得
    ''' </summary>
    ''' <param name="resultString">送信XML文字列</param>
    ''' <returns>結果XML</returns>
    ''' <remarks></remarks>
    Private Function GetSendReserveXmlResultData(ByVal resultString As String) As Dictionary(Of String, String)

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.S IN:resultString:{1}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  resultString))

        Dim retXmlValueDic As New Dictionary(Of String, String)

        Try
            'XmlDocument
            Dim resultXmlDocument As New XmlDocument

            '返却された文字列をXML化
            resultXmlDocument.LoadXml(resultString)

            'XmlElementを取得
            Dim resultXmlElement As XmlElement = resultXmlDocument.DocumentElement

            'XmlElementの確認
            If IsNothing(resultXmlElement) Then
                '取得失敗
                Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.Error Err: XmlDocument.DocumentElement is nothing." _
                            , MethodBase.GetCurrentMethod.Name))

                retXmlValueDic = Nothing
                Exit Try
            End If

            '予約送信の返却XML内の必要なタグがない場合、エラー(戻り値Nothing)とする
            If Not Me.CheckResultXmlElementTag(resultXmlElement) Then
                retXmlValueDic = Nothing
                Exit Try
            End If

            '返却XMLの中から必要な値を取得
            Dim resultId As String = resultXmlElement.GetElementsByTagName(TagResultId).Item(0).InnerText
            Dim message As String = resultXmlElement.GetElementsByTagName(TagMessage).Item(0).InnerText
            Dim basRezId As String = String.Empty

            'BASREZIDタグが存在する場合のみ取得する
            If Not IsNothing(resultXmlElement.GetElementsByTagName(TagBasRezId).Item(0)) Then
                basRezId = resultXmlElement.GetElementsByTagName(TagBasRezId).Item(0).InnerText
            End If

            '戻り値のDictionaryに設定
            retXmlValueDic.Add(TagResultId, resultId)
            retXmlValueDic.Add(TagMessage, message)
            retXmlValueDic.Add(TagBasRezId, basRezId)

            Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                      "{0}.E OUT:ResultId:{1}, Message:{2}, BASREZID:{3}", _
                                      MethodBase.GetCurrentMethod.Name, _
                                      resultId, _
                                      message, _
                                      basRezId))

        Catch ex As XmlException

            Logger.Error(String.Format(CultureInfo.CurrentCulture _
                         , "{0}.Error" _
                         , MethodBase.GetCurrentMethod.Name), ex)
            retXmlValueDic = Nothing

        End Try

        Return retXmlValueDic

    End Function

#End Region

#Region "更新系"

    ''' <summary>
    ''' 受信したXML内の基幹作業内容IDで作業内容テーブルの値を更新する
    ''' </summary>
    ''' <param name="jobDtlId">作業内容ID</param>
    ''' <param name="dmsJobDtlId">基幹作業内容ID</param>
    ''' <param name="recieveBasRezId">受信したXML内の基幹作業内容ID</param>
    ''' <param name="nowDateTime">現在日時</param>
    ''' <param name="updateFunction">機能ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function UpdateBasRezId(ByVal jobDtlId As Decimal, _
                                    ByVal dmsJobDtlId As String, _
                                    ByVal recieveBasRezId As String, _
                                    ByVal nowDateTime As Date, _
                                    ByVal updateFunction As String) As Integer

        'Logger.Info(String.Format(CultureInfo.InvariantCulture, _
        '                          "{0}_S IN:jobDtlId={1}, dmsJobDtlId={2}, recieveBasRezId={3}, nowDateTime={4}, updateFunction={5}", _
        '                          MethodBase.GetCurrentMethod.Name, _
        '                          jobDtlId, _
        '                          dmsJobDtlId, _
        '                          recieveBasRezId, _
        '                          nowDateTime, _
        '                          updateFunction))

        '戻り値(処理結果コード)
        Dim resultCode As Integer = Success

        '2016/8/24 NSK 竹中　TR-SVT-TMT-20150817-001 START
        '受信XMLオブジェクトから有効な基幹予約IDが取得できない、
        'または送信前と同じ基幹予約IDの場合は更新する必要がない
        'If String.IsNullOrEmpty(recieveBasRezId) _
        'OrElse recieveBasRezId.Equals(dmsJobDtlId) Then

        '受信XMLオブジェクトから有効な基幹予約IDが取得できない、
        'または送信前の基幹予約IDに値が設定されている場合は更新する必要がない
        If String.IsNullOrEmpty(recieveBasRezId) _
        OrElse Not String.IsNullOrWhiteSpace(dmsJobDtlId) Then
            '2016/8/24 NSK 竹中　TR-SVT-TMT-20150817-001 END

            '正常終了
            resultCode = Success
        Else

            Dim userContext As StaffContext = StaffContext.Current

            Dim updateDmsJobDtlIdResult As Integer = 0

            Using myTableAdapter As New IC3800903TableAdapter
                '受信XMLオブジェクト内の基幹予約IDをDBに登録
                updateDmsJobDtlIdResult = myTableAdapter.UpdateDmsJobDtlId(jobDtlId, _
                                                                           recieveBasRezId, _
                                                                           userContext.Account, _
                                                                           nowDateTime, _
                                                                           updateFunction)
            End Using

            If updateDmsJobDtlIdResult <= 0 Then
                '作業内容テーブルの更新に失敗
                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                             "{0}_Error ErrCode:{1}, UpdateDmsJobDtlId Error. OUT:updateDmsJobDtlIdResult={2}", _
                             MethodBase.GetCurrentMethod.Name, _
                             ErrorUpdateJobDtl, _
                             updateDmsJobDtlIdResult))

                resultCode = ErrorSystem
            End If
        End If

        'Logger.Info(String.Format(CultureInfo.InvariantCulture, _
        '                          "{0}_E OUT:resultCode={1}", _
        '                          MethodBase.GetCurrentMethod.Name, resultCode))

        Return resultCode

    End Function

#End Region

#Region "予約送信用XMLの構築"

    ''' <summary>
    ''' 予約送信用XMLを構築する(メイン)
    ''' </summary>
    ''' <param name="sysValueRow">システム設定値データ行</param>
    ''' <param name="cstVclInfoRow">車両・顧客情報データ行</param>
    ''' <param name="reserveInfoRow">予約情報データ行</param>
    ''' <param name="otherInfoRow">その他情報データ行</param>
    ''' <param name="nowDateTime">現在日時</param>
    ''' <returns>構築したXMLドキュメント</returns>
    ''' <remarks></remarks>
    Private Function StructSendReserveXml(ByVal sysValueRow As IC3800903SystemSettingValueRow, _
                                          ByVal cstVclInfoRow As IC3800903DmsSendCstVclInfoRow, _
                                          ByVal reserveInfoRow As IC3800903DmsSendReserveInfoRow, _
                                          ByVal otherInfoRow As IC3800903DmsSendOtherInfoRow, _
                                          ByVal nowDateTime As Date) As XmlDocument

        '引数をログに出力
        Dim args As New List(Of String)

        'DataRow内の項目を列挙
        Me.AddLogData(args, sysValueRow)
        Me.AddLogData(args, cstVclInfoRow)
        Me.AddLogData(args, reserveInfoRow)
        Me.AddLogData(args, otherInfoRow)

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.S IN:{1}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  String.Join(", ", args.ToArray())))


        '65001がUTF-8
        Dim xmlEncode As Encoding = Encoding.GetEncoding(EncodeUtf8)

        'XMLドキュメント作成
        Dim xmlDocument As New XmlDocument

        'ヘッダ部作成(<?xml version="1.0" encoding="utf-8"?>の部分)
        Dim xmlDeclaration As XmlDeclaration = xmlDocument.CreateXmlDeclaration("1.0", xmlEncode.BodyName, Nothing)

        'ルートタグ(UpdateReserveタグ)の作成
        Dim xmlRoot As XmlElement = xmlDocument.CreateElement(TagUpdateReserve)

        'headタグの構築
        Dim headTag As XmlElement = Me.StructSendReserveXmlHeadTag(xmlDocument, _
                                                                   sysValueRow.DIST_CD, _
                                                                   sysValueRow.DATE_FORMAT, _
                                                                   nowDateTime)

        'Detailタグの構築
        Dim detailTag As XmlElement = Me.StructSendReserveXmlDetailTag(xmlDocument, _
                                                                       cstVclInfoRow, _
                                                                       reserveInfoRow, _
                                                                       otherInfoRow, _
                                                                       nowDateTime, _
                                                                       sysValueRow.CDATA_APD_FLG)

        If String.IsNullOrEmpty(detailTag.InnerXml) Then
            '必須チェックエラー

            xmlDocument = Nothing

        Else
            'UpdateReserveタグを構築
            xmlRoot.AppendChild(headTag)
            xmlRoot.AppendChild(detailTag)

            '送信用XMLを構築
            xmlDocument.AppendChild(xmlDeclaration)
            xmlDocument.AppendChild(xmlRoot)

            Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                      "{0}.E OUT:STRUCTXML = " & vbCrLf & "{1}", _
                      MethodBase.GetCurrentMethod.Name, _
                      Me.FormatXml(xmlDocument)))

        End If

        Return xmlDocument

    End Function

    ''' <summary>
    ''' 予約送信用XMLのheadタグを構築する
    ''' </summary>
    ''' <param name="xmlDocument">予約送信用XMLドキュメント</param>
    ''' <param name="countryCode">国コード</param>
    ''' <param name="dateFormat">日付フォーマット</param>
    ''' <param name="nowDateTime">現在日時</param>
    ''' <returns>headタグエレメント</returns>
    ''' <remarks></remarks>
    Private Function StructSendReserveXmlHeadTag(ByVal xmlDocument As XmlDocument, _
                                                 ByVal countryCode As String, _
                                                 ByVal dateFormat As String, _
                                                 ByVal nowDateTime As Date) As XmlElement

        'Logger.Info(String.Format(CultureInfo.InvariantCulture, _
        '                          "{0}_S OUT:countryCode={1}, dateFormat={2}, dateFormat={3}", _
        '                          MethodBase.GetCurrentMethod.Name, _
        '                          countryCode, _
        '                          dateFormat, _
        '                          nowDateTime))

        'headタグを作成
        Dim headTag As XmlElement = xmlDocument.CreateElement(TagHead)

        'headタグの子要素を作成
        Dim messageIdTag As XmlElement = xmlDocument.CreateElement(TagMessageID)
        Dim countryCodeTag As XmlElement = xmlDocument.CreateElement(TagCountryCode)
        Dim linkSystemCodeTag As XmlElement = xmlDocument.CreateElement(TagLinkSystemCode)
        Dim TransmissionDateTag As XmlElement = xmlDocument.CreateElement(TagTransmissionDate)

        '子要素に値を設定
        messageIdTag.AppendChild(xmlDocument.CreateTextNode(SendReservationId))
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

        'Logger.Info(String.Format(CultureInfo.InvariantCulture, _
        '                          "{0}_E OUT:reserveCustomerInfoTag={1}", _
        '                          MethodBase.GetCurrentMethod.Name, _
        '                          headTag.InnerXml))

        Return headTag

    End Function

    ''' <summary>
    ''' 予約送信用XMLのDetailタグを構築する
    ''' </summary>
    ''' <param name="xmlDocument">予約送信用XMLドキュメント</param>
    ''' <param name="cstVclInfoRow">車両・顧客情報データ行</param>
    ''' <param name="reserveInfoRow">予約情報データ行</param>
    ''' <param name="otherInfoRow">その他情報データ行</param>
    ''' <param name="nowDateTime">現在日時</param>
    ''' <param name="cdataApdFlg">CDATA付与フラグ</param>
    ''' <returns>予約送信用XMLクラスインスタンス</returns>
    ''' <remarks></remarks>
    Private Function StructSendReserveXmlDetailTag(ByVal xmlDocument As XmlDocument, _
                                                   ByVal cstVclInfoRow As IC3800903DmsSendCstVclInfoRow, _
                                                   ByVal reserveInfoRow As IC3800903DmsSendReserveInfoRow, _
                                                   ByVal otherInfoRow As IC3800903DmsSendOtherInfoRow, _
                                                   ByVal nowDateTime As Date, _
                                                   ByVal cdataApdFlg As String) As XmlElement

        ''引数をログに出力
        'Dim args As New List(Of String)

        ''DataRow内の項目を列挙
        'Me.AddLogData(args, cstVclInfoRow)
        'Me.AddLogData(args, reserveInfoRow)
        'Me.AddLogData(args, otherInfoRow)

        ''開始ログの出力
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '            , "{0}.S IN:{1}" _
        '            , MethodBase.GetCurrentMethod.Name _
        '            , String.Join(", ", args.ToArray())))

        'Detailタグを作成
        Dim detailTag As XmlElement = xmlDocument.CreateElement(TagDetail)

        Try
            'Commonタグを構築
            Dim commonTag As XmlElement = Me.StructSendReserveXmlCommonTag(xmlDocument, otherInfoRow)

            If String.IsNullOrEmpty(commonTag.InnerXml) Then
                '必須チェックエラー
                detailTag.InnerXml = String.Empty
                Exit Try
            End If

            'UpdateReserveInformationタグを構築
            Dim updateReserveInfoTag As XmlElement _
                = Me.StructSendReserveXmlUpdateReserveInformationTag(xmlDocument, _
                                                                     cstVclInfoRow, _
                                                                     reserveInfoRow, _
                                                                     otherInfoRow, _
                                                                     nowDateTime, _
                                                                     cdataApdFlg)

            If String.IsNullOrEmpty(updateReserveInfoTag.InnerXml) Then
                '必須チェックエラー
                detailTag.InnerXml = String.Empty
                Exit Try
            End If

            'Detailタグを構築
            With detailTag
                .AppendChild(commonTag)
                .AppendChild(updateReserveInfoTag)
            End With

        Finally

            'Logger.Info(String.Format(CultureInfo.InvariantCulture, _
            '            "{0}_E OUT:detailTag={1}", _
            '            MethodBase.GetCurrentMethod.Name, _
            '            detailTag.InnerXml))

        End Try

        Return detailTag

    End Function

    ''' <summary>
    ''' 予約送信用XMLのCommonタグを構築する
    ''' </summary>
    ''' <param name="xmlDocument">予約送信用XMLドキュメント</param>
    ''' <param name="otherInfoRow">その他情報データ行</param>
    ''' <returns>予約送信用XMLクラスインスタンス</returns>
    ''' <remarks></remarks>
    Private Function StructSendReserveXmlCommonTag(ByVal xmlDocument As XmlDocument, _
                                                   ByVal otherInfoRow As IC3800903DmsSendOtherInfoRow) As XmlElement

        'Logger.Info(String.Format(CultureInfo.InvariantCulture, _
        '                          "{0}_S ", _
        '                          MethodBase.GetCurrentMethod.Name))

        'Commonタグを作成
        Dim commonTag As XmlElement = xmlDocument.CreateElement(TagCommon)

        If Me.CheckNecessaryCommonTag(otherInfoRow.DMS_DLR_CD, otherInfoRow.DMS_BRN_CD) Then
            '必須チェックOK

            'Commonタグの子要素を作成
            Dim dealerCodeTag As XmlElement = xmlDocument.CreateElement(TagDealerCode)
            Dim branchCodeTag As XmlElement = xmlDocument.CreateElement(TagBranchCode)
            Dim staffCodeTag As XmlElement = xmlDocument.CreateElement(TagStaffCode)
            Dim customerCodeTag As XmlElement = xmlDocument.CreateElement(TagCustomerCode)
            Dim salesBookingNumberTag As XmlElement = xmlDocument.CreateElement(TagSalesBookingNumber)
            Dim vinTag As XmlElement = xmlDocument.CreateElement(TagVin1)

            '子要素に値を設定
            dealerCodeTag.AppendChild(xmlDocument.CreateTextNode(otherInfoRow.DMS_DLR_CD))
            branchCodeTag.AppendChild(xmlDocument.CreateTextNode(otherInfoRow.DMS_BRN_CD))
            staffCodeTag.AppendChild(xmlDocument.CreateTextNode(String.Empty))
            customerCodeTag.AppendChild(xmlDocument.CreateTextNode(String.Empty))
            salesBookingNumberTag.AppendChild(xmlDocument.CreateTextNode(String.Empty))
            vinTag.AppendChild(xmlDocument.CreateTextNode(String.Empty))

            'Commonタグの子要素を追加
            With commonTag
                .AppendChild(dealerCodeTag)
                .AppendChild(branchCodeTag)
                .AppendChild(staffCodeTag)
                .AppendChild(customerCodeTag)
                .AppendChild(salesBookingNumberTag)
                .AppendChild(vinTag)
            End With

        Else
            '必須チェックNG

            commonTag.InnerXml = String.Empty

        End If

        'Logger.Info(String.Format(CultureInfo.InvariantCulture, _
        '                          "{0}_E OUT:commonTag={1}", _
        '                          MethodBase.GetCurrentMethod.Name, commonTag.InnerXml))

        Return commonTag

    End Function

    ''' <summary>
    ''' 予約送信用XMLのUpdateReserveInformationタグを構築する
    ''' </summary>
    ''' <param name="xmlDocument">予約送信用XMLドキュメント</param>
    ''' <param name="cstVclInfoRow">車両・顧客情報データ行</param>
    ''' <param name="reserveInfoRow">予約情報データ行</param>
    ''' <param name="otherInfoRow">その他情報データ行</param>
    ''' <param name="nowDateTime">現在日時</param>
    ''' <param name="cdataApdFlg">CDATA付与フラグ</param>
    ''' <returns>UpdateReserveInformationタグエレメント</returns>
    ''' <remarks></remarks>
    Private Function StructSendReserveXmlUpdateReserveInformationTag( _
                                                   ByVal xmlDocument As XmlDocument, _
                                                   ByVal cstVclInfoRow As IC3800903DmsSendCstVclInfoRow, _
                                                   ByVal reserveInfoRow As IC3800903DmsSendReserveInfoRow, _
                                                   ByVal otherInfoRow As IC3800903DmsSendOtherInfoRow, _
                                                   ByVal nowDateTime As Date, _
                                                   ByVal cdataApdFlg As String) As XmlElement

        'Logger.Info(String.Format(CultureInfo.InvariantCulture, _
        '                          "{0}_S ", _
        '                          MethodBase.GetCurrentMethod.Name))

        'UpdateReserveInfomationタグを作成
        Dim updateReserveInfoTag As XmlElement = xmlDocument.CreateElement(TagUpdateReserveInformation)

        Try
            'Reserve_CustomerInformationタグを構築
            Dim reserveCustomerInfoTag As XmlElement _
                = Me.StructSendReserveXmlReserveCustomerInformationTag(xmlDocument,
                                                                       cstVclInfoRow, _
                                                                       cdataApdFlg)

            If String.IsNullOrEmpty(reserveCustomerInfoTag.InnerXml) Then
                '必須チェックエラー
                updateReserveInfoTag.InnerXml = String.Empty
                Exit Try
            End If

            'Reserve_VehicleInformationタグを構築
            Dim reserveVehicleInfoTag As XmlElement _
                = Me.StructSendReserveXmlReserveVehicleInformationTag(xmlDocument, _
                                                                      cstVclInfoRow, _
                                                                      reserveInfoRow, _
                                                                      cdataApdFlg)

            If String.IsNullOrEmpty(reserveVehicleInfoTag.InnerXml) Then
                '必須チェックエラー
                updateReserveInfoTag.InnerXml = String.Empty
                Exit Try
            End If

            'Reserve_ServiceInformationタグを構築
            Dim reserveServiceInfoTag As XmlElement _
                = Me.StructSendReserveXmlReserveServiceInformationTag(xmlDocument, _
                                                                      reserveInfoRow, _
                                                                      otherInfoRow, _
                                                                      cdataApdFlg)

            If String.IsNullOrEmpty(reserveServiceInfoTag.InnerXml) Then
                '必須チェックエラー
                updateReserveInfoTag.InnerXml = String.Empty
                Exit Try
            End If

            'タグに設定する値を全てローカル変数に格納
            Dim dmsDealerCode As String = otherInfoRow.DMS_DLR_CD
            Dim dmsBranchCode As String = otherInfoRow.DMS_BRN_CD
            Dim sequenceNo As String = nowDateTime.ToString(SeqNoNumberingFormat, CultureInfo.CurrentCulture)
            Dim reserveId As String = reserveInfoRow.JOB_DTL_ID.ToString(CultureInfo.CurrentCulture)
            Dim basReserveId As String = reserveInfoRow.DMS_JOB_DTL_ID.Trim()
            Dim pReserveId As String = otherInfoRow.PREZID
            Dim reserveChildNo As String = otherInfoRow.REZ_CHILDNO
            Dim memo As String = reserveInfoRow.JOB_DTL_MEMO.Trim()
            Dim saCode As String = Me.SplitStaffCd(cstVclInfoRow.SVC_PIC_STF_CD.Trim())
            Dim accountPlan As String = Me.SplitStaffCd(reserveInfoRow.PIC_SA_STF_CD.Trim())
            Dim inputAccount As String = Me.SplitStaffCd(reserveInfoRow.CREATE_STF_CD.Trim())
            Dim updateAccount As String = Me.SplitStaffCd(reserveInfoRow.UPDATE_STF_CD.Trim())
            Dim status As String = reserveInfoRow.RESV_STATUS.Trim()
            Dim walkin As String = reserveInfoRow.ACCEPTANCE_TYPE.Trim()
            Dim smsFlg As String = reserveInfoRow.SMS_TRANSMISSION_FLG.Trim()
            Dim reserveType As String = reserveInfoRow.SVCIN_CREATE_TYPE.Trim()
            Dim cancelFlg As String = otherInfoRow.CANCELFLGVALUE
            Dim createDate As String = reserveInfoRow.CREATE_DATETIME.ToString(yyyyMMddHHmmssDateFormat, CultureInfo.CurrentCulture)
            Dim updateDate As String = reserveInfoRow.UPDATE_DATETIME.ToString(yyyyMMddHHmmssDateFormat, CultureInfo.CurrentCulture)

            'UpdateReserveInfomationタグの子要素を作成
            Dim dealerCodeTag As XmlElement = xmlDocument.CreateElement(TagDealerCode)
            Dim branchCodeTag As XmlElement = xmlDocument.CreateElement(TagBranchCode)
            Dim seqNoTag As XmlElement = xmlDocument.CreateElement(TagSeqNo)
            Dim rezIdTag As XmlElement = xmlDocument.CreateElement(TagRezId)
            Dim basRezIdTag As XmlElement = xmlDocument.CreateElement(TagBasRezId)
            Dim pRezIdTag As XmlElement = xmlDocument.CreateElement(TagPrezId)
            Dim rezChildNoTag As XmlElement = xmlDocument.CreateElement(TagRezChildNo)
            Dim memoTag As XmlElement = xmlDocument.CreateElement(TagMemo)
            Dim saCodeTag As XmlElement = xmlDocument.CreateElement(TagSaCode)
            Dim accountPlanTag As XmlElement = xmlDocument.CreateElement(TagAccountPlan)
            Dim inputAccountTag As XmlElement = xmlDocument.CreateElement(TagInputAccount)
            Dim updateAccountTag As XmlElement = xmlDocument.CreateElement(TagUpdateAccount)
            Dim statusTag As XmlElement = xmlDocument.CreateElement(TagStatus)
            Dim walkInTag As XmlElement = xmlDocument.CreateElement(TagWalkIn)
            Dim smsFlgTag As XmlElement = xmlDocument.CreateElement(TagSmsFlg)
            Dim rezTypeTag As XmlElement = xmlDocument.CreateElement(TagRezType)
            Dim cancelFlgTag As XmlElement = xmlDocument.CreateElement(TagCancelFlg)
            Dim createDateTag As XmlElement = xmlDocument.CreateElement(TagCreateDate)
            Dim updateDateTag As XmlElement = xmlDocument.CreateElement(TagUpdateDate)

            '子要素に値を設定
            dealerCodeTag.AppendChild(xmlDocument.CreateTextNode(dmsDealerCode))
            branchCodeTag.AppendChild(xmlDocument.CreateTextNode(dmsBranchCode))
            seqNoTag.AppendChild(xmlDocument.CreateTextNode(sequenceNo))
            rezIdTag.AppendChild(xmlDocument.CreateTextNode(reserveId))
            basRezIdTag.AppendChild(xmlDocument.CreateTextNode(basReserveId))
            pRezIdTag.AppendChild(xmlDocument.CreateTextNode(pReserveId))
            rezChildNoTag.AppendChild(xmlDocument.CreateTextNode(reserveChildNo))
            'memoTag.AppendChild(xmlDocument.CreateTextNode(memo))
            saCodeTag.AppendChild(xmlDocument.CreateTextNode(saCode))
            accountPlanTag.AppendChild(xmlDocument.CreateTextNode(accountPlan))
            inputAccountTag.AppendChild(xmlDocument.CreateTextNode(inputAccount))
            updateAccountTag.AppendChild(xmlDocument.CreateTextNode(updateAccount))
            statusTag.AppendChild(xmlDocument.CreateTextNode(status))
            walkInTag.AppendChild(xmlDocument.CreateTextNode(walkin))
            smsFlgTag.AppendChild(xmlDocument.CreateTextNode(smsFlg))
            rezTypeTag.AppendChild(xmlDocument.CreateTextNode(reserveType))
            cancelFlgTag.AppendChild(xmlDocument.CreateTextNode(cancelFlg))
            createDateTag.AppendChild(xmlDocument.CreateTextNode(createDate))
            updateDateTag.AppendChild(xmlDocument.CreateTextNode(updateDate))

            If AppendCData.Equals(cdataApdFlg) Then
                'CDATAを付与する場合
                memoTag.AppendChild(xmlDocument.CreateCDataSection(memo))
            Else
                'CDATAを付与しない場合
                memoTag.AppendChild(xmlDocument.CreateTextNode(memo))
            End If

            'UpdateReserveInfomationタグを構築
            With updateReserveInfoTag
                .AppendChild(reserveCustomerInfoTag)
                .AppendChild(reserveVehicleInfoTag)
                .AppendChild(reserveServiceInfoTag)
                .AppendChild(dealerCodeTag)
                .AppendChild(branchCodeTag)
                .AppendChild(seqNoTag)
                .AppendChild(rezIdTag)
                .AppendChild(basRezIdTag)
                .AppendChild(pRezIdTag)
                .AppendChild(rezChildNoTag)
                .AppendChild(memoTag)
                .AppendChild(saCodeTag)
                .AppendChild(accountPlanTag)
                .AppendChild(inputAccountTag)
                .AppendChild(updateAccountTag)
                .AppendChild(statusTag)
                .AppendChild(walkInTag)
                .AppendChild(smsFlgTag)
                .AppendChild(rezTypeTag)
                .AppendChild(cancelFlgTag)
                .AppendChild(createDateTag)
                .AppendChild(updateDateTag)
            End With

        Finally
            'Logger.Info(String.Format(CultureInfo.InvariantCulture, _
            '                          "{0}_E OUT:updateReserveInfoTag={1}", _
            '                          MethodBase.GetCurrentMethod.Name, _
            '                          updateReserveInfoTag.InnerXml))
        End Try

        Return updateReserveInfoTag

    End Function

    ''' <summary>
    ''' 予約送信用XMLのReserve_CustomerInformationタグを構築する
    ''' </summary>
    ''' <param name="xmlDocument">予約送信用XMLドキュメント</param>
    ''' <param name="cstVclInfoRow">顧客、車両情報データ行</param>
    ''' <param name="cdataApdFlg">CDATA付与フラグ</param>
    ''' <returns>Reserve_CustomerInformationタグエレメント</returns>
    ''' <remarks></remarks>
    Private Function StructSendReserveXmlReserveCustomerInformationTag( _
                                                   ByVal xmlDocument As XmlDocument, _
                                                   ByVal cstVclInfoRow As IC3800903DmsSendCstVclInfoRow, _
                                                   ByVal cdataApdFlg As String) As XmlElement

        'Logger.Info(String.Format(CultureInfo.InvariantCulture, _
        '                          "{0}_S ", _
        '                          MethodBase.GetCurrentMethod.Name))

        'Reserve_CustomerInformationタグを作成
        Dim reserveCustomerInfoTag As XmlElement = xmlDocument.CreateElement(TagReserveCustomerInformation)

        'タグに設定する値を全てローカル変数に格納
        Dim customerCode As String = Me.TrimBeforeAtmark(cstVclInfoRow.DMS_CST_CD)
        Dim newCustomerId As String = cstVclInfoRow.NEWCST_CD.Trim()
        'Dim customerClass As String = cstVclInfoRow.CST_VCL_TYPE.Trim()
        Dim customerClass As String = IIf(CustomerTypeNew.Equals(cstVclInfoRow.CST_TYPE), _
                                          CustomerVehicleTypeNew, _
                                          cstVclInfoRow.CST_VCL_TYPE).ToString()
        Dim customerName As String = cstVclInfoRow.CST_NAME.Trim()
        Dim telNo As String = cstVclInfoRow.CST_PHONE.Trim()
        Dim mobile As String = cstVclInfoRow.CST_MOBILE.Trim()
        Dim eMail As String = cstVclInfoRow.CST_EMAIL_1.Trim()
        Dim zipCode As String = cstVclInfoRow.CST_ZIPCD.Trim()
        Dim address As String = cstVclInfoRow.CST_ADDRESS.Trim()

        '電話番号・携帯電話両方とも入っていない場合は電話番号をハイフンに設定する
        If String.IsNullOrEmpty(telNo) And String.IsNullOrEmpty(mobile) Then
            telNo = Hyphen
        End If

        If Me.CheckNecessaryReserveCustomerInformationTag(customerName) Then
            '必須チェックOK

            'Reserve_CustomerInformationタグの子要素を作成
            Dim custCDTag As XmlElement = xmlDocument.CreateElement(TagCustCode)
            Dim newCustmerIdTag As XmlElement = xmlDocument.CreateElement(TagNewCustomerId)
            Dim customerClassTag As XmlElement = xmlDocument.CreateElement(TagCustomerClass)
            Dim customerNameTag As XmlElement = xmlDocument.CreateElement(TagCustomerName)
            Dim telNoTag As XmlElement = xmlDocument.CreateElement(TagTelNo)
            Dim mobileTag As XmlElement = xmlDocument.CreateElement(TagMobile)
            Dim eMailTag As XmlElement = xmlDocument.CreateElement(TagEmail)
            Dim zipCodeTag As XmlElement = xmlDocument.CreateElement(TagZipCode)
            Dim addressTag As XmlElement = xmlDocument.CreateElement(TagAddress)

            '子要素に値を設定
            custCDTag.AppendChild(xmlDocument.CreateTextNode(customerCode))
            newCustmerIdTag.AppendChild(xmlDocument.CreateTextNode(newCustomerId))
            customerClassTag.AppendChild(xmlDocument.CreateTextNode(customerClass))
            'customerNameTag.AppendChild(xmlDocument.CreateTextNode(customerName))
            telNoTag.AppendChild(xmlDocument.CreateTextNode(telNo))
            mobileTag.AppendChild(xmlDocument.CreateTextNode(mobile))
            eMailTag.AppendChild(xmlDocument.CreateTextNode(eMail))
            zipCodeTag.AppendChild(xmlDocument.CreateTextNode(zipCode))
            'addressTag.AppendChild(xmlDocument.CreateTextNode(address))

            If AppendCData.Equals(cdataApdFlg) Then
                'CDATAを付与する場合
                customerNameTag.AppendChild(xmlDocument.CreateCDataSection(customerName))
                addressTag.AppendChild(xmlDocument.CreateCDataSection(address))
            Else
                'CDATAを付与しない場合
                customerNameTag.AppendChild(xmlDocument.CreateTextNode(customerName))
                addressTag.AppendChild(xmlDocument.CreateTextNode(address))
            End If

            'Reserve_CustomerInformationタグを構築
            With reserveCustomerInfoTag
                .AppendChild(custCDTag)
                .AppendChild(newCustmerIdTag)
                .AppendChild(customerClassTag)
                .AppendChild(customerNameTag)
                .AppendChild(telNoTag)
                .AppendChild(mobileTag)
                .AppendChild(eMailTag)
                .AppendChild(zipCodeTag)
                .AppendChild(addressTag)
            End With

        Else
            '必須チェックNG

            reserveCustomerInfoTag.InnerXml = String.Empty

        End If

        'Logger.Info(String.Format(CultureInfo.InvariantCulture, _
        '                          "{0}_E OUT:reserveCustomerInfoTag={1}", _
        '                          MethodBase.GetCurrentMethod.Name, reserveCustomerInfoTag.InnerXml))

        Return reserveCustomerInfoTag

    End Function

    ''' <summary>
    ''' 予約送信用XMLのReserve_VehicleInformationタグを構築する
    ''' </summary>
    ''' <param name="xmlDocument">予約送信用XMLドキュメント</param>
    ''' <param name="cstVclInfoRow">顧客、車両情報データ行</param>
    ''' <param name="reserveInfoRow">予約情報データ行</param>
    ''' <param name="cdataApdFlg">CDATA付与フラグ</param>
    ''' <returns>Reserve_VehicleInformationタグエレメント</returns>
    ''' <remarks></remarks>
    Private Function StructSendReserveXmlReserveVehicleInformationTag( _
                                           ByVal xmlDocument As XmlDocument, _
                                           ByVal cstVclInfoRow As IC3800903DmsSendCstVclInfoRow, _
                                           ByVal reserveInfoRow As IC3800903DmsSendReserveInfoRow, _
                                           ByVal cdataApdFlg As String) As XmlElement

        'Logger.Info(String.Format(CultureInfo.InvariantCulture, _
        '                          "{0}_S ", _
        '                          MethodBase.GetCurrentMethod.Name))

        'Reserve_VehicleInformationタグを作成
        Dim reserveVehicleInfoTag As XmlElement = xmlDocument.CreateElement(TagReserveVehicleInformation)

        'タグに設定する値を全てローカル変数に格納
        Dim vehicleRegNo As String = cstVclInfoRow.REG_NUM.Trim()
        Dim vin As String = Me.GetVin(cstVclInfoRow.VCL_VIN, cstVclInfoRow.REG_NUM, cstVclInfoRow.VCL_VIN_SALESBOOKING)
        Dim seriesCode As String = cstVclInfoRow.MODEL_CD.Trim()
        Dim seriesName As String = Me.EmptyToHyphen(cstVclInfoRow.MODEL_NAME.Trim())
        Dim baseType As String = Me.TrimAfterHyphen(cstVclInfoRow.VCL_KATASHIKI.Trim())
        Dim mileage As String = reserveInfoRow.SVCIN_MILE.ToString(CultureInfo.CurrentCulture)

        If Me.CheckNecessaryReserveVehicleInformationTag(vin, vehicleRegNo, seriesName) Then
            '必須チェックOK

            'Reserve_VehicleInformationタグの子要素を作成
            Dim vclRegNoTag As XmlElement = xmlDocument.CreateElement(TagVclRegNo)
            Dim vinTag As XmlElement = xmlDocument.CreateElement(TagVin2)
            Dim makerCodeTag As XmlElement = xmlDocument.CreateElement(TagMakerCode)
            Dim seriesCodeTag As XmlElement = xmlDocument.CreateElement(TagSeriesCode)
            Dim seriesNameTag As XmlElement = xmlDocument.CreateElement(TagSeriesName)
            Dim baseTypeTag As XmlElement = xmlDocument.CreateElement(TagBaseType)
            Dim mileageTag As XmlElement = xmlDocument.CreateElement(TagMileage)

            '子要素に値を設定
            'vclRegNoTag.AppendChild(xmlDocument.CreateTextNode(vehicleRegNo))
            vinTag.AppendChild(xmlDocument.CreateTextNode(vin))
            makerCodeTag.AppendChild(xmlDocument.CreateTextNode(String.Empty))
            seriesCodeTag.AppendChild(xmlDocument.CreateTextNode(seriesCode))
            'seriesNameTag.AppendChild(xmlDocument.CreateTextNode(seriesName))
            baseTypeTag.AppendChild(xmlDocument.CreateTextNode(baseType))
            mileageTag.AppendChild(xmlDocument.CreateTextNode(mileage))

            If AppendCData.Equals(cdataApdFlg) Then
                'CDATAを付与する場合
                vclRegNoTag.AppendChild(xmlDocument.CreateCDataSection(vehicleRegNo))
                seriesNameTag.AppendChild(xmlDocument.CreateCDataSection(seriesName))
            Else
                'CDATAを付与しない場合
                vclRegNoTag.AppendChild(xmlDocument.CreateTextNode(vehicleRegNo))
                seriesNameTag.AppendChild(xmlDocument.CreateTextNode(seriesName))
            End If

            'Reserve_VehicleInformationタグを構築
            With reserveVehicleInfoTag
                .AppendChild(vclRegNoTag)
                .AppendChild(vinTag)
                .AppendChild(makerCodeTag)
                .AppendChild(seriesCodeTag)
                .AppendChild(seriesNameTag)
                .AppendChild(baseTypeTag)
                .AppendChild(mileageTag)
            End With

        Else
            '必須チェックNG

            reserveVehicleInfoTag.InnerXml = String.Empty

        End If

        'Logger.Info(String.Format(CultureInfo.InvariantCulture, _
        '                          "{0}_E OUT:reserveVehicleInfoTag={1}", _
        '                          MethodBase.GetCurrentMethod.Name, reserveVehicleInfoTag.InnerXml))

        Return reserveVehicleInfoTag

    End Function

    ''' <summary>
    ''' 予約送信用XMLのReserve_ServiceInformationタグを構築する
    ''' </summary>
    ''' <param name="xmlDocument">予約送信用XMLドキュメント</param>
    ''' <param name="reserveInfoRow">予約情報データ行</param>
    ''' <param name="otherInfoRow">その他情報データ行</param>
    ''' <param name="cdataApdFlg">CDATA付与フラグ</param>
    ''' <returns>Reserve_ServiceInformationタグエレメント</returns>
    ''' <remarks></remarks>
    Private Function StructSendReserveXmlReserveServiceInformationTag( _
                                           ByVal xmlDocument As XmlDocument, _
                                           ByVal reserveInfoRow As IC3800903DmsSendReserveInfoRow, _
                                           ByVal otherInfoRow As IC3800903DmsSendOtherInfoRow, _
                                           ByVal cdataApdFlg As String) As XmlElement

        'Logger.Info(String.Format(CultureInfo.InvariantCulture, _
        '                          "{0}_S ", _
        '                          MethodBase.GetCurrentMethod.Name))

        'Reserve_ServiceInformationタグを構築
        Dim reserveServiceInfoTag As XmlElement = xmlDocument.CreateElement(TagReserveServiceInformation)

        'タグに設定する値を全てローカル変数に格納

        'STALLID:
        '0(DBデフォルト値)の場合、空文字を設定
        Dim stallId As String = IIf(otherInfoRow.DMS_STALLID.Equals("0"), String.Empty, otherInfoRow.DMS_STALLID.Trim()).ToString()

        'STARTTIME、ENDTIME:
        'STALLID値が空文字の場合、空文字を設定
        'それ以外は「yyyy/MM/dd HH:mm:ss」のフォーマットで設定
        Dim startTime As String = IIf(String.IsNullOrEmpty(stallId), String.Empty, reserveInfoRow.SCHE_START_DATETIME.ToString(yyyyMMddHHmmssDateFormat, CultureInfo.CurrentCulture)).ToString()
        Dim endTime As String = IIf(String.IsNullOrEmpty(stallId), String.Empty, reserveInfoRow.SCHE_END_DATETIME.ToString(yyyyMMddHHmmssDateFormat, CultureInfo.CurrentCulture)).ToString()

        Dim workTime As String = reserveInfoRow.SCHE_WORKTIME.ToString(CultureInfo.CurrentCulture)
        Dim washFlg As String = reserveInfoRow.CARWASH_NEED_FLG
        Dim inspectionFlg As String = reserveInfoRow.INSPECTION_NEED_FLG
        '2014/04/23 TMEJ 明瀬 サービスタブレットDMS連携コード変換対応 START
        'Dim merchandiseCode As String = IIf((reserveInfoRow.MERC_ID = 0), String.Empty, reserveInfoRow.MERC_ID).ToString()
        Dim merchandiseCode As String = reserveInfoRow.MERC_CD.Trim()
        '2014/04/23 TMEJ 明瀬 サービスタブレットDMS連携コード変換対応 END
        Dim maintenanceCode As String = reserveInfoRow.MAINTE_CD.Trim()
        Dim serviceCode As String = reserveInfoRow.SVC_CLASS_CD.Trim()
        Dim serviceName As String = reserveInfoRow.SVC_CLASS_NAME.Trim()
        Dim rezReception As String = reserveInfoRow.PICK_DELI_TYPE.Trim()

        'REZ_PICK_DATE、REZ_PICK_LOC、REZ_PICK_TIME:
        Dim rezPickDate As String
        Dim rezPickLoc As String
        Dim rezPickTime As String

        If reserveInfoRow.IsPICK_PREF_DATETIMENull Then
            '引取希望日時がNULL(車両引取テーブルに該当データなし)の場合
            rezPickDate = reserveInfoRow.SCHE_SVCIN_DATETIME.ToString(yyyyMMddHHmmssDateFormat, _
                                                                      CultureInfo.CurrentCulture)

            If yyyyMMddHHmmssMinDate.Equals(rezPickDate) Then
                'DBデフォルト値の場合、空文字に変換する
                rezPickDate = String.Empty
            End If

            rezPickLoc = String.Empty
            rezPickTime = String.Empty
        Else
            '引取希望日時があり(車両引取テーブルに該当データあり)の場合
            rezPickDate = reserveInfoRow.PICK_PREF_DATETIME.ToString(yyyyMMddHHmmssDateFormat, _
                                                                     CultureInfo.CurrentCulture)
            rezPickLoc = reserveInfoRow.PICK_DESTINATION.Trim()
            rezPickTime = reserveInfoRow.PICK_WORKTIME.ToString(CultureInfo.CurrentCulture)
        End If

        'REZ_DELI_DATE、REZ_DELI_LOC、REZ_DELI_TIME:
        Dim rezDeliDate As String
        Dim rezDeliLoc As String
        Dim rezDeliTime As String

        If reserveInfoRow.IsDELI_PREF_DATETIMENull Then
            '配送希望日時がNULL(車両配送テーブルに該当データなし)の場合
            rezDeliDate = reserveInfoRow.SCHE_DELI_DATETIME.ToString(yyyyMMddHHmmssDateFormat, _
                                                                     CultureInfo.CurrentCulture)

            If yyyyMMddHHmmssMinDate.Equals(rezDeliDate) Then
                'DBデフォルト値の場合、空文字に変換する
                rezDeliDate = String.Empty
            End If

            rezDeliLoc = String.Empty
            rezDeliTime = String.Empty
        Else
            '配送希望日時があり(車両配送テーブルに該当データあり)の場合
            rezDeliDate = reserveInfoRow.DELI_PREF_DATETIME.ToString(yyyyMMddHHmmssDateFormat, _
                                                                     CultureInfo.CurrentCulture)
            rezDeliLoc = reserveInfoRow.DELI_DESTINATION.Trim()
            rezDeliTime = reserveInfoRow.DELI_WORKTIME.ToString(CultureInfo.CurrentCulture)
        End If

        If Me.CheckNecessaryReserveServiceInformationTag(workTime, rezReception, stallId, startTime, endTime) Then
            '必須チェックOK

            'Reserve_ServiceInformationタグの子要素を作成
            Dim stallIdTag As XmlElement = xmlDocument.CreateElement(TagStallId)
            Dim startTimeTag As XmlElement = xmlDocument.CreateElement(TagStartTime)
            Dim endTimeTag As XmlElement = xmlDocument.CreateElement(TagEndTime)
            Dim workTimeTag As XmlElement = xmlDocument.CreateElement(TagWorkTime)
            Dim washFlgTag As XmlElement = xmlDocument.CreateElement(TagWashFlg)
            Dim inspectionFlgTag As XmlElement = xmlDocument.CreateElement(TagInspectionFlg)
            Dim merchandiseCodeTag As XmlElement = xmlDocument.CreateElement(TagMerchandiseCode)
            Dim mntnCodeTag As XmlElement = xmlDocument.CreateElement(TagMntnCode)
            Dim serviceCodeTag As XmlElement = xmlDocument.CreateElement(TagServiceCode)
            Dim serviceNameTag As XmlElement = xmlDocument.CreateElement(TagServiceName)
            Dim rezReceptionTag As XmlElement = xmlDocument.CreateElement(TagRezReception)
            Dim rezPickDateTag As XmlElement = xmlDocument.CreateElement(TagRezPickDate)
            Dim rezPickLocTag As XmlElement = xmlDocument.CreateElement(TagRezPickLoc)
            Dim rezPickTimeTag As XmlElement = xmlDocument.CreateElement(TagRezPickTime)
            Dim rezDeliDateTag As XmlElement = xmlDocument.CreateElement(TagRezDeliDate)
            Dim rezDeliLocTag As XmlElement = xmlDocument.CreateElement(TagRezDeliLoc)
            Dim rezDeliTimeTag As XmlElement = xmlDocument.CreateElement(TagRezDeliTime)

            '子要素に値を設定
            stallIdTag.AppendChild(xmlDocument.CreateTextNode(stallId))
            startTimeTag.AppendChild(xmlDocument.CreateTextNode(startTime))
            endTimeTag.AppendChild(xmlDocument.CreateTextNode(endTime))
            workTimeTag.AppendChild(xmlDocument.CreateTextNode(workTime))
            washFlgTag.AppendChild(xmlDocument.CreateTextNode(washFlg))
            inspectionFlgTag.AppendChild(xmlDocument.CreateTextNode(inspectionFlg))
            merchandiseCodeTag.AppendChild(xmlDocument.CreateTextNode(merchandiseCode))
            mntnCodeTag.AppendChild(xmlDocument.CreateTextNode(maintenanceCode))
            serviceCodeTag.AppendChild(xmlDocument.CreateTextNode(serviceCode))
            'serviceNameTag.AppendChild(xmlDocument.CreateTextNode(serviceName))
            rezReceptionTag.AppendChild(xmlDocument.CreateTextNode(rezReception))
            rezPickDateTag.AppendChild(xmlDocument.CreateTextNode(rezPickDate))
            rezPickLocTag.AppendChild(xmlDocument.CreateTextNode(rezPickLoc))
            rezPickTimeTag.AppendChild(xmlDocument.CreateTextNode(rezPickTime))
            rezDeliDateTag.AppendChild(xmlDocument.CreateTextNode(rezDeliDate))
            rezDeliLocTag.AppendChild(xmlDocument.CreateTextNode(rezDeliLoc))
            rezDeliTimeTag.AppendChild(xmlDocument.CreateTextNode(rezDeliTime))

            If AppendCData.Equals(cdataApdFlg) Then
                'CDATAを付与する場合
                serviceNameTag.AppendChild(xmlDocument.CreateCDataSection(serviceName))
            Else
                'CDATAを付与しない場合
                serviceNameTag.AppendChild(xmlDocument.CreateTextNode(serviceName))
            End If

            'Reserve_ReserveInformationタグを構築
            With reserveServiceInfoTag
                .AppendChild(stallIdTag)
                .AppendChild(startTimeTag)
                .AppendChild(endTimeTag)
                .AppendChild(workTimeTag)
                .AppendChild(washFlgTag)
                .AppendChild(inspectionFlgTag)
                .AppendChild(merchandiseCodeTag)
                .AppendChild(mntnCodeTag)
                .AppendChild(serviceCodeTag)
                .AppendChild(serviceNameTag)
                .AppendChild(rezReceptionTag)
                .AppendChild(rezPickDateTag)
                .AppendChild(rezPickLocTag)
                .AppendChild(rezPickTimeTag)
                .AppendChild(rezDeliDateTag)
                .AppendChild(rezDeliLocTag)
                .AppendChild(rezDeliTimeTag)
            End With

        Else
            '必須チェックNG

            reserveServiceInfoTag.InnerXml = String.Empty

        End If

        'Logger.Info(String.Format(CultureInfo.InvariantCulture, _
        '                          "{0}_E OUT:reserveServiceInfoTag={1}", _
        '                          MethodBase.GetCurrentMethod.Name, reserveServiceInfoTag.InnerXml))

        Return reserveServiceInfoTag

    End Function

#End Region

#Region "予約送信用XMLの送受信"

    ''' <summary>
    ''' WebServiceにXMLを送信し、結果を受信する
    ''' </summary>
    ''' <param name="sendXml">送信XML文字列</param>
    ''' <param name="webServiceUrl">送信先URL</param>
    ''' <param name="timeOutValue">タイムアウト値</param>
    ''' <param name="soapVersion">SOAPバージョン判定値</param>
    ''' <param name="distCode">国コード</param>
    ''' <returns>受信XML文字列</returns>
    ''' <remarks></remarks>
    Private Function ExecuteSendReserveXml(ByVal sendXml As String, _
                                           ByVal webServiceUrl As String, _
                                           ByVal timeOutValue As String, _
                                           ByVal soapVersion As String, _
                                           ByVal distCode As String) As String

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_S IN:sendXml={1}, webServiceUrl={2}, timeOutValue={3}, soapVersion={4}, distCode={5}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  sendXml, _
                                  webServiceUrl, _
                                  timeOutValue, _
                                  soapVersion, _
                                  distCode))

        Dim returnXml As String = String.Empty

        If DistCodeTH.Equals(distCode) Then
            '国コードがTHの場合
            returnXml = SendToTH(sendXml, webServiceUrl, timeOutValue, soapVersion)
        Else
            'それ以外の場合
            returnXml = SendToOther(sendXml, webServiceUrl, timeOutValue, soapVersion)
        End If

        Return returnXml

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_E", _
                                  MethodBase.GetCurrentMethod.Name))

    End Function

    ''' <summary>
    ''' TH(タイ)のSOAPクライアントを使用して予約送信を行う
    ''' </summary>
    ''' <param name="sendXml">送信XML文字列</param>
    ''' <param name="webServiceUrl">送信先URL</param>
    ''' <param name="timeOutValue">タイムアウト値</param>
    ''' <param name="soapVersion">SOAPバージョン判定値</param>
    ''' <returns>返信XML</returns>
    ''' <remarks></remarks>
    Private Function SendToTH(ByVal sendXml As String, _
                              ByVal webServiceUrl As String, _
                              ByVal timeOutValue As String, _
                              ByVal soapVersion As String) As String

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_S", _
                                  MethodBase.GetCurrentMethod.Name))

        'SoapClientインスタンスを生成
        Using service As New UpdateReserveImportDMSSoapService

            service.Url = webServiceUrl
            service.Timeout = CType(timeOutValue, Integer)

            If soapVersion.Equals("1") Then
                'SOAPバージョンを1.1に設定
                service.SoapVersion = Services.Protocols.SoapProtocolVersion.Soap11

            ElseIf soapVersion.Equals("2") Then
                'SOAPバージョンを1.2に設定
                service.SoapVersion = Services.Protocols.SoapProtocolVersion.Soap12

            End If

            Dim resultString As String = String.Empty

            Try
                '2015/07/17 TMEJ 明瀬 タブレットSMB性能改善 ログ出力強化対応 START

                Dim sendReserveLogFlg As String = "0"

                Using svcCommonClassBiz As New ServiceCommonClassBusinessLogic

                    sendReserveLogFlg = svcCommonClassBiz.GetSystemSettingValueBySettingName("SEND_RESERVE_LOG_FLG")

                End Using

                If "1".Equals(sendReserveLogFlg) Then
                    If ReceptonChipPlacementFlg Then
                        Logger.Info( _
                            String.Format(CultureInfo.CurrentCulture, _
                            "{0}.{1} ⑩-1 SC3240301_受付エリアからのチップ配置処理 [予約連携送信] START", _
                            Me.GetType.ToString, _
                            System.Reflection.MethodBase.GetCurrentMethod.Name))
                    End If
                End If

                '2015/07/17 TMEJ 明瀬 タブレットSMB性能改善 ログ出力強化対応 END

                'XML送信
                resultString = service.IC45201(sendXml)

                '2015/07/17 TMEJ 明瀬 タブレットSMB性能改善 ログ出力強化対応 START

                If "1".Equals(sendReserveLogFlg) Then
                    If ReceptonChipPlacementFlg Then
                        Logger.Info( _
                            String.Format(CultureInfo.CurrentCulture, _
                            "{0}.{1} ⑩-1 SC3240301_受付エリアからのチップ配置処理 [予約連携送信] END", _
                            Me.GetType.ToString, _
                            System.Reflection.MethodBase.GetCurrentMethod.Name))
                    End If
                End If

                '2015/07/17 TMEJ 明瀬 タブレットSMB性能改善 ログ出力強化対応 END

            Catch webEx As WebException

                If webEx.Status = WebExceptionStatus.Timeout Then
                    'タイムアウトが発生した場合
                    Logger.Error(String.Format(CultureInfo.InvariantCulture, _
                                               "{0}_Error ErrorCode:{1}, Timeout error occurred.", _
                                               MethodBase.GetCurrentMethod.Name, _
                                               ErrorTimeOut), webEx)
                Else
                    'それ以外のネットワークエラー
                    Logger.Error(String.Format(CultureInfo.InvariantCulture, _
                                               "{0}_Error ErrorCode:{1}", _
                                               MethodBase.GetCurrentMethod.Name, _
                                               ErrorNetwork), webEx)

                End If

                resultString = String.Empty

            End Try

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}_E OUT:resultString={1}", _
                                      MethodBase.GetCurrentMethod.Name, _
                                      resultString))

            Return resultString

        End Using

    End Function

    ''' <summary>
    ''' TH(タイ)、KR(韓国)以外のSOAPクライアントを使用して予約送信を行う
    ''' </summary>
    ''' <param name="sendXml">送信XML文字列</param>
    ''' <param name="webServiceUrl">送信先URL</param>
    ''' <param name="timeOutValue">タイムアウト値</param>
    ''' <param name="soapVersion">SOAPバージョン判定値</param>
    ''' <returns>返信XML</returns>
    ''' <remarks></remarks>
    Private Function SendToOther(ByVal sendXml As String, _
                                 ByVal webServiceUrl As String, _
                                 ByVal timeOutValue As String, _
                                 ByVal soapVersion As String) As String

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_S", _
                                  MethodBase.GetCurrentMethod.Name))

        'SoapClientインスタンスを生成
        Using service As New UpdateReserveImportDMS

            service.Url = webServiceUrl
            service.Timeout = CType(timeOutValue, Integer)

            If soapVersion.Equals("1") Then
                'SOAPバージョンを1.1に設定
                service.SoapVersion = Services.Protocols.SoapProtocolVersion.Soap11

            ElseIf soapVersion.Equals("2") Then
                'SOAPバージョンを1.2に設定
                service.SoapVersion = Services.Protocols.SoapProtocolVersion.Soap12

            End If

            Dim resultString As String = String.Empty

            Try
                '2015/07/17 TMEJ 明瀬 タブレットSMB性能改善 ログ出力強化対応 START

                Dim sendReserveLogFlg As String = "0"

                Using svcCommonClassBiz As New ServiceCommonClassBusinessLogic

                    sendReserveLogFlg = svcCommonClassBiz.GetSystemSettingValueBySettingName("SEND_RESERVE_LOG_FLG")

                End Using

                If "1".Equals(sendReserveLogFlg) Then
                    If ReceptonChipPlacementFlg Then
                        Logger.Info( _
                            String.Format(CultureInfo.CurrentCulture, _
                            "{0}.{1} ⑩-1 SC3240301_受付エリアからのチップ配置処理 [予約連携送信] START", _
                            Me.GetType.ToString, _
                            System.Reflection.MethodBase.GetCurrentMethod.Name))
                    End If
                End If

                '2015/07/17 TMEJ 明瀬 タブレットSMB性能改善 ログ出力強化対応 END

                'XML送信
                resultString = service.IC45201(sendXml)

                '2015/07/17 TMEJ 明瀬 タブレットSMB性能改善 ログ出力強化対応 START

                If "1".Equals(sendReserveLogFlg) Then
                    If ReceptonChipPlacementFlg Then
                        Logger.Info( _
                            String.Format(CultureInfo.CurrentCulture, _
                            "{0}.{1} ⑩-1 SC3240301_受付エリアからのチップ配置処理 [予約連携送信] END", _
                            Me.GetType.ToString, _
                            System.Reflection.MethodBase.GetCurrentMethod.Name))
                    End If
                End If

                '2015/07/17 TMEJ 明瀬 タブレットSMB性能改善 ログ出力強化対応 END

            Catch webEx As WebException

                If webEx.Status = WebExceptionStatus.Timeout Then
                    'タイムアウトが発生した場合
                    Logger.Error(String.Format(CultureInfo.InvariantCulture, _
                                               "{0}_Error ErrorCode:{1}, Timeout error occurred.", _
                                               MethodBase.GetCurrentMethod.Name, _
                                               ErrorTimeOut), webEx)
                Else
                    'それ以外のネットワークエラー
                    Logger.Error(String.Format(CultureInfo.InvariantCulture, _
                                               "{0}_Error ErrorCode:{1}", _
                                               MethodBase.GetCurrentMethod.Name, _
                                               ErrorNetwork), webEx)

                End If

                resultString = String.Empty

            End Try

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}_E OUT:resultString={1}", _
                                      MethodBase.GetCurrentMethod.Name, _
                                      resultString))

            Return resultString

        End Using

    End Function

#End Region

#Region "文字列操作系"

    ''' <summary>
    ''' アットマーク("@")以降の文字列のみを切り出す。
    ''' </summary>
    ''' <param name="value">文字列</param>
    ''' <returns>
    ''' アットマーク以降の文字列を返却する。
    ''' アットマークが存在しない場合、引数で受け取った文字列を返却する。
    ''' </returns>
    Private Function TrimBeforeAtmark(ByVal value As String) As String

        'Logger.Info(String.Format(CultureInfo.InvariantCulture, _
        '                          "{0}_S IN:value={1}", _
        '                          MethodBase.GetCurrentMethod.Name, value))

        Dim retValue As String

        If value.Contains("@") Then
            retValue = value.Split("@"c)(1)
        Else
            retValue = value
        End If

        'Logger.Info(String.Format(CultureInfo.InvariantCulture, _
        '                          "{0}_E OUT:retValue={1}", _
        '                          MethodBase.GetCurrentMethod.Name, retValue))

        Return retValue

    End Function

    ''' <summary>
    ''' ハイフン("-")より前の文字列のみを切り出す。
    ''' </summary>
    ''' <param name="value">文字列</param>
    ''' <returns>
    ''' ハイフンより前の文字列を返却する。
    ''' ハイフンが存在しない場合、引数で受け取った文字列を返却する。
    ''' </returns>
    ''' <remarks></remarks>
    Private Function TrimAfterHyphen(ByVal value As String) As String

        'Logger.Info(String.Format(CultureInfo.InvariantCulture, _
        '                          "{0}_S IN:value={1}", _
        '                          MethodBase.GetCurrentMethod.Name, value))

        Dim retValue As String

        If value.Contains("-") Then
            retValue = value.Split("-"c)(0)
        Else
            retValue = value
        End If

        'Logger.Info(String.Format(CultureInfo.InvariantCulture, _
        '                          "{0}_E OUT:retValue={1}", _
        '                          MethodBase.GetCurrentMethod.Name, retValue))

        Return retValue

    End Function

    ''' <summary>
    ''' 空白文字のみから構成された文字列をハイフンに変換する。
    ''' </summary>
    ''' <param name="value">文字列</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function EmptyToHyphen(ByVal value As String) As String

        'Logger.Info(String.Format(CultureInfo.InvariantCulture, _
        '                          "{0}_S IN:value={1}", _
        '                          MethodBase.GetCurrentMethod.Name, value))

        Dim retValue As String = IIf(String.IsNullOrEmpty(value), Hyphen, value).ToString()

        'Logger.Info(String.Format(CultureInfo.InvariantCulture, _
        '                          "{0}_E OUT:retValue={1}", _
        '                          MethodBase.GetCurrentMethod.Name, retValue))

        Return retValue

    End Function

    ''' <summary>
    ''' スタッフコードの"@"以前を取得します。
    ''' </summary>
    ''' <param name="staffCode">スタッフコード</param>
    ''' <returns>
    ''' スタッフコードの"@"以前を返却します。
    ''' "@"が存在しない場合、引数で受け取った文字列を返却する。
    ''' </returns>
    ''' <remarks></remarks>
    Private Function SplitStaffCd(ByVal staffCode As String) As String

        'Logger.Info(String.Format(CultureInfo.InvariantCulture, _
        '                          "{0}_S IN:staffCode={1}", _
        '                          MethodBase.GetCurrentMethod.Name, staffCode))

        Dim retValue As String

        If staffCode.Contains("@") Then
            retValue = staffCode.Split("@"c)(0)
        Else
            retValue = staffCode
        End If

        'Logger.Info(String.Format(CultureInfo.InvariantCulture, _
        '                          "{0}_E OUT:retValue={1}", _
        '                          MethodBase.GetCurrentMethod.Name, retValue))

        Return retValue

    End Function

    ''' <summary>
    ''' XMLをインデントを付加して整形する(ログ出力用)
    ''' </summary>
    ''' <param name="xmlDoc">XMLドキュメント</param>
    ''' <returns>整形後XML文字列</returns>
    ''' <remarks></remarks>
    Private Function FormatXml(ByVal xmlDoc As XmlDocument) As String

        'Logger.Info(String.Format(CultureInfo.InvariantCulture, _
        '                          "{0}_S", _
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
                '                          "{0}_E", _
                '                          MethodBase.GetCurrentMethod.Name))

                Return textWriter.ToString()

            Finally
                xmlWriter = Nothing
            End Try

        End Using

    End Function

#End Region

#Region "チェック系"

    ''' <summary>
    ''' Commonタグ内の設定値必須チェックを行う
    ''' </summary>
    ''' <param name="dmsDealerCode">基幹販売店コード</param>
    ''' <param name="dmsBranchCode">基幹店舗コード</param>
    ''' <returns>チェックOK：True/チェックNG：False</returns>
    ''' <remarks></remarks>
    Private Function CheckNecessaryCommonTag(ByVal dmsDealerCode As String, ByVal dmsBranchCode As String) As Boolean

        'Logger.Info(String.Format(CultureInfo.CurrentCulture, _
        '              "{0}.S IN:dmsDealerCode:{1}, dmsBranchCode:{2}", _
        '              MethodBase.GetCurrentMethod.Name, dmsDealerCode, dmsBranchCode))

        Dim retCheckOkFlg As Boolean = True

        If String.IsNullOrEmpty(dmsDealerCode) Then
            '基幹販売店コードが存在しないため、エラー
            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                       "{0}.Error Err:DMSDealerCode is not set.", _
                                       MethodBase.GetCurrentMethod.Name))
            retCheckOkFlg = False
        End If

        If String.IsNullOrEmpty(dmsBranchCode) Then
            '基幹店舗コードが存在しないため、エラー
            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                       "{0}.Error Err:DMSBranchCode is not set.", _
                                       MethodBase.GetCurrentMethod.Name))
            retCheckOkFlg = False
        End If

        'Logger.Info(String.Format(CultureInfo.CurrentCulture, _
        '              "{0}.E OUT:retCheckOkFlg:{1}", _
        '              MethodBase.GetCurrentMethod.Name, retCheckOkFlg))

        Return retCheckOkFlg

    End Function

    ''' <summary>
    ''' Reserve_CustomerInformationタグ内の設定値必須チェックを行う
    ''' </summary>
    ''' <param name="customerName">顧客氏名</param>
    ''' <returns>チェックOK：True/チェックNG：False</returns>
    ''' <remarks></remarks>
    Private Function CheckNecessaryReserveCustomerInformationTag(ByVal customerName As String) As Boolean

        'Logger.Info(String.Format(CultureInfo.CurrentCulture, _
        '              "{0}.S IN:customerName:{1}, telNo:{2}, mobile:{3}", _
        '              MethodBase.GetCurrentMethod.Name, customerName, telNo, mobile))

        Dim retCheckOkFlg As Boolean = True

        If String.IsNullOrEmpty(customerName) Then
            '氏名が存在しないため、エラー
            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                       "{0}.Error Err:Name is not set.", _
                                       MethodBase.GetCurrentMethod.Name))
            retCheckOkFlg = False
        End If

        'If String.IsNullOrEmpty(telNo) And String.IsNullOrEmpty(mobile) Then
        '    '電話番号、携帯電話番号の両方が存在しないため、エラー
        '    Logger.Error(String.Format(CultureInfo.InvariantCulture, _
        '                               "{0}.Error Err:TelNo, Mobile is not set both.", _
        '                               MethodBase.GetCurrentMethod.Name))
        '    retCheckOkFlg = False
        'End If

        'Logger.Info(String.Format(CultureInfo.CurrentCulture, _
        '              "{0}.E OUT:retCheckOkFlg:{1}", _
        '              MethodBase.GetCurrentMethod.Name, retCheckOkFlg))

        Return retCheckOkFlg

    End Function

    ''' <summary>
    ''' Reserve_VehicleInformationタグ内の設定値必須チェックを行う
    ''' </summary>
    ''' <param name="vin">Vin</param>
    ''' <param name="vehicleRegNo">車両登録番号</param>
    ''' <param name="seriesName">モデル名</param>
    ''' <returns>チェックOK：True/チェックNG：False</returns>
    ''' <remarks></remarks>
    Private Function CheckNecessaryReserveVehicleInformationTag(ByVal vin As String, _
                                                                ByVal vehicleRegNo As String, _
                                                                ByVal seriesName As String) As Boolean

        'Logger.Info(String.Format(CultureInfo.CurrentCulture, _
        '              "{0}.S IN:vin:{1}, vehicleRegNo:{2}, seriesName:{3}", _
        '              MethodBase.GetCurrentMethod.Name, vin, vehicleRegNo, seriesName))

        Dim retCheckOkFlg As Boolean = True

        If String.IsNullOrEmpty(vin) And String.IsNullOrEmpty(vehicleRegNo) Then
            'VIN、車両登録番号の両方が存在しないため、エラー
            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                       "{0}.Error Err:Vin, VehicleRegNo is not set both.", _
                                       MethodBase.GetCurrentMethod.Name))
            retCheckOkFlg = False
        End If

        If String.IsNullOrEmpty(seriesName) Then
            'モデル名が存在しないため、エラー
            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                       "{0}.Error Err:ModelName is not set.", _
                                       MethodBase.GetCurrentMethod.Name))
            retCheckOkFlg = False
        End If

        'Logger.Info(String.Format(CultureInfo.CurrentCulture, _
        '              "{0}.E OUT:retCheckOkFlg:{1}", _
        '              MethodBase.GetCurrentMethod.Name, retCheckOkFlg))

        Return retCheckOkFlg

    End Function

    ''' <summary>
    ''' Reserve_ServiceInformationタグ内の設定値必須チェックを行う
    ''' </summary>
    ''' <param name="workTime">予定作業時間</param>
    ''' <param name="rezReception">受付納車区分</param>
    ''' <param name="stallId">ストールID</param>
    ''' <param name="startTime">予定開始日時</param>
    ''' <param name="endTime">予定終了日時</param>
    ''' <returns>チェックOK：True/チェックNG：False</returns>
    ''' <remarks></remarks>
    Private Function CheckNecessaryReserveServiceInformationTag(ByVal workTime As String, _
                                                                ByVal rezReception As String, _
                                                                ByVal stallId As String, _
                                                                ByVal startTime As String, _
                                                                ByVal endTime As String) As Boolean

        'Logger.Info(String.Format(CultureInfo.CurrentCulture, _
        '              "{0}.S IN:workTime:{1}, rezReception:{2}, stallId:{3}, startTime:{4}, endTime:{5}", _
        '              MethodBase.GetCurrentMethod.Name, workTime, rezReception, stallId, startTime, endTime))

        Dim retCheckOkFlg As Boolean = True

        If String.IsNullOrEmpty(workTime) Then
            '予定作業時間が存在しないため、エラー
            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                       "{0}.Error Err:WorkTime is not set.", _
                                       MethodBase.GetCurrentMethod.Name))
            retCheckOkFlg = False
        End If

        If String.IsNullOrEmpty(rezReception) Then
            '受付納車区分が存在しないため、エラー
            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                       "{0}.Error Err:PickDeliType is not set.", _
                                       MethodBase.GetCurrentMethod.Name))
            retCheckOkFlg = False
        End If

        If Not String.IsNullOrEmpty(stallId) Then
            'ストールIDがある場合
            If String.IsNullOrEmpty(startTime) Then
                '予定開始日時がない場合エラー
                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                           "{0}.Error Err:SCHE_START_DATETIME is not set.", _
                                           MethodBase.GetCurrentMethod.Name))
                retCheckOkFlg = False
            End If
            If String.IsNullOrEmpty(endTime) Then
                '予定終了日時がない場合エラー
                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                           "{0}.Error Err:SCHE_END_DATETIME is not set.", _
                                           MethodBase.GetCurrentMethod.Name))
                retCheckOkFlg = False
            End If
        End If

        'Logger.Info(String.Format(CultureInfo.CurrentCulture, _
        '              "{0}.E OUT:retCheckOkFlg:{1}", _
        '              MethodBase.GetCurrentMethod.Name, retCheckOkFlg))

        Return retCheckOkFlg

    End Function

    ''' <summary>
    ''' 予約送信の返却XML内の必要なタグ存在チェックを行う
    ''' </summary>
    ''' <param name="xmlElement"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CheckResultXmlElementTag(ByVal xmlElement As XmlElement) As Boolean

        'Logger.Info(String.Format(CultureInfo.CurrentCulture, _
        '                          "{0}.S IN:xmlElement:{1}", _
        '                          MethodBase.GetCurrentMethod.Name, xmlElement))

        Dim retCheckOkFlg As Boolean = True

        'ResultId
        If IsNothing(xmlElement.GetElementsByTagName(TagResultId).Item(0)) Then
            'ResultIdタグが存在しないため、エラー
            Logger.Warn(String.Format(CultureInfo.InvariantCulture, _
                                       "{0}.Error Err:Failed to get the ResultId.", _
                                       MethodBase.GetCurrentMethod.Name))
            retCheckOkFlg = False
        End If

        'Message
        If IsNothing(xmlElement.GetElementsByTagName(TagMessage).Item(0)) Then
            'Messageタグが存在しないため、エラー
            Logger.Warn(String.Format(CultureInfo.InvariantCulture, _
                                       "{0}.Error Err:Failed to get the Message.", _
                                       MethodBase.GetCurrentMethod.Name))
            retCheckOkFlg = False
        End If

        'Logger.Info(String.Format(CultureInfo.CurrentCulture, _
        '                          "{0}.E OUT:retCheckOkFlg:{1}", _
        '                          MethodBase.GetCurrentMethod.Name, retCheckOkFlg))

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

#End Region

#End Region

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

    ' このコードは、破棄可能なパターンを正しく実装できるように Visual Basic によって追加されました。
    Public Sub Dispose() Implements IDisposable.Dispose
        ' このコードを変更しないでください。クリーンアップ コードを上の Dispose(ByVal disposing As Boolean) に記述します。
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

#End Region

End Class
