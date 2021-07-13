Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess

Partial Class Test_OperationTypeTest
    Inherits System.Web.UI.Page

    Protected Sub Button1_Click(sender As Object, e As System.EventArgs) Handles Button1.Click

        Dim ope As New OperationType
        Dim opelist As New List(Of Decimal)({7, 8, 9})
        Dim opeDt1 As OperationTypeDataSet.OPERATIONTYPEDataTable = ope.GetAllOperationType("44B40", opelist, "0")
        Me.GridView1.DataSource = opeDt1
        Me.GridView1.DataBind()

    End Sub

    Protected Sub Button2_Click(sender As Object, e As System.EventArgs) Handles Button2.Click

        Dim ope As New OperationType
        Dim opelist As New List(Of Decimal)({7, 8, 9})
        Dim opeDt2 As OperationTypeDataSet.OPERATIONTYPEDataTable = ope.GetAllOperationType("44B40", Nothing, "0")
        Me.GridView2.DataSource = opeDt2
        Me.GridView2.DataBind()

    End Sub

    Protected Sub Button3_Click(sender As Object, e As System.EventArgs) Handles Button3.Click

        Dim ope As New OperationType
        Dim opelist As New List(Of Decimal)({7, 8, 9})
        Dim opeDt3 As OperationTypeDataSet.OPERATIONTYPEDataTable = ope.GetAllOperationType("44B40", opelist, Nothing)
        Me.GridView3.DataSource = opeDt3
        Me.GridView3.DataBind()

    End Sub

    Protected Sub Button4_Click(sender As Object, e As System.EventArgs) Handles Button4.Click

        Dim ope As New OperationType
        Dim opelist As New List(Of Decimal)({7, 8, 9})
        Dim opeDt4 As OperationTypeDataSet.OPERATIONTYPEDataTable = ope.GetAllOperationType("44B40", Nothing, Nothing)
        Me.GridView4.DataSource = opeDt4
        Me.GridView4.DataBind()

    End Sub

    Protected Sub Button5_Click(sender As Object, e As System.EventArgs) Handles Button5.Click

        Dim ope As New OperationType
        Dim opelist As New List(Of Decimal)({7, 8, 9})
        Dim opeDt5 As OperationTypeDataSet.OPERATIONTYPEDataTable = ope.GetAllOperationType(Nothing, Nothing, Nothing)
        Me.GridView5.DataSource = opeDt5
        Me.GridView5.DataBind()

    End Sub

    Protected Sub Button6_Click(sender As Object, e As System.EventArgs) Handles Button6.Click

        Dim ope As New OperationType
        Dim opeDt6 As OperationTypeDataSet.OPERATIONTYPERow = ope.GetOperationType("44B40", 7, "0")
        Me.GridView6.DataSource = opeDt6.Table
        Me.GridView6.DataBind()

    End Sub

    Protected Sub Button7_Click(sender As Object, e As System.EventArgs) Handles Button7.Click

        Dim ope As New OperationType
        Dim opeDt7 As OperationTypeDataSet.OPERATIONTYPERow = ope.GetOperationType("44B40", Nothing, "0")
        Me.GridView7.DataSource = opeDt7.Table
        Me.GridView7.DataBind()

    End Sub

    Protected Sub Button8_Click(sender As Object, e As System.EventArgs) Handles Button8.Click

        Dim ope As New OperationType
        Dim opeDt8 As OperationTypeDataSet.OPERATIONTYPERow = ope.GetOperationType("44B40", 7, Nothing)
        Me.GridView8.DataSource = opeDt8.Table
        Me.GridView8.DataBind()

    End Sub

    Protected Sub Button9_Click(sender As Object, e As System.EventArgs) Handles Button9.Click

        Dim ope As New OperationType
        Dim opeDt9 As OperationTypeDataSet.OPERATIONTYPERow = ope.GetOperationType("44B40", Nothing, Nothing)
        Me.GridView9.DataSource = opeDt9.Table
        Me.GridView9.DataBind()

    End Sub

    Protected Sub Button10_Click(sender As Object, e As System.EventArgs) Handles Button10.Click

        Dim ope As New OperationType
        Dim opeDt10 As OperationTypeDataSet.OPERATIONTYPERow = ope.GetOperationType(Nothing, Nothing, Nothing)
        Me.GridView10.DataSource = opeDt10.Table
        Me.GridView10.DataBind()

    End Sub

End Class
