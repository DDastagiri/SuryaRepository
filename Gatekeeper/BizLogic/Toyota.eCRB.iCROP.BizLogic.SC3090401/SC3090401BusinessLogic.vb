'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3090401BusinessLogic.vb
'──────────────────────────────────
'機能： 予約一覧
'補足： 
'作成： 2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001 iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加
'更新： 2019/05/10 NSK 鈴木 18PRJ00XXX_(FS)納車時オペレーションCS向上にむけた評価（サービス）
'更新：                      [TKM]PUAT-3088 セールスタブレットの通知履歴をタップするとシステムエラーが発生する を修正
'更新： 
'──────────────────────────────────

Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Configuration
Imports System.Globalization
Imports System.Text
Imports Toyota.eCRB.GateKeeper.AppointmentList.DataAccess.SC3090401DataSet
Imports Toyota.eCRB.GateKeeper.AppointmentList.DataAccess.SC3090401DataSetTableAdapters
Imports Toyota.eCRB.GateKeeper.AppointmentList.DataAccess
Imports Toyota.eCRB.iCROP.BizLogic.IC3810101
Imports Toyota.eCRB.Visit.Api.DataAccess.VisitUtilityDataSet
Imports Toyota.eCRB.Visit.Api.DataAccess.VisitUtilityDataSetTableAdapters
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.Tool.Notify.Api.BizLogic
Imports Toyota.eCRB.Tool.Notify.Api.DataAccess
Imports Toyota.eCRB.Tool.Notify.Api.DataAccess.ConstCode
Imports Toyota.eCRB.Visit.Api.BizLogic

Public Class SC3090401BusinessLogic
    Inherits BaseBusinessComponent

#Region "定数"
    ''' <summary>
    ''' 機能ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ApplicationId As String = "SC3090401"

    ''' <summary>
    ''' 来店人数(1人)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VisitPersonNumberOne As Integer = 1

    ''' <summary>
    ''' 来店目的(1:車)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VisitPurposeCar As String = "1"

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

    ''' <summary>
    ''' システム管理マスタ.パラメータ名:敬称表示位置
    ''' </summary>
    ''' <remarks></remarks>
    Private Const KeisyoZengo As String = "KEISYO_ZENGO"

    ''' <summary>
    ''' システム管理マスタ.パラメータ名:苦情情報日数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ComplaintDisplayDate As String = "COMPLAINT_DISPLAYDATE"

    ''' <summary>
    ''' 敬称表示位置:前
    ''' </summary>
    ''' <remarks></remarks>
    Private Const HonorificTitleMae As String = "1"

    ''' <summary>
    ''' 顧客種別：オーナー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CustomerKindOwner As String = "1"

#End Region

#Region "メッセージID"
    ''' <summary>
    ''' メッセージID:成功
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MessageIdSuccess As Integer = 0

    ''' <summary>
    ''' 文言ID：苦情文字
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ClameWord As Integer = 12

    ''' <summary>
    ''' 文言ID：サービス入庫
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ServiceinWord As Integer = 13

    ''' <summary>
    ''' メッセージID:エラー[DBタイムアウト]
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MessageIdErrorDbTimeOut As Integer = 901

    ''' <summary>
    ''' メッセージID:エラー[予約変更]
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MessageIdErrorReserveUpdate As Integer = 902
#End Region

