'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3240501BusinessLogic.vb
'─────────────────────────────────────
'機能： 新規予約作成
'補足： 
'作成： 2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発
'更新： 2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応
'更新： 2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発
'更新： 2015/10/08 TM 皆川 タブレットSMBのチップ移動時の連携送信メッセージ詳細化
'更新： 2015/11/10 TM 杉田  (トライ店システム評価)SMBチップ検索の絞り込み方法変更
'更新： 2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証
'更新： 2019/07/22 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証
'更新： 2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証
'更新： 
'─────────────────────────────────────
Option Explicit On
Option Strict On

Imports System.Text
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web
Imports System.Reflection
Imports System.Globalization
Imports Toyota.eCRB.SMB.ReservationManagement.DataAccess.SC3240501DataSet
Imports System.Web.Script.Serialization
Imports Toyota.eCRB
Imports Toyota.eCRB.CommonUtility
Imports Toyota.eCRB.CommonUtility.SMBCommonClass.Api.BizLogic
Imports Toyota.eCRB.CommonUtility.SMBCommonClass.Api.BizLogic.SMBCommonClassBusinessLogic
Imports Toyota.eCRB.CommonUtility.SMBCommonClass.Api.DataAccess
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.BizLogic
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.BizLogic.TabletSMBCommonClassBusinessLogic
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.DataAccess
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.DataAccess.TabletSMBCommonClassDataSet
Imports Toyota.eCRB.SMB.ReservationManagement.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SMB.ReservationManagement.DataAccess.SC3240501DataSetTableAdapters
Imports Toyota.eCRB.SMBLinkage.GetUserList.Api.BizLogic
Imports Toyota.eCRB.SMBLinkage.GetUserList.Api.DataAccess
Imports Toyota.eCRB.DMSLinkage.Reserve.Api.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.BizLogic

'Imports Toyota.eCRB.DMSLinkage.StatusInfo.Api.BizLogic

''' <summary>
''' SC3240501
''' </summary>
''' <remarks></remarks>
Public Class SC3240501BusinessLogic
    Inherits BaseBusinessComponent
    Implements IDisposable

