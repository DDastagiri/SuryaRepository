'-------------------------------------------------------------------------
'IC3802701BusinessLogic.vb
'-------------------------------------------------------------------------
'機能：JobDispatch実績送信(ビジネスロジック)
'補足：
'作成：2013/12/13 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発
'更新：2015/04/23 TMEJ 明瀬 DMS連携版サービスタブレット強制納車機能追加開発
'更新：2016/06/23 NSK 皆川 TR-SVT-TMT-20151110-001 技術者がタブレットでジョブを停止できない
'更新：2016/09/02 NSK 竹中 TR-SVT-TMT-20151110-001 検証配信後障害対応
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
Imports Toyota.eCRB.DMSLinkage.JobDispatchResult.Api.DataAccess.IC3802701DataSetTableAdapters
Imports Toyota.eCRB.DMSLinkage.JobDispatchResult.Api.DataAccess.IC3802701DataSet
Imports Toyota.eCRB.DMSLinkage.JobDispatchResult.Api.BizLogic.IC46202CN
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.BizLogic
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.DataAccess

Public Class IC3802701BusinessLogic
    Inherits BaseBusinessComponent
    Implements IDisposable

#Region "Privateパラ"
    ''' <summary>
    ''' 作業詳細データテーブル
    ''' </summary>
    Private Property jobDetailDatatable As IC3802701JobDetailDataTable
#End Region

#Region "Publicメソッド"

    ''' <summary>
    ''' 基幹連携(JobDispatch実績情報送信処理)を行う(メイン)
    ''' </summary>
    ''' <param name="inSvcInId">サービス入庫ID</param>
    ''' <param name="inJobDtlId">作業内容ID</param>
    ''' <param name="inPrevJobStatus">更新前作業連携ステータス</param>
    ''' <param name="inCrntJobStatus">更新後作業連携ステータス</param>
    ''' <returns>作業実績送信処理結果コード(0:正常終了/-1:異常終了)</returns>
    ''' <remarks></remarks>
    Public Function SendJobClockOnInfo(ByVal inSvcInId As Decimal, _
                                       ByVal inJobDtlId As Decimal, _
                                       ByVal inPrevJobStatus As IC3802701JobStatusDataTable, _
                                       ByVal inCrntJobStatus As IC3802701JobStatusDataTable) As Integer

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_S inSvcInId={1}, inJobDtlId={2}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  inSvcInId, _
                                  inJobDtlId))

        '戻り値
        Dim retValue As Integer = Success

        Try
            'ステータス送信するかしないかのフラグを取得
            Dim sendFlg As Boolean = Me.IsSendJobClock(inPrevJobStatus, inCrntJobStatus, inJobDtlId)

            If sendFlg Then
                '送信する
                retValue = Me.SendJobClockOnInfoDetails(inSvcInId, _
                                                        inJobDtlId)
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

