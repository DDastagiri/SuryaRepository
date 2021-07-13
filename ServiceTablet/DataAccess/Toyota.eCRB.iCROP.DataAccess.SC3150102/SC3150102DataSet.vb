'------------------------------------------------------------------------------
'SC3150102DataSet.vb
'------------------------------------------------------------------------------
'機能：R/O情報タブ_データセット
'補足：
'作成：2013/02/21 TMEJ 成澤
'更新：2014/07/10 TMEJ 小澤 UAT不具合対応 入庫履歴SQLの修正
'更新：2014/07/23 TMEJ 成澤 IT9711_タブレットSMB Job Dispatch機能開発
'更新：2014/08/29 TMEJ 成澤【開発】IT9737_NextSTEPサービス ロケ管理の効率化に向けた評価用アプリ作成
'更新：2014/10/06 TMEJ 成澤【開発】IT9772_DMS連携版サービスタブレット RO管理追加開発
'更新：2014/12/17 TMEJ 小澤 IT9824_NextSTEPサービス 洗車用端末開発向けた評価用アプリ作成
'更新：2015/09/04 TMEJ 春日井【開発】IT9960_(トライ店システム評価)サービス入庫予約のユーザ管理機能開発
'更新：2016/02/03 NSK 中村【開発】（トライ店システム評価）コールセンター業務支援機能開発
'更新：2016/06/14 NSK 皆川 TR-SVT-TMT-20160525-001 TCメインスクリーンのレスポンスが遅い
'更新：2016/09/14 NSK 秋田谷 TR-SVT-TMT-20160727-002 顧客データ画面がサービス履歴を表示しない
'更新：2018/06/28 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示
'更新: 2018/11/19 NSK 坂本 17PRJ03047-06_TKM Next Gen e-CRB Project Test（Connectivity, SIT & UAT）Block D-1 ISSUE-0095 RO情報タブでPマークが表示されない
'------------------------------------------------------------------------------
Option Strict On
Option Explicit On

Imports System.Text
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Oracle.DataAccess.Client
Imports System.Globalization


Namespace SC3150102DataSetTableAdapters
    Public Class SC3150102StallInfoDataTableAdapter
        Inherits Global.System.ComponentModel.Component

