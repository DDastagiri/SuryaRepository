'===================================================================
' SC3100401BusinessLogic
'-------------------------------------------------------------------
' 機能：未振当て一覧画面 ビジネスロジック
' 補足：               
' 作成：2013/03/01 TMEJ 河原 
' 更新：2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
' 更新：2013/08/27 TMEJ 小澤 IT9559_タブレット版SMB機能開発(IF開発)
' 更新：2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発
' 更新：2014/05/20 TMEJ 小澤 IT9684_サービスタブレットゲートカメラ連携機能追加開発
' 更新：2015/09/10 TMEJ 春日井 IT9960_(トライ店システム評価)サービス入庫予約のユーザ管理機能開発
' 更新：2015/11/10 TM 小牟禮 (トライ店システム評価)SMBチップ検索の絞り込み方法変更
' 更新：2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応
' 更新：2016/10/19 NSK 中ノ瀬 TR-SVT-TMT-20160916-001 SVRのアカウント権限が使えない
' 更新：2017/01/23 NSK 加藤 TR-SVT-TMT-20161019-001 jobをオープンしたがチップのステータスが変更しない
' 更新：2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする
' 更新：
'===================================================================

Imports System.Text
Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.iCROP.DataAccess.SC3100401
Imports Toyota.eCRB.SMBLinkage.GetServiceLT.Api.BizLogic
Imports Toyota.eCRB.SMBLinkage.GetServiceLT.Api.DataAccess
Imports Toyota.eCRB.CommonUtility.SMBCommonClass.Api.BizLogic
Imports Toyota.eCRB.CommonUtility.SMBCommonClass.Api.BizLogic.SMBCommonClassBusinessLogic
Imports Toyota.eCRB.Tool.Notify.Api.BizLogic
Imports Toyota.eCRB.Tool.Notify.Api.DataAccess
Imports Toyota.eCRB.Tool.Notify.Api.DataAccess.ConstCode
'2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
'Imports Toyota.eCRB.DMSLinkage.SA.BizLogic.IC3802001
'Imports Toyota.eCRB.DMSLinkage.SA.DataAccess.IC3802001
'Imports Toyota.eCRB.DMSLinkage.SA.DataAccess.IC3802001.IC3802001DataSet
'2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END
Imports Toyota.eCRB.Visit.Api.BizLogic
'2013/08/27 TMEJ 小澤 IT9559_タブレット版SMB機能開発(IF開発) START
Imports Toyota.eCRB.SMBLinkage.GetUserList.Api.BizLogic
Imports Toyota.eCRB.SMBLinkage.GetUserList.Api.DataAccess
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.BizLogic

'2013/08/27 TMEJ 小澤 IT9559_タブレット版SMB機能開発(IF開発) END

'2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 START
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.DataAccess
'2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 END

Imports Toyota.eCRB.Visit.Api.DataAccess.VisitUtilityDataSetTableAdapters
Imports Toyota.eCRB.Visit.Api.DataAccess
Imports Toyota.eCRB.Visit.Api.DataAccess.VisitUtilityDataSet


Public Class SC3100401BusinessLogic
    Inherits BaseBusinessComponent
    Implements IDisposable

#Region "定数"

    ''' <summary>
    ''' アプリケーションID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ApplicationID As String = "SC3100401"

    ''' <summary>
    ''' 販売店環境マスタ.パラメータ名:変換フォーマット
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VclRegNoChangeFormat As String = "VCLREGNO_CHANGE_FORMAT"

    ''' <summary>
    ''' 販売店環境マスタ.パラメータ名:変換当て込み文字
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VclRegNoChangeString As String = "VCLREGNO_CHANGE_STRING"

    ''' <summary>
    ''' 自社客
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CustSegmentMyCustomer As String = "1"

    ''' <summary>
    ''' 未取引客
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CustSegmentNewCustomer As String = "2"

    ''' <summary>
    ''' 実績ステータス:未入庫
    ''' </summary>
    ''' <remarks></remarks>
    Public Const StatusNoReceiving As String = "00"

    ''' <summary>
    ''' 予約無効
    ''' </summary>
    ''' <remarks></remarks>
    Public Const UnavailableReserve As Long = -1

    ''' <summary>
    ''' 事前準備フラグ(本R/O)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RepairOrder As String = "0"

    ''' <summary>
    ''' 事前準備フラグ(仮R/O)
    ''' </summary>
    Private Const PrepareFlag As String = "1"

    ''' <summary>
    ''' サービスコード(定期)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ServiceCodeTeiki As String = "20"

    ''' <summary>
    ''' サービスコード(一般)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ServiceCodeIppan As String = "30"

    ''' <summary>
    ''' 削除されていないユーザ(delflg=0)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DelFlgNone As String = "0"

    '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START

    ''' <summary>
    ''' SAリフレッシュエリア(0：全体)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SARefreshTypeAll As String = "0"

    ''' <summary>
    ''' SAリフレッシュエリア(1：未振当てエリア)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SARefreshTypeAssignment As String = "1"

    '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

    '2015/09/10 TMEJ 春日井 IT9960_(トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START

    ''' <summary>
    ''' 顧客車両区分（1：所有者）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CustomerVehicleTypeOwner As String = "1"

    '2015/09/10 TMEJ 春日井 IT9960_(トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END

    '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 START
    ''' <summary>
    ''' 実績フラグ(実績チップを含む全てのチップ)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ResultsFlgOn As Long = 1

    ''' <summary>
    ''' キャンセルフラグ(キャンセルチップを含まない)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CancelFlgOff As Long = 0
    '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 END
#End Region

#Region "Enum"

    ''' <summary>
    ''' イベントキーID
    ''' </summary>
    Private Enum EventKeyId

        ''' <summary>
        ''' 呼出ボタン
        ''' </summary>
        FooterCallButton = 100

        '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START

        ''' <summary>
        ''' 呼出キャンセルボタン
        ''' </summary>
        FooterCancelButton = 200

        ''' <summary>
        ''' チップ削除ボタン
        ''' </summary>
        FooterDeleteButton = 300

        ''' <summary>
        ''' 発券番号テキスト
        ''' </summary>
        ReceiptNoText = 1100

        '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        ''' <summary>
        ''' SA振当
        ''' </summary>
        SAAssig = 3000
        ''' <summary>
        ''' SA変更
        ''' </summary>
        SAChange = 3100
        ''' <summary>
        ''' SA解除
        ''' </summary>
        SAUndo = 3200

    End Enum

    ''' <summary>
    ''' リターンコード
    ''' </summary>
    Private Enum ResultCode

        ''' <summary>
        ''' 成功
        ''' </summary>
        Success = 0

        ''' <summary>
        ''' DBタイムアウト
        ''' </summary>
        ErrDBTimeout = 901

        ''' <summary>
        ''' DBエラー
        ''' </summary>
        ErrDB = 902

        ''' <summary>
        ''' 排他エラー
        ''' </summary>
        ErrExclusion = 903

        ''' <summary>
        ''' 呼出処理エラー
        ''' </summary>
        ErrCall = 918

        ''' <summary>
        ''' 呼出キャンセル処理エラー
        ''' </summary>
        ErrCallCancel = 920

        ''' <summary>
        ''' チップ削除処理エラー
        ''' </summary>
        ErrTipDelete = 921

        ''' <summary>
        ''' 予期せぬエラー
        ''' </summary>
        ErrOutType = 922

    End Enum

    ''' <summary>
    ''' 文言ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Enum WordId

        ''' <summary>
        ''' 件
        ''' </summary>
        ''' <remarks></remarks>
        Id024 = 24

        ''' <summary>
        ''' ～
        ''' </summary>
        ''' <remarks></remarks>
        Id035 = 35

        ''' <summary>
        ''' 一般整備
        ''' </summary>
        ''' <remarks></remarks>
        Id037 = 37

        ''' <summary>
        ''' 定期点検
        ''' </summary>
        ''' <remarks></remarks>
        Id038 = 38

        ''' <summary>
        ''' 様
        ''' </summary>
        ''' <remarks></remarks>
        Id039 = 39

        ''' <summary>
        ''' お客様
        ''' </summary>
        ''' <remarks></remarks>
        Id040 = 40

        ''' <summary>
        ''' ご来店
        ''' </summary>
        ''' <remarks></remarks>
        Id041 = 41

        ''' <summary>
        ''' ご来店キャンセル
        ''' </summary>
        ''' <remarks></remarks>
        Id042 = 42

    End Enum

#End Region

#Region "来店一覧取得"

    ''' <summary>
    ''' 来店一覧情報取得
    ''' </summary>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inStoreCode">店舗コード</param>
    ''' <param name="inPresentTime">現在日付</param>
    ''' <returns>SA一覧情報</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Public Function GetReceptionInfo(ByVal inDealerCode As String, _
                                     ByVal inStoreCode As String, _
                                     ByVal inPresentTime As Date, _
                                     ByVal inSC3100401DataSet As SC3100401DataSetTableAdapters.SC3100401TableAdapter) _
                                     As SC3100401DataSet.ReceptionListDataTable

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} DEALERCODE:{2} STORECODE:{3} PRESENTTIME:{4}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , inDealerCode, inStoreCode, inPresentTime))

        '来店情報の取得
        Dim dtReceptionList As SC3100401DataSet.ReceptionListDataTable = _
            inSC3100401DataSet.GetDBReceptionList(inDealerCode, inStoreCode, inPresentTime)


        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END COUNT = {2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , dtReceptionList.Count))

        Return dtReceptionList

    End Function

#End Region

#Region "SA一覧取得"

