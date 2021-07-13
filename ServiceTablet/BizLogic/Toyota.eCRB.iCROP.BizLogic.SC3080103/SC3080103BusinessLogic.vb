'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3080103BusinessLogic.vb
'─────────────────────────────────────
'機能： 顧客検索結果一覧BusinessLogic
'補足： 
'作成： 2013/12/20 TMEJ陳	TMEJ次世代サービス 工程管理機能開発
'更新： 2014/09/16 TMEJ 小澤 BTS不具合対応 RO_INFO検索時は販売店と店舗も条件に入れるように修正
'更新： 2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発
'更新： 2015/11/10 TM 杉田  (トライ店システム評価)SMBチップ検索の絞り込み方法変更
'更新： 2017/01/06 NSK  竹中 TR-SVT-TMT-20160602-001 顧客がWelcomeボードに表示されない
'更新： 2017/03/22 NSK  秋田 TR-SVT-TMT-20151224-001 顧客一覧が遅れてwelcome boardに表示される
'更新： 2018/02/27 NSK  山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証
'更新： 2018/06/22 NSK  可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示
'更新： 2018/07/19 NSK  坂本 TKM Next Gen e-CRB Project Application development Block B-1  顧客タイプ分類
'更新： 2019/03/05 NSK 山田 18PRJ02750-00_(トライ店システム評価)サービス業務における次世代オペレーション実現の為の性能対策
'更新： 
'─────────────────────────────────────
Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.CustomerInfo.Search.DataAccess.SC3080103DataSetTableAdapters
Imports Toyota.eCRB.CustomerInfo.Search.DataAccess
Imports Toyota.eCRB.SMBLinkage.Reservation.Api.BizLogic
Imports Toyota.eCRB.SMBLinkage.Reservation.Api.DataAccess
Imports Toyota.eCRB.DMSLinkage.CustomerInfo.Api.DataAccess
Imports Toyota.eCRB.DMSLinkage.CustomerInfo.Api.DataAccess.IC3800709DataSet
Imports Toyota.eCRB.DMSLinkage.CustomerInfo.Api.BizLogic
Imports Toyota.eCRB.DMSLinkage.CustomerInfo.Api.BizLogic.IC3800709BusinessLogic
Imports Toyota.eCRB.SMBLinkage.Customer.BizLogic
Imports Toyota.eCRB.SMBLinkage.Customer.DataAccess.IC3810203DataSet
Imports Toyota.eCRB.iCROP.BizLogic.IC3810301
Imports Toyota.eCRB.iCROP.DataAccess.IC3810301.IC3810301DataSet
Imports Toyota.eCRB.Visit.Api.DataAccess.VisitUtilityDataSetTableAdapters
Imports Toyota.eCRB.Visit.Api.DataAccess
Imports Toyota.eCRB.Visit.Api.DataAccess.VisitUtilityDataSet
Imports System.Reflection
Imports System.Text
Imports Toyota.eCRB.Visit.Api.BizLogic
Imports Toyota.eCRB.CommonUtility.SMBCommonClass.Api.BizLogic
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.BizLogic
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.DataAccess

Public Class SC3080103BusinessLogic
    Inherits BaseBusinessComponent
    Implements IDisposable

