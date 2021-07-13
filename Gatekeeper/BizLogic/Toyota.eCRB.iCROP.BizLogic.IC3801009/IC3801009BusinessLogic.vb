Public Class IC3801009BusinessLogic

    Public Sub GetDelayInfoWorking(ByVal DLRCD As String,
                                        ByVal STRCD As String,
                                        ByVal SACODE As String,
                                        ByRef DELAY_TOTAL As Long,
                                        ByRef lDELAY_TIME As Long,
                                        ByRef WORKING_CHIP As Long,
                                        ByRef RES_DELIVERY_CHIP As Long)
        'Dim dt As New DataTable
        'dt.Columns.Add("DELAY_TOTAL")
        'dt.Columns.Add("DELAY_TIME")
        'dt.Columns.Add("WORKING_CHIP")
        'dt.Columns.Add("RES_DELIVERY_CHIP")

        'Dim row As DataRow = dt.NewRow()

        'row("DELAY_TOTAL") = 0

        'row("DELAY_TIME") = 0
        'row("WORKING_CHIP") = 0
        'row("RES_DELIVERY_CHIP") = 0
        'dt.Rows.Add(row)
        'Return dt

        DELAY_TOTAL = 0
        lDELAY_TIME = 0
        WORKING_CHIP = 0
        RES_DELIVERY_CHIP = 0
    End Sub
End Class
