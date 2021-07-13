'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3100101DataSet.vb
'──────────────────────────────────
'機能： 受付メイン
'補足： 
'作成： 2011/12/12 KN t.mizumoto
'更新： 2012/08/23 TMEJ m.okamura 新車受付機能改善 $01
'更新： 2012/12/26 TMEJ t.shimamura 新車タブレットショールーム管理機能開発 $02
'更新： 2013/02/26 TMEJ t.shimamura 新車タブレット受付画面管理指標の変更対応 $03
'更新： 2013/05/22 TMEJ t.shimamura 問連対応 $04
'更新： 2013/05/24 TMEJ t.shimamura 【A.STEP2】次世代e-CRB新車タブレット　新DB適応に向けた機能開発 $05
'更新： 2014/03/05 TMEJ m.asano 受注後フォロー機能開発（スタッフ活動KPI）に向けたシステム設計 $06
'更新： 2018/07/12 NSK a.kani TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示
'更新： 2020/02/05 NSK s.natsume TKM Change request development for Next Gen e-CRB (CR008) $08
'更新： 2020/02/05 NSK s.natsume TKM Change request development for Next Gen e-CRB (CR075) $09
'更新： 2020/02/05 NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060) $10
'更新：
'──────────────────────────────────

Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core


Namespace SC3100101DataSetTableAdapters

    ''' <summary>
    ''' 受付メインのデータアクセスクラス
    ''' </summary>
    ''' <remarks></remarks>
    Public Class SC3100101TableAdapter
        Inherits Global.System.ComponentModel.Component

#Region "定数"
#Region "来店実績ステータス"
        ''' <summary>
        ''' 来店実績ステータス（フリー）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const VisitStatusFree As String = "01"

        ''' <summary>
        ''' 来店実績ステータス（フリー（ブロードキャスト））
        ''' </summary>
        ''' <remarks></remarks>
        Private Const VisitStatusFreeBroadcast As String = "02"

        ''' <summary>
        ''' 来店実績ステータス（調整中）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const VisitStatusAdjustment As String = "03"

        ''' <summary>
        ''' 来店実績ステータス（確定（ブロードキャスト））
        ''' </summary>
        ''' <remarks></remarks>
        Private Const VisitStatusDecisionBroadcast As String = "04"

        ''' <summary>
        ''' 来店実績ステータス（確定）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const VisitStatusDecision As String = "05"

        ''' <summary>
        ''' 来店実績ステータス（待ち）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const VisitStatusWating As String = "06"

        ''' <summary>
        ''' 来店実績ステータス（商談中）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const VisitStatusNegotiate As String = "07"

        ''' <summary>
        ''' 来店実績ステータス（商談終了）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const VisitStatusNegotiateEnd As String = "08"

        ' $01 start 複数顧客に対する商談平行対応
        ''' <summary>
        ''' 来店実績ステータス（商談中断）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const VisitStatusNegotiateStop As String = "09"
        ' $01 end   複数顧客に対する商談平行対応

        ' $03 start 納車作業ステータス対応
        ''' <summary>
        ''' 来店実績ステータス（納車作業中）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const VisitStatusDeliverlyStart As String = "11"
        ''' <summary>
        ''' 来店実績ステータス（納車作業中）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const VisitStatusDeliverlyEnd As String = "12"
        ' $03 end   納車作業ステータス対応

        ''' <summary>
        ''' 来店実績ステータス（来店キャンセル）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const VisitStatusCancel As String = "99"
#End Region
#Region "スタッフ在席状況"
        ''' <summary>
        ''' スタッフステータス（スタンバイ）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const StaffStatusStandby As String = "1"

        ''' <summary>
        ''' スタッフステータス（商談中）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const StaffStatusNegotiate As String = "2"

        ''' <summary>
        ''' スタッフステータス（一時退席）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const StaffStatusLeaving As String = "3"

        ''' <summary>
        ''' スタッフステータス（オフライン）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const StaffStatusOffline As String = "4"

        ' $02 start

        ''' <summary>
        ''' スタッフステータス（納車作業中）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const StaffStatusDeliverly As String = "5"
        ' 02 end
#End Region

        ''' <summary>
        ''' 操作権限コード（セールススタッフ）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const OperationCodeSalesStaff As Long = 8

        ''' <summary>
        ''' 削除フラグ（未削除）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const DeleteFlagNotDelete As String = "0"

        ''' <summary>
        ''' 削除フラグ（削除）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const DeleteFlagDelete As String = "1"

        ''' <summary>
        ''' 顧客区分（自社客）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const VisitorSegmentOriginal As String = "1"

        ''' <summary>
        ''' 顧客区分（新規顧客）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const VisitorSegmentNew As String = "2"

        ''' <summary>
        ''' ブロードキャストフラグ（送信済み）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const BroadcastFlagOn As String = "1"

        ''' <summary>
        ''' ブロードキャストフラグ（未送信）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const BroadcastFlagOff As String = "0"

        ''' <summary>
        ''' 更新ID
        ''' </summary>
        ''' <remarks></remarks>
        Private Const UpdateId As String = "SC3100101"

        ''' <summary>
        ''' 商談テーブル未設定
        ''' </summary>
        ''' <remarks></remarks>
        Private Const SalesTableNull As Integer = -1

        ' $02 start 受付共通からの移植
#Region "受付共通"
        ''' <summary>
        ''' 苦情情報ステータス（1次対応中）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ClaimStatusFirst As String = "1"

        ''' <summary>
        ''' 苦情情報ステータス（最終対応中）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ClaimStatusLast As String = "2"

        ''' <summary>
        ''' 苦情情報ステータス（完了）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ClaimStatusComplete As String = "3"

        ''' <summary>
        ''' 苦情情報紐付け関係フラグ(親)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const RelationFlgOn As String = "1"

        ''' <summary>
        ''' 苦情情報付け関係フラグ(なし)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const RelationFlgOff As String = "0"

        ''' <summary>
        ''' 顧客種別（所有者）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const CustomerClassOwner As String = "1"
        '2018/07/12 NSK a.kani TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
        ''' <summary>
        ''' オーナーチェンジフラグ（0：オーナーチェンジ無し）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const OwnerChgFlgNone As String = "0"
        ''' <summary>
        ''' Lマークのフラグ（2：Lマーク表示）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const IconFlagL As String = "2"
        ''' <summary>
        ''' マークのフラグ（1：表示）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const IconFlagOn As String = "1"
        ''' <summary>
        ''' マークのフラグ（0：非表示）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const IconFlagOff As String = "0"
        '2018/07/12 NSK a.kani TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
#Region "通知"
        ''' <summary>
        ''' 最終ステータス(依頼)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const LastStatusAssessment As String = "1"

        ''' <summary>
        ''' 最終ステータス(受信)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const LastStatusRecv As String = "3"

        ''' <summary>
        ''' 通知ステータス(依頼)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const NoticeStatus As String = "1"

        ''' <summary>
        ''' 通知ステータス(受信)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ReceiveStatus As String = "3"
#End Region
        ''' <summary>
        ''' 来店回数取得(6:来店)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ContactNoVisit As Long = 11

        ''' <summary>
        ''' 処理区分(3:Success/Give-Uo以外)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const RegistrationType As String = "3"
#End Region

        ' $02 end 受付共通からの移植
        ' $02 start 新車タブレットショールーム管理機能開発5
        ''' <summary>
        ''' 通知種類 - 査定
        ''' </summary>
        ''' <remarks></remarks>
        Private Const NoticeReqCGTAssessment As String = "01"

        ''' <summary>
        ''' CR活動結果 - Success 
        ''' </summary>
        ''' <remarks></remarks>
        Private Const CRActResultSuccess As String = "3"

        ''' <summary>
        ''' 通知種別 - 依頼
        ''' </summary>
        ''' <remarks></remarks>
        Private Const NoticeInfoStatusRequest As String = "1"

        ''' <summary>
        ''' 接客区分 - 振り当て待ち 
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ReceptionClassWaitAssgined As String = "1"

        ''' <summary>
        ''' 接客区分 - 接客待ち 
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ReceptionClassWaitService As String = "2"

        ''' <summary>
        ''' 接客区分 - 接客中
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ReceptionClassNegotiation As String = "3"

        ''' <summary>
        ''' 来店実績ステータス - 接客不要 
        ''' </summary>
        ''' <remarks></remarks>
        Private Const VisitStatusUnnecessary As String = "10"

#Region "スタッフ在席状況小分類"
        ''' <summary>
        ''' スタッフステータス小分類 - 0
        ''' </summary>
        ''' <remarks></remarks>
        Private Const PresenceDetail0 As String = "0"

        ''' <summary>
        ''' スタッフステータス小分類 - 1
        ''' </summary>
        ''' <remarks></remarks>
        Private Const PresenceDetail1 As String = "1"

        ''' <summary>
        ''' スタッフステータス小分類 - 2
        ''' </summary>
        ''' <remarks></remarks>
        Private Const PresenceDetail2 As String = "2"

        ''' <summary>
        ''' スタッフステータス小分類 - 3
        ''' </summary>
        ''' <remarks></remarks>
        Private Const PresenceDetail3 As String = "3"
#End Region

        ' $06 start
        ''' <summary>
        ''' 商談ステータス（Success）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const SalesStatusSuccess As String = "31"
        ' $06 end

        ''' <summary>
        ''' 最終ステータス(回答)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const LastStatusAnswer As String = "4"

        ' $02 end 新車タブレットショールーム管理機能開発

        ' $05 start
        ''' <summary>
        ''' 用件内容(苦情)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const BizTypeClaim As String = "3"
        ' $05 end

        '$09 NSK s.natsume TKM Change request development for Next Gen e-CRB (CR075) START
        ''' <summary>
        ''' 実績商談分類(見積)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ResultSalesActionContract As String = "6"

        ''' <summary>
        ''' 受注後活動コード(受注)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const AfterOrderActionCodeContract As String = "11"
        '$09 NSK s.natsume TKM Change request development for Next Gen e-CRB (CR075) END

#End Region

#Region "商談テーブル使用有無の取得"

        ''' <summary>
        ''' 商談テーブル使用有無の取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="storeCode">店舗コード</param>
        ''' <param name="startTime">開始日時</param>
        ''' <param name="endTime">終了日時</param>
        ''' <returns>商談テーブル使用有無データテーブル</returns>
        ''' <remarks></remarks>
        Public Function GetSalesTableUse(ByVal dealerCode As String, ByVal storeCode As String, _
                                         ByVal startTime As Date, ByVal endTime As Date) _
                                         As SC3100101DataSet.SC3100101SalesTableUseDataTable
            'SQL組み立て
            Dim sql As New StringBuilder
            With sql
                .Append(" SELECT /* SC3100101_008 */")
                .Append("        ST.SALESTABLENO")
                .Append("      , CASE ")
                .Append("            WHEN UT.SALESTABLENO IS NULL THEN '0'")
                .Append("            ELSE '1'")
                .Append("        END AS SHIYOFLG")
                .Append("   FROM TBL_SALESTABLE ST")
                .Append("      , (")
                .Append("     SELECT DISTINCT SALESTABLENO")
                .Append("       FROM TBL_VISIT_SALES")
                .Append("      WHERE DLRCD = :DLRCD")
                .Append("        AND STRCD = :STRCD")
                .Append("        AND VISITSTATUS IN (:VISITSTATUS1, :VISITSTATUS2, :VISITSTATUS3, ")
                .Append("                            :VISITSTATUS4, :VISITSTATUS5, :VISITSTATUS6, ")
                ' $01 start 複数顧客に対する商談平行対応
                .Append("                            :VISITSTATUS7, :VISITSTATUS9, ")
                ' $03 start 納車作業ステータス対応
                .Append("                            :VISITSTATUS11)")
                ' $03 end   納車作業ステータス対応
                .Append("        AND NVL(STOPTIME, NVL(VISITTIMESTAMP, SALESSTART)) BETWEEN :STARTTIME AND :ENDTIME")
                ' $01 end   複数顧客に対する商談平行対応
                .Append("        ) UT")
                .Append(" WHERE ST.SALESTABLENO = UT.SALESTABLENO(+)")
                .Append("   AND ST.DLRCD = :DLRCD")
                .Append("   AND ST.STRCD = :STRCD")
                .Append("   AND ROWNUM <= 20")
                .Append(" ORDER BY ST.SALESTABLENO")
            End With

            'DbSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3100101DataSet.SC3100101SalesTableUseDataTable)("SC3100101_008")
                query.CommandText = sql.ToString()

                'SQLパラメータ設定
                query.AddParameterWithTypeValue("VISITSTATUS1", OracleDbType.Char, VisitStatusFree)
                query.AddParameterWithTypeValue("VISITSTATUS2", OracleDbType.Char, VisitStatusFreeBroadcast)
                query.AddParameterWithTypeValue("VISITSTATUS3", OracleDbType.Char, VisitStatusAdjustment)
                query.AddParameterWithTypeValue("VISITSTATUS4", OracleDbType.Char, VisitStatusDecisionBroadcast)
                query.AddParameterWithTypeValue("VISITSTATUS5", OracleDbType.Char, VisitStatusDecision)
                query.AddParameterWithTypeValue("VISITSTATUS6", OracleDbType.Char, VisitStatusWating)
                query.AddParameterWithTypeValue("VISITSTATUS7", OracleDbType.Char, VisitStatusNegotiate)
                ' $01 start 複数顧客に対する商談平行対応
                query.AddParameterWithTypeValue("VISITSTATUS9", OracleDbType.Char, VisitStatusNegotiateStop)
                ' $01 end   複数顧客に対する商談平行対応
                ' $03 start 納車作業ステータス対応
                query.AddParameterWithTypeValue("VISITSTATUS11", OracleDbType.Char, VisitStatusDeliverlyStart)
                ' $03 end   納車作業ステータス対応
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, storeCode)
                query.AddParameterWithTypeValue("STARTTIME", OracleDbType.Date, startTime)
                query.AddParameterWithTypeValue("ENDTIME", OracleDbType.Date, endTime)

                'SQL実行（結果表を返却）
                Return query.GetData()
            End Using
        End Function
#End Region

