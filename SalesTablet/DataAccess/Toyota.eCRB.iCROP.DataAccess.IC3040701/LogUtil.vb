'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'LogUtil.vb
'─────────────────────────────────────
'機能： テンプレート取得インターフェイス ログ出力共通関数
'補足： 
'作成： 2014/05/13 TMEJ 曽山
'更新： 
'─────────────────────────────────────

Imports System.Text
Imports Toyota.eCRB.SystemFrameworks.Core

''' <summary>
''' テンプレート取得インターフェイス ログ出力共通関数
''' </summary>
''' <remarks></remarks>
Public NotInheritable Class LogUtil

    ''' <summary>
    ''' コンストラクタ
    ''' </summary>
    ''' <remarks>インスタンス生成抑制のため</remarks>
    Private Sub New()
    End Sub

    ''' <summary>
    ''' ログデータ（メソッド）
    ''' </summary>
    ''' <param name="methodName">メソッド名</param>
    ''' <param name="isStart">True：「method start」を表示、False：「method end」を表示</param>
    ''' <returns>加工した文字列</returns>
    ''' <remarks></remarks>
    Public Shared Function GetLogMethod(ByVal methodName As String,
                                ByVal isStart As Boolean) As String
        Dim sb As New StringBuilder
        With sb
            .Append("[")
            .Append(If(methodName Is Nothing, "Null", methodName))
            .Append("]")
            If isStart Then
                .Append(" method start")
            Else
                .Append(" method end")
            End If
        End With
        Return sb.ToString
    End Function

    ''' <summary>
    ''' ログデータ（引数）
    ''' </summary>
    ''' <param name="paramName">引数名</param>
    ''' <param name="paramData">引数値</param>
    ''' <param name="useKanma">True：引数名の前に「,」を表示、False：特になし</param>
    ''' <returns>加工した文字列</returns>
    ''' <remarks></remarks>
    Public Shared Function GetLogParam(ByVal paramName As String,
                                 ByVal paramData As IEnumerable(Of String),
                                 ByVal useKanma As Boolean) As String
        Dim result As String = GetLogParam(paramName, ToFlatString(paramData), useKanma)
        Return result
    End Function

    ''' <summary>
    ''' ログデータ（引数）
    ''' </summary>
    ''' <param name="paramName">引数名</param>
    ''' <param name="paramData">引数値</param>
    ''' <param name="useKanma">True：引数名の前に「,」を表示、False：特になし</param>
    ''' <returns>加工した文字列</returns>
    ''' <remarks></remarks>
    Public Shared Function GetLogParam(ByVal paramName As String,
                                 ByVal paramData As String,
                                 ByVal useKanma As Boolean) As String
        Dim sb As New StringBuilder
        With sb
            If useKanma Then
                .Append(",")
            End If
            .Append(If(paramName Is Nothing, "Null", paramName))
            .Append("=")
            .Append(If(paramData Is Nothing, "Null", paramData))
        End With
        Return sb.ToString
    End Function

    ''' <summary>
    ''' ログデータ（戻り値）
    ''' </summary>
    ''' <param name="paramData">引数値</param>
    ''' <returns>加工した文字列</returns>
    ''' <remarks></remarks>
    Public Shared Function GetReturnParam(ByVal paramData As String) As String
        Dim sb As New StringBuilder
        With sb
            .Append("Return=")
            .Append(If(paramData Is Nothing, "Null", paramData))
        End With
        Return sb.ToString
    End Function

    ''' <summary>
    ''' 配列の中身をカンマ区切りで列挙した文字列に変換する。
    ''' </summary>
    ''' <param name="strs">配列</param>
    ''' <returns>配列の中身をカンマ区切りで列挙した文字列を返却する。</returns>
    Private Shared Function ToFlatString(ByVal strs As IEnumerable(Of String)) As String
        Dim sb As New StringBuilder
        With sb
            For Each s As String In strs
                sb.Append(s)
                sb.Append(",")
            Next

            If 1 <= sb.Length Then
                sb.Remove(sb.Length - 1, 1)
            End If
        End With
        Return sb.ToString
    End Function

End Class