#Region "Publicメソッド"

    ''' <summary>
    ''' SA一覧情報取得
    ''' </summary>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inStoreCode">店舗コード</param>
    ''' <param name="inPresentTime">現在日付</param>
    ''' <returns>SA一覧情報</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発
    ''' 2016/10/19 NSK 中ノ瀬 TR-SVT-TMT-20160916-001 SVRのアカウント権限が使えない
    ''' </history>
    Public Function GetServiceAdvisorInfo(ByVal inDealerCode As String, _
                                          ByVal inStoreCode As String, _
                                          ByVal inPresentTime As Date, _
                                          ByVal inSC3100401DataSet As SC3100401DataSetTableAdapters.SC3100401TableAdapter) _
                                          As SC3100401DataSet.ServiceAdviserListDataTable

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} DEALERCODE:{2} STORECODE:{3} PRESENTTIME:{4}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , inDealerCode, inStoreCode, inPresentTime))

        '表示用DateTable
        Dim dtServiceAdviserList As New SC3100401DataSet.ServiceAdviserListDataTable

        'SA情報の取得
        Dim dtSAInfo As SC3100401DataSet.ServiceAdviserInfoDataTable = _
            inSC3100401DataSet.GetDBServiceAdvisorList(inDealerCode, inStoreCode, inPresentTime)

        '取得結果確認
        If dtSAInfo IsNot Nothing _
            AndAlso 0 < dtSAInfo.Count Then
            '取得成功

            '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
            ''@マーク無しのSAリスト
            'Dim saList As List(Of String)

            ''@マークを向いてSAコードのみのリストを作成
            'saList = (From item In dtSAInfo Select item.ACCOUNT.ToString.Replace("@" & inDealerCode, "")).ToList()

            ''SA負荷情報テーブル
            'Dim dtSALoadList As IC3802001DataSet.SALoadListDataTable

            ''SA負荷情報取得
            'dtSALoadList = Me.GetServiceAdvisorLoad(inDealerCode, inStoreCode, saList)

            '2016/10/19 NSK 中ノ瀬 TR-SVT-TMT-20160916-001 SVRのアカウント権限が使えない START

            'SA負荷情報取得()
            'Dim dtSALoadList As SC3100401DataSet.AssumingInfoDataTable = _
            '    inSC3100401DataSet.GetServiceAdviserAssumingInfo(inDealerCode, _
            '                                                     inStoreCode, _
            '                                                     inPresentTime)

            '追加承認工程台数情報
            Dim dtAddWorkProcessList As SC3100401DataSet.AddWorkProcessInfoDataTable = _
                inSC3100401DataSet.GetAddWorkProcess(inDealerCode, inStoreCode)


            '納車工程台数情報
            Dim dtDeliveryProcessList As SC3100401DataSet.DeliveryProcessInfoDataTable = _
                inSC3100401DataSet.GetDeliveryProcess(inDealerCode, inStoreCode)

            '本日納車予定台数情報
            Dim dtTodayDeliveryPlanList As SC3100401DataSet.TodayDeliveryPlanInfoDataTable = _
                inSC3100401DataSet.GetTodayDeliveryPlan(inDealerCode, inStoreCode, inPresentTime)

            '2016/10/19 NSK 中ノ瀬 TR-SVT-TMT-20160916-001 SVRのアカウント権限が使えない END

            '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

            '標準時間データRow
            Dim rowStandardLTList As IC3810701DataSet.StandardLTListRow

            '標準時間取得
            rowStandardLTList = Me.GetGetStandardLT(inDealerCode, inStoreCode)

            '2016/10/19 NSK 中ノ瀬 TR-SVT-TMT-20160916-001 SVRのアカウント権限が使えない START
            ''表示用SA一覧の作成
            'dtServiceAdviserList = Me.CreatServiceAdvisorList(dtSAInfo, dtSALoadList, rowStandardLTList, inPresentTime)

            '表示用SA一覧の作成
            dtServiceAdviserList = Me.CreatServiceAdvisorList(dtSAInfo, dtAddWorkProcessList, dtDeliveryProcessList, dtTodayDeliveryPlanList, rowStandardLTList, inPresentTime)
            '2016/10/19 NSK 中ノ瀬 TR-SVT-TMT-20160916-001 SVRのアカウント権限が使えない END

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END COUNT = {2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , dtServiceAdviserList.Count))

        Return dtServiceAdviserList

    End Function

#End Region

#Region "Privateメソッド"

    '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START

    ' ''' <summary>
    ' ''' SA負荷情報取得
    ' ''' </summary>
    ' ''' <param name="inDealerCode">販売店コード</param>
    ' ''' <param name="inStoreCode">店舗コード</param>
    ' ''' <param name="inSAList">SAアカウント一覧(@なし)</param>
    ' ''' <returns>SA一覧情報</returns>
    ' ''' <remarks></remarks>
    ' ''' <history>
    ' ''' </history>
    'Private Function GetServiceAdvisorLoad(ByVal inDealerCode As String, _
    '                                       ByVal inStoreCode As String, _
    '                                       ByVal inSAList As List(Of String)) _
    '                                       As IC3802001DataSet.SALoadListDataTable

    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                , "{0}.{1} DEALERCODE:{2} STORECODE:{3} SALIST:{4}" _
    '                , Me.GetType.ToString _
    '                , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                , inDealerCode, inStoreCode, inSAList))

    '    'BMTSAPI(SA負荷取得)
    '    Dim ic3802001Biz As New IC3802001BusinessLogic

    '    'SA負荷情報テーブル
    '    Dim dtSALoadList As IC3802001DataSet.SALoadListDataTable = _
    '        ic3802001Biz.GetSALoadList(inDealerCode, inStoreCode, inSAList)

    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                , "{0}.{1} END" _
    '                , Me.GetType.ToString _
    '                , System.Reflection.MethodBase.GetCurrentMethod.Name))

    '    Return dtSALoadList

    'End Function

    '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

    ''' <summary>
    ''' 作業標準時間取得
    ''' </summary>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inStoreCode">店舗コード</param>
    ''' <returns>SA一覧情報</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Function GetGetStandardLT(ByVal inDealerCode As String, _
                                      ByVal inStoreCode As String) _
                                      As IC3810701DataSet.StandardLTListRow

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} DEALERCODE:{2} STORECODE:{3}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , inDealerCode, inStoreCode))

        '標準時間取得API
        Using ic3810701Biz As New IC3810701BusinessLogic

            '標準時間取得
            Dim dtStandardLTList As IC3810701DataSet.StandardLTListDataTable = _
                ic3810701Biz.GetStandardLTList(inDealerCode, inStoreCode)

            '標準時間データRow
            Dim rowStandardLTList As IC3810701DataSet.StandardLTListRow

            '取得チェック
            If 0 < dtStandardLTList.Count Then
                '取得成功

                'ROWに変換
                rowStandardLTList = DirectCast(dtStandardLTList.Rows(0), IC3810701DataSet.StandardLTListRow)
            Else
                '取得失敗

                '新しい行
                rowStandardLTList = dtStandardLTList.NewStandardLTListRow

                'デフォルト設定
                rowStandardLTList.RECEPT_STANDARD_LT = 0
                rowStandardLTList.ADDWORK_STANDARD_LT = 0
                rowStandardLTList.DELIVERYPRE_STANDARD_LT = 0
                rowStandardLTList.DELIVERYWR_STANDARD_LT = 0
                rowStandardLTList.PARTS_STANDARD_LT = 0
                rowStandardLTList.WASHTIME = 0

            End If

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))

            Return rowStandardLTList

        End Using
    End Function

    '2016/10/19 NSK 中ノ瀬 TR-SVT-TMT-20160916-001 SVRのアカウント権限が使えない START
    ' ''' <summary>
    ' ''' SA負荷情報の作成
    ' ''' </summary>
    ' ''' <param name="inDtSAInfo">SA一覧情報</param>
    ' ''' <param name="inDtSALoadList">SA負荷情報</param>
    ' ''' <param name="inRowStandardLTList">標準時間情報</param>
    ' ''' <param name="inPresentTime">現在日付</param>
    ' ''' <returns>SA一覧情報</returns>
    ' ''' <remarks></remarks>
    ' ''' <history>
    ' ''' </history>
    'Private Function CreatServiceAdvisorList(ByVal inDtSAInfo As SC3100401DataSet.ServiceAdviserInfoDataTable, _
    '                                         ByVal inDtSALoadList As SC3100401DataSet.AssumingInfoDataTable, _
    '                                         ByVal inRowStandardLTList As IC3810701DataSet.StandardLTListRow, _
    '                                         ByVal inPresentTime As Date) _
    '                                         As SC3100401DataSet.ServiceAdviserListDataTable
    '    '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
    '    'Private Function CreatServiceAdvisorList(ByVal inDtSAInfo As SC3100401DataSet.ServiceAdviserInfoDataTable, _
    '    '                                         ByVal inDtSALoadList As IC3802001DataSet.SALoadListDataTable, _
    '    '                                         ByVal inRowStandardLTList As IC3810701DataSet.StandardLTListRow, _
    '    '                                         ByVal inPresentTime As Date) _
    '    '                                         As SC3100401DataSet.ServiceAdviserListDataTable
    '    '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                , "{0}.{1} START" _
    '                , Me.GetType.ToString _
    '                , System.Reflection.MethodBase.GetCurrentMethod.Name))

    '    'データ整形用テーブル
    '    Using dtSAList As New SC3100401DataSet.ServiceAdviserListDataTable

    '        '返却用テーブル
    '        Dim dtServiceAdviserList As New SC3100401DataSet.ServiceAdviserListDataTable

    '        '画面出力用データ格納テーブル
    '        Dim rowSAList As SC3100401DataSet.ServiceAdviserListRow

    '        '受付待ちの文言「件」の取得
    '        Dim word024 As String = WebWordUtility.GetWord(ApplicationID, WordId.Id024)

    '        '受付可能の文言「～」の取得
    '        Dim word035 As String = WebWordUtility.GetWord(ApplicationID, WordId.Id035)

    '        '取得してきた情報を画面表示用に編集し格納
    '        For Each inRowSAInfo As SC3100401DataSet.ServiceAdviserInfoRow In inDtSAInfo.Rows

    '            '新しい行の作成
    '            rowSAList = dtSAList.NewServiceAdviserListRow

    '            rowSAList.ROWNO = inRowSAInfo.ROWNO                                                         'ROWNO
    '            rowSAList.ACCOUNT = inRowSAInfo.ACCOUNT                                                     'SACODAE
    '            rowSAList.USERNAME = inRowSAInfo.USERNAME                                                   'SA名前
    '            rowSAList.PRESENCECATEGORY = inRowSAInfo.PRESENCECATEGORY                                   'カテゴリー
    '            rowSAList.SALOADTIME = (inRowSAInfo.WORKFRONTCARS * inRowStandardLTList.RECEPT_STANDARD_LT) 'SA負荷時間
    '            rowSAList.SALOADCOUNT = inRowSAInfo.WORKFRONTCARS                                           'SA受付(未着工)台数

    '            'SA受付(仕掛中)台数
    '            Dim loadCount As Long = 0

    '            '該当SAの負荷情報が存在するかチェック
    '            If inDtSALoadList IsNot Nothing _
    '                AndAlso 0 < (From rowItem In inDtSALoadList Where rowItem.SACODE = rowSAList.ACCOUNT Select rowItem).Count Then
    '                'SA負荷有り

    '                'selectした結果の先頭行の取得
    '                '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
    '                'Dim rowSALoadList As IC3802001DataSet.SALoadListRow = _
    '                '    (From itemdd In inDtSALoadList Where itemdd.SACODE = rowSAList.ACCOUNT Select itemdd).Min
    '                Dim rowSALoadList As SC3100401DataSet.AssumingInfoRow = _
    '                    (From itemdd In inDtSALoadList Where itemdd.SACODE = rowSAList.ACCOUNT Select itemdd).Min
    '                '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

    '                '負荷時間の計算
    '                rowSAList.SALOADTIME += ((rowSALoadList.RECEPT_PROCESS_COUNT * inRowStandardLTList.RECEPT_STANDARD_LT) _
    '                                          + (rowSALoadList.ADDWORK_PROCESS_COUNT * inRowStandardLTList.ADDWORK_STANDARD_LT) _
    '                                          + (rowSALoadList.DELIVERYPRE_PROCESS_COUNT * inRowStandardLTList.DELIVERYPRE_STANDARD_LT) _
    '                                          + (rowSALoadList.DELIVERYWR_PROCESS_COUNT * inRowStandardLTList.DELIVERYWR_STANDARD_LT))

    '                '負荷台数の計算(ソート用)
    '                rowSAList.SALOADCOUNT += (rowSALoadList.RECEPT_PROCESS_COUNT _
    '                                           + rowSALoadList.ADDWORK_PROCESS_COUNT _
    '                                           + rowSALoadList.DELIVERYPRE_PROCESS_COUNT _
    '                                           + rowSALoadList.DELIVERYWR_PROCESS_COUNT)

    '                '納車予定台数(ソート用)
    '                rowSAList.DELIVERYCARS = rowSALoadList.TODAY_DELIVERY_PLAN_COUNT

    '                'SA受付(仕掛中)台数の設定
    '                loadCount = rowSALoadList.RECEPT_PROCESS_COUNT

    '            End If

    '            '表示用負荷時間
    '            Dim loadTimeBuilder As New StringBuilder

    '            '時間を設定
    '            loadTimeBuilder.Append(CType(DateTimeFunc.FormatDate(14, inPresentTime.AddMinutes(rowSAList.SALOADTIME)), String))

    '            '「～」を設定
    '            loadTimeBuilder.Append(word035)

    '            '表示用負荷時間(STRING)を格納
    '            rowSAList.DISPLOADTIME = loadTimeBuilder.ToString


    '            '表示用受付数台数
    '            Dim loadCountBuilder As New StringBuilder

    '            '受付台数を設定
    '            loadCountBuilder.Append(CType((inRowSAInfo.WORKFRONTCARS + loadCount), String))

    '            '「件」を設定
    '            loadCountBuilder.Append(word024)

    '            '表示用受付数台数(STRING)を格納
    '            rowSAList.DISPLOADCOUNT = loadCountBuilder.ToString

    '            'テーブルに追加
    '            dtSAList.AddServiceAdviserListRow(rowSAList)

    '            '後処理
    '            loadTimeBuilder = Nothing
    '            loadCountBuilder = Nothing

    '        Next

    '        'ソート処理
    '        Dim sortList As New List(Of SC3100401DataSet.ServiceAdviserListRow)
    '        'ソート順（ログイン状況・SA負荷時間・SA負荷台数・本日中に納車予定のチップ数）
    '        sortList = (From item In dtSAList.AsEnumerable _
    '                              Order By item.PRESENCECATEGORY Ascending, _
    '                                       item.SALOADTIME Ascending, _
    '                                       item.SALOADCOUNT Ascending, _
    '                                       item.DELIVERYCARS Ascending _
    '                                Select item).ToList

    '        '変更設定
    '        dtServiceAdviserList.AcceptChanges()

    '        '表示用テーブルにインポート
    '        For Each listRow As SC3100401DataSet.ServiceAdviserListRow In sortList

    '            'インポート
    '            dtServiceAdviserList.ImportRow(listRow)

    '        Next

    '        '変更内容をコミット
    '        dtServiceAdviserList.AcceptChanges()

    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                    , "{0}.{1} END COUNT = {2}" _
    '                    , Me.GetType.ToString _
    '                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                    , dtServiceAdviserList.Count))

    '        Return dtServiceAdviserList
    '    End Using

    'End Function
    '2016/10/19 NSK 中ノ瀬 TR-SVT-TMT-20160916-001 SVRのアカウント権限が使えない END

    '2016/10/19 NSK 中ノ瀬 TR-SVT-TMT-20160916-001 SVRのアカウント権限が使えない START
    ''' <summary>
    ''' SA負荷情報の作成
    ''' </summary>
    ''' <param name="inDtSAInfo">SA一覧情報</param>
    ''' <param name="inDtAddWorkProcessList">追加承認工程台数情報</param>
    ''' <param name="inDtDeliveryProcessList">納車工程台数情報</param>
    ''' <param name="inDtTodayDeliveryPlanList">本日納車予定台数情報</param>
    ''' <param name="inRowStandardLTList">標準時間情報</param>
    ''' <param name="inPresentTime">現在日付</param>
    ''' <returns>SA一覧情報</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2016/10/19 NSK 中ノ瀬 TR-SVT-TMT-20160916-001 SVRのアカウント権限が使えない
    ''' </history>
    Private Function CreatServiceAdvisorList(ByVal inDtSAInfo As SC3100401DataSet.ServiceAdviserInfoDataTable, _
                                             ByVal inDtAddWorkProcessList As SC3100401DataSet.AddWorkProcessInfoDataTable, _
                                             ByVal inDtDeliveryProcessList As SC3100401DataSet.DeliveryProcessInfoDataTable, _
                                             ByVal inDtTodayDeliveryPlanList As SC3100401DataSet.TodayDeliveryPlanInfoDataTable, _
                                             ByVal inRowStandardLTList As IC3810701DataSet.StandardLTListRow, _
                                             ByVal inPresentTime As Date) _
                                             As SC3100401DataSet.ServiceAdviserListDataTable

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'データ整形用テーブル
        Using dtSAList As New SC3100401DataSet.ServiceAdviserListDataTable

            '返却用テーブル
            Dim dtServiceAdviserList As New SC3100401DataSet.ServiceAdviserListDataTable

            '画面出力用データ格納テーブル
            Dim rowSAList As SC3100401DataSet.ServiceAdviserListRow

            '受付待ちの文言「件」の取得
            Dim word024 As String = WebWordUtility.GetWord(ApplicationID, WordId.Id024)

            '受付可能の文言「～」の取得
            Dim word035 As String = WebWordUtility.GetWord(ApplicationID, WordId.Id035)


            '取得してきた情報を画面表示用に編集し格納
            For Each inRowSAInfo As SC3100401DataSet.ServiceAdviserInfoRow In inDtSAInfo.Rows

                '新しい行の作成
                rowSAList = dtSAList.NewServiceAdviserListRow

                rowSAList.ROWNO = inRowSAInfo.ROWNO                                                                 'ROWNO
                rowSAList.ACCOUNT = inRowSAInfo.ACCOUNT                                                             'SACODAE
                rowSAList.USERNAME = inRowSAInfo.USERNAME                                                           'SA名前
                rowSAList.PRESENCECATEGORY = inRowSAInfo.PRESENCECATEGORY                                           'カテゴリー
                rowSAList.SALOADTIME = ((inRowSAInfo.COUNT_WORKBEF * inRowStandardLTList.RECEPT_STANDARD_LT) _
                                        + (inRowSAInfo.COUNT_WORK * inRowStandardLTList.RECEPT_STANDARD_LT))        'SA負荷時間
                rowSAList.SALOADCOUNT = (inRowSAInfo.COUNT_WORKBEF + inRowSAInfo.COUNT_WORK)                        'SA受付(未着工)台数+SA受付(仕掛中)台数

                Dim rowAddWorkProcess As SC3100401DataSet.AddWorkProcessInfoRow = _
                    (From itemdd In inDtAddWorkProcessList Where itemdd.SACODE = rowSAList.ACCOUNT Select itemdd).Min

                '追加承認工程台数が存在するかチェック
                If rowAddWorkProcess IsNot Nothing Then

                    '負荷時間の計算
                    rowSAList.SALOADTIME += rowAddWorkProcess.ADDWORK_PROCESS_COUNT * inRowStandardLTList.ADDWORK_STANDARD_LT

                    '負荷台数の計算(ソート用)
                    rowSAList.SALOADCOUNT += rowAddWorkProcess.ADDWORK_PROCESS_COUNT

                End If

                Dim rowDeliveryProcess As SC3100401DataSet.DeliveryProcessInfoRow = _
                    (From itemdd In inDtDeliveryProcessList Where itemdd.SACODE = rowSAList.ACCOUNT Select itemdd).Min

                '納車工程台数が存在するかチェック
                If rowDeliveryProcess IsNot Nothing Then

                    '負荷時間の計算
                    rowSAList.SALOADTIME += ((rowDeliveryProcess.DELIVERYPRE_PROCESS_COUNT * inRowStandardLTList.DELIVERYPRE_STANDARD_LT) _
                                             + (rowDeliveryProcess.DELIVERYWR_PROCESS_COUNT * inRowStandardLTList.DELIVERYWR_STANDARD_LT))

                    '負荷台数の計算(ソート用)
                    rowSAList.SALOADCOUNT += (rowDeliveryProcess.DELIVERYPRE_PROCESS_COUNT _
                                              + rowDeliveryProcess.DELIVERYWR_PROCESS_COUNT)

                End If

                Dim rowTodayDeliveryPlan As SC3100401DataSet.TodayDeliveryPlanInfoRow = _
                    (From itemdd In inDtTodayDeliveryPlanList Where itemdd.PIC_SA_STF_CD = rowSAList.ACCOUNT Select itemdd).Min

                '本日納車予定台数が存在するかチェック
                If rowTodayDeliveryPlan IsNot Nothing Then

                    '納車予定台数(ソート用)
                    rowSAList.DELIVERYCARS = rowTodayDeliveryPlan.TODAY_DELIVERY_PLAN_COUNT

                End If

                '表示用負荷時間
                Dim loadTimeBuilder As New StringBuilder

                '時間を設定
                loadTimeBuilder.Append(CType(DateTimeFunc.FormatDate(14, inPresentTime.AddMinutes(rowSAList.SALOADTIME)), String))

                '「～」を設定
                loadTimeBuilder.Append(word035)

                '表示用負荷時間(STRING)を格納
                rowSAList.DISPLOADTIME = loadTimeBuilder.ToString

                '表示用受付数台数
                Dim loadCountBuilder As New StringBuilder

                '受付台数を設定
                loadCountBuilder.Append(CType((inRowSAInfo.COUNT_WORKBEF + inRowSAInfo.COUNT_WORK), String))

                '「件」を設定
                loadCountBuilder.Append(word024)

                '表示用受付数台数(STRING)を格納
                rowSAList.DISPLOADCOUNT = loadCountBuilder.ToString

                'テーブルに追加
                dtSAList.AddServiceAdviserListRow(rowSAList)

                '後処理
                loadTimeBuilder = Nothing
                loadCountBuilder = Nothing

            Next


            'ソート処理
            Dim sortList As New List(Of SC3100401DataSet.ServiceAdviserListRow)
            'ソート順（ログイン状況・SA負荷時間・SA負荷台数・本日中に納車予定のチップ数）
            sortList = (From item In dtSAList.AsEnumerable _
                                  Order By item.PRESENCECATEGORY Ascending, _
                                           item.SALOADTIME Ascending, _
                                           item.SALOADCOUNT Ascending, _
                                           item.DELIVERYCARS Ascending _
                                    Select item).ToList

            '変更設定
            dtServiceAdviserList.AcceptChanges()

            '表示用テーブルにインポート
            For Each listRow As SC3100401DataSet.ServiceAdviserListRow In sortList

                'インポート
                dtServiceAdviserList.ImportRow(listRow)

            Next

            '変更内容をコミット
            dtServiceAdviserList.AcceptChanges()

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END COUNT = {2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , dtServiceAdviserList.Count))

            Return dtServiceAdviserList
        End Using

    End Function
    '2016/10/19 NSK 中ノ瀬 TR-SVT-TMT-20160916-001 SVRのアカウント権限が使えない END

#End Region

#End Region

#Region "車両登録No登録処理"

#Region "Publicメソッド"

    '2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする START

    ' ''' <summary>
    ' ''' 車両登録No登録処理
    ' ''' </summary>
    ' ''' <param name="invisitSeq">来店実績連番</param>
    ' ''' <param name="inupDateTime">更新日時</param>
    ' ''' <param name="inRegNo">車両登録No</param>
    ' ''' <param name="inStaffInfo">ログイン情報</param>
    ' ''' <param name="inPresentTime">現在日時</param>
    ' ''' <returns>処理結果</returns>
    ' ''' <remarks></remarks>
    ' ''' <history>
    ' ''' 2014/05/20 TMEJ 小澤 IT9684_サービスタブレットゲートカメラ連携機能追加開発
    ' ''' 2015/09/10 TMEJ 春日井 IT9960_(トライ店システム評価)サービス入庫予約のユーザ管理機能開発
    ' ''' </history>
    'Public Function RegisterRegNo(ByVal inVisitSeq As Long, _
    '                              ByVal inUpDateTime As Date, _
    '                              ByVal inRegNo As String, _
    '                              ByVal inStaffInfo As StaffContext, _
    '                              ByVal inPresentTime As Date) _
    '                              As Integer

    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                , "{0}.{1} VISITSEQ:{2} UPDATETIME:{3} REGNO:{4} ACCOUNT:{5} PRESENTTIME:{6}" _
    '                , Me.GetType.ToString _
    '                , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                , inVisitSeq, inUpDateTime, inRegNo, inStaffInfo.Account, inPresentTime))
    '    '処理結果
    '    Dim returnCode As Integer = ResultCode.Success

    '    '2015/11/10 TM 小牟禮 (トライ店システム評価)SMBチップ検索の絞り込み方法変更 START
    '    Using serviceCommonClass As New ServiceCommonClassBusinessLogic
    '        '2015/11/10 TM 小牟禮 (トライ店システム評価)SMBチップ検索の絞り込み方法変更 START

    '        Try

    '            'SYSTEMのフォーマット変換後車両登録Noの取得
    '            '2014/05/20 TMEJ 小澤 IT9684_サービスタブレットゲートカメラ連携機能追加開発 START
    '            'Dim changeRegNo As String = Me.GetChangeRegNo(inRegNo, inStaffInfo.DlrCD, inStaffInfo.BrnCD)

    '            '2015/11/10 TM 小牟禮 (トライ店システム評価)SMBチップ検索の絞り込み方法変更 START
    '            'Dim changeRegNo As List(Of String) = Me.GetChangeRegNo(inRegNo, inStaffInfo.DlrCD, inStaffInfo.BrnCD)
    '            '2015/11/10 TM 小牟禮 (トライ店システム評価)SMBチップ検索の絞り込み方法変更 END

    '            '2014/05/20 TMEJ 小澤 IT9684_サービスタブレットゲートカメラ連携機能追加開発 END

    '            '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

    '            'Dim rowChageRegNoInfo As SC3100401DataSet.ChageRegNoInfoRow

    '            'DataSetの宣言
    '            Using sc3100401Dac As New SC3100401DataSetTableAdapters.SC3100401TableAdapter

    '                ''自社客情報の取得
    '                'Dim dtCustomerInfo As SC3100401DataSet.ChageRegNoInfoDataTable = _
    '                '    sc3100401Dac.GetDBCustomerInfo(inRegNo, changeRegNo, inStaffInfo.DlrCD)

    '                ''自社客情報の取得確認
    '                'If dtCustomerInfo Is Nothing OrElse dtCustomerInfo.Rows.Count = 0 Then
    '                '    '自社客情報が取得できず
    '                '    '未取引客

    '                '    '新しいROWの作成
    '                '    rowChageRegNoInfo = dtCustomerInfo.NewChageRegNoInfoRow

    '                '    '識別コード：自社客
    '                '    rowChageRegNoInfo.CUSTSEGMENT = CustSegmentNewCustomer

    '                'Else
    '                '    '自社客情報が取得成功
    '                '    '自社客

    '                '    'ROWに変換
    '                '    rowChageRegNoInfo = DirectCast(dtCustomerInfo.Rows(0), SC3100401DataSet.ChageRegNoInfoRow)

    '                '    '識別コード：未取引客
    '                '    rowChageRegNoInfo.CUSTSEGMENT = CustSegmentMyCustomer

    '                'End If

    '                ''ストール予約情報の取得(変更前・変更後の車両登録Noで検索)
    '                'Dim dtStallRezInfo As SC3100401DataSet.StallRezInfoDataTable = _
    '                '    Me.GetStallReserveInfo(inRegNo, changeRegNo, inStaffInfo.DlrCD, inStaffInfo.BrnCD, inPresentTime, sc3100401Dac)

    '                ''ストール予約の取得確認
    '                'If dtStallRezInfo IsNot Nothing _
    '                '    AndAlso 0 < dtStallRezInfo.Count Then
    '                '    'ストール予約取得

    '                '    'ROWに変換
    '                '    Dim rowStallRezInfo As SC3100401DataSet.StallRezInfoRow = _
    '                '         DirectCast(dtStallRezInfo.Rows(0), SC3100401DataSet.StallRezInfoRow)

    '                '    'REZIDの設定
    '                '    rowChageRegNoInfo.REZID = rowStallRezInfo.REZID

    '                '    '顧客コードの確認
    '                '    If Not rowStallRezInfo.IsCUSTCDNull Then
    '                '        '顧客コード取得成功

    '                '        'ストール予約の顧客コードを設定
    '                '        rowChageRegNoInfo.CUSTCD = rowStallRezInfo.CUSTCD

    '                '    End If

    '                '    '車両登録Noの確認
    '                '    If Not rowStallRezInfo.IsVCLREGNONull Then
    '                '        '車両登録No取得成功

    '                '        'ストール予約の車両登録Noを設定
    '                '        rowChageRegNoInfo.VCLREGNO = rowStallRezInfo.VCLREGNO

    '                '    End If

    '                '    '氏名の確認
    '                '    If Not rowStallRezInfo.IsCUSTOMERNAMENull Then
    '                '        '氏名取得成功

    '                '        'ストール予約の氏名を設定
    '                '        rowChageRegNoInfo.NAME = rowStallRezInfo.CUSTOMERNAME

    '                '    End If

    '                '    '電話番号の確認
    '                '    If Not rowStallRezInfo.IsTELNONull Then
    '                '        '電話番号取得成功

    '                '        'ストール予約の電話番号を設定
    '                '        rowChageRegNoInfo.TELNO = rowStallRezInfo.TELNO

    '                '    End If

    '                '    '携帯番号の確認
    '                '    If Not rowStallRezInfo.IsMOBILENull Then
    '                '        '携帯番号取得成功

    '                '        'ストール予約の携帯番号を設定
    '                '        rowChageRegNoInfo.MOBILE = rowStallRezInfo.MOBILE

    '                '    End If

    '                '    'モデルコードの確認
    '                '    If Not rowStallRezInfo.IsMODELCODENull Then
    '                '        'モデルコード取得成功

    '                '        'ストール予約のモデルコードを設定
    '                '        rowChageRegNoInfo.MODELCODE = rowStallRezInfo.MODELCODE

    '                '    End If

    '                '    'サービスコードの設定
    '                '    rowChageRegNoInfo.SERVICECODE = rowStallRezInfo.SERVICECODE

    '                '    '担当SAの確認
    '                '    If Not rowStallRezInfo.IsACCOUNT_PLANNull Then
    '                '        '担当SA取得成功

    '                '        'ストール予約の担当SAをデフォルトSAに設定
    '                '        rowChageRegNoInfo.DEFAULTSACODE = rowStallRezInfo.ACCOUNT_PLAN

    '                '    End If

    '                '    '識別フラグの確認
    '                '    If Not rowStallRezInfo.IsCUSTSEGMENTNull Then
    '                '        '識別フラグ取得成功

    '                '        'ストール予約の識別フラグを登録区分に設定
    '                '        rowChageRegNoInfo.CUSTSEGMENT = rowStallRezInfo.CUSTSEGMENT

    '                '    End If

    '                '    '整備受注Noの確認
    '                '    If Not rowStallRezInfo.IsORDERNONull Then
    '                '        '整備受注No取得成功

    '                '        'ストール予約の整備受注Noを設定
    '                '        rowChageRegNoInfo.ORDERNO = rowStallRezInfo.ORDERNO

    '                '    End If

    '                'Else
    '                '    'ストール予約が無い

    '                '    'REZIDの設定(-1)
    '                '    rowChageRegNoInfo.REZID = -1

    '                'End If

    '                ''車両登録Noの確認
    '                'If rowChageRegNoInfo.IsVCLREGNONull Then
    '                '    '車両登録Noが存在しない

    '                '    '入力された登録Noを設定
    '                '    rowChageRegNoInfo.VCLREGNO = inRegNo
    '                'End If

    '                ''来店実績連番の設定
    '                'rowChageRegNoInfo.VISITSEQ = inVisitSeq

    '                ''更新アカウントの設定
    '                'rowChageRegNoInfo.ACCOUNT = inStaffInfo.Account

    '                ''更新日時の設定
    '                'rowChageRegNoInfo.UPDATEDATE = inUpDateTime

    '                ''現在日時の設定
    '                'rowChageRegNoInfo.PRESENTTIME = inPresentTime


    '                ''サービス来店管理更新(車両登録No変更に伴う情報)処理
    '                'returnCode = Me.UpDateRegNo(rowChageRegNoInfo)


    '                '車両登録番号情報の取得
    '                '2015/11/10 TM 小牟禮 (トライ店システム評価)SMBチップ検索の絞り込み方法変更 START
    '                'Dim dtChageRegNoInfo As SC3100401DataSet.ChageRegNoInfoDataTable = _
    '                '   sc3100401Dac.GetRegNoInfo(inRegNo, _
    '                '                          changeRegNo, _
    '                '                          inStaffInfo.DlrCD, _
    '                '                          inStaffInfo.BrnCD, _
    '                '                          inPresentTime)
    '                '車両登録番号検索ワード変換を行う
    '                Dim searchRegNo As String = serviceCommonClass.ConvertVclRegNumWord(inRegNo)

    '                Dim dtChageRegNoInfo As SC3100401DataSet.ChageRegNoInfoDataTable = _
    '                    sc3100401Dac.GetRegNoInfo(searchRegNo, _
    '                                              inStaffInfo.DlrCD, _
    '                                              inStaffInfo.BrnCD, _
    '                                              inPresentTime)
    '                '2015/11/10 TM 小牟禮 (トライ店システム評価)SMBチップ検索の絞り込み方法変更 END

    '                Dim rowChageRegNoInfo As SC3100401DataSet.ChageRegNoInfoRow

    '                '車両登録番号情報取得確認
    '                If dtChageRegNoInfo.Rows.Count = 0 Then
    '                    '取得できなかった場合

    '                    '新しいROWの作成
    '                    rowChageRegNoInfo = dtChageRegNoInfo.NewChageRegNoInfoRow

    '                    '識別コード：未取引客
    '                    rowChageRegNoInfo.CUSTSEGMENT = CustSegmentNewCustomer

    '                    '予約ID：-1
    '                    rowChageRegNoInfo.REZID = -1

    '                Else
    '                    '取得成功

    '                    'ROWに変換
    '                    rowChageRegNoInfo = DirectCast(dtChageRegNoInfo.Rows(0), SC3100401DataSet.ChageRegNoInfoRow)

    '                    '予約情報の取得確認
    '                    If Not rowChageRegNoInfo.IsREZIDNull _
    '                        AndAlso 0 < rowChageRegNoInfo.CST_ID _
    '                        AndAlso Not String.IsNullOrEmpty(rowChageRegNoInfo.CST_VCL_TYPE) Then
    '                        '予約情報が取得でき、予約の中の顧客情報が取得できた場合

    '                        '予約の顧客コードに書き換える
    '                        rowChageRegNoInfo.CUSTCD = rowChageRegNoInfo.CST_ID

    '                        '2015/09/10 TMEJ 春日井 IT9960_(トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START

    '                        '来店者氏名に顧客氏名を設定
    '                        rowChageRegNoInfo.VISITNAME = rowChageRegNoInfo.NAME

    '                        '顧客電話番号の取得確認
    '                        If Not String.IsNullOrEmpty(rowChageRegNoInfo.TELNO) Then
    '                            '取得成功

    '                            '来店者電話番号に顧客電話番号を設定
    '                            rowChageRegNoInfo.VISITTELNO = rowChageRegNoInfo.TELNO

    '                            '顧客携帯電話番号の取得確認
    '                        ElseIf Not String.IsNullOrEmpty(rowChageRegNoInfo.MOBILE) Then
    '                            '取得成功

    '                            '来店者電話番号に顧客携帯電話番号を設定
    '                            rowChageRegNoInfo.VISITTELNO = rowChageRegNoInfo.MOBILE

    '                        Else
    '                            '取得できなかった場合
    '                            rowChageRegNoInfo.SetVISITTELNONull()
    '                        End If

    '                        '2015/09/10 TMEJ 春日井 IT9960_(トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END

    '                        '顧客情報の取得
    '                        Dim dtCustomerInfo As SC3100401DataSet.CustomerInfoDataTable = _
    '                            sc3100401Dac.GetCustomerInfo(inStaffInfo.DlrCD, _
    '                                                         rowChageRegNoInfo.CST_ID, _
    '                                                         rowChageRegNoInfo.VCL_ID, _
    '                                                         rowChageRegNoInfo.CST_VCL_TYPE)

    '                        '顧客情報取得確認
    '                        If 0 < dtCustomerInfo.Rows.Count Then
    '                            '顧客情報取得成功

    '                            'ROWに変換
    '                            Dim rowCustomerInfo As SC3100401DataSet.CustomerInfoRow = _
    '                                DirectCast(dtCustomerInfo.Rows(0), SC3100401DataSet.CustomerInfoRow)

    '                            '性別
    '                            rowChageRegNoInfo.SEX = rowCustomerInfo.CST_GENDER

    '                            'スタッフコード
    '                            rowChageRegNoInfo.STAFFCD = rowCustomerInfo.SVC_PIC_STF_CD

    '                            '顧客氏名
    '                            rowChageRegNoInfo.NAME = rowCustomerInfo.CST_NAME

    '                            '顧客電話番号
    '                            rowChageRegNoInfo.TELNO = rowCustomerInfo.CST_PHONE

    '                            '顧客携帯番号
    '                            rowChageRegNoInfo.MOBILE = rowCustomerInfo.CST_MOBILE

    '                            '2015/09/10 TMEJ 春日井 IT9960_(トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START

    '                            '来店者氏名に顧客氏名を設定
    '                            rowChageRegNoInfo.VISITNAME = rowChageRegNoInfo.NAME

    '                            '顧客電話番号の取得確認
    '                            If Not String.IsNullOrEmpty(rowChageRegNoInfo.TELNO) Then
    '                                '取得成功

    '                                '来店者電話番号に顧客電話番号を設定
    '                                rowChageRegNoInfo.VISITTELNO = rowChageRegNoInfo.TELNO

    '                                '顧客携帯電話番号の取得確認
    '                            ElseIf Not String.IsNullOrEmpty(rowChageRegNoInfo.MOBILE) Then
    '                                '取得成功

    '                                '来店者電話番号に顧客携帯電話番号を設定
    '                                rowChageRegNoInfo.VISITTELNO = rowChageRegNoInfo.MOBILE

    '                            Else
    '                                '取得できなかった場合
    '                                rowChageRegNoInfo.SetVISITTELNONull()
    '                            End If

    '                            '2015/09/10 TMEJ 春日井 IT9960_(トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END

    '                            'サービス入庫追加情報の取得確認
    '                            If rowChageRegNoInfo.IsDMS_DMSIDNull Then
    '                                '取得できなかった場合

    '                                '取得した顧客データに更新する

    '                                '顧客種別
    '                                rowChageRegNoInfo.CUSTSEGMENT = rowCustomerInfo.CST_TYPE

    '                                '基幹顧客コード
    '                                rowChageRegNoInfo.DMS_CST_CD = rowCustomerInfo.DMS_CST_CD

    '                            End If

    '                            '2015/09/10 TMEJ 春日井 IT9960_(トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START

    '                            '車両登録No情報の顧客車両区分が1:所有者以外の場合
    '                            If Not String.Equals(CustomerVehicleTypeOwner, rowChageRegNoInfo.CST_VCL_TYPE) Then

    '                                'オーナー顧客情報取得
    '                                Dim dtOwnerCustomerInfo As SC3100401DataSet.OwnerCustomerInfoDataTable = _
    '                                    sc3100401Dac.GetOwnerCustomerInfo(inStaffInfo.DlrCD, _
    '                                                                      rowChageRegNoInfo.VCL_ID)

    '                                'オーナー顧客情報取得確認
    '                                If 0 < dtOwnerCustomerInfo.Rows.Count Then
    '                                    'オーナー顧客情報取得成功

    '                                    'ROWに変換
    '                                    Dim rowOwnerCustomerInfo As SC3100401DataSet.OwnerCustomerInfoRow = _
    '                                        DirectCast(dtOwnerCustomerInfo.Rows(0), SC3100401DataSet.OwnerCustomerInfoRow)

    '                                    '顧客名
    '                                    rowChageRegNoInfo.NAME = rowOwnerCustomerInfo.CUSTOMERNAME

    '                                    '顧客電話番号
    '                                    rowChageRegNoInfo.TELNO = rowOwnerCustomerInfo.TELNO

    '                                    '顧客携帯電話番号
    '                                    rowChageRegNoInfo.MOBILE = rowOwnerCustomerInfo.MOBILE

    '                                    '性別
    '                                    rowChageRegNoInfo.SEX = rowOwnerCustomerInfo.SEX

    '                                    '基幹顧客コード
    '                                    rowChageRegNoInfo.DMS_CST_CD = rowOwnerCustomerInfo.DMSID

    '                                End If

    '                            End If

    '                            '2015/09/10 TMEJ 春日井 IT9960_(トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END

    '                        End If

    '                    Else
    '                        '予約が取得できなかった場合

    '                        'サービス入庫IDに(－1)を設定
    '                        rowChageRegNoInfo.REZID = -1

    '                    End If
    '                End If

    '                '車両登録Noの確認
    '                If rowChageRegNoInfo.IsVCLREGNONull Then
    '                    '車両登録Noが存在しない

    '                    '入力された登録Noを設定
    '                    rowChageRegNoInfo.VCLREGNO = inRegNo
    '                End If

    '                '来店実績連番の設定
    '                rowChageRegNoInfo.VISITSEQ = inVisitSeq

    '                '更新アカウントの設定
    '                rowChageRegNoInfo.ACCOUNT = inStaffInfo.Account

    '                '更新日時の設定
    '                rowChageRegNoInfo.UPDATEDATE = inUpDateTime

    '                '現在日時の設定
    '                rowChageRegNoInfo.PRESENTTIME = inPresentTime


    '                'サービス来店管理更新(車両登録No変更に伴う情報)処理
    '                returnCode = Me.UpDateRegNo(rowChageRegNoInfo)

    '                '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

    '            End Using

    '        Catch ex As OracleExceptionEx When ex.Number = 1013

    '            'DBタイムアウトエラー
    '            returnCode = ResultCode.ErrDBTimeout

    '            Logger.Error(String.Format(CultureInfo.CurrentCulture _
    '                         , "{0}.{1} ERR:DBTIMEOUT RETURNCODE = {2}" _
    '                         , Me.GetType.ToString _
    '                         , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                         , returnCode))
    '        End Try

    '        '2015/11/10 TM 小牟禮 (トライ店システム評価)SMBチップ検索の絞り込み方法変更 START
    '    End Using
    '    '2015/11/10 TM 小牟禮 (トライ店システム評価)SMBチップ検索の絞り込み方法変更 END

    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '               , "{0}.{1} END RETURNCODE = {2}" _
    '               , Me.GetType.ToString _
    '               , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '               , returnCode))

    '    Return returnCode

    'End Function

    ''' <summary>
    ''' 車両情報取得処理
    ''' </summary>
    ''' <param name="inRegNo">車両登録No</param>
    ''' <param name="inStaffInfo">ログイン情報</param>
    ''' <param name="inPresentTime">現在日時</param>
    ''' <returns>顧客車両情報</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする
    ''' </history>
    Public Function GetVehicleInfo(ByVal inRegNo As String, _
                                  ByVal inStaffInfo As StaffContext, _
                                  ByVal inPresentTime As Date) _
                                  As SC3100401DataSet.VehicleInfoDataTable

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} REGNO:{2} ACCOUNT:{3} PRESENTTIME:{4}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , inRegNo, inStaffInfo.Account, inPresentTime))
        '処理結果
        Dim dtVehicleInfo As SC3100401DataSet.VehicleInfoDataTable

        Using serviceCommonClass As New ServiceCommonClassBusinessLogic

            Try

                'DataSetの宣言
                Using sc3100401Dac As New SC3100401DataSetTableAdapters.SC3100401TableAdapter

                    '車両登録番号検索ワード変換を行う
                    Dim searchRegNo As String = serviceCommonClass.ConvertVclRegNumWord(inRegNo)

                    dtVehicleInfo = sc3100401Dac.GetVehicleInfo(searchRegNo, _
                                                                   inStaffInfo.DlrCD, _
                                                                   inStaffInfo.BrnCD, _
                                                                   inPresentTime)
                End Using

                If IsNothing(dtVehicleInfo) Then
                    Return Nothing
                End If
                If dtVehicleInfo.Rows.Count = 0 Then
                    Return Nothing
                End If

                Return dtVehicleInfo

            Catch ex As OracleExceptionEx When ex.Number = 1013
                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                             , "{0}.{1} ERR:DBTIMEOUT RETURNCODE = {2}" _
                             , Me.GetType.ToString _
                             , System.Reflection.MethodBase.GetCurrentMethod.Name _
                             , ResultCode.ErrDBTimeout))

                Return Nothing

            End Try
        End Using

    End Function

    ''' <summary>
    ''' 来店車両情報更新
    ''' </summary>
    ''' <param name="invisitSeq">来店実績連番</param>
    ''' <param name="inupDateTime">更新日時</param>
    ''' <param name="inRegNo">車両登録No</param>
    ''' <param name="inStaffInfo">ログイン情報</param>
    ''' <param name="inPresentTime">現在日時</param>
    ''' <param name="inCstId">顧客ID</param>
    ''' <param name="inVclId">車両ID</param>
    ''' <param name="inRezId">予約ID</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする
    ''' </history>
    Public Function UpdateVisitVehicle(ByVal inVisitSeq As Long, _
                                       ByVal inUpDateTime As Date, _
                                       ByVal inRegNo As String, _
                                       ByVal inStaffInfo As StaffContext, _
                                       ByVal inPresentTime As Date, _
                                       ByVal inCstId As Decimal, _
                                       ByVal inVclId As Decimal, _
                                       ByVal inRezId As Decimal) _
                                       As Integer

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} VISITSEQ:{2} UPDATETIME:{3} REGNO:{4} ACCOUNT:{5} PRESENTTIME:{6}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , inVisitSeq, inUpDateTime, inRegNo, inStaffInfo.Account, inPresentTime))
        '処理結果
        Dim returnCode As Integer = ResultCode.Success

        Try

            'DataSetの宣言
            Using sc3100401Dac As New SC3100401DataSetTableAdapters.SC3100401TableAdapter

                Dim dtChageRegNoInfo As New SC3100401DataSet.ChageRegNoInfoDataTable

                '顧客IDと車両IDの確認
                If 0 < inCstId AndAlso 0 < inVclId Then
                    '設定されている場合

                    '更新用車両情報の取得
                    dtChageRegNoInfo = sc3100401Dac.GetUpdateVehicleInfo(inStaffInfo.DlrCD, _
                                                                        inPresentTime, _
                                                                        inCstId, _
                                                                        inVclId)
                End If

                Dim rowChageRegNoInfo As SC3100401DataSet.ChageRegNoInfoRow

                '更新用車両情報取得確認
                If dtChageRegNoInfo.Rows.Count = 0 Then
                    '取得できなかった場合

                    '新しいROWの作成
                    rowChageRegNoInfo = dtChageRegNoInfo.NewChageRegNoInfoRow

                    '識別コード：未取引客
                    rowChageRegNoInfo.CUSTSEGMENT = CustSegmentNewCustomer

                    '予約ID：-1
                    rowChageRegNoInfo.REZID = -1

                Else
                    '取得成功

                    'ROWに変換
                    rowChageRegNoInfo = DirectCast(dtChageRegNoInfo.Rows(0), SC3100401DataSet.ChageRegNoInfoRow)

                    '来店者氏名に顧客氏名を設定
                    rowChageRegNoInfo.VISITNAME = rowChageRegNoInfo.NAME

                    '顧客携帯電話番号の取得確認
                    If Not String.IsNullOrEmpty(rowChageRegNoInfo.MOBILE) Then
                        '取得成功

                        '来店者電話番号に顧客携帯電話番号を設定
                        rowChageRegNoInfo.VISITTELNO = rowChageRegNoInfo.MOBILE

                        '顧客電話番号の取得確認
                    ElseIf Not String.IsNullOrEmpty(rowChageRegNoInfo.TELNO) Then
                        '取得成功

                        '来店者電話番号に顧客電話番号を設定
                        rowChageRegNoInfo.VISITTELNO = rowChageRegNoInfo.TELNO

                    Else
                        '取得できなかった場合
                        rowChageRegNoInfo.SetVISITTELNONull()
                    End If

                    '車両登録No情報の顧客車両区分が1:所有者以外の場合
                    If Not String.Equals(CustomerVehicleTypeOwner, rowChageRegNoInfo.CST_VCL_TYPE) Then

                        'オーナー顧客情報取得
                        Dim dtOwnerCustomerInfo As SC3100401DataSet.OwnerCustomerInfoDataTable = _
                            sc3100401Dac.GetOwnerCustomerInfo(inStaffInfo.DlrCD, _
                                                              rowChageRegNoInfo.VCL_ID)

                        'オーナー顧客情報取得確認
                        If 0 < dtOwnerCustomerInfo.Rows.Count Then
                            'オーナー顧客情報取得成功

                            'ROWに変換
                            Dim rowOwnerCustomerInfo As SC3100401DataSet.OwnerCustomerInfoRow = _
                                DirectCast(dtOwnerCustomerInfo.Rows(0), SC3100401DataSet.OwnerCustomerInfoRow)

                            '顧客名
                            rowChageRegNoInfo.NAME = rowOwnerCustomerInfo.CUSTOMERNAME

                            '顧客電話番号
                            rowChageRegNoInfo.TELNO = rowOwnerCustomerInfo.TELNO

                            '顧客携帯電話番号
                            rowChageRegNoInfo.MOBILE = rowOwnerCustomerInfo.MOBILE

                            '性別区分
                            rowChageRegNoInfo.SEX = rowOwnerCustomerInfo.SEX

                            '基幹顧客コード
                            rowChageRegNoInfo.DMS_CST_CD = rowOwnerCustomerInfo.DMSID

                        End If

                    End If

                    '予約情報の取得確認
                    If 0 < inRezId Then

                        Dim dtRezInfo As SC3100401DataSet.RezInfoDataTable = _
                            sc3100401Dac.GetRezInfo(inRezId, _
                                                    inStaffInfo.DlrCD, _
                                                    inStaffInfo.BrnCD)

                        If 0 < dtRezInfo.Rows.Count Then
                            '予約が取得できた場合

                            'Rowに変換
                            Dim rowRezInfo As SC3100401DataSet.RezInfoRow = _
                                DirectCast(dtRezInfo.Rows(0), SC3100401DataSet.RezInfoRow)

                            '予約ID
                            rowChageRegNoInfo.REZID = rowRezInfo.REZID

                            If Not rowChageRegNoInfo.IsORDERNONull Then
                                'RO番号
                                rowChageRegNoInfo.ORDERNO = rowRezInfo.ORDERNO
                            End If

                            If Not String.IsNullOrEmpty(rowRezInfo.DEFAULTSACODE) Then
                                '担当SAスタッフコード
                                rowChageRegNoInfo.DEFAULTSACODE = rowRezInfo.DEFAULTSACODE
                            End If

                            '顧客ID
                            rowChageRegNoInfo.CUSTCD = rowRezInfo.CUSTCD

                            '顧客車両種別
                            rowChageRegNoInfo.CST_VCL_TYPE = rowRezInfo.CST_VCL_TYPE

                        Else
                            '予約が取得できなかった場合

                            'サービス入庫IDに(－1)を設定
                            rowChageRegNoInfo.REZID = -1

                        End If
                    Else
                        'サービス入庫IDに(－1)を設定
                        rowChageRegNoInfo.REZID = -1
                    End If
                End If

                '車両登録Noの確認
                If rowChageRegNoInfo.IsVCLREGNONull Then
                    '車両登録Noが存在しない

                    '入力された登録Noを設定
                    rowChageRegNoInfo.VCLREGNO = inRegNo
                End If

                '来店実績連番の設定
                rowChageRegNoInfo.VISITSEQ = inVisitSeq

                '更新アカウントの設定
                rowChageRegNoInfo.ACCOUNT = inStaffInfo.Account

                '更新日時の設定
                rowChageRegNoInfo.UPDATEDATE = inUpDateTime

                '現在日時の設定
                rowChageRegNoInfo.PRESENTTIME = inPresentTime


                'サービス来店管理更新(車両登録No変更に伴う情報)処理
                returnCode = Me.UpDateRegNo(rowChageRegNoInfo)

            End Using

        Catch ex As OracleExceptionEx When ex.Number = 1013

            'DBタイムアウトエラー
            returnCode = ResultCode.ErrDBTimeout

            Logger.Error(String.Format(CultureInfo.CurrentCulture _
                         , "{0}.{1} ERR:DBTIMEOUT RETURNCODE = {2}" _
                         , Me.GetType.ToString _
                         , System.Reflection.MethodBase.GetCurrentMethod.Name _
                         , returnCode))
        End Try
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END RETURNCODE = {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , returnCode))

        Return returnCode

    End Function

    '2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする END
