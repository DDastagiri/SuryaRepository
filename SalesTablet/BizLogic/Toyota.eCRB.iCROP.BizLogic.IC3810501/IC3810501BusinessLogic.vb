'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'IC3810501BusinessLogic.vb
'─────────────────────────────────────
'機能： 完成検査結果連携
'補足： 
'作成： 2012/01/27 KN 佐藤（真）
'更新： 2012/02/06 KN 佐藤 【SERVICE_1】チップの移動時に休憩時間を考慮
'更新： 2012/02/09 KN 佐藤 【SERVICE_1】SC3150101のメソッド名が変更になったによる対応
'更新： 2012/02/13 KN 佐藤 【SERVICE_1】担当者ストール実績で作業終了日付を更新するように修正
'更新： 2012/02/23 KN 佐藤 【SERVICE_1】StallWorkEndの例外エラーをキャッチしないように修正
'更新： 2012/02/25 KN 佐藤 【SERVICE_1】エラーの戻り値を修正
'更新： 
'─────────────────────────────────────

Imports System.Text
Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.iCROP.DataAccess.IC3810501
Imports Toyota.eCRB.iCROP.DataAccess.SC3150101

''' <summary>
''' IC3810501
''' </summary>
''' <remarks>完成検査結果連携</remarks>
Public Class IC3810501BusinessLogic
    Inherits BaseBusinessComponent
    Implements IDisposable

#Region "定数"
    ''' <summary>
    ''' データ更新用：指定値で上書き
    ''' </summary>
    ''' <remarks></remarks>
    Public Const OVERWRITE_NEW_VALUE As Integer = 1
    ''' <summary>
    ''' データ更新用：変更しない
    ''' </summary>
    ''' <remarks></remarks>
    Public Const KEEP_CURRENT As Integer = 2

    ''' <summary>
    ''' ステータス：ストール本予約
    ''' </summary>
    ''' <remarks></remarks>
    Public Const STATUS_COMMITE_RESOURCE As Integer = 1

    ''' <summary>
    ''' 実績_ステータス：作業中
    ''' </summary>
    ''' <remarks></remarks>
    Public Const RESULT_STATUS_WORKING As String = "20"
    ''' <summary>
    ''' 実績_ステータス:洗車待ち
    ''' </summary>
    ''' <remarks></remarks>
    Public Const RESULT_STATUS_WAITING_FOR_CAR_WASH As String = "40"
    ''' <summary>
    ''' 実績_ステータス:検査待ち
    ''' </summary>
    ''' <remarks></remarks>
    Public Const RESULT_STATUS_WAIT_FOR_INSPECTION As String = "42"
    ''' <summary>
    ''' 実績_ステータス:預かり中
    ''' </summary>
    ''' <remarks></remarks>
    Public Const RESULT_STATUS_IN_CUSTODY As String = "50"
    ''' <summary>
    ''' 実績_ステータス:納車待ち
    ''' </summary>
    ''' <remarks></remarks>
    Public Const RESULT_STATUS_WAIT_FOR_CAR_DELIVERY As String = "60"
    ''' <summary>
    ''' 実績_ステータス:関連チップの前工程作業終了
    ''' </summary>
    ''' <remarks></remarks>
    Public Const RESULT_STATUS_BEFORE_THE_END_OF_STEP As String = "97"

    ''' <summary>
    ''' 戻り値：OK
    ''' </summary>
    ''' <remarks></remarks>
    Public Const RETURN_VALUE_OK As Integer = 0
    ' 2012/02/25 KN 佐藤 【SERVICE_1】エラーの戻り値を修正（処理修正） START
    ' ''' <summary>
    ' ''' 戻り値：NG
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Public Const RETURN_VALUE_NG As Integer = -1
    ''' <summary>
    ''' 戻り値：システムエラー
    ''' </summary>
    ''' <remarks>完成検査承認処理に失敗しました。システム管理者に連絡してください。</remarks>
    Public Const RETURN_VALUE_NG As Integer = 902
    ''' <summary>
    ''' 戻り値：関連データの不整合エラー
    ''' </summary>
    ''' <remarks>完成検査の承認が行えません。関連データに不整合があります。</remarks>
    Public Const RETURN_VALUE_DATA_INCONSISTENCIES As Integer = 903
    ''' <summary>
    ''' 戻り値：ステータスエラー
    ''' </summary>
    ''' <remarks>完成検査の承認が行えません。すでに承認済みか、承認できないステータスになっています。</remarks>
    Public Const RETURN_VALUE_STATUS_ERROR As Integer = 904
    ''' <summary>
    ''' 戻り値：衝突チップの移動エラー
    ''' </summary>
    ''' <remarks>完成検査の承認が行えません。他の作業と重なるか、当日中に収まらない作業が発生します。</remarks>
    Public Const RETURN_VALUE_MOVE_CHIP_ERROR As Integer = 905
    ' 2012/02/25 KN 佐藤 【SERVICE_1】エラーの戻り値を修正（処理修正） END

    ''' <summary>
    ''' 検査エリア使用フラグ
    ''' </summary>
    ''' <remarks></remarks>
    Public Const C_INSPECTION_USE_FLG As String = "INSPECTION_USE_FLG"
    ''' <summary>
    ''' 検査順フラグ
    ''' </summary>
    ''' <remarks></remarks>
    Public Const C_SMB_INSPECTION_ORDER_FLG As String = "SMB_INSPECTION_ORDER_FLG"
#End Region