#Region "来店実績キャンセル更新"

        ''' <summary>
        ''' 来店実績キャンセル更新
        ''' </summary>
        ''' <param name="visitSequence">来店実績連番</param>
        ''' <param name="updateAccount">更新アカウント</param>
        ''' <returns>処理結果</returns>
        ''' <remarks></remarks>
        Public Function UpdateVisitorCancel(ByVal visitSequence As Long, _
                                            ByVal updateAccount As String) As Boolean
            'DbUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("SC3100101_010")

                'SQL組み立て
                Dim sql As New StringBuilder
                With sql
                    .Append(" UPDATE /* SC3100101_010 */")
                    .Append("        TBL_VISIT_SALES")
                    .Append("    SET VISITSTATUS = :VISITSTATUS")
                    .Append("      , UPDATEDATE = SYSDATE")
                    .Append("      , UPDATEACCOUNT = :UPDATEACCOUNT")
                    .Append("      , UPDATEID = :UPDATEID")
                    .Append("  WHERE VISITSEQ = :VISITSEQ")
                    .Append("    AND VISITSTATUS IN (:VISITSTATUS1, :VISITSTATUS2, :VISITSTATUS3,")
                    ' $01 start 複数顧客に対する商談平行対応
                    .Append("                        :VISITSTATUS4, :VISITSTATUS5, :VISITSTATUS6,")
                    ' $02 start 新車タブレットショールーム管理機能開発
                    .Append("                        :VISITSTATUS9, :VISITSTATUS10)")
                    ' $02 end 新車タブレットショールーム管理機能開発
                    ' $01 end   複数顧客に対する商談平行対応
                End With

                query.CommandText = sql.ToString()

                'SQLパラメータ設定
                query.AddParameterWithTypeValue("VISITSTATUS", OracleDbType.Char, VisitStatusCancel)
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Char, updateAccount)
                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Char, UpdateId)
                query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Long, visitSequence)
                query.AddParameterWithTypeValue("VISITSTATUS1", OracleDbType.Char, VisitStatusFree)
                query.AddParameterWithTypeValue("VISITSTATUS2", OracleDbType.Char, VisitStatusFreeBroadcast)
                query.AddParameterWithTypeValue("VISITSTATUS3", OracleDbType.Char, VisitStatusAdjustment)
                query.AddParameterWithTypeValue("VISITSTATUS4", OracleDbType.Char, VisitStatusDecisionBroadcast)
                query.AddParameterWithTypeValue("VISITSTATUS5", OracleDbType.Char, VisitStatusDecision)
                query.AddParameterWithTypeValue("VISITSTATUS6", OracleDbType.Char, VisitStatusWating)
                ' $01 start 複数顧客に対する商談平行対応
                query.AddParameterWithTypeValue("VISITSTATUS9", OracleDbType.Char, VisitStatusNegotiateStop)
                ' $01 end   複数顧客に対する商談平行対応
                ' $02 start 新車タブレットショールーム管理機能開発
                query.AddParameterWithTypeValue("VISITSTATUS10", OracleDbType.Char, VisitStatusUnnecessary)
                ' $02 end 新車タブレットショールーム管理機能開発

                'SQL実行（結果を返却）
                If query.Execute() > 0 Then
                    Return True
                Else
                    Return False
                End If
            End Using
        End Function
#End Region

#Region "仮登録氏名更新"

        ''' <summary>
        ''' 仮登録氏名更新
        ''' </summary>
        ''' <param name="visitSequence">来店実績連番</param>
        ''' <param name="tentativeName">仮登録氏名</param>
        ''' <returns>処理結果</returns>
        ''' <remarks></remarks>
        Public Function UpdateTentativeName(ByVal visitSequence As Long, _
                                            ByVal tentativeName As String, _
                                            ByVal updateAccount As String) As Boolean
            'DbUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("SC3100101_011")

                'SQL組み立て
                Dim sql As New StringBuilder
                With sql
                    .Append(" UPDATE /* SC3100101_011 */")
                    .Append("        TBL_VISIT_SALES")
                    .Append("    SET TENTATIVENAME = :TENTATIVENAME")
                    .Append("      , UPDATEDATE = SYSDATE")
                    .Append("      , UPDATEACCOUNT = :UPDATEACCOUNT")
                    .Append("      , UPDATEID = :UPDATEID")
                    .Append("  WHERE VISITSEQ = :VISITSEQ")
                    .Append("    AND CUSTSEGMENT IS NULL")
                End With

                query.CommandText = sql.ToString()

                'SQLパラメータ設定
                query.AddParameterWithTypeValue("TENTATIVENAME", OracleDbType.Char, tentativeName)
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Char, updateAccount)
                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Char, UpdateId)
                query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Long, visitSequence)

                'SQL実行（結果を返却）
                If query.Execute() > 0 Then
                    Return True
                Else
                    Return False
                End If
            End Using
        End Function
#End Region

#Region "来店実績使用商談テーブル更新"

        ''' <summary>
        ''' 来店実績使用商談テーブル更新
        ''' </summary>
        ''' <param name="visitSequence">来店実績連番</param>
        ''' <param name="oldSalesTableNo">更新前商談テーブルNo.</param>
        ''' <param name="newSalesTableNo">更新後商談テーブルNo.</param>
        ''' <param name="updateAccount">更新アカウント</param>
        ''' <returns>処理結果</returns>
        ''' <remarks></remarks>
        Public Function UpdateSalesTableNo(ByVal visitSequence As Long, _
                                           ByVal oldSalesTableNo As Integer, _
                                           ByVal newSalesTableNo As Integer, _
                                           ByVal updateAccount As String) As Boolean
            'DbUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("SC3100101_012")

                'SQL組み立て
                Dim sql As New StringBuilder
                With sql
                    .Append(" UPDATE /* SC3100101_012 */")
                    .Append("        TBL_VISIT_SALES")
                    If newSalesTableNo = SalesTableNull Then
                        .Append("    SET SALESTABLENO = NULL")
                    Else
                        .Append("    SET SALESTABLENO = :SALESTABLENO_NEW")
                    End If
                    .Append("      , UPDATEDATE = SYSDATE")
                    .Append("      , UPDATEACCOUNT = :UPDATEACCOUNT")
                    .Append("      , UPDATEID = :UPDATEID")
                    .Append("  WHERE VISITSEQ = :VISITSEQ")
                    If oldSalesTableNo = SalesTableNull Then
                        .Append("    AND SALESTABLENO IS NULL")
                    Else
                        .Append("    AND SALESTABLENO = :SALESTABLENO_OLD")
                    End If
                End With

                query.CommandText = sql.ToString()

                'SQLパラメータ設定
                If Not newSalesTableNo = SalesTableNull Then
                    query.AddParameterWithTypeValue("SALESTABLENO_NEW", OracleDbType.Char, newSalesTableNo)
                End If
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Char, updateAccount)
                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Char, UpdateId)
                query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Long, visitSequence)
                If Not oldSalesTableNo = SalesTableNull Then
                    query.AddParameterWithTypeValue("SALESTABLENO_OLD", OracleDbType.Char, oldSalesTableNo)
                End If

                'SQL実行（結果を返却）
                If query.Execute() > 0 Then
                    Return True
                Else
                    Return False
                End If
            End Using
        End Function
#End Region

#Region "対応依頼通知追加"

        ''' <summary>
        ''' 対応依頼通知追加
        ''' </summary>
        ''' <param name="visitSequence">来店実績連番</param>
        ''' <param name="requestAccount">依頼アカウント</param>
        ''' <param name="updateAccount">更新アカウント</param>
        ''' <returns>処理結果</returns>
        ''' <remarks></remarks>
        Public Function InsertRequestNotice(ByVal visitSequence As Long, _
                                            ByVal requestAccount As String, _
                                            ByVal updateAccount As String) As Boolean
            'DbUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("SC3100101_013")

                'SQL組み立て
                Dim sql As New StringBuilder
                With sql
                    .Append(" INSERT /* SC3100101_013 */")
                    .Append("   INTO TBL_VISITDEAL_NOTICE (")
                    .Append("        VISITSEQ")
                    .Append("      , ACCOUNT")
                    .Append("      , DELFLG")
                    .Append("      , CREATEDATE")
                    .Append("      , UPDATEDATE")
                    .Append("      , CREATEACCOUNT")
                    .Append("      , UPDATEACCOUNT")
                    .Append("      , CREATEID")
                    .Append("      , UPDATEID")
                    .Append(" )")
                    .Append(" VALUES (")
                    .Append("        :VISITSEQ")
                    .Append("      , :ACCOUNT")
                    .Append("      , :DELFLG")
                    .Append("      , SYSDATE")
                    .Append("      , SYSDATE")
                    .Append("      , :CREATEACCOUNT")
                    .Append("      , :UPDATEACCOUNT")
                    .Append("      , :CREATEID")
                    .Append("      , :UPDATEID")
                    .Append(" )")
                End With

                query.CommandText = sql.ToString()

                'SQLパラメータ設定
                query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Long, visitSequence)
                query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.Char, requestAccount)
                query.AddParameterWithTypeValue("DELFLG", OracleDbType.Char, DeleteFlagNotDelete)
                query.AddParameterWithTypeValue("CREATEACCOUNT", OracleDbType.Char, updateAccount)
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Char, updateAccount)
                query.AddParameterWithTypeValue("CREATEID", OracleDbType.Char, UpdateId)
                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Char, UpdateId)

                'SQL実行（結果を返却）
                If query.Execute() > 0 Then
                    Return True
                Else
                    Return False
                End If
            End Using
        End Function
#End Region

#Region "来店実績ブロードキャスト更新"

        ''' <summary>
        ''' 来店実績ブロードキャスト更新
        ''' </summary>
        ''' <param name="visitSequence">来店実績連番</param>
        ''' <param name="updateAccount">更新アカウント</param>
        ''' <returns>処理結果</returns>
        ''' <remarks></remarks>
        Public Function UpdateBroadcast(ByVal visitSequence As Long, _
                                        ByVal updateAccount As String) As Boolean
            'DbUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("SC3100101_014")

                'SQL組み立て
                Dim sql As New StringBuilder
                With sql
                    .Append(" UPDATE /* SC3100101_014 */")
                    .Append("        TBL_VISIT_SALES")
                    .Append("    SET VISITSTATUS = :VISITSTATUS")
                    .Append("      , BROUDCASTFLG = :BROUDCASTFLG_ON")
                    ' $02 start 新車タブレットショールーム管理機能開発                 
                    .Append("      , SC_ASSIGNDATE = CASE WHEN VISITSTATUS = :VISITSTATUS10 THEN SYSDATE")
                    .Append("                     　      WHEN VISITSTATUS = :VISITSTATUS1  THEN NULL")
                    .Append("                             ELSE SC_ASSIGNDATE")
                    .Append("                        END")
                    ' $02 end 新車タブレットショールーム管理機能開発
                    .Append("      , UPDATEDATE = SYSDATE")
                    .Append("      , UPDATEACCOUNT = :UPDATEACCOUNT")
                    .Append("      , UPDATEID = :UPDATEID")
                    .Append("  WHERE VISITSEQ = :VISITSEQ")
                    .Append("    AND CUSTSEGMENT IS NULL")
                    .Append("    AND BROUDCASTFLG = :BROUDCASTFLG_OFF")
                    .Append("    AND VISITSTATUS IN (:VISITSTATUS1, :VISITSTATUS2, :VISITSTATUS3,")
                    ' $02 start 新車タブレットショールーム管理機能開発
                    .Append("                        :VISITSTATUS4, :VISITSTATUS5, :VISITSTATUS6, :VISITSTATUS10)")
                    ' $02 end 新車タブレットショールーム管理機能開発
                End With

                query.CommandText = sql.ToString()

                'SQLパラメータ設定
                query.AddParameterWithTypeValue("VISITSTATUS", OracleDbType.Char, VisitStatusFreeBroadcast)
                query.AddParameterWithTypeValue("BROUDCASTFLG_ON", OracleDbType.Char, BroadcastFlagOn)
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Char, updateAccount)
                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Char, UpdateId)
                query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Long, visitSequence)
                query.AddParameterWithTypeValue("BROUDCASTFLG_OFF", OracleDbType.Char, BroadcastFlagOff)
                query.AddParameterWithTypeValue("VISITSTATUS1", OracleDbType.Char, VisitStatusFree)
                query.AddParameterWithTypeValue("VISITSTATUS2", OracleDbType.Char, VisitStatusFreeBroadcast)
                query.AddParameterWithTypeValue("VISITSTATUS3", OracleDbType.Char, VisitStatusAdjustment)
                query.AddParameterWithTypeValue("VISITSTATUS4", OracleDbType.Char, VisitStatusDecisionBroadcast)
                query.AddParameterWithTypeValue("VISITSTATUS5", OracleDbType.Char, VisitStatusDecision)
                query.AddParameterWithTypeValue("VISITSTATUS6", OracleDbType.Char, VisitStatusWating)
                ' $02 start 新車タブレットショールーム管理機能開発
                query.AddParameterWithTypeValue("VISITSTATUS10", OracleDbType.Char, VisitStatusUnnecessary)
                ' $02 end 新車タブレットショールーム管理機能開発

                'SQL実行（結果を返却）
                If query.Execute() > 0 Then
                    Return True
                Else
                    Return False
                End If
            End Using
        End Function
#End Region

#Region "対応依頼通知削除"

        ''' <summary>
        ''' 対応依頼通知削除
        ''' </summary>
        ''' <param name="visitSequence">来店実績連番</param>
        ''' <param name="updateAccount">更新アカウント</param>
        ''' <remarks></remarks>
        Public Sub DeleteRequestNotice(ByVal visitSequence As Long, _
                                            ByVal updateAccount As String)
            'DbUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("SC3100101_015")

                'SQL組み立て
                Dim sql As New StringBuilder
                With sql
                    .Append(" UPDATE /* SC3100101_015 */")
                    .Append("        TBL_VISITDEAL_NOTICE")
                    .Append("    SET DELFLG = :DELFLG")
                    .Append("      , UPDATEDATE = SYSDATE")
                    .Append("      , UPDATEACCOUNT = :UPDATEACCOUNT")
                    .Append("      , UPDATEID = :UPDATEID")
                    .Append("  WHERE VISITSEQ = :VISITSEQ")
                End With

                query.CommandText = sql.ToString()

                'SQLパラメータ設定
                query.AddParameterWithTypeValue("DELFLG", OracleDbType.Char, DeleteFlagDelete)
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Char, updateAccount)
                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Char, UpdateId)
                query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Long, visitSequence)

                'SQL実行（結果を返却）
                query.Execute()
            End Using
        End Sub
#End Region

#Region "対応担当スタッフコード更新"

        ''' <summary>
        ''' 対応担当スタッフコード更新
        ''' </summary>
        ''' <param name="visitSequence">来店実績連番</param>
        ''' <param name="dealAccount">対応アカウント</param>
        ''' <param name="updateAccount">更新アカウント</param>
        ''' <returns>処理結果</returns>
        ''' <remarks></remarks>
        Public Function UpdateDealStaffCode(ByVal visitSequence As Long, _
                                            ByVal dealAccount As String, _
                                            ByVal updateAccount As String) As Boolean
            'DbUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("SC3100101_016")

                'SQL組み立て
                Dim sql As New StringBuilder
                With sql
                    .Append(" UPDATE /* SC3100101_016 */")
                    .Append("        TBL_VISIT_SALES")
                    .Append("    SET ACCOUNT = :ACCOUNT")
                    .Append("      , VISITSTATUS = :VISITSTATUS")
                    .Append("      , UPDATEDATE = SYSDATE")
                    .Append("      , UPDATEACCOUNT = :UPDATEACCOUNT")
                    ' $02 start 新車タブレットショールーム管理機能開発                 
                    .Append("      , SC_ASSIGNDATE = CASE WHEN VISITSTATUS = :VISITSTATUS10 THEN SYSDATE")
                    .Append("                     　      WHEN VISITSTATUS = :VISITSTATUS1  THEN NULL")
                    .Append("                             ELSE SC_ASSIGNDATE")
                    .Append("                        END")
                    ' $02 end 新車タブレットショールーム管理機能開発
                    .Append("      , UPDATEID = :UPDATEID")
                    .Append("  WHERE VISITSEQ = :VISITSEQ")
                    .Append("    AND VISITSTATUS IN (:VISITSTATUS1, :VISITSTATUS2, :VISITSTATUS3,")
                    ' $01 start 複数顧客に対する商談平行対応
                    .Append("                        :VISITSTATUS4, :VISITSTATUS5, :VISITSTATUS6,")
                    ' $02 start 新車タブレットショールーム管理機能開発      
                    .Append("                        :VISITSTATUS9, :VISITSTATUS10)")
                    ' $02 end 新車タブレットショールーム管理機能開発
                    ' $01 end   複数顧客に対する商談平行対応
                    .Append("    AND EXISTS (")
                    .Append("     SELECT 1")
                    .Append("       FROM TBL_USERS")
                    .Append("      WHERE ACCOUNT = :ACCOUNT")
                    .Append("        AND PRESENCECATEGORY IN (:STAFFSTATUS1, :STAFFSTATUS2,")
                    .Append("                                 :STAFFSTATUS3)")
                    .Append("        )")
                End With

                query.CommandText = sql.ToString()

                'SQLパラメータ設定
                query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.Char, dealAccount)
                query.AddParameterWithTypeValue("VISITSTATUS", OracleDbType.Char, VisitStatusAdjustment)
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Char, updateAccount)
                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Char, UpdateId)
                query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Long, visitSequence)
                query.AddParameterWithTypeValue("VISITSTATUS1", OracleDbType.Char, VisitStatusFree)
                query.AddParameterWithTypeValue("VISITSTATUS2", OracleDbType.Char, VisitStatusFreeBroadcast)
                query.AddParameterWithTypeValue("VISITSTATUS3", OracleDbType.Char, VisitStatusAdjustment)
                query.AddParameterWithTypeValue("VISITSTATUS4", OracleDbType.Char, VisitStatusDecisionBroadcast)
                query.AddParameterWithTypeValue("VISITSTATUS5", OracleDbType.Char, VisitStatusDecision)
                query.AddParameterWithTypeValue("VISITSTATUS6", OracleDbType.Char, VisitStatusWating)
                ' $01 start 複数顧客に対する商談平行対応
                query.AddParameterWithTypeValue("VISITSTATUS9", OracleDbType.Char, VisitStatusNegotiateStop)
                ' $01 end   複数顧客に対する商談平行対応
                ' $02 start 新車タブレットショールーム管理機能開発      
                query.AddParameterWithTypeValue("VISITSTATUS10", OracleDbType.Char, VisitStatusUnnecessary)
                ' $02 end 新車タブレットショールーム管理機能開発
                query.AddParameterWithTypeValue("STAFFSTATUS1", OracleDbType.Char, StaffStatusStandby)
                query.AddParameterWithTypeValue("STAFFSTATUS2", OracleDbType.Char, StaffStatusNegotiate)
                query.AddParameterWithTypeValue("STAFFSTATUS3", OracleDbType.Char, StaffStatusLeaving)
                'SQL実行（結果を返却）
                If query.Execute() > 0 Then
                    Return True
                Else
                    Return False
                End If
            End Using
        End Function
