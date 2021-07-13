'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3010202BusinessLogic.vb
'──────────────────────────────────
'機能： ダッシュボード
'補足： 
'作成： 
'更新： 2013/05/27 TMEJ m.asano 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 $01
'更新： 2014/05/30 TMEJ y.gotoh 受注後フォロー機能開発 $02
'更新： 2015/01/16 TMEJ y.gotoh 組織IDの型変更 $03
'──────────────────────────────────

Imports System.Xml
Imports System.Text
Imports System.Web
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.iCROP.DataAccess.SC3010202.SC3010202DataSet
Imports Toyota.eCRB.iCROP.DataAccess.SC3010202.SC3010202DataSetTableAdapters
Imports System.Globalization

' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証 START
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess.SystemSettingDataSet
' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証 END

Public Class SC3010202BusinessLogic
    Inherits BaseBusinessComponent

#Region "定数"

    '$02 受注後フォロー機能開発 START
    ''' <summary>
    ''' 実施商談分類（試乗）
    ''' </summary>
    Private Const RSLT_SALES_CAT_TESTDRIVE As String = "4"

    ''' <summary>
    ''' 実施商談分類（査定）
    ''' </summary>
    Private Const RSLT_SALES_CAT_EVALUATION As String = "7"
    '$02 受注後フォロー機能開発 END

    ''' <summary>
    ''' システム設定（納車活動を示す受注後活動コード）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SYSTEMSETTING_AFTER_ODR_ACT_CD_DELI As String = "AFTER_ODR_ACT_CD_DELI"
#End Region

#Region " バッチ動作時間取得 "
    ''' <summary>
    ''' MC3C10102バッチの稼動時間を取得する
    ''' </summary>
    ''' <param name="dealerCode">販売点コード</param>
    ''' <returns>MC3C10102バッチ稼動終了時間</returns>
    ''' <remarks></remarks>
    Public Function GetBatchStartTime(ByVal dealerCode As String) As Date
        Logger.Info("GetBacthStartTime Start")

        Dim startTime As Date
        Using da As New SC3010202TableAdapter
            'スタート時間を取得
            startTime = da.GetStarBatchTime(dealerCode)
        End Using

        Logger.Info("GetBacthStartTime End : Ret[" + startTime.ToShortTimeString & "]")

        '処理結果返却
        Return startTime
    End Function

#End Region

