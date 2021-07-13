'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3010501BusinessLogic.vb
'─────────────────────────────────────
'機能： サービス用共通関数処理
'補足： 
'作成： 2013/07/24 TMEJ小澤	初版作成
'更新： 
'─────────────────────────────────────
Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.Common.OtherLinkage.DataAccess.SC3010501DataSet
Imports Toyota.eCRB.Common.OtherLinkage.DataAccess.SC3010501DataSetTableAdapters
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.DataAccess.ServiceCommonClassDataSet
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.BizLogic

Public Class SC3010501BusinessLogic
    Inherits BaseBusinessComponent
    Implements IDisposable

#Region "定数"

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
        ErrDBTimeout = 901

    End Enum

#End Region

#Region "メイン処理"

    ''' <summary>
    ''' 画面URL情報取得
    ''' </summary>
    ''' <param name="inDisplayNumber">表示番号</param>
    ''' <returns>URL情報</returns>
    ''' <remarks></remarks>
    Public Function GetDisplayUrl(ByVal inDisplayNumber As Long) As SC3010501DisplayRelationDataTable
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START IN:inDisplayNumber = {2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , inDisplayNumber.ToString(CultureInfo.CurrentCulture)))

        Using da As New SC3010501DataTableAdapter
            '画面URL情報取得
            Dim dt As SC3010501DisplayRelationDataTable = _
                da.GetDisplayUrl(inDisplayNumber)

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return dt
        End Using
    End Function

    ''' <summary>
    ''' DMS情報取得
    ''' </summary>
    ''' <param name="inStaffInfo">sスタッフ情報</param>
    ''' <returns>DMS情報</returns>
    ''' <remarks></remarks>
    Public Function GetDmsDealerData(ByVal inStaffInfo As StaffContext) As DmsCodeMapDataTable
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START " _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))


        Using biz As New ServiceCommonClassBusinessLogic
            'DMS販売店データの取得
            Dim dtDmsCodeMapDataTable As DmsCodeMapDataTable = _
                biz.GetIcropToDmsCode(inStaffInfo.DlrCD,
                                      ServiceCommonClassBusinessLogic.DmsCodeType.BranchCode, _
                                      inStaffInfo.DlrCD, _
                                      inStaffInfo.BrnCD, _
                                      String.Empty, _
                                      inStaffInfo.Account)

            If dtDmsCodeMapDataTable.Count <= 0 Then
                'データが取得できない場合はエラー
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} ERROR:TB_M_DMS_CODE_MAP is nothing" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return Nothing

            ElseIf 1 < dtDmsCodeMapDataTable.Count Then
                'データが2件以上取得できた場合は一意に決定できないためエラー
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} ERROR:TB_M_DMS_CODE_MAP is sum data" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return Nothing
            Else
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} END " _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return dtDmsCodeMapDataTable

            End If

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

End Class

