'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3240101BusinessLogic.vb
'─────────────────────────────────────
'機能： タブレットSMBの工程管理メインのビジネスロジック
'補足： 
'作成： 2013/06/05 TMEJ 張 タブレット版SMB機能開発(工程管理)
'更新： 2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発
'更新： 2014/07/02 TMEJ 張 BTS-283 「サービスマネージャをTCとして表示」対応
'更新： 2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発
'更新： 2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応
'更新： 2015/02/09 TMEJ 範 DMS連携版サービスタブレット SMB納車予定時刻通知機能開発
'更新： 2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発
'更新： 2015/09/08 TMEJ 皆川 タブレットSMB ストールグループ表示対応
'更新： 2016/01/12 NSK 皆川 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応
'更新： 2016/04/20 NSK 小牟禮 工程管理の初期表示処理性能改善対応
'更新： 2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする
'更新： 2017/09/07 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加 
'更新： 2017/10/04 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一
'更新： 2019/10/31 NSK 鈴木【（FS）MaaSビジネス向けサービス予約の登録オペレーションの効率化に向けた試験研究】[No156]予定入庫日時、予定納車日時の自動セット
'更新： 2019/07/22 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証
'更新： 2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証
'更新：
'─────────────────────────────────────

Option Strict On
Option Explicit On

Imports System.Globalization
Imports System.Web.Script.Serialization
Imports Toyota.eCRB.SMB.ProcessManagement
Imports Toyota.eCRB.SMB.ProcessManagement.DataAccess
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.BizLogic
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web
Imports System.Text
'2015/02/09 TMEJ 範 DMS連携版サービスタブレット SMB納車予定時刻通知機能開発 START
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.DataAccess.TabletSMBCommonClassDataSet
'2015/02/09 TMEJ 範 DMS連携版サービスタブレット SMB納車予定時刻通知機能開発 END
'2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.BizLogic.TabletSMBCommonClassBusinessLogic
'2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END

Public Class SC3240101BusinessLogic
    Inherits BaseBusinessComponent

#Region "定数"

    '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
    ''' <summary>Job開始した後、Push送信やる必要フラグ (True:Push送信 False:Push送信しない)</summary>
    Public Property NeedPushAfterStartJob As Boolean
    ''' <summary>Job終了した後、Push送信やる必要フラグ (True:Push送信 False:Push送信しない)</summary>
    Public Property NeedPushAfterFinishJob As Boolean
    ''' <summary>Job中断した後、Push送信やる必要フラグ (True:Push送信 False:Push送信しない)</summary>
    Public Property NeedPushAfterStopJob As Boolean
    '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

    ''' <summary>DateTimeFuncにて、"yyyy/MM/dd HH:mm"形式をコンバートするためのID</summary>
    Private Const DATE_CONVERT_ID_YYYYMMDDHHMM As Integer = 2
    ''' <summary>DateTimeFuncにて、"yyyyMMdd"形式をコンバートするためのID</summary>
    Private Const DATE_CONVERT_ID_YYYYMMDD As Integer = 9
    ''' <summary>プログラムID：工程管理(SMB)</summary>
    Private Const PGMID_SMB As String = "SC3240101"

    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    ''' <summary>部品ステータス情報取得結果コード</summary>
    Public Property IC3802503ResultValue As Long

    ''' <summary>削除されたチップに作業指示情報</summary>
    Public Property TabletSmbCommonCancelInstructedChipInfo As DataTable
    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

#Region "サービスステータス"
    ''' <summary>
    ''' 未入庫
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SvcStatusNotCarIn As String = "0"
    ''' <summary>
    ''' 未来店客（予定通りの入庫がないため一旦チップをNoShowエリアに移動している）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SvcStatusNoshow As String = "1"
    ''' <summary>
    ''' キャンセル
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SvcStatusCancel As String = "2"
    ''' <summary>
    ''' 着工指示待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SvcStatusWorkOrderWait As String = "3"
    ''' <summary>
    ''' 作業開始待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SvcStatusStartWait As String = "4"
    ''' <summary>
    ''' 作業中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SvcStatusStart As String = "5"
    ''' <summary>
    ''' 次の作業開始待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SvcStatusNextStartWait As String = "6"
    ''' <summary>
    ''' 洗車待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SvcStatusCarWashWait As String = "7"
    ''' <summary>
    ''' 洗車中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SvcStatusCarWashStart As String = "8"
    ''' <summary>
    ''' 検査待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SvcStatusInspectionWait As String = "9"
    ''' <summary>
    ''' 検査中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SvcStatusInspectionStart As String = "10"
    ''' <summary>
    ''' 預かり中（DropOff）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SvcStatusDropOffCustomer As String = "11"
    ''' <summary>
    ''' 納車待ち（Waiting）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SvcStatusWaitingCustomer As String = "12"
    ''' <summary>
    ''' 作業開始待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SvcStatusDelivery As String = "13"
#End Region

#Region "休憩取得フラグ"
    ''' <summary>
    ''' 取得しない（取得しなかった）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RestTimeGetFlgNoGetRest As String = "0"
    ''' <summary>
    ''' 取得する（取得した）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RestTimeGetFlgGetRest As String = "1"
#End Region

    '2019/07/22 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
#Region "ストール利用ステータス"
    ''' <summary>
    ''' 作業中
    ''' </summary>
    ''' <remarks></remarks>
    Public Const StalluseStatusStart As String = "02"
    ''' <summary>
    ''' 作業計画の一部の作業が中断
    ''' </summary>
    ''' <remarks></remarks>
    Public Const StalluseStatusStartIncludeStopJob As String = "04"
#End Region
    '2019/07/22 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END

#End Region

#Region "プロパティ"
    Public Property NewStallIdleId As Decimal

    '2014/07/02 TMEJ 張 BTS-283 「サービスマネージャをTCとして表示」対応 START
    Private Property StfStallDispType As String = String.Empty
    '2014/07/02 TMEJ 張 BTS-283 「サービスマネージャをTCとして表示」対応 END
#End Region

#Region "JSON変換"

    ''' <summary>
    '''   DataTableをJSON文字列に変換する
    ''' </summary>
    ''' <param name="dataTable">変換対象 DataSet</param>
    ''' <returns>JSON文字列</returns>
    Public Function DataTableToJson(ByVal dataTable As DataTable) As String
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S.", System.Reflection.MethodBase.GetCurrentMethod.Name))
        Dim resultMain As New Dictionary(Of String, Object)
        Dim JSerializer As New JavaScriptSerializer

        If dataTable Is Nothing Then
            Return JSerializer.Serialize(resultMain)
        End If

        For Each dr As DataRow In dataTable.Rows
            Dim result As New Dictionary(Of String, Object)

            For Each dc As DataColumn In dataTable.Columns
                result.Add(dc.ColumnName, dr(dc).ToString)
            Next
            resultMain.Add("Key" + CType(resultMain.Count + 1, String), result)
        Next
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", System.Reflection.MethodBase.GetCurrentMethod.Name))
        Return JSerializer.Serialize(resultMain)
    End Function

#End Region

#Region "店舗設定の取得"

    ''' <summary>
    ''' 店舗稼動時間情報の取得
    ''' </summary>
    ''' <param name="objStaffContext">スタッフ情報</param>
    ''' <returns>店舗稼動時間情報テーブル</returns>
    Public Function GetBranchOperatingHours(ByVal objStaffContext As StaffContext) As TabletSMBCommonClassDataSet.TabletSmbCommonClassBranchOperatingHoursDataTable
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S.", System.Reflection.MethodBase.GetCurrentMethod.Name))
        Dim dt As TabletSMBCommonClassDataSet.TabletSmbCommonClassBranchOperatingHoursDataTable

        Using clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic
            dt = clsTabletSMBCommonClass.GetBranchOperatingHours(objStaffContext.DlrCD, objStaffContext.BrnCD)
        End Using
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", System.Reflection.MethodBase.GetCurrentMethod.Name))
        Return dt
    End Function

    ''' <summary>
    ''' リサイズ単位の取得
    ''' </summary>
    ''' <param name="objStaffContext">スタッフ情報</param>
    ''' <returns>リサイズ単位</returns>
    Public Function GetIntervalTime(ByVal objStaffContext As StaffContext) As Long
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S.", System.Reflection.MethodBase.GetCurrentMethod.Name))
        Dim intervalTime As Long
        Using clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic
            intervalTime = clsTabletSMBCommonClass.GetIntervalTime(objStaffContext.DlrCD, objStaffContext.BrnCD)
        End Using
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", System.Reflection.MethodBase.GetCurrentMethod.Name))
        Return intervalTime
    End Function

    '2017/10/04 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
    ''' <summary>
    ''' 洗車標準所要時間を取得
    ''' </summary>
    ''' <returns></returns>
    'Public Function GetStandardWashTime(ByVal objStaffContext As StaffContext) As Long
    '    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S", System.Reflection.MethodBase.GetCurrentMethod.Name))

    '    Dim washTime As Long = 0
    '    Using ta As New SC3240101DataSetTableAdapters.SC3240101DataAdapter
    '        washTime = ta.GetStandardWashTime(objStaffContext.DlrCD, objStaffContext.BrnCD)
    '    End Using

    '    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E. washTime={1}", System.Reflection.MethodBase.GetCurrentMethod.Name, washTime))
    '    Return washTime
    'End Function

    ''' <summary>
    ''' 標準洗車時間、標準検査時間、標準納車時間を取得
    ''' </summary>
    ''' <returns></returns>
    Public Function GetServiceSettingTime(ByVal objStaffContext As StaffContext) As SC3240101DataSet.SC3240101ServiceSettingDataTable
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S", System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim dt As SC3240101DataSet.SC3240101ServiceSettingDataTable
        Using ta As New SC3240101DataSetTableAdapters.SC3240101DataAdapter
            dt = ta.GetServiceSetting(objStaffContext.DlrCD, objStaffContext.BrnCD)
        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E.", System.Reflection.MethodBase.GetCurrentMethod.Name))
        Return dt
    End Function
    '2017/10/04 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END

    ' 2019/10/31 NSK 鈴木【（FS）MaaSビジネス向けサービス予約の登録オペレーションの効率化に向けた試験研究】[No156]予定入庫日時、予定納車日時の自動セット START
    ' ''' <summary>
    ' ''' 納車作業工程標準作業時間(分)を取得
    ' ''' </summary>
    ' ''' <param name="objStaffContext">スタッフ情報</param>
    ' ''' <returns>標準作業時間テーブル</returns>
    'Public Function GetStandardDeliveryTime(ByVal objStaffContext As StaffContext) As SC3240101DataSet.SC3240101StandardTimeDataTable
    '    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S", System.Reflection.MethodBase.GetCurrentMethod.Name))

    '    Dim dt As SC3240101DataSet.SC3240101StandardTimeDataTable
    '    Using ta As New SC3240101DataSetTableAdapters.SC3240101DataAdapter
    '        dt = ta.GetStandardDeliveryTime(objStaffContext.DlrCD, objStaffContext.BrnCD)
    '    End Using

    '    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E.", System.Reflection.MethodBase.GetCurrentMethod.Name))
    '    Return dt
    'End Function
    ' 2019/10/31 NSK 鈴木【（FS）MaaSビジネス向けサービス予約の登録オペレーションの効率化に向けた試験研究】[No156]予定入庫日時、予定納車日時の自動セット END

#End Region