#End Region

#Region "Privateメソッド"

    '2015/11/10 TM 小牟禮 (トライ店システム評価)SMBチップ検索の絞り込み方法変更 START
    '''' <summary>
    '''' 変換フォーマット置換後車両登録Noの取得
    '''' </summary>
    '''' <param name="inRegNo">対象の車輌登録No</param>
    '''' <param name="inDealerCode">StaffContext</param>
    '''' <param name="inStoreCode">StaffContext</param>
    '''' <returns>変換フォーマット置換後車両登録Noリスト</returns>
    '''' <remarks></remarks>
    '''' <history>
    '''' 2014/05/20 TMEJ 小澤 IT9684_サービスタブレットゲートカメラ連携機能追加開発
    '''' </history>
    'Private Function GetChangeRegNo(ByVal inRegNo As String, _
    '                                ByVal inDealerCode As String, _
    '                                ByVal inStoreCode As String) As List(Of String)
    '    '2014/05/20 TMEJ 小澤 IT9684_サービスタブレットゲートカメラ連携機能追加開発 START
    '    'Private Function GetChangeRegNo(ByVal inRegNo As String, _
    '    '                                ByVal inDealerCode As String, _
    '    '                                ByVal inStoreCode As String) As String
    '    '2014/05/20 TMEJ 小澤 IT9684_サービスタブレットゲートカメラ連携機能追加開発 END
    '
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                , "{0}.{1} REGNO:{2} DEALERCODE:{3} STORECODE:{4}" _
    '                , Me.GetType.ToString _
    '                , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                , inRegNo, inDealerCode, inStoreCode))
    '
    '    '変換フォーマット、変換当て込み文字パスを取得
    '    Dim dlrEnvSet As New BranchEnvSetting
    '
    '    '変換フォーマットを取得
    '    Dim sysEnvChangeFormatRow As DlrEnvSettingDataSet.DLRENVSETTINGRow _
    '        = dlrEnvSet.GetEnvSetting(inDealerCode, inStoreCode, VclRegNoChangeFormat)
    '
    '    '変換フォーマット用置換文字列を取得
    '    Dim sysEnvChangeStringRow As DlrEnvSettingDataSet.DLRENVSETTINGRow _
    '        = dlrEnvSet.GetEnvSetting(inDealerCode, inStoreCode, VclRegNoChangeString)
    '
    '    'フォーマット文字列 例：[フォーマット「XX-XXXX」]
    '    Dim formatString As String = String.Empty
    '
    '    'フォーマットを作成している文字列 例：[フォーマットが「XX-XXXX」の場合の「X」]
    '    Dim makeFormatString As String = String.Empty
    '
    '    ' どちらか一方でも設定されていなければ、フォーマットによる変換は行わない。
    '    If sysEnvChangeFormatRow IsNot Nothing _
    '        AndAlso sysEnvChangeStringRow IsNot Nothing Then
    '        '上記2つの情報が存在する場合
    '
    '        '2014/05/20 TMEJ 小澤 IT9684_サービスタブレットゲートカメラ連携機能追加開発 START
    '        ''値の設定フォーマット文字列
    '        'formatString = sysEnvChangeFormatRow.PARAMVALUE
    '
    '        ''値の設定フォーマットを作成している文字列
    '        'makeFormatString = sysEnvChangeStringRow.PARAMVALUE
    '
    '        'フォーマット文字列の検索
    '        If 0 <= sysEnvChangeFormatRow.PARAMVALUE.IndexOf(sysEnvChangeStringRow.PARAMVALUE) Then
    '            '存在する場合
    '
    '            '値の設定フォーマット文字列
    '            formatString = sysEnvChangeFormatRow.PARAMVALUE
    '
    '            '値の設定フォーマットを作成している文字列
    '            makeFormatString = sysEnvChangeStringRow.PARAMVALUE
    '
    '        End If
    '        '2014/05/20 TMEJ 小澤 IT9684_サービスタブレットゲートカメラ連携機能追加開発 END
    '
    '
    '    End If
    '
    '    '2014/05/20 TMEJ 小澤 IT9684_サービスタブレットゲートカメラ連携機能追加開発 START
    '    ''作業用STRING
    '    'Dim returnString As New StringBuilder
    '
    '    '作業用STRING
    '    Dim returnString As New List(Of String)
    '    '2014/05/20 TMEJ 小澤 IT9684_サービスタブレットゲートカメラ連携機能追加開発 END
    '
    '    'フォーマットの文字数
    '    Dim formatIndex As Integer = 0
    '
    '    '車両登録Noの文字数
    '    Dim targetIndex As Integer = 0
    '
    '    '2014/05/20 TMEJ 小澤 IT9684_サービスタブレットゲートカメラ連携機能追加開発 START
    '    ''変換フォーマットか対象の車両登録Noの文字数を越えるまでループ
    '    'While formatIndex < formatString.Length _
    '    '    AndAlso targetIndex < inRegNo.Length
    '
    '    '    '変更前車両登録Noにハイフンを追加する処理
    '    '    '例：車両登録No：AAAA-12345 フォーマット：XX-XXXXX の場合「AA-AA-12」に変換
    '
    '    '    '変換フォーマットと当て込み文字が一致していたら
    '    '    If String.Equals(formatString(formatIndex), makeFormatString) Then
    '    '        '一致している場合
    '
    '    '        '車両登録Noの文字を設定
    '    '        returnString.Append(inRegNo(targetIndex))
    '
    '    '        'Index+1
    '    '        targetIndex += 1
    '    '        formatIndex += 1
    '    '    Else
    '    '        '一致していない場合
    '
    '    '        '変換フォーマットの文字を当て込み設定
    '    '        returnString.Append(formatString(formatIndex))
    '
    '    '        'Index+1
    '    '        formatIndex += 1
    '    '    End If
    '    'End While
    '    For Each replaceRegisterNo As String In formatString.Split(CChar(","))
    '        '変換後文字列
    '        Dim replaceRegisterNoAfter As New StringBuilder
    '
    '        '文字数初期化
    '        formatIndex = 0
    '        targetIndex = 0
    '
    '        '変換フォーマットか対象の車両登録Noの文字数を越えるまでループ
    '        While formatIndex < replaceRegisterNo.Length _
    '            AndAlso targetIndex < inRegNo.Length
    '
    '            '変更前車両登録Noにハイフンを追加する処理
    '            '例：車両登録No：AAAA-12345 フォーマット：XX-XXXXX の場合「AA-AA-12」に変換
    '
    '            '変換フォーマットと当て込み文字が一致していたら
    '            If String.Equals(replaceRegisterNo(formatIndex), makeFormatString) Then
    '                '一致している場合
    '
    '                '車両登録Noの文字を設定
    '                replaceRegisterNoAfter.Append(inRegNo(targetIndex))
    '
    '                'Index+1
    '                targetIndex += 1
    '                formatIndex += 1
    '
    '            Else
    '                '一致していない場合
    '
    '                '変換フォーマットの文字を当て込み設定
    '                replaceRegisterNoAfter.Append(replaceRegisterNo(formatIndex))
    '
    '                'Index+1
    '                formatIndex += 1
    '            End If
    '
    '        End While
    '
    '        '変換文字列チェック
    '        If Not (String.IsNullOrEmpty(replaceRegisterNoAfter.ToString)) Then
    '            '変換されている場合
    '            '格納文字列チェック
    '            returnString.Add(replaceRegisterNoAfter.ToString)
    '
    '        End If
    '
    '    Next
    '
    '    '2014/05/20 TMEJ 小澤 IT9684_サービスタブレットゲートカメラ連携機能追加開発 END
    '
    '    '2014/05/20 TMEJ 小澤 IT9684_サービスタブレットゲートカメラ連携機能追加開発 START
    '    ''変換結果
    '    'Dim returnValue As String = returnString.ToString
    '    '2014/05/20 TMEJ 小澤 IT9684_サービスタブレットゲートカメラ連携機能追加開発 END
    '
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                , "{0}.{1} END CHANGEREGNO = {2}" _
    '                , Me.GetType.ToString _
    '                , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                , returnString))
    '
    '    '2014/05/20 TMEJ 小澤 IT9684_サービスタブレットゲートカメラ連携機能追加開発 START
    '    ''変換処理が行われているか確認
    '    'If String.IsNullOrEmpty(returnValue) Then
    '    '    '変換が行われていない
    '
    '    '    'Emptyを返却
    '    '    Return String.Empty
    '    'Else
    '    '    '変換が行われている
    '
    '    '    '変換結果を返却
    '    '    Return returnValue
    '    'End If
    '
    '    Return returnString
    '    '2014/05/20 TMEJ 小澤 IT9684_サービスタブレットゲートカメラ連携機能追加開発 END
    '
    'End Function
    '2015/11/10 TM 小牟禮 (トライ店システム評価)SMBチップ検索の絞り込み方法変更 END

    '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

    ' ''' <summary>
    ' ''' ストール予約情報の取得
    ' ''' </summary>
    ' ''' <param name="inRegNo">車両登録No</param>
    ' ''' <param name="inchangeRegNo">変換後車両登録No</param>
    ' ''' <param name="inDealerCode">販売店コード</param>
    ' ''' <param name="inStoreCode">店舗コード</param>
    ' ''' <param name="inPresentTime">現在時間</param>
    ' ''' <param name="inSC3100401DataSet">Dac</param>
    ' ''' <returns>ストール予約情報</returns>
    ' ''' <remarks></remarks>
    ' ''' <history></history>
    'Private Function GetStallReserveInfo(ByVal inRegNo As String, _
    '                                     ByVal inchangeRegNo As String, _
    '                                     ByVal inDealerCode As String, _
    '                                     ByVal inStoreCode As String, _
    '                                     ByVal inPresentTime As Date, _
    '                                     ByVal inSC3100401DataSet As SC3100401DataSetTableAdapters.SC3100401TableAdapter) _
    '                                     As SC3100401DataSet.StallRezInfoDataTable

    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                , "{0}.{1} REGNO:{2} DEALERCODE:{3} STORECODE:{4}" _
    '                , Me.GetType.ToString _
    '                , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                , inRegNo, inDealerCode, inStoreCode))

    '    'ストール予約情報の取得
    '    Dim dtStallRezInfo As SC3100401DataSet.StallRezInfoDataTable = _
    '        inSC3100401DataSet.GetDBStallRezInfo(inRegNo, inchangeRegNo, inDealerCode, inStoreCode, inPresentTime)

    '    For Each row As SC3100401DataSet.StallRezInfoRow In dtStallRezInfo.Rows

    '        'ストール実績の取得
    '        Using dtStallProcess As SC3100401DataSet.StallProcessDataTable = _
    '            inSC3100401DataSet.GetDBStallProcess(row.REZID, inDealerCode, inStoreCode)

    '            'ストール実績の取得確認
    '            If dtStallProcess.Rows.Count = 0 Then
    '                '取得できなかった場合

    '                '次の行へ
    '                Continue For

    '            End If

    '            'ROWへ変換
    '            Dim rowStallProcess As SC3100401DataSet.StallProcessRow = _
    '                DirectCast(dtStallProcess.Rows(0), SC3100401DataSet.StallProcessRow)

    '            '検索に使った予約IDは作業が始まっているか検索する

    '            '日跨ぎシーケンスの確認(初日実績は0から始まり、実績日時が複数有る場合は+1) 
    '            If 0 < rowStallProcess.DSEQNO Then
    '                '日を跨いで作業が行われている場合

    '                'このストール予約は対象から除外する
    '                row.Delete()

    '                '次の行へ
    '                Continue For
    '            End If

    '            '作業実績シーケンス(通常実績は1から始まり、中断、再開時に+1)
    '            If 1 < rowStallProcess.SEQNO Then
    '                '1以上の場合作業が開始済

    '                'このストール予約は対象から除外する
    '                row.Delete()

    '                '次の行へ
    '                Continue For
    '            End If

    '            '実績ステータスの確認
    '            If rowStallProcess.IsRESULT_STATUSNull _
    '                OrElse Not StatusNoReceiving.Equals(rowStallProcess.RESULT_STATUS) Then
    '                '未入庫以外のものの場合

    '                'このストール予約は対象から除外する
    '                row.Delete()

    '                '次の行へ
    '                Continue For
    '            End If

    '        End Using
    '    Next

    '    '変更のコミット
    '    dtStallRezInfo.AcceptChanges()

    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                , "{0}.{1} END COUNT = {2}" _
    '                , Me.GetType.ToString _
    '                , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                , dtStallRezInfo.Count))

    '    Return dtStallRezInfo

    'End Function

    '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

    ''' <summary>
    ''' 車両登録No変更に伴うサービス来店管理テーブルの更新
    ''' </summary>
    ''' <param name="inRowChageRegNoInfo">車両登録No更新情報</param>
    ''' <returns>更新結果</returns>
    ''' <remarks></remarks>
    ''' <history></history>
    <EnableCommit()>
    Private Function UpDateRegNo(ByVal inRowChageRegNoInfo As SC3100401DataSet.ChageRegNoInfoRow) _
                                 As Integer

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '処理結果
        Dim returnCode As Integer = ResultCode.Success

        '更新件数
        Dim upDateCount As Integer = 0

        'DataSetの宣言
        Using sc3100401Dac As New SC3100401DataSetTableAdapters.SC3100401TableAdapter

            Try
                '車両登録No変更に伴うサービス来店管理テーブルの更新処理
                upDateCount = sc3100401Dac.UpDateDBRegNo(inRowChageRegNoInfo)

                '更新確認
                If upDateCount = 0 Then
                    '更新失敗

                    '排他エラー
                    returnCode = ResultCode.ErrExclusion

                End If

            Catch ex As OracleExceptionEx When ex.Number = 1013

                'DBタイムアウトエラー
                returnCode = ResultCode.ErrDBTimeout

                'ロールバック
                Me.Rollback = True

                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                             , "{0}.{1} ERR:DBTIMEOUT RETURNCODE = {2}" _
                             , Me.GetType.ToString _
                             , System.Reflection.MethodBase.GetCurrentMethod.Name _
                             , returnCode))

            Catch ex As OracleExceptionEx

                'DBエラー
                returnCode = ResultCode.ErrDB

                'ロールバック
                Me.Rollback = True

                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                             , "{0}.{1} ERR:DBERR RETURNCODE = {2}" _
                             , Me.GetType.ToString _
                             , System.Reflection.MethodBase.GetCurrentMethod.Name _
                             , returnCode))

            Catch ex As Exception

                'その他処理エラー
                returnCode = ResultCode.ErrOutType

                'ロールバック
                Me.Rollback = True

                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                             , "{0}.{1} ERR:EXCEPTION RETURNCODE = {2}" _
                             , Me.GetType.ToString _
                             , System.Reflection.MethodBase.GetCurrentMethod.Name _
                             , returnCode))

            End Try
        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END RETURNCODE = {2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , returnCode))

        Return returnCode

    End Function

#End Region

#End Region

#Region "SA登録・変更・UNDO処理"

#Region "Publicメソッド"

    ''' <summary>
    ''' SA登録・変更・UNDO処理
    ''' </summary>
    ''' <param name="invisitSeq">来店実績連番</param>
    ''' <param name="inupDateTime">更新日時</param>
    ''' <param name="inEventKeyID">イベントID</param>
    ''' <param name="inAfterAccount">更新する値</param>
    ''' <param name="inStaffInfo">ログイン情報</param>
    ''' <param name="inPresentTime">現在日時</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Public Function RegisterSA(ByVal inVisitSeq As Long, _
                               ByVal inUpDateTime As Date, _
                               ByVal inEventKeyId As String, _
                               ByVal inAfterAccount As String, _
                               ByVal inStaffInfo As StaffContext, _
                               ByVal inPresentTime As Date) _
                               As Integer

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} VISITSEQ:{2} UPDATEDATE:{3} EVENTID:{4} AFTERVALUE:{5} ACCOUNT:{6} PRESENTTIME:{7}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , inVisitSeq, inUpDateTime, inEventKeyId, inAfterAccount, inStaffInfo.Account, inPresentTime))
        '処理結果
        Dim returnCode As Integer = ResultCode.Success

        'DataSetの宣言
        Using sc3100401Dac As New SC3100401DataSetTableAdapters.SC3100401TableAdapter

            Try

                '来店管理情報の取得
                Dim dtVisitInfo As SC3100401DataSet.VisitInfoDataTable = _
                    sc3100401Dac.GetDBVisitInfo(inVisitSeq, inUpDateTime)

                '来店管理情報の取得確認(更新日時を条件にしているので検索結果がなければ排他エラー)
                If dtVisitInfo IsNot Nothing _
                    AndAlso 0 < dtVisitInfo.Count Then
                    '取得成功

                    'ROWに変換
                    Dim rowVisitInfo As SC3100401DataSet.VisitInfoRow = _
                        DirectCast(dtVisitInfo.Rows(0), SC3100401DataSet.VisitInfoRow)

                    '振当SAの設定
                    rowVisitInfo.SACODE = inAfterAccount

                    '現在日時の設定
                    rowVisitInfo.PRESENTTIME = inPresentTime

                    'SA登録・変更・UNDO処理
                    returnCode = Me.RegisterSACommit(rowVisitInfo, inEventKeyId, inStaffInfo, sc3100401Dac)

                    '更新処理結果
                    If returnCode = ResultCode.Success Then
                        '更新成功

                        '通知処理
                        Me.NoticeProcessing(rowVisitInfo, inStaffInfo, inEventKeyId)

                    End If

                Else
                    '取得失敗

                    '排他エラー
                    returnCode = ResultCode.ErrExclusion

                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                 , "{0}.{1} ERR:EXCLUSION RETURNCODE = {2}" _
                                 , Me.GetType.ToString _
                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                 , returnCode))

                End If

            Catch ex As OracleExceptionEx When ex.Number = 1013

                'DBタイムアウトエラー
                returnCode = ResultCode.ErrDBTimeout

                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                             , "{0}.{1} ERR:DBTIMEOUT RETURNCODE = {2}" _
                             , Me.GetType.ToString _
                             , System.Reflection.MethodBase.GetCurrentMethod.Name _
                             , returnCode))

            End Try

        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END RETURNCODE = {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , returnCode))

        Return returnCode

    End Function

