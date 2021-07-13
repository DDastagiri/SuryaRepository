'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3080227BusinessLogic.vb
'─────────────────────────────────────
'機能： 顧客イメージ画像取得のビジネスロジック
'補足： 
'作成： 2017/01/05 NSK 荒川
'更新： 
'──────────────────────────────────


Imports System
Imports System.Globalization
Imports System.Reflection
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.BizLogic
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.DataAccess
Imports Toyota.eCRB.iCROP.DataAccess.SC3080227
Imports Toyota.eCRB.iCROP.DataAccess.SC3080227.SC3080227DataSetTableAdapters
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic


Public Class SC3080227BusinessLogic
    Inherits BaseBusinessComponent
    Implements IDisposable

#Region "定数"

    ''' <summary>
    ''' 全販売店を意味するワイルドカード販売店コード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const AllDealerCode As String = "XXXXX"

#End Region

#Region "公開メソッド"

    ''' <summary>
    ''' 顧客イメージ画像パス取得
    ''' </summary>
    ''' <param name="DMSDealerCode">基幹販売店コード</param>
    ''' <param name="DMSCustCdDisp">基幹顧客コード(表示用)</param>
    ''' <param name="imageSize">画像サイズ</param>
    ''' <returns>顧客イメージ画像パス（取得失敗時はNothing）</returns>
    ''' <remarks></remarks>
    Public Function GetCustImageFilePath(ByVal DMSDealerCode As String, _
                                         ByVal DMSCustCdDisp As String, _
                                         ByVal imageSize As String) As String

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                              "{0} Start DMSDealerCode:[{1}] DMSCustCdDisp:[{2}] imageSize:[{3}]",
                              MethodBase.GetCurrentMethod.Name,
                              DMSDealerCode, DMSCustCdDisp, imageSize))

        '引数が不正な場合は終了
        If String.IsNullOrEmpty(DMSDealerCode) OrElse String.IsNullOrEmpty(DMSCustCdDisp) Then
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} End Return:[Nothing]",
                                  MethodBase.GetCurrentMethod.Name))
            Return Nothing
        End If

        Dim serviceCommonBizLogic As New ServiceCommonClassBusinessLogic

        'DMSの販売店コードをi-CROPの販売店コードに変換
        Dim iCROPCodeDt As ServiceCommonClassDataSet.DmsCodeMapDataTable _
            = serviceCommonBizLogic.GetDmsToIcropCode(AllDealerCode, _
                                                      ServiceCommonClassBusinessLogic.DmsCodeType.DealerCode, _
                                                      DMSDealerCode, String.Empty, String.Empty)

        '販売店コードの変換に失敗したら終了
        If iCROPCodeDt.Count <= 0 Then
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} End Return:[Nothing]",
                                  MethodBase.GetCurrentMethod.Name))
            Return Nothing
        End If

        '顧客イメージ画像がアップロードされるURLを取得
        Dim sysEnv As New SystemEnvSetting
        Dim custImageUrl As String = sysEnv.GetSystemEnvSetting("FACEPIC_UPLOADURL").PARAMVALUE

        '取得に失敗したら終了
        If String.IsNullOrEmpty(custImageUrl) Then
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} End Return:[Nothing]",
                                  MethodBase.GetCurrentMethod.Name))
            Return Nothing
        End If

        Dim custImageFileName As String '顧客イメージ画像ファイル名

        Using da As New SC3080227TableAdapter

            '顧客イメージ画像ファイル名取得
            Dim dt As SC3080227DataSet.CustImageFileDataTable _
                = da.GetCustImageFile(iCROPCodeDt.Rows(0).Item("CODE1").ToString(), _
                                      DMSCustCdDisp, imageSize)

            If dt.Count <= 0 Then
                'ファイル名が取得できなければ終了
                Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} End Return:[Nothing]",
                                  MethodBase.GetCurrentMethod.Name))
                Return Nothing
            End If

            custImageFileName = dt.Rows(0).Item("IMAGEFILE_NAME").ToString()
        End Using

        Dim custImageFilePath As String = custImageUrl + custImageFileName 'URLとファイル名を連結

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} End Return:[{1}]",
                                  MethodBase.GetCurrentMethod.Name,
                                  custImageFilePath))

        Return custImageFilePath
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
