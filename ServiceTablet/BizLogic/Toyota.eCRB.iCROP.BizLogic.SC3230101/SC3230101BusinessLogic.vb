'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3230101.aspx.vb
'─────────────────────────────────────
'機能： メインメニュー(FM)画面 ビジネスロジック
'補足： 
'作成： 2014/02/XX NEC 桜井
'更新： 
'更新： 
'─────────────────────────────────────

Option Explicit On
Imports Toyota.eCRB.Foreman.MainMenu.DataAccess
Imports Toyota.eCRB.Foreman.MainMenu.DataAccess.SC3230101DataSet
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.BizLogic
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.DataAccess
Imports Toyota.eCRB.CommonUtility.SMBCommonClass.Api.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web
Imports System.Globalization

''' <summary>
''' メインメニュー(FM)画面のビジネスロジック
''' </summary>
''' <remarks>FMメインメニューのビジネスロジッククラス</remarks>
Public Class SC3230101BusinessLogic
    Inherits BaseBusinessComponent
    Implements IDisposable

#Region "定数"

    ' ''' <summary>車両アイコン出力用Style属性の設定値</summary>
    'Private Const StyleBackGround As String = "background:url('~/../Styles/Images/SC3230101/{0}') no-repeat;"

    ''' <summary>車両アイコン出力用CSSのClass名</summary>
    Private Structure CssCarIcon

        ''' <summary>青＋黄色枠線無し</summary>
        Public Const Blue As String = "CarIcon01"

        ''' <summary>青＋黄色枠線有り</summary>
        Public Const BlueWithLine As String = "CarIcon02"

        ''' <summary>赤＋黄色枠線無し</summary>
        Public Const Red As String = "CarIcon03"

        ''' <summary>赤＋黄色枠線有り</summary>
        Public Const RedWithLine As String = "CarIcon04"

        ''' <summary>黄色＋黄色枠線無し</summary>
        Public Const Yellow As String = "CarIcon05"

        ''' <summary>黄色＋黄色枠線有り</summary>
        Public Const YellowWithLine As String = "CarIcon06"

    End Structure

    ''' <summary>完成検査承認待ちチップの表示順（納車見込み時刻, RO作成日時の昇順）</summary>
    Private Const SortKey_InsRltAppr As String = "DELIVERY_DATE, RO_CREATE_DATETIME"

    ''' <summary>追加作業承認待ちチップの表示順（RO作成日時の昇順）</summary>
    Private Const SortKey_AddJobAppr As String = "RO_CREATE_DATETIME"

    ''' <summary>
    ''' 残完成検査区分
    ''' </summary>
    ''' <remarks></remarks>
    Private Structure Remaining_Inspection_Status

        ''' <summary>
        ''' 残完成検査あり:未完了
        ''' </summary>
        ''' <remarks>残完成検査あり:未完了</remarks>
        Public Const NotComp As String = "0"

        ''' <summary>
        ''' 残完成検査あり：検査待ち
        ''' </summary>
        ''' <remarks>残完成検査あり：検査待ち</remarks>
        Public Const WaitApprove As String = "1"

        ''' <summary>
        ''' 残完成検査なし
        ''' </summary>
        ''' <remarks>残完成検査なし</remarks>
        Public Const NoneInspection As String = "2"

    End Structure

    ''' <summary>
    ''' 完成検査ステータス
    ''' </summary>
    ''' <remarks>作業内容の完成検査ステータス</remarks>
    Private Structure Ins_Status
        ''' <summary>
        ''' 完成検査承認待ち
        ''' </summary>
        ''' <remarks>完成検査承認待ち："1"</remarks>
        Public Const InsRltAppr As String = "1"

        ''' <summary>
        '''  完成検査未完了
        ''' </summary>
        ''' <remarks>完成検査未完了："0"</remarks>
        Public Const insRltNotComp As String = "0"

    End Structure

    Private Const FormatDbDateTime As String = "1900/01/01"                     ' 日時初期値(年月日)

#End Region

#Region "完成検査承認待ちエリア用"

#Region "完成検査承認待ちデータ取得"

    ''' <summary>
    ''' 完成検査承認待ちデータ取得
    ''' </summary>
    ''' <returns>完成検査承認待ちデータ</returns>
    ''' <remarks></remarks>
    Public Function GetInsRltApprData(dlrCD As String, brnCD As String) As DataView
        'TMT2販社 BTS256 他販売店・店舗が表示されないよう修正 2015/03/31 
        '    Public Function GetInsRltApprData() As DataView

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} START" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Using dac As New SC3230101DataSet

            '完成検査承認待ちチップデータ取得
            'TMT2販社 BTS256 他販売店・店舗が表示されないよう修正 2015/03/31 
            '            Dim dt As SC3230101InsRltApprDataTable = dac.GetInsRltApprData()
            'TR-SVT-TMT-20160909-001(レスポンス対応)↓
            'Dim dt As SC3230101InsRltApprDataTable = dac.GetInsRltApprData(dlrCD, brnCD)
            Dim dt As New SC3230101InsRltApprDataTable
            Dim dtBase As SC3230101InsRltApprBaseDataTable
            Dim dtStall As SC3230101InsRltApprStallDataTable
            Dim dtWorktime As SC3230101InsRltApprWorktimeDataTable
            Dim drStall As SC3230101InsRltApprStallRow
            Dim drWorktimeArr As Array

            Dim dv As DataView

            Dim ParameterStallUseId As String = String.Empty
            Dim ParameterJobDtlId As String = String.Empty

            '完成検査承認待ち基本データを取得
            dtBase = dac.GetInsRltApprDataBase(dlrCD, brnCD)
            If dtBase.Rows.Count > 0 Then
                'パラメータ作成
                For Each drBase As SC3230101InsRltApprBaseRow In dtBase.Rows

                    '二つ目以降であればカンマで区切る
                    If ParameterStallUseId <> String.Empty Then
                        ParameterStallUseId &= ","
                    End If
                    If ParameterJobDtlId <> String.Empty Then
                        ParameterJobDtlId &= ","
                    End If

                    ParameterStallUseId &= drBase.MAX_ID
                    ParameterJobDtlId &= drBase.JOB_DTL_ID

                Next

                '完成検査承認待ちストールデータを取得
                dtStall = dac.GetInsRltApprDataStall(ParameterStallUseId)

                '完成検査承認待ち作業時間データを取得
                dtWorktime = dac.GetInsRltApprDataWorktime(ParameterJobDtlId)

                '取得した完成検査情報(基本、ストール、作業時間)から結合データテーブルを作成する
                For Each drBase As SC3230101InsRltApprBaseRow In dtBase.Rows

                    Dim dr As SC3230101InsRltApprRow = DirectCast(dt.NewRow, SC3230101InsRltApprRow)

                    'データ行をセット
                    dr.RO_NUM = drBase.RO_NUM
                    dr.RO_SEQ = drBase.RO_SEQ
                    dr.DLR_CD = drBase.DLR_CD
                    dr.BRN_CD = drBase.BRN_CD
                    dr.RO_CREATE_DATETIME = drBase.RO_CREATE_DATETIME
                    dr.VISITSEQ = drBase.VISITSEQ
                    dr.REZID = drBase.REZID
                    dr.VIN = drBase.VIN
                    dr.SCHE_DELI_DATETIME = drBase.SCHE_DELI_DATETIME
                    dr.CARWASH_NEED_FLG = drBase.CARWASH_NEED_FLG
                    dr.REG_NUM = drBase.REG_NUM
                    dr.CAR_WASH_START = If(drBase.IsRSLT_START_DATETIMENull, Nothing, drBase.RSLT_START_DATETIME)
                    dr.CAR_WASH_END = If(drBase.IsRSLT_END_DATETIMENull, Nothing, drBase.RSLT_END_DATETIME)
                    dr.INSPECTION_REQ_STF_CD = drBase.INSPECTION_REQ_STF_CD
                    dr.JOB_DTL_ID = drBase.JOB_DTL_ID
                    dr.MAX_SCHE_END_DATETIME = If(drBase.IsMAX_SCHE_END_DATETIMENull, Nothing, drBase.MAX_SCHE_END_DATETIME)
                    dr.SVCIN_ID = drBase.SVCIN_ID
                    dr.IMP_VCL_FLG = drBase.IMP_VCL_FLG

                    '完成検査承認待ちストール情報を抽出
                    drStall = DirectCast(dtStall.Select(String.Format("JOB_DTL_ID = {0} AND STALL_USE_ID = {1}", drBase.JOB_DTL_ID, drBase.MAX_ID))(0), SC3230101InsRltApprStallRow)
                    'データ行をセット
                    dr.RSLT_END_DATETIME = If(drStall.IsRSLT_END_DATETIMENull, Nothing, drStall.RSLT_END_DATETIME)
                    dr.STALL_NAME_SHORT = If(drStall.IsSTALL_NAME_SHORTNull, " ", drStall.STALL_NAME_SHORT)

                    '完成検査承認待ち作業時間情報を抽出
                    drWorktimeArr = dtWorktime.Select(String.Format("JOB_DTL_ID = {0} AND STALL_USE_ID = {1}", drBase.JOB_DTL_ID, drBase.MAX_ID))

                    Dim sumWorkTime As Long = 0
                    For Each drWorktime As SC3230101InsRltApprWorktimeRow In drWorktimeArr
                        sumWorkTime = sumWorkTime + drWorktime.SCHE_WORKTIME
                    Next

                    'データ行をセット
                    dr.SUM_SCHE_WORKTIME = sumWorkTime

                    dt.Rows.Add(dr)
                Next

                dt = Me.GetInsRltApprCarIconUrl(dt)

                dv = Me.SortedInsRltApprData(dt)

            Else
                dv = New DataView
            End If
            'TR-SVT-TMT-20160909-001(レスポンス対応)↑

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} END" _
                , Me.GetType.ToString _
                , System.Reflection.MethodBase.GetCurrentMethod.Name))

            Return dv

        End Using

    End Function