#Region "定数"
    ''' <summary>
    ''' 自画面のプログラムID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NEWCHIP_PROGRAMID As String = "SC3240501"

    ''' <summary>
    ''' 検索標準読み込み数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DEFAULT_READ_COUNT As String = "SC3240501_DEFAULT_READ_COUNT"

    ''' <summary>
    ''' 検索最大表示数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MAX_DISPLAY_COUNT As String = "SC3240501_MAX_DISPLAY_COUNT"

    ''' <summary>
    ''' 検索標準読み込み数(初期値)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DEFAULT_READ_COUNT_NUM As Long = 20

    ''' <summary>
    ''' 検索最大表示数(初期値)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MAX_DISPLAY_COUNT_NUM As Long = 40

    ''' <summary>
    ''' 引取納車区分:Waiting
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DeliTypeWaiting As String = "0"

    ''' <summary>
    ''' DB数値型の既定値（0）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DefaultNumberValue As Long = 0

    ''' <summary>
    ''' 休憩取得フラグ（0：休憩を取得しない）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NOT_USE_REST As String = "0"

    ''' <summary>
    ''' 休憩取得フラグ（1：休憩を取得する）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const USE_REST As String = "1"

    ''' <summary>
    ''' 日付フォーマット(システム設定値)
    ''' </summary>
    Private Const SYSDATEFORMAT = "DATE_FORMAT"
#End Region


#Region "Publicメソッド"
    ''' <summary>
    ''' 自社客検索結果取得
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="storeCode">店舗コード</param>
    ''' <param name="registrationNo">車両登録No.</param>
    ''' <param name="vin">VIN</param>
    ''' <param name="customerName">氏名</param>
    ''' <param name="phone">電話番号</param>
    ''' <param name="startRow">現在の表示開始行</param>
    ''' <param name="endRow">現在の表示終了行</param>
    ''' <param name="selectLoad">指定読み込み値</param>
    ''' <returns>自社客検索結果</returns>
    ''' <remarks></remarks>
    Public Function GetCustomerList(ByVal dealerCode As String, _
                                    ByVal storeCode As String, _
                                    ByVal registrationNo As String, _
                                    ByVal vin As String, _
                                    ByVal customerName As String, _
                                    ByVal phone As String, _
                                    ByVal startRow As Long, _
                                    ByVal endRow As Long, _
                                    ByVal selectLoad As Long) As SC3240501SearchResult

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                 , "{0}.{1} {2} IN:dealerCode = {3}, branchCode = {4}, vclRegNo = {5}, vin = {6}, customerName = {7}, " & _
                                 "phone = {8}, startRow = {9}, endRow = {10}, selectLoad = {11}" _
                                 , Me.GetType.ToString _
                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                 , "Start" _
                                 , dealerCode _
                                 , storeCode _
                                 , registrationNo _
                                 , vin _
                                 , customerName _
                                 , phone _
                                 , startRow _
                                 , endRow _
                                 , selectLoad))

        Dim systemEnv As New SystemEnvSetting

        Dim drSysEnvSetting As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow = Nothing
        Dim loadCount As Long = DEFAULT_READ_COUNT_NUM
        Dim maxDispCount As Long = MAX_DISPLAY_COUNT_NUM

        ' 検索標準読み込み数取得
        drSysEnvSetting = systemEnv.GetSystemEnvSetting(DEFAULT_READ_COUNT)
        If Not (IsNothing(drSysEnvSetting)) AndAlso Not (String.IsNullOrEmpty(drSysEnvSetting.PARAMVALUE)) Then
            loadCount = CType(drSysEnvSetting.PARAMVALUE, Long)
        End If

        ' 検索最大表示数取得
        drSysEnvSetting = systemEnv.GetSystemEnvSetting(MAX_DISPLAY_COUNT)
        If Not (IsNothing(drSysEnvSetting)) AndAlso Not (String.IsNullOrEmpty(drSysEnvSetting.PARAMVALUE)) Then
            maxDispCount = CType(drSysEnvSetting.PARAMVALUE, Long)
        End If

        '2015/11/10 TM 杉田  (トライ店システム評価)SMBチップ検索の絞り込み方法変更 START
        If Not String.IsNullOrEmpty(registrationNo) Then
            Dim convertRegistrationNo As String
            Using smbCommonBiz As New ServiceCommonClassBusinessLogic
                '車両登録No.の「*」と区切り文字を削除する
                convertRegistrationNo = smbCommonBiz.ConvertVclRegNumWord(registrationNo)
            End Using

            '区切り文字削除後の検索文字列が空の場合、検索結果0件として表示する(顧客を全件検索しない)
            If String.IsNullOrEmpty(convertRegistrationNo) Then
                Dim resultZero As New SC3240501SearchResult
                resultZero.SearchResult = 0
                resultZero.DataTable = Nothing
                resultZero.ResultStartRow = 0
                resultZero.ResultEndRow = 0
                resultZero.ResultCustomerCount = 0
                resultZero.StandardCount = loadCount

                '終了ログ
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2} OUT:COUNT = 0" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , "End"))

                Return resultZero
            End If

            '区切り文字削除後の検索文字列が空でない場合、車両登録No.にセットする
            registrationNo = convertRegistrationNo
        End If
        '2015/11/10 TM 杉田  (トライ店システム評価)SMBチップ検索の絞り込み方法変更 END

        ' 顧客数取得
        Dim customerCount As Long = 0
        Using da As New SC3240501DataTableAdapter
            customerCount = da.GetCustomerCount(dealerCode, _
                                                storeCode, _
                                                registrationNo, _
                                                vin, _
                                                customerName, _
                                                phone)
        End Using

        Dim searchStartRow As Long = 0
        Dim searchEndRow As Long = 0
        Dim result As New SC3240501SearchResult

        If Not customerCount.Equals(0) Then

            ' 検索処理呼び出し方法による分岐
            If selectLoad = 0 Then
                ' 検索アイコンタップ時
                searchStartRow = 1
                If customerCount < loadCount Then
                    searchEndRow = customerCount
                Else
                    searchEndRow = loadCount
                End If
            ElseIf 1 <= selectLoad Then
                ' 次のN件表示タップ時
                ' 終了行の設定
                Dim setEndMax As Long = endRow + loadCount
                If customerCount < setEndMax Then
                    searchEndRow = customerCount
                Else
                    searchEndRow = setEndMax
                End If
                ' 開始行の設定
                Dim setStartMax As Long = searchEndRow - startRow + 1
                If setStartMax <= maxDispCount Then
                    searchStartRow = startRow
                Else
                    searchStartRow = searchEndRow - maxDispCount + 1

                    If searchStartRow <= 0 Then
                        searchStartRow = 1
                    End If
                End If
            Else
                ' 前のN件表示タップ時
                ' 開始行の設定
                Dim setStartMin As Long = startRow - loadCount
                If setStartMin <= 0 Then
                    searchStartRow = 1
                Else
                    searchStartRow = setStartMin
                End If
                ' 終了行の設定
                Dim setEndMin As Long = endRow - searchStartRow + 1
                If setEndMin < maxDispCount Then
                    searchEndRow = endRow
                Else
                    searchEndRow = searchStartRow + maxDispCount - 1
                End If
                If customerCount < searchEndRow Then
                    searchEndRow = customerCount
                End If
            End If

            ' 顧客検索処理
            Dim dtCustomerSearch As SC3240501CustomerListDataTable
            Using da As New SC3240501DataTableAdapter
                dtCustomerSearch = da.GetCustomerList(dealerCode, _
                                                                storeCode, _
                                                                registrationNo, _
                                                                vin, _
                                                                customerName, _
                                                                phone, _
                                                                searchStartRow, _
                                                                searchEndRow)
            End Using
            ' 返却値の形成
            result.SearchResult = 0
            result.DataTable = dtCustomerSearch 'dt
            result.ResultStartRow = searchStartRow
            result.ResultEndRow = searchEndRow
            result.ResultCustomerCount = customerCount
            result.StandardCount = loadCount

            '終了ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
               , "{0}.{1} {2} OUT:COUNT = {3}" _
               , Me.GetType.ToString _
               , System.Reflection.MethodBase.GetCurrentMethod.Name _
               , "End" _
               , result.DataTable.Rows.Count))
        Else
            result.SearchResult = 0
            result.DataTable = Nothing
            result.ResultStartRow = searchStartRow      '初期値(0)
            result.ResultEndRow = searchEndRow          '初期値(0)
            result.ResultCustomerCount = customerCount  '初期値(0)
            result.StandardCount = loadCount

            '終了ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
               , "{0}.{1} {2} OUT:COUNT = 0" _
               , Me.GetType.ToString _
               , System.Reflection.MethodBase.GetCurrentMethod.Name _
               , "End"))
        End If

        Return result
    End Function

    ''' <summary>
    ''' 整備種類情報を、チップ情報表示用データテーブルに設定する
    ''' </summary>
    ''' <param name="ds">返却用データセット</param>
    ''' <param name="dlrCD">販売店コード</param>
    ''' <param name="strCD">店舗コード</param>
    ''' <remarks></remarks>
    Public Sub SetSvcData(ByVal ds As SC3240501DataSet, _
                               ByVal dlrCD As String, _
                               ByVal strCD As String)

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True)

        Dim svcClassDt As SC3240501DataSet.SC3240501SvcClassListDataTable
        Using ta As New SC3240501DataTableAdapter
            '整備種類
            svcClassDt = ta.GetSvcClassList(dlrCD, strCD)
        End Using

        '表示用データテーブル・データ行
        Dim dispSvcClassDt As SC3240501DataSet.SC3240501SvcClassListDataTable = ds.SC3240501SvcClassList

        '整備種類情報の事前バインディング
        Dim svcIdTime As DataColumn = svcClassDt.SVCID_TIMEColumn
        Dim svcClassName As DataColumn = svcClassDt.SVC_CLASS_NAMEColumn
        Dim svcClassType As DataColumn = svcClassDt.SVC_CLASS_TYPEColumn
        Dim carWashNeedFlg As DataColumn = svcClassDt.CARWASH_NEED_FLGColumn

        For i = 0 To svcClassDt.Rows.Count - 1

            Dim selectSvcClassDr As SC3240501DataSet.SC3240501SvcClassListRow = DirectCast(svcClassDt.Rows(i), SC3240501DataSet.SC3240501SvcClassListRow)

            Dim dispSvcClassRow As SC3240501DataSet.SC3240501SvcClassListRow = dispSvcClassDt.NewSC3240501SvcClassListRow

            'サービス分類ID,標準作業時間 + サービス分類区分 + 洗車必要フラグ
            dispSvcClassRow.SVCID_TIME = String.Format(CultureInfo.CurrentCulture _
                                                       , "{0},{1},{2}" _
                                                       , CType(selectSvcClassDr(svcIdTime), String) _
                                                       , Me.ConvertDbNullToEmpty(selectSvcClassDr(svcClassType)) _
                                                       , Me.ConvertDbNullToEmpty(selectSvcClassDr(carWashNeedFlg)))
            'サービス分類名称
            dispSvcClassRow.SVC_CLASS_NAME = Me.ConvertDbNullToEmpty(selectSvcClassDr(svcClassName))

            '行を追加
            ds.SC3240501SvcClassList.AddSC3240501SvcClassListRow(dispSvcClassRow)
        Next

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, False)

    End Sub

    ''' <summary>
    ''' 商品情報取得
    ''' </summary>
    ''' <param name="arg">引数クラスオブジェクト</param>
    ''' <returns></returns>
    Public Function GetChangeMercInfo(ByVal arg As NewChipCallBackArgumentClass) As SC3240501DataSet.SC3240501MercListDataTable

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True, _
                      "[DlrCD:{0}][StrCD:{1}][SvcClassId:{2}]", _
                      arg.DlrCD, arg.StrCD, arg.SvcClassId)

        Using ta As New SC3240501DataTableAdapter
            Dim mercDt As SC3240501DataSet.SC3240501MercListDataTable = ta.GetMercList(arg.DlrCD, arg.StrCD, arg.SvcClassId)

            OutputInfoLog(MethodBase.GetCurrentMethod.Name, False)

            Return mercDt
        End Using

    End Function

    ''' <summary>
    ''' 敬称情報を、チップ情報表示用データテーブルに設定する
    ''' </summary>
    ''' <param name="ds">返却用データセット</param>
    ''' <remarks></remarks>
    Public Sub SetNameTitleData(ByVal ds As SC3240501DataSet)

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True)

        Dim nameTitleClassDt As SC3240501DataSet.SC3240501NameTitleListDataTable
        Using ta As New SC3240501DataTableAdapter
            '敬称
            nameTitleClassDt = ta.GetSvcNameTitle()
        End Using

        '表示用データテーブル・データ行
        Dim dispNameTitleClassDt As SC3240501DataSet.SC3240501NameTitleListDataTable = ds.SC3240501NameTitleList

        '敬称情報の事前バインディング
        Dim nameTitleName As DataColumn = nameTitleClassDt.NAMETITLE_NAMEColumn
        Dim nameTitleCd As DataColumn = nameTitleClassDt.NAMETITLE_CDColumn

        For i = 0 To nameTitleClassDt.Rows.Count - 1

            Dim selectNameTitleClassDr As SC3240501DataSet.SC3240501NameTitleListRow = DirectCast(nameTitleClassDt.Rows(i), SC3240501DataSet.SC3240501NameTitleListRow)

            Dim dispNameTitleClassRow As SC3240501DataSet.SC3240501NameTitleListRow = dispNameTitleClassDt.NewSC3240501NameTitleListRow

            dispNameTitleClassRow.NAMETITLE_NAME = Me.ConvertDbNullToEmpty(selectNameTitleClassDr(nameTitleName))   '敬称名称
            dispNameTitleClassRow.NAMETITLE_CD = Me.ConvertDbNullToEmpty(selectNameTitleClassDr(nameTitleCd))       '敬称コード

            '行を追加
            ds.SC3240501NameTitleList.AddSC3240501NameTitleListRow(dispNameTitleClassRow)
        Next

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, False)

    End Sub

    ''' <summary>
    ''' SA情報の取得
    ''' </summary>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inStoreCode">店舗コード</param>
    ''' <returns>SA情報</returns>
    ''' <remarks></remarks>
    Public Function GetAcknowledgeStaffList(ByVal inDealerCode As String, _
                                            ByVal inStoreCode As String) _
                                        As IC3810601DataSet.AcknowledgeStaffListDataTable
        '開始ログ
        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True, "[inDealerCode:{0}][inStoreCode:{1}]", inDealerCode, inStoreCode)

        Dim bl As IC3810601BusinessLogic = Nothing
        Dim dt As IC3810601DataSet.AcknowledgeStaffListDataTable = Nothing

        Dim operationCodeList As New List(Of Long)
        operationCodeList.Add(9)

        Try
            bl = New IC3810601BusinessLogic
            dt = bl.GetAcknowledgeStaffList(inDealerCode, inStoreCode, operationCodeList)
        Finally
            If bl IsNot Nothing Then
                bl.Dispose()
                bl = Nothing
            End If
        End Try

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[dt:{0}]", dt)
        Return dt

    End Function

