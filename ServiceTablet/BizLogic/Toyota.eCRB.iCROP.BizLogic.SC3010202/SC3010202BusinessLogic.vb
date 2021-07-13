Imports System.Xml
Imports System.Text
Imports System.Web
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.iCROP.DataAccess.SC3010202.SC3010202DataSet
Imports Toyota.eCRB.iCROP.DataAccess.SC3010202.SC3010202DataSetTableAdapters
Imports System.Globalization

Public Class SC3010202BusinessLogic
    Inherits BaseBusinessComponent

#Region "定数"

    ''' <summary>
    ''' アクションコード（商談）
    ''' </summary>
    Private Const ACTIONCODE_QUOTATION As String = "A23"

    ''' <summary>
    ''' アクションコード（試乗）
    ''' </summary>
    Private Const ACTIONCODE_TESTDRIVE As String = "A26"

    ''' <summary>
    ''' アクションコード（査定）
    ''' </summary>
    Private Const ACTIONCODE_EVALUATION As String = "A30"

    ''' <summary>
    ''' CR活動履歴（受付）
    ''' </summary>
    Private Const CRACTRESULT_COLD As Integer = 7

    ''' <summary>
    ''' CR活動履歴（Prospect）
    ''' </summary>
    Private Const CRACTRESULT_WARM As Integer = 2

    ''' <summary>
    ''' CR活動履歴（Hot）
    ''' </summary>
    Private Const CRACTRESULT_HOT As Integer = 1

    ''' <summary>
    ''' CR活動履歴（Success）
    ''' </summary>
    Private Const CRACTRESULT_SUCCESS As Integer = 3

#End Region

#Region " バッチ動作時間取得 "
    ''' <summary>
    ''' MC30101バッチの稼動時間を取得する
    ''' </summary>
    ''' <returns>MC30101バッチ稼動終了時間</returns>
    ''' <remarks></remarks>
    Public Function GetBatchStartTime() As Date
        Logger.Debug("GetBacthStartTime Start")

        Dim startTime As Date
        Using da As New SC3010202TableAdapter
            'スタート時間を取得
            startTime = da.GetStarBatchTime()
        End Using

        Logger.Debug("GetBacthStartTime End : " + startTime.ToShortTimeString)

        '処理結果返却
        Return startTime
    End Function

#End Region

#Region " 目標情報取得 "
    ''' <summary>
    ''' ログインユーザまたは店舗の営業目標情報を取得する。
    ''' </summary>
    ''' <param name="staffInfo">スタッフ情報</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Public Function GetTargetInfo(ByVal staffInfo As StaffContext) As SC3010202TargetDataTable

        Logger.Debug("GetTargetInfo Start")
        Dim dt As SC3010202TargetDataTable

        'ログインしてい以内場合は処理を行わない
        If staffInfo Is Nothing Then
            Return Nothing
        End If

        Using da As New SC3010202TableAdapter
            '現在日付から年月を取り出す
            Dim culture As CultureInfo = New CultureInfo("en")
            Dim Now As Date = DateTimeFunc.Now(staffInfo.DlrCd)
            Dim YearMonthString As String = String.Format(culture, "{0:0000}{1:00}", Now.Year, Now.Month)

            '基盤からログイン者情報を取得する
            Dim staffOparationCode As Operation = staffInfo.OpeCd

            'ログイン者権限を判断
            'ブランチマネージャー(6)またはセールスマネージャー(10)の場合は
            '店舗単位での目標を取得する。
            If staffOparationCode.Equals(Operation.BM) Or _
                staffOparationCode.Equals(Operation.SSM) Then

                '検索処理(店舗単位)
                dt = da.GetTargetInfoOfBranch(staffInfo.DlrCd, staffInfo.BrnCd, YearMonthString)
            Else
                '検索処理(ユーザ単位)
                dt = da.GetTargetInfo(staffInfo.DlrCd, staffInfo.BrnCd, YearMonthString, staffInfo.Account)
            End If

        End Using

        Logger.Debug("GetTargetInfo End")
        '処理結果返却
        Return dt

    End Function


#End Region