#Region "サービス来店登録"
    ''' <summary>
    ''' サービス来店登録
    ''' </summary>
    ''' <param name="inDealerCode">ログインユーザーの販売店コード</param>
    ''' <param name="inBranchCode">ログインユーザーの店舗コード</param>
    ''' <param name="inServiceinId">選択中の予約情報のサービス入庫ID</param>
    ''' <param name="inNowDate">現在日時</param>
    ''' <returns>メッセージID</returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Function RegistServiceVisit(ByVal inDealerCode As String, _
                                       ByVal inBranchCode As String, _
                                       ByVal inServiceinId As Decimal, _
                                       ByVal inNowDate As Date) As Integer

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                          , "{0}.{1} START IN:inDealerCode = {2}, inBranchCode = {3}, inServiceinId = {4}" & _
                          ", inNowDate = {5}" _
                          , Me.GetType.ToString _
                          , System.Reflection.MethodBase.GetCurrentMethod.Name _
                          , inDealerCode, inBranchCode, inServiceinId, inNowDate))

        Dim staffInfo As StaffContext = StaffContext.Current

        Using ta As New SC3090401TableAdapter

            Dim reservationCount As Integer = ta.IsNotCarInStatus(inServiceinId)

            ' サービス入庫IDに紐付く未入庫の予約が存在しない場合
            If reservationCount <= 0 Then

                '排他エラー
                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                             , "{0}.{1} ERR:RECORD LOCK TIMEOUT" _
                             , Me.GetType.ToString _
                             , System.Reflection.MethodBase.GetCurrentMethod.Name))

                ' エラーメッセージを返却
                Return MessageIdErrorReserveUpdate
            End If

            Dim visitMngCount As Integer = ta.GetVisitManagementCount(inDealerCode, _
                                                                      inBranchCode, _
                                                                      inServiceinId)

            ' 予約に紐付くサービス来店者管理が存在する場合
            If 0 < visitMngCount Then

                '排他エラー
                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                             , "{0}.{1} ERR:RECORD LOCK TIMEOUT" _
                             , Me.GetType.ToString _
                             , System.Reflection.MethodBase.GetCurrentMethod.Name))

                ' エラーメッセージを返却
                Return MessageIdErrorReserveUpdate
            End If

            Using ic3810101Biz As IC3810101BusinessLogic = New IC3810101BusinessLogic

                ' 顧客車両情報取得
                Dim dtCstVclInfo As SC3090401DataSet.SC3090401CustomerVehicleDataDataTable = _
                    ta.GetCustomerVehicleInfo(inServiceinId)

                ' 顧客車両情報が取得できた場合
                If 0 < dtCstVclInfo.Count Then

                    Dim drCstVclInfo As SC3090401DataSet.SC3090401CustomerVehicleDataRow = dtCstVclInfo(0)

                    Try

                        ' 来店車両実績更新
                        ta.UpdateVisitVehicle(inDealerCode, _
                                              inBranchCode, _
                                              staffInfo.Account, _
                                              inNowDate, _
                                              drCstVclInfo.REG_NUM_SEARCH)

                    Catch ex As OracleExceptionEx When ex.Number = 30006
                        '行ロック失敗(WAIT時間超え)

                        '排他エラー
                        Logger.Error(String.Format(CultureInfo.CurrentCulture _
                                     , "{0}.{1} ERR:RECORD LOCK TIMEOUT" _
                                     , Me.GetType.ToString _
                                     , System.Reflection.MethodBase.GetCurrentMethod.Name))

                        Return MessageIdErrorReserveUpdate

                    Catch ex As OracleExceptionEx When ex.Number = 1013

                        ''ORACLEのタイムアウトのみ処理
                        Me.Rollback = True

                        ''終了ログの出力
                        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} OUT:RETURNCODE = {2}" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                            , MessageIdErrorDbTimeOut))

                        Return MessageIdErrorDbTimeOut

                    Catch ex As Exception

                        Me.Rollback = True
                        ''エラーログの出力

                        Logger.Error(ex.Message, ex)
                        Throw

                    End Try

                    ' 車両登録番号の設定
                    Dim regNum As String = Nothing
                    If Not drCstVclInfo.IsREG_NUMNull Then
                        regNum = drCstVclInfo.REG_NUM
                    End If

                    ' VINの設定
                    Dim vclVin As String = Nothing
                    If Not drCstVclInfo.IsVCL_VINNull Then
                        vclVin = drCstVclInfo.VCL_VIN
                    End If

                    ' 顧客区分の設定
                    Dim cstType As String = Nothing
                    If Not drCstVclInfo.IsCST_TYPENull Then
                        cstType = drCstVclInfo.CST_TYPE
                    End If

                    ' 基幹顧客コードの設定
                    Dim dmsCstCd As String = Nothing
                    If Not drCstVclInfo.IsDMS_CST_CDNull Then
                        dmsCstCd = drCstVclInfo.DMS_CST_CD
                    End If

                    ' 顧客氏名の設定
                    Dim cstName As String = Nothing
                    If Not drCstVclInfo.IsCST_NAMENull Then
                        cstName = drCstVclInfo.CST_NAME
                    End If

                    ' 敬称の設定
                    Dim nameTitleName As String = Nothing
                    If Not drCstVclInfo.IsNAMETITLE_NAMENull Then
                        nameTitleName = drCstVclInfo.NAMETITLE_NAME
                    End If

                    ' 顧客性別の設定
                    Dim cstGender As String = Nothing
                    If Not drCstVclInfo.IsCST_GENDERNull Then
                        dmsCstCd = drCstVclInfo.CST_GENDER
                    End If

                    ' セールス担当スタッフコードの設定
                    Dim slsPicStfCd As String = Nothing
                    If Not drCstVclInfo.IsSLS_PIC_STF_CDNull Then
                        slsPicStfCd = drCstVclInfo.SLS_PIC_STF_CD
                    End If

                    ' サービス担当スタッフコードの設定
                    Dim svcPicStfCd As String = Nothing
                    If Not drCstVclInfo.IsSVC_PIC_STF_CDNull Then
                        svcPicStfCd = drCstVclInfo.SVC_PIC_STF_CD
                    End If

                    ' サービス来店者登録
                    Dim messageId As Long = ic3810101Biz.InsertServiceVisit(inDealerCode, _
                                                                            inBranchCode, _
                                                                            inNowDate, _
                                                                            regNum, _
                                                                            cstType, _
                                                                            CStr(drCstVclInfo.CST_ID), _
                                                                            slsPicStfCd, _
                                                                            VisitPersonNumberOne, _
                                                                            VisitPurposeCar, _
                                                                            vclVin, _
                                                                            drCstVclInfo.VCL_ID, _
                                                                            cstGender, _
                                                                            cstName, _
                                                                            svcPicStfCd, _
                                                                            staffInfo.Account, _
                                                                            staffInfo.UserName, _
                                                                            ApplicationId, _
                                                                            inServiceinId)

                    ' DBタイムアウトエラー時
                    If Not messageId = MessageIdSuccess Then

                        ' ロールバックする
                        Me.Rollback = True

                        ''終了ログの出力
                        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} OUT:RETURNCODE = {2}" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                            , MessageIdErrorDbTimeOut))

                        Return MessageIdErrorDbTimeOut
                    End If


                    ' 担当SCへの通知処理
                    SendOrgCustomerServiceToSC(ic3810101Biz.VisitSeqInserted, _
                                               inDealerCode, _
                                               inBranchCode, _
                                               slsPicStfCd, _
                                               cstName, _
                                               nameTitleName, _
                                               cstType, _
                                               drCstVclInfo.CST_ID, _
                                               regNum)

                    ' 通知処理
                    ic3810101Biz.NoticeProcessing(ic3810101Biz.VisitSeqInserted, _
                                                  inNowDate, _
                                                  inDealerCode, _
                                                  inBranchCode, _
                                                  staffInfo.Account, _
                                                  staffInfo.UserName)
                End If
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                          , "{0}.{1} END" _
                                          , Me.GetType.ToString _
                                          , System.Reflection.MethodBase.GetCurrentMethod.Name))

                Return MessageIdSuccess

            End Using
        End Using
    End Function