#Region " 目標情報取得 "
    ''' <summary>
    ''' ログインユーザまたはチームまたは店舗の営業目標情報を取得する。
    ''' </summary>
    ''' <param name="staffInfo">スタッフ情報</param>
    ''' <param name="orgnzIdList">組織IDリスト</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Public Function GetTargetInfo(ByVal staffInfo As StaffContext, ByVal orgnzIdList As List(Of String)) As SC3010202TargetDataTable

        Logger.Info("GetTargetInfo Start Param[staffInfo, orgnzIdList=" & String.Join(",", orgnzIdList) & "]")
        Dim dt As SC3010202TargetDataTable

        'ログインしてい以内場合は処理を行わない
        If staffInfo Is Nothing Then
            Return Nothing
        End If

        Using da As New SC3010202TableAdapter
            '現在日付から年月を取り出す
            Dim culture As CultureInfo = New CultureInfo("en")
            Dim Now As Date = DateTimeFunc.Now(staffInfo.DlrCD)
            Dim YearMonthString As String = String.Format(culture, "{0:0000}{1:00}", Now.Year, Now.Month)

            '基盤からログイン者情報を取得する
            Dim staffOparationCode As Operation = staffInfo.OpeCD
            Dim IsTeamLeader As Boolean = staffInfo.TeamLeader

            'ログイン者権限を判断
            'ブランチマネージャー(6)またはセールスマネージャー(10)の場合は店舗単位、
            ''チームリーダー(8)かつリーダーフラグが"1"の場合は、チーム単位での目標を取得する。
            If staffOparationCode.Equals(Operation.BM) Or _
                staffOparationCode.Equals(Operation.SSM) Then

                '検索処理(店舗単位)
                dt = da.GetTargetInfoOfBranch(staffInfo.DlrCD, staffInfo.BrnCD, YearMonthString)

                '$02 受注後フォロー機能開発 START
            ElseIf staffOparationCode.Equals(Operation.SL) And IsTeamLeader Then
                '検索処理(チーム単位)
                dt = da.GetTargetInfoOfTeam(staffInfo.DlrCD, staffInfo.BrnCD, YearMonthString, orgnzIdList)
                '$02 受注後フォロー機能開発 END
            Else
                '検索処理(ユーザ単位)
                dt = da.GetTargetInfo(staffInfo.DlrCD, staffInfo.BrnCD, YearMonthString, staffInfo.Account)
            End If

        End Using
        Logger.Info("GetTargetInfo End Ret[SC3010202TargetDataTable[Count =" & dt.Count & "}]")

        '処理結果返却
        Return dt

    End Function


#End Region

#Region "実績情報取得"

    ''' <summary>
    ''' ログインユーザまたはチームまたは店舗の営業実績情報を取得する。
    ''' </summary>
    ''' <param name="staffInfo">スタッフ情報</param>
    ''' <param name="orgnzIdList">組織IDリスト</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Public Function GetResultInfo(ByVal staffInfo As StaffContext, ByVal orgnzIdList As List(Of String)) As SC3010202ResultDataTable

        Logger.Info("GetResultInfo Start Param[staffInfo, orgnzIdList=" & String.Join(",", orgnzIdList) & "]")

        'ログインしていない場合は処理を行わない
        If staffInfo Is Nothing Then
            Return Nothing
        End If

        Using da As New SC3010202TableAdapter
            '現在日付から年月を取り出す
            Dim culture As CultureInfo = New CultureInfo("en")
            Dim Now As Date = DateTimeFunc.Now(staffInfo.DlrCD)
            Dim YearMonthString As String = String.Format(culture, "{0:0000}{1:00}", Now.Year, Now.Month)

            '基盤からログイン者情報を取得する
            Dim staffOparationCode As Operation = staffInfo.OpeCD
            Dim IsTeamLeader As Boolean = staffInfo.TeamLeader

            'ログイン者権限を判断
            'ブランチマネージャー(6)またはセールスマネージャー(10)の場合は
            '店舗単位での目標を取得する。
            Dim returnDataTable As SC3010202ResultDataTable
            If staffOparationCode.Equals(Operation.BM) Or _
                staffOparationCode.Equals(Operation.SSM) Then
                '検索処理(店舗単位)
                returnDataTable = GetResultInfoOfBranch(staffInfo, da, YearMonthString)
                '$02 受注後フォロー機能開発 START
            ElseIf staffOparationCode.Equals(Operation.SL) And IsTeamLeader Then
                returnDataTable = GetResultInfoOfTeam(staffInfo, da, YearMonthString, orgnzIdList)
                '$02 受注後フォロー機能開発 END
            Else
                '検索処理(ユーザ単位)
                returnDataTable = GetResultInfoOfUser(staffInfo, da, YearMonthString)
            End If

            Logger.Info("GetResultInfo End Ret[SC3010202ResultDataTable[Count =" & returnDataTable.Count & "}]")
            Return returnDataTable
        End Using

    End Function

    ''' <summary>
    ''' 店舗の営業実績情報を取得する。
    ''' </summary>
    ''' <param name="staffInfo">スタッフ情報</param>
    ''' <param name="TableAdapter">テーブルアダプター</param>
    ''' <param name="YearMonthString">対象年月</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証
    ''' </history>
    Private Function GetResultInfoOfBranch(ByVal staffInfo As StaffContext, ByVal TableAdapter As SC3010202TableAdapter, _
                                           ByVal YearMonthString As String) As SC3010202ResultDataTable

        Logger.Info("GetResultInfoOfBranch Start Param[staffInfo, TableAdapter, YearMonthString=" & YearMonthString & "]")

        Using dt As SC3010202ResultDataTable = New SC3010202ResultDataTable

            '検索処理(店舗単位)
            Dim dr As SC3010202ResultRow = dt.NewSC3010202ResultRow()

            '来店実績情報を取得
            Dim walkInDataTable As SC3010202ResultWalkInDataTable
            walkInDataTable = TableAdapter.GetResultWalkInOfBranch(staffInfo.DlrCD, staffInfo.BrnCD, YearMonthString)
            If Not IsNothing(walkInDataTable) Then
                Dim walkInDataRow As SC3010202ResultWalkInRow = walkInDataTable.Rows(0)
                dr.WALKIN = walkInDataRow.WALKIN
            End If


            '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 START
            'FLLWBOX履歴情報を取得(見積、試乗、査定)
            Dim crhisDataTable As SC3010202ResultCRHISDataTable
            Dim crhisDataTableHis As SC3010202ResultCRHISDataTable
            Dim evaluationCount As Integer = 0
            Dim testdriveCount As Integer = 0
            crhisDataTable = TableAdapter.GetResultHistoryOfBranch(staffInfo.DlrCD, staffInfo.BrnCD, YearMonthString, False)

            If Not IsNothing(crhisDataTable) Then
                For Each crhisDataRow As SC3010202ResultCRHISRow In crhisDataTable.Rows
                    Select Case crhisDataRow.ACTIONCD
                        Case RSLT_SALES_CAT_EVALUATION
                            '査定情報を設定
                            evaluationCount = evaluationCount + crhisDataRow.CNT
                        Case RSLT_SALES_CAT_TESTDRIVE
                            '試乗情報を設定
                            testdriveCount = testdriveCount + crhisDataRow.CNT
                    End Select
                Next
            End If

            crhisDataTableHis = TableAdapter.GetResultHistoryOfBranch(staffInfo.DlrCD, staffInfo.BrnCD, YearMonthString, True)
            If Not IsNothing(crhisDataTableHis) Then
                For Each crhisHisDataRow As SC3010202ResultCRHISRow In crhisDataTableHis.Rows
                    Select Case crhisHisDataRow.ACTIONCD
                        Case RSLT_SALES_CAT_EVALUATION
                            '査定情報を設定
                            evaluationCount = evaluationCount + crhisHisDataRow.CNT
                        Case RSLT_SALES_CAT_TESTDRIVE
                            '試乗情報を設定
                            testdriveCount = testdriveCount + crhisHisDataRow.CNT
                    End Select
                Next
            End If

            dr.EVALUATION = evaluationCount
            dr.TESTDRIVE = testdriveCount
            '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 END


            'FLLWUPBOX集計情報を取得
            Dim fllwupBoxTallyDataTable As SC3010202ResultTallyDataTable
            fllwupBoxTallyDataTable = TableAdapter.GetResultFollowUpBoxTallyOfBranch(staffInfo.DlrCD, staffInfo.BrnCD, YearMonthString)
            If Not IsNothing(fllwupBoxTallyDataTable) Then
                '受注情報を設定
                Dim resultTallyDataRow As SC3010202ResultTallyRow = fllwupBoxTallyDataTable.Rows(0)
                dr.ORDERS = resultTallyDataRow.CNT
            End If

            ' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証 START
            ''システム設定から納車活動コードを取得
            'Dim deliveryActCd As String = TableAdapter.GetSytemSetting(SYSTEMSETTING_AFTER_ODR_ACT_CD_DELI)

            'システム設定から納車活動コードを取得
            Dim systemSetting As New SystemSetting
            Dim row As TB_M_SYSTEM_SETTINGRow = systemSetting.GetSystemSetting(SYSTEMSETTING_AFTER_ODR_ACT_CD_DELI)
            Dim deliveryActCd As String = String.Empty
            If row IsNot Nothing Then
                deliveryActCd = row.SETTING_VAL
            End If
            ' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証 END

            '納車実績情報を取得
            Dim deliveryCount As Integer
            deliveryCount = TableAdapter.GetResultDeliveryOfBranch(staffInfo.DlrCD, staffInfo.BrnCD, YearMonthString, deliveryActCd)
            dr.DELIVERY = deliveryCount

            '結果行をテーブルに反映
            dt.Rows.Add(dr)

            Logger.Info("GetResultInfoOfBranch End Ret[SC3010202ResultDataTable[Count =" & dt.Count & "}]")
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
    ''' <history>
    ''' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証
    ''' </history>
    Private Function GetResultInfoOfUser(ByVal staffInfo As StaffContext, ByVal TableAdapter As SC3010202TableAdapter, _
                                         ByVal YearMonthString As String) As SC3010202ResultDataTable

        Logger.Info("GetResultInfoOfUser Start Param[staffInfo, TableAdapter, YearMonthString=" & YearMonthString & "]")

        Using dt As SC3010202ResultDataTable = New SC3010202ResultDataTable

            '検索処理(ユーザ単位)
            Dim dr As SC3010202ResultRow = dt.NewSC3010202ResultRow()

            '来店実績情報を取得
            Dim walkInDataTable As SC3010202ResultWalkInDataTable
            walkInDataTable = TableAdapter.GetResultWalkIn(staffInfo.DlrCD, staffInfo.BrnCD, YearMonthString, staffInfo.Account)
            If Not IsNothing(walkInDataTable) Then
                Dim walkInDataRow As SC3010202ResultWalkInRow = walkInDataTable.Rows(0)
                dr.WALKIN = walkInDataRow.WALKIN
            End If

            '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 START
            'FLLWBOX履歴情報を取得(見積、試乗、査定)
            Dim crhisDataTable As SC3010202ResultCRHISDataTable
            Dim crhisDataTableHis As SC3010202ResultCRHISDataTable
            Dim evaluationCount As Integer = 0
            Dim testdriveCount As Integer = 0

            crhisDataTable = TableAdapter.GetResultHistory(staffInfo.DlrCD, YearMonthString, staffInfo.Account, False)

            If Not IsNothing(crhisDataTable) Then
                For Each crhisDataRow As SC3010202ResultCRHISRow In crhisDataTable.Rows
                    Select Case crhisDataRow.ACTIONCD
                        Case RSLT_SALES_CAT_EVALUATION
                            '査定情報を設定
                            evaluationCount = evaluationCount + crhisDataRow.CNT
                        Case RSLT_SALES_CAT_TESTDRIVE
                            '試乗情報を設定
                            testdriveCount = testdriveCount + crhisDataRow.CNT
                    End Select
                Next
            End If

            crhisDataTableHis = TableAdapter.GetResultHistory(staffInfo.DlrCD, YearMonthString, staffInfo.Account, True)
            If Not IsNothing(crhisDataTableHis) Then
                For Each crhisHisDataRow As SC3010202ResultCRHISRow In crhisDataTableHis.Rows
                    Select Case crhisHisDataRow.ACTIONCD
                        Case RSLT_SALES_CAT_EVALUATION
                            '査定情報を設定
                            evaluationCount = evaluationCount + crhisHisDataRow.CNT
                        Case RSLT_SALES_CAT_TESTDRIVE
                            '試乗情報を設定
                            testdriveCount = testdriveCount + crhisHisDataRow.CNT
                    End Select
                Next
            End If

            dr.EVALUATION = evaluationCount
            dr.TESTDRIVE = testdriveCount
            '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 END

            'FLLWUPBOX集計情報を取得
            Dim fllwupBoxTallyDataTable As SC3010202ResultTallyDataTable
            fllwupBoxTallyDataTable = TableAdapter.GetResultFollowUpBoxTally(staffInfo.DlrCD, staffInfo.BrnCD, staffInfo.Account, YearMonthString)
            If Not IsNothing(fllwupBoxTallyDataTable) Then

                Dim ordersCount As Integer = 0

                '受注情報を設定
                Dim resultTallyDataRow As SC3010202ResultTallyRow = fllwupBoxTallyDataTable.Rows(0)

                If Not resultTallyDataRow.IsCNTNull Then
                    ordersCount = resultTallyDataRow.CNT
                End If

                dr.ORDERS = ordersCount
            End If

            ' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証 START
            ''システム設定から納車活動コードを取得
            'Dim deliveryActCd As String = TableAdapter.GetSytemSetting(SYSTEMSETTING_AFTER_ODR_ACT_CD_DELI)

            'システム設定から納車活動コードを取得
            Dim systemSetting As New SystemSetting
            Dim row As TB_M_SYSTEM_SETTINGRow = systemSetting.GetSystemSetting(SYSTEMSETTING_AFTER_ODR_ACT_CD_DELI)
            Dim deliveryActCd As String = String.Empty
            If row IsNot Nothing Then
                deliveryActCd = row.SETTING_VAL
            End If
            ' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証 END

            '納車実績情報を取得
            Dim deliveryCount As Integer
            deliveryCount = TableAdapter.GetResultDelivery(staffInfo.DlrCD, staffInfo.BrnCD, YearMonthString, staffInfo.Account, deliveryActCd)
            dr.DELIVERY = deliveryCount

            '結果行をテーブルに反映
            dt.Rows.Add(dr)

            Logger.Info("GetResultInfoOfUser End Ret[SC3010202ResultDataTable[Count =" & dt.Count & "}]")
            Return dt
        End Using

    End Function

    ''' <summary>
    ''' チームの営業実績情報を取得する。
    ''' </summary>
    ''' <param name="staffInfo">スタッフ情報</param>
    ''' <param name="TableAdapter">テーブルアダプター</param>
    ''' <param name="YearMonthString">対象年月</param>
    ''' <param name="orgnzIdList">組織IDリスト</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証
    ''' </history>
    Private Function GetResultInfoOfTeam(ByVal staffInfo As StaffContext, ByVal TableAdapter As SC3010202TableAdapter, _
                                         ByVal YearMonthString As String, ByVal orgnzIdList As List(Of String)) As SC3010202ResultDataTable

        Logger.Info("GetResultInfoOfTeam Start Param[staffInfo, TableAdapter, YearMonthString=" & YearMonthString & _
            ", orgnzIdList=" & String.Join(",", orgnzIdList) & "]")

        Using dt As SC3010202ResultDataTable = New SC3010202ResultDataTable

            '検索処理(ユーザ単位)
            Dim dr As SC3010202ResultRow = dt.NewSC3010202ResultRow()

            '来店実績情報を取得
            Dim walkInDataTable As SC3010202ResultWalkInDataTable
            walkInDataTable = TableAdapter.GetResultWalkInOfTeam(staffInfo.DlrCD, staffInfo.BrnCD, YearMonthString, orgnzIdList)
            If Not IsNothing(walkInDataTable) Then
                Dim walkInDataRow As SC3010202ResultWalkInRow = walkInDataTable.Rows(0)
                dr.WALKIN = walkInDataRow.WALKIN
            End If

            '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 START
            'FLLWBOX履歴情報を取得(見積、試乗、査定)
            Dim crhisDataTable As SC3010202ResultCRHISDataTable
            Dim crhisDataTableHis As SC3010202ResultCRHISDataTable
            Dim evaluationCount As Integer = 0
            Dim testdriveCount As Integer = 0

            crhisDataTable = TableAdapter.GetResultHistoryOfTeam(staffInfo.DlrCD, staffInfo.BrnCD, YearMonthString, orgnzIdList, False)

            If Not IsNothing(crhisDataTable) Then
                For Each crhisDataRow As SC3010202ResultCRHISRow In crhisDataTable.Rows
                    Select Case crhisDataRow.ACTIONCD
                        Case RSLT_SALES_CAT_EVALUATION
                            '査定情報を設定
                            evaluationCount = evaluationCount + crhisDataRow.CNT
                        Case RSLT_SALES_CAT_TESTDRIVE
                            '試乗情報を設定
                            testdriveCount = testdriveCount + crhisDataRow.CNT
                    End Select
                Next
            End If

            crhisDataTableHis = TableAdapter.GetResultHistoryOfTeam(staffInfo.DlrCD, staffInfo.BrnCD, YearMonthString, orgnzIdList, True)
            If Not IsNothing(crhisDataTableHis) Then
                For Each crhisHisDataRow As SC3010202ResultCRHISRow In crhisDataTableHis.Rows
                    Select Case crhisHisDataRow.ACTIONCD
                        Case RSLT_SALES_CAT_EVALUATION
                            '査定情報を設定
                            evaluationCount = evaluationCount + crhisHisDataRow.CNT
                        Case RSLT_SALES_CAT_TESTDRIVE
                            '試乗情報を設定
                            testdriveCount = testdriveCount + crhisHisDataRow.CNT
                    End Select
                Next
            End If

            dr.EVALUATION = evaluationCount
            dr.TESTDRIVE = testdriveCount
            '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 END

            'FLLWUPBOX集計情報を取得
            Dim fllwupBoxTallyDataTable As SC3010202ResultTallyDataTable
            fllwupBoxTallyDataTable = TableAdapter.GetResultFollowUpBoxTallyOfTeam(staffInfo.DlrCD, staffInfo.BrnCD, orgnzIdList, YearMonthString)
            If Not IsNothing(fllwupBoxTallyDataTable) Then
                '受注情報を設定
                Dim resultTallyDataRow As SC3010202ResultTallyRow = fllwupBoxTallyDataTable.Rows(0)
                dr.ORDERS = resultTallyDataRow.CNT
            End If

            ' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証 START
            ''システム設定から納車活動コードを取得
            'Dim deliveryActCd As String = TableAdapter.GetSytemSetting(SYSTEMSETTING_AFTER_ODR_ACT_CD_DELI)

            'システム設定から納車活動コードを取得
            Dim systemSetting As New SystemSetting
            Dim row As TB_M_SYSTEM_SETTINGRow = systemSetting.GetSystemSetting(SYSTEMSETTING_AFTER_ODR_ACT_CD_DELI)
            Dim deliveryActCd As String = String.Empty
            If row IsNot Nothing Then
                deliveryActCd = row.SETTING_VAL
            End If
            ' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証 END

            '納車実績情報を取得
            Dim deliveryCount As Integer
            deliveryCount = TableAdapter.GetResultDeliveryOfTeam(staffInfo.DlrCD, staffInfo.BrnCD, YearMonthString, orgnzIdList, deliveryActCd)
            dr.DELIVERY = deliveryCount

            '結果行をテーブルに反映
            dt.Rows.Add(dr)

            Logger.Info("GetResultInfoOfTeam End Ret[SC3010202ResultDataTable[Count =" & dt.Count & "}]")
            Return dt
        End Using

    End Function