#End Region

#Region "Privateメソッド"

    ''' <summary>
    ''' "SA登録・変更・UNDO処理(EnableCommit用)
    ''' </summary>
    ''' <param name="inRowVisitInfo">来店管理情報</param>
    ''' <param name="inStaffInfo">ログイン情報</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    <EnableCommit()>
    Private Function RegisterSACommit(ByVal inRowVisitInfo As SC3100401DataSet.VisitInfoRow, _
                                      ByVal inEventKeyID As String, _
                                      ByVal inStaffInfo As StaffContext, _
                                      ByVal inSC3100401Dac As SC3100401DataSetTableAdapters.SC3100401TableAdapter) _
                                      As Integer

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} REZID:{2} ORDERNO:{3} ACCOUNT:{4} " _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , inRowVisitInfo.REZID, inRowVisitInfo.ORDERNO, inStaffInfo.Account))
        '処理結果
        Dim returnCode As Integer = ResultCode.Success

        Try

            '登録内容確認
            Select Case inEventKeyID
                Case CType(EventKeyId.SAAssig, String),
                     CType(EventKeyId.SAChange, String)
                    'SA登録
                    'SA変更

                    'SA振当登録処理
                    returnCode = Me.RegisterSAAssig(inRowVisitInfo, inStaffInfo, inSC3100401Dac)

                Case CType(EventKeyId.SAUndo, String)
                    'SA解除

                    'SA解除処理
                    returnCode = Me.RegisterSAUndo(inRowVisitInfo, inStaffInfo, inSC3100401Dac)

                Case Else
                    '上記以外

                    'その他処理エラー
                    returnCode = ResultCode.ErrOutType

                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                 , "{0}.{1} ERR: EVENTKEYID = {2} RETURNCODE = {3}" _
                                 , Me.GetType.ToString _
                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                 , inEventKeyID, returnCode))

            End Select

            '更新処理結果
            If returnCode <> ResultCode.Success Then
                '更新失敗

                'その他処理エラー
                returnCode = ResultCode.ErrOutType

                'ロールバック
                Me.Rollback = True

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                             , "{0}.{1} ERR:EXCEPTION RETURNCODE = {2}" _
                             , Me.GetType.ToString _
                             , System.Reflection.MethodBase.GetCurrentMethod.Name _
                             , returnCode))

            End If

        Catch ex As OracleExceptionEx When ex.Number = 1013

            'DBタイムアウトエラー
            returnCode = ResultCode.ErrDBTimeout

            'ロールバック
            Me.Rollback = True

            Logger.Error(String.Format(CultureInfo.CurrentCulture _
                         , "{0}.{1} ERR:DBTIMEOUT RETURNCODE = {2}" _
                         , Me.GetType.ToString _
                         , System.Reflection.MethodBase.GetCurrentMethod.Name _
                         , returnCode))

        Catch ex As OracleExceptionEx

            'DBエラー
            returnCode = ResultCode.ErrDB

            'ロールバック
            Me.Rollback = True

            Logger.Error(String.Format(CultureInfo.CurrentCulture _
                         , "{0}.{1} ERR:DBERR RETURNCODE = {2}" _
                         , Me.GetType.ToString _
                         , System.Reflection.MethodBase.GetCurrentMethod.Name _
                         , returnCode))

        Catch ex As Exception

            'その他処理エラー
            returnCode = ResultCode.ErrOutType

            'ロールバック
            Me.Rollback = True

            Logger.Error(String.Format(CultureInfo.CurrentCulture _
                         , "{0}.{1} ERR:EXCEPTION RETURNCODE = {2}" _
                         , Me.GetType.ToString _
                         , System.Reflection.MethodBase.GetCurrentMethod.Name _
                         , returnCode))

        End Try

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END RETURNCODE = {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , returnCode))

        Return returnCode

    End Function

    ''' <summary>
    ''' SA振当登録・SA変更登録処理
    ''' </summary>
    ''' <param name="inRowVisitInfo">来店管理情報</param>
    ''' <param name="inStaffInfo">ログイン情報</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
    ''' </history>
    Private Function RegisterSAAssig(ByVal inRowVisitInfo As SC3100401DataSet.VisitInfoRow, _
                                     ByVal inStaffInfo As StaffContext, _
                                     ByVal inSC3100401Dac As SC3100401DataSetTableAdapters.SC3100401TableAdapter) _
                                     As Integer

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} REZID:{2} ORDERNO:{3} ACCOUNT:{4} " _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , inRowVisitInfo.REZID, inRowVisitInfo.ORDERNO, inStaffInfo.Account))
        '処理結果
        Dim returnCode As Integer = ResultCode.Success

        '更新件数
        Dim upDateCount As Integer = 0

        '予約の確認
        If inRowVisitInfo.REZID < 0 Then
            '予約なし

            '整備受注Noの存在確認
            If Not String.IsNullOrEmpty(inRowVisitInfo.ORDERNO) Then
                '整備受注Noが存在する

                '無効予約(-1)を設定
                inRowVisitInfo.REZID = UnavailableReserve

                '基幹システムR/Oの変更
                returnCode = Me.UpdateStallRezInfo(inRowVisitInfo, inStaffInfo, EventKeyId.SAAssig)

            End If
        Else
            '予約有り

            'ストール予約の最新情報取得
            Dim dtNewestStallRezInfo As SC3100401DataSet.NewestStallRezInfoDataTable =
                inSC3100401Dac.GetDBNewestStallRezInfo(inRowVisitInfo.REZID, inStaffInfo.DlrCD, inStaffInfo.BrnCD)

            '予約情報取得確認
            If dtNewestStallRezInfo IsNot Nothing _
                AndAlso 0 < dtNewestStallRezInfo.Count Then
                '取得成功

                'ROWに変換
                Dim rowNewestStallRezInfo As SC3100401DataSet.NewestStallRezInfoRow =
                    DirectCast(dtNewestStallRezInfo.Rows(0), SC3100401DataSet.NewestStallRezInfoRow)

                '受付担当予定者の確認
                If Not rowNewestStallRezInfo.IsACCOUNT_PLANNull Then
                    '受付担当予定者が取得できた場合

                    '最新の情報に書換え
                    inRowVisitInfo.DEFAULTSACODE = rowNewestStallRezInfo.ACCOUNT_PLAN
                End If

                '整備受注Noのチェック
                If Not rowNewestStallRezInfo.IsORDERNONull Then
                    '整備受注Noが取得できた場合

                    '最新の情報に書換え
                    inRowVisitInfo.ORDERNO = rowNewestStallRezInfo.ORDERNO
                End If

                '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

                '行更新カウント
                If Not rowNewestStallRezInfo.IsROW_LOCK_VERSIONNull Then
                    '行更新カウントが取得できた場合

                    '最新の情報に書換え
                    inRowVisitInfo.ROW_LOCK_VERSION = rowNewestStallRezInfo.ROW_LOCK_VERSION
                End If

                '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

                'ストール予約・基幹システムR/Oの変更
                returnCode = Me.UpdateStallRezInfo(inRowVisitInfo, inStaffInfo, EventKeyId.SAAssig)

                '受付担当予定者のチェック
                If Not inRowVisitInfo.DEFAULTSACODE.Equals(inRowVisitInfo.SACODE) Then
                    '取得した受付担当予定者と、選択行の「SAコード」が異なっている場合

                    '選択されたSAをデフォルトSAに設定
                    inRowVisitInfo.DEFAULTSACODE = inRowVisitInfo.SACODE

                End If
            End If
        End If

        'IFの処理結果確認
        If returnCode = ResultCode.Success Then
            '処理が成功している場合

            '来店管理テーブルの更新処理(SA振当登録・SA変更登録)
            upDateCount = inSC3100401Dac.RegisterDBAssignSA(inRowVisitInfo, inStaffInfo.Account)

        End If

        '来店管理テーブルの更新結果確認
        If 0 < upDateCount Then
            '処理が成功している場合

            '更新成功
            returnCode = ResultCode.Success

        Else
            '更新失敗

            '更新失敗
            returnCode = ResultCode.ErrOutType

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                 , "{0}.{1} ERR:EXCLUSION RETURNCODE = {2}" _
                                 , Me.GetType.ToString _
                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                 , returnCode))

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END RETURNCODE = {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , returnCode))

        Return returnCode

    End Function

    ''' <summary>
    ''' UNDO処理
    ''' </summary>
    ''' <param name="inRowVisitInfo">来店管理情報</param>
    ''' <param name="inStaffInfo">ログイン情報</param>
    ''' <param name="inSC3100401Dac">テーブルアダプター</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
    ''' </history>
    Private Function RegisterSAUndo(ByVal inRowVisitInfo As SC3100401DataSet.VisitInfoRow, _
                                    ByVal inStaffInfo As StaffContext, _
                                    ByVal inSC3100401Dac As SC3100401DataSetTableAdapters.SC3100401TableAdapter) _
                                    As Integer

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} REZID:{2} ORDERNO:{3} ACCOUNT:{4} " _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , inRowVisitInfo.REZID, inRowVisitInfo.ORDERNO, inStaffInfo.Account))
        '処理結果
        Dim returnCode As Integer = ResultCode.Success

        '更新件数
        Dim upDateCount As Integer = 0

        '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        'ストール予約の最新情報取得
        Dim dtNewestStallRezInfo As SC3100401DataSet.NewestStallRezInfoDataTable =
            inSC3100401Dac.GetDBNewestStallRezInfo(inRowVisitInfo.REZID, inStaffInfo.DlrCD, inStaffInfo.BrnCD)

        '予約情報取得確認
        If dtNewestStallRezInfo IsNot Nothing _
            AndAlso 0 < dtNewestStallRezInfo.Count Then
            '取得成功

            'ROWに変換
            Dim rowNewestStallRezInfo As SC3100401DataSet.NewestStallRezInfoRow =
                DirectCast(dtNewestStallRezInfo.Rows(0), SC3100401DataSet.NewestStallRezInfoRow)

            '行更新カウント
            If Not rowNewestStallRezInfo.IsROW_LOCK_VERSIONNull Then
                '行更新カウントが取得できた場合

                '最新の情報に書換え
                inRowVisitInfo.ROW_LOCK_VERSION = rowNewestStallRezInfo.ROW_LOCK_VERSION
            End If

        End If

        '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        'ストール予約・基幹システムR/Oの変更
        returnCode = Me.UpdateStallRezInfo(inRowVisitInfo, inStaffInfo, EventKeyId.SAUndo)

        'IFの処理結果確認
        If returnCode = ResultCode.Success Then
            '処理が成功している場合

            '来店管理テーブルの更新処理(SA解除)
            upDateCount = inSC3100401Dac.RegisterDBUndoSA(inRowVisitInfo, inStaffInfo.Account)

        End If

        '来店管理テーブルの更新結果確認
        If 0 < upDateCount Then
            '処理が成功している場合

            '更新成功
            returnCode = ResultCode.Success

        Else
            '更新失敗

            '更新失敗
            returnCode = ResultCode.ErrOutType

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                 , "{0}.{1} ERR:EXCLUSION RETURNCODE = {2}" _
                                 , Me.GetType.ToString _
                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                 , returnCode))

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END RETURNCODE = {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , returnCode))

        Return returnCode

    End Function

    ''' <summary>
    ''' ストール予約・基幹システムR/O情報更新処理
    ''' </summary>
    ''' <param name="inRowVisitInfo">来店管理情報</param>
    ''' <param name="inStaffInfo">ログイン情報</param>
    ''' <param name="inCallFlag">呼出フラグ</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
    ''' </history>
    Private Function UpdateStallRezInfo(ByVal inRowVisitInfo As SC3100401DataSet.VisitInfoRow, _
                                        ByVal inStaffInfo As StaffContext, _
                                        ByVal inCallFlag As EventKeyId) _
                                        As Integer

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} REZID:{2} ORDERNO:{3} ACCOUNT:{4} CALLFLAG:{5}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , inRowVisitInfo.REZID, inRowVisitInfo.ORDERNO, inStaffInfo.Account, inCallFlag))

        '処理結果
        Dim returnCode As Integer = ResultCode.Success

        'IF結果
        Dim retCode As Long = 0

        'SMB共通関数の宣言
        Using commonClass As New SMBCommonClassBusinessLogic

            '登録と解除で処理の分岐
            Select Case inCallFlag
                Case EventKeyId.SAAssig
                    'SA振当登録処理

                    '予約の存在確認
                    If inRowVisitInfo.REZID < 0 Then
                        '予約が存在しない顧客の場合

                        '整備受注Noが存在するか確認
                        If Not (String.IsNullOrEmpty(inRowVisitInfo.ORDERNO)) Then
                            '整備受注Noは存在する

                            '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

                            '仮予約から本予約に変更
                            'retCode = commonClass.ChangeSACode(inStaffInfo.DlrCD, _
                            '                                   inStaffInfo.BrnCD, _
                            '                                   NoReserveId, _
                            '                                   inRowVisitInfo.ORDERNO, _
                            '                                   inRowVisitInfo.SACODE,
                            '                                   RepairOrder, _
                            '                                   inRowVisitInfo.PRESENTTIME, _
                            '                                   inStaffInfo.Account, _
                            '                                   inRowVisitInfo.PRESENTTIME)

                            retCode = commonClass.ChangeSACode(inStaffInfo.DlrCD, _
                                                               inStaffInfo.BrnCD, _
                                                               NoReserveId, _
                                                               inRowVisitInfo.ORDERNO, _
                                                               inRowVisitInfo.SACODE,
                                                               RepairOrder, _
                                                               inRowVisitInfo.PRESENTTIME, _
                                                               inStaffInfo.Account, _
                                                               inRowVisitInfo.PRESENTTIME, _
                                                               ApplicationID)

                            '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

                        End If

                    Else
                        '予約が存在する場合

                        '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

                        'サービス入庫行ロック処理
                        retCode = commonClass.LockServiceInTable(inRowVisitInfo.REZID, _
                                                                 inRowVisitInfo.ROW_LOCK_VERSION, _
                                                                 "0", _
                                                                 inStaffInfo.Account, _
                                                                 inRowVisitInfo.PRESENTTIME, _
                                                                 ApplicationID)
                        '更新処理の結果確認
                        If retCode = CType(ResultCode.Success, Long) Then
                            'ストールロック成功

                            '入庫日付替え処理
                            'retCode = commonClass.ChangeCarInDate(inStaffInfo.DlrCD, _
                            '                                      inStaffInfo.BrnCD, _
                            '                                      NoReserveId, _
                            '                                      inRowVisitInfo.REZID, _
                            '                                      inRowVisitInfo.PRESENTTIME, _
                            '                                      inStaffInfo.Account, _
                            '                                      inRowVisitInfo.PRESENTTIME)

                            retCode = commonClass.ChangeCarInDate(inStaffInfo.DlrCD, _
                                                                  inStaffInfo.BrnCD, _
                                                                  NoReserveId, _
                                                                  inRowVisitInfo.REZID, _
                                                                  inRowVisitInfo.PRESENTTIME, _
                                                                  inStaffInfo.Account, _
                                                                  inRowVisitInfo.PRESENTTIME, _
                                                                  ApplicationID)

                            '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

                            '更新処理の結果確認
                            If retCode = CType(ResultCode.Success, Long) Then
                                '入庫日付替え更新成功

                                '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

                                'RezHisの登録
                                'commonClass.RegisterStallReserveHis(inStaffInfo.DlrCD, _
                                '                                    inStaffInfo.BrnCD, _
                                '                                    inRowVisitInfo.REZID, _
                                '                                    inRowVisitInfo.PRESENTTIME, _
                                '                                    RegisterType.ReserveHisAll)

                                commonClass.RegisterStallReserveHis(inStaffInfo.DlrCD, _
                                                                    inStaffInfo.BrnCD, _
                                                                    inRowVisitInfo.REZID, _
                                                                    inRowVisitInfo.PRESENTTIME, _
                                                                    RegisterType.RegisterServiceIn, _
                                                                    inStaffInfo.Account, _
                                                                    ApplicationID, _
                                                                    NoActivityId)

                                '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

                                '受付担当予定者のチェック
                                If inRowVisitInfo.DEFAULTSACODE.Equals(inRowVisitInfo.SACODE) Then
                                    '取得した受付担当予定者と、選択行の「SAコード」が等しい場合

                                    '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

                                    'R/Oの担当SA変更
                                    'retCode = commonClass.ChangeSACode(inStaffInfo.DlrCD, _
                                    '                                   inStaffInfo.BrnCD, _
                                    '                                   NoReserveId, _
                                    '                                   inRowVisitInfo.ORDERNO, _
                                    '                                   inRowVisitInfo.SACODE,
                                    '                                   RepairOrder, _
                                    '                                   inRowVisitInfo.PRESENTTIME, _
                                    '                                   inStaffInfo.Account, _
                                    '                                   inRowVisitInfo.PRESENTTIME)

                                    retCode = commonClass.ChangeSACode(inStaffInfo.DlrCD, _
                                                                       inStaffInfo.BrnCD, _
                                                                       NoReserveId, _
                                                                       inRowVisitInfo.ORDERNO, _
                                                                       inRowVisitInfo.SACODE,
                                                                       RepairOrder, _
                                                                       inRowVisitInfo.PRESENTTIME, _
                                                                       inStaffInfo.Account, _
                                                                       inRowVisitInfo.PRESENTTIME, _
                                                                       ApplicationID)

                                    '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

                                Else
                                    '取得した受付担当予定者と、選択行の「SAコード」が異なっていた場合

                                    '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

                                    'ストール予約とR/Oの担当SA変更
                                    'retCode = commonClass.ChangeSACode(inStaffInfo.DlrCD, _
                                    '                                   inStaffInfo.BrnCD, _
                                    '                                   inRowVisitInfo.REZID, _
                                    '                                   inRowVisitInfo.ORDERNO, _
                                    '                                   inRowVisitInfo.SACODE,
                                    '                                   RepairOrder, _
                                    '                                   inRowVisitInfo.PRESENTTIME, _
                                    '                                   inStaffInfo.Account, _
                                    '                                   inRowVisitInfo.PRESENTTIME)

                                    retCode = commonClass.ChangeSACode(inStaffInfo.DlrCD, _
                                                                       inStaffInfo.BrnCD, _
                                                                       inRowVisitInfo.REZID, _
                                                                       inRowVisitInfo.ORDERNO, _
                                                                       inRowVisitInfo.SACODE,
                                                                       RepairOrder, _
                                                                       inRowVisitInfo.PRESENTTIME, _
                                                                       inStaffInfo.Account, _
                                                                       inRowVisitInfo.PRESENTTIME, _
                                                                       ApplicationID)

                                    '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

                                End If
                            End If
                        End If
                    End If

                Case EventKeyId.SAUndo
                    'SA解除処理

                    '整備受注NOの存在チェック
                    If Not String.IsNullOrEmpty(inRowVisitInfo.ORDERNO) Then

                        '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START


                        '本R/Oから事前R/Oに変更
                        'retCode = commonClass.ChangeSACode(inStaffInfo.DlrCD, _
                        '                                   inStaffInfo.BrnCD, _
                        '                                   NoReserveId, _
                        '                                   inRowVisitInfo.ORDERNO, _
                        '                                   inRowVisitInfo.BEFORESACODE, _
                        '                                   PrepareFlag, _
                        '                                   inRowVisitInfo.VISITTIMESTAMP, _
                        '                                   inStaffInfo.Account, _
                        '                                   inRowVisitInfo.PRESENTTIME)

                        retCode = commonClass.ChangeSACode(inStaffInfo.DlrCD, _
                                                           inStaffInfo.BrnCD, _
                                                           NoReserveId, _
                                                           inRowVisitInfo.ORDERNO, _
                                                           inRowVisitInfo.BEFORESACODE, _
                                                           PrepareFlag, _
                                                           inRowVisitInfo.VISITTIMESTAMP, _
                                                           inStaffInfo.Account, _
                                                           inRowVisitInfo.PRESENTTIME, _
                                                           ApplicationID)

                        '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

                    End If

                    '更新結果・予約の存在確認
                    If retCode = CType(ResultCode.Success, Long) AndAlso _
                        0 < inRowVisitInfo.REZID Then
                        '更新成功かつ予約が存在する場合

                        '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

                        'サービス入庫行ロック処理
                        retCode = commonClass.LockServiceInTable(inRowVisitInfo.REZID, _
                                                                 inRowVisitInfo.ROW_LOCK_VERSION, _
                                                                 "0", _
                                                                 inStaffInfo.Account, _
                                                                 inRowVisitInfo.PRESENTTIME, _
                                                                 ApplicationID)

                        '更新処理の結果確認
                        If retCode = CType(ResultCode.Success, Long) Then
                            'ストールロック成功

                            '入庫日時付替え
                            'retCode = commonClass.ChangeCarInDate(inStaffInfo.DlrCD, _
                            '                                      inStaffInfo.BrnCD, _
                            '                                      inRowVisitInfo.REZID, _
                            '                                      NoReserveId, _
                            '                                      Date.MinValue, _
                            '                                      inStaffInfo.Account, _
                            '                                      inRowVisitInfo.PRESENTTIME)

                            retCode = commonClass.ChangeCarInDate(inStaffInfo.DlrCD, _
                                                                  inStaffInfo.BrnCD, _
                                                                  inRowVisitInfo.REZID, _
                                                                  NoReserveId, _
                                                                  Date.MinValue, _
                                                                  inStaffInfo.Account, _
                                                                  inRowVisitInfo.PRESENTTIME, _
                                                                  ApplicationID)

                            '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

                            '更新処理の結果確認
                            If retCode = CType(ResultCode.Success, Long) Then
                                '入庫日付替え更新成功

                                '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

                                'RezHisの登録
                                'commonClass.RegisterStallReserveHis(inStaffInfo.DlrCD, _
                                '                                    inStaffInfo.BrnCD, _
                                '                                    inRowVisitInfo.REZID, _
                                '                                    inRowVisitInfo.PRESENTTIME, _
                                '                                    RegisterType.ReserveHisAll)

                                commonClass.RegisterStallReserveHis(inStaffInfo.DlrCD, _
                                                                    inStaffInfo.BrnCD, _
                                                                    inRowVisitInfo.REZID, _
                                                                    inRowVisitInfo.PRESENTTIME, _
                                                                    RegisterType.RegisterServiceIn, _
                                                                    inStaffInfo.Account, _
                                                                    ApplicationID, _
                                                                    NoActivityId)

                            End If
                        End If

                        '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 EMD

                    End If
            End Select

            '結果チェック
            Select Case retCode
                Case CType(ResultCode.Success, Long)
                    '成功

                    returnCode = ResultCode.Success

                Case CType(ResultCode.ErrDBTimeout, Long)
                    'DBタイムアウト

                    returnCode = ResultCode.ErrDBTimeout

                Case Else
                    'その他エラー

                    returnCode = ResultCode.ErrOutType

            End Select

        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                          , "{0}.{1} END RETURNCODE = {2}" _
                          , Me.GetType.ToString _
                          , System.Reflection.MethodBase.GetCurrentMethod.Name _
                          , returnCode))

        Return returnCode

    End Function

#End Region

#End Region

#Region "テキストエリア「受付No・来店者・電話番号・テーブルNo」登録処理"

#Region "Publicメソッド"

    ''' <summary>
    ''' テキストエリア「受付No・来店者・電話番号・テーブルNo」登録処理
    ''' </summary>
    ''' <param name="invisitSeq">来店実績連番</param>
    ''' <param name="inupDateTime">更新日時</param>
    ''' <param name="inTextAreaID">テキストエリアID</param>
    ''' <param name="inAfterValue">更新する値</param>
    ''' <param name="inStaffInfo">ログイン情報</param>
    ''' <param name="inPresentTime">現在日時</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    <EnableCommit()>
    Public Function RegisterTextArea(ByVal inVisitSeq As Long, _
                                     ByVal inUpDateTime As Date, _
                                     ByVal inTextAreaId As String, _
                                     ByVal inAfterValue As String, _
                                     ByVal inStaffInfo As StaffContext, _
                                     ByVal inPresentTime As Date) _
                                     As Integer

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} VISITSEQ:{2} UPDATETIME:{3} AREAID:{4} AFTERVALUE:{5} ACCOUNT:{6} PRESENTTIME:{7}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , inVisitSeq, inUpDateTime, inTextAreaId, inAfterValue, inStaffInfo.Account, inPresentTime))
        '処理結果
        Dim returnCode As Integer = ResultCode.Success

        '更新件数
        Dim upDateCount As Integer = 0

        'DataSetの宣言
        Using sc3100401Dac As New SC3100401DataSetTableAdapters.SC3100401TableAdapter

            Try
                'テキストエリア「受付No・来店者・電話番号・テーブルNo」登録処理
                upDateCount = sc3100401Dac.RegisterDBTextArea(inVisitSeq, _
                                                              inUpDateTime, _
                                                              inTextAreaId, _
                                                              inAfterValue, _
                                                              inStaffInfo.Account, _
                                                              inPresentTime)

                '更新件数の確認
                If upDateCount = 0 Then
                    '更新件数0件

                    '排他エラー
                    returnCode = ResultCode.ErrExclusion

                End If


            Catch ex As OracleExceptionEx When ex.Number = 1013

                'DBタイムアウトエラー
                returnCode = ResultCode.ErrDBTimeout

                'ロールバック
                Me.Rollback = True

                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                             , "{0}.{1} ERR:DBTIMEOUT RETURNCODE = {2}" _
                             , Me.GetType.ToString _
                             , System.Reflection.MethodBase.GetCurrentMethod.Name _
                             , returnCode))

            Catch ex As OracleExceptionEx

                'DBエラー
                returnCode = ResultCode.ErrDB

                'ロールバック
                Me.Rollback = True

                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                             , "{0}.{1} ERR:DBERR RETURNCODE = {2}" _
                             , Me.GetType.ToString _
                             , System.Reflection.MethodBase.GetCurrentMethod.Name _
                             , returnCode))

            Catch ex As Exception

                'その他処理エラー
                returnCode = ResultCode.ErrOutType

                'ロールバック
                Me.Rollback = True

                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                             , "{0}.{1} ERR:EXCEPTION RETURNCODE = {2}" _
                             , Me.GetType.ToString _
                             , System.Reflection.MethodBase.GetCurrentMethod.Name _
                             , returnCode))

            End Try
        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END RETURNCODE = {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , returnCode))

        Return returnCode

    End Function

