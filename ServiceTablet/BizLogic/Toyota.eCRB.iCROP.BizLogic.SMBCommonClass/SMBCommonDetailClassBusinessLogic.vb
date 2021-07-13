'-------------------------------------------------------------------------
'SMBCommonDetailClassBusinessLogic.vb
'-------------------------------------------------------------------------
'機能：共通関数API
'補足：
'作成：2012/05/11 KN 小澤
'更新：2012/06/06 KN 小澤 STEP2事前準備対応
'更新：2012/06/19 KN 小澤 STEP2対応(事前準備用の処理削除)
'更新：2012/08/16 TMEJ 日比野 STEP2(車種は顧客参照情報を優先して表示するように修正)
'更新：2012/09/21 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.35）
'更新：2012/09/27 TMEJ 日比野 キャンセルしたチップが表示される不具合対応
'更新：2012/11/02 TMEJ 小澤 【SERVICE_2】次世代サービスROステータス切り離し対応
'更新：2012/11/03 TMEJ 河原 【A.STEP2】次世代サービス ROステータス切り離し対応
'更新：2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成
'更新：2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発
'更新：2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発
'更新：2015/06/26 TMEJ 明瀬 TMTレスポンス調査のためのログ出力
'更新：2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発
'更新：2016/06/09 NSK 皆川 TR-V4-TMT-20160107-001
'更新：2017/09/06 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一
'更新：2018/02/22 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証
'更新：2018/07/06 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示
'更新：2019/06/14 NSK 鈴木 [TKM]PUAT-4100 連続で追加作業起票するとRO発行ボタンが押せなくなる
'更新：2019/07/02 NSK 鈴木 [TKM]PUAT-4100-1 SAメインでチップとチップ詳細の項目に差異がある
'更新：
'─────────────────────────────────────
Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.CommonUtility.SMBCommonClass.Api.DataAccess.SMBCommonClassDataSet
Imports Toyota.eCRB.CommonUtility.SMBCommonClass.Api.DataAccess.SMBCommonClassDataSetTableAdapters
'2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
'Imports Toyota.eCRB.DMSLinkage.CustomerInfo.BizLogic.IC3800703
'Imports Toyota.eCRB.DMSLinkage.CustomerInfo.DataAccess.IC3800703.IC3800703DataSet
'Imports Toyota.eCRB.DMSLinkage.DeliveryDateList.BizLogic.IC3801701
'Imports Toyota.eCRB.DMSLinkage.DeliveryDateList.DataAccess.IC3801701
'Imports Toyota.eCRB.DMSLinkage.RepairOrderStatus.BizLogic.IC3801901
'Imports Toyota.eCRB.DMSLinkage.RepairOrderStatus.DataAccess.IC3801901
'2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
'2012/11/03 TMEJ 河原 【A.STEP2】次世代サービス ROステータス切り離し対応 START
'2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
'Imports Toyota.eCRB.DMSLinkage.AddRepair.BizLogic.IC3800804
'Imports Toyota.eCRB.DMSLinkage.AddRepair.DataAccess.IC3800804
'Imports Toyota.eCRB.DMSLinkage.AddRepair.DataAccess.IC3800804.IC3800804DataSet
'2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
'2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
'Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.BizLogic.IC3801012
'Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.DataAccess.IC3801012
'Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.BizLogic.IC3801001
'Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.DataAccess.IC3801001
'2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END
'2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.DMSLinkage.PartsInfo.Api.BizLogic
Imports Toyota.eCRB.DMSLinkage.PartsInfo.Api.DataAccess.IC3802503DataSet
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.BizLogic

'2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
'Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.BizLogic.IC3801014
'Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.DataAccess.IC3801014
'2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

'2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

'2012/11/03 TMEJ 河原 【A.STEP2】次世代サービス ROステータス切り離し対応

Partial Class SMBCommonClassBusinessLogic

#Region "定数"

    ''' <summary>
    ''' デフォルト時刻
    ''' </summary>
    Private Const DEFAULT_TIME As String = "--:--"

    ''' <summary>
    ''' 日付フォーマット
    ''' </summary>
    Private Const FORMAT_DATE As String = "yyyyMMddHHmm"

    ''' <summary>
    ''' 自社客：1
    ''' </summary>
    Private Const COMPANY_VISITOR As String = "1"

    '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
    ''' <summary>
    ''' 中断区分：01：部品欠品
    ''' </summary>
    Private Const StopTypePartsMissing As String = "01"
    Private Const StopTypePartsMissingWordId As Long = 301
    ''' <summary>
    ''' 中断区分：02：お客様連絡待ち
    ''' </summary>
    Private Const StopTypeVisitorConnectionWaiting As String = "02"
    Private Const StopTypeVisitorConnectionWaitingWordId As Long = 302
    ''' <summary>
    ''' 中断区分：03：検査不合格
    ''' </summary>
    Private Const StopTypeInspectionFailure As String = "03"
    Private Const StopTypeInspectionFailureWordId As Long = 303
    ''' <summary>
    ''' 中断区分：99：その他
    ''' </summary>
    Private Const StopTypeOther As String = "99"
    Private Const StopTypeOtherWordId As Long = 304
    '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

#End Region

#Region "Publicメソッド"

    ''' <summary>
    ''' チップ詳細情報取得(来店)
    ''' </summary>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inStoreCode">店舗コード</param>
    ''' <param name="inVisitSequence">来店実績連番</param>
    ''' <returns>チップ詳細情報</returns>
    ''' <remarks></remarks>
    ''' <History>
    ''' 2012/11/03 TMEJ 河原 【A.STEP2】次世代サービス ROステータス切り離し対応
    ''' 2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発
    ''' </History>
    Public Function GetChipDetailVisit(ByVal inDealerCode As String, _
                                       ByVal inStoreCode As String, _
                                       ByVal inVisitSequence As Decimal) As ChipDetail
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} inDealerCode:{2} inStoreCode:{3} inVisitSequence:{4}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , inDealerCode, inStoreCode, inVisitSequence.ToString(CultureInfo.InvariantCulture)))

        Dim dtChipDetailVisit As ChipDetailVisitDataTable = Nothing
        Dim dtChipDetailReserve As ChipDetailReserveDataTable = Nothing
        Dim dtChipDetailProcess As ChipDetailProcessDataTable = Nothing

        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
        'Dim dtSrvCustomer As New IC3800703SrvCustomerDataTable
        Dim dtChipDetailCustomerInfo As ChipDetailCustomerInfoDataTable = Nothing
        Dim dtChipDetailRepairOrderInfo As ChipDetailRepairOrderInfoDataTable = Nothing
        Dim partsStatus As String = String.Empty
        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        Dim dtStop As StopDataTable = Nothing

        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
        'Dim dtHistoryDeliveryDateList As New IC3801701DataSet.HistoryDeliveryDateListDataTable
        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        Dim daSMBCommonClass As New SMBCommonClassTableAdapter
        Try
            '戻り値
            Dim returnData As New ChipDetail
            '現在日時
            Dim nowDate As DateTime = DateTimeFunc.Now(inDealerCode, inStoreCode)

            'チップ詳細情報取得(来店)
            dtChipDetailVisit = daSMBCommonClass.GetChipDetailVisitData(inDealerCode, _
                                                                        inStoreCode, _
                                                                        inVisitSequence)
            'データが取得できなかった場合はNULLを返す
            If dtChipDetailVisit.Count = 0 Then
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} OUT:RETURN = {2}" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                            , ReturnCode.ErrNoCases))
                Return Nothing
            End If
            'この後に色々と使用するためここでROWを取得する
            Dim drChipDetailVisit As ChipDetailVisitRow = _
                DirectCast(dtChipDetailVisit.Rows(0), ChipDetailVisitRow)

            '初回予約IDが存在する場合はチップ予約情報取得(予約)をする
            If Not (drChipDetailVisit.IsFREZIDNull) Then
                'チップ詳細情報取得(予約)
                dtChipDetailReserve = _
                    daSMBCommonClass.GetChipDetailReserveData(inDealerCode, _
                                                              inStoreCode, _
                                                              drChipDetailVisit.FREZID)

                'チップ詳細情報取得(実績)
                dtChipDetailProcess = _
                    daSMBCommonClass.GetChipDetailProcessData(inDealerCode, _
                                                              inStoreCode, _
                                                              drChipDetailVisit.FREZID)

                '中断理由取得
                dtStop = daSMBCommonClass.GetStopData(inDealerCode, _
                                                      inStoreCode, _
                                                      drChipDetailVisit.FREZID)

            Else
                dtChipDetailReserve = Nothing
                dtChipDetailProcess = Nothing
                dtStop = Nothing

            End If

            '顧客参照を取得する
            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
            'dtSrvCustomer = Me.GetSrvCustomer(inDealerCode, drChipDetailVisit)

            '顧客IDと車両IDが存在する場合は取得する
            If Not (drChipDetailVisit.IsCUSTIDNull) AndAlso 0 < drChipDetailVisit.CUSTID AndAlso _
               Not (drChipDetailVisit.IsVCL_IDNull) AndAlso 0 < drChipDetailVisit.VCL_ID Then
                dtChipDetailCustomerInfo = daSMBCommonClass.GetCustomerInfo(inDealerCode, _
                                                                            drChipDetailVisit.CUSTID, _
                                                                            drChipDetailVisit.VCL_ID)

            End If
            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
            '「整備受注No <> NULL」の場合はステータス情報をする
            'Dim drOrderStatus As IC3801901DataSet.OrderStatusDataRow
            'If Not (drChipDetailVisit.IsORDERNONull) Then
            '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
            '                   , "{0}.{1} P1:{2} P2:{3} P3:{4}" _
            '                   , Me.GetType.ToString _
            '                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
            '                   , inDealerCode, inStoreCode, drChipDetailVisit.ORDERNO))
            '    Dim bizIC3801901 As New IC3801901BusinessLogic
            '    drOrderStatus = bizIC3801901.GetOrderStatus(inDealerCode, _
            '                                                inStoreCode, _
            '                                                drChipDetailVisit.ORDERNO)
            'Else
            '    drOrderStatus = Nothing
            'End If

            'RO情報のチェック
            If RepairOrderTypeExist.Equals(drChipDetailVisit.RO_TYPE) Then
                'RO情報が存在する場合
                'RO情報を取得
                dtChipDetailRepairOrderInfo = daSMBCommonClass.GetRepariOrderInfo(inDealerCode, _
                                                                                  inStoreCode, _
                                                                                  drChipDetailVisit.VISITSEQ)

                '部品ステータスを取得
                partsStatus = Me.GetPartsStatus(inDealerCode, _
                                                inStoreCode, _
                                                drChipDetailVisit.ORDERNO, _
                                                Nothing)

            Else
                'RO情報が存在しない場合
                dtChipDetailRepairOrderInfo = Nothing

            End If
            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

            '2012/11/03 TMEJ 河原 【A.STEP2】次世代サービス ROステータス切り離し対応 START
            ' 2012/09/27 TMEJ 日比野 キャンセルしたチップが表示される不具合対応 START
            ''予約の有無
            'Dim reserveExistence As String = String.Empty

            'If dtChipDetailReserve IsNot Nothing AndAlso 0 < dtChipDetailReserve.Rows.Count Then
            '    reserveExistence = "1"  '予約の有無:あり
            'Else
            '    reserveExistence = "0"  '予約の有無:なし
            'End If

            '表示区分判定を取得する
            'Dim chipArea As DisplayType = Me.GetDispType(dtChipDetailProcess, drOrderStatus)
            'Dim chipArea As DisplayType = Me.GetDispType(dtChipDetailProcess, drOrderStatus, reserveExistence)

            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
            'Dim chipArea As DisplayType = Me.GetDispType(drOrderStatus, drChipDetailVisit, dtChipDetailReserve)
            Dim chipArea As DisplayType = Me.GetDispType(dtChipDetailRepairOrderInfo, _
                                                         drChipDetailVisit, _
                                                         dtChipDetailReserve, _
                                                         dtChipDetailProcess)
            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

            ' 2012/09/27 TMEJ 日比野 キャンセルしたチップが表示される不具合対応 END

            '2012/11/03 TMEJ 河原 【A.STEP2】次世代サービス ROステータス切り離し対応 END

            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
            ''「整備受注No <> NULL AndAlso 表示区分 <> 1：受付」の場合は納車予定変更履歴取得をする
            'If Not (drChipDetailVisit.IsORDERNONull) AndAlso _
            '   chipArea <> DisplayType.Invalid AndAlso chipArea <> DisplayType.Err Then
            '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
            '                   , "CALL [IC3801701] {0}.{1} P1:{2} P2:{3} P3:{4}" _
            '                   , Me.GetType.ToString _
            '                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
            '                   , inDealerCode, inStoreCode, drChipDetailVisit.ORDERNO))
            '    Dim bizIC3801701 As New IC3801701BusinessLogic
            '    dtHistoryDeliveryDateList = _
            '        bizIC3801701.GetHistoryDeliveryDateList(inDealerCode, _
            '                                                inStoreCode, _
            '                                                drChipDetailVisit.ORDERNO)
            'Else
            '    dtHistoryDeliveryDateList = Nothing
            'End If
            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

            'ステータス判定を取得する
            '2012/11/28 TMEJ 小澤 問連「GTMC121126088」対応 START
            'Dim status() As String = GetStatusVisit(drChipDetailVisit. _
            '                                        CUSTSEGMENT, _
            '                                        dtChipDetailProcess, _
            '                                        drOrderStatus)

            Dim status() As String = GetStatusVisit(drChipDetailVisit, _
                                                    dtChipDetailReserve, _
                                                    dtChipDetailProcess, _
                                                    dtChipDetailRepairOrderInfo, _
                                                    partsStatus)
            '2012/11/28 TMEJ 小澤 問連「GTMC121126088」対応 END

            '「表示区分 <> 1:受付、0:エラー AndAlso 有効以外件数 = 0」の場合は納車見込時刻を取得する
            Dim deliveryDate As String = DEFAULT_TIME
            If chipArea <> DisplayType.Invalid AndAlso chipArea <> DisplayType.Err AndAlso _
               dtChipDetailProcess IsNot Nothing AndAlso 0 < dtChipDetailProcess.Count Then
                Dim drChipDetailProcess As ChipDetailProcessRow = _
                    DirectCast(dtChipDetailProcess.Rows(0), ChipDetailProcessRow)
                If drChipDetailProcess.STOPCOUNT = 0 Then

                    '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
                    'deliveryDate = Me.GetDeliveryHopeDate(inDealerCode, _
                    '                                      inStoreCode, _
                    '                                      chipArea, _
                    '                                      dtChipDetailProcess, _
                    '                                      drOrderStatus, _
                    '                                      nowDate)

                    deliveryDate = Me.GetDeliveryHopeDate(inDealerCode, _
                                                          inStoreCode, _
                                                          chipArea, _
                                                          dtChipDetailProcess, _
                                                          dtChipDetailRepairOrderInfo, _
                                                          nowDate)
                    '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

                End If

            End If

            'Returnのデータ作成
            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
            'returnData = Me.SetVisitData(status, _
            '                             chipArea, _
            '                             deliveryDate, _
            '                             nowDate, _
            '                             dtChipDetailVisit, _
            '                             dtChipDetailReserve, _
            '                             dtChipDetailProcess, _
            '                             dtSrvCustomer, _
            '                             dtStop, _
            '                             dtHistoryDeliveryDateList, _
            '                             drOrderStatus)

            returnData = Me.SetVisitData(status, _
                                         chipArea, _
                                         deliveryDate, _
                                         nowDate, _
                                         partsStatus, _
                                         dtChipDetailVisit, _
                                         dtChipDetailReserve, _
                                         dtChipDetailProcess, _
                                         dtChipDetailCustomerInfo, _
                                         dtStop, _
                                         dtChipDetailRepairOrderInfo)
            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

            '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START

            '顧客車両区分を付与した顧客氏名を取得
            Using GetCstNameAddCstVclType As New ServiceCommonClassBusinessLogic

                returnData.CustomerName = GetCstNameAddCstVclType.GetCstNameWithCstVclType(returnData.CustomerName, _
                                                                                           returnData.CustomerVehicleType, _
                                                                                           returnData.NameCustomerType)
                'Null or 空チェック
                If String.IsNullOrEmpty(returnData.CustomerName) Then
                    'エラーログ
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.{1} .Error GetCstNameWithCstVclType Is Empty VISITSEQ = {2}", _
                                  Me.GetType.ToString, _
                                  Reflection.MethodBase.GetCurrentMethod.Name, _
                                  inVisitSequence))

                End If

            End Using

            '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:RETURN = {2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , ReturnCode.Success))
            Return returnData

        Catch ex As OracleExceptionEx When ex.Number = 1013
            'DBタイムアウト
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:RETURN = {2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , ReturnCode.ErrDBTimeout))
            Return Nothing

        Finally
            If dtChipDetailVisit IsNot Nothing Then dtChipDetailVisit.Dispose()
            If dtChipDetailReserve IsNot Nothing Then dtChipDetailReserve.Dispose()
            If dtChipDetailProcess IsNot Nothing Then dtChipDetailProcess.Dispose()

            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
            'If dtSrvCustomer IsNot Nothing Then dtSrvCustomer.Dispose()
            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

            If dtStop IsNot Nothing Then dtStop.Dispose()

            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
            'If dtHistoryDeliveryDateList IsNot Nothing Then dtHistoryDeliveryDateList.Dispose()
            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

            If daSMBCommonClass IsNot Nothing Then daSMBCommonClass.Dispose()

            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
            If dtChipDetailCustomerInfo IsNot Nothing Then dtChipDetailCustomerInfo.Dispose()
            If dtChipDetailRepairOrderInfo IsNot Nothing Then dtChipDetailRepairOrderInfo.Dispose()
            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        End Try
    End Function

    ''' <summary>
    ''' チップ詳細情報取得(予約)
    ''' </summary>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inStoreCode">店舗コード</param>
    ''' <param name="inReserveId">予約ID</param>
    ''' <returns>チップ詳細情報</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発
    ''' </history>
    Public Function GetChipDetailReserve(ByVal inDealerCode As String, _
                                         ByVal inStoreCode As String, _
                                         ByVal inReserveId As Decimal) As ChipDetail
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} inDealerCode:{2} inStoreCode:{3} inReserveId:{4}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , inDealerCode, inStoreCode, inReserveId.ToString(CultureInfo.InvariantCulture)))

        Dim dtChipDetailReserve As ChipDetailReserveDataTable = Nothing

        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
        'Dim dtSrvCustomer As New IC3800703SrvCustomerDataTable
        'Dim dtOrderStatus As IC3801901DataSet.OrderStatusDataRow
        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        Dim daSMBCommonClass As New SMBCommonClassTableAdapter
        Try
            '戻り値
            Dim returnData As New ChipDetail

            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
            ''現在日時
            'Dim nowDate As DateTime = DateTimeFunc.Now(inDealerCode, inStoreCode)
            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

            'ストール予約取得
            dtChipDetailReserve = daSMBCommonClass.GetChipDetailReserveData(inDealerCode, _
                                                                            inStoreCode, _
                                                                            inReserveId)
            'データが取得できなかった場合はNULLを返す
            If dtChipDetailReserve.Count = 0 Then
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} OUT:RETURN = {2}" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                            , ReturnCode.ErrNoCases))
                Return Nothing
            End If

            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
            'Dim drChipDetailReserve As ChipDetailReserveRow = _
            '    DirectCast(dtChipDetailReserve.Rows(0), ChipDetailReserveRow)
            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
            ''「顧客区分 = 1：自社客 AndAlso (車両登録No <> NULL OrElse VIN <> NULL)」の場合は顧客参照をする
            'If COMPANY_VISITOR.Equals(drChipDetailReserve.CUSTOMERFLAG) AndAlso _
            '   (Not (String.IsNullOrWhiteSpace(drChipDetailReserve.VCLREGNO)) OrElse _
            '    Not (String.IsNullOrWhiteSpace(drChipDetailReserve.VIN))) Then
            '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
            '                   , "{0}.{1} P1:{2} P2:{3} P3:{4}" _
            '                   , Me.GetType.ToString _
            '                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
            '                   , drChipDetailReserve.VCLREGNO, drChipDetailReserve.VIN, inDealerCode))
            '    Dim bizIC3800703 As New IC3800703BusinessLogic
            '    dtSrvCustomer = _
            '        bizIC3800703.GetCustomerInfo(drChipDetailReserve.VCLREGNO, _
            '                                     drChipDetailReserve.VIN, _
            '                                     inDealerCode)
            'Else
            '    dtSrvCustomer = Nothing
            'End If
            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
            ''「整備受注No <> NULL」の場合はステータス参照をする
            'If Not (drChipDetailReserve.IsORDERNONull) Then
            '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
            '                   , "CALL [IC3801901] {0}.{1} P1:{2} P2:{3} P3:{4}" _
            '                   , Me.GetType.ToString _
            '                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
            '                   , inDealerCode, inStoreCode, drChipDetailReserve.ORDERNO))
            '    Dim bizIC3801901 As New IC3801901BusinessLogic
            '    dtOrderStatus = bizIC3801901.GetOrderStatus(inDealerCode, _
            '                                                inStoreCode, _
            '                                                drChipDetailReserve.ORDERNO)
            'Else
            '    dtOrderStatus = Nothing
            'End If
            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
            ''ステータス判定を取得する
            'Dim status() As String = _
            '    Me.GetStatusReserve(drChipDetailReserve.CUSTOMERFLAG, dtOrderStatus)
            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
            ''Returnのデータ作成
            'returnData = Me.SetReserveData(status, _
            '                               nowDate, _
            '                               dtChipDetailReserve, _
            '                               dtSrvCustomer, _
            '                               dtOrderStatus)
            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:RETURN = {2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , ReturnCode.Success))

            Return returnData
            'DBタイムアウト
        Catch ex As OracleExceptionEx When ex.Number = 1013
            Logger.Error(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:RETURN = {2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , ReturnCode.ErrDBTimeout))
            Return Nothing

        Finally
            If dtChipDetailReserve IsNot Nothing Then dtChipDetailReserve.Dispose()

            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
            'If dtSrvCustomer IsNot Nothing Then dtSrvCustomer.Dispose()
            'If dtOrderStatus IsNot Nothing Then dtOrderStatus.Dispose()
            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

            If daSMBCommonClass IsNot Nothing Then daSMBCommonClass.Dispose()

        End Try

    End Function

    ''' <summary>
    ''' チップ詳細情報取得(SMB)
    ''' </summary>
    ''' <param name="drSMBChipDetailInputInfo">SMBチップ情報</param>
    ''' <returns>チップ詳細情報</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発
    ''' 2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発
    ''' </history>
    Public Function GetSmbChipDetail(ByVal drSmbChipDetailInputInfo As SmbChipDetailInputInfoRow) As ChipDetail
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} " _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
        'Public Function GetSmbChipDetail(ByVal inDealerCode As String, _
        '                                 ByVal inStoreCode As String, _
        '                                 ByVal inServiceInId As Decimal, _
        '                                 ByVal inVisitType As String, _
        '                                 ByVal inWorkStartDate As Date, _
        '                                 ByVal inWorkEndDate As Date, _
        '                                 ByVal inWashType As String, _
        '                                 ByVal inOrderNo As String, _
        '                                 ByVal inCompleteExaminationType As String, _
        '                                 ByVal inStallUseStatus As String, _
        '                                 ByVal inOrderJobSequence As Long, _
        '                                 ByVal inSequenceNo As Long, _
        '                                 ByVal inDeliveryDate As Date, _
        '                                 ByVal inStalluseId As Decimal) As ChipDetail
        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        Dim daSMBCommonClass As New SMBCommonClassTableAdapter
        Try
            '戻り値
            Dim returnData As New ChipDetail
            '現在日時
            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
            'Dim nowDate As DateTime = DateTimeFunc.Now(inDealerCode, inStoreCode)

            Dim nowDate As DateTime = DateTimeFunc.Now(drSmbChipDetailInputInfo.DLR_CD, _
                                                       drSmbChipDetailInputInfo.BRN_CD)
            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

            'ストール予約取得
            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
            'Dim dtChipDetailReserve As ChipDetailReserveDataTable = _
            '    daSMBCommonClass.GetChipDetailReserveData(inDealerCode, _
            '                                              inStoreCode, _
            '                                              inServiceInId)

            Dim dtChipDetailReserve As ChipDetailReserveDataTable = _
                daSMBCommonClass.GetChipDetailReserveData(drSmbChipDetailInputInfo.DLR_CD, _
                                                          drSmbChipDetailInputInfo.BRN_CD, _
                                                          drSmbChipDetailInputInfo.SVCIN_ID)
            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

            'チップ詳細情報取得(実績)
            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
            'Dim dtChipDetailProcess As ChipDetailProcessDataTable = _
            '    daSMBCommonClass.GetChipDetailProcessData(inDealerCode, _
            '                                              inStoreCode, _
            '                                              inServiceInId)

            Dim dtChipDetailProcess As ChipDetailProcessDataTable = _
                daSMBCommonClass.GetChipDetailProcessData(drSmbChipDetailInputInfo.DLR_CD, _
                                                          drSmbChipDetailInputInfo.BRN_CD, _
                                                          drSmbChipDetailInputInfo.SVCIN_ID)
            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

            '中断理由取得
            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
            'Dim dtStop As StopDataTable = Me.GetSMBStopInfo(inDealerCode, _
            '                                                inStoreCode, _
            '                                                inStalluseId, _
            '                                                inStallUseStatus, _
            '                                                daSMBCommonClass)

            Dim dtStop As StopDataTable = Me.GetSMBStopInfo(drSmbChipDetailInputInfo.DLR_CD, _
                                                            drSmbChipDetailInputInfo.BRN_CD, _
                                                            drSmbChipDetailInputInfo.STALL_USE_ID, _
                                                            drSmbChipDetailInputInfo.STALL_USE_STATUS, _
                                                            daSMBCommonClass)
            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

            'RO基本情報を取得する
            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
            'Dim drOrderCommon As IC3801001DataSet.IC3801001OrderCommRow = _
            '    Me.GetOrderCommonInfo(inDealerCode, _
            '                          inStoreCode, _
            '                          inOrderNo)
            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

            'ステータス情報を取得する
            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
            'Dim drOrderStatus As IC3801901DataSet.OrderStatusDataRow = _
            '    Me.GetOrderStatusInfo(inDealerCode, _
            '                          inStoreCode, _
            '                          inOrderNo)

            Dim dtChipDetailRepairOrderInfo As ChipDetailRepairOrderInfoDataTable = _
                Me.GetSMBRepairOrderInfo(drSmbChipDetailInputInfo.DLR_CD, _
                                         drSmbChipDetailInputInfo.BRN_CD, _
                                         drSmbChipDetailInputInfo.VISIT_SEQ, _
                                         drSmbChipDetailInputInfo.RO_TYPE, _
                                         daSMBCommonClass)
            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
            ''追加作業ステータスの取得する
            'Dim dtAddRepairStatus As IC3800804AddRepairStatusDataTableDataTable = _
            '    Me.GetAddRepairStatusInfo(inDealerCode, _
            '                              inOrderNo)
            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
            'R/O事前準備状態一覧を取得する
            'Dim drReserveROStatusList As IC3801012DataSet.REZROStatusListRow = _
            '    Me.GetReserveOrderStatusInfo(inDealerCode, _
            '                                 inStoreCode, _
            '                                 inOrderNo)
            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
            ''故障原因を取得する
            'Dim dtFaultReasonInfo As IC3801014DataSet.IC3801014FaultReasonInfoDataTable = _
            '    Me.GetFaultReasonInfo(inDealerCode, _
            '                          inOrderNo, _
            '                          inOrderJobSequence)
            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

            '表示区分の取得
            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
            'Dim chipArea As DisplayType = Me.GetSMBDispType(drOrderStatus, _
            '                                                inOrderNo, _
            '                                                dtAddRepairStatus)

            Dim chipArea As DisplayType = Me.GetSMBDispType(dtChipDetailReserve, _
                                                            dtChipDetailProcess, _
                                                            dtChipDetailRepairOrderInfo, _
                                                            drSmbChipDetailInputInfo.RO_NUM)
            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
            ''納車予定変更履歴取得をする
            'Dim dtHistoryDeliveryDateList As IC3801701DataSet.HistoryDeliveryDateListDataTable = _
            '    Me.GetHistoryDeliveryInfo(inDealerCode, _
            '                              inStoreCode, _
            '                              inOrderNo, _
            '                              chipArea)
            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
            'RO作業連番情報の取得
            Dim dtJobDetailSequenceInfo As JobDetailSequenceInfoDataTable = _
                Me.GetSMBJobDetailSequenceInfo(drSmbChipDetailInputInfo.VISIT_SEQ, _
                                               drSmbChipDetailInputInfo.SVCIN_ID, _
                                               drSmbChipDetailInputInfo.JOB_DTL_ID, _
                                               drSmbChipDetailInputInfo.RO_NUM, _
                                               daSMBCommonClass)
            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

            'ステータス判定を取得する
            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
            'Dim statusWord As String = Me.GetStatusSMB(inVisitType, _
            '                                           inWorkStartDate, _
            '                                           inWorkEndDate, _
            '                                           inWashType, _
            '                                           inOrderNo, _
            '                                           inCompleteExaminationType, _
            '                                           dtChipDetailProcess, _
            '                                           drOrderStatus, _
            '                                           dtAddRepairStatus, _
            '                                           drReserveROStatusList, _
            '                                           inStallUseStatus, _
            '                                           inSequenceNo, _
            '                                           inDeliveryDate)

            Dim statusWord As String = Me.GetStatusSMB(drSmbChipDetailInputInfo.DLR_CD, _
                                                       drSmbChipDetailInputInfo.BRN_CD, _
                                                       drSmbChipDetailInputInfo.CHIP_AREA_TYPE, _
                                                       drSmbChipDetailInputInfo.VISIT_SEQ, _
                                                       drSmbChipDetailInputInfo.RO_TYPE, _
                                                       drSmbChipDetailInputInfo.RO_NUM, _
                                                       drSmbChipDetailInputInfo.RSLT_START_DATETIME, _
                                                       drSmbChipDetailInputInfo.STALL_USE_STATUS, _
                                                       drSmbChipDetailInputInfo.RSLT_END_DATETIME, _
                                                       drSmbChipDetailInputInfo.INSPECTION_STATUS, _
                                                       drSmbChipDetailInputInfo.SVC_STATUS, _
                                                       drSmbChipDetailInputInfo.INVOICE_PRINT_DATETIME, _
                                                       dtJobDetailSequenceInfo)
            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

            '「表示区分 <> 1:受付、0:エラー」の場合は納車見込時刻を取得する
            Dim deliveryDate As String = DEFAULT_TIME

            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
            'If chipArea <> DisplayType.Invalid AndAlso chipArea <> DisplayType.Err Then
            '    deliveryDate = Me.GetDeliveryHopeDate(inDealerCode, _
            '                                          inStoreCode, _
            '                                          chipArea, _
            '                                          dtChipDetailProcess, _
            '                                          drOrderStatus, _
            '                                          nowDate)
            'End If

            If chipArea <> DisplayType.Invalid AndAlso chipArea <> DisplayType.Err Then
                deliveryDate = Me.GetDeliveryHopeDate(drSmbChipDetailInputInfo.DLR_CD, _
                                                      drSmbChipDetailInputInfo.BRN_CD, _
                                                      chipArea, _
                                                      dtChipDetailProcess, _
                                                      dtChipDetailRepairOrderInfo, _
                                                      nowDate)
            End If
            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

            'Returnのデータ作成
            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
            'returnData = Me.SetSMBChipDetail(statusWord, _
            '                                 deliveryDate, _
            '                                 dtChipDetailReserve, _
            '                                 dtStop, _
            '                                 dtHistoryDeliveryDateList, _
            '                                 drOrderCommon,
            '                                 dtFaultReasonInfo)

            returnData = Me.SetSMBChipDetail(statusWord, _
                                             deliveryDate, _
                                             dtStop, _
                                             dtChipDetailReserve)
            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:RETURN = {2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , ReturnCode.Success))
            Return returnData
            'DBタイムアウト
        Catch ex As OracleExceptionEx When ex.Number = 1013
            Logger.Error(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:RETURN = {2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , ReturnCode.ErrDBTimeout))
            Return Nothing
        Finally
            If daSMBCommonClass IsNot Nothing Then daSMBCommonClass.Dispose()
        End Try
    End Function