#Region "ストール情報の取得"

    ''' <summary>
    ''' 全ストールの情報を取得
    ''' </summary>
    ''' <param name="userContext">スタッフ情報</param>
    ''' <returns>全ストールの情報を格納したDataTable</returns>
    Public Function GetAllStall(ByVal userContext As StaffContext) As SC3240101DataSet.SC3240101AllStallDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S", System.Reflection.MethodBase.GetCurrentMethod.Name))

        Using ta As New SC3240101DataSetTableAdapters.SC3240101DataAdapter
            Dim dt As SC3240101DataSet.SC3240101AllStallDataTable

            '2016/01/12 NSK 皆川 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 START
            ''2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
            ''使用権限がChTの場合、ChTに紐づくストールのみを表示する
            'If userContext.OpeCD = iCROP.BizLogic.Operation.CHT Then
            '    Dim stallidList As New List(Of Decimal)
            '    Using clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic
            '        'アカウントより、対応するストールIDを取得する
            '        Dim stallidTalbe As TabletSMBCommonClassDataSet.TabletSmbCommonClassNumberValueDataTable = _
            '            clsTabletSMBCommonClass.GetStallidByChtAccount(userContext.Account)
            '        'ストールidがない場合、空白テーブルを戻す
            '        If stallidTalbe.Count = 0 Then
            '            Using retTable As New SC3240101DataSet.SC3240101AllStallDataTable
            '                Return retTable
            '            End Using
            '        End If
            '        'ストールIDリストを作成する
            '        For Each stallid As TabletSMBCommonClassDataSet.TabletSmbCommonClassNumberValueRow In stallidTalbe
            '            stallidList.Add(stallid.COL1)
            '        Next
            '        'ストールリスト対応するデータを取得する
            '        dt = ta.GetAllStall(userContext.DlrCD, userContext.BrnCD, stallidList)
            '    End Using
            'Else
            '    '2015/09/08 TMEJ 皆川 タブレットSMB ストールグループ表示対応 START
            '    Dim stallIdList As New List(Of Decimal)
            '    Using clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic
            '        'アカウントより、対応するストールIDを取得する
            '        Dim stallIdTalbe As TabletSMBCommonClassDataSet.TabletSmbCommonClassNumberValueDataTable = _
            '            clsTabletSMBCommonClass.GetStaffStall(userContext.Account)
            '        'ストールIDリストを作成する
            '        For Each stallId As TabletSMBCommonClassDataSet.TabletSmbCommonClassNumberValueRow In stallIdTalbe
            '            stallIdList.Add(stallId.COL1)
            '        Next
            '        '2015/09/08 TMEJ 皆川 タブレットSMB ストールグループ表示対応 END
            '        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
            '        '該店舗のストール情報を取得
            '        '2015/09/08 TMEJ 皆川 タブレットSMB ストールグループ表示対応 START
            '        'dt = ta.GetAllStall(userContext.DlrCD, userContext.BrnCD)
            '        dt = ta.GetAllStall(userContext.DlrCD, userContext.BrnCD, stallIdList)
            '    End Using
            '    '2015/09/08 TMEJ 皆川 タブレットSMB ストールグループ表示対応 END
            '    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
            'End If
            ''2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
            Dim stallIdList As New List(Of Decimal)
            Using clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic
                'アカウントより、対応するストールIDを取得する
                Dim stallIdTalbe As TabletSMBCommonClassDataSet.TabletSmbCommonClassNumberValueDataTable = _
                    clsTabletSMBCommonClass.GetStaffStall(userContext.Account)
                'ストールIDリストを作成する
                For Each stallId As TabletSMBCommonClassDataSet.TabletSmbCommonClassNumberValueRow In stallIdTalbe
                    stallIdList.Add(stallId.COL1)
                Next
                'ストールリストに対応するデータを取得する
                dt = ta.GetAllStall(userContext.DlrCD, userContext.BrnCD, stallIdList)
            End Using
            '2016/01/12 NSK 皆川 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 END
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return dt
        End Using

    End Function

#End Region

#Region "ストールスタッフ情報の取得"

    ''2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    ' ''' <summary>
    ' ''' 指定店舗の指定日の全ストールの情報と配属テクニシャン名を取得
    ' ''' </summary>
    ' ''' <param name="workDate">作業日付(yyyyMMdd)</param>
    ' ''' <returns></returns>
    'Public Function GetAllStallStaff(ByVal workDate As String) As SC3240101DataSet.SC3240101StallStaffDataTable
    'Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. workDate={1}" _
    '                          , System.Reflection.MethodBase.GetCurrentMethod.Name, workDate))
    ''' <summary>
    ''' 指定店舗の指定日の全ストールの情報と配属テクニシャン名を取得
    ''' </summary>
    ''' <param name="userContext">スタッフ情報</param>
    ''' <returns></returns>
    Public Function GetAllStallStaff(ByVal userContext As StaffContext) As SC3240101DataSet.SC3240101StallStaffDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. " _
                          , System.Reflection.MethodBase.GetCurrentMethod.Name))
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

        Using ta As New SC3240101DataSetTableAdapters.SC3240101DataAdapter
            Dim dt As SC3240101DataSet.SC3240101StallStaffDataTable
            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
            'Dim userContext As StaffContext = StaffContext.Current
            'dt = ta.GetAllStallStaff(userContext.DlrCD, userContext.BrnCD, workDate)

            '2014/07/02 TMEJ 張 BTS-283 「サービスマネージャをTCとして表示」対応 START
            'dt = ta.GetAllStallStaff(userContext.DlrCD, userContext.BrnCD)

            'Me.StfStallDispTypeもう一回初期化する
            Me.InitStaffStallDispType(userContext)
            '指定店舗のストールスタッフ情報を取得する
            dt = ta.GetAllStallStaff(userContext.DlrCD, userContext.BrnCD, Me.StfStallDispType)
            '2014/07/02 TMEJ 張 BTS-283 「サービスマネージャをTCとして表示」対応 END

            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return dt
        End Using

    End Function

    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    ''' <summary>
    ''' 指定店舗の全ストールの情報と配属テクニシャン名を取得
    ''' </summary>
    ''' <returns></returns>
    Public Function GetAllTechnicianByBrn(ByVal userContext As StaffContext) As SC3240101DataSet.SC3240101StallStaffDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. " _
                                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim dt As SC3240101DataSet.SC3240101StallStaffDataTable

        Using ta As New SC3240101DataSetTableAdapters.SC3240101DataAdapter

            '2014/07/02 TMEJ 張 BTS-283 「サービスマネージャをTCとして表示」対応 START
            'dt = ta.GetAllTechnicianByBrn(userContext.DlrCD, userContext.BrnCD)

            'StfStallDispTypeをもし設定してない場合、もう一回取得する
            Me.InitStaffStallDispType(userContext)
            '指定店舗の全ストールの情報と配属テクニシャン名を取得
            dt = ta.GetAllTechnicianByBrn(userContext.DlrCD, userContext.BrnCD, Me.StfStallDispType)
            '2014/07/02 TMEJ 張 BTS-283 「サービスマネージャをTCとして表示」対応 END

        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", System.Reflection.MethodBase.GetCurrentMethod.Name))
        Return dt

    End Function
    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

    '2014/07/02 TMEJ 張 BTS-283 「サービスマネージャをTCとして表示」対応 START
    ''' <summary>
    ''' スタッフストール表示区分変数を初期化
    ''' </summary>
    ''' <param name="userContext">スタッフ情報</param>
    ''' <remarks></remarks>
    Public Sub InitStaffStallDispType(ByVal userContext As StaffContext)

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. " _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'StfStallDispTypeが存在してない場合、初期化する
        If String.IsNullOrEmpty(Me.StfStallDispType) Then
            Using clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic
                Me.StfStallDispType = clsTabletSMBCommonClass.GetStaffStallDispType(userContext.DlrCD, userContext.BrnCD)
            End Using
        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E StfStallDispType={1}",
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  Me.StfStallDispType))

    End Sub
    '2014/07/02 TMEJ 張 BTS-283 「サービスマネージャをTCとして表示」対応 END

#End Region

#Region "ストールチップ情報の取得"

    ''' <summary>
    ''' ストール上のチップ一覧の取得
    ''' </summary>
    ''' <param name="dlrCode">販売店コード</param>
    ''' <param name="brnCode">店舗コード</param>
    ''' <param name="stallIdList">ストールIDリスト</param>
    ''' <param name="stallStartTime">ストール稼動開始日時</param>
    ''' <param name="stallEndTime">ストール稼動終了日時</param>
    ''' <param name="theTime">この日時後変更があったチップを取得</param>
    ''' <returns>ストールチップの情報テーブル</returns>
    ''' '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    Public Function GetAllStallChip(ByVal dlrCode As String, _
                                    ByVal brnCode As String, _
                                    ByVal stallIdList As List(Of Decimal), _
                                    ByVal stallStartTime As Date, _
                                    ByVal stallEndTime As Date, _
                                    Optional ByVal theTime As Date = Nothing) As TabletSMBCommonClassDataSet.TabletSmbCommonClassStallChipInfoDataTable

        'Public Function GetAllStallChip(ByVal dlrCode As String, _
        '                            ByVal brnCode As String, _
        '                            ByVal stallIdList As List(Of Long), _
        '                            ByVal stallStartTime As Date, _
        '                            ByVal stallEndTime As Date) As TabletSMBCommonClassDataSet.TabletSmbCommonClassStallChipInfoDataTable
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S.", System.Reflection.MethodBase.GetCurrentMethod.Name))
        Dim dtChipInfo As TabletSMBCommonClassDataSet.TabletSmbCommonClassStallChipInfoDataTable

        Using clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic
            '全てチップの情報を取得する
            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
            'dtChipInfo = clsTabletSMBCommonClass.GetAllStallChip(dlrCode, brnCode, stallStartTime, stallEndTime, stallIdList)
            dtChipInfo = clsTabletSMBCommonClass.GetAllStallChip(dlrCode, brnCode, stallStartTime, stallEndTime, stallIdList, theTime)
            '部品ステータス取得結果を設定する
            IC3802503ResultValue = clsTabletSMBCommonClass.IC3802503ResultValue
            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", System.Reflection.MethodBase.GetCurrentMethod.Name))
        Return dtChipInfo
    End Function

    ''' <summary>
    ''' ストール上のチップ一覧の遅れ見込み時間の取得
    ''' </summary>
    ''' <returns>ストールチップの情報テーブル</returns>
    ''' <remarks></remarks>
    Public Function GetPLanLaterTime(ByVal dlrCode As String, _
                                    ByVal brnCode As String, _
                                    ByVal dtNow As Date, _
                                    ByVal svcinIds As String) As TabletSMBCommonClassDataSet.TabletSmbCommonClassDeliDelayDateDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. svcinIds={1}", System.Reflection.MethodBase.GetCurrentMethod.Name, svcinIds))

        'string→List(Of Long)に変更する
        Dim svcinIdList As New List(Of Decimal)
        Dim split As String() = svcinIds.Split(CChar(","))
        For Each s As String In split
            If String.IsNullOrEmpty(s.Trim()) Then
                Continue For
            End If

            If Not IsNumeric(s) Then
                Continue For
            End If

            Dim svcinId As Decimal = CType(s, Decimal)
            '重複のサービス入庫IDをいれない
            If Not svcinIdList.Contains(svcinId) Then
                svcinIdList.Add(svcinId)
            End If
        Next

        Using clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic
            '遅れ見込み列のデータを取得する
            Dim dtDelay As TabletSMBCommonClassDataSet.TabletSmbCommonClassDeliDelayDateDataTable = clsTabletSMBCommonClass.GetDeliveryDelayDateList(svcinIdList, dlrCode, brnCode, dtNow)
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return dtDelay
        End Using
    End Function

    '2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応 START
    ' ''' <summary>
    ' ''' サービス入庫IDにより、チップの全情報を取得:サービス入庫IDにより、チップの全情報を取得する
    ' ''' </summary>
    ' ''' <param name="dtNow">今の時間</param>
    ' ''' <param name="objStaffContext">スタッフ情報</param>
    ' ''' <param name="svcinIdList">サービス入庫IDリスト</param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Function GetStallChipBySvcinId(ByVal dtNow As Date, _
    '                                        ByVal objStaffContext As StaffContext, _
    '                                        ByVal svcinIdList As List(Of Decimal)) As TabletSMBCommonClassDataSet.TabletSmbCommonClassStallChipInfoDataTable

    '    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S.", System.Reflection.MethodBase.GetCurrentMethod.Name))

    '    Dim dtChipInfo As TabletSMBCommonClassDataSet.TabletSmbCommonClassStallChipInfoDataTable
    '    Using clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic
    '        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    '        'dtChipInfo = clsTabletSMBCommonClass.GetStallChipBySvcinId(objStaffContext.DlrCD, objStaffContext.BrnCD, dtNow, svcinIdList)
    '        dtChipInfo = clsTabletSMBCommonClass.GetStallChipBySvcinId(objStaffContext.DlrCD, objStaffContext.BrnCD, dtNow, svcinIdList, True)
    '        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
    '    End Using

    '    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", System.Reflection.MethodBase.GetCurrentMethod.Name))
    '    Return dtChipInfo
    'End Function

    ''' <summary>
    ''' 各操作後、ストール上更新されたチップの情報を取得
    ''' </summary>
    ''' <param name="inDlrCode">販売店コード</param>
    ''' <param name="inBrnCode">店舗コード</param>
    ''' <param name="inShowDate">画面に表示されてる日時</param>
    ''' <param name="inLastRefreshTime">最新の更新日時</param>
    ''' <returns>最新のチップ情報</returns>
    ''' <remarks></remarks>
    Public Function GetStallChipAfterOperation(ByVal inDlrCode As String, _
                                               ByVal inBrnCode As String, _
                                               ByVal inShowDate As Date, _
                                               ByVal inLastRefreshTime As Date) As TabletSMBCommonClassDataSet.TabletSmbCommonClassStallChipInfoDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture _
                                , "{0}.Start. inDlrCode={1}, inBrnCode={2}, inShowDate={3}, inLastRefreshTime={4}" _
                                , Reflection.MethodBase.GetCurrentMethod.Name _
                                , inDlrCode _
                                , inBrnCode _
                                , inShowDate _
                                , inLastRefreshTime))


        Using clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic

            Dim dtChipInfo As TabletSMBCommonClassDataSet.TabletSmbCommonClassStallChipInfoDataTable = _
                clsTabletSMBCommonClass.GetStallChipAfterOperation(inDlrCode, _
                                                                   inBrnCode, _
                                                                   inShowDate, _
                                                                   inLastRefreshTime)

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}.End. Chip count is = {1}", _
                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                      dtChipInfo.Count))
            Return dtChipInfo

        End Using


    End Function

    '2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応 END


    ''' <summary>
    ''' サービス入庫IDにより、関連チップ情報を取得:サービス入庫IDに紐ずく、リレーションチップの情報を取得する
    ''' </summary>
    ''' <param name="svcInIdList">サービス入庫IDリスト</param>
    ''' <returns></returns>
    Public Function GetAllRelationChipInfo(ByVal svcInIdList As List(Of Decimal)) As TabletSMBCommonClassDataSet.TabletSmbCommonClassRelationChipInfoDataDataTable
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S.", System.Reflection.MethodBase.GetCurrentMethod.Name))
        Using ta As New TabletSMBCommonClassBusinessLogic
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return ta.GetAllRelationChipInfo(svcInIdList)
        End Using
    End Function

    ''' <summary>
    ''' StallChipInfoストール上チップ情報テーブルからサービス入庫IDリストを取得する
    ''' </summary>
    ''' <param name="dtChipInfo">StallChipInfoテーブル</param>
    ''' <returns>サービス入庫IDリスト</returns>
    ''' <remarks></remarks>
    Public Function GetSvcIdListFromStallChipTable(ByVal dtChipInfo As TabletSMBCommonClassDataSet.TabletSmbCommonClassStallChipInfoDataTable) As List(Of Decimal)

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S.", System.Reflection.MethodBase.GetCurrentMethod.Name))
        Dim svcIdList As New List(Of Decimal)

        'テーブルからサービス入庫IDリストを作成
        For Each drChipInfo As TabletSMBCommonClassDataSet.TabletSmbCommonClassStallChipInfoRow In dtChipInfo.Rows
            '重複のサービス入庫IDをいれない
            If Not svcIdList.Contains(drChipInfo.SVCIN_ID) Then
                svcIdList.Add(drChipInfo.SVCIN_ID)
            End If
        Next

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", System.Reflection.MethodBase.GetCurrentMethod.Name))
        Return svcIdList
    End Function

    ''' <summary>
    ''' ストール上の仮仮チップ情報を取得する
    ''' </summary>
    ''' <param name="dtNow">今の時間</param>
    ''' <param name="stallList">ストールリスト</param>
    ''' <param name="dtStallStartTime">営業開始時間</param>
    ''' <param name="dtStallEndTime">営業終了時間</param>
    ''' <returns>仮仮チップ情報</returns>
    ''' <remarks></remarks>
    Public Function GetAllStallKariKariChip(ByVal stallList As List(Of Decimal), _
                                            ByVal dtNow As Date, _
                                            ByVal dtStallStartTime As Date, _
                                            ByVal dtStallEndTime As Date) As TabletSMBCommonClassDataSet.TabletSmbCommonClassKariKariChipInfoDataTable
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. dtNow={1}", System.Reflection.MethodBase.GetCurrentMethod.Name, dtNow))
        Dim chipList As TabletSMBCommonClassDataSet.TabletSmbCommonClassKariKariChipInfoDataTable

        Using ta As New TabletSMBCommonClassBusinessLogic
            '仮仮チップ情報を取得する
            chipList = ta.GetAllStallKariKariChip(stallList, dtNow, dtStallStartTime, dtStallEndTime)
        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", System.Reflection.MethodBase.GetCurrentMethod.Name))
        Return chipList
    End Function

#End Region

#Region "ストール非稼働情報の取得"
    ''' <summary>
    ''' すべて非稼働情報を取得します
    ''' </summary>
    ''' <param name="stallIdList">ストールIDリスト</param>
    ''' <param name="idleStartDateTime">営業開始日時</param>
    ''' <param name="idleEndDateTime">営業終了日時</param>
    ''' <returns>ストール非稼働マスタ情報リスト</returns>
    ''' <remarks></remarks>
    Public Function GetAllIdleDateInfo(ByVal stallIdList As List(Of Decimal), idleStartDateTime As Date, _
                                 ByVal idleEndDateTime As Date) As DataTable

        Dim dtIdleDate As DataTable

        Using clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic
            '非稼働時間を取得
            dtIdleDate = clsTabletSMBCommonClass.GetAllIdleDateInfo(stallIdList, idleStartDateTime, idleEndDateTime)
        End Using

        Return dtIdleDate
    End Function
#End Region

    '2015/02/09 TMEJ 範 DMS連携版サービスタブレット SMB納車予定時刻通知機能開発
#Region "指定サービス入庫IDに紐づく最大のストール利用IDの取得"

    ''' <summary>
    ''' 指定サービス入庫IDに紐づく最大のストール利用IDを取得する
    ''' </summary>
    ''' <param name="inServiceInId">サービス入庫ID</param>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inBranchCode">店舗コード</param>
    ''' <returns>サービス入庫IDに紐づく最大のストール利用ID</returns>
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

        Using clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic

            '最大ストールIDを取得
            maxStallUseId = clsTabletSMBCommonClass.GetMaxStallUseIdGroupByServiceId(inServiceInId, _
                                                                                     inDealerCode, _
                                                                                     inBranchCode)

        End Using


        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} Start maxStallUseId={2}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , maxStallUseId))

        Return maxStallUseId

    End Function

