'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3150101DataSet.vb
'─────────────────────────────────────
'機能： TCメインメニューデータセット
'補足： 
'作成： 2012/01/26 KN 鶴田
'更新： 2012/02/27 KN 佐藤 DevPartner 1回目の指摘事項を修正
'更新： 2012/02/27 KN 佐藤 スタッフストール割当の抽出条件を追加
'更新： 2012/02/28 KN 渡辺 関連チップの順不同開始を抑制するように修正
'更新： 2012/02/28 KN 上田 SQLインスペクション対応
'─────────────────────────────────────

Option Strict On
Option Explicit On

Imports System.Text
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Oracle.DataAccess.Client
Imports System.Globalization

Partial Class SC3150101DataSet

End Class

Namespace SC3150101DataSetTableAdapters
    Public Class SC3150101StallInfoDataTableAdapter
        Inherits Global.System.ComponentModel.Component


        ''' <summary>
        ''' ストール予約情報を取得する。
        ''' </summary>
        ''' <param name="dealerCode">販売点コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="reserveId">予約ID</param>
        ''' <returns>処理結果</returns>
        ''' <remarks></remarks>
        Public Function GetStallReserveInfo(ByVal dealerCode As String, _
                                            ByVal branchCode As String, _
                                            ByVal reserveId As Integer) As SC3150101DataSet.SC3150101StallReserveInfoDataTable

            Logger.Info("[S]GetStallReserveInfo()")

            Using query As New DBSelectQuery(Of SC3150101DataSet.SC3150101StallReserveInfoDataTable)("SC3150101_001")

                Dim sql As New StringBuilder

                ' SQL文の作成
                With sql
                    .Append(" SELECT /* SC3150101_001 */ ")
                    .Append("        DLRCD, ")                ' 01 販売店コード
                    .Append("        STRCD, ")                ' 02 店舗コード
                    .Append("        REZID, ")                ' 03 予約ID
                    .Append("        STALLID, ")              ' 05 ストールID
                    .Append("        STARTTIME, ")            ' 06 使用開始日時
                    .Append("        ENDTIME, ")              ' 07 使用終了日時
                    .Append("        STATUS, ")               ' 19 ステータス
                    .Append("        WASHFLG, ")              ' 30 洗車フラグ
                    .Append("        REZ_RECEPTION, ")        ' 33 予約_受付納車区分
                    .Append("        REZ_WORK_TIME, ")        ' 34 予定_作業時間
                    .Append("        REZ_PICK_DATE, ")        ' 35 予約_引取_希望日時時刻
                    .Append("        REZ_PICK_LOC, ")         ' 36 予約_引取_場所
                    .Append("        REZ_PICK_TIME, ")        ' 37 予約_引取_所要時間
                    .Append("        REZ_DELI_DATE, ")        ' 39 予約_納車_希望日時時刻
                    .Append("        REZ_DELI_LOC, ")         ' 40 予約_納車_場所
                    .Append("        REZ_DELI_TIME, ")        ' 41 予約_納車_所要時間
                    .Append("        STOPFLG, ")              ' 44 中断フラグ
                    '2012/02/28 KN 渡辺 【SERVICE_1】関連チップの順不同開始を抑制するように修正 START
                    .Append("        REZCHILDNO, ")           ' 46 子予約連番
                    '2012/02/28 KN 渡辺 【SERVICE_1】関連チップの順不同開始を抑制するように修正 END
                    .Append("        STRDATE, ")              ' 54 入庫時間
                    .Append("        CANCELFLG, ")            ' 58 キャンセルフラグ
                    .Append("        INSPECTIONFLG, ")        ' 67 検査フラグ
                    .Append("        DELIVERY_FLG ")          ' 66 納車済フラグ
                    .Append("   FROM tbl_STALLREZINFO ")      ' [ストール予約]
                    .Append("  WHERE DLRCD = :DLRCD ")        ' 01 販売店コード
                    .Append("    AND STRCD = :STRCD ")        ' 02 店舗コード
                    .Append("    AND REZID = :REZID")         ' 03 予約ID
                End With

                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
                query.AddParameterWithTypeValue("REZID", OracleDbType.Int64, reserveId)

                Logger.Info("[E]GetStallReserveInfo()")

                ' 検索結果の返却
                Return query.GetData()

            End Using

        End Function

        ''' <summary>
        ''' ストール実績情報を取得する。
        ''' </summary>
        ''' <param name="dealerCode">販売点コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="reserveId">予約ID</param>
        ''' <returns>処理結果</returns>
        ''' <remarks></remarks>
        Public Function GetStallProcessInfo(ByVal dealerCode As String, _
                                            ByVal branchCode As String, _
                                            ByVal reserveId As Integer) As SC3150101DataSet.SC3150101StallProcessInfoDataTable

            Logger.Info("[S]GetStallProcessInfo()")

            Dim sql As New StringBuilder

            ' SQL文の作成
            With sql
                .Append("    SELECT /* SC3150101_002 */ ")
                .Append("           t1.DLRCD AS DLRCD, ")                                        ' 01 販売店コード
                .Append("           t1.STRCD AS STRCD, ")                                        ' 02 店舗コード
                .Append("           t1.REZID AS REZID, ")                                        ' 03 予約ID
                '.Append("           t2.DSEQNO AS DSEQNO, ")                                      ' 04 日跨ぎシーケンス番号
                '.Append("           t2.SEQNO AS SEQNO, ")                                        ' 05 シーケンス番号
                .Append("           NVL(t2.DSEQNO, 0) AS DSEQNO, ")                                      ' 04 日跨ぎシーケンス番号
                .Append("           NVL(t2.SEQNO, 0) AS SEQNO, ")                                        ' 05 シーケンス番号
                .Append("           t2.RESULT_STATUS AS RESULT_STATUS, ")                        ' 16 実績_ステータス
                '.Append("           t2.RESULT_STALLID AS RESULT_STALLID, ")                      ' 17 実績_ストールID
                .Append("           NVL(t2.RESULT_STALLID, 0) AS RESULT_STALLID, ")                      ' 17 実績_ストールID
                .Append("           t2.RESULT_START_TIME AS RESULT_START_TIME, ")                ' 18 実績_ストール開始日時時刻
                .Append("           t2.RESULT_END_TIME AS RESULT_END_TIME, ")                    ' 19 実績_ストール終了日時時刻
                '.Append("           t2.RESULT_WORK_TIME AS RESULT_WORK_TIME, ")                  ' 21 実績_実績時間
                .Append("           NVL(t2.RESULT_WORK_TIME, 0) AS RESULT_WORK_TIME, ")                  ' 21 実績_実績時間
                .Append("           t2.RESULT_IN_TIME AS RESULT_IN_TIME, ")                      ' 20 実績_入庫時間
                .Append("           t2.REZ_START_TIME AS REZ_START_TIME, ")                      ' 23 予定_ストール開始日時時刻
                .Append("           t2.REZ_END_TIME AS REZ_END_TIME, ")                          ' 24 予定_ストール終了日時時刻
                .Append("           NVL(t2.REZ_WORK_TIME, t1.REZ_WORK_TIME) AS REZ_WORK_TIME, ") ' 25 予定_作業時間
                .Append("           t2.RESULT_WASH_START AS RESULT_WASH_START, ")                ' 34 洗車開始時刻
                .Append("           t2.RESULT_WASH_END AS RESULT_WASH_END, ")                    ' 35 洗車終了時刻
                .Append("           t2.RESULT_WAIT_START AS RESULT_WAIT_START, ")                ' 36 納車待ち開始時刻
                .Append("           t2.RESULT_WAIT_END AS RESULT_WAIT_END, ")                    ' 37 納車待ち終了時刻
                .Append("           t2.RESULT_INSPECTION_START AS RESULT_INSPECTION_START, ")    ' 51 実績検査開始時刻
                .Append("           t2.RESULT_INSPECTION_END AS RESULT_INSPECTION_END ")         ' 52 実績検査終了時刻
                .Append("      FROM tbl_STALLREZINFO t1 ")                                       ' [ストール予約]
                .Append(" LEFT JOIN tbl_STALLPROCESS t2 ")                                       ' [ストール実績]
                .Append("        ON t2.DLRCD = t1.DLRCD ")                                       ' 01 販売店コード
                .Append("       AND t2.STRCD = t1.STRCD ")                                       ' 02 店舗コード
                .Append("       AND t2.REZID = t1.REZID ")                                       ' 03 予約ID
                .Append("     WHERE t1.DLRCD = :DLRCD ")                                         ' 01 販売店コード
                .Append("       AND t1.STRCD = :STRCD ")                                         ' 02 店舗コード
                .Append("       AND t1.REZID = :REZID ")                                         ' 03 予約ID
                .Append("       AND (t2.SEQNO IS NULL ")                                         ' 05 シーケンス番号
                .Append("           OR (t2.DSEQNO = (SELECT MAX(t3.DSEQNO) ")                    ' 04 日跨ぎシーケンス番号
                .Append("                              FROM tbl_STALLPROCESS t3 ")               ' [ストール実績]
                .Append("                             WHERE t3.DLRCD = t2.DLRCD ")               ' 01 販売店コード
                .Append("                               AND t3.STRCD = t2.STRCD ")               ' 02 店舗コード
                .Append("                               AND t3.REZID = t2.REZID ")               ' 03 予約ID
                .Append("                          GROUP BY t3.DLRCD, t3.STRCD, t3.REZID) ")
                .Append("          AND t2.SEQNO = (SELECT MAX(t4.SEQNO) ")                       ' 05 シーケンス番号
                .Append("                            FROM tbl_STALLPROCESS t4 ")                 ' [ストール実績]
                .Append("                           WHERE t4.DLRCD = t2.DLRCD ")                 ' 01 販売店コード
                .Append("                             AND t4.STRCD = t2.STRCD ")                 ' 02 店舗コード
                .Append("                             AND t4.REZID = t2.REZID ")                 ' 03 予約ID
                .Append("                             AND t4.DSEQNO = t2.DSEQNO) ")              ' 04 日跨ぎシーケンス番号
                .Append("              ) ")
                .Append("           )")
            End With

            Using query As New DBSelectQuery(Of SC3150101DataSet.SC3150101StallProcessInfoDataTable)("SC3150101_002")

                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
                query.AddParameterWithTypeValue("REZID", OracleDbType.Int64, reserveId)

                Dim stallProcessInfoTable As SC3150101DataSet.SC3150101StallProcessInfoDataTable

                ' SQLの実行
                stallProcessInfoTable = query.GetData()

                If stallProcessInfoTable.Rows.Count <> 0 Then
                    stallProcessInfoTable.Rows.Item(0).Item("DLRCD") = SetData(stallProcessInfoTable.Rows.Item(0).Item("DLRCD"), "")
                    stallProcessInfoTable.Rows.Item(0).Item("STRCD") = SetData(stallProcessInfoTable.Rows.Item(0).Item("STRCD"), "")
                    stallProcessInfoTable.Rows.Item(0).Item("REZID") = SetData(stallProcessInfoTable.Rows.Item(0).Item("REZID"), 0)
                    stallProcessInfoTable.Rows.Item(0).Item("DSEQNO") = SetData(stallProcessInfoTable.Rows.Item(0).Item("DSEQNO"), 0)
                    stallProcessInfoTable.Rows.Item(0).Item("SEQNO") = SetData(stallProcessInfoTable.Rows.Item(0).Item("SEQNO"), 0)
                    stallProcessInfoTable.Rows.Item(0).Item("RESULT_STATUS") = SetData(stallProcessInfoTable.Rows.Item(0).Item("RESULT_STATUS"), "0")
                    stallProcessInfoTable.Rows.Item(0).Item("RESULT_STALLID") = SetData(stallProcessInfoTable.Rows.Item(0).Item("RESULT_STALLID"), 0)
                    stallProcessInfoTable.Rows.Item(0).Item("RESULT_START_TIME") = SetData(stallProcessInfoTable.Rows.Item(0).Item("RESULT_START_TIME"), "")
                    stallProcessInfoTable.Rows.Item(0).Item("RESULT_END_TIME") = SetData(stallProcessInfoTable.Rows.Item(0).Item("RESULT_END_TIME"), "")
                    stallProcessInfoTable.Rows.Item(0).Item("RESULT_WORK_TIME") = SetData(stallProcessInfoTable.Rows.Item(0).Item("RESULT_WORK_TIME"), 0)
                    stallProcessInfoTable.Rows.Item(0).Item("RESULT_IN_TIME") = SetData(stallProcessInfoTable.Rows.Item(0).Item("RESULT_IN_TIME"), "")
                    stallProcessInfoTable.Rows.Item(0).Item("RESULT_WASH_START") = SetData(stallProcessInfoTable.Rows.Item(0).Item("RESULT_WASH_START"), "")
                    stallProcessInfoTable.Rows.Item(0).Item("RESULT_WASH_END") = SetData(stallProcessInfoTable.Rows.Item(0).Item("RESULT_WASH_END"), "")
                    stallProcessInfoTable.Rows.Item(0).Item("RESULT_INSPECTION_START") = SetData(stallProcessInfoTable.Rows.Item(0).Item("RESULT_INSPECTION_START"), "")
                    stallProcessInfoTable.Rows.Item(0).Item("RESULT_INSPECTION_END") = SetData(stallProcessInfoTable.Rows.Item(0).Item("RESULT_INSPECTION_END"), "")
                    stallProcessInfoTable.Rows.Item(0).Item("RESULT_WAIT_START") = SetData(stallProcessInfoTable.Rows.Item(0).Item("RESULT_WAIT_START"), "")
                    stallProcessInfoTable.Rows.Item(0).Item("RESULT_WAIT_END") = SetData(stallProcessInfoTable.Rows.Item(0).Item("RESULT_WAIT_END"), "")
                    stallProcessInfoTable.Rows.Item(0).Item("REZ_START_TIME") = SetData(stallProcessInfoTable.Rows.Item(0).Item("REZ_START_TIME"), "")
                    stallProcessInfoTable.Rows.Item(0).Item("REZ_END_TIME") = SetData(stallProcessInfoTable.Rows.Item(0).Item("REZ_END_TIME"), "")
                    stallProcessInfoTable.Rows.Item(0).Item("REZ_WORK_TIME") = SetData(stallProcessInfoTable.Rows.Item(0).Item("REZ_WORK_TIME"), 0)
                Else
                    Logger.Info("[E]GetStallProcessInfo()")
                    Return Nothing
                End If

                Logger.Info("[E]GetStallProcessInfo()")
                Return (stallProcessInfoTable)

            End Using

        End Function


        ''' <summary>
        ''' ストール予約情報を更新する。
        ''' </summary>
        ''' <param name="reserveInfo">ストール予約情報</param>
        ''' <param name="actualStartTime">販売点コード</param>
        ''' <param name="actualEndTime">店舗コード</param>
        ''' <param name="updateStartTime">作業開始時間の更新方法(0:Nullで上書き, 1:指定値で上書き, 2:変更しない)</param>
        ''' <param name="updateEndTime">作業終了時間の更新方法(0:Nullで上書き, 1:指定値で上書き, 2:変更しない)</param>
        ''' <param name="updateAccount">アカウント</param>
        ''' <param name="newChildNo"></param>
        ''' <returns>処理結果</returns>
        ''' <remarks></remarks>
        Public Function UpdateStallReserveInfo(ByVal reserveInfo As SC3150101DataSet.SC3150101StallReserveInfoDataTable, _
                                               ByVal actualStartTime As Date, _
                                               ByVal actualEndTime As Date, _
                                               ByVal updateStartTime As Integer, _
                                               ByVal updateEndTime As Integer, _
                                               ByVal updateAccount As String, _
                                               Optional ByVal newChildNo As Integer = -1) As Integer 'UpdateStallRezInfo

            Logger.Info("[S]UpdateStallReserveInfo()")

            ' 引数チェック
            If reserveInfo Is Nothing Then
                'Argument is nothing
                Logger.Error("Argument is nothing [FUNC:UpdateStallReserveInfo()]")
                Logger.Info("[E]UpdateStallReserveInfo()")
                Return (-1)
            End If

            '-----------------
            ' データセットを展開
            Dim drReserveInfo As SC3150101DataSet.SC3150101StallReserveInfoRow
            drReserveInfo = CType(reserveInfo.Rows(0), SC3150101DataSet.SC3150101StallReserveInfoRow)
            '-----------------

            ' DbUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("SC3150101_003")

                Dim sql As New StringBuilder


                ' SQL文の作成
                With sql
                    .Append(" UPDATE /* SC3150101_003 */ ")
                    .Append("        tbl_STALLREZINFO ")
                    .Append("    SET STALLID = :STALLID, ")               ' 05 ストールID
                    .Append("        STARTTIME = :STARTTIME, ")           ' 06 使用開始日時
                    .Append("        ENDTIME = :ENDTIME, ")               ' 07 使用終了日時
                    .Append("        REZ_WORK_TIME = :REZ_WORK_TIME, ")   ' 34 予定_作業時間
                    .Append("        STATUS = :STATUS, ")                 ' 19 ステータス
                    If drReserveInfo.STRDATE = DateTime.MinValue Then
                        .Append("        STRDATE = NULL, ")                   ' 54 入庫日時
                    Else
                        .Append("        STRDATE = :STRDATE, ")               ' 54 入庫日時
                    End If
                    .Append("        WASHFLG = :WASHFLG, ")               ' 30 洗車フラグ
                    .Append("        INSPECTIONFLG = :INSPECTIONFLG, ")   ' 67 検査フラグ
                    .Append("        STOPFLG = :STOPFLG, ")               ' 44 中断フラグ
                    If CType(drReserveInfo.STOPFLG, Integer) = 0 Then
                        .Append("        CANCELFLG = '0', ")                  ' 58 キャンセルフラグ
                    Else
                        .Append("        CANCELFLG = '1', ")                  ' 58 キャンセルフラグ
                    End If
                    .Append("        DELIVERY_FLG = :DELIVERY_FLG, ")     ' 66 納車済フラグ
                    .Append("        UPDATE_COUNT = UPDATE_COUNT + 1, ")  ' 43 更新カウント
                    .Append("        UPDATEACCOUNT = :UPDATEACCOUNT, ")   ' 61 更新ユーザーアカウント
                    .Append("        UPDATEDATE = sysdate ")
                    If updateStartTime = 0 Then
                        .Append("      , ACTUAL_STIME = NULL ")             ' 47 作業開始時間
                    ElseIf updateStartTime = 1 Then
                        .Append("      , ACTUAL_STIME = :ACTUAL_STIME ")    ' 47 作業開始時間
                    End If
                    If updateEndTime = 0 Then
                        .Append("      , ACTUAL_ETIME = NULL ")             ' 48 作業終了時間
                    ElseIf updateEndTime = 1 Then
                        .Append("      , ACTUAL_ETIME = :ACTUAL_ETIME ")    ' 48 作業終了時間
                    End If
                    If newChildNo > 0 Then
                        .Append("      , REZCHILDNO = :REZCHILDNO ")        ' 46 子予約連番
                    End If
                    .Append("  WHERE DLRCD = :DLRCD ")                    ' 01 販売店コード
                    .Append("    AND STRCD = :STRCD ")                    ' 02 店舗コード
                    .Append("    AND REZID = :REZID ")                    ' 03 予約ID
                End With

                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("STALLID", OracleDbType.Int64, drReserveInfo.STALLID)             ' 05 ストールID
                query.AddParameterWithTypeValue("STARTTIME", OracleDbType.Date, drReserveInfo.STARTTIME)          ' 06 使用開始日時
                query.AddParameterWithTypeValue("ENDTIME", OracleDbType.Date, drReserveInfo.ENDTIME)              ' 07 使用終了日時
                query.AddParameterWithTypeValue("REZ_WORK_TIME", OracleDbType.Int64, drReserveInfo.REZ_WORK_TIME)      ' 34 予定_作業時間
                query.AddParameterWithTypeValue("STATUS", OracleDbType.Int64, drReserveInfo.STATUS)            ' 19 ステータス
                If drReserveInfo.STRDATE <> DateTime.MinValue Then
                    query.AddParameterWithTypeValue("STRDATE", OracleDbType.Date, drReserveInfo.STRDATE)            ' 54 入庫日時
                End If
                query.AddParameterWithTypeValue("WASHFLG", OracleDbType.Char, drReserveInfo.WASHFLG)              ' 30 洗車フラグ
                query.AddParameterWithTypeValue("INSPECTIONFLG", OracleDbType.Char, drReserveInfo.INSPECTIONFLG)  ' 67 検査フラグ
                query.AddParameterWithTypeValue("STOPFLG", OracleDbType.Char, drReserveInfo.STOPFLG)              ' 44 中断フラグ
                query.AddParameterWithTypeValue("DELIVERY_FLG", OracleDbType.Char, drReserveInfo.DELIVERY_FLG)     ' 66 納車済フラグ
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, updateAccount)  ' 61 更新ユーザーアカウント
                If updateStartTime = 1 Then
                    query.AddParameterWithTypeValue("ACTUAL_STIME", OracleDbType.Date, actualStartTime)  ' 47 作業開始時間
                End If
                If updateEndTime = 1 Then
                    query.AddParameterWithTypeValue("ACTUAL_ETIME", OracleDbType.Date, actualEndTime)  ' 48 作業終了時間
                End If
                If newChildNo > 0 Then
                    query.AddParameterWithTypeValue("REZCHILDNO", OracleDbType.Int64, newChildNo)           ' 46 子予約連番
                End If

                'query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)                  ' 販売店コード
                'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)                  ' 店舗コード
                'query.AddParameterWithTypeValue("REZID", OracleDbType.Int64, reserveId)                     ' 予約ID
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, drReserveInfo.DLRCD)                  ' 販売店コード
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, drReserveInfo.STRCD)                  ' 店舗コード
                query.AddParameterWithTypeValue("REZID", OracleDbType.Int64, drReserveInfo.REZID)                 ' 予約ID

                Logger.Info("[E]UpdateStallReserveInfo()")

                'SQL実行
                Return query.Execute()

            End Using

        End Function


        ''' <summary>
        ''' ストール実績情報の更新
        ''' </summary>
        ''' <param name="procInfo">ストール実績情報</param>
        ''' <param name="reserveInfo">ストール予約情報</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function UpdateStallProcessInfo(ByVal procInfo As SC3150101DataSet.SC3150101StallProcessInfoDataTable, _
                                               ByVal reserveInfo As SC3150101DataSet.SC3150101StallReserveInfoDataTable) As Integer

            Logger.Info("[S]UpdateStallProcessInfo()")

            ' 引数チェック
            If procInfo Is Nothing Then
                'Argument is nothing
                Logger.Error("Argument is nothing [FUNC:UpdateStallProcessInfo()]")
                Logger.Info("[E]UpdateStallProcessInfo()")
                Return (-1)
            End If

            '-----------------
            Dim drProcInfo As SC3150101DataSet.SC3150101StallProcessInfoRow
            drProcInfo = CType(procInfo.Rows(0), SC3150101DataSet.SC3150101StallProcessInfoRow)
            Dim drReserveInfo As SC3150101DataSet.SC3150101StallReserveInfoRow
            If reserveInfo IsNot Nothing Then
                drReserveInfo = CType(reserveInfo.Rows(0), SC3150101DataSet.SC3150101StallReserveInfoRow)
            Else
                drReserveInfo = Nothing
            End If
            '-----------------

            Dim sql As New StringBuilder

            ' SQL文の作成
            With sql
                .Append(" UPDATE /* SC3150101_004 */ ")
                .Append("        tbl_STALLPROCESS ")
                .Append("    SET RESULT_STATUS = :RESULT_STATUS, ")                        ' 16 実績_ステータス
                .Append("        RESULT_STALLID = :RESULT_STALLID, ")                      ' 17 実績_ストールID
                .Append("        RESULT_START_TIME = :RESULT_START_TIME, ")                ' 18 実績_ストール開始日時時刻
                .Append("        RESULT_END_TIME = :RESULT_END_TIME, ")                    ' 19 実績_ストール終了日時時刻
                If drProcInfo.RESULT_WORK_TIME >= 0 And CType(drProcInfo.RESULT_STATUS, Integer) > 20 Then     ' 作業時間が0以上 かつ ステータスが20(作業中)より大きい 時
                    .Append("        RESULT_WORK_TIME = :RESULT_WORK_TIME, ")                  ' 21 実績_実績時間
                Else
                    .Append("        RESULT_WORK_TIME = NULL, ")                               ' 21 実績_実績時間
                End If
                .Append("        RESULT_IN_TIME = :RESULT_IN_TIME, ")                      ' 20 実績_入庫時間
                .Append("        RESULT_WASH_START = :RESULT_WASH_START, ")                ' 34 洗車開始時刻
                .Append("        RESULT_WASH_END = :RESULT_WASH_END, ")                    ' 35 洗車終了時刻
                .Append("        RESULT_INSPECTION_START = :RESULT_INSPECTION_START, ")    ' 51 実績検査開始時刻
                .Append("        RESULT_INSPECTION_END = :RESULT_INSPECTION_END, ")        ' 52 実績検査終了時刻
                .Append("        RESULT_WAIT_START = :RESULT_WAIT_START, ")                ' 36 納車待ち開始時刻
                .Append("        RESULT_WAIT_END = :RESULT_WAIT_END, ")                    ' 37 納車待ち終了時刻
                If reserveInfo IsNot Nothing Then ' 予約情報が取得できた場合
                    .Append("        REZ_Reception = :REZ_Reception, ")                        ' 22 予約_受付納車区分
                    .Append("        REZ_START_TIME = :REZ_START_TIME, ")                      ' 23 予定_ストール開始日時時刻
                    .Append("        REZ_END_TIME = :REZ_END_TIME, ")                          ' 24 予定_ストール終了日時時刻
                    .Append("        REZ_WORK_TIME = :REZ_WORK_TIME, ")                        ' 25 予定_作業時間
                    .Append("        REZ_PICK_DATE = :REZ_PICK_DATE, ")                        ' 26 予約_引取_希望日時時刻
                    .Append("        REZ_PICK_LOC = :REZ_PICK_LOC, ")                          ' 27 予約_引取_場所
                    .Append("        REZ_PICK_TIME = :REZ_PICK_TIME, ")                        ' 28 予約_引取_所要時間
                    .Append("        REZ_DELI_DATE = :REZ_DELI_DATE, ")                        ' 30 予約_納車_希望日時時刻
                    .Append("        REZ_DELI_LOC = :REZ_DELI_LOC, ")                          ' 31 予約_納車_場所
                    .Append("        REZ_DELI_TIME = :REZ_DELI_TIME, ")                        ' 32 予約_納車_所要時間
                    .Append("        RESULT_CARRY_IN = :RESULT_CARRY_IN, ")                    ' 38 預かり日時時刻
                    .Append("        RESULT_CARRY_OUT = :RESULT_CARRY_OUT, ")                  ' 39 引渡し日時時刻
                End If
                .Append("        UPDATE_COUNT = UPDATE_COUNT + 1, ")                       ' 40 更新カウント
                .Append("        UPDATEDATE = sysdate ")                                   ' 47 更新日
                .Append("  WHERE DLRCD = :DLRCD ")                                         ' 01 販売店コード
                .Append("    AND STRCD = :STRCD ")                                         ' 02 店舗コード
                .Append("    AND REZID = :REZID ")                                         ' 03 予約ID
                .Append("    AND DSEQNO = :DSEQNO ")                                       ' 04 日跨ぎシーケンス番号
                .Append("    AND SEQNO = :SEQNO ")                                         ' 05 シーケンス番号
            End With

            ' DbUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("SC3150101_004")

                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("RESULT_STATUS", OracleDbType.Char, drProcInfo.RESULT_STATUS)                                  ' 16 実績_ステータス
                query.AddParameterWithTypeValue("RESULT_STALLID", OracleDbType.Int64, drProcInfo.RESULT_STALLID)                     ' 17 実績_ストールID
                query.AddParameterWithTypeValue("RESULT_START_TIME", OracleDbType.Char, drProcInfo.RESULT_START_TIME)                ' 18 実績_ストール開始日時時刻
                query.AddParameterWithTypeValue("RESULT_END_TIME", OracleDbType.Char, drProcInfo.RESULT_END_TIME)                    ' 19 実績_ストール終了日時時刻
                If drProcInfo.RESULT_WORK_TIME >= 0 And CType(drProcInfo.RESULT_STATUS, Integer) > 20 Then
                    query.AddParameterWithTypeValue("RESULT_WORK_TIME", OracleDbType.Int64, drProcInfo.RESULT_WORK_TIME)                 ' 21 実績_実績時間
                End If
                query.AddParameterWithTypeValue("RESULT_IN_TIME", OracleDbType.Char, drProcInfo.RESULT_IN_TIME)                      ' 20 実績_入庫時間
                query.AddParameterWithTypeValue("RESULT_WASH_START", OracleDbType.Char, drProcInfo.RESULT_WASH_START)                ' 34 洗車開始時刻
                query.AddParameterWithTypeValue("RESULT_WASH_END", OracleDbType.Char, drProcInfo.RESULT_WASH_END)                    ' 35 洗車終了時刻
                query.AddParameterWithTypeValue("RESULT_INSPECTION_START", OracleDbType.Char, drProcInfo.RESULT_INSPECTION_START)    ' 51 実績検査開始時刻
                query.AddParameterWithTypeValue("RESULT_INSPECTION_END", OracleDbType.Char, drProcInfo.RESULT_INSPECTION_END)        ' 52 実績検査終了時刻
                query.AddParameterWithTypeValue("RESULT_WAIT_START", OracleDbType.Char, drProcInfo.RESULT_WAIT_START)                ' 36 納車待ち開始時刻
                query.AddParameterWithTypeValue("RESULT_WAIT_END", OracleDbType.Char, drProcInfo.RESULT_WAIT_END)                    ' 37 納車待ち終了時刻
                'If IsNothing(rez) = False Then
                If reserveInfo IsNot Nothing Then
                    query.AddParameterWithTypeValue("REZ_Reception", OracleDbType.Char, drReserveInfo.REZ_RECEPTION)                        ' 22 予約_受付納車区分
                    query.AddParameterWithTypeValue("REZ_START_TIME", OracleDbType.Char, drReserveInfo.STARTTIME.ToString("yyyyMMddHHmm", CultureInfo.InvariantCulture()))                      ' 23 予定_ストール開始日時時刻
                    query.AddParameterWithTypeValue("REZ_END_TIME", OracleDbType.Char, drReserveInfo.ENDTIME.ToString("yyyyMMddHHmm", CultureInfo.InvariantCulture()))                          ' 24 予定_ストール終了日時時刻
                    query.AddParameterWithTypeValue("REZ_WORK_TIME", OracleDbType.Int64, drReserveInfo.REZ_WORK_TIME)                       ' 25 予定_作業時間
                    query.AddParameterWithTypeValue("REZ_PICK_DATE", OracleDbType.Char, drReserveInfo.REZ_PICK_DATE)                        ' 26 予約_引取_希望日時時刻
                    query.AddParameterWithTypeValue("REZ_PICK_LOC", OracleDbType.Char, drReserveInfo.REZ_PICK_LOC)                          ' 27 予約_引取_場所
                    query.AddParameterWithTypeValue("REZ_PICK_TIME", OracleDbType.Int64, drReserveInfo.REZ_PICK_TIME)                       ' 28 予約_引取_所要時間
                    query.AddParameterWithTypeValue("REZ_DELI_DATE", OracleDbType.Char, drReserveInfo.REZ_DELI_DATE)                        ' 30 予約_納車_希望日時時刻
                    query.AddParameterWithTypeValue("REZ_DELI_LOC", OracleDbType.Char, drReserveInfo.REZ_DELI_LOC)                          ' 31 予約_納車_場所
                    query.AddParameterWithTypeValue("REZ_DELI_TIME", OracleDbType.Int64, drReserveInfo.REZ_DELI_TIME)                       ' 32 予約_納車_所要時間
                    query.AddParameterWithTypeValue("RESULT_CARRY_IN", OracleDbType.Char, drReserveInfo.REZ_PICK_DATE)                      ' 38 預かり日時時刻
                    query.AddParameterWithTypeValue("RESULT_CARRY_OUT", OracleDbType.Char, drReserveInfo.REZ_DELI_DATE)                     ' 39 引渡し日時時刻
                End If
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, drProcInfo.DLRCD)                                        ' 01 販売店コード
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, drProcInfo.STRCD)                                        ' 02 店舗コード
                query.AddParameterWithTypeValue("REZID", OracleDbType.Int64, drProcInfo.REZID)                                       ' 03 予約ID
                query.AddParameterWithTypeValue("DSEQNO", OracleDbType.Int64, drProcInfo.DSEQNO)                                     ' 04 日跨ぎシーケンス番号
                query.AddParameterWithTypeValue("SEQNO", OracleDbType.Int64, drProcInfo.SEQNO)                                       ' 05 シーケンス番号

                Logger.Info("[E]UpdateStallProcessInfo()")

                'SQL実行
                Return query.Execute()

            End Using

        End Function



        ''' <summary>
        ''' ストール実績情報の登録
        ''' </summary>
        ''' <param name="procInfo">ストール実績情報</param>
        ''' <param name="updateAccount">更新アカウント</param>
        ''' <param name="middleFinish">当日処理でのInsertか否か</param>
        ''' <param name="relocate"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function InsertStallProcessInfo(ByVal procInfo As SC3150101DataSet.SC3150101StallProcessInfoDataTable, _
                                               ByVal updateAccount As String, _
                                               ByVal middleFinish As Boolean, _
                                               ByVal relocate As Boolean) As Integer

            Logger.Info("[S]InsertStallProcessInfo()")

            ' 引数チェック
            If procInfo Is Nothing Then
                'Argument is nothing
                Logger.Error("Argument is nothing [FUNC:InsertStallProcessInfo()]")
                Logger.Info("[E]InsertStallProcessInfo()")
                Return (-1)
            End If

            '-----------------
            Dim drProcInfo As SC3150101DataSet.SC3150101StallProcessInfoRow
            drProcInfo = CType(procInfo.Rows(0), SC3150101DataSet.SC3150101StallProcessInfoRow)
            '-----------------

            Dim sql As New StringBuilder

            ' SQL文の作成
            With sql
                .Append("INSERT /* SC3150101_005 */ ")
                .Append("  INTO tbl_STALLPROCESS (DLRCD, ")
                .Append("                         STRCD, ")
                .Append("                         REZID, ")
                .Append("                         DSEQNO, ")
                .Append("                         SEQNO, ")
                .Append("                         ORIGINALID, ")
                .Append("                         VIN, ")
                .Append("                         SERVICEMSTCD, ")
                .Append("                         NAME, ")
                .Append("                         MODELCODE, ")
                .Append("                         VCLREGNO, ")
                .Append("                         SERVICECODE, ")
                .Append("                         WASHFLG, ")
                .Append("                         INSPECTIONFLG, ")
                .Append("                         MILEAGE, ")
                .Append("                         RESULT_STATUS, ")
                .Append("                         RESULT_STALLID, ")
                .Append("                         RESULT_START_TIME, ")
                .Append("                         RESULT_END_TIME, ")
                .Append("                         RESULT_IN_TIME, ")
                .Append("                         RESULT_WORK_TIME, ")
                .Append("                         REZ_Reception, ")
                .Append("                         REZ_START_TIME, ")
                .Append("                         REZ_END_TIME, ")
                .Append("                         REZ_WORK_TIME, ")
                .Append("                         RESULT_WASH_START, ")
                .Append("                         RESULT_WASH_END, ")
                .Append("                         RESULT_INSPECTION_START, ")
                .Append("                         RESULT_INSPECTION_END, ")
                .Append("                         RESULT_WAIT_START, ")
                .Append("                         RESULT_WAIT_END, ")
                .Append("                         RESULT_CARRY_IN, ")
                .Append("                         RESULT_CARRY_OUT, ")
                .Append("                         UPDATE_COUNT, ")
                .Append("                         MEMO, ")
                .Append("                         PREZID, ")
                .Append("                         REZ_PICK_DATE, ")
                .Append("                         REZ_PICK_LOC, ")
                .Append("                         REZ_PICK_TIME, ")
                .Append("                         REZ_DELI_DATE, ")
                .Append("                         REZ_DELI_LOC, ")
                .Append("                         REZ_DELI_TIME, ")
                .Append("                         MERCHANDISECD, ")
                .Append("                         RSSTATUS, ")
                .Append("                         RSDATE, ")
                .Append("                         UPDATESERVER, ")
                .Append("                         INPUTACCOUNT, ")
                .Append("                         CREATEDATE, ")
                .Append("                         UPDATEDATE ")
                .Append("                        ) ")
                If drProcInfo.SEQNO <= 1 Then
                    .Append("SELECT t1.DLRCD, ")                                     ' 01 販売店コード
                    .Append("       t1.STRCD, ")                                     ' 02 店舗コード
                    .Append("       t1.REZID, ")                                     ' 03 予約ID
                    .Append("       :DSEQNO, ")                                      ' 04 日跨ぎシーケンス番号
                    .Append("       1, ")                                            ' 05 シーケンス番号
                    .Append("       NVL(t1.INSDID, ''), ")                           ' 06 連番
                    .Append("       t1.VIN, ")                                       ' 07 VIN
                    .Append("       t1.SERVICEMSTCD, ")                              ' 09 サービスマスタコード
                    .Append("       t1.CUSTOMERNAME, ")                              ' 10 氏名
                    .Append("       t1.MODELCODE, ")                                 ' 11 モデルコード
                    .Append("       t1.VCLREGNO, ")                                  ' 12 車両登録No.
                    .Append("       t1.SERVICECODE_S, ")                             ' 13 サービスコード
                    .Append("       t1.WASHFLG, ")                                   ' 14 洗車フラグ
                    .Append("       t1.INSPECTIONFLG, ")                             ' 53 検査フラグ
                    .Append("       NVL(t1.MILEAGE,0), ")                            ' 15 走行距離
                    .Append("       :RESULT_STATUS, ")                               ' 16 実績_ステータス
                    .Append("       t1.STALLID, ")                                   ' 17 実績_ストールID
                    .Append("       :RESULT_START_TIME, ")                           ' 18 実績_ストール開始日時時刻
                    .Append("       :RESULT_END_TIME, ")                             ' 19 実績_ストール終了日時時刻
                    .Append("       :RESULT_IN_TIME, ")                              ' 20 実績_入庫時間
                    .Append("       0, ")                                            ' 21 実績_実績時間
                    .Append("       t1.REZ_Reception, ")                             ' 22 予約_受付納車区分
                    .Append("       TO_CHAR(t1.STARTTIME, 'YYYYMMDDHH24MI'), ")      ' 23 予定_ストール開始日時時刻
                    .Append("       TO_CHAR(t1.ENDTIME, 'YYYYMMDDHH24MI'), ")        ' 24 予定_ストール終了日時時刻
                    .Append("       t1.REZ_WORK_TIME, ")                             ' 25 予定_作業時間
                    .Append("       :RESULT_WASH_START, ")                           ' 34 洗車開始時刻
                    .Append("       :RESULT_WASH_END, ")                             ' 35 洗車終了時刻
                    .Append("       :RESULT_INSPECTION_START, ")                     ' 51 実績検査開始時刻
                    .Append("       :RESULT_INSPECTION_END, ")                       ' 52 実績検査終了時刻
                    .Append("       :RESULT_WAIT_START, ")                           ' 36 納車待ち開始時刻
                    .Append("       :RESULT_WAIT_END, ")                             ' 37 納車待ち終了時刻
                    .Append("       TO_CHAR(t1.CRRYINTIME, 'YYYYMMDDHH24MI'), ")     ' 38 預かり日時時刻
                    .Append("       TO_CHAR(t1.CRRYOUTTIME, 'YYYYMMDDHH24MI'), ")    ' 39 引渡し日時時刻
                    If middleFinish Then
                        .Append("       t1.UPDATE_COUNT, ")                              ' 40 更新カウント
                    Else
                        .Append("       t1.UPDATE_COUNT + 1, ")                          ' 40 更新カウント
                    End If
                    .Append("       t1.MEMO, ")                                      ' 41 メモ
                    .Append("       t1.PREZID, ")                                    ' 43 管理予約ID
                    .Append("       t1.REZ_PICK_DATE, ")                             ' 26 予約_引取_希望日時時刻
                    .Append("       t1.REZ_PICK_LOC, ")                              ' 27 予約_引取_場所
                    .Append("       t1.REZ_PICK_TIME, ")                             ' 28 予約_引取_所要時間
                    .Append("       t1.REZ_DELI_DATE, ")                             ' 30 予約_納車_希望日時時刻
                    .Append("       t1.REZ_DELI_LOC, ")                              ' 31 予約_納車_場所
                    .Append("       t1.REZ_DELI_TIME, ")                             ' 32 予約_納車_所要時間
                    .Append("       t1.MERCHANDISECD, ")                             ' 08 商品コード
                    .Append("       '99', ")                                         ' 48 送受信完了フラグ
                    .Append("       sysdate, ")                                      ' 49 送受信日時
                    .Append("       '', ")                                           ' 50 データ発生サーバ
                    .Append("       :INPUTACCOUNT, ")                                ' 45 入力オペレータ
                    .Append("       sysdate, ")                                      ' 46 作成日
                    .Append("       sysdate ")                                       ' 47 更新日
                    .Append("  FROM tbl_STALLREZINFO t1 ")
                    .Append(" WHERE t1.DLRCD = :DLRCD ")
                    .Append("   AND t1.STRCD = :STRCD ")
                    .Append("   AND t1.REZID = :REZID ")
                Else
                    .Append("SELECT t1.DLRCD, ")                                     ' 01 販売店コード
                    .Append("       t1.STRCD, ")                                     ' 02 店舗コード
                    .Append("       t1.REZID, ")                                     ' 03 予約ID
                    .Append("       t1.DSEQNO, ")                                    ' 04 日跨ぎシーケンス番号
                    .Append("       t1.SEQNO + 1, ")                                 ' 05 シーケンス番号
                    .Append("       t1.ORIGINALID, ")                                ' 06 連番
                    .Append("       t1.VIN, ")                                       ' 07 VIN
                    .Append("       t1.SERVICEMSTCD, ")                              ' 09 サービスマスタコード
                    .Append("       t1.NAME, ")                                      ' 10 氏名
                    .Append("       t1.MODELCODE, ")                                 ' 11 モデルコード
                    .Append("       t1.VCLREGNO, ")                                  ' 12 車両登録No.
                    .Append("       t1.SERVICECODE, ")                               ' 13 サービスコード
                    .Append("       t1.WASHFLG, ")                                   ' 14 洗車フラグ
                    .Append("       t1.INSPECTIONFLG, ")                             ' 53 検査フラグ
                    .Append("       t1.MILEAGE, ")                                   ' 15 走行距離
                    .Append("       :RESULT_STATUS, ")                               ' 16 実績_ステータス
                    .Append("       :RESULT_STALLID, ")                              ' 17 実績_ストールID
                    .Append("       :RESULT_START_TIME, ")                           ' 18 実績_ストール開始日時時刻
                    .Append("       :RESULT_END_TIME, ")                             ' 19 実績_ストール終了日時時刻
                    .Append("       :RESULT_IN_TIME, ")                              ' 20 実績_入庫時間
                    .Append("       0, ")                                            ' 21 実績_実績時間
                    .Append("       t1.REZ_Reception, ")                             ' 22 予約_受付納車区分
                    .Append("       :REZ_START_TIME, ")                              ' 23 予定_ストール開始日時時刻
                    .Append("       :REZ_END_TIME, ")                                ' 24 予定_ストール終了日時時刻
                    If relocate Then
                        .Append("       :REZ_WORK_TIME, ")                               ' 25 予定_作業時間
                    Else
                        .Append("       t1.REZ_WORK_TIME, ")                             ' 25 予定_作業時間
                    End If
                    .Append("       :RESULT_WASH_START, ")                           ' 34 洗車開始時刻
                    .Append("       :RESULT_WASH_END, ")                             ' 35 洗車終了時刻
                    .Append("       :RESULT_INSPECTION_START, ")                     ' 51 実績検査開始時刻
                    .Append("       :RESULT_INSPECTION_END, ")                       ' 52 実績検査終了時刻
                    .Append("       :RESULT_WAIT_START, ")                           ' 36 納車待ち開始時刻
                    .Append("       :RESULT_WAIT_END, ")                             ' 37 納車待ち終了自国
                    .Append("       t1.RESULT_CARRY_IN, ")                           ' 38 預かり日時時刻
                    .Append("       t1.RESULT_CARRY_OUT, ")                          ' 39 引渡し日時時刻
                    .Append("       t1.UPDATE_COUNT + 1, ")                          ' 40 更新カウント
                    .Append("       t1.MEMO, ")                                      ' 41 メモ
                    .Append("       t1.PREZID, ")                                    ' 43 管理予約ID
                    .Append("       t1.REZ_PICK_DATE, ")                             ' 26 予約_引取_希望日時時刻
                    .Append("       t1.REZ_PICK_LOC, ")                              ' 27 予約_引取_場所
                    .Append("       t1.REZ_PICK_TIME, ")                             ' 28 予約_引取_所要時間
                    .Append("       t1.REZ_DELI_DATE, ")                             ' 30 予約_納車_希望日時時刻
                    .Append("       t1.REZ_DELI_LOC, ")                              ' 31 予約_納車_場所
                    .Append("       t1.REZ_DELI_TIME, ")                             ' 32 予約_納車_所要時間
                    .Append("       t1.MERCHANDISECD, ")                             ' 08 商品コード
                    .Append("       '99', ")                                         ' 48 送受信完了フラグ
                    .Append("       sysdate, ")                                      ' 49 送受信日時
                    .Append("       '', ")                                           ' 50 データ発生サーバ
                    .Append("       :INPUTACCOUNT, ")                                ' 45 入力オペレータ
                    .Append("       sysdate, ")                                      ' 46 作成日
                    .Append("       sysdate ")                                       ' 47 更新日
                    .Append("  FROM tbl_STALLPROCESS t1 ")
                    .Append(" WHERE t1.DLRCD = :DLRCD ")
                    .Append("   AND t1.STRCD = :STRCD ")
                    .Append("   AND t1.REZID = :REZID ")
                    .Append("   AND t1.DSEQNO = :DSEQNO ")
                    .Append("   AND t1.SEQNO = (SELECT MAX(t2.SEQNO) ")
                    .Append("                     FROM tbl_STALLPROCESS t2 ")
                    .Append("                    WHERE t1.DLRCD = t2.DLRCD ")
                    .Append("                      AND t1.STRCD = t2.STRCD ")
                    .Append("                      AND t1.REZID = t2.REZID ")
                    .Append("                      AND t1.DSEQNO = t2.DSEQNO ")
                    .Append("                   ) ")
                End If
            End With

            ' DbUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("SC3150101_005")

                query.CommandText = sql.ToString()

                ' バインド変数定義
                If drProcInfo.SEQNO <= 1 Then
                    query.AddParameterWithTypeValue("DSEQNO", OracleDbType.Int64, drProcInfo.DSEQNO)                                  ' 04 日跨ぎシーケンス番号
                    query.AddParameterWithTypeValue("RESULT_STATUS", OracleDbType.Char, drProcInfo.RESULT_STATUS)                     ' 16 実績_ステータス
                    query.AddParameterWithTypeValue("RESULT_START_TIME", OracleDbType.Char, drProcInfo.RESULT_START_TIME)             ' 18 実績_ストール開始日時時刻
                    query.AddParameterWithTypeValue("RESULT_END_TIME", OracleDbType.Char, drProcInfo.RESULT_END_TIME)                 ' 19 実績_ストール終了日時時刻
                    query.AddParameterWithTypeValue("RESULT_IN_TIME", OracleDbType.Char, drProcInfo.RESULT_IN_TIME)                   ' 20 実績_入庫時間
                    query.AddParameterWithTypeValue("RESULT_WASH_START", OracleDbType.Char, drProcInfo.RESULT_WASH_START)             ' 34 洗車開始時刻
                    query.AddParameterWithTypeValue("RESULT_WASH_END", OracleDbType.Char, drProcInfo.RESULT_WASH_END)                 ' 35 洗車終了時刻
                    query.AddParameterWithTypeValue("RESULT_INSPECTION_START", OracleDbType.Char, drProcInfo.RESULT_INSPECTION_START) ' 51 実績検査開始時刻
                    query.AddParameterWithTypeValue("RESULT_INSPECTION_END", OracleDbType.Char, drProcInfo.RESULT_INSPECTION_END)     ' 52 実績検査終了時刻
                    query.AddParameterWithTypeValue("RESULT_WAIT_START", OracleDbType.Char, drProcInfo.RESULT_WAIT_START)             ' 36 納車待ち開始時刻
                    query.AddParameterWithTypeValue("RESULT_WAIT_END", OracleDbType.Char, drProcInfo.RESULT_WAIT_END)                 ' 37 納車待ち終了時刻
                    query.AddParameterWithTypeValue("INPUTACCOUNT", OracleDbType.Varchar2, updateAccount)                        ' 45 入力オペレータ
                    query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, drProcInfo.DLRCD)                                     ' 01 販売店コード
                    query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, drProcInfo.STRCD)                                     ' 02 店舗コード
                    query.AddParameterWithTypeValue("REZID", OracleDbType.Int64, drProcInfo.REZID)                                    ' 03 予約ID
                Else
                    query.AddParameterWithTypeValue("RESULT_STATUS", OracleDbType.Char, drProcInfo.RESULT_STATUS)                     ' 16 実績_ステータス
                    'query.AddParameterWithTypeValue("RESULT_STALLID", OracleDbType.Int64, recDr("StallID"))                      ' 17 実績_ストールID
                    query.AddParameterWithTypeValue("RESULT_STALLID", OracleDbType.Int64, drProcInfo.RESULT_STALLID)
                    query.AddParameterWithTypeValue("RESULT_START_TIME", OracleDbType.Char, drProcInfo.RESULT_START_TIME)             ' 18 実績_ストール開始日時時刻
                    query.AddParameterWithTypeValue("RESULT_END_TIME", OracleDbType.Char, drProcInfo.RESULT_END_TIME)                 ' 19 実績_ストール終了日時時刻
                    query.AddParameterWithTypeValue("RESULT_IN_TIME", OracleDbType.Char, drProcInfo.RESULT_IN_TIME)                   ' 20 実績_入庫時間
                    'query.AddParameterWithTypeValue("REZ_START_TIME", OracleDbType.Char, recDr("RezStartTime"))                  ' 23 予定_ストール開始日時時刻
                    query.AddParameterWithTypeValue("REZ_START_TIME", OracleDbType.Char, drProcInfo.REZ_START_TIME)
                    'query.AddParameterWithTypeValue("REZ_END_TIME", OracleDbType.Char, recDr("RezEndTime"))                      ' 24 予定_ストール終了日時時刻
                    query.AddParameterWithTypeValue("REZ_END_TIME", OracleDbType.Char, drProcInfo.REZ_END_TIME)
                    If relocate Then
                        'query.AddParameterWithTypeValue("REZ_WORK_TIME", OracleDbType.Int64, recDr("RezWorkTime"))               ' 25 予定作業時間
                        query.AddParameterWithTypeValue("REZ_WORK_TIME", OracleDbType.Int64, drProcInfo.REZ_WORK_TIME)
                    End If
                    query.AddParameterWithTypeValue("RESULT_WASH_START", OracleDbType.Char, drProcInfo.RESULT_WASH_START)             ' 34 洗車開始時刻
                    query.AddParameterWithTypeValue("RESULT_WASH_END", OracleDbType.Char, drProcInfo.RESULT_WASH_END)                 ' 35 洗車終了時刻
                    query.AddParameterWithTypeValue("RESULT_INSPECTION_START", OracleDbType.Char, drProcInfo.RESULT_INSPECTION_START) ' 51 実績検査開始時刻
                    query.AddParameterWithTypeValue("RESULT_INSPECTION_END", OracleDbType.Char, drProcInfo.RESULT_INSPECTION_END)     ' 52 実績検査終了時刻
                    query.AddParameterWithTypeValue("RESULT_WAIT_START", OracleDbType.Char, drProcInfo.RESULT_WAIT_START)             ' 36 納車待ち開始時刻
                    query.AddParameterWithTypeValue("RESULT_WAIT_END", OracleDbType.Char, drProcInfo.RESULT_WAIT_END)                 ' 37 納車待ち終了時刻
                    query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, drProcInfo.DLRCD)                                     ' 01 販売店コード
                    query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, drProcInfo.STRCD)                                     ' 02 店舗コード
                    query.AddParameterWithTypeValue("REZID", OracleDbType.Int64, drProcInfo.REZID)                                    ' 03 予約ID
                    query.AddParameterWithTypeValue("DSEQNO", OracleDbType.Int64, drProcInfo.DSEQNO)                                  ' 04 日跨ぎシーケンス番号
                End If

                Logger.Info("[E]InsertStallProcessInfo()")

                'SQL実行
                Return query.Execute()

            End Using

        End Function


        ''' <summary>
        ''' ストール予約履歴の登録
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="reserveId">予約ID</param>
        ''' <param name="insertType"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function InsertReserveHistory(ByVal dealerCode As String, _
                                             ByVal branchCode As String, _
                                             ByVal reserveId As Integer, _
                                             ByVal insertType As Integer) As Integer 'InsertRezHistory

            Logger.Info("[S]InsertReserveHistory()")

            Dim sql As New StringBuilder

            ' SQL文の作成
            With sql
                .Append("INSERT /* SC3150101_006 */ ")
                .Append("  INTO tbl_STALLREZHIS ( ")
                .Append("       DLRCD, ")
                .Append("       STRCD, ")
                .Append("       REZID, ")
                .Append("       SEQNO, ")
                .Append("       UPDDVSID, ")
                .Append("       STALLID, ")
                .Append("       STARTTIME, ")
                .Append("       ENDTIME, ")
                .Append("       CUSTCD, ")
                .Append("       PERMITID, ")
                .Append("       CUSTOMERNAME, ")
                .Append("       TELNO, ")
                .Append("       MOBILE, ")
                .Append("       EMAIL1, ")
                .Append("       VEHICLENAME, ")
                .Append("       VCLREGNO, ")
                .Append("       SERVICECODE, ")
                .Append("       SERVICECODE_S, ")
                .Append("       REZDATE, ")
                .Append("       NETREZID, ")
                .Append("       STATUS, ")
                .Append("       INSDID, ")
                .Append("       VIN, ")
                .Append("       CUSTOMERFLAG, ")
                .Append("       CUSTVCLRE_SEQNO, ")
                .Append("       SERVICEMSTCD, ")
                .Append("       ZIPCODE, ")
                .Append("       ADDRESS, ")
                .Append("       MODELCODE, ")
                .Append("       MILEAGE, ")
                .Append("       WASHFLG, ")
                .Append("       INSPECTIONFLG, ")
                .Append("       WALKIN, ")
                .Append("       REZ_RECEPTION, ")
                .Append("       REZ_WORK_TIME, ")
                .Append("       REZ_PICK_DATE, ")
                .Append("       REZ_PICK_LOC, ")
                .Append("       REZ_PICK_TIME, ")
                .Append("       REZ_DELI_DATE, ")
                .Append("       REZ_DELI_LOC, ")
                .Append("       REZ_DELI_TIME, ")
                .Append("       UPDATE_COUNT, ")
                .Append("       STOPFLG, ")
                .Append("       PREZID, ")
                .Append("       REZCHILDNO, ")
                .Append("       ACTUAL_STIME, ")
                .Append("       ACTUAL_ETIME, ")
                .Append("       CRRY_TYPE, ")
                .Append("       CRRYINTIME, ")
                .Append("       CRRYOUTTIME, ")
                .Append("       MEMO, ")
                .Append("       STRDATE, ")
                .Append("       NETDEVICESFLG, ")
                .Append("       INPUTACCOUNT, ")
                .Append("       INFOUPDATEDATE, ")
                .Append("       INFOUPDATEACCOUNT, ")
                .Append("       CREATEDATE, ")
                .Append("       UPDATEDATE, ")
                .Append("       HIS_FLG, ")
                .Append("       MERCHANDISECD, ")
                .Append("       BASREZID, ")
                .Append("       ACCOUNT_PLAN, ")
                .Append("       RSSTATUS, ")
                .Append("       RSDATE, ")
                .Append("       UPDATESERVER, ")
                .Append("       REZTYPE, ")
                .Append("       CRCUSTID, ")
                .Append("       CUSTOMERCLASS, ")
                .Append("       STALLWAIT_REZID ")
                .Append("     , ORDERNO ")
                .Append("       ) ")
                .Append("SELECT DLRCD, ")
                .Append("       STRCD, ")
                .Append("       REZID, ")
                If insertType = 0 Then
                    .Append("       1, ")
                Else
                    .Append("       (")
                    .Append("        SELECT NVL(MAX(SEQNO) + 1, 1) ")
                    .Append("          FROM tbl_STALLREZHIS t2 ")
                    .Append("         WHERE t2.DLRCD = t1.DLRCD ")
                    .Append("           AND t2.STRCD = t1.STRCD ")
                    .Append("           AND t2.REZID = t1.REZID ), ")
                End If
                If insertType = 2 Then
                    .Append("       '1', ")
                Else
                    .Append("       '0', ")
                End If
                .Append("       STALLID, ")
                .Append("       STARTTIME, ")
                .Append("       ENDTIME, ")
                .Append("       CUSTCD, ")
                .Append("       PERMITID, ")
                .Append("       CUSTOMERNAME, ")
                .Append("       TELNO, ")
                .Append("       MOBILE, ")
                .Append("       EMAIL1, ")
                .Append("       VEHICLENAME, ")
                .Append("       VCLREGNO, ")
                .Append("       SERVICECODE, ")
                .Append("       SERVICECODE_S, ")
                .Append("       REZDATE, ")
                .Append("       NETREZID, ")
                .Append("       STATUS, ")
                .Append("       INSDID, ")
                .Append("       VIN, ")
                .Append("       CUSTOMERFLAG, ")
                .Append("       CUSTVCLRE_SEQNO, ")
                .Append("       SERVICEMSTCD, ")
                .Append("       ZIPCODE, ")
                .Append("       ADDRESS, ")
                .Append("       MODELCODE, ")
                .Append("       MILEAGE, ")
                .Append("       WASHFLG, ")
                .Append("       INSPECTIONFLG, ")
                .Append("       WALKIN, ")
                .Append("       REZ_Reception, ")
                .Append("       REZ_WORK_TIME, ")
                .Append("       REZ_PICK_DATE, ")
                .Append("       REZ_PICK_LOC, ")
                .Append("       REZ_PICK_TIME, ")
                .Append("       REZ_DELI_DATE, ")
                .Append("       REZ_DELI_LOC, ")
                .Append("       REZ_DELI_TIME, ")
                .Append("       UPDATE_COUNT, ")
                .Append("       STOPFLG, ")
                .Append("       PREZID, ")
                .Append("       REZCHILDNO, ")
                .Append("       ACTUAL_STIME, ")
                .Append("       ACTUAL_ETIME, ")
                .Append("       CRRY_TYPE, ")
                .Append("       CRRYINTIME, ")
                .Append("       CRRYOUTTIME, ")
                .Append("       MEMO, ")
                .Append("       STRDATE, ")
                .Append("       NETDEVICESFLG, ")
                .Append("       INPUTACCOUNT, ")
                .Append("       UPDATEDATE, ")
                .Append("       UPDATEACCOUNT, ")
                .Append("       sysdate, ")
                .Append("       sysdate, ")
                If insertType = 0 Then
                    .Append("       '0', ")
                ElseIf insertType = 2 Then
                    .Append("       '2', ")
                Else
                    .Append("       '1', ")
                End If
                .Append("       MERCHANDISECD, ")
                .Append("       BASREZID, ")
                .Append("       ACCOUNT_PLAN, ")
                .Append("       '99', ")
                .Append("       sysdate, ")
                .Append("       '', ")
                .Append("       REZTYPE, ")
                .Append("       CRCUSTID, ")
                .Append("       CUSTOMERCLASS, ")
                .Append("       STALLWAIT_REZID ")
                .Append("     , ORDERNO ")
                .Append("  FROM tbl_STALLREZINFO t1 ")
                .Append(" WHERE DLRCD = :DLRCD1 ")
                .Append("   AND STRCD = :STRCD1 ")
                If insertType <> 3 Then
                    .Append("   AND REZID = :REZID1 ")
                Else
                    .Append("   AND PREZID = ( ")
                    .Append("                 SELECT PREZID ")
                    .Append("                   FROM tbl_STALLREZINFO t3 ")
                    .Append("                  WHERE DLRCD = :DLRCD2 ")
                    .Append("                    AND STRCD = :STRCD2 ")
                    .Append("                    AND REZID = :REZID2 ")
                    .Append("                ) ")
                    .Append("   AND REZID <> :REZID3 ")
                    .Append("   AND CANCELFLG = '0'")
                End If

            End With
            ' DbUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("SC3150101_006")

                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("DLRCD1", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("STRCD1", OracleDbType.Char, branchCode)
                If insertType <> 3 Then
                    query.AddParameterWithTypeValue("REZID1", OracleDbType.Int64, reserveId)
                Else
                    query.AddParameterWithTypeValue("DLRCD2", OracleDbType.Char, dealerCode)
                    query.AddParameterWithTypeValue("STRCD2", OracleDbType.Char, branchCode)
                    query.AddParameterWithTypeValue("REZID2", OracleDbType.Int64, reserveId)
                    query.AddParameterWithTypeValue("REZID3", OracleDbType.Int64, reserveId)
                End If

                Logger.Info("[E]InsertReserveHistory()")

                'SQL実行
                Return query.Execute()

            End Using

        End Function


        ''' <summary>
        ''' ストール時間情報の取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="stallId">ストールID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetStallTimeInfo(ByVal dealerCode As String, _
                                         ByVal branchCode As String, _
                                         ByVal stallId As Integer) As SC3150101DataSet.SC3150101StallTimeInfoDataTable

            Logger.Info("[S]GetStallTimeInfo()")

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3150101DataSet.SC3150101StallTimeInfoDataTable)("SC3150101_007")
                Dim sql As New StringBuilder

                ' SQL文の作成
                With sql
                    .Append("    SELECT /* SC3150101_007 */ ")
                    .Append("           t1.DLRCD AS DLRCD, ")                ' 販売店コード
                    .Append("           t1.STRCD AS STRCD, ")                ' 店舗コード
                    .Append("           t1.STALLID AS STALLID, ")            ' ストールID
                    .Append("           t1.STALLNAME AS STALLNAME, ")        ' ストール名称
                    .Append("           t1.STALLNAME_S AS STALLNAME_S, ")    ' ストール省略名称
                    .Append("           t2.STARTTIME AS STARTTIME, ")        ' 開始時間
                    .Append("           t2.ENDTIME AS ENDTIME, ")            ' 終了時間
                    .Append("           t2.TIMEINTERVAL AS TIMEINTERVAL, ")  ' 時間間隔
                    .Append("           t2.PSTARTTIME AS PSTARTTIME, ")      ' プログレス開始時間
                    .Append("           t2.PENDTIME AS PENDTIME ")           ' プログレス終了時間
                    .Append("      FROM tbl_STALL t1 ")                      ' [ストールマスタ]
                    .Append("INNER JOIN tbl_STALLTIME t2 ")                  ' [ストール時間]
                    .Append("        ON t1.DLRCD = t2.DLRCD ")               ' 販売店コード
                    .Append("       AND t1.STRCD = t2.STRCD ")               ' 店舗コード
                    .Append("  WHERE t1.DLRCD = :DLRCD ")                    ' 販売店コード
                    .Append("    AND t1.STRCD = :STRCD ")                    ' 店舗コード
                    .Append("    AND t1.STALLID = :STALLID")                 ' ストールID
                End With

                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
                query.AddParameterWithTypeValue("STALLID", OracleDbType.Int64, stallId)

                Logger.Info("[E]GetStallTimeInfo()")

                ' 検索結果の返却
                Return query.GetData()

            End Using

        End Function


        ''' <summary>
        ''' 指定範囲内のストール予約情報を取得する。
        ''' </summary>
        ''' <param name="dealerCode">販売点コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="stallId">ストールID</param>
        ''' <param name="reserveId">予約ID</param>
        ''' <param name="fromDate">範囲時間(FROM)</param>
        ''' <param name="toDate">範囲時間(TO)</param>
        ''' <returns>処理結果</returns>
        ''' <remarks></remarks>
        Public Function GetStallReserveList(ByVal dealerCode As String, _
                                            ByVal branchCode As String, _
                                            ByVal stallId As Integer, _
                                            ByVal reserveId As Integer, _
                                            ByVal fromDate As Date, _
                                            ByVal toDate As Date) As SC3150101DataSet.SC3150101StallReserveListDataTable

            Logger.Info("[S]GetStallReserveList()")

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3150101DataSet.SC3150101StallReserveListDataTable)("SC3150101_008")

                Dim sql As New StringBuilder

                ' SQL文の作成
                With sql
                    .Append(" SELECT /* SC3150101_008 */ ")
                    .Append("        DLRCD, ")                ' 01 販売店コード
                    .Append("        STRCD, ")                ' 02 店舗コード
                    .Append("        REZID, ")                ' 03 予約ID
                    .Append("        STALLID, ")              ' 05 ストールID
                    .Append("        STARTTIME, ")            ' 06 使用開始日時
                    .Append("        ENDTIME, ")              ' 07 使用終了日時
                    .Append("        STATUS, ")               ' 19 ステータス
                    .Append("        WASHFLG, ")              ' 30 洗車フラグ
                    .Append("        REZ_RECEPTION, ")        ' 33 予約_受付納車区分
                    .Append("        REZ_WORK_TIME, ")        ' 34 予定_作業時間
                    .Append("        REZ_PICK_DATE, ")        ' 35 予約_引取_希望日時時刻
                    .Append("        REZ_DELI_DATE, ")        ' 39 予約_納車_希望日時時刻
                    .Append("        STOPFLG, ")              ' 44 中断フラグ
                    .Append("        STRDATE, ")              ' 54 入庫時間
                    .Append("        CANCELFLG, ")            ' 58 キャンセルフラグ
                    .Append("        INSPECTIONFLG ")         ' 67 検査フラグ
                    .Append("   FROM tbl_STALLREZINFO ")      ' [ストール予約]
                    .Append("  WHERE DLRCD = :DLRCD ")        ' 01 販売店コード
                    .Append("    AND STRCD = :STRCD ")        ' 02 店舗コード
                    .Append("    AND STALLID = :STALLID ")
                    .Append("    AND ( ")
                    .Append("         ( ")
                    '.Append("          STARTTIME < :STARTTIME ")
                    '.Append("      AND ENDTIME > :ENDTIME ")
                    .Append("          STARTTIME < TO_DATE(:STARTTIME, 'YYYY/MM/DD HH24:MI:SS') ")
                    .Append("      AND ENDTIME > TO_DATE(:ENDTIME, 'YYYY/MM/DD HH24:MI:SS') ")
                    .Append("          ) ")
                    .Append("       OR REZID = :REZID ")
                    .Append("         ) ")
                    .Append("    AND STATUS < 3 ")
                    .Append("    AND ( ")
                    .Append("         CANCELFLG = '0' ")
                    .Append("     OR ( ")
                    .Append("         CANCELFLG = '1' ")
                    .Append("     AND STOPFLG = '1' ")
                    .Append("         ) ")
                    .Append("        )")

                End With

                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
                query.AddParameterWithTypeValue("STALLID", OracleDbType.Int64, stallId)
                'query.AddParameterWithTypeValue("STARTTIME", OracleDbType.Date, CType(toDate.ToString("yyyy/MM/dd HH:mm:00"), Date))
                'query.AddParameterWithTypeValue("ENDTIME", OracleDbType.Date, CType(fromDate.ToString("yyyy/MM/dd HH:mm:00"), Date))
                query.AddParameterWithTypeValue("STARTTIME", OracleDbType.Char, toDate.ToString("yyyy/MM/dd HH:mm:00", CultureInfo.InvariantCulture()))
                query.AddParameterWithTypeValue("ENDTIME", OracleDbType.Char, fromDate.ToString("yyyy/MM/dd HH:mm:00", CultureInfo.InvariantCulture()))
                query.AddParameterWithTypeValue("REZID", OracleDbType.Int64, reserveId)

                Logger.Info("[E]GetStallReserveList()")

                ' 検索結果の返却
                Return query.GetData()

            End Using

        End Function


        ''' <summary>
        ''' 指定範囲内のストール実績情報を取得する。
        ''' </summary>
        ''' <param name="dealerCode">販売点コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="stallId">ストールID</param>
        ''' <param name="fromDate">範囲時間(FROM)</param>
        ''' <param name="toDate">範囲時間(TO)</param>
        ''' <returns>処理結果</returns>
        ''' <remarks></remarks>
        Public Function GetStallProcessList(ByVal dealerCode As String, _
                                            ByVal branchCode As String, _
                                            ByVal stallId As Integer, _
                                            ByVal fromDate As Date, _
                                            ByVal toDate As Date) As SC3150101DataSet.SC3150101StallProcessListDataTable

            Logger.Info("[S]GetStallProcessList()")

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3150101DataSet.SC3150101StallProcessListDataTable)("SC3150101_009")

                Dim sql As New StringBuilder

                ' SQL文の作成
                With sql
                    .Append("    SELECT /* SC3150101_009 */ ")
                    .Append("           t1.DLRCD AS DLRCD, ")                                        ' 01 販売店コード
                    .Append("           t1.STRCD AS STRCD, ")                                        ' 02 店舗コード
                    .Append("           t1.REZID AS REZID, ")                                        ' 03 予約ID
                    .Append("           NVL(t2.DSEQNO, 0) AS DSEQNO, ")                              ' 04 日跨ぎシーケンス番号
                    .Append("           NVL(t2.SEQNO, 0) AS SEQNO, ")                                ' 05 シーケンス番号
                    .Append("           t2.RESULT_STATUS AS RESULT_STATUS, ")                        ' 16 実績_ステータス
                    .Append("           NVL(t2.RESULT_STALLID, 0) AS RESULT_STALLID, ")              ' 17 実績_ストールID
                    .Append("           t2.RESULT_START_TIME AS RESULT_START_TIME, ")                ' 18 実績_ストール開始日時時刻
                    .Append("           t2.RESULT_END_TIME AS RESULT_END_TIME, ")                    ' 19 実績_ストール終了日時時刻
                    .Append("           NVL(t2.RESULT_WORK_TIME, 0) AS RESULT_WORK_TIME, ")          ' 21 実績_実績時間
                    .Append("           t2.RESULT_IN_TIME AS RESULT_IN_TIME, ")                      ' 20 実績_入庫時間
                    .Append("           t2.RESULT_WASH_START AS RESULT_WASH_START, ")                ' 34 洗車開始時刻
                    .Append("           t2.RESULT_WASH_END AS RESULT_WASH_END, ")                    ' 35 洗車終了時刻
                    .Append("           t2.RESULT_WAIT_START AS RESULT_WAIT_START, ")                ' 36 納車待ち開始時刻
                    .Append("           t2.RESULT_WAIT_END AS RESULT_WAIT_END, ")                    ' 37 納車待ち終了時刻
                    .Append("           t2.RESULT_INSPECTION_START AS RESULT_INSPECTION_START, ")    ' 51 実績検査開始時刻
                    .Append("           t2.RESULT_INSPECTION_END AS RESULT_INSPECTION_END ")         ' 52 実績検査終了時刻
                    .Append("      FROM tbl_STALLREZINFO t1 ")                                       ' [ストール予約]
                    .Append(" LEFT JOIN tbl_STALLPROCESS t2 ")                                       ' [ストール実績]
                    .Append("        ON t2.DLRCD = t1.DLRCD ")                                       ' 01 販売店コード
                    .Append("       AND t2.STRCD = t1.STRCD ")                                       ' 02 店舗コード
                    .Append("       AND t2.REZID = t1.REZID ")                                       ' 03 予約ID
                    .Append("     WHERE t1.DLRCD = :DLRCD ")                                         ' 01 販売店コード
                    .Append("       AND t1.STRCD = :STRCD ")                                         ' 02 店舗コード
                    .Append("       AND t1.STALLID = :STALLID ")                                     ' 05 ストールID
                    '.Append("       AND t1.STARTTIME < :STARTTIME ")                                 ' 06 使用開始日時
                    '.Append("       AND t1.ENDTIME > :ENDTIME ")                                     ' 07 使用終了日時
                    .Append("       AND t1.STARTTIME < TO_DATE(:STARTTIME, 'YYYY/MM/DD HH24:MI:SS') ") ' 06 使用開始日時
                    .Append("       AND t1.ENDTIME > TO_DATE(:ENDTIME, 'YYYY/MM/DD HH24:MI:SS') ")     ' 07 使用終了日時
                    .Append("       AND t1.STATUS < 3 ")                                             ' 19 ステータス
                    .Append("       AND t1.CANCELFLG = '0' ")                                        ' 58 キャンセルフラグ
                    .Append("       AND (t2.SEQNO IS NULL ")                                         ' 05 シーケンス番号
                    .Append("           OR (t2.DSEQNO = (SELECT MAX(t3.DSEQNO) ")                    ' 04 日跨ぎシーケンス番号
                    .Append("                              FROM tbl_STALLPROCESS t3 ")               ' [ストール実績]
                    .Append("                             WHERE t3.DLRCD = t2.DLRCD ")               ' 01 販売店コード
                    .Append("                               AND t3.STRCD = t2.STRCD ")               ' 02 店舗コード
                    .Append("                               AND t3.REZID = t2.REZID ")               ' 03 予約ID
                    .Append("                          GROUP BY t3.DLRCD, t3.STRCD, t3.REZID) ")
                    .Append("          AND t2.SEQNO = (SELECT MAX(t4.SEQNO) ")                       ' 05 シーケンス番号
                    .Append("                            FROM tbl_STALLPROCESS t4 ")                 ' [ストール実績]
                    .Append("                           WHERE t4.DLRCD = t2.DLRCD ")                 ' 01 販売店コード
                    .Append("                             AND t4.STRCD = t2.STRCD ")                 ' 02 店舗コード
                    .Append("                             AND t4.REZID = t2.REZID ")                 ' 03 予約ID
                    .Append("                             AND t4.DSEQNO = t2.DSEQNO) ")              ' 04 日跨ぎシーケンス番号
                    .Append("              ) ")
                    .Append("           )")
                End With

                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
                query.AddParameterWithTypeValue("STALLID", OracleDbType.Int64, stallId)
                'query.AddParameterWithTypeValue("STARTTIME", OracleDbType.Date, CType(toDate.ToString("yyyy/MM/dd HH:mm:00"), Date))
                'query.AddParameterWithTypeValue("ENDTIME", OracleDbType.Date, CType(fromDate.ToString("yyyy/MM/dd HH:mm:00"), Date))
                query.AddParameterWithTypeValue("STARTTIME", OracleDbType.Char, toDate.ToString("yyyy/MM/dd HH:mm:00", CultureInfo.InvariantCulture()))
                query.AddParameterWithTypeValue("ENDTIME", OracleDbType.Char, fromDate.ToString("yyyy/MM/dd HH:mm:00", CultureInfo.InvariantCulture()))

                Logger.Info("[E]GetStallProcessList()")

                ' 検索結果の返却
                Return query.GetData()

            End Using

        End Function


        ''' <summary>
        ''' 担当者実績情報の取得
        ''' </summary>
        ''' <param name="stallID">ストールID</param>
        ''' <param name="reserveId">予約ID</param>
        ''' <param name="workDate">作業日付</param>
        ''' <param name="midFinish">当日処理判定値</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetStaffResultInfo(ByVal stallId As Integer, _
                                           ByVal reserveId As Integer, _
                                           ByVal workDate As DateTime, _
                                           Optional ByVal midFinish As Boolean = False) As SC3150101DataSet.SC3150101StaffResultInfoDataTable

            Logger.Info("[S]GetStaffResultInfo()")

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3150101DataSet.SC3150101StaffResultInfoDataTable)("SC3150101_010")

                Dim sql As New StringBuilder

                ' SQL文の作成
                With sql
                    .Append("    SELECT /* SC3150101_010 */ ")
                    .Append("           t2.DSEQNO AS DSEQNO, ")
                    .Append("           t2.SEQNO AS SEQNO, ")
                    .Append("           t2.RESULT_STATUS AS RESULT_STATUS, ")
                    .Append("           t2.RESULT_END_TIME AS RESULT_END_TIME ")
                    .Append("      FROM tbl_TSTAFFSTALL t1 ")
                    .Append("INNER JOIN tbl_STALLPROCESS t2 ")
                    .Append("        ON t1.DLRCD = t2.DLRCD ")
                    .Append("       AND t1.STRCD = t2.STRCD ")
                    .Append("       AND t1.REZID = t2.REZID ")
                    .Append("     WHERE t1.STALLID = :STALLID ")
                    .Append("       AND t1.REZID = :REZID ")
                    '.Append("       AND t1.WORKDATE = :WORKDATE ")
                    .Append("       AND t1.WORKDATE = TO_DATE(:WORKDATE, 'YYYY/MM/DD') ")
                    If midFinish Then
                        .Append("       AND t2.DSEQNO = (SELECT MAX(t3.DSEQNO) - 1 ")
                    Else
                        .Append("       AND t2.DSEQNO = (SELECT MAX(t3.DSEQNO) ")
                    End If
                    .Append("                          FROM tbl_STALLPROCESS t3 ")
                    .Append("                         WHERE t3.DLRCD = t2.DLRCD ")
                    .Append("                           AND t3.STRCD = t2.STRCD ")
                    .Append("                           AND t3.REZID = t2.REZID ")
                    .Append("                      GROUP BY t3.DLRCD, t3.STRCD, t3.REZID) ")
                    .Append("       AND t2.SEQNO = (SELECT MAX(t4.SEQNO) ")
                    .Append("                         FROM tbl_STALLPROCESS t4 ")
                    .Append("                        WHERE t4.DLRCD = t2.DLRCD ")
                    .Append("                          AND t4.STRCD = t2.STRCD ")
                    .Append("                          AND t4.REZID = t2.REZID ")
                    .Append("                          AND t4.DSEQNO = t2.DSEQNO ")
                    .Append("                       ) ")
                End With

                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("STALLID", OracleDbType.Int64, stallId)
                query.AddParameterWithTypeValue("REZID", OracleDbType.Int64, reserveId)
                'query.AddParameterWithTypeValue("WORKDATE", OracleDbType.Date, CType(workDate.ToString("yyyy/MM/dd"), Date))
                query.AddParameterWithTypeValue("WORKDATE", OracleDbType.Char, workDate.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture()))

                Logger.Info("[E]GetStaffResultInfo()")

                'SQL実行
                Return query.GetData()

            End Using

        End Function


        ''' <summary>
        ''' 担当者実績情報の作成
        ''' </summary>
        ''' <param name="stallId">ストールID</param>
        ''' <param name="reserveId">予約ID</param>
        ''' <param name="workDate">作業日付</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function InsertStaffStall(ByVal stallId As Integer, _
                                         ByVal reserveId As Integer, _
                                         ByVal workDate As Date) As Integer

            Logger.Info("[S]InsertStaffStall()")

            Dim sql As New StringBuilder

            ' SQL文の作成
            With sql
                .Append("    INSERT /* SC3150101_011 */ ")
                .Append("      INTO tbl_TSTAFFSTALL ( ")
                .Append("                            DLRCD, ")
                .Append("                            STRCD, ")
                .Append("                            REZID, ")
                .Append("                            DSEQNO, ")
                .Append("                            SEQNO, ")
                .Append("                            SSEQNO, ")
                .Append("                            STAFFCD, ")
                .Append("                            WORKDATE, ")
                .Append("                            STALLID, ")
                .Append("                            CREATEDATE, ")
                .Append("                            RSSTATUS, ")
                .Append("                            RSDATE, ")
                .Append("                            UPDATESERVER, ")
                .Append("                            WORK_START, ")
                .Append("                            WORK_END ")
                .Append("                           ) ")
                .Append("    SELECT t2.DLRCD, ")
                .Append("           t2.STRCD, ")
                .Append("           t2.REZID, ")
                .Append("           t2.DSEQNO, ")
                .Append("           t2.SEQNO, ")
                .Append("           0, ")
                .Append("           t1.STAFFCD, ")
                .Append("           TO_DATE(:WORKDATE1, 'YYYY/MM/DD'), ")
                .Append("           t1.STALLID, ")
                .Append("           sysdate, ")
                .Append("           '00', ")
                .Append("           NULL, ")
                .Append("           '', ")
                .Append("           t2.RESULT_START_TIME, ")
                .Append("           NULL ")
                .Append("      FROM tbl_WSTAFFSTALL t1 ")
                .Append("INNER JOIN tbl_STALLPROCESS t2 ")
                .Append("        ON t1.DLRCD = t2.DLRCD ")
                .Append("       AND t1.STRCD = t2.STRCD ")
                .Append("     WHERE t1.STALLID = :STALLID ")
                .Append("       AND t1.WORKDATE = :WORKDATE2 ")
                .Append("       AND t2.REZID = :REZID ")
                .Append("       AND t2.DSEQNO = (SELECT MAX(t3.DSEQNO) ")
                .Append("                          FROM tbl_STALLPROCESS t3 ")
                .Append("                         WHERE t3.DLRCD = t2.DLRCD ")
                .Append("                           AND t3.STRCD = t2.STRCD ")
                .Append("                           AND t3.REZID = t2.REZID ")
                .Append("                      GROUP BY t3.DLRCD, t3.STRCD, t3.REZID) ")
                .Append("       AND t2.SEQNO = (SELECT MAX(t4.SEQNO) ")
                .Append("                         FROM tbl_STALLPROCESS t4 ")
                .Append("                        WHERE t4.DLRCD = t2.DLRCD ")
                .Append("                          AND t4.STRCD = t2.STRCD ")
                .Append("                          AND t4.REZID = t2.REZID ")
                .Append("                          AND t4.DSEQNO = t2.DSEQNO ")
                .Append("                       ) ")
            End With

            ' DbUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("SC3150101_011")

                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("WORKDATE1", OracleDbType.Char, workDate.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture()))
                query.AddParameterWithTypeValue("STALLID", OracleDbType.Int64, stallId)
                query.AddParameterWithTypeValue("WORKDATE2", OracleDbType.Char, SetSqlValue(workDate.ToString("yyyyMMdd", CultureInfo.InvariantCulture())))
                query.AddParameterWithTypeValue("REZID", OracleDbType.Int64, reserveId)

                Logger.Info("[E]InsertStaffStall()")

                'SQL実行
                Return query.Execute()

            End Using

        End Function


        ''' <summary>
        ''' 担当者実績情報の更新(実績ステータス："10"=入庫)
        ''' </summary>
        ''' <param name="stallId">ストールID</param>
        ''' <param name="reserveId">予約ID</param>
        ''' <param name="daySeqNo">日跨ぎシーケンス番号</param>
        ''' <param name="seqNo">シーケンス番号</param>
        ''' <param name="workDate">作業日付</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function DeleteStaffStall(ByVal stallId As Integer, _
                                         ByVal reserveId As Integer, _
                                         ByVal daySeqNo As Integer, _
                                         ByVal seqNo As Integer, _
                                         ByVal workDate As Date) As Integer

            Logger.Info("[S]DeleteStaffStall()")

            ' DbUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("SC3150101_012")

                Dim sql As New StringBuilder

                ' SQL文の作成
                With sql
                    'status = "10"
                    .Append("DELETE /* SC3150101_012 */ ")
                    .Append("  FROM tbl_TSTAFFSTALL t1 ")
                    .Append(" WHERE t1.STALLID = :STALLID ")
                    .Append("   AND t1.REZID = :REZID ")
                    .Append("   AND t1.DSEQNO = :DSEQNO ")
                    .Append("   AND t1.SEQNO = :SEQNO ")
                    .Append("   AND t1.WORKDATE = TO_DATE(:WORKDATE, 'YYYY/MM/DD') ")
                End With

                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("STALLID", OracleDbType.Int64, stallId)
                query.AddParameterWithTypeValue("REZID", OracleDbType.Int64, reserveId)
                query.AddParameterWithTypeValue("DSEQNO", OracleDbType.Int64, daySeqNo)
                query.AddParameterWithTypeValue("SEQNO", OracleDbType.Int64, seqNo)
                query.AddParameterWithTypeValue("WORKDATE", OracleDbType.Char, workDate.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture()))

                Logger.Info("[E]DeleteStaffStall()")

                'SQL実行
                Return query.Execute()

            End Using

        End Function


        ''' <summary>
        ''' 担当者実績情報の更新(実績ステータス："20"=作業中)
        ''' </summary>
        ''' <param name="stallID">ストールID</param>
        ''' <param name="reserveId">予約ID</param>
        ''' <param name="daySeqNo">日跨ぎシーケンス番号</param>
        ''' <param name="seqNo">シーケンス番号</param>
        ''' <param name="workDate">作業日付</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function UpdateStaffStallAtWork(ByVal stallId As Integer, _
                                               ByVal reserveId As Integer, _
                                               ByVal daySeqNo As Integer, _
                                               ByVal seqNo As Integer, _
                                               ByVal workDate As Date) As Integer

            Logger.Info("[S]UpdateStaffStallAtWork()")

            ' DbUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("SC3150101_013")

                Dim sql As New StringBuilder

                ' SQL文の作成
                With sql
                    '20
                    .Append("UPDATE /* SC3150101_013 */ ")
                    .Append("       tbl_TSTAFFSTALL t1 ")
                    .Append("   SET t1.WORK_END = NULL ")
                    .Append(" WHERE t1.STALLID = :STALLID ")
                    .Append("   AND t1.REZID = :REZID ")
                    .Append("   AND t1.DSEQNO = :DSEQNO ")
                    .Append("   AND t1.SEQNO = :SEQNO ")
                    .Append("   AND t1.SSEQNO = (SELECT MAX(t2.SSEQNO) ")
                    .Append("                      FROM tbl_TSTAFFSTALL t2 ")
                    .Append("                     WHERE t2.DLRCD = t1.DLRCD ")
                    .Append("                       AND t2.STRCD = t1.STRCD ")
                    .Append("                       AND t2.REZID = t1.REZID ")
                    .Append("                       AND t2.DSEQNO = t1.DSEQNO ")
                    .Append("                       AND t2.SEQNO = t1.SEQNO ) ")
                    '.Append("   AND t1.WORKDATE = :WORKDATE ")
                    .Append("   AND t1.WORKDATE = TO_DATE(:WORKDATE, 'YYYY/MM/DD') ")
                    .Append("   AND t1.WORK_END IS NOT NULL ")

                End With

                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("STALLID", OracleDbType.Int64, stallId)
                query.AddParameterWithTypeValue("REZID", OracleDbType.Int64, reserveId)
                query.AddParameterWithTypeValue("DSEQNO", OracleDbType.Int64, daySeqNo)
                query.AddParameterWithTypeValue("SEQNO", OracleDbType.Int64, seqNo)
                'query.AddParameterWithTypeValue("WORKDATE", OracleDbType.Date, CType(workDate.ToString("yyyy/MM/dd"), Date))
                query.AddParameterWithTypeValue("WORKDATE", OracleDbType.Char, workDate.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture()))

                Logger.Info("[E]UpdateStaffStallAtWork()")

                'SQL実行
                Return query.Execute()

            End Using

        End Function


        ''' <summary>
        ''' 担当者実績情報の更新(実績ステータス：実績ステータス："10"=入庫、"20"=作業中 以外)
        ''' </summary>
        ''' <param name="stallID">ストールID</param>
        ''' <param name="reserveId">予約ID</param>
        ''' <param name="daySeqNo">日跨ぎシーケンス番号</param>
        ''' <param name="seqNo">シーケンス番号</param>
        ''' <param name="workDate">作業日付</param>
        ''' <param name="endTime">作業終了日付</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function UpdateStaffStall(ByVal stallId As Integer, _
                                         ByVal reserveId As Integer, _
                                         ByVal daySeqNo As Integer, _
                                         ByVal seqNo As Integer, _
                                         ByVal workDate As Date, _
                                         ByVal endTime As String) As Integer

            Logger.Info("[S]UpdateStaffStall()")

            ' DbUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("SC3150101_014")

                Dim sql As New StringBuilder

                ' SQL文の作成
                With sql
                    'etc
                    .Append("UPDATE /* SC3150101_014 */ ")
                    .Append("       tbl_TSTAFFSTALL t1 ")
                    .Append("   SET t1.WORK_END = :WORK_END ")
                    .Append(" WHERE t1.STALLID = :STALLID ")
                    .Append("   AND t1.REZID = :REZID ")
                    .Append("   AND t1.DSEQNO = :DSEQNO ")
                    .Append("   AND t1.SEQNO = :SEQNO ")
                    '.Append("   AND t1.WORKDATE = :WORKDATE ")
                    .Append("   AND t1.WORKDATE = TO_DATE(:WORKDATE, 'YYYY/MM/DD') ")
                    .Append("   AND t1.WORK_END IS NULL ")
                End With

                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("STALLID", OracleDbType.Int64, stallId)
                query.AddParameterWithTypeValue("REZID", OracleDbType.Int64, reserveId)
                query.AddParameterWithTypeValue("DSEQNO", OracleDbType.Int64, daySeqNo)
                query.AddParameterWithTypeValue("SEQNO", OracleDbType.Int64, seqNo)
                'query.AddParameterWithTypeValue("WORKDATE", OracleDbType.Date, CType(workDate.ToString("yyyy/MM/dd"), Date))
                query.AddParameterWithTypeValue("WORKDATE", OracleDbType.Char, workDate.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture()))
                query.AddParameterWithTypeValue("WORK_END", OracleDbType.Char, endTime)

                Logger.Info("[E]UpdateStaffStall()")

                'SQL実行
                Return query.Execute()

            End Using

        End Function


        ''' <summary>
        ''' 休憩時間帯、使用不可時間帯取得
        ''' </summary>
        ''' <param name="stallId">ストールID</param>
        ''' <param name="fromDate">取得対象時刻範囲(FROM)</param>
        ''' <param name="toDate">取得対象時刻範囲(TO)</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetBreakSlot(ByVal stallId As Integer, _
                                     ByVal fromDate As Date, _
                                     ByVal toDate As Date) As SC3150101DataSet.SC3150101StallBreakInfoDataTable

            Logger.Info("[S]GetBreakSlot()")

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3150101DataSet.SC3150101StallBreakInfoDataTable)("SC3150101_015")

                Dim sql As New StringBuilder

                ' SQL文の作成
                With sql
                    .Append("  SELECT /* SC3150101_015 */ ")
                    .Append("         t1.STALLID AS STALLID, ")
                    .Append("         t1.STARTTIME AS STARTTIME, ")
                    .Append("         t1.ENDTIME AS ENDTIME ")
                    .Append("    FROM tbl_STALLBREAK t1 ")
                    .Append("   WHERE t1.STALLID = :STALLID1 ")
                    .Append("     AND t1.BREAKKBN = '1' ")
                    .Append("   UNION ")
                    .Append("  SELECT t2.STALLID AS STALLID, ")
                    .Append("         TO_CHAR(t2.STARTTIME, 'HH24MI') AS STARTTIME, ")
                    .Append("         TO_CHAR(t2.ENDTIME, 'HH24MI') AS ENDTIME ")
                    .Append("    FROM tbl_STALLREZINFO t2 ")
                    .Append("   WHERE t2.STALLID = :STALLID2 ")
                    '.Append("     AND t2.ENDTIME < :ENDTIME1 ")
                    '.Append("     AND t2.ENDTIME > :ENDTIME2 ")
                    .Append("     AND t2.ENDTIME < TO_DATE(:ENDTIME1, 'YYYY/MM/DD HH24:MI:SS') ")
                    .Append("     AND t2.ENDTIME > TO_DATE(:ENDTIME2, 'YYYY/MM/DD HH24:MI:SS') ")
                    .Append("     AND t2.CANCELFLG <> '1' ")
                    .Append("     AND t2.STATUS = '3' ")
                    .Append("ORDER BY 2, 3")
                End With

                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("STALLID1", OracleDbType.Int64, stallId)
                query.AddParameterWithTypeValue("STALLID2", OracleDbType.Int64, stallId)
                'query.AddParameterWithTypeValue("ENDTIME1", OracleDbType.Date, CType(toDate.ToString("yyyy/MM/dd HH:mm:00"), Date))
                'query.AddParameterWithTypeValue("ENDTIME2", OracleDbType.Date, CType(fromDate.ToString("yyyy/MM/dd HH:mm:00"), Date))
                query.AddParameterWithTypeValue("ENDTIME1", OracleDbType.Char, toDate.ToString("yyyy/MM/dd HH:mm:00", CultureInfo.InvariantCulture()))
                query.AddParameterWithTypeValue("ENDTIME2", OracleDbType.Char, fromDate.ToString("yyyy/MM/dd HH:mm:00", CultureInfo.InvariantCulture()))

                Logger.Info("[E]GetBreakSlot()")

                'SQL実行
                Return query.GetData()

            End Using

        End Function


        ''' <summary>
        ''' 販売店環境設定値取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="parameterName">パラメータ名</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetDealerEnvironmentSettingValue(ByVal dealerCode As String, _
                                                         ByVal branchCode As String, _
                                                         ByVal parameterName As String) As SC3150101DataSet.SC3150101DealerEnvironmentSettingInfoDataTable

            Logger.Info("[S]GetDealerEnvironmentSettingValue()")

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3150101DataSet.SC3150101DealerEnvironmentSettingInfoDataTable)("SC3150101_016")

                Dim sql As New StringBuilder

                ' SQL文の作成
                With sql
                    .Append("   SELECT /* SC3150101_016 */ ")
                    .Append("          PARAMVALUE ")
                    .Append("     FROM tbl_DLRENVSETTING ")
                    .Append("    WHERE DLRCD IN (:DLRCD, '00000') ")
                    .Append("      AND STRCD IN (:STRCD, '000') ")
                    .Append("      AND PARAMNAME = :PARAMNAME ")
                    .Append(" ORDER BY DLRCD DESC, STRCD DESC ")
                End With

                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
                query.AddParameterWithTypeValue("PARAMNAME", OracleDbType.Varchar2, parameterName)

                Logger.Info("[E]GetDealerEnvironmentSettingValue()")

                'SQL実行
                Return query.GetData()

            End Using

        End Function


        ''' <summary>
        ''' 処理対象日のUnavailableチップのリストを取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="stallId">ストールID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetUnavailableList(ByVal dealerCode As String, _
                                           ByVal branchCode As String, _
                                           ByVal stallId As Integer, _
                                           ByVal targetDayStart As Date, _
                                           ByVal targetDayEnd As Date) As SC3150101DataSet.SC3150101UnavailableChipListDataTable

            Logger.Info("[S]GetUnavailableList()")

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3150101DataSet.SC3150101UnavailableChipListDataTable)("SC3150101_017")

                Dim sql As New StringBuilder

                ' SQL文の作成
                With sql
                    .Append("SELECT /* SC3150101_017 */ " & vbCrLf)
                    .Append("       TO_CHAR(STARTTIME, 'yyyyMMdd') AS STARTTIME_DAY, ")
                    .Append("       TO_CHAR(STARTTIME, 'HH24MI') AS STARTTIME_TIME, ")
                    .Append("       TO_CHAR(ENDTIME, 'yyyyMMdd') AS ENDTIME_DAY, ")
                    .Append("       TO_CHAR(ENDTIME, 'HH24MI') AS ENDTIME_TIME ")
                    .Append("  FROM tbl_STALLREZINFO ")
                    .Append(" WHERE DLRCD = :DLRCD ")
                    .Append("   AND STRCD = :STRCD ")
                    .Append("   AND STALLID = :STALLID ")
                    .Append("   AND STARTTIME < TO_DATE(:STARTTIME, 'YYYYMMDDHH24MI') ")
                    .Append("   AND ENDTIME > TO_DATE(:ENDTIME, 'YYYYMMDDHH24MI') ")
                    .Append("   AND STATUS = 3 ")
                    .Append("   AND CANCELFLG <> '1'")
                End With

                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
                query.AddParameterWithTypeValue("STALLID", OracleDbType.Int64, stallId)
                query.AddParameterWithTypeValue("STARTTIME", OracleDbType.Char, targetDayEnd.ToString("yyyyMMddHHmm", CultureInfo.InvariantCulture()))
                query.AddParameterWithTypeValue("ENDTIME", OracleDbType.Char, targetDayStart.ToString("yyyyMMddHHmm", CultureInfo.InvariantCulture()))

                Logger.Info("[E]GetUnavailableList()")

                ' 検索結果の返却
                Return query.GetData()

            End Using

        End Function


        ''' <summary>
        ''' 次の非稼動日の取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="stallId">ストールID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetNextNonworkingDate(ByVal dealerCode As String, _
                                              ByVal branchCode As String, _
                                              ByVal stallId As Integer, _
                                              ByVal targetDate As Date) As SC3150101DataSet.SC3150101NextNonworkingDateDataTable

            Logger.Info("[S]GetNextNonworkingDate()")

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3150101DataSet.SC3150101NextNonworkingDateDataTable)("SC3150101_018")

                Dim sql As New StringBuilder

                ' SQL文の作成
                With sql
                    .Append("SELECT /* SC3150101_018 */ ")
                    .Append("       WORKDATE ")
                    .Append("  FROM (SELECT WORKDATE ")
                    .Append("          FROM tbl_STALLPLAN ")
                    .Append("         WHERE DLRCD = :DLRCD ")
                    .Append("           AND STRCD = :STRCD ")
                    .Append("           AND STALLID IN(-1, :STALLID) ")
                    .Append("           AND WORKDATE > :WORKDATE ")
                    .Append("      ORDER BY WORKDATE) ")
                    .Append(" WHERE ROWNUM <= 1")
                End With

                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
                query.AddParameterWithTypeValue("STALLID", OracleDbType.Int64, stallId)
                query.AddParameterWithTypeValue("WORKDATE", OracleDbType.Char, targetDate.ToString("yyyyMMdd", CultureInfo.InvariantCulture()))

                Logger.Info("[E]GetNextNonworkingDate()")

                ' 検索結果の返却
                Return query.GetData()

            End Using

        End Function


        ''' <summary>
        ''' 予約チップ情報の取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="stallId">ストールID</param>
        ''' <param name="dateFrom">稼働時間From</param>
        ''' <param name="dateTo">稼働時間To</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetReserveChipInfo(ByVal dealerCode As String, _
                                           ByVal branchCode As String, _
                                           ByVal stallId As Integer, _
                                           ByVal dateFrom As Date, _
                                           ByVal dateTo As Date) As SC3150101DataSet.SC3150101ReserveChipInfoDataTable

            Logger.Info("[S]GetReserveChipInfo()")

            Dim sql As New StringBuilder

            ' SQL文の作成
            With sql
                ' 2012/02/28 上田 SQLインスペクション対応 Start
                '.Append("with vREZINFO AS (SELECT /* SC3150101_019 */ ")
                '.Append("          T1.STARTTIME, ")
                '.Append("          T1.ENDTIME, ")
                '.Append("          T1.STALLID, ")
                '.Append("          T1.REZID, ")
                '.Append("          T1.INSDID, ")
                '.Append("          T1.STATUS, ")
                '.Append("          T1.CUSTOMERNAME, ")
                '.Append("          T1.REZ_RECEPTION, ")
                '.Append("          T1.CRRYINTIME, ")
                '.Append("          T1.CRRYOUTTIME, ")
                '.Append("          T1.VCLREGNO, ")
                '.Append("          T1.SERVICECODE_S, ")
                '.Append("          T1.STRDATE, ")
                '.Append("          T4.SVCORGNMCT, ")
                '.Append("          T4.SVCORGNMCB, ")
                '.Append("          NVL(T1.UPDATE_COUNT, 0) AS UPDATE_COUNT, ")
                '.Append("          T1.STOPFLG As STOPFLG, ")
                ''.Append("          (CASE T1.STOPFLG ")
                ''.Append("               WHEN '2' THEN '32' ")
                ''.Append("               WHEN '5' THEN '34' ")
                ''.Append("               WHEN '6' THEN '33' ")
                ''.Append("               ELSE '00' ")
                ''.Append("           END) AS STOPFLG, ")
                '.Append("          T2.RESULT_STATUS, ")
                '.Append("          T1.REZ_WORK_TIME, ")
                '.Append("          T3.SERVICECODE, ")
                '.Append("          T1.UPDATEACCOUNT, ")
                '.Append("          T1.VEHICLENAME, ")
                '.Append("          T1.CANCELFLG, ")
                '.Append("          T1.UPDATEDATE, ")
                '.Append("          T1.INPUTACCOUNT, ")
                '.Append("          T1.MERCHANDISECD, ")
                '.Append("          T4.SERVICECODE AS SERVICECODE_2, ")
                '.Append("          T1.WALKIN, ")
                '.Append("          T2.UPDATE_COUNT AS UPDATE_COUNT_2, ")
                '.Append("          T1.STOPFLG AS STOPFLG_2, ")
                '.Append("          NVL(T2.SEQNO, 0) AS SEQNO, ")
                '.Append("          NVL(T2.DSEQNO,0) AS DSEQNO , ")
                '.Append("          NVL(T1.PREZID,'') AS PREZID, ")
                '.Append("          NVL(T1.REZCHILDNO,'') AS REZCHILDNO, ")
                '.Append("          T2.REZ_END_TIME, ")
                '.Append("          T3.DLRCD, ")
                '.Append("          T3.STRCD, ")
                '.Append("          T1.DLRCD AS DLRCD_2, ")
                '.Append("          T1.STRCD AS STRCD_2, ")
                '.Append("          T1.VIN, ")
                '.Append("          T2.REZ_START_TIME, ")
                '.Append("          T1.ACCOUNT_PLAN ") ' SAコード
                ''.Append("          /* USERNAME */ ") ' SA名 tbl_USERSより
                '.Append("        , T1.ORDERNO ") ' R/O No.
                '.Append("     FROM TBL_STALLREZINFO T1 ")
                '.Append("LEFT JOIN (SELECT T5.DLRCD, ")
                '.Append("                  T5.STRCD, ")
                '.Append("                  T5.REZID, ")
                '.Append("                  T5.DSEQNO, ")
                '.Append("                  T5.SEQNO, ")
                '.Append("                  T5.RESULT_STATUS, ")
                '.Append("                  T5.UPDATE_COUNT, ")
                '.Append("                  T5.REZ_END_TIME, ")
                '.Append("                  T5.REZ_START_TIME ")
                '.Append("             FROM TBL_STALLPROCESS T5 ")
                '.Append("            WHERE T5.DLRCD = :DLRCD1 ") '''''販売店コード
                '.Append("              AND T5.STRCD = :STRCD1 ") '''''店舗コード
                '.Append("              AND ( T5.RESULT_START_TIME < :RESULT_START_TIME1 ") '''''稼働時間To
                '.Append("                   AND T5.RESULT_START_TIME >= :RESULT_START_TIME2 ") '''''稼働時間From
                '.Append("                    OR T5.RESULT_STATUS IN ('0', '00', '10', '11')) ")
                '.Append("          ) T2 ")
                '.Append("       ON T2.DLRCD = T1.DLRCD ")
                '.Append("      AND T2.STRCD = T1.STRCD ")
                '.Append("      AND T2.REZID = T1.REZID ")
                '.Append("LEFT JOIN TBL_STALL T3 ")
                '.Append("       ON T3.STALLID = T1.STALLID ")
                '.Append("LEFT JOIN tbl_MERCHANDISEMST T4 ")
                '.Append("       ON T4.MERCHANDISECD = T1.MERCHANDISECD ")
                '.Append("      AND T4.DLRCD = T1.DLRCD ")
                '.Append("    WHERE T1.DLRCD = :DLRCD2 ") '''''販売店コード
                '.Append("      AND T1.STRCD = :STRCD2 ") '''''店舗コード
                '.Append("      AND T1.STALLID = :STALLID1 ") '''''ストールID
                '.Append("      AND T1.STATUS <> 3 ")
                '.Append("      AND ( T2.RESULT_STATUS IN ('0', '00', '10') OR T2.RESULT_STATUS IS NULL ) ")
                '.Append("      AND ( T1.ENDTIME >= TO_DATE( :ENDTIME1 , 'YYYY/MM/DD HH24:MI:SS') ") '''''稼働時間Fromの日付部分＋00:00:00
                '.Append("           AND T1.STARTTIME < TO_DATE( :STARTTIME1 , 'YYYY/MM/DD HH24:MI:SS') ") '''''稼働時間To
                '.Append("           AND T1.STOPFLG NOT IN ('2', '5', '6') ")
                '.Append("           AND T1.CANCELFLG <> '1' ")
                '.Append("           AND ( T1.ENDTIME >= TO_DATE( :ENDTIME2 , 'YYYY/MM/DD HH24:MI:SS') ") '''''稼働時間From
                '.Append("                OR ( T1.STARTTIME = TO_DATE( :STARTTIME2 , 'YYYY/MM/DD HH24:MI:SS') ") '''''稼働時間Fromの日付部分＋00:00:00
                '.Append("                    AND T1.ENDTIME = TO_DATE( :ENDTIME3 , 'YYYY/MM/DD HH24:MI:SS') ") '''''稼働時間Fromの日付部分＋00:00:00
                '.Append("                   ) ")
                '.Append("               ) ")
                '.Append("          ) ")
                '.Append("      AND ( TO_CHAR(T1.STARTTIME, 'YYYYMMDD') = TO_CHAR(T1.ENDTIME, 'YYYYMMDD') ") '''''
                '.Append("           OR (T2.RESULT_STATUS IS NULL OR T2.RESULT_STATUS < 20) ")
                '.Append("           OR NOT EXISTS ( SELECT T6.REZID ")
                '.Append("                             FROM TBL_STALLPROCESS T6 ")
                '.Append("                            WHERE T6.DLRCD = T2.DLRCD ")
                '.Append("                              AND T6.STRCD = T2.STRCD ")
                '.Append("                              AND T6.REZID = T2.REZID ")
                '.Append("                              AND T6.DSEQNO = T2.DSEQNO ")
                '.Append("                              AND T6.RESULT_START_TIME < :RESULT_START_TIME3 ") '''''稼働時間To
                '.Append("                              AND T6.RESULT_START_TIME >= :RESULT_START_TIME4 ) ") '''''稼働時間From
                '.Append("          ) ")
                '.Append(") ")
                '.Append("   SELECT MT.STARTTIME AS STARTTIME, ")
                '.Append("          MT.ENDTIME AS ENDTIME, ")
                ''.Append("          MT.STALLID AS STALLID, ")
                '.Append("          NVL(MT.STALLID, 0) AS STALLID, ")
                ''.Append("          MT.REZID AS REZID, ")
                '.Append("          NVL(MT.REZID, 0) AS REZID, ")
                '.Append("          MT.INSDID AS INSDID, ")
                ''.Append("          MT.STATUS AS STATUS, ")
                '.Append("          NVL(MT.STATUS, 0) AS STATUS, ")
                '.Append("          MT.CUSTOMERNAME AS CUSTOMERNAME, ")
                '.Append("          MT.REZ_RECEPTION AS REZ_RECEPTION, ")
                '.Append("          MT.CRRYINTIME AS CRRYINTIME, ")
                '.Append("          MT.CRRYOUTTIME AS CRRYOUTTIME, ")
                '.Append("          MT.VCLREGNO AS VCLREGNO, ")
                '.Append("          MT.SERVICECODE_S AS SERVICECODE_S, ")
                ''.Append("          MT.STRDATE AS STRDATE, ")
                '.Append("          NVL(MT.STRDATE, TO_DATE(:MINDATE1, 'YYYY/MM/DD HH24:MI:SS')) AS STRDATE, ")
                '.Append("          MT.SVCORGNMCT AS SVCORGNMCT, ")
                '.Append("          MT.SVCORGNMCB AS SVCORGNMCB, ")
                ''.Append("          MT.UPDATE_COUNT AS UPDATE_COUNT, ")
                '.Append("          NVL(MT.UPDATE_COUNT, 0) AS UPDATE_COUNT, ")
                '.Append("          MT.STOPFLG AS STOPFLG, ")
                ''.Append("          MT.RESULT_STATUS AS RESULT_STATUS, ")
                '.Append("          NVL(MT.RESULT_STATUS, '  ') AS RESULT_STATUS, ")
                ''.Append("          MT.REZ_WORK_TIME AS REZ_WORK_TIME, ")
                '.Append("          NVL(MT.REZ_WORK_TIME, 0) AS REZ_WORK_TIME, ")
                '.Append("          MT.SERVICECODE AS SERVICECODE, ")
                '.Append("          MT.UPDATEACCOUNT AS UPDATEACCOUNT, ")
                '.Append("          MT.VEHICLENAME AS VEHICLENAME, ")
                '.Append("          MT.CANCELFLG AS CANCELFLG, ")
                '.Append("          MT.UPDATEDATE AS UPDATEDATE, ")
                '.Append("          MT.INPUTACCOUNT AS INPUTACCOUNT, ")
                '.Append("          MT.MERCHANDISECD AS MERCHANDISECD, ")
                '.Append("          MT.SERVICECODE_2 AS SERVICECODE_2, ")
                '.Append("          MT.WALKIN AS WALKIN, ")
                ''.Append("          MT.UPDATE_COUNT_2 AS UPDATE_COUNT_2, ")
                '.Append("          NVL(MT.UPDATE_COUNT_2, 0) AS UPDATE_COUNT_2, ")
                '.Append("          MT.STOPFLG_2 AS STOPFLG_2, ")
                ''.Append("          MT.SEQNO AS SEQNO, ")
                '.Append("          NVL(MT.SEQNO, 0) AS SEQNO, ")
                ''.Append("          MT.DSEQNO AS DSEQNO, ")
                '.Append("          NVL(MT.DSEQNO, 0) AS DSEQNO, ")
                ''.Append("          MT.PREZID AS PREZID, ")
                ''.Append("          MT.REZCHILDNO AS REZCHILDNO, ")
                ''.Append("          MT.REZ_END_TIME AS REZ_END_TIME, ")
                '.Append("          NVL(MT.PREZID, -1) AS PREZID, ")
                '.Append("          NVL(MT.REZCHILDNO, -1) AS REZCHILDNO, ")
                '.Append("          NVL(MT.REZ_END_TIME, '') AS REZ_END_TIME, ")
                '.Append("          MT.DLRCD AS DLRCD, ")
                '.Append("          MT.STRCD AS STRCD, ")
                ''.Append("          MT.REZ_START_TIME AS REZ_START_TIME, ")
                '.Append("          NVL(MT.REZ_START_TIME, '            ') AS REZ_START_TIME, ")
                '.Append("          MT.ACCOUNT_PLAN AS ACCOUNT_PLAN, ")
                '.Append("          (CASE ")
                '.Append("               WHEN MT.PREZID IS NOT NULL THEN NVL((SELECT T20.RESULT_STATUS ")
                '.Append("                                                      FROM TBL_STALLREZINFO T10 ")
                '.Append("                                                INNER JOIN TBL_STALLPROCESS T20 ")
                '.Append("                                                        ON T10.DLRCD = T20.DLRCD ")
                '.Append("                                                       AND T10.STRCD = T20.STRCD ")
                '.Append("                                                       AND T10.REZID = T20.REZID ")
                '.Append("                                                     WHERE T10.DLRCD = MT.DLRCD_2 ")
                '.Append("                                                       AND T10.STRCD = MT.STRCD_2 ")
                '.Append("                                                       AND T10.PREZID = MT.PREZID ")
                '.Append("                                                       AND T10.REZCHILDNO > 0 ")
                '.Append("                                                       AND T10.REZCHILDNO < 999 ")
                '.Append("                                                       AND NOT T20.RESULT_STATUS IS NULL ")
                '.Append("                                                       AND T20.RESULT_STATUS NOT IN('00','01','10','11','32','33','34') ")
                '.Append("                                                       AND ROWNUM = 1 ),'0') ")
                '.Append("               ELSE '0' ")
                '.Append("           END) AS RELATIONSTATUS, ")
                '.Append("          (CASE ")
                '.Append("               WHEN MT.PREZID IS NOT NULL THEN NVL((SELECT COUNT(1) ")
                '.Append("                                                      FROM TBL_STALLREZINFO T11 ")
                '.Append("                                           LEFT OUTER JOIN TBL_STALLPROCESS T21 ")
                '.Append("                                                        ON T11.DLRCD = T21.DLRCD ")
                '.Append("                                                       AND T11.STRCD = T21.STRCD ")
                '.Append("                                                       AND T11.REZID = T21.REZID ")
                '.Append("                                                     WHERE T11.DLRCD = MT.DLRCD ")
                '.Append("                                                       AND T11.STRCD = MT.STRCD ")
                '.Append("                                                       AND T11.PREZID = MT.PREZID ")
                '.Append("                                                       AND T11.REZCHILDNO > 0 ")
                '.Append("                                                       AND T11.REZCHILDNO < 999 ")
                ''.Append("                                                       AND NOT (T11.CANCELFLG = '1' AND T11.STOPFLG = '0') ")
                '.Append("                                                       AND NOT (T11.CANCELFLG = '1' AND T11.STOPFLG IN ('0', '2', '5', '6')) ")
                '.Append("                                                       AND (T21.RESULT_STATUS IS NULL OR T21.RESULT_STATUS NOT IN ('97','99')) ")
                '.Append("                                                       AND (T21.DSEQNO IS NULL ")
                '.Append("                                                            OR T21.DSEQNO = (SELECT MAX(T22.DSEQNO) ")
                '.Append("                                                                               FROM TBL_STALLPROCESS T22 ")
                '.Append("                                                                              WHERE T22.DLRCD = T21.DLRCD ")
                '.Append("                                                                                AND T22.STRCD = T21.STRCD ")
                '.Append("                                                                                AND T22.REZID = T21.REZID)) ")
                '.Append("                                                                                AND (T21.SEQNO IS NULL ")
                '.Append("                                                                                     OR T21.SEQNO = (SELECT MAX(T23.SEQNO) ")
                '.Append("                                                                                                       FROM TBL_STALLPROCESS T23 ")
                '.Append("                                                                                                      WHERE T23.DLRCD = T21.DLRCD ")
                '.Append("                                                                                                        AND T23.STRCD = T21.STRCD ")
                '.Append("                                                                                                        AND T23.REZID = T21.REZID ")
                '.Append("                                                                                                        AND T23.DSEQNO = T21.DSEQNO) ")
                '.Append("                                                                                    ) ")
                '.Append("                                                   ), 0) ")
                '.Append("               ELSE 0 ")
                '.Append("           END) AS RELATION_UNFINISHED_COUNT ")
                '.Append("         , MT.ORDERNO AS ORDERNO ")
                '.Append("     FROM vREZINFO MT ")
                '.Append("LEFT JOIN TBL_SMBVCLINFO OV ")
                '.Append("       ON OV.DLRCD = :DLRCD3 ") '''''販売店コード
                '.Append("      AND MT.INSDID = OV.ORIGINALID ")
                '.Append("      AND MT.VIN = OV.VIN ")
                '.Append("      AND MT.VCLREGNO = OV.VCLREGNO ")
                '.Append("LEFT JOIN TBL_SMBCUSTOMER NC ")
                '.Append("       ON NC.DLRCD = :DLRCD4 ") '''''販売店コード
                '.Append("      AND MT.INSDID = NC.ORIGINALID ")

                .Append("SELECT /* SC3150101_019 */")
                .Append("       T1.DLRCD AS DLRCD")
                .Append("     , T1.STRCD AS STRCD")
                .Append("     , NVL(T1.STALLID, 0) AS STALLID")
                .Append("     , NVL(T1.REZID, 0) AS REZID")
                .Append("     , T1.ORDERNO AS ORDERNO")
                .Append("     , T1.STARTTIME AS STARTTIME")
                .Append("     , T1.ENDTIME AS ENDTIME")
                .Append("     , T1.INSDID AS INSDID")
                .Append("     , NVL(T1.STATUS, 0) AS STATUS")
                .Append("     , T1.CUSTOMERNAME AS CUSTOMERNAME")
                .Append("     , T1.REZ_RECEPTION AS REZ_RECEPTION")
                .Append("     , T1.CRRYINTIME AS CRRYINTIME")
                .Append("     , T1.CRRYOUTTIME AS CRRYOUTTIME")
                .Append("     , T1.VCLREGNO AS VCLREGNO")
                .Append("     , T1.SERVICECODE_S AS SERVICECODE_S")
                .Append("     , NVL(T1.STRDATE, TO_DATE(:MINDATE1, 'YYYY/MM/DD HH24:MI:SS')) AS STRDATE")
                .Append("     , T4.SVCORGNMCT AS SVCORGNMCT")
                .Append("     , T4.SVCORGNMCB AS SVCORGNMCB")
                .Append("     , NVL(T1.UPDATE_COUNT, 0) AS UPDATE_COUNT")
                .Append("     , T1.STOPFLG AS STOPFLG")
                .Append("     , NVL(T2.RESULT_STATUS, '  ') AS RESULT_STATUS")
                .Append("     , NVL(T1.REZ_WORK_TIME, 0) AS REZ_WORK_TIME")
                .Append("     , T3.SERVICECODE AS SERVICECODE")
                .Append("     , T1.UPDATEACCOUNT AS UPDATEACCOUNT")
                .Append("     , T1.VEHICLENAME AS VEHICLENAME")
                .Append("     , T1.CANCELFLG AS CANCELFLG")
                .Append("     , T1.UPDATEDATE AS UPDATEDATE")
                .Append("     , T1.INPUTACCOUNT AS INPUTACCOUNT")
                .Append("     , T1.MERCHANDISECD AS MERCHANDISECD")
                .Append("     , T4.SERVICECODE AS SERVICECODE_2")
                .Append("     , T1.WALKIN AS WALKIN")
                .Append("     , NVL(T2.UPDATE_COUNT, 0) AS UPDATE_COUNT_2")
                .Append("     , T1.STOPFLG AS STOPFLG_2")
                .Append("     , NVL(T2.SEQNO, 0) AS SEQNO")
                .Append("     , NVL(T2.DSEQNO, 0) AS DSEQNO")
                .Append("     , NVL(T1.PREZID, -1) AS PREZID")
                .Append("     , NVL(T1.REZCHILDNO, -1) AS REZCHILDNO")
                .Append("     , NVL(T2.REZ_END_TIME, '') AS REZ_END_TIME")
                .Append("     , NVL(T2.REZ_START_TIME, '            ') AS REZ_START_TIME")
                .Append("     , T1.ACCOUNT_PLAN AS ACCOUNT_PLAN")
                .Append("  FROM TBL_STALLREZINFO T1")
                .Append("     , ( SELECT T6.DLRCD")
                .Append("              , T6.STRCD")
                .Append("              , T6.REZID")
                .Append("              , T6.DSEQNO")
                .Append("              , T6.SEQNO")
                .Append("              , T6.RESULT_STATUS")
                .Append("              , T6.UPDATE_COUNT")
                .Append("              , T6.REZ_END_TIME")
                .Append("              , T6.REZ_START_TIME")
                .Append("           FROM TBL_STALLPROCESS T6")
                .Append("          WHERE T6.DLRCD = :DLRCD1")
                .Append("            AND T6.STRCD = :STRCD1")
                .Append("            AND (")
                .Append("                     T6.RESULT_START_TIME < :RESULT_START_TIME1")
                .Append("                 AND T6.RESULT_START_TIME >= :RESULT_START_TIME2")
                .Append("                  OR T6.RESULT_STATUS IN ('0', '00', '10', '11')")
                .Append("                )")
                .Append("       ) T2")
                .Append("     , TBL_STALL T3")
                .Append("     , TBL_MERCHANDISEMST T4")
                .Append(" WHERE T1.DLRCD = T2.DLRCD (+)")
                .Append("   AND T1.STRCD = T2.STRCD (+)")
                .Append("   AND T1.REZID = T2.REZID (+)")
                .Append("   AND T1.STALLID  =T3.STALLID (+)")
                .Append("   AND T1.DLRCD = T4.DLRCD (+)")
                .Append("   AND T1.MERCHANDISECD = T4.MERCHANDISECD (+)")
                .Append("   AND T1.DLRCD = :DLRCD2")
                .Append("   AND T1.STRCD = :STRCD2")
                .Append("   AND T1.STALLID = :STALLID1")
                .Append("   AND T1.STATUS <> 3")
                .Append("   AND ( T2.RESULT_STATUS IN ('0', '00', '10') OR T2.RESULT_STATUS IS NULL ) ")
                .Append("   AND (")
                .Append("           T1.ENDTIME >= TO_DATE( :ENDTIME1 , 'YYYY/MM/DD HH24:MI:SS')")
                .Append("       AND T1.STARTTIME < TO_DATE( :STARTTIME1 , 'YYYY/MM/DD HH24:MI:SS')")
                .Append("       AND T1.STOPFLG NOT IN ('2', '5', '6')")
                .Append("       AND T1.CANCELFLG <> '1'")
                .Append("       AND (")
                .Append("                T1.ENDTIME >= TO_DATE( :ENDTIME2 , 'YYYY/MM/DD HH24:MI:SS')")
                .Append("             OR (")
                .Append("                      T1.STARTTIME = TO_DATE( :STARTTIME2 , 'YYYY/MM/DD HH24:MI:SS')")
                .Append("                  AND T1.ENDTIME = TO_DATE( :ENDTIME3 , 'YYYY/MM/DD HH24:MI:SS')")
                .Append("                )")
                .Append("           )")
                .Append("       )")
                .Append("   AND (")
                .Append("            TO_CHAR(T1.STARTTIME, 'YYYYMMDD') = TO_CHAR(T1.ENDTIME, 'YYYYMMDD')")
                .Append("         OR (T2.RESULT_STATUS IS NULL OR T2.RESULT_STATUS < 20)")
                .Append("         OR NOT EXISTS (SELECT T5.REZID")
                .Append("                          FROM TBL_STALLPROCESS T5")
                .Append("                WHERE(T5.DLRCD = T2.DLRCD)")
                .Append("                           AND T5.STRCD = T2.STRCD")
                .Append("                           AND T5.REZID = T2.REZID")
                .Append("                           AND T5.DSEQNO = T2.DSEQNO")
                .Append("                           AND T5.RESULT_START_TIME < :RESULT_START_TIME3")
                .Append("                           AND T5.RESULT_START_TIME >= :RESULT_START_TIME4)")
                .Append("       )")
                ' 2012/02/28 上田 SQLインスペクション対応 End
            End With
            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3150101DataSet.SC3150101ReserveChipInfoDataTable)("SC3150101_019")

                query.CommandText = sql.ToString()

                ' バインド変数定義
                'Dim workTimeFromString As String = dateFrom.ToString("yyyyMMddHHmmss") ' 稼働時間From
                'Dim workTimeToString As String = dateTo.ToString("yyyyMMddHHmmss")     ' 稼働時間To
                Dim workTimeFromString As String = dateFrom.ToString("yyyyMMddHHmm", CultureInfo.InvariantCulture())   ' 稼働時間From
                Dim workTimeToString As String = dateTo.ToString("yyyyMMddHHmm", CultureInfo.InvariantCulture())       ' 稼働時間To
                Dim workTimeFrom As String = dateFrom.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture())  ' 稼働時間From
                Dim workTimeTo As String = dateTo.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture())      ' 稼働時間To
                Dim workTimeZeroFrom As String = SetSearchDate(dateFrom)               ' 稼働時間Fromの日付部分＋00:00:00
                query.AddParameterWithTypeValue("DLRCD1", OracleDbType.Char, dealerCode)                     ' 販売店コード
                query.AddParameterWithTypeValue("STRCD1", OracleDbType.Char, branchCode)                     ' 店舗コード
                query.AddParameterWithTypeValue("RESULT_START_TIME1", OracleDbType.Char, workTimeToString)   ' 稼働時間To
                query.AddParameterWithTypeValue("RESULT_START_TIME2", OracleDbType.Char, workTimeFromString) ' 稼働時間From
                query.AddParameterWithTypeValue("DLRCD2", OracleDbType.Char, dealerCode)                     ' 販売店コード
                query.AddParameterWithTypeValue("STRCD2", OracleDbType.Char, branchCode)                     ' 店舗コード
                query.AddParameterWithTypeValue("STALLID1", OracleDbType.Int64, stallId)                     ' ストールID
                query.AddParameterWithTypeValue("ENDTIME1", OracleDbType.Char, workTimeZeroFrom)             ' 稼働時間Fromの日付部分＋00:00:00
                query.AddParameterWithTypeValue("STARTTIME1", OracleDbType.Char, workTimeTo)                 ' 稼働時間To
                query.AddParameterWithTypeValue("ENDTIME2", OracleDbType.Char, workTimeFrom)                 ' 稼働時間From
                query.AddParameterWithTypeValue("STARTTIME2", OracleDbType.Char, workTimeZeroFrom)           ' 稼働時間Fromの日付部分＋00:00:00
                query.AddParameterWithTypeValue("ENDTIME3", OracleDbType.Char, workTimeZeroFrom)             ' 稼働時間Fromの日付部分＋00:00:00
                query.AddParameterWithTypeValue("RESULT_START_TIME3", OracleDbType.Char, workTimeToString)   ' 稼働時間To
                query.AddParameterWithTypeValue("RESULT_START_TIME4", OracleDbType.Char, workTimeFromString) ' 稼働時間From
                ' 2012/02/28 上田 SQLインスペクション対応 Start 
                'query.AddParameterWithTypeValue("DLRCD3", OracleDbType.Char, dealerCode)                     ' 販売店コード
                'query.AddParameterWithTypeValue("DLRCD4", OracleDbType.Char, dealerCode)                     ' 販売店コード
                ' 2012/02/28 上田 SQLインスペクション対応 End 
                ' 入庫日時の仮デフォルト値として設定
                query.AddParameterWithTypeValue("MINDATE1", OracleDbType.Char, DateTime.MinValue.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture()))

                Logger.Info("[E]GetReserveChipInfo()")

                ' 検索結果の返却
                Return query.GetData()

            End Using

        End Function


        ''' <summary>
        ''' 実績チップ情報の取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="stallId">ストールID</param>
        ''' <param name="dateFrom">稼働時間From</param>
        ''' <param name="dateTo">稼働時間To</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetResultChipInfo(ByVal dealerCode As String, _
                                          ByVal branchCode As String, _
                                          ByVal stallId As Integer, _
                                          ByVal dateFrom As Date, _
                                          ByVal dateTo As Date) As SC3150101DataSet.SC3150101ResultChipInfoDataTable

            Logger.Info("[S]GetResultChipInfo()")

            Dim sql As New StringBuilder


            ' SQL文の作成
            With sql
                ' 2012/02/28 上田 SQLインスペクション対応 Start 
                '.Append("WITH vREZINFO as ( /* SC3150101_020 */ ")
                '.Append("    SELECT T1.DLRCD, ")
                '.Append("           T1.STRCD, ")
                '.Append("           T1.REZID, ")
                '.Append("           T1.SEQNO, ")
                '.Append("           T1.DSEQNO , ")
                '.Append("           NVL(T2.PREZID,'') AS PREZID, ")
                '.Append("           NVL(T2.REZCHILDNO,'') AS REZCHILDNO, ")
                '.Append("           T1.REZ_START_TIME,  ")
                '.Append("           T1.REZ_END_TIME, ")
                '.Append("           T1.RESULT_STALLID, ")
                '.Append("           T2.INSDID, ")
                '.Append("           T2.STATUS, ")
                '.Append("           T2.CUSTOMERNAME, ")
                '.Append("           T1.REZ_RECEPTION, ")
                '.Append("           T1.REZ_PICK_DATE, ")
                '.Append("           T1.REZ_DELI_DATE, ")
                '.Append("           T1.MODELCODE, ")
                '.Append("           T1.VCLREGNO, ")
                '.Append("           T1.SERVICECODE, ")
                '.Append("           T2.STRDATE, ")
                '.Append("           T4.SVCORGNMCT, ")
                '.Append("           T4.SVCORGNMCB, ")
                '.Append("           T1.UPDATE_COUNT, ")
                '.Append("           T1.RESULT_STATUS, ")
                '.Append("           T1.REZ_WORK_TIME, ")
                '.Append("           NVL(T1.RESULT_START_TIME, ' ') AS RESULT_START_TIME, ")
                '.Append("           T1.RESULT_END_TIME, ")
                '.Append("           T1.REZ_PICK_TIME, ")
                '.Append("           T1.REZ_DELI_TIME, ")
                '.Append("           T1.INPUTACCOUNT, ")
                '.Append("           T1.RESULT_IN_TIME, ")
                '.Append("           T1.RESULT_WAIT_END, ")
                '.Append("           T2.VEHICLENAME, ")
                '.Append("           T1.UPDATEDATE, ")
                '.Append("           T2.CANCELFLG, ")
                '.Append("           T2.STOPFLG, ")
                '.Append("           T2.STARTTIME, ")
                '.Append("           T2.ENDTIME, ")
                '.Append("           T2.REZ_WORK_TIME AS REZ_WORK_TIME_2, ")
                '.Append("           T2.UPDATEACCOUNT, ")
                '.Append("           T1.RESULT_WORK_TIME, ")
                '.Append("           T1.VIN, ")
                '.Append("           T4.SERVICECODE AS SERVICECODE_MST, ")
                '.Append("           T2.WALKIN, ")
                '.Append("           T2.ACCOUNT_PLAN ")
                '.Append("         , T2.ORDERNO ")
                '.Append("      FROM tbl_STALLPROCESS T1 ")
                '.Append("INNER JOIN (SELECT TT.DLRCD, ")
                '.Append("                   TT.STRCD, ")
                '.Append("                   TT.REZID, ")
                '.Append("                   TT.INSDID, ")
                '.Append("                   TT.STATUS, ")
                '.Append("                   TT.CUSTOMERNAME, ")
                '.Append("                   TT.STRDATE, ")
                '.Append("                   TT.VEHICLENAME, ")
                '.Append("                   TT.CANCELFLG, ")
                '.Append("                   TT.STOPFLG, ")
                '.Append("                   TT.STARTTIME, ")
                '.Append("                   TT.ENDTIME, ")
                '.Append("                   TT.REZ_WORK_TIME, ")
                '.Append("                   TT.UPDATEACCOUNT, ")
                '.Append("                   TT.PREZID, ")
                '.Append("                   TT.REZCHILDNO, ")
                '.Append("                   TT.VIN, ")
                '.Append("                   TT.WALKIN, ")
                '.Append("                   TT.ACCOUNT_PLAN ")
                '.Append("                 , TT.ORDERNO ")
                '.Append("              FROM TBL_STALLREZINFO TT ")
                '.Append("             WHERE TT.DLRCD = :DLRCD1 ") '''''販売店コード
                '.Append("               AND TT.STRCD = :STRCD1 ") '''''店舗コード
                '.Append("           ) T2 ")
                '.Append("        ON T2.DLRCD = T1.DLRCD ")
                '.Append("       AND T2.STRCD = T1.STRCD ")
                '.Append("       AND T2.REZID = T1.REZID ")
                '.Append(" LEFT JOIN TBL_MERCHANDISEMST T4 ")
                '.Append("        ON T4.DLRCD = T1.DLRCD ")
                '.Append("       AND T4.MERCHANDISECD = T1.MERCHANDISECD ")
                '.Append("     WHERE T1.DLRCD = :DLRCD2 ") '''''販売店コード
                '.Append("       AND T1.STRCD = :STRCD2 ") '''''店舗コード
                '.Append("       AND T1.RESULT_STALLID = :RESULT_STALLID1 ") '''''ストールID
                '.Append("       AND T1.RESULT_STATUS NOT IN ('0', '00', '10', '32', '33') ")
                '.Append("       AND ( ( T2.STATUS <> '0' ")
                '.Append("              AND T1.RESULT_START_TIME >= :RESULT_START_TIME1 ") '''''稼働時間From
                '.Append("              AND T1.RESULT_START_TIME < :RESULT_START_TIME2 ") '''''稼働時間To
                '.Append("             ) ")
                '.Append("            OR ( T1.RESULT_START_TIME < :RESULT_START_TIME3 ") '''''稼働時間To
                '.Append("                AND T1.RESULT_STATUS IN ('30', '31', '38', '39' , '42', '43', '44') ")
                '.Append("                AND T1.SEQNO = ( SELECT MAX(T6.SEQNO) ")
                '.Append("                                   FROM TBL_STALLPROCESS T6 ")
                '.Append("                                  WHERE T6.DLRCD = T1.DLRCD ")
                '.Append("                                    AND T6.STRCD = T1.STRCD ")
                '.Append("                                    AND T6.REZID = T1.REZID ")
                '.Append("                                    AND T6.DSEQNO = T1.DSEQNO ")
                '.Append("                               ) ")
                '.Append("               ) ")
                '.Append("           ) ")
                '.Append("       AND ( T2.CANCELFLG <> '1' ")
                '.Append("            OR T2.STOPFLG IN ('1', '2', '5', '6') ")
                '.Append("           ) ")
                '.Append("        OR (T1.RESULT_STATUS = '11' ")
                '.Append("            AND T1.REZ_START_TIME = :REZ_START_TIME1 ") '''''稼働時間Fromの日付部分＋0000
                '.Append("            AND ( T2.CANCELFLG <> '1' ")
                '.Append("                 OR T2.STOPFLG IN ('1', '2', '5', '6') ")
                '.Append("                ) ")
                '.Append("           ) ")
                '.Append(") ")
                '.Append("   SELECT MT.REZID AS REZID, ")
                '.Append("          MT.SEQNO AS SEQNO, ")
                '.Append("          MT.DSEQNO AS DSEQNO, ")
                ''.Append("          MT.PREZID AS PREZID, ")
                '.Append("          NVL(MT.PREZID, -1) AS PREZID, ")
                ''.Append("          MT.REZCHILDNO AS REZCHILDNO, ")
                '.Append("          NVL(MT.REZCHILDNO, -1) AS REZCHILDNO, ")
                '.Append("          MT.REZ_START_TIME AS REZ_START_TIME, ")
                '.Append("          MT.REZ_END_TIME AS REZ_END_TIME, ")
                '.Append("          MT.RESULT_STALLID AS RESULT_STALLID, ")
                '.Append("          MT.INSDID AS INSDID, ")
                '.Append("          MT.STATUS AS STATUS, ")
                '.Append("          MT.CUSTOMERNAME AS CUSTOMERNAME, ")
                '.Append("          MT.REZ_RECEPTION AS REZ_RECEPTION, ")
                '.Append("          MT.REZ_PICK_DATE AS REZ_PICK_DATE, ")
                '.Append("          MT.REZ_DELI_DATE AS REZ_DELI_DATE, ")
                '.Append("          MT.MODELCODE AS MODELCODE, ")
                '.Append("          MT.VCLREGNO AS VCLREGNO, ")
                '.Append("          MT.SERVICECODE AS SERVICECODE, ")
                ''.Append("          MT.STRDATE AS STRDATE, ")
                '.Append("          NVL(MT.STRDATE, TO_DATE(:MINDATE1, 'YYYY/MM/DD HH24:MI:SS')) AS STRDATE, ")
                '.Append("          MT.SVCORGNMCT AS SVCORGNMCT, ")
                '.Append("          MT.SVCORGNMCB AS SVCORGNMCB, ")
                '.Append("          MT.UPDATE_COUNT AS UPDATE_COUNT, ")
                '.Append("          MT.RESULT_STATUS AS RESULT_STATUS, ")
                ''.Append("          MT.REZ_WORK_TIME AS REZ_WORK_TIME, ")
                '.Append("          NVL(MT.REZ_WORK_TIME, 0) AS REZ_WORK_TIME, ")
                '.Append("          MT.RESULT_START_TIME AS RESULT_START_TIME, ")
                '.Append("          MT.RESULT_END_TIME AS RESULT_END_TIME, ")
                '.Append("          MT.REZ_PICK_TIME AS REZ_PICK_TIME, ")
                '.Append("          MT.REZ_DELI_TIME AS REZ_DELI_TIME, ")
                '.Append("          MT.INPUTACCOUNT AS INPUTACCOUNT, ")
                '.Append("          MT.RESULT_IN_TIME AS RESULT_IN_TIME, ")
                '.Append("          MT.RESULT_WAIT_END AS RESULT_WAIT_END, ")
                '.Append("          MT.VEHICLENAME AS VEHICLENAME, ")
                '.Append("          MT.UPDATEDATE AS UPDATEDATE, ")
                '.Append("          MT.CANCELFLG AS CANCELFLG, ")
                '.Append("          MT.STOPFLG AS STOPFLG, ")
                '.Append("          MT.STARTTIME AS STARTTIME, ")
                '.Append("          MT.ENDTIME AS ENDTIME, ")
                ''.Append("          MT.REZ_WORK_TIME_2 AS REZ_WORK_TIME_2, ")
                '.Append("          NVL(MT.REZ_WORK_TIME_2, 0) AS REZ_WORK_TIME_2, ")
                '.Append("          MT.UPDATEACCOUNT AS UPDATEACCOUNT, ")
                ''.Append("          MT.RESULT_WORK_TIME AS RESULT_WORK_TIME, ")
                '.Append("          NVL(MT.RESULT_WORK_TIME, 0) AS RESULT_WORK_TIME, ")
                '.Append("          MT.SERVICECODE_MST AS SERVICECODE_MST, ")
                '.Append("          MT.WALKIN AS WALKIN, ")
                '.Append("          MT.ACCOUNT_PLAN AS ACCOUNT_PLAN, ")
                '.Append("          (CASE ")
                '.Append("               WHEN MT.PREZID IS NOT NULL THEN NVL((SELECT T20.RESULT_STATUS ")
                '.Append("                                                      FROM TBL_STALLREZINFO T10 ")
                '.Append("                                                INNER JOIN TBL_STALLPROCESS T20 ")
                '.Append("                                                        ON T10.DLRCD = T20.DLRCD ")
                '.Append("                                                       AND T10.STRCD = T20.STRCD ")
                '.Append("                                                       AND T10.REZID = T20.REZID ")
                '.Append("                                                     WHERE T10.DLRCD = MT.DLRCD ")
                '.Append("                                                       AND T10.STRCD = MT.STRCD ")
                '.Append("                                                       AND T10.PREZID = MT.PREZID ")
                '.Append("                                                       AND T10.REZCHILDNO > 0 ")
                '.Append("                                                       AND T10.REZCHILDNO < 999 ")
                '.Append("                                                       AND NOT T20.RESULT_STATUS IS NULL ")
                '.Append("                                                       AND T20.RESULT_STATUS NOT IN('00','01','10','11','32','33','34') ")
                '.Append("                                                       AND ROWNUM = 1 ")
                '.Append("                                                   ),'0') ")
                '.Append("               ELSE '0' ")
                '.Append("           END) AS RELATIONSTATUS, ")
                '.Append("          (CASE ")
                '.Append("               WHEN MT.PREZID IS NOT NULL THEN NVL((SELECT COUNT(1) ")
                '.Append("                                                      FROM TBL_STALLREZINFO T11 ")
                '.Append("                                           LEFT OUTER JOIN TBL_STALLPROCESS T21 ")
                '.Append("                                                        ON T11.DLRCD = T21.DLRCD ")
                '.Append("                                                       AND T11.STRCD = T21.STRCD ")
                '.Append("                                                       AND T11.REZID = T21.REZID ")
                '.Append("                                                     WHERE T11.DLRCD = MT.DLRCD ")
                '.Append("                                                       AND T11.STRCD = MT.STRCD ")
                '.Append("                                                       AND T11.PREZID = MT.PREZID ")
                '.Append("                                                       AND T11.REZCHILDNO > 0 ")
                '.Append("                                                       AND T11.REZCHILDNO < 999 ")
                '.Append("                                                       AND NOT (T11.CANCELFLG = '1' AND T11.STOPFLG = '0') ")
                '.Append("                                                       AND (T21.RESULT_STATUS IS NULL ")
                '.Append("                                                            OR T21.RESULT_STATUS NOT IN ('97','99') ")
                '.Append("                                                           ) ")
                '.Append("                                                       AND (T21.DSEQNO IS NULL ")
                '.Append("                                                            OR T21.DSEQNO = (SELECT MAX(T22.DSEQNO) ")
                '.Append("                                                                               FROM TBL_STALLPROCESS T22 ")
                '.Append("                                                                              WHERE T22.DLRCD = T21.DLRCD ")
                '.Append("                                                                                AND T22.STRCD = T21.STRCD ")
                '.Append("                                                                                AND T22.REZID = T21.REZID ")
                '.Append("                                                                            ) ")
                '.Append("                                                           ) ")
                '.Append("                                                       AND (T21.SEQNO IS NULL ")
                '.Append("                                                            OR T21.SEQNO = (SELECT MAX(T23.SEQNO) ")
                '.Append("                                                                              FROM TBL_STALLPROCESS T23 ")
                '.Append("                                                                             WHERE T23.DLRCD = T21.DLRCD ")
                '.Append("                                                                               AND T23.STRCD = T21.STRCD ")
                '.Append("                                                                               AND T23.REZID = T21.REZID ")
                '.Append("                                                                               AND T23.DSEQNO = T21.DSEQNO ")
                '.Append("                                                                           )")
                '.Append("                                                           ) ")
                '.Append("                                                   ), 0) ")
                '.Append("               ELSE 0 ")
                '.Append("           END) AS RELATION_UNFINISHED_COUNT ")
                '.Append("        , MT.ORDERNO AS ORDERNO ")
                '.Append("     FROM vREZINFO MT ")
                '.Append("LEFT JOIN TBL_SMBVCLINFO OV ")
                '.Append("       ON OV.DLRCD = :DLRCD5 ") '''''販売店コード
                '.Append("      AND MT.INSDID = OV.ORIGINALID ")
                '.Append("      AND MT.VIN = OV.VIN ")
                '.Append("      AND MT.VCLREGNO = OV.VCLREGNO ")
                '.Append("LEFT JOIN TBL_SMBCUSTOMER NC ")
                '.Append("       ON NC.DLRCD = :DLRCD6 ") '''''販売店コード
                '.Append("      AND MT.INSDID = NC.ORIGINALID ")

                .Append("SELECT /* SC3150101_020 */")
                .Append("       T1.REZID AS REZID")
                .Append("     , NVL(T1.SEQNO, 0) AS SEQNO")
                .Append("     , NVL(T1.DSEQNO, 0) AS DSEQNO")
                .Append("     , NVL(T2.PREZID, -1) AS PREZID")
                .Append("     , NVL(T2.REZCHILDNO, -1) AS REZCHILDNO")
                .Append("     , T1.REZ_START_TIME AS REZ_START_TIME")
                .Append("     , T1.REZ_END_TIME AS REZ_END_TIME")
                .Append("     , T1.RESULT_STALLID AS RESULT_STALLID")
                .Append("     , T2.INSDID AS INSDID")
                .Append("     , T2.STATUS AS STATUS")
                .Append("     , T2.CUSTOMERNAME AS CUSTOMERNAME")
                .Append("     , T1.REZ_RECEPTION AS REZ_RECEPTION")
                .Append("     , T1.REZ_PICK_DATE AS REZ_PICK_DATE")
                .Append("     , T1.REZ_DELI_DATE AS REZ_DELI_DATE")
                .Append("     , T1.MODELCODE AS MODELCODE")
                .Append("     , T1.VCLREGNO AS VCLREGNO")
                .Append("     , T1.SERVICECODE AS SERVICECODE")
                .Append("     , NVL(T2.STRDATE, TO_DATE(:MINDATE1, 'YYYY/MM/DD HH24:MI:SS')) AS STRDATE")
                .Append("     , T3.SVCORGNMCT AS SVCORGNMCT")
                .Append("     , T3.SVCORGNMCB AS SVCORGNMCB")
                .Append("     , T1.UPDATE_COUNT AS UPDATE_COUNT")
                .Append("     , T1.RESULT_STATUS AS RESULT_STATUS")
                .Append("     , NVL(T1.REZ_WORK_TIME, 0) AS REZ_WORK_TIME")
                .Append("     , T1.RESULT_START_TIME AS RESULT_START_TIME")
                .Append("     , T1.RESULT_END_TIME AS RESULT_END_TIME")
                .Append("     , T1.REZ_PICK_TIME AS REZ_PICK_TIME")
                .Append("     , T1.REZ_DELI_TIME AS REZ_DELI_TIME")
                .Append("     , T1.INPUTACCOUNT AS INPUTACCOUNT")
                .Append("     , T1.RESULT_IN_TIME AS RESULT_IN_TIME")
                .Append("     , T1.RESULT_WAIT_END AS RESULT_WAIT_END")
                .Append("     , T2.VEHICLENAME AS VEHICLENAME")
                .Append("     , T1.UPDATEDATE AS UPDATEDATE")
                .Append("     , T2.CANCELFLG AS CANCELFLG")
                .Append("     , T2.STOPFLG AS STOPFLG")
                .Append("     , T2.STARTTIME AS STARTTIME")
                .Append("     , T2.ENDTIME AS ENDTIME")
                .Append("     , NVL(T2.REZ_WORK_TIME, 0) AS REZ_WORK_TIME_2")
                .Append("     , T2.UPDATEACCOUNT AS UPDATEACCOUNT")
                .Append("     , NVL(T1.RESULT_WORK_TIME, 0) AS RESULT_WORK_TIME")
                .Append("     , T3.SERVICECODE AS SERVICECODE_MST")
                .Append("     , T2.WALKIN AS WALKIN")
                .Append("     , T2.ACCOUNT_PLAN AS ACCOUNT_PLAN")
                .Append("     , T2.ORDERNO AS ORDERNO")
                .Append("  FROM TBL_STALLPROCESS T1")
                .Append("     , TBL_STALLREZINFO T2")
                .Append("     , TBL_MERCHANDISEMST T3")
                .Append(" WHERE T1.DLRCD = T2.DLRCD")
                .Append("   AND T1.STRCD = T2.STRCD")
                .Append("   AND T1.REZID = T2.REZID")
                .Append("   AND T1.DLRCD = T3.DLRCD (+)")
                .Append("   AND T1.MERCHANDISECD = T3.MERCHANDISECD (+)")
                .Append("   AND T1.DLRCD = :DLRCD1")
                .Append("   AND T1.STRCD = :STRCD1")
                .Append("   AND T1.RESULT_STALLID = :RESULT_STALLID1")
                .Append("   AND T1.RESULT_STATUS NOT IN ('0', '00', '10', '32', '33')")
                .Append("   AND (")
                .Append("         (")
                .Append("             T2.STATUS <> '0'")
                .Append("         AND T1.RESULT_START_TIME >= :RESULT_START_TIME1")
                .Append("         AND T1.RESULT_START_TIME < :RESULT_START_TIME2")
                .Append("         )")
                .Append("         OR")
                .Append("         (")
                .Append("             T1.RESULT_START_TIME < :RESULT_START_TIME3")
                .Append("         AND T1.RESULT_STATUS IN ('30', '31', '38', '39' , '42', '43', '44')")
                .Append("         AND T1.SEQNO = ( SELECT MAX(T4.SEQNO)")
                .Append("                            FROM TBL_STALLPROCESS T4")
                .Append("                WHERE(T4.DLRCD = T1.DLRCD)")
                .Append("                             AND T4.STRCD = T1.STRCD")
                .Append("                             AND T4.REZID = T1.REZID")
                .Append("                             AND T4.DSEQNO = T1.DSEQNO")
                .Append("                        )")
                .Append("         )")
                .Append("       )")
                .Append("   AND ")
                .Append("       (")
                .Append("        (")
                .Append("            T2.CANCELFLG <> '1'")
                .Append("         OR T2.STOPFLG IN ('1', '2', '5', '6')")
                .Append("        )")
                .Append("       OR ")
                .Append("        (")
                .Append("            T1.RESULT_STATUS = '11'")
                .Append("        AND T1.REZ_START_TIME = :REZ_START_TIME1")
                .Append("        AND (")
                .Append("                 T2.CANCELFLG <> '1'")
                .Append("              OR T2.STOPFLG IN ('1', '2', '5', '6')")
                .Append("            )")
                .Append("        )")
                .Append("       )")
                ' 2012/02/28 上田 SQLインスペクション対応 End
            End With

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3150101DataSet.SC3150101ResultChipInfoDataTable)("SC3150101_020")

                query.CommandText = Sql.ToString()

                ' バインド変数定義
                'Dim workTimeFrom As String = dateFrom.ToString("yyyyMMddHHmmss")                        ' 稼働時間From
                'Dim workTimeTo As String = dateTo.ToString("yyyyMMddHHmmss")                            ' 稼働時間To
                Dim workTimeFrom As String = dateFrom.ToString("yyyyMMddHHmm", CultureInfo.InvariantCulture())                          ' 稼働時間From
                Dim workTimeTo As String = dateTo.ToString("yyyyMMddHHmm", CultureInfo.InvariantCulture())                              ' 稼働時間To
                Dim workTimeZeroFrom As String = dateFrom.Date.ToString("yyyyMMdd", CultureInfo.InvariantCulture()) & "0000"            ' 稼働時間Fromの日付部分＋00:00:00
                query.AddParameterWithTypeValue("DLRCD1", OracleDbType.Char, dealerCode)                ' 販売店コード
                query.AddParameterWithTypeValue("STRCD1", OracleDbType.Char, branchCode)                ' 店舗コード
                ' 2012/02/28 上田 SQLインスペクション対応 Start
                'query.AddParameterWithTypeValue("DLRCD2", OracleDbType.Char, dealerCode)                ' 販売店コード
                'query.AddParameterWithTypeValue("STRCD2", OracleDbType.Char, branchCode)                ' 店舗コード
                ' 2012/02/28 上田 SQLインスペクション対応 End
                query.AddParameterWithTypeValue("RESULT_STALLID1", OracleDbType.Int64, stallId)         ' ストールID
                query.AddParameterWithTypeValue("RESULT_START_TIME1", OracleDbType.Char, workTimeFrom)  ' 稼働時間From
                query.AddParameterWithTypeValue("RESULT_START_TIME2", OracleDbType.Char, workTimeTo)    ' 稼働時間To
                query.AddParameterWithTypeValue("RESULT_START_TIME3", OracleDbType.Char, workTimeTo)    ' 稼働時間To
                query.AddParameterWithTypeValue("REZ_START_TIME1", OracleDbType.Char, workTimeZeroFrom) ' 稼働時間Fromの日付部分＋0000
                ' 2012/02/28 上田 SQLインスペクション対応 Start
                'query.AddParameterWithTypeValue("DLRCD5", OracleDbType.Char, dealerCode)                ' 販売店コード
                'query.AddParameterWithTypeValue("DLRCD6", OracleDbType.Char, dealerCode)                ' 販売店コード
                ' 2012/02/28 上田 SQLインスペクション対応 End
                ' 入庫日時の仮デフォルト値として設定
                query.AddParameterWithTypeValue("MINDATE1", OracleDbType.Char, DateTime.MinValue.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture()))

                Logger.Info("[E]GetResultChipInfo()")

                ' 検索結果の返却
                Return query.GetData()

            End Using

        End Function


        ''' <summary>
        ''' 使用不可チップ情報の取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="stallId">ストールID</param>
        ''' <param name="fromDate">稼働時間From</param>
        ''' <param name="toDate">稼働時間To</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetUnavailableChipInfo(ByVal dealerCode As String, _
                                               ByVal branchCode As String, _
                                               ByVal stallId As Integer, _
                                               ByVal fromDate As Date, _
                                               ByVal toDate As Date) As SC3150101DataSet.SC3150101UnavailableChipInfoDataTable

            Logger.Info("[S]GetUnavailableChipInfo()")

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3150101DataSet.SC3150101UnavailableChipInfoDataTable)("SC3150101_021")

                Dim sql As New StringBuilder

                ' SQL文の作成
                With sql
                    .Append("  SELECT /* SC3150101_021 */")
                    .Append("         STARTTIME, ")
                    .Append("         ENDTIME ")
                    .Append("    FROM TBL_STALLREZINFO")
                    .Append("   WHERE DLRCD = :DLRCD ")
                    .Append("     AND STRCD = :STRCD ")
                    .Append("     AND STALLID = :STALLID ")
                    .Append("     AND STATUS = 3 ")
                    .Append("     AND CANCELFLG = '0' ")
                    .Append("     AND STOPFLG = '0' ")
                    .Append("     AND ENDTIME >= TO_DATE(:ENDTIME, 'YYYY/MM/DD HH24:MI:SS') ")
                    .Append("     AND STARTTIME < TO_DATE(:STARTTIME, 'YYYY/MM/DD HH24:MI:SS') ")
                    .Append("ORDER BY STARTTIME")
                End With

                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
                query.AddParameterWithTypeValue("STALLID", OracleDbType.Int64, stallId)
                query.AddParameterWithTypeValue("ENDTIME", OracleDbType.Char, SetSearchDate(fromDate))                  ' 稼働時間Fromの日付部分＋00:00:00
                query.AddParameterWithTypeValue("STARTTIME", OracleDbType.Char, toDate.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture())) ' 稼働時間To

                Logger.Info("[E]GetUnavailableChipInfo()")

                ' 検索結果の返却
                Return query.GetData()

            End Using

        End Function


        ''' <summary>
        ''' 休憩情報の取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="stallId">ストールID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetBreakChipInfo(ByVal dealerCode As String, _
                                         ByVal branchCode As String, _
                                         ByVal stallId As Integer) As SC3150101DataSet.SC3150101BreakChipInfoDataTable

            Logger.Info("[S]GetBreakChipInfo()")

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3150101DataSet.SC3150101BreakChipInfoDataTable)("SC3150101_022")

                Dim sql As New StringBuilder

                ' SQL文の作成
                With sql
                    .Append("  SELECT /* SC3150101_022 */ ")
                    .Append("         STARTTIME, ")
                    .Append("         ENDTIME ")
                    .Append("    FROM TBL_STALLBREAK ")
                    .Append("   WHERE DLRCD = :DLRCD ")
                    .Append("     AND STRCD = :STRCD ")
                    .Append("     AND STALLID = :STALLID ")
                    .Append("     AND BREAKKBN = '1' ")
                    .Append("ORDER BY STARTTIME")
                End With

                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
                query.AddParameterWithTypeValue("STALLID", OracleDbType.Int64, stallId)

                Logger.Info("[E]GetBreakChipInfo()")

                ' 検索結果の返却
                Return query.GetData()

            End Using

        End Function


        ''' <summary>
        ''' ログインアカウントが所属するストール情報の取得
        ''' </summary>
        ''' <param name="account">ログインアカウント</param>
        ''' <param name="workDate">作業日付(yyyyMMdd)</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetBelongStallInfo(ByVal account As String, _
                                           ByVal workDate As String) As SC3150101DataSet.SC3150101BelongStallInfoDataTable

            Logger.Info("[S]GetBelongStallInfo()")

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3150101DataSet.SC3150101BelongStallInfoDataTable)("SC3150101_023")

                Dim sql As New StringBuilder

                ' SQL文の作成
                With sql
                    .Append("SELECT /* SC3150101_023 */ ")
                    .Append("       T3.STALLID AS STALLID, ")
                    .Append("       T3.STALLNAME AS STALLNAME, ")
                    .Append("       T3.STALLNAME_S AS STALLNAME_S, ")
                    .Append("       T4.PSTARTTIME AS PSTARTTIME, ")
                    .Append("       T4.PENDTIME AS PENDTIME ")
                    .Append("  FROM TBL_SSTAFF      T1, ")
                    .Append("       TBL_WSTAFFSTALL T2, ")
                    .Append("       TBL_STALL       T3, ")
                    .Append("       TBL_STALLTIME   T4 ")
                    .Append(" WHERE T1.ACCOUNT  = :ACCOUNT ")
                    .Append("   AND T2.DLRCD    = T1.DLRCD ")
                    .Append("   AND T2.STRCD    = T1.STRCD ")
                    .Append("   AND T2.STAFFCD  = T1.STAFFCD ")
                    .Append("   AND T2.WORKDATE = :WORKDATE ")
                    .Append("   AND T3.STALLID  = T2.STALLID ")
                    .Append("   AND T4.DLRCD    = T2.DLRCD ")
                    .Append("   AND T4.STRCD    = T2.STRCD")
                End With

                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.Varchar2, account)
                query.AddParameterWithTypeValue("WORKDATE", OracleDbType.Char, workDate)

                Logger.Info("[E]GetBelongStallInfo()")

                ' 検索結果の返却
                Return query.GetData()

            End Using

        End Function


        ''' <summary>
        ''' 指定日の指定ストールに所属するテクニシャン名の取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="workDate">作業日付(yyyyMMdd)</param>
        ''' <param name="stallId">ストールID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetBelongStallStaff(ByVal dealerCode As String, _
                                            ByVal branchCode As String, _
                                            ByVal workDate As String, _
                                            ByVal stallId As Integer) As SC3150101DataSet.SC3150101BelongStallStaffDataTable

            Logger.Info("[S]GetBelongStallStaff()")

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3150101DataSet.SC3150101BelongStallStaffDataTable)("SC3150101_024")

                Dim sql As New StringBuilder

                ' SQL文の作成
                With sql
                    .Append("SELECT /* SC3150101_024 */ ")
                    .Append("       T3.USERNAME AS USERNAME ")
                    .Append("  FROM TBL_WSTAFFSTALL T1,")
                    .Append("       TBL_SSTAFF      T2,")
                    .Append("       TBL_USERS       T3 ")
                    .Append(" WHERE T1.DLRCD    = :DLRCD ")
                    .Append("   AND T1.STRCD    = :STRCD ")
                    .Append("   AND T1.WORKDATE = :WORKDATE ")
                    .Append("   AND T1.STALLID  = :STALLID ")
                    .Append("   AND T2.DLRCD    = T1.DLRCD ")
                    .Append("   AND T2.STRCD    = T1.STRCD ")
                    .Append("   AND T2.STAFFCD  = T1.STAFFCD ")
                    .Append("   AND T3.ACCOUNT  = T2.ACCOUNT")
                End With

                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
                query.AddParameterWithTypeValue("WORKDATE", OracleDbType.Char, workDate)
                query.AddParameterWithTypeValue("STALLID", OracleDbType.Int64, stallId)

                Logger.Info("[E]GetBelongStallStaff()")

                ' 検索結果の返却
                Return query.GetData()

            End Using

        End Function


        ' 2012/02/27 KN 佐藤 【SERVICE_1】スタッフストール割当の抽出条件を追加（処理修正） START
        ''' <summary>
        ''' ストールの作業担当者数の取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="processDate">対象日</param>
        ''' <param name="stallId">ストールID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetStaffCount(ByVal dealerCode As String, _
                                      ByVal branchCode As String, _
                                      ByVal processDate As Date, _
                                      ByVal stallId As Integer) As SC3150101DataSet.SC3150101StallStaffCountDataTable
            'Public Function GetStaffCount(ByVal processDate As Date, _
            '                              ByVal stallId As Integer) As SC3150101DataSet.SC3150101StallStaffCountDataTable
            ' 2012/02/27 KN 佐藤 【SERVICE_1】スタッフストール割当の抽出条件を追加（処理修正） END

            Logger.Info("[S]GetStaffCount()")

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3150101DataSet.SC3150101StallStaffCountDataTable)("SC3150101_025")

                Dim sql As New StringBuilder

                ' SQL文の作成
                With sql
                    .Append("SELECT /* SC3150101_025 */ ")
                    .Append("       COUNT(1) AS COUNT ")
                    .Append("  FROM tbl_WSTAFFSTALL ")
                    ' 2012/02/27 KN 佐藤 【SERVICE_1】スタッフストール割当の抽出条件を追加（処理修正） START
                    '.Append(" WHERE STALLID  = :STALLID ")
                    '.Append("   AND WORKDATE = :WORKDATE")
                    .Append(" WHERE DLRCD = :DLRCD ")
                    .Append("   AND STRCD = :STRCD")
                    .Append("   AND WORKDATE = :WORKDATE")
                    .Append("   AND STALLID = :STALLID")
                    ' 2012/02/27 KN 佐藤 【SERVICE_1】スタッフストール割当の抽出条件を追加（処理修正） END
                End With

                query.CommandText = sql.ToString()

                ' バインド変数定義
                ' 2012/02/27 KN 佐藤 【SERVICE_1】スタッフストール割当の抽出条件を追加（処理修正） START
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
                ' 2012/02/27 KN 佐藤 【SERVICE_1】スタッフストール割当の抽出条件を追加（処理修正） END
                query.AddParameterWithTypeValue("WORKDATE", OracleDbType.Char, processDate.ToString("yyyyMMdd", CultureInfo.InvariantCulture()))
                query.AddParameterWithTypeValue("STALLID", OracleDbType.Int64, stallId)

                Logger.Info("[E]GetStaffCount()")

                ' 検索結果の返却
                Return query.GetData()

            End Using

        End Function


        ''' <summary>
        ''' 作業中の数の取得
        ''' </summary>
        ''' <param name="stallId">ストールID</param>
        ''' <param name="startTime">稼動開始時間</param>
        ''' <param name="endTime">稼動終了時間</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetWorkingStateCount(ByVal stallId As Integer, _
                                             ByVal startTime As Date, _
                                             ByVal endTime As Date) As SC3150101DataSet.SC3150101WorkingStateCountDataTable

            Logger.Info("[S]GetWorkingStateCount()")

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3150101DataSet.SC3150101WorkingStateCountDataTable)("SC3150101_026")

                Dim sql As New StringBuilder

                ' SQL文の作成
                With sql
                    .Append("    SELECT /* SC3150101_026 */ ")
                    .Append("           COUNT(T1.DLRCD) AS COUNT ")
                    .Append("      FROM tbl_STALLPROCESS T1 ")
                    .Append("INNER JOIN tbl_STALLREZINFO T2 ")
                    .Append("        ON T2.DLRCD = T1.DLRCD ")
                    .Append("       AND T2.STRCD = T1.STRCD ")
                    .Append("       AND T2.REZID = T1.REZID ")
                    .Append("     WHERE T1.SEQNO =( SELECT MAX(T3.SEQNO) ")
                    .Append("                         FROM tbl_STALLPROCESS T3 ")
                    .Append("                        WHERE T3.DLRCD = T1.DLRCD ")
                    .Append("                          AND T3.STRCD = T1.STRCD ")
                    .Append("                          AND T3.REZID = T1.REZID ")
                    .Append("                     GROUP BY T3.DLRCD, T3.STRCD, T3.REZID ) ")
                    .Append("       AND T1.RESULT_STALLID = :RESULT_STALLID ")
                    .Append("       AND T1.RESULT_STATUS = '20' ")
                    .Append("       AND T2.CANCELFLG <> '1' ")
                    .Append("       AND T1.RESULT_START_TIME >= :RESULT_START_TIME1 ")
                    .Append("       AND T1.RESULT_START_TIME < :RESULT_START_TIME2")
                End With

                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("RESULT_STALLID", OracleDbType.Int64, stallId)
                'query.AddParameterWithTypeValue("RESULT_START_TIME1", OracleDbType.Char, startTime.ToString("yyyyMMddHHmmss"))
                'query.AddParameterWithTypeValue("RESULT_START_TIME2", OracleDbType.Char, endTime.ToString("yyyyMMddHHmmss"))
                query.AddParameterWithTypeValue("RESULT_START_TIME1", OracleDbType.Char, startTime.ToString("yyyyMMddHHmm", CultureInfo.InvariantCulture()))
                query.AddParameterWithTypeValue("RESULT_START_TIME2", OracleDbType.Char, endTime.ToString("yyyyMMddHHmm", CultureInfo.InvariantCulture()))

                Logger.Info("[E]GetWorkingStateCount()")

                ' 検索結果の返却
                Return query.GetData()

            End Using

        End Function


        ''' <summary>
        ''' 管理予約ID(PREZID)の取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="reserveId">予約ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetParentsReserveId(ByVal dealerCode As String, _
                                            ByVal branchCode As String, _
                                            ByVal reserveId As Integer) As SC3150101DataSet.SC3150101ParentsReserveIdDataTable

            Logger.Info("[S]GetParentsReserveId()")

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3150101DataSet.SC3150101ParentsReserveIdDataTable)("SC3150101_027")

                Dim sql As New StringBuilder

                ' SQL文の作成
                With sql
                    .Append("SELECT /* SC3150101_027 */ ")
                    .Append("       PREZID ")
                    .Append("  FROM tbl_STALLREZINFO ")
                    .Append(" WHERE DLRCD = :DLRCD ")
                    .Append("   AND STRCD = :STRCD ")
                    .Append("   AND REZID = :REZID")
                End With

                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
                query.AddParameterWithTypeValue("REZID", OracleDbType.Int64, reserveId)

                Logger.Info("[E]GetParentsReserveId()")

                ' 検索結果の返却
                Return query.GetData()

            End Using

        End Function


        ''' <summary>
        ''' リレーション内の作業終了(実績ステータス：97)チップの最大REZCHILDNOを取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="parentsReserveId">管理予約ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetRelationLastChildNo(ByVal dealerCode As String, _
                                               ByVal branchCode As String, _
                                               ByVal parentsReserveId As Integer) As SC3150101DataSet.SC3150101RelationLastChildNoDataTable

            Logger.Info("[S]GetRelationLastChildNo()")

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3150101DataSet.SC3150101RelationLastChildNoDataTable)("SC3150101_028")

                Dim sql As New StringBuilder

                ' SQL文の作成
                With sql
                    .Append("   SELECT /* SC3150101_028 */ ")
                    .Append("          MAX(T1.REZCHILDNO) REZCHILDNO ")
                    .Append("     FROM tbl_STALLREZINFO T1 ")
                    .Append("LEFT JOIN tbl_STALLPROCESS T2 ")
                    .Append("       ON T1.DLRCD = T2.DLRCD ")
                    .Append("      AND T1.STRCD = T2.STRCD ")
                    .Append("      AND T1.REZID = T2.REZID ")
                    .Append("    WHERE T1.DLRCD = :DLRCD ")
                    .Append("      AND T1.STRCD = :STRCD ")
                    .Append("      AND T1.PREZID = :PREZID ")
                    .Append("      AND T2.RESULT_STATUS = '97'")
                End With

                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
                query.AddParameterWithTypeValue("PREZID", OracleDbType.Int64, parentsReserveId)

                Logger.Info("[E]GetRelationLastChildNo()")

                ' 検索結果の返却
                Return query.GetData()

            End Using

        End Function


        ''' <summary>
        ''' リレーション内のREZCHILDNO更新対象を取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="parentsReserveId">管理予約ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetChildNoUpdateTarget(ByVal dealerCode As String, _
                                               ByVal branchCode As String, _
                                               ByVal parentsReserveId As Integer, _
                                               ByVal childNo As Integer, _
                                               ByVal reserveId As Integer) As SC3150101DataSet.SC3150101TargetChildNoInfoDataTable

            Logger.Info("[S]GetChildNoUpdateTarget()")

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3150101DataSet.SC3150101TargetChildNoInfoDataTable)("SC3150101_029")

                Dim sql As New StringBuilder

                ' SQL文の作成
                With sql
                    .Append("SELECT /* SC3150101_029 */ ")
                    .Append("       DLRCD, ")
                    .Append("       STRCD, ")
                    .Append("       REZID ")
                    .Append("  FROM tbl_STALLREZINFO ")
                    .Append(" WHERE DLRCD = :DLRCD ")
                    .Append("   AND STRCD = :STRCD ")
                    .Append("   AND PREZID = :PREZID ")
                    .Append("   AND REZCHILDNO > :REZCHILDNO ")
                    .Append("ORDER BY DECODE(REZID, :REZID, 1, 2) ASC, ")
                    .Append("         REZCHILDNO ASC ")
                End With

                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
                query.AddParameterWithTypeValue("PREZID", OracleDbType.Int64, parentsReserveId)
                query.AddParameterWithTypeValue("REZCHILDNO", OracleDbType.Int64, childNo)
                query.AddParameterWithTypeValue("REZID", OracleDbType.Int64, reserveId)

                Logger.Info("[E]GetChildNoUpdateTarget()")

                ' 検索結果の返却
                Return query.GetData()

            End Using

        End Function


        ''' <summary>
        ''' 指定チップのREZCHILDNOを指定値で更新
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="reserveId">予約ID</param>
        ''' <param name="childNo">子予約連番</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function UpdateChildNo(ByVal dealerCode As String, _
                                      ByVal branchCode As String, _
                                      ByVal reserveId As Integer, _
                                      ByVal childNo As Integer) As Integer

            Logger.Info("[S]UpdateChildNo()")

            ' DbUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("SC3150101_030")

                Dim sql As New StringBuilder

                ' SQL文の作成
                With sql
                    'etc
                    .Append("UPDATE /* SC3150101_030 */ ")
                    .Append("       tbl_STALLREZINFO ")
                    .Append("   SET REZCHILDNO = :REZCHILDNO ")
                    .Append(" WHERE DLRCD = :DLRCD ")
                    .Append("   AND STRCD = :STRCD ")
                    .Append("   AND REZID = :REZID")
                End With

                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("REZCHILDNO", OracleDbType.Int64, childNo)
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
                query.AddParameterWithTypeValue("REZID", OracleDbType.Int64, reserveId)

                Logger.Info("[E]UpdateChildNo()")

                'SQL実行
                Return query.Execute()

            End Using

        End Function


        ''' <summary>
        ''' 子チップのORDERNOを取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="parentsReserveId">管理予約ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetChildOrderNo(ByVal dealerCode As String, _
                                        ByVal branchCode As String, _
                                        ByVal parentsReserveId As Integer) As SC3150101DataSet.SC3150101ChildChipOrderNoDataTable

            Logger.Info("[S]GetChildOrderNo()")

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3150101DataSet.SC3150101ChildChipOrderNoDataTable)("SC3150101_031")

                Dim sql As New StringBuilder

                ' SQL文の作成
                With sql
                    .Append("SELECT /* SC3150101_031 */ ")
                    .Append("       ORDERNO ")            ' R/O No.
                    .Append("  FROM tbl_STALLREZINFO ")
                    .Append(" WHERE DLRCD = :DLRCD ")     ' 販売店コード
                    .Append("   AND STRCD = :STRCD ")     ' 店舗コード
                    .Append("   AND REZID = :PREZID ")    ' 予約ID

                End With

                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
                'query.AddParameterWithTypeValue("REZID", OracleDbType.Int64, reserveId)
                query.AddParameterWithTypeValue("PREZID", OracleDbType.Int64, parentsReserveId)
                'query.AddParameterWithTypeValue("REZCHILDNO", OracleDbType.Int64, childNo)

                Logger.Info("[E]GetChildOrderNo()")

                ' 検索結果の返却
                Return query.GetData()

            End Using

        End Function


        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="src"></param>
        ''' <param name="defult">デフォルト値</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function SetData(ByVal src As Object, ByVal defult As Object) As Object

            If IsDBNull(src) = True Then
                Return defult
            End If

            Return src

        End Function


        ''' <summary>
        ''' SQL用の値を設定
        ''' </summary>
        ''' <param name="Value">対象文字列</param>
        ''' <returns>SQLに設定する文字列</returns>
        ''' <remarks></remarks>
        Private Function SetSqlValue(ByVal value As String) As String
            ' 2012/02/27 KN 佐藤 【SERVICE_1】DevPartner 1回目の指摘事項を修正（処理修正） START
            'If IsNothing(value) OrElse value.Trim() = "" Then
            '    '値がない場合、半角スペースを設定
            '    value = " "
            'End If
            If String.IsNullOrEmpty(value) OrElse value.Trim.Length = 0 Then
                '値がない場合、半角スペースを設定
                value = " "
            End If
            ' 2012/02/27 KN 佐藤 【SERVICE_1】DevPartner 1回目の指摘事項を修正（処理修正） END
            Return value
        End Function


        ''' <summary>
        ''' 日付+00:00:00を返す
        ''' </summary>
        ''' <param name="Value">対象文字列</param>
        ''' <returns>SQLに設定する文字列</returns>
        ''' <remarks></remarks>
        Private Function SetSearchDate(ByVal value As Date) As String

            Dim retValue As String

            retValue = DateSerial(value.Year, value.Month, value.Day).ToString("yyyy/MM/dd HH:mm:ss", Globalization.CultureInfo.CurrentCulture())

            Return retValue

        End Function

    End Class

End Namespace