#End Region

#Region "組織リスト取得"

    '$03 組織IDの型変更 START
    '$02 受注後フォロー機能開発 START
    ''' <summary>
    ''' 所属組織とその配下組織の組織情報リストを取得する。
    ''' </summary>
    ''' <param name="staffInfo">スタッフ情報</param>
    ''' <param name="orgnzId">組織ID</param>
    ''' <returns>組織IDリスト</returns>
    ''' <remarks></remarks>
    Public Function GetTeamList(ByVal staffInfo As StaffContext, ByVal orgnzId As Decimal) As List(Of String)

        '$03 組織IDの型変更 END

        Logger.Info("GetTargetInfo Start Param[staffInfo, orgnzId=" & orgnzId.ToString(CultureInfo.InvariantCulture) & "]")

        Dim organizationIdList As New List(Of String)
        Dim dt As SC3010202OrganizationInfoDataTable

        Using da As New SC3010202TableAdapter
            '組織リスト取得
            dt = da.GetTeamList(staffInfo.DlrCD, staffInfo.BrnCD)
        End Using

        GetTeamList(dt, orgnzId, organizationIdList)

        Logger.Info("GetTargetInfo End Ret[organizationIdList=" & String.Join(",", organizationIdList) & "]")
        '処理結果返却
        Return organizationIdList

    End Function

    '$03 組織IDの型変更 START
    ''' <summary>
    ''' 下位組織情報を取得
    ''' </summary>
    ''' <param name="dt">店舗内全組織データ</param>
    ''' <param name="parentOrgnzId">親組織ID</param>
    ''' <param name="organizationIdList">組織IDリスト</param>
    ''' <remarks></remarks>
    Private Sub GetTeamList(ByRef dt As SC3010202OrganizationInfoDataTable, ByVal parentOrgnzId As Decimal, ByRef organizationIdList As List(Of String))

        '$03 組織IDの型変更 END

        Logger.Info("GetTeamList Start Param[parentOrgzId=" & parentOrgnzId.ToString(CultureInfo.InvariantCulture) & "]")

        '$03 組織IDの型変更 START
        organizationIdList.Add(parentOrgnzId.ToString(CultureInfo.InvariantCulture))

        dt.DefaultView.RowFilter = "PARENT_ORGNZ_ID = '" & parentOrgnzId.ToString(CultureInfo.InvariantCulture) & "'"
        '$03 組織IDの型変更 END

        '下位組織無しのため現階層から抜ける
        If dt.DefaultView.Count = 0 Then Exit Sub

        For Each dvr As DataRowView In dt.DefaultView
            Dim dr As SC3010202OrganizationInfoRow = CType(dvr.Row, SC3010202OrganizationInfoRow)

            '再起処理にて下位組織を収集
            GetTeamList(dt, dr.ORGNZ_ID, organizationIdList)
        Next

        Logger.Info("GetTeamList End")
    End Sub
    '$02 受注後フォロー機能開発 END

#End Region

End Class