#End Region
    '2015/02/09 TMEJ 範 DMS連携版サービスタブレット SMB納車予定時刻通知機能開発

#Region "中断メモテンプレートの取得"
    ''' <summary>
    ''' 当店の中断メモテンプレートを取得します(中断理由ウィンドウボックス用)
    ''' </summary>
    ''' <param name="dlrCode">ストールIDリスト</param>
    ''' <param name="brnCode">営業開始日時</param>
    ''' <returns>中断メモテンプレートリスト</returns>
    ''' <remarks></remarks>
    Public Function GetStopMemoTemplate(ByVal dlrCode As String, ByVal brnCode As String) As SC3240101DataSet.SC3240101StringValueDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S", System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim dtStopMemoTemplate As SC3240101DataSet.SC3240101StringValueDataTable

        Using ta As New SC3240101DataSetTableAdapters.SC3240101DataAdapter
            dtStopMemoTemplate = ta.GetStopMemoTemplate(dlrCode, brnCode)
        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", System.Reflection.MethodBase.GetCurrentMethod.Name))
        Return dtStopMemoTemplate

    End Function
#End Region

#Region "チップの移動、リサイズ処理"

    ''' <summary>
    ''' ストール上の一つのチップを移動、リサイズ(当日の各リレーションチップの残作業時間を再計算して返すバージョン)
    ''' </summary>
    ''' <param name="stallUseId">チップの予約ID</param>
    ''' <param name="stallId">変更後のストールのSTALLID</param>
    ''' <param name="dispStartDateTime">変更後の表示開始日時</param>
    ''' <param name="scheWorkTime">仕事時間</param>
    ''' <param name="restFlg">休憩フラグ</param>
    ''' <param name="stallStartTime">稼働開始日時</param>
    ''' <param name="stallEndTime">稼働終了日時</param>
    ''' <param name="objStaffContext">スタッフ情報</param>
    ''' <param name="rowLockVersion">更新回数</param>
    ''' <returns></returns>
    ''' <history>
    ''' 2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発
    ''' </history>
    <EnableCommit()>
    Public Function StallChipMoveResize(ByVal stallUseId As Decimal, _
                                        ByVal stallId As Decimal, _
                                        ByVal dispStartDateTime As Date, _
                                        ByVal scheWorkTime As Long, _
                                        ByVal restFlg As String, _
                                        ByVal stallStartTime As Date, _
                                        ByVal stallEndTime As Date, _
                                        ByVal dtNow As Date, _
                                        ByVal objStaffContext As StaffContext, _
                                        ByVal rowLockVersion As Long) As Long
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S", System.Reflection.MethodBase.GetCurrentMethod.Name))

        'ストールロックフラグ
        Dim isStallLock As Boolean = False

        '戻り値
        Dim result As Long

        Using clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic
            Try
                'ストールロック処理実施
                result = clsTabletSMBCommonClass.LockStall(stallId, dispStartDateTime, objStaffContext.Account, dtNow, PGMID_SMB)

                'ストールロック処理結果チェック
                If result <> TabletSMBCommonClassBusinessLogic.ActionResult.Success Then
                    '「0：成功」以外の場合
                    'ロールバックして処理結果を返却
                    Me.Rollback = True
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Error:LockStallError", System.Reflection.MethodBase.GetCurrentMethod.Name))
                    Return result

                End If

                'ストールロックフラグをTrueにする
                isStallLock = True

                'チップ移動、リサイズ処理実施
                result = clsTabletSMBCommonClass.MoveAndResize(stallUseId, _
                                                               stallId, _
                                                               dispStartDateTime, _
                                                               scheWorkTime, _
                                                               restFlg, _
                                                               stallStartTime, _
                                                               stallEndTime, _
                                                               dtNow, _
                                                               objStaffContext, _
                                                               PGMID_SMB, _
                                                               dtNow, _
                                                               rowLockVersion)

                'チップ移動、リサイズ処理結果チェック
                If result <> TabletSMBCommonClassBusinessLogic.ActionResult.Success AndAlso _
                   result <> TabletSMBCommonClassBusinessLogic.ActionResult.WarningOmitDmsError Then
                    '「0：成功」「-9000：DMS除外エラーの警告」以外の場合
                    'ロールバックしてエラーを返却

                    '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START
                    'If result <> TabletSMBCommonClassBusinessLogic.ActionResult.Success Then
                    '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

                    Me.Rollback = True
                    Logger.Warn(String.Format(CultureInfo.InvariantCulture, "{0}.E Error:MoveAndResize failed.", System.Reflection.MethodBase.GetCurrentMethod.Name))
                    Return result

                End If

            Catch ex As OracleExceptionEx When ex.Number = 1013
                Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.E Error:DBTimeOutError.", System.Reflection.MethodBase.GetCurrentMethod.Name))
                Me.Rollback = True
                Return TabletSMBCommonClassBusinessLogic.ActionResult.DBTimeOutError

            Finally
                'ストールロックフラグのチェック
                If isStallLock Then
                    'ストールロックしている場合
                    'ストールロック解除処理実施
                    clsTabletSMBCommonClass.LockStallReset(stallId, _
                                                           dispStartDateTime, _
                                                           objStaffContext.Account, _
                                                           dtNow, _
                                                           PGMID_SMB)

                End If

            End Try

        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", System.Reflection.MethodBase.GetCurrentMethod.Name))

        '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        'Return TabletSMBCommonClassBusinessLogic.ActionResult.Success

        Return result

        '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

    End Function

    '2015/02/09 TMEJ 範 DMS連携版サービスタブレット SMB納車予定時刻通知機能開発 START
    ''' <summary>
    ''' 納車遅れとなる配置を行った場合、通知の処理
    ''' </summary>
    ''' <param name="inStallUseId">ストール利用ID</param>
    ''' <param name="inNow">現在日時</param>
    ''' <param name="inUserInfo">ログイン情報</param>
    ''' <remarks></remarks>
    Public Sub SendNoticeWhenSetChipToBeLated(ByVal inStallUseId As Decimal, _
                                              ByVal inNow As Date, _
                                              ByVal inUserInfo As StaffContext)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} Start inStallUseId={2}, inNow={3}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , inStallUseId _
                  , inNow))

        Using clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic

            'チップエンティティを取得
            Dim dtChipEntity As TabletSmbCommonClassChipEntityDataTable = _
                clsTabletSMBCommonClass.GetChipEntity(inStallUseId)

            If 0 < dtChipEntity.Count Then
                '取得できた場合(0 < 取得行数)

                '通知を出す処理
                clsTabletSMBCommonClass.SendNoticeWhenSetChipToBeLated(inUserInfo, _
                                                                       inNow, _
                                                                       dtChipEntity(0))

            End If

        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} End" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    '2015/02/09 TMEJ 範 DMS連携版サービスタブレット SMB納車予定時刻通知機能開発 END

#End Region