#End Region

#Region "スタッフ情報（スタンバイ）の取得"

        ''' <summary>
        ''' スタッフ情報（スタンバイ）の取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="storeCode">店舗コード</param>
        ''' <returns>スタッフ情報（スタンバイ）データテーブル</returns>
        ''' <remarks></remarks>
        Public Function GetStaffStandby(ByVal dealerCode As String, ByVal storeCode As String) _
                                          As SC3100101DataSet.SC3100101StandbyStaffDataTable
            'SQL組み立て
            Dim sql As New StringBuilder
            With sql
                .Append(" SELECT /* SC3100101_017 */")
                .Append("        US.ACCOUNT AS ACCOUNT")
                .Append("   FROM TBL_USERS US")
                .Append("  WHERE US.DLRCD = :DLRCD")
                .Append("    AND US.STRCD = :STRCD")
                .Append("    AND US.PRESENCECATEGORY = :STAFFSTATUS")
                .Append("    AND US.OPERATIONCODE = :OPERATIONCODE")
                .Append("    AND US.DELFLG = :DELFLG")
            End With

            'DbSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3100101DataSet.SC3100101StandbyStaffDataTable)("SC3100101_0017")
                query.CommandText = sql.ToString()

                'SQLパラメータ設定
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, storeCode)
                query.AddParameterWithTypeValue("STAFFSTATUS", OracleDbType.Char, StaffStatusStandby)
                query.AddParameterWithTypeValue("DELFLG", OracleDbType.Char, DeleteFlagNotDelete)
                query.AddParameterWithTypeValue("OPERATIONCODE", OracleDbType.Long, OperationCodeSalesStaff)

                'SQL実行（結果表を返却）
                Return query.GetData()
            End Using
        End Function
#End Region

        ' $01 start 複数顧客に対する商談平行対応
