'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'UnallocatedCustomerBusinessLogic.vb
'─────────────────────────────────────
'機能： 顧客担当未割り当て件数取得API
'補足： 
'作成： 2014/05/30 TCS藤井 セールスタブレットMGR機能  
'─────────────────────────────────────

Imports Toyota.eCRB.CommonUtility.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web
Imports System.Globalization
Imports System.Reflection
Imports System.Reflection.MethodBase

Public Class UnallocatedCustomerBusinessLogic
    Inherits BaseBusinessComponent

#Region "メソッド"
    ''' <summary>
    ''' デフォルトコンストラクタ
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        '処理なし
    End Sub

    ''' <summary>
    '''  顧客担当未割り当て件数取得
    ''' </summary>
    ''' <param name="dlrcd">顧客担当の販売店コード</param>
    ''' <param name="brncd">顧客担当の店舗コード</param>
    ''' <returns>顧客担当未割り当て件数</returns>
    ''' <remarks></remarks>
    Public Function GetStaffAssignToCustCount(ByVal dlrcd As String, ByVal brncd As String) As Integer

        Dim count As Integer

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_Start", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        count = UnallocatedCustomerTableAdapter.GetStaffAssignToCustCount(dlrcd, brncd)

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_End", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        Return count

    End Function
#End Region

End Class
