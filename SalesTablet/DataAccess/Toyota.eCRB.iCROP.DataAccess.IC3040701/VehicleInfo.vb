'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'VehicleInfo.vb
'─────────────────────────────────────
'機能： テンプレート取得インターフェイス 車両関連情報クラス
'補足： 
'作成： 2014/05/13 TMEJ 曽山
'更新： 
'─────────────────────────────────────

Imports System.Text
Imports Toyota.eCRB.SystemFrameworks.Core

''' <summary>
''' 車両関連情報クラス
''' </summary>
''' <remarks></remarks>
Public Class VehicleInfo

    ''' <summary>
    ''' シリーズ名
    ''' </summary>
    Public Property SeriesName As String = String.Empty

    ''' <summary>
    ''' メーカー名
    ''' </summary>
    Public Property MakerName As String = String.Empty

    ''' <summary>
    ''' サービス名
    ''' </summary>
    Public Property ServiceName As String = String.Empty

    ''' <summary>
    ''' 点検推奨日
    ''' </summary>
    Public Property CRDate As String = String.Empty

    ''' <summary>
    ''' このインスタンスの文字列表現を返却する。
    ''' </summary>
    ''' <returns>インスタンスの文字列表現を返却する。</returns>
    Public Overloads Function ToString() As String
        Dim sb As New StringBuilder
        With sb
            .Append(LogUtil.GetLogParam("SeriesName", Me.SeriesName, False))
            .Append(LogUtil.GetLogParam("MakerName", Me.MakerName, True))
            .Append(LogUtil.GetLogParam("ServiceName", Me.ServiceName, True))
            .Append(LogUtil.GetLogParam("CRDate", Me.CRDate, True))
        End With
        Return sb.ToString
    End Function
End Class