#End Region


#Region "チップの登録(WebService)"

    ''' <summary>
    ''' データを登録する（予約情報更新WebServiceを利用してDB更新）
    ''' </summary>
    ''' <param name="argument">引数クラスオブジェクト</param>
    ''' <param name="objStaffContext">スタッフ情報</param>
    ''' <param name="svcInId">サービス入庫ID</param>
    ''' <param name="startPlanTime">開始予定時間</param>
    ''' <param name="finishPlanTime">終了予定時間</param>
    ''' <param name="dtNow">更新日時</param>
    ''' <returns></returns>
    <EnableCommit()>
    Public Function InsertDataUsingWebService(
                                        ByVal argument As NewChipCallBackArgumentClass, _
                                        ByVal objStaffContext As StaffContext, _
                                        ByRef svcInId As Decimal, _
                                        ByVal startPlanTime As Date, _
                                        ByVal finishPlanTime As Date, _
                                        ByVal dtNow As Date) As Long
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
           , "{0}.{1} {2}" _
           , Me.GetType.ToString _
           , System.Reflection.MethodBase.GetCurrentMethod.Name _
           , "Start"))

        'ストールロックフラグ
        Dim isStallLock As Boolean = False
        Dim result As Long
        Using clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic
            Try
                'ストールロック
                result = clsTabletSMBCommonClass.LockStall(CType(argument.StallId, Decimal), argument.NewChipDispStartDate, objStaffContext.Account, dtNow, NEWCHIP_PROGRAMID)
                If result <> TabletSMBCommonClassBusinessLogic.ActionResult.Success Then
                    Me.Rollback = True
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} {2} LockStallError" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , "End"))
                    Return result
                End If
                isStallLock = True

                '新規チップ作成
                result = InsertNewChip(argument, objStaffContext, svcInId, startPlanTime, finishPlanTime)

                '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 START

                ''エラーコードを戻す
                'If result <> TabletSMBCommonClassBusinessLogic.ActionResult.Success Then
                '    Me.Rollback = True
                '    Logger.Error(String.Format(CultureInfo.CurrentCulture _
                '       , "{0}.{1} {2} InsertDataUsingWebService failed." _
                '       , Me.GetType.ToString _
                '       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                '       , "End"))

                '    Return result
                'End If

                If result <> TabletSMBCommonClassBusinessLogic.ActionResult.Success _
                And result <> TabletSMBCommonClassBusinessLogic.ActionResult.WarningOmitDmsError Then
                    '新規チップ作成結果が
                    '　成功(0)でない場合、かつ
                    '　DMS除外エラーの警告(-9000)でない場合

                    Me.Rollback = True

                    Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                                             , "{0}.{1} {2} InsertDataUsingWebService failed." _
                                             , Me.GetType.ToString _
                                             , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                             , "End"))

                    Return result

                End If

                '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 END

            Catch ex As OracleExceptionEx When ex.Number = 1013
                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2} DBTimeOutError." _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , "End"))
                Return TabletSMBCommonClassBusinessLogic.ActionResult.DBTimeOutError
            Finally
                If isStallLock Then
                    'ストールロック解除
                    clsTabletSMBCommonClass.LockStallReset(CType(argument.StallId, Decimal), argument.NewChipDispStartDate, objStaffContext.Account, dtNow, NEWCHIP_PROGRAMID)
                End If
            End Try

        End Using
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
           , "{0}.{1} {2}" _
           , Me.GetType.ToString _
           , System.Reflection.MethodBase.GetCurrentMethod.Name _
           , "End"))

        '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 START

        'Return TabletSMBCommonClassBusinessLogic.ActionResult.Success
        Return result

        '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 END

    End Function

    ''' <summary>
    ''' 新規チップ作成
    ''' </summary>
    ''' <param name="argument">引数クラスオブジェクト</param>
    ''' <param name="objStaffContext">スタッフ情報</param>
    ''' <param name="svcInId">サービス入庫ID</param>
    ''' <param name="startPlanTime">開始予定時間</param>
    ''' <param name="finishPlanTime">終了予定時間</param>
    ''' <returns>操作結果</returns>
    ''' <remarks></remarks>
    Public Function InsertNewChip(ByVal argument As NewChipCallBackArgumentClass, _
                                  ByVal objStaffContext As StaffContext, _
                                  ByRef svcInId As Decimal, _
                                  ByVal startPlanTime As Date, _
                                  ByVal finishPlanTime As Date) As Long
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
           , "{0}.{1} {2}" _
           , Me.GetType.ToString _
           , System.Reflection.MethodBase.GetCurrentMethod.Name _
           , "Start"))

        Dim stallUseId As Decimal       'ストール利用ID
        Dim jobDtlId As Decimal         '作業内容ID

        '整備種類DropDownListで選択されている値の、サービス分類IDを取得
        Dim svcClassId As Decimal = CType(argument.SvcClassId, Decimal)
        Dim svcClassCD As String = String.Empty
        If svcClassId > 0 Then
            '整備種類が指定されている場合

            Using ta As New SC3240501DataTableAdapter
                'サービス分類IDを条件にサービス分類コードを取得
                svcClassCD = ta.GetSvcClassCD(svcClassId)
            End Using
        End If

        'WebService用XMLを構築
        Dim xmlclass As New SMBCommonClassBusinessLogic.XmlDocumentClass
        xmlclass = StructWebServiceXml(argument, objStaffContext, svcClassCD, startPlanTime, finishPlanTime)

        Using commbiz As New SMBCommonClassBusinessLogic
            Dim drWebServiceResult As SMBCommonClassDataSet.WebServiceResultRow = commbiz.CallReserveWebService(xmlclass)
            If drWebServiceResult.RESULTCODE <> 0 Then
                '予期せぬエラー
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2} InsertNewChip call webservice failed. RESULTCODE={3}." _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , "End" _
                   , drWebServiceResult.RESULTCODE))
                Return ActionResult.ExceptionError
            End If
            svcInId = drWebServiceResult.SVCIN_ID
            stallUseId = drWebServiceResult.STALL_USE_ID
            jobDtlId = drWebServiceResult.JOB_DTL_ID
        End Using

        '更新後のチップステータスを取得する ※更新前のステータスも同値を使用する)
        Dim crntChipStatus As String = String.Empty
        Using clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic
            crntChipStatus = clsTabletSMBCommonClass.JudgeChipStatus(stallUseId)
        End Using

        'Walk-inフラグ=「1:飛び込み」の場合、予約ステータスを「1:本予約」にする
        Dim resvStatus As String = "0"
        If argument.RezFlg.Equals("1") Then
            resvStatus = "1"
        End If

        '●予約送信を行う
        Dim retVal As Integer
        Using biz3800903 As New IC3800903BusinessLogic
            retVal = biz3800903.SendReserveInfo(svcInId, _
                                                jobDtlId, _
                                                stallUseId, _
                                                crntChipStatus, _
                                                crntChipStatus, _
                                                resvStatus, _
                                                NEWCHIP_PROGRAMID, _
                                                Nothing, _
                                                True)
        End Using


        If retVal <> ActionResult.Success Then

            '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 START

            'Me.Rollback = True
            'Logger.Error(String.Format(CultureInfo.CurrentCulture _
            '                           , "{0}.{1} biz3800903.SendReserveInfo svcInId={2} jobDtlId={3} stallUseId={4} crntChipStatus={5} crntChipStatus={6} resvStatus={7} MY_PROGRAMID={8}" _
            '                           , Me.GetType.ToString _
            '                           , System.Reflection.MethodBase.GetCurrentMethod.Name _
            '                           , svcInId, jobDtlId, stallUseId, crntChipStatus, crntChipStatus, resvStatus, NEWCHIP_PROGRAMID))
            'Return ActionResult.DmsLinkageError

            If retVal = ActionResult.WarningOmitDmsError Then
                '予約送信の結果が-9000(DMS除外エラーの警告)の場合

                Return ActionResult.WarningOmitDmsError

            Else
                '予約送信の結果が上記以外の場合

                Me.Rollback = True

                '2015/10/08 TM 皆川 タブレットSMBのチップ移動時の連携送信メッセージ詳細化 START

                '戻り値
                Dim returnValue As Long

                If ActionResult.IC3800903ResultRangeLower <= retVal _
                AndAlso retVal <= ActionResult.IC3800903ResultRangeUpper Then
                    '予約連携処理結果コードが8000以上かつ8999以下の場合
                    '予約連携処理結果コードを返却
                    returnValue = retVal

                Else
                    '上記以外の場合
                    '「15：他システムとの連携エラー」を返却
                    returnValue = ActionResult.DmsLinkageError

                End If

                '2015/10/08 TM 皆川 タブレットSMBのチップ移動時の連携送信メッセージ詳細化 END

                Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                                           , "{0}.{1} biz3800903.SendReserveInfo svcInId={2} jobDtlId={3} stallUseId={4} crntChipStatus={5} crntChipStatus={6} resvStatus={7} MY_PROGRAMID={8}" _
                                           , Me.GetType.ToString _
                                           , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                           , svcInId, jobDtlId, stallUseId, crntChipStatus, crntChipStatus, resvStatus, NEWCHIP_PROGRAMID))

                '2015/10/08 TM 皆川 タブレットSMBのチップ移動時の連携送信メッセージ詳細化 START
                'Return ActionResult.DmsLinkageError
                Return returnValue
                '2015/10/08 TM 皆川 タブレットSMBのチップ移動時の連携送信メッセージ詳細化 END

            End If

            '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 END

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
           , "{0}.{1} {2}" _
           , Me.GetType.ToString _
           , System.Reflection.MethodBase.GetCurrentMethod.Name _
           , "End"))

        Return ActionResult.Success

    End Function

    ''' <summary>
    ''' WebService用XMLを構築
    ''' </summary>
    ''' <param name="argument">引数クラスオブジェクト</param>
    ''' <param name="objStaffContext">スタッフ情報</param>
    ''' <param name="svcClassCD">サービス分類コード</param>
    ''' <param name="startPlanTime">開始予定時間</param>
    ''' <param name="finishPlanTime">終了予定時間</param>
    ''' <returns>構築したXMLドキュメント</returns>
    ''' <remarks></remarks>
    Private Function StructWebServiceXml(ByVal argument As NewChipCallBackArgumentClass, _
                                           ByVal objStaffContext As StaffContext, _
                                           ByVal svcClassCD As String, _
                                           ByVal startPlanTime As Date, _
                                           ByVal finishPlanTime As Date) As SMBCommonClassBusinessLogic.XmlDocumentClass


        Logger.Info(String.Format(CultureInfo.CurrentCulture _
           , "{0}.{1} {2}" _
           , Me.GetType.ToString _
           , System.Reflection.MethodBase.GetCurrentMethod.Name _
           , "Start"))

        Dim dtNow As Date = DateTimeFunc.Now(objStaffContext.DlrCD)

        'WebServiceを呼ぶためXML作成
        Dim xmlclass As New SMBCommonClassBusinessLogic.XmlDocumentClass

        'headタグの構築
        '送信日付
        Using smbCommonBiz As New ServiceCommonClassBusinessLogic
            Dim dateFormat As String = smbCommonBiz.GetSystemSettingValueBySettingName(SYSDATEFORMAT)
            If String.IsNullOrEmpty(dateFormat) Then
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2} ErrCode:Failed to get System Date Format." _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , "End"))
                'システム設定値から取得できない場合、固定値とする
                xmlclass.Head.TransmissionDate = String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd HH:mm:ss}", dtNow)
            Else
                'システム設定値から変換したDateFormatで設定
                xmlclass.Head.TransmissionDate = dtNow.ToString(dateFormat, CultureInfo.InvariantCulture)
            End If
        End Using

        'Commonタグの構築
        '販売店コード
        xmlclass.Detail.Common.DealerCode = objStaffContext.DlrCD
        '店舗コード
        xmlclass.Detail.Common.BranchCode = objStaffContext.BrnCD
        'スタッフコード
        xmlclass.Detail.Common.StaffCode = Nothing

        'Reserve_Customerタグの構築
        StructWebServiceXmlReserveCustomer(argument, xmlclass)

        'Reserve_VehicleInformationタグの構築
        StructWebServiceXmlReserveVehicleInformation(argument, xmlclass)

        'Detail_ReserveInformation_ReserveServiceInformationタグの構築
        StructWebServiceXmlDetailReserveInformation(argument, xmlclass, svcClassCD, startPlanTime, finishPlanTime)

        'シーケンスナンバー
        xmlclass.Detail.ReserveInformation.SeqNo = Nothing
        '予約ID
        xmlclass.Detail.ReserveInformation.ReserveId = Nothing
        '基幹予約ID
        xmlclass.Detail.ReserveInformation.BasReserveId = Nothing
        '管理予約ID
        xmlclass.Detail.ReserveInformation.PReserveId = Nothing
        'Walk-inフラグ(予約有無)
        If Not String.IsNullOrWhiteSpace(argument.RezFlg) Then
            xmlclass.Detail.ReserveInformation.WalkIn = argument.RezFlg
            If argument.RezFlg.Equals("1") Then
                '予約ステータス(Walk-inフラグ「1:飛び込み」の場合、本予約に設定)
                xmlclass.Detail.ReserveInformation.Status = "1"
            Else
                xmlclass.Detail.ReserveInformation.Status = Nothing
            End If
        End If
        'SMS送信可否フラグ
        xmlclass.Detail.ReserveInformation.SmsFlg = Nothing
        'キャンセルフラグ
        xmlclass.Detail.ReserveInformation.CancelFlg = Nothing
        '未来店客フラグ
        xmlclass.Detail.ReserveInformation.NoShowFlg = Nothing
        '着工指示フラグ
        xmlclass.Detail.ReserveInformation.WorkOrderFlg = Nothing
        '受付担当予定者
        If Not String.IsNullOrEmpty(argument.SACode) _
        AndAlso Not argument.SACode.Equals("0") Then
            xmlclass.Detail.ReserveInformation.AcountPlan = argument.SACode
        Else
            xmlclass.Detail.ReserveInformation.AcountPlan = Nothing
        End If
        'MEMO
        If String.IsNullOrEmpty(argument.Order) Then
            xmlclass.Detail.ReserveInformation.Memo = Nothing
        Else
            xmlclass.Detail.ReserveInformation.Memo = argument.Order
        End If
        '更新オペレータ
        xmlclass.Detail.ReserveInformation.UpdateAccount = objStaffContext.Account
        'R_O
        xmlclass.Detail.ReserveInformation.OerderNo = Nothing

        'ROWロックバージョン
        xmlclass.Detail.ReserveInformation.RowLockVersion = Nothing

        '検索済みの場合（未検索の場合は、新規顧客を作成する為、設定しない）
        If Not String.IsNullOrWhiteSpace(argument.SearchedFlg) _
        AndAlso argument.SearchedFlg.Equals("1") Then
            '顧客車両区分
            xmlclass.Detail.ReserveInformation.CstVclType = argument.CstVclType
        Else
            xmlclass.Detail.ReserveInformation.CstVclType = Nothing
        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
           , "{0}.{1} {2}" _
           , Me.GetType.ToString _
           , System.Reflection.MethodBase.GetCurrentMethod.Name _
           , "End"))

        Return xmlclass
    End Function

    ''' <summary>
    ''' ReserveCustomer用XMLを構築
    ''' </summary>
    ''' <param name="argument">引数クラスオブジェクト</param>
    ''' <param name="xmlclass">構築したXMLドキュメント</param>
    ''' <remarks></remarks>
    Private Sub StructWebServiceXmlReserveCustomer(ByVal argument As NewChipCallBackArgumentClass, _
                                           xmlclass As SMBCommonClassBusinessLogic.XmlDocumentClass)

        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '   , "{0}.{1} {2}" _
        '   , Me.GetType.ToString _
        '   , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '   , "Start"))

        If Not String.IsNullOrWhiteSpace(argument.SearchedFlg) Then
            '検索済みの場合（未検索の場合は、新規顧客を作成する為、設定しない）
            If argument.SearchedFlg.Equals("1") Then
                '顧客ID
                Dim cstId As String = argument.CstId.ToString(CultureInfo.InvariantCulture)
                If Not String.IsNullOrWhiteSpace(cstId) Then
                    xmlclass.Detail.ReserveInformation.ReserveCustomerInformation.CstId = cstId
                End If
            End If
        End If

        '顧客コード
        If String.IsNullOrEmpty(argument.DmsCstCode.Trim()) Then
            xmlclass.Detail.ReserveInformation.ReserveCustomerInformation.CustCode = Nothing
        Else
            xmlclass.Detail.ReserveInformation.ReserveCustomerInformation.CustCode = TrimBeforeAtmark(argument.DmsCstCode)
        End If
        '氏名
        If String.IsNullOrEmpty(argument.CstName.Trim()) Then
            xmlclass.Detail.ReserveInformation.ReserveCustomerInformation.CustomerName = Nothing
        Else
            xmlclass.Detail.ReserveInformation.ReserveCustomerInformation.CustomerName = argument.CstName
        End If
        '電話番号
        If String.IsNullOrEmpty(argument.Home.Trim()) Then
            xmlclass.Detail.ReserveInformation.ReserveCustomerInformation.TelNo = Nothing
        Else
            xmlclass.Detail.ReserveInformation.ReserveCustomerInformation.TelNo = argument.Home
        End If
        '携帯番号
        If String.IsNullOrEmpty(argument.Mobile.Trim()) Then
            xmlclass.Detail.ReserveInformation.ReserveCustomerInformation.Mobile = Nothing
        Else
            xmlclass.Detail.ReserveInformation.ReserveCustomerInformation.Mobile = argument.Mobile
        End If
        '住所
        xmlclass.Detail.ReserveInformation.ReserveCustomerInformation.Address = argument.CstAddress
        '郵便番号
        xmlclass.Detail.ReserveInformation.ReserveCustomerInformation.ZipCode = Nothing
        'E-MAILアドレス
        xmlclass.Detail.ReserveInformation.ReserveCustomerInformation.Email = Nothing
        '敬称コード
        xmlclass.Detail.ReserveInformation.ReserveCustomerInformation.NameTitleCD = argument.NameTitleCD
        '敬称
        xmlclass.Detail.ReserveInformation.ReserveCustomerInformation.NameTitleName = argument.NameTitleName

        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '   , "{0}.{1} {2}" _
        '   , Me.GetType.ToString _
        '   , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '   , "End"))

    End Sub


    ''' <summary>
    ''' ReserveVehicleInformation用XMLを構築
    ''' </summary>
    ''' <param name="argument">引数クラスオブジェクト</param>
    ''' <param name="xmlclass">構築したXMLドキュメント</param>
    ''' <remarks></remarks>
    Private Sub StructWebServiceXmlReserveVehicleInformation(ByVal argument As NewChipCallBackArgumentClass, _
                                           xmlclass As SMBCommonClassBusinessLogic.XmlDocumentClass)

        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '   , "{0}.{1} {2}" _
        '   , Me.GetType.ToString _
        '   , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '   , "Start"))

        If Not String.IsNullOrWhiteSpace(argument.SearchedFlg) Then
            '検索済みの場合（未検索の場合は、新規顧客を作成する為、設定しない）
            If argument.SearchedFlg.Equals("1") Then
                '車両ID
                Dim vclId As String = argument.VclId.ToString(CultureInfo.InvariantCulture)
                If Not String.IsNullOrWhiteSpace(vclId) Then
                    xmlclass.Detail.ReserveInformation.ReserveVehicleInformation.VclId = vclId
                End If
            End If
        End If

        '登録ナンバー
        If String.IsNullOrEmpty(argument.RegNo.Trim()) Then
            xmlclass.Detail.ReserveInformation.ReserveVehicleInformation.VehicleNo = Nothing
        Else
            xmlclass.Detail.ReserveInformation.ReserveVehicleInformation.VehicleNo = argument.RegNo
        End If
        'VIN
        If String.IsNullOrEmpty(argument.Vin.Trim()) Then
            xmlclass.Detail.ReserveInformation.ReserveVehicleInformation.Vin = Nothing
        Else
            xmlclass.Detail.ReserveInformation.ReserveVehicleInformation.Vin = argument.Vin
        End If
        '車両メーカー区分
        xmlclass.Detail.ReserveInformation.ReserveVehicleInformation.MakerCode = Nothing
        '車名コード
        xmlclass.Detail.ReserveInformation.ReserveVehicleInformation.SeriesCode = Nothing
        '車名
        If String.IsNullOrEmpty(argument.Vehicle.Trim()) Then
            xmlclass.Detail.ReserveInformation.ReserveVehicleInformation.SeriesName = Nothing
        Else
            xmlclass.Detail.ReserveInformation.ReserveVehicleInformation.SeriesName = argument.Vehicle
        End If
        '型式
        xmlclass.Detail.ReserveInformation.ReserveVehicleInformation.BaseType = Nothing
        '走行距離
        xmlclass.Detail.ReserveInformation.ReserveVehicleInformation.Mileage = Nothing

        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '   , "{0}.{1} {2}" _
        '   , Me.GetType.ToString _
        '   , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '   , "End"))

    End Sub


    ''' <summary>
    ''' DetailReserveInformation用XMLを構築
    ''' </summary>
    ''' <param name="argument">引数クラスオブジェクト</param>
    ''' <param name="xmlclass">構築したXMLドキュメント</param>
    ''' <param name="svcClassCD">サービス分類コード</param>
    ''' <param name="startPlanTime">開始予定時間</param>
    ''' <param name="finishPlanTime">終了予定時間</param>
    ''' <remarks></remarks>
    Private Sub StructWebServiceXmlDetailReserveInformation(ByVal argument As NewChipCallBackArgumentClass, _
                                           xmlclass As SMBCommonClassBusinessLogic.XmlDocumentClass, _
                                           ByVal svcClassCD As String, _
                                           ByVal startPlanTime As Date, _
                                           ByVal finishPlanTime As Date)

        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '   , "{0}.{1} {2}" _
        '   , Me.GetType.ToString _
        '   , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '   , "Start"))

        'ストールID
        Dim stallId As String = argument.StallId.ToString(CultureInfo.InvariantCulture)
        If Not String.IsNullOrWhiteSpace(stallId) Then
            xmlclass.Detail.ReserveInformation.ReserveServiceInformation.StallId = stallId
        End If
        '作業開始予定日時
        Dim startDateTime As String = String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd HH:mm:ss}", startPlanTime)
        If Not String.IsNullOrWhiteSpace(startDateTime) Then
            xmlclass.Detail.ReserveInformation.ReserveServiceInformation.StartTime = startDateTime
        End If
        '作業終了予定日時
        Dim serviceWorkEndDateTime As String = String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd HH:mm:ss}", finishPlanTime)
        If Not String.IsNullOrWhiteSpace(serviceWorkEndDateTime) Then
            xmlclass.Detail.ReserveInformation.ReserveServiceInformation.EndTime = serviceWorkEndDateTime
        End If
        '予定作業時間
        Dim serviceWorkTime As String = argument.WorkTime.ToString(CultureInfo.InvariantCulture)
        If Not String.IsNullOrWhiteSpace(serviceWorkTime) Then
            xmlclass.Detail.ReserveInformation.ReserveServiceInformation.WorkTime = serviceWorkTime
        End If
        'ストール休憩フラグ (0:休憩を取る、1:休憩を取らない)
        If argument.RestFlg = 0 Then
            xmlclass.Detail.ReserveInformation.ReserveServiceInformation.BreakFlg = "1"
        Else
            xmlclass.Detail.ReserveInformation.ReserveServiceInformation.BreakFlg = "0"
        End If
        '検査フラグ (0:無し、1:検査有り)
        If Not String.IsNullOrWhiteSpace(argument.CompleteExaminationFlg) Then
            xmlclass.Detail.ReserveInformation.ReserveServiceInformation.InspectionFlg = argument.CompleteExaminationFlg
        End If
        '洗車フラグ (0:無し、1:洗車有り)
        If Not String.IsNullOrWhiteSpace(argument.CarWashFlg) Then
            xmlclass.Detail.ReserveInformation.ReserveServiceInformation.WashFlg = argument.CarWashFlg
        End If

        '受付納車区分
        If Not String.IsNullOrWhiteSpace(argument.WaitingFlg) Then
            xmlclass.Detail.ReserveInformation.ReserveServiceInformation.ReserveReception = argument.WaitingFlg
        End If
        '引取希望日時
        Dim scheSvcinDatetime As String = String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd HH:mm:ss}", argument.VisitPlanTime)
        If Not String.IsNullOrWhiteSpace(scheSvcinDatetime) Then
            xmlclass.Detail.ReserveInformation.ReserveServiceInformation.ReservePickDate = scheSvcinDatetime
        Else
            '受付納車区分のコードが（1、2、3、4）の場合は「引取希望日時」指定が必須。（0）の場合はOptional
            If Not argument.WaitingFlg.Equals(DeliTypeWaiting) Then
                xmlclass.Detail.ReserveInformation.ReserveServiceInformation.ReservePickDate = String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd HH:mm:ss}", DefaultDateTimeValueGet())
            End If
        End If
        '納車希望日時
        Dim scheDeliDatetime As String = String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd HH:mm:ss}", argument.DeriveredPlanTime)
        If Not String.IsNullOrWhiteSpace(scheDeliDatetime) Then
            xmlclass.Detail.ReserveInformation.ReserveServiceInformation.ReserveDeliDate = scheDeliDatetime
        Else
            '受付納車区分のコードが（1、2、3、4）の場合は「納車希望日時」指定が必須。（0）の場合はOptional
            If Not argument.WaitingFlg.Equals(DeliTypeWaiting) Then
                xmlclass.Detail.ReserveInformation.ReserveServiceInformation.ReserveDeliDate = String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd HH:mm:ss}", DefaultDateTimeValueGet())
            End If
        End If
        '引取場所
        xmlclass.Detail.ReserveInformation.ReserveServiceInformation.ReservePickLoc = Nothing
        '引取所要時間
        xmlclass.Detail.ReserveInformation.ReserveServiceInformation.ReservePickTime = Nothing
        '納車場所
        xmlclass.Detail.ReserveInformation.ReserveServiceInformation.ReserveDeliLoc = Nothing
        '納車所要時間
        xmlclass.Detail.ReserveInformation.ReserveServiceInformation.ReserveDeliTime = Nothing
        '商品コード
        If argument.MercId.Equals(0) Then
            xmlclass.Detail.ReserveInformation.ReserveServiceInformation.MerchandiseCode = Nothing
        Else
            xmlclass.Detail.ReserveInformation.ReserveServiceInformation.MerchandiseCode = CType(argument.MercId, String)
        End If
        '整備コード
        xmlclass.Detail.ReserveInformation.ReserveServiceInformation.MntnCode = Nothing
        'サービスコード
        If Not String.IsNullOrEmpty(svcClassCD) AndAlso Not svcClassCD.Equals("0") Then
            xmlclass.Detail.ReserveInformation.ReserveServiceInformation.ServiceCode = svcClassCD
        Else
            xmlclass.Detail.ReserveInformation.ReserveServiceInformation.ServiceCode = Nothing
        End If

        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '   , "{0}.{1} {2}" _
        '   , Me.GetType.ToString _
        '   , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '   , "End"))

    End Sub


    ''' <summary>
    ''' Update操作をした後のチップ情報を取得する
    ''' </summary>
    ''' <param name="dlrCD">販売店コード</param>
    ''' <param name="strCD">店舗コード</param>
    ''' <param name="dtNow">更新日時</param>
    ''' <param name="svcInId">サービス入庫ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetStallChipInfoFromSvcInId(ByVal dlrCD As String, _
                                                ByVal strCD As String, _
                                                ByVal dtNow As Date, _
                                                ByVal svcInId As Decimal) As String

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True, _
              "[dlrCD:{0}][strCD:{1}][dtNow:{2}][svcInId:{3}]", _
              dlrCD, strCD, dtNow, svcInId)

        Dim dtChipInfo As TabletSMBCommonClassDataSet.TabletSmbCommonClassStallChipInfoDataTable = Nothing

        'サービス入庫IDをリストにセット
        Dim svcInIdList As New List(Of Decimal)
        svcInIdList.Add(svcInId)

        Using clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic
            dtChipInfo = clsTabletSMBCommonClass.GetStallChipBySvcinId(dlrCD, strCD, dtNow, svcInIdList)
        End Using

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, False)

        Return NewChipDataTableToJson(dtChipInfo)
    End Function

    '2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応 START
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
                                               ByVal inLastRefreshTime As Date) As String

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, _
                      True, _
                      "[inDlrCode:{0}][inBrnCode:{1}][inShowDate:{2}][inLastRefreshTime:{3}]", _
                      inDlrCode, _
                      inBrnCode, _
                      inShowDate, _
                      inLastRefreshTime)


        Using clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic

            Dim dtChipInfo As TabletSMBCommonClassDataSet.TabletSmbCommonClassStallChipInfoDataTable = _
                clsTabletSMBCommonClass.GetStallChipAfterOperation(inDlrCode, _
                                                                   inBrnCode, _
                                                                   inShowDate, _
                                                                   inLastRefreshTime)

            OutputInfoLog(MethodBase.GetCurrentMethod.Name, False)

            Return NewChipDataTableToJson(dtChipInfo)

        End Using

    End Function

    '2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応 END