#Region "開始処理"

    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    ' ''' <summary>
    ' ''' チップ作業の開始処理を行う
    ' ''' </summary>
    ' ''' <param name="stallUseId">ストール利用ID</param>
    ' ''' <param name="svcInId">サービス入庫ID</param>
    ' ''' <param name="stallId">ストールID</param>
    ' ''' <param name="rsltStartDateTime">実績開始日時</param>
    ' ''' <param name="restFlg">休憩取得フラグ</param>
    ' ''' <param name="dtNow">更新日時</param>
    ' ''' <param name="stallStartTime">営業開始時間</param>
    ' ''' <param name="stallEndTime">営業終了時間</param>
    ' ''' <param name="objStaffContext">スタッフ情報</param>
    ' ''' <param name="rowLockVersion">更新回数</param>
    ' ''' <returns>正常終了：0、異常終了：エラーコード</returns>
    ' ''' <remarks></remarks>
    '<EnableCommit()>
    'Public Function StallChipStart(ByVal svcInId As Decimal, _
    '                               ByVal stallUseId As Decimal, _
    '                               ByVal stallId As Decimal, _
    '                               ByVal rsltStartDateTime As Date, _
    '                               ByVal restFlg As String, _
    '                               ByVal dtNow As Date, _
    '                               ByVal stallStartTime As Date, _
    '                               ByVal stallEndTime As Date, _
    '                               ByVal objStaffContext As StaffContext, _
    '                               ByVal rowLockVersion As Long) As Long
    '    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S.", System.Reflection.MethodBase.GetCurrentMethod.Name))

    '    Using clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic

    '        Dim isStallLock As Boolean = False
    '        Dim result As Long
    '        Try
    '            'ストールロック
    '            result = clsTabletSMBCommonClass.LockStall(stallId, rsltStartDateTime, objStaffContext.Account, dtNow, PGMID_SMB)
    '            If result <> TabletSMBCommonClassBusinessLogic.ActionResult.Success Then
    '                Me.Rollback = True
    '                Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.E Error:LockStallError", System.Reflection.MethodBase.GetCurrentMethod.Name))
    '                Return result
    '            End If
    '            isStallLock = True

    '            'サービス入庫をロックして、チェックする
    '            result = clsTabletSMBCommonClass.LockServiceInTable(svcInId, rowLockVersion, objStaffContext.Account, dtNow, PGMID_SMB)
    '            If result <> TabletSMBCommonClassBusinessLogic.ActionResult.Success Then
    '                Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.E Error:LockServiceInTableError", System.Reflection.MethodBase.GetCurrentMethod.Name))
    '                Me.Rollback = True
    '                Return result
    '            End If

    '            '開始
    '            result = clsTabletSMBCommonClass.Start(stallUseId, svcInId, stallId, rsltStartDateTime, restFlg, objStaffContext, stallStartTime, stallEndTime, dtNow, PGMID_SMB)
    '            'エラーコードを戻す
    '            If result <> TabletSMBCommonClassBusinessLogic.ActionResult.Success Then
    '                Me.Rollback = True
    '                Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.E Error:Failed to Start.", System.Reflection.MethodBase.GetCurrentMethod.Name))
    '                Return result
    '            End If
    '        Catch ex As OracleExceptionEx When ex.Number = 1013
    '            Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.E Error:DBTimeOutError.", System.Reflection.MethodBase.GetCurrentMethod.Name))
    '            Me.Rollback = True
    '            Return TabletSMBCommonClassBusinessLogic.ActionResult.DBTimeOutError
    '        Finally
    '            If isStallLock Then
    '                'ストールロック解除
    '                clsTabletSMBCommonClass.LockStallReset(stallId, rsltStartDateTime, objStaffContext.Account, dtNow, PGMID_SMB)
    '            End If
    '        End Try
    '    End Using

    '    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", System.Reflection.MethodBase.GetCurrentMethod.Name))
    '    Return TabletSMBCommonClassBusinessLogic.ActionResult.Success

    'End Function

    '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
    ' ''' <summary>
    ' ''' チップの作業開始処理
    ' ''' </summary>
    ' ''' <param name="stallUseId">ストール利用ID</param>
    ' ''' <param name="rsltStartDateTime">実績開始日時</param>
    ' ''' <param name="restFlg">休憩取得フラグ</param>
    ' ''' <param name="dtNow">更新日時</param>
    ' ''' <param name="rowLockVersion">行ロックバージョン</param>
    ' ''' <returns>実行結果</returns>
    ' ''' <remarks></remarks>
    '<EnableCommit()>
    'Public Function StallChipStart(ByVal stallUseId As Decimal, _
    '                               ByVal rsltStartDateTime As Date, _
    '                               ByVal restFlg As String, _
    '                               ByVal dtNow As Date, _
    '                               ByVal rowLockVersion As Long) As Long

    ''' <summary>
    ''' チップの作業開始処理
    ''' </summary>
    ''' <param name="stallUseId">ストール利用ID</param>
    ''' <param name="rsltStartDateTime">実績開始日時</param>
    ''' <param name="restFlg">休憩取得フラグ</param>
    ''' <param name="dtNow">更新日時</param>
    ''' <param name="rowLockVersion">行ロックバージョン</param>
    ''' <param name="restartStopJobFlg">中断Job再開フラグ</param>
    ''' <returns>実行結果</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発
    ''' </history>
    <EnableCommit()>
    Public Function StallChipStart(ByVal stallUseId As Decimal, _
                                   ByVal rsltStartDateTime As Date, _
                                   ByVal restFlg As String, _
                                   ByVal dtNow As Date, _
                                   ByVal rowLockVersion As Long, _
                                   Optional ByVal restartStopJobFlg As Boolean = True) As Long
        '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

        '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        '戻り値
        Dim result As Long = TabletSMBCommonClassBusinessLogic.ActionResult.Success

        '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S.", System.Reflection.MethodBase.GetCurrentMethod.Name))

        Using clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic

            '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

            ''開始
            ''2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
            ''Dim result As Long = clsTabletSMBCommonClass.Start(stallUseId, _
            ''                                       rsltStartDateTime, _
            ''                                       restFlg, _
            ''                                       dtNow, _
            ''                                       rowLockVersion, _
            ''                                       PGMID_SMB, _
            ''                                       restartStopJobFlg, _
            ''                                       TabletSMBCommonClassBusinessLogic.CallerTypeSMBAllJobAction)

            'Dim result As Long = clsTabletSMBCommonClass.Start(stallUseId, _
            '                                                   rsltStartDateTime, _
            '                                                   restFlg, _
            '                                                   dtNow, _
            '                                                   rowLockVersion, _
            '                                                   PGMID_SMB, _
            '                                                   restartStopJobFlg, _
            '                                                   TabletSMBCommonClassBusinessLogic.CallerTypeSmbAllJobAction)

            ''2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

            '開始処理実施
            result = clsTabletSMBCommonClass.Start(stallUseId, _
                                                   rsltStartDateTime, _
                                                   restFlg, _
                                                   dtNow, _
                                                   rowLockVersion, _
                                                   PGMID_SMB, _
                                                   restartStopJobFlg, _
                                                   TabletSMBCommonClassBusinessLogic.CallerTypeSmbAllJobAction)

            '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

            '開始処理結果チェック
            If result <> TabletSMBCommonClassBusinessLogic.ActionResult.Success AndAlso _
               result <> TabletSMBCommonClassBusinessLogic.ActionResult.WarningOmitDmsError Then
                '「0：成功」「-9000：DMS除外エラーの警告」以外の場合
                'ロールバックしてエラーを返却

                '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START
                'If result <> TabletSMBCommonClassBusinessLogic.ActionResult.Success Then
                '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

                Me.Rollback = True
                Logger.Warn(String.Format(CultureInfo.InvariantCulture, "{0}.E Error:Failed to Start.", System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return result

            End If

        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", System.Reflection.MethodBase.GetCurrentMethod.Name))

        '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        'Return TabletSMBCommonClassBusinessLogic.ActionResult.Success

        Return result

        '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

    End Function

    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

    '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START

    ''' <summary>
    ''' 中断Job存在判定
    ''' </summary>
    ''' <param name="inJobDtlId">作業内容ID</param>
    ''' <returns>true：中断Jobが存在する、false：中断Jobが存在しない</returns>
    ''' <remarks></remarks>
    Public Function HasStopJob(ByVal inJobDtlId As Decimal) As Boolean

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.Start. inJobDtlId={1}", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  inJobDtlId))

        '返却値
        Dim retHasStopJob As Boolean

        Using clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic

            '中断Job存在判定(共通関数)
            retHasStopJob = clsTabletSMBCommonClass.HasStopJob(inJobDtlId)

        End Using

        '結果返却
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.End return={1}", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  retHasStopJob))
        Return retHasStopJob

    End Function

    '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END
#End Region

#Region "完了処理"

    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    ' ''' <summary>
    ' ''' ストール利用を「作業完了」へ更新します
    ' ''' </summary>
    ' ''' <param name="stallUseId">ストール利用ID</param>
    ' ''' <param name="rsltStartDateTime">実績開始日時</param>
    ' ''' <param name="rsltEndDateTime">実績終了日時</param>
    ' ''' <param name="restFlg">休憩フラグ</param>
    ' ''' <param name="stallStartTime">営業開始時間</param>
    ' ''' <param name="stallEndTime">営業終了時間</param>
    ' ''' <param name="dtNow">更新日時</param>
    ' ''' <param name="staffInfo">スタッフ情報</param>
    ' ''' <param name="rowLockVersion">更新回数</param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    '<EnableCommit()>
    'Public Function StallChipFinish(ByVal svcInId As Decimal, _
    '                                ByVal stallUseId As Decimal, _
    '                                ByVal stallId As Decimal, _
    '                                ByVal rsltStartDateTime As Date, _
    '                                ByVal rsltEndDateTime As Date, _
    '                                ByVal restFlg As String, _
    '                                ByVal stallStartTime As Date, _
    '                                ByVal stallEndTime As Date, _
    '                                ByVal dtNow As Date, _
    '                                ByVal staffInfo As StaffContext, _
    '                                ByVal rowLockVersion As Long) As Long

    '    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. ", System.Reflection.MethodBase.GetCurrentMethod.Name))

    '    Using clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic
    '        Try
    '            '開始時間から終了時間までの範囲に重複休憩エリアがあるか
    '            Dim workTime As Long = DateDiff("n", rsltStartDateTime, rsltEndDateTime)
    '            Dim hasRestTimeInServiceTime As Boolean = clsTabletSMBCommonClass.HasRestTimeInServiceTime(stallStartTime, stallEndTime, stallId, rsltStartDateTime, workTime, True)
    '            '休憩または使用不可エリアと重複場合、
    '            If hasRestTimeInServiceTime Then
    '                '画面に重複で表示してない
    '                If IsNothing(restFlg) Then
    '                    Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.E OverlapError", System.Reflection.MethodBase.GetCurrentMethod.Name))
    '                    Return TabletSMBCommonClassBusinessLogic.ActionResult.OverlapError
    '                End If
    '            End If

    '            'サービス入庫をロックして、チェックする
    '            Dim result As Long = clsTabletSMBCommonClass.LockServiceInTable(svcInId, rowLockVersion, staffInfo.Account, dtNow, PGMID_SMB)
    '            If result <> TabletSMBCommonClassBusinessLogic.ActionResult.Success Then
    '                Me.Rollback = True
    '                Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.E LockServiceInTableError", System.Reflection.MethodBase.GetCurrentMethod.Name))
    '                Return result
    '            End If

    '            '終了
    '            result = clsTabletSMBCommonClass.Finish(stallUseId, rsltEndDateTime, restFlg, staffInfo, stallStartTime, stallEndTime, dtNow, PGMID_SMB)
    '            'エラーコードを戻す
    '            If result <> TabletSMBCommonClassBusinessLogic.ActionResult.Success Then
    '                Me.Rollback = True
    '                Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.E Failed to Finish.", System.Reflection.MethodBase.GetCurrentMethod.Name))
    '                Return result
    '            End If

    '        Catch ex As OracleExceptionEx When ex.Number = 1013
    '            'DBTimeout
    '            Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.E DBTimeOutError.", System.Reflection.MethodBase.GetCurrentMethod.Name))
    '            Me.Rollback = True
    '            Return TabletSMBCommonClassBusinessLogic.ActionResult.DBTimeOutError
    '        End Try
    '    End Using
    '    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", System.Reflection.MethodBase.GetCurrentMethod.Name))
    '    Return TabletSMBCommonClassBusinessLogic.ActionResult.Success

    'End Function

    ''' <summary>
    ''' 作業完了操作
    ''' </summary>
    ''' <param name="stallUseId">ストール利用ID</param>
    ''' <param name="rsltEndDateTime">実績終了日時</param>
    ''' <param name="restFlg">休憩フラグ</param>
    ''' <param name="dtNow">更新日時</param>
    ''' <param name="rowLockVersion">更新回数</param>
    ''' <returns>実行結果</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発
    ''' </history>
    <EnableCommit()>
    Public Function StallChipFinish(ByVal stallUseId As Decimal, _
                                    ByVal rsltEndDateTime As Date, _
                                    ByVal restFlg As String, _
                                    ByVal dtNow As Date, _
                                    ByVal rowLockVersion As Long) As Long

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. ", System.Reflection.MethodBase.GetCurrentMethod.Name))

        '戻り値
        Dim result As Long

        Using clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic
            Try
                '作業完了処理実施
                result = clsTabletSMBCommonClass.Finish(stallUseId, _
                                                        rsltEndDateTime, _
                                                        restFlg, _
                                                        dtNow, _
                                                        rowLockVersion, _
                                                        PGMID_SMB)

                '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START

                '中断操作でPushするかフラグ
                Me.NeedPushAfterStopJob = clsTabletSMBCommonClass.NeedPushAfterStopSingleJob

                '終了操作でPushするかフラグ
                Me.NeedPushAfterFinishJob = clsTabletSMBCommonClass.NeedPushAfterFinishSingleJob

                '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

                '作業完了処理結果チェック
                If result <> TabletSMBCommonClassBusinessLogic.ActionResult.Success AndAlso _
                   result <> TabletSMBCommonClassBusinessLogic.ActionResult.WarningOmitDmsError Then
                    '「0：成功」「-9000：DMS除外エラーの警告」以外の場合
                    'ロールバックする

                    '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START
                    'If result <> TabletSMBCommonClassBusinessLogic.ActionResult.Success Then
                    '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

                    Me.Rollback = True

                End If

            Catch ex As OracleExceptionEx When ex.Number = 1013
                'DBTimeout
                Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.E DBTimeOutError.", System.Reflection.MethodBase.GetCurrentMethod.Name))
                Me.Rollback = True
                Return TabletSMBCommonClassBusinessLogic.ActionResult.DBTimeOutError

            End Try

        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.E result={1}", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  result))
        Return result

    End Function

    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

#End Region

#Region "日跨ぎ終了処理"

    ''' <summary>
    ''' 日跨ぎ終了処理
    ''' </summary>
    ''' <param name="svcinId"></param>
    ''' <param name="stallUseId">ストール利用ID</param>
    ''' <param name="stallId">ストールID</param>
    ''' <param name="rsltStartDateTime">実績開始日時</param>
    ''' <param name="rsltEndDateTime">実績終了日時</param>
    ''' <param name="restFlg">休憩フラグ</param>
    ''' <param name="dtNow">更新日時</param>
    ''' <param name="objStaffContext">スタッフ情報</param>
    ''' <param name="stallStartTime">営業開始時間</param>
    ''' <param name="stallEndTime">営業終了時間</param>
    ''' <param name="rowLockVersion">行ロックバージョン</param>
    ''' <returns>実行結果</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発
    ''' </history>
    <EnableCommit()>
    Public Function StallChipMidFinish(ByVal svcinId As Decimal, _
                                       ByVal stallUseId As Decimal, _
                                       ByVal stallId As Decimal, _
                                       ByVal rsltStartDateTime As Date, _
                                       ByVal rsltEndDateTime As Date, _
                                       ByVal restFlg As String, _
                                       ByVal dtNow As Date, _
                                       ByVal objStaffContext As StaffContext, _
                                       ByVal stallStartTime As Date, _
                                       ByVal stallEndTime As Date, _
                                       ByVal rowLockVersion As Long) As Long

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S.", System.Reflection.MethodBase.GetCurrentMethod.Name))

        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START

        '戻り値
        Dim result As Long

        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

        Using clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic
            Try

                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                ''開始時間から終了時間までの範囲に重複休憩エリアがあるか
                'Dim workTime As Long = DateDiff("n", rsltStartDateTime, rsltEndDateTime)
                'Dim hasRestTimeInServiceTime As Boolean = clsTabletSMBCommonClass.HasRestTimeInServiceTime(stallStartTime, stallEndTime, stallId, rsltStartDateTime, workTime, True)
                ''休憩または使用不可エリアと重複場合、
                'If hasRestTimeInServiceTime Then
                '    '画面に重複で表示してない
                '    If IsNothing(restFlg) Then
                '        Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.E Error:OverlapError", System.Reflection.MethodBase.GetCurrentMethod.Name))
                '        Return TabletSMBCommonClassBusinessLogic.ActionResult.OverlapError
                '    End If
                'End If

                ''サービス入庫をロックして、チェックする
                'Dim result As Long = clsTabletSMBCommonClass.LockServiceInTable(svcinId, rowLockVersion, objStaffContext.Account, dtNow, PGMID_SMB)
                'If result <> TabletSMBCommonClassBusinessLogic.ActionResult.Success Then
                '    Me.Rollback = True
                '    Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.E Error:LockServiceInTableError", System.Reflection.MethodBase.GetCurrentMethod.Name))
                '    Return result
                'End If

                '日跨ぎ終了
                'result = clsTabletSMBCommonClass.MidFinish(stallUseId, rsltEndDateTime, restFlg, objStaffContext, stallStartTime, stallEndTime, dtNow, PGMID_SMB)
                result = clsTabletSMBCommonClass.MidFinish(svcinId, _
                                                           stallUseId, _
                                                           stallId, _
                                                           rsltStartDateTime, _
                                                           rsltEndDateTime, _
                                                           restFlg, _
                                                           objStaffContext, _
                                                           stallStartTime, _
                                                           stallEndTime, _
                                                           dtNow, _
                                                           PGMID_SMB, _
                                                           rowLockVersion)

                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

                '日跨ぎ終了処理結果チェック
                If result <> TabletSMBCommonClassBusinessLogic.ActionResult.Success AndAlso _
                   result <> TabletSMBCommonClassBusinessLogic.ActionResult.WarningOmitDmsError Then
                    '「0：成功」「-9000：DMS除外エラーの警告」以外の場合
                    'ロールバックしてエラーを返却

                    '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START
                    'If result <> TabletSMBCommonClassBusinessLogic.ActionResult.Success Then
                    '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

                    Me.Rollback = True
                    Logger.Warn(String.Format(CultureInfo.InvariantCulture, "{0}.E Error:Fail to MidFinish.", System.Reflection.MethodBase.GetCurrentMethod.Name))
                    Return result

                End If

            Catch ex As OracleExceptionEx When ex.Number = 1013
                Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.E Error:DBTimeOutError.", System.Reflection.MethodBase.GetCurrentMethod.Name))
                Me.Rollback = True
                Return TabletSMBCommonClassBusinessLogic.ActionResult.DBTimeOutError

            End Try

        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", System.Reflection.MethodBase.GetCurrentMethod.Name))

        '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        'Return TabletSMBCommonClassBusinessLogic.ActionResult.Success

        Return result

        '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

    End Function

#End Region

#Region "本予約処理"

    ''' <summary>
    ''' 本予約
    ''' </summary>
    ''' <param name="svcinId">サービス入庫ID</param>
    ''' <param name="stallUseId">ストール利用ID</param>
    ''' <param name="dtNow">更新日時</param>
    ''' <param name="objStaffContext">スタッフ情報</param>
    ''' <param name="rowLockVersion">更新回数</param>
    ''' <returns>正常終了：0、異常終了：エラーコード</returns>
    ''' <history>
    ''' 2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発
    ''' </history>
    <EnableCommit()>
    Public Function StallChipConfirmRez(ByVal svcinId As Decimal, _
                                        ByVal stallUseId As Decimal, _
                                        ByVal dtNow As Date, _
                                        ByVal objStaffContext As StaffContext, _
                                        ByVal rowLockVersion As Long) As Long

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S.", System.Reflection.MethodBase.GetCurrentMethod.Name))

        '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        '戻り値
        Dim result As Long = TabletSMBCommonClassBusinessLogic.ActionResult.Success

        '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        Using clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic
            Try
                '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

                ''サービス入庫をロック処理実施
                'Dim result As Long = clsTabletSMBCommonClass.LockServiceInTable(svcinId, rowLockVersion, objStaffContext.Account, dtNow, PGMID_SMB)

                'サービス入庫をロック処理実施
                result = clsTabletSMBCommonClass.LockServiceInTable(svcinId, _
                                                                    rowLockVersion, _
                                                                    objStaffContext.Account, _
                                                                    dtNow, _
                                                                    PGMID_SMB)

                '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

                'サービス入庫をロック処理結果チェック
                If result <> TabletSMBCommonClassBusinessLogic.ActionResult.Success Then
                    '「0：成功」以外の場合
                    'ロールバックしてエラーを返却
                    Me.Rollback = True
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Error:LockServiceInTableError", System.Reflection.MethodBase.GetCurrentMethod.Name))
                    Return result

                End If

                '本予約処理実施
                result = clsTabletSMBCommonClass.Reserve(svcinId, _
                                                         stallUseId, _
                                                         dtNow, _
                                                         objStaffContext, _
                                                         PGMID_SMB)

                '本予約処理結果チェック
                If result <> TabletSMBCommonClassBusinessLogic.ActionResult.Success AndAlso _
                   result <> TabletSMBCommonClassBusinessLogic.ActionResult.WarningOmitDmsError Then
                    '「0：成功」「-9000：DMS除外エラーの警告」以外の場合
                    'ロールバックしてエラーを返却

                    '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START
                    'If result <> TabletSMBCommonClassBusinessLogic.ActionResult.Success Then
                    '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

                    Me.Rollback = True
                    Logger.Warn(String.Format(CultureInfo.InvariantCulture, "{0}.E Error:Failed to Reserve.", System.Reflection.MethodBase.GetCurrentMethod.Name))
                    Return result

                End If

            Catch ex As OracleExceptionEx When ex.Number = 1013
                Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.E Error:DBTimeOutError.", System.Reflection.MethodBase.GetCurrentMethod.Name))
                Me.Rollback = True
                Return TabletSMBCommonClassBusinessLogic.ActionResult.DBTimeOutError

            End Try

        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", System.Reflection.MethodBase.GetCurrentMethod.Name))

        '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        'Return TabletSMBCommonClassBusinessLogic.ActionResult.Success

        Return result

        '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

    End Function

