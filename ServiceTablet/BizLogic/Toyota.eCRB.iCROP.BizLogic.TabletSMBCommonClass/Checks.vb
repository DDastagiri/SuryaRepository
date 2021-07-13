'-------------------------------------------------------------------------
'Checks.vb
'-------------------------------------------------------------------------
'機能：タブレットSMB共通関数
'補足：チェック関数
'作成：2013/08/14 TMEJ 張 タブレット版SMB機能開発(工程管理)
'更新：2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発
'更新：2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発
'更新：2014/02/13 TMEJ 小澤 【開発】IT9611_次世代サービス 工程管理機能開発
'更新：2013/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発
'更新：2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発
'更新：2014/09/12 TMEJ 張 BTS-392「TopServのROとSMBのJOBが不一致」対応
'更新：2014/09/25 TMEJ 張 BTS-180 「洗車中に関連チップ作成すると予期せぬエラーメッセージ」対応
'更新：2015/02/13 TMEJ 範 新プラットフォーム版i-CROP仕様変更対応 (SMBチップ検索及び作業中断機能強化)
'更新：2017/09/13 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加 
'更新：2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証
'更新：2019/07/22 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証
'更新：2019/08/09 NSK 鈴木 DLR-002 子チップを着工指示した時にエラー発生（子チップから開始でアラート表示を修正）
'更新：2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証
'更新：
'─────────────────────────────────────

Imports System.Xml
Imports System.Net
Imports System.Web
Imports System.IO
Imports System.Globalization
Imports System.Reflection
Imports System.Xml.Serialization
Imports System.Text.RegularExpressions
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.DataAccess.TabletSMBCommonClassDataSet
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.DataAccess.TabletSMBCommonClassDataSetTableAdapters
Imports Toyota.eCRB.CommonUtility.SMBCommonClass.Api.BizLogic
Imports System.Text

'2014/02/13 TMEJ 小澤 【開発】IT9611_次世代サービス 工程管理機能開発 START
'Imports Toyota.eCRB.DMSLinkage.AddRepair.BizLogic.IC3800804
'Imports Toyota.eCRB.DMSLinkage.AddRepair.DataAccess.IC3800804.IC3800804DataSet
'2014/02/13 TMEJ 小澤 【開発】IT9611_次世代サービス 工程管理機能開発 END

Partial Class TabletSMBCommonClassBusinessLogic

