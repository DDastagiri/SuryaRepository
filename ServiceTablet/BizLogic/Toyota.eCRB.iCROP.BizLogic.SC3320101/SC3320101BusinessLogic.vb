'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3320101BusinessLogic.vb
'─────────────────────────────────────
'機能：メインメニュー(ASA)のビジネスロジック
'補足： 
'作成：2014/08/14 TMEJ 丁 NextSTEPサービス 作業進捗管理に向けたシステム構想検討
'更新： 
'─────────────────────────────────────

Imports System.Xml
Imports System.Net
Imports System.IO
Imports System.Globalization
Imports System.Reflection
Imports System.Xml.Serialization
Imports System.Text.RegularExpressions
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports System.Text
Imports System.Web
Imports Toyota.eCRB.AssistantSA.MainMenu.DataAccess
Public Class SC3320101BusinessLogic
    Inherits BaseBusinessComponent
    Implements IDisposable

#Region "定数"
    ''' <summary>
    ''' 自画面のプログラムID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MY_PROGRAMID As String = "SC3320101"

    ''' <summary>
    ''' 処理結果コード： [0:成功]
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SUCCESAS As Integer = 0

    ''' <summary>
    ''' 処理結果コード： [-1:予期せぬエラー]
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ERR_CODE_UNEXPECTED As Integer = -1

    ''' <summary>
    ''' N日分前のデータを取得システム設定値名
    ''' </summary>
    Private Const SYS_GET_BEFORE_NDAYS_VAL = "ASA_DISP_DAYS"
#End Region

#Region "Publicメソッド"
    ''' <summary>
    ''' メイン画面表示するための来店者情報取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetInitInfoForDisplay() As SC3320101DataSet.SC3320101VisitInfoDataTable
        'ログ出力
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                          "{0}_S.", _
                          System.Reflection.MethodBase.GetCurrentMethod.Name))

        'スタフ情報
        Dim userContext As StaffContext = StaffContext.Current
        Dim dlrCode As String = userContext.DlrCD
        Dim brnCode As String = userContext.BrnCD

        'N日分前のデータを取得システム設定値取得
        Dim nDays As String = String.Empty

        'DealerEnvSettingのインスタンス
        Dim dealerEnvBiz As New DealerEnvSetting

        'DealerEnvSettingの取得処理( N日分前のデータを取得システム設定値)
        Dim drDealerEnvSetting As DlrEnvSettingDataSet.DLRENVSETTINGRow = _
        dealerEnvBiz.GetEnvSetting(dlrCode, SYS_GET_BEFORE_NDAYS_VAL)

        '取得できた場合のみ設定する
        If Not (IsNothing(drDealerEnvSetting)) Then

            nDays = drDealerEnvSetting.PARAMVALUE

        End If

        '取得できなかった場合は当日のデータのみ取得する
        If String.IsNullOrEmpty(nDays) Then

            nDays = "0"

        End If

        Using sc3320101DataAdapter As New SC3320101DataSetTableAdapters.SC3320101DataTableAdapter
            '来店者情報取得
            Dim dtVisitInfo As SC3320101DataSet.SC3320101VisitInfoDataTable = _
                sc3320101DataAdapter.GetServiceVisitInfoForDisplay(nDays, _
                                                                   dlrCode, _
                                                                   brnCode)

            'ログ出力
            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                          "{0}_E　count={1}", _
                          System.Reflection.MethodBase.GetCurrentMethod.Name, _
                          dtVisitInfo.Count))

            Return dtVisitInfo
        End Using

    End Function


    ''' <summary>
    ''' ロケーション情報更新
    ''' </summary>
    ''' <param name="drVisitInfo">更新用来店情報データ行</param>
    ''' <param name="updateAccount">更新アカウント</param>
    ''' <param name="updateDate">更新日時</param>
    ''' <returns>結果コード</returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Function UpdLocationInfo(ByVal drVisitInfo As SC3320101DataSet.SC3320101VisitInfoRow, _
                                    ByVal updateDate As Date, _
                                    ByVal updateAccount As String) As Integer
        'ログ出力
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                          "{0}_S. visitSeq={1} parkingCode={2} updateDate={3} updateAccount={4}", _
                          System.Reflection.MethodBase.GetCurrentMethod.Name, _
                          drVisitInfo.VISITSEQ, _
                          drVisitInfo.PARKINGCODE, _
                          updateDate, _
                          updateAccount))

        Using sc3320101DataAdapter As New SC3320101DataSetTableAdapters.SC3320101DataTableAdapter

            Dim updCount As Integer
            'DB更新実行
            updCount = sc3320101DataAdapter.UpdParkingInfo(drVisitInfo, _
                                                         updateDate, _
                                                         updateAccount)

            If updCount <> 1 Then
                '更新エラー
                'ログ出力
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} RETURNCODE = {2} " _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , ERR_CODE_UNEXPECTED))
                Return ERR_CODE_UNEXPECTED
            End If

        End Using

        'ログ出力
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_E RETURNCODE={1}", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  SUCCESAS))
        'DB更新成功
        Return SUCCESAS

    End Function

    ' ''' <summary>
    ' ''' システム設定値を設定値名を条件に取得する
    ' ''' </summary>
    ' ''' <param name="settingName">システム設定値名</param>
    ' ''' <param name="inDlrCode">販売店コード</param>
    ' ''' <param name="inBrnCode">店舗コード</param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Function GetSystemSettingValueBySettingName(ByVal settingName As String, _
    '                                                   ByVal inDlrCode As String, _
    '                                                   ByVal inBrnCode As String) As String

    '    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
    '                              "{0}_S IN:settingName={1}", _
    '                              System.Reflection.MethodBase.GetCurrentMethod.Name, _
    '                              settingName))

    '    '戻り値
    '    Dim retValue As String = String.Empty

    '    '自分のテーブルアダプタークラスインスタンスを生成
    '    Using ta As New SC3320101DataSetTableAdapters.SC3320101DataTableAdapter
    '        'システム設定から取得
    '        Dim dt As SC3320101DataSet.SC3320101SystemSettingDataTable _
    '            = ta.GetSystemSettingValue(settingName, _
    '                                       inDlrCode, _
    '                                       inBrnCode)

    '        If 0 < dt.Count Then

    '            '設定値を取得
    '            retValue = dt.Item(0).SETTING_VAL

    '        End If

    '    End Using

    '    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
    '                              "{0}_S OUT:{1}={2}", _
    '                              System.Reflection.MethodBase.GetCurrentMethod.Name, _
    '                              settingName, _
    '                              retValue))

    '    Return retValue

    'End Function

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

End Class
