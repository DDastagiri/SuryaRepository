'Copyright c2009 Toyota Motor Corporation. All rights reserved. For internal use only.

Imports System.Globalization
Imports System.Diagnostics.CodeAnalysis

Namespace Toyota.eCRB.SystemFrameworks.Configuration

    ''' <summary>
    ''' Class�v�f���������邽�߂̃N���X�ł��B
    ''' ���̃N���X�̓A�v���P�[�V�����R�[�h�Ŏg�p���邽�߂̂��̂ł͂���܂���B
    ''' </summary>
    Public Class ClassSection

        Private _settings As List(Of Setting)
        Private _items As List(Of Item)

        ''' <summary>
        ''' Class�v�f�Ɋ܂܂��Setting�v�f�̃��X�g���擾���܂��B
        ''' </summary>
        ''' <value>Setting�v�f�̃��X�g</value>
        <SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists"), _
         SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods")> _
        Public ReadOnly Property Setting() As List(Of Setting)
            Get
                Return Me._settings
            End Get
        End Property

        ''' <summary>
        ''' ClassSection�̃C���X�^���X�����������܂��B
        ''' </summary>
        Public Sub New()

            Me._settings = New List(Of Setting)
            Me._items = New List(Of Item)

        End Sub

        ''' <summary>
        ''' �J�X�^���\���Z�N�V��������Setting�v�f���w�肵�āAClassSection�̃C���X�^���X�����������܂��B
        ''' </summary>
        ''' <param name="classSectionNode">�J�X�^���\���Z�N�V��������Setting�v�f</param>
        Public Sub New(ByVal classSectionNode As System.Xml.XmlNode)

            Me.New()

            Dim ienum As IEnumerator = classSectionNode.ChildNodes().GetEnumerator()
            While ienum.MoveNext()
                Dim childNode As System.Xml.XmlNode = DirectCast(ienum.Current, System.Xml.XmlNode)
                If (childNode.Name.Equals("Item")) Then
                    Dim itemNode As Toyota.eCRB.SystemFrameworks.Configuration.Item = New Toyota.eCRB.SystemFrameworks.Configuration.Item(childNode)

                    Me._items.Add(itemNode)
                End If
                If (childNode.Name.Equals("Setting")) Then
                    Dim settingNode As Toyota.eCRB.SystemFrameworks.Configuration.Setting = New Toyota.eCRB.SystemFrameworks.Configuration.Setting(childNode)

                    Me._settings.Add(settingNode)
                End If
            End While

        End Sub

        ''' <summary>
        ''' Class�v�f�̒����Ɋ܂܂��Item�v�f��Value�������擾���܂��B
        ''' </summary>
        ''' <param name="itemName">Item�v�f��Name����</param>
        ''' <returns>Item�v�f��Value����</returns>
        Public Function GetValue(ByVal itemName As String) As Object

            Dim item As Item = Nothing
            Dim returnValue As Object = Nothing

            For Each item In Me._items
                If (item.Name.Equals(itemName)) Then
                    returnValue = System.Convert.ChangeType(item.Value, System.Type.GetType(item.SerializeAs), CultureInfo.InvariantCulture)
                    Exit For
                End If
            Next

            Return returnValue

        End Function

        ''' <summary>
        ''' Class�v�f�̒����Ɋ܂܂��Setting�v�f���擾���܂��B
        ''' </summary>
        ''' <param name="settingName">Setting�v�f��Name����</param>
        ''' <returns>Setting�N���X�̃C���X�^���X</returns>
        ''' <remarks>
        ''' settingName���󕶎���̏ꍇ�́AName��������܂���"default"�̍ŏ���Setting�v�f����������Setting�N���X�̃C���X�^���X��Ԃ��B
        ''' </remarks>
        Public Function GetSetting(ByVal settingName As String) As Setting

            Dim returnValue As Setting = Nothing

            If settingName Is Nothing Then
                Return returnValue
            End If

            If (settingName.Length = 0) Then
                For Each settingSection As Toyota.eCRB.SystemFrameworks.Configuration.Setting In Me._settings
                    If (String.IsNullOrEmpty(settingSection.Name)) Or (settingSection.Name.Equals("default")) Then
                        returnValue = settingSection
                        Exit For
                    End If
                Next
            Else
                For Each settingSection As Toyota.eCRB.SystemFrameworks.Configuration.Setting In Me._settings
                    If (settingSection.Name.Equals(settingName)) Then
                        returnValue = settingSection
                        Exit For
                    End If
                Next
            End If

            Return returnValue

        End Function

    End Class

End Namespace