#End Region

#End Region

#Region "呼出・呼出キャンセル登録処理"

    ''' <summary>
    ''' 呼出・呼出キャンセル登録処理
    ''' </summary>
    ''' <param name="invisitSeq">来店実績連番</param>
    ''' <param name="inupDateTime">更新日時</param>
    ''' <param name="incustomFooterID">イベントキーID</param>
    ''' <param name="inAccount">更新アカウント</param>
    ''' <param name="inPresentTime">現在日時</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    <EnableCommit()>
    Public Function RegisterCallStatus(ByVal inVisitSeq As Long, _
                                       ByVal inUpDateTime As Date, _
                                       ByVal inCustomFooterId As String, _
                                       ByVal inAccount As String, _
                                       ByVal inPresentTime As Date) _
                                       As Integer

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} VISITSEQ:{2} UPDATETIME:{3} CUSTOMFOOTERID:{4} ACCOUNT:{5} PRESENTTIME:{6}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , inVisitSeq, inUpDateTime, inCustomFooterId, inAccount, inPresentTime))
        '処理結果
        Dim returnCode As Integer = ResultCode.Success

        '更新件数
        Dim upDateCount As Integer = 0

        'DataSetの宣言
        Using sc3100401Dac As New SC3100401DataSetTableAdapters.SC3100401TableAdapter

            Try
                'フッターボタンの確認
                If inCustomFooterId = CType(EventKeyId.FooterCallButton, String) Then
                    '呼出

                    '呼出登録処理
                    upDateCount = sc3100401Dac.RegisterDBCallStatus(inVisitSeq, inUpDateTime, inAccount, inPresentTime)

                Else
                    '呼出キャンセル

                    '呼出キャンセル登録処理
                    upDateCount = sc3100401Dac.RegisterDBCallCancel(inVisitSeq, inUpDateTime, inAccount, inPresentTime)

                End If

                '更新確認
                If upDateCount = 0 Then
                    '更新失敗

                    '排他エラー
                    returnCode = ResultCode.ErrExclusion

                End If

            Catch ex As OracleExceptionEx When ex.Number = 1013

                'DBタイムアウトエラー
                returnCode = ResultCode.ErrDBTimeout

                'ロールバック
                Me.Rollback = True

                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                             , "{0}.{1} ERR:DBTIMEOUT RETURNCODE = {2}" _
                             , Me.GetType.ToString _
                             , System.Reflection.MethodBase.GetCurrentMethod.Name _
                             , returnCode))

            Catch ex As OracleExceptionEx

                'DBエラー
                returnCode = ResultCode.ErrDB

                'ロールバック
                Me.Rollback = True

                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                             , "{0}.{1} ERR:DBERR RETURNCODE = {2}" _
                             , Me.GetType.ToString _
                             , System.Reflection.MethodBase.GetCurrentMethod.Name _
                             , returnCode))

            Catch ex As Exception

                'コール元の確認
                If inCustomFooterId.Equals(CType(EventKeyId.FooterCallButton, String)) Then
                    '呼出ボタンの場合

                    'その他処理エラー
                    returnCode = ResultCode.ErrCall
                Else
                    '呼出キャンセルボタンの場合

                    'その他処理エラー
                    returnCode = ResultCode.ErrCallCancel
                End If

                'ロールバック
                Me.Rollback = True

                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                             , "{0}.{1} ERR:EXCEPTION RETURNCODE = {2}" _
                             , Me.GetType.ToString _
                             , System.Reflection.MethodBase.GetCurrentMethod.Name _
                             , returnCode))

            End Try
        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END RETURNCODE = {2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , returnCode))

        Return returnCode

    End Function

