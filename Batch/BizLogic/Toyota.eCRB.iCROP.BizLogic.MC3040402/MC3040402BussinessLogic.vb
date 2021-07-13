Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Batch
Imports Toyota.eCRB.iCROP.DataAccess.MC3040402

Namespace MC3040402BizLogic
    ''' <summary>
    ''' カレンダー情報削除バッチ
    ''' </summary>
    ''' <remarks></remarks>
    Public Class MC3040402BussinessLogic
        Inherits BaseBusinessComponent
        Implements IDisposable

#Region "定数定義"

        ''' <summary>
        ''' モジュールID
        ''' </summary>
        ''' <remarks></remarks>
        Private Const MODULEID As String = "MC3040402"
        ''' <summary>
        ''' INIファイルのセクション名
        ''' </summary>
        ''' <remarks></remarks>
        Private Const INI_SECTION_MC3040402 As String = "MC3040402"
        ''' <summary>
        ''' INIファイルの退避期間キー名
        ''' </summary>
        ''' <remarks></remarks>
        Private Const INI_KEY_PASTTARGET As String = "PastTarget"
        ''' <summary>
        ''' 退避期間のデフォルト値
        ''' </summary>
        ''' <remarks></remarks>
        Private Const INI_DEFAULT_PASTTARGET As String = "30"
        ''' <summary>
        ''' 空文字列
        ''' </summary>
        ''' <remarks></remarks>
        Private Const EmptyString As String = ""

#Region "ログ出力レベル"
        Private Enum LOGLEVEL
            ERR = 1
            INFO = 2
            DEBUG = 3
        End Enum
#End Region

#Region "ログ出力メッセージID"
        Private Enum MSGID
            ''' <summary>
            ''' メッセージIDなしメッセージID
            ''' </summary>
            ''' <remarks></remarks>
            MSG_000 = 0

            ''' <summary>
            ''' MovePastCalendar関数用メッセージID
            ''' </summary>
            ''' <remarks></remarks>
            MSG_101 = 101
            MSG_102 = 102
            MSG_103 = 103

            ''' <summary>
            ''' TruncateWkTable関数用メッセージID
            ''' </summary>
            ''' <remarks></remarks>
            MSG_201 = 201
            MSG_202 = 202
            MSG_203 = 203
            MSG_204 = 204

            ''' <summary>
            ''' MovePastProc関数用メッセージID
            ''' </summary>
            ''' <remarks></remarks>
            MSG_301 = 301
            MSG_302 = 302

            ''' <summary>
            ''' ExtractedPastTargetKey関数用メッセージID
            ''' </summary>
            ''' <remarks></remarks>
            MSG_401 = 401
            MSG_402 = 402
            MSG_403 = 403
            MSG_404 = 404
            MSG_405 = 405
            MSG_406 = 406

            ''' <summary>
            ''' InsertPastTable関数用メッセージ
            ''' </summary>
            ''' <remarks></remarks>
            MSG_501 = 501
            MSG_502 = 502
            MSG_503 = 503
            MSG_504 = 504
            MSG_505 = 505
            MSG_506 = 506
            MSG_507 = 507
            MSG_508 = 508
            MSG_509 = 509
            MSG_510 = 510
            MSG_511 = 511

            ''' <summary>
            ''' DeleteTodoEvent関数用メッセージID
            ''' </summary>
            ''' <remarks></remarks>
            MSG_601 = 601
            MSG_602 = 602
            MSG_603 = 603
            MSG_604 = 604
            MSG_605 = 605
            MSG_606 = 606
            MSG_607 = 607
            MSG_608 = 608
            MSG_609 = 609
            MSG_610 = 610
            MSG_611 = 611

            ''' <summary>
            ''' MoveIcropInfo関数用メッセージID
            ''' </summary>
            ''' <remarks></remarks>
            MSG_701 = 701
            MSG_702 = 702
            MSG_703 = 703
            MSG_704 = 704
            MSG_705 = 705
        End Enum
#End Region
#End Region