#Region "チップステータスチェック"
    ''' <summary>
    ''' 本予約に遷移できるか否かチェックを行います
    ''' </summary>
    ''' <param name="svcStatus">サービスステータス</param>
    ''' <param name="stallUseStatus">ストール利用ステータス</param>
    ''' <param name="containsMidfinishChip">日跨ぎ終了を含むか否か</param>
    ''' <param name="resvStatus">予約ステータス</param>
    ''' <param name="acceptanceType">受付区分</param>
    ''' <returns>遷移可能な場合は<c>true</c>、それ以外は<c>false</c></returns>
    ''' <remarks></remarks>
    Private Function CanReserve(ByVal svcStatus As String, _
                               ByVal stallUseStatus As String, _
                               ByVal containsMidfinishChip As Boolean, _
                               ByVal resvStatus As String, _
                               ByVal acceptanceType As String
                               ) As Boolean
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. svcStatus={1}, stallUseStatus={2}, containsMidfinishChip={3}, resvStatus={4}, acceptanceType={5}" _
                                , MethodBase.GetCurrentMethod.Name, svcStatus, stallUseStatus, containsMidfinishChip, resvStatus, acceptanceType))
        ' サービスステータスが「未入庫」「未来店客」「着工指示待ち」「作業開始待ち」であること。
        If svcStatus.Equals(SvcStatusNotCarin) _
            Or svcStatus.Equals(SvcStatusNoShow) _
            Or svcStatus.Equals(SvcStatusWorkOrderWait) _
            Or svcStatus.Equals(SvcStatusStartwait) Then
            ' ストール利用ステータスが「着工指示待ち」「作業開始待ち」「未来店客」であること。
            If stallUseStatus.Equals(StalluseStatusWorkOrderWait) _
                Or stallUseStatus.Equals(StalluseStatusStartWait) _
                Or stallUseStatus.Equals(StalluseStatusNoshow) Then
                ' 日跨ぎ終了を含まない場合のみ
                If Not containsMidfinishChip Then
                    If resvStatus.Equals(ResvStatusTentative) Then
                        'WalkInでないこと
                        If Not acceptanceType.Equals(AcceptanceTypeWalkin) Then
                            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Return True", MethodBase.GetCurrentMethod.Name))
                            Return True
                        End If
                    End If
                End If
            End If
        End If
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Return False", MethodBase.GetCurrentMethod.Name))
        Return False
    End Function

    ''' <summary>
    ''' 仮予約に遷移できるか否かチェックを行います
    ''' </summary>
    ''' <param name="svcStatus">サービスステータス</param>
    ''' <param name="stallUseStatus">ストール利用ステータス</param>
    ''' <param name="containsMidfinishChip">日跨ぎ終了を含むか否か</param>
    ''' <param name="resvStatus">予約ステータス</param>
    ''' <param name="acceptanceType">受付区分</param>
    ''' <returns>遷移可能な場合は<c>true</c>、それ以外は<c>false</c></returns>
    ''' <remarks></remarks>
    Private Function CanTentativeReserve(ByVal svcStatus As String, _
                               ByVal stallUseStatus As String, _
                               ByVal containsMidfinishChip As Boolean, _
                               ByVal resvStatus As String, _
                               ByVal acceptanceType As String
                               ) As Boolean
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. svcStatus={1}, stallUseStatus={2}, containsMidfinishChip={3}, resvStatus={4}, acceptanceType={5}" _
                    , MethodBase.GetCurrentMethod.Name, svcStatus, stallUseStatus, containsMidfinishChip, resvStatus, acceptanceType))

        ' サービスステータスが「未入庫」「未来店客」「着工指示待ち」「作業開始待ち」であること。
        If svcStatus.Equals(SvcStatusNotCarin) _
            Or svcStatus.Equals(SvcStatusNoShow) _
            Or svcStatus.Equals(SvcStatusWorkOrderWait) _
            Or svcStatus.Equals(SvcStatusStartwait) Then
            ' ストール利用ステータスが「着工指示待ち」「作業開始待ち」「未来店客」であること。
            If stallUseStatus.Equals(StalluseStatusWorkOrderWait) _
                Or stallUseStatus.Equals(StalluseStatusStartWait) _
                Or stallUseStatus.Equals(StalluseStatusNoshow) Then
                ' 日跨ぎ終了を含まない場合のみ
                If Not containsMidfinishChip Then
                    ' 予約ステータスが「本予約」であること。
                    If resvStatus.Equals(ResvStatusConfirmed) Then
                        'WalkInでないこと
                        If Not acceptanceType.Equals(AcceptanceTypeWalkin) Then
                            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Return True", MethodBase.GetCurrentMethod.Name))
                            Return True
                        End If
                    End If
                End If
            End If
        End If
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Return False", MethodBase.GetCurrentMethod.Name))
        Return False
    End Function

    ''' <summary>
    ''' 入庫に遷移できるか否かチェックを行います
    ''' </summary>
    ''' <param name="svcStatus">サービスステータス</param>
    ''' <param name="tempFlg">仮置きフラグ</param>
    ''' <param name="stallId">ストールID</param>
    ''' <param name="scheStartDateTime">予定開始日時</param>
    ''' <returns>遷移可能な場合は<c>true</c>、それ以外は<c>false</c></returns>
    ''' <remarks></remarks>
    Private Function CanCarIn(ByVal svcStatus As String, _
                               ByVal tempFlg As String, _
                               ByVal stallId As Decimal, _
                               ByVal scheStartDateTime As Date
                               ) As Boolean
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. svcStatus={1}, tempFlg={2}, stallId={3}, scheStartDateTime={4}" _
                        , MethodBase.GetCurrentMethod.Name, svcStatus, tempFlg, stallId, scheStartDateTime))

        ' サービスステータスが「未入庫」であること
        If svcStatus.Equals(SvcStatusNotCarin) Then
            ' 仮置き・飛び込み客でないこと
            If IsNotTempAndWalkIn(tempFlg, stallId) Then
                ' 予定開始日時が未設定値以外であること
                If Not IsDefaultValue(scheStartDateTime) Then
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Return True", MethodBase.GetCurrentMethod.Name))
                    Return True
                End If
            End If
        End If
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Return False", MethodBase.GetCurrentMethod.Name))
        Return False
    End Function

    ''' <summary>
    ''' 入庫取消に遷移できるか否かチェックを行います
    ''' </summary>
    ''' <param name="svcStatus">サービスステータス</param>
    ''' <param name="containsResultChip">実績チップ（中断再配置等）を含むか否か</param>
    ''' <param name="tempFlg">仮置きフラグ</param>
    ''' <param name="stallId">ストールID</param>
    ''' <param name="scheStartDateTime">予定開始日時</param>
    ''' <returns>遷移可能な場合は<c>true</c>、それ以外は<c>false</c></returns>
    ''' <remarks></remarks>
    Private Function CanCancelCarIn(ByVal svcStatus As String, _
                                ByVal containsResultChip As Boolean, _
                                ByVal tempFlg As String, _
                                ByVal stallId As Decimal, _
                                ByVal scheStartDateTime As Date
                               ) As Boolean

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. svcStatus={1}, containsResultChip={2}, tempFlg={3}, stallId={4}, scheStartDateTime={5}" _
            , MethodBase.GetCurrentMethod.Name, svcStatus, containsResultChip, tempFlg, stallId, scheStartDateTime))

        '実績チップを含まないこと
        If Not containsResultChip Then
            'サービスステータスが「着工指示待ち」「作業開始待ち」であること
            If svcStatus.Equals(SvcStatusWorkOrderWait) Or svcStatus.Equals(SvcStatusStartwait) Then
                ' 仮置き・飛び込み客でないこと
                If IsNotTempAndWalkIn(tempFlg, stallId) Then
                    ' 予定開始日時が未設定値以外であること
                    If Not IsDefaultValue(scheStartDateTime) Then
                        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Return True", MethodBase.GetCurrentMethod.Name))
                        Return True
                    End If
                End If
            End If
        End If
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Return False", MethodBase.GetCurrentMethod.Name))
        Return False
    End Function

    ''' <summary>
    ''' チップ移動に遷移できるか否かチェックを行います
    ''' </summary>
    ''' <param name="rsltEndDatetime">実績終了日時</param>
    ''' <param name="stallUseStatus">ストール利用ステータス</param>
    ''' <returns>遷移可能な場合は<c>true</c>、それ以外は<c>false</c></returns>
    ''' <remarks></remarks>
    Private Function CanMoveAndResize(ByVal rsltEndDatetime As Date, _
                                ByVal stallUseStatus As String) As Boolean
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. rsltEndDatetime={1}, stallUseStatus={2}" _
                    , MethodBase.GetCurrentMethod.Name, rsltEndDatetime, stallUseStatus))

        '実績終了日時が未設定値であること。又はストール利用ステータスが「中断」であること
        If IsDefaultValue(rsltEndDatetime) Or stallUseStatus.Equals(StalluseStatusStop) Then
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Return True", MethodBase.GetCurrentMethod.Name))
            Return True
        End If
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Return False", MethodBase.GetCurrentMethod.Name))
        Return False
    End Function

    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    ' ''' <summary>
    ' ''' 作業開始できるかチェックします
    ' ''' </summary>
    ' ''' <param name="objStaffContext">スタッフ情報</param>
    ' ''' <param name="serviceInId">サービス入庫ID</param>
    ' ''' <returns>作業開始できる場合<c>true</c>、それ以外の場合<c>false</c></returns>
    ' ''' <remarks></remarks>
    'Private Function CanStart(ByVal objStaffContext As StaffContext, ByVal serviceInId As Decimal) As Boolean
    '    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. serviceInId={1}" _
    '                , MethodBase.GetCurrentMethod.Name, serviceInId))

    '    Using ta As New TabletSMBCommonClassDataAdapter
    '        Dim dtStallUse As TabletSmbCommonClassStringValueDataTable = ta.GetlStallUseStatusListBySvcInId(objStaffContext.DlrCD, objStaffContext.BrnCD, serviceInId)
    '        'ストール利用．ストール利用ステータスが「02：作業中」「04：作業計画の一部の作業が中断」の場合
    '        For Each stallUseRow As TabletSmbCommonClassStringValueRow In dtStallUse.Rows
    '            If stallUseRow.COL1.ToString().Equals(StalluseStatusStart) _
    '                Or stallUseRow.COL1.ToString().Equals(StalluseStatusStartIncludeStopJob) Then
    '                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Return False", MethodBase.GetCurrentMethod.Name))
    '                Return False
    '            End If
    '        Next
    '        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Return True", MethodBase.GetCurrentMethod.Name))
    '        Return True
    '    End Using
    'End Function
    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

    ''' <summary>
    ''' 作業開始に遷移できるか否かチェックを行います。
    ''' </summary>
    ''' <param name="svcStatus">サービスステータス</param>
    ''' <param name="stallUseStatus">ストール利用ステータス</param>
    ''' <param name="tempFlg">仮置きフラグ</param>
    ''' <param name="stallId">ストールID</param>
    ''' <param name="scheStartDateTime">予定開始日時</param>
    ''' <returns>作業開始できる場合<c>true</c>、それ以外の場合<c>false</c></returns>
    ''' <remarks></remarks>
    Private Function CanWorkStart(ByVal svcStatus As String, _
                                 ByVal stallUseStatus As String, _
                                 ByVal tempFlg As String, _
                                 ByVal stallId As Decimal, _
                                 ByVal scheStartDateTime As Date) As Boolean
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. svcStatus={1}, stallUseStatus={2}, tempFlg={3}, stallId={4}, scheStartDateTime={5}" _
                    , MethodBase.GetCurrentMethod.Name, svcStatus, stallUseStatus, tempFlg, stallId, scheStartDateTime))
        'サービスステータスが「作業開始待ち」「次の作業開始待ち」であること
        If svcStatus.Equals(SvcStatusStartwait) _
            Or svcStatus.Equals(SvcStatusNextStartWait) Then
            'ストール利用ステータスが「作業開始待ち」であること
            If stallUseStatus.Equals(StalluseStatusStartWait) Then
                '仮置き・飛び込み客でないこと
                If IsNotTempAndWalkIn(tempFlg, stallId) Then
                    '予定開始日時が未設定値以外であること
                    If Not IsDefaultValue(scheStartDateTime) Then
                        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Return True", MethodBase.GetCurrentMethod.Name))
                        Return True
                    End If
                End If
            End If
        End If
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Return False", MethodBase.GetCurrentMethod.Name))
        Return False

    End Function

    ''' <summary>
    ''' 作業完了に遷移できるか否かチェックを行います
    ''' </summary>
    ''' <param name="stallUseStatus">ストール利用ステータス</param>
    ''' <returns>遷移可能な場合は<c>true</c>、それ以外は<c>false</c></returns>
    ''' <remarks></remarks>
    Private Function CanFinish(ByVal stallUseStatus As String) As Boolean

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. stallUseStatus={1}" _
                    , MethodBase.GetCurrentMethod.Name, stallUseStatus))

        'ストール利用ステータスが「作業中」「作業計画の一部の作業が中断」「中断」であること。
        If stallUseStatus.Equals(StalluseStatusStart) _
            Or stallUseStatus.Equals(StalluseStatusStartIncludeStopJob) _
            Or stallUseStatus.Equals(StalluseStatusStop) Then
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Return True", MethodBase.GetCurrentMethod.Name))
            Return True
        End If
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Return False", MethodBase.GetCurrentMethod.Name))
        Return False
    End Function

    ''' <summary>
    ''' 未来店客に遷移できるか否かチェックを行います
    ''' </summary>
    ''' <param name="svcStatus">サービスステータス</param>
    ''' <param name="stallUseStatus">ストール利用ステータス</param>
    ''' <param name="tempFlg">仮置きフラグ</param>
    ''' <returns>遷移可能な場合は<c>true</c>、それ以外は<c>false</c></returns>
    ''' <remarks></remarks>
    Private Function CanNoShow(ByVal svcStatus As String, _
                              ByVal stallUseStatus As String, _
                              ByVal tempFlg As String) As Boolean
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. svcStatus={1}, stallUseStatus={2}, tempFlg={3}" _
                   , MethodBase.GetCurrentMethod.Name, svcStatus, stallUseStatus, tempFlg))

        'サービスステータスが「未入庫」「未来店客」であること
        If svcStatus.Equals(SvcStatusNotCarin) _
            OrElse svcStatus.Equals(SvcStatusNoShow) Then
            'ストール利用ステータスが「未来店客」でないこと。
            If Not stallUseStatus.Equals(StalluseStatusNoshow) Then
                '仮置きフラグが「0：仮置ではない」であること
                If tempFlg.Equals(TempFlgNotTemp) Then
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Return True", MethodBase.GetCurrentMethod.Name))
                    Return True
                End If
            End If
        End If
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Return False", MethodBase.GetCurrentMethod.Name))
        Return False
    End Function
    ''' <summary>
    ''' 未来店客に遷移できるか否かチェックを行います
    ''' </summary>
    ''' <param name="stallUseStatus">ストール利用ステータス</param>
    ''' <returns>遷移可能な場合は<c>true</c>、それ以外は<c>false</c></returns>
    ''' <remarks></remarks>
    Private Function CanStop(ByVal stallUseStatus As String) As Boolean
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. stallUseStatus={1}" _
        , MethodBase.GetCurrentMethod.Name, stallUseStatus))

        'ストール利用ステータスが「作業中」「作業計画の一部の作業が中断」であること
        If stallUseStatus.Equals(StalluseStatusStart) _
            Or stallUseStatus.Equals(StalluseStatusStartIncludeStopJob) Then
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Return True", MethodBase.GetCurrentMethod.Name))
            Return True
        End If
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Return False", MethodBase.GetCurrentMethod.Name))
        Return False
    End Function

    ''' <summary>
    ''' キャンセルに遷移できるか否かチェックを行います。
    ''' </summary>
    ''' <param name="stallUseStatus">ストール利用ステータス</param>
    ''' <param name="tempFlg">仮置きフラグ</param>
    ''' <param name="stallId">ストールID</param>
    ''' <returns>遷移可能な場合は<c>true</c>、それ以外は<c>false</c></returns>
    Private Function CanCancel(ByVal stallUseStatus As String, ByVal tempFlg As String, ByVal stallId As Decimal) As Boolean
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. stallUseStatus={1}, tempFlg={2}, stallId={3}" _
                , MethodBase.GetCurrentMethod.Name, stallUseStatus, tempFlg, stallId))

        ' ストール利用ステータスが「未来店客」「着工指示待ち」「作業開始待ち」であること。
        If stallUseStatus = StalluseStatusNoshow OrElse _
            stallUseStatus = StalluseStatusWorkOrderWait OrElse _
            stallUseStatus = StalluseStatusStartWait Then
            ' 仮置き・飛び込み客でないこと。
            If IsNotTempAndWalkIn(tempFlg, stallId) Then
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Return True", MethodBase.GetCurrentMethod.Name))
                Return True
            End If
        End If
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Return False", MethodBase.GetCurrentMethod.Name))
        Return False
    End Function

    ''' <summary>
    ''' 日跨ぎ終了に遷移できるか否かチェックを行います。
    ''' </summary>
    ''' <param name="stallUseStatus">ストール利用ステータス</param>
    ''' <returns>遷移可能な場合は<c>true</c>、それ以外は<c>false</c></returns>
    Private Function CanMidFinish(ByVal stallUseStatus As String) As Boolean
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. stallUseStatus={1}" _
                , MethodBase.GetCurrentMethod.Name, stallUseStatus))

        'ストール利用ステータスが「作業中」「作業計画の一部の作業が中断」であること。
        If stallUseStatus = StalluseStatusStart OrElse _
            stallUseStatus = StalluseStatusStartIncludeStopJob Then
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Return True", MethodBase.GetCurrentMethod.Name))
            Return True
        End If
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Return False", MethodBase.GetCurrentMethod.Name))
        Return False
    End Function

    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    ''' <summary>
    ''' 作業中undoが行えるかどうかのチェックを行う
    ''' </summary>
    ''' <param name="svcStatus">サービスステータス</param>
    ''' <param name="stallUseStatus">ストール利用ステータス</param>
    ''' <returns>遷移可能な場合は<c>true</c>、それ以外は<c>false</c></returns>
    ''' <remarks></remarks>
    Private Function CanWorkingChipUndo(ByVal svcStatus As String, _
                                ByVal stallUseStatus As String) As Boolean
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. svcStatus={1}, stallUseStatus={2}" _
                    , MethodBase.GetCurrentMethod.Name, svcStatus, stallUseStatus))
        Dim returnValue As Boolean
        '作業中、中断の場合
        If SvcStatusStart.Equals(svcStatus) And _
            (StalluseStatusStart.Equals(stallUseStatus) Or _
                StalluseStatusStartIncludeStopJob.Equals(stallUseStatus)) Then
            returnValue = True
        Else
            returnValue = False
        End If
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Return {1}", MethodBase.GetCurrentMethod.Name, returnValue))
        Return returnValue
    End Function
    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

#End Region

    '2013/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
#Region "作業ステータスチェック"

    ''' <summary>
    ''' Jobが作業中に遷移できるか否かチェックを行います。
    ''' </summary>
    ''' <param name="inJobStatus">作業ステータス</param>
    ''' <returns>作業開始できる場合<c>true</c>、それ以外の場合<c>false</c></returns>
    ''' <remarks></remarks>
    Private Function CanStartJob(ByVal inJobStatus As String) As Boolean

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.S. inJobStatus={1}", _
                                  MethodBase.GetCurrentMethod.Name, inJobStatus))

        '作業ステータスが「作業開始前」または「中断中」の場合、作業開始できる
        If JobStatusBeforeStart.Equals(inJobStatus) _
            Or JobStatusStop.Equals(inJobStatus) Then

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}.E Return True", _
                                      MethodBase.GetCurrentMethod.Name))
            Return True

        Else

            '他の場合、開始できない
            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}.E Return False", _
                                      MethodBase.GetCurrentMethod.Name))
            Return False

        End If

    End Function

    ''' <summary>
    ''' Jobが作業完了に遷移できるか否かチェックを行います。
    ''' </summary>
    ''' <param name="inJobStatus">作業ステータス</param>
    ''' <returns>Jobが作業完了に遷移できる場合<c>true</c>、それ以外の場合<c>false</c></returns>
    ''' <remarks></remarks>
    Private Function CanFinishJob(ByVal inJobStatus As String) As Boolean

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.S. inJobStatus={1}", _
                                  MethodBase.GetCurrentMethod.Name, inJobStatus))

        '作業ステータスが「作業中」の場合、作業終了できる
        If JobStatusWorking.Equals(inJobStatus) Then

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}.E Return True", _
                                      MethodBase.GetCurrentMethod.Name))
            Return True

        Else

            '他の場合、終了できない
            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}.E Return False", _
                                      MethodBase.GetCurrentMethod.Name))
            Return False

        End If

    End Function

#End Region
    '2013/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

#Region "その他"
    ''' <summary>
    ''' 仮置き・飛び込み客でないか否かを判定する
    ''' </summary>
    ''' <param name="tempFlg">仮置きフラグ</param>
    ''' <param name="stallId">ストールID</param>
    ''' <returns>遷移可能な場合は<c>true</c>、それ以外は<c>false</c></returns>
    ''' <remarks></remarks>
    Private Function IsNotTempAndWalkIn(ByVal tempFlg As String, ByVal stallId As Decimal) As Boolean
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. tempFlg={1}, stallId={2}" _
                , MethodBase.GetCurrentMethod.Name, tempFlg, stallId))

        ' 仮置きフラグが「仮置きではない」であること。
        ' ストールIDが指定されていること。
        If tempFlg.Equals(TempFlgNotTemp) And IsDefaultValue(stallId) = False Then
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Return True", MethodBase.GetCurrentMethod.Name))
            Return True
        End If
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Return False", MethodBase.GetCurrentMethod.Name))
        Return False
    End Function

    ''' <summary>
    ''' 営業時間外判定
    ''' </summary>
    ''' <param name="judgeDateTime">判定日時</param>
    ''' <param name="stallStartTime">営業開始時間</param>
    ''' <param name="stallEndTime">営業終了時間</param>
    ''' <returns>営業時間外の場合：true、営業時間内の場合：false</returns>
    ''' <remarks>指定された日時が営業時間外か否かを判定します</remarks>
    Private Function IsOutOfWorkingTime(ByVal judgeDateTime As Date, ByVal stallStartTime As Date, ByVal stallEndTime As Date) As Boolean
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. judgeDateTime={1}, stallStartTime={2}, stallEndTime={3}" _
            , MethodBase.GetCurrentMethod.Name, judgeDateTime, stallStartTime, stallEndTime))
        '営業開始時間取得
        Dim startTime As TimeSpan = New TimeSpan(stallStartTime.Hour, stallStartTime.Minute, 0)
        '営業終了時間を取得
        Dim endTime As TimeSpan = New TimeSpan(stallEndTime.Hour, stallEndTime.Minute, 0)
        '営業時間が日を跨がない場合 (営業開始時間 < 営業終了時間)
        If startTime.CompareTo(endTime) < 0 Then
            '入力データ．判定日時の時間（ローカル時間で判断） < 入力データ．営業開始時間、または
            '入力データ．判定日時の時間（ローカル時間で判断） > 入力データ．営業終了時間
            If judgeDateTime.TimeOfDay.CompareTo(startTime) < 0 _
              Or endTime.CompareTo(judgeDateTime.TimeOfDay) <= 0 Then
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Return True", MethodBase.GetCurrentMethod.Name))
                Return True
            End If
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Return False", MethodBase.GetCurrentMethod.Name))
            Return False
        End If
        '入力データ．判定日時の時間（ローカル時間で判断） > 入力データ．営業終了時間、かつ
        '入力データ．判定日時の時間（ローカル時間で判断） < 入力データ．営業開始時間
        If endTime.CompareTo(judgeDateTime.TimeOfDay) <= 0 _
            And judgeDateTime.TimeOfDay.CompareTo(startTime) < 0 Then
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Return True", MethodBase.GetCurrentMethod.Name))
            Return True
        End If
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Return False", MethodBase.GetCurrentMethod.Name))
        Return False
    End Function

    '2013/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
    ''' <summary>
    ''' 開始日時に対する営業日が現在日時に対する営業日のチェック
    ''' </summary>
    ''' <param name="inStartDate">開始日時</param>
    ''' <param name="inNowDate">現在日時</param>
    ''' <param name="inStallStartTime">ストール開始時間</param>
    ''' <returns>合ってる：true、営業時間内の場合：false</returns>
    ''' <remarks>指定された日時が営業時間外か否かを判定します</remarks>
    Private Function CheckStartDateIsToday(ByVal inStartDate As Date, ByVal inNowDate As Date, ByVal inStallStartTime As Date) As Boolean

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.Start. inStartDate={1}, inNowDate={2}, inStallStartTime={3}" _
            , MethodBase.GetCurrentMethod.Name, inStartDate, inNowDate, inStallStartTime))
        '予定開始日時に対する営業日を取得する
        Dim startWorkingDate As Date = Me.GetWorkingDate(inStartDate, inStallStartTime)
        '現在日時に対する営業日を取得する
        Dim nowWorkingDate As Date = Me.GetWorkingDate(inNowDate, inStallStartTime)

        '予定開始日時に対する営業日が現在日時に対する営業日と異なる場合
        If startWorkingDate <> nowWorkingDate Then
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.End. Return False", MethodBase.GetCurrentMethod.Name))
            Return False
        Else
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.End. Return True", MethodBase.GetCurrentMethod.Name))
            Return True
        End If

    End Function

    ''' <summary>
    ''' 表示サービス分類または表示商品設定チェック
    ''' </summary>
    ''' <param name="inSvcClassId">表示サービス分類ID</param>
    ''' <param name="inMercId">表示商品ID</param>
    ''' <returns>True:いずれ設定した場合</returns>
    ''' <remarks></remarks>
    Private Function HasSetJobSvcClassId(ByVal inSvcClassId As Decimal, ByVal inMercId As Long) As Boolean

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.Start. inSvcClassId={1}, inMercId={2}" _
            , MethodBase.GetCurrentMethod.Name, inSvcClassId, inMercId))

        '全部設定してない場合Falseを戻す
        If IsDefaultValue(inSvcClassId) _
            And IsDefaultValue(inMercId) Then
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.End. Return False", MethodBase.GetCurrentMethod.Name))
            Return False
        Else
            'いずれ設定した場合、Trueを戻す
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.End. Return True", MethodBase.GetCurrentMethod.Name))
            Return True
        End If

    End Function
    '2013/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    ''' <summary>
    ''' 指定ストールにテクニシャンいるかどうかチェック
    ''' </summary>
    ''' <param name="stallId">指定ストールID</param>
    ''' <param name="objStaffContext">店舗情報</param>
    ''' <returns>いる：true、いない：false</returns>
    ''' <remarks></remarks>
    Private Function HasTechnicianInStall(ByVal stallId As Decimal _
                                        , ByVal objStaffContext As StaffContext) As Boolean

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.Start. dealerCode={1}, branchCode={2}, stallId={3}" _
                                , MethodBase.GetCurrentMethod.Name, objStaffContext.DlrCD, objStaffContext.BrnCD, stallId))

        '該ストールのテクニシャン人数を取得する
        Dim staffStallDataList As TabletSmbCommonClassStringValueDataTable = GetStaffCodeByStallId(objStaffContext.DlrCD, objStaffContext.BrnCD, stallId)
        Dim nCount As Long = staffStallDataList.Count
        'テクニシャンがない場合
        If nCount = 0 Then
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.End. Return False.", MethodBase.GetCurrentMethod.Name))
            Return False
        Else
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.End. Return True.", MethodBase.GetCurrentMethod.Name))
            Return True
        End If
    End Function
    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

