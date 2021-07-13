'Copyright c2009 Toyota Motor Corporation. All rights reserved. For internal use only.
Imports System.Runtime.Remoting.Contexts
Imports System.Runtime.Remoting.Messaging
Imports Toyota.eCRB.SystemFrameworks.Core


Namespace Toyota.eCRB.SystemFrameworks.Core

    ''' <summary>
    ''' ContextBoundObjectを利用したアスペクト指向によるコミット管理機能を
    ''' 実現するためのコンテキストプロパティです。
    ''' </summary>
    ''' <remarks>オブジェクトがアクティベートされると、個々のコンテキスト
    ''' 属性について、GetPropertiesForNewContextメソッドが呼び出されます。
    ''' これにより、オブジェクトのために作成されつつある新コンテキストに結び
    ''' 付けられたプロパティリストに、独自のコンテキストプロパティを
    ''' 追加することができます。コンテキストプロパティは、
    ''' メッセージシンクチェーン内のオブジェクトにメッセージシンクを結
    ''' び付けられるようにします。コンテキストプロパティクラスは、
    ''' IContextPropertyとIContributeObjectSinkを実装し、アスペクトメッ
    ''' セージシンクのファクトリとして機能します。</remarks>
    Friend Class TransactionAspectProperty
        Implements IContextProperty, IContributeObjectSink

        ''' <summary>
        ''' コンストラクタです。
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()

        End Sub

        ''' <summary>
        ''' コンテキストが固定されるときに呼び出されます。
        ''' </summary>
        ''' <param name="newContext">固定するコンテキスト</param>
        ''' <remarks>コンテキストが固定された後でコンテキストの
        ''' プロパティを追加することはできません。</remarks>
        Public Sub Freeze( _
                ByVal newContext As Context) _
                Implements IContextProperty.Freeze

            ' 処理実装なし

        End Sub

        ''' <summary>
        ''' コンテキスト プロパティと新しいコンテキストとの間に
        ''' 互換性があるかどうかを示すブール値を返します。
        ''' </summary>
        ''' <param name="newCtx">ContextPropertyが作成された新しいコンテキスト。</param>
        ''' <returns>常にTrue。</returns>
        ''' <remarks>このメソッドはコンテキスト プロパティが特定の
        ''' コンテキスト内の他のコンテキスト プロパティと共存できる
        ''' 場合は True、それ以外の場合は Falseを返すように実装する必要が
        ''' あります。この実装では、常に True を返すように実施しています。
        ''' </remarks>
        Public Function IsNewContextOK( _
                ByVal newCtx As Context) As Boolean _
                Implements IContextProperty.IsNewContextOK

            Return True

        End Function

        ''' <summary>
        ''' コンテキストに追加されるときのプロパティの名前を取得します。
        ''' 型の名前を返すように実装しています。
        ''' </summary>
        ''' <value></value>
        ''' <returns>プロパティの名前。</returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Name() As String _
                Implements IContextProperty.Name

            Get
                Return "TransactionAspectProperty"
            End Get

        End Property

        ''' <summary>
        ''' 指定されたサーバー オブジェクトのメッセージ シンクを指定された
        ''' シンクチェーンの前につなげます。
        ''' TransactionAspectクラスの新しいインスタンスを返すように実装しています。
        ''' </summary>
        ''' <param name="obj">指定されたチェーンの前につなげる、
        ''' メッセージ シンクを提供するサーバー オブジェクト。</param>
        ''' <param name="nextSink">これまでに作成されたシンクチェイン。</param>
        ''' <returns>複合シンク チェーン。</returns>
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