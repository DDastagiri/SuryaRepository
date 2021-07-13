Imports Toyota.eCRB.GateKeeper.GateKeeperMain.DataAccess.SC3090301DataSet

''' <summary>
''' SC3090301(ゲートキーパーメイン)
''' ビジネスロジック用インターフェース
''' コミット行うメソッドを定義します。
''' </summary>
''' <remarks></remarks>
Public Interface ISC3090301BusinessLogic

    ''' <summary>
    ''' 送信処理_新規(セールス)
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="storeCode">店舗コード</param>
    ''' <param name="visitDate">来店日時</param>
    ''' <param name="visitPersonNumber">来店人数</param>
    ''' <param name="visitMeans">来店手段</param>
    ''' <param name="account">アカウント</param>
    ''' <param name="regNum">車両登録番号</param>
    ''' <returns>メッセージID</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加
    ''' </history>
    Function SendNewSales(ByVal dealerCode As String, ByVal storeCode As String, _
                          ByVal visitDate As Date, ByVal visitPersonNumber As String, _
                          ByVal visitMeans As String, ByVal account As String, ByVal regNum As String) As Integer
    '2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 START
    'Function SendNewSales(ByVal dealerCode As String, ByVal storeCode As String, _
    '                  ByVal visitDate As Date, ByVal visitPersonNumber As String, _
    '                  ByVal visitMeans As String, ByVal account As String) As Integer
    '2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 END

    ''' <summary>
    ''' 送信処理_自社客・未取引客
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="storeCode">店舗コード</param>
    ''' <param name="visitPersonNumber">来店人数</param>
    ''' <param name="visitPurpose">来店目的</param>
    ''' <param name="visitMeans">来店手段</param>
    ''' <param name="account">アカウント</param>
    ''' <param name="userName">アカウントユーザ名</param>
    ''' <param name="unsentRow">来店通知未送信データロウ</param>
    ''' <returns>メッセージID</returns>
    ''' <remarks></remarks>
    Function SendOrgOrNewCustomer(ByVal dealerCode As String, ByVal storeCode As String, _
                                         ByVal visitPersonNumber As String, ByVal visitPurpose As String, _
                                         ByVal visitMeans As String, ByVal account As String, _
                                         ByVal userName As String, _
                                         ByVal unsentRow As SC3090301VisitUnsentDataRow) As Integer
    '2014/01/17 TMEJ 陳   TMEJ次世代サービス 工程管理機能開発 $11 START
    'Function SendOrgOrNewCustomer(ByVal dealerCode As String, ByVal storeCode As String, _
    '                                     ByVal visitPersonNumber As String, ByVal visitPurpose As String, _
    '                                     ByVal visitMeans As String, ByVal account As String, _
    '                                     ByVal unsentRow As SC3090301VisitUnsentDataRow) As Integer
    '2014/01/17 TMEJ 陳   TMEJ次世代サービス 工程管理機能開発 $11 END

    ''' <summary>
    ''' 送信処理_顧客情報なし
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="storeCode">店舗コード</param>
    ''' <param name="visitPersonNumber">来店人数</param>
    ''' <param name="visitPurpose">来店目的</param>
    ''' <param name="visitMeans">来店手段</param>
    ''' <param name="account">アカウント</param>
    ''' <param name="userName">アカウントユーザ名</param>
    ''' <param name="unsentRow">来店通知未送信データロウ</param>
    ''' <returns>メッセージID</returns>
    ''' <remarks></remarks>
    Function SendNotCustomerInfo(ByVal dealerCode As String, ByVal storeCode As String, _
                                        ByVal visitPersonNumber As String, ByVal visitPurpose As String, _
                                        ByVal visitMeans As String, ByVal account As String, _
                                        ByVal userName As String, _
                                        ByVal unsentRow As SC3090301VisitUnsentDataRow) As Integer
    '2014/01/17 TMEJ 陳   TMEJ次世代サービス 工程管理機能開発 $11 START
    'Function SendNotCustomerInfo(ByVal dealerCode As String, ByVal storeCode As String, _
    '                                    ByVal visitPersonNumber As String, ByVal visitPurpose As String, _
    '                                    ByVal visitMeans As String, ByVal account As String, _
    '                                    ByVal unsentRow As SC3090301VisitUnsentDataRow) As Integer
    '2014/01/17 TMEJ 陳   TMEJ次世代サービス 工程管理機能開発 $11 END

End Interface
