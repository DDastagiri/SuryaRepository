Imports System.Web.UI.WebControls
Imports System.Web.UI
Imports System.Web.UI.HtmlControls

<Assembly: WebResource("MultiItemSelector.js", "application/x-javascript")> 
Namespace Toyota.eCRB.SystemFrameworks.Web.Controls
    Public Class MultiItemSelector
        Inherits ListControl
        Implements IPostBackDataHandler, INamingContainer

        Private _popOver As PopOver
        Private _trigger As CustomHyperLink

        Public Overrides Property Enabled As Boolean
            Get
                EnsureChildControls()
                Return _trigger.Enabled
            End Get
            Set(value As Boolean)
                EnsureChildControls()
                _trigger.Enabled = value
            End Set
        End Property

        Public Property PlaceHolderTextWordNo As Decimal
            Get
                If ViewState("PlaceHolderWordNo") Is Nothing Then
                    Return Nothing
                Else
                    Return CDec(ViewState("PlaceHolderWordNo"))
                End If
            End Get
            Set(value As Decimal)
                ViewState("PlaceHolderWordNo") = value
            End Set
        End Property

        Public Property HeaderText As String
            Get
                EnsureChildControls()
                Return _popOver.HeaderText
            End Get
            Set(value As String)
                EnsureChildControls()
                _popOver.HeaderText = value
            End Set
        End Property

        Public Property HeaderTextWordNo As Decimal
            Get
                EnsureChildControls()
                Return _popOver.HeaderTextWordNo
            End Get
            Set(value As Decimal)
                EnsureChildControls()
                _popOver.HeaderTextWordNo = value
            End Set
        End Property

        Public Property PopOverWidth As Unit
            Get
                EnsureChildControls()
                Return _popOver.Width
            End Get
            Set(value As System.Web.UI.WebControls.Unit)
                EnsureChildControls()
                _popOver.Width = value
            End Set
        End Property
        Public Property PopOverHeight As Unit
            Get
                EnsureChildControls()
                Return _popOver.Height
            End Get
            Set(value As System.Web.UI.WebControls.Unit)
                EnsureChildControls()
                _popOver.Height = value
            End Set
        End Property
        Public Property PopOverHeaderTextWordNo As Decimal
            Get
                EnsureChildControls()
                Return _popOver.HeaderTextWordNo
            End Get
            Set(value As Decimal)
                EnsureChildControls()
                _popOver.HeaderTextWordNo = value
            End Set
        End Property

        Public Overrides ReadOnly Property Controls As System.Web.UI.ControlCollection
            Get
                EnsureChildControls()
                Return MyBase.Controls
            End Get
        End Property

        Public Overrides Property Width As System.Web.UI.WebControls.Unit
            Get
                EnsureChildControls()
                Return _trigger.Width
            End Get
            Set(value As System.Web.UI.WebControls.Unit)
                EnsureChildControls()
                _trigger.Width = value
            End Set
        End Property

        Public Overrides Property Height As System.Web.UI.WebControls.Unit
            Get
                EnsureChildControls()
                Return _trigger.Height
            End Get
            Set(value As System.Web.UI.WebControls.Unit)
                EnsureChildControls()
                _trigger.Height = value
            End Set
        End Property

        Protected Overrides Sub CreateChildControls()
            Controls.Clear()
            _trigger = New CustomHyperLink()
            Me.Controls.Add(_trigger)
            _trigger.ID = "trigger"
            _trigger.ClientIDMode = System.Web.UI.ClientIDMode.AutoID
            _trigger.CausesPostBack = False

            _popOver = New PopOver()
            Me.Controls.Add(_popOver)
            _popOver.ID = "popOver"
            _popOver.ClientIDMode = System.Web.UI.ClientIDMode.AutoID
        End Sub

        Protected Overrides Sub OnPreRender(e As System.EventArgs)
            MyBase.OnPreRender(e)

            EnsureChildControls()
            If (Me.PlaceHolderTextWordNo <> Nothing AndAlso Me.DesignMode = False) Then
                _trigger.Text = WebWordUtility.GetWord(Me.PlaceHolderTextWordNo)
            End If
 
            _popOver.Controls.Clear()
            For Each item As ListItem In Me.Items
                Dim selectedClass As String = ""
                If (item.Selected) Then
                    selectedClass = " icrop-selected"
                End If
                Dim itemFrame As New HtmlGenericControl("div")
                itemFrame.Attributes.Add("class", "icrop-MultiItemSelector-item" & selectedClass)
                itemFrame.Attributes.Add("data-value", item.Value)

                Dim textLabel As New CustomLabel()
                textLabel.Text = item.Text
                itemFrame.Controls.Add(textLabel)

                _popOver.Controls.Add(itemFrame)
            Next

            If (Me.Enabled) Then
                _popOver.TriggerClientId = _trigger.ClientID
            Else
                _popOver.TriggerClientId = ""
            End If

            Page.ClientScript.RegisterClientScriptResource(Me.GetType(), "MultiItemSelector.js")
        End Sub

        Protected Overrides Sub AddAttributesToRender(writer As System.Web.UI.HtmlTextWriter)
            MyBase.AddAttributesToRender(writer)
            writer.AddAttribute("class", "icrop-MultiItemSelector")
            writer.AddStyleAttribute("position", "relative")
        End Sub

        Protected Overrides Sub Render(writer As System.Web.UI.HtmlTextWriter)
            Me.AddAttributesToRender(writer)
            writer.RenderBeginTag("div")

            'postBackData
            Common.RenderHiddenField(writer, Me.UniqueID, Me.SelectedValue)

            _trigger.RenderControl(writer)

            _popOver.TriggerClientId = _trigger.ClientID
            _popOver.RenderControl(writer)

            writer.RenderEndTag()

        End Sub


        Public Function LoadPostData(postDataKey As String, postCollection As System.Collections.Specialized.NameValueCollection) As Boolean Implements System.Web.UI.IPostBackDataHandler.LoadPostData
            Dim selectedValue As String = postCollection(Me.UniqueID)
            If (Not Me.SelectedValue.Equals(selectedValue)) Then
                Me.SelectedValue = selectedValue
                Return True
            End If
            Return False
        End Function

        Public Sub RaisePostDataChangedEvent() Implements System.Web.UI.IPostBackDataHandler.RaisePostDataChangedEvent
            Me.OnSelectedIndexChanged(EventArgs.Empty)
        End Sub
    End Class
End Namespace