#End Region

#Region "重複チェック"
    ''' <summary>
    ''' チップが重複配置されているか否かを判定します
    ''' </summary>
    ''' <param name="dlrCode">販売店</param>
    ''' <param name="brnCode">店舗</param>
    ''' <param name="stallUseId">ストール利用ID</param>
    ''' <param name="stallId">ストールID</param>
    ''' <param name="scheStartDateTime">予定開始日時</param>
    ''' <param name="scheEndDateTime">予定終了日時</param>
    ''' <param name="dtNow">今の時間</param>
    ''' <returns>チップ重複配置ありの場合<c>true</c>、それ以外の場合<c>false</c></returns>
    ''' <remarks></remarks>
    Public Function CheckChipOverlapPosition(ByVal dlrCode As String, _
                                             ByVal brnCode As String, _
                                             ByVal stallUseId As Decimal, _
                                             ByVal stallId As Decimal, _
                                             ByVal scheStartDateTime As Date, _
                                             ByVal scheEndDateTime As Date, _
                                             ByVal dtNow As Date) As Boolean

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. dlrCode={1}, brnCode={2}, stallUseId={3}, stallId={4}, scheStartDateTime={5}, scheEndDateTime={6}, dtNow={7}" _
            , MethodBase.GetCurrentMethod.Name, dlrCode, brnCode, stallUseId, stallId, scheStartDateTime, scheEndDateTime, dtNow))

        Using ta As New TabletSMBCommonClassDataAdapter
            '重複してるチップの数を取得する
            Dim overlapChipNums As Long = ta.GetChipOverlapChipNums(dlrCode, brnCode, stallUseId, stallId, _
                                                                    scheStartDateTime, scheEndDateTime, DefaultDateTimeValueGet())
            If overlapChipNums > 0 Then
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Return True", MethodBase.GetCurrentMethod.Name))
                Return True
            End If

            '重複してる仮チップの数を取得する
            overlapChipNums = ta.GetKariKariChipOverlapChipNums(stallId, scheStartDateTime, scheEndDateTime, dtNow)
            If overlapChipNums > 0 Then
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Return True", MethodBase.GetCurrentMethod.Name))
                Return True
            End If
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Return False", MethodBase.GetCurrentMethod.Name))
            Return False
        End Using
    End Function

    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    ''' <summary>
    ''' チップが重複配置されているか否かを判定します
    ''' </summary>
    ''' <param name="dlrCode">販売店</param>
    ''' <param name="brnCode">店舗</param>
    ''' <param name="stallUseId">ストール利用ID</param>
    ''' <param name="stallId">ストールID</param>
    ''' <param name="scheStartDateTime">予定開始日時</param>
    ''' <param name="scheEndDateTime">予定終了日時</param>
    ''' <param name="dtNow">今の時間</param>
    ''' <returns>チップ重複配置ありの場合<c>true</c>、それ以外の場合<c>false</c></returns>
    ''' <remarks></remarks>
    Public Function CheckRezChipOverlapPosition(ByVal dlrCode As String, _
                                             ByVal brnCode As String, _
                                             ByVal stallUseId As Decimal, _
                                             ByVal stallId As Decimal, _
                                             ByVal scheStartDateTime As Date, _
                                             ByVal scheEndDateTime As Date, _
                                             ByVal dtNow As Date) As Boolean

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. dlrCode={1}, brnCode={2}, stallUseId={3}, stallId={4}, scheStartDateTime={5}, scheEndDateTime={6}, dtNow={7}" _
            , MethodBase.GetCurrentMethod.Name, dlrCode, brnCode, stallUseId, stallId, scheStartDateTime, scheEndDateTime, dtNow))

        Using ta As New TabletSMBCommonClassDataAdapter
            '重複してるチップの数を取得する
            Dim overlapChipNums As Long = ta.GetChipOverlapRezChipNums(dlrCode, brnCode, stallUseId, stallId, _
                                                                    scheStartDateTime, scheEndDateTime, DefaultDateTimeValueGet())
            If overlapChipNums > 0 Then
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Return True", MethodBase.GetCurrentMethod.Name))
                Return True
            End If

            '重複してる仮チップの数を取得する
            overlapChipNums = ta.GetKariKariChipOverlapChipNums(stallId, scheStartDateTime, scheEndDateTime, dtNow)
            If overlapChipNums > 0 Then
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Return True", MethodBase.GetCurrentMethod.Name))
                Return True
            End If
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Return False", MethodBase.GetCurrentMethod.Name))
            Return False
        End Using
    End Function
    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

    ''' <summary>
    ''' 他の休憩時間と重複配置チェック
    ''' </summary>
    ''' <param name="stallId">ストールID</param>
    ''' <param name="startDateTime">開始日時</param>
    ''' <param name="endDateTime">終了日時</param>
    ''' <returns>チップ重複配置ありの場合<c>true</c>、それ以外の場合<c>false</c></returns>
    ''' <remarks></remarks>
    Private Function CheckStallIdleOverlapPosition(ByVal stallIdleId As Decimal, _
                                                ByVal stallId As Decimal, _
                                                ByVal startDateTime As Date, _
                                                ByVal endDateTime As Date) As Boolean

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. stallIdleId={1}, stallId={2}, startDateTime={3}, endDateTime={4}" _
            , MethodBase.GetCurrentMethod.Name, stallIdleId, stallId, startDateTime, endDateTime))

        Using ta As New TabletSMBCommonClassDataAdapter
            Dim stallIdLst As New List(Of Decimal)
            stallIdLst.Add(stallId)
            'ある時間範囲で休憩エリアがあれば、取得する
            Dim dt As TabletSmbCommonClassStallIdleInfoDataTable = _
                    ta.GetAllIdleDateInfo(stallIdLst, startDateTime, endDateTime)

            Dim rowCount = dt.Count
            '自分を除外
            For Each dr As TabletSmbCommonClassStallIdleInfoRow In dt.Rows
                If dr.STALL_IDLE_ID = stallIdleId Then
                    rowCount = rowCount - 1
                    Exit For
                End If
            Next

            If rowCount > 0 Then
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Return True", MethodBase.GetCurrentMethod.Name))
                '重複がある
                Return True
            Else
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Return False", MethodBase.GetCurrentMethod.Name))
                Return False
            End If
        End Using
    End Function

    ''' <summary>
    ''' 同一のストールに既に作業中のステータスが存在するかチェックします
    ''' </summary>
    ''' <param name="objStaffContext">スタッフ情報</param>
    ''' <param name="stallId">ストールID</param>
    ''' <param name="workingStartDateTime">営業開始時間</param>
    ''' <returns>作業中のステータスが存在する場合<c>true</c>、それ以外の場合<c>false</c></returns>
    ''' <remarks></remarks>
    Private Function HasWorkingChipInOneStall(ByVal objStaffContext As StaffContext, _
                                              ByVal stallId As Decimal, _
                                              ByVal workingStartDateTime As Date) As Boolean
        Using ta As New TabletSMBCommonClassDataAdapter
            Return ta.HasWorkingChipInOneStall(objStaffContext.DlrCD, objStaffContext.BrnCD, stallId, workingStartDateTime)
        End Using
    End Function

    '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START

    ''' <summary>
    ''' チップがストール使用不可と重複しているか判定します
    ''' </summary>
    ''' <param name="scheStartDateTime">予定開始日時</param>
    ''' <param name="scheEndDateTime">予定終了日時</param>
    ''' <param name="stallId">ストールID</param>
    ''' <returns>チップ重複配置ありの場合<c>true</c>、それ以外の場合<c>false</c></returns>
    ''' <remarks></remarks>
    Public Function CheckStallUnavailableOverlapPosition(ByVal scheStartDateTime As Date, _
                                                ByVal scheEndDateTime As Date, _
                                                ByVal stallId As Decimal) As Boolean

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. scheStartDateTime={1}, scheEndDateTime={2}, stallId={3}" _
            , MethodBase.GetCurrentMethod.Name, scheStartDateTime, scheEndDateTime, stallId))

        Using ta As New TabletSMBCommonClassDataAdapter
            '重複してるストール使用不可の数を取得する
            Dim overlapUnavailable As TabletSmbCommonClassIdleTimeInfoDataTable = ta.GetStallUnavailableInfo(scheStartDateTime, scheEndDateTime, stallId)
            If 0 < overlapUnavailable.Rows.Count Then
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Return True", MethodBase.GetCurrentMethod.Name))
                Return True
            End If
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Return False", MethodBase.GetCurrentMethod.Name))
            Return False
        End Using
    End Function

    '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END

#End Region

#Region "各操作に対してチェック"

