Imports System
Imports System.Collections
Imports System.Data
Imports System.IO
Imports System.Reflection

Namespace Toyota.eCRB.SystemFrameworks.Core
    Friend Class ObjectDumper
        Public Delegate Sub WriteLineDelegate(ByVal msg As String)
        Private maxObjectInfoLine As Integer
        Private maxObjectInfoLength As Integer
        Private writer As WriteLineDelegate
        Private indentLevel As Integer
        Private lineCount As Integer

        Public Sub New(ByVal writer As WriteLineDelegate, ByVal maxObjectInfoLine As Integer, ByVal maxObjectInfoLength As Integer)
            Me.maxObjectInfoLine = maxObjectInfoLine
            Me.maxObjectInfoLength = maxObjectInfoLength
            Me.writer = writer
            Me.indentLevel = 0
            Me.lineCount = 0
        End Sub

        Public Sub OutputObjectString(ByVal o As Object)
            If lineCount > maxObjectInfoLine Then Return

            If o Is Nothing Then
                Me.OutputString("null")
            ElseIf TypeOf o Is String Then
                Me.OutputString(CStr(o))
            ElseIf TypeOf o Is DataSet Then
                OutputDataSetString(CType(o, DataSet))
            ElseIf TypeOf o Is DataTable Then
                OutputDataTableString(CType(o, DataTable))
            ElseIf TypeOf o Is Byte() Then
                Me.OutputString(o.ToString())
            ElseIf TypeOf o Is IEnumerable Then
                OutputString("[]")
                indentLevel += 1

                For Each item As Object In CType(o, IEnumerable)
                    OutputObjectString(item)
                    If lineCount >= maxObjectInfoLine Then Exit For
                Next

                indentLevel -= 1
            Else
                Dim objString As String = o.ToString()

                If objString = o.[GetType]().ToString() Then
                    OutputMemberInfoString(o)
                Else
                    Me.OutputString(objString)
                End If
            End If
        End Sub

        Private Sub OutputMemberInfoString(ByVal o As Object)
            Me.OutputString(o.[GetType]().Name)
            indentLevel += 1

            Try
                Dim props As PropertyInfo() = o.[GetType]().GetProperties(BindingFlags.[Public] Or BindingFlags.Instance Or BindingFlags.GetProperty)

                For i As Integer = 0 To props.Length - 1

                    If props(i).GetIndexParameters().Length = 0 Then
                        OutputMemberValue(props(i).Name, GetPropValue(o, props(i)))
                        If lineCount >= maxObjectInfoLine Then Return
                    End If
                Next

                Dim fields As FieldInfo() = o.[GetType]().GetFields(BindingFlags.[Public] Or BindingFlags.Instance)

                For i As Integer = 0 To fields.Length - 1
                    OutputMemberValue(fields(i).Name, fields(i).GetValue(o))
                    If lineCount >= maxObjectInfoLine Then Return
                Next

            Finally
                indentLevel -= 1
            End Try
        End Sub

        Private Sub OutputMemberValue(ByVal name As String, ByVal value As Object)
            If value Is Nothing Then
                Me.OutputString(name & "=null")
                Return
            End If

            Dim valueType As Type = value.GetType()

            If valueType = GetType(String) Then
                Me.OutputString(name & "=" & DirectCast(value, String))
            ElseIf valueType.IsValueType Then
                Me.OutputString(name & "=" & value.ToString())
            Else
                Me.OutputString(name)
                indentLevel += 1
                OutputObjectString(value)
                indentLevel -= 1
            End If
        End Sub

        Private Function GetPropValue(ByVal o As Object, ByVal prop As PropertyInfo) As Object
            Try
                Return prop.GetValue(o, Nothing)
            Catch ex As Exception
                Return ex.Message
            End Try
        End Function

        Private Sub OutputDataSetString(ByVal ds As DataSet)
            Using wst As StringWriter = New StringWriter()
                ds.WriteXml(wst, XmlWriteMode.DiffGram)

                Using st As StringReader = New StringReader(wst.ToString())

                    While lineCount < maxObjectInfoLine
                        Dim line As String = st.ReadLine()
                        If line Is Nothing Then Exit While
                        If maxObjectInfoLength > 0 AndAlso line.Length > maxObjectInfoLength Then line = line.Substring(0, maxObjectInfoLength)
                        OutputString(line)
                        If lineCount >= maxObjectInfoLine Then Return
                    End While
                End Using
            End Using
        End Sub

        Private Sub OutputDataTableString(ByVal tbl As DataTable)
            Using wst As StringWriter = New StringWriter()
                tbl.WriteXml(wst, XmlWriteMode.DiffGram)

                Using st As StringReader = New StringReader(wst.ToString())

                    While lineCount < maxObjectInfoLine
                        Dim line As String = st.ReadLine()
                        If line Is Nothing Then Exit While
                        If maxObjectInfoLength > 0 AndAlso line.Length > maxObjectInfoLength Then line = line.Substring(0, maxObjectInfoLength)
                        OutputString(line)
                        If lineCount >= maxObjectInfoLine Then Return
                    End While
                End Using
            End Using
        End Sub

        Private Sub OutputString(ByVal msg As String)
            Dim indentStr As String
            If msg Is Nothing Then Return

            If IndentSpace.Length > indentLevel Then
                indentStr = IndentSpace(indentLevel)
            Else
                indentStr = String.Format("{0," & (indentLevel * 2).ToString() & "}", "")
            End If

            If maxObjectInfoLength > 0 AndAlso msg.Length > maxObjectInfoLength Then msg = msg.Substring(0, maxObjectInfoLength)
            writer(indentStr & msg)
            lineCount += 1
        End Sub

        Shared IndentSpace As String() = {"", "  ", "    ", "      ", "        "}
    End Class
End Namespace
