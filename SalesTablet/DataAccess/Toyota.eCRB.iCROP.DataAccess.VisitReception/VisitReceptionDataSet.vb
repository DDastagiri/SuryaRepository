'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'VisitReceptionDataSet.vb
'──────────────────────────────────
'機能： 受付共通
'補足： 
'作成： 2012/02/06 KN t.mizumoto
'更新： 2012/08/23 TMEJ m.okamura   新車受付機能改善 $01
'更新： 2012/08/23 TMEJ t.shimamura 【開発】IT9565_新車受付係りによる来店チップ作成機能開発 $02
'更新： 2013/09/05 TMEJ t.shimamura 新車受付係りによる来店チップ作成機能開発 $03
'更新： 2013/10/15 TMEJ m.asano     次世代e-CRBセールス機能 新DB適応に向けた機能開発 $04
'更新： 2014/01/21 TMEJ t.shimamura 顧客管理運用性向上開発 $05
'更新： 2014/06/23 TMEJ m.asano     TMT UAT課題対応 $06
'更新： 2015/11/10 TMEJ t.komure    (トライ店システム評価)SMBチップ検索の絞り込み方法変更 $07
'更新： 2018/07/19 NSK a.kani TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示
'更新： 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証
'──────────────────────────────────

Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core

' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証 START
'Imports Toyota.eCRB.Visit.Api.DataAccess.VisitUtilityDataSet
'Imports Toyota.eCRB.Visit.Api.DataAccess.VisitUtilityDataSetTableAdapters
' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証 END

Imports System.Globalization

Namespace VisitReceptionDataSetTableAdapters

    ''' <summary>
    ''' 来店機能の受付処理共通テーブルアダプター
    ''' </summary>
    ''' <remarks></remarks>
    Public Class VisitReceptionTableAdapter
        Inherits Global.System.ComponentModel.Component

#Region "定数"

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

        ''' <summary>
        ''' 来店実績ステータス（来店キャンセル）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const VisitStatusCancel As String = "99"

        ''' <summary>
        ''' スタッフステータス（商談中）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const StaffStatusNegotiate As String = "2"

        ''' <summary>
        ''' スタッフステータス（スタンバイ）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const StaffStatusStandby As String = "1"

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

        ''' <summary>
        ''' 操作権限コード（セールススタッフ）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const OperationCodeSalesStaff As Long = 8

        ''' <summary>
        ''' CR活動実績（制約）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ActionResultSuccess As String = "3"

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
        ''' 顧客種別（所有者）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const CustomerClassOwner As String = "1"

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
        ''' 通知ステータス(依頼)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const NoticeStatus As String = "1"

        ''' <summary>
        ''' 通知ステータス(受信)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ReceiveStatus As String = "3"

        ''' <summary>
        ''' 来店回数取得(6:来店)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ContactNoVisit As Long = 6

        ''' <summary>
        ''' 処理区分(3:Success/Give-Uo以外)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const RegistrationType As String = "3"

        '$03 start

        ''' <summary>
        ''' 検索タイプ：車両登録No
        ''' </summary>
        ''' <remarks></remarks>
        Private Const SearchTypeRegNumber As String = "1"

        ''' <summary>
        ''' 検索タイプ：顧客名称
        ''' </summary>
        ''' <remarks></remarks>
        Private Const SearchTypeCustomerName As String = "2"

        ''' <summary>
        ''' 検索タイプ：VIN
        ''' </summary>
        ''' <remarks></remarks>
        Private Const SearchTypeVin As String = "3"

        ''' <summary>
        ''' 検索タイプ：電話番号/携帯番号
        ''' </summary>
        ''' <remarks></remarks>
        Private Const SearchTypeTelephoneNumber As String = "4"

        Private Const GateKeeperID As String = "SC3090301"

        ''' <summary>
        ''' ソートタイプ：GK
        ''' </summary>
        ''' <remarks></remarks>
        Private Const SortTypeGateKeeper As String = "1"

        '$03 end

        '04 start 国民ID検索
        ''' <summary>
        ''' 検索タイプ：国民ID
        ''' </summary>
        ''' <remarks></remarks>
        Private Const SearchTypeSocialNumber As String = "5"
        ' $04 end 国民ID検索

        '2018/07/19 NSK a.kani TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
        ''' <summary>
        ''' アイコンの非表示フラグ（0：非表示）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const IconFlagOff As String = "0"
        '2018/07/19 NSK a.kani TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
#End Region

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
                                     ByVal completeDate As Date) As VisitReceptionDataSet.VisitReceptionClaimInfoDataTable

            'SQL組み立て
            Dim sql As New StringBuilder
            With sql
                .Append(" SELECT /* VisitReception_015 */")
                .Append("        DISTINCT VS.VISITSEQ")
                .Append("   FROM TBL_VISIT_SALES VS")
                .Append("      , TBL_CLM_COMPLAINT CLM")
                .Append("      , TBL_CLM_COMPLAINTDETAIL CLMD")
                .Append("  WHERE VS.CUSTID = CLM.INSDID")
                .Append("    AND VS.CUSTSEGMENT = CLM.CSTKIND")
                .Append("    AND CLM.COMPLAINTNO = CLMD.COMPLAINTNO")
                .Append("    AND VS.DLRCD = :DLRCD")
                .Append("    AND VS.STRCD = :STRCD")
                .Append("    AND VISITSTATUS IN (:VISITSTATUS1, :VISITSTATUS2, :VISITSTATUS3, ")
                .Append("                        :VISITSTATUS4, :VISITSTATUS5, :VISITSTATUS6, ")
                ' $01 start 複数顧客に対する商談平行対応
                .Append("                        :VISITSTATUS7, :VISITSTATUS9)")
                .Append("    AND NVL(VS.STOPTIME, NVL(VS.VISITTIMESTAMP, VS.SALESSTART)) BETWEEN :STARTTIME")
                .Append("                                                                    AND :ENDTIME")
                ' $01 end   複数顧客に対する商談平行対応
                .Append("    AND CLM.RELATIONFLG IN (:RELATIONFLG1, :RELATIONFLG2)")
                .Append("    AND CLMD.COMPLAINTSEQ = (")
                .Append("     SELECT")
                .Append("            MAX(CLMDM.COMPLAINTSEQ)")
                .Append("       FROM TBL_CLM_COMPLAINTDETAIL CLMDM")
                .Append("      WHERE CLM.COMPLAINTNO = CLMDM.COMPLAINTNO")
                .Append("                            )")
                .Append("    AND (")
                .Append("            CLM.STATUS IN (:CLAIMSTATUS1, :CLAIMSTATUS2)")
                .Append("         OR (")
                .Append("            CLM.STATUS = :CLAIMSTATUS3")
                .Append("        AND CLMD.FIRST_LAST_ANSWER = :CLAIMSTATUS2")
                .Append("        AND CLMD.ACTUAL_DATE >= :ACTUALDATE")
                .Append("            )")
                .Append("        )")
            End With

            'DbSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of VisitReceptionDataSet.VisitReceptionClaimInfoDataTable)("VisitReception_016")
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
                ' $01 start 複数顧客に対する商談平行対応
                query.AddParameterWithTypeValue("VISITSTATUS9", OracleDbType.Char, VisitStatusNegotiateStop)
                ' $01 end   複数顧客に対する商談平行対応

                query.AddParameterWithTypeValue("CLAIMSTATUS1", OracleDbType.Char, ClaimStatusFirst)
                query.AddParameterWithTypeValue("CLAIMSTATUS2", OracleDbType.Char, ClaimStatusLast)
                query.AddParameterWithTypeValue("CLAIMSTATUS3", OracleDbType.Char, ClaimStatusComplete)

                query.AddParameterWithTypeValue("RELATIONFLG1", OracleDbType.Char, RelationFlgOff)
                query.AddParameterWithTypeValue("RELATIONFLG2", OracleDbType.Char, RelationFlgOn)

                'SQL実行（結果表を返却）
                Return query.GetData()
            End Using

        End Function
#End Region