#Region "メイン関数"
        ''' <summary>
        ''' ワークテーブルのトランケート処理
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function TruncateWKTable() As Boolean

            Try
                OutputLog(LOGLEVEL.INFO, MSGID.MSG_201, EmptyString)

                ' カレンダーTODO退避のトランケート
                Using adapter As New MC3040402DataSetTableAdapters.CalWKTodoPastAdapter
                    adapter.TruncateWKTodoPast()
                    OutputLog(LOGLEVEL.INFO, MSGID.MSG_203, EmptyString)
                End Using

                ' カレンダーイベント退避ワークのトランケート
                Using adapter As New MC3040402DataSetTableAdapters.CalWKEventPastAdapter
                    adapter.TruncateWKEventPast()
                    OutputLog(LOGLEVEL.INFO, MSGID.MSG_204, EmptyString)
                End Using

                OutputLog(LOGLEVEL.INFO, MSGID.MSG_202, EmptyString)

                Return True

            Catch ex As OracleExceptionEx

                OutputLog(LOGLEVEL.INFO, MSGID.MSG_000, "ErrorSQL:" + ex.CommandText)
                OutputLog(LOGLEVEL.ERR, MSGID.MSG_000, EmptyString, ex)

                ' 異常終了
                Return False

            Catch ex As ApplicationException
                OutputLog(LOGLEVEL.ERR, MSGID.MSG_000, EmptyString, ex)

                ' 異常終了
                Return False

            Catch ex As SystemException
                OutputLog(LOGLEVEL.ERR, MSGID.MSG_000, EmptyString, ex)

                ' 異常終了
                Return False

            End Try

        End Function

        ''' <summary>
        ''' 退避処理
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <EnableCommit()>
        Public Function MovePastProc() As Boolean

            Try

                OutputLog(LOGLEVEL.INFO, MSGID.MSG_301, EmptyString)

                ' 退避対象抽出処理
                ExtractedPastTargetKey()

                ' 退避テーブルへのレコード追加処理
                InsertPastTable()

                ' 退避済みのTodo、イベントの削除処理
                DeleteTodoEvent()

                ' カレンダーiCROP情報テーブル退避処理
                MoveIcropInfo()

                OutputLog(LOGLEVEL.INFO, MSGID.MSG_302, EmptyString)

                Return True

            Catch ex As OracleExceptionEx
                OutputLog(LOGLEVEL.INFO, MSGID.MSG_000, "ErrorSQL:" + ex.CommandText)
                OutputLog(LOGLEVEL.ERR, MSGID.MSG_000, EmptyString, ex)

                ' ロールバック
                Me.Rollback = True

                ' 異常終了
                Return False

            Catch ex As ApplicationException
                OutputLog(LOGLEVEL.ERR, MSGID.MSG_000, EmptyString, ex)

                'ロールバック
                Me.Rollback = True

                ' 異常終了
                Return False


            Catch ex As SystemException
                OutputLog(LOGLEVEL.ERR, MSGID.MSG_000, EmptyString, ex)

                'ロールバック
                Me.Rollback = True

                ' 異常終了
                Return False

            End Try

        End Function

        ''' <summary>
        ''' ログ出力処理
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub OutputLog(ByVal Level As Integer, ByVal wordNo As Long, ByVal parameter As String, Optional ByVal ex As Exception = Nothing)

            Dim LogMessage As String    ' 出力メッセージ

            If wordNo = MSGID.MSG_000 Then
                LogMessage = "%1"
            Else
                '出力メッセージの取得()
                LogMessage = BatchWordUtility.GetWord(MODULEID, wordNo)
                If LogMessage = EmptyString Then
                    LogMessage = "%1"
                End If
                ' メッセージの編集
                LogMessage = CType(wordNo, String) & ":" & LogMessage
            End If
            ' パラメータの置き換え
            LogMessage = LogMessage.Replace("%1", parameter)

            ' ログ出力
            If Level = LOGLEVEL.ERR Then
                ' ログレベル：ERROR
                Logger.Error(LogMessage, ex)
                Logger.Debug(ex.Message)
            ElseIf Level = LOGLEVEL.INFO Then
                'ログレベル：INFO
                Logger.Info(LogMessage)
                Logger.Debug(LogMessage)
            Else
                'ログレベル：DEBUG
                Logger.Debug(LogMessage)
            End If
        End Sub

        ''' <summary>
        ''' Disposeメソッド
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