#Region "Jobdispatch実績送信実行"

    ''' <summary>
    ''' Jobdispatch実績送信実行
    ''' </summary>
    ''' <param name="inSvcInId">サービス入庫ID</param>
    ''' <param name="inJobDtlId">作業内容ID</param>
    ''' <returns>実行結果(0：成功)</returns>
    ''' <remarks></remarks>
    Private Function SendJobClockOnInfoDetails(ByVal inSvcInId As Decimal, _
                                               ByVal inJobDtlId As Decimal) As Integer

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.E inSvcInId={1}, inJobDtlId={2}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  inSvcInId, _
                                  inJobDtlId))

        '戻り値を初期化(正常終了)
        Dim retValue As Integer = Success

        Try
            'ログインスタッフ情報取得
            Dim userContext As StaffContext = StaffContext.Current

            '現在日時取得
            Dim nowDateTime As Date = DateTimeFunc.Now(userContext.DlrCD)

            '**************************************************
            '* システム設定値を取得
            '**************************************************
            Dim systemSettingsValueRow As IC3802701SystemSettingValueRow = Me.GetSystemSettingValues()

            '必要なシステム設定値が一つでも取得できない場合はエラー
            If IsNothing(systemSettingsValueRow) Then

                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                           "{0}.Error: Not setting value is present in the system settings.", _
                                           MethodBase.GetCurrentMethod.Name))
                retValue = ErrorSystem
                Exit Try

            End If

            '**************************************************
            '* 基幹販売店コード、店舗コードを取得
            '**************************************************
            Dim dmsDlrBrnRow As ServiceCommonClassDataSet.DmsCodeMapRow = Me.GetDmsBlnCd(userContext.DlrCD, _
                                                                                         userContext.BrnCD)
            If IsNothing(dmsDlrBrnRow) Then

                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                           "{0}.Error: Failed to convert key dealer code.", _
                                           MethodBase.GetCurrentMethod.Name))
                retValue = ErrorSystem
                Exit Try

            End If

            '**************************************************
            '* JobDetail内容を取得する
            '**************************************************
            Dim dmsJobDetail As List(Of JobDetailClass) = Me.GetJobDetails(inSvcInId, _
                                                                           inJobDtlId, _
                                                                           systemSettingsValueRow.SEND_RELATION_STATUS, _
                                                                           userContext.DlrCD)
            If IsNothing(dmsJobDetail) Then

                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                           "{0}.Error: Failed to get JobDetails.", _
                                           MethodBase.GetCurrentMethod.Name))
                retValue = ErrorSystem
                Exit Try

            End If

            '**************************************************
            '* DispatchInformation内容を取得する
            '**************************************************
            ' 2016/09/02 NSK 竹中 TR-SVT-TMT-20151110-001 技術者がタブレットでジョブを停止できない START
            'Dim dmsDispatchInformation As IC3802701DispatchInfoRow = _
            '    Me.GetDispatchInformation(inSvcInId, _
            '                                  inJobDtlId, _
            '                                  systemSettingsValueRow.SEND_RELATION_STATUS, _
            '                                  nowDateTime)
            Dim dmsDispatchInformation As IC3802701DispatchInfoRow = _
                    Me.GetDispatchInformation(inSvcInId, _
                                              inJobDtlId, _
                                              systemSettingsValueRow.SEND_RELATION_STATUS, _
                                              nowDateTime, _
                                              dmsJobDetail)
            '2016/09/02 NSK 竹中 TR-SVT-TMT-20151110-001 技術者がタブレットでジョブを停止できない　END

            If IsNothing(dmsDispatchInformation) Then

                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                           "{0}.Error: Failed to get DispatchInformation.", _
                                           MethodBase.GetCurrentMethod.Name))
                retValue = ErrorSystem
                Exit Try

            End If

            '**************************************************
            '* StallInformation情報を取得する
            '**************************************************
            Dim stallInfos As List(Of StallInfomationClass) = Me.GetStallInfo(inSvcInId, _
                                                                              inJobDtlId, _
                                                                              systemSettingsValueRow.SEND_RELATION_STATUS, _
                                                                              systemSettingsValueRow.STAFF_SHOW_FLG)
            If IsNothing(stallInfos) Then

                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                           "{0}.Error: Failed to get stallInfo.", _
                                           MethodBase.GetCurrentMethod.Name))
                retValue = ErrorSystem
                Exit Try

            End If

            '**************************************************
            '* 送信XMLの作成
            '**************************************************
            Dim sendXml As XmlDocument = Me.StructSendJobDispatchXml(systemSettingsValueRow, _
                                                                     dmsDlrBrnRow, _
                                                                     dmsDispatchInformation, _
                                                                     dmsJobDetail, _
                                                                     stallInfos, _
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
            Dim resultString As String = Me.ExecuteSendJobDispatchInfoXml(sendXml.InnerXml, _
                                                                          systemSettingsValueRow.LINK_URL_STATUS_INFO, _
                                                                          systemSettingsValueRow.LINK_SEND_TIMEOUT_VAL, _
                                                                          systemSettingsValueRow.LINK_SOAP_VERSION)

            '結果文字列が空文字の場合、エラーとする(送受信処理でエラー発生)
            If String.IsNullOrEmpty(resultString) Then
                Logger.Warn(String.Format(CultureInfo.InvariantCulture, _
                                           "{0}.Error Err:Received XML is empty.", _
                                           MethodBase.GetCurrentMethod.Name))
                retValue = ErrorSystem
                Exit Try
            End If

            '受信XMLから必要な値を取得する
            Dim resultValueList As Dictionary(Of String, String) = Me.GetSendJobDispatchXmlResultData(resultString)

            '受信XMLから必要な値を取得できない場合、エラーとする
            If IsNothing(resultValueList) Then
                Logger.Warn(String.Format(CultureInfo.InvariantCulture, _
                                           "{0}.Error Err:Received XML is empty.", _
                                           MethodBase.GetCurrentMethod.Name))
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
                    If svcCommonBiz.IsOmitDmsErrorCode(InterfaceTypeSendJobDispatch, _
                                                       resultValueList.Item(TagResultId)) Then

                        '除外対象であった場合
                        '-9000(DMS除外エラーの警告)を返却値に設定
                        retValue = WarningOmitDmsError

                    Else
                        '除外対象でなかった場合
                        '9999(異常終了)を返却値に設定し、以降の処理中止
                        retValue = ErrorSystem
                        Exit Try

                    End If

                End Using

                '2015/04/23 TMEJ 明瀬 DMS連携版サービスタブレット強制納車機能追加開発 END

            End If

        Finally

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}.E OUT:retValue={1}", _
                                      MethodBase.GetCurrentMethod.Name, retValue))
        End Try

        Return retValue

    End Function
#End Region

#Region "取得系"

    ''' <summary>
    ''' 作業実績送信を行うかどうかを決定するフラグを取得する
    ''' </summary>
    ''' <param name="prevStatus">変更前チップステータス(基幹連携独自の定義)</param>
    ''' <param name="crntStatus">変更後チップステータス(基幹連携独自の定義)</param>
    ''' <returns>送信フラグ</returns>
    ''' <remarks></remarks>
    Private Function GetSendReserveFlg(ByVal prevStatus As String, _
                                       ByVal crntStatus As String) As String

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_S prevStatus={1}, crntStatus={2}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  prevStatus, _
                                  crntStatus))


        'ログインユーザー情報取得
        Dim userContext As StaffContext = StaffContext.Current

        '送信フラグ(戻り値)
        Dim sendFlg As String = String.Empty

        '自分のテーブルアダプタークラスインスタンスを生成
        Using myTableAdapter As New IC3802701DataTableAdapter
            '作業実績送信するかしないかのフラグを取得
            Dim getTable As IC3802701LinkSendSettingsDataTable = _
                myTableAdapter.GetLinkSettings(userContext.DlrCD, _
                                               userContext.BrnCD, _
                                               AllDealerCode, _
                                               AllBranchCode, _
                                               "3", _
                                               prevStatus, _
                                               crntStatus)

            'ログ出力用販売店コード、店舗コード
            Dim dealerCode As String = String.Empty
            Dim branchCode As String = String.Empty

            Dim getFirstRow As IC3802701LinkSendSettingsRow

            If 0 < getTable.Count Then

                '取得データの1行目（最優先レコード）のみを取得
                getFirstRow = getTable.Item(0)
                '最優先レコードの送信フラグ、販売店コード、店舗コードを取得
                sendFlg = getFirstRow.SEND_FLG
                dealerCode = getFirstRow.DLR_CD
                branchCode = getFirstRow.BRN_CD

            End If

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}_E SEND_FLG={1}, DLR_CD={2}, BRN_CD={3}", _
                                      MethodBase.GetCurrentMethod.Name, _
                                      sendFlg, _
                                      dealerCode, _
                                      branchCode))

        End Using

        Return sendFlg

    End Function

    ''' <summary>
    ''' 基幹販売店、基幹店舗コードを取得する
    ''' </summary>
    ''' <param name="dealerCode">i-CROP販売店コード</param>
    ''' <param name="branchCode">i-CROP店舗コード</param>
    ''' <returns>中断情報テーブル</returns>
    ''' <remarks></remarks>
    Private Function GetDmsBlnCd(ByVal dealerCode As String, _
                                 ByVal branchCode As String) As ServiceCommonClassDataSet.DmsCodeMapRow

        Dim dmsDlrBrnTable As ServiceCommonClassDataSet.DmsCodeMapDataTable = Nothing

        Using serviceCommonBiz As New ServiceCommonClassBusinessLogic
            '基幹販売店コード、店舗コードを取得
            dmsDlrBrnTable = serviceCommonBiz.GetIcropToDmsCode(dealerCode, _
                                                                ServiceCommonClassBusinessLogic.DmsCodeType.BranchCode, _
                                                                dealerCode, _
                                                                branchCode, _
                                                                String.Empty)
            If dmsDlrBrnTable.Count <= 0 Then
                'データが取得できない場合はエラー
                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                           "{0}.Error ErrCode:{1}, Failed to convert key dealer code.(No data found)", _
                                           MethodBase.GetCurrentMethod.Name, _
                                           ErrorDmsCodeMap))
                Return Nothing
            ElseIf 1 < dmsDlrBrnTable.Count Then
                'データが2件以上取得できた場合は一意に決定できないためエラー
                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                           "{0}.Error ErrCode:{1}, Failed to convert key dealer code.(Non-unique)", _
                                           MethodBase.GetCurrentMethod.Name, _
                                           ErrorDmsCodeMap))
                Return Nothing
            End If
        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.E ", _
                                  MethodBase.GetCurrentMethod.Name))

        Return dmsDlrBrnTable.Item(0)

    End Function

    ''' <summary>
    ''' DispatchInformationの内容を取得する
    ''' </summary>
    ''' <param name="inSvcInId">サービス入庫ID</param>
    ''' <param name="inJobDtlId">作業内容ID</param>
    ''' <param name="inSendRelationStatus">関連チップ送信フラグ</param>
    ''' <param name="inNowDateTime">今の日時</param>
    ''' <returns>DispatchInformation情報</returns>
    ''' <remarks></remarks>
    Private Function GetDispatchInformation(ByVal inSvcInId As Decimal, _
                                            ByVal inJobDtlId As Decimal, _
                                            ByVal inSendRelationStatus As String, _
                                            ByVal inNowDateTime As Date, _
                                            ByVal dmsJobDetail As List(Of JobDetailClass)) As IC3802701DispatchInfoRow

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_S inSvcInId={1}, inJobDtlId={2}, inSendRelationStatus={3}, inNowDateTime={4}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  inSvcInId, _
                                  inJobDtlId, _
                                  inSendRelationStatus, _
                                  inNowDateTime))

        Dim dmsDispatchInformation As IC3802701DispatchInfoRow = Nothing
        Dim chipInfo As IC3802701DispatchInfoDataTable = Nothing

        'サービス入庫IDで関連チップの情報を取得する
        Using ic3802701Ta As New IC3802701DataTableAdapter
            chipInfo = ic3802701Ta.GetDispatchInfoBySvcinId(inSvcInId)
        End Using

        If chipInfo.Count = 0 Then
            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                       "{0}.Error: GetChipInfoBySevcinId's count is 0.", _
                                       MethodBase.GetCurrentMethod.Name))
            Return Nothing
        End If


        '処理対象の作業内容．作業内容IDを設定する。
        Dim dmsDispatchInformations As IC3802701DispatchInfoRow() = _
            CType(chipInfo.Select(String.Format(CultureInfo.CurrentCulture, "JOB_DTL_ID = '{0}'", inJobDtlId)), IC3802701DispatchInfoRow())

        If dmsDispatchInformations.Count = 1 Then
            dmsDispatchInformation = dmsDispatchInformations(0)
        Else

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                       "{0}.Error: DmsDispatchInformations's count is {1}.", _
                       MethodBase.GetCurrentMethod.Name, _
                       dmsDispatchInformations.Count))
            Return Nothing

        End If

        'システム設定．関連チップ送信フラグ = 「1：送信する」の場合
        If SendRelationChipFlg_Send.Equals(inSendRelationStatus) Then
            '作業内容IDが最小の作業内容．作業内容ID（親チップの作業内容ID）を設定する。
            dmsDispatchInformation.JOB_DTL_ID = chipInfo(0).JOB_DTL_ID
        End If

        'シーケンス値を設定する
        dmsDispatchInformation.SEQ_NO = inNowDateTime.ToString(SeqNoNumberingFormat, CultureInfo.CurrentCulture)

        'VinがDBNullの場合、空白を設定する
        If dmsDispatchInformation.IsVCL_VINNull Then
            dmsDispatchInformation.VCL_VIN = ""
        End If

        'JobDeatilタブに一番最小の開始実績日時を取得する
        Dim startTime As Date = Nothing
        '作業実績最小の作業実績から取得する
        For Each jobDetailRow As IC3802701JobDetailRow In jobDetailDatatable
            If Not jobDetailRow.IsRSLT_START_DATETIMENull _
                AndAlso Not jobDetailRow.RSLT_START_DATETIME = Date.Parse(DefaultDate, CultureInfo.InvariantCulture) Then
                If startTime = Date.MinValue Then
                    startTime = jobDetailRow.RSLT_START_DATETIME
                    Continue For
                End If
                '小さい方を登録する
                If Date.Compare(startTime, jobDetailRow.RSLT_START_DATETIME) > 0 Then
                    startTime = jobDetailRow.RSLT_START_DATETIME
                End If
            End If
        Next

        '作業実績最大の作業実績から取得する
        Dim endTime As Date = Nothing
        '全部終了の場合、作業実績終了日時がある

        ' 2016/09/02 NSK 竹中 TR-SVT-TMT-20151110-001 技術者がタブレットでジョブを停止できない START
        'For Each jobDetailRow As IC3802701JobDetailRow In jobDetailDatatable
        For Each jobDetailRow As JobDetailClass In dmsJobDetail
            '2016/09/02 NSK 竹中 TR-SVT-TMT-20151110-001 技術者がタブレットでジョブを停止できない　END

            ' 2016/09/02 NSK 竹中 TR-SVT-TMT-20151110-001 技術者がタブレットでジョブを停止できない START
            '    'If Not jobDetailRow.IsRSLT_END_DATETIMENull _
            '    '        AndAlso Not jobDetailRow.RSLT_END_DATETIME = Date.Parse(DefaultDate, CultureInfo.InvariantCulture) Then
            If Not jobDetailRow.EndTime = Date.MinValue _
                    AndAlso Not jobDetailRow.EndTime = Date.Parse(DefaultDate, CultureInfo.InvariantCulture) Then
                '2016/09/02 NSK 竹中 TR-SVT-TMT-20151110-001 技術者がタブレットでジョブを停止できない　END

                If endTime = Date.MinValue Then
                    endTime = jobDetailRow.EndTime
                    Continue For
                End If
                '大きい方を登録する
                If Date.Compare(endTime, jobDetailRow.EndTime) < 0 Then
                    endTime = jobDetailRow.EndTime
                End If
            Else
                '終了してない作業があれば、作業終了日時をnothingに設定する
                endTime = Nothing
                Exit For
            End If

        Next

        dmsDispatchInformation.START_DATETIME = startTime
        dmsDispatchInformation.END_DATETIME = endTime

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                "{0}.E ", _
                                MethodBase.GetCurrentMethod.Name))

        Return dmsDispatchInformation

    End Function

    ''' <summary>
    ''' 全てJobDetailの内容を取得する
    ''' </summary>
    ''' <param name="inSvcInId">サービス入庫ID</param>
    ''' <param name="inJobDtlId">作業内容ID</param>
    ''' <param name="inSendRelationStatus">関連チップ送信フラグ</param>
    ''' <param name="inDlrCode">販売店コード</param>
    ''' <returns>DispatchInformation情報</returns>
    ''' <remarks></remarks>
    Private Function GetJobDetails(ByVal inSvcInId As Decimal, _
                                   ByVal inJobDtlId As Decimal, _
                                   ByVal inSendRelationStatus As String, _
                                   ByVal inDlrCode As String) As List(Of JobDetailClass)

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_S inSvcInId={1}, inJobDtlId={2}, inSendRelationStatus={3}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  inSvcInId, _
                                  inJobDtlId, _
                                  inSendRelationStatus))

        jobDetailDatatable = Nothing

        'JobDetailの情報を取得する
        Using ic3802701Ta As New IC3802701DataTableAdapter
            'システム設定．関連チップ送信フラグ = 「0：送信しない」の場合
            If SendRelationChipFlg_NotSend.Equals(inSendRelationStatus) Then
                jobDetailDatatable = ic3802701Ta.GetJobDetailByJobDtlId(inJobDtlId)
            Else
                jobDetailDatatable = ic3802701Ta.GetJobDetailBySvcinId(inSvcInId)
            End If
        End Using

        If jobDetailDatatable.Count = 0 Then
            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                       "{0}.Error: GetChipInfoBySevcinId's count is 0.", _
                                       MethodBase.GetCurrentMethod.Name))
            Return Nothing
        End If


        Dim jobDetails As New List(Of JobDetailClass)
        '同じ作業チェック用テーブル
        Using sameCheckTable As New IC3802701JobDetailDataTable

            '作業指示IDでループする
            For Each jobDetailRow As IC3802701JobDetailRow In jobDetailDatatable

                '同じ作業があるかチェック、あれば、次の作業に
                Dim checkRows As IC3802701JobDetailRow() = _
                    CType(sameCheckTable.Select(String.Format(CultureInfo.InvariantCulture, _
                                                            "JOB_DTL_ID = '{0}' AND JOB_INSTRUCT_ID = '{1}' AND JOB_INSTRUCT_SEQ = '{2}'", _
                                                            jobDetailRow.JOB_DTL_ID, _
                                                            jobDetailRow.JOB_INSTRUCT_ID, _
                                                            jobDetailRow.JOB_INSTRUCT_SEQ)), IC3802701JobDetailRow())
                If checkRows.Count > 0 Then
                    Continue For
                End If

                '作業単位でデータを取得する
                Dim oneJobRows As IC3802701JobDetailRow() = _
                    CType(jobDetailDatatable.Select(String.Format(CultureInfo.InvariantCulture, _
                                                            "JOB_DTL_ID = '{0}' AND JOB_INSTRUCT_ID = '{1}' AND JOB_INSTRUCT_SEQ = '{2}'", _
                                                            jobDetailRow.JOB_DTL_ID, _
                                                            jobDetailRow.JOB_INSTRUCT_ID, _
                                                            jobDetailRow.JOB_INSTRUCT_SEQ)), IC3802701JobDetailRow())

                If oneJobRows.Count > 0 Then
                    '該作業指示のJobDetailの内容を取得する
                    Dim jobDetail As JobDetailClass = Me.GetJobDetail(oneJobRows, _
                                                                      inDlrCode)

                    If IsNothing(jobDetail) Then
                        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                                   "{0}.Error : Function GetJobDetail is Failed.", _
                                                   MethodBase.GetCurrentMethod.Name))
                        Return Nothing
                    End If

                    'チェック用テーブルにバックする
                    Dim newCheckRow As IC3802701JobDetailRow = sameCheckTable.NewIC3802701JobDetailRow
                    newCheckRow.ItemArray = oneJobRows(0).ItemArray
                    sameCheckTable.AddIC3802701JobDetailRow(newCheckRow)

                    'XML用の構造体に値を設定する
                    jobDetails.Add(jobDetail)
                End If
            Next

        End Using
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.E ", _
                                  MethodBase.GetCurrentMethod.Name))

        Return jobDetails

    End Function

    ''' <summary>
    ''' １つ作業指示のJobDetailの内容を取得する
    ''' </summary>
    ''' <param name="jobDetailRows">１つ作業指示のデータテーブル</param>
    ''' <param name="dlrCode">販売店コード</param>
    ''' <returns>１つ作業指示のJobDetailの内容</returns>
    ''' <remarks></remarks>
    Private Function GetJobDetail(ByVal jobDetailRows As IC3802701JobDetailRow(), _
                                  ByVal dlrCode As String) As JobDetailClass

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.E dlrCode={1}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  dlrCode))

        Dim jobDetailCount As Long = jobDetailRows.Count
        '作業ステータス
        Dim jobStatus As String
        '作業ステータスが設定してない場合、作業前を指定する
        If jobDetailRows(CType(jobDetailCount - 1, Integer)).IsJOB_STATUSNull Then
            jobStatus = JobStatusBeforeStart
        Else
            jobStatus = jobDetailRows(CType(jobDetailCount - 1, Integer)).JOB_STATUS
        End If

        '作業実績最小の作業実績から取得する
        Dim startTime As Date = Nothing
        For Each jobDetailRow As IC3802701JobDetailRow In jobDetailRows
            If Not jobDetailRow.IsRSLT_START_DATETIMENull Then
                If startTime = Date.MinValue Then
                    startTime = jobDetailRow.RSLT_START_DATETIME
                    Continue For
                End If
                '小さい方を登録する
                If Date.Compare(startTime, jobDetailRow.RSLT_START_DATETIME) > 0 Then
                    startTime = jobDetailRow.RSLT_START_DATETIME
                End If
            End If
        Next

        '作業実績最大の作業実績から取得する
        Dim endTime As Date = Nothing
        '全部終了の場合、作業実績終了日時がある
        If JobStatusFinish.Equals(jobStatus) Then
            For Each jobDetailRow As IC3802701JobDetailRow In jobDetailRows
                If Not jobDetailRow.IsRSLT_END_DATETIMENull Then
                    If endTime = Date.MinValue Then
                        endTime = jobDetailRow.RSLT_END_DATETIME
                        Continue For
                    End If
                    '大きい方を登録する
                    If Date.Compare(endTime, jobDetailRow.RSLT_END_DATETIME) < 0 Then
                        endTime = jobDetailRow.RSLT_END_DATETIME
                    End If
                End If
            Next
        End If

        '中断情報(StopInformation)取得
        Dim stopInfo As List(Of StopInfomationClass) = _
            Me.GetStopInformation(jobDetailRows, _
                                  dlrCode)

        If IsNothing(stopInfo) Then
            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                       "{0}.Error: Function GetStopInformation is failed.", _
                                       MethodBase.GetCurrentMethod.Name))
            Return Nothing
        End If

        '中断情報が1つもない場合、nothingに設定する
        If stopInfo.Count = 0 Then
            stopInfo = Nothing
        End If

        '作業実績時間取得
        Dim workTime As Long = Me.GetWorkTime(startTime, _
                                              endTime, _
                                              jobStatus, _
                                              stopInfo)

        'ステータスがDMSのステータスに変える
        Dim dmsJobStatus = Me.ConvertToDmsJobStatus(jobStatus, dlrCode)
        If IsNothing(dmsJobStatus) Then
            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                       "{0}.Error ErrCode: Function ConvertToDmsJobStatus is failed.", _
                                       MethodBase.GetCurrentMethod.Name))
            Return Nothing
        End If

        '作業コード
        Dim jobCd As String = String.Empty
        If Not jobDetailRows(0).IsJOB_CDNull Then
            jobCd = jobDetailRows(0).JOB_CD
        End If

        '作業スタッフグループID
        Dim jobStfGroupId As String = String.Empty
        If Not jobDetailRows(0).IsJOB_STF_GROUP_IDNull Then
            jobStfGroupId = jobDetailRows(0).JOB_STF_GROUP_ID
        End If

        'FM承認アカウント
        Dim fmAccount As String = String.Empty
        If Not jobDetailRows(0).IsINSPECTION_APPROVAL_STF_CDNull Then
            fmAccount = jobDetailRows(0).INSPECTION_APPROVAL_STF_CD
        End If

        Dim clsJobDetail As New JobDetailClass
        With clsJobDetail
            .DispatchNo = jobDetailRows(0).JOB_INSTRUCT_ID                                              '作業指示ID
            .JobSequenceNumber = jobDetailRows(0).JOB_INSTRUCT_SEQ                                      '作業指示枝番
            .JobID = jobCd                                                                              '作業コード
            .GroupID = jobStfGroupId                                                                    '作業スタッフグループID
            .Status = dmsJobStatus                                                                      '作業ステータス
            .StartTime = startTime                                                                      '実績開始日時
            .EndTime = endTime                                                                          '実績終了日時
            .WorkTime = workTime                                                                        '作業実績時間
            .FMAccount = fmAccount                                                                      'FM承認アカウント
            .InspectionFlg = jobDetailRows(0).INSPECTION_NEED_FLG                                       '検査フラグ
            .StopInformation = stopInfo                                                                 '中断情報
        End With

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.E  ", _
                                  MethodBase.GetCurrentMethod.Name))

        Return clsJobDetail
    End Function

    ''' <summary>
    ''' 中断情報(StopInformation)取得
    ''' </summary>
    ''' <param name="jobDetailRows">１つ作業指示のデータテーブル</param>
    ''' <param name="dlrCode">販売店コード</param>
    ''' <returns>１つ作業指示のJobDetailの中断情報</returns>
    ''' <remarks></remarks>
    Private Function GetStopInformation(ByVal jobDetailRows As IC3802701JobDetailRow(), _
                                        ByVal dlrCode As String) As List(Of StopInfomationClass)

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                          "{0}.E dlrCode={1}", _
                          MethodBase.GetCurrentMethod.Name, _
                          dlrCode))

        Dim stopInfos As New List(Of StopInfomationClass)

        '2行以下、または終了してないの場合、戻る
        If jobDetailRows.Count < 2 _
            OrElse jobDetailRows(0).IsJOB_STATUSNull _
            OrElse Not (JobStatusFinish.Equals(jobDetailRows(0).JOB_STATUS) _
                        Or JobStatusStop.Equals(jobDetailRows(0).JOB_STATUS)) Then
            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                  "{0}.E: jobDetailRows.Count = 0 ", _
                  MethodBase.GetCurrentMethod.Name))
            Return stopInfos
        End If

        '中断情報の設定
        For i = 1 To jobDetailRows.Count - 1

            '中断理由区分
            Dim stopReason As String
            '設定してない場合(日跨ぎ終了)、「99 その他」を指定する
            If jobDetailRows(i - 1).IsSTOP_REASON_TYPENull _
                OrElse String.IsNullOrEmpty(jobDetailRows(i - 1).STOP_REASON_TYPE.Trim()) Then
                stopReason = StopTypeOther
            Else
                stopReason = jobDetailRows(i - 1).STOP_REASON_TYPE
            End If

            'DMS中断理由区分に変える
            Dim dmsStopReason As String = Me.ConvertToDmsStopReason(stopReason, dlrCode)
            If IsNothing(dmsStopReason) Then
                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                            "{0}.Error ErrCode: Failed to convert DMS StopReason.", _
                                            MethodBase.GetCurrentMethod.Name))
                Return Nothing
            End If

            Dim stopInfo As New StopInfomationClass
            With stopInfo
                .StopSequenceNumber = i                                                                 '中断シーケンスナンバー(1から)
                .StopStart = jobDetailRows(i - 1).RSLT_END_DATETIME                                     '中断開始日時
                .StopReason = dmsStopReason                                                             '中断理由区分
                .StopEnd = jobDetailRows(i).RSLT_START_DATETIME                                         '中断終了日時
            End With

            stopInfos.Add(stopInfo)
        Next

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                          "{0}.E ", _
                          MethodBase.GetCurrentMethod.Name))
        Return stopInfos
    End Function

    ''' <summary>
    ''' 作業実績時間取得
    ''' </summary>
    ''' <param name="startDatetime">作業の実績開始日時</param>
    ''' <param name="endDatetime">作業の実績終了日時</param>
    ''' <param name="jobStatus">作業ステータス</param>
    ''' <param name="stopInfos">中断情報</param>
    ''' <returns>作業実績時間</returns>
    ''' <remarks></remarks>
    Private Function GetWorkTime(ByVal startDatetime As Date, _
                                 ByVal endDatetime As Date, _
                                 ByVal jobStatus As String, _
                                 ByVal stopInfos As List(Of StopInfomationClass)) As Long

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_S startDatetime={1}, endDatetime={2}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  startDatetime, _
                                  endDatetime))

        '作業実績がnull、または、作業実績IDが最大の作業実績．作業ステータスが「1：完了」以外の場合、
        If startDatetime = Date.MinValue _
            OrElse endDatetime = Date.MinValue _
            OrElse Not JobStatusFinish.Equals(jobStatus) Then

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}.E Return Nothing ", _
                                      MethodBase.GetCurrentMethod.Name))

            Return Nothing
        End If

        'ローカル変数．作業実績時間に、ローカル変数．実績終了日時 - ローカル変数．実績開始日時を設定する
        Dim workTime As Long = DateDiff(DateInterval.Minute, _
                                        startDatetime, _
                                        endDatetime)

        '中断情報にデータがあれば
        If Not IsNothing(stopInfos) Then
            '中断情報テーブルの件数分繰り返す
            For Each stopInfo As StopInfomationClass In stopInfos
                '中断時間を計算する
                Dim stopTime As Long = DateDiff(DateInterval.Minute, _
                                                CType(stopInfo.StopStart, Date), _
                                                CType(stopInfo.StopEnd, Date))

                'ローカル変数．作業実績時間に、ローカル変数．作業実績時間 - ローカル変数．中断時間を設定する
                workTime = workTime - stopTime
            Next
        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                "{0}.E workTime={1} ", _
                                MethodBase.GetCurrentMethod.Name, _
                                workTime))

        Return workTime

    End Function

    ''' <summary>
    ''' ストール情報取得
    ''' </summary>
    ''' <param name="inSvcinId">サービス入庫ID</param>
    ''' <param name="inJobDtlId">作業内容ID</param>
    ''' <param name="inSendRelationStatus">関連チップ送信フラグ</param>
    ''' <param name="inDlrStaffCode">販売店システム設定．スタッフ表示フラグ</param>
    ''' <returns>作業実績時間</returns>
    ''' <remarks></remarks>
    Private Function GetStallInfo(ByVal inSvcinId As Decimal, _
                                  ByVal inJobDtlId As Decimal, _
                                  ByVal inSendRelationStatus As String, _
                                  ByVal inDlrStaffCode As String) As List(Of StallInfomationClass)

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_S inSvcinId={1}, inJobDtlId={2}, inSendRelationStatus={3}, inDlrStaffCode={4} ", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  inSvcinId, _
                                  inJobDtlId, _
                                  inSendRelationStatus, _
                                  inDlrStaffCode))

        '戻る情報作成
        Dim retStallInfos As New List(Of StallInfomationClass)

        'ストールID、実績開始日時、実績終了日時、作業IDを取得する
        Dim stallInfoDatatable As IC3802701StallInformationDataTable = Nothing

        '実績チップのストール利用の実績情報を取得する
        Using ic3802701Ta As New IC3802701DataTableAdapter
            'システム設定．関連チップ送信フラグ = 「1：送信しない」の場合
            If SendRelationChipFlg_NotSend.Equals(inSendRelationStatus) Then
                stallInfoDatatable = ic3802701Ta.GetResultStallUseInfo(inSvcinId, inJobDtlId)
            Else
                stallInfoDatatable = ic3802701Ta.GetResultStallUseInfo(inSvcinId)
            End If

            'ストール利用分繰り返す
            For Each stallInfoRow As IC3802701StallInformationRow In stallInfoDatatable

                'テクニシャン情報と非稼働情報テーブルを定義する
                Dim techInfoDatatable As IC3802701StaffJobDataTable = Nothing

                'スタッフ表示フラグが「0：ストールに割り当てられたテクニシャンを表示する」の場合
                If StaffCodeShow.Equals(inDlrStaffCode) Then
                    'スタッフ作業テーブルから指定作業IDのスタッフコードを取得する
                    techInfoDatatable = ic3802701Ta.GetTechnicianIdByJobId(stallInfoRow.JOB_ID)
                    If techInfoDatatable.Count = 0 Then
                        techInfoDatatable = Nothing
                    End If
                End If

                Dim stallInfo As New StallInfomationClass

                Dim startTime As Date = Nothing
                If Not stallInfoRow.IsRSLT_START_DATETIMENull Then
                    startTime = stallInfoRow.RSLT_START_DATETIME
                End If

                Dim endTime As Date = Nothing
                '中断の場合、実績終了日時を空白に設定する
                If Not stallInfoRow.IsRSLT_END_DATETIMENull _
                    AndAlso Not StalluseStatusStop.Equals(stallInfoRow.STALL_USE_STATUS) Then
                    endTime = stallInfoRow.RSLT_END_DATETIME
                End If

                Dim restEndTime As Date = Nothing
                '実績日時があれば、
                If Not Me.IsDefaultValue(stallInfoRow.RSLT_END_DATETIME) Then
                    '休憩チップ探す用の終了日時が実績終了日時を使う
                    restEndTime = stallInfoRow.RSLT_END_DATETIME
                Else
                    '休憩チップ探す用の終了日時が見込み終了日時を使う
                    restEndTime = stallInfoRow.PRMS_END_DATETIME
                End If

                With stallInfo
                    .StallId = stallInfoRow.STALL_ID
                    .StartTime = startTime
                    .EndTime = endTime
                    .TechnicianId = techInfoDatatable
                    .RestInformation = Me.GetRestInfo(stallInfoRow.STALL_ID, startTime, restEndTime)
                End With

                retStallInfos.Add(stallInfo)
            Next

        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                "{0}.E  ", _
                                MethodBase.GetCurrentMethod.Name))
        Return retStallInfos

    End Function

    ''' <summary>
    ''' 指定ストール、指定時間内に休憩情報を取得する
    ''' </summary>
    ''' <param name="inStallId">指定ストールID</param>
    ''' <param name="inStartDateTime">指定開始日時</param>
    ''' <param name="inEndDateTime">指定終了日時</param>
    ''' <returns>休憩情報</returns>
    ''' <remarks></remarks>
    Private Function GetRestInfo(ByVal inStallId As Decimal, _
                                 ByVal inStartDateTime As Date, _
                                 ByVal inEndDateTime As Date) As IC3802701StallIdleRow()

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                "{0}_S inStallId={1}, inStartDateTime={2}, inEndDateTime={3}", _
                                MethodBase.GetCurrentMethod.Name, _
                                inStallId, _
                                inStartDateTime, _
                                inEndDateTime))

        Dim idleDatatable As IC3802701StallIdleDataTable = Nothing

        Using ic3802701Ta As New IC3802701DataTableAdapter
            'stallIdsにより、非稼働情報を取得する
            idleDatatable = ic3802701Ta.GetIdleByStallId(inStallId, inStartDateTime, inEndDateTime)

            If (idleDatatable.Count > 0) Then
                For Each idleMergeRow As IC3802701StallIdleRow In idleDatatable

                    '休憩エリアの場合、日付をチップの日付に設定する
                    If IdleTypeRest.Equals(idleMergeRow.IDLE_TYPE) Then
                        Dim startDatetime As Date = inStartDateTime
                        startDatetime = startDatetime.AddHours(startDatetime.Hour * -1 + idleMergeRow.IDLE_START_TIME.Hour)
                        startDatetime = startDatetime.AddMinutes(startDatetime.Minute * -1 + idleMergeRow.IDLE_START_TIME.Minute)
                        startDatetime = startDatetime.AddSeconds(startDatetime.Second * -1)

                        idleMergeRow.IDLE_START_DATETIME = startDatetime

                        Dim endDatetime As Date = inStartDateTime
                        endDatetime = endDatetime.AddHours(endDatetime.Hour * -1 + idleMergeRow.IDLE_END_TIME.Hour)
                        endDatetime = endDatetime.AddMinutes(endDatetime.Minute * -1 + idleMergeRow.IDLE_END_TIME.Minute)
                        endDatetime = endDatetime.AddSeconds(endDatetime.Second * -1)

                        idleMergeRow.IDLE_END_DATETIME = endDatetime
                    End If
                Next
            End If
        End Using


        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                "{0}.E  ", _
                                MethodBase.GetCurrentMethod.Name))

        Return CType(idleDatatable.Select(String.Format(CultureInfo.CurrentCulture, "STALL_ID = '{0}'", inStallId)), IC3802701StallIdleRow())
    End Function

#Region "システム設定値を取得"

    ''' <summary>
    ''' システム設定、販売店設定から作業実績送信に必要な設定値を取得する
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetSystemSettingValues() As IC3802701SystemSettingValueRow

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_S", _
                                  MethodBase.GetCurrentMethod.Name))

        '戻り値
        Dim retRow As IC3802701SystemSettingValueRow = Nothing

        'エラー発生フラグ
        Dim errorFlg As Boolean = False

        Try

            Dim linkSendTimeoutVal As String
            Dim countryCode As String
            Dim dateFormat As String
            Dim StatusCdConv As String
            Dim sendRelationStatus As String
            Dim linkUrlResvInfo As String
            Dim staffShow As String
            Dim soapVersion As String

            Using serviceCommonBiz As New ServiceCommonClassBusinessLogic

                '******************************
                '* システム設定から取得
                '******************************
                '基幹連携送信時タイムアウト値
                linkSendTimeoutVal = serviceCommonBiz.GetSystemSettingValueBySettingName(SysLinkSendTimeOutVal)

                If String.IsNullOrEmpty(linkSendTimeoutVal) Then
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                               "{0}.Error ErrCode:{1}, LINK_SEND_TIMEOUT_VAL does not exist.", _
                                               MethodBase.GetCurrentMethod.Name, _
                                               ErrorSysEnv))
                    errorFlg = True
                    Exit Try
                End If

                '国コード
                countryCode = serviceCommonBiz.GetSystemSettingValueBySettingName(SysCountryCode)

                If String.IsNullOrEmpty(countryCode) Then
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                               "{0}.Error ErrCode:{1}, DIST_CD does not exist.", _
                                               MethodBase.GetCurrentMethod.Name, _
                                               ErrorSysEnv))
                    errorFlg = True
                    Exit Try
                End If

                '日付フォーマット
                dateFormat = serviceCommonBiz.GetSystemSettingValueBySettingName(SysDateFormat)

                If String.IsNullOrEmpty(dateFormat) Then
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                               "{0}.Error ErrCode:{1}, DATE_FORMAT does not exist.", _
                                               MethodBase.GetCurrentMethod.Name, _
                                               ErrorSysEnv))
                    errorFlg = True
                    Exit Try
                End If

                'ステータスコード変換フラグ
                StatusCdConv = serviceCommonBiz.GetSystemSettingValueBySettingName(StatusCodeConvFlg)

                If String.IsNullOrEmpty(StatusCdConv) Then
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                               "{0}.Error ErrCode:{1}, STATUS_CD_CONV_FLG does not exist.", _
                                               MethodBase.GetCurrentMethod.Name, _
                                               ErrorSysEnv))
                    errorFlg = True
                    Exit Try
                End If

                '関連チップ送信フラグ
                sendRelationStatus = serviceCommonBiz.GetSystemSettingValueBySettingName(SysSendRelationStatus)

                If String.IsNullOrEmpty(sendRelationStatus) Then
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                               "{0}.Error ErrCode:{1}, SEND_RELATION_STATUS does not exist.", _
                                               MethodBase.GetCurrentMethod.Name, _
                                               ErrorSysEnv))
                    errorFlg = True
                    Exit Try
                End If

                'SOAPバージョン判定値
                soapVersion = serviceCommonBiz.GetSystemSettingValueBySettingName(SysSoapVersion)

                If String.IsNullOrEmpty(soapVersion) Then
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                               "{0}.Error ErrCode:{1}, LINK_SOAP_VERSION does not exist.", _
                                               MethodBase.GetCurrentMethod.Name, _
                                               ErrorSysEnv))
                    errorFlg = True
                    Exit Try
                End If

                '******************************
                '* 販売店システム設定から取得
                '******************************
                '送信先アドレス
                linkUrlResvInfo = serviceCommonBiz.GetDlrSystemSettingValueBySettingName(DlrSysLinkUrlJobRsltInfo)

                If String.IsNullOrEmpty(linkUrlResvInfo) Then
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                               "{0}.Error ErrCode:{1}, LINK_URL_STATUS_INFO does not exist.", _
                                               MethodBase.GetCurrentMethod.Name, _
                                               ErrorDlrEnv))
                    errorFlg = True
                    Exit Try
                End If

                'スタッフ表示フラグ
                staffShow = serviceCommonBiz.GetDlrSystemSettingValueBySettingName(StaffShowFlg)

                If String.IsNullOrEmpty(staffShow) Then
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                               "{0}.Error ErrCode:{1}, SMB_STF_DISP_FLG does not exist.", _
                                               MethodBase.GetCurrentMethod.Name, _
                                               ErrorDlrEnv))
                    errorFlg = True
                    Exit Try
                End If

            End Using

            Using table As New IC3802701SystemSettingValueDataTable

                retRow = table.NewIC3802701SystemSettingValueRow

                With retRow
                    '取得した値を戻り値のデータ行に設定
                    .LINK_SEND_TIMEOUT_VAL = linkSendTimeoutVal
                    .DIST_CD = countryCode
                    .DATE_FORMAT = dateFormat
                    .STATUS_CD_CONV_FLG = StatusCdConv
                    .SEND_RELATION_STATUS = sendRelationStatus
                    .LINK_URL_STATUS_INFO = linkUrlResvInfo
                    .STAFF_SHOW_FLG = staffShow
                    .LINK_SOAP_VERSION = soapVersion
                End With

            End Using

        Finally

            If errorFlg Then
                retRow = Nothing
            End If

        End Try

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_E", _
                                  MethodBase.GetCurrentMethod.Name))

        'システム値があれば、ログを出す
        If Not IsNothing(retRow) Then
            '引数をログに出力
            Dim args As New List(Of String)

            'DataRow内の項目を列挙
            Me.AddLogData(args, retRow)

            Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                      "{0}_E. Out:{1}", _
                                      MethodBase.GetCurrentMethod.Name, _
                                      String.Join(", ", args.ToArray())))
        End If

        Return retRow

    End Function

#End Region

#End Region

#Region "JobDispatch実績情報送信用XMLの構築"

    ''' <summary>
    ''' JobDispatch実績送信用XMLを構築する(メイン)
    ''' </summary>
    ''' <param name="inSysSettingValueRow">システム設定値データ行</param>
    ''' <param name="inDmsDlrbrnRow">基幹販売店・店舗コードデータ行</param>
    ''' <param name="inDmsDispatchInformation">DispatchInfomation情報データ行</param>
    ''' <param name="inDmsJobDetail">作業詳細リスト</param>
    ''' <param name="inStallInformation">StallInformation情報リスト</param>
    ''' <param name="inNowDateTime">現在日時</param>
    ''' <returns>構築したXMLドキュメント</returns>
    ''' <remarks></remarks>
    Private Function StructSendJobDispatchXml(ByVal inSysSettingValueRow As IC3802701SystemSettingValueRow, _
                                              ByVal inDmsDlrbrnRow As ServiceCommonClassDataSet.DmsCodeMapRow, _
                                              ByVal inDmsDispatchInformation As IC3802701DispatchInfoRow, _
                                              ByVal inDmsJobDetail As List(Of JobDetailClass), _
                                              ByVal inStallInformation As List(Of StallInfomationClass), _
                                              ByVal inNowDateTime As Date) As XmlDocument

        '引数をログに出力
        Dim args As New List(Of String)

        'DataRow内の項目を列挙
        Me.AddLogData(args, inSysSettingValueRow)
        Me.AddLogData(args, inDmsDlrbrnRow)
        Me.AddLogData(args, inDmsDispatchInformation)

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.S IN:{1}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  String.Join(", ", args.ToArray())))


        '65001がUTF-8
        Dim xmlEncode As Encoding = Encoding.GetEncoding(EncodeUtf8)

        'XMLドキュメント作成
        Dim xmlDocument As New XmlDocument

        'ヘッダ部作成(<?xml version="1.0" encoding="utf-8"?>の部分)
        Dim xmlDeclaration As XmlDeclaration = xmlDocument.CreateXmlDeclaration("1.0", _
                                                                                xmlEncode.BodyName, _
                                                                                Nothing)

        'ルートタグ(UpdateStatusタグ)の作成
        Dim xmlRoot As XmlElement = xmlDocument.CreateElement(TagJobClockOn)

        'headタグの構築
        Dim headTag As XmlElement = Me.StructSendJobDispatchXmlHeadTag(xmlDocument, _
                                                                       inSysSettingValueRow.DIST_CD, _
                                                                       inSysSettingValueRow.DATE_FORMAT, _
                                                                       inNowDateTime, _
                                                                       MessageId, _
                                                                       "0")

        'Detailタグの構築
        Dim detailTag As XmlElement = Me.StructSendJobDispatchXmlDetailTag(xmlDocument, _
                                                                           inDmsDlrbrnRow, _
                                                                           inDmsDispatchInformation, _
                                                                           inDmsJobDetail, _
                                                                           inStallInformation)

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
    ''' JobDispatch実績送信用XMLのheadタグを構築する
    ''' </summary>
    ''' <param name="xmlDocument">ステータス送信用XMLドキュメント</param>
    ''' <param name="countryCode">国コード</param>
    ''' <param name="dateFormat">日付フォーマット</param>
    ''' <param name="nowDateTime">現在日時</param>
    ''' <returns>headタグエレメント</returns>
    ''' <remarks></remarks>
    Private Function StructSendJobDispatchXmlHeadTag(ByVal xmlDocument As XmlDocument, _
                                                     ByVal countryCode As String, _
                                                     ByVal dateFormat As String, _
                                                     ByVal nowDateTime As Date, _
                                                     ByVal inMessageId As String, _
                                                     ByVal inlinkSystemCode As String) As XmlElement

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_S OUT:countryCode={1}, dateFormat={2}, dateFormat={3}", _
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
        messageIdTag.AppendChild(xmlDocument.CreateTextNode(inMessageId))
        countryCodeTag.AppendChild(xmlDocument.CreateTextNode(countryCode))
        linkSystemCodeTag.AppendChild(xmlDocument.CreateTextNode(inlinkSystemCode))
        TransmissionDateTag.AppendChild(xmlDocument.CreateTextNode(nowDateTime.ToString(dateFormat, CultureInfo.CurrentCulture)))

        'headタグを構築
        With headTag
            .AppendChild(messageIdTag)
            .AppendChild(countryCodeTag)
            .AppendChild(linkSystemCodeTag)
            .AppendChild(TransmissionDateTag)
        End With

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_E OUT:statusInfoTag={1}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  headTag.InnerXml))

        Return headTag

    End Function

    ''' <summary>
    ''' JobDispatch実績送信用XMLのDetailタグを構築する
    ''' </summary>
    ''' <param name="xmlDocument">JobDispatch実績送信用XMLドキュメント</param>
    ''' <param name="inDmsDlrbrnRow">基幹販売店・店舗コードデータ行</param>
    ''' <param name="inDmsDispatchInformation">DispatchInfomation情報データ行</param>
    ''' <param name="inDmsJobDetail">作業詳細リスト</param>
    ''' <param name="inStallInformation">StallInformation情報リスト</param>
    ''' <returns>JobDispatch実績送信用XMLクラスインスタンス</returns>
    ''' <remarks></remarks>
    Private Function StructSendJobDispatchXmlDetailTag(ByVal xmlDocument As XmlDocument, _
                                                       ByVal inDmsDlrbrnRow As ServiceCommonClassDataSet.DmsCodeMapRow, _
                                                       ByVal inDmsDispatchInformation As IC3802701DispatchInfoRow, _
                                                       ByVal inDmsJobDetail As List(Of JobDetailClass), _
                                                       ByVal inStallInformation As List(Of StallInfomationClass)) As XmlElement

        '引数をログに出力
        Dim args As New List(Of String)

        'DataRow内の項目を列挙
        Me.AddLogData(args, inDmsDispatchInformation)

        '開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.S IN:{1}" _
                    , MethodBase.GetCurrentMethod.Name _
                    , String.Join(", ", args.ToArray())))

        'Detailタグを作成
        Dim detailTag As XmlElement = xmlDocument.CreateElement(TagDetail)

        Try

            'Commonタグを構築
            Dim commonTag As XmlElement = Me.StructSendJobDispatchXmlCommonTag(xmlDocument, _
                                                                               inDmsDlrbrnRow, _
                                                                               inDmsDispatchInformation.VCL_VIN)

            If String.IsNullOrEmpty(commonTag.InnerXml) Then
                '必須チェックエラー
                detailTag.InnerXml = String.Empty
                Exit Try
            End If

            'DispatchInformationタグを構築
            Dim dispatchInfoTag As XmlElement = Me.StructSendJobDispatchXmlDispatchInfoTag(xmlDocument, _
                                                                                           inDmsDispatchInformation, _
                                                                                           inDmsJobDetail, _
                                                                                           inStallInformation)

            If String.IsNullOrEmpty(dispatchInfoTag.InnerXml) Then
                '必須チェックエラー
                detailTag.InnerXml = String.Empty
                Exit Try
            End If

            'Detailタグを構築
            With detailTag
                .AppendChild(commonTag)
                .AppendChild(dispatchInfoTag)
            End With

        Finally

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                      "{0}_E OUT:detailTag={1}", _
                      MethodBase.GetCurrentMethod.Name, detailTag.InnerXml))

        End Try

        Return detailTag

    End Function

    ''' <summary>
    ''' JobDispatch実績送信用XMLのCommonタグを構築する
    ''' </summary>
    ''' <param name="xmlDocument">ステータス送信用XMLドキュメント</param>
    ''' <param name="inDmsDlrbrnRow">基幹販売店・店舗コードデータ行</param>
    ''' <returns>ステータス送信用XMLクラスインスタンス</returns>
    ''' <remarks></remarks>
    Private Function StructSendJobDispatchXmlCommonTag(ByVal xmlDocument As XmlDocument, _
                                                       ByVal inDmsDlrbrnRow As ServiceCommonClassDataSet.DmsCodeMapRow, _
                                                       ByVal inVin As String) As XmlElement

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                          "{0}_S ", _
                          MethodBase.GetCurrentMethod.Name))

        'Commonタグを作成
        Dim commonTag As XmlElement = xmlDocument.CreateElement(TagCommon)

        If Me.CheckNecessaryCommonTag(inDmsDlrbrnRow.CODE1, inDmsDlrbrnRow.CODE2) Then
            '必須チェックOK

            'Commonタグの子要素を作成
            Dim dealerCodeTag As XmlElement = xmlDocument.CreateElement(TagDealerCode)
            Dim branchCodeTag As XmlElement = xmlDocument.CreateElement(TagBranchCode)
            Dim staffCodeTag As XmlElement = xmlDocument.CreateElement(TagStaffCode)
            Dim customerCodeTag As XmlElement = xmlDocument.CreateElement(TagCustomerCode)
            Dim salesBookingNumberTag As XmlElement = xmlDocument.CreateElement(TagSalesBookingNumber)
            Dim vinTag As XmlElement = xmlDocument.CreateElement(TagVin)

            '子要素に値を設定
            dealerCodeTag.AppendChild(xmlDocument.CreateTextNode(inDmsDlrbrnRow.CODE1))
            branchCodeTag.AppendChild(xmlDocument.CreateTextNode(inDmsDlrbrnRow.CODE2))
            If Not String.IsNullOrEmpty(inVin.Trim()) Then
                vinTag.AppendChild(xmlDocument.CreateTextNode(inVin.Trim()))
            End If

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

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                          "{0}_E OUT:commonTag={1}", _
                          MethodBase.GetCurrentMethod.Name, commonTag.InnerXml))

        Return commonTag

    End Function

    ''' <summary>
    ''' JobDispatch実績送信用XMLのDispatchInfomationタグを構築する
    ''' </summary>
    ''' <param name="xmlDocument">JobDispatch実績送信用XMLドキュメント</param>
    ''' <param name="inDmsDispatchInformation">DispatchInfomation情報データ行</param>
    ''' <param name="inDmsJobDetails">作業詳細リスト</param>
    ''' <param name="inStallInformations">StallInformation情報リスト</param>
    ''' <returns>JobDispatch実績送信用XMLクラスインスタンス</returns>
    ''' <remarks></remarks>
    Private Function StructSendJobDispatchXmlDispatchInfoTag(ByVal xmlDocument As XmlDocument, _
                                                             ByVal inDmsDispatchInformation As IC3802701DispatchInfoRow, _
                                                             ByVal inDmsJobDetails As List(Of JobDetailClass), _
                                                             ByVal inStallInformations As List(Of StallInfomationClass)) As XmlElement
        '引数をログに出力
        Dim args As New List(Of String)

        'DataRow内の項目を列挙
        Me.AddLogData(args, inDmsDispatchInformation)

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.S IN:{1}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  String.Join(", ", args.ToArray())))

        'DispatchInfomationタグを作成
        Dim dispatchInfoTag As XmlElement = xmlDocument.CreateElement(TagDispatchInformation)

        'DispatchInfomationタグの子要素を作成
        Dim seqNoTag As XmlElement = xmlDocument.CreateElement(TagSeqNo)
        Dim rezidTag As XmlElement = xmlDocument.CreateElement(TagRezId)
        Dim basRezidTag As XmlElement = xmlDocument.CreateElement(TagBasRezId)
        Dim roTag As XmlElement = xmlDocument.CreateElement(TagRo)
        Dim startTimeTag As XmlElement = xmlDocument.CreateElement(TagStartTime)
        Dim endTimeTag As XmlElement = xmlDocument.CreateElement(TagEndTime)

        '子要素に値を設定
        seqNoTag.AppendChild(xmlDocument.CreateTextNode(inDmsDispatchInformation.SEQ_NO))
        rezidTag.AppendChild(xmlDocument.CreateTextNode(CType(inDmsDispatchInformation.JOB_DTL_ID, String)))

        If Not inDmsDispatchInformation.IsDMS_JOB_DTL_IDNull _
            AndAlso Not String.IsNullOrEmpty(inDmsDispatchInformation.DMS_JOB_DTL_ID.Trim()) Then
            basRezidTag.AppendChild(xmlDocument.CreateTextNode(CType(inDmsDispatchInformation.DMS_JOB_DTL_ID.Trim(), String)))
        End If

        If Not inDmsDispatchInformation.IsRO_NUMNull _
            AndAlso Not String.IsNullOrEmpty(inDmsDispatchInformation.RO_NUM.Trim()) Then
            roTag.AppendChild(xmlDocument.CreateTextNode(inDmsDispatchInformation.RO_NUM))
        End If

        If Not inDmsDispatchInformation.IsSTART_DATETIMENull _
            AndAlso Not inDmsDispatchInformation.START_DATETIME = CType(Nothing, Date) Then
            startTimeTag.AppendChild(xmlDocument.CreateTextNode(inDmsDispatchInformation.START_DATETIME.ToString(DateFormatYYYYMMddHHmmss, CultureInfo.CurrentCulture)))
        End If

        If Not inDmsDispatchInformation.IsEND_DATETIMENull _
            AndAlso Not inDmsDispatchInformation.END_DATETIME = CType(Nothing, Date) Then
            endTimeTag.AppendChild(xmlDocument.CreateTextNode(inDmsDispatchInformation.END_DATETIME.ToString(DateFormatYYYYMMddHHmmss, CultureInfo.CurrentCulture)))
        End If

        'DispatchInfomationタグの子要素を追加
        With dispatchInfoTag
            .AppendChild(seqNoTag)
            .AppendChild(rezidTag)
            .AppendChild(basRezidTag)
            .AppendChild(roTag)
            .AppendChild(startTimeTag)
            .AppendChild(endTimeTag)
        End With

        'DispatchInfomationタグにJobDetailタグ内容追加
        For Each dmsJobDetail As JobDetailClass In inDmsJobDetails

            Dim jobDetailTag As XmlElement = Me.StructSendJobDispatchXmlJobDetailTag(xmlDocument, _
                                                                                     dmsJobDetail)
            dispatchInfoTag.AppendChild(jobDetailTag)

        Next

        'DispatchInfomationタグにStallInformationタグ内容追加
        For Each stallInformation As StallInfomationClass In inStallInformations

            Dim stallInfo As XmlElement = Me.StructSendJobDispatchXmlStallInfoTag(xmlDocument, _
                                                                                  stallInformation)
            dispatchInfoTag.AppendChild(stallInfo)

        Next

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                          "{0}.E OUT:STRUCTXML = " & vbCrLf & "{1}", _
                          MethodBase.GetCurrentMethod.Name, _
                          Me.FormatXml(xmlDocument)))

        Return dispatchInfoTag

    End Function

    ''' <summary>
    ''' JobDispatch実績送信用XMLのJobDetailタグを構築する
    ''' </summary>
    ''' <param name="xmlDocument">JobDetail実績送信用XMLドキュメント</param>
    ''' <param name="inDmsJobDetail">作業詳細リスト</param>
    ''' <returns>JobDetail実績送信用XMLクラスインスタンス</returns>
    ''' <remarks></remarks>
    Private Function StructSendJobDispatchXmlJobDetailTag(ByVal xmlDocument As XmlDocument, _
                                                          ByVal inDmsJobDetail As JobDetailClass) As XmlElement

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.S ", _
                                  MethodBase.GetCurrentMethod.Name))

        'JobDetailタグを作成
        Dim jobDetailTag As XmlElement = xmlDocument.CreateElement(TagJobDetail)

        'JobDetailタグの子要素を作成
        Dim dispatchNoTag As XmlElement = xmlDocument.CreateElement(TagDispatchNo)
        Dim jobSequenceNumberTag As XmlElement = xmlDocument.CreateElement(TagJobSequenceNumber)
        Dim jobidTag As XmlElement = xmlDocument.CreateElement(TagJobId)
        Dim groupidTag As XmlElement = xmlDocument.CreateElement(TagGroupId)
        Dim statusTag As XmlElement = xmlDocument.CreateElement(TagStatus)
        Dim startTimeTag As XmlElement = xmlDocument.CreateElement(TagRsltStartTime)
        Dim endTimeTag As XmlElement = xmlDocument.CreateElement(TagRsltEndTime)
        Dim workTimeTag As XmlElement = xmlDocument.CreateElement(TagWorkTime)
        Dim fmAccountTag As XmlElement = xmlDocument.CreateElement(FmAccount)
        Dim inspectionFlgTag As XmlElement = xmlDocument.CreateElement(InspectionFlg)

        '子要素に値を設定
        If Not String.IsNullOrEmpty(inDmsJobDetail.DispatchNo.Trim()) Then
            dispatchNoTag.AppendChild(xmlDocument.CreateTextNode(inDmsJobDetail.DispatchNo.Trim()))
        End If

        jobSequenceNumberTag.AppendChild(xmlDocument.CreateTextNode(CType(inDmsJobDetail.JobSequenceNumber, String)))

        If Not String.IsNullOrEmpty(inDmsJobDetail.JobID.Trim()) Then
            jobidTag.AppendChild(xmlDocument.CreateTextNode(inDmsJobDetail.JobID.Trim()))
        End If

        If Not String.IsNullOrEmpty(inDmsJobDetail.GroupID.Trim()) Then
            groupidTag.AppendChild(xmlDocument.CreateTextNode(inDmsJobDetail.GroupID.Trim()))
        End If

        statusTag.AppendChild(xmlDocument.CreateTextNode(inDmsJobDetail.Status))

        If Not inDmsJobDetail.StartTime = CType(Nothing, Date) Then
            startTimeTag.AppendChild(xmlDocument.CreateTextNode(inDmsJobDetail.StartTime.ToString(DateFormatYYYYMMddHHmmss, CultureInfo.CurrentCulture)))
        End If

        If Not inDmsJobDetail.EndTime = CType(Nothing, Date) Then
            endTimeTag.AppendChild(xmlDocument.CreateTextNode(inDmsJobDetail.EndTime.ToString(DateFormatYYYYMMddHHmmss, CultureInfo.CurrentCulture)))
        End If

        If inDmsJobDetail.WorkTime > 0 Then
            workTimeTag.AppendChild(xmlDocument.CreateTextNode(CType(inDmsJobDetail.WorkTime, String)))
        End If

        If Not String.IsNullOrEmpty(inDmsJobDetail.FMAccount.Trim()) Then
            fmAccountTag.AppendChild(xmlDocument.CreateTextNode(ConvertAccount(inDmsJobDetail.FMAccount).Trim()))
        End If

        If Not String.IsNullOrEmpty(inDmsJobDetail.InspectionFlg.Trim()) Then
            inspectionFlgTag.AppendChild(xmlDocument.CreateTextNode(CType(inDmsJobDetail.InspectionFlg.Trim(), String)))
        End If

        'JobDetailタグの子要素を追加
        With jobDetailTag
            .AppendChild(dispatchNoTag)
            .AppendChild(jobSequenceNumberTag)
            .AppendChild(jobidTag)
            .AppendChild(groupidTag)
            .AppendChild(statusTag)
            .AppendChild(startTimeTag)
            .AppendChild(endTimeTag)
            .AppendChild(workTimeTag)
            .AppendChild(fmAccountTag)
            .AppendChild(inspectionFlgTag)
        End With

        'jobDetailタグにstopInfoタグ内容追加
        If IsNothing(inDmsJobDetail.StopInformation) Then
            '名前しかない白の中断タブxmlを取得する
            Dim stopInfo As XmlElement = Me.StructSendJobDispatchXmlStopInfoTag(xmlDocument, _
                                                                                Nothing)
            jobDetailTag.AppendChild(stopInfo)
        Else

            For Each stopInformation As StopInfomationClass In inDmsJobDetail.StopInformation

                Dim stopInfo As XmlElement = Me.StructSendJobDispatchXmlStopInfoTag(xmlDocument, _
                                                                                    stopInformation)
                jobDetailTag.AppendChild(stopInfo)

            Next
        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                          "{0}.E OUT:STRUCTXML = " & vbCrLf & "{1}", _
                          MethodBase.GetCurrentMethod.Name, _
                          Me.FormatXml(xmlDocument)))

        Return jobDetailTag

    End Function

    ''' <summary>
    ''' JobDispatch実績送信用XMLのStopInfomationタグを構築する
    ''' </summary>
    ''' <param name="xmlDocument">JobDispatch実績送信用XMLドキュメント</param>
    ''' <param name="inStopInformation">中断情報</param>
    ''' <returns>JobDispatch実績送信用XMLクラスインスタンス</returns>
    ''' <remarks></remarks>
    Private Function StructSendJobDispatchXmlStopInfoTag(ByVal xmlDocument As XmlDocument, _
                                                         ByVal inStopInformation As StopInfomationClass) As XmlElement

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.S ", _
                                  MethodBase.GetCurrentMethod.Name))

        'StopInformationタグを作成
        Dim stopInfoTag As XmlElement = xmlDocument.CreateElement(TagStopInformation)

        'StopInformationタグの子要素を作成
        Dim stopSequenceNumberTag As XmlElement = xmlDocument.CreateElement(TagStopSequenceNumber)
        Dim stopStartTag As XmlElement = xmlDocument.CreateElement(TagStopStart)
        Dim stopEnd As XmlElement = xmlDocument.CreateElement(TagStopEnd)
        Dim stopReason As XmlElement = xmlDocument.CreateElement(TagStopReason)

        '中断情報データがあれば、設定する
        If Not IsNothing(inStopInformation) Then
            '子要素に値を設定
            stopSequenceNumberTag.AppendChild(xmlDocument.CreateTextNode(CType(inStopInformation.StopSequenceNumber, String)))
            stopStartTag.AppendChild(xmlDocument.CreateTextNode(inStopInformation.StopStart.ToString(DateFormatYYYYMMddHHmmss, CultureInfo.CurrentCulture)))
            stopEnd.AppendChild(xmlDocument.CreateTextNode(inStopInformation.StopEnd.ToString(DateFormatYYYYMMddHHmmss, CultureInfo.CurrentCulture)))
            stopReason.AppendChild(xmlDocument.CreateTextNode(inStopInformation.StopReason))
        End If

        'JobDetailタグの子要素を追加
        With stopInfoTag
            .AppendChild(stopSequenceNumberTag)
            .AppendChild(stopStartTag)
            .AppendChild(stopEnd)
            .AppendChild(stopReason)
        End With

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                          "{0}.E OUT:STRUCTXML = " & vbCrLf & "{1}", _
                          MethodBase.GetCurrentMethod.Name, _
                          Me.FormatXml(xmlDocument)))

        Return stopInfoTag

    End Function

    ''' <summary>
    ''' JobDispatch実績送信用XMLのStallInfomationタグを構築する
    ''' </summary>
    ''' <param name="xmlDocument">JobDispatch実績送信用XMLドキュメント</param>
    ''' <param name="inStallInformations">ストール情報</param>
    ''' <returns>JobDispatch実績送信用XMLクラスインスタンス</returns>
    ''' <remarks></remarks>
    Private Function StructSendJobDispatchXmlStallInfoTag(ByVal xmlDocument As XmlDocument, _
                                                          ByVal inStallInformations As StallInfomationClass) As XmlElement

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.S ", _
                                  MethodBase.GetCurrentMethod.Name))

        'StallInfomationタグを作成
        Dim stallInfoTag As XmlElement = xmlDocument.CreateElement(TagStallInfomation)

        'DispatchInfomationタグの子要素を作成
        Dim stallidTag As XmlElement = xmlDocument.CreateElement(TagStallId)
        Dim startTimeTag As XmlElement = xmlDocument.CreateElement(TagWorkStartTime)
        Dim endTimeTag As XmlElement = xmlDocument.CreateElement(TagWorkEndTime)

        '子要素に値を設定

        'ストールID
        Dim dmsStallId As String = Me.ConvertToDmsStallId(inStallInformations.StallId)
        stallidTag.AppendChild(xmlDocument.CreateTextNode(dmsStallId))

        If Not inStallInformations.StartTime = Date.Parse(DefaultDate, CultureInfo.InvariantCulture) _
            And Not inStallInformations.StartTime = Date.MinValue Then
            startTimeTag.AppendChild(xmlDocument.CreateTextNode(inStallInformations.StartTime.ToString(DateFormatYYYYMMddHHmmss, CultureInfo.CurrentCulture)))
        End If

        If Not inStallInformations.EndTime = Date.Parse(DefaultDate, CultureInfo.InvariantCulture) _
            And Not inStallInformations.EndTime = Date.MinValue Then
            endTimeTag.AppendChild(xmlDocument.CreateTextNode(inStallInformations.EndTime.ToString(DateFormatYYYYMMddHHmmss, CultureInfo.CurrentCulture)))
        End If

        'StallInfomationタグの子要素を追加
        With stallInfoTag
            .AppendChild(stallidTag)
            .AppendChild(startTimeTag)
            .AppendChild(endTimeTag)
        End With

        'StallInfomationタグにTechnicianInformationタグ内容追加
        If IsNothing(inStallInformations.TechnicianId) Then

            '空白のTechnicianInformationタグを作成
            Dim technicianInfoTag As XmlElement = xmlDocument.CreateElement(TagTechnicianInformation)
            'TechnicianIdタグを作成
            Dim technicianIdTag As XmlElement = xmlDocument.CreateElement(TagTechnicianId)
            'TechnicianIdタグの子要素を追加
            technicianInfoTag.AppendChild(technicianIdTag)
            'StallInfomationタグにTechnicianInformationタグを追加
            stallInfoTag.AppendChild(technicianInfoTag)

        Else

            For Each staffJobRow As IC3802701StaffJobRow In inStallInformations.TechnicianId
                'TechnicianInformationタグを作成
                Dim technicianInfoTag As XmlElement = Me.StructSendJobDispatchXmlTechnicianInfoTag(xmlDocument, _
                                                                                                   staffJobRow)

                'StallInfomationタグにTechnicianInformationタグを追加
                stallInfoTag.AppendChild(technicianInfoTag)
            Next
        End If

        'DispatchInfomationタグにStallInformationタグ内容追加
        If inStallInformations.RestInformation.Length = 0 Then
            '空白のRestInfomationタグを作成
            Dim restInfoTag As XmlElement = xmlDocument.CreateElement(TagRestInfomation)
            Dim restInfoAttr As XmlAttribute = xmlDocument.CreateAttribute("xsi", "nil", "http://www.w3.org/2001/XMLSchema-instance")
            restInfoAttr.Value = "true"
            restInfoTag.SetAttributeNode(restInfoAttr)
            stallInfoTag.AppendChild(restInfoTag)
        Else
            For Each restInfoRow As IC3802701StallIdleRow In inStallInformations.RestInformation

                Dim restInfo As XmlElement = Me.StructSendJobDispatchXmlRestInfoTag(xmlDocument, _
                                                                                    restInfoRow)
                stallInfoTag.AppendChild(restInfo)

            Next
        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                          "{0}.E OUT:STRUCTXML = " & vbCrLf & "{1}", _
                          MethodBase.GetCurrentMethod.Name, _
                          Me.FormatXml(xmlDocument)))

        Return stallInfoTag

    End Function

    ''' <summary>
    ''' JobDispatch実績送信用XMLのRestInformationタグを構築する
    ''' </summary>
    ''' <param name="xmlDocument">JobDispatch実績送信用XMLドキュメント</param>
    ''' <param name="inTechnicianInfoRow">実績チップ操作するテクニシャン</param>
    ''' <returns>JobDispatch実績送信用XMLクラスインスタンス</returns>
    ''' <remarks></remarks>
    Private Function StructSendJobDispatchXmlTechnicianInfoTag(ByVal xmlDocument As XmlDocument, _
                                                               ByVal inTechnicianInfoRow As IC3802701StaffJobRow) As XmlElement

        '引数をログに出力
        Dim args As New List(Of String)

        'DataRow内の項目を列挙
        Me.AddLogData(args, inTechnicianInfoRow)

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.S IN:{1}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  String.Join(", ", args.ToArray())))

        'TechnicianInformationタグを作成
        Dim technicianInfoTag As XmlElement = xmlDocument.CreateElement(TagTechnicianInformation)

        'TechnicianIdタグを作成
        Dim technicianIdTag As XmlElement = xmlDocument.CreateElement(TagTechnicianId)

        'TechnicianId要素に値を設定
        technicianIdTag.AppendChild(xmlDocument.CreateTextNode(ConvertAccount(inTechnicianInfoRow.STF_CD)))

        'TechnicianIdタグの子要素を追加
        technicianInfoTag.AppendChild(technicianIdTag)

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                          "{0}.E OUT:STRUCTXML = " & vbCrLf & "{1}", _
                          MethodBase.GetCurrentMethod.Name, _
                          Me.FormatXml(xmlDocument)))

        Return technicianInfoTag

    End Function

    ''' <summary>
    ''' JobDispatch実績送信用XMLのRestInformationタグを構築する
    ''' </summary>
    ''' <param name="xmlDocument">JobDispatch実績送信用XMLドキュメント</param>
    ''' <param name="inRestInfoRow">実績チップと被ってる非稼働エリア情報</param>
    ''' <returns>JobDispatch実績送信用XMLクラスインスタンス</returns>
    ''' <remarks></remarks>
    Private Function StructSendJobDispatchXmlRestInfoTag(ByVal xmlDocument As XmlDocument, _
                                                         ByVal inRestInfoRow As IC3802701StallIdleRow) As XmlElement

        '引数をログに出力
        Dim args As New List(Of String)

        'DataRow内の項目を列挙
        Me.AddLogData(args, inRestInfoRow)

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.S IN:{1}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  String.Join(", ", args.ToArray())))

        'RestInfomationタグを作成
        Dim stallInfoTag As XmlElement = xmlDocument.CreateElement(TagRestInfomation)

        'RestInfomationタグの子要素を作成
        Dim startTimeTag As XmlElement = xmlDocument.CreateElement(TagIdleStartTime)
        Dim endTimeTag As XmlElement = xmlDocument.CreateElement(TagIdleEndTime)

        '子要素に値を設定
        startTimeTag.AppendChild(xmlDocument.CreateTextNode(inRestInfoRow.IDLE_START_DATETIME.ToString(DateFormatYYYYMMddHHmmss, CultureInfo.CurrentCulture)))
        endTimeTag.AppendChild(xmlDocument.CreateTextNode(inRestInfoRow.IDLE_END_DATETIME.ToString(DateFormatYYYYMMddHHmmss, CultureInfo.CurrentCulture)))

        'RestInfomationタグの子要素を追加
        With stallInfoTag
            .AppendChild(startTimeTag)
            .AppendChild(endTimeTag)
        End With

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                          "{0}.E OUT:STRUCTXML = " & vbCrLf & "{1}", _
                          MethodBase.GetCurrentMethod.Name, _
                          Me.FormatXml(xmlDocument)))

        Return stallInfoTag

    End Function

#End Region

#Region "JobDispatch実績情報送信用XMLの送受信"

    ''' <summary>
    ''' WebServiceにXMLを送信し、結果を受信する
    ''' </summary>
    ''' <param name="sendXml">送信XML文字列</param>
    ''' <param name="webServiceUrl">送信先URL</param>
    ''' <returns>受信XML文字列</returns>
    ''' <remarks></remarks>
    Private Function ExecuteSendJobDispatchInfoXml(ByVal sendXml As String, _
                                                   ByVal webServiceUrl As String, _
                                                   ByVal timeOutValue As String, _
                                                   ByVal soapVersion As String) As String

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_S IN:sendXml={1}, webServiceUrl={2}, timeOutValue={3}", _
                                  MethodBase.GetCurrentMethod.Name, sendXml, webServiceUrl, timeOutValue))

        'SoapClientインスタンスを生成
        Using service As New JobClockOnImportDMS

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
                'XML送信
                resultString = service.IC45202(sendXml)

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
                          MethodBase.GetCurrentMethod.Name, resultString))

            Return resultString

        End Using

    End Function

