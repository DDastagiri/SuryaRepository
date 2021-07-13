'Copyright c2009 Toyota Motor Corporation. All rights reserved. For internal use only.

Imports System.Globalization
Imports System.Diagnostics.CodeAnalysis

Namespace Toyota.eCRB.SystemFrameworks.Configuration

    ''' <summary>
    ''' Setting�v�f���������邽�߂̃N���X�ł��B
    ''' ���̃N���X�̓A�v���P�[�V�����R�[�h�Ŏg�p���邽�߂̂��̂ł͂���܂���B
    ''' </summary>
    Public Class Setting

        Private _name As String
        Private _setting As List(Of Item)

        ''' <summary>
        ''' Setting�v�f��Name�������擾���܂��B
        ''' </summary>
        ''' <value>Name�����̒l</value>
        Friend ReadOnly Property Name() As String
            Get
                Return Me._name
            End Get
        End Property

        ''' <summary>
        ''' Setting�v�f�Ɋ܂܂��Item�v�f�̃��X�g���擾���܂��B
        ''' </summary>
        ''' <value>Item�v�f�̃��X�g</value>
        <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")> _
        Public ReadOnly Property Item() As List(Of Item)
            Get
                Return Me._setting
            End Get
        End Property

        ''' <summary>
        ''' Setting�N���X�̃C���X�^���X�����������܂��B
        ''' </summary>
        Friend Sub New()

            Me._setting = New List(Of Item)

        End Sub

        ''' <summary>
        ''' �J�X�^���\���Z�N�V��������Setting�v�f���w�肵��Setting�N���X�̃C���X�^���X�����������܂��B
        ''' </summary>
        ''' <param name="settingSectionNode">�J�X�^���\���Z�N�V��������Setting�v�f</param>
        Friend Sub New(ByVal settingSectionNode As System.Xml.XmlNode)

            Me.New()

            Me._name = settingSectionNode.Attributes("Name").Value

            For Each childNode As System.Xml.XmlNode In settingSectionNode.ChildNodes
                If (childNode.Name.Equals("Item")) Then
                    Dim itemNode As Toyota.eCRB.SystemFrameworks.Configuration.Item = New Toyota.eCRB.SystemFrameworks.Configuration.Item(childNode)

                    Me._setting.Add(itemNode)
                End If
            Next
        End Sub

        ''' <summary>
        ''' Setting�v�f�z����Item�v�f�ɐݒ肳��Ă�����e���擾���܂��B
        ''' </summary>
        ''' <param name="itemName">Item�v�f��Name����</param>
        ''' <returns>Item�v�f��Value�����ɐݒ肳��Ă���l</returns>
        Public Function GetValue(ByVal itemName As String) As Object

            Dim item As Item = Nothing
            Dim returnValue As Object = Nothing

            For Each item In Me._setting
                If (item.Name.Equals(itemName)) Then
                    returnValue = System.Convert.ChangeType(item.Value, System.Type.GetType(item.SerializeAs), CultureInfo.InvariantCulture)
                    Exit For
                End If
            Next

            Return returnValue

        End Function

    End Class

End Namespace
