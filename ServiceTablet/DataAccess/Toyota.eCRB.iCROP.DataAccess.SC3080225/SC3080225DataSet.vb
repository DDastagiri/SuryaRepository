'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3080225DataTableTableAdapter.vb
'─────────────────────────────────────
'機能： 顧客詳細（参照） (データ)
'補足： 
'作成： 2014/02/14 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発
'更新： 2014/07/10 TMEJ 小澤 UAT不具合対応 ロゴの取得SQL修正
'更新： 2014/07/11 TMEJ 小澤 UAT不具合対応　入庫履歴SQLの修正
'更新： 2014/09/22 SKFC 佐藤 e-Mail,Line送信機能対応
'更新： 2015/09/04 TMEJ 春日井 サービス入庫予約のユーザ管理機能開発
'更新： 2016/02/04 NSK 中村【開発】（トライ店システム評価）コールセンター業務支援機能開発
'更新： 2016/06/30 NSK 皆川 TR-SVT-TMT-20150922-001 画面で画像プロフィールが表示しない
'更新： 2016/09/14 NSK 秋田谷 TR-SVT-TMT-20160727-002 顧客データ画面がサービス履歴を表示しない
'更新： 2018/02/27 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証
'更新： 2018/06/19 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示
'更新： 2018/07/23 NSK 坂本 TKM Next Gen e-CRB Project Application development Block B-1  顧客タイプ分類
'更新： 2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証
'─────────────────────────────────────
Imports System.Text
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Oracle.DataAccess.Client
Imports System.Globalization
Imports Toyota.eCRB.CustomerInfo.Details.DataAccess.SC3080225DataSet

Namespace SC3080225DataSetTableAdapters
    Public Class SC3080225StallInfoDataTableAdapter
        Inherits Global.System.ComponentModel.Component

