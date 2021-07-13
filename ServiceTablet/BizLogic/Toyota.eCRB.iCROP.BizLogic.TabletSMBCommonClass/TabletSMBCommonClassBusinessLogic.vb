'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'TabletSMBCommonClassBusinessLogic.vb
'─────────────────────────────────────
'機能： タブレットSMB共通関数のビジネスロジック
'補足： 
'作成： 2013/06/05 TMEJ 張 タブレット版SMB機能開発(工程管理)
'更新： 2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発
'更新： 2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発
'更新： 2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発
'更新： 2014/01/17 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発
'更新： 2014/07/02 TMEJ 張 BTS-283 「サービスマネージャをTCとして表示」対応
'更新： 2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発
'更新： 2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発
'更新： 2014/09/11 TMEJ 張 BTS-389 「SMBで異常ではないはずが、遅れ見込み表示になっている」対応
'更新： 2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応
'更新： 2014/12/04 TMEJ 丁 新プラットフォーム版i-CROP仕様変更対応（サービス入庫実績更新処理変更）
'更新： 2014/12/17 TMEJ 小澤 IT9824_NextSTEPサービス 洗車用端末開発向けた評価用アプリ作成
'更新： 2015/02/09 TMEJ 範 DMS連携版サービスタブレット SMB納車予定時刻通知機能開発
'更新： 2015/02/13 TMEJ 範 新プラットフォーム版i-CROP仕様変更対応 (SMBチップ検索及び作業中断機能強化)
'更新： 2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発
'更新： 2015/07/17 TMEJ 明瀬 タブレットSMB性能改善 通知WebService化
'更新： 2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応
'更新： 2016/06/29 NSK 皆川 TR-SVT-TMT-20160512-001 SA1はチップを作成していないのに、通知を受け取った
'更新：2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする
'更新： 2017/09/06 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一
'更新： 2020/01/08 NSK 鈴木 TR-V4-BMS-20191001-001 (トライ店システム評価)コールセンター業務における洗車要否判断の効率化検証
'更新： 2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証
'更新： 2019/07/30 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証
'更新： 2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証
'更新：
'─────────────────────────────────────

Option Strict On
Option Explicit On

Imports System.Globalization
Imports System.Reflection
Imports System.IO
Imports System.Net
Imports System.Text
Imports System.Web
Imports System.Xml
Imports System.Xml.Serialization
Imports System.Text.RegularExpressions
Imports System.Web.Script.Serialization
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.CommonUtility.SMBCommonClass.Api.BizLogic
Imports Toyota.eCRB.CommonUtility.SMBCommonClass.Api.BizLogic.SMBCommonClassBusinessLogic
Imports Toyota.eCRB.Visit.Api.BizLogic
Imports Toyota.eCRB.Visit.Api.DataAccess.VisitUtilityDataSet
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.DataAccess.TabletSMBCommonClassDataSet
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.DataAccess.TabletSMBCommonClassDataSetTableAdapters
'2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
Imports Toyota.eCRB.Tool.Notify.Api.BizLogic
Imports Toyota.eCRB.Tool.Notify.Api.DataAccess
Imports Toyota.eCRB.Tool.Notify.Api.DataAccess.ConstCode
Imports Toyota.eCRB.DMSLinkage.JobDispatchResult.Api.BizLogic
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.DataAccess
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.BizLogic
Imports Toyota.eCRB.DMSLinkage.JobDispatchResult.Api.DataAccess.IC3802701DataSet
'2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
'2015/02/09 TMEJ 範 DMS連携版サービスタブレット SMB納車予定時刻通知機能開発 START
Imports Toyota.eCRB.CommonUtility.SMBCommonClass.Api.DataAccess
'2015/02/09 TMEJ 範 DMS連携版サービスタブレット SMB納車予定時刻通知機能開発 END

Public Class TabletSMBCommonClassBusinessLogic
    Inherits BaseBusinessComponent
    Implements IDisposable

#Region "プロパティ"
    Public Property NewStallIdleId As Decimal
#End Region

    '2015/07/02 TMEJ 明瀬 ITXXXX_タブレットSMB性能調査 ログ出力強化 START
    Private LogServiceCommonBiz As New ServiceCommonClassBusinessLogic(True)
    '2015/07/02 TMEJ 明瀬 ITXXXX_タブレットSMB性能調査 ログ出力強化 END

#Region "作業開始、終了日時取得"
    ''' <summary>
    ''' 作業開始日時取得
    ''' </summary>
    ''' <param name="stallId">ストールID</param>
    ''' <param name="workStartDateTime">作業開始日時</param>
    ''' <param name="stallStartTime">稼働開始日時</param>
    ''' <param name="stallEndTime">稼働終了日時</param>
    ''' <param name="restFlg">休憩取得フラグ</param>
    ''' <param name="isCompulsory">計算強制フラグ</param>
    ''' <returns>作業開始日時</returns>
    ''' <remarks></remarks>
    Public Function GetServiceStartDateTime(ByVal stallId As Decimal, _
                                          ByVal workStartDateTime As Date, _
                                          ByVal stallStartTime As Date, _
                                          ByVal stallEndTime As Date, _
                                          ByVal restFlg As String, _
                                          Optional ByVal isCompulsory As Boolean = False) As Date
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. stallId={1}, workStartDateTime={2}, stallStartTime={3}, stallEndTime={4}, restFlg={5}" _
        , MethodBase.GetCurrentMethod.Name, stallId, workStartDateTime, stallStartTime, stallEndTime, restFlg))

        '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
        '休憩を自動判定かつ計算が強制でない場合、引数の開始日時をそのまま返却する
        If IsRestAutoJudge() AndAlso Not isCompulsory Then
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E startTime={1}", MethodBase.GetCurrentMethod.Name, workStartDateTime))
            Return workStartDateTime
        End If
        '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END

        '休憩取得しない場合、チップ開始時間が変わらない
        If restFlg.Equals("0") Then
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E startTime={1}", MethodBase.GetCurrentMethod.Name, workStartDateTime))
            Return workStartDateTime
        End If
        Dim stallIdLst As New List(Of Decimal)
        stallIdLst.Add(stallId)

        LogServiceCommonBiz.OutputLog(2, "●■● 1.2.1 TABLETSMBCOMMONCLASS_014 START")

        '移動先のストールの非稼働情報全部取得する
        Dim dtIdleInfo As TabletSmbCommonClassStallIdleInfoDataTable = Me.GetAllIdleDateInfo(stallIdLst, stallStartTime, stallEndTime)

        LogServiceCommonBiz.OutputLog(2, "●■● 1.2.1 TABLETSMBCOMMONCLASS_014 END")

        'チップの開始時間
        Dim chipTime As TimeSpan = New TimeSpan(CType(workStartDateTime.Hour, Integer), CType(workStartDateTime.Minute, Integer), 0)
        Dim chipTimeCompare As TimeSpan = New TimeSpan(CType(workStartDateTime.Hour, Integer), CType(workStartDateTime.Minute, Integer), 0)
        '1日/5分=288
        Dim nMaxLoop As Long = 288
        While (True)

            '開始時間が休憩時間にあるかチェック
            For Each drIdleInfo As TabletSmbCommonClassStallIdleInfoRow In dtIdleInfo
                '休憩エリアの開始、終了時間
                Dim startTime As TimeSpan
                Dim endTime As TimeSpan
                If drIdleInfo.IDLE_TYPE.Equals("1") Then
                    startTime = New TimeSpan(CType(drIdleInfo.IDLE_START_TIME.Hour, Integer), CType(drIdleInfo.IDLE_START_TIME.Minute, Integer), 0)
                    endTime = New TimeSpan(CType(drIdleInfo.IDLE_END_TIME.Hour, Integer), CType(drIdleInfo.IDLE_END_TIME.Minute, Integer), 0)
                    '2019/07/30 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
                    'ElseIf drIdleInfo.IDLE_TYPE.Equals("2") Then
                    '    startTime = New TimeSpan(CType(drIdleInfo.IDLE_START_DATETIME.Hour, Integer), CType(drIdleInfo.IDLE_START_DATETIME.Minute, Integer), 0)
                    '    endTime = New TimeSpan(CType(drIdleInfo.IDLE_END_DATETIME.Hour, Integer), CType(drIdleInfo.IDLE_END_DATETIME.Minute, Integer), 0)
                    '2019/07/30 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END
                End If

                'チップが休憩エリアにある時、休憩エリアの終了時間に設定する
                If chipTime.CompareTo(startTime) >= 0 _
                    And chipTime.CompareTo(endTime) < 0 Then
                    chipTimeCompare = endTime
                    Exit For
                End If
            Next

            '変更してない場合、戻す
            If chipTime.CompareTo(chipTimeCompare) = 0 Then
                Exit While
            Else
                chipTime = chipTimeCompare
            End If

            '無限ループを防止する
            nMaxLoop = nMaxLoop - 1
            If nMaxLoop <= 0 Then
                Throw New InvalidOperationException("GetServiceStartDateTime while loop error.")
            End If

        End While
        '開始日時を戻す
        Dim rtDate As Date = New Date(workStartDateTime.Year, workStartDateTime.Month, workStartDateTime.Day, chipTime.Hours, chipTime.Minutes, 0)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E startTime={1}", MethodBase.GetCurrentMethod.Name, rtDate))
        Return rtDate
    End Function

    '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
    ' ''' <summary>
    ' ''' 作業開始日時と作業時間から作業終了日時を算出します
    ' ''' </summary>
    ' ''' <param name="stallId">ストールID</param>
    ' ''' <param name="workStartDateTime">作業開始日時</param>
    ' ''' <param name="workTime">作業時間</param>
    ' ''' <param name="stallStartTime">稼働開始日時</param>
    ' ''' <param name="stallEndTime">稼働終了日時</param>
    ' ''' <param name="restFlg">休憩取得フラグ</param>
    ' ''' <returns>作業終了日時</returns>
    ' ''' <remarks></remarks>
    'Public Function GetServiceEndDateTime(ByVal stallId As Decimal, _
    '                                      ByVal workStartDateTime As Date, _
    '                                      ByVal workTime As Long, _
    '                                      ByVal stallStartTime As Date, _
    '                                      ByVal stallEndTime As Date, _
    '                                      ByVal restFlg As String) As Date

    ''' <summary>
    ''' 作業開始日時と作業時間から作業終了日時を算出します
    ''' </summary>
    ''' <param name="stallId">ストールID</param>
    ''' <param name="workStartDateTime">作業開始日時</param>
    ''' <param name="workTime">作業時間</param>
    ''' <param name="stallStartTime">稼働開始日時</param>
    ''' <param name="stallEndTime">稼働終了日時</param>
    ''' <param name="restFlg">休憩取得フラグ</param>
    ''' <returns>作業終了日時情報</returns>
    ''' <remarks></remarks>
    Public Function GetServiceEndDateTime(ByVal stallId As Decimal, _
                                          ByVal workStartDateTime As Date, _
                                          ByVal workTime As Long, _
                                          ByVal stallStartTime As Date, _
                                          ByVal stallEndTime As Date, _
                                          ByVal restFlg As String) As ServiceEndDateTimeData
        '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. stallId={1}, workStartDateTime={2}, workTime={3}, stallStartTime={4}, stallEndTime={5}, restFlg={6}" _
                , MethodBase.GetCurrentMethod.Name, stallId, workStartDateTime, workTime, stallStartTime, stallEndTime, restFlg))

        If String.IsNullOrEmpty(restFlg) Then
            Throw New ArgumentNullException
        End If
        '秒を切り捨てる
        Dim workStartDateTimeNoSec = Me.GetDateTimeFloorSecond(workStartDateTime)

        'ローカル変数．跨ぎ作業時間 = 作業時間
        'ローカル変数．跨ぎ終了日時 = 作業開始日時

        Dim crossWorkTime As Long = workTime
        Dim crossEndDateTime As Date = workStartDateTimeNoSec

        Dim loopCount As Integer = 0
        '跨ぎ作業時間が0以外の場合、繰り返す
        While crossWorkTime <> 0
            'ローカル変数．跨ぎ開始日時 = ローカル変数．跨ぎ終了日時
            'ローカル変数．跨ぎ終了日時 = ローカル変数．跨ぎ開始日時 + ローカル変数．跨ぎ作業時間
            Dim crossStartDateTime As Date = crossEndDateTime
            crossEndDateTime = crossStartDateTime.AddMinutes(crossWorkTime)

            '非稼働跨ぎ日時リストを取得する
            Dim idleCrossList As TabletSmbCommonClassCrossDateDataTable = GetIdleDateCrossList(stallId, crossStartDateTime, crossEndDateTime, stallStartTime, stallEndTime)
            '跨ぎ日時リスト
            Dim crossList As TabletSmbCommonClassCrossDateDataTable
            crossList = CType(idleCrossList.Copy(), TabletSmbCommonClassCrossDateDataTable)

            For i = crossList.Rows.Count - 1 To 0 Step -1
                If crossList(i).IsSTART_CROSS_TIMENull Then
                    crossList.Rows.RemoveAt(i)
                End If
            Next

            '跨ぎ終了日時リスト
            Dim crossEndTimeList As List(Of Date) = New List(Of Date)
            If idleCrossList.Rows.Count > 0 Then
                If Not idleCrossList(0).IsCROSS_END_DATETIMENull Then
                    crossEndTimeList.Add(idleCrossList(0).CROSS_END_DATETIME)
                End If
            End If

            '入力データ．休憩取得フラグが「1：取得する（取得した）」の場合
            If restFlg.Equals("1") Then
                '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
                ''休憩・使用不可跨ぎ日時リストを取得する
                'Dim restUnavailableList As TabletSmbCommonClassCrossDateDataTable = GetRestAndUnavailableCrossList(stallId, crossStartDateTime, crossEndDateTime)
                ''非稼働跨ぎ日時リストと休憩・使用不可跨ぎ日時リストを１つのリストにする
                'crossList = CType(CombineTheSameDatatable(crossList, restUnavailableList), TabletSmbCommonClassCrossDateDataTable)
                'If restUnavailableList.Rows.Count > 0 Then
                '    If Not restUnavailableList(0).IsCROSS_END_DATETIMENull Then
                '        '跨ぎ終了日時リストに跨ぎ終了日時を追加する
                '        crossEndTimeList.Add(restUnavailableList(0).CROSS_END_DATETIME)
                '    End If
                'End If

                '休憩跨ぎ日時リストを取得する
                Dim restList As TabletSmbCommonClassCrossDateDataTable = GetRestCrossList(stallId, crossStartDateTime, crossEndDateTime)

                '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
                '休憩を自動判定する場合
                If IsRestAutoJudge() Then
                    '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END

                    '作業開始日時以前に開始されている休憩を削除する
                    For i = restList.Rows.Count - 1 To 0 Step -1
                        If restList(i).START_CROSS_TIME <= workStartDateTime Then
                            restList.Rows.RemoveAt(i)
                        End If
                    Next

                    '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
                End If
                '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END

                '非稼働跨ぎ日時リストと休憩跨ぎ日時リストを１つのリストにする
                crossList = CType(CombineTheSameDatatable(crossList, restList), TabletSmbCommonClassCrossDateDataTable)

                If 0 < restList.Rows.Count Then
                    If Not restList(0).IsCROSS_END_DATETIMENull Then
                        '跨ぎ終了日時リストに跨ぎ終了日時を追加する
                        crossEndTimeList.Add(restList(0).CROSS_END_DATETIME)
                    End If
                End If
                '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END
            End If
            '跨ぎ作業時間を取得する
            crossWorkTime = GetCrossServiceWorkTime(crossList)
            '跨ぎ日時リストから、最大の終了日時を取得し、跨ぎ終了日時に設定する
            crossEndDateTime = (From p In crossEndTimeList
                Select p).Max()

            loopCount = loopCount + 1
            '検索日数が非稼働情報検索日数の最大値を超えている場合
            '無限ループを回避するためにチェックする
            Dim ts As TimeSpan = crossEndDateTime.Subtract(workStartDateTimeNoSec)
            If ts.TotalDays > 100 Then
                Throw New InvalidOperationException("StallIdleData is Invalid Data.")
            End If
        End While


        '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START

        ''2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
        ''休憩跨ぎ日時リスト（営業時間外、非稼働日を除く）を再取得する
        'Dim AfterRestCrossList As TabletSmbCommonClassCrossDateDataTable =
        '    GetRestCrossListWithoutIdle(stallId, workStartDateTime, crossEndDateTime, stallStartTime, stallEndTime)

        ''休憩取得フラグ（自動判別）
        'Dim autoJudgeRestFlg = RestTimeGetFlgGetRest

        ''取得結果が存在する場合
        'If 1 <= AfterRestCrossList.Rows.Count Then

        '    Dim isStartBeforeRest As Boolean = False

        '    '作業開始日時 < 休憩開始日時 のデータが１件でもある場合
        '    For Each restCrossRow In AfterRestCrossList
        '        If workStartDateTime < restCrossRow.START_CROSS_TIME Then
        '            isStartBeforeRest = True
        '        End If
        '    Next

        '    If Not isStartBeforeRest Then
        '        autoJudgeRestFlg = RestTimeGetFlgNoGetRest
        '    Else
        '        autoJudgeRestFlg = restFlg
        '    End If

        'End If

        'Dim serviceEndDateTimeData As New ServiceEndDateTimeData
        'serviceEndDateTimeData.ServiceEndDateTime = crossEndDateTime
        'serviceEndDateTimeData.RestFlg = autoJudgeRestFlg
        ''2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END

        '休憩取得フラグ（戻り値）
        Dim rtnRestFlg = restFlg

        '休憩を自動判定する場合
        If IsRestAutoJudge() Then
            '休憩跨ぎ日時リスト（営業時間外、非稼働日を除く）を再取得する
            Dim AfterRestCrossList As TabletSmbCommonClassCrossDateDataTable =
                GetRestCrossListWithoutIdle(stallId, workStartDateTime, crossEndDateTime, stallStartTime, stallEndTime)

            '休憩取得フラグ（自動判別）
            rtnRestFlg = RestTimeGetFlgGetRest

            '取得結果が存在する場合
            If 1 <= AfterRestCrossList.Rows.Count Then

                Dim isStartBeforeRest As Boolean = False

                '作業開始日時 < 休憩開始日時 のデータが１件でもある場合
                For Each restCrossRow In AfterRestCrossList
                    If workStartDateTime < restCrossRow.START_CROSS_TIME Then
                        isStartBeforeRest = True
                    End If
                Next

                If Not isStartBeforeRest Then
                    rtnRestFlg = RestTimeGetFlgNoGetRest
                Else
                    rtnRestFlg = restFlg
                End If

            End If
        End If

        Dim serviceEndDateTimeData As New ServiceEndDateTimeData
        serviceEndDateTimeData.ServiceEndDateTime = crossEndDateTime
        serviceEndDateTimeData.RestFlg = rtnRestFlg

        '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END

        '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
        'Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E crossEndDateTime={1}", MethodBase.GetCurrentMethod.Name, crossEndDateTime))
        'Return crossEndDateTime
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E ServiceEndDateTime={1}, RestFlg={2}", MethodBase.GetCurrentMethod.Name, _
                                  serviceEndDateTimeData.ServiceEndDateTime, serviceEndDateTimeData.RestFlg))
        Return serviceEndDateTimeData
        '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END
    End Function

#End Region

#Region "実績終了日時取得"
    ''' <summary>
    ''' 実績終了日時を算出して取得します
    ''' </summary>
    ''' <param name="rsltStartDateTime">実績開始日時</param>
    ''' <param name="judgeDateTime">店舗処理日時</param>
    ''' <param name="stallStartTime">稼働開始日時</param>
    ''' <param name="stallEndTime">稼働終了日時</param>
    ''' <returns>実績終了日時</returns>
    ''' <remarks></remarks>
    Private Function GetRsltEndDateTime(ByVal rsltStartDateTime As Date, _
                                       ByVal judgeDateTime As Date, _
                                       ByVal stallStartTime As Date, _
                                       ByVal stallEndTime As Date) As Date

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. rsltStartDateTime={1}, judgeDateTime={2}, stallStartTime={3}, stallEndTime={4}" _
            , MethodBase.GetCurrentMethod.Name, rsltStartDateTime, judgeDateTime, stallStartTime, stallEndTime))

        '営業開始時間・営業終了時間を取得する
        Dim workDate As List(Of Date) = GetStallDispDate(rsltStartDateTime, stallStartTime, stallEndTime)
        Dim endTime As Date = workDate(1)
        '判定日時 > 営業終了日時 の場合
        If endTime.CompareTo(judgeDateTime) < 0 Then
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E endDateTime={1}", MethodBase.GetCurrentMethod.Name, endTime))
            Return endTime
        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E endDateTime={1}", MethodBase.GetCurrentMethod.Name, judgeDateTime))
        Return judgeDateTime
    End Function
#End Region

#Region "作業時間取得"
    ''' <summary>
    ''' 作業開始日時と作業終了日時から作業時間を算出します
    ''' </summary>
    ''' <param name="stallId">ストールID</param>
    ''' <param name="workStartDateTime">作業開始日時</param>
    ''' <param name="workEndDateTime">作業終了日時</param>
    ''' <param name="restFlg">休憩取得フラグ</param>
    ''' <param name="stallStartTime">営業開始時間</param>
    ''' <param name="stallEndTime">営業終了時間</param>
    ''' <returns>作業時間</returns>
    ''' <remarks></remarks>
    Public Function GetServiceWorkTime(ByVal stallId As Decimal, _
                                       ByVal workStartDateTime As Date, _
                                       ByVal workEndDateTime As Date, _
                                       ByVal restFlg As String, _
                                       ByVal stallStartTime As Date, _
                                       ByVal stallEndTime As Date) As Long

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. stallId={1}, workStartDateTime={2}, workEndDateTime={3}, restFlg={4}, stallStartTime={5}, stallEndTime={6}" _
            , MethodBase.GetCurrentMethod.Name, stallId, workStartDateTime, workEndDateTime, restFlg, stallStartTime, stallEndTime))

        '秒を切り捨てる
        Dim workStartDateTimeNoSec As Date = GetDateTimeFloorSecond(workStartDateTime)
        Dim workEndDateTimeNoSec As Date = GetDateTimeFloorSecond(workEndDateTime)

        '非稼働跨ぎ情報を取得する
        Dim idleCrossData As TabletSmbCommonClassCrossDateDataTable = GetIdleDateCrossList(stallId, workStartDateTime, workEndDateTime, stallStartTime, stallEndTime)
        Dim crossDateList As TabletSmbCommonClassCrossDateDataTable = idleCrossData

        '入力データ．休憩取得フラグが「1：取得する（取得した）」の場合
        If restFlg = RestTimeGetFlgGetRest Then
            '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
            ''休憩・使用不可跨ぎ情報を取得する
            'Dim restUnavailableCrossData As TabletSmbCommonClassCrossDateDataTable = GetRestAndUnavailableCrossList(stallId, workStartDateTimeNoSec, workEndDateTimeNoSec)
            ''非稼働跨ぎ日時リストと休憩・使用不可跨ぎ日時リストを１つのリストにする
            'crossDateList = CType(CombineTheSameDatatable(crossDateList, idleCrossData), TabletSmbCommonClassCrossDateDataTable)
            'crossDateList = CType(CombineTheSameDatatable(crossDateList, restUnavailableCrossData), TabletSmbCommonClassCrossDateDataTable)

            '休憩跨ぎ情報を取得する
            Dim restCrossData As TabletSmbCommonClassCrossDateDataTable = GetRestCrossList(stallId, workStartDateTimeNoSec, workEndDateTimeNoSec)

            '休憩を自動判定する場合
            If IsRestAutoJudge() Then
                '作業開始日時以前に開始されている休憩を削除する
                For i = restCrossData.Rows.Count - 1 To 0 Step -1
                    If restCrossData(i).START_CROSS_TIME <= workStartDateTime Then
                        restCrossData.Rows.RemoveAt(i)
                    End If
                Next
            End If

            '非稼働跨ぎ日時リストと休憩跨ぎ日時リストを１つのリストにする
            crossDateList = CType(CombineTheSameDatatable(crossDateList, idleCrossData), TabletSmbCommonClassCrossDateDataTable)
            crossDateList = CType(CombineTheSameDatatable(crossDateList, restCrossData), TabletSmbCommonClassCrossDateDataTable)
            '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END
        End If

        '跨ぎ作業時間を取得する
        Dim crossWorkTime As Long = GetCrossServiceWorkTime(crossDateList)
        ' 作業時間を算出し、返却する。
        '（作業時間 = 入力データ．作業終了日時 - 入力データ．作業開始日時 - ローカル変数．跨ぎ作業時間）
        Dim ts As TimeSpan = workEndDateTimeNoSec.Subtract(workStartDateTimeNoSec)

        Dim workTime As Long = CType(ts.TotalMinutes - crossWorkTime, Long)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E workTime={1}", MethodBase.GetCurrentMethod.Name, workTime))
        Return workTime
    End Function
#End Region

#Region "跨ぎ作業時間取得"
    ''' <summary>
    ''' 冗長な時間を除いた跨ぎ作業時間を算出します
    ''' </summary>
    ''' <param name="crossList">跨ぎ日時リスト</param>
    ''' <returns>跨ぎ作業時間</returns>
    ''' <remarks></remarks>
    Private Function GetCrossServiceWorkTime(ByVal crossList As TabletSmbCommonClassCrossDateDataTable) As Long
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S." _
            , MethodBase.GetCurrentMethod.Name))

        Using localCrossList As New TabletSmbCommonClassCrossDateDataTable
            'START_CROSS_TIMEの値があるdatarowを絞り込む
            For Each drCrossRow As TabletSmbCommonClassCrossDateRow In crossList
                If Not drCrossRow.IsSTART_CROSS_TIMENull Then
                    Dim drLocalCrossList As TabletSmbCommonClassCrossDateRow = CType(localCrossList.NewRow, TabletSmbCommonClassCrossDateRow)
                    drLocalCrossList.START_CROSS_TIME = drCrossRow.START_CROSS_TIME
                    drLocalCrossList.END_CROSS_TIME = drCrossRow.END_CROSS_TIME
                    If Not drCrossRow.IsCROSS_END_DATETIMENull Then
                        drLocalCrossList.CROSS_END_DATETIME = drCrossRow.CROSS_END_DATETIME
                    End If
                    localCrossList.AddTabletSmbCommonClassCrossDateRow(drLocalCrossList)
                End If
            Next

            '跨ぎ日時リストの件数が0件の場合
            If localCrossList.Rows.Count = 0 Then
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E total=0", MethodBase.GetCurrentMethod.Name))
                '跨ぎ作業時間として、0を返却する
                Return 0
            End If

            '跨ぎ日時リストをソートする
            Dim crossSortedList = (From p In localCrossList _
                                   Order By p.START_CROSS_TIME, p.END_CROSS_TIME _
                                   Select p)
            Dim list As List(Of TabletSmbCommonClassCrossDateRow) = crossSortedList.ToList()
            '跨ぎ日時リスト（冗長分なし）
            'ローカル変数．最早開始日時 = 跨ぎ日時リストの先頭の開始日時
            'ローカル変数．最遅開始日時 = 跨ぎ日時リストの先頭の終了日時
            Dim crossNonOverlapList As List(Of Long) = New List(Of Long)
            Dim minDateTime As Date = list(0).START_CROSS_TIME
            Dim maxDateTime As Date = list(0).END_CROSS_TIME
            '跨ぎ日時リストが2件以上の場合
            If 2 <= list.Count Then
                '跨ぎ日時リストの2件目から末尾まで、跨ぎ作業時間の冗長分の削除を繰り返す
                For i As Integer = 0 To list.Count - 2
                    If maxDateTime.CompareTo(list(i + 1).START_CROSS_TIME) < 0 Then
                        '作業時間リストに、最終終了日時 - 最早開始日時 を追加する
                        Dim ts1 As TimeSpan = maxDateTime.Subtract(minDateTime)
                        crossNonOverlapList.Add(ts1.Hours * 60 + ts1.Minutes)
                        'ローカル変数．最早開始日時 = 跨ぎ日時リストの開始日時
                        'ローカル変数．最遅開始日時 = 跨ぎ日時リストの終了日時
                        minDateTime = list(i + 1).START_CROSS_TIME
                        maxDateTime = list(i + 1).END_CROSS_TIME

                        Continue For
                    End If

                    '最遅終了日時 < 跨ぎ日時リスト．終了日時 の場合
                    'ローカル変数．最終終了日時 = 跨ぎ日時リスト．終了日時
                    If maxDateTime.CompareTo(list(i + 1).END_CROSS_TIME) < 0 Then
                        maxDateTime = list(i + 1).END_CROSS_TIME
                    End If
                Next
            End If

            '作業時間リストにローカル変数．最遅終了日時 - ローカル変数．最早開始日時を追加する。
            Dim ts2 = maxDateTime.Subtract(minDateTime)
            crossNonOverlapList.Add(ts2.Hours * 60 + ts2.Minutes)

            Dim total As Long = 0
            '跨ぎ作業時間の合計値を算出する
            For Each val As Long In crossNonOverlapList
                total = total + val
            Next
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E total={1}", MethodBase.GetCurrentMethod.Name, total))
            '跨ぎ作業時間の合計値を返却する
            Return total
        End Using
    End Function
#End Region

#Region "非稼働跨ぎ日時リスト取得"
    ''' <summary>
    ''' 非稼働跨ぎ日時リストを取得します
    ''' </summary>
    ''' <param name="stallId">ストールID</param>
    ''' <param name="workStartDateTime">作業開始日時</param>
    ''' <param name="workEndDateTime">作業終了日時</param>
    ''' <param name="stallStartTime">稼働開始日時</param>
    ''' <param name="stallEndTime">稼働終了日時</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2015/04/08 TMEJ 明瀬 TMT２販社号口後フォロー BTS-318
    ''' </history>
    Private Function GetIdleDateCrossList(ByVal stallId As Decimal, _
                                          ByVal workStartDateTime As Date, _
                                          ByVal workEndDateTime As Date, _
                                          ByVal stallStartTime As Date, _
                                          ByVal stallEndTime As Date) As TabletSmbCommonClassCrossDateDataTable
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. stallId={1}, workStartDateTime={2}, workEndDateTime={3}" _
            , MethodBase.GetCurrentMethod.Name, stallId, workStartDateTime, workEndDateTime))

        'ローカル変数．跨ぎ終了日時 = 入力データ．作業開始日時
        'ローカル変数．跨ぎ情報を初期化する
        Dim crossEndDateTime As Date = workStartDateTime
        Using crossData As New TabletSmbCommonClassCrossDateDataTable
            Dim nLoop As Long = 0

            LogServiceCommonBiz.OutputLog(4, "●■● 1.3.1 TABLETSMBCOMMONCLASS_011(ループ有り) START")

            'ローカル変数．跨ぎ終了日時 < 入力データ．作業終了日時の場合、繰り返す
            While crossEndDateTime < workEndDateTime
                nLoop = nLoop + 1
                If nLoop >= 100 Then
                    Exit While
                End If

                'ローカル変数．跨ぎ開始日時 = ローカル変数．跨ぎ終了日時
                Dim crossStartDateTime As Date = crossEndDateTime

                '営業時間外判定を行い、「true：営業時間外」の場合
                If IsOutOfWorkingTime(crossStartDateTime, stallStartTime, stallEndTime) Then
                    'ローカル変数．翌営業日
                    'ローカル変数．跨ぎ開始日時（ローカル時間）
                    '営業開始時間
                    Dim nextWorkingDate As Date
                    Dim localCrossStartDateTime As Date = crossStartDateTime
                    Dim workingStartTimeSpan As TimeSpan = New TimeSpan(stallStartTime.Hour, stallStartTime.Minute, 0)

                    'ローカル変数．跨ぎ開始日時（ローカル時間）< ローカル変数．営業開始時間 の場合
                    If localCrossStartDateTime.TimeOfDay.CompareTo(workingStartTimeSpan) < 0 Then
                        'ローカル変数．翌営業日 = ローカル変数．跨ぎ開始日時（ローカル時間）の日付
                        nextWorkingDate = localCrossStartDateTime.Date
                    Else
                        'ローカル変数．翌営業日 =ローカル変数． 跨ぎ開始日時（ローカル時間）の翌日の日付
                        nextWorkingDate = localCrossStartDateTime.AddDays(1).Date
                    End If
                    '跨ぎ終了日時 = 翌営業日 + 営業開始時間 
                    crossEndDateTime = nextWorkingDate.Add(workingStartTimeSpan)
                Else
                    Using ta As New TabletSMBCommonClassDataSetTableAdapters.TabletSMBCommonClassDataAdapter
                        '営業開始日時と営業終了日時を取得する
                        Dim workDate As List(Of Date) = GetStallDispDate(crossStartDateTime, stallStartTime, stallEndTime)

                        Dim startDateTime As Date = workDate(0)
                        Dim endDateTime As Date = workDate(1)

                        '非稼働日判定を行い、「true：非稼働日」の場合
                        '2019/07/30 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
                        'If ta.IsStallIdleDay(startDateTime, startDateTime, stallId) Then
                        If ta.IsStallIdleDay(startDateTime.Date, startDateTime.Date, stallId) Then
                            '2019/07/30 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END

                            crossEndDateTime = startDateTime.AddDays(1)

                        Else
                            '入力データ．作業終了日時 <= ローカル変数．営業終了日時 の場合
                            'ローカル変数．跨ぎ終了日時 = 入力データ．作業終了日時

                            '2015/04/08 TMEJ 明瀬 TMT２販社号口後フォロー BTS-318 START

                            'If workEndDateTime.CompareTo(endDateTime) < 0 Then
                            '    crossEndDateTime = workEndDateTime

                            '    Dim drCrossDateTimeData As TabletSmbCommonClassCrossDateRow = CType(crossData.NewRow, TabletSmbCommonClassCrossDateRow)
                            '    drCrossDateTimeData.CROSS_END_DATETIME = crossEndDateTime
                            '    crossData.Rows.Add(drCrossDateTimeData)
                            '    '繰り返しを終了する
                            '    Exit While
                            'End If

                            If workEndDateTime.CompareTo(endDateTime) <= 0 Then
                                crossEndDateTime = workEndDateTime

                                Dim drCrossDateTimeData As TabletSmbCommonClassCrossDateRow = CType(crossData.NewRow, TabletSmbCommonClassCrossDateRow)
                                drCrossDateTimeData.CROSS_END_DATETIME = crossEndDateTime
                                crossData.Rows.Add(drCrossDateTimeData)
                                '繰り返しを終了する
                                Exit While
                            End If

                            '2015/04/08 TMEJ 明瀬 TMT２販社号口後フォロー BTS-318 END

                            'ローカル変数．跨ぎ開始日時 = ローカル変数．営業終了日時
                            'ローカル変数．跨ぎ終了日時 = ローカル変数．営業開始日時の翌日
                            crossStartDateTime = endDateTime
                            crossEndDateTime = startDateTime.AddDays(1)
                        End If
                    End Using
                End If
                '開始日時 = ローカル変数．跨ぎ開始日時
                Dim crossDateTimeData As TabletSmbCommonClassCrossDateRow = CType(crossData.NewRow, TabletSmbCommonClassCrossDateRow)
                crossDateTimeData.START_CROSS_TIME = crossStartDateTime
                '入力データ．作業終了日時 < ローカル変数．跨ぎ終了日時 の場合
                If workEndDateTime.CompareTo(crossEndDateTime) < 0 Then
                    '終了日時 = 入力データ．作業終了日時
                    crossDateTimeData.END_CROSS_TIME = workEndDateTime
                Else
                    '終了日時 = ローカル変数．跨ぎ終了日時
                    crossDateTimeData.END_CROSS_TIME = crossEndDateTime
                End If
                'ローカル変数．跨ぎ情報．跨ぎ日時リストに開始日時・終了日時を追加する
                crossData.Rows.Add(crossDateTimeData)
            End While

            LogServiceCommonBiz.OutputLog(4, "●■● 1.3.1 TABLETSMBCOMMONCLASS_011(ループ有り)[ループ数：" & nLoop & "] END")

            For Each crossDateTimeDataRow As TabletSmbCommonClassCrossDateRow In crossData
                'ローカル変数．跨ぎ情報．跨ぎ終了日時 = ローカル変数．跨ぎ終了日時
                crossDateTimeDataRow.CROSS_END_DATETIME = crossEndDateTime
            Next

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
            '作成したローカル変数．跨ぎ情報を返却する
            Return crossData
        End Using
    End Function
