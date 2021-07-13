'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'CommonBusinessLogic.vb
'─────────────────────────────────────
'機能： セッション情報用IF
'補足： 
'作成： 2013/02/20 TMEJ 明瀬
'更新： 
'─────────────────────────────────────

Imports Toyota.eCRB.SystemFrameworks.Web

Public Interface ICommonSessionControl
    Sub SetValueCommonBypass(pos As ScreenPos, key As String, value As Object)
    Function GetValueCommonBypass(pos As ScreenPos, key As String, removeFlg As Boolean) As Object
    Function ContainsKeyBypass(pos As ScreenPos, key As String) As Boolean
End Interface

