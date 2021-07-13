'Copyright c2009 Toyota Motor Corporation. All rights reserved. For internal use only.
Imports System.Runtime.Remoting.Contexts
Imports System.Runtime.Remoting.Messaging
Imports Toyota.eCRB.SystemFrameworks.Core


Namespace Toyota.eCRB.SystemFrameworks.Core

    ''' <summary>
    ''' ContextBoundObject�𗘗p�����A�X�y�N�g�w���ɂ��R�~�b�g�Ǘ��@�\��
    ''' �������邽�߂̃R���e�L�X�g�v���p�e�B�ł��B
    ''' </summary>
    ''' <remarks>�I�u�W�F�N�g���A�N�e�B�x�[�g�����ƁA�X�̃R���e�L�X�g
    ''' �����ɂ��āAGetPropertiesForNewContext���\�b�h���Ăяo����܂��B
    ''' ����ɂ��A�I�u�W�F�N�g�̂��߂ɍ쐬�������V�R���e�L�X�g�Ɍ���
    ''' �t����ꂽ�v���p�e�B���X�g�ɁA�Ǝ��̃R���e�L�X�g�v���p�e�B��
    ''' �ǉ����邱�Ƃ��ł��܂��B�R���e�L�X�g�v���p�e�B�́A
    ''' ���b�Z�[�W�V���N�`�F�[�����̃I�u�W�F�N�g�Ƀ��b�Z�[�W�V���N����
    ''' �ѕt������悤�ɂ��܂��B�R���e�L�X�g�v���p�e�B�N���X�́A
    ''' IContextProperty��IContributeObjectSink���������A�A�X�y�N�g���b
    ''' �Z�[�W�V���N�̃t�@�N�g���Ƃ��ċ@�\���܂��B</remarks>
    Friend Class TransactionAspectProperty
        Implements IContextProperty, IContributeObjectSink

        ''' <summary>
        ''' �R���X�g���N�^�ł��B
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()

        End Sub

        ''' <summary>
        ''' �R���e�L�X�g���Œ肳���Ƃ��ɌĂяo����܂��B
        ''' </summary>
        ''' <param name="newContext">�Œ肷��R���e�L�X�g</param>
        ''' <remarks>�R���e�L�X�g���Œ肳�ꂽ��ŃR���e�L�X�g��
        ''' �v���p�e�B��ǉ����邱�Ƃ͂ł��܂���B</remarks>
        Public Sub Freeze( _
                ByVal newContext As Context) _
                Implements IContextProperty.Freeze

            ' ���������Ȃ�

        End Sub

        ''' <summary>
        ''' �R���e�L�X�g �v���p�e�B�ƐV�����R���e�L�X�g�Ƃ̊Ԃ�
        ''' �݊��������邩�ǂ����������u�[���l��Ԃ��܂��B
        ''' </summary>
        ''' <param name="newCtx">ContextProperty���쐬���ꂽ�V�����R���e�L�X�g�B</param>
        ''' <returns>���True�B</returns>
        ''' <remarks>���̃��\�b�h�̓R���e�L�X�g �v���p�e�B�������
        ''' �R���e�L�X�g���̑��̃R���e�L�X�g �v���p�e�B�Ƌ����ł���
        ''' �ꍇ�� True�A����ȊO�̏ꍇ�� False��Ԃ��悤�Ɏ�������K�v��
        ''' ����܂��B���̎����ł́A��� True ��Ԃ��悤�Ɏ��{���Ă��܂��B
        ''' </remarks>
        Public Function IsNewContextOK( _
                ByVal newCtx As Context) As Boolean _
                Implements IContextProperty.IsNewContextOK

            Return True

        End Function

        ''' <summary>
        ''' �R���e�L�X�g�ɒǉ������Ƃ��̃v���p�e�B�̖��O���擾���܂��B
        ''' �^�̖��O��Ԃ��悤�Ɏ������Ă��܂��B
        ''' </summary>
        ''' <value></value>
        ''' <returns>�v���p�e�B�̖��O�B</returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Name() As String _
                Implements IContextProperty.Name

            Get
                Return "TransactionAspectProperty"
            End Get

        End Property

        ''' <summary>
        ''' �w�肳�ꂽ�T�[�o�[ �I�u�W�F�N�g�̃��b�Z�[�W �V���N���w�肳�ꂽ
        ''' �V���N�`�F�[���̑O�ɂȂ��܂��B
        ''' TransactionAspect�N���X�̐V�����C���X�^���X��Ԃ��悤�Ɏ������Ă��܂��B
        ''' </summary>
        ''' <param name="obj">�w�肳�ꂽ�`�F�[���̑O�ɂȂ���A
        ''' ���b�Z�[�W �V���N��񋟂���T�[�o�[ �I�u�W�F�N�g�B</param>
        ''' <param name="nextSink">����܂łɍ쐬���ꂽ�V���N�`�F�C���B</param>
        ''' <returns>�����V���N �`�F�[���B</returns>
        ''' <remarks></remarks>
        Public Function GetObjectSink( _
                ByVal obj As MarshalByRefObject, _
                ByVal nextSink As IMessageSink) As IMessageSink _
                Implements IContributeObjectSink.GetObjectSink

            Dim biz As BaseBusinessComponent _
                    = DirectCast(obj, BaseBusinessComponent)

            Dim result As TransactionAspect _
                    = New TransactionAspect(biz, nextSink)

            Return result

        End Function

    End Class

End Namespace