#Region "作業完了処理"
    ''' <summary>
    '''   完成結果連携で作業終了を行う
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="orderNo">整備受注NO</param>
    ''' <param name="account">更新アカウント</param>
    ''' <param name="break">休憩取得有無</param>
    ''' <returns>処理結果（正常：0、エラー：-1）</returns>
    ''' <remarks></remarks>
    ''' 
    ''' <History>
    ''' </History>
    <EnableCommit()>
    Public Function StallWorkEnd(ByVal dealerCode As String, _
                                    ByVal branchCode As String, _
                                    ByVal orderNo As String, _
                                    ByVal account As String, _
                                    Optional ByVal break As Boolean = False) As Integer
        ' 戻り値の初期化
        Dim retrunCd As Integer = RETURN_VALUE_NG
        Try
            '開始ログを出力
            OutputLog("I", "[S]IC3810501.StallWorkEnd()", "", Nothing, _
                      "DLRCD:" & dealerCode, "STRCD:" & branchCode, "ORDERNO:" & orderNo, _
                      "ACCOUNT:" & account, "break:" & break.ToString(CultureInfo.CurrentCulture))

            ' SC3150101TableAdapterクラスのインスタンスを生成
            Using adapter As New SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter

                '作業終了時刻を取得する
                Dim endTime As Date = DateTimeFunc.Now(StaffContext.Current.DlrCD)

                ' ストール実績情報を取得する
                Dim procInfo As IC3810501DataSet.IC3810501StallProcessInfoDataTable
                Using da As New IC3810501DataSetTableAdapters.IC3810501StallInfoDataTableAdapter
                    procInfo = da.GetStallProcessWorkingInfo(dealerCode, branchCode, orderNo)
                End Using
                ' ストール実績情報が取得できなかった場合、または作業中のデータが複数存在する場合、エラー
                If (procInfo.Rows.Count = 0) Then
                    OutputLog("E", "GetStallProcessWorkingInfo()", "ストール実績の取得に失敗", Nothing)
                    ' 2012/02/25 KN 佐藤 【SERVICE_1】エラーの戻り値を修正（処理修正） START
                    'Return RETURN_VALUE_NG
                    retrunCd = RETURN_VALUE_STATUS_ERROR
                    Exit Try
                    ' 2012/02/25 KN 佐藤 【SERVICE_1】エラーの戻り値を修正（処理修正） END
                End If
                ' ストール実績のDataSetを型変換する
                Dim resultProcInfo As SC3150101DataSet.SC3150101StallProcessInfoDataTable = CastProcessInfo(procInfo)
                Dim drProcInfo As SC3150101DataSet.SC3150101StallProcessInfoRow = DirectCast(resultProcInfo.Rows(0), SC3150101DataSet.SC3150101StallProcessInfoRow)

                ' ストール予約情報を取得する
                ' 2012/02/09 KN 佐藤 【SERVICE_1】SC3150101のメソッド名が変更になったによる対応（処理修正） START
                'Dim reserveInfo As SC3150101DataSet.SC3150101StallReserveInfoDataTable = adapter.GetStallRezInfo(dealerCode, branchCode, CType(drProcInfo.REZID, Integer))
                Dim reserveInfo As SC3150101DataSet.SC3150101StallReserveInfoDataTable = adapter.GetStallReserveInfo(dealerCode, branchCode, CType(drProcInfo.REZID, Integer))
                ' 2012/02/09 KN 佐藤 【SERVICE_1】SC3150101のメソッド名が変更になったによる対応（処理修正） END
                ' ストール予約情報の初期値設定
                InitReserveInfo(reserveInfo)
                Dim drReserveInfo As SC3150101DataSet.SC3150101StallReserveInfoRow = DirectCast(reserveInfo.Rows(0), SC3150101DataSet.SC3150101StallReserveInfoRow)
              
                ' 実績の作業開始日時を取得する
                Dim procStartTime As Date = Date.ParseExact(drProcInfo.RESULT_START_TIME, "yyyyMMddHHmm", Nothing)
                ' 実績の作業終了日時を取得する
                Dim procEndTime As Date = Date.ParseExact(drProcInfo.RESULT_END_TIME, "yyyyMMddHHmm", Nothing)

                ' ストール時間を取得する
                Dim stallTimeInfo As SC3150101DataSet.SC3150101StallTimeInfoDataTable = adapter.GetStallTimeInfo(dealerCode, branchCode, CType(drReserveInfo.STALLID, Integer))
                ' 時間情報を取得する
                Dim drStallTimeInfo As SC3150101DataSet.SC3150101StallTimeInfoRow = DirectCast(stallTimeInfo.Rows(0), SC3150101DataSet.SC3150101StallTimeInfoRow)
          
                ' プログレス開始および終了時間を設定する
                If (drStallTimeInfo.IsPSTARTTIMENull = True) Then
                    drStallTimeInfo.PSTARTTIME = drStallTimeInfo.STARTTIME
                    drStallTimeInfo.PENDTIME = drStallTimeInfo.ENDTIME
                End If

                ' 作業終了時刻をチェック
                Dim resultEndTime As Date = CheckEndTime(drStallTimeInfo, procStartTime, endTime, procEndTime)
                ' 作業終了時刻を再設定する
                endTime = resultEndTime

                ' ストール予約情報の取得範囲(FROM)
                Dim fromDate As Date = procStartTime
                ' ストール予約情報の取得範囲(TO)
                Dim toDate As Date = GetEndDateRange(fromDate, SetStallTime(drStallTimeInfo.PSTARTTIME).TimeOfDay, SetStallTime(drStallTimeInfo.PENDTIME).TimeOfDay)

                ' 指定範囲内のストール予約情報を取得する
                Dim reserveList As SC3150101DataSet.SC3150101StallReserveListDataTable = adapter.GetStallReserveList(dealerCode, _
                                                            branchCode, _
                                                            CType(drReserveInfo.STALLID, Integer), _
                                                            CType(drReserveInfo.REZID, Integer), _
                                                            fromDate, _
                                                            toDate)

                ' 指定範囲内のストール実績情報を取得する
                Dim processList As SC3150101DataSet.SC3150101StallProcessListDataTable = adapter.GetStallProcessList(dealerCode, _
                                                            branchCode, _
                                                            CType(drReserveInfo.STALLID, Integer), _
                                                            fromDate, _
                                                            toDate)
                
                ' チップの移動可能フラグを取得する
                reserveList = GetReserveList(reserveList, processList, break)

                ' タグチェックは行わない

                ' 予約の作業終了時刻を見直す

                ' 2012/02/06 KN 佐藤 【SERVICE_1】チップの移動時に休憩時間を考慮（処理追加） START
                ' 休憩時間帯・使用不可時間帯を取得する
                Dim breakInfo As SC3150101DataSet.SC3150101StallBreakInfoDataTable = adapter.GetBreakSlot(CType(drReserveInfo.STALLID, Integer), fromDate, toDate)

                ' 休憩取得有無をチェック（休憩取得有無を取得）
                Dim resultBreak As Boolean = CheckBreak(breakInfo, _
                                                        break, _
                                                        ParseDate(drProcInfo.RESULT_START_TIME), _
                                                        ParseDate(drProcInfo.RESULT_END_TIME), _
                                                        CType(drReserveInfo.REZ_WORK_TIME, Integer))
                ' 2012/02/06 KN 佐藤 【SERVICE_1】チップの移動時に休憩時間を考慮（処理追加） END

                ' 作業終了時間からの経過時間を取得する
                Dim timeDiff As Long = endTime.Minute Mod drStallTimeInfo.TIMEINTERVAL
                Dim endTimeReserve As Date
                If (timeDiff > 0) Then
                    endTimeReserve = endTime.AddMinutes(drStallTimeInfo.TIMEINTERVAL - timeDiff)
                Else
                    endTimeReserve = endTime
                End If

                ' 2012/02/25 KN 佐藤 【SERVICE_1】エラーの戻り値を修正（処理修正） START
                '' 2012/02/06 KN 佐藤 【SERVICE_1】チップの移動時に休憩時間を考慮（処理修正） START
                '' チップが衝突する場合、移動する
                ''If MoveCollisionChip(reserveList, _
                ''                        drReserveInfo, _
                ''                        stallTimeInfo, _
                ''                        drReserveInfo.STARTTIME, _
                ''                        endTimeReserve, _
                ''                        account) <> RETURN_VALUE_OK Then
                ''    OutputLog("E", "MoveCollisionChip()", "チップの移動に失敗", Nothing)
                ''    Return RETURN_VALUE_NG
                ''End If
                'If MoveCollisionChip(reserveList, _
                '                        drReserveInfo, _
                '                        stallTimeInfo, _
                '                        breakInfo, _
                '                        drReserveInfo.STARTTIME, _
                '                        endTimeReserve, _
                '                        account) <> RETURN_VALUE_OK Then
                '    OutputLog("E", "MoveCollisionChip()", "チップの移動に失敗", Nothing)
                '    Return RETURN_VALUE_NG
                'End If
                '' 2012/02/06 KN 佐藤 【SERVICE_1】チップの移動時に休憩時間を考慮（処理修正） END
                ' チップが衝突する場合、移動する
                Dim resultMoveChip As Integer = MoveCollisionChip(reserveList, _
                                                                  drReserveInfo, _
                                                                  stallTimeInfo, _
                                                                  breakInfo, _
                                                                  drReserveInfo.STARTTIME, _
                                                                  endTimeReserve, _
                                                                  account)
                If (resultMoveChip <> RETURN_VALUE_OK) Then
                    OutputLog("E", "MoveCollisionChip()", "チップの移動に失敗", Nothing)
                    retrunCd = resultMoveChip
                    Exit Try
                End If
                ' 2012/02/25 KN 佐藤 【SERVICE_1】エラーの戻り値を修正（処理修正） END

                ' 2012/02/25 KN 佐藤 【SERVICE_1】エラーの戻り値を修正（処理修正） START
                '' 実績ステータスを更新する
                'If UpdateResultStatus(adapter, _
                '                      drReserveInfo, _
                '                      drProcInfo, _
                '                      dealerCode, _
                '                      branchCode, _
                '                      endTime) <> RETURN_VALUE_OK Then
                '    OutputLog("E", "UpdateResultStatus()", "実績ステータスの更新に失敗", Nothing)
                '    Return RETURN_VALUE_NG
                'End If
                ' 実績ステータスを更新する
                Dim resultUpdateStatus As Integer = UpdateResultStatus(adapter, _
                                                                       drReserveInfo, _
                                                                       drProcInfo, _
                                                                       dealerCode, _
                                                                       branchCode, _
                                                                       endTime)
                If (resultUpdateStatus <> RETURN_VALUE_OK) Then
                    OutputLog("E", "UpdateResultStatus()", "実績ステータスの更新に失敗", Nothing)
                    retrunCd = resultUpdateStatus
                    Exit Try
                End If
                ' 2012/02/25 KN 佐藤 【SERVICE_1】エラーの戻り値を修正（処理修正） END

                ' ストール予約情報の使用終了日時を取得する
                drReserveInfo.ENDTIME = endTimeReserve

                ' ストール予約情報を更新する
                ' 2012/02/09 KN 佐藤 【SERVICE_1】SC3150101のメソッド名が変更になったによる対応（処理修正） START
                'If (adapter.UpdateStallRezInfo(reserveInfo, _
                '                                Nothing, _
                '                                endTime, _
                '                                KEEP_CURRENT, _
                '                                OVERWRITE_NEW_VALUE, _
                '                                account) <= 0) Then
                '    OutputLog("E", "UpdateStallRezInfo()", "ストール予約の更新に失敗", Nothing)
                '    Return RETURN_VALUE_NG
                'End If
                If (adapter.UpdateStallReserveInfo(reserveInfo, _
                                                    Nothing, _
                                                    endTime, _
                                                    KEEP_CURRENT, _
                                                    OVERWRITE_NEW_VALUE, _
                                                    account) <= 0) Then
                    OutputLog("E", "UpdateStallReserveInfo()", "ストール予約の更新に失敗", Nothing)
                    ' 2012/02/25 KN 佐藤 【SERVICE_1】エラーの戻り値を修正（処理修正） START
                    'Return RETURN_VALUE_NG
                    Exit Try
                    ' 2012/02/25 KN 佐藤 【SERVICE_1】エラーの戻り値を修正（処理修正） END
                End If
                ' 2012/02/09 KN 佐藤 【SERVICE_1】SC3150101のメソッド名が変更になったによる対応（処理修正） END

                ' ストール予約履歴を作成する
                ' 2012/02/09 KN 佐藤 【SERVICE_1】SC3150101のメソッド名が変更になったによる対応（処理修正） START
                'If (adapter.InsertRezHistory(drReserveInfo.DLRCD, _
                '                                drReserveInfo.STRCD, _
                '                                CType(drReserveInfo.REZID, Integer), _
                '                                1) <= 0) Then
                '    OutputLog("E", "InsertRezHistory()", "ストール予約履歴の登録に失敗", Nothing)
                '    Return RETURN_VALUE_NG
                'End If
                If (adapter.InsertReserveHistory(drReserveInfo.DLRCD, _
                                                drReserveInfo.STRCD, _
                                                CType(drReserveInfo.REZID, Integer), _
                                                1) <= 0) Then
                    OutputLog("E", "InsertReserveHistory()", "ストール予約履歴の登録に失敗", Nothing)
                    ' 2012/02/25 KN 佐藤 【SERVICE_1】エラーの戻り値を修正（処理修正） START
                    'Return RETURN_VALUE_NG
                    Exit Try
                    ' 2012/02/25 KN 佐藤 【SERVICE_1】エラーの戻り値を修正（処理修正） END
                End If
                ' 2012/02/09 KN 佐藤 【SERVICE_1】SC3150101のメソッド名が変更になったによる対応（処理修正） END

                ' 2012/02/06 KN 佐藤 【SERVICE_1】チップの移動時に休憩時間を考慮（処理削除） START
                '' 休憩時間帯・使用不可時間帯を取得する
                'Dim breakInfo As SC3150101DataSet.SC3150101StallBreakInfoDataTable = adapter.GetBreakSlot(CType(drReserveInfo.STALLID, Integer), fromDate, toDate)

                '' 休憩取得有無をチェック（休憩取得有無を取得）
                'Dim resultBreak As Boolean = CheckBreak(breakInfo, _
                '                                        break, _
                '                                        ParseDate(drProcInfo.RESULT_START_TIME), _
                '                                        ParseDate(drProcInfo.RESULT_END_TIME), _
                '                                        CType(drReserveInfo.REZ_WORK_TIME, Integer))
                ' 2012/02/06 KN 佐藤 【SERVICE_1】チップの移動時に休憩時間を考慮（処理削除） END

                ' ストール実績情報を更新する
                drProcInfo.RESULT_START_TIME = procStartTime.ToString("yyyyMMddHHmm", CultureInfo.CurrentCulture)
                drProcInfo.RESULT_END_TIME = endTime.ToString("yyyyMMddHHmm", CultureInfo.CurrentCulture)
                drProcInfo.RESULT_WORK_TIME = CalculateWorkTime(breakInfo, procStartTime, endTime, resultBreak)
                If (adapter.UpdateStallProcessInfo(resultProcInfo, Nothing) <= 0) Then
                    OutputLog("E", "UpdateStallProcessInfo()", "ストール実績の更新に失敗", Nothing)
                    ' 2012/02/25 KN 佐藤 【SERVICE_1】エラーの戻り値を修正（処理修正） START
                    'Return RETURN_VALUE_NG
                    Exit Try
                    ' 2012/02/25 KN 佐藤 【SERVICE_1】エラーの戻り値を修正（処理修正） END
                End If

                '' 担当者ストール実績データの更新
                If UpdateStaffStall(adapter, drReserveInfo, stallTimeInfo, procStartTime) <> RETURN_VALUE_OK Then
                    OutputLog("E", "UpdateStaffStall()", "担当者ストール実績データの更新に失敗", Nothing)
                    ' 2012/02/25 KN 佐藤 【SERVICE_1】エラーの戻り値を修正（処理修正） START
                    'Return RETURN_VALUE_NG
                    Exit Try
                    ' 2012/02/25 KN 佐藤 【SERVICE_1】エラーの戻り値を修正（処理修正） END
                End If

                '' 洗車順データの更新
                ' 実績_ステータスが「洗車待ち」の場合
                'If drProcInfo.RESULT_STATUS = RESULT_STATUS_WAITING_FOR_CAR_WASH Then
                If String.Equals(drProcInfo.RESULT_STATUS, RESULT_STATUS_WAITING_FOR_CAR_WASH, StringComparison.CurrentCulture) = True Then
                    ' 洗車順データを更新する
                    If UpdateWash(drProcInfo) <> RETURN_VALUE_OK Then
                        OutputLog("E", "UpdateWash()", "洗車順データの登録に失敗", Nothing)
                        ' 2012/02/25 KN 佐藤 【SERVICE_1】エラーの戻り値を修正（処理修正） START
                        'Return RETURN_VALUE_NG
                        Exit Try
                        ' 2012/02/25 KN 佐藤 【SERVICE_1】エラーの戻り値を修正（処理修正） END
                    End If
                End If

                '' 検査順データの更新
                ' 実績_ステータスが「検査待ち」の場合
                'If drProcInfo.RESULT_STATUS = RESULT_STATUS_WAIT_FOR_INSPECTION Then
                If String.Equals(drProcInfo.RESULT_STATUS, RESULT_STATUS_WAIT_FOR_INSPECTION, StringComparison.CurrentCulture) = True Then
                    ' 検査順データを更新する
                    If UpdateInspection(drProcInfo) <> RETURN_VALUE_OK Then
                        OutputLog("E", "UpdateInspection()", "検査順データの登録に失敗", Nothing)
                        ' 2012/02/25 KN 佐藤 【SERVICE_1】エラーの戻り値を修正（処理修正） START
                        'Return RETURN_VALUE_NG
                        Exit Try
                        ' 2012/02/25 KN 佐藤 【SERVICE_1】エラーの戻り値を修正（処理修正） END
                    End If
                End If
            End Using

            ' 正常終了
            retrunCd = RETURN_VALUE_OK

            ' 2012/02/23 KN 佐藤 【SERVICE_1】StallWorkEndの例外エラーをキャッチしないように修正（処理削除） START
            'Catch ex As Exception
            '    retrunCd = RETURN_VALUE_NG
            '    OutputLog("E", "IC3810501.StallWorkEnd()", "例外発生", ex)
            '    Throw
            ' 2012/02/23 KN 佐藤 【SERVICE_1】StallWorkEndの例外エラーをキャッチしないように修正（処理削除） END
        Finally
            ' エラーが発生した場合
            ' 2012/02/25 KN 佐藤 【SERVICE_1】エラーの戻り値を修正（処理修正） START
            'If (retrunCd = RETURN_VALUE_NG) Then
            '    ' ロールバック
            '    Me.Rollback = True
            'End If
            If (retrunCd <> RETURN_VALUE_OK) Then
                ' ロールバック
                Me.Rollback = True
            End If
            ' 2012/02/25 KN 佐藤 【SERVICE_1】エラーの戻り値を修正（処理修正） END
            ' 終了ログ出力
            OutputLog("I", "[E]IC3810501.StallWorkEnd()", "", Nothing, _
                      "RETURN_VALUE:" & retrunCd.ToString(CultureInfo.CurrentCulture))
        End Try
        Return retrunCd
    End Function
#End Region

#Region "メソッド"
   
    ''' <summary>
    ''' 作業終了時刻判定
    ''' 作業開始時刻と作業終了時刻の稼動時間帯が異なる場合、終了時刻を作業予定終了時刻にする
    ''' </summary>
    ''' <param name="drStallTimeInfo">ストール時間情報</param>
    ''' <param name="startTime">作業開始時間</param>
    ''' <param name="endTime">作業終了時間</param>
    ''' <param name="procEndTime">実績の作業予定終了時間</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' 
    ''' <History>
    ''' </History>
    Public Function CheckEndTime(ByVal drStallTimeInfo As SC3150101DataSet.SC3150101StallTimeInfoRow, _
                                 ByVal startTime As Date, _
                                 ByVal endTime As Date, _
                                 ByVal procEndTime As Date) As Date

        Logger.Info("[S]CheckEndTime()")

        Dim operationStartTime As TimeSpan ' 稼働開始時間
        Dim operationEndTime As TimeSpan   ' 稼働終了時間
        operationStartTime = SetStallTime(drStallTimeInfo.PSTARTTIME).TimeOfDay
        operationEndTime = SetStallTime(drStallTimeInfo.PENDTIME).TimeOfDay

        Dim sTimeKadoStart As Date ' 稼動開始時刻(開始)
        Dim eTimeKadoStart As Date ' 稼動開始時刻(終了)
        If (startTime.Add(operationStartTime) < startTime.Add(operationEndTime)) Then
            ' 通常稼動の場合は単に日付の差異をチェック
            If (startTime.Date <> endTime.Date) Then
                endTime = procEndTime
            End If
        Else
            ' 日跨ぎ稼動の場合は、開始・終了ごとの稼動開始時刻を取得
            ' 開始時刻
            If (startTime.Date.AddDays(-1).Add(operationStartTime) <= startTime) _
                AndAlso (startTime < startTime.Date.Add(operationEndTime)) Then
                sTimeKadoStart = startTime.Date.AddDays(-1).Add(operationStartTime)
            Else
                sTimeKadoStart = startTime.Date.Add(operationStartTime)
            End If
            ' 終了時刻
            If (endTime.Date.AddDays(-1).Add(operationStartTime) <= endTime) _
                AndAlso (endTime < endTime.Date.Add(operationEndTime)) Then
                eTimeKadoStart = endTime.Date.AddDays(-1).Add(operationStartTime)
            Else
                eTimeKadoStart = endTime.Date.Add(operationStartTime)
            End If

            If (sTimeKadoStart.Date <> eTimeKadoStart.Date) Then
                endTime = procEndTime
            End If
        End If

        Logger.Info("[E]CheckEndTime()")

        Return endTime
    End Function

    ' 2012/02/06 KN 佐藤 【SERVICE_1】チップの移動時に休憩時間を考慮（処理修正） START
    ''' <summary>
    ''' 指定範囲内の予約情報の取得
    ''' ToDateを指定しない場合に本メソッドでToDateを確定する
    ''' </summary>
    ''' <param name="reserveList">ストール予約情報</param>
    ''' <param name="processList">ストール実績情報</param>
    ''' <param name="isBreak">休憩取得有無</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    ''' 
    ''' <History>
    ''' </History>
    Public Function GetReserveList(ByVal reserveList As SC3150101DataSet.SC3150101StallReserveListDataTable, _
                                   ByVal processList As SC3150101DataSet.SC3150101StallProcessListDataTable, _
                                   ByVal isBreak As Boolean) As SC3150101DataSet.SC3150101StallReserveListDataTable
        'Public Function GetReserveList(ByVal reserveList As SC3150101DataSet.SC3150101StallReserveListDataTable, _
        '                                ByVal processList As SC3150101DataSet.SC3150101StallProcessListDataTable) As SC3150101DataSet.SC3150101StallReserveListDataTable

        'Logger.Info("[S]GetReserveList()")

        'For Each reserveItem As SC3150101DataSet.SC3150101StallReserveListRow In reserveList.Rows

        '    reserveItem.Movable = "1"
        '    If (CType(reserveItem.REZ_RECEPTION, Integer) = 0) Then
        '        If (reserveItem.STATUS = STATUS_COMMITE_RESOURCE) Then
        '            reserveItem.Movable = "0"
        '        End If
        '    Else
        '        If (reserveItem.STARTTIME < Date.ParseExact(reserveItem.REZ_PICK_DATE, "yyyyMMddHHmm", Nothing)) _
        '            OrElse (reserveItem.ENDTIME > Date.ParseExact(reserveItem.REZ_DELI_DATE, "yyyyMMddHHmm", Nothing)) Then
        '            reserveItem.Movable = "0"
        '        End If
        '    End If
        'Next

        'For Each processItem As SC3150101DataSet.SC3150101StallProcessListRow In processList.Rows

        '    Dim drRezList() As SC3150101DataSet.SC3150101StallReserveListRow
        '    drRezList = DirectCast(reserveList.Select("REZID = " & processItem.REZID), SC3150101DataSet.SC3150101StallReserveListRow())

        '    drRezList(0).ProcStatus = processItem.RESULT_STATUS
        '    If (CType(drRezList(0).ProcStatus, Integer) >= CType(RESULT_STATUS_WORKING, Integer)) Then
        '        drRezList(0).STARTTIME = Date.ParseExact(processItem.RESULT_START_TIME, "yyyyMMddHHmm", Nothing)
        '        drRezList(0).ENDTIME = Date.ParseExact(processItem.RESULT_END_TIME, "yyyyMMddHHmm", Nothing)
        '        drRezList(0).Movable = "0"
        '    End If
        'Next

        'Logger.Info("[E]GetReserveList()")

        'Return reserveList
        Logger.Info("[S]GetReserveList()")

        Dim reserveItem As SC3150101DataSet.SC3150101StallReserveListRow

        For Each reserveItem In reserveList.Rows

            reserveItem.Movable = "1"
            If CType(reserveItem.REZ_RECEPTION, Integer) = 0 Then
                'If reserveItem.RezStatus = 1 Then
                If reserveItem.STATUS = STATUS_COMMITE_RESOURCE Then
                    reserveItem.Movable = "0"
                End If
            Else
                If IsDBNull(reserveItem.Item("REZ_PICK_DATE")) Then
                    ' このスコープに入ってきた時は基本的にデータがないことは無いはずだが、
                    '稀に存在するのでとりあえず値を入れておく
                    reserveItem.REZ_PICK_DATE = Date.MinValue.ToString("yyyyMMddHHmm", CultureInfo.CurrentCulture())
                End If
                If IsDBNull(reserveItem.Item("REZ_DELI_DATE")) Then
                    ' このスコープに入ってきた時は基本的にデータがないことは無いはずだが、
                    '稀に存在するのでとりあえず値を入れておく
                    reserveItem.REZ_DELI_DATE = Date.MinValue.ToString("yyyyMMddHHmm", CultureInfo.CurrentCulture())
                End If
                If reserveItem.STARTTIME < Date.ParseExact(reserveItem.REZ_PICK_DATE, "yyyyMMddHHmm", Nothing) _
                    Or reserveItem.ENDTIME > Date.ParseExact(reserveItem.REZ_DELI_DATE, "yyyyMMddHHmm", Nothing) Then
                    reserveItem.Movable = "0"
                End If
            End If
            ' 次世代で追加
            If isBreak Then
                reserveItem.InBreak = "1"
            Else
                reserveItem.InBreak = "0"
            End If
        Next

        ' DBNullの実績データにデフォルト値をセットする
        InitProcessInfo(processList)

        Dim processItem As SC3150101DataSet.SC3150101StallProcessListRow
        Dim drRezList() As SC3150101DataSet.SC3150101StallReserveListRow
        For Each processItem In processList.Rows

            'drRezList = reserveList.Select("REZID = " & processItem.REZID)
            drRezList = CType(reserveList.Select("REZID = " & processItem.REZID), SC3150101DataSet.SC3150101StallReserveListRow())
            'RezItem = _ReserveList.Item(processItem.REZID)

            drRezList(0).ProcStatus = processItem.RESULT_STATUS
            If CType(drRezList(0).ProcStatus, Integer) >= CType(RESULT_STATUS_WORKING, Integer) Then
                drRezList(0).STARTTIME = Date.ParseExact(processItem.RESULT_START_TIME, "yyyyMMddHHmm", Nothing)
                drRezList(0).ENDTIME = Date.ParseExact(processItem.RESULT_END_TIME, "yyyyMMddHHmm", Nothing)
                drRezList(0).Movable = "0"
            End If

        Next

        Logger.Info("[E]GetReserveList()")

        Return reserveList

    End Function
    ' 2012/02/06 KN 佐藤 【SERVICE_1】チップの移動時に休憩時間を考慮（処理修正） END

    ''' <summary>
    ''' 作業日付取得
    ''' 日跨ぎ稼動の場合は作業日付を-1日する
    ''' </summary>
    ''' <param name="stallTimeInfo">ストール時間情報</param>
    ''' <param name="procDate">作業開始時間</param>
    ''' <returns>作業日付</returns>
    ''' <remarks></remarks>
    ''' 
    ''' <History>
    ''' </History>
    Public Function GetWorkDate(ByVal stallTimeInfo As SC3150101DataSet.SC3150101StallTimeInfoDataTable, _
                                ByVal procDate As DateTime) As Date

        Logger.Info("[S]GetWorkDate()")

        Dim workDate As Date

        Dim drStallTimeInfo As SC3150101DataSet.SC3150101StallTimeInfoRow
        drStallTimeInfo = CType(stallTimeInfo.Rows(0), SC3150101DataSet.SC3150101StallTimeInfoRow)

        '稼動時間帯を取得
        Dim operationStartTimet As TimeSpan
        Dim operationEndTime As TimeSpan
        operationStartTimet = SetStallTime(drStallTimeInfo.PSTARTTIME).TimeOfDay
        operationEndTime = SetStallTime(drStallTimeInfo.PENDTIME).TimeOfDay

        workDate = procDate
        'WORKDATEの値を確定
        If (procDate.Date.Add(operationStartTimet) > procDate.Date.Add(operationEndTime)) Then
            '日跨ぎ稼動の場合、前日か当日かどちらの稼働時間帯かを判定
            If (procDate.Date.AddDays(-1).Add(operationStartTimet) <= procDate) _
                AndAlso (procDate < procDate.Date.Add(operationEndTime)) Then
                '前日の稼動時間帯なら-1日する
                workDate = procDate.AddDays(-1)
            End If
        End If

        Logger.Info("[E]GetWorkDate()")

        Return workDate

    End Function

    ''' <summary>
    ''' 指定範囲時間の終了時間を取得
    ''' </summary>
    ''' <param name="fromDate">範囲(FROM)</param>
    ''' <param name="procStartTime">開始時間</param>
    ''' <param name="procEndTime">終了時間</param>
    ''' <returns>範囲(TO)</returns>
    ''' <remarks></remarks>
    ''' 
    ''' <History>
    ''' </History>
    Private Function GetEndDateRange(ByVal fromDate As Date, _
                                        ByVal procStartTime As TimeSpan, _
                                        ByVal procEndTime As TimeSpan) As Date

        Logger.Info("[S]GetEndDateRange()")

        Dim toDate As Date

        '日跨ぎ稼動の場合
        If (fromDate.Date.Add(procStartTime) > fromDate.Date.Add(procEndTime)) Then
            '日跨ぎ稼動の場合、前日か当日かどちらの稼働時間帯かを判定
            If (fromDate.Date.AddDays(-1).Add(procStartTime) <= fromDate) _
                AndAlso (fromDate < fromDate.Date.Add(procEndTime)) Then
                toDate = fromDate.Date.Add(procEndTime)
            Else
                toDate = fromDate.Date.AddDays(1).Add(procEndTime)
            End If
        Else
            toDate = New Date(fromDate.Year, fromDate.Month, fromDate.Day, 23, 59, 59)
        End If

        Logger.Info("[E]GetEndDateRange()")

        Return toDate

    End Function

    ''' <summary>
    ''' 休憩取得有無判定
    ''' </summary>
    ''' <param name="breakList">休憩時間帯・使用不可時間帯情報</param>
    ''' <param name="getBreak">I/Fからの休憩取得フラグ</param>
    ''' <param name="startTime">判定開始時間</param>
    ''' <param name="endTime">判定終了時間</param>
    ''' <param name="workTime">作業予定時間</param>
    ''' <returns>休憩取得有無</returns>
    ''' <remarks></remarks>
    ''' 
    ''' <History>
    ''' </History>
    Public Function CheckBreak(ByVal breakList As SC3150101DataSet.SC3150101StallBreakInfoDataTable, _
                                ByVal getBreak As Boolean, _
                                ByVal startTime As Date, _
                                ByVal endTime As Date, _
                                ByVal workTime As Integer) As Boolean

        Logger.Info("[S]CheckBreak()")

        ' 休憩取得の有無
        Dim retBreak As Boolean

        ' 休憩取得有無の判定
        If (getBreak = True) Then
            ' 休憩を取得する
            retBreak = True
        ElseIf (getBreak = False) Then
            ' 休憩を取得しない
            retBreak = False
        ElseIf (IsBreak(breakList, startTime, endTime) = False) Then
            ' 休憩にかかる場合、休憩を取得する
            retBreak = True
        ElseIf (startTime.AddMinutes(workTime) = endTime) Then
            ' 作業開始時刻から作業時間を引いた時刻が、作業終了時刻の場合、休憩を取得しない
            retBreak = False
        Else
            '　上記条件以外の場合、休憩を取得しない
            retBreak = True
        End If

        Logger.Info("[E]CheckBreak()")

        Return retBreak

    End Function

    ''' <summary>
    ''' 休憩時間にかかるか否かを判定
    ''' </summary>
    ''' <param name="breakList">休憩時間帯・使用不可時間帯情報</param>
    ''' <param name="startTime">判定開始時間</param>
    ''' <param name="endTime">判定終了時間</param>
    ''' <returns>休憩にかかる場合、True</returns>
    ''' <remarks></remarks>
    ''' 
    ''' <History>
    ''' </History>
    Public Function IsBreak(ByVal breakList As SC3150101DataSet.SC3150101StallBreakInfoDataTable, _
                            ByVal startTime As DateTime, _
                            ByVal endTime As DateTime) As Boolean

        Logger.Info("[S]IsBreak()")

        Dim breakItem As SC3150101DataSet.SC3150101StallBreakInfoRow
        For Each breakItem In breakList.Rows

            If (breakItem.STARTTIME < endTime.ToString("HHmm", CultureInfo.CurrentCulture)) _
                AndAlso (breakItem.ENDTIME > startTime.ToString("HHmm", CultureInfo.CurrentCulture)) Then

                Return True

            End If

        Next

        Logger.Info("[E]IsBreak()")

        Return False

    End Function

    ''' <summary>
    ''' 作業時間の計算
    ''' </summary>
    ''' <param name="breakList">休憩時間帯・使用不可時間帯情報</param>
    ''' <param name="startTime">作業開始日時</param>
    ''' <param name="endTime">作業終了日時</param>
    ''' <param name="break">休憩取得有無</param>
    ''' <returns>実作業時間</returns>
    ''' <remarks></remarks>
    ''' 
    ''' <History>
    ''' </History>
    Public Function CalculateWorkTime(ByVal breakList As SC3150101DataSet.SC3150101StallBreakInfoDataTable, _
                                       ByVal startTime As Date, _
                                       ByVal endTime As Date, _
                                       ByVal break As Boolean) As Integer

        Logger.Info("[S]CalculateWorkTime()")

        Dim workTime As Integer
        Dim breakTime As Integer
        Dim breakStartTime As Date
        Dim breakEndTime As Date

        workTime = CType(endTime.Subtract(startTime).TotalMinutes, Integer)

        If (break = True) Then

            For i As Integer = 1 To breakList.Count

                Dim breakItem As SC3150101DataSet.SC3150101StallBreakInfoRow
                breakItem = CType(breakList.Rows(i - 1), SC3150101DataSet.SC3150101StallBreakInfoRow)

                breakStartTime = ParseDate(startTime.ToString("yyyyMMdd", CultureInfo.CurrentCulture) & breakItem.STARTTIME)
                breakEndTime = ParseDate(startTime.ToString("yyyyMMdd", CultureInfo.CurrentCulture) & breakItem.ENDTIME)

                If (breakStartTime >= endTime) Then
                    Exit For
                End If

                If (breakEndTime > startTime) Then
                    If (breakStartTime <= startTime) Then
                        If (breakEndTime <= endTime) Then
                            breakTime = CType(breakEndTime.Subtract(startTime).TotalMinutes, Integer)
                        Else
                            breakTime = CType(endTime.Subtract(startTime).TotalMinutes, Integer)
                        End If
                    Else
                        If (breakEndTime <= endTime) Then
                            breakTime = CType(breakEndTime.Subtract(breakStartTime).TotalMinutes, Integer)
                        Else
                            breakTime = CType(endTime.Subtract(breakStartTime).TotalMinutes, Integer)
                        End If
                    End If
                    workTime = workTime - breakTime

                End If
            Next
        End If

        Logger.Info("[E]CalculateWorkTime()")

        Return workTime

    End Function

    ''' <summary>
    ''' ストール実績DataSetの型変換
    ''' </summary>
    ''' <param name="proccessInfo">ストール実績情報</param>
    ''' <returns>型変換後のストール実績情報</returns>
    ''' <remarks></remarks>
    ''' 
    ''' <History>
    ''' </History>
    Private Function CastProcessInfo(ByVal proccessInfo As IC3810501DataSet.IC3810501StallProcessInfoDataTable) As SC3150101DataSet.SC3150101StallProcessInfoDataTable

        Logger.Info("[S]CastProcessInfo()")

        Using dt As New SC3150101DataSet.SC3150101StallProcessInfoDataTable

            ' DataSetの型を変換する（IC3810501StallProcessInfoDataTable→SC3150101StallProcessInfoDataTable）
            For Each drProcessInfo As DataRow In proccessInfo.Rows
                Dim dr As DataRow = dt.NewRow
                ' 項目名が同じ場合、値を代入する。
                For Each drProcessInfoColumn As DataColumn In proccessInfo.Columns
                    If (dt.Columns.Contains(drProcessInfoColumn.ColumnName) = True) Then
                        dr(drProcessInfoColumn.ColumnName) = drProcessInfo(drProcessInfoColumn.ColumnName)
                    End If
                Next
                dt.Rows.Add(dr)
            Next

            Return dt

        End Using

        Logger.Info("[E]CastProcessInfo()")

    End Function

    ''' <summary>
    ''' ストール予約情報の初期値設定
    ''' </summary>
    ''' <param name="reserveInfo">ストール予約情報</param>
    ''' <remarks></remarks>
    ''' 
    ''' <History>
    ''' </History>
    Private Sub InitReserveInfo(ByVal reserveInfo As SC3150101DataSet.SC3150101StallReserveInfoDataTable)

        Logger.Info("[S]InitReserveInfo()")

        ' ストール予約情報のNULL値変換
        For Each dr As SC3150101DataSet.SC3150101StallReserveInfoRow In reserveInfo.Rows
            dr("DLRCD") = SetData(dr("DLRCD"), "")
            dr("STRCD") = SetData(dr("STRCD"), "")
            dr("REZID") = SetNum(dr("REZID"), 0)
            dr("STALLID") = SetNum(dr("STALLID"), 0)
            dr("STARTTIME") = SetData(dr("STARTTIME"), DateTime.MinValue)
            dr("ENDTIME") = SetData(dr("ENDTIME"), DateTime.MinValue)
            dr("REZ_WORK_TIME") = SetNum(dr("REZ_WORK_TIME"), 0)
            dr("REZ_RECEPTION") = SetNum(dr("REZ_RECEPTION"), 0)
            dr("REZ_PICK_DATE") = SetData(dr("REZ_PICK_DATE"), "")
            dr("REZ_PICK_LOC") = SetData(dr("REZ_PICK_LOC"), "")
            dr("REZ_PICK_TIME") = SetNum(dr("REZ_PICK_TIME"), 0)
            dr("REZ_DELI_DATE") = SetData(dr("REZ_DELI_DATE"), "")
            dr("REZ_DELI_LOC") = SetData(dr("REZ_DELI_LOC"), "")
            dr("REZ_DELI_TIME") = SetNum(dr("REZ_DELI_TIME"), 0)
            dr("STATUS") = SetNum(dr("STATUS"), 0)
            dr("STRDATE") = SetData(dr("STRDATE"), DateTime.MinValue)
            dr("WASHFLG") = SetNum(dr("WASHFLG"), 0)
            dr("INSPECTIONFLG") = SetNum(dr("INSPECTIONFLG"), 0)
            dr("STOPFLG") = SetNum(dr("STOPFLG"), 0)
            dr("CANCELFLG") = SetNum(dr("CANCELFLG"), 0)
            dr("DELIVERY_FLG") = SetNum(dr("DELIVERY_FLG"), 0)
        Next

        Logger.Info("[E]InitReserveInfo()")

    End Sub

    ' 2012/02/06 KN 佐藤 【SERVICE_1】チップの移動時に休憩時間を考慮（処理追加） START
    ''' <summary>
    ''' ストール実績情報の初期値設定
    ''' </summary>
    ''' <param name="ProcessInfo">ストール実績リスト情報</param>
    ''' <remarks></remarks>
    ''' 
    ''' <History>
    ''' </History>
    Private Sub InitProcessInfo(ByVal ProcessInfo As SC3150101DataSet.SC3150101StallProcessListDataTable)

        Logger.Info("[S]InitProcessInfo()")

        ' ストール実績情報のNULL値変換
        For Each dr As SC3150101DataSet.SC3150101StallProcessListRow In ProcessInfo.Rows
            dr("DLRCD") = SetData(dr("DLRCD"), "")
            dr("STRCD") = SetData(dr("STRCD"), "")
            dr("REZID") = SetNum(dr("REZID"), 0)
            dr("DSEQNO") = SetNum(dr("DSEQNO"), 0)
            dr("SEQNO") = SetNum(dr("SEQNO"), 0)
            dr("RESULT_STATUS") = SetData(dr("RESULT_STATUS"), "0")
            dr("RESULT_STALLID") = SetNum(dr("RESULT_STALLID"), 0)
            dr("RESULT_START_TIME") = SetData(dr("RESULT_START_TIME"), "")
            dr("RESULT_END_TIME") = SetData(dr("RESULT_END_TIME"), "")
            dr("RESULT_WORK_TIME") = SetNum(dr("RESULT_WORK_TIME"), 0)
            dr("RESULT_IN_TIME") = SetData(dr("RESULT_IN_TIME"), "")
            dr("RESULT_WASH_START") = SetData(dr("RESULT_WASH_START"), "")
            dr("RESULT_WASH_END") = SetData(dr("RESULT_WASH_END"), "")
            dr("RESULT_INSPECTION_START") = SetData(dr("RESULT_INSPECTION_START"), "")
            dr("RESULT_INSPECTION_END") = SetData(dr("RESULT_INSPECTION_END"), "")
            dr("RESULT_WAIT_START") = SetData(dr("RESULT_WAIT_START"), "")
            dr("RESULT_WAIT_END") = SetData(dr("RESULT_WAIT_END"), "")
        Next

        Logger.Info("[E]InitProcessInfo()")

    End Sub
    ' 2012/02/06 KN 佐藤 【SERVICE_1】チップの移動時に休憩時間を考慮（処理追加） END

    ''' <summary>
    ''' YYYYMMDDHHMMの形式の文字列をDateTime型に変換する
    ''' </summary>
    ''' <param name="value">変換対象文字列</param>
    ''' <returns></returns>
    ''' <remarks>DataTime型に変換した値</remarks>
    ''' 
    ''' <History>
    ''' </History>
    Public Function SetStallTime(ByVal value As String) As DateTime

        Dim returnValue As DateTime = DateTime.Now
        Dim hour As Integer
        Dim minute As Integer
        Dim retValue As DateTime

        'If IsDBNull(value) = True Then
        '    Return DateTime.MinValue
        'End If
        'If value.Trim() = "" Then
        If (String.IsNullOrEmpty(value) = True) _
           OrElse (value.Trim.Length = 0) Then
            Return DateTime.MinValue
        End If
        hour = CType(value.Substring(0, 2), Integer)
        minute = CType(value.Substring(3, 2), Integer)

        retValue = New DateTime(returnValue.Year, returnValue.Month, returnValue.Day, hour, minute, 0)

        Return retValue

    End Function

    ''' <summary>
    ''' NULL値をデフォルト値で置き換える
    ''' </summary>
    ''' <param name="value">変換対象データ</param>
    ''' <param name="defaltValue">デフォルト値</param>
    ''' <returns>変換後の値</returns>
    ''' <remarks></remarks>
    Protected Function SetData(ByVal value As Object, ByVal defaltValue As Object) As Object
        If (IsDBNull(value) = True) Then
            Return defaltValue
        End If
        Return value
    End Function

    ''' <summary>
    ''' 数値以外をデフォルト値で置き換える
    ''' </summary>
    ''' <param name="value">変換対象データ</param>
    ''' <param name="defaltValue">デフォルト値</param>
    ''' <returns>変換後の値</returns>
    ''' <remarks></remarks>
    Protected Function SetNum(ByVal value As Object, ByVal defaltValue As Object) As Object
        If (IsNumeric(value) = False) Then
            Return defaltValue
        End If
        Return value
    End Function

    ''' <summary>
    ''' 日付変換
    ''' </summary>
    ''' <param name="value">変換対象文字列</param>
    ''' <returns></returns>
    ''' <remarks>Date型に変換した値</remarks>
    ''' 
    ''' <History>
    ''' </History>
    Public Function ParseDate(ByVal value As String) As Date

        Dim returnValue As Date
        Dim year As Integer = Integer.Parse(value.Substring(0, 4), CultureInfo.CurrentCulture)
        Dim month As Integer = Integer.Parse(value.Substring(4, 2), CultureInfo.CurrentCulture)
        Dim day As Integer = Integer.Parse(value.Substring(6, 2), CultureInfo.CurrentCulture)
        Dim hour As Integer = Integer.Parse(value.Substring(8, 2), CultureInfo.CurrentCulture)
        Dim minute As Integer = Integer.Parse(value.Substring(10, 2), CultureInfo.CurrentCulture)

        returnValue = New Date(year, month, day, hour, minute, 0)

        Return returnValue

    End Function

    ''' <summary>
    ''' オブジェクトの文字列値を取得し返却する
    ''' </summary>
    ''' <param name="value">DBから取得した文字列 or DBNull</param>
    ''' <returns>文字列。DBNullの場合、空文字列</returns>
    ''' 
    ''' <History>
    ''' </History>
    Public Shared Function StringValueOfDB(ByVal value As Object) As String

        If (Convert.IsDBNull(value)) Then
            Return String.Empty
        End If

        Return CStr(value)

    End Function

