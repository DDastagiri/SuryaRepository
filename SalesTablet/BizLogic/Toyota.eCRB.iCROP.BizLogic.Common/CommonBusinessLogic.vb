'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'CommonBusinessLogic.vb
'─────────────────────────────────────
'機能： セッション情報用IF
'補足： 
'作成： 2012/02/09 TCS 鈴木(恭)
'更新： 
'─────────────────────────────────────

Imports Toyota.eCRB.SystemFrameworks.Web

Public Interface ICommonSessionControl
    Sub SetValueCommonBypass(pos As ScreenPos, key As String, value As Object)
    Function GetValueCommonBypass(pos As ScreenPos, key As String, removeFlg As Boolean) As Object
End Interface