#End Region

#Region "サービス来店取消"
    ''' <summary>
    ''' サービス来店取消
    ''' </summary>
    ''' <param name="inDealerCode">ログインユーザーの販売店コード</param>
    ''' <param name="inBranchCode">ログインユーザーの店舗コード</param>
    ''' <param name="inServiceinId">選択中の予約情報のサービス入庫ID</param>
    ''' <param name="inUpdateDate">選択中の予約情報の来店更新日時</param>
    ''' <returns>メッセージID</returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Function CancelServiceVisit(ByVal inDealerCode As String, _
                                       ByVal inBranchCode As String, _
                                       ByVal inServiceinId As Decimal, _
                                       ByVal inUpdateDate As Date) As Integer

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START IN:inDealerCode = {2}, inBranchCode = {3}, inServiceinId = {4}" & _
                  ", inNowDate = {5}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , inDealerCode, inBranchCode, inServiceinId, inUpdateDate))

        Using ta As New SC3090401TableAdapter

            Dim dtLockVisitManagement As SC3090401DataSet.SC3090401LockTargetDataDataTable

            Try

                dtLockVisitManagement = ta.GetLockVisitManagement(inDealerCode, _
                                                                  inBranchCode, _
                                                                  inServiceinId)
            Catch ex As OracleExceptionEx When ex.Number = 30006
                '行ロック失敗(WAIT時間超え)

                '排他エラー
                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                             , "{0}.{1} ERR:RECORD LOCK TIMEOUT" _
                             , Me.GetType.ToString _
                             , System.Reflection.MethodBase.GetCurrentMethod.Name))

                Return MessageIdErrorReserveUpdate

            Catch ex As OracleExceptionEx When ex.Number = 1013

                ''ORACLEのタイムアウトのみ処理
                Me.Rollback = True

                ''終了ログの出力
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} OUT:RETURNCODE = {2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , MessageIdErrorDbTimeOut))

                Return MessageIdErrorDbTimeOut

            Catch ex As Exception

                Me.Rollback = True
                ''エラーログの出力

                Logger.Error(ex.Message, ex)
                Throw

            End Try

            ' 行ロック対象件数が0件の場合、排他エラー
            If dtLockVisitManagement.Count <= 0 Then

                '排他エラー
                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                             , "{0}.{1} ERR:RECORD LOCK TIMEOUT" _
                             , Me.GetType.ToString _
                             , System.Reflection.MethodBase.GetCurrentMethod.Name))

                Return MessageIdErrorReserveUpdate

            End If

            Dim reservationCount As Integer = ta.IsNotCarInStatus(inServiceinId)

            ' サービス入庫IDに紐付く未入庫の予約が存在しない場合
            If reservationCount <= 0 Then

                ' エラーメッセージを返却
                Return MessageIdErrorReserveUpdate
            End If

            ' ロック対象一覧．更新日の最大を取得
            Dim maxUpdateDate As Date = Date.MinValue

            For Each drLockVisitManagement In dtLockVisitManagement

                ' DataRowの更新日時がmaxUpdateDateよりも大きい場合
                If Date.Compare(maxUpdateDate, drLockVisitManagement.UPDATEDATE) < 0 Then

                    ' 日時の最大値を更新する
                    maxUpdateDate = drLockVisitManagement.UPDATEDATE
                End If
            Next

            ' 更新日が変更されている場合
            If Date.Compare(inUpdateDate, maxUpdateDate) < 0 Then

                ' エラーメッセージを返却
                Return MessageIdErrorReserveUpdate
            End If

            ' 予約に紐付く来店者実績連番のリストを作成
            Dim targetVisitSeqList As List(Of Decimal) = New List(Of Decimal)

            For Each drLockVisitManagement In dtLockVisitManagement

                targetVisitSeqList.Add(drLockVisitManagement.VISITSEQ)
            Next

            Using ic3810101Biz As IC3810101BusinessLogic = New IC3810101BusinessLogic

                ' サービス来店取消
                Dim messageId As Long = ic3810101Biz.DeleteServiceVisit(targetVisitSeqList)

                ' DBタイムアウトエラー時
                If Not messageId = MessageIdSuccess Then

                    ''終了ログの出力
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:RETURNCODE = {2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , MessageIdErrorDbTimeOut))

                    Return MessageIdErrorDbTimeOut
                End If

                '来店者取消通知処理
                ic3810101Biz.VisitCalcelNoticeProcessing(inDealerCode, inBranchCode)

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                          , "{0}.{1} END" _
                                          , Me.GetType.ToString _
                                          , System.Reflection.MethodBase.GetCurrentMethod.Name))

                Return MessageIdSuccess

            End Using
        End Using

    End Function