#Region "来店組数の取得"

        ''' <summary>
        ''' 来店組数の取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="storeCode">店舗コード</param>
        ''' <param name="startTime">開始日時</param>
        ''' <param name="endTime">終了日時</param>
        ''' <param name="visitStatus">来店実績ステータス</param>
        ''' <returns>来店組数データテーブル</returns>
        ''' <remarks></remarks>
        Public Function GetVisitorCount(ByVal dealerCode As String, ByVal storeCode As String, _
                                        ByVal startTime As Date, ByVal endTime As Date, _
                                        ByVal visitStatus As List(Of String)) _
                                        As VisitReceptionDataSet.VisitReceptionVisitorCountDataTable

            '来店実績ステータスSQL組み立て
            Dim sqlVisitStatus As New StringBuilder
            Dim isFirst As Boolean = True
            For Each status As String In visitStatus

                If isFirst Then
                    isFirst = False
                Else
                    sqlVisitStatus.Append(",")
                End If

                sqlVisitStatus.Append("'")
                sqlVisitStatus.Append(status)
                sqlVisitStatus.Append("'")
            Next

            'SQL組み立て
            Dim sql As New StringBuilder
            With sql
                .Append(" SELECT /* VisitReception_001 */")
                .Append("        COUNT(1) AS VISITORCOUNT")
                .Append("   FROM TBL_VISIT_SALES")
                .Append("  WHERE DLRCD = :DLRCD")
                .Append("    AND STRCD = :STRCD")
                .Append("                       ")
                .Append("    AND VISITSTATUS IN (")
                .Append(sqlVisitStatus)
                .Append("                       )")
                .Append("    AND VISITTIMESTAMP BETWEEN :STARTTIME")
                .Append("                           AND :ENDTIME")
            End With

            'DbSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of VisitReceptionDataSet.VisitReceptionVisitorCountDataTable)("VisitReception_001")
                query.CommandText = sql.ToString()

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

#Region "実績件数の取得"

        ''' <summary>
        ''' 実績件数の取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="storeCode">店舗コード</param>
        ''' <param name="startTime">開始日時</param>
        ''' <param name="endTime">終了日時</param>
        ''' <returns>実績件数データテーブル</returns>
        ''' <remarks></remarks>
        Public Function GetResultCount(ByVal dealerCode As String, ByVal storeCode As String, _
                                       ByVal startTime As Date, ByVal endTime As Date) _
                                       As VisitReceptionDataSet.VisitReceptionResultCountDataTable

            'SQL組み立て
            Dim sql As New StringBuilder
            With sql
                .Append(" SELECT /* VisitReception_002 */")
                .Append("        COUNT(1) AS RESULTCOUNT")
                .Append("   FROM (")
                .Append("     SELECT DISTINCT")
                .Append("            CUSTID")
                .Append("          , CUSTSEGMENT")
                .Append("       FROM TBL_VISIT_SALES")
                .Append("      WHERE DLRCD = :DLRCD")
                .Append("        AND STRCD = :STRCD")
                .Append("        AND SALESSTART BETWEEN :STARTTIME")
                .Append("                           AND :ENDTIME")
                .Append("        AND VISITSTATUS IN (:VISITSTATUS1, :VISITSTATUS2)")
                .Append("   )")
            End With

            'DbSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of VisitReceptionDataSet.VisitReceptionResultCountDataTable)("VisitReception_002")
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

#Region "成約件数の取得"

        ''' <summary>
        ''' 成約件数の取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="storeCode">店舗コード</param>
        ''' <param name="startTime">開始日時</param>
        ''' <param name="endTime">終了日時</param>
        ''' <returns>成約件数データテーブル</returns>
        ''' <remarks></remarks>
        Public Function GetConclusionCount(ByVal dealerCode As String, ByVal storeCode As String, _
                                          ByVal startTime As Date, ByVal endTime As Date) _
                                          As VisitReceptionDataSet.VisitReceptionConclusionCountDataTable
            'SQL組み立て
            Dim sql As New StringBuilder
            With sql
                .Append(" SELECT /* VisitReception_003 */")
                .Append("        COUNT(1) AS CONCLUSIONCOUNT")
                .Append("   FROM TBL_FLLWUPBOXTALLY FLW")
                .Append("      , TBL_FLBOX_SUCS_SRES_TLY CAR")
                .Append("  WHERE FLW.DLRCD = CAR.DLRCD")
                .Append("    AND FLW.STRCD = CAR.STRCD")
                .Append("    AND FLW.FLLWUPBOX_SEQNO = CAR.FLLWUPBOX_SEQNO")
                .Append("    AND FLW.DLRCD = :DLRCD")
                .Append("    AND FLW.BRANCH_PLAN = :BRANCH_PLAN")
                .Append("    AND FLW.CRACTRESULT = :CRACTRESULT")
                .Append("    AND FLW.FINSHCRACTIVEDATE BETWEEN :STARTTIME")
                .Append("                                  AND :ENDTIME")
                .Append("    AND FLW.DELFLG = :DELFLG")
            End With

            'DbSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of VisitReceptionDataSet.VisitReceptionConclusionCountDataTable)("VisitReception_003")
                query.CommandText = sql.ToString()

                'SQLパラメータ設定
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("BRANCH_PLAN", OracleDbType.Char, storeCode)
                query.AddParameterWithTypeValue("CRACTRESULT", OracleDbType.Char, ActionResultSuccess)
                query.AddParameterWithTypeValue("STARTTIME", OracleDbType.Date, startTime)
                query.AddParameterWithTypeValue("ENDTIME", OracleDbType.Date, endTime)
                query.AddParameterWithTypeValue("DELFLG", OracleDbType.Char, DeleteFlagNotDelete)

                'SQL実行（結果表を返却）
                Return query.GetData()
            End Using
        End Function
#End Region

#Region "通知依頼件数の取得"

        ''' <summary>
        ''' 通知依頼件数の取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="storeCode">店舗コード</param>
        ''' <param name="startDate">開始日時</param>
        ''' <param name="endDate">終了日時</param>
        ''' <param name="noticeRequest">通知依頼種別</param>
        ''' <param name="statusList">最終ステータスリスト</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetNoticeRequestCount(ByVal dealerCode As String, ByVal storeCode As String, _
                                                 ByVal startDate As Date, ByVal endDate As Date, _
                                                 ByVal noticeRequest As String, ByVal statusList As List(Of String)) _
                                                 As VisitReceptionDataSet.VisitReceptionNoticeRequestCountDataTable

            '来店実績ステータスSQL組み立て
            Dim sqlStatus As New StringBuilder
            Dim isFirst As Boolean = True
            For Each status As String In statusList

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
                .Append("SELECT /* VisitReception_009 */")
                .Append("       COUNT(1) AS NOTICEREQUESTCOUNT")
                .Append("  FROM (")
                .Append("    SELECT")
                .Append("           VS.VISITSEQ")
                .Append("      FROM TBL_VISIT_SALES VS")
                .Append("         , TBL_USERS US")
                .Append("         , TBL_NOTICEREQUEST NR")
                .Append("         , TBL_NOTICEINFO NI")
                .Append("     WHERE VS.ACCOUNT = US.ACCOUNT")
                .Append("       AND VS.DLRCD = NR.DLRCD")
                .Append("       AND VS.STRCD = NR.STRCD")
                .Append("       AND VS.ACCOUNT = NI.FROMACCOUNT")
                .Append("       AND (VS.ACCOUNT <> NI.TOACCOUNT")
                .Append("        OR NI.TOACCOUNT IS NULL)")
                .Append("       AND VS.CUSTID = NR.CRCUSTID")
                .Append("       AND VS.CUSTSEGMENT = NR.CSTKIND")
                .Append("       AND NR.NOTICEREQID = NI.NOTICEREQID")
                .Append("       AND VS.DLRCD = :DLRCD")
                .Append("       AND VS.STRCD = :STRCD")
                .Append("       AND VS.SALESSTART BETWEEN :STARTTIME")
                .Append("                             AND :ENDTIME")
                .Append("       AND NI.SENDDATE >= VS.SALESSTART")
                .Append("       AND VS.VISITSTATUS = :VISITSTATUS7")
                .Append("       AND US.DELFLG = :DELFLG")
                .Append("       AND US.OPERATIONCODE = :OPERATIONCODE")
                .Append("       AND NR.NOTICEREQCTG = :NOTICEREQCTG")
                .Append("       AND NR.CUSTOMERCLASS = :CUSTOMERCLASS")
                .Append("       AND NR.STATUS IN (")
                .Append(sqlStatus.ToString())
                .Append("                        )")
                .Append("       AND NI.STATUS = :STATUSREQUEST")
                .Append("     GROUP BY VS.VISITSEQ")
                .Append("       )")
            End With
            'DbSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of VisitReceptionDataSet.VisitReceptionNoticeRequestCountDataTable)("VisitReception_009")
                query.CommandText = sql.ToString()

                'SQLパラメータ設定
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, storeCode)
                query.AddParameterWithTypeValue("STARTTIME", OracleDbType.Date, startDate)
                query.AddParameterWithTypeValue("ENDTIME", OracleDbType.Date, endDate)
                query.AddParameterWithTypeValue("VISITSTATUS7", OracleDbType.Char, VisitStatusNegotiate)
                query.AddParameterWithTypeValue("DELFLG", OracleDbType.Char, DeleteFlagNotDelete)
                query.AddParameterWithTypeValue("OPERATIONCODE", OracleDbType.Long, OperationCodeSalesStaff)
                query.AddParameterWithTypeValue("NOTICEREQCTG", OracleDbType.Char, noticeRequest)
                query.AddParameterWithTypeValue("CUSTOMERCLASS", OracleDbType.Char, CustomerClassOwner)
                query.AddParameterWithTypeValue("STATUSREQUEST", OracleDbType.Char, LastStatusAssessment)

                'SQL実行（結果表を返却）
                Return query.GetData()
            End Using
        End Function
#End Region

#Region "来店状況の取得"

        ''' <summary>
        ''' 来店状況の取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="storeCode">店舗コード</param>
        ''' <param name="startTime">開始日時</param>
        ''' <param name="endTime">終了日時</param>
        ''' <param name="visitStatus">来店実績ステータス</param>
        ''' <returns>来店状況データテーブル</returns>
        ''' <remarks></remarks>
        Public Function GetVisitorSituation(ByVal dealerCode As String, _
                                            ByVal storeCode As String, _
                                            ByVal startTime As Date, _
                                            ByVal endTime As Date, _
                                            ByVal visitStatus As List(Of String)) _
                                            As VisitReceptionDataSet.VisitReceptionVisitorSituationDataTable
            '来店実績ステータスSQL組み立て
            Dim sqlVisitStatus As New StringBuilder
            Dim isFirst As Boolean = True
            For Each status As String In visitStatus

                If isFirst Then
                    isFirst = False
                Else
                    sqlVisitStatus.Append(",")
                End If

                sqlVisitStatus.Append("'")
                sqlVisitStatus.Append(status)
                sqlVisitStatus.Append("'")
            Next

            'SQL組み立て
            Dim sql As New StringBuilder
            With sql
                .Append(" SELECT /* VisitReception_004 */")
                .Append("        VS.VISITSEQ")
                .Append("      , VS.VISITTIMESTAMP")
                .Append("      , VS.VCLREGNO")
                .Append("      , VS.VISITPERSONNUM")
                .Append("      , VS.VISITMEANS")
                .Append("      , VS.VISITSTATUS")
                .Append("      , VS.SALESTABLENO")
                .Append("      , US1.ORG_IMGFILE")
                .Append("      , US2.USERNAME")
                .Append("      , CASE ")
                .Append("            WHEN VS.CUSTSEGMENT = :CUSTSEGMENT_JI THEN CU.NAME")
                .Append("            WHEN VS.CUSTSEGMENT = :CUSTSEGMENT_MI THEN NC.NAME")
                .Append("            ELSE VS.TENTATIVENAME")
                .Append("        END AS CUSTNAME")
                .Append("      , CASE ")
                .Append("            WHEN VS.CUSTSEGMENT = :CUSTSEGMENT_JI THEN CU.NAMETITLE")
                .Append("            WHEN VS.CUSTSEGMENT = :CUSTSEGMENT_MI THEN NC.NAMETITLE")
                .Append("            ELSE NULL")
                .Append("        END AS CUSTNAMETITLE")
                .Append("      , CASE ")
                .Append("            WHEN VS.CUSTSEGMENT = :CUSTSEGMENT_JI THEN CA.IMAGEFILE_S")
                .Append("            WHEN VS.CUSTSEGMENT = :CUSTSEGMENT_MI THEN NC.IMAGEFILE_S")
                .Append("            ELSE NULL")
                .Append("        END AS CUSTIMAGEFILE")
                .Append("      , VS.CUSTSEGMENT")
                ' $01 start 複数顧客に対する商談平行対応
                .Append("      , VS.STOPTIME")
                ' $01 end   複数顧客に対する商談平行対応
                .Append("   FROM TBL_VISIT_SALES VS")
                .Append("      , TBL_USERS US1")
                .Append("      , TBL_USERS US2")
                .Append("      , TBLORG_CUSTOMER CU")
                .Append("      , TBLORG_CUSTOMER_APPEND CA")
                .Append("      , TBL_NEWCUSTOMER NC")
                .Append("  WHERE VS.ACCOUNT = US1.ACCOUNT(+)")
                .Append("    AND VS.STAFFCD = US2.ACCOUNT(+)")
                .Append("    AND VS.CUSTID = CU.ORIGINALID(+)")
                .Append("    AND CU.ORIGINALID = CA.ORIGINALID(+)")
                .Append("    AND VS.CUSTID = NC.CSTID(+)")
                .Append("    AND US1.OPERATIONCODE(+) = :OPERATIONCODE")
                .Append("    AND US1.DELFLG(+) = :DELFLG")
                .Append("    AND US2.OPERATIONCODE(+) = :OPERATIONCODE")
                .Append("    AND US2.DELFLG(+) = :DELFLG")
                .Append("    AND CU.DELFLG(+) = :DELFLG")
                .Append("    AND NC.DELFLG(+) = :DELFLG")
                .Append("    AND VS.DLRCD = :DLRCD")
                .Append("    AND VS.STRCD = :STRCD")
                ' $01 start 複数顧客に対する商談平行対応
                .Append("    AND NVL(VS.STOPTIME, VS.VISITTIMESTAMP) BETWEEN :STARTTIME")
                .Append("                                                AND :ENDTIME")
                ' $01 end   複数顧客に対する商談平行対応
                .Append("    AND VS.VISITSTATUS IN (")
                .Append(sqlVisitStatus.ToString)
                .Append("                          )")
                ' $01 start 複数顧客に対する商談平行対応
                .Append("  ORDER BY NVL(VS.STOPTIME, VS.VISITTIMESTAMP) ASC")
                ' $01 end   複数顧客に対する商談平行対応
            End With

            'DbSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of VisitReceptionDataSet.VisitReceptionVisitorSituationDataTable)("VisitReception_004")
                query.CommandText = sql.ToString()

                'SQLパラメータ設定
                query.AddParameterWithTypeValue("CUSTSEGMENT_JI", OracleDbType.Char, VisitorSegmentOriginal)
                query.AddParameterWithTypeValue("CUSTSEGMENT_MI", OracleDbType.Char, VisitorSegmentNew)
                query.AddParameterWithTypeValue("DELFLG", OracleDbType.Char, DeleteFlagNotDelete)
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, storeCode)
                query.AddParameterWithTypeValue("STARTTIME", OracleDbType.Date, startTime)
                query.AddParameterWithTypeValue("ENDTIME", OracleDbType.Date, endTime)
                query.AddParameterWithTypeValue("OPERATIONCODE", OracleDbType.Long, OperationCodeSalesStaff)

                'SQL実行（結果表を返却）
                Return query.GetData()
            End Using
        End Function
