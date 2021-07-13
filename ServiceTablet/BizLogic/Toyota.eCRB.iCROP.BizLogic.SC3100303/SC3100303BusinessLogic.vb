'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3100303BusinessLogic.vb
'─────────────────────────────────────
'機能： 来店管理メインのビジネスロジック
'補足： 
'作成： 2013/03/06 TMEJ 張	初版作成
'更新： 2012/04/25 TMEJ 張  ITxxxx_TSL自主研緊急対応（サービス）
'更新： 2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
'更新： 2014/02/20 TMEJ陳	TMEJ次世代サービス 工程管理機能開発
'更新： 2018/02/20 NSK  山田 REQ-SVT-TMT-20170615-001 来店管理に入庫予約を指定してROを発行する機能を追加
'─────────────────────────────────────

Imports System.Text
Imports System.Net
Imports System.IO
Imports System.Globalization
Imports System.Web.Script.Serialization
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.iCROP.DataAccess.SC3100303


'2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

Imports Toyota.eCRB.CommonUtility.SMBCommonClass.Api.BizLogic

'2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

'2014/02/20 TMEJ陳	TMEJ次世代サービス 工程管理機能開発 START

Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.DataAccess
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.BizLogic

'2014/02/20 TMEJ陳	TMEJ次世代サービス 工程管理機能開発 END

'2018/02/20 NSK 山田 REQ-SVT-TMT-20170615-001 来店管理に入庫予約を指定してROを発行する機能を追加 START
Imports Toyota.eCRB.SMBLinkage.Customer.DataAccess.IC3810203DataSet
Imports Toyota.eCRB.SMBLinkage.Customer.BizLogic
Imports Toyota.eCRB.iCROP.BizLogic.IC3810301
Imports Toyota.eCRB.Visit.Api.DataAccess.VisitUtilityDataSet
Imports Toyota.eCRB.Visit.Api.BizLogic
'2018/02/20 NSK 山田 REQ-SVT-TMT-20170615-001 来店管理に入庫予約を指定してROを発行する機能を追加 END

Public Class SC3100303BusinessLogic
    Inherits BaseBusinessComponent
    '2014/02/20 TMEJ陳	TMEJ次世代サービス 工程管理機能開発 START
    Implements IDisposable
    '2014/02/20 TMEJ陳	TMEJ次世代サービス 工程管理機能開発 END

#Region "定数"

    '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

    ''' <summary>
    ''' アプリケーションID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ApplicationID As String = "SC3100303"

    '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

    '2018/02/20 NSK 山田 REQ-SVT-TMT-20170615-001 来店管理に入庫予約を指定してROを発行する機能を追加 START
    ''' <summary>
    ''' VISITSEQ既定値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VISITSEQ_DEFAULT_VALUE As Long = 0
    ''' <summary>
    ''' 事前準備チップフラグ（0：事前準備チップ以外に対する顧客登録）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PREPEARENCE_CHIP_FLG As String = "0"
    ''' <summary>
    ''' 削除フラグ（未削除）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DeleteFlagNone As String = "0"

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
        DBTimeOut = 901

        ''' <summary>
        ''' 排他エラー
        ''' </summary>
        OtherChanged = 902

    End Enum
    '2018/02/20 NSK 山田 REQ-SVT-TMT-20170615-001 来店管理に入庫予約を指定してROを発行する機能を追加 END

#End Region

#Region "JSON変換"

    ''' <summary>
    '''   DataTableをJSON文字列に変換する
    ''' </summary>
    ''' <param name="dataTable">変換対象 DataSet</param>
    ''' <returns>JSON文字列</returns>
    Public Function DataTableToJson(ByVal dataTable As DataTable) As String
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

        Return JSerializer.Serialize(resultMain)
    End Function

#End Region

#Region "店舗設定の取得"

    ''' <summary>
    ''' 店舗稼動時間情報の取得
    ''' </summary>
    ''' <returns></returns>
    Public Function GetBranchOperatingHours() As SC3100303DataSet.SC3100303BranchOperatingHoursDataTable
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S.", System.Reflection.MethodBase.GetCurrentMethod.Name))

        Using ta As New SC3100303DataSetTableAdapters.SC3100303DataAdapter
            Dim dt As SC3100303DataSet.SC3100303BranchOperatingHoursDataTable
            Dim userContext As StaffContext = StaffContext.Current
            dt = ta.GetBranchOperatingHours(userContext.DlrCD, userContext.BrnCD)

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return dt
        End Using
    End Function
#End Region