#End Region

#Region "WebServiceの戻りXMLを解析し値を取得"
    ''' <summary>
    ''' WebServiceの戻りXMLを解析し値を取得
    ''' </summary>
    ''' <param name="resultString">送信XML文字列</param>
    ''' <returns>結果XML</returns>
    ''' <remarks></remarks>
    Private Function GetSendJobDispatchXmlResultData(ByVal resultString As String) As Dictionary(Of String, String)

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

            'ステータス送信の返却XML内の必要なタグがない場合、エラー(戻り値Nothing)とする
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
    ''' XMLをインデントを付加して整形する(ログ出力用)
    ''' </summary>
    ''' <param name="xmlDoc">XMLドキュメント</param>
    ''' <returns>整形後XML文字列</returns>
    ''' <remarks></remarks>
    Private Function FormatXml(ByVal xmlDoc As XmlDocument) As String

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_S", _
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
                                          "{0}_E", _
                                          MethodBase.GetCurrentMethod.Name))

                Return textWriter.ToString()

            Finally
                xmlWriter = Nothing
            End Try

        End Using

    End Function

#End Region

#Region "チェック用メッソド"

    ''' <summary>
    ''' Commonタグ内の設定値必須チェックを行う
    ''' </summary>
    ''' <param name="dmsDealerCode">基幹販売店コード</param>
    ''' <param name="dmsBranchCode">基幹店舗コード</param>
    ''' <returns>チェックOK：True/チェックNG：False</returns>
    ''' <remarks></remarks>
    Private Function CheckNecessaryCommonTag(ByVal dmsDealerCode As String, ByVal dmsBranchCode As String) As Boolean

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                      "{0}.S IN:dmsDealerCode:{1}, dmsBranchCode:{2}", _
                      MethodBase.GetCurrentMethod.Name, dmsDealerCode, dmsBranchCode))

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

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                      "{0}.E OUT:retCheckOkFlg:{1}", _
                      MethodBase.GetCurrentMethod.Name, retCheckOkFlg))

        Return retCheckOkFlg

    End Function

    ''' <summary>
    ''' 作業実績送信の返却XML内の必要なタグ存在チェックを行う
    ''' </summary>
    ''' <param name="xmlElement"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CheckResultXmlElementTag(ByVal xmlElement As XmlElement) As Boolean

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.S IN:xmlElement:{1}", _
                                  MethodBase.GetCurrentMethod.Name, xmlElement))

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

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.E OUT:retCheckOkFlg:{1}", _
                                  MethodBase.GetCurrentMethod.Name, retCheckOkFlg))

        Return retCheckOkFlg

    End Function

