'Copyright c2009 Toyota Motor Corporation. All rights reserved. For internal use only.
Namespace Toyota.eCRB.SystemFrameworks.Configuration

    ''' <summary>
    ''' Toyota.eCRB.SystemFrameworks�J�X�^���\���Z�N�V�����̓��e���擾���邽�߂̃N���X�ł��B
    ''' ���̃N���X�̓A�v���P�[�V�����R�[�h�Ŏg�p���邽�߂̂��̂ł͂���܂���B
    ''' </summary>
    ''' <remarks>
    ''' ���̃N���X�𗘗p�����\���v�f�ւ̃A�N�Z�X�͍����ł͂���܂���B���������āA���̃N���X�𗘗p���ē���v�f�ւ̌J��Ԃ�
    ''' �̃A�N�Z�X�͍s��Ȃ��ł��������B���̂悤�ȏꍇ�́A�ŏ��̃A�N�Z�X�̌��ʂ� Shared �����o�ɑҔ�����Ȃǂ̍H�v���s����
    ''' ���������B
    ''' </remarks>
    Public NotInheritable Class ConfigurationManager

        Private _config As System.Xml.XmlElement = Nothing

        ''' <summary>
        ''' �C���X�^���X�̐������ł��Ȃ��悤�ɂ��邽�߂̃f�t�H���g�̃R���X�g���N�^�ł��B
        ''' </summary>
        Public Sub New(ByVal config As System.Xml.XmlElement)
            Me._config = config
        End Sub

        ''' <summary>
        ''' �Z�b�V�����}�l�[�W���[�ݒ�����擾���܂��B
        ''' </summary>
        ''' <value>���ݒ�擾�N���X�p�ݒ���</value>
        Public ReadOnly Property ScreenUrl() As Toyota.eCRB.SystemFrameworks.Configuration.ClassSection
            Get
                Return Me.GetClassSection("ScreenUrl")
            End Get
        End Property

        ''' <summary>
        ''' �Z�b�V�����}�l�[�W���[�ݒ�����擾���܂��B
        ''' </summary>
        ''' <value>���ݒ�擾�N���X�p�ݒ���</value>
        Public ReadOnly Property TopPageUrl() As Toyota.eCRB.SystemFrameworks.Configuration.ClassSection
            Get
                Return Me.GetClassSection("TopPageUrl")
            End Get
        End Property

        ''' <summary>
        ''' �Z�b�V�����}�l�[�W���[�ݒ�����擾���܂��B
        ''' </summary>
        ''' <value>���ݒ�擾�N���X�p�ݒ���</value>
        Public ReadOnly Property StaffDivision() As Toyota.eCRB.SystemFrameworks.Configuration.ClassSection
            Get
                Return Me.GetClassSection("StaffDivision")
            End Get
        End Property

        ''' <summary>
        ''' �Z�b�V�����}�l�[�W���[�ݒ�����擾���܂��B
        ''' </summary>
        ''' <value>���ݒ�擾�N���X�p�ݒ���</value>
        Public ReadOnly Property Individual() As Toyota.eCRB.SystemFrameworks.Configuration.ClassSection
            Get
                Return Me.GetClassSection("Individual")
            End Get
        End Property

        ''' <summary>
        ''' �Z�b�V�����}�l�[�W���[�ݒ�����擾���܂��B
        ''' </summary>
        ''' <value>���ݒ�擾�N���X�p�ݒ���</value>
        Public ReadOnly Property SessionManager() As Toyota.eCRB.SystemFrameworks.Configuration.ClassSection
            Get
                Return Me.GetClassSection("SessionManager")
            End Get
        End Property

        ''' <summary>
        ''' ���ݒ�擾�N���X�p�ݒ�����擾���܂��B
        ''' </summary>
        ''' <value>���ݒ�擾�N���X�p�ݒ���</value>
        Public ReadOnly Property EnvironmentSetting() As Toyota.eCRB.SystemFrameworks.Configuration.ClassSection
            Get
                Return Me.GetClassSection("EnvironmentSetting")
            End Get
        End Property

        ''' <summary>
        ''' ���O�}�l�[�W���ݒ�����擾���܂��B
        ''' </summary>
        ''' <value>���ݒ�擾�N���X�p�ݒ���</value>
        Public ReadOnly Property LogManager() As Toyota.eCRB.SystemFrameworks.Configuration.ClassSection
            Get
                Return Me.GetClassSection("LogManager")
            End Get
        End Property

        ''' <summary>
        ''' ���O�C���}�l�[�W���ݒ�����擾���܂��B
        ''' </summary>
        ''' <value>���ݒ�擾�N���X�p�ݒ���</value>
        Public ReadOnly Property LoginManager() As Toyota.eCRB.SystemFrameworks.Configuration.ClassSection
            Get
                Return Me.GetClassSection("LoginManager")
            End Get
        End Property

        ''' <summary>
        ''' �Z�b�V�����}�l�[�W���[�ݒ�����擾���܂��B
        ''' </summary>
        ''' <value>���ݒ�擾�N���X�p�ݒ���</value>
        Public ReadOnly Property ConnectionManager() As Toyota.eCRB.SystemFrameworks.Configuration.ClassSection
            Get
                Return Me.GetClassSection("ConnectionManager")
            End Get
        End Property

        ''' <summary>
        ''' �Í����N���X�p�ݒ�����擾���܂��B
        ''' </summary>
        ''' <value>�Í����N���X�p�ݒ���</value>
        Public ReadOnly Property Encryption() As Toyota.eCRB.SystemFrameworks.Configuration.ClassSection
            Get
                Return Me.GetClassSection("Encryption")
            End Get
        End Property

        ''' <summary>
        ''' �L���b�V���N���X�p�ݒ�����擾���܂��B
        ''' </summary>
        ''' <value>�L���b�V���N���X�p�ݒ���</value>
        Public ReadOnly Property CachingManager() As Toyota.eCRB.SystemFrameworks.Configuration.ClassSection
            Get
                Return Me.GetClassSection("CachingManager")
            End Get
        End Property

        ''' <summary>
        ''' �o���f�[�V�����N���X�p�ݒ�����擾���܂��B
        ''' </summary>
        ''' <value>�o���f�[�V�����N���X�p�ݒ���</value>
        Public ReadOnly Property Validation() As Toyota.eCRB.SystemFrameworks.Configuration.ClassSection
            Get
                Return Me.GetClassSection("Validation")
            End Get
        End Property

        ''' <summary>
        ''' �Z�b�V�����}�l�[�W���[�ݒ�����擾���܂��B
        ''' </summary>
        ''' <value>���ݒ�擾�N���X�p�ݒ���</value>
        Public ReadOnly Property BatchCommonSetting() As Toyota.eCRB.SystemFrameworks.Configuration.ClassSection
            Get
                Return Me.GetClassSection("BatchCommonSetting")
            End Get
        End Property

        ''' <summary>
        ''' �R�[�h�e�[�u���T�[�r�X�N���X�p�ݒ�����擾���܂��B
        ''' </summary>
        ''' <value>�R�[�h�e�[�u���T�[�r�X�N���X�p�ݒ���</value>
        Public ReadOnly Property CodeTableSerivce() As Toyota.eCRB.SystemFrameworks.Configuration.ClassSection
            Get
                Return Me.GetClassSection("CodeTableSerivce")
            End Get
        End Property

        ''' <summary>
        ''' �y�[�X�y�[�W�p�̐ݒ�����擾���܂��B
        ''' </summary>
        ''' <value>�x�[�X�y�[�W�p�ݒ���</value>
        Public ReadOnly Property BasePage() As Toyota.eCRB.SystemFrameworks.Configuration.ClassSection
            Get
                Return Me.GetClassSection("BasePage")
            End Get
        End Property

        ''' <summary>
        ''' i-CROP��Version�����擾���܂��B
        ''' </summary>
        ''' <value>i-CROP��Version���</value>
        Public ReadOnly Property VersionInformation() As Toyota.eCRB.SystemFrameworks.Configuration.ClassSection
            Get
                Return Me.GetClassSection("VersionInformation")
            End Get
        End Property

        ''' <summary>
        ''' �}�X�^�[�y�[�W�̏����擾���܂��B
        ''' </summary>
        ''' <value>�}�X�^�[�y�[�W�̏��</value>
        Public ReadOnly Property CommonMasterPage() As Toyota.eCRB.SystemFrameworks.Configuration.ClassSection
            Get
                Return Me.GetClassSection("CommonMasterPage")
            End Get
        End Property

        ''' <summary>
        ''' �A�b�v���[�h�T�C�Y����̏����擾���܂��B
        ''' </summary>
        ''' <value>�A�b�v���[�h�T�C�Y����̏��</value>
        Public ReadOnly Property CustomFileUpload() As Toyota.eCRB.SystemFrameworks.Configuration.ClassSection
            Get
                Return Me.GetClassSection("CustomFileUpload")
            End Get
        End Property

        ''' <summary>
        ''' CSV�t�@�C������̏����擾���܂��B
        ''' </summary>
        ''' <value>�A�b�v���[�h�T�C�Y����̏��</value>
        Public ReadOnly Property CsvManager() As Toyota.eCRB.SystemFrameworks.Configuration.ClassSection
            Get
                Return Me.GetClassSection("CsvManager")
            End Get
        End Property

        ''' <summary>
        ''' ���C�����j���[URL�̏����擾���܂��B
        ''' </summary>
        ''' <value>���C�����j���[URL</value>
        Public ReadOnly Property MainMenu() As Toyota.eCRB.SystemFrameworks.Configuration.ClassSection
            Get
                Return Me.GetClassSection("MainMenu")
            End Get
        End Property

        ''' <summary>
        ''' ����̏����擾���܂��B
        ''' </summary>
        ''' <value></value>
        Public ReadOnly Property ConcurrentProcMgr() As Toyota.eCRB.SystemFrameworks.Configuration.ClassSection
            Get
                Return Me.GetClassSection("ConcurrentProcMgr")
            End Get
        End Property

        ''' <summary>
        ''' �Ɩ��N����ʂ̏����擾���܂��B
        ''' </summary>
        ''' <value></value>
        Public ReadOnly Property S90B0003() As Toyota.eCRB.SystemFrameworks.Configuration.ClassSection
            Get
                Return Me.GetClassSection("S90B0003")
            End Get
        End Property

        ''' <summary>
        ''' �A�v����ՌŗL�̃V�X�e���ݒ�����擾���܂��B
        ''' </summary>
        ''' <value></value>
        Public ReadOnly Property System() As Toyota.eCRB.SystemFrameworks.Configuration.ClassSection
            Get
                Return Me.GetClassSection("System")
            End Get
        End Property

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <value>���ݒ�擾�N���X�p�ݒ���</value>
        Public ReadOnly Property DocumentDomain() As Toyota.eCRB.SystemFrameworks.Configuration.ClassSection
            Get
                Return Me.GetClassSection("DocumentDomain")
            End Get
        End Property

        ''' <summary>
        ''' Class�v�f�̓ǂݍ��݂��s���܂��B
        ''' </summary>
        ''' <param name="className">�擾����Class�v�f��Name����</param>
        ''' <returns>Class�v�f�̂�\������ClassSection�N���X�̃C���X�^���X</returns>
        Public Function GetClassSection(ByVal className As String) As Toyota.eCRB.SystemFrameworks.Configuration.ClassSection

            ''---------- Class�Z�N�V�����̐錾 ----------------------- 
            Dim returnValue As Toyota.eCRB.SystemFrameworks.Configuration.ClassSection = Nothing

            ''----- �N���X�Z�N�V�����̎擾
            'Dim section As System.Xml.XmlElement = DirectCast(System.Web.Configuration.WebConfigurationManager.GetSection(APP_NAMESPACE), System.Xml.XmlElement)

            For Each item As System.Xml.XmlNode In Me._config

                Dim element As System.Xml.XmlElement = TryCast(item, System.Xml.XmlElement)

                If element IsNot Nothing Then
                    If (element.Attributes("Name").Value.Equals(className)) Then
                        returnValue = New Toyota.eCRB.SystemFrameworks.Configuration.ClassSection(item)
                        Exit For
                    End If
                End If
            Next

            '---------- �Ԃ�l�̐ݒ� --------------------------------
            Return returnValue

        End Function

    End Class


    ''' <summary>
    ''' e-CRB Framework�p�̃J�X�^���\���Z�N�V�������������邽�߂̃N���X�ł��B
    ''' </summary>
    ''' <remarks>
    ''' <para>
    ''' ���̃N���X�́Ae-CRB Framework�Ǝ��̍\���v�f���������邽�߂̃N���X�ł��B
    ''' �\���t�@�C���Ɏ��̌`���ō\���Z�N�V������o�^���邱�Ƃɂ��Ae-CRB Framework�̍\�����s�����Ƃ��\�ƂȂ�܂��B
    ''' </para>
    ''' <example>
    ''' <code>
    '''   &lt;configSections&gt;
    '''     &lt;section name="Toyota.eCRB.SystemFrameworks" type="Toyota.eCRB.SystemFrameworks.Configuration.ConfigurationHandler, Toyota.eCRB.SystemFrameworks.Configuration, version=1.1.0.0, Culture=neutral, PublicKeyToken=18229613dc9cad02"/&gt;
    '''   &lt;/configSections&gt;
    ''' </code>
    ''' </example>
    ''' </remarks>
    Friend Class ConfigurationHandler

        Implements System.Configuration.IConfigurationSectionHandler

        ''' <summary>
        ''' �\���Z�N�V���� �n���h�����쐬���܂��B
        ''' </summary>
        ''' <param name="parent"></param>
        ''' <param name="configContext">�\���R���e�L�X�g �I�u�W�F�N�g</param>
        ''' <param name="section"></param>
        ''' <returns>�쐬���ꂽ�Z�N�V���� �n���h�� �I�u�W�F�N�g�B</returns>
        ''' <remarks>parent ����� section �ɂ��ẮAMSDN�Ȃǂɂ����m�ȋL�ڂ��������ߐ������������Ă��܂��B</remarks>
        Friend Function Create(ByVal parent As Object, ByVal configContext As Object, ByVal section As System.Xml.XmlNode) As Object Implements System.Configuration.IConfigurationSectionHandler.Create

            Return section

        End Function
    End Class

End Namespace
