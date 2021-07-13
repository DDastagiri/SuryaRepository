Public Interface ISC3080202Control
    Event CreateFollow As EventHandler
    Event ChangeFollow As EventHandler
    Event ChangeSelectedSeries As EventHandler
    Sub RefreshSalesCondition()
    Sub ReflectionActivityStatus()
End Interface
