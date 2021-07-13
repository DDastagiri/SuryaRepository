'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'IC3810501DataSet.vb
'─────────────────────────────────────
'機能： 完成検査結果連携
'補足： 
'作成： 2012/01/27 KN 佐藤
'更新： 2012/02/13 KN 佐藤 【SERVICE_1】単一の予約IDが取得できない不具合を修正
'更新： 2012/02/16 KN 佐藤 【SERVICE_1】SQLのレスポンス改善
'更新： 
'─────────────────────────────────────

Imports System.Text
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Oracle.DataAccess.Client

Partial Class IC3810501DataSet

End Class

Namespace IC3810501DataSetTableAdapters
    Public Class IC3810501StallInfoDataTableAdapter
        Inherits Global.System.ComponentModel.Component

        ''' <summary>
        ''' ストール実績情報を取得する。
        ''' </summary>
        ''' <param name="dealerCode">販売点コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="orderNo">整備受注NO</param>
        ''' <returns>処理結果</returns>
        ''' <remarks></remarks>
        ''' 
        ''' <History>
        ''' </History>
        Public Function GetStallProcessWorkingInfo(ByVal dealerCode As String, _
                                                    ByVal branchCode As String, _
                                                    ByVal orderNo As String) As IC3810501DataSet.IC3810501StallProcessInfoDataTable

            ' SQL文の作成
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* IC3810501_001 */ ")
                .Append("       T1.DLRCD ")                                                         ' 01 販売店コード
                .Append("     , T1.STRCD ")                                                         ' 02 店舗コード
                .Append("     , T2.REZID ")                                                         ' 03 予約ID
                .Append("     , T2.DSEQNO ")                                                        ' 04 日跨ぎシーケンス番号
                .Append("     , T2.SEQNO ")                                                         ' 05 シーケンス番号
                .Append("     , NVL(T2.RESULT_STATUS, '') AS RESULT_STATUS ")                       ' 16 実績_ステータス
                .Append("     , NVL(T2.RESULT_STALLID, 0) AS RESULT_STALLID ")                      ' 17 実績_ストールID
                .Append("     , NVL(T2.RESULT_START_TIME, '') AS RESULT_START_TIME ")               ' 18 実績_ストール開始日時時刻
                .Append("     , NVL(T2.RESULT_END_TIME, '') AS RESULT_END_TIME ")                   ' 19 実績_ストール終了日時時刻
                .Append("     , NVL(T2.RESULT_WORK_TIME, 0) AS RESULT_WORK_TIME ")                  ' 21 実績_実績時間
                .Append("     , NVL(T2.RESULT_IN_TIME, '') AS RESULT_IN_TIME ")                     ' 20 実績_入庫時間
                .Append("     , NVL(T2.REZ_START_TIME, '') AS REZ_START_TIME ")                     ' 23 予定_ストール開始日時時刻
                .Append("     , NVL(T2.REZ_END_TIME, '') AS REZ_END_TIME ")                         ' 24 予定_ストール終了日時時刻
                .Append("     , NVL(T2.REZ_WORK_TIME, 0) AS REZ_WORK_TIME ")                        ' 25 予定_作業時間
                .Append("     , NVL(T2.RESULT_WASH_START, '') AS RESULT_WASH_START ")               ' 34 洗車開始時刻
                .Append("     , NVL(T2.RESULT_WASH_END, '') AS RESULT_WASH_END ")                   ' 35 洗車終了時刻
                .Append("     , NVL(T2.RESULT_WAIT_START, '') AS RESULT_WAIT_START ")               ' 36 納車待ち開始時刻
                .Append("     , NVL(T2.RESULT_WAIT_END, '') AS RESULT_WAIT_END ")                   ' 37 納車待ち終了時刻
                .Append("     , NVL(T2.RESULT_INSPECTION_START, '') AS RESULT_INSPECTION_START ")   ' 51 実績検査開始時刻
                .Append("     , NVL(T2.RESULT_INSPECTION_END, '') AS RESULT_INSPECTION_END ")       ' 52 実績検査終了時刻
                ' 2012/02/13 KN 佐藤 【SERVICE_1】単一の予約IDが取得できない不具合を修正（処理修正） START
                '.Append("  FROM TBL_STALLREZINFO T1 ")                                              ' [ストール予約]
                '.Append("     , TBL_STALLPROCESS T2 ")                                              ' [ストール実績]
                '.Append(" WHERE T1.DLRCD = T2.DLRCD ")                                              ' 01 販売店コード
                '.Append("   AND T1.STRCD = T2.STRCD ")                                              ' 02 店舗コード
                '.Append("   AND T1.REZID = T2.REZID ")                                              ' 03 予約ID
                '.Append("   AND T1.DLRCD = :DLRCD ")                                                ' 01 販売店コード
                '.Append("   AND T1.STRCD = :STRCD ")                                                ' 02 店舗コード
                '.Append("   AND EXISTS ( ")                                                         ' 72 整備受注NO
                '.Append("       SELECT 1 ")
                '.Append("         FROM TBL_STALLREZINFO ")
                '.Append("        WHERE DLRCD = T1.DLRCD ")
                '.Append("          AND STRCD = T1.STRCD ")
                '.Append("          AND PREZID = T1.PREZID ")
                '.Append("          AND DLRCD = :DLRCD ")
                '.Append("          AND STRCD = :STRCD ")
                '.Append("          AND ORDERNO = :ORDERNO ")
                '.Append("       ) ")
                .Append("  FROM (SELECT DLRCD ")
                .Append("             , STRCD ")
                .Append("             , REZID ")
                .Append("          FROM TBL_STALLREZINFO ")
                .Append("         WHERE DLRCD = :DLRCD ")
                .Append("           AND STRCD = :STRCD ")
                .Append("           AND ORDERNO = :ORDERNO ")
                .Append("           AND PREZID IS  NULL ")
                .Append("         UNION ALL ")
                .Append("        SELECT T3.DLRCD ")
                .Append("             , T3.STRCD ")
                .Append("             , T3.REZID ")
                .Append("          FROM TBL_STALLREZINFO T3 ")
                .Append("         WHERE T3.DLRCD = :DLRCD ")
                .Append("           AND T3.STRCD = :STRCD ")
                .Append("           AND EXISTS (SELECT 1 ")
                .Append("                         FROM TBL_STALLREZINFO ")
                .Append("                        WHERE DLRCD = T3.DLRCD ")
                .Append("                          AND STRCD = T3.STRCD ")
                .Append("                          AND PREZID = T3.PREZID ")
                .Append("                          AND DLRCD = :DLRCD ")
                .Append("                          AND STRCD = :STRCD ")
                .Append("                          AND ORDERNO = :ORDERNO ")
                .Append("                          AND PREZID IS NOT NULL ")
                .Append("                      ) ")
                .Append("       ) T1 ")
                .Append("     , TBL_STALLPROCESS T2 ")
                .Append(" WHERE T1.DLRCD = T2.DLRCD ")
                .Append("   AND T1.STRCD = T2.STRCD ")
                .Append("   AND T1.REZID = T2.REZID ")
                ' 2012/02/13 KN 佐藤 【SERVICE_1】単一の予約IDが取得できない不具合を修正（処理修正） END
                .Append("   AND T2.RESULT_STATUS = '20' ")                                          ' 16 実績_ステータス
                .Append(" ORDER BY T2.DSEQNO DESC ")                                                ' 04 日跨ぎシーケンス番号
                .Append("     , T2.SEQNO DESC ")                                                    ' 05 シーケンス番号
            End With

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of IC3810501DataSet.IC3810501StallProcessInfoDataTable)("IC3810501_001")
                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
                query.AddParameterWithTypeValue("ORDERNO", OracleDbType.Char, orderNo)

                ' SQLの実行
                Return query.GetData()
            End Using

        End Function

        ''' <summary>
        ''' 最終チップの取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="reserveId">予約ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' 
        ''' <History>
        ''' </History>
        Public Function GetLastChip(ByVal dealerCode As String, _
                                    ByVal branchCode As String, _
                                    ByVal reserveId As Integer) As IC3810501DataSet.IC3810501GetLastChipDataTable

            ' SQL文の作成
            Dim sql As New StringBuilder
            With sql
                .Append(" SELECT /* IC3810501_002 */ ")
                .Append("        T1.REZID ")
                ' 2012/02/16 KN 佐藤 【SERVICE_1】SQLのレスポンス改善（処理修正） START
                '.Append("   FROM ")
                '.Append("        TBL_STALLREZINFO T1")
                '.Append("  WHERE T1.DLRCD = :DLRCD ")
                '.Append("    AND T1.STRCD = :STRCD ")
                '.Append("    AND T1.STATUS < 3 ")
                '.Append("    AND T1.CANCELFLG = '0' ")
                '' 2012/02/13 KN 佐藤 【SERVICE_1】単一の予約IDが取得できない不具合を修正（処理修正） START
                ''.Append("    AND EXISTS ( ")
                ''.Append("        SELECT 1 ")
                ''.Append("          FROM TBL_STALLREZINFO ")
                ''.Append("         WHERE DLRCD = T1.DLRCD ")
                ''.Append("           AND STRCD = T1.STRCD ")
                ''.Append("           AND PREZID = T1.PREZID ")
                ''.Append("           AND DLRCD = :DLRCD ")
                ''.Append("           AND STRCD = :STRCD ")
                ''.Append("           AND REZID = :REZID ")
                ''.Append("         ) ")
                '.Append("    AND ((    T1.REZID = :REZID ")
                '.Append("          AND T1.PREZID IS NULL ")
                '.Append("         ) ")
                '.Append("           OR EXISTS ( ")
                '.Append("                      SELECT 1 ")
                '.Append("                        FROM TBL_STALLREZINFO ")
                '.Append("                       WHERE DLRCD = T1.DLRCD ")
                '.Append("                         AND STRCD = T1.STRCD ")
                '.Append("                         AND PREZID = T1.PREZID ")
                '.Append("                         AND PREZID IS NOT NULL ")
                '.Append("                         AND DLRCD = :DLRCD ")
                '.Append("                         AND STRCD = :STRCD ")
                '.Append("                         AND REZID = :REZID ")
                '.Append("                     ) ")
                '.Append("        ) ")
                '' 2012/02/13 KN 佐藤 【SERVICE_1】単一の予約IDが取得できない不具合を修正（処理修正） END
                .Append("  FROM (SELECT T2.REZID ")
                .Append("             , T2.REZCHILDNO ")
                .Append("          FROM TBL_STALLREZINFO T2 ")
                .Append("         WHERE T2.DLRCD = :DLRCD ")
                .Append("           AND T2.STRCD = :STRCD ")
                .Append("           AND T2.REZID = :REZID ")
                .Append("           AND T2.STATUS < 3 ")
                .Append("           AND T2.PREZID IS NULL ")
                .Append("           AND T2.CANCELFLG = '0' ")
                .Append("         UNION ALL ")
                .Append("        SELECT T3.REZID ")
                .Append("             , T3.REZCHILDNO ")
                .Append("          FROM TBL_STALLREZINFO T3 ")
                .Append("         WHERE T3.DLRCD = :DLRCD ")
                .Append("           AND T3.STRCD = :STRCD ")
                .Append("           AND EXISTS (SELECT 1 ")
                .Append("                         FROM TBL_STALLREZINFO ")
                .Append("                        WHERE DLRCD = T3.DLRCD ")
                .Append("                          AND STRCD = T3.STRCD ")
                .Append("                          AND PREZID = T3.PREZID ")
                .Append("                          AND DLRCD = :DLRCD ")
                .Append("                          AND STRCD = :STRCD ")
                .Append("                          AND REZID = :REZID ")
                .Append("                          AND STATUS < 3 ")
                .Append("                          AND PREZID IS NOT NULL ")
                .Append("                          AND CANCELFLG = '0' ")
                .Append("                      ) ")
                .Append("           AND T3.STATUS < 3 ")
                .Append("           AND T3.CANCELFLG = '0' ")
                .Append("       ) T1 ")
                ' 2012/02/16 KN 佐藤 【SERVICE_1】SQLのレスポンス改善（処理修正） END
                .Append("  ORDER BY T1.REZCHILDNO DESC ")
            End With

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of IC3810501DataSet.IC3810501GetLastChipDataTable)("IC3810501_002")
                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
                query.AddParameterWithTypeValue("REZID", OracleDbType.Int64, reserveId)

                ' 検索結果の返却
                Return query.GetData()
            End Using

        End Function

        ''' <summary>
        ''' ストール実績のシーケンス番号取得（洗車順データ）
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' 
        ''' <History>
        ''' </History>
        Public Function GetWashRefreshSeq(ByVal dealerCode As String, ByVal branchCode As String) As IC3810501DataSet.IC3810501WashRefreshSeqDataTable

            ' SQL文の作成
            Dim sql As New StringBuilder
            With sql
                .Append("    SELECT /* IC3810501_003 */ ")
                .Append("           T1.REZID ")
                .Append("         , T1.SEQNO ")
                .Append("      FROM ")
                .Append("           TBL_STALLPROCESS T1 ")
                .Append("         , TBL_STALLREZINFO T2 ")
                .Append("     WHERE T1.DLRCD = T2.DLRCD ")
                .Append("       AND T1.STRCD = T2.STRCD ")
                .Append("       AND T1.REZID = T2.REZID ")
                .Append("       AND T1.DLRCD = :DLRCD ")
                .Append("       AND T1.STRCD = :STRCD ")
                .Append("       AND T1.RESULT_STATUS IN ('40', '41') ")
                .Append("       AND T1.DSEQNO = ( SELECT MAX(T4.DSEQNO) ")
                .Append("                           FROM TBL_STALLPROCESS T4 ")
                .Append("                          WHERE T4.DLRCD = T1.DLRCD ")
                .Append("                            AND T4.STRCD = T1.STRCD ")
                .Append("                            AND T4.REZID = T1.REZID ")
                .Append("                          GROUP BY T4.DLRCD, T4.STRCD, T4.REZID ) ")
                .Append("       AND T1.SEQNO = ( SELECT MAX(T5.SEQNO) ")
                .Append("                          FROM TBL_STALLPROCESS T5 ")
                .Append("                         WHERE T5.DLRCD = T1.DLRCD ")
                .Append("                           AND T5.STRCD = T1.STRCD ")
                .Append("                           AND T5.REZID = T1.REZID ")
                .Append("                           AND T5.DSEQNO = T1.DSEQNO ) ")
                .Append("       AND (T2.CANCELFLG = '0' OR T2.STOPFLG = '1' ) ")
                .Append("     ORDER BY T1.RESULT_END_TIME ")
                .Append("         , T1.UPDATEDATE ")
            End With

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of IC3810501DataSet.IC3810501WashRefreshSeqDataTable)("IC3810501_003")
                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)

                ' 検索結果の返却
                Return query.GetData()
            End Using
        End Function

        ''' <summary>
        ''' 洗車順データ削除
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' 
        ''' <History>
        ''' </History>
        Public Function DeleteWashData(ByVal dealerCode As String, ByVal branchCode As String) As Integer

            ' SQL文の作成
            Dim sql As New StringBuilder
            With sql
                .Length = 0
                .Append(" DELETE /* IC3810501_004 */ ")
                .Append("   FROM TBL_WASHDATA ")
                .Append("  WHERE DLRCD = :DLRCD ")
                .Append("    AND STRCD = :STRCD ")
            End With

            ' DbUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("IC3810501_004")
                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)

                ' SQL実行
                Return query.Execute()

            End Using

        End Function

        ''' <summary>
        ''' 洗車順データ追加
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="reserveId">予約ID</param>
        ''' <param name="seqNo">シーケンス番号</param>
        ''' <param name="washSeq">洗車シーケンス番号</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' 
        ''' <History>
        ''' </History>
        Public Function InsertWashData(ByVal dealerCode As String, _
                                        ByVal branchCode As String, _
                                        ByVal reserveId As Integer, _
                                        ByVal seqNo As Integer, _
                                        ByVal washSeq As Integer) As Integer

            ' SQL文の作成
            Dim sql As New StringBuilder
            With sql
                .Append(" INSERT /* IC3810501_005 */ ")
                .Append("   INTO TBL_WASHDATA ( ")
                .Append("        DLRCD, ")
                .Append("        STRCD, ")
                .Append("        REZID, ")
                .Append("        SEQNO, ")
                .Append("        WASHSEQ, ")
                .Append("        INPUTACCOUNT, ")
                .Append("        CREATEDATE, ")
                .Append("        UPDATEDATE ")
                .Append(" ) ")
                .Append(" VALUES ( ")
                .Append("        :DLRCD, ")
                .Append("        :STRCD, ")
                .Append("        :REZID, ")
                .Append("        :SEQNO, ")
                .Append("        :WASHSEQ, ")
                .Append("        ' ', ")
                .Append("        SYSDATE, ")
                .Append("        SYSDATE) ")
            End With

            ' DbUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("IC3810501_005")
                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
                query.AddParameterWithTypeValue("REZID", OracleDbType.Int64, reserveId)
                query.AddParameterWithTypeValue("SEQNO", OracleDbType.Int64, seqNo)
                query.AddParameterWithTypeValue("WASHSEQ", OracleDbType.Int64, washSeq)

                ' SQL実行
                Return query.Execute()

            End Using

        End Function

        ''' <summary>
        ''' ストール実績のシーケンス番号取得（検査順データ）
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' 
        ''' <History>
        ''' </History>
        Public Function GetInspectionRefreshSeq(ByVal dealerCode As String, ByVal branchCode As String) As IC3810501DataSet.IC3810501InspectionRefreshSeqDataTable

            ' SQL文の作成
            Dim sql As New StringBuilder
            With sql
                .Append(" SELECT /* IC3810501_006 */ ")
                .Append("        T1.REZID, ")
                .Append("        T1.SEQNO ")
                .Append("   FROM TBL_STALLPROCESS T1 ")
                .Append("      , TBL_STALLREZINFO T2 ")
                .Append("  WHERE T1.DLRCD = T2.DLRCD ")
                .Append("    AND T1.STRCD = T2.STRCD ")
                .Append("    AND T1.REZID = T2.REZID ")
                .Append("    AND T1.DLRCD = :DLRCD ")
                .Append("    AND T1.STRCD = :STRCD ")
                .Append("    AND T1.RESULT_STATUS IN ('42', '43') ")
                .Append("    AND T1.DSEQNO = ( SELECT MAX(T4.DSEQNO) ")
                .Append("                        FROM TBL_STALLPROCESS T4 ")
                .Append("                       WHERE T4.DLRCD = T1.DLRCD ")
                .Append("                         AND T4.STRCD = T1.STRCD ")
                .Append("                         AND T4.REZID = T1.REZID ")
                .Append("                       GROUP BY T4.DLRCD, T4.STRCD, T4.REZID ) ")
                .Append("    AND T1.SEQNO = ( SELECT MAX(T5.SEQNO) ")
                .Append("                       FROM TBL_STALLPROCESS T5 ")
                .Append("                      WHERE T5.DLRCD = T1.DLRCD ")
                .Append("                        AND T5.STRCD = T1.STRCD ")
                .Append("                        AND T5.REZID = T1.REZID ")
                .Append("                        AND T5.DSEQNO = T1.DSEQNO ) ")
                .Append("    AND (T2.CANCELFLG = '0' OR T2.STOPFLG = '1' ) ")
                .Append("  ORDER BY T1.RESULT_END_TIME, ")
                .Append("        T1.UPDATEDATE ")
            End With

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of IC3810501DataSet.IC3810501InspectionRefreshSeqDataTable)("IC3810501_006")
                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)

                ' 検索結果の返却
                Return query.GetData()
            End Using

        End Function

        ''' <summary>
        ''' 検査順データ削除
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' 
        ''' <History>
        ''' </History>
        Public Function DeleteInspectionData(ByVal dealerCode As String, ByVal branchCode As String) As Integer

            ' SQL文の作成
            Dim sql As New StringBuilder
            With sql
                .Length = 0
                .Append(" DELETE /* IC3810501_007 */ ")
                .Append("   FROM TBL_INSPECTIONDATA ")
                .Append("  WHERE DLRCD = :DLRCD ")
                .Append("    AND STRCD = :STRCD ")
            End With

            ' DbUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("IC3810501_007")
                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)

                ' SQL実行
                Return query.Execute()

            End Using

        End Function

        ''' <summary>
        ''' 検査順データ追加
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="reserveId">予約ID</param>
        ''' <param name="inspectionSeq">シーケンス番号</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' 
        ''' <History>
        ''' </History>
        Public Function InsertInspectionData(ByVal dealerCode As String, _
                                                ByVal branchCode As String, _
                                                ByVal reserveId As Integer, _
                                                ByVal inspectionSeq As Integer) As Integer

            ' SQL文の作成
            Dim sql As New StringBuilder
            With sql
                .Append(" INSERT /* IC3810501_008 */ ")
                .Append("   INTO TBL_INSPECTIONDATA ( ")
                .Append("        DLRCD, ")
                .Append("        STRCD, ")
                .Append("        REZID, ")
                .Append("        INSPECTIONSEQ, ")
                .Append("        INPUTACCOUNT, ")
                .Append("        CREATEDATE, ")
                .Append("        UPDATEDATE ")
                .Append(" ) ")
                .Append(" VALUES ( ")
                .Append("        :DLRCD, ")
                .Append("        :STRCD, ")
                .Append("        :REZID, ")
                .Append("        :INSPECTIONSEQ, ")
                .Append("        ' ', ")
                .Append("        SYSDATE, ")
                .Append("        SYSDATE) ")
            End With

            ' DbUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("IC3810501_008")
                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
                query.AddParameterWithTypeValue("REZID", OracleDbType.Int64, reserveId)
                query.AddParameterWithTypeValue("INSPECTIONSEQ", OracleDbType.Int64, inspectionSeq)

                ' SQL実行
                Return query.Execute()

            End Using

        End Function

    End Class

End Namespace

Partial Class IC3810501DataSet

End Class