#Region "定数"

        ''' <summary>
        ''' サービスステータス"13":納車済み
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ServiceStatusEndDelivery As String = "13"

        ''' <summary>
        ''' 最小日付
        ''' </summary>
        ''' <remarks></remarks>
        Private Const MinDate As String = "1900/01/01 00:00:00"

        ''' <summary>
        ''' プログラムID
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ProgramId As String = "SC3080225"

        ''' <summary>
        ''' 振当ステータス「2：SA振当済み」
        ''' </summary>
        ''' <remarks></remarks>
        Private Const AssignStatusAssignFinish As String = "2"

        ''' <summary>
        ''' 使用中フラグ（1：使用中）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const InuseTypeUse As String = "1"

        '2014/07/11 TMEJ 小澤 UAT不具合対応　入庫履歴SQLの修正 START

        ''' <summary>
        ''' オーナーチェンジフラグ：０：現オーナー
        ''' </summary>
        ''' <remarks></remarks>
        Private Const OWNER_CHG_FLG_0 As String = "0"

        ''' <summary>
        ''' 所有者フラグ：１：所有者
        ''' </summary>
        ''' <remarks></remarks>
        ''' 
        Private Const CST_VCL_TYPE_1 As String = "1"


        '2015/09/04 TMEJ 春日井 サービス入庫予約のユーザ管理機能開発 START
        ''' <summary>
        ''' 所有者フラグ：4：保険
        ''' </summary>
        ''' <remarks></remarks>
        ''' 
        Private Const CST_VCL_TYPE_4 As String = "4"
        '2015/09/04 TMEJ 春日井 サービス入庫予約のユーザ管理機能開発 END

        ''' <summary>
        ''' 登録方法：１：基幹入庫履歴
        ''' </summary>
        ''' <remarks></remarks>
        ''' 
        Private Const REG_MTD_1 As String = "1"

        ''' <summary>
        ''' 削除フラグ：０：未削除
        ''' </summary>
        ''' <remarks></remarks>
        ''' 
        Private Const DELFLG_0 As String = "0"

        '2014/07/11 TMEJ 小澤 UAT不具合対応　入庫履歴SQLの修正 END

        '2018/02/27 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 START
        ''' <summary>
        ''' 取得件数1件
        ''' </summary>
        ''' <remarks></remarks>
        ''' 
        Private Const ROWNUM_1 As Integer = 1
        '2018/02/27 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 END

        '2018/07/03 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
        ''' <summary>
        ''' アイコンのフラグ(0：対象外)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const IconFlagOff As String = "0"
        ''' <summary>
        ''' アイコンのフラグ(1：対象内)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const IconFlagOn As String = "1"
        '2018/07/03 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END

        '2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証 START
        ''' <summary>
        ''' IN句に指定できる最大件数:1000
        ''' </summary>
        ''' <remarks></remarks>
        Private Const InQueryMax As Integer = 1000
        '2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証 END

#End Region

#Region "SELECT"

        ''' <summary>
        ''' SC3080225_001:入庫履歴情報取得
        ''' </summary>
        ''' <param name="inDealerCode">販売点コード</param>
        ''' <param name="inVin">VIN</param>
        ''' <param name="inRegisterNumber">車両登録番号</param>
        ''' <param name="inAllServiceInHistoryType">全販売店入庫履歴情報取得条件「False：自販売店」「True：全販売店」</param>
        ''' <returns>入庫履歴情報</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' 2014/07/11 TMEJ 小澤 UAT不具合対応　入庫履歴SQLの修正
        ''' 2015/09/04 TMEJ 春日井 サービス入庫予約のユーザ管理機能開発
        ''' 2016/02/04 NSK 中村【開発】（トライ店システム評価）コールセンター業務支援機能開発
        ''' 2016/09/14 NSK 秋田谷TR-SVT-TMT-20160727-002 顧客データ画面がサービス履歴を表示しない
        ''' </history>
        Public Function GetServiceInHistoryInfo(ByVal inDealerCode As String, _
                                                ByVal inVin As String, _
                                                ByVal inRegisterNumber As String, _
                                                ByVal inAllServiceInHistoryType As Boolean) As SC3080225ContactHistoryInfoDataTable
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} START P1:{2} P2:{3} P3:{4} P4:{5} " _
                , Me.GetType.ToString _
                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                , inDealerCode _
                , inVin _
                , inRegisterNumber _
                , inAllServiceInHistoryType.ToString(CultureInfo.CurrentCulture)))

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3080225ContactHistoryInfoDataTable)("SC3080225_001")
                Dim sql As New StringBuilder

                ' SQL文の作成

                '2014/07/11 TMEJ 小澤 UAT不具合対応　入庫履歴SQLの修正 START

                'sql.AppendLine("SELECT /* SC3080225_001 */  ")
                'sql.AppendLine("       ROWNUM AS ROW_COUNT  ")
                'sql.AppendLine("      ,M1.DLR_CD  ")
                'sql.AppendLine("      ,M1.VCL_ID  ")
                'sql.AppendLine("      ,M1.SVCIN_NUM  ")
                'sql.AppendLine("      ,M1.SVCIN_DELI_DATE  ")
                'sql.AppendLine("      ,TRIM(M2.MAINTE_NAME) AS MAINTE_NAME  ")
                'sql.AppendLine("      ,TRIM(T6.SVC_NAME_MILE) AS SVC_NAME_MILE  ")
                'sql.AppendLine("      ,TRIM(T7.STF_NAME) AS STF_NAME  ")
                'sql.AppendLine("  FROM  ")
                'sql.AppendLine("       (SELECT T3.DLR_CD  ")
                'sql.AppendLine("              ,T1.VCL_ID  ")
                'sql.AppendLine("              ,T3.SVCIN_NUM  ")
                'sql.AppendLine("              ,CASE  ")
                'sql.AppendLine("                    WHEN T3.SVCIN_DELI_DATE = :MINDATE THEN NULL  ")
                'sql.AppendLine("                    ELSE T3.SVCIN_DELI_DATE  ")
                'sql.AppendLine("               END AS SVCIN_DELI_DATE  ")
                'sql.AppendLine("              ,T1.VCL_KATASHIKI ")
                'sql.AppendLine("              ,T4.MAINTE_CD ")
                'sql.AppendLine("              ,T4.SVC_CD ")
                'sql.AppendLine("              ,T3.PIC_STF_CD ")
                'sql.AppendLine("          FROM TB_M_VEHICLE T1  ")
                'sql.AppendLine("              ,TB_M_VEHICLE_DLR T2  ")
                'sql.AppendLine("              ,TB_T_VEHICLE_SVCIN_HIS T3  ")
                'sql.AppendLine("              ,TB_T_VEHICLE_MAINTE_HIS T4  ")
                'sql.AppendLine("         WHERE T1.VCL_ID = T2.VCL_ID  ")
                'sql.AppendLine("           AND T2.DLR_CD = T3.DLR_CD  ")
                'sql.AppendLine("           AND T2.VCL_ID = T3.VCL_ID  ")
                'sql.AppendLine("           AND T3.DLR_CD = T4.DLR_CD  ")
                'sql.AppendLine("           AND T3.SVCIN_NUM = T4.SVCIN_NUM  ")

                ''履歴情報取得条件チェック
                'If Not (inAllServiceInHistoryType) Then
                '    '「False：自販売店」の場合
                '    '条件追加
                '    sql.AppendLine("           AND T2.DLR_CD = :DLR_CD ")
                '    sql.AppendLine("           AND T3.DLR_CD = :DLR_CD ")

                'End If

                ''VINと車両登録番号のチェック
                'If Not (String.IsNullOrEmpty(inVin)) Then
                '    'VINが存在する場合
                '    'VIN条件追加
                '    sql.AppendLine("           AND T1.VCL_VIN_SEARCH = UPPER(:VCL_VIN_SEARCH) ")

                'ElseIf Not (String.IsNullOrEmpty(inRegisterNumber)) Then
                '    '車両登録番号が存在する場合
                '    '車両登録番号条件追加
                '    sql.AppendLine("           AND T2.REG_NUM_SEARCH = UPPER(:REG_NUM_SEARCH) ")

                'End If

                'sql.AppendLine("        ORDER BY T3.SVCIN_DELI_DATE DESC  ")
                'sql.AppendLine("       ) M1  ")
                'sql.AppendLine("      ,TB_M_MAINTE M2  ")
                'sql.AppendLine("      ,TB_M_SERVICE T6  ")
                'sql.AppendLine("      ,TB_M_STAFF T7  ")
                'sql.AppendLine(" WHERE M1.DLR_CD = M2.DLR_CD(+) ")
                'sql.AppendLine("   AND M1.MAINTE_CD = M2.MAINTE_CD(+) ")
                'sql.AppendLine("   AND M2.MAINTE_KATASHIKI(+) = SUBSTR(M1.VCL_KATASHIKI, 1, INSTR(M1.VCL_KATASHIKI, '-')-1)  ")
                'sql.AppendLine("   AND M1.DLR_CD = T6.DLR_CD(+)  ")
                'sql.AppendLine("   AND M1.SVC_CD = T6.SVC_CD(+)  ")
                'sql.AppendLine("   AND M1.PIC_STF_CD = T7.STF_CD(+)  ")

                sql.AppendLine("SELECT /* SC3080225_001 */ ")
                sql.AppendLine("       ROWNUM AS ROW_COUNT  ")
                sql.AppendLine("      ,M1.DLR_CD ")
                sql.AppendLine("      ,M1.VCL_ID ")
                sql.AppendLine("      ,M1.SVCIN_NUM ")
                sql.AppendLine("      ,CASE ")
                sql.AppendLine("            WHEN M1.SVCIN_DELI_DATE = :MINDATE THEN NULL ")
                sql.AppendLine("            ELSE M1.SVCIN_DELI_DATE ")
                sql.AppendLine("       END AS SVCIN_DELI_DATE ")
                sql.AppendLine("      ,TRIM(M3.MAINTE_NAME) AS MAINTE_NAME ")
                sql.AppendLine("      ,TRIM(M2.SVC_NAME_MILE) AS SVC_NAME_MILE  ")
                sql.AppendLine("      ,TRIM(M1.USERNAME) AS STF_NAME  ")
                '2016/02/04 NSK 中村【開発】（トライ店システム評価）コールセンター業務支援機能開発 START
                sql.AppendLine("      ,TRIM(M2.MAINTE_NAME_HIS) AS MAINTE_NAME_HIS  ")
                '2016/02/04 NSK 中村【開発】（トライ店システム評価）コールセンター業務支援機能開発 END
                sql.AppendLine("  FROM (SELECT Q4.DLR_CD ")
                sql.AppendLine("              ,Q4.VCL_ID ")
                sql.AppendLine("              ,Q4.SVCIN_NUM ")
                sql.AppendLine("              ,Q6.USERNAME ")
                sql.AppendLine("              ,Q7.REG_DATE AS SVCIN_DELI_DATE ")
                sql.AppendLine("              ,MAX(Q5.INSPEC_SEQ) AS INSPEC_SEQ_MAX ")
                sql.AppendLine("         FROM TB_M_CUSTOMER_VCL Q1 ")
                sql.AppendLine("             ,TB_M_VEHICLE Q2  ")
                sql.AppendLine("             ,TB_M_VEHICLE_DLR Q3  ")
                sql.AppendLine("             ,TB_T_VEHICLE_SVCIN_HIS Q4 ")
                sql.AppendLine("             ,TB_T_VEHICLE_MAINTE_HIS Q5 ")
                sql.AppendLine("             ,TBL_USERS Q6 ")
                sql.AppendLine("             ,TB_T_VEHICLE_MILEAGE Q7 ")
                sql.AppendLine("        WHERE Q1.VCL_ID=Q2.VCL_ID ")
                sql.AppendLine("          AND Q1.DLR_CD=Q3.DLR_CD ")
                sql.AppendLine("          AND Q1.VCL_ID=Q3.VCL_ID ")
                sql.AppendLine("          AND Q1.VCL_ID = Q4.VCL_ID ")
                sql.AppendLine("          AND Q1.CST_ID = Q4.CST_ID ")
                sql.AppendLine("          AND Q1.DLR_CD = Q4.DLR_CD ")
                sql.AppendLine("          AND Q4.DLR_CD = Q5.DLR_CD(+) ")
                sql.AppendLine("          AND Q4.SVCIN_NUM = Q5.SVCIN_NUM(+) ")
                sql.AppendLine("          AND Q4.VCL_MILE_ID = Q7.VCL_MILE_ID ")
                sql.AppendLine("          AND Q4.DLR_CD = Q7.DLR_CD ")
                sql.AppendLine("          AND Q4.VCL_ID = Q7.VCL_ID ")
                sql.AppendLine("          AND Q4.PIC_STF_CD = Q6.ACCOUNT(+) ")

                '履歴情報取得条件チェック
                If Not (inAllServiceInHistoryType) Then
                    '「False：自販売店」の場合
                    '条件追加
                    sql.AppendLine("          AND Q4.DLR_CD = :DLR_CD ")

                End If

                '2016/09/14 NSK 秋田谷 【開発】顧客データ画面がサービス履歴を表示しない START'
                'オーナーチェンジフラグの条件を削除
                'sql.AppendLine("          AND Q1.OWNER_CHG_FLG = :OWNER_CHG_FLG_0 ")
                '2016/09/14 NSK 秋田谷 【開発】顧客データ画面がサービス履歴を表示しない END'

                '2015/09/04 TMEJ 春日井 サービス入庫予約のユーザ管理機能開発 START
                'sql.AppendLine("          AND Q1.CST_VCL_TYPE = :CST_VCL_TYPE_1 ")
                sql.AppendLine("          AND Q1.CST_VCL_TYPE <> :CST_VCL_TYPE_4 ")
                '2015/09/04 TMEJ 春日井 サービス入庫予約のユーザ管理機能開発 END
                sql.AppendLine("          AND Q7.REG_MTD = :REG_MTD_1 ")
                sql.AppendLine("          AND Q6.DELFLG(+) = :DELFLG_0 ")

                'VINと車両登録番号のチェック
                If Not (String.IsNullOrEmpty(inVin)) Then
                    'VINが存在する場合
                    'VIN条件追加
                    sql.AppendLine("          AND Q2.VCL_VIN_SEARCH = UPPER(:VCL_VIN_SEARCH) ")

                ElseIf Not (String.IsNullOrEmpty(inRegisterNumber)) Then
                    '車両登録番号が存在する場合
                    '車両登録番号条件追加
                    sql.AppendLine("          AND Q3.REG_NUM_SEARCH = UPPER(:REG_NUM_SEARCH) ")

                End If

                sql.AppendLine("        GROUP BY Q4.DLR_CD ")
                sql.AppendLine("                ,Q4.VCL_ID ")
                sql.AppendLine("                ,Q4.SVCIN_NUM ")
                sql.AppendLine("                ,Q6.USERNAME ")
                sql.AppendLine("                ,Q7.REG_DATE ")
                sql.AppendLine("        ORDER BY Q7.REG_DATE DESC) M1 ")
                sql.AppendLine("      ,(SELECT W3.DLR_CD ")
                sql.AppendLine("              ,W3.SVCIN_NUM ")
                sql.AppendLine("              ,W4.INSPEC_SEQ ")
                '2016/02/04 NSK 中村【開発】（トライ店システム評価）コールセンター業務支援機能開発 START
                sql.AppendLine("              ,W4.MAINTE_NAME AS MAINTE_NAME_HIS ")
                '2016/02/04 NSK 中村【開発】（トライ店システム評価）コールセンター業務支援機能開発 END
                sql.AppendLine("              ,W5.SVC_NAME_MILE ")
                sql.AppendLine("          FROM TB_M_VEHICLE W1  ")
                sql.AppendLine("              ,TB_M_VEHICLE_DLR W2  ")
                sql.AppendLine("              ,TB_T_VEHICLE_SVCIN_HIS W3 ")
                sql.AppendLine("              ,TB_T_VEHICLE_MAINTE_HIS W4 ")
                sql.AppendLine("              ,TB_M_SERVICE W5 ")
                sql.AppendLine("         WHERE W1.VCL_ID=W2.VCL_ID ")
                sql.AppendLine("           AND W2.DLR_CD=W3.DLR_CD ")
                sql.AppendLine("           AND W2.VCL_ID=W3.VCL_ID ")
                sql.AppendLine("           AND W3.DLR_CD = W4.DLR_CD ")
                sql.AppendLine("           AND W3.SVCIN_NUM = W4.SVCIN_NUM ")
                sql.AppendLine("           AND W3.DLR_CD = W5.DLR_CD(+) ")
                sql.AppendLine("           AND W3.SVC_CD = W5.SVC_CD(+) ")

                '履歴情報取得条件チェック
                If Not (inAllServiceInHistoryType) Then
                    '「False：自販売店」の場合
                    '条件追加
                    sql.AppendLine("           AND W3.DLR_CD = :DLR_CD ")

                End If

                'VINと車両登録番号のチェック
                If Not (String.IsNullOrEmpty(inVin)) Then
                    'VINが存在する場合
                    'VIN条件追加
                    sql.AppendLine("           AND W1.VCL_VIN_SEARCH = UPPER(:VCL_VIN_SEARCH) ")

                ElseIf Not (String.IsNullOrEmpty(inRegisterNumber)) Then
                    '車両登録番号が存在する場合
                    '車両登録番号条件追加
                    sql.AppendLine("           AND W2.REG_NUM_SEARCH = UPPER(:REG_NUM_SEARCH) ")

                End If

                sql.AppendLine("       ) M2")
                sql.AppendLine("      ,(SELECT E3.DLR_CD ")
                sql.AppendLine("              ,E3.SVCIN_NUM ")
                sql.AppendLine("              ,E4.INSPEC_SEQ ")
                sql.AppendLine("              ,E5.MAINTE_NAME ")
                sql.AppendLine("          FROM TB_M_VEHICLE E1 ")
                sql.AppendLine("              ,TB_M_VEHICLE_DLR E2  ")
                sql.AppendLine("              ,TB_T_VEHICLE_SVCIN_HIS E3 ")
                sql.AppendLine("              ,TB_T_VEHICLE_MAINTE_HIS E4 ")
                sql.AppendLine("              ,TB_M_MAINTE E5 ")
                sql.AppendLine("         WHERE E1.VCL_ID=E2.VCL_ID ")
                sql.AppendLine("           AND E2.DLR_CD = E3.DLR_CD ")
                sql.AppendLine("           AND E2.VCL_ID = E3.VCL_ID ")
                sql.AppendLine("           AND E3.DLR_CD = E4.DLR_CD ")
                sql.AppendLine("           AND E3.SVCIN_NUM = E4.SVCIN_NUM ")
                sql.AppendLine("           AND E4.DLR_CD = E5.DLR_CD ")
                sql.AppendLine("           AND E4.MAINTE_CD = E5.MAINTE_CD ")
                sql.AppendLine("           AND E5.MAINTE_KATASHIKI = SUBSTR(E1.VCL_KATASHIKI, 1, INSTR(E1.VCL_KATASHIKI, '-') - 1) ")

                '履歴情報取得条件チェック
                If Not (inAllServiceInHistoryType) Then
                    '「False：自販売店」の場合
                    '条件追加
                    sql.AppendLine("           AND E3.DLR_CD = :DLR_CD ")

                End If

                'VINと車両登録番号のチェック
                If Not (String.IsNullOrEmpty(inVin)) Then
                    'VINが存在する場合
                    'VIN条件追加
                    sql.AppendLine("           AND E1.VCL_VIN_SEARCH = UPPER(:VCL_VIN_SEARCH) ")

                ElseIf Not (String.IsNullOrEmpty(inRegisterNumber)) Then
                    '車両登録番号が存在する場合
                    '車両登録番号条件追加
                    sql.AppendLine("           AND E2.REG_NUM_SEARCH = UPPER(:REG_NUM_SEARCH) ")

                End If

                sql.AppendLine("       ) M3")
                sql.AppendLine(" WHERE ")
                sql.AppendLine("       M1.DLR_CD = M2.DLR_CD(+) ")
                sql.AppendLine("   AND M1.SVCIN_NUM = M2.SVCIN_NUM(+) ")
                sql.AppendLine("   AND M1.INSPEC_SEQ_MAX = M2.INSPEC_SEQ(+) ")
                sql.AppendLine("   AND M1.DLR_CD = M3.DLR_CD(+) ")
                sql.AppendLine("   AND M1.SVCIN_NUM = M3.SVCIN_NUM(+) ")
                sql.AppendLine("   AND M1.INSPEC_SEQ_MAX = M3.INSPEC_SEQ(+) ")
                '2016/09/14 NSK 秋田谷 【開発】顧客データ画面がサービス履歴を表示しない START'
                'ソート条件の追記
                sql.AppendLine(" ORDER BY M1.SVCIN_DELI_DATE DESC ")
                '2016/09/14 NSK 秋田谷 【開発】顧客データ画面がサービス履歴を表示しない END'

                '2014/07/11 TMEJ 小澤 UAT不具合対応　入庫履歴SQLの修正 END

                query.CommandText = sql.ToString()

                ' バインド変数定義
                '履歴情報取得条件チェック
                If Not (inAllServiceInHistoryType) Then
                    '「False：自販売店」の場合
                    'バインド情報追加
                    query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, inDealerCode)

                End If

                query.AddParameterWithTypeValue("MINDATE", OracleDbType.Date, Date.Parse(MinDate, CultureInfo.CurrentCulture))

                'VINと車両登録番号のチェック
                If Not (String.IsNullOrEmpty(inVin)) Then
                    'VINが存在する場合
                    'バインド情報追加
                    query.AddParameterWithTypeValue("VCL_VIN_SEARCH", OracleDbType.NVarchar2, inVin)

                ElseIf Not (String.IsNullOrEmpty(inRegisterNumber)) Then
                    '車両登録番号が存在する場合
                    'バインド情報追加
                    query.AddParameterWithTypeValue("REG_NUM_SEARCH", OracleDbType.NVarchar2, inRegisterNumber)

                End If

                '2014/07/11 TMEJ 小澤 UAT不具合対応　入庫履歴SQLの修正 START

                '2016/09/14 NSK 秋田谷 【開発】顧客データ画面がサービス履歴を表示しない START'
                'オーナーチェンジフラグの条件は削除
                'オーナーチェンジフラグ
                'query.AddParameterWithTypeValue("OWNER_CHG_FLG_0", OracleDbType.NVarchar2, OWNER_CHG_FLG_0)
                '2016/09/14 NSK 秋田谷 【開発】顧客データ画面がサービス履歴を表示しない END'

                '2015/09/04 TMEJ 春日井 サービス入庫予約のユーザ管理機能開発 START
                '所有者フラグ
                'query.AddParameterWithTypeValue("CST_VCL_TYPE_1", OracleDbType.NVarchar2, CST_VCL_TYPE_1)
                '保険フラグ
                query.AddParameterWithTypeValue("CST_VCL_TYPE_4", OracleDbType.NVarchar2, CST_VCL_TYPE_4)
                '2015/09/04 TMEJ 春日井 サービス入庫予約のユーザ管理機能開発 END
                '登録方法
                query.AddParameterWithTypeValue("REG_MTD_1", OracleDbType.NVarchar2, REG_MTD_1)
                '削除フラグ
                query.AddParameterWithTypeValue("DELFLG_0", OracleDbType.Char, DELFLG_0)

                '2014/07/11 TMEJ 小澤 UAT不具合対応　入庫履歴SQLの修正 END

                'SQL実行
                Dim dt As SC3080225ContactHistoryInfoDataTable = query.GetData()

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} END:RETURN COUNT={2}" _
                , Me.GetType.ToString _
                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                , dt.Count))

                ' 検索結果の返却
                Return dt

            End Using

        End Function

        ''' <summary>
        ''' SC3080225_002:敬称情報取得
        ''' </summary>
        ''' <param name="inDmsCustomerCode">基幹顧客ID</param>
        ''' <returns>敬称情報</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' </history>
        Public Function GetTitleNameInfo(ByVal inDmsCustomerCode As String) As SC3080225NameTitleInfoDataTable
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} START P1:{2} " _
                , Me.GetType.ToString _
                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                , inDmsCustomerCode))

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3080225NameTitleInfoDataTable)("SC3080225_002")

                Dim sql As New StringBuilder

                ' SQL文の作成
                sql.AppendLine("SELECT /* SC3080225_002 */ ")
                sql.AppendLine("       T2.POSITION_TYPE ")
                sql.AppendLine("      ,T2.NAMETITLE_NAME ")
                sql.AppendLine("  FROM ")
                sql.AppendLine("       TB_M_CUSTOMER T1 ")
                sql.AppendLine("      ,TB_M_NAMETITLE T2 ")
                sql.AppendLine(" WHERE ")
                sql.AppendLine("       T1.NAMETITLE_CD = T2.NAMETITLE_CD(+) ")
                sql.AppendLine("   AND T1.DMS_CST_CD = :DMS_CST_CD ")

                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("DMS_CST_CD", OracleDbType.NVarchar2, inDmsCustomerCode)

                'SQL実行
                Dim dt As SC3080225NameTitleInfoDataTable = query.GetData()

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END RETURN:COUNT={2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , dt.Count))

                ' 検索結果の返却
                Return dt

            End Using

        End Function

        ''' <summary>
        ''' SC3080225_003:車両登録エリア情報取得
        ''' </summary>
        ''' <param name="inRegisterAreaCodeList">車両登録エリアコード</param>
        ''' <returns>車両登録エリア情報</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' 2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証
        ''' </history>
        Public Function GetRegisterAreaInfo(ByVal inRegisterAreaCodeList As List(Of String)) As SC3080225RegisterAreaInfoDataTable
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} START P1:{2} " _
                , Me.GetType.ToString _
                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                , inRegisterAreaCodeList))

            'IN句用文字列生成
            Dim inQuery As String = GetInQuery(inRegisterAreaCodeList, "REG_AREA_CD")

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3080225RegisterAreaInfoDataTable)("SC3080225_003")

                Dim sql As New StringBuilder

                ' SQL文の作成
                sql.AppendLine("SELECT /* SC3080225_003 */ ")
                sql.AppendLine("    　 REG_AREA_CD")
                sql.AppendLine("      ,T1.REG_AREA_NAME ")
                sql.AppendLine("  FROM ")
                sql.AppendLine("       TB_M_REG_AREA T1 ")
                sql.AppendLine(" WHERE ")
                sql.AppendLine(inQuery)

                query.CommandText = sql.ToString()

                'SQL実行
                Dim dt As SC3080225RegisterAreaInfoDataTable = query.GetData()

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END RETURN:COUNT={2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , dt.Count))

                ' 検索結果の返却
                Return dt

            End Using

        End Function

        ''' <summary>
        ''' SC3080225_004:モデルロゴ情報取得
        ''' </summary>
        ''' <param name="inSeriesCodeList">シリーズコード</param>
        ''' <returns>モデルロゴ情報</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' 2014/07/10 TMEJ 小澤 UAT不具合対応 ロゴの取得SQL修正
        ''' 2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証
        ''' </history>
        Public Function GetModelLogoInfo(ByVal inSeriesCodeList As List(Of String)) As SC3080225ModelLogoInfoDataTable
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} START P1:{2} " _
                , Me.GetType.ToString _
                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                , inSeriesCodeList))

            'IN句用文字列生成
            Dim inQuery As String = GetInQuery(inSeriesCodeList, "MODEL_CD")

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3080225ModelLogoInfoDataTable)("SC3080225_004")

                Dim sql As New StringBuilder

                ' SQL文の作成
                sql.AppendLine("SELECT /* SC3080225_004 */ ")
                sql.AppendLine("       MODEL_CD ")
                sql.AppendLine("      ,TRIM(T1.LOGO_PICTURE) AS LOGO_PICTURE ")
                sql.AppendLine("      ,TRIM(T1.LOGO_PICTURE_SEL) AS LOGO_PICTURE_SEL ")
                sql.AppendLine("  FROM ")
                sql.AppendLine("       TB_M_MODEL T1 ")
                sql.AppendLine(" WHERE ")
                sql.AppendLine(inQuery)
                '2014/07/10 TMEJ 小澤 UAT不具合対応 ロゴの取得SQL修正 START
                'sql.AppendLine("   AND T1.INUSE_FLG = :INUSE_FLG_1 ")
                '2014/07/10 TMEJ 小澤 UAT不具合対応 ロゴの取得SQL修正 END

                query.CommandText = sql.ToString()

                ' バインド変数定義
                'query.AddParameterWithTypeValue("MODEL_CD", OracleDbType.NVarchar2, inSeriesCodeList)
                '2014/07/10 TMEJ 小澤 UAT不具合対応 ロゴの取得SQL修正 START
                'query.AddParameterWithTypeValue("INUSE_FLG_1", OracleDbType.NVarchar2, InuseTypeUse)
                '2014/07/10 TMEJ 小澤 UAT不具合対応 ロゴの取得SQL修正 END

                'SQL実行
                Dim dt As SC3080225ModelLogoInfoDataTable = query.GetData()

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END RETURN:COUNT={2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , dt.Count))

                ' 検索結果の返却
                Return dt

            End Using

        End Function

        ''' <summary>
        ''' SC3080225_005:顧客情報取得
        ''' </summary>
        ''' <param name="inDmsCustomerCode">基幹顧客ID</param>
        ''' <returns>顧客情報情報</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' </history>
        Public Function GetCustomerInfo(ByVal inDealerCode As String, _
                                        ByVal inDmsCustomerCode As String) As SC3080225CustomerInfoDataTable
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} START P1:{2} P2:{3} " _
                , Me.GetType.ToString _
                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                , inDealerCode _
                , inDmsCustomerCode))

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3080225CustomerInfoDataTable)("SC3080225_005")

                Dim sql As New StringBuilder

                ' SQL文の作成
                sql.AppendLine("SELECT /* SC3080225_005 */ ")
                sql.AppendLine("       T2.CST_ID ")
                sql.AppendLine("      ,TRIM(T3.CST_TYPE) AS CST_TYPE ")
                sql.AppendLine("      ,TRIM(T1.CST_VCL_TYPE) AS CST_VCL_TYPE ")
                sql.AppendLine("      ,TRIM(T3.IMG_FILE_SMALL) AS IMG_FILE_SMALL ")
                sql.AppendLine("      ,T2.ROW_LOCK_VERSION ")
                sql.AppendLine("  FROM ")
                sql.AppendLine("       TB_M_CUSTOMER_VCL T1 ")
                sql.AppendLine("      ,TB_M_CUSTOMER T2 ")
                sql.AppendLine("      ,TB_M_CUSTOMER_DLR T3 ")
                sql.AppendLine(" WHERE ")
                sql.AppendLine("       T1.CST_ID = T2.CST_ID ")
                sql.AppendLine("   AND T1.CST_ID = T3.CST_ID ")
                sql.AppendLine("   AND T1.DLR_CD = T3.DLR_CD ")
                sql.AppendLine("   AND T1.DLR_CD = :DLR_CD ")
                sql.AppendLine("   AND T2.DMS_CST_CD = :DMS_CST_CD ")
                '2016/06/30 NSK 皆川 TR-SVT-TMT-20150922-001 画面で画像プロフィールが表示しない START
                sql.AppendLine(" ORDER BY T2.DMS_TAKEIN_DATETIME DESC ")
                sql.AppendLine("         ,T1.CST_VCL_TYPE ")
                '2016/06/30 NSK 皆川 TR-SVT-TMT-20150922-001 画面で画像プロフィールが表示しない END

                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, inDealerCode)
                query.AddParameterWithTypeValue("DMS_CST_CD", OracleDbType.NVarchar2, inDmsCustomerCode)

                'SQL実行
                Dim dt As SC3080225CustomerInfoDataTable = query.GetData()

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END RETURN:COUNT={2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , dt.Count))

                ' 検索結果の返却
                Return dt

            End Using

        End Function

        ''' <summary>
        ''' SC3080225_006:ROプレビュー情報取得
        ''' </summary>
        ''' <param name="inOrderNumber">RO番号</param>
        ''' <returns>RO情報</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' </history>
        Public Function GetOrderInfo(ByVal inDealerCode As String, _
                                     ByVal inOrderNumber As String) As SC3080225OrderPreviewInfoDataTable
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} START P1:{2} P2:{3} " _
                , Me.GetType.ToString _
                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                , inDealerCode _
                , inOrderNumber))

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3080225OrderPreviewInfoDataTable)("SC3080225_006")

                Dim sql As New StringBuilder

                ' SQL文の作成
                sql.AppendLine("SELECT /* SC3080225_006 */ ")
                sql.AppendLine("       T3.VISITSEQ ")
                sql.AppendLine("      ,T1.RO_NUM ")
                sql.AppendLine("      ,T2.DMS_JOB_DTL_ID ")
                sql.AppendLine("  FROM ")
                sql.AppendLine("       TB_T_SERVICEIN T1 ")
                sql.AppendLine("      ,TB_T_JOB_DTL T2 ")
                sql.AppendLine("      ,TBL_SERVICE_VISIT_MANAGEMENT T3 ")
                sql.AppendLine(" WHERE ")
                sql.AppendLine("       T1.SVCIN_ID = T2.SVCIN_ID ")
                sql.AppendLine("   AND T1.DLR_CD = T3.DLRCD(+) ")
                sql.AppendLine("   AND T1.RO_NUM = T3.ORDERNO(+) ")
                sql.AppendLine("   AND T1.DLR_CD = :DLR_CD ")
                sql.AppendLine("   AND T1.RO_NUM = :RO_NUM ")
                sql.AppendLine("   AND T2.JOB_DTL_ID = (SELECT MIN(R1.JOB_DTL_ID) ")
                sql.AppendLine("                          FROM TB_T_JOB_DTL R1 ")
                sql.AppendLine("                         WHERE R1.SVCIN_ID = T1.SVCIN_ID) ")
                sql.AppendLine("   AND T3.ASSIGNSTATUS(+) = :ASSIGNSTATUS_2 ")

                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, inDealerCode)
                query.AddParameterWithTypeValue("RO_NUM", OracleDbType.NVarchar2, inOrderNumber)
                query.AddParameterWithTypeValue("ASSIGNSTATUS_2", OracleDbType.NVarchar2, AssignStatusAssignFinish)

                'SQL実行
                Dim dt As SC3080225OrderPreviewInfoDataTable = query.GetData()

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END RETURN:COUNT={2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , dt.Count))

                ' 検索結果の返却
                Return dt

            End Using

        End Function

        ''' <summary>
        ''' SC3080225_007:個人法人項目マスタ文言No取得
        ''' </summary>
        ''' <param name="inSubCustomerType">個人法人項目コード</param>
        ''' <returns>個人法人項目マスタ文言No</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' </history>
        Public Function GetPrivateFleetWord(ByVal inSubCustomerType As String) As SC3080225PrivateFleetInfoDataTable
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} START P1:{2} " _
                , Me.GetType.ToString _
                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                , inSubCustomerType))

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3080225PrivateFleetInfoDataTable)("SC3080225_007")

                Dim sql As New StringBuilder

                ' SQL文の作成
                sql.AppendLine("SELECT /* SC3080225_007 */ ")
                sql.AppendLine("       TRIM(T1.PRIVATE_FLEET_ITEM) AS PRIVATE_FLEET_ITEM ")
                sql.AppendLine("  FROM ")
                sql.AppendLine("       TB_M_PRIVATE_FLEET_ITEM T1 ")
                sql.AppendLine(" WHERE ")
                sql.AppendLine("       T1.PRIVATE_FLEET_ITEM_CD = :PRIVATE_FLEET_ITEM_CD ")
                sql.AppendLine("   AND T1.INUSE_FLG = :INUSE_FLG_1 ")

                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("PRIVATE_FLEET_ITEM_CD", OracleDbType.NVarchar2, inSubCustomerType)
                query.AddParameterWithTypeValue("INUSE_FLG_1", OracleDbType.NVarchar2, InuseTypeUse)

                'SQL実行
                Dim dt As SC3080225PrivateFleetInfoDataTable = query.GetData()

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END RETURN:COUNT={2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , dt.Count))

                ' 検索結果の返却
                Return dt

            End Using

        End Function

        ''' <summary>
        ''' SC3080225_008:RO情報取得
        ''' </summary>
        ''' <param name="inDealerCode">Dealerコード</param>
        ''' <param name="inCustomerCode">顧客コード</param>
        ''' <param name="inVin">VIN</param>
        ''' <param name="inRegisterNumber">登録番号</param>
        ''' <returns>RONUMBER</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' 2014/09/22 SKFC 佐藤 e-Mail,Line送信機能対応により追加
        ''' </history>
        Public Function GetRONumber(ByVal inDealerCode As String, _
                                    ByVal inCustomerCode As Decimal, _
                                    ByVal inVin As String, _
                                    ByVal inRegisterNumber As String) As SC3080225RONumberInfoDataTable
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} START P1:{2} P2:{3} P3:{4} P4:{5} " _
                , Me.GetType.ToString _
                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                , inDealerCode _
                , inCustomerCode _
                , inVin _
                , inRegisterNumber))

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3080225RONumberInfoDataTable)("SC3080225_008")

                Dim sql As New StringBuilder

                ' SQL文の作成
                'sql.AppendLine("SELECT /* SC3080225_008 */ ")
                'sql.AppendLine("  MAX(RO.RO_NUM) ")
                'sql.AppendLine("  FROM ")
                'sql.AppendLine("       TB_T_RO_INFO RO ")
                'sql.AppendLine("  INNER JOIN ")
                'sql.AppendLine("       TB_T_SERVICEIN SV ")
                'sql.AppendLine("     ON RO.SVCIN_ID = SV.SVCIN_ID  ")
                'sql.AppendLine("    AND RO.DLR_CD = SV.DLR_CD ")
                'sql.AppendLine("  WHERE ")
                'sql.AppendLine("        SV.CST_ID = :CST_ID ")
                'sql.AppendLine("    AND SV.DLR_CD = :DLR_CD ")
                sql.AppendLine("SELECT /* SC3080225_008 */ ")
                sql.AppendLine("       MAX(RO.RO_NUM) RO_NUM ")
                sql.AppendLine("  FROM TB_M_CUSTOMER_VCL CV ")
                sql.AppendLine("      ,TB_M_VEHICLE VCL ")
                sql.AppendLine("      ,TB_M_VEHICLE_DLR VD ")
                sql.AppendLine("      ,TB_T_SERVICEIN SV ")
                sql.AppendLine("      ,TB_T_RO_INFO RO ")
                sql.AppendLine(" WHERE CV.VCL_ID = VCL.VCL_ID ")
                sql.AppendLine("   AND CV.DLR_CD = VD.DLR_CD ")
                sql.AppendLine("   AND CV.VCL_ID = VD.VCL_ID ")
                sql.AppendLine("   AND SV.CST_ID = CV.CST_ID ")
                sql.AppendLine("   AND SV.VCL_ID = CV.VCL_ID ")
                sql.AppendLine("   AND RO.SVCIN_ID = SV.SVCIN_ID ")
                sql.AppendLine("   AND RO.DLR_CD = SV.DLR_CD ")
                sql.AppendLine("   AND CV.CST_ID = :CST_ID ")
                sql.AppendLine("   AND SV.DLR_CD = :DLR_CD ")
                'VINと車両登録番号のチェック
                If Not (String.IsNullOrEmpty(inVin)) Then
                    'VINが存在する場合
                    'VIN条件追加
                    sql.AppendLine("   AND VCL.VCL_VIN_SEARCH = UPPER(:VCL_VIN_SEARCH) ")

                ElseIf Not (String.IsNullOrEmpty(inRegisterNumber)) Then
                    '車両登録番号が存在する場合
                    '車両登録番号条件追加
                    sql.AppendLine("   AND VD.REG_NUM_SEARCH = UPPER(:REG_NUM_SEARCH) ")

                End If

                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("CST_ID", OracleDbType.Decimal, inCustomerCode)
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, inDealerCode)
                'VINと車両登録番号のチェック
                If Not (String.IsNullOrEmpty(inVin)) Then
                    'VINが存在する場合
                    'バインド情報追加
                    query.AddParameterWithTypeValue("VCL_VIN_SEARCH", OracleDbType.NVarchar2, inVin)

                ElseIf Not (String.IsNullOrEmpty(inRegisterNumber)) Then
                    '車両登録番号が存在する場合
                    'バインド情報追加
                    query.AddParameterWithTypeValue("REG_NUM_SEARCH", OracleDbType.NVarchar2, inRegisterNumber)

                End If

                'SQL実行
                Dim dt As SC3080225RONumberInfoDataTable = query.GetData

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END RETURN:COUNT={2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , dt.Count))

                ' 検索結果の返却
                Return dt

            End Using

        End Function

        '2018/02/27 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 START
        ''' <summary>
        ''' SC3080225_009:SSCフラグ取得
        ''' </summary>
        ''' <param name="inDealerCode">販売店コード</param>
        ''' <param name="inVin">VIN</param>
        ''' <param name="inRegisterNumber">登録番号</param>
        ''' <returns>SSC対象フラグ</returns>
        ''' <remarks></remarks>
        Public Function GetSscFlg(ByVal inDealerCode As String, _
                                  ByVal inVin As String, _
                                  ByVal inRegisterNumber As String) As String

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} START P1:{2} P2:{3} P3:{4} " _
                , Me.GetType.ToString _
                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                , inDealerCode _
                , inVin _
                , inRegisterNumber))

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3080225SscFlgDataTable)("SC3080225_009")

                Dim sql As New StringBuilder

                ' SQL文の作成
                sql.AppendLine("SELECT /* SC3080225_009 */ ")
                sql.AppendLine("         S1.SPECIAL_CAMPAIGN_TGT_FLG AS SSC_MARK ")
                sql.AppendLine("FROM ")
                sql.AppendLine("( ")
                sql.AppendLine("    SELECT ")
                sql.AppendLine("             T1.SPECIAL_CAMPAIGN_TGT_FLG ")
                sql.AppendLine("    FROM ")
                sql.AppendLine("             TB_M_VEHICLE T1 ")
                sql.AppendLine("            ,TB_M_CUSTOMER_VCL T2 ")
                sql.AppendLine("            ,TB_M_VEHICLE_DLR T3 ")
                sql.AppendLine("            ,TB_M_CUSTOMER_DLR T4 ")
                sql.AppendLine("            ,TB_M_CUSTOMER T5 ")
                sql.AppendLine("    WHERE ")
                sql.AppendLine("                 T1.VCL_ID = T2.VCL_ID ")
                sql.AppendLine("             AND T2.DLR_CD = T3.DLR_CD ")
                sql.AppendLine("             AND T1.VCL_ID = T3.VCL_ID ")
                sql.AppendLine("             AND T2.DLR_CD = T4.DLR_CD ")
                sql.AppendLine("             AND T2.CST_ID = T4.CST_ID ")
                sql.AppendLine("             AND T2.CST_ID = T5.CST_ID ")
                sql.AppendLine("             AND T2.DLR_CD = :DLR_CD ")
                sql.AppendLine("             AND T2.OWNER_CHG_FLG = :OWNER_CHG_FLG_0 ")
                sql.AppendLine("             AND ( ")
                sql.AppendLine("                     (T1.VCL_VIN_SEARCH = :VCL_VIN_UPPER) ")
                sql.AppendLine("                     OR ")
                sql.AppendLine("                     ( ")
                sql.AppendLine("                             (T1.VCL_VIN_SEARCH = :VCL_VIN_UPPER) ")
                sql.AppendLine("                         AND (T3.REG_NUM_SEARCH = :SPACE) ")
                sql.AppendLine("                     ) ")
                sql.AppendLine("                     OR ")
                sql.AppendLine("                     ( ")
                sql.AppendLine("                             (T1.VCL_VIN_SEARCH = :SPACE) ")
                sql.AppendLine("                         AND (T3.REG_NUM_SEARCH = :REG_NUM_UPPER) ")
                sql.AppendLine("                     ) ")
                sql.AppendLine("                 ) ")
                sql.AppendLine("    ORDER BY ")
                sql.AppendLine("         T5.DMS_TAKEIN_DATETIME DESC ")
                sql.AppendLine("        ,T4.CST_TYPE ASC ")
                sql.AppendLine("        ,T3.REG_NUM DESC ")
                sql.AppendLine("        ,T1.VCL_VIN DESC ")
                sql.AppendLine("        ,T1.VCL_ID DESC ")
                sql.AppendLine(") S1 ")
                sql.AppendLine("WHERE ")
                sql.AppendLine("    ROWNUM <= :ROWNUM_1 ")


                query.CommandText = sql.ToString()

                ' バインド変数定義
                '販売店コード
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, inDealerCode)
                'オーナーチェンジフラグ
                query.AddParameterWithTypeValue("OWNER_CHG_FLG_0", OracleDbType.NVarchar2, OWNER_CHG_FLG_0)
                'VIN(UPPER)
                query.AddParameterWithTypeValue("VCL_VIN_UPPER", OracleDbType.NVarchar2, inVin.ToUpper(CultureInfo.CurrentCulture))

                'SPACE
                query.AddParameterWithTypeValue("SPACE", OracleDbType.NVarchar2, Strings.Space(1))
                '車両登録番号(UPPER)
                query.AddParameterWithTypeValue("REG_NUM_UPPER", OracleDbType.NVarchar2, inRegisterNumber.ToUpper(CultureInfo.CurrentCulture))
                '取得行数
                query.AddParameterWithTypeValue("ROWNUM_1", OracleDbType.Int16, ROWNUM_1)

                'SQLの実行
                Using dt As SC3080225SscFlgDataTable = query.GetData()
                    Dim resSscFlag As String = String.Empty
                    If dt.Count > 0 Then
                        resSscFlag = dt(0).SSC_MARK
                    End If

                    ''終了ログの出力
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:SSCFLAG = {2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , resSscFlag))

                    Return resSscFlag
                End Using
            End Using
        End Function
        '2018/02/27 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 END

        '2018/07/03 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示START

        Public Function GetVehicleFlg(ByVal inDealerCode As String, _
                                      ByVal inVin As String, _
                                      ByVal inRegisterNumber As String) As SC3080225VehicleFlgDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} START P1:{2} P2:{3} P3:{4} " _
                , Me.GetType.ToString _
                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                , inDealerCode _
                , inVin _
                , inRegisterNumber))

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3080225VehicleFlgDataTable)("SC3080225_010")

                Dim sql As New StringBuilder

                sql.AppendLine("SELECT /* SC3080225_010 */ ")
                sql.AppendLine("          S1.IMP_VCL_FLG ")
                sql.AppendLine("        , S1.SML_AMC_FLG ")
                sql.AppendLine("        , S1.EW_FLG ")
                sql.AppendLine("        , S1.TLM_MBR_FLG ")
                sql.AppendLine("FROM")
                sql.AppendLine("( ")
                sql.AppendLine("    SELECT ")
                sql.AppendLine("              NVL(TRIM(T3.IMP_VCL_FLG), :ICON_FLAG_OFF) AS IMP_VCL_FLG ")
                sql.AppendLine("            , NVL(TRIM(T6.SML_AMC_FLG), :ICON_FLAG_OFF) AS SML_AMC_FLG ")
                sql.AppendLine("            , NVL(TRIM(T6.EW_FLG), :ICON_FLAG_OFF) AS EW_FLG ")
                sql.AppendLine("            , CASE ")
                sql.AppendLine("                    WHEN T7.VCL_VIN IS NOT NULL THEN :ICON_FLAG_ON ")
                sql.AppendLine("                    ELSE :ICON_FLAG_OFF ")
                sql.AppendLine("              END AS TLM_MBR_FLG ")
                sql.AppendLine("    FROM ")
                sql.AppendLine("             TB_M_VEHICLE T1 ")
                sql.AppendLine("           , TB_M_CUSTOMER_VCL T2 ")
                sql.AppendLine("           , TB_M_VEHICLE_DLR T3 ")
                sql.AppendLine("           , TB_M_CUSTOMER_DLR T4 ")
                sql.AppendLine("           , TB_M_CUSTOMER T5 ")
                sql.AppendLine("           , TB_LM_VEHICLE T6 ")
                sql.AppendLine("           , TB_LM_TLM_MEMBER T7 ")
                sql.AppendLine("    WHERE ")
                sql.AppendLine("                T1.VCL_ID = T2.VCL_ID ")
                sql.AppendLine("            AND T2.DLR_CD = T3.DLR_CD ")
                sql.AppendLine("            AND T1.VCL_ID = T3.VCL_ID ")
                sql.AppendLine("            AND T2.DLR_CD = T4.DLR_CD ")
                sql.AppendLine("            AND T2.CST_ID = T4.CST_ID ")
                sql.AppendLine("            AND T2.CST_ID = T5.CST_ID ")
                sql.AppendLine("            AND T2.VCL_ID = T6.VCL_ID(+) ")
                sql.AppendLine("            AND T1.VCL_VIN = T7.VCL_VIN(+) ")
                sql.AppendLine("            AND T2.DLR_CD = :DLRCD ")
                sql.AppendLine("            AND T2.OWNER_CHG_FLG = :OWNER_CHG_FLG_0 ")
                sql.AppendLine("            AND ( ")
                sql.AppendLine("                     (T1.VCL_VIN_SEARCH = :VCL_VIN_UPPER) ")
                sql.AppendLine("                     OR ")
                sql.AppendLine("                     ( ")
                sql.AppendLine("                             (T1.VCL_VIN_SEARCH = :VCL_VIN_UPPER) ")
                sql.AppendLine("                         AND (T3.REG_NUM_SEARCH = :SPACE) ")
                sql.AppendLine("                     ) ")
                sql.AppendLine("                     OR ")
                sql.AppendLine("                     ( ")
                sql.AppendLine("                             (T1.VCL_VIN_SEARCH = :SPACE) ")
                sql.AppendLine("                         AND (T3.REG_NUM_SEARCH = :REG_NUM_UPPER) ")
                sql.AppendLine("                     ) ")
                sql.AppendLine("                 ) ")
                sql.AppendLine("    ORDER BY ")
                sql.AppendLine("         T5.DMS_TAKEIN_DATETIME DESC ")
                sql.AppendLine("        ,T4.CST_TYPE ASC ")
                sql.AppendLine("        ,T3.REG_NUM DESC ")
                sql.AppendLine("        ,T1.VCL_VIN DESC ")
                sql.AppendLine("        ,T1.VCL_ID DESC ")
                sql.AppendLine(") S1 ")
                sql.AppendLine("WHERE ")
                sql.AppendLine("    ROWNUM <= :ROWNUM_1 ")

                query.CommandText = sql.ToString()

                'バインド変数
                '販売店コード
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, inDealerCode)
                'アイコン非表示フラグ
                query.AddParameterWithTypeValue("ICON_FLAG_OFF", OracleDbType.NVarchar2, IconFlagOff)
                'アイコン表示フラグ
                query.AddParameterWithTypeValue("ICON_FLAG_ON", OracleDbType.NVarchar2, IconFlagOn)
                'オーナーチェンジフラグ
                query.AddParameterWithTypeValue("OWNER_CHG_FLG_0", OracleDbType.NVarchar2, OWNER_CHG_FLG_0)
                'VIN(UPPER)
                query.AddParameterWithTypeValue("VCL_VIN_UPPER", OracleDbType.NVarchar2, inVin.ToUpper(CultureInfo.CurrentCulture))
                'SPACE
                query.AddParameterWithTypeValue("SPACE", OracleDbType.NVarchar2, Strings.Space(1))
                '車両登録番号(UPPER)
                query.AddParameterWithTypeValue("REG_NUM_UPPER", OracleDbType.NVarchar2, inRegisterNumber.ToUpper(CultureInfo.CurrentCulture))
                '取得行数
                query.AddParameterWithTypeValue("ROWNUM_1", OracleDbType.Int16, ROWNUM_1)

                'SQL実行
                Dim dt As SC3080225VehicleFlgDataTable = query.GetData()

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END RETURN:COUNT={2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , dt.Count))

                ' 検索結果の返却
                Return dt

            End Using
        End Function
        '2018/07/03 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示END

        '2018/07/23 NSK 坂本 TKM Next Gen e-CRB Project Application development Block B-1  顧客タイプ分類 START
        ''' <summary>
        ''' SC3080225_011:個人法人項目マスタ文言No取得
        ''' </summary>
        ''' <param name="inSubCustomerType">個人法人項目コード</param>
        ''' <returns>個人法人項目マスタ文言No</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' </history>
        Public Function GetCustomerMarkType(ByVal inSubCustomerType As String) As SC3080225CustomerJoinTypeDataTable
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} START P1:{2} " _
                , Me.GetType.ToString _
                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                , inSubCustomerType))

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3080225CustomerJoinTypeDataTable)("SC3080225_011")

                Dim sql As New StringBuilder

                ' SQL文の作成
                sql.AppendLine("SELECT /* SC3080225_011 */ ")
                sql.AppendLine("       CST_JOIN_TYPE ")
                sql.AppendLine("  FROM ")
                sql.AppendLine("       TB_LM_PRIVATE_FLEET_ITEM T1 ")
                sql.AppendLine(" WHERE ")
                sql.AppendLine("       T1.PRIVATE_FLEET_ITEM_CD = :PRIVATE_FLEET_ITEM_CD ")

                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("PRIVATE_FLEET_ITEM_CD", OracleDbType.NVarchar2, inSubCustomerType)

                'SQL実行
                Dim dt As SC3080225CustomerJoinTypeDataTable = query.GetData()

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END RETURN:COUNT={2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , dt.Count))

                ' 検索結果の返却
                Return dt

            End Using

        End Function
        '2018/07/23 NSK 坂本 TKM Next Gen e-CRB Project Application development Block B-1  顧客タイプ分類 END