#End Region

#Region "Privateメソッド"

    ''' <summary>
    ''' チップ詳細情報データ格納(来店)
    ''' </summary>
    ''' <param name="inStatus">ステータス</param>
    ''' <param name="inChipArea">表示区分</param>
    ''' <param name="inDeliveryDate">納車見込時刻</param>
    ''' <param name="inNowDate">現在日時</param>
    ''' <param name="inPartsStatus">部品準備ステータス</param>
    ''' <param name="dtChipDetailVisit">チップ詳細情報(来店)</param>
    ''' <param name="dtChipDetailReserve">チップ詳細情報(予約)</param>
    ''' <param name="dtChipDetailProcess">チップ詳細情報(実績)</param>
    ''' <param name="dtChipDetailCustomerInfo">顧客情報</param>
    ''' <param name="dtStop">中断理由List</param>
    ''' <param name="dtChipDetailRepairOrderInfo">ステータス情報</param>
    ''' <returns>チップ詳細情報</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発
    ''' </history>
    Private Function SetVisitData(ByVal inStatus() As String, _
                                  ByVal inChipArea As DisplayType, _
                                  ByVal inDeliveryDate As String, _
                                  ByVal inNowDate As DateTime, _
                                  ByVal inPartsStatus As String, _
                                  ByVal dtChipDetailVisit As ChipDetailVisitDataTable, _
                                  ByVal dtChipDetailReserve As ChipDetailReserveDataTable, _
                                  ByVal dtChipDetailProcess As ChipDetailProcessDataTable, _
                                  ByVal dtChipDetailCustomerInfo As ChipDetailCustomerInfoDataTable, _
                                  ByVal dtStop As StopDataTable, _
                                  ByVal dtChipDetailRepairOrderInfo As ChipDetailRepairOrderInfoDataTable) As ChipDetail

        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
        'Private Function SetVisitData(ByVal inStatus() As String, _
        '                              ByVal inChipArea As DisplayType, _
        '                              ByVal inDeliveryDate As String, _
        '                              ByVal inNowDate As DateTime, _
        '                              ByVal dtChipDetailVisit As ChipDetailVisitDataTable, _
        '                              ByVal dtChipDetailReserve As ChipDetailReserveDataTable, _
        '                              ByVal dtChipDetailProcess As ChipDetailProcessDataTable, _
        '                              ByVal dtSrvCustomer As IC3800703SrvCustomerDataTable, _
        '                              ByVal dtStop As StopDataTable, _
        '                              ByVal dtHistoryDeliveryDateList As IC3801701DataSet.HistoryDeliveryDateListDataTable, _
        '                              ByVal drOrderStatus As IC3801901DataSet.OrderStatusDataRow) As ChipDetail
        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} " _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim returnChipDetail As ChipDetail = Nothing
        If inChipArea = DisplayType.Invalid Then

            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
            'returnChipDetail = Me.SetChipDetailVisitInvalid(inStatus, _
            '                                                inChipArea, _
            '                                                inNowDate, _
            '                                                dtChipDetailVisit, _
            '                                                dtChipDetailReserve, _
            '                                                dtSrvCustomer, _
            '                                                drOrderStatus)

            returnChipDetail = Me.SetChipDetailVisitInvalid(inStatus, _
                                                            inChipArea, _
                                                            inNowDate, _
                                                            inPartsStatus, _
                                                            dtChipDetailVisit, _
                                                            dtChipDetailReserve, _
                                                            dtChipDetailCustomerInfo, _
                                                            dtChipDetailRepairOrderInfo)
            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        Else

            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
            'returnChipDetail = Me.SetChipDetailVisitNotInvalid(inStatus, _
            '                                                   inChipArea, _
            '                                                   inDeliveryDate, _
            '                                                   inNowDate, _
            '                                                   dtChipDetailVisit, _
            '                                                   dtChipDetailReserve, _
            '                                                   dtChipDetailProcess, _
            '                                                   dtSrvCustomer, _
            '                                                   dtStop, _
            '                                                   dtHistoryDeliveryDateList, _
            '                                                   drOrderStatus)

            returnChipDetail = Me.SetChipDetailVisitNotInvalid(inStatus, _
                                                               inChipArea, _
                                                               inDeliveryDate, _
                                                               inNowDate, _
                                                               inPartsStatus, _
                                                               dtChipDetailVisit, _
                                                               dtChipDetailReserve, _
                                                               dtChipDetailProcess, _
                                                               dtChipDetailCustomerInfo, _
                                                               dtStop, _
                                                               dtChipDetailRepairOrderInfo)
            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} OUT:RETURN = {2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , "ChipDetail"))
        Return returnChipDetail
    End Function

    ''' <summary>
    ''' チップ詳細情報データ格納(来店：受付)
    ''' </summary>
    ''' <param name="inStatus">ステータス</param>
    ''' <param name="inChipArea">表示区分</param>
    ''' <param name="inNowDate">現在日時</param>
    ''' <param name="inPartsStatus">部品準備ステータス</param>
    ''' <param name="dtChipDetailVisit">チップ詳細情報(来店)</param>
    ''' <param name="dtChipDetailReserve">チップ詳細情報(予約)</param>
    ''' <param name="dtChipDetailCustomerInfo">顧客情報</param>
    ''' <param name="dtChipDetailRepairOrderInfo">RO情報</param>
    ''' <returns>チップ詳細情報</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発
    ''' 2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発
    ''' </history>
    Private Function SetChipDetailVisitInvalid(ByVal inStatus() As String, _
                                               ByVal inChipArea As DisplayType, _
                                               ByVal inNowDate As DateTime, _
                                               ByVal inPartsStatus As String, _
                                               ByVal dtChipDetailVisit As ChipDetailVisitDataTable, _
                                               ByVal dtChipDetailReserve As ChipDetailReserveDataTable, _
                                               ByVal dtChipDetailCustomerInfo As ChipDetailCustomerInfoDataTable, _
                                               ByVal dtChipDetailRepairOrderInfo As ChipDetailRepairOrderInfoDataTable) As ChipDetail

        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
        'Private Function SetChipDetailVisitInvalid(ByVal inStatus() As String, _
        '                                           ByVal inChipArea As DisplayType, _
        '                                           ByVal inNowDate As DateTime, _
        '                                           ByVal dtChipDetailVisit As ChipDetailVisitDataTable, _
        '                                           ByVal dtChipDetailReserve As ChipDetailReserveDataTable, _
        '                                           ByVal dtSrvCustomer As IC3800703SrvCustomerDataTable, _
        '                                           ByVal drOrderStatus As IC3801901DataSet.OrderStatusDataRow) As ChipDetail
        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} " _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'チップ詳細情報
        Dim returnData As New ChipDetail

        'チップ詳細情報(来店)
        Dim drChipDetailVisit As ChipDetailVisitRow = _
            DirectCast(dtChipDetailVisit.Rows(0), ChipDetailVisitRow)
        'チップ詳細情報(予約)
        Dim drChipDetailReserve As ChipDetailReserveRow = Nothing
        If dtChipDetailReserve IsNot Nothing AndAlso 0 < dtChipDetailReserve.Count Then
            drChipDetailReserve = DirectCast(dtChipDetailReserve.Rows(0), ChipDetailReserveRow)

        End If

        returnData.Status = inStatus(0)                                                     'ステータス
        returnData.DeliveryHopeDate = DEFAULT_TIME                                          '納車見込時刻
        returnData.PartsPreparationWaitType = inPartsStatus                                 '部品準備待ちフラグ
        If drChipDetailReserve IsNot Nothing AndAlso _
           Not (String.IsNullOrEmpty(drChipDetailReserve.REZ_DELI_DATE)) Then               '納車予定時刻
            Dim deliveryPlanDate As DateTime = _
                DateTimeFunc.FormatString(FORMAT_DATE, drChipDetailReserve.REZ_DELI_DATE)

            returnData.DeliveryPlanDate = _
                SetDateTimeToString(inNowDate, deliveryPlanDate)

        Else
            returnData.DeliveryPlanDate = DEFAULT_TIME

        End If

        returnData.DeliveryPlanDateUpdateCount = 0                                          '納車予定時刻変更回数

        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
        'returnData.VehicleRegNo = drChipDetailVisit.VCLREGNO                                '車両登録No
        'returnData.CustomerName = drChipDetailVisit.NAME                                    '顧客名
        'returnData.TelNo = drChipDetailVisit.TELNO                                          '電話番号
        'returnData.Mobile = drChipDetailVisit.MOBILE                                        '携帯電話番号

        returnData.VehicleRegNo = Me.checkStringRowData(drChipDetailVisit, "VCLREGNO")      '車両登録No
        returnData.CustomerName = Me.checkStringRowData(drChipDetailVisit, "NAME")          '顧客名
        returnData.TelNo = Me.checkStringRowData(drChipDetailVisit, "TELNO")                '電話番号
        returnData.Mobile = Me.checkStringRowData(drChipDetailVisit, "MOBILE")              '携帯電話番号
        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
        ''2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　START
        'returnData.UpdateDate = drChipDetailVisit.UPDATEDATE                                '更新日
        'returnData.CallNO = drChipDetailVisit.CALLNO                                        '呼出No.
        'returnData.CallPlace = drChipDetailVisit.CALLPLACE                                  '呼出場所
        'returnData.CallStatus = drChipDetailVisit.CALLSTATUS                                '呼出ステータス
        'returnData.VisitName = drChipDetailVisit.VISITNAME                                  '来店者氏名
        'returnData.VisitTelNO = drChipDetailVisit.VISITTELNO                                '来店者電話番号
        ''2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　END

        returnData.UpdateDate = Me.checkDateRowData(drChipDetailVisit, "UPDATEDATE")        '更新日
        returnData.CallNO = Me.checkStringRowData(drChipDetailVisit, "CALLNO")              '呼出No.
        returnData.CallPlace = Me.checkStringRowData(drChipDetailVisit, "CALLPLACE")        '呼出場所
        returnData.CallStatus = Me.checkStringRowData(drChipDetailVisit, "CALLSTATUS")      '呼出ステータス
        returnData.VisitName = Me.checkStringRowData(drChipDetailVisit, "VISITNAME")        '来店者氏名
        returnData.VisitTelNO = Me.checkStringRowData(drChipDetailVisit, "VISITTELNO")      '来店者電話番号
        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START
        returnData.NameCustomerType = Me.checkStringRowData(drChipDetailVisit, "CUSTSEGMENT")   '顧客区分
        '顧客情報が取得できない場合、"1"(所有者)を設定
        returnData.CustomerVehicleType = "1"                                                    '顧客車両区分
        '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END

        If drChipDetailReserve IsNot Nothing Then

            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
            'returnData.MerchandiseName = drChipDetailReserve.MERCHANDISENAME_VISIT         '整備内容

            returnData.MerchandiseName = Me.checkStringRowData(drChipDetailReserve, "MERCHANDISENAME")                '整備内容
            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
            'returnData.ReserveReception = drChipDetailReserve.REZ_RECEPTION                 '待ち方
            'returnData.WalkIn = drChipDetailReserve.WALKIN                                  '予約マーク
            'returnData.VehicleName = drChipDetailReserve.VEHICLENAME                        '車種

            returnData.ReserveReception = Me.checkStringRowData(drChipDetailReserve, "REZ_RECEPTION")                 '待ち方
            returnData.WalkIn = Me.checkStringRowData(drChipDetailReserve, "WALKIN")                                  '予約マーク
            returnData.VehicleName = Me.checkStringRowData(drChipDetailReserve, "VEHICLENAME")                        '車種
            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
            returnData.ServiceinLockVersion = drChipDetailReserve.ROW_LOCK_VERSION          'サービス入庫テーブル行ロックバージョン
            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

            '2016/06/09 NSK 皆川 TR-V4-TMT-20160107-001 START
            '同一顧客ID・車両IDの場合でも予約取得時の顧客車両区分が表示されるようにするため、サービス入庫．顧客車両区分を設定するように修正
            returnData.CustomerVehicleType = Me.checkStringRowData(drChipDetailReserve, "CST_VCL_TYPE")               '顧客車両区分
            '2016/06/09 NSK 皆川 TR-V4-TMT-20160107-001 END

        Else
            returnData.MerchandiseName = Nothing
            returnData.ReserveReception = Nothing
            returnData.WalkIn = Nothing
            returnData.VehicleName = Nothing
            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
            returnData.ServiceinLockVersion = -1
            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        End If

        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
        'If dtSrvCustomer IsNot Nothing AndAlso 0 < dtSrvCustomer.Count Then
        '    '顧客参照
        '    Dim drSrvCustomer As IC3800703SrvCustomerFRow = Nothing
        '    drSrvCustomer = DirectCast(dtSrvCustomer.Rows(0), IC3800703SrvCustomerFRow)

        '    returnData.Grade = Me.checkStringRowData(drSrvCustomer, "GRADE")                'グレード
        '    returnData.JdpType = Me.checkStringRowData(drSrvCustomer, "JDPFLAG")            'JDPマーク
        '    returnData.SscType = Me.checkStringRowData(drSrvCustomer, "SSCFLAG")            'SSCマーク
        '    returnData.Vin = Me.checkStringRowData(drSrvCustomer, "VINNO")                  'VIN
        '    returnData.Model = Me.checkStringRowData(drSrvCustomer, "MODEL")                'モデル

        '    '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
        '    '2012/08/16 TMEJ 日比野 STEP2(車種は顧客参照情報を優先して表示するように修正) START
        '    'returnData.VehicleName = Me.checkStringRowData(drSrvCustomer, "VHCNAME")        '車種
        '    '2012/08/16 TMEJ 日比野 STEP2(車種は顧客参照情報を優先して表示するように修正) END
        '    '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

        '    '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
        '    '顧客情報が取得できた場合はこちらを優先に設定する
        '    returnData.VehicleRegNo = Me.checkStringRowData(drSrvCustomer, "REGISTERNO")    '車両登録No
        '    If Not (drSrvCustomer.IsBUYERNAMENull) Then
        '        returnData.CustomerName = Me.checkStringRowData(drSrvCustomer, "BUYERNAME") '顧客名
        '    End If
        '    If Not (drSrvCustomer.IsBUYERTEL1Null) Then
        '        returnData.TelNo = Me.checkStringRowData(drSrvCustomer, "BUYERTEL1")        '電話番号
        '    End If
        '    If Not (drSrvCustomer.IsBUYERTEL2Null) Then
        '        returnData.Mobile = Me.checkStringRowData(drSrvCustomer, "BUYERTEL2")       '携帯電話番号
        '    End If
        '    If Not (drSrvCustomer.IsVHCNAMENull) Then
        '        returnData.VehicleName = Me.checkStringRowData(drSrvCustomer, "VHCNAME")    '車種
        '    End If
        '    '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END
        'Else
        '    returnData.Grade = Nothing
        '    returnData.JdpType = Nothing
        '    returnData.SscType = Nothing
        '    returnData.Vin = Nothing
        '    returnData.Model = Nothing
        'End If

        If dtChipDetailCustomerInfo IsNot Nothing AndAlso 0 < dtChipDetailCustomerInfo.Count Then
            '顧客参照
            returnData.Grade = Me.checkStringRowData(dtChipDetailCustomerInfo(0), "GRADE_NAME")                           'グレード
            returnData.JdpType = Me.checkStringRowData(dtChipDetailCustomerInfo(0), "VIP_FLG")                            'JDPマーク
            returnData.Vin = Me.checkStringRowData(dtChipDetailCustomerInfo(0), "VCL_VIN")                                'VIN
            returnData.Model = Me.checkStringRowData(dtChipDetailCustomerInfo(0), "MODEL_CD")                             'モデル
            returnData.RegisterAreaName = _
                Me.checkStringRowData(dtChipDetailCustomerInfo(0), "REG_AREA_NAME")                                       '車両登録エリア名称

            '顧客情報が取得できた場合はこちらを優先に設定する
            returnData.VehicleRegNo = Me.checkStringRowData(dtChipDetailCustomerInfo(0), "REG_NUM")                       '車両登録No
            If Not (dtChipDetailCustomerInfo(0).IsCST_NAMENull) Then
                returnData.CustomerName = Me.checkStringRowData(dtChipDetailCustomerInfo(0), "CST_NAME")                  '顧客名

            End If
            If Not (dtChipDetailCustomerInfo(0).IsCST_PHONENull) Then
                returnData.TelNo = Me.checkStringRowData(dtChipDetailCustomerInfo(0), "CST_PHONE")                        '電話番号

            End If
            If Not (dtChipDetailCustomerInfo(0).IsCST_MOBILENull) Then
                returnData.Mobile = Me.checkStringRowData(dtChipDetailCustomerInfo(0), "CST_MOBILE")                      '携帯電話番号

            End If
            If Not (dtChipDetailCustomerInfo(0).IsMODEL_NAMENull) Then
                returnData.VehicleName = Me.checkStringRowData(dtChipDetailCustomerInfo(0), "MODEL_NAME")                 '車種
            End If

            '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START

            '2016/06/09 NSK 皆川 TR-V4-TMT-20160107-001 START
            '同一顧客ID・車両IDの場合でも予約取得時の顧客車両区分が表示されるようにするため、予約が存在しない場合のみ顧客車両区分を設定するように修正
            'If Not (dtChipDetailCustomerInfo(0).IsCST_VCL_TYPENull) Then
            If drChipDetailReserve Is Nothing AndAlso Not (dtChipDetailCustomerInfo(0).IsCST_VCL_TYPENull) Then
            '2016/06/09 NSK 皆川 TR-V4-TMT-20160107-001 END
                returnData.CustomerVehicleType = Me.checkStringRowData(dtChipDetailCustomerInfo(0), "CST_VCL_TYPE")       '顧客車種区分

            End If
            If Not (dtChipDetailCustomerInfo(0).IsCST_TYPENull) Then
                returnData.NameCustomerType = Me.checkStringRowData(dtChipDetailCustomerInfo(0), "CST_TYPE")             '顧客種別

            End If

            '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END

            '2018/02/22 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 START
            returnData.SscType = Me.checkStringRowData(dtChipDetailCustomerInfo(0), "SSC_MARK")                          'SSCアイコン
            '2018/02/22 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 END

            '2018/07/06 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
            returnData.JdpType = Me.checkStringRowData(dtChipDetailCustomerInfo(0), "IMP_VCL_FLG")                          'P/Lアイコン
            '2018/07/06 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END

        Else
            returnData.Grade = Nothing
            returnData.JdpType = Nothing
            returnData.SscType = Nothing
            returnData.Vin = Nothing
            returnData.Model = Nothing

        End If
        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        '中断理由は設定しない
        '納車予定時刻変更は設定しない
        returnData.VisitType = "1"                                                          '来店実績有無
        returnData.DisplayType = inChipArea                                                 '表示区分
        returnData.CustomerType = drChipDetailVisit.CUSTSEGMENT                             '顧客区分
        returnData.WorkStartType = Nothing                                                  '作業開始有無
        returnData.StopType = Nothing                                                       '中断有無
        returnData.WashType = Nothing                                                       '洗車有無
        returnData.RemainingWorkTime = 0                                                    '残作業時間(分)
        returnData.WorkEndPlanDateLast = Nothing                                            '作業終了予定時刻(最終)
        returnData.WashStartDate = Nothing                                                  '洗車開始時刻
        returnData.WashEndDate = Nothing                                                    '洗車終了時刻

        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
        'If drOrderStatus IsNot Nothing Then
        '    'ステータス情報
        '    returnData.OrderDataType = "1"                                                  'R/O有無
        '    returnData.OrderStatus = drOrderStatus.ORDERSTATUS                              'R/Oステータス
        '    returnData.PartsPreparationWaitType = _
        '        Me.checkStringRowData(drOrderStatus, "PARTSREPAREFLAG")                     '部品準備待ちフラグ
        '    returnData.CompleteExaminationType = _
        '        Me.checkStringRowData(drOrderStatus, "INSPECTIONAPPROVALFLAG")              '完成検査フラグ
        'Else
        '    returnData.OrderDataType = "0"
        '    returnData.OrderStatus = Nothing
        '    returnData.PartsPreparationWaitType = Nothing
        '    returnData.CompleteExaminationType = Nothing
        'End If

        If dtChipDetailRepairOrderInfo IsNot Nothing AndAlso 0 < dtChipDetailRepairOrderInfo.Count Then
            'ステータス情報
            returnData.OrderDataType = "1"                                                  'R/O有無
            returnData.OrderStatus = dtChipDetailRepairOrderInfo(0).RO_STATUS               'R/Oステータス
            returnData.CompleteExaminationType = _
                Me.checkStringRowData(dtChipDetailRepairOrderInfo(0), "INSPECTION_STATUS")  '完成検査フラグ

        Else
            returnData.OrderDataType = "0"
            returnData.OrderStatus = Nothing
            returnData.CompleteExaminationType = Nothing

        End If
        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        returnData.AddWorkStatus = Nothing                                                  '追加作業ステータス
        returnData.ReissueVouchers = Nothing                                                '起票者
        returnData.CompleteExaminationEndDate = Nothing                                     '完成検査完了時刻
        returnData.StatementPrintDate = Nothing                                             '清算書印刷時刻
        returnData.StatusLeft = inStatus(1)                                                 'ステータスコード(左)
        returnData.StatusRight = inStatus(2)                                                'ステータスコード(右)
        returnData.AddAccountName = Nothing                                                 '追加作業起票者名

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} OUT:RETURN = {2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , "ChipDetail"))
        Return returnData
    End Function

    ''' <summary>
    ''' チップ詳細情報データ格納(来店：受付以外)
    ''' </summary>
    ''' <param name="inStatus">ステータス</param>
    ''' <param name="inChipArea">表示区分</param>
    ''' <param name="inDeliveryDate">納車見込時刻</param>
    ''' <param name="inNowDate">現在日時</param>
    ''' <param name="inPartsStatus">部品準備ステータス</param>
    ''' <param name="dtChipDetailVisit">チップ詳細情報(来店)</param>
    ''' <param name="dtChipDetailReserve">チップ詳細情報(予約)</param>
    ''' <param name="dtChipDetailProcess">チップ詳細情報(実績)</param>
    ''' <param name="dtChipDetailCustomerInfo">顧客情報</param>
    ''' <param name="dtStop">中断理由List</param>
    ''' <param name="dtChipDetailRepairOrderInfo">RO情報</param>
    ''' <returns>チップ詳細情報</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発
    ''' 2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発
    ''' 2019/07/02 NSK 鈴木 [TKM]PUAT-4100-1 SAメインでチップとチップ詳細の項目に差異がある
    ''' </history>
    Private Function SetChipDetailVisitNotInvalid(ByVal inStatus() As String, _
                                                  ByVal inChipArea As DisplayType, _
                                                  ByVal inDeliveryDate As String, _
                                                  ByVal inNowDate As DateTime, _
                                                  ByVal inPartsStatus As String, _
                                                  ByVal dtChipDetailVisit As ChipDetailVisitDataTable, _
                                                  ByVal dtChipDetailReserve As ChipDetailReserveDataTable, _
                                                  ByVal dtChipDetailProcess As ChipDetailProcessDataTable, _
                                                  ByVal dtChipDetailCustomerInfo As ChipDetailCustomerInfoDataTable, _
                                                  ByVal dtStop As StopDataTable, _
                                                  ByVal dtChipDetailRepairOrderInfo As ChipDetailRepairOrderInfoDataTable) As ChipDetail

        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
        'Private Function SetChipDetailVisitNotInvalid(ByVal inStatus() As String, _
        '                                              ByVal inChipArea As DisplayType, _
        '                                              ByVal inDeliveryDate As String, _
        '                                              ByVal inNowDate As DateTime, _
        '                                              ByVal dtChipDetailVisit As ChipDetailVisitDataTable, _
        '                                              ByVal dtChipDetailReserve As ChipDetailReserveDataTable, _
        '                                              ByVal dtChipDetailProcess As ChipDetailProcessDataTable, _
        '                                              ByVal dtSrvCustomer As IC3800703SrvCustomerDataTable, _
        '                                              ByVal dtStop As StopDataTable, _
        '                                              ByVal dtHistoryDeliveryDateList As IC3801701DataSet.HistoryDeliveryDateListDataTable, _
        '                                              ByVal drOrderStatus As IC3801901DataSet.OrderStatusDataRow) As ChipDetail
        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} " _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'チップ詳細情報
        Dim returnData As New ChipDetail

        'チップ詳細情報(来店)
        Dim drChipDetailVisit As ChipDetailVisitRow = _
            DirectCast(dtChipDetailVisit.Rows(0), ChipDetailVisitRow)

        returnData.Status = inStatus(0)                                                             'ステータス
        returnData.DeliveryHopeDate = SetDateStringToString(inNowDate, inDeliveryDate)              '納車見込時刻
        returnData.PartsPreparationWaitType = inPartsStatus                                         '部品準備待ちフラグ

        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
        'If drOrderStatus IsNot Nothing Then
        '    'ステータス情報
        '    If Not (drOrderStatus.IsDELIVERYHOPEDATENull) Then                                      '納車予定時刻
        '        returnData.DeliveryPlanDate = _
        '            SetDateStringToString(inNowDate, drOrderStatus.DELIVERYHOPEDATE)
        '    Else
        '        returnData.DeliveryPlanDate = DEFAULT_TIME
        '    End If
        '    returnData.OrderDataType = "1"                                                          'R/O有無
        '    returnData.OrderStatus = drOrderStatus.ORDERSTATUS                                      'R/Oステータス
        '    returnData.PartsPreparationWaitType = _
        '        Me.checkStringRowData(drOrderStatus, "PARTSREPAREFLAG")                             '部品準備待ちフラグ
        '    returnData.CompleteExaminationType = _
        '        Me.checkStringRowData(drOrderStatus, "INSPECTIONAPPROVALFLAG")                      '完成検査フラグ
        '    returnData.AddWorkStatus = Me.checkStringRowData(drOrderStatus, "ADDStatus")            '追加作業ステータス
        '    returnData.ReissueVouchers = Me.checkStringRowData(drOrderStatus, "DRAWER")             '起票者
        '    returnData.CompleteExaminationEndDate = _
        '        Me.checkDateRowData(drOrderStatus, "EXAMINETIME")                                   '完成検査完了時刻
        '    returnData.StatementPrintDate = Me.checkDateRowData(drOrderStatus, "COLSINGPRINTTIME")  '清算書印刷時刻
        '    '2012/09/21 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.35） START
        '    '追加作業起票アカウントがある場合は名前を取得する
        '    If Not (drOrderStatus.IsADDACCOUNTNull) Then
        '        Dim userInfo As UsersDataSet.USERSRow = (New Users).GetUser(drOrderStatus.ADDACCOUNT)
        '        If Not (IsNothing(userInfo)) Then
        '            returnData.AddAccountName = userInfo.USERNAME                                   '追加作業起票者名
        '        Else
        '            returnData.AddAccountName = Nothing
        '        End If
        '    Else
        '        returnData.AddAccountName = Nothing
        '    End If
        '    '2012/09/21 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.35） END

        'Else
        '    returnData.OrderDataType = "0"
        '    returnData.OrderStatus = Nothing
        '    returnData.PartsPreparationWaitType = Nothing
        '    returnData.CompleteExaminationType = Nothing
        '    returnData.AddWorkStatus = Nothing
        '    returnData.ReissueVouchers = Nothing
        '    returnData.CompleteExaminationEndDate = Nothing
        '    returnData.StatementPrintDate = Nothing
        '    '2012/09/21 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.35） START
        '    returnData.AddAccountName = Nothing
        '    '2012/09/21 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.35） END
        'End If

        If dtChipDetailRepairOrderInfo IsNot Nothing AndAlso 0 < dtChipDetailRepairOrderInfo.Count Then
            'ステータス情報
            If Not (dtChipDetailRepairOrderInfo(0).IsSCHE_DELI_DATETIMENull) Then                   '納車予定時刻
                returnData.DeliveryPlanDate = _
                    SetDateStringToString(inNowDate, _
                                          dtChipDetailRepairOrderInfo(0).SCHE_DELI_DATETIME)

            Else
                returnData.DeliveryPlanDate = DEFAULT_TIME

            End If
            returnData.OrderDataType = "1"                                                          'R/O有無
            returnData.OrderStatus = dtChipDetailRepairOrderInfo(0).RO_STATUS                       'R/Oステータス
            returnData.CompleteExaminationType = _
                Me.checkStringRowData(dtChipDetailRepairOrderInfo(0), "INSPECTION_STATUS")          '完成検査フラグ
            returnData.AddWorkStatus = _
                Me.checkStringRowData(dtChipDetailRepairOrderInfo(0), "ADD_RO_STATUS")              '追加作業ステータス
            returnData.ReissueVouchers = _
                Me.checkStringRowData(dtChipDetailRepairOrderInfo(0), "DRAWER")                     '起票者
            returnData.CompleteExaminationEndDate = _
                Me.checkDateRowData(dtChipDetailRepairOrderInfo(0), "INSPECTION_APPROVAL_DATETIME") '完成検査完了時刻
            returnData.StatementPrintDate = _
                Me.checkDateRowData(dtChipDetailRepairOrderInfo(0), "INVOICE_PRINT_DATETIME")       '清算書印刷時刻

            '追加作業起票アカウントがある場合は名前を取得する
            ' 2019/07/02 NSK 鈴木 [TKM]PUAT-4100-1 SAメインでチップとチップ詳細の項目に差異がある START
            'If Not (dtChipDetailRepairOrderInfo(0).IsRO_CREATE_STF_CDNull) Then
            '    Dim userInfo As UsersDataSet.USERSRow = _
            '        (New Users).GetUser(dtChipDetailRepairOrderInfo(0).RO_CREATE_STF_CD)
            '
            '    If Not (IsNothing(userInfo)) Then
            '        returnData.AddAccountName = userInfo.USERNAME                                   '追加作業起票者名
            '
            '    Else
            '        returnData.AddAccountName = Nothing
            '
            '    End If
            '
            'Else
            '    returnData.AddAccountName = Nothing
            '
            'End If
            Using tableAdapter As New SMBCommonClassTableAdapter
                ' SA承認待ち（ROステータス：35）のRO情報取得
                Dim dtRoInfosConfirmationWait As ChipDetailRepairOrderInfoDataTable = _
                    tableAdapter.GetAddRepairOrderInfo(drChipDetailVisit.VISITSEQ)

                ' 上記RO情報が存在する場合、1件目の「RO作成スタッフコード」から追加作業起票者名を取得する。
                If dtRoInfosConfirmationWait IsNot Nothing AndAlso 0 < dtRoInfosConfirmationWait.Count Then
                    If Not (dtRoInfosConfirmationWait(0).IsRO_CREATE_STF_CDNull) Then
                        Dim userInfo As UsersDataSet.USERSRow = _
                            (New Users).GetUser(dtRoInfosConfirmationWait(0).RO_CREATE_STF_CD)
                        If Not (IsNothing(userInfo)) Then
                            returnData.AddAccountName = userInfo.USERNAME                               '追加作業起票者名
                        Else
                            returnData.AddAccountName = Nothing
                        End If
                    Else
                        returnData.AddAccountName = Nothing
                    End If

                    ' SA承認待ち（ROステータス：35）の追加作業が存在しない場合、チップ詳細情報(来店)から追加作業起票者名を取得する。
                Else
                    If Not (dtChipDetailRepairOrderInfo(0).IsRO_CREATE_STF_CDNull) Then
                        Dim userInfo As UsersDataSet.USERSRow = _
                            (New Users).GetUser(dtChipDetailRepairOrderInfo(0).RO_CREATE_STF_CD)

                        If Not (IsNothing(userInfo)) Then
                            returnData.AddAccountName = userInfo.USERNAME                               '追加作業起票者名

                        Else
                            returnData.AddAccountName = Nothing

                        End If

                    Else
                        returnData.AddAccountName = Nothing

                    End If
                End If
            End Using
            ' 2019/07/02 NSK 鈴木 [TKM]PUAT-4100-1 SAメインでチップとチップ詳細の項目に差異がある END

        Else
            returnData.OrderDataType = "0"
            returnData.OrderStatus = Nothing
            returnData.CompleteExaminationType = Nothing
            returnData.AddWorkStatus = Nothing
            returnData.ReissueVouchers = Nothing
            returnData.CompleteExaminationEndDate = Nothing
            returnData.StatementPrintDate = Nothing
            returnData.AddAccountName = Nothing

        End If
        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
        'If dtHistoryDeliveryDateList IsNot Nothing AndAlso 0 < dtHistoryDeliveryDateList.Count Then '納車予定時刻変更回数
        '    returnData.DeliveryPlanDateUpdateCount = dtHistoryDeliveryDateList.Count
        'Else
        '    returnData.DeliveryPlanDateUpdateCount = 0
        'End If
        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
        ''2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
        'returnData.VehicleRegNo = drChipDetailVisit.VCLREGNO                                        '車両登録No
        'returnData.CustomerName = drChipDetailVisit.NAME                                            '顧客名
        'returnData.TelNo = drChipDetailVisit.TELNO                                                  '電話番号
        'returnData.Mobile = drChipDetailVisit.MOBILE                                                '携帯電話番号
        ''2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

        returnData.VehicleRegNo = Me.checkStringRowData(drChipDetailVisit, "VCLREGNO")                '車両登録No
        returnData.CustomerName = Me.checkStringRowData(drChipDetailVisit, "NAME")                    '顧客名
        returnData.TelNo = Me.checkStringRowData(drChipDetailVisit, "TELNO")                          '電話番号
        returnData.Mobile = Me.checkStringRowData(drChipDetailVisit, "MOBILE")                        '携帯電話番号
        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START
        returnData.NameCustomerType = Me.checkStringRowData(drChipDetailVisit, "CUSTSEGMENT")         '顧客区分
        '顧客情報が取得できない場合、"1"(所有者)を設定　
        returnData.CustomerVehicleType = "1"                                                          '顧客車両区分
        '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END

        If dtChipDetailReserve IsNot Nothing AndAlso 0 < dtChipDetailReserve.Count Then
            'チップ詳細情報(予約)
            Dim drChipDetailReserve As ChipDetailReserveRow = _
                DirectCast(dtChipDetailReserve.Rows(0), ChipDetailReserveRow)

            '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
            'returnData.VehicleRegNo = drChipDetailReserve.VCLREGNO                                  '車両登録No
            'returnData.CustomerName = drChipDetailReserve.CUSTOMERNAME                              '顧客名
            'returnData.TelNo = drChipDetailReserve.TELNO                                            '電話番号
            'returnData.Mobile = drChipDetailReserve.MOBILE                                          '携帯電話番号
            '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
            'returnData.MerchandiseName = drChipDetailReserve.MERCHANDISENAME_VISIT                  '整備内容

            returnData.MerchandiseName = Me.checkStringRowData(drChipDetailReserve, "MERCHANDISENAME")  '整備内容
            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
            'returnData.ReserveReception = Me.checkStringRowData(drChipDetailReserve, "REZ_RECEPTION")   '待ち方
            'returnData.WalkIn = Me.checkStringRowData(drChipDetailReserve, "WALKIN")                    '予約マーク
            'returnData.VehicleName = Me.checkStringRowData(drChipDetailReserve, "VEHICLENAME")          '車種

            returnData.ReserveReception = drChipDetailReserve.REZ_RECEPTION                         '待ち方
            returnData.WalkIn = drChipDetailReserve.WALKIN                                          '予約マーク
            returnData.VehicleName = drChipDetailReserve.VEHICLENAME                                '車種

            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
            returnData.ServiceinLockVersion = drChipDetailReserve.ROW_LOCK_VERSION                      'サービス入庫テーブル行ロックバージョン
            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

            '2016/06/09 NSK 皆川 TR-V4-TMT-20160107-001 START
            '同一顧客ID・車両IDの場合でも予約取得時の顧客車両区分が表示されるようにするため、サービス入庫．顧客車両区分を設定するように修正
            returnData.CustomerVehicleType = Me.checkStringRowData(drChipDetailReserve, "CST_VCL_TYPE")               '顧客車両区分
            '2016/06/09 NSK 皆川 TR-V4-TMT-20160107-001 END

        Else
            '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
            'returnData.VehicleRegNo = Nothing
            'returnData.CustomerName = Nothing
            'returnData.TelNo = Nothing
            'returnData.Mobile = Nothing
            '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

            returnData.MerchandiseName = Nothing
            returnData.ReserveReception = Nothing
            returnData.WalkIn = Nothing
            returnData.VehicleName = Nothing

            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
            returnData.ServiceinLockVersion = -1
            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        End If

        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
        'If dtSrvCustomer IsNot Nothing AndAlso 0 < dtSrvCustomer.Count Then
        '    '顧客参照
        '    Dim drSrvCustomer As IC3800703SrvCustomerFRow = _
        '        DirectCast(dtSrvCustomer.Rows(0), IC3800703SrvCustomerFRow)

        '    returnData.Grade = Me.checkStringRowData(drSrvCustomer, "GRADE")                        'グレード
        '    returnData.JdpType = Me.checkStringRowData(drSrvCustomer, "JDPFLAG")                    'JDPマーク
        '    returnData.SscType = Me.checkStringRowData(drSrvCustomer, "SSCFLAG")                    'SSCマーク
        '    returnData.Vin = Me.checkStringRowData(drSrvCustomer, "VINNO")                          'VIN
        '    returnData.Model = Me.checkStringRowData(drSrvCustomer, "MODEL")                        'モデル

        '    '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
        '    '顧客情報が取得できた場合はこちらを優先に設定する
        '    returnData.VehicleRegNo = Me.checkStringRowData(drSrvCustomer, "REGISTERNO")            '車両登録No
        '    If Not (drSrvCustomer.IsBUYERNAMENull) Then
        '        returnData.CustomerName = Me.checkStringRowData(drSrvCustomer, "BUYERNAME")         '顧客名
        '    End If
        '    If Not (drSrvCustomer.IsBUYERTEL1Null) Then
        '        returnData.TelNo = Me.checkStringRowData(drSrvCustomer, "BUYERTEL1")                '電話番号
        '    End If
        '    If Not (drSrvCustomer.IsBUYERTEL2Null) Then
        '        returnData.Mobile = Me.checkStringRowData(drSrvCustomer, "BUYERTEL2")               '携帯電話番号
        '    End If
        '    If Not (drSrvCustomer.IsVHCNAMENull) Then
        '        returnData.VehicleName = Me.checkStringRowData(drSrvCustomer, "VHCNAME")            '車種
        '    End If
        '    '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END
        'Else
        '    returnData.Grade = Nothing
        '    returnData.JdpType = Nothing
        '    returnData.SscType = Nothing
        '    returnData.Vin = Nothing
        '    returnData.Model = Nothing
        'End If

        If dtChipDetailCustomerInfo IsNot Nothing AndAlso 0 < dtChipDetailCustomerInfo.Count Then
            '顧客参照

            returnData.Grade = Me.checkStringRowData(dtChipDetailCustomerInfo(0), "GRADE_NAME")                           'グレード
            returnData.JdpType = Me.checkStringRowData(dtChipDetailCustomerInfo(0), "VIP_FLG")                            'JDPマーク
            returnData.Vin = Me.checkStringRowData(dtChipDetailCustomerInfo(0), "VCL_VIN")                                'VIN
            returnData.Model = Me.checkStringRowData(dtChipDetailCustomerInfo(0), "MODEL_CD")                             'モデル
            returnData.RegisterAreaName = Me.checkStringRowData(dtChipDetailCustomerInfo(0), "REG_AREA_NAME")             '車両登録エリア名称

            '顧客情報が取得できた場合はこちらを優先に設定する
            returnData.VehicleRegNo = Me.checkStringRowData(dtChipDetailCustomerInfo(0), "REG_NUM")                       '車両登録No
            If Not (dtChipDetailCustomerInfo(0).IsCST_NAMENull) Then
                returnData.CustomerName = Me.checkStringRowData(dtChipDetailCustomerInfo(0), "CST_NAME")                  '顧客名

            End If
            If Not (dtChipDetailCustomerInfo(0).IsCST_PHONENull) Then
                returnData.TelNo = Me.checkStringRowData(dtChipDetailCustomerInfo(0), "CST_PHONE")                        '電話番号

            End If
            If Not (dtChipDetailCustomerInfo(0).IsCST_MOBILENull) Then
                returnData.Mobile = Me.checkStringRowData(dtChipDetailCustomerInfo(0), "CST_MOBILE")                      '携帯電話番号

            End If
            If Not (dtChipDetailCustomerInfo(0).IsMODEL_NAMENull) Then
                returnData.VehicleName = Me.checkStringRowData(dtChipDetailCustomerInfo(0), "MODEL_NAME")                 '車種

            End If

            '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START

            '2016/06/09 NSK 皆川 TR-V4-TMT-20160107-001 START
            '同一顧客ID・車両IDの場合でも予約取得時の顧客車両区分が表示されるようにするため、予約が存在しない場合のみ顧客車両区分を設定するように修正
            'If Not (dtChipDetailCustomerInfo(0).IsCST_VCL_TYPENull) Then
            If dtChipDetailReserve Is Nothing AndAlso Not (dtChipDetailCustomerInfo(0).IsCST_VCL_TYPENull) Then
            '2016/06/09 NSK 皆川 TR-V4-TMT-20160107-001 END
                returnData.CustomerVehicleType = Me.checkStringRowData(dtChipDetailCustomerInfo(0), "CST_VCL_TYPE")       '顧客車両区分

            End If
            If Not (dtChipDetailCustomerInfo(0).IsCST_TYPENull) Then
                returnData.NameCustomerType = Me.checkStringRowData(dtChipDetailCustomerInfo(0), "CST_TYPE")              '顧客種別

            End If

            '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END

            '2018/02/22 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 START
            returnData.SscType = Me.checkStringRowData(dtChipDetailCustomerInfo(0), "SSC_MARK")                          'SSCアイコン
            '2018/02/22 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 END

            '2018/07/06 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
            returnData.JdpType = Me.checkStringRowData(dtChipDetailCustomerInfo(0), "IMP_VCL_FLG")                          'P/Lアイコン
            '2018/07/06 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END

        Else
            returnData.Grade = Nothing
            returnData.JdpType = Nothing
            returnData.SscType = Nothing
            returnData.Vin = Nothing
            returnData.Model = Nothing

        End If
        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        Me.setStopReasonList(returnData.StopReasonList, dtStop)                                     '中断理由

        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
        'Me.setDeliveryChgList(returnData.DeliveryChgList, dtHistoryDeliveryDateList)                '納車予定時刻変更
        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        returnData.VisitType = "1"                                                                  '来店実績有無
        returnData.DisplayType = inChipArea                                                         '表示区分

        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
        'returnData.CustomerType = drChipDetailVisit.CUSTSEGMENT                                     '顧客区分

        returnData.CustomerType = Me.checkStringRowData(drChipDetailVisit, "CUSTSEGMENT")           '顧客区分

        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        If dtChipDetailProcess IsNot Nothing AndAlso 0 < dtChipDetailProcess.Count Then
            'チップ詳細情報(実績)
            Dim drChipDetailProcess As ChipDetailProcessRow = _
                DirectCast(dtChipDetailProcess.Rows(0), ChipDetailProcessRow)

            If drChipDetailProcess.IsSTARTTIMENull Then                                             '作業開始有無
                returnData.WorkStartType = "0"

            Else
                returnData.WorkStartType = "1"

            End If
            If drChipDetailProcess.STOPCOUNT = 0 Then                                               '中断有無
                returnData.StopType = "0"

            Else
                returnData.StopType = "1"

            End If
            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
            'returnData.WashType = drChipDetailProcess.WASHFLG                                       '洗車有無

            returnData.WashType = Me.checkStringRowData(drChipDetailProcess, "WASHFLG")             '洗車有無
            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

            returnData.RemainingWorkTime = drChipDetailProcess.WORKTIME                             '残作業時間(分)

            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
            'returnData.WorkEndPlanDateLast = drChipDetailProcess.ENDTIME                            '作業終了予定時刻(最終)

            returnData.WorkEndPlanDateLast = Me.checkDateRowData(drChipDetailProcess, "ENDTIME")    '作業終了予定時刻(最終)
            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

            If Not (drChipDetailProcess.IsRESULT_WASH_STARTNull) Then                               '洗車開始時刻
                returnData.WashStartDate = _
                    DateTimeFunc.FormatString(FORMAT_DATE, drChipDetailProcess.RESULT_WASH_START)

            End If
            If Not (drChipDetailProcess.IsRESULT_WASH_ENDNull) Then                                 '洗車終了時刻
                returnData.WashEndDate = _
                    DateTimeFunc.FormatString(FORMAT_DATE, drChipDetailProcess.RESULT_WASH_END)

            End If
        Else
            returnData.WorkStartType = "0"
            returnData.StopType = "0"
            returnData.WashType = "0"
            returnData.RemainingWorkTime = 0
            returnData.WorkEndPlanDateLast = Nothing
            returnData.WashStartDate = Nothing
            returnData.WashEndDate = Nothing

        End If
        returnData.StatusLeft = inStatus(1)                                                         'ステータスコード(左)
        returnData.StatusRight = inStatus(2)                                                        'ステータスコード(右)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} OUT:RETURN = {2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , "ChipDetail"))
        Return returnData
    End Function

    '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
    ' ''' <summary>
    ' ''' チップ詳細情報データ格納(予約)
    ' ''' </summary>
    ' ''' <param name="inStatus">ステータス</param>
    ' ''' <param name="inNowDate">現在日時</param>
    ' ''' <param name="dtChipDetailReserve">チップ詳細情報(予約)</param>
    ' ''' <param name="dtSrvCustomer">顧客参照</param>
    ' ''' <param name="drOrderStatus">ステータス情報取得</param>
    ' ''' <returns>チップ詳細情報</returns>
    ' ''' <remarks></remarks>
    ' ''' <history>
    ' ''' 2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発
    ' ''' </history>
    'Private Function SetReserveData(ByVal inStatus() As String, _
    '                                ByVal inNowDate As DateTime, _
    '                                ByVal dtChipDetailReserve As ChipDetailReserveDataTable, _
    '                                ByVal dtSrvCustomer As IC3800703SrvCustomerDataTable, _
    '                                ByVal drOrderStatus As IC3801901DataSet.OrderStatusDataRow) As ChipDetail
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                   , "{0}.{1} " _
    '                   , Me.GetType.ToString _
    '                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

    '    'チップ詳細情報
    '    Dim returnData As New ChipDetail

    '    'ストール予約
    '    Dim drChipDetailReserve As ChipDetailReserveRow = _
    '        DirectCast(dtChipDetailReserve.Rows(0), ChipDetailReserveRow)

    '    returnData.Status = inStatus(0)                                                     'ステータス
    '    returnData.DeliveryHopeDate = DEFAULT_TIME                                          '納車見込時刻
    '    If Not (String.IsNullOrEmpty(drChipDetailReserve.REZ_DELI_DATE)) Then               '納車予定時刻
    '        Dim deliveryPlanDate As DateTime = _
    '            DateTimeFunc.FormatString(FORMAT_DATE, drChipDetailReserve.REZ_DELI_DATE)
    '        returnData.DeliveryPlanDate = _
    '            SetDateTimeToString(inNowDate, deliveryPlanDate)
    '    Else
    '        returnData.DeliveryPlanDate = DEFAULT_TIME
    '    End If
    '    returnData.DeliveryPlanDateUpdateCount = 0                                          '納車予定時刻変更回数

    '    returnData.VehicleRegNo = drChipDetailReserve.VCLREGNO                              '車両登録No
    '    returnData.CustomerName = drChipDetailReserve.CUSTOMERNAME                          '顧客名
    '    returnData.TelNo = drChipDetailReserve.TELNO                                        '電話番号
    '    returnData.Mobile = drChipDetailReserve.MOBILE                                      '携帯電話番号
    '    returnData.MerchandiseName = drChipDetailReserve.MERCHANDISENAME_RESERVE            '整備内容
    '    returnData.ReserveReception = drChipDetailReserve.REZ_RECEPTION                     '待ち方
    '    returnData.WalkIn = drChipDetailReserve.WALKIN                                      '予約マーク
    '    returnData.VehicleName = drChipDetailReserve.VEHICLENAME                            '車種
    '    If dtSrvCustomer IsNot Nothing AndAlso 0 < dtSrvCustomer.Count Then
    '        '顧客参照
    '        Dim drSrvCustomer As IC3800703SrvCustomerFRow = _
    '            DirectCast(dtSrvCustomer.Rows(0), IC3800703SrvCustomerFRow)

    '        returnData.Grade = Me.checkStringRowData(drSrvCustomer, "GRADE")                'グレード
    '        returnData.JdpType = Me.checkStringRowData(drSrvCustomer, "JDPFLAG")            'JDPマーク
    '        returnData.SscType = Me.checkStringRowData(drSrvCustomer, "SSCFLAG")            'SSCマーク
    '        returnData.Vin = Me.checkStringRowData(drSrvCustomer, "VINNO")                  'VIN
    '        returnData.Model = Me.checkStringRowData(drSrvCustomer, "MODEL")                'モデル

    '        '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
    '        '顧客情報が取得できた場合はこちらを優先に設定する
    '        returnData.VehicleRegNo = Me.checkStringRowData(drSrvCustomer, "REGISTERNO")    '車両登録No
    '        If Not (drSrvCustomer.IsBUYERNAMENull) Then
    '            returnData.CustomerName = Me.checkStringRowData(drSrvCustomer, "BUYERNAME") '顧客名
    '        End If
    '        If Not (drSrvCustomer.IsBUYERTEL1Null) Then
    '            returnData.TelNo = Me.checkStringRowData(drSrvCustomer, "BUYERTEL1")        '電話番号
    '        End If
    '        If Not (drSrvCustomer.IsBUYERTEL2Null) Then
    '            returnData.Mobile = Me.checkStringRowData(drSrvCustomer, "BUYERTEL2")       '携帯電話番号
    '        End If
    '        If Not (drSrvCustomer.IsVHCNAMENull) Then
    '            returnData.VehicleName = Me.checkStringRowData(drSrvCustomer, "VHCNAME")    '車種
    '        End If
    '        '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

    '    Else
    '        returnData.Grade = Nothing
    '        returnData.JdpType = Nothing
    '        returnData.SscType = Nothing
    '        returnData.Vin = Nothing
    '        returnData.Model = Nothing
    '    End If
    '    '中断理由は設定しない
    '    '納車予定時刻変更は設定しない
    '    returnData.VisitType = "0"                                                          '来店実績有無
    '    returnData.DisplayType = 0                                                          '表示区分
    '    returnData.CustomerType = drChipDetailReserve.CUSTOMERFLAG                          '顧客区分
    '    returnData.WorkStartType = Nothing                                                  '作業開始有無
    '    returnData.StopType = Nothing                                                       '中断有無
    '    returnData.WashType = Nothing                                                       '洗車有無
    '    returnData.RemainingWorkTime = 0                                                    '残作業時間(分)
    '    returnData.WorkEndPlanDateLast = Nothing                                            '作業終了予定時刻(最終)
    '    returnData.WashStartDate = Nothing                                                  '洗車開始時刻
    '    returnData.WashEndDate = Nothing                                                    '洗車終了時刻

    '    'ステータス情報がある場合はデータを格納する
    '    If drOrderStatus IsNot Nothing Then
    '        'ステータス情報
    '        returnData.OrderDataType = "1"                                                  'R/O有無
    '        returnData.OrderStatus = drOrderStatus.ORDERSTATUS                              'R/Oステータス
    '    Else
    '        returnData.OrderDataType = "0"
    '        returnData.OrderStatus = Nothing
    '    End If
    '    returnData.PartsPreparationWaitType = Nothing                                       '部品準備待ちフラグ
    '    returnData.CompleteExaminationType = Nothing                                        '完成検査フラグ
    '    returnData.AddWorkStatus = Nothing                                                  '追加作業ステータス
    '    returnData.ReissueVouchers = Nothing                                                '起票者
    '    returnData.CompleteExaminationEndDate = Nothing                                     '完成検査完了時刻
    '    returnData.StatementPrintDate = Nothing                                             '清算書印刷時刻
    '    returnData.StatusLeft = inStatus(1)                                                 'ステータスコード(左)
    '    returnData.StatusRight = inStatus(2)                                                'ステータスコード(右)

    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                , "{0}.{1} OUT:RETURN = {2}" _
    '                , Me.GetType.ToString _
    '                , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                , "ChipDetail"))
    '    Return returnData
    'End Function
    '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

    '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START

    ''' <summary>
    ''' チップ詳細情報データ格納(SMB)
    ''' </summary>
    ''' <param name="inStatusWord">ステータス</param>
    ''' <param name="inDeliveryDate">納車見込時刻</param>
    ''' <param name="dtStop">中断理由List</param>
    ''' <param name="dtChipDetailReserve">予約情報</param>
    ''' <returns>チップ詳細情報</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発
    ''' 2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発
    ''' </history>
    Private Function SetSMBChipDetail(ByVal inStatusWord As String, _
                                      ByVal inDeliveryDate As String, _
                                      ByVal dtStop As StopDataTable, _
                                      ByVal dtChipDetailReserve As ChipDetailReserveDataTable) As ChipDetail

        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
        'Private Function SetSMBChipDetail(ByVal inStatusWord As String, _
        '                                  ByVal inDeliveryDate As String, _
        '                                  ByVal dtChipDetailReserve As ChipDetailReserveDataTable, _
        '                                  ByVal dtStop As StopDataTable, _
        '                                  ByVal dtHistoryDeliveryDateList As IC3801701DataSet.HistoryDeliveryDateListDataTable, _
        '                                  ByVal drOrderCommon As IC3801001DataSet.IC3801001OrderCommRow, _
        '                                  ByVal dtFaultReasonInfo As IC3801014DataSet.IC3801014FaultReasonInfoDataTable) As ChipDetai
        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} " _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'チップ詳細情報
        Dim returnData As New ChipDetail

        returnData.Status = inStatusWord                                                                    'ステータス

        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
        'If 0 < dtChipDetailReserve.Count Then
        '    returnData.CustomerType = dtChipDetailReserve(0).CUSTOMERFLAG
        'Else
        '    returnData.CustomerType = String.Empty
        'End If
        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        If Not (DEFAULT_TIME.Equals(inDeliveryDate)) Then                                                   '納車見込時刻_日付型
            returnData.DeliveryHopeDateTime = Date.Parse(inDeliveryDate, CultureInfo.CurrentCulture)

        Else
            returnData.DeliveryHopeDateTime = Date.MinValue

        End If

        Me.setStopReasonList(returnData.StopReasonList, dtStop)                                             '中断理由

        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
        'Me.setDeliveryChgList(returnData.DeliveryChgList, dtHistoryDeliveryDateList)
        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        '納車時刻変更履歴
        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
        'If dtHistoryDeliveryDateList IsNot Nothing AndAlso 0 < dtHistoryDeliveryDateList.Count Then
        '    returnData.DeliveryPlanDateUpdateCount = dtHistoryDeliveryDateList.Count
        'Else
        '    returnData.DeliveryPlanDateUpdateCount = 0
        'End If

        returnData.DeliveryPlanDateUpdateCount = 0                                                          '納車予定時刻変更回数
        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        'RO基本情報
        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
        'If drOrderCommon IsNot Nothing Then
        '    If Not (drOrderCommon.IsDeliveryHopeDateNull) Then                                              '納車予定時刻_日付型
        '        returnData.DeliveryPlanDateTime = Date.Parse(drOrderCommon.DeliveryHopeDate, CultureInfo.CurrentCulture)
        '    Else
        '        returnData.DeliveryPlanDateTime = Date.MinValue
        '    End If

        '    returnData.OrderMemo = Me.checkStringRowData(drOrderCommon, "orderMemo")                        'ご用命
        '    returnData.WorkResultAdvice = Me.checkStringRowData(drOrderCommon, "twcResult")                 '作業結果及びアドバイス
        'Else
        '    returnData.DeliveryPlanDateTime = Date.MinValue
        '    returnData.JdpType = Nothing
        '    returnData.SscType = Nothing
        '    returnData.OrderMemo = Nothing
        '    returnData.FailureCause = Nothing
        '    returnData.DiagnosticResult = Nothing
        '    returnData.WorkResultAdvice = Nothing
        'End If

        If dtChipDetailReserve IsNot Nothing AndAlso 0 < dtChipDetailReserve.Count Then
            If Not (dtChipDetailReserve(0).IsREZ_DELI_DATENull) Then                                              '納車予定時刻_日付型
                returnData.DeliveryPlanDateTime = _
                    DateTimeFunc.FormatString(FORMAT_DATE, dtChipDetailReserve(0).REZ_DELI_DATE)

            Else
                returnData.DeliveryPlanDateTime = Date.MinValue

            End If

            '2016/06/09 NSK 皆川 TR-V4-TMT-20160107-001 START
            returnData.CustomerVehicleType = Me.checkStringRowData(dtChipDetailReserve(0), "CST_VCL_TYPE")
            '2016/06/09 NSK 皆川 TR-V4-TMT-20160107-001 END

        Else
            returnData.DeliveryPlanDateTime = Date.MinValue

        End If
        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
        ''故障原因、診断結果
        'If Not (IsNothing(dtFaultReasonInfo)) AndAlso 0 < dtFaultReasonInfo.Count Then
        '    returnData.FailureCause = Me.checkStringRowData(dtFaultReasonInfo(0), "faultReason")
        '    returnData.DiagnosticResult = Me.checkStringRowData(dtFaultReasonInfo(0), "diagnosisResult")
        'Else
        '    returnData.FailureCause = Nothing
        '    returnData.DiagnosticResult = Nothing
        'End If
        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} OUT:RETURN = {2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , "ChipDetail"))
        Return returnData
    End Function

    '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

    ''' <summary>
    ''' ステータス判定取得処理(来店)
    ''' </summary>
    ''' <param name="drChipDetailVisit">来店情報</param>
    ''' <param name="dtChipDetailReserve">チップ詳細情報(予約)</param>
    ''' <param name="dtChipDetailProcess">チップ詳細情報(実績)</param>
    ''' <param name="dtChipDetailRepairOrderInfo">RO情報</param>
    ''' <param name="inPartsStatus">部品ステータス</param>
    ''' <returns>ステータス</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2012/11/02 TMEJ 小澤 【SERVICE_2】次世代サービスROステータス切り離し対応
    ''' 2012/11/28 TMEJ 小澤 問連「GTMC121126088」対応
    ''' 2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発
    ''' 2019/06/14 NSK 鈴木 [TKM]PUAT-4100 連続で追加作業起票するとRO発行ボタンが押せなくなる
    ''' </history>
    Private Function GetStatusVisit(ByVal drChipDetailVisit As ChipDetailVisitRow, _
                                    ByVal dtChipDetailReserve As ChipDetailReserveDataTable, _
                                    ByVal dtChipDetailProcess As ChipDetailProcessDataTable, _
                                    ByVal dtChipDetailRepairOrderInfo As ChipDetailRepairOrderInfoDataTable, _
                                    ByVal inPartsStatus As String) As String()

        '2012/11/28 TMEJ 小澤 問連「GTMC121126088」対応 START
        'Private Function GetStatusVisit(ByVal inCustomerType As String, _
        '                                ByVal dtChipDetailProcess As ChipDetailProcessDataTable, _
        '                                ByVal drOrderStatus As IC3801901DataSet.OrderStatusDataRow) As String()
        '2012/11/28 TMEJ 小澤 問連「GTMC121126088」対応 END

        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
        'Private Function GetStatusVisit(ByVal drChipDetailVisit As ChipDetailVisitRow, _
        '                                ByVal dtChipDetailProcess As ChipDetailProcessDataTable, _
        '                                ByVal drOrderStatus As IC3801901DataSet.OrderStatusDataRow) As String()
        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} " _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim returnStatus() As String
        Dim visitType As String = "1"                                       '来店実績有無
        '2012/11/28 TMEJ 小澤 問連「GTMC121126088」対応 START
        'Dim customerType As String = inCustomerType
        Dim customerType As String = drChipDetailVisit.CUSTSEGMENT          '顧客区分
        '2012/11/28 TMEJ 小澤 問連「GTMC121126088」対応 END

        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
        Dim assignStatus As String = drChipDetailVisit.ASSIGNSTATUS         '振当ステータス

        Dim serviceStatus As String = Nothing                               'サービスステータス

        '予約情報チェック
        If Not (IsNothing(dtChipDetailReserve)) AndAlso 0 < dtChipDetailReserve.Count Then
            '存在する場合
            'サービスステータスを設定
            serviceStatus = Me.checkStringRowData(dtChipDetailReserve(0), "SVC_STATUS")

        End If


        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        'チップ詳細情報(実績)がある場合はデータを格納
        Dim workStartType As String = Nothing                               '作業開始有無
        Dim stopType As String = Nothing                                    '中断有無
        Dim washType As String = Nothing                                    '洗車有無
        ' 2012/11/02 TMEJ 小澤 【SERVICE_2】次世代サービスROステータス切り離し対応 START
        'Dim resultStatus As String = Nothing                                '実績ステータス
        ' 2012/11/02 TMEJ 小澤 【SERVICE_2】次世代サービスROステータス切り離し対応 END
        Dim instruct As String = Nothing                                    '着工指示区分
        Dim resultWashStart As String = Nothing                             '洗車開始実績日時
        Dim resultWashEnd As String = Nothing                               '洗車終了実績日時

        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
        '2012/11/28 TMEJ 小澤 問連「GTMC121126088」対応 START
        'Dim dtAddRepairStatus As IC3800804AddRepairStatusDataTableDataTable = Nothing
        '2012/11/28 TMEJ 小澤 問連「GTMC121126088」対応 END
        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        If dtChipDetailProcess IsNot Nothing AndAlso 0 < dtChipDetailProcess.Count Then
            'チップ詳細情報(実績)
            Dim drChipDetailProcess As ChipDetailProcessRow
            drChipDetailProcess = DirectCast(dtChipDetailProcess.Rows(0), ChipDetailProcessRow)

            If drChipDetailProcess.IsSTARTTIMENull Then
                workStartType = "0"

            Else
                workStartType = "1"

            End If
            If drChipDetailProcess.STOPCOUNT = 0 Then
                stopType = "0"

            Else
                stopType = "1"

            End If
            washType = drChipDetailProcess.WASHFLG
            ' 2012/11/02 TMEJ 小澤 【SERVICE_2】次世代サービスROステータス切り離し対応 START
            'If drChipDetailProcess.INDEXNUMBER <> 0 Then
            '    resultStatus = drChipDetailProcess.RESULT_STATUS
            'End If
            ' 2012/11/02 TMEJ 小澤 【SERVICE_2】次世代サービスROステータス切り離し対応 END
            instruct = Me.checkStringRowData(drChipDetailProcess, "INSTRUCT")
            resultWashStart = drChipDetailProcess.RESULT_WASH_START
            resultWashEnd = drChipDetailProcess.RESULT_WASH_END

        Else
            workStartType = "0"
            stopType = "0"
            washType = "0"
            resultWashStart = String.Empty
            resultWashEnd = String.Empty

        End If

        'ステータス情報がある場合はデータを格納
        Dim orderDataType As String = Nothing                               'R/O有無
        Dim orderStatus As String = Nothing                                 'R/Oステータス

        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
        'Dim partsPreparationWaitType As String = Nothing
        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        Dim completeExaminationType As String = Nothing                     '完成検査フラグ
        Dim addWorkStatus As String = Nothing                               '追加作業ステータス
        Dim reissueVouchers As String = Nothing                             '起票者

        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
        Dim workEndType As String = Nothing                                 '作業終了有無
        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
        'If drOrderStatus IsNot Nothing Then
        '    'ステータス情報
        '    orderDataType = "1"
        '    orderStatus = drOrderStatus.ORDERSTATUS
        '    partsPreparationWaitType = Me.checkStringRowData(drOrderStatus, "PARTSREPAREFLAG")
        '    completeExaminationType = Me.checkStringRowData(drOrderStatus, "INSPECTIONAPPROVALFLAG")
        '    addWorkStatus = Me.checkStringRowData(drOrderStatus, "ADDStatus")
        '    reissueVouchers = Me.checkStringRowData(drOrderStatus, "DRAWER")

        '    '2012/11/28 TMEJ 小澤 問連「GTMC121126088」対応 START
        '    If ROFinInspection.Equals(orderStatus) Then
        '        Dim bizAddRepairStatusList As New IC3800804BusinessLogic
        '        dtAddRepairStatus = _
        '            DirectCast(bizAddRepairStatusList.GetAddRepairStatusList(drChipDetailVisit.DLRCD, drChipDetailVisit.ORDERNO),  _
        '                IC3800804AddRepairStatusDataTableDataTable)
        '    End If
        '    '2012/11/28 TMEJ 小澤 問連「GTMC121126088」対応 END
        'Else
        '    orderDataType = "0"
        'End If

        'RO情報チェック
        If dtChipDetailRepairOrderInfo IsNot Nothing AndAlso 0 < dtChipDetailRepairOrderInfo.Count Then
            '存在する場合
            orderDataType = "1"
            orderStatus = Me.checkStringRowData(dtChipDetailRepairOrderInfo(0), "RO_STATUS")
            completeExaminationType = Me.checkStringRowData(dtChipDetailRepairOrderInfo(0), "INSPECTION_STATUS")
            addWorkStatus = Me.checkStringRowData(dtChipDetailRepairOrderInfo(0), "ADD_RO_STATUS")
            reissueVouchers = Me.checkStringRowData(dtChipDetailRepairOrderInfo(0), "DRAWER")
            workEndType = Me.checkStringRowData(dtChipDetailRepairOrderInfo(0), "WORK_END_TYPE")

            ' 2019/06/14 NSK 鈴木 [TKM]PUAT-4100 連続で追加作業起票するとRO発行ボタンが押せなくなる START
            ' RO_SEQが最小のSA承認待ち（ROステータス：35）RO情報から「追加作業ステータス」「起票者」を取得し、ローカル変数に更新する。
            Using tableAdapter As New SMBCommonClassTableAdapter
                ' SA承認待ち（ROステータス：35）のRO情報取得
                Dim dtRoInfosConfirmationWait As ChipDetailRepairOrderInfoDataTable _
                    = tableAdapter.GetAddRepairOrderInfo(drChipDetailVisit.VISITSEQ)

                ' 上記RO情報が存在する場合、1件目の「追加作業ステータス」「起票者」を取得し、ローカル変数に更新する。
                If dtRoInfosConfirmationWait IsNot Nothing AndAlso 0 < dtRoInfosConfirmationWait.Count Then
                    addWorkStatus = Me.checkStringRowData(dtRoInfosConfirmationWait(0), "ADD_RO_STATUS")    ' 追加作業ステータス
                    reissueVouchers = Me.checkStringRowData(dtRoInfosConfirmationWait(0), "DRAWER")         ' 起票者
                End If
            End Using
            ' 2019/06/14 NSK 鈴木 [TKM]PUAT-4100 連続で追加作業起票するとRO発行ボタンが押せなくなる END

        Else
            '存在しない場合
            orderDataType = "0"

        End If
        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        'ステータス判定取得
        ' 2012/11/02 TMEJ 小澤 【SERVICE_2】次世代サービスROステータス切り離し対応 START
        'returnStatus = Me.GetChipDetailStatus(visitType, _
        '                                      customerType, _
        '                                      workStartType, _
        '                                      stopType, _
        '                                      washType, _
        '                                      resultStatus, _
        '                                      orderDataType, _
        '                                      orderStatus, _
        '                                      partsPreparationWaitType, _
        '                                      completeExaminationType, _
        '                                      addWorkStatus, _
        '                                      reissueVouchers, _
        '                                      instruct, _
        '                                      resultWashStart, _
        '                                      resultWashEnd)
        '2012/11/28 TMEJ 小澤 問連「GTMC121126088」対応 START
        'returnStatus = Me.GetChipDetailStatus(visitType, _
        '                                      customerType, _
        '                                      workStartType, _
        '                                      stopType, _
        '                                      washType, _
        '                                      orderDataType, _
        '                                      orderStatus, _
        '                                      partsPreparationWaitType, _
        '                                      completeExaminationType, _
        '                                      addWorkStatus, _
        '                                      reissueVouchers, _
        '                                      instruct, _
        '                                      resultWashStart, _
        '                                      resultWashEnd)

        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
        'returnStatus = Me.GetChipDetailStatus(visitType, _
        '                                      customerType, _
        '                                      workStartType, _
        '                                      stopType, _
        '                                      washType, _
        '                                      orderDataType, _
        '                                      orderStatus, _
        '                                      partsPreparationWaitType, _
        '                                      completeExaminationType, _
        '                                      addWorkStatus, _
        '                                      reissueVouchers, _
        '                                      instruct, _
        '                                      resultWashStart, _
        '                                      resultWashEnd, _
        '                                      dtAddRepairStatus)

        returnStatus = Me.GetChipDetailStatus(visitType, _
                                              assignStatus, _
                                              customerType, _
                                              workStartType, _
                                              stopType, _
                                              washType, _
                                              orderDataType, _
                                              orderStatus, _
                                              inPartsStatus, _
                                              completeExaminationType, _
                                              addWorkStatus, _
                                              reissueVouchers, _
                                              instruct, _
                                              resultWashStart, _
                                              resultWashEnd, _
                                              workEndType, _
                                              serviceStatus)
        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END
        '2012/11/28 TMEJ 小澤 問連「GTMC121126088」対応 END
        ' 2012/11/02 TMEJ 小澤 【SERVICE_2】次世代サービスROステータス切り離し対応 END

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} OUT:RETURN = {2}, {3}, {4}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , returnStatus(0), returnStatus(1), returnStatus(2)))
        Return returnStatus
    End Function

    '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
    ' ''' <summary>
    ' ''' ステータス判定取得処理(予約)
    ' ''' </summary>
    ' ''' <param name="inCustomerType">顧客区分</param>
    ' ''' <param name="drOrderStatus">ステータス情報</param>
    ' ''' <returns>ステータス</returns>
    ' ''' <remarks></remarks>
    'Private Function GetStatusReserve(ByVal inCustomerType As String, _
    '                                  ByVal drOrderStatus As IC3801901DataSet.OrderStatusDataRow) As String()
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                   , "{0}.{1} " _
    '                   , Me.GetType.ToString _
    '                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

    '    Dim returnStatus() As String
    '    Dim visitType As String = "0"                   '来店実績有無
    '    Dim customerType As String = inCustomerType     '顧客区分

    '    'ステータス情報がある場合はデータを格納する
    '    Dim orderDataType As String = Nothing           'R/O有無
    '    Dim orderStatus As String = Nothing             'R/Oステータス
    '    If drOrderStatus IsNot Nothing Then
    '        'ステータス情報
    '        orderDataType = "1"
    '        orderStatus = drOrderStatus.ORDERSTATUS
    '    Else
    '        orderDataType = "0"
    '    End If

    '    'ステータス判定取得
    '    ' 2012/11/02 TMEJ 小澤 【SERVICE_2】次世代サービスROステータス切り離し対応 START
    '    'returnStatus = Me.GetChipDetailStatus(visitType, _
    '    '                                      customerType, _
    '    '                                      Nothing, _
    '    '                                      Nothing, _
    '    '                                      Nothing, _
    '    '                                      Nothing, _
    '    '                                      orderDataType, _
    '    '                                      orderStatus, _
    '    '                                      Nothing, _
    '    '                                      Nothing, _
    '    '                                      Nothing, _
    '    '                                      Nothing, _
    '    '                                      Nothing, _
    '    '                                      Nothing, _
    '    '                                      Nothing)
    '    '2012/11/28 TMEJ 小澤 問連「GTMC121126088」対応 START
    '    'returnStatus = Me.GetChipDetailStatus(visitType, _
    '    '                                      customerType, _
    '    '                                      Nothing, _
    '    '                                      Nothing, _
    '    '                                      Nothing, _
    '    '                                      orderDataType, _
    '    '                                      orderStatus, _
    '    '                                      Nothing, _
    '    '                                      Nothing, _
    '    '                                      Nothing, _
    '    '                                      Nothing, _
    '    '                                      Nothing, _
    '    '                                      Nothing, _
    '    '                                      Nothing)
    '    returnStatus = Me.GetChipDetailStatus(visitType, _
    '                                          customerType, _
    '                                          Nothing, _
    '                                          Nothing, _
    '                                          Nothing, _
    '                                          orderDataType, _
    '                                          orderStatus, _
    '                                          Nothing, _
    '                                          Nothing, _
    '                                          Nothing, _
    '                                          Nothing, _
    '                                          Nothing, _
    '                                          Nothing, _
    '                                          Nothing, _
    '                                          Nothing)
    '    '2012/11/28 TMEJ 小澤 問連「GTMC121126088」対応 END
    '    ' 2012/11/02 TMEJ 小澤 【SERVICE_2】次世代サービスROステータス切り離し対応 END

    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                , "{0}.{1} OUT:RETURN = {2}, {3}, {4}" _
    '                , Me.GetType.ToString _
    '                , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                , returnStatus(0), returnStatus(1), returnStatus(2)))
    '    Return returnStatus
    'End Function
    '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

    ''' <summary>
    ''' 表示区分処理(来店)
    ''' </summary>
    ''' <param name="dtChipDetailRepairOrderInfo">RO情報</param>
    ''' <param name="drChipDetailVisit">来店情報</param>
    ''' <param name="dtChipDetailReserve">予約情報</param>
    ''' <param name="dtChipDetailProcess">実績情報</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <History>
    ''' 2012/11/03 TMEJ 河原 【A.STEP2】次世代サービス ROステータス切り離し対応
    ''' 2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発
    ''' </History>
    Private Function GetDispType(ByVal dtChipDetailRepairOrderInfo As ChipDetailRepairOrderInfoDataTable, _
                                 ByVal drChipDetailVisit As ChipDetailVisitRow, _
                                 ByVal dtChipDetailReserve As ChipDetailReserveDataTable, _
                                 ByVal dtChipDetailProcess As ChipDetailProcessDataTable) As DisplayType

        '2012/09/27 TMEJ 日比野 キャンセルしたチップが表示される不具合対応 START
        'Private Function GetDispType(ByVal dtChipDetailProcess As ChipDetailProcessDataTable, _
        '                             ByVal drOrderStatus As IC3801901DataSet.OrderStatusDataRow, _
        '                             ByVal reserveExistence As String) As DisplayType
        '2012/09/27 TMEJ 日比野 キャンセルしたチップが表示される不具合対応 END

        '2012/11/03 TMEJ 河原 【A.STEP2】次世代サービス ROステータス切り離し対応 START 
        'Private Function GetDispType(ByVal dtChipDetailProcess As ChipDetailProcessDataTable, _
        '                             ByVal drOrderStatus As IC3801901DataSet.OrderStatusDataRow) As DisplayType
        '2012/11/03 TMEJ 河原 【A.STEP2】次世代サービス ROステータス切り離し対応 END

        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
        'Private Function GetDispType(ByVal drOrderStatus As IC3801901DataSet.OrderStatusDataRow, _
        '                             ByVal inChipDetailVisit As ChipDetailVisitRow, _
        '                             ByVal inDTChipDetailReserve As ChipDetailReserveDataTable) As DisplayType
        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} " _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim returnDispType As DisplayType
        'ステータス情報がある場合はデータを格納
        Dim orderDataType As String                'R/O有無
        Dim orderStatus As String                  'R/Oステータス
        '2017/09/28 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
        Dim minOrderStatus As String = String.Empty 'R/Oステータス（最小）
        '2017/09/28 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
        Dim workEndType As String                  '作業終了有無
        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
        'If drOrderStatus IsNot Nothing Then
        '    orderDataType = "1"
        '    orderStatus = drOrderStatus.ORDERSTATUS
        'Else
        '    orderDataType = "0"
        '    orderStatus = Nothing
        'End If

        'R/O有無を格納
        orderDataType = drChipDetailVisit.RO_TYPE

        'RO情報のチェック
        If dtChipDetailRepairOrderInfo IsNot Nothing AndAlso 0 < dtChipDetailRepairOrderInfo.Count Then
            '存在する場合
            'ROステータスを格納
            orderStatus = dtChipDetailRepairOrderInfo(0).RO_STATUS

            '取得したR/Oステータス（最小）を格納
            minOrderStatus = dtChipDetailRepairOrderInfo(0).RO_STATUS_MIN

            '作業完了有無チェック
            If Not (dtChipDetailRepairOrderInfo(0).IsWORK_END_TYPENull) Then
                'データが存在する場合
                'データを設定
                workEndType = dtChipDetailRepairOrderInfo(0).WORK_END_TYPE

            Else
                'データが存在しない場合
                '「0：作業中」を設定
                workEndType = WorkEndTypeWorking

            End If

        Else
            '存在しない場合
            'ROステータスにNULLを格納
            orderStatus = Nothing
            workEndType = Nothing

        End If
        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        '2012/11/03 TMEJ 河原 【A.STEP2】次世代サービス ROステータス切り離し対応 START
        Dim reserveExistence As String              '予約の有無


        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
        Dim serviceStatus As String                 'サービスステータス
        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        If dtChipDetailReserve IsNot Nothing AndAlso 0 < dtChipDetailReserve.Rows.Count Then
            reserveExistence = "1"  '予約の有無:あり

            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
            serviceStatus = dtChipDetailReserve(0).SVC_STATUS
            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        Else
            reserveExistence = "0"  '予約の有無:なし

            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
            serviceStatus = Nothing
            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        End If

        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
        'チップ詳細情報取得(実績)のデータがある場合はデータを格納
        'Dim processStatus As String                '実績ステータス
        'If dtChipDetailProcess IsNot Nothing AndAlso 0 < dtChipDetailProcess.Count Then
        '    Dim drChipDetailProcess As ChipDetailProcessRow
        '    drChipDetailProcess = DirectCast(dtChipDetailProcess.Rows(0), ChipDetailProcessRow)

        '    If drChipDetailProcess.INDEXNUMBER <> 0 Then
        '        processStatus = drChipDetailProcess.RESULT_STATUS
        '    Else
        '        processStatus = Nothing
        '    End If
        'Else
        '    processStatus = Nothing
        'End If

        Dim carWashEndType As String                        '洗車終了有無
        If dtChipDetailProcess IsNot Nothing AndAlso 0 < dtChipDetailProcess.Count Then
            If WashFlag.Equals(dtChipDetailProcess(0).WASHFLG) AndAlso _
               String.IsNullOrEmpty(dtChipDetailProcess(0).RESULT_WASH_END) Then
                carWashEndType = CarWashEndTypeWashing      '洗車中

            Else
                carWashEndType = CarWashEndTypeWashEnd      '洗車終了

            End If
        Else
            carWashEndType = Nothing

        End If
        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
        ''整備受注No,が存在するかチェック
        'If Not inChipDetailVisit.IsORDERNONull AndAlso Not inChipDetailVisit.ORDERNO.Trim = String.Empty Then
        '    'R/O有り
        '    Dim ic3800804Biz As New IC3800804BusinessLogic
        '    '追加作業ステータスの取得
        '    Dim dtAddRepairStatus As IC3800804AddRepairStatusDataTableDataTable = _
        '        DirectCast(ic3800804Biz.GetAddRepairStatusList(inChipDetailVisit.DLRCD, inChipDetailVisit.ORDERNO),  _
        '                   IC3800804AddRepairStatusDataTableDataTable)

        '    '表示区分取得
        '    returnDispType = CType(Me.GetChipArea(orderDataType, orderStatus, reserveExistence, dtAddRepairStatus), DisplayType)

        'Else 'R/O無し
        '    '表示区分取得
        '    returnDispType = CType(Me.GetChipArea(orderDataType, orderStatus, reserveExistence, Nothing), DisplayType)
        'End If
        '2017/09/28 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
        '表示区分取得
        'returnDispType = CType(Me.GetChipArea(orderDataType, _
        '                                      orderStatus, _
        '                                      reserveExistence, _
        '                                      workEndType, _
        '                                      carWashEndType, _
        '                                      serviceStatus), DisplayType)
        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        '2012/11/03 TMEJ 河原 【A.STEP2】次世代サービス ROステータス切り離し対応 END


        '表示区分取得
        returnDispType = CType(Me.GetChipArea(orderDataType, orderStatus, reserveExistence, _
                                              carWashEndType, serviceStatus, minOrderStatus), DisplayType)
        '2017/09/28 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END

        '2012/11/03 TMEJ 河原 【A.STEP2】次世代サービス ROステータス切り離し対応 START
        '2012/09/27 TMEJ 日比野 キャンセルしたチップが表示される不具合対応 START
        '表示区分取得
        'returnDispType = CType(Me.GetChipArea(processStatus, orderDataType, orderStatus), DisplayType)
        'returnDispType = CType(Me.GetChipArea(processStatus, orderDataType, orderStatus, reserveExistence), DisplayType)
        '2012/09/27 TMEJ 日比野 キャンセルしたチップが表示される不具合対応 END
        '2012/11/03 TMEJ 河原 【A.STEP2】次世代サービス ROステータス切り離し対応 END

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} OUT:RETURN = {2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , returnDispType))
        Return returnDispType
    End Function

    ''' <summary>
    ''' 納車見込時刻取得
    ''' </summary>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inStoreCode">店舗コード</param>
    ''' <param name="inChipArea">表示区分</param>
    ''' <param name="dtChipDetailProcess">チップ詳細情報(実績)</param>
    ''' <param name="dtChipDetailRepairOrderInfo">RO情報</param>
    ''' <param name="inNowDate">現在日時</param>
    ''' <returns>納車見込時刻</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発
    ''' </history>
    Private Function GetDeliveryHopeDate(ByVal inDealerCode As String, _
                                         ByVal inStoreCode As String, _
                                         ByVal inChipArea As DisplayType, _
                                         ByVal dtChipDetailProcess As ChipDetailProcessDataTable, _
                                         ByVal dtChipDetailRepairOrderInfo As ChipDetailRepairOrderInfoDataTable,
                                         ByVal inNowDate As DateTime) As String

        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
        'Private Function GetDeliveryHopeDate(ByVal inDealerCode As String, _
        '                                     ByVal inStoreCode As String, _
        '                                     ByVal inChipArea As DisplayType, _
        '                                     ByVal dtChipDetailProcess As ChipDetailProcessDataTable, _
        '                                     ByVal drOrderStatus As IC3801901DataSet.OrderStatusDataRow,
        '                                     ByVal inNowDate As DateTime) As String
        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} " _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'チップ詳細情報(実績)がある場合はデータを格納
        Dim washStartDate As DateTime                           '洗車開始時刻
        Dim washEndDate As DateTime                             '洗車終了時刻
        Dim workEndPlanDateLast As DateTime                     '作業終了予定時刻(最後)
        Dim remainingWorkTime As Long                           '残作業時間
        Dim washType As String                                  '洗車有無

        '2017/09/06 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
        Dim remainingInspectionType As String                   '残完成検査区分
        '2017/09/06 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END

        If dtChipDetailProcess IsNot Nothing AndAlso 0 < dtChipDetailProcess.Count Then
            Dim drChipDetailProcess As ChipDetailProcessRow
            drChipDetailProcess = DirectCast(dtChipDetailProcess.Rows(0), ChipDetailProcessRow)
            If Not (drChipDetailProcess.IsRESULT_WASH_STARTNull) Then
                washStartDate = _
                    DateTimeFunc.FormatString(FORMAT_DATE, drChipDetailProcess.RESULT_WASH_START)
            End If
            If Not (drChipDetailProcess.IsRESULT_WASH_ENDNull) Then
                washEndDate = _
                    DateTimeFunc.FormatString(FORMAT_DATE, drChipDetailProcess.RESULT_WASH_END)
            End If
            workEndPlanDateLast = drChipDetailProcess.ENDTIME
            remainingWorkTime = drChipDetailProcess.WORKTIME
            washType = drChipDetailProcess.WASHFLG

            '2017/09/06 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
            remainingInspectionType = drChipDetailProcess.REMAINING_INSPECTION_TYPE
            '2017/09/06 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
        Else
            washStartDate = Nothing
            washEndDate = Nothing
            workEndPlanDateLast = Nothing
            remainingWorkTime = Nothing
            washType = Nothing

            '2017/09/06 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
            remainingInspectionType = Nothing
            '2017/09/06 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
        End If

        'ステータス情報がある場合はデータを格納
        Dim statementPrintDate As DateTime                      '清算書印刷時刻
        Dim completeExaminationEndDate As DateTime              '完成検査完了時刻

        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
        'If drOrderStatus IsNot Nothing Then
        '    statementPrintDate = Me.checkDateRowData(drOrderStatus, "COLSINGPRINTTIME")
        '    completeExaminationEndDate = Me.checkDateRowData(drOrderStatus, "EXAMINETIME")
        'Else
        '    statementPrintDate = Nothing
        '    completeExaminationEndDate = Nothing
        'End If

        If dtChipDetailRepairOrderInfo IsNot Nothing AndAlso 0 < dtChipDetailRepairOrderInfo.Count Then
            statementPrintDate = Me.checkDateRowData(dtChipDetailRepairOrderInfo(0), "INVOICE_PRINT_DATETIME")
            completeExaminationEndDate = Me.checkDateRowData(dtChipDetailRepairOrderInfo(0), "INSPECTION_APPROVAL_DATETIME")
        Else
            statementPrintDate = Nothing
            completeExaminationEndDate = Nothing
        End If
        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        '納車見込時刻取得
        Try
            '共通関数の初期処理が失敗した場合は「--:--」を返す
            If Me.InitCommon(inDealerCode, inStoreCode, inNowDate) <> 0 Then
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} OUT:RETURN = {2}" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                            , DEFAULT_TIME))
                Return DEFAULT_TIME

            End If

            '2017/09/06 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
            'Dim returnDeliveryDate As DateTime = _
            '    Me.GetDeliveryDate(inChipArea, _
            '                       workEndPlanDateLast, _
            '                       completeExaminationEndDate, _
            '                       washStartDate, _
            '                       washEndDate, _
            '                       statementPrintDate, _
            '                       remainingWorkTime, _
            '                       washType, _
            '                       inNowDate)
            Dim returnDeliveryDate As DateTime = _
                Me.GetDeliveryDate(inChipArea, _
                                   workEndPlanDateLast, _
                                   completeExaminationEndDate, _
                                   washStartDate, _
                                   washEndDate, _
                                   statementPrintDate, _
                                   remainingWorkTime, _
                                   washType, _
                                   inNowDate,
                                   remainingInspectionType)
            '2017/09/06 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:RETURN = {2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , returnDeliveryDate.ToString(CultureInfo.InvariantCulture())))
            Return returnDeliveryDate.ToString(CultureInfo.InvariantCulture())

        Catch ex As Exception
            'エラーになった場合は「--:--」を返す
            Logger.Error(ex.ToString())
            Logger.Error(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:RETURN = {2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , DEFAULT_TIME))
            Return DEFAULT_TIME

        End Try
    End Function

    ''' <summary>
    ''' 中断理由リスト格納処理
    ''' </summary>
    ''' <param name="stopReasonList">中断理由リスト</param>
    ''' <param name="dtStop">中断理由データ</param>
    ''' <remarks></remarks>
    Private Sub setStopReasonList(ByVal stopReasonList As List(Of StopReason), _
                                  ByVal dtStop As StopDataTable)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} " _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name))

        If dtStop IsNot Nothing AndAlso dtStop.Count <> 0 Then
            For Each drStop As StopRow In dtStop
                Dim setStopReason As New StopReason
                setStopReason.ResultEndTime = DateTimeFunc.FormatString(FORMAT_DATE, drStop.RESULT_END_TIME)
                '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
                'setStopReason.ResultStatus = drStop.RESULT_STATUS
                setStopReason.ResultStatus = Me.GetStopReasonWord(drStop.RESULT_STATUS)
                '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END
                setStopReason.StopMemo = drStop.STOPMEMO
                stopReasonList.Add(setStopReason)
            Next
        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} " _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START

    ''' <summary>
    ''' 中断区分を文字列に変換する処理
    ''' </summary>
    ''' <param name="inStopType">中断区分「01：部品欠品」「02：お客様連絡待ち」「03：検査不合格」「99：その他」</param>
    ''' <returns>中断区分文字列</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発
    ''' </history>
    Private Function GetStopReasonWord(ByVal inStopType As String) As String
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} P1:{2}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , inStopType))

        '戻り値宣言
        Dim returnWord As String = String.Empty

        '中断区分の判断
        Select Case inStopType
            Case StopTypePartsMissing
                '部品欠品
                returnWord = WebWordUtility.GetWord(WordProgramID, StopTypePartsMissingWordId)
            Case StopTypeVisitorConnectionWaiting
                'お客様連絡待ち
                returnWord = WebWordUtility.GetWord(WordProgramID, StopTypeVisitorConnectionWaitingWordId)
            Case StopTypeInspectionFailure
                '検査不合格
                returnWord = WebWordUtility.GetWord(WordProgramID, StopTypeInspectionFailureWordId)
            Case StopTypeOther
                'その他
                returnWord = WebWordUtility.GetWord(WordProgramID, StopTypeOtherWordId)
            Case Else
                Logger.Info("Unknown StopType")
        End Select

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} RETURN:{2}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , returnWord))
        Return returnWord
    End Function

    ''' <summary>
    ''' 表示区分処理(SMB)
    ''' </summary>
    ''' <param name="dtChipDetailReserve">予約情報</param>
    ''' <param name="dtChipDetailProcess">実績情報</param>
    ''' <param name="dtChipDetailRepairOrderInfo">RO情報</param>
    ''' <param name="inOrderNo">RO番号</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <History>
    ''' 2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発
    ''' 2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発
    ''' </History>
    Private Function GetSMBDispType(ByVal dtChipDetailReserve As ChipDetailReserveDataTable, _
                                    ByVal dtChipDetailProcess As ChipDetailProcessDataTable, _
                                    ByVal dtChipDetailRepairOrderInfo As ChipDetailRepairOrderInfoDataTable, _
                                    ByVal inOrderNo As String) As DisplayType

        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
        'Private Function GetSMBDispType(ByVal drOrderStatus As IC3801901DataSet.OrderStatusDataRow, _
        '                                ByVal inOrderNo As String, _
        '                                ByVal dtAddRepairStatus As IC3800804AddRepairStatusDataTableDataTable) As DisplayType
        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} P1:{2}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , inOrderNo))

        Dim returnDispType As DisplayType
        'ステータス情報がある場合はデータを格納
        Dim orderDataType As String                'R/O有無
        Dim orderStatus As String                  'R/Oステータス
        '2017/09/28 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
        Dim minOrderStatus As String = String.Empty              'R/Oステータス（最小）
        '2017/09/28 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END

        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
        'If drOrderStatus IsNot Nothing Then
        '    orderDataType = "1"
        '    orderStatus = drOrderStatus.ORDERSTATUS
        'Else
        '    orderDataType = "0"
        '    orderStatus = Nothing
        'End If

        Dim workEndType As String                  '作業終了有無
        If dtChipDetailRepairOrderInfo IsNot Nothing AndAlso 0 < dtChipDetailRepairOrderInfo.Count Then
            orderDataType = "1"
            orderStatus = dtChipDetailRepairOrderInfo(0).RO_STATUS
            workEndType = dtChipDetailRepairOrderInfo(0).WORK_END_TYPE

            '2017/09/28 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
            '取得したR/Oステータス（最小）
            minOrderStatus = dtChipDetailRepairOrderInfo(0).RO_STATUS_MIN
            '2017/09/28 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
        Else
            orderDataType = "0"
            orderStatus = Nothing
            workEndType = "0"

        End If

        Dim carWashEndType As String                    '洗車終了有無
        If dtChipDetailProcess IsNot Nothing AndAlso 0 < dtChipDetailProcess.Count Then
            '洗車フラグと実績チェック
            If WashFlag.Equals(dtChipDetailProcess(0).WASHFLG) AndAlso _
               String.IsNullOrEmpty(dtChipDetailProcess(0).RESULT_WASH_END) Then
                '「1：洗車あり」且つ洗車が終了していない場合
                carWashEndType = CarWashEndTypeWashing      '洗車中

            Else
                carWashEndType = CarWashEndTypeWashEnd      '洗車終了

            End If
        Else
            carWashEndType = Nothing

        End If

        Dim serviceStatus As String                     'サービスステータス
        If dtChipDetailReserve IsNot Nothing AndAlso 0 < dtChipDetailReserve.Count Then
            serviceStatus = dtChipDetailReserve(0).SVC_STATUS

        Else
            serviceStatus = Nothing

        End If
        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        '表示区分取得
        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
        'returnDispType = CType(Me.GetChipArea(orderDataType, _
        '                                      orderStatus, _
        '                                      ReserveEffective, _
        '                                      dtAddRepairStatus), DisplayType)
        '2017/09/28 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
        'returnDispType = CType(Me.GetChipArea(orderDataType, _
        '                                      orderStatus, _
        '                                      ReserveEffective, _
        '                                      workEndType, _
        '                                      carWashEndType, _
        '                                      serviceStatus), DisplayType)
        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END
        returnDispType = CType(Me.GetChipArea(orderDataType, orderStatus, ReserveEffective, _
                                              carWashEndType, serviceStatus, minOrderStatus), DisplayType)
        '2017/09/28 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} OUT:RETURN = {2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , returnDispType))
        Return returnDispType
    End Function

    ''' <summary>
    ''' ステータス判定取得処理(SMB)
    ''' </summary>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inBranchCode">店舗コード</param>
    ''' <param name="inChipAreaType">チップエリア(1：ストール、2：受付、3：追加作業、4：完成検査、5：洗車、6：納車待ち、7：中断、8：NoShow)</param>
    ''' <param name="inVisitSequence">来店実績連番</param>
    ''' <param name="inOrderType">R/O情報有無(0：無、1：有)</param>
    ''' <param name="inOrderNo">R/O番号</param>
    ''' <param name="inWorkStartDate">実績開始日時</param>
    ''' <param name="inStallUseStatus">ストール利用ステータス</param>
    ''' <param name="inWorkEndDate">実績終了日時</param>
    ''' <param name="inCompleteExaminationType">完成検査フラグ(0：完成検査依頼未、1：完成検査依頼済み、2：完成検査承認済み)</param>
    ''' <param name="inServiceinStatus">サービス入庫ステータス</param>
    ''' <param name="inInvoicePrintDate">清算書印刷日時</param>
    ''' <param name="dtJobDetailSequenceInfo">RO作業連番情報</param>
    ''' <returns>ステータス</returns>
    ''' <remarks></remarks>
    ''' <History>
    ''' 2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発
    ''' 2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発
    ''' </History>
    Private Function GetStatusSMB(ByVal inDealerCode As String, _
                                  ByVal inBranchCode As String, _
                                  ByVal inChipAreaType As Integer, _
                                  ByVal inVisitSequence As Long, _
                                  ByVal inOrderType As String, _
                                  ByVal inOrderNo As String, _
                                  ByVal inWorkStartDate As Date, _
                                  ByVal inStallUseStatus As String, _
                                  ByVal inWorkEndDate As Date, _
                                  ByVal inCompleteExaminationType As String, _
                                  ByVal inServiceinStatus As String, _
                                  ByVal inInvoicePrintDate As Date, _
                                  ByVal dtJobDetailSequenceInfo As JobDetailSequenceInfoDataTable) As String

        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
        'Private Function GetStatusSMB(ByVal inVisitType As String, _
        '                              ByVal inWorkStartDate As Date, _
        '                              ByVal inWorkEndDate As Date, _
        '                              ByVal inWashType As String, _
        '                              ByVal inOrderNo As String, _
        '                              ByVal inCompleteExaminationType As String, _
        '                              ByVal dtChipDetailProcess As ChipDetailProcessDataTable, _
        '                              ByVal drOrderStatus As IC3801901DataSet.OrderStatusDataRow, _
        '                              ByVal dtAddRepairStatus As IC3800804AddRepairStatusDataTableDataTable, _
        '                              ByVal drReserveROStatusList As IC3801012DataSet.REZROStatusListRow, _
        '                              ByVal inStallUseStatus As String, _
        '                              ByVal inSequenceNo As Long, _
        '                              ByVal inDeliveryDate As Date) As String
        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} P1:{2} P2:{3} P3:{4} P4:{5} P5:{6} P6:{7} P7:{8} P8:{9} P9:{10} P10:{11} " _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , inChipAreaType, inVisitSequence, inOrderType, inOrderNo, inWorkStartDate, inStallUseStatus _
                  , inWorkEndDate, inCompleteExaminationType, inServiceinStatus, inInvoicePrintDate))

        '戻り値宣言
        Dim returnStatus As String

        'チップ詳細情報(実績)がある場合はデータを格納
        Dim stopType As String = Nothing                                    '中断有無

        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
        'Dim resultWashStart As String = Nothing                             '洗車開始実績日時
        'Dim resultWashEnd As String = Nothing                               '洗車終了実績日時
        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
        'If dtChipDetailProcess IsNot Nothing AndAlso 0 < dtChipDetailProcess.Count Then
        '    'チップ詳細情報(実績)
        '    Dim drChipDetailProcess As ChipDetailProcessRow
        '    drChipDetailProcess = DirectCast(dtChipDetailProcess.Rows(0), ChipDetailProcessRow)

        '    'ストール利用ステータスチェック
        '    If StallUseStatusStop.Equals(inStallUseStatus) Then
        '        '「05：中断」の場合
        '        stopType = Discontinuation

        '    Else
        '        '上記以外の場合
        '        stopType = NoDiscontinuation

        '    End If
        '    resultWashStart = drChipDetailProcess.RESULT_WASH_START
        '    resultWashEnd = drChipDetailProcess.RESULT_WASH_END

        'Else
        '    stopType = "0"
        '    resultWashStart = String.Empty
        '    resultWashEnd = String.Empty

        'End If

        'ストール利用ステータスチェック
        If StallUseStatusStop.Equals(inStallUseStatus) Then
            '「05：中断」の場合
            stopType = Discontinuation

        Else
            '上記以外の場合
            stopType = NoDiscontinuation

        End If
        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
        ''ステータス情報がある場合はデータを格納
        'Dim orderStatus As String = Nothing
        'Dim partsPreparationWaitType As String = Nothing
        'If drOrderStatus IsNot Nothing Then
        '    'ステータス情報
        '    orderStatus = drOrderStatus.ORDERSTATUS
        '    partsPreparationWaitType = Me.checkStringRowData(drOrderStatus, "PARTSREPAREFLAG")
        'End If

        Dim partsPreparationWaitType As String = Me.GetPartsStatus(inDealerCode, _
                                                                   inBranchCode, _
                                                                   inOrderNo, _
                                                                   dtJobDetailSequenceInfo)     '部品準備ステータス

        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        'ステータス判定取得
        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
        'returnStatus = Me.GetSmbChipDetailStatus(inVisitType, _
        '                                         inWorkStartDate, _
        '                                         inWorkEndDate, _
        '                                         stopType, _
        '                                         inWashType, _
        '                                         inOrderNo, _
        '                                         orderStatus, _
        '                                         partsPreparationWaitType, _
        '                                         inCompleteExaminationType, _
        '                                         inStallUseStatus, _
        '                                         resultWashStart, _
        '                                         resultWashEnd, _
        '                                         dtAddRepairStatus, _
        '                                         drReserveROStatusList, _
        '                                         inSequenceNo, _
        '                                         inDeliveryDate)

        returnStatus = Me.GetSmbChipDetailStatus(inChipAreaType, _
                                                 inVisitSequence, _
                                                 inOrderType, _
                                                 inOrderNo, _
                                                 inWorkStartDate, _
                                                 inStallUseStatus, _
                                                 partsPreparationWaitType, _
                                                 stopType, _
                                                 inWorkEndDate, _
                                                 inCompleteExaminationType, _
                                                 inServiceinStatus, _
                                                 inInvoicePrintDate)
        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} OUT:RETURN = {2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , returnStatus))
        Return returnStatus

    End Function

    '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

    '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
    ' ''' <summary>
    ' ''' 納車予定時刻変更リスト格納処理
    ' ''' </summary>
    ' ''' <param name="deliveryChgList">納車予定時刻変更リスト</param>
    ' ''' <param name="dtHistoryDeliveryDateList">納車予定時刻変更データ</param>
    ' ''' <remarks></remarks>
    'Private Sub setDeliveryChgList(ByVal deliveryChgList As List(Of DeliveryChg), _
    '                               ByVal dtHistoryDeliveryDateList As IC3801701DataSet.HistoryDeliveryDateListDataTable)
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                   , "{0}.{1} " _
    '                   , Me.GetType.ToString _
    '                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

    '    If dtHistoryDeliveryDateList IsNot Nothing AndAlso dtHistoryDeliveryDateList.Count <> 0 Then
    '        For Each drHistoryDeliveryDateList As IC3801701DataSet.HistoryDeliveryDateListRow In dtHistoryDeliveryDateList
    '            Dim setDeliveryChg As New DeliveryChg
    '            setDeliveryChg.ChangeDate = drHistoryDeliveryDateList.CHANGEDATE
    '            setDeliveryChg.OldDeliveryHopeDate = drHistoryDeliveryDateList.OLDDELIVERYHOPEDATE
    '            setDeliveryChg.NewDeliveryHopeDate = drHistoryDeliveryDateList.NEWDELIVERYHOPEDATE
    '            setDeliveryChg.ChangeReason = drHistoryDeliveryDateList.CHANGEREASON
    '            deliveryChgList.Add(setDeliveryChg)
    '        Next
    '    End If

    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                   , "{0}.{1} " _
    '                   , Me.GetType.ToString _
    '                   , System.Reflection.MethodBase.GetCurrentMethod.Name))
    'End Sub
    '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

    ''' <summary>
    ''' 時間変換 (hh:mm) 又は (mm/dd)　
    ''' </summary>
    ''' <param name="nowDate">現在日時</param>
    ''' <param name="time">変換対象時間</param>
    ''' <returns>変換値</returns>
    Private Function SetDateTimeToString(ByVal nowDate As DateTime, ByVal time As DateTime) As String
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} " _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim strResult As String
        ' 日付チェック
        If time.Equals(DateTime.MinValue) Then
            Return String.Empty
        End If
        ' 時間範囲チェック
        If nowDate.ToString("yyyyMMdd", CultureInfo.CurrentCulture).Equals(time.ToString("yyyyMMdd", CultureInfo.CurrentCulture)) Then
            ' 当日 (hh:mm)
            strResult = DateTimeFunc.FormatDate(14, time)
        Else
            ' 上記以外 (mm/dd)
            strResult = DateTimeFunc.FormatDate(11, time)
        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} OUT:RETURN = {2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , strResult))
        Return strResult
    End Function

    ''' <summary>
    ''' 時間変換 (hh:mm) 又は (mm/dd)　
    ''' </summary>
    ''' <param name="nowDate">対象時間</param>
    ''' <param name="time">変換対象時間</param>
    ''' <returns>変換値</returns>
    Private Function SetDateStringToString(ByVal nowDate As DateTime, ByVal time As String) As String
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} " _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name))

        ' 空白チェック
        If String.IsNullOrEmpty(time) Then
            Return String.Empty
        End If
        ' 日付チェック
        Dim result As DateTime
        If Not (DateTime.TryParse(time, result)) Then
            Return String.Empty
        End If
        If result.Equals(DateTime.MinValue) Then
            Return String.Empty
        End If

        Dim strResult As String = SetDateTimeToString(nowDate, result)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} OUT:RETURN = {2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , strResult))
        Return strResult
    End Function

    ''' <summary>
    ''' 列データのチェック処理「String」
    ''' </summary>
    ''' <param name="checkDataRow">チェックする列データ</param>
    ''' <param name="checkDataName">チェックする列名</param>
    ''' <returns>列のデータ or Nothing</returns>
    ''' <remarks></remarks>
    Private Function checkStringRowData(ByVal checkDataRow As DataRow, ByVal checkDataName As String) As String
        Try
            Dim returnData As String
            'DBNULLかをチェックする
            If IsDBNull(checkDataRow.Item(checkDataName)) Then
                'DBNULLの場合はNothingを返す
                returnData = Nothing
            Else
                'データがある場合はそれを返す
                returnData = CType(checkDataRow.Item(checkDataName), String)
            End If
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:RETURN = {2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , returnData))
            Return returnData
        Catch ex As ArgumentException
            Logger.Error(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1}「{2}」OUT:RETURN = {3}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , checkDataName & " is not column." _
                        , "Nothing"))
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' 列データのチェック処理「DateTimeFunc」
    ''' </summary>
    ''' <param name="checkDataRow">チェックする列データ</param>
    ''' <param name="checkDataName">チェックする列名</param>
    ''' <returns>列のデータ or Nothing</returns>
    ''' <remarks></remarks>
    Private Function checkDateRowData(ByVal checkDataRow As DataRow, ByVal checkDataName As String) As Date
        Try
            Dim returnData As Date
            'DBNULLかをチェックする
            If IsDBNull(checkDataRow.Item(checkDataName)) Then
                'DBNULLの場合はNothingを返す
                returnData = Nothing
            Else
                'データがある場合はそれを返す
                returnData = CType(checkDataRow.Item(checkDataName), Date)
            End If
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:RETURN = {2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , returnData))
            Return returnData
        Catch ex As ArgumentException
            Logger.Error(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1}「{2}」OUT:RETURN = {3}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , checkDataName & " is not column." _
                        , "Nothing"))
            Return Nothing
        End Try
    End Function

    '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
    ' ''' <summary>
    ' ''' 顧客参照取得
    ' ''' </summary>
    ' ''' <param name="inDealerCode">販売店コード</param>
    ' ''' <param name="drChipDetailVisit">チップ詳細情報取得(来店)</param>
    ' ''' <returns>顧客参照情報</returns>
    ' ''' <remarks></remarks>
    'Private Function GetSrvCustomer(ByVal inDealerCode As String, _
    '                                ByVal drChipDetailVisit As ChipDetailVisitRow) As IC3800703SrvCustomerDataTable
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                   , "{0}.{1} P1:{2}" _
    '                   , Me.GetType.ToString _
    '                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                   , inDealerCode))

    '    '「顧客区分 = 1：自社客 AndAlso (車両登録No <> NULL OrElse VIN <> NULL)」の場合は顧客参照をする
    '    If COMPANY_VISITOR.Equals(drChipDetailVisit.CUSTSEGMENT) AndAlso _
    '       (Not (drChipDetailVisit.IsVCLREGNONull) OrElse _
    '        Not (drChipDetailVisit.IsVINNull)) Then
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                       , "CALL [IC3800703] {0}.{1} P1:{2} P2:{3} P3:{4}" _
    '                       , Me.GetType.ToString _
    '                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                       , drChipDetailVisit.VCLREGNO, drChipDetailVisit.VIN, inDealerCode))
    '        Dim bizIC3800703 As New IC3800703BusinessLogic
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                       , "{0}.{1} OUT:IC3800703SrvCustomerDataTable" _
    '                       , Me.GetType.ToString _
    '                       , System.Reflection.MethodBase.GetCurrentMethod.Name))
    '        Return bizIC3800703.GetCustomerInfo(drChipDetailVisit.VCLREGNO, _
    '                                         drChipDetailVisit.VIN, _
    '                                         inDealerCode)
    '    Else
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                       , "{0}.{1} OUT:Nothing" _
    '                       , Me.GetType.ToString _
    '                       , System.Reflection.MethodBase.GetCurrentMethod.Name))
    '        Return Nothing
    '    End If
    'End Function
    '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

    '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
    ' ''' <summary>
    ' ''' RO基本情報を取得
    ' ''' </summary>
    ' ''' <param name="inDealerCode">販売店コード</param>
    ' ''' <param name="inStoreCode">店舗コード</param>
    ' ''' <param name="inOrderNo">RO番号</param>
    ' ''' <returns>RO基本情報</returns>
    ' ''' <remarks></remarks>
    'Private Function GetOrderCommonInfo(ByVal inDealerCode As String, _
    '                                    ByVal inStoreCode As String, _
    '                                    ByVal inOrderNo As String) As IC3801001DataSet.IC3801001OrderCommRow

    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                   , "{0}.{1} START P1:{2} P2:{3} P3:{4}" _
    '                   , Me.GetType.ToString _
    '                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                   , inDealerCode, inStoreCode, inOrderNo))

    '    Dim drOrderCommon As IC3801001DataSet.IC3801001OrderCommRow

    '    'RO番号がある場合のみ処理を行う
    '    If Not (String.IsNullOrEmpty(inOrderNo)) Then
    '        'RO基本情報を取得する
    '        Dim bizIC3801001 As New IC3801001BusinessLogic

    '        Dim dtOrderCommon As IC3801001DataSet.IC3801001OrderCommDataTable = _
    '            bizIC3801001.GetROBaseInfoList(inDealerCode, inOrderNo)

    '        '取得できた場合のみ設定する
    '        If Not (IsNothing(dtOrderCommon)) AndAlso 0 < dtOrderCommon.Count Then
    '            drOrderCommon = DirectCast(dtOrderCommon.Rows(0), IC3801001DataSet.IC3801001OrderCommRow)

    '        Else
    '            drOrderCommon = Nothing

    '        End If

    '    Else
    '        drOrderCommon = Nothing

    '    End If

    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                   , "{0}.{1} END" _
    '                   , Me.GetType.ToString _
    '                   , System.Reflection.MethodBase.GetCurrentMethod.Name))
    '    Return drOrderCommon
    'End Function
    '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

    '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
    ' ''' <summary>
    ' ''' ステータス情報を取得
    ' ''' </summary>
    ' ''' <param name="inDealerCode">販売店コード</param>
    ' ''' <param name="inStoreCode">店舗コード</param>
    ' ''' <param name="inOrderNo">RO番号</param>
    ' ''' <returns>ステータス情報</returns>
    ' ''' <remarks></remarks>
    'Private Function GetOrderStatusInfo(ByVal inDealerCode As String, _
    '                                    ByVal inStoreCode As String, _
    '                                    ByVal inOrderNo As String) As IC3801901DataSet.OrderStatusDataRow
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                   , "{0}.{1} START P1:{2} P2:{3} P3:{4}" _
    '                   , Me.GetType.ToString _
    '                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                   , inDealerCode, inStoreCode, inOrderNo))

    '    Dim drOrderStatus As IC3801901DataSet.OrderStatusDataRow

    '    'RO番号がある場合のみ処理を行う
    '    If Not (String.IsNullOrEmpty(inOrderNo)) Then
    '        'ステータス情報を取得する
    '        Dim bizIC3801901 As New IC3801901BusinessLogic
    '        drOrderStatus = _
    '            bizIC3801901.GetOrderStatus(inDealerCode, _
    '                                        inStoreCode, _
    '                                        inOrderNo)

    '    Else
    '        drOrderStatus = Nothing

    '    End If

    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                   , "{0}.{1} END" _
    '                   , Me.GetType.ToString _
    '                   , System.Reflection.MethodBase.GetCurrentMethod.Name))
    '    Return drOrderStatus
    'End Function
    '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

    '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
    ' ''' <summary>
    ' ''' 追加作業ステータスを取得
    ' ''' </summary>
    ' ''' <param name="inDealerCode">販売店コード</param>
    ' ''' <param name="inOrderNo">RO番号</param>
    ' ''' <returns>追加作業ステータス</returns>
    ' ''' <remarks></remarks>
    'Private Function GetAddRepairStatusInfo(ByVal inDealerCode As String, _
    '                                        ByVal inOrderNo As String) As IC3800804AddRepairStatusDataTableDataTable
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                   , "{0}.{1} START P1:{2} P2:{3} " _
    '                   , Me.GetType.ToString _
    '                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                   , inDealerCode, inOrderNo))

    '    'RO番号がある場合のみ処理を行う
    '    If Not (String.IsNullOrEmpty(inOrderNo)) Then
    '        '追加作業ステータスの取得する
    '        Dim bizIC3800804 As New IC3800804BusinessLogic
    '        Dim dtAddRepairStatus As IC3800804AddRepairStatusDataTableDataTable = _
    '            DirectCast(bizIC3800804.GetAddRepairStatusList(inDealerCode, inOrderNo),  _
    '                       IC3800804AddRepairStatusDataTableDataTable)

    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                       , "{0}.{1} END" _
    '                       , Me.GetType.ToString _
    '                       , System.Reflection.MethodBase.GetCurrentMethod.Name))
    '        Return dtAddRepairStatus

    '    Else
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                       , "{0}.{1} END" _
    '                       , Me.GetType.ToString _
    '                       , System.Reflection.MethodBase.GetCurrentMethod.Name))
    '        Return Nothing

    '    End If
    'End Function
    '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

    '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
    ' ''' <summary>
    ' ''' R/O事前準備状態一覧を取得
    ' ''' </summary>
    ' ''' <param name="inDealerCode">販売店コード</param>
    ' ''' <param name="inStoreCode">店舗コード</param>
    ' ''' <param name="inOrderNo">RO番号</param>
    ' ''' <returns>R/O事前準備状態一覧</returns>
    ' ''' <remarks></remarks>
    'Private Function GetReserveOrderStatusInfo(ByVal inDealerCode As String, _
    '                                           ByVal inStoreCode As String, _
    '                                           ByVal inOrderNo As String) As IC3801012DataSet.REZROStatusListRow
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                   , "{0}.{1} P1:{2} P2:{3} P3:{4}" _
    '                   , Me.GetType.ToString _
    '                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                   , inDealerCode, inStoreCode, inOrderNo))

    '    'RO番号がある場合のみ処理を行う
    '    If Not (String.IsNullOrEmpty(inOrderNo)) Then
    '        'R/O事前準備状態一覧を取得する
    '        Dim drReserveROStatusList As IC3801012DataSet.REZROStatusListRow
    '        Dim bizIC3801012 As New IC3801012BusinessLogic
    '        Dim orderList As New List(Of String)
    '        orderList.Add(inOrderNo)

    '        Dim dtReserveROStatusList As IC3801012DataSet.REZROStatusListDataTable = _
    '            bizIC3801012.GetREZROStatusList(inDealerCode, inStoreCode, orderList)

    '        If Not (IsNothing(dtReserveROStatusList)) AndAlso 0 < dtReserveROStatusList.Count Then
    '            drReserveROStatusList = DirectCast(dtReserveROStatusList.Rows(0),  _
    '                                               IC3801012DataSet.REZROStatusListRow)
    '        Else
    '            drReserveROStatusList = Nothing
    '        End If

    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                       , "{0}.{1} END" _
    '                       , Me.GetType.ToString _
    '                       , System.Reflection.MethodBase.GetCurrentMethod.Name))
    '        Return drReserveROStatusList

    '    Else
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                       , "{0}.{1} END" _
    '                       , Me.GetType.ToString _
    '                       , System.Reflection.MethodBase.GetCurrentMethod.Name))
    '        Return Nothing

    '    End If
    'End Function
    '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

    '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
    ' ''' <summary>
    ' ''' 故障原因、診断結果情報を取得
    ' ''' </summary>
    ' ''' <param name="inDealerCode">販売店コード</param>
    ' ''' <param name="inOrderNo">RO番号</param>
    ' ''' <param name="inOrderJobSequence">RO作業連番</param>
    ' ''' <returns>故障原因、診断結果情報</returns>
    ' ''' <remarks></remarks>
    'Private Function GetFaultReasonInfo(ByVal inDealerCode As String, _
    '                                    ByVal inOrderNo As String, _
    '                                    ByVal inOrderJobSequence As Long) As IC3801014DataSet.IC3801014FaultReasonInfoDataTable
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                   , "{0}.{1} P1:{2} P2:{3} P3:{4}" _
    '                   , Me.GetType.ToString _
    '                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                   , inDealerCode, inOrderNo, inOrderJobSequence))

    '    'RO番号がある場合のみ処理を行う
    '    If Not (String.IsNullOrEmpty(inOrderNo)) Then
    '        Dim bizIC3801014 As New IC3801014BusinessLogic

    '        Dim dtReserveROStatusList As IC3801014DataSet.IC3801014FaultReasonInfoDataTable = _
    '            bizIC3801014.GetFaultReason(inDealerCode, _
    '                                        inOrderNo, _
    '                                        CType(inOrderJobSequence, Integer))

    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                       , "{0}.{1} END" _
    '                       , Me.GetType.ToString _
    '                       , System.Reflection.MethodBase.GetCurrentMethod.Name))
    '        Return dtReserveROStatusList

    '    Else
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                       , "{0}.{1} END" _
    '                       , Me.GetType.ToString _
    '                       , System.Reflection.MethodBase.GetCurrentMethod.Name))
    '        Return Nothing

    '    End If
    'End Function
    '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

    '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
    ' ''' <summary>
    ' ''' 納車予定変更履歴を取得
    ' ''' </summary>
    ' ''' <param name="inDealerCode">販売店コード</param>
    ' ''' <param name="inStoreCode">店舗コード</param>
    ' ''' <param name="inOrderNo">RO番号</param>
    ' ''' <param name="inChipArea">表示区分</param>
    ' ''' <returns>納車予定変更履歴</returns>
    ' ''' <remarks></remarks>
    'Private Function GetHistoryDeliveryInfo(ByVal inDealerCode As String, _
    '                                        ByVal inStoreCode As String, _
    '                                        ByVal inOrderNo As String, _
    '                                        ByVal inChipArea As DisplayType) As IC3801701DataSet.HistoryDeliveryDateListDataTable
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                   , "{0}.{1} P1:{2} P2:{3} P3:{4}" _
    '                   , Me.GetType.ToString _
    '                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                   , inDealerCode, inStoreCode, inOrderNo))

    '    'RO番号がある、表示区分が「1：受付、0：エラー」ではない場合のみ処理を行う
    '    If Not (String.IsNullOrEmpty(inOrderNo)) AndAlso _
    '           inChipArea <> DisplayType.Invalid AndAlso inChipArea <> DisplayType.Err Then
    '        '納車予定変更履歴取得をする
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                       , "CALL [IC3801701] {0}.{1} P1:{2} P2:{3} P3:{4}" _
    '                       , Me.GetType.ToString _
    '                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                       , inDealerCode, inStoreCode, inOrderNo))
    '        Dim bizIC3801701 As New IC3801701BusinessLogic
    '        Dim dtHistoryDeliveryDateList As IC3801701DataSet.HistoryDeliveryDateListDataTable = _
    '            bizIC3801701.GetHistoryDeliveryDateList(inDealerCode, _
    '                                                    inStoreCode, _
    '                                                    inOrderNo)

    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                       , "{0}.{1} END" _
    '                       , Me.GetType.ToString _
    '                       , System.Reflection.MethodBase.GetCurrentMethod.Name))
    '        Return dtHistoryDeliveryDateList

    '    Else
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                       , "{0}.{1} END" _
    '                       , Me.GetType.ToString _
    '                       , System.Reflection.MethodBase.GetCurrentMethod.Name))
    '        Return Nothing

    '    End If
    'End Function
    '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

    ''' <summary>
    ''' 中断情報(SMB)を取得
    ''' </summary>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inStoreCode">店舗コード</param>
    ''' <param name="inStallUseId">ストール利用ID</param>
    ''' <param name="inStallUseStatus">ストール利用ステータス</param>
    ''' <param name="daSMBCommonClass">DataAccess</param>
    ''' <returns>中断情報(SMB)</returns>
    ''' <remarks></remarks>
    Private Function GetSMBStopInfo(ByVal inDealerCode As String, _
                                    ByVal inStoreCode As String, _
                                    ByVal inStallUseId As Decimal, _
                                    ByVal inStallUseStatus As String, _
                                    ByVal daSMBCommonClass As SMBCommonClassTableAdapter) As StopDataTable
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} P1:{2} P2:{3} P3:{4} P4:{5}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , inDealerCode, inStoreCode, inStallUseId, inStallUseStatus))

        'ストール利用ステータスが「05：中断」の場合のみ中断情報(SMB)を取得する
        If StallUseStatusStop.Equals(inStallUseStatus) Then
            Dim dtStop As StopDataTable = daSMBCommonClass.GetSmbStopData(inDealerCode, _
                                                                          inStoreCode, _
                                                                          inStallUseId)

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                           , "{0}.{1} END" _
                           , Me.GetType.ToString _
                           , System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return dtStop

        Else
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                           , "{0}.{1} END" _
                           , Me.GetType.ToString _
                           , System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return Nothing

        End If
    End Function

    '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START

    ''' <summary>
    ''' RO情報(SMB)を取得
    ''' </summary>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inStoreCode">店舗コード</param>
    ''' <param name="inVisitSequence">来店実績連番</param>
    ''' <param name="inVisitType">来店実績有無「0：無」「1：有」</param>
    ''' <param name="daSMBCommonClass">DataAccess</param>
    ''' <returns>RO情報</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発
    ''' </history>
    Private Function GetSMBRepairOrderInfo(ByVal inDealerCode As String, _
                                           ByVal inStoreCode As String, _
                                           ByVal inVisitSequence As Long, _
                                           ByVal inVisitType As String, _
                                           ByVal daSMBCommonClass As SMBCommonClassTableAdapter) As ChipDetailRepairOrderInfoDataTable
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} P1:{2} P2:{3} P3:{4} P4:{5}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , inDealerCode _
                       , inStoreCode _
                       , inVisitSequence.ToString(CultureInfo.CurrentCulture) _
                       , inVisitType))

        'RO情報のチェック
        If RepairOrderTypeExist.Equals(inVisitType) Then
            'RO情報が存在する場合
            'RO情報を取得
            Dim dtChipDetailRepairOrderInfo As ChipDetailRepairOrderInfoDataTable = _
                daSMBCommonClass.GetRepariOrderInfo(inDealerCode, _
                                                    inStoreCode, _
                                                    inVisitSequence)

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                           , "{0}.{1} END" _
                           , Me.GetType.ToString _
                           , System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return dtChipDetailRepairOrderInfo

        Else
            'RO情報が存在しない場合

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                           , "{0}.{1} END" _
                           , Me.GetType.ToString _
                           , System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return Nothing

        End If

    End Function

    ''' <summary>
    ''' 部品準備ステータスを取得
    ''' </summary>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inStoreCode">店舗コード</param>
    ''' <param name="inOrderNo">RO番号</param>
    ''' <param name="dtJobDetailSequenceInfo">RO作業連番情報</param>
    ''' <returns>部品準備ステータス</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発
    ''' </history>
    Private Function GetPartsStatus(ByVal inDealerCode As String, _
                                    ByVal inStoreCode As String, _
                                    ByVal inOrderNo As String, _
                                    ByVal dtJobDetailSequenceInfo As JobDetailSequenceInfoDataTable) As String
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0} P1:{1} P2:{2} P3:{3} " _
                       , String.Concat(Me.GetType.ToString, ".", System.Reflection.MethodBase.GetCurrentMethod.Name) _
                       , inDealerCode, inStoreCode, inOrderNo))

        '戻り値
        Dim returnParsStatus As String = String.Empty

        'RO番号のチェック
        If Not String.IsNullOrEmpty(inOrderNo) Then
            'RO番号が存在する場合
            'RO番号を取得

            Using biz As New IC3802503BusinessLogic

                Dim dtPartsStatus As IC3802503PartsStatusDataTable = Nothing

                Using dtRONumInfo As New IC3802503RONumInfoDataTable

                    '取得枝番チェック
                    If dtJobDetailSequenceInfo IsNot Nothing AndAlso 0 < dtJobDetailSequenceInfo.Count Then
                        '枝番単位で取得する場合
                        'RO作業連番情報を設定する
                        For Each drJobDetailSequenceInfo As JobDetailSequenceInfoRow In dtJobDetailSequenceInfo
                            '親の情報を設定する
                            Dim drRONumInfo As IC3802503RONumInfoRow = dtRONumInfo.NewIC3802503RONumInfoRow
                            drRONumInfo.R_O = inOrderNo
                            drRONumInfo.R_O_SEQNO = _
                                drJobDetailSequenceInfo.RO_SEQ.ToString(CultureInfo.CurrentCulture)

                            dtRONumInfo.AddIC3802503RONumInfoRow(drRONumInfo)

                        Next

                    Else
                        '枝番単位で取得しない場合
                        '親の情報を設定する
                        Dim drRONumInfo As IC3802503RONumInfoRow = dtRONumInfo.NewIC3802503RONumInfoRow
                        drRONumInfo.R_O = inOrderNo
                        drRONumInfo.R_O_SEQNO = "0"

                        dtRONumInfo.AddIC3802503RONumInfoRow(drRONumInfo)

                    End If

                    '2015/06/26 TMEJ 明瀬 TMTレスポンス調査のためのログ出力 START
                    Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                 "{0}.{1} ⑥SC3240201_チップ詳細表示[部品ステータス情報取得] START", _
                                 Me.GetType.ToString, _
                                 System.Reflection.MethodBase.GetCurrentMethod.Name))
                    '2015/06/26 TMEJ 明瀬 TMTレスポンス調査のためのログ出力 END

                    dtPartsStatus = _
                        biz.GetPartsStatusList(inDealerCode, _
                                               inStoreCode, _
                                               dtRONumInfo)

                    '2015/06/26 TMEJ 明瀬 TMTレスポンス調査のためのログ出力 START
                    Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                 "{0}.{1} ⑥SC3240201_チップ詳細表示[部品ステータス情報取得] END", _
                                 Me.GetType.ToString, _
                                 System.Reflection.MethodBase.GetCurrentMethod.Name))
                    '2015/06/26 TMEJ 明瀬 TMTレスポンス調査のためのログ出力 END

                End Using

                '部品情報のチェック
                If dtPartsStatus IsNot Nothing AndAlso 0 < dtPartsStatus.Count Then
                    '部品が取得できた場合
                    'エラーチェック
                    If dtPartsStatus(0).ResultCode = 0 Then
                        '成功している場合
                        '各ステータスの数を集計

                        '各ステータスのカウントをチェック
                        If dtPartsStatus.Count = (From dr As IC3802503PartsStatusRow In dtPartsStatus _
                                                  Where dr.PARTS_ISSUE_STATUS = PartsPreparationFinish).Count Then
                            '部品準備が終了している場合
                            '「8：部品準備完了」を設定
                            returnParsStatus = PartsPreparationFinish

                        ElseIf 0 < (From dr As IC3802503PartsStatusRow In dtPartsStatus _
                                    Where dr.PARTS_ISSUE_STATUS = PartsInPreparation).Count Then
                            '部品準備中の場合
                            '「1：部品準備中」を設定
                            returnParsStatus = PartsInPreparation

                        ElseIf dtPartsStatus.Count = (From dr As IC3802503PartsStatusRow In dtPartsStatus _
                                                      Where dr.PARTS_ISSUE_STATUS = PartsPreparationWaiting).Count Then
                            '部品準備していない場合
                            '「0：部品準備待ち」を設定
                            returnParsStatus = PartsPreparationWaiting

                        End If

                    Else
                        '失敗している場合ばエラーに出しておく
                        Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                                                    , "{0} END ERROR:IC3802503BusinessLogic.GetPartsStatusList:{1}" _
                                                    , String.Concat(Me.GetType.ToString, ".", System.Reflection.MethodBase.GetCurrentMethod.Name) _
                                                    , returnParsStatus))

                    End If

                End If

            End Using

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                    , "{0} END RETURN:{1}" _
                                    , String.Concat(Me.GetType.ToString, ".", System.Reflection.MethodBase.GetCurrentMethod.Name) _
                                    , returnParsStatus))
        Return returnParsStatus
    End Function

    ''' <summary>
    ''' RO作業連番情報(SMB)を取得
    ''' </summary>
    ''' <param name="inVisitSequence">来店実績連番</param>
    ''' <param name="inServiceInId">サービス入庫ID</param>
    ''' <param name="inJobDetailSequenceId">作業内容ID</param>
    ''' <param name="inOrderNo">RO番号</param>
    ''' <param name="daSMBCommonClass">DataAccess</param>
    ''' <returns>RO情報</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発
    ''' </history>
    Private Function GetSMBJobDetailSequenceInfo(ByVal inVisitSequence As Long, _
                                                 ByVal inServiceInId As Decimal, _
                                                 ByVal inJobDetailSequenceId As Decimal, _
                                                 ByVal inOrderNo As String, _
                                                 ByVal daSMBCommonClass As SMBCommonClassTableAdapter) As JobDetailSequenceInfoDataTable
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0} P1:{1} P2:{2} P3:{3}" _
                       , String.Concat(Me.GetType.ToString, ".", System.Reflection.MethodBase.GetCurrentMethod.Name) _
                       , inServiceInId.ToString(CultureInfo.CurrentCulture) _
                       , inJobDetailSequenceId.ToString(CultureInfo.CurrentCulture) _
                       , inOrderNo))

        'RO情報のチェック
        If Not String.IsNullOrEmpty(inOrderNo) Then
            'RO番号が存在する場合
            'RO情報を取得
            Dim dtJobDetailSequenceInfoDataTable As JobDetailSequenceInfoDataTable = _
                daSMBCommonClass.GetJobDetailSequenceInfo(inVisitSequence, _
                                                          inServiceInId, _
                                                          inJobDetailSequenceId, _
                                                          inOrderNo)

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                           , "{0}.{1} END" _
                           , Me.GetType.ToString _
                           , System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return dtJobDetailSequenceInfoDataTable

        Else
            'RO番号存在しない場合
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                           , "{0}.{1} END" _
                           , Me.GetType.ToString _
                           , System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return Nothing

        End If

    End Function

    '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

#End Region

End Class