#End Region

#Region "スタッフ情報（商談中）の取得"

        ''' <summary>
        ''' スタッフ情報（商談中）の取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="storeCode">店舗コード</param>
        ''' <param name="startTime">開始日時</param>
        ''' <param name="endTime">終了日時</param>
        ''' <returns>スタッフ情報（商談中）データテーブル</returns>
        ''' <remarks>未決定　スタッフステータスを条件に追加</remarks>
        Public Function GetStaffNegotiate(ByVal dealerCode As String, ByVal storeCode As String, _
                                          ByVal startTime As Date, ByVal endTime As Date) _
                                          As VisitReceptionDataSet.VisitReceptionStaffSituationDataTable
            'SQL組み立て
            Dim sql As New StringBuilder
            With sql
                .Append(" SELECT /* VisitReception_005 */")
                .Append("        US.ACCOUNT")
                .Append("      , VS.VISITSEQ")
                .Append("      , VS.SALESTABLENO")
                .Append("      , VS.SALESSTART")
                .Append("      , US.ORG_IMGFILE")
                .Append("      , US.USERNAME")
                .Append("      , CASE ")
                .Append("            WHEN VS.CUSTSEGMENT = :CUSTSEGMENT_JI THEN CU.NAME")
                .Append("            WHEN VS.CUSTSEGMENT = :CUSTSEGMENT_MI THEN NC.NAME")
                .Append("            ELSE VS.TENTATIVENAME")
                .Append("        END AS CUSTNAME")
                .Append("      , CASE ")
                .Append("            WHEN VS.CUSTSEGMENT = :CUSTSEGMENT_JI THEN CU.NAMETITLE")
                .Append("            WHEN VS.CUSTSEGMENT = :CUSTSEGMENT_MI THEN NC.NAMETITLE")
                .Append("            ELSE NULL")
                .Append("        END AS CUSTNAMETITLE")
                .Append("      , CASE ")
                .Append("            WHEN VS.CUSTSEGMENT = :CUSTSEGMENT_JI THEN CA.IMAGEFILE_S")
                .Append("            WHEN VS.CUSTSEGMENT = :CUSTSEGMENT_MI THEN NC.IMAGEFILE_S")
                .Append("            ELSE NULL")
                .Append("        END AS CUSTIMAGEFILE")
                .Append("      , US.PRESENCECATEGORY AS STAFFSTATUS")
                .Append("      , VS.CUSTSEGMENT")
                .Append("   FROM")
                .Append("        TBL_VISIT_SALES VS")
                .Append("      , TBL_USERS US")
                .Append("      , TBLORG_CUSTOMER CU")
                .Append("      , TBLORG_CUSTOMER_APPEND CA")
                .Append("      , TBL_NEWCUSTOMER NC")
                .Append("  WHERE VS.ACCOUNT = US.ACCOUNT")
                .Append("    AND VS.CUSTID = CU.ORIGINALID(+)")
                .Append("    AND CU.ORIGINALID = CA.ORIGINALID(+)")
                .Append("    AND VS.CUSTID = NC.CSTID(+)")
                .Append("    AND CU.DELFLG(+) = :DELFLG")
                .Append("    AND NC.DELFLG(+) = :DELFLG")
                .Append("    AND US.OPERATIONCODE = :OPERATIONCODE")
                .Append("    AND US.DELFLG = :DELFLG")
                .Append("    AND VS.DLRCD = :DLRCD")
                .Append("    AND VS.STRCD = :STRCD")
                .Append("    AND VS.SALESSTART BETWEEN :STARTTIME")
                .Append("                          AND :ENDTIME")
                .Append("    AND VS.VISITSTATUS = :VISITSTATUS")
                .Append("    AND US.PRESENCECATEGORY = :STAFFSTATUS")
                .Append("  ORDER BY VS.SALESSTART ASC, US.USERNAME ASC")
            End With

            'DbSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of VisitReceptionDataSet.VisitReceptionStaffSituationDataTable)("VisitReception_005")
                query.CommandText = sql.ToString()

                'SQLパラメータ設定
                query.AddParameterWithTypeValue("CUSTSEGMENT_JI", OracleDbType.Char, VisitorSegmentOriginal)
                query.AddParameterWithTypeValue("CUSTSEGMENT_MI", OracleDbType.Char, VisitorSegmentNew)
                query.AddParameterWithTypeValue("DELFLG", OracleDbType.Char, DeleteFlagNotDelete)
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, storeCode)
                query.AddParameterWithTypeValue("STARTTIME", OracleDbType.Date, startTime)
                query.AddParameterWithTypeValue("ENDTIME", OracleDbType.Date, endTime)
                query.AddParameterWithTypeValue("VISITSTATUS", OracleDbType.Char, VisitStatusNegotiate)
                query.AddParameterWithTypeValue("STAFFSTATUS", OracleDbType.Char, StaffStatusNegotiate)
                query.AddParameterWithTypeValue("OPERATIONCODE", OracleDbType.Long, OperationCodeSalesStaff)

                'SQL実行（結果表を返却）
                Return query.GetData()
            End Using
        End Function
