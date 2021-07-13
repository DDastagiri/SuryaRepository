'Copyright c2009 Toyota Motor Corporation. All rights reserved. For internal use only.
Imports System.Diagnostics.CodeAnalysis

Namespace Toyota.eCRB.SystemFrameworks.Web

    ''' <summary>
    ''' シリアライズ可能なSiteMapNode
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable()> _
    Public Class SerializableSiteMapNode
        Private _key As String
        Private _url As String
        Private _title As String
        Private _pageSession As Dictionary(Of String, Object)

        ''' <summary>
        ''' キー
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Key() As String
            Get
                Return _key
            End Get
        End Property

        ''' <summary>
        ''' URL
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", _
        Scope:="member", Justification:="この URLは文字列型でよい。")> _
        Public ReadOnly Property Url() As String
            Get
                Return _url
            End Get
        End Property

        ''' <summary>
        ''' タイトル
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Title() As String
            Get
                Return _title
            End Get
        End Property

        ''' <summary>
        ''' 各ページ毎のSession保持情報
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property PageSessionInfo() As Dictionary(Of String, Object)
            Get
                Return _pageSession
            End Get
        End Property

        ''' <summary>
        ''' コンストラクタ
        ''' </summary>
        ''' <param name="key">キー</param>
        ''' <param name="url">URL</param>
        ''' <param name="title">タイトル</param>
        ''' <param name="pageSession">各ページ毎のSession保持情報</param>
        ''' <remarks></remarks>
        <SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", _
        Scope:="member", Justification:="この URLは文字列型でよい。")> _
        Public Sub New( _
            ByVal key As String, _
            ByVal url As String, _
            ByVal title As String, _
            ByVal pageSession As Dictionary(Of String, Object))

            _key = key
            _url = url
            _title = title
            _pageSession = pageSession
        End Sub
    End Class
End Namespace