#End Region

#Region "その他"

    ''' <summary>
    ''' 基幹作業ステータスに転換する
    ''' </summary>
    ''' <param name="inJobStatus">作業実績．作業ステータス</param>
    ''' <returns>正しいの場合：基幹作業ステータス　エラーの場合：String.Empty</returns>
    ''' <remarks></remarks>
    Private Function ConvertToDmsJobStatus(ByVal inJobStatus As String, _
                                           ByVal dlrCode As String) As String

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_S inJobStatus={1}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  inJobStatus))

        Dim dmsJobStatus As String = String.Empty
        Dim dmsDlrBrnTable As ServiceCommonClassDataSet.DmsCodeMapDataTable = Nothing

        'コードマップエンティティを利用して期間作業ステータスに変換する
        Using serviceCommonBiz As New ServiceCommonClassBusinessLogic
            dmsDlrBrnTable = serviceCommonBiz.GetIcropToDmsCode(dlrCode, _
                                                                ServiceCommonClassBusinessLogic.DmsCodeType.WorkStatus, _
                                                                inJobStatus, _
                                                                String.Empty, _
                                                                String.Empty)
        End Using

        If dmsDlrBrnTable.Count <= 0 Then

            'データが取得できない場合はエラー
            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                       "{0}.Error ErrCode:{1}, Failed to convert DMS jobstatus.(No data found)", _
                                       MethodBase.GetCurrentMethod.Name, _
                                       ErrorDmsCodeMap))
            Return Nothing

        ElseIf 1 < dmsDlrBrnTable.Count Then

            'データが2件以上取得できた場合は一意に決定できないためエラー
            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                       "{0}.Error ErrCode:{1}, Failed to convert DMS jobstatus.(Non-unique)", _
                                       MethodBase.GetCurrentMethod.Name, _
                                       ErrorDmsCodeMap))
            Return Nothing

        Else

            '期間作業ステータスを取得する
            dmsJobStatus = dmsDlrBrnTable(0).CODE1

            '2016/06/23 NSK 皆川 TR-SVT-TMT-20151110-001 技術者がタブレットでジョブを停止できない START
            If DmsJobStatusStop.Equals(dmsJobStatus) Then
                '中断中は作業中として送信する
                dmsJobStatus = DmsJobStatusWorking

            End If
            '2016/06/23 NSK 皆川 TR-SVT-TMT-20151110-001 技術者がタブレットでジョブを停止できない END

        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                          "{0}.E OUT:dmsJobStatus={1}", _
                          MethodBase.GetCurrentMethod.Name, _
                          dmsJobStatus))

        Return dmsJobStatus

    End Function

    ''' <summary>
    ''' 基幹中断理由に転換する
    ''' </summary>
    ''' <param name="inStopReason">i-CROPの作業実績．中断理由区分</param>
    ''' <returns></returns>
    ''' <remarks>
    ''' i-CROPの作業実績．中断理由区分「0：その他、1：部品欠品、2：お客様連絡待ち」を
    ''' 基幹中断理由にコードマップエンティティを利用して変換する。
    ''' </remarks>
    Private Function ConvertToDmsStopReason(ByVal inStopReason As String, _
                                            ByVal dlrCode As String) As String

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_S inStopReason={1}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  inStopReason))

        Dim dmsStopReason As String = String.Empty
        Dim dmsDlrBrnTable As ServiceCommonClassDataSet.DmsCodeMapDataTable = Nothing

        'コードマップエンティティを利用して期間作業ステータスに変換する
        Using serviceCommonBiz As New ServiceCommonClassBusinessLogic
            dmsDlrBrnTable = serviceCommonBiz.GetIcropToDmsCode(dlrCode, _
                                                                ServiceCommonClassBusinessLogic.DmsCodeType.JobStopReasonType, _
                                                                inStopReason, _
                                                                String.Empty, _
                                                                String.Empty)
        End Using

        If dmsDlrBrnTable.Count <= 0 Then

            'データが取得できない場合はエラー
            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                       "{0}.Error ErrCode:{1}, Failed to convert jobstatus.(No data found)", _
                                       MethodBase.GetCurrentMethod.Name, _
                                       ErrorDmsCodeMap))
            Return Nothing

        ElseIf 1 < dmsDlrBrnTable.Count Then

            'データが2件以上取得できた場合は一意に決定できないためエラー
            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                       "{0}.Error ErrCode:{1}, Failed to convert jobstatus.(Non-unique)", _
                                       MethodBase.GetCurrentMethod.Name, _
                                       ErrorDmsCodeMap))
            Return Nothing
        Else

            '基幹中断理由を取得する
            dmsStopReason = dmsDlrBrnTable(0).CODE1

        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                          "{0}.E OUT:dmsStopReason={1}", _
                          MethodBase.GetCurrentMethod.Name, _
                          dmsStopReason))
        Return dmsStopReason

    End Function

    ''' <summary>
    ''' 基幹ストールIDに転換する
    ''' </summary>
    ''' <param name="inStallId">変更前ストールID</param>
    ''' <returns>変更後ストールID</returns>
    ''' <remarks></remarks>
    Private Function ConvertToDmsStallId(ByVal inStallId As Decimal) As String

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_S inStallId={1}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  inStallId))

        Dim dmsDlrBrnTable As ServiceCommonClassDataSet.DmsCodeMapDataTable = Nothing

        'ログインスタッフ情報取得
        Dim userContext As StaffContext = StaffContext.Current

        'コードマップエンティティを利用して期間作業ステータスに変換する
        Using serviceCommonBiz As New ServiceCommonClassBusinessLogic
            dmsDlrBrnTable = serviceCommonBiz.GetIcropToDmsCode(userContext.DlrCD, _
                                                                ServiceCommonClassBusinessLogic.DmsCodeType.StallId, _
                                                                userContext.DlrCD, _
                                                                userContext.BrnCD, _
                                                                CType(inStallId, String))
        End Using

        If dmsDlrBrnTable.Count <> 1 Then

            'データが取得できない場合、元のストールを返す
            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}.E OUT:Return stallId={1} (No Change)", _
                                      MethodBase.GetCurrentMethod.Name, _
                                      inStallId))
            Return inStallId.ToString(CultureInfo.CurrentCulture)
        Else

            'データが取得できる場合、基幹ストールIDを戻す
            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}.E OUT:return Dms stallId={1} ", _
                                      MethodBase.GetCurrentMethod.Name, _
                                      dmsDlrBrnTable(0).CODE3))
            '基幹ストールIDを取得する
            Return dmsDlrBrnTable(0).CODE3

        End If

    End Function

    ''' <summary>
    ''' アカウントの@以降は削除
    ''' </summary>
    ''' <param name="InAccount">変える前のアカウント</param>
    ''' <returns>変える後のアカウント</returns>
    ''' <remarks></remarks>
    Private Function ConvertAccount(ByVal InAccount As String) As String

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.Start. InAccount={1}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  InAccount))

        Dim index As Integer = InAccount.IndexOf("@", StringComparison.OrdinalIgnoreCase)

        '見つからない場合、そのままで戻る
        If index < 0 Then
            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}.End. OUT:{1}", _
                                      MethodBase.GetCurrentMethod.Name, _
                                      InAccount))
            Return InAccount
        End If

        InAccount = InAccount.Substring(0, index)

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.End. OUT:{1}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  InAccount))

        Return InAccount
    End Function

    ''' <summary>
    ''' ディフォルト値(DB日付型の既定値（1900-1-1 00:00:00）)を判断する
    ''' </summary>
    ''' <param name="para"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function IsDefaultValue(ByVal para As Date) As Boolean
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. para={1}" _
            , MethodBase.GetCurrentMethod.Name, para))

        If para = Date.Parse("1900/01/01 00:00:00", CultureInfo.InvariantCulture) Then
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Return True", MethodBase.GetCurrentMethod.Name))
            Return True
        Else
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Return False", MethodBase.GetCurrentMethod.Name))
            Return False
        End If
    End Function

    ''' <summary>
    ''' 作業ステータス連携制御により、送信するかどうかをチェック
    ''' </summary>
    ''' <param name="inPrevJobStatus">更新前作業連携ステータス</param>
    ''' <param name="inCrntJobStatus">更新後作業連携ステータス</param>
    ''' <param name="inJobDtlId">操作チップの作業内容ID</param>
    ''' <returns>送信するかどうか：trueの場合、送信する</returns>
    ''' <remarks></remarks>
    Private Function IsSendJobClock(ByVal inPrevJobStatus As IC3802701JobStatusDataTable, _
                                    ByVal inCrntJobStatus As IC3802701JobStatusDataTable, _
                                    ByVal inJobDtlId As Decimal) As Boolean

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_Start inJobDtlId={1} ", _
                                  MethodBase.GetCurrentMethod.Name, inJobDtlId))

        Dim jobDetailTable As IC3802701JobDetailDataTable = Nothing

        'JobDetailの情報を取得する
        Using ic3802701Ta As New IC3802701DataTableAdapter
            jobDetailTable = ic3802701Ta.GetJobDetailByJobDtlId(inJobDtlId)
        End Using

        For Each jobDetail As IC3802701JobDetailRow In jobDetailTable

            '更新前の作業ステータスを取得する
            Dim prevJobStatus As IC3802701JobStatusRow() = _
                CType(inPrevJobStatus.Select(String.Format(CultureInfo.CurrentCulture, _
                                                           "JOB_DTL_ID = '{0}' AND JOB_INSTRUCT_ID = '{1}' AND JOB_INSTRUCT_SEQ = '{2}'", _
                                                           jobDetail.JOB_DTL_ID, _
                                                           jobDetail.JOB_INSTRUCT_ID, _
                                                           jobDetail.JOB_INSTRUCT_SEQ)),  _
                                                    IC3802701JobStatusRow())

            '更新後の作業ステータスを取得する
            Dim crntJobStatus As IC3802701JobStatusRow() = _
                CType(inCrntJobStatus.Select(String.Format(CultureInfo.CurrentCulture, _
                                                           "JOB_DTL_ID = '{0}' AND JOB_INSTRUCT_ID = '{1}' AND JOB_INSTRUCT_SEQ = '{2}'", _
                                                           jobDetail.JOB_DTL_ID, _
                                                           jobDetail.JOB_INSTRUCT_ID, _
                                                           jobDetail.JOB_INSTRUCT_SEQ)),  _
                                                    IC3802701JobStatusRow())

            '全部見つけた場合、送信フラグを取得する
            If prevJobStatus.Count = 1 And crntJobStatus.Count = 1 Then
                '2016/06/23 NSK 皆川 TR-SVT-TMT-20151110-001 技術者がタブレットでジョブを停止できない START
                'Dim sendFlg As String = Me.GetSendReserveFlg(prevJobStatus(0).JOB_STATUS, _
                '                                             crntJobStatus(0).JOB_STATUS)

                ''送信の場合、戻る用テーブルに追加
                'If SendStatus.Equals(sendFlg) Then
                '    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                '                              "{0}.End Return true", _
                '                              MethodBase.GetCurrentMethod.Name))
                '    Return True
                'End If

                '変更前後でステータスが変更されている場合
                If Not String.Equals(prevJobStatus(0).JOB_STATUS, crntJobStatus(0).JOB_STATUS) Then
                    Dim sendFlg As String = Me.GetSendReserveFlg(prevJobStatus(0).JOB_STATUS, _
                                                                 crntJobStatus(0).JOB_STATUS)
                    '送信の場合、戻る用テーブルに追加
                    If SendStatus.Equals(sendFlg) Then
                        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                                  "{0}.End Return true", _
                                                  MethodBase.GetCurrentMethod.Name))
                        Return True
                    End If
                End If
                '2016/06/23 NSK 皆川 TR-SVT-TMT-20151110-001 技術者がタブレットでジョブを停止できない END
            End If

        Next

        '2016/06/23 NSK 皆川 TR-SVT-TMT-20151110-001 技術者がタブレットでジョブを停止できない START
        '変更前後の全ての作業ステータスが終了の場合
        If IsAllJobStatusFinish(inPrevJobStatus) AndAlso IsAllJobStatusFinish(inCrntJobStatus) Then

            '完成検査承認時の送信フラグを取得
            Dim sendFlg As String = Me.GetSendReserveFlg(JobLinkStatusFinish, JobLinkStatusFinish)
            If SendStatus.Equals(sendFlg) Then
                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                          "{0}.End Return true", _
                                          MethodBase.GetCurrentMethod.Name))
                Return True
            End If
        End If
        '2016/06/23 NSK 皆川 TR-SVT-TMT-20151110-001 技術者がタブレットでジョブを停止できない END

        'sendflgが1のレコードがないので、送信しない
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.End Return false", _
                                  MethodBase.GetCurrentMethod.Name))

        Return False

    End Function

    ''' <summary>
    ''' Jobの作業ステータスが全て終了か確認する
    ''' </summary>
    ''' <param name="inJobStatus">作業連携ステータス</param>
    ''' <returns>Jobの作業ステータスが全て終了かどうか：trueの場合、全て終了</returns>
    ''' <remarks></remarks>
    Private Function IsAllJobStatusFinish(ByVal inJobStatus As IC3802701JobStatusDataTable) As Boolean

        For Each jobStatus In inJobStatus

            If Not JobLinkStatusFinish.Equals(jobStatus.JOB_STATUS) Then
                '作業ステータスが終了以外のものがある場合
                Return False
            End If

        Next

        '作業ステータスが全て終了の場合
        Return True

    End Function
#End Region

#End Region

#Region "Privateクラス"

    ''' <summary>
    ''' 作業詳細
    ''' </summary>
    ''' <remarks></remarks>
    Private Class JobDetailClass
        Public Property DispatchNo As String
        Public Property JobSequenceNumber As Long
        Public Property JobID As String
        Public Property GroupID As String
        Public Property Status As String
        Public Property StartTime As Date
        Public Property EndTime As Date
        Public Property WorkTime As Long
        Public Property FMAccount As String
        Public Property InspectionFlg As String
        Public Property StopInformation As List(Of StopInfomationClass)
    End Class

    ''' <summary>
    ''' 中断情報
    ''' </summary>
    ''' <remarks></remarks>
    Private Class StopInfomationClass
        Public Property StopSequenceNumber As Long
        Public Property StopStart As Date
        Public Property StopEnd As Date
        Public Property StopReason As String
    End Class

    ''' <summary>
    ''' ストール情報
    ''' </summary>
    ''' <remarks></remarks>
    Private Class StallInfomationClass
        Public Property StallId As Decimal
        Public Property StartTime As Date
        Public Property EndTime As Date
        Public Property TechnicianId As IC3802701StaffJobDataTable
        Public Property RestInformation As IC3802701StallIdleRow()
    End Class

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
