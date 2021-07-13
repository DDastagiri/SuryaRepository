'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3010204BusinessLogic.vb
'─────────────────────────────────────
'機能： SCメイン(KPI)
'補足： 
'作成：  
'更新： 2014/02/19 TCS 高橋 受注後フォロー機能開発
'更新： 2020/01/06 TS  重松 [TMTレスポンススロー] SLT基盤への横展
'─────────────────────────────────────
Imports System.Xml
Imports System.Text
Imports System.Web
Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.Common.MainMenu.DataAccess.SC3010204
'2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 START
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
'2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 END

Public Class SC3010204BusinessLogic
    Inherits BaseBusinessComponent

#Region "定数"

    ''' <summary>
    ''' 日別集計を表示する日数(列数)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SHOW_SUMMRY_DAY As Integer = 4

    ''' <summary>
    ''' マネージャフラグ (マネージャ or アシスタント)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MNG_FLG_MNG As String = "1"

    ''' <summary>
    ''' マネージャフラグ (担当)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MNG_FLG_SC As String = "0"

    ''' <summary>
    ''' 活用指標KPI項目コード TCV活用数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SALES_KPI_ITEM_CD_002 As String = "002"

    ''' <summary>
    ''' 活用指標KPI項目コード 見積数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SALES_KPI_ITEM_CD_003 As String = "003"

    ''' <summary>
    ''' 活用指標KPI項目コード 試乗数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SALES_KPI_ITEM_CD_004 As String = "004"

    ''' <summary>
    ''' 活用指標KPI項目コード 成約数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SALES_KPI_ITEM_CD_005 As String = "005"

    ''' <summary>
    ''' 活用指標KPI項目コード N分超過接客数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SALES_KPI_ITEM_CD_007 As String = "007"

    ''' <summary>
    ''' 事業体共通設定取得用販売店コード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const COMMON_DLR_CD As String = "XXXXX"

    ''' <summary>
    ''' 共通設定取得用店舗コード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const COMMON_BRN_CD As String = "XXX"

    '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 START
    ''' <summary>
    ''' システム設定の指定パラメータ N分超過接客数限度値取得
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_SALES_DELAY_TIME As String = "SALES_DELAY_TIME"
    ''' <summary>
    ''' システム設定の指定パラメータ N日以上計画数限度値取得
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_NEXT_PLAN_INTERVAL As String = "NEXT_PLAN_INTERVAL"
    '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 END