#End Region

#Region "休憩・使用不可跨ぎ日時リスト取得"
    '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
    ' ''' <summary>
    ' ''' 休憩・使用不可跨ぎ日時リスト取得
    ' ''' </summary>
    ' ''' <param name="stallId">ストールID</param>
    ' ''' <param name="idleStartDateTime">開始時間</param>
    ' ''' <param name="idleEndDateTime">終了時間</param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Private Function GetRestAndUnavailableCrossList(ByVal stallId As Decimal, _
    '                                      ByVal idleStartDateTime As Date, _
    '                                      ByVal idleEndDateTime As Date) As TabletSmbCommonClassCrossDateDataTable

    '    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. stallId={1}, idleStartDateTime={2}, idleEndDateTime={3}" _
    '        , MethodBase.GetCurrentMethod.Name, stallId, idleStartDateTime, idleEndDateTime))

    '    'ローカル変数．跨ぎ終了日時 = 入力データ．非稼働終了日時
    '    Dim crossEndDateTime As Date = idleEndDateTime

    '    '入力データ．ストールIDをリスト形式にする。
    '    Dim stallIdList As List(Of Decimal) = New List(Of Decimal)
    '    stallIdList.Add(stallId)


    '    'ストール非稼動マスタ．設定単位区分が「1：時間（毎日何時）」の休憩時間情報を取得して設定する。
    '    'ストール非稼動マスタ．設定単位区分が「2：日時（年月日時分）」の使用不可情報を取得して設定する。

    '    LogServiceCommonBiz.OutputLog(5, "●■● 1.3.2 休憩時間情報取得 START")

    '    Dim restInfo As TabletSmbCommonClassIdleTimeInfoDataTable = GetRestTimeInfo(stallId, idleStartDateTime, idleEndDateTime)

    '    LogServiceCommonBiz.OutputLog(5, "●■● 1.3.2 休憩時間情報取得 END")

    '    LogServiceCommonBiz.OutputLog(7, "●■● 1.3.3 TABLETSMBCOMMONCLASS_013 START")

    '    Dim unavailableInfo As TabletSmbCommonClassIdleTimeInfoDataTable = GetStallUnavailableInfo(stallId, idleStartDateTime, idleEndDateTime)

    '    LogServiceCommonBiz.OutputLog(7, "●■● 1.3.3 TABLETSMBCOMMONCLASS_013 END")

    '    Dim idleInfo As TabletSmbCommonClassIdleTimeInfoDataTable
    '    '取得した2つのストール非稼働マスタ情報を1つにする。
    '    idleInfo = CType(CombineTheSameDatatable(restInfo, unavailableInfo), TabletSmbCommonClassIdleTimeInfoDataTable)

    '    'ローカル変数．跨ぎ情報を作成する。
    '    Using crossData As New TabletSmbCommonClassCrossDateDataTable

    '        '取得したストール非稼働マスタ分処理を繰り返す。
    '        For Each stallIdle In idleInfo
    '            Dim crossDateTimeData As TabletSmbCommonClassCrossDateRow = CType(crossData.NewRow, TabletSmbCommonClassCrossDateRow)
    '            'ストール非稼働マスタ．非稼働開始日時 < 入力データ．非稼働開始日時 の場合
    '            If stallIdle.IDLE_START_TIME.CompareTo(idleStartDateTime) < 0 Then
    '                '開始日時 = 入力データ．非稼働開始日時
    '                crossDateTimeData.START_CROSS_TIME = idleStartDateTime
    '            Else
    '                '開始日時 = ストール非稼働マスタ．非稼働開始日時
    '                crossDateTimeData.START_CROSS_TIME = stallIdle.IDLE_START_TIME
    '            End If
    '            '入力データ．非稼働終了日時 < ストール非稼働マスタ．非稼働終了日時 の場合
    '            If idleEndDateTime.CompareTo(stallIdle.IDLE_END_TIME) < 0 Then
    '                '終了日時 =  入力データ．非稼働終了日時
    '                crossDateTimeData.END_CROSS_TIME = idleEndDateTime
    '            Else
    '                '終了日時 = ストール非稼働マスタ．非稼働終了日時
    '                crossDateTimeData.END_CROSS_TIME = stallIdle.IDLE_END_TIME
    '            End If

    '            'ローカル変数．跨ぎ終了日時 < ストール非稼働マスタ．非稼動終了日時 の場合
    '            'ローカル変数．跨ぎ情報．跨ぎ終了日時 = ストール非稼働マスタ．非稼動終了日時
    '            If crossEndDateTime.CompareTo(stallIdle.IDLE_END_TIME) < 0 Then
    '                crossDateTimeData.CROSS_END_DATETIME = stallIdle.IDLE_END_TIME
    '            End If

    '            'ローカル変数．跨ぎ情報．跨ぎ日時リストに開始日時・終了日時を追加する
    '            crossData.Rows.Add(crossDateTimeData)
    '        Next
    '        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
    '        '作成したローカル変数．跨ぎ情報を返却する。
    '        Return crossData
    '    End Using
    'End Function

#Region "休憩跨ぎ日時リスト取得"
    ''' <summary>
    ''' 休憩跨ぎ日時リスト取得
    ''' </summary>
    ''' <param name="stallId">ストールID</param>
    ''' <param name="idleStartDateTime">開始時間</param>
    ''' <param name="idleEndDateTime">終了時間</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetRestCrossList(ByVal stallId As Decimal, _
                                          ByVal idleStartDateTime As Date, _
                                          ByVal idleEndDateTime As Date) As TabletSmbCommonClassCrossDateDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. stallId={1}, idleStartDateTime={2}, idleEndDateTime={3}" _
            , MethodBase.GetCurrentMethod.Name, stallId, idleStartDateTime, idleEndDateTime))

        'ローカル変数．跨ぎ終了日時 = 入力データ．非稼働終了日時
        Dim crossEndDateTime As Date = idleEndDateTime

        '入力データ．ストールIDをリスト形式にする。
        Dim stallIdList As List(Of Decimal) = New List(Of Decimal)
        stallIdList.Add(stallId)


        'ストール非稼動マスタ．設定単位区分が「1：時間（毎日何時）」の休憩時間情報を取得して設定する。

        LogServiceCommonBiz.OutputLog(5, "●■● 1.3.2 休憩時間情報取得 START")

        Dim restInfo As TabletSmbCommonClassIdleTimeInfoDataTable = GetRestTimeInfo(stallId, idleStartDateTime, idleEndDateTime)

        LogServiceCommonBiz.OutputLog(5, "●■● 1.3.2 休憩時間情報取得 END")

        'ローカル変数．跨ぎ情報を作成する。
        Using crossData As New TabletSmbCommonClassCrossDateDataTable

            '取得したストール非稼働マスタ分処理を繰り返す。
            For Each stallIdle In restInfo
                Dim crossDateTimeData As TabletSmbCommonClassCrossDateRow = CType(crossData.NewRow, TabletSmbCommonClassCrossDateRow)
                'ストール非稼働マスタ．非稼働開始日時 < 入力データ．非稼働開始日時 の場合
                If stallIdle.IDLE_START_TIME.CompareTo(idleStartDateTime) < 0 Then
                    '開始日時 = 入力データ．非稼働開始日時
                    crossDateTimeData.START_CROSS_TIME = idleStartDateTime
                Else
                    '開始日時 = ストール非稼働マスタ．非稼働開始日時
                    crossDateTimeData.START_CROSS_TIME = stallIdle.IDLE_START_TIME
                End If
                '入力データ．非稼働終了日時 < ストール非稼働マスタ．非稼働終了日時 の場合
                If idleEndDateTime.CompareTo(stallIdle.IDLE_END_TIME) < 0 Then
                    '終了日時 =  入力データ．非稼働終了日時
                    crossDateTimeData.END_CROSS_TIME = idleEndDateTime
                Else
                    '終了日時 = ストール非稼働マスタ．非稼働終了日時
                    crossDateTimeData.END_CROSS_TIME = stallIdle.IDLE_END_TIME
                End If

                'ローカル変数．跨ぎ終了日時 < ストール非稼働マスタ．非稼動終了日時 の場合
                'ローカル変数．跨ぎ情報．跨ぎ終了日時 = ストール非稼働マスタ．非稼動終了日時
                If crossEndDateTime.CompareTo(stallIdle.IDLE_END_TIME) < 0 Then
                    crossDateTimeData.CROSS_END_DATETIME = stallIdle.IDLE_END_TIME
                End If

                'ローカル変数．跨ぎ情報．跨ぎ日時リストに開始日時・終了日時を追加する
                crossData.Rows.Add(crossDateTimeData)
            Next
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
            '作成したローカル変数．跨ぎ情報を返却する。
            Return crossData
        End Using
    End Function
#End Region

#Region "休憩跨ぎ日時リスト（営業時間外、非稼働日を除く）取得"
    ''' <summary>
    ''' 休憩跨ぎ日時リスト（営業時間外、非稼働日を除く）取得
    ''' </summary>
    ''' <param name="stallId">ストールID</param>
    ''' <param name="startDateTime">開始時間</param>
    ''' <param name="endDateTime">終了時間</param>
    ''' <param name="stallStartTime">稼働開始日時</param>
    ''' <param name="stallEndTime">稼働終了日時</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetRestCrossListWithoutIdle(ByVal stallId As Decimal, _
                                          ByVal startDateTime As Date, _
                                          ByVal endDateTime As Date, _
                                          ByVal stallStartTime As Date, _
                                          ByVal stallEndTime As Date) As TabletSmbCommonClassCrossDateDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. stallId={1}, startDateTime={2}, endDateTime={3}, stallStartTime={4}, stallEndTime={5}" _
            , MethodBase.GetCurrentMethod.Name, stallId, startDateTime, endDateTime, stallStartTime, stallEndTime))

        '休憩跨ぎ情報を取得する
        Dim restCrossList As TabletSmbCommonClassCrossDateDataTable =
            GetRestCrossList(stallId, startDateTime, endDateTime)

        '非稼働跨ぎ情報を取得する
        Dim idleCrossList As TabletSmbCommonClassCrossDateDataTable =
            GetIdleDateCrossList(stallId, startDateTime, endDateTime, stallStartTime, stallEndTime)

        '開始時間～終了時間にストール非稼働が無い場合
        '跨ぎ終了日時(CROSS_END_TIME)のみ設定した行が返されるため削除する
        For i = idleCrossList.Rows.Count - 1 To 0 Step -1
            If idleCrossList(i).IsSTART_CROSS_TIMENull Or idleCrossList(i).IsEND_CROSS_TIMENull Then
                idleCrossList.Rows.RemoveAt(i)
            End If
        Next

        '非稼働跨ぎ情報が2件以上の場合
        If 2 <= idleCrossList.Count Then

            '非稼働跨ぎ情報をマージする（営業時間外と非稼働日が別のリストになっているため）
            Dim mergeIdleCrossList As New TabletSmbCommonClassCrossDateDataTable

            Dim beforeCrossDateTime As TabletSmbCommonClassCrossDateRow = idleCrossList(0)

            For Each idleCrossDateTime As TabletSmbCommonClassCrossDateRow In idleCrossList

                '2019/07/30 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
                'If beforeCrossDateTime.END_CROSS_TIME = idleCrossDateTime.START_CROSS_TIME Then
                If beforeCrossDateTime.END_CROSS_TIME = idleCrossDateTime.START_CROSS_TIME AndAlso _
                    mergeIdleCrossList.Rows.Count <> 0 Then
                    '2019/07/30 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END

                    'マージできる場合
                    '2019/07/30 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
                    'beforeCrossDateTime.END_CROSS_TIME = idleCrossDateTime.END_CROSS_TIME

                    '一番最後のリストの要素の非稼働日時を更新する
                    mergeIdleCrossList(mergeIdleCrossList.Rows.Count - 1).END_CROSS_TIME = idleCrossDateTime.END_CROSS_TIME
                    '2019/07/30 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END
                Else
                    'マージできない場合
                    mergeIdleCrossList.ImportRow(idleCrossDateTime)
                End If

                beforeCrossDateTime = idleCrossDateTime

            Next

            idleCrossList = mergeIdleCrossList

        End If

        '非稼働跨ぎ情報の分だけ繰り返す
        For Each idleCrossDateTime As TabletSmbCommonClassCrossDateRow In idleCrossList

            '休憩跨ぎ情報から、非稼働跨ぎ情報に含まれている休憩跨ぎ情報を削除する
            For i = restCrossList.Rows.Count - 1 To 0 Step -1
                If idleCrossDateTime.START_CROSS_TIME <= restCrossList(i).START_CROSS_TIME AndAlso _
                    restCrossList(i).END_CROSS_TIME <= idleCrossDateTime.END_CROSS_TIME Then

                    restCrossList.Rows.RemoveAt(i)
                End If
            Next
        Next

        Return restCrossList
    End Function
#End Region

#Region "休憩変更可能な位置かどうか判定する"
    ''' <summary>
    ''' 休憩変更可能な位置かどうか判定する
    ''' </summary>
    ''' <param name="stallUseId">ストール利用ID</param>
    ''' <param name="stallStartTime">稼働開始日時</param>
    ''' <param name="stallEndTime">稼働終了日時</param>
    ''' <returns>True:休憩変更可能 False:休憩変更不可能</returns>
    ''' <remarks></remarks>
    Public Function CanRestChange(ByVal stallUseId As Decimal, ByVal stallStartTime As Date, ByVal stallEndTime As Date) As Boolean
        '対象の情報を取得する
        Dim targetStallUse = GetChipEntity(stallUseId, 0)

        Dim startDateTime = targetStallUse(0).SCHE_START_DATETIME
        Dim endDateTime = targetStallUse(0).SCHE_END_DATETIME

        'ストール利用ステータスが「作業中」「作業計画の一部の作業が中断」の場合
        If targetStallUse(0).STALL_USE_STATUS = "02" Or targetStallUse(0).STALL_USE_STATUS = "04" Then
            startDateTime = targetStallUse(0).RSLT_START_DATETIME
            endDateTime = targetStallUse(0).PRMS_END_DATETIME
        End If

        '休憩跨ぎ日時リスト（営業時間外、非稼働日を除く）を再取得する
        Dim restCrossList As TabletSmbCommonClassCrossDateDataTable = GetRestCrossListWithoutIdle( _
                                        targetStallUse(0).STALL_ID, startDateTime, endDateTime, stallStartTime, stallEndTime)

        Dim canRestChangeFlg As Boolean = False

        '取得結果が存在する場合
        If 1 <= restCrossList.Rows.Count() Then
            For Each restCrossRow In restCrossList
                '作業開始日時 < 休憩開始日時 の休憩が存在する場合
                If startDateTime < restCrossRow.START_CROSS_TIME Then
                    canRestChangeFlg = True
                End If
            Next
        End If

        Return canRestChangeFlg

    End Function
#End Region

    '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END

#Region "ストール使用不可情報取得"
    ''' <summary>
    ''' ストール使用不可情報を取得します
    ''' </summary>
    ''' <param name="stallId">表示対象ストールID</param>
    ''' <param name="idleStartDateTime">非稼働開始日時</param>
    ''' <param name="idleEndDateTime">非稼働終了日時</param>
    ''' <returns>ストール非稼働マスタ情報リスト</returns>
    ''' <remarks></remarks>
    Private Function GetStallUnavailableInfo(ByVal stallId As Decimal, _
                                  ByVal idleStartDateTime As Date, _
                                  ByVal idleEndDateTime As Date) As TabletSmbCommonClassIdleTimeInfoDataTable
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. stallId={1}, idleStartDateTime={2}, idleEndDateTime={3}" _
                , MethodBase.GetCurrentMethod.Name, stallId, idleStartDateTime, idleEndDateTime))

        Using ta As New TabletSMBCommonClassDataSetTableAdapters.TabletSMBCommonClassDataAdapter
            '休憩時間情報を取得します
            Dim unavailableTable As TabletSmbCommonClassIdleTimeInfoDataTable = ta.GetStallUnavailableInfo(idleStartDateTime, idleEndDateTime, stallId)
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
            Return unavailableTable
        End Using
    End Function
#End Region

#Region "休憩時間情報取得"
    ''' <summary>
    ''' 休憩時間情報を取得します
    ''' </summary>
    ''' <param name="stallId">表示対象ストールID</param>
    ''' <param name="idleStartDateTime">非稼働開始日時</param>
    ''' <param name="idleEndDateTime">非稼働終了日時</param>
    ''' <returns>ストール非稼働リスト</returns>
    ''' <remarks></remarks>
    Private Function GetRestTimeInfo(ByVal stallId As Decimal, _
                                  ByVal idleStartDateTime As Date, _
                                  ByVal idleEndDateTime As Date) As TabletSmbCommonClassIdleTimeInfoDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. stallId={1}, idleStartDateTime={2}, idleEndDateTime={3}" _
               , MethodBase.GetCurrentMethod.Name, stallId, idleStartDateTime, idleEndDateTime))

        Dim restIdleTimeTable As TabletSmbCommonClassIdleTimeInfoDataTable

        LogServiceCommonBiz.OutputLog(6, "●■● 1.3.2.1 TABLETSMBCOMMONCLASS_012 START")

        Using ta As New TabletSMBCommonClassDataSetTableAdapters.TabletSMBCommonClassDataAdapter
            '休憩時間情報を取得します
            restIdleTimeTable = ta.GetRestTimeInfo(stallId)
        End Using

        LogServiceCommonBiz.OutputLog(6, "●■● 1.3.2.1 TABLETSMBCOMMONCLASS_012 END")

        'ローカル変数．処理開始日付 = 入力データ．非稼働開始日時（ローカル）の日付
        'ローカル変数．処理終了日付 = 入力データ．非稼働終了日時（ローカル）の日付
        'ローカル変数．処理日付 = ローカル変数．処理開始日付
        Dim procStartDate As Date = idleStartDateTime.Date
        Dim procEndDate As Date = idleEndDateTime.Date
        Dim procDate As Date = procStartDate

        '当ページ日付を追加する(休憩エリアがただ時間があって、日付がない)
        Dim restIdleTimeList As TabletSmbCommonClassIdleTimeInfoDataTable = GetRestList(restIdleTimeTable, procStartDate, procEndDate, procDate)
        'ローカル変数．ストール非稼働マスタ情報リストを絞り込む
        Dim sortedList = From p In restIdleTimeList _
                           Where p.IDLE_START_TIME < idleEndDateTime _
                           And idleStartDateTime < p.IDLE_END_TIME _
                           Order By p.IDLE_START_TIME, p.IDLE_END_TIME


        Using retTable As New TabletSmbCommonClassIdleTimeInfoDataTable
            For Each sortedRow As TabletSmbCommonClassIdleTimeInfoRow In sortedList
                Dim newRow As TabletSmbCommonClassIdleTimeInfoRow = CType(retTable.NewRow, TabletSmbCommonClassIdleTimeInfoRow)
                If Not sortedRow.IsIDLE_TYPENull Then
                    newRow.IDLE_TYPE = sortedRow.IDLE_TYPE
                End If
                If Not sortedRow.IsIDLE_START_TIMENull Then
                    newRow.IDLE_START_TIME = sortedRow.IDLE_START_TIME
                End If
                If Not sortedRow.IsIDLE_END_TIMENull Then
                    newRow.IDLE_END_TIME = sortedRow.IDLE_END_TIME
                End If
                If Not sortedRow.IsSTALL_IDLE_IDNull Then
                    newRow.STALL_IDLE_ID = sortedRow.STALL_IDLE_ID
                End If

                retTable.Rows.Add(newRow)
            Next
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
            '作成したテーブルを返却する
            Return retTable
        End Using
    End Function


    ''' <summary>
    ''' 休憩テーブルの休憩時間に日付を追加する
    ''' </summary>
    ''' <param name="stallIdleTbl">ストール非稼働テーブル</param>
    ''' <param name="procStartDate">処理開始日付</param>
    ''' <param name="procEndDate">処理終了日付</param>
    ''' <param name="procDate">処理日付</param>
    ''' <returns>絞り込んだ休憩テーブル</returns>
    ''' <remarks></remarks>
    Private Function GetRestList(ByVal stallIdleTbl As TabletSmbCommonClassIdleTimeInfoDataTable, _
                          ByVal procStartDate As Date, _
                          ByVal procEndDate As Date, _
                          ByVal procDate As Date) As TabletSmbCommonClassIdleTimeInfoDataTable
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. procStartDate={1}, procEndDate={2}, procDate={3}" _
                , MethodBase.GetCurrentMethod.Name, procStartDate, procEndDate, procDate))

        '戻す用テーブル
        Using stallIdleMstList As New TabletSmbCommonClassIdleTimeInfoDataTable

            'ローカル変数．処理日付 <= ローカル変数．処理終了日付 の場合、処理を繰り返す
            While procDate.CompareTo(procEndDate) <= 0
                '取得したストール非稼働マスタエンティティ分繰り返す
                For Each stallIdle As TabletSMBCommonClassDataSet.TabletSmbCommonClassIdleTimeInfoRow In stallIdleTbl.Rows
                    'ストール非稼働マスタ．非稼働開始時間 <= ストール非稼働マスタ．非稼働終了時間 の場合
                    If stallIdle.IDLE_START_TIME.CompareTo(stallIdle.IDLE_END_TIME) <= 0 Then
                        Dim stallIdleData As TabletSmbCommonClassIdleTimeInfoRow = CType(stallIdleMstList.NewRow, TabletSmbCommonClassIdleTimeInfoRow)
                        stallIdleData.STALL_IDLE_ID = stallIdle.STALL_IDLE_ID
                        stallIdleData.IDLE_START_TIME = procDate.Add(stallIdle.IDLE_START_TIME.TimeOfDay)
                        stallIdleData.IDLE_END_TIME = procDate.Add(stallIdle.IDLE_END_TIME.TimeOfDay)
                        'ローカル変数．ストール非稼働マスタ情報リストに追加する
                        stallIdleMstList.Rows.Add(stallIdleData)
                    Else
                        '処理日付 = 処理開始日付 の場合
                        If procDate = procStartDate Then
                            '非稼働開始日時 = 処理日付の前日 + ストール非稼働マスタ．非稼働開始時間
                            '非稼働終了日時 = 処理日付 + ストール非稼働マスタ．非稼働開始時間
                            Dim stallIdlePrevData As TabletSmbCommonClassIdleTimeInfoRow = CType(stallIdleMstList.NewRow, TabletSmbCommonClassIdleTimeInfoRow)
                            stallIdlePrevData.STALL_IDLE_ID = stallIdle.STALL_IDLE_ID
                            stallIdlePrevData.IDLE_START_TIME = procDate.AddDays(-1).Add(stallIdle.IDLE_START_TIME.TimeOfDay)
                            stallIdlePrevData.IDLE_END_TIME = procDate.Add(stallIdle.IDLE_END_TIME.TimeOfDay)
                            'ローカル変数．ストール非稼働マスタ情報リストに追加する
                            stallIdleMstList.Rows.Add(stallIdlePrevData)
                        End If
                        '非稼働開始日時 = 処理日付 + ストール非稼働マスタ．非稼働開始時間 
                        '非稼働終了日時 = 処理日付の翌日 + ストール非稼働マスタ．非稼働開始時間  
                        Dim stallIdleNextData As TabletSmbCommonClassIdleTimeInfoRow = CType(stallIdleMstList.NewRow, TabletSmbCommonClassIdleTimeInfoRow)
                        stallIdleNextData.STALL_IDLE_ID = stallIdle.STALL_IDLE_ID
                        stallIdleNextData.IDLE_START_TIME = procDate.Add(stallIdle.IDLE_START_TIME.TimeOfDay)
                        stallIdleNextData.IDLE_END_TIME = procDate.AddDays(1).Add(stallIdle.IDLE_END_TIME.TimeOfDay)
                        'ローカル変数．ストール非稼働マスタ情報リストに追加する
                        stallIdleMstList.Rows.Add(stallIdleNextData)
                    End If
                Next
                'ローカル変数．処理日付 = 処理日付の翌日
                procDate = procDate.AddDays(1)
            End While

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
            Return stallIdleMstList
        End Using
    End Function
#End Region

#End Region

#Region "営業日取得"
    ''' <summary>
    ''' 指定された日時又は日付の営業日を取得します
    ''' </summary>
    ''' <param name="judgeDateTime">判定日時</param>
    ''' <param name="workingStartTime">営業開始時間</param>
    ''' <returns>営業日</returns>
    ''' <remarks></remarks>
    Private Function GetWorkingDate(ByVal judgeDateTime As Date, _
                                          ByVal workingStartTime As Date) As Date

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. judgeDateTime={1}" _
                , MethodBase.GetCurrentMethod.Name, judgeDateTime))
        '営業開始時間
        Dim startTime As TimeSpan = New TimeSpan(workingStartTime.Hour, workingStartTime.Minute, 0)
        '判定時間
        Dim judgeTime As TimeSpan = judgeDateTime.TimeOfDay

        '判定時間 < 開始時間
        If judgeTime.CompareTo(startTime) < 0 Then
            '判定日時の前日を返却する
            Return judgeDateTime.AddDays(-1).Date
        End If
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E workDate={1}", MethodBase.GetCurrentMethod.Name, judgeDateTime.Date))
        '判定日時の日付を返却する
        Return judgeDateTime.Date
    End Function
#End Region

#Region "ストール表示対象日時取得"
    ''' <summary>
    ''' 指定された日時又は日付の営業日を取得します
    ''' </summary>
    ''' <param name="judgeDateTime">判定日時</param>
    ''' <param name="stallStartTime">ストール稼動開始日時</param>
    ''' <param name="stallEndTime">ストール稼動終了日時</param>
    ''' <returns>営業日</returns>
    ''' <remarks></remarks>
    Private Function GetStallDispDate(ByVal judgeDateTime As Date, _
                                     ByVal stallStartTime As Date, _
                                     ByVal stallEndTime As Date) As List(Of Date)

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. judgeDateTime={1},  stallStartTime={2},  stallEndTime={3}" _
            , MethodBase.GetCurrentMethod.Name, judgeDateTime, stallStartTime, stallEndTime))

        '営業開始日時
        Dim startWorkingDateTime As Date = DefaultDateTimeValueGet()

        Dim localWorkingDate As Date = GetWorkingDate(judgeDateTime, stallStartTime)
        startWorkingDateTime = New Date(localWorkingDate.Year, localWorkingDate.Month, localWorkingDate.Day, _
                                          stallStartTime.Hour, stallStartTime.Minute, 0)

        '営業終了日時
        Dim endWorkingDateTime As Date = DefaultDateTimeValueGet()
        '営業時間が日を跨がない場合
        If New TimeSpan(stallStartTime.Hour, stallStartTime.Minute, 0).CompareTo(New TimeSpan(stallEndTime.Hour, stallEndTime.Minute, 0)) < 0 Then
            '営業終了日時 = 営業開始日時の日付 + 営業終了時間
            endWorkingDateTime = New DateTime(startWorkingDateTime.Year, startWorkingDateTime.Month, startWorkingDateTime.Day, _
                                                stallEndTime.Hour, stallEndTime.Minute, 0)
        Else
            '営業開始日時の翌日の日付を算出
            Dim nextDate As Date = startWorkingDateTime.AddDays(1)

            '営業終了日時 = 営業開始日時の翌日の日付 + 営業終了時間
            endWorkingDateTime = New DateTime(nextDate.Year, nextDate.Month, nextDate.Day, _
                                                stallEndTime.Hour, stallEndTime.Minute, 0)
        End If

        Dim rtDate As New List(Of Date)
        rtDate.Add(startWorkingDateTime)
        rtDate.Add(endWorkingDateTime)

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
        'ストール表示対象の日時情報を返却する
        Return rtDate

    End Function
#End Region

