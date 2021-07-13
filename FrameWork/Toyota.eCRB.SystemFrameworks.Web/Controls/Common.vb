Imports System.Globalization
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.Script.Serialization
Imports System.Text
Imports System.Web

<Assembly: WebResource("Common.js", "application/x-javascript")> 
Namespace Toyota.eCRB.SystemFrameworks.Web.Controls
    Class Common

        Private Sub New()

        End Sub

        Friend Shared Sub RenderHiddenField(ByVal writer As System.Web.UI.HtmlTextWriter, ByVal name As String, ByVal defaultValue As String)
            writer.AddAttribute("class", "postBackData")
            writer.AddAttribute("type", "hidden")
            writer.AddAttribute("name", name)
            writer.AddAttribute("value", defaultValue)
            writer.RenderBeginTag("input")
            writer.RenderEndTag()
        End Sub

        Friend Shared Function SerializeToJSON(ByVal source As Dictionary(Of String, Object)) As String
            Dim json As New StringBuilder()
            json.Append("{ ")
            Dim firstElement As Boolean = True
            For Each name As String In source.Keys
                If (firstElement) Then
                    firstElement = False
                Else
                    json.Append(", ")
                End If
                If (TypeOf source(name) Is String) Then
                    If (name.IndexOf("@", StringComparison.OrdinalIgnoreCase) = 0) Then
                        json.Append(String.Format(CultureInfo.InvariantCulture, """{0}"": {1}", HttpUtility.JavaScriptStringEncode(name.Substring(1)), source(name)))
                    Else
                        json.Append(String.Format(CultureInfo.InvariantCulture, """{0}"": ""{1}""", HttpUtility.JavaScriptStringEncode(name), HttpUtility.JavaScriptStringEncode(CStr(source(name)))))
                    End If
                Else
                    json.Append(String.Format(CultureInfo.InvariantCulture, """{0}"": {1}", HttpUtility.JavaScriptStringEncode(name.Substring(1)), source(name).ToString()))
                End If
            Next
            json.Append(" }")
            Return json.ToString()
        End Function


        Friend Shared Function GetJqueryPluginBinding(ByVal clientId As String, ByVal pluginName As String, ByVal options As Dictionary(Of String, String), ByVal enabled As Boolean) As String
            Dim jsonOptions As String = ""
            If (options IsNot Nothing) Then
                Dim normalizedOptions As New Dictionary(Of String, Object)
                For Each key As String In options.Keys
                    normalizedOptions.Add(key, options(key))
                Next
                jsonOptions = SerializeToJSON(normalizedOptions)
            End If
            If (enabled) Then
                Return String.Format(CultureInfo.InvariantCulture, "$(function() {{ $('#{0}').{1}({2}); }});", clientId, pluginName, jsonOptions)
            Else
                Return String.Format(CultureInfo.InvariantCulture, "$(function() {{ $('#{0}').{1}({2}); $('#{0}').{1}('disabled', true); }});", clientId, pluginName, jsonOptions)
            End If
        End Function
    End Class
End Namespace

