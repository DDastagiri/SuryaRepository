Imports Toyota.eCRB.SystemFrameworks.Web
Public Interface IEstimateInfoControl
    Sub SetValueBypass(pos As ScreenPos, key As String, value As Object)
    Function GetValueBypass(pos As ScreenPos, key As String, removeFlg As Boolean) As Object
    Sub ShowMessageBoxBypass(wordNo As Integer, ParamArray wordParam() As String)
    Function ContainsKeyBypass(pos As Toyota.eCRB.SystemFrameworks.Web.ScreenPos, key As String) As Boolean
    Sub RemoveValueBypass(pos As Toyota.eCRB.SystemFrameworks.Web.ScreenPos, key As String)
    Function OperationLockedBypass() As Boolean
End Interface