#Region "スタッフコード取得"
    ''' <summary>
    ''' スタッフストール割当テーブルからストールIDにより、スタッフコードテーブルを取得します
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="stallId">ストールID</param>
    ''' <returns>スタッフコード</returns>
    ''' <remarks>ストールに配置可能なスタッフ(TC、CHT)を取得対象とする</remarks>
    Private Function GetStaffCodeByStallId(ByVal dealerCode As String, ByVal branchCode As String, ByVal stallId As Decimal) As TabletSmbCommonClassStringValueDataTable
        Using ta As New TabletSMBCommonClassDataSetTableAdapters.TabletSMBCommonClassDataAdapter
            Dim stallIdList As List(Of Decimal) = New List(Of Decimal)
            stallIdList.Add(stallId)

            '2014/07/02 TMEJ 張 BTS-283 「サービスマネージャをTCとして表示」対応 START
            'スタッフストール表示区分を取得
            Dim stfStallDisptype As String = Me.GetStaffStallDispType(dealerCode, branchCode)

            'Return ta.GetStaffCodeByStallId(dealerCode, branchCode, stallIdList)
            Return ta.GetStaffCodeByStallId(dealerCode, branchCode, stallIdList, stfStallDisptype)
            '2014/07/02 TMEJ 張 BTS-283 「サービスマネージャをTCとして表示」対応 END
        End Using
    End Function

    '2014/07/02 TMEJ 張 BTS-283 「サービスマネージャをTCとして表示」対応 START
    ''' <summary>
    ''' スタッフストール表示区分を取得します
    ''' </summary>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inBranchCode">店舗コード</param>
    ''' <returns>スタッフコード</returns>
    ''' <remarks>ストールに配置可能なスタッフ(TC、CHT)を取得対象とする</remarks>
    Public Function GetStaffStallDispType(ByVal inDealerCode As String, ByVal inBranchCode As String) As String
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.Start. inDealerCode={1}, inBranchCode={2}", _
                                  MethodBase.GetCurrentMethod.Name, inDealerCode, inBranchCode))

        Using ta As New TabletSMBCommonClassDataSetTableAdapters.TabletSMBCommonClassDataAdapter

            'スタッフストール表示区分を取得
            Dim stfStallDisptypeTable As TabletSmbCommonClassStringValueDataTable = _
                ta.GetStaffStallDispType(inDealerCode, inBranchCode)
            'スタッフストール表示区分
            Dim stfStallDisptype As String = String.Empty
            'スタッフストール表示区分
            If stfStallDisptypeTable.Count > 0 Then
                If Not IsDBNull(stfStallDisptypeTable(0).COL1) Then
                    stfStallDisptype = stfStallDisptypeTable(0).COL1
                End If
            End If

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}.End return={1}", _
                                      MethodBase.GetCurrentMethod.Name, _
                                      stfStallDisptype))
            Return stfStallDisptype
            '2014/07/02 TMEJ 張 BTS-283 「サービスマネージャをTCとして表示」対応 END
        End Using
    End Function
    '2014/07/02 TMEJ 張 BTS-283 「サービスマネージャをTCとして表示」対応 END

#End Region

#Region "画面用基本情報を取得"
    ''' <summary>
    ''' 店舗稼動時間情報の取得
    ''' </summary>
    ''' <param name="dlrCode">販売店コード</param>
    ''' <param name="brnCode">店舗コード</param>
    ''' <returns>店舗稼動時間情報テーブル</returns>
    ''' <remarks></remarks>
    Public Function GetBranchOperatingHours(ByVal dlrCode As String, ByVal brnCode As String) As TabletSMBCommonClassDataSet.TabletSmbCommonClassBranchOperatingHoursDataTable
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S.", MethodBase.GetCurrentMethod.Name))
        Using ta As New TabletSMBCommonClassDataSetTableAdapters.TabletSMBCommonClassDataAdapter
            Dim dt As TabletSMBCommonClassDataSet.TabletSmbCommonClassBranchOperatingHoursDataTable
            dt = ta.GetBranchOperatingHours(dlrCode, brnCode)
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
            Return dt
        End Using
    End Function

    '2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応 START
    ''' <summary>
    ''' ある日の店舗稼動時間情報の取得
    ''' </summary>
    ''' <param name="inDay">取得したい営業日時の日付</param>
    ''' <param name="inDlrCode">販売店コード</param>
    ''' <param name="inBrnCode">店舗コード</param>
    ''' <returns>店舗稼動時間情報テーブル(エラー発生する場合、Nothingを戻す)</returns>
    ''' <remarks></remarks>
    Public Function GetOneDayBrnOperatingHours(ByVal inDay As Date, _
                                               ByVal inDlrCode As String, _
                                               ByVal inBrnCode As String) As TabletSmbCommonClassBranchOperatingHoursDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.Start. inDay={1}, inDlrCode={2}, inBrnCode={3}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  inDay, _
                                  inDlrCode, _
                                  inBrnCode))

        '■■■■■SQL_TABLETSMBCOMMONCLASS_001 店舗稼働時間の取得 2-19 3-1-0-0-0-0 START■■■■■
        LogServiceCommonBiz.OutputLog(53, "●■● 2.3.1 TABLETSMBCOMMONCLASS_001 START")

        '営業開始と終了時間を取得する
        Dim dtBranchOperatingHours As TabletSmbCommonClassBranchOperatingHoursDataTable = _
            Me.GetBranchOperatingHours(inDlrCode, _
                                       inBrnCode)

        LogServiceCommonBiz.OutputLog(53, "●■● 2.3.1 TABLETSMBCOMMONCLASS_001 END")
        '■■■■■SQL_TABLETSMBCOMMONCLASS_001 店舗稼働時間の取得 2-19 3-1--0-0-0 END■■■■■

        '取得できなかった場合、Nothingを戻す
        If 0 = dtBranchOperatingHours.Count Then

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                       "{0}.End. ExceptionError:GetBranchOperatingHours", _
                                      MethodBase.GetCurrentMethod.Name))
            Return Nothing

        End If

        '当日の営業開始日時を設定する(実績終了日時の年月日+営業開始時間)
        Dim stallStartTime As Date = New Date(inDay.Year, _
                                              inDay.Month, _
                                              inDay.Day, _
                                              dtBranchOperatingHours(0).SVC_JOB_START_TIME.Hour, _
                                              dtBranchOperatingHours(0).SVC_JOB_START_TIME.Minute, _
                                              0)

        '当日の営業終了日時を設定する(実績終了日時の年月日+営業終了時間)
        Dim stallEndTime As Date = New Date(inDay.Year, _
                                            inDay.Month, _
                                            inDay.Day, _
                                            dtBranchOperatingHours(0).SVC_JOB_END_TIME.Hour, _
                                            dtBranchOperatingHours(0).SVC_JOB_END_TIME.Minute, _
                                            0)

        '営業終了日時 < 営業開始日時の場合、予期せぬエラーをを出す
        If 0 <= Date.Compare(stallStartTime, stallEndTime) Then

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                           "{0}.End. ExceptionError:stallEndTime <= stallStartTime", _
                          MethodBase.GetCurrentMethod.Name))
            Return Nothing

        End If

        'テーブルに戻す用データを設定する
        dtBranchOperatingHours(0).SVC_JOB_START_TIME = stallStartTime
        dtBranchOperatingHours(0).SVC_JOB_END_TIME = stallEndTime

        '返却
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.End. stallStartTime={1}, stallEndTime={2}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  stallStartTime, _
                                  stallEndTime))
        Return dtBranchOperatingHours

    End Function

    '2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応 END

    ''' <summary>
    ''' リサイズ単位の取得
    ''' </summary>
    ''' <param name="dlrCode">販売店コード</param>
    ''' <param name="brnCode">店舗コード</param>
    ''' <returns>リサイズ単位</returns>
    ''' <remarks></remarks>
    Public Function GetIntervalTime(ByVal dlrCode As String, ByVal brnCode As String) As Long
        Using ta As New TabletSMBCommonClassDataSetTableAdapters.TabletSMBCommonClassDataAdapter
            '何分単位でリサイズ
            Dim intervalTime As Long = ta.GetIntervalTime(dlrCode, brnCode)
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E intervalTime={1}", MethodBase.GetCurrentMethod.Name, intervalTime))
            Return intervalTime
        End Using
    End Function

#End Region

#Region "ストール非稼働情報の取得"
    ''' <summary>
    ''' ストール非稼働情報の取得
    ''' </summary>
    Public Function GetAllIdleDateInfo(ByVal stallIdList As List(Of Decimal), ByVal idleStartDateTime As Date, _
                             ByVal idleEndDateTime As Date) As TabletSMBCommonClassDataSet.TabletSmbCommonClassStallIdleInfoDataTable
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S.", MethodBase.GetCurrentMethod.Name))

        Dim dt As TabletSMBCommonClassDataSet.TabletSmbCommonClassStallIdleInfoDataTable
        Using ta As New TabletSMBCommonClassDataSetTableAdapters.TabletSMBCommonClassDataAdapter
            dt = ta.GetAllIdleDateInfo(stallIdList, idleStartDateTime, idleEndDateTime)
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
        End Using
        Return dt
    End Function
#End Region

#Region "遅れ見込み時間の取得"
    ''' <summary>
    ''' 遅れ見込みリストを取得
    ''' </summary>
    ''' <param name="svcinIdList">サービス入庫ID</param>
    ''' <param name="dlrCode">販売店コード</param>
    ''' <param name="brnCode">店舗コード</param>
    ''' <param name="dtNow">現在日時</param>
    ''' <returns>遅れ見込み情報</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2015/01/14 TMEJ 明瀬 納車遅れ見込計算の不具合修正
    ''' </history>
    Public Function GetDeliveryDelayDateList(ByVal svcinIdList As List(Of Decimal), _
                                             ByVal dlrCode As String, _
                                             ByVal brnCode As String, _
                                             ByVal dtNow As Date) As TabletSMBCommonClassDataSet.TabletSmbCommonClassDeliDelayDateDataTable
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S.", MethodBase.GetCurrentMethod.Name))

        Dim dtDeliDelay As TabletSmbCommonClassDeliDelayDateDataTable
        Using commBiz As New SMBCommonClassBusinessLogic
            Dim localSvcinIdList As List(Of Decimal)
            Using ta As New TabletSMBCommonClassDataSetTableAdapters.TabletSMBCommonClassDataAdapter

                '■■■■■SQL_TABLETSMBCOMMONCLASS_033 予定納車日時があるサービス入庫IDを取得する 2-5 1-3-1-0-0-0 START■■■■■
                LogServiceCommonBiz.OutputLog(39, "●■● 2.1.3.1 TABLETSMBCOMMONCLASS_033 START")

                '予定納車日時がある、かつ実績納車日時がないサービス入庫IDを絞り込む
                localSvcinIdList = ta.GetHasScheDeliDateSvcinId(svcinIdList, dtNow)

                LogServiceCommonBiz.OutputLog(39, "●■● 2.1.3.1 TABLETSMBCOMMONCLASS_033 END")
                '■■■■■SQL_TABLETSMBCOMMONCLASS_033 予定納車日時があるサービス入庫IDを絞り込む 2-5 1-3-1-0-0-0 END■■■■■

                If localSvcinIdList.Count = 0 Then
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E GetDeliveryDelayDateList count=0", MethodBase.GetCurrentMethod.Name))
                    Using dtReturn As New TabletSmbCommonClassDeliDelayDateDataTable
                        Return dtReturn
                    End Using
                End If

                '■■■■■SQL_TABLETSMBCOMMONCLASS_005 遅れ見込み情報を取得する 2-6 1-3-2-0-0-0 START■■■■■
                LogServiceCommonBiz.OutputLog(40, "●■● 2.1.3.2 TABLETSMBCOMMONCLASS_005 START")

                '遅れ見込みを計算するため、いろいろな引数を取得する
                dtDeliDelay = ta.GetDeliDelayInfo(localSvcinIdList)

                LogServiceCommonBiz.OutputLog(40, "●■● 2.1.3.2 TABLETSMBCOMMONCLASS_005 END")
                '■■■■■SQL_TABLETSMBCOMMONCLASS_005 遅れ見込み情報を取得する 2-6 1-3-2-0-0-0 END■■■■■

            End Using

            '■■■■■残作業時間、作業終了予定時刻追加 2-7 1-3-3-0-0-0 START■■■■■
            LogServiceCommonBiz.OutputLog(41, "●■● 2.1.3.3 残作業時間、作業終了予定時刻追加 START")

            '残作業時間、作業終了予定時刻など値をdtDeliDelayテーブルに追加
            dtDeliDelay = GetAllMaxEndDateAndRemainingTime(dlrCode, brnCode, localSvcinIdList, dtDeliDelay)

            LogServiceCommonBiz.OutputLog(41, "●■● 2.1.3.3 残作業時間、作業終了予定時刻追加 END")
            '■■■■■残作業時間、作業終了予定時刻追加 2-7 1-3-3-0-0-0 END■■■■■

            '■■■■■検査ステータスを遅れ見込み情報テーブルに追加 2-9 1-3-4-0-0-0 START■■■■■
            LogServiceCommonBiz.OutputLog(43, "●■● 2.1.3.4 検査ステータスを遅れ見込み情報テーブルに追加 START")

            '検査ステータスをdtDeliDelayテーブルに追加
            dtDeliDelay = GetInspectionStatusBySvcinId(localSvcinIdList, dtDeliDelay)

            LogServiceCommonBiz.OutputLog(43, "●■● 2.1.3.4 検査ステータスを遅れ見込み情報テーブルに追加 END")
            '■■■■■検査ステータスを遅れ見込み情報テーブルに追加 2-9 1-3-4-0-0-0 END■■■■■

            '■■■■■SMBCOMMON共通関数初期処理 2-11 1-3-5-0-0-0 START■■■■■
            LogServiceCommonBiz.OutputLog(45, "●■● 2.1.3.5 SMBCOMMON共通関数初期処理 START")

            'GetDeliveryDelayDate関数を呼ぶ前、initが必要
            commBiz.InitCommon(dlrCode, brnCode, dtNow)

            LogServiceCommonBiz.OutputLog(45, "●■● 2.1.3.5 SMBCOMMON共通関数初期処理 END")
            '■■■■■SMBCOMMON共通関数初期処理 2-11 1-3-5-0-0-0 START■■■■■

            '完成検査完了時刻、納車予定時刻など値を取得する
            For Each drDeliDelay As TabletSMBCommonClassDataSet.TabletSmbCommonClassDeliDelayDateRow In dtDeliDelay.Rows

                '2017/09/27 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
                '表示区分を取得する
                'Dim dispType As SMBCommonClassBusinessLogic.DisplayType = GetDispType(drDeliDelay.SVC_STATUS, drDeliDelay.INSPECTION_STATUS)

                '洗車終了有無
                Dim carWashEndType As String

                'RO有無
                Dim orderDataType As String

                '表示区分
                Dim dispType As DisplayType

                'R/Oステータス（最大）
                Dim maxROStatus = String.Empty

                'R/Oステータス（最小）
                Dim minROStatus = String.Empty

                '清算書印刷日時
                Dim printDateTime As Date = Date.MinValue

                '2017/09/27 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END

                Dim inInspectEndTime As Date
                Dim inWashStartTime As Date
                Dim inWashEndTime As Date

                '2015/01/14 TMEJ 明瀬 納車遅れ見込計算の不具合修正 START

                ''洗車開始時刻
                'If drDeliDelay.IsCARWASH_START_DATETIMENull Then
                '    inWashStartTime = Date.MinValue
                'Else
                '    inWashStartTime = drDeliDelay.CARWASH_START_DATETIME
                'End If
                ''洗車終了時刻
                'If drDeliDelay.IsCARWASH_END_DATETIMENull Then
                '    inWashEndTime = Date.MinValue
                'Else
                '    inWashEndTime = drDeliDelay.CARWASH_END_DATETIME
                'End If

                '洗車開始日時
                If drDeliDelay.IsCARWASH_START_DATETIMENull _
                OrElse drDeliDelay.CARWASH_START_DATETIME.Equals(DefaultDateTimeValueGet) Then
                    '洗車実績レコードがない(洗車開始前)、
                    'または洗車実績レコードがあるが実績開始日時が未設定(通常あり得ない)の場合
                    '実績開始日時を日付の最小値に設定
                    inWashStartTime = Date.MinValue
                Else
                    '上記以外(洗車中、洗車済み)の場合
                    '洗車開始実績を設定
                    inWashStartTime = drDeliDelay.CARWASH_START_DATETIME
                End If

                '洗車終了日時
                If drDeliDelay.IsCARWASH_END_DATETIMENull _
                OrElse drDeliDelay.CARWASH_END_DATETIME.Equals(DefaultDateTimeValueGet) Then
                    '洗車実績レコードがない(洗車開始前)、
                    'または洗車実績レコードがあるが実績終了日時が未設定(洗車中)の場合
                    '実績終了日時を日付の最小値に設定
                    inWashEndTime = Date.MinValue

                    '2017/09/27 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
                    '洗車終了した
                    carWashEndType = CarWashEndTypeWashing
                    '2017/09/27 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
                Else
                    '上記以外(洗車済み)の場合
                    '実績終了日時を設定
                    inWashEndTime = drDeliDelay.CARWASH_END_DATETIME

                    '2017/09/27 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
                    '洗車終了していない
                    carWashEndType = CarWashEndTypeWashEnd
                    '2017/09/27 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
                End If

                '2015/01/14 TMEJ 明瀬 納車遅れ見込計算の不具合修正 END

                '2017/09/27 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START


                'RO番号が設定されている場合
                'かつROステータスの最大が設定されている場合
                'かつROステータスの最小が設定されている場合
                If Not drDeliDelay.IsRO_NUMNull AndAlso _
                    Not String.IsNullOrWhiteSpace(drDeliDelay.RO_NUM) AndAlso _
                     Not drDeliDelay.IsMAX_RO_STATUSNull AndAlso _
                      Not String.IsNullOrWhiteSpace(drDeliDelay.MAX_RO_STATUS) AndAlso _
                       Not drDeliDelay.IsMIN_RO_STATUSNull AndAlso _
                        Not String.IsNullOrWhiteSpace(drDeliDelay.MIN_RO_STATUS) Then

                    'RO情報あり
                    orderDataType = RepairOrderTypeExist

                    '取得したR/Oステータス（最大）
                    maxROStatus = drDeliDelay.MAX_RO_STATUS

                    '取得したR/Oステータス（最小）
                    minROStatus = drDeliDelay.MIN_RO_STATUS
                Else
                    '上記以外の場合
                    'RO情報なし
                    orderDataType = RepairOrderTypeNone


                End If

                Using smbCommonBiz As New SMBCommonClassBusinessLogic
                    '表示区分取得
                    dispType = CType(smbCommonBiz.GetChipArea(orderDataType, maxROStatus, ReserveEffective, carWashEndType, _
                                                              drDeliDelay.SVC_STATUS, minROStatus), DisplayType)

                    '取得した表示区分が納車作業また納車準備以外の場合
                    If Not (dispType = DisplayType.DeliveryWork OrElse _
                                dispType = DisplayType.DeliveryPreparation) Then

                        '表示区分を作業中に設定
                        dispType = DisplayType.Work
                    End If
                End Using
                '2017/09/27 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END


                '完成検査完了時刻がただ納車準備の場合使う
                '2017/10/25 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
                'If dispType = DisplayType.Work Or drDeliDelay.IsMAX_END_DATETIMENull Then
                If dispType = DisplayType.Work Or drDeliDelay.IsMAX_INSPECTION_DATETIMENull Then
                    '2017/10/25 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
                    '作業中の場合、完成検査完了時刻もいらない
                    inInspectEndTime = Date.MinValue
                Else
                    '2017/10/25 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
                    ''作業終了日時に設定する
                    'inInspectEndTime = drDeliDelay.MAX_END_DATETIME

                    '完成検査承認日時を設定する
                    inInspectEndTime = drDeliDelay.MAX_INSPECTION_DATETIME
                    '2017/10/25 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
                End If

                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                Try
                    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
                    '遅れ見込み時間を計算する
                    '2017/09/05 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
                    'drDeliDelay.DELI_DELAY_DATETIME = commBiz.GetDeliveryDelayDate(dispType, drDeliDelay.SCHE_DELI_DATETIME, _
                    '                                                               drDeliDelay.MAX_END_DATETIME, inInspectEndTime, inWashStartTime, _
                    '                                                               inWashEndTime, Date.MinValue, drDeliDelay.REMAINING_WORK_TIME, drDeliDelay.CARWASH_NEED_FLG, dtNow)
                    drDeliDelay.DELI_DELAY_DATETIME = commBiz.GetDeliveryDelayDate(dispType, drDeliDelay.SCHE_DELI_DATETIME, _
                                                               drDeliDelay.MAX_END_DATETIME, inInspectEndTime, inWashStartTime, _
                                                               inWashEndTime, drDeliDelay.INVOICE_PRINT_DATETIME, drDeliDelay.REMAINING_WORK_TIME, drDeliDelay.CARWASH_NEED_FLG, dtNow, drDeliDelay.REMAINING_INSPECTION_TYPE)

                    '2017/09/05 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END

                    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                Catch ex As Exception
                    '遅れ見込み計算の場合、エラーがあれば、そのままで出さない(画面の運用に影響がないように)
                    drDeliDelay.DELI_DELAY_DATETIME = Me.DefaultDateTimeValueGet()
                End Try
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
            Next
        End Using
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
        Return dtDeliDelay

    End Function

    ''' <summary>
    ''' 残作業時間、作業終了予定時刻など値を遅れ見込み情報テーブルに追加
    ''' </summary>
    ''' <param name="dlrCode">販売店コード</param>
    ''' <param name="brnCode">店舗コード</param>
    ''' <param name="svcinIdList">サービス入庫ID</param>
    ''' <param name="dtDeliDelay">遅れ見込み情報テーブル</param>
    ''' <returns>遅れ見込み情報テーブル</returns>
    ''' <remarks></remarks>
    Private Function GetAllMaxEndDateAndRemainingTime(ByVal dlrCode As String, _
                                                ByVal brnCode As String, _
                                                ByVal svcinIdList As List(Of Decimal), _
                                                ByVal dtDeliDelay As TabletSMBCommonClassDataSet.TabletSmbCommonClassDeliDelayDateDataTable) As TabletSMBCommonClassDataSet.TabletSmbCommonClassDeliDelayDateDataTable
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S.", MethodBase.GetCurrentMethod.Name))

        '残作業時間、作業終了予定時刻など値のテーブルを取得する
        Dim dtMaxEndTimeInfo As TabletSMBCommonClassDataSet.TabletSmbCommonClassMaxEndDateInfoDataTable
        Using ta As New TabletSMBCommonClassDataSetTableAdapters.TabletSMBCommonClassDataAdapter

            '■■■■■SQL_TABLETSMBCOMMONCLASS_018 遅れ見込み時刻計算用の残作業時間と最後終了時間を取得する 2-8 1-3-3-1-0-0 START■■■■■
            LogServiceCommonBiz.OutputLog(42, "●■● 2.1.3.3.1 TABLETSMBCOMMONCLASS_018 START")

            dtMaxEndTimeInfo = ta.GetAllMaxEndDateAndRemainingTime(svcinIdList, dlrCode, brnCode)

            LogServiceCommonBiz.OutputLog(42, "●■● 2.1.3.3.1 TABLETSMBCOMMONCLASS_018 END")
            '■■■■■SQL_TABLETSMBCOMMONCLASS_018 遅れ見込み時刻計算用の残作業時間と最後終了時間を取得する 2-8 1-3-3-1-0-0 END■■■■■

        End Using
        '結果がない場合、サービス入庫ID対応するチップが全部納車済またはサービス入庫IDは空白
        If dtMaxEndTimeInfo.Count = 0 Then
            Return dtDeliDelay
        End If

        'ループ用の上回サービス入庫ID
        Dim preSvcinId As Decimal = -1
        Dim remainingTime As Long = 0
        Dim maxDate As Date = DefaultDateTimeValueGet()
        Using dtMaxEndDateList As New TabletSmbCommonClassMaxEndDateAndRemainingTimeDataTable
            Dim drMaxEndDate As TabletSmbCommonClassMaxEndDateAndRemainingTimeRow = CType(dtMaxEndDateList.NewRow, TabletSmbCommonClassMaxEndDateAndRemainingTimeRow)
            For Each drMaxEndTimeInfo As TabletSmbCommonClassMaxEndDateInfoRow In dtMaxEndTimeInfo
                drMaxEndDate = CType(dtMaxEndDateList.NewRow, TabletSmbCommonClassMaxEndDateAndRemainingTimeRow)
                If preSvcinId <> -1 And preSvcinId <> drMaxEndTimeInfo.SVCIN_ID Then
                    '設定する
                    drMaxEndDate.SVCIN_ID = preSvcinId
                    drMaxEndDate.REMAIN_WORKTIME = remainingTime
                    drMaxEndDate.MAX_END_DATETIME = maxDate
                    dtMaxEndDateList.AddTabletSmbCommonClassMaxEndDateAndRemainingTimeRow(drMaxEndDate)

                    'クリア
                    remainingTime = 0
                    maxDate = DefaultDateTimeValueGet()
                End If
                '各チップの作業終了予定日時をArrayListに設定する
                Dim dtMaxEndTime As TabletSmbCommonClassMaxEndDateDataTable = GetMaxEndDate(drMaxEndTimeInfo)
                '大きいの日時を保存する
                If maxDate.CompareTo(dtMaxEndTime(0).MAX_END_DATE) < 0 Then
                    maxDate = dtMaxEndTime(0).MAX_END_DATE
                End If
                'リレーションチップの残作業時間を全部加える
                remainingTime = remainingTime + dtMaxEndTime(0).REMAIN_WORK_TIME
                preSvcinId = drMaxEndTimeInfo.SVCIN_ID
            Next
            drMaxEndDate = CType(dtMaxEndDateList.NewRow, TabletSmbCommonClassMaxEndDateAndRemainingTimeRow)
            '最後のサービス入庫記録を挿入する
            drMaxEndDate.SVCIN_ID = preSvcinId
            drMaxEndDate.REMAIN_WORKTIME = remainingTime
            '最後の日付を取得して、maxEndTimeに設定する
            drMaxEndDate.MAX_END_DATETIME = maxDate
            dtMaxEndDateList.AddTabletSmbCommonClassMaxEndDateAndRemainingTimeRow(drMaxEndDate)

            'テーブルに最終作業完了日時と残作業時間を設定する
            For Each objNewMaxEndDate As TabletSmbCommonClassMaxEndDateAndRemainingTimeRow In dtMaxEndDateList
                Dim svcinId As Decimal = objNewMaxEndDate.SVCIN_ID
                Dim targetList = (From p In dtDeliDelay Where p.SVCIN_ID = svcinId Select p).ToList()
                targetList(0).MAX_END_DATETIME = objNewMaxEndDate.MAX_END_DATETIME
                targetList(0).REMAINING_WORK_TIME = objNewMaxEndDate.REMAIN_WORKTIME
            Next
        End Using
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
        Return dtDeliDelay
    End Function

    ''' <summary>
    ''' 検査ステータスを遅れ見込み情報テーブルに追加
    ''' </summary>
    ''' <param name="svcinIdList">サービス入庫ID</param>
    ''' <param name="dtDeliDelay">遅れ見込み情報テーブル</param>
    ''' <returns>遅れ見込み情報テーブル</returns>
    ''' <remarks></remarks>
    Private Function GetInspectionStatusBySvcinId(ByVal svcinIdList As List(Of Decimal), _
                                                ByVal dtDeliDelay As TabletSMBCommonClassDataSet.TabletSmbCommonClassDeliDelayDateDataTable) As TabletSMBCommonClassDataSet.TabletSmbCommonClassDeliDelayDateDataTable
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S.", MethodBase.GetCurrentMethod.Name))

        Dim dtInspectionStatus As TabletSMBCommonClassDataSet.TabletSmbCommonClassDeliDelayDateDataTable
        Using ta As New TabletSMBCommonClassDataSetTableAdapters.TabletSMBCommonClassDataAdapter

            '■■■■■SQL_TABLETSMBCOMMONCLASS_032 サービス入庫単位での完成検査ステータスの取得 2-10 1-3-4-1-0-0 START■■■■■
            LogServiceCommonBiz.OutputLog(44, "●■● 2.1.3.4.1 TABLETSMBCOMMONCLASS_032 START")

            dtInspectionStatus = ta.GetInspectionStatusBySvcinId(svcinIdList)

            LogServiceCommonBiz.OutputLog(44, "●■● 2.1.3.4.1 TABLETSMBCOMMONCLASS_032 END")
            '■■■■■SQL_TABLETSMBCOMMONCLASS_032 サービス入庫単位での完成検査ステータスの取得 2-10 1-3-4-1-0-0 END■■■■■
        End Using

        '検査ステータスを設定
        For Each drDeliDelay As TabletSMBCommonClassDataSet.TabletSmbCommonClassDeliDelayDateRow In dtDeliDelay
            Dim svcinId As Decimal = drDeliDelay.SVCIN_ID
            Dim targetList = (From p In dtInspectionStatus Where p.SVCIN_ID = svcinId Select p).ToArray()

            '全て仕事が検査済の場合、検査済に設定する
            Dim inspectionStatus As String = InspectionFinished
            For Each drInspectionStatus As TabletSMBCommonClassDataSet.TabletSmbCommonClassDeliDelayDateRow In targetList
                If Not InspectionFinished.Equals(drInspectionStatus.INSPECTION_STATUS) Then
                    inspectionStatus = drInspectionStatus.INSPECTION_STATUS
                    Exit For
                End If
            Next

            drDeliDelay.INSPECTION_STATUS = inspectionStatus
        Next
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
        Return dtDeliDelay
    End Function

    ''' <summary>
    ''' 作業終了予定時刻を取得
    ''' </summary>
    ''' <param name="drMaxEndTimeInfo">1行のデータ</param>
    ''' <returns>遅れ見込み情報テーブル</returns>
    ''' <remarks></remarks>
    Private Function GetMaxEndDate(ByVal drMaxEndTimeInfo As TabletSmbCommonClassMaxEndDateInfoRow) As TabletSmbCommonClassMaxEndDateDataTable
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S." _
            , MethodBase.GetCurrentMethod.Name))

        Using dtRet As New TabletSmbCommonClassMaxEndDateDataTable
            Dim drRet As TabletSmbCommonClassMaxEndDateRow = CType(dtRet.NewRow, TabletSmbCommonClassMaxEndDateRow)
            '作業前チップの場合、予定終了時刻を戻る
            If IsDefaultValue(drMaxEndTimeInfo.RSLT_START_DATETIME) Then
                drRet.MAX_END_DATE = drMaxEndTimeInfo.SCHE_END_DATETIME
                drRet.REMAIN_WORK_TIME = drMaxEndTimeInfo.SCHE_WORKTIME
            Else
                If IsDefaultValue(drMaxEndTimeInfo.RSLT_END_DATETIME) Then
                    '作業中チップ
                    drRet.MAX_END_DATE = drMaxEndTimeInfo.PRMS_END_DATETIME

                    '2014/09/11 TMEJ 張 BTS-389 「SMBで異常ではないはずが、遅れ見込み表示になっている」対応 START

                    'drRet.REMAIN_WORK_TIME = drMaxEndTimeInfo.SCHE_WORKTIME

                    '残作業時間 (未着手)：実績チップの場合、残作業の値が要らない
                    drRet.REMAIN_WORK_TIME = 0

                    '2014/09/11 TMEJ 張 BTS-389 「SMBで異常ではないはずが、遅れ見込み表示になっている」対応 END
                Else
                    '作業完了チップ
                    drRet.MAX_END_DATE = drMaxEndTimeInfo.RSLT_END_DATETIME
                    '実績チップの場合、残作業の値が要らない
                    drRet.REMAIN_WORK_TIME = 0
                End If
            End If

            dtRet.Rows.Add(drRet)
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))

            Return dtRet
        End Using
    End Function

    ''' <summary>
    ''' 表示区分を取得
    ''' </summary>
    ''' <param name="stallUseStatus">ストール利用ステータス</param>
    ''' <param name="inspectionStatus">検査ステータス</param>
    ''' <returns>表示区分</returns>
    ''' <remarks></remarks>
    Private Function GetDispType(ByVal stallUseStatus As String, _
                                 ByVal inspectionStatus As String) As SMBCommonClassBusinessLogic.DisplayType
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. stallUseStatus={1}, inspectionStatus={2}" _
            , MethodBase.GetCurrentMethod.Name, stallUseStatus, inspectionStatus))

        '全て作業が検査済の場合、
        If inspectionStatus.Equals("2") Then
            '納車準備
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E showType={1}", MethodBase.GetCurrentMethod.Name, SMBCommonClassBusinessLogic.DisplayType.DeliveryPreparation))
            Return SMBCommonClassBusinessLogic.DisplayType.DeliveryPreparation
        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E showType={1}", MethodBase.GetCurrentMethod.Name, SMBCommonClassBusinessLogic.DisplayType.Work))
        '作業中
        Return SMBCommonClassBusinessLogic.DisplayType.Work
    End Function

#End Region

#Region "履歴登録判断用のチップ情報の取得"
    ''' <summary>
    ''' 履歴登録判断用のチップ情報の取得
    ''' </summary>
    ''' <param name="svcInId">サービス入庫ID</param>
    ''' <param name="dlrCode">販売店コード</param>
    ''' <param name="brnCode">店舗コード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetChipChangeInfo(ByVal svcInId As Decimal, ByVal dlrCode As String, ByVal brnCode As String) As TabletSMBCommonClassDataSet.TabletSmbCommonClassServiceinChangeInfoDataTable
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S.", MethodBase.GetCurrentMethod.Name))

        Dim dt As TabletSMBCommonClassDataSet.TabletSmbCommonClassServiceinChangeInfoDataTable
        Using ta As New TabletSMBCommonClassDataSetTableAdapters.TabletSMBCommonClassDataAdapter
            dt = ta.GetChipChangeInfo(svcInId, dlrCode, brnCode)
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
        End Using
        Return dt
    End Function
#End Region

    '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START

#Region "関連チップに作業中チップ存在するかのチェック"

    ''' <summary>
    ''' 関連チップに作業中チップ存在するかのチェック
    ''' </summary>
    ''' <param name="inSvcInId">サービス入庫ID</param>
    ''' <param name="inStallUseId">ストール利用ID(該当チップ以外の関連チップで検索する)</param>
    ''' <returns>True:存在する False:存在しない</returns>
    ''' <remarks></remarks>
    Private Function IsExistWorkingRelationChip(ByVal inSvcInId As Decimal, _
                                                Optional ByVal inStallUseId As Decimal = -1) As Boolean

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                 "{0}_Start. inSvcInId={1}, inStallUseId={2}", _
                                 System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                 inSvcInId, _
                                 inStallUseId))

        Using ta As New TabletSMBCommonClassDataSetTableAdapters.TabletSMBCommonClassDataAdapter

            '関連チップに作業中チップ存在するかのを取得する
            Dim dt As TabletSMBCommonClassDataSet.TabletSmbCommonClassNumberValueDataTable = _
                ta.IsExistWorkingRelationChip(inSvcInId, inStallUseId)


            If 0 < dt.Count Then
                '取得したデータテーブルのCountが0以上の場合

                '作業中チップがあるから、Trueを戻す
                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                          "{0}.End. Return true.", _
                                          MethodBase.GetCurrentMethod.Name))
                Return True

            Else
                'ほかの場合

                '作業中チップがないから、Falseを戻す
                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                          "{0}.End. Return false.", _
                                          MethodBase.GetCurrentMethod.Name))
                Return False

            End If

        End Using

    End Function

#End Region

    '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

    '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発　START
    '#Region "商品ID取得"
    '    ''' <summary>
    '    ''' 商品ID取得
    '    ''' </summary>
    '    ''' <param name="inDlrCode">ストールID</param>
    '    ''' <param name="inMainteCode">作業開始日時</param>
    '    ''' <param name="inVclVin">作業時間</param>
    '    ''' <returns>商品ID</returns>
    '    ''' <remarks></remarks>
    '    Public Function GetMercId(ByVal inDlrCode As String, _
    '                                          ByVal inMainteCode As String, _
    '                                          ByVal inVclVin As String) As Long
    '        Dim mercId As Long
    '        Using ta As New TabletSMBCommonClassDataSetTableAdapters.TabletSMBCommonClassDataAdapter
    '            mercId = ta.GetMercId(inDlrCode, _
    '                                  inMainteCode, _
    '                                  inVclVin)

    '        End Using
    '        Return mercId
    '    End Function
    '#End Region

    '#Region "商品情報を取得"
    '    ''' <summary>
    '    ''' 商品情報を取得
    '    ''' </summary>
    '    ''' <param name="inMercId">商品ID</param>
    '    ''' <returns>商品情報</returns>
    '    ''' <remarks></remarks>
    '    Public Function GetSvcClassInfo(ByVal inMercId As Long) As TabletSMBCommonClassDataSet.TabletSmbCommonClassMercinfoDataTable
    '        Dim dtMercInfo As TabletSMBCommonClassDataSet.TabletSmbCommonClassMercinfoDataTable
    '        Using ta As New TabletSMBCommonClassDataSetTableAdapters.TabletSMBCommonClassDataAdapter
    '            dtMercInfo = ta.GetServiceClassId(inMercId)
    '        End Using
    '        Return dtMercInfo
    '    End Function
    '#End Region
    '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

