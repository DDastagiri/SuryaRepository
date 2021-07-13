'Copyright c2009 Toyota Motor Corporation. All rights reserved. For internal use only.
Imports System.Runtime.Remoting.Messaging
Imports System.Reflection
Imports System.Threading.Thread
Imports System.Transactions
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Configuration


Namespace Toyota.eCRB.SystemFrameworks.Core

    ''' <summary>
    ''' �R�~�b�g�Ǘ��������郁�b�Z�[�W�V���N�̎����ł��B
    ''' </summary>
    ''' <remarks></remarks>
    Friend Class TransactionAspect
        Implements IMessageSink

        ''' <summary>
        ''' �A�X�y�N�g�ΏۃN���X�̃C���X�^���X�B
        ''' </summary>
        ''' <remarks></remarks>
        Private _biz As BaseBusinessComponent

        ''' <summary>
        ''' �V���N�`�F�C�����̎��̃��b�Z�[�W�V���N�B
        ''' </summary>
        ''' <remarks></remarks>
        Private _nextSink As IMessageSink

        ''' <summary>
        ''' �R���X�g���N�^�ł��B
        ''' </summary>
        ''' <param name="biz">�A�X�y�N�g�ΏۃN���X�̃C���X�^���X�B</param>
        ''' <param name="nextSink">�V���N�`�F�C�����̎���
        ''' ���b�Z�[�W�V���N�B</param>
        ''' <remarks></remarks>
        Public Sub New( _
                ByVal biz As BaseBusinessComponent, _
                ByVal nextSink As IMessageSink)

            Me._biz = biz
            Me._nextSink = nextSink

        End Sub

        ''' <summary>
        ''' �w�肵�����b�Z�[�W��񓯊��I�ɏ������܂��B
        ''' </summary>
        ''' <param name="msg">�������郁�b�Z�[�W�B</param>
        ''' <param name="replySink">�������b�Z�[�W�p�̉����V���N�B</param>
        ''' <returns>�f�B�X�p�b�`���ꂽ��̔񓯊����b�Z�[�W�𐧌�ł���悤��
        ''' ���� IMessageCtrl �C���^�[�t�F�C�X��Ԃ��܂��B</returns>
        ''' <remarks>���̎����ł́A�񓯊����b�Z�[�W���T�|�[�g���Ă��Ȃ����߁A
        ''' ��� Nothing ���߂�悤�Ɏ������Ă��܂��B</remarks>
        Public Function AsyncProcessMessage( _
                ByVal msg As IMessage, _
                ByVal replySink As IMessageSink) As IMessageCtrl _
                Implements IMessageSink.AsyncProcessMessage

            Return Nothing

        End Function

        ''' <summary>
        ''' �V���N �`�F�C�����̎��̃��b�Z�[�W �V���N���擾���܂��B
        ''' </summary>
        ''' <value></value>
        ''' <returns>�V���N �`�F�C�����̎��̃��b�Z�[�W �V���N�B</returns>
        ''' <remarks></remarks>
        Public ReadOnly Property NextSink() As IMessageSink _
                Implements IMessageSink.NextSink

            Get
                Return _nextSink
            End Get

        End Property

        ''' <summary>
        ''' �w�肵�����b�Z�[�W�𓯊��I�ɏ������܂��B 
        ''' </summary>
        ''' <param name="msg">�������郁�b�Z�[�W�B</param>
        ''' <returns>�v���ɑ΂��鉞�����b�Z�[�W�B </returns>
        ''' <remarks>���̃��\�b�h����A�^�[�Q�b�g�I�u�W�F�N�g�̃��\�b�h��
        ''' �Ăяo���܂��B�Ăяo���̑O��ŁATransactionScope�𐶐��A
        ''' ����уR�~�b�g�E���[���o�b�N�����s���܂��B</remarks>
        Public Function SyncProcessMessage( _
                ByVal msg As IMessage) As IMessage _
                Implements IMessageSink.SyncProcessMessage

            Dim methodMesssage As IMethodMessage _
                    = DirectCast(msg, IMethodMessage)

            Dim returnMessage As IMessage

            _biz.Rollback = False

            If TransactionAspect.ExistsEnableCommit( _
                    methodMesssage.MethodBase) Then ' �R�~�b�g��������

                Using scope As New TransactionScope( _
                        TransactionScopeOption.RequiresNew, New TimeSpan(0)) '�g�����U�N�V�����^�C���A�E�g���

                    ' �r�W�l�X���W�b�N�����s
                    returnMessage = _nextSink.SyncProcessMessage(msg)

                    Dim resultCommitException As Exception = DirectCast(returnMessage, IMethodReturnMessage).Exception
                    If resultCommitException IsNot Nothing Then
                        Throw resultCommitException
                    End If

                    If Not _biz.Rollback Then
                        ' �R�~�b�g
                        scope.Complete()
                    End If

                End Using

            Else ' �R�~�b�g�����Ȃ�
                ' �r�W�l�X���W�b�N�����s
                returnMessage = _nextSink.SyncProcessMessage(msg)

                Dim resultException As Exception = DirectCast(returnMessage, IMethodReturnMessage).Exception
                If resultException IsNot Nothing Then
                    Throw resultException
                End If
            End If

            Return returnMessage

        End Function

        ''' <summary>
        ''' �����Ŏw�肵�����\�b�h�ɁA
        ''' �R�~�b�g�������w�肳��Ă��邩�𔻒肵�܂��B
        ''' </summary>
        ''' <param name="m">���\�b�h���</param>
        ''' <returns>True�F�R�~�b�g��������AFalse�F�R�~�b�g�����Ȃ�</returns>
        ''' <remarks></remarks>
        Private Shared Function ExistsEnableCommit( _
                ByVal m As MethodBase) As Boolean

            Dim attributes() As Object = m.GetCustomAttributes(True)
            Dim retFlg As Boolean = False

            For Each attribute As Object In attributes

                If TypeOf attribute Is EnableCommitAttribute Then

                    retFlg = True
                    Exit For

                End If

            Next

            Return retFlg

        End Function

    End Class

End Namespace