#Region "紐付け解除情報の取得"

        ' ''' <summary>
        ' ''' 紐付け解除情報の取得
        ' ''' </summary>
        ' ''' <param name="dealerCode">販売店コード</param>
        ' ''' <param name="storeCode">店舗コード</param>
        ' ''' <param name="staffCode">スタッフコード</param>
        ' ''' <param name="startDate">開始日時</param>
        ' ''' <param name="endDate">終了日時</param>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Function GetLinkingCancel(ByVal dealerCode As String, ByVal storeCode As String, _
        '                                 ByVal staffCode As String, ByVal startDate As Date, _
        '                                 ByVal endDate As Date) As SC3100101DataSet.SC3100101LinkingCancelDataTable
        '    'SQL組み立て
        '    Dim sql As New StringBuilder
        '    With sql
        '        .Append(" SELECT /* SC3100101_018 */")
        '        .Append("        VS.VISITSEQ")
        '        .Append("      , CASE VS.CUSTSEGMENT")
        '        .Append("            WHEN :CUSTSEGMENT_JI THEN CU.NAME")
        '        .Append("            WHEN :CUSTSEGMENT_MI THEN NC.NAME ")
        '        .Append("            ELSE VS.TENTATIVENAME ")
        '        .Append("        END AS CUSTNAME")
        '        .Append("      , CASE VS.CUSTSEGMENT")
        '        .Append("            WHEN :CUSTSEGMENT_JI THEN CU.NAMETITLE")
        '        .Append("            WHEN :CUSTSEGMENT_MI THEN NC.NAMETITLE")
        '        .Append("            ELSE NULL ")
        '        .Append("        END AS CUSTNAMETITLE")
        '        .Append("      , VS.VISITTIMESTAMP")
        '        .Append("      , VS.VISITSTATUS")
        '        .Append("      , VS.CUSTSEGMENT")
        '        .Append("   FROM TBL_VISIT_SALES VS")
        '        .Append("      , TBL_USERS US")
        '        .Append("      , TBLORG_CUSTOMER CU")
        '        .Append("      , TBL_NEWCUSTOMER NC")
        '        .Append("  WHERE VS.CUSTID = NC.CSTID(+)")
        '        .Append("    AND VS.CUSTID = CU.ORIGINALID(+)")
        '        .Append("    AND VS.ACCOUNT = US.ACCOUNT ")
        '        .Append("    AND VS.DLRCD = :DLRCD")
        '        .Append("    AND VS.STRCD = :STRCD")
        '        .Append("    AND VS.ACCOUNT = :ACCOUNT")
        '        .Append("    AND VS.VISITTIMESTAMP BETWEEN :STARTTIME AND :ENDTIME")
        '        .Append("    AND VS.VISITSTATUS IN (:VISITSTATUS3, :VISITSTATUS4, :VISITSTATUS5, :VISITSTATUS6)")
        '        .Append("    AND US.DELFLG = :DELFLG")
        '        .Append("    AND US.OPERATIONCODE = :OPERATIONCODE")
        '        .Append("    AND CU.DELFLG(+) = :DELFLG")
        '        .Append("    AND NC.DELFLG(+) = :DELFLG")
        '        .Append("  ORDER BY VS.VISITTIMESTAMP ASC, CUSTNAME ASC")
        '    End With

        '    'DbSelectQueryインスタンス生成
        '    Using query As New DBSelectQuery(Of SC3100101DataSet.SC3100101LinkingCancelDataTable)("SC3100101_018")
        '        query.CommandText = sql.ToString()

        '        'SQLパラメータ設定
        '        query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
        '        query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, storeCode)
        '        query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.Char, staffCode)
        '        query.AddParameterWithTypeValue("STARTTIME", OracleDbType.Date, startDate)
        '        query.AddParameterWithTypeValue("ENDTIME", OracleDbType.Date, endDate)

        '        query.AddParameterWithTypeValue("CUSTSEGMENT_JI", OracleDbType.Char, VisitorSegmentOriginal)
        '        query.AddParameterWithTypeValue("CUSTSEGMENT_MI", OracleDbType.Char, VisitorSegmentNew)
        '        query.AddParameterWithTypeValue("VISITSTATUS3", OracleDbType.Char, VisitStatusAdjustment)
        '        query.AddParameterWithTypeValue("VISITSTATUS4", OracleDbType.Char, VisitStatusDecisionBroadcast)
        '        query.AddParameterWithTypeValue("VISITSTATUS5", OracleDbType.Char, VisitStatusDecision)
        '        query.AddParameterWithTypeValue("VISITSTATUS6", OracleDbType.Char, VisitStatusWating)
        '        query.AddParameterWithTypeValue("DELFLG", OracleDbType.Char, DeleteFlagNotDelete)
        '        query.AddParameterWithTypeValue("OPERATIONCODE", OracleDbType.Long, OperationCodeSalesStaff)

        '        'SQL実行（結果表を返却）
        '        Return query.GetData()
        '    End Using
        'End Function

#End Region

#Region "紐付け解除更新"

        ' ''' <summary>
        ' ''' 紐付け解除更新
        ' ''' </summary>
        ' ''' <param name="visitSequence">来店実績連番</param>
        ' ''' <param name="dealAccount">対応アカウント</param>
        ' ''' <param name="updateAccount">更新アカウント</param>
        ' ''' <returns>処理結果</returns>
        ' ''' <remarks></remarks>
        'Public Function UpdateLinkingCancel(ByVal visitSequence As Long, _
        '                                    ByVal dealAccount As String, _
        '                                    ByVal updateAccount As String) As Boolean

        '    'DbUpdateQueryインスタンス生成
        '    Using query As New DBUpdateQuery("SC3100101_019")

        '        'SQL組み立て
        '        Dim sql As New StringBuilder
        '        With sql
        '            .Append(" UPDATE /* SC3100101_019 */")
        '            .Append("        TBL_VISIT_SALES")
        '            .Append("    SET ACCOUNT = NULL")
        '            .Append("      , VISITSTATUS = :VISITSTATUS1")
        '            .Append("      , UPDATEDATE = SYSDATE")
        '            .Append("      , UPDATEACCOUNT = :UPDATEACCOUNT")
        '            .Append("      , UPDATEID = :UPDATEID")
        '            .Append("  WHERE VISITSEQ = :VISITSEQ")
        '            .Append("    AND ACCOUNT = :ACCOUNT")
        '            .Append("    AND VISITSTATUS IN (:VISITSTATUS3, :VISITSTATUS4")
        '            .Append("                      , :VISITSTATUS5, :VISITSTATUS6)")
        '        End With

        '        query.CommandText = sql.ToString()

        '        'SQLパラメータ設定
        '        query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Long, visitSequence)
        '        query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.Char, dealAccount)
        '        query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Char, updateAccount)

        '        query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Char, UpdateId)
        '        query.AddParameterWithTypeValue("VISITSTATUS1", OracleDbType.Char, VisitStatusFree)
        '        query.AddParameterWithTypeValue("VISITSTATUS3", OracleDbType.Char, VisitStatusAdjustment)
        '        query.AddParameterWithTypeValue("VISITSTATUS4", OracleDbType.Char, VisitStatusDecisionBroadcast)
        '        query.AddParameterWithTypeValue("VISITSTATUS5", OracleDbType.Char, VisitStatusDecision)
        '        query.AddParameterWithTypeValue("VISITSTATUS6", OracleDbType.Char, VisitStatusWating)

        '        'SQL実行（結果を返却）
        '        If query.Execute() > 0 Then
        '            Return True
        '        Else
        '            Return False
        '        End If

        '    End Using
        'End Function

#End Region
        ' $01 end   複数顧客に対する商談平行対応

        ' $02 start 受付共通からの移植
#Region "店舗苦情情報の取得"

        ''' <summary>
        ''' 店舗苦情情報の取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="storeCode">店舗コード</param>
        ''' <param name="startDate">取得開始日時</param>
        ''' <param name="endDate">取得終了日時</param>
        ''' <param name="completeDate">完了表示日時</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetClaimInfo(ByVal dealerCode As String, ByVal storeCode As String, _
                                     ByVal startDate As Date, ByVal endDate As Date, _
                                     ByVal completeDate As Date) As SC3100101DataSet.VisitReceptionClaimInfoDataTable

            'SQL組み立て
            Dim sql As New StringBuilder
            With sql
                ' $05 start
                .Append(" SELECT /* SC3100101_020 */ ")
                .Append("    DISTINCT VISITSEQ ")
                .Append(" FROM  ")
                .Append(" ( ")
                .Append("     SELECT ")
                .Append("        VS.VISITSEQ ")
                .Append("     FROM  ")
                .Append("          TBL_VISIT_SALES VS ")
                .Append("        , TB_T_REQUEST REQ ")
                .Append("        , TB_T_COMPLAINT COM ")
                .Append("        , TB_T_COMPLAINT_DETAIL COM_DL ")
                .Append("     WHERE  ")
                .Append("            TO_NUMBER(TRIM(VS.CUSTID)) = REQ.CST_ID ")
                .Append("        AND REQ.REQ_ID = COM.REQ_ID  ")
                .Append("        AND COM.CMPL_ID = COM_DL.CMPL_ID ")
                .Append("        AND VS.DLRCD = :DLRCD ")
                .Append("        AND VS.STRCD = :STRCD ")
                .Append("        AND VISITSTATUS IN (:VISITSTATUS1, :VISITSTATUS2, :VISITSTATUS3, :VISITSTATUS4, ")
                .Append("                            :VISITSTATUS5, :VISITSTATUS6, :VISITSTATUS7, :VISITSTATUS9, :VISITSTATUS11) ")
                .Append("        AND NVL(VS.STOPTIME, NVL(VS.VISITTIMESTAMP, VS.SALESSTART)) BETWEEN :STARTTIME AND :ENDTIME ")
                .Append("        AND COM.RELATION_TYPE IN (:RELATIONFLG1, :RELATIONFLG2) ")
                .Append("        AND COM_DL.CMPL_DETAIL_ID = ( ")
                .Append("                SELECT ")
                .Append("                    MAX(COM_DLM.CMPL_DETAIL_ID) ")
                .Append("                  FROM TB_T_COMPLAINT_DETAIL COM_DLM ")
                .Append("                 WHERE COM.CMPL_ID = COM_DLM.CMPL_ID ")
                .Append("            ) ")
                .Append("        AND ( ")
                .Append("                COM.CMPL_STATUS IN (:CLAIMSTATUS1, :CLAIMSTATUS2) ")
                .Append("                OR ( ")
                .Append("                        COM.CMPL_STATUS = :CLAIMSTATUS3 ")
                .Append("                    AND COM_DL.FIRST_LAST_ACT_TYPE = :CLAIMSTATUS2 ")
                .Append("                    AND COM_DL.ACT_DATETIME >= :ACTUALDATE ")
                .Append("                   ) ")
                .Append("            ) ")
                .Append("  ")
                .Append("     UNION ALL  ")
                .Append("  ")
                .Append("     SELECT ")
                .Append("        VS.VISITSEQ ")
                .Append("     FROM  ")
                .Append("          TBL_VISIT_SALES VS ")
                .Append("        , TB_H_REQUEST REQ ")
                .Append("        , TB_H_COMPLAINT COM ")
                .Append("        , TB_H_COMPLAINT_DETAIL COM_DL ")
                .Append("     WHERE  ")
                .Append("            TO_NUMBER(TRIM(VS.CUSTID)) = REQ.CST_ID ")
                .Append("        AND REQ.REQ_ID = COM.REQ_ID  ")
                .Append("        AND COM.CMPL_ID = COM_DL.CMPL_ID ")
                .Append("        AND VS.DLRCD = :DLRCD ")
                .Append("        AND VS.STRCD = :STRCD ")
                .Append("        AND VISITSTATUS IN (:VISITSTATUS1, :VISITSTATUS2, :VISITSTATUS3, :VISITSTATUS4, ")
                .Append("                            :VISITSTATUS5, :VISITSTATUS6, :VISITSTATUS7, :VISITSTATUS9, :VISITSTATUS11) ")
                .Append("        AND NVL(VS.STOPTIME, NVL(VS.VISITTIMESTAMP, VS.SALESSTART)) BETWEEN :STARTTIME AND :ENDTIME ")
                .Append("        AND COM.RELATION_TYPE IN (:RELATIONFLG1, :RELATIONFLG2) ")
                .Append("        AND COM_DL.CMPL_DETAIL_ID = ( ")
                .Append("                SELECT ")
                .Append("                    MAX(COM_DLM.CMPL_DETAIL_ID) ")
                .Append("                  FROM TB_H_COMPLAINT_DETAIL COM_DLM ")
                .Append("                 WHERE COM.CMPL_ID = COM_DLM.CMPL_ID ")
                .Append("            ) ")
                .Append("        AND ( ")
                .Append("                COM.CMPL_STATUS IN (:CLAIMSTATUS1, :CLAIMSTATUS2) ")
                .Append("                OR ( ")
                .Append("                        COM.CMPL_STATUS = :CLAIMSTATUS3 ")
                .Append("                    AND COM_DL.FIRST_LAST_ACT_TYPE = :CLAIMSTATUS2 ")
                .Append("                    AND COM_DL.ACT_DATETIME >= :ACTUALDATE ")
                .Append("                   ) ")
                .Append("            ) ")
                .Append(" ) ")
                ' $05 end
            End With

            'DbSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3100101DataSet.VisitReceptionClaimInfoDataTable)("SC3100101_20")
                query.CommandText = sql.ToString()

                'SQLパラメータ設定
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, storeCode)
                query.AddParameterWithTypeValue("STARTTIME", OracleDbType.Date, startDate)
                query.AddParameterWithTypeValue("ENDTIME", OracleDbType.Date, endDate)
                query.AddParameterWithTypeValue("ACTUALDATE", OracleDbType.Date, completeDate)

                query.AddParameterWithTypeValue("VISITSTATUS1", OracleDbType.Char, VisitStatusFree)
                query.AddParameterWithTypeValue("VISITSTATUS2", OracleDbType.Char, VisitStatusFreeBroadcast)
                query.AddParameterWithTypeValue("VISITSTATUS3", OracleDbType.Char, VisitStatusAdjustment)
                query.AddParameterWithTypeValue("VISITSTATUS4", OracleDbType.Char, VisitStatusDecisionBroadcast)
                query.AddParameterWithTypeValue("VISITSTATUS5", OracleDbType.Char, VisitStatusDecision)
                query.AddParameterWithTypeValue("VISITSTATUS6", OracleDbType.Char, VisitStatusWating)
                query.AddParameterWithTypeValue("VISITSTATUS7", OracleDbType.Char, VisitStatusNegotiate)
                query.AddParameterWithTypeValue("VISITSTATUS9", OracleDbType.Char, VisitStatusNegotiateStop)
                ' $02 start 新車タブレットショールーム管理機能開発
                query.AddParameterWithTypeValue("VISITSTATUS11", OracleDbType.Char, VisitStatusDeliverlyStart)
                ' $02 end   新車タブレットショールーム管理機能開発
                query.AddParameterWithTypeValue("CLAIMSTATUS1", OracleDbType.NVarchar2, ClaimStatusFirst)
                query.AddParameterWithTypeValue("CLAIMSTATUS2", OracleDbType.NVarchar2, ClaimStatusLast)
                query.AddParameterWithTypeValue("CLAIMSTATUS3", OracleDbType.NVarchar2, ClaimStatusComplete)

                query.AddParameterWithTypeValue("RELATIONFLG1", OracleDbType.NVarchar2, RelationFlgOff)
                query.AddParameterWithTypeValue("RELATIONFLG2", OracleDbType.NVarchar2, RelationFlgOn)

                'SQL実行（結果表を返却）
                Return query.GetData()
            End Using

        End Function
#End Region

#Region "お客様との紐付け人数の取得"
        ''' <summary>
        ''' お客様との紐付け人数の取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="storeCode">店舗コード</param>
        ''' <param name="startTime">取得開始日時</param>
        ''' <param name="endTime">取得終了日時</param>
        ''' <returns>お客様との紐付け人数データテーブル</returns>
        ''' <remarks></remarks>
        Public Function GetVisitorLinkingCount(ByVal dealerCode As String, ByVal storeCode As String, _
                                               ByVal startTime As Date, ByVal endTime As Date) _
                                                   As SC3100101DataSet.VisitReceptionVisitorLinkingCountDataTable
            'SQL組み立て
            Dim sql As New StringBuilder
            With sql
                .Append(" SELECT /* SC3100101_021 */")
                .Append("        VS.ACCOUNT")
                .Append("      , COUNT(1) AS VISITORLINKINGCOUNT")
                .Append("   FROM TBL_VISIT_SALES VS")
                .Append("      , TBL_USERS US")
                .Append("  WHERE VS.ACCOUNT = US.ACCOUNT")
                .Append("    AND VS.DLRCD = :DLRCD")
                .Append("    AND VS.STRCD = :STRCD")
                .Append("    AND NVL(VS.STOPTIME, VS.VISITTIMESTAMP) BETWEEN :STARTTIME")
                .Append("                                                AND :ENDTIME")
                .Append("    AND VS.VISITSTATUS IN (:VISITSTATUS3, :VISITSTATUS4")
                .Append("                         , :VISITSTATUS5, :VISITSTATUS6")
                .Append("                         , :VISITSTATUS9)")
                .Append("    AND US.OPERATIONCODE = :OPERATIONCODE")
                .Append("    AND US.DELFLG = :DELFLG")
                .Append("  GROUP BY VS.ACCOUNT")
            End With

            'DbSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3100101DataSet.VisitReceptionVisitorLinkingCountDataTable)("SC3100101_021")
                query.CommandText = sql.ToString()

                'SQLパラメータ設定
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, storeCode)
                query.AddParameterWithTypeValue("STARTTIME", OracleDbType.Date, startTime)
                query.AddParameterWithTypeValue("ENDTIME", OracleDbType.Date, endTime)

                query.AddParameterWithTypeValue("VISITSTATUS3", OracleDbType.Char, VisitStatusAdjustment)
                query.AddParameterWithTypeValue("VISITSTATUS4", OracleDbType.Char, VisitStatusDecisionBroadcast)
                query.AddParameterWithTypeValue("VISITSTATUS5", OracleDbType.Char, VisitStatusDecision)
                query.AddParameterWithTypeValue("VISITSTATUS6", OracleDbType.Char, VisitStatusWating)
                ' $01 start 複数顧客に対する商談平行対応
                query.AddParameterWithTypeValue("VISITSTATUS9", OracleDbType.Char, VisitStatusNegotiateStop)
                ' $01 end   複数顧客に対する商談平行対応

                query.AddParameterWithTypeValue("OPERATIONCODE", OracleDbType.Long, OperationCodeSalesStaff)
                query.AddParameterWithTypeValue("DELFLG", OracleDbType.Char, DeleteFlagNotDelete)

                'SQL実行（結果表を返却）
                Return query.GetData()
            End Using
        End Function
#End Region

#Region "通知依頼情報の取得"

        ''' <summary>
        ''' 通知依頼情報の取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="storeCode">店舗コード</param>
        ''' <param name="startTime">取得開始日時</param>
        ''' <param name="endTime">取得終了日時</param>
        ''' <param name="noticeRequestCategory">通知依頼種別</param>
        ''' <param name="lastStatus">ステータス</param>
        ''' <returns>通知依頼データテーブル</returns>
        ''' <remarks></remarks>
        Public Function GetNoticeRequests(ByVal dealerCode As String, ByVal storeCode As String, _
                                          ByVal startTime As Date, ByVal endTime As Date, _
                                          ByVal noticeRequestCategory As String, ByVal lastStatus As List(Of String)) _
                                              As SC3100101DataSet.VisitReceptionNoticeRequestsDataTable
            'SQL組み立て
            Dim sqlLastStatus As New StringBuilder
            Dim isFirst As Boolean = True
            For Each status As String In lastStatus

                If isFirst Then
                    isFirst = False
                Else
                    sqlLastStatus.Append(", ")
                End If

                sqlLastStatus.Append("'")
                sqlLastStatus.Append(status)
                sqlLastStatus.Append("'")
            Next

            Dim sql As New StringBuilder
            With sql
                .Append(" SELECT /* SC3100101_023 */")
                .Append("        VS.ACCOUNT")
                .Append("      , NR.NOTICEREQCTG")
                .Append("      , VS.CUSTID")
                .Append("      , MIN(NI.SENDDATE) AS SENDDATE")
                .Append("   FROM TBL_VISIT_SALES VS")
                .Append("      , TBL_USERS US")
                .Append("      , TBL_NOTICEREQUEST NR")
                .Append("      , TBL_NOTICEINFO NI")
                .Append("  WHERE VS.ACCOUNT = US.ACCOUNT")
                .Append("    AND VS.DLRCD = NR.DLRCD")
                .Append("    AND VS.STRCD = NR.STRCD")
                .Append("    AND VS.ACCOUNT = NI.FROMACCOUNT")
                .Append("    AND (VS.ACCOUNT <> NI.TOACCOUNT")
                .Append("     OR NI.TOACCOUNT IS NULL)")
                .Append("    AND VS.CUSTID = NR.CRCUSTID")
                .Append("    AND VS.CUSTSEGMENT = NR.CSTKIND")
                .Append("    AND NR.NOTICEREQID = NI.NOTICEREQID")
                .Append("    AND VS.DLRCD = :DLRCD")
                .Append("    AND VS.STRCD = :STRCD")
                ' $01 start 複数顧客に対する商談平行対応
                .Append("    AND VS.FIRST_SALESSTART BETWEEN :STARTTIME")
                .Append("                                AND :ENDTIME")
                .Append("    AND VS.FIRST_SALESSTART <= NI.SENDDATE")
                ' $01 end   複数顧客に対する商談平行対応
                ' $03 start 納車作業ステータス対応
                .Append("    AND VS.VISITSTATUS IN (:VISITSTATUS7, :VISITSTATUS11)")
                ' $03 end   納車作業ステータス対応
                .Append("    AND US.DELFLG = :DELFLG")
                .Append("    AND US.OPERATIONCODE = :OPERATIONCODE")
                .Append("    AND NR.NOTICEREQCTG = :NOTICEREQCTG")
                .Append("    AND NR.CUSTOMERCLASS = :CUSTOMERCLASS")
                .Append("    AND NR.STATUS IN (")
                .Append(sqlLastStatus)
                .Append("                     )")
                .Append("    AND NI.STATUS = :STATUSREQUEST")
                .Append("  GROUP BY VS.ACCOUNT, NR.NOTICEREQCTG, VS.CUSTID")
            End With

            'DbSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3100101DataSet.VisitReceptionNoticeRequestsDataTable)("SC3100101_023")
                query.CommandText = sql.ToString()

                'SQLパラメータ設定
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, storeCode)
                query.AddParameterWithTypeValue("STARTTIME", OracleDbType.Date, startTime)
                query.AddParameterWithTypeValue("ENDTIME", OracleDbType.Date, endTime)
                query.AddParameterWithTypeValue("NOTICEREQCTG", OracleDbType.Char, noticeRequestCategory)
                query.AddParameterWithTypeValue("VISITSTATUS7", OracleDbType.Char, VisitStatusNegotiate)
                ' $03 start 納車作業ステータス対応
                query.AddParameterWithTypeValue("VISITSTATUS11", OracleDbType.Char, VisitStatusDeliverlyStart)
                ' $03 end   納車作業ステータス対応
                query.AddParameterWithTypeValue("DELFLG", OracleDbType.Char, DeleteFlagNotDelete)
                query.AddParameterWithTypeValue("OPERATIONCODE", OracleDbType.Long, OperationCodeSalesStaff)
                query.AddParameterWithTypeValue("CUSTOMERCLASS", OracleDbType.Char, CustomerClassOwner)
                query.AddParameterWithTypeValue("STATUSREQUEST", OracleDbType.Char, LastStatusAssessment)

                'SQL実行（結果表を返却）
                Return query.GetData()
            End Using
        End Function
#End Region

#Region "来店実績お客様情報の取得"

        ''' <summary>
        ''' 来店実績お客様情報の取得
        ''' </summary>
        ''' <param name="visitSequence">来店実績連番</param>
        ''' <param name="visitStatus">来店実績ステータス</param>
        ''' <returns>来店実績お客様情報データテーブル</returns>
        ''' <remarks></remarks>
        Public Function GetVisitorCustomer(ByVal visitSequence As Long, _
                                           Optional ByVal visitStatus As String = Nothing) _
                                           As SC3100101DataSet.VisitReceptionVisitorCustomerDataTable
            'SQL組み立て
            Dim sql As New StringBuilder
            With sql
                '$05 start
                .Append("SELECT /* SC3100101_024 */")
                .Append("        VS.VCLREGNO        AS VCLREGNO")
                .Append("      , VS.CUSTSEGMENT     AS CUSTSEGMENT")
                .Append("      , VS.BROUDCASTFLG    AS BROUDCASTFLG")
                .Append("      , VS.SALESTABLENO    AS SALESTABLENO")
                .Append("      , CASE ")
                .Append("             WHEN VS.CUSTSEGMENT IN(:CUSTSEGMENT_JI, :CUSTSEGMENT_MI) THEN MC.CST_NAME")
                .Append("             ELSE VS.TENTATIVENAME")
                .Append("         END AS CUSTNAME")
                .Append("      , CASE ")
                .Append("             WHEN VS.CUSTSEGMENT IN(:CUSTSEGMENT_JI, :CUSTSEGMENT_MI) THEN MC.NAMETITLE_NAME")
                .Append("             ELSE NULL")
                .Append("         END AS CUSTNAMETITLE")
                .Append("      , VS.STAFFCD         AS STAFFCD")
                .Append("      , VS.ACCOUNT         AS ACCOUNT")
                .Append("      , VS.CUSTID          AS CUSTID")
                .Append("      , VS.SALESSTART      AS SALESSTART")
                .Append("      , VS.VISITPERSONNUM  AS VISITPERSONNUM")
                .Append("      , VS.FLLWUPBOX_DLRCD AS FLLOWUPBOX_DLRCD")
                .Append("      , VS.FLLWUPBOX_STRCD AS FLLOWUPBOX_STRCD")
                .Append("      , VS.FLLWUPBOX_SEQNO AS FLLOWUPBOX_SEQNO")
                .Append("      , VS.VISITSTATUS AS VISITSTATUS")
                ' $10 start NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)
                .Append("      , NVL(LVS.TELNO, MC.CST_PHONE) AS TELNUMBER")
                ' $10 end NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)
                .Append("   FROM TBL_VISIT_SALES VS")
                ' $10 start NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)
                .Append("      , TBL_LC_VISIT_SALES LVS ")
                ' $10 end NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)
                .Append("      , TB_M_CUSTOMER MC")
                .Append("  WHERE TO_NUMBER(VS.CUSTID) = MC.CST_ID(+)")
                .Append("    AND VS.VISITSEQ = :VISITSEQ")
                ' $10 start NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)
                .Append("    AND VS.VISITSEQ = LVS.VISITSEQ(+)")
                ' $10 end NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)
                ' $05 end
                If Not String.IsNullOrEmpty(visitStatus) Then
                    .Append("     AND VS.VISITSTATUS = :VISITSTATUS")
                End If

            End With

            'DbSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3100101DataSet.VisitReceptionVisitorCustomerDataTable)("SC3100101_024")
                query.CommandText = sql.ToString()

                'SQLパラメータ設定
                query.AddParameterWithTypeValue("CUSTSEGMENT_JI", OracleDbType.Char, VisitorSegmentOriginal)
                query.AddParameterWithTypeValue("CUSTSEGMENT_MI", OracleDbType.Char, VisitorSegmentNew)
                query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Long, visitSequence)

                If Not String.IsNullOrEmpty(visitStatus) Then
                    query.AddParameterWithTypeValue("VISITSTATUS", OracleDbType.Char, visitStatus)
                End If

                'SQL実行（結果表を返却）
                Return query.GetData()
            End Using
        End Function
