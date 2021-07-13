''' <summary>
''' 来店通知送信IF インタフェース
''' </summary>
''' <remarks></remarks>
Public Interface IIC3090201BusinessLogic

    ''' <summary>
    ''' 来店通知送信
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="storeCode">店舗コード</param>
    ''' <param name="vehicleRegNo">車両登録No.</param>
    ''' <returns>終了コード</returns>
    ''' <remarks>門番に来店通知を送信する</remarks>
    Function SendGateNotice(ByVal dealerCode As String, ByVal storeCode As String, ByVal vehicleRegNo As String) As Integer


End Interface
