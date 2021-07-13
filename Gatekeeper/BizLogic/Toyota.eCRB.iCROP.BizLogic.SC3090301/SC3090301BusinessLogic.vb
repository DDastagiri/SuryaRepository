'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3090301BusinessLogic.vb
'──────────────────────────────────
'機能： ゲートキーパーメイン
'補足： 
'作成： yyyy/MM/dd KN  x.xxxxxx
'更新： 2012/02/13 KN  y.nakamura STEP2開発 $01
'更新： 2012/02/22 KN  m.asano    性能改善  $02
'更新： 2012/08/08 KN  m.asano    案内係へのPush  $03
'更新： 2012/09/04 KN  彭健       問連GTMC120806001の修正  $04
'更新： 2012/11/06 TMEJ t.shimamura サービス入庫時の担当SC通知追加 $05
'更新： 2013/01/21 TMEJ t.shimamura 問連GTMC1301118131の修正 $06
'更新： 2013/03/13 TMEJ t.shimamura 来店歓迎オペレーション確立に向けたアプリ評価 $07
'更新： 2013/04/16 TMEJ m.asano   ウェルカムボード仕様変更対応 $08
'更新： 2013/05/20 TMEJ t.shimamura   問連FTMS13042603の修正 $09
'更新： 2013/10/16 TMEJ m.asano   次世代e-CRBセールス機能 新DB適応に向けた機能開発 $10
'更新： 2013/12/02 TMEJ t.shimamura   次世代e-CRBサービス 店舗展開に向けた標準作業確立 $11
'更新： 2014/01/17 TMEJ 陳   TMEJ次世代サービス 工程管理機能開発 $12
'更新： 2014/05/21 TMEJ y.gotoh   サービスタブレットゲートカメラ連携機能追加開発 $13
'更新： 2015/02/18 TMEJ y.nakamura UAT課題#158 $14
'更新： 2015/12/17 TM y.nakamura ゲートキーパーのユーザ表示対応 $15
'更新： 2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001 iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 $16
'更新： 2018/04/19 NSK 井本 TR-V4-TMT-20171117-001 登録番号のシングルクォーテ－ションによるエラー対応 $17
'──────────────────────────────────

Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Configuration
Imports Toyota.eCRB.GateKeeper.GateKeeperMain.DataAccess.SC3090301DataSet
Imports Toyota.eCRB.GateKeeper.GateKeeperMain.DataAccess.SC3090301DataSetTableAdapters
Imports Toyota.eCRB.Visit.Api.BizLogic
Imports System.Net
Imports System.Text
Imports System.Web
Imports System.IO
Imports System.Xml
Imports Toyota.eCRB.Tool.Notify.Api.BizLogic
Imports Toyota.eCRB.Tool.Notify.Api.DataAccess
Imports Toyota.eCRB.Tool.Notify.Api.DataAccess.ConstCode
Imports Toyota.eCRB.iCROP.BizLogic.IC3810101
' $01 start step2開発
Imports Toyota.eCRB.Visit.Api.DataAccess.VisitUtilityDataSet
Imports Toyota.eCRB.GateKeeper.GateKeeperMain.DataAccess
' $01 end   step2開発

' $10 START 次世代e-CRBセールス機能 新DB適応に向けた機能開発
Imports Toyota.eCRB.Visit.Api.BizLogic.VisitUtilityBusinessLogic
Imports Toyota.eCRB.Visit.Api.DataAccess
Imports Toyota.eCRB.Visit.Api.DataAccess.VisitReceptionDataSetTableAdapters
Imports Toyota.eCRB.Visit.Api.DataAccess.VisitReceptionDataSet
Imports Toyota.eCRB.Visit.Api.DataAccess.VisitUtilityDataSetTableAdapters
' $10 END   次世代e-CRBセールス機能 新DB適応に向けた機能開発
' $15 start ゲートキーパーのユーザ表示対応
Imports System.Globalization
' $15 end   ゲートキーパーのユーザ表示対応

''' <summary>
''' SC3090301(ゲートキーパーメイン)
''' ビジネスロジック層
''' </summary>
''' <remarks></remarks>
Public Class SC3090301BusinessLogic
    Inherits BaseBusinessComponent
    Implements ISC3090301BusinessLogic

#Region "定数"

#Region "DB関連"

    ''' <summary>
    ''' システム管理マスタ.パラメータ名:敬称表示位置
    ''' </summary>
    ''' <remarks></remarks>
    Private Const KeisyoZengo As String = "KEISYO_ZENGO"

    ''' <summary>
    ''' システム管理マスタ.パラメータ名:顔写真の保存先フォルダ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FacepicUploadUrl As String = "FACEPIC_UPLOADURL"

    ' $01 start step2開発
    ''' <summary>
    ''' システム管理マスタ.パラメータ名:苦情情報日数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ComplaintDisplayDate As String = "COMPLAINT_DISPLAYDATE"
    ' $01 end   step2開発

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
    ''' 敬称表示位置:前
    ''' </summary>
    ''' <remarks></remarks>
    Private Const HonorificTitleMae As String = "1"

    ''' <summary>
    ''' 敬称表示位置:後
    ''' </summary>
    ''' <remarks></remarks>
    Private Const HonorificTitleUshiro As String = "2"

    ''' <summary>
    ''' 性別：男性
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TypeMale As String = "0"

    ''' <summary>
    ''' 性別：女性
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TypeFemale As String = "1"

    ''' <summary>
    ''' 顧客区分:自社客
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CustKubunOrg As String = "1"

    ''' <summary>
    ''' 機能ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ApplicationId As String = "SC3090301"

    ''' <summary>
    ''' 対応フラグ：未送信
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DealFlagUnsend As String = "0"

    ''' <summary>
    ''' 対応フラグ：送信済み
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DealFlagSend As String = "1"

    ''' <summary>
    ''' ブロードキャストフラグ：未送信
    ''' </summary>
    ''' <remarks></remarks>
    Private Const BroudcastFlagUnsend As String = "0"

    ''' <summary>
    ''' ブロードキャストフラグ：対象外
    ''' </summary>
    ''' <remarks></remarks>
    Private Const BroudcastFlagNotTarget As String = "9"

    ''' <summary>
    ''' 来店実績ステータス:フリー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VisitStatusFree As String = "01"

    ''' <summary>
    ''' 来店実績ステータス:調整中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VisitStatusAdjust As String = "03"

    ''' <summary>
    ''' 権限コード：セールススタッフ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OperationCodeSs As Integer = 8

    ''' <summary>
    ''' 権限コード：受付係
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OperationCodeSlr As Integer = 51

    ''' <summary>
    ''' 権限コード：案内係
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OperationCodeSvr As Integer = 52

    ''' <summary>
    ''' 権限コード：ブランチマネージャー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OperationCodeBm As Integer = 6

    ''' <summary>
    ''' 権限コード：セールスマネージャー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OperationCodeSsm As Integer = 7

    ' $01 start step2開発
    ''' <summary>
    ''' 権限コード：SSV
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OperationCodeSsv As Integer = 53
    ' $01 end   step2開発

    ''' <summary>
    ''' オラクルエラーコード:タイムアウト
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ErrorCodeOra2049 As Integer = 2049

    ' $10 START 次世代e-CRBセールス機能 新DB適応に向けた機能開発
    ''' <summary>
    ''' 在席状態(大分類)：スタンバイ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PresenceCategoryStandby As String = "1"

    ''' <summary>
    ''' 在席状態(大分類)：商談中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PresenceCategoryNegotiate As String = "2"

    ''' <summary>
    ''' 在席状態(大分類)：退席中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PresenceCategoryLeaving As String = "3"

    ''' <summary>
    ''' 在席状態(大分類)：オフライン
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PresenceCategoryOffline As String = "4"
    ' $10 END   次世代e-CRBセールス機能 新DB適応に向けた機能開発

    ' $11 start 削除フラグ
    ''' <summary>
    ''' 削除フラグ：削除以外
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DeleteFlagNotDeleted As String = "0"

    ''' <summary>
    ''' 削除フラグ：論理削除
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DeleteFlagDelete As String = "1"
    ' $11 end 削除フラグ

    '$14 start UAT課題#158
    ''' <summary>
    ''' 予約フラグ：予約あり
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ReservFlagOn As String = "1"

    ''' <summary>
    '''予約フラグ：予約なし
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ReservFlagOff As String = "0"
    '$14 end UAT課題#158

    ' $15 start ゲートキーパーのユーザ表示対応
    ''' <summary>
    ''' システム設定名（車両登録番号の区切文字）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SysRegNumDlmtr As String = "REG_NUM_DELIMITER"
    ' $15 end   ゲートキーパーのユーザ表示対応

    '2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 START
    ''' <summary>
    ''' サービス入庫ID(0:予約指定無し)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SvcinIdNotSpecified As Integer = 0

    ''' <summary>
    ''' サービス入庫ID(-1:予約を引き当てない)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SvcinIdNotIdentified As Integer = -1
    '2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 END
#End Region

#Region "画面パラメータ"

    ''' <summary>
    ''' 送信タイプ：顧客担当SS
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SendTypeCsutSs As Integer = 1

    ''' <summary>
    ''' 送信タイプ：セールスマネージャー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SendTypeSsm As Integer = 2

    ''' <summary>
    ''' 送信タイプ：ブランチマネージャー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SendTypeBm As Integer = 3

    ''' <summary>
    ''' 送信タイプ：受付係
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SendTypeSlr As Integer = 4

    ''' <summary>
    ''' 送信タイプ：セールススタッフ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SendTypeSs As Integer = 5

    ''' <summary>
    ''' 送信タイプ：案内係
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SendTypeSvr As Integer = 6

    ' $05 start 顧客担当SCへの通知 
    ''' <summary>
    ''' 送信タイプ：顧客担当SC
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SendTypeSC As Integer = 7
    ' $05 end   顧客担当SCへの通知 

    ''' <summary>
    ''' 来店目的：セールス
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VisitPurposeSales As String = "1"

    ''' <summary>
    ''' 来店目的：サービス
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VisitPurposeService As String = "2"

    ''' <summary>
    ''' 来店目的：対象外
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VisitPurposeNotTarget As String = "3"

    ' $01 start step2開発
    ' ''' <summary>
    ' ''' シルエットアイコンパス：顧客
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const SilhouettePersonIconPath As String = "../Styles/Images/VisitCommon/silhouette_person.png"

    ' ''' <summary>
    ' ''' シルエットアイコンパス：車両
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const SilhouetteCarIconPath As String = "../Styles/Images/VisitCommon/silhouette_Car.png"
    ' $01 end step2開発

    ''' <summary>
    ''' 顧客種別：オーナー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CustomerKindOwner As String = "1"

    ''' <summary>
    ''' Push送信タイプ：新規
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PushTypeNew As Integer = 1

    ''' <summary>
    ''' Push送信タイプ：自社客・未取引客(担当スタッフあり・ステータスオフライン以外)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PushTypeOrgOrNewCustomerOffline As Integer = 2

    ''' <summary>
    ''' Push送信タイプ：自社客・未取引客(担当スタッフなし・ステータスオフライン)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PushTypeOrgOrNewCustomerNotOffline As Integer = 3

    ''' <summary>
    ''' Push送信タイプ：自社客・未取引客(担当スタッフなし)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PushTypeOrgOrNewCustomerNotStuff As Integer = 4

    ''' <summary>
    ''' Push送信タイプ：顧客情報なし
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PushTypeNotCustomerInfo As Integer = 5


    ''' <summary>
    ''' 送信タイプ：顧客担当SC(サービス入庫)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PushTypeSCService As Integer = 6
    ' $05 end 顧客担当SCへの通知 

#End Region

#Region "メッセージID"

    ''' <summary>
    ''' メッセージID:成功
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MessageIdSuccess As Integer = 0

    ''' <summary>
    ''' メッセージID:エラー[DBタイムアウト]
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MessageIdErrorDbTimeOut As Integer = 902

    '$11 start 削除メッセージ
    ''' <summary>
    ''' メッセージID:エラー[DBタイムアウト(削除時)]
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MessageIdErrorDbTimeOutInDelete As Integer = 903

    ''' <summary>
    ''' メッセージID:削除済みの場合
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MessageIdErrorNoDeleteTarget As Integer = 904

    ''' <summary>
    ''' メッセージID:送信済み
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MessageIdErrorSend As Integer = 901

    '$11 end 削除メッセージ

    ' $01 start step2開発
    ''' <summary>
    ''' 文言ID：苦情文字
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ClameWord As Integer = 7
    ' $01 end   step2開発


    '$08 start ウェルカムボード仕様変更対応
    ''' <summary>
    ''' 文言ID：固定表示敬称
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NameTitleDefault As Integer = 13
    '$08 end ウェルカムボード仕様変更対応