#Region "サービスステータス更新判定"
    ''' <summary>
    ''' サービスステータス更新判定
    ''' </summary>
    ''' <param name="insvcStatus">サービスステータス</param>
    ''' <returns>商品情報</returns>
    ''' <remarks></remarks>
    Private Function CheckSvcStatusUpdate(ByVal insvcStatus As String) As Boolean
        Dim updateFlg As Boolean = False
        If SvcStatusCarWashWait.Equals(insvcStatus) _
            OrElse SvcStatusInspectionWait.Equals(insvcStatus) _
            OrElse SvcStatusDropOffCustomer.Equals(insvcStatus) _
            OrElse SvcStatusWaitingCustomer.Equals(insvcStatus) _
            OrElse SvcStatusCanel.Equals(insvcStatus) Then
            updateFlg = True
        End If
        Return updateFlg
    End Function
#End Region

#Region "共通関数"
#Region "秒を切り捨てた処理"
    ''' <summary>
    ''' 秒を切り捨てた日時を取得する.
    ''' </summary>
    ''' <param name="target">対象日時</param>
    ''' <returns>秒を切り捨てた日時</returns>
    Private Function GetDateTimeFloorSecond(ByVal target As DateTime) As DateTime
        Return New DateTime(target.Year, target.Month, target.Day, target.Hour, target.Minute, 0)
    End Function
#End Region

#Region "2つテーブルを合わせて、1つにする"
    ''' <summary>
    ''' 2つテーブルを合わせて、1つにする
    ''' </summary>
    ''' <param name="dt1">テーブル1</param>
    ''' <param name="dt2">テーブル2</param>
    ''' <returns>テーブル1 + テーブル2</returns>
    Private Function CombineTheSameDatatable(ByVal dt1 As DataTable, ByVal dt2 As DataTable) As DataTable
        If dt1.Rows.Count = 0 Then
            Return dt2
        ElseIf dt2.Rows.Count = 0 Then
            Return dt1
        End If
        Dim dtReturn As DataTable = dt1.Copy()
        dtReturn.Merge(dt2)
        Return dtReturn
    End Function
#End Region
#End Region

#Region "履歴登録"
    ''' <summary>
    ''' チップ操作履歴を作成します
    ''' </summary>
    ''' <param name="dtServiceinBefore">チップ情報（変更前）</param>
    ''' <param name="dtServiceinAfter">チップ情報（変更後）</param>
    ''' <param name="inPresentTime">現在時間</param>
    ''' <param name="inAccount">スタッフコード</param>
    ''' <param name="inActId">活動ID</param>
    ''' <param name="inSystem">プログラムID</param>
    ''' <returns>returnCode: 0 成功、1 履歴を残る必要がない。以外はエラー</returns>
    ''' <remarks></remarks>
    Public Function CreateChipOperationHistory(ByVal dtServiceinBefore As TabletSMBCommonClassDataSet.TabletSmbCommonClassServiceinChangeInfoDataTable, _
                                                      ByVal dtServiceinAfter As TabletSMBCommonClassDataSet.TabletSmbCommonClassServiceinChangeInfoDataTable, _
                                                      ByVal inPresentTime As Date, _
                                                      ByVal inAccount As String, _
                                                      ByVal inActId As Decimal, _
                                                      ByVal inSystem As String) As Long
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START " _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name))
        Dim returnCode As Long = ActionResult.Success
        Using biz As New SMBCommonClassBusinessLogic
            If dtServiceinBefore.Rows.Count = 0 AndAlso dtServiceinAfter.Rows.Count = 0 Then
                Throw New ArgumentException
            End If

            If String.IsNullOrEmpty(inAccount) Then
                Throw New ArgumentNullException
            End If
            '新規で予約が作成された場合
            If dtServiceinBefore.Rows.Count = 0 AndAlso dtServiceinAfter.Rows.Count <> 0 Then
                '同じサービス入庫IDの全ての作業内容（変更前後が共にキャンセルされているチップを除く）を履歴を作成する。
                returnCode = biz.RegisterStallReserveHis(dtServiceinAfter(0).DLR_CD, _
                                                         dtServiceinAfter(0).BRN_CD, _
                                                         dtServiceinAfter(0).SVCIN_ID, _
                                                         inPresentTime, _
                                                         RegisterType.RegisterServiceIn, _
                                                         inAccount, _
                                                         inSystem, _
                                                         inActId)
                Return returnCode
            End If

            If IsChangedServicein(dtServiceinBefore, dtServiceinAfter) Then
                '同じサービス入庫IDの全ての作業内容（変更前後が共にキャンセルされているチップを除く）を履歴を作成する。
                returnCode = biz.RegisterStallReserveHis(dtServiceinAfter(0).DLR_CD, _
                                                         dtServiceinAfter(0).BRN_CD, _
                                                         dtServiceinAfter(0).SVCIN_ID, _
                                                         inPresentTime, _
                                                         RegisterType.RegisterServiceIn, _
                                                         inAccount, _
                                                         inSystem, _
                                                         inActId)
                Return returnCode
            End If
            Dim jobDtlIdBeforeList As List(Of Decimal) = _
            (From col In dtServiceinBefore Select col.JOB_DTL_ID).ToList()
            Dim temJobDtlId As Decimal
            For Each serviceinAfter As TabletSMBCommonClassDataSet.TabletSmbCommonClassServiceinChangeInfoRow In dtServiceinAfter
                If serviceinAfter.JOB_DTL_ID = temJobDtlId Then
                    Continue For
                Else
                    temJobDtlId = serviceinAfter.JOB_DTL_ID
                End If
                'リレーションチップ作成の場合
                If Not jobDtlIdBeforeList.Contains(serviceinAfter.JOB_DTL_ID) Then
                    '作業内容のMAXストール利用IDの履歴を作成する
                    returnCode = biz.RegisterStallReserveHis(dtServiceinAfter(0).DLR_CD, _
                                                             dtServiceinAfter(0).BRN_CD, _
                                                            dtServiceinAfter(0).STALL_USE_ID, _
                                                             inPresentTime, _
                                                             RegisterType.RegisterStallUse, _
                                                             inAccount, _
                                                             inSystem, _
                                                             inActId)
                    Return returnCode
                Else
                    If IsChangedJobDtlOrStallUse(dtServiceinBefore, _
                                                 dtServiceinAfter, _
                                                 serviceinAfter.JOB_DTL_ID) Then
                        '作業内容のMAXストール利用IDの履歴を作成する
                        returnCode = biz.RegisterStallReserveHis(dtServiceinAfter(0).DLR_CD, _
                                         dtServiceinAfter(0).BRN_CD, _
                                         dtServiceinAfter(0).STALL_USE_ID, _
                                         inPresentTime, _
                                         RegisterType.RegisterStallUse, _
                                         inAccount, _
                                         inSystem, _
                                         inActId)
                        Return returnCode
                    End If
                End If
            Next
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END RETURNCODE:{2} " _
                        , Me.GetType.ToString _
                        , MethodBase.GetCurrentMethod.Name _
                        , returnCode.ToString(CultureInfo.CurrentCulture)))
            Return returnCode
        End Using
    End Function

    ''' <summary>
    ''' サービス入庫ID単位で変更判断する
    ''' </summary>
    ''' <param name="dtServiceinBefore">変更前のデータセット</param>
    ''' <param name="dtServiceinAfter">変更後のデータセット</param>
    ''' <returns>true：変化あり　flase：変化なし</returns>
    ''' <remarks></remarks>
    Private Function IsChangedServicein(ByVal dtServiceinBefore As TabletSMBCommonClassDataSet.TabletSmbCommonClassServiceinChangeInfoDataTable, _
                                                      ByVal dtServiceinAfter As TabletSMBCommonClassDataSet.TabletSmbCommonClassServiceinChangeInfoDataTable) As Boolean

        'サービス入庫に変更があった場合
        If Not dtServiceinBefore(0).PICK_DELI_TYPE.Equals(dtServiceinAfter(0).PICK_DELI_TYPE) Then
            Return True
        End If
        If Not dtServiceinBefore(0).CARWASH_NEED_FLG.Equals(dtServiceinAfter(0).CARWASH_NEED_FLG) Then
            Return True
        End If
        If Not dtServiceinBefore(0).RESV_STATUS.Equals(dtServiceinAfter(0).RESV_STATUS) Then
            Return True
        End If
        If Not dtServiceinBefore(0).SVC_STATUS.Equals(dtServiceinAfter(0).SVC_STATUS) Then
            Return True
        End If
        If Not dtServiceinBefore(0).SCHE_SVCIN_DATETIME.Equals(dtServiceinAfter(0).SCHE_SVCIN_DATETIME) Then
            Return True
        End If
        If Not dtServiceinBefore(0).SCHE_DELI_DATETIME.Equals(dtServiceinAfter(0).SCHE_DELI_DATETIME) Then
            Return True
        End If
        If Not dtServiceinBefore(0).RSLT_SVCIN_DATETIME.Equals(dtServiceinAfter(0).RSLT_SVCIN_DATETIME) Then
            Return True
        End If

        '車両引取に変更があったか否かを判定します
        If Not (dtServiceinBefore(0).IsPICK_PREF_DATETIMENull AndAlso dtServiceinAfter(0).IsPICK_PREF_DATETIMENull) Then
            If Not dtServiceinBefore(0).IsPICK_PREF_DATETIMENull AndAlso dtServiceinAfter(0).IsPICK_PREF_DATETIMENull Then
                Return True
            End If
            If dtServiceinBefore(0).IsPICK_PREF_DATETIMENull AndAlso Not dtServiceinAfter(0).IsPICK_PREF_DATETIMENull Then
                Return True
            End If
            If Not dtServiceinBefore(0).PICK_PREF_DATETIME.Equals(dtServiceinAfter(0).PICK_PREF_DATETIME) Then
                Return True
            End If
        End If
        '車両配送に変更があったか否かを判定します
        If Not (dtServiceinBefore(0).IsDELI_PREF_DATETIMENull AndAlso dtServiceinAfter(0).IsDELI_PREF_DATETIMENull) Then
            If Not dtServiceinBefore(0).IsDELI_PREF_DATETIMENull And dtServiceinAfter(0).IsDELI_PREF_DATETIMENull Then
                Return True
            End If

            If dtServiceinBefore(0).IsDELI_PREF_DATETIMENull And Not dtServiceinAfter(0).IsDELI_PREF_DATETIMENull Then
                Return True
            End If

            If Not dtServiceinBefore(0).DELI_PREF_DATETIME.Equals(dtServiceinAfter(0).DELI_PREF_DATETIME) Then
                Return True
            End If
        End If
        Return False
    End Function

    ''' <summary>
    ''' 作業内容またはストール利用に変更判定
    ''' </summary>
    ''' <param name="dtServiceinBefore">変更前のデータセット</param>
    ''' <param name="dtServiceinAfter">変更後のデータセット</param>
    ''' <returns>true：変化あり　flase：変化なし</returns>
    ''' <remarks></remarks>
    Private Function IsChangedJobDtlOrStallUse(ByVal dtServiceinBefore As TabletSMBCommonClassDataSet.TabletSmbCommonClassServiceinChangeInfoDataTable, _
                                                      ByVal dtServiceinAfter As TabletSMBCommonClassDataSet.TabletSmbCommonClassServiceinChangeInfoDataTable, _
                                                      ByVal jobDtlIdAfter As Decimal) As Boolean

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. jobDtlIdAfter={1}" _
                    , MethodBase.GetCurrentMethod.Name, jobDtlIdAfter))
        Dim rowServiceBeforeInfo As TabletSMBCommonClassDataSet.TabletSmbCommonClassServiceinChangeInfoRow() = _
              (From col In dtServiceinBefore Where col.JOB_DTL_ID = jobDtlIdAfter Select col).ToArray
        Dim rowServiceAfterInfo As TabletSMBCommonClassDataSet.TabletSmbCommonClassServiceinChangeInfoRow() = _
              (From col In dtServiceinAfter Where col.JOB_DTL_ID = jobDtlIdAfter Select col).ToArray
        '作業開始待ちでない場合は操作履歴を作成しない
        If Not rowServiceAfterInfo(0).STALL_USE_STATUS.Equals(StalluseStatusWorkOrderWait) AndAlso _
            Not rowServiceAfterInfo(0).STALL_USE_STATUS.Equals(StalluseStatusStartWait) AndAlso _
            Not rowServiceAfterInfo(0).STALL_USE_STATUS.Equals(StalluseStatusNoshow) Then
            Return False
        End If

        '前後共にキャンセルされているチップを除く
        If rowServiceBeforeInfo(0).CANCEL_FLG.Equals(CancelFlgCancel) AndAlso rowServiceAfterInfo(0).CANCEL_FLG.Equals(CancelFlgCancel) Then
            Return False
        End If

        '作業内容に変更があった場合（※対象カラムのみ）
        If rowServiceBeforeInfo(0).SVC_CLASS_ID <> rowServiceAfterInfo(0).SVC_CLASS_ID Then
            Return True
        End If
        If rowServiceBeforeInfo(0).MERC_ID <> rowServiceAfterInfo(0).MERC_ID Then
            Return True
        End If
        If Not rowServiceBeforeInfo(0).INSPECTION_NEED_FLG.Equals(rowServiceAfterInfo(0).INSPECTION_NEED_FLG) Then
            Return True
        End If
        If Not rowServiceBeforeInfo(0).CANCEL_FLG.Equals(rowServiceAfterInfo(0).CANCEL_FLG) Then
            Return True
        End If

        'ストール利用に変更があった場合（※対象カラムのみ）
        If rowServiceBeforeInfo(0).STALL_USE_ID <> rowServiceAfterInfo(0).STALL_USE_ID Then
            Return True
        End If
        If rowServiceBeforeInfo(0).STALL_ID <> rowServiceAfterInfo(0).STALL_ID Then
            Return True
        End If
        If Not rowServiceBeforeInfo(0).TEMP_FLG.Equals(rowServiceAfterInfo(0).TEMP_FLG) Then
            Return True
        End If
        If Not rowServiceBeforeInfo(0).SCHE_START_DATETIME.Equals(rowServiceAfterInfo(0).SCHE_START_DATETIME) Then
            Return True
        End If
        If Not rowServiceBeforeInfo(0).SCHE_END_DATETIME.Equals(rowServiceAfterInfo(0).SCHE_END_DATETIME) Then
            Return True
        End If
        If rowServiceBeforeInfo(0).SCHE_WORKTIME <> rowServiceAfterInfo(0).SCHE_WORKTIME Then
            Return True
        End If

        Return False
    End Function

#End Region

#Region "ロック処理"
#Region "サービス入庫テーブルロック処理"
    ''' <summary>
    ''' サービス入庫テーブルロック処理
    ''' </summary>
    ''' <param name="inServiceInId">サービス入庫ID</param>
    ''' <param name="inRowLockVersion">サービス入庫テーブルの行ロックバージョン</param>
    ''' <param name="inAccount">アカウント</param>
    ''' <param name="inNowDate">現在日時</param>
    ''' <param name="inSystem">プログラムID</param>
    ''' <remarks></remarks>
    Public Function LockServiceInTable(ByVal inServiceInId As Decimal, _
                                       ByVal inRowLockVersion As Long, _
                                       ByVal inAccount As String, _
                                       ByVal inNowDate As Date, _
                                       ByVal inSystem As String) As Long

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. inServiceInId={1}, inRowLockVersion={2}, inAccount={3}, inNowDate={4}, inSystem={5}" _
        , MethodBase.GetCurrentMethod.Name, inServiceInId, inRowLockVersion, inAccount, inNowDate, inSystem))

        Using commBiz As New SMBCommonClassBusinessLogic
            'サービス入庫テーブルロック処理
            Dim result As Long = commBiz.LockServiceInTable(inServiceInId, inRowLockVersion, "0", inAccount, inNowDate, inSystem)

            Select Case result
                Case ActionResult.Success
                    Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
                    Return ActionResult.Success
                Case ReturnCode.ErrorDBConcurrency
                    Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0}.E ErrorDBConcurrency ", MethodBase.GetCurrentMethod.Name))
                    '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
                    'Return ActionResult.LockStallError
                    Return ActionResult.RowLockVersionError
                    '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END
                Case ReturnCode.ErrorNoDataFound
                    Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0}.E NoDataFound ", MethodBase.GetCurrentMethod.Name))
                    Return ActionResult.NoDataFound
                Case ReturnCode.ErrDBTimeout
                    Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0}.E DBTimeOutError ", MethodBase.GetCurrentMethod.Name))
                    Return ActionResult.DBTimeOutError
                Case Else
                    Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0}.E result={1} ", MethodBase.GetCurrentMethod.Name, result))
                    Return ActionResult.ExceptionError
            End Select
        End Using
    End Function
#End Region

#Region "ストールロック処理"
    ''' <summary>
    ''' ストールロック処理
    ''' </summary>
    ''' <param name="stallId">サービス入庫ID</param>
    ''' <param name="lockDate">サービス入庫テーブルの行ロックバージョン</param>
    ''' <param name="staffCode">アカウント</param>
    ''' <param name="updateDate">現在日時</param>
    ''' <param name="systemId">プログラムID</param>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Function LockStall(ByVal stallId As Decimal, _
                                    ByVal lockDate As Date, _
                                    ByVal staffCode As String, _
                                    ByVal updateDate As Date, _
                                    ByVal systemId As String) As Long

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. stallId={1}, lockDate={2}, staffCode={3}, updateDate={4}, systemId={5}" _
                    , MethodBase.GetCurrentMethod.Name, stallId, lockDate, staffCode, updateDate, systemId))

        Using commBiz As New SMBCommonClassBusinessLogic
            Dim result As Long = commBiz.RegisterStallLock(stallId, lockDate, staffCode, updateDate, systemId)
            If result = 901 Then
                Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0}.E DBTimeOutError ", MethodBase.GetCurrentMethod.Name))
                Return ActionResult.DBTimeOutError
            ElseIf result = 904 Then
                Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0}.E LockStallError ", MethodBase.GetCurrentMethod.Name))
                Return ActionResult.LockStallError
            ElseIf result = ActionResult.Success Then
                Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
                Return ActionResult.Success
            Else
                Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0}.E result={1} ", MethodBase.GetCurrentMethod.Name, result))
                Return ActionResult.ExceptionError
            End If
        End Using
    End Function

    ''' <summary>
    ''' ストールロック解除処理
    ''' </summary>
    ''' <param name="stallId">サービス入庫ID</param>
    ''' <param name="lockDate">サービス入庫テーブルの行ロックバージョン</param>
    ''' <param name="staffCode">アカウント</param>
    ''' <param name="updateDate">現在日時</param>
    ''' <param name="systemId">プログラムID</param>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Function LockStallReset(ByVal stallId As Decimal, _
                                    ByVal lockDate As Date, _
                                    ByVal staffCode As String, _
                                    ByVal updateDate As Date, _
                                    ByVal systemId As String) As Long

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. stallId={1}, lockDate={2}, staffCode={3}, updateDate={4}, systemId={5}" _
                    , MethodBase.GetCurrentMethod.Name, stallId, lockDate, staffCode, updateDate, systemId))

        Using commBiz As New SMBCommonClassBusinessLogic
            Dim result As Long = commBiz.DeleteStallLock(stallId, lockDate, staffCode, updateDate, systemId)
            If result = 901 Then
                Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0}.E DBTimeOutError ", MethodBase.GetCurrentMethod.Name))
                Return ActionResult.DBTimeOutError
            ElseIf result = ActionResult.Success Then
                Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
                Return ActionResult.Success
            Else
                Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0}.E result={1} ", MethodBase.GetCurrentMethod.Name, result))
                Return ActionResult.ExceptionError
            End If
        End Using
    End Function
#End Region

#End Region

#Region "Push通知の送信"

    ''' <summary>
    ''' Push送信準備
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="storeCode">店舗コード</param>
    ''' <param name="operationCodeList">権限コードリスト</param>
    ''' <param name="functionName">呼び出しJS関数名(括弧付き)</param>
    ''' <param name="stallIdList">ストールIDのリスト</param>
    ''' <remarks>
    ''' ●ストールIDのリストを元にテクニシャンアカウントをスタッフストール割当テーブル
    ''' 　から取得し、該当ストールに所属しているテクニシャンのみPush送信を行う。
    ''' ●ストールIDがNothingの場合は該当する権限のオンラインユーザー全てにPush送信をする
    ''' </remarks>
    Public Sub SendPushGetReady(ByVal dealerCode As String, _
                                ByVal storeCode As String, _
                                ByVal operationCodeList As List(Of Decimal), _
                                ByVal functionName As String, _
                                ByVal stallIdList As List(Of Decimal))

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.S. dealerCode={1}, storeCode={2}, functionName={3}", _
                                  MethodBase.GetCurrentMethod.Name, dealerCode, storeCode, functionName))

        If IsNothing(stallIdList) Then

            'オンラインユーザー情報の取得
            Dim utility As New VisitUtilityBusinessLogic
            Dim sendPushUsers As VisitUtilityUsersDataTable = _
                utility.GetOnlineUsers(dealerCode, storeCode, operationCodeList)

            utility = Nothing

            'オンラインユーザー分Push送信する
            For Each userRow As VisitUtilityUsersRow In sendPushUsers
                'Push送信処理
                Transmission(userRow.ACCOUNT, functionName)
            Next
        Else

            Dim stallStaffAccount As TabletSmbCommonClassStringValueDataTable

            Using ta As New TabletSMBCommonClassDataAdapter
                '該当ストール所属テクニシャンを取得する
                stallStaffAccount = ta.GetStaffCodeByStallIdForTC(dealerCode, storeCode, stallIdList)
            End Using

            '該当ストール所属テクニシャン分Push送信する
            For Each stallStaffRow In stallStaffAccount
                'Push送信処理
                Transmission(stallStaffRow.COL1, functionName)
            Next

        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' Push送信する
    ''' </summary>
    ''' <param name="staffCode">スタッフコード</param>
    ''' <param name="functionName">呼び出しJS関数名</param>
    ''' <remarks></remarks>
    Private Sub Transmission(ByVal staffCode As String, ByVal functionName As String)

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.S. staffCode={1}, functionName={2}", _
                                  MethodBase.GetCurrentMethod.Name, staffCode, functionName))

        'POST送信メッセージの作成
        Dim postSendMessage As New StringBuilder
        postSendMessage.Append("cat=action")
        postSendMessage.Append("&type=main")
        postSendMessage.Append("&sub=js")
        postSendMessage.Append("&uid=" & staffCode)
        postSendMessage.Append("&time=0")
        postSendMessage.Append("&js1=" & functionName)

        '送信処理
        Dim visitUtility As New VisitUtility
        visitUtility.SendPush(postSendMessage.ToString)

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))

    End Sub

    '2015/07/17 TMEJ 明瀬 タブレットSMB性能改善 通知WebService化 START

    ''' <summary>
    ''' Push送信する
    ''' </summary>
    ''' <param name="staffCode">スタッフコード</param>
    ''' <param name="functionName">呼び出しJS関数名</param>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <remarks></remarks>
    Private Sub Transmission(ByVal staffCode As String, _
                             ByVal functionName As String, _
                             ByVal inDealerCode As String)

        Logger.Info(String.Format( _
                    CultureInfo.InvariantCulture, _
                    "{0}.{1} Start [staffCode={2}, functionName={3}, inDealerCode={4}]", _
                    Me.GetType(), _
                    MethodBase.GetCurrentMethod.Name, _
                    staffCode, _
                    functionName, _
                    inDealerCode))

        'POST送信メッセージの作成
        Dim postSendMessage As New StringBuilder
        postSendMessage.Append("cat=action")
        postSendMessage.Append("&type=main")
        postSendMessage.Append("&sub=js")
        postSendMessage.Append("&uid=" & staffCode)
        postSendMessage.Append("&time=0")
        postSendMessage.Append("&js1=" & functionName)

        '送信処理
        Dim visitUtility As New VisitUtility
        visitUtility.SendPush(postSendMessage.ToString, _
                              inDealerCode)

        Logger.Info(String.Format( _
                    CultureInfo.InvariantCulture, _
                    "{0}.{1} End", _
                    Me.GetType(), _
                    MethodBase.GetCurrentMethod.Name))

    End Sub

    '2015/07/17 TMEJ 明瀬 タブレットSMB性能改善 通知WebService化 END

    '2014/01/17 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START

    ''' <summary>
    ''' TCスタッフコードの取得
    ''' </summary>
    ''' <param name="dlrCD">販売店コード</param>
    ''' <param name="strCD">店舗コード</param>
    ''' <param name="stallIdList">ストールIDリスト</param>
    ''' <returns>該当ストールに所属するTCスタッフコードを取得</returns>
    '''<remarks></remarks>
    Public Function GetSendStaffCodeTC(ByVal dlrCD As String, _
                                ByVal strCD As String, _
                                ByVal stallIdList As List(Of Decimal)) As List(Of String)

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                          "{0}.S. dlrCD={1} strCD={2}", _
                          MethodBase.GetCurrentMethod.Name, dlrCD, strCD))

        Dim sendStaffAccountData As TabletSmbCommonClassStringValueDataTable
        Dim sendStaffCodeList As New List(Of String)

        Using ta As New TabletSMBCommonClassDataAdapter
            '該当ストール所属のTCテクニシャンを取得する
            sendStaffAccountData = ta.GetStaffCodeByStallIdForTC(dlrCD, strCD, stallIdList)
        End Using

        For Each sendStaffAccountRow In sendStaffAccountData
            sendStaffCodeList.Add(sendStaffAccountRow.COL1)
        Next

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))

        Return sendStaffCodeList

    End Function

    ''' <summary>
    ''' ChTスタッフコードの取得
    ''' </summary>
    ''' <param name="dlrCD">販売店コード</param>
    ''' <param name="strCD">店舗コード</param>
    ''' <param name="stallIdList">ストールIDリスト</param>
    ''' <returns>該当ストールに所属するChTスタッフコードを取得</returns>
    '''<remarks></remarks>
    Public Function GetSendStaffCodeCht(ByVal dlrCD As String, _
                                ByVal strCD As String, _
                                ByVal stallIdList As List(Of Decimal)) As List(Of String)

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                          "{0}.S. dlrCD={1} strCD={2}", _
                          MethodBase.GetCurrentMethod.Name, dlrCD, strCD))

        Dim sendStaffAccountData As TabletSmbCommonClassStringValueDataTable
        Dim sendStaffCodeList As New List(Of String)

        Using ta As New TabletSMBCommonClassDataAdapter
            '該当ストール所属のChTテクニシャンを取得する
            sendStaffAccountData = ta.GetStaffCodeByStallIdForCht(dlrCD, strCD, stallIdList)
        End Using

        For Each sendStaffAccountRow In sendStaffAccountData
            sendStaffCodeList.Add(sendStaffAccountRow.COL1)
        Next

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))

        Return sendStaffCodeList

    End Function

    ''' <summary>
    ''' 権限コードリストに指定した全スタッフコードの取得
    ''' </summary>
    ''' <param name="dlrCD">販売店コード</param>
    ''' <param name="strCD">店舗コード</param>
    ''' <param name="operationCodeList">権限コードリスト</param>
    ''' <param name="exceptStaffCodeList">除外するスタッフコードリスト</param>
    ''' <returns>権限コードリストに指定した全スタッフコードを取得</returns>
    '''<remarks></remarks>
    Public Function GetSendStaffCode(ByVal dlrCD As String, _
                                ByVal strCD As String, _
                                ByVal operationCodeList As List(Of Decimal), _
                                Optional ByVal exceptStaffCodeList As List(Of String) = Nothing) As List(Of String)

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.S. dlrCD={1} strCD={2}", _
                                  MethodBase.GetCurrentMethod.Name, dlrCD, strCD))

        Dim sendStaffCodeList As New List(Of String)
        Dim exceptStaffFlg As Boolean = False

        'オンラインユーザー情報の取得
        Dim utility As New VisitUtilityBusinessLogic
        Dim sendStaffAccountData As VisitUtilityUsersDataTable = utility.GetOnlineUsers(dlrCD, strCD, operationCodeList)

        utility = Nothing

        For Each sendStaffAccountRow In sendStaffAccountData
            If IsNothing(exceptStaffCodeList) Then
                sendStaffCodeList.Add(sendStaffAccountRow.ACCOUNT)
            Else
                exceptStaffFlg = False
                For Each exceptStaffCodeRow In exceptStaffCodeList
                    If exceptStaffCodeRow.Equals(sendStaffAccountRow.ACCOUNT) Then
                        '除外スタッフコードリストに該当
                        exceptStaffFlg = True
                        Exit For
                    End If
                Next
                If exceptStaffFlg = False Then
                    'スタッフコードを追加
                    sendStaffCodeList.Add(sendStaffAccountRow.ACCOUNT)
                End If
            End If
        Next

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))

        Return sendStaffCodeList

    End Function

    ''' <summary>
    ''' Push送信処理
    ''' </summary>
    ''' <param name="sendStaffCodeList">スタッフコードリスト</param>
    ''' <param name="functionName">呼び出しJS関数名</param>
    ''' <remarks></remarks>
    Public Sub SendPushByStaffCodeList(ByVal sendStaffCodeList As List(Of String), ByVal functionName As String)

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.S. functionName={1}", _
                                  MethodBase.GetCurrentMethod.Name, functionName))

        'スタッフコードリスト分Push送信する
        For Each staffCode In sendStaffCodeList

            'Push送信処理
            Transmission(staffCode, functionName)
        Next

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))

    End Sub
    '2014/01/17 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

    '2015/07/17 TMEJ 明瀬 タブレットSMB性能改善 通知WebService化 START

    ''' <summary>
    ''' Push送信処理
    ''' </summary>
    ''' <param name="sendStaffCodeList">スタッフコードリスト</param>
    ''' <param name="functionName">呼び出しJS関数名</param>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <remarks></remarks>
    Public Sub SendPushByStaffCodeList(ByVal sendStaffCodeList As List(Of String), _
                                       ByVal functionName As String, _
                                       ByVal inDealerCode As String)

        Logger.Info(String.Format( _
                    CultureInfo.InvariantCulture, _
                    "{0}.{1} Start [functionName={2}, inDealerCode={3}]", _
                    Me.GetType(), _
                    MethodBase.GetCurrentMethod.Name, _
                    functionName, _
                    inDealerCode))

        'スタッフコードリスト分Push送信する
        For Each staffCode In sendStaffCodeList

            'Push送信処理
            Me.Transmission(staffCode, _
                            functionName, _
                            inDealerCode)

        Next

        Logger.Info(String.Format( _
                    CultureInfo.InvariantCulture, _
                    "{0}.{1} End", _
                    Me.GetType(), _
                    MethodBase.GetCurrentMethod.Name))

    End Sub

    '2015/07/17 TMEJ 明瀬 タブレットSMB性能改善 通知WebService化 END

    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START