#End Region

    ''' <summary>
    ''' 完成検査承認待ちチップの車両アイコン取得
    ''' </summary>
    ''' <param name="dt">完成検査承認待ちチップデータ</param>
    ''' <returns>完成検査承認待ちチップの納車見込み時刻、車両アイコンUrlを格納したチップデータ</returns>
    ''' <remarks>納車見込み時刻を取得して、その情報を元に表示する車両アイコンを取得する</remarks>
    Protected Function GetInsRltApprCarIconUrl _
        (ByVal dt As SC3230101InsRltApprDataTable) As SC3230101InsRltApprDataTable

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} START" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '現地の現在日時を取得
        Dim staffInfo As StaffContext = StaffContext.Current
        Logger.Info(String.Format("★★★ DlrCD:[{0}], BrnCD:[{1}] ", staffInfo.DlrCD, staffInfo.BrnCD))   'TODO：デバッグログ
        Dim nowDate As Date = DateTimeFunc.Now(staffInfo.DlrCD, staffInfo.BrnCD)
        Logger.Info(String.Format("★★★ DateTimeFunc.Now:[{0}]", nowDate))                               'TODO：デバッグログ

        Dim userID As String = staffInfo.Account    'ログインユーザID
        'Dim deliveryDate As Date                    '納車見込み時刻格納用ワーク変数
        Dim deliveryDelayDate As Date                    '納車見込み遅れ時刻格納用ワーク変数
        Dim iconCssClass As String = ""             '車両アイコンスタイルシートクラス格納用ワーク変数

        Using smbBiz As New SMBCommonClassBusinessLogic

            Dim maxCount As Integer = dt.Rows.Count

            Logger.Info(String.Format("★★★ DataCount:[{0}]", maxCount))                                 'TODO：デバッグログ

            'SMB用共通関数の初期処理
            Dim smbRtn As Long
            smbRtn = smbBiz.InitCommon(staffInfo.DlrCD, staffInfo.BrnCD, nowDate)
            If smbRtn <> SMBCommonClassBusinessLogic.ReturnCode.Success Then

                'エラー時、処理を抜ける
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} {2} [{3}]" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , "SMBCommonClassBusinessLogic.InitCommon Error" _
                    , smbRtn.ToString()))

                Return dt
            End If

            'サービス入庫リストの作成
            Dim SvcinIdList As New List(Of Decimal)
            For Each dr As SC3230101InsRltApprRow In dt
                '重複のサービス入庫IDを省く
                If Not SvcinIdList.Contains(dr.SVCIN_ID) Then
                    SvcinIdList.Add(dr.SVCIN_ID)
                End If
            Next

            Dim dtRminingInspectionKbn As SC3230101GetRemainInspectionStatusDataTable
            Dim dtMaxEndDate As SC3230101MaxEndDateInfoDataTable
            Using dac As New SC3230101DataSet

                If SvcinIdList.Count <> 0 Then
                    '残完成検査区分の取得
                    dtRminingInspectionKbn = dac.GetRemainInspectionStatus(SvcinIdList)

                    '作業終了時刻の取得
                    dtMaxEndDate = dac.GetScheEndInfo(staffInfo.DlrCD, staffInfo.BrnCD, SvcinIdList)
                Else
                    dtRminingInspectionKbn = New SC3230101GetRemainInspectionStatusDataTable

                    dtMaxEndDate = New SC3230101MaxEndDateInfoDataTable

                End If

            End Using

            For i As Integer = 0 To maxCount - 1

                Dim row As SC3230101InsRltApprRow
                row = DirectCast(dt.Rows(i), SC3230101InsRltApprRow)

                '残完成検査区分　個別に設定 reminingInspectionKbn
                Dim reminingInspectionKbn As String = String.Empty
                Dim drRminingInspectionKbn() As DataRow = dtRminingInspectionKbn.Select(String.Format(" SVCIN_ID = {0}", row.SVCIN_ID))
                Dim reminingInspectionCount As Decimal
                Dim reminingInspectionStatus As String

                If drRminingInspectionKbn.Count > 0 Then
                    reminingInspectionCount = Decimal.Parse(drRminingInspectionKbn(0).Item("ROW_COUNT").ToString)
                    reminingInspectionStatus = drRminingInspectionKbn(0).Item("REMAIN_INSPECTION_STATUS").ToString
                Else
                    reminingInspectionCount = 0
                    reminingInspectionStatus = String.Empty
                End If

                If reminingInspectionCount = 0 Then
                    '集約の結果、残完成検査が無い場合は、"2"（なし）を返却
                    reminingInspectionKbn = Remaining_Inspection_Status.NoneInspection

                ElseIf reminingInspectionStatus = Ins_Status.insRltNotComp Then
                    '集約の結果、残完成検査があり、未完了がある場合は、"0"（残完成検査未完了）を返却
                    reminingInspectionKbn = Remaining_Inspection_Status.NotComp

                Else
                    '集約の結果、残完成検査があり、未完了がない場合は、"1"（残完成検査承認待ち）を返却
                    reminingInspectionKbn = Remaining_Inspection_Status.WaitApprove
                End If

                '作業終了時刻　個別に設定 maxEndDate
                Dim drMaxEndDate() As DataRow = dtMaxEndDate.Select(String.Format(" SVCIN_ID = {0}", row.SVCIN_ID))
                Dim maxEndDate As Date
                If drMaxEndDate.Count > 0 Then
                    maxEndDate = Date.Parse(drMaxEndDate(0).Item("MAX_END_DATETIME").ToString)
                Else
                    maxEndDate = Date.Parse(FormatDbDateTime, CultureInfo.CurrentCulture)
                End If

                'Logger.Info(String.Format("★★★ smbBiz.GetDeliveryDate START " _
                '          & " RO_NUM=[{0}] / REG_NUM=[{1}] /  " _
                '          & " Param:Now=[{2}] / WORKTIME=[{3}]" _
                '          , row.RO_NUM _
                '          , row.REG_NUM _
                '          , nowDate.ToString() _
                '          , row.SUM_SCHE_WORKTIME.ToString()))                              'TODO：デバッグログ

                Logger.Info(String.Format("★★★ smbBiz.GetDeliveryDelayDate START " _
                          & " RO_NUM=[{0}] / REG_NUM=[{1}] /  " _
                          & " Param:Now=[{2}] / WORKTIME=[{3}]" _
                          & " PARAMETER inDisplayType=[{4}] inDeliveryTime=[{5}] inWorkEndTime=[{6}] inWashStartTime=[{7}] " _
                          & " inWashEndTime=[{8}] inWashExistence=[{9}] inPresentTime=[{10}] reminingInspectionKbn=[{11}]" _
                          , row.RO_NUM _
                          , row.REG_NUM _
                          , nowDate.ToString() _
                          , row.SUM_SCHE_WORKTIME.ToString() _
                          , SMBCommonClassBusinessLogic.DisplayType.Work, _
                            row.SCHE_DELI_DATETIME, _
                            maxEndDate, _
                            If(row.IsCAR_WASH_STARTNull, Nothing, row.CAR_WASH_START), _
                            If(row.IsCAR_WASH_ENDNull, Nothing, row.CAR_WASH_END), _
                            row.CARWASH_NEED_FLG, _
                            nowDate, _
                            reminingInspectionKbn
                          ))

                ''完成検査承認待ちチップの納車見込み時刻取得
                'deliveryDelayDate = smbBiz.GetDeliveryDelayDate( _
                '            SMBCommonClassBusinessLogic.DisplayType.Work, _
                '            row.MAX_SCHE_END_DATETIME, _
                '            Nothing, _
                '            If(row.IsCAR_WASH_STARTNull, Nothing, row.CAR_WASH_START), _
                '            If(row.IsCAR_WASH_ENDNull, Nothing, row.CAR_WASH_END), _
                '            Nothing, _
                '            row.SUM_SCHE_WORKTIME, _
                '            row.CARWASH_NEED_FLG, _
                '            nowDate)

                '完成検査承認待ちチップの納車見込み遅れ時刻取得
                deliveryDelayDate = smbBiz.GetDeliveryDelayDate( _
                            SMBCommonClassBusinessLogic.DisplayType.Work, _
                            row.SCHE_DELI_DATETIME, _
                            maxEndDate, _
                            Nothing, _
                            If(row.IsCAR_WASH_STARTNull, Nothing, row.CAR_WASH_START), _
                            If(row.IsCAR_WASH_ENDNull, Nothing, row.CAR_WASH_END), _
                            Nothing, _
                            row.SUM_SCHE_WORKTIME, _
                            row.CARWASH_NEED_FLG, _
                            nowDate, _
                            reminingInspectionKbn)

                'Logger.Info(String.Format("★★★ smbBiz.GetDeliveryDate END Value：[{0}]", deliveryDate)) 'TODO：デバッグログ
                Logger.Info(String.Format("★★★ smbBiz.GetDeliveryDelayDate END Value：[{0}]", deliveryDelayDate))

                'row.DELIVERY_DATE = deliveryDate
                row.DELIVERY_DATE = deliveryDelayDate

                '「現在時刻＞納車予定時刻」の場合、車両アイコンは赤色
                If Date.Compare(nowDate, row.SCHE_DELI_DATETIME) = 1 Then

                    '「ログインユーザID = [作業内容.完成検査依頼スタッフコード]」の場合、黄色枠付
                    If userID = row.INSPECTION_REQ_STF_CD Then
                        iconCssClass = CssCarIcon.RedWithLine
                    Else
                        iconCssClass = CssCarIcon.Red
                    End If

                    '「納車見込み時刻＞納車予定時刻」の場合、車両アイコンは黄色
                    'ElseIf Date.Compare(deliveryDate, row.SCHE_DELI_DATETIME) = 1 Then
                    '「現在時刻＞＝納車見込み遅れ時刻」の場合、車両アイコンは黄色
                ElseIf Date.Compare(nowDate, deliveryDelayDate) >= 0 Then

                    '「ログインユーザID = [作業内容.完成検査依頼スタッフコード]」の場合、黄色枠付
                    If userID = row.INSPECTION_REQ_STF_CD Then
                        iconCssClass = CssCarIcon.YellowWithLine
                    Else
                        iconCssClass = CssCarIcon.Yellow
                    End If

                    '上記以外の場合、車両アイコンは青色
                Else

                    '「ログインユーザID = [作業内容.完成検査依頼スタッフコード]」の場合、黄色枠付
                    If userID = row.INSPECTION_REQ_STF_CD Then
                        iconCssClass = CssCarIcon.BlueWithLine
                    Else
                        iconCssClass = CssCarIcon.Blue
                    End If

                End If

                Logger.Info(String.Format("★★★ CAR_ICON_URL：[{0}]", iconCssClass))                       'TODO：デバッグログ

                row.CAR_ICON_CSS = iconCssClass

            Next

        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} END" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return dt

    End Function

    ''' <summary>
    ''' 完成検査承認待ちチップデータソート処理
    ''' </summary>
    ''' <param name="dt">完成検査承認待ちチップデータ</param>
    ''' <returns>納車見込み時刻, RO作成日時の昇順でソートしたデータ</returns>
    ''' <remarks>完成検査承認待ちチップデータを「納車見込み時刻, RO作成日時の昇順」でソートする</remarks>
    Protected Function SortedInsRltApprData(ByVal dt As SC3230101InsRltApprDataTable) As DataView

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} START" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim dv As New DataView(dt)

        dv.Sort = SortKey_InsRltAppr

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} END" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return dv

    End Function

