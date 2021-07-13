'Copyright c2009 Toyota Motor Corporation. All rights reserved. For internal use only.
Imports System
Imports System.Collections.Generic
Imports System.Configuration
Imports System.Globalization
Imports System.Text
Imports System.Web
Imports System.Web.UI
Imports System.Diagnostics.CodeAnalysis

Namespace Toyota.eCRB.SystemFrameworks.Web

    ''' <summary>
    ''' �����^�T�C�g�}�b�v�v���o�C�_�ł��B
    ''' </summary>
    ''' <remarks>
    ''' �J�ڂ����y�[�W�̏����Z�b�V�����ɕێ����܂��B
    ''' </remarks>
    Public Class HistorySiteMapProvider
        Inherits System.Web.SiteMapProvider

        ''' <summary>
        ''' ��ʑJ�ڗ������X�g��Session�L�[��
        ''' </summary>
        Public Const SESSION_KEY_PAGE_HISTORY_LIST As String = _
            "Toyota.eCRB.SystemFrameworks.Web.HistorySiteMapProvider.PageHistoryList"
        ''' <summary>
        ''' ����ʈ��p��Dictionary(Of String, Object)��Session�L�[��
        ''' </summary>
        Public Const SESSION_KEY_NEXT_PAGE_INFO As String = _
            "Toyota.eCRB.SystemFrameworks.Web.HistorySiteMapProvider.NextPageInfo"

        ''' <summary>
        ''' ���C�����j���[�pSession�ێ��f�[�^
        ''' Dictionary(Of String, Object)��Session�L�[��
        ''' </summary>
        ''' <remarks></remarks>
        Public Const SESSION_KEY_MAINMENU_CONTEXT As String = _
            "Toyota.eCRB.SystemFrameworks.AppService.BasePage.MainMenuContext"

        ''' <summary>
        ''' �w�肵�� URL �̃y�[�W��\�� SiteMapNode �I�u�W�F�N�g���擾���܂��B 
        ''' </summary>
        ''' <param name="rawUrl">SiteMapNode �̎擾�Ώۃy�[�W������ URL�B</param>
        ''' <returns>rawURL �Ŏ������y�[�W��\�� SiteMapNode�B</returns>
        ''' <remarks></remarks>
        Public Overloads Overrides Function FindSiteMapNode( _
            ByVal rawUrl As String) As SiteMapNode

            Dim nodeList As List(Of SerializableSiteMapNode) = _
                HistorySiteMapProvider.SiteMapNodeList

            If nodeList Is Nothing Then
                '�w�肵�� URL �̃y�[�W��\�� SiteMapNode �I�u�W�F�N�g���擾�����̃��[�v���J�n
                For i As Integer = nodeList.Count - 1 To 0 Step -1
                    If nodeList(i).Url.Equals(rawUrl) Then
                        Return ConvertSiteMapNode(nodeList(i), i)
                    End If
                Next
                '�w�肵�� URL �̃y�[�W��\�� SiteMapNode �I�u�W�F�N�g���擾�����̃��[�v���I��
            End If

            Return Nothing
        End Function

        ''' <summary>
        ''' ����� SiteMapNode �̎q�m�[�h���擾���܂��B
        ''' </summary>
        ''' <param name="node">���ׂĂ̎q�m�[�h���擾����Ώۂ� SiteMapNode�B</param>
        ''' <returns>�w�肵�� SiteMapNode �̒��ڂ̎q�m�[�h���i�[����Ă���ǂݎ���p�� SiteMapNodeCollection�B</returns>
        ''' <remarks></remarks>
        Public Overrides Function GetChildNodes( _
            ByVal node As System.Web.SiteMapNode) As System.Web.SiteMapNodeCollection

            Dim nodeList As List(Of SerializableSiteMapNode) = _
                HistorySiteMapProvider.SiteMapNodeList
            Dim col As SiteMapNodeCollection = New SiteMapNodeCollection()
            Dim current As Boolean = False

            '����� SiteMapNode �̎q�m�[�h���擾�����̃��[�v���J�n
            For i As Integer = 0 To nodeList.Count - 1
                If current Then
                    col.Add(ConvertSiteMapNode(nodeList(i), i))
                ElseIf nodeList(i).Key.Equals(node.Key) Then
                    current = True
                End If
            Next
            '����� SiteMapNode �̎q�m�[�h���擾�����̃��[�v���I��

            Return col
        End Function

        ''' <summary>
        ''' ����� SiteMapNode �I�u�W�F�N�g�̐e�m�[�h���擾���܂��B 
        ''' </summary>
        ''' <param name="node">�e�m�[�h���擾����Ώۂ� SiteMapNode�B</param>
        ''' <returns>node �̐e��\�� SiteMapNode�B</returns>
        ''' <remarks></remarks>
        Public Overrides Function GetParentNode( _
            ByVal node As System.Web.SiteMapNode) As System.Web.SiteMapNode

            Dim nodeList As List(Of SerializableSiteMapNode) = _
                HistorySiteMapProvider.SiteMapNodeList

            '����� SiteMapNode �I�u�W�F�N�g�̐e�m�[�h���擾�����̃��[�v���J�n
            For i As Integer = nodeList.Count - 1 To 1 Step -1
                If nodeList(i).Key.Equals(node.Key) Then
                    Return ConvertSiteMapNode(nodeList(i - 1), i - 1)
                End If
            Next
            '����� SiteMapNode �I�u�W�F�N�g�̐e�m�[�h���擾�����̃��[�v���I��

            Return Nothing
        End Function

        ''' <summary>
        ''' ���݂̃v���o�C�_�ɂ���Č��݊Ǘ�����Ă���S�m�[�h�̃��[�g �m�[�h���擾���܂��B 
        ''' </summary>
        ''' <returns>���̏����͎g���Ȃ��ׁANothing��Ԃ��܂��B</returns>
        ''' <remarks></remarks>
        Protected Overrides Function GetRootNodeCore() As System.Web.SiteMapNode
            Return Nothing
        End Function

        ''' <summary>
        ''' �T�C�g�}�b�v�m�[�h�̃��X�g���擾���܂��B
        ''' </summary>
        ''' <returns>�T�C�g�}�b�v�m�[�h�̃��X�g</returns>
        ''' <remarks>
        ''' �J�ڂ����y�[�W�̏����Z�b�V�����ɕێ����܂��B
        ''' </remarks>
        Public Shared ReadOnly Property SiteMapNodeList() As List(Of SerializableSiteMapNode)
            Get
                Dim nodeList As List(Of SerializableSiteMapNode) _
                    = DirectCast(HttpContext.Current.Session(SESSION_KEY_PAGE_HISTORY_LIST),  _
                        List(Of SerializableSiteMapNode))

                If nodeList Is Nothing Then
                    nodeList = New List(Of SerializableSiteMapNode)
                    HttpContext.Current.Session(SESSION_KEY_PAGE_HISTORY_LIST) = nodeList
                End If

                Return nodeList
            End Get
        End Property

        ''' <summary>
        ''' �����̃y�[�WSession���ɂāASerializableSiteMapNode�𐶐����A��ʑJ�ڗ������X�g�ɒǉ����܂��B
        ''' </summary>
        ''' <param name="url">��ʕ\���Ƀ��N�G�X�g���ꂽUrl</param>
        ''' <param name="title">�p�������\���Ɏg�p�����ʖ�</param>
        ''' <param name="pageSessionInfo">�y�[�WSession�����i�[����Dictionary(Of String, Object)</param>
        ''' <remarks></remarks>
        Public Shared Sub AddNewNode( _
            ByVal url As String, _
            ByVal title As String, _
            ByVal pageSessionInfo As Dictionary(Of String, Object))
            Dim nodeList As List(Of SerializableSiteMapNode) = _
                HistorySiteMapProvider.SiteMapNodeList
            nodeList.Add( _
                New SerializableSiteMapNode(Guid.NewGuid.ToString, url, title, pageSessionInfo))
        End Sub

        ''' <summary>
        ''' �T�C�g�}�b�v�m�[�h�̃��X�g��S�č폜���܂��B
        ''' </summary>
        ''' <remarks></remarks>
        ''' 
        <SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Scope:="member", _
        Justification:="����͓����ŃX�^�e�b�N�����o�̂݃A�N�Z�X�ł��邪�A_�Ӗ��I�ɂ̓X�^�e�B�b�N���\�b�h�ł͂Ȃ��̂ŏ��O����B")> _
        Public Sub Clear()
            HttpContext.Current.Session.Remove(SESSION_KEY_PAGE_HISTORY_LIST)
        End Sub

        ''' <summary>
        ''' SerializableSiteMapNode���AASP.NET�W����SiteMapNode�ɕϊ����܂��B
        ''' </summary>
        ''' <param name="node">�ϊ��Ώۂ�SerializableSiteMapNode</param>
        ''' <param name="index">�ϊ��Ώ�Node�̉�ʑJ�ڗ���List�̈ʒu</param>
        ''' <returns>�ϊ����ꂽSiteMapNode</returns>
        ''' <remarks></remarks>
        Private Function ConvertSiteMapNode( _
            ByVal node As SerializableSiteMapNode, _
            ByVal index As Integer) As SiteMapNode
            Dim nodeTitle As String

            nodeTitle = node.Title
            Dim siteMapNode As SiteMapNode = New SiteMapNode(Me, node.Key, node.Url, nodeTitle)
            siteMapNode.Description = index.ToString(CultureInfo.InvariantCulture)
            Return siteMapNode
        End Function


    End Class
End Namespace