#Region "開始操作に関するチェック"


    '2013/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
    ' ''' <summary>
    ' ''' 親R/Oが作業開始するかどうかチェックします
    ' ''' </summary>
    ' ''' <param name="svcinId">サービス入庫ID</param>
    ' ''' <param name="jobDtlId">作業内容ID</param>
    ' ''' <returns>親ROが作業開始されたチップがある<c>true</c>、それ以外の場合<c>false</c></returns>
    ' ''' <remarks></remarks>
    ' ''' 2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    'Private Function HasParentroStarted(ByVal svcinId As Decimal, _
    '                                    ByVal jobDtlId As Decimal) As Boolean
    '    'Private Function HasParentroStarted(ByVal svcinId As Decimal) As Boolean
    '    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. svcinId={1}, jobDtlId={2}" _
    '                            , MethodBase.GetCurrentMethod.Name, svcinId, jobDtlId))
    '    Using ta As New TabletSMBCommonClassDataAdapter
    '        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    '        '該作業のRO枝番を取得
    '        Dim roJobSeqTable As TabletSmbCommonClassJobInstructDataTable = _
    '            ta.GetROJobSeqByJobDtlId(jobDtlId)
    '        For Each roJobSeqRow As TabletSmbCommonClassJobInstructRow In roJobSeqTable
    '            '該チップに親ROがあれば、Falseを戻す
    '            If roJobSeqRow.RO_JOB_SEQ = 0 Then
    '                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Return True(This work has parent RO).", MethodBase.GetCurrentMethod.Name))
    '                Return True
    '            End If
    '        Next
    '        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
    '        Return ta.HasParentroStarted(svcinId)
    '    End Using
    'End Function

    ''' <summary>
    ''' 親R/Oが作業開始するかどうかチェックします
    ''' </summary>
    ''' <param name="roNum">RO番号</param>
    ''' <param name="jobDtlId">作業内容ID</param>
    ''' <param name="inRoSeq">RO連番</param>
    ''' <returns>親ROが作業開始されたチップがある<c>true</c>、それ以外の場合<c>false</c></returns>
    ''' <remarks></remarks>
    ''' 2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    Private Function HasParentroStarted(ByVal roNum As String, _
                                        ByVal jobDtlId As Decimal, _
                                        Optional ByVal inRoSeq As Long = -1) As Boolean

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.Start. roNum={1}, jobDtlId={2}, inRoSeq={3}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  roNum, _
                                  jobDtlId, _
                                  inRoSeq))

        '親R/Oが作業開始してるで初期化
        Dim parentroStarted As Boolean = True

        Using ta As New TabletSMBCommonClassDataAdapter

            If -1 = inRoSeq Then
                'All Startの場合

                '該当チップに紐づく全RO枝番を取得
                Dim roJobSeqTable As TabletSmbCommonClassJobInstructDataTable = _
                    ta.GetROJobSeqByJobDtlId(jobDtlId)

                For Each roJobSeqRow As TabletSmbCommonClassJobInstructRow In roJobSeqTable

                    '該チップに親ROがあれば、
                    If roJobSeqRow.RO_JOB_SEQ = 0 Then

                        '今回の開始操作で親ROを開始させるので、Trueを戻す
                        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                                  "{0}.E Return True(This chip has parent RO).", _
                                                  MethodBase.GetCurrentMethod.Name))
                        Return True

                    End If

                Next

            Else
                '単独なJob開始

                If 0 = inRoSeq Then
                    '開始するのは親ROのJob

                    '今回の開始Jobが親ROなので、Trueを戻す
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                              "{0}.E Return True(Started RO is parent RO).", _
                                              MethodBase.GetCurrentMethod.Name))
                    Return True

                End If

            End If

            '親RO開始したか
            parentroStarted = ta.HasParentroStarted(roNum)

        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.End. parentroStarted={1}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  parentroStarted))

        Return parentroStarted

    End Function
    '2013/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

#End Region

    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
#Region "開始操作に関するチェック"

    '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
    ' ''' <summary>
    ' ''' 開始操作のいろいろなチェック
    ' ''' </summary>
    ' ''' <param name="chipEntity">操作チップエンティティ</param>
    ' ''' <param name="inRsltStartDateTime">実績開始日時</param>
    ' ''' <param name="inStallStartTime">営業開始日時</param>
    ' ''' <param name="inStallEndTime">営業終了日時</param>
    ' ''' <param name="inRowLockVersion">行ロックバージョン</param>
    ' ''' <param name="objStaffContext">スタッフ情報</param>
    ' ''' <param name="inUpdateDate">更新日時</param>
    ' ''' <param name="inSystemId">呼ぶ画面ID</param>
    ' ''' <returns>チェック結果</returns>
    ' ''' <remarks></remarks>
    'Private Function CheckStartAction(ByVal chipEntity As TabletSmbCommonClassChipEntityRow, _
    '                                  ByVal inRsltStartDateTime As Date, _
    '                                  ByVal inStallStartTime As Date, _
    '                                  ByVal inStallEndTime As Date, _
    '                                  ByVal inRowLockVersion As Long, _
    '                                  ByVal objStaffContext As StaffContext, _
    '                                  ByVal inUpdateDate As Date, _
    '                                  ByVal inSystemId As String) As Long
    'Logger.Info(String.Format(CultureInfo.InvariantCulture _
    '                    , "{0}.S. inRsltStartDateTime={1}, inStallStartTime={2}, inStallEndTime={3}, inRowLockVersion={4} " _
    '                    , MethodBase.GetCurrentMethod.Name, _
    '                    inRsltStartDateTime, _
    '                    inStallStartTime, _
    '                    inStallEndTime, _
    '                    inRowLockVersion))
    ''' <summary>
    ''' 開始操作のいろいろなチェック
    ''' </summary>
    ''' <param name="chipEntity">操作チップエンティティ</param>
    ''' <param name="inRsltStartDateTime">実績開始日時</param>
    ''' <param name="inStallStartTime">営業開始日時</param>
    ''' <param name="inStallEndTime">営業終了日時</param>
    ''' <param name="inRowLockVersion">行ロックバージョン</param>
    ''' <param name="objStaffContext">スタッフ情報</param>
    ''' <param name="inUpdateDate">更新日時</param>
    ''' <param name="inSystemId">呼ぶ画面ID</param>
    ''' <param name="inCallerType">呼び元タイプ (1:工程管理画面の全開始から呼ぶ 2:詳細画面またはTC画面の全開始から呼ぶ 3:詳細画面の作業開始から呼ぶ)</param>
    ''' <param name="inJobStatus">作業ステータス(inCallerTypeが3の場合作業ステータスチェック用)</param>
    ''' <param name="inRoSeq">RO連番</param>
    ''' <returns>チェック結果</returns>
    ''' <remarks></remarks>
    Private Function CheckStartAction(ByVal chipEntity As TabletSmbCommonClassChipEntityRow, _
                                      ByVal inRsltStartDateTime As Date, _
                                      ByVal inStallStartTime As Date, _
                                      ByVal inStallEndTime As Date, _
                                      ByVal inRowLockVersion As Long, _
                                      ByVal objStaffContext As StaffContext, _
                                      ByVal inUpdateDate As Date, _
                                      ByVal inSystemId As String, _
                                      Optional ByVal inCallerType As Long = CallerTypeSmbAllJobAction, _
                                      Optional ByVal inJobStatus As String = JobStatusBeforeStart, _
                                      Optional ByVal inRoSeq As Long = -1) As Long

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.S. inRsltStartDateTime={1}, inStallStartTime={2}, inStallEndTime={3}, inRowLockVersion={4}, inUpdateDate={5}, inSystemId={6}, inCallerType={7}, inJobStatus={8} ", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  inRsltStartDateTime, _
                                  inStallStartTime, _
                                  inStallEndTime, _
                                  inRowLockVersion, _
                                  inUpdateDate, _
                                  inSystemId, _
                                  inCallerType, _
                                  inJobStatus))

        '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

        ' サービス入庫をロックして、チェックする
        Dim result As Long = Me.LockServiceInTable(chipEntity.SVCIN_ID, inRowLockVersion, objStaffContext.Account, inUpdateDate, inSystemId)
        If result <> TabletSMBCommonClassBusinessLogic.ActionResult.Success Then
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E LockServiceInTableError", System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return result
        End If

        '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
        ''ステータス遷移可否をチェックする
        'If Not CanWorkStart(chipEntity.SVC_STATUS, _
        '                    chipEntity.STALL_USE_STATUS, _
        '                    chipEntity.TEMP_FLG, _
        '                    chipEntity.STALL_ID, _
        '                    chipEntity.SCHE_START_DATETIME) Then
        '    Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.E CheckError" _
        '            , MethodBase.GetCurrentMethod.Name))
        '    Return ActionResult.CheckError
        'End If
        '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

        'RO NOを紐付けてるかチェックする
        Dim roNum As String = chipEntity.RO_NUM
        If String.IsNullOrEmpty(roNum.Trim()) Then
            Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0} NotSetroNoError. ", System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return ActionResult.NotSetroNoError
        End If

        '指定ストールにテクニシャンいない場合
        Dim hasTechnicianInStall As Boolean = Me.HasTechnicianInStall(chipEntity.STALL_ID, objStaffContext)
        If Not hasTechnicianInStall Then
            Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0} NoTechnicianError. ", System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return ActionResult.NoTechnicianError
        End If

        '営業時間外
        If IsOutOfWorkingTime(inRsltStartDateTime, inStallStartTime, inStallEndTime) Then
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E OutOfWorkingTimeError", MethodBase.GetCurrentMethod.Name))
            Return ActionResult.OutOfWorkingTimeError
        End If

        '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
        ''予定開始日時に対する営業日を取得する
        'Dim scheStartWorkingDate As Date = Me.GetWorkingDate(inRsltStartDateTime, inStallStartTime)
        ''現在日時に対する営業日を取得する
        'Dim nowWorkingDate As Date = Me.GetWorkingDate(inUpdateDate, inStallStartTime)

        ''予定開始日時に対する営業日が現在日時に対する営業日と異なる場合
        'If scheStartWorkingDate <> nowWorkingDate Then
        '    Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.E NotStartDayError", MethodBase.GetCurrentMethod.Name))
        '    Return ActionResult.NotStartDayError
        'End If

        ''処理対象の作業内容．表示サービス分類コードが未設定値の場合
        'If IsDefaultValue(chipEntity.SVC_CLASS_ID) _
        '    And IsDefaultValue(chipEntity.MERC_ID) Then
        '    Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.E NotSetJobSvcClassIdError", MethodBase.GetCurrentMethod.Name))
        '    Return ActionResult.NotSetJobSvcClassIdError
        'End If

        '予定開始日時に対する営業日が現在日時に対する営業日のチェック
        If Not CheckStartDateIsToday(inRsltStartDateTime, inUpdateDate, inStallStartTime) Then

            'チェック失敗の場合、予定開始日時に対する営業日が現在日時に対する営業日と異なるエラーコードを戻す
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E NotStartDayError", MethodBase.GetCurrentMethod.Name))
            Return ActionResult.NotStartDayError

        End If

        '表示サービス分類または表示商品設定チェック
        If Not HasSetJobSvcClassId(chipEntity.SVC_CLASS_ID, chipEntity.MERC_ID) Then

            'チェック失敗の場合、処理対象チップのサービス（整備内容）が未設定のエラーコードを戻す
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E NotSetJobSvcClassIdError", MethodBase.GetCurrentMethod.Name))
            Return ActionResult.NotSetJobSvcClassIdError

        End If
        '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

        Dim workingDateTimeData As List(Of Date) = GetStallDispDate(inRsltStartDateTime, inStallStartTime, inStallEndTime)
        Dim startTime As Date = workingDateTimeData(0)

        '同一のストールに既に作業中のステータスが存在する場合
        If HasWorkingChipInOneStall(objStaffContext, chipEntity.STALL_ID, startTime) Then
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E HasWorkingChipInOneStallError", MethodBase.GetCurrentMethod.Name))
            Return ActionResult.HasWorkingChipInOneStallError
        End If

        '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
        ''親R/Oが作業開始されていないため、追加作業の作業は開始できない
        'If Not HasParentroStarted(chipEntity.SVCIN_ID, chipEntity.JOB_DTL_ID) Then
        '    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
        '    Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.E ParentroNotStartedError", MethodBase.GetCurrentMethod.Name))
        '    Return ActionResult.ParentroNotStartedError
        'End If

        ' 2019/08/09 NSK 鈴木 DLR-002 子チップを着工指示した時にエラー発生（子チップから開始でアラート表示を修正） START
        ' 子チップ側から先に作業開始できるようにするため、親チップの作業開始チェックを外す。
        ' '親R/Oが開始したかチェック
        ' If Not HasParentroStarted(chipEntity.RO_NUM, _
        '                           chipEntity.JOB_DTL_ID, _
        '                           inRoSeq) Then
        '     '親RO開始してない
        '
        '     '親RO開始してないエラーコードを戻す
        '     Logger.Error(String.Format(CultureInfo.InvariantCulture, _
        '                                "{0}.E ParentroNotStartedError", _
        '                                MethodBase.GetCurrentMethod.Name))
        '     Return ActionResult.ParentroNotStartedError
        '
        ' End If
        ' 2019/08/09 NSK 鈴木 DLR-002 子チップを着工指示した時にエラー発生（子チップから開始でアラート表示を修正） END

        '関連チップに作業中チップ存在するかのをチェックする
        If Me.IsExistWorkingRelationChip(chipEntity.SVCIN_ID, _
                                         chipEntity.STALL_USE_ID) Then
            '関連チップに作業中チップが存在する場合

            '関連チップに作業中チップが存在しているエラーコードを戻す
            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                       "{0}.E HasStartedRelationChipError", _
                                       MethodBase.GetCurrentMethod.Name))
            Return ActionResult.HasStartedRelationChipError

        End If

        '工程管理画面から開始の場合、チップステータスチェックがいる
        If CallerTypeSmbAllJobAction = inCallerType Then

            'チップステータス遷移可否をチェックする
            If Not Me.CanWorkStart(chipEntity.SVC_STATUS, _
                                   chipEntity.STALL_USE_STATUS, _
                                   chipEntity.TEMP_FLG, _
                                   chipEntity.STALL_ID, _
                                   chipEntity.SCHE_START_DATETIME) Then

                'エラーの場合、チップステータスチェックエラーを戻す
                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                           "{0}.E CheckError", _
                                           MethodBase.GetCurrentMethod.Name))
                Return ActionResult.CheckError

            End If

        End If

        '単独作業開始の場合、作業ステータスをチェックする
        If CallerTypeDetailSingleJobAction = inCallerType Then

            '作業ステータス遷移可否をチェックする
            If Not Me.CanStartJob(inJobStatus) Then

                'エラーの場合、作業ステータスチェックエラーを戻す
                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                           "{0}.E InvalidJobStatusError", _
                                           MethodBase.GetCurrentMethod.Name))
                Return ActionResult.InvalidJobStatusError

            End If

        End If
        '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return ActionResult.Success
    End Function


    '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
    ''' <summary>
    ''' 既に開始したチップに対する開始操作のチェックを行う
    ''' </summary>
    ''' <param name="chipEntity">操作チップエンティティ</param>
    ''' <param name="inRsltStartDateTime">実績開始日時</param>
    ''' <param name="inStallStartTime">営業開始日時</param>
    ''' <param name="inStallEndTime">営業終了日時</param>
    ''' <param name="inRowLockVersion">行ロックバージョン</param>
    ''' <param name="objStaffContext">スタッフ情報</param>
    ''' <param name="inUpdateDate">更新日時</param>
    ''' <param name="inSystemId">呼ぶ画面ID</param>
    ''' <param name="inJobStatus">作業ステータス</param>
    ''' <param name="inRoSeq">RO連番</param>
    ''' <returns>チェック結果</returns>
    ''' <remarks></remarks>
    Private Function CheckStartedStartAction(ByVal chipEntity As TabletSmbCommonClassChipEntityRow, _
                                             ByVal inRsltStartDateTime As Date, _
                                             ByVal inStallStartTime As Date, _
                                             ByVal inStallEndTime As Date, _
                                             ByVal inRowLockVersion As Long, _
                                             ByVal objStaffContext As StaffContext, _
                                             ByVal inUpdateDate As Date, _
                                             ByVal inSystemId As String, _
                                             Optional ByVal inJobStatus As String = "", _
                                             Optional ByVal inRoSeq As Long = -1) As Long

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.Start. inRsltStartDateTime={1}, inStallStartTime={2}, inStallEndTime={3}, inRowLockVersion={4}, inJobStatus={5} ", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  inRsltStartDateTime, _
                                  inStallStartTime, _
                                  inStallEndTime, _
                                  inRowLockVersion, _
                                  inJobStatus))

        ' サービス入庫をロックして、チェックする
        Dim result As Long = Me.LockServiceInTable(chipEntity.SVCIN_ID, inRowLockVersion, objStaffContext.Account, inUpdateDate, inSystemId)

        'サービス入庫ロック失敗の場合、エラーコードを戻す
        If result <> TabletSMBCommonClassBusinessLogic.ActionResult.Success Then

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.End. LockServiceInTableError", System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return result

        End If

        'RO NOを紐付けてるかチェックする
        Dim roNum As String = chipEntity.RO_NUM

        'RO番号が空白の場合、R/Oが紐づいていないエラーコードを戻す
        If String.IsNullOrEmpty(roNum.Trim()) Then

            Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0}.End. NotSetroNoError. ", System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return ActionResult.NotSetroNoError

        End If

        '指定ストールにテクニシャンいない場合、テクニシャン未配置エラーコードを戻す
        If Not Me.HasTechnicianInStall(chipEntity.STALL_ID, objStaffContext) Then

            Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0}.End. NoTechnicianError. ", System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return ActionResult.NoTechnicianError

        End If

        '営業時間外開始かをチェックする
        If Me.IsOutOfWorkingTime(inRsltStartDateTime, inStallStartTime, inStallEndTime) Then

            '営業時間外開始の場合、営業時間を超えるエラーコードを戻す
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.End. OutOfWorkingTimeError", MethodBase.GetCurrentMethod.Name))
            Return ActionResult.OutOfWorkingTimeError

        End If

        '予定開始日時に対する営業日が現在日時に対する営業日のチェック
        If Not Me.CheckStartDateIsToday(inRsltStartDateTime, inUpdateDate, inStallStartTime) Then

            'チェックエラーの場合、予定開始日時に対する営業日が現在日時に対する営業日と異なるエラーコードを戻す
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.End. NotStartDayError", MethodBase.GetCurrentMethod.Name))
            Return ActionResult.NotStartDayError

        End If

        '表示サービス分類または表示商品設定チェック
        If Not HasSetJobSvcClassId(chipEntity.SVC_CLASS_ID, chipEntity.MERC_ID) Then

            '表示サービス分類または表示商品設定設定されていない場合、処理対象チップのサービス（整備内容）が未設定のエラーコードを戻す
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E NotSetJobSvcClassIdError", MethodBase.GetCurrentMethod.Name))
            Return ActionResult.NotSetJobSvcClassIdError

        End If

        ' 2019/08/09 NSK 鈴木 DLR-002 子チップを着工指示した時にエラー発生（子チップから開始でアラート表示を修正） START
        ' 子チップ側から先に作業開始できるようにするため、親チップの作業開始チェックを外す。
        ' '親R/Oが開始したかチェック
        ' If Not HasParentroStarted(chipEntity.RO_NUM, _
        '                           chipEntity.JOB_DTL_ID, _
        '                           inRoSeq) Then
        '     '親RO開始してない
        '
        '     '親RO開始してないエラーコードを戻す
        '     Logger.Error(String.Format(CultureInfo.InvariantCulture, _
        '                                "{0}.E ParentroNotStartedError", _
        '                                MethodBase.GetCurrentMethod.Name))
        '     Return ActionResult.ParentroNotStartedError
        '
        ' End If
        ' 2019/08/09 NSK 鈴木 DLR-002 子チップを着工指示した時にエラー発生（子チップから開始でアラート表示を修正） END

        If Not String.IsNullOrWhiteSpace(inJobStatus) Then

            '作業ステータス遷移可否をチェックする
            If Not Me.CanStartJob(inJobStatus) Then

                '遷移できない場合、作業ステータスチェックエラーコードを戻す
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E InvalidJobStatusError", MethodBase.GetCurrentMethod.Name))
                Return ActionResult.InvalidJobStatusError

            End If

        End If



        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.End. Success", System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return ActionResult.Success

    End Function
    '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

#End Region

#Region "終了操作に関するチェック"
    ''' <summary>
    ''' 終了操作のいろいろなチェック
    ''' </summary>
    ''' <param name="chipEntity">操作チップのエンティティ</param>
    ''' <param name="inRsltEndDateTime">実績終了日時</param>
    ''' <param name="inStallStartTime">営業開始日時</param>
    ''' <param name="inStallEndTime">営業終了日時</param>
    ''' <param name="inRowLockVersion">行ロックバージョン</param>
    ''' <param name="inRestFlg">休憩取るフラグ</param>
    ''' <param name="inStaffInfo">スタッフ情報</param>
    ''' <param name="inUpdateDate">更新日時</param>
    ''' <param name="inSystemId">呼ぶ画面ID</param>
    ''' <returns>チェック結果</returns>
    ''' <remarks></remarks>
    Private Function CheckFinishAction(ByVal chipEntity As TabletSmbCommonClassChipEntityRow, _
                                       ByVal inRsltEndDateTime As Date, _
                                       ByVal inStallStartTime As Date, _
                                       ByVal inStallEndTime As Date, _
                                       ByVal inRowLockVersion As Long, _
                                       ByVal inRestFlg As String, _
                                       ByVal inStaffInfo As StaffContext, _
                                       ByVal inUpdateDate As Date, _
                                       ByVal inSystemId As String) As Long

        Dim inStallId As Decimal = chipEntity.STALL_ID
        Dim inJobDtlId As Decimal = chipEntity.JOB_DTL_ID
        Dim inSvcinId As Decimal = chipEntity.SVCIN_ID
        Dim inStallUseStatus As String = chipEntity.STALL_USE_STATUS
        Dim inInspectionStatus As String = chipEntity.INSPECTION_STATUS
        Dim inRONum As String = chipEntity.RO_NUM
        Dim inRsltStartDateTime As Date = chipEntity.RSLT_START_DATETIME

        Logger.Info(String.Format(CultureInfo.InvariantCulture _
                                , "{0}.S. inStallUseStatus={1}, inInspectionStatus={2}, inRONum={3}, inRsltStartDateTime={4}, inRsltEndDateTime={5} " _
                                , MethodBase.GetCurrentMethod.Name, _
                                inStallUseStatus, _
                                inInspectionStatus, _
                                inRONum, _
                                inRsltStartDateTime, _
                                inRsltEndDateTime))

        '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
        '行ロックバージョンが-1の場合、ストールロックが不要
        If inRowLockVersion <> -1 Then
            '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

            'サービス入庫テーブルの行をロックして、チェックする
            Dim resultCheckVersion As Long = Me.LockServiceInTable(inSvcinId, _
                                                                   inRowLockVersion, _
                                                                   inStaffInfo.Account, _
                                                                   inUpdateDate, _
                                                                   inSystemId)
            If resultCheckVersion <> ActionResult.Success Then
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E LockServiceInTableError", System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return resultCheckVersion
            End If

            '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
        End If
        '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

        'ステータス遷移可否をチェックする
        If Not CanFinish(inStallUseStatus) Then
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E CheckError" _
                            , MethodBase.GetCurrentMethod.Name))
            Return ActionResult.CheckError
        End If

        '検査ステータスが1(検査依頼中)の場合、終了できない
        If InspectionApproval.Equals(inInspectionStatus) Then
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E InspectionStatusFinishError" _
                            , MethodBase.GetCurrentMethod.Name))
            Return ActionResult.InspectionStatusFinishError
        End If

        'RO NOを紐付けてるかチェックする
        If String.IsNullOrEmpty(inRONum.Trim()) Then
            Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0} NotSetroNoError. ", System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return ActionResult.NotSetroNoError
        End If

        ''開始時間から終了時間までの範囲に重複休憩エリアがあるか
        Dim workTime As Long = DateDiff("n", inRsltStartDateTime, inRsltEndDateTime)
        Dim hasRestTimeInServiceTime As Boolean = Me.HasRestTimeInServiceTime(inStallStartTime, _
                                                                              inStallEndTime, _
                                                                              inStallId, _
                                                                              inRsltStartDateTime, _
                                                                              workTime, _
                                                                              True)
        '休憩と重複場合、
        If hasRestTimeInServiceTime Then
            '画面に重複で表示してない
            If IsNothing(inRestFlg) Then
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E OverlapError", System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return TabletSMBCommonClassBusinessLogic.ActionResult.OverlapError
            End If
        End If

        '指定ストールにテクニシャンいない場合
        Dim hasTechnicianInStall As Boolean = Me.HasTechnicianInStall(inStallId, inStaffInfo)
        If Not hasTechnicianInStall Then
            Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0} NoTechnicianError. ", System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return ActionResult.NoTechnicianError
        End If

        '着工指示した作業終了時、実績データが持ってないエラー
        If Me.HasNoRsltData(inJobDtlId) Then
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E NoJobResultDataError", System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return TabletSMBCommonClassBusinessLogic.ActionResult.NoJobResultDataError
        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return ActionResult.Success
    End Function

    '2013/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
    ''' <summary>
    ''' 終了操作のいろいろなチェック
    ''' </summary>
    ''' <param name="chipEntity">操作チップのエンティティ</param>
    ''' <param name="inRsltEndDateTime">実績終了日時</param>
    ''' <param name="inRowLockVersion">行ロックバージョン</param>
    ''' <param name="inStaffInfo">スタッフ情報</param>
    ''' <param name="inUpdateDate">更新日時</param>
    ''' <param name="inSystemId">呼ぶ画面ID</param>
    ''' <param name="inJobStatus">選択した作業のステータス</param>
    ''' <param name="inLockTableFlg">サービス入庫テーブルをロックするかどうかフラグ</param>
    ''' <returns>チェック結果</returns>
    ''' <remarks></remarks>
    Private Function CheckSingleFinishAction(ByVal chipEntity As TabletSmbCommonClassChipEntityRow, _
                                             ByVal inRsltEndDateTime As Date, _
                                             ByVal inRowLockVersion As Long, _
                                             ByVal inStaffInfo As StaffContext, _
                                             ByVal inUpdateDate As Date, _
                                             ByVal inSystemId As String, _
                                             ByVal inJobStatus As String, _
                                             ByVal inLockTableFlg As Boolean) As Long

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.S. inRsltEndDateTime={1}, inRowLockVersion={2}, inUpdateDate={3}, inSystemId={4}, inLockTableFlg={5}, inJobStatus={6} ", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  inRsltEndDateTime, _
                                  inRowLockVersion, _
                                  inUpdateDate, _
                                  inSystemId, _
                                  inJobStatus, _
                                  inLockTableFlg))


        'ロックテーブルフラグがTrueの場合、テーブルロック処理をやる
        If inLockTableFlg Then

            'サービス入庫テーブルの行をロックして、チェックする
            Dim resultCheckVersion As Long = Me.LockServiceInTable(chipEntity.SVCIN_ID, _
                                                                   inRowLockVersion, _
                                                                   inStaffInfo.Account, _
                                                                   inUpdateDate, _
                                                                   inSystemId)

            'エラーがあれば、エラーコードを戻す
            If ActionResult.Success <> resultCheckVersion Then

                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E ErrorCode=LockServiceInTableError", System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return resultCheckVersion

            End If

        End If

        'RO NOを紐付けてるかチェックする
        If String.IsNullOrEmpty(chipEntity.RO_NUM.Trim()) Then

            '空白の場合、R/Oが紐づいていないエラーを戻す
            Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0} ErrorCode=NotSetroNoError. ", System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return ActionResult.NotSetroNoError

        End If

        '指定ストールにテクニシャンいるかどうかをチェックする
        Dim hasTechnicianInStall As Boolean = Me.HasTechnicianInStall(chipEntity.STALL_ID, inStaffInfo)

        '指定ストールにテクニシャンいない場合、テクニシャン未配置エラーを戻す
        If Not hasTechnicianInStall Then

            Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0} NoTechnicianError. ", System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return ActionResult.NoTechnicianError

        End If

        '作業ステータスチェック
        '作業ステータス遷移可否をチェックする
        If Not Me.CanFinishJob(inJobStatus) Then

            'エラーの場合、作業ステータスチェックエラーを戻す
            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                       "{0}.E InvalidJobStatusError", _
                                       MethodBase.GetCurrentMethod.Name))
            Return ActionResult.InvalidJobStatusError

        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return ActionResult.Success

    End Function
    '2013/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

    ''' <summary>
    ''' 指定作業内容IDに着工指示したデータが全部実績作業テーブルに持ってるかどうか
    ''' </summary>
    ''' <param name="inJobDtlId">作業内容ID</param>
    ''' <returns>true:持ってないデータがある</returns>
    ''' <remarks></remarks>
    Private Function HasNoRsltData(ByVal inJobDtlId As Decimal) As Boolean

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. inJobDtlId={1}" _
                                , MethodBase.GetCurrentMethod.Name, inJobDtlId))
        Dim numberTable As TabletSmbCommonClassNumberValueDataTable
        Using ta As New TabletSMBCommonClassDataAdapter
            numberTable = ta.GetNoRsltDataCount(inJobDtlId)
        End Using

        If numberTable(0).COL1 > 0 Then
            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                          "{0}_E Return True", _
                          System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return True
        Else
            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                          "{0}_E Return False", _
                          System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return False
        End If
    End Function