#End Region

#Region "スタッフ通知依頼情報の取得"

        ''' <summary>
        ''' スタッフ通知依頼情報の取得
        ''' </summary>
        ''' <param name="visitSeq">シーケンス連番</param>
        ''' <param name="noticeKind">通知依頼種別</param>
        ''' <param name="lastStatus">最終ステータス</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetStaffNoticeRequest(ByVal visitSeq As Long, _
                                              ByVal noticeKind As String, _
                                              ByVal lastStatus As List(Of String) _
                                              ) As SC3100101DataSet.VisitReceptionStaffNoticeRequestDataTable

            '最終ステータスSQL組み立て
            Dim sqlStatus As New StringBuilder
            Dim isFirst As Boolean = True
            For Each status As String In lastStatus

                If isFirst Then
                    isFirst = False
                Else
                    sqlStatus.Append(",")
                End If

                sqlStatus.Append("'")
                sqlStatus.Append(status)
                sqlStatus.Append("'")
            Next

            'SQL組み立て
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* SC3100101_025 */")
                .Append("       Q1.NOTICEREQCTG AS NOTICEREQCTG")
                .Append("     , MIN(Q1.TOACCOUNTNAME) AS TOACCOUNTNAME")
                .Append("     , MIN(NI3.FROMACCOUNTNAME) AS FROMACCOUNTNAME")
                .Append("     , MIN(Q1.SENDDATE) AS SENDDATE")
                .Append("  FROM (")
                .Append("    SELECT")
                .Append("           NQ.NOTICEREQID")
                .Append("         , NQ.NOTICEREQCTG")
                .Append("         , NI2.TOACCOUNTNAME")
                .Append("         , NI2.SENDDATE")
                .Append("         , VS.VISITSEQ")
                .Append("         , VS.ACCOUNT")
                .Append("      FROM TBL_NOTICEREQUEST NQ")
                .Append("         , TBL_VISIT_SALES VS")
                .Append("         , TBL_NOTICEINFO NI2")
                .Append("     WHERE NQ.NOTICEREQID = NI2.NOTICEREQID")
                .Append("       AND NI2.FROMACCOUNT = VS.ACCOUNT")
                .Append("       AND (NI2.TOACCOUNT <> VS.ACCOUNT")
                .Append("        OR NI2.TOACCOUNT IS NULL)")
                ' $01 start 複数顧客に対する商談平行対応
                .Append("       AND NI2.SENDDATE >= VS.FIRST_SALESSTART")
                ' $01 end   複数顧客に対する商談平行対応
                .Append("       AND NI2.STATUS = :NOTICESTATUS1")
                .Append("       AND NQ.DLRCD = VS.DLRCD")
                .Append("       AND NQ.STRCD = VS.STRCD")
                .Append("       AND NQ.CRCUSTID = VS.CUSTID")
                .Append("       AND NQ.CSTKIND = VS.CUSTSEGMENT")
                .Append("       AND NQ.CUSTOMERCLASS = :CUSTOMERCLASS")
                .Append("       AND NQ.NOTICEREQCTG = :NOTICEREQCTG")
                .Append("       AND NQ.STATUS IN (")
                .Append(sqlStatus.ToString)
                .Append(")")
                .Append("       AND VS.VISITSEQ = :VISITSEQ")
                .Append("   ) Q1")
                .Append("   , TBL_NOTICEINFO NI3")
                .Append(" WHERE Q1.NOTICEREQID = NI3.NOTICEREQID(+)")
                .Append("   AND Q1.ACCOUNT = NI3.TOACCOUNT(+)")
                .Append("   AND NI3.STATUS(+) = :NOTICESTATUS3")
                .Append(" GROUP BY Q1.NOTICEREQID, Q1.NOTICEREQCTG")
            End With

            'DbSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3100101DataSet.VisitReceptionStaffNoticeRequestDataTable)("SC3100101_025")
                query.CommandText = sql.ToString()

                'SQLパラメータ設定
                query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Long, visitSeq)
                query.AddParameterWithTypeValue("NOTICEREQCTG", OracleDbType.Char, noticeKind)
                query.AddParameterWithTypeValue("CUSTOMERCLASS", OracleDbType.Char, CustomerClassOwner)
                query.AddParameterWithTypeValue("NOTICESTATUS1", OracleDbType.Char, NoticeStatus)
                query.AddParameterWithTypeValue("NOTICESTATUS3", OracleDbType.Char, ReceiveStatus)

                'SQL実行（結果表を返却）
                Return query.GetData()
            End Using

        End Function
#End Region

#Region "来店回数の取得"

        ''' <summary>
        ''' 来店回数の取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="storeCode">店舗コード</param>
        ''' <param name="followUpBoxSeqNo">Fllow-UpBox 内連番</param>
        ''' <returns>来店回数</returns>
        ''' <remarks></remarks>
        Public Function GetVisitCount(ByVal dealerCode As String, _
                                      ByVal storeCode As String, _
                                      ByVal followUpBoxSeqNo As Decimal) As SC3100101DataSet.VisitReceptionVisitCountDataTable

            'SQL組み立て
            Dim sql As New StringBuilder
            With sql
                ' $05 start
                .Append(" SELECT /* SC3100101_026 */ ")
                .Append("        SUM(CNT) AS VISITCOUNT ")
                .Append("   FROM ")
                .Append("      ( ")
                .Append("        ( ")
                .Append("         SELECT ")
                .Append("                NVL(COUNT(1),0) CNT ")
                .Append("           FROM ")
                .Append("                TB_T_SALES T1 ")
                .Append("              , TB_T_ACTIVITY T2 ")
                .Append("          WHERE T1.REQ_ID = T2.REQ_ID ")
                .Append("            AND T1.ATT_ID = T2.ATT_ID ")
                .Append("            AND T1.SALES_ID = :FLLWUPBOX_SEQNO ")
                .Append("            AND T2.RSLT_CONTACT_MTD = :CONTACTNO ")
                .Append("        ) ")
                .Append("        UNION ALL ")
                .Append("        ( ")
                .Append("         SELECT ")
                .Append("                NVL(COUNT(1),0) CNT ")
                .Append("           FROM ")
                .Append("                TB_H_SALES T1 ")
                .Append("              , TB_H_ACTIVITY T2 ")
                .Append("          WHERE T1.REQ_ID = T2.REQ_ID ")
                .Append("            AND T1.ATT_ID = T2.ATT_ID ")
                .Append("            AND T1.SALES_ID = :FLLWUPBOX_SEQNO ")
                .Append("            AND T2.RSLT_CONTACT_MTD = :CONTACTNO ")
                .Append("        )  ")
                .Append("        UNION ALL ")
                .Append("        ( ")
                .Append("         SELECT ")
                .Append("                NVL(COUNT(1),0) CNT ")
                .Append("           FROM ")
                .Append("                TBL_BOOKEDAFTERFOLLOWRSLT ")
                .Append("          WHERE FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO ")
                .Append("            AND CONTACTNO = :CONTACTNO ")
                .Append("        ) ")
                .Append("      ) ")
                ' $05 end
            End With

            'DbSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3100101DataSet.VisitReceptionVisitCountDataTable)("SC3100101_026")
                query.CommandText = sql.ToString()

                ' $05 start FollowUp-Box 桁数変更対応
                query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, followUpBoxSeqNo)
                query.AddParameterWithTypeValue("CONTACTNO", OracleDbType.NVarchar2, ContactNoVisit.ToString)
                ' $05 end FollowUp-Box 桁数変更対応

                Return query.GetData()
            End Using

        End Function
#End Region
        ' $02 end 受付共通からの移植
        ' $02 start 新車タブレットショールーム管理機能開発

#Region "来店件数"
        ''' <summary>
        ''' 来店組数の取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="storeCode">店舗コード</param>
        ''' <param name="startTime">開始日時</param>
        ''' <param name="endTime">終了日時</param>
        ''' <returns>アンドンデータテーブル</returns>
        ''' <remarks></remarks>
        Public Function GetVisitCustCount(ByVal dealerCode As String, _
                                        ByVal storeCode As String, _
                                        ByVal startTime As Date, _
                                        ByVal endTime As Date) _
                                          As SC3100101DataSet.SC3100101BordCountDataTable

            'DbSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3100101DataSet.SC3100101BordCountDataTable)("SC3100101_027")


                'SQL組み立て
                Dim sql As New StringBuilder
                '$09 NSK s.natsume TKM Change request development for Next Gen e-CRB (CR075) START
                'With sql
                '    .Append(" SELECT /* SC3100101_027 */")
                '    .Append("        COUNT(1) AS BORDCOUNT")
                '    .Append("   FROM (")
                '    .Append("     SELECT DISTINCT")
                '    .Append("            CUSTID")
                '    .Append("          , CUSTSEGMENT")
                '    .Append("       FROM TBL_VISIT_SALES")
                '    .Append("      WHERE COALESCE(VISITTIMESTAMP, STOPTIME, SALESSTART)")
                '    .Append("            BETWEEN :STARTTIME AND :ENDTIME")
                '    .Append("        AND CUSTID IS NOT NULL")
                '    .Append("        AND DLRCD = :DLRCD")
                '    .Append("        AND STRCD = :STRCD")
                '    .Append("      UNION ALL")
                '    .Append("     SELECT")
                '    .Append("            CUSTID")
                '    .Append("          , CUSTSEGMENT")
                '    .Append("       FROM TBL_VISIT_SALES")
                '    .Append("      WHERE NVL(STOPTIME, VISITTIMESTAMP) BETWEEN :STARTTIME AND :ENDTIME")
                '    .Append("        AND DLRCD = :DLRCD")
                '    .Append("        AND STRCD = :STRCD")
                '    .Append("        AND CUSTID IS NULL")
                '    .Append("       )")
                'End With
                With sql
                    .Append("SELECT /* SC3100101_027 */ ")
                    .Append("	 COUNT(1) AS BORDCOUNT ")
                    .Append(" FROM TBL_VISIT_SALES ")
                    .Append(" WHERE VISITTIMESTAMP BETWEEN :STARTTIME ")
                    .Append("						 AND :ENDTIME")
                    .Append("  AND DLRCD = :DLRCD ")
                    .Append("  AND STRCD = :STRCD ")
                End With
                '$09 NSK s.natsume TKM Change request development for Next Gen e-CRB (CR075) END
                query.CommandText = sql.ToString()
                sql = Nothing

                'SQLパラメータ設定
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, storeCode)
                query.AddParameterWithTypeValue("STARTTIME", OracleDbType.Date, startTime)
                query.AddParameterWithTypeValue("ENDTIME", OracleDbType.Date, endTime)

                'SQL実行（結果表を返却）
                Return query.GetData()
            End Using
        End Function
#End Region

        '$08 NSK s.natsume TKM Change request development for Next Gen e-CRB (CR008) START
#Region "査定件数"
        ' ''' <summary>
        ' ''' 査定依頼を行った件数
        ' ''' </summary>
        ' ''' <param name="dealerCode">販売店コード</param>
        ' ''' <param name="storeCode">店舗コード</param>
        ' ''' <param name="startTime">開始日時</param>
        ' ''' <param name="endTime">終了日時</param>
        ' ''' <returns>アンドンデータテーブル</returns>
        ' ''' <remarks></remarks>
        'Public Function GetAssessmentCount(ByVal dealerCode As String, _
        '                                ByVal storeCode As String, _
        '                                ByVal startTime As Date, _
        '                                ByVal endTime As Date) _
        '                                  As SC3100101DataSet.SC3100101BordCountDataTable

        '    'DbSelectQueryインスタンス生成
        '    Using query As New DBSelectQuery(Of SC3100101DataSet.SC3100101BordCountDataTable)("SC3100101_028")

        '        'SQL組み立て
        '        Dim sql As New StringBuilder
        '        With sql
        '            ' $04 start 
        '            .Append("SELECT /* SC3100101_028 */")
        '            .Append("       COUNT(DISTINCT")
        '            .Append("                 CASE WHEN T2.CSTKIND = :CUSTSEGMENT_JI")
        '            .Append("                      THEN T2.ORGCSTVCL_VIN ")
        '            .Append("                      WHEN T2.CSTKIND = :CUSTSEGMENT_MI")
        '            .Append("                      THEN TO_CHAR(T2.NEWCSTVCL_SEQNO)")
        '            .Append("                 END")
        '            .Append("            ) AS BORDCOUNT")
        '            .Append("  FROM (")
        '            .Append("    SELECT DISTINCT")
        '            .Append("              NI1.NOTICEREQID")
        '            .Append("            , NR1.REQCLASSID")
        '            .Append("      FROM TBL_NOTICEREQUEST NR1")
        '            .Append("         , TBL_NOTICEINFO NI1")
        '            .Append("         , TBL_NOTICEINFO NI4")
        '            .Append("     WHERE NI1.NOTICEREQID = NR1.NOTICEREQID")
        '            .Append("       AND NI1.NOTICEREQID = NI4.NOTICEREQID")
        '            .Append("       AND NR1.NOTICEREQCTG = :NOTICEREQCGT01")
        '            .Append("       AND NI1.STATUS = :STATUS1")
        '            .Append("       AND NI4.STATUS = :STATUS4")
        '            .Append("       AND NI1.SENDDATE BETWEEN :STARTTIME AND :ENDTIME")
        '            .Append("       AND NR1.DLRCD = :DLRCD")
        '            .Append("       AND NR1.STRCD = :STRCD ")
        '            .Append("       AND NI4.SENDDATE BETWEEN :STARTTIME AND :ENDTIME")
        '            .Append("       ) T1")
        '            .Append("      , TBL_UCARASSESSMENT T2")
        '            .Append("  WHERE T1.REQCLASSID = T2.ASSESSMENTNO")
        '            .Append("    AND T1.NOTICEREQID = T2.NOTICEREQID")
        '            ' $04 end
        '        End With

        '        query.CommandText = sql.ToString()
        '        sql = Nothing

        '        'SQLパラメータ設定
        '        query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
        '        query.AddParameterWithTypeValue("NOTICEREQCGT01", OracleDbType.Char, NoticeReqCGTAssessment)
        '        query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, storeCode)
        '        query.AddParameterWithTypeValue("STATUS1", OracleDbType.Char, LastStatusAssessment)
        '        query.AddParameterWithTypeValue("STATUS4", OracleDbType.Char, LastStatusAnswer)
        '        query.AddParameterWithTypeValue("STARTTIME", OracleDbType.Date, startTime)
        '        query.AddParameterWithTypeValue("ENDTIME", OracleDbType.Date, endTime)
        '        ' $04 start
        '        query.AddParameterWithTypeValue("CUSTSEGMENT_JI", OracleDbType.Char, VisitorSegmentOriginal)
        '        query.AddParameterWithTypeValue("CUSTSEGMENT_MI", OracleDbType.Char, VisitorSegmentNew)
        '        ' $04 end
        '        'SQL実行（結果表を返却）
        '        Return query.GetData()
        '    End Using
        'End Function
#End Region
        '$08 NSK s.natsume TKM Change request development for Next Gen e-CRB (CR008) END