#End Region


#Region "Check処理"

    ''' <summary>
    ''' 登録しようとしたチップが休憩・使用不可チップと重複しないかチェックする
    ''' </summary>
    ''' <param name="arg">引数クラスオブジェクト</param>
    ''' <returns>True:衝突しない/False:衝突する</returns>
    ''' <remarks></remarks>
    Public Function CheckRestOrUnavailableChipCollision(ByVal arg As NewChipCallBackArgumentClass) As Boolean

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True, _
                      "[ProcWorkTime:{0}][StallStartTime:{1}][StallEndTime:{2}][StallId:{3}][StartPlanTime:{4}]", _
                      arg.WorkTime, arg.StallStartTime, arg.StallEndTime, arg.StallId, arg.StartPlanTime)

        Dim clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic

        Try
            Dim rtnVal As Boolean = True

            '作業時間
            Dim workTime As Long

            'ストール開始日時、時間
            Dim dtStartDate As Date
            Dim stallStartTime As TimeSpan

            'ストール終了日時、時間
            Dim dtEndDate As Date
            Dim stallEndTime As TimeSpan

            '作業開始実績日時が入ってなければ、予定時間をセット
            workTime = CLng(arg.WorkTime)

            '現在開いている日付の営業開始時間と営業終了時間を取得する
            If arg.StallStartTime <> "" Then
                stallStartTime = New TimeSpan(CInt(arg.StallStartTime.Substring(0, 2)), CInt(arg.StallStartTime.Substring(3, 2)), 0)
            End If
            If arg.StallEndTime <> "" Then
                stallEndTime = New TimeSpan(CInt(arg.StallEndTime.Substring(0, 2)), CInt(arg.StallEndTime.Substring(3, 2)), 0)
            End If

            'arg.ShowDateがあれば、営業日時を計算する
            If CType(arg.ShowDate, Date) <> Date.MinValue Then
                dtStartDate = CType(arg.ShowDate, Date)
                dtStartDate = dtStartDate.AddHours(stallStartTime.Hours).AddMinutes(stallStartTime.Minutes)

                dtEndDate = CType(arg.ShowDate, Date)
                dtEndDate = dtEndDate.AddHours(stallEndTime.Hours).AddMinutes(stallEndTime.Minutes)
            End If

            '休憩エリア確認
            Dim hasRestTimeInServiceTime As Boolean = clsTabletSMBCommonClass.HasRestTimeInServiceTime(dtStartDate, _
                                                                                                       dtEndDate,
                                                                                                       CType(arg.StallId, Decimal), _
                                                                                                       CDate(arg.StartPlanTime), _
                                                                                                       workTime, _
                                                                                                       True)
            '休憩または使用不可エリアと重複する場合
            If hasRestTimeInServiceTime Then
                'False:衝突する
                rtnVal = False
            End If

            OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[rtnVal:{0}]", rtnVal)

            Return rtnVal

        Finally
            If clsTabletSMBCommonClass IsNot Nothing Then
                clsTabletSMBCommonClass.Dispose()
                clsTabletSMBCommonClass = Nothing
            End If
        End Try

    End Function

    ''' <summary>
    ''' 登録しようとしたチップが他のチップと衝突しないかチェックする
    ''' </summary>
    ''' <param name="arg">引数クラスオブジェクト</param>
    ''' <param name="finishPlanTime">更新用の予定終了日時</param>
    ''' <param name="dtNow">更新日時</param>
    ''' <returns>True:衝突しない/False:衝突する</returns>
    ''' <remarks></remarks>
    Public Function CheckChipCollision(ByVal arg As NewChipCallBackArgumentClass, _
                                       ByVal finishPlanTime As Date, _
                                       ByVal dtNow As Date) As Boolean

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True, _
                      "[StartPlanTime:{0}][DlrCD:{1}][StrCD:{2}][StallId:{3}][finishPlanTime:{4}]", _
                      arg.StartPlanTime, arg.DlrCD, arg.StrCD, arg.StallId, finishPlanTime)

        Dim clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic

        Try
            Dim rtnVal As Boolean = True
            Dim dispStartDateTime As Date
            Dim dispEndDateTime As Date

            '作業開始予定日時～作業終了予定日時の範囲で衝突チェック
            dispStartDateTime = CDate(arg.StartPlanTime)
            dispEndDateTime = finishPlanTime

            '新規予約作成なのでストール利用IDは0とする
            Dim StallUseId As Decimal = 0

            'ストール利用．チップ重複配置チェック
            If clsTabletSMBCommonClass.CheckChipOverlapPosition(arg.DlrCD, _
                                                                arg.StrCD, _
                                                                StallUseId, _
                                                                CType(arg.StallId, Decimal), _
                                                                dispStartDateTime, _
                                                                dispEndDateTime,
                                                                dtNow) Then

                rtnVal = False
            End If

            OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[rowCount:{0}]", rtnVal)

            Return rtnVal

        Finally
            If clsTabletSMBCommonClass IsNot Nothing Then
                clsTabletSMBCommonClass.Dispose()
                clsTabletSMBCommonClass = Nothing
            End If
        End Try

    End Function

    '2019/07/22 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
    ''' <summary>
    ''' ストール使用不可チップ重複配置チェック
    ''' チップがストール使用不可と重複しているか判定します
    ''' </summary>
    ''' <param name="arg">引数クラスオブジェクト</param>
    ''' <param name="finishPlanTime">更新用の予定終了日時</param>
    ''' <returns>チップ重複配置ありの場合<c>true</c>、それ以外の場合<c>false</c></returns>
    ''' <remarks></remarks>
    Public Function CheckStallUnavailableOverlapPosition(ByVal arg As NewChipCallBackArgumentClass, _
                                                         ByVal finishPlanTime As Date) As Boolean

        Using clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic
            Return clsTabletSMBCommonClass.CheckStallUnavailableOverlapPosition( _
                CDate(arg.StartPlanTime), finishPlanTime, CType(arg.StallId, Decimal))
        End Using
    End Function
    '2019/07/22 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END
#End Region


#Region "変換・取得処理"
    ''' <summary>
    ''' DBNullチェックをした値を返却する(String)
    ''' </summary>
    ''' <param name="objColumn"></param>
    ''' <returns>DBNull:""/Not DBNull:StringにCastして返却</returns>
    ''' <remarks></remarks>
    Private Function ConvertDbNullToEmpty(ByVal objColumn As Object) As String

        'OutputInfoLog(MethodBase.GetCurrentMethod.Name, True)

        Dim rtnVal As String = String.Empty

        If Not IsDBNull(objColumn) Then
            rtnVal = CStr(objColumn)
        End If

        'OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[rtnVal:{0}]", rtnVal)

        Return rtnVal

    End Function

    ''' <summary>
    '''   DataTableをJSON文字列に変換する
    ''' </summary>
    ''' <param name="dataTable">変換対象 DataSet</param>
    ''' <returns>JSON文字列</returns>
    Public Function NewChipDataTableToJson(ByVal dataTable As DataTable) As String

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True)

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

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, False)

        Return JSerializer.Serialize(resultMain)

    End Function

    ''' <summary>
    ''' デフォルト値(DB日付型の既定値（1900-1-1 00:00:00）)を取得する
    ''' </summary>
    ''' <returns>デフォルト日付</returns>
    ''' <remarks></remarks>
    Private Function DefaultDateTimeValueGet() As Date
        Return Date.Parse("1900/01/01 00:00:00", CultureInfo.InvariantCulture)
    End Function

    ''' <summary>
    ''' アットマーク("@")以降の文字列のみを切り出す。
    ''' </summary>
    ''' <param name="value">文字列</param>
    ''' <returns>
    ''' アットマーク以降の文字列を返却する。
    ''' アットマークが存在しない場合、引数で受け取った文字列を返却する。
    ''' </returns>
    Private Function TrimBeforeAtmark(ByVal value As String) As String

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_S IN:value={1}", _
                                  MethodBase.GetCurrentMethod.Name, value))

        Dim retValue As String

        If value.Contains("@") Then
            retValue = value.Split("@"c)(1)
        Else
            retValue = value
        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_E OUT:retValue={1}", _
                                  MethodBase.GetCurrentMethod.Name, retValue))

        Return retValue

    End Function
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


