'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3070210BusinessLogic.vb
'─────────────────────────────────────
'機能： 相談履歴
'補足： 
'作成： 2015/03/17 TCS 鈴木  次世代e-CRB 価格相談履歴参照機能開発
'─────────────────────────────────────

Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.Estimate.Quotation.DataAccess
Imports System.Reflection.MethodBase
Imports System.Globalization

Public Class SC3070210BusinessLogic
    Inherits BaseBusinessComponent

    Public Function GetDiscountApproval(ByVal estimateId As Long) As SC3070210DataSet.SC3070210DISCOUNTAPPROVALDataTable
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_Start", GetCurrentMethod.Name), True)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name), True)
        Return SC3070210TableAdapter.GetDiscountApproval(estimateId)
    End Function

    Public Function GetContracatApproval(ByVal estimateId As Long) As SC3070210DataSet.SC3070210CONTRACTAPPROVALDataTable
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_Start", GetCurrentMethod.Name), True)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name), True)
        Return SC3070210TableAdapter.GetContracatApproval(estimateId)
    End Function

    Public Function IsBookedVehicleDelivered(ByVal estimateId As Long) As Boolean
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_Start", GetCurrentMethod.Name), True)

        Dim delivered As Boolean = False

        Dim request As SC3070210DataSet.SC3070210REQUESTDataTable = SC3070210TableAdapter.GetRequestInfo(estimateId)
        If (0 < request.Count AndAlso request(0).REQ_STATUS = "31") Then
            '商談活動がSuccessで完了している
            Dim vehicle As SC3070210DataSet.SC3070210VEHICLEDataTable = SC3070210TableAdapter.GetBookedVehicle(estimateId)
            If (0 < vehicle.Count AndAlso 1900 < vehicle(0).DELI_DATE.Date.Year) Then
                '基幹連携(G03)により納車日が連携された
                delivered = True
            Else
                Dim processCount As Integer = SC3070210TableAdapter.GetBookedAfterProcessCount(estimateId)
                If (processCount = 0) Then
                    '関連する受注後活動の必須活動が全て完了している（HISTORYに退避されている）
                    delivered = True
                End If
            End If
        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name), True)
        Return delivered
    End Function

End Class
