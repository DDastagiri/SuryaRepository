'Copyright c2009 Toyota Motor Corporation. All rights reserved. For internal use only.
Imports System.Runtime.Remoting.Contexts
Imports System.Runtime.Remoting.Activation


Namespace Toyota.eCRB.SystemFrameworks.Core

    ''' <summary>
    ''' ContextBoundObjectを利用したアスペクト指向によるトランザクション
    ''' 管理実現のためのコンテキスト属性です。
    ''' </summary>
    ''' <remarks>ContextBoundObjectを利用したアスペクト指向による
    ''' トランザクション管理を実現するためには、メッセージシンクの
    ''' チェーンに参加するに、まず、ContextAttribute（単なる
    ''' Attributeではなく）派生クラスを作り、コンテキストプロパティと
    ''' 呼ばれるものを与えて、ContextBoundObjectとともに参加するように
    ''' 属性を書き換える必要があります。</remarks>
    Friend NotInheritable Class TransactionAspectAttribute
        Inherits ContextAttribute

        ''' <summary>
        ''' コンストラクタです。
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()

            MyBase.New("TransactionAspect")

        End Sub

        ''' <summary>
        ''' 現在のコンテキストプロパティを、指定されたメッセージに追加します。 
        ''' </summary>
        ''' <param name="ccm">コンテキスト プロパティを追加する対象の
        ''' IConstructionCallMessage。</param>
        ''' <remarks>GetPropertiesForNewContext メソッドは、特定の
        ''' IConstructionCallMessage クラスにプロパティを追加して、
        ''' メッセージが受信されたときに、要求されたコンテキスト環境で
        ''' 新しいオブジェクトを作成できるようにします。</remarks>
        Public Overrides Sub GetPropertiesForNewContext( _
                ByVal ccm As IConstructionCallMessage)

            If ccm IsNot Nothing Then
                ccm.ContextProperties.Add(New TransactionAspectProperty())
            End If
        End Sub

    End Class

End Namespace