#End Region

#End Region

#Region "メンバ変数"
    ' $05 start 顧客担当SCへの通知 
    Dim MESSAGE_SERVICE As String = WebWordUtility.GetWord(12)
    ' $05 end 顧客担当SCへの通知 

    ' $10 START 次世代e-CRBセールス機能 新DB適応に向けた機能開発
    ''' <summary>
    ''' 現在顧客来店情報
    ''' </summary>
    ''' <remarks></remarks>
    Private VisitSalesDataRow As VisitReceptionVisitSalesRow

    ''' <summary>
    ''' 現在顧客苦情情報
    ''' </summary>
    ''' <remarks></remarks>
    Private isClaimeInfo As Boolean = False
    ' $10 END   次世代e-CRBセールス機能 新DB適応に向けた機能開発

    ' $13 START サービスタブレットゲートカメラ連携機能追加開発
    ''' <summary>
    ''' 変換フォーマット
    ''' </summary>
    ''' <remarks></remarks>
    Private sysChangeFormat As String = Nothing

    ''' <summary>
    ''' 変換当て込み文字
    ''' </summary>
    ''' <remarks></remarks>
    Private sysChangeString As String = Nothing
    ' $13 END サービスタブレットゲートカメラ連携機能追加開発

    '$14 start UAT課題#158
    ''' <summary>
    ''' StallRangeDays
    ''' </summary>
    ''' <param name="dealerCD"></param>
    ''' <param name="storeCD"></param>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private ReadOnly Property StallRangeDays(ByVal dealerCD As String, _
                                                    ByVal storeCD As String) As Long
        Get
            Dim value As Long

            Dim row As DlrEnvSettingDataSet.DLRENVSETTINGRow _
                    = (New BranchEnvSetting).GetEnvSetting(dealerCD, storeCD, "STALL_RANGE_DAYS")

            If row IsNot Nothing _
                AndAlso Long.TryParse(row.PARAMVALUE, value) = True Then

                Return value
            Else

                Return 0
            End If
        End Get
    End Property
    '$14 end UAT課題#158

#End Region

#Region "コンストラクタ"
    ' $10 START 次世代e-CRBセールス機能 新DB適応に向けた機能開発
    ''' <summary>
    ''' デフォルトコンストラクタ
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        Using visitReceptionTable As New VisitReceptionVisitSalesDataTable
            VisitSalesDataRow = visitReceptionTable.NewVisitReceptionVisitSalesRow
        End Using
    End Sub
    ' $10 END   次世代e-CRBセールス機能 新DB適応に向けた機能開発
#End Region

#Region "来店通知未送信データ取得"

    ''' <summary>
    ''' 来店通知未送信データ件数取得
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="storeCode">店舗コード</param>
    ''' <param name="nowDate">本日日付</param>
    ''' <returns>来店通知未送信データ件数</returns>
    ''' <remarks></remarks>
    Public Function GetUnsentDataTotalCount(ByVal dealerCode As String, _
                                            ByVal storeCode As String, _
                                            ByVal nowDate As Date) As Integer

        Logger.Info("GetUnsentDataTotalCount_Start Pram[" & dealerCode & "," & storeCode & "," & nowDate & "]")

        '日付検索用
        Dim dateSt As New Date(nowDate.Year, nowDate.Month, nowDate.Day, 0, 0, 0)
        Dim dateEd As New Date(nowDate.Year, nowDate.Month, nowDate.Day, 23, 59, 59)
        Dim visUnsentTotalRow As SC3090301VisitUnsentTotalCountRow

        '検索処理
        Using adapter As New SC3090301TableAdapter

            Using visUnsentTotalTbl As SC3090301VisitUnsentTotalCountDataTable = adapter.GetVisitUnsentTotalCount(dealerCode, storeCode, dateSt, dateEd)

                visUnsentTotalRow = visUnsentTotalTbl.Item(0)

            End Using
        End Using

        Logger.Info("GetUnsentDataTotalCount_End Ret[" & CType(visUnsentTotalRow.TOTALCOUNT, Integer) & "]")
        Return CType(visUnsentTotalRow.TOTALCOUNT, Integer)

    End Function

    '$04 Start
    ' $11 start データ取得範囲を指定
    ''' <summary>
    ''' 来店通知未送信データの取得
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="storeCode">店舗コード</param>
    ''' <param name="nowDate">本日日付</param>
    ''' <returns>来店通知未送信データ</returns>
    ''' <remarks></remarks>
    Public Function GetUnsentData(ByVal dealerCode As String, _
                                  ByVal storeCode As String, _
                                  ByVal nowDate As Date, _
                                  ByVal startRowNumber As Integer, _
                                  ByVal endRownNumber As Integer) As DataAccess.SC3090301DataSet

        Logger.Info("GetUnsentData_Start Pram[" & dealerCode & "," & storeCode & "," & nowDate & "," & startRowNumber & "," & endRownNumber & "]")
        ' $11 end データ取得範囲を指定

        Dim sc3090301DataSet As New DataAccess.SC3090301DataSet
        Dim vclUnSentTblAdd As SC3090301VisitVehicleUnsentDataDataTable = sc3090301DataSet.SC3090301VisitVehicleUnsentData
        Dim unsentTbl As SC3090301VisitUnsentDataDataTable = sc3090301DataSet.SC3090301VisitUnsentData
        '$14 start UAT課題#158
        Dim unsentTblWork As New SC3090301VisitUnsentDataDataTable
        '$14 end UAT課題#158

        '日付検索用
        Dim dateSt As New Date(nowDate.Year, nowDate.Month, nowDate.Day, 0, 0, 0)
        Dim dateEd As New Date(nowDate.Year, nowDate.Month, nowDate.Day, 23, 59, 59)

        '検索処理
        Using adapter As New SC3090301TableAdapter

            ' $11 start データ取得範囲を指定
            Using vclUnSentTbl As SC3090301VisitVehicleUnsentDataDataTable = _
                adapter.GetVisitUnsentData(dealerCode, storeCode, dateSt, dateEd, startRowNumber, endRownNumber)
                ' $11 end データ取得範囲を指定

                Dim custCount As Integer
                '$14 start UAT課題#158
                '予約確認条件
                Dim cstId As New Collection
                Dim vclId As New Collection
                Dim reservData As SC3090301ReservDataDataTable

                '$14 end UAT課題#158

                '取得した未送信データ分処理を行う。
                For Each drVclUnSent As SC3090301VisitVehicleUnsentDataRow In vclUnSentTbl.Rows

                    vclUnSentTblAdd.AddSC3090301VisitVehicleUnsentDataRow(drVclUnSent.VISITVCLSEQ, _
                                                                          drVclUnSent.VISITTIMESTAMP, _
                                                                          drVclUnSent.VCLREGNO)

                    ' $13 START サービスタブレットゲートカメラ連携機能追加開発
                    'Dim changeVclRegNo As String = GetFormattedVclRegNo(drVclUnSent.VCLREGNO)     'フォーマット変換をした車両登録番号を取得する
                    Dim changeVclRegNo As List(Of String) = GetFormattedVclRegNo(drVclUnSent.VCLREGNO)     'フォーマット変換をした車両登録番号を取得する
                    ' $13 END サービスタブレットゲートカメラ連携機能追加開発

                    ' $10 START 次世代e-CRBセールス機能 新DB適応に向けた機能開発
                    Using VisitReceptionAdapter As New VisitReceptionTableAdapter

                        ' $13 START サービスタブレットゲートカメラ連携機能追加開発
                        'Dim sortVclRegNo As New List(Of String)
                        'sortVclRegNo.Add(drVclUnSent.VCLREGNO)
                        'sortVclRegNo.Add(changeVclRegNo)
                        ' $13 END サービスタブレットゲートカメラ連携機能追加開発

                        ' 顧客情報取得
                        ' $13 START サービスタブレットゲートカメラ連携機能追加開発
                        Using custTbl As VisitReceptionCustomerListDataTable = _
                            VisitReceptionAdapter.GetCustomerList(dealerCode, "1", changeVclRegNo, "1")
                            'VisitReceptionAdapter.GetCustomerList(dealerCode, "1", sortVclRegNo, "1")
                            ' $13 END サービスタブレットゲートカメラ連携機能追加開発

                            ' 該当顧客件数の取得
                            custCount = custTbl.Rows.Count

                            ' 該当顧客件数が0の場合
                            If custCount = 0 Then
                                Logger.Debug("GetUnsentData_002 CustomerInfo Not Exist")

                                '$08 start ウェルカムボード仕様変更対応
                                '$12 START TMEJ次世代サービス 工程管理機能開発
                                'unsentTbl.AddSC3090301VisitUnsentDataRow(drVclUnSent.VISITVCLSEQ, _
                                '                                        drVclUnSent.VISITTIMESTAMP, _
                                '                                        drVclUnSent.VCLREGNO, _
                                '                                        custCount, _
                                '                                        "", _
                                '                                        "", _
                                '                                        "", _
                                '                                        "", _
                                '                                        "", _
                                '                                        "", _
                                '                                        "", _
                                '                                        "", _
                                '                                        "", _
                                '                                        CStr(0), _
                                '                                        "",
                                '                                        "")
                                unsentTblWork.AddSC3090301VisitUnsentDataRow(drVclUnSent.VISITVCLSEQ, _
                                                                        drVclUnSent.VISITTIMESTAMP, _
                                                                        drVclUnSent.VCLREGNO, _
                                                                        custCount, _
                                                                        "", _
                                                                        "", _
                                                                        "", _
                                                                        "", _
                                                                        "", _
                                                                        "", _
                                                                        "", _
                                                                        "", _
                                                                        "", _
                                                                        CStr(0), _
                                                                        "", _
                                                                        "", _
                                                                        "", _
                                                                        ReservFlagOff, _
                                                                        "", _
                                                                        "")
                                '$12 END TMEJ次世代サービス 工程管理機能開発
                                '$08 start ウェルカムボード仕様変更対応
                            Else
                                Logger.Debug("GetUnsentData_003 CustomerInfo Exist")

                                '自社客情報マージ
                                For Each custRow As VisitReceptionCustomerListRow In custTbl

                                    '$08 start ウェルカムボード仕様変更対応
                                    '行追加
                                    unsentTblWork.AddSC3090301VisitUnsentDataRow(drVclUnSent.VISITVCLSEQ, _
                                                                    drVclUnSent.VISITTIMESTAMP, _
                                                                    custRow.VCLREGNO, _
                                                                    custCount, _
                                                                    custRow.NAME, _
                                                                    custRow.NAMETITLE, _
                                                                    custRow.CUSTKBN, _
                                                                    CStr(IIf(String.Equals(custRow.CUSTKBN, CustKubunOrg), custRow.SERIESNM, custRow.MAKERNAME)), _
                                                                    CStr(IIf(String.Equals(custRow.CUSTKBN, CustKubunOrg), custRow.EXTERIORNAME, custRow.SERIESNM)), _
                                                                    custRow.CUSTCD, _
                                                                    custRow.STUFFCD, _
                                                                    CStr(IIf(String.Equals(custRow.SEX, TypeFemale), custRow.SEX, TypeMale)), _
                                                                    custRow.VIN, _
                                                                    custRow.SEQNO, _
                                                                    custRow.SACODE,
                                                                    custRow.CUSTYPE,
                                                                    custRow.REG_AREA_NAME,
                                                                    ReservFlagOff, _
                                                                    custRow.NAME, _
                                                                    custRow.NAMETITLE)
                                    '$08 start ウェルカムボード仕様変更対応

                                    '$14 start UAT課題#158
                                    '予約確認の条件追加
                                    cstId.Add(custRow.CUSTCD)
                                    vclId.Add(custRow.SEQNO)
                                    '$14 end UAT課題#158
                                Next
                            End If
                        End Using
                    End Using
                    ' $10 END   次世代e-CRBセールス機能 新DB適応に向けた機能開発
                Next
                
                '$14 start UAT課題#158

                '顧客、車両がない場合は予約確認しない
                If cstId.Count <> 0 And vclId.Count <> 0 then

                    reservData = adapter.GetReservData(cstId, vclId, dateSt, StallRangeDays(dealerCode, storeCode), dealerCode, storeCode)
                    ' $15 start ゲートキーパーのユーザ表示対応
                    'ユーザ予約確認対象の車両登録番号
                    Dim regNumList As New Collection()
                    Dim allReservData As SC3090301AllReservDataDataTable

                    UpdateReservFlg(reservData, unsentTblWork, regNumList)
                    ' $15 end   ゲートキーパーのユーザ表示対応

                    ''予約が存在する場合、予約フラグを変更する
                    'Dim whereParam As New StringBuilder
                    'For Each unsentDataRowustomerRow As SC3090301VisitUnsentDataRow in unsentTblWork
                    '    If Not String.IsNullOrEmpty(unsentDataRowustomerRow.CUSTCD) and Not String.IsNullOrEmpty(unsentDataRowustomerRow.SEQNO)
                    
                    '        whereParam.Append(" CUSTCD = '" & unsentDataRowustomerRow.CUSTCD & "'")
                    '        whereParam.Append(" AND SEQNO = '" & unsentDataRowustomerRow.SEQNO & "'")
                    '        Dim selectRow() As DataRow = reservData.Select(whereParam.ToString)
                    '        whereParam.Clear()

                    '        '予約ありに更新する
                    '        If selectRow.Length <> 0 Then
                    '            unsentDataRowustomerRow.RESERVFLG = ReservFlagOn
                    '        ' $15 start ゲートキーパーのユーザ表示対応
                    '        Else
                    '            'ユーザ検索用の車両登録Noリスト作成
                    '            regNumList.Add(visitReception.ConvertVclRegNumWord(unsentDataRowustomerRow.VCLREGNO, regNumDlmtr).ToUpper)
                    '        End If
                    '        ' $15 end   ゲートキーパーのユーザ表示対応
                    '    End If
                    'Next
                                            
                    ' $15 start ゲートキーパーのユーザ表示対応
                    If regNumList.Count > 0 then

                        '予約あり未送信データの取得(全て)
                        allReservData = adapter.GetAllReservData(dealerCode, storeCode, regNumList, dateSt, StallRangeDays(dealerCode, storeCode))
                        UpdateAllReservFlg(allReservData, unsentTblWork)

                        'Dim orderByParam As New StringBuilder                                                    
                        'orderByParam.Append(" SCHESTARTDATETIME ")
                        'orderByParam.Append(" , CUSTOMERFLAG ")
                        'orderByParam.Append(" , CUSTVCLTYPE ")
                        'For Each unsentDataRowustomerRow As SC3090301VisitUnsentDataRow in unsentTblWork
                        '    If Not String.IsNullOrEmpty(unsentDataRowustomerRow.CUSTCD) and Not String.IsNullOrEmpty(unsentDataRowustomerRow.SEQNO)
                        '        '予約なしの場合
                        '        If unsentDataRowustomerRow.RESERVFLG = ReservFlagOff then
                        '            whereParam.Append(" VCLREGNO = '" & unsentDataRowustomerRow.VCLREGNO & "'")
                        '            Dim selectRow() As DataRow = allReservData.Select(whereParam.ToString, orderByParam.ToString)
                        '            whereParam.Clear

                        '            '予約ありに更新する
                        '            If selectRow.Length <> 0 Then
                        '                unsentDataRowustomerRow.RESERVFLG = ReservFlagOn
                        '                unsentDataRowustomerRow.NAME = CStr(selectRow(0)("NAME"))
                        '                unsentDataRowustomerRow.NAMETITLE = CStr(selectRow(0)("NAMETITLE"))
                        '            End If
                        '        End If
                        '    End If
                        'Next
                    End If
                    ' $15 end   ゲートキーパーのユーザ表示対応
                    
                End If
                
                'ソート
                Using orderView As DataView = New DataView(unsentTblWork)
                    orderView.Sort = "RESERVFLG DESC, CUSTKBN ASC, NAME ASC"
                    For Each orderRowView As DataRowView In orderView
                        unsentTbl.ImportRow(orderRowView.Row)
                    Next
                End Using

                '$14 end UAT課題#158

            End Using
        End Using

        Logger.Info("GetUnsentData_End Ret[" & (sc3090301DataSet IsNot Nothing) & "]")
        Return sc3090301DataSet

    End Function
    '$04 End

    ' $15 start ゲートキーパーのユーザ表示対応
    ''' <summary>
    ''' 未送信データの予約フラグ更新
    ''' </summary>
    ''' <param name="reservData">予約あり未送信データ</param>
    ''' <param name="unsentTblWork">未送信データ</param>
    ''' <param name="regNumList">ユーザ予約確認対象の車両登録番号</param>
    ''' <remarks></remarks>
    Private Sub UpdateReservFlg(ByVal reservData As SC3090301ReservDataDataTable, ByRef unsentTblWork As SC3090301VisitUnsentDataDataTable, ByRef regNumList As Collection)

        Dim visitReception As New VisitReceptionBusinessLogic
        '車両登録番号区切り文字
        Dim regNumDlmtr As String = visitReception.GetSystemSettingValueBySettingName(SysRegNumDlmtr)

        '予約が存在する場合、予約フラグを変更する
        Dim whereParam As New StringBuilder
        For Each unsentDataRowustomerRow As SC3090301VisitUnsentDataRow in unsentTblWork
            If Not String.IsNullOrEmpty(unsentDataRowustomerRow.CUSTCD) and Not String.IsNullOrEmpty(unsentDataRowustomerRow.SEQNO)
                    
                whereParam.Append(" CUSTCD = '" & unsentDataRowustomerRow.CUSTCD & "'")
                whereParam.Append(" AND SEQNO = '" & unsentDataRowustomerRow.SEQNO & "'")
                Dim selectRow() As DataRow = reservData.Select(whereParam.ToString)
                whereParam.Clear()

                '予約ありに更新する
                If selectRow.Length <> 0 Then
                    unsentDataRowustomerRow.RESERVFLG = ReservFlagOn
                ' $15 start ゲートキーパーのユーザ表示対応
                Else
                    'ユーザ検索用の車両登録Noリスト作成
                    regNumList.Add(visitReception.ConvertVclRegNumWord(unsentDataRowustomerRow.VCLREGNO, regNumDlmtr).ToUpper(CultureInfo.CurrentCulture))
                End If
                ' $15 end   ゲートキーパーのユーザ表示対応
            End If
        Next
    End Sub
    ' $15 end   ゲートキーパーのユーザ表示対応

    ' $15 start ゲートキーパーのユーザ表示対応
    ''' <summary>
    ''' 未送信データ(全て)の予約フラグ更新
    ''' </summary>
    ''' <param name="allReservData">予約あり未送信データ(全て)</param>
    ''' <param name="unsentTblWork">未送信データ</param>
    ''' <remarks></remarks>
    Private Sub UpdateAllReservFlg(ByVal allReservData As SC3090301AllReservDataDataTable, ByRef unsentTblWork As SC3090301VisitUnsentDataDataTable)

        Dim whereParam As New StringBuilder
        Dim orderByParam As New StringBuilder                                                    
        orderByParam.Append(" SCHESTARTDATETIME ")
        orderByParam.Append(" , CUSTOMERFLAG ")
        orderByParam.Append(" , CUSTVCLTYPE ")
        For Each unsentDataRowustomerRow As SC3090301VisitUnsentDataRow in unsentTblWork
            If Not String.IsNullOrEmpty(unsentDataRowustomerRow.CUSTCD) and Not String.IsNullOrEmpty(unsentDataRowustomerRow.SEQNO)
                '予約なしの場合
                If unsentDataRowustomerRow.RESERVFLG = ReservFlagOff then

                    ' $17 start シングルクォーテーション
                    'whereParam.Append(" VCLREGNO = '" & unsentDataRowustomerRow.VCLREGNO & "'")
                    whereParam.Append(" VCLREGNO = '" & unsentDataRowustomerRow.VCLREGNO.Replace("'", "''") & "'")
                    ' $17 end シングルクォーテーション

                    Dim selectRow() As DataRow = allReservData.Select(whereParam.ToString, orderByParam.ToString)
                    whereParam.Clear

                    '予約ありに更新する
                    If selectRow.Length <> 0 Then
                        unsentDataRowustomerRow.RESERVFLG = ReservFlagOn
                        unsentDataRowustomerRow.NAMEDISP = CStr(selectRow(0)("NAME"))
                        unsentDataRowustomerRow.NAMETITLEDISP = CStr(selectRow(0)("NAMETITLE"))
                    End If
                End If
            End If
        Next

    End Sub
    ' $15 end   ゲートキーパーのユーザ表示対応

