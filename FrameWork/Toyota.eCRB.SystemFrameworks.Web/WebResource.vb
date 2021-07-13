Imports System.Web
Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Globalization
Imports System.IO

Namespace Toyota.eCRB.SystemFrameworks.Web

    ''' <summary>
    ''' HTMLリソースファイル（.js .cssなど）用のユーティリティメソッドを提供します。
    ''' </summary>
    Public NotInheritable Class WebResource

        Private Sub New()

        End Sub

        ''' <summary>
        ''' 最適化されたHTMLリソースファイルURLを返します。
        ''' </summary>
        ''' <param name="path">HTMLリソースファイルのURL</param>
        ''' <returns>最適化されたHTMLリソースファイルURL</returns>
        ''' <remarks>
        ''' サーバー上のHTMLリソースファイルと、クライアントブラウザのキャッシュファイルの内容
        ''' に不一致が生じないよう、URLの末尾にサフィックス(更新日時）を追加します。
        ''' </remarks>
        Public Shared Function GetUrl(ByVal path As String) As String
            Try
                Dim physicalPath As String = HttpContext.Current.Server.MapPath(path)
                Dim lastModified As Date = File.GetLastWriteTime(physicalPath)
                Return path & "?d=" & lastModified.ToString("yyyyMMddHHmm", CultureInfo.InvariantCulture)

            Catch ex As Exception
                Logger.Warn("WebResource.GetUrl - couldn't get physical path from " & path & "  (" & ex.Message & ")")
            End Try

            Return path
        End Function
    End Class
End Namespace

