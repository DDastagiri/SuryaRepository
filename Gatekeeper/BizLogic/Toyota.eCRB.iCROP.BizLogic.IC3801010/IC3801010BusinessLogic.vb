''' <summary>
''' 遅れ作業（納車準備）
''' </summary>
''' <remarks></remarks>
Public Class IC3801010BusinessLogic
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="StrCode">店舗コード</param>
    ''' <param name="SaCode">SA担当者コード</param>
    ''' <param name="abnormalLt">納車準備エリアの異常表示の標準時間(分)</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Sub GetDelayInfoPreDelivery(ByVal dealerCode As String _
                                          , ByVal strCode As String _
                                          , ByVal saCode As String _
                                          , ByVal abnormalLT As Long,
                                            ByRef DELAY_TOTAL As Long,
                                            ByRef DELAY_TIME As Long,
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
        DELAY_TIME = 0
        WORKING_CHIP = 0
        RES_DELIVERY_CHIP = 0


    End Sub
End Class
