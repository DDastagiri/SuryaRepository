'Copyright c2009 Toyota Motor Corporation. All rights reserved. For internal use only.

Imports System.Diagnostics.CodeAnalysis
Imports Oracle.DataAccess.Client



Namespace Toyota.eCRB.SystemFrameworks.Core

    ''' <summary>
    ''' トランザクション処理基底ビジネスコンポーネントクラスです。
    ''' コミット管理機能を提供します。
    ''' </summary>
    ''' <remarks>アプリケーションではトランザクション処理の
    ''' ビジネスロジッククラスを作成するとき、このクラスを基底クラスとしてください。
    ''' このクラスには、コミット管理を有効にするためにクラス属性として
    ''' TransactionAspect属性が設定されています。</remarks>
    <TransactionAspect()> _
    Public MustInherit Class BaseBusinessComponent
        Inherits ContextBoundObject

        ' ''' <summary>
        ' ''' トランザクション処理内でTableAdapterへ引き渡すConnection。
        ' ''' </summary>
        ' ''' <remarks></remarks>
        'Private _trxConnection As OracleConnection

        ''' <summary>
        ''' コミット管理対象のメソッド終了時にトランザクションを
        ''' ロールバックするかを設定。
        ''' </summary>
        ''' <remarks></remarks>
        Private _rollback As Boolean

        ''' <summary>
        ''' コンストラクタ
        ''' </summary>
        ''' <remarks>サブクラスのみインスタン化できます。</remarks>
        Protected Sub New()
            MyBase.New()
        End Sub

        ' ''' <summary>
        ' ''' トランザクション処理内でTableAdapterへ引き渡すConnection。
        ' ''' </summary>
        ' ''' <value></value>
        ' ''' <returns>Oracleデータベース接続</returns>
        ' ''' <remarks></remarks>
        'Public ReadOnly Property TrxConnection() As OracleConnection

        '    Get
        '        Return _trxConnection
        '    End Get

        'End Property

        ''' <summary>
        ''' コミット管理対象のメソッド終了時にトランザクションを
        ''' ロールバックするかを設定。
        ''' </summary>
        ''' <value>True:ロールバックする、False:ロールバックしない</value>
        ''' <returns>True:ロールバックする、False:ロールバックしない</returns>
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
        ' ''' アスペクト処理にて生成したConnectionをセットし、BizLogicの
        ' ''' インスタンス変数として保持させる。
        ' ''' </summary>
        ' ''' <value>Oracleデータベース接続</value>
        ' ''' <remarks></remarks>
        'Friend WriteOnly Property OpenedConnection() As OracleConnection

        '    Set(ByVal value As OracleConnection)
        '        _trxConnection = value
        '    End Set

        'End Property

    End Class

End Namespace