#End Region

#Region "予約情報取得"
    ''' <summary>
    ''' 予約情報取得
    ''' </summary>
    ''' <param name="inDealerCode">ログインユーザーの販売店コード</param>
    ''' <param name="inBranchCode">ログインユーザーの店舗コード</param>
    ''' <param name="inVisitFlag">来店済み取得フラグ</param>
    ''' <param name="inSortType">ソート条件区分</param>
    ''' <param name="inBeginIndex">取得する予約情報の開始行番号</param>
    ''' <param name="inEndIndex">取得する予約情報の終了行番号</param>
    ''' <param name="inNowDate">現在日時</param>
    ''' <returns>予約情報</returns>
    ''' <remarks></remarks>
    Public Function GetReservationInfo(ByVal inDealerCode As String, _
                                       ByVal inBranchCode As String, _
                                       ByVal inVisitFlag As String, _
                                       ByVal inSortType As String, _
                                       ByVal inBeginIndex As Integer, _
                                       ByVal inEndIndex As Integer, _
                                       ByVal inNowDate As Date) As SC3090401DataSet.SC3090401ReserveDataDataTable

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                  , "{0}.{1} START IN:inDealerCode = {2}, inBranchCode = {3}, inVisitFlag = {4}" & _
                                  ", inSortType = {5}, inBeginIndex = {6}, inEndIndex = {7}, inNowDate = {8}" _
                                  , Me.GetType.ToString _
                                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                  , inDealerCode, inBranchCode, inVisitFlag, inSortType _
                                  , inBeginIndex, inEndIndex, inNowDate))

        Using ta As New SC3090401TableAdapter

            Dim dt As SC3090401DataSet.SC3090401ReserveDataDataTable = _
                ta.GetReservationInfo(inDealerCode, _
                                      inBranchCode, _
                                      inVisitFlag, _
                                      inSortType, _
                                      inBeginIndex, _
                                      inEndIndex, _
                                      inNowDate)


            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                      , "{0}.{1} END" _
                                      , Me.GetType.ToString _
                                      , System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return dt

        End Using
    End Function