#Region "見積件数"
        ''' <summary>
        ''' 見積りを印刷した件数
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="storeCode">店舗コード</param>
        ''' <param name="startTime">開始日時</param>
        ''' <param name="endTime">終了日時</param>
        ''' <returns>アンドンデータテーブル</returns>
        ''' <remarks></remarks>
        Public Function GetEstimateCount(ByVal dealerCode As String, ByVal storeCode As String, _
                                        ByVal startTime As Date, ByVal endTime As Date) _
                                          As SC3100101DataSet.SC3100101BordCountDataTable

            'DbSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3100101DataSet.SC3100101BordCountDataTable)("SC3100101_029")

                'SQL組み立て
                Dim sql As New StringBuilder
                '$09 NSK s.natsume TKM Change request development for Next Gen e-CRB (CR075) START
                'With sql

                '    .Append(" SELECT /* SC3100101_029 */ ")
                '    .Append("        COUNT(1) AS BORDCOUNT ")
                '    .Append("   FROM ( ")
                '    .Append("     SELECT DISTINCT ")
                '    .Append("            VS.CUSTID ")
                '    .Append("          , VS.CUSTSEGMENT ")
                '    .Append("       FROM TBL_ESTIMATEINFO EI ")
                '    .Append("          , TBL_VISIT_SALES VS ")
                '    .Append("      WHERE EI.DLRCD = VS.DLRCD ")
                '    .Append("        AND EI.STRCD = VS.STRCD ")
                '    .Append("        AND EI.FLLWUPBOX_SEQNO = VS.FLLWUPBOX_SEQNO ")
                '    .Append("        AND EI.DLRCD = :DLRCD ")
                '    .Append("        AND EI.STRCD = :STRCD ")
                '    .Append("        AND EI.UPDATEDATE  BETWEEN :STARTTIME AND :ENDTIME ")
                '    .Append("        AND VS.VISITSTATUS IN (:VISITSTATUS07, :VISITSTATUS08) ")
                '    .Append("        AND VS.FIRST_SALESSTART BETWEEN :STARTTIME AND :ENDTIME ")
                '    .Append("   ) ")

                'End With
                With sql
                    .Append("SELECT /* SC3100101_029 */")
                    .Append("	SUM(CNT) AS BORDCOUNT")
                    .Append("	FROM (")
                    .Append("		 SELECT")
                    .Append("		        COUNT(1) AS CNT")
                    .Append("		   FROM TBL_VISIT_SALES VS")
                    .Append("		  WHERE EXISTS (")
                    .Append("		            SELECT 1")
                    .Append("		              FROM TB_T_SALES_ACT SA")
                    .Append("		              WHERE VS.FLLWUPBOX_SEQNO = SA.SALES_ID")
                    .Append("		                AND SA.RSLT_SALES_CAT = :RSLTSALESACT6")
                    .Append("		        )")
                    .Append("		    AND VS.DLRCD = :DLRCD")
                    .Append("		    AND VS.STRCD = :STRCD")
                    .Append("		    AND VS.SALESSTART BETWEEN :STARTTIME AND :ENDTIME")
                    .Append("		    AND VS.VISITTIMESTAMP BETWEEN :STARTTIME AND :ENDTIME")
                    .Append("	    UNION ALL")
                    .Append("	     SELECT")
                    .Append("		       COUNT(1) AS CNT")
                    .Append("		   FROM TBL_VISIT_SALES VS")
                    .Append("		  WHERE EXISTS (")
                    .Append("		            SELECT 1")
                    .Append("		              FROM TB_H_SALES_ACT SA")
                    .Append("		             WHERE VS.FLLWUPBOX_SEQNO = SA.SALES_ID")
                    .Append("		               AND SA.RSLT_SALES_CAT = :RSLTSALESACT6")
                    .Append("		        )")
                    .Append("		    AND VS.DLRCD = :DLRCD")
                    .Append("		    AND VS.STRCD = :STRCD")
                    .Append("		    AND VS.SALESSTART BETWEEN :STARTTIME AND :ENDTIME")
                    .Append("		    AND VS.VISITTIMESTAMP BETWEEN :STARTTIME AND :ENDTIME")
                    .Append("	    )")
                End With
                '$09 NSK s.natsume TKM Change request development for Next Gen e-CRB (CR075) END
                query.CommandText = sql.ToString()
                sql = Nothing

                'SQLパラメータ設定
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, storeCode)
                '$09 NSK s.natsume TKM Change request development for Next Gen e-CRB (CR075) START
                'query.AddParameterWithTypeValue("VISITSTATUS07", OracleDbType.Char, VisitStatusNegotiate)
                'query.AddParameterWithTypeValue("VISITSTATUS08", OracleDbType.Char, VisitStatusNegotiateEnd)
                query.AddParameterWithTypeValue("RSLTSALESACT6", OracleDbType.NVarchar2, ResultSalesActionContract)
                '$09 NSK s.natsume TKM Change request development for Next Gen e-CRB (CR075) END
                query.AddParameterWithTypeValue("STARTTIME", OracleDbType.Date, startTime)
                query.AddParameterWithTypeValue("ENDTIME", OracleDbType.Date, endTime)

                'SQL実行（結果表を返却）
                Return query.GetData()
            End Using
        End Function
#End Region

#Region "受注件数"
        ''' <summary>
        ''' 成約件数を取得する
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="storeCode">店舗コード</param>
        ''' <param name="startTime">開始日時</param>
        ''' <param name="endTime">終了日時</param>
        ''' <returns>アンドンデータテーブル</returns>
        ''' <remarks></remarks>
        Public Function GetConclusionCount(ByVal dealerCode As String, _
                                        ByVal storeCode As String, _
                                        ByVal startTime As Date, _
                                        ByVal endTime As Date) _
                                          As SC3100101DataSet.SC3100101BordCountDataTable

            'SQL組み立て
            Dim sql As New StringBuilder
            '$09 NSK s.natsume TKM Change request development for Next Gen e-CRB (CR075) START
            '' $06 start
            'With sql
            '    .Append(" SELECT /* SC3100101_030 */ ")
            '    .Append("        NVL(SUM(T2.PREF_AMOUNT),0) AS BORDCOUNT ")
            '    .Append("   FROM TB_T_SPM_BEFORE_ODR_CHIP T1 ")
            '    .Append("      , TB_T_SPM_SUCCESS_VCL T2 ")
            '    .Append("  WHERE T1.SALES_ID = T2.SALES_ID ")
            '    .Append("    AND T1.DLR_CD = :DLR_CD ")
            '    .Append("    AND T1.BRN_CD = :BRN_CD ")
            '    .Append("    AND T1.SALES_STATUS = :SALES_STATUS ")
            '    .Append("    AND T1.LAST_ACT_DATE BETWEEN :STARTTIME AND :ENDTIME ")
            'End With
            '' $06 end
            With sql
                .Append("SELECT /* SC3100101_030 */ ")
                .Append("	SUM(CNT) AS BORDCOUNT ")
                .Append(" FROM ( ")
                .Append("	SELECT ")
                .Append("	       COUNT(1) AS CNT ")
                .Append("	  FROM TBL_VISIT_SALES VS")
                .Append("	  WHERE EXISTS ( ")
                .Append("	  			SELECT 1 ")
                .Append("	  			FROM  TB_T_AFTER_ODR AO ")
                .Append("	  				, TB_T_AFTER_ODR_ACT AOA")
                .Append("	  			WHERE VS.FLLWUPBOX_SEQNO = AO.SALES_ID ")
                .Append("	  			  AND AO.AFTER_ODR_ID = AOA.AFTER_ODR_ID ")
                .Append("	  			  AND AOA.RSLT_END_DATEORTIME BETWEEN :STARTTIME AND :ENDTIME ")
                .Append("	    		  AND AOA.AFTER_ODR_ACT_CD = :AFTERODRACT11 ")
                .Append("	    		)")
                .Append("	    AND VS.DLRCD = :DLR_CD ")
                .Append("	    AND VS.STRCD = :BRN_CD ")
                .Append("	    AND VS.VISITTIMESTAMP BETWEEN :STARTTIME AND :ENDTIME ")
                .Append("	    AND VS.SALESSTART BETWEEN :STARTTIME AND :ENDTIME ")
                .Append("	UNION ALL")
                .Append("	SELECT ")
                .Append("	       COUNT(1) AS CNT ")
                .Append("	  FROM TBL_VISIT_SALES VS ")
                .Append("	  WHERE EXISTS ( ")
                .Append("	  			SELECT 1 ")
                .Append("	  			FROM  TB_H_AFTER_ODR AO ")
                .Append("	  				, TB_H_AFTER_ODR_ACT AOA")
                .Append("	  			WHERE VS.FLLWUPBOX_SEQNO = AO.SALES_ID ")
                .Append("	  			  AND AO.AFTER_ODR_ID = AOA.AFTER_ODR_ID ")
                .Append("	  			  AND AOA.RSLT_END_DATEORTIME BETWEEN :STARTTIME AND :ENDTIME ")
                .Append("	    		  AND AOA.AFTER_ODR_ACT_CD = :AFTERODRACT11 ")
                .Append("	    		)")
                .Append("	    AND VS.DLRCD = :DLR_CD ")
                .Append("	    AND VS.STRCD = :BRN_CD ")
                .Append("	    AND VS.VISITTIMESTAMP BETWEEN :STARTTIME AND :ENDTIME ")
                .Append("	    AND VS.SALESSTART BETWEEN :STARTTIME AND :ENDTIME ")
                .Append("		)")
            End With
            '$09 NSK s.natsume TKM Change request development for Next Gen e-CRB (CR075) START
            'DbSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3100101DataSet.SC3100101BordCountDataTable)("SC3100101_030")
                query.CommandText = sql.ToString()

                'SQLパラメータ設定
                ' $06 start
                '$09 NSK s.natsume TKM Change request development for Next Gen e-CRB (CR075) START
                'query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)
                'query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, storeCode)
                'query.AddParameterWithTypeValue("SALES_STATUS", OracleDbType.NVarchar2, SalesStatusSuccess)
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.Char, storeCode)
                query.AddParameterWithTypeValue("AFTERODRACT11", OracleDbType.NVarchar2, AfterOrderActionCodeContract)
                '$09 NSK s.natsume TKM Change request development for Next Gen e-CRB (CR075) END
                query.AddParameterWithTypeValue("STARTTIME", OracleDbType.Date, startTime)
                query.AddParameterWithTypeValue("ENDTIME", OracleDbType.Date, endTime)
                ' $06 end

                'SQL実行（結果表を返却）
                Return query.GetData()
            End Using

        End Function
#End Region

#Region "商談件数"
        ''' <summary>
        ''' 商談数を取得する
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="storeCode">店舗コード</param>
        ''' <param name="startTime">開始日時</param>
        ''' <param name="endTime">終了日時</param>
        ''' <returns>アンドンデータテーブル</returns>
        ''' <remarks></remarks>
        Public Function GetSalesCount(ByVal dealerCode As String, _
                                        ByVal storeCode As String, _
                                        ByVal startTime As Date, _
                                        ByVal endTime As Date) _
                                          As SC3100101DataSet.SC3100101BordCountDataTable
            'SQL組み立て
            Dim sql As New StringBuilder
            '$09 NSK s.natsume TKM Change request development for Next Gen e-CRB (CR075) START
            'With sql
            '    .Append(" SELECT /* SC3100101_031 */")
            '    .Append("        COUNT(1) AS BORDCOUNT")
            '    .Append("   FROM (")
            '    .Append("     SELECT DISTINCT")
            '    .Append("            CUSTID")
            '    .Append("          , CUSTSEGMENT")
            '    .Append("       FROM TBL_VISIT_SALES")
            '    .Append("      WHERE DLRCD = :DLRCD")
            '    .Append("        AND STRCD = :STRCD")
            '    .Append("        AND SALESSTART BETWEEN :STARTTIME")
            '    .Append("                           AND :ENDTIME")
            '    .Append("        AND VISITSTATUS IN (:VISITSTATUS1, :VISITSTATUS2)")
            '    .Append("        )")
            'End With
            With sql
                .Append("SELECT /* SC3100101_031 */")
                .Append("       COUNT(1) AS BORDCOUNT ")
                .Append(" FROM TBL_VISIT_SALES ")
                .Append(" WHERE DLRCD = :DLRCD ")
                .Append("  AND STRCD = :STRCD ")
                .Append("  AND SALESSTART BETWEEN :STARTTIME")
                .Append("                     AND :ENDTIME")
                .Append("  AND VISITSTATUS IN (:VISITSTATUS1, :VISITSTATUS2)")
                .Append("  AND VISITTIMESTAMP BETWEEN :STARTTIME")
                .Append("                     AND :ENDTIME")
            End With
            '$09 NSK s.natsume TKM Change request development for Next Gen e-CRB (CR075) END

            'DbSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3100101DataSet.SC3100101BordCountDataTable)("SC3100101_031")
                query.CommandText = sql.ToString()

                'SQLパラメータ設定
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, storeCode)
                query.AddParameterWithTypeValue("STARTTIME", OracleDbType.Date, startTime)
                query.AddParameterWithTypeValue("ENDTIME", OracleDbType.Date, endTime)
                query.AddParameterWithTypeValue("VISITSTATUS1", OracleDbType.Char, VisitStatusNegotiate)
                query.AddParameterWithTypeValue("VISITSTATUS2", OracleDbType.Char, VisitStatusNegotiateEnd)

                'SQL実行（結果表を返却）
                Return query.GetData()
            End Using
        End Function
#End Region

