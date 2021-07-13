'Copyright c2009 Toyota Motor Corporation. All rights reserved. For internal use only.
Namespace Toyota.eCRB.SystemFrameworks.Configuration

    ''' <summary>
    ''' Item要素を処理するためのクラスです。
    ''' このクラスはアプリケーションコードで使用するためのものではありません。
    ''' </summary>
    Public Class Item

        Private _name As String
        Private _serializeAs As String
        Private _value As String

        ''' <summary>
        ''' Item要素のName属性を取得します。
        ''' </summary>
        ''' <value>Name属性の値</value>
        Public ReadOnly Property Name() As String
            Get
                Return Me._name
            End Get
        End Property

        ''' <summary>
        ''' Item要素のSerializeAs属性を取得します。
        ''' </summary>
        ''' <value>SerializeAs属性の値</value>
        Friend ReadOnly Property SerializeAs() As String
            Get
                Return Me._serializeAs
            End Get
        End Property

        ''' <summary>
        ''' Item要素のValue属性を取得します。
        ''' </summary>
        ''' <value>Value属性の値</value>
        Public ReadOnly Property Value() As String
            Get
                Return Me._value
            End Get
        End Property

        ''' <summary>
        ''' Item要素を指定して、Itemクラスのインスタンスを初期化する。
        ''' </summary>
        ''' <param name="itemNode">カスタム構成セクションに含まれるItem要素</param>
        Friend Sub New(ByVal itemNode As System.Xml.XmlNode)

            Me._name = itemNode.Attributes("Name").Value
            Me._serializeAs = itemNode.Attributes("SerializeAs").Value
            Me._value = itemNode.Attributes("Value").Value

        End Sub

    End Class

End Namespace