#End Region

#Region "予約件数取得"
    ''' <summary>
    ''' 予約件数取得
    ''' </summary>
    ''' <param name="inDealerCode">ログインユーザーの販売店コード</param>
    ''' <param name="inBranchCode">ログインユーザーの店舗コード</param>
    ''' <param name="inVisitFlag">来店済み取得フラグ</param>
    ''' <param name="inNowDate">現在日時</param>
    ''' <returns>予約件数</returns>
    ''' <remarks></remarks>
    Public Function GetReservationCount(ByVal inDealerCode As String, _
                                        ByVal inBranchCode As String, _
                                        ByVal inVisitFlag As String, _
                                        ByVal inNowDate As Date) As Integer

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                  , "{0}.{1} START IN:inDealerCode = {2}, inBranchCode = {3}, inVisitFlag = {4}" & _
                                  ", inNowDate = {5}" _
                                  , Me.GetType.ToString _
                                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                  , inDealerCode, inBranchCode, inVisitFlag, inNowDate))

        Using ta As New SC3090401TableAdapter

            Dim reservationCount As Integer = ta.GetReservationCount(inDealerCode, _
                                                          inBranchCode, _
                                                          inVisitFlag, _
                                                          inNowDate)

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                      , "{0}.{1} END" _
                                      , Me.GetType.ToString _
                                      , System.Reflection.MethodBase.GetCurrentMethod.Name))

            Return reservationCount

        End Using

    End Function

#End Region

#Region "担当SCへの送信処理"
    ''' <summary>
    ''' 送信処理_自社客・未取引客(サービス入庫の時担当SCへ)
    ''' </summary>
    ''' <param name="visitSeq">来店実績連番</param>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="storeCode">店舗コード</param>
    ''' <param name="staffCode">セールス担当スタッフコード</param>
    ''' <param name="cstName">顧客氏名</param>
    ''' <param name="nameTitleName">敬称名</param>
    ''' <param name="cstType">顧客区分</param>
    ''' <param name="cstId">顧客ID</param>
    ''' <param name="vclRegNum">車両登録番号</param>
    ''' <returns>メッセージID</returns>
    ''' <remarks></remarks>
    ''' <History>
    ''' 2019/05/10 NSK 鈴木 18PRJ00XXX_(FS)納車時オペレーションCS向上にむけた評価（サービス）
    '''                      [TKM]PUAT-3088 セールスタブレットの通知履歴をタップするとシステムエラーが発生する を修正
    ''' </History>
    Private Function SendOrgCustomerServiceToSC(ByVal visitSeq As Long, ByVal dealerCode As String, ByVal storeCode As String, _
                                                ByVal staffCode As String, ByVal cstName As String, _
                                                ByVal nameTitleName As String, ByVal cstType As String, _
                                                ByVal cstId As Decimal, ByVal vclRegNum As String) As Integer

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                  , "{0}.{1} START IN:visitSeq = {2}, dealerCode = {3}, storeCode = {4}, staffCode = {5}" & _
                                  ", cstName = {6}, nameTitleName = {7}, cstType = {8}, cstId = {9}, vclRegNum = {10}" _
                                  , Me.GetType.ToString _
                                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                  , visitSeq, dealerCode, storeCode, staffCode, cstName, nameTitleName, cstType, cstId, vclRegNum))

        Dim isExistCustStaff As Boolean = False
        Dim messageId As Integer = MessageIdSuccess
        Dim operationCodeList As New List(Of Decimal)
        operationCodeList.Add(8)
        Dim presenceCategoryList As New List(Of String)
        presenceCategoryList.Add(PresenceCategoryStandby)
        presenceCategoryList.Add(PresenceCategoryNegotiate)
        presenceCategoryList.Add(PresenceCategoryLeaving)
        presenceCategoryList.Add(PresenceCategoryOffline)

        Using adapter As New SC3090401TableAdapter

            '顧客担当スタッフの有無
            If Not String.IsNullOrEmpty(Trim(staffCode)) Then

                '顧客担当スタッフが存在する
                isExistCustStaff = True
                Logger.Debug("SendOrgOrNewCustomerService_001 CustomerStaff Exist")

                '顧客担当スタッフのステータス取得
                Using staffStatusTbl As VisitUtilityUsersDataTable = _
                    VisitUtilityDataSetTableAdapter.GetUsers(dealerCode, storeCode, _
                        operationCodeList, presenceCategoryList, "0", staffCode)

                    If staffStatusTbl.Rows.Count <= 0 Then
                        isExistCustStaff = False
                    End If

                End Using

            End If

            '顧客担当スタッフが存在しない場合は、処理を抜ける
            If Not isExistCustStaff Then

                Logger.Debug("SendOrgOrNewCustomerService_End Ret[" & MessageIdSuccess & "]")
                Return MessageIdSuccess
            End If

            'お客様名
            Dim custName As String = CreateCustomerName(cstName, nameTitleName)

            'スタッフ情報の取得(セールススタッフ)
            Using salesStaffInfoTbl As VisitUtilityUsersDataTable = _
                VisitUtilityDataSetTableAdapter.GetUsers(dealerCode, storeCode, _
                    operationCodeList, presenceCategoryList, "0", staffCode)

                ' 苦情情報
                Dim isClaime As Boolean = HasClaimed(dealerCode, cstType, CStr(cstId))

                Dim MessageService As String = WebWordUtility.GetWord(ServiceinWord)

                ' 顧客担当SCへサービス入庫通知を送信
                ' 2019/05/10 NSK 鈴木 18PRJ00XXX_(FS)納車時オペレーションCS向上にむけた評価（サービス）
                '                      [TKM]PUAT-3088 セールスタブレットの通知履歴をタップするとシステムエラーが発生する を修正 START
                ' messageId = SendStandbyStaff(salesStaffInfoTbl, staffCode, _
                '                 CreateSendMessage(custName, vclRegNum, MessageService, isClaime), _
                '                 staffCode, custName, cstType, visitSeq)
                messageId = SendStandbyStaff(salesStaffInfoTbl, staffCode, _
                                 CreateSendMessage(custName, vclRegNum, MessageService, isClaime), _
                                 CStr(cstId), custName, cstType, visitSeq)
                ' 2019/05/10 NSK 鈴木 18PRJ00XXX_(FS)納車時オペレーションCS向上にむけた評価（サービス）
                '                      [TKM]PUAT-3088 セールスタブレットの通知履歴をタップするとシステムエラーが発生する を修正 END
            End Using

        End Using

        Logger.Debug("SendCustomerInfoToStaff_End Ret[" & messageId & "]")
        Return messageId

    End Function

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

    ''' <summary>
    ''' セールススタッフ[スタンバイ]への通知処理
    ''' </summary>
    ''' <param name="staffList">セールススタッフリスト</param>
    ''' <param name="customerStaffCode">顧客担当スタッフコード</param>
    ''' <param name="pushMessage">Postメッセージ</param>
    ''' <param name="customerID">顧客ID</param>
    ''' <param name="customerName">顧客名</param>
    ''' <param name="customerClass">顧客分類</param>
    ''' <param name="visitSequence">来店実績連番</param>
    ''' <returns>メッセージID</returns>
    ''' <remarks></remarks>
    Private Function SendStandbyStaff(ByVal staffList As VisitUtilityUsersDataTable, ByVal customerStaffCode As String, _
                                 ByVal pushMessage As String, ByVal customerID As String, _
                                 ByVal customerName As String, ByVal customerClass As String, _
                                 ByVal visitSequence As Long) As Integer

        Logger.Debug("SendStandbyStaff_Start Pram[(staffList IsNot Nothing)" & "," & customerStaffCode & "," & _
                                                      pushMessage & "," & customerID & "," & customerName & "," & "]")
        '通知IFへ渡すクラスの生成
        Dim noticeData As Toyota.eCRB.Tool.Notify.Api.DataAccess.XmlNoticeData
        noticeData = CreateInputClassService(pushMessage, staffList, customerID, customerName, customerClass, visitSequence)

        If noticeData Is Nothing Then
            Return MessageIdSuccess
        End If

        '通知IFの呼び出し
        Dim returnXml As Toyota.eCRB.Tool.Notify.Api.DataAccess.XmlCommon = Nothing
        Using ic3040801Biz As New IC3040801BusinessLogic

            Logger.Info("SendStandbyStaff Call_Start IC3040801BusinessLogic.NoticeDisplay Pram[" & (noticeData IsNot Nothing) & "]")
            returnXml = ic3040801Biz.NoticeDisplay(noticeData, NoticeDisposal.Peculiar)
            Logger.Info("SendStandbyStaff Call_End IC3040801BusinessLogic.NoticeDisplay Ret[" & returnXml.Message & "," & returnXml.ResultId & "]")

        End Using

        ' 戻り値判断
        ' IFの戻り値は成功='0'又はDBアクセスエラー='6000'のため成功かどうかのみで判断
        ' DBアクセスエラー以外のエラーはExceptionで帰ってくるためそのまま基盤へthrow
        If String.Equals(returnXml.ResultId, "006000") Then

            Logger.Debug("SendStandbyStaff_End Ret[" & MessageIdErrorDbTimeOut & "]")
            Return MessageIdErrorDbTimeOut
        End If

        Logger.Debug("SendStandbyStaff_End Ret[" & MessageIdSuccess & "]")
        Return MessageIdSuccess
    End Function

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
        Dim pushStandbyStaffMessage As New StringBuilder

        '苦情情報の有無を判定
        If claimeInfo Then
            Logger.Debug("CreateSendMessage_001 " & "Call_Start WebWordUtility.GetWord Param[" & ClameWord & "]")
            Dim claimMessage As String = WebWordUtility.GetWord(ClameWord)
            Logger.Debug("CreateSendMessage_001 " & "Call_End WebWordUtility.GetWord")
            pushStandbyStaffMessage.Append(claimMessage)
            pushStandbyStaffMessage.Append(" ")
        End If

        '送信メッセージ作成
        pushStandbyStaffMessage.Append(customerName)
        pushStandbyStaffMessage.Append(" ")
        pushStandbyStaffMessage.Append(message)
        pushStandbyStaffMessage.Append(" ")
        pushStandbyStaffMessage.Append(vehicleNo)

        Logger.Debug("CreateSendMessage_End Ret[" & pushStandbyStaffMessage.ToString & "]")
        Return pushStandbyStaffMessage.ToString

    End Function

    ''' <summary>
    ''' 通知IFへ渡すXmlNoticeDataクラスの作成処理(サービス入庫)
    ''' </summary>
    ''' <param name="pushMessage">Postメッセージ</param>
    ''' <param name="staffList">セールススタッフリスト</param>
    ''' <param name="customerID">顧客ID</param>
    ''' <param name="customerName">顧客名</param>
    ''' <param name="customerClass">顧客分類</param>
    ''' <param name="visitSequence">来店実績連番</param>
    ''' <returns>XmlNoticeDataクラス</returns>
    ''' <remarks></remarks>
    Private Function CreateInputClassService(ByVal pushMessage As String, ByVal staffList As VisitUtilityUsersDataTable, _
                               ByVal customerID As String, _
                               ByVal customerName As String, ByVal customerClass As String, _
                               ByVal visitSequence As Long) As Toyota.eCRB.Tool.Notify.Api.DataAccess.XmlNoticeData

        Logger.Info("CreateInputClassService_Start Pram[" & pushMessage & "," & _
                    (staffList IsNot Nothing) & "," & customerID & "," & customerName & "]")

        Dim standbyStaffCount As Integer = 0

        Dim returnValue As Toyota.eCRB.Tool.Notify.Api.DataAccess.XmlNoticeData = _
            New Toyota.eCRB.Tool.Notify.Api.DataAccess.XmlNoticeData

        'ヘッダー情報
        returnValue.TransmissionDate = DateTimeFunc.Now(StaffContext.Current.DlrCD)
        Logger.Info("CreateInputClassService SetValue TransmissionDate[" & returnValue.TransmissionDate & "]")

        '来店通知命令の送信(顧客担当スタッフ)
        For Each salesStaffInfoRow As VisitUtilityUsersRow In staffList.Rows

            Dim xmlAccount As XmlAccount = New XmlAccount
            xmlAccount.ToAccount = salesStaffInfoRow.ACCOUNT
            xmlAccount.ToAccountName = salesStaffInfoRow.USERNAME

            Logger.Info("CreateInputClassService SetValue XmlAccount ToAccount[" & salesStaffInfoRow.ACCOUNT & "]")
            Logger.Info("CreateInputClassService SetValue XmlAccount ToAccountName[" & salesStaffInfoRow.USERNAME & "]")

            returnValue.AccountList.Add(xmlAccount)
            standbyStaffCount = standbyStaffCount + 1
            If standbyStaffCount >= 1 Then Exit For
        Next

        ' standbyStaffが0人なら以降処理は行わない。
        If standbyStaffCount = 0 Then
            Return Nothing
        End If

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

#End Region

End Class
