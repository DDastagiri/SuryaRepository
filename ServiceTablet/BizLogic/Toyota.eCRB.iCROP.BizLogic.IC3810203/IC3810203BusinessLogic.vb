'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'IC3810203BusinessLogic.vb
'─────────────────────────────────────
'機能： 来店情報登録
'補足： 
'作成： 2012/09/19 TMEJ 小澤
'更新： 2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発
'更新： 2013/08/08 TMEJ 小澤 次世代e-CRB SAサービス受付機能開発
'更新： 2014/02/24 TMEJ 陳	TMEJ次世代サービス 工程管理機能開発
'更新： 2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発
'更新： 2015/11/10 TM 皆川 (トライ店システム評価)SMBチップ検索の絞り込み方法変更
'更新： 2018/12/13 NSK 坂本 ウェルカムボードが同じお客様を2件表示する 
'更新： 
'─────────────────────────────────────

Imports System.Xml
Imports System.Text
Imports System.Web
Imports System.Reflection
Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SMBLinkage.Customer.DataAccess.IC3810203DataSet
Imports Toyota.eCRB.SMBLinkage.Customer.DataAccess.IC3810203DataSetTableAdapters
Imports Toyota.eCRB.CommonUtility.SMBCommonClass.Api.BizLogic
Imports Toyota.eCRB.CommonUtility.SMBCommonClass.Api.BizLogic.SMBCommonClassBusinessLogic
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.BizLogic

