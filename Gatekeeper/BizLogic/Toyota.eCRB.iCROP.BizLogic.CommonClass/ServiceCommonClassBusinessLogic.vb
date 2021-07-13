'-------------------------------------------------------------------------
'ServiceCommonClassBusinessLogic.vb
'-------------------------------------------------------------------------
'機能：サービス共通関数API
'補足：
'作成：2019/02/28 NSK 河谷 REQ-SVT-TMT-20180601-001 Gate Keeper機能の視認性操作性改善
'更新：
'─────────────────────────────────────
Imports System.Text
Imports System.Net
Imports System.IO
Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.DataAccess

Public Class ServiceCommonClassBusinessLogic
    Inherits BaseBusinessComponent
    Implements IDisposable

#Region "定数"

    ''' <summary>
    ''' 全販売店を意味するワイルドカード販売店コード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const AllDealerCode As String = "XXXXX"

    ''' <summary>
    ''' 全店舗を意味するワイルドカード店舗コード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const AllBranchCode As String = "XXX"

#End Region

#Region "Publicメソッド"

    ''' <summary>
    ''' システム設定値を設定値名を条件に取得する
    ''' </summary>
    ''' <param name="settingName">システム設定値名</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetSystemSettingValueBySettingName(ByVal settingName As String) As String

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_S IN:settingName={1}", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  settingName))

        '戻り値
        Dim retValue As String = String.Empty

        '自分のテーブルアダプタークラスインスタンスを生成
        Using ta As New ServiceCommonClassDataSetTableAdapters.ServiceCommonClassTableAdapter

            'システム設定から取得
            Dim dt As ServiceCommonClassDataSet.SystemSettingDataTable _
                = ta.GetSystemSettingValue(settingName)

            If 0 < dt.Count Then

                '設定値を取得
                retValue = dt.Item(0).SETTING_VAL

            End If

        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_E OUT:{1}={2}", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  settingName, _
                                  retValue))

        Return retValue

    End Function

    ''' <summary>
    ''' 販売店システム設定値を設定値名を条件に取得する
    ''' </summary>
    ''' <param name="settingName">販売店システム設定値名</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDlrSystemSettingValueBySettingName(ByVal settingName As String) As String

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_S IN:settingName={1}", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  settingName))

        '戻り値
        Dim retValue As String = String.Empty

        'ログイン情報
        Dim userContext As StaffContext = StaffContext.Current

        '自分のテーブルアダプタークラスインスタンスを生成
        Using ta As New ServiceCommonClassDataSetTableAdapters.ServiceCommonClassTableAdapter

            '販売店システム設定から取得
            Dim dt As ServiceCommonClassDataSet.SystemSettingDataTable _
                                    = ta.GetDlrSystemSettingValue(userContext.DlrCD, _
                                                                              userContext.BrnCD, _
                                                                              AllDealerCode, _
                                                                              AllBranchCode, _
                                                                              settingName)

            If 0 < dt.Count Then

                '設定値を取得
                retValue = dt.Item(0).SETTING_VAL

            End If

        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_E OUT:{1}={2}", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  settingName, _
                                  retValue))

        Return retValue

    End Function

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

    ' このコードは、破棄可能なパターンを正しく実装できるように Visual Basic によって追加されました。
    Public Sub Dispose() Implements IDisposable.Dispose
        ' このコードを変更しないでください。クリーンアップ コードを上の Dispose(ByVal disposing As Boolean) に記述します。
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

#End Region

End Class