#End Region

#Region "UPDATE"

        ''' <summary>
        ''' SC3080225_101:顧客写真パス登録処理
        ''' </summary>
        ''' <param name="inDealerCode">モデルコード</param>
        ''' <returns>モデルロゴ情報</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' </history>
        Public Function RegisterCustomerPhoto(ByVal inDealerCode As String, _
                                              ByVal inCustomerId As Decimal, _
                                              ByVal inCustomerPhotoPathLarge As String, _
                                              ByVal inCustomerPhotoPathMiddle As String, _
                                              ByVal inCustomerPhotoPathSmall As String, _
                                              ByVal inNowDate As Date, _
                                              ByVal inAccount As String) As Long
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} START P1:{2} P2:{3} P3:{4} P4:{5} P5:{6} P6:{7} P7:{8} " _
                , Me.GetType.ToString _
                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                , inDealerCode _
                , inCustomerId.ToString(CultureInfo.CurrentCulture) _
                , inCustomerPhotoPathLarge _
                , inCustomerPhotoPathMiddle _
                , inCustomerPhotoPathSmall _
                , inNowDate.ToString(CultureInfo.CurrentCulture) _
                , inAccount))

            ' DBSelectQueryインスタンス生成
            Using query As New DBUpdateQuery("SC3080225_101")

                Dim sql As New StringBuilder

                ' SQL文の作成
                sql.Append("UPDATE /* SC3080225_101 */")
                sql.Append("       TB_M_CUSTOMER_DLR ")
                sql.Append("   SET IMG_FILE_LARGE = :IMAGEFILE_L")
                sql.Append("      ,IMG_FILE_MEDIUM = :IMAGEFILE_M")
                sql.Append("      ,IMG_FILE_SMALL = :IMAGEFILE_S")
                sql.Append("      ,ROW_UPDATE_DATETIME = :UPDATEDATE")
                sql.Append("      ,ROW_UPDATE_ACCOUNT = :UPDATEACCOUNT")
                sql.Append("      ,ROW_UPDATE_FUNCTION = :UPDATEFUNCTION")
                sql.Append("      ,ROW_LOCK_VERSION = ROW_LOCK_VERSION + 1")
                sql.Append(" WHERE DLR_CD = :DLRCD   ")
                sql.Append("   AND CST_ID = :CST_ID ")

                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("IMAGEFILE_L", OracleDbType.NVarchar2, inCustomerPhotoPathLarge)
                query.AddParameterWithTypeValue("IMAGEFILE_M", OracleDbType.NVarchar2, inCustomerPhotoPathMiddle)
                query.AddParameterWithTypeValue("IMAGEFILE_S", OracleDbType.NVarchar2, inCustomerPhotoPathSmall)
                query.AddParameterWithTypeValue("UPDATEDATE", OracleDbType.Date, inNowDate)
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.NVarchar2, inAccount)
                query.AddParameterWithTypeValue("UPDATEFUNCTION", OracleDbType.NVarchar2, ProgramId)
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, inDealerCode)
                query.AddParameterWithTypeValue("CST_ID", OracleDbType.Decimal, inCustomerId)

                'SQL実行
                Dim count As Long = query.Execute

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END RETURN:COUNT={2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , count))

                ' 検索結果の返却
                Return count

            End Using

        End Function

#End Region

#Region "IN句作成"

        '2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証 START
        ''' <summary>
        ''' SQLのIN句作成
        ''' </summary>
        ''' <param name="valueList">検索条件のリスト</param>
        ''' <param name="rowName">列名</param>
        ''' <returns>IN句部分の文字列</returns>
        ''' <remarks></remarks>
        Private Function GetInQuery(ByVal valueList As List(Of String), ByVal rowName As String) As String
            'IN句用文字列生成
            Dim sbIn As New StringBuilder
            If InQueryMax < valueList.Count Then
                '1000件超過の場合は全体を括弧に入れる
                sbIn.Append("(")
            End If
            sbIn.Append(rowName)
            sbIn.Append(" IN ( ")
            Dim count As Integer = 0
            For Each val As String In valueList
                If InQueryMax <= count Then
                    'IN句内の値が1000個に達したら別のIN句をORで連結する
                    '末尾のカンマを削除
                    sbIn.Length -= 1
                    sbIn.Append(")")
                    sbIn.Append(" OR ")
                    sbIn.Append(rowName)
                    sbIn.Append(" IN (")
                    count = 0
                End If
                sbIn.Append(String.Format(CultureInfo.CurrentCulture, " '{0}' ,", val.Replace("'", "''")))
                count += 1
            Next
            '末尾のカンマを削除
            sbIn.Length -= 1
            'IN句の括弧閉じる
            sbIn.Append(") ")
            If InQueryMax < valueList.Count Then
                '1000件超過の場合は全体を括弧に入れる
                sbIn.Append(")")
            End If

            Return sbIn.ToString()
        End Function

        '2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証 END

#End Region

    End Class

End Namespace