#End Region

#Region "通知関連"

#Region "新規"

    ''' <summary>
    ''' 送信処理_新規(セールス)
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="storeCode">店舗コード</param>
    ''' <param name="visitDate">来店日時</param>
    ''' <param name="visitPersonNumber">来店人数</param>
    ''' <param name="visitMeans">来店手段</param>
    ''' <param name="account">アカウント</param>
    ''' <param name="regNum">車両登録番号</param>
    ''' <returns>メッセージID</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加
    ''' </history>
    <EnableCommit()>
    Public Function SendNewSales(ByVal dealerCode As String, ByVal storeCode As String, _
                            ByVal visitDate As Date, ByVal visitPersonNumber As String, _
                            ByVal visitMeans As String, ByVal account As String, _
                            ByVal regNum As String) As Integer Implements ISC3090301BusinessLogic.SendNewSales
        '2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 START
        'Public Function SendNewSales(ByVal dealerCode As String, ByVal storeCode As String, _
        '                ByVal visitDate As Date, ByVal visitPersonNumber As String, _
        '                ByVal visitMeans As String, ByVal account As String) As Integer Implements ISC3090301BusinessLogic.SendNewSales
        '2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 END
        Logger.Info("SendNewSales_Start Pram[" & dealerCode & "," & storeCode & "," & _
                     visitDate & "," & visitPersonNumber & "," & visitMeans & "," & account & "]")

        ' $10 START 次世代e-CRBセールス機能 新DB適応に向けた機能開発
        Dim message As Integer = MessageIdSuccess

        Try

            VisitSalesDataRow.DEALERCODE = dealerCode
            VisitSalesDataRow.STORECODE = storeCode
            VisitSalesDataRow.VISITTIME = visitDate
            VisitSalesDataRow.VISITPERSONNUMBER = CInt(visitPersonNumber)
            VisitSalesDataRow.VISITMEANS = visitMeans
            VisitSalesDataRow.CREATEACCOUNT = account
            VisitSalesDataRow.FUNCTIONID = ApplicationId
            '2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 START
            VisitSalesDataRow.VEHICLEREGNO = regNum
            '2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 END
            Dim visitSales As New VisitReceptionBusinessLogic
            message = visitSales.CreateCustomerChip(VisitSalesDataRow, False)

        Catch oraEx As OracleExceptionEx

            If oraEx.Number = ErrorCodeOra2049 Then

                'ロールバックを行う。
                Me.Rollback = True

                'ログ出力
                Logger.Error(CStr(MessageIdErrorDbTimeOut), oraEx)

                'DBタイムアウトエラー時
                Logger.Info("SendNewSales_End Ret[messageId=" & MessageIdErrorDbTimeOut & "]")
                Return MessageIdErrorDbTimeOut
            Else
                '上記以外のエラーは基盤側で制御
                Throw
            End If
        End Try

        Logger.Info("SendNewSales_End Ret[messageId=" & MessageIdSuccess & "]")
        Return message
        ' $10 END   次世代e-CRBセールス機能 新DB適応に向けた機能開発

    End Function

    ''' <summary>
    ''' 送信処理_新規(サービス)
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="storeCode">店舗コード</param>
    ''' <param name="visitDate">来店日時</param>
    ''' <param name="visitPersonNumber">来店人数</param>
    ''' <param name="visitMeans">来店手段</param>
    ''' <param name="account">アカウント</param>
    ''' <param name="userName">アカウント名</param>
    ''' <param name="regNum">車両登録番号</param>
    ''' <returns>メッセージID</returns>
    ''' <history>
    ''' 2014/01/17 TMEJ 陳   TMEJ次世代サービス 工程管理機能開発 $11
    ''' 2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加
    ''' </history>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Function SendNewService(ByVal dealerCode As String, ByVal storeCode As String, _
                            ByVal visitDate As Date, ByVal visitPersonNumber As String, _
                            ByVal visitMeans As String, ByVal account As String, _
                            ByVal userName As String, ByVal regNum As String) As Integer
        '2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 START
        'Public Function SendNewService(ByVal dealerCode As String, ByVal storeCode As String, _
        '                        ByVal visitDate As Date, ByVal visitPersonNumber As String, _
        '                        ByVal visitMeans As String, ByVal account As String, _
        '                        ByVal userName As String) As Integer
        '2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 END
        '2014/01/17 TMEJ 陳   TMEJ次世代サービス 工程管理機能開発 $11 START
        'Public Function SendNewService(ByVal dealerCode As String, ByVal storeCode As String, _
        '                        ByVal visitDate As Date, ByVal visitPersonNumber As String, _
        '                        ByVal visitMeans As String, ByVal account As String) As Integer
        '2014/01/17 TMEJ 陳   TMEJ次世代サービス 工程管理機能開発 $11 END

        Logger.Info("SendNewService_Start Pram[" & dealerCode & "," & storeCode & "," & _
                     visitDate & "," & visitPersonNumber & "," & visitMeans & "," & account & "]")

        Dim messageId As Long = MessageIdSuccess
        Try
            'サービス来店実績テーブルの作成
            '2014/01/17 TMEJ 陳   TMEJ次世代サービス 工程管理機能開発 $11 START
            'Using ic3810101 As IC3810101BusinessLogic = New IC3810101BusinessLogic
            '2014/01/17 TMEJ 陳   TMEJ次世代サービス 工程管理機能開発 $11 START
            'messageId = ic3810101.InsertServiceVisit(dealerCode, _
            '                   storeCode, _
            '                   visitDate, _
            '                   Nothing, _
            '                   Nothing, _
            '                   Nothing, _
            '                   Nothing, _
            '                   CShort(visitPersonNumber), _
            '                   visitMeans, _
            '                   Nothing, _
            '                   0, _
            '                   Nothing, _
            '                   Nothing, _
            '                   Nothing, _
            '                   account, _
            '                   ApplicationId)
            '2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 START
            'Dim ic3810101 As IC3810101BusinessLogic = New IC3810101BusinessLogic
            'messageId = ic3810101.InsertServiceVisit(dealerCode, _
            '                   storeCode, _
            '                   visitDate, _
            '                   Nothing, _
            '                   Nothing, _
            '                   Nothing, _
            '                   Nothing, _
            '                   CShort(visitPersonNumber), _
            '                   visitMeans, _
            '                   Nothing, _
            '                   0, _
            '                   Nothing, _
            '                   Nothing, _
            '                   Nothing, _
            '                   account, _
            '                   userName, _
            '                   ApplicationId)
            'End Using
            '2014/01/17 TMEJ 陳   TMEJ次世代サービス 工程管理機能開発 $11 END
            Using ic3810101 As IC3810101BusinessLogic = New IC3810101BusinessLogic
                messageId = ic3810101.InsertServiceVisit(dealerCode, _
                                   storeCode, _
                                   visitDate, _
                                   regNum, _
                                   Nothing, _
                                   Nothing, _
                                   Nothing, _
                                   CShort(visitPersonNumber), _
                                   visitMeans, _
                                   Nothing, _
                                   0, _
                                   Nothing, _
                                   Nothing, _
                                   Nothing, _
                                   account, _
                                   userName, _
                                   ApplicationId, _
                                   SvcinIdNotIdentified)
                '2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 END

                ' DBタイムアウトエラー時
                If Not messageId = MessageIdSuccess Then

                    '2014/01/17 TMEJ 陳   TMEJ次世代サービス 工程管理機能開発 $11 START
                    'ロールバックを行う。
                    Me.Rollback = True
                    '2014/01/17 TMEJ 陳   TMEJ次世代サービス 工程管理機能開発 $11 END

                    Logger.Debug("SendNewService_001 ShowDefeatMessage")

                    Logger.Info("SendNewService_End Ret[" & messageId & "]")
                    Return MessageIdErrorDbTimeOut
                End If

                '2014/01/17 TMEJ 陳   TMEJ次世代サービス 工程管理機能開発 $11 START

                '現在時間
                Dim dateNow As DateTime = DateTime.Now

                '通知処理
                ic3810101.NoticeProcessing(ic3810101.VisitSeqInserted, _
                                     dateNow, _
                                     dealerCode, _
                                     storeCode, _
                                     account, _
                                     userName)

                '2014/01/17 TMEJ 陳   TMEJ次世代サービス 工程管理機能開発 $11 END

                Logger.Info("SendNewService_End Ret[" & messageId & "]")
                Return CInt(messageId)

                '2014/01/17 TMEJ 陳   TMEJ次世代サービス 工程管理機能開発 $11 START
                'Catch ex As OracleExceptionEx When ex.Number = ErrorCodeOra2049

            End Using

            '    Return MessageIdErrorDbTimeOut
        Catch oraEx As OracleExceptionEx

            If oraEx.Number = ErrorCodeOra2049 Then

                'ロールバックを行う。
                Me.Rollback = True

                'ログ出力
                Logger.Error(CStr(MessageIdErrorDbTimeOut), oraEx)

                'DBタイムアウトエラー時
                Logger.Info("SendNotCustomerInfo_End Ret[" & MessageIdErrorDbTimeOut & "]")
                Return MessageIdErrorDbTimeOut
            Else
                '上記以外のエラーは基盤側で制御
                Throw
            End If
            '2014/01/17 TMEJ 陳   TMEJ次世代サービス 工程管理機能開発 $11 END
        End Try

    End Function