#End Region

    ''' <summary>
    ''' KPI活用指標情報取得
    ''' </summary>
    ''' <returns>KPI活用指標情報</returns>
    ''' <remarks>KPI活用指標情報を取得し、月間指標の計算を実施する。</remarks>
    Public Function SelectProcessKpi() As DataSet

        Using ds As New DataSet   '返却用データセット
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SelectProcessKpi_Start")

            ds.Locale = CultureInfo.CurrentCulture

            Dim currentDate As Date = DateTimeFunc.Now(StaffContext.Current.DlrCD)      '当日
            currentDate = New Date(currentDate.Year, currentDate.Month, currentDate.Day)
            Dim sumKpiDailyStartDate As Date = currentDate.AddDays(-SHOW_SUMMRY_DAY)    '日別集係の開始日時
            Dim sumKpiDailyEndDate As Date = currentDate.AddSeconds(-1)                 '日別集係の終了日時

            'スタッフ組織
            Dim stfcd As String = StaffContext.Current.Account
            Dim staffOrganization As Decimal = StaffContext.Current.TeamCD
            Dim mngFlg As String = MNG_FLG_SC

            Dim dtDate As New SC3010204DataSet.SalesKpiTgtDateDataTable
            dtDate.TableName = "SalesKpiTgtDateDataTable"

            '表示日付
            For i As Integer = SHOW_SUMMRY_DAY To 1 Step -1
                Dim dr As SC3010204DataSet.SalesKpiTgtDateRow = dtDate.NewRow
                dr("TGT_DATE") = currentDate.AddDays(-i)
                dtDate.AddSalesKpiTgtDateRow(dr)
            Next
            ds.Tables.Add(dtDate)

            Dim organizations As String = String.Empty
            If IsManagerOrAssistant() Then
                'マネージャ、アシスタントの場合
                mngFlg = MNG_FLG_MNG
                '組織IDリスト取得
                organizations = GetMyTeamId(staffOrganization)
            End If

            'KPI活用指標（名称）取得
            Dim dtSelectProcessKpiItem As SC3010204DataSet.SalesKpiItemDataTable _
                = SC3010204TableAdapter.SelectProcessKpiItem
            dtSelectProcessKpiItem.TableName = "SalesKpiItemDataTable"
            ds.Tables.Add(dtSelectProcessKpiItem)

            'KPI活用指標(日別)
            Dim kpiSumDay As SC3010204DataSet.SalesKpiSummaryDataTable _
                = SC3010204TableAdapter.SelectProcessKpiValue(sumKpiDailyStartDate, sumKpiDailyEndDate, stfcd, mngFlg, organizations)
            kpiSumDay.TableName = "SalesKpiSummaryDataTable"
            ds.Tables.Add(kpiSumDay)

            '月間指標計算用情報
            Dim kpiSumMonth As SC3010204DataSet.MonthlyKpiInfoDataTable _
                = GetMonthlyProcessKpiInfo(dtSelectProcessKpiItem, organizations)
            Dim drKpiSumMonth As SC3010204DataSet.MonthlyKpiInfoRow = kpiSumMonth(0)

            'KPI活用指標(月間)
            Dim kpiMonth As New SC3010204DataSet.MonthlyKpiDataTable
            kpiMonth.TableName = "MonthlyKpiDataTable"
            ds.Tables.Add(kpiMonth)

            'TCV活用数
            If drKpiSumMonth.TcvFlg Then
                Dim drKpiMonth As SC3010204DataSet.MonthlyKpiRow = kpiMonth.NewMonthlyKpiRow
                drKpiMonth.SALES_KPI_ITEM_CD = SALES_KPI_ITEM_CD_002
                drKpiMonth.SUM_VAL = CalTcvRate(drKpiSumMonth.NegotiationNumber, drKpiSumMonth.TcvNumber)
                kpiMonth.AddMonthlyKpiRow(drKpiMonth)
            End If

            '見積数
            If drKpiSumMonth.QuotatinFlg Then
                Dim drKpiMonth As SC3010204DataSet.MonthlyKpiRow = kpiMonth.NewMonthlyKpiRow
                drKpiMonth.SALES_KPI_ITEM_CD = SALES_KPI_ITEM_CD_003
                drKpiMonth.SUM_VAL = CalQuotationRate(drKpiSumMonth.NegotiationNumber, drKpiSumMonth.QuotatinNumber)
                kpiMonth.AddMonthlyKpiRow(drKpiMonth)
            End If

            '試乗数
            If drKpiSumMonth.TestDriveFlg Then
                Dim drKpiMonth As SC3010204DataSet.MonthlyKpiRow = kpiMonth.NewMonthlyKpiRow
                drKpiMonth.SALES_KPI_ITEM_CD = SALES_KPI_ITEM_CD_004
                drKpiMonth.SUM_VAL = CalTestDriveRate(drKpiSumMonth.NegotiationNumber, drKpiSumMonth.TestDriveNumber)
                kpiMonth.AddMonthlyKpiRow(drKpiMonth)
            End If

            '成約数
            If drKpiSumMonth.BookingFlg Then
                Dim drKpiMonth As SC3010204DataSet.MonthlyKpiRow = kpiMonth.NewMonthlyKpiRow
                drKpiMonth.SALES_KPI_ITEM_CD = SALES_KPI_ITEM_CD_005
                drKpiMonth.SUM_VAL = CalBookingRate(drKpiSumMonth.NegotiationNumber, drKpiSumMonth.BookingNumber)
                kpiMonth.AddMonthlyKpiRow(drKpiMonth)
            End If

            'N分以内接客数
            If drKpiSumMonth.NormalNegotiationFlg Then
                Dim drKpiMonth As SC3010204DataSet.MonthlyKpiRow = kpiMonth.NewMonthlyKpiRow
                drKpiMonth.SALES_KPI_ITEM_CD = SALES_KPI_ITEM_CD_007
                drKpiMonth.SUM_VAL = CalNormalNegotiationRate(drKpiSumMonth.NegotiationNumber, drKpiSumMonth.B2DNegotiationNumber, drKpiSumMonth.NormalNegotiationNumber)
                kpiMonth.AddMonthlyKpiRow(drKpiMonth)
            End If

            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SelectProcessKpi_End")
            Return ds

        End Using

    End Function


#Region "組織ID取得"
    ''' <summary>
    ''' 自チーム配下の組織IDを取得する。
    ''' </summary>
    ''' <param name="orgnzId">自分が所属する組織ID</param>
    ''' <returns>自チーム配下の組織ID</returns>
    ''' <remarks>チームリーダーログイン時に利用</remarks>
    Public Function GetMyTeamId(ByVal orgnzId As Decimal) As String

        Dim dt As SC3010204DataSet.BranchSalesOrganzDataTable = Nothing
        Dim myTeamList As New List(Of Decimal)
        Dim strRet As String = orgnzId.ToString(CultureInfo.CurrentCulture)

        Try
            '同じ店舗内の全組織を取得
            dt = SC3010204TableAdapter.GetBranchSalesOrganizations()
            If dt Is Nothing OrElse dt.Rows.Count = 0 Then Exit Try

            '再起処理にて下位セールス組織を収集
            GetMyTeamId(dt, orgnzId, myTeamList)
            For Each teamId As Decimal In myTeamList
                strRet &= "," & teamId.ToString()
            Next

        Finally
            If Not dt Is Nothing Then dt.Dispose()
            myTeamList.Clear()
        End Try

        Return strRet
    End Function

    ''' <summary>
    ''' 下位セールス組織を収集
    ''' </summary>
    ''' <param name="dt">店舗内全組織データ</param>
    ''' <param name="parentOrgnzId">親組織ID</param>
    ''' <param name="myTeamList">自チームIDリスト</param>
    ''' <remarks></remarks>
    Private Sub getMyTeamId(ByRef dt As SC3010204DataSet.BranchSalesOrganzDataTable, ByVal parentOrgnzId As Decimal, ByRef myTeamList As List(Of Decimal))

        dt.DefaultView.RowFilter = "PARENT_ORGNZ_ID = " & parentOrgnzId.ToString()
        '下位組織無しのため現階層から抜ける
        If dt.DefaultView.Count = 0 Then Exit Sub

        For Each dvr As DataRowView In dt.DefaultView
            Dim dr As SC3010204DataSet.BranchSalesOrganzRow = CType(dvr.Row, SC3010204DataSet.BranchSalesOrganzRow)
            'セールス組織の場合、配下のチームとみなす
            If dr.ORGNZ_SC_FLG = "1" Then myTeamList.Add(dr.ORGNZ_ID)
            '再起処理にて下位セールス組織を収集
            GetMyTeamId(dt, dr.ORGNZ_ID, myTeamList)
        Next

    End Sub
