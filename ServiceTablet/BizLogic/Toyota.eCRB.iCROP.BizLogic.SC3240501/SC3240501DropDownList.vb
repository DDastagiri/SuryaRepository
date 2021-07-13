'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3240501DropDownList.vb
'─────────────────────────────────────
'機能： 新規予約作成
'補足： 
'作成： 2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発
'更新： 
'─────────────────────────────────────

Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Text
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Collections.Specialized

Public Class SC3240501DropDownList
    Inherits DropDownList

    Public Overloads Function LoadPostData(postDataKey As String, _
       postCollection As NameValueCollection) As Boolean

        Dim postedValue As String = postCollection(postDataKey)

        If (postedValue Is Nothing AndAlso _
            Me.Items.FindByValue(postedValue) Is Nothing) Then

            Me.Items.Add(postedValue)
            Me.SelectedValue = postedValue

        End If

        Return MyBase.LoadPostData(postDataKey, postCollection)

    End Function

End Class