#End Region

#Region "衝突チップの移動処理"
    ' 2012/02/06 KN 佐藤 【SERVICE_1】チップの移動時に休憩時間を考慮（処理修正） START
    ''' <summary>
    ''' 衝突チップを移動する
    ''' </summary>
    ''' <param name="reserveList">ストール予約情報のDataSet</param>
    ''' <param name="drReserveInfo">ストール予約情報のDataRow</param>
    ''' <param name="stallTimeInfo">ストール時間のDataSet</param>
    ''' <param name="breakInfo">休憩情報</param>
    ''' <param name="startTimeReserve">作業開始時刻</param>
    ''' <param name="endTimeReserve">作業終了時刻</param>
    ''' <param name="account">更新アカウント</param>
    ''' <returns>処理結果</returns>
    ''' 
    ''' <History>
    ''' </History>
    Private Function MoveCollisionChip(ByVal reserveList As SC3150101DataSet.SC3150101StallReserveListDataTable, _
                            ByVal drReserveInfo As SC3150101DataSet.SC3150101StallReserveInfoRow, _
                            ByVal stallTimeInfo As SC3150101DataSet.SC3150101StallTimeInfoDataTable, _
                            ByVal breakInfo As SC3150101DataSet.SC3150101StallBreakInfoDataTable, _
                            ByVal startTimeReserve As Date, _
                            ByVal endTimeReserve As Date, _
                            ByVal account As String) As Integer
        'Private Function MoveCollisionChip(ByVal reserveList As SC3150101DataSet.SC3150101StallReserveListDataTable, _
        '                                ByVal drReserveInfo As SC3150101DataSet.SC3150101StallReserveInfoRow, _
        '                                ByVal stallTimeInfo As SC3150101DataSet.SC3150101StallTimeInfoDataTable, _
        '                                ByVal startTimeReserve As Date, _
        '                                ByVal endTimeReserve As Date, _
        '                                ByVal account As String) As Integer
        ' 2012/02/06 KN 佐藤 【SERVICE_1】チップの移動時に休憩時間を考慮（処理修正） END

        Logger.Info("[S]MoveCollisionChip()")

        ' ストール予約の中断処理クラスを定義
        Dim stallWork As New SC3150101.SC3150101BusinessLogic
        ' 2012/02/25 KN 佐藤 【SERVICE_1】エラーの戻り値を修正（処理修正） START
        ' 戻り値の初期化
        MoveCollisionChip = RETURN_VALUE_NG
        ' 2012/02/25 KN 佐藤 【SERVICE_1】エラーの戻り値を修正（処理修正） END
        Try
            ' 衝突有無判定
            If stallWork.IsCollision(reserveList,
                                        CType(drReserveInfo.REZID, Integer), _
                                        drReserveInfo.STARTTIME, _
                                        endTimeReserve) = True Then

                ' 指定時間への予約の移動
                ' 2012/02/06 KN 佐藤 【SERVICE_1】チップの移動時に休憩時間を考慮（処理修正） START
                'Using reserveListTemp As SC3150101DataSet.SC3150101StallReserveListDataTable = _
                '                stallWork.MoveReserve(reserveList, _
                '                                        stallTimeInfo, _
                '                                        breakInfo, _
                '                                        drReserveInfo.DLRCD, _
                '                                        drReserveInfo.STRCD, _
                '                                        CType(drReserveInfo.REZID, Integer), _
                '                                        CType(drReserveInfo.STALLID, Integer), _
                '                                        startTimeReserve, _
                '                                        endTimeReserve)
                Using reserveListTemp As SC3150101DataSet.SC3150101StallReserveListDataTable = _
                                stallWork.MoveReserve(reserveList, _
                                                        stallTimeInfo, _
                                                        breakInfo, _
                                                        drReserveInfo.DLRCD, _
                                                        drReserveInfo.STRCD, _
                                                        CType(drReserveInfo.REZID, Integer), _
                                                        CType(drReserveInfo.STALLID, Integer), _
                                                        startTimeReserve, _
                                                        endTimeReserve)
                    ' 2012/02/06 KN 佐藤 【SERVICE_1】チップの移動時に休憩時間を考慮（処理修正） END
                    If (reserveListTemp Is Nothing) Then
                        ' 2012/02/25 KN 佐藤 【SERVICE_1】エラーの戻り値を修正（処理修正） START
                        'Return RETURN_VALUE_NG
                        MoveCollisionChip = RETURN_VALUE_MOVE_CHIP_ERROR
                        Exit Try
                        ' 2012/02/25 KN 佐藤 【SERVICE_1】エラーの戻り値を修正（処理修正） END
                    End If

                    ' 時間に変更のあった予約情報の更新
                    Dim result As Integer
                    result = stallWork.UpdateAllReserve(reserveListTemp, _
                                                        CType(drReserveInfo.REZID, Integer), _
                                                        drReserveInfo.DLRCD, _
                                                        drReserveInfo.STRCD, _
                                                        CType(drReserveInfo.STALLID, Integer), _
                                                        account)
                    If (result < RETURN_VALUE_OK) Then
                        ' 2012/02/25 KN 佐藤 【SERVICE_1】エラーの戻り値を修正（処理修正） START
                        'Return RETURN_VALUE_NG
                        Exit Try
                        ' 2012/02/25 KN 佐藤 【SERVICE_1】エラーの戻り値を修正（処理修正） END
                    End If
                End Using
            End If
            ' 2012/02/25 KN 佐藤 【SERVICE_1】エラーの戻り値を修正（処理追加） START
            MoveCollisionChip = RETURN_VALUE_OK
            ' 2012/02/25 KN 佐藤 【SERVICE_1】エラーの戻り値を修正（処理追加） END

            Logger.Info("[E]MoveCollisionChip()")

        Finally
            ' メモリの解放
            If (Not stallWork Is Nothing) Then
                stallWork = Nothing
            End If
        End Try

        ' 2012/02/25 KN 佐藤 【SERVICE_1】エラーの戻り値を修正（処理修正） START
        'Return RETURN_VALUE_OK
        Return MoveCollisionChip
        ' 2012/02/25 KN 佐藤 【SERVICE_1】エラーの戻り値を修正（処理修正） END

    End Function
