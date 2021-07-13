'Copyright c2009 Toyota Motor Corporation. All rights reserved. For internal use only.

Imports System.Diagnostics.CodeAnalysis
Imports Oracle.DataAccess.Client



Namespace Toyota.eCRB.SystemFrameworks.Core

    ''' <summary>
    ''' �g�����U�N�V�����������r�W�l�X�R���|�[�l���g�N���X�ł��B
    ''' �R�~�b�g�Ǘ��@�\��񋟂��܂��B
    ''' </summary>
    ''' <remarks>�A�v���P�[�V�����ł̓g�����U�N�V����������
    ''' �r�W�l�X���W�b�N�N���X���쐬����Ƃ��A���̃N���X�����N���X�Ƃ��Ă��������B
    ''' ���̃N���X�ɂ́A�R�~�b�g�Ǘ���L���ɂ��邽�߂ɃN���X�����Ƃ���
    ''' TransactionAspect�������ݒ肳��Ă��܂��B</remarks>
    <TransactionAspect()> _
    Public MustInherit Class BaseBusinessComponent
        Inherits ContextBoundObject

        ' ''' <summary>
        ' ''' �g�����U�N�V������������TableAdapter�ֈ����n��Connection�B
        ' ''' </summary>
        ' ''' <remarks></remarks>
        'Private _trxConnection As OracleConnection

        ''' <summary>
        ''' �R�~�b�g�Ǘ��Ώۂ̃��\�b�h�I�����Ƀg�����U�N�V������
        ''' ���[���o�b�N���邩��ݒ�B
        ''' </summary>
        ''' <remarks></remarks>
        Private _rollback As Boolean

        ''' <summary>
        ''' �R���X�g���N�^
        ''' </summary>
        ''' <remarks>�T�u�N���X�̂݃C���X�^�����ł��܂��B</remarks>
        Protected Sub New()
            MyBase.New()
        End Sub

        ' ''' <summary>
        ' ''' �g�����U�N�V������������TableAdapter�ֈ����n��Connection�B
        ' ''' </summary>
        ' ''' <value></value>
        ' ''' <returns>Oracle�f�[�^�x�[�X�ڑ�</returns>
        ' ''' <remarks></remarks>
        'Public ReadOnly Property TrxConnection() As OracleConnection

        '    Get
        '        Return _trxConnection
        '    End Get

        'End Property

        ''' <summary>
        ''' �R�~�b�g�Ǘ��Ώۂ̃��\�b�h�I�����Ƀg�����U�N�V������
        ''' ���[���o�b�N���邩��ݒ�B
        ''' </summary>
        ''' <value>True:���[���o�b�N����AFalse:���[���o�b�N���Ȃ�</value>
        ''' <returns>True:���[���o�b�N����AFalse:���[���o�b�N���Ȃ�</returns>
        ''' <remarks></remarks>
        Public Property Rollback() As Boolean

            Get
                Return _rollback
            End Get

            Set(ByVal value As Boolean)
                _rollback = value
            End Set

        End Property

        ' ''' <summary>
        ' ''' �A�X�y�N�g�����ɂĐ�������Connection���Z�b�g���ABizLogic��
        ' ''' �C���X�^���X�ϐ��Ƃ��ĕێ�������B
        ' ''' </summary>
        ' ''' <value>Oracle�f�[�^�x�[�X�ڑ�</value>
        ' ''' <remarks></remarks>
        'Friend WriteOnly Property OpenedConnection() As OracleConnection

        '    Set(ByVal value As OracleConnection)
        '        _trxConnection = value
        '    End Set

        'End Property

    End Class

End Namespace