#Region "来店チップ情報の取得"
    ''' <summary>
    ''' 来店チップ情報の取得
    ''' </summary>
    ''' <param name="fromDate">ストール稼動開始日時</param>
    ''' <param name="toDate">ストール稼動終了日時</param>
    ''' <returns></returns>
    Public Function GetVisitChips(ByVal fromDate As Date, _
                                    ByVal toDate As Date) As SC3100303DataSet.SC3100303VisitChipDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. fromDate={1}, toDate={2}" _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name, fromDate, toDate))

        Dim userContext As StaffContext = StaffContext.Current
        Dim chipList As SC3100303DataSet.SC3100303VisitChipDataTable

        Using ta As New SC3100303DataSetTableAdapters.SC3100303DataAdapter
            chipList = ta.GetVisitChips(userContext.DlrCD, userContext.BrnCD, fromDate, toDate)
        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return chipList

    End Function
#End Region

#Region "点滅チップIDの取得"
    ''' <summary>
    ''' 来店チップ情報の取得
    ''' </summary>
    ''' <param name="fromDate">ストール稼動開始日時</param>
    ''' <param name="toDate">ストール稼動終了日時</param>
    ''' <returns></returns>
    Public Function GetSwitchChipId(ByVal fromDate As Date, _
                                    ByVal toDate As Date) As SC3100303DataSet.SC3100303SwitchChipIdDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. fromDate={1}, toDate={2}" _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name, fromDate, toDate))

        Dim userContext As StaffContext = StaffContext.Current
        Dim chipList As SC3100303DataSet.SC3100303SwitchChipIdDataTable

        Using ta As New SC3100303DataSetTableAdapters.SC3100303DataAdapter
            chipList = ta.GetSwitchChipId(userContext.DlrCD, userContext.BrnCD, fromDate, toDate)
        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return chipList

    End Function
#End Region

    '2013/04/25 TMEJ 張 ITxxxx_TSL自主研緊急対応（サービス） START