#Region "接客情報取得"

        ''' <summary>
        ''' 接客情報を取得する
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="storeCode">店舗コード</param>
        ''' <param name="presenceClass">取得エリア区分</param>
        ''' <param name="serviceTimestampStart">開始日時</param>
        ''' <param name="serviceTimestampEnd">終了日時</param>
        ''' <returns>接客データセット</returns>
        ''' <remarks></remarks>
        Public Function GetSalesInfo(ByVal dealerCode As String, _
                                          ByVal storeCode As String, _
                                          ByVal presenceClass As String, _
                                          ByVal serviceTimestampStart As Date, _
                                          ByVal serviceTimestampEnd As Date) As SC3100101DataSet.SC3100101ReceptionInfoDataTable

            Dim dt As SC3100101DataSet.SC3100101ReceptionInfoDataTable = Nothing

            ' SQL文作成
            Dim sql As New StringBuilder
            With sql
                ' $05 start
                .Append("SELECT /* SC3100101_032 */")
                .Append("        VS.VISITSEQ")
                .Append("      , VS.VISITTIMESTAMP")
                .Append("      , VS.VCLREGNO")
                .Append("      , VS.VISITPERSONNUM")
                .Append("      , VS.VISITMEANS")
                .Append("      , VS.VISITSTATUS")
                .Append("      , VS.SALESSTART")
                .Append("      , VS.SALESTABLENO")
                .Append("      , US2.USERNAME")
                .Append("      , US1.ORG_IMGFILE")
                .Append("      , US1.ACCOUNT")
                .Append("      , CASE")
                .Append("             WHEN VS.CUSTSEGMENT IN (:CUSTSEGMENT_JI, :CUSTSEGMENT_MI) THEN CU.CST_NAME")
                .Append("             ELSE VS.TENTATIVENAME")
                .Append("         END AS CUSTNAME")
                .Append("      , CASE")
                .Append("             WHEN VS.CUSTSEGMENT IN(:CUSTSEGMENT_JI, :CUSTSEGMENT_MI) THEN CU.NAMETITLE_NAME")
                .Append("             ELSE NULL")
                .Append("         END AS CUSTNAMETITLE")
                .Append("      , VS.STOPTIME")
                .Append("      , VS.UNNECESSARYCOUNT")
                .Append("      , VS.UNNECESSARYDATE")
                .Append("      , VS.SC_ASSIGNDATE")
                .Append("      , US1.PRESENCECATEGORY")
                .Append("      , US1.PRESENCEDETAIL")
                .Append("      , VS.CUSTSEGMENT")
                .Append("      , VS.CUSTID")
                '2018/07/12 NSK a.kani TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
                .Append("      , CASE ")
                .Append("               WHEN T1.CST_ID IS NOT NULL THEN :ICON_FLAG_ON ")
                .Append("                ELSE :ICON_FLAG_OFF ")
                .Append("              END AS ICON_FLAG_L ")
                '2018/07/12 NSK a.kani TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
                .Append("   FROM TBL_VISIT_SALES VS")
                .Append("      , TBL_USERS US1")
                .Append("      , TBL_USERS US2")
                .Append("      , TB_M_CUSTOMER CU")
                '2018/07/12 NSK a.kani TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
                .Append("      , (SELECT ")
                .Append("                 MCV.CST_ID ")
                .Append("           FROM  TB_M_CUSTOMER_VCL MCV ")
                .Append("                ,TB_M_VEHICLE_DLR MVD ")
                .Append("           WHERE MCV.VCL_ID = MVD.VCL_ID ")
                .Append("             AND MCV.DLR_CD = MVD.DLR_CD ")
                .Append("             AND MCV.DLR_CD = :DLRCD ")
                .Append("             AND MCV.OWNER_CHG_FLG = :OWNER_CHG_FLG_NONE ")
                .Append("             AND MVD.IMP_VCL_FLG = :ICON_FLAG_L ")
                .Append("           GROUP BY ")
                .Append("                   MCV.CST_ID ")
                .Append("         ) T1 ")
                '2018/07/12 NSK a.kani TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
                .Append("  WHERE VS.ACCOUNT = US1.ACCOUNT(+)")
                .Append("    AND VS.STAFFCD = US2.ACCOUNT(+)")
                .Append("    AND TO_NUMBER(VS.CUSTID) = CU.CST_ID(+)")
                '2018/07/12 NSK a.kani TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
                .Append("    AND CU.CST_ID = T1.CST_ID(+) ")
                '2018/07/12 NSK a.kani TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
                .Append("    AND US2.OPERATIONCODE(+) = :OPERATIONCODE")
                .Append("    AND US2.DELFLG(+) = :DELFLG")
                .Append("    AND VS.DLRCD = :DLRCD")
                .Append("    AND VS.STRCD = :STRCD")
                ' $05 end

                ' 振り当て待ち
                If presenceClass = ReceptionClassWaitAssgined Then
                    .Append("    AND US1.OPERATIONCODE(+) = :OPERATIONCODE")
                    .Append("    AND US1.DELFLG(+) = :DELFLG")
                    .Append("    AND NVL(VS.STOPTIME, VS.VISITTIMESTAMP) BETWEEN :STARTTIME AND :ENDTIME")
                    .Append("    AND VS.VISITSTATUS IN (:VISITSTATUS01, :VISITSTATUS10)")
                    .Append("  ORDER BY NVL(VS.STOPTIME, VS.VISITTIMESTAMP) ASC")
                    ' 接客待ち
                ElseIf presenceClass = ReceptionClassWaitService Then
                    .Append("    AND US1.OPERATIONCODE(+) = :OPERATIONCODE")
                    .Append("    AND US1.DELFLG(+) = :DELFLG")
                    .Append("    AND NVL(VS.STOPTIME, VS.VISITTIMESTAMP) BETWEEN :STARTTIME AND :ENDTIME")
                    .Append("    AND VS.VISITSTATUS IN (:VISITSTATUS02, :VISITSTATUS03")
                    .Append("                         , :VISITSTATUS04, :VISITSTATUS05, :VISITSTATUS06)")
                    .Append("  ORDER BY NVL(VS.STOPTIME, VS.VISITTIMESTAMP) ASC")

                    ' 接客中
                ElseIf presenceClass = ReceptionClassNegotiation Then
                    ' $03 start 納車作業中ステータス対応
                    .Append("    AND US1.OPERATIONCODE = :OPERATIONCODE")
                    .Append("    AND US1.DELFLG = :DELFLG")
                    .Append("    AND NVL(VS.STOPTIME, VS.SALESSTART) BETWEEN :STARTTIME AND :ENDTIME ")
                    .Append("    AND VS.VISITSTATUS IN(:VISITSTATUS07, :VISITSTATUS09, :VISITSTATUS11) ")
                    .Append("  ORDER BY ")
                    .Append("        CASE ")
                    .Append("        WHEN VS.VISITSTATUS = :VISITSTATUS07 THEN 0 ")
                    .Append("        WHEN VS.VISITSTATUS = :VISITSTATUS11 THEN 1 ")
                    .Append("        ELSE 2 ")
                    .Append("         END ASC ")
                    .Append("      , CASE ")
                    .Append("        WHEN VS.VISITSTATUS IN (:VISITSTATUS07, :VISITSTATUS11) THEN VS.SALESSTART ")
                    .Append("        ELSE VS.STOPTIME ")
                    .Append("         END ASC ")
                    ' $03 end 納車作業中ステータス対応
                End If
            End With
            ' $05 start コード分析対応
            Using query As New DBSelectQuery( _
                Of SC3100101DataSet.SC3100101ReceptionInfoDataTable)("SC3100101_032")
                query.CommandText = sql.ToString()
                sql = Nothing
                ' $05 end コード分析対応
                ' バインド変数
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, storeCode)
                query.AddParameterWithTypeValue("CUSTSEGMENT_JI", OracleDbType.Char, VisitorSegmentOriginal)
                query.AddParameterWithTypeValue("CUSTSEGMENT_MI", OracleDbType.Char, VisitorSegmentNew)
                query.AddParameterWithTypeValue("OPERATIONCODE", OracleDbType.Int64, OperationCodeSalesStaff)

                query.AddParameterWithTypeValue("STARTTIME", OracleDbType.Date, _
                        serviceTimestampStart)
                query.AddParameterWithTypeValue("ENDTIME", OracleDbType.Date, _
                        serviceTimestampEnd)
                query.AddParameterWithTypeValue("DELFLG", OracleDbType.Char, DeleteFlagNotDelete)

                '2018/07/12 NSK a.kani TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
                query.AddParameterWithTypeValue("OWNER_CHG_FLG_NONE", OracleDbType.NVarchar2, OwnerChgFlgNone)
                query.AddParameterWithTypeValue("ICON_FLAG_L", OracleDbType.NVarchar2, IconFlagL)
                query.AddParameterWithTypeValue("ICON_FLAG_ON", OracleDbType.NVarchar2, IconFlagOn)
                query.AddParameterWithTypeValue("ICON_FLAG_OFF", OracleDbType.NVarchar2, IconFlagOff)
                '2018/07/12 NSK a.kani TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
                If presenceClass = ReceptionClassWaitAssgined Then
                    query.AddParameterWithTypeValue("VISITSTATUS10", OracleDbType.Char, VisitStatusUnnecessary)
                    query.AddParameterWithTypeValue("VISITSTATUS01", OracleDbType.Char, VisitStatusFree)
                ElseIf presenceClass = ReceptionClassWaitService Then
                    query.AddParameterWithTypeValue("VISITSTATUS02", OracleDbType.Char, VisitStatusFreeBroadcast)
                    query.AddParameterWithTypeValue("VISITSTATUS03", OracleDbType.Char, VisitStatusAdjustment)
                    query.AddParameterWithTypeValue("VISITSTATUS04", OracleDbType.Char, VisitStatusDecisionBroadcast)
                    query.AddParameterWithTypeValue("VISITSTATUS05", OracleDbType.Char, VisitStatusDecision)
                    query.AddParameterWithTypeValue("VISITSTATUS06", OracleDbType.Char, VisitStatusWating)
                ElseIf presenceClass = ReceptionClassNegotiation Then
                    query.AddParameterWithTypeValue("VISITSTATUS07", OracleDbType.Char, VisitStatusNegotiate)
                    query.AddParameterWithTypeValue("VISITSTATUS09", OracleDbType.Char, VisitStatusNegotiateStop)
                    ' $03 start 納車作業中ステータス対応
                    query.AddParameterWithTypeValue("VISITSTATUS11", OracleDbType.Char, VisitStatusDeliverlyStart)
                    ' $03 end 納車作業中ステータス対応
                End If

                ' SQLの実行
                dt = query.GetData()

            End Using

            ' 検索結果返却
            Return dt

        End Function
#End Region

#Region "スタッフ情報取得"

        ''' <summary>
        ''' スタッフ情報を取得する
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="storeCode">店舗コード</param>
        ''' <param name="staffStatusClass">スタッフ状態区分</param>
        ''' <returns>スタッフ情報データセット</returns>
        ''' <remarks></remarks>
        Public Function GetSalesStuffInfo(ByVal dealerCode As String, _
                                          ByVal storeCode As String, _
                                          ByVal staffStatusClass As String, _
                                          ByVal startTime As Date, _
                                          ByVal endTime As Date) As SC3100101DataSet.SC3100101StaffStatusDataTable

            Dim dt As SC3100101DataSet.SC3100101StaffStatusDataTable = Nothing

                ' SQL文作成
                Dim sql As New StringBuilder
                With sql
                    .Append(" SELECT /* SC3100101_033 */")
                    .Append("        URS.ACCOUNT")
                    .Append("      , URS.USERNAME")
                    .Append("      , URS.ORG_IMGFILE")
                    .Append("      , URS.PRESENCECATEGORY")
                    .Append("      , URS.PRESENCEDETAIL")
                    .Append("   FROM TBL_USERS URS")

                    ' スタンバイ状態
                    If staffStatusClass = StaffStatusStandby Then
                        .Append("     , TBL_STANDBYSTAFF_SORT SS")
                        .Append(" WHERE URS.ACCOUNT = SS.ACCOUNT(+)")
                        .Append("   AND URS.DLRCD = SS.DLRCD(+)")
                        .Append("   AND URS.STRCD = SS.STRCD(+)")
                        .Append("   AND URS.PRESENCECATEGORYDATE = SS.PRESENCECATEGORYDATE(+)")
                        .Append("   AND URS.OPERATIONCODE = :OPERATIONCODE")
                        .Append("   AND URS.DLRCD = :DLRCD")
                        .Append("   AND URS.STRCD = :STRCD")
                        .Append("   AND URS.DELFLG = :DELFLG")
                        .Append("   AND URS.PRESENCECATEGORY = :PRESENCECATEGORY1")
                        .Append(" ORDER BY SS.SORTNO ASC, URS.PRESENCECATEGORYDATE ASC")

                        ' 商談中
                    ElseIf staffStatusClass = StaffStatusNegotiate Then
                        .Append("     , TBL_VISIT_SALES VS")
                        .Append(" WHERE VS.ACCOUNT = URS.ACCOUNT")
                        .Append("   AND VS.DLRCD = URS.DLRCD")
                        .Append("   AND VS.STRCD = URS.STRCD")
                        .Append("   AND URS.DLRCD = :DLRCD")
                        .Append("   AND URS.STRCD = :STRCD")
                        .Append("   AND URS.OPERATIONCODE = :OPERATIONCODE")
                        .Append("   AND URS.DELFLG = :DELFLG")
                        .Append("   AND URS.PRESENCECATEGORY = :PRESENCECATEGORY2")
                        .Append("   AND URS.PRESENCEDETAIL IN (:PRESENCEDETAIL0 , :PRESENCEDETAIL1)")
                        .Append("   AND VS.VISITSTATUS = :VISITSTATUS07")
                        .Append("   AND VS.SALESSTART BETWEEN :STARTTIME AND :ENDTIME")
                        .Append(" ORDER BY VS.SALESSTART")

                        ' 納車作業中
                    ElseIf staffStatusClass = StaffStatusDeliverly Then
                        .Append("     , TBL_VISIT_SALES VS")
                        .Append(" WHERE VS.ACCOUNT = URS.ACCOUNT")
                        .Append("   AND VS.DLRCD = URS.DLRCD")
                        .Append("   AND VS.STRCD = URS.STRCD")
                        .Append("   AND URS.DLRCD = :DLRCD")
                        .Append("   AND URS.STRCD = :STRCD")
                        .Append("   AND URS.OPERATIONCODE = :OPERATIONCODE")
                        .Append("   AND URS.DELFLG = :DELFLG")
                        .Append("   AND URS.PRESENCECATEGORY = :PRESENCECATEGORY2")
                        .Append("   AND URS.PRESENCEDETAIL IN(:PRESENCEDETAIL2 , :PRESENCEDETAIL3)")
                        ' $03 start 納車作業ステータス対応
                        .Append("   AND VS.VISITSTATUS = :VISITSTATUS11")
                        ' $03 end   納車作業ステータス対応
                        .Append("   AND VS.SALESSTART BETWEEN :STARTTIME AND :ENDTIME")
                        .Append(" ORDER BY VS.SALESSTART")

                        ' 一時退席中
                    ElseIf staffStatusClass = StaffStatusLeaving Then
                        .Append(" WHERE URS.DLRCD = :DLRCD")
                        .Append("   AND URS.STRCD = :STRCD")
                        .Append("   AND URS.OPERATIONCODE = :OPERATIONCODE")
                        .Append("   AND URS.DELFLG = :DELFLG")
                        .Append("   AND URS.PRESENCECATEGORY = :PRESENCECATEGORY3")
                        .Append(" ORDER BY URS.USERNAME")

                        ' オフライン状態
                    Else
                        .Append(" WHERE URS.DLRCD = :DLRCD")
                        .Append("   AND URS.STRCD = :STRCD")
                        .Append("   AND URS.OPERATIONCODE = :OPERATIONCODE")
                        .Append("   AND URS.DELFLG = :DELFLG")
                        .Append("   AND URS.PRESENCECATEGORY = :PRESENCECATEGORY4")
                        .Append(" ORDER BY URS.USERNAME")
                    End If

                End With
            ' $05 start コード分析対応
            Using query As New DBSelectQuery( _
                    Of SC3100101DataSet.SC3100101StaffStatusDataTable)("SC3100101_033")
                query.CommandText = sql.ToString()
                sql = Nothing
                ' $05 end コード分析対応
                ' バインド変数
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, storeCode)
                query.AddParameterWithTypeValue("OPERATIONCODE", OracleDbType.Int64, OperationCodeSalesStaff)
                query.AddParameterWithTypeValue("DELFLG", OracleDbType.Char, DeleteFlagNotDelete)
                If staffStatusClass = StaffStatusStandby Then
                    query.AddParameterWithTypeValue("PRESENCECATEGORY1", OracleDbType.Char, StaffStatusStandby)
                ElseIf staffStatusClass = StaffStatusNegotiate Then
                    query.AddParameterWithTypeValue("PRESENCECATEGORY2", OracleDbType.Char, StaffStatusNegotiate)
                    query.AddParameterWithTypeValue("PRESENCEDETAIL0", OracleDbType.Char, PresenceDetail0)
                    query.AddParameterWithTypeValue("PRESENCEDETAIL1", OracleDbType.Char, PresenceDetail1)
                    query.AddParameterWithTypeValue("VISITSTATUS07", OracleDbType.Char, VisitStatusNegotiate)
                    query.AddParameterWithTypeValue("STARTTIME", OracleDbType.Date, startTime)
                    query.AddParameterWithTypeValue("ENDTIME", OracleDbType.Date, endTime)
                ElseIf staffStatusClass = StaffStatusDeliverly Then
                    query.AddParameterWithTypeValue("PRESENCECATEGORY2", OracleDbType.Char, StaffStatusNegotiate)
                    query.AddParameterWithTypeValue("PRESENCEDETAIL2", OracleDbType.Char, PresenceDetail2)
                    query.AddParameterWithTypeValue("PRESENCEDETAIL3", OracleDbType.Char, PresenceDetail3)
                    ' $03 start 納車作業ステータス対応
                    query.AddParameterWithTypeValue("VISITSTATUS11", OracleDbType.Char, VisitStatusDeliverlyStart)
                    ' $03 end   納車作業ステータス対応
                    query.AddParameterWithTypeValue("STARTTIME", OracleDbType.Date, startTime)
                    query.AddParameterWithTypeValue("ENDTIME", OracleDbType.Date, endTime)
                ElseIf staffStatusClass = StaffStatusLeaving Then
                    query.AddParameterWithTypeValue("PRESENCECATEGORY3", OracleDbType.Char, StaffStatusLeaving)
                Else
                    query.AddParameterWithTypeValue("PRESENCECATEGORY4", OracleDbType.Char, StaffStatusOffline)
                End If

                ' SQLの実行
                dt = query.GetData()

            End Using

            ' 検索結果返却
            Return dt

        End Function
