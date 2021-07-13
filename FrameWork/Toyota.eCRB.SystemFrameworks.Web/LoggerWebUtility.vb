'Copyright c2009 Toyota Motor Corporation. All rights reserved. For internal use only.
Imports System.Text
Imports System.Web
Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks.Core

Namespace Toyota.eCRB.SystemFrameworks.Web

    ''' <summary>
    ''' �I�����C���݂̂ŗ��p����A���O�o�͂̂��߂̃��[�e�B���e�B�@�\��񋟂���N���X�ł��B
    ''' ���O�̏o�͐ݒ�́A�O���t�@�C���Ƃ��Ē�`����܂��B
    ''' </summary>
    ''' <remarks>
    ''' �����o�ɃA�N�Z�X���邽�߂ɁA�ÓI�N���X�̃C���X�^���X��錾����K�v�͂���܂���B
    ''' ���̃N���X�̓A�Z���u���O�Ɍ��J���܂��B
    ''' ���̃N���X�͌p���ł��܂���B
    ''' </remarks>
    Friend NotInheritable Class LoggerWebUtility

        ''' <summary>
        ''' �R���X�g���N�^�ł��B�C���X�^���X�𐶐������Ȃ��悤�ɂ��邽�߁A�C���q��Private�ł��B
        ''' </summary>
        Private Sub New()

        End Sub

        ' ''' <summary>
        ' ''' Friend Shared Sub SetWebConfig()
        ' ''' </summary>
        'Friend Shared Sub SetWebConfig()

        '    '1.�ғ����O�̃t�@�C�����ƁA�C�x���g���O�̃\�[�X���ڕ������ݒ肷��B		
        '    LoggerUtility.UpdateLogConfiguration()

        'End Sub

        ''' <summary>
        ''' ���O�o�͂ɕK�v�ȃA�N�Z�XURL�ƃA�v��ID�����擾���A�R���e�L�X�g�ɏ����i�[����B
        ''' </summary>
        ''' <param name="context">�R���e�L�X�g�B</param>
        Friend Shared Sub SetAccessUrlInfo(ByVal context As HttpContext)
            '1.���݂�URL���擾���A���[�J���ϐ�aplId�Ɋi�[����B
            Dim aplId As String = context.Request.AppRelativeCurrentExecutionFilePath

            '2.URL����A�N�Z�XURL�i"/"��؂�̈�ԍŌ�̕�����j���擾���A���[�J���ϐ�aplId�Ɋi�[����B
            aplId = aplId.Substring(aplId.LastIndexOf("/", StringComparison.OrdinalIgnoreCase) + 1)

            '3.�A�N�Z�XURL���R���e�L�X�g�Ɋi�[����B�L�[�́A�萔LoggerUtility.CONTEXT_KEY_ACCESSURL�𗘗p����B
            context.Items(LoggerUtility.ContextKeyAccessUrl) = aplId

            '4.�A�N�Z�XURL����A�v��ID���擾���A���[�J���ϐ�aplId�Ɋi�[����B
            aplId = aplId.Remove(aplId.LastIndexOf(".", StringComparison.OrdinalIgnoreCase))

            '5.�A�v��ID���R���e�L�X�g�Ɋi�[����B�L�[�́A�萔LoggerUtility.CONTEXT_KEY_APLID�𗘗p����B
            context.Items(LoggerUtility.ContextKeyAplId) = aplId
        End Sub

        ''' <summary>
        ''' ���O�o�͂ɕK�v�ȃ��O�C��ID�A���[�U�������R���e�L�X�g�Ɋi�[����B
        ''' </summary>
        ''' <param name="context">�R���e�L�X�g�B</param>
        ''' <param name="loginId">���O�C��ID</param>
        ''' <param name="selectedRole">���݂̃��[�U����</param>
        Friend Shared Sub SetUserInfo(ByVal context As HttpContext, _
                                      ByVal loginId As String, _
                                      ByVal selectedRole As String)
            '1.���O�C��ID���R���e�L�X�g�Ɋi�[����B�L�[�́A�萔LoggerUtility.CONTEXT_KEY_LOGINID�𗘗p����B
            context.Items(LoggerUtility.ContextKeyLoginId) = loginId

            '2.���݂̌������R���e�L�X�g�Ɋi�[����B�L�[�́A�萔LoggerUtility.CONTEXT_KEY_SELECTEDROLE�𗘗p����B
            context.Items(LoggerUtility.ContextKeySelectedRole) = selectedRole

        End Sub

        ''' <summary>
        ''' ASP.NET�F�؏���胍�O�C��ID���擾����B
        ''' </summary>
        ''' <param name="context">�R���e�L�X�g�B</param>
        ''' <returns>���O�C��ID������</returns>
        Friend Shared Function GetLoginIdFromHttpHeader(ByVal context As HttpContext) As String
            '1.ASP.NET�F�؏�񂩂烍�O�C��ID���擾���܂��B
            '2.�擾�����������Ԃ��܂��B
            Return context.User.Identity.Name

        End Function

        ' ''' <summary>
        ' ''' Friend Shared Function GetSessionInfo() As String
        ' ''' </summary>
        ' ''' <returns>�Z�b�V�������</returns>
        'Friend Shared Function GetSessionInfo() As String
        '    '(��������̂��߁A���݂�Return "" �����������Ă������Ɓj
        '    Return ""
        'End Function

    End Class

End Namespace