'Copyright c2009 Toyota Motor Corporation. All rights reserved. For internal use only.
Imports System.Runtime.Remoting.Contexts
Imports System.Runtime.Remoting.Activation


Namespace Toyota.eCRB.SystemFrameworks.Core

    ''' <summary>
    ''' ContextBoundObject�𗘗p�����A�X�y�N�g�w���ɂ��g�����U�N�V����
    ''' �Ǘ������̂��߂̃R���e�L�X�g�����ł��B
    ''' </summary>
    ''' <remarks>ContextBoundObject�𗘗p�����A�X�y�N�g�w���ɂ��
    ''' �g�����U�N�V�����Ǘ����������邽�߂ɂ́A���b�Z�[�W�V���N��
    ''' �`�F�[���ɎQ������ɁA�܂��AContextAttribute�i�P�Ȃ�
    ''' Attribute�ł͂Ȃ��j�h���N���X�����A�R���e�L�X�g�v���p�e�B��
    ''' �Ă΂����̂�^���āAContextBoundObject�ƂƂ��ɎQ������悤��
    ''' ����������������K�v������܂��B</remarks>
    Friend NotInheritable Class TransactionAspectAttribute
        Inherits ContextAttribute

        ''' <summary>
        ''' �R���X�g���N�^�ł��B
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()

            MyBase.New("TransactionAspect")

        End Sub

        ''' <summary>
        ''' ���݂̃R���e�L�X�g�v���p�e�B���A�w�肳�ꂽ���b�Z�[�W�ɒǉ����܂��B 
        ''' </summary>
        ''' <param name="ccm">�R���e�L�X�g �v���p�e�B��ǉ�����Ώۂ�
        ''' IConstructionCallMessage�B</param>
        ''' <remarks>GetPropertiesForNewContext ���\�b�h�́A�����
        ''' IConstructionCallMessage �N���X�Ƀv���p�e�B��ǉ����āA
        ''' ���b�Z�[�W����M���ꂽ�Ƃ��ɁA�v�����ꂽ�R���e�L�X�g����
        ''' �V�����I�u�W�F�N�g���쐬�ł���悤�ɂ��܂��B</remarks>
        Public Overrides Sub GetPropertiesForNewContext( _
                ByVal ccm As IConstructionCallMessage)

            If ccm IsNot Nothing Then
                ccm.ContextProperties.Add(New TransactionAspectProperty())
            End If
        End Sub

    End Class

End Namespace