#Region "各操作により、送信する"

    ''' <summary>
    ''' ストールチップ削除通知を出す
    ''' </summary>
    ''' <param name="stallUseId">ストール利用ID</param>
    ''' <param name="objStaffContext">スタッフ情報</param>
    ''' <remarks></remarks>
    Public Sub SendNoticeByDeleteStallChip(ByVal stallUseId As Decimal, _
                                           ByVal objStaffContext As StaffContext)
        Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0}.{1}.S. " _
                                , Me.GetType.ToString, MethodBase.GetCurrentMethod.Name))

        '対象の情報を取得する
        Dim dtChipEntity As TabletSmbCommonClassChipEntityDataTable = GetChipEntity(stallUseId, 1)
        If dtChipEntity.Count <> 1 _
            OrElse IsNothing(TabletSmbCommonCancelInstructedChipInfo) _
            OrElse TabletSmbCommonCancelInstructedChipInfo.Count = 0 Then
            Return
        End If

        Dim dmsDlrBrnTable As ServiceCommonClassDataSet.DmsCodeMapRow
        '基幹販売店・店舗コード
        dmsDlrBrnTable = Me.GetDmsDlrBrnCode(objStaffContext.DlrCD, objStaffContext.BrnCD, objStaffContext.Account)
        If IsNothing(dmsDlrBrnTable) Then
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E.GetDmsDlrBrnCode ExceptionError " _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return
        End If

        For Each drJobDtlId As TabletSmbCommonClassCanceledJobInfoRow In TabletSmbCommonCancelInstructedChipInfo
            '通知
            '情報取得
            Dim dtNoticeInfo As TabletSmbCommonClassNoticeInfoDataTable

            Using ta As New TabletSMBCommonClassDataAdapter
                dtNoticeInfo = ta.GetNoticeInfo(dtChipEntity(0).SVCIN_ID, _
                                                objStaffContext.DlrCD, _
                                                objStaffContext.BrnCD, _
                                                drJobDtlId.JOB_DTL_ID)
            End Using

            If dtNoticeInfo.Count > 0 Then
                Dim drNoticeInfo As TabletSmbCommonClassNoticeInfoRow = dtNoticeInfo(0)

                '通知処理
                Me.SendCancelJobInstructNoticeApi(drNoticeInfo, _
                                                  objStaffContext, _
                                                  drJobDtlId.SCHE_START_DATETIME, _
                                                  drJobDtlId.SCHE_END_DATETIME, _
                                                  drJobDtlId.STALL_ID, _
                                                  dmsDlrBrnTable)
            End If

        Next

        '着工指示キャンセルの時、push送信
        'TCにPUSH送信する(着工指示整備解除)
        Me.SendPushByCancel(TabletSmbCommonCancelInstructedChipInfo, objStaffContext)

        '全てCHTにPUSH送信(自分以外)
        Me.SendAllChtPush(objStaffContext.DlrCD, objStaffContext.BrnCD, objStaffContext.Account)
        '全てCTにPUSH送信(自分以外)
        Me.SendAllCTPush(objStaffContext.DlrCD, objStaffContext.BrnCD, objStaffContext.Account)

        '全てPSにPUSH送信
        Me.SendAllPSPush(objStaffContext.DlrCD, objStaffContext.BrnCD)

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E. ", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' NoShowの通知を出す
    ''' </summary>
    ''' <param name="objStaffContext">スタッフ情報</param>
    ''' <remarks></remarks>
    Public Sub SendNoticeByNoShow(ByVal objStaffContext As StaffContext)
        'Push
        Dim pushUsersList As New List(Of String)
        Dim operationCodeList As New List(Of Decimal)
        Dim exceptStaffCodeList As New List(Of String)
        'SVR権限を追加
        operationCodeList.Add(Operation.SVR)
        'SVR権限のユーザーを取得
        pushUsersList = Me.GetSendStaffCode(objStaffContext.DlrCD, objStaffContext.BrnCD, operationCodeList, exceptStaffCodeList)
        'ユーザーリストに対してPUSHする
        SendPushByStaffCodeList(pushUsersList, PUSH_FuntionSVR)
    End Sub

    ''' <summary>
    ''' 中断のPushを出す
    ''' </summary>
    ''' <param name="objStaffContext">スタッフ情報</param>
    ''' <param name="stallId">ストールID</param>
    ''' <remarks></remarks>
    Public Sub SendNoticeByJobStop(ByVal objStaffContext As StaffContext, _
                                   ByVal stallId As Decimal)

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. ", MethodBase.GetCurrentMethod.Name))

        '指定ストールのTCにPush送信
        Me.SendNamedTCPush(objStaffContext.DlrCD, objStaffContext.BrnCD, stallId)
        '全てCTにPush送信(自分以外)
        Me.SendAllCTPush(objStaffContext.DlrCD, objStaffContext.BrnCD, objStaffContext.Account)
        '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 START
        ''指定ストールのChTにPush送信(自分以外)
        'Dim stallList As New List(Of Decimal)
        'stallList.Add(stallId)
        'Me.SendNamedChtPush(objStaffContext.DlrCD, objStaffContext.BrnCD, objStaffContext.Account, stallList)
        'すべてのChTにPush送信(自分以外)
        Me.SendAllChtPush(objStaffContext.DlrCD, objStaffContext.BrnCD, objStaffContext.Account)
        '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 END

        Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0}.E ", MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' 開始の通知を出す
    ''' </summary>
    ''' <param name="objStaffContext">スタッフ情報</param>
    ''' <param name="inSvcinId">サービス入庫ID</param>
    ''' <param name="inStallId">ストールID</param>
    ''' <param name="inIsFirstStartchipFlg">最初開始チップフラグ</param>
    ''' <remarks></remarks>
    Public Sub SendNoticeByStart(ByVal objStaffContext As StaffContext, _
                                 ByVal inSvcinId As Decimal, _
                                 ByVal inStallId As Decimal, _
                                 ByVal inIsFirstStartChipFlg As Boolean)

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.S. inSvcinId={1}, inStallId={2}, inIsFirstStartChipFlg={3}", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  inSvcinId, _
                                  inStallId, _
                                  inIsFirstStartChipFlg))

        '指定ストールのTCにPush送信
        Me.SendNamedTCPush(objStaffContext.DlrCD, objStaffContext.BrnCD, inStallId)
        '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 START
        'CT/CHTへのPush処理(ストールに紐づくユーザーのみ)
        SendPushCtChtToStall(objStaffContext.DlrCD, objStaffContext.BrnCD, objStaffContext.Account, inStallId)

        ''全てCTにPush送信(自分以外)
        'Me.SendAllCTPush(objStaffContext.DlrCD, objStaffContext.BrnCD, objStaffContext.Account)
        ''指定ストールのChTにPush送信(自分以外)
        'Dim stallList As New List(Of Decimal)
        'stallList.Add(inStallId)
        'Me.SendNamedChtPush(objStaffContext.DlrCD, objStaffContext.BrnCD, objStaffContext.Account, stallList)
        '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 END

        '全てPSにPush送信
        Me.SendAllPSPush(objStaffContext.DlrCD, objStaffContext.BrnCD)
        '最初の作業の場合、SAにPush送信
        If inIsFirstStartChipFlg Then
            Me.SendNamedSAPush(inSvcinId, objStaffContext.DlrCD, objStaffContext.BrnCD, objStaffContext.Account)
        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' 作業中チップのUndo通知を出す
    ''' </summary>
    ''' <param name="objStaffContext">スタッフ情報</param>
    ''' <param name="inStallId">ストールID</param>
    ''' <remarks></remarks>
    Public Sub SendNoticeByUndoWorkingChip(ByVal objStaffContext As StaffContext, _
                                           ByVal inStallId As Decimal)

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S.", System.Reflection.MethodBase.GetCurrentMethod.Name))

        '指定ストールのTCにPush送信
        Me.SendNamedTCPush(objStaffContext.DlrCD, objStaffContext.BrnCD, inStallId)

        '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 START
        'CT/CHTへのPush処理(ストールに紐づくユーザーのみ)
        SendPushCtChtToStall(objStaffContext.DlrCD, objStaffContext.BrnCD, objStaffContext.Account, inStallId)
        ''全てCTにPush送信(自分以外)
        'Me.SendAllCTPush(objStaffContext.DlrCD, objStaffContext.BrnCD, objStaffContext.Account)
        ''指定ストールのChTにPush送信(自分以外)
        'Dim stallList As New List(Of Decimal)
        'stallList.Add(inStallId)
        'Me.SendNamedChtPush(objStaffContext.DlrCD, objStaffContext.BrnCD, objStaffContext.Account, stallList)
        '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 END

        '全てPSにPush送信
        Me.SendAllPSPush(objStaffContext.DlrCD, objStaffContext.BrnCD)

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' 終了の通知を出す
    ''' </summary>
    ''' <param name="staffInfo">スタッフ情報</param>
    ''' <param name="svcInId">サービス入庫ID</param>
    ''' <param name="stallId">ストールID</param>
    ''' <param name="systemId">呼ぶ画面ID</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2014/12/17 TMEJ 小澤 IT9824_NextSTEPサービス 洗車用端末開発向けた評価用アプリ作成
    ''' </history>
    Public Sub SendNoticeByFinish(ByVal staffInfo As StaffContext, _
                                  ByVal svcInId As Decimal, _
                                  ByVal stallId As Decimal, _
                                  ByVal systemId As String)

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. ", MethodBase.GetCurrentMethod.Name))


        Dim expectAccount As String = Nothing
        '工程管理画面から呼ぶ時
        If ProgramId_Main.Equals(systemId) Then
            '送信しない方を設定する(操作者自分)
            expectAccount = staffInfo.Account
        End If

        '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 START
        ' 最終作業かどうかの判定フラグ
        Dim isLastWorkChip As Boolean = False
        '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 END

        Using ta As New TabletSMBCommonClassDataAdapter
            '最終作業の場合、
            If ta.GetBeforeFinishRelationChipCount(staffInfo.DlrCD, staffInfo.BrnCD, svcInId) = 0 Then
                '指定SAにPush送信(自分以外)
                Me.SendNamedSAPush(svcInId, staffInfo.DlrCD, staffInfo.BrnCD, expectAccount)
                '通知を出す
                Me.SendFinishNoticeApi(svcInId, staffInfo, expectAccount)

                '2014/12/17 TMEJ 小澤 IT9824_NextSTEPサービス 洗車用端末開発向けた評価用アプリ作成 START

                'サービス入庫情報を取得
                Dim dtServiceinInfo As TabletSmbCommonClassServiceinInfoDataTable = _
                    ta.GetServiceinInfo(svcInId, _
                                        staffInfo.DlrCD, _
                                        staffInfo.BrnCD)

                'サービス入庫情報とサービスステータスのチェック
                If 0 < dtServiceinInfo.Count AndAlso SvcStatusCarWashWait.Equals(dtServiceinInfo(0).SVC_STATUS) Then
                    '1件以上ある場合且つ、「07：洗車待ち」の場合

                    'CW権限にPUSHする
                    Me.SendAllCWPush(staffInfo.DlrCD, staffInfo.BrnCD, staffInfo.Account)

                End If

                '2014/12/17 TMEJ 小澤 IT9824_NextSTEPサービス 洗車用端末開発向けた評価用アプリ作成 END

                '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 START
                '最終作業の為フラグを立てる
                isLastWorkChip = True
                '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 END

            End If
        End Using

        '工程管理画面から呼ぶ時、TCに送信
        If ProgramId_Main.Equals(systemId) Then
            '指定ストールのTCにPush送信
            Me.SendNamedTCPush(staffInfo.DlrCD, staffInfo.BrnCD, stallId)
        End If

        '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 START
        '================================================================
        '         　　　　　　　CT/ChT通知送信処理
        '================================================================
        If isLastWorkChip Then
            ' 最終作業の場合
            ' すべてのユーザーにPush通知を送信する
            SendPushCtChtAll(staffInfo.DlrCD, staffInfo.BrnCD, staffInfo.Account)
        Else
            ' 最終作業でない場合
            ' 対象ストールに紐づくユーザーのみPush通知を送信する
            SendPushCtChtToStall(staffInfo.DlrCD, staffInfo.BrnCD, staffInfo.Account, stallId)
        End If
        ''全てCTにPush送信
        'Me.SendAllCTPush(staffInfo.DlrCD, staffInfo.BrnCD, expectAccount)
        ''指定ストールのChTにPush送信
        'Dim stallList As New List(Of Decimal)
        'stallList.Add(stallId)
        'Me.SendNamedChtPush(staffInfo.DlrCD, staffInfo.BrnCD, expectAccount, stallList)
        '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 END

        Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0}.E ", MethodBase.GetCurrentMethod.Name))

    End Sub

    '2015/02/13 TMEJ 範 新プラットフォーム版i-CROP仕様変更対応 (SMBチップ検索及び作業中断機能強化) START
    ''' <summary>
    ''' 中断終了操作で通知を出す
    ''' </summary>
    ''' <param name="inStaffInfo">スタッフ情報</param>
    ''' <param name="inSvcInId">サービス入庫ID</param>
    ''' <param name="inStallId">ストールID</param>
    ''' <param name="inSystemId">呼ぶ先プログラムID</param>
    ''' <remarks></remarks>
    Public Sub SendNoticeByStopFinish(ByVal inStaffInfo As StaffContext, _
                                      ByVal inSvcInId As Decimal, _
                                      ByVal inStallId As Decimal, _
                                      ByVal inSystemId As String)

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.{1} Start inSvcInId={2}, inStallId={3}, inSystemId={4} ", _
                                  Me.GetType.ToString, _
                                  MethodBase.GetCurrentMethod.Name, _
                                  inSvcInId, _
                                  inStallId, _
                                  inSystemId))
        '送信しない方を設定する(操作者自分)
        Dim expectAccount As String = inStaffInfo.Account

        '指定SAにPush送信(自分以外)
        Me.SendNamedSAPush(inSvcInId, inStaffInfo.DlrCD, inStaffInfo.BrnCD, expectAccount)
        '通知を出す
        Me.SendFinishNoticeApi(inSvcInId, inStaffInfo, expectAccount)

        Using ta As New TabletSMBCommonClassDataAdapter

            'サービス入庫情報を取得
            Dim dtServiceinInfo As TabletSmbCommonClassServiceinInfoDataTable = ta.GetServiceinInfo(inSvcInId, _
                                                                                                    inStaffInfo.DlrCD, _
                                                                                                    inStaffInfo.BrnCD)
            'サービス入庫情報とサービスステータスのチェック
            If 0 < dtServiceinInfo.Count Then
                '1件以上ある場合
                If SvcStatusCarWashWait.Equals(dtServiceinInfo(0).SVC_STATUS) Then
                    '「07：洗車待ち」の場合

                    'CW権限にPUSHする
                    Me.SendAllCWPush(inStaffInfo.DlrCD, inStaffInfo.BrnCD, inStaffInfo.Account)

                End If

            End If

        End Using

        '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 START
        ''指定ストールのChTにPush送信
        'Dim stallList As New List(Of Decimal)
        'stallList.Add(inStallId)
        'Me.SendNamedChtPush(inStaffInfo.DlrCD, inStaffInfo.BrnCD, expectAccount, stallList)
        '全てのChTにPush送信(自分以外)
        Me.SendAllChtPush(inStaffInfo.DlrCD, inStaffInfo.BrnCD, inStaffInfo.Account)
        '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 END

        '全てCTにPush送信
        Me.SendAllCTPush(inStaffInfo.DlrCD, inStaffInfo.BrnCD, expectAccount)

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.{1} End ", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' 指定サービス入庫ID単位で未完了作業件数の取得
    ''' </summary>
    ''' <param name="inDlrCode">販売店コード</param>
    ''' <param name="inBrnCode">店舗コード</param>
    ''' <param name="inSvcInIdList">サービス入庫ID</param>
    ''' <remarks></remarks>
    Public Function GetNotFinishedChipCount(ByVal inDlrCode As String, _
                                            ByVal inBrnCode As String, _
                                            ByVal inSvcInIdList As List(Of Decimal)) As TabletSmbCommonClassNotFinishedChipCountDataTable

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.{1} Start inDlrCode={2}, inBrnCode={3} ", _
                                  Me.GetType.ToString, _
                                  MethodBase.GetCurrentMethod.Name, _
                                  inDlrCode, _
                                  inBrnCode))

        Dim dtNotFinishedChipCount As TabletSmbCommonClassNotFinishedChipCountDataTable = Nothing

        Using ta As New TabletSMBCommonClassDataAdapter

            '指定サービス入庫ID単位で未完了作業内容件数テーブルを取得
            dtNotFinishedChipCount = ta.GetNotFinishedJobDtlCount(inDlrCode, inBrnCode, inSvcInIdList)

        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.{1} End ", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return dtNotFinishedChipCount

    End Function
    '2015/02/13 TMEJ 範 新プラットフォーム版i-CROP仕様変更対応 (SMBチップ検索及び作業中断機能強化) END

    ''' <summary>
    ''' 日跨ぎ終了通知出す
    ''' </summary>
    ''' <param name="objStaffContext">スタッフ情報</param>
    ''' <param name="inStallId">ストールID</param>
    ''' <remarks></remarks>
    Public Sub SendNoticeByMidFinish(ByVal objStaffContext As StaffContext, ByVal inStallId As Decimal)

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. inStallId={1}" _
                                , MethodBase.GetCurrentMethod.Name, inStallId))

        '指定ストールのTCにPush送信
        Me.SendNamedTCPush(objStaffContext.DlrCD, objStaffContext.BrnCD, inStallId)
        '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 START
        '全てCTにPush送信(自分以外)
        'Me.SendAllCTPush(objStaffContext.DlrCD, objStaffContext.BrnCD, objStaffContext.Account)
        ''指定ストールのChTにPush送信(自分以外)
        'Dim stallList As New List(Of Decimal)
        'stallList.Add(inStallId)
        'Me.SendNamedChtPush(objStaffContext.DlrCD, objStaffContext.BrnCD, objStaffContext.Account, stallList)
        'CT/CHTへのPush処理(ストールに紐づくユーザーのみ)
        SendPushCtChtToStall(objStaffContext.DlrCD, objStaffContext.BrnCD, objStaffContext.Account, inStallId)
        '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 END

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' 洗車中チップUndoの通知を出す
    ''' </summary>
    ''' <param name="svcinId">サービス入庫ID</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2014/12/17 TMEJ 小澤 IT9824_NextSTEPサービス 洗車用端末開発向けた評価用アプリ作成
    ''' </history>
    Public Sub SendNoticeByUndoWashingChip(ByVal svcinId As Decimal)

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. svcinId={1}" _
                                , MethodBase.GetCurrentMethod.Name, svcinId))

        Dim objStaffContext As StaffContext = StaffContext.Current
        '指定SAにPush送信(自分以外)
        Me.SendNamedSAPush(svcinId, objStaffContext.DlrCD, objStaffContext.BrnCD, objStaffContext.Account)

        '2014/12/17 TMEJ 小澤 IT9824_NextSTEPサービス 洗車用端末開発向けた評価用アプリ作成 START

        'CW権限にPUSHする
        Me.SendAllCWPush(objStaffContext.DlrCD, objStaffContext.BrnCD, objStaffContext.Account)

        '2014/12/17 TMEJ 小澤 IT9824_NextSTEPサービス 洗車用端末開発向けた評価用アプリ作成 END

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' 作業終了APIの通知を出す
    ''' </summary>
    ''' <param name="svcInId">サービス入庫ID</param>
    ''' <param name="userInfo">スタフ情報</param>
    ''' <param name="staffAccount">スタッフアカウント</param>
    ''' <remarks></remarks>
    Private Sub SendFinishNoticeApi(ByVal svcInId As Decimal, _
                                    ByVal userInfo As StaffContext, _
                                    ByVal staffAccount As String)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} START " _
            , Me.GetType.ToString _
            , MethodBase.GetCurrentMethod.Name))

        Dim dtNoticeInfo As TabletSmbCommonClassNoticeInfoDataTable
        Dim dmsDlrBrnTable As ServiceCommonClassDataSet.DmsCodeMapRow

        Using ta As New TabletSMBCommonClassDataAdapter
            '情報取得
            dtNoticeInfo = ta.GetNoticeInfo(svcInId, userInfo.DlrCD, userInfo.BrnCD)
            '基幹販売店・店舗コード
            dmsDlrBrnTable = Me.GetDmsDlrBrnCode(userInfo.DlrCD, userInfo.BrnCD, userInfo.Account)
            If IsNothing(dmsDlrBrnTable) Then
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
                Return
            End If
        End Using

        '通知対象ユーザー
        Dim noticeUsersList As New List(Of String)
        If dtNoticeInfo.Count > 0 Then
            Dim drNoticeInfo As TabletSmbCommonClassNoticeInfoRow = dtNoticeInfo(0)
            '通知対象のユーザー
            If Not String.IsNullOrEmpty(drNoticeInfo.PIC_SA_STF_CD) Then

                '指定SAが自分、且つ自分以外送信しない場合、戻す
                '2016/06/29 NSK 皆川 TR-SVT-TMT-20160512-001 SA1はチップを作成していないのに、通知を受け取った START
                'If Not IsNothing(staffAccount) AndAlso staffAccount.Equals(drNoticeInfo.PIC_SA_STF_CD) Then
                If Not IsNothing(staffAccount) AndAlso staffAccount.Equals(drNoticeInfo.PIC_SA_STF_CD) OrElse _
                    String.IsNullOrEmpty(drNoticeInfo.SAChipID) OrElse drNoticeInfo.SAChipID.Equals(DefaultNumberValue.ToString()) Then
                    '2016/06/29 NSK 皆川 TR-SVT-TMT-20160512-001 SA1はチップを作成していないのに、通知を受け取った END
                    ' 正常終了
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
                    Return
                End If

                '通知対象はない場合は通知しない
                noticeUsersList.Add(drNoticeInfo.PIC_SA_STF_CD)
                '基幹販売店コード
                drNoticeInfo.DearlerCode = dmsDlrBrnTable.CODE1
                '基幹店舗コード
                drNoticeInfo.BranchCode = dmsDlrBrnTable.CODE2
                'ログインユーザー
                drNoticeInfo.LoginUserID = dmsDlrBrnTable.ACCOUNT
                '枝番「0」で固定
                drNoticeInfo.SEQ_NO = "0"

                '顧客名敬称付く
                If Not drNoticeInfo.IsNAMETITLE_NAMENull AndAlso _
                    Not String.IsNullOrEmpty(drNoticeInfo.NAMETITLE_NAME) Then
                    Dim cstName As New StringBuilder
                    If Position_Type_Before.Equals(drNoticeInfo.POSITION_TYPE) Then
                        cstName.Append(drNoticeInfo.NAMETITLE_NAME)
                        cstName.Append(drNoticeInfo.CST_NAME)
                    Else
                        cstName.Append(drNoticeInfo.CST_NAME)
                        cstName.Append(drNoticeInfo.NAMETITLE_NAME)
                    End If
                    drNoticeInfo.CST_NAME = cstName.ToString
                End If

                '通知のメッセージを作成する
                Dim noticeMessage As New StringBuilder
                noticeMessage.Append(WebWordUtility.GetWord(ProgramId_Main, 37))
                noticeMessage.Append(Space(3))
                noticeMessage.Append(drNoticeInfo.R_O)
                noticeMessage.Append(Space(3))
                noticeMessage.Append(drNoticeInfo.VCLREGNO)
                noticeMessage.Append(Space(3))
                noticeMessage.Append(drNoticeInfo.CST_NAME)

                drNoticeInfo.Message = noticeMessage.ToString


                '送り先リストを作成
                Dim toAccountList As New List(Of String)
                If Not drNoticeInfo.IsPIC_SA_STF_CDNull AndAlso _
                   Not userInfo.Account.Equals(drNoticeInfo.PIC_SA_STF_CD) Then
                    toAccountList.Add(drNoticeInfo.PIC_SA_STF_CD)
                End If
                '通知共通関数を呼ぶ
                Notice(toAccountList, userInfo, drNoticeInfo)
            End If
        End If

        ' 正常終了
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' 着工指示キャンセルAPIの通知処理
    ''' </summary>
    ''' <param name="drNoticeInfo">通知情報行</param>
    ''' <param name="userInfo">スタフ情報</param>
    ''' <param name="startDateTime">予約の開始日時</param>
    ''' <param name="endDateTime">予約の終了日時</param>
    ''' <param name="stallId">ストールID</param>
    ''' <param name="dmsDlrBrnTable">基幹販売店・店舗コード</param>
    ''' <remarks></remarks>
    Private Sub SendCancelJobInstructNoticeApi(ByVal drNoticeInfo As TabletSmbCommonClassNoticeInfoRow, _
                                               ByVal userInfo As StaffContext, _
                                               ByVal startDateTime As Date, _
                                               ByVal endDateTime As Date, _
                                               ByVal stallId As Decimal, _
                                               ByVal dmsDlrBrnTable As ServiceCommonClassDataSet.DmsCodeMapRow)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} START " _
                                , Me.GetType.ToString _
                                , MethodBase.GetCurrentMethod.Name))

        '基幹販売店コード
        drNoticeInfo.DearlerCode = dmsDlrBrnTable.CODE1
        '基幹店舗コード
        drNoticeInfo.BranchCode = dmsDlrBrnTable.CODE2
        'ログインユーザー
        drNoticeInfo.LoginUserID = dmsDlrBrnTable.ACCOUNT
        '枝番「0」で固定
        drNoticeInfo.SEQ_NO = "0"

        '現在日付を取得
        Dim nowDate As Date = DateTimeFunc.Now(userInfo.DlrCD)
        '開始時間
        Dim startTime As String
        Dim startTimeDateFormat As String
        '当日ではない場合、日付で表示
        If String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd}", nowDate).Equals(String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd}", startDateTime)) Then
            startTimeDateFormat = DateTimeFunc.GetDateFormat(14)
        Else
            startTimeDateFormat = DateTimeFunc.GetDateFormat(11)
        End If
        startTime = String.Format(CultureInfo.InvariantCulture, "{0:" & startTimeDateFormat & "}", startDateTime)

        '終了時間
        Dim endTime As String
        Dim endTimeDateFormat As String
        '当日ではない場合、日付で表示
        If String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd}", nowDate).Equals(String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd}", endDateTime)) Then
            endTimeDateFormat = DateTimeFunc.GetDateFormat(14)
        Else
            endTimeDateFormat = DateTimeFunc.GetDateFormat(11)
        End If
        endTime = String.Format(CultureInfo.InvariantCulture, "{0:" & endTimeDateFormat & "}", endDateTime)

        '顧客名敬称付く
        If Not drNoticeInfo.IsNAMETITLE_NAMENull AndAlso _
            Not String.IsNullOrEmpty(drNoticeInfo.NAMETITLE_NAME) Then
            Dim cstName As New StringBuilder
            If Position_Type_Before.Equals(drNoticeInfo.POSITION_TYPE) Then
                cstName.Append(drNoticeInfo.NAMETITLE_NAME)
                cstName.Append(drNoticeInfo.CST_NAME)
            Else
                cstName.Append(drNoticeInfo.CST_NAME)
                cstName.Append(drNoticeInfo.NAMETITLE_NAME)
            End If
            drNoticeInfo.CST_NAME = cstName.ToString
        End If

        '通知のメッセージを作成する
        Dim noticeMessage As New StringBuilder
        noticeMessage.Append(WebWordUtility.GetWord(ProgramId_Main, 36))
        noticeMessage.Append(Space(3))
        noticeMessage.Append(drNoticeInfo.R_O)
        noticeMessage.Append(Space(3))
        noticeMessage.Append(drNoticeInfo.VCLREGNO)
        noticeMessage.Append(Space(3))
        noticeMessage.Append(drNoticeInfo.CST_NAME)
        noticeMessage.Append(Space(3))
        noticeMessage.Append(startTime)
        noticeMessage.Append(WebWordUtility.GetWord(ProgramId_Main, 38))
        noticeMessage.Append(endTime)
        noticeMessage.Append(Space(3))
        If Not drNoticeInfo.IsMAINTE_NAMENull Then
            noticeMessage.Append(drNoticeInfo.MAINTE_NAME)
        End If

        drNoticeInfo.Message = noticeMessage.ToString

        '送り先リストを作成
        Dim stallIdList As New List(Of Decimal)
        stallIdList.Add(stallId)
        Dim toAccountList As List(Of String) = Me.GetNoticeAccountList(userInfo, stallIdList)

        '通知共通関数を呼ぶ
        Me.Notice(toAccountList, userInfo, drNoticeInfo)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} " _
            , Me.GetType.ToString _
            , MethodBase.GetCurrentMethod.Name))
    End Sub

    '2015/02/09 TMEJ 範 DMS連携版サービスタブレット SMB納車予定時刻通知機能開発 START

    ''' <summary>
    ''' 納車遅れとなる配置を行った場合、通知の処理
    ''' </summary>
    ''' <param name="inUserInfo">ログイン情報</param>
    ''' <param name="inNow">現在日時</param>
    ''' <param name="inChipEntityRow">チップ情報データ行</param>
    ''' <remarks></remarks>
    Public Sub SendNoticeWhenSetChipToBeLated(ByVal inUserInfo As StaffContext, _
                                              ByVal inNow As Date, _
                                              ByVal inChipEntityRow As TabletSmbCommonClassChipEntityRow)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START" _
                  , Me.GetType().ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'RO番号がない場合は通知不要
        If inChipEntityRow.IsRO_NUMNull _
        OrElse String.IsNullOrWhiteSpace(inChipEntityRow.RO_NUM) Then

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                      , "{0}.{1} End [There is no RO_NUM.]" _
                      , Me.GetType.ToString _
                      , System.Reflection.MethodBase.GetCurrentMethod.Name))

            Return

        End If

        '通知用の情報取得
        Dim noticeInfoTable As TabletSmbCommonClassNoticeInfoDataTable = _
            GetSendNoticeInfo(inChipEntityRow.SVCIN_ID, _
                              inUserInfo.DlrCD, _
                              inUserInfo.BrnCD)

        If noticeInfoTable.Count = 0 Then
            '通知情報を取得できない場合

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} END[Failed to get NoticeInfo.]" _
                       , Me.GetType().ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return

        End If

        Dim drNoticeInfo As TabletSmbCommonClassNoticeInfoRow = noticeInfoTable(0)

        '納車見込日時
        Dim prospectDeliveryDate As Date = Nothing

        Using smbCommonClassBiz As New SMBCommonClassBusinessLogic

            '納車見込日時を取得する
            prospectDeliveryDate = smbCommonClassBiz.GetDeliveryDate(inUserInfo.DlrCD, _
                                                                     inUserInfo.BrnCD, _
                                                                     inChipEntityRow.SVCIN_ID, _
                                                                     CLng(drNoticeInfo.SAChipID), _
                                                                     DisplayType.Work, _
                                                                     inNow)

        End Using

        '納車見込日時が下記のいずれかである場合、通知は不要
        '　・Date.MinValueに等しい(smbCommonClassBiz.GetDeliveryDateで計算できなかった)
        '　・予定納車日時以下
        If prospectDeliveryDate.Equals(Date.MinValue) _
        OrElse prospectDeliveryDate <= inChipEntityRow.SCHE_DELI_DATETIME Then

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                      , "{0}.{1} END" _
                      , Me.GetType().ToString _
                      , System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return

        End If

        Dim dmsRow As ServiceCommonClassDataSet.DmsCodeMapRow

        '基幹販売店・店舗コードの取得
        dmsRow = Me.GetDmsDlrBrnCode(inUserInfo.DlrCD, _
                                       inUserInfo.BrnCD, _
                                       inUserInfo.Account)

        If IsNothing(dmsRow) Then
            '取得失敗の場合

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} END[DMS Dealer(Branch) Code are not find.]" _
                       , Me.GetType().ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name))

            Return

        End If

        '通知情報の担当SAスタッフコードがない場合
        '2016/06/29 NSK 皆川 TR-SVT-TMT-20160512-001 SA1はチップを作成していないのに、通知を受け取った START
        'If drNoticeInfo.IsPIC_SA_STF_CDNull _
        'OrElse String.IsNullOrWhiteSpace(drNoticeInfo.PIC_SA_STF_CD) Then
        If drNoticeInfo.IsPIC_SA_STF_CDNull OrElse String.IsNullOrWhiteSpace(drNoticeInfo.PIC_SA_STF_CD) OrElse _
            String.IsNullOrEmpty(drNoticeInfo.SAChipID) OrElse drNoticeInfo.SAChipID.Equals(DefaultNumberValue.ToString()) Then
            '2016/06/29 NSK 皆川 TR-SVT-TMT-20160512-001 SA1はチップを作成していないのに、通知を受け取った END

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                     , "{0}.{1} End[PIC_SA_STF_CD is Null or WhiteSpace.]" _
                                     , Me.GetType.ToString _
                                     , System.Reflection.MethodBase.GetCurrentMethod.Name))

            Return

        End If

        'ログインアカウントが担当SAスタッフコードと異なる場合
        If Not inUserInfo.Account.Equals(drNoticeInfo.PIC_SA_STF_CD) Then

            'Dateフォーマット(MM/dd)
            Dim strDateFormatMMdd As String = DateTimeFunc.GetDateFormat(11)

            'Dateフォーマット(HH/mm)
            Dim strDateFormatHHmm As String = DateTimeFunc.GetDateFormat(14)

            '予定納車日時
            Dim strScheDeliDateTime As String = String.Empty

            If prospectDeliveryDate.Date.Equals(inNow.Date) Then
                '当日の場合

                '予定納車日時変更依頼日時を「HH:mm」にフォーマット
                strScheDeliDateTime = _
                    prospectDeliveryDate.ToString(strDateFormatHHmm, CultureInfo.InvariantCulture)

            Else

                '予定納車日時変更依頼日時を「MM/dd HH:mm」にフォーマット
                strScheDeliDateTime = _
                    prospectDeliveryDate.ToString(strDateFormatMMdd & " " & strDateFormatHHmm, _
                                                  CultureInfo.InvariantCulture)

            End If

            '顧客名に敬称を付与する
            If Not drNoticeInfo.IsNAMETITLE_NAMENull _
            AndAlso Not String.IsNullOrEmpty(drNoticeInfo.NAMETITLE_NAME) Then

                '顧客氏名
                Dim cstName As New StringBuilder

                If Position_Type_Before.Equals(drNoticeInfo.POSITION_TYPE) Then
                    '敬称が名前の前の場合

                    cstName.Append(drNoticeInfo.NAMETITLE_NAME)
                    cstName.Append(drNoticeInfo.CST_NAME)

                Else
                    '敬称が名前の後の場合

                    cstName.Append(drNoticeInfo.CST_NAME)
                    cstName.Append(drNoticeInfo.NAMETITLE_NAME)

                End If

                '名前+敬称
                drNoticeInfo.CST_NAME = cstName.ToString

            End If

            '通知のメッセージを作成する
            Dim noticeMessage As New StringBuilder

            '通知用文言(納車予定日時変更依頼({0}))
            noticeMessage.Append(String.Format(CultureInfo.CurrentCulture, _
                                               WebWordUtility.GetWord(ProgramId_TabletSMBCommon, 1), _
                                               strScheDeliDateTime))
            noticeMessage.Append(Space(3))

            '通知情報のRO番号 
            noticeMessage.Append(drNoticeInfo.R_O)
            noticeMessage.Append(Space(3))

            '通知情報の車両登録番号
            noticeMessage.Append(drNoticeInfo.VCLREGNO)
            noticeMessage.Append(Space(3))

            '顧客氏名
            noticeMessage.Append(drNoticeInfo.CST_NAME)

            '通知メッセージ
            drNoticeInfo.Message = noticeMessage.ToString

            '基幹販売店コード
            drNoticeInfo.DearlerCode = dmsRow.CODE1

            '基幹店舗コード
            drNoticeInfo.BranchCode = dmsRow.CODE2

            'ログインユーザー
            drNoticeInfo.LoginUserID = dmsRow.ACCOUNT

            '枝番「0」で固定
            drNoticeInfo.SEQ_NO = "0"

            '送信先リスト
            Dim toAccountList As New List(Of String)

            '送信対象に通知情報の担当SAスタッフコードを設定
            toAccountList.Add(drNoticeInfo.PIC_SA_STF_CD)

            '通知を出す（Notice関数を呼ぶ）
            Me.Notice(toAccountList, inUserInfo, drNoticeInfo)

            'SAにPushを送信する
            Me.SendNamedSAPush(inChipEntityRow.SVCIN_ID, _
                               inUserInfo.DlrCD, _
                               inUserInfo.BrnCD, _
                               inUserInfo.Account)

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} End" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    '2015/02/09 TMEJ 範 DMS連携版サービスタブレット SMB納車予定時刻通知機能開発 END

    '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする START
    ''' <summary>
    ''' 計画取り消し通知処理
    ''' </summary>
    ''' <param name="stallUseId">ストール利用ID</param>
    ''' <param name="stallId">ストールID</param>
    ''' <param name="jobDtlId">作業内容ID</param>
    ''' <param name="objStaffContext">スタッフ情報</param>
    ''' <param name="chipInstructFlg">着工指示フラグ</param>
    ''' <remarks></remarks>
    Public Sub SendNoticeByToReception(ByVal stallUseId As Decimal, _
                                       ByVal stallId As Decimal, _
                                       ByVal jobDtlId As Decimal, _
                                       ByVal objStaffContext As StaffContext, _
                                       ByVal chipInstructFlg As Boolean)
        Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0}.{1}.S. " _
                                , Me.GetType.ToString, MethodBase.GetCurrentMethod.Name))

        '対象のチップ情報を取得する
        Dim dtChipEntity As TabletSmbCommonClassChipEntityDataTable = GetChipEntity(stallUseId, 1)
        If dtChipEntity.Count <> 1 Then
            Return
        End If

        Dim dmsDlrBrnTable As ServiceCommonClassDataSet.DmsCodeMapRow
        '基幹販売店・店舗コード
        dmsDlrBrnTable = Me.GetDmsDlrBrnCode(objStaffContext.DlrCD, objStaffContext.BrnCD, objStaffContext.Account)
        If IsNothing(dmsDlrBrnTable) Then
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E.GetDmsDlrBrnCode ExceptionError " _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return
        End If

        'Jobの更新が行われていた場合
        If chipInstructFlg = True Then

            '通知
            '情報取得
            Dim dtNoticeInfo As TabletSmbCommonClassNoticeInfoDataTable

            Using ta As New TabletSMBCommonClassDataAdapter
                dtNoticeInfo = ta.GetNoticeInfo(dtChipEntity(0).SVCIN_ID, _
                                                objStaffContext.DlrCD, _
                                                objStaffContext.BrnCD, _
                                                jobDtlId)
            End Using

            If dtNoticeInfo.Count > 0 Then
                Dim drNoticeInfo As TabletSmbCommonClassNoticeInfoRow = dtNoticeInfo(0)
                '通知処理
                Me.SendCancelJobInstructNoticeApi(drNoticeInfo, _
                                                  objStaffContext, _
                                                  dtChipEntity(0).SCHE_START_DATETIME, _
                                                  dtChipEntity(0).SCHE_END_DATETIME, _
                                                  stallId, _
                                                  dmsDlrBrnTable)
            End If

        End If

        'push送信
        '指定ストールのTCにPush送信
        Me.SendNamedTCPush(objStaffContext.DlrCD, objStaffContext.BrnCD, stallId)

        '全てCHTにPUSH送信(自分以外)
        Me.SendAllChtPush(objStaffContext.DlrCD, objStaffContext.BrnCD, objStaffContext.Account)
        '全てCTにPUSH送信(自分以外)
        Me.SendAllCTPush(objStaffContext.DlrCD, objStaffContext.BrnCD, objStaffContext.Account)

        '全てPSにPUSH送信
        Me.SendAllPSPush(objStaffContext.DlrCD, objStaffContext.BrnCD)

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E. ", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする END
#End Region