#End Region

#Region "チップ削除登録処理"

    ''' <summary>
    ''' チップ削除登録処理
    ''' </summary>
    ''' <param name="invisitSeq">来店実績連番</param>
    ''' <param name="inupDateTime">更新日時</param>
    ''' <param name="inAccount">更新アカウント</param>
    ''' <param name="inPresentTime">現在日時</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    <EnableCommit()>
    Public Function RegisterTipDelete(ByVal inVisitSeq As Long, _
                                      ByVal inUpDateTime As Date, _
                                      ByVal inAccount As String, _
                                      ByVal inPresentTime As Date) _
                                      As Integer

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} VISITSEQ:{2} UPDATETIME:{3} ACCOUNT:{4} PRESENTTIME:{5}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , inVisitSeq, inUpDateTime, inAccount, inPresentTime))
        '処理結果
        Dim returnCode As Integer = ResultCode.Success

        '更新件数
        Dim upDateCount As Integer = 0

        'DataSetの宣言
        Using sc3100401Dac As New SC3100401DataSetTableAdapters.SC3100401TableAdapter

            Try
                'チップ削除登録(退店)処理
                upDateCount = sc3100401Dac.RegisterDBTipDelete(inVisitSeq, inUpDateTime, inAccount, inPresentTime)

                '更新確認
                If upDateCount = 0 Then
                    '更新失敗

                    '排他エラー
                    returnCode = ResultCode.ErrExclusion

                End If

            Catch ex As OracleExceptionEx When ex.Number = 1013

                'DBタイムアウトエラー
                returnCode = ResultCode.ErrDBTimeout

                'ロールバック
                Me.Rollback = True

                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                             , "{0}.{1} ERR:DBTIMEOUT RETURNCODE = {2}" _
                             , Me.GetType.ToString _
                             , System.Reflection.MethodBase.GetCurrentMethod.Name _
                             , returnCode))

            Catch ex As OracleExceptionEx

                'DBエラー
                returnCode = ResultCode.ErrDB

                'ロールバック
                Me.Rollback = True

                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                             , "{0}.{1} ERR:DBERR RETURNCODE = {2}" _
                             , Me.GetType.ToString _
                             , System.Reflection.MethodBase.GetCurrentMethod.Name _
                             , returnCode))

            Catch ex As Exception

                'その他処理エラー
                returnCode = ResultCode.ErrTipDelete

                'ロールバック
                Me.Rollback = True

                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                             , "{0}.{1} ERR:EXCEPTION RETURNCODE = {2}" _
                             , Me.GetType.ToString _
                             , System.Reflection.MethodBase.GetCurrentMethod.Name _
                             , returnCode))

            End Try
        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END RETURNCODE = {2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , returnCode))

        Return returnCode

    End Function

#End Region

#Region "共通"

#Region "通知"

#Region "通知用定数"

    ''' <summary>
    ''' 通知API用(カテゴリータイプ)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NotifyPushCategory As String = "1"

    ''' <summary>
    ''' 通知API用(表示位置)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NotifyPotisionType As String = "1"

    ''' <summary>
    ''' 通知API用(表示時間)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NotifyTime As Integer = 3

    ''' <summary>
    ''' 通知API用(表示タイプ)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NotifyDispType As String = "1"

    ''' <summary>
    ''' 通知API用(色)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NotifyColor As String = "1"

    ''' <summary>
    ''' 通知API用(呼び出し関数)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NotifyDispFunction As String = "icropScript.ui.setNotice()"

    ''' <summary>
    ''' 通知履歴のSessionValue(カンマ)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueKanma As String = ","

    '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
    ' ''' <summary>
    ' ''' 通知履歴のSessionValue(来店管理連番)
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const SessionValueVisitSequence As String = "Redirect.VISITSEQ,String,"

    ' ''' <summary>
    ' ''' 通知履歴のSessionValue(車両登録番号)
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const SessionValueRegisterNo As String = "Redirect.REGISTERNO,String,"

    ' ''' <summary>
    ' ''' 通知履歴のSessionValue(名前)
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const SessionValueName As String = "Redirect.NAME,String,"

    ' ''' <summary>
    ' ''' 通知履歴のSessionValue(VIN)
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const SessionValueVinNo As String = "Redirect.VINNO,String,"

    ' ''' <summary>
    ' ''' 通知履歴のSessionValue(モデルコード)
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const SessionValueModelCode As String = "Redirect.MODELCODE,String,"

    ' ''' <summary>
    ' ''' 通知履歴のSessionValue(電話番号)
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const SessionValueTelNo1 As String = "Redirect.TEL1,String,"

    ' ''' <summary>
    ' ''' 通知履歴のSessionValue(携帯番号)
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const SessionValueTelNo2 As String = "Redirect.TEL2,String,"

    ' ''' <summary>
    ' ''' 通知履歴のSessionValue(予約ID)
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const SessionValueReserveId As String = "Redirect.REZID,String,"

    ' ''' <summary>
    ' ''' 通知履歴のSessionValue(事前準備フラグ)
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const SessionValuePrepareChipType As String = "Redirect.PREPARECHIPFLAG,String,"

    ' ''' <summary>
    ' ''' 通知履歴のSessionValue(販売店コード)
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const SessionValueDealerCode As String = "Redirect.CRDEALERCODE,String,"

    ' ''' <summary>
    ' ''' 通知履歴のSessionValue(顧客詳細フラグ)
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const SessionValueType As String = "Redirect.FLAG,String,"

    ''' <summary>
    ''' 通知履歴のSessionValue(VIN)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueVinNo As String = "SessionKey.VIN,String,"

    ''' <summary>
    ''' 通知履歴のSessionValue(基幹顧客ID)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueDmsId As String = "SessionKey.DMS_CST_ID,String,"

    '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

    '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START

    ' ''' <summary>
    ' ''' 未取引客のリンク文字列
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const NewCUstomerLink As String = "<a id='SC30802070' Class='SC3080207' href='/Website/Pages/SC3080207.aspx' onclick='return ServiceLinkClick(event)'>"

    ' ''' <summary>
    ' ''' 自社客のリンク文字列
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const CustomerLink As String = "<a id='SC30802080' Class='SC3080208' href='/Website/Pages/SC3080208.aspx' onclick='return ServiceLinkClick(event)'>"

    ''' <summary>
    ''' 自社客のリンク文字列
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CustomerLink As String = "<a id='SC30802250' Class='SC3080225' href='/Website/Pages/SC3080225.aspx' onclick='return ServiceLinkClick(event)'>"

    '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

    ''' <summary>
    ''' Aタグ終了文字列
    ''' </summary>
    ''' <remarks></remarks>
    Private Const EndLikTag As String = "</a>"

    ''' <summary>
    ''' リフレッシュ通知のPush情報(SAメイン：全体)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RefreshSAPushInfo As String = "cat=action&type=main&sub=js&uid=#USER_ACCOUNT#&js1=MainRefresh()"

    '2013/08/27 TMEJ 小澤 IT9559_タブレット版SMB機能開発(IF開発) START

    ''' <summary>
    ''' リフレッシュ通知のPush情報(CT、FM)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RefreshCTAndFMPushInfo As String = "cat=action&type=main&sub=js&uid=#USER_ACCOUNT#&js1=RefreshSMB()"

    ''' <summary>
    ''' リフレッシュ通知のPush情報(来店管理)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RefreshSVRPushInfo As String = "cat=action&type=main&sub=js&uid=#USER_ACCOUNT#&js1=Send_Visit()"

    ''' <summary>
    ''' リフレッシュ通知のPush情報(SAメイン：未振当てエリア)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RefreshSAPushInfoAssginmentArea As String = "cat=action&type=main&sub=js&uid=#USER_ACCOUNT#&js1=AssignmentRefresh()"

    '2013/08/27 TMEJ 小澤 IT9559_タブレット版SMB機能開発(IF開発) END

    ''' <summary>
    ''' リフレッシュ通知のAccount置換用文字列
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RefreshSAReplaceWord As String = "#USER_ACCOUNT#"

    '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START

    ' ''' <summary>
    ' ''' 作成するメッセージフラグ
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Enum MessageType

    '    ''' <summary>
    '    ''' 未取引客
    '    ''' </summary>
    '    ''' <remarks></remarks>
    '    NewCustomer = 0

    '    ''' <summary>
    '    ''' 自社客かつ車両登録No情報有
    '    ''' </summary>
    '    ''' <remarks></remarks>
    '    CustomerRegsterNo = 1

    '    ''' <summary>
    '    ''' 自社客かつ車両登録No.情報無
    '    ''' </summary>
    '    ''' <remarks></remarks>
    '    CustomerNoRegsterNo = 2

    'End Enum

    ''' <summary>
    ''' アンカーフラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Enum AnchorType

        ''' <summary>
        ''' アンカー無し
        ''' </summary>
        ''' <remarks></remarks>
        AnchorNone = 0

        ''' <summary>
        ''' アンカー有り
        ''' </summary>
        ''' <remarks></remarks>
        AnchorSet = 1

    End Enum

    ''' <summary>
    ''' 敬称フラグ(1：後ろにつける)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NameTitlePositionTypeBefore As String = "1"

    '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END


#End Region

#Region "Publicメソッド"

    ''' <summary>
    ''' 通知処理
    ''' </summary>
    ''' <param name="inRowVisitInfo">来店者情報欄表示情報</param>
    ''' <param name="inStaffInfo">ログイン情報</param>
    ''' <param name="inEventKey">イベント情報</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2013/08/27 TMEJ 小澤 IT9559_タブレット版SMB機能開発(IF開発)
    ''' </history>
    Public Sub NoticeProcessing(ByVal inRowVisitInfo As SC3100401DataSet.VisitInfoRow, _
                                ByVal inStaffInfo As StaffContext, _
                                ByVal inEventKey As String)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'イベント情報判定
        Select Case inEventKey
            Case CType(EventKeyId.SAAssig, String)
                'SA振当の場合

                'SA振当通知処理の実行
                Me.NoticeMainProcessing(inRowVisitInfo, inStaffInfo, EventKeyId.SAAssig)

                'SA画面リフレッシュ通知
                '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
                'Me.SendPushServerSA(inRowVisitInfo.SACODE)
                Me.RefreshSA(inStaffInfo, inRowVisitInfo)
                '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

                '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
                'CT、CHT権限の工程管理画面リフレッシュ通知

                '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 START
                'Me.RefreshCTAndCht(inStaffInfo)
                Me.RefreshCTAndCht(inStaffInfo, inRowVisitInfo)
                '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 END

                '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

            Case CType(EventKeyId.SAChange, String)
                'SA変更

                'SA解除通知処理の実行
                Me.NoticeMainProcessing(inRowVisitInfo, inStaffInfo, EventKeyId.SAAssig)

                '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
                ''SA画面リフレッシュ通知
                'Me.SendPushServerSA(inRowVisitInfo.BEFORESACODE)
                Me.SendPushServerSA(inRowVisitInfo.BEFORESACODE, SARefreshTypeAll)
                '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

                'SA振当通知処理の実行
                Me.NoticeMainProcessing(inRowVisitInfo, inStaffInfo, EventKeyId.SAUndo)

                'SA画面リフレッシュ通知
                '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
                'Me.SendPushServerSA(inRowVisitInfo.SACODE)
                Me.SendPushServerSA(inRowVisitInfo.SACODE, SARefreshTypeAll)
                '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

                '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
                'CT、CHT権限の工程管理画面リフレッシュ通知
                '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 START
                'Me.RefreshCTAndCht(inStaffInfo)
                Me.RefreshCTAndCht(inStaffInfo, inRowVisitInfo)
                '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 END
                '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

            Case CType(EventKeyId.SAUndo, String)
                'SA解除

                'SA解除通知処理の実行
                Me.NoticeMainProcessing(inRowVisitInfo, inStaffInfo, EventKeyId.SAUndo)

                'SA画面リフレッシュ通知
                '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
                'Me.SendPushServerSA(inRowVisitInfo.BEFORESACODE)
                Me.RefreshSA(inStaffInfo, inRowVisitInfo)
                '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        End Select

        '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
        ''2013/08/27 TMEJ 小澤 IT9559_タブレット版SMB機能開発(IF開発) START
        ''CT、FMの全アカウントに工程管理画面リフレッシュ通知を送る
        'Using biz As New IC3810601BusinessLogic
        '    'CT、FM情報取得用の権限リストを作成
        '    Dim operationCodeList As New List(Of Long)
        '    operationCodeList.Add(Operation.CT)
        '    operationCodeList.Add(Operation.FM)

        '    'CT、FM権限のアカウント情報を取得する
        '    Dim dtAcknowledgeStaffList As IC3810601DataSet.AcknowledgeStaffListDataTable = _
        '        biz.GetAcknowledgeStaffList(inStaffInfo.DlrCD, _
        '                                    inStaffInfo.BrnCD, _
        '                                    operationCodeList)

        '    '件数分を通知をする
        '    For Each drAcknowledgeStaffList As IC3810601DataSet.AcknowledgeStaffListRow In dtAcknowledgeStaffList

        '        'CT、FM画面リフレッシュ通知
        '        Me.SendPushServerCTAndFM(drAcknowledgeStaffList.ACCOUNT)

        '    Next

        'End Using
        ''2013/08/27 TMEJ 小澤 IT9559_タブレット版SMB機能開発(IF開発) END
        '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START

    ''' <summary>
    ''' SA権限のSAメインメニュー画面再描画Push処理
    ''' </summary>
    ''' <param name="inStaffInfo">ログイン情報</param>
    ''' <param name="inRowVisitInfo">来店情報</param>
    ''' <remarks></remarks>
    Public Sub RefreshSA(ByVal inStaffInfo As StaffContext, _
                         ByVal inRowVisitInfo As SC3100401DataSet.VisitInfoRow)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Using biz As New IC3810601BusinessLogic
            'SA情報取得用の権限リストを作成
            Dim operationCodeList As New List(Of Long)
            operationCodeList.Add(Operation.SA)

            'SA権限のアカウント情報を取得する
            Dim dtAcknowledgeStaffList As IC3810601DataSet.AcknowledgeStaffListDataTable = _
                biz.GetAcknowledgeStaffList(inStaffInfo.DlrCD, _
                                            inStaffInfo.BrnCD, _
                                            operationCodeList)

            '件数分を通知をする
            For Each drAcknowledgeStaffList As IC3810601DataSet.AcknowledgeStaffListRow In dtAcknowledgeStaffList

                '送信先の情報チェック
                If inRowVisitInfo IsNot Nothing Then
                    '指定のアカウントが存在する場合
                    'アカウントのチェック
                    If Not (inRowVisitInfo.IsSACODENull) AndAlso inRowVisitInfo.SACODE.Equals(drAcknowledgeStaffList.ACCOUNT) Then
                        '振当て先アカウントと同じアカウントの場合
                        'SAメインメニュー画面リフレッシュ通知
                        Me.SendPushServerSA(drAcknowledgeStaffList.ACCOUNT, SARefreshTypeAll)

                    ElseIf Not (inRowVisitInfo.IsBEFORESACODENull) AndAlso inRowVisitInfo.BEFORESACODE.Equals(drAcknowledgeStaffList.ACCOUNT) Then
                        '振当て前アカウントと同じアカウントの場合
                        'SAメインメニュー画面リフレッシュ通知
                        Me.SendPushServerSA(drAcknowledgeStaffList.ACCOUNT, SARefreshTypeAll)

                    Else
                        '上記外の場合
                        'SAメインメニュー画面の未振当てエリアリフレッシュ通知
                        Me.SendPushServerSA(drAcknowledgeStaffList.ACCOUNT, SARefreshTypeAssignment)

                    End If

                Else
                    '
                    'SAメインメニュー画面の未振当てエリアリフレッシュ通知
                    Me.SendPushServerSA(drAcknowledgeStaffList.ACCOUNT, SARefreshTypeAssignment)

                End If

            Next

        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' SVR権限の来店管理画面再描画Push処理
    ''' </summary>
    ''' <param name="inStaffInfo">ログイン情報</param>
    ''' <remarks></remarks>
    Public Sub RefreshSvr(ByVal inStaffInfo As StaffContext)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Using biz As New IC3810601BusinessLogic
            'SVR情報取得用の権限リストを作成
            Dim operationCodeList As New List(Of Long)
            operationCodeList.Add(Operation.SVR)

            'SVR権限のアカウント情報を取得する
            Dim dtAcknowledgeStaffList As IC3810601DataSet.AcknowledgeStaffListDataTable = _
                biz.GetAcknowledgeStaffList(inStaffInfo.DlrCD, _
                                            inStaffInfo.BrnCD, _
                                            operationCodeList)

            '件数分を通知をする
            For Each drAcknowledgeStaffList As IC3810601DataSet.AcknowledgeStaffListRow In dtAcknowledgeStaffList

                '来店管理画面リフレッシュ通知
                Me.SendPushServerSVR(drAcknowledgeStaffList.ACCOUNT)

            Next

        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 START
    ' ''' <summary>
    ' ''' CT、CHT権限の工程管理画面再描画Push処理
    ' ''' </summary>
    ' ''' <param name="inStaffInfo">ログイン情報</param>
    ' ''' <remarks></remarks>
    'Public Sub RefreshCTAndCht(ByVal inStaffInfo As StaffContext)
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                , "{0}.{1} START" _
    '                , Me.GetType.ToString _
    '                , System.Reflection.MethodBase.GetCurrentMethod.Name))

    '    Using biz As New IC3810601BusinessLogic
    '        'CT、CHT情報取得用の権限リストを作成
    '        Dim operationCodeList As New List(Of Long)
    '        operationCodeList.Add(Operation.CT)
    '        operationCodeList.Add(Operation.CHT)

    '        'CT、CHT権限のアカウント情報を取得する
    '        Dim dtAcknowledgeStaffList As IC3810601DataSet.AcknowledgeStaffListDataTable = _
    '            biz.GetAcknowledgeStaffList(inStaffInfo.DlrCD, _
    '                                        inStaffInfo.BrnCD, _
    '                                        operationCodeList)

    '        '件数分を通知をする
    '        For Each drAcknowledgeStaffList As IC3810601DataSet.AcknowledgeStaffListRow In dtAcknowledgeStaffList

    '            'CT、CHT画面リフレッシュ通知
    '            Me.SendPushServerCTAndCHT(drAcknowledgeStaffList.ACCOUNT)

    '        Next

    '    End Using

    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                , "{0}.{1} END" _
    '                , Me.GetType.ToString _
    '                , System.Reflection.MethodBase.GetCurrentMethod.Name))
    'End Sub
    '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 END

    '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

    '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 START

    ''' <summary>
    ''' CT、CHT権限の工程管理画面再描画Push処理
    ''' </summary>
    ''' <param name="inStaffInfo">ログインスタッフ情報</param>
    ''' <param name="inRowVisitInfo">SA振当用来店管理情報</param>
    ''' <remarks></remarks>
    Public Sub RefreshCTAndCht(ByVal inStaffInfo As StaffContext, ByVal inRowVisitInfo As SC3100401DataSet.VisitInfoRow)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Using biz As New ServiceCommonClassBusinessLogic

            ' サービス入庫IDよりストールリストを取得
            Dim stallListDataTable As ServiceCommonClassDataSet.StallInfoDataTable
            stallListDataTable = biz.GetStallListToReserve(inRowVisitInfo.REZID, ResultsFlgOn, CancelFlgOff)

            ' ストールIDのリスト生成
            Dim stallIdList As List(Of Decimal) = New List(Of Decimal)
            For Each row As ServiceCommonClassDataSet.StallInfoRow In stallListDataTable.Rows
                stallIdList.Add(row.STALL_ID)
            Next

            ' ストールIDリストが存在しない場合処理を終了する
            If stallIdList.Count <= 0 Then
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} END It does not exist StallId of interest" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return
            End If

            ' ストールIDよりPush送信先のユーザー情報(CT、Cht)を取得する
            Dim operationCodeList As New List(Of Decimal)
            operationCodeList.Add(Operation.CT)
            operationCodeList.Add(Operation.CHT)

            Dim staffInfoDataTable As ServiceCommonClassDataSet.StaffInfoDataTable
            staffInfoDataTable = biz.GetNoticeSendAccountListToStall(inStaffInfo.DlrCD, inStaffInfo.BrnCD, stallIdList, operationCodeList)

            ' 取得ユーザーに対しPush通知を送信する
            For Each row As ServiceCommonClassDataSet.StaffInfoRow In staffInfoDataTable.Rows
                'CT、CHT画面リフレッシュ通知
                Me.SendPushServerCTAndCHT(row.ACCOUNT)
            Next
        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub
    '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 END