#End Region

#Region "自社客・未取引客"

    ''' <summary>
    ''' 送信処理_自社客・未取引客(セールス)
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="storeCode">店舗コード</param>
    ''' <param name="visitPersonNumber">来店人数</param>
    ''' <param name="visitMeans">来店手段</param>
    ''' <param name="account">アカウント</param>
    ''' <param name="unsentRow">来店通知未送信データロウ</param>
    ''' <returns>メッセージID</returns>
    ''' <remarks></remarks>
    Private Function SendOrgOrNewCustomerSales(ByVal dealerCode As String, ByVal storeCode As String, _
                                          ByVal visitPersonNumber As String, _
                                          ByVal visitMeans As String, ByVal account As String, _
                                          ByVal unsentRow As SC3090301VisitUnsentDataRow) As Integer

        Logger.Debug("SendOrgOrNewCustomerSales_Start Pram[" & dealerCode & "," & storeCode & "," & visitPersonNumber & "," & _
                     visitMeans & "," & account & "," & (unsentRow IsNot Nothing) & "]")

        ' $10 START 次世代e-CRBセールス機能 新DB適応に向けた機能開発
        Dim messageId As Integer = MessageIdSuccess

        ' 苦情情報
        isClaimeInfo = HasClaimed(dealerCode, unsentRow.CUSTKBN, unsentRow.CUSTCD)
        VisitSalesDataRow.CUSTNAME = unsentRow.NAME
        VisitSalesDataRow.CUSTNAMETITLE = unsentRow.NAMETITLE
        VisitSalesDataRow.DEALERCODE = dealerCode
        VisitSalesDataRow.STORECODE = storeCode
        VisitSalesDataRow.VISITTIME = unsentRow.VISITTIMESTAMP
        VisitSalesDataRow.VEHICLEREGNO = unsentRow.VCLREGNO
        VisitSalesDataRow.CUSTOMERSEGMENT = unsentRow.CUSTKBN
        VisitSalesDataRow.CUSTOMERID = unsentRow.CUSTCD
        VisitSalesDataRow.CUSTYPE = unsentRow.CUSTYPE
        '顧客担当スタッフ
        VisitSalesDataRow.STAFFCODE = unsentRow.STUFFCD
        VisitSalesDataRow.VISITPERSONNUMBER = CInt(visitPersonNumber)
        VisitSalesDataRow.VISITMEANS = visitMeans
        '更新アカウント
        VisitSalesDataRow.CREATEACCOUNT = account
        VisitSalesDataRow.FUNCTIONID = ApplicationId
        Dim visitSales As New VisitReceptionBusinessLogic

        messageId = visitSales.CreateCustomerChip(VisitSalesDataRow, isClaimeInfo)
        Logger.Debug("SendOrgOrNewCustomerSales_End Ret[" & messageId & "]")

        If String.Equals(messageId, "900") Then
            messageId = MessageIdErrorDbTimeOut
        End If

        Return messageId
        ' $10 END   次世代e-CRBセールス機能 新DB適応に向けた機能開発

    End Function

    ' $01 start step2開発
    ''' <summary>
    ''' 苦情情報の取得
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="customerSegment">顧客区分</param>
    ''' <param name="customerCode">顧客コード</param>
    ''' <returns>苦情情報の有無</returns>
    ''' <remarks></remarks>
    Private Function HasClaimed(ByVal dealerCode As String, ByVal customerSegment As String, _
                                   ByVal customerCode As String) As Boolean
        Logger.Debug("HasClaimed_Start Pram[" & dealerCode & "," & customerSegment & "," & customerCode & "]")

        '現在日時の取得
        Logger.Debug("HasClaimed_001 " & "Call_Start DateTimeFunc.Now Param[" & dealerCode & "]")
        Dim now As Date = DateTimeFunc.Now(dealerCode)
        Logger.Debug("HasClaimed_001 " & "Call_End   DateTimeFunc.Now Ret[" & now & "]")

        '苦情表示日数の取得
        ' システム環境設定（苦情表示日数の取得）
        Dim sysEnvSet As New SystemEnvSetting
        Dim sysEnvSetComplaintDisplayDateRow As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow = Nothing
        Logger.Info("HasClaimed_002 " & "Call_Start GetSystemEnvSetting Param[" & ComplaintDisplayDate & "]")
        sysEnvSetComplaintDisplayDateRow = sysEnvSet.GetSystemEnvSetting(ComplaintDisplayDate)
        Logger.Info("HasClaimed_002 " & "Call_End GetSystemEnvSetting Ret[" & IsDBNull(sysEnvSetComplaintDisplayDateRow) & "]")
        Dim complaintDateCount As Integer = CType(sysEnvSetComplaintDisplayDateRow.PARAMVALUE, Integer)

        sysEnvSet = Nothing
        sysEnvSetComplaintDisplayDateRow = Nothing

        '苦情情報有無の取得
        Dim utility As New VisitUtilityBusinessLogic
        Dim isClaim As Boolean = utility.HasClaimInfo(customerSegment, customerCode, now, complaintDateCount)
        utility = Nothing

        Logger.Debug("HasClaimed_End Ret[" & isClaim.ToString & "]")
        Return isClaim
    End Function
    ' $01 end   step2開発

    ' $01 start step2開発
    ''' <summary>
    ''' 敬称付きお客様名作成
    ''' </summary>
    ''' <param name="customerName">お客様名</param>
    ''' <param name="customerNameTitle">お客様敬称</param>
    ''' <returns>敬称付きお客様名</returns>
    ''' <remarks></remarks>
    Private Function CreateCustomerName(ByVal customerName As String, ByVal customerNameTitle As String) As String
        Logger.Debug("CreateCustomerName_Start Pram[" & customerName & "," & customerNameTitle & "]")

        '敬称表示位置を取得
        Dim sysEnvSet As New SystemEnvSetting
        Dim sysEnvSetRow As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow
        Logger.Info("CreateCustomerName_001 " & "Call_Start SystemEnvSetting.GetSystemEnvSetting Pram[" & KeisyoZengo & "]")
        sysEnvSetRow = sysEnvSet.GetSystemEnvSetting(KeisyoZengo)
        Logger.Info("CreateCustomerName_001 " & "Call_End SystemEnvSetting.GetSystemEnvSetting Ret[" & (sysEnvSetRow IsNot Nothing) & "]")

        'お客様名作成
        Dim result As String
        If String.Equals(sysEnvSetRow.PARAMVALUE, HonorificTitleMae) Then

            Logger.Debug("CreateCustomerName_002 HonorificTitleMae")

            ' 敬称表示位置が前
            result = customerNameTitle & " " & customerName
        Else
            Logger.Debug("CreateCustomerName_003 HonorificTitleUshiro")

            ' 敬称表示位置が後
            result = customerName & " " & customerNameTitle
        End If

        Logger.Debug("CreateCustomerName_End Ret[" & result & "]")
        Return result

    End Function
    ' $01 end   step2開発

    ' $01 start step2開発
    ''' <summary>
    ''' 送信メッセージ作成
    ''' </summary>
    ''' <param name="customerName">顧客名</param>
    ''' <param name="vehicleNo">車両登録No</param>
    ''' <param name="message">通知メッセージ</param>
    ''' <param name="claimeInfo">苦情有無</param>
    ''' <returns>送信メッセージ</returns>
    ''' <remarks></remarks>
    Private Function CreateSendMessage(ByVal customerName As String, ByVal vehicleNo As String, _
                                       ByVal message As String, ByVal claimeInfo As Boolean) As String
        Logger.Debug("CreateSendMessage_Start Pram[" & customerName & "," & vehicleNo & "," & message & "," & claimeInfo.ToString & "]")

        '送信メッセージ
        Dim pushStandbyStuffMessage As New StringBuilder

        '苦情情報の有無を判定
        If claimeInfo Then
            Logger.Debug("CreateSendMessage_001 " & "Call_Start WebWordUtility.GetWord Param[" & ClameWord & "]")
            Dim claimMessage As String = WebWordUtility.GetWord(ClameWord)
            Logger.Debug("CreateSendMessage_001 " & "Call_End WebWordUtility.GetWord")
            pushStandbyStuffMessage.Append(claimMessage)
            pushStandbyStuffMessage.Append(" ")
        End If

        '送信メッセージ作成
        pushStandbyStuffMessage.Append(customerName)
        pushStandbyStuffMessage.Append(" ")
        pushStandbyStuffMessage.Append(message)
        pushStandbyStuffMessage.Append(" ")
        pushStandbyStuffMessage.Append(vehicleNo)

        Logger.Debug("CreateSendMessage_End Ret[" & pushStandbyStuffMessage.ToString & "]")
        Return pushStandbyStuffMessage.ToString

    End Function
    ' $01 end   step2開発

    ''' <summary>
    ''' 送信処理_自社客・未取引客(サービス)
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="storeCode">店舗コード</param>
    ''' <param name="visitPersonNumber">来店人数</param>
    ''' <param name="visitMeans">来店手段</param>
    ''' <param name="account">アカウント</param>
    ''' <param name="unsentRow">来店通知未送信データロウ</param>
    ''' <returns>メッセージID</returns>
    ''' <remarks></remarks>
    Private Function SendOrgOrNewCustomerService(ByVal dealerCode As String, ByVal storeCode As String, _
                                            ByVal visitPersonNumber As String, _
                                            ByVal visitMeans As String, ByVal account As String, _
                                            ByVal userName As String, _
                                            ByVal unsentRow As SC3090301VisitUnsentDataRow) As Integer

        '2014/01/17 TMEJ 陳   TMEJ次世代サービス 工程管理機能開発 $11 START
        'Private Function SendOrgOrNewCustomerService(ByVal dealerCode As String, ByVal storeCode As String, _
        '                                        ByVal visitPersonNumber As String, _
        '                                        ByVal visitMeans As String, ByVal account As String, _
        '                                        ByVal unsentRow As SC3090301VisitUnsentDataRow) As Integer
        '2014/01/17 TMEJ 陳   TMEJ次世代サービス 工程管理機能開発 $11 END

        Logger.Debug("SendOrgOrNewCustomerService_Start Pram[" & dealerCode & "," & storeCode & "," & visitPersonNumber & "," & _
                     visitMeans & "," & account & "," & (unsentRow IsNot Nothing) & "]")

        Dim messageId As Long = MessageIdSuccess

        '2014/01/17 TMEJ 陳   TMEJ次世代サービス 工程管理機能開発 $11 START
        Dim visitSeq As Long
        '2014/01/17 TMEJ 陳   TMEJ次世代サービス 工程管理機能開発 $11 END

        'サービス来店実績テーブルの作成
        Using ic3810101Biz As IC3810101BusinessLogic = New IC3810101BusinessLogic

            '2014/01/17 TMEJ 陳   TMEJ次世代サービス 工程管理機能開発 $11 START
            'messageId = ic3810101Biz.InsertServiceVisit(dealerCode, _
            '                       storeCode, _
            '                       unsentRow.VISITTIMESTAMP, _
            '                       unsentRow.VCLREGNO, _
            '                       unsentRow.CUSTKBN, _
            '                       unsentRow.CUSTCD, _
            '                       unsentRow.STUFFCD, _
            '                       CShort(visitPersonNumber), _
            '                       visitMeans, _
            '                       CStr(IIf(String.Equals(unsentRow.CUSTKBN, CustKubunOrg), unsentRow.VIN, Nothing)),
            '                       CLng(unsentRow.SEQNO), _
            '                       unsentRow.SEX, _
            '                       unsentRow.NAME, _
            '                       unsentRow.SACODE, _
            '                       account, _
            '                       ApplicationId)
            '2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 START
            'messageId = ic3810101Biz.InsertServiceVisit(dealerCode, _
            '                       storeCode, _
            '                       unsentRow.VISITTIMESTAMP, _
            '                       unsentRow.VCLREGNO, _
            '                       unsentRow.CUSTKBN, _
            '                       unsentRow.CUSTCD, _
            '                       unsentRow.STUFFCD, _
            '                       CShort(visitPersonNumber), _
            '                       visitMeans, _
            '                       CStr(IIf(String.Equals(unsentRow.CUSTKBN, CustKubunOrg), unsentRow.VIN, Nothing)),
            '                       CDec(unsentRow.SEQNO), _
            '                       unsentRow.SEX, _
            '                       unsentRow.NAME, _
            '                       unsentRow.SACODE, _
            '                       account, _
            '                       userName,
            '                       ApplicationId)
            messageId = ic3810101Biz.InsertServiceVisit(dealerCode, _
                       storeCode, _
                       unsentRow.VISITTIMESTAMP, _
                       unsentRow.VCLREGNO, _
                       unsentRow.CUSTKBN, _
                       unsentRow.CUSTCD, _
                       unsentRow.STUFFCD, _
                       CShort(visitPersonNumber), _
                       visitMeans, _
                       CStr(IIf(String.Equals(unsentRow.CUSTKBN, CustKubunOrg), unsentRow.VIN, Nothing)),
                       CDec(unsentRow.SEQNO), _
                       unsentRow.SEX, _
                       unsentRow.NAME, _
                       unsentRow.SACODE, _
                       account, _
                       userName,
                       ApplicationId, _
                       SvcinIdNotSpecified)
            '2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 END

            visitSeq = ic3810101Biz.VisitSeqInserted
            '2014/01/17 TMEJ 陳   TMEJ次世代サービス 工程管理機能開発 $11 END

        End Using

        ' DBタイムアウトエラー時
        If Not messageId = MessageIdSuccess Then
            Logger.Debug("SendOrgOrNewCustomerService_001 ShowDefeatMessage")

            Logger.Debug("SendOrgOrNewCustomerService_End Ret[" & MessageIdErrorDbTimeOut & "]")
            Return MessageIdErrorDbTimeOut
        End If

        ' $05 start 担当SCへ通知
        SendOrgCustomerServiceToSC(dealerCode, storeCode, unsentRow)
        ' $05 end 担当SCへ通知

        '2014/01/17 TMEJ 陳   TMEJ次世代サービス 工程管理機能開発 $11 START

        '現在時間
        Dim dateNow As DateTime = DateTime.Now

        '通知処理
        Using ic3810101Biz2 As IC3810101BusinessLogic = New IC3810101BusinessLogic
            ic3810101Biz2.NoticeProcessing(visitSeq, _
                                 dateNow, _
                                 dealerCode, _
                                 storeCode, _
                                 account, _
                                 userName)
        End Using

        '2014/01/17 TMEJ 陳   TMEJ次世代サービス 工程管理機能開発 $11 END

        Logger.Debug("SendOrgOrNewCustomerService_End Ret[" & MessageIdSuccess & "]")
        Return MessageIdSuccess
    End Function

    ' $05 start サービス入庫時セールス担当スタッフへ通知
    ''' <summary>
    ''' 送信処理_自社客・未取引客(サービス入庫の時担当SCへ)
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="storeCode">店舗コード</param>
    ''' <param name="unsentRow">来店通知未送信データロウ</param>
    ''' <returns>メッセージID</returns>
    ''' <remarks></remarks>
    Private Function SendOrgCustomerServiceToSC(ByVal dealerCode As String, ByVal storeCode As String, _
                                          ByVal unsentRow As SC3090301VisitUnsentDataRow) As Integer

        Dim isExistCustStuff As Boolean = False
        Dim messageId As Integer = MessageIdSuccess
        ' $10 START 次世代e-CRBセールス機能 新DB適応に向けた機能開発
        Dim operationCodeList As New List(Of Decimal)
        operationCodeList.Add(8)
        Dim presenceCategoryList As New List(Of String)
        presenceCategoryList.Add(PresenceCategoryStandby)
        presenceCategoryList.Add(PresenceCategoryNegotiate)
        presenceCategoryList.Add(PresenceCategoryLeaving)
        presenceCategoryList.Add(PresenceCategoryOffline)

        Logger.Debug("SendOrgOrNewCustomerService_Start Pram[" & dealerCode & "," _
                     & storeCode & "," & (unsentRow IsNot Nothing) & "]")

        Using adapter As New SC3090301TableAdapter

            '来店実績連番の取得
            Dim visitSequence As Long
            visitSequence = adapter.GetVisitSalesSeqNextValue()

            '顧客担当スタッフの有無
            If Not String.IsNullOrEmpty(Trim(unsentRow.STUFFCD)) Then
                ' $06 start
                '顧客担当スタッフが存在する
                isExistCustStuff = True
                Logger.Debug("SendOrgOrNewCustomerService_001 CustomerStuff Exist")

                '顧客担当スタッフのステータス取得
                Using staffStatusTbl As VisitUtilityUsersDataTable = _
                    VisitUtilityDataSetTableAdapter.GetUsers(dealerCode, storeCode, _
                        operationCodeList, presenceCategoryList, "0", unsentRow.STUFFCD)

                    If staffStatusTbl.Rows.Count <= 0 Then
                        isExistCustStuff = False
                    End If

                End Using

            End If

            '顧客担当スタッフが存在しない場合は、処理を抜ける
            If Not isExistCustStuff Then

                Logger.Debug("SendOrgOrNewCustomerService_End Ret[" & MessageIdSuccess & "]")
                Return MessageIdSuccess
            End If
            ' $06 end
            'お客様名
            Dim custName As String = CreateCustomerName(unsentRow.NAME, unsentRow.NAMETITLE)

            'スタッフ情報の取得(セールススタッフ)
            Using salesStuffInfoTbl As VisitUtilityUsersDataTable = _
                VisitUtilityDataSetTableAdapter.GetUsers(dealerCode, storeCode, _
                    operationCodeList, presenceCategoryList, "0", unsentRow.STUFFCD)

                ' 苦情情報
                Dim isClaime As Boolean = HasClaimed(dealerCode, unsentRow.CUSTKBN, unsentRow.CUSTCD)

                ' 顧客担当SCへサービス入庫通知を送信
                '2014/01/17 TMEJ 陳   TMEJ次世代サービス 工程管理機能開発 $12 START
                'messageId = SendStandbyStuff(salesStuffInfoTbl, unsentRow.STUFFCD, _
                '                 CreateSendMessage(custName, unsentRow.VCLREGNO, MESSAGE_SERVICE, isClaime), _
                '                 unsentRow.CUSTCD, custName, unsentRow.CUSTKBN, visitSequence, False)
                messageId = SendStandbyStuff(salesStuffInfoTbl, unsentRow.STUFFCD, _
                                 CreateSendMessage(custName, unsentRow.VCLREGNO, MESSAGE_SERVICE, isClaime), _
                                 unsentRow.CUSTCD, custName, unsentRow.CUSTKBN, visitSequence)
                '2014/01/17 TMEJ 陳   TMEJ次世代サービス 工程管理機能開発 $12 END
            End Using

        End Using
        ' $10 END   次世代e-CRBセールス機能 新DB適応に向けた機能開発

        Logger.Debug("SendCustomerInfoToStuff_End Ret[" & messageId & "]")
        Return messageId

    End Function
    ' $05 end サービス入庫時セールス担当スタッフへ通知

    ''' <summary>
    ''' 送信処理_自社客・未取引客
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="storeCode">店舗コード</param>
    ''' <param name="visitPersonNumber">来店人数</param>
    ''' <param name="visitPurpose">来店目的</param>
    ''' <param name="visitMeans">来店手段</param>
    ''' <param name="account">アカウント</param>
    ''' <param name="userName">アカウントユーザ名</param>
    ''' <param name="unsentRow">来店通知未送信データロウ</param>
    ''' <returns>メッセージID</returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Function SendOrgOrNewCustomer(ByVal dealerCode As String, ByVal storeCode As String, _
                                         ByVal visitPersonNumber As String, ByVal visitPurpose As String, _
                                         ByVal visitMeans As String, ByVal account As String, _
                                         ByVal userName As String, _
                                         ByVal unsentRow As SC3090301VisitUnsentDataRow) As Integer Implements ISC3090301BusinessLogic.SendOrgOrNewCustomer
        '2014/01/17 TMEJ 陳   TMEJ次世代サービス 工程管理機能開発 $11 START
        'Public Function SendOrgOrNewCustomer(ByVal dealerCode As String, ByVal storeCode As String, _
        '                                     ByVal visitPersonNumber As String, ByVal visitPurpose As String, _
        '                                     ByVal visitMeans As String, ByVal account As String, _
        '                                     ByVal unsentRow As SC3090301VisitUnsentDataRow) As Integer Implements ISC3090301BusinessLogic.SendOrgOrNewCustomer
        '2014/01/17 TMEJ 陳   TMEJ次世代サービス 工程管理機能開発 $11 END

        Logger.Info("SendOrgOrNewCustomer_Start Pram[" & dealerCode & "," & storeCode & "," & visitPersonNumber & "," & _
                     visitPurpose & "," & visitMeans & "," & account & "," & (unsentRow IsNot Nothing) & "]")

        Dim messageId As Integer = MessageIdSuccess
        Try

            '来店車両実績テーブルの更新
            Using adapter As New SC3090301TableAdapter
                adapter.UpdateVisitVehicle(CStr(unsentRow.VISITVCLSEQ), DealFlagSend, account, ApplicationId)
            End Using

            '来店目的がセールス
            If String.Equals(visitPurpose, VisitPurposeSales) Then

                Logger.Debug("SendOrgOrNewCustomer_001 VisitPurpose is Sales")

                '来店目的がセールス
                messageId = SendOrgOrNewCustomerSales(dealerCode, storeCode, visitPersonNumber, visitMeans, account, unsentRow)

            ElseIf String.Equals(visitPurpose, VisitPurposeService) Then

                Logger.Debug("SendOrgOrNewCustomer_002 VisitPurpose is Service")

                '来店目的がサービス
                '2014/01/17 TMEJ 陳   TMEJ次世代サービス 工程管理機能開発 $11 START
                'messageId = SendOrgOrNewCustomerService(dealerCode, storeCode, visitPersonNumber, visitMeans, account, unsentRow)
                messageId = SendOrgOrNewCustomerService(dealerCode, storeCode, visitPersonNumber, visitMeans, account, userName, unsentRow)
                '2014/01/17 TMEJ 陳   TMEJ次世代サービス 工程管理機能開発 $11 END

            End If

            ' DBタイムアウトエラー時
            If messageId = MessageIdErrorDbTimeOut Then
                'ロールバックを行う。
                Me.Rollback = True
            End If

        Catch oraEx As OracleExceptionEx

            If oraEx.Number = ErrorCodeOra2049 Then

                'ロールバックを行う。
                Me.Rollback = True

                'ログ出力
                Logger.Error(CStr(MessageIdErrorDbTimeOut), oraEx)

                'DBタイムアウトエラー時
                Logger.Info("SendOrgOrNewCustomer_End Ret[" & MessageIdErrorDbTimeOut & "]")
                Return MessageIdErrorDbTimeOut
            Else
                '上記以外のエラーは基盤側で制御
                Throw
            End If
        End Try

        Logger.Info("SendOrgOrNewCustomer_End Ret[" & messageId & "]")
        Return messageId

    End Function

#End Region

#Region "顧客情報なし"

    ''' <summary>
    ''' 送信処理_顧客情報なし
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="storeCode">店舗コード</param>
    ''' <param name="visitPersonNumber">来店人数</param>
    ''' <param name="visitPurpose">来店目的</param>
    ''' <param name="visitMeans">来店手段</param>
    ''' <param name="account">アカウント</param>
    ''' <param name="unsentRow">来店通知未送信データロウ</param>
    ''' <returns>メッセージID</returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Function SendNotCustomerInfo(ByVal dealerCode As String, ByVal storeCode As String, _
                                        ByVal visitPersonNumber As String, ByVal visitPurpose As String, _
                                        ByVal visitMeans As String, ByVal account As String, _
                                        ByVal userName As String, _
                                        ByVal unsentRow As SC3090301VisitUnsentDataRow) As Integer Implements ISC3090301BusinessLogic.SendNotCustomerInfo
        '2014/01/17 TMEJ 陳   TMEJ次世代サービス 工程管理機能開発 $11 START
        'Public Function SendNotCustomerInfo(ByVal dealerCode As String, ByVal storeCode As String, _
        '                                ByVal visitPersonNumber As String, ByVal visitPurpose As String, _
        '                                ByVal visitMeans As String, ByVal account As String, _
        '                                ByVal unsentRow As SC3090301VisitUnsentDataRow) As Integer Implements ISC3090301BusinessLogic.SendNotCustomerInfo
        '2014/01/17 TMEJ 陳   TMEJ次世代サービス 工程管理機能開発 $11 END

        Logger.Info("SendNotCustomerInfo_Start Pram[" & dealerCode & "," & storeCode & "," & visitPersonNumber & "," & _
                      visitMeans & "," & account & "," & (unsentRow IsNot Nothing) & "]")

        Try
            Using adapter As New SC3090301TableAdapter

                '来店車両実績テーブルの更新
                adapter.UpdateVisitVehicle(CStr(unsentRow.VISITVCLSEQ), DealFlagSend, account, ApplicationId)

                '来店目的がセールス
                If String.Equals(visitPurpose, VisitPurposeSales) Then

                    Logger.Debug("SendNotCustomerInfo_001 VisitPurpose is Sales")

                    ' $10 START 次世代e-CRBセールス機能 新DB適応に向けた機能開発
                    VisitSalesDataRow.DEALERCODE = dealerCode
                    VisitSalesDataRow.STORECODE = storeCode
                    VisitSalesDataRow.VISITTIME = unsentRow.VISITTIMESTAMP
                    VisitSalesDataRow.VEHICLEREGNO = unsentRow.VCLREGNO
                    VisitSalesDataRow.CUSTOMERSEGMENT = Nothing
                    VisitSalesDataRow.CUSTOMERID = Nothing
                    '顧客担当スタッフ
                    VisitSalesDataRow.STAFFCODE = Nothing
                    VisitSalesDataRow.VISITPERSONNUMBER = CInt(visitPersonNumber)
                    VisitSalesDataRow.VISITMEANS = visitMeans
                    VisitSalesDataRow.VISITSTATUS = VisitStatusFree
                    VisitSalesDataRow.BROUDCAST = BroudcastFlagUnsend
                    '対応担当スタッフ
                    VisitSalesDataRow.PHYSICSSTAFFCODE = Nothing
                    '更新アカウント
                    VisitSalesDataRow.CREATEACCOUNT = account
                    VisitSalesDataRow.FUNCTIONID = ApplicationId

                    '来店実績テーブルの作成
                    Dim VisitUtilityBiz As New VisitReceptionBusinessLogic
                    VisitUtilityBiz.CreateCustomerChip(VisitSalesDataRow, False)
                    ' $10 END   次世代e-CRBセールス機能 新DB適応に向けた機能開発

                ElseIf String.Equals(visitPurpose, VisitPurposeService) Then

                    Logger.Debug("SendNotCustomerInfo_002 VisitPurpose is Service")

                    'サービス来店実績テーブルの作成
                    Dim messageId As Long = MessageIdSuccess

                    '$04 Start
                    '読取機でから受け取る車両登録Noに「-」が入っていないので、「-」を指定の箇所に追加して車両登録ナンバーを登録するように改修
                    ' $13 START サービスタブレットゲートカメラ連携機能追加開発
                    'Dim VclRegNoFormatted = GetFormattedVclRegNo(unsentRow.VCLREGNO)
                    Dim VclRegNoFormatted = GetFormattedVclRegNo(unsentRow.VCLREGNO)(0)
                    ' $13 END サービスタブレットゲートカメラ連携機能追加開発
                    '2014/01/17 TMEJ 陳   TMEJ次世代サービス 工程管理機能開発 $11 START
                    Dim visitSeq As Long
                    '2014/01/17 TMEJ 陳   TMEJ次世代サービス 工程管理機能開発 $11 END

                    'サービス来店実績テーブルの作成
                    Using ic3810101Biz As IC3810101BusinessLogic = New IC3810101BusinessLogic

                        '2014/01/17 TMEJ 陳   TMEJ次世代サービス 工程管理機能開発 $11 START
                        'messageId = ic3810101Biz.InsertServiceVisit(dealerCode, _
                        '                               storeCode, _
                        '                               unsentRow.VISITTIMESTAMP, _
                        '                               VclRegNoFormatted, _
                        '                               Nothing, _
                        '                               Nothing, _
                        '                               Nothing, _
                        '                               CShort(visitPersonNumber), _
                        '                               visitMeans, _
                        '                               Nothing, _
                        '                               0, _
                        '                               Nothing, _
                        '                               Nothing, _
                        '                               Nothing, _
                        '                               account, _
                        '                               ApplicationId)
                        '2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 START
                        'messageId = ic3810101Biz.InsertServiceVisit(dealerCode, _
                        '                               storeCode, _
                        '                               unsentRow.VISITTIMESTAMP, _
                        '                               VclRegNoFormatted, _
                        '                               Nothing, _
                        '                               Nothing, _
                        '                               Nothing, _
                        '                               CShort(visitPersonNumber), _
                        '                               visitMeans, _
                        '                               Nothing, _
                        '                               0, _
                        '                               Nothing, _
                        '                               Nothing, _
                        '                               Nothing, _
                        '                               account, _
                        '                               userName, _
                        '                               ApplicationId)
                        messageId = ic3810101Biz.InsertServiceVisit(dealerCode, _
                               storeCode, _
                               unsentRow.VISITTIMESTAMP, _
                               VclRegNoFormatted, _
                               Nothing, _
                               Nothing, _
                               Nothing, _
                               CShort(visitPersonNumber), _
                               visitMeans, _
                               Nothing, _
                               0, _
                               Nothing, _
                               Nothing, _
                               Nothing, _
                               account, _
                               userName, _
                               ApplicationId, _
                               SvcinIdNotSpecified)
                        '2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 END
                        visitSeq = ic3810101Biz.VisitSeqInserted
                        '2014/01/17 TMEJ 陳   TMEJ次世代サービス 工程管理機能開発 $11 END

                    End Using
                    '$04 End

                    ' DBタイムアウトエラー時
                    If Not messageId = MessageIdSuccess Then
                        Me.Rollback = True

                        Logger.Info("SendNotCustomerInfo_End Ret[" & MessageIdErrorDbTimeOut & "]")
                        Return MessageIdErrorDbTimeOut
                    End If

                    '2014/01/17 TMEJ 陳   TMEJ次世代サービス 工程管理機能開発 $11 START

                    '現在時間
                    Dim dateNow As DateTime = DateTime.Now

                    '通知処理
                    Using ic3810101Biz2 As IC3810101BusinessLogic = New IC3810101BusinessLogic
                        ic3810101Biz2.NoticeProcessing(visitSeq, _
                                             dateNow, _
                                             dealerCode, _
                                             storeCode, _
                                             account, _
                                             userName)
                    End Using

                    '2014/01/17 TMEJ 陳   TMEJ次世代サービス 工程管理機能開発 $11 END
                    Logger.Info("SendNotCustomerInfo_End Ret[" & MessageIdSuccess & "]")
                    Return MessageIdSuccess
                End If

            End Using

        Catch oraEx As OracleExceptionEx

            If oraEx.Number = ErrorCodeOra2049 Then

                'ロールバックを行う。
                Me.Rollback = True

                'ログ出力
                Logger.Error(CStr(MessageIdErrorDbTimeOut), oraEx)

                'DBタイムアウトエラー時
                Logger.Info("SendNotCustomerInfo_End Ret[" & MessageIdErrorDbTimeOut & "]")
                Return MessageIdErrorDbTimeOut
            Else
                '上記以外のエラーは基盤側で制御
                Throw
            End If
        End Try

        Logger.Info("SendNotCustomerInfo_End Ret[" & MessageIdSuccess & "]")
        Return MessageIdSuccess

    End Function

#End Region

#Region "送信チェック"

    ''' <summary>
    ''' 送信対象車両送信チェック
    ''' </summary>
    ''' <param name="visitVehicleSequence">来店車両実績連番</param>
    ''' <returns>メッセージID</returns>
    ''' <remarks></remarks>
    Public Function IsUnsent(ByVal visitVehicleSequence As String) As Integer

        Logger.Info("IsUnsent_Start Pram[" & visitVehicleSequence & "]")

        Dim visitVclDealRow As SC3090301VisitVehicleDealFlgRow

        '検索処理
        Using adapter As New SC3090301TableAdapter

            Using visitVclDealTbl As SC3090301VisitVehicleDealFlgDataTable = _
                adapter.GetDealType(visitVehicleSequence)

                visitVclDealRow = visitVclDealTbl.Item(0)

            End Using
        End Using

        '対応フラグをチェック
        If String.Equals(visitVclDealRow.DEALFLG, DealFlagUnsend) Then

            If String.Equals(visitVclDealRow.DELFLG, DeleteFlagDelete) Then
                '削除
                Logger.Info("IsUnsent_End Ret[Deleted]")
                Return MessageIdErrorNoDeleteTarget

            Else
                '未送信
                Logger.Info("IsUnsent_End Ret[True]")
                Return MessageIdSuccess
            End If
        Else

            '未送済み
            Logger.Info("IsUnsent_End Ret[Sended]")
            Return MessageIdErrorSend

        End If

    End Function

#End Region

#Region "送信処理"
    ' $07 start ウェルカムボードへの通知に必要な引数追加
    ''' <summary>
    ''' Push実行処理
    ''' </summary>
    ''' <param name="visitPurpose">来店区分</param>
    ''' <returns>メッセージID</returns>
    ''' <remarks></remarks>
    Public Function PushExecution(ByVal visitPurpose As String) As Integer
        ' $12 STARTTMEJ次世代サービス 工程管理機能開発 START
        'Public Function PushExecution(ByVal dealerCode As String, ByVal storeCode As String, ByVal visitPurpose As String) As Integer
        ' $12 STARTTMEJ次世代サービス 工程管理機能開発 END
        ' $07 end ウェルカムボードへの通知に必要な引数追加

        Logger.Info("PushExecution_Start")
        Dim messageId As Integer = MessageIdSuccess

        '来店目的にて処理分岐
        If String.Equals(visitPurpose, VisitPurposeSales) Then

            Logger.Debug("PushExecution_001 VisitPurpose is Sales")

            ' $10 START 次世代e-CRBセールス機能 新DB適応に向けた機能開発
            Dim visit As New VisitReceptionBusinessLogic
            visit.SendPushSales(VisitSalesDataRow, isClaimeInfo)
            ' $10 END   次世代e-CRBセールス機能 新DB適応に向けた機能開発

            ' $11 STARTTMEJ次世代サービス 工程管理機能開発 START
            'ElseIf String.Equals(visitPurpose, VisitPurposeService) Then

            '    Logger.Debug("PushExecution_002 VisitPurpose is Service")

            '    '来店目的がサービス
            '    '来店通知送信処理(案内係)
            '    SendVisitInfoCommonService(dealerCode, storeCode)
            ' $11 ENDTMEJ次世代サービス 工程管理機能開発 END
        End If

        Logger.Info("PushExecution_End Ret[" & messageId & "]")
        Return messageId

    End Function

    '2014/01/17 TMEJ 陳   TMEJ次世代サービス 工程管理機能開発 $11 START
    ' ''' <summary>
    ' ''' サービス共通の送信処理を実行
    ' ''' </summary>
    ' ''' <param name="dealerCode">販売店コード</param>
    ' ''' <param name="storeCode">店舗コード</param>
    ' ''' <remarks></remarks>
    'Private Sub SendVisitInfoCommonService(ByVal dealerCode As String, ByVal storeCode As String)

    '    Logger.Debug("SendVisitInfoCommonService_Start Pram[" & dealerCode & "," & storeCode & "]")

    '    'スタッフ情報の取得(案内受付係)
    '    Dim user As New Users
    '    Dim userTable As UsersDataSet.USERSDataTable
    '    Dim stuffCodeList As New List(Of Decimal)
    '    stuffCodeList.Add(OperationCodeSvr)
    '    Logger.Info("SendVisitInfoCommonService_001 " & "Call_Start Users.GetAllUser Pram[" & dealerCode & "," & storeCode & ", 0" & "]")
    '    userTable = user.GetAllUser(dealerCode, storeCode, stuffCodeList, "0")
    '    Logger.Info("SendVisitInfoCommonService_001 " & "Call_End   Users.GetAllUser")

    '    user = Nothing
    '    stuffCodeList = Nothing

    '    '来店通知命令の送信
    '    ' $03 案内係へのPush 
    '    For Each userRow As UsersDataSet.USERSRow In userTable.Rows

    '        '送信処理
    '        SendVisitInfo(SendTypeSvr, userRow.ACCOUNT)
    '    Next
    '    ' $03 案内係へのPush 

    '    Logger.Debug("SendVisitInfoCommonService_End")
    'End Sub

    ' ''' <summary>
    ' ''' 来店通知送信命令の送信
    ' ''' </summary>
    ' ''' <param name="sendKind">通知種別</param>
    ' ''' <param name="stuffCode">スタッフコード</param>
    ' ''' <param name="message">メッセージ</param>
    ' ''' <remarks></remarks>
    'Private Sub SendVisitInfo(ByVal sendKind As Integer, ByVal stuffCode As String, _
    '                                 Optional ByVal message As String = "")

    '    Logger.Debug("SendVisitInfo_Start Pram[" & sendKind & "," & stuffCode & "," & message & "]")

    '    Dim postMsg As New StringBuilder

    '    'POST送信する文字列を作成する。
    '    Select Case sendKind
    '        Case SendTypeCsutSs

    '            Logger.Debug("SendVisitInfo_001 PostType SS")

    '            '顧客担当SSの場合
    '            With postMsg
    '                .Append("cat=popup")
    '                .Append("&type=header")
    '                .Append("&sub=text")
    '                .Append("&uid=" & stuffCode)
    '                .Append("&time=3")
    '                .Append("&color=F9EDBE64")
    '                .Append("&height=50")
    '                .Append("&width=1024")
    '                .Append("&pox=0")
    '                .Append("&msg=" & message)
    '                .Append("&js1=icropScript.ui.setVisitor()")
    '                .Append("&js2=icropScript.ui.openVisitorListDialog()")
    '            End With

    '        Case SendTypeSlr, SendTypeBm, SendTypeSsm

    '            Logger.Debug("SendVisitInfo_002 PostType SLR or BM or SSM")

    '            '受付係、ブランチマネージャー、セールスマネージャー
    '            With postMsg
    '                .Append("cat=action")
    '                .Append("&type=main")
    '                .Append("&sub=js")
    '                .Append("&uid=" & stuffCode)
    '                .Append("&time=0")
    '                .Append("&js1=SC3100101Update('01','01')")
    '            End With

    '        Case SendTypeSvr

    '            Logger.Debug("SendVisitInfo_003 PostType SVR")

    '            '案内係
    '            With postMsg
    '                .Append("cat=action")
    '                .Append("&type=main")
    '                .Append("&sub=js")
    '                .Append("&uid=" & stuffCode)
    '                .Append("&time=0")
    '                .Append("&js1=Send_Visit()")
    '            End With

    '            '$05 start 顧客担当SC（サービス入庫）
    '        Case SendTypeSC

    '            With postMsg
    '                .Append("cat=popup")
    '                .Append("&type=header")
    '                .Append("&sub=text")
    '                .Append("&uid=" & stuffCode)
    '                .Append("&time=3")
    '                .Append("&color=CCFFFF64")
    '                .Append("&height=50")
    '                .Append("&width=1024")
    '                .Append("&pox=0")
    '                .Append("&msg=" & message)
    '                .Append("&js1=icropScript.ui.setNotice ()")
    '                .Append("&js2=icropScript.ui.openNoticeDialog ()")
    '            End With
    '            '$05 start 顧客担当SC（サービス入庫）
    '    End Select

    '    Dim visitUtility As New VisitUtility
    '    visitUtility.SendPush(postMsg.ToString())

    '    Logger.Debug("SendVisitInfo_End")
    'End Sub
    '2014/01/17 TMEJ 陳   TMEJ次世代サービス 工程管理機能開発 $11 END

    ''' <summary>
    ''' セールススタッフ[スタンバイ]への通知処理
    ''' </summary>
    ''' <param name="stuffList">セールススタッフリスト</param>
    ''' <param name="customerStuffCode">顧客担当スタッフコード</param>
    ''' <param name="pushMessage">Postメッセージ</param>
    ''' <param name="customerID">顧客ID</param>
    ''' <param name="customerName">顧客名</param>
    ''' <param name="customerClass">顧客分類</param>
    ''' <param name="visitSequence">来店実績連番</param>
    ''' <returns>メッセージID</returns>
    ''' <remarks></remarks>
    Private Function SendStandbyStuff(ByVal stuffList As VisitUtilityUsersDataTable, ByVal customerStuffCode As String, _
                                 ByVal pushMessage As String, ByVal customerID As String, _
                                 ByVal customerName As String, ByVal customerClass As String, _
                                 ByVal visitSequence As Long) As Integer
        '2014/01/17 TMEJ 陳   TMEJ次世代サービス 工程管理機能開発 $11 START
        'Private Function SendStandbyStuff(ByVal stuffList As VisitUtilityUsersDataTable, ByVal customerStuffCode As String, _
        '                             ByVal pushMessage As String, ByVal customerID As String, _
        '                             ByVal customerName As String, ByVal customerClass As String, _
        '                             ByVal visitSequence As Long, _
        '                             ByVal visitReason As Boolean) As Integer
        '2014/01/17 TMEJ 陳   TMEJ次世代サービス 工程管理機能開発 $11 END

        Logger.Debug("SendStandbyStuff_Start Pram[(stuffList IsNot Nothing)" & "," & customerStuffCode & "," & _
                                                      pushMessage & "," & customerID & "," & customerName & "," & "]")
        '通知IFへ渡すクラスの生成
        Dim noticeData As Toyota.eCRB.Tool.Notify.Api.DataAccess.XmlNoticeData
        noticeData = CreateInputClassService(pushMessage, stuffList, customerID, customerName, customerClass, visitSequence)

        If noticeData Is Nothing Then
            Return MessageIdSuccess
        End If

        '通知IFの呼び出し
        Dim returnXml As Toyota.eCRB.Tool.Notify.Api.DataAccess.XmlCommon = Nothing
        Using ic3040801Biz As New IC3040801BusinessLogic

            Logger.Info("SendStandbyStuff Call_Start IC3040801BusinessLogic.NoticeDisplay Pram[" & (noticeData IsNot Nothing) & "]")
            returnXml = ic3040801Biz.NoticeDisplay(noticeData, NoticeDisposal.Peculiar)
            Logger.Info("SendStandbyStuff Call_End IC3040801BusinessLogic.NoticeDisplay Ret[" & returnXml.Message & "," & returnXml.ResultId & "]")

        End Using

        ' 戻り値判断
        ' IFの戻り値は成功='0'又はDBアクセスエラー='6000'のため成功かどうかのみで判断
        ' DBアクセスエラー以外のエラーはExceptionで帰ってくるためそのまま基盤へthrow
        If String.Equals(returnXml.ResultId, "006000") Then

            Logger.Debug("SendStandbyStuff_End Ret[" & MessageIdErrorDbTimeOut & "]")
            Return MessageIdErrorDbTimeOut
        End If

        Logger.Debug("SendStandbyStuff_End Ret[" & MessageIdSuccess & "]")
        Return MessageIdSuccess
    End Function

    '$05 start サービス入庫
    ''' <summary>
    ''' 通知IFへ渡すXmlNoticeDataクラスの作成処理(サービス入庫)
    ''' </summary>
    ''' <param name="pushMessage">Postメッセージ</param>
    ''' <param name="stuffList">セールススタッフリスト</param>
    ''' <param name="customerID">顧客ID</param>
    ''' <param name="customerName">顧客名</param>
    ''' <param name="customerClass">顧客分類</param>
    ''' <param name="visitSequence">来店実績連番</param>
    ''' <returns>XmlNoticeDataクラス</returns>
    ''' <remarks></remarks>
    Private Function CreateInputClassService(ByVal pushMessage As String, ByVal stuffList As VisitUtilityUsersDataTable, _
                               ByVal customerID As String, _
                               ByVal customerName As String, ByVal customerClass As String, _
                               ByVal visitSequence As Long) As Toyota.eCRB.Tool.Notify.Api.DataAccess.XmlNoticeData

        Logger.Info("CreateInputClassService_Start Pram[" & pushMessage & "," & _
                    (stuffList IsNot Nothing) & "," & customerID & "," & customerName & "]")

        Dim standbyStuffCount As Integer = 0

        Dim returnValue As Toyota.eCRB.Tool.Notify.Api.DataAccess.XmlNoticeData = _
            New Toyota.eCRB.Tool.Notify.Api.DataAccess.XmlNoticeData

        'ヘッダー情報
        returnValue.TransmissionDate = DateTimeFunc.Now(StaffContext.Current.DlrCD)
        Logger.Info("CreateInputClassService SetValue TransmissionDate[" & returnValue.TransmissionDate & "]")

        ' $10 START 次世代e-CRBセールス機能 新DB適応に向けた機能開発
        '来店通知命令の送信(顧客担当スタッフ)
        For Each salesStuffInfoRow As VisitUtilityUsersRow In stuffList.Rows

            Dim xmlAccount As XmlAccount = New XmlAccount
            xmlAccount.ToAccount = salesStuffInfoRow.ACCOUNT
            xmlAccount.ToAccountName = salesStuffInfoRow.USERNAME

            Logger.Info("CreateInputClassService SetValue XmlAccount ToAccount[" & salesStuffInfoRow.ACCOUNT & "]")
            Logger.Info("CreateInputClassService SetValue XmlAccount ToAccountName[" & salesStuffInfoRow.USERNAME & "]")

            returnValue.AccountList.Add(xmlAccount)
            standbyStuffCount = standbyStuffCount + 1
            If standbyStuffCount >= 1 Then Exit For
        Next
        ' $10 END   次世代e-CRBセールス機能 新DB適応に向けた機能開発

        ' $06 start スタッフが無いとき処理終了
        ' standbyStuffが０人なら以降処理は行わない。
        If standbyStuffCount = 0 Then
            Return Nothing
        End If
        ' $06 end スタッフが無いとき処理終了

        'Request情報
        Dim requestNotice As XmlRequestNotice = New XmlRequestNotice
        requestNotice.DealerCode = StaffContext.Current.DlrCD
        requestNotice.StoreCode = StaffContext.Current.BrnCD
        requestNotice.RequestClass = "07"
        requestNotice.Status = "1"
        requestNotice.RequestClassId = visitSequence
        requestNotice.FromAccount = StaffContext.Current.Account
        requestNotice.FromAccountName = StaffContext.Current.UserName
        requestNotice.CustomId = customerID
        requestNotice.CustomName = customerName
        requestNotice.CustomerClass = CustomerKindOwner
        requestNotice.CustomerKind = customerClass
        returnValue.RequestNotice = requestNotice

        Logger.Info("CreateInputClassService SetValue XmlRequestNotice DealerCode[" & requestNotice.DealerCode & "]")
        Logger.Info("CreateInputClassService SetValue XmlRequestNotice StoreCode[" & requestNotice.StoreCode & "]")
        Logger.Info("CreateInputClassService SetValue XmlRequestNotice RequestClass[" & requestNotice.RequestClass & "]")
        Logger.Info("CreateInputClassService SetValue XmlRequestNotice Status[" & requestNotice.Status & "]")
        Logger.Info("CreateInputClassService SetValue XmlRequestNotice RequestClassId[" & requestNotice.RequestClassId & "]")
        Logger.Info("CreateInputClassService SetValue XmlRequestNotice FromAccount[" & requestNotice.FromAccount & "]")
        Logger.Info("CreateInputClassService SetValue XmlRequestNotice FromAccountName[" & requestNotice.FromAccountName & "]")
        Logger.Info("CreateInputClassService SetValue XmlRequestNotice CustomID[" & requestNotice.CustomId & "]")
        Logger.Info("CreateInputClassService SetValue XmlRequestNotice CustomName[" & requestNotice.CustomName & "]")
        Logger.Info("CreateInputClassService SetValue XmlRequestNotice CustomerClass[" & requestNotice.CustomerClass & "]")
        Logger.Info("CreateInputClassService SetValue XmlRequestNotice CustomerKind[" & requestNotice.CustomerKind & "]")

        'Push情報
        Dim pushInfo As XmlPushInfo = New XmlPushInfo
        pushInfo.PushCategory = "1"
        pushInfo.PositionType = "1"
        pushInfo.Time = 3
        pushInfo.DisplayType = "1"
        pushInfo.DisplayContents = pushMessage
        pushInfo.Color = "2"
        pushInfo.PopWidth = 1024
        pushInfo.PopHeight = 50
        pushInfo.PopX = 0
        pushInfo.DisplayFunction = "icropScript.ui.setNotice()"
        pushInfo.ActionFunction = "icropScript.ui.openNoticeDialog()"
        returnValue.PushInfo = pushInfo

        Logger.Info("CreateInputClassService SetValue XmlPushInfo PushCategory[" & pushInfo.PushCategory & "]")
        Logger.Info("CreateInputClassService SetValue XmlPushInfo PositionType[" & pushInfo.PositionType & "]")
        Logger.Info("CreateInputClassService SetValue XmlPushInfo Time[" & pushInfo.Time & "]")
        Logger.Info("CreateInputClassService SetValue XmlPushInfo DisplayType[" & pushInfo.DisplayType & "]")
        Logger.Info("CreateInputClassService SetValue XmlPushInfo DisplayContents[" & pushInfo.DisplayContents & "]")
        Logger.Info("CreateInputClassService SetValue XmlPushInfo Color[" & pushInfo.Color & "]")
        Logger.Info("CreateInputClassService SetValue XmlPushInfo PopWidth[" & pushInfo.PopWidth & "]")
        Logger.Info("CreateInputClassService SetValue XmlPushInfo PopHeight[" & pushInfo.PopHeight & "]")
        Logger.Info("CreateInputClassService SetValue XmlPushInfo PopX[" & pushInfo.PopX & "]")
        Logger.Info("CreateInputClassService SetValue XmlPushInfo DisplayFunction[" & pushInfo.DisplayFunction & "]")
        Logger.Info("CreateInputClassService SetValue XmlPushInfo ActionFunction[" & pushInfo.ActionFunction & "]")

        Logger.Info("CreateInputClassService_End")
        Return returnValue

    End Function
    '$05 end サービス入庫

#End Region

#End Region

#Region "変換後車輌登録Noの取得"

    '$04 Start
    ''' <summary>
    ''' 変換後車輌登録Noの取得
    ''' </summary>
    ''' <param name="vclRegNo">変換前車輌登録No</param>
    ''' <returns>変換後車輌登録Noリスト</returns>
    ''' <remarks>変換フォーマットは本関数内にDB設定から取得。変換フォーマットはDBに未設定の場合、変換前車輌登録Noをそのまま返す</remarks>
    Public Function GetFormattedVclRegNo(ByVal vclRegNo As String) As List(Of String)
        Logger.Info("GetFormattedVclRegNo_S. VclRegNo=" & vclRegNo)

        ' $13 START サービスタブレットゲートカメラ連携機能追加開発
        'Static changeFormat As String = Nothing
        'Static changeString As String = Nothing
        ' $13 END サービスタブレットゲートカメラ連携機能追加開発

        If sysChangeFormat Is Nothing Then
            Dim context As StaffContext = StaffContext.Current
            Dim dlrEnvSet As New BranchEnvSetting

            Dim sysEnvChangeFormatRow As DlrEnvSettingDataSet.DLRENVSETTINGRow = Nothing
            sysEnvChangeFormatRow = dlrEnvSet.GetEnvSetting(context.DlrCD, context.BrnCD, VclRegNoChangeFormat)

            Dim sysEnvChangeStringRow As DlrEnvSettingDataSet.DLRENVSETTINGRow = Nothing
            sysEnvChangeStringRow = dlrEnvSet.GetEnvSetting(context.DlrCD, context.BrnCD, VclRegNoChangeString)

            'どちらか一方でも設定されていなければ、フォーマットによる変換は行わない。
            If (sysEnvChangeFormatRow IsNot Nothing) AndAlso (sysEnvChangeStringRow IsNot Nothing) Then
                sysChangeFormat = sysEnvChangeFormatRow.PARAMVALUE
                sysChangeString = sysEnvChangeStringRow.PARAMVALUE
                Logger.Info("GetFormattedVclRegNo " & "changeFormat=" & sysChangeFormat & ", changeString=" & sysChangeString)
            Else
                ' $13 START サービスタブレットゲートカメラ連携機能追加開発
                'changeFormat = String.Empty
                'changeString = String.Empty
                sysChangeFormat = Nothing
                sysChangeString = Nothing
                ' $13 END サービスタブレットゲートカメラ連携機能追加開発
            End If

            If (sysEnvChangeFormatRow Is Nothing) Then
                Logger.Info("GetFormattedVclRegNo " & "GetEnvSetting[" & VclRegNoChangeFormat & "] NG")
            Else
                sysEnvChangeFormatRow = Nothing
            End If
            If (sysEnvChangeStringRow Is Nothing) Then
                Logger.Info("GetFormattedVclRegNo " & "GetEnvSetting[" & VclRegNoChangeString & "] NG")
            Else
                sysEnvChangeStringRow = Nothing
            End If
        End If

        ' $13 START サービスタブレットゲートカメラ連携機能追加開発
        Dim VclRegNoFormattedList As List(Of String) = New List(Of String)

        If String.IsNullOrEmpty(sysChangeFormat) Then
            'フォーマットによる変換は行わない。
            'VclRegNoFormatted = VclRegNo
            VclRegNoFormattedList.Add(vclRegNo)

        Else
            'フォーマット変換をした文字列を生成する
            'VclRegNoFormatted = GetChangeVclRegNo(VclRegNo, changeFormat, changeString)

            'フォーマットのリストを取得
            Dim changeFormatList As String()
            changeFormatList = sysChangeFormat.Split(CType(",", Char))

            For Each cFormat In changeFormatList
                Dim VclRegNoFormatted As String = String.Empty

                'フォーマット変換をした文字列を生成する
                VclRegNoFormatted = GetChangeVclRegNo(vclRegNo, cFormat, sysChangeString)

                VclRegNoFormattedList.Add(VclRegNoFormatted)
            Next

            '変換前の文字列を末尾に追加
            VclRegNoFormattedList.Add(vclRegNo)
        End If

        'Logger.Info("GetFormattedVclRegNo_E. VclRegNoFormatted=" & VclRegNoFormatted)
        Logger.Info("GetFormattedVclRegNo_E. VclRegNoFormatted=" & String.Join(",", VclRegNoFormattedList))

        'Return VclRegNoFormatted
        Return VclRegNoFormattedList
        ' $13 END サービスタブレットゲートカメラ連携機能追加開発
    End Function
    '$04 End

    ''' <summary>
    ''' 変換後車輌登録Noの取得
    ''' </summary>
    ''' <param name="targetVclRegNo">対象の車輌登録No</param>
    ''' <param name="changeFormat">変換フォーマット</param>
    ''' <param name="changeString">変換当て込み文字</param>
    ''' <returns>当て込んだ文字列</returns>
    ''' <remarks></remarks>
    Private Function GetChangeVclRegNo(ByVal targetVclRegNo As String, _
                                       ByVal changeFormat As String, _
                                       ByVal changeString As String) As String
        Logger.Debug("GetChangeVclRegNo_Start " & _
                    "Param[" & targetVclRegNo & "," & changeFormat & "," & changeString & "]")

        Dim returnString As New StringBuilder
        Dim formatIndex As Integer = 0
        Dim targetIndex As Integer = 0

        '変換フォーマットか対象の車輌Noの文字数を越えるまでループ
        While formatIndex < changeFormat.Length And _
            targetIndex < targetVclRegNo.Length

            '変換フォーマットと当て込み文字が一致していたら
            If String.Equals(changeFormat(formatIndex), changeString) Then
                Logger.Debug("GetChangeVclRegNo_001 changeFormat(" & formatIndex & ") = changeString ")

                '車輌登録Noの文字を当て込み
                returnString.Append(targetVclRegNo(targetIndex))
                targetIndex += 1
                formatIndex += 1
            Else
                Logger.Debug("GetChangeVclRegNo_002 changeFormat(" & formatIndex & ") <> changeString ")

                '変換フォーマットの文字を当て込み
                returnString.Append(changeFormat(formatIndex))
                formatIndex += 1
            End If
        End While
        Logger.Debug("GetChangeVclRegNo_End Ret[" & returnString.ToString & "]")
        Return returnString.ToString
    End Function
#End Region

    ' $11 start 
#Region "来店車両情報削除"

    ''' <summary>
    ''' 来店車両情報削除
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="storeCode">店舗コード</param>
    ''' <param name="nowDate">来店日時</param>
    ''' <returns>メッセージID</returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Function DeleteVisitVehicl(ByVal dealerCode As String, _
                                            ByVal storeCode As String, _
                                            ByVal nowDate As Date) As Integer

        Logger.Info("DeleteVisitVehicl_Start Pram[" & dealerCode & "," & storeCode & "," & nowDate & "]")

        '日付検索用
        Dim dateSt As New Date(nowDate.Year, nowDate.Month, nowDate.Day, 0, 0, 0)
        Dim dateEd As New Date(nowDate.Year, nowDate.Month, nowDate.Day, 23, 59, 59)
        Dim messageID As Integer = MessageIdSuccess
        Dim deleteCount As Integer = 0

        ' 削除実行
        Try
            Using adapter As New SC3090301TableAdapter

                deleteCount = adapter.DeleteVisitVehicl(dealerCode, storeCode, dateSt, dateEd)

            End Using
        Catch oraEx As OracleExceptionEx
            If oraEx.Number = ErrorCodeOra2049 Then

                'ロールバックを行う。
                Me.Rollback = True

                'ログ出力
                Logger.Error(CStr(MessageIdErrorDbTimeOutInDelete), oraEx)

                'DBタイムアウトエラー時
                Logger.Info("SendNewSales_End Ret[messageId=" & MessageIdErrorDbTimeOutInDelete & "]")
                Return MessageIdErrorDbTimeOutInDelete
            Else
                '上記以外のエラーは基盤側で制御
                Throw
            End If
        End Try


        '0件ならすでに削除されている
        If deleteCount <= 0 Then
            messageID = MessageIdErrorNoDeleteTarget
            Logger.Info("DeleteVisitVehicl_End Selected Vehicle has been deleted")
            Return messageID
        Else
            Logger.Info("DeleteVisitVehicl_End " & deleteCount & " Row deleted")
            Return messageID
        End If

    End Function
#End Region
    ' $11 end

End Class