#Region "各権限により、送信する"
    ''' <summary>
    ''' 指定SAにPushする
    ''' </summary>
    ''' <param name="dealCode">販売店コード</param>
    ''' <param name="brnCode">店舗コード</param>
    ''' <param name="staffCode">ログインアカウント</param>
    ''' <remarks></remarks>
    Public Sub SendNamedSAPush(ByVal svcinId As Decimal, _
                                ByVal dealCode As String, _
                                ByVal brnCode As String, _
                                ByVal staffCode As String)

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. dealCode={1}, brnCode={2}" _
                , MethodBase.GetCurrentMethod.Name, dealCode, brnCode))

        Dim accountSA As String

        '指定SAアカウント取得
        Using ta As New TabletSMBCommonClassDataAdapter
            Dim accountTable As TabletSmbCommonClassStringValueDataTable = _
                    ta.GetSAAcountBySvcinId(svcinId, _
                                            dealCode, _
                                            brnCode)
            If accountTable.Count <> 1 Then
                Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0}.{1} GetSAAcountBySvcinId FAILURE " _
                                         , Me.GetType.ToString, MethodBase.GetCurrentMethod.Name))
                Return
            End If

            '空白の場合送信が要らない
            If accountTable(0).IsCOL1Null OrElse String.IsNullOrEmpty(accountTable(0).COL1) Then
                Return
            End If

            accountSA = accountTable(0).COL1

        End Using

        If Not IsNothing(staffCode) Then
            '該SAが自分の場合、push送信はいらない
            If accountSA.Equals(staffCode) Then
                Return
            End If
        End If

        Dim pushUsersList As New List(Of String)
        pushUsersList.Add(accountSA)

        'ユーザーリストに対してPUSHする
        SendPushByStaffCodeList(pushUsersList, PUSH_FuntionSA)

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))

    End Sub

    '2015/07/17 TMEJ 明瀬 タブレットSMB性能改善 通知WebService化 START

    ' ''' <summary>
    ' ''' 全てPSにPushする
    ' ''' </summary>
    ' ''' <param name="dealCode">販売店コード</param>
    ' ''' <param name="brnCode">店舗コード</param>
    ' ''' <remarks></remarks>
    'Private Sub SendAllPSPush(ByVal dealCode As String, _
    '                         ByVal brnCode As String)

    'Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. dealCode={1}, brnCode={2}" _
    '        , MethodBase.GetCurrentMethod.Name, dealCode, brnCode))

    ''' <summary>
    ''' 全てPSにPushする
    ''' </summary>
    ''' <param name="dealCode">販売店コード</param>
    ''' <param name="brnCode">店舗コード</param>
    ''' <param name="inCallFromWebServiceFlg">
    ''' 呼び出し元がWebServiceフラグ(True：WebService / False：WebService以外)
    ''' ※WebServiceから呼ばれた場合はPush実行メソッドに販売店コードを引数として渡す必要
    ''' 　があるため、SendPushByStaffCodeListの呼び出し分岐に必要
    ''' </param>
    ''' <remarks></remarks>
    Private Sub SendAllPSPush(ByVal dealCode As String, _
                              ByVal brnCode As String, _
                              Optional ByVal inCallFromWebServiceFlg As Boolean = False)

        Logger.Info(String.Format( _
                    CultureInfo.CurrentCulture, _
                    "{0}.{1} START [dealCode={2}, brnCode={3}, inCallFromWebServiceFlg={4}]", _
                    Me.GetType.ToString, _
                    MethodBase.GetCurrentMethod.Name, _
                    dealCode, _
                    brnCode, _
                    inCallFromWebServiceFlg))

        '2015/07/17 TMEJ 明瀬 タブレットSMB性能改善 通知WebService化 END

        Dim pushUsersListPS As New List(Of String)
        Dim operationCodeListPS As New List(Of Decimal)
        Dim exceptStaffCodeListPS As New List(Of String)

        'PS権限を追加
        operationCodeListPS.Add(Operation.PS)

        LogServiceCommonBiz.OutputLog(28, "●■● 1.6.4.1 PSアカウント取得 START")

        'PS権限取得
        pushUsersListPS = Me.GetSendStaffCode(dealCode, brnCode, operationCodeListPS, exceptStaffCodeListPS)

        LogServiceCommonBiz.OutputLog(28, "●■● 1.6.4.1 PSアカウント取得 END")

        LogServiceCommonBiz.OutputLog(29, "●■● 1.6.4.2 PSへPush実行 START")

        '2015/07/17 TMEJ 明瀬 タブレットSMB性能改善 通知WebService化 START

        ''PSに対してPUSHする
        'SendPushByStaffCodeList(pushUsersListPS, PUSH_FuntionPS)

        If inCallFromWebServiceFlg Then
            '呼び出し元がWebServiceメソッドの場合は引数に販売店コードが必要

            'PSに対してPUSHする
            SendPushByStaffCodeList(pushUsersListPS, _
                                    PUSH_FuntionPS, _
                                    dealCode)

        Else

            'PSに対してPUSHする
            SendPushByStaffCodeList(pushUsersListPS, _
                                    PUSH_FuntionPS)

        End If

        '2015/07/17 TMEJ 明瀬 タブレットSMB性能改善 通知WebService化 END

        LogServiceCommonBiz.OutputLog(29, "●■● 1.6.4.2 PSへPush実行[送信件数：" & pushUsersListPS.Count & "] END")

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' 指定ストールのTCにPushする
    ''' </summary>
    ''' <param name="dealCode">販売店コード</param>
    ''' <param name="brnCode">店舗コード</param>
    ''' <remarks></remarks>
    Private Sub SendNamedTCPush(ByVal dealCode As String, _
                               ByVal brnCode As String, _
                               ByVal stallId As Decimal)

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. dealCode={1}, brnCode={2}" _
                , MethodBase.GetCurrentMethod.Name, dealCode, brnCode))
        '指定TCリスト
        Dim toAccountListTC As New List(Of String)

        Dim stallIdList As New List(Of Decimal)
        stallIdList.Add(stallId)

        'TC権限のユーザーを取得
        toAccountListTC = Me.GetSendStaffCodeTC(dealCode, brnCode, stallIdList)

        'TCに対してPUSHする
        SendPushByStaffCodeList(toAccountListTC, PUSH_FuntionNM)

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' 指定ストールのCHTにPushする
    ''' </summary>
    ''' <param name="dealCode">販売店コード</param>
    ''' <param name="brnCode">店舗コード</param>
    ''' <remarks></remarks>
    Private Sub SendNamedChtPush(ByVal dealCode As String, _
                                ByVal brnCode As String, _
                                ByVal staffCode As String, _
                                ByVal stallIdList As List(Of Decimal))

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. dealCode={1}, brnCode={2}, staffCode={3}" _
                , MethodBase.GetCurrentMethod.Name, dealCode, brnCode, staffCode))
        '自分を除外するCHTリスト
        Dim toAccountExceptedCHTList As New List(Of String)

        '指定のCHTリスト
        Dim toAccountListCHT As New List(Of String)

        'CHT権限のユーザーを取得
        toAccountListCHT = Me.GetSendStaffCodeCht(dealCode, brnCode, stallIdList)

        If Not IsNothing(staffCode) Then
            '自分以外のCHTを送信先リストに追加
            For Each toAccountCHT As String In toAccountListCHT
                If staffCode.Equals(toAccountCHT) Then
                    Continue For
                End If
                toAccountExceptedCHTList.Add(toAccountCHT)
            Next
        End If

        'CHTに対してPUSHする
        SendPushByStaffCodeList(toAccountExceptedCHTList, PUSH_FuntionTabletSMB)

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' 全てCTにPushする
    ''' </summary>
    ''' <param name="dealCode">販売店コード</param>
    ''' <param name="brnCode">店舗コード</param>
    ''' <remarks></remarks>
    Private Sub SendAllCTPush(ByVal dealCode As String, _
                             ByVal brnCode As String, _
                             ByVal staffCode As String)

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. dealCode={1}, brnCode={2}, staffCode={3}" _
                , MethodBase.GetCurrentMethod.Name, dealCode, brnCode, staffCode))
        'CT
        Dim pushUsersList As New List(Of String)
        Dim operationCodeList As New List(Of Decimal)
        Dim exceptStaffCodeList As New List(Of String)

        'CT権限を追加
        operationCodeList.Add(Operation.CT)

        If Not IsNothing(staffCode) Then
            '自分を除外する
            exceptStaffCodeList.Add(staffCode)
        End If

        '自分以外のCT権限のユーザーを取得
        pushUsersList = Me.GetSendStaffCode(dealCode, brnCode, operationCodeList, exceptStaffCodeList)

        'CTに対してPUSHする
        SendPushByStaffCodeList(pushUsersList, PUSH_FuntionTabletSMB)

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' 全てのCHTにPushする
    ''' </summary>
    ''' <param name="dealCode">販売店コード</param>
    ''' <param name="brnCode">店舗コード</param>
    ''' <param name="staffCode">除外アカウント</param>
    ''' <remarks></remarks>
    Private Sub SendAllChtPush(ByVal dealCode As String, _
                               ByVal brnCode As String, _
                               ByVal staffCode As String)

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. dealCode={1}, brnCode={2}, staffCode={3}" _
                , MethodBase.GetCurrentMethod.Name, dealCode, brnCode, staffCode))
        'CHT
        Dim pushUsersList As New List(Of String)
        Dim operationCodeList As New List(Of Decimal)
        Dim exceptStaffCodeList As New List(Of String)

        'CHT権限を追加
        operationCodeList.Add(Operation.CHT)

        If Not IsNothing(staffCode) Then
            '除外アカウントを除外する
            exceptStaffCodeList.Add(staffCode)
        End If

        '除外アカウント以外のCHT権限のユーザーを取得
        pushUsersList = Me.GetSendStaffCode(dealCode, brnCode, operationCodeList, exceptStaffCodeList)

        'CHTに対してPUSHする
        SendPushByStaffCodeList(pushUsersList, PUSH_FuntionTabletSMB)

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))

    End Sub

    '2014/12/17 TMEJ 小澤 IT9824_NextSTEPサービス 洗車用端末開発向けた評価用アプリ作成 START

    ''' <summary>
    ''' CW権限へのPush処理
    ''' </summary>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inBranchCode">店舗コード</param>
    ''' <param name="inExceptAccount">除外アカウント</param>
    ''' <remarks></remarks>
    Public Sub SendAllCWPush(ByVal inDealerCode As String, _
                             ByVal inBranchCode As String, _
                             ByVal inExceptAccount As String)
        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.{1} START inDealerCode:{2} inBranchCode:{3} inExceptAccount:{4} ", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  inDealerCode, _
                                  inBranchCode, _
                                  inExceptAccount))

        '送信権限リスト
        Dim listOperationCode As New List(Of Decimal)
        'CW権限を追加
        listOperationCode.Add(Operation.CW)

        '除外アカウントリスト
        Dim listExceptAccount As New List(Of String)

        '除外アカウントチェック
        If Not (String.IsNullOrEmpty(inExceptAccount)) Then
            '存在する場合
            '除外アカウントを追加する
            listExceptAccount.Add(inExceptAccount)

        End If

        '除外アカウント以外のCW権限のユーザーを取得
        Dim listPushUsers As List(Of String) = Me.GetSendStaffCode(inDealerCode, _
                                                                   inBranchCode, _
                                                                   listOperationCode, _
                                                                   listExceptAccount)

        'CWに対してPUSHする
        Me.SendPushByStaffCodeList(listPushUsers, PUSH_FuntionTabletCW)

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.{1} END ", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    '2014/12/17 TMEJ 小澤 IT9824_NextSTEPサービス 洗車用端末開発向けた評価用アプリ作成 END

    '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 START
    ''' <summary>
    ''' CT/CHTへのPush処理(ストールに紐づくユーザーのみ)
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="staffCode">ログインスタッフコード</param>
    ''' <param name="stallId">ストールID</param>
    ''' <remarks></remarks>
    Private Sub SendPushCtChtToStall(ByVal dealerCode As String, _
                                     ByVal branchCode As String, _
                                     ByVal staffCode As String, _
                                     ByVal stallId As Decimal)

        Using serviceCommonbiz As New ServiceCommonClassBusinessLogic
            ' ストールIDのリスト生成
            Dim stallIdList As List(Of Decimal) = New List(Of Decimal)
            stallIdList.Add(stallId)

            ' 権限コードリスト生成(CT・ChT)
            Dim stuffCodeList As New List(Of Decimal)
            stuffCodeList.Add(OperationCT)
            stuffCodeList.Add(OperationChT)

            ' ストールIDよりPush通知アカウントリスト取得
            Dim staffInfoDataTable As ServiceCommonClassDataSet.StaffInfoDataTable
            staffInfoDataTable = serviceCommonbiz.GetNoticeSendAccountListToStall(dealerCode, branchCode, stallIdList, stuffCodeList)

            ' 自分以外の場合Push送信する
            Dim pushUsersList As List(Of String) = New List(Of String)
            For Each row As ServiceCommonClassDataSet.StaffInfoRow In staffInfoDataTable.Rows
                If Not String.Equals(row.ACCOUNT, staffCode) Then
                    pushUsersList.Add(row.ACCOUNT)
                End If
            Next
            SendPushByStaffCodeList(pushUsersList, PUSH_FuntionTabletSMB)
        End Using
    End Sub

    ''' <summary>
    ''' CT/CHTへのPush処理(全て)
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="staffCode">ログインスタッフコード</param>
    ''' <remarks></remarks>
    Private Sub SendPushCtChtAll(ByVal dealerCode As String, _
                                 ByVal branchCode As String, _
                                 ByVal staffCode As String)

        Dim utility As New VisitUtilityBusinessLogic

        ' 権限コードリスト生成(CT・ChT)
        Dim stuffCodeList As New List(Of Decimal)
        stuffCodeList.Add(OperationCT)
        stuffCodeList.Add(OperationChT)

        ' オンラインユーザーの取得
        Dim sendPushUsers As VisitUtilityUsersDataTable = _
        utility.GetOnlineUsers(dealerCode, branchCode, stuffCodeList)
        utility = Nothing

        Dim pushUsersList As List(Of String) = New List(Of String)
        For Each row As VisitUtilityUsersRow In sendPushUsers.Rows
            ' 自分以外の場合Push送信する
            If Not String.Equals(row.ACCOUNT, staffCode) Then
                pushUsersList.Add(row.ACCOUNT)
            End If
        Next
        SendPushByStaffCodeList(pushUsersList, PUSH_FuntionTabletSMB)
    End Sub
    '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 END

#End Region
    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
#End Region

    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START

#Region "JobDispatch送信"

    ''' <summary>
    ''' 基幹連携(JobDispatch実績情報送信処理)を行う(メイン)
    ''' </summary>
    ''' <param name="svcinId">サービス入庫ID</param>
    ''' <param name="jobDtlId">作業内容ID</param>
    ''' <param name="prevJobStatus">更新前作業連携ステータス</param>
    ''' <param name="crntJobStatus">更新後作業連携ステータス</param>
    ''' <returns>正常終了：0、DMS除外エラーの警告：-9000、基幹連携エラー：15</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発
    ''' </history>
    Public Function SendJobClockOnInfo(ByVal svcinId As Decimal, _
                                       ByVal jobDtlId As Decimal, _
                                       ByVal prevJobStatus As IC3802701JobStatusDataTable, _
                                       ByVal crntJobStatus As IC3802701JobStatusDataTable) As Long
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. " _
                                , MethodBase.GetCurrentMethod.Name))

        Using IC3802701Biz As New IC3802701BusinessLogic
            'JobDispatch連携実施
            Dim dmsSendResult As Long = IC3802701Biz.SendJobClockOnInfo(svcinId, _
                                                                        jobDtlId, _
                                                                        prevJobStatus, _
                                                                        crntJobStatus)

            '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

            'If dmsSendResult <> 0 Then
            '    Logger.Error(String.Format(CultureInfo.CurrentCulture, "{0}.{1} SendJobClockOnInfo Failure " _
            '                , Me.GetType.ToString, MethodBase.GetCurrentMethod.Name))
            '    Return ActionResult.DmsLinkageError
            'Else
            '    Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0}.{1} SendJobClockOnInfo Success " _
            '                , Me.GetType.ToString, MethodBase.GetCurrentMethod.Name))
            '    Return ActionResult.Success
            'End If

            '処理結果チェック
            If dmsSendResult = ActionResult.Success Then
                '「0：成功」の場合
                '「0：成功」を返却
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END RETURNCODE={2}[SendStatusInfo Success]" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , ActionResult.Success))
                Return ActionResult.Success

            ElseIf dmsSendResult = ActionResult.WarningOmitDmsError Then
                '「-9000：DMS除外エラーの警告」の場合
                '「-9000：DMS除外エラーの警告」を返却
                Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END RETURNCODE={2}[SendStatusInfo WarningOmitDmsError]" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , ActionResult.WarningOmitDmsError))
                Return ActionResult.WarningOmitDmsError

            Else
                '上記以外の場合
                '「15：他システムとの連携エラー」を返却
                Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END RETURNCODE={2}[SendStatusInfo DmsLinkageError]" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , ActionResult.DmsLinkageError))
                Return ActionResult.DmsLinkageError

            End If

            '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        End Using

    End Function


    ''' <summary>
    ''' Job Dispatch運用を行うか否かを設定
    ''' </summary>
    ''' <returns>作業実績送信使用フラグ True:使用</returns>
    ''' <remarks></remarks>
    Private Function IsUseJobDispatch() As Boolean
        Using serviceCommonBiz As New ServiceCommonClassBusinessLogic
            'Job Dispatch運用フラグ
            Dim jobDispatchUseFlg As String = serviceCommonBiz.GetDlrSystemSettingValueBySettingName(SysParaNameJobDispatchUseFlg)

            If String.IsNullOrEmpty(jobDispatchUseFlg) Then
                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                           "{0}.Error ErrCode: JOBDISPATCH_USE_FLG does not exist.", _
                                           MethodBase.GetCurrentMethod.Name))
                Return False
            Else
                '使用の場合、trueを戻す
                If jobDispatchUseFlg.Trim().Equals("1") Then
                    Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0}.{1}.END IsUseJobDispatch return true. " _
                                , Me.GetType.ToString, MethodBase.GetCurrentMethod.Name))
                    Return True
                Else
                    Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0}.{1}.END IsUseJobDispatch return false. " _
                                , Me.GetType.ToString, MethodBase.GetCurrentMethod.Name))
                    Return False
                End If
            End If

        End Using
    End Function

#Region "Jobステータス判定"

    ''' <summary>
    ''' 該チップに紐付く作業のステータスを取得する
    ''' </summary>
    ''' <param name="inJobDtlId">作業内容ID</param>
    ''' <returns>作業ステータステーブル</returns>
    ''' <remarks></remarks>
    Private Function JudgeJobStatus(ByVal inJobDtlId As Decimal) As IC3802701JobStatusDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture _
                                , "{0}.START IN:inJobDtlId={1}" _
                                , MethodBase.GetCurrentMethod.Name, inJobDtlId))

        Dim jobStatusTable As TabletSmbCommonClassJobResultDataTable = Nothing
        Using ta As New TabletSMBCommonClassDataAdapter
            '作業単位でステータスを取得する
            jobStatusTable = ta.GetJobStatusByJob(inJobDtlId)
        End Using

        '戻す用テーブル
        Using retJobStatusTable As New IC3802701JobStatusDataTable
            For Each jobStatusRow As TabletSmbCommonClassJobResultRow In jobStatusTable
                Dim retJobStatusRow As IC3802701JobStatusRow = retJobStatusTable.NewIC3802701JobStatusRow

                '値の設定
                retJobStatusRow.JOB_DTL_ID = jobStatusRow.JOB_DTL_ID
                retJobStatusRow.JOB_INSTRUCT_ID = jobStatusRow.JOB_INSTRUCT_ID
                retJobStatusRow.JOB_INSTRUCT_SEQ = jobStatusRow.JOB_INSTRUCT_SEQ

                '作業ステータスを設定する
                '作業前(実績テーブルに作業指示のレコードがないので、DBNULLだ)
                If jobStatusRow.IsJOB_STATUSNull Then
                    retJobStatusRow.JOB_STATUS = JobLinkStatusBeforeWork
                Else
                    'Linkテーブル用の作業ステータスに変更する
                    Select Case jobStatusRow.JOB_STATUS
                        Case JobStatusFinish
                            '終了
                            retJobStatusRow.JOB_STATUS = JobLinkStatusFinish
                        Case JobStatusWorking
                            '作業中
                            retJobStatusRow.JOB_STATUS = JobLinkStatusWorking
                        Case JobStatusStop
                            '中断
                            retJobStatusRow.JOB_STATUS = JobLinkStatusStop
                    End Select
                End If

                '一行追加
                retJobStatusTable.AddIC3802701JobStatusRow(retJobStatusRow)
            Next

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                          "{0}.END " _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))
            'ログ
            Me.OutPutIFLog(retJobStatusTable, "IC3802701JobStatusDataTable:")

            Return retJobStatusTable
        End Using

    End Function

    '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
    ''' <summary>
    ''' 単独作業のステータスを取得する
    ''' </summary>
    ''' <param name="inJobStatus">作業内容ID</param>
    ''' <returns>作業ステータステーブル</returns>
    ''' <remarks></remarks>
    Private Function JudgeSingleJobStatus(ByVal inJobDetailId As Decimal, _
                                          ByVal inJobInstructId As String, _
                                          ByVal inJobInstructSeq As Long, _
                                          ByVal inJobStatus As String) As IC3802701JobStatusDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.START. inJobDetailId={1}, inJobInstructId={2}, inJobInstructSeq={3}, inJobStatus={4}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  inJobDetailId, _
                                  inJobInstructId, _
                                  inJobInstructSeq, _
                                  inJobStatus))

        '戻す用テーブル
        Using retJobStatusTable As New IC3802701JobStatusDataTable

            Dim retJobStatusRow As IC3802701JobStatusRow = retJobStatusTable.NewIC3802701JobStatusRow

            '値の設定
            retJobStatusRow.JOB_DTL_ID = inJobDetailId
            retJobStatusRow.JOB_INSTRUCT_ID = inJobInstructId
            retJobStatusRow.JOB_INSTRUCT_SEQ = inJobInstructSeq

            '作業ステータスを設定する
            '作業前(実績テーブルに作業指示のレコードがないので、DBNULLだ)
            Select Case inJobStatus
                Case JobStatusBeforeStart
                    '作業前
                    retJobStatusRow.JOB_STATUS = JobLinkStatusBeforeWork
                Case JobStatusStop
                    '中断
                    retJobStatusRow.JOB_STATUS = JobLinkStatusStop
                Case JobStatusWorking
                    '作業中
                    retJobStatusRow.JOB_STATUS = JobLinkStatusWorking
                Case JobStatusFinish
                    '作業終了
                    retJobStatusRow.JOB_STATUS = JobLinkStatusFinish
            End Select

            '一行追加
            retJobStatusTable.AddIC3802701JobStatusRow(retJobStatusRow)

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}.END JOB_STATUS={1}", _
                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                      retJobStatusRow.JOB_STATUS))

            Return retJobStatusTable
        End Using

    End Function

    ''' <summary>
    ''' 該チップに紐付く作業のステータスを取得する
    ''' </summary>
    ''' <param name="inJobDtlId">作業内容ID</param>
    ''' <returns>作業ステータステーブル</returns>
    ''' <remarks></remarks>
    Public Function GetJobStatusByJob(ByVal inJobDtlId As Decimal) As TabletSmbCommonClassJobResultDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.START IN:inJobDtlId={1}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  inJobDtlId))

        Dim jobStatusTable As TabletSmbCommonClassJobResultDataTable = Nothing
        Using ta As New TabletSMBCommonClassDataAdapter
            '作業単位でステータスを取得する
            jobStatusTable = ta.GetJobStatusByJob(inJobDtlId)
        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.END ", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return jobStatusTable

    End Function

    '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

#End Region
#End Region
    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

    '2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 START
