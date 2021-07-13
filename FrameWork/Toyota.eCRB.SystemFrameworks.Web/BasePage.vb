'Copyright c2009 Toyota Motor Corporation. All rights reserved. For internal use only.
Imports System.ComponentModel
Imports System.Globalization
Imports System.IO
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Web
Imports System.Web.HttpContext
Imports System.Web.UI
Imports System.Text
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Configuration

Namespace Toyota.eCRB.SystemFrameworks.Web

    ''' <summary>
    ''' �_�C�A���O�\���̃G�t�F�N�g��\���񋓌^
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum DialogEffect As Integer
        ''' <summary>
        ''' �����Ƀt�F�[�h�C��
        ''' </summary>
        FadeIn = 0
        ''' <summary>
        ''' ������̃X���C�h�C��
        ''' </summary>
        Left = 1
        ''' <summary>
        ''' �E����̃X���C�h�C��
        ''' </summary>
        Right = 2
        ''' <summary>
        ''' �ォ��̃X���C�h�C��
        ''' </summary>
        Top = 3
        ''' <summary>
        ''' ������̃X���C�h�C��
        ''' </summary>
        Bottom = 4
    End Enum

    ''' <summary>
    ''' ��ʑJ�ڗ����ł̈ʒu��\���񋓌^
    ''' Prev    :���ݕ\�����Ă����ʂ̂P�O�ɂ����ʈʒu
    ''' Current :���ݕ\�����Ă����ʈʒu
    ''' [Next]  :���ݕ\�����Ă����ʂ̎��ɕ\�����悤�Ƃ��Ă����ʈʒu
    ''' Last    :������ɂ��錻�݂̉�ʂƓ���ŁA���߂̉�ʈʒu
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum ScreenPos As Integer
        Prev = 0
        Current = 1
        [Next] = 2
        Last = 3
    End Enum

    ''' <summary>
    ''' ���v���[���e�[�V�����N���X�ł��B���ʂŎg�p����@�\��񋟂��܂��B
    ''' </summary>
    ''' <remarks>
    ''' �A�v���P�[�V�����ł� Web �y�[�W�N���X���쐬����Ƃ�
    ''' <see cref="System.Web.UI.Page"/> �ł͂Ȃ��A
    ''' ���̃N���X�����N���X�Ƃ��Ă��������B
    ''' </remarks>
    Public MustInherit Class BasePage
        Inherits System.Web.UI.Page

        ''' <summary>
        ''' ���ʊ�ՊǗ��p�g�b�v�y�[�WURL�̃Z�b�V�����L�[
        ''' </summary>
        ''' <remarks></remarks>
        Private Const SESSION_TOPPAGE As String = "Toyota.eCRB.SystemFrameworks.Web.BasePage.TopPage"

        Private ReadOnly Property CommonMaster As CommonMasterPage
            Get
                Dim m As MasterPage = Me.Master
                Do While (m IsNot Nothing)
                    If (TypeOf m Is CommonMasterPage) Then
                        Return CType(m, CommonMasterPage)
                    End If
                    m = m.Master
                Loop
                Return Nothing
            End Get
        End Property

        ''' <summary>
        ''' �\������t�b�^�[�{�^����錾���܂��B
        ''' </summary>
        ''' <param name="commonMaster"></param>
        ''' <param name="category">�y�[�W�������郁�j���[�J�e�S���i�h���N���X���ݒ肵�܂��j</param>
        ''' <returns>�t�b�^�[�{�^��ID�̔z��</returns>
        ''' <remarks>���̃��\�b�h�́A�h���N���X���I�[�o�[���C�h����K�v������܂��B</remarks>
        Public Overridable Function DeclareCommonMasterFooter(ByVal commonMaster As CommonMasterPage, ByRef category As FooterMenuCategory) As Integer()
            Return New Integer() {}
        End Function

        ''' <summary>
        ''' �\������R���e�L�X�g���j���[���ڂ�錾���܂��B
        ''' </summary>
        ''' <param name="commonMaster"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Function DeclareCommonMasterContextMenu(ByVal commonMaster As CommonMasterPage) As Integer()
            Return New Integer() {CommonMasterContextMenuBuiltinMenuID.StandByItem, CommonMasterContextMenuBuiltinMenuID.SuspendItem, CommonMasterContextMenuBuiltinMenuID.LogoutItem}
        End Function

        Protected Overrides Sub OnPreRender(ByVal e As System.EventArgs)
            MyBase.OnPreRender(e)

            'icropScript.ui.account ��ݒ�
            Dim account As String = ""
            Try
                account = StaffContext.Current.Account
            Catch ex As InvalidOperationException
                '�����O�C��
            End Try

            Dim script As String = String.Format(CultureInfo.InvariantCulture, "icropScript.ui.account = '{0}';", account)
            ClientScript.RegisterStartupScript(GetType(BasePage), "icropScript.ui.account", script, True)

        End Sub

#Region " �v���p�e�B "
        ''' <summary>
        ''' �y�[�W�v���p�e�B�̕ԓ��l���㏑�����A�{�N���X�̃C���X�^���X���g���v���p�e�B�l�Ƃ��܂��B
        ''' </summary>
        ''' <value></value>
        ''' <returns>�{�N���X�̃C���X�^���X���g���v���p�e�B�l�Ƃ��Ė߂��܂��B</returns>
        ''' <remarks></remarks>
        Public Shadows ReadOnly Property Page() As BasePage
            Get
                Return Me
            End Get
        End Property

        ''' <summary>
        ''' ����ʂ����샍�b�N�����ǂ�����Ԃ��܂��B����ʂ�ICustomerForm�C���^�t�F�[�X���������Ă��Ȃ��ꍇ�́A���False�ł��B
        ''' </summary>
        ''' <returns>True�F���b�N���@False�F���b�N���łȂ�</returns>
        ''' <remarks></remarks>
        Protected ReadOnly Property OperationLocked() As Boolean
            Get
                If TypeOf Me Is ICustomerForm Then
                    'ICustomerForm���������Ă�����
                    Dim master As CommonMasterPage = Me.CommonMaster
                    If (master IsNot Nothing) Then
                        '��ʃ��b�N�̃`�F�b�N��Ԃ�ԋp
                        Return master.OperationLocked.Value.Equals("1")
                    Else
                        'CommonMasterPage�ȊO
                        Return False
                    End If
                Else
                    'ICustomerForm���������Ă��Ȃ����
                    Return False
                End If
            End Get
        End Property
#End Region

#Region "Session����"
        Private _nextPageInfo As Dictionary(Of String, Object)
        Private _prevPageInfo As Dictionary(Of String, Object)
        Private _lastPageInfo As Dictionary(Of String, Object)
        Private _currentPageInfo As Dictionary(Of String, Object)

        ''' <summary>
        ''' Session�֌����������̃f�[�^���i�[���܂��B
        ''' </summary>
        ''' <param name="pos">��ʈʒu��\���񋓌^�B</param>
        ''' <param name="key">Session�Ɋi�[����Object�̃L�[���B</param>
        ''' <param name="value">Session�Ɋi�[����Oblect</param>
        ''' <remarks></remarks>
        ''' <exception cref="ArgumentNullException">
        ''' �����ukey�v��Nothing���w�肵���ꍇ�ɃX���[����܂��B
        ''' </exception>
        ''' <exception cref=" InvalidOperationException">
        ''' �J�ڌ������݂��Ȃ��ꍇ��screenPos�ɁuPrev�v�uLast�v���w�肵���ꍇ�ɃX���[����܂��B
        ''' </exception>
        Protected Sub SetValue( _
            ByVal pos As ScreenPos, _
            ByVal key As String, _
            ByVal value As Object)

            '����key��Nothing�̏ꍇ
            If key Is Nothing Then
                '��O�Ƃ���ArgumentNullException���X���[����
                Throw New ArgumentNullException("key")
            End If

            Dim pageInfo As Dictionary(Of String, Object)

            '����screenPos����
            Select Case pos
                Case ScreenPos.Next    'screenPos��Next(����ʁj�̏ꍇ
                    pageInfo = Me.NextPageInfo

                Case ScreenPos.Current 'screenPos��Current(����ʁj�̏ꍇ
                    pageInfo = Me.CurrentPageInfo

                Case ScreenPos.Prev    'screenPos��Prev�i�O��ʁj�̏ꍇ
                    pageInfo = Me.PrevPageInfo

                Case Else   'screenPos��Last�i���߂̓����ʁj�̏ꍇ
                    pageInfo = Me.LastPageInfo

                    '���ߓ����ʈ��n��Dictionary(Of String, Object)��Nothing�̏ꍇ
                    If pageInfo Is Nothing Then
                        Throw New InvalidOperationException
                    End If

            End Select

            pageInfo.Item(key) = value

        End Sub

        ''' <summary>
        ''' Session���猟���������̃f�[�^���擾���܂��B
        ''' </summary>
        ''' <param name="pos">��ʈʒu��\���񋓌^�B</param>
        ''' <param name="key">Session�Ɋi�[����Ă���Object�̃L�[���B</param>
        ''' <param name="removeFlg">Session�Ɋi�[����Ă���f�[�^�����o������A
        ''' �폜�������ꍇ��True�A����ȊO��False���w��B</param>
        ''' <returns>Session�Ɋi�[����Ă���Object</returns>
        ''' <remarks></remarks>
        ''' <exception cref="ArgumentNullException">
        ''' �����ukey�v��Nothing���w�肵���ꍇ�ɃX���[����܂��B
        ''' </exception>
        ''' <exception cref=" InvalidOperationException">
        ''' �J�ڌ������݂��Ȃ��ꍇ��screenPos�ɁuPrev�v���w�肵���ꍇ�ɃX���[����܂��B
        ''' </exception>
        Protected Function GetValue( _
            ByVal pos As ScreenPos, _
            ByVal key As String, _
            ByVal removeFlg As Boolean) As Object

            '����key��Nothing�̏ꍇ
            If key Is Nothing Then
                '��O�Ƃ���ArgumentNullException���X���[����
                Throw New ArgumentNullException("key")
            End If

            Dim pageInfo As Dictionary(Of String, Object)

            '����screenPos����
            Select Case pos
                Case ScreenPos.Current 'screenPos��Current(����ʁj�̏ꍇ
                    pageInfo = Me.CurrentPageInfo

                Case ScreenPos.Prev    'screenPos��Prev�i�O��ʁj�̏ꍇ
                    pageInfo = Me.PrevPageInfo

                Case ScreenPos.Last    'screenPos��Last�i���߂̓����ʁj�̏ꍇ
                    pageInfo = Me.LastPageInfo

                    '���ߓ����ʈ��n��Dictionary(Of String, Object)��Nothing�̏ꍇ
                    If pageInfo Is Nothing Then
                        Return Nothing
                    End If

                Case Else   'screenPos��Next(����ʁj�̏ꍇ
                    pageInfo = Me.NextPageInfo

            End Select

            '�Ԃ�l�i�[�p�I�u�W�F�N�g�ɃZ�b�V�����Ɋi�[����Ă���Object���i�[
            Dim returnObject As Object = pageInfo.Item(key)

            'removeFlg��True�̏ꍇ
            If removeFlg Then
                '������key���L�[�Ƃ��āA���[�J���ϐ�pageInfo��remove���\�b�h�ɂ�
                '�Ώۂ�Object���폜����
                pageInfo.Remove(key)

                ''���C�����j���[Session�ޔ�Cookie����
                'RemoveMainMenuCookie(screenPos, key)
            End If

            Return returnObject
        End Function

        ''' <summary>
        ''' Session���猟���������̃f�[�^���폜���܂��B
        ''' </summary>
        ''' <param name="pos">��ʈʒu��\���񋓌^�B</param>
        ''' <param name="key">Session����폜����Object�̃L�[���B</param>
        ''' <remarks></remarks>
        ''' <exception cref="ArgumentNullException">
        ''' �����ukey�v��Nothing���w�肵���ꍇ�ɃX���[����܂��B
        ''' </exception>
        ''' <exception cref=" InvalidOperationException">
        ''' �J�ڌ������݂��Ȃ��ꍇ��screenPos�ɁuPrev�v�uLast�v���w�肵���ꍇ�ɃX���[����܂��B
        ''' </exception>
        Protected Sub RemoveValue( _
            ByVal pos As ScreenPos, _
            ByVal key As String)

            '����key��Nothing�̏ꍇ
            If key Is Nothing Then
                '��O�Ƃ���ArgumentNullException���X���[����
                Throw New ArgumentNullException("key")
            End If

            Dim pageInfo As Dictionary(Of String, Object)

            '����screenPos����
            Select Case pos
                Case ScreenPos.Current 'screenPos��Current(����ʁj�̏ꍇ
                    pageInfo = Me.CurrentPageInfo

                Case ScreenPos.Prev    'screenPos��Prev�i�O��ʁj�̏ꍇ
                    pageInfo = Me.PrevPageInfo

                Case ScreenPos.Last    'screenPos��Last�i���߂̓����ʁj�̏ꍇ
                    pageInfo = Me.LastPageInfo

                    '���ߓ����ʈ��n��Dictionary(Of String, Object)��Nothing�̏ꍇ
                    If pageInfo Is Nothing Then
                        Throw New InvalidOperationException
                    End If

                Case Else   'screenPos��Next(����ʁj�̏ꍇ
                    pageInfo = Me.NextPageInfo

            End Select

            '������key���L�[�Ƃ��āA���[�J���ϐ�pageInfo��remove���\�b�h�ɂđΏۂ�Object���폜����
            pageInfo.Remove(key)

            ''���C�����j���[Session�ޔ�Cookie����
            'RemoveMainMenuCookie(screenPos, key)

        End Sub

        ''' <summary>
        ''' �w�肵���L�[����Object��Session�ɑ��݂��邩�m�F���܂��B
        ''' </summary>
        ''' <param name="pos">���݊m�F�����ʈʒu��\���񋓌^�B</param>
        ''' <param name="key">Session�ɑ��݂��邩�m�F����Object�̃L�[���B</param>
        ''' <returns>True�F�w�肵���f�[�^��Session�ɑ��݂���BFalse�F���݂��Ȃ��B</returns>
        ''' <remarks></remarks>
        ''' <exception cref="ArgumentNullException">
        ''' �����ukey�v��Nothing���w�肵���ꍇ�ɃX���[����܂��B
        ''' </exception>
        ''' <exception cref=" InvalidOperationException">
        ''' �J�ڌ������݂��Ȃ��ꍇ��screenPos�ɁuPrev�v�uLast�v���w�肵���ꍇ�ɃX���[����܂��B
        ''' </exception>
        Protected Function ContainsKey( _
            ByVal pos As ScreenPos, _
            ByVal key As String) As Boolean

            '����key��Nothing�̏ꍇ
            If key Is Nothing Then
                '��O�Ƃ���ArgumentNullException���X���[����
                Throw New ArgumentNullException("key")
            End If

            Dim pageInfo As Dictionary(Of String, Object)

            '����screenPos����
            Select Case pos
                Case ScreenPos.Current 'screenPos��Current(����ʁj�̏ꍇ
                    pageInfo = Me.CurrentPageInfo

                Case ScreenPos.Prev    'screenPos��Prev�i�O��ʁj�̏ꍇ
                    pageInfo = Me.PrevPageInfo

                Case ScreenPos.Last    'screenPos��Last�i���߂̓����ʁj�̏ꍇ
                    pageInfo = Me.LastPageInfo

                    '���ߓ����ʈ��n��Dictionary(Of String, Object)��Nothing�̏ꍇ
                    If pageInfo Is Nothing Then
                        Return False
                    End If

                Case Else    'screenPos��Next(����ʁj�̏ꍇ
                    pageInfo = Me.NextPageInfo

            End Select

            '���[�J���ϐ�pageInfo��ContainsKey���\�b�h������key���w�肵�Ď��s���A
            '���ʂ�߂�l�Ƃ��Ė߂��B
            Return pageInfo.ContainsKey(key)

        End Function

        ''' <summary>
        ''' ��ʑJ�ڗ�����ɂāA�J�����g��ʂ���O�ʒu�̉�ʂ�ID���擾���܂��B
        ''' </summary>
        ''' <returns>�O�ʒu�̉��ID</returns>
        ''' <remarks></remarks>
        ''' <exception cref=" InvalidOperationException">
        ''' �J�ڌ��̉�ʂ����݂��Ȃ��ꍇ�ɃX���[����܂��B
        ''' </exception>
        Protected ReadOnly Property GetPrevScreenId() As String
            Get
                Dim nodeList As List(Of SerializableSiteMapNode)

                'HistorySiteMapProvider��SiteMapNodeList�v���p�e�B�ɂ�Session����
                '��ʑJ�ڗ���List���擾
                nodeList = HistorySiteMapProvider.SiteMapNodeList

                '��ʑJ�ڗ���List�̃T�C�Y���P�ȉ��̏ꍇ
                If nodeList.Count <= 1 Then


                    '��ʑJ�ڗ���List�̃T�C�Y���P���A���C�����j���[Session�����݂���ꍇ
                    If nodeList.Count = 1 Then
                        '���C�����j���[�̃A�v���h�c��Ԃ�
                        Return CStr(Session(SESSION_TOPPAGE))
                    Else
                        'Nothing��Ԃ�
                        Return Nothing
                    End If
                    'End If

                End If

                Dim node As SerializableSiteMapNode

                '��ʑJ�ڗ���List�̍Ō������P�O�̈ʒu�̗������擾
                node = nodeList(nodeList.Count - 2)

                '�擾����������URL�v���p�e�B���擾
                Dim prevUrl As String = node.Url

                '���N�G�������񂪊܂܂��ꍇ��
                If 0 < prevUrl.IndexOf("?", StringComparison.OrdinalIgnoreCase) Then
                    '�N�G����������폜
                    prevUrl = prevUrl.Remove(prevUrl.IndexOf("?", StringComparison.OrdinalIgnoreCase))
                End If

                '���ID�̎擾
                Dim screenId As String = prevUrl.Substring(prevUrl.LastIndexOf("/", StringComparison.OrdinalIgnoreCase) + 1)
                screenId = screenId.Remove(screenId.LastIndexOf(".", StringComparison.OrdinalIgnoreCase))

                Return screenId
            End Get
        End Property



        ''' <summary>
        ''' �����ő�ێ�������эő�T�C�Y�𒴂����ꍇ�ɁA�T�C�g�}�b�v�����T�C�Y���܂��B
        ''' </summary>
        ''' <remarks></remarks>
        Private Shared Sub resizeSiteMapNoneList()

            Dim siteMapNodeList As List(Of SerializableSiteMapNode) = HistorySiteMapProvider.SiteMapNodeList

            '����\������������擾
            Dim maxHistoryCount As Integer = EnvironmentSetting.MaxHistoryCount

            '��ʑJ�ڗ���List�̌����Ə���l���r
            If maxHistoryCount < siteMapNodeList.Count Then

                '���ߕ����폜
                siteMapNodeList.RemoveRange(0, siteMapNodeList.Count - maxHistoryCount)
            End If

            '����Session�T�C�Y������擾
            Dim maxHistorySize As Integer = EnvironmentSetting.MaxHistorySize * 1024

            '��ʑJ�ڗ���List�̃V���A���C�Y�����T�C�Y���擾
            Dim nodeListSize As Long = CalculateSize(siteMapNodeList)

            '��ʑJ�ڗ���List�̃V���A���C�Y�����T�C�Y�Ə���l���r
            If maxHistorySize < nodeListSize Then

                'Node�T�C�Y���v�p�̃��[�J���ϐ�nodeSize��錾����
                Dim nodeSize As Long = 0

                '�폜�����p�̃��[�J���ϐ�delCount��錾����
                Dim delCount As Integer

                '��ʑJ�ڗ���List�̐擪����Ō���̗v�f�܂Ń��[�v�������J�n
                For Each serializableSiteMapNode As SerializableSiteMapNode In siteMapNodeList

                    '��ʑJ�ڗ����̃V���A���C�Y�����T�C�Y�����Z
                    nodeSize += CalculateSize(serializableSiteMapNode)

                    '�폜����delCount���C���N�������g
                    delCount += 1

                    If (nodeListSize - nodeSize) < maxHistorySize Then
                        '���[�v�𔲂���
                        Exit For
                    End If

                Next

                '��ʑJ�ڗ���List�̐擪����Ō���̗v�f�܂Ń��[�v�������I��
                '��ʑJ�ڗ���List�̐擪����AdelCount�̌������̗������폜
                siteMapNodeList.RemoveRange(0, delCount)

            End If

        End Sub



        ''' <summary>
        ''' Session�ɂĎ���ʂɈ��n�������i�[����Dictionary(Of String, Object)���擾���܂��B
        ''' </summary>
        ''' <returns>
        ''' Session�ɂĎ���ʂɈ��n�������i�[����Dictionary(Of String, Object)
        ''' </returns>
        Friend ReadOnly Property NextPageInfo() As Dictionary(Of String, Object)
            Get
                If Me._nextPageInfo Is Nothing Then
                    '�C���X�^���X�ϐ�_nextPageInfo��Nothing�̏ꍇ�A�ȉ��̏��������s

                    '����ʈ��n��Dictionary(Of String, Object)
                    Dim nextPage As Dictionary(Of String, Object) = Nothing

                    'Session��莟��ʈ��n��Dictionary(Of String, Object)���擾
                    nextPage = DirectCast( _
                        Current.Session(HistorySiteMapProvider.SESSION_KEY_NEXT_PAGE_INFO),  _
                        Dictionary(Of String, Object))

                    If nextPage Is Nothing Then

                        '����ʈ��n��Dictionary(Of String, Object)�𐶐�
                        nextPage = New Dictionary(Of String, Object)

                        '��������Dictionary(Of String, Object)��HistorySiteMapProvider.SESSION_KEY_NEXT_PAGE_INFO��
                        '�L�[�Ƃ���Session�Ɋi�[
                        Current.Session(HistorySiteMapProvider.SESSION_KEY_NEXT_PAGE_INFO) = nextPage
                    End If

                    'Dictionary(Of String, Object)���C���X�^���X�ϐ�_nextPageInfo�ɐݒ�
                    Me._nextPageInfo = nextPage
                End If

                '�߂�l�Ƃ��āA�C���X�^���X�ϐ�_nextPageInfo��߂��B
                Return Me._nextPageInfo
            End Get
        End Property

        ''' <summary>
        ''' Session�ɂđO��ʂɈ��n�������i�[����Dictionary(Of String, Object)���擾���܂��B
        ''' </summary>
        ''' <returns>
        ''' Session�ɂđO��ʂɈ��n�������i�[����Dictionary(Of String, Object)
        ''' </returns>
        ''' <exception cref="InvalidOperationException">
        ''' �J�ڌ��̉�ʂ����݂��Ȃ��ꍇ�ɃX���[����܂��B
        ''' </exception>
        Private ReadOnly Property PrevPageInfo() As Dictionary(Of String, Object)
            Get
                If Me._prevPageInfo Is Nothing Then
                    '�C���X�^���X�ϐ�_prevPageInfo��Nothing�̏ꍇ�A�ȉ��̏��������s

                    Dim nodeList As List(Of SerializableSiteMapNode)

                    '�O��ʈ��n��HasTable
                    Dim prevPage As Dictionary(Of String, Object) = Nothing

                    'HistorySiteMapProvider��SiteMapNodeList�v���p�e�B�ɂ�
                    'Session�����ʑJ�ڗ���List���擾
                    nodeList = HistorySiteMapProvider.SiteMapNodeList

                    '��ʑJ�ڗ���List�̃T�C�Y���P�ȉ��̏ꍇ
                    If nodeList.Count <= 1 Then
                        Throw New InvalidOperationException
                    End If

                    Dim node As SerializableSiteMapNode

                    '��ʑJ�ڗ���List�̍Ō������P�O�̈ʒu�̗������擾
                    node = nodeList(nodeList.Count - 2)

                    '������PageSessionInfo�v���p�e�B�ɂđO��ʈ��n��Dictionary(Of String, Object)���擾
                    prevPage = node.PageSessionInfo

                    'Dictionary(Of String, Object)���C���X�^���X�ϐ�_prevPageInfo�ɐݒ�
                    Me._prevPageInfo = prevPage
                End If

                '�߂�l�Ƃ��āA�C���X�^���X�ϐ�_prevPageInfo��߂��B
                Return Me._prevPageInfo
            End Get
        End Property

        ''' <summary>
        ''' Session�ɂĒ��߂̓����ʂɈ��n�������i�[����Dictionary(Of String, Object)���擾���܂��B
        ''' ��ʑJ�ڗ����ɊY�����闚�������݂��Ȃ��ꍇ�A����ёO�ʒu�ɉ�ʑJ�ڗ������Ȃ��ꍇ��
        ''' ��O���X���[�����ANothing��߂��܂��B
        ''' </summary>
        ''' <returns>
        ''' Session�ɂĒ��߂̓����ʂɈ��n�������i�[����Dictionary(Of String, Object)
        ''' </returns>
        Private ReadOnly Property LastPageInfo() As Dictionary(Of String, Object)
            Get
                If Me._lastPageInfo Is Nothing Then
                    '�C���X�^���X�ϐ�_lastPageInfo��Nothing�̏ꍇ�A�ȉ��̏��������s
                    Dim nodeList As List(Of SerializableSiteMapNode)

                    'HistorySiteMapProvider��SiteMapNodeList�v���p�e�B�ɂ�Session����
                    '��ʑJ�ڗ���List���擾
                    nodeList = HistorySiteMapProvider.SiteMapNodeList

                    '��ʑJ�ڗ���List�̃T�C�Y���P�ȉ��̏ꍇ
                    If nodeList.Count <= 1 Then
                        Me._lastPageInfo = Nothing
                    Else
                        '���n��HasTable
                        Dim lastPage As Dictionary(Of String, Object) = Nothing

                        For i As Integer = nodeList.Count - 2 To 0 Step -1
                            If nodeList(i).Url.Equals(Current.Request.RawUrl) Then
                                lastPage = nodeList(i).PageSessionInfo
                                Exit For
                            End If
                        Next i

                        'Dictionary(Of String, Object)���C���X�^���X�ϐ�_lastPageInfo�ɐݒ�
                        Me._lastPageInfo = lastPage
                    End If

                End If

                '�߂�l�Ƃ��āA�C���X�^���X�ϐ�_lastPageInfo��߂��B
                Return Me._lastPageInfo
            End Get
        End Property

        ''' <summary>
        ''' ����ʂ�Session�����i�[����Dictionary(Of String, Object)���擾���܂��B
        ''' </summary>
        ''' <returns>
        ''' ����ʂ�Session�����i�[����Dictionary(Of String, Object)
        ''' </returns>
        Private ReadOnly Property CurrentPageInfo() As Dictionary(Of String, Object)
            Get
                If Me._currentPageInfo Is Nothing Then
                    '�C���X�^���X�ϐ�_currentPageInfo��Nothing�̏ꍇ�A�ȉ��̏��������s

                    '����ʈ��n��HasTable
                    Dim currentPage As Dictionary(Of String, Object) = Nothing

                    Dim nodeList As List(Of SerializableSiteMapNode)

                    'HistorySiteMapProvider��SiteMapNodeList�v���p�e�B�ɂ�Session����
                    '��ʑJ�ڗ���List���擾
                    nodeList = HistorySiteMapProvider.SiteMapNodeList

                    '��ʑJ�ڗ���List���O���̏ꍇ�͑J�ڗ������쐬
                    '���C�����j���[�̏ꍇ�͓Ǝ���Key�ŊǗ�
                    If (nodeList.Count = 0) Then
                        If Session(HistorySiteMapProvider.SESSION_KEY_MAINMENU_CONTEXT) Is Nothing Then
                            currentPage = New Dictionary(Of String, Object)
                            Session(HistorySiteMapProvider.SESSION_KEY_MAINMENU_CONTEXT) = currentPage
                        Else
                            currentPage = CType(Session(HistorySiteMapProvider.SESSION_KEY_MAINMENU_CONTEXT), Dictionary(Of String, Object))
                        End If
                    Else
                        '��ʑJ�ڗ���List�̍Ō���̈ʒu���痚�����擾
                        Dim node As SerializableSiteMapNode = nodeList(nodeList.Count - 1)

                        '������PageSessionInfo�v���p�e�B�ɂĉ�ʈ��n��Dictionary(Of String, Object)���擾
                        currentPage = node.PageSessionInfo
                    End If
                    ''��ʑJ�ڗ���List���O���̏ꍇ�͑J�ڗ������쐬
                    'If (nodeList.Count = 0) Then
                    '    HistorySiteMapProvider.AddNewNode(Context.Request.Url.ToString(), "", New Dictionary(Of String, Object))
                    'End If

                    ''��ʑJ�ڗ���List�̍Ō���̈ʒu���痚�����擾
                    'Dim node As SerializableSiteMapNode = nodeList(nodeList.Count - 1)

                    ''������PageSessionInfo�v���p�e�B�ɂĉ�ʈ��n��Dictionary(Of String, Object)���擾
                    'currentPage = node.PageSessionInfo

                    'Dictionary(Of String, Object)���C���X�^���X�ϐ�_currentPageInfo�ɐݒ�
                    Me._currentPageInfo = currentPage
                End If

                '�߂�l�Ƃ��āA�C���X�^���X�ϐ�_currentPageInfo��߂��B
                Return Me._currentPageInfo
            End Get
        End Property

        ''' <summary>
        ''' �w�肵��Object�̃V���A���C�Y�����T�C�Y���o�C�g�P�ʂŕԂ��܂��B
        ''' </summary>
        ''' <param name="obj">�T�C�Y���߂�Object</param>
        ''' <remarks>
        ''' </remarks>
        Friend Shared Function CalculateSize(ByVal obj As Object) As Long

            Dim returnSize As Long = 0

            Using stream As MemoryStream = New MemoryStream()
                Dim writer As BinaryWriter = New BinaryWriter(stream)
                Dim formatter As New BinaryFormatter
                formatter.Serialize(writer.BaseStream, obj)

                writer.Flush()
                returnSize = stream.Length
            End Using

            Return returnSize
        End Function
#End Region

#Region " ��ʑJ�ڋy�щ�ʑ��� "

        ''' <summary>
        ''' �h���C�����i�[�p�Z�b�V������
        ''' </summary>
        ''' <remarks></remarks>
        Private Const SESSION_DOMAIN As String = "Toyota.eCRB.SystemFrameworks.Web."

        ''' <summary>
        ''' �o���f�[�V�������ʂ�ʒm���邽�߂̃|�b�v�A�b�v�_�C�A���O��\�����������Ɏg�p���܂�
        ''' </summary>
        ''' <param name="wordNo">�\�����b�Z�[�W�i����No�j</param>
        ''' <param name="wordParam">�\�����b�Z�[�W�i�u��������j</param>
        ''' <remarks></remarks>
        Protected Sub ShowMessageBox(ByVal wordNo As Integer, ByVal ParamArray wordParam As String())
            Dim word As String = WebWordUtility.GetWord(wordNo)
            If wordParam IsNot Nothing AndAlso wordParam.Length > 0 Then
                word = String.Format(CultureInfo.InvariantCulture, word, wordParam)
            End If
            JavaScriptUtility.RegisterAlertMessege(Me, "", "", word)
        End Sub

        ''' <summary>
        ''' �o���f�[�V�������ʂ�ʒm���邽�߂̃|�b�v�A�b�v�_�C�A���O��\�����������Ɏg�p���܂�
        ''' </summary>
        ''' <param name="code">�G���[�R�[�h</param>
        ''' <param name="detail">��Q��͗p������</param>
        ''' <param name="wordNo">�\�����b�Z�[�W�i����No�j</param>
        ''' <param name="wordParam">�\�����b�Z�[�W�i�u��������j</param>
        ''' <remarks></remarks>
        Protected Sub ShowMessageBox(ByVal code As String, ByVal detail As String, ByVal wordNo As Integer, ByVal ParamArray wordParam As String())
            Dim word As String = WebWordUtility.GetWord(wordNo)
            If wordParam IsNot Nothing AndAlso wordParam.Length > 0 Then
                word = String.Format(CultureInfo.InvariantCulture, word, wordParam)
            End If
            JavaScriptUtility.RegisterAlertMessege(Me, code, detail, word)
        End Sub

        ''' <summary>
        ''' ����ʂɑJ�ڂ��܂��B
        ''' </summary>
        ''' <param name="appId">���ID</param>
        ''' <remarks></remarks>
        Public Sub RedirectNextScreen(ByVal appId As String)

            '��ʑJ�ڗ���List���擾
            'Dim siteMapNodeList As List(Of SerializableSiteMapNode) = HistorySiteMapProvider.SiteMapNodeList
            Dim aspxFileName As String = appId & ".aspx"

            '����ʈ��n��Dictionary(Of String, Object)���擾
            Dim nextPageInfo As Dictionary(Of String, Object) = Me.NextPageInfo

            '����ʈ��n��Dictionary(Of String, Object)�����݂���ꍇ
            If Session(HistorySiteMapProvider.SESSION_KEY_NEXT_PAGE_INFO) IsNot Nothing Then
                'Session��莟��ʈ��n��Dictionary(Of String, Object)���폜
                Session.Remove(HistorySiteMapProvider.SESSION_KEY_NEXT_PAGE_INFO)
            End If

            '��ʑJ�ڗ�����ǉ�
            Dim canonicalUrl As String = Me.ResolveUrl("~/Pages/" & aspxFileName)
            'Dim hisMapProvider As New HistorySiteMapProvider
            HistorySiteMapProvider.AddNewNode(canonicalUrl, "", nextPageInfo)

            '�T�C�g�}�b�v�̃��T�C�Y
            resizeSiteMapNoneList()

            '�J�ڐ��ʂ̃h���C�����擾
            Dim config As ClassSection = SystemConfiguration.Current.Manager.DocumentDomain
            If config IsNot Nothing Then
                Dim domain As String = Nothing
                If config IsNot Nothing Then
                    Dim setting As Setting = config.GetSetting(String.Empty)
                    If (setting IsNot Nothing) Then
                        domain = DirectCast(setting.GetValue(appId), String)
                        Session(SESSION_DOMAIN) = domain
                    End If
                End If
            End If

            '��ʑJ��
            Logger.Debug(String.Format(CultureInfo.InvariantCulture, "BasePage.RedirectNextScreen: {0}", canonicalUrl))
            Me.Response.Redirect(canonicalUrl)
        End Sub

        ''' <summary>
        ''' �s�v��Session�i����ʁA����ʁj���폜���A��ʑJ�ڗ�������w���ʐ��O�̉�ʂɃ��_�C���N�g���܂��B
        ''' </summary>
        ''' <param name="prev">�߂��ʐ�</param>
        ''' <remarks></remarks>
        ''' <exception cref=" InvalidOperationException">
        ''' �J�ڌ��̉�ʂ����݂��Ȃ��ꍇ�A�������}�C�i�X�̏ꍇ�ɃX���[����܂��B
        ''' </exception>
        Public Sub RedirectPrevScreen(ByVal prev As Integer)

            If prev < 1 Then
                '��O�Ƃ���InvalidOperationException���X���[����
                Throw New InvalidOperationException
            End If

            Dim nodeList As List(Of SerializableSiteMapNode)

            'HistorySiteMapProvider��SiteMapNodeList�v���p�e�B�ɂ�Session�����ʑJ�ڗ���List���擾
            nodeList = HistorySiteMapProvider.SiteMapNodeList

            Dim prevUrl As String = Nothing
            If nodeList.Count <= 1 Then
                ''������1�̎��߂��̂̓g�b�v�y�[�W�̂�
                prevUrl = ResolveUrl("~/Pages/" & CStr(Session(SESSION_TOPPAGE)) & ".aspx")
            Else
                Dim node As SerializableSiteMapNode
                '��ʑJ�ڗ���List�̍Ō������P�O�̈ʒu�̗������擾
                node = nodeList(nodeList.Count - (1 + prev))

                '�擾����������URL�v���p�e�B���擾
                prevUrl = node.Url
            End If

            '��ʑJ�ڗ���List�̍Ō���̗������폜
            If (0 < nodeList.Count) Then
                nodeList.RemoveRange(nodeList.Count - prev, prev)
            End If

            '�萔SESSION_KEY_NEXT_PAGE_INFO���L�[�Ƃ��āASession��莟��ʈ��n��Dictionary(Of String, Object)���폜
            Current.Session.Remove(HistorySiteMapProvider.SESSION_KEY_NEXT_PAGE_INFO)

            '�J�ڐ��ʂ̃h���C�����擾
            Dim AppIdAry As String()
            AppIdAry = Split(prevUrl, "/")
            Dim AppId As String
            AppId = AppIdAry(AppIdAry.Length - 1)
            AppId = Replace(AppId, ".aspx", "")
            Dim config As Configuration.ClassSection = SystemConfiguration.Current.Manager.DocumentDomain
            If config IsNot Nothing Then
                Dim domain As String = Nothing
                If config IsNot Nothing Then
                    Dim setting As Configuration.Setting = config.GetSetting(String.Empty)
                    If (setting IsNot Nothing) Then
                        domain = DirectCast(setting.GetValue(AppId), String)
                        Session(SESSION_DOMAIN) = domain
                    End If
                End If
            End If

            '�擾����������URL�փ��_�C���N�g
            Logger.Debug(String.Format(CultureInfo.InvariantCulture, "BasePage.RedirectPrevScreen: {0}", prevUrl))
            Response.Redirect(prevUrl)

        End Sub

        ''' <summary>
        ''' �s�v��Session�i����ʁA����ʁj���폜���A��ʑJ�ڗ����̑O�ʒu�̉�ʂɃ��_�C���N�g���܂��B
        ''' </summary>
        ''' <remarks></remarks>
        ''' <exception cref=" InvalidOperationException">
        ''' �J�ڌ��̉�ʂ����݂��Ȃ��ꍇ�ɃX���[����܂��B
        ''' </exception>
        Public Sub RedirectPrevScreen()

            RedirectPrevScreen(1)

        End Sub
#End Region

#Region "�_�C�A���O����"
        ''' <summary>
        ''' �����uappId�v�Ŏw�肳�ꂽ��ʂ��_�C�A���O�\�����܂��B
        ''' </summary>
        ''' <param name="appId">�q�_�C�A���O��\���@�\ID</param>
        ''' <param name="effect">�_�C�A���O�̃G�t�F�N�g</param>
        ''' <remarks></remarks>
        Public Sub OpenDialog(ByVal appId As String, ByVal effect As DialogEffect)
            Dim paramEffect As String = "fadeIn"
            Select Case effect
                Case DialogEffect.Left
                    paramEffect = "left"
                Case DialogEffect.Right
                    paramEffect = "right"
                Case DialogEffect.Top
                    paramEffect = "top"
                Case DialogEffect.Bottom
                    paramEffect = "bottom"
            End Select

            Dim sb As New StringBuilder
            sb.Append("<script type='text/javascript'>").Append(vbCrLf)
            sb.Append("    (function(window) {").Append(vbCrLf)
            sb.Append("         icropScript.ui.openDialog('").Append(HttpUtility.JavaScriptStringEncode(appId & ".aspx"))
            sb.Append("', '").Append(paramEffect).Append("', ")
            sb.Append("function() { ").Append(Me.ClientScript.GetPostBackEventReference(Me, "")).Append("; });")
            sb.Append("    })(window);").Append(vbCrLf)
            sb.Append("</script>" & vbCrLf)
            JavaScriptUtility.RegisterStartupScript(Me, sb.ToString, "icropScript.ui.openDialog")

        End Sub

        ''' <summary>
        ''' �_�C�A���O����܂��B
        ''' ���̃��\�b�h�̓_�C�A���O�ŕ\������Ă���q��ʂł̂݋@�\���܂��B
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub CloseDialog()
            'Dim sb As New StringBuilder
            JavaScriptUtility.RegisterStartupFunctionCallScript(Me, "icropScript.ui.closeDialog", "icropScript.ui.closeDialog")
        End Sub
#End Region

#Region " �ҏW��ԕۑ� "

        ''' <summary>
        ''' ��ʂ̓��͒��f�[�^���擾���܂��B
        ''' </summary>
        ''' <returns>����ʂ��AISafeInputForm.SaveFormState���\�b�h�ɂđޔ��������͒��f�[�^</returns>
        ''' <remarks></remarks>
        Public Function GetFormState() As Dictionary(Of String, ISerializable)

            Dim state As Dictionary(Of String, Dictionary(Of String, ISerializable)) = GetSessionFormState()
            Dim appId As String = GetCurrentAppID()

            If Not state.ContainsKey(appId) Then
                '�Ȃ���Βǉ�
                state(appId) = New Dictionary(Of String, ISerializable)
            End If

            Return state(appId)
        End Function

        ''' <summary>
        ''' ����ʂ̓��͒��f�[�^��ێ����Ă���Session���N���A���܂��B
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub ClearFormState()
            Dim state As Dictionary(Of String, Dictionary(Of String, ISerializable)) = GetSessionFormState()
            Dim appId As String = GetCurrentAppID()
            If state.ContainsKey(appId) Then
                state.Remove(appId)
            End If
        End Sub

        ''' <summary>
        ''' ��ʓ��͏��ێ���Session�L�[��
        ''' </summary>
        Private Const SESSION_KEY_FORM_STATE As String = "Toyota.eCRB.SystemFrameworks.Web.BasePage.FormState"

        ''' <summary>
        ''' �Z�b�V���������͏����擾���܂��B
        ''' </summary>
        ''' <returns>���ID���ŁA���͍��ږ��̃L�[�Ƃ��̒l��Dictionary</returns>
        ''' <remarks></remarks>
        Private Function GetSessionFormState() As Dictionary(Of String, Dictionary(Of String, ISerializable))

            Dim state As Dictionary(Of String, Dictionary(Of String, ISerializable)) = Nothing

            '�Z�b�V��������ʓ��͏��擾
            state = DirectCast(Session(SESSION_KEY_FORM_STATE), Dictionary(Of String, Dictionary(Of String, ISerializable)))
            If state Is Nothing Then
                '�V�K�쐬
                state = New Dictionary(Of String, Dictionary(Of String, ISerializable))
                '�Z�b�V�����̈�ɕۑ�
                Current.Session(SESSION_KEY_FORM_STATE) = state
            End If

            '�ԋp
            Return state
        End Function

        ''' <summary>
        ''' ���݃��N�G�X�g����Ă����ʂ�ID���擾���܂��B
        ''' </summary>
        ''' <returns>���ID</returns>
        ''' <remarks></remarks>
        Private Shared Function GetCurrentAppID() As String
            Dim path As String = Current.Request.AppRelativeCurrentExecutionFilePath
            path = path.Substring(path.LastIndexOf("/", StringComparison.OrdinalIgnoreCase) + 1)
            path = path.Remove(path.LastIndexOf(".", StringComparison.OrdinalIgnoreCase))
            Return path.ToUpper(CultureInfo.InvariantCulture)
        End Function

#End Region

#Region "��������"
        ''' <summary>
        ''' ��������
        ''' </summary>
        ''' <param name="searchString"></param>
        ''' <param name="searchType"></param>
        ''' <remarks></remarks>
        Friend Sub CustomerSearch_Click(ByVal searchString As String, ByVal searchType As Integer)
            '���͂��ꂽ�����l��ێ�
            SetValue(ScreenPos.Next, "searchString", searchString)
            SetValue(ScreenPos.Next, "searchType", searchType)

            '���O�C�����[�U���Z�[���X���T�[�r�X�����f
            Dim type As String = String.Empty
            Try
                Dim staff As StaffContext = StaffContext.Current
                Dim config As Configuration.ClassSection = SystemConfiguration.Current.Manager.StaffDivision
                If config IsNot Nothing Then
                    Dim setting As Configuration.Setting = config.GetSetting(String.Empty)
                    If (setting IsNot Nothing) Then
                        type = DirectCast(setting.GetValue(CStr(staff.OpeCD)), String)
                    End If
                End If
            Catch ex As InvalidOperationException
                '�����O�C��
            End Try

            '�Ώۉ�ʂ֑J��
            If String.IsNullOrEmpty(type) Then type = String.Empty
            If (type.Equals("Service")) Then
                RedirectNextScreen("SC3080102")
            Else
                RedirectNextScreen("SC3080101")
            End If

        End Sub
        ''' <summary>
        ''' ��������
        ''' </summary>
        ''' <param name="searchString"></param>
        ''' <param name="searchType"></param>
        ''' <remarks></remarks>
        Friend Sub CustomerSearch_Click(ByVal searchString As String, ByVal searchType As Integer, ByVal chipType As Integer)
            '2013/06/03 TMEJ ���V �yA.STEP2�zi-CROP�Ɩ��@�\�v����`�i�������p�@�\�j START
            'Friend Sub CustomerSearch_Click(ByVal searchString As String, ByVal searchType As Integer, ByVal chipType As Integer)
            '2013/06/03 TMEJ ���V �yA.STEP2�zi-CROP�Ɩ��@�\�v����`�i�������p�@�\�j END
            '���͂��ꂽ�����l��ێ�
            SetValue(ScreenPos.Next, "searchString", searchString)
            SetValue(ScreenPos.Next, "searchType", searchType)

            '���O�C�����[�U���Z�[���X���T�[�r�X�����f
            Dim type As String = String.Empty
            Try
                Dim staff As StaffContext = StaffContext.Current
                Dim config As Configuration.ClassSection = SystemConfiguration.Current.Manager.StaffDivision
                If config IsNot Nothing Then
                    Dim setting As Configuration.Setting = config.GetSetting(String.Empty)
                    If (setting IsNot Nothing) Then
                        type = DirectCast(setting.GetValue(CStr(staff.OpeCD)), String)
                    End If
                End If
            Catch ex As InvalidOperationException
                '�����O�C��
            End Try

            '�Ώۉ�ʂ֑J��
            If String.IsNullOrEmpty(type) Then type = String.Empty
            If (type.Equals("Service")) Then
                '2013/06/03 TMEJ ���V �yA.STEP2�zi-CROP�Ɩ��@�\�v����`�i�������p�@�\�j START
                'RedirectNextScreen("SC3080102")
                If chipType = 1 Then
                    RedirectNextScreen("SC3080103")
                Else
                    RedirectNextScreen("SC3240401")
                End If
                '2013/06/03 TMEJ ���V �yA.STEP2�zi-CROP�Ɩ��@�\�v����`�i�������p�@�\�j END
            Else
                RedirectNextScreen("SC3080101")
            End If

        End Sub
#End Region

        '2013/12/16 TMEJ ���V ������T�[�r�X �H���Ǘ��@�\�J�� START
#Region "��ʑJ�ڏ���"

        ''' <summary>
        ''' ��ʑJ�ڏ���
        ''' </summary>
        ''' <param name="inProgramId">�J�ڐ���ID</param>
        ''' <param name="inSessionKey">Session�L�[</param>
        ''' <param name="inSessionData">Session�f�[�^</param>
        ''' <remarks></remarks>
        Friend Sub RedirectNextScreenButton_Click(ByVal inProgramId As String, _
                                                  ByVal inSessionKey As String, _
                                                  ByVal inSessionData As String)
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} START" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))

            '��ʊԈ����̊m�F
            If (Not (String.IsNullOrEmpty(inSessionKey)) AndAlso 0 < inSessionKey.Length) Then

                '���������݂���ꍇ
                '�f�[�^���J���}��؂�Ŕz��ɂ���
                Dim sessionKeyList As String() = inSessionKey.Split(CType(",", Char))
                Dim sessionDataList As String() = inSessionData.Split(CType(",", Char))

                '��ʑJ�ڐ���`�F�b�N
                If inProgramId.Equals("SC3010501") Then
                    '���V�X�e���A�g��ʂ̏ꍇ
                    '�\���ԍ���Session�Ƀf�[�^�i�[
                    Me.SetValue(ScreenPos.Next, "Session.DISP_NUM", sessionDataList(0))

                    '���V�X�e����ʕ\���ɕK�v�ȃf�[�^���i�[
                    For i As Integer = 1 To sessionKeyList.Count - 1
                        Me.SetValue(ScreenPos.Next, String.Concat("Session.Param", i), sessionDataList(i))
                    Next

                Else
                    'i-CROP��ʂ̏ꍇ
                    'Session�Ƀf�[�^�i�[
                    For i As Integer = 0 To sessionKeyList.Count - 1
                        Me.SetValue(ScreenPos.Next, sessionKeyList(i), sessionDataList(i))
                    Next
                End If

            End If

            '��ʑJ�ڏ���
            RedirectNextScreen(inProgramId)

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))
        End Sub
#End Region
        '2013/12/16 TMEJ ���V ������T�[�r�X �H���Ǘ��@�\�J�� END

#Region "TCV�߂菈��"
        Friend Sub TCVCallBack(ByVal params As Dictionary(Of String, Object))

            For Each param In params
                SetValue(ScreenPos.Next, param.Key, param.Value)
            Next param

            RedirectNextScreen(CStr(params("StartPageId")))

        End Sub
#End Region

    End Class
End Namespace