''' <summary>
''' IC3810203
''' </summary>
''' <remarks>来店情報登録</remarks>
Public Class IC3810203BusinessLogic
    Inherits BaseBusinessComponent
    Implements IDisposable

    ''' <summary>
    ''' 成功
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ResultSuccess As Long = 0
    ''' <summary>
    ''' エラー:DBタイムアウト
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ResultDBTimeout As Long = 901
    ''' <summary>
    ''' エラー:該当データなし
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ResultNoMatch As Long = 902
    ''' <summary>
    ''' エラー:引数エラー
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ResultParameterError As Long = 903

    ''' <summary>
    ''' 画面ID:顧客情報編集画面
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SYSTEM_EDIT_CUSTOMER_INFO As String = "SC3080209"

    ''' <summary>
    ''' 画面ID:車両情報編集画面
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SYSTEM_EDIT_VECHICLE_INFO As String = "SC3080211"
    ''' <summary>
    ''' 事前準備フラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PREPARECHIPFLAG As String = "1"

    '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
    ''' <summary>
    ''' キャンセルフラグ（0：有効）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CancelTypeEffective As String = "0"
    ''' <summary>
    ''' 顧客区分（2：未取引客）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CustomerTypeNew As String = "2"
    '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

    ''' <summary>
    ''' 来店情報登録
    ''' </summary>
    ''' <param name="rowIN">顧客登録結果反引数</param>
    ''' <returns>登録結果</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発
    ''' 2013/08/08 TMEJ 小澤 次世代e-CRB SAサービス受付機能開発
    ''' </history>
    Public Function RegisterVisitManagement(ByVal rowIN As IC3810203InCustomerSaveRow) As IC3810203ReservationInfoRow
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '引数チェック
        Dim checkDataCode As Long = Me.CheckParameter(rowIN)
        If checkDataCode <> ResultSuccess Then
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} END OUT:RETURNCODE = {2}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , checkDataCode))
            Return SetReturnDataRow(checkDataCode)
        End If

        '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
        'Dim dtStallReserveInfo As IC3810203StallReserveInfoDataTable = Nothing
        '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

        Using commonClass As New SMBCommonClassBusinessLogic
            Try
                '現在日時を取得
                Dim nowDate As Date = DateTimeFunc.Now(rowIN.DLRCD, rowIN.STRCD)

                '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START

                '2014/02/24 TMEJ 陳	TMEJ次世代サービス 工程管理機能開発 Start
                ''基幹顧客コードを置換
                'If Not (rowIN.IsDMSIDNull) Then
                '    rowIN.DMSID = commonClass.ReplaceBaseCustomerCode(rowIN.DLRCD, rowIN.DMSID)
                'End If

                ''作業内容IDの確認
                'If Not (rowIN.IsREZIDNull) AndAlso rowIN.REZID > 0 Then
                '    '作業内容IDが存在する場合
                '    '作業内容IDからサービス入庫IDを取得する
                '    'rowIN.SVCIN_ID = commonClass.GetJobDetailIdToServiceInId(rowIN.REZID)
                'End If
                '2014/02/24 TMEJ 陳	TMEJ次世代サービス 工程管理機能開発 End

                '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

                '2013/08/08 TMEJ 小澤 次世代e-CRB SAサービス受付機能開発 START
                'Using da As New IC3810203DataTableAdapter
                '    '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
                '    ''予約IDが存在する場合はストール予約テーブルから情報を取得する
                '    'If Not (rowIN.IsREZIDNull) AndAlso rowIN.REZID > 0 Then
                '    '    dtStallReserveInfo = da.GetStallReseveInfo(rowIN, nowDate)
                '    'Else
                '    '    dtStallReserveInfo = da.GetStallReseveInfoNotReserveId(rowIN, nowDate)
                '    'End If
                '    ''取得したデータを顧客登録結果反引数に格納する
                '    'For Each drStallReserveInfo As IC3810203StallReserveInfoRow In dtStallReserveInfo
                '    '    da.UpdateDBStallOrder(rowIN, drStallReserveInfo, nowDate)
                '    '    '履歴の作成
                '    '    Dim commonReturnCodeCustomer As Long = _
                '    '        commonClass.RegisterStallReserveHis(rowIN.DLRCD, _
                '    '                                            rowIN.STRCD, _
                '    '                                            drStallReserveInfo.REZID, _
                '    '                                            nowDate, _
                '    '                                            RegisterType.ReserveHisIndividual)

                '    '    If commonReturnCodeCustomer <> 0 Then
                '    '        Logger.Error(String.Format(CultureInfo.CurrentCulture _
                '    '                    , "{0}.{1} ERROR SMBCommonClassBusinessLogic.RegisterStallReserveHis OUT:RETURNCODE = {2}" _
                '    '                    , Me.GetType.ToString _
                '    '                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                '    '                    , commonReturnCodeCustomer))
                '    '        Return SetReturnDataRow(commonReturnCodeCustomer)
                '    '    End If
                '    'Next
                '    ''2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START

                '    ''予約IDが有る場合は予約情報を取得する
                '    'If Not (rowIN.IsREZIDNull) AndAlso rowIN.REZID > 0 Then
                '    '    Dim dtStallReserveIdInfo As IC3810203StallReserveInfoDataTable = da.GetStallReseveIdInfo(rowIN)
                '    '    If 0 < dtStallReserveIdInfo.Count Then
                '    '        '整備受注NOの取得
                '    '        If Not (dtStallReserveIdInfo.Item(0).IsORDERNONull) Then
                '    '            rowIN.ORDERNO = dtStallReserveIdInfo.Item(0).ORDERNO
                '    '        End If
                '    '    Else
                '    '        Logger.Error(String.Format(CultureInfo.CurrentCulture _
                '    '                    , "{0}.{1} ERROR[IC3810203DataTableAdapter.GetStallReseveInfo] OUT:RETURNCODE = {2}" _
                '    '                    , Me.GetType.ToString _
                '    '                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                '    '                    , ResultNoMatch))
                '    '        Return SetReturnDataRow(ResultNoMatch)
                '    '    End If
                '    'End If
                '    'サービス入庫IDが存在する場合
                '    If Not (rowIN.IsREZIDNull) AndAlso rowIN.REZID > 0 Then
                '        '未取引客の情報を取得する
                '        Dim dtServiceInNewCustomer As IC3810203ServiceInNewCustomerDataTable = _
                '            da.GetServiceNewCustomerData(rowIN)
                '        '未取引客が存在する場合
                '        If 0 < dtServiceInNewCustomer.Count Then
                '            'レコード情報取得
                '            Dim drServiceInNewCust As IC3810203ServiceInNewCustomerRow = _
                '                DirectCast(dtServiceInNewCustomer.Rows(0), IC3810203ServiceInNewCustomerRow)
                '            'サービス入庫追加情報を取得する
                '            Dim dt As IC3810203ServiceInAppendDataTable = _
                '                da.GetServiceInAppendData(drServiceInNewCust)

                '            If 0 < dt.Count Then
                '                'データが存在する場合は更新処理を行う
                '                If da.UpdateServiceInAppend(rowIN, drServiceInNewCust, nowDate) = 0 Then
                '                    Me.Rollback = True
                '                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                '                        , "{0}.{1} OUT:RETURNCODE = {2}" _
                '                        , Me.GetType.ToString _
                '                        , MethodBase.GetCurrentMethod.Name _
                '                        , ResultNoMatch))
                '                    Return SetReturnDataRow(ResultNoMatch)
                '                End If
                '            Else
                '                'データが存在しない場合は新規登録処理を行う
                '                da.InsertServiceInAppend(rowIN, drServiceInNewCust, nowDate)
                '            End If
                '        End If
                '        'サービス入庫情報からRO番号を取得して格納する
                '        Dim dtServiceInInfo As IC3810203ServiceInInfoDataTable = da.GetServiceInData(rowIN)
                '        If 0 < dtServiceInInfo.Count Then
                '            '整備受注NOの取得
                '            If Not (dtServiceInInfo.Item(0).IsRO_NUMNull) Then
                '                rowIN.ORDERNO = dtServiceInInfo.Item(0).RO_NUM
                '            End If
                '            serviceInTableLockCount = dtServiceInInfo.Item(0).ROW_LOCK_VERSION
                '        Else
                '            Logger.Error(String.Format(CultureInfo.CurrentCulture _
                '                        , "{0}.{1} ERROR[IC3810203DataTableAdapter.GetStallReseveInfo] OUT:RETURNCODE = {2}" _
                '                        , Me.GetType.ToString _
                '                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                '                        , ResultNoMatch))
                '            Return SetReturnDataRow(ResultNoMatch)
                '        End If
                '    End If
                '    '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

                '    '来店者情報を追加または更新する
                '    '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
                '    'Dim visitDataResultCode As Long = Me.VisitDataQuery(rowIN, da)
                '    Dim visitDataResultCode As Long = Me.VisitDataQuery(rowIN, da, nowDate)
                '    '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END
                '    If visitDataResultCode <> ResultSuccess Then
                '        Logger.Error(String.Format(CultureInfo.CurrentCulture _
                '                    , "{0}.{1} ERROR OUT:RETURNCODE = {2}" _
                '                    , Me.GetType.ToString _
                '                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                '                    , visitDataResultCode))
                '        Return SetReturnDataRow(visitDataResultCode)
                '    End If
                'End Using
                Dim visitDataResultCode As Long = Me.RegisterCustomerVIsitInfo(rowIN, nowDate)

                If visitDataResultCode <> ResultSuccess Then
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} ERROR OUT:RETURNCODE = {2}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , visitDataResultCode))
                    Return SetReturnDataRow(visitDataResultCode)
                End If
                '2013/08/08 TMEJ 小澤 次世代e-CRB SAサービス受付機能開発 END

                '2013/08/08 TMEJ 小澤 次世代e-CRB SAサービス受付機能開発 START
                If rowIN.IsASSIGNSTATUSNull OrElse _
                   Not (New String() {IC3810203DataTableAdapter.AssignFinished, _
                                      IC3810203DataTableAdapter.AssignOutStore}.Contains(rowIN.ASSIGNSTATUS)) Then
                    'SA未振当ての場合
                    '2013/08/08 TMEJ 小澤 次世代e-CRB SAサービス受付機能開発 END

                    'サービス入庫IDがある場合は本予約にする
                    If Not (rowIN.IsSVCIN_IDNull) AndAlso rowIN.SVCIN_ID > 0 Then
                        '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
                        'If Not (rowIN.IsREZIDNull) Then
                        '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END
                        '戻り値宣言
                        Dim retCode As Long

                        '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
                        'サービス入庫テーブルをロックする
                        retCode = commonClass.LockServiceInTable(rowIN.SVCIN_ID, _
                                                                 rowIN.ROW_LOCK_VERSION, _
                                                                 CancelTypeEffective, _
                                                                 rowIN.ACCOUNT, _
                                                                 nowDate, _
                                                                 rowIN.SYSTEM)
                        If retCode <> SMBCommonClassBusinessLogic.ReturnCode.Success Then
                            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                        , "{0}.{1} SMBCommonClass:LockServiceInTable ERROR = {2}" _
                                        , Me.GetType.ToString _
                                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                        , retCode))
                            Return SetReturnDataRow(retCode)
                        End If
                        '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

                        '入庫日付替え処理
                        retCode = commonClass.ChangeCarInDate(rowIN.DLRCD, _
                                                              rowIN.STRCD, _
                                                              NoReserveId, _
                                                              rowIN.SVCIN_ID, _
                                                              nowDate, _
                                                              rowIN.ACCOUNT, _
                                                              nowDate, _
                                                              rowIN.SYSTEM)
                        If retCode <> 0 Then
                            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                        , "{0}.{1} ERROR SMBCommonClassBusinessLogic.ChangeCarInDate OUT:RETURNCODE = {2}" _
                                        , Me.GetType.ToString _
                                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                        , ResultNoMatch))
                            Return SetReturnDataRow(ResultNoMatch)
                        End If
                        '担当SA変更処理
                        retCode = commonClass.ChangeSACode(rowIN.DLRCD, _
                                                           rowIN.STRCD, _
                                                           rowIN.SVCIN_ID, _
                                                           rowIN.ORDERNO, _
                                                           rowIN.SACODE,
                                                           rowIN.PREPARECHIPFLAG, _
                                                           nowDate, _
                                                           rowIN.ACCOUNT, _
                                                           nowDate, _
                                                           rowIN.SYSTEM)
                        If retCode <> 0 Then
                            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                        , "{0}.{1} ERROR SMBCommonClassBusinessLogic.ChangeSACode OUT:RETURNCODE = {2}" _
                                        , Me.GetType.ToString _
                                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                        , retCode))
                            Return SetReturnDataRow(retCode)
                        End If

                        '履歴の作成
                        '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
                        'Dim commonReturnCodeReserve As Long = _
                        '    commonClass.RegisterStallReserveHis(rowIN.DLRCD, _
                        '                                        rowIN.STRCD, _
                        '                                        rowIN.REZID, _
                        '                                        nowDate, _
                        '                                        RegisterType.ReserveHisIndividual)
                        retCode = commonClass.RegisterStallReserveHis(rowIN.DLRCD, _
                                                                     rowIN.STRCD, _
                                                                     rowIN.SVCIN_ID, _
                                                                     nowDate, _
                                                                     RegisterType.RegisterServiceIn, _
                                                                     rowIN.ACCOUNT, _
                                                                     rowIN.SYSTEM, _
                                                                     SMBCommonClassBusinessLogic.NoActivityId)
                        '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

                        If retCode <> 0 Then
                            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                        , "{0}.{1} ERROR SMBCommonClassBusinessLogic.RegisterStallReserveHis OUT:RETURNCODE = {2}" _
                                        , Me.GetType.ToString _
                                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                        , retCode))
                            Return SetReturnDataRow(retCode)
                        End If
                    End If
                    '2013/08/08 TMEJ 小澤 次世代e-CRB SAサービス受付機能開発 START
                End If
                '2013/08/08 TMEJ 小澤 次世代e-CRB SAサービス受付機能開発 END

            Catch ex As OracleExceptionEx When ex.Number = 1013
                ''ORACLEのタイムアウトのみ処理
                ''終了ログの出力
                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} END OUT:RETURNCODE = {2}" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                            , ResultDBTimeout))
                Return SetReturnDataRow(ResultDBTimeout)
                '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
                'Finally
                '    ''終了処理
                '    If dtStallReserveInfo IsNot Nothing Then dtStallReserveInfo.Dispose()
                '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

            End Try
        End Using
        ''終了ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END OUT:RETURNCODE = {2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , ResultSuccess))
        Return SetReturnDataRow(ResultSuccess, rowIN)
    End Function

    ''' <summary>
    ''' 来店情報登録処理
    ''' </summary>
    ''' <param name="rowIN">顧客登録結果反引数</param>
    ''' <param name="inNowDate">現在日時</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2013/08/08 TMEJ 小澤 次世代e-CRB SAサービス受付機能開発
    ''' </history>
    Private Function RegisterCustomerVIsitInfo(ByVal rowIN As IC3810203InCustomerSaveRow, _
                                               ByVal inNowDate As Date) As Long
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START " _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Using da As New IC3810203DataTableAdapter
            'サービス入庫IDが存在する場合
            If Not (rowIN.IsSVCIN_IDNull) AndAlso rowIN.SVCIN_ID > 0 Then
                'サービス入庫情報を取得
                Dim dtServiceInInfo As IC3810203ServiceInInfoDataTable = da.GetServiceInData(rowIN)
                If 0 < dtServiceInInfo.Count Then

                    '顧客IDを設定
                    rowIN.CUSTOMERCODE = CType(dtServiceInInfo(0).CST_ID, String)

                    '車両IDを設定
                    rowIN.VCL_ID = dtServiceInInfo(0).VCL_ID

                    '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START

                    '顧客氏名を設定
                    rowIN.VISITNAME = dtServiceInInfo(0).CST_NAME

                    '顧客電話番号を設定
                    rowIN.VISITTELNO = dtServiceInInfo(0).CST_TELNO

                    '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END

                    '未取引客の場合
                    If CustomerTypeNew.Equals(dtServiceInInfo.Item(0).CST_TYPE) Then
                        'サービス入庫追加情報テーブルに顧客情報が入っているかを確認する
                        Dim dt As IC3810203ServiceInAppendDataTable = _
                            da.GetServiceInAppendData(dtServiceInInfo(0).CST_ID, _
                                                      dtServiceInInfo(0).VCL_ID)

                        If 0 < dt.Count Then
                            'データが存在する場合は更新処理を行う
                            If da.UpdateServiceInAppend(rowIN, inNowDate) = 0 Then
                                Me.Rollback = True
                                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                    , "{0}.{1} OUT:RETURNCODE = {2}" _
                                    , Me.GetType.ToString _
                                    , MethodBase.GetCurrentMethod.Name _
                                    , ResultNoMatch))
                                Return ResultNoMatch

                            End If
                        Else
                            'データが存在しない場合は新規登録処理を行う
                            da.InsertServiceInAppend(rowIN, inNowDate)

                        End If

                    End If

                    '整備受注NOの取得
                    If Not (dtServiceInInfo.Item(0).IsRO_NUMNull) Then
                        rowIN.ORDERNO = dtServiceInInfo.Item(0).RO_NUM
                    End If

                    '行ロックバージョン取得
                    rowIN.ROW_LOCK_VERSION = dtServiceInInfo.Item(0).ROW_LOCK_VERSION

                Else
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} ERROR[IC3810203DataTableAdapter.GetStallReseveInfo] OUT:RETURNCODE = {2}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , ResultNoMatch))
                    Return ResultNoMatch

                End If

            Else
                '顧客IDと車両IDを設定
                Me.SetCustomerVehicleInfo(rowIN, da)
            End If

            '来店者情報を追加または更新する
            Dim visitDataResultCode As Long = Me.VisitDataQuery(rowIN, da, inNowDate)

            If visitDataResultCode <> ResultSuccess Then
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} ERROR OUT:RETURNCODE = {2}" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                            , visitDataResultCode))
                Return visitDataResultCode
            End If
        End Using

        ''終了ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END OUT:RETURNCODE = {2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , ResultSuccess))
        Return ResultSuccess
    End Function

    ''' <summary>
    ''' 来店者情報の追加または更新処理
    ''' </summary>
    ''' <param name="rowIN">顧客登録結果反引数</param>
    ''' <param name="da">データテーブル</param>
    ''' <param name="inNowDate">現在日時</param>
    ''' <returns>登録、更新結果</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2013/08/08 TMEJ 小澤 次世代e-CRB SAサービス受付機能開発
    ''' </history>
    Private Function VisitDataQuery(ByVal rowIN As IC3810203InCustomerSaveRow, _
                                    ByVal da As IC3810203DataTableAdapter, _
                                    ByVal inNowDate As Date) As Long
        '2013/08/08 TMEJ 小澤 次世代e-CRB SAサービス受付機能開発 START
        'Private Function VisitDataQuery(ByVal rowIN As IC3810203InCustomerSaveRow, _
        '                                ByVal da As IC3810203DataTableAdapter) As Long
        '2013/08/08 TMEJ 小澤 次世代e-CRB SAサービス受付機能開発 END
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START " _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        ''来店実績番号の入力チェック
        If rowIN.IsVISITSEQNull = False _
            AndAlso (rowIN.VISITSEQ > 0) Then
            ''来店実績番号が入力されている場合、修正更新
            ''サービス来店者キー情報の取得
            Using dtVisit As IC3810203VisitKeyDataTable = da.GetVisitKey(rowIN)
                If dtVisit.Rows.Count = 0 Then
                    ''該当データが存在しない場合
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} ERROR IC3810203DataTableAdapter.GetVisitKey OUT:RETURNCODE = {2}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , ResultNoMatch))
                    Return ResultNoMatch
                End If
            End Using
            ''修正更新処理
            da.UpdateVisitCustomer(rowIN)
        Else
            ''顧客情報編集画面および車両情報編集画面から呼ばれた場合、来店者情報を新規登録しない
            If (rowIN.SYSTEM.TrimEnd = SYSTEM_EDIT_CUSTOMER_INFO) _
                OrElse (rowIN.SYSTEM.TrimEnd = SYSTEM_EDIT_VECHICLE_INFO) Then
                ''サービス来店者ユニークキー情報で検索
                Using dtVisitUnique As IC3810203VisitUniqueKeyDataTable = da.GetVisitUniqueKey(rowIN, inNowDate)
                    '2013/08/08 TMEJ 小澤 次世代e-CRB SAサービス受付機能開発 START
                    'Using dtVisitUnique As IC3810203VisitUniqueKeyDataTable = da.GetVisitUniqueKey(rowIN)
                    '2013/08/08 TMEJ 小澤 次世代e-CRB SAサービス受付機能開発 END
                    ''ユニークキー情報に該当するデータが存在する場合
                    If dtVisitUnique.Rows.Count > 0 Then
                        For Each rowVisitSeq As IC3810203VisitUniqueKeyRow In dtVisitUnique.Rows
                            ''修正更新の対象データ取得
                            rowIN.VISITSEQ = rowVisitSeq.VISITSEQ
                            ''修正更新処理
                            da.UpdateVisitCustomer(rowIN)
                        Next
                    End If
                End Using
            Else
                '2013/08/08 TMEJ 小澤 次世代e-CRB SAサービス受付機能開発 START
                ' ''サービス来店者ユニークキー情報で検索
                'Using dtVisitUnique As IC3810203VisitUniqueKeyDataTable = da.GetVisitUniqueKey(rowIN)
                '    ''ユニークキー情報に該当するデータが存在する場合
                '    If dtVisitUnique.Rows.Count > 0 Then
                '        '追加フラグ
                '        Dim insertType As Boolean = True
                '        '戻り値用の来店実績番号
                '        Dim returnVisitSequense As Long

                '        For Each rowVisitSeq As IC3810203VisitUniqueKeyRow In dtVisitUnique.Rows
                '            ''修正更新の対象データ取得
                '            rowIN.VISITSEQ = rowVisitSeq.VISITSEQ
                '            ''修正更新処理
                '            da.UpdateVisitCustomer(rowIN)

                '            If String.Equals(IC3810203DataTableAdapter.AssignFinished, rowVisitSeq.ASSIGNSTATUS) AndAlso _
                '               String.Equals(rowIN.SACODE, rowVisitSeq.SACODE) Then
                '                '担当SAが自分のレコードがある場合は追加フラグを「False」にする
                '                insertType = False
                '                returnVisitSequense = rowVisitSeq.VISITSEQ

                '            End If
                '        Next

                '        If Not (insertType) Then
                '            '担当SAが自分のレコードがある場合は戻り値の来店実績番号を入れる
                '            rowIN.VISITSEQ = returnVisitSequense

                '        ElseIf insertType AndAlso Not (PREPARECHIPFLAG.Equals(rowIN.PREPARECHIPFLAG)) Then
                '            '「追加フラグ=True Andalso 事前準備チップフラグ<>1」の場合は追加処理を行う
                '            rowIN.VISITSEQ = da.InsertVisitCustomer(rowIN)
                '        End If
                '    Else
                '        '「事前準備チップフラグ<>1」の場合は新規登録処理を行う
                '        If Not (PREPARECHIPFLAG.Equals(rowIN.PREPARECHIPFLAG)) Then
                '            rowIN.VISITSEQ = da.InsertVisitCustomer(rowIN)
                '        End If
                '    End If
                'End Using
                Dim returnVisitInfoCode As Long = Me.RegisterVisitInfo(rowIN, _
                                                                       da, _
                                                                       inNowDate)

                If returnVisitInfoCode <> 0 Then
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} ERROR SMBCommonClassBusinessLogic.ChangeCarInDate OUT:RETURNCODE = {2}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , returnVisitInfoCode))
                    Return returnVisitInfoCode
                End If
                '2013/08/08 TMEJ 小澤 次世代e-CRB SAサービス受付機能開発 END
            End If
        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
        Return ResultSuccess
    End Function

    ''' <summary>
    ''' 来店情報登録処理
    ''' </summary>
    ''' <param name="rowIN">顧客登録結果反引数</param>
    ''' <param name="da">データテーブル</param>
    ''' <param name="inNowDate">現在日時</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2013/08/08 TMEJ 小澤 次世代e-CRB SAサービス受付機能開発
    ''' </history>
    Private Function RegisterVisitInfo(ByVal rowIN As IC3810203InCustomerSaveRow, _
                                       ByVal da As IC3810203DataTableAdapter, _
                                       ByVal inNowDate As Date) As Long
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        ''来店実績番号の入力チェック2018/12/13 NSK  坂本 TR-SVT-TMT-20180427-001 ウェルカムボードが同じお客様を2件表示する STRAT
        ''サービス来店者ユニークキー情報で検索
        Dim dtVisitUnique As IC3810203VisitUniqueKeyDataTable = da.GetVisitUniqueKey(rowIN, inNowDate)
        'サービス来店者情報が取得できず、且つサービス入庫IDがある場合
        If dtVisitUnique.Rows.Count = 0 AndAlso Not (rowIN.IsSVCIN_IDNull) AndAlso 0 < rowIN.SVCIN_ID Then
            dtVisitUnique = da.GetVisitUniqueKeyByServiceId(rowIN, inNowDate)
        End If
        '取得できない
        If dtVisitUnique.Rows.Count = 0 Then
            dtVisitUnique = da.GetVisitUniqueKeyByVehcle(rowIN, inNowDate)
        End If
        ''来店実績番号の入力チェック2018/12/13 NSK  坂本 TR-SVT-TMT-20180427-001 ウェルカムボードが同じお客様を2件表示する END
        ''ユニークキー情報に該当するデータが存在する場合
        If dtVisitUnique.Rows.Count > 0 Then
            '追加フラグ
            Dim insertType As Boolean = True
            '戻り値用の格納変数
            Dim returnVisitSequense As Long
            Dim returnAssignStatus As String = IC3810203DataTableAdapter.AssignFinished

            For Each rowVisitSeq As IC3810203VisitUniqueKeyRow In dtVisitUnique.Rows

                '2013/08/08 TMEJ 小澤 次世代e-CRB SAサービス受付機能開発 START
                ' ''修正更新の対象データ取得
                'rowIN.VISITSEQ = rowVisitSeq.VISITSEQ
                ' ''修正更新処理
                'da.UpdateVisitCustomer(rowIN)
                '2013/08/08 TMEJ 小澤 次世代e-CRB SAサービス受付機能開発 END

                If String.Equals(IC3810203DataTableAdapter.AssignFinished, rowVisitSeq.ASSIGNSTATUS) AndAlso _
                   String.Equals(rowIN.SACODE, rowVisitSeq.SACODE) Then
                    '本日来店で担当SAが自分のレコードがある場合は追加フラグを「False」にする
                    insertType = False
                    returnVisitSequense = rowVisitSeq.VISITSEQ

                    '2014/02/24 TMEJ 陳	TMEJ次世代サービス 工程管理機能開発 START
                    rowIN.VISITSEQ = returnVisitSequense
                    da.UpdateAssginInfo(rowIN, inNowDate)
                    '2014/02/24 TMEJ 陳	TMEJ次世代サービス 工程管理機能開発 END

                ElseIf Not (New String() {IC3810203DataTableAdapter.AssignFinished, _
                                          IC3810203DataTableAdapter.AssignOutStore}.Contains(rowVisitSeq.ASSIGNSTATUS)) Then
                    '本日来店でSA未振当て前の場合は振当て処理を行う

                    ''更新の対象データ取得
                    rowIN.VISITSEQ = rowVisitSeq.VISITSEQ

                    '振当て処理
                    da.UpdateAssginInfo(rowIN, inNowDate)

                    '担当SAが自分のレコードがある場合は追加フラグを「False」にする
                    insertType = False

                    '対象の来店管理番号と振当てステータスを保持
                    returnVisitSequense = rowVisitSeq.VISITSEQ
                    returnAssignStatus = rowVisitSeq.ASSIGNSTATUS

                End If
            Next

            If Not (insertType) Then
                '担当SAが自分のレコードがある場合は戻り値の来店実績番号を入れる
                rowIN.VISITSEQ = returnVisitSequense
                rowIN.ASSIGNSTATUS = returnAssignStatus

                '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
                ''修正更新処理
                da.UpdateVisitCustomer(rowIN)
                '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

            ElseIf insertType AndAlso Not (PREPARECHIPFLAG.Equals(rowIN.PREPARECHIPFLAG)) Then
                '「追加フラグ=True Andalso 事前準備チップフラグ<>1」の場合は追加処理を行う
                rowIN.VISITSEQ = da.InsertVisitCustomer(rowIN)
            End If
        Else
            '「事前準備チップフラグ<>1」の場合は新規登録処理を行う
            If Not (PREPARECHIPFLAG.Equals(rowIN.PREPARECHIPFLAG)) Then
                rowIN.VISITSEQ = da.InsertVisitCustomer(rowIN)
            End If
        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
        Return ResultSuccess
    End Function

    ''' <summary>
    ''' 顧客ID・車両ID設定処理
    ''' </summary>
    ''' <param name="rowIN">顧客登録結果反引数</param>
    ''' <param name="da">データテーブル</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2013/08/08 TMEJ 小澤 次世代e-CRB SAサービス受付機能開発
    ''' </history>
    Private Sub SetCustomerVehicleInfo(ByVal rowIN As IC3810203InCustomerSaveRow, _
                                       ByVal da As IC3810203DataTableAdapter)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '2015/11/10 TM 皆川 (トライ店システム評価)SMBチップ検索の絞り込み方法変更 START
        '車両登録番号を退避
        Dim vclRegNum = rowIN.VCLREGNO

        Using svcCommonClassBiz As New ServiceCommonClassBusinessLogic
            '顧客情報取得の検索条件（車両登録番号）から区切り文字を除去
            rowIN.VCLREGNO = svcCommonClassBiz.ConvertVclRegNumWord(rowIN.VCLREGNO)
        End Using
        '2015/11/10 TM 皆川 (トライ店システム評価)SMBチップ検索の絞り込み方法変更 END

        '顧客情報取得
        Dim dtCustomerInfo As IC3810203CustomerInfoDataTable = da.GetCustomerInfo(rowIN)

        '2015/11/10 TM 皆川 (トライ店システム評価)SMBチップ検索の絞り込み方法変更 START
        '車両登録番号を元に戻す
        rowIN.VCLREGNO = vclRegNum
        '2015/11/10 TM 皆川 (トライ店システム評価)SMBチップ検索の絞り込み方法変更 END

        If 0 < dtCustomerInfo.Count Then
            '顧客情報が取得できた場合は顧客IDと車両IDを設定する

            Dim drCustomerInfo As IC3810203CustomerInfoRow = dtCustomerInfo(0)
            '顧客IDを設定
            If Not (drCustomerInfo.IsCST_IDNull) Then
                rowIN.CUSTOMERCODE = CType(drCustomerInfo.CST_ID, String)

            End If

            '車両IDの設定
            If Not (drCustomerInfo.IsVCL_IDNull) Then
                rowIN.VCL_ID = drCustomerInfo.VCL_ID

            End If

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END " _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' 戻り値用データ格納処理
    ''' </summary>
    ''' <param name="inReturn">エラーコード</param>
    ''' <param name="rowIN">顧客登録結果反引数</param>
    ''' <returns>戻り値用DataRow</returns>
    ''' <remarks></remarks>
    Private Function SetReturnDataRow(ByVal inReturn As Long, _
                                      Optional ByVal rowIN As IC3810203InCustomerSaveRow = Nothing) As IC3810203ReservationInfoRow
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START P1:{2} P2:IC3810203InCustomerSaveRow " _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , inReturn))

        Using dtReservationInfo As New IC3810203ReservationInfoDataTable
            Dim drReservationInfo As IC3810203ReservationInfoRow = dtReservationInfo.NewIC3810203ReservationInfoRow
            drReservationInfo._RETURN = inReturn
            If Not (IsNothing(rowIN)) Then
                If Not (rowIN.IsVISITSEQNull) Then
                    drReservationInfo.VISITSEQ = rowIN.VISITSEQ
                End If
                If Not (rowIN.IsREZIDNull) Then
                    drReservationInfo.REZID = rowIN.REZID
                End If
                If Not (rowIN.IsORDERNONull) Then
                    drReservationInfo.ORDERNO = rowIN.ORDERNO
                End If
            End If

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END " _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return drReservationInfo
        End Using
    End Function

    ''' <summary>
    ''' 引数チェック
    ''' </summary>
    ''' <param name="rowIN">顧客登録結果反引数</param>
    ''' <returns>結果</returns>
    ''' <remarks></remarks>
    ''' 
    ''' <history>
    ''' 2013/08/08 TMEJ 小澤 次世代e-CRB SAサービス受付機能開発
    ''' </history>
    Private Function CheckParameter(ByVal rowIN As IC3810203InCustomerSaveRow) As Long
        '販売店コード
        If rowIN.IsDLRCDNull OrElse String.IsNullOrEmpty(rowIN.DLRCD) Then
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} ERROR[DLRCD] OUT:RETURNCODE = {2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , ResultParameterError))
            Return ResultParameterError
        End If
        '店舗コード
        If rowIN.IsSTRCDNull OrElse String.IsNullOrEmpty(rowIN.STRCD) Then
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} ERROR[STRCD] OUT:RETURNCODE = {2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , ResultParameterError))
            Return ResultParameterError
        End If
        '基幹顧客ID
        If rowIN.IsCUSTOMERCODENull OrElse String.IsNullOrEmpty(rowIN.CUSTOMERCODE) Then
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} ERROR[CUSTOMERCODE] OUT:RETURNCODE = {2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , ResultParameterError))
            Return ResultParameterError
            '2013/08/08 TMEJ 小澤 次世代e-CRB SAサービス受付機能開発 START
        Else
            rowIN.CUSTOMERCODE = Nothing
            '2013/08/08 TMEJ 小澤 次世代e-CRB SAサービス受付機能開発 END
        End If
        '基幹顧客ID
        If rowIN.IsDMSIDNull OrElse String.IsNullOrEmpty(rowIN.DMSID) Then
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} ERROR[DMSID] OUT:RETURNCODE = {2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , ResultParameterError))
            Return ResultParameterError
        End If
        'アカウント
        If rowIN.IsACCOUNTNull OrElse String.IsNullOrEmpty(rowIN.ACCOUNT) Then
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} ERROR[ACCOUNT] OUT:RETURNCODE = {2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , ResultParameterError))
            Return ResultParameterError
        End If
        '画面ID
        If rowIN.IsSYSTEMNull OrElse String.IsNullOrEmpty(rowIN.SYSTEM) Then
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} ERROR[SYSTEM] OUT:RETURNCODE = {2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , ResultParameterError))
            Return ResultParameterError
        End If
        '事前準備フラグ
        If rowIN.IsPREPARECHIPFLAGNull OrElse String.IsNullOrEmpty(rowIN.PREPARECHIPFLAG) Then
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} ERROR[PREPARECHIPFLAG] OUT:RETURNCODE = {2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , ResultParameterError))
            Return ResultParameterError
        End If
        Return ResultSuccess
    End Function


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