#End Region

#Region "Privateメソッド"

    ''' <summary>
    ''' 通知メイン処理
    ''' </summary>
    ''' <param name="inRowVisitInfo">来店者情報欄表示情報</param>
    ''' <param name="inStaffInfo">ログイン情報</param>
    ''' <param name="inEventKey">イベント情報</param>
    ''' <remarks></remarks>
    Private Sub NoticeMainProcessing(ByVal inRowVisitInfo As SC3100401DataSet.VisitInfoRow, _
                                     ByVal inStaffInfo As StaffContext, _
                                     ByVal inEventKey As EventKeyId)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '送信先アカウント情報設定
        Dim account As XmlAccount = Me.CreateAccount(inRowVisitInfo, inEventKey)

        '通知履歴登録情報の設定
        Dim requestNotice As XmlRequestNotice = Me.CreateRequestNotice(inRowVisitInfo, inStaffInfo, inEventKey)

        'Push内容設定
        Dim pushInfo As XmlPushInfo = Me.CreatePushInfo(inRowVisitInfo, inEventKey)

        '設定したものを格納し、通知APIをコール
        Using noticeData As New XmlNoticeData

            '現在時間データの格納
            noticeData.TransmissionDate = inRowVisitInfo.PRESENTTIME
            '送信ユーザーデータ格納
            noticeData.AccountList.Add(account)
            '通知履歴用のデータ格納
            noticeData.RequestNotice = requestNotice
            'Pushデータ格納
            noticeData.PushInfo = pushInfo

            '通知処理実行
            Using ic3040801Biz As New IC3040801BusinessLogic

                '通知処理実行
                ic3040801Biz.NoticeDisplay(noticeData, NoticeDisposal.GeneralPurpose)

            End Using
        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' Push情報作成メソッド
    ''' </summary>
    ''' <param name="inRowVisitInfo"></param>
    ''' <param name="inEventKey"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CreatePushInfo(ByVal inRowVisitInfo As SC3100401DataSet.VisitInfoRow, _
                                    ByVal inEventKey As EventKeyId) As XmlPushInfo

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

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
            pushInfo.DisplayContents = Me.CreatePusuMessage(inRowVisitInfo, inEventKey)
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
    ''' <param name="inRowVisitInfo"></param>
    ''' <param name="inStaffInfo"></param>
    ''' <param name="inEventKey"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CreateRequestNotice(ByVal inRowVisitInfo As SC3100401DataSet.VisitInfoRow, _
                                         ByVal inStaffInfo As StaffContext, _
                                         ByVal inEventKey As EventKeyId) As XmlRequestNotice

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Using requestNotice As New XmlRequestNotice

            '作成するメッセージの種類
            '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
            'Dim kindNumber As MessageType
            Dim anchor As AnchorType
            '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

            '自社客判断
            If Not (inRowVisitInfo.IsCUSTSEGMENTNull) _
                AndAlso CustSegmentMyCustomer.Equals(inRowVisitInfo.CUSTSEGMENT) Then
                '自社客の場合

                '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
                ''車両登録Noの確認
                'If Not inRowVisitInfo.IsVCLREGNONull _
                '    AndAlso Not inRowVisitInfo.IsVCLREGNONull.Equals(String.Empty) Then
                '    '車両登録番号がある場合

                '    '「1:自社客かつ車両登録No情報有」を設定
                '    kindNumber = MessageType.CustomerRegsterNo

                'Else
                '    '車両登録番号がない場合

                '    '「2:自社客かつ車両登録No.情報無」を設定
                '    kindNumber = MessageType.CustomerNoRegsterNo

                'End If
                '基幹顧客IDの確認
                If Not inRowVisitInfo.IsDMSIDNull _
                    AndAlso Not String.IsNullOrEmpty(inRowVisitInfo.DMSID) Then
                    '基幹顧客IDがある場合

                    '「1:アンカー有り」を設定
                    anchor = AnchorType.AnchorSet

                Else
                    '基幹顧客IDがない場合

                    '「0:アンカー無し」を設定
                    anchor = AnchorType.AnchorNone

                End If
                '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START

            Else
                '未取引客の場合

                '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
                ''「0:未取引客」を設定
                'kindNumber = MessageType.NewCustomer
                '「0:アンカー無し」を設定
                anchor = AnchorType.AnchorNone
                '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

            End If

            '販売店コード設定
            requestNotice.DealerCode = inStaffInfo.DlrCD
            '店舗コード設定
            requestNotice.StoreCode = inStaffInfo.BrnCD
            'スタッフコード(送信元)設定
            requestNotice.FromAccount = inStaffInfo.Account
            'スタッフ名(送信元)設定
            requestNotice.FromAccountName = inStaffInfo.UserName
            '表示内容設定
            '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
            requestNotice.Message = Me.CreateNoticeRequestMessage(inRowVisitInfo, anchor, inEventKey)
            '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END
            'セッション設定値設定
            '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
            'requestNotice.SessionValue = Me.CreateNoticeRequestSession(inRowVisitInfo, inStaffInfo, kindNumber)
            requestNotice.SessionValue = Me.CreateNoticeRequestSession(inRowVisitInfo, anchor)
            '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} START" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return requestNotice
        End Using
    End Function

    ''' <summary>
    ''' 送信先アカウント情報作成メソッド
    ''' </summary>
    ''' <param name="inRowVisitInfo"></param>
    ''' <param name="inEventKey"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CreateAccount(ByVal inRowVisitInfo As SC3100401DataSet.VisitInfoRow, _
                                   ByVal inEventKey As EventKeyId) As XmlAccount

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Using account As New XmlAccount

            Dim usersClass As New Users

            Dim rowUsers As UsersDataSet.USERSRow

            'イベント情報判定
            Select Case inEventKey
                Case EventKeyId.SAAssig
                    'SA振当の場合

                    'SACODEでユーザー情報の取得
                    rowUsers = usersClass.GetUser(inRowVisitInfo.SACODE, DelFlgNone)

                    '受信先のアカウント設定
                    account.ToAccount = rowUsers.ACCOUNT

                    '受信者名設定
                    account.ToAccountName = rowUsers.USERNAME

                Case EventKeyId.SAUndo
                    'SA解除の場合

                    'BEFORESACODEでユーザー情報の取得
                    rowUsers = usersClass.GetUser(inRowVisitInfo.BEFORESACODE, DelFlgNone)

                    '受信先のアカウント設定
                    account.ToAccount = rowUsers.ACCOUNT

                    '受信者名設定
                    account.ToAccountName = rowUsers.USERNAME

            End Select

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))

            Return account

        End Using
    End Function

    ''' <summary>
    ''' 通知履歴用メッセージ作成メソッド
    ''' </summary>
    ''' <param name="inRowVisitInfo">来店者情報欄表示情報</param>
    ''' <param name="inAnchor">アンカーフラグ「0:アンカー無し、1:アンカー有り」</param>
    ''' <param name="inEventKey">イベント情報</param>
    ''' <returns>作成したメッセージ文言</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発
    ''' 2017/02/03 NSK 加藤 TR-SVT-TMT-20161019-001 jobをオープンしたがチップのステータスが変更しない
    ''' </history>
    Private Function CreateNoticeRequestMessage(ByVal inRowVisitInfo As SC3100401DataSet.VisitInfoRow, _
                                                ByVal inAnchor As AnchorType, _
                                                ByVal inEventKey As EventKeyId) As String
        '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
        'Private Function CreateNoticeRequestMessage(ByVal inRowVisitInfo As SC3100401DataSet.VisitInfoRow, _
        '                                            ByVal inKindNumber As MessageType, _
        '                                            ByVal inEventKey As EventKeyId) As String
        '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim workMessage As New StringBuilder

        'メッセージ組立：リンク(開始)作成

        '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
        ''メッセージ種別の判定
        'Select Case inKindNumber
        '    Case MessageType.NewCustomer
        '        '「0:未取引客」の場合

        '        '未取引のAタグを設定
        '        workMessage.Append(NewCUstomerLink)

        '    Case MessageType.CustomerRegsterNo
        '        '「1:自社客かつ車両登録No有」の場合

        '        '自社客のAタグを設定
        '        workMessage.Append(CustomerLink)

        'End Select

        ''メッセージ組立：車両登録番号
        'If Not (inRowVisitInfo.IsVCLREGNONull) _
        '    AndAlso Not (String.IsNullOrEmpty(inRowVisitInfo.VCLREGNO)) Then
        '    '車両登録番号がある場合

        '    '車両登録番号を設定
        '    workMessage.Append(inRowVisitInfo.VCLREGNO)

        'End If

        ''メッセージ組立：リンク(終了)作成
        'If inKindNumber <> MessageType.CustomerNoRegsterNo Then
        '    '「2:自社客かつ車両登録No無」でない場合

        '    'Aタグ終了を設定
        '    workMessage.Append(EndLikTag)

        'End If

        ''メッセージ間にスペースの設定
        'workMessage.Append(Space(3))

        ''メッセージ組立：サービスコード+来店情報
        ''メッセージ種別の判定
        'Select Case inEventKey
        '    Case EventKeyId.SAAssig
        '        '「SA振当登録」の場合

        '        'サービスコードのチェック
        '        If ServiceCodeTeiki.Equals(inRowVisitInfo.SERVICECODE) Then
        '            '「定期点検」の場合

        '            '定期点検を設定
        '            workMessage.Append(WebWordUtility.GetWord(ApplicationID, WordId.Id038))

        '        ElseIf ServiceCodeIppan.Equals(inRowVisitInfo.SERVICECODE) Then
        '            '「一般整備」の場合

        '            '一般整備を設定
        '            workMessage.Append(WebWordUtility.GetWord(ApplicationID, WordId.Id037))

        '        End If

        '        'メッセージ間にスペースの設定
        '        workMessage.Append(Space(3))
        '        'ご来店を設定
        '        workMessage.Append(WebWordUtility.GetWord(ApplicationID, WordId.Id041))

        '    Case EventKeyId.SAUndo
        '        '「UNDO」の場合

        '        'ご来店キャンセルを設定
        '        workMessage.Append(WebWordUtility.GetWord(ApplicationID, WordId.Id042))

        'End Select

        ''メッセージ間にスペースの設定
        'workMessage.Append(Space(3))

        ''メッセージ組立：お客様名
        'If Not (inRowVisitInfo.IsNAMENull) _
        '    AndAlso Not (String.IsNullOrEmpty(inRowVisitInfo.NAME)) Then
        '    '名前がある場合

        '    '名前＋「様」を設定
        '    workMessage.Append(inRowVisitInfo.NAME)
        '    workMessage.Append(WebWordUtility.GetWord(ApplicationID, WordId.Id039))

        'Else
        '    'データがない場合

        '    '「お客様」を設定
        '    workMessage.Append(WebWordUtility.GetWord(ApplicationID, WordId.Id040))

        'End If

        'メッセージ組立：「振当て」「振当てキャンセル」
        Select Case inEventKey
            Case EventKeyId.SAAssig
                '「SA振当登録」の場合
                '「振当て」を設定
                workMessage.Append(WebWordUtility.GetWord(ApplicationID, WordId.Id041))

            Case EventKeyId.SAUndo
                '「UNDO」の場合
                '「振当てキャンセル」を設定
                workMessage.Append(WebWordUtility.GetWord(ApplicationID, WordId.Id042))

        End Select

        'メッセージ組立：スペース
        workMessage.Append(Space(3))

        'メッセージ組立：顧客詳細Aタグ(開始)
        If inAnchor = AnchorType.AnchorSet Then
            '「1:アンカー有り」の場合
            'Aタグ開始を設定
            workMessage.Append(CustomerLink)

        End If

        'メッセージ組立：車両登録番号
        If Not (inRowVisitInfo.IsVCLREGNONull) _
            AndAlso Not (String.IsNullOrEmpty(inRowVisitInfo.VCLREGNO)) Then
            '車両登録番号がある場合
            '車両登録番号を設定
            workMessage.Append(inRowVisitInfo.VCLREGNO)

        End If

        'メッセージ組立：スペース
        workMessage.Append(Space(3))

        'メッセージ組立：お客様名
        If Not (inRowVisitInfo.IsNAMENull) _
            AndAlso Not (String.IsNullOrEmpty(inRowVisitInfo.NAME)) Then
            '名前がある場合
            If Not (inRowVisitInfo.IsNAMETITLE_NAMENull) Then
                '敬称が存在する場合
                If NameTitlePositionTypeBefore.Equals(inRowVisitInfo.POSITION_TYPE) Then
                    '敬称を後ろに設定する場合
                    workMessage.Append(inRowVisitInfo.NAME)
                    workMessage.Append(inRowVisitInfo.NAMETITLE_NAME)

                Else
                    '敬称を前に設定する場合
                    workMessage.Append(inRowVisitInfo.NAMETITLE_NAME)
                    workMessage.Append(inRowVisitInfo.NAME)

                End If

            Else
                '敬称が存在しない場合
                workMessage.Append(inRowVisitInfo.NAME)

            End If

        Else
            'データがない場合
            '「お客様」を設定
            workMessage.Append(WebWordUtility.GetWord(ApplicationID, WordId.Id040))

        End If

        'メッセージ組立：顧客詳細Aタグ(終了)
        If inAnchor = AnchorType.AnchorSet Then
            '「1:アンカー有り」の場合
            'Aタグ終了を設定
            workMessage.Append(EndLikTag)

        End If

        'メッセージ組立：予約情報
        '予約IDチェック
        ' 2017/01/23 NSK 加藤 TR-SVT-TMT-20161019-001 jobをオープンしたがチップのステータスが変更しない START
        If Not (inRowVisitInfo.IsREZIDNull) AndAlso 0 < inRowVisitInfo.REZID AndAlso
            Not (inRowVisitInfo.IsSTART_DATETIMENull) Then
            ' 2017/01/23 NSK 加藤 TR-SVT-TMT-20161019-001 jobをオープンしたがチップのステータスが変更しない END
            '予約IDが存在している場合
            'メッセージ組立：スペース
            workMessage.Append(Space(3))

            'メッセージ組立：開始日時
            workMessage.Append(String.Format(CultureInfo.InvariantCulture, "{0:HH:mm}", inRowVisitInfo.START_DATETIME))

            'メッセージ組立：「～」文言
            workMessage.Append(WebWordUtility.GetWord(ApplicationID, WordId.Id035))

            'メッセージ組立：終了日時
            workMessage.Append(String.Format(CultureInfo.InvariantCulture, "{0:HH:mm}", inRowVisitInfo.END_DATETIME))

            '整備名チェック
            If Not (inRowVisitInfo.IsMERCHANDISENAMENull) AndAlso _
               Not (String.IsNullOrEmpty(inRowVisitInfo.MERCHANDISENAME)) Then
                'データが存在する場合
                'メッセージ組立：スペース
                workMessage.Append(Space(3))

                'メッセージ組立：整備名
                workMessage.Append(inRowVisitInfo.MERCHANDISENAME)

            End If

        End If

        '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        '戻り値設定
        Dim notifyMessage As String = workMessage.ToString()

        '開放処理
        workMessage = Nothing

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return notifyMessage

    End Function

    ''' <summary>
    ''' 通知履歴用セッション情報作成メソッド
    ''' </summary>
    ''' <param name="inRowVisitInfo">来店者情報表示欄</param>
    ''' <param name="inAnchor">アンカーフラグ「0:アンカー無し、1:アンカー有り」</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発
    ''' </history>
    Private Function CreateNoticeRequestSession(ByVal inRowVisitInfo As SC3100401DataSet.VisitInfoRow, _
                                                ByVal inAnchor As AnchorType) As String
        '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
        'Private Function CreateNoticeRequestSession(ByVal inRowVisitInfo As SC3100401DataSet.VisitInfoRow, _
        '                                            ByVal inStaffInfo As StaffContext, _
        '                                            ByVal inKindNumber As MessageType) As String
        '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim notifySession As String = String.Empty

        'メッセージ種別判定
        '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
        'Select Case inKindNumber
        '    Case MessageType.NewCustomer
        '        「0:未取引客」の場合

        '        '未取引客のセッション情報を作成
        '        notifySession = CreateNewCustomerSession(inRowVisitInfo)

        '    Case MessageType.CustomerRegsterNo
        '        「1:自社客かつ車両登録No有」の場合

        '        '自社客のセッション情報を作成
        '        notifySession = CreateCustomerSession(inRowVisitInfo, inStaffInfo)

        'End Select
        'アンカーフラグのチェック
        If inAnchor = AnchorType.AnchorSet Then
            'アンカーフラグ「1：アンカー有り」の場合

            '自社客のセッション情報を作成
            notifySession = CreateCustomerSession(inRowVisitInfo)

        End If

        '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return notifySession

    End Function

    '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
    ' ''' <summary>
    ' ''' 未取引客の通知用セッション情報作成メソッド
    ' ''' </summary>
    ' ''' <param name="inRowVisitInfo">来店者情報欄表示情報</param>
    ' ''' <returns>戻り値</returns>
    ' ''' <remarks></remarks>
    ' ''' <history>
    ' ''' </history>
    'Private Function CreateNewCustomerSession(ByVal inRowVisitInfo As SC3100401DataSet.VisitInfoRow) As String

    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                , "{0}.{1} START" _
    '                , Me.GetType.ToString _
    '                , System.Reflection.MethodBase.GetCurrentMethod.Name))

    '    Dim workSession As New StringBuilder

    '    '来店管理連番のセッション値設定
    '    Me.SetSessionValueWord(workSession, _
    '                           SessionValueVisitSequence, _
    '                           inRowVisitInfo.VISITSEQ.ToString(CultureInfo.CurrentCulture))

    '    '名前の設定
    '    If Not inRowVisitInfo.IsNAMENull Then
    '        '名前がある場合

    '        '名前のセッション値作成
    '        Me.SetSessionValueWord(workSession, SessionValueName, inRowVisitInfo.NAME)

    '    End If

    '    '車両登録番号の設定
    '    If Not inRowVisitInfo.IsVCLREGNONull Then
    '        '車両登録番号がある場合は設定

    '        '車両登録Noのセッション値作成
    '        Me.SetSessionValueWord(workSession, SessionValueRegisterNo, inRowVisitInfo.VCLREGNO)

    '    End If

    '    'VINの設定
    '    If Not inRowVisitInfo.IsVINNull Then
    '        'VINがある場合は設定

    '        'VINのセッション値作成
    '        Me.SetSessionValueWord(workSession, SessionValueVinNo, inRowVisitInfo.VIN)

    '    End If

    '    'モデルコードの設定
    '    If Not inRowVisitInfo.IsMODELCODENull Then
    '        'モデルコードがある場合は設定

    '        'モデルコードのセッション値作成
    '        Me.SetSessionValueWord(workSession, SessionValueModelCode, inRowVisitInfo.MODELCODE)

    '    End If

    '    '電話番号の設定
    '    If Not inRowVisitInfo.IsTELNONull Then
    '        '電話番号がある場合は設定

    '        '電話番号のセッション値作成
    '        Me.SetSessionValueWord(workSession, SessionValueTelNo1, inRowVisitInfo.TELNO)

    '    End If

    '    '携帯番号の設定
    '    If Not inRowVisitInfo.IsMOBILENull Then
    '        '携帯番号がある場合は設定

    '        '携帯番号のセッション値作成
    '        Me.SetSessionValueWord(workSession, SessionValueTelNo2, inRowVisitInfo.MOBILE)

    '    End If

    '    '予約IDの設定
    '    If Not inRowVisitInfo.IsREZIDNull _
    '        AndAlso inRowVisitInfo.REZID >= 0 Then
    '        '予約IDがある場合は設定

    '        '予約IDのセッション値作成
    '        Me.SetSessionValueWord(workSession, _
    '                               SessionValueReserveId, _
    '                               inRowVisitInfo.REZID.ToString(CultureInfo.CurrentCulture))

    '    Else
    '        '予約IDがない場合は空を設定

    '        '予約IDのセッション値作成(空文字)
    '        Me.SetSessionValueWord(workSession, SessionValueReserveId, String.Empty)

    '    End If

    '    '事前準備フラグのセッション値設定
    '    Me.SetSessionValueWord(workSession, SessionValuePrepareChipType, RepairOrder)

    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                , "{0}.{1} END" _
    '                , Me.GetType.ToString _
    '                , System.Reflection.MethodBase.GetCurrentMethod.Name))

    '    Return workSession.ToString

    'End Function
    '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

    ''' <summary>
    ''' 自社客かつ車両登録NOがあるときの通知用セッション情報作成メソッド
    ''' </summary>
    ''' <param name="inRowVisitInfo">来店者情報欄表示情報</param>
    ''' <returns>戻り値</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発
    ''' </history>
    Private Function CreateCustomerSession(ByVal inRowVisitInfo As SC3100401DataSet.VisitInfoRow) As String
        '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
        'Private Function CreateCustomerSession(ByVal inRowVisitInfo As SC3100401DataSet.VisitInfoRow, _
        '                                       ByVal inStaffInfo As StaffContext) As String
        '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim workSession As New StringBuilder

        '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
        ''来店管理連番のセッション値設定
        'Me.SetSessionValueWord(workSession, _
        '                       SessionValueVisitSequence, _
        '                       inRowVisitInfo.VISITSEQ.ToString(CultureInfo.CurrentCulture))

        ''車両登録番号のセッション値設定
        'Me.SetSessionValueWord(workSession, SessionValueRegisterNo, inRowVisitInfo.VCLREGNO)

        '基幹顧客IDの設定
        If Not inRowVisitInfo.IsDMSIDNull Then
            '基幹顧客IDがある場合は設定

            '基幹顧客IDのセッション値作成
            Me.SetSessionValueWord(workSession, SessionValueDmsId, inRowVisitInfo.DMSID)

        End If

        '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        'VINの設定
        If Not inRowVisitInfo.IsVINNull Then
            'VINがある場合は設定

            'VINのセッション値作成
            Me.SetSessionValueWord(workSession, SessionValueVinNo, inRowVisitInfo.VIN)

        End If

        '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
        ''モデルコードの設定
        'If Not inRowVisitInfo.IsMODELCODENull Then
        '    'モデルコードがある場合は設定

        '    'モデルコードのセッション値作成
        '    Me.SetSessionValueWord(workSession, SessionValueModelCode, inRowVisitInfo.MODELCODE)

        'End If

        ''電話番号の設定
        'If Not inRowVisitInfo.IsTELNONull Then
        '    '電話番号がある場合は設定

        '    '電話番号のセッション値作成
        '    Me.SetSessionValueWord(workSession, SessionValueTelNo1, inRowVisitInfo.TELNO)

        'End If

        ''携帯番号の設定
        'If Not inRowVisitInfo.IsMOBILENull Then
        '    '携帯番号がある場合は設定

        '    '携帯番号のセッション値作成
        '    Me.SetSessionValueWord(workSession, SessionValueTelNo2, inRowVisitInfo.MOBILE)

        'End If

        ''予約IDの設定
        'If Not inRowVisitInfo.IsREZIDNull _
        '    AndAlso inRowVisitInfo.REZID >= 0 Then
        '    '予約IDがある場合は設定

        '    '予約IDのセッション値作成
        '    Me.SetSessionValueWord(workSession, _
        '                           SessionValueReserveId, _
        '                           inRowVisitInfo.REZID.ToString(CultureInfo.CurrentCulture))

        'Else
        '    '予約IDがない場合は空を設定

        '    '予約IDのセッション値作成(空文字)
        '    Me.SetSessionValueWord(workSession, SessionValueReserveId, String.Empty)

        'End If

        ''販売店コードのセッション設定
        'Me.SetSessionValueWord(workSession, SessionValueDealerCode, inStaffInfo.DlrCD)

        ''顧客詳細フラグのセッション設定
        'Me.SetSessionValueWord(workSession, SessionValueType, CustSegmentMyCustomer)

        ''事前準備フラグのセッション設定
        'Me.SetSessionValueWord(workSession, SessionValuePrepareChipType, RepairOrder)
        '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

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

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

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

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return workSession

    End Function

    ''' <summary>
    ''' Push用メッセージ作成メソッド
    ''' </summary>
    ''' <param name="inRowVisitInfo">来店者情報欄表示情報</param>
    ''' <param name="inEventKey">イベント情報</param>
    ''' <returns>作成したメッセージ文言</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発
    ''' 2017/02/03 NSK 加藤 TR-SVT-TMT-20161019-001 jobをオープンしたがチップのステータスが変更しない
    ''' </history>
    Private Function CreatePusuMessage(ByVal inRowVisitInfo As SC3100401DataSet.VisitInfoRow, _
                                       ByVal inEventKey As EventKeyId) As String

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim workContents As New StringBuilder

        '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
        ''メッセージ組立：車両登録番号
        'If Not (inRowVisitInfo.IsVCLREGNONull) _
        '    AndAlso Not (String.IsNullOrEmpty(inRowVisitInfo.VCLREGNO)) Then
        '    '車両登録番号がある場合

        '    '車両登録番号を設定
        '    workContents.Append(inRowVisitInfo.VCLREGNO)

        'End If

        ''メッセージ間にスペースの設定
        'workContents.Append(Space(3))

        ''メッセージ組立：名前
        ''名前の確認
        'If Not (inRowVisitInfo.IsNAMENull) _
        '    AndAlso Not (String.IsNullOrEmpty(inRowVisitInfo.NAME)) Then
        '    '名前がある場合

        '    '名前＋「様」を設定
        '    workContents.Append(inRowVisitInfo.NAME)
        '    workContents.Append(WebWordUtility.GetWord(ApplicationID, WordId.Id039))

        'Else
        '    '名前がない場合

        '    '「お客様」を設定
        '    workContents.Append(WebWordUtility.GetWord(ApplicationID, WordId.Id040))

        'End If

        ''メッセージ間にスペースの設定
        'workContents.Append(Space(3))

        ''メッセージ組立：サービスコード+来店情報
        ''メッセージ種別の判定
        'Select Case inEventKey
        '    Case EventKeyId.SAAssig
        '        '「SA振当登録」の場合

        '        'ご来店を設定
        '        workContents.Append(WebWordUtility.GetWord(ApplicationID, WordId.Id041))

        '        'メッセージ間にスペースの設定
        '        workContents.Append(Space(3))

        '        'サービスコードのチェック
        '        If ServiceCodeTeiki.Equals(inRowVisitInfo.SERVICECODE) Then
        '            '「定期点検」の場合

        '            '定期点検を設定
        '            workContents.Append(WebWordUtility.GetWord(ApplicationID, WordId.Id038))

        '        ElseIf ServiceCodeIppan.Equals(inRowVisitInfo.SERVICECODE) Then
        '            '「一般整備」の場合

        '            '一般整備を設定
        '            workContents.Append(WebWordUtility.GetWord(ApplicationID, WordId.Id037))

        '        End If
        '    Case EventKeyId.SAUndo
        '        '「UNDO」の場合

        '        'ご来店キャンセルを設定
        '        workContents.Append(WebWordUtility.GetWord(ApplicationID, WordId.Id042))

        'End Select


        'メッセージ組立：「振当て」「振当てキャンセル」
        Select Case inEventKey
            Case EventKeyId.SAAssig
                '「SA振当登録」の場合
                '「振当て」を設定
                workContents.Append(WebWordUtility.GetWord(ApplicationID, WordId.Id041))

            Case EventKeyId.SAUndo
                '「UNDO」の場合
                '「振当てキャンセル」を設定
                workContents.Append(WebWordUtility.GetWord(ApplicationID, WordId.Id042))

        End Select

        'メッセージ組立：スペース
        workContents.Append(Space(3))

        'メッセージ組立：車両登録番号
        If Not (inRowVisitInfo.IsVCLREGNONull) _
            AndAlso Not (String.IsNullOrEmpty(inRowVisitInfo.VCLREGNO)) Then
            '車両登録番号がある場合
            '車両登録番号を設定
            workContents.Append(inRowVisitInfo.VCLREGNO)

        End If

        'メッセージ組立：スペース
        workContents.Append(Space(3))

        'メッセージ組立：お客様名
        If Not (inRowVisitInfo.IsNAMENull) _
            AndAlso Not (String.IsNullOrEmpty(inRowVisitInfo.NAME)) Then
            '名前がある場合
            If Not (inRowVisitInfo.IsNAMETITLE_NAMENull) Then
                '敬称が存在する場合
                If NameTitlePositionTypeBefore.Equals(inRowVisitInfo.POSITION_TYPE) Then
                    '敬称を後ろに設定する場合
                    workContents.Append(inRowVisitInfo.NAME)
                    workContents.Append(inRowVisitInfo.NAMETITLE_NAME)

                Else
                    '敬称を前に設定する場合
                    workContents.Append(inRowVisitInfo.NAMETITLE_NAME)
                    workContents.Append(inRowVisitInfo.NAME)

                End If

            Else
                '敬称が存在しない場合
                workContents.Append(inRowVisitInfo.NAME)

            End If

        Else
            'データがない場合
            '「お客様」を設定
            workContents.Append(WebWordUtility.GetWord(ApplicationID, WordId.Id040))

        End If

        'メッセージ組立：予約情報
        '予約IDチェック
        ' 2017/01/23 NSK 加藤 TR-SVT-TMT-20161019-001 jobをオープンしたがチップのステータスが変更しない START
        If Not (inRowVisitInfo.IsREZIDNull) AndAlso 0 < inRowVisitInfo.REZID AndAlso
            Not (inRowVisitInfo.IsSTART_DATETIMENull) Then
            ' 2017/01/23 NSK 加藤 TR-SVT-TMT-20161019-001 jobをオープンしたがチップのステータスが変更しない END

            '予約IDが存在している場合
            'メッセージ組立：スペース
            workContents.Append(Space(3))

            'メッセージ組立：開始日時
            workContents.Append(String.Format(CultureInfo.InvariantCulture, "{0:HH:mm}", inRowVisitInfo.START_DATETIME))

            'メッセージ組立：「～」文言
            workContents.Append(WebWordUtility.GetWord(ApplicationID, WordId.Id035))

            'メッセージ組立：終了日時
            workContents.Append(String.Format(CultureInfo.InvariantCulture, "{0:HH:mm}", inRowVisitInfo.END_DATETIME))

            '整備名チェック
            If Not (inRowVisitInfo.IsMERCHANDISENAMENull) AndAlso _
               Not (String.IsNullOrEmpty(inRowVisitInfo.MERCHANDISENAME)) Then
                'データが存在する場合
                'メッセージ組立：スペース
                workContents.Append(Space(3))

                'メッセージ組立：整備名
                workContents.Append(inRowVisitInfo.MERCHANDISENAME)

            End If

        End If

        '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        '戻り値設定
        Dim pushMessage As String = workContents.ToString()

        '開放処理
        workContents = Nothing

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return pushMessage

    End Function

    ''' <summary>
    ''' SAに対するPush処理
    ''' </summary>
    ''' <param name="inSACode">リフレッシュ先アカウント</param>
    ''' <param name="inRefreshArea">再描画エリア(0：全て、1：未振当てエリア)</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発
    ''' </history>
    Private Sub SendPushServerSA(ByVal inSACode As String, _
                                 ByVal inRefreshArea As String)
        '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
        'Private Sub SendPushServerSA(ByVal inSACode As String)
        '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'リフレッシュの文字列作成
        '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
        'Dim pushWord = RefreshSAPushInfo.Replace(RefreshSAReplaceWord, inSACode)

        Dim pushWord As String = String.Empty
        'リフレッシュエリアのチェック
        If SARefreshTypeAssignment.Equals(inRefreshArea) Then
            '未振当てエリアの場合
            '未振当てエリアを更新する文字列作成
            pushWord = RefreshSAPushInfoAssginmentArea.Replace(RefreshSAReplaceWord, inSACode)

        Else
            '上記以外の場合
            '全エリアを更新する文字列作成
            pushWord = RefreshSAPushInfo.Replace(RefreshSAReplaceWord, inSACode)

        End If

        '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        'Push
        Dim visitUtility As New VisitUtility

        'Push処理実行
        visitUtility.SendPush(pushWord)

        '開放処理
        visitUtility = Nothing

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START

    ''2013/08/27 TMEJ 小澤 IT9559_タブレット版SMB機能開発(IF開発) START
    ' ''' <summary>
    ' ''' CT、FMに対するPush処理
    ' ''' </summary>
    ' ''' <param name="inAccount">リフレッシュ先アカウント</param>
    ' ''' <remarks></remarks>
    'Private Sub SendPushServerCTAndFM(ByVal inAccount As String)

    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                , "{0}.{1} START" _
    '                , Me.GetType.ToString _
    '                , System.Reflection.MethodBase.GetCurrentMethod.Name))

    '    'リフレッシュの文字列作成
    '    Dim pushWord = RefreshCTAndFMPushInfo.Replace(RefreshSAReplaceWord, inAccount)

    '    'Push
    '    Dim visitUtility As New VisitUtility

    '    'Push処理実行
    '    visitUtility.SendPush(pushWord)

    '    '開放処理
    '    visitUtility = Nothing

    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                , "{0}.{1} END" _
    '                , Me.GetType.ToString _
    '                , System.Reflection.MethodBase.GetCurrentMethod.Name))

    'End Sub
    ''2013/08/27 TMEJ 小澤 IT9559_タブレット版SMB機能開発(IF開発) END

    ''' <summary>
    ''' 来店管理画面再描画Push処理
    ''' </summary>
    ''' <param name="inAccount">リフレッシュ先アカウント</param>
    ''' <remarks></remarks>
    Private Sub SendPushServerSVR(ByVal inAccount As String)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'リフレッシュの文字列作成
        Dim pushWord = RefreshSVRPushInfo.Replace(RefreshSAReplaceWord, inAccount)

        'Push
        Dim visitUtility As New VisitUtility

        'Push処理実行
        visitUtility.SendPush(pushWord)

        '開放処理
        visitUtility = Nothing

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' 工程管理画面再描画Push処理
    ''' </summary>
    ''' <param name="inAccount">リフレッシュ先アカウント</param>
    ''' <remarks></remarks>
    Private Sub SendPushServerCTAndCHT(ByVal inAccount As String)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'リフレッシュの文字列作成
        Dim pushWord = RefreshCTAndFMPushInfo.Replace(RefreshSAReplaceWord, inAccount)

        'Push
        Dim visitUtility As New VisitUtility

        'Push処理実行
        visitUtility.SendPush(pushWord)

        '開放処理
        visitUtility = Nothing

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

