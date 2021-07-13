
Partial Class Pages_SC999000Stub
    Inherits BasePage

    Protected Sub Pages_SC999000Stub_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
        Dim QueryString As String = Uri.UnescapeDataString(sender.ClientQueryString)
        Dim KeyValues = QueryString.Split("&").Where(Function(ws) Not String.IsNullOrWhiteSpace(ws)).Select(Function(p) p.Split("=")).Select(
            Function(kv)
                Return New KeyValuePair(Of String, String)(kv(0), kv(1))
            End Function)

        CustomLabel0.Text = QueryString
        For Each kv In KeyValues

            If (kv.Key = "Account") Then
                CustomLabel1.Text = kv.Value

            End If
            If (kv.Key = "Dlrcd") Then
                CustomLabel2.Text = kv.Value

            End If
            If (kv.Key = "Strcd") Then
                CustomLabel3.Text = kv.Value

            End If
            If (kv.Key = "EstimateId") Then
                CustomLabel4.Text = kv.Value

            End If
            If (kv.Key = "SelectedEstimateId") Then
                CustomLabel5.Text = kv.Value

            End If
            If (kv.Key = "SalesFlg") Then
                CustomLabel6.Text = kv.Value

            End If
            If (kv.Key = "DispModeFlg") Then
                CustomLabel7.Text = kv.Value

            End If
            If (kv.Key = "ApprovalStatus") Then
                CustomLabel8.Text = kv.Value

            End If
            If (kv.Key = "NoCustomerFlg") Then
                CustomLabel9.Text = kv.Value

            End If
            If (kv.Key = "DirectBillingFlag") Then
                CustomLabel10.Text = kv.Value

            End If
            If (kv.Key = "CustomerCode") Then
                CustomLabel11.Text = kv.Value

            End If
            If (kv.Key = "NewCustomerID") Then
                CustomLabel12.Text = kv.Value

            End If
        Next
    End Sub
End Class
