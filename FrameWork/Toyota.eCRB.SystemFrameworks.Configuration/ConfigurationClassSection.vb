'Copyright c2009 Toyota Motor Corporation. All rights reserved. For internal use only.

Imports System.Globalization
Imports System.Diagnostics.CodeAnalysis

Namespace Toyota.eCRB.SystemFrameworks.Configuration

    ''' <summary>
    ''' Class要素を処理するためのクラスです。
    ''' このクラスはアプリケーションコードで使用するためのものではありません。
    ''' </summary>
    Public Class ClassSection

        Private _settings As List(Of Setting)
        Private _items As List(Of Item)

        ''' <summary>
        ''' Class要素に含まれるSetting要素のリストを取得します。
        ''' </summary>
        ''' <value>Setting要素のリスト</value>
        <SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists"), _
         SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods")> _
        Public ReadOnly Property Setting() As List(Of Setting)
            Get
                Return Me._settings
            End Get
        End Property

        ''' <summary>
        ''' ClassSectionのインスタンスを初期化します。
        ''' </summary>
        Public Sub New()

            Me._settings = New List(Of Setting)
            Me._items = New List(Of Item)

        End Sub

        ''' <summary>
        ''' カスタム構成セクション中のSetting要素を指定して、ClassSectionのインスタンスを初期化します。
        ''' </summary>
        ''' <param name="classSectionNode">カスタム構成セクション中のSetting要素</param>
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
        ''' Class要素の直下に含まれるItem要素のValue属性を取得します。
        ''' </summary>
        ''' <param name="itemName">Item要素のName属性</param>
        ''' <returns>Item要素のValue属性</returns>
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
        ''' Class要素の直下に含まれるSetting要素を取得します。
        ''' </summary>
        ''' <param name="settingName">Setting要素のName属性</param>
        ''' <returns>Settingクラスのインスタンス</returns>
        ''' <remarks>
        ''' settingNameが空文字列の場合は、Name属性が空または"default"の最初のSetting要素を処理するSettingクラスのインスタンスを返す。
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