#Region "定数"

    ''' <summary>
    ''' ページID
    ''' </summary>
    Private Const WordIdPageId As String = "SC3080103"
    ''' <summary>
    ''' 文言ID（-）
    ''' </summary>
    Private Const WordIdHyphen As Integer = 20
    ''' <summary>
    ''' 文言ID（R/O未作成）
    ''' </summary>
    Private Const WordIdUnCreated As Integer = 24
    ''' <summary>
    ''' 文言ID（R/O作成中）
    ''' </summary>
    Private Const WordIdCreated As Integer = 25
    ''' <summary>
    ''' Log終了文言
    ''' </summary>
    ''' <remarks></remarks>
    Private Const LOG_END As String = "End"
    ''' <summary>
    ''' DateTimeFuncにて、"MM/dd"形式にコンバートするための定数
    ''' </summary>
    Private Const CONVERTDATE_MD As Integer = 11
    ''' <summary>
    ''' DateTimeFuncにて、"hh:mm"形式にコンバートするための定数
    ''' </summary>
    Private Const CONVERTDATE_HM As Integer = 14
    ''' <summary>
    ''' 行追加ステータス（0：追加していない行）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const AddRecordTypeOff As String = "0"
    ''' <summary>
    ''' 行追加ステータス（1：追加した行）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const AddRecordTypeOn As String = "1"
    ''' <summary>
    ''' ROステータス（10：RO起票）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RO_SAIssuing As String = "10"
    ''' <summary>
    ''' ROステータス（15：TC Issuing）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RO_TCIssuing As String = "15"
    ''' <summary>
    ''' ROステータス（20：Waiting for FM Approval）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RO_WaitingForFMApproval As String = "20"
    ''' <summary>
    ''' ROステータス（25：Creating Parts rough quotation）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RO_CreatingPartsRoughQuotation As String = "25"
    ''' <summary>
    ''' ROステータス（30：Creating Parts quotation）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RO_CreatingPartsQuotation As String = "30"
    ''' <summary>
    ''' ROステータス（35：Waiting for R/O Confirmation）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RO_WaitingForROConfirmation As String = "35"
    ''' <summary>
    ''' ROステータス（40：Waiting for Customer Approval）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RO_WaitingForCustomerApproval As String = "40"
    ''' <summary>
    ''' ROステータス（50：Approved by Customer）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RO_ApprovedByCustomer As String = "50"

    ''' <summary>
    ''' サービスステータス（07：洗車待ち）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ServiceStatusWaitWash As String = "07"
    ''' <summary>
    ''' サービスステータス（08：洗車中）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ServiceStatusWashing As String = "08"
    ''' <summary>
    ''' サービスステータス（12：納車待ち）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ServiceStatusWaitDelivery As String = "12"

    ''' <summary>
    ''' ストール利用テータス（05：中断）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StallUseStatusStop As String = "05"
    ''' <summary>
    ''' ストール利用テータス（07：未来店客）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StallUseStatusNoVisitor As String = "07"

    ''' <summary>
    ''' 完成検査ステータス（1：完成検査承認待ち）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ApprovalStatusWaitApproval As String = "1"

    ''' <summary>
    ''' VIP区分（1：VIP）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VIP_FLG As String = "1"

    '2018/07/19 NSK  坂本 TKM Next Gen e-CRB Project Application development Block B-1  顧客タイプ分類 START
    ' ''' <summary>
    ' ''' 顧客区分（P：個人）
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const CUSTOMER_TYPE_PERSONAL_VALUE As String = "P"

    ''' <summary>
    ''' 顧客区分（1：個人）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CUSTOMER_TYPE_PERSONAL_VALUE As String = "1"
    '2018/07/19 NSK  坂本 TKM Next Gen e-CRB Project Application development Block B-1  顧客タイプ分類 END

    ''' <summary>
    ''' 顧客区分（S：自社客）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CUSTOMER_TYPE_MY_COMPANY_VALUE As String = "S"

    '2018/07/19 NSK  坂本 TKM Next Gen e-CRB Project Application development Block B-1  顧客タイプ分類 START
    ' ''' <summary>
    ' ''' 法人フラグ（F：個人）
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const CORPORATION_TYPE_MINE_VALUE As String = "F"

    ''' <summary>
    ''' 法人フラグ（0：法人）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CORPORATION_TYPE_MINE_VALUE As String = "0"
    '2018/07/19 NSK  坂本 TKM Next Gen e-CRB Project Application development Block B-1  顧客タイプ分類 END

    ''' <summary>
    ''' 顧客区分（1：個人）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CUSTOMER_TYPE_PERSONAL As String = "1"

    ''' <summary>
    ''' 顧客区分（2：自社客）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CUSTOMER_TYPE_MY_COMPANY As String = "2"

    ''' <summary>
    ''' 顧客区分（3：Other）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CUSTOMER_TYPE_OTHER As String = "3"

    ''' <summary>
    ''' 法人フラグ（0：個人）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CORPORATION_TYPE_MINE As String = "0"

    ''' <summary>
    ''' 予約ありフラグ（1：予約あり）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const APPOINTMENT_CST As String = "1"

    ''' <summary>
    ''' 検索条件（1：車両登録No）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SearchTypeRegisterNo As String = "1"
    ''' <summary>
    ''' 検索条件（2：顧客氏名）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SearchTypeCustomerName As String = "2"
    ''' <summary>
    ''' 検索条件（3：VIN）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SearchTypeVin As String = "3"
    ''' <summary>
    ''' 検索条件（4：予約番号）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SearchTypeBaseRez As String = "4"
    ''' <summary>
    ''' 検索条件（5：RO番号）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SearchTypeOrderNo As String = "5"

    ''' <summary>
    ''' マッチタイプ（0：完全一致）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MatchTypeExact As String = "0"
    ''' <summary>
    ''' マッチタイプ（1：前方一致）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MatchTypeForward As String = "1"
    ''' <summary>
    ''' マッチタイプ（2：後方一致）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MatchTypeBackword As String = "2"

    ''' <summary>
    ''' 車両ソートタイプ（1：昇順）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SortTypeVehicleAcs As String = "1"
    ''' <summary>
    ''' 車両ソートタイプ（2：降順）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SortTypeVehicleDesc As String = "2"
    ''' <summary>
    ''' 顧客ソートタイプ（1：昇順）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SortTypeCustomerAcs As String = "1"
    ''' <summary>
    ''' 顧客ソートタイプ（2：降順）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SortTypeCustomerDesc As String = "2"
    ''' <summary>
    ''' SAソートタイプ（1：昇順）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SortTypeSAAcs As String = "1"
    ''' <summary>
    ''' SAソートタイプ（2：降順）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SortTypeSADesc As String = "2"
    ''' <summary>
    ''' SCソートタイプ（1：昇順）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SortTypeSCAcs As String = "1"
    ''' <summary>
    ''' SCソートタイプ（2：降順）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SortTypeSCDesc As String = "2"
    ''' <summary>
    ''' SCソート（0：Vclregno）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const Sort1DefaultValue As String = "0"
    ''' <summary>
    ''' リターンコード
    ''' </summary>
    Private Enum ReturnCode

        ''' <summary>
        ''' 成功
        ''' </summary>
        Success = 0

        ''' <summary>
        ''' DBタイムアウト
        ''' </summary>
        ErrDBTimeout = 901

        ''' <summary>
        ''' 該当データなし
        ''' </summary>
        ResultNoMatch = 902

        ''' <summary>
        ''' 登録失敗
        ''' </summary>
        InsertFailure = 903

    End Enum

    ''' <summary>
    ''' DateTimeFuncにて、"yyyy/MM/dd HH:mm:ss"形式をコンバートするためのID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DATE_CONVERT_ID_YYYYMMDDHHMMSS As Integer = 1
    ''' <summary>
    ''' メッセージID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RequestCustomerDetailId As String = "IC35151"
    ''' <summary>
    ''' LinkSystemCode
    ''' </summary>
    ''' <remarks></remarks>
    Private Const LinkSystemCode As String = "0"

    '2017/03/22 NSK  秋田 TR-SVT-TMT-20151224-001 顧客一覧が遅れてwelcome boardに表示される START
    ''' <summary>
    ''' 削除フラグ（未削除）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DeleteFlagNone As String = "0"
    '2017/03/22 NSK  秋田 TR-SVT-TMT-20151224-001 顧客一覧が遅れてwelcome boardに表示される END

#Region "エラーコード"

    ''' <summary>
    ''' システム設定不備エラー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ErrorSysEnv As Integer = 1121

    ''' <summary>
    ''' 販売店システム設定不備エラー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ErrorDlrEnv As Integer = 1150

#End Region

#Region "システム設定名"

    ''' <summary>
    ''' 基幹連携送信タイムアウト値
    ''' </summary>
    Private Const SysLinkSendTimeOutVal = "LINK_SEND_TIMEOUT_VAL"

    ''' <summary>
    ''' 国コード
    ''' </summary>
    Private Const SysCountryCode = "DIST_CD"

    ''' <summary>
    ''' 日付フォーマット
    ''' </summary>
    Private Const SysDateFormat = "DATE_FORMAT"

    '2019/03/05 NSK 山田 18PRJ02750-00_(トライ店システム評価)サービス業務における次世代オペレーション実現の為の性能対策 START
    ''' <summary>
    ''' 車両登録番号区切り文字
    ''' </summary>
    Private Const SysRegNumDlmtr As String = "REG_NUM_DELIMITER"
    '2019/03/05 NSK 山田 18PRJ02750-00_(トライ店システム評価)サービス業務における次世代オペレーション実現の為の性能対策 END

#End Region

#End Region

#Region "メイン処理"

    ''' <summary>
    ''' 顧客情報を取得する
    ''' </summary>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inBranchCode">店舗コード</param>
    ''' <param name="inStaffCode">スタッフコード</param>
    ''' <param name="inSearchType">サーチタイプ</param>
    ''' <param name="inSearchValue">サーチ値</param>
    ''' <param name="inNowDate">今日の日付</param>
    ''' <param name="inStartIndex">検索結果開始索引</param>
    ''' <param name="inCount">最大検索数</param>
    ''' <param name="inSortModelCode">モデルコードソート値</param>
    ''' <param name="inSortCustomerName">顧客名ソート値</param>
    ''' <param name="inSortSAName">SAソート値</param>
    ''' <param name="inSortSCName">SCソート値</param>
    ''' <returns>取得結果</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Public Function GetCustomerList(ByVal inDealerCode As String, _
                                    ByVal inBranchCode As String, _
                                    ByVal inStaffCode As String, _
                                    ByVal inSearchType As String, _
                                    ByVal inSearchValue As String, _
                                    ByVal inNowDate As Date, _
                                    ByVal inStartIndex As Long, _
                                    ByVal inCount As Long, _
                                    ByVal inSortModelCode As String, _
                                    ByVal inSortCustomerName As String, _
                                    ByVal inSortSAName As String, _
                                    ByVal inSortSCName As String) As SC3080103DataSet.SC3080103CustomerInfoRow()
        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START IN:inDealerCode = {2}, inBranchCode = {3}, inSearchType = {4}" & _
                     "inSearchValue = {5}, inNowDate = {6}, inStartIndex = {7}, inCount = {8}" & _
                     "inSortModelCode = {9}, inSortCustomerName = {10}, inSortSAName = {11}, inSortSCName = {12}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , inDealerCode, inBranchCode, inSearchType, inSearchValue _
                    , inNowDate.ToString(CultureInfo.CurrentCulture), inStartIndex.ToString(CultureInfo.CurrentCulture) _
                    , inCount.ToString(CultureInfo.CurrentCulture), inSortModelCode, inSortCustomerName, inSortSAName, inSortSCName))

        Dim dtRequestTemp As New IC3800709DataSet.CustomerSearchRequestDataTable
        Dim rowIN As IC3800709DataSet.CustomerSearchRequestRow = dtRequestTemp.NewCustomerSearchRequestRow()

        rowIN.Sort1 = Sort1DefaultValue
        rowIN.Sort2 = String.Empty
        rowIN.BasRezid = String.Empty
        rowIN.BasRezid_MatchType = String.Empty
        rowIN.TelNumber = String.Empty
        rowIN.TelNumber_MatchType = String.Empty

        rowIN.DealerCode = inDealerCode
        rowIN.BranchCode = inBranchCode
        rowIN.StaffCode = inStaffCode
        rowIN.Start = inStartIndex.ToString(CultureInfo.CurrentCulture)
        rowIN.Count = inCount.ToString(CultureInfo.CurrentCulture)
        If SearchTypeRegisterNo.Equals(inSearchType) Then
            rowIN.VclRegNo = inSearchValue
            rowIN.VclRegNo_MatchType = MatchTypeBackword
        Else
            rowIN.VclRegNo = String.Empty
            rowIN.VclRegNo_MatchType = String.Empty
        End If
        If SearchTypeCustomerName.Equals(inSearchType) Then
            rowIN.CustomerName = inSearchValue
            rowIN.CustomerName_MatchType = MatchTypeForward
        Else
            rowIN.CustomerName = String.Empty
            rowIN.CustomerName_MatchType = String.Empty
        End If
        If SearchTypeVin.Equals(inSearchType) Then
            rowIN.Vin = inSearchValue
            rowIN.Vin_MatchType = MatchTypeBackword
        Else
            rowIN.Vin = String.Empty
            rowIN.Vin_MatchType = String.Empty
        End If
        If SearchTypeBaseRez.Equals(inSearchType) Then
            rowIN.BasRezid = inSearchValue
            rowIN.BasRezid_MatchType = MatchTypeBackword
        Else
            rowIN.BasRezid = String.Empty
            rowIN.BasRezid_MatchType = String.Empty
        End If
        If SearchTypeOrderNo.Equals(inSearchType) Then
            rowIN.R_O = inSearchValue
            rowIN.R_O_MatchType = MatchTypeExact
        Else
            rowIN.R_O = String.Empty
            rowIN.R_O_MatchType = String.Empty
        End If

        Dim dr As SC3080103DataSet.SC3080103CustomerInfoRow()

        '共通関数インスタンス化
        Using IC3800709Logic As New IC3800709BusinessLogic

            ''引数をログに出力
            Dim args As New List(Of String)

            'DataRow内の項目を列挙
            Me.AddLogData(args, rowIN)

            '開始ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} IN:{2}" _
                        , Me.GetType.ToString _
                        , MethodBase.GetCurrentMethod.Name _
                        , String.Join(", ", args.ToArray())))

            'WebService送信用XMLクラス作成処理
            Dim sendXml As CustomerSearchXmlDocumentClass = CreateXml(rowIN)

            'WebService呼出処理
            Dim dtWebServiceResult As IC3800709DataSet.CustomerSearchResultDataTable = _
                IC3800709Logic.CallGetCustomerSearchInfoWebService(sendXml)

            If IsNothing(dtWebServiceResult) Then
                Return Nothing
            End If

            '結果がない場合、0件データを返却する
            If dtWebServiceResult.Rows.Count = 0 Then
                Dim retRows(0) As SC3080103DataSet.SC3080103CustomerInfoRow
                Return retRows
            End If

            'WebServiceの結果確認
            If dtWebServiceResult Is Nothing OrElse dtWebServiceResult(0).ResultCode <> ResultSuccess Then
                'WebService処理失敗

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} WebServiceErr OUT:RETURNCODE = {2}" _
                        , Me.GetType.ToString _
                        , MethodBase.GetCurrentMethod.Name _
                        , ReturnCode.ResultNoMatch))

                Return Nothing

            End If

            '必要データ転換
            dr = Me.Convert2SC3080103DataTable(dtWebServiceResult, _
                                               inDealerCode, _
                                               inBranchCode, _
                                               inNowDate, _
                                               inSortModelCode, _
                                               inSortCustomerName, _
                                               inSortSAName, _
                                               inSortSCName)

        End Using

        Return dr

    End Function

    ''' <summary>
    ''' 返却データ変換
    ''' </summary>
    ''' <param name="inCustomerSearchResult">変換元データテーブル</param>
    ''' <param name="inNowDate">今日の日付</param>
    ''' <param name="inSortModelCode">モデルコードソート値</param>
    ''' <param name="inSortCustomerName">顧客名ソート値</param>
    ''' <param name="inSortSAName">SAソート値</param>
    ''' <param name="inSortSCName">SCソート値</param>
    ''' <returns>変換データテーブル</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発
    ''' </history>
    Private Function Convert2SC3080103DataTable(ByVal inCustomerSearchResult As IC3800709DataSet.CustomerSearchResultDataTable, _
                                               ByVal inDealerCode As String, _
                                               ByVal inBranchCode As String, _
                                               ByVal inNowDate As Date, _
                                               ByVal inSortModelCode As String, _
                                               ByVal inSortCustomerName As String, _
                                               ByVal inSortSAName As String, _
                                               ByVal inSortSCName As String) As SC3080103DataSet.SC3080103CustomerInfoRow()

        '開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START IN:{2}" _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name _
                    , inNowDate.ToString(CultureInfo.CurrentCulture)))

        '2019/03/05 NSK 山田 18PRJ02750-00_(トライ店システム評価)サービス業務における次世代オペレーション実現の為の性能対策 START

        '車両登録番号区切り文字を取得
        Dim regNumDlmtr As String
        Using smbCommonBiz As New ServiceCommonClassBusinessLogic
            regNumDlmtr = smbCommonBiz.GetSystemSettingValueBySettingName(SysRegNumDlmtr)
        End Using

        'DB検索用リスト生成
        Dim accountList As List(Of String) = New List(Of String)
        Dim dmsCstCdList As List(Of String) = New List(Of String)
        Dim vinList As List(Of String) = New List(Of String)
        Dim regNoList As List(Of String) = New List(Of String)
        For Each row As IC3800709DataSet.CustomerSearchResultRow In inCustomerSearchResult.Rows
            '車両登録番号リストに登録番号を追加
            If Not String.IsNullOrWhiteSpace(row.VehicleRegistrationNumber) Then
                '車両登録番号検索ワード変換 大文字変換
                Dim searchRegNum = ConvertVclRegNumWord(row.VehicleRegistrationNumber, regNumDlmtr).ToUpper
                If Not regNoList.Contains(searchRegNum) Then
                    regNoList.Add(searchRegNum)
                End If
            End If
            'VINリストにVINを追加
            If Not String.IsNullOrWhiteSpace(row.Vin) Then
                '大文字変換
                Dim searchVin As String = row.Vin.ToUpper
                If Not vinList.Contains(searchVin) Then
                    vinList.Add(searchVin)
                End If
            End If
            '基幹顧客コードリストに基幹顧客コードを追加
            If Not String.IsNullOrWhiteSpace(row.CustomerCode) AndAlso
               Not dmsCstCdList.Contains(row.CustomerCode) Then
                dmsCstCdList.Add(row.CustomerCode)
            End If
            'アカウントリストにSA、SAコードを追加
            If Not String.IsNullOrWhiteSpace(row.ServiceAdviserCode) Then
                Dim searchAccount As String = row.ServiceAdviserCode + "@" + inDealerCode
                If Not accountList.Contains(searchAccount) Then
                    accountList.Add(searchAccount)
                End If
            End If
            If Not String.IsNullOrWhiteSpace(row.SalesStaffCode) Then
                Dim searchAccount As String = row.SalesStaffCode + "@" + inDealerCode
                If Not accountList.Contains(searchAccount) Then
                    accountList.Add(searchAccount)
                End If
            End If
        Next

        '顧客検索結果から付帯情報取得
        Dim dtAccount As SC3080103DataSet.SC3080103UserInfoDataTable = New SC3080103DataSet.SC3080103UserInfoDataTable
        Dim dtCustomer As SC3080103DataSet.SC3080103AdditionCustomerInfoDataTable = New SC3080103DataSet.SC3080103AdditionCustomerInfoDataTable
        Dim dtVehicle As SC3080103DataSet.SC3080103VehicleInfoDataTable = New SC3080103DataSet.SC3080103VehicleInfoDataTable
        Dim dtApointment As SC3080103DataSet.SC3080103ApointmentInfoDataTable = New SC3080103DataSet.SC3080103ApointmentInfoDataTable
        Using dataAdapter As New SC3080103DataTableAdapter
            'アカウント情報取得
            If 0 < accountList.Count Then
                dtAccount = dataAdapter.GetAccountInfoList(accountList)
            End If
            '顧客情報取得
            If 0 < dmsCstCdList.Count Then
                dtCustomer = dataAdapter.GetCustomerInfoList(inDealerCode, dmsCstCdList)
            End If
            '車両情報取得
            If 0 < vinList.Count OrElse
               0 < regNoList.Count Then
                dtVehicle = dataAdapter.GetVehicleInfoList(inDealerCode, vinList, regNoList)
                '車両IDリスト生成
                Dim vclIdList As List(Of Decimal) = New List(Of Decimal)
                For Each row As SC3080103DataSet.SC3080103VehicleInfoRow In dtVehicle
                    If Not vclIdList.Contains(row.VCL_ID) Then
                        vclIdList.Add(row.VCL_ID)
                    End If
                Next

                '予約状況取得
                If 0 < vclIdList.Count Then
                    dtApointment = dataAdapter.GetApointmentInfoList(inDealerCode, inBranchCode, vclIdList, inNowDate)
                End If
            End If

        End Using
        '2019/03/05 NSK 山田 18PRJ02750-00_(トライ店システム評価)サービス業務における次世代オペレーション実現の為の性能対策 END

        Using dtRet As New SC3080103DataSet.SC3080103CustomerInfoDataTable
            '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 START
            'Dim dtRet As New SC3080103DataSet.SC3080103CustomerInfoDataTable
            '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 END

            For Each rowOrg As IC3800709DataSet.CustomerSearchResultRow In inCustomerSearchResult.Rows
                Dim rowDect As SC3080103DataSet.SC3080103CustomerInfoRow = dtRet.NewSC3080103CustomerInfoRow()

                '初期化
                Dim dCST_ID As Decimal = 0
                Dim dVCL_ID As Decimal = 0
                Dim strNAMETITLE_NAME As String = String.Empty
                Dim strPOSITIONTYPE As String = String.Empty
                Dim strCST_EMAIL_1 As String = String.Empty
                Dim strMODEL_NAME As String = String.Empty
                Dim strREG_AREA_NAME As String = String.Empty
                Dim strSA As String = String.Empty
                Dim strSC As String = String.Empty
                Dim strCST_TYPE As String = String.Empty
                Dim strFLEET_FLG As String = String.Empty
                Dim strVIP_FLG As String = String.Empty
                Dim strIMG_FILE As String = String.Empty
                Dim strAPPOINTMENT_FLG As String = String.Empty
                '2018/02/27 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 START
                Dim strSSC_MARK As String = String.Empty
                '2018/02/27 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 END

                '2018/06/22 NSK  可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
                Dim strPL_MARK As String = String.Empty
                Dim strMB_MARK As String = String.Empty
                Dim strE_MARK As String = String.Empty
                Dim strTLM_MBR_FLG As String = String.Empty
                '2018/06/22 NSK  可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END

                '2019/03/05 NSK 山田 18PRJ02750-00_(トライ店システム評価)サービス業務における次世代オペレーション実現の為の性能対策 START

                ''2015/11/10 TM 杉田  (トライ店システム評価)SMBチップ検索の絞り込み方法変更 START
                'Dim strConvertRegNumber As String = String.Empty
                'Using smbCommonBiz As New ServiceCommonClassBusinessLogic
                '    '車両登録番号の「*」と区切り文字を削除する
                '    strConvertRegNumber = smbCommonBiz.ConvertVclRegNumWord(rowOrg.VehicleRegistrationNumber)
                'End Using
                ''2015/11/10 TM 杉田  (トライ店システム評価)SMBチップ検索の絞り込み方法変更 END

                'Using da As New SC3080103DataTableAdapter

                '    '顧客IDと敬称とEmail1取得
                '    Dim dt As SC3080103DataSet.SC3080103CustomerInfoDataTable = _
                '        da.GetCustomerIDAndNameTitleAndEmail(rowOrg.CustomerCode)

                '    If dt.Count > 0 Then
                '        dCST_ID = dt(0).CST_ID
                '        strNAMETITLE_NAME = dt(0).NAMETITLE_NAME
                '        strCST_EMAIL_1 = dt(0).CST_EMAIL_1
                '        strPOSITIONTYPE = dt(0).POSITION_TYPE
                '    End If

                '    '車両ID取得
                '    '2015/11/10 TM 杉田  (トライ店システム評価)SMBチップ検索の絞り込み方法変更 START
                '    'dt = da.GetVCLID(inDealerCode, _
                '    '                 rowOrg.Vin, _
                '    '                 rowOrg.VehicleRegistrationNumber)
                '    dt = da.GetVCLID(inDealerCode, _
                '                     rowOrg.Vin, _
                '                     strConvertRegNumber)
                '    '2015/11/10 TM 杉田  (トライ店システム評価)SMBチップ検索の絞り込み方法変更 END

                '    If dt.Count > 0 Then
                '        dVCL_ID = dt(0).VCL_ID
                '        '2018/02/27 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 START
                '        strSSC_MARK = dt(0).SSC_MARK
                '        '2018/02/27 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 END
                '        '2018/06/22 NSK  可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
                '        strPL_MARK = dt(0).PL_MARK
                '        strMB_MARK = dt(0).MB_MARK
                '        strE_MARK = dt(0).E_MARK
                '        strTLM_MBR_FLG = dt(0).TLM_MBR_FLG
                '        '2018/06/22 NSK  可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
                '    End If

                '    'Model名称取得
                '    If Not rowOrg.IsModelCodeNull Then
                '        dt = da.GetModelName(rowOrg.ModelCode, dVCL_ID)

                '        If dt.Count > 0 Then
                '            strMODEL_NAME = dt(0).MODEL_NAME
                '        End If
                '    End If

                '    'Province取得
                '    dt = da.GetAreaName(inDealerCode, dVCL_ID)

                '    If dt.Count > 0 Then
                '        strREG_AREA_NAME = dt(0).REG_AREA_NAME
                '    End If

                '    'SA名称取得
                '    If Not rowOrg.IsServiceAdviserCodeNull Then
                '        dt = da.GetSAName(rowOrg.ServiceAdviserCode + "@" + inDealerCode)

                '        If dt.Count > 0 Then
                '            strSA = dt(0).SA
                '        End If
                '    End If

                '    'SC名称取得
                '    If Not rowOrg.IsSalesStaffCodeNull Then
                '        dt = da.GetSCName(rowOrg.SalesStaffCode + "@" + inDealerCode)

                '        If dt.Count > 0 Then
                '            strSC = dt(0).SC
                '        End If
                '    End If

                '    '予約状況チェック
                '    Dim hasApointmented As Boolean = da.GetApointmentCst(inDealerCode, _
                '                              inBranchCode, _
                '                              dCST_ID, _
                '                              dVCL_ID, _
                '                              inNowDate)

                '    If hasApointmented Then
                '        strAPPOINTMENT_FLG = APPOINTMENT_CST
                '    End If

                '    'VIPチェック
                '    If VIP_FLG.Equals(rowOrg.VIPFlg) Then
                '        'VIPの場合
                '        strVIP_FLG = VIP_FLG
                '    End If

                '    '個人・法人
                '    If CUSTOMER_TYPE_PERSONAL_VALUE.Equals(rowOrg.CustomerType) Then
                '        '個人の場合
                '        strCST_TYPE = CUSTOMER_TYPE_PERSONAL
                '    ElseIf CORPORATION_TYPE_MINE_VALUE.Equals(rowOrg.CustomerType) Then
                '        '法人客の場合
                '        strFLEET_FLG = CORPORATION_TYPE_MINE
                '    End If

                '    'イメージ取得
                '    dt = da.GetImageFileSmall(inDealerCode, _
                '                              dCST_ID)

                '    If dt.Count > 0 Then
                '        strIMG_FILE = dt(0).IMG_FILE
                '    End If

                'End Using


                'SC3080103DataTableAdapterのUsing内から移動
                'VIPチェック
                If VIP_FLG.Equals(rowOrg.VIPFlg) Then
                    'VIPの場合
                    strVIP_FLG = VIP_FLG
                End If

                '個人・法人
                If CUSTOMER_TYPE_PERSONAL_VALUE.Equals(rowOrg.CustomerType) Then
                    '個人の場合
                    strCST_TYPE = CUSTOMER_TYPE_PERSONAL
                ElseIf CORPORATION_TYPE_MINE_VALUE.Equals(rowOrg.CustomerType) Then
                    '法人客の場合
                    strFLEET_FLG = CORPORATION_TYPE_MINE
                End If

                '顧客情報取得
                Dim dmsCstCdSearch As String = rowOrg.CustomerCode
                Dim dtCustomerRow As IEnumerable(Of SC3080103DataSet.SC3080103AdditionCustomerInfoRow) = _
                                From row In dtCustomer
                                Where row.DMS_CST_CD = dmsCstCdSearch
                                Order By row.DMS_TAKEIN_DATETIME Descending

                If 0 < dtCustomerRow.Count Then
                    '顧客ID
                    dCST_ID = dtCustomerRow(0).CST_ID
                    '敬称
                    strNAMETITLE_NAME = dtCustomerRow(0).NAMETITLE_NAME
                    '敬称位置
                    strPOSITIONTYPE = dtCustomerRow(0).POSITION_TYPE
                    'メール１
                    strCST_EMAIL_1 = dtCustomerRow(0).CST_EMAIL_1
                    'イメージ画像(小)
                    strIMG_FILE = dtCustomerRow(0).IMG_FILE_SMALL
                End If
                '車両情報取得
                Dim regNumSearch = ConvertVclRegNumWord(rowOrg.VehicleRegistrationNumber, regNumDlmtr).ToUpper
                Dim vinSerach = rowOrg.Vin.ToUpper
                Dim dtVehicleRow As IEnumerable(Of SC3080103DataSet.SC3080103VehicleInfoRow) = _
                               From row In dtVehicle
                               Where row.VCL_VIN_SEARCH = vinSerach OrElse (row.VCL_VIN_SEARCH = " " AndAlso row.REG_NUM_SEARCH = regNumSearch)
                               Order By row.DMS_TAKEIN_DATETIME Descending, _
                                        row.CST_TYPE, _
                                        row.REG_NUM Descending, _
                                        row.VCL_VIN Descending, _
                                        row.VCL_ID Descending

                If 0 < dtVehicleRow.Count Then
                    '車両ID取得
                    dVCL_ID = dtVehicleRow(0).VCL_ID
                    'モデル名取得
                    If rowOrg.ModelCode = dtVehicleRow(0).MODEL_CD Then
                        strMODEL_NAME = dtVehicleRow(0).MODEL_NAME
                    End If
                    'エリア名取得
                    strREG_AREA_NAME = dtVehicleRow(0).REG_AREA_NAME
                    'SSCマーク取得
                    strSSC_MARK = dtVehicleRow(0).SSC_MARK
                    'PLマーク取得
                    strPL_MARK = dtVehicleRow(0).PL_MARK
                    'MBマーク取得
                    strMB_MARK = dtVehicleRow(0).MB_MARK
                    'Eマーク取得
                    strE_MARK = dtVehicleRow(0).E_MARK
                    'テレマ会員フラグ取得
                    strTLM_MBR_FLG = dtVehicleRow(0).TLM_MBR_FLG
                End If

                '予約状況取得
                Dim dtApointmentRow As IEnumerable(Of SC3080103DataSet.SC3080103ApointmentInfoRow) = _
                               From row In dtApointment
                               Where row.VCL_ID = dVCL_ID

                If 0 < dtApointmentRow.Count Then
                    strAPPOINTMENT_FLG = APPOINTMENT_CST
                End If

                'SAスタッフ取得
                Dim saSearch As String = rowOrg.ServiceAdviserCode + "@" + inDealerCode
                Dim dtSaRow As IEnumerable(Of SC3080103DataSet.SC3080103UserInfoRow) = _
                               From row In dtAccount
                               Where row.ACCOUNT = saSearch

                If 0 < dtSaRow.Count Then
                    strSA = dtSaRow(0).USERNAME
                End If
                'SCスタッフ取得
                Dim scSearch As String = rowOrg.SalesStaffCode + "@" + inDealerCode
                Dim dtScRow As IEnumerable(Of SC3080103DataSet.SC3080103UserInfoRow) = _
                               From row In dtAccount
                               Where row.ACCOUNT = scSearch

                If 0 < dtScRow.Count Then
                    strSC = dtScRow(0).USERNAME
                End If
                '2019/03/05 NSK 山田 18PRJ02750-00_(トライ店システム評価)サービス業務における次世代オペレーション実現の為の性能対策 END

                dtRet.AcceptChanges()

                rowDect.CST_ID = dCST_ID

                If rowOrg.CustomerCode.Length >= 2 Then
                    rowDect.DMS_CST_CD = rowOrg.CustomerCode.Substring(rowOrg.CustomerCode.IndexOf("@", StringComparison.CurrentCulture) + 1)
                Else
                    rowDect.DMS_CST_CD = rowOrg.CustomerCode.Substring(rowOrg.CustomerCode.IndexOf("@", StringComparison.CurrentCulture))
                End If
                rowDect.DLR_CD = inDealerCode
                rowDect.STR_CD = inBranchCode
                rowDect.VCL_ID = dVCL_ID
                rowDect.REG_NUM = rowOrg.VehicleRegistrationNumber
                rowDect.MODEL_NAME = strMODEL_NAME
                rowDect.VCL_VIN = rowOrg.Vin
                rowDect.CST_NAME = rowOrg.CustomerName
                rowDect.NAMETITLE_NAME = strNAMETITLE_NAME
                rowDect.POSITION_TYPE = strPOSITIONTYPE
                rowDect.REG_AREA_NAME = strREG_AREA_NAME
                rowDect.CST_TYPE = strCST_TYPE
                rowDect.FLEET_FLG = strFLEET_FLG
                rowDect.CUSTOMER_FLAG = rowOrg.Customer_Flag
                rowDect.VIPFLG = strVIP_FLG
                rowDect.APPOITMENT_FLG = strAPPOINTMENT_FLG
                rowDect.CST_PHONE = rowOrg.TelNumber
                rowDect.CST_MOBILE = rowOrg.Mobile
                rowDect.CST_EMAIL_1 = strCST_EMAIL_1
                rowDect.IMG_FILE = strIMG_FILE
                rowDect.VCL_KATASHIKI = String.Empty
                rowDect.SA = strSA
                rowDect.SC = strSC
                rowDect.SACODE = rowOrg.ServiceAdviserCode
                rowDect.MODEL_CD = rowOrg.ModelCode
                rowDect.ALLCOUNT = CType(rowOrg.AllCount, Long)
                '2018/02/27 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 START
                rowDect.SSC_MARK = strSSC_MARK
                '2018/02/27 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 END
                '2018/06/22 NSK  可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
                rowDect.PL_MARK = strPL_MARK
                rowDect.MB_MARK = strMB_MARK
                rowDect.E_MARK = strE_MARK
                rowDect.TLM_MBR_FLG = strTLM_MBR_FLG
                '2018/06/22 NSK  可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END

                dtRet.AddSC3080103CustomerInfoRow(rowDect)
            Next

            Return Me.SortDataTable(dtRet, inSortModelCode, inSortCustomerName, inSortSAName, inSortSCName)

            '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 START
        End Using
        '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 END

    End Function

    ''' <summary>
    ''' 顧客情報DataTableソート
    ''' </summary>
    ''' <param name="dtRet">修正元DataTable</param>
    ''' <param name="inSortModelCode">モデルコードソート値</param>
    ''' <param name="inSortCustomerName">顧客名ソート値</param>
    ''' <param name="inSortSAName">SAソート値</param>
    ''' <param name="inSortSCName">SCソート値</param>
    ''' <returns>処理結果コードを返却</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Function SortDataTable(ByVal dtRet As SC3080103DataSet.SC3080103CustomerInfoDataTable, _
                                               ByVal inSortModelCode As String, _
                                               ByVal inSortCustomerName As String, _
                                               ByVal inSortSAName As String, _
                                               ByVal inSortSCName As String) As SC3080103DataSet.SC3080103CustomerInfoRow()

        Dim drSortedCustomerInfo As SC3080103DataSet.SC3080103CustomerInfoRow() = _
                (From drCustomerInfo In dtRet
                 Select drCustomerInfo).ToArray

        'ソート条件追加
        If SortTypeVehicleAcs.Equals(inSortModelCode) Then
            'モデルコードの昇順
            drSortedCustomerInfo = _
                (From drCustomerInfo In dtRet _
                 Order By drCustomerInfo.MODEL_CD Ascending
                 Select drCustomerInfo).ToArray

        ElseIf SortTypeVehicleDesc.Equals(inSortModelCode) Then
            'モデルコードの降順
            drSortedCustomerInfo = _
                (From drCustomerInfo In dtRet _
                 Order By drCustomerInfo.MODEL_CD Descending
                 Select drCustomerInfo).ToArray

        ElseIf SortTypeCustomerAcs.Equals(inSortCustomerName) Then
            '顧客氏名の昇順
            drSortedCustomerInfo = _
                (From drCustomerInfo In dtRet _
                 Order By drCustomerInfo.CST_NAME Ascending
                 Select drCustomerInfo).ToArray

        ElseIf SortTypeCustomerDesc.Equals(inSortCustomerName) Then
            '顧客氏名の降順
            drSortedCustomerInfo = _
                (From drCustomerInfo In dtRet _
                 Order By drCustomerInfo.CST_NAME Descending
                 Select drCustomerInfo).ToArray

        ElseIf SortTypeSAAcs.Equals(inSortSAName) Then
            'SA氏名の昇順
            drSortedCustomerInfo = _
                (From drCustomerInfo In dtRet _
                 Order By drCustomerInfo.SA Ascending
                 Select drCustomerInfo).ToArray

        ElseIf SortTypeSADesc.Equals(inSortSAName) Then
            'SA氏名の降順
            drSortedCustomerInfo = _
                (From drCustomerInfo In dtRet _
                 Order By drCustomerInfo.SA Descending
                 Select drCustomerInfo).ToArray

        ElseIf SortTypeSCAcs.Equals(inSortSCName) Then
            'SC氏名の昇順
            drSortedCustomerInfo = _
                (From drCustomerInfo In dtRet _
                 Order By drCustomerInfo.SC Ascending
                 Select drCustomerInfo).ToArray

        ElseIf SortTypeSCDesc.Equals(inSortSCName) Then
            'SC氏名の降順
            drSortedCustomerInfo = _
                (From drCustomerInfo In dtRet _
                 Order By drCustomerInfo.SC Descending
                 Select drCustomerInfo).ToArray

        End If

        Return drSortedCustomerInfo

    End Function

    ''' <summary>
    ''' 来店情報登録及びRO情報登録・RO連携処理
    ''' </summary>
    ''' <param name="rowIN">来店登録引数</param>
    ''' <param name="nowDateIN">今日の日付</param>
    ''' <returns>処理結果コードを返却</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発
    ''' </history>
    <EnableCommit()>
    Public Function VisitRegistProccess(ByVal rowIN As IC3810203InCustomerSaveRow, _
                                        ByVal nowDateIN As Date) As IC3810203ReservationInfoRow
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim bizIC3810203 As New IC3810203BusinessLogic

        Try
            '来店情報登録
            '戻り値を保持
            Dim rowReservationInfo As IC3810203ReservationInfoRow = bizIC3810203.RegisterVisitManagement(rowIN)
            If rowReservationInfo._RETURN <> ReturnCode.Success Then
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} ERROR OUT:RETURNCODE = {2}" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                            , rowReservationInfo._RETURN.ToString(CultureInfo.CurrentCulture)))

                Me.Rollback = True

                Return Nothing
            End If

            Using da As New SC3080103DataTableAdapter

                'RO存在するかチェック
                Dim IsRoExists As Boolean = da.CheckRoExists(rowReservationInfo.VISITSEQ)

                '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 START
                'If IsRoExists Then
                '    'RO No.が存在する場合Return
                '    Return rowReservationInfo
                'End If

                If Not IsRoExists Then
                    'RO No.が存在しない場合

                    If rowIN.IsSVCIN_IDNull Then
                        '入庫IDはない場合は「0」を入れる
                        rowIN.SVCIN_ID = 0
                    End If

                    'RO連携→IC3810301のAPIを利用、RO登録を含めて処理される
                    'メソッド:InsertRepairOrderInfo()
                    '概要:    RO情報の登録()
                    '引数：   serviceInID (Long)　「入庫ID」
                    '　　　   visitSequence (Decimal)　「訪問連番」
                    '　　　   account (String)　「スタッフコード」
                    ' 　　    nowDataTime(Date) 「現在時刻」
                    '         applicationId(string) 「画面ID」

                    '戻り値： 登録結果 ：0(成功)
                    '　　　　　　　　　：901 (DBタイムアウト)
                    '　　　　　　　　　：903 (登録失敗)
                    Using IC3810301Biz As New IC3810301BusinessLogic

                        Dim resultCode As Long = IC3810301Biz.InsertRepairOrderInfo(rowIN.SVCIN_ID, rowIN.DLRCD, rowIN.STRCD, rowIN.VISITSEQ, rowIN.ACCOUNT, nowDateIN, WordIdPageId)
                        If resultCode <> ReturnCode.Success Then
                            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                        , "{0}.{1} ERROR OUT:RETURNCODE = {2}" _
                                        , Me.GetType.ToString _
                                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                        , resultCode.ToString(CultureInfo.CurrentCulture)))

                            Me.Rollback = True

                            Return Nothing
                        End If

                    End Using

                End If

                '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 END

            End Using

            Return rowReservationInfo

        Catch ex As OracleExceptionEx When ex.Number = 1013
            'ORACLEのタイムアウトのみ処理
            ''終了ログの出力
            Logger.Error(String.Format(CultureInfo.CurrentCulture _
                     , "{0}.{1} OUT:RETURNCODE = {2}" _
                     , Me.GetType.ToString _
                     , System.Reflection.MethodBase.GetCurrentMethod.Name _
                     , ReturnCode.ErrDBTimeout))

            Me.Rollback = True

            Return Nothing
        Finally
            If bizIC3810203 IsNot Nothing Then
                bizIC3810203.Dispose()
                bizIC3810203 = Nothing
            End If
        End Try

    End Function


    ''' <summary>
    ''' 予約情報を取得する
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="custCode">顧客コード</param>
    ''' <param name="vclRegNo">車両登録No</param>
    ''' <param name="vin">VIN</param>
    ''' <param name="baseDate">取得基準日</param>
    ''' <param name="nowDate">今日の日付</param>
    ''' <param name="isGetDmsCstFlg">自社客取得フラグ</param>
    ''' <returns>取得結果</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2014/09/16 TMEJ 小澤 BTS不具合対応 RO_INFO検索時は販売店と店舗も条件に入れるように修正
    ''' 2017/01/06 NSK  竹中 TR-SVT-TMT-20160602-001 顧客がWelcomeボードに表示されない
    ''' </history>
    Public Function GetReservationList(ByVal dealerCode As String, _
                                       ByVal branchCode As String, _
                                       ByVal custCode As String, _
                                       ByVal vclRegNo As String, _
                                       ByVal vin As String, _
                                       ByVal baseDate As String, _
                                       ByVal nowDate As Date, _
                                       ByVal isGetDmsCstFlg As Boolean) As SC3080103DataSet.SC3080103ReserveInfoDataTable

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} dealerCode:{2} branchCode:{3} custCode:{4} vclRegNo:{5} vin:{6} baseDate:{7} nowDate:{8}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , dealerCode, branchCode, custCode, vclRegNo, vin, baseDate, nowDate))

        '戻り値データ
        Dim dtRet As New SC3080103DataSet.SC3080103ReserveInfoDataTable
        Dim bl As New IC3811501BusinessLogic

        Try
            '2017/01/06 NSK  竹中 TR-SVT-TMT-20160602-001 顧客がWelcomeボードに表示されない START
            '取得基準日以降の予約情報を取得（※取得基準日を含む）
            Dim dt As IC3811501DataSet.IC3811501ReservationListDataTable = _
                bl.GetReservationList(dealerCode, _
                                      branchCode, _
                                      custCode, _
                                      vclRegNo, _
                                      vin, _
                                      baseDate, _
                                      isGetDmsCstFlg)
            '2017/01/06 NSK  竹中 TR-SVT-TMT-20160602-001 顧客がWelcomeボードに表示されない END

            '予約情報が存在しない場合は処理終了（予約情報ポップアップは出力しない）
            If IsNothing(dt) Then
                Return Nothing
            End If
            If dt.Rows.Count = 0 Then
                Return Nothing
            End If

            '文言の取得
            Dim unCreatedWord As String = WebWordUtility.GetWord(WordIdPageId, WordIdUnCreated)     '未生成
            Dim CreatedWord As String = WebWordUtility.GetWord(WordIdPageId, WordIdCreated)         '生成済

            '予約情報が存在する場合、戻り値データ用に編集してDataTableを返却する
            Dim dtMod As New SC3080103DataSet.SC3080103ReserveInfoDataTable
            Dim daSC3080103 As New SC3080103DataTableAdapter

            For Each row As IC3811501DataSet.IC3811501ReservationListRow In dt

                'レコード情報取得
                Dim dr As SC3080103DataSet.SC3080103ReserveInfoRow = dtMod.NewSC3080103ReserveInfoRow

                With dr

                    'RO情報が作成されているかチェック
                    If "0".Equals(row.ROSTATUSCODE) Then
                        '作成されていない場合

                        'RO No.
                        If row.IsORDERNONull Then
                            .RO_NUM = String.Empty
                        Else
                            .RO_NUM = row.ORDERNO
                        End If

                        '予約ID（サービス入庫ID）
                        .SVCIN_ID = row.REZID

                        '予約開始日時
                        .START_DATETIME = row.REZSTARTTIME

                        '予約終了日時
                        .END_DATETIME = row.REZENDTIME

                        'サービス名称
                        If row.IsSERVICENAMENull Then
                            .SERVICE_NAME = String.Empty
                        Else
                            .SERVICE_NAME = row.SERVICENAME
                        End If

                        'RO作業連番
                        If row.IsORDERNONull Then
                            .RO_JOB_SEQ = String.Empty
                        Else

                            '2014/09/16 TMEJ 小澤 BTS不具合対応 RO_INFO検索時は販売店と店舗も条件に入れるように修正 START

                            '.RO_JOB_SEQ = daSC3080103.GetROJobSeq(row.ORDERNO)
                            .RO_JOB_SEQ = daSC3080103.GetROJobSeq(dealerCode, branchCode, row.ORDERNO)

                            '2014/09/16 TMEJ 小澤 BTS不具合対応 RO_INFO検索時は販売店と店舗も条件に入れるように修正 END

                        End If

                        'DMS予約ID
                        If row.IsDMS_JOB_DTL_IDNull Then
                            .DMS_JOB_DTL_ID = String.Empty
                        Else
                            .DMS_JOB_DTL_ID = row.DMS_JOB_DTL_ID
                        End If

                        'ROの作成ステータス（ROステータスがNULL: 未作成の場合　→　『未作成』とする）
                        .ROSTATUS = unCreatedWord

                        '2017/01/06 NSK  竹中 TR-SVT-TMT-20160602-001 顧客がWelcomeボードに表示されない START
                        If row.IsCST_NAMENull Then
                            .CST_NAME = String.Empty
                        Else
                            .CST_NAME = row.CST_NAME
                        End If
                        '2017/01/06 NSK  竹中 TR-SVT-TMT-20160602-001 顧客がWelcomeボードに表示されない END

                    ElseIf "1".Equals(row.ROSTATUSCODE) Then
                        'ROが作成されている場合

                        'ROステータス取得
                        Dim roStatusCode As String = String.Empty

                        'RO番号が採番されているかチェック
                        If Not row.IsORDERNONull Then
                            '採番されている場合

                            'RO番号のROステータス（最小値）を取得する

                            '2014/09/16 TMEJ 小澤 BTS不具合対応 RO_INFO検索時は販売店と店舗も条件に入れるように修正 START

                            'roStatusCode = daSC3080103.GetROStatusCode(row.ORDERNO)
                            roStatusCode = daSC3080103.GetROStatusCode(dealerCode, branchCode, row.ORDERNO)

                            '2014/09/16 TMEJ 小澤 BTS不具合対応 RO_INFO検索時は販売店と店舗も条件に入れるように修正 END

                        End If

                        'ROステータスのチェック
                        If row.IsORDERNONull Or _
                           RO_SAIssuing.Equals(roStatusCode) Or _
                           RO_TCIssuing.Equals(roStatusCode) Or _
                           RO_WaitingForFMApproval.Equals(roStatusCode) Or _
                           RO_CreatingPartsRoughQuotation.Equals(roStatusCode) Or _
                           RO_CreatingPartsQuotation.Equals(roStatusCode) Or _
                           RO_WaitingForROConfirmation.Equals(roStatusCode) Or _
                           RO_WaitingForCustomerApproval.Equals(roStatusCode) Or _
                           RO_ApprovedByCustomer.Equals(roStatusCode) Then
                            'RO番号が存在しないまたは、ROステータスが
                            '「10:SA起票中」「15:TC起票中」「20:FM承認待ち」「25:部品仮見積中」
                            '「30:部品本見積中」「35:SA承認待ち」「40:顧客承認待ち」「50:着工指示待ち」の場合

                            Dim dateROCreateDate As Date = daSC3080103.GetROCreateDate(row.REZID)

                            'RO作成日のチェック
                            If dateROCreateDate.Date = nowDate.Date Or _
                              (dateROCreateDate.Date <> nowDate.Date And row.REZSTARTTIME.Date >= nowDate.Date) Then
                                '本日作成されたRO情報または、予定開始日時が本日移行の場合

                                'RO No.
                                If row.IsORDERNONull Then
                                    .RO_NUM = String.Empty
                                Else
                                    .RO_NUM = row.ORDERNO
                                End If

                                '予約ID（サービス入庫ID）
                                .SVCIN_ID = row.REZID

                                '予約開始日時
                                .START_DATETIME = row.REZSTARTTIME

                                '予約終了日時
                                .END_DATETIME = row.REZENDTIME

                                'サービス名称
                                If row.IsSERVICENAMENull Then
                                    .SERVICE_NAME = String.Empty
                                Else
                                    .SERVICE_NAME = row.SERVICENAME
                                End If

                                'RO作業連番
                                If row.IsORDERNONull Then
                                    .RO_JOB_SEQ = String.Empty
                                Else

                                    '2014/09/16 TMEJ 小澤 BTS不具合対応 RO_INFO検索時は販売店と店舗も条件に入れるように修正 START

                                    '.RO_JOB_SEQ = daSC3080103.GetROJobSeq(row.ORDERNO)
                                    .RO_JOB_SEQ = daSC3080103.GetROJobSeq(dealerCode, branchCode, row.ORDERNO)

                                    '2014/09/16 TMEJ 小澤 BTS不具合対応 RO_INFO検索時は販売店と店舗も条件に入れるように修正 END

                                End If

                                'DMS予約ID
                                If row.IsDMS_JOB_DTL_IDNull Then
                                    .DMS_JOB_DTL_ID = String.Empty
                                Else
                                    .DMS_JOB_DTL_ID = row.DMS_JOB_DTL_ID
                                End If

                                'ROの作成ステータス（ROステータスが10,15,20,25,30,35,40,50の場合　→　『作成中』とする）
                                .ROSTATUS = CreatedWord

                                '2017/01/06 NSK  竹中 TR-SVT-TMT-20160602-001 顧客がWelcomeボードに表示されない START
                                If row.IsCST_NAMENull Then
                                    .CST_NAME = String.Empty
                                Else
                                    .CST_NAME = row.CST_NAME
                                End If
                                '2017/01/06 NSK  竹中 TR-SVT-TMT-20160602-001 顧客がWelcomeボードに表示されない END
                            Else
                                Continue For
                            End If
                        Else
                            Continue For
                        End If

                    Else
                        Continue For
                    End If
                End With

                '行追加
                dtMod.Rows.Add(dr)
            Next

            dtRet = dtMod

            '終了ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                    , "{0}.{1} {2}" _
                                    , Me.GetType.ToString _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                    , LOG_END))
            Return dtRet
        Catch ex As OracleExceptionEx When ex.Number = 1013
            'ORACLEのタイムアウトのみ処理
            ''終了ログの出力
            Logger.Error(String.Format(CultureInfo.CurrentCulture _
                     , "{0}.{1} OUT:RETURNCODE = {2}" _
                     , Me.GetType.ToString _
                     , System.Reflection.MethodBase.GetCurrentMethod.Name _
                     , ReturnCode.ErrDBTimeout))
            Return Nothing
        Finally
            If bl IsNot Nothing Then
                bl.Dispose()
                bl = Nothing
            End If
        End Try
    End Function

    ''' <summary>
    ''' ストール利用情報取得
    ''' </summary>
    ''' <param name="inStallUseId">ストール利用ID</param>
    ''' <returns>ストール利用情報</returns>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Public Function GetStallUseInfo(ByVal inStallUseId As Decimal) As SC3080103DataSet.SC3080103StallUseInfoDataTable
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START IN:inStallUseId = {2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , inStallUseId.ToString(CultureInfo.CurrentCulture)))

        Using da As New SC3080103DataTableAdapter
            'ストール利用情報を取得
            Dim dt As SC3080103DataSet.SC3080103StallUseInfoDataTable = _
                da.GetStallUseInfo(inStallUseId)

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return dt
        End Using
    End Function

    ''' <summary>
    ''' 顧客情報取得
    ''' </summary>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inCustomerId">顧客ID</param>
    ''' <param name="inVehiceleId">車両ID</param>
    ''' <returns>顧客情報</returns>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Public Function GetCustomerInfo(ByVal inDealerCode As String, _
                                    ByVal inCustomerId As Decimal, _
                                    ByVal inVehiceleId As Decimal) As SC3080103DataSet.SC3080103CustomerInfoDataTable
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START IN:inDealerCode = {2}, inCustomerId = {3}, inVehiceleId = {4}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , inDealerCode, inCustomerId.ToString(CultureInfo.CurrentCulture) _
                    , inVehiceleId.ToString(CultureInfo.CurrentCulture)))

        Using da As New SC3080103DataTableAdapter
            '顧客情報取得
            Dim dtCustomerInfo As SC3080103DataSet.SC3080103CustomerInfoDataTable = _
                da.GetCustomerInfo(inDealerCode, _
                                   inCustomerId, _
                                   inVehiceleId)

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return dtCustomerInfo
        End Using
    End Function

    ''' <summary>
    ''' 基幹コードへ変換処理
    ''' 販売店コード・店舗コード・アカウントをそれぞれ
    ''' 基幹販売店コード・基幹店舗コード・基幹アカウントに変換
    ''' </summary>
    ''' <param name="inStaffInfo">スタッフ情報</param>
    ''' <remarks>基幹コード情報ROW</remarks>
    ''' <history>
    ''' </history>
    Public Function ChangeDmsCode(ByVal inStaffInfo As StaffContext) _
                                  As ServiceCommonClassDataSet.DmsCodeMapRow

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} Start IN:DLRCD = {2} STRCD = {3} ACCOUNT = {4} " _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , inStaffInfo.DlrCD, inStaffInfo.BrnCD, inStaffInfo.Account))

        'SMBCommonClassBusinessLogicのインスタンス
        Using smbCommon As New ServiceCommonClassBusinessLogic


            '基幹コードへ変換処理
            Dim dtDmsCodeMap As ServiceCommonClassDataSet.DmsCodeMapDataTable = _
                smbCommon.GetIcropToDmsCode(inStaffInfo.DlrCD, _
                                            ServiceCommonClassBusinessLogic.DmsCodeType.BranchCode, _
                                            inStaffInfo.DlrCD, _
                                            inStaffInfo.BrnCD, _
                                            String.Empty, _
                                            inStaffInfo.Account)

            '基幹コード情報Row
            Dim rowDmsCodeMap As ServiceCommonClassDataSet.DmsCodeMapRow

            '基幹コードへ変換処理結果チェック
            If dtDmsCodeMap IsNot Nothing AndAlso 0 < dtDmsCodeMap.Rows.Count Then
                '基幹コードへ変換処理成功

                'Rowに変換
                rowDmsCodeMap = CType(dtDmsCodeMap.Rows(0), ServiceCommonClassDataSet.DmsCodeMapRow)

                '基幹アカウントチェック
                If rowDmsCodeMap.IsACCOUNTNull Then
                    '値無し

                    '空文字を設定する
                    '基幹アカウント
                    rowDmsCodeMap.ACCOUNT = String.Empty

                End If

                '基幹販売店コードチェック
                If rowDmsCodeMap.IsCODE1Null Then
                    '値無し

                    '空文字を設定する
                    '基幹販売店コード
                    rowDmsCodeMap.CODE1 = String.Empty

                End If

                '基幹店舗コードチェック
                If rowDmsCodeMap.IsCODE2Null Then
                    '値無し

                    '空文字を設定する
                    '基幹店舗コード
                    rowDmsCodeMap.CODE2 = String.Empty

                End If

            Else
                '基幹コードへ変換処理成功失敗

                '新しいRowを作成
                rowDmsCodeMap = CType(dtDmsCodeMap.NewDmsCodeMapRow, ServiceCommonClassDataSet.DmsCodeMapRow)

                '空文字を設定する
                '基幹アカウント
                rowDmsCodeMap.ACCOUNT = String.Empty
                '基幹販売店コード
                rowDmsCodeMap.CODE1 = String.Empty
                '基幹店舗コード
                rowDmsCodeMap.CODE2 = String.Empty

            End If


            '終了ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} {2} dtDmsCodeMap:COUNT = {3}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , LOG_END _
                       , dtDmsCodeMap.Count))

            '結果返却
            Return rowDmsCodeMap

        End Using

    End Function

    '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 START

    ''' <summary>
    ''' 来店情報を取得
    ''' </summary>
    ''' <param name="inVisitiSequence">来店実績連番</param>
    ''' <returns>来店情報</returns>
    ''' <remarks></remarks>
    Public Function GetVisitManagmentInfo(ByVal inVisitiSequence As Long) As SC3080103DataSet.SC3080103VisitManagmentInfoDataTable
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START IN:inVisitiSequence = {2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , inVisitiSequence.ToString(CultureInfo.CurrentCulture)))

        '戻り値
        Dim dt As SC3080103DataSet.SC3080103VisitManagmentInfoDataTable

        Using da As New SC3080103DataTableAdapter
            '来店情報取得
            dt = da.GetVisitManagmentInfo(inVisitiSequence)

        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END OUT:COUNT = {2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , dt.Count.ToString(CultureInfo.CurrentCulture)))
        Return dt

    End Function

    '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 END

#End Region

#Region "XML送信用クラス作成"

    ''' <summary>
    ''' XML作成(メイン)
    ''' </summary>
    ''' <param name="rowIN">顧客検索条件</param>
    ''' <returns>XML送信用クラス</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Function CreateXml(ByVal rowIN As IC3800709DataSet.CustomerSearchRequestRow) As CustomerSearchXmlDocumentClass

        ''引数をログに出力
        Dim args As New List(Of String)

        ' DataRow内の項目を列挙
        Me.AddLogData(args, rowIN)

        ''開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} IN:{2}" _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name _
                    , String.Join(", ", args.ToArray())))

        'XMLクラスのインスタンスか
        Dim sendXml As New CustomerSearchXmlDocumentClass

        'XMLのHeadTagの作成処理
        sendXml = CreateHeadTag(sendXml, rowIN)

        'XMLのDetailTagの作成処理
        sendXml = CreateDetailTag(sendXml, rowIN)

        Return sendXml

    End Function

    ''' <summary>
    ''' XML作成(HeadTag)
    ''' </summary>
    ''' <param name="sendXml">顧客検索RequestXML</param>
    ''' <param name="rowIN">顧客検索条件</param>
    ''' <returns>XMLドキュメントクラス</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Function CreateHeadTag(ByVal sendXml As CustomerSearchXmlDocumentClass, _
                                   ByVal rowIN As IC3800709DataSet.CustomerSearchRequestRow) As CustomerSearchXmlDocumentClass

        ''引数をログに出力
        Dim args As New List(Of String)

        'DataRow内の項目を列挙
        Me.AddLogData(args, rowIN)

        ''開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} IN:{2}" _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name _
                    , String.Join(", ", args.ToArray())))

        'システム設定値を取得
        Dim systemSettingsValueRow As SystemSettingValueRow _
            = Me.GetSystemSettingValues()
        'システム設定値の取得でエラーがあった場合
        If IsNothing(systemSettingsValueRow) Then
            Return Nothing
        End If

        Dim transmissionDate As Date = DateTimeFunc.Now(rowIN.DealerCode)
        '送信日時
        sendXml.Head.TransmissionDate = String.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy HH:mm:ss}", transmissionDate)

        '終了ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} OUT:RETURNCODE = {2}" _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name _
                    , sendXml))

        Return sendXml


    End Function

    ''' <summary>
    ''' システム設定値を取得する
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetSystemSettingValues() As SystemSettingValueRow

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_Start", _
                                  MethodBase.GetCurrentMethod.Name))

        '戻り値
        Dim retRow As SystemSettingValueRow = Nothing

        'エラー発生フラグ
        Dim errorFlg As Boolean = False


        Try
            Using smbCommonBiz As New ServiceCommonClassBusinessLogic

                '******************************
                '* システム設定から取得
                '******************************

                '日付フォーマット
                Dim dateFormat As String _
                    = smbCommonBiz.GetSystemSettingValueBySettingName(SysDateFormat)

                If String.IsNullOrEmpty(dateFormat) Then
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                               "{0}.Error ErrCode:{1}, DATE_FORMAT does not exist.", _
                                               MethodBase.GetCurrentMethod.Name, _
                                               ErrorSysEnv))
                    errorFlg = True
                    Exit Try
                End If

                Using table As New SystemSettingValueDataTable

                    retRow = table.NewSystemSettingValueRow

                    With retRow
                        '取得した値を戻り値のデータ行に設定
                        .DATE_FORMAT = dateFormat
                    End With

                End Using

            End Using

        Finally

            If errorFlg Then
                retRow = Nothing
            End If

        End Try

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_End", _
                                  MethodBase.GetCurrentMethod.Name))

        Return retRow

    End Function

    ''' <summary>
    ''' XML作成(DetailTag)
    ''' </summary>
    ''' <param name="sendXml">XML Template</param>
    ''' <param name="rowIN">顧客検索条件</param>
    ''' <returns>XML作成</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Function CreateDetailTag(ByVal sendXml As CustomerSearchXmlDocumentClass, _
                                     ByVal rowIN As IC3800709DataSet.CustomerSearchRequestRow) As CustomerSearchXmlDocumentClass

        ''引数をログに出力
        Dim args As New List(Of String)

        'DataRow内の項目を列挙
        Me.AddLogData(args, rowIN)

        ''開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} IN:{2}" _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name _
                    , String.Join(", ", args.ToArray())))

        'XMLのCommonTagの作成処理
        sendXml = CreateCommonTag(sendXml, rowIN)

        'XMLのCreateCustomerTagの作成処理
        sendXml = CreateCustomerTag(sendXml, rowIN)

        '終了ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} OUT:RETURNCODE = {2}" _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name _
                    , sendXml))

        Return sendXml


    End Function

    ''' <summary>
    ''' XML作成(CommonTag)
    ''' </summary>
    ''' <param name="sendXml">顧客検索RequestXML</param>
    ''' <param name="rowIN">顧客検索条件</param>
    ''' <returns>XMLドキュメントクラス</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Function CreateCommonTag(ByVal sendXml As CustomerSearchXmlDocumentClass, _
                                     ByVal rowIN As IC3800709DataSet.CustomerSearchRequestRow) As CustomerSearchXmlDocumentClass

        ''引数をログに出力
        Dim args As New List(Of String)

        'DataRow内の項目を列挙
        Me.AddLogData(args, rowIN)

        ''開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} IN:{2}" _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name _
                    , String.Join(", ", args.ToArray())))

        '販売店コード
        sendXml.Detail.Common.DealerCode = rowIN.DealerCode

        '店舗コード
        sendXml.Detail.Common.BranchCode = rowIN.BranchCode

        'スタッフコード
        sendXml.Detail.Common.StaffCode = rowIN.StaffCode

        '終了ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} OUT:RETURNCODE = {2}" _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name _
                    , sendXml))

        Return sendXml


    End Function

    ''' <summary>
    ''' XML作成(ReserveInformationTag)
    ''' </summary>
    ''' <param name="sendXml">顧客検索RequestXML</param>
    ''' <param name="rowIN">顧客検索条件</param>
    ''' <returns>XMLドキュメントクラス</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Function CreateCustomerTag(ByVal sendXml As CustomerSearchXmlDocumentClass, _
                                                 ByVal rowIN As IC3800709DataSet.CustomerSearchRequestRow) As CustomerSearchXmlDocumentClass

        ''引数をログに出力
        Dim args As New List(Of String)

        'DataRow内の項目を列挙
        Me.AddLogData(args, rowIN)

        ''開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} IN:{2}" _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name _
                    , String.Join(", ", args.ToArray())))

        'Start
        sendXml.Detail.SearchCondition.Start = rowIN.Start

        'Count
        sendXml.Detail.SearchCondition.Count = rowIN.Count

        'Sort1
        sendXml.Detail.SearchCondition.Sort1 = rowIN.Sort1

        If Not (rowIN.Sort2 = String.Empty) Then
            'Sort2
            sendXml.Detail.SearchCondition.Sort2 = rowIN.Sort2
        End If

        If Not (rowIN.VclRegNo = String.Empty) Then
            'VclRegNo
            sendXml.Detail.SearchCondition.VclRegNo = rowIN.VclRegNo

            'VclRegNo_MatchType
            sendXml.Detail.SearchCondition.VclRegNo_MatchType = rowIN.VclRegNo_MatchType
        End If

        If Not (rowIN.CustomerName = String.Empty) Then
            'CustomerName
            sendXml.Detail.SearchCondition.CustomerName = rowIN.CustomerName

            'CustomerName_MatchType
            sendXml.Detail.SearchCondition.CustomerName_MatchType = rowIN.CustomerName_MatchType
        End If

        If Not (rowIN.Vin = String.Empty) Then
            'Vin
            sendXml.Detail.SearchCondition.Vin = rowIN.Vin

            'Vin_MatchType
            sendXml.Detail.SearchCondition.Vin_MatchType = rowIN.Vin_MatchType
        End If

        If Not (rowIN.BasRezid = String.Empty) Then
            'BasRezid
            sendXml.Detail.SearchCondition.BasRezid = rowIN.BasRezid

            'BasRezid_MatchType
            sendXml.Detail.SearchCondition.BasRezid_MatchType = rowIN.BasRezid_MatchType
        End If

        If Not (rowIN.R_O = String.Empty) Then
            'R_O
            sendXml.Detail.SearchCondition.R_O = rowIN.R_O

            'R_O_MatchType
            sendXml.Detail.SearchCondition.R_O_MatchType = rowIN.R_O_MatchType
        End If

        If Not (rowIN.TelNumber = String.Empty) Then
            'TelNumber
            sendXml.Detail.SearchCondition.TelNumber = rowIN.TelNumber

            'TelNumber_MatchType
            sendXml.Detail.SearchCondition.TelNumber_MatchType = rowIN.TelNumber_MatchType
        End If

        '終了ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} OUT:RETURNCODE = {2}" _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name _
                    , sendXml))

        Return sendXml


    End Function