#Region "チップステータス判定"
    ''' <summary>
    ''' チップのステータス判定
    ''' </summary>
    ''' <param name="stallUseId">ストール利用ID</param>
    ''' <returns>
    ''' チップのステータスを判定し、以下のいずれかのチップステータスを返却する。
    ''' 1：未入庫(仮予約)、2：未入庫(本予約)、3：作業開始待ち(仮予約)、
    ''' 4：作業開始待ち(本予約)、6：未来店客、7：飛び込み客、
    ''' 8：作業中、9：中断・部品欠品、10：中断・お客様連絡待ち、
    ''' 11：中断・ストール待機、12：中断・その他、13：中断・検査中断、
    ''' 14：洗車待ち、15：洗車中、16：検査待ち、17：検査中、18：預かり中、
    ''' 19：納車待ち、20：次の作業開始待ち、22：納車済み
    ''' </returns>
    ''' <remarks></remarks>
    Public Function JudgeChipStatus(ByVal stallUseId As Decimal) As String

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.S IN:stallUseId={1}" _
                                , MethodBase.GetCurrentMethod.Name, stallUseId))

        '戻り値
        Dim retValue As String = String.Empty

        'エラー発生フラグ
        Dim errorFlg As Boolean = False

        Try
            'チップエンティティ
            Dim chipEntityTable As TabletSmbCommonClassChipEntityDataTable

            'TabletSMBCommonClassのテーブルアダプタークラスインスタンスを生成
            Using myTableAdapter As New TabletSMBCommonClassDataAdapter
                'チップ情報取得
                chipEntityTable = myTableAdapter.GetChipEntity(stallUseId, 0)
                Me.OutPutIFLog(chipEntityTable, "ChipEntityTable:") 'ログ
            End Using

            'チップ情報が取得できない場合はエラー
            If chipEntityTable.Count <= 0 Then
                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                       "{0}.Error Err:Failed to get the chip information.", _
                       MethodBase.GetCurrentMethod.Name))
                errorFlg = True
                Exit Try
            End If

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.DirectCast Row Start", MethodBase.GetCurrentMethod.Name))
            'データ行の抜き出し
            Dim chipEntityRow As TabletSmbCommonClassChipEntityRow _
                = DirectCast(chipEntityTable.Rows(0), TabletSmbCommonClassChipEntityRow)
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.DirectCast Row End", MethodBase.GetCurrentMethod.Name))
            'サービスステータス
            Dim svcStatus As String = chipEntityRow.SVC_STATUS
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.Set SVC_STATUS={1}", MethodBase.GetCurrentMethod.Name, chipEntityRow.SVC_STATUS))
            '予約ステータス
            Dim resvStatus As String = chipEntityRow.RESV_STATUS
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.Set RESV_STATUS={1}", MethodBase.GetCurrentMethod.Name, chipEntityRow.RESV_STATUS))
            'ストール利用ステータス
            Dim stallUseStatus As String = chipEntityRow.STALL_USE_STATUS
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.Set STALL_USE_STATUS={1}", MethodBase.GetCurrentMethod.Name, chipEntityRow.STALL_USE_STATUS))
            '中断理由区分
            Dim stopReasonType As String = chipEntityRow.STOP_REASON_TYPE
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.Set STOP_REASON_TYPE={1}", MethodBase.GetCurrentMethod.Name, chipEntityRow.STOP_REASON_TYPE))
            '関連ストール非稼動ID
            Dim stallIdleId As Decimal = chipEntityRow.STALL_IDLE_ID
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.Set STALL_IDLE_ID={1}", MethodBase.GetCurrentMethod.Name, chipEntityRow.STALL_IDLE_ID))
            '受付区分
            Dim acceptanceType As String = chipEntityRow.ACCEPTANCE_TYPE
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.Set ACCEPTANCE_TYPE={1}", MethodBase.GetCurrentMethod.Name, chipEntityRow.ACCEPTANCE_TYPE))
            'ストールID
            Dim stallId As Decimal = chipEntityRow.STALL_ID
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.Set STALL_ID={1}", MethodBase.GetCurrentMethod.Name, chipEntityRow.STALL_ID))

            '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする START
            '仮置きフラグ
            Dim tempFlg As String = chipEntityRow.TEMP_FLG
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.Set TEMP_FLG={1}", MethodBase.GetCurrentMethod.Name, chipEntityRow.TEMP_FLG))
            'サービスステータスによって分岐
            Select Case svcStatus

                Case SvcStatusNotCarin
                    'サービスステータス「00：未入庫」の場合
                    'retValue = Me.JudgeNotCarInStatus(resvStatus)
                    retValue = Me.JudgeNotCarInStatus(resvStatus, tempFlg)

                Case SvcStatusNoShow
                    'サービスステータス「01：未来店客」の場合
                    'retValue = Me.JudgeNoShowStatus(resvStatus, stallUseStatus)
                    retValue = Me.JudgeNoShowStatus(resvStatus, stallUseStatus, tempFlg)

                Case SvcStatusWorkOrderWait, SvcStatusStartwait, SvcStatusNextStartWait
                    'サービスステータス「03：着工指示待ち」「04：作業開始待ち」「06：次の作業開始待ち」の場合
                    retValue = Me.JudgeWaitStartStatus(resvStatus, stallUseStatus, stopReasonType, stallIdleId, acceptanceType, stallId, tempFlg)
                    '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする END

                Case SvcStatusStart
                    'サービスステータス「05：作業中」
                    retValue = Me.JudgeStartStatus(stallUseStatus, stopReasonType, stallIdleId)

                Case SvcStatusCarWashWait
                    'サービスステータス「07：洗車待ち」→チップステータス【14:洗車待ち】
                    retValue = ChipStatusWaitWash

                Case SvcStatusCarWashStart
                    'サービスステータス「08：洗車中」→チップステータス【15:洗車中】
                    retValue = ChipStatusWashing

                Case SvcStatusInspectionWait
                    'サービスステータス「09：検査待ち」→チップステータス【16:検査待ち】
                    retValue = ChipStatusWaitInspection

                Case SvcStatusInspectionStart
                    'サービスステータス「10：検査中」→チップステータス【17:検査中】
                    retValue = ChipStatusInspecting

                Case SvcStatusDropOffCustomer
                    'サービスステータス「11：預かり中」→チップステータス【18:預かり中】
                    retValue = ChipStatusKeeping

                Case SvcStatusWaitingCustomer
                    'サービスステータス「12：納車待ち」→チップステータス【19:納車待ち】
                    retValue = ChipStatusWaitDelivery

                Case SvcStatusDelivery
                    'サービスステータス「13：納車済み」→チップステータス【22:納車済み】
                    retValue = ChipStatusDeliveryEnd

            End Select

        Finally
            If errorFlg Then
                retValue = String.Empty
            End If
        End Try

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.E OUT:retValue={1}", _
                                  MethodBase.GetCurrentMethod.Name, retValue))

        Return retValue

    End Function

    '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする START
    ' ''' <summary>
    ' ''' サービスステータス「00:未入庫」の場合のチップステータスを判定
    ' ''' </summary>
    ' ''' <param name="resvStatus">予約ステータス</param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Private Function JudgeNotCarInStatus(ByVal resvStatus As String) As String

    '    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
    '                              "{0}.S IN:resvStatus={1}", _
    '                              MethodBase.GetCurrentMethod.Name, resvStatus))

    '    '戻り値
    '    Dim retValue As String = String.Empty

    '    If resvStatus.Equals(ResvStatusTentative) Then
    '        'チップステータス【1：未入庫(仮予約)】
    '        retValue = ChipStatusTentativeNotCarIn
    '    Else
    '        'チップステータス【2：未入庫(本予約)】
    '        retValue = ChipStatusConfirmedNotCarIn
    '    End If

    '    Return retValue

    '    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
    '                              "{0}.E OUT:retValue={1}", _
    '                              MethodBase.GetCurrentMethod.Name, retValue))

    'End Function

    ''' <summary>
    ''' サービスステータス「00:未入庫」の場合のチップステータスを判定
    ''' </summary>
    ''' <param name="resvStatus">予約ステータス</param>
    ''' <param name="tempFlg">仮置きフラグ</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function JudgeNotCarInStatus(ByVal resvStatus As String, ByVal tempFlg As String) As String

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.S IN:resvStatus={1}, tempFlg={2}", _
                                  MethodBase.GetCurrentMethod.Name, resvStatus, tempFlg))

        '戻り値
        Dim retValue As String = String.Empty

        If tempFlg = "1" Then
            'チップステータス【5：仮置き】
            retValue = ChipStatusTemp
        Else

            If resvStatus.Equals(ResvStatusTentative) Then
                'チップステータス【1：未入庫(仮予約)】
                retValue = ChipStatusTentativeNotCarIn
            Else
                'チップステータス【2：未入庫(本予約)】
                retValue = ChipStatusConfirmedNotCarIn
            End If

        End If

        Return retValue

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.E OUT:retValue={1}", _
                                  MethodBase.GetCurrentMethod.Name, retValue))

    End Function

    ' ''' <summary>
    ' ''' サービスステータス「01：未来店客」の場合のチップステータスを判定
    ' ''' </summary>
    ' ''' <param name="resvStatus">予約ステータス</param>
    ' ''' <param name="stallUseStatus">ストール利用ステータス</param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Private Function JudgeNoShowStatus(ByVal resvStatus As String, ByVal stallUseStatus As String) As String

    '    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
    '                              "{0}.S IN:resvStatus={1}, stallUseStatus={2}", _
    '                              MethodBase.GetCurrentMethod.Name, resvStatus, stallUseStatus))

    '    '戻り値
    '    Dim retValue As String = String.Empty

    '    If stallUseStatus.Equals(StalluseStatusNoshow) Then
    '        'ストール利用ステータス「07：未来店客」の場合

    '        'If resvStatus.Equals(ResvStatusTentative) Then
    '        '    'チップステータス【24：未来店客(仮予約)】
    '        '    retValue = ChipStatusTentativeNoShow
    '        'Else
    '        '    'チップステータス【6：未来店客(本予約)】
    '        '    retValue = ChipStatusNoshow
    '        'End If

    '        'チップステータス【6：未来店客】
    '        retValue = ChipStatusNoshow

    '    ElseIf stallUseStatus.Equals(StalluseStatusWorkOrderWait) _
    '    OrElse stallUseStatus.Equals(StalluseStatusStartWait) Then
    '        'ストール利用ステータス「00：着工指示待ち」または「01：作業開始待ち」の場合

    '        If resvStatus.Equals(ResvStatusTentative) Then
    '            'チップステータス【1：未入庫(仮予約)】
    '            retValue = ChipStatusTentativeNotCarIn
    '        Else
    '            'チップステータス【2：未入庫(本予約)】
    '            retValue = ChipStatusConfirmedNotCarIn
    '        End If
    '    End If

    '    Return retValue

    '    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
    '                              "{0}.E OUT:retValue={1}", _
    '                              MethodBase.GetCurrentMethod.Name, retValue))

    'End Function

    ''' <summary>
    ''' サービスステータス「01：未来店客」の場合のチップステータスを判定
    ''' </summary>
    ''' <param name="resvStatus">予約ステータス</param>
    ''' <param name="stallUseStatus">ストール利用ステータス</param>
    ''' <param name="tempFlg">仮置きフラグ</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function JudgeNoShowStatus(ByVal resvStatus As String, ByVal stallUseStatus As String, ByVal tempFlg As String) As String

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.S IN:resvStatus={1}, stallUseStatus={2}, tempFlg={3}", _
                                  MethodBase.GetCurrentMethod.Name, resvStatus, stallUseStatus, tempFlg))

        '戻り値
        Dim retValue As String = String.Empty

        If tempFlg = "1" Then
            'チップステータス【5：仮置き】
            retValue = ChipStatusTemp
        ElseIf stallUseStatus.Equals(StalluseStatusNoshow) Then
            'チップステータス【6：未来店客(本予約)】
            retValue = ChipStatusNoshow
        ElseIf stallUseStatus.Equals(StalluseStatusWorkOrderWait) Or stallUseStatus.Equals(StalluseStatusStartWait) Then
            If resvStatus.Equals(ResvStatusTentative) Then
                'チップステータス【1：未入庫(仮予約)】
                retValue = ChipStatusTentativeNotCarIn
            Else
                'チップステータス【2：未入庫(本予約)】
                retValue = ChipStatusConfirmedNotCarIn
            End If

        End If
        Return retValue

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.E OUT:retValue={1}", _
                                  MethodBase.GetCurrentMethod.Name, retValue))
    End Function

    ' ''' <summary>
    ' ''' サービスステータス「03:着工指示待ち」「04:作業開始待ち」「06:次の作業開始待ち」の場合のチップステータスを判定
    ' ''' </summary>
    ' ''' <param name="resvStatus">予約ステータス</param>
    ' ''' <param name="stallUseStatus">ストール利用ステータス</param>
    ' ''' <param name="stopReasonType">中断理由区分</param>
    ' ''' <param name="stallIdleId">関連ストール非稼動ID</param>
    ' ''' <param name="acceptanceType">受付区分</param>
    ' ''' <param name="stallId">ストールID</param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Private Function JudgeWaitStartStatus(ByVal resvStatus As String, ByVal stallUseStatus As String, _
    '                                      ByVal stopReasonType As String, ByVal stallIdleId As Decimal, _
    '                                      ByVal acceptanceType As String, ByVal stallId As Decimal) As String

    '    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
    '                              "{0}.S IN:resvStatus={1}, stallUseStatus={2}, stopReasonType={3}, stallIdleId={4}, acceptanceType={5}, stallId={6}", _
    '                              MethodBase.GetCurrentMethod.Name, resvStatus, stallUseStatus, stopReasonType, stallIdleId, acceptanceType, stallId))

    '    '戻り値
    '    Dim retValue As String = String.Empty

    '    'ストール利用ステータスで分岐
    '    Select Case stallUseStatus

    '        Case StalluseStatusFinish
    '            'ストール利用ステータス「03：完了」の場合
    '            'チップステータス【20：次の作業開始待ち】
    '            retValue = ChipStatusJobFinish

    '        Case StalluseStatusStop
    '            'ストール利用ステータス「05：中断」の場合
    '            retValue = Me.JudgeStopStatus(stopReasonType, stallIdleId)

    '        Case StalluseStatusWorkOrderWait, StalluseStatusStartWait
    '            'ストール利用ステータス「00：着工指示待ち」「01：作業開始待ち」の場合

    '            If acceptanceType.Equals(AcceptanceTypeWalkin) _
    '            AndAlso stallId = DefaultNumberValue Then
    '                '受付区分が「1：Walk-in」、かつストールIDが未設定の場合
    '                'チップステータス【7：Walk-in】
    '                retValue = ChipStatusWalkin

    '            Else
    '                If resvStatus.Equals(ResvStatusTentative) Then
    '                    'チップステータス【3：作業開始待ち(仮予約)】
    '                    retValue = ChipStatusTentativeWaitStart
    '                Else
    '                    'チップステータス【4：作業開始待ち(本予約)】
    '                    retValue = ChipStatusConfirmedWaitStart
    '                End If
    '            End If

    '        Case StalluseStatusMidfinish
    '            'ストール利用ステータス「06：日跨ぎ終了」の場合
    '            'チップステータス【21：日跨ぎ終了】
    '            retValue = ChipStatusDateCrossEnd

    '    End Select

    '    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
    '                              "{0}.E OUT:retValue={1}", _
    '                              MethodBase.GetCurrentMethod.Name, retValue))

    '    Return retValue

    'End Function

    ''' <summary>
    ''' サービスステータス「03:着工指示待ち」「04:作業開始待ち」「06:次の作業開始待ち」の場合のチップステータスを判定
    ''' </summary>
    ''' <param name="resvStatus">予約ステータス</param>
    ''' <param name="stallUseStatus">ストール利用ステータス</param>
    ''' <param name="stopReasonType">中断理由区分</param>
    ''' <param name="stallIdleId">関連ストール非稼動ID</param>
    ''' <param name="acceptanceType">受付区分</param>
    ''' <param name="stallId">ストールID</param>
    ''' <param name="tempFlg">仮置きフラグ</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function JudgeWaitStartStatus(ByVal resvStatus As String, ByVal stallUseStatus As String, _
                                          ByVal stopReasonType As String, ByVal stallIdleId As Decimal, _
                                          ByVal acceptanceType As String, ByVal stallId As Decimal, _
                                          ByVal tempFlg As String) As String

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.S IN:resvStatus={1}, stallUseStatus={2}, stopReasonType={3}, stallIdleId={4}, acceptanceType={5}, stallId={6}, tempFlg={7}", _
                                  MethodBase.GetCurrentMethod.Name, resvStatus, stallUseStatus, stopReasonType, stallIdleId, acceptanceType, stallId, tempFlg))

        '戻り値
        Dim retValue As String = String.Empty

        'ストール利用ステータスで分岐
        Select Case stallUseStatus

            Case StalluseStatusFinish
                'ストール利用ステータス「03：完了」の場合
                'チップステータス【20：次の作業開始待ち】
                retValue = ChipStatusJobFinish

            Case StalluseStatusStop
                'ストール利用ステータス「05：中断」の場合
                retValue = Me.JudgeStopStatus(stopReasonType, stallIdleId)

            Case StalluseStatusWorkOrderWait, StalluseStatusStartWait
                'ストール利用ステータス「00：着工指示待ち」「01：作業開始待ち」の場合

                If acceptanceType.Equals(AcceptanceTypeWalkin) _
                AndAlso stallId = DefaultNumberValue Then
                    '受付区分が「1：Walk-in」、かつストールIDが未設定の場合
                    'チップステータス【7：Walk-in】
                    retValue = ChipStatusWalkin
                ElseIf tempFlg = "1" Then
                    'チップステータス【5：仮置き】
                    retValue = ChipStatusTemp
                Else
                    If resvStatus.Equals(ResvStatusTentative) Then
                        'チップステータス【3：作業開始待ち(仮予約)】
                        retValue = ChipStatusTentativeWaitStart
                    Else
                        'チップステータス【4：作業開始待ち(本予約)】
                        retValue = ChipStatusConfirmedWaitStart
                    End If
                End If

            Case StalluseStatusMidfinish
                'ストール利用ステータス「06：日跨ぎ終了」の場合
                'チップステータス【21：日跨ぎ終了】
                retValue = ChipStatusDateCrossEnd

        End Select

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.E OUT:retValue={1}", _
                                  MethodBase.GetCurrentMethod.Name, retValue))

        Return retValue

    End Function

    '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする END

    ''' <summary>
    ''' ストール利用ステータス「05:中断」の場合のチップステータス判定
    ''' </summary>
    ''' <param name="stopReasonType">中断理由区分</param>
    ''' <param name="stallIdleId">関連ストール非稼動ID</param>
    ''' <returns>
    ''' チップのステータスを判定し、以下のいずれかのチップステータスを返却する。
    ''' 9：中断・部品欠品、10：中断・お客様連絡待ち、11：中断・ストール待機、
    ''' 12：中断・その他、13：中断・検査中断
    ''' </returns>
    ''' <remarks></remarks>
    Private Function JudgeStopStatus(ByVal stopReasonType As String, ByVal stallIdleId As Decimal) As String

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.S IN:stopReasonType={1}, stallIdleId={2}", _
                                  MethodBase.GetCurrentMethod.Name, stopReasonType, stallIdleId))

        '戻り値
        Dim retValue As String = String.Empty

        If stallIdleId <> DefaultNumberValue Then
            '関連ストール非稼動IDが設定されている場合
            'チップステータス【11：作業中断(ストール待ち)】
            retValue = ChipStatusStopForWaitStall
        Else
            '中断理由区分で分岐
            Select Case stopReasonType
                Case StopReasonPartsStockOut
                    '中断理由区分「01：部品欠品」の場合
                    'チップステータス【09：作業中断(部品欠品)】
                    retValue = ChipStatusStopForPartsStockout

                Case StopReasonCustomerReportWaiting
                    '中断理由区分「02：お客様連絡待ち」の場合
                    'チップステータス【10：作業中断(お客様連絡待ち)】
                    retValue = ChipStatusStopForWaitCustomer

                Case StopReasonInspectionFailure
                    '中断理由区分「03：検査不合格」の場合
                    'チップステータス【13：作業中断(検査中断)】
                    retValue = ChipStatusStopForInspection

                Case StopReasonOthers
                    '中断理由区分が「99：その他」の場合
                    'チップステータス【12：作業中断(その他)】
                    retValue = ChipStatusStopForOtherReason

                Case Else
                    '中断理由区分が上記以外の場合
                    'チップステータス【12：作業中断(その他)】
                    retValue = ChipStatusStopForOtherReason
            End Select
        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.E OUT:retValue={1}", _
                                  MethodBase.GetCurrentMethod.Name, retValue))

        Return retValue

    End Function

    ''' <summary>
    ''' サービスステータス「05:作業中」の場合のチップステータス判定
    ''' </summary>
    ''' <param name="stallUseStatus">ストール利用ステータス</param>
    ''' <param name="stopReasonType">中断理由区分</param>
    ''' <param name="stallIdleId">関連ストール非稼動ID</param>
    ''' <returns>
    ''' チップのステータスを判定し、以下のいずれかのチップステータスを返却する。
    ''' 4：作業開始待ち（本予約）、8：作業中、9：中断・部品欠品、
    ''' 10：中断・お客様連絡待ち、11：中断・ストール待機、12：中断・その他、13：中断・検査中断、20：作業完了
    ''' </returns>
    ''' <remarks></remarks>
    Private Function JudgeStartStatus(ByVal stallUseStatus As String, ByVal stopReasonType As String, ByVal stallIdleId As Decimal) As String

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.S IN:stallUseStatus={1}, stopReasonType={2}, stallIdleId={3}", _
                                  MethodBase.GetCurrentMethod.Name, stallUseStatus, stopReasonType, stallIdleId))

        '戻り値
        Dim retValue As String = String.Empty

        'ストール利用ステータスで分岐
        Select Case stallUseStatus

            Case StalluseStatusWorkOrderWait, StalluseStatusStartWait
                'ストール利用ステータス「00：着工指示待ち」「01：作業開始待ち」の場合
                'チップステータス【4：作業開始待ち(本予約)】
                retValue = ChipStatusConfirmedWaitStart

            Case StalluseStatusStart, StalluseStatusStartIncludeStopJob
                'ストール利用ステータス「02：作業中」「04：作業指示の一部の作業が中断」の場合
                'チップステータス【8：作業中】
                retValue = ChipStatusWorking

            Case StalluseStatusStop
                'ストール利用ステータス「05：中断」の場合
                retValue = Me.JudgeStopStatus(stopReasonType, stallIdleId)

            Case StalluseStatusFinish
                'ストール利用ステータス「03：完了」の場合
                'チップステータス【20：次の作業開始待ち】
                retValue = ChipStatusJobFinish

        End Select

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.E OUT:retValue={1}", _
                                  MethodBase.GetCurrentMethod.Name, retValue))

        Return retValue

    End Function
#End Region
    '2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 END

    '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
#Region "通知"

    ''' <summary>
    ''' 通知メイン処理
    ''' </summary>
    ''' <param name="toAccountList">送り先リスト</param>
    ''' <param name="inStaffInfo">登録情報</param>
    ''' <param name="inRowNoticeInfo">送り情報ROW</param>
    ''' <remarks></remarks>
    Public Sub Notice(ByVal toAccountList As List(Of String), _
                      ByVal inStaffInfo As StaffContext, _
                      ByVal inRowNoticeInfo As TabletSmbCommonClassNoticeInfoRow)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} START" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name))

        If Not toAccountList.Count > 0 Then
            '送信先が設定されない場合、処理しない
            Return
        End If

        LogServiceCommonBiz.OutputLog(15, "●■● 1.4.4.1 送信先アカウントリスト作成 START")

        '送信先リスト(TC、SVR以外)
        Dim sendToUsersList As New List(Of XmlAccount)
        '送信先リスト(TC、SVR)
        Dim sendToUsersListTcSvr As New List(Of XmlAccount)
        '送信先アカウント情報設定
        For Each toAccount As String In toAccountList
            Dim usersClass As New Users
            Dim rowUsers As UsersDataSet.USERSRow
            'ユーザー情報の取得
            rowUsers = usersClass.GetUser(toAccount, DelFlgNone)
            If IsNothing(rowUsers) Then
                Continue For
            End If
            Dim account As XmlAccount = Me.CreateAccount(toAccount)
            If OperationTC.Equals(rowUsers.OPERATIONCODE) OrElse _
                OperationSVR.Equals(rowUsers.OPERATIONCODE) Then
                sendToUsersListTcSvr.Add(account)
            Else
                sendToUsersList.Add(account)
            End If
        Next

        LogServiceCommonBiz.OutputLog(15, "●■● 1.4.4.1 送信先アカウントリスト作成[TC・SVR以外件数：" & _
                                      sendToUsersList.Count & _
                                      "][TC・SVR件数：" & _
                                      sendToUsersListTcSvr.Count & _
                                      "] END")

        'TC、SVR以外に送る
        If sendToUsersList.Count > 0 Then
            Dim requestNotice As XmlRequestNotice
            If Not (inRowNoticeInfo.IsCUSTSEGMENTNull) _
                AndAlso CustSegmentMyCustomer.Equals(inRowNoticeInfo.CUSTSEGMENT) _
                AndAlso Not String.IsNullOrWhiteSpace(inRowNoticeInfo.DMS_CST_ID) Then
                '顧客詳細リンクあり場合
                '通知履歴登録情報の設定
                requestNotice = Me.CreateRequestNotice(inRowNoticeInfo, inStaffInfo, MessageType.CustomerLink_ON)
            Else
                requestNotice = Me.CreateRequestNotice(inRowNoticeInfo, inStaffInfo, MessageType.CustomerLink_OFF)
            End If
            'Push内容設定
            Dim pushInfo As XmlPushInfo = Me.CreatePushInfo(inRowNoticeInfo.Message)
            '設定したものを格納し、通知APIをコール
            Using noticeData As New XmlNoticeData
                '現在時間データの格納
                noticeData.TransmissionDate = DateTimeFunc.Now(inStaffInfo.DlrCD)
                '送信ユーザーデータ格納
                noticeData.AccountList.AddRange(sendToUsersList)
                '通知履歴用のデータ格納
                noticeData.RequestNotice = requestNotice
                'Pushデータ格納
                noticeData.PushInfo = pushInfo

                LogServiceCommonBiz.OutputLog(16, "●■● 1.4.4.2 IC3040801呼び出し(TC・SVR以外) START")

                '通知処理実行
                Using ic3040801Biz As New IC3040801BusinessLogic

                    '通知処理実行
                    ic3040801Biz.NoticeDisplay(noticeData, NoticeDisposal.GeneralPurpose)

                End Using

                LogServiceCommonBiz.OutputLog(16, "●■● 1.4.4.2 IC3040801呼び出し(TC・SVR以外) END")

            End Using
        End If

        'TCとSVRに送る場合顧客詳細リンクなし
        If sendToUsersListTcSvr.Count > 0 Then
            Dim requestNoticeTcSvr As XmlRequestNotice = Me.CreateRequestNotice(inRowNoticeInfo, inStaffInfo, MessageType.CustomerLink_OFF)
            'Push内容設定
            Dim pushInfo As XmlPushInfo = Me.CreatePushInfo(inRowNoticeInfo.Message)
            '設定したものを格納し、通知APIをコール
            Using noticeData As New XmlNoticeData
                '現在時間データの格納
                noticeData.TransmissionDate = DateTimeFunc.Now(inStaffInfo.DlrCD)
                '送信ユーザーデータ格納
                noticeData.AccountList.AddRange(sendToUsersListTcSvr)
                '通知履歴用のデータ格納
                noticeData.RequestNotice = requestNoticeTcSvr
                'Pushデータ格納
                noticeData.PushInfo = pushInfo

                LogServiceCommonBiz.OutputLog(17, "●■● 1.4.4.3 IC3040801呼び出し(TC・SVR) START")

                '通知処理実行
                Using ic3040801Biz As New IC3040801BusinessLogic

                    '通知処理実行
                    ic3040801Biz.NoticeDisplay(noticeData, NoticeDisposal.GeneralPurpose)

                End Using

                LogServiceCommonBiz.OutputLog(17, "●■● 1.4.4.3 IC3040801呼び出し(TC・SVR) END")

            End Using
        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    '2015/07/17 TMEJ 明瀬 タブレットSMB性能改善 通知WebService化 START

    ''' <summary>
    ''' 通知メイン処理
    ''' </summary>
    ''' <param name="toAccountList">送り先リスト</param>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inBranchCode">店舗コード</param>
    ''' <param name="inStaffAccount">ログインスタッフアカウント</param>
    ''' <param name="inStaffName">ログインスタッフ名</param>
    ''' <param name="inRowNoticeInfo">送り情報ROW</param>
    ''' <remarks></remarks>
    Public Sub Notice(ByVal toAccountList As List(Of String), _
                      ByVal inDealerCode As String, _
                      ByVal inBranchCode As String, _
                      ByVal inStaffAccount As String, _
                      ByVal inStaffName As String, _
                      ByVal inRowNoticeInfo As TabletSmbCommonClassNoticeInfoRow)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} START" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name))

        If Not toAccountList.Count > 0 Then
            '送信先が設定されない場合、処理しない
            Return
        End If

        LogServiceCommonBiz.OutputLog(15, "●■● 1.4.4.1 送信先アカウントリスト作成 START")

        '送信先リスト(TC、SVR以外)
        Dim sendToUsersList As New List(Of XmlAccount)
        '送信先リスト(TC、SVR)
        Dim sendToUsersListTcSvr As New List(Of XmlAccount)
        '送信先アカウント情報設定
        For Each toAccount As String In toAccountList
            Dim usersClass As New Users
            Dim rowUsers As UsersDataSet.USERSRow
            'ユーザー情報の取得
            rowUsers = usersClass.GetUser(toAccount, DelFlgNone)
            If IsNothing(rowUsers) Then
                Continue For
            End If
            Dim account As XmlAccount = Me.CreateAccount(toAccount)
            If OperationTC.Equals(rowUsers.OPERATIONCODE) OrElse _
                OperationSVR.Equals(rowUsers.OPERATIONCODE) Then
                sendToUsersListTcSvr.Add(account)
            Else
                sendToUsersList.Add(account)
            End If
        Next

        LogServiceCommonBiz.OutputLog(15, "●■● 1.4.4.1 送信先アカウントリスト作成[TC・SVR以外件数：" & _
                                      sendToUsersList.Count & _
                                      "][TC・SVR件数：" & _
                                      sendToUsersListTcSvr.Count & _
                                      "] END")

        'TC、SVR以外に送る
        If sendToUsersList.Count > 0 Then
            Dim requestNotice As XmlRequestNotice
            If Not (inRowNoticeInfo.IsCUSTSEGMENTNull) _
                AndAlso CustSegmentMyCustomer.Equals(inRowNoticeInfo.CUSTSEGMENT) _
                AndAlso Not String.IsNullOrWhiteSpace(inRowNoticeInfo.DMS_CST_ID) Then
                '顧客詳細リンクあり場合
                '通知履歴登録情報の設定
                requestNotice = Me.CreateRequestNotice(inRowNoticeInfo, _
                                                       inDealerCode, _
                                                       inBranchCode, _
                                                       inStaffAccount, _
                                                       inStaffName, _
                                                       MessageType.CustomerLink_ON)
            Else
                requestNotice = Me.CreateRequestNotice(inRowNoticeInfo, _
                                                       inDealerCode, _
                                                       inBranchCode, _
                                                       inStaffAccount, _
                                                       inStaffName, _
                                                       MessageType.CustomerLink_OFF)
            End If
            'Push内容設定
            Dim pushInfo As XmlPushInfo = Me.CreatePushInfo(inRowNoticeInfo.Message)
            '設定したものを格納し、通知APIをコール
            Using noticeData As New XmlNoticeData
                '現在時間データの格納
                noticeData.TransmissionDate = DateTimeFunc.Now(inDealerCode)
                '送信ユーザーデータ格納
                noticeData.AccountList.AddRange(sendToUsersList)
                '通知履歴用のデータ格納
                noticeData.RequestNotice = requestNotice
                'Pushデータ格納
                noticeData.PushInfo = pushInfo

                LogServiceCommonBiz.OutputLog(16, "●■● 1.4.4.2 IC3040801呼び出し(TC・SVR以外) START")

                '通知処理実行
                Using ic3040801Biz As New IC3040801BusinessLogic

                    '通知処理実行
                    ic3040801Biz.NoticeDisplay(noticeData, NoticeDisposal.GeneralPurpose)

                End Using

                LogServiceCommonBiz.OutputLog(16, "●■● 1.4.4.2 IC3040801呼び出し(TC・SVR以外) END")

            End Using
        End If

        'TCとSVRに送る場合顧客詳細リンクなし
        If sendToUsersListTcSvr.Count > 0 Then
            Dim requestNoticeTcSvr As XmlRequestNotice = Me.CreateRequestNotice(inRowNoticeInfo, _
                                                                                inDealerCode, _
                                                                                inBranchCode, _
                                                                                inStaffAccount, _
                                                                                inStaffName, _
                                                                                MessageType.CustomerLink_OFF)
            'Push内容設定
            Dim pushInfo As XmlPushInfo = Me.CreatePushInfo(inRowNoticeInfo.Message)
            '設定したものを格納し、通知APIをコール
            Using noticeData As New XmlNoticeData
                '現在時間データの格納
                noticeData.TransmissionDate = DateTimeFunc.Now(inDealerCode)
                '送信ユーザーデータ格納
                noticeData.AccountList.AddRange(sendToUsersListTcSvr)
                '通知履歴用のデータ格納
                noticeData.RequestNotice = requestNoticeTcSvr
                'Pushデータ格納
                noticeData.PushInfo = pushInfo

                LogServiceCommonBiz.OutputLog(17, "●■● 1.4.4.3 IC3040801呼び出し(TC・SVR) START")

                '通知処理実行
                Using ic3040801Biz As New IC3040801BusinessLogic

                    '通知処理実行
                    ic3040801Biz.NoticeDisplay(noticeData, NoticeDisposal.GeneralPurpose)

                End Using

                LogServiceCommonBiz.OutputLog(17, "●■● 1.4.4.3 IC3040801呼び出し(TC・SVR) END")

            End Using
        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    '2015/07/17 TMEJ 明瀬 タブレットSMB性能改善 通知WebService化 END

    ''' <summary>
    ''' 送信先アカウント情報作成メソッド
    ''' </summary>
    ''' <param name="toAccount">アカウント</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CreateAccount(ByVal toAccount As String) As XmlAccount
        Using account As New XmlAccount
            Dim usersClass As New Users
            Dim rowUsers As UsersDataSet.USERSRow
            'ユーザー情報の取得
            rowUsers = usersClass.GetUser(toAccount, DelFlgNone)
            '受信先のアカウント設定
            account.ToAccount = toAccount
            '受信者名設定
            account.ToAccountName = rowUsers.USERNAME

            Return account
        End Using
    End Function

    ''' <summary>
    ''' Push情報作成メソッド
    ''' </summary>
    ''' <param name="inDisplayContents">表示メッセージ</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CreatePushInfo(ByVal inDisplayContents As String) As XmlPushInfo

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START inDisplayContents={2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , inDisplayContents))

        'PUSH内容設定
        Using pushInfo As New XmlPushInfo

            'カテゴリータイプ設定
            pushInfo.PushCategory = NotifyPushCategory
            '表示位置設定
            pushInfo.PositionType = NotifyPotisionType
            '表示時間設定
            pushInfo.Time = NotifyTime
            '表示タイプ設定
            pushInfo.DisplayType = NotifyDispType
            '表示内容設定
            pushInfo.DisplayContents = inDisplayContents
            '色設定
            pushInfo.Color = NotifyColor
            '表示時関数設定
            pushInfo.DisplayFunction = NotifyDispFunction

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))

            Return pushInfo

        End Using
    End Function

    ''' <summary>
    ''' 通知履歴登録情報作成メソッド
    ''' </summary>
    ''' <param name="inRowNoticeInfo">送り情報行</param>
    ''' <param name="inStaffInfo">登録情報</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CreateRequestNotice(ByVal inRowNoticeInfo As TabletSmbCommonClassNoticeInfoRow, _
                                         ByVal inStaffInfo As StaffContext, _
                                         ByVal kindNumber As MessageType) As XmlRequestNotice

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Using requestNotice As New XmlRequestNotice

            '販売店コード設定
            requestNotice.DealerCode = inStaffInfo.DlrCD
            '店舗コード設定
            requestNotice.StoreCode = inStaffInfo.BrnCD
            'スタッフコード(送信元)設定
            requestNotice.FromAccount = inStaffInfo.Account
            'スタッフ名(送信元)設定
            requestNotice.FromAccountName = inStaffInfo.UserName
            '表示内容設定
            requestNotice.Message = Me.CreateNoticeRequestMessage(inRowNoticeInfo, kindNumber)
            'セッション設定値設定
            requestNotice.SessionValue = Me.CreateNoticeRequestSession(inRowNoticeInfo, kindNumber)

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} START" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return requestNotice
        End Using
    End Function

    '2015/07/17 TMEJ 明瀬 タブレットSMB性能改善 通知WebService化 START

    ''' <summary>
    ''' 通知履歴登録情報作成メソッド
    ''' </summary>
    ''' <param name="inRowNoticeInfo">送り情報行</param>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inBranchCode">店舗コード</param>
    ''' <param name="inStaffAccount">ログインスタッフアカウント</param>
    ''' <param name="inStaffName">ログインスタッフ名</param>
    ''' <param name="kindNumber"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CreateRequestNotice(ByVal inRowNoticeInfo As TabletSmbCommonClassNoticeInfoRow, _
                                         ByVal inDealerCode As String, _
                                         ByVal inBranchCode As String, _
                                         ByVal inStaffAccount As String, _
                                         ByVal inStaffName As String, _
                                         ByVal kindNumber As MessageType) As XmlRequestNotice

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Using requestNotice As New XmlRequestNotice

            '販売店コード設定
            requestNotice.DealerCode = inDealerCode
            '店舗コード設定
            requestNotice.StoreCode = inBranchCode
            'スタッフコード(送信元)設定
            requestNotice.FromAccount = inStaffAccount
            'スタッフ名(送信元)設定
            requestNotice.FromAccountName = inStaffName
            '表示内容設定
            requestNotice.Message = Me.CreateNoticeRequestMessage(inRowNoticeInfo, kindNumber)
            'セッション設定値設定
            requestNotice.SessionValue = Me.CreateNoticeRequestSession(inRowNoticeInfo, kindNumber)

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} START" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return requestNotice
        End Using
    End Function

    '2015/07/17 TMEJ 明瀬 タブレットSMB性能改善 通知WebService化 END

    ''' <summary>
    ''' 通知履歴用メッセージ作成メソッド
    ''' </summary>
    ''' <param name="inRowNoticeInfo">送り情報</param>
    ''' <param name="inKindNumber">メッセージ種別「0:顧客詳細画面リンクなし　1:顧客詳細画面リンクあり」</param>
    ''' <returns>作成したメッセージ文言</returns>
    ''' <history>
    ''' </history>
    ''' <remarks></remarks>
    Private Function CreateNoticeRequestMessage(ByVal inRowNoticeInfo As TabletSmbCommonClassNoticeInfoRow, _
                                                ByVal inKindNumber As MessageType) As String

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim roMessage As New StringBuilder
        Dim cstMessage As New StringBuilder
        Dim vclMessage As New StringBuilder

        ''RO番号のAタグを設定
        roMessage.Append(RoPreviewLink)
        roMessage.Append(inRowNoticeInfo.R_O)
        roMessage.Append(EndLikTag)

        'メッセージ組立：リンク(開始)作成
        '顧客詳細リンクありの場合
        If inKindNumber.Equals(MessageType.CustomerLink_ON) Then
            cstMessage.Append(CustomerCstLink)
            cstMessage.Append(inRowNoticeInfo.CST_NAME)
            cstMessage.Append(EndLikTag)
            'メッセージ組立：車両登録番号
            If Not (inRowNoticeInfo.IsVCLREGNONull) _
                AndAlso Not (String.IsNullOrEmpty(inRowNoticeInfo.VCLREGNO)) Then
                vclMessage.Append(CustomerVclLink)
                vclMessage.Append(inRowNoticeInfo.VCLREGNO)
                vclMessage.Append(EndLikTag)
            End If

        End If


        '戻り値設定
        'リンクを追加
        'ROプレビュー
        Dim notifyMessage As String = inRowNoticeInfo.Message.Replace(inRowNoticeInfo.R_O, roMessage.ToString)
        '顧客詳細
        If Not String.IsNullOrEmpty(cstMessage.ToString) Then
            notifyMessage = notifyMessage.Replace(inRowNoticeInfo.CST_NAME, cstMessage.ToString)
        End If
        '車両登録番号
        If Not String.IsNullOrEmpty(vclMessage.ToString) Then
            notifyMessage = notifyMessage.Replace(inRowNoticeInfo.VCLREGNO, vclMessage.ToString)
        End If


        '開放処理
        roMessage = Nothing
        cstMessage = Nothing
        vclMessage = Nothing

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} OUT:{2} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , notifyMessage))

        Return notifyMessage

    End Function

    ''' <summary>
    ''' 通知履歴用セッション情報作成メソッド
    ''' </summary>
    ''' <param name="inRowNoticeInfo">来店者情報表示欄</param>
    ''' <param name="inKindNumber">メッセージ種別「0:未取引客、1:自社客かつ車両登録No有、2:自社客かつ車両登録No無」</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Function CreateNoticeRequestSession(ByVal inRowNoticeInfo As TabletSmbCommonClassNoticeInfoRow, _
                                                ByVal inKindNumber As MessageType) As String

        Dim notifySession As String = String.Empty

        'メッセージ種別判定
        Select Case inKindNumber
            Case MessageType.CustomerLink_OFF
                '「0:顧客詳細リンクなし」の場合

                'R/Oプレビューのセッション情報を作成
                notifySession = CreateRoPreviewSession(inRowNoticeInfo)

            Case MessageType.CustomerLink_ON
                '「1:顧客詳細リンクあり」の場合

                'R/Oプレビューと顧客詳細のセッション情報を作成
                notifySession = CreateRoAndCstSession(inRowNoticeInfo)

        End Select

        Return notifySession

    End Function

    ''' <summary>
    ''' Roプレビュー遷移の通知用セッション情報作成メソッド
    ''' </summary>
    ''' <param name="inRowNoticeInfo">来店者情報欄表示情報</param>
    ''' <returns>戻り値</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Function CreateRoPreviewSession(ByVal inRowNoticeInfo As TabletSmbCommonClassNoticeInfoRow) As String

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim workSession As New StringBuilder

        'DMS販売店コードのセッション設定
        Me.SetSessionValueWord(workSession, _
                               SessionValueDearlerCode, _
                               inRowNoticeInfo.DearlerCode)

        'DMS店舗コードのセッション値作成
        Me.SetSessionValueWord(workSession, SessionValueBranchCode, inRowNoticeInfo.BranchCode)

        'LoginUserIDのセッション値作成
        Me.SetSessionValueWord(workSession, SessionValueLoginUserID, inRowNoticeInfo.LoginUserID)

        '来店管理番号のセッション値作成
        Me.SetSessionValueWord(workSession, SessionValueSAChipID, inRowNoticeInfo.SAChipID)


        'BASREZIDの設定
        If Not inRowNoticeInfo.IsBASREZIDNull Then
            'BASREZIDがある場合は設定
            'BASREZIDのセッション値作成
            Me.SetSessionValueWord(workSession, SessionValueBASREZID, inRowNoticeInfo.BASREZID)
        Else
            '値がない場合は空文字を設定
            Me.SetSessionValueWord(workSession, SessionValueBASREZID, "")
        End If

        'R_Oの設定
        If Not inRowNoticeInfo.IsR_ONull Then
            'R_Oがある場合は設定

            'R_Oのセッション値作成
            Me.SetSessionValueWord(workSession, SessionValueR_O, inRowNoticeInfo.R_O)
        Else
            '値がない場合は空文字を設定
            Me.SetSessionValueWord(workSession, SessionValueR_O, "")
        End If

        'SEQ_NOの設定
        If Not inRowNoticeInfo.IsSEQ_NONull Then
            'SEQ_NOがある場合は設定

            'SEQ_NOのセッション値作成
            Me.SetSessionValueWord(workSession, SessionValueSEQ_NO, inRowNoticeInfo.SEQ_NO)
        Else
            '値がない場合は空文字を設定
            Me.SetSessionValueWord(workSession, SessionValueSEQ_NO, "")
        End If

        'VIN_NOの設定
        If Not inRowNoticeInfo.IsVINNull Then
            'VIN_NOのセッション値作成
            Me.SetSessionValueWord(workSession, SessionValueVIN_NO, inRowNoticeInfo.VIN)
        Else
            '値がない場合は空文字を設定
            Me.SetSessionValueWord(workSession, SessionValueVIN_NO, "")
        End If

        'ViewModeのセッション値作成
        Me.SetSessionValueWord(workSession, SessionValueViewMode, "0")

        'Formatのセッション値作成
        Me.SetSessionValueWord(workSession, SessionValueFormat, "0")

        '入庫管理番号のセッション値作成
        Me.SetSessionValueWord(workSession, SessionValueSvcInNum, "")

        '入庫販売店コードのセッション値作成
        Me.SetSessionValueWord(workSession, SessionValueSvcInDlrCd, "")

        'DISP_NUMのセッション値作成
        Me.SetSessionValueWord(workSession, SessionValueDisp_Num, "13")

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return workSession.ToString

    End Function

    ''' <summary>
    ''' Roプレビューと顧客詳細遷移の通知用セッション情報作成メソッド
    ''' </summary>
    ''' <param name="inRowNoticeInfo">来店者情報欄表示情報</param>
    ''' <returns>戻り値</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Function CreateRoAndCstSession(ByVal inRowNoticeInfo As TabletSmbCommonClassNoticeInfoRow) As String

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim workSession As New StringBuilder

        'DMS販売店コードのセッション設定
        Me.SetSessionValueWord(workSession, _
                               SessionValueDearlerCode, _
                               inRowNoticeInfo.DearlerCode)
        'DMS店舗コードのセッション値作成
        Me.SetSessionValueWord(workSession, SessionValueBranchCode, inRowNoticeInfo.BranchCode)
        'LoginUserIDのセッション値作成
        Me.SetSessionValueWord(workSession, SessionValueLoginUserID, inRowNoticeInfo.LoginUserID)
        '来店管理番号のセッション値作成
        Me.SetSessionValueWord(workSession, SessionValueSAChipID, inRowNoticeInfo.SAChipID)
        'BASREZIDの設定
        If Not inRowNoticeInfo.IsBASREZIDNull Then
            'BASREZIDがある場合は設定
            'BASREZIDのセッション値作成
            Me.SetSessionValueWord(workSession, SessionValueBASREZID, inRowNoticeInfo.BASREZID)
        Else
            '値がない場合は空文字を設定
            Me.SetSessionValueWord(workSession, SessionValueBASREZID, "")
        End If
        'R_Oの設定
        If Not inRowNoticeInfo.IsR_ONull Then
            'R_Oがある場合は設定

            'R_Oのセッション値作成
            Me.SetSessionValueWord(workSession, SessionValueR_O, inRowNoticeInfo.R_O)
        Else
            '値がない場合は空文字を設定
            Me.SetSessionValueWord(workSession, SessionValueR_O, "")
        End If
        'SEQ_NOの設定
        If Not inRowNoticeInfo.IsSEQ_NONull Then
            'SEQ_NOがある場合は設定

            'SEQ_NOのセッション値作成
            Me.SetSessionValueWord(workSession, SessionValueSEQ_NO, inRowNoticeInfo.SEQ_NO)
        Else
            '値がない場合は空文字を設定
            Me.SetSessionValueWord(workSession, SessionValueSEQ_NO, "")
        End If
        'VIN_NOの設定
        If Not inRowNoticeInfo.IsVINNull Then
            'VIN_NOがある場合は設定
            'VIN_NOのセッション値作成
            Me.SetSessionValueWord(workSession, SessionValueVIN_NO, inRowNoticeInfo.VIN)
        Else
            '値がない場合は空文字を設定
            Me.SetSessionValueWord(workSession, SessionValueVIN_NO, "")
        End If
        'ViewModeのセッション値作成
        Me.SetSessionValueWord(workSession, SessionValueViewMode, "0")
        'Formatのセッション値作成
        Me.SetSessionValueWord(workSession, SessionValueFormat, "0")
        'DISP_NUMのセッション値作成
        Me.SetSessionValueWord(workSession, SessionValueDisp_Num, "13")

        'タブで分ける(顧客名のリンク)
        workSession.Append(vbTab)

        '顧客詳細のセッション値設定
        '基幹顧客IDのセッション設定
        workSession.Append(SessionValueDmsCstId)
        workSession.Append(inRowNoticeInfo.DMS_CST_ID)

        'VINのセッション設定
        Me.SetSessionValueWord(workSession, _
                               SessionValueVin, _
                               inRowNoticeInfo.VIN)

        'タブで分ける(車両番号のリンク)
        workSession.Append(vbTab)

        '顧客詳細のセッション値設定
        '基幹顧客IDのセッション設定
        workSession.Append(SessionValueDmsCstId)
        workSession.Append(inRowNoticeInfo.DMS_CST_ID)

        'VINのセッション設定
        Me.SetSessionValueWord(workSession, _
                               SessionValueVin, _
                               inRowNoticeInfo.VIN)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return workSession.ToString

    End Function

    ''' <summary>
    ''' SessionValue文字列作成
    ''' </summary>
    ''' <param name="workSession">追加元文字列</param>
    ''' <param name="SessionValueWord">追加するSESSIONKEY</param>
    ''' <param name="SessionValueData">追加するデータ</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SetSessionValueWord(ByVal workSession As StringBuilder, _
                                         ByVal SessionValueWord As String, _
                                         ByVal SessionValueData As String) As StringBuilder

        'カンマの設定
        If workSession.Length <> 0 Then
            'データがある場合

            '「,」を結合する
            workSession.Append(SessionValueKanma)

        End If

        'セッションキーを設定
        workSession.Append(SessionValueWord)

        'セッション値を設定
        workSession.Append(SessionValueData)


        Return workSession

    End Function