#End Region

#Region "中断操作に関するチェック"
    ''' <summary>
    ''' 中断操作で中断になるチェック
    ''' </summary>
    ''' <param name="chipEntity">操作チップエンティティ</param>
    ''' <param name="inRsltEndDateTime">実績終了日時</param>
    ''' <param name="inStallStartTime">営業開始日時</param>
    ''' <param name="inStallEndTime">営業終了日時</param>
    ''' <param name="inStallWaitTime">中断時間</param>
    ''' <param name="inRowLockVersion">行ロックバージョン</param>
    ''' <param name="inRestFlg">休憩取るフラグ</param>
    ''' <param name="objStaffContext">スタッフ情報</param>
    ''' <param name="inUpdateDate">更新日時</param>
    ''' <param name="inSystemId">呼ぶ画面ID</param>
    ''' <returns>チェック結果</returns>
    ''' <remarks></remarks>
    Private Function CheckChangeToStopChipByStop(ByVal chipEntity As TabletSmbCommonClassChipEntityRow, _
                                                 ByVal inRsltEndDateTime As Date, _
                                                 ByVal inStallStartTime As Date, _
                                                 ByVal inStallEndTime As Date, _
                                                 ByVal inStallWaitTime As Long, _
                                                 ByVal inRowLockVersion As Long, _
                                                 ByVal inRestFlg As String, _
                                                 ByVal objStaffContext As StaffContext, _
                                                 ByVal inUpdateDate As Date, _
                                                 ByVal inSystemId As String) As Long

        Dim inStallId As Decimal = chipEntity.STALL_ID
        Dim inSvcinId As Decimal = chipEntity.SVCIN_ID
        Dim inStallUseId As Decimal = chipEntity.STALL_USE_ID
        Dim inStallUseStatus As String = chipEntity.STALL_USE_STATUS
        Dim inInspectionStatus As String = chipEntity.INSPECTION_STATUS
        Dim inRsltStartDateTime As Date = chipEntity.RSLT_START_DATETIME
        Dim inPrmsEndDateTime As Date = chipEntity.PRMS_END_DATETIME
        Dim inDealerCode As String = objStaffContext.DlrCD
        Dim inBrnCode As String = objStaffContext.BrnCD
        Dim inUpdateAccount As String = objStaffContext.Account

        Logger.Info(String.Format(CultureInfo.InvariantCulture _
                                , "{0}.S. inStallUseStatus={1}, inInspectionStatus={2}, inRsltStartDateTime={3}, inRsltEndDateTime={4} " _
                                , MethodBase.GetCurrentMethod.Name, _
                                inStallUseStatus, _
                                inInspectionStatus, _
                                inRsltStartDateTime, _
                                inRsltEndDateTime))

        ' サービス入庫をロックして、チェックする
        Dim result As Long = Me.LockServiceInTable(inSvcinId, _
                                                   inRowLockVersion, _
                                                   inUpdateAccount, _
                                                   inUpdateDate, _
                                                   inSystemId)
        If result <> TabletSMBCommonClassBusinessLogic.ActionResult.Success Then
            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                       "{0}.E LockServiceInTableError", _
                                       System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return result
        End If

        'ステータス遷移可否をチェックする
        If Not CanStop(inStallUseStatus) Then
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E CheckError" _
                    , MethodBase.GetCurrentMethod.Name))
            Return ActionResult.CheckError
        End If

        '検査ステータスが1(検査依頼中)の場合、中断できない
        Dim inspectionStatus As String = inInspectionStatus
        If inspectionStatus.Equals(InspectionApproval) Then
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E InspectionStatusStopError" _
                            , MethodBase.GetCurrentMethod.Name))
            Return ActionResult.InspectionStatusStopError
        End If

        '指定ストールにテクニシャンいない場合
        Dim hasTechnicianInStall As Boolean = Me.HasTechnicianInStall(inStallId, objStaffContext)
        If Not hasTechnicianInStall Then
            Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0} NoTechnicianError. ", System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return ActionResult.NoTechnicianError
        End If

        '開始時間から終了時間までの範囲に重複休憩エリアがあるか
        Dim workTime As Long = DateDiff("n", inRsltStartDateTime, inRsltEndDateTime)
        Dim hasRestTimeInServiceTime As Boolean = Me.HasRestTimeInServiceTime(inStallStartTime, _
                                                                              inStallEndTime, _
                                                                              inStallId, _
                                                                              inRsltStartDateTime, _
                                                                              workTime, _
                                                                              True)
        '休憩と重複場合、
        If hasRestTimeInServiceTime Then
            '画面に重複で表示してない
            If IsNothing(inRestFlg) Then
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E OverlapError", System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return TabletSMBCommonClassBusinessLogic.ActionResult.OverlapError
            End If
        End If

        '中断で移動不可エリア生成すれば
        If inStallWaitTime > 0 Then
            '2017/09/13 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加 START
            ''移動不可チップ生成範囲の重複チェック
            'Dim hasRestTimeFlg As Boolean = Me.HasRestTimeInServiceTime(inStallStartTime, _
            '                                                            inStallEndTime, _
            '                                                            inStallId, _
            '                                                            inRsltEndDateTime, _
            '                                                            inStallWaitTime, _
            '                                                            False)
            ''休憩または使用不可エリアと重複場合、
            'If hasRestTimeFlg Then
            '    Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.E OverlapError", System.Reflection.MethodBase.GetCurrentMethod.Name))
            '    Return ActionResult.OverlapUnavailableError
            'End If
            '2017/09/13 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加 END

            'ストール利用チップとの重複配置チェック
            Dim idleEndDateTime As Date = inRsltEndDateTime.AddMinutes(inStallWaitTime)

            Using ta As New TabletSMBCommonClassDataAdapter
                '重複してるチップの数を取得する
                Dim overlapChipNums As Long = ta.GetChipOverlapChipNums(inDealerCode, _
                                                                        inBrnCode, _
                                                                        inStallUseId, _
                                                                        inStallId, _
                                                                        inRsltEndDateTime, _
                                                                        idleEndDateTime, _
                                                                        DefaultDateTimeValueGet())
                If overlapChipNums > 0 Then
                    '重複のは自分以外の場合
                    If Not (overlapChipNums = 1 And inPrmsEndDateTime.CompareTo(inUpdateDate) > 0) Then
                        Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0}.E CheckChipOverlapPosition error. ", System.Reflection.MethodBase.GetCurrentMethod.Name))
                        Return ActionResult.OverlapUnavailableError
                    End If
                End If
            End Using
        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return ActionResult.Success
    End Function

    '2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発 START
    ''' <summary>
    ''' 中断操作で作業中または一部作業中断になるチェック
    ''' </summary>
    ''' <param name="chipEntity">操作チップのエンティティ</param>
    ''' <param name="inRowLockVersion">行ロックバージョン</param>
    ''' <param name="inStaffInfo">スタッフ情報</param>
    ''' <param name="inUpdateDate">更新日時</param>
    ''' <param name="inSystemId">呼ぶ画面ID</param>
    ''' <param name="inJobStatus">選択した作業のステータス</param>
    ''' <returns>チェック結果</returns>
    ''' <remarks></remarks>
    Private Function CheckChangeToWorkingChipByStop(ByVal chipEntity As TabletSmbCommonClassChipEntityRow, _
                                                    ByVal inRowLockVersion As Long, _
                                                    ByVal inStaffInfo As StaffContext, _
                                                    ByVal inUpdateDate As Date, _
                                                    ByVal inSystemId As String, _
                                                    ByVal inJobStatus As String) As Long

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.S. inRowLockVersion={1}, inUpdateDate={2}, inSystemId={3}, inJobStatus={4} ", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  inRowLockVersion, _
                                  inUpdateDate, _
                                  inSystemId, _
                                  inJobStatus))

        'サービス入庫テーブルの行をロックして、チェックする
        Dim resultLock As Long = Me.LockServiceInTable(chipEntity.SVCIN_ID, _
                                                       inRowLockVersion, _
                                                       inStaffInfo.Account, _
                                                       inUpdateDate, _
                                                       inSystemId)

        'エラーがあれば、エラーコードを戻す
        If ActionResult.Success <> resultLock Then

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                       "{0}.E ErrorCode=LockServiceInTableError", _
                                       System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return resultLock

        End If

        'RO NOを紐付けてるかチェックする
        If String.IsNullOrEmpty(chipEntity.RO_NUM.Trim()) Then

            '空白の場合、R/Oが紐づいていないエラーを戻す
            Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                       "{0} ErrorCode=NotSetroNoError. ", _
                                       System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return ActionResult.NotSetroNoError

        End If

        '指定ストールにテクニシャンいるかどうかをチェックする
        Dim hasTechnicianInStall As Boolean = Me.HasTechnicianInStall(chipEntity.STALL_ID, inStaffInfo)

        '指定ストールにテクニシャンいない場合、テクニシャン未配置エラーを戻す
        If Not hasTechnicianInStall Then

            Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                       "{0} NoTechnicianError. ", _
                                       System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return ActionResult.NoTechnicianError

        End If

        '作業ステータスチェック(値があるときチェック)
        If Not String.IsNullOrEmpty(inJobStatus) Then

            '作業ステータスが[0:作業中]以外の場合はエラーにする
            If Not JobStatusWorking.Equals(inJobStatus) Then

                'エラーの場合、作業ステータスチェックエラーを戻す
                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                           "{0}.E InvalidJobStatusError", _
                                           MethodBase.GetCurrentMethod.Name))
                Return ActionResult.InvalidJobStatusError

            End If

        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.E", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return ActionResult.Success

    End Function
    '2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発 END

    '2015/02/13 TMEJ 範 新プラットフォーム版i-CROP仕様変更対応 (SMBチップ検索及び作業中断機能強化) START
    ''' <summary>
    ''' 中断チップの終了操作のチェック
    ''' </summary>
    ''' <param name="inChipEntity">操作チップのエンティティ</param>
    ''' <param name="inRowLockVersion">行ロックバージョン</param>
    ''' <param name="inStaffAccount">ログインユーザーアカウント</param>
    ''' <param name="inUpdateDate">現在日時</param>
    ''' <param name="inSystemId">呼ぶ先プログラムID</param>
    ''' <returns>チェック結果 0：エラーなし/他の場合：エラーコード</returns>
    ''' <remarks></remarks>
    Private Function CheckStopFinishAction(ByVal inChipEntity As TabletSmbCommonClassChipEntityRow, _
                                           ByVal inRowLockVersion As Long, _
                                           ByVal inStaffAccount As String, _
                                           ByVal inUpdateDate As Date, _
                                           ByVal inSystemId As String) As Long

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.{1} Start inRowLockVersion={2}, inStaffAccount={3}, inUpdateDate={4}, inSystemId={5} ", _
                                  Me.GetType.ToString, _
                                  MethodBase.GetCurrentMethod.Name, _
                                  inRowLockVersion, _
                                  inStaffAccount, _
                                  inUpdateDate, _
                                  inSystemId))

        '行ロックバージョンが-1の場合、ストールロックが不要
        If inRowLockVersion <> -1 Then

            'サービス入庫テーブルの行をロックして、チェックする
            Dim resultCheckVersion As Long = Me.LockServiceInTable(inChipEntity.SVCIN_ID, _
                                                                   inRowLockVersion, _
                                                                   inStaffAccount, _
                                                                   inUpdateDate, _
                                                                   inSystemId)

            'ロック失敗の場合（ローカル変数ロック返却結果<>0時）
            If resultCheckVersion <> ActionResult.Success Then

                Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                           "{0}.{1} End [LockServiceInTableError]", _
                                           Me.GetType.ToString, _
                                           MethodBase.GetCurrentMethod.Name))

                Return resultCheckVersion

            End If

        End If

        'ステータス遷移可否をチェックする
        'ストール利用ステータスが「中断」以外の場合
        If Not CanFinish(inChipEntity.STALL_USE_STATUS) Then

            Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                       "{0}.{1} End [CanFinishError]", _
                                       Me.GetType.ToString, _
                                       MethodBase.GetCurrentMethod.Name))

            Return ActionResult.CheckError

        End If

        '着工指示した作業終了時、実績データが持ってないエラー
        If Me.HasNoRsltData(inChipEntity.JOB_DTL_ID) Then

            Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                       "{0}.{1} End [NoJobResultDataError]", _
                                       Me.GetType.ToString, _
                                       MethodBase.GetCurrentMethod.Name))

            Return TabletSMBCommonClassBusinessLogic.ActionResult.NoJobResultDataError

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.{1} End ReturnValue={2}", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  ActionResult.Success))

        Return ActionResult.Success

    End Function
    '2015/02/13 TMEJ 範 新プラットフォーム版i-CROP仕様変更対応 (SMBチップ検索及び作業中断機能強化) END