#End Region

#Region "実績ステータスの更新処理"
    ''' <summary>
    ''' 実績ステータスの更新処理
    ''' </summary>
    ''' <param name="adapter">SC3150101のTableAdapter</param>
    ''' <param name="drReserveInfo">ストール予約情報</param>
    ''' <param name="drProcInfo">ストール実績情報</param>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Private Function UpdateResultStatus(ByVal adapter As SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter, _
                                        ByVal drReserveInfo As SC3150101DataSet.SC3150101StallReserveInfoRow, _
                                        ByVal drProcInfo As SC3150101DataSet.SC3150101StallProcessInfoRow, _
                                        ByVal dealerCode As String, _
                                        ByVal branchCode As String, _
                                        ByVal endTime As Date) As Integer

        Logger.Info("[S]UpdateResultStatus()")

        ' 2012/02/25 KN 佐藤 【SERVICE_1】エラーの戻り値を修正（処理修正） START
        ' 戻り値の設定
        UpdateResultStatus = RETURN_VALUE_NG
        ' 2012/02/25 KN 佐藤 【SERVICE_1】エラーの戻り値を修正（処理修正） END

        ' 最終チップを取得する
        Dim lastChip As IC3810501DataSet.IC3810501GetLastChipDataTable
        Using da As New IC3810501DataSetTableAdapters.IC3810501StallInfoDataTableAdapter
            lastChip = da.GetLastChip(dealerCode, _
                                        branchCode, _
                                        CType(drReserveInfo.REZID, Integer))
        End Using
        Dim drLastChip As IC3810501DataSet.IC3810501GetLastChipRow
        drLastChip = DirectCast(lastChip.Rows(0), IC3810501DataSet.IC3810501GetLastChipRow)

        ' 予約IDが最終チップ以外の場合
        If (drLastChip.REZID <> drReserveInfo.REZID) Then
            ' 実績_ステータスを「関連チップの前工程作業終了」にする
            drProcInfo.RESULT_STATUS = RESULT_STATUS_BEFORE_THE_END_OF_STEP
        Else
            '予約IDが最終チップの場合

            ' 販売店環境設定値を取得する(検査エリア使用フラグ取得)
            Using envSettingUseFlg As SC3150101DataSet.SC3150101DealerEnvironmentSettingInfoDataTable = _
                    adapter.GetDealerEnvironmentSettingValue(dealerCode, branchCode, C_INSPECTION_USE_FLG)
                Dim drEnvSettingUseFlg As SC3150101DataSet.SC3150101DealerEnvironmentSettingInfoRow
                drEnvSettingUseFlg = DirectCast(envSettingUseFlg.Rows(0), SC3150101DataSet.SC3150101DealerEnvironmentSettingInfoRow)

                ' 検査エリア使用フラグ＝使用しない、検査フラグ＝検査する場合、エラー
                'If drEnvSettingUseFlg.PARAMVALUE <> "1" AndAlso drReserveInfo.INSPECTIONFLG = "1" Then
                If String.Equals(drEnvSettingUseFlg.PARAMVALUE, "1", StringComparison.CurrentCulture) = False _
                    AndAlso String.Equals(drReserveInfo.INSPECTIONFLG, "1", StringComparison.CurrentCulture) = True Then
                    ' 2012/02/25 KN 佐藤 【SERVICE_1】エラーの戻り値を修正（処理修正） START
                    'Return RETURN_VALUE_NG
                    Return RETURN_VALUE_DATA_INCONSISTENCIES
                    ' 2012/02/25 KN 佐藤 【SERVICE_1】エラーの戻り値を修正（処理修正） END
                End If

                ' 洗車フラグ＝洗車する、検査フラグ＝検査するの場合
                ' If drReserveInfo.WASHFLG = "1" AndAlso drReserveInfo.INSPECTIONFLG = "1" Then
                If String.Equals(drReserveInfo.WASHFLG, "1", StringComparison.CurrentCulture) = True _
                    AndAlso String.Equals(drReserveInfo.INSPECTIONFLG, "1", StringComparison.CurrentCulture) = True Then
                    ' 販売店環境設定値取得(検査順フラグ取得)
                    Using envSettingOrderFlg As SC3150101DataSet.SC3150101DealerEnvironmentSettingInfoDataTable = _
                            adapter.GetDealerEnvironmentSettingValue(dealerCode, branchCode, C_SMB_INSPECTION_ORDER_FLG)
                        Dim drEnvSettingOrderFlg As SC3150101DataSet.SC3150101DealerEnvironmentSettingInfoRow
                        drEnvSettingOrderFlg = DirectCast(envSettingOrderFlg.Rows(0), SC3150101DataSet.SC3150101DealerEnvironmentSettingInfoRow)

                        ' 検査順が洗車→検査の場合
                        'If drEnvSettingOrderFlg.PARAMVALUE = "1" Then
                        If String.Equals(drEnvSettingOrderFlg.PARAMVALUE, "1", StringComparison.CurrentCulture) = True Then
                            ' 実績_ステータスを「洗車待ち」にする
                            drProcInfo.RESULT_STATUS = RESULT_STATUS_WAITING_FOR_CAR_WASH
                        Else
                            ' 検査順が検査→洗車の場合

                            ' 実績_ステータスを「検査待ち」にする
                            drProcInfo.RESULT_STATUS = RESULT_STATUS_WAIT_FOR_INSPECTION
                        End If
                    End Using

                    ' 洗車フラグ＝洗車する、検査フラグ＝検査しないの場合
                    'ElseIf drReserveInfo.WASHFLG = "1" AndAlso drReserveInfo.INSPECTIONFLG <> "1" Then
                ElseIf String.Equals(drReserveInfo.WASHFLG, "1", StringComparison.CurrentCulture) = True _
                    AndAlso String.Equals(drReserveInfo.INSPECTIONFLG, "1", StringComparison.CurrentCulture) = False Then
                    ' 実績_ステータスを「洗車待ち」にする
                    drProcInfo.RESULT_STATUS = RESULT_STATUS_WAITING_FOR_CAR_WASH

                    ' 洗車フラグ＝洗車しない、検査フラグ＝検査するの場合
                    'ElseIf drReserveInfo.WASHFLG <> "1" AndAlso drReserveInfo.INSPECTIONFLG = "1" Then
                ElseIf String.Equals(drReserveInfo.WASHFLG, "1", StringComparison.CurrentCulture) = False _
                    AndAlso String.Equals(drReserveInfo.INSPECTIONFLG, "1", StringComparison.CurrentCulture) = True Then
                    ' 実績_ステータスを「検査待ち」にする
                    drProcInfo.RESULT_STATUS = RESULT_STATUS_WAIT_FOR_INSPECTION

                    ' 洗車フラグ＝洗車しない、検査フラグ＝検査しないの場合
                Else
                    ' 予約_受付納車区分が「納車待ち」の場合
                    'If drReserveInfo.REZ_RECEPTION = "0" Then
                    If String.Equals(drReserveInfo.REZ_RECEPTION, "0", StringComparison.CurrentCulture) = True Then
                        ' 実績_ステータスを「預かり中」にする
                        drProcInfo.RESULT_STATUS = RESULT_STATUS_IN_CUSTODY
                    Else
                        ' 実績_ステータスを「納車待ち」にする
                        drProcInfo.RESULT_STATUS = RESULT_STATUS_WAIT_FOR_CAR_DELIVERY
                    End If
                    ' 納車待ち開始時刻に作業終了時刻を設定する。
                    drProcInfo.RESULT_WAIT_START = endTime.ToString("yyyyMMddHHmm", CultureInfo.CurrentCulture)
                End If
            End Using
        End If

        Logger.Info("[E]UpdateResultStatus()")

        Return RETURN_VALUE_OK

    End Function
