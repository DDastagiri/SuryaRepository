'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3340101BusinessLogic.vb
'─────────────────────────────────────
'機能：洗車マンメインメニュー(CW)のビジネスロジック
'補足： 
'作成：2015/01/05 TMEJ 範  　NextSTEPサービス 洗車用端末開発向けた評価用アプリ作成
'更新：2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発
'更新：
'─────────────────────────────────────

Option Strict On
Option Explicit On

Imports System.Globalization
Imports System.Reflection
Imports System.Web.Script.Serialization
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.CarWash.MainMenu.DataAccess
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.BizLogic
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.BizLogic.TabletSMBCommonClassBusinessLogic
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.DataAccess

Public Class SC3340101BusinessLogic
    Inherits BaseBusinessComponent
    Implements IDisposable

#Region "定数"
    ''' <summary>
    ''' 自画面のプログラムID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MY_PROGRA_MID As String = "SC3340101"

    ''' <summary>
    ''' 自画面のプログラムID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MIN_DATE As String = "1900/01/01 0:00:00"

#End Region

#Region "洗車情報/件数の取得処理"

    ''' <summary>
    ''' 洗車情報の取得
    ''' </summary>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inBranchCode">店舗コード</param>
    ''' <param name="inSelectCount">バナー取得件数</param>
    ''' <returns>洗車情報</returns>
    ''' <remarks></remarks>
    Public Function GetCarWashInfo(ByVal inDealerCode As String, _
                                   ByVal inBranchCode As String, _
                                   ByVal inSelectCount As Long) As SC3340101DataSet.SC3340101CarWashInfoDataTable

        'ログ出力
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                 "{0}_Start. inDealerCode={1}, inBranchCode={2}, inSelectCount={3}", _
                                 System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                 inDealerCode, _
                                 inBranchCode, _
                                 inSelectCount))

        Dim dtCarWash As SC3340101DataSet.SC3340101CarWashInfoDataTable

        Using adapter As New SC3340101DataSetTableAdapters.SC3340101DataTableAdapter

            '洗車情報の取得
            dtCarWash = adapter.GetCarWashInfo(inDealerCode, _
                                               inBranchCode, _
                                               inSelectCount)

        End Using

        '遅れ見込み時間を取得して、洗車情報に設定する
        dtCarWash = Me.GetDeliveryDelayDateList(inDealerCode, _
                                                inBranchCode, _
                                                dtCarWash)

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_End.Return count={1}", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  dtCarWash.Count))

        Return dtCarWash

    End Function

    ''' <summary>
    ''' 洗車情報件数取得
    ''' </summary>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inBranchCode">店舗コード</param>
    ''' <returns>洗車情報件数</returns>
    ''' <remarks></remarks>
    Public Function GetCarWashInfoCount(ByVal inDealerCode As String, _
                                        ByVal inBranchCode As String) As Long

        'ログ出力
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_Start. inDealerCode={1}, inBranchCode={2}", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  inDealerCode, _
                                  inBranchCode))

        Dim retCount As Long = 0

        Using adapter As New SC3340101DataSetTableAdapters.SC3340101DataTableAdapter

            '洗車情報件数取得
            Dim dtCarWashCount As SC3340101DataSet.SC3340101CarWashCountDataTable = _
                adapter.GetCarWashInfoCount(inDealerCode, inBranchCode)

            If dtCarWashCount.Count > 0 Then
                '取得した場合

                '件数を戻す
                retCount = dtCarWashCount(0).CAR_WASH_COUNT

            End If

        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_End Return={1}", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  retCount))

        Return retCount

    End Function


    ''' <summary>
    ''' 遅れ見込みリストを取得
    ''' </summary>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inBranchCode">店舗コード</param>
    ''' <param name="inCarWashDataTable">洗車情報テーブル</param>
    ''' <returns>遅れ見込み情報</returns>
    ''' <remarks></remarks>
    Private Function GetDeliveryDelayDateList(ByVal inDealerCode As String, _
                                              ByVal inBranchCode As String, _
                                              ByVal inCarWashDataTable As SC3340101DataSet.SC3340101CarWashInfoDataTable) As SC3340101DataSet.SC3340101CarWashInfoDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.Start.inDealerCode={1}, inBranchCode={2}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  inDealerCode, _
                                  inBranchCode))

        'サービス入庫ID
        Dim svcinIdList As New List(Of Decimal)

        For Each drCarWash As SC3340101DataSet.SC3340101CarWashInfoRow In inCarWashDataTable

            Dim svcinId As Decimal = drCarWash.SVCIN_ID

            '重複のサービス入庫IDをいれない
            If Not svcinIdList.Contains(svcinId) Then

                svcinIdList.Add(svcinId)

            End If

        Next

        Using clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic

            Dim dateTimeNow As Date = DateTimeFunc.Now(inDealerCode).Date

            '遅れ見込み列のデータを取得する
            Dim dtDelay As TabletSMBCommonClassDataSet.TabletSmbCommonClassDeliDelayDateDataTable = _
                clsTabletSMBCommonClass.GetDeliveryDelayDateList(svcinIdList, _
                                                                 inDealerCode, _
                                                                 inBranchCode, _
                                                                 dateTimeNow)

            Dim showScheDeliDateWord As String = WebWordUtility.GetWord(14)

            'HH:mm
            Dim strHhMm As String = DateTimeFunc.GetDateFormat(14)

            'MM/dd
            Dim strMmDd As String = DateTimeFunc.GetDateFormat(11)

            '引数テーブルに設定する
            For Each drCarWash As SC3340101DataSet.SC3340101CarWashInfoRow In inCarWashDataTable

                Dim svcinId As Decimal = drCarWash.SVCIN_ID

                Dim targetList As List(Of TabletSMBCommonClassDataSet.TabletSmbCommonClassDeliDelayDateRow) = _
                    (From p In dtDelay Where p.SVCIN_ID = svcinId Select p).ToList()

                If targetList.Count = 1 Then

                    drCarWash.PLAN_DELAYDATE = targetList(0).DELI_DELAY_DATETIME

                Else

                    drCarWash.PLAN_DELAYDATE = Date.Parse(MIN_DATE, CultureInfo.InvariantCulture)

                End If

                If drCarWash.SCHE_DELI_DATETIME = Date.Parse(MIN_DATE, CultureInfo.InvariantCulture) Then

                    '納車予定日がデフォルト日時の場合

                    '「-:-」文言を設定
                    drCarWash.SHOW_SCHE_DELIDATE = showScheDeliDateWord

                ElseIf drCarWash.SCHE_DELI_DATETIME.Date = dateTimeNow Then
                    '今日の場合

                    'HH:mm
                    drCarWash.SHOW_SCHE_DELIDATE = drCarWash.SCHE_DELI_DATETIME.ToString(strHhMm, CultureInfo.InvariantCulture)
                Else

                    'MM/dd
                    drCarWash.SHOW_SCHE_DELIDATE = drCarWash.SCHE_DELI_DATETIME.ToString(strMmDd, CultureInfo.InvariantCulture)

                End If

            Next

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}.End", _
                                      System.Reflection.MethodBase.GetCurrentMethod.Name))

            Return inCarWashDataTable

        End Using

    End Function

#End Region

#Region "洗車情報更新処理"

    ''' <summary>
    ''' 洗車開始登録
    ''' </summary>
    ''' <param name="inServiceInId">サービス入庫ID</param>
    ''' <param name="inJobDetailId">作業内容ID</param>
    ''' <param name="inStallUseId">ストール使用ID</param>
    ''' <param name="inRowLockVersion">行ロックバージョン</param>
    ''' <returns>実行結果</returns>
    ''' <remarks></remarks>
    Public Function RegisterCarWashStart(ByVal inServiceInId As Decimal, _
                                         ByVal inJobDetailId As Decimal, _
                                         ByVal inStallUseId As Decimal, _
                                         ByVal inRowlockVersion As Long) As Long

        'ログ出力
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_Start. inServiceInId={1}, inJobDetailId={2}, inStallUseId={3}, inRowlockVersion={4}", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  inServiceInId, _
                                  inJobDetailId, _
                                  inStallUseId, _
                                  inRowlockVersion))

        Dim retUpdate As Long = 0


        Using tabletSmbCommonClsBlz As New TabletSMBCommonClassBusinessLogic

            Try

                '洗車開始登録を行う
                retUpdate = _
                    tabletSmbCommonClsBlz.UpdateChipWashStart(inServiceInId, _
                                                              inJobDetailId, _
                                                              inStallUseId, _
                                                              inRowlockVersion, _
                                                              MY_PROGRA_MID)

                '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 START

                'If retUpdate = ActionResult.Success Then
                '    '成功の場合

                '    Dim userInfo As StaffContext = StaffContext.Current

                '    '指定SAにPush送信(自分以外)
                '    tabletSmbCommonClsBlz.SendNamedSAPush(inServiceInId, _
                '                                          userInfo.DlrCD, _
                '                                          userInfo.BrnCD, _
                '                                          userInfo.Account)

                '    'CW権限へPush送信
                '    tabletSmbCommonClsBlz.SendAllCwPush(userInfo.DlrCD, _
                '                                        userInfo.BrnCD, _
                '                                        userInfo.Account)

                'End If

                If retUpdate = ActionResult.Success _
                OrElse retUpdate = ActionResult.WarningOmitDmsError Then
                    '成功の場合
                    'DMS除外エラーの警告の場合

                    Dim userInfo As StaffContext = StaffContext.Current

                    '指定SAにPush送信(自分以外)
                    tabletSmbCommonClsBlz.SendNamedSAPush(inServiceInId, _
                                                          userInfo.DlrCD, _
                                                          userInfo.BrnCD, _
                                                          userInfo.Account)

                    'CW権限へPush送信
                    tabletSmbCommonClsBlz.SendAllCWPush(userInfo.DlrCD, _
                                                        userInfo.BrnCD, _
                                                        userInfo.Account)

                End If

                '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 END

            Catch ex As OracleExceptionEx When ex.Number = 1013

                'タイムアウトエラーを戻す
                retUpdate = ActionResult.DBTimeOutError

                Logger.Error(String.Format(CultureInfo.CurrentCulture, _
                                           "{0} Error={1} ", _
                                           MethodBase.GetCurrentMethod.Name, _
                                           ex))

            End Try


        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_End Return={1}", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  retUpdate))

        Return retUpdate

    End Function

    ''' <summary>
    ''' 洗車Undo登録
    ''' </summary>
    ''' <param name="inServiceInId">サービス入庫ID</param>
    ''' <param name="inJobDetailId">作業内容ID</param>
    ''' <param name="inStallUseId">サービス入庫ID</param>
    ''' <param name="inRowLockVersion">行ロックバージョン</param>
    ''' <returns>実行結果</returns>
    ''' <remarks></remarks>
    Public Function RegisterCarWashUndo(ByVal inServiceInId As Decimal, _
                                        ByVal inJobDetailId As Decimal, _
                                        ByVal inStallUseId As Decimal, _
                                        ByVal inRowlockVersion As Long) As Long

        'ログ出力
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_Start. inServiceInId={1}, inJobDetailId={2}, inStallUseId={3}, inRowlockVersion={4}", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  inServiceInId, _
                                  inJobDetailId, _
                                  inStallUseId, _
                                  inRowlockVersion))

        Dim retUpdate As Long = 0


        Using tabletSmbCommonClsBlz As New TabletSMBCommonClassBusinessLogic

            Try

                '洗車Undo登録を行う
                retUpdate = _
                    tabletSmbCommonClsBlz.UndoWashingChip(inServiceInId, _
                                                          inJobDetailId, _
                                                          inStallUseId, _
                                                          inRowlockVersion, _
                                                          MY_PROGRA_MID)

                '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 START

                'If retUpdate = ActionResult.Success Then
                '    '成功の場合

                '    'Undo通知出す
                '    tabletSmbCommonClsBlz.SendNoticeByUndoWashingChip(inServiceInId)

                'End If

                If retUpdate = ActionResult.Success _
                OrElse retUpdate = ActionResult.WarningOmitDmsError Then
                    '成功の場合
                    'DMS除外エラーの警告の場合

                    'Undo通知出す
                    tabletSmbCommonClsBlz.SendNoticeByUndoWashingChip(inServiceInId)

                End If

                '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 END

            Catch ex As OracleExceptionEx When ex.Number = 1013

                'タイムアウトエラーを戻す
                retUpdate = ActionResult.DBTimeOutError

                Logger.Error(String.Format(CultureInfo.CurrentCulture, _
                                           "{0} Error={1} ", _
                                           MethodBase.GetCurrentMethod.Name, _
                                           ex))

            End Try

        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_End Return={1}", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  retUpdate))

        Return retUpdate

    End Function

    ''' <summary>
    ''' 洗車スキップ登録
    ''' </summary>
    ''' <param name="inServiceInId">サービス入庫ID</param>
    ''' <param name="inJobDetailId">作業内容ID</param>
    ''' <param name="inStallUseId">ストール利用ID</param>
    ''' <param name="inPickDeliType">引取納車区分</param>
    ''' <param name="inRowlockVersion">行ロックバージョン</param>
    ''' <param name="inRONumber">RO番号</param>
    ''' <returns>実行結果</returns>
    ''' <remarks>納車待ちへ移動処理</remarks>
    Public Function RegisterCarWashSkip(ByVal inServiceInId As Decimal, _
                                        ByVal inJobDetailId As Decimal, _
                                        ByVal inStallUseId As Decimal, _
                                        ByVal inPickDeliType As String, _
                                        ByVal inRowlockVersion As Long, _
                                        ByVal inRONumber As String) As Long

        'ログ出力
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_Start. inServiceInId={1}, inJobDetailId={2}, inStallUseId={3}, inPickDeliType={4}, inRowlockVersion={5}, inRONumber={6}", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  inServiceInId, _
                                  inJobDetailId, _
                                  inStallUseId, _
                                  inPickDeliType, _
                                  inRowlockVersion, _
                                  inRONumber))

        Dim retUpdate As Long = 0

        Using tabletSmbCommonClsBlz As New TabletSMBCommonClassBusinessLogic

            Try

                '納車待ちへ移動処理
                retUpdate = _
                    tabletSmbCommonClsBlz.ChipMoveToDeliWait(inServiceInId, _
                                                             inJobDetailId, _
                                                             inStallUseId, _
                                                             inPickDeliType, _
                                                             inRowlockVersion, _
                                                             MY_PROGRA_MID, _
                                                             inRONumber)

                '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 START

                'If retUpdate = ActionResult.Success Then
                '    '成功の場合

                '    '納車待ちへ移動Push処理を行う
                '    tabletSmbCommonClsBlz.ToDeliWaitNoticePush()

                'End If

                If retUpdate = ActionResult.Success _
                OrElse retUpdate = ActionResult.WarningOmitDmsError Then
                    '成功の場合
                    'DMS除外エラーの警告の場合

                    '納車待ちへ移動Push処理を行う
                    tabletSmbCommonClsBlz.ToDeliWaitNoticePush()

                End If

                '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 END

            Catch ex As OracleExceptionEx When ex.Number = 1013

                'タイムアウトエラーを戻す
                retUpdate = ActionResult.DBTimeOutError

                Logger.Error(String.Format(CultureInfo.CurrentCulture, _
                                           "{0} Error={1} ", _
                                           MethodBase.GetCurrentMethod.Name, _
                                           ex))

            End Try

        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_End Return={1}", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  retUpdate))

        Return retUpdate


    End Function

    ''' <summary>
    ''' 洗車終了登録
    ''' </summary>
    ''' <param name="inServiceInId">サービス入庫ID</param>
    ''' <param name="inJobDetailId">作業内容ID</param>
    ''' <param name="inStallUseId">ストール利用ID</param>
    ''' <param name="inPickDeliType">引取納車区分</param>
    ''' <param name="inRowlockVersion">行ロックバージョン</param>
    ''' <param name="inRONumber">RO番号</param>
    ''' <returns>実行結果</returns>
    ''' <remarks></remarks>
    Public Function RegisterCarWashFinish(ByVal inServiceInId As Decimal, _
                                          ByVal inJobDetailId As Decimal, _
                                          ByVal inStallUseId As Decimal, _
                                          ByVal inPickDeliType As String, _
                                          ByVal inRowlockVersion As Long, _
                                          ByVal inRONumber As String) As Long

        'ログ出力
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_Start. inServiceInId={1}, inJobDetailId={2}, inStallUseId={3}, inPickDeliType={4}, inRowlockVersion={5}, inRONumber={6}", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  inServiceInId, _
                                  inJobDetailId, _
                                  inStallUseId, _
                                  inPickDeliType, _
                                  inRowlockVersion, _
                                  inRONumber))

        Dim retUpdate As Long = 0


        Using tabletSmbCommonClsBlz As New TabletSMBCommonClassBusinessLogic

            Try

                '洗車終了登録を行う
                retUpdate = _
                    tabletSmbCommonClsBlz.UpdateChipWashEnd(inServiceInId, _
                                                            inJobDetailId, _
                                                            inStallUseId, _
                                                            inPickDeliType, _
                                                            inRowlockVersion, _
                                                            MY_PROGRA_MID, _
                                                            inRONumber)

                '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 START

                'If retUpdate = ActionResult.Success Then

                '    Dim userInfo As StaffContext = StaffContext.Current

                '    '通知情報を取得
                '    Dim dtNoticeInfo As TabletSMBCommonClassDataSet.TabletSmbCommonClassNoticeInfoDataTable = Nothing
                '    dtNoticeInfo = _
                '            tabletSmbCommonClsBlz.GetSendNoticeInfo(inServiceInId, _
                '                                                    userInfo.DlrCD, _
                '                                                    userInfo.BrnCD)

                '    If dtNoticeInfo.Count > 0 Then
                '        '通知取得した場合

                '        '通知処理
                '        tabletSmbCommonClsBlz.WashEndNoticePush(dtNoticeInfo, userInfo)

                '    Else

                '        Logger.Error(String.Format(CultureInfo.CurrentCulture, _
                '                    "{0} Function GetSendNoticeInfo return count is 0 ", _
                '                    MethodBase.GetCurrentMethod.Name))

                '    End If

                'End If

                If retUpdate = ActionResult.Success _
                OrElse retUpdate = ActionResult.WarningOmitDmsError Then
                    '成功の場合
                    'DMS除外エラーの警告の場合

                    Dim userInfo As StaffContext = StaffContext.Current

                    '通知情報を取得
                    Dim dtNoticeInfo As TabletSMBCommonClassDataSet.TabletSmbCommonClassNoticeInfoDataTable = Nothing
                    dtNoticeInfo = _
                            tabletSmbCommonClsBlz.GetSendNoticeInfo(inServiceInId, _
                                                                    userInfo.DlrCD, _
                                                                    userInfo.BrnCD)

                    If dtNoticeInfo.Count > 0 Then
                        '通知取得した場合

                        '通知処理
                        tabletSmbCommonClsBlz.WashEndNoticePush(dtNoticeInfo, userInfo)

                    Else

                        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                                   "{0} Function GetSendNoticeInfo return count is 0 ", _
                                                   MethodBase.GetCurrentMethod.Name))

                    End If

                End If

                '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 END

            Catch ex As OracleExceptionEx When ex.Number = 1013

                'タイムアウトエラーを戻す
                retUpdate = ActionResult.DBTimeOutError

                Logger.Error(String.Format(CultureInfo.CurrentCulture, _
                                           "{0} Error={1} ", _
                                           MethodBase.GetCurrentMethod.Name, _
                                           ex))

            End Try

        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_End Return={1}", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  retUpdate))

        Return retUpdate

    End Function

#End Region

#Region "JSON変換"

    ''' <summary>
    '''   DataTableをJSON文字列に変換する
    ''' </summary>
    ''' <param name="dataTable">変換対象 DataSet</param>
    ''' <returns>JSON文字列</returns>
    Public Function DataTableToJson(ByVal dataTable As DataTable) As String

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.Start.", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))

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

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.End", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return JSerializer.Serialize(resultMain)

    End Function

#End Region

#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            ' TODO: set large fields to null.
        End If
        Me.disposedValue = True
    End Sub

    ' TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
    'Protected Overrides Sub Finalize()
    '    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class
