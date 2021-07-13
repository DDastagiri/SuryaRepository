'-------------------------------------------------------------------------
'IC3811501BusinessLogic.vb
'-------------------------------------------------------------------------
'機能：予約情報を取得するクラス
'補足：
'作成：2012/09/05 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.42）
'更新：2013/06/27 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発
'更新：2015/11/10 TM 皆川 (トライ店システム評価)SMBチップ検索の絞り込み方法変更 START
'更新：2017/01/06 NSK  竹中 TR-SVT-TMT-20160602-001 顧客がWelcomeボードに表示されない
'更新：
'─────────────────────────────────────

Imports System.Text
Imports System.Net
Imports System.IO
Imports System.Globalization
Imports System.Reflection
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SMBLinkage.Reservation.Api.DataAccess
Imports Toyota.eCRB.SMBLinkage.Reservation.Api.DataAccess.IC3811501DataSet
'2013/06/27 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
Imports Toyota.eCRB.CommonUtility.SMBCommonClass.Api.BizLogic
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.BizLogic

'2013/06/27 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

Public Class IC3811501BusinessLogic
    Inherits BaseBusinessComponent
    Implements IDisposable

#Region "デフォルトコンストラクタ処理"
    ''' <summary>
    ''' デフォルトコンストラクタ処理
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
    End Sub
#End Region

#Region "定数"

    ''' <summary>
    ''' 成功
    ''' </summary>
    Public Const Success As Long = 0

    ''' <summary>
    ''' DBタイムアウト
    ''' </summary>
    Public Const ErrDBTimeout = 901

    ''' <summary>
    ''' 更新失敗
    ''' </summary>
    Public Const ErrNoData As Long = 902

    ''' <summary>
    ''' 引数エラー
    ''' </summary>
    Public Const ErrArgument As Long = 903

#End Region