#Region "ログ出力メソッド"

    ''' <summary>
    ''' 引数のないInfoレベルのログを出力する
    ''' </summary>
    ''' <param name="method">メソッド名</param>
    ''' <param name="isStart">True:Startログ/False:Endログ</param>
    ''' <remarks></remarks>
    Private Sub OutputInfoLog(ByVal method As String, ByVal isStart As Boolean)

        If isStart Then
            Logger.Info(NEWCHIP_PROGRAMID & ".ascx " & method & "_Start")
        Else
            Logger.Info(NEWCHIP_PROGRAMID & ".ascx " & method & "_End")
        End If

    End Sub

    ''' <summary>
    ''' 引数のあるInfoレベルのログを出力する
    ''' </summary>
    ''' <param name="method">メソッド名</param>
    ''' <param name="isStart">True:Startログ/False:Endログ</param>
    ''' <param name="argString">フォーマット用文字列</param>
    ''' <param name="args">フォーマット用文字列に当てはめる引数値</param>
    ''' <remarks></remarks>
    Private Sub OutputInfoLog(ByVal method As String, ByVal isStart As Boolean, ByVal argString As String, ParamArray args() As Object)

        Dim logString As String = String.Empty

        If isStart Then
            logString = NEWCHIP_PROGRAMID & ".ascx " & method & "_Start" & argString
            Logger.Info(String.Format(CultureInfo.InvariantCulture, logString, args))
        Else
            logString = NEWCHIP_PROGRAMID & ".ascx " & method & "_End" & argString
            Logger.Info(String.Format(CultureInfo.InvariantCulture, logString, args))
        End If

    End Sub

    ' ''' <summary>
    ' ''' エラーログを出力する
    ' ''' </summary>
    ' ''' <param name="method">メソッド名</param>
    ' ''' <param name="ex">例外オブジェクト</param>
    ' ''' <param name="argString">フォーマット用文字列</param>
    ' ''' <param name="args">フォーマット用文字列に当てはめる引数値</param>
    ' ''' <remarks></remarks>
    'Private Sub OutputErrLog(ByVal method As String, ByVal ex As Exception, ByVal argString As String, ParamArray args() As Object)

    '    Dim logString As String = String.Empty

    '    logString = NEWCHIP_PROGRAMID & ".ascx " & method & "_Error" & argString
    '    Logger.Error(String.Format(CultureInfo.InvariantCulture, logString, args), ex)

    'End Sub

#End Region


End Class