#End Region

#Region "接客不要更新"

        ''' <summary>
        ''' 接客不要更新
        ''' </summary>
        ''' <param name="visitSequence">来店実績連番</param>
        ''' <param name="updateAccount">更新アカウント</param>
        ''' <returns>処理結果</returns>
        ''' <remarks></remarks>
        Public Function UpdateUnNecessary(ByVal visitSequence As Long, _
                                            ByVal updateAccount As String) As Boolean

            'DbUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("SC3100101_034")

                'SQL組み立て
                Dim sql As New StringBuilder
                With sql
                    .Append(" UPDATE /* SC3100101_034 */")
                    .Append("        TBL_VISIT_SALES")
                    .Append("    SET")
                    .Append("        UNNECESSARYCOUNT = CASE WHEN UNNECESSARYCOUNT = 99 THEN 99")
                    .Append("                                ELSE UNNECESSARYCOUNT + 1")
                    .Append("                           END")
                    .Append("      , UNNECESSARYDATE = SYSDATE")
                    .Append("      , VISITSTATUS = :VISITSTATUS10")
                    .Append("      , UPDATEACCOUNT = :UPDATEACCOUNT")
                    .Append("      , UPDATEDATE = SYSDATE")
                    .Append("  WHERE VISITSEQ = :VISITSEQ")
                End With

                query.CommandText = sql.ToString()
                sql = Nothing

                query.AddParameterWithTypeValue("VISITSTATUS10", OracleDbType.Char, VisitStatusUnnecessary)
                query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Long, visitSequence)
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Char, updateAccount)

                'SQL実行（結果を返却）
                If query.Execute() > 0 Then
                    Return True
                Else
                    Return False
                End If
            End Using
        End Function
#End Region
        ' $02 end 新車タブレットショールーム管理機能開発

        ' $03 start 納車アンドン対応
#Region "納車件数"
        '$09 NSK s.natsume TKM Change request development for Next Gen e-CRB (CR075) START
        ' ''' <summary>
        ' ''' 納車数を取得する
        ' ''' </summary>
        ' ''' <param name="dealerCode">販売店コード</param>
        ' ''' <param name="storeCode">店舗コード</param>
        ' ''' <param name="startTime">開始日時</param>
        ' ''' <param name="endTime">終了日時</param>
        ' ''' <returns>アンドンデータテーブル</returns>
        ' ''' <remarks></remarks>
        'Public Function GetDeliverlyCount(ByVal dealerCode As String, _
        '                                ByVal storeCode As String, _
        '                                ByVal startTime As Date, _
        '                                ByVal endTime As Date) _
        '                                  As SC3100101DataSet.SC3100101BordCountDataTable
        ''' <summary>
        ''' 納車数を取得する
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="storeCode">店舗コード</param>
        ''' <param name="startTime">開始日時</param>
        ''' <param name="endTime">終了日時</param>
        ''' <param name="afterOrderActionCodeDelivery">納車活動を示す受注後活動コード</param>
        ''' <returns>アンドンデータテーブル</returns>
        ''' <remarks></remarks>
        Public Function GetDeliverlyCount(ByVal dealerCode As String, _
                                        ByVal storeCode As String, _
                                        ByVal startTime As Date, _
                                        ByVal endTime As Date, _
                                        ByVal afterOrderActionCodeDelivery As String) _
                                          As SC3100101DataSet.SC3100101BordCountDataTable
            '$09 NSK s.natsume TKM Change request development for Next Gen e-CRB (CR075) END
            Dim sql As New StringBuilder

            '$09 NSK s.natsume TKM Change request development for Next Gen e-CRB (CR075) START
            'With sql
            '    .Append(" SELECT /* SC3100101_035 */")
            '    .Append("        COUNT(1) AS BORDCOUNT")
            '    .Append("   FROM (")
            '    .Append("     SELECT DISTINCT")
            '    .Append("            CUSTID")
            '    .Append("          , CUSTSEGMENT")
            '    .Append("       FROM TBL_VISIT_SALES")
            '    .Append("      WHERE DLRCD = :DLRCD")
            '    .Append("        AND STRCD = :STRCD")
            '    .Append("        AND SALESSTART BETWEEN :STARTTIME")
            '    .Append("                           AND :ENDTIME")
            '    .Append("        AND VISITSTATUS IN (:VISITSTATUS11, :VISITSTATUS12)")
            '    .Append("        )")
            'End With
            With sql
                .Append("SELECT /* SC3100101_035 */")
                .Append("		SUM(CNT) AS BORDCOUNT ")
                .Append(" FROM (")
                .Append("	SELECT ")
                .Append("        COUNT(1) AS CNT")
                .Append("	FROM TBL_VISIT_SALES VS ")
                .Append("     WHERE EXISTS ( ")
                .Append("				SELECT 1 ")
                .Append("				FROM  TB_T_AFTER_ODR AO ")
                .Append("					, TB_T_AFTER_ODR_ACT AOA")
                .Append("				WHERE VS.FLLWUPBOX_SEQNO = AO.SALES_ID ")
                .Append("				  AND AO.AFTER_ODR_ID = AOA.AFTER_ODR_ID ")
                .Append("				  AND AOA.RSLT_END_DATEORTIME BETWEEN :STARTTIME AND :ENDTIME ")
                .Append("				  AND AOA.AFTER_ODR_ACT_CD = :AFTERODRACT ")
                .Append("	    		 )")
                .Append("        AND VS.DLRCD = :DLRCD ")
                .Append("        AND VS.STRCD = :STRCD ")
                .Append("        AND VS.VISITTIMESTAMP BETWEEN :STARTTIME AND :ENDTIME ")
                .Append("        AND VS.SALESSTART BETWEEN :STARTTIME AND :ENDTIME ")
                .Append("    UNION ALL")
                .Append("    SELECT ")
                .Append("        COUNT(1) AS CNT")
                .Append("	FROM TBL_VISIT_SALES VS ")
                .Append("    WHERE EXISTS ( ")
                .Append("	  			SELECT 1 ")
                .Append("	  			FROM  TB_H_AFTER_ODR AO ")
                .Append("	  				, TB_H_AFTER_ODR_ACT AOA")
                .Append("	  			WHERE VS.FLLWUPBOX_SEQNO = AO.SALES_ID ")
                .Append("	  			  AND AO.AFTER_ODR_ID = AOA.AFTER_ODR_ID ")
                .Append("	  			  AND AOA.RSLT_END_DATEORTIME BETWEEN :STARTTIME AND :ENDTIME ")
                .Append("	    		  AND AOA.AFTER_ODR_ACT_CD = :AFTERODRACT ")
                .Append("	    		 )")
                .Append("        AND VS.DLRCD = :DLRCD ")
                .Append("        AND VS.STRCD = :STRCD ")
                .Append("        AND VS.VISITTIMESTAMP BETWEEN :STARTTIME AND :ENDTIME ")
                .Append("        AND VS.SALESSTART BETWEEN :STARTTIME AND :ENDTIME ")
                .Append("	    )")
            End With
            '$09 NSK s.natsume TKM Change request development for Next Gen e-CRB (CR075) END
            'DbSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3100101DataSet.SC3100101BordCountDataTable)("SC3100101_031")
                query.CommandText = sql.ToString()

                'SQLパラメータ設定
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, storeCode)
                query.AddParameterWithTypeValue("STARTTIME", OracleDbType.Date, startTime)
                query.AddParameterWithTypeValue("ENDTIME", OracleDbType.Date, endTime)
                '$09 NSK s.natsume TKM Change request development for Next Gen e-CRB (CR075) START
                'query.AddParameterWithTypeValue("VISITSTATUS11", OracleDbType.Char, VisitStatusDeliverlyStart)
                'query.AddParameterWithTypeValue("VISITSTATUS12", OracleDbType.Char, VisitStatusDeliverlyEnd)
                query.AddParameterWithTypeValue("AFTERODRACT", OracleDbType.NVarchar2, afterOrderActionCodeDelivery)
                '$09 NSK s.natsume TKM Change request development for Next Gen e-CRB (CR075) END
                'SQL実行（結果表を返却）
                Return query.GetData()
            End Using
        End Function


#End Region
        ' $03 end   納車アンドン対応

#Region "受注後工程アイコンの取得"
        ' $06 start
        ''' <summary>
        ''' 受注後工程アイコンを取得する
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <returns>受注後工程アイコンテーブル</returns>
        ''' <remarks></remarks>
        Public Function GetAfterOrderProcessIcon(ByVal dealerCode As String) _
                                          As SC3100101DataSet.SC3100101AfterOrderProcessIconInfoDataTable

            'SQL組み立て
            Dim sql As New StringBuilder
            With sql
                .Append(" SELECT /* SC3100101_036 */ ")
                .Append("        T1.AFTER_ODR_PRCS_CD ")
                .Append("      , T2.ICON_PATH AS ICON_PATH_ON ")
                .Append("      , T3.ICON_PATH AS ICON_PATH_OFF ")
                .Append("      , T4.ICON_PATH AS ICON_PATH_NOT ")
                .Append("   FROM TB_M_AFTER_ODR_PROC T1 ")
                .Append("      , TB_M_IMG_PATH_CONTROL T2 ")
                .Append("      , TB_M_IMG_PATH_CONTROL T3 ")
                .Append("      , TB_M_IMG_PATH_CONTROL T4 ")
                .Append("  WHERE T2.DLR_CD = :DLR_CD ")
                .Append("    AND T2.TYPE_CD = 'AFTER_ODR_PRCS_CD' ")
                .Append("    AND T2.DEVICE_TYPE = '01' ")
                .Append("    AND T2.FIRST_KEY = T1.AFTER_ODR_PRCS_CD ")
                .Append("    AND T2.SECOND_KEY = '20' ")
                .Append("    AND T3.DLR_CD = :DLR_CD ")
                .Append("    AND T3.TYPE_CD = 'AFTER_ODR_PRCS_CD' ")
                .Append("    AND T3.DEVICE_TYPE = '01' ")
                .Append("    AND T3.FIRST_KEY = T1.AFTER_ODR_PRCS_CD ")
                .Append("    AND T3.SECOND_KEY = '21' ")
                .Append("    AND T4.DLR_CD = :DLR_CD ")
                .Append("    AND T4.TYPE_CD = 'AFTER_ODR_PRCS_CD' ")
                .Append("    AND T4.DEVICE_TYPE = '01' ")
                .Append("    AND T4.FIRST_KEY = T1.AFTER_ODR_PRCS_CD ")
                .Append("    AND T4.SECOND_KEY = '22' ")
            End With

            'DbSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3100101DataSet.SC3100101AfterOrderProcessIconInfoDataTable)("SC3100101_036")
                query.CommandText = sql.ToString()

                'SQLパラメータ設定
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)

                'SQL実行（結果表を返却）
                Return query.GetData()
            End Using

        End Function
        ' $06 end
#End Region

        ' $10 NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060) START
#Region "セールス来店実績ローカル件数取得"
        ''' <summary>
        ''' セールス来店実績ローカル件数取得
        ''' </summary>
        ''' <param name="visitSequence">来店実績連番</param>
        ''' <returns>取得件数</returns>
        ''' <remarks></remarks>
        Public Function GetVisitSalesLocalCount(ByVal visitSequence As Long) As Integer
            'DbSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3100101DataSet.VisitSalesLocalCountDataTable)("SC3100101_037")

                'SQL組み立て
                Dim sql As New StringBuilder
                With sql
                    .Append(" SELECT /* SC3100101_037 */")
                    .Append("        COUNT(1) AS LOCALCOUNT")
                    .Append("   FROM TBL_LC_VISIT_SALES ")
                    .Append("  WHERE VISITSEQ = :VISITSEQ ")
                End With

                query.CommandText = sql.ToString()

                'SQLパラメータ設定
                query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Long, visitSequence)

                'SQL実行（結果を返却）
                Return CInt(query.GetData().Item(0)(0))
            End Using
        End Function
#End Region

#Region "電話番号追加"
        ''' <summary>
        ''' セールス来店実績ローカル件数取得
        ''' </summary>
        ''' <param name="visitSequence">来店実績連番</param>
        ''' <returns>処理結果</returns>
        ''' <remarks></remarks>
        Public Function InsertTelNo(ByVal visitSequence As Long, _
                                    ByVal telNumber As String, _
                                            ByVal updateAccount As String) As Boolean
            'DbUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("SC3100101_038")

                'SQL組み立て
                Dim sql As New StringBuilder
                With sql
                    .Append(" INSERT /* SC3100101_038 */")
                    .Append("   INTO TBL_LC_VISIT_SALES (")
                    .Append("        VISITSEQ")
                    .Append("      , TELNO")
                    .Append("      , CREATEDATE")
                    .Append("      , UPDATEDATE")
                    .Append("      , CREATEACCOUNT")
                    .Append("      , UPDATEACCOUNT")
                    .Append("      , CREATEID")
                    .Append("      , UPDATEID")
                    .Append(" )")
                    .Append(" VALUES (")
                    .Append("        :VISITSEQ")
                    .Append("      , :TELNO")
                    .Append("      , SYSDATE")
                    .Append("      , SYSDATE")
                    .Append("      , :CREATEACCOUNT")
                    .Append("      , :UPDATEACCOUNT")
                    .Append("      , :CREATEID")
                    .Append("      , :UPDATEID")
                    .Append(" )")
                End With

                query.CommandText = sql.ToString()

                'SQLパラメータ設定
                query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Long, visitSequence)
                query.AddParameterWithTypeValue("TELNO", OracleDbType.NVarchar2, telNumber)
                query.AddParameterWithTypeValue("CREATEACCOUNT", OracleDbType.Varchar2, updateAccount)
                query.AddParameterWithTypeValue("CREATEID", OracleDbType.Varchar2, UpdateId)
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, updateAccount)
                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, UpdateId)

                'SQL実行（結果を返却）
                If 0 < query.Execute() Then
                    Return True
                Else
                    Return False
                End If
            End Using
        End Function
#End Region

#Region "電話番号更新"

        ''' <summary>
        ''' セールス来店実績ローカル件数取得
        ''' </summary>
        ''' <param name="visitSequence">来店実績連番</param>
        ''' <param name="telNumber">電話番号</param>
        ''' <returns>処理結果</returns>
        ''' <remarks></remarks>
        Public Function UpdateTelNo(ByVal visitSequence As Long, _
                                    ByVal telNumber As String, _
                                            ByVal updateAccount As String) As Boolean
            'DbUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("SC3100101_039")

                'SQL組み立て
                Dim sql As New StringBuilder
                With sql
                    .Append(" UPDATE /* SC3100101_039 */")
                    .Append("        TBL_LC_VISIT_SALES")
                    .Append("    SET TELNO = :TELNO")
                    .Append("      , UPDATEDATE = SYSDATE")
                    .Append("      , UPDATEACCOUNT = :UPDATEACCOUNT")
                    .Append("      , UPDATEID = :UPDATEID")
                    .Append("  WHERE VISITSEQ = :VISITSEQ")
                End With

                query.CommandText = sql.ToString()

                'SQLパラメータ設定
                query.AddParameterWithTypeValue("TELNO", OracleDbType.NVarchar2, telNumber)
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, updateAccount)
                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, UpdateId)
                query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Long, visitSequence)

                'SQL実行（結果を返却）
                If 0 < query.Execute() Then
                    Return True
                Else
                    Return False
                End If
            End Using
        End Function
#End Region
        ' $10 NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060) END
    End Class
End Namespace

Partial Class SC3100101DataSet
End Class