#End Region

#Region "仮予約処理"

    ''' <summary>
    '''   仮予約処理
    ''' </summary>
    ''' <param name="svcinId">サービス入庫ID</param>
    ''' <param name="stallUseId">ストール利用ID</param>
    ''' <param name="dtNow">更新日時</param>
    ''' <param name="objStaffContext">スタッフ情報</param>
    ''' <param name="rowLockVersion">更新回数</param>
    ''' <returns>正常終了：0、異常終了：エラーコード</returns>
    ''' <history>
    ''' 2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発
    ''' </history>
    <EnableCommit()>
    Public Function StallChipCancelConfirmRez(ByVal svcinId As Decimal, _
                                        ByVal stallUseId As Decimal, _
                                        ByVal dtNow As Date, _
                                        ByVal objStaffContext As StaffContext, _
                                        ByVal rowLockVersion As Long) As Long

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S.", System.Reflection.MethodBase.GetCurrentMethod.Name))

        '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        '戻り値
        Dim result As Long = TabletSMBCommonClassBusinessLogic.ActionResult.Success

        '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        Using clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic
            Try
                '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

                ''サービス入庫をロックして、チェックする
                'Dim result As Long = clsTabletSMBCommonClass.LockServiceInTable(svcinId, rowLockVersion, objStaffContext.Account, dtNow, PGMID_SMB)

                'サービス入庫をロック処理実施
                result = clsTabletSMBCommonClass.LockServiceInTable(svcinId, _
                                                                    rowLockVersion, _
                                                                    objStaffContext.Account, _
                                                                    dtNow, _
                                                                    PGMID_SMB)

                '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

                'サービス入庫をロック処理結果チェック
                If result <> TabletSMBCommonClassBusinessLogic.ActionResult.Success Then
                    '「0：成功」以外の場合
                    'ロールバックしてエラーを返却
                    Me.Rollback = True
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Error:LockServiceInTableError", System.Reflection.MethodBase.GetCurrentMethod.Name))
                    Return result

                End If

                '仮予約処理実施
                result = clsTabletSMBCommonClass.TentativeReserve(svcinId, _
                                                                  stallUseId, _
                                                                  dtNow, _
                                                                  objStaffContext, _
                                                                  PGMID_SMB)

                '仮予約処理結果チェック
                If result <> TabletSMBCommonClassBusinessLogic.ActionResult.Success AndAlso _
                   result <> TabletSMBCommonClassBusinessLogic.ActionResult.WarningOmitDmsError Then
                    '「0：成功」「-9000：DMS除外エラーの警告」以外の場合
                    'ロールバックしてエラーを返却

                    '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START
                    'If result <> TabletSMBCommonClassBusinessLogic.ActionResult.Success Then
                    '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

                    Me.Rollback = True
                    Return result

                End If

            Catch ex As OracleExceptionEx When ex.Number = 1013
                Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.E Error:DBTimeOutError.", System.Reflection.MethodBase.GetCurrentMethod.Name))
                Me.Rollback = True
                Return TabletSMBCommonClassBusinessLogic.ActionResult.DBTimeOutError

            End Try

        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", System.Reflection.MethodBase.GetCurrentMethod.Name))

        '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        'Return TabletSMBCommonClassBusinessLogic.ActionResult.Success

        Return result

        '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

    End Function

#End Region

#Region "入庫処理"

    ''' <summary>
    '''   入庫
    ''' </summary>
    ''' <param name="svcinId">サービス入庫ID</param>
    ''' <param name="stallUseId">ストール利用ID</param>
    ''' <param name="rsltServiceinDateTime">実績入庫日時</param>
    ''' <param name="dtNow">更新日時</param>
    ''' <param name="staffCode">スタッフコード</param>
    ''' <param name="rowLockVersion">更新回数</param>
    ''' <returns>正常終了：0、異常終了：エラーコード</returns>
    ''' <history>
    ''' 2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発
    ''' </history>
    <EnableCommit()>
    Public Function StallChipCarIn(ByVal svcinId As Decimal, _
                                ByVal stallUseId As Decimal, _
                                ByVal rsltServiceinDateTime As Date, _
                                ByVal dtNow As Date, _
                                ByVal staffCode As String, _
                                ByVal rowLockVersion As Long) As Long

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S.", System.Reflection.MethodBase.GetCurrentMethod.Name))

        '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        '戻り値
        Dim result As Long = TabletSMBCommonClassBusinessLogic.ActionResult.Success

        '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        Using clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic

            Try
                '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

                '' サービス入庫をロックして、チェックする
                'Dim result As Long = clsTabletSMBCommonClass.LockServiceInTable(svcinId, rowLockVersion, staffCode, dtNow, PGMID_SMB)

                'サービス入庫をロック処理実施
                result = clsTabletSMBCommonClass.LockServiceInTable(svcinId, _
                                                                    rowLockVersion, _
                                                                    staffCode, _
                                                                    dtNow, _
                                                                    PGMID_SMB)

                '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

                'サービス入庫をロック処理結果チェック
                If result <> TabletSMBCommonClassBusinessLogic.ActionResult.Success Then
                    '「0：成功」以外の場合
                    'ロールバックしてエラーを返却
                    Me.Rollback = True
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Error:LockServiceInTableError", System.Reflection.MethodBase.GetCurrentMethod.Name))
                    Return result

                End If

                '入庫処理実施
                result = clsTabletSMBCommonClass.CarIn(svcinId, _
                                                       stallUseId, _
                                                       rsltServiceinDateTime, _
                                                       dtNow, _
                                                       staffCode, _
                                                       PGMID_SMB)

                '入庫処理結果チェック
                If result <> TabletSMBCommonClassBusinessLogic.ActionResult.Success AndAlso _
                   result <> TabletSMBCommonClassBusinessLogic.ActionResult.WarningOmitDmsError Then
                    '「0：成功」「-9000：DMS除外エラーの警告」以外の場合
                    'ロールバックしてエラーを返却

                    '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START
                    'If result <> TabletSMBCommonClassBusinessLogic.ActionResult.Success Then
                    '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

                    Me.Rollback = True
                    Logger.Warn(String.Format(CultureInfo.InvariantCulture, "{0}.E Error:Failed to CarIn.", System.Reflection.MethodBase.GetCurrentMethod.Name))
                    Return result

                End If

            Catch ex As OracleExceptionEx When ex.Number = 1013
                Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.E Error:DBTimeOutError.", System.Reflection.MethodBase.GetCurrentMethod.Name))
                Me.Rollback = True
                Return TabletSMBCommonClassBusinessLogic.ActionResult.DBTimeOutError

            End Try

        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", System.Reflection.MethodBase.GetCurrentMethod.Name))

        '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        'Return TabletSMBCommonClassBusinessLogic.ActionResult.Success

        Return result

        '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

    End Function

#End Region

#Region "入庫取消処理"

    ''' <summary>
    '''   入庫取消
    ''' </summary>
    ''' <param name="svcinId">サービス入庫ID</param>
    ''' <param name="stallUseId">ストール利用ID</param>
    ''' <param name="dtNow">更新日時</param>
    ''' <param name="staffCode">スタッフコード</param>
    ''' <param name="rowLockVersion">更新回数</param>
    ''' <returns>正常終了：0、異常終了：エラーコード</returns>
    ''' <history>
    ''' 2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発
    ''' </history>
    <EnableCommit()>
    Public Function StallChipCancelCarIn(ByVal svcinId As Decimal, _
                                ByVal stallUseId As Decimal, _
                                ByVal dtNow As Date, _
                                ByVal staffCode As String, _
                                ByVal rowLockVersion As Long) As Long

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S.", System.Reflection.MethodBase.GetCurrentMethod.Name))

        '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        '戻り値
        Dim result As Long = TabletSMBCommonClassBusinessLogic.ActionResult.Success

        '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        Using clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic

            Try
                '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

                '' サービス入庫をロックして、チェックする
                'Dim result As Long = clsTabletSMBCommonClass.LockServiceInTable(svcinId, rowLockVersion, staffCode, dtNow, PGMID_SMB)

                'サービス入庫をロック処理実施
                result = clsTabletSMBCommonClass.LockServiceInTable(svcinId, _
                                                                    rowLockVersion, _
                                                                    staffCode, _
                                                                    dtNow, _
                                                                    PGMID_SMB)

                '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

                'サービス入庫をロック処理結果チェック
                If result <> TabletSMBCommonClassBusinessLogic.ActionResult.Success Then
                    '「0：成功」以外の場合
                    'ロールバックしてエラーを返却
                    Me.Rollback = True
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Error:LockServiceInTableError", System.Reflection.MethodBase.GetCurrentMethod.Name))
                    Return result

                End If

                '入庫取消処理実施
                result = clsTabletSMBCommonClass.CancelCarIn(svcinId, _
                                                             stallUseId, _
                                                             dtNow, _
                                                             staffCode, _
                                                             PGMID_SMB)

                '入庫取消処理結果チェック
                If result <> TabletSMBCommonClassBusinessLogic.ActionResult.Success AndAlso _
                   result <> TabletSMBCommonClassBusinessLogic.ActionResult.WarningOmitDmsError Then
                    '「0：成功」「-9000：DMS除外エラーの警告」以外の場合
                    'ロールバックしてエラーを返却

                    '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START
                    'If result <> TabletSMBCommonClassBusinessLogic.ActionResult.Success Then
                    '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

                    Me.Rollback = True
                    Logger.Warn(String.Format(CultureInfo.InvariantCulture, "{0}.E Error:Failed to CancelCarIn.", System.Reflection.MethodBase.GetCurrentMethod.Name))
                    Return result

                End If

            Catch ex As OracleExceptionEx When ex.Number = 1013
                Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.E Error:DBTimeOutError.", System.Reflection.MethodBase.GetCurrentMethod.Name))
                Me.Rollback = True
                Return TabletSMBCommonClassBusinessLogic.ActionResult.DBTimeOutError

            End Try

        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", System.Reflection.MethodBase.GetCurrentMethod.Name))

        '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        'Return TabletSMBCommonClassBusinessLogic.ActionResult.Success

        Return result

        '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

    End Function

#End Region

#Region "削除処理"

    ' 2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    ' ''' <summary>
    ' ''' ストールチップ削除
    ' ''' </summary>
    ' ''' <param name="stallUseId">ストール利用ID</param>
    ' ''' <param name="objStaffContext">スタッフ情報</param>
    ' ''' <param name="rowLockVersion">行ロックバージョン</param>
    ' ''' <returns>戻り値「0：正常終了、0以外：エラー」</returns>
    ' ''' <remarks></remarks>
    '<EnableCommit()>
    'Public Function DeleteStallChip(ByVal stallUseId As Decimal, _
    '                            ByVal objStaffContext As StaffContext, _
    '                            ByVal rowLockVersion As Long) As Long

    ''' <summary>
    ''' ストールチップ削除
    ''' </summary>
    ''' <param name="stallUseId">ストール利用ID</param>
    ''' <param name="rowLockVersion">行ロックバージョン</param>
    ''' <returns>実行結果</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発
    ''' </history>
    <EnableCommit()>
    Public Function DeleteStallChip(ByVal stallUseId As Decimal, _
                                    ByVal rowLockVersion As Long) As Long
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S.", System.Reflection.MethodBase.GetCurrentMethod.Name))

        '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        '戻り値
        Dim result As Long = TabletSMBCommonClassBusinessLogic.ActionResult.Success

        '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        Using clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic
            Try

                '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

                '' 更新処理を実行する
                ''2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                ''Dim result As Long = clsTabletSMBCommonClass.DeleteStallChip(stallUseId, objStaffContext, PGMID_SMB, rowLockVersion)
                'Dim result As Long = clsTabletSMBCommonClass.DeleteStallChip(stallUseId, PGMID_SMB, rowLockVersion)
                ''2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

                'ストールチップ削除処理実施
                result = clsTabletSMBCommonClass.DeleteStallChip(stallUseId, _
                                                                 PGMID_SMB, _
                                                                 rowLockVersion)

                '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

                'ストールチップ削除処理結果チェック
                If result <> TabletSMBCommonClassBusinessLogic.ActionResult.Success AndAlso _
                   result <> TabletSMBCommonClassBusinessLogic.ActionResult.WarningOmitDmsError Then
                    '「0：成功」「-9000：DMS除外エラーの警告」以外の場合
                    'ロールバックしてエラーを返却

                    '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START
                    'If result <> TabletSMBCommonClassBusinessLogic.ActionResult.Success Then
                    '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

                    Me.Rollback = True
                    Logger.Warn(String.Format(CultureInfo.InvariantCulture, "{0}.E Error:Failed to DeleteStallChip.", System.Reflection.MethodBase.GetCurrentMethod.Name))
                    Return result

                End If

                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                '削除されたチップに紐付く作業指示情報を移す(通知出す用)
                TabletSmbCommonCancelInstructedChipInfo = clsTabletSMBCommonClass.TabletSmbCommonCancelInstructedChipInfo
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

            Catch ex As OracleExceptionEx When ex.Number = 1013
                Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.E Error:DBTimeOutError.", System.Reflection.MethodBase.GetCurrentMethod.Name))
                Me.Rollback = True
                Return TabletSMBCommonClassBusinessLogic.ActionResult.DBTimeOutError

            End Try

        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", System.Reflection.MethodBase.GetCurrentMethod.Name))

        '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        'Return TabletSMBCommonClassBusinessLogic.ActionResult.Success

        Return result

        '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

    End Function

    ''' <summary>
    ''' ストール使用不可削除
    ''' </summary>
    ''' <param name="stallIdleId">ストール非稼働ID</param>
    ''' <param name="dtNow">更新日時</param>
    ''' <param name="staffCode">スタッフコード</param>
    ''' <returns>戻り値「0：正常終了、0以外：エラー」</returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Function DeleteStallUnavailable(ByVal stallIdleId As Decimal, ByVal dtNow As Date, ByVal staffCode As String, ByVal rowLockVersion As Long) As Long

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S.", System.Reflection.MethodBase.GetCurrentMethod.Name))
        Dim cnt As Long = 0
        ' ステータス遷移可否をチェックする
        Using clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic
            ' 更新処理を実行する
            cnt = clsTabletSMBCommonClass.DeleteStallUnavailable(stallIdleId, dtNow, staffCode, rowLockVersion, PGMID_SMB)
        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", System.Reflection.MethodBase.GetCurrentMethod.Name))
        Return cnt
    End Function

