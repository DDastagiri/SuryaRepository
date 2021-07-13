Imports System.Web.UI.WebControls

Namespace Toyota.eCRB.SystemFrameworks.Web.Controls
    Public Class CustomHyperLink
        Inherits CustomButton

        Protected Overrides ReadOnly Property TagKey As System.Web.UI.HtmlTextWriterTag
            Get
                Return System.Web.UI.HtmlTextWriterTag.Div
            End Get
        End Property
    End Class
End Namespace

