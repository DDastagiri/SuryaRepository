'-------------------------------------------------------------------------
'SMBCommonSAChangeClassBusinessLogic.vb
'-------------------------------------------------------------------------
'機能：共通関数API
'補足：
'作成：2012/05/11 KN 河原
'更新：2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発
'更新：2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発
'更新：
'─────────────────────────────────────
Imports System.Text
Imports System.Net
Imports System.IO
Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.CommonUtility.SMBCommonClass.Api.DataAccess
'2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
'Imports Toyota.eCRB.iCROP.BizLogic.IC3801401
'2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

Partial Class SMBCommonClassBusinessLogic

#Region "Publicメソッド"

    ''' <summary>
    ''' 担当SA変更
    ''' </summary>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inStoreCode">店舗コード</param>
    ''' <param name="inReserveId">管理予約ID</param>
    ''' <param name="inOrderNo">整備NO</param>
    ''' <param name="inSACode">変更後担当SA</param>
    ''' <param name="isReserveType">事前準備フラグ</param>
    ''' <param name="inInStockTime">入庫日時</param>
    ''' <param name="inAccount">更新者</param>
    ''' <param name="inPresentTime">現在日時</param>
    ''' <param name="inSystem">プログラムID</param>
    ''' <returns>登録結果</returns>
    ''' <history>2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発</history>
    ''' <remarks></remarks>
    Public Function ChangeSACode(ByVal inDealerCode As String, _
                                 ByVal inStoreCode As String, _
                                 ByVal inReserveId As Decimal, _
                                 ByVal inOrderNo As String, _
                                 ByVal inSACode As String, _
                                 ByVal isReserveType As String, _
                                 ByVal inInStockTime As DateTime, _
                                 ByVal inAccount As String, _
                                 ByVal inPresentTime As DateTime, _
                                 ByVal inSystem As String) As Long
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} P1:{2} P2:{3} P3:{4} P4:{5} P5:{6} P6:{7} P7:{8} P8:{9} P9:{10} P10:{11}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , inDealerCode, inStoreCode, inReserveId, inOrderNo, inSACode _
                    , isReserveType, inInStockTime, inPresentTime, inAccount, inSystem))
        '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
        'Public Function ChangeSACode(ByVal inDealerCode As String, _
        '                             ByVal inStoreCode As String, _
        '                             ByVal inReserveId As Long, _
        '                             ByVal inOrderNo As String, _
        '                             ByVal inSACode As String, _
        '                             ByVal isReserveType As String, _
        '                             ByVal inInStockTime As DateTime, _
        '                             ByVal inAccount As String, _
        '                             ByVal inPresentTime As DateTime) As Long
        '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                , "{0}.{1} P1:{2} P2:{3} P3:{4} P4:{5} P5:{6} P6:{7} P7:{8} P8:{9} P9:{10}" _
        '                , Me.GetType.ToString _
        '                , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                , inDealerCode, inStoreCode, inReserveId, inOrderNo, inSACode, isReserveType, inInStockTime, inPresentTime, inAccount))
        '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

        Dim dataSet As SMBCommonClassDataSetTableAdapters.SMBCommonClassTableAdapter = Nothing
        Try
            Dim returnCode As Long '返却コード

            '管理予約IDの有無
            If Not inReserveId = NoReserveId Then
                dataSet = New SMBCommonClassDataSetTableAdapters.SMBCommonClassTableAdapter
                '更新処理
                '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
                'dataSet.UpdateReserveSACode(inDealerCode, _
                '                            inStoreCode, _
                '                            inReserveId, _
                '                            inSACode, _
                '                            inAccount, _
                '                            inPresentTime)
                dataSet.UpdateReserveSACode(inDealerCode, _
                                            inStoreCode, _
                                            inReserveId, _
                                            inSACode, _
                                            inAccount, _
                                            inPresentTime, _
                                            inSystem)
                '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END
            End If

            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
            '整備受注NOの有無
            'If Not String.IsNullOrEmpty(inOrderNo) Then
            '    '@マーク以降を削除する
            '    Dim saCodeList() As String = inSACode.Split(CType("@", Char))
            '    Dim saCode As String = saCodeList(0)
            '    'BMTS
            '    Dim ic3801401Biz As New IC3801401BusinessLogic
            '    'IF用ログ
            '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
            '                              , "CALL IF:IC3801401BusinessLogic.ChangPICSA IN:inDealerCode={0}, inOrderNo={1}, saCode={2}, inReserveId={3}, isRezFlag={4}, inInStockTime={5}" _
            '                              , inDealerCode _
            '                              , inOrderNo _
            '                              , saCode _
            '                              , inReserveId _
            '                              , isReserveType _
            '                              , inInStockTime))
            '    returnCode = ic3801401Biz.ChangPICSA(inDealerCode, inOrderNo, saCode, isReserveType, inInStockTime)
            'End If
            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:RETURN = {2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , returnCode))

            Return returnCode
            'DBタイムアウト
        Catch ex As OracleExceptionEx When ex.Number = 1013
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:RETURN = {2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , ReturnCode.ErrDBTimeout))
            Return ReturnCode.ErrDBTimeout
        Finally
            If dataSet IsNot Nothing Then dataSet.Dispose()
        End Try
    End Function

#End Region

End Class
