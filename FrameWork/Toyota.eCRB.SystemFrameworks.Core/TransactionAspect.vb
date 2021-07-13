'Copyright c2009 Toyota Motor Corporation. All rights reserved. For internal use only.
Imports System.Runtime.Remoting.Messaging
Imports System.Reflection
Imports System.Threading.Thread
Imports System.Transactions
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Configuration


Namespace Toyota.eCRB.SystemFrameworks.Core

    ''' <summary>
    ''' コミット管理処理するメッセージシンクの実装です。
    ''' </summary>
    ''' <remarks></remarks>
    Friend Class TransactionAspect
        Implements IMessageSink

        ''' <summary>
        ''' アスペクト対象クラスのインスタンス。
        ''' </summary>
        ''' <remarks></remarks>
        Private _biz As BaseBusinessComponent

        ''' <summary>
        ''' シンクチェイン内の次のメッセージシンク。
        ''' </summary>
        ''' <remarks></remarks>
        Private _nextSink As IMessageSink

        ''' <summary>
        ''' コンストラクタです。
        ''' </summary>
        ''' <param name="biz">アスペクト対象クラスのインスタンス。</param>
        ''' <param name="nextSink">シンクチェイン内の次の
        ''' メッセージシンク。</param>
        ''' <remarks></remarks>
        Public Sub New( _
                ByVal biz As BaseBusinessComponent, _
                ByVal nextSink As IMessageSink)

            Me._biz = biz
            Me._nextSink = nextSink

        End Sub

        ''' <summary>
        ''' 指定したメッセージを非同期的に処理します。
        ''' </summary>
        ''' <param name="msg">処理するメッセージ。</param>
        ''' <param name="replySink">応答メッセージ用の応答シンク。</param>
        ''' <returns>ディスパッチされた後の非同期メッセージを制御できるように
        ''' する IMessageCtrl インターフェイスを返します。</returns>
        ''' <remarks>この実装では、非同期メッセージをサポートしていないため、
        ''' 常に Nothing が戻るように実装しています。</remarks>
        Public Function AsyncProcessMessage( _
                ByVal msg As IMessage, _
                ByVal replySink As IMessageSink) As IMessageCtrl _
                Implements IMessageSink.AsyncProcessMessage

            Return Nothing

        End Function

        ''' <summary>
        ''' シンク チェイン内の次のメッセージ シンクを取得します。
        ''' </summary>
        ''' <value></value>
        ''' <returns>シンク チェイン内の次のメッセージ シンク。</returns>
        ''' <remarks></remarks>
        Public ReadOnly Property NextSink() As IMessageSink _
                Implements IMessageSink.NextSink

            Get
                Return _nextSink
            End Get

        End Property

        ''' <summary>
        ''' 指定したメッセージを同期的に処理します。 
        ''' </summary>
        ''' <param name="msg">処理するメッセージ。</param>
        ''' <returns>要求に対する応答メッセージ。 </returns>
        ''' <remarks>このメソッドから、ターゲットオブジェクトのメソッドを
        ''' 呼び出します。呼び出しの前後で、TransactionScopeを生成、
        ''' およびコミット・ロールバックを実行します。</remarks>
        Public Function SyncProcessMessage( _
                ByVal msg As IMessage) As IMessage _
                Implements IMessageSink.SyncProcessMessage

            Dim methodMesssage As IMethodMessage _
                    = DirectCast(msg, IMethodMessage)

            Dim returnMessage As IMessage

            _biz.Rollback = False

            If TransactionAspect.ExistsEnableCommit( _
                    methodMesssage.MethodBase) Then ' コミット属性あり

                Using scope As New TransactionScope( _
                        TransactionScopeOption.RequiresNew, New TimeSpan(0)) 'トランザクションタイムアウト回避

                    ' ビジネスロジックを実行
                    returnMessage = _nextSink.SyncProcessMessage(msg)

                    Dim resultCommitException As Exception = DirectCast(returnMessage, IMethodReturnMessage).Exception
                    If resultCommitException IsNot Nothing Then
                        Throw resultCommitException
                    End If

                    If Not _biz.Rollback Then
                        ' コミット
                        scope.Complete()
                    End If

                End Using

            Else ' コミット属性なし
                ' ビジネスロジックを実行
                returnMessage = _nextSink.SyncProcessMessage(msg)

                Dim resultException As Exception = DirectCast(returnMessage, IMethodReturnMessage).Exception
                If resultException IsNot Nothing Then
                    Throw resultException
                End If
            End If

            Return returnMessage

        End Function

        ''' <summary>
        ''' 引数で指定したメソッドに、
        ''' コミット属性が指定されているかを判定します。
        ''' </summary>
        ''' <param name="m">メソッド情報</param>
        ''' <returns>True：コミット属性あり、False：コミット属性なし</returns>
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