#End Region

#Region "NOSHOW処理"

    ''' <summary>
    '''   NoShow処理
    ''' </summary>
    ''' <param name="stallUseId">ストール利用ID</param>
    ''' <param name="dtNow">更新日時</param>
    ''' <param name="objStaffContext">スタッフ情報</param>
    ''' <param name="rowLockVersion">行ロックバージョン</param>
    ''' <returns>正常終了：0、異常終了：エラーコード</returns>
    ''' <history>
    ''' 2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発
    ''' </history>
    <EnableCommit()>
    Public Function StallChipNoShow(ByVal stallUseId As Decimal, _
                                    ByVal dtNow As Date, _
                                    ByVal objStaffContext As StaffContext, _
                                    ByVal rowLockVersion As Long) As Long

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S.", System.Reflection.MethodBase.GetCurrentMethod.Name))

        '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        '戻り値
        Dim result As Long = TabletSMBCommonClassBusinessLogic.ActionResult.Success

        '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        Using clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic
            Try

                '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START
                '' 更新処理を実行する
                'Dim result As Long = clsTabletSMBCommonClass.NoShow(stallUseId, dtNow, objStaffContext, rowLockVersion)

                'NoShow処理実施
                result = clsTabletSMBCommonClass.NoShow(stallUseId, _
                                                        dtNow, _
                                                        objStaffContext, _
                                                        rowLockVersion)

                '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

                'NoShow処理結果チェック
                If result <> TabletSMBCommonClassBusinessLogic.ActionResult.Success AndAlso _
                   result <> TabletSMBCommonClassBusinessLogic.ActionResult.WarningOmitDmsError Then
                    '「0：成功」「-9000：DMS除外エラーの警告」以外の場合
                    'ロールバックしてエラーを返却

                    '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START
                    'If result <> TabletSMBCommonClassBusinessLogic.ActionResult.Success Then
                    '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

                    Me.Rollback = True
                    Logger.Warn(String.Format(CultureInfo.InvariantCulture, "{0}.E Error:Failed to NoShow.", System.Reflection.MethodBase.GetCurrentMethod.Name))
                    Return result

                End If

            Catch ex As OracleExceptionEx When ex.Number = 1013
                Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.E Error:DBTimeOutError.", System.Reflection.MethodBase.GetCurrentMethod.Name))
                Me.Rollback = True
                Return TabletSMBCommonClassBusinessLogic.ActionResult.DBTimeOutError

            End Try

        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", System.Reflection.MethodBase.GetCurrentMethod.Name))

        '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        'Return TabletSMBCommonClassBusinessLogic.ActionResult.Success

        Return result

        '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

    End Function

#End Region

#Region "中断処理"

    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    ' ''' <summary>
    ' '''   中断処理
    ' ''' </summary>
    ' ''' <param name="svcinId">サービス入庫ID</param>
    ' ''' <param name="stallUseId">ストール利用ID</param>
    ' ''' <param name="stallId">ストールID</param>
    ' ''' <param name="rsltStartDateTime">実績開始日時</param>
    ' ''' <param name="rsltEndDateTime">実績終了日時</param>
    ' ''' <param name="stallWaitTime">中断時間</param>
    ' ''' <param name="stopMemo">中断メモ</param>
    ' ''' <param name="stopReasonType">中断原因</param>
    ' ''' <param name="restFlg">休憩フラグ</param>
    ' ''' <param name="objStaffContext">スタッフ情報</param>
    ' ''' <returns>正常終了：0、異常終了：エラーコード</returns>
    '<EnableCommit()>
    'Public Function StallChipJobStop(ByVal svcinId As Decimal, _
    '                                 ByVal stallUseId As Decimal, _
    '                                 ByVal stallId As Decimal, _
    '                                 ByVal rsltStartDateTime As Date, _
    '                                 ByVal rsltEndDateTime As Date, _
    '                                 ByVal stallWaitTime As Long, _
    '                                 ByVal stopMemo As String, _
    '                                 ByVal stopReasonType As String, _
    '                                 ByVal restFlg As String, _
    '                                 ByVal stallStartTime As Date, _
    '                                 ByVal stallEndTime As Date, _
    '                                 ByVal dtNow As Date, _
    '                                 ByVal objStaffContext As StaffContext, _
    '                                 ByVal rowLockVersion As Long) As Long


    ''' <summary>
    '''   中断処理
    ''' </summary>
    ''' <param name="stallUseId">ストール利用ID</param>
    ''' <param name="rsltEndDateTime">実績終了日時</param>
    ''' <param name="stallWaitTime">中断時間</param>
    ''' <param name="stopMemo">中断メモ</param>
    ''' <param name="stopReasonType">中断原因区分</param>
    ''' <param name="restFlg">休憩フラグ</param>
    ''' <param name="updateDate">更新日時</param>
    ''' <param name="rowLockVersion">行ロックバージョン</param>
    ''' <returns>正常終了：0、異常終了：エラーコード</returns>
    ''' <history>
    ''' 2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発
    ''' </history>
    <EnableCommit()>
    Public Function StallChipJobStop(ByVal stallUseId As Decimal, _
                                     ByVal rsltEndDateTime As Date, _
                                     ByVal stallWaitTime As Long, _
                                     ByVal stopMemo As String, _
                                     ByVal stopReasonType As String, _
                                     ByVal restFlg As String, _
                                     ByVal updateDate As Date, _
                                     ByVal rowLockVersion As Long) As Long
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S.", System.Reflection.MethodBase.GetCurrentMethod.Name))

        '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        '戻り値
        Dim result As Long = TabletSMBCommonClassBusinessLogic.ActionResult.Success

        '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        Using clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic
            Try
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                ''開始時間から終了時間までの範囲に重複休憩エリアがあるか
                'Dim workTime As Long = DateDiff("n", rsltStartDateTime, rsltEndDateTime)
                'Dim hasRestTimeInServiceTime As Boolean = clsTabletSMBCommonClass.HasRestTimeInServiceTime(stallStartTime, stallEndTime, stallId, rsltStartDateTime, workTime, True)
                ''休憩または使用不可エリアと重複場合、
                'If hasRestTimeInServiceTime Then
                '    '画面に重複で表示してない
                '    If IsNothing(restFlg) Then
                '        Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.E OverlapError", System.Reflection.MethodBase.GetCurrentMethod.Name))
                '        Return TabletSMBCommonClassBusinessLogic.ActionResult.OverlapError
                '    End If
                'End If


                '' サービス入庫をロックして、チェックする
                'Dim result As Long = clsTabletSMBCommonClass.LockServiceInTable(svcinId, rowLockVersion, objStaffContext.Account, dtNow, PGMID_SMB)
                'If result <> TabletSMBCommonClassBusinessLogic.ActionResult.Success Then
                '    Me.Rollback = True
                '    Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.E LockServiceInTableError", System.Reflection.MethodBase.GetCurrentMethod.Name))
                '    Return result
                'End If
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

                '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

                '' 更新処理を実行する
                'Dim result As Long = clsTabletSMBCommonClass.JobStop(stallUseId, _
                '                                                     rsltEndDateTime, _
                '                                                     stallWaitTime, _
                '                                                     stopMemo, _
                '                                                     stopReasonType, _
                '                                                     restFlg, _
                '                                                     updateDate, _
                '                                                     rowLockVersion, _
                '                                                     PGMID_SMB)

                '中断処理実施
                result = clsTabletSMBCommonClass.JobStop(stallUseId, _
                                                         rsltEndDateTime, _
                                                         stallWaitTime, _
                                                         stopMemo, _
                                                         stopReasonType, _
                                                         restFlg, _
                                                         updateDate, _
                                                         rowLockVersion, _
                                                         PGMID_SMB)

                '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

                '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
                '中断操作でPushするかフラグ
                NeedPushAfterStopJob = clsTabletSMBCommonClass.NeedPushAfterStopSingleJob
                '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

                '中断処理結果チェック
                If result <> TabletSMBCommonClassBusinessLogic.ActionResult.Success AndAlso _
                   result <> TabletSMBCommonClassBusinessLogic.ActionResult.WarningOmitDmsError Then
                    '「0：成功」「-9000：DMS除外エラーの警告」以外の場合
                    'ロールバックしてエラーを返却

                    '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START
                    'If result <> TabletSMBCommonClassBusinessLogic.ActionResult.Success Then
                    '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

                    Me.Rollback = True
                    Logger.Warn(String.Format(CultureInfo.InvariantCulture, "{0}.End. Error:Failed to JobStop. ", System.Reflection.MethodBase.GetCurrentMethod.Name))
                    Return result

                End If

                '中断で生成された使用不可チップのIDを取得する
                NewStallIdleId = clsTabletSMBCommonClass.NewStallIdleId

                '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

                'If result <> TabletSMBCommonClassBusinessLogic.ActionResult.Success Then
                '    Me.Rollback = True
                '    Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.E Failed to JobStop. ", System.Reflection.MethodBase.GetCurrentMethod.Name))
                '    Return result

                'End If

                '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

            Catch ex As OracleExceptionEx When ex.Number = 1013
                Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.E DBTimeOutError.", System.Reflection.MethodBase.GetCurrentMethod.Name))
                Me.Rollback = True
                Return TabletSMBCommonClassBusinessLogic.ActionResult.DBTimeOutError

            End Try

        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", System.Reflection.MethodBase.GetCurrentMethod.Name))

        '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        'Return TabletSMBCommonClassBusinessLogic.ActionResult.Success

        Return result

        '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

    End Function


    '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START

    ''' <summary>
    ''' 未開始Job存在判定
    ''' </summary>
    ''' <param name="inJobDtlId">作業内容ID</param>
    ''' <returns>true：開始前Jobが存在する、false：開始前Jobが存在しない</returns>
    ''' <remarks></remarks>
    Public Function HasBeforeStartJob(ByVal inJobDtlId As Decimal) As Boolean

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.Start. inJobDtlId={1}", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  inJobDtlId))

        Using clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic

            Dim bHasBeforeStartJob As Boolean = _
                clsTabletSMBCommonClass.HasBeforeStartJob(inJobDtlId)

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}.End return={1}", _
                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                      bHasBeforeStartJob))

            Return bHasBeforeStartJob

        End Using

    End Function

    '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

#End Region

#Region "非稼働エリア移動、リサイズ処理"

    ''' <summary>
    ''' 非稼働エリア移動、リサイズ処理
    ''' </summary>
    ''' <param name="stallIdleId">非稼働テーブルID</param>
    ''' <param name="stallId">ストールID</param>
    ''' <param name="idleStartDateTime">非稼働エリア開始時間</param>
    ''' <param name="workTime">非稼働エリアの時間</param>
    ''' <param name="stallStartTime">稼働開始日時</param>
    ''' <param name="stallEndTime">稼働終了日時</param>
    ''' <param name="dtNow">更新日時</param>
    ''' <param name="objStaffContext">スタッフ情報</param>
    ''' <param name="rowLockVersion">行ロックバージョン</param>
    ''' <returns></returns>
    <EnableCommit()>
    Public Function StallUnavailableChipMoveResize(ByVal stallIdleId As Decimal, _
                                                    ByVal stallId As Decimal, _
                                                    ByVal idleStartDateTime As Date, _
                                                    ByVal workTime As Long, _
                                                    ByVal stallStartTime As Date, _
                                                    ByVal stallEndTime As Date, _
                                                    ByVal dtNow As Date, _
                                                    ByVal objStaffContext As StaffContext, _
                                                    ByVal rowLockVersion As Long) As Long

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S.", System.Reflection.MethodBase.GetCurrentMethod.Name))
        Dim result As Long
        Using clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic
            Try

                '終了時間を取得する
                '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
                'Dim endDateTime As Date = clsTabletSMBCommonClass.GetServiceEndDateTime(stallId, idleStartDateTime, workTime, stallStartTime, stallEndTime, "0")
                Dim serviceEndDateTimeData As ServiceEndDateTimeData = clsTabletSMBCommonClass.GetServiceEndDateTime(stallId, idleStartDateTime, workTime, stallStartTime, stallEndTime, "0")
                Dim endDateTime As Date = serviceEndDateTimeData.ServiceEndDateTime
                '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END

                'ストール利用．チップ重複配置チェック
                '2017/09/07 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加 START
                'result = clsTabletSMBCommonClass.UpdateStallUnavailable(stallIdleId, stallId, idleStartDateTime, _
                '                     endDateTime, Nothing, stallStartTime, stallEndTime, dtNow, dtNow, objStaffContext, PGMID_SMB, rowLockVersion)
                '呼び出し先でstallStartTime, stallEndTimeの利用がないため削除
                result = clsTabletSMBCommonClass.UpdateStallUnavailable(stallIdleId, stallId, idleStartDateTime, _
                     endDateTime, Nothing, dtNow, dtNow, objStaffContext, PGMID_SMB, rowLockVersion)
                '2017/09/07 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加 END
                'エラーコードを戻す
                If result <> TabletSMBCommonClassBusinessLogic.ActionResult.Success Then
                    Me.Rollback = True
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Error:Failed to UpdateStallUnavailable.", System.Reflection.MethodBase.GetCurrentMethod.Name))
                    Return result
                End If

            Catch ex As OracleExceptionEx When ex.Number = 1013
                Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.E Error:DBTimeOutError.", System.Reflection.MethodBase.GetCurrentMethod.Name))
                Me.Rollback = True
                Return TabletSMBCommonClassBusinessLogic.ActionResult.DBTimeOutError
            End Try
        End Using
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", System.Reflection.MethodBase.GetCurrentMethod.Name))
        Return TabletSMBCommonClassBusinessLogic.ActionResult.Success
    End Function

#End Region

