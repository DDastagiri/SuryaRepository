'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'TemplateInfo.vb
'─────────────────────────────────────
'機能： テンプレート取得インターフェイス テンプレート取得情報クラス
'補足： 
'作成： 2014/05/13 TMEJ 曽山
'更新： 
'─────────────────────────────────────

Imports System.Text
Imports Toyota.eCRB.SystemFrameworks.Core

''' <summary>
''' テンプレート取得情報クラス
''' </summary>
''' <remarks></remarks>
Public Class TemplateInfo

    ''' <summary>
    ''' 販売店コード
    ''' </summary>
    Public Property DealerCode As String = String.Empty

    ''' <summary>
    ''' 店舗コード
    ''' </summary>
    Public Property StoreCode As String = String.Empty

    ''' <summary>
    ''' テンプレート区分
    ''' </summary>
    Public Property TemplateClass As String = String.Empty

    ''' <summary>
    ''' 画面ID
    ''' </summary>
    Public Property DisplayID As String = String.Empty

    ''' <summary>
    ''' 顧客ID
    ''' </summary>
    Public Property CustomId As String = String.Empty

    ''' <summary>
    ''' スタッフコード
    ''' </summary>
    Public Property StaffCode As String = String.Empty

    ''' <summary>
    ''' 商談ID
    ''' </summary>
    Public Property SalesID As String = String.Empty

    ''' <summary>
    ''' このインスタンスの文字列表現を返却する。
    ''' </summary>
    ''' <returns>インスタンスの文字列表現を返却する。</returns>
    Public Overloads Function ToString() As String
        Dim sb As New StringBuilder
        With sb
            .Append(LogUtil.GetLogParam("DealerCode", Me.DealerCode, False))
            .Append(LogUtil.GetLogParam("StoreCode", Me.StoreCode, True))
            .Append(LogUtil.GetLogParam("TemplateClass", Me.TemplateClass, True))
            .Append(LogUtil.GetLogParam("DisplayID", Me.DisplayID, True))
            .Append(LogUtil.GetLogParam("CustomId", Me.CustomId, True))
            .Append(LogUtil.GetLogParam("StaffCode", Me.StaffCode, True))
            .Append(LogUtil.GetLogParam("SalesID", Me.SalesID, True))
        End With
        Return sb.ToString
    End Function

End Class