#End Region

#Region "追加作業承認待ちエリア用"

#Region "追加作業承認待ちデータ取得"

    ''' <summary>
    ''' 追加作業承認待ちデータ取得
    ''' </summary>
    ''' <returns>追加作業承認待ちデータ</returns>
    ''' <remarks></remarks>
    Public Function GetAddJobApprData(dlrCD As String, brnCD As String) As DataView
        'BTS256 他販売店・店舗が表示されないよう修正 2015/03/31 
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} START" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Using dac As New SC3230101DataSet

            '追加作業承認待ちチップデータ取得
            'BTS256 他販売店・店舗が表示されないよう修正 2015/03/31 
            'Dim dt As SC3230101AddJobApprDataTable = dac.GetAddJobApprData()
            Dim dt As SC3230101AddJobApprDataTable = dac.GetAddJobApprData(dlrCD, brnCD)

            dt = Me.GetAddJobApprCarIconUrl(dt)

            Dim dv As DataView = Me.SortedAddJobApprData(dt)

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} END" _
                , Me.GetType.ToString _
                , System.Reflection.MethodBase.GetCurrentMethod.Name))

            Return dv

        End Using

    End Function

#End Region

    ''' <summary>
    ''' 追加作業承認待ちチップの車両アイコン取得
    ''' </summary>
    ''' <param name="dt">追加作業承認待ちチップデータ</param>
    ''' <returns>追加作業承認待ちチップの納車見込み時刻、車両アイコンUrlを格納したチップデータ</returns>
    ''' <remarks>納車見込み時刻を取得して、その情報を元に表示する車両アイコンを取得する</remarks>
    Protected Function GetAddJobApprCarIconUrl _
        (ByVal dt As SC3230101AddJobApprDataTable) As SC3230101AddJobApprDataTable

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} START" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '現地の現在日時を取得
        Dim staffInfo As StaffContext = StaffContext.Current
        Logger.Info(String.Format("★★★ DlrCD:[{0}], BrnCD:[{1}] ", staffInfo.DlrCD, staffInfo.BrnCD))   'TODO：デバッグログ
        Dim nowDate As Date = DateTimeFunc.Now(staffInfo.DlrCD, staffInfo.BrnCD)
        Logger.Info(String.Format("★★★ DateTimeFunc.Now:[{0}]", nowDate))                               'TODO：デバッグログ

        Dim userID As String = staffInfo.Account    'ログインユーザID
        'Dim deliveryDate As Date                    '納車見込み時刻格納用ワーク変数
        Dim deliveryDelayDate As Date                    '納車見込み遅れ時刻格納用ワーク変数
        Dim iconCssClass As String = ""             '車両アイコンスタイルシートクラス格納用ワーク変数

        Using smbBiz As New SMBCommonClassBusinessLogic

            Dim maxCount As Integer = dt.Rows.Count

            Logger.Info(String.Format("★★★ DataCount:[{0}]", maxCount))                                 'TODO：デバッグログ

            'SMB用共通関数の初期処理
            Dim smbRtn As Long
            smbBiz.InitCommon(staffInfo.DlrCD, staffInfo.BrnCD, nowDate)
            If smbRtn <> SMBCommonClassBusinessLogic.ReturnCode.Success Then

                'エラー時、処理を抜ける
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} {2} [{3}]" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , "SMBCommonClassBusinessLogic.InitCommon Error" _
                    , smbRtn.ToString()))

                Return dt
            End If

            'サービス入庫リストの作成
            Dim SvcinIdList As New List(Of Decimal)
            For Each dr As SC3230101AddJobApprRow In dt
                '重複のサービス入庫IDを省く
                If Not SvcinIdList.Contains(dr.SVCIN_ID) Then
                    SvcinIdList.Add(dr.SVCIN_ID)
                End If
            Next

            Dim dtRminingInspectionKbn As SC3230101GetRemainInspectionStatusDataTable
            Dim dtMaxEndDate As SC3230101MaxEndDateInfoDataTable
            Using dac As New SC3230101DataSet

                If SvcinIdList.Count <> 0 Then
                    '残完成検査区分の取得
                    dtRminingInspectionKbn = dac.GetRemainInspectionStatus(SvcinIdList)

                    '作業終了時刻の取得
                    dtMaxEndDate = dac.GetScheEndInfo(staffInfo.DlrCD, staffInfo.BrnCD, SvcinIdList)
                Else
                    dtRminingInspectionKbn = New SC3230101GetRemainInspectionStatusDataTable

                    dtMaxEndDate = New SC3230101MaxEndDateInfoDataTable

                End If

            End Using
            For i As Integer = 0 To maxCount - 1

                Dim row As SC3230101AddJobApprRow
                row = DirectCast(dt.Rows(i), SC3230101AddJobApprRow)

                '残完成検査区分　個別に設定 reminingInspectionKbn
                Dim reminingInspectionKbn As String = String.Empty
                Dim drRminingInspectionKbn() As DataRow = dtRminingInspectionKbn.Select(String.Format(" SVCIN_ID = {0}", row.SVCIN_ID))
                Dim reminingInspectionCount As Decimal
                Dim reminingInspectionStatus As String

                If drRminingInspectionKbn.Count > 0 Then
                    reminingInspectionCount = Decimal.Parse(drRminingInspectionKbn(0).Item("ROW_COUNT").ToString)
                    reminingInspectionStatus = drRminingInspectionKbn(0).Item("REMAIN_INSPECTION_STATUS").ToString
                Else
                    reminingInspectionCount = 0
                    reminingInspectionStatus = String.Empty
                End If

                If reminingInspectionCount = 0 Then
                    '集約の結果、残完成検査が無い場合は、"2"（なし）を返却
                    reminingInspectionKbn = Remaining_Inspection_Status.NoneInspection

                ElseIf reminingInspectionStatus = Ins_Status.insRltNotComp Then
                    '集約の結果、残完成検査があり、未完了がある場合は、"0"（残完成検査未完了）を返却
                    reminingInspectionKbn = Remaining_Inspection_Status.NotComp

                Else
                    '集約の結果、残完成検査があり、未完了がない場合は、"1"（残完成検査承認待ち）を返却
                    reminingInspectionKbn = Remaining_Inspection_Status.WaitApprove
                End If

                '作業終了時刻　個別に設定 maxEndDate
                Dim drMaxEndDate() As DataRow = dtMaxEndDate.Select(String.Format(" SVCIN_ID = {0}", row.SVCIN_ID))
                Dim maxEndDate As Date
                If drMaxEndDate.Count > 0 Then
                    maxEndDate = Date.Parse(drMaxEndDate(0).Item("MAX_END_DATETIME").ToString)
                Else
                    maxEndDate = Date.Parse(FormatDbDateTime, CultureInfo.CurrentCulture)
                End If

                'Logger.Info(String.Format("★★★ smbBiz.GetDeliveryDate START " _
                '                          & " RO_NUM=[{0}] / REG_NUM=[{1}] /  " _
                '                          & " Param:Now=[{2}] / WORKTIME=[{3}]" _
                '                          , row.RO_NUM _
                '                          , row.REG_NUM _
                '                          , nowDate.ToString() _
                '                          , row.SUM_SCHE_WORKTIME.ToString()))                              'TODO：デバッグログ

                Logger.Info(String.Format("★★★ smbBiz.GetDeliveryDelayDate START " _
                          & " RO_NUM=[{0}] / REG_NUM=[{1}] /  " _
                          & " Param:Now=[{2}] / WORKTIME=[{3}]" _
                          & " PARAMETER inDisplayType=[{4}] inDeliveryTime=[{5}] inWorkEndTime=[{6}] inWashStartTime=[{7}] " _
                          & " inWashEndTime=[{8}] inWashExistence=[{9}] inPresentTime=[{10}] reminingInspectionKbn=[{11}]" _
                          , row.RO_NUM _
                          , row.REG_NUM _
                          , nowDate.ToString() _
                          , row.SUM_SCHE_WORKTIME.ToString() _
                          , SMBCommonClassBusinessLogic.DisplayType.AddApprove, _
                            row.SCHE_DELI_DATETIME, _
                            maxEndDate, _
                            If(row.IsCAR_WASH_STARTNull, Nothing, row.CAR_WASH_START), _
                            If(row.IsCAR_WASH_ENDNull, Nothing, row.CAR_WASH_END), _
                            row.CARWASH_NEED_FLG, _
                            nowDate, _
                            reminingInspectionKbn
                          ))

                ''追加作業承認待ちチップの納車見込み時刻取得
                'deliveryDate = smbBiz.GetDeliveryDate( _
                '            SMBCommonClassBusinessLogic.DisplayType.AddApprove, _
                '            row.MAX_SCHE_END_DATETIME, _
                '            Nothing, _
                '            If(row.IsCAR_WASH_STARTNull, Nothing, row.CAR_WASH_START), _
                '            If(row.IsCAR_WASH_ENDNull, Nothing, row.CAR_WASH_END), _
                '            Nothing, _
                '            row.SUM_SCHE_WORKTIME, _
                '            row.CARWASH_NEED_FLG, _
                '            nowDate)

                '完成検査承認待ちチップの納車見込み遅れ時刻取得
                deliveryDelayDate = smbBiz.GetDeliveryDelayDate( _
                            SMBCommonClassBusinessLogic.DisplayType.AddApprove, _
                            row.SCHE_DELI_DATETIME, _
                            maxEndDate, _
                            Nothing, _
                            If(row.IsCAR_WASH_STARTNull, Nothing, row.CAR_WASH_START), _
                            If(row.IsCAR_WASH_ENDNull, Nothing, row.CAR_WASH_END), _
                            Nothing, _
                            row.SUM_SCHE_WORKTIME, _
                            row.CARWASH_NEED_FLG, _
                            nowDate, _
                            reminingInspectionKbn)

                'Logger.Info(String.Format("★★★ smbBiz.GetDeliveryDate END Value：[{0}]", deliveryDate))  'TODO：デバッグログ
                Logger.Info(String.Format("★★★ smbBiz.GetDeliveryDelayDate END Value：[{0}]", deliveryDelayDate))

                'row.DELIVERY_DATE = deliveryDate
                row.DELIVERY_DATE = deliveryDelayDate

                '「現在時刻＞納車予定時刻」の場合、車両アイコンは赤色
                If Date.Compare(nowDate, row.SCHE_DELI_DATETIME) = 1 Then

                    '「ログインユーザID = [RO情報.RO確認スタッフコード]」の場合、黄色枠付
                    If userID = row.RO_CHECK_STF_CD Then
                        iconCssClass = CssCarIcon.RedWithLine
                    Else
                        iconCssClass = CssCarIcon.Red
                    End If

                    '「納車見込み時刻＞納車予定時刻」の場合、車両アイコンは黄色
                    'ElseIf Date.Compare(deliveryDate, row.SCHE_DELI_DATETIME) = 1 Then
                    '「現在時刻＞＝納車見込み遅れ時刻」の場合、車両アイコンは黄色
                ElseIf Date.Compare(nowDate, deliveryDelayDate) >= 0 Then

                    '「ログインユーザID = [RO情報.RO確認スタッフコード]」の場合、黄色枠付
                    If userID = row.RO_CHECK_STF_CD Then
                        iconCssClass = CssCarIcon.YellowWithLine
                    Else
                        iconCssClass = CssCarIcon.Yellow
                    End If

                    '上記以外の場合、車両アイコンは青色
                Else

                    '「ログインユーザID = [RO情報.RO確認スタッフコード]」の場合、黄色枠付
                    If userID = row.RO_CHECK_STF_CD Then
                        iconCssClass = CssCarIcon.BlueWithLine
                    Else
                        iconCssClass = CssCarIcon.Blue
                    End If

                End If

                Logger.Info(String.Format("★★★ CAR_ICON_CSS：[{0}]", iconCssClass))                      'TODO：デバッグログ

                row.CAR_ICON_CSS = iconCssClass

            Next

        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} END" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return dt

    End Function

    ''' <summary>
    ''' 追加作業承認待ちチップデータソート処理
    ''' </summary>
    ''' <param name="dt">追加作業承認待ちチップデータ</param>
    ''' <returns>RO作成日時の昇順でソートしたデータ</returns>
    ''' <remarks>追加作業承認待ちチップデータを「RO作成日時の昇順」でソートする</remarks>
    Protected Function SortedAddJobApprData(ByVal dt As SC3230101AddJobApprDataTable) As DataView

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} START" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim dv As New DataView(dt)

        dv.Sort = SortKey_AddJobAppr

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} END" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return dv

    End Function

#End Region

#Region "フッター制御"

    ''' <summary>
    ''' 基幹コードへ変換処理
    ''' 販売店コード・店舗コード・アカウントをそれぞれ
    ''' 基幹販売店コード・基幹店舗コード・基幹アカウントに変換
    ''' </summary>
    ''' <param name="inStaffInfo">スタッフ情報</param>
    ''' <remarks>基幹コード情報ROW</remarks>
    ''' <history>
    ''' </history>
    Public Function ChangeDmsCode(ByVal inStaffInfo As StaffContext) _
                                  As ServiceCommonClassDataSet.DmsCodeMapRow

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START IN:DLRCD=[{2}] STRCD=[{3}] ACCOUNT=[{4}] " _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , inStaffInfo.DlrCD, inStaffInfo.BrnCD, inStaffInfo.Account))

        'SMBCommonClassBusinessLogicのインスタンス
        Using smbCommon As New ServiceCommonClassBusinessLogic

            '基幹コードへ変換処理
            Dim dtDmsCodeMap As ServiceCommonClassDataSet.DmsCodeMapDataTable = _
                smbCommon.GetIcropToDmsCode(inStaffInfo.DlrCD, _
                                            ServiceCommonClassBusinessLogic.DmsCodeType.BranchCode, _
                                            inStaffInfo.DlrCD, _
                                            inStaffInfo.BrnCD, _
                                            String.Empty, _
                                            inStaffInfo.Account)

            '基幹コード情報Row
            Dim rowDmsCodeMap As ServiceCommonClassDataSet.DmsCodeMapRow

            '基幹コードへ変換処理結果チェック
            If dtDmsCodeMap IsNot Nothing AndAlso 0 < dtDmsCodeMap.Rows.Count Then
                '基幹コードへ変換処理成功

                'Rowに変換
                rowDmsCodeMap = CType(dtDmsCodeMap.Rows(0), ServiceCommonClassDataSet.DmsCodeMapRow)

                '基幹アカウントチェック
                If rowDmsCodeMap.IsACCOUNTNull Then
                    '値無し

                    '空文字を設定する
                    '基幹アカウント
                    rowDmsCodeMap.ACCOUNT = String.Empty

                End If

                '基幹販売店コードチェック
                If rowDmsCodeMap.IsCODE1Null Then
                    '値無し

                    '空文字を設定する
                    '基幹販売店コード
                    rowDmsCodeMap.CODE1 = String.Empty

                End If

                '基幹店舗コードチェック
                If rowDmsCodeMap.IsCODE2Null Then
                    '値無し

                    '空文字を設定する
                    '基幹店舗コード
                    rowDmsCodeMap.CODE2 = String.Empty

                End If

            Else
                '基幹コードへ変換処理成功失敗

                '新しいRowを作成
                rowDmsCodeMap = CType(dtDmsCodeMap.NewDmsCodeMapRow, ServiceCommonClassDataSet.DmsCodeMapRow)

                '空文字を設定する
                '基幹アカウント
                rowDmsCodeMap.ACCOUNT = String.Empty
                '基幹販売店コード
                rowDmsCodeMap.CODE1 = String.Empty
                '基幹店舗コード
                rowDmsCodeMap.CODE2 = String.Empty

            End If

            '終了ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} END dtDmsCodeMap:COUNT = {2}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , dtDmsCodeMap.Count))

            '結果返却
            Return rowDmsCodeMap

        End Using

    End Function
#End Region

#Region "IDisposable Support"
    Private disposedValue As Boolean ' 重複する呼び出しを検出するには

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: マネージ状態を破棄します (マネージ オブジェクト)。
            End If

            ' TODO: アンマネージ リソース (アンマネージ オブジェクト) を解放し、下の Finalize() をオーバーライドします。
            ' TODO: 大きなフィールドを null に設定します。
        End If
        Me.disposedValue = True
    End Sub

    ' TODO: 上の Dispose(ByVal disposing As Boolean) にアンマネージ リソースを解放するコードがある場合にのみ、Finalize() をオーバーライドします。
    'Protected Overrides Sub Finalize()
    '    ' このコードを変更しないでください。クリーンアップ コードを上の Dispose(ByVal disposing As Boolean) に記述します。
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' このコードは、破棄可能なパターンを正しく実装できるように Visual Basic によって追加されました。
    Public Sub Dispose() Implements IDisposable.Dispose
        ' このコードを変更しないでください。クリーンアップ コードを上の Dispose(disposing As Boolean) に記述します。
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class
