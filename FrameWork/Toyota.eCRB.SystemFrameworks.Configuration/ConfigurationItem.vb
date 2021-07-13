'Copyright c2009 Toyota Motor Corporation. All rights reserved. For internal use only.
Namespace Toyota.eCRB.SystemFrameworks.Configuration

    ''' <summary>
    ''' Item�v�f���������邽�߂̃N���X�ł��B
    ''' ���̃N���X�̓A�v���P�[�V�����R�[�h�Ŏg�p���邽�߂̂��̂ł͂���܂���B
    ''' </summary>
    Public Class Item

        Private _name As String
        Private _serializeAs As String
        Private _value As String

        ''' <summary>
        ''' Item�v�f��Name�������擾���܂��B
        ''' </summary>
        ''' <value>Name�����̒l</value>
        Public ReadOnly Property Name() As String
            Get
                Return Me._name
            End Get
        End Property

        ''' <summary>
        ''' Item�v�f��SerializeAs�������擾���܂��B
        ''' </summary>
        ''' <value>SerializeAs�����̒l</value>
        Friend ReadOnly Property SerializeAs() As String
            Get
                Return Me._serializeAs
            End Get
        End Property

        ''' <summary>
        ''' Item�v�f��Value�������擾���܂��B
        ''' </summary>
        ''' <value>Value�����̒l</value>
        Public ReadOnly Property Value() As String
            Get
                Return Me._value
            End Get
        End Property

        ''' <summary>
        ''' Item�v�f���w�肵�āAItem�N���X�̃C���X�^���X������������B
        ''' </summary>
        ''' <param name="itemNode">�J�X�^���\���Z�N�V�����Ɋ܂܂��Item�v�f</param>
        Friend Sub New(ByVal itemNode As System.Xml.XmlNode)

            Me._name = itemNode.Attributes("Name").Value
            Me._serializeAs = itemNode.Attributes("SerializeAs").Value
            Me._value = itemNode.Attributes("Value").Value

        End Sub

    End Class

End Namespace