#End Region

#Region "担当者ストール実績データの更新"
    ''' <summary>
    ''' 担当者ストール実績データの更新
    ''' </summary>
    ''' <param name="adapter">SC3150101TableAdapterクラス</param>
    ''' <param name="drReserveInfo">ストール予約情報</param>
    ''' <param name="stallTimeInfo">ストール時間情報</param>
    ''' <param name="procStartTime">実績の作業開始日時</param>
    ''' <returns>処理結果（正常：0、エラー：-1）</returns>
    ''' <remarks></remarks>
    Private Function UpdateStaffStall(ByVal adapter As SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter _
                                      , ByVal drReserveInfo As SC3150101DataSet.SC3150101StallReserveInfoRow _
                                      , ByVal stallTimeInfo As SC3150101DataSet.SC3150101StallTimeInfoDataTable _
                                      , ByVal procStartTime As Date) As Long

        Logger.Info("[S]UpdateStaffStall()")

        ' 作業日付を取得する
        Dim workTime As Date = GetWorkDate(stallTimeInfo, procStartTime)

        ' 担当者ストール実績データ情報を取得する()
        Dim staffResultInfo As SC3150101DataSet.SC3150101StaffResultInfoDataTable = adapter.GetStaffResultInfo(CType(drReserveInfo.STALLID, Integer), _
                                                        CType(drReserveInfo.REZID, Integer), _
                                                        workTime)
        Dim drStaffResultInfo As SC3150101DataSet.SC3150101StaffResultInfoRow = DirectCast(staffResultInfo.Rows(0), SC3150101DataSet.SC3150101StaffResultInfoRow)

        ' 2012/02/13 KN 佐藤 【SERVICE_1】担当者ストール実績で作業終了日付を更新するように修正（処理修正） START
        ' 作業終了日付を取得する
        Dim workEndTime As String = Nothing
        If drStaffResultInfo.IsRESULT_END_TIMENull = False Then
            workEndTime = drStaffResultInfo.RESULT_END_TIME
        End If
        ' 2012/02/13 KN 佐藤 【SERVICE_1】担当者ストール実績で作業終了日付を更新するように修正（処理修正） END

        ' 担当者ストール実績データの更新
        ' 2012/02/13 KN 佐藤 【SERVICE_1】担当者ストール実績で作業終了日付を更新するように修正（処理修正） START
        'If (adapter.UpdateStaffStallAtWork(CType(drReserveInfo.STALLID, Integer), _
        '                                                            CType(drReserveInfo.REZID, Integer), _
        '                                                            CType(drStaffResultInfo.DSEQNO, Integer), _
        '                                                            CType(drStaffResultInfo.SEQNO, Integer), _
        '                                                            workTime) <= 0) Then
        '    Return RETURN_VALUE_NG
        'End If
        If (adapter.UpdateStaffStall(CType(drReserveInfo.STALLID, Integer), _
                                     CType(drReserveInfo.REZID, Integer), _
                                     CType(drStaffResultInfo.DSEQNO, Integer), _
                                     CType(drStaffResultInfo.SEQNO, Integer), _
                                     workTime, _
                                     workEndTime) <= 0) Then
            Return RETURN_VALUE_NG
        End If
        ' 2012/02/13 KN 佐藤 【SERVICE_1】担当者ストール実績で作業終了日付を更新するように修正（処理修正） END

        Logger.Info("[E]UpdateStaffStall()")

        Return RETURN_VALUE_OK

    End Function
#End Region

#Region "洗車順データの更新"
    ''' <summary>
    ''' 洗車順データの更新
    ''' </summary>
    ''' <param name="drProcInfo">ストール実績</param>
    ''' <returns>処理結果（正常：0、エラー：-1）</returns>
    ''' <remarks></remarks>
    Private Function UpdateWash(ByVal drProcInfo As SC3150101DataSet.SC3150101StallProcessInfoRow) As Integer

        Logger.Info("[S]UpdateWash()")

        ' 洗車順データを更新する
        Using da As New IC3810501DataSetTableAdapters.IC3810501StallInfoDataTableAdapter
            ' 最新のスール実績情報を取得する
            Dim washRefreshSeq As IC3810501DataSet.IC3810501WashRefreshSeqDataTable = da.GetWashRefreshSeq(drProcInfo.DLRCD, drProcInfo.STRCD)
            ' 洗車順データを削除する
            da.DeleteWashData(drProcInfo.DLRCD, drProcInfo.STRCD)
            ' 洗車順データを追加する
            For i As Integer = 0 To washRefreshSeq.Rows.Count - 1
                If da.InsertWashData(drProcInfo.DLRCD, _
                                        drProcInfo.STRCD, _
                                        CType(StringValueOfDB(washRefreshSeq.Rows.Item(i).Item("REZID")), Integer), _
                                        CType(StringValueOfDB(washRefreshSeq.Rows.Item(i).Item("SEQNO")), Integer), _
                                        i) <= 0 Then
                    Return RETURN_VALUE_NG
                End If
            Next
        End Using

        Logger.Info("[E]UpdateWash()")

        Return RETURN_VALUE_OK

    End Function
#End Region

#Region "検査順データの更新"
    ''' <summary>
    ''' 検査順データの更新
    ''' </summary>
    ''' <param name="drProcInfo">ストール実績</param>
    ''' <returns>処理結果（正常：0、エラー：-1）</returns>
    ''' <remarks></remarks>
    Private Function UpdateInspection(ByVal drProcInfo As SC3150101DataSet.SC3150101StallProcessInfoRow) As Integer

        Logger.Info("[S]UpdateInspection()")

        ' 検査順データを更新する
        Using da As New IC3810501DataSetTableAdapters.IC3810501StallInfoDataTableAdapter
            ' 最新のスール実績情報を取得する
            Dim inspectionRefreshSeq As IC3810501DataSet.IC3810501InspectionRefreshSeqDataTable = da.GetInspectionRefreshSeq(drProcInfo.DLRCD, drProcInfo.STRCD)
            ' 検査順データを削除する
            da.DeleteInspectionData(drProcInfo.DLRCD, drProcInfo.STRCD)
            ' 検査順データを追加する
            For i As Integer = 0 To inspectionRefreshSeq.Rows.Count - 1
                If da.InsertInspectionData(drProcInfo.DLRCD, _
                                            drProcInfo.STRCD, _
                                            CType(StringValueOfDB(inspectionRefreshSeq.Rows.Item(i).Item("REZID")), Integer), _
                                            i) <= 0 Then
                    Return RETURN_VALUE_NG
                End If
            Next
        End Using

        Logger.Info("[E]UpdateInspection()")

        Return RETURN_VALUE_OK

    End Function
#End Region

#Region "ログ出力"
    ''' <summary>
    ''' ログを出力する
    ''' </summary>
    ''' <param name="logLevel">ログレベル</param>
    ''' <param name="functionName">関数名</param>
    ''' <param name="message">メッセージ</param>
    ''' <param name="ex">例外</param>
    ''' <param name="values">パラメータ</param>
    ''' <remarks></remarks>
    Private Sub OutputLog(ByVal logLevel As String, _
                            ByVal functionName As String, _
                            ByVal message As String, _
                            ByVal ex As Exception, _
                            ByVal ParamArray values() As String)

        Dim i As Integer
        Dim logMessage As String = ""

        For i = 0 To values.Length() - 1
            logMessage = logMessage + "[" & values(i) & "]"
        Next i

        'If logLevel = "I" Then
        If String.Equals(logLevel, "I", StringComparison.CurrentCulture) = True Then
            Logger.Info(functionName & " " & logMessage & " " & message)
            'ElseIf logLevel = "E" Then
        ElseIf String.Equals(logLevel, "E", StringComparison.CurrentCulture) = True Then
            If ex Is Nothing Then
                Logger.Error(message & "[FUNC:" & functionName & "]")
            Else
                Logger.Error(message & "[FUNC:" & functionName & "]", ex)
            End If
            'ElseIf logLevel = "W" Then
        ElseIf String.Equals(logLevel, "W", StringComparison.CurrentCulture) = True Then
            Logger.Warn(message)
        End If

    End Sub
#End Region

#Region "IDisposable Support"
    Private disposedValue As Boolean ' 重複する呼び出しを検出するには

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: マネージ状態を破棄します (マネージ オブジェクト)。
            End If

            ' TODO: アンマネージ リソース (アンマネージ オブジェクト) を解放し、下の Finalize() をオーバーライドします。
            ' TODO: 大きなフィールドを null に設定します。
        End If
        Me.disposedValue = True
    End Sub

    ' このコードは、破棄可能なパターンを正しく実装できるように Visual Basic によって追加されました。
    Public Sub Dispose() Implements IDisposable.Dispose
        ' このコードを変更しないでください。クリーンアップ コードを上の Dispose(ByVal disposing As Boolean) に記述します。
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

#End Region

End Class