#End Region

#Region "Push送信呼出"

    ''' <summary>
    ''' WelcomeBoardリフレッシュPush送信
    ''' </summary>
    ''' <param name="inStaffInfo">ログインスタッフ情報</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2017/03/22 NSK  秋田 TR-SVT-TMT-20151224-001 顧客一覧が遅れてwelcome boardに表示される
    ''' </history>
    Public Sub SendPushForRefreshWelcomeBoard(ByVal inStaffInfo As StaffContext)
        Logger.Debug("SendPushForRefreshWelcomeBoard_Start Pram[" & inStaffInfo.DlrCD & "," & inStaffInfo.BrnCD & inStaffInfo.Account & "]")

        'スタッフ情報の取得(WB)
        Dim stuffCodeList As New List(Of Decimal)
        stuffCodeList.Add(SystemFrameworks.Core.iCROP.BizLogic.Operation.WBS)

        '2017/03/22 NSK  秋田 TR-SVT-TMT-20151224-001 顧客一覧が遅れてwelcome boardに表示される START
        '全ユーザー情報の取得
        Dim utility As New VisitUtilityBusinessLogic
        'Dim sendPushUsers As VisitUtilityUsersDataTable = _
        'utility.GetOnlineUsers(inStaffInfo.DlrCD, inStaffInfo.BrnCD, stuffCodeList)
        Dim sendPushUsers As VisitUtilityUsersDataTable = _
            utility.GetUsers(inStaffInfo.DlrCD, inStaffInfo.BrnCD, stuffCodeList, Nothing, DeleteFlagNone)
        utility = Nothing
        '2017/03/22 NSK  秋田 TR-SVT-TMT-20151224-001 顧客一覧が遅れてwelcome boardに表示される END

        '来店通知命令の送信
        For Each userRow As VisitUtilityUsersRow In sendPushUsers

            '送信処理
            TransmissionWelcomeBoardRefresh(userRow.ACCOUNT, inStaffInfo.Account, inStaffInfo.DlrCD)
        Next
        Logger.Debug("SendPushForRefreshWelcomeBoard_End")
    End Sub

#End Region

#Region "WelcomeBoardへPUSH送信"
    ''' <summary>
    ''' WelcomeBoardへPUSH送信（受付待ちモニター画面再描画）
    ''' </summary>
    ''' <param name="staffCode">スタッフコード</param>
    ''' <remarks></remarks>
    Private Sub TransmissionWelcomeBoardRefresh(ByVal staffCode As String, _
                                                ByVal loginStaffCode As String, _
                                                ByVal loginDlrCd As String)
        Logger.Debug("TransmissionWelcomeBoardRefresh_Start Pram[" & staffCode & "," & loginStaffCode & "]")

        '送信処理
        Dim visitUtility As New Visit.Api.BizLogic.VisitUtility
        visitUtility.SendPushReconstructionPC(loginStaffCode, staffCode, "", loginDlrCd)

        Logger.Debug("TransmissionWelcomeBoardRefresh_End]")
    End Sub