#Region "実績情報取得"

    ''' <summary>
    ''' ログインユーザまたは店舗の営業実績情報を取得する。
    ''' </summary>
    ''' <param name="staffInfo">スタッフ情報</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Public Function GetResultInfo(ByVal staffInfo As StaffContext) As SC3010202ResultDataTable

        Logger.Debug("GetResultInfo Start")

        'ログインしていない場合は処理を行わない
        If staffInfo Is Nothing Then
            Return Nothing
        End If

        Using da As New SC3010202TableAdapter
            '現在日付から年月を取り出す
            Dim culture As CultureInfo = New CultureInfo("en")
            Dim Now As Date = DateTimeFunc.Now(staffInfo.DlrCd)
            Dim YearMonthString As String = String.Format(culture, "{0:0000}{1:00}", Now.Year, Now.Month)

            '基盤からログイン者情報を取得する
            Dim staffOparationCode As Operation = staffInfo.OpeCd

            'ログイン者権限を判断
            'ブランチマネージャー(6)またはセールスマネージャー(10)の場合は
            '店舗単位での目標を取得する。
            Dim returnDataTable As SC3010202ResultDataTable
            If staffOparationCode.Equals(Operation.BM) Or _
                staffOparationCode.Equals(Operation.SSM) Then
                '検索処理(店舗単位)
                returnDataTable = GetResultInfoOfBranch(staffInfo, da, YearMonthString)
            Else
                '検索処理(ユーザ単位)
                returnDataTable = GetResultInfoOfUser(staffInfo, da, YearMonthString)
            End If

            Logger.Debug("GetResultInfo End")
            Return returnDataTable
        End Using

    End Function

    ''' <summary>
    ''' ログインユーザの営業実績情報を取得する。
    ''' </summary>
    ''' <param name="staffInfo">スタッフ情報</param>
    ''' <param name="TableAdapter">テーブルアダプター</param>
    ''' <param name="YearMonthString">対象年月</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Private Function GetResultInfoOfBranch(ByVal staffInfo As StaffContext, ByVal TableAdapter As SC3010202TableAdapter, ByVal YearMonthString As String) As SC3010202ResultDataTable
        Logger.Debug("GetResultInfoOfBranch Start")
        Using dt As SC3010202ResultDataTable = New SC3010202ResultDataTable

            '検索処理(店舗単位)
            Dim dr As SC3010202ResultRow = dt.NewSC3010202ResultRow()

            '来店実績情報を取得
            Dim walkInDataTable As SC3010202ResultWalkInDataTable
            walkInDataTable = TableAdapter.GetResultWalkInOfBranch(staffInfo.DlrCd, staffInfo.BrnCd, YearMonthString)
            If Not IsNothing(walkInDataTable) Then
                Dim walkInDataRow As SC3010202ResultWalkInRow = walkInDataTable.Rows(0)
                dr.WALKIN = walkInDataRow.WALKIN
            End If


            'FLLWBOX履歴情報を取得(見積、試乗、査定)
            Dim crhisDataTable As SC3010202ResultCRHISDataTable
            crhisDataTable = TableAdapter.GetResultHistoryOfBranch(staffInfo.DlrCd, staffInfo.BrnCd, YearMonthString)

            If Not IsNothing(crhisDataTable) Then
                For Each crhisDataRow As SC3010202ResultCRHISRow In crhisDataTable.Rows
                    Select Case crhisDataRow.ACTIONCD
                        Case ACTIONCODE_EVALUATION
                            '商談情報を設定
                            dr.EVALUATION = crhisDataRow.CNT
                        Case ACTIONCODE_TESTDRIVE
                            '試乗情報を設定
                            dr.TESTDRIVE = crhisDataRow.CNT
                        Case ACTIONCODE_QUOTATION
                            '査定情報を設定
                            dr.QUOTATION = crhisDataRow.CNT
                    End Select
                Next
            End If



            'FLLWUPBOX集計情報を取得
            Dim fllwupBoxTallyDataTable As SC3010202ResultTallyDataTable
            fllwupBoxTallyDataTable = TableAdapter.GetResultFollowUpBoxTallyOfBranch(staffInfo.DlrCd, staffInfo.BrnCd, YearMonthString)
            If Not IsNothing(fllwupBoxTallyDataTable) Then
                For Each fllwupBoxTallyDataRow As SC3010202ResultTallyRow In fllwupBoxTallyDataTable.Rows
                    Select Case fllwupBoxTallyDataRow.CRACTRESULT
                        Case CRACTRESULT_COLD
                            '受付情報を設定
                            dr.COLD = fllwupBoxTallyDataRow.CNT
                        Case CRACTRESULT_WARM
                            '見込情報を設定
                            dr.WARM = fllwupBoxTallyDataRow.CNT
                        Case CRACTRESULT_HOT
                            'ホット情報を設定
                            dr.HOT = fllwupBoxTallyDataRow.CNT
                        Case CRACTRESULT_SUCCESS
                            '受注情報を設定
                            dr.ORDERS = fllwupBoxTallyDataRow.CNT
                    End Select
                Next
            End If


            '納車実績情報を取得
            Dim deliveryCount As Integer
            deliveryCount = TableAdapter.GetResultDeliveryOfBranch(staffInfo.DlrCd, staffInfo.BrnCd, YearMonthString)
            dr.DELIVERY = deliveryCount


            '販売実績情報を取得
            Dim salesCount As Integer
            salesCount = TableAdapter.GetResultSalesOfBranch(staffInfo.DlrCd, staffInfo.BrnCd, YearMonthString)
            dr.SALES = salesCount


            '結果行をテーブルに反映
            dt.Rows.Add(dr)

            Logger.Debug("GetResultInfoOfBranch End")
            Return dt
        End Using

    End Function


    ''' <summary>
    ''' ログインユーザの営業実績情報を取得する。
    ''' </summary>
    ''' <param name="staffInfo">スタッフ情報</param>
    ''' <param name="TableAdapter">テーブルアダプター</param>
    ''' <param name="YearMonthString">対象年月</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Private Function GetResultInfoOfUser(ByVal staffInfo As StaffContext, ByVal TableAdapter As SC3010202TableAdapter, ByVal YearMonthString As String) As SC3010202ResultDataTable
        Logger.Debug("GetResultInfoOfUser Start")

        Using dt As SC3010202ResultDataTable = New SC3010202ResultDataTable

            '検索処理(ユーザ単位)
            Dim dr As SC3010202ResultRow = dt.NewSC3010202ResultRow()

            '来店実績情報を取得
            Dim walkInDataTable As SC3010202ResultWalkInDataTable
            walkInDataTable = TableAdapter.GetResultWalkIn(staffInfo.DlrCd, staffInfo.BrnCd, YearMonthString, staffInfo.Account)
            If Not IsNothing(walkInDataTable) Then
                Dim walkInDataRow As SC3010202ResultWalkInRow = walkInDataTable.Rows(0)
                dr.WALKIN = walkInDataRow.WALKIN
            End If



            'FLLWBOX履歴情報を取得(見積、試乗、査定)
            Dim crhisDataTable As SC3010202ResultCRHISDataTable
            crhisDataTable = TableAdapter.GetResultHistory(staffInfo.DlrCd, YearMonthString, staffInfo.Account)

            If Not IsNothing(crhisDataTable) Then
                For Each crhisDataRow As SC3010202ResultCRHISRow In crhisDataTable.Rows
                    Select Case crhisDataRow.ACTIONCD
                        Case ACTIONCODE_EVALUATION
                            '商談情報を設定
                            dr.EVALUATION = crhisDataRow.CNT
                        Case ACTIONCODE_TESTDRIVE
                            '試乗情報を設定
                            dr.TESTDRIVE = crhisDataRow.CNT
                        Case ACTIONCODE_QUOTATION
                            '査定情報を設定
                            dr.QUOTATION = crhisDataRow.CNT
                    End Select
                Next
            End If



            'FLLWUPBOX集計情報を取得
            Dim fllwupBoxTallyDataTable As SC3010202ResultTallyDataTable
            fllwupBoxTallyDataTable = TableAdapter.GetResultFollowUpBoxTally(staffInfo.DlrCd, staffInfo.BrnCd, staffInfo.Account, YearMonthString)
            If Not IsNothing(fllwupBoxTallyDataTable) Then
                For Each fllwupBoxTallyDataRow As SC3010202ResultTallyRow In fllwupBoxTallyDataTable.Rows
                    Select Case fllwupBoxTallyDataRow.CRACTRESULT
                        Case CRACTRESULT_COLD
                            '受付情報を設定
                            dr.COLD = fllwupBoxTallyDataRow.CNT
                        Case CRACTRESULT_WARM
                            '見込情報を設定
                            dr.WARM = fllwupBoxTallyDataRow.CNT
                        Case CRACTRESULT_HOT
                            'ホット情報を設定
                            dr.HOT = fllwupBoxTallyDataRow.CNT
                        Case CRACTRESULT_SUCCESS
                            '受注情報を設定
                            dr.ORDERS = fllwupBoxTallyDataRow.CNT
                    End Select
                Next
            End If



            '納車実績情報を取得
            Dim deliveryCount As Integer
            deliveryCount = TableAdapter.GetResultDelivery(staffInfo.DlrCd, YearMonthString, staffInfo.Account)
            dr.DELIVERY = deliveryCount


            '販売実績情報を取得
            Dim salesCount As Integer
            salesCount = TableAdapter.GetResultSales(staffInfo.DlrCd, YearMonthString, staffInfo.Account)
            dr.SALES = salesCount

            '結果行をテーブルに反映
            dt.Rows.Add(dr)

            Logger.Debug("GetResultInfoOfUser End")
            Return dt
        End Using

    End Function
#End Region
End Class