#Region "リレーションコピー"

    ''' <summary>
    ''' リレーションコピー
    ''' </summary>
    ''' <param name="stallUseId">ストール利用ID</param>
    ''' <param name="jobDtlId">作業内容ID</param>
    ''' <param name="svcinId">サービス入庫ID</param>
    ''' <param name="stallId">変更後のストールのSTALLID</param>
    ''' <param name="scheStartDateTime">変更後の表示開始日時</param>
    ''' <param name="scheWorkTime">仕事時間</param>
    ''' <param name="restFlg">休憩取得フラグ</param>
    ''' <param name="stallStartTime">稼働開始日時</param>
    ''' <param name="stallEndTime">稼働終了日時</param>
    ''' <param name="updatedt">更新日時</param>
    ''' <param name="objStaffContext">スタッフコード</param>
    ''' <param name="rowLockVersion">ローロックバージョン</param>
    ''' <param name="scheDeliDate">ローロックバージョン</param>
    ''' <param name="inspectionNeedFlg">検査必須フラグ</param>
    ''' <returns></returns>
    ''' <history>
    ''' 2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発
    ''' </history>
    <EnableCommit()>
    Public Function RelationCopy(ByVal stallUseId As Decimal, _
                                 ByVal jobDtlId As Decimal, _
                                 ByVal svcinId As Decimal, _
                                 ByVal stallId As Decimal, _
                                 ByVal scheStartDateTime As Date, _
                                 ByVal scheWorkTime As Long, _
                                 ByVal restFlg As String, _
                                 ByVal stallStartTime As Date, _
                                 ByVal stallEndTime As Date, _
                                 ByVal updatedt As Date, _
                                 ByVal objStaffContext As StaffContext, _
                                 ByVal rowLockVersion As Long, _
                                 ByVal inspectionNeedFlg As String, _
                                 ByVal picekDeliType As String, _
                                 ByVal scheDeliDate As Date, _
                                 ByVal scheSvcinDateTime As Date) As Long

        'ストールロックフラグ
        Dim isStallLock As Boolean = False

        '戻り値
        Dim result As Long

        Using clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic
            Try
                'ストールロック処理実施
                result = clsTabletSMBCommonClass.LockStall(stallId, _
                                                           scheStartDateTime, _
                                                           objStaffContext.Account, _
                                                           updatedt, _
                                                           PGMID_SMB)

                'ストールロック処理結果チェック
                If result <> TabletSMBCommonClassBusinessLogic.ActionResult.Success Then
                    '「0：成功」出ない場合
                    'ロールバックして処理結果を返却
                    Me.Rollback = True
                    Return result

                End If

                'ストールロック成功
                isStallLock = True

                'リレーションコピー処理実施
                result = clsTabletSMBCommonClass.RelationCopy(stallUseId, _
                                                              jobDtlId, _
                                                              svcinId, _
                                                              stallId, _
                                                              scheStartDateTime, _
                                                              scheWorkTime, _
                                                              restFlg, _
                                                              stallStartTime, _
                                                              stallEndTime, _
                                                              updatedt, _
                                                              objStaffContext, _
                                                              inspectionNeedFlg, _
                                                              picekDeliType, _
                                                              scheSvcinDateTime, _
                                                              scheDeliDate, _
                                                              rowLockVersion, _
                                                              PGMID_SMB)

                'リレーションコピー処理結果チェック
                If result <> TabletSMBCommonClassBusinessLogic.ActionResult.Success AndAlso _
                   result <> TabletSMBCommonClassBusinessLogic.ActionResult.WarningOmitDmsError Then
                    '「0：成功」「-9000：DMS除外エラーの警告」以外の場合
                    'ロールバックして処理結果を返却

                    '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START
                    'If result <> TabletSMBCommonClassBusinessLogic.ActionResult.Success Then
                    '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

                    Me.Rollback = True
                    Return result

                End If

                '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

                'Return TabletSMBCommonClassBusinessLogic.ActionResult.Success

                Return result

                '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

            Catch ex As OracleExceptionEx When ex.Number = 1013
                Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.E Error:DBTimeOutError.", System.Reflection.MethodBase.GetCurrentMethod.Name))
                Me.Rollback = True
                Return TabletSMBCommonClassBusinessLogic.ActionResult.DBTimeOutError

            Finally

                If isStallLock Then
                    'ストールロック解除
                    clsTabletSMBCommonClass.LockStallReset(stallId, scheStartDateTime, objStaffContext.Account, updatedt, PGMID_SMB)
                End If

            End Try

        End Using

    End Function

#End Region

    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START

#Region "Undo処理"

    ''' <summary>
    ''' 作業中チップの履歴情報を取得(前回だけの履歴情報)
    ''' </summary>
    ''' <param name="svcInIdList">サービス入庫ID</param>
    ''' <returns>履歴情報</returns>
    ''' <remarks></remarks>
    Public Function GetWorkingChipHisInfo(ByVal svcInIdList As List(Of Decimal)) As TabletSMBCommonClassDataSet.TabletSmbCommonClassChipHisDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S.", System.Reflection.MethodBase.GetCurrentMethod.Name))
        Using clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return clsTabletSMBCommonClass.GetWorkingChipHis(svcInIdList)
        End Using

    End Function

    '2016/04/20 NSK 小牟禮 工程管理の初期表示処理性能改善対応 START
    ''' <summary>
    ''' 作業中チップの履歴情報を取得(前回だけの履歴情報) ※営業時間より取得する
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="stallStartTime">稼働時間From</param>
    ''' <param name="stallEndTime">稼働時間To</param>
    ''' <returns>履歴情報</returns>
    ''' <remarks></remarks>
    Public Function GetWorkingChipHisInfoFromStallTime(ByVal dealerCode As String, _
                                          ByVal branchCode As String, _
                                          ByVal stallStartTime As Date, _
                                          ByVal stallEndTime As Date _
                                          ) As TabletSMBCommonClassDataSet.TabletSmbCommonClassChipHisDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S.", System.Reflection.MethodBase.GetCurrentMethod.Name))
        Using clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return clsTabletSMBCommonClass.GetWorkingChipHisFromStallTime(dealerCode, branchCode, stallStartTime, stallEndTime)
        End Using

    End Function
    '2016/04/20 NSK 小牟禮 工程管理の初期表示処理性能改善対応 END

    '2019/07/22 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
    ' ''' <summary>
    ' ''' 作業中チップのUndo操作
    ' ''' </summary>
    ' ''' <param name="svcinId">サービス入庫ID</param>
    ' ''' <param name="stallUseId">ストール利用ID</param>
    ' ''' <param name="updatedt">更新日時</param>
    ' ''' <param name="objStaffContext">スタッフ情報</param>
    ' ''' <param name="rowLockVersion">行ロックバージョン</param>
    ' ''' <returns>実行結果</returns>
    ' ''' <remarks></remarks>
    ' ''' <history>
    ' ''' 2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発
    ' ''' </history>
    '<EnableCommit()>
    'Public Function UndoWorkingChip(ByVal svcinId As Decimal, _
    '                                ByVal stallUseId As Decimal, _
    '                                ByVal updatedt As Date, _
    '                                ByVal objStaffContext As StaffContext, _
    '                                ByVal rowLockVersion As Long) As Long
    ''' <summary>
    ''' 作業中チップのUndo操作
    ''' </summary>
    ''' <param name="svcinId">サービス入庫ID</param>
    ''' <param name="stallUseId">ストール利用ID</param>
    ''' <param name="dtStartDate">稼動開始日時</param>
    ''' <param name="dtEndDate">稼動終了日時</param>
    ''' <param name="updatedt">更新日時</param>
    ''' <param name="objStaffContext">スタッフ情報</param>
    ''' <param name="rowLockVersion">行ロックバージョン</param>
    ''' <returns>実行結果</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発
    ''' </history>
    <EnableCommit()>
    Public Function UndoWorkingChip(ByVal svcinId As Decimal, _
                                    ByVal stallUseId As Decimal, _
                                    ByVal dtStartDate As Date, _
                                    ByVal dtEndDate As Date, _
                                    ByVal updatedt As Date, _
                                    ByVal objStaffContext As StaffContext, _
                                    ByVal rowLockVersion As Long) As Long
        '2019/07/22 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END

        '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        '戻り値
        Dim result As Long = TabletSMBCommonClassBusinessLogic.ActionResult.Success

        '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        Using clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic
            Try

                '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

                '' サービス入庫をロックして、チェックする
                'Dim result As Long = clsTabletSMBCommonClass.LockServiceInTable(svcinId, rowLockVersion, objStaffContext.Account, updatedt, PGMID_SMB)

                'サービス入庫をロック処理実施
                result = clsTabletSMBCommonClass.LockServiceInTable(svcinId, _
                                                                    rowLockVersion, _
                                                                    objStaffContext.Account, _
                                                                    updatedt, _
                                                                    PGMID_SMB)

                '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END


                'サービス入庫をロック処理結果チェック
                If result <> TabletSMBCommonClassBusinessLogic.ActionResult.Success Then
                    '「0：成功」以外の場合
                    'ロールバックしてエラーを返却
                    Me.Rollback = True
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Error:LockServiceInTableError", System.Reflection.MethodBase.GetCurrentMethod.Name))
                    Return result

                End If

                '作業中チップUndo処理実施
                '2019/07/22 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
                result = clsTabletSMBCommonClass.UndoWorkingChip(stallUseId, dtStartDate, dtEndDate, updatedt, objStaffContext, PGMID_SMB)
                '2019/07/22 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END


                '作業中チップUndo処理結果チェック
                If result <> TabletSMBCommonClassBusinessLogic.ActionResult.Success AndAlso _
                   result <> TabletSMBCommonClassBusinessLogic.ActionResult.WarningOmitDmsError Then
                    '「0：成功」「-9000：DMS除外エラーの警告」以外の場合
                    'ロールバックしてエラーを返却

                    '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START
                    'If result <> TabletSMBCommonClassBusinessLogic.ActionResult.Success Then
                    '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

                    Me.Rollback = True
                    Return result

                End If

                '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

                'Return TabletSMBCommonClassBusinessLogic.ActionResult.Success

                '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

            Catch ex As OracleExceptionEx When ex.Number = 1013
                Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.E Error:DBTimeOutError.", System.Reflection.MethodBase.GetCurrentMethod.Name))
                Me.Rollback = True
                Return TabletSMBCommonClassBusinessLogic.ActionResult.DBTimeOutError

            End Try

        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", System.Reflection.MethodBase.GetCurrentMethod.Name))

        '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        'Return TabletSMBCommonClassBusinessLogic.ActionResult.Success

        Return result

        '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

    End Function

#End Region

#Region "チーフテクニシャン処理"
    ''' <summary>
    ''' 指定チーフテクニシャンアカウントのストールIDを取得する
    ''' </summary>
    ''' <param name="account">チーフテクニシャンアカウント</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetStallidByChtAccount(ByVal account As String) As TabletSMBCommonClassDataSet.TabletSmbCommonClassNumberValueDataTable
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S.", System.Reflection.MethodBase.GetCurrentMethod.Name))
        Using clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic
            Return clsTabletSMBCommonClass.GetStallidByChtAccount(account)
        End Using
    End Function
#End Region