#End Region

#Region "Undo操作に関するチェック"

    ''' <summary>
    ''' Undo操作に関するチェック
    ''' </summary>
    ''' <param name="inStallUseId">ストール利用ID</param>
    ''' <param name="inStallId">ストールID</param>
    ''' <param name="inSvcinStatus">サービス入庫ステータス</param>
    ''' <param name="inStallUsestatus">ストール利用利用ステータス</param>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inBrnCodea">店舗コード</param>
    ''' <param name="inUndoStartTime">元先の開始日時</param>
    ''' <param name="inUndoEndTime">元先の終了日時</param>
    ''' <param name="inUpdateDate">更新日時</param>
    ''' <returns>チェック結果</returns>
    ''' <remarks></remarks>
    Private Function CheckUndoAction(ByVal inStallUseId As Decimal, _
                                     ByVal inStallId As Decimal, _
                                     ByVal inSvcinStatus As String, _
                                     ByVal inStallUsestatus As String, _
                                     ByVal inDealerCode As String, _
                                     ByVal inBrnCodea As String, _
                                     ByVal inUndoStartTime As Date, _
                                     ByVal inUndoEndTime As Date, _
                                     ByVal inUpdateDate As Date) As Long

        Logger.Info(String.Format(CultureInfo.InvariantCulture _
                                  , "{0}.S. inStallUseId={1}, inStallId={2}, inSvcinStatus={3}, inStallUsestatus={4}, inUndoStartTime={5}, inUndoEndTime={6}, inUpdateDate={7} " _
                                  , MethodBase.GetCurrentMethod.Name, _
                                  inStallUseId, _
                                  inStallId, _
                                  inSvcinStatus, _
                                  inStallUsestatus, _
                                  inUndoStartTime, _
                                  inUndoEndTime, _
                                  inUpdateDate))

        ' ステータス遷移可否をチェックする
        If Not CanWorkingChipUndo(inSvcinStatus, inStallUsestatus) Then
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E CheckError" _
                                    , MethodBase.GetCurrentMethod.Name))
            Return ActionResult.CheckError
        End If

        '戻る先に予約チップと重複するかどうかを判断する
        If Me.CheckRezChipOverlapPosition(inDealerCode, inBrnCodea, inStallUseId, inStallId, inUndoStartTime, inUndoEndTime, inUpdateDate) Then
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E OverlapError" _
                        , MethodBase.GetCurrentMethod.Name))
            Return ActionResult.OverlapError
        End If

        '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
        'ストール使用不可重複配置チェック
        If CheckStallUnavailableOverlapPosition(inUndoStartTime, inUndoEndTime, inStallId) Then
            Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0}.{1}.E OverlapUnavailableError " _
                                , Me.GetType.ToString, MethodBase.GetCurrentMethod.Name))
            Return ActionResult.ChipOverlapUnavailableError
        End If
        '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", System.Reflection.MethodBase.GetCurrentMethod.Name))
        Return ActionResult.Success
    End Function