#Region "来店チップ情報の取得"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="fromDate"></param>
    ''' <param name="toDate"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetVstCarCnt(ByVal fromDate As Date, _
                                    ByVal toDate As Date) As Integer

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. fromDate={1}, toDate={2}" _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name, fromDate, toDate))

        Dim userContext As StaffContext = StaffContext.Current
        Dim chipList As SC3100303DataSet.SC3100303VstCarCntDataTable

        Using ta As New SC3100303DataSetTableAdapters.SC3100303DataAdapter
            chipList = ta.GetVstCarCnt(userContext.DlrCD, userContext.BrnCD, fromDate, toDate)
        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return chipList.Rows(0)(0)

    End Function
#End Region
    '2013/04/25 TMEJ 張 ITxxxx_TSL自主研緊急対応（サービス） END

#Region "フォロー、フォロー解除処理"

    ''' <summary>
    ''' フォロー、フォロー解除処理
    ''' </summary>
    ''' <param name="rezid">予約id</param>
    ''' <param name="noShowFollowFlg">NoShowフォローフラグ</param>
    ''' <param name="dlrCD">販売点コード</param>
    ''' <param name="strCD">店コード</param>
    ''' <param name="updateAccount">更新アカウント</param>
    ''' <returns>正常終了：0、異常終了：エラーコード</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
    ''' </history>
    <EnableCommit()>
    Public Function UpdateFollowFlg(ByVal rezid As Long _
                                  , ByVal noShowFollowFlg As String _
                                  , ByVal dlrCD As String _
                                  , ByVal strCD As String _
                                  , ByVal updateAccount As String _
                                  , ByVal updateCnt As Integer) As Integer

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. rezid={1}, noShowFollowFlg={2}, dlrCD={3}, strCD={4}, updateAccount={5}" _
                          , System.Reflection.MethodBase.GetCurrentMethod.Name, rezid, noShowFollowFlg, dlrCD, strCD, updateAccount))

        Dim tblUpdateCnt As SC3100303DataSet.SC3100303UpdateCntDataTable

        'リターンコード
        Dim returnCode As Long = 0

        'フォロー更新、解除処理
        Using ta As New SC3100303DataSetTableAdapters.SC3100303DataAdapter
            '最新の記録かどうかをチェックする


            '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

            'tblUpdateCnt = ta.GetUpdateCount(rezid, dlrCD, strCD)

            '行ロックバージョンの取得
            tblUpdateCnt = ta.GetUpdateCount(rezid)

            '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END


            'データ取得確認
            If tblUpdateCnt.Rows.Count <> 1 Then
                '取得できず

                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E. Data not found. rezid={1}, dlrCD={2}, strCD={3}" _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name, rezid, dlrCD, strCD))

                '予期せぬエラー
                Return 903
            End If

            '最新のデータをチェック
            Dim dbUpdateCnt As Integer = CType(tblUpdateCnt.Rows(0)(0), Integer)

            '排他チェック
            If dbUpdateCnt <> updateCnt Then
                '排他エラー

                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E. Data changed by other(s). rezid={1}, dlrCD={2}, strCD={3}" _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name, rezid, dlrCD, strCD))

                'そのチップは、既に他のユーザーによって変更が加えられています。
                Return 902
            End If


            '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

            '現在日時の取得
            Dim presentTime As Date = DateTimeFunc.Now(dlrCD)

            Dim result As Long = 0

            'SMBCommonClass
            Using smbCommon As New SMBCommonClassBusinessLogic

                'テーブルロック処理
                result = smbCommon.LockServiceInTable(rezid, _
                                                      updateCnt, _
                                                      "0", _
                                                      updateAccount, _
                                                      presentTime, _
                                                      ApplicationID)

            End Using


            'ロック確認
            If result = 0 Then
                'ロック成功

                'returnCode = ta.UpdateFollowFlg(rezid, noShowFollowFlg, dlrCD, strCD, updateAccount)

                'フォロー登録・フォロー解除、登録処理
                returnCode = ta.UpdateFollowFlg(rezid, noShowFollowFlg, updateAccount, presentTime)

                '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END


                '更新処理確認
                If returnCode = 1 Then
                    '成功

                    returnCode = 0

                End If
            End If
        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E", System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return returnCode
    End Function
#End Region

    '2014/02/20 TMEJ陳	TMEJ次世代サービス 工程管理機能開発 START
#Region "基幹コードへ変換処理"

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
                       , "{0}.{1} End dtDmsCodeMap:COUNT = {2}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , dtDmsCodeMap.Count))

            '結果返却
            Return rowDmsCodeMap

        End Using

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

    ' このコードは、破棄可能なパターンを正しく実装できるように Visual Basic によって追加されました。
    Public Sub Dispose() Implements IDisposable.Dispose
        ' このコードを変更しないでください。クリーンアップ コードを上の Dispose(ByVal disposing As Boolean) に記述します。
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

#End Region

    '2014/02/20 TMEJ陳	TMEJ次世代サービス 工程管理機能開発 END

    '2018/02/20 NSK 山田 REQ-SVT-TMT-20170615-001 来店管理に入庫予約を指定してROを発行する機能を追加 START
#Region "来店情報登録及びRO情報登録・RO連携処理"
    ''' <summary>
    ''' 来店情報登録及びRO情報登録・RO連携処理
    ''' </summary>
    ''' <param name="svcInId">サービス入庫ID</param>
    ''' <param name="nowDate">現在日時</param>
    ''' <returns>来店情報登録API処理結果</returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Function VisitRegistProccess(ByVal svcInId As Decimal, _
                                        ByVal nowDate As Date) As IC3810203ReservationInfoRow

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
        '戻り値
        Dim rowReservationInfo As IC3810203ReservationInfoRow
        Using da As New SC3100303DataSetTableAdapters.SC3100303DataAdapter
            '未入庫予約存在チェック
            Dim notCarInCount As Integer = da.IsNotCarInStatus(svcInId)

            If notCarInCount = 0 Then
                '未入庫予約が存在しない場合
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} ERROR OUT:CarInStatus not exist." _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name))
                Me.Rollback = True
                Using dtReservationInfo As New IC3810203ReservationInfoDataTable
                    rowReservationInfo = dtReservationInfo.NewIC3810203ReservationInfoRow
                End Using
                rowReservationInfo._RETURN = ReturnCode.OtherChanged
                Return rowReservationInfo
            End If

            '顧客車両情報取得
            Dim cusVehicleInfo As SC3100303DataSet.SC3100303CstVehicleDataTable
            cusVehicleInfo = da.GetCstVehicle(svcInId)

            '来店登録API
            Using dtCondition As New IC3810203InCustomerSaveDataTable
                Dim rowCondition As IC3810203InCustomerSaveRow = dtCondition.NewIC3810203InCustomerSaveRow
                Dim staffInfo As StaffContext = StaffContext.Current
                'サービス入庫ID
                rowCondition.SVCIN_ID = svcInId
                '予約ID
                rowCondition.REZID = svcInId
                '来店実績番号（固定「0」）
                rowCondition.VISITSEQ = VISITSEQ_DEFAULT_VALUE
                '機能ID（画面ID）
                rowCondition.SYSTEM = ApplicationID
                'アカウント（ログインユーザアカウント）
                rowCondition.ACCOUNT = staffInfo.Account
                '事前準備チップフラグ（固定「0」）
                rowCondition.PREPARECHIPFLAG = PREPEARENCE_CHIP_FLG

                '販売店コード
                rowCondition.DLRCD = staffInfo.DlrCD
                '店舗コード
                rowCondition.STRCD = staffInfo.BrnCD

                '顧客コード
                If String.IsNullOrWhiteSpace(cusVehicleInfo(0).DMS_CST_CD_DISP) Then
                    rowCondition.CUSTOMERCODE = String.Empty
                Else
                    rowCondition.CUSTOMERCODE = cusVehicleInfo(0).DMS_CST_CD_DISP
                End If

                '基幹顧客ID
                If String.IsNullOrWhiteSpace(cusVehicleInfo(0).DMS_CST_CD) Then
                    rowCondition.DMSID = String.Empty
                Else
                    rowCondition.DMSID = cusVehicleInfo(0).DMS_CST_CD
                End If

                '車両登録No
                If String.IsNullOrWhiteSpace(cusVehicleInfo(0).REG_NUM) Then
                    rowCondition.VCLREGNO = String.Empty
                Else
                    rowCondition.VCLREGNO = cusVehicleInfo(0).REG_NUM
                End If

                'VIN
                If String.IsNullOrWhiteSpace(cusVehicleInfo(0).VCL_VIN) Then
                    rowCondition.VIN = String.Empty
                Else
                    rowCondition.VIN = cusVehicleInfo(0).VCL_VIN
                End If

                'モデルコード(車両型式をセットする)
                If String.IsNullOrWhiteSpace(cusVehicleInfo(0).VCL_KATASHIKI) Then
                    rowCondition.MODELCODE = String.Empty
                Else
                    rowCondition.MODELCODE = cusVehicleInfo(0).VCL_KATASHIKI
                End If

                '顧客名
                If String.IsNullOrWhiteSpace(cusVehicleInfo(0).CST_NAME) Then
                    rowCondition.CUSTOMERNAME = String.Empty
                Else
                    rowCondition.CUSTOMERNAME = cusVehicleInfo(0).CST_NAME
                End If

                '電話番号
                If String.IsNullOrWhiteSpace(cusVehicleInfo(0).CST_PHONE) Then
                    rowCondition.TELNO = String.Empty
                Else
                    rowCondition.TELNO = cusVehicleInfo(0).CST_PHONE
                End If

                '携帯番号
                If String.IsNullOrWhiteSpace(cusVehicleInfo(0).CST_MOBILE) Then
                    rowCondition.MOBILE = String.Empty
                Else
                    rowCondition.MOBILE = cusVehicleInfo(0).CST_MOBILE
                End If

                '振当SA
                rowCondition.SACODE = staffInfo.Account

                '車名
                If String.IsNullOrWhiteSpace(cusVehicleInfo(0).MODEL_NAME) Then
                    rowCondition.VEHICLENAME = String.Empty
                Else
                    rowCondition.VEHICLENAME = cusVehicleInfo(0).MODEL_NAME
                End If

                'E-MAILアドレス1
                If String.IsNullOrWhiteSpace(cusVehicleInfo(0).CST_EMAIL_1) Then
                    rowCondition.EMAIL1 = String.Empty
                Else
                    rowCondition.EMAIL1 = cusVehicleInfo(0).CST_EMAIL_1
                End If

                '来店情報登録
                Using bizIC3810203 As New IC3810203BusinessLogic
                    '戻り値を保持
                    rowReservationInfo = bizIC3810203.RegisterVisitManagement(rowCondition)
                End Using

                If rowReservationInfo._RETURN = ReturnCode.DBTimeOut Then
                    'DBタイムアウト
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} ERROR OUT:RETURNCODE = {2}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , rowReservationInfo._RETURN.ToString(CultureInfo.CurrentCulture)))

                    Me.Rollback = True

                    Return rowReservationInfo
                ElseIf rowReservationInfo._RETURN <> ReturnCode.Success Then
                    'その他エラー
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} ERROR OUT:RETURNCODE = {2}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , rowReservationInfo._RETURN.ToString(CultureInfo.CurrentCulture)))

                    Me.Rollback = True

                    Return Nothing
                End If

                'RO存在チェック
                Dim roCount As Integer = da.CheckRoExists(rowReservationInfo.VISITSEQ)

                If roCount = 0 Then
                    'ROが存在しない場合
                    'RO連携APIを呼び出す
                    Using IC3810301Biz As New IC3810301BusinessLogic

                        Dim resultCode As Long = IC3810301Biz.InsertRepairOrderInfo(rowCondition.SVCIN_ID, _
                                                                                    rowCondition.DLRCD, _
                                                                                    rowCondition.STRCD, _
                                                                                    rowCondition.VISITSEQ, _
                                                                                    rowCondition.ACCOUNT, _
                                                                                    nowDate, _
                                                                                    ApplicationID)
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
            End Using 'dtCondition
        End Using 'da

        'リフレッシュPush送信
        SendPushForRefreshWelcomeBoard(StaffContext.Current)

        Return rowReservationInfo
    End Function

#End Region

#Region "来店情報取得"
    ''' <summary>
    ''' 来店情報取得
    ''' </summary>
    ''' <param name="vstSeq">来店実績連番</param>
    ''' <returns>来店情報</returns>
    ''' <remarks></remarks>
    Public Function GetVisitInfo(ByVal vstSeq As Long) As SC3100303DataSet.SC3100303ContactInfoDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. vstSeq={1}" _
            , System.Reflection.MethodBase.GetCurrentMethod.Name, vstSeq))

        Dim contactInfo As SC3100303DataSet.SC3100303ContactInfoDataTable
        Using da As New SC3100303DataSetTableAdapters.SC3100303DataAdapter
            contactInfo = da.GetContactInfo(vstSeq)
        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return contactInfo
    End Function
#End Region

#Region "セッション格納用情報取得"
    ''' <summary>
    ''' セッション格納用情報取得
    ''' </summary>
    ''' <param name="svcInId">サービス入庫ID</param>
    ''' <returns>セッション格納用情報</returns>
    ''' <remarks></remarks>
    Public Function GetSessionInfo(ByVal svcInId As Decimal) _
                                   As SC3100303DataSet.SC3100303SessionInfoDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. svcInId={1}" _
                 , System.Reflection.MethodBase.GetCurrentMethod.Name, svcInId))

        Dim sessionInfo As SC3100303DataSet.SC3100303SessionInfoDataTable
        Using da As New SC3100303DataSetTableAdapters.SC3100303DataAdapter
            sessionInfo = da.GetSessionInfo(svcInId)
        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return sessionInfo
    End Function
#End Region

#Region "基幹作業内容ID取得"
    ''' <summary>
    ''' 基幹作業内容ID取得
    ''' </summary>
    ''' <param name="svcInId">サービス入庫ID</param>
    ''' <param name="dlrCd">販売店コード</param>
    ''' <param name="brnCd">店舗コード</param>
    ''' <returns>基幹作業内容ID</returns>
    ''' <remarks></remarks>
    Public Function GetDmsJobDtlId(ByVal svcInId As Decimal, _
                                   ByVal dlrCd As String, _
                                   ByVal brnCd As String) _
                                   As SC3100303DataSet.SC3100303DmsJobDtlIdDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. svcInId={1},dealerCode={2}, branchCode={3}" _
                 , System.Reflection.MethodBase.GetCurrentMethod.Name, svcInId, dlrCd, brnCd))

        Dim dmsJobDtlId As SC3100303DataSet.SC3100303DmsJobDtlIdDataTable
        Using da As New SC3100303DataSetTableAdapters.SC3100303DataAdapter
            dmsJobDtlId = da.GetDmsJobDtlId(svcInId, _
                                            dlrCd, _
                                            brnCd)
        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return dmsJobDtlId
    End Function
#End Region

#Region "Push送信呼出"

    ''' <summary>
    ''' WelcomeBoardリフレッシュPush送信
    ''' </summary>
    ''' <param name="inStaffInfo">ログインスタッフ情報</param>
    ''' <remarks></remarks>
    Public Sub SendPushForRefreshWelcomeBoard(ByVal inStaffInfo As StaffContext)
        Logger.Debug("SendPushForRefreshWelcomeBoard_Start Pram[" & inStaffInfo.DlrCD & "," & inStaffInfo.BrnCD & inStaffInfo.Account & "]")

        'スタッフ情報の取得(WB)
        Dim stuffCodeList As New List(Of Decimal)
        stuffCodeList.Add(SystemFrameworks.Core.iCROP.BizLogic.Operation.WBS)

        '全ユーザー情報の取得
        Dim utility As New VisitUtilityBusinessLogic
        Dim sendPushUsers As VisitUtilityUsersDataTable = _
            utility.GetUsers(inStaffInfo.DlrCD, inStaffInfo.BrnCD, stuffCodeList, Nothing, DeleteFlagNone)
        utility = Nothing

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
    '2018/02/20 NSK 山田 REQ-SVT-TMT-20170615-001 来店管理に入庫予約を指定してROを発行する機能を追加 END

End Class
