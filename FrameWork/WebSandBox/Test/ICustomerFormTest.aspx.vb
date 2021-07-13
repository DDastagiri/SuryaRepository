Imports Toyota.eCRB.SystemFrameworks.Web

Partial Class Test_ICustomerFormTest
    Inherits BasePage
    Implements ICustomerForm

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        Dim authManager As New AuthenticationManager
        authManager.Auth("200003@44B40", "icrop", "")

    End Sub

    Public ReadOnly Property DefaultOperationLocked As Boolean Implements ICustomerForm.DefaultOperationLocked
        Get
            Return True
        End Get
    End Property

End Class