#Region "メイン処理"

    ''' <summary>
    ''' 予約情報を取得する
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="custCode">顧客コード</param>
    ''' <param name="vclRegNo">車両登録No</param>
    ''' <param name="vin">VIN</param>
    ''' <param name="baseDate">取得基準日</param>
    ''' <param name="isGetDmsCstFlg">自社客取得フラグ</param>
    ''' <returns>取得結果</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2013/06/27 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発
    ''' 2017/01/06 NSK  竹中 TR-SVT-TMT-20160602-001 顧客がWelcomeボードに表示されない
    ''' </history>
    Public Function GetReservationList(ByVal dealerCode As String, _
                                       ByVal branchCode As String, _
                                       ByVal custCode As String, _
                                       ByVal vclRegNo As String, _
                                       ByVal vin As String, _
                                       ByVal baseDate As String, _
                                       ByVal isGetDmsCstFlg As Boolean) As IC3811501ReservationListDataTable
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} dealerCode:{2} branchCode:{3} custCode:{4} vclRegNo:{5} vin:{6} baseDate:{7}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , dealerCode, branchCode, custCode, vclRegNo, vin, baseDate))
        '2013/06/27 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
        'Public Function GetReservationList(ByVal dealerCode As String, _
        '                                   ByVal branchCode As String, _
        '                                   ByVal custCode As String, _
        '                                   ByVal vclRegNo As String, _
        '                                   ByVal vin As String, _
        '                                   ByVal baseDate As String) As IC3811501ReservationListDataTable
        '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                , "{0}.{1} dealerCode:{2} branchCode:{3} custCode:{4} vclRegNo:{5} vin:{6} baseDate:{7}" _
        '                , Me.GetType.ToString _
        '                , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                , dealerCode, branchCode, custCode, vclRegNo, vin, baseDate))
        '2013/06/27 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

        'パラメータチェック
        '販売店コード
        If String.IsNullOrEmpty(dealerCode) Then
            'ログ出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} dealerCode = NOTHING OR EMPTY  RETURNCODE:{2} " _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , ErrArgument))
            Return Nothing
        End If
        '店舗コード
        If String.IsNullOrEmpty(branchCode) Then
            'ログ出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} branchCode = NOTHING OR EMPTY  RETURNCODE:{2} " _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , ErrArgument))
            Return Nothing
        End If
        '顧客コード
        If String.IsNullOrEmpty(Trim(custCode)) Then
            '2013/06/27 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
            'If String.IsNullOrEmpty(custCode) Then
            '2013/06/27 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END
            'ログ出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} custCode = NOTHING OR EMPTY  RETURNCODE:{2} " _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , ErrArgument))
            Return Nothing
        Else
            '販売店コードをなくす
            custCode = Trim(custCode).Replace(dealerCode + "@", "")
        End If
        '取得基準日
        If String.IsNullOrEmpty(baseDate) Then
            'ログ出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} baseDate = NOTHING OR EMPTY  RETURNCODE:{2} " _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , ErrArgument))
            Return Nothing
        End If

        '2015/11/10 TM 皆川 (トライ店システム評価)SMBチップ検索の絞り込み方法変更 START
        Using svcCommonClassBiz As New ServiceCommonClassBusinessLogic
            '2015/11/10 TM 皆川 (トライ店システム評価)SMBチップ検索の絞り込み方法変更 END

            '予約情報取得
            Dim da As New IC3811501DataSetTableAdapters.IC3811501TableAdapter
            Try
                '2013/06/27 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
                'Dim dtReservationList As IC3811501ReservationListDataTable = _
                '    da.GetReserveInfo(dealerCode, _
                '                      branchCode, _
                '                      custCode.Trim(), _
                '                      vclRegNo.Trim(), _
                '                      vin.Trim(), _
                '                      baseDate)

                '2015/11/10 TM 皆川 (トライ店システム評価)SMBチップ検索の絞り込み方法変更 START
                Dim vclRegNumSearch As String = svcCommonClassBiz.ConvertVclRegNumWord(vclRegNo.Trim())

                'Dim dtReservationList As IC3811501ReservationListDataTable = _
                '    da.GetReserveInfo(dealerCode, _
                '                      branchCode, _
                '                      Me.ReplaceCustomerId(dealerCode, custCode), _
                '                      vclRegNo.Trim(), _
                '                      vin.Trim(), _
                '                      baseDate)

                '2017/01/06 NSK  竹中 TR-SVT-TMT-20160602-001 顧客がWelcomeボードに表示されない START
                ' 引数に自社客取得フラグを追加
                Dim dtReservationList As IC3811501ReservationListDataTable = _
                    da.GetReserveInfo(dealerCode, _
                                      branchCode, _
                                      Me.ReplaceCustomerId(dealerCode, custCode), _
                                      vclRegNumSearch, _
                                      vin.Trim(), _
                                      baseDate, _
                                      isGetDmsCstFlg)
                '2017/01/06 NSK  竹中 TR-SVT-TMT-20160602-001 顧客がWelcomeボードに表示されない END

                '2015/11/10 TM 皆川 (トライ店システム評価)SMBチップ検索の絞り込み方法変更 END
                '2013/06/27 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} OUT:RETURN = {2} TABLECOUNT = {3}" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                            , Success, dtReservationList.Count.ToString(CultureInfo.CurrentCulture)))
                Return dtReservationList
            Catch ex As OracleExceptionEx When ex.Number = 1013
                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} OUT:RETURN = {2}" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                            , ErrDBTimeout))
                Return Nothing
            Finally
                da.Dispose()
            End Try

            '2015/11/10 TM 皆川 (トライ店システム評価)SMBチップ検索の絞り込み方法変更 START
        End Using
        '2015/11/10 TM 皆川 (トライ店システム評価)SMBチップ検索の絞り込み方法変更 END

    End Function

    '2013/06/27 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
    ''' <summary>
    ''' 顧客ID置換処理
    ''' </summary>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inCustomerId">顧客ID</param>
    ''' <returns>置換した顧客ID</returns>
    ''' <remarks></remarks>
    Private Function ReplaceCustomerId(ByVal inDealerCode As String, ByVal inCustomerId As String) As String
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START: inDealerCode:{2} inCustomerId:{3}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , inDealerCode, inCustomerId))

        Using daSMBCommonClass As New SMBCommonClassBusinessLogic
            Return daSMBCommonClass.ReplaceBaseCustomerCode(inDealerCode, inCustomerId)
        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Function
    '2013/06/27 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

#End Region

    ''' <summary>
    ''' IDisposable.Dispoase
    ''' </summary>
    ''' <remarks></remarks>
    Public Overloads Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
    Protected Overridable Overloads Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
        End If
    End Sub
End Class
