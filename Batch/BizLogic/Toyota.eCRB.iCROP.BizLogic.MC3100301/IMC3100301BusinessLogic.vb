
''' <summary>
''' 来店実績データ退避バッチ インタフェース
''' </summary>
''' <remarks></remarks>
Public Interface IMC3100301BusinessLogic


    ''' <summary>
    '''  来店車両実績処理
    ''' </summary>
    ''' <param name="delDate">過去データと判断する日付</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Function VisitVehicle(ByVal delDate As Date) As Integer

    ''' <summary>
    ''' セールス来店実績移行
    ''' </summary>
    ''' <param name="delDate">過去データと判断する日付</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Function VisitSales(ByVal delDate As Date) As Integer

    ''' <summary>
    ''' 対応依頼通知処理
    ''' </summary>
    ''' <param name="delDate">過去データと判断する日付</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Function VisitDealNotice(ByVal delDate As Date) As Integer

End Interface