#End Region

#Region "月間指標計算用情報取得"
    ''' <summary>
    ''' 月間指標計算用情報取得
    ''' </summary>
    ''' <param name="dtSelectProcessKpiItem">データテーブル(インプット)</param>
    ''' <param name="organizations">配下組織ID</param>
    ''' <returns>月間指標計算用情報</returns>
    ''' <remarks></remarks>
    Private Function GetMonthlyProcessKpiInfo(dtSelectProcessKpiItem As SC3010204DataSet.SalesKpiItemDataTable,
                                              organizations As String)

        Using monthlyKpiInfo As New SC3010204DataSet.MonthlyKpiInfoDataTable

            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetMonthlyProcessKpiInfo_Start")

            Dim drMonthly As SC3010204DataSet.MonthlyKpiInfoRow = monthlyKpiInfo.NewMonthlyKpiInfoRow
            drMonthly.TcvFlg = "0"
            drMonthly.QuotatinFlg = "0"
            drMonthly.TestDriveFlg = "0"
            drMonthly.BookingFlg = "0"
            drMonthly.NormalNegotiationFlg = "0"

            Dim flgSelectNegotiationNumber As Boolean = False       '商談数の月間合計数の取得有無
            Dim flgSelectTcvNumber As Boolean = False               'TCV活用数の月間合計数の取得有無
            Dim flgSelectQuotatinNumber As Boolean = False          '見積り数の月間合計数の取得有無
            Dim flgSelectTestDriveNumber As Boolean = False         '試乗数の月間合計数の取得有無
            Dim flgSelectBookingNumber As Boolean = False           '成約数の月間合計数の取得有無
            Dim flgSelectB2DNegotiationNumber As Boolean = False    '納車数の月間合計数の取得有無
            Dim flgSelectNormalNegotiationNumber As Boolean = False 'N分超過接客数の月間合計数の取得有無

            Dim negotiationNumber As Decimal = 0        '商談数の月間合計数
            Dim tcvNumber As Decimal = 0                'TCV活用数の月間合計数
            Dim quotatinNumber As Decimal = 0           '見積り数の月間合計数
            Dim testDriveNumber As Decimal = 0          '試乗数の月間合計数
            Dim bookingNumber As Decimal = 0            '成約数の月間合計数
            Dim b2DNegotiationNumber As Decimal = 0     '納車数の月間合計数
            Dim normalNegotiationNumber As Decimal = 0  'N分超過接客数の月間合計数

            For Each dr As SC3010204DataSet.SalesKpiItemRow In dtSelectProcessKpiItem
                Select Case dr.SALES_KPI_ITEM_CD
                    Case SALES_KPI_ITEM_CD_002
                        flgSelectNegotiationNumber = True
                        flgSelectTcvNumber = True
                        drMonthly.TcvFlg = "1"
                    Case (SALES_KPI_ITEM_CD_003)
                        flgSelectNegotiationNumber = True
                        flgSelectQuotatinNumber = True
                        drMonthly.QuotatinFlg = "1"
                    Case SALES_KPI_ITEM_CD_004
                        flgSelectNegotiationNumber = True
                        flgSelectTestDriveNumber = True
                        drMonthly.TestDriveFlg = "1"
                    Case SALES_KPI_ITEM_CD_005
                        flgSelectNegotiationNumber = True
                        flgSelectBookingNumber = True
                        drMonthly.BookingFlg = "1"
                    Case SALES_KPI_ITEM_CD_007
                        flgSelectNegotiationNumber = True
                        flgSelectB2DNegotiationNumber = True
                        flgSelectNormalNegotiationNumber = True
                        drMonthly.NormalNegotiationFlg = "1"
                    Case Else
                End Select
            Next

            Dim currentDate As Date = DateTimeFunc.Now(StaffContext.Current.DlrCD)  '当日
            currentDate = New Date(currentDate.Year, currentDate.Month, currentDate.Day)
            Dim previousDate As Date = currentDate.AddDays(-1)                      '前日
            Dim startDate As Date
            Dim endDate As Date

            If currentDate.Day <> 1 Then
                '当日が1日以外
                startDate = New Date(currentDate.Year, currentDate.Month, 1)    '当月初日
                endDate = currentDate.AddSeconds(-1)                            '前日の23時59分59秒
                '前日
            Else
                startDate = New Date(previousDate.Year, previousDate.Month, 1)  '前月初日
                endDate = startDate.AddMonths(1).AddDays(-1)                    '前月末日
            End If

            Dim stfcd As String = StaffContext.Current.Account
            Dim mngFlg As String = MNG_FLG_SC
            If Not String.IsNullOrEmpty(organizations) Then
                '組織が指定されている場合、マネージャフラグを設定
                mngFlg = MNG_FLG_MNG
            End If

            '商談数の月間合計数
            If flgSelectNegotiationNumber Then
                Dim dt As SC3010204DataSet.SalesKpiSummaryMonthlyDataTable _
                    = SC3010204TableAdapter.SelectNegotiationNumber(startDate, endDate, stfcd, mngFlg, organizations)
                Dim dr As SC3010204DataSet.SalesKpiSummaryMonthlyRow = dt(0)
                If Not dr.IsSUM_VALNull Then
                    negotiationNumber = dr.SUM_VAL
                End If
            End If
            'TCV活用数の月間合計数
            If flgSelectTcvNumber Then
                Dim dt As SC3010204DataSet.SalesKpiSummaryMonthlyDataTable _
                    = SC3010204TableAdapter.SelectTcvNumber(startDate, endDate, stfcd, mngFlg, organizations)
                Dim dr As SC3010204DataSet.SalesKpiSummaryMonthlyRow = dt(0)
                If Not dr.IsSUM_VALNull Then
                    tcvNumber = dr.SUM_VAL
                End If
            End If
            '見積り数の月間合計数
            If flgSelectQuotatinNumber Then
                Dim dt As SC3010204DataSet.SalesKpiSummaryMonthlyDataTable _
                    = SC3010204TableAdapter.SelectQuotatinNumber(startDate, endDate, stfcd, mngFlg, organizations)
                Dim dr As SC3010204DataSet.SalesKpiSummaryMonthlyRow = dt(0)
                If Not dr.IsSUM_VALNull Then
                    quotatinNumber = dr.SUM_VAL
                End If
            End If
            '試乗数の月間合計数
            If flgSelectTestDriveNumber Then
                Dim dt As SC3010204DataSet.SalesKpiSummaryMonthlyDataTable _
                    = SC3010204TableAdapter.SelectTestDriveNumber(startDate, endDate, stfcd, mngFlg, organizations)
                Dim dr As SC3010204DataSet.SalesKpiSummaryMonthlyRow = dt(0)
                If Not dr.IsSUM_VALNull Then
                    testDriveNumber = dr.SUM_VAL
                End If
            End If
            '成約数の月間合計数
            If flgSelectBookingNumber Then
                Dim dt As SC3010204DataSet.SalesKpiSummaryMonthlyDataTable _
                    = SC3010204TableAdapter.SelectBookingNumber(startDate, endDate, stfcd, mngFlg, organizations)
                Dim dr As SC3010204DataSet.SalesKpiSummaryMonthlyRow = dt(0)
                If Not dr.IsSUM_VALNull Then
                    bookingNumber = dr.SUM_VAL
                End If
            End If
            '納車数の月間合計数
            If flgSelectB2DNegotiationNumber Then
                Dim dt As SC3010204DataSet.SalesKpiSummaryMonthlyDataTable _
                    = SC3010204TableAdapter.SelectB2DNegotiationNumber(startDate, endDate, stfcd, mngFlg, organizations)
                Dim dr As SC3010204DataSet.SalesKpiSummaryMonthlyRow = dt(0)
                If Not dr.IsSUM_VALNull Then
                    b2DNegotiationNumber = dr.SUM_VAL
                End If
            End If
            'N分超過接客数の月間合計数
            If flgSelectNormalNegotiationNumber Then
                Dim dt As SC3010204DataSet.SalesKpiSummaryMonthlyDataTable _
                    = SC3010204TableAdapter.SelectNormalNegotiationNumber(startDate, endDate, stfcd, mngFlg, organizations)
                Dim dr As SC3010204DataSet.SalesKpiSummaryMonthlyRow = dt(0)
                If Not dr.IsSUM_VALNull Then
                    normalNegotiationNumber = dr.SUM_VAL
                End If
            End If

            drMonthly.NegotiationNumber = negotiationNumber
            drMonthly.TcvNumber = tcvNumber
            drMonthly.QuotatinNumber = quotatinNumber
            drMonthly.TestDriveNumber = testDriveNumber
            drMonthly.BookingNumber = bookingNumber
            drMonthly.B2DNegotiationNumber = b2DNegotiationNumber
            drMonthly.NormalNegotiationNumber = normalNegotiationNumber
            monthlyKpiInfo.AddMonthlyKpiInfoRow(drMonthly)

            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetMonthlyProcessKpiInfo_End")

            Return monthlyKpiInfo
        End Using

    End Function
#End Region

#Region "月間指標計算"
    ''' <summary>
    ''' 月間指標（TCV活用率）計算
    ''' </summary>
    ''' <param name="negotiationNumber">商談数の月間合計数</param>
    ''' <param name="tcvNumber">TCV活用数の月間合計数</param>
    ''' <returns>月間指標（TCV活用率）</returns>
    ''' <remarks></remarks>
    Public Function CalTcvRate(negotiationNumber As Decimal, tcvNumber As Decimal) As Decimal
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("CalTcvRate_Start")
        Dim value As Decimal = 0
        If negotiationNumber <> 0 Then
            'TCV活用率 ＝ TCV活用数の月間合計数 ÷ 商談数の月間合計数 × 100
            value = Math.Floor(tcvNumber / negotiationNumber * 100)
        End If
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("CalTcvRate_End")
        Return value
    End Function

    ''' <summary>
    ''' 月間指標（見積り率）計算
    ''' </summary>
    ''' <param name="negotiationNumber">商談数の月間合計数</param>
    ''' <param name="quotatinNumber">見積り数の月間合計数</param>
    ''' <returns>月間指標（見積り率）</returns>
    ''' <remarks></remarks>
    Public Function CalQuotationRate(negotiationNumber As Decimal, quotatinNumber As Decimal) As Decimal
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("CalQuotationRate_Start")
        Dim value As Decimal = 0
        If negotiationNumber <> 0 Then
            '見積り率 ＝ 見積り数の月間合計数 ÷ 商談数の月間合計数 × 100
            value = Math.Floor(quotatinNumber / negotiationNumber * 100)
        End If
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("CalQuotationRate_End")
        Return value
    End Function

    ''' <summary>
    ''' 月間指標（試乗率）計算
    ''' </summary>
    ''' <param name="negotiationNumber">商談数の月間合計数</param>
    ''' <param name="testDriveNumber">試乗数の月間合計数</param>
    ''' <returns>月間指標（試乗率）</returns>
    ''' <remarks></remarks>
    Public Function CalTestDriveRate(negotiationNumber As Decimal, testDriveNumber As Decimal) As Decimal
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("CalTestDriveRate_Start")
        Dim value As Decimal = 0
        If negotiationNumber <> 0 Then
            '試乗率 ＝ 試乗数の月間合計数 ÷ 商談数の月間合計数 × 100
            value = Math.Floor(testDriveNumber / negotiationNumber * 100)

        End If
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("CalTestDriveRate_End")
        Return value

    End Function

    ''' <summary>
    ''' 月間指標（成約率）計算
    ''' </summary>
    ''' <param name="negotiationNumber">商談数の月間合計数</param>
    ''' <param name="bookingNumber">成約数の月間合計数</param>
    ''' <returns>月間指標（成約率）</returns>
    ''' <remarks></remarks>
    Public Function CalBookingRate(negotiationNumber As Decimal, bookingNumber As Decimal) As Decimal
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("CalBookingRate_Start")
        Dim value As Decimal = 0
        If negotiationNumber <> 0 Then
            '成約率 ＝ 成約数の月間合計数 ÷ 商談数の月間合計数 × 100
            value = Math.Floor(bookingNumber / negotiationNumber * 100)
        End If
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("CalBookingRate_End")
        Return value

    End Function

    ''' <summary>
    ''' 月間指標（N分以内接客率）計算
    ''' </summary>
    ''' <param name="negotiationNumber">商談数の月間合計数</param>
    ''' <param name="b2DNegotiationNumber">納車数の月間合計数</param>
    ''' <param name="normalNegotiationNumber">N分超過接客数の月間合計数</param>
    ''' <returns>月間指標（N分以内接客率）</returns>
    ''' <remarks></remarks>
    Public Function CalNormalNegotiationRate(negotiationNumber As Decimal, b2DNegotiationNumber As Decimal, normalNegotiationNumber As Decimal) As Decimal
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("CalNormalNegotiationRate_Start")
        Dim value As Decimal = 0
        If negotiationNumber + b2DNegotiationNumber > 0 Then
            'N分以内接客率 ＝ （商談数の月間合計数 ＋ 納車数の月間合計数 － N分超過接客数の月間合計数） ÷ （商談数の月間合計数 ＋ 納車数の月間合計数） × 100
            value = Math.Floor(((negotiationNumber + b2DNegotiationNumber - normalNegotiationNumber) / (negotiationNumber + b2DNegotiationNumber)) * 100)
        End If
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("CalNormalNegotiationRate_End")
        Return value

    End Function
#End Region

#Region "権限判定"
    ''' <summary>
    ''' マネージャまたはアシスタントかを判定する
    ''' </summary>
    ''' <returns>True:マネージャまたはアシスタント権限あり / False:マネージャまたはアシスタント権限なし</returns>
    ''' <remarks></remarks>
    Private Function IsManagerOrAssistant() As Boolean
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("IsManagerOrAssistant_Start")
        Dim ret As Boolean = False
        Dim staff As StaffContext = StaffContext.Current

        Dim isSalesStaff As Boolean = Operation.SSF.Equals(staff.OpeCD)
        If (isSalesStaff And Not staff.TeamLeader) Then
            '担当(権限コード=SSFかつ、リーダーではない)の場合
            ret = False
        Else
            'マネージャ
            ret = True
        End If

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("IsManagerOrAssistant_End")
        Return ret

    End Function
