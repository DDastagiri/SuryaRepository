'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'ReplacedTemplate.vb
'─────────────────────────────────────
'機能： テンプレート取得インターフェイス テンプレート情報クラス
'補足： 
'作成： 2014/05/13 TMEJ 曽山
'更新： 
'─────────────────────────────────────

Imports System.Text
Imports Toyota.eCRB.SystemFrameworks.Core

''' <summary>
''' テンプレート取得インターフェイス テンプレート情報クラス
''' </summary>
''' <remarks></remarks>
Public Class ReplacedTemplate

    ''' <summary>
    ''' テンプレート取得結果
    ''' </summary>
    Public Enum TemplateResult As Integer

        ''' <summary>
        ''' Success
        ''' </summary>
        Success = 0

        ''' <summary>
        ''' 該当テンプレートが存在しない
        ''' </summary>
        ''' <remarks></remarks>
        NotFound = 11
    End Enum

    ''' <summary>
    ''' テンプレートタイトル
    ''' </summary>
    Public Property Subject As String = String.Empty

    ''' <summary>
    ''' テンプレート本文
    ''' </summary>
    Public Property Text As String = String.Empty

    ''' <summary>
    ''' e-Mailアドレス
    ''' </summary>
    Public Property Email As String = String.Empty

    ''' <summary>
    ''' テンプレート取得結果
    ''' </summary>
    Public Property Result As TemplateResult = TemplateResult.NotFound

    ''' <summary>
    ''' テンプレート区分
    ''' </summary>
    Public Property TemplateClass As String = String.Empty

    ''' <summary>
    ''' テンプレートタイトル、本文に指定したカテゴリの置換文字列が存在するかを判定する。
    ''' </summary>
    ''' <param name="literals">置換文字列カテゴリ</param>
    ''' <returns>置換文字列が存在している場合はTrueを返却する。</returns>
    Public Function ContainsGroupLiteral(ByVal literals As List(Of String)) As Boolean
        Logger.Info(LogUtil.GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, True))
        Logger.Info(LogUtil.GetLogParam("literals", literals, False))

        Dim textHas As Boolean = _
            Not String.IsNullOrWhiteSpace(Me.Text) AndAlso literals.Any(Function(x) Me.Text.Contains(x))
        Dim subjectHas As Boolean = _
            Not String.IsNullOrWhiteSpace(Me.Subject) AndAlso literals.Any(Function(x) Me.Subject.Contains(x))
        Dim has As Boolean = textHas Or subjectHas

        Logger.Info(LogUtil.GetReturnParam(CStr(has)))
        Logger.Info(LogUtil.GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, False))
        Return has
    End Function

    ''' <summary>
    ''' このインスタンスの文字列表現を返却する。
    ''' </summary>
    ''' <returns>インスタンスの文字列表現を返却する。</returns>
    Public Overloads Function ToString() As String
        Dim sb As New StringBuilder
        With sb
            .Append(LogUtil.GetLogParam("Subject", Me.Subject, False))
            .Append(LogUtil.GetLogParam("Text", Me.Text, True))
            .Append(LogUtil.GetLogParam("Email", Me.Email, True))
            .Append(LogUtil.GetLogParam("Result", CStr(Me.Result), True))
            .Append(LogUtil.GetLogParam("TemplateClass", Me.TemplateClass, True))
        End With
        Return sb.ToString
    End Function
End Class