#End Region

#Region "共通部品"

    ''' <summary>
    ''' DataRow内の項目を列挙(ログ出力用)
    ''' </summary>
    ''' <param name="args">ログ項目のコレクション</param>
    ''' <param name="row">対象となるDataRow</param>
    ''' <remarks></remarks>
    Private Sub AddLogData(ByVal args As List(Of String), ByVal row As DataRow)
        For Each column As DataColumn In row.Table.Columns
            If row.IsNull(column.ColumnName) = True Then
                args.Add(String.Format(CultureInfo.CurrentCulture, "{0} = NULL", column.ColumnName))
            Else
                args.Add(String.Format(CultureInfo.CurrentCulture, "{0} = {1}", column.ColumnName, row(column.ColumnName)))
            End If
        Next
    End Sub

    '2019/03/05 NSK 山田 18PRJ02750-00_(トライ店システム評価)サービス業務における次世代オペレーション実現の為の性能対策 START
    ''' <summary>
    ''' 車両登録番号検索ワード変換
    ''' </summary>
    ''' <param name="inSearchWord">検索ワード</param>
    ''' <param name="inRegNumDlmtr">区切り文字</param>
    ''' <returns>「*」と区切り文字を取り除いた検索ワード</returns>
    ''' <remarks></remarks>
    Private Function ConvertVclRegNumWord(ByVal inSearchWord As String, _
                                          ByVal inRegNumDlmtr As String) As String

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.{1}_S IN:inSearchWord={2},inRegNumDlmtr={3}", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  inSearchWord, _
                                  inRegNumDlmtr))

        '区切り文字が存在する場合
        If Not String.IsNullOrEmpty(inRegNumDlmtr) Then

            '文字間に入力された'*'を検索文字列より削除
            inSearchWord = inSearchWord.Replace("*", String.Empty)

            '取得された区切文字を'*'で分割
            Dim regNumDlmtrList As List(Of String) = inRegNumDlmtr.Split("*"c).ToList

            For Each dlmtr As String In regNumDlmtrList
                '区切り文字を削除
                inSearchWord = inSearchWord.Replace(dlmtr, String.Empty)
            Next

        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.{1}_E OUT:returnValue={2}", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  inSearchWord))

        Return inSearchWord

    End Function

    '2019/03/05 NSK 山田 18PRJ02750-00_(トライ店システム評価)サービス業務における次世代オペレーション実現の為の性能対策 END
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