#End Region

#Region "サブ関数"

        ''' <summary>
        ''' 退避対象キー抽出処理
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function ExtractedPastTargetKey() As Boolean

            OutputLog(LOGLEVEL.INFO, MSGID.MSG_401, EmptyString)

            ' INIファイルより退避期間の取得
            Dim PastaTarget As String = BatchSetting.GetValue(INI_SECTION_MC3040402, INI_KEY_PASTTARGET, INI_DEFAULT_PASTTARGET)
            OutputLog(LOGLEVEL.INFO, MSGID.MSG_403, PastaTarget)

            ' 退避対象日付
            Dim TargetPastDate As Date = DateTimeFunc.Now().Date.AddDays(CLng(PastaTarget) * -1)
            OutputLog(LOGLEVEL.INFO, MSGID.MSG_404, Format(TargetPastDate, "yyyy/MM/dd HH:mm:ss"))
            ' DataTable生成
            Using dataTable As New MC3040402DataSet.PastDateDataTable
                ' DataRow生成
                Dim dataRow As MC3040402DataSet.PastDateRow = dataTable.NewPastDateRow()
                With dataRow
                    .PASTDATE = TargetPastDate  ' 対象退避日付
                    .MODULEID = MODULEID        ' モジュールID
                End With

                Using adapter As New MC3040402DataSetTableAdapters.PastDateDataTable

                    ' 退避対象のTodoを退避する
                    Dim evacuationTodoCount As Long = adapter.InsertWKTodoPast(dataRow)
                    OutputLog(LOGLEVEL.INFO, MSGID.MSG_405, CType(evacuationTodoCount, String))

                    ' 退避対象のイベントを退避する
                    Dim evacuationEventCount = adapter.InsertWKEventPast(dataRow)
                    OutputLog(LOGLEVEL.INFO, MSGID.MSG_406, CType(evacuationEventCount, String))
                End Using

                OutputLog(LOGLEVEL.INFO, MSGID.MSG_402, EmptyString)

                Return True

            End Using

        End Function

        ''' <summary>
        ''' 退避テーブルへのレコード追加処理
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function InsertPastTable() As Boolean

            OutputLog(LOGLEVEL.INFO, MSGID.MSG_501, EmptyString)

            ' カレンダーイベントアラームの退避
            Using adapter As New MC3040402DataSetTableAdapters.CalEventAlarmPastAdapter
                Dim evacuationEventAlarmCount As Long = adapter.InsertEventAlarmPast
                OutputLog(LOGLEVEL.INFO, MSGID.MSG_503, CType(evacuationEventAlarmCount, String))
            End Using

            ' カレンダーイベント繰り返し除外日の退避
            Using adapter As New MC3040402DataSetTableAdapters.CalEventExDatePastAdapter
                Dim evacuationEventExDateCount As Long = adapter.InsertEventExdatePast
                OutputLog(LOGLEVEL.INFO, MSGID.MSG_504, CType(evacuationEventExDateCount, String))
            End Using

            ' カレンダーイベント情報の退避
            Using adapter As New MC3040402DataSetTableAdapters.CalEventItemPastAdapter
                Dim evacuationEventItemCount As Long = adapter.InsertEventItemPast()
                OutputLog(LOGLEVEL.INFO, MSGID.MSG_505, CType(evacuationEventItemCount, String))
            End Using

            ' カレンダーTodoアラームの退避
            Using adapter As New MC3040402DataSetTableAdapters.CalTodoAlarmPastAdapter
                Dim evacuationTodoAlarmCount As Long = adapter.InsertTodoAlarmPast
                OutputLog(LOGLEVEL.INFO, MSGID.MSG_506, CType(evacuationTodoAlarmCount, String))
            End Using

            ' カレンダーTODO繰り返し除外日の退避
            Using adapter As New MC3040402DataSetTableAdapters.CalTodoExdatePastAdapter
                Dim evacuationTodoExDateCount As Long = adapter.InsertTodoExdatePast
                OutputLog(LOGLEVEL.INFO, MSGID.MSG_507, CType(evacuationTodoExDateCount, String))
            End Using

            ' カレンダーTODO情報の退避
            Using adapter As New MC3040402DataSetTableAdapters.CalTodoItemPastAdapter
                Dim evacuationTodoItemCount As Long = adapter.InsertTodoItemPast()
                OutputLog(LOGLEVEL.INFO, MSGID.MSG_508, CType(evacuationTodoItemCount, String))
            End Using


            ' カレンダーイベントアラームの退避(Native分)
            Using adapter As New MC3040402DataSetTableAdapters.CalEventAlarmPastAdapter
                Dim evacuationEventAlarmNativeCount As Long = adapter.InsertEventAlarmPastNative
                OutputLog(LOGLEVEL.INFO, MSGID.MSG_509, CType(evacuationEventAlarmNativeCount, String))
            End Using

            ' カレンダーイベント繰り返し除外日の退避(Native分)
            Using adapter As New MC3040402DataSetTableAdapters.CalEventExDatePastAdapter
                Dim evacuationEventExDateNativeCount As Long = adapter.InsertEventExdatePastNative
                OutputLog(LOGLEVEL.INFO, MSGID.MSG_510, CType(evacuationEventExDateNativeCount, String))
            End Using

            ' カレンダーイベント情報の退避(Native分)
            Using adapter As New MC3040402DataSetTableAdapters.CalEventItemPastAdapter
                Dim evacuationEventItemNativeCount As Long = adapter.InsertEventItemPastNative
                OutputLog(LOGLEVEL.INFO, MSGID.MSG_511, CType(evacuationEventItemNativeCount, String))
            End Using

            OutputLog(LOGLEVEL.INFO, MSGID.MSG_502, EmptyString)

            Return True

        End Function

        ''' <summary>
        ''' 退避済みTodo、イベントの削除処理
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function DeleteTodoEvent() As Boolean

            OutputLog(LOGLEVEL.INFO, MSGID.MSG_601, EmptyString)

            ' カレンダーイベントアラームより退避データの削除
            Using adapter As New MC3040402DataSetTableAdapters.CalEventAlarmAdapter
                Dim evacuationEventAlarmCount As Long = adapter.DeleteEventAlarm
                OutputLog(LOGLEVEL.INFO, MSGID.MSG_603, CType(evacuationEventAlarmCount, String))
            End Using

            ' カレンダーイベント繰り返し除外日より退避データの削除
            Using adapter As New MC3040402DataSetTableAdapters.CalEventExdateAdapter
                Dim evacuationEventExDateCount As Long = adapter.DeleteEventExdate
                OutputLog(LOGLEVEL.INFO, MSGID.MSG_604, CType(evacuationEventExDateCount, String))
            End Using

            ' カレンダーイベント情報より退避データの削除
            Using adapter As New MC3040402DataSetTableAdapters.CalEventItemAdapter
                Dim evacuationEventItemCount As Long = adapter.DeleteEventItem
                OutputLog(LOGLEVEL.INFO, MSGID.MSG_605, CType(evacuationEventItemCount, String))
            End Using

            ' カレンダーTodoアラームより退避データの削除
            Using adapter As New MC3040402DataSetTableAdapters.CalTodoAlarmAdapter
                Dim evacuationTodoAlarmCount As Long = adapter.DeleteTodoAlarm
                OutputLog(LOGLEVEL.INFO, MSGID.MSG_606, CType(evacuationTodoAlarmCount, String))
            End Using

            ' カレンダーTODO繰り返し除外日より退避データの削除
            Using adapter As New MC3040402DataSetTableAdapters.CalTodoExDateAdapter
                Dim evacuationTodoExDateCount As Long = adapter.DeleteTodoExdate
                OutputLog(LOGLEVEL.INFO, MSGID.MSG_607, CType(evacuationTodoExDateCount, String))
            End Using

            ' カレンダーTODO情報より退避データの削除
            Using adapter As New MC3040402DataSetTableAdapters.CalTodoItemAdapter
                Dim evacuationTodoItemCount As Long = adapter.DeleteTodoItem
                OutputLog(LOGLEVEL.INFO, MSGID.MSG_608, CType(evacuationTodoItemCount, String))
            End Using


            ' カレンダーイベントアラームより退避データの削除(Native分)
            Using adapter As New MC3040402DataSetTableAdapters.CalEventAlarmAdapter
                Dim evacuationEventAlarmNativeCount As Long = adapter.DeleteEventAlarmNative
                OutputLog(LOGLEVEL.INFO, MSGID.MSG_609, CType(evacuationEventAlarmNativeCount, String))
            End Using

            ' カレンダーイベント繰り返し除外日より退避データの削除(Native分)
            Using adapter As New MC3040402DataSetTableAdapters.CalEventExdateAdapter
                Dim evacuationEventExDateNativeCount As Long = adapter.DeleteEventExdateNative
                OutputLog(LOGLEVEL.INFO, MSGID.MSG_610, CType(evacuationEventExDateNativeCount, String))
            End Using

            ' カレンダーイベント情報より退避データの削除(Native分)
            Using adapter As New MC3040402DataSetTableAdapters.CalEventItemAdapter
                Dim evacuationEventItemNativeCount As Long = adapter.DeleteEventItemNative
                OutputLog(LOGLEVEL.INFO, MSGID.MSG_611, CType(evacuationEventItemNativeCount, String))
            End Using

            OutputLog(LOGLEVEL.INFO, MSGID.MSG_602, EmptyString)

            Return True

        End Function

        ''' <summary>
        ''' iCROP情報テーブル退避処理
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function MoveIcropInfo() As Boolean

            OutputLog(LOGLEVEL.INFO, MSGID.MSG_701, EmptyString)

            ' カレンダーIDに該当するTodo情報の存在チェック
            ' DataTable生成
            Using dataTable As New MC3040402DataSet.UpdateTodoFlgDataTable
                ' DataRow生成
                Dim dataRow As MC3040402DataSet.UpdateTodoFlgRow = dataTable.NewUpdateTodoFlgRow()
                ' 変数の設定
                With dataRow
                    .MODULEID = MODULEID        ' モジュールID
                End With
                Using adapter As New MC3040402DataSetTableAdapters.UpdateTodoFlgDataTable
                    Dim updateTodoFlgCount As Long = adapter.UpdateTodoFlg(dataRow)
                    OutputLog(LOGLEVEL.INFO, MSGID.MSG_703, CType(updateTodoFlgCount, String))
                End Using

                ' カレンダーiCROP情報の退避
                Using adapter As New MC3040402DataSetTableAdapters.CalIcropInfoPastAdapter
                    Dim evacuationIcropInfoCount As Long = adapter.InsertIcropInfoPast
                    OutputLog(LOGLEVEL.INFO, MSGID.MSG_704, CType(evacuationIcropInfoCount, String))
                End Using

                ' カレンダーiCROP情報より退避データの削除
                Using adapter As New MC3040402DataSetTableAdapters.CalIcropInfoAdapter
                    Dim evacuationDelIcropInfoCount As Long = adapter.DeleteIcropInfo
                    OutputLog(LOGLEVEL.INFO, MSGID.MSG_705, CType(evacuationDelIcropInfoCount, String))
                End Using

                OutputLog(LOGLEVEL.INFO, MSGID.MSG_702, EmptyString)

                Return True

            End Using

        End Function

#End Region

    End Class

End Namespace