#Region "定数"

        ''' <summary>
        ''' サービスステータス"13":納車済み
        ''' </summary>
        ''' <remarks></remarks>
        Private Const sarviceStatus13 As String = "13"
        ''' <summary>
        ''' 最小日付
        ''' </summary>
        ''' <remarks></remarks>
        Private Const MINDATE As String = "1900/01/01 00:00:00"
        ''' <summary>
        ''' 省略値
        ''' </summary>
        ''' <remarks></remarks>
        Private Const DEFAULT_VALUE As String = " "
        ''' <summary>
        ''' 省略値
        ''' </summary>
        ''' <remarks></remarks>
        Private Const DEFAULT_RO_SEQUENCE As Integer = -1
        ''' <summary>
        ''' ロウナンバー:1
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ROW_NUM As Integer = 1
        ''' <summary>
        ''' 着工指示フラグ:着工済み
        ''' </summary>
        ''' <remarks></remarks>
        Private Const STARTWORK_INSTRUCT_FLG As String = "1"
        ''' <summary>
        ''' 空文字
        ''' </summary>
        ''' <remarks></remarks>
        Private Const NUll_CHARACTER As String = " "
        ''' <summary>
        ''' キャンセルフラグ：有効
        ''' </summary>
        ''' <remarks></remarks>
        Private Const CANCEL_FLG As String = "0"
        ''' <summary>
        ''' RO連番
        ''' </summary>
        ''' <remarks></remarks>
        Private Const RO_SEQ As Integer = 0
        ''' <summary>
        ''' ROステータス：キャンセル
        ''' </summary>
        ''' <remarks></remarks>
        Private Const RO_STATUS_CANCEL As String = "99"

        '2014/07/10 TMEJ 小澤 UAT不具合対応 入庫履歴SQLの修正 START

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

        '2015/09/04 TMEJ 春日井【開発】IT9960_(トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START
        ''' <summary>
        ''' 所有者フラグ：4：保険
        ''' </summary>
        ''' <remarks></remarks>
        ''' 
        Private Const CST_VCL_TYPE_4 As String = "4"
        '2015/09/04 TMEJ 春日井【開発】IT9960_(トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END

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

        '2014/07/10 TMEJ 小澤 UAT不具合対応 入庫履歴SQLの修正 END

        '2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 START
        ''' <summary>
        ''' キャンセルフラグ
        ''' </summary>
        ''' <remarks></remarks>
        Private Const CANCEL_FLG_0 As String = "0"
        ''' <summary>
        '''  非稼動区分"1":休憩時間 
        ''' </summary>
        ''' <remarks></remarks>
        Private Const IDLE_TYPE_1 As String = "1"
        ''' <summary>
        '''  非稼動区分"2":使用不可
        ''' </summary>
        ''' <remarks></remarks>
        Private Const IDLE_TYPE_2 As String = "2"
        ''' <summary>
        '''  休憩区分"1"
        ''' </summary>
        ''' <remarks></remarks>
        Private Const BREAKKBN_1 As String = "1"
        ''' <summary>
        ''' オペレーションコード：チーフテクニシャン
        ''' </summary>
        ''' <remarks></remarks>
        Private Const OPERATIONCODE_CHIEF_TECHNICIAN As Integer = 62
        ''' <summary>
        ''' 業務権限フラグ（サービス）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ORGNZ_SA_FLG As String = "1"
        ''' <summary>
        ''' 使用中フラグ
        ''' </summary>
        ''' <remarks></remarks>
        Private Const INUSE_FLG As String = "1"
        '2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 END

        '2018/06/28 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
        ''' <summary>
        ''' アイコンのフラグ（1：表示）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const IconFlagOn = "1"
        ''' <summary>
        ''' アイコンのフラグ（2：非表示）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const IconFlagOff = "0"
        '2018/06/28 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END

#End Region

        ''' <summary>
        ''' RO情報の取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="repairOrderNumber">RO番号</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <History>
        ''' </History>
        Public Function GetROInfo(ByVal dealerCode As String, _
                                  ByVal branchCode As String, _
                                  ByVal repairOrderNumber As String) As SC3150102DataSet.SC3150102GetRoInfoDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} IN.DLR_CD:{2},RO_NUM:{3}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , dealerCode _
                   , repairOrderNumber))

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3150102DataSet.SC3150102GetRoInfoDataTable)("SC3150102_001")

                Dim sql As New StringBuilder

                ' SQL文の作成
                With sql
                    .AppendLine("SELECT /* SC3150102_001 */ ")
                    .AppendLine("       T6.RO_NUM AS RO_NUM ")
                    .AppendLine("     , T6.STF_NAME AS STF_NAME ")
                    .AppendLine("     , T7.RO_SEQ ")
                    .AppendLine("     , T7.CNT AS ADD_SVC_COUNT ")
                    .AppendLine("  FROM (")
                    .AppendLine("        SELECT T1.RO_NUM ")
                    .AppendLine("             , T2.STF_NAME ")
                    .AppendLine("             , T1.SVCIN_ID ")
                    .AppendLine("          FROM TB_T_SERVICEIN T1 ")
                    .AppendLine("             , TB_M_STAFF T2 ")
                    .AppendLine("         WHERE T1.PIC_SA_STF_CD = T2.STF_CD (+) ")
                    .AppendLine("           AND T1.DLR_CD = :DLR_CD ")
                    .AppendLine("           AND T1.BRN_CD = :BRN_CD ")
                    .AppendLine("           AND T1.RO_NUM = :RO_NUM ")
                    .AppendLine("        ) T6 ")
                    .AppendLine("      , ( ")
                    .AppendLine("        SELECT COUNT(1) OVER (PARTITION BY RO_NUM) AS CNT ")
                    .AppendLine("             , T5.SVCIN_ID ")
                    .AppendLine("             , T5.RO_SEQ ")
                    .AppendLine("          FROM TB_T_RO_INFO T5 ")
                    .AppendLine("         WHERE T5.RO_NUM = :RO_NUM ")
                    .AppendLine("           AND NOT T5.RO_STATUS = :RO_STATUS_CANCEL ")
                    '2014/10/06 TMEJ 成澤 【開発】IT9772_DMS連携版サービスタブレット RO管理追加開発 START
                    .AppendLine("           AND T5.DLR_CD = :DLR_CD ")
                    .AppendLine("           AND T5.BRN_CD = :BRN_CD ")
                    '2014/10/06 TMEJ 成澤 【開発】IT9772_DMS連携版サービスタブレット RO管理追加開発 START
                    .AppendLine("         ORDER BY RO_SEQ ASC) T7 ")
                    .AppendLine(" WHERE T6.SVCIN_ID = T7.SVCIN_ID ")
                End With

                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)  '販売店コード
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, branchCode)  '店舗コード
                query.AddParameterWithTypeValue("RO_NUM", OracleDbType.NVarchar2, repairOrderNumber)  'RO番号
                query.AddParameterWithTypeValue("RO_STATUS_CANCEL", OracleDbType.NVarchar2, RO_STATUS_CANCEL) 'ROステータス

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} OUT" _
                , Me.GetType.ToString _
                , System.Reflection.MethodBase.GetCurrentMethod.Name))

                ' 検索結果の返却
                Return query.GetData()

            End Using

        End Function

        ''' <summary>
        ''' 入庫履歴情報の取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="repairOrderNumber">基幹顧客ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' 2014/07/10 TMEJ 小澤 UAT不具合対応 入庫履歴SQLの修正
        ''' 2015/09/04 TMEJ 春日井【開発】IT9960_(トライ店システム評価)サービス入庫予約のユーザ管理機能開発
        ''' 2016/02/03 NSK 中村【開発】（トライ店システム評価）コールセンター業務支援機能開発
        ''' 2016/09/14 NSK 秋田谷 TR-SVT-TMT-20160727-002 顧客データ画面がサービス履歴を表示しない
        ''' </history>
        Public Function GetServiceInHistory(ByVal dealerCode As String, _
                                            ByVal branchCode As String, _
                                            ByVal repairOrderNumber As String, _
                                            ByVal getCount As Integer) As SC3150102DataSet.SC3150102GetServiceInHistoryDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1}  START DLR_CD:{2}, BNR_CD:{3},RO_NUM:{4} GETCOUNT:{5}. " _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , dealerCode _
                  , branchCode _
                  , repairOrderNumber _
                  , getCount))

            ' DBSelectQueryインスタンス生成

            Dim sql As New StringBuilder

            ' SQL文の作成
            With sql
                '2014/07/10 TMEJ 小澤 UAT不具合対応 入庫履歴SQLの修正 START
                '.AppendLine("SELECT /* SC3150102_002 */ ")
                '.AppendLine("       M1.DLR_CD ")
                '.AppendLine("     , M1.SVCIN_NUM ")
                '.AppendLine("     , M1.SVCIN_DELI_DATE ")
                '.AppendLine("     , M1.MAINTE_NAME ")
                '.AppendLine("     , M1.SVC_NAME_MILE ")
                '.AppendLine("     , M1.STF_NAME ")
                '.AppendLine("                FROM ")
                '.AppendLine("     ( SELECT T3.DLR_CD ")
                '.AppendLine("            , T3.SVCIN_NUM ")
                '.AppendLine("            , CASE ")
                '.AppendLine("                   WHEN T3.SVCIN_DELI_DATE = :MINDATE THEN NULL ")
                '.AppendLine("                   ELSE T3.SVCIN_DELI_DATE ")
                '.AppendLine("              END AS SVCIN_DELI_DATE ")
                '.AppendLine("            , TRIM(T5.MAINTE_NAME) AS MAINTE_NAME ")
                '.AppendLine("            , NVL(TRIM(T6.SVC_NAME_MILE), TRIM(T5.MAINTE_NAME)) AS SVC_NAME_MILE ")
                '.AppendLine("            , NVL(T7.STF_NAME, :DEFAULT_VALUE) AS STF_NAME ")
                '.AppendLine("         FROM TB_M_VEHICLE T1 ")
                '.AppendLine("            , TB_M_VEHICLE_DLR T2 ")
                '.AppendLine("            , TB_T_VEHICLE_SVCIN_HIS T3 ")
                '.AppendLine("            , TB_T_VEHICLE_MAINTE_HIS T4 ")
                '.AppendLine("            , TB_M_MAINTE T5 ")
                '.AppendLine("            , TB_M_SERVICE T6 ")
                '.AppendLine("            , TB_M_STAFF T7 ")
                '.AppendLine("        WHERE T1.VCL_ID = T2.VCL_ID ")
                '.AppendLine("          AND T2.DLR_CD = T3.DLR_CD ")
                '.AppendLine("          AND T2.VCL_ID = T3.VCL_ID ")
                '.AppendLine("          AND T3.DLR_CD = T4.DLR_CD ")
                '.AppendLine("          AND T1.VCL_ID = T3.VCL_ID ")
                '.AppendLine("          AND T3.SVCIN_NUM = T4.SVCIN_NUM ")
                '.AppendLine("          AND T4.DLR_CD = T5.DLR_CD ")
                '.AppendLine("          AND T4.MAINTE_CD = T5.MAINTE_CD ")
                '.AppendLine("          AND T5.MAINTE_KATASHIKI = SUBSTR(T1.VCL_KATASHIKI, 1, INSTR(T1.VCL_KATASHIKI, '-')-1) ")
                '.AppendLine("          AND T3.DLR_CD = T6.DLR_CD(+) ")
                '.AppendLine("          AND T3.SVC_CD = T6.SVC_CD(+) ")
                '.AppendLine("          AND T3.PIC_STF_CD = T7.STF_CD(+) ")
                ''初期表示時の場合、販売店で絞る
                'If getCount = 0 Then
                '    .AppendLine("          AND T2.DLR_CD = :DLR_CD ")
                '    .AppendLine("          AND T3.DLR_CD = :DLR_CD ")
                'End If
                '.AppendLine("          AND T1.VCL_ID = ( SELECT VCL_ID ")
                '.AppendLine("                              FROM TB_T_SERVICEIN ")
                '.AppendLine("                             WHERE RO_NUM = :RO_NUM ")
                ''初期表示時の場合、販売店で絞る
                'If getCount = 0 Then
                '    .AppendLine("                               AND DLR_CD = :DLR_CD ")
                '    .AppendLine("                               AND BRN_CD = :BRN_CD  ")
                'End If
                '.AppendLine("                           ) ")
                '.AppendLine("          ORDER BY T3.SVCIN_DELI_DATE DESC) M1 ")

                sql.AppendLine("SELECT /* SC3150102_002 */ ")
                sql.AppendLine("       M1.DLR_CD ")
                sql.AppendLine("      ,M1.SVCIN_NUM ")
                sql.AppendLine("      ,CASE ")
                sql.AppendLine("            WHEN M1.SVCIN_DELI_DATE = :MINDATE THEN NULL ")
                sql.AppendLine("            ELSE M1.SVCIN_DELI_DATE ")
                sql.AppendLine("       END AS SVCIN_DELI_DATE ")
                sql.AppendLine("      ,NVL(TRIM(M2.SVC_NAME_MILE), NVL(TRIM(M3.MAINTE_NAME), :DEFAULT_VALUE)) AS SVC_NAME_MILE ")
                sql.AppendLine("      ,NVL(TRIM(M3.MAINTE_NAME), :DEFAULT_VALUE) AS MAINTE_NAME ")
                sql.AppendLine("      ,M1.STF_NAME ")
                '2016/02/03 NSK 中村【開発】（トライ店システム評価）コールセンター業務支援機能開発 START
                sql.AppendLine("      ,NVL(TRIM(M2.MAINTE_NAME_HIS), :DEFAULT_VALUE) AS MAINTE_NAME_HIS  ")
                '2016/02/03 NSK 中村【開発】（トライ店システム評価）コールセンター業務支援機能開発 END
                sql.AppendLine("  FROM (SELECT Q2.DLR_CD ")
                sql.AppendLine("              ,Q2.SVCIN_NUM ")
                sql.AppendLine("              ,NVL(Q4.USERNAME, :DEFAULT_VALUE) AS STF_NAME ")
                sql.AppendLine("              ,Q5.REG_DATE AS SVCIN_DELI_DATE ")
                sql.AppendLine("              ,MAX(Q3.INSPEC_SEQ) AS INSPEC_SEQ_MAX ")
                sql.AppendLine("         FROM TB_M_CUSTOMER_VCL Q1 ")
                sql.AppendLine("             ,TB_T_VEHICLE_SVCIN_HIS Q2 ")
                sql.AppendLine("             ,TB_T_VEHICLE_MAINTE_HIS Q3 ")
                sql.AppendLine("             ,TBL_USERS Q4 ")
                sql.AppendLine("             ,TB_T_VEHICLE_MILEAGE Q5 ")
                sql.AppendLine("        WHERE Q1.VCL_ID = Q2.VCL_ID ")
                '2015/09/04 TMEJ 春日井【開発】IT9960_(トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START
                sql.AppendLine("          AND Q1.CST_ID = Q2.CST_ID")
                '2015/09/04 TMEJ 春日井【開発】IT9960_(トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END
                sql.AppendLine("          AND Q1.DLR_CD = Q2.DLR_CD ")
                sql.AppendLine("          AND Q2.DLR_CD = Q3.DLR_CD(+) ")
                sql.AppendLine("          AND Q2.SVCIN_NUM = Q3.SVCIN_NUM(+) ")
                sql.AppendLine("          AND Q2.VCL_MILE_ID = Q5.VCL_MILE_ID ")
                sql.AppendLine("          AND Q2.PIC_STF_CD = Q4.ACCOUNT(+) ")

                '初期表示時の場合、販売店で絞る
                If getCount = 0 Then
                    sql.AppendLine("          AND Q2.DLR_CD = :DLR_CD ")

                End If

                '2016/09/14 NSK 秋田谷 【開発】顧客データ画面がサービス履歴を表示しない START'
                'オーナーチェンジフラグの条件は削除
                'sql.AppendLine("          AND Q1.OWNER_CHG_FLG = :OWNER_CHG_FLG_0 ")
                '2016/09/14 NSK 秋田谷 【開発】顧客データ画面がサービス履歴を表示しない END'

                '2015/09/04 TMEJ 春日井【開発】IT9960_(トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START
                'sql.AppendLine("          AND Q1.CST_VCL_TYPE = :CST_VCL_TYPE_1 ")
                sql.AppendLine("          AND Q1.CST_VCL_TYPE <> :CST_VCL_TYPE_4 ")
                '2015/09/04 TMEJ 春日井【開発】IT9960_(トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END
                sql.AppendLine("          AND Q5.REG_MTD = :REG_MTD_1 ")
                sql.AppendLine("          AND Q4.DELFLG(+) = :DELFLG_0 ")
                sql.AppendLine("          AND Q2.VCL_ID = (SELECT VCL_ID ")
                sql.AppendLine("                             FROM TB_T_SERVICEIN ")
                sql.AppendLine("                            WHERE RO_NUM = :RO_NUM ")
                sql.AppendLine("                              AND DLR_CD = :DLR_CD ")
                sql.AppendLine("                              AND BRN_CD = :BRN_CD) ")
                sql.AppendLine("        GROUP BY Q2.DLR_CD ")
                sql.AppendLine("                ,Q2.SVCIN_NUM ")
                sql.AppendLine("                ,NVL(Q4.USERNAME, :DEFAULT_VALUE) ")
                sql.AppendLine("                ,Q5.REG_DATE) M1 ")
                sql.AppendLine("      ,(SELECT W1.DLR_CD ")
                sql.AppendLine("              ,W1.SVCIN_NUM ")
                sql.AppendLine("              ,W2.INSPEC_SEQ ")
                '2016/02/03 NSK 中村【開発】（トライ店システム評価）コールセンター業務支援機能開発 START
                sql.AppendLine("              ,W2.MAINTE_NAME AS MAINTE_NAME_HIS ")
                '2016/02/03 NSK 中村【開発】（トライ店システム評価）コールセンター業務支援機能開発 END
                sql.AppendLine("              ,W3.SVC_NAME_MILE ")
                sql.AppendLine("          FROM TB_T_VEHICLE_SVCIN_HIS W1 ")
                sql.AppendLine("              ,TB_T_VEHICLE_MAINTE_HIS W2 ")
                sql.AppendLine("              ,TB_M_SERVICE W3 ")
                sql.AppendLine("         WHERE W1.DLR_CD = W2.DLR_CD ")
                sql.AppendLine("           AND W1.SVCIN_NUM = W2.SVCIN_NUM ")
                sql.AppendLine("           AND W1.DLR_CD = W3.DLR_CD(+) ")
                sql.AppendLine("           AND W1.SVC_CD = W3.SVC_CD(+) ")

                '初期表示時の場合、販売店で絞る
                If getCount = 0 Then
                    sql.AppendLine("           AND W1.DLR_CD = :DLR_CD ")

                End If

                sql.AppendLine("           AND W1.VCL_ID = (SELECT VCL_ID ")
                sql.AppendLine("                              FROM TB_T_SERVICEIN ")
                sql.AppendLine("                             WHERE RO_NUM = :RO_NUM ")
                sql.AppendLine("                               AND DLR_CD = :DLR_CD ")
                sql.AppendLine("                               AND BRN_CD = :BRN_CD)) M2 ")
                sql.AppendLine("      ,(SELECT E2.DLR_CD ")
                sql.AppendLine("              ,E2.SVCIN_NUM ")
                sql.AppendLine("              ,E3.INSPEC_SEQ ")
                sql.AppendLine("              ,TRIM(E4.MAINTE_NAME) AS MAINTE_NAME ")
                sql.AppendLine("          FROM TB_M_VEHICLE E1 ")
                sql.AppendLine("              ,TB_T_VEHICLE_SVCIN_HIS E2 ")
                sql.AppendLine("              ,TB_T_VEHICLE_MAINTE_HIS E3 ")
                sql.AppendLine("              ,TB_M_MAINTE E4 ")
                sql.AppendLine("         WHERE E1.VCL_ID = E2.VCL_ID ")
                sql.AppendLine("           AND E2.DLR_CD = E3.DLR_CD ")
                sql.AppendLine("           AND E2.SVCIN_NUM = E3.SVCIN_NUM ")
                sql.AppendLine("           AND E3.DLR_CD = E4.DLR_CD ")
                sql.AppendLine("           AND E3.MAINTE_CD = E4.MAINTE_CD ")
                sql.AppendLine("           AND E4.MAINTE_KATASHIKI = SUBSTR(E1.VCL_KATASHIKI, 1, INSTR(E1.VCL_KATASHIKI, '-') - 1) ")

                '初期表示時の場合、販売店で絞る
                If getCount = 0 Then
                    sql.AppendLine("           AND E2.DLR_CD = :DLR_CD ")

                End If

                sql.AppendLine("           AND E1.VCL_ID = (SELECT VCL_ID ")
                sql.AppendLine("                              FROM TB_T_SERVICEIN ")
                sql.AppendLine("                             WHERE RO_NUM = :RO_NUM ")
                sql.AppendLine("                               AND DLR_CD = :DLR_CD ")
                sql.AppendLine("                               AND BRN_CD = :BRN_CD)) M3 ")
                sql.AppendLine(" WHERE ")
                sql.AppendLine("       M1.DLR_CD = M2.DLR_CD(+) ")
                sql.AppendLine("   AND M1.SVCIN_NUM = M2.SVCIN_NUM(+) ")
                sql.AppendLine("   AND M1.INSPEC_SEQ_MAX = M2.INSPEC_SEQ(+) ")
                sql.AppendLine("   AND M1.DLR_CD = M3.DLR_CD(+) ")
                sql.AppendLine("   AND M1.SVCIN_NUM = M3.SVCIN_NUM(+) ")
                sql.AppendLine("   AND M1.INSPEC_SEQ_MAX = M3.INSPEC_SEQ(+) ")
                sql.AppendLine(" ORDER BY M1.SVCIN_DELI_DATE DESC ")

                '2014/07/10 TMEJ 小澤 UAT不具合対応 入庫履歴SQLの修正 END

            End With

            Using query As New DBSelectQuery(Of SC3150102DataSet.SC3150102GetServiceInHistoryDataTable)("SC3150102_002")

                query.CommandText = sql.ToString()
                
                ' バインド変数定義
                '2014/07/10 TMEJ 小澤 UAT不具合対応 入庫履歴SQLの修正 START
                'If getCount = 0 Then
                '    ' バインド変数定義
                '    query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)  '販売店コード
                '    query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, branchCode)  '店舗コード
                'End If

                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)  '販売店コード
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, branchCode)  '店舗コード

                '2016/09/14 NSK 秋田谷 【開発】顧客データ画面がサービス履歴を表示しない START'
                'オーナーチェンジフラグの条件は削除
                'query.AddParameterWithTypeValue("OWNER_CHG_FLG_0", OracleDbType.NVarchar2, OWNER_CHG_FLG_0)  'オーナーチェンジフラグ
                '2016/09/14 NSK 秋田谷 【開発】顧客データ画面がサービス履歴を表示しない END'

                '2015/09/04 TMEJ 春日井【開発】IT9960_(トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START
                'query.AddParameterWithTypeValue("CST_VCL_TYPE_1", OracleDbType.NVarchar2, CST_VCL_TYPE_1)  '所有者フラグ
                query.AddParameterWithTypeValue("CST_VCL_TYPE_4", OracleDbType.NVarchar2, CST_VCL_TYPE_4)  '保険フラグ
                '2015/09/04 TMEJ 春日井【開発】IT9960_(トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END
                query.AddParameterWithTypeValue("REG_MTD_1", OracleDbType.NVarchar2, REG_MTD_1)  '登録方法
                query.AddParameterWithTypeValue("DELFLG_0", OracleDbType.Char, DELFLG_0)  '削除フラグ
                '2014/07/10 TMEJ 小澤 UAT不具合対応 入庫履歴SQLの修正 END

                query.AddParameterWithTypeValue("RO_NUM", OracleDbType.NVarchar2, repairOrderNumber)  'RO番号
                query.AddParameterWithTypeValue("MINDATE", OracleDbType.Date, Date.Parse(MINDATE, CultureInfo.InvariantCulture)) '日付省略値
                query.AddParameterWithTypeValue("DEFAULT_VALUE", OracleDbType.NVarchar2, DEFAULT_VALUE)  '省略値

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                 , "{0}.{1} OUT" _
                 , Me.GetType.ToString _
                 , System.Reflection.MethodBase.GetCurrentMethod.Name))

                Return query.GetData()
            End Using
        End Function

        ''' <summary>
        ''' 整備詳細情報取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="repairOrderNumber">RO番号</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetOperationDetailInfo(ByVal dealerCode As String, _
                                               ByVal branchCode As String, _
                                               ByVal repairOrderNumber As String) As SC3150102DataSet.SC3150102OperationDetailInfoDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} IN,DLR_CD:{2},BRN_CD:{3},RO_NUM:{4}" _
                , Me.GetType.ToString _
                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                , dealerCode _
                , branchCode _
                , repairOrderNumber))

           
                Dim sql As New StringBuilder

                ' SQL文の作成
            With sql
                .AppendLine("SELECT /* SC3150102_003 */ ")
                .AppendLine("       T7.RO_NUM AS RO_NUM ")
                .AppendLine("     , T4.JOB_DTL_ID AS JOB_DTL_ID ")
                .AppendLine("     , T7.INSPECTION_STATUS AS INSPECTION_STATUS ")
                .AppendLine("     , T7.STALL_USE_STATUS AS STALL_USE_STATUS ")
                .AppendLine("     , T7.SCHE_START_DATETIME AS SCHE_START_DATETIME ")
                .AppendLine("     , T7.RSLT_START_DATETIME AS RSLT_START_DATETIME ")
                .AppendLine("     , T4.RO_SEQ AS RO_SEQ ")
                .AppendLine("     , T4.JOB_NAME AS JOB_NAME ")
                .AppendLine("     , T4.STD_WORKTIME AS STD_WORKTIME ")
                .AppendLine("     , T4.WORK_PRICE AS WORK_PRICE ")
                .AppendLine("     , T4.WORK_UNIT_PRICE AS WORK_UNIT_PRICE ")
                .AppendLine("     , T4.RO_NUM AS INST_RO_NUM ")
                .AppendLine("     , T4.STARTWORK_INSTRUCT_FLG AS STARTWORK_INSTRUCT_FLG ")
                .AppendLine("     , T4.JOB_STF_GROUP_NAME AS JOB_STF_GROUP_NAME ")
                .AppendLine("     , T5.STALL_NAME_SHORT AS STALL_NAME_SHORT ")
                '2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 START
                .AppendLine("     , T4.JOB_INSTRUCT_ID ")
                .AppendLine("     , T4.JOB_INSTRUCT_SEQ ")
                .AppendLine("     , T8.RSLT_START_DATETIME AS JOB_RSLT_START_DATETIME")
                .AppendLine("     , T8.RSLT_END_DATETIME AS JOB_RSLT_END_DATETIME")
                .AppendLine("     , T8.JOB_STATUS ")
                '2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 END
                .AppendLine("  FROM TB_T_JOB_INSTRUCT T4 ")
                .AppendLine("     , TB_M_STALL T5 ")
                .AppendLine("     , TB_T_RO_INFO T6 ")
                '2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 START
                .AppendLine("     , TB_T_JOB_RESULT T8 ")
                '2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 END
                .AppendLine("     , ( SELECT T1.SVCIN_ID ")
                .AppendLine("              , T1.RO_NUM ")
                .AppendLine("              , T2.JOB_DTL_ID ")
                .AppendLine("              , T3.STALL_USE_ID ")
                .AppendLine("              , T2.INSPECTION_STATUS ")
                .AppendLine("              , T3.STALL_USE_STATUS ")
                .AppendLine("              , T3.STALL_ID ")
                .AppendLine("              , T3.SCHE_START_DATETIME ")
                .AppendLine("              , T3.RSLT_START_DATETIME ")
                '2016/06/14 NSK 皆川 TR-SVT-TMT-20160525-001 TCメインスクリーンのレスポンスが遅い START
                '.AppendLine("              , ROW_NUMBER() OVER( ")
                '.AppendLine("                                  PARTITION BY T3.JOB_DTL_ID ")
                '.AppendLine("                                  ORDER BY T3.STALL_USE_ID DESC ")
                '.AppendLine("                                  ) AS RNUM  ")
                '2016/06/14 NSK 皆川 TR-SVT-TMT-20160525-001 TCメインスクリーンのレスポンスが遅い END
                .AppendLine("           FROM TB_T_SERVICEIN T1 ")
                .AppendLine("              , TB_T_JOB_DTL T2 ")
                .AppendLine("              , TB_T_STALL_USE T3 ")
                .AppendLine("          WHERE T1.SVCIN_ID = T2.SVCIN_ID ")
                .AppendLine("            AND T2.JOB_DTL_ID = T3.JOB_DTL_ID ")
                .AppendLine("            AND T1.DLR_CD = :DLR_CD ")
                .AppendLine("            AND T1.BRN_CD = :BRN_CD ")
                .AppendLine("            AND T1.RO_NUM = :RO_NUM ")
                .AppendLine("            AND T2.CANCEL_FLG = :CANCEL_FLG ")
                '2016/06/14 NSK 皆川 TR-SVT-TMT-20160525-001 TCメインスクリーンのレスポンスが遅い START
                .AppendLine("            AND EXISTS ( ")
                .AppendLine("              SELECT ")
                .AppendLine("                1 ")
                .AppendLine("              FROM ")
                .AppendLine("                TB_T_STALL_USE T9 ")
                .AppendLine("              WHERE T9.JOB_DTL_ID = T2.JOB_DTL_ID ")
                .AppendLine("              GROUP BY ")
                .AppendLine("                T9.JOB_DTL_ID ")
                .AppendLine("              HAVING ")
                .AppendLine("                T3.STALL_USE_ID = MAX(T9.STALL_USE_ID) ")
                .AppendLine("            ) ")
                '2016/06/14 NSK 皆川 TR-SVT-TMT-20160525-001 TCメインスクリーンのレスポンスが遅い END
                .AppendLine("       ) T7 ")
                '2016/06/14 NSK 皆川 TR-SVT-TMT-20160525-001 TCメインスクリーンのレスポンスが遅い START
                '.AppendLine(" WHERE T7.RO_NUM = T4.RO_NUM (+) ")
                '.AppendLine("   AND T7.JOB_DTL_ID = T4.JOB_DTL_ID ")
                '.AppendLine("   AND T4.RO_NUM = T6.RO_NUM (+) ")
                .AppendLine(" WHERE T7.JOB_DTL_ID = T4.JOB_DTL_ID ")
                .AppendLine("   AND T6.SVCIN_ID = T7.SVCIN_ID ")
                .AppendLine("   AND T6.RO_NUM = T7.RO_NUM ")
                '2016/06/14 NSK 皆川 TR-SVT-TMT-20160525-001 TCメインスクリーンのレスポンスが遅い END
                .AppendLine("   AND T4.RO_SEQ = T6.RO_SEQ   ")
                '2016/06/14 NSK 皆川 TR-SVT-TMT-20160525-001 TCメインスクリーンのレスポンスが遅い START
                ''2014/10/06【開発】 TMEJ 成澤 IT9772_DMS連携版サービスタブレット RO管理追加開発 START
                '.AppendLine("   AND T6.DLR_CD = :DLR_CD ")
                '.AppendLine("   AND T6.BRN_CD = :BRN_CD ")
                ''2014/10/06【開発】 TMEJ 成澤 IT9772_DMS連携版サービスタブレット RO管理追加開発 END
                '2016/06/14 NSK 皆川 TR-SVT-TMT-20160525-001 TCメインスクリーンのレスポンスが遅い END
                .AppendLine("   AND T7.STALL_ID = T5.STALL_ID (+) ")
                '2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 START
                .AppendLine("   AND T4.JOB_DTL_ID = T8.JOB_DTL_ID(+) ")
                .AppendLine("   AND T4.JOB_INSTRUCT_ID = T8.JOB_INSTRUCT_ID(+) ")
                .AppendLine("   AND T4.JOB_INSTRUCT_SEQ = T8.JOB_INSTRUCT_SEQ(+) ")
                .AppendLine("   AND (    T8.JOB_RSLT_ID = ( SELECT MAX(JOB_RSLT_ID) ")
                .AppendLine("                                 FROM TB_T_JOB_RESULT A ")
                .AppendLine("                                WHERE A.JOB_DTL_ID = T8.JOB_DTL_ID  ")
                .AppendLine("                                  AND A.JOB_INSTRUCT_ID = T8.JOB_INSTRUCT_ID ")
                .AppendLine("                                  AND A.JOB_INSTRUCT_SEQ = T8.JOB_INSTRUCT_SEQ ")
                .AppendLine("                             GROUP BY A.JOB_DTL_ID ")
                .AppendLine("                                    , A.JOB_INSTRUCT_ID ")
                .AppendLine("                                    , A.JOB_INSTRUCT_SEQ ) ")
                .AppendLine("         OR T8.JOB_RSLT_ID IS NULL ) ")
                '2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 END
                .AppendLine("   AND NOT T4.RO_SEQ = :RO_SEQ ")
                .AppendLine("   AND NOT T6.RO_STATUS = :RO_STATUS_CANCEL ")
                '2016/06/14 NSK 皆川 TR-SVT-TMT-20160525-001 TCメインスクリーンのレスポンスが遅い START
                '.AppendLine("   AND T7.RNUM = :ROW_NUM ")
                '2016/06/14 NSK 皆川 TR-SVT-TMT-20160525-001 TCメインスクリーンのレスポンスが遅い END
                .AppendLine("ORDER BY T4.RO_SEQ, T4.JOB_INSTRUCT_ID, T4.JOB_INSTRUCT_SEQ ")
            End With

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3150102DataSet.SC3150102OperationDetailInfoDataTable)("SC3150102_003")

                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)         '販売店コード
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, branchCode)         '店舗コード
                query.AddParameterWithTypeValue("RO_NUM", OracleDbType.NVarchar2, repairOrderNumber)  'RO番号
                query.AddParameterWithTypeValue("RO_SEQ", OracleDbType.Int32, DEFAULT_RO_SEQUENCE)    'RO作業連番
                query.AddParameterWithTypeValue("CANCEL_FLG", OracleDbType.NVarchar2, CANCEL_FLG)     'キャンセルフラグ
                query.AddParameterWithTypeValue("RO_STATUS_CANCEL", OracleDbType.NVarchar2, RO_STATUS_CANCEL) 'ROステータス
                '2016/06/14 NSK 皆川 TR-SVT-TMT-20160525-001 TCメインスクリーンのレスポンスが遅い START
                'query.AddParameterWithTypeValue("ROW_NUM", OracleDbType.Int32, ROW_NUM)
                '2016/06/14 NSK 皆川 TR-SVT-TMT-20160525-001 TCメインスクリーンのレスポンスが遅い END

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} OUT" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

                Return query.GetData()
            End Using
        End Function

        ''' <summary>
        ''' 顧客情報取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="repairOrderNumber">RO番号</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetCustomerInfo(ByVal dealerCode As String, _
                                        ByVal branchCode As String, _
                                        ByVal repairOrderNumber As String) As SC3150102DataSet.SC3150102CutomerInfoDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} IN,DLR_CD:{2},BRN_CD:{3},RO_NUM:{4}" _
                , Me.GetType.ToString _
                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                , dealerCode _
                , branchCode _
                , repairOrderNumber))

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3150102DataSet.SC3150102CutomerInfoDataTable)("SC3150102_004")



                Dim sql As New StringBuilder

                ' SQL文の作成
                With sql
                    .AppendLine("SELECT /* SC3150102_004 */ ")
                    .AppendLine("       T1.CONTACT_PERSON_NAME AS CONTACT_PERSON ")
                    .AppendLine("     , T2.CST_NAME AS CST_NAME ")
                    .AppendLine("     , T3.VCL_VIN AS VCL_VIN ")
                    .AppendLine("	  , T3.GRADE_NAME AS GRADE_NAME ")
                    .AppendLine("	  , T4.REG_NUM AS REG_NUM ")
                    .AppendLine("	  , T1.SVCIN_MILE AS SVCIN_MILE ")
                    .AppendLine("	  , T4.DELI_DATE AS DELI_DATE ")
                    .AppendLine("	  , NVL(TRIM(T5.MODEL_NAME), T3.NEWCST_MODEL_NAME) AS MODEL_NAME ")
                    .AppendLine("	  , T5.MAKER_CD AS MAKER_CD ")                    '2018/06/28 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
                    .AppendLine("     , NVL(TRIM(T4.IMP_VCL_FLG), :ICON_FLAG_OFF) AS IMP_VCL_FLG ")
                    .AppendLine("     , NVL(TRIM(T6.SML_AMC_FLG), :ICON_FLAG_OFF) AS SML_AMC_FLG ")
                    .AppendLine("     , NVL(TRIM(T6.EW_FLG), :ICON_FLAG_OFF) AS EW_FLG ")
                    .AppendLine("     , CASE ")
                    .AppendLine("           WHEN T7.VCL_VIN IS NOT NULL THEN :ICON_FLAG_ON ")
                    .AppendLine("           ELSE :ICON_FLAG_OFF ")
                    .AppendLine("       END AS TLM_MBR_FLG ")
                    '2018/06/28 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END
                    .AppendLine("  FROM TB_T_SERVICEIN T1 ")
                    .AppendLine("     , TB_M_CUSTOMER T2 ")
                    .AppendLine("	  , TB_M_VEHICLE T3 ")
                    .AppendLine("	  , TB_M_VEHICLE_DLR T4 ")
                    .AppendLine("	  , TB_M_MODEL T5 ")
                    '2018/06/28 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
                    .AppendLine("     , TB_LM_VEHICLE T6 ")
                    .AppendLine("     , TB_LM_TLM_MEMBER T7 ")
                    '2018/06/28 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END
                    .AppendLine(" WHERE T1.CST_ID = T2.CST_ID ")
                    .AppendLine("   AND T1.VCL_ID = T3.VCL_ID ")
                    .AppendLine("   AND T1.VCL_ID = T4.VCL_ID ")
                    '2018/11/19 NSK 坂本 17PRJ03047-06_TKM Next Gen e-CRB Project Test（Connectivity, SIT & UAT）Block D-1 ISSUE-0095 RO情報タブでPマークが表示されない START
                    .AppendLine("   AND T1.DLR_CD = T4.DLR_CD ")
                    '2018/11/19 NSK 坂本 17PRJ03047-06_TKM Next Gen e-CRB Project Test（Connectivity, SIT & UAT）Block D-1 ISSUE-0095 RO情報タブでPマークが表示されない END
                    .AppendLine("   AND T3.MODEL_CD = T5.MODEL_CD (+) ")
                    '2018/06/28 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
                    .AppendLine("   AND T1.VCL_ID = T6.VCL_ID (+) ")
                    .AppendLine("   AND T3.VCL_VIN = T7.VCL_VIN (+) ")
                    '2018/06/28 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END
                    .AppendLine("   AND T1.DLR_CD = :DLR_CD ")
                    .AppendLine("   AND T1.BRN_CD = :BRN_CD ")
                    .AppendLine("   AND T1.RO_NUM = :RO_NUM ")
                    '2018/11/19 NSK 坂本 17PRJ03047-06_TKM Next Gen e-CRB Project Test（Connectivity, SIT & UAT）Block D-1 ISSUE-0095 RO情報タブでPマークが表示されない START
                    .AppendLine("   AND T4.DLR_CD = :DLR_CD ")
                    '2018/11/19 NSK 坂本 17PRJ03047-06_TKM Next Gen e-CRB Project Test（Connectivity, SIT & UAT）Block D-1 ISSUE-0095 RO情報タブでPマークが表示されない END
                End With
                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)  '販売店コード
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, branchCode)  '店舗コード
                query.AddParameterWithTypeValue("RO_NUM", OracleDbType.NVarchar2, repairOrderNumber)  'RO番号
                '2018/06/28 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
                query.AddParameterWithTypeValue("ICON_FLAG_ON", OracleDbType.NVarchar2, IconFlagOn)
                query.AddParameterWithTypeValue("ICON_FLAG_OFF", OracleDbType.NVarchar2, IconFlagOff)
                '2018/06/28 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} OUT" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

                Return query.GetData()
            End Using
        End Function

        ''' <summary>
        ''' 画面連携URLの取得
        ''' </summary>
        ''' <param name="displayNumber">画面番号</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' </history>
        Public Function GetDisplayUrl(ByVal displayNumber As Integer) As SC3150102DataSet.SC3150102DisplayUrlDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
              , "{0}.{1}  START DISP_NUM:{2}. " _
              , Me.GetType.ToString _
              , System.Reflection.MethodBase.GetCurrentMethod.Name _
              , displayNumber))

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3150102DataSet.SC3150102DisplayUrlDataTable)("SC3150102_006")

                Dim sql As New StringBuilder

                ' SQL文の作成
                With sql
                    .AppendLine("SELECT /* SC3150102_006 */ ")
                    .AppendLine("       DMS_DISP_URL ")
                    .AppendLine("  FROM TB_M_DISP_RELATION ")
                    .AppendLine(" WHERE DMS_DISP_ID = :DMS_DISP_ID ")
                End With

                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("DMS_DISP_ID", OracleDbType.Int32, displayNumber)                  '販売店コード


                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} END" _
                , Me.GetType.ToString _
                , System.Reflection.MethodBase.GetCurrentMethod.Name))

                'SQL実行
                Return query.GetData()

            End Using

        End Function

        ''' <summary>
        ''' 画面連携の引数取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="jobDetailId">作業内容ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' </history>
        Public Function GetScreenLinkageInfo(ByVal dealerCode As String, _
                                             ByVal branchCode As String, _
                                             ByVal jobDetailId As Decimal) As SC3150102DataSet.SC3150102ScreenLinkageInfoDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1}  START DLR_CD:{2}, BNR_CD:{3}, JOB_DTL_ID:{4}. " _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , dealerCode _
                  , branchCode _
                  , jobDetailId))

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3150102DataSet.SC3150102ScreenLinkageInfoDataTable)("SC3150102_007")

                Dim sql As New StringBuilder

                ' SQL文の作成
                With sql
                    .AppendLine("SELECT /* SC3150102_007 */ ")
                    .AppendLine("       T2.DMS_JOB_DTL_ID AS DMS_JOB_DTL_ID ")
                    .AppendLine("     , T3.VISITSEQ AS VISITSEQ ")
                    .AppendLine("     , T3.VIN AS VIN ")
                    .AppendLine("     , T4.RO_SEQ AS RO_SEQ ")
                    '2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 START
                    .AppendLine("     , T1.RO_NUM AS RO_NUM  ")
                    .AppendLine("     , TRIM(T3.SACODE) AS SACODE ")
                    .AppendLine("     , TRIM(T3.VCLREGNO) AS VCLREGNO ")
                    .AppendLine("     , TRIM(T5.CST_NAME) AS CST_NAME ")
                    .AppendLine("     , NVL(TRIM(T5.DMS_CST_CD) , T3.DMSID) AS DMS_CST_CD ")
                    .AppendLine("     , T6.NAMETITLE_NAME  ")
                    .AppendLine("     , T6.POSITION_TYPE  ")
                    '2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 END
                    .AppendLine("  FROM TB_T_SERVICEIN T1 ")
                    .AppendLine("     , TB_T_JOB_DTL T2 ")
                    .AppendLine("     , TBL_SERVICE_VISIT_MANAGEMENT T3")
                    .AppendLine("     , (SELECT RO_SEQ ")
                    '2014/10/06 TMEJ 成澤 【開発】IT9772_DMS連携版サービスタブレット RO管理追加開発 START
                    .AppendLine("             , JOB_DTL_ID ")
                    '2014/10/06 TMEJ 成澤 【開発】IT9772_DMS連携版サービスタブレット RO管理追加開発 END
                    .AppendLine("             , ROW_NUMBER() OVER (PARTITION BY JOB_DTL_ID ")
                    .AppendLine("                                  ORDER BY RO_SEQ ASC ")
                    .AppendLine("               ) AS RNUM")
                    .AppendLine("          FROM TB_T_JOB_INSTRUCT ")
                    .AppendLine("         WHERE JOB_DTL_ID = :JOB_DTL_ID")
                    .AppendLine("           AND STARTWORK_INSTRUCT_FLG = :STARTWORK_INSTRUCT_FLG) T4")
                    '2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 START
                    .AppendLine("     , TB_M_CUSTOMER T5 ")
                    .AppendLine("     , TB_M_NAMETITLE T6 ")
                    '2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 START
                    .AppendLine(" WHERE T1.SVCIN_ID = T2.SVCIN_ID ")
                    .AppendLine("   AND T1.RO_NUM = T3.ORDERNO ")
                    '2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 START
                    .AppendLine("   AND T1.CST_ID = T5.CST_ID (+)  ")
                    .AppendLine("   AND T5.NAMETITLE_CD = T6.NAMETITLE_CD (+)  ")
                    '2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 END
                    '2014/10/06 TMEJ 成澤 【開発】IT9772_DMS連携版サービスタブレット RO管理追加開発 START
                    .AppendLine("   AND T2.JOB_DTL_ID = T4.JOB_DTL_ID ")
                    '2014/10/06 TMEJ 成澤 【開発】IT9772_DMS連携版サービスタブレット RO管理追加開発 END
                    .AppendLine("   AND T2.DLR_CD = :DLR_CD ")
                    .AppendLine("   AND T2.BRN_CD = :BRN_CD ")
                    .AppendLine("   AND T2.JOB_DTL_ID = :JOB_DTL_ID ")
                    .AppendLine("   AND T3.DLRCD = :DLR_CD ")
                    .AppendLine("   AND T3.STRCD = :BRN_CD ")
                    .AppendLine("   AND T4.RNUM = :ROW_NUM ")
                End With

                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)                             '販売店コード
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, branchCode)                             '店舗コード
                query.AddParameterWithTypeValue("JOB_DTL_ID", OracleDbType.Decimal, jobDetailId)                          '作業内容ID
                query.AddParameterWithTypeValue("STARTWORK_INSTRUCT_FLG", OracleDbType.NVarchar2, STARTWORK_INSTRUCT_FLG) '着工指示フラグ
                query.AddParameterWithTypeValue("ROW_NUM", OracleDbType.Int32, ROW_NUM)                                   'ロウナンバー

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} END" _
                , Me.GetType.ToString _
                , System.Reflection.MethodBase.GetCurrentMethod.Name))

                'SQL実行
                Return query.GetData()

            End Using

        End Function

        '2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 START

        ''' <summary>
        ''' 選択チップの作業日時情報を取得する
        ''' </summary>
        ''' <param name="stallUseId">ストール利用ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetSelectChipTimeInfo(ByVal stallUseId As Decimal) As SC3150102DataSet.SC3150102ChipDateTimeInfoDataTable

            Logger.Info("[S]GetSelectChipTimeInfo()")

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3150102DataSet.SC3150102ChipDateTimeInfoDataTable)("SC3150102_008")

                Dim sql As New StringBuilder

                ' SQL文の作成
                With sql
                    .AppendLine("SELECT /* SC3150102_008 */ ")
                    .AppendLine("       SCHE_START_DATETIME ")
                    .AppendLine("     , SCHE_END_DATETIME ")
                    .AppendLine("     , RSLT_START_DATETIME ")
                    .AppendLine("     , PRMS_END_DATETIME ")
                    .AppendLine("     , STALL_USE_STATUS ")
                    .AppendLine("  FROM TB_T_STALL_USE ")
                    .AppendLine(" WHERE  STALL_USE_ID = :STALL_USE_ID  ")
                End With

                query.CommandText = sql.ToString()

                ' バインド変数定義

                query.AddParameterWithTypeValue("STALL_USE_ID", OracleDbType.Decimal, stallUseId)

                Logger.Info("[E]GetSelectChipTimeInfo()")

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
        ''' <history>
        '''  2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 
        ''' </history>
        Public Function GetBreakChipInfo(ByVal dealerCode As String, _
                                         ByVal branchCode As String, _
                                         ByVal stallId As Decimal) As SC3150102DataSet.SC3150102BreakChipInfoDataTable

            Logger.Info("[S]GetBreakChipInfo()")

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3150102DataSet.SC3150102BreakChipInfoDataTable)("SC3150102_009")

                Dim sql As New StringBuilder

                ' SQL文の作成
                With sql
                    .Append("  SELECT /* SC3150102_009 */ ")
                    .Append("         STARTTIME ")
                    .Append("       , ENDTIME ")
                    .Append("    FROM TBL_STALLBREAK ")
                    .Append("   WHERE DLRCD = :DLRCD ")
                    .Append("     AND STRCD = :STRCD ")
                    .Append("     AND STALLID = :STALLID ")
                    .Append("     AND BREAKKBN = :BREAKKBN_1 ")
                    .Append("ORDER BY STARTTIME")
                End With

                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, branchCode)
                query.AddParameterWithTypeValue("STALLID", OracleDbType.Decimal, stallId)
                query.AddParameterWithTypeValue("BREAKKBN_1", OracleDbType.NVarchar2, BREAKKBN_1)
                Logger.Info("[E]GetBreakChipInfo()")

                ' 検索結果の返却
                Return query.GetData()

            End Using

        End Function

        ''' <summary>
        ''' 使用不可チップ情報の取得
        ''' </summary>
        ''' <param name="stallId">ストールID</param>
        ''' <param name="fromDate">稼働時間From</param>
        ''' <param name="toDate">稼働時間To</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' </history>
        Public Function GetUnavailableChipInfo(ByVal stallId As Decimal, _
                                               ByVal fromDate As Date, _
                                               ByVal toDate As Date) As SC3150102DataSet.SC3150102UnavailableChipInfoDataTable

            Logger.Info("[S]GetUnavailableChipInfo()")

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3150102DataSet.SC3150102UnavailableChipInfoDataTable)("SC3150102_010")

                Dim sql As New StringBuilder

                ' SQL文の作成
                With sql
                    .Append("SELECT /* SC3150102_010 */ ")
                    .Append("         IDLE_START_DATETIME AS STARTTIME  ")
                    .Append("       , IDLE_END_DATETIME AS ENDTIME ")
                    .Append("    FROM TB_M_STALL_IDLE  ")
                    .Append("   WHERE STALL_ID = :STALL_ID ")
                    .Append("     AND CANCEL_FLG = :CANCEL_FLG_0 ")
                    .Append("     AND IDLE_TYPE = :IDLE_TYPE_2 ")
                    .Append("     AND IDLE_START_DATETIME >= :STARTTIME ")
                    .Append("     AND IDLE_END_DATETIME < :ENDTIME ")
                    .Append("ORDER BY IDLE_START_DATETIME ")
                End With

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("STALL_ID", OracleDbType.Decimal, stallId) ' ストールID
                query.AddParameterWithTypeValue("CANCEL_FLG_0", OracleDbType.NVarchar2, CANCEL_FLG_0)
                query.AddParameterWithTypeValue("IDLE_TYPE_2", OracleDbType.NVarchar2, IDLE_TYPE_2)
                query.AddParameterWithTypeValue("STARTTIME", OracleDbType.Date, fromDate)                 ' 稼働時間Fromの日付部分＋00:00:00
                query.AddParameterWithTypeValue("ENDTIME", OracleDbType.Date, toDate) ' 稼働時間To

                Logger.Info("[E]GetUnavailableChipInfo()")

                ' 検索結果の返却
                Return query.GetData()

            End Using

        End Function

        ''' <summary>
        ''' 現ストール担当のChtアカウント取得
        ''' </summary>
        ''' <param name="stallId">ストールID</param>
        ''' <remarks></remarks>
        ''' <history>
        ''' </history>
        Public Function GetChtTechnicianAccount(ByVal stallId As Decimal) _
                                                As SC3150102DataSet.SC3150102ChtStaffCodeDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} STALL_ID:{2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , stallId))

            Using query As New DBSelectQuery(Of SC3150102DataSet.SC3150102ChtStaffCodeDataTable)("SC3150102_011")

                Dim sql As New StringBuilder      ' SQL文格納
                With sql
                    .AppendLine("SELECT /* SC3150102_011 */ ")
                    .AppendLine("       T4.STF_CD ")
                    .AppendLine("  FROM TB_M_STALL_STALL_GROUP T1 ")
                    .AppendLine("     , TB_M_STALL_GROUP T2 ")
                    .AppendLine("	  , TB_M_ORGANIZATION T3 ")
                    .AppendLine("     , TB_M_STAFF T4 ")
                    .AppendLine("	  , TBL_USERS T5 ")
                    .AppendLine(" WHERE T1.STALL_GROUP_ID = T2.STALL_GROUP_ID ")
                    .AppendLine("   AND T2.ORGNZ_ID = T3.ORGNZ_ID ")
                    .AppendLine("   AND T3.ORGNZ_ID = T4.ORGNZ_ID ")
                    .AppendLine("   AND T4.STF_CD = T5.ACCOUNT ")
                    .AppendLine("   AND T1.STALL_ID = :STALL_ID ")
                    .AppendLine("   AND T3.ORGNZ_SA_FLG = :ORGNZ_SA_FLG ")
                    .AppendLine("   AND T3.INUSE_FLG = :INUSE_FLG ")
                    .AppendLine("   AND T5.OPERATIONCODE = :OPERATIONCODE ")

                End With
                query.CommandText = sql.ToString()

                'バインド変数
                query.AddParameterWithTypeValue("STALL_ID", OracleDbType.Decimal, stallId)
                query.AddParameterWithTypeValue("OPERATIONCODE", OracleDbType.Int32, OPERATIONCODE_CHIEF_TECHNICIAN)
                query.AddParameterWithTypeValue("ORGNZ_SA_FLG", OracleDbType.NVarchar2, ORGNZ_SA_FLG)
                query.AddParameterWithTypeValue("INUSE_FLG", OracleDbType.NVarchar2, INUSE_FLG)

                '実行
                Dim dt As SC3150102DataSet.SC3150102ChtStaffCodeDataTable = query.GetData()

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} END COUNT = {2}" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                            , dt.Count))

                Return dt

            End Using
        End Function

        ''' <summary>
        ''' 最初の作業チップと担当SAコードの取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="jobDetailId">作業内容ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' </history>
        Public Function GetFirstWorkChip(ByVal dealerCode As String, _
                                         ByVal branchCode As String, _
                                         ByVal jobDetailId As Decimal) As SC3150102DataSet.SC3150102FirstWorkChipDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1}  START DLR_CD:{2}, BNR_CD:{3}, RO_NUM:{4}. " _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , dealerCode _
                  , branchCode _
                  , jobDetailId.ToString(CultureInfo.CurrentCulture)))

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3150102DataSet.SC3150102FirstWorkChipDataTable)("SC3150102_012")

                Dim sql As New StringBuilder

                ' SQL文の作成
                With sql
                    .AppendLine("SELECT /* SC3150102_012 */ ")
                    .AppendLine("       T6.PIC_SA_STF_CD ")
                    .AppendLine("     , T6.JOB_DTL_ID ")
                    .AppendLine("  FROM ( SELECT T1.PIC_SA_STF_CD ")
                    .AppendLine("              , T2.JOB_DTL_ID  ")
                    .AppendLine("              , ROW_NUMBER() OVER(  ")
                    .AppendLine("                                   PARTITION BY T1.SVCIN_ID ")
                    .AppendLine("                                   ORDER BY T3.SCHE_START_DATETIME ASC ")
                    .AppendLine("                                 ) AS RNUM ")
                    .AppendLine("           FROM TB_T_SERVICEIN T1 ")
                    .AppendLine("              , TB_T_JOB_DTL T2 ")
                    .AppendLine("              , TB_T_STALL_USE T3 ")
                    .AppendLine("          WHERE T1.SVCIN_ID = T2.SVCIN_ID ")
                    .AppendLine("            AND T2.JOB_DTL_ID = T3.JOB_DTL_ID ")
                    .AppendLine("            AND T1.DLR_CD = :DLR_CD ")
                    .AppendLine("            AND T1.BRN_CD = :BRN_CD  ")
                    .AppendLine("            AND T1.SVCIN_ID = ( SELECT T4.SVCIN_ID ")
                    .AppendLine("                                  FROM TB_T_SERVICEIN T4 ")
                    .AppendLine("                                     , TB_T_JOB_DTL T5 ")
                    .AppendLine("                                 WHERE T4.SVCIN_ID = T5.SVCIN_ID ")
                    .AppendLine("                                   AND T5.DLR_CD = :DLR_CD ")
                    .AppendLine("                                   AND T5.BRN_CD = :BRN_CD ")
                    .AppendLine("                                   AND T5.JOB_DTL_ID = :JOB_DTL_ID ) ")
                    .AppendLine("        ) T6 ")
                    .AppendLine(" WHERE T6.RNUM = :ROW_NUM ")

                End With

                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)                             '販売店コード
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, branchCode)                             '店舗コード
                query.AddParameterWithTypeValue("JOB_DTL_ID", OracleDbType.Decimal, jobDetailId)                          '作業内容ID
                query.AddParameterWithTypeValue("ROW_NUM", OracleDbType.Int32, ROW_NUM)                                   'ロウナンバー

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} END" _
                , Me.GetType.ToString _
                , System.Reflection.MethodBase.GetCurrentMethod.Name))

                'SQL実行
                Return query.GetData()

            End Using

        End Function

        ''' <summary>
        ''' 最後の作業チップと着工指示フラグのない整備数を取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="OrderRepiarNumber">RO番号</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' 2014/12/17 TMEJ 小澤 IT9824_NextSTEPサービス 洗車用端末開発向けた評価用アプリ作成
        ''' </history>
        Public Function GetLastWorkChip(ByVal dealerCode As String, _
                                        ByVal branchCode As String, _
                                        ByVal orderRepiarNumber As String) As SC3150102DataSet.SC3150102GetLastWorkChipDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1}  START DLR_CD:{2}, BNR_CD:{3}, RO_NUM:{4}. " _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , dealerCode _
                  , branchCode _
                  , orderRepiarNumber))

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3150102DataSet.SC3150102GetLastWorkChipDataTable)("SC3150102_013")

                Dim sql As New StringBuilder

                ' SQL文の作成
                With sql
                    .AppendLine("SELECT /* SC3150102_013 */ ")
                    .AppendLine("       T1.NO_FLG_COUNT ")
                    .AppendLine("     , T5.JOB_DTL_ID ")
                    .AppendLine("     , T5.PIC_SA_STF_CD ")

                    '2014/12/17 TMEJ 小澤 IT9824_NextSTEPサービス 洗車用端末開発向けた評価用アプリ作成 START

                    .AppendLine("     , T5.SVC_STATUS ")

                    '2014/12/17 TMEJ 小澤 IT9824_NextSTEPサービス 洗車用端末開発向けた評価用アプリ作成 END

                    .AppendLine("  FROM (SELECT COUNT(1) AS NO_FLG_COUNT ")
                    .AppendLine("          FROM TB_T_JOB_INSTRUCT T6 ")
                    .AppendLine("         WHERE T6.RO_NUM = :RO_NUM ")
                    .AppendLine("           AND NOT T6.STARTWORK_INSTRUCT_FLG = :STARTWORK_INSTRUCT_FLG ")
                    .AppendLine("        ) T1 ")
                    .AppendLine("     , (SELECT T3.JOB_DTL_ID ")
                    .AppendLine("             , T2.PIC_SA_STF_CD ")
                    .AppendLine("             , ROW_NUMBER() OVER (PARTITION BY T2.SVCIN_ID ")
                    .AppendLine("                                  ORDER BY T4.SCHE_END_DATETIME DESC ")
                    .AppendLine("                                ) AS RNUM ")

                    '2014/12/17 TMEJ 小澤 IT9824_NextSTEPサービス 洗車用端末開発向けた評価用アプリ作成 START

                    .AppendLine("             , T2.SVC_STATUS  ")

                    '2014/12/17 TMEJ 小澤 IT9824_NextSTEPサービス 洗車用端末開発向けた評価用アプリ作成 END

                    .AppendLine("         FROM TB_T_SERVICEIN T2 ")
                    .AppendLine("             , TB_T_JOB_DTL T3 ")
                    .AppendLine("             , TB_T_STALL_USE T4 ")
                    .AppendLine("        WHERE T2.SVCIN_ID = T3.SVCIN_ID (+) ")
                    .AppendLine("          AND T3.JOB_DTL_ID = T4.JOB_DTL_ID (+) ")
                    .AppendLine("          AND T2.DLR_CD = :DLR_CD ")
                    .AppendLine("         AND T3.BRN_CD = :BRN_CD ")
                    .AppendLine("         AND T2.RO_NUM = :RO_NUM ")
                    .AppendLine("        ) T5 ")
                    .AppendLine(" WHERE T5.RNUM = :ROW_NUM ")

                End With

                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)                             '販売店コード
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, branchCode)                             '店舗コード
                query.AddParameterWithTypeValue("RO_NUM", OracleDbType.NVarchar2, orderRepiarNumber)                      'RO番号
                query.AddParameterWithTypeValue("STARTWORK_INSTRUCT_FLG", OracleDbType.NVarchar2, STARTWORK_INSTRUCT_FLG) '着工指示フラグ
                query.AddParameterWithTypeValue("ROW_NUM", OracleDbType.Int32, ROW_NUM)                                   'ロウナンバー

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} END" _
                , Me.GetType.ToString _
                , System.Reflection.MethodBase.GetCurrentMethod.Name))

                'SQL実行
                Return query.GetData()

            End Using

        End Function

        '2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 END

        '2014/08/29 TMEJ 成澤 【開発】IT9737_NextSTEPサービス ロケ管理の効率化に向けた評価用アプリ作成　START
        ''' <summary>
        ''' かご番号情報取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="repairOrderNumber">RO番号</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetCageNoInfo(ByVal dealerCode As String, _
                                      ByVal branchCode As String, _
                                      ByVal repairOrderNumber As String) As SC3150102DataSet.SC3150102CageInfoDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                    , "{0}.{1}  START DLR_CD:{2}, BNR_CD:{3}, RO_NUM:{4}. " _
                                    , Me.GetType.ToString _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                    , dealerCode _
                                    , branchCode _
                                    , repairOrderNumber))

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3150102DataSet.SC3150102CageInfoDataTable)("SC3150102_014")

                Dim sql As New StringBuilder

                ' SQL文の作成
                With sql
                    .AppendLine("SELECT /* SC3150102_014 */ ")
                    .AppendLine("       CAGE_NO ")
                    .AppendLine("  FROM TB_T_CAGE_INFO ")
                    .AppendLine(" WHERE DLR_CD = :DLR_CD ")
                    .AppendLine("   AND BRN_CD = :BRN_CD ")
                    .AppendLine("   AND RO_NUM = :RO_NUM ")
                    .AppendLine(" ORDER BY RO_SEQ ASC ")
                End With

                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, branchCode)
                query.AddParameterWithTypeValue("RO_NUM", OracleDbType.NVarchar2, repairOrderNumber)

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                        , "{0}.{1} END" _
                                        , Me.GetType.ToString _
                                        , System.Reflection.MethodBase.GetCurrentMethod.Name))

                ' 検索結果の返却
                Return query.GetData()

            End Using

        End Function
        '2014/08/29 TMEJ 成澤 【開発】IT9737_NextSTEPサービス ロケ管理の効率化に向けた評価用アプリ作成　END

    End Class

End Namespace
Partial Class SC3150102DataSet
End Class