#End Region

#Region "基幹販売店・店舗コード取得"
    ''' <summary>
    ''' 基幹販売店・店舗コード取得
    ''' </summary>
    ''' <param name="inDlrCode">icrop販売店コード</param>
    ''' <param name="brnCode">icrop店舗コード</param>
    ''' <param name="account">ログインユーザー名</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDmsDlrBrnCode(ByVal inDlrCode As String, _
                                     ByVal brnCode As String, _
                                     Optional ByVal account As String = "") As ServiceCommonClassDataSet.DmsCodeMapRow
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} START inDlrCode={2} brnCode={3} account={4}" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name _
            , inDlrCode _
            , brnCode _
            , account))

        '基幹販売店・店舗コード
        Dim dmsDlrBrnTable As ServiceCommonClassDataSet.DmsCodeMapDataTable = Nothing
        Using srvCommonBiz As New ServiceCommonClassBusinessLogic
            dmsDlrBrnTable = srvCommonBiz.GetIcropToDmsCode(inDlrCode, _
                                                            ServiceCommonClassBusinessLogic.DmsCodeType.BranchCode, _
                                                            inDlrCode, _
                                                            brnCode, _
                                                            String.Empty, _
                                                            account)
            If dmsDlrBrnTable.Count <= 0 Then

                'データが取得できない場合はエラー
                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                           "{0}.Error, Failed to convert key dealer code.(No data found)", _
                                           MethodBase.GetCurrentMethod.Name))
                Return Nothing

            ElseIf 1 < dmsDlrBrnTable.Count Then
                'データが2件以上取得できた場合は一意に決定できないためエラー
                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                           "{0}.Error, Failed to convert key dealer code.(Non-unique)", _
                                           MethodBase.GetCurrentMethod.Name))
                Return Nothing

            End If
        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} END" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return dmsDlrBrnTable(0)
    End Function
#End Region

    '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END


    '2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発 START
#Region "未開始Job存在判定"
    ''' <summary>
    ''' 未開始Job存在判定
    ''' </summary>
    ''' <param name="inJobDtlId">作業内容ID</param>
    ''' <returns>true：開始前Jobが存在する、false：開始前Jobが存在しない</returns>
    ''' <remarks></remarks>
    Public Function HasBeforeStartJob(ByVal inJobDtlId As Decimal) As Boolean

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} START inJobDtlId={2}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , inJobDtlId))

        Dim dtJobInstruct As TabletSmbCommonClassJobInstructDataTable = Nothing
        Dim dtJobInstructResult As TabletSmbCommonClassJobStatusDataTable = Nothing

        Using ta As New TabletSMBCommonClassDataAdapter

            '該当作業に紐づく全て作業を取得する(作業指示テーブルから)
            dtJobInstruct = ta.GetJobInstructIdAndSeqByJobDtlId(inJobDtlId)

            '該当作業に紐づく全て作業実績を取得する(作業実績テーブルから)
            dtJobInstructResult = ta.GetAllJobRsltInfoByJobDtlId(inJobDtlId)

        End Using


        If dtJobInstruct.Count <> dtJobInstructResult.Count Then
            '未開始チップがある

            'Trueを戻す
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                    , "{0}.{1} HasBeforeStartJob=True END" _
                                    , Me.GetType.ToString _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return True

        Else
            '未開始チップがない

            'Falseを戻す
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                    , "{0}.{1} HasBeforeStartJob=False END" _
                                    , Me.GetType.ToString _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return False

        End If

    End Function

#End Region

#Region "中断Job存在判定"
    ''' <summary>
    ''' 中断Job存在判定
    ''' </summary>
    ''' <param name="inJobDtlId">作業内容ID</param>
    ''' <returns>true：中断Jobが存在する、false：中断Jobが存在しない</returns>
    ''' <remarks></remarks>
    Public Function HasStopJob(ByVal inJobDtlId As Decimal) As Boolean
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} START inJobDtlId={2}" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name _
            , inJobDtlId))
        '返却用変数宣言[true：中断Jobが存在する、false：中断Jobが存在しない]
        Dim stopJobFlg As Boolean = False

        Using tabletSMBCommonDa As New TabletSMBCommonClassDataAdapter

            '中断JOBの件数取得
            Dim dtStopJobCount As TabletSmbCommonClassNumberValueDataTable = tabletSMBCommonDa.GetStopJobCount(inJobDtlId)

            If 0 < dtStopJobCount(0).COL1 Then
                '中断JOB件数が一件以上あれば、フラグを「true：中断Jobが存在する」にする
                stopJobFlg = True

            End If
        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} HasStopJob={2} END" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name _
            , stopJobFlg.ToString))

        Return stopJobFlg
    End Function
#End Region

#Region "Jobステータスの取得"
    ''' <summary>
    ''' Jobステータスの取得
    ''' </summary>
    ''' <param name="inJobDtlId">作業内容ID</param>
    ''' <param name="inJobInstructId">作業指示ID</param>
    ''' <param name="inJobInstructSeq">作業指示枝番</param>
    ''' <returns>Jobステータス</returns>
    ''' <remarks></remarks>
    Public Function GetJobStatus(ByVal inJobDtlId As Decimal, _
                                 ByVal inJobInstructId As String, _
                                 ByVal inJobInstructSeq As Long) As String
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} START inJobDtlId={2} inServiceInId={3} inJobInstructSeq={4}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , inJobDtlId _
                                , inJobInstructId _
                                , inJobInstructSeq))

        '返却用JobステータスDT
        Dim dtJobStatus As TabletSmbCommonClassJobStatusDataTable = Nothing

        Using tabletSMBCommonDa As New TabletSMBCommonClassDataAdapter

            'Jobステータスデータテーブル取得
            dtJobStatus = tabletSMBCommonDa.GetSingleJobStatus(inJobDtlId, inJobInstructId, inJobInstructSeq)

        End Using

        '開始前で初期化する(3未開始Jobを初期値として設定)
        Dim jobStatus As String = JobStatusBeforeStart

        '作業実績データがあれば、作業ステータスに作業実績データ.作業ステータスを設定する
        If 0 < dtJobStatus.Count Then

            jobStatus = dtJobStatus(0).JOB_STATUS

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} END JobStatus={2}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , jobStatus))

        Return jobStatus

    End Function


    ''' <summary>
    ''' Jobステータスの取得
    ''' </summary>
    ''' <param name="inJobDtlId">作業内容ID</param>
    ''' <param name="inJobInstructId">作業指示ID</param>
    ''' <param name="inJobInstructSeq">作業指示枝番</param>
    ''' <returns>Jobステータス</returns>
    ''' <remarks></remarks>
    Public Function GetJobStatusDataTable(ByVal inJobDtlId As Decimal, _
                                          ByVal inJobInstructId As String, _
                                          ByVal inJobInstructSeq As Long) As TabletSmbCommonClassJobStatusDataTable
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} START inJobDtlId={2} inServiceInId={3} inJobInstructSeq={4}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , inJobDtlId _
                                , inJobInstructId _
                                , inJobInstructSeq))

        '返却用JobステータスDT
        Dim dtJobStatus As TabletSmbCommonClassJobStatusDataTable = Nothing

        Using tabletSMBCommonDa As New TabletSMBCommonClassDataAdapter

            'Jobステータスデータテーブル取得
            dtJobStatus = tabletSMBCommonDa.GetSingleJobStatus(inJobDtlId, inJobInstructId, inJobInstructSeq)

        End Using


        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} END " _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return dtJobStatus

    End Function

#End Region
    '2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発 END

    '2014/12/04 TMEJ 丁 新プラットフォーム版i-CROP仕様変更対応（サービス入庫実績更新処理変更） START
#Region "納車実績日時を取得"
    ''' <summary>
    ''' 納車実績日時を取得
    ''' </summary>
    ''' <param name="inServiceInId">サービス入庫ID</param>
    ''' <returns>実績入庫日時</returns>
    ''' <remarks></remarks>
    Public Function GetRsltDeliDate(ByVal inServiceInId As Decimal) As Date
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} START inServiceInId={2}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , inServiceInId))

        '返却用納車実績日時DT
        Dim dtRsltDeliDate As TabletSmbCommonClassRsltDeliDateTimeDataTable = Nothing
        Dim rsltDeliDateTime As Date

        Using tabletSMBCommonDa As New TabletSMBCommonClassDataAdapter

            '納車実績日時テーブル取得
            dtRsltDeliDate = tabletSMBCommonDa.GetRsltDeliDateTime(inServiceInId)

        End Using

        '納車実績日時データがあれば、納車実績日時データ.納車実績日時を設定する
        If 0 < dtRsltDeliDate.Count Then

            rsltDeliDateTime = dtRsltDeliDate(0).RSLT_DELI_DATETIME
        Else
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} ERROR NoData inServiceInId={2}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                inServiceInId))

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} END rsltDeliDateTime={2}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , rsltDeliDateTime))

        Return rsltDeliDateTime

    End Function
#End Region
    '2014/12/04 TMEJ 丁 新プラットフォーム版i-CROP仕様変更対応（サービス入庫実績更新処理変更） END

    '2014/12/17 TMEJ 小澤 IT9824_NextSTEPサービス 洗車用端末開発向けた評価用アプリ作成 START

    ''' <summary>
    ''' 通知用の情報取得
    ''' </summary>
    ''' <param name="inServiceinId">サービス入庫ID</param>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inBranchCode">店舗コード</param>
    ''' <returns>通知用情報</returns>
    ''' <remarks></remarks>
    Public Function GetSendNoticeInfo(ByVal inServiceinId As Decimal, _
                                      ByVal inDealerCode As String, _
                                      ByVal inBranchCode As String) As TabletSmbCommonClassNoticeInfoDataTable
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START IN:inServiceinId = {2},inDealerCode = {3},inBranchCode = {4}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , inServiceinId _
                    , inDealerCode _
                    , inBranchCode))

        '戻り値
        Dim returnNoticeInfo As TabletSmbCommonClassNoticeInfoDataTable

        Using tabletSMBCommonDataAdapter As New TabletSMBCommonClassDataAdapter
            '通知用の情報取得
            returnNoticeInfo = tabletSMBCommonDataAdapter.GetNoticeInfo(inServiceinId, _
                                                                        inDealerCode, _
                                                                        inBranchCode)


        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
        Return returnNoticeInfo

    End Function

    '2014/12/17 TMEJ 小澤 IT9824_NextSTEPサービス 洗車用端末開発向けた評価用アプリ作成 END

    '2015/02/09 TMEJ 範 DMS連携版サービスタブレット SMB納車予定時刻通知機能開発 START
    ''' <summary>
    ''' 指定サービス入庫IDに最大のストール利用IDを取得する
    ''' </summary>
    ''' <param name="inServiceInId">指定サービス入庫ID</param>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inBranchCode">店舗コード</param>
    ''' <returns>最大のストール利用ID</returns>
    ''' <remarks></remarks>
    Public Function GetMaxStallUseIdGroupByServiceId(ByVal inServiceInId As Decimal, _
                                                     ByVal inDealerCode As String, _
                                                     ByVal inBranchCode As String) As Decimal

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} Start inServiceInId={2}, inDealerCode={3}, inBranchCode={4}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , inServiceInId _
                                , inDealerCode _
                                , inBranchCode))

        'ストール利用IDに0を設定する
        Dim maxStallUseId As Decimal = 0

        Using ta As New TabletSMBCommonClassDataAdapter

            '指定サービス入庫IDに最大のストール利用IDを取得する
            Dim dtStallUseId As TabletSmbCommonClassNumberValueDataTable = _
                ta.GetMaxStallUseIdGroupByServiceId(inServiceInId, _
                                                    inDealerCode, _
                                                    inBranchCode)

            If dtStallUseId.Count > 0 Then
                '取得できた場合

                'ストール利用IDに設定
                maxStallUseId = dtStallUseId(0).COL1

            End If

        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} Start maxStallUseId={2}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , maxStallUseId))

        Return maxStallUseId

    End Function
    '2015/02/09 TMEJ 範 DMS連携版サービスタブレット SMB納車予定時刻通知機能開発 END

#Region "強制納車処理"

    ''' <summary>
    ''' 強制納車処理
    ''' </summary>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inBranchCode">店舗コード</param>
    ''' <param name="inServiceinId">サービス入庫ID</param>
    ''' <param name="inRepairOrderNum">RO番号</param>
    ''' <param name="inResultDeliveryDate">実績納車日時</param>
    ''' <param name="inAccount">アカウント</param>
    ''' <param name="inNowDate">現在日時</param>
    ''' <param name="inSystemId">システムID</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Public Function ForceDeliverd(ByVal inDealerCode As String, _
                                  ByVal inBranchCode As String, _
                                  ByVal inServiceinId As Decimal, _
                                  ByVal inRepairOrderNum As String, _
                                  ByVal inResultDeliveryDate As Date, _
                                  ByVal inAccount As String, _
                                  ByVal inNowDate As Date, _
                                  ByVal inSystemId As String) As Integer
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} START inDealerCode:{2} inBranchCode:{3} inServiceinId:{4} inRepairOrderNum:{5} inResultDeliveryDate:{6} inAccount:{7} inNowDate:{8} inSystemId:{9} " _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name _
            , inDealerCode _
            , inBranchCode _
            , inServiceinId.ToString(CultureInfo.CurrentCulture) _
            , inRepairOrderNum _
            , inResultDeliveryDate.ToString(CultureInfo.CurrentCulture) _
            , inAccount _
            , inNowDate.ToString(CultureInfo.CurrentCulture) _
            , inSystemId))

        '戻り値
        Dim returnCode As ActionResult = ActionResult.Success

        Using da As New TabletSMBCommonClassDataAdapter

            'サービス入庫テーブルの強制納車更新
            Dim serviceinUpdateCount As Integer = da.UpdateServiceinForceDeliverd(inServiceinId, _
                                                                                  inResultDeliveryDate)

            '更新件数のチェック
            If serviceinUpdateCount = 0 Then
                '0件の場合
                '予期せぬエラー「22」を返却
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END RETURN:CODE={2} [inServiceinId:{3} inResultDeliveryDate:{4}]" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , ActionResult.ExceptionError _
                    , inServiceinId.ToString(CultureInfo.CurrentCulture) _
                    , inResultDeliveryDate.ToString(CultureInfo.CurrentCulture)))
                Return ActionResult.ExceptionError

            End If

            '作業内容テーブルの強制納車更新
            Dim jobDetailUpdateCount As Integer = da.UpdateJobDetailForceDeliverd(inServiceinId, _
                                                                                  inAccount, _
                                                                                  inNowDate, _
                                                                                  inSystemId)

            '更新件数のチェック
            If jobDetailUpdateCount = 0 Then
                '0件の場合
                '予期せぬエラー「22」を返却
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END RETURN:CODE={2} [inServiceinId:{3} inAccount:{4} inNowDate:{5} inSystemId:{6}]" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , ActionResult.ExceptionError _
                    , inServiceinId.ToString(CultureInfo.CurrentCulture) _
                    , inAccount _
                    , inNowDate.ToString(CultureInfo.CurrentCulture) _
                    , inSystemId))
                Return ActionResult.ExceptionError

            End If

            'RO情報テーブルの強制納車更新
            Dim roInfoUpdateCount As Integer = da.UpdateROInfoForceDeliverd(inDealerCode, _
                                                                            inBranchCode, _
                                                                            inRepairOrderNum, _
                                                                            inAccount, _
                                                                            inNowDate, _
                                                                            inSystemId)

            '更新件数のチェック
            If roInfoUpdateCount = 0 Then
                '0件の場合
                '予期せぬエラー「22」を返却
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END RETURN:CODE={2} [inDealerCode:{3} inBranchCode:{4} inRepairOrderNum:{5} inAccount:{6} inNowDate:{7} inSystemId:{8}]" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , ActionResult.ExceptionError _
                    , inDealerCode _
                    , inBranchCode _
                    , inRepairOrderNum _
                    , inAccount _
                    , inNowDate.ToString(CultureInfo.CurrentCulture) _
                    , inSystemId))
                Return ActionResult.ExceptionError

            End If

        End Using

        'サービスDMS納車実績ワークの削除
        Using bizServiceCommonClass As New ServiceCommonClassBusinessLogic
            'サービスDMS納車実績ワークの削除処理実施
            bizServiceCommonClass.DeleteWorkServiceDmsResultDelivery(inServiceinId)

        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} END RETURN:CODE={2}" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name _
            , returnCode))
        Return returnCode

    End Function


#End Region

    ' 2020/01/08 NSK 鈴木 TR-V4-BMS-20191001-001 (トライ店システム評価)コールセンター業務における洗車要否判断の効率化検証 START
#Region "サービス分類情報取得"

    ''' <summary>
    ''' ストールに紐付くサービス分類情報取得
    ''' </summary>
    ''' <param name="stallId">ストールID</param>
    ''' <returns>サービス分類情報</returns>
    ''' <remarks></remarks>
    ''' <history></history>
    Public Function GetSvcClassInfo(ByVal stallId As Decimal) As TabletSmbCommonClassServiceClassRow

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} START StallId:{2} " _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name _
            , stallId.ToString(CultureInfo.CurrentCulture)))

        Dim rowServiceClass As TabletSmbCommonClassServiceClassRow = Nothing

        Using ta As New TabletSMBCommonClassDataSetTableAdapters.TabletSMBCommonClassDataAdapter
            Dim dtServiceClass As TabletSmbCommonClassServiceClassDataTable = ta.GetSvcClassInfo(stallId)

            If (dtServiceClass IsNot Nothing) AndAlso (0 < dtServiceClass.Count) Then
                rowServiceClass = dtServiceClass.First()
            End If
        End Using

        Return rowServiceClass

    End Function

#End Region
    ' 2020/01/08 NSK 鈴木 TR-V4-BMS-20191001-001 (トライ店システム評価)コールセンター業務における洗車要否判断の効率化検証 END

#Region "ログ出力用"
    '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
    ' ''' <summary>
    ' ''' DataRow内の項目を列挙(ログ出力用)
    ' ''' </summary>
    ' ''' <param name="args">ログ項目のコレクション</param>
    ' ''' <param name="row">対象となるDataRow</param>
    ' ''' <remarks></remarks>
    'Private Sub AddLogData(ByVal args As List(Of String), ByVal row As DataRow)
    '    For Each column As DataColumn In row.Table.Columns
    '        If row.IsNull(column.ColumnName) Then
    '            args.Add(String.Format(CultureInfo.CurrentCulture, "{0} = NULL", column.ColumnName))
    '        Else
    '            args.Add(String.Format(CultureInfo.CurrentCulture, "{0} = {1}", column.ColumnName, row(column.ColumnName)))
    '        End If
    '    Next
    'End Sub
    '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

    ''' <summary>
    ''' テーブルのログ出力(IF戻り値用)
    ''' </summary>
    ''' <param name="dt">戻り値(DataTable)</param>
    ''' <param name="ifName">使用IF名</param>
    ''' <remarks></remarks>
    Private Sub OutPutIFLog(ByVal dt As DataTable, ByVal ifName As String)

        If dt Is Nothing Then
            Return
        End If

        Logger.Info(ifName + " Result START " + " OutPutCount: " + (dt.Rows.Count).ToString(CultureInfo.InvariantCulture))

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

        Logger.Info(ifName + " Result END ")

    End Sub

    '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
    ' ''' <summary>
    ' ''' String()をStringに変更
    ' ''' </summary>
    ' ''' <param name="stringArray">変更したいString()</param>
    ' ''' <returns>変更されたString</returns>
    ' ''' <remarks></remarks>
    'Private Function ConvertStringArrayToString(ByVal stringArray As String()) As String

    '    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S" _
    '                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    '    If IsNothing(stringArray) OrElse stringArray.Count = 0 Then
    '        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E. Array is empty." _
    '            , System.Reflection.MethodBase.GetCurrentMethod.Name))
    '        Return ""
    '    End If

    '    Dim sbReturn As New StringBuilder
    '    With sbReturn
    '        For Each stringData As String In stringArray
    '            .Append(stringData)
    '            .Append(",")
    '        Next
    '    End With
    '    '最後の,を削除する
    '    Dim strReturn As String = sbReturn.ToString()
    '    strReturn = strReturn.Substring(0, strReturn.Length - 1)
    '    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E. retrun={1}" _
    '                , System.Reflection.MethodBase.GetCurrentMethod.Name, strReturn))
    '    Return strReturn
    'End Function

    ''' <summary>
    ''' StringテーブルをStringに変更
    ''' </summary>
    ''' <param name="stringTbl">変更したいDecimalテーブル</param>
    ''' <returns>変更されたString</returns>
    ''' <remarks></remarks>
    Private Function ConvertNumberTableToString(ByVal stringTbl As TabletSmbCommonClassNumberValueDataTable) As String

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S" _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))

        If IsNothing(stringTbl) OrElse stringTbl.Count = 0 Then
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E. DecimalTable is empty." _
                , System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return ""
        End If

        Dim sbReturn As New StringBuilder
        With sbReturn
            For Each numberData As TabletSmbCommonClassNumberValueRow In stringTbl.Rows
                .Append(numberData.COL1.ToString(CultureInfo.InvariantCulture))
                .Append(",")
            Next
        End With
        '最後の,を削除する
        Dim strReturn As String = sbReturn.ToString()
        strReturn = strReturn.Substring(0, strReturn.Length - 1)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E. retrun={1}" _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name, strReturn))
        Return strReturn
    End Function

    '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

    ''' <summary>
    ''' String()をStringに変更
    ''' </summary>
    ''' <param name="stringArray">変更したいString()</param>
    ''' <returns>変更されたString</returns>
    ''' <remarks></remarks>
    Private Function ConvertStringArrayToString(ByVal stringArray As List(Of String)) As String

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S" _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))

        If IsNothing(stringArray) OrElse stringArray.Count = 0 Then
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E. Array is empty." _
                , System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return ""
        End If

        Dim sbReturn As New StringBuilder
        With sbReturn
            For Each stringData As String In stringArray
                .Append("N'")
                .Append(stringData)
                .Append("'")
                .Append(",")
            Next
        End With
        '最後の,を削除する
        Dim strReturn As String = sbReturn.ToString()
        strReturn = strReturn.Substring(0, strReturn.Length - 1)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E. retrun={1}" _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name, strReturn))
        Return strReturn
    End Function
#End Region

    '2019/07/30 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
#Region "作業終了日時情報のDTOクラス"
    ''' <summary>
    ''' 作業終了日時情報のDTOクラス
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ServiceEndDateTimeData

        ''' <summary>
        ''' 作業終了日時
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ServiceEndDateTime As Date

        ''' <summary>
        ''' 休憩取得フラグ
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property RestFlg As String

    End Class
#End Region
    '2019/07/30 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END

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

    '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
#Region "休憩自動判定有無の判別"
    ''' <summary>
    ''' 休憩の自動判定を行うか否かを設定
    ''' </summary>
    ''' <returns>休憩取得自動判定フラグ True:自動判定する</returns>
    ''' <remarks></remarks>
    Public Function IsRestAutoJudge() As Boolean

        '休憩取得自動判定フラグ
        Dim autoJudgeFlg = String.Empty

        Using dealerEnvBiz As New ServiceCommonClassBusinessLogic
            autoJudgeFlg = dealerEnvBiz.GetDlrSystemSettingValueBySettingName(RestAutoJudgeFlg)
        End Using

        '自動判定する場合、trueを戻す
        If RestAutoJudge.Equals(autoJudgeFlg) Then
            Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0}.{1}.END IsRestAutoJudge return true. " _
                        , Me.GetType.ToString, MethodBase.GetCurrentMethod.Name))
            Return True
        Else
            Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0}.{1}.END IsRestAutoJudge return false. " _
                        , Me.GetType.ToString, MethodBase.GetCurrentMethod.Name))
            Return False
        End If

    End Function
#End Region
    '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END

End Class