#End Region

#End Region

#Region "PC通知"

#Region "PC通知用定数"

    ''' <summary>
    ''' リフレッシュ通知のPush情報
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RefreshPCPushInfo As String = "cat=action&type=main&sub=js&uid=#USER_ACCOUNT#&js1=#JS1#"

    ''' <summary>
    ''' リフレッシュ通知のAccount置換用文字列
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RefreshPCReplaceAccount As String = "#USER_ACCOUNT#"

    ''' <summary>
    ''' リフレッシュ通知のJs1置換用文字列
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RefreshPCReplaceJs1 As String = "#JS1#"

    ''' <summary>
    ''' リフレッシュ通知のJs1置換文字列(呼び出し)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RefreshPCReplaceJs1AddWord As String = "addCallee()"

    ''' <summary>
    ''' リフレッシュ通知のJs1置換文字列(呼び出しキャンセル)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RefreshPCReplaceJs1DeleteWord As String = "delCallee()"

    ''' <summary>
    ''' 受付権限（サービス受付ボード） PC版
    ''' </summary>
    ''' <remarks></remarks>
    Private Const Operation_Reception As Decimal = 60

#End Region

#Region "Publicメソッド"

    ''' <summary>
    ''' PC通知処理
    ''' 受付待ちモニターを再描画する関数がいるかも
    ''' </summary>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inStoreCode">店舗コード</param>
    ''' <param name="inCustomFooterID">イベントキーID</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Public Sub SendPushServerPC(ByVal inDealerCode As String, _
                                ByVal inStoreCode As String, _
                                ByVal inCustomFooterId As String)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'リフレッシュの文字列作成
        Dim pushWord As String = RefreshPCPushInfo

        'フッターボタンの確認
        If CType(EventKeyId.FooterCallButton, String).Equals(inCustomFooterId) Then
            '呼出の場合

            '音有りPushを設定
            pushWord = pushWord.Replace(RefreshPCReplaceJs1, RefreshPCReplaceJs1AddWord)

        Else
            '上記以外の場合

            '音無しPushを設定
            pushWord = pushWord.Replace(RefreshPCReplaceJs1, RefreshPCReplaceJs1DeleteWord)

        End If

        '受付権限コード
        Dim sendOperationList As New List(Of Decimal)

        '受付権限コード取得
        sendOperationList.Add(Operation_Reception)

        '受付権限ユーザー情報
        Dim visitUtility As New VisitUtility

        '全受付権限ユーザー分ループ
        For Each drUser As UsersDataSet.USERSRow In _
                                    UsersTableAdapter.GetUsersDataTable(inDealerCode, _
                                                                        inStoreCode, _
                                                                        sendOperationList, _
                                                                        DelFlgNone)

            'Push情報のAccount取得したアカウントに置換
            Dim pushAccount = pushWord.Replace(RefreshPCReplaceAccount, drUser.ACCOUNT)

            'Push処理実行
            visitUtility.SendPushPC(pushAccount)

        Next

        '開放処理
        visitUtility = Nothing

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    '2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする START

    ''' <summary>
    ''' WelcomeBoardリフレッシュPush送信
    ''' </summary>
    ''' <param name="inStaffInfo">ログインスタッフ情報</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Public Sub SendPushForRefreshWelcomeBoard(ByVal inStaffInfo As StaffContext)
        Logger.Debug("SendPushForRefreshWelcomeBoard_Start Pram[" & inStaffInfo.DlrCD & "," & inStaffInfo.BrnCD & inStaffInfo.Account & "]")

        'スタッフ情報の取得(WB)
        Dim stuffCodeList As New List(Of Decimal)
        stuffCodeList.Add(SystemFrameworks.Core.iCROP.BizLogic.Operation.WBS)

        '全ユーザー情報の取得
        Dim utility As New VisitUtilityBusinessLogic
        Dim sendPushUsers As VisitUtilityUsersDataTable = _
            utility.GetUsers(inStaffInfo.DlrCD, inStaffInfo.BrnCD, stuffCodeList, Nothing, DelFlgNone)
        utility = Nothing

        '来店通知命令の送信
        For Each userRow As VisitUtilityUsersRow In sendPushUsers

            '送信処理
            Dim visitUtility As New Visit.Api.BizLogic.VisitUtility
            visitUtility.SendPushReconstructionPC(inStaffInfo.Account, userRow.ACCOUNT, "", inStaffInfo.DlrCD)

        Next
        Logger.Debug("SendPushForRefreshWelcomeBoard_End")
    End Sub
    '2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする END

#End Region

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
