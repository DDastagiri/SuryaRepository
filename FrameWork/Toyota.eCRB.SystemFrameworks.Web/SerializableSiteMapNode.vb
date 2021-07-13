'Copyright c2009 Toyota Motor Corporation. All rights reserved. For internal use only.
Imports System.Diagnostics.CodeAnalysis

Namespace Toyota.eCRB.SystemFrameworks.Web

    ''' <summary>
    ''' �V���A���C�Y�\��SiteMapNode
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable()> _
    Public Class SerializableSiteMapNode
        Private _key As String
        Private _url As String
        Private _title As String
        Private _pageSession As Dictionary(Of String, Object)

        ''' <summary>
        ''' �L�[
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
        Scope:="member", Justification:="���� URL�͕�����^�ł悢�B")> _
        Public ReadOnly Property Url() As String
            Get
                Return _url
            End Get
        End Property

        ''' <summary>
        ''' �^�C�g��
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
        ''' �e�y�[�W����Session�ێ����
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
        ''' �R���X�g���N�^
        ''' </summary>
        ''' <param name="key">�L�[</param>
        ''' <param name="url">URL</param>
        ''' <param name="title">�^�C�g��</param>
        ''' <param name="pageSession">�e�y�[�W����Session�ێ����</param>
        ''' <remarks></remarks>
        <SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", _
        Scope:="member", Justification:="���� URL�͕�����^�ł悢�B")> _
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