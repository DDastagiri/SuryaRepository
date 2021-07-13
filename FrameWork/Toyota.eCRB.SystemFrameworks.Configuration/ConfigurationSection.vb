'Copyright c2009 Toyota Motor Corporation. All rights reserved. For internal use only.

Imports System.Globalization
Imports System.Diagnostics.CodeAnalysis

Namespace Toyota.eCRB.SystemFrameworks.Configuration

    ''' <summary>
    ''' Setting要素を処理するためのクラスです。
    ''' このクラスはアプリケーションコードで使用するためのものではありません。
    ''' </summary>
    Public Class Setting

        Private _name As String
        Private _setting As List(Of Item)

        ''' <summary>
        ''' Setting要素のName属性を取得します。
        ''' </summary>
        ''' <value>Name属性の値</value>
        Friend ReadOnly Property Name() As String
            Get
                Return Me._name
            End Get
        End Property

        ''' <summary>
        ''' Setting要素に含まれるItem要素のリストを取得します。
        ''' </summary>
        ''' <value>Item要素のリスト</value>
        <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")> _
        Public ReadOnly Property Item() As List(Of Item)
            Get
                Return Me._setting
            End Get
        End Property

        ''' <summary>
        ''' Settingクラスのインスタンスを初期化します。
        ''' </summary>
        Friend Sub New()

            Me._setting = New List(Of Item)

        End Sub

        ''' <summary>
        ''' カスタム構成セクション中のSetting要素を指定してSettingクラスのインスタンスを初期化します。
        ''' </summary>
        ''' <param name="settingSectionNode">カスタム構成セクション中のSetting要素</param>
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
        ''' Setting要素配下のItem要素に設定されている内容を取得します。
        ''' </summary>
        ''' <param name="itemName">Item要素のName属性</param>
        ''' <returns>Item要素のValue属性に設定されている値</returns>
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