#End Region
    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

    '2014/09/25 TMEJ 張 BTS-180 「洗車中に関連チップ作成すると予期せぬエラーメッセージ」対応 START
#Region "チップ変更時のサービスステータスチェック"

    ''' <summary>
    ''' チップ変更時のサービスステータスチェック
    ''' </summary>
    ''' <param name="inSvcStatus">サービスステータス</param>
    ''' <returns>チェック結果</returns>
    ''' <remarks>
    ''' 下記の場合、このチェックをする
    ''' ①関連チップコピーチェック
    ''' ②受付エリアからストール上に配置
    ''' </remarks>
    Private Function CheckSvcStatusByPlaningChip(ByVal inSvcStatus As String) As Integer

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.Start. inSvcStatus={1}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  inSvcStatus))

        Select Case inSvcStatus

            Case SvcStatusCarWashStart
                '洗車中

                '洗車中、チップ変更不可エラーを戻す
                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                           "{0}.End Return error code {1}(UnablePlanChipInWashingError).", _
                                           MethodBase.GetCurrentMethod.Name, _
                                           ActionResult.UnablePlanChipInWashingError))

                Return ActionResult.UnablePlanChipInWashingError

            Case SvcStatusInspectionStart
                '検査中

                '検査中、チップ変更不可エラーを戻す
                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                           "{0}.End Return error code {1}(UnablePlanChipInInspectingError).", _
                                           MethodBase.GetCurrentMethod.Name, _
                                           ActionResult.UnablePlanChipInInspectingError))

                Return ActionResult.UnablePlanChipInInspectingError

            Case SvcStatusDelivery
                '納車済

                '納車済の場合、チップ変更不可エラーを戻す
                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                           "{0}.End Return error code {1}(UnablePlanChipAfterDeliveriedError).", _
                                           MethodBase.GetCurrentMethod.Name, _
                                           ActionResult.UnablePlanChipAfterDeliveriedError))

                Return ActionResult.UnablePlanChipAfterDeliveriedError

        End Select


        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.End Return 0(Success).", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return ActionResult.Success

    End Function

#End Region
    '2014/09/25 TMEJ 張 BTS-180 「洗車中に関連チップ作成すると予期せぬエラーメッセージ」対応 END

#End Region

#Region "ディフォルト値チェック"
    ''' <summary>
    ''' ディフォルト値(DB数値型の既定値（0）)を判断する
    ''' </summary>
    ''' <param name="para"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function IsDefaultValue(ByVal para As Decimal) As Boolean
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. para={1}" _
            , MethodBase.GetCurrentMethod.Name, para))

        If para = 0 Then
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Return True", MethodBase.GetCurrentMethod.Name))
            Return True
        Else
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Return False", MethodBase.GetCurrentMethod.Name))
            Return False
        End If
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
    ''' ディフォルト値(DB日付型の既定値（1900-1-1 00:00:00）)を取得する
    ''' </summary>
    ''' <returns>ディフォルト日付</returns>
    ''' <remarks></remarks>
    Private Function DefaultDateTimeValueGet() As Date
        Return Date.Parse("1900/01/01 00:00:00", CultureInfo.InvariantCulture)
    End Function