#End Region

#Region "V4 システム設定値取得"

    ''' <summary>
    ''' N分超過接客数限度値取得
    ''' </summary>
    ''' <returns>N分超過接客数限度値</returns>
    ''' <remarks></remarks>
    Public Function GetWaitOverMin() As String
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetWaitOverMin_Start")

        Dim staff As StaffContext = StaffContext.Current
        Dim dlrcd As String = staff.DlrCD
        Dim brncd As String = staff.BrnCD

        '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 START
        Dim setVal As String
        Dim systemBiz As New SystemSettingDlr

        '①販売店≠'XXXXX'、店舗≠'XXX'（販売店コード・店舗コード該当）
        '②①実行でデータがなければ販売店≠'XXXXX'、店舗＝'XXX'販売店（販売店コードのみ該当）
        '③①②実行でデータがなければ販売店＝'XXXXX'、店舗＝'XXX'（販売店コード・店舗コードいずれも該当なし(デフォルト値)  
        Dim drSettingDlr As SystemSettingDlrDataSet.TB_M_SYSTEM_SETTING_DLRRow = systemBiz.GetEnvSetting(dlrcd, brncd, C_SALES_DELAY_TIME)

        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetWaitOverMin_End")

        If (drSettingDlr Is Nothing) Then
            setVal = String.Empty
        Else
            setVal = drSettingDlr.SETTING_VAL
        End If

        Return setVal
        '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 END
    End Function

    ''' <summary>
    ''' N日以上計画数限度値取得
    ''' </summary>
    ''' <returns>N日以上計画数限度値</returns>
    ''' <remarks></remarks>
    Public Function GetPlanOverDays() As String
        '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 START
        Dim systemBiz As New SystemSetting
        Dim sysDt As New SystemSettingDataSet.TB_M_SYSTEM_SETTINGDataTable
        Dim setVal As String

        Dim dataRow As SystemSettingDataSet.TB_M_SYSTEM_SETTINGRow = sysDt.NewTB_M_SYSTEM_SETTINGRow()
        dataRow = systemBiz.GetSystemSetting(C_NEXT_PLAN_INTERVAL)

        If (dataRow Is Nothing) Then
            setVal = String.Empty
        Else
            setVal = dataRow.SETTING_VAL
        End If

        Return setVal
        '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 END
    End Function
#End Region

End Class