#Region "テクニシャン選択処理"
    ''' <summary>
    ''' ストールスタッフを更新する
    ''' </summary>
    ''' <param name="addStaff">新しいチェックしたスタッフコード</param>
    ''' <param name="deletedStaff">チェックマーク外したスタッフコード</param>
    ''' <param name="addRowLockVersion">新しいチェックしたスタッフコードの行ロックバージョン</param>
    ''' <param name="deleteRowLockVersion">チェックマーク外したスタッフコードの行ロックバージョン</param>
    ''' <param name="selectedStallId">タップされたストールID</param>
    ''' <param name="updateDate">更新日時</param>
    ''' <param name="objStaffContext">スタッフ情報</param>
    ''' <returns>実行結果</returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Function UpdateStallStaff(ByVal addStaff As String, _
                                     ByVal deletedStaff As String, _
                                     ByVal addRowLockVersion As String, _
                                     ByVal deleteRowLockVersion As String, _
                                     ByVal selectedStallId As Decimal, _
                                     ByVal updateDate As Date, _
                                     ByVal objStaffContext As StaffContext) As Long

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S.  addStaff={1}, deletedStaff={2}, addRowLockVersion={3}, deleteRowLockVersion={4}, selectedStallId={5}" _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name, addStaff, deletedStaff, addRowLockVersion, deleteRowLockVersion, selectedStallId))

        '行ロックバージョンをチェック引数を作成
        Dim staffCheck As StringBuilder = New StringBuilder
        Dim rowLockVersionCheck As StringBuilder = New StringBuilder
        If Not String.IsNullOrEmpty(addStaff) Then
            staffCheck.Append(addStaff)
            rowLockVersionCheck.Append(addRowLockVersion)
            'deletedStaffがない場合、コンマが要らない
            If Not String.IsNullOrEmpty(deletedStaff) Then
                staffCheck.Append(",")
                rowLockVersionCheck.Append(",")
            End If
        End If
        If Not String.IsNullOrEmpty(deletedStaff) Then
            staffCheck.Append(deletedStaff)
            rowLockVersionCheck.Append(deleteRowLockVersion)
        End If

        '行ロックバージョンチェックを行う
        If Me.CheckStallStaffRowLockVersion(staffCheck.ToString(), rowLockVersionCheck.ToString()) _
                <> TabletSMBCommonClassBusinessLogic.ActionResult.Success Then
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Error:RowLockVersionError.", System.Reflection.MethodBase.GetCurrentMethod.Name))
            Me.Rollback = True
            Return TabletSMBCommonClassBusinessLogic.ActionResult.RowLockVersionError
        End If

        Using ta As New SC3240101DataSetTableAdapters.SC3240101DataAdapter
            'チェック外したのはdb更新を行う
            If Not String.IsNullOrEmpty(deletedStaff) Then
                Dim deleteStaffString As String() = deletedStaff.Split(","c)
                Dim updateResult As Long
                'チェック外すから、stallIdが0に設定する
                updateResult = ta.UpdateStaffStallSetStallId(0, _
                                                            ConvertStringToDBString(deletedStaff), _
                                                            updateDate, _
                                                            objStaffContext.Account, _
                                                            PGMID_SMB)

                If updateResult <> deleteStaffString.Length Then
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E. Error:UpdateStaffStallSetStallId failed.", System.Reflection.MethodBase.GetCurrentMethod.Name))
                    Me.Rollback = True
                    Return TabletSMBCommonClassBusinessLogic.ActionResult.ExceptionError
                End If
            End If
        End Using
        '新しいチェックしたのはdb更新を行う
        If Not String.IsNullOrEmpty(addStaff) Then
            Dim addStaffCode As String() = addStaff.Split(","c)
            Dim addStaffRowLockVersion As String() = addRowLockVersion.Split(","c)
            'RowLockVersionにより、
            '該スタッフがストールスタッフテーブルにあれば、update操作をする
            '無ければ、insert操作をする
            Dim insertStaffSb As New StringBuilder
            Dim updateStaffSb As New StringBuilder
            Dim insertCount As Long = 0
            Dim updateCount As Long = 0
            For i As Integer = 0 To addStaffCode.Length - 1
                'RowLockVersionがないの場合、insert操作をする
                If String.IsNullOrEmpty(addStaffRowLockVersion(i)) Then
                    insertStaffSb.Append(addStaffCode(i))
                    insertStaffSb.Append(",")
                    insertCount = insertCount + 1
                Else
                    updateStaffSb.Append(addStaffCode(i))
                    updateStaffSb.Append(",")
                    updateCount = updateCount + 1
                End If
            Next

            Dim insertStaff As String = insertStaffSb.ToString()
            Dim updateStaff As String = updateStaffSb.ToString()

            If insertCount > 0 Then
                insertStaff = insertStaff.Substring(0, insertStaff.Length - 1)
                '挿入処理を行う
                Dim insertStaffString As String() = insertStaff.Split(","c)
                Using ta As New SC3240101DataSetTableAdapters.SC3240101DataAdapter
                    For Each staffCode As String In insertStaffString
                        Dim updateResult As Long = ta.InsertTblStaffJob(selectedStallId, _
                                                                        staffCode, _
                                                                        updateDate, _
                                                                        objStaffContext.Account, _
                                                                        PGMID_SMB)
                        If updateResult <> 1 Then
                            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E. ExceptionError:InsertTblStaffJob failed. updateResult={1}", _
                                                       System.Reflection.MethodBase.GetCurrentMethod.Name, updateResult))
                            Me.Rollback = True
                            Return TabletSMBCommonClassBusinessLogic.ActionResult.ExceptionError
                        End If
                    Next
                End Using
            End If
            If updateCount > 0 Then
                updateStaff = updateStaff.Substring(0, updateStaff.Length - 1)
                Using ta As New SC3240101DataSetTableAdapters.SC3240101DataAdapter
                    '更新処理を行う
                    Dim updateResult As Long = ta.UpdateStaffStallSetStallId(selectedStallId, _
                                                                             ConvertStringToDBString(updateStaff), _
                                                                             updateDate, _
                                                                             objStaffContext.Account, _
                                                                             PGMID_SMB)
                    If updateResult <> updateCount Then
                        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E. ExceptionError:UpdateStaffStallSetStallId failed. updateResult={1}", _
                                                   System.Reflection.MethodBase.GetCurrentMethod.Name, updateResult))
                        Me.Rollback = True
                        Return TabletSMBCommonClassBusinessLogic.ActionResult.ExceptionError
                    End If
                End Using
            End If

            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
            '更新終わった後、該ストールに4名テクニシャン以下かチェックする
            'Dim staffCount As SC3240101DataSet.SC3240101NumberValueDataTable = ta.GetStaffCountByStallid(selectedStallId)

            '2014/07/02 TMEJ 張 BTS-283 「サービスマネージャをTCとして表示」対応 START
            'Dim staffCount As SC3240101DataSet.SC3240101NumberValueDataTable = ta.GetStaffCountByStallid(selectedStallId, _
            '                                                                                             objStaffContext.DlrCD, _
            '                                                                                             objStaffContext.BrnCD)
            Me.InitStaffStallDispType(objStaffContext)
            Using ta As New SC3240101DataSetTableAdapters.SC3240101DataAdapter
                Dim staffCount As SC3240101DataSet.SC3240101NumberValueDataTable = ta.GetStaffCountByStallid(selectedStallId, _
                                                                                                             objStaffContext.DlrCD, _
                                                                                                             objStaffContext.BrnCD, _
                                                                                                             Me.StfStallDispType)
                '2014/07/02 TMEJ 張 BTS-283 「サービスマネージャをTCとして表示」対応 END

                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
                If staffCount(0).COL1 > 4 Then
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E. Error:OverMaxTechnicianNumsError ", _
                                                   System.Reflection.MethodBase.GetCurrentMethod.Name))
                    Me.Rollback = True
                    Return TabletSMBCommonClassBusinessLogic.ActionResult.OverMaxTechnicianNumsError
                End If
            End Using
        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E.", System.Reflection.MethodBase.GetCurrentMethod.Name))
        Return TabletSMBCommonClassBusinessLogic.ActionResult.Success



    End Function

    ''' <summary>
    ''' ストールスタッフテーブルの行ロックバージョンチェック
    ''' </summary>
    ''' <param name="staffCodes">スタッフコード</param>
    ''' <param name="rowLockVersions">行ロックバージョン</param>
    ''' <returns>check結果</returns>
    ''' <remarks></remarks>
    Private Function CheckStallStaffRowLockVersion(ByVal staffCodes As String, ByVal rowLockVersions As String) As Long

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S.  staffCodes={1}, rowLockVersions={2}" _
                , System.Reflection.MethodBase.GetCurrentMethod.Name, staffCodes, rowLockVersions))

        '空白の場合、戻す
        If String.IsNullOrEmpty(staffCodes) Then
            Return TabletSMBCommonClassBusinessLogic.ActionResult.Success
        End If

        Dim checkTable As SC3240101DataSet.SC3240101StallStaffDataTable
        Using ta As New SC3240101DataSetTableAdapters.SC3240101DataAdapter
            'スタッフコードより、対応の行ロックバージョンを取得する
            checkTable = ta.GetAllStallStaffRowlockVersion(ConvertStringToDBString(staffCodes))
        End Using


        Dim StaffCode As String() = staffCodes.Split(","c)
        Dim rowLockVersion As String() = rowLockVersions.Split(","c)
        '1つづつチェックする
        For i As Integer = 0 To StaffCode.Length - 1
            Dim checkRow As SC3240101DataSet.SC3240101StallStaffRow() = _
                CType(checkTable.Select(String.Format(CultureInfo.CurrentCulture, "STF_CD = '{0}'", StaffCode(i))), SC3240101DataSet.SC3240101StallStaffRow())
            '行ロックバージョンが空白の場合、チェックテーブルに該記録がないはず
            '行ロックバージョンがあれば、チェックテーブルに対応記録の値が同じはず
            '上記の2つ情況ではない場合、エラーを出す
            If Not ((String.IsNullOrEmpty(rowLockVersion(i)) And checkRow.Length = 0) _
                    Or (checkRow.Length = 1 AndAlso rowLockVersion(i).Equals(checkRow(0).ROW_LOCK_VERSION))) Then
                If checkRow.Length = 1 Then
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.End. Error:RowLockVersionError: StaffCode={1}, RowLockVersion1={2}, RowLockVersion2={3}", _
                                                   System.Reflection.MethodBase.GetCurrentMethod.Name, StaffCode(i), rowLockVersion(i), checkRow(0).ROW_LOCK_VERSION))
                Else
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.End. Error:RowLockVersionError: StaffCode={1}, RowLockVersion={2}, checkRows={3}", _
                                                   System.Reflection.MethodBase.GetCurrentMethod.Name, StaffCode(i), rowLockVersion(i), checkRow.Length))
                End If

                Return TabletSMBCommonClassBusinessLogic.ActionResult.RowLockVersionError
            End If
        Next

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.End.", System.Reflection.MethodBase.GetCurrentMethod.Name))
        Return TabletSMBCommonClassBusinessLogic.ActionResult.Success
    End Function

    ''' <summary>
    ''' String"111,222,333..."を"N'111',N'222',N'333'..."に変える
    ''' </summary>
    ''' <param name="stringData">変更前データ</param>
    ''' <returns>変更後データ</returns>
    ''' <remarks></remarks>
    Private Function ConvertStringToDBString(ByVal stringData As String) As String
        Dim splitString As String() = stringData.Split(","c)
        Dim retStringBuilder As New StringBuilder
        For Each tempString As String In splitString
            retStringBuilder.Append("N'")
            retStringBuilder.Append(tempString)
            retStringBuilder.Append("',")
        Next

        Dim retString = retStringBuilder.ToString()
        '最後のコンマを削除する
        retString = retString.Substring(0, retString.Length - 1)
        Return retString
    End Function
#End Region

    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

    '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする START
#Region "計画取り消し処理"
    ''' <summary>
    ''' 計画取り消しを実行する
    ''' </summary>
    ''' <param name="serviceInId">サービス利用ID</param>
    ''' <param name="jobDtlId">作業内容ID</param>
    ''' <param name="stallUseId">ストール利用ID</param>
    ''' <param name="updateDate">更新日時</param>
    ''' <param name="staffcode">スタッフコード</param>
    ''' <param name="functionId">機能ID</param>
    ''' <param name="chipInstructFlg">着工指示フラグ</param>
    ''' <returns>実行結果</returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Function StallChipToReception(ByVal serviceInId As Decimal, _
                         ByVal jobDtlId As Decimal, _
                         ByVal stallUseId As Decimal, _
                         ByVal updateDate As Date, _
                         ByVal staffcode As String, _
                         ByVal functionId As String, _
                         ByVal rowLockVersion As Long, _
                         ByRef chipInstructFlg As Boolean) As Long

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S.", System.Reflection.MethodBase.GetCurrentMethod.Name))

        '戻り値
        Dim result As Long = TabletSMBCommonClassBusinessLogic.ActionResult.Success

        Using clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic
            Try

                result = clsTabletSMBCommonClass.ToReception(serviceInId, _
                                                             jobDtlId, _
                                                             stallUseId, _
                                                             updateDate, _
                                                             staffcode, _
                                                             functionId,
                                                             rowLockVersion, _
                                                             chipInstructFlg)

                '計画取り消し処理結果チェック
                If result <> TabletSMBCommonClassBusinessLogic.ActionResult.Success AndAlso _
                   result <> TabletSMBCommonClassBusinessLogic.ActionResult.WarningOmitDmsError Then
                    '「0：成功」「-9000：DMS除外エラーの警告」以外の場合
                    'ロールバックしてエラーを返却

                    Me.Rollback = True
                    Logger.Warn(String.Format(CultureInfo.InvariantCulture, "{0}.E Error:Failed to ToReception.", System.Reflection.MethodBase.GetCurrentMethod.Name))
                    Return result

                End If

            Catch ex As OracleExceptionEx When ex.Number = 1013
                Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.E Error:DBTimeOutError.", System.Reflection.MethodBase.GetCurrentMethod.Name))
                Me.Rollback = True
                Return TabletSMBCommonClassBusinessLogic.ActionResult.DBTimeOutError

            End Try


        End Using

        Return result

    End Function
#End Region
    '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする END

    '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
#Region "休憩取得変更処理"
    ''' <summary>
    ''' 休憩取得変更処理
    ''' </summary>
    ''' <param name="stallUseId">ストール利用ID</param>
    ''' <param name="restFlg">休憩フラグ</param>
    ''' <param name="stallStartTime">稼働開始日時</param>
    ''' <param name="stallEndTime">稼働終了日時</param>
    ''' <param name="dtNow">更新日時</param>
    ''' <param name="objStaffContext">スタッフ情報</param>
    ''' <param name="rowLockVersion">更新回数</param>
    ''' <returns></returns>
    <EnableCommit()>
    Public Function RestChange(ByVal stallUseId As Decimal, _
                               ByVal restFlg As String, _
                               ByVal stallStartTime As Date, _
                               ByVal stallEndTime As Date, _
                               ByVal dtNow As Date, _
                               ByVal objStaffContext As StaffContext, _
                               ByVal rowLockVersion As Long) As Long

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S", System.Reflection.MethodBase.GetCurrentMethod.Name))

        'ストールロックフラグ
        Dim isStallLock As Boolean = False

        Dim dispStartDateTime As Date = Nothing
        Dim stallId As Decimal = Nothing

        '戻り値
        Dim result As Long

        Using clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic
            Try
                Dim dtChipEntity As TabletSmbCommonClassChipEntityDataTable = clsTabletSMBCommonClass.GetChipEntity(stallUseId)

                '取得結果が1件以外(通常あり得ない)の場合はエラー
                If dtChipEntity.Count <> 1 Then
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                               "{0}.{1}_Error GetChipEntityError", _
                                               Me.GetType.ToString, _
                                               System.Reflection.MethodBase.GetCurrentMethod.Name))
                    'チップエンティティ取得エラー
                    Return ActionResult.GetChipEntityError
                End If

                ' ストール利用tableのストール利用ステータスが「02:作業中」または「04:作業計画の一部作業が中断」の場合
                If StalluseStatusStart.Equals(dtChipEntity(0).STALL_USE_STATUS) Or StalluseStatusStartIncludeStopJob.Equals(dtChipEntity(0).STALL_USE_STATUS) Then
                    ' 実績開始日時を設定
                    dispStartDateTime = dtChipEntity(0).RSLT_START_DATETIME
                Else
                    ' 予定開始日時を設定
                    dispStartDateTime = dtChipEntity(0).SCHE_START_DATETIME
                End If

                stallId = dtChipEntity(0).STALL_ID

                'ストールロック処理実施
                result = clsTabletSMBCommonClass.LockStall(stallId, dispStartDateTime, objStaffContext.Account, dtNow, PGMID_SMB)

                'ストールロック処理結果チェック
                If result <> TabletSMBCommonClassBusinessLogic.ActionResult.Success Then
                    '「0：成功」以外の場合
                    'ロールバックして処理結果を返却
                    Me.Rollback = True
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Error:LockStallError", System.Reflection.MethodBase.GetCurrentMethod.Name))
                    Return result

                End If

                'ストールロックフラグをTrueにする
                isStallLock = True

                'チップ移動、リサイズ処理実施
                result = clsTabletSMBCommonClass.MoveAndResize(stallUseId, _
                                                               stallId, _
                                                               dispStartDateTime, _
                                                               dtChipEntity(0).SCHE_WORKTIME, _
                                                               restFlg, _
                                                               stallStartTime, _
                                                               stallEndTime, _
                                                               dtNow, _
                                                               objStaffContext, _
                                                               PGMID_SMB, _
                                                               dtNow, _
                                                               rowLockVersion)

                'チップ移動、リサイズ処理結果チェック
                If result <> TabletSMBCommonClassBusinessLogic.ActionResult.Success AndAlso _
                   result <> TabletSMBCommonClassBusinessLogic.ActionResult.WarningOmitDmsError Then
                    '「0：成功」「-9000：DMS除外エラーの警告」以外の場合
                    'ロールバックしてエラーを返却

                    Me.Rollback = True
                    Logger.Warn(String.Format(CultureInfo.InvariantCulture, "{0}.E Error:MoveAndResize failed.", System.Reflection.MethodBase.GetCurrentMethod.Name))
                    Return result

                End If

            Catch ex As OracleExceptionEx When ex.Number = 1013
                Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.E Error:DBTimeOutError.", System.Reflection.MethodBase.GetCurrentMethod.Name))
                Me.Rollback = True
                Return TabletSMBCommonClassBusinessLogic.ActionResult.DBTimeOutError

            Finally
                'ストールロックフラグのチェック
                If isStallLock Then
                    'ストールロックしている場合
                    'ストールロック解除処理実施
                    clsTabletSMBCommonClass.LockStallReset(stallId, _
                                                           dispStartDateTime, _
                                                           objStaffContext.Account, _
                                                           dtNow, _
                                                           PGMID_SMB)

                End If

            End Try

        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return result

    End Function
#End Region
    '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END

End Class