#End Region

    '2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 START
    '#Region "チップステータス判定"
    '    ''' <summary>
    '    ''' チップのステータス判定
    '    ''' </summary>
    '    ''' <param name="stallUseId">ストール利用ID</param>
    '    ''' <returns>
    '    ''' チップのステータスを判定し、以下のいずれかのチップステータスを返却する。
    '    ''' 1：未入庫(仮予約)、2：未入庫(本予約)、3：作業開始待ち(仮予約)、
    '    ''' 4：作業開始待ち(本予約)、6：未来店客(本予約)、7：飛び込み客、
    '    ''' 8：作業中、9：中断・部品欠品、10：中断・お客様連絡待ち、
    '    ''' 11：中断・ストール待機、12：中断・その他、13：中断・検査中断、
    '    ''' 14：洗車待ち、15：洗車中、16：検査待ち、17：検査中、18：預かり中、
    '    ''' 19：納車待ち、20：次の作業開始待ち、22：納車済み、24：未来店客(仮予約)
    '    ''' </returns>
    '    ''' <remarks></remarks>
    '    Public Function JudgeChipStatus(ByVal stallUseId As Long) As String

    '        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
    '                                  "{0}.S IN:stallUseId={1}" _
    '                                , MethodBase.GetCurrentMethod.Name, stallUseId))

    '        '戻り値
    '        Dim retValue As String = String.Empty

    '        'エラー発生フラグ
    '        Dim errorFlg As Boolean = False

    '        Try
    '            'チップエンティティ
    '            Dim chipEntityTable As TabletSmbCommonClassChipEntityDataTable

    '            'TabletSMBCommonClassのテーブルアダプタークラスインスタンスを生成
    '            Using myTableAdapter As New TabletSMBCommonClassDataAdapter
    '                'チップ情報取得
    '                chipEntityTable = myTableAdapter.GetChipEntity(stallUseId, 0)
    '                Me.OutPutIFLog(chipEntityTable, "ChipEntityTable:") 'ログ
    '            End Using

    '            'チップ情報が取得できない場合はエラー
    '            If chipEntityTable.Count <= 0 Then
    '                Logger.Error(String.Format(CultureInfo.InvariantCulture, _
    '                       "{0}.Error Err:Failed to get the chip information.", _
    '                       MethodBase.GetCurrentMethod.Name))
    '                errorFlg = True
    '                Exit Try
    '            End If

    '            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.DirectCast Row Start", MethodBase.GetCurrentMethod.Name))
    '            'データ行の抜き出し
    '            Dim chipEntityRow As TabletSmbCommonClassChipEntityRow _
    '                = DirectCast(chipEntityTable.Rows(0), TabletSmbCommonClassChipEntityRow)
    '            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.DirectCast Row End", MethodBase.GetCurrentMethod.Name))
    '            'サービスステータス
    '            Dim svcStatus As String = chipEntityRow.SVC_STATUS
    '            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.Set SVC_STATUS={1}", MethodBase.GetCurrentMethod.Name, chipEntityRow.SVC_STATUS))
    '            '予約ステータス
    '            Dim resvStatus As String = chipEntityRow.RESV_STATUS
    '            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.Set RESV_STATUS={1}", MethodBase.GetCurrentMethod.Name, chipEntityRow.RESV_STATUS))
    '            'ストール利用ステータス
    '            Dim stallUseStatus As String = chipEntityRow.STALL_USE_STATUS
    '            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.Set STALL_USE_STATUS={1}", MethodBase.GetCurrentMethod.Name, chipEntityRow.STALL_USE_STATUS))
    '            '中断理由区分
    '            Dim stopReasonType As String = chipEntityRow.STOP_REASON_TYPE
    '            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.Set STOP_REASON_TYPE={1}", MethodBase.GetCurrentMethod.Name, chipEntityRow.STOP_REASON_TYPE))
    '            '関連ストール非稼動ID
    '            Dim stallIdleId As Long = chipEntityRow.STALL_IDLE_ID
    '            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.Set STALL_IDLE_ID={1}", MethodBase.GetCurrentMethod.Name, chipEntityRow.STALL_IDLE_ID))
    '            '受付区分
    '            Dim acceptanceType As String = chipEntityRow.ACCEPTANCE_TYPE
    '            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.Set ACCEPTANCE_TYPE={1}", MethodBase.GetCurrentMethod.Name, chipEntityRow.ACCEPTANCE_TYPE))
    '            'ストールID
    '            Dim stallId As Long = chipEntityRow.STALL_ID
    '            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.Set STALL_ID={1}", MethodBase.GetCurrentMethod.Name, chipEntityRow.STALL_ID))
    '            'サービスステータスによって分岐
    '            Select Case svcStatus

    '                Case SvcStatusNotCarin
    '                    'サービスステータス「00：未入庫」の場合
    '                    retValue = Me.JudgeNotCarInStatus(resvStatus)

    '                Case SvcStatusNoShow
    '                    'サービスステータス「01：未来店客」の場合
    '                    retValue = Me.JudgeNoShowStatus(resvStatus, stallUseStatus)

    '                Case SvcStatusWorkOrderWait, SvcStatusStartwait, SvcStatusNextStartWait
    '                    'サービスステータス「03：着工指示待ち」「04：作業開始待ち」「06：次の作業開始待ち」の場合
    '                    retValue = Me.JudgeWaitStartStatus(resvStatus, stallUseStatus, stopReasonType, stallIdleId, acceptanceType, stallId)

    '                Case SvcStatusStart
    '                    'サービスステータス「05：作業中」
    '                    retValue = Me.JudgeStartStatus(stallUseStatus, stopReasonType, stallIdleId)

    '                Case SvcStatusCarWashWait
    '                    'サービスステータス「07：洗車待ち」→チップステータス【14:洗車待ち】
    '                    retValue = ChipStatusWaitWash

    '                Case SvcStatusCarWashStart
    '                    'サービスステータス「08：洗車中」→チップステータス【15:洗車中】
    '                    retValue = ChipStatusWashing

    '                Case SvcStatusInspectionWait
    '                    'サービスステータス「09：検査待ち」→チップステータス【16:検査待ち】
    '                    retValue = ChipStatusWaitInspection

    '                Case SvcStatusInspectionStart
    '                    'サービスステータス「10：検査中」→チップステータス【17:検査中】
    '                    retValue = ChipStatusInspecting

    '                Case SvcStatusDropOffCustomer
    '                    'サービスステータス「11：預かり中」→チップステータス【18:預かり中】
    '                    retValue = ChipStatusKeeping

    '                Case SvcStatusWaitingCustomer
    '                    'サービスステータス「12：納車待ち」→チップステータス【19:納車待ち】
    '                    retValue = ChipStatusWaitDelivery

    '                Case SvcStatusDelivery
    '                    'サービスステータス「13：納車済み」→チップステータス【22:納車済み】
    '                    retValue = ChipStatusDeliveryEnd

    '            End Select

    '        Finally
    '            If errorFlg Then
    '                retValue = String.Empty
    '            End If
    '        End Try

    '        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
    '                                  "{0}.E OUT:retValue={1}", _
    '                                  MethodBase.GetCurrentMethod.Name, retValue))

    '        Return retValue

    '    End Function

    '    ''' <summary>
    '    ''' サービスステータス「00:未入庫」の場合のチップステータスを判定
    '    ''' </summary>
    '    ''' <param name="resvStatus">予約ステータス</param>
    '    ''' <returns></returns>
    '    ''' <remarks></remarks>
    '    Private Function JudgeNotCarInStatus(ByVal resvStatus As String) As String

    '        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
    '                                  "{0}.S IN:resvStatus={1}", _
    '                                  MethodBase.GetCurrentMethod.Name, resvStatus))

    '        '戻り値
    '        Dim retValue As String = String.Empty

    '        If resvStatus.Equals(ResvStatusTentative) Then
    '            'チップステータス【1：未入庫(仮予約)】
    '            retValue = ChipStatusTentativeNotCarIn
    '        Else
    '            'チップステータス【2：未入庫(本予約)】
    '            retValue = ChipStatusConfirmedNotCarIn
    '        End If

    '        Return retValue

    '        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
    '                                  "{0}.E OUT:retValue={1}", _
    '                                  MethodBase.GetCurrentMethod.Name, retValue))

    '    End Function

    '    ''' <summary>
    '    ''' サービスステータス「01：未来店客」の場合のチップステータスを判定
    '    ''' </summary>
    '    ''' <param name="resvStatus">予約ステータス</param>
    '    ''' <param name="stallUseStatus">ストール利用ステータス</param>
    '    ''' <returns></returns>
    '    ''' <remarks></remarks>
    '    Private Function JudgeNoShowStatus(ByVal resvStatus As String, ByVal stallUseStatus As String) As String

    '        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
    '                                  "{0}.S IN:resvStatus={1}, stallUseStatus={2}", _
    '                                  MethodBase.GetCurrentMethod.Name, resvStatus, stallUseStatus))

    '        '戻り値
    '        Dim retValue As String = String.Empty

    '        If stallUseStatus.Equals(StalluseStatusNoshow) Then
    '            'ストール利用ステータス「07：未来店客」の場合

    '            If resvStatus.Equals(ResvStatusTentative) Then
    '                'チップステータス【24：未来店客(仮予約)】
    '                retValue = ChipStatusTentativeNoShow
    '            Else
    '                'チップステータス【6：未来店客(本予約)】
    '                retValue = ChipStatusNoshow
    '            End If

    '        ElseIf stallUseStatus.Equals(StalluseStatusWorkOrderWait) _
    '        OrElse stallUseStatus.Equals(StalluseStatusStartWait) Then
    '            'ストール利用ステータス「00：着工指示待ち」または「01：作業開始待ち」の場合

    '            If resvStatus.Equals(ResvStatusTentative) Then
    '                'チップステータス【1：未入庫(仮予約)】
    '                retValue = ChipStatusTentativeNotCarIn
    '            Else
    '                'チップステータス【2：未入庫(本予約)】
    '                retValue = ChipStatusConfirmedNotCarIn
    '            End If
    '        End If

    '        Return retValue

    '        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
    '                                  "{0}.E OUT:retValue={1}", _
    '                                  MethodBase.GetCurrentMethod.Name, retValue))

    '    End Function

    '    ''' <summary>
    '    ''' サービスステータス「03:着工指示待ち」「04:作業開始待ち」「06:次の作業開始待ち」の場合のチップステータスを判定
    '    ''' </summary>
    '    ''' <param name="resvStatus">予約ステータス</param>
    '    ''' <param name="stallUseStatus">ストール利用ステータス</param>
    '    ''' <param name="stopReasonType">中断理由区分</param>
    '    ''' <param name="stallIdleId">関連ストール非稼動ID</param>
    '    ''' <param name="acceptanceType">受付区分</param>
    '    ''' <param name="stallId">ストールID</param>
    '    ''' <returns></returns>
    '    ''' <remarks></remarks>
    '    Private Function JudgeWaitStartStatus(ByVal resvStatus As String, ByVal stallUseStatus As String, _
    '                                          ByVal stopReasonType As String, ByVal stallIdleId As Long, _
    '                                          ByVal acceptanceType As String, ByVal stallId As Long) As String

    '        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
    '                                  "{0}.S IN:resvStatus={1}, stallUseStatus={2}, stopReasonType={3}, stallIdleId={4}, acceptanceType={5}, stallId={6}", _
    '                                  MethodBase.GetCurrentMethod.Name, resvStatus, stallUseStatus, stopReasonType, stallIdleId, acceptanceType, stallId))

    '        '戻り値
    '        Dim retValue As String = String.Empty

    '        'ストール利用ステータスで分岐
    '        Select Case stallUseStatus

    '            Case StalluseStatusFinish
    '                'ストール利用ステータス「03：完了」の場合
    '                'チップステータス【20：次の作業開始待ち】
    '                retValue = ChipStatusJobFinish

    '            Case StalluseStatusStop
    '                'ストール利用ステータス「05：中断」の場合
    '                retValue = Me.JudgeStopStatus(stopReasonType, stallIdleId)

    '            Case StalluseStatusWorkOrderWait, StalluseStatusStartWait
    '                'ストール利用ステータス「00：着工指示待ち」「01：作業開始待ち」の場合

    '                If acceptanceType.Equals(AcceptanceTypeWalkin) _
    '                AndAlso stallId = DefaultNumberValue Then
    '                    '受付区分が「1：Walk-in」、かつストールIDが未設定の場合
    '                    'チップステータス【7：Walk-in】
    '                    retValue = ChipStatusWalkin

    '                Else
    '                    If resvStatus.Equals(ResvStatusTentative) Then
    '                        'チップステータス【3：作業開始待ち(仮予約)】
    '                        retValue = ChipStatusTentativeWaitStart
    '                    Else
    '                        'チップステータス【4：作業開始待ち(本予約)】
    '                        retValue = ChipStatusConfirmedWaitStart
    '                    End If
    '                End If

    '        End Select

    '        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
    '                                  "{0}.E OUT:retValue={1}", _
    '                                  MethodBase.GetCurrentMethod.Name, retValue))

    '        Return retValue

    '    End Function

    '    ''' <summary>
    '    ''' ストール利用ステータス「05:中断」の場合のチップステータス判定
    '    ''' </summary>
    '    ''' <param name="stopReasonType">中断理由区分</param>
    '    ''' <param name="stallIdleId">関連ストール非稼動ID</param>
    '    ''' <returns>
    '    ''' チップのステータスを判定し、以下のいずれかのチップステータスを返却する。
    '    ''' 9：中断・部品欠品、10：中断・お客様連絡待ち、11：中断・ストール待機、
    '    ''' 12：中断・その他、13：中断・検査中断
    '    ''' </returns>
    '    ''' <remarks></remarks>
    '    Private Function JudgeStopStatus(ByVal stopReasonType As String, ByVal stallIdleId As Long) As String

    '        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
    '                                  "{0}.S IN:stopReasonType={1}, stallIdleId={2}", _
    '                                  MethodBase.GetCurrentMethod.Name, stopReasonType, stallIdleId))

    '        '戻り値
    '        Dim retValue As String = String.Empty

    '        If stallIdleId <> DefaultNumberValue Then
    '            '関連ストール非稼動IDが設定されている場合
    '            'チップステータス【11：作業中断(ストール待ち)】
    '            retValue = ChipStatusStopForWaitStall
    '        Else
    '            '中断理由区分で分岐
    '            Select Case stopReasonType
    '                Case StopReasonPartsStockOut
    '                    '中断理由区分「01：部品欠品」の場合
    '                    'チップステータス【09：作業中断(部品欠品)】
    '                    retValue = ChipStatusStopForPartsStockout

    '                Case StopReasonCustomerReportWaiting
    '                    '中断理由区分「02：お客様連絡待ち」の場合
    '                    'チップステータス【10：作業中断(お客様連絡待ち)】
    '                    retValue = ChipStatusStopForWaitCustomer

    '                Case StopReasonInspectionFailure
    '                    '中断理由区分「03：検査不合格」の場合
    '                    'チップステータス【13：作業中断(検査中断)】
    '                    retValue = ChipStatusStopForInspection

    '                Case StopReasonOthers
    '                    '中断理由区分が「99：その他」の場合
    '                    'チップステータス【12：作業中断(その他)】
    '                    retValue = ChipStatusStopForOtherReason

    '                Case Else
    '                    '中断理由区分が上記以外の場合
    '                    'チップステータス【12：作業中断(その他)】
    '                    retValue = ChipStatusStopForOtherReason
    '            End Select
    '        End If

    '        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
    '                                  "{0}.E OUT:retValue={1}", _
    '                                  MethodBase.GetCurrentMethod.Name, retValue))

    '        Return retValue

    '    End Function

    '    ''' <summary>
    '    ''' サービスステータス「05:作業中」の場合のチップステータス判定
    '    ''' </summary>
    '    ''' <param name="stallUseStatus">ストール利用ステータス</param>
    '    ''' <param name="stopReasonType">中断理由区分</param>
    '    ''' <param name="stallIdleId">関連ストール非稼動ID</param>
    '    ''' <returns>
    '    ''' チップのステータスを判定し、以下のいずれかのチップステータスを返却する。
    '    ''' 4：作業開始待ち（本予約）、8：作業中、9：中断・部品欠品、
    '    ''' 10：中断・お客様連絡待ち、11：中断・ストール待機、12：中断・その他、13：中断・検査中断、20：作業完了
    '    ''' </returns>
    '    ''' <remarks></remarks>
    '    Private Function JudgeStartStatus(ByVal stallUseStatus As String, ByVal stopReasonType As String, ByVal stallIdleId As Long) As String

    '        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
    '                                  "{0}.S IN:stallUseStatus={1}, stopReasonType={2}, stallIdleId={3}", _
    '                                  MethodBase.GetCurrentMethod.Name, stallUseStatus, stopReasonType, stallIdleId))

    '        '戻り値
    '        Dim retValue As String = String.Empty

    '        'ストール利用ステータスで分岐
    '        Select Case stallUseStatus

    '            Case StalluseStatusWorkOrderWait, StalluseStatusStartWait
    '                'ストール利用ステータス「00：着工指示待ち」「01：作業開始待ち」の場合
    '                'チップステータス【4：作業開始待ち(本予約)】
    '                retValue = ChipStatusConfirmedWaitStart

    '            Case StalluseStatusStart, StalluseStatusStartIncludeStopJob
    '                'ストール利用ステータス「02：作業中」「04：作業指示の一部の作業が中断」の場合
    '                'チップステータス【8：作業中】
    '                retValue = ChipStatusWorking

    '            Case StalluseStatusStop
    '                'ストール利用ステータス「05：中断」の場合
    '                retValue = Me.JudgeStopStatus(stopReasonType, stallIdleId)

    '            Case StalluseStatusFinish
    '                'ストール利用ステータス「03：完了」の場合
    '                'チップステータス【20：次の作業開始待ち】
    '                retValue = ChipStatusJobFinish

    '        End Select

    '        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
    '                                  "{0}.E OUT:retValue={1}", _
    '                                  MethodBase.GetCurrentMethod.Name, retValue))

    '        Return retValue

    '    End Function
    '#End Region
    '2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 END

    '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
    ' ''' <summary>
    ' ''' 追加作業チェック
    ' ''' </summary>
    ' ''' <param name="inDlrCode">販売店コード</param>
    ' ''' <param name="inRONum">RO番号</param>
    ' ''' <returns>洗車、納車処理フラグ</returns>
    ' ''' <remarks></remarks>
    'Private Function CheckAddWorkStatus(ByVal inDlrCode As String, ByVal inRONum As String) As Boolean

    '    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. inDlrCode={1}, inRONum={2}" _
    '                , MethodBase.GetCurrentMethod.Name, inDlrCode, inRONum))
    '    Dim operationFlg As Boolean = False
    '    If Not String.IsNullOrWhiteSpace(inRONum) Then
    '        Dim bizAddRepairStatusList As New IC3800804BusinessLogic
    '        Dim dtAddRepairStatus As IC3800804AddRepairStatusDataTableDataTable = _
    '        DirectCast(bizAddRepairStatusList.GetAddRepairStatusList(inDlrCode, inRONum),  _
    '            IC3800804AddRepairStatusDataTableDataTable)

    '        If Not IsNothing(dtAddRepairStatus) Or 0 < dtAddRepairStatus.Count Then
    '            '追加作業が存在する場合
    '            Dim rowAddList As IC3800804AddRepairStatusDataTableRow() = _
    '            (From col In dtAddRepairStatus Where col.STATUS <> "9" Select col).ToArray
    '            If 0 < rowAddList.Count Then
    '                '「追加作業ステータス≠9」が存在する場合
    '                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E return=False", MethodBase.GetCurrentMethod.Name))
    '                Return False
    '            Else
    '                operationFlg = True
    '            End If
    '        Else
    '            '追加作業が存在しない場合
    '            operationFlg = True
    '        End If
    '    Else
    '        operationFlg = True
    '    End If
    '    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E return=True", MethodBase.GetCurrentMethod.Name))
    '    Return operationFlg
    'End Function

    ''' <summary>
    ''' 納車前チェック
    ''' </summary>
    ''' <param name="inRONum">RO番号</param>
    ''' <returns>処理続行フラグ(True:処理する/False:処理しない)</returns>
    ''' <remarks>
    ''' 追加作業を含めた全てのROステータスが85(納車準備済み)の場合は0、
    ''' それ以外は0以外を返却する
    ''' </remarks>
    Private Function CheckBeforeDelivery(ByVal inRONum As String) As Integer

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.S. inRONum={1}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  inRONum))

        Dim returnCode As Integer = 0

        Using commonDataAdapter As New TabletSMBCommonClassDataAdapter

            '追加作業を含めた全てのROステータスを取得する
            '2014/09/12 TMEJ 張 BTS-392「TopServのROとSMBのJOBが不一致」対応 START
            Dim userInfo As StaffContext = StaffContext.Current

            'Dim dtRoStatus As TabletSmbCommonClassROStatusDataTable _
            '    = commonDataAdapter.GetROStatusInfo(inRONum)

            Dim dtRoStatus As TabletSmbCommonClassROStatusDataTable = _
                commonDataAdapter.GetROStatusInfo(inRONum, _
                                                  userInfo.DlrCD, _
                                                  userInfo.BrnCD)
            '2014/09/12 TMEJ 張 BTS-392「TopServのROとSMBのJOBが不一致」対応 END


            If Not IsNothing(dtRoStatus) _
            AndAlso 0 < dtRoStatus.Count Then
                'データが存在する場合(運用上必ずあるはず)

                Dim strSelect As String = String.Format(CultureInfo.InvariantCulture, _
                                                        "RO_STATUS <> '{0}'", _
                                                        RostatusClosingJob)

                'ROステータスが85(納車準備済み)でないレコードを絞り込む
                Dim drRoStatus As TabletSmbCommonClassROStatusRow() _
                    = CType(dtRoStatus.Select(strSelect), TabletSmbCommonClassROStatusRow())

                'ROステータスが85(納車準備済み)でないレコードがある場合、
                If 0 < drRoStatus.Count Then
                    returnCode = ActionResult.CheckInvoicePrintDateTimeError
                End If

            End If

        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.E. returnCode={1}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  returnCode))
        Return returnCode

    End Function

    '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END


End Class