#End Region

#Region "スタッフ情報（成果）の取得"

        ''' <summary>
        ''' スタッフ情報（成果）の取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="storeCode">店舗コード</param>
        ''' <param name="startTime">開始日時</param>
        ''' <param name="endTime">終了日時</param>
        ''' <param name="staffStatus">スタッフステータス</param>
        ''' <returns>スタッフ情報（成果）データテーブル</returns>
        ''' <remarks>未決定　スタッフステータスを条件に追加</remarks>
        Public Function GetStaffResult(ByVal dealerCode As String, ByVal storeCode As String, _
                                       ByVal startTime As Date, ByVal endTime As Date, _
                                       ByVal staffStatus As String) _
                                       As VisitReceptionDataSet.VisitReceptionStaffSituationDataTable

            ' $01 start スタンバイスタッフ並び順変更対応
            'スタッフステータスがスタンバイであるか
            Dim isStaffStatusStandby As Boolean = (staffStatus = StaffStatusStandby)
            ' $01 end   スタンバイスタッフ並び順変更対応

            'SQL組み立て
            Dim sql As New StringBuilder
            With sql
                .Append(" SELECT /* VisitReception_006 */")
                .Append("        US.ACCOUNT")
                .Append("      , US.ORG_IMGFILE")
                .Append("      , US.USERNAME")
                .Append("      , NVL(VR.KENSU,0) AS RESULTCOUNT")
                .Append("      , NVL(CR.KENSU,0) AS CONCLUSIONCOUNT")
                .Append("      , US.PRESENCECATEGORY AS STAFFSTATUS")
                .Append("   FROM TBL_USERS US")
                .Append("      , (")
                .Append("     SELECT")
                .Append("            ACCOUNT")
                .Append("          , COUNT(DISTINCT RPAD(CUSTID, 19) || CUSTSEGMENT) AS KENSU")
                .Append("       FROM TBL_VISIT_SALES")
                .Append("      WHERE DLRCD = :DLRCD")
                .Append("        AND STRCD = :STRCD")
                .Append("        AND SALESSTART BETWEEN :STARTTIME")
                .Append("                           AND :ENDTIME")
                .Append("        AND VISITSTATUS IN (:VISITSTATUS1, :VISITSTATUS2)")
                .Append("      GROUP BY ACCOUNT")
                .Append("        ) VR")
                .Append("      , (")
                .Append("     SELECT")
                .Append("            FLW.ACCOUNT_PLAN")
                .Append("          , COUNT(1) AS KENSU")
                .Append("       FROM TBL_FLLWUPBOXTALLY FLW")
                .Append("          , TBL_FLBOX_SUCS_SRES_TLY CAR")
                .Append("      WHERE FLW.DLRCD = CAR.DLRCD")
                .Append("        AND FLW.STRCD = CAR.STRCD")
                .Append("        AND FLW.FLLWUPBOX_SEQNO = CAR.FLLWUPBOX_SEQNO")
                .Append("        AND FLW.DLRCD = :DLRCD")
                .Append("        AND FLW.BRANCH_PLAN = :BRANCH_PLAN")
                .Append("        AND FLW.CRACTRESULT = :CRACTRESULT")
                .Append("        AND FLW.FINSHCRACTIVEDATE BETWEEN :STARTTIME")
                .Append("                                      AND :ENDTIME")
                .Append("        AND FLW.DELFLG = :DELFLG")
                .Append("      GROUP BY FLW.ACCOUNT_PLAN")
                .Append("        ) CR")
                ' $01 start スタンバイスタッフ並び順変更対応
                If isStaffStatusStandby Then
                    .Append("      , TBL_STANDBYSTAFF_SORT SS")
                End If
                ' $01 end   スタンバイスタッフ並び順変更対応
                .Append("  WHERE US.ACCOUNT = VR.ACCOUNT(+)")
                .Append("    AND US.ACCOUNT = CR.ACCOUNT_PLAN(+)")
                ' $01 start スタンバイスタッフ並び順変更対応
                If isStaffStatusStandby Then
                    .Append("    AND US.ACCOUNT = SS.ACCOUNT(+)")
                    .Append("    AND US.DLRCD = SS.DLRCD(+)")
                    .Append("    AND US.STRCD = SS.STRCD(+)")
                    .Append("    AND US.PRESENCECATEGORYDATE = SS.PRESENCECATEGORYDATE(+)")
                End If
                ' $01 end   スタンバイスタッフ並び順変更対応
                .Append("    AND US.DLRCD = :DLRCD")
                .Append("    AND US.STRCD = :STRCD")
                .Append("    AND US.OPERATIONCODE = :OPERATIONCODE")
                .Append("    AND US.DELFLG = :DELFLG")
                .Append("    AND US.PRESENCECATEGORY = :STAFFSTATUS")
                ' $01 start スタンバイスタッフ並び順変更対応
                If isStaffStatusStandby Then
                    .Append("  ORDER BY SS.SORTNO ASC, US.PRESENCECATEGORYDATE ASC")
                Else
                    .Append("  ORDER BY NVL(VR.KENSU,0) ASC, US.USERNAME ASC")
                End If
                ' $01 end   スタンバイスタッフ並び順変更対応
            End With

            'DbSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of VisitReceptionDataSet.VisitReceptionStaffSituationDataTable)("VisitReception_006")
                query.CommandText = sql.ToString()

                'SQLパラメータ設定
                query.AddParameterWithTypeValue("STAFFSTATUS", OracleDbType.Char, staffStatus)
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, storeCode)
                query.AddParameterWithTypeValue("STARTTIME", OracleDbType.Date, startTime)
                query.AddParameterWithTypeValue("ENDTIME", OracleDbType.Date, endTime)
                query.AddParameterWithTypeValue("VISITSTATUS1", OracleDbType.Char, VisitStatusNegotiate)
                query.AddParameterWithTypeValue("VISITSTATUS2", OracleDbType.Char, VisitStatusNegotiateEnd)
                query.AddParameterWithTypeValue("BRANCH_PLAN", OracleDbType.Char, storeCode)
                query.AddParameterWithTypeValue("CRACTRESULT", OracleDbType.Char, ActionResultSuccess)
                query.AddParameterWithTypeValue("DELFLG", OracleDbType.Char, DeleteFlagNotDelete)
                query.AddParameterWithTypeValue("OPERATIONCODE", OracleDbType.Long, OperationCodeSalesStaff)
                'SQL実行（結果表を返却）
                Return query.GetData()
            End Using
        End Function
#End Region

#Region "スタッフ情報（オフライン）の取得"

        ''' <summary>
        ''' スタッフ情報（オフライン）の取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="storeCode">店舗コード</param>
        ''' <returns>スタッフ情報（オフライン）データテーブル</returns>
        ''' <remarks>未決定　スタッフステータスを条件に追加</remarks>
        Public Function GetStaffOffline(ByVal dealerCode As String, ByVal storeCode As String) _
                                        As VisitReceptionDataSet.VisitReceptionStaffSituationDataTable
            'SQL組み立て
            Dim sql As New StringBuilder
            With sql
                .Append(" SELECT /* VisitReception_007 */")
                .Append("        US.ACCOUNT")
                .Append("      , US.ORG_IMGFILE")
                .Append("      , US.USERNAME")
                .Append("      , US.PRESENCECATEGORY AS STAFFSTATUS")
                .Append("   FROM TBL_USERS US")
                .Append("  WHERE US.DLRCD = :DLRCD")
                .Append("    AND US.STRCD = :STRCD")
                .Append("    AND US.OPERATIONCODE = :OPERATIONCODE")
                .Append("    AND US.DELFLG = :DELFLG")
                .Append("    AND US.PRESENCECATEGORY = :STAFFSTATUS")
                .Append("  ORDER BY US.USERNAME ASC")
            End With

            'DbSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of VisitReceptionDataSet.VisitReceptionStaffSituationDataTable)("VisitReception_007")
                query.CommandText = sql.ToString()

                'SQLパラメータ設定
                query.AddParameterWithTypeValue("STAFFSTATUS", OracleDbType.Char, StaffStatusOffline)
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, storeCode)
                query.AddParameterWithTypeValue("DELFLG", OracleDbType.Char, DeleteFlagNotDelete)
                query.AddParameterWithTypeValue("OPERATIONCODE", OracleDbType.Long, OperationCodeSalesStaff)

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
                                                   As VisitReceptionDataSet.VisitReceptionVisitorLinkingCountDataTable
            'SQL組み立て
            Dim sql As New StringBuilder
            With sql
                .Append(" SELECT /* VisitReception_010 */")
                .Append("        VS.ACCOUNT")
                .Append("      , COUNT(1) AS VISITORLINKINGCOUNT")
                .Append("   FROM TBL_VISIT_SALES VS")
                .Append("      , TBL_USERS US")
                .Append("  WHERE VS.ACCOUNT = US.ACCOUNT")
                .Append("    AND VS.DLRCD = :DLRCD")
                .Append("    AND VS.STRCD = :STRCD")
                ' $01 start 複数顧客に対する商談平行対応
                .Append("    AND NVL(VS.STOPTIME, VS.VISITTIMESTAMP) BETWEEN :STARTTIME")
                .Append("                                                AND :ENDTIME")
                ' $01 end   複数顧客に対する商談平行対応
                .Append("    AND VS.VISITSTATUS IN (:VISITSTATUS3, :VISITSTATUS4")
                ' $01 start 複数顧客に対する商談平行対応
                .Append("                         , :VISITSTATUS5, :VISITSTATUS6")
                .Append("                         , :VISITSTATUS9)")
                ' $01 end   複数顧客に対する商談平行対応
                .Append("    AND US.OPERATIONCODE = :OPERATIONCODE")
                .Append("    AND US.DELFLG = :DELFLG")
                .Append("  GROUP BY VS.ACCOUNT")
            End With

            'DbSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of VisitReceptionDataSet.VisitReceptionVisitorLinkingCountDataTable)("VisitReception_010")
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

#Region "お客様との紐付け情報の取得"
        ''' <summary>
        ''' お客様との紐付け情報の取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="storeCode">店舗コード</param>
        ''' <param name="startTime">取得開始日時</param>
        ''' <param name="endTime">取得終了日時</param>
        ''' <returns>お客様との紐付け情報データテーブル</returns>
        ''' <remarks></remarks>
        Public Function GetVisitorLinking(ByVal dealerCode As String, ByVal storeCode As String, _
                                          ByVal startTime As Date, ByVal endTime As Date) _
                                             As VisitReceptionDataSet.VisitReceptionVisitorLinkingDataTable
            'SQL組み立て
            Dim sql As New StringBuilder
            With sql
                .Append(" SELECT /* VisitReception_011 */")
                .Append("        VS.ACCOUNT")
                .Append("      , VS.VISITSEQ")
                .Append("      , VS.SALESTABLENO")
                .Append("      , CASE VS.CUSTSEGMENT")
                .Append("            WHEN :CUSTSEGMENT_JI THEN CU.NAME")
                .Append("            WHEN :CUSTSEGMENT_MI THEN NC.NAME")
                .Append("            ELSE VS.TENTATIVENAME ")
                .Append("        END AS CUSTNAME")
                .Append("      , CASE VS.CUSTSEGMENT")
                .Append("            WHEN :CUSTSEGMENT_JI THEN CU.NAMETITLE")
                .Append("            WHEN :CUSTSEGMENT_MI THEN NC.NAMETITLE")
                .Append("            ELSE NULL")
                .Append("        END AS CUSTNAMETITLE")
                .Append("      , CASE VS.CUSTSEGMENT")
                .Append("            WHEN :CUSTSEGMENT_JI THEN CA.IMAGEFILE_S")
                .Append("            WHEN :CUSTSEGMENT_MI THEN NC.IMAGEFILE_S")
                .Append("            ELSE NULL")
                .Append("        END AS CUSTIMAGEFILE")
                .Append("      , VS.CUSTSEGMENT")
                .Append("   FROM")
                .Append("        TBL_VISIT_SALES VS")
                .Append("      , TBL_USERS US")
                .Append("      , TBLORG_CUSTOMER CU")
                .Append("      , TBLORG_CUSTOMER_APPEND CA")
                .Append("      , TBL_NEWCUSTOMER NC")
                .Append("  WHERE VS.ACCOUNT = US.ACCOUNT")
                .Append("    AND VS.CUSTID = CU.ORIGINALID(+)")
                .Append("    AND VS.CUSTID = CA.ORIGINALID(+)")
                .Append("    AND VS.CUSTID = NC.CSTID(+)")
                .Append("    AND VS.DLRCD = :DLRCD")
                .Append("    AND VS.STRCD = :STRCD")
                ' $01 start 複数顧客に対する商談平行対応
                .Append("    AND NVL(VS.STOPTIME, VS.VISITTIMESTAMP) BETWEEN :STARTTIME")
                .Append("                                                AND :ENDTIME")
                ' $01 end   複数顧客に対する商談平行対応
                .Append("    AND VS.VISITSTATUS IN (:VISITSTATUS3, :VISITSTATUS4")
                ' $01 start 複数顧客に対する商談平行対応
                .Append("                         , :VISITSTATUS5, :VISITSTATUS6")
                .Append("                         , :VISITSTATUS9)")
                ' $01 end   複数顧客に対する商談平行対応
                .Append("    AND US.PRESENCECATEGORY IN (:STAFFSTANDBY")
                .Append("                              , :STAFFLEAVING, :STAFFOFFLINE)")
                .Append("    AND US.DELFLG = :DELFLG")
                .Append("    AND US.OPERATIONCODE = :OPERATIONCODE")
                .Append("    AND CU.DELFLG(+) = :DELFLG")
                .Append("    AND NC.DELFLG(+) = :DELFLG")
                ' $01 start 複数顧客に対する商談平行対応
                .Append("  ORDER BY NVL(VS.STOPTIME, VS.VISITTIMESTAMP) ASC")
                ' $01 end   複数顧客に対する商談平行対応
                .Append("         , CUSTNAME ASC")
            End With

            'DbSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of VisitReceptionDataSet.VisitReceptionVisitorLinkingDataTable)("VisitReception_011")
                query.CommandText = sql.ToString()

                'SQLパラメータ設定
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, storeCode)
                query.AddParameterWithTypeValue("STARTTIME", OracleDbType.Date, startTime)
                query.AddParameterWithTypeValue("ENDTIME", OracleDbType.Date, endTime)

                query.AddParameterWithTypeValue("CUSTSEGMENT_JI", OracleDbType.Char, VisitorSegmentOriginal)
                query.AddParameterWithTypeValue("CUSTSEGMENT_MI", OracleDbType.Char, VisitorSegmentNew)

                query.AddParameterWithTypeValue("VISITSTATUS3", OracleDbType.Char, VisitStatusAdjustment)
                query.AddParameterWithTypeValue("VISITSTATUS4", OracleDbType.Char, VisitStatusDecisionBroadcast)
                query.AddParameterWithTypeValue("VISITSTATUS5", OracleDbType.Char, VisitStatusDecision)
                query.AddParameterWithTypeValue("VISITSTATUS6", OracleDbType.Char, VisitStatusWating)
                ' $01 start 複数顧客に対する商談平行対応
                query.AddParameterWithTypeValue("VISITSTATUS9", OracleDbType.Char, VisitStatusNegotiateStop)
                ' $01 end   複数顧客に対する商談平行対応

                query.AddParameterWithTypeValue("STAFFSTANDBY", OracleDbType.Char, StaffStatusStandby)
                query.AddParameterWithTypeValue("STAFFLEAVING", OracleDbType.Char, StaffStatusLeaving)
                query.AddParameterWithTypeValue("STAFFOFFLINE", OracleDbType.Char, StaffStatusOffline)

                query.AddParameterWithTypeValue("DELFLG", OracleDbType.Char, DeleteFlagNotDelete)
                query.AddParameterWithTypeValue("OPERATIONCODE", OracleDbType.Long, OperationCodeSalesStaff)

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
                                              As VisitReceptionDataSet.VisitReceptionNoticeRequestsDataTable
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
                .Append(" SELECT /* VisitReception_012 */")
                .Append("        VS.ACCOUNT")
                .Append("      , NR.NOTICEREQCTG")
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
                .Append("    AND VS.VISITSTATUS = :VISITSTATUS7")
                .Append("    AND US.DELFLG = :DELFLG")
                .Append("    AND US.OPERATIONCODE = :OPERATIONCODE")
                .Append("    AND NR.NOTICEREQCTG = :NOTICEREQCTG")
                .Append("    AND NR.CUSTOMERCLASS = :CUSTOMERCLASS")
                .Append("    AND NR.STATUS IN (")
                .Append(sqlLastStatus)
                .Append("                     )")
                .Append("    AND NI.STATUS = :STATUSREQUEST")
                .Append("  GROUP BY VS.ACCOUNT, NR.NOTICEREQCTG")
            End With

            'DbSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of VisitReceptionDataSet.VisitReceptionNoticeRequestsDataTable)("VisitReception_012")
                query.CommandText = sql.ToString()

                'SQLパラメータ設定
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, storeCode)
                query.AddParameterWithTypeValue("STARTTIME", OracleDbType.Date, startTime)
                query.AddParameterWithTypeValue("ENDTIME", OracleDbType.Date, endTime)
                query.AddParameterWithTypeValue("NOTICEREQCTG", OracleDbType.Char, noticeRequestCategory)

                query.AddParameterWithTypeValue("VISITSTATUS7", OracleDbType.Char, VisitStatusNegotiate)
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
                                           As VisitReceptionDataSet.VisitReceptionVisitorCustomerDataTable
            'SQL組み立て
            Dim sql As New StringBuilder
            With sql
                .Append(" SELECT /* VisitReception_008 */")
                .Append("        VS.VCLREGNO     AS VCLREGNO")
                .Append("      , VS.CUSTSEGMENT  AS CUSTSEGMENT")
                .Append("      , VS.BROUDCASTFLG AS BROUDCASTFLG")
                .Append("      , VS.SALESTABLENO AS SALESTABLENO")
                .Append("      , CASE ")
                .Append("            WHEN VS.CUSTSEGMENT = :CUSTSEGMENT_JI THEN CU.NAME")
                .Append("            WHEN VS.CUSTSEGMENT = :CUSTSEGMENT_MI THEN NC.NAME")
                .Append("            ELSE VS.TENTATIVENAME")
                .Append("        END AS CUSTNAME")
                .Append("      , CASE ")
                .Append("            WHEN VS.CUSTSEGMENT = :CUSTSEGMENT_JI THEN CU.NAMETITLE")
                .Append("            WHEN VS.CUSTSEGMENT = :CUSTSEGMENT_MI THEN NC.NAMETITLE")
                .Append("            ELSE NULL")
                .Append("        END AS CUSTNAMETITLE")
                .Append("      , VS.STAFFCD AS STAFFCD")
                .Append("      , VS.ACCOUNT AS ACCOUNT")
                .Append("      , VS.CUSTID AS CUSTID")
                .Append("      , VS.SALESSTART AS SALESSTART")
                .Append("      , VS.VISITPERSONNUM  AS VISITPERSONNUM")
                .Append("      , VS.FLLWUPBOX_DLRCD AS FLLOWUPBOX_DLRCD")
                .Append("      , VS.FLLWUPBOX_STRCD AS FLLOWUPBOX_STRCD")
                .Append("      , VS.FLLWUPBOX_SEQNO AS FLLOWUPBOX_SEQNO")
                .Append("   FROM TBL_VISIT_SALES VS")
                .Append("      , TBLORG_CUSTOMER CU")
                .Append("      , TBL_NEWCUSTOMER NC")
                .Append("  WHERE VS.CUSTID = CU.ORIGINALID(+)")
                .Append("    AND VS.CUSTID = NC.CSTID(+)")
                .Append("    AND CU.DELFLG(+) = :DELFLG")
                .Append("    AND NC.DELFLG(+) = :DELFLG")
                .Append("    AND VS.VISITSEQ = :VISITSEQ")

                If Not String.IsNullOrEmpty(visitStatus) Then
                    .Append("     AND VS.VISITSTATUS = :VISITSTATUS")
                End If

            End With

            'DbSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of VisitReceptionDataSet.VisitReceptionVisitorCustomerDataTable)("VisitReception_008")
                query.CommandText = sql.ToString()

                'SQLパラメータ設定
                query.AddParameterWithTypeValue("CUSTSEGMENT_JI", OracleDbType.Char, VisitorSegmentOriginal)
                query.AddParameterWithTypeValue("CUSTSEGMENT_MI", OracleDbType.Char, VisitorSegmentNew)
                query.AddParameterWithTypeValue("DELFLG", OracleDbType.Char, DeleteFlagNotDelete)
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
                                              ) As VisitReceptionDataSet.VisitReceptionStaffNoticeRequestDataTable

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
                .Append("SELECT /* VisitReception_014 */")
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
            Using query As New DBSelectQuery(Of VisitReceptionDataSet.VisitReceptionStaffNoticeRequestDataTable)("VisitReception_014")
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
                                      ByVal followUpBoxSeqNo As Long) As VisitReceptionDataSet.VisitReceptionVisitCountDataTable

            'SQL組み立て
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* VisitReception_013 */")
                .Append("       SUM(CNT) AS VISITCOUNT")
                .Append("  FROM (")
                .Append("   (SELECT ")
                .Append("           NVL(COUNT(1),0) CNT ")
                .Append("      FROM TBL_FLLWUPBOXRSLT ")
                .Append("     WHERE DLRCD = :DLRCD ")
                .Append("       AND STRCD = :STRCD ")
                .Append("       AND FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO ")
                .Append("       AND CONTACTNO = :CONTACTNO) ")
                .Append(" UNION ALL")
                .Append("   (SELECT ")
                .Append("           NVL(COUNT(1),0) CNT ")
                .Append("      FROM TBL_FLLWUPBOXRSLT_PAST ")
                .Append("     WHERE DLRCD = :DLRCD ")
                .Append("       AND STRCD = :STRCD ")
                .Append("       AND FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO ")
                .Append("       AND CONTACTNO = :CONTACTNO) ")
                .Append(" UNION ALL")
                .Append("   (SELECT ")
                .Append("           NVL(COUNT(1),0) CNT ")
                .Append("      FROM TBL_WALKINPERSON ")
                .Append("     WHERE DLRCD = :DLRCD ")
                .Append("       AND STRCD = :STRCD ")
                .Append("       AND FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO ")
                .Append("       AND CONTACTNO = :CONTACTNO")
                .Append("       AND REGISTRATIONTYPE <> :REGISTRATIONTYPE")
                .Append("   )")
                ' 受注後工程フォロー
                .Append(" UNION ALL ")
                .Append("   (SELECT ")
                .Append("           NVL(COUNT(1),0) CNT ")
                .Append("      FROM TBL_BOOKEDAFTERFOLLOWRSLT ")
                .Append("     WHERE DLRCD = :DLRCD ")
                .Append("       AND STRCD = :STRCD ")
                .Append("       AND FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO ")
                .Append("       AND CONTACTNO = :CONTACTNO ")
                .Append("   )")
                .Append("       )")
            End With

            'DbSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of VisitReceptionDataSet.VisitReceptionVisitCountDataTable)("VisitReception_013")
                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, storeCode)
                query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Int64, followUpBoxSeqNo)
                query.AddParameterWithTypeValue("CONTACTNO", OracleDbType.Int64, ContactNoVisit)
                query.AddParameterWithTypeValue("REGISTRATIONTYPE", OracleDbType.Char, RegistrationType)

                Return query.GetData()
            End Using

        End Function
#End Region

        '$03 start

#Region "顧客一覧取得"

        ''' <summary>
        ''' 顧客一覧を取得する
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="searchType">検索タイプ(1：車両登録No 、2：顧客名称、 3：VIN、 4：電話番号/携帯番号、5：国民ID)</param>
        ''' <param name="searchText">検索テキスト</param>
        ''' <param name="sortType">1:GK、2:受付</param>
        ''' <returns>顧客情報データテーブル</returns>
        ''' <remarks></remarks>
        Public Function GetCustomerList(ByVal dealerCode As String, _
                                        ByVal searchType As String, _
                                        ByVal searchText As List(Of String),
                                        ByVal sortType As String) As VisitReceptionDataSet.VisitReceptionCustomerListDataTable

            Dim dt As VisitReceptionDataSet.VisitReceptionCustomerListDataTable = Nothing


            ' $04 START 次世代e-CRBセールス機能 新DB適応に向けた機能開発
            ' SQL文作成
            Dim sql As New StringBuilder
            With sql
                .Append(" SELECT /* VisitReception_016 */ ")
                .Append("        CUST.NAME ")
                .Append("      , CUST.NAMETITLE ")
                .Append("      , CUST.CUSTKBN ")
                .Append("      , CUST.TELNO ")
                .Append("      , CUST.MOBILE ")
                .Append("      , CUST.VCLREGNO ")
                .Append("      , CUST.SERIESNM ")
                .Append("      , CUST.EXTERIORNAME ")
                .Append("      , CUST.MAKERNAME ")
                .Append("      , CUST.CUSTCD ")
                .Append("      , CUST.STUFFCD ")
                .Append("      , CUST.STUFFNAME ")
                .Append("      , CUST.VIN ")
                .Append("      , CUST.SEX ")
                .Append("      , CUST.SEQNO ")
                .Append("      , CUST.SACODE ")
                .Append("      , CUST.CUSTYPE ")
                ' $04 start 国民ID検索
                .Append("      , CUST.SOSCIALNUM ")
                ' $04 end 国民ID検索
                '2018/07/19 NSK a.kani TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
                .Append("      , CUST.IMP_VCL_FLG ")
                '2018/07/19 NSK a.kani TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
                .Append(" FROM ( ")
                .Append("         SELECT ")
                .Append("               CST.CST_NAME AS NAME ")
                .Append("             , CST.NAMETITLE_NAME AS NAMETITLE ")
                .Append("             , '1' AS CUSTKBN ")
                .Append("             , CST.CST_PHONE AS TELNO ")
                .Append("             , CST.CST_MOBILE AS MOBILE ")
                .Append("             , VCL_DLR.REG_NUM AS VCLREGNO ")
                .Append("             , MODEL.MODEL_NAME AS SERIESNM ")
                .Append("             , VCL.BODYCLR_NAME AS EXTERIORNAME ")
                .Append("             , Null AS MAKERNAME ")
                .Append("             , CST.CST_ID AS CUSTCD ")
                .Append("             , CST_VCL.SLS_PIC_STF_CD AS STUFFCD ")
                .Append("             , USERS.USERNAME AS STUFFNAME ")
                .Append("             , VCL.VCL_VIN AS VIN ")
                .Append("             , CST.CST_GENDER AS SEX ")
                .Append("             , CST_VCL.SVC_PIC_STF_CD AS SACODE ")
                .Append("             , VCL.VCL_ID AS SEQNO ")
                .Append("             , CST.FLEET_FLG AS CUSTYPE ")
                ' $04 start 国民ID検索
                .Append("             , CST.CST_SOCIALNUM AS SOSCIALNUM ")
                ' $04 end 国民ID検索
                '2018/07/19 NSK a.kani TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
                .Append("             , NVL(TRIM(VCL_DLR.IMP_VCL_FLG), :ICON_FLAG_OFF ) AS IMP_VCL_FLG ")
                '2018/07/19 NSK a.kani TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
                .Append("         FROM ")
                .Append("               TB_M_VEHICLE_DLR VCL_DLR ")
                .Append("             , TB_M_CUSTOMER_VCL CST_VCL ")
                .Append("             , TB_M_CUSTOMER_DLR CST_DLR ")
                .Append("             , TB_M_CUSTOMER CST ")
                .Append("             , TB_M_VEHICLE VCL ")
                .Append("             , TB_M_MODEL MODEL ")
                .Append("             , TBL_USERS USERS ")
                .Append("         WHERE ")
                .Append("               CST_DLR.DLR_CD = :DLRCD ")
                .Append("           AND CST_DLR.DLR_CD = CST_VCL.DLR_CD ")
                .Append("           AND CST_DLR.DLR_CD = VCL_DLR.DLR_CD ")
                .Append("           AND CST_DLR.CST_TYPE = '1' ")
                .Append("           AND CST_DLR.CST_ID = CST.CST_ID ")
                .Append("           AND CST_DLR.CST_ID = CST_VCL.CST_ID ")
                .Append("           AND CST_VCL.VCL_ID = VCL_DLR.VCL_ID ")
                .Append("           AND CST_VCL.VCL_ID = VCL.VCL_ID ")
                .Append("           AND Trim(VCL.VCL_VIN) IS NOT NULL ")
                .Append("           AND CST_VCL.OWNER_CHG_FLG = '0' ")
                .Append("           AND CST_VCL.CST_VCL_TYPE = '1' ")
                .Append("           AND MODEL.MODEL_CD(+) = VCL.MODEL_CD ")
                .Append("           AND RTRIM(USERS.ACCOUNT(+)) = CST_VCL.SLS_PIC_STF_CD ")
                .Append("           AND USERS.DELFLG(+) = '0' ")

                '1：車両登録No 、2：顧客名称、 3：VIN、 4：電話番号/携帯番号 5：国民ID
                Select Case searchType
                    Case SearchTypeRegNumber '車両登録No
                        .Append("    AND VCL_DLR.REG_NUM_SEARCH IN (")
                        For i As Decimal = 1 To searchText.Count()
                            .Append(" :VCLREGNO" & CStr(i))
                            If Not i = searchText.Count() Then
                                .Append(",")
                            End If
                        Next
                        .Append(" ) ")
                    Case SearchTypeCustomerName       '顧客名称
                        .Append("    AND CST.CST_NAME_SEARCH = :NAME")
                    Case SearchTypeVin  'VIN
                        .Append("    AND VCL.VCL_VIN_SEARCH = :VIN")
                        ' VINユニーク対応
                        .Append("    AND VCL.DMS_TAKEIN_DATETIME <> TO_DATE('1900/01/01 00:00:00','YYYY-MM-DD HH24:MI:SS')")
                    Case SearchTypeTelephoneNumber '電話番号/携帯番号
                        .Append("    AND (CST.CST_PHONE_SEARCH = :TELNO ")
                        .Append("         OR  CST.CST_MOBILE_SEARCH = :TELNO )")

                        ' $04 start 国民ID検索
                    Case SearchTypeSocialNumber '国民ID
                        .Append("    AND CST.CST_SOCIALNUM_SEARCH = :SOCIALNUM ")
                        ' $04 end 国民ID検索

                    Case Else
                End Select

                .Append("         UNION ALL ")
                .Append("         ")
                .Append("         SELECT ")
                .Append("               CST.CST_NAME AS NAME ")
                .Append("             , CST.NAMETITLE_NAME AS NAMETITLE ")
                .Append("             , '2' AS CUSTKBN ")
                .Append("             , CST.CST_PHONE AS TELNO ")
                .Append("             , CST.CST_MOBILE AS MOBILE ")
                .Append("             , VCL_DLR.REG_NUM AS VCLREGNO ")
                .Append("             , VCL.NEWCST_MODEL_NAME AS SERIESNM ")
                .Append("             , Null AS EXTERIORNAME ")
                .Append("             , VCL.NEWCST_MAKER_NAME AS MAKERNAME ")
                .Append("             , CST.CST_ID AS CUSTCD ")
                .Append("             , CST_VCL.SLS_PIC_STF_CD AS STUFFCD ")
                .Append("             , USERS.USERNAME AS STUFFNAME ")
                .Append("             , VCL.VCL_VIN AS VIN ")
                .Append("             , CST.CST_GENDER AS SEX ")
                .Append("             , CST_VCL.SVC_PIC_STF_CD AS SACODE ")
                .Append("             , VCL.VCL_ID AS SEQNO ")
                .Append("             , CST.FLEET_FLG AS CUSTYPE ")
                ' $04 start 国民ID検索
                .Append("             , CST.CST_SOCIALNUM AS SOSCIALNUM ")
                ' $04 end 国民ID検索
                '2018/07/19 NSK a.kani TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
                .Append("             , NVL(TRIM(VCL_DLR.IMP_VCL_FLG), :ICON_FLAG_OFF ) AS IMP_VCL_FLG ")
                '2018/07/19 NSK a.kani TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
                .Append("         FROM ")
                .Append("               TB_M_VEHICLE_DLR VCL_DLR ")
                .Append("             , TB_M_CUSTOMER_VCL CST_VCL ")
                .Append("             , TB_M_CUSTOMER_DLR CST_DLR ")
                .Append("             , TB_M_CUSTOMER CST ")
                .Append("             , TB_M_VEHICLE VCL ")
                .Append("             , TBL_USERS USERS ")
                .Append("         WHERE ")

                If String.Equals(sortType, SortTypeGateKeeper) Then
                    ' GKの場合は、車両情報を内部結合とする
                    .Append("               CST_DLR.DLR_CD = :DLRCD ")
                    .Append("           AND CST_DLR.DLR_CD = CST_VCL.DLR_CD ")
                    .Append("           AND CST_DLR.DLR_CD = VCL_DLR.DLR_CD ")
                    .Append("           AND CST_DLR.CST_TYPE = '2' ")
                    .Append("           AND CST_DLR.CST_ID = CST.CST_ID ")
                    .Append("           AND CST_DLR.CST_ID = CST_VCL.CST_ID ")
                    .Append("           AND CST_VCL.VCL_ID = VCL_DLR.VCL_ID ")
                    .Append("           AND CST_VCL.VCL_ID = VCL.VCL_ID ")
                    .Append("           AND CST_VCL.CST_VCL_TYPE = '1' ")
                Else
                    ' お客様チップ作成の場合は、車両情報を外部結合する
                    ' 保有車両の無い未取引客も取得対象とする為
                    .Append("               CST_DLR.DLR_CD = :DLRCD ")
                    .Append("           AND CST_DLR.DLR_CD = CST_VCL.DLR_CD(+) ")
                    .Append("           AND CST_VCL.DLR_CD = VCL_DLR.DLR_CD(+) ")
                    .Append("           AND CST_DLR.CST_TYPE = '2' ")
                    .Append("           AND CST_DLR.CST_ID = CST.CST_ID ")
                    .Append("           AND CST_DLR.CST_ID = CST_VCL.CST_ID(+) ")
                    .Append("           AND CST_VCL.VCL_ID = VCL_DLR.VCL_ID(+) ")
                    .Append("           AND CST_VCL.VCL_ID = VCL.VCL_ID(+) ")
                    .Append("           AND CST_VCL.CST_VCL_TYPE = '1' ")

                End If

                .Append("           AND RTRIM(USERS.ACCOUNT(+)) = CST_VCL.SLS_PIC_STF_CD ")
                .Append("           AND USERS.DELFLG(+) = '0' ")

                '1：車両登録No 、2：顧客名称、 3：VIN、 4：電話番号/携帯番号、5：国民ID
                Select Case searchType
                    Case SearchTypeRegNumber '車両登録No
                        .Append("    AND VCL_DLR.REG_NUM_SEARCH IN (")
                        For i As Decimal = 1 To searchText.Count()
                            .Append(" :VCLREGNO" & CStr(i))
                            If Not i = searchText.Count() Then
                                .Append(",")
                            End If
                        Next
                        .Append(" ) ")
                    Case SearchTypeCustomerName       '顧客名称
                        .Append("    AND CST.CST_NAME_SEARCH = :NAME")
                    Case SearchTypeVin  'VIN
                        .Append("    AND VCL.VCL_VIN_SEARCH = :VIN")
                    Case SearchTypeTelephoneNumber '電話番号/携帯番号
                        .Append("    AND (CST.CST_PHONE_SEARCH = :TELNO ")
                        .Append("         OR  CST.CST_MOBILE_SEARCH = :TELNO )")

                        ' $04 start 国民ID検索
                    Case SearchTypeSocialNumber '国民ID
                        .Append("    AND CST.CST_SOCIALNUM_SEARCH = :SOCIALNUM ")
                        ' $04 end 国民ID検索

                    Case Else
                End Select
                
                .Append("      ) CUST ")

                If String.Equals(sortType, SortTypeGateKeeper) Then
                    .Append(" ORDER BY CUST.CUSTKBN, CUST.NAME ")
                Else
                    .Append(" ORDER BY CUST.TELNO, CUST.STUFFNAME ")
                End If

            End With
            ' $04 END   次世代e-CRBセールス機能 新DB適応に向けた機能開発

            Using query As New DBSelectQuery( _
                Of VisitReceptionDataSet.VisitReceptionCustomerListDataTable)("VisitReception_003")
                query.CommandText = sql.ToString()
                sql = Nothing

                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                '2018/07/19 NSK a.kani TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
                query.AddParameterWithTypeValue("ICON_FLAG_OFF", OracleDbType.NVarchar2, IconFlagOff)
                '2018/07/19 NSK a.kani TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END

                Select Case searchType
                    Case SearchTypeRegNumber        '車両登録No
                        Dim i As Integer = 1
                        For Each value As String In searchText
                            query.AddParameterWithTypeValue("VCLREGNO" & CStr(i), OracleDbType.NVarchar2, value.ToUpper(CultureInfo.CurrentCulture))
                            i = i + 1
                        Next
                    Case SearchTypeCustomerName     '顧客名称
                        ' $06 START TMT UAT課題 
                        ' 全角空白・半角空白を除去
                        Dim searchVal = searchText(0).ToUpper(CultureInfo.CurrentCulture).Replace(" ", "")
                        searchVal = searchVal.Replace("　", "")
                        query.AddParameterWithTypeValue("NAME", OracleDbType.NVarchar2, searchVal)
                        ' $04 END   TMT UAT課題 
                    Case SearchTypeVin              'VIN
                        query.AddParameterWithTypeValue("VIN", OracleDbType.NVarchar2, searchText(0).ToUpper(CultureInfo.CurrentCulture))
                    Case SearchTypeTelephoneNumber  '電話番号/携帯番号
                        query.AddParameterWithTypeValue("TELNO", OracleDbType.NVarchar2, searchText(0).Replace("-", ""))
                        ' $04 start 国民ID
                    Case SearchTypeSocialNumber '国民ID
                        query.AddParameterWithTypeValue("SOCIALNUM", OracleDbType.NVarchar2, searchText(0).ToUpper(CultureInfo.CurrentCulture))
                        ' $04 end 国民ID
                    Case Else
                End Select

                ' SQLの実行
                dt = query.GetData()

            End Using

            ' 検索結果返却
            Return dt

        End Function

#End Region

#Region "セールス来店実績作成"

        ''' <summary>
        ''' セールス来店実績テーブル作成
        ''' </summary>
        ''' <param name="insertRow">登録情報を保持しているデータロウ</param>
        ''' <returns>影響行数</returns>
        ''' <remarks></remarks>
        Public Function InsertVisitSales(ByVal insertRow As VisitReceptionDataSet.VisitReceptionVisitSalesRow) As Integer

            Dim returnValue As Integer = 0

            'SQL文作成
            Dim sql As New StringBuilder
            With sql

                .Append(" INSERT /* VisitReception_017 */")
                .Append(" 　INTO TBL_VISIT_SALES (")
                .Append(" 	      VISITSEQ")
                .Append(" 	    , DLRCD")
                .Append(" 	    , STRCD")
                .Append(" 	    , VISITTIMESTAMP")
                .Append(" 	    , VCLREGNO")
                .Append(" 	    , CUSTSEGMENT")
                .Append(" 	    , CUSTID")
                .Append(" 	    , STAFFCD")
                .Append(" 	    , VISITPERSONNUM")
                .Append(" 	    , VISITMEANS")
                .Append(" 	    , VISITSTATUS")
                .Append(" 	    , BROUDCASTFLG")
                .Append(" 	    , TENTATIVENAME")
                .Append(" 	    , ACCOUNT")
                .Append(" 	    , SALESTABLENO")
                .Append(" 	    , FLLWUPBOX_DLRCD")
                .Append(" 	    , FLLWUPBOX_STRCD")
                .Append(" 	    , FLLWUPBOX_SEQNO")
                .Append(" 	    , SALESSTART")
                .Append(" 	    , SALESEND")
                .Append(" 	    , CREATEDATE")
                .Append(" 	    , UPDATEDATE")
                .Append(" 	    , CREATEACCOUNT")
                .Append(" 	    , UPDATEACCOUNT")
                .Append(" 	    , CREATEID")
                .Append(" 	    , UPDATEID")
                .Append(" )")
                .Append(" VALUES(")
                .Append(" 	      :VISITSEQ")
                .Append(" 	    , :DLRCD")
                .Append(" 	    , :STRCD")
                '$01 start 呼び出し元による来店時間の選択
                If insertRow.FUNCTIONID = GateKeeperID Then
                    .Append(" 	    , :VISITTIMESTAMP")
                Else
                    .Append(" 	    , SYSDATE")
                End If
                .Append(" 	    , :VCLREGNO")
                .Append(" 	    , :CUSTSEGMENT")
                .Append(" 	    , :CUSTID")
                .Append(" 	    , :STAFFCD")
                .Append(" 	    , :VISITPERSONNUM")
                .Append(" 	    , :VISITMEANS")
                .Append(" 	    , :VISITSTATUS")
                .Append(" 	    , :BROUDCASTFLG")
                .Append(" 	    , NULL")
                .Append(" 	    , :ACCOUNT")
                .Append(" 	    , NULL")
                .Append(" 	    , NULL")
                .Append(" 	    , NULL")
                .Append(" 	    , NULL")
                .Append(" 	    , NULL")
                .Append(" 	    , NULL")
                .Append(" 	    , SYSDATE")
                .Append(" 	    , SYSDATE")
                .Append(" 	    , :CREATEACCOUNT")
                .Append(" 	    , :UPDATEACCOUNT")
                .Append(" 	    , :CREATEID")
                .Append(" 	    , :UPDATEID")
                .Append(" )")
            End With

            'DbUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("VisitReception_004")

                query.CommandText = sql.ToString()
                sql = Nothing

                'SQLパラメータ設定
                query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Int64, insertRow.VISITSEQUENCE)
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, insertRow.DEALERCODE)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, insertRow.STORECODE)
                query.AddParameterWithTypeValue("VCLREGNO", OracleDbType.Varchar2, insertRow.VEHICLEREGNO)
                query.AddParameterWithTypeValue("CUSTSEGMENT", OracleDbType.Char, insertRow.CUSTOMERSEGMENT)
                query.AddParameterWithTypeValue("CUSTID", OracleDbType.Char, insertRow.CUSTOMERID)
                query.AddParameterWithTypeValue("STAFFCD", OracleDbType.Varchar2, insertRow.STAFFCODE)
                query.AddParameterWithTypeValue("VISITPERSONNUM", OracleDbType.Int16, insertRow.VISITPERSONNUMBER)
                query.AddParameterWithTypeValue("VISITMEANS", OracleDbType.Char, insertRow.VISITMEANS)
                query.AddParameterWithTypeValue("VISITSTATUS", OracleDbType.Char, insertRow.VISITSTATUS)
                query.AddParameterWithTypeValue("BROUDCASTFLG", OracleDbType.Char, insertRow.BROUDCAST)
                query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.Varchar2, insertRow.PHYSICSSTAFFCODE)
                query.AddParameterWithTypeValue("CREATEACCOUNT", OracleDbType.Varchar2, insertRow.CREATEACCOUNT)
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, insertRow.CREATEACCOUNT)
                query.AddParameterWithTypeValue("CREATEID", OracleDbType.Varchar2, insertRow.FUNCTIONID)
                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, insertRow.FUNCTIONID)

                '$01 start 呼び出し元による来店時間の選択
                If insertRow.FUNCTIONID = GateKeeperID Then
                    query.AddParameterWithTypeValue("VISITTIMESTAMP", OracleDbType.Date, insertRow.VISITTIME)
                End If
                'SQL実行(影響行数を返却)
                returnValue = query.Execute()

            End Using

            Return returnValue

        End Function

#End Region
        '$03 end

        ' $07 START (トライ店システム評価)SMBチップ検索の絞り込み方法変更
#Region "システム設定値取得"

        ' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証 START
        ' ''' <summary>
        ' ''' システム設定から設定値を取得する
        ' ''' </summary>
        ' ''' <param name="settingName">システム設定名</param>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Function GetSystemSettingValue(ByVal settingName As String) As VisitReceptionDataSet.SystemSettingDataTable

        '    Logger.Info(String.Format(CultureInfo.CurrentCulture, _
        '                              "{0}.{1} P1:{2} ", _
        '                              Me.GetType.ToString, _
        '                              System.Reflection.MethodBase.GetCurrentMethod.Name, _
        '                              settingName))

        '    Dim sql As New StringBuilder
        '    With sql
        '        .AppendLine(" SELECT /* VisitReception_018 */ ")
        '        .AppendLine(" 		 SETTING_VAL ")
        '        .AppendLine("   FROM ")
        '        .AppendLine(" 		 TB_M_SYSTEM_SETTING ")
        '        .AppendLine("  WHERE ")
        '        .AppendLine(" 		 SETTING_NAME = :SETTING_NAME ")
        '    End With

        '    Dim dt As VisitReceptionDataSet.SystemSettingDataTable = Nothing

        '    Using query As New DBSelectQuery(Of VisitReceptionDataSet.SystemSettingDataTable)("VisitReception_018")
        '        query.CommandText = sql.ToString()
        '        query.AddParameterWithTypeValue("SETTING_NAME", OracleDbType.NVarchar2, settingName)

        '        dt = query.GetData()
        '    End Using

        '    Logger.Info(String.Format(CultureInfo.CurrentCulture, _
        '                              "{0}.{1} QUERY:COUNT = {2}", _
        '                              Me.GetType.ToString, _
        '                              System.Reflection.MethodBase.GetCurrentMethod.Name, _
        '                              dt.Count))

        '    Return dt

        'End Function
        ' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証 END

#End Region
        ' $07 END (トライ店システム評価)SMBチップ検索の絞り込み方法変更

    End Class
End Namespace
Partial Class VisitReceptionDataSet
    Partial Class VisitReceptionVisitSalesDataTable

    